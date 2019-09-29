//-----------------------------------------------------------------------
// <copyright file="ProcessModuleContainerVm.cs" company="FH Wiener Neustadt">
//     Copyright (c) Emre Rauhofer. All rights reserved.
// </copyright>
// <author>Emre Rauhofer</author>
// <summary>
// This is a dashboard.
// </summary>
//-----------------------------------------------------------------------
namespace ProcessWatcher.ViewModel
{
    using NetworkLibrary;

    /// <summary>
    /// The <see cref="ProcessModuleContainerVm"/> class.
    /// </summary>
    public class ProcessModuleContainerVm
    {
        /// <summary>
        /// The <see cref="ProcessModuleContainer"/>.
        /// </summary>
        private ProcessModuleContainer moduleContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessModuleContainerVm"/> class.
        /// </summary>
        /// <param name="module"> The <see cref="ProcessModule"/>. </param>
        public ProcessModuleContainerVm(ProcessModuleContainer module)
        {
            this.moduleContainer = module;
        }

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        /// <value> A normal string. </value>
        public string Name
        {
            get
            {
                return this.moduleContainer.Name;
            }
        }

        /// <summary>
        /// Gets the path of the module.
        /// </summary>
        /// <value> A normal string. </value>
        public string Path
        {
            get
            {
                return this.moduleContainer.Path;
            }
        }
    }
}
