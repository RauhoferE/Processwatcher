//-----------------------------------------------------------------------
// <copyright file="StringEventArgs.cs" company="FH Wiener Neustadt">
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
    /// The <see cref="StringEventArgs"/> class.
    /// </summary>
    public class StringEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringEventArgs"/> class.
        /// </summary>
        /// <param name="message"> The message as a string. </param>
        public StringEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value> A normal string. </value>
        public string Message
        {
            get;
            private set;
        }
    }
}