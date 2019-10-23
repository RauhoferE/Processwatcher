//-----------------------------------------------------------------------
// <copyright file="AgentListVm.cs" company="FH Wiener Neustadt">
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
    using System.Collections.ObjectModel;
    using System.Net;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using ProcessWatcher.Commands;
    using ProcessWatcher.Model;
    using ProcessWatcher.Resource;

    /// <summary>
    /// The <see cref="AgentListVm"/> class.
    /// </summary>
    public class AgentListVm
    {
        /// <summary>
        /// The current dispatcher.
        /// </summary>
        private readonly Dispatcher current;

        /// <summary>
        /// The command to delete a <see cref="TrigFunctionVM"/> from the list view.
        /// </summary>
        private ICommand removeAgentCommand;

        /// <summary>
        /// The IP address.
        /// </summary>
        private string ipAdress;

        /// <summary>
        /// The port number.
        /// </summary>
        private int port;

        /// <summary>
        /// The string to filter for processes.
        /// </summary>
        private string searchTextForProcesses;

        /// <summary>
        /// This value checks if the port is correct.
        /// </summary>
        private bool isPortInputCorrect;

        /// <summary>
        /// The current process list from the checked agent.
        /// </summary>
        private ObservableCollection<ProcessContainerVM> currentProcessesFromCheckedAgent;

        /// <summary>
        /// The cloned processes list from the checked agent.
        /// </summary>
        private ObservableCollection<ProcessContainerVM> clonedCurrentProcessesFromCheckedAgent;

        /// <summary>
        /// The current module list from the selected process.
        /// </summary>
        private ObservableCollection<ProcessModuleContainerVm> currentModulesFromSelectedProcess;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentListVm"/> class.
        /// </summary>
        public AgentListVm()
        {
            this.current = App.Current.Dispatcher;
            this.Agents = new ObservableCollection<AgentVm>();
            this.isPortInputCorrect = true;

            this.removeAgentCommand = new Command(obj => 
            {
                var agentVm = obj as AgentVm;

                if (agentVm != null && !agentVm.IsActive)
                {
                    if (agentVm.IsChecked)
                    {
                        this.CurrentProcessesFromCheckedAgent.Clear();
                    }

                    this.Agents.Remove(agentVm);
                }
                else
                {
                    MessageBox.Show("Error disconnect the agent first!!!");
                }
            });

            this.port = 80;
            this.CurrentProcessesFromCheckedAgent = new ObservableCollection<ProcessContainerVM>();
            this.CurrentModulesFromSelectedProcess = new ObservableCollection<ProcessModuleContainerVm>();
            this.ClonedCurrentProcessesFromCheckedAgent = new ObservableCollection<ProcessContainerVM>();
        }

        /// <summary>
        /// Gets the command to add a agent.
        /// </summary>
        /// <value> A normal agent. </value>
        public ICommand AddAgent
        {
            get
            {
                return new Command(obj =>
                {
                    if (this.port >= 65536 || this.IpAdress == null || this.port < 0 || !this.isPortInputCorrect)
                    {
                        MessageBox.Show($"Please enter a valid ip adress and port number first. ");
                        return;
                    }

                    var agent = new Agent(new IPEndPoint(IPAddress.Parse(this.IpAdress), this.Port));
                    var vm = new AgentVm(agent, this.removeAgentCommand);
                    vm.OnChecked += this.GetCurrentProcesses;
                    vm.OnBoolChanged += this.ChangeBoolOfAgents;
                    vm.OnModulesChanged += this.GetCurrentModules;
                    this.Agents.Add(vm);
                });
            }
        }

        /// <summary>
        /// Gets a list of <see cref="AgentVm"/>.
        /// </summary>
        /// <value> A normal collection. </value>
        public ObservableCollection<AgentVm> Agents
        {
            get;
        }

        /// <summary>
        /// Gets a list of <see cref="ProcessContainerVM"/>.
        /// </summary>
        /// <value> A normal collection. </value>
        public ObservableCollection<ProcessContainerVM> CurrentProcessesFromCheckedAgent
        {
            get
            {
                return this.currentProcessesFromCheckedAgent;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentException("Error the list cant be null.");
                }

                this.currentProcessesFromCheckedAgent = value;
            }
        }

        /// <summary>
        /// Gets a list of cloned <see cref="ProcessContainerVM"/>.
        /// </summary>
        /// <value> A normal collection. </value>
        public ObservableCollection<ProcessContainerVM> ClonedCurrentProcessesFromCheckedAgent
        {
            get
            {
                return this.clonedCurrentProcessesFromCheckedAgent;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentException("Error the list cant be null.");
                }

                this.clonedCurrentProcessesFromCheckedAgent = value;
            }
        }

        /// <summary>
        /// Gets a list of <see cref="ProcessModuleContainerVm"/>.
        /// </summary>
        /// <value> A normal collection. </value>
        public ObservableCollection<ProcessModuleContainerVm> CurrentModulesFromSelectedProcess
        {
            get
            {
                return this.currentModulesFromSelectedProcess;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentException("Error the list cant be null.");
                }

                this.currentModulesFromSelectedProcess = value;
            }
        }

        /// <summary>
        /// Gets or sets the IP address for the text block.
        /// </summary>
        /// <value> A normal string. </value>
        public string IpAdress
        {
            get
            {
                return this.ipAdress;
            }

            set
            {
                IPAddress temp;

                if (!IPAddress.TryParse(value, out temp))
                {
                    throw new ArgumentException("Error please eneter a valid ip adress.");
                }

                this.ipAdress = value;
            }
        }

        /// <summary>
        /// Gets or sets the port for the text block.
        /// </summary>
        /// <value> A normal integer. </value>
        public int Port
        {
            get
            {
                return this.port;
            }

            set
            {
                int i;

                if (value < 0 || !int.TryParse(value.ToString(), out i))
                {
                    this.isPortInputCorrect = false;
                    throw new ArgumentException("Error please enter a valid port.");
                }

                this.isPortInputCorrect = true;
                this.port = value;
            }
        }

        /// <summary>
        /// Gets or sets a value to filter for processes.
        /// </summary>
        /// <value> A normal string. </value>
        public string SearchTextForProcesses
        {
            get
            {
                return this.searchTextForProcesses;
            }

            set
            {
                this.searchTextForProcesses = value;
                this.SearchForProcess();
            }
        }

        /// <summary>
        /// This method unchecks all the other agents.
        /// </summary>
        /// <param name="sender"> The object sender. </param>
        /// <param name="e"> The current agent. </param>
        private void ChangeBoolOfAgents(object sender, AgentVmEventArgs e)
        {
            foreach (var item in this.Agents)
            {
                if (item != e.AgentVm)
                {
                    item.IsChecked = false;
                }
            }

            this.current.Invoke(new Action(() => { this.CurrentModulesFromSelectedProcess.Clear(); }));
            this.current.Invoke(new Action(() => { this.ClonedCurrentProcessesFromCheckedAgent.Clear(); }));
            this.current.Invoke(new Action(() => { this.CurrentProcessesFromCheckedAgent.Clear(); }));
        }

        /// <summary>
        /// This method gets the current processes from the checked agent.
        /// </summary>
        /// <param name="sender"> The object sender. </param>
        /// <param name="e"> The processes. </param>
        private void GetCurrentProcesses(object sender, ProcessContainerVMEventArgs e)
        {
            this.current.Invoke(new Action(() => { this.ClonedCurrentProcessesFromCheckedAgent.Clear(); }));

            if (e.Current.Count > 0)
            {
                foreach (var item in e.Current)
                {
                    this.current.Invoke(new Action(() => { this.ClonedCurrentProcessesFromCheckedAgent.Add(item); }));
                }
            }

            this.SearchForProcess();
        }

        /// <summary>
        /// This method gets the current modules from the checked process.
        /// </summary>
        /// <param name="sender"> The object sender. </param>
        /// <param name="e"> The processes. </param>
        private void GetCurrentModules(object sender, ProcessModulesVMEventArgs e)
        {
            this.current.Invoke(new Action(() => { this.CurrentModulesFromSelectedProcess.Clear(); }));

            if (e.Current.Count > 0)
            {
                foreach (var item in e.Current)
                {
                    this.current.Invoke(new Action(() => { this.CurrentModulesFromSelectedProcess.Add(item); }));
                }
            }
        }

        /// <summary>
        /// This method searches for the filtered text.
        /// </summary>
        private void SearchForProcess()
        {
            this.current.Invoke(new Action(() => { this.CurrentProcessesFromCheckedAgent.Clear(); }));

            if (this.ClonedCurrentProcessesFromCheckedAgent.Count > 0)
            {
                if (string.IsNullOrEmpty(this.searchTextForProcesses))
                {
                    foreach (var item in this.ClonedCurrentProcessesFromCheckedAgent)
                    {
                        this.current.Invoke(new Action(() => { this.CurrentProcessesFromCheckedAgent.Add(item); }));
                    }
                }
                else
                {
                    foreach (var item in this.ClonedCurrentProcessesFromCheckedAgent)
                    {
                        if (item.Name.Contains(this.searchTextForProcesses))
                        {
                            this.current.Invoke(new Action(() => { this.CurrentProcessesFromCheckedAgent.Add(item); }));
                        }
                    }
                }
            }
        }
    }
}
