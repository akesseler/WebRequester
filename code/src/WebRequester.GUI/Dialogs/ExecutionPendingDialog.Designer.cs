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
    partial class ExecutionPendingDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExecutionPendingDialog));
            tpLayout = new TableLayoutPanel();
            btCancel = new Button();
            lbMessage = new Label();
            tpLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpLayout
            // 
            tpLayout.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tpLayout.ColumnCount = 3;
            tpLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tpLayout.ColumnStyles.Add(new ColumnStyle());
            tpLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tpLayout.Controls.Add(btCancel, 1, 1);
            tpLayout.Controls.Add(lbMessage, 0, 0);
            tpLayout.Location = new Point(12, 12);
            tpLayout.Name = "tpLayout";
            tpLayout.RowCount = 2;
            tpLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tpLayout.RowStyles.Add(new RowStyle());
            tpLayout.Size = new Size(260, 87);
            tpLayout.TabIndex = 0;
            // 
            // btCancel
            // 
            btCancel.Dock = DockStyle.Fill;
            btCancel.Location = new Point(92, 64);
            btCancel.Margin = new Padding(0, 6, 0, 0);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(75, 23);
            btCancel.TabIndex = 0;
            btCancel.Text = "&Cancel";
            btCancel.UseVisualStyleBackColor = true;
            btCancel.Click += this.OnButtonCancelClicked;
            // 
            // lbMessage
            // 
            lbMessage.AutoSize = true;
            tpLayout.SetColumnSpan(lbMessage, 3);
            lbMessage.Dock = DockStyle.Fill;
            lbMessage.Location = new Point(3, 0);
            lbMessage.Name = "lbMessage";
            lbMessage.Size = new Size(254, 58);
            lbMessage.TabIndex = 1;
            lbMessage.Text = "The request is currently being executed...";
            lbMessage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ExecutionPendingDialog
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = btCancel;
            this.ClientSize = new Size(284, 111);
            this.Controls.Add(tpLayout);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.MaximumSize = new Size(300, 150);
            this.MinimizeBox = false;
            this.MinimumSize = new Size(300, 150);
            this.Name = "ExecutionPendingDialog";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.StartPosition = FormStartPosition.Manual;
            this.Text = "Running...";
            this.TopMost = true;
            tpLayout.ResumeLayout(false);
            tpLayout.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tpLayout;
        private Button btCancel;
        private Label lbMessage;
    }
}