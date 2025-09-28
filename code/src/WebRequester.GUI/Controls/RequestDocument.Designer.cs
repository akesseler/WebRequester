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

using Plexdata.WebRequester.GUI.Controls.Composed;
using Plexdata.WebRequester.GUI.Controls.General;

namespace Plexdata.WebRequester.GUI.Controls
{
    partial class RequestDocument
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
            cbMethods = new ComboBox();
            txUrl = new TextBoxEx();
            btSend = new Button();
            spContainer = new SplitContainerEx();
            tcRequestDetails = new TabControlEx();
            tpRequestQueries = new TabPage();
            tpRequestSecurity = new TabPage();
            tpRequestHeaders = new TabPage();
            tpRequestPayload = new TabPage();
            pnResponseDetails = new ResponseDetailsPanel();
            pnContainer = new Panel();
            pnLayout = new TableLayoutPanel();
            lbNotes = new Label();
            cmContext = new ContextMenuStrip(this.components);
            miCloseTab = new ToolStripMenuItem();
            miCloseAllTabs = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)spContainer).BeginInit();
            spContainer.Panel1.SuspendLayout();
            spContainer.Panel2.SuspendLayout();
            spContainer.SuspendLayout();
            tcRequestDetails.SuspendLayout();
            pnContainer.SuspendLayout();
            pnLayout.SuspendLayout();
            cmContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbMethods
            // 
            cbMethods.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMethods.FormattingEnabled = true;
            cbMethods.Location = new Point(0, 0);
            cbMethods.Margin = new Padding(0, 0, 6, 0);
            cbMethods.Name = "cbMethods";
            cbMethods.Size = new Size(100, 23);
            cbMethods.TabIndex = 0;
            cbMethods.SelectedIndexChanged += this.OnMethodsComboBoxSelectedIndexChanged;
            // 
            // txUrl
            // 
            txUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txUrl.Location = new Point(109, 0);
            txUrl.Name = "txUrl";
            txUrl.Size = new Size(651, 23);
            txUrl.TabIndex = 1;
            // 
            // btSend
            // 
            btSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btSend.Location = new Point(769, 0);
            btSend.Margin = new Padding(6, 0, 0, 0);
            btSend.Name = "btSend";
            btSend.Size = new Size(67, 23);
            btSend.TabIndex = 2;
            btSend.Text = "Send";
            btSend.UseVisualStyleBackColor = true;
            btSend.Click += this.OnButtonSendClicked;
            // 
            // spContainer
            // 
            spContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            spContainer.Location = new Point(0, 32);
            spContainer.Margin = new Padding(0, 6, 0, 0);
            spContainer.Name = "spContainer";
            spContainer.Orientation = Orientation.Horizontal;
            // 
            // spContainer.Panel1
            // 
            spContainer.Panel1.Controls.Add(tcRequestDetails);
            // 
            // spContainer.Panel2
            // 
            spContainer.Panel2.Controls.Add(pnResponseDetails);
            spContainer.Size = new Size(836, 463);
            spContainer.SplitterDistance = 224;
            spContainer.SplitterWidth = 8;
            spContainer.TabIndex = 3;
            spContainer.TabStop = false;
            // 
            // tcRequestDetails
            // 
            tcRequestDetails.Controls.Add(tpRequestQueries);
            tcRequestDetails.Controls.Add(tpRequestSecurity);
            tcRequestDetails.Controls.Add(tpRequestHeaders);
            tcRequestDetails.Controls.Add(tpRequestPayload);
            tcRequestDetails.Dock = DockStyle.Fill;
            tcRequestDetails.Location = new Point(0, 0);
            tcRequestDetails.Margin = new Padding(0);
            tcRequestDetails.Name = "tcRequestDetails";
            tcRequestDetails.Padding = new Point(10, 5);
            tcRequestDetails.SelectedIndex = 0;
            tcRequestDetails.Size = new Size(836, 224);
            tcRequestDetails.SizeMode = TabSizeMode.Fixed;
            tcRequestDetails.TabIndex = 0;
            // 
            // tpRequestQueries
            // 
            tpRequestQueries.Location = new Point(4, 28);
            tpRequestQueries.Margin = new Padding(0);
            tpRequestQueries.Name = "tpRequestQueries";
            tpRequestQueries.Size = new Size(828, 192);
            tpRequestQueries.TabIndex = 0;
            tpRequestQueries.Text = "Query";
            tpRequestQueries.UseVisualStyleBackColor = true;
            // 
            // tpRequestSecurity
            // 
            tpRequestSecurity.Location = new Point(4, 28);
            tpRequestSecurity.Margin = new Padding(0);
            tpRequestSecurity.Name = "tpRequestSecurity";
            tpRequestSecurity.Size = new Size(828, 192);
            tpRequestSecurity.TabIndex = 1;
            tpRequestSecurity.Text = "Security";
            tpRequestSecurity.UseVisualStyleBackColor = true;
            // 
            // tpRequestHeaders
            // 
            tpRequestHeaders.Location = new Point(4, 28);
            tpRequestHeaders.Margin = new Padding(0);
            tpRequestHeaders.Name = "tpRequestHeaders";
            tpRequestHeaders.Size = new Size(828, 192);
            tpRequestHeaders.TabIndex = 2;
            tpRequestHeaders.Text = "Headers";
            tpRequestHeaders.UseVisualStyleBackColor = true;
            // 
            // tpRequestPayload
            // 
            tpRequestPayload.Location = new Point(4, 28);
            tpRequestPayload.Margin = new Padding(0);
            tpRequestPayload.Name = "tpRequestPayload";
            tpRequestPayload.Size = new Size(828, 192);
            tpRequestPayload.TabIndex = 3;
            tpRequestPayload.Text = "Payload";
            tpRequestPayload.UseVisualStyleBackColor = true;
            // 
            // pnResponseDetails
            // 
            pnResponseDetails.BackColor = Color.Transparent;
            pnResponseDetails.Dock = DockStyle.Fill;
            pnResponseDetails.Location = new Point(0, 0);
            pnResponseDetails.Name = "pnResponseDetails";
            pnResponseDetails.Size = new Size(836, 231);
            pnResponseDetails.TabIndex = 0;
            pnResponseDetails.Text = "Response";
            pnResponseDetails.PanelHeightIncreased += this.OnResponseDetailsPanelHeightIncreased;
            pnResponseDetails.PanelHeightDecreased += this.OnResponseDetailsPanelHeightDecreased;
            // 
            // pnContainer
            // 
            pnContainer.BackColor = Color.Transparent;
            pnContainer.Controls.Add(btSend);
            pnContainer.Controls.Add(spContainer);
            pnContainer.Controls.Add(txUrl);
            pnContainer.Controls.Add(cbMethods);
            pnContainer.Dock = DockStyle.Fill;
            pnContainer.Location = new Point(0, 30);
            pnContainer.Margin = new Padding(0);
            pnContainer.Name = "pnContainer";
            pnContainer.Size = new Size(836, 495);
            pnContainer.TabIndex = 4;
            // 
            // pnLayout
            // 
            pnLayout.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnLayout.ColumnCount = 1;
            pnLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnLayout.Controls.Add(lbNotes, 0, 0);
            pnLayout.Controls.Add(pnContainer, 0, 1);
            pnLayout.Location = new Point(12, 12);
            pnLayout.Name = "pnLayout";
            pnLayout.RowCount = 2;
            pnLayout.RowStyles.Add(new RowStyle());
            pnLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnLayout.Size = new Size(836, 525);
            pnLayout.TabIndex = 5;
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
            // RequestDocument
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(860, 549);
            this.Controls.Add(pnLayout);
            this.Name = "RequestDocument";
            this.TabPageContextMenuStrip = cmContext;
            this.Text = "RequestDocument";
            spContainer.Panel1.ResumeLayout(false);
            spContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)spContainer).EndInit();
            spContainer.ResumeLayout(false);
            tcRequestDetails.ResumeLayout(false);
            pnContainer.ResumeLayout(false);
            pnContainer.PerformLayout();
            pnLayout.ResumeLayout(false);
            pnLayout.PerformLayout();
            cmContext.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
        private ComboBox cbMethods;
        private TextBoxEx txUrl;
        private Button btSend;
        private SplitContainerEx spContainer;
        private TabControlEx tcRequestDetails;
        private TabPage tpRequestQueries;
        private TabPage tpRequestSecurity;
        private TabPage tpRequestHeaders;
        private TabPage tpRequestPayload;
        private ResponseDetailsPanel pnResponseDetails;
        private Panel pnContainer;
        private TableLayoutPanel pnLayout;
        private Label lbNotes;
        private ContextMenuStrip cmContext;
        private ToolStripMenuItem miCloseTab;
        private ToolStripMenuItem miCloseAllTabs;
    }
}