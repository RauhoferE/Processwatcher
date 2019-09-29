//-----------------------------------------------------------------------
// <copyright file="Client.cs" company="FH Wiener Neustadt">
//     Copyright (c) Emre Rauhofer. All rights reserved.
// </copyright>
// <author>Emre Rauhofer</author>
// <summary>
// This is a dashboard.
// </summary>
//-----------------------------------------------------------------------
namespace ProcessWatcher.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Timers;
    using NetworkLibrary;

    /// <summary>
    /// The <see cref="Client"/> class.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// The thread for the accepting the messages of the client.
        /// </summary>
        private Thread thread;

        /// <summary>
        /// The current client.
        /// </summary>
        private TcpClient tcpClient;

        /// <summary>
        /// The message builder.
        /// </summary>
        private MessageBuilder messageBuilder;

        /// <summary>
        /// The current network stream.
        /// </summary>
        private NetworkStream stream;

        /// <summary>
        /// The timer for calculating the timeout.
        /// </summary>
        private System.Timers.Timer timeout;

        /// <summary>
        /// The clients machine name.
        /// </summary>
        private string hostName;

        /// <summary>
        /// A value indicating whether the client is still running.
        /// </summary>
        private bool isRunning;

        /// <summary>
        /// The IP endpoint.
        /// </summary>
        private IPEndPoint ipEndPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="endPoint"> The IP endpoint. </param>
        public Client(IPEndPoint endPoint)
        {
            this.tcpClient = new TcpClient();
            this.messageBuilder = new MessageBuilder();
            this.IPEndPoint = endPoint;
            this.timeout = new System.Timers.Timer(10000);
            this.messageBuilder.OnMessageCompleted += this.CheckCompletedMessage;
            this.OnMessageReceived += this.messageBuilder.BuildMessage;
            this.timeout.Elapsed += this.TryToReconnect;
            this.isRunning = false;
            this.HostName = string.Empty;
        }

        /// <summary>
        /// The event when a byte message has been received.
        /// </summary>
        public event EventHandler<ByteMessageEventArgs> OnMessageReceived;

        /// <summary>
        /// The event when the message has been completed by the builder.
        /// </summary>
        public event EventHandler<ProcessListEventArgs> OnMessageCompleted;

        /// <summary>
        /// The event when the clients machine name changes.
        /// </summary>
        public event EventHandler<StringEventArgs> OnHostNameChanged;

        /// <summary>
        /// The event when the client has been connected.
        /// </summary>
        public event EventHandler<BoolEventArgs> OnStateChanged;

        /// <summary>
        /// Gets the hostname of the client.
        /// </summary>
        /// <value> A normal string. </value>
        public string HostName
        {
            get
            {
                return this.hostName; 
            }

            private set
            {
                if (value != this.hostName)
                {
                    this.hostName = value;
                    this.FireOnHostNameChanged(new StringEventArgs(this.hostName));
                }
            }
        }

        /// <summary>
        /// Gets the timer for the timeout.
        /// </summary>
        /// <value> A normal system timer. </value>
        public System.Timers.Timer Timeout
        {
            get
            {
                return this.timeout;
            }

            private set
            {
                this.timeout = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the client is still running.
        /// </summary>
        /// <value> Is true if the client is still running. </value>
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }

            private set
            {
                this.isRunning = value;
                this.FireOnStateChanged(new BoolEventArgs(this.isRunning));
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IPEndPoint"/> of the client.
        /// </summary>
        /// <value> A normal endpoint. </value>
        public IPEndPoint IPEndPoint
        {
            get
            {
                return this.ipEndPoint;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Error value cant be null.");
                }

                this.ipEndPoint = value;
            }
        }

        /// <summary>
        /// This method starts the thread.
        /// </summary>
        public void Start()
        {
            if ((this.thread != null && this.thread.IsAlive) || this.IPEndPoint == null)
            {
                throw new ArgumentException("Error thread is already running.");
            }

            try
            {
                this.tcpClient.Connect(this.IPEndPoint);
            }
            catch (Exception)
            {
                throw new ArgumentException("Error cant connect.");
            }

            this.IsRunning = true;
            this.timeout.Start();
            this.thread = new Thread(this.Worker);
            this.thread.IsBackground = true;
            this.thread.Start();
        }

        /// <summary>
        /// This method stops the thread.
        /// </summary>
        public void Stop()
        {
            if (this.thread == null || !this.thread.IsAlive)
            {
                throw new ArgumentException("Error thread is already dead.");
            }

            this.tcpClient.Close();
            this.stream.Close();
            this.IsRunning = false;
            this.timeout.Stop();
            this.timeout.Close();
            this.thread.Join();
            this.tcpClient = new TcpClient();
        }

        /// <summary>
        /// This method fires when a byte message has been received.
        /// </summary>
        /// <param name="e"> The byte message. </param>
        protected virtual void FireOnMessageReceived(ByteMessageEventArgs e)
        {
            if (this.OnMessageReceived != null)
            {
                this.OnMessageReceived(this, e);
            }
        }

        /// <summary>
        /// This method fires when a byte message has been completed.
        /// </summary>
        /// <param name="e"> The process list. </param>
        protected virtual void FireOnMessageCompleted(ProcessListEventArgs e)
        {
            if (this.OnMessageCompleted != null)
            {
                this.OnMessageCompleted(this, e);
            }
        }

        /// <summary>
        /// This method fires when the hostname has been changed.
        /// </summary>
        /// <param name="e"> The hostname. </param>
        protected virtual void FireOnHostNameChanged(StringEventArgs e)
        {
            if (this.OnHostNameChanged != null)
            {
                this.OnHostNameChanged(this, e);
            }
        }

        /// <summary>
        /// This method fires when the client state has been changed.
        /// </summary>
        /// <param name="e"> The <see cref="BoolEventArgs"/>. </param>
        protected virtual void FireOnStateChanged(BoolEventArgs e)
        {
            if (this.OnStateChanged != null)
            {
                this.OnStateChanged(this, e);
            }
        }

        /// <summary>
        /// The main thread.
        /// </summary>
        private void Worker()
        {
            if (this.tcpClient == null || this.tcpClient.GetStream() == null)
            {
                throw new ArgumentNullException("Error client is null.");
            }

            this.stream = this.tcpClient.GetStream();

            while (this.IsRunning)
            {
                if (this.tcpClient == null)
                {
                    throw new ArgumentNullException("Error client is null.");
                }

                if (this.tcpClient.Available == 0)
                {
                    Thread.Sleep(100);
                    continue;
                }

                byte[] searchBuffer = new byte[8192];

                    try
                    {
                        this.stream.Read(searchBuffer, 0, searchBuffer.Length);
                        this.FireOnMessageReceived(new ByteMessageEventArgs(searchBuffer));
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("Error Message couldnt be received." + ex);
                    }
            }
        }

        /// <summary>
        /// This method tries on timeout to reconnect.
        /// </summary>
        /// <param name="sender"> The object sender. </param>
        /// <param name="e"> The elapsed event args. </param>
        private void TryToReconnect(object sender, ElapsedEventArgs e)
        {
            int reconnect = 3;
            this.tcpClient.Close();
            this.stream.Close();
            this.IsRunning = false;
            this.timeout.Stop();
            this.timeout.Close();
            this.thread.Join();
            this.tcpClient = new TcpClient();

            for (int i = 0; i < reconnect; i++)
            {
                try
                {
                    this.Start();
                    return;
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// This method resets the timer.
        /// </summary>
        private void ResetTimer()
        {
            this.timeout.Stop();
            this.timeout.Start();
        }

        /// <summary>
        /// This method checks the completed message.
        /// </summary>
        /// <param name="sender"> The object sender. </param>
        /// <param name="e"> The byte event args. </param>
        private void CheckCompletedMessage(object sender, ByteMessageEventArgs e)
        {
            ProcessListMessage messageType = new ProcessListMessage();
            Ping pingType = new Ping();

            if (e.Message[0] == (byte)messageType.Type)
            {
                List<byte> list = e.Message.ToList();
                List<byte> hostname = new List<byte>();

                list.RemoveAt(0);

                int hostNameLength = list[0];

                for (int i = 0; i < hostNameLength; i++)
                {
                    hostname.Add(list[i + 1]);
                }

                this.HostName = Encoding.UTF8.GetString(hostname.ToArray());

                list.RemoveRange(0, hostname.Count + 1);

                ProcessListContainer container = NetworkDeSerealizer.DesSerealize(list.ToArray());

                this.FireOnMessageCompleted(new ProcessListEventArgs(container));

                this.ResetTimer();
            }
            else if (e.Message[0] == (byte)pingType.Type)
            {
                this.ResetTimer();
            }
        }
    }
}
