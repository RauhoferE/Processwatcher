//-----------------------------------------------------------------------
// <copyright file="ApplicationParamsparser.cs" company="FH Wiener Neustadt">
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
    using System.Net;

    /// <summary>
    /// The application parameter parser.
    /// </summary>
    public class ApplicationParamsparser
    {
        /// <summary>
        /// The port number.
        /// </summary>
        private int port;

        /// <summary>
        /// Gets the port number.
        /// </summary>
        /// <value> An integer that is bigger than 0. </value>
        public int Port
        {
            get
            {
                return this.port;
            }

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Error the port cant be smaller than 0.");
                }

                this.port = value;
            }
        }

        /// <summary>
        /// This method Parses the arguments into the parser class.
        /// </summary>
        /// <param name="args"> The string arguments. </param>
        /// <returns> It returns a class where the values have been parsed. </returns>
        public static ApplicationParamsparser Parse(string[] args)
        {
            ApplicationParamsparser applicationSettings = new ApplicationParamsparser();

            if (args.Length == 0)
            {
                applicationSettings.SetPortNumber(80);
                return applicationSettings;
            }

            for (int i = 0; i < args.Length; i++)
            {
                string portName = "/port:" + args[i].Substring(6, args[i].Length - 6);

                if (args[i].ToLower() == portName)
                {
                    applicationSettings.SetPortNumber(ReturnPortNumber(args[i].Substring(6, args[i].Length - 6)));
                }
                else
                {
                    throw new ArgumentException("Error parameter is unknown: " + args[i]);
                }
            }

            return applicationSettings;
        }

        /// <summary>
        /// This method sets the port number in the parser.
        /// </summary>
        /// <param name="port"> The port number. </param>
        public void SetPortNumber(int port)
        {
            this.Port = port;
        }

        /// <summary>
        /// This method returns the port number.
        /// </summary>
        /// <param name="portNumber"> The to be parsed number. </param>
        /// <returns> It returns the parsed port number. </returns>
        private static int ReturnPortNumber(string portNumber)
        {
            int port = 80;

            if (!int.TryParse(portNumber, out port))
            {
                throw new ArgumentException("Error the value has to be a number.");
            }

            if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
            {
                throw new ArgumentException("Error the parameter has to contain a valid port number.");
            }

            return port;
        }
    }
}