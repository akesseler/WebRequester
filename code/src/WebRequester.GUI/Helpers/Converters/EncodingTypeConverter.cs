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

using Newtonsoft.Json;
using Plexdata.WebRequester.GUI.Extensions;
using System.Text;

namespace Plexdata.WebRequester.GUI.Helpers.Converters
{
    internal class EncodingTypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, Object? value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value is Encoding source)
            {
                value = source.WebName;
            }

            writer.WriteValue(value);
        }

        public override Object? ReadJson(JsonReader reader, Type objectType, Object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            Object? result = reader.Value;

            if (result is String source)
            {
                result = this.GetEncodingOrDefault(source);
            }

            return result;
        }

        public override Boolean CanConvert(Type objectType)
        {
            return objectType == typeof(String) || objectType == typeof(Encoding);
        }
    }
}
