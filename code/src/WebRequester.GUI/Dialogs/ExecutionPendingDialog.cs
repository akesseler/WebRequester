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

namespace Plexdata.WebRequester.GUI.Dialogs
{
    public partial class ExecutionPendingDialog : Form
    {
        private static ExecutionPendingDialog singleton;

        private readonly CancellationTokenSource cancellationTokenSource;

        private ExecutionPendingDialog()
            : base()
        {
            this.InitializeComponent();
        }

        private ExecutionPendingDialog(CancellationTokenSource cancellationTokenSource)
            : this()
        {
            this.Cursor = Cursors.AppStarting;
            this.cancellationTokenSource = cancellationTokenSource;
        }

        public static void Display(Control parent, CancellationTokenSource cancel)
        {
            if (ExecutionPendingDialog.singleton != null)
            {
                return;
            }

            ExecutionPendingDialog.singleton = ExecutionPendingDialog.CreateInstance(parent, cancel);
            ExecutionPendingDialog.singleton.Show();
        }

        public static void Dismiss()
        {
            if (ExecutionPendingDialog.singleton == null)
            {
                return;
            }

            ExecutionPendingDialog.singleton.Close();
            ExecutionPendingDialog.singleton = null;
        }

        private static ExecutionPendingDialog CreateInstance(Control parent, CancellationTokenSource cancel)
        {
            ExecutionPendingDialog dialog = new ExecutionPendingDialog(cancel);

            Form window = ExecutionPendingDialog.GetTopMostParentForm(parent);

            if (window != null)
            {
                window.Enabled = false;
                dialog.Shown += (s, e) => dialog.BringToFront();
                window.Activated += (s, e) => dialog.BringToFront();
                dialog.FormClosed += (s, e) => window.Enabled = true;

                Int32 x = window.Location.X + (window.Size.Width - dialog.Size.Width) / 2;
                Int32 y = window.Location.Y + (window.Size.Height - dialog.Size.Height) / 2;

                dialog.Location = new Point(x, y);
                dialog.Owner = window;
            }
            else
            {
                dialog.StartPosition = FormStartPosition.CenterScreen;
            }

            return dialog;
        }

        private static Form GetTopMostParentForm(Control control)
        {
            if (control is Form && control.Parent == null)
            {
                return control as Form;
            }

            if (control.Parent == null)
            {
                return null;
            }

            return GetTopMostParentForm(control.Parent);
        }

        protected override void OnFormClosing(FormClosingEventArgs args)
        {
            this.btCancel.PerformClick();
            base.OnFormClosing(args);
        }

        private void OnButtonCancelClicked(Object sender, EventArgs args)
        {
            this.cancellationTokenSource.Cancel(true);
        }
    }
}
