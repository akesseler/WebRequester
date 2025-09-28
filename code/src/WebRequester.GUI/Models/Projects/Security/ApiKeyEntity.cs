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
using Plexdata.WebRequester.GUI.Helpers.Converters;

namespace Plexdata.WebRequester.GUI.Models.Projects.Security
{
    internal class ApiKeyEntity : NotifiableEntity, ICloneable
    {
        #region Private Fields

        private String key;
        private String value;
        private UsageType usage;

        #endregion

        #region Construction

        public ApiKeyEntity()
            : base()
        {
            this.Key = null;
            this.Value = null;
            this.Usage = UsageType.Query;
        }

        private ApiKeyEntity(ApiKeyEntity other)
            : this()
        {
            this.Key = other.Key;
            this.Value = other.Value;
            this.Usage = other.Usage;
        }

        #endregion

        #region Public Properties

        [JsonProperty(Order = 0)]
        public String Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = value ?? String.Empty;
            }
        }

        [JsonProperty(Order = 1)]
        [JsonConverter(typeof(EncryptedStringConverter))]
        public String Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value ?? String.Empty;

                base.RaisePropertyChanged();
            }
        }

        [JsonProperty(Order = 2)]
        public UsageType Usage
        {
            get
            {
                return this.usage;
            }
            set
            {
                this.usage = this.GetUsageTypeOrDefault(value);

                base.RaisePropertyChanged();
            }
        }

        #endregion

        #region Public Methods

        public Object Clone()
        {
            return new ApiKeyEntity(this);
        }

        #endregion

        #region Private Methods

        private UsageType GetUsageTypeOrDefault(UsageType value)
        {
            if (Enum.IsDefined(value))
            {
                return value;
            }

            return UsageType.Query;
        }

        #endregion
    }
}
