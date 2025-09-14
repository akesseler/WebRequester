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

using Plexdata.Converters;
using Plexdata.WebRequester.GUI.Extensions;
using System.Reflection;
using System.Text;

namespace Plexdata.WebRequester.GUI.Models.Execution;

internal class PayloadEntity
{
    public static PayloadEntity Empty { get; } = new PayloadEntity();

    public static PayloadEntity Create(HttpRequestMessage request, CancellationToken cancel)
    {
        return new PayloadEntity(request?.Content, cancel);
    }

    public static PayloadEntity Create(HttpResponseMessage response, CancellationToken cancel)
    {
        return new PayloadEntity(response?.Content, cancel);
    }

    private PayloadEntity()
        : base()
    {
    }

    private PayloadEntity(HttpContent content, CancellationToken cancel)
        : this()
    {
        this.CharSet = content?.Headers.ContentType?.CharSet ?? String.Empty;
        this.MediaType = content?.Headers.ContentType?.MediaType ?? String.Empty;
        this.Encoding = this.GetEncodingOrDefault(this.CharSet);
        this.Payload = PayloadEntity.GetPayload(content, cancel);
    }

    public String CharSet { get; private set; } = String.Empty;

    public String MediaType { get; private set; } = String.Empty;

    public Encoding Encoding { get; private set; } = Encoding.Default;

    public Byte[] Payload { get; private set; } = Array.Empty<Byte>();

    public Boolean HasPayload
    {
        get
        {
            return this.Payload.Length > 0;
        }
    }

    public String GetPayloadAsString()
    {
        // TODO: Solve problem of an unconvertable payload.
        return this.Encoding.GetString(this.Payload);
    }

    private static Byte[] GetPayload(HttpContent content, CancellationToken cancel)
    {
        if (content == null)
        {
            return [];
        }

        if (content is MultipartFormDataContent formData)
        {
            PayloadEntity.HackFileStreamContent(formData);
        }
        else if (content is StreamContent streamContent)
        {
            PayloadEntity.HackFileStreamContent(streamContent);
        }

        using (MemoryStream memory = new MemoryStream())
        {
            // Please note that the content stream may
            // no longer be available after this!

            content.CopyTo(memory, null, cancel);

            return memory.ToArray();
        }
    }

    private static void HackFileStreamContent(MultipartFormDataContent formData)
    {
        foreach (HttpContent formItem in formData)
        {
            if (formItem is StreamContent streamContent)
            {
                PayloadEntity.HackFileStreamContent(streamContent);
            }
        }
    }

    private static void HackFileStreamContent(StreamContent streamContent)
    {
        Encoding encoding = Encoding.UTF8;

        FieldInfo fieldInfo = streamContent.GetType().GetField("_content", BindingFlags.NonPublic | BindingFlags.Instance);

        if (fieldInfo.GetValue(streamContent) is FileStream fileStream)
        {
            String fakeData = PayloadEntity.GetConverter().Convert(PayloadEntity.ReadFileData(fileStream, encoding));

            MemoryStream tempStream = new MemoryStream(encoding.GetBytes(fakeData));

            fieldInfo.SetValue(streamContent, tempStream);

            fileStream.Dispose();
            fileStream = null;
        }
    }

    private static Byte[] ReadFileData(FileStream fileStream, Encoding encoding, Int32 limit = 1024)
    {
        fileStream.Position = 0;

        using BinaryReader fileReader = new BinaryReader(fileStream);

        Int32 count = fileStream.Length < limit ? (Int32)fileStream.Length : limit;
        Byte[] bytes = fileReader.ReadBytes(count);

        if (fileStream.Length > bytes.Length)
        {
            using MemoryStream tempStream = new MemoryStream();

            tempStream.Write(bytes);
            tempStream.Write(encoding.GetBytes("MORE DATA..."));

            bytes = tempStream.ToArray();
        }

        return bytes;
    }

    private static BinConverter GetConverter()
    {
        return new BinConverter(new BinConverterSettings()
        {
            ByteBlockCount = 16,
            ByteBlockWidth = 2,
        });
    }
}
