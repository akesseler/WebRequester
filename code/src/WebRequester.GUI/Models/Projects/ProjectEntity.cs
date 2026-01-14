/*
 * MIT License
 * 
 * Copyright (c) 2026 plexdata.de
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

using Newtonsoft.Json;

namespace Plexdata.WebRequester.GUI.Models.Projects
{
    internal class ProjectEntity : LabeledEntity, ICloneable
    {
        #region Private Fields

        private List<SectionEntity> sections;

        #endregion

        #region Construction

        public ProjectEntity()
            : this("Unnamed Project")
        {
        }

        public ProjectEntity(String label)
            : base(label)
        {
            this.Sections = null;
        }

        private ProjectEntity(ProjectEntity other)
            : base(other)
        {
            this.Sections = other.Sections.Select(x => (SectionEntity)x.Clone());
        }

        #endregion

        #region Public Properties

        [JsonProperty(Order = 2)]
        public IEnumerable<SectionEntity> Sections
        {
            get
            {
                return this.sections;
            }
            set
            {
                this.sections = (value ??= Enumerable.Empty<SectionEntity>()).ToList();

                base.RaisePropertyChanged();
            }
        }

        #endregion

        #region Public Methods

        public Object Clone()
        {
            return new ProjectEntity(this);
        }

        #endregion
    }
}
