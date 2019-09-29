//-----------------------------------------------------------------------
// <copyright file="ProcessListMessage.cs" company="FH Wiener Neustadt">
//     Copyright (c) Emre Rauhofer. All rights reserved.
// </copyright>
// <author>Emre Rauhofer</author>
// <summary>
// This is a network library.
// </summary>
//-----------------------------------------------------------------------
namespace NetworkLibrary
{
    /// <summary>
    /// The process list message type.
    /// </summary>
    public class ProcessListMessage : IMessageType
    {
        /// <summary>
        /// Gets the type number of the message.
        /// </summary>
        /// <value> Returns 0. </value>
        public int Type
        {
            get
            {
                return 0;
            }
        }
    }
}