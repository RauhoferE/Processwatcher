//-----------------------------------------------------------------------
// <copyright file="AgentVm.cs" company="FH Wiener Neustadt">
//     Copyright (c) Emre Rauhofer. All rights reserved.
// </copyright>
// <author>Emre Rauhofer</author>
// <summary>
// This is a dashboard.
// </summary>
//-----------------------------------------------------------------------
namespace ProcessWatcher.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;
    using NetworkLibrary;
    using ProcessWatcher.Commands;
    using ProcessWatcher.Model;

    /// <summary>
    /// The <see cref="AgentVm"/> class.
    /// </summary>
    public class AgentVm : INotifyPropertyChanged
    {
        /// <summary>
        /// The target agent.
        /// </summary>
        private Agent agent;

        /// <summary>
        /// The command to remove the function.
        /// </summary>
        private ICommand removeCommand;

        /// <summary>
        /// A value indicating whether a client is active or not.
        /// </summary>
        private bool isChecked;

        /// <summary>
        /// The process list.
        /// </summary>
        private List<ProcessContainerVM> processes;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentVm"/> class.
        /// </summary>
        /// <param name="agent"> The current agent. </param>
        /// <param name="removeCommand"> The remove command. </param>
        public AgentVm(Agent agent, ICommand removeCommand)
        {
            this.agent = agent;
            this.removeCommand = removeCommand;
            this.Processes = new List<ProcessContainerVM>();

            foreach (var item in this.agent.Processes)
            {
                this.Processes.Add(new ProcessContainerVM(item));
            }

            this.agent.OnProcessChanged += this.UpdateProcesses;
            this.agent.OnStateChanged += this.ChangeActive;
            this.agent.OnHostNameChanged += this.ChangeHostName;

            this.IsChecked = false;
        }

        /// <summary>
        /// The event when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The event when the agent has been checked.
        /// </summary>
        public event EventHandler<ProcessContainerVMEventArgs> OnChecked;

        /// <summary>
        /// The event when the agent state changes.
        /// </summary>
        public event EventHandler<AgentVmEventArgs> OnBoolChanged;

        /// <summary>
        /// The event when the modules change.
        /// </summary>
        public event EventHandler<ProcessModulesVMEventArgs> OnModulesChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the agent is checked or not.
        /// </summary>
        /// <value> Is true if the agent is checked. </value>
        public bool IsChecked
        {
            get
            {
                return this.isChecked;
            }

            set
            {
                this.isChecked = value;

                if (this.isChecked == true)
                {
                    this.FireOnBoolChanged(new AgentVmEventArgs(this));
                    this.FireOnIsChecked(new ProcessContainerVMEventArgs(this.Processes));
                }
                else
                {
                    foreach (var item in this.Processes)
                    {
                        item.IsChecked = false;
                    }

                    this.FireOnModulesChanged(new ProcessModulesVMEventArgs(new List<ProcessModuleContainerVm>()));
                }

                this.FireOnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the agent is still active.
        /// </summary>
        /// <value> Is true if the agent is still active. </value>
        public bool IsActive
        {
            get
            {
                return this.agent.IsActive;
            }

            private set
            {
                this.FireOnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the name of the client machine.
        /// </summary>
        /// <value> A normal string. </value>
        public string HostName
        {
            get
            {
                return this.agent.HostName;
            }

            private set
            {
                this.FireOnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the port number of the connected port.
        /// </summary>
        /// <value> A normal integer. </value>
        public int Port
        {
            get
            {
                return this.agent.IPEndPoint.Port;
            }
        }

        /// <summary>
        /// Gets the IP address of the client.
        /// </summary>
        /// <value> A normal string. </value>
        public string IpAdress
        {
            get
            {
                return this.agent.IPEndPoint.Address.ToString();
            }
        }

        /// <summary>
        /// Gets the number of processes.
        /// </summary>
        /// <value> A normal integer. </value>
        public int ProcessCount
        {
            get
            {
                return this.Processes.Count;
            }

            private set
            {
                this.FireOnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the command to connect to the client.
        /// </summary>
        /// <value> A normal command. </value>
        public ICommand Connenct
        {
            get
            {
                return new Command(obj =>
                {
                    try
                    {
                        this.agent.Start();
                        MessageBox.Show($"Sucessfully connected.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occured: " + ex);
                    }
                });
            }
        }

        /// <summary>
        /// Gets the command to disconnect to the client.
        /// </summary>
        /// <value> A normal command. </value>
        public ICommand Disconnect
        {
            get
            {
                return new Command(obj =>
                {
                    try
                    {
                        this.agent.Stop();
                        MessageBox.Show($"Sucessfully disconnected.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occured: " + ex.Message);
                    }
                });
            }
        }

        /// <summary>
        /// Gets the command to remove the client.
        /// </summary>
        /// <value> A normal command. </value>
        public ICommand Remove
        {
            get
            {
                return this.removeCommand;
            }
        }

        /// <summary>
        /// Gets the list of processes.
        /// </summary>
        /// <value> A normal list. </value>
        public List<ProcessContainerVM> Processes
        {
            get
            {
                return this.processes;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Error the list cant be null");
                }

                this.processes = value;
            }
        }

        /// <summary>
        /// Fires when the property has been changed.
        /// </summary>
        /// <param name="propertyName"> The property name. </param>
        protected virtual void FireOnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Fires when the agent has been checked.
        /// </summary>
        /// <param name="e"> The processes. </param>
        protected virtual void FireOnIsChecked(ProcessContainerVMEventArgs e)
        {
            if (this.OnChecked != null)
            {
                this.OnChecked(this, e);
            }
        }

        /// <summary>
        /// Fires when the agent activated.
        /// </summary>
        /// <param name="e"> The activated agent. </param>
        protected virtual void FireOnBoolChanged(AgentVmEventArgs e)
        {
            if (this.OnBoolChanged != null)
            {
                this.OnBoolChanged(this, e);
            }
        }

        /// <summary>
        /// Fires when the modules changed.
        /// </summary>
        /// <param name="e"> The changed modules. </param>
        protected virtual void FireOnModulesChanged(ProcessModulesVMEventArgs e)
        {
            if (this.OnModulesChanged != null)
            {
                this.OnModulesChanged(this, e);
            }
        }

        /// <summary>
        /// This method changes the host name.
        /// </summary>
        /// <param name="sender"> The object sender. </param>
        /// <param name="e"> The name of the host. </param>
        private void ChangeHostName(object sender, StringEventArgs e)
        {
            this.HostName = e.Message;
        }

        /// <summary>
        /// This method changes the active.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The <see cref="BoolEventArgs"/>. </param>
        private void ChangeActive(object sender, BoolEventArgs e)
        {
            this.IsActive = e.IsRunning;
        }

        /// <summary>
        /// This method updates the processes.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The processes. </param>
        private void UpdateProcesses(object sender, ProcessListEventArgs e)
        {
            List<ProcessContainerVM> temp = new List<ProcessContainerVM>();

            foreach (var item in e.List.NewProcesses)
            {
                ProcessContainerVM processContainerVM = new ProcessContainerVM(item);
                foreach (var item2 in this.Processes)
                {
                    if (item.Id == item2.Id && item2.IsChecked == true)
                    {
                        processContainerVM.IsChecked = true;
                    }
                }
                
                processContainerVM.OnBoolChanged += this.ChangeBoolOfProcesses;
                processContainerVM.OnModulesChanged += this.ChangeModulesOfProcesses;
                temp.Add(processContainerVM);
            }

            this.Processes.Clear();

            this.ProcessCount = this.Processes.Count;

            foreach (var item in temp)
            {
                this.Processes.Add(item);
            }

            if (this.isChecked == true)
            {
                this.FireOnIsChecked(new ProcessContainerVMEventArgs(this.Processes));
            }
        }

        /// <summary>
        /// This method changes the value of the truth or false value.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The process. </param>
        private void ChangeBoolOfProcesses(object sender, ProcessContainerVMBoolEventArgs e)
        {
            foreach (var item in this.Processes)
            {
                if (item != e.ProcessContainerVM)
                {
                    item.IsChecked = false;
                }
            }
        }

        /// <summary>
        /// Fires when the modules have changed.
        /// </summary>
        /// <param name="sender"> The object sender. </param>
        /// <param name="e"> The modules. </param>
        private void ChangeModulesOfProcesses(object sender, ProcessModulesVMEventArgs e)
        {
            this.FireOnModulesChanged(e);
        }
    }
}
