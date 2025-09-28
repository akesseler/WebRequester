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
using Plexdata.WebRequester.GUI.Events;
using Plexdata.WebRequester.GUI.Extensions;
using Plexdata.WebRequester.GUI.Models.Execution;
using System.ComponentModel;

namespace Plexdata.WebRequester.GUI.Controls.Composed;

internal class ResponseDetailsPanel : Panel
{
    #region Public Events

    public event EventHandler<PanelHeightEventArgs> PanelHeightIncreased;
    public event EventHandler<PanelHeightEventArgs> PanelHeightDecreased;

    public event EventHandler<FormatPayloadEventArgs> FormatPayload
    {
        add
        {
            this.txPayload.FormatPayload += value;
        }
        remove
        {
            this.txPayload.FormatPayload -= value;
        }
    }

    #endregion

    #region Private Fields

    private ResultEntity content;

    private Int32 lastContentHeight = 0;
    private const String resizeTextHide = "\u25BC";
    private const String resizeTextShow = "\u25B2";

    private TableLayoutPanel pnHeader;
    private Panel pnContent;
    private Label lbHeader;
    private Label lbStatus;
    private Label lbTime;
    private Label lbSize;
    private CheckBoxButton cbResize;
    private RadioBoxButton rbPayload;
    private RadioBoxButton rbHeaders;
    private RadioBoxButton rbDetails;
    private TextEditor txPayload;
    private TextEditor txDetails;
    private DataGridView dgHeaders;

    #endregion

    #region Construction

    public ResponseDetailsPanel()
        : base()
    {
        this.InitializeControl();
    }

    #endregion

    #region Public Properties

    public override String Text
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

    internal ResultEntity Content
    {
        get
        {
            return this.content;
        }
        set
        {
            this.content = value;

            this.rbPayload.Visible = false;
            this.rbPayload.Checked = true;
            this.rbHeaders.Visible = false;
            this.rbHeaders.Checked = true;
            this.rbDetails.Visible = false;
            this.rbDetails.Checked = true;
            this.lbStatus.Visible = false;
            this.lbTime.Visible = false;
            this.lbSize.Visible = false;
            this.txPayload.Visible = true;
            this.dgHeaders.Visible = false;
            this.txDetails.Visible = false;
            this.lbStatus.Text = String.Empty;
            this.lbTime.Text = String.Empty;
            this.lbSize.Text = String.Empty;
            this.txPayload.Text = String.Empty;
            this.dgHeaders.DataSource = null;
            this.txDetails.Text = String.Empty;

            if (this.content == null)
            {
                return;
            }

            if (this.content.IsError || this.content.IsCanceled)
            {
                this.txPayload.Text = this.content.Error.Message;
            }
            else
            {
                this.rbPayload.Visible = true;
                this.rbHeaders.Visible = true;
                this.rbDetails.Visible = true;
                this.lbStatus.Visible = true;
                this.lbTime.Visible = true;
                this.lbSize.Visible = true;
                this.lbStatus.Text = $"Status: {(Int32)this.content.Status.StatusCode}";
                this.lbStatus.BackColor = this.content.Status.IsSuccess ? Color.Transparent : Color.FromArgb(unchecked((Int32)0xFFFFB2B2));
                this.lbTime.Text = $"Time: {this.content.Common.Elapsed.TotalMilliseconds:#,##0} ms";
                this.lbSize.Text = $"Size: {this.content.ResponsePayload.Payload.Length:#,##0} MiB";
                this.txPayload.Text = this.content.ResponsePayload.GetPayloadAsString().ReplaceLineEndings(Environment.NewLine);

                this.dgHeaders.DataSource = new BindingSource(this.content.ResponseHeaders.Select(x => new { x.Label, x.Value }), null);

                if ((this.dgHeaders.DataSource as BindingSource).Count > 1)
                {
                    this.dgHeaders.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.dgHeaders.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                this.txDetails.Text = this.content.ToDisplay();

                this.rbPayload.Checked = true;
            }
        }
    }

    #endregion

    #region Event Handlers

    private void OnContentViewRadioButtonCheckedChanged(Object sender, EventArgs args)
    {
        if (this.rbPayload.Checked)
        {
            this.txPayload.Visible = true;
            this.dgHeaders.Visible = false;
            this.txDetails.Visible = false;
            return;
        }

        if (this.rbHeaders.Checked)
        {
            this.txPayload.Visible = false;
            this.dgHeaders.Visible = true;
            this.txDetails.Visible = false;
            return;
        }

        if (this.rbDetails.Checked)
        {
            this.txPayload.Visible = false;
            this.dgHeaders.Visible = false;
            this.txDetails.Visible = true;
            return;
        }
    }

    private void OnCheckBoxResizeCheckedChanged(Object sender, EventArgs args)
    {
        if (this.cbResize.Checked)
        {
            this.lastContentHeight = this.pnContent.Height;

            this.cbResize.Text = ResponseDetailsPanel.resizeTextShow;

            this.pnContent.Visible = false;

            this.PanelHeightDecreased?.Invoke(this, new PanelHeightEventArgs(this.lastContentHeight));
        }
        else
        {
            this.cbResize.Text = ResponseDetailsPanel.resizeTextHide;

            this.pnContent.Visible = true;

            this.PanelHeightIncreased?.Invoke(this, new PanelHeightEventArgs(this.lastContentHeight));
        }
    }

    #endregion

    #region Private Methods

    private void InitializeControl()
    {
        this.pnHeader = new TableLayoutPanel();
        this.pnContent = new Panel();
        this.txPayload = new TextEditor();
        this.txDetails = new TextEditor();
        this.dgHeaders = new DataGridView();
        this.lbHeader = new Label();
        this.rbPayload = new RadioBoxButton();
        this.rbHeaders = new RadioBoxButton();
        this.rbDetails = new RadioBoxButton();
        this.lbStatus = new Label();
        this.lbTime = new Label();
        this.lbSize = new Label();
        this.cbResize = new CheckBoxButton();

        base.SuspendLayout();
        this.pnHeader.SuspendLayout();
        this.pnContent.SuspendLayout();
        ((ISupportInitialize)this.dgHeaders).BeginInit();

        this.Controls.Add(this.pnContent);
        this.Controls.Add(this.pnHeader);

        this.pnHeader.BackColor = Color.WhiteSmoke;
        this.pnHeader.Dock = DockStyle.Top;
        this.pnHeader.Height = 29;
        this.pnHeader.Margin = new Padding(0);
        this.pnHeader.ColumnCount = 8;
        this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 26F));
        this.pnHeader.RowCount = 1;
        this.pnHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        this.pnHeader.TabStop = false;
        this.pnHeader.TabIndex = 0;

        this.pnHeader.Controls.Add(this.lbHeader, 0, 0);
        this.pnHeader.Controls.Add(this.rbPayload, 1, 0);
        this.pnHeader.Controls.Add(this.rbHeaders, 2, 0);
        this.pnHeader.Controls.Add(this.rbDetails, 3, 0);
        this.pnHeader.Controls.Add(this.lbStatus, 4, 0);
        this.pnHeader.Controls.Add(this.lbTime, 5, 0);
        this.pnHeader.Controls.Add(this.lbSize, 6, 0);
        this.pnHeader.Controls.Add(this.cbResize, 7, 0);

        this.lbHeader.BackColor = Color.Transparent;
        this.lbHeader.Dock = DockStyle.Fill;
        this.lbHeader.ForeColor = Color.DimGray;
        this.lbHeader.Text = "Response";
        this.lbHeader.TextAlign = ContentAlignment.MiddleLeft;
        this.lbHeader.TabStop = false;
        this.lbHeader.TabIndex = 0;
        this.lbHeader.AddFontStyle(FontStyle.Bold);

        this.rbPayload.Dock = DockStyle.Fill;
        this.rbPayload.ForeColor = Color.DimGray;
        this.rbPayload.Margin = new Padding(0, 3, 3, 3);
        this.rbPayload.Text = "Payload";
        this.rbPayload.CheckedChanged += this.OnContentViewRadioButtonCheckedChanged;
        this.rbPayload.TabStop = false;
        this.rbPayload.TabIndex = 0;
        this.rbPayload.Visible = false;

        this.rbHeaders.Dock = DockStyle.Fill;
        this.rbHeaders.ForeColor = Color.DimGray;
        this.rbHeaders.Margin = new Padding(0, 3, 3, 3);
        this.rbHeaders.Text = "Headers";
        this.rbHeaders.CheckedChanged += this.OnContentViewRadioButtonCheckedChanged;
        this.rbHeaders.TabStop = false;
        this.rbHeaders.TabIndex = 0;
        this.rbHeaders.Visible = false;

        this.rbDetails.Dock = DockStyle.Fill;
        this.rbDetails.ForeColor = Color.DimGray;
        this.rbDetails.Margin = new Padding(0, 3, 3, 3);
        this.rbDetails.Text = "Details";
        this.rbDetails.CheckedChanged += this.OnContentViewRadioButtonCheckedChanged;
        this.rbDetails.TabStop = false;
        this.rbDetails.TabIndex = 0;
        this.rbDetails.Visible = false;

        this.lbStatus.AutoSize = true;
        this.lbStatus.BackColor = Color.Transparent;
        this.lbStatus.Dock = DockStyle.Fill;
        this.lbStatus.ForeColor = Color.DimGray;
        this.lbStatus.Margin = new Padding(0, 3, 3, 3);
        this.lbStatus.TextAlign = ContentAlignment.MiddleLeft;
        this.lbStatus.TabStop = false;
        this.lbStatus.TabIndex = 0;
        this.lbStatus.Visible = false;

        this.lbTime.AutoSize = true;
        this.lbTime.BackColor = Color.Transparent;
        this.lbTime.Dock = DockStyle.Fill;
        this.lbTime.ForeColor = Color.DimGray;
        this.lbTime.Margin = new Padding(0, 3, 3, 3);
        this.lbTime.TextAlign = ContentAlignment.MiddleLeft;
        this.lbTime.TabStop = false;
        this.lbTime.TabIndex = 0;
        this.lbTime.Visible = false;

        this.lbSize.AutoSize = true;
        this.lbSize.BackColor = Color.Transparent;
        this.lbSize.Dock = DockStyle.Fill;
        this.lbSize.ForeColor = Color.DimGray;
        this.lbSize.Margin = new Padding(0, 3, 3, 3);
        this.lbSize.TextAlign = ContentAlignment.MiddleLeft;
        this.lbSize.TabStop = false;
        this.lbSize.TabIndex = 0;
        this.lbSize.Visible = false;

        this.cbResize.Dock = DockStyle.Fill;
        this.cbResize.ForeColor = Color.Gray;
        this.cbResize.FlatAppearance.CheckedBackColor = Color.Transparent;
        this.cbResize.Margin = new Padding(0, 3, 3, 3);
        this.cbResize.Text = ResponseDetailsPanel.resizeTextHide;
        this.cbResize.CheckedChanged += this.OnCheckBoxResizeCheckedChanged;
        this.cbResize.TabStop = false;
        this.cbResize.TabIndex = 0;

        this.pnContent.BackColor = Color.Transparent;
        this.pnContent.Dock = DockStyle.Fill;
        this.pnContent.TabStop = false;
        this.pnContent.TabIndex = 0;

        this.pnContent.Controls.Add(this.txPayload);
        this.pnContent.Controls.Add(this.dgHeaders);
        this.pnContent.Controls.Add(this.txDetails);

        this.txPayload.BackColor = Color.White;
        this.txPayload.BorderStyle = BorderStyle.None;
        this.txPayload.Dock = DockStyle.Fill;
        this.txPayload.Font = new Font("Consolas", this.txPayload.Font.Size);
        this.txPayload.ReadOnly = true;
        this.txPayload.TabStop = true;
        this.txPayload.TabIndex = 0;
        this.txPayload.Visible = false;
        this.txPayload.ContextMenuSelection = TextEditor.ContextMenuItems.ReadOnly | TextEditor.ContextMenuItems.Format;

        this.dgHeaders.BackgroundColor = Color.White;
        this.dgHeaders.BorderStyle = BorderStyle.None;
        this.dgHeaders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgHeaders.Dock = DockStyle.Fill;
        this.dgHeaders.RowTemplate.Height = 25;
        this.dgHeaders.TabStop = true;
        this.dgHeaders.TabIndex = 0;
        this.dgHeaders.Margin = new Padding(0);
        this.dgHeaders.Visible = false;

        this.txDetails.BackColor = Color.White;
        this.txDetails.BorderStyle = BorderStyle.None;
        this.txDetails.Dock = DockStyle.Fill;
        this.txDetails.Font = new Font("Consolas", this.txDetails.Font.Size);
        this.txDetails.ReadOnly = true;
        this.txDetails.TabStop = true;
        this.txDetails.TabIndex = 0;
        this.txDetails.Visible = false;
        this.txDetails.ContextMenuSelection = TextEditor.ContextMenuItems.ReadOnly;

        this.ResumeLayout(false);
        this.pnContent.ResumeLayout(false);
        this.pnContent.PerformLayout();
        this.pnHeader.ResumeLayout(false);
        this.pnHeader.PerformLayout();
    }

    #endregion
}
