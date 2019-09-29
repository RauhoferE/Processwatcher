//-----------------------------------------------------------------------
// <copyright file="Renderer.cs" company="FH Wiener Neustadt">
//     Copyright (c) Emre Rauhofer. All rights reserved.
// </copyright>
// <author>Emre Rauhofer</author>
// <summary>
// This is a remote agent.
// </summary>
//-----------------------------------------------------------------------
namespace RemoteAgent
{
    using System;

    /// <summary>
    /// The <see cref="Renderer"/> class.
    /// </summary>
    public class Renderer : IRenderer
    {
        /// <summary>
        /// This method prints a error message.
        /// </summary>
        /// <param name="message"> The message. </param>
        public void PrintErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(message);
        }

        /// <summary>
        /// This method prints a message.
        /// </summary>
        /// <param name="message"> The message. </param>
        public void PrintMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(message);
        }
    }
}