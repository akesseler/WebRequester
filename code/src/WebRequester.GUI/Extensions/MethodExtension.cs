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

using System.Collections.ObjectModel;

namespace Plexdata.WebRequester.GUI.Extensions
{
    internal static class MethodExtension
    {
        private static readonly IDictionary<String, HttpMethod> allowedHttpMethods;

        static MethodExtension()
        {
            MethodExtension.allowedHttpMethods = new ReadOnlyDictionary<String, HttpMethod>(new Dictionary<String, HttpMethod>()
            {
                { HttpMethod.Get.Method,     HttpMethod.Get     },
                { HttpMethod.Put.Method,     HttpMethod.Put     },
                { HttpMethod.Post.Method,    HttpMethod.Post    },
                { HttpMethod.Delete.Method,  HttpMethod.Delete  },
                { HttpMethod.Head.Method,    HttpMethod.Head    },
                { HttpMethod.Options.Method, HttpMethod.Options },
                { HttpMethod.Trace.Method,   HttpMethod.Trace   },
                { HttpMethod.Patch.Method,   HttpMethod.Patch   }
            });
        }

        public static String[] AllowedHttpMethods(this Object _)
        {
            return allowedHttpMethods.Select(x => x.Key.ToString()).ToArray();
        }

        public static String HttpMethodStringOrDefault(this Object _, String method)
        {
            KeyValuePair<String, HttpMethod> result = MethodExtension.allowedHttpMethods
                .FirstOrDefault(x => String.Equals(x.Key, method, StringComparison.OrdinalIgnoreCase));

            if (String.IsNullOrWhiteSpace(result.Key))
            {
                result = MethodExtension.allowedHttpMethods.First();
            }

            return result.Value.Method;
        }

        public static HttpMethod ToHttpMethod(this String method)
        {
            KeyValuePair<String, HttpMethod> result = MethodExtension.allowedHttpMethods
                .FirstOrDefault(x => String.Equals(x.Key, method, StringComparison.OrdinalIgnoreCase));

            if (String.IsNullOrWhiteSpace(result.Key))
            {
                throw new KeyNotFoundException($"Unable to find any HTTP method for value '{method}'.");
            }

            return result.Value;
        }
    }
}
