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

using AngleSharp;
using AngleSharp.Css;
using AngleSharp.Css.Dom;
using AngleSharp.Css.Parser;
using AngleSharp.Dom;
using AngleSharp.Html;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Extensions;
using Plexdata.WebRequester.GUI.Definitions;
using Plexdata.WebRequester.GUI.Interfaces;

namespace Plexdata.WebRequester.GUI.Helpers.Formatters;

internal class HtmlFormatter : IHtmlFormatter
{
    // Note that the NuGet package is causing this warning.
    // NU1608: Detected package version outside of dependency constraint: AngleSharp.Css 0.17.0
    // requires AngleSharp (>= 0.17.0 && < 0.18.0) but version AngleSharp 1.3.0 was resolved.

    private static readonly String spc = SeparatorType.Indentation;
    private static readonly String eol = Environment.NewLine;
    private static readonly String sep = SeparatorType.UnitSeparator.ToString();
    private static readonly String cr = "\r";
    private static readonly String lf = "\n";
    private static readonly String sp = " ";
    private static readonly String style = "style";

    private readonly ILogger logger;

    private readonly HtmlParser htmParser;
    private readonly CssParser cssParser;
    private readonly BetterMarkupFormatter htmFormatter;
    private readonly PrettyStyleFormatter cssFormatter;

    public HtmlFormatter(ILogger logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        IConfiguration config = Configuration.Default.WithCss();
        IBrowsingContext context = BrowsingContext.New(config);

        HtmlParserOptions htmOptions = new HtmlParserOptions();
        this.htmParser = new HtmlParser(htmOptions, context);
        this.htmFormatter = new BetterMarkupFormatter() { Indentation = HtmlFormatter.spc, NewLine = HtmlFormatter.eol };

        CssParserOptions cssOptions = new CssParserOptions();
        this.cssParser = new CssParser(cssOptions);
        this.cssFormatter = new PrettyStyleFormatter() { Indentation = HtmlFormatter.spc, NewLine = HtmlFormatter.eol };
    }

    public String Format(String source)
    {
        try
        {
            String value = source.Replace(HtmlFormatter.cr, String.Empty);

            IHtmlDocument document = this.htmParser.ParseDocument(value);

            this.FormatStyleClasses(document);

            this.FormatInlineStyles(document);

            return document
                .ToHtml(this.htmFormatter)
                .Replace(HtmlFormatter.sep, HtmlFormatter.eol);
        }
        catch (Exception exception)
        {
            this.logger.Error("Applying HTML format failed.", exception);

            return source;
        }
    }

    private void FormatStyleClasses(IHtmlDocument document)
    {
        foreach (IElement element in document.QuerySelectorAll(HtmlFormatter.style))
        {
            Int32 indent = HtmlFormatter.Calculate(element);

            String padding = String.Empty.PadLeft((indent + 1) * HtmlFormatter.spc.Length, HtmlFormatter.spc[0]);

            ICssStyleSheet parsed = this.cssParser.ParseStyleSheet(element.TextContent);

            String[] lines = parsed.ToCss(cssFormatter).Split(HtmlFormatter.eol, StringSplitOptions.RemoveEmptyEntries);

            String content = String.Join(HtmlFormatter.eol, lines.Select(x => $"{padding}{x}"));

            element.TextContent = $"{HtmlFormatter.eol}{content}{HtmlFormatter.eol}";
        }
    }

    private void FormatInlineStyles(IHtmlDocument document)
    {
        foreach (IElement element in document.All)
        {
            if (!element.HasAttribute(HtmlFormatter.style))
            {
                continue;
            }

            String attribute = element.GetAttribute(HtmlFormatter.style);

            ICssStyleSheet parsed = this.cssParser.ParseStyleSheet(attribute);

            String content = parsed.ToCss();

            element.SetAttribute(HtmlFormatter.style, content);
        }
    }

    private static Int32 Calculate(IElement element)
    {
        if (element is null)
        {
            return 0;
        }

        Int32 depth = 0;
        IElement parent = element.ParentElement;

        while (parent is not null)
        {
            depth++;
            parent = parent.ParentElement;
        }

        return depth;
    }

    // The NuGet package is so buggy. Therefore, many tricks must be used to
    // get a halfway reasonably decent result. Major task here is to remove
    // all CRs and replace all LFs with something "unknown" so that, in the
    // end, everything "unknown" can be manually replaced by the wanted CRLF.
    private class BetterMarkupFormatter : PrettyMarkupFormatter
    {
        public BetterMarkupFormatter()
            : base()
        {
        }

        // This is especially for plain text formatting.
        public override String Text(ICharacterData text)
        {
            if (!String.IsNullOrWhiteSpace(text?.Data))
            {
                String padding = String.Empty;

                if ((this.Indentation?.Length ?? 0) > 0)
                {
                    padding = String.Empty.PadLeft(
                        this.Indentation.Length * (HtmlFormatter.Calculate(text.ParentElement) + 1),
                        this.Indentation[0]);
                }

                text.Data = String.Join(HtmlFormatter.sep, text.Data
                    .Replace(HtmlFormatter.cr, String.Empty)
                    .Replace(HtmlFormatter.lf, HtmlFormatter.sep)
                    .Split(HtmlFormatter.sep, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => $"{padding}{x.Trim()}"));
            }

            return base.Text(text);
        }

        // This is especially for CSS formatting.
        public override String LiteralText(ICharacterData text)
        {
            if (!String.IsNullOrWhiteSpace(text?.Data))
            {
                text.Data = text.Data
                    .Replace(HtmlFormatter.cr, String.Empty)
                    .TrimEnd()
                    .Replace(HtmlFormatter.lf, HtmlFormatter.sep);

                // Adding a final space cause the NuGet-Package to add the
                // closing tag on a new line (including correct padding).
                text.Data += HtmlFormatter.sp;
            }

            return base.LiteralText(text);
        }
    }
}
