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
using Plexdata.WebRequester.GUI.Interfaces;
using System.Text;

namespace Plexdata.WebRequester.GUI.Helpers
{
    internal class JsonSerializer : ISerializer
    {
        // NOTE: Never use the ILogger in this class!
        //       Otherwise, a stack overflow exception
        //       will occur due to a circular reference.

        #region Public Methods

        public String Serialize<TValue>(TValue value, Boolean indented)
        {
            if (value == null)
            {
                return String.Empty;
            }

            return this.SerializeInternal(value, this.GetFormatting(indented));
        }

        public TResult Deserialize<TResult>(String value) where TResult : new()
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return default;
            }

            return this.DeserializeInternal<TResult>(value);
        }

        public Boolean LoadFile<TValue>(String filename, out TValue value, Encoding encoding)
        {
            value = default;

            if (String.IsNullOrWhiteSpace(filename))
            {
                return false;
            }

            encoding ??= Encoding.UTF8;

            try
            {
                if (!File.Exists(filename))
                {
                    value = Activator.CreateInstance<TValue>();
                    return true;
                }

                using (FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (TextReader reader = new StreamReader(stream, encoding))
                    {
                        value = this.DeserializeInternal<TValue>(reader.ReadToEnd());
                    }
                }

                return value != null;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Loading data from file '{filename}' failed unexpectedly.", exception);
            }
        }

        public Boolean SaveFile<TValue>(String filename, TValue value, Encoding encoding, Boolean indented)
        {
            if (String.IsNullOrWhiteSpace(filename) || value == null)
            {
                return false;
            }

            encoding ??= Encoding.UTF8;

            try
            {
                using (FileStream stream = File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    using (TextWriter writer = new StreamWriter(stream, encoding))
                    {
                        writer.Write(this.SerializeInternal(value, this.GetFormatting(indented)));
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Saving data into file '{filename}' failed unexpectedly.", exception);
            }
        }

        #endregion

        #region Private Methods

        private Formatting GetFormatting(Boolean indented)
        {
            return indented ? Formatting.Indented : Formatting.None;
        }

        private TValue DeserializeInternal<TValue>(String value, JsonSerializerSettings settings = null)
        {
            settings ??= new JsonSerializerSettings();

            return JsonConvert.DeserializeObject<TValue>(value, settings);
        }

        private String SerializeInternal(Object value, Formatting formatting)
        {
            return JsonConvert.SerializeObject(value, formatting);
        }

        #endregion
    }
}
