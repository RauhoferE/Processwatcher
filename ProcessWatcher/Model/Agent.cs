//-----------------------------------------------------------------------
// <copyright file="Agent.cs" company="FH Wiener Neustadt">
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
    using System.Net;
    using System.Timers;
    using NetworkLibrary;

    /// <summary>
    /// The <see cref="Agent"/> class.
    /// </summary>
    public class Agent
    {
        /// <summary>
        /// The current client.
        /// </summary>
        private Client client;

        /// <summary>
        /// The list of processes.
        /// </summary>
        private List<ProcessContainer> processes;

        /// <summary>
        /// Initializes a new instance of the <see cref="Agent"/> class.
        /// </summary>
        /// <param name="endPoint"> The current endpoint. </param>
        public Agent(IPEndPoint endPoint)
        {
            this.Processes = new List<ProcessContainer>();
            this.client = new Client(endPoint);
            this.client.OnMessageCompleted += this.GetNewProcesses;
            this.client.Timeout.Elapsed += this.OnTimeout;
            this.client.OnStateChanged += this.ChangeIsRunning;
            this.client.OnHostNameChanged += this.ChangeHostName;
        }

        /// <summary>
        /// The event when the processes has been changed.
        /// </summary>
        public event EventHandler<ProcessListEventArgs> OnProcessChanged;

        /// <summary>
        /// The event when the hostname changes.
        /// </summary>
        public event EventHandler<StringEventArgs> OnHostNameChanged;

        /// <summary>
        /// The event when the clients state changed.
        /// </summary>
        public event EventHandler<BoolEventArgs> OnStateChanged;

        /// <summary>
        /// Gets a value indicating whether the client is active.
        /// </summary>
        /// <value> Is true when the client is still active. </value>
        public bool IsActive
        {
            get
            {
                return this.client.IsRunning;
            }

            private set
            {
                this.FireOnStateChanged(new BoolEventArgs(this.client.IsRunning));
            }
        }

        /// <summary>
        /// Gets the hostname of the client.
        /// </summary>
        /// <value> A normal string. </value>
        public string HostName
        {
            get
            {
                return this.client.HostName;
            }

            private set
            {
                this.FireOnHostNameChanged(new StringEventArgs(this.client.HostName));
            }
        }

        /// <summary>
        /// Gets or sets the endpoint of the connection.
        /// </summary>
        /// <value> A normal IP endpoint. </value>
        public IPEndPoint IPEndPoint
        {
            get
            {
                return this.client.IPEndPoint;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Error the endpoint cant be null.");
                }

                value = this.client.IPEndPoint;
            }
        }

        /// <summary>
        /// Gets the list of processes.
        /// </summary>
        /// <value> A normal list. </value>
        public List<ProcessContainer> Processes
        {
            get
            {
                return this.processes;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Error the process list cant be null.");
                }

                this.processes = value;
            }
        }

        /// <summary>
        /// This method starts the client.
        /// </summary>
        public void Start()
        {
            try
            {
                this.client.Start();
            }
            catch (Exception)
            {
                throw new ArgumentException("Error couldnt start the client.");
            }
        }

        /// <summary>
        /// This method stops the client.
        /// </summary>
        public void Stop()
        {
            try
            {
                this.client.Stop();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error couldnt stop the client." + ex);
            }
        }

        /// <summary>
        /// This method fires when the processes have been changed.
        /// </summary>
        /// <param name="e"> The process list. </param>
        protected virtual void FireOnProcessChanged(ProcessListEventArgs e)
        {
            if (this.OnProcessChanged != null)
            {
                this.OnProcessChanged(this, e);
            }
        }

        /// <summary>
        /// This method fires when the state has been changed.
        /// </summary>
        /// <param name="e"> The current state. </param>
        protected virtual void FireOnStateChanged(BoolEventArgs e)
        {
            if (this.OnStateChanged != null)
            {
                this.OnStateChanged(this, e);
            }
        }

        /// <summary>
        /// This method fires when the host name has been changed.
        /// </summary>
        /// <param name="e"> The host name. </param>
        protected virtual void FireOnHostNameChanged(StringEventArgs e)
        {
            if (this.OnHostNameChanged != null)
            {
                this.OnHostNameChanged(this, e);
            }
        }

        /// <summary>
        /// This method clears the process list.
        /// </summary>
        /// <param name="sender"> The object sender. </param>
        /// <param name="e"> The elapsed event args. </param>
        private void OnTimeout(object sender, ElapsedEventArgs e)
        {
            this.Processes.Clear();
            this.FireOnProcessChanged(new ProcessListEventArgs(new ProcessListContainer()));
        }

        /// <summary>
        /// This method fires when the state changes.
        /// </summary>
        /// <param name="sender"> The object sender. </param>
        /// <param name="e"> The <see cref="BoolEventArgs"/>. </param>
        private void ChangeIsRunning(object sender, BoolEventArgs e)
        {
            this.IsActive = e.IsRunning;
        }

        /// <summary>
        /// This method fires when the host name changes.
        /// </summary>
        /// <param name="sender"> The object sender. </param>
        /// <param name="e"> The string args. </param>
        private void ChangeHostName(object sender, StringEventArgs e)
        {
            this.HostName = e.Message;
        }

        /// <summary>
        /// This method gets the new processes.
        /// </summary>
        /// <param name="sender"> The object sender. </param>
        /// <param name="e"> The process list. </param>
        private void GetNewProcesses(object sender, ProcessListEventArgs e)
        {
            List<ProcessContainer> temp = new List<ProcessContainer>();

            foreach (var item in this.Processes)
            {
                temp.Add(item);
            }

            if (this.Processes.Count > 0)
            {
                foreach (var item in e.List.OldProcesses)
                {
                    foreach (var oldProcesses in this.Processes)
                    {
                        if (item.Id == oldProcesses.Id)
                        {
                            temp.Remove(oldProcesses);
                        }
                    }
                }
            }

            this.Processes = temp;

            foreach (var item in e.List.NewProcesses)
            {
                this.Processes.Add(item);
            }

            ProcessListContainer container = new ProcessListContainer();

            foreach (var item in this.Processes)
            {
                container.NewProcesses.Add(item);
            }

            this.FireOnProcessChanged(new ProcessListEventArgs(container));
        }
    }
}
