//-----------------------------------------------------------------------
// <copyright file="NetworkDeSerealizer.cs" company="FH Wiener Neustadt">
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
    using System.IO;
    using System.IO.Compression;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// The <see cref="NetworkDeSerealizer"/> class.
    /// </summary>
    public static class NetworkDeSerealizer
    {
        /// <summary>
        /// This method can decrypt a message.
        /// </summary>
        /// <param name="message"> The byte message. </param>
        /// <returns> It returns a <see cref="ProcessListContainer"/>. </returns>
        public static ProcessListContainer DesSerealize(byte[] message)
        {
            ProcessListContainer processListContainer;

            try
            {
                using (var s = new MemoryStream(message))
                {
                    BinaryFormatter writer = new BinaryFormatter();
                    processListContainer = (ProcessListContainer)writer.Deserialize(s);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error couldnt serealize the message." + ex);
            }

            return processListContainer;
        }
    }
}