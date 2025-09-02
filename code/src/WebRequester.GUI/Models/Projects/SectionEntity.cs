/*
 * MIT License
 * 
 * Copyright (c) 2025 plexdata.de
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
using Plexdata.WebRequester.GUI.Definitions;
using Plexdata.WebRequester.GUI.Interfaces;

namespace Plexdata.WebRequester.GUI.Models.Projects
{
    internal class SectionEntity : LabeledEntity, ICloneable, IManageableEntity<ISectionDocument>
    {
        #region Private Fields

        private Boolean visible;
        private List<RequestEntity> requests;
        private List<VariableEntity> variables;
        private SecurityEntity security;

        #endregion

        #region Construction

        public SectionEntity()
            : this("Unnamed Section")
        {
        }

        public SectionEntity(String label)
            : base(label)
        {
            this.Requests = null;
            this.Variables = null;
            this.Security = null;
        }

        private SectionEntity(SectionEntity other)
            : base(other)
        {
            this.Requests = other.Requests.Select(x => (RequestEntity)x.Clone());
            this.Variables = other.Variables.Select(x => (VariableEntity)x.Clone());
            this.Security = (SecurityEntity)other.Security.Clone();
        }

        #endregion

        #region Public Properties

        [JsonIgnore]
        public TreeNode TreeNode { get; set; }

        [JsonIgnore]
        public ISectionDocument Document { get; set; }

        [JsonProperty(Order = 2)]
        public Boolean Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                this.visible = value;

                base.RaisePropertyChanged();
            }
        }

        [JsonProperty(Order = 3)]
        public IEnumerable<RequestEntity> Requests
        {
            get
            {
                return this.requests;
            }
            set
            {
                this.requests = (value ??= Enumerable.Empty<RequestEntity>()).ToList();

                base.RaisePropertyChanged();
            }
        }

        [JsonProperty(Order = 4)]
        public IEnumerable<VariableEntity> Variables
        {
            get
            {
                return this.variables;
            }
            set
            {
                this.variables = (value ??= Enumerable.Empty<VariableEntity>()).ToList();

                base.RaisePropertyChanged();
            }
        }

        [JsonProperty(Order = 5)]
        public SecurityEntity Security
        {
            get
            {
                return this.security;
            }
            set
            {
                this.security = value ?? new SecurityEntity(AuthorizationType.NoAuthorization);

                base.RaisePropertyChanged();
            }
        }

        #endregion

        #region Public Methods

        public Object Clone()
        {
            return new SectionEntity(this);
        }

        public void SaveValues()
        {
            this.Document?.SaveEntityValues();
        }

        #endregion
    }
}
