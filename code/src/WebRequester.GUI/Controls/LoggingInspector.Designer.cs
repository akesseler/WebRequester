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

namespace Plexdata.WebRequester.GUI.Controls
{
    partial class LoggingInspector
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
            lvItems = new ListView();
            colLevel = new ColumnHeader();
            colMessage = new ColumnHeader();
            colException = new ColumnHeader();
            colDetails = new ColumnHeader();
            tbMain = new ToolStrip();
            btClear = new ToolStripButton();
            btSave = new ToolStripButton();
            btFilter = new ToolStripDropDownButton();
            sbMain = new StatusStrip();
            slMain = new ToolStripStatusLabel();
            tbMain.SuspendLayout();
            sbMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvItems
            // 
            lvItems.Activation = ItemActivation.OneClick;
            lvItems.BorderStyle = BorderStyle.None;
            lvItems.Columns.AddRange(new ColumnHeader[] { colLevel, colMessage, colException, colDetails });
            lvItems.Dock = DockStyle.Fill;
            lvItems.FullRowSelect = true;
            lvItems.Location = new Point(0, 25);
            lvItems.Name = "lvItems";
            lvItems.Size = new Size(788, 119);
            lvItems.TabIndex = 1;
            lvItems.UseCompatibleStateImageBehavior = false;
            lvItems.View = View.Details;
            lvItems.VirtualMode = true;
            lvItems.RetrieveVirtualItem += this.OnRetrieveVirtualItem;
            lvItems.MouseDoubleClick += this.OnListViewMouseDoubleClick;
            // 
            // colLevel
            // 
            colLevel.Text = "Level";
            // 
            // colMessage
            // 
            colMessage.Text = "Message";
            // 
            // colException
            // 
            colException.Text = "Exception";
            // 
            // colDetails
            // 
            colDetails.Text = "Details";
            // 
            // tbMain
            // 
            tbMain.Items.AddRange(new ToolStripItem[] { btClear, btSave, btFilter });
            tbMain.Location = new Point(0, 0);
            tbMain.Name = "tbMain";
            tbMain.Size = new Size(788, 25);
            tbMain.TabIndex = 2;
            tbMain.Text = "tbMain";
            // 
            // btClear
            // 
            btClear.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btClear.Image = Properties.Resources.DeleteSmall;
            btClear.ImageTransparentColor = Color.Magenta;
            btClear.Name = "btClear";
            btClear.Size = new Size(23, 22);
            btClear.Text = "Clear";
            btClear.ToolTipText = "Remove all logging messages.";
            btClear.Click += this.OnButtonClearClicked;
            // 
            // btSave
            // 
            btSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btSave.Image = Properties.Resources.SaveSmall;
            btSave.ImageTransparentColor = Color.Magenta;
            btSave.Name = "btSave";
            btSave.Size = new Size(23, 22);
            btSave.Text = "Save";
            btSave.ToolTipText = "Save all logging messages.";
            btSave.Click += this.OnButtonSaveClicked;
            // 
            // btFilter
            // 
            btFilter.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btFilter.Image = Properties.Resources.FilterSmall;
            btFilter.ImageTransparentColor = Color.Magenta;
            btFilter.Name = "btFilter";
            btFilter.Size = new Size(29, 22);
            btFilter.Text = "Filter";
            btFilter.ToolTipText = "Filter all visible logging messages.";
            // 
            // sbMain
            // 
            sbMain.Items.AddRange(new ToolStripItem[] { slMain });
            sbMain.Location = new Point(0, 144);
            sbMain.Name = "sbMain";
            sbMain.Size = new Size(788, 22);
            sbMain.SizingGrip = false;
            sbMain.TabIndex = 3;
            // 
            // slMain
            // 
            slMain.Name = "slMain";
            slMain.Size = new Size(22, 17);
            slMain.Text = "???";
            // 
            // LoggingControl
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(788, 166);
            this.Controls.Add(lvItems);
            this.Controls.Add(tbMain);
            this.Controls.Add(sbMain);
            this.DoubleBuffered = true;
            this.HideOnClose = true;
            this.Name = "LoggingControl";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Logging";
            this.VisibleChanged += this.OnFormVisibleChanged;
            tbMain.ResumeLayout(false);
            tbMain.PerformLayout();
            sbMain.ResumeLayout(false);
            sbMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private ListView lvItems;
        private ColumnHeader colLevel;
        private ColumnHeader colMessage;
        private ColumnHeader colException;
        private ColumnHeader colDetails;
        private ToolStrip tbMain;
        private ToolStripButton btClear;
        private ToolStripDropDownButton btFilter;
        private StatusStrip sbMain;
        private ToolStripStatusLabel slMain;
        private ToolStripButton btSave;
    }
}