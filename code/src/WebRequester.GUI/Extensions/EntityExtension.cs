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

using Plexdata.WebRequester.GUI.Definitions;
using Plexdata.WebRequester.GUI.Models.Execution;
using Plexdata.WebRequester.GUI.Models.Projects;
using Plexdata.WebRequester.GUI.Models.Projects.Security;
using System.Net.Http.Headers;
using System.Text;

namespace Plexdata.WebRequester.GUI.Extensions
{
    internal static class EntityExtension
    {
        #region Public Methods

        public static String ToDisplay(this LabeledEntity entity)
        {
            if (entity is ProjectEntity project)
            {
                return project.Label;
            }

            if (entity is SectionEntity section)
            {
                return section.Label;
            }

            if (entity is RequestEntity request)
            {
                return $"{request.Method}: {request.Label}";
            }

            throw new NotSupportedException();
        }

        public static IEnumerable<(String Label, AuthorizationType Value)> GetAuthorizationTypes(this SecurityEntity _)
        {
            return new List<(String Label, AuthorizationType Value)>
            {
                ( "Inherit from Parent", AuthorizationType.InheritFromParent  ),
                ( "No Authorization",    AuthorizationType.NoAuthorization    ),
                ( "API Key",             AuthorizationType.ApiKey             ),
                ( "Bearer Token",        AuthorizationType.BearerToken        ),
                ( "Basic Authorization", AuthorizationType.BasicAuthorization )
            };
        }

        public static String ToDisplay(this AuthorizationType value)
        {
            return EntityExtension.GetAuthorizationTypes(null).FirstOrDefault(x => x.Value == value).Label ?? String.Empty;
        }

        public static String ToDisplay(this ResultEntity entity)
        {
            if (entity == null)
            {
                return String.Empty;
            }

            Int32 depth = 0;
            StringBuilder builder = new StringBuilder();

            builder.AddOverview(depth, entity);
            builder.AddRequest(depth, entity);
            builder.AddResponse(depth, entity);

            return builder.ToString();
        }

        public static AuthenticationHeaderValue ToAuthenticationHeaderValue(this BasicAuthorizationEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), $"Value of {nameof(entity)}' must not be null");
            }

            // Property 'Username' may be empty or contains only of white spaces, but it is
            // never null due to the behavior of the property. Property 'Password' instead
            // can be empty or contain only white spaces, which is acceptable. However, due
            // to the behavior of the property, it is never null.

            if (String.IsNullOrWhiteSpace(entity.Username))
            {
                throw new ArgumentOutOfRangeException(nameof(entity), $"Value of {nameof(entity.Username)}' must not be null, empty or whitespace.");
            }

            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(entity.Encoding.GetBytes(String.Format("{0}:{1}", entity.Username, entity.Password))));
        }

        public static AuthenticationHeaderValue ToAuthenticationHeaderValue(this BearerTokenEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), $"Value of {nameof(entity)}' must not be null");
            }

            // I'm not really sure if an empty access token represents a valid use case.
            // Therefore, just check presented token and throw an exception if it is empty.

            if (String.IsNullOrWhiteSpace(entity.Token))
            {
                throw new ArgumentOutOfRangeException(nameof(entity), $"Value of {nameof(entity.Token)}' must not be null, empty or whitespace.");
            }

            return new AuthenticationHeaderValue("Bearer", entity.Token);
        }

        #endregion

        #region Private Methods

        private static StringBuilder AddOverview(this StringBuilder builder, Int32 depth, ResultEntity entity)
        {
            builder.AddCaption(depth, "Overview");

            depth++;

            String indent = builder.GetIndent(depth);

            builder.AppendLine($"{indent}Timestamp: {entity.Common.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            builder.AppendLine($"{indent}Duration: {entity.Common.Elapsed.TotalMilliseconds:#,##0} ms");
            builder.AppendLine($"{indent}Status: {(Int32)entity.Status.StatusCode}");

            if (!String.IsNullOrWhiteSpace(entity.Status.ReasonPhrase))
            {
                builder.AppendLine($"{indent}Phrase: {entity.Status.ReasonPhrase}");
            }

            return builder;
        }

        private static StringBuilder AddRequest(this StringBuilder builder, Int32 depth, ResultEntity entity)
        {
            builder.AddCaption(depth, "Request");

            depth++;

            String indent = builder.GetIndent(depth);

            builder.AppendLine($"{indent}{entity.Common.Method} {entity.Common.RequestUri.AbsoluteUri}");

            builder.AddHeaders(depth, entity.RequestHeaders);
            builder.AddPayload(depth, entity.RequestPayload);

            return builder;
        }

        private static StringBuilder AddResponse(this StringBuilder builder, Int32 depth, ResultEntity entity)
        {
            builder.AddCaption(depth, "Response");

            depth++;

            builder.AddHeaders(depth, entity.ResponseHeaders);
            builder.AddPayload(depth, entity.ResponsePayload);

            return builder;
        }

        private static StringBuilder AddHeaders(this StringBuilder builder, Int32 depth, IEnumerable<(String Label, String Value)> headers)
        {
            if (headers.Any())
            {
                builder.AddCaption(depth, "Headers");

                depth++;

                String indent = builder.GetIndent(depth);

                foreach ((String Label, String Value) header in headers)
                {
                    builder.AppendLine($"{indent}{header.Label}: {header.Value}");
                }
            }

            return builder;
        }

        private static StringBuilder AddPayload(this StringBuilder builder, Int32 depth, Models.Execution.PayloadEntity payload)
        {
            if (payload.HasPayload)
            {
                builder.AddCaption(depth, "Payload");

                depth++;

                String indent = builder.GetIndent(depth);
                String dashes = String.Empty.PadLeft(50, '-');

                builder.AppendLine($"{indent}Length: {payload.Payload.Length:#,##0} MiB");

                builder.AppendLine($"{indent}{dashes}");

                foreach (String line in payload.GetPayloadLines())
                {
                    builder.AppendLine($"{indent}{line}");
                }

                builder.AppendLine($"{indent}{dashes}");
            }

            return builder;
        }

        private static StringBuilder AddCaption(this StringBuilder builder, Int32 depth, String caption)
        {
            String indent = builder.GetIndent(depth);

            builder.AppendLine($"{indent}{caption}");

            return builder;
        }

        private static String GetIndent(this StringBuilder _, Int32 depth)
        {
            return String.Empty.PadLeft(depth * 2, ' ');
        }

        private static String[] GetPayloadLines(this Models.Execution.PayloadEntity entity)
        {
            return entity.GetPayloadAsString().ReplaceLineEndings(Environment.NewLine).Split(Environment.NewLine);
        }

        #endregion
    }
}
