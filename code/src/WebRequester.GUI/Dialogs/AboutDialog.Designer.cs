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
    partial class AboutDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            picLogo = new PictureBox();
            lblProduct = new Label();
            lblVersion = new Label();
            lblCopyright = new Label();
            txtDescription = new TextBox();
            btnClose = new Button();
            ttpHelper = new ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            this.SuspendLayout();
            // 
            // picLogo
            // 
            picLogo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            picLogo.Cursor = Cursors.Hand;
            picLogo.Image = (Image)resources.GetObject("picLogo.Image");
            picLogo.Location = new Point(208, 14);
            picLogo.Margin = new Padding(4, 3, 4, 3);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(182, 76);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 12;
            picLogo.TabStop = false;
            ttpHelper.SetToolTip(picLogo, "Click here to open the homepage!");
            picLogo.Click += this.OnLogoClicked;
            // 
            // lblProduct
            // 
            lblProduct.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblProduct.Location = new Point(14, 14);
            lblProduct.Margin = new Padding(4, 3, 4, 3);
            lblProduct.Name = "lblProduct";
            lblProduct.Size = new Size(187, 20);
            lblProduct.TabIndex = 0;
            lblProduct.Text = "Product";
            lblProduct.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVersion
            // 
            lblVersion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblVersion.Location = new Point(14, 40);
            lblVersion.Margin = new Padding(4, 3, 4, 3);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(187, 20);
            lblVersion.TabIndex = 1;
            lblVersion.Text = "Version";
            lblVersion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCopyright
            // 
            lblCopyright.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblCopyright.Location = new Point(14, 67);
            lblCopyright.Margin = new Padding(4, 3, 4, 3);
            lblCopyright.Name = "lblCopyright";
            lblCopyright.Size = new Size(187, 20);
            lblCopyright.TabIndex = 2;
            lblCopyright.Text = "Copyright";
            lblCopyright.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDescription
            // 
            txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtDescription.BackColor = Color.White;
            txtDescription.Location = new Point(14, 97);
            txtDescription.Margin = new Padding(4, 3, 4, 3);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.ReadOnly = true;
            txtDescription.ScrollBars = ScrollBars.Both;
            txtDescription.Size = new Size(375, 114);
            txtDescription.TabIndex = 3;
            txtDescription.TabStop = false;
            txtDescription.Text = "Description";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Location = new Point(302, 218);
            btnClose.Margin = new Padding(4, 3, 4, 3);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(88, 29);
            btnClose.TabIndex = 5;
            btnClose.Text = "&Close";
            btnClose.Click += this.OnCloseButtonClicked;
            // 
            // ttpHelper
            // 
            ttpHelper.ShowAlways = true;
            // 
            // AboutDialog
            // 
            this.AcceptButton = btnClose;
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = btnClose;
            this.ClientSize = new Size(404, 261);
            this.Controls.Add(btnClose);
            this.Controls.Add(lblProduct);
            this.Controls.Add(picLogo);
            this.Controls.Add(lblVersion);
            this.Controls.Add(lblCopyright);
            this.Controls.Add(txtDescription);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Margin = new Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new Size(420, 300);
            this.Name = "AboutDialog";
            this.Padding = new Padding(10);
            this.ShowInTaskbar = false;
            this.SizeGripStyle = SizeGripStyle.Show;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "About ";
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private PictureBox picLogo;
        private Label lblProduct;
        private Label lblVersion;
        private Label lblCopyright;
        private TextBox txtDescription;
        private Button btnClose;
        private ToolTip ttpHelper;
    }
}
