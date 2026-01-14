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
using Plexdata.WebRequester.GUI.Helpers.Converters;
using System.Text;

namespace Plexdata.WebRequester.GUI.Models.Projects.Security
{
    internal class BasicAuthorizationEntity : NotifiableEntity, ICloneable
    {
        #region Private Fields

        private String username;
        private String password;
        private Encoding encoding;

        #endregion

        #region Construction

        public BasicAuthorizationEntity()
            : base()
        {
            this.Username = null;
            this.Password = null;
            this.Encoding = null;
        }

        private BasicAuthorizationEntity(BasicAuthorizationEntity other)
            : this()
        {
            this.Username = other.Username;
            this.Password = other.Password;
            this.Encoding = other.Encoding;
        }

        #endregion

        #region Public Properties

        [JsonProperty(Order = 0)]
        public String Username
        {
            get
            {
                return this.username;
            }
            set
            {
                this.username = value ?? String.Empty;

                base.RaisePropertyChanged();
            }
        }

        [JsonProperty(Order = 1)]
        [JsonConverter(typeof(EncryptedStringConverter))]
        public String Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value ?? String.Empty;

                base.RaisePropertyChanged();
            }
        }

        [JsonProperty(Order = 2)]
        [JsonConverter(typeof(EncodingTypeConverter))]
        public Encoding Encoding
        {
            get
            {
                return this.encoding;
            }
            set
            {
                this.encoding = value ?? Encoding.UTF8;

                base.RaisePropertyChanged();
            }
        }

        #endregion

        #region Public Methods

        public Object Clone()
        {
            return new BasicAuthorizationEntity(this);
        }

        #endregion
    }
}
