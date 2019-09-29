//-----------------------------------------------------------------------
// <copyright file="ProcessListEventArgs.cs" company="FH Wiener Neustadt">
//     Copyright (c) Emre Rauhofer. All rights reserved.
// </copyright>
// <author>Emre Rauhofer</author>
// <summary>
// This is a network library.
// </summary>
//-----------------------------------------------------------------------
namespace NetworkLibrary
{
    using System;

    /// <summary>
    /// The <see cref="ProcessListEventArgs"/> class.
    /// </summary>
    public class ProcessListEventArgs : EventArgs
    {
        /// <summary>
        /// The process list.
        /// </summary>
        private ProcessListContainer list;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessListEventArgs"/> class.
        /// </summary>
        /// <param name="container"> The list of the new and old processes. </param>
        public ProcessListEventArgs(ProcessListContainer container)
        {
            this.List = container;
        }

        /// <summary>
        /// Gets the list of new and old processes.
        /// </summary>
        /// <value> A normal <see cref="ProcessListContainer"/> class. </value>
        public ProcessListContainer List
        {
            get
            {
                return this.list;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Error the list cant be null.");  
                }

                this.list = value;
            }
        }
    }
}