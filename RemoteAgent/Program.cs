//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="FH Wiener Neustadt">
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
    /// The <see cref="Program"/> class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry of the app.
        /// </summary>
        /// <param name="args"> The command line arguments. </param>
        private static void Main(string[] args)
        {
            Renderer renderer = new Renderer();
            ApplicationParamsparser settings = new ApplicationParamsparser();

            try
            {
                settings = ApplicationParamsparser.Parse(args);
            }
            catch (Exception e)
            {
                renderer.PrintErrorMessage(e.Message);
                Environment.Exit(1);
            }
            
            App app = new App(settings, renderer);
            app.Start();
        }
    }
}
