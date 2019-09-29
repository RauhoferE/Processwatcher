//-----------------------------------------------------------------------
// <copyright file="MessageBuilder.cs" company="FH Wiener Neustadt">
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
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The <see cref="MessageBuilder"/> class.
    /// </summary>
    public class MessageBuilder
    {
        /// <summary>
        /// The message without meta information as a byte list.
        /// </summary>
        private List<byte> messageArray;

        /// <summary>
        /// The full message as a byte array.
        /// </summary>
        private List<byte> fullMessage;

        /// <summary>
        /// This value is true when a header has been found.
        /// </summary>
        private bool headerFound;

        /// <summary>
        /// This is the header as a byte array.
        /// </summary>
        private byte[] header;

        /// <summary>
        /// The length of the message.
        /// </summary>
        private int messageLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBuilder"/> class.
        /// </summary>
        public MessageBuilder()
        {
            this.headerFound = false;
            this.header = Encoding.UTF8.GetBytes(NetworkSerealizer.Header);
            this.messageArray = new List<byte>();
            this.fullMessage = new List<byte>();
            this.messageLength = 0;
        }

        /// <summary>
        /// This event fires when a message has been completed.
        /// </summary>
        public event EventHandler<ByteMessageEventArgs> OnMessageCompleted;

        /// <summary>
        /// This method gets a byte  array and then looks for either the header or terminator.
        /// </summary>
        /// <param name="sender"> The object sender. </param>
        /// <param name="e"> The message event args. </param>
        public void BuildMessage(object sender, ByteMessageEventArgs e)
        {
            if (!this.headerFound)
            {
                this.LookForHeader(e);
            }
            else
            {
                this.GetMessage(e, 0, 0);
            }
        }

        /// <summary>
        /// This method fires when a message has been completed.
        /// </summary>
        /// <param name="e"> The message event args. </param>
        protected virtual void FireOnMessageCompleted(ByteMessageEventArgs e)
        {
            if (this.OnMessageCompleted != null)
            {
                this.OnMessageCompleted(this, e);
            }
        }

        /// <summary>
        /// This method returns the first n bytes of the buffer at the index.
        /// </summary>
        /// <param name="buffer"> The buffer to be searched. </param>
        /// <param name="index"> The index where to return. </param>
        /// <param name="length"> The length of the byte array. </param>
        /// <returns> It returns a new byte array. </returns>
        private byte[] CopyByte(byte[] buffer, int index, int length)
        {
            List<byte> test = new List<byte>();

            for (int i = index; i < index + length; i++)
            {
                if (i < buffer.Length)
                {
                    test.Add(buffer[i]);
                }
            }

            return test.ToArray();
        }

        /// <summary>
        /// This method looks for a header in the message.
        /// </summary>
        /// <param name="e"> The byte message. </param>
        private void LookForHeader(ByteMessageEventArgs e)
        {
            for (int i = 0; i < e.Message.Length; i++)
            {
                byte[] searchbuffer = this.CopyByte(e.Message, i, 8);

                if (this.header.SequenceEqual(searchbuffer))
                {
                    this.fullMessage.AddRange(this.header);
                    this.headerFound = true;
                    this.SearchForLength(e, i);
                    this.GetMessage(e, i, 108);
                    return;
                }
            }
        }

        /// <summary>
        /// This method gets the whole message.
        /// </summary>
        /// <param name="e"> The byte message. </param>
        /// <param name="index"> The start index of the header. </param>
        /// <param name="length"> The start of the message. </param>
        private void GetMessage(ByteMessageEventArgs e, int index, int length)
        {
            int tempLength = this.messageLength;

            for (int i = 0; i < tempLength; i++)
            {
                if (i + index + length < e.Message.Length)
                {
                    this.messageArray.Add(e.Message[i + index + length]);
                    this.fullMessage.Add(e.Message[i + index + length]);
                    this.messageLength--;
                }
            }

            if (this.messageLength == 0)
            {
                if (this.IsCheckSumCorrect())
                {
                    this.messageArray.RemoveAt(this.messageArray.Count - 1);
                    this.FireOnMessageCompleted(new ByteMessageEventArgs(this.messageArray.ToArray()));
                }

                this.fullMessage.Clear();
                this.headerFound = false;
                this.messageArray.Clear();
            }
        }

        /// <summary>
        /// This method creates the message length.
        /// </summary>
        /// <param name="e"> The message. </param>
        /// <param name="index"> The index of the message. </param>
        private void SearchForLength(ByteMessageEventArgs e, int index)
        {
            List<byte> lengthArray = new List<byte>();

            for (int i = index + 8; i < 100; i++)
            {
                lengthArray.Add(e.Message[i]);
            }

            this.fullMessage.AddRange(lengthArray);
            this.messageLength = BitConverter.ToInt32(lengthArray.ToArray(), 0) + 1;
        }

        /// <summary>
        /// This method checks if the checksum is correct.
        /// </summary>
        /// <returns> It returns true if the checksum is correct. </returns>
        private bool IsCheckSumCorrect()
        {
            return NetworkSerealizer.CalculateCheckSum(this.fullMessage.GetRange(0, this.fullMessage.Count - 1).ToArray()) == this.fullMessage.ElementAt(this.fullMessage.Count - 1);
        }
    }
}