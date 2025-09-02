/*
 * MIT License
 * 
 * Copyright (c) 2024 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Plexdata.WebRequester.GUI.Models.Projects;
using WeifenLuo.WinFormsUI.Docking;

namespace Plexdata.WebRequester.GUI.Models.Settings
{
    internal class ProjectSettings
    {
        #region Private Fields

        private List<ProjectEntity> projects;

        #endregion

        #region Construction

        public ProjectSettings()
            : base()
        {
            this.DockState = DockState.DockLeft;
            this.Projects = null;
        }

        #endregion

        #region Public Properties

        public DockState DockState { get; set; }

        public IEnumerable<ProjectEntity> Projects
        {
            get
            {
                return this.projects;
            }
            set
            {
                this.projects = new List<ProjectEntity>();

                if (value != null)
                {
                    foreach (ProjectEntity project in value)
                    {
                        if (project != null)
                        {
                            this.projects.Add(project);
                        }
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        public Boolean CanCheckMenuItem()
        {
            return this.DockState != DockState.Unknown && this.DockState != DockState.Hidden;
        }

        #endregion
    }
}
