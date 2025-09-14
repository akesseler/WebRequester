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

using Plexdata.WebRequester.GUI.Definitions;

namespace Plexdata.WebRequester.GUI.Extensions;

internal static class ContentExtension
{
    public static String BuildContent(this Object caller, params String[] values)
    {
        if (values is null || values.Length < 1)
        {
            return String.Empty;
        }

        return caller.BuildContent(values.Length, values);
    }

    public static String BuildContent(this Object _, Int32 count, params String[] values)
    {
        if (values == null || values.Length < 1)
        {
            return String.Empty;
        }

        if (count < values.Length)
        {
            count = values.Length;
        }

        List<String> items = [.. values.Select(x => (x is null ? String.Empty : x))];

        if (items.Count < count)
        {
            items.AddRange(Enumerable.Repeat(String.Empty, count - items.Count));
        }

        return String.Join(SeparatorType.GroupSeparator, items);
    }

    public static String[] SplitContent(this Object _, Int32 count, String value)
    {
        if (String.IsNullOrEmpty(value))
        {
            return [.. Enumerable.Repeat(String.Empty, count)];
        }

        List<String> values = [.. value.Split(SeparatorType.GroupSeparator)];

        if (values.Count < count)
        {
            values.AddRange(Enumerable.Repeat(String.Empty, count - values.Count));
        }
        else if (values.Count > count)
        {
            values = values.Take(count).ToList();
        }

        return [.. values];
    }
}
