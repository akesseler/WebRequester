/*
 * MIT License
 * 
 * Copyright (c) 2025 plexdata.de
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

using System.Net.Sockets;

namespace Plexdata.WebRequester.GUI.Models.Execution
{
    internal class ErrorEntity
    {
        public static ErrorEntity Empty { get; } = new ErrorEntity();

        public static ErrorEntity Create(Exception exception)
        {
            return new ErrorEntity(exception);
        }

        private ErrorEntity()
            : base()
        {
        }

        private ErrorEntity(Exception exception)
            : this()
        {
            this.Exception = exception;

            if (exception == null)
            {
                this.IsError = false;
                this.IsCanceled = false;
                this.Status = 0;
                this.Messages = Enumerable.Empty<String>();
            }
            else if (exception is TaskCanceledException)
            {
                this.IsError = false;
                this.IsCanceled = true;
                this.Status = exception.HResult;
                this.Messages = ErrorEntity.CollectMessages(exception);
            }
            else if (exception is HttpRequestException && exception.InnerException is SocketException socketException)
            {
                this.IsError = true;
                this.IsCanceled = false;
                this.Status = socketException.NativeErrorCode;
                this.Messages = new String[]
                {
                    String.Format("{0} ({1})", socketException.NativeErrorCode, socketException.SocketErrorCode),
                    socketException.Message
                };
            }
            else
            {
                this.IsError = true;
                this.IsCanceled = false;
                this.Status = exception.HResult;
                this.Messages = ErrorEntity.CollectMessages(exception);
            }
        }

        public Boolean IsError { get; private set; } = false;

        public Boolean IsCanceled { get; private set; } = false;

        public Exception? Exception { get; private set; } = null;

        public Int32 Status { get; private set; } = 0;

        public IEnumerable<String> Messages { get; private set; } = Enumerable.Empty<String>();

        public String Message
        {
            get
            {
                if (this.Messages?.Any() ?? false)
                {
                    return String.Join(Environment.NewLine, this.Messages);
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        private static List<String> CollectMessages(Exception exception, List<String> messages = null)
        {
            messages ??= new List<String>();

            if (exception != null)
            {
                messages.Add(exception.Message);

                if (exception.InnerException != null)
                {
                    messages = ErrorEntity.CollectMessages(exception.InnerException, messages);
                }
            }

            return messages;
        }
    }
}
