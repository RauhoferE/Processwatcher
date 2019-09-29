//-----------------------------------------------------------------------
// <copyright file="ProcessModulesVMEventArgs.cs" company="FH Wiener Neustadt">
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

    /// <summary>
    /// The <see cref="ProcessModulesVMEventArgs"/> class.
    /// </summary>
    public class ProcessModulesVMEventArgs : EventArgs
    {
        /// <summary>
        /// The process list.
        /// </summary>
        private List<ProcessModuleContainerVm> current;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessModulesVMEventArgs"/> class.
        /// </summary>
        /// <param name="processModules"> The list of process modules. </param>
        public ProcessModulesVMEventArgs(List<ProcessModuleContainerVm> processModules)
        {
            this.Current = processModules;
        }

        /// <summary>
        /// Gets the list of process modules.
        /// </summary>
        /// <value> A normal list of modules. </value>
        public List<ProcessModuleContainerVm> Current
        {
            get
            {
                return this.current;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Error the list cant be null.");
                }

                this.current = value;
            }
        }
    }
}
