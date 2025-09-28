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

namespace Plexdata.WebRequester.GUI.Controls.Composed;

internal class PayloadEntityEditorPanel : Panel, IPayloadEntityEditor<PayloadEntity>
{
    #region Private Fields

    private Container components = null;
    private TableLayoutPanel pnHeader;
    private Label lbHeader;
    private RadioBoxButton rbNone;
    private RadioBoxButton rbText;
    private RadioBoxButton rbFile;
    private RadioBoxButton rbForm;
    private ComboBox cbCharSets;
    private ComboBox cbMimeTypes;
    private Panel pnContent;
    private TextEditor txContent;
    private TextEditor fiContent;
    private DataGridView dgContent;
    private Label lbContent;
    private ToolTip ttTooltip;

    private PayloadEntity entity;
    private BindingList<DataGridItem> formData;

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
            this.fiContent.Visible = false;
            this.dgContent.Visible = false;
            this.lbContent.Visible = true;
            return;
        }

        if (sender == this.rbText)
        {
            this.cbCharSets.Enabled = this.rbText.Checked;
            this.cbMimeTypes.Enabled = this.rbText.Checked;
            this.txContent.Visible = true;
            this.fiContent.Visible = false;
            this.dgContent.Visible = false;
            this.lbContent.Visible = false;
            return;
        }

        if (sender == this.rbFile)
        {
            this.cbCharSets.Enabled = this.rbFile.Checked;
            this.cbMimeTypes.Enabled = this.rbFile.Checked;
            this.txContent.Visible = false;
            this.fiContent.Visible = true;
            this.dgContent.Visible = false;
            this.lbContent.Visible = false;
            return;
        }

        if (sender == this.rbForm)
        {
            this.cbCharSets.Enabled = !this.rbForm.Checked;
            this.cbMimeTypes.Enabled = !this.rbForm.Checked;
            this.txContent.Visible = false;
            this.fiContent.Visible = false;
            this.dgContent.Visible = true;
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
        // Secret knowledge! The property contains the payload for text content at
        // index zero or a fully qualified file path at index one or the payload
        // for form-data content at index two.

        this.entity.Content = this.BuildContent(this.txContent.Text, this.fiContent.Text, DataGridItem.Merge(this.formData));
        this.entity.CharSet = (String)this.cbCharSets.SelectedValue;
        this.entity.MimeType = (String)this.cbMimeTypes.SelectedValue;

        if (this.rbNone.Checked) { this.entity.SendType = PayloadType.None; }
        if (this.rbText.Checked) { this.entity.SendType = PayloadType.Text; }
        if (this.rbFile.Checked) { this.entity.SendType = PayloadType.File; }
        if (this.rbForm.Checked) { this.entity.SendType = PayloadType.Form; }

        return this.entity;
    }

    private void SetSettingsValue(PayloadEntity value)
    {
        this.entity = value ?? new PayloadEntity();

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

        // Secret knowledge! The property contains the payload for text content at
        // index zero or a fully qualified file path at index one or the payload
        // for form-data content at index two.

        String[] values = this.SplitContent(3, this.entity.Content);

        this.txContent.Text = values[0];
        this.fiContent.Text = values[1];
        this.formData = new BindingList<DataGridItem>([.. DataGridItem.Split(values[2])]);

        this.dgContent.DataSource = new BindingSource(this.formData, null);

        this.dgContent.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
        this.dgContent.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        this.dgContent.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        this.dgContent.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        switch (this.entity.SendType)
        {
            case PayloadType.None:
                this.rbNone.Checked = true;
                break;
            case PayloadType.Text:
                this.rbText.Checked = true;
                break;
            case PayloadType.File:
                this.rbFile.Checked = true;
                break;
            case PayloadType.Form:
                this.rbForm.Checked = true;
                break;
        }
    }

    private BindingList<ComboBoxItem<String>> CreateDataSource(IEnumerable<String> values)
    {
        return new BindingList<ComboBoxItem<String>>([.. values.Select(x => new ComboBoxItem<String>(x))]);
    }

    private void InitializeComponent()
    {
        this.components = new Container();

        this.pnHeader = new TableLayoutPanel();
        this.lbHeader = new Label();
        this.rbNone = new RadioBoxButton();
        this.rbText = new RadioBoxButton();
        this.rbFile = new RadioBoxButton();
        this.rbForm = new RadioBoxButton();
        this.cbCharSets = new ComboBox();
        this.cbMimeTypes = new ComboBox();
        this.pnContent = new Panel();
        this.txContent = new TextEditor();
        this.fiContent = new TextEditor();
        this.dgContent = new DataGridView();
        this.lbContent = new Label();
        this.ttTooltip = new ToolTip(this.components);

        base.SuspendLayout();
        this.pnHeader.SuspendLayout();
        this.pnContent.SuspendLayout();
        ((ISupportInitialize)this.dgContent).BeginInit();
        base.SuspendLayout();

        // Change the tooltip's display time to 60 seconds
        this.ttTooltip.AutoPopDelay = 60 * 1000;

        base.Controls.Add(this.pnContent);
        base.Controls.Add(this.pnHeader);

        this.pnHeader.BackColor = Color.WhiteSmoke;
        this.pnHeader.Dock = DockStyle.Top;
        this.pnHeader.Height = 29;
        this.pnHeader.Margin = new Padding(0);
        this.pnHeader.ColumnCount = 7;
        this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
        this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
        this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
        this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
        this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 85F));
        this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 165F));
        this.pnHeader.RowCount = 1;
        this.pnHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        this.pnHeader.TabStop = false;
        this.pnHeader.TabIndex = 0;

        this.pnHeader.Controls.Add(this.lbHeader, 0, 0);
        this.pnHeader.Controls.Add(this.rbNone, 1, 0);
        this.pnHeader.Controls.Add(this.rbText, 2, 0);
        this.pnHeader.Controls.Add(this.rbFile, 3, 0);
        this.pnHeader.Controls.Add(this.rbForm, 4, 0);
        this.pnHeader.Controls.Add(this.cbCharSets, 5, 0);
        this.pnHeader.Controls.Add(this.cbMimeTypes, 6, 0);

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
        this.ttTooltip.SetToolTip(this.rbText, "The entire payload is sent as plain text and the\r\nchosen MIME type is applied as specified.");

        this.rbFile.Dock = DockStyle.Fill;
        this.rbFile.ForeColor = Color.DimGray;
        this.rbFile.Margin = new Padding(0, 3, 3, 3);
        this.rbFile.Text = "File";
        this.rbFile.CheckedChanged += this.OnPayloadTypeRadioButtonCheckedChanged;
        this.rbFile.TabStop = false;
        this.rbFile.TabIndex = 0;
        this.ttTooltip.SetToolTip(this.rbFile, "The payload must only contain a fully qualified\r\nfile name and the selected MIME type determines\r\nhow the file content is sent.");

        this.rbForm.Dock = DockStyle.Fill;
        this.rbForm.ForeColor = Color.DimGray;
        this.rbForm.Margin = new Padding(0, 3, 3, 3);
        this.rbForm.Text = "Form";
        this.rbForm.CheckedChanged += this.OnPayloadTypeRadioButtonCheckedChanged;
        this.rbForm.TabStop = false;
        this.rbForm.TabIndex = 0;
        this.ttTooltip.SetToolTip(this.rbForm, "The payload is specified as a key-value pair. In case of one value\r\ncontains a fully qualified path of an existing file, the content of\r\nthat file is sent as a binary stream. All other key-value pairs are\r\nsent as named string values.\r\n\r\nThe MIME type \"multipart/form-data\" is set automatically!");

        this.cbCharSets.Dock = DockStyle.Fill;
        this.cbCharSets.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cbCharSets.Margin = new Padding(0, 3, 3, 3);
        this.cbCharSets.Size = new Size(80, 0);
        this.cbCharSets.TabStop = false;
        this.cbCharSets.TabIndex = 0;
        this.ttTooltip.SetToolTip(this.cbCharSets, "Available character sets.");

        this.cbMimeTypes.Dock = DockStyle.Fill;
        this.cbMimeTypes.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cbMimeTypes.Margin = new Padding(0, 3, 3, 3);
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
        this.txContent.ContextMenuSelection = TextEditor.ContextMenuItems.Editable;

        this.pnContent.Controls.Add(this.fiContent);

        this.fiContent.BackColor = Color.White;
        this.fiContent.BorderStyle = BorderStyle.None;
        this.fiContent.Dock = DockStyle.Fill;
        this.fiContent.Font = new Font("Consolas", this.fiContent.Font.Size);
        this.fiContent.Location = new Point(0, 0);
        this.fiContent.ReadOnly = false;
        this.fiContent.TabStop = true;
        this.fiContent.TabIndex = 0;
        this.fiContent.ContextMenuSelection = TextEditor.ContextMenuItems.Editable;

        this.pnContent.Controls.Add(this.dgContent);

        this.dgContent.BackgroundColor = Color.White;
        this.dgContent.BorderStyle = BorderStyle.None;
        this.dgContent.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgContent.Dock = DockStyle.Fill;
        this.dgContent.RowTemplate.Height = 25;
        this.dgContent.TabStop = true;
        this.dgContent.TabIndex = 0;
        this.dgContent.Margin = new Padding(0);
        this.dgContent.Visible = true;

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
        ((ISupportInitialize)this.dgContent).EndInit();
        base.ResumeLayout(false);
    }

    #endregion

    #region Private Helper Classes

    private class DataGridItem
    {
        public Boolean Apply { get; set; } = false;
        public String Label { get; set; } = String.Empty;
        public String Value { get; set; } = String.Empty;
        public String Notes { get; set; } = String.Empty;

        public override String ToString()
        {
            return String.Join(
                SeparatorType.UnitSeparator,
                this.Apply ? Boolean.TrueString : Boolean.FalseString,
                this.Label ?? String.Empty,
                this.Value ?? String.Empty,
                this.Notes ?? String.Empty
            );
        }

        public static IEnumerable<DataGridItem> Split(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return [];
            }

            return value.Split(Environment.NewLine).Select(x => DataGridItem.Parse(x));
        }

        public static String Merge(IEnumerable<DataGridItem> values)
        {
            if (!values?.Any() ?? true)
            {
                return String.Empty;
            }

            return String.Join(Environment.NewLine, values.Select(x => x.ToString()));
        }

        private static DataGridItem Parse(String value)
        {
            DataGridItem result = new DataGridItem();

            if (String.IsNullOrWhiteSpace(value))
            {
                return result;
            }

            String[] values = value.Split(SeparatorType.UnitSeparator);

            if (values.Length > 0 && Boolean.TryParse(values[0], out Boolean apply))
            {
                result.Apply = apply;
            }

            if (values.Length > 1)
            {
                result.Label = values[1];
            }

            if (values.Length > 2)
            {
                result.Value = values[2];
            }

            if (values.Length > 3)
            {
                result.Notes = values[3];
            }

            return result;
        }
    }

    #endregion
}
