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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Extensions;
using Plexdata.WebRequester.GUI.Definitions;
using Plexdata.WebRequester.GUI.Extensions;
using Plexdata.WebRequester.GUI.Interfaces;
using Plexdata.WebRequester.GUI.Models.Projects;
using Plexdata.WebRequester.GUI.Models.Projects.Security;
using System.Text;

namespace Plexdata.WebRequester.GUI.Helpers.Importers
{
    internal class PostmanImporter : IPostmanImporter
    {
        #region Private Fields

        private readonly ILogger logger;
        private readonly ISerializer serializer;
        private readonly StringComparison comparison = StringComparison.OrdinalIgnoreCase;

        private Int32 warnings = 0;
        private Int32 errors = 0;

        #endregion

        #region Construction

        public PostmanImporter(ILogger logger, ISerializer serializer)
            : base()
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        #endregion

        #region Public Properties

        public Int32 Warnings
        {
            get
            {
                return this.warnings;
            }
        }

        public Int32 Errors
        {
            get
            {
                return this.errors;
            }
        }

        #endregion

        #region Public Methods

        public IEnumerable<ProjectEntity> Import(String filename)
        {
            if (String.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentOutOfRangeException(nameof(filename), $"Value of '{nameof(filename)}' must not be null, empty or whitespace.");
            }

            String content = this.ReadContent(filename);

            if (content.IndexOf("_postman_id", this.comparison) < 0)
            {
                throw new FileFormatException("Postman file indicator not found.");
            }

            content = content.Replace("{{", "<<").Replace("}}", ">>");

            PostmanDataModel import = this.serializer.Deserialize<PostmanDataModel>(content);

            import = this.ValidateAndSanitize(import);

            return new ProjectEntity[]
            {
                new ProjectEntity()
                {
                    Label = "Postman Import",
                    Notes = $"Collection imported on {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}",
                    Sections = new SectionEntity[]
                    {
                        this.Convert(import)
                    }
                }
            };
        }

        #endregion

        #region Validation and Sanitization

        private PostmanDataModel ValidateAndSanitize(PostmanDataModel value)
        {
            if (value == null)
            {
                return null;
            }

            value.Items = this.ValidateAndSanitize(value.Items);
            value.Authorization = this.ValidateAndSanitize(value.Authorization);
            value.Variables = this.ValidateAndSanitize(value.Variables);

            return value;
        }

        private IEnumerable<ItemModel> ValidateAndSanitize(IEnumerable<ItemModel> values)
        {
            if (values == null)
            {
                return null;
            }

            Int32 count = values.Where(x => String.IsNullOrWhiteSpace(x.Name)).Count();

            if (count > 0)
            {
                this.warnings++;
                this.logger.Warning($"Found {count} {(count == 1 ? "entry" : "entries")} without a valid name.");
            }

            foreach (ItemModel value in values)
            {
                value.Items = this.ValidateAndSanitize(value.Items);
                value.Authorization = this.ValidateAndSanitize(value.Authorization);
                value.Request = this.ValidateAndSanitize(value.Name, value.Request);
            }

            return values;
        }

        private RequestModel ValidateAndSanitize(String name, RequestModel value)
        {
            if (value == null)
            {
                return null;
            }

            Boolean allowed = this.AllowedHttpMethods().Contains(value.Method, StringComparer.FromComparison(this.comparison));

            if (!allowed)
            {
                this.warnings++;
                this.logger.Warning($"Request '{name}' contains the unsupported method '{value.Method}'.");
                value.Method = this.HttpMethodStringOrDefault(value.Method);
            }

            value.Authorization = this.ValidateAndSanitize(value.Authorization);
            value.Headers = this.ValidateAndSanitize(value.Headers);
            value.Url = this.ValidateAndSanitize(value.Url);
            value.Body = this.ValidateAndSanitize(value.Body);

            return value;
        }

        private IEnumerable<HeaderModel> ValidateAndSanitize(IEnumerable<HeaderModel> values)
        {
            if (values == null)
            {
                return null;
            }

            Int32 count = values.Where(x => String.IsNullOrWhiteSpace(x.Key)).Count();

            if (count > 0)
            {
                this.warnings++;
                this.logger.Warning($"Found {count} {(count == 1 ? "header" : "headers")} without a valid key.");
            }

            return values;
        }

        private UrlModel ValidateAndSanitize(UrlModel value)
        {
            if (value == null)
            {
                return null;
            }

            if (String.IsNullOrWhiteSpace(value.Raw))
            {
                value.Raw = String.Empty;
            }

            if (String.IsNullOrWhiteSpace(value.Protocol))
            {
                value.Protocol = String.Empty;
            }

            String[] hosts = (value.Host ?? Enumerable.Empty<String>()).ToArray();

            for (Int32 index = 0; index < hosts.Length; index++)
            {
                hosts[index] = (hosts[index] ?? String.Empty).Trim();
            }

            value.Host = hosts;

            if (String.IsNullOrWhiteSpace(value.Port))
            {
                value.Port = String.Empty;
            }

            String[] paths = (value.Path ?? Enumerable.Empty<String>()).ToArray();

            for (Int32 index = 0; index < paths.Length; index++)
            {
                paths[index] = String.Format("/{0}", (paths[index] ?? String.Empty).Trim());
            }

            value.Path = paths;

            StringBuilder builder = new StringBuilder();

            if (!String.IsNullOrWhiteSpace(value.Protocol))
            {
                builder.AppendFormat("{0}://", value.Protocol);
            }

            if (value.Host.Any())
            {
                builder.Append(String.Join(".", value.Host));
            }

            if (!String.IsNullOrWhiteSpace(value.Port))
            {
                builder.AppendFormat(":{0}", value.Port);
            }

            if (value.Path.Any())
            {
                builder.Append(String.Join(String.Empty, value.Path));
            }

            value.Url = builder.ToString();

            if (!value.Raw.StartsWith(value.Url))
            {
                this.warnings++;
                this.logger.Warning("Mismatch between raw data and the generated URL.", (UrlModel.RawTag, value.Raw), ("URL", value.Url));
            }
            else
            {
                this.logger.Trace("Able to build URL from raw data.", (UrlModel.RawTag, value.Raw), ("URL", value.Url));
            }

            value.Queries = this.ValidateAndSanitize(value.Queries);

            return value;
        }

        private IEnumerable<QueryModel> ValidateAndSanitize(IEnumerable<QueryModel> values)
        {
            if (values == null)
            {
                return null;
            }

            Int32 count = values.Where(x => String.IsNullOrWhiteSpace(x.Key)).Count();

            if (count > 0)
            {
                this.warnings++;
                this.logger.Warning($"Found {count} query {(count == 1 ? "parameter" : "parameters")} without a valid key.");
            }

            return values;
        }

        private BodyModel ValidateAndSanitize(BodyModel value)
        {
            if (value == null)
            {
                return new BodyModel()
                {
                    Mode = BodyModel.NoneMode,
                    Raw = null,
                    Options = null,
                };
            }

            Boolean supported =
                this.IsEqual(value.Mode, BodyModel.NoneMode) ||
                this.IsEqual(value.Mode, BodyModel.RawMode);

            if (!supported)
            {
                this.errors++;
                this.logger.Error($"Imported body mode '{value.Mode}' is not supported and will not be used.");
                return null;
            }

            if (this.IsEqual(value.Mode, BodyModel.NoneMode))
            {
                value.Raw = null;
                value.Options = null;

                return value;
            }

            if (this.IsEqual(value.Mode, BodyModel.RawMode))
            {
                value.Raw = (value.Raw ?? String.Empty).ReplaceLineEndings();
                value.Options = this.ValidateAndSanitize(value.Options);

                return value;
            }

            return value;
        }

        private OptionsModel ValidateAndSanitize(OptionsModel value)
        {
            if (value == null)
            {
                // Missing Options model means raw type is "text".
                value = new OptionsModel() { Raw = new RawOptionsModel() { Language = "text" } };
                // This "fall through" is important!
            }

            value.Raw = this.ValidateAndSanitize(value.Raw);

            return value;
        }

        private RawOptionsModel ValidateAndSanitize(RawOptionsModel value)
        {
            if (value == null)
            {
                return null;
            }

            Boolean supported =
                this.IsEqual(value.Language, "xml") ||
                this.IsEqual(value.Language, "text") ||
                this.IsEqual(value.Language, "html") ||
                this.IsEqual(value.Language, "json") ||
                this.IsEqual(value.Language, "javascript");

            if (!supported)
            {
                this.errors++;
                this.logger.Error($"Imported language type '{value.Language}' is not supported and will not be used.");
                return null;
            }

            if (this.IsEqual(value.Language, "xml"))
            {
                value.Language = "application/xml";
            }
            else if (this.IsEqual(value.Language, "text"))
            {
                value.Language = "text/plain";
            }
            else if (this.IsEqual(value.Language, "html"))
            {
                value.Language = "text/html";
            }
            else if (this.IsEqual(value.Language, "json"))
            {
                value.Language = "application/json";
            }
            else if (this.IsEqual(value.Language, "javascript"))
            {
                value.Language = "application/javascript";
            }

            return value;
        }

        private AuthorizationModel ValidateAndSanitize(AuthorizationModel value)
        {
            if (value == null)
            {
                return null;
            }

            if (String.IsNullOrWhiteSpace(value.Type))
            {
                this.errors++;
                this.logger.Error("An authorization type could not be determined from the imported data.");
                return null;
            }

            Boolean supported =
                this.IsEqual(value.Type, AuthorizationModel.NoAuthType) ||
                this.IsEqual(value.Type, AuthorizationModel.ApiKeyType) ||
                this.IsEqual(value.Type, AuthorizationModel.BearerType) ||
                this.IsEqual(value.Type, AuthorizationModel.BasicType);

            if (!supported)
            {
                this.errors++;
                this.logger.Error($"Imported authorization type '{value.Type}' is not supported and will not be used.");
                return null;
            }

            // Under some circumstances some values are missing. In such a
            // case, simply add them with their assumed default settings.

            if (this.IsEqual(value.Type, AuthorizationModel.NoAuthType))
            {
                value.ApiKey = null;
                value.Bearer = null;
                value.Basic = null;

                return value;
            }

            if (this.IsEqual(value.Type, AuthorizationModel.ApiKeyType))
            {
                if (value.ApiKey == null)
                {
                    this.logger.Trace($"Value of '{AuthorizationModel.ApiKeyType}' is not set.");
                    value.ApiKey = Enumerable.Empty<KeyValueModel>();
                }

                if (!value.ApiKey.Any(x => this.IsEqual(x.Key, ApiKeyModelConverter.InTag)))
                {
                    this.logger.Trace($"Value of item '{ApiKeyModelConverter.InTag}' is not set. Append default.");
                    value.ApiKey = value.ApiKey.Append(new KeyValueModel(ApiKeyModelConverter.InTag, "header"));
                }

                if (!value.ApiKey.Any(x => this.IsEqual(x.Key, ApiKeyModelConverter.KeyTag)))
                {
                    this.logger.Trace($"Value of item '{ApiKeyModelConverter.KeyTag}' is not set. Append default.");
                    value.ApiKey = value.ApiKey.Append(new KeyValueModel(ApiKeyModelConverter.KeyTag));
                }

                if (!value.ApiKey.Any(x => this.IsEqual(x.Key, ApiKeyModelConverter.ValueTag)))
                {
                    this.logger.Trace($"Value of item '{ApiKeyModelConverter.ValueTag}' is not set. Append default.");
                    value.ApiKey = value.ApiKey.Append(new KeyValueModel(ApiKeyModelConverter.ValueTag));
                }

                value.Bearer = null;
                value.Basic = null;

                return value;
            }

            if (this.IsEqual(value.Type, AuthorizationModel.BearerType))
            {
                if (value.Bearer == null)
                {
                    this.logger.Trace($"Value of '{AuthorizationModel.BearerType}' is not set.");
                    value.Bearer = Enumerable.Empty<KeyValueModel>();
                }

                if (!value.Bearer.Any(x => this.IsEqual(x.Key, BearerModelConverter.TokenTag)))
                {
                    this.logger.Trace($"Value of item '{BearerModelConverter.TokenTag}' is not set. Append default.");
                    value.Bearer = value.Bearer.Append(new KeyValueModel(BearerModelConverter.TokenTag));
                }

                value.ApiKey = null;
                value.Basic = null;

                return value;
            }

            if (this.IsEqual(value.Type, AuthorizationModel.BasicType))
            {
                if (value.Basic == null)
                {
                    this.logger.Trace($"Value of '{AuthorizationModel.BasicType}' is not set.");
                    value.Basic = Enumerable.Empty<KeyValueModel>();
                }

                if (!value.Basic.Any(x => this.IsEqual(x.Key, BasicModelConverter.UsernameTag)))
                {
                    this.logger.Trace($"Value of item '{BasicModelConverter.UsernameTag}' is not set. Append default.");
                    value.Basic = value.Basic.Append(new KeyValueModel(BasicModelConverter.UsernameTag));
                }

                if (!value.Basic.Any(x => this.IsEqual(x.Key, BasicModelConverter.PasswordTag)))
                {
                    this.logger.Trace($"Value of item '{BasicModelConverter.PasswordTag}' is not set. Append default.");
                    value.Basic = value.Basic.Append(new KeyValueModel(BasicModelConverter.PasswordTag));
                }

                value.ApiKey = null;
                value.Bearer = null;

                return value;
            }

            return value;
        }

        private IEnumerable<VariableModel> ValidateAndSanitize(IEnumerable<VariableModel> values)
        {
            if (values == null)
            {
                return null;
            }

            Int32 count = values.Where(x => String.IsNullOrWhiteSpace(x.Key)).Count();

            if (count > 0)
            {
                this.warnings++;
                this.logger.Warning($"Found {count} {(count == 1 ? "variable" : "variables")} without a valid key.");
            }

            return values;
        }

        #endregion

        #region Postman Model Converters

        // Since version 2.1, key-value-pairs are used for a significant number of properties. In
        // previous versions, objects with corresponding values are used instead. What a shitty
        // mismatch for a minor version change. And I'm pretty sure I didn't catch all of them...

        private class UrlModelConverter : JsonConverter
        {
            public override Boolean CanConvert(Type objectType)
            {
                return true;
            }

            public override Object? ReadJson(JsonReader reader, Type objectType, Object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.String:
                        {
                            String value = (String)reader.Value;

                            return new UrlModel()
                            {
                                Raw = value,
                                Host = new String[] { value }
                            };
                        }
                    case JsonToken.StartObject:
                        {
                            JObject value = JObject.Load(reader);

                            return new UrlModel()
                            {
                                Raw = (String)value[UrlModel.RawTag],
                                Protocol = (String)value[UrlModel.ProtocolTag],
                                Host = ((JArray)value[UrlModel.HostTag])?.ToObject<IEnumerable<String>>(),
                                Port = (String)value[UrlModel.PortTag],
                                Path = ((JArray)value[UrlModel.PathTag])?.ToObject<IEnumerable<String>>(),
                                Queries = ((JArray)value[UrlModel.QueryTag])?.ToObject<IEnumerable<QueryModel>>()
                            };
                        }
                }

                throw new NotSupportedException($"Token type '{reader.TokenType}' is not supported to deserialize a URL model.");
            }

            public override void WriteJson(JsonWriter writer, Object? value, Newtonsoft.Json.JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }

        public class ApiKeyModelConverter : JsonConverter
        {
            public const String InTag = "in";
            public const String KeyTag = "key";
            public const String ValueTag = "value";

            public override Boolean CanConvert(Type objectType)
            {
                return true;
            }

            public override Object? ReadJson(JsonReader reader, Type objectType, Object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartArray:
                        {
                            JArray value = JArray.Load(reader);

                            return value?.ToObject<IEnumerable<KeyValueModel>>();
                        }
                    case JsonToken.StartObject:
                        {
                            JObject value = JObject.Load(reader);

                            return new KeyValueModel[]
                            {
                                new KeyValueModel(ApiKeyModelConverter.InTag, (String)value[ApiKeyModelConverter.InTag]),
                                new KeyValueModel(ApiKeyModelConverter.KeyTag, (String)value[ApiKeyModelConverter.KeyTag]),
                                new KeyValueModel(ApiKeyModelConverter.ValueTag, (String)value[ApiKeyModelConverter.ValueTag])
                            };
                        }
                }

                throw new NotSupportedException($"Token type '{reader.TokenType}' is not supported to deserialize a bearer authorization model.");
            }

            public override void WriteJson(JsonWriter writer, Object? value, Newtonsoft.Json.JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }

        public class BearerModelConverter : JsonConverter
        {
            public const String TokenTag = "token";

            public override Boolean CanConvert(Type objectType)
            {
                return true;
            }

            public override Object? ReadJson(JsonReader reader, Type objectType, Object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartArray:
                        {
                            JArray value = JArray.Load(reader);

                            return value?.ToObject<IEnumerable<KeyValueModel>>();
                        }
                    case JsonToken.StartObject:
                        {
                            JObject value = JObject.Load(reader);

                            return new KeyValueModel[]
                            {
                                new KeyValueModel(BearerModelConverter.TokenTag, (String)value[BearerModelConverter.TokenTag])
                            };
                        }
                }

                throw new NotSupportedException($"Token type '{reader.TokenType}' is not supported to deserialize a bearer authorization model.");
            }

            public override void WriteJson(JsonWriter writer, Object? value, Newtonsoft.Json.JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }

        private class BasicModelConverter : JsonConverter
        {
            public const String UsernameTag = "username";
            public const String PasswordTag = "password";

            public override Boolean CanConvert(Type objectType)
            {
                return true;
            }

            public override Object? ReadJson(JsonReader reader, Type objectType, Object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartArray:
                        {
                            JArray value = JArray.Load(reader);

                            return value?.ToObject<IEnumerable<KeyValueModel>>();
                        }
                    case JsonToken.StartObject:
                        {
                            JObject value = JObject.Load(reader);

                            return new KeyValueModel[]
                            {
                                new KeyValueModel(BasicModelConverter.UsernameTag, (String)value[BasicModelConverter.UsernameTag]),
                                new KeyValueModel(BasicModelConverter.PasswordTag, (String)value[BasicModelConverter.PasswordTag])
                            };
                        }
                }

                throw new NotSupportedException($"Token type '{reader.TokenType}' is not supported to deserialize a basic authorization model.");
            }

            public override void WriteJson(JsonWriter writer, Object? value, Newtonsoft.Json.JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Result Model Converters

        private SectionEntity Convert(PostmanDataModel value)
        {
            if (value == null)
            {
                return null;
            }

            StringBuilder builder = new StringBuilder();

            if (!String.IsNullOrWhiteSpace(value.Info?.PostmanId))
            {
                builder.AppendLine(String.Format("Postman-ID: {0}", value.Info.PostmanId));
            }

            if (!String.IsNullOrWhiteSpace(value.Info?.Schema))
            {
                builder.AppendLine(String.Format("Schema: {0}", value.Info.Schema));
            }

            if (!String.IsNullOrWhiteSpace(value.Info?.ExporterId))
            {
                builder.AppendLine(String.Format("Exporter-ID: {0}", value.Info.ExporterId));
            }

            SectionEntity result = new SectionEntity()
            {
                Label = value.Info?.Name ?? "Unnamed Section",
                Notes = builder.ToString()
            };

            result.Visible = false;
            result.Requests = this.Convert(value.Items, null, null);
            result.Variables = this.Convert(value.Variables);
            result.Security = this.Convert(value.Authorization);

            return result;
        }

        private List<RequestEntity> Convert(IEnumerable<ItemModel> values, SecurityEntity inherit, List<RequestEntity> results)
        {
            if (results == null)
            {
                results = new List<RequestEntity>();
            }

            if (!values?.Any() ?? true)
            {
                return results;
            }

            foreach (ItemModel value in values)
            {
                // Each locally assigned authorization wins against any potentially
                // inherited authorization! Hopefully I'm right with this theory.

                SecurityEntity security = this.Convert(value.Authorization);

                results = this.Convert(value.Items, security != null ? security : inherit, results);

                RequestEntity request = this.Convert(value.Request, value.Name, security != null ? security : inherit);

                if (request != null)
                {
                    results.Add(request);
                }
            }

            return results;
        }

        private RequestEntity Convert(RequestModel value, String name, SecurityEntity inherit)
        {
            if (value == null)
            {
                return null;
            }

            RequestEntity result;

            if (String.IsNullOrWhiteSpace(name))
            {
                result = new RequestEntity();
            }
            else
            {
                result = new RequestEntity(name);
            }

            SecurityEntity security = this.Convert(value.Authorization);

            result.Visible = false;
            result.Method = this.HttpMethodStringOrDefault(value.Method);
            result.Url = value.Url?.Url;
            result.Query = this.Convert(value.Url?.Queries);
            result.Security = security != null ? security : inherit;
            result.Header = this.Convert(value.Headers);
            result.Payload = this.Convert(value.Body);

            return result;
        }

        private QueryEntity Convert(IEnumerable<QueryModel> values)
        {
            if (!values?.Any() ?? true)
            {
                return null;
            }

            return new QueryEntity()
            {
                Parameters = values.Select(x => new ActionEntity(!x.Disabled, x.Key, x.Value)).ToArray()
            };
        }

        private HeaderEntity Convert(IEnumerable<HeaderModel> values)
        {
            if (!values?.Any() ?? true)
            {
                return null;
            }

            return new HeaderEntity()
            {
                Headers = values.Select(x => new ActionEntity(!x.Disabled, x.Key, x.Value)).ToArray()
            };
        }

        private PayloadEntity Convert(BodyModel value)
        {
            if (value == null)
            {
                return null;
            }

            if (this.IsEqual(value.Mode, BodyModel.NoneMode))
            {
                return new PayloadEntity()
                {
                    SendType = PayloadType.None,
                    CharSet = this.GetCharSetOrDefault(null),
                    MimeType = this.GetMimeTypeOrDefault(null),
                    Content = null
                };
            }

            if (this.IsEqual(value.Mode, BodyModel.RawMode))
            {
                return new PayloadEntity()
                {
                    SendType = PayloadType.Text,
                    CharSet = this.GetCharSetOrDefault(null),
                    MimeType = this.GetMimeTypeOrDefault(value.Options.Raw.Language),
                    Content = value.Raw
                };
            }

            return null;
        }

        private IEnumerable<VariableEntity> Convert(IEnumerable<VariableModel> value)
        {
            if (!value?.Any() ?? true)
            {
                return null;
            }

            return value
                .Select(x => new VariableEntity(!x.Disabled, x.Key, x.Value))
                .ToArray();
        }

        private SecurityEntity Convert(AuthorizationModel value)
        {
            if (value == null)
            {
                return null;
            }

            switch (value.Type)
            {
                case AuthorizationModel.NoAuthType:

                    return new SecurityEntity(AuthorizationType.NoAuthorization);

                case AuthorizationModel.ApiKeyType:

                    return new SecurityEntity(AuthorizationType.ApiKey)
                    {
                        ApiKey = (ApiKeyEntity)this.Convert(value.ApiKey, AuthorizationType.ApiKey)
                    };

                case AuthorizationModel.BearerType:

                    return new SecurityEntity(AuthorizationType.BearerToken)
                    {
                        BearerToken = (BearerTokenEntity)this.Convert(value.Bearer, AuthorizationType.BearerToken)
                    };

                case AuthorizationModel.BasicType:

                    return new SecurityEntity(AuthorizationType.BasicAuthorization)
                    {
                        BasicAuthorization = (BasicAuthorizationEntity)this.Convert(value.Basic, AuthorizationType.BasicAuthorization)
                    };

                default:

                    return null;
            }
        }

        private Object Convert(IEnumerable<KeyValueModel> values, AuthorizationType type)
        {
            if (!values?.Any() ?? true)
            {
                return null;
            }

            switch (type)
            {
                case AuthorizationType.ApiKey:

                    return new ApiKeyEntity()
                    {
                        Usage = this.IsEqual(values.First(x => this.IsEqual(x.Key, ApiKeyModelConverter.InTag)).Value, "header") ? UsageType.Header : UsageType.Query,
                        Key = values.First(x => this.IsEqual(x.Key, ApiKeyModelConverter.KeyTag)).Value,
                        Value = values.First(x => this.IsEqual(x.Key, ApiKeyModelConverter.ValueTag)).Value
                    };

                case AuthorizationType.BearerToken:

                    return new BearerTokenEntity()
                    {
                        Token = values.First(x => this.IsEqual(x.Key, BearerModelConverter.TokenTag)).Value
                    };

                case AuthorizationType.BasicAuthorization:

                    return new BasicAuthorizationEntity()
                    {
                        Encoding = Encoding.UTF8,
                        Username = values.First(x => this.IsEqual(x.Key, BasicModelConverter.UsernameTag)).Value,
                        Password = values.First(x => this.IsEqual(x.Key, BasicModelConverter.PasswordTag)).Value,
                    };

                default:

                    return null;
            }
        }

        #endregion

        #region Private Helper Methods

        private String ReadContent(String filename)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                return reader.ReadToEnd();
            }
        }

        private Boolean IsEqual(String left, String right)
        {
            return String.Equals(left, right, this.comparison);
        }

        #endregion

        #region Postman Helper Models

        private class PostmanDataModel
        {
            [JsonProperty("info")]
            public InfoModel Info { get; set; }

            [JsonProperty("item")]
            public IEnumerable<ItemModel> Items { get; set; }

            [JsonProperty("auth")]
            public AuthorizationModel Authorization { get; set; }

            [JsonProperty("variable")]
            public IEnumerable<VariableModel> Variables { get; set; }
        }

        private class InfoModel
        {
            [JsonProperty("_postman_id")]
            public String PostmanId { get; set; }

            [JsonProperty("name")]
            public String Name { get; set; }

            [JsonProperty("schema")]
            public String Schema { get; set; }

            [JsonProperty("_exporter_id")]
            public String ExporterId { get; set; }
        }

        private class ItemModel
        {
            [JsonProperty("name")]
            public String Name { get; set; }

            // Recursion of requests in folders and sub-folders.
            [JsonProperty("item")]
            public IEnumerable<ItemModel> Items { get; set; }

            [JsonProperty("auth")]
            public AuthorizationModel Authorization { get; set; }

            [JsonProperty("request")]
            public RequestModel Request { get; set; }
        }

        private class RequestModel
        {
            [JsonProperty("method")]
            public String Method { get; set; }

            // A valid authorization might be in the root model if this property is null!
            [JsonProperty("auth")]
            public AuthorizationModel Authorization { get; set; }

            [JsonProperty("header")]
            public IEnumerable<HeaderModel> Headers { get; set; }

            [JsonProperty("url")]
            [JsonConverter(typeof(UrlModelConverter))]
            public UrlModel Url { get; set; }

            [JsonProperty("body")]
            public BodyModel Body { get; set; }
        }

        private class BodyModel
        {
            public const String NoneMode = "none";
            public const String RawMode = "raw";

            [JsonProperty("mode")]
            public String Mode { get; set; }

            [JsonProperty("raw")]
            public String Raw { get; set; }

            [JsonProperty("options")]
            public OptionsModel Options { get; set; }
        }

        private class OptionsModel
        {
            [JsonProperty("raw")]
            public RawOptionsModel Raw { get; set; }
        }

        private class RawOptionsModel
        {
            [JsonProperty("language")]
            public String Language { get; set; }
        }

        private class AuthorizationModel
        {
            public const String NoAuthType = "noauth";
            public const String ApiKeyType = "apikey";
            public const String BearerType = "bearer";
            public const String BasicType = "basic";

            [JsonProperty("type")]
            public String Type { get; set; }

            [JsonProperty("apikey")]
            [JsonConverter(typeof(ApiKeyModelConverter))]
            public IEnumerable<KeyValueModel> ApiKey { get; set; }

            [JsonProperty("bearer")]
            [JsonConverter(typeof(BearerModelConverter))]
            public IEnumerable<KeyValueModel> Bearer { get; set; }

            [JsonProperty("basic")]
            [JsonConverter(typeof(BasicModelConverter))]
            public IEnumerable<KeyValueModel> Basic { get; set; }
        }

        private class UrlModel
        {
            public const String RawTag = "raw";
            public const String ProtocolTag = "protocol";
            public const String HostTag = "host";
            public const String PortTag = "port";
            public const String PathTag = "path";
            public const String QueryTag = "query";

            [JsonProperty(UrlModel.RawTag)]
            public String Raw { get; set; }

            [JsonIgnore]
            public String Url { get; set; }

            [JsonProperty(UrlModel.ProtocolTag)]
            public String Protocol { get; set; }

            [JsonProperty(UrlModel.HostTag)]
            public IEnumerable<String> Host { get; set; }

            [JsonProperty(UrlModel.PortTag)]
            public String Port { get; set; }

            [JsonProperty(UrlModel.PathTag)]
            public IEnumerable<String> Path { get; set; }

            [JsonProperty(UrlModel.QueryTag)]
            public IEnumerable<QueryModel> Queries { get; set; }
        }

        private class KeyValueModel
        {
            public KeyValueModel()
                : this(null, null, false)
            {
            }
            public KeyValueModel(String key)
                : this(key, null, false)
            {
            }

            public KeyValueModel(String key, String value)
                : this(key, value, false)
            {
            }

            public KeyValueModel(String key, String value, Boolean disabled)
            {
                this.Key = key ?? String.Empty;
                this.Value = value ?? String.Empty;
                this.Disabled = disabled;
            }

            [JsonProperty("key")]
            public String Key { get; set; }

            [JsonProperty("value")]
            public String Value { get; set; }

            [JsonProperty("disabled")]
            public Boolean Disabled { get; set; }
        }

        private class HeaderModel : KeyValueModel { }

        private class QueryModel : KeyValueModel { }

        private class VariableModel : KeyValueModel { }

        #endregion
    }
}
