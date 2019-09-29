//-----------------------------------------------------------------------
// <copyright file="ProcessContainerVM.cs" company="FH Wiener Neustadt">
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
    using System.Linq;
    using NetworkLibrary;

    /// <summary>
    /// The <see cref="ProcessContainerVM"/> class.
    /// </summary>
    public class ProcessContainerVM
    {
        /// <summary>
        /// The <see cref="ProcessContainer"/>.
        /// </summary>
        private ProcessContainer processContainer;

        /// <summary>
        /// Checks if the process is  checked in the view.
        /// </summary>
        private bool isChecked;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessContainerVM"/> class.
        /// </summary>
        /// <param name="process"> The process to be contained. </param>
        public ProcessContainerVM(ProcessContainer process)
        {
            this.processContainer = process;

            this.Modules = new ObservableCollection<ProcessModuleContainerVm>();

            if (this.processContainer.Modules != null)
            {
                foreach (var item in this.processContainer.Modules)
                {
                    this.Modules.Add(new ProcessModuleContainerVm(item));
                }
            }

            this.isChecked = false;
        }

        /// <summary>
        /// The event when the process has been changed.
        /// </summary>
        public event EventHandler<ProcessContainerVMBoolEventArgs> OnBoolChanged;

        /// <summary>
        /// The event when the modules changed.
        /// </summary>
        public event EventHandler<ProcessModulesVMEventArgs> OnModulesChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the process is checked or not.
        /// </summary>
        /// <value> Is true if the process is checked. </value>
        public bool IsChecked
        {
            get
            {
                return this.isChecked;
            }

            set
            {
                this.isChecked = value;

                if (value == true)
                {
                    this.FireOnBoolChanged(new ProcessContainerVMBoolEventArgs(this));
                    this.FireOnModulesChanged(new ProcessModulesVMEventArgs(this.Modules.ToList()));
                }
            }
        }

        /// <summary>
        /// Gets the name of the process.
        /// </summary>
        /// <value> A normal string. </value>
        public string Name
        {
            get
            {
                return this.processContainer.Name;
            }
        }

        /// <summary>
        /// Gets the id of the process.
        /// </summary>
        /// <value> A normal integer. </value>
        public int Id
        {
            get
            {
                return this.processContainer.Id;
            }
        }

        /// <summary>
        /// Gets the memory size of the process.
        /// </summary>
        /// <value> A normal integer. </value>
        public double Memory
        {
            get
            {
                return Math.Round((this.processContainer.Memory / 1024d) / 1024d, 2);
            }
        }

        /// <summary>
        /// Gets the start time of the process.
        /// </summary>
        /// <value> A normal <see cref="DateTime"/>. </value>
        public string StartDate
        {
            get
            {
                return this.processContainer.StartDate.ToString();
            }
        }

        /// <summary>
        /// Gets the title of the process.
        /// </summary>
        /// <value> A normal string. </value>
        public string Title
        {
            get
            {
                return this.processContainer.Title;
            }
        }

        /// <summary>
        /// Gets the priority of the process.
        /// </summary>
        /// <value> A normal integer. </value>
        public int Priority
        {
            get
            {
                return this.processContainer.Priority;
            }
        }

        /// <summary>
        /// Gets the modules of the Process.
        /// </summary>
        /// <value> A normal list of modules. </value>
        public ObservableCollection<ProcessModuleContainerVm> Modules
        {
            get;
        }

        /// <summary>
        /// This method fires when the process has been checked.
        /// </summary>
        /// <param name="e"> The current process. </param>
        protected virtual void FireOnBoolChanged(ProcessContainerVMBoolEventArgs e)
        {
            if (this.OnBoolChanged != null)
            {
                this.OnBoolChanged(this, new ProcessContainerVMBoolEventArgs(this));
            }
        }

        /// <summary>
        /// This method fires when the modules changed.
        /// </summary>
        /// <param name="e"> The changed modules. </param>
        protected virtual void FireOnModulesChanged(ProcessModulesVMEventArgs e)
        {
            if (this.OnModulesChanged != null)
            {
                this.OnModulesChanged(this, e);
            }
        }
    }
}
