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
using System.ComponentModel;

namespace Plexdata.WebRequester.GUI.Controls.General
{
    public class TabControlEx : TabControl
    {
        private const Int32 TCM_FIRST = 0x1300;
        private const Int32 TCM_ADJUSTRECT = (TCM_FIRST + 40);

        public TabControlEx()
            : base()
        {
            base.DoubleBuffered = true;
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Boolean HideTabs { get; set; }

        protected override CreateParams CreateParams
        {
            get
            {
                // Attention: The WS_EX_COMPOSITED style is disabled under Win XP.
                // Setting the COMPOSITED window style works very fine under Window 7. But 
                // under Windows XP, this style causes some sub-controls to be drawn very 
                // ugly. Additionally, the resizing behaviour is also very bad. Therefore, 
                // this feature is not supported for Windows XP, with the result that the 
                // Setting dialog flickers a bit.
                CreateParams cp = base.CreateParams;

                if (this.IsVistaOrHigher())
                {
                    // This makes the tab control flicker free.
                    cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                }
                return cp;
            }
        }

        protected override void WndProc(ref Message message)
        {
            // Hide tabs by trapping the TCM_ADJUSTRECT message.
            // http://stackoverflow.com/questions/6953487/remove-hide-tab-headerswitcher-of-c-sharp-tabcontrol
            if (message.Msg == TCM_ADJUSTRECT && this.CanHideTabs())
            {
                message.Result = (IntPtr)1;
            }
            else
            {
                base.WndProc(ref message);
            }
        }

        private Boolean CanHideTabs()
        {
            return this.HideTabs && !base.DesignMode;
        }
    }
}
