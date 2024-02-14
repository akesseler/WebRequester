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
using Plexdata.WebRequester.GUI.Models.Projects.Security;
using System.Diagnostics;

namespace Plexdata.WebRequester.GUI.Models.Projects
{
    [DebuggerDisplay("{GetDebuggerDisplay(),nq}")]
    internal class SecurityEntity : NotifiableEntity, ICloneable
    {
        #region Private Fields

        private AuthorizationType authorizationType;
        private ApiKeyEntity apiKey;
        private BearerTokenEntity bearerToken;
        private BasicAuthorizationEntity basicAuthorization;

        #endregion

        #region Construction

        public SecurityEntity(AuthorizationType type)
            : base()
        {
            this.AuthorizationType = type;
            this.ApiKey = null;
            this.BearerToken = null;
            this.BasicAuthorization = null;
        }

        private SecurityEntity(SecurityEntity other)
            : base()
        {
            this.AuthorizationType = other.AuthorizationType;
            this.ApiKey = (ApiKeyEntity)other.ApiKey.Clone();
            this.BearerToken = (BearerTokenEntity)other.BearerToken.Clone();
            this.BasicAuthorization = (BasicAuthorizationEntity)other.BasicAuthorization.Clone();
        }

        #endregion

        #region Public Properties

        [JsonProperty(Order = 2)]
        public AuthorizationType AuthorizationType
        {
            get
            {
                return this.authorizationType;
            }
            set
            {
                this.authorizationType = this.GetAuthorizationTypeOrDefault(value);

                base.RaisePropertyChanged();
            }
        }

        [JsonProperty(Order = 3)]
        public ApiKeyEntity ApiKey
        {
            get
            {
                return this.apiKey;
            }
            set
            {
                this.apiKey = value ?? new ApiKeyEntity();

                base.RaisePropertyChanged();
            }
        }

        [JsonProperty(Order = 4)]
        public BearerTokenEntity BearerToken
        {
            get
            {
                return this.bearerToken;
            }
            set
            {
                this.bearerToken = value ?? new BearerTokenEntity();

                base.RaisePropertyChanged();
            }
        }

        [JsonProperty(Order = 5)]
        public BasicAuthorizationEntity BasicAuthorization
        {
            get
            {
                return this.basicAuthorization;
            }
            set
            {
                this.basicAuthorization = value ?? new BasicAuthorizationEntity();

                base.RaisePropertyChanged();
            }
        }

        #endregion

        #region Public Methods

        public Object Clone()
        {
            return new SecurityEntity(this);
        }

        #endregion

        #region Debugging Helper

        private  String GetDebuggerDisplay()
        {
            return $"{nameof(this.AuthorizationType)}: {this.AuthorizationType}";
        }

        #endregion

        #region Private Methods

        private AuthorizationType GetAuthorizationTypeOrDefault(AuthorizationType value)
        {
            if (Enum.IsDefined<AuthorizationType>(value))
            {
                return value;
            }

            return AuthorizationType.InheritFromParent;
        }

        #endregion
    }
}
