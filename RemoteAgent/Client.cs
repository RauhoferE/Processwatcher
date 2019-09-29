//-----------------------------------------------------------------------
// <copyright file="Client.cs" company="FH Wiener Neustadt">
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
    using System.Net.Sockets;

    /// <summary>
    /// The <see cref="Client"/> class.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// The connected client.
        /// </summary>
        private TcpClient tcpClient;

        /// <summary>
        /// The current network stream.
        /// </summary>
        private NetworkStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="tcpClient"> The connected client. </param>
        public Client(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            this.Strikes = 0;
            this.IsConnectionClosed = false;
        }

        /// <summary>
        /// Gets or sets the timeout strikes for the client.
        /// </summary>
        /// <value> A normal integer. </value>
        public int Strikes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the connection is still up.
        /// </summary>
        /// <value> Is true if the connection is closed. </value>
        public bool IsConnectionClosed
        {
            get;
            private set;
        }

        /// <summary>
        /// This method closes the connection.
        /// </summary>
        public void CloseConnection()
        {
            this.stream.Close();
            this.tcpClient.Close();
            this.IsConnectionClosed = true;
        }

        /// <summary>
        /// This method sends the byte message to the client.
        /// </summary>
        /// <param name="message"> The byte message. </param>
        public void SendMessage(byte[] message)
        {
            if (this.tcpClient == null || this.tcpClient.GetStream() == null || !this.tcpClient.GetStream().CanWrite || !this.tcpClient.Connected)
            {
                throw new ArgumentException("Error");
            }
            else
            {
                this.stream = this.tcpClient.GetStream();
                this.stream.Write(message, 0, message.Length);
            }
        }
    }
}