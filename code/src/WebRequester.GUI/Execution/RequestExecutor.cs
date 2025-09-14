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

using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Extensions;
using Plexdata.WebRequester.GUI.Definitions;
using Plexdata.WebRequester.GUI.Extensions;
using Plexdata.WebRequester.GUI.Interfaces;
using Plexdata.WebRequester.GUI.Models.Execution;
using Plexdata.WebRequester.GUI.Models.Projects;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Web;

namespace Plexdata.WebRequester.GUI.Execution;

internal class RequestExecutor : IRequestExecutor
{
    private readonly ILogger logger;

    public RequestExecutor(ILogger logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<ResultEntity> ExecuteAsync(RequestEntity entity, CancellationToken cancel)
    {
        this.Validate(entity);

        return this.ExecuteInternalAsync(entity, cancel);
    }

    private async Task<ResultEntity> ExecuteInternalAsync(RequestEntity entity, CancellationToken cancel)
    {
        ResultEntity result = null;
        Stopwatch stopwatch = Stopwatch.StartNew();
#if DEBUG
        //TODO Enable... await Task.Delay(10000, cancel); 
#endif
        using (HttpRequestMessage request = this.CreateRequest(entity))
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = await client.SendAsync(request, cancel))
                    {
                        stopwatch.Stop();

                        result = ResultEntity.Create(response, stopwatch.Elapsed, cancel);
                    }
                }
            }
            catch (Exception exception)
            {
                stopwatch.Stop();

                result = ResultEntity.Create(request, exception, stopwatch.Elapsed, cancel);
            }
        }

        return result;
    }

    private void Validate(RequestEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), $"Value of '{nameof(entity)}' must not be null.");
        }

        if (this.IsInvalid(entity.Url))
        {
            throw new ArgumentOutOfRangeException(nameof(entity), $"Value of '{nameof(entity.Url)}' must not be null, empty or whitespace.");
        }
    }

    private HttpRequestMessage CreateRequest(RequestEntity entity)
    {
        HttpRequestMessage request = new HttpRequestMessage(this.GetMethod(entity), this.GetRequestUrl(entity));

        this.SetInitialHeaders(entity, ref request);
        this.AddAuthorization(entity, ref request); // Do not move in front of method SetInitialHeaders()!
        this.SetContent(entity, ref request);

        return request;
    }

    private HttpMethod GetMethod(RequestEntity entity)
    {
        return entity.Method.ToHttpMethod();
    }

    private Uri GetRequestUrl(RequestEntity entity)
    {
        List<String> include = new List<String>();
        List<ActionEntity> parameters = entity.Query.Parameters.Where(x => x.Apply).ToList();

        if (entity.Security.AuthorizationType == AuthorizationType.ApiKey && entity.Security.ApiKey.Usage == UsageType.Query)
        {
            parameters.Add(new ActionEntity(true, entity.Security.ApiKey.Key, entity.Security.ApiKey.Value));
        }

        List<(String, Object)> skipped = new List<(String, Object)>();

        foreach (ActionEntity parameter in parameters)
        {
            Boolean labelValid = this.IsValid(parameter.Label);
            Boolean valueValid = this.IsValid(parameter.Value);

            if (labelValid && valueValid)
            {
                include.Add(String.Format("{0}={1}", HttpUtility.UrlEncode(parameter.Label), HttpUtility.UrlEncode(parameter.Value)));
            }
            else if (labelValid && !valueValid)
            {
                include.Add(String.Format("{0}", HttpUtility.UrlEncode(parameter.Label)));
            }
            else
            {
                skipped.Add((labelValid ? parameter.Label : "???", (Object)(valueValid ? parameter.Value : "???")));
            }
        }

        if (skipped.Count > 0)
        {
            this.logger.Warning("Found at least one invalid query parameter.", skipped.ToArray());
        }

        String url = entity.Url;

        if (include.Count > 0)
        {
            this.logger.Trace("Adding query parameters to URL.", include
                .Select(x => x.Split('='))
                .Select(x => x.Length > 1 ? (x[0], (Object)x[1]) : (x[0], (Object)String.Empty))
                .ToArray());

            url = String.Format("{0}?{1}", entity.Url, String.Join("&", include));
        }

        UriBuilder builder = new UriBuilder(url);

        return builder.Uri;
    }

    private void SetInitialHeaders(RequestEntity entity, ref HttpRequestMessage request)
    {
        List<ActionEntity> headers = entity.Header.Headers.Where(x => x.Apply).ToList();

        if (entity.Security.AuthorizationType == AuthorizationType.ApiKey && entity.Security.ApiKey.Usage == UsageType.Header)
        {
            headers.Add(new ActionEntity(true, entity.Security.ApiKey.Key, entity.Security.ApiKey.Value));
        }

        (String Label, Object Value)[] skipped = headers
            .Where(x => this.IsInvalid(x.Label) || this.IsInvalid(x.Value))
            .Select(x => (Label: this.IsValid(x.Label) ? x.Label : "???", Value: (Object)(this.IsValid(x.Value) ? x.Value : "???")))
            .ToArray();

        if (skipped.Length > 0)
        {
            this.logger.Warning("Found at least one invalid header value.", skipped);
        }

        request.Headers.Clear();

        // A header item is considered as valid if it is applied, if it has a valid label
        // and if it contains at least one value. And this is what the following grouping
        // actually does.

        var groups = headers
            .Where(x => this.IsValid(x.Label))
            .GroupBy(l => l.Label, v => v.Value, (k, g) => new
            {
                Label = k,
                Values = g.Where(x => this.IsValid(x))
                          .Select(x => x)
                          .ToArray()
            })
            .Where(x => x.Values.Length > 0);

        this.logger.Trace("Adding headers to request.", groups
            .Select(x => (x.Label, (Object)String.Format("[{0}]", String.Join(";", x.Values))))
            .ToArray()
        );

        foreach (var group in groups)
        {
            request.Headers.Add(group.Label, group.Values);
        }
    }

    private void AddAuthorization(RequestEntity entity, ref HttpRequestMessage request)
    {
        if (entity.Security.AuthorizationType == AuthorizationType.BasicAuthorization)
        {
            request.Headers.Authorization = entity.Security.BasicAuthorization.ToAuthenticationHeaderValue();
        }

        if (entity.Security.AuthorizationType == AuthorizationType.BearerToken)
        {
            request.Headers.Authorization = entity.Security.BearerToken.ToAuthenticationHeaderValue();
        }
    }

    private void SetContent(RequestEntity entity, ref HttpRequestMessage request)
    {
        switch (entity.Payload.SendType)
        {
            case PayloadType.None:
                request.Content = null;
                break;
            case PayloadType.Text:
                request.Content = this.CreateTextContent(entity);
                break;
            case PayloadType.File:
                request.Content = this.CreateFileContent(entity);
                break;
            case PayloadType.Form:
                request.Content = this.CreateFormContent(entity);
                break;
        }
    }

    private HttpContent CreateTextContent(RequestEntity entity)
    {
        // Secret knowledge! The property contains the payload for text content at
        // index zero or a fully qualified file path at index one or the payload
        // for form-data content at index two. Take item at index zero in this case.

        String[] contents = this.SplitContent(3, entity.Payload.Content);

        return new StringContent(contents[0], entity.Payload.Encoding, entity.Payload.MimeType);
    }

    private HttpContent CreateFileContent(RequestEntity entity)
    {
        // Secret knowledge! The property contains the payload for text content at
        // index zero or a fully qualified file path at index one or the payload
        // for form-data content at index two. Take item at index one in this case.

        String[] contents = this.SplitContent(3, entity.Payload.Content);

        String filePath = contents[1].Replace("\r", String.Empty).Replace("\n", String.Empty).Trim('"');

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The file \"{Path.GetFileName(filePath)}\"could not be found.", filePath);
        }

        // NOTE: There's no guarantee that this will actually work.
        //       Therefore, this should remain under observation.
        StreamContent content = new StreamContent(File.OpenRead(filePath));
        content.Headers.ContentType = new MediaTypeHeaderValue(entity.Payload.MimeType, entity.Payload.CharSet);

        return content;
    }

    private HttpContent CreateFormContent(RequestEntity entity)
    {
        // Secret knowledge! The property contains the payload for text content at
        // index zero or a fully qualified file path at index one or the payload
        // for form-data content at index two. Take item at index two in this case.

        String[] contents = this.SplitContent(3, entity.Payload.Content);

        ActionEntity[] actions = contents[2]
            .Split(Environment.NewLine)
            .Select(x => ActionEntity.Parse(x))
            .Where(x => x.Apply)
            .ToArray();

        if (actions.Length < 1)
        {
            throw new FormatException("The data block for multipart form data must consist of at least one data line.");
        }

        if (actions.Any(x => String.IsNullOrWhiteSpace(x.Label) || String.IsNullOrWhiteSpace(x.Value)))
        {
            (String Label, Object)[] details = actions
                .Where(x => String.IsNullOrWhiteSpace(x.Label) || String.IsNullOrWhiteSpace(x.Value))
                .Select(x => (x.Label, (Object)x.Value))
                .ToArray();

            this.logger.Warning("Found at least one invalid action entity. Each action entity must have its label plus its value in order to be processed.", details);
        }

        actions = actions.Where(x => !String.IsNullOrWhiteSpace(x.Label) && !String.IsNullOrWhiteSpace(x.Value)).ToArray();

        MultipartFormDataContent content = new();

        foreach (ActionEntity action in actions)
        {
            String dataName = action.Label;
            String filePath = action.Value.Trim('"');

            if (File.Exists(filePath))
            {
                content.Add(new StreamContent(File.OpenRead(filePath)), dataName, Path.GetFileName(filePath));
            }
            else
            {
                content.Add(new StringContent(action.Value), dataName);
            }
        }

        return content;
    }

    private Boolean IsValid(String value)
    {
        return !this.IsInvalid(value);
    }

    private Boolean IsInvalid(String value)
    {
        return String.IsNullOrWhiteSpace(value);
    }
}
