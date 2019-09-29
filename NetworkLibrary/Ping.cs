//-----------------------------------------------------------------------
// <copyright file="Ping.cs" company="FH Wiener Neustadt">
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
    /// The ping message type.
    /// </summary>
    public class Ping : IMessageType
    {
        /// <summary>
        /// Gets the number of the message type.
        /// </summary>
        /// <value> Returns 1. </value>
        public int Type
        {
            get
            {
                return 1;
            }
        }
    }
}