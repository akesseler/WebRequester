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

using System.Net.Http.Headers;

namespace Plexdata.WebRequester.GUI.Models.Execution
{
    internal class ResultEntity
    {
        public static ResultEntity Create(HttpResponseMessage response, TimeSpan elapsed, CancellationToken cancel)
        {
            return new ResultEntity(response?.RequestMessage, response, elapsed, cancel);
        }

        public static ResultEntity Create(HttpRequestMessage request, Exception exception, TimeSpan elapsed, CancellationToken cancel)
        {
            return new ResultEntity(request, exception, elapsed, cancel);
        }

        private ResultEntity(HttpRequestMessage request, HttpResponseMessage response, TimeSpan elapsed, CancellationToken cancel)
            : base()
        {
            this.Common = CommonEntity.Create(request, elapsed);
            this.Status = StatusEntity.Create(response);
            this.Error = ErrorEntity.Empty;
            this.RequestHeaders = ResultEntity.ParseHeaders(request);
            this.RequestPayload = PayloadEntity.Create(request, cancel);
            this.ResponseHeaders = ResultEntity.ParseHeaders(response);
            this.ResponsePayload = PayloadEntity.Create(response, cancel);
        }

        private ResultEntity(HttpRequestMessage request, Exception exception, TimeSpan elapsed, CancellationToken cancel)
            : base()
        {
            this.Common = CommonEntity.Create(request, elapsed);
            this.Status = StatusEntity.Empty;
            this.Error = ErrorEntity.Create(exception);
            this.RequestHeaders = ResultEntity.ParseHeaders(request);
            this.RequestPayload = PayloadEntity.Create(request, cancel);
            this.ResponseHeaders = Enumerable.Empty<(String Label, String Value)>();
            this.ResponsePayload = PayloadEntity.Empty;
        }

        public Boolean IsError
        {
            get
            {
                return this.Error.IsError;
            }
        }

        public Boolean IsCanceled
        {
            get
            {
                return this.Error.IsCanceled;
            }
        }

        public CommonEntity Common { get; private set; }

        public StatusEntity Status { get; private set; }

        public ErrorEntity Error { get; private set; }

        public IEnumerable<(String Label, String Value)> RequestHeaders { get; private set; }

        public PayloadEntity RequestPayload { get; private set; }

        public IEnumerable<(String Label, String Value)> ResponseHeaders { get; private set; }

        public PayloadEntity ResponsePayload { get; private set; }

        private static IEnumerable<(String Label, String Value)> ParseHeaders(HttpRequestMessage request)
        {
            List<(String Label, String Value)> headers = new List<(String Label, String Value)>();

            ResultEntity.ParseHeaders(request?.Headers, ref headers);
            ResultEntity.ParseHeaders(request?.Content?.Headers, ref headers);

            return headers;
        }

        private static IEnumerable<(String Label, String Value)> ParseHeaders(HttpResponseMessage response)
        {
            List<(String Label, String Value)> headers = new List<(String Label, String Value)>();

            ResultEntity.ParseHeaders(response?.Headers, ref headers);
            ResultEntity.ParseHeaders(response?.Content?.Headers, ref headers);

            return headers;
        }

        private static void ParseHeaders(HttpHeaders headers, ref List<(String Label, String Value)> result)
        {
            if (headers == null)
            {
                return;
            }

            foreach (KeyValuePair<String, IEnumerable<String>> header in headers)
            {
                Int32 count = header.Value.Count();
                String label = header.Key;
                String value;

                if (count == 0)
                {
                    value = $"<empty>";
                }
                else if (count == 1)
                {
                    value = $"{header.Value.First()}";
                }
                else
                {
                    value = $"[{String.Join(", ", header.Value)}]";
                }

                result.Add((label, value));
            }
        }
    }
}
