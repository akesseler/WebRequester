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

namespace Plexdata.WebRequester.GUI.Models.Projects
{
    internal class HeaderEntity : NotifiableEntity, ICloneable
    {
        #region Private Fields

        private List<ActionEntity> headers;

        #endregion

        #region Construction

        public HeaderEntity()
            : base()
        {
            this.Headers = null;
        }

        private HeaderEntity(HeaderEntity other)
            : this()
        {
            this.Headers = other.Headers.Select(x => (ActionEntity)x.Clone());
        }

        #endregion

        #region Public Properties

        [JsonProperty(Order = 2)]
        public IEnumerable<ActionEntity> Headers
        {
            get
            {
                return this.headers;
            }
            set
            {
                this.headers = (value ??= Enumerable.Empty<ActionEntity>()).ToList();

                base.RaisePropertyChanged();
            }
        }

        #endregion

        #region Public Methods

        public Object Clone()
        {
            return new HeaderEntity(this);
        }

        #endregion
    }
}
