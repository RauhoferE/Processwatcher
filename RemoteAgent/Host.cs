//-----------------------------------------------------------------------
// <copyright file="Host.cs" company="FH Wiener Neustadt">
//     Copyright (c) Emre Rauhofer. All rights reserved.
// </copyright>
// <author>Emre Rauhofer</author>
// <summary>
// This is a remote agent.
// </summary>
//-----------------------------------------------------------------------
namespace RemoteAgent
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// The <see cref="Host"/> class.
    /// </summary>
    public class Host
    {
        /// <summary>
        /// The listener for the clients.
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// The thread for accepting the clients.
        /// </summary>
        private Thread thread;

        /// <summary>
        /// The list of clients.
        /// </summary>
        private List<Client> clients;

        /// <summary>
        /// Initializes a new instance of the <see cref="Host"/> class.
        /// </summary>
        /// <param name="port"> The number of the port. </param>
        public Host(int port)
        {
            this.clients = new List<Client>();
            this.listener = new TcpListener(IPAddress.Any, port);
            this.IsRunning = false;
        }

        /// <summary>
        /// The event for when a new client has been connected.
        /// </summary>
        public event EventHandler OnClientConnected;

        /// <summary>
        /// Gets a value indicating whether the listener is running or not.
        /// </summary>
        /// <value> Is true if the host is running. </value>
        public bool IsRunning
        {
            get;
            private set;
        }

        /// <summary>
        /// This method starts the host and the thread.
        /// </summary>
        public void Start()
        {
            if (this.thread != null && this.thread.IsAlive)
            {
                throw new ArgumentException("Error thread is already running.");
            }

            this.listener.Start();
            this.IsRunning = true;
            this.thread = new Thread(this.AcceptClients);
            this.thread.Start();
        }

        /// <summary>
        /// This method sends the byte message to all the clients.
        /// </summary>
        /// <param name="message"> The byte message. </param>
        public void SendToClients(byte[] message)
         {
            foreach (var item in this.clients)
            {
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        item.SendMessage(message);
                        break;
                    }
                    catch (Exception)
                    {
                        item.Strikes++;
                    }
                }

                if (item.Strikes == 3)
                {
                    item.CloseConnection();
                }
            }

            this.RemoveClosedConnectionsFromList();
        }

        /// <summary>
        /// This method stops the host.
        /// </summary>
        public void Stop()
        {
            if (this.thread == null || !this.thread.IsAlive)
            {
                throw new ArgumentException("Error thread is already running.");
            }

            this.listener.Stop();
            this.IsRunning = false;
            this.thread.Join();

            foreach (var item in this.clients)
            {
                item.CloseConnection();
            }

            this.clients.Clear();
        }

        /// <summary>
        /// This method fires when a client has been connected.
        /// </summary>
        protected virtual void FireOnClientConnected()
        {
            if (this.OnClientConnected != null)
            {
                this.OnClientConnected(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// This method accepts the clients.
        /// </summary>
        private void AcceptClients()
        {
            while (this.IsRunning == true)
            {
                Client client = new Client(this.listener.AcceptTcpClient());
                this.clients.Add(client);
                this.FireOnClientConnected();
            }
        }

        /// <summary>
        /// This method removes the closed connections from the list.
        /// </summary>
        private void RemoveClosedConnectionsFromList()
        {
            List<Client> oldClients = new List<Client>();

            foreach (var item in this.clients)
            {
                oldClients.Add(item);
            }

            foreach (var item in oldClients)
            {
                if (item.IsConnectionClosed)
                {
                    this.clients.Remove(item);
                }
            }
        }
    }
}