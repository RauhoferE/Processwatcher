//-----------------------------------------------------------------------
// <copyright file="ProcessContainerVMBoolEventArgs.cs" company="FH Wiener Neustadt">
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

    /// <summary>
    /// The <see cref="ProcessContainerVMBoolEventArgs"/> class.
    /// </summary>
    public class ProcessContainerVMBoolEventArgs : EventArgs
    {
        /// <summary>
        /// The process container.
        /// </summary>
        private ProcessContainerVM processContainerVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessContainerVMBoolEventArgs"/> class.
        /// </summary>
        /// <param name="e"> The <see cref="ProcessContainerVM"/>. </param>
        public ProcessContainerVMBoolEventArgs(ProcessContainerVM e)
        {
            this.ProcessContainerVM = e;
        }

        /// <summary>
        /// Gets the process container.
        /// </summary>
        /// <value> A <see cref="ProcessContainerVM"/>. </value>
        public ProcessContainerVM ProcessContainerVM
        {
            get
            {
                return this.processContainerVM;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Error value cant be null.");
                }

                this.processContainerVM = value;
            }
        }
    }
}