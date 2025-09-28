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
    partial class LoggerEntityDetailsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoggerEntityDetailsDialog));
            btClose = new Button();
            lbLevel = new Label();
            lbMessage = new Label();
            lbException = new Label();
            tlPanel = new TableLayoutPanel();
            txAdditionals = new TextBox();
            lbDetails = new Label();
            lvDetails = new ListView();
            clLabel = new ColumnHeader();
            clValue = new ColumnHeader();
            txLevel = new TextBox();
            txMessage = new TextBox();
            txException = new TextBox();
            llCopy = new LinkLabel();
            tlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btClose
            // 
            btClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btClose.Location = new Point(497, 426);
            btClose.Name = "btClose";
            btClose.Size = new Size(75, 23);
            btClose.TabIndex = 0;
            btClose.Text = "&Close";
            btClose.UseVisualStyleBackColor = true;
            // 
            // lbLevel
            // 
            lbLevel.AutoSize = true;
            lbLevel.Dock = DockStyle.Fill;
            lbLevel.Location = new Point(3, 0);
            lbLevel.Name = "lbLevel";
            lbLevel.Padding = new Padding(0, 6, 0, 0);
            lbLevel.Size = new Size(74, 30);
            lbLevel.TabIndex = 1;
            lbLevel.Text = "&Level:";
            // 
            // lbMessage
            // 
            lbMessage.AutoSize = true;
            lbMessage.Dock = DockStyle.Fill;
            lbMessage.Location = new Point(3, 30);
            lbMessage.Name = "lbMessage";
            lbMessage.Padding = new Padding(0, 6, 0, 0);
            lbMessage.Size = new Size(74, 30);
            lbMessage.TabIndex = 3;
            lbMessage.Text = "&Message:";
            // 
            // lbException
            // 
            lbException.AutoSize = true;
            lbException.Dock = DockStyle.Fill;
            lbException.Location = new Point(3, 60);
            lbException.Name = "lbException";
            lbException.Padding = new Padding(0, 6, 0, 0);
            lbException.Size = new Size(74, 30);
            lbException.TabIndex = 5;
            lbException.Text = "&Exception:";
            // 
            // tlPanel
            // 
            tlPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tlPanel.ColumnCount = 2;
            tlPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tlPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlPanel.Controls.Add(lbLevel, 0, 0);
            tlPanel.Controls.Add(lbException, 0, 2);
            tlPanel.Controls.Add(lbMessage, 0, 1);
            tlPanel.Controls.Add(txAdditionals, 1, 3);
            tlPanel.Controls.Add(lbDetails, 0, 4);
            tlPanel.Controls.Add(lvDetails, 1, 4);
            tlPanel.Controls.Add(txLevel, 1, 0);
            tlPanel.Controls.Add(txMessage, 1, 1);
            tlPanel.Controls.Add(txException, 1, 2);
            tlPanel.Location = new Point(12, 12);
            tlPanel.Name = "tlPanel";
            tlPanel.RowCount = 5;
            tlPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlPanel.Size = new Size(560, 408);
            tlPanel.TabIndex = 7;
            // 
            // txAdditionals
            // 
            txAdditionals.BackColor = SystemColors.Window;
            txAdditionals.Dock = DockStyle.Fill;
            txAdditionals.Location = new Point(80, 93);
            txAdditionals.Margin = new Padding(0, 3, 0, 3);
            txAdditionals.Multiline = true;
            txAdditionals.Name = "txAdditionals";
            txAdditionals.ReadOnly = true;
            txAdditionals.ScrollBars = ScrollBars.Both;
            txAdditionals.Size = new Size(480, 153);
            txAdditionals.TabIndex = 7;
            txAdditionals.WordWrap = false;
            // 
            // lbDetails
            // 
            lbDetails.AutoSize = true;
            lbDetails.Dock = DockStyle.Fill;
            lbDetails.Location = new Point(3, 249);
            lbDetails.Name = "lbDetails";
            lbDetails.Padding = new Padding(0, 9, 0, 0);
            lbDetails.Size = new Size(74, 159);
            lbDetails.TabIndex = 8;
            lbDetails.Text = "&Details:";
            // 
            // lvDetails
            // 
            lvDetails.Columns.AddRange(new ColumnHeader[] { clLabel, clValue });
            lvDetails.Dock = DockStyle.Fill;
            lvDetails.FullRowSelect = true;
            lvDetails.Location = new Point(80, 252);
            lvDetails.Margin = new Padding(0, 3, 0, 3);
            lvDetails.MultiSelect = false;
            lvDetails.Name = "lvDetails";
            lvDetails.Size = new Size(480, 153);
            lvDetails.TabIndex = 9;
            lvDetails.UseCompatibleStateImageBehavior = false;
            lvDetails.View = View.Details;
            // 
            // clLabel
            // 
            clLabel.Text = "Label";
            // 
            // clValue
            // 
            clValue.Text = "Value";
            // 
            // txLevel
            // 
            txLevel.BackColor = SystemColors.Window;
            txLevel.Dock = DockStyle.Fill;
            txLevel.Location = new Point(80, 3);
            txLevel.Margin = new Padding(0, 3, 0, 3);
            txLevel.Name = "txLevel";
            txLevel.ReadOnly = true;
            txLevel.Size = new Size(480, 23);
            txLevel.TabIndex = 10;
            // 
            // txMessage
            // 
            txMessage.BackColor = SystemColors.Window;
            txMessage.Dock = DockStyle.Fill;
            txMessage.Location = new Point(80, 33);
            txMessage.Margin = new Padding(0, 3, 0, 3);
            txMessage.Name = "txMessage";
            txMessage.ReadOnly = true;
            txMessage.Size = new Size(480, 23);
            txMessage.TabIndex = 11;
            // 
            // txException
            // 
            txException.BackColor = SystemColors.Window;
            txException.Dock = DockStyle.Fill;
            txException.Location = new Point(80, 63);
            txException.Margin = new Padding(0, 3, 0, 3);
            txException.Name = "txException";
            txException.ReadOnly = true;
            txException.Size = new Size(480, 23);
            txException.TabIndex = 12;
            // 
            // llCopy
            // 
            llCopy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            llCopy.AutoSize = true;
            llCopy.Location = new Point(12, 430);
            llCopy.Name = "llCopy";
            llCopy.Size = new Size(104, 15);
            llCopy.TabIndex = 8;
            llCopy.TabStop = true;
            llCopy.Text = "Copy to Clipboard";
            llCopy.LinkClicked += this.OnCopyLinkClicked;
            // 
            // LoggerEntityDetailsDialog
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = btClose;
            this.ClientSize = new Size(584, 461);
            this.Controls.Add(llCopy);
            this.Controls.Add(tlPanel);
            this.Controls.Add(btClose);
            this.DoubleBuffered = true;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new Size(600, 500);
            this.Name = "LoggerEntityDetailsDialog";
            this.SizeGripStyle = SizeGripStyle.Show;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Logging Message Details";
            tlPanel.ResumeLayout(false);
            tlPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Button btClose;
        private Label lbLevel;
        private Label lbMessage;
        private Label lbException;
        private TableLayoutPanel tlPanel;
        private TextBox txAdditionals;
        private Label lbDetails;
        private ListView lvDetails;
        private ColumnHeader clLabel;
        private ColumnHeader clValue;
        private TextBox txLevel;
        private TextBox txMessage;
        private TextBox txException;
        private LinkLabel llCopy;
    }
}