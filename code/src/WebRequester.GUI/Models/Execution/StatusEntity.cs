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

using System.Net;

namespace Plexdata.WebRequester.GUI.Models.Execution
{
    internal class StatusEntity
    {
        public static StatusEntity Empty { get; } = new StatusEntity();

        public static StatusEntity Create(HttpResponseMessage response)
        {
            return new StatusEntity(response);
        }

        private StatusEntity()
            : base()
        {
        }

        private StatusEntity(HttpResponseMessage response)
            : this()
        {
            this.IsSuccess = response.IsSuccessStatusCode;
            this.StatusCode = response?.StatusCode ?? 0;
            this.ReasonPhrase = response?.ReasonPhrase ?? String.Empty;
        }

        public Boolean IsSuccess { get; private set; } = false;

        public HttpStatusCode StatusCode { get; private set; } = 0;

        public String ReasonPhrase { get; private set; } = String.Empty;

        public override String ToString()
        {
            return $"{(Int32)this.StatusCode} '{this.ReasonPhrase}'";
        }
    }
}
