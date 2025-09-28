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

using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Plexdata.WebRequester.GUI.Helpers.Converters
{
    internal class EncryptedStringConverter : JsonConverter
    {
        #region Private Fields

        private readonly Byte[] secret = new Byte[]
        {
            0x57, 0x61, 0x73, 0x20, 0x74, 0x75, 0x6D, 0x20,
            0x54, 0x65, 0x75, 0x66, 0x65, 0x6C, 0x20, 0x69,
            0x73, 0x74, 0x20, 0x65, 0x69, 0x6E, 0x20, 0x22,
            0x69, 0x6E, 0x69, 0x74, 0x69, 0x61, 0x6C, 0x69
        };

        private readonly Byte[] vector = new Byte[]
        {
            0x23, 0x70, 0x72, 0x61, 0x67, 0x6D, 0x61, 0x20,
            0x72, 0x65, 0x69, 0x6E, 0x20, 0x75, 0x6E, 0x64
        };

        #endregion

        #region Public Methods

        public override void WriteJson(JsonWriter writer, Object? value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value is String source)
            {
                value = this.EncryptString(source);
            }

            writer.WriteValue(value);
        }

        public override Object? ReadJson(JsonReader reader, Type objectType, Object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            Object? result = reader.Value;

            if (result is String source)
            {
                result = this.DecryptString(source);
            }

            return result;
        }

        public override Boolean CanConvert(Type objectType)
        {
            return objectType == typeof(String);
        }

        #endregion

        #region Private Methods

        private String EncryptString(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }

            using (Aes algorithm = Aes.Create())
            {
                using (ICryptoTransform transformer = algorithm.CreateEncryptor(this.secret, this.vector))
                {
                    using (MemoryStream target = new MemoryStream())
                    {
                        using (CryptoStream crypto = new CryptoStream(target, transformer, CryptoStreamMode.Write))
                        {
                            Byte[] bytes = Encoding.UTF8.GetBytes(value);

                            crypto.Write(bytes, 0, bytes.Length);
                            crypto.FlushFinalBlock();

                            return Convert.ToBase64String(target.ToArray());
                        }
                    }
                }
            }
        }

        private String DecryptString(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }

            using (Aes algorithm = Aes.Create())
            {
                using (ICryptoTransform transformer = algorithm.CreateDecryptor(this.secret, this.vector))
                {
                    using (MemoryStream source = new MemoryStream(Convert.FromBase64String(value)))
                    {
                        using (CryptoStream crypto = new CryptoStream(source, transformer, CryptoStreamMode.Read))
                        {
                            using (MemoryStream target = new MemoryStream())
                            {
                                crypto.CopyTo(target);

                                return Encoding.UTF8.GetString(target.ToArray());
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
