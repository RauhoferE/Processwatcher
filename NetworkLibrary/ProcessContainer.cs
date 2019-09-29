//-----------------------------------------------------------------------
// <copyright file="ProcessContainer.cs" company="FH Wiener Neustadt">
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
    using System.Diagnostics;

    /// <summary>
    /// The <see cref="ProcessContainer"/> class.
    /// </summary>
    [Serializable]
    public class ProcessContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessContainer"/> class.
        /// </summary>
        /// <param name="process"> The process to be contained. </param>
        public ProcessContainer(Process process)
        {
            try
            {
                this.Memory = Convert.ToInt32(process.WorkingSet64);
            }
            catch (Exception)
            {
                this.Memory = 0;
            }

            try
            {
                this.Name = process.ProcessName;
            }
            catch (Exception)
            {
                this.Name = string.Empty;
            }

            try
            {
                this.Id = process.Id;
            }
            catch (Exception)
            {
                this.Id = 0;
            }

            try
            {
                this.StartDate = process.StartTime;
            }
            catch (Exception)
            {
                this.StartDate = new DateTime();
            }

            try
            {
                this.Title = process.MainModule.FileVersionInfo.FileDescription;
            }
            catch (Exception)
            {
                this.Title = string.Empty;
            }

            try
            {
                this.Priority = process.BasePriority;
            }
            catch (Exception)
            {
                this.Priority = 0;
            }
            
            this.Modules = new List<ProcessModuleContainer>();

            try
            {
                foreach (var item in process.Modules)
                {
                    var item2 = (ProcessModule)item;
                    this.Modules.Add(new ProcessModuleContainer(item2));
                }
            }
            catch (Exception)
            {
                this.Modules = null;
            }
        }

        /// <summary>
        /// Gets the name of the process.
        /// </summary>
        /// <value> A normal string. </value>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the id of the process.
        /// </summary>
        /// <value> A normal integer. </value>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the memory size of the process.
        /// </summary>
        /// <value> A normal integer. </value>
        public int Memory
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the start time of the process.
        /// </summary>
        /// <value> A normal <see cref="DateTime"/>. </value>
        public DateTime StartDate
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the title of the process.
        /// </summary>
        /// <value> A normal string. </value>
        public string Title
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the priority of the process.
        /// </summary>
        /// <value> A normal integer. </value>
        public int Priority
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the modules of the Process.
        /// </summary>
        /// <value> A normal list of modules. </value>
        public List<ProcessModuleContainer> Modules
        {
            get;
            private set;
        }

        /// <summary>
        /// This method compares two processes.
        /// </summary>
        /// <param name="newProcess"> The new process. </param>
        /// <returns> It returns true if the process is different. </returns>
        public bool IsProcessDifferent(ProcessContainer newProcess)
        {
            if (this.Priority != newProcess.Priority)
            {
                return true;
            }

            if (this.Modules != newProcess.Modules)
            {
                return true;
            }

            if (this.StartDate != newProcess.StartDate)
            {
                return true;
            }

            if (this.Title != newProcess.Title)
            {
                return true;
            }

            if (this.Name != newProcess.Name)
            {
                return true;
            }

            return false;
        }
    }
}