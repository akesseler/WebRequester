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

using Plexdata.WebRequester.GUI.Controls.General;
using Plexdata.WebRequester.GUI.Definitions;
using Plexdata.WebRequester.GUI.Extensions;
using Plexdata.WebRequester.GUI.Interfaces;
using Plexdata.WebRequester.GUI.Models.Helpers;
using Plexdata.WebRequester.GUI.Models.Projects;
using System.ComponentModel;

namespace Plexdata.WebRequester.GUI.Controls.Composed
{
    internal class PayloadEntityEditorPanel : Panel, IPayloadEntityEditor<PayloadEntity>
    {
        #region Private Fields

        private IContainer components = null;
        private TableLayoutPanel pnHeader;
        private Label lbHeader;
        private RadioBoxButton rbNone;
        private RadioBoxButton rbText;
        private ComboBox cbCharSets;
        private ComboBox cbMimeTypes;
        private Panel pnContent;
        private TextEditor txContent;
        private Label lbContent;
        private ToolTip ttTooltip;

        private PayloadEntity entity;

        #endregion

        #region Construction

        public PayloadEntityEditorPanel()
            : base()
        {
            this.InitializeComponent();

            this.rbNone.PerformClick();
        }

        #endregion

        #region Public Properties

        public String Heading
        {
            get
            {
                return this.lbHeader.Text;
            }
            set
            {
                this.lbHeader.Text = value;
            }
        }

        public PayloadEntity Value
        {
            get
            {
                return this.GetSettingsValue();
            }
            set
            {
                this.SetSettingsValue(value);
            }
        }

        #endregion

        #region Event Handlers

        private void OnPayloadTypeRadioButtonCheckedChanged(Object? sender, EventArgs args)
        {
            if (sender == this.rbNone)
            {
                this.cbCharSets.Enabled = !this.rbNone.Checked;
                this.cbMimeTypes.Enabled = !this.rbNone.Checked;
                this.txContent.Visible = false;
                this.lbContent.Visible = true;
                return;
            }

            if (sender == this.rbText)
            {
                this.cbCharSets.Enabled = this.rbText.Checked;
                this.cbMimeTypes.Enabled = this.rbText.Checked;
                this.txContent.Visible = true;
                this.lbContent.Visible = false;
                return;
            }
        }

        #endregion

        #region Protected Methods

        protected override void Dispose(Boolean disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Private Methods

        private PayloadEntity GetSettingsValue()
        {
            this.entity.Content = this.txContent.Text;
            this.entity.CharSet = (String)this.cbCharSets.SelectedValue;
            this.entity.MimeType = (String)this.cbMimeTypes.SelectedValue;

            if (this.rbNone.Checked) { this.entity.SendType = PayloadType.None; }
            if (this.rbText.Checked) { this.entity.SendType = PayloadType.Text; }

            return this.entity;
        }

        private void SetSettingsValue(PayloadEntity value)
        {
            this.entity = value ?? new PayloadEntity();

            this.txContent.Text = value.Content;

            this.cbCharSets.Items.Clear();
            this.cbCharSets.DisplayMember = nameof(ComboBoxItem<String>.Label);
            this.cbCharSets.ValueMember = nameof(ComboBoxItem<String>.Value);
            this.cbCharSets.DataSource = this.CreateDataSource(this.GetSupportedCharSets());
            this.cbCharSets.SelectedValue = this.entity.CharSet;

            this.cbMimeTypes.Items.Clear();
            this.cbMimeTypes.DisplayMember = nameof(ComboBoxItem<String>.Label);
            this.cbMimeTypes.ValueMember = nameof(ComboBoxItem<String>.Value);
            this.cbMimeTypes.DataSource = this.CreateDataSource(this.GetSupportedMimeTypes());
            this.cbMimeTypes.SelectedValue = this.entity.MimeType;

            switch (this.entity.SendType)
            {
                case PayloadType.None:
                    this.rbNone.Checked = true;
                    break;
                case PayloadType.Text:
                    this.rbText.Checked = true;
                    break;
            }
        }

        private BindingList<ComboBoxItem<String>> CreateDataSource(IEnumerable<String> values)
        {
            return new BindingList<ComboBoxItem<String>>(values.Select(x => new ComboBoxItem<String>(x)).ToList());
        }

        private void InitializeComponent()
        {
            this.components = new Container();

            this.pnHeader = new TableLayoutPanel();
            this.lbHeader = new Label();
            this.rbNone = new RadioBoxButton();
            this.rbText = new RadioBoxButton();
            this.cbCharSets = new ComboBox();
            this.cbMimeTypes = new ComboBox();
            this.pnContent = new Panel();
            this.txContent = new TextEditor();
            this.lbContent = new Label();
            this.ttTooltip = new ToolTip(this.components);

            base.SuspendLayout();
            this.pnHeader.SuspendLayout();
            this.pnContent.SuspendLayout();

            base.Controls.Add(this.pnContent);
            base.Controls.Add(this.pnHeader);

            this.pnHeader.BackColor = Color.WhiteSmoke;
            this.pnHeader.Dock = DockStyle.Top;
            this.pnHeader.Height = 29;
            this.pnHeader.Margin = new Padding(0);
            this.pnHeader.ColumnCount = 5;
            this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.pnHeader.RowCount = 1;
            this.pnHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.pnHeader.TabStop = false;
            this.pnHeader.TabIndex = 0;

            this.pnHeader.Controls.Add(this.lbHeader, 0, 0);
            this.pnHeader.Controls.Add(this.rbNone, 1, 0);
            this.pnHeader.Controls.Add(this.rbText, 2, 0);
            this.pnHeader.Controls.Add(this.cbCharSets, 3, 0);
            this.pnHeader.Controls.Add(this.cbMimeTypes, 4, 0);

            this.lbHeader.BackColor = Color.Transparent;
            this.lbHeader.Dock = DockStyle.Fill;
            this.lbHeader.AddFontStyle(FontStyle.Bold);
            this.lbHeader.ForeColor = Color.DimGray;
            this.lbHeader.Text = "Unknown";
            this.lbHeader.TextAlign = ContentAlignment.MiddleLeft;
            this.lbHeader.TabStop = false;
            this.lbHeader.TabIndex = 0;

            this.rbNone.Dock = DockStyle.Fill;
            this.rbNone.ForeColor = Color.DimGray;
            this.rbNone.Margin = new Padding(0, 3, 3, 3);
            this.rbNone.Text = "None";
            this.rbNone.CheckedChanged += this.OnPayloadTypeRadioButtonCheckedChanged;
            this.rbNone.TabStop = false;
            this.rbNone.TabIndex = 0;
            this.ttTooltip.SetToolTip(this.rbNone, "Ensures that no content is transmitted.");

            this.rbText.Dock = DockStyle.Fill;
            this.rbText.ForeColor = Color.DimGray;
            this.rbText.Margin = new Padding(0, 3, 3, 3);
            this.rbText.Text = "Text";
            this.rbText.CheckedChanged += this.OnPayloadTypeRadioButtonCheckedChanged;
            this.rbText.TabStop = false;
            this.rbText.TabIndex = 0;
            this.ttTooltip.SetToolTip(this.rbText, "Content is transmitted as plain text.\r\nCharacter set and MIME type are used\r\nin this case.");

            this.cbCharSets.Dock = DockStyle.Fill;
            this.cbCharSets.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbCharSets.Margin = new Padding(0, 3, 3, 3);
            this.cbCharSets.Size = new Size(80, 0);
            this.cbCharSets.TabStop = false;
            this.cbCharSets.TabIndex = 0;
            this.ttTooltip.SetToolTip(this.cbCharSets, "Available character sets.");

            this.cbMimeTypes.Dock = DockStyle.Fill;
            this.cbMimeTypes.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbMimeTypes.Margin = new Padding(0, 3, 0, 3);
            this.cbMimeTypes.Size = new Size(130, 0);
            this.cbMimeTypes.TabStop = false;
            this.cbMimeTypes.TabIndex = 0;
            this.ttTooltip.SetToolTip(this.cbMimeTypes, "Available MIME types.");

            this.pnContent.BackColor = Color.Transparent;
            this.pnContent.Dock = DockStyle.Fill;
            this.pnContent.TabStop = false;
            this.pnContent.TabIndex = 0;

            this.pnContent.Controls.Add(this.txContent);

            this.txContent.BackColor = Color.White;
            this.txContent.BorderStyle = BorderStyle.None;
            this.txContent.Dock = DockStyle.Fill;
            this.txContent.Font = new Font("Consolas", this.txContent.Font.Size);
            this.txContent.Location = new Point(0, 0);
            this.txContent.ReadOnly = false;
            this.txContent.TabStop = true;
            this.txContent.TabIndex = 0;
            this.txContent.ContextMenuSelection = TextEditor.ContextMenuItems.Maximal;

            this.pnContent.Controls.Add(this.lbContent);

            this.lbContent.BackColor = Color.Transparent;
            this.lbContent.BorderStyle = BorderStyle.None;
            this.lbContent.Dock = DockStyle.Fill;
            this.lbContent.Location = new Point(0, 0);
            this.lbContent.TabIndex = 0;
            this.lbContent.TabStop = false;
            this.lbContent.Text = "This request does not have any payload.";
            this.lbContent.TextAlign = ContentAlignment.MiddleCenter;
            this.lbContent.Visible = false;

            this.Dock = DockStyle.Fill;
            this.TabStop = true;
            this.TabIndex = 0;

            base.ResumeLayout(false);
            this.pnContent.ResumeLayout(false);
            this.pnContent.PerformLayout();
            this.pnHeader.ResumeLayout(false);
            this.pnHeader.PerformLayout();
        }

        #endregion
    }
}
