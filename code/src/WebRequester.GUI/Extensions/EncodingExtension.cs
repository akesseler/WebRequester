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

using System.Text;

namespace Plexdata.WebRequester.GUI.Extensions
{
    internal static class EncodingExtension
    {
        private static readonly Encoding[] encodings;
        private static readonly (String CharSet, Encoding Encoding)[] relations;

        static EncodingExtension()
        {
            EncodingExtension.encodings = new Encoding[]
            {
                Encoding.GetEncoding("utf-8"),      // Unicode (UTF-8)
                Encoding.GetEncoding("utf-16"),     // Unicode
                Encoding.GetEncoding("utf-16BE"),   // Unicode (Big-Endian)
                Encoding.GetEncoding("utf-32"),     // Unicode (UTF-32)
                Encoding.GetEncoding("utf-32BE"),   // Unicode (UTF-32 Big-Endian)
                Encoding.GetEncoding("us-ascii"),   // US-ASCII
                Encoding.GetEncoding("iso-8859-1"), // Western European (ISO)
            };

            EncodingExtension.relations = EncodingExtension.encodings.Select(x => (CharSet: x.WebName, Encoding: x)).ToArray();
        }

        public static IEnumerable<(String CharSet, Encoding Encoding)> GetSupportedEncodingRelations(this Object _)
        {
            return EncodingExtension.relations;
        }

        public static IEnumerable<Encoding> GetSupportedEncodings(this Object _)
        {
            return EncodingExtension.encodings;
        }

        public static Encoding GetEncodingOrDefault(this Object _, String value)
        {
            return EncodingExtension.encodings.FirstOrDefault(x => String.Equals(x.WebName, value, StringComparison.OrdinalIgnoreCase)) ?? Encoding.Default;
        }

        public static IEnumerable<String> GetSupportedCharSets(this Object _)
        {
            return EncodingExtension.relations.Select(x => x.CharSet);
        }

        public static String GetCharSetOrDefault(this Object _, String value)
        {
            const StringComparison comparison = StringComparison.OrdinalIgnoreCase;

            (String CharSet, Encoding Encoding) result = EncodingExtension.relations.FirstOrDefault(x => String.Equals(x.CharSet, value, comparison));

            return String.IsNullOrWhiteSpace(result.CharSet) ? Encoding.Default.WebName : result.CharSet;
        }

        public static Encoding GetEncodingFromCharSet(this Object _, String value)
        {
            const StringComparison comparison = StringComparison.OrdinalIgnoreCase;

            (String CharSet, Encoding Encoding) result = EncodingExtension.relations.FirstOrDefault(x => String.Equals(x.CharSet, value, comparison));

            return String.IsNullOrWhiteSpace(result.CharSet) ? Encoding.Default : result.Encoding;
        }
    }
}
