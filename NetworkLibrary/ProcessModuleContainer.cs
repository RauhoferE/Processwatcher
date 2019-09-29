//-----------------------------------------------------------------------
// <copyright file="ProcessModuleContainer.cs" company="FH Wiener Neustadt">
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
    using System.Diagnostics;

    /// <summary>
    /// The <see cref="ProcessModuleContainer"/> class.
    /// </summary>
    [Serializable]
    public class ProcessModuleContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessModuleContainer"/> class.
        /// </summary>
        /// <param name="module"> The <see cref="ProcessModule"/>. </param>
        public ProcessModuleContainer(ProcessModule module)
        {
            this.Name = module.ModuleName;
            this.Path = module.FileName;
        }

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        /// <value> A normal string. </value>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the path of the module.
        /// </summary>
        /// <value> A normal string. </value>
        public string Path
        {
            get;
            private set;
        }
    }
}