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

using Plexdata.WebRequester.GUI.Controls.General;
using Plexdata.WebRequester.GUI.Extensions;
using Plexdata.WebRequester.GUI.Interfaces;
using Plexdata.WebRequester.GUI.Models.Projects;
using System.ComponentModel;
using System.Text;

namespace Plexdata.WebRequester.GUI.Controls.Composed
{
    internal class ActionEntitiesEditorPanel : Panel, IActionEntitiesEditor<IEnumerable<ActionEntity>>
    {
        #region Private Fields

        private const String reviseTextBulk = "Bulk";
        private const String reviseTextChart = "Chart";

        private IContainer components = null;
        private TableLayoutPanel pnHeader;
        private Label lbHeader;
        private CheckBoxButton cbRevise;
        private Panel pnContent;
        private DataGridView dgContent;
        private TextEditor txContent;
        private ToolTip ttTooltip;

        private BindingList<DataGridItem> actionEntities;

        #endregion

        #region Construction

        public ActionEntitiesEditorPanel()
            : base()
        {
            this.InitializeComponent();
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

        public Boolean AllowBulkEdit
        {
            get
            {
                return this.cbRevise.Visible;
            }
            set
            {
                this.cbRevise.Visible = value;
            }
        }

        public IEnumerable<ActionEntity> Value
        {
            get
            {
                return this.GetSettingsValues();
            }
            set
            {
                this.SetSettingsValues(value);
            }
        }

        #endregion

        #region Event Handlers

        private void OnCheckBoxResizeCheckedChanged(Object sender, EventArgs args)
        {
            if (this.cbRevise.Checked)
            {
                this.ParseFromChart();

                this.cbRevise.Text = ActionEntitiesEditorPanel.reviseTextChart;
                this.ttTooltip.SetToolTip(this.cbRevise, "Switch to chart edit mode.");
                this.dgContent.Visible = false;
                this.txContent.Visible = true;
            }
            else
            {
                this.ParseFromPlain();

                this.cbRevise.Text = ActionEntitiesEditorPanel.reviseTextBulk;
                this.ttTooltip.SetToolTip(this.cbRevise, "Switch to bulk edit mode.");
                this.txContent.Visible = false;
                this.dgContent.Visible = true;
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

        private IEnumerable<ActionEntity> GetSettingsValues()
        {
            if (this.cbRevise.Checked)
            {
                // Intentionally, restore default view and parse results.
                this.cbRevise.Checked = false;
            }

            return this.actionEntities.Select(x => x.ToActionEntity()).ToList();
        }

        private void SetSettingsValues(IEnumerable<ActionEntity> value)
        {
            List<DataGridItem> items = new List<DataGridItem>();

            if (value != null)
            {
                items = value.Select(x => new DataGridItem(x)).ToList();
            }

            this.actionEntities = new BindingList<DataGridItem>(items);

            this.dgContent.DataSource = new BindingSource(this.actionEntities, null);

            this.dgContent.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dgContent.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgContent.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgContent.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void ParseFromChart()
        {
            this.txContent.Lines = this.actionEntities.Select(x => x.ToString()).ToArray();
        }

        private void ParseFromPlain()
        {
            this.actionEntities.Clear();

            foreach (String line in this.txContent.Lines)
            {
                if (!String.IsNullOrWhiteSpace(line))
                {
                    this.actionEntities.Add(DataGridItem.Parse(line));
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();

            this.pnHeader = new TableLayoutPanel();
            this.lbHeader = new Label();
            this.cbRevise = new CheckBoxButton();
            this.pnContent = new Panel();
            this.dgContent = new DataGridView();
            this.txContent = new TextEditor();
            this.ttTooltip = new ToolTip(this.components);

            this.SuspendLayout();
            this.pnHeader.SuspendLayout();
            this.pnContent.SuspendLayout();
            ((ISupportInitialize)this.dgContent).BeginInit();
            this.SuspendLayout();

            this.Controls.Add(this.pnContent);
            this.Controls.Add(this.pnHeader);

            this.pnHeader.BackColor = Color.WhiteSmoke;
            this.pnHeader.Dock = DockStyle.Top;
            this.pnHeader.Height = 29;
            this.pnHeader.Margin = new Padding(0);
            this.pnHeader.ColumnCount = 2;
            this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            this.pnHeader.RowCount = 1;
            this.pnHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.pnHeader.TabStop = false;
            this.pnHeader.TabIndex = 0;

            this.pnHeader.Controls.Add(this.lbHeader, 0, 0);
            this.pnHeader.Controls.Add(this.cbRevise, 1, 0);

            this.lbHeader.BackColor = Color.Transparent;
            this.lbHeader.Dock = DockStyle.Fill;
            this.lbHeader.AddFontStyle(FontStyle.Bold);
            this.lbHeader.ForeColor = Color.DimGray;
            this.lbHeader.Text = "Unknown";
            this.lbHeader.TextAlign = ContentAlignment.MiddleLeft;
            this.lbHeader.TabStop = false;
            this.lbHeader.TabIndex = 0;

            this.cbRevise.Dock = DockStyle.Fill;
            this.cbRevise.ForeColor = Color.Gray;
            this.cbRevise.FlatAppearance.CheckedBackColor = Color.Transparent;
            this.cbRevise.Margin = new Padding(0, 3, 3, 3);
            this.cbRevise.Text = ActionEntitiesEditorPanel.reviseTextBulk;
            this.cbRevise.CheckedChanged += this.OnCheckBoxResizeCheckedChanged;
            this.cbRevise.TabStop = false;
            this.cbRevise.TabIndex = 2;
            this.ttTooltip.SetToolTip(this.cbRevise, "Switch to bulk edit mode.");

            this.pnContent.BackColor = Color.Transparent;
            this.pnContent.Controls.Add(this.dgContent);
            this.pnContent.Controls.Add(this.txContent);
            this.pnContent.Dock = DockStyle.Fill;
            this.pnContent.TabStop = false;
            this.pnContent.TabIndex = 0;

            this.dgContent.BackgroundColor = Color.White;
            this.dgContent.BorderStyle = BorderStyle.None;
            this.dgContent.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgContent.Dock = DockStyle.Fill;
            this.dgContent.RowTemplate.Height = 25;
            this.dgContent.TabStop = true;
            this.dgContent.TabIndex = 0;
            this.dgContent.Margin = new Padding(0);
            this.dgContent.Visible = true;

            this.txContent.BackColor = Color.White;
            this.txContent.BorderStyle = BorderStyle.None;
            this.txContent.Dock = DockStyle.Fill;
            this.txContent.Font = new Font("Consolas", this.txContent.Font.Size);
            this.txContent.Location = new Point(0, 0);
            this.txContent.ReadOnly = false;
            this.txContent.Size = new Size(836, 178);
            this.txContent.TabStop = true;
            this.txContent.TabIndex = 0;
            this.txContent.Visible = false;
            this.txContent.ContextMenuSelection = TextEditor.ContextMenuItems.Maximal;

            this.Dock = DockStyle.Fill;
            this.TabIndex = 0;

            this.ResumeLayout(false);
            this.pnHeader.ResumeLayout(false);
            this.pnContent.ResumeLayout(false);
            ((ISupportInitialize)this.dgContent).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        #region Private Helper Classes

        private class DataGridItem
        {
            public DataGridItem()
                : this(null)
            {
            }

            public DataGridItem(ActionEntity value)
                : base()
            {
                this.Apply = value?.Apply ?? false;
                this.Label = value?.Label ?? String.Empty;
                this.Value = value?.Value ?? String.Empty;
                this.Notes = value?.Notes ?? String.Empty;
            }

            public Boolean Apply { get; set; }
            public String Label { get; set; }
            public String Value { get; set; }
            public String Notes { get; set; }

            public static DataGridItem Parse(String value)
            {
                value ??= String.Empty;

                DataGridItem result = new DataGridItem();

                result.Apply = !value.StartsWith("//");

                value = value.TrimStart('/');

                Int32 start = 0;
                Int32 index = value.IndexOf(':', start);

                if (index == -1)
                {
                    result.Label = value;
                    return result;
                }

                result.Label = value.Substring(start, index - start);

                start = index + 1;

                index = value.IndexOf(':', start);

                if (index == -1)
                {
                    result.Value = value.Substring(start);
                    return result;
                }

                result.Value = value.Substring(start, index - start);

                result.Notes = value.Substring(index + 1);

                return result;
            }

            public ActionEntity ToActionEntity()
            {
                return new ActionEntity()
                {
                    Apply = this.Apply,
                    Label = this.Label,
                    Value = this.Value,
                    Notes = this.Notes
                };
            }

            public override String ToString()
            {
                StringBuilder builder = new StringBuilder();

                if (!this.Apply)
                {
                    builder.Append("//");
                }

                if (!String.IsNullOrWhiteSpace(this.Label))
                {
                    builder.Append(this.Label);
                }

                builder.Append(":");

                if (!String.IsNullOrWhiteSpace(this.Value))
                {
                    builder.Append(this.Value);
                }

                if (!String.IsNullOrWhiteSpace(this.Notes))
                {
                    builder.Append(":");
                    builder.Append(this.Notes);
                }

                return builder.ToString();
            }
        }

        #endregion
    }
}
