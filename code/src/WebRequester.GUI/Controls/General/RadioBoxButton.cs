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

namespace Plexdata.WebRequester.GUI.Controls.General
{
    public class RadioBoxButton : RadioButton
    {
        public RadioBoxButton()
            : base()
        {
            base.Appearance = Appearance.Button;
            base.AutoSize = true;
            base.BackColor = Color.Transparent;
            base.Cursor = Cursors.Hand;
            base.FlatStyle = FlatStyle.Flat;
            base.FlatAppearance.BorderSize = 0;
            base.FlatAppearance.CheckedBackColor = Color.FromArgb(unchecked((Int32)0xFFD8EAF9));
            base.FlatAppearance.MouseDownBackColor = Color.FromArgb(unchecked((Int32)0xFFCCD5F0));
            base.FlatAppearance.MouseOverBackColor = Color.FromArgb(unchecked((Int32)0xFFD8EAF9));
            base.TextAlign = ContentAlignment.MiddleCenter;
            base.UseVisualStyleBackColor = false;
        }
    }
}
