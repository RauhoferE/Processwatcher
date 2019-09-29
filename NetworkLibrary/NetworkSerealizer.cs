//-----------------------------------------------------------------------
// <copyright file="NetworkSerealizer.cs" company="FH Wiener Neustadt">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    /// <summary>
    /// The <see cref="NetworkSerealizer"/> class.
    /// </summary>
    public static class NetworkSerealizer
    {
        /// <summary>
        /// The header for every message.
        /// </summary>
        public static readonly string Header = "7T6Ru2kd";

        /// <summary>
        /// The terminator for every message.
        /// </summary>
        public static readonly string Terminator = "&&6Ru2&&";

        /// <summary>
        /// This method encrypts a process container.
        /// </summary>
        /// <param name="e"> The <see cref="ProcessListContainer"/> to encrypt. </param>
        /// <param name="hostname"> The hostname of the machine. </param>
        /// <returns> It returns a byte message. </returns>
        public static byte[] Serealize(ProcessListContainer e, string hostname)
        {
            ProcessListMessage messageType = new ProcessListMessage(); 

            List<byte> message;
            byte[] headerEnc = Encoding.UTF8.GetBytes(Header);
            byte[] hostEnc = Encoding.UTF8.GetBytes(hostname);

            if (hostEnc.Length > 255)
            {
                throw new ArgumentException("Error hostname to long.");
            }

            try
            {
                using (var ms = new MemoryStream())
                {
                    BinaryFormatter writer = new BinaryFormatter();
                    writer.Serialize(ms, e);
                    message = ms.ToArray().ToList();
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("Error couldnt serealize the message.");
            }

            List<byte> finalMessage = new List<byte>();

            finalMessage.AddRange(headerEnc);

            byte[] messageLength = new byte[100];

            for (int i = 0; i < BitConverter.GetBytes(hostEnc.Length + message.Count + 2).Length; i++)
            {
                messageLength[i] = BitConverter.GetBytes(hostEnc.Length + message.Count + 2)[i];
            }

            finalMessage.AddRange(messageLength);

            finalMessage.Add((byte)messageType.Type);

            finalMessage.Add((byte)hostEnc.Length);

            finalMessage.AddRange(hostEnc);

            finalMessage.AddRange(message);

            finalMessage.Add(CalculateCheckSum(finalMessage.ToArray()));

            return finalMessage.ToArray();
        }

        /// <summary>
        /// This method encrypt a message to ping the dashboard.
        /// </summary>
        /// <returns> It returns a encrypt message. </returns>
        public static byte[] SerealizePing()
        {
            Ping ping = new Ping();

            byte[] headerEnc = Encoding.UTF8.GetBytes(Header);
            List<byte> message = new List<byte>();

            byte[] messageLength = new byte[100];

            for (int i = 0; i < BitConverter.GetBytes(1).Length; i++)
            {
                messageLength[i] = BitConverter.GetBytes(1)[i];
            }

            message.AddRange(headerEnc);
            message.AddRange(messageLength);

            message.Add((byte)ping.Type);

            message.Add(CalculateCheckSum(message.ToArray()));

            return message.ToArray();
        }

        /// <summary>
        /// This method gets the calculated checksum.
        /// </summary>
        /// <param name="array"> The message array. </param>
        /// <returns> It returns a byte. </returns>
        public static byte CalculateCheckSum(byte[] array)
        {
            int firstChecksum = 1;

            int secondChecksum = 0;

            for (int i = 0; i < array.Length; i++)
            {
                firstChecksum = (firstChecksum + array[i]) % 255;
                secondChecksum = (secondChecksum + firstChecksum) % 255;
            }

            return (byte)((secondChecksum << 8) | firstChecksum);
        }
    }
}