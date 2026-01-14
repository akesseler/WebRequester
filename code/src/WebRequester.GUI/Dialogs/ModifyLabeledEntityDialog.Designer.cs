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

namespace Plexdata.WebRequester.GUI.Dialogs
{
    partial class ModifyLabeledEntityDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModifyLabeledEntityDialog));
            btCancel = new Button();
            btAccept = new Button();
            lbLabel = new Label();
            lbNotes = new Label();
            txLabel = new TextBox();
            txNotes = new TextBox();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            btCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btCancel.DialogResult = DialogResult.Cancel;
            btCancel.Location = new Point(297, 126);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(75, 23);
            btCancel.TabIndex = 5;
            btCancel.Text = "&Cancel";
            btCancel.UseVisualStyleBackColor = true;
            // 
            // btAccept
            // 
            btAccept.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btAccept.DialogResult = DialogResult.OK;
            btAccept.Location = new Point(216, 126);
            btAccept.Name = "btAccept";
            btAccept.Size = new Size(75, 23);
            btAccept.TabIndex = 4;
            btAccept.Text = "&Accept";
            btAccept.UseVisualStyleBackColor = true;
            // 
            // lbLabel
            // 
            lbLabel.AutoSize = true;
            lbLabel.Location = new Point(12, 15);
            lbLabel.Name = "lbLabel";
            lbLabel.Size = new Size(35, 15);
            lbLabel.TabIndex = 0;
            lbLabel.Text = "&Label";
            // 
            // lbNotes
            // 
            lbNotes.AutoSize = true;
            lbNotes.Location = new Point(12, 44);
            lbNotes.Name = "lbNotes";
            lbNotes.Size = new Size(38, 15);
            lbNotes.TabIndex = 2;
            lbNotes.Text = "&Notes";
            // 
            // txLabel
            // 
            txLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txLabel.Location = new Point(80, 12);
            txLabel.Name = "txLabel";
            txLabel.Size = new Size(292, 23);
            txLabel.TabIndex = 1;
            // 
            // txNotes
            // 
            txNotes.AcceptsReturn = true;
            txNotes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txNotes.Location = new Point(80, 41);
            txNotes.Multiline = true;
            txNotes.Name = "txNotes";
            txNotes.ScrollBars = ScrollBars.Both;
            txNotes.Size = new Size(292, 79);
            txNotes.TabIndex = 3;
            // 
            // ModifyLabeledEntityDialog
            // 
            this.AcceptButton = btAccept;
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = btCancel;
            this.ClientSize = new Size(384, 161);
            this.Controls.Add(txNotes);
            this.Controls.Add(txLabel);
            this.Controls.Add(lbNotes);
            this.Controls.Add(lbLabel);
            this.Controls.Add(btAccept);
            this.Controls.Add(btCancel);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new Size(400, 200);
            this.Name = "ModifyLabeledEntityDialog";
            this.SizeGripStyle = SizeGripStyle.Show;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Modify";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Button btCancel;
        private Button btAccept;
        private Label lbLabel;
        private Label lbNotes;
        private TextBox txLabel;
        private TextBox txNotes;
    }
}