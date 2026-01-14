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
using Plexdata.WebRequester.GUI.Definitions;
using Plexdata.WebRequester.GUI.Extensions;
using Plexdata.WebRequester.GUI.Helpers.Converters;
using System.Text;

namespace Plexdata.WebRequester.GUI.Models.Projects
{
    internal class PayloadEntity : NotifiableEntity, ICloneable
    {
        #region Private Fields

        private PayloadType sendType;
        private String content = null;
        private String charSet = null;
        private String mimeType = null;

        #endregion

        #region Construction

        public PayloadEntity()
            : base()
        {
            this.SendType = PayloadType.None;
            this.CharSet = null;
            this.MimeType = null;
            this.Content = null;
        }

        private PayloadEntity(PayloadEntity other)
            : this()
        {
            this.SendType = other.SendType;
            this.CharSet = other.CharSet;
            this.MimeType = other.MimeType;
            this.Content = other.Content;
        }

        #endregion

        #region Public Properties

        [JsonProperty(Order = 2)]
        public PayloadType SendType
        {
            get
            {
                return this.sendType;
            }
            set
            {
                this.sendType = this.GetSendTypeOrDefault(value);

                base.RaisePropertyChanged();
            }
        }

        [JsonProperty(Order = 3)]
        public String CharSet
        {
            get
            {
                return this.charSet;
            }
            set
            {
                this.charSet = this.GetCharSetOrDefault(value);

                base.RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        public Encoding Encoding
        {
            get
            {
                return this.GetEncodingFromCharSet(this.charSet);
            }
        }

        [JsonProperty(Order = 4)]
        public String MimeType
        {
            get
            {
                return this.mimeType;
            }
            set
            {
                this.mimeType = this.GetMimeTypeOrDefault(value);

                base.RaisePropertyChanged();
            }
        }

        [JsonProperty(Order = 5)]
        [JsonConverter(typeof(CompressedStringConverter))]
        public String Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value ?? String.Empty;

                base.RaisePropertyChanged();
            }
        }

        #endregion

        #region Public Methods

        public Object Clone()
        {
            return new PayloadEntity(this);
        }

        #endregion

        #region Private Methods

        private PayloadType GetSendTypeOrDefault(PayloadType value)
        {
            if (Enum.IsDefined<PayloadType>(value))
            {
                return value;
            }

            return PayloadType.None;
        }

        #endregion
    }
}
