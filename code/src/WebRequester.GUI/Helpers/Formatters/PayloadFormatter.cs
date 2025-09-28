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

using Plexdata.LogWriter.Abstraction;
using Plexdata.WebRequester.GUI.Definitions;
using Plexdata.WebRequester.GUI.Interfaces;

namespace Plexdata.WebRequester.GUI.Helpers.Formatters;

internal class PayloadFormatter : IPayloadFormatter
{
    private readonly ILogger logger;
    private readonly IJsonFormatter jsonFormatter;
    private readonly IXmlFormatter xmlFormatter;
    private readonly IHtmlFormatter htmlFormatter;

    public PayloadFormatter(ILogger logger, IJsonFormatter jsonFormatter, IXmlFormatter xmlFormatter, IHtmlFormatter htmlFormatter)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.jsonFormatter = jsonFormatter ?? throw new ArgumentNullException(nameof(jsonFormatter));
        this.xmlFormatter = xmlFormatter ?? throw new ArgumentNullException(nameof(xmlFormatter));
        this.htmlFormatter = htmlFormatter ?? throw new ArgumentNullException(nameof(htmlFormatter));
    }

    public String Format(FormatType format, String source)
    {
        source = source ?? String.Empty;

        if (format == FormatType.None || source.Length < 1)
        {
            return source;
        }

        switch (format)
        {
            case FormatType.Json:
                return this.jsonFormatter.Format(source);
            case FormatType.Xml:
                return this.xmlFormatter.Format(source);
            case FormatType.Html:
                return this.htmlFormatter.Format(source);
            default:
                throw new NotSupportedException("Fix this missing payload formatting type.");
        }
    }
}
