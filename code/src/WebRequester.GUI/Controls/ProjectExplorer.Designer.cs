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

using Plexdata.WebRequester.GUI.Controls.General;

namespace Plexdata.WebRequester.GUI.Controls
{
    partial class ProjectExplorer
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
            sbMain = new StatusStrip();
            tbMain = new ToolStrip();
            btInsert = new ToolStripSplitButton();
            miProject = new ToolStripMenuItem();
            miSection = new ToolStripMenuItem();
            miRequest = new ToolStripMenuItem();
            btRemove = new ToolStripButton();
            btRename = new ToolStripButton();
            tvProjects = new TreeViewEx();
            msContext = new ContextMenuStrip(this.components);
            miCreate = new ToolStripMenuItem();
            miPrompt = new ToolStripMenuItem();
            miSeparator1 = new ToolStripSeparator();
            miCopy = new ToolStripMenuItem();
            miCut = new ToolStripMenuItem();
            miPaste = new ToolStripMenuItem();
            miSeparator2 = new ToolStripSeparator();
            miRename = new ToolStripMenuItem();
            miRemove = new ToolStripMenuItem();
            tbMain.SuspendLayout();
            msContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // sbMain
            // 
            sbMain.Location = new Point(0, 428);
            sbMain.Name = "sbMain";
            sbMain.Size = new Size(800, 22);
            sbMain.TabIndex = 0;
            sbMain.Text = "statusStrip1";
            // 
            // tbMain
            // 
            tbMain.Items.AddRange(new ToolStripItem[] { btInsert, btRemove, btRename });
            tbMain.Location = new Point(0, 0);
            tbMain.Name = "tbMain";
            tbMain.Size = new Size(800, 25);
            tbMain.TabIndex = 1;
            tbMain.Text = "toolStrip1";
            // 
            // btInsert
            // 
            btInsert.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btInsert.DropDownItems.AddRange(new ToolStripItem[] { miProject, miSection, miRequest });
            btInsert.Image = Properties.Resources.AddSmall;
            btInsert.ImageTransparentColor = Color.Magenta;
            btInsert.Name = "btInsert";
            btInsert.Size = new Size(32, 22);
            btInsert.Text = "Insert";
            btInsert.ToolTipText = "Create and insert new entry.";
            btInsert.DropDownOpening += this.OnInsertButtonDropDownOpening;
            btInsert.Click += this.OnInsertButtonClicked;
            // 
            // miProject
            // 
            miProject.Name = "miProject";
            miProject.Size = new Size(180, 22);
            miProject.Text = "Project";
            miProject.Click += this.OnInsertButtonChildMenuClicked;
            // 
            // miSection
            // 
            miSection.Name = "miSection";
            miSection.Size = new Size(180, 22);
            miSection.Text = "Section";
            miSection.Click += this.OnInsertButtonChildMenuClicked;
            // 
            // miRequest
            // 
            miRequest.Name = "miRequest";
            miRequest.Size = new Size(180, 22);
            miRequest.Text = "Request";
            miRequest.Click += this.OnInsertButtonChildMenuClicked;
            // 
            // btRemove
            // 
            btRemove.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btRemove.Image = Properties.Resources.DelSmall;
            btRemove.ImageTransparentColor = Color.Magenta;
            btRemove.Name = "btRemove";
            btRemove.Size = new Size(23, 22);
            btRemove.Text = "Remove";
            btRemove.ToolTipText = "Remove selected entry and all its children.";
            btRemove.Click += this.OnRemoveButtonClicked;
            // 
            // btRename
            // 
            btRename.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btRename.Image = Properties.Resources.PencilSmall;
            btRename.ImageTransparentColor = Color.Magenta;
            btRename.Name = "btRename";
            btRename.Size = new Size(23, 22);
            btRename.Text = "Rename";
            btRename.ToolTipText = "Rename selected entry.";
            btRename.Click += this.OnRenameButtonClicked;
            // 
            // tvProjects
            // 
            tvProjects.BorderStyle = BorderStyle.None;
            tvProjects.ContextMenuStrip = msContext;
            tvProjects.Dock = DockStyle.Fill;
            tvProjects.FullRowSelect = true;
            tvProjects.HideSelection = false;
            tvProjects.Location = new Point(0, 25);
            tvProjects.Name = "tvProjects";
            tvProjects.ShowLines = false;
            tvProjects.ShowNodeToolTips = true;
            tvProjects.Size = new Size(800, 403);
            tvProjects.TabIndex = 2;
            tvProjects.AfterSelect += this.OnProjectsTreeNodeAfterSelect;
            tvProjects.NodeMouseClick += this.OnProjectsTreeNodeMouseClick;
            tvProjects.NodeMouseDoubleClick += this.OnProjectsTreeNodeMouseDoubleClick;
            tvProjects.KeyUp += this.OnProjectsTreeNodeKeyUp;
            // 
            // msContext
            // 
            msContext.Items.AddRange(new ToolStripItem[] { miCreate, miPrompt, miSeparator1, miCopy, miCut, miPaste, miSeparator2, miRename, miRemove });
            msContext.Name = "contextMenuStrip1";
            msContext.Size = new Size(118, 148);
            msContext.Opening += this.OnContextMenuOpening;
            // 
            // miCreate
            // 
            miCreate.Image = Properties.Resources.AddSmall;
            miCreate.Name = "miCreate";
            miCreate.Size = new Size(117, 22);
            miCreate.Text = "New";
            miCreate.Click += this.OnContextMenuCreateClicked;
            // 
            // miPrompt
            // 
            miPrompt.Image = Properties.Resources.OpenSmall;
            miPrompt.Name = "miPrompt";
            miPrompt.Size = new Size(117, 22);
            miPrompt.Text = "???";
            miPrompt.Click += this.OnContextMenuPromptClicked;
            // 
            // miSeparator1
            // 
            miSeparator1.Name = "miSeparator1";
            miSeparator1.Size = new Size(114, 6);
            // 
            // miCopy
            // 
            miCopy.Image = Properties.Resources.CopySmall;
            miCopy.Name = "miCopy";
            miCopy.Size = new Size(117, 22);
            miCopy.Text = "Copy";
            miCopy.Click += this.OnContextMenuCopyClicked;
            // 
            // miCut
            // 
            miCut.Image = Properties.Resources.CutSmall;
            miCut.Name = "miCut";
            miCut.Size = new Size(117, 22);
            miCut.Text = "Cut";
            miCut.Click += this.OnContextMenuCutClicked;
            // 
            // miPaste
            // 
            miPaste.Image = Properties.Resources.PasteSmall;
            miPaste.Name = "miPaste";
            miPaste.Size = new Size(117, 22);
            miPaste.Text = "Paste";
            miPaste.Click += this.OnContextMenuPasteClicked;
            // 
            // miSeparator2
            // 
            miSeparator2.Name = "miSeparator2";
            miSeparator2.Size = new Size(114, 6);
            // 
            // miRename
            // 
            miRename.Image = Properties.Resources.PencilSmall;
            miRename.Name = "miRename";
            miRename.Size = new Size(117, 22);
            miRename.Text = "Rename";
            miRename.Click += this.OnContextMenuRenameClicked;
            // 
            // miRemove
            // 
            miRemove.Image = Properties.Resources.DeleteSmall;
            miRemove.Name = "miRemove";
            miRemove.Size = new Size(117, 22);
            miRemove.Text = "Remove";
            miRemove.Click += this.OnContextMenuRemoveClicked;
            // 
            // ProjectExplorer
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(tvProjects);
            this.Controls.Add(tbMain);
            this.Controls.Add(sbMain);
            this.HideOnClose = true;
            this.Name = "ProjectExplorer";
            this.Text = "Projects";
            tbMain.ResumeLayout(false);
            tbMain.PerformLayout();
            msContext.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private StatusStrip sbMain;
        private ToolStrip tbMain;
        private TreeViewEx tvProjects;
        private ToolStripButton btRemove;
        private ToolStripButton btRename;
        private ToolStripSplitButton btInsert;
        private ToolStripMenuItem miProject;
        private ToolStripMenuItem miSection;
        private ToolStripMenuItem miRequest;
        private ContextMenuStrip msContext;
        private ToolStripMenuItem miCopy;
        private ToolStripMenuItem miCut;
        private ToolStripMenuItem miPaste;
        private ToolStripMenuItem miCreate;
        private ToolStripMenuItem miPrompt;
        private ToolStripSeparator miSeparator1;
        private ToolStripMenuItem miRemove;
        private ToolStripSeparator miSeparator2;
        private ToolStripMenuItem miRename;
    }
}