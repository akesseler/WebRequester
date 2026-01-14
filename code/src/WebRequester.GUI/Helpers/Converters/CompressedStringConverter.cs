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
using System.IO.Compression;
using System.Text;

namespace Plexdata.WebRequester.GUI.Helpers.Converters
{
    internal class CompressedStringConverter : JsonConverter
    {
        #region Public Methods

        public override void WriteJson(JsonWriter writer, Object? value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value is String source)
            {
                value = this.CompressString(source);
            }

            writer.WriteValue(value);
        }

        public override Object? ReadJson(JsonReader reader, Type objectType, Object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            Object? result = reader.Value;

            if (result is String source)
            {
                try
                {
                    result = this.DecompressString(source);
                }
                catch (InvalidDataException)
                {
                    // Ensure backward compatibility!
                    result = this.DecompressStringLegacy(source);
                }
            }

            return result;
        }

        public override Boolean CanConvert(Type objectType)
        {
            return objectType == typeof(String);
        }

        #endregion

        #region Private Methods

        private String CompressString(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }

            using MemoryStream result = new MemoryStream();
            using GZipStream zipper = new GZipStream(result, CompressionMode.Compress);

            zipper.Write(Encoding.UTF8.GetBytes(value));
            zipper.Flush();

            return Convert.ToBase64String(result.ToArray());
        }

        private String DecompressString(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }

            using MemoryStream source = new MemoryStream(Convert.FromBase64String(value));
            using GZipStream zipper = new GZipStream(source, CompressionMode.Decompress);
            using MemoryStream result = new MemoryStream();

            zipper.CopyTo(result);

            return Encoding.UTF8.GetString(result.ToArray());
        }

        private String DecompressStringLegacy(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }

            Byte[] source = Convert.FromBase64String(value);

            using (MemoryStream stream = new MemoryStream())
            {
                Int32 length = BitConverter.ToInt32(source, 0);
                stream.Write(source, 4, source.Length - 4);

                Byte[] buffer = new Byte[length];

                stream.Position = 0;

                using (GZipStream zipper = new GZipStream(stream, CompressionMode.Decompress))
                {
                    zipper.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        #endregion
    }
}
