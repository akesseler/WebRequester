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

using Plexdata.LogWriter.Definitions;
using Plexdata.WebRequester.GUI.Extensions;
using Plexdata.WebRequester.GUI.Interfaces;

namespace Plexdata.WebRequester.GUI.Models.Logging
{
    internal class LoggerEntity : ListViewItem, ILoggerEntity
    {
        public LoggerEntity(LogLevel level, String message, Exception exception, (String Label, Object Value)[] details)
            : base()
        {
            this.Level = level;
            this.Message = message;
            this.Exception = exception;
            this.Details = details ?? Array.Empty<(String Label, Object Value)>();

            base.Text = level.GetDisplayText();
            base.BackColor = this.GetBackgroundColor(level);

            base.SubItems.Add(new ListViewItem.ListViewSubItem(this, message ?? String.Empty));
            base.SubItems.Add(new ListViewItem.ListViewSubItem(this, exception?.Message ?? String.Empty));
            base.SubItems.Add(new ListViewItem.ListViewSubItem(this, details?.Any() ?? false ? "More..." : String.Empty));
        }

        public LogLevel Level { get; private set; }

        public String Message { get; private set; }

        public Exception Exception { get; private set; }

        public (String Label, Object Value)[] Details { get; private set; }

        public ListViewItem ToListViewItem()
        {
            return this;
        }

        private Color GetBackgroundColor(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return Color.FromArgb(0x00F2F2F2);
                case LogLevel.Warning:
                    return Color.FromArgb(0x00FFFFCC);
                case LogLevel.Error:
                case LogLevel.Fatal:
                case LogLevel.Critical:
                case LogLevel.Disaster:
                    return Color.FromArgb(0x00FFB2B2);
                case LogLevel.Verbose:
                case LogLevel.Message:
                default:
                    return Color.White;
            }
        }
    }
}
