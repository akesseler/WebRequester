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

using Plexdata.WebRequester.GUI.Events;

namespace Plexdata.WebRequester.GUI.Controls.General
{
    public class TextBoxEx : TextBox
    {
        public event EventHandler<ClipboardEventArgs> Pasted;

        public TextBoxEx()
            : base()
        {
        }

        public void Replace(String value, Boolean selection = true)
        {
            if (base.ReadOnly)
            {
                return;
            }

            value ??= String.Empty;

            if (selection)
            {
                base.SelectedText = value;
            }
            else
            {
                base.Text = value;
            }
        }

        protected override void WndProc(ref Message message)
        {
            const Int32 WM_PASTE = 0x0302;

            if (message.Msg == WM_PASTE)
            {
                EventHandler<ClipboardEventArgs> pastedHandler = this.Pasted;

                if (pastedHandler != null)
                {
                    ClipboardEventArgs args = new ClipboardEventArgs(Clipboard.GetText());

                    pastedHandler(this, args);

                    return;
                }
            }

            base.WndProc(ref message);
        }
    }
}
