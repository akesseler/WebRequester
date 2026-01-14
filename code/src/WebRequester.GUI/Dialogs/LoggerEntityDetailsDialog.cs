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

using Plexdata.WebRequester.GUI.Extensions;
using Plexdata.WebRequester.GUI.Helpers;
using Plexdata.WebRequester.GUI.Interfaces;

namespace Plexdata.WebRequester.GUI.Dialogs
{
    internal partial class LoggerEntityDetailsDialog : Form, ILoggerEntityDetails
    {
        private readonly ILoggerEntity entity = null;
        private readonly ISerializer serializer = null;

        public LoggerEntityDetailsDialog(ILoggerEntity entity, ISerializer serializer)
            : base()
        {
            this.entity = entity ?? throw new ArgumentNullException(nameof(entity));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            this.InitializeComponent();
            this.InitializeControls();
        }

        private void InitializeControls()
        {
            this.txLevel.Text = this.entity.Level.GetDisplayText();
            this.txMessage.Text = this.entity.Message ?? String.Empty;
            this.txException.Text = this.entity.Exception?.Message ?? String.Empty;

            if (this.entity.Exception != null)
            {
                this.txAdditionals.Lines = new String[]
                {
                    String.Format("{0}: {1} (0x{1:X8})", nameof(this.entity.Exception.HResult), this.entity.Exception.HResult ),
                    String.Format("{0}: {1}", nameof(this.entity.Exception.StackTrace), this.entity.Exception.StackTrace?.Trim() ?? String.Empty)
                };
            }

            if (this.entity.Details?.Any() ?? false)
            {
                foreach ((String label, Object value) in this.entity.Details)
                {
                    this.lvDetails.Items.Add(new ListViewItem(new String[]
                    {
                        label ?? String.Empty,
                        value?.ToString() ?? String.Empty
                    }));
                }

                this.lvDetails.Columns[0].Width = -1;
                this.lvDetails.Columns[1].Width = -2;
            }
        }

        private void OnCopyLinkClicked(Object sender, LinkLabelLinkClickedEventArgs args)
        {
            try
            {
                using (new WaitCursor(this))
                {
                    Clipboard.SetDataObject(this.serializer.Serialize(this.entity.ToSerializable()));
                }
            }
            catch (Exception exception)
            {
                this.ShowError("Copying of all logging message details to clipboard failed unexpectedly.", this.Text, exception);
            }
        }
    }
}
