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

using WeifenLuo.WinFormsUI.Docking;

namespace Plexdata.WebRequester.GUI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            dpContainer = new DockPanel();
            msMain = new MenuStrip();
            miFile = new ToolStripMenuItem();
            miOpen = new ToolStripMenuItem();
            miSave = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            miImport = new ToolStripMenuItem();
            miRequesterProjects = new ToolStripMenuItem();
            miPostmanCollection = new ToolStripMenuItem();
            miExport = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            miExit = new ToolStripMenuItem();
            miView = new ToolStripMenuItem();
            miProject = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            miLogging = new ToolStripMenuItem();
            miHelp = new ToolStripMenuItem();
            miAbout = new ToolStripMenuItem();
            msMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // dpContainer
            // 
            dpContainer.Dock = DockStyle.Fill;
            dpContainer.DockBackColor = SystemColors.Control;
            dpContainer.Location = new Point(0, 24);
            dpContainer.Name = "dpContainer";
            dpContainer.Size = new Size(800, 426);
            dpContainer.TabIndex = 3;
            // 
            // msMain
            // 
            msMain.Items.AddRange(new ToolStripItem[] { miFile, miView, miHelp });
            msMain.Location = new Point(0, 0);
            msMain.Name = "msMain";
            msMain.Size = new Size(800, 24);
            msMain.TabIndex = 5;
            msMain.Text = "menuStrip1";
            // 
            // miFile
            // 
            miFile.DropDownItems.AddRange(new ToolStripItem[] { miOpen, miSave, toolStripMenuItem2, miImport, miExport, toolStripMenuItem3, miExit });
            miFile.Name = "miFile";
            miFile.Size = new Size(37, 20);
            miFile.Text = "&File";
            // 
            // miOpen
            // 
            miOpen.Image = Properties.Resources.OpenBlackSmall;
            miOpen.Name = "miOpen";
            miOpen.Size = new Size(135, 22);
            miOpen.Text = "&Open...";
            miOpen.ToolTipText = "Opens a previously saved file containing a set of projects.";
            miOpen.Click += this.OnOpenMenuItemClicked;
            // 
            // miSave
            // 
            miSave.Image = Properties.Resources.SaveBlackSmall;
            miSave.Name = "miSave";
            miSave.Size = new Size(135, 22);
            miSave.Text = "&Save As...";
            miSave.ToolTipText = "Saves all currently available projects into an external file.";
            miSave.Click += this.OnSaveMenuItemClicked;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(132, 6);
            // 
            // miImport
            // 
            miImport.DropDownItems.AddRange(new ToolStripItem[] { miRequesterProjects, miPostmanCollection });
            miImport.Image = Properties.Resources.ImportBlackSmall;
            miImport.Name = "miImport";
            miImport.Size = new Size(135, 22);
            miImport.Text = "&Import";
            // 
            // miRequesterProjects
            // 
            miRequesterProjects.Name = "miRequesterProjects";
            miRequesterProjects.Size = new Size(178, 22);
            miRequesterProjects.Text = "Requester Projects";
            miRequesterProjects.Click += this.OnImportMenuItemClicked;
            // 
            // miPostmanCollection
            // 
            miPostmanCollection.Name = "miPostmanCollection";
            miPostmanCollection.Size = new Size(178, 22);
            miPostmanCollection.Text = "Postman Collection";
            miPostmanCollection.Click += this.OnImportMenuItemClicked;
            // 
            // miExport
            // 
            miExport.Image = Properties.Resources.ExportBlackSmall;
            miExport.Name = "miExport";
            miExport.Size = new Size(135, 22);
            miExport.Text = "&Export...";
            miExport.TextAlign = ContentAlignment.MiddleLeft;
            miExport.Click += this.OnExportMenuItemClicked;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(132, 6);
            // 
            // miExit
            // 
            miExit.Image = Properties.Resources.ExitSmall;
            miExit.Name = "miExit";
            miExit.ShortcutKeys = Keys.Alt | Keys.F4;
            miExit.Size = new Size(135, 22);
            miExit.Text = "E&xit";
            miExit.Click += this.OnExitMenuClicked;
            // 
            // miView
            // 
            miView.DropDownItems.AddRange(new ToolStripItem[] { miProject, toolStripMenuItem1, miLogging });
            miView.Name = "miView";
            miView.Size = new Size(44, 20);
            miView.Text = "&View";
            // 
            // miProject
            // 
            miProject.CheckOnClick = true;
            miProject.Name = "miProject";
            miProject.ShortcutKeys = Keys.Control | Keys.Shift | Keys.P;
            miProject.Size = new Size(190, 22);
            miProject.Text = "&Project";
            miProject.CheckedChanged += this.OnProjectExplorerMenuCheckedChanged;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(187, 6);
            // 
            // miLogging
            // 
            miLogging.CheckOnClick = true;
            miLogging.Name = "miLogging";
            miLogging.ShortcutKeys = Keys.Control | Keys.Shift | Keys.L;
            miLogging.Size = new Size(190, 22);
            miLogging.Text = "&Logging";
            miLogging.CheckedChanged += this.OnLoggingMenuCheckedChanged;
            // 
            // miHelp
            // 
            miHelp.DropDownItems.AddRange(new ToolStripItem[] { miAbout });
            miHelp.Name = "miHelp";
            miHelp.Size = new Size(44, 20);
            miHelp.Text = "&Help";
            // 
            // miAbout
            // 
            miAbout.Image = Properties.Resources.InfoSmall;
            miAbout.Name = "miAbout";
            miAbout.Size = new Size(116, 22);
            miAbout.Text = "About...";
            miAbout.Click += this.OnAboutMenuClicked;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(dpContainer);
            this.Controls.Add(msMain);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.IsMdiContainer = true;
            this.MainMenuStrip = msMain;
            this.Name = "MainForm";
            this.Text = "Web Requester";
            msMain.ResumeLayout(false);
            msMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private DockPanel dpContainer;
        private MenuStrip msMain;
        private ToolStripMenuItem miFile;
        private ToolStripMenuItem miView;
        private ToolStripMenuItem miLogging;
        private ToolStripMenuItem miExit;
        private ToolStripMenuItem miProject;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem miHelp;
        private ToolStripMenuItem miAbout;
        private ToolStripMenuItem miImport;
        private ToolStripMenuItem miRequesterProjects;
        private ToolStripMenuItem miPostmanCollection;
        private ToolStripMenuItem miOpen;
        private ToolStripMenuItem miSave;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem miExport;
        private ToolStripSeparator toolStripMenuItem3;
    }
}