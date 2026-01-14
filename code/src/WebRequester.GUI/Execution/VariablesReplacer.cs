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

using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Extensions;
using Plexdata.WebRequester.GUI.Interfaces;
using Plexdata.WebRequester.GUI.Models.Projects;
using Plexdata.WebRequester.GUI.Models.Projects.Security;

namespace Plexdata.WebRequester.GUI.Execution
{
    internal class VariablesReplacer : IVariablesReplacer
    {
        private readonly ILogger logger;

        public VariablesReplacer(ILogger logger)
            : base()
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public RequestEntity Replace(RequestEntity request, IEnumerable<VariableEntity> variables)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), $"Value of '{nameof(request)}' must not be null.");
            }

            if (!variables?.Any() ?? true)
            {
                return request;
            }

            List<(String, Object)> loggings = new List<(String, Object)>();
            IDictionary<String, String> replacements = this.GetNormalizedVariables(variables);

            request.Url = this.Replace(request.Url, replacements, ref loggings);
            request.Query = this.Replace(request.Query, replacements, ref loggings);
            request.Header = this.Replace(request.Header, replacements, ref loggings);
            request.Security = this.Replace(request.Security, replacements, ref loggings);
            request.Payload = this.Replace(request.Payload, replacements, ref loggings);

            if (loggings.Count > 0)
            {
                this.logger.Trace("Variables replaced.", loggings.ToArray());
            }

            return request;
        }

        private QueryEntity Replace(QueryEntity source, IDictionary<String, String> replacements, ref List<(String, Object)> loggings)
        {
            source.Parameters = this.Replace(source.Parameters, replacements, ref loggings);

            return source;
        }

        private HeaderEntity Replace(HeaderEntity source, IDictionary<String, String> replacements, ref List<(String, Object)> loggings)
        {
            source.Headers = this.Replace(source.Headers, replacements, ref loggings);

            return source;
        }

        private SecurityEntity Replace(SecurityEntity source, IDictionary<String, String> replacements, ref List<(String, Object)> loggings)
        {
            source.ApiKey = this.Replace(source.ApiKey, replacements, ref loggings);
            source.BearerToken = this.Replace(source.BearerToken, replacements, ref loggings);
            source.BasicAuthorization = this.Replace(source.BasicAuthorization, replacements, ref loggings);

            return source;
        }

        private ApiKeyEntity Replace(ApiKeyEntity source, IDictionary<String, String> replacements, ref List<(String, Object)> loggings)
        {
            source.Key = this.Replace(source.Key, replacements, ref loggings);
            source.Value = this.Replace(source.Value, replacements, ref loggings);

            return source;
        }

        private BearerTokenEntity Replace(BearerTokenEntity source, IDictionary<String, String> replacements, ref List<(String, Object)> loggings)
        {
            source.Token = this.Replace(source.Token, replacements, ref loggings);

            return source;
        }

        private BasicAuthorizationEntity Replace(BasicAuthorizationEntity source, IDictionary<String, String> replacements, ref List<(String, Object)> loggings)
        {
            source.Username = this.Replace(source.Username, replacements, ref loggings);
            source.Password = this.Replace(source.Password, replacements, ref loggings);

            return source;
        }

        private PayloadEntity Replace(PayloadEntity source, IDictionary<String, String> replacements, ref List<(String, Object)> loggings)
        {
            source.Content = this.Replace(source.Content, replacements, ref loggings);

            return source;
        }

        private IEnumerable<ActionEntity> Replace(IEnumerable<ActionEntity> source, IDictionary<String, String> replacements, ref List<(String, Object)> loggings)
        {
            foreach (ActionEntity entity in source)
            {
                if (entity.Apply)
                {
                    entity.Label = this.Replace(entity.Label, replacements, ref loggings);
                    entity.Value = this.Replace(entity.Value, replacements, ref loggings);
                }
            }

            return source;
        }

        private String Replace(String source, IDictionary<String, String> replacements, ref List<(String, Object)> loggings)
        {
            String result = source;

            foreach (KeyValuePair<String, String> replacement in replacements)
            {
                if (result.Contains(replacement.Key))
                {
                    loggings.Add((replacement.Key, replacement.Value));
                    result = result.Replace(replacement.Key, replacement.Value);
                }
            }

            return result;
        }

        private IDictionary<String, String> GetNormalizedVariables(IEnumerable<VariableEntity> variables)
        {
            Dictionary<String, String> result = new Dictionary<String, String>();

            List<VariableEntity> skippedVariables = new List<VariableEntity>();
            List<VariableEntity> invalidVariables = new List<VariableEntity>();
            List<(String, Object)> emptyVariables = new List<(String, Object)>();
            List<(String, Object)> duplicateVariables = new List<(String, Object)>();

            foreach (VariableEntity variable in variables)
            {
                if (!variable.Apply)
                {
                    skippedVariables.Add(variable);
                    continue;
                }

                if (!this.TryGetNormalizedLabel(variable.Label, out String label))
                {
                    invalidVariables.Add(variable);
                    continue;
                }

                if (String.IsNullOrWhiteSpace(variable.Value))
                {
                    emptyVariables.Add((label, variable.Value));
                }

                if (!result.ContainsKey(label))
                {
                    result.Add(label, variable.Value);
                }
                else
                {
                    duplicateVariables.Add((label, variable.Value));
                }
            }

            if (skippedVariables.Count > 0)
            {
                this.logger.Trace("Found variables to be skipped.", skippedVariables.Select(x => (x.Label, (Object)x.Value)).ToArray());
            }

            if (invalidVariables.Count > 0)
            {
                this.logger.Warning("Found invalid variables.", invalidVariables.Select(x => (x.Label, (Object)x.Value)).ToArray());
            }

            if (emptyVariables.Count > 0)
            {
                this.logger.Message("Found empty variables.", emptyVariables.ToArray());
            }

            if (duplicateVariables.Count > 0)
            {
                this.logger.Warning("Found duplicate variables.", duplicateVariables.ToArray());
            }

            return result;
        }

        private Boolean TryGetNormalizedLabel(String label, out String result)
        {
            const String allowed = "abcdefghijklmnopqrstuvwxyz_ABCDEFGHIJKLMNOPQRSTUVWXYZ-0123456789";

            result = label.TrimStart('<').TrimEnd('>');

            foreach (Char current in result)
            {
                if (!allowed.Contains(current))
                {
                    return false;
                }
            }

            result = String.Format("<<{0}>>", result);

            return true;
        }
    }
}
