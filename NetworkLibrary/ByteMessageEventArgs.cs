//-----------------------------------------------------------------------
// <copyright file="ByteMessageEventArgs.cs" company="FH Wiener Neustadt">
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
    /// The byte message event args.
    /// </summary>
    public class ByteMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ByteMessageEventArgs"/> class.
        /// </summary>
        /// <param name="message"> The byte message. </param>
        public ByteMessageEventArgs(byte[] message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets the byte message.
        /// </summary>
        /// <value> A normal byte array. </value>
        public byte[] Message
        {
            get;
            private set;
        }
    }
}
