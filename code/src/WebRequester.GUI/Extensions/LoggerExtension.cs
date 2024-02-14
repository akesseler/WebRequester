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

using Plexdata.LogWriter.Definitions;
using Plexdata.WebRequester.GUI.Interfaces;

namespace Plexdata.WebRequester.GUI.Extensions
{
    internal static class LoggerExtension
    {
        public static String GetDisplayText(this LogLevel level)
        {
            // This exception for logging level 'Message' is necessary because
            // method 'ToString()' returns name 'Default' instead of desired name.

            if (level == LogLevel.Message)
            {
                return nameof(LogLevel.Message);
            }

            return level.ToString();
        }

        public static ISerializableLoggerEntity ToSerializable(this ILoggerEntity entity)
        {
            return new SerializableLoggerEntity(entity);
        }

        private class SerializableLoggerEntity : ISerializableLoggerEntity
        {
            private readonly ILoggerEntity source = null;

            public SerializableLoggerEntity(ILoggerEntity source)
            {
                this.source = source ?? throw new ArgumentNullException(nameof(source));
            }

            public String Level
            {
                get
                {
                    return String.Format("{0} ({1})", (Int32)this.source.Level, this.source.Level.ToString());
                }
            }

            public String Message
            {
                get
                {
                    return this.source.Message;
                }
            }

            public IDictionary<String, String> Exception
            {
                get
                {
                    if (this.source.Exception == null)
                    {
                        return null;
                    }

                    return new Dictionary<String, String>()
                    {
                        { nameof(this.source.Exception.HResult), String.Format("{0} (0x{0:X8})", this.source.Exception.HResult ) },
                        { nameof(this.source.Exception.Message), this.source.Exception.Message ?? String.Empty },
                        { nameof(this.source.Exception.StackTrace), this.source.Exception.StackTrace?.Trim() ?? String.Empty }
                    };
                }
            }

            public (String Label, Object Value)[] Details
            {
                get
                {
                    return this.source.Details;
                }
            }
        }
    }
}
