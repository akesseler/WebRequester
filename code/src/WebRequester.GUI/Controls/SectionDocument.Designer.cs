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

namespace Plexdata.WebRequester.GUI.Controls
{
    partial class SectionDocument
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
            this.components = new System.ComponentModel.Container();
            pnLayout = new TableLayoutPanel();
            lbNotes = new Label();
            tcContainer = new TabControl();
            tpVariables = new TabPage();
            tlVariables = new TableLayoutPanel();
            lbVariables = new Label();
            pnVariables = new Panel();
            tpSecurity = new TabPage();
            tlSecurity = new TableLayoutPanel();
            lbSecurity = new Label();
            pnSecurity = new Panel();
            cmContext = new ContextMenuStrip(this.components);
            miCloseTab = new ToolStripMenuItem();
            miCloseAllTabs = new ToolStripMenuItem();
            pnLayout.SuspendLayout();
            tcContainer.SuspendLayout();
            tpVariables.SuspendLayout();
            tlVariables.SuspendLayout();
            tpSecurity.SuspendLayout();
            tlSecurity.SuspendLayout();
            cmContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnLayout
            // 
            pnLayout.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnLayout.ColumnCount = 1;
            pnLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnLayout.Controls.Add(lbNotes, 0, 0);
            pnLayout.Controls.Add(tcContainer, 0, 1);
            pnLayout.Location = new Point(12, 12);
            pnLayout.Name = "pnLayout";
            pnLayout.RowCount = 2;
            pnLayout.RowStyles.Add(new RowStyle());
            pnLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnLayout.Size = new Size(836, 525);
            pnLayout.TabIndex = 1;
            // 
            // lbNotes
            // 
            lbNotes.AutoSize = true;
            lbNotes.BackColor = Color.FromArgb(255, 255, 240);
            lbNotes.Dock = DockStyle.Fill;
            lbNotes.Location = new Point(0, 0);
            lbNotes.Margin = new Padding(0, 0, 0, 9);
            lbNotes.Name = "lbNotes";
            lbNotes.Padding = new Padding(10, 3, 10, 3);
            lbNotes.Size = new Size(836, 21);
            lbNotes.TabIndex = 0;
            lbNotes.Text = "??? leave here but initialize at runtime ???";
            // 
            // tcContainer
            // 
            tcContainer.Controls.Add(tpVariables);
            tcContainer.Controls.Add(tpSecurity);
            tcContainer.Dock = DockStyle.Fill;
            tcContainer.Location = new Point(0, 30);
            tcContainer.Margin = new Padding(0);
            tcContainer.Name = "tcContainer";
            tcContainer.Padding = new Point(10, 5);
            tcContainer.SelectedIndex = 0;
            tcContainer.Size = new Size(836, 495);
            tcContainer.SizeMode = TabSizeMode.Fixed;
            tcContainer.TabIndex = 1;
            // 
            // tpVariables
            // 
            tpVariables.Controls.Add(tlVariables);
            tpVariables.Location = new Point(4, 28);
            tpVariables.Margin = new Padding(0);
            tpVariables.Name = "tpVariables";
            tpVariables.Size = new Size(828, 463);
            tpVariables.TabIndex = 0;
            tpVariables.Text = "Variables";
            tpVariables.UseVisualStyleBackColor = true;
            // 
            // tlVariables
            // 
            tlVariables.ColumnCount = 1;
            tlVariables.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlVariables.Controls.Add(lbVariables, 0, 0);
            tlVariables.Controls.Add(pnVariables, 0, 1);
            tlVariables.Dock = DockStyle.Fill;
            tlVariables.Location = new Point(0, 0);
            tlVariables.Margin = new Padding(0);
            tlVariables.Name = "tlVariables";
            tlVariables.RowCount = 2;
            tlVariables.RowStyles.Add(new RowStyle());
            tlVariables.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlVariables.Size = new Size(828, 463);
            tlVariables.TabIndex = 0;
            // 
            // lbVariables
            // 
            lbVariables.AutoSize = true;
            lbVariables.BackColor = Color.FromArgb(255, 255, 240);
            lbVariables.Dock = DockStyle.Fill;
            lbVariables.Location = new Point(0, 0);
            lbVariables.Margin = new Padding(0, 0, 0, 9);
            lbVariables.Name = "lbVariables";
            lbVariables.Padding = new Padding(10, 3, 10, 3);
            lbVariables.Size = new Size(828, 21);
            lbVariables.TabIndex = 1;
            lbVariables.Text = "??? leave here but initialize at runtime ???";
            // 
            // pnVariables
            // 
            pnVariables.Dock = DockStyle.Fill;
            pnVariables.Location = new Point(0, 30);
            pnVariables.Margin = new Padding(0);
            pnVariables.Name = "pnVariables";
            pnVariables.Size = new Size(828, 433);
            pnVariables.TabIndex = 2;
            // 
            // tpSecurity
            // 
            tpSecurity.Controls.Add(tlSecurity);
            tpSecurity.Location = new Point(4, 28);
            tpSecurity.Name = "tpSecurity";
            tpSecurity.Size = new Size(828, 463);
            tpSecurity.TabIndex = 1;
            tpSecurity.Text = "Security";
            tpSecurity.UseVisualStyleBackColor = true;
            // 
            // tlSecurity
            // 
            tlSecurity.ColumnCount = 1;
            tlSecurity.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlSecurity.Controls.Add(lbSecurity, 0, 0);
            tlSecurity.Controls.Add(pnSecurity, 0, 1);
            tlSecurity.Dock = DockStyle.Fill;
            tlSecurity.Location = new Point(0, 0);
            tlSecurity.Margin = new Padding(0);
            tlSecurity.Name = "tlSecurity";
            tlSecurity.RowCount = 2;
            tlSecurity.RowStyles.Add(new RowStyle());
            tlSecurity.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlSecurity.Size = new Size(828, 463);
            tlSecurity.TabIndex = 1;
            // 
            // lbSecurity
            // 
            lbSecurity.AutoSize = true;
            lbSecurity.BackColor = Color.FromArgb(255, 255, 240);
            lbSecurity.Dock = DockStyle.Fill;
            lbSecurity.Location = new Point(0, 0);
            lbSecurity.Margin = new Padding(0, 0, 0, 9);
            lbSecurity.Name = "lbSecurity";
            lbSecurity.Padding = new Padding(10, 3, 10, 3);
            lbSecurity.Size = new Size(828, 21);
            lbSecurity.TabIndex = 1;
            lbSecurity.Text = "??? leave here but initialize at runtime ???";
            // 
            // pnSecurity
            // 
            pnSecurity.Dock = DockStyle.Fill;
            pnSecurity.Location = new Point(0, 30);
            pnSecurity.Margin = new Padding(0);
            pnSecurity.Name = "pnSecurity";
            pnSecurity.Size = new Size(828, 433);
            pnSecurity.TabIndex = 2;
            // 
            // cmContext
            // 
            cmContext.Items.AddRange(new ToolStripItem[] { miCloseTab, miCloseAllTabs });
            cmContext.Name = "cmContext";
            cmContext.Size = new Size(181, 70);
            // 
            // miCloseTab
            // 
            miCloseTab.Image = Properties.Resources.CloseThisWindowSmall;
            miCloseTab.Name = "miCloseTab";
            miCloseTab.Size = new Size(180, 22);
            miCloseTab.Text = "Close Tab";
            miCloseTab.Click += this.OnDocumentContextMenuItemClicked;
            // 
            // miCloseAllTabs
            // 
            miCloseAllTabs.Image = Properties.Resources.CloseAllWindowsSmall;
            miCloseAllTabs.Name = "miCloseAllTabs";
            miCloseAllTabs.Size = new Size(180, 22);
            miCloseAllTabs.Text = "Close All Tabs";
            miCloseAllTabs.Click += this.OnDocumentContextMenuItemClicked;
            // 
            // SectionDocument
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(860, 549);
            this.Controls.Add(pnLayout);
            this.Name = "SectionDocument";
            this.TabPageContextMenuStrip = cmContext;
            this.Text = "SectionDocument";
            pnLayout.ResumeLayout(false);
            pnLayout.PerformLayout();
            tcContainer.ResumeLayout(false);
            tpVariables.ResumeLayout(false);
            tlVariables.ResumeLayout(false);
            tlVariables.PerformLayout();
            tpSecurity.ResumeLayout(false);
            tlSecurity.ResumeLayout(false);
            tlSecurity.PerformLayout();
            cmContext.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel pnLayout;
        private Label lbNotes;
        private TabControl tcContainer;
        private TabPage tpVariables;
        private TableLayoutPanel tlVariables;
        private Label lbVariables;
        private Panel pnVariables;
        private TabPage tpSecurity;
        private TableLayoutPanel tlSecurity;
        private Label lbSecurity;
        private Panel pnSecurity;
        private ContextMenuStrip cmContext;
        private ToolStripMenuItem miCloseTab;
        private ToolStripMenuItem miCloseAllTabs;
    }
}