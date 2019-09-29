//-----------------------------------------------------------------------
// <copyright file="BoolEventArgs.cs" company="FH Wiener Neustadt">
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

    /// <summary>
    /// The <see cref="BoolEventArgs"/> class.
    /// </summary>
    public class BoolEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoolEventArgs"/> class.
        /// </summary>
        /// <param name="isRunning"> A true or false value. </param>
        public BoolEventArgs(bool isRunning)
        {
            this.IsRunning = isRunning;
        }

        /// <summary>
        /// Gets a value indicating whether a client is running or not.
        /// </summary>
        /// <value> Is true if the client is still running. </value>
        public bool IsRunning
        {
            get;
            private set;
        }
    }
}
