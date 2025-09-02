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

namespace Plexdata.WebRequester.GUI.Dialogs
{
    public partial class ExecutionPendingDialog : Form
    {
        private static ExecutionPendingDialog instance;

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
            if (ExecutionPendingDialog.instance != null)
            {
                return;
            }

            ExecutionPendingDialog.instance = ExecutionPendingDialog.CreateInstance(parent, cancel);
            ExecutionPendingDialog.instance.Show();
        }

        public static void Dismiss()
        {
            if (ExecutionPendingDialog.instance == null)
            {
                return;
            }

            ExecutionPendingDialog.instance.Close();
            ExecutionPendingDialog.instance = null;
        }

        private static ExecutionPendingDialog CreateInstance(Control parent, CancellationTokenSource cancel)
        {
            ExecutionPendingDialog instance = new ExecutionPendingDialog(cancel);

            Form form = ExecutionPendingDialog.GetTopMostParentForm(parent);

            if (form != null)
            {
                Int32 x = form.Location.X + (form.Size.Width - instance.Size.Width) / 2;
                Int32 y = form.Location.Y + (form.Size.Height - instance.Size.Height) / 2;
                instance.Location = new Point(x, y);
            }
            else
            {
                instance.StartPosition = FormStartPosition.CenterScreen;
            }

            return instance;

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
