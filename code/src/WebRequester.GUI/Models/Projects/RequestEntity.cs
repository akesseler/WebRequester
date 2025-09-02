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

using Newtonsoft.Json;
using Plexdata.WebRequester.GUI.Definitions;
using Plexdata.WebRequester.GUI.Extensions;
using Plexdata.WebRequester.GUI.Interfaces;

namespace Plexdata.WebRequester.GUI.Models.Projects
{
    internal class RequestEntity : LabeledEntity, ICloneable, IManageableEntity<IRequestDocument>
    {
        #region Private Fields

        private Boolean visible;
        private String method;
        private String url;
        private QueryEntity query;
        private SecurityEntity security;
        private HeaderEntity header;
        private PayloadEntity payload;

        #endregion

        #region Construction

        public RequestEntity()
            : this("Unnamed Request")
        {
        }

        public RequestEntity(String label)
            : base(label)
        {
            this.Visible = false;
            this.Method = null;
            this.Url = null;
            this.Query = null;
            this.Security = null;
            this.Header = null;
            this.Payload = null;
        }

        private RequestEntity(RequestEntity other)
            : base(other)
        {
            this.Visible = other.Visible;
            this.Method = other.Method;
            this.Url = other.Url;
            this.Query = (QueryEntity)other.Query.Clone();
            this.Security = (SecurityEntity)other.Security.Clone();
            this.Header = (HeaderEntity)other.Header.Clone();
            this.Payload = (PayloadEntity)other.Payload.Clone();
        }

        #endregion

        #region Public Properties

        [JsonIgnore]
        public TreeNode TreeNode { get; set; }

        [JsonIgnore]
        public IRequestDocument Document { get; set; }

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
        public String Method
        {
            get
            {
                return this.method;
            }
            set
            {
                this.method = this.HttpMethodStringOrDefault(value);

                base.RaisePropertyChanged();
            }
        }

        [JsonProperty(Order = 4)]
        public String Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value ?? String.Empty;

                base.RaisePropertyChanged();
            }
        }

        [JsonProperty(Order = 5)]
        public QueryEntity Query
        {
            get
            {
                return this.query;
            }
            set
            {
                this.query = value ?? new QueryEntity();

                base.RaisePropertyChanged();
            }
        }

        [JsonProperty(Order = 6)]
        public SecurityEntity Security
        {
            get
            {
                return this.security;
            }
            set
            {
                this.security = value ?? new SecurityEntity(AuthorizationType.InheritFromParent);

                base.RaisePropertyChanged();
            }
        }

        [JsonProperty(Order = 7)]
        public HeaderEntity Header
        {
            get
            {
                return this.header;
            }
            set
            {
                this.header = value ?? new HeaderEntity();

                base.RaisePropertyChanged();
            }
        }

        [JsonProperty(Order = 8)]
        public PayloadEntity Payload
        {
            get
            {
                return this.payload;
            }
            set
            {
                this.payload = value ?? new PayloadEntity();

                base.RaisePropertyChanged();
            }
        }

        #endregion

        #region Public Methods

        public static RequestEntity CreateDefault()
        {
            return new RequestEntity()
            {
                Header = RequestEntity.GetDefaultHeaderEntity()
            };
        }

        public Object Clone()
        {
            return new RequestEntity(this);
        }

        public void SaveValues()
        {
            this.Document?.SaveEntityValues();
        }

        #endregion

        #region Private Methods

        private static HeaderEntity GetDefaultHeaderEntity()
        {
            HeaderEntity result = new HeaderEntity();

            result.Headers = new ActionEntity[]
            {
                new ActionEntity(true, "Connection", "keep-alive", "Instructs the server to keep connections open."),
                new ActionEntity(true, "User-Agent", RequestEntity.GetUserAgent(), "Identifies this application to the server."),
                new ActionEntity(true, "Accept", "*/*", "Tells the server that all types of responses can be processed.")
            };

            return result;
        }

        private static String GetUserAgent()
        {
            return $"WebRequesterRuntime/{Application.ProductVersion}";
        }

        #endregion
    }
}
