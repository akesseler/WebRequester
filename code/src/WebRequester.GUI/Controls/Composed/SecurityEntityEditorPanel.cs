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
using Plexdata.WebRequester.GUI.Definitions;
using Plexdata.WebRequester.GUI.Extensions;
using Plexdata.WebRequester.GUI.Interfaces;
using Plexdata.WebRequester.GUI.Models.Helpers;
using Plexdata.WebRequester.GUI.Models.Projects;
using Plexdata.WebRequester.GUI.Properties;
using System.ComponentModel;
using System.Text;

namespace Plexdata.WebRequester.GUI.Controls.Composed
{
    internal class SecurityEntityEditorPanel : Panel, ISecurityEntityEditor<SecurityEntity>
    {
        #region Public Events 

        public event EventHandler<EventArgs> ShowQueriesPanel;
        public event EventHandler<EventArgs> ShowHeadersPanel;

        #endregion

        #region Private Fields

        private const String queriesLinkData = "Query Parameters";
        private const String headersLinkData = "Header Values";

        private IContainer components = null;
        private TableLayoutPanel pnHeader;
        private Label lbHeader;
        private ComboBox cbAuthTypes;
        private TableLayoutPanel pnContent;
        private LinkLabel lbRemarks;
        private TabControlEx tcAuthTypes;
        private ToolTip ttTooltip;
        private ApiKeyPanel pnApiKey;
        private BearerTokenPanel pnBearerToken;
        private BasicAuthorizationPanel pnBasicAuthorization;
        private Dictionary<AuthorizationType, TabPage> tpPages = null;
        private SecurityEntity entity;

        private Boolean hideRemarks = false;
        private Boolean canInherit = true;

        #endregion

        #region Construction

        public SecurityEntityEditorPanel()
            : base()
        {
            this.InitializeComponent();
        }

        #endregion

        #region ISecurityEntityEditor<SecurityEntity>

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

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Boolean HideRemarks
        {
            get
            {
                return this.hideRemarks;
            }
            set
            {
                this.hideRemarks = value;

                if (this.lbRemarks.Visible && this.hideRemarks)
                {
                    this.lbRemarks.Visible = false;
                }
            }
        }

        /// <summary>
        /// Shows or hides the "Inherit" combo-box entry. 
        /// This property works only at initialization time.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Boolean CanInherit
        {
            get
            {
                return this.canInherit;
            }
            set
            {
                // Do never renew a data source once it has been assigned!
                this.canInherit = value;
            }
        }

        #endregion

        #region ISettingsDetailsManager<SecurityEntity>

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SecurityEntity Value
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

        private void OnAuthTypesSelectedValueChanged(Object? sender, EventArgs args)
        {
            if (this.cbAuthTypes.SelectedValue == null)
            {
                this.lbRemarks.Visible = false;
                this.tcAuthTypes.SelectedTab = null;
                return;
            }

            AuthorizationType type = (AuthorizationType)this.cbAuthTypes.SelectedValue;

            this.lbRemarks.Visible = this.CanShowRemarks(type);

            this.tcAuthTypes.SelectedTab = this.tpPages[type];
        }

        private void OnLabelRemarksLinkClicked(Object sender, LinkLabelLinkClickedEventArgs args)
        {
            if (String.Equals(args.Link.LinkData, SecurityEntityEditorPanel.queriesLinkData))
            {
                this.ShowQueriesPanel?.Invoke(this, EventArgs.Empty);
            }

            if (String.Equals(args.Link.LinkData, SecurityEntityEditorPanel.headersLinkData))
            {
                this.ShowHeadersPanel?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Private Methods

        private SecurityEntity GetSettingsValue()
        {
            this.entity.AuthorizationType = (AuthorizationType)this.cbAuthTypes.SelectedValue;

            this.entity.ApiKey.Key = this.pnApiKey.Key;
            this.entity.ApiKey.Value = this.pnApiKey.Value;
            this.entity.ApiKey.Usage = this.pnApiKey.Usage;

            this.entity.BearerToken.Token = this.pnBearerToken.Token;

            this.entity.BasicAuthorization.Username = this.pnBasicAuthorization.Username;
            this.entity.BasicAuthorization.Password = this.pnBasicAuthorization.Password;
            this.entity.BasicAuthorization.Encoding = this.pnBasicAuthorization.Encoding;

            return this.entity;
        }

        private void SetSettingsValue(SecurityEntity value)
        {
            this.entity = value ?? new SecurityEntity(this.CanInherit ? AuthorizationType.InheritFromParent : AuthorizationType.NoAuthorization);

            this.cbAuthTypes.Items.Clear();
            this.cbAuthTypes.DisplayMember = nameof(ComboBoxItem<AuthorizationType>.Label);
            this.cbAuthTypes.ValueMember = nameof(ComboBoxItem<AuthorizationType>.Value);
            this.cbAuthTypes.DataSource = this.CreateDataSource(this.entity.GetAuthorizationTypes());
            this.cbAuthTypes.SelectedValue = this.entity.AuthorizationType;

            this.pnApiKey.Key = this.entity.ApiKey.Key;
            this.pnApiKey.Value = this.entity.ApiKey.Value;
            this.pnApiKey.Usage = this.entity.ApiKey.Usage;

            this.pnBearerToken.Token = this.entity.BearerToken.Token;

            this.pnBasicAuthorization.Username = this.entity.BasicAuthorization.Username;
            this.pnBasicAuthorization.Password = this.entity.BasicAuthorization.Password;
            this.pnBasicAuthorization.Encoding = this.entity.BasicAuthorization.Encoding;
        }

        private BindingList<ComboBoxItem<AuthorizationType>> CreateDataSource(IEnumerable<(String Label, AuthorizationType Value)> values)
        {
            if (!this.CanInherit)
            {
                values = values.Where(x => x.Value != AuthorizationType.InheritFromParent);
            }

            return new BindingList<ComboBoxItem<AuthorizationType>>(values.Select(x => new ComboBoxItem<AuthorizationType>(x)).ToList());
        }

        private void InitializeComponent()
        {
            this.components = new Container();

            this.tpPages = new Dictionary<AuthorizationType, TabPage>();

            this.pnHeader = new TableLayoutPanel();
            this.lbHeader = new Label();
            this.cbAuthTypes = new ComboBox();
            this.pnContent = new TableLayoutPanel();
            this.lbRemarks = new LinkLabel();
            this.tcAuthTypes = new TabControlEx();
            this.ttTooltip = new ToolTip(this.components);
            this.pnApiKey = new ApiKeyPanel();
            this.pnBearerToken = new BearerTokenPanel();
            this.pnBasicAuthorization = new BasicAuthorizationPanel();

            base.SuspendLayout();
            this.pnHeader.SuspendLayout();
            this.pnContent.SuspendLayout();
            this.tcAuthTypes.SuspendLayout();

            base.Controls.Add(this.pnContent);
            base.Controls.Add(this.pnHeader);

            this.pnHeader.BackColor = Color.WhiteSmoke;
            this.pnHeader.Dock = DockStyle.Top;
            this.pnHeader.Height = 29;
            this.pnHeader.Margin = new Padding(0);
            this.pnHeader.ColumnCount = 2;
            this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.pnHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.pnHeader.RowCount = 1;
            this.pnHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.pnHeader.TabStop = false;
            this.pnHeader.TabIndex = 0;

            this.pnHeader.Controls.Add(this.lbHeader, 0, 0);
            this.pnHeader.Controls.Add(this.cbAuthTypes, 1, 0);

            this.lbHeader.BackColor = Color.Transparent;
            this.lbHeader.Dock = DockStyle.Fill;
            this.lbHeader.AddFontStyle(FontStyle.Bold);
            this.lbHeader.ForeColor = Color.DimGray;
            this.lbHeader.Text = "Unknown";
            this.lbHeader.TextAlign = ContentAlignment.MiddleLeft;
            this.lbHeader.TabStop = false;
            this.lbHeader.TabIndex = 0;

            this.pnContent.BackColor = Color.Transparent;
            this.pnContent.ColumnCount = 1;
            this.pnContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.pnContent.Dock = DockStyle.Fill;
            this.pnContent.Location = new Point(12, 85);
            this.pnContent.RowCount = 2;
            this.pnContent.RowStyles.Add(new RowStyle());
            this.pnContent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.pnContent.Size = new Size(836, 452);
            this.pnContent.TabStop = false;
            this.pnContent.TabIndex = 0;

            this.cbAuthTypes.Dock = DockStyle.Fill;
            this.cbAuthTypes.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbAuthTypes.Margin = new Padding(0, 3, 0, 3);
            this.cbAuthTypes.Size = new Size(130, 0);
            this.cbAuthTypes.TabStop = false;
            this.cbAuthTypes.TabIndex = 0;
            this.cbAuthTypes.SelectedValueChanged += this.OnAuthTypesSelectedValueChanged;
            this.ttTooltip.SetToolTip(this.cbAuthTypes, "Choose an authorization type.");

            this.lbRemarks.AutoSize = true;
            this.lbRemarks.BackColor = Color.FromArgb(unchecked((Int32)0xFFFFEBE7));
            this.lbRemarks.BorderStyle = BorderStyle.None;
            this.lbRemarks.Dock = DockStyle.Top;
            this.lbRemarks.LinkClicked += this.OnLabelRemarksLinkClicked;
            this.lbRemarks.Margin = new Padding(0);
            this.lbRemarks.Padding = new Padding(10, 3, 10, 3);
            this.lbRemarks.Size = new Size(0, 50);
            this.lbRemarks.TabStop = false;
            this.lbRemarks.TextAlign = ContentAlignment.MiddleLeft;
            this.lbRemarks.Text = $"Please note that an API key configured here will not be used if the selected " +
                                  $"authorization type is not 'API key'. In such a case, you better add the API key either " +
                                  $"to {SecurityEntityEditorPanel.queriesLinkData} or to {SecurityEntityEditorPanel.headersLinkData}.";

            this.lbRemarks.Links.Add(
                this.lbRemarks.Text.IndexOf(SecurityEntityEditorPanel.queriesLinkData),
                SecurityEntityEditorPanel.queriesLinkData.Length,
                SecurityEntityEditorPanel.queriesLinkData);
            this.lbRemarks.Links.Add(
                this.lbRemarks.Text.IndexOf(SecurityEntityEditorPanel.headersLinkData),
                SecurityEntityEditorPanel.headersLinkData.Length,
                SecurityEntityEditorPanel.headersLinkData);

            this.pnContent.Controls.Add(this.lbRemarks, 0, 0);
            this.pnContent.Controls.Add(this.tcAuthTypes, 0, 1);

            this.tcAuthTypes.Dock = DockStyle.Fill;
            this.tcAuthTypes.Margin = new Padding(0);
            this.tcAuthTypes.SelectedIndex = 0;
            this.tcAuthTypes.TabIndex = 0;
            this.tcAuthTypes.HideTabs = true;

            TabPage tabPage;

            tabPage = this.CreateLabeledTabPage("The request inherits the authorization from its parent section.", AuthorizationType.InheritFromParent);

            this.tpPages.Add(AuthorizationType.InheritFromParent, tabPage);
            this.tcAuthTypes.Controls.Add(tabPage);

            tabPage = this.CreateLabeledTabPage("The request does not use any authorization.", AuthorizationType.NoAuthorization);

            this.tpPages.Add(AuthorizationType.NoAuthorization, tabPage);
            this.tcAuthTypes.Controls.Add(tabPage);

            tabPage = new TabPage();
            tabPage.BackColor = Color.White;
            tabPage.Tag = AuthorizationType.ApiKey;
            tabPage.Controls.Add(this.pnApiKey);

            this.tpPages.Add(AuthorizationType.ApiKey, tabPage);
            this.tcAuthTypes.Controls.Add(tabPage);

            tabPage = new TabPage();
            tabPage.BackColor = Color.White;
            tabPage.Tag = AuthorizationType.BearerToken;
            tabPage.Controls.Add(this.pnBearerToken);

            this.tpPages.Add(AuthorizationType.BearerToken, tabPage);
            this.tcAuthTypes.Controls.Add(tabPage);

            tabPage = new TabPage();
            tabPage.BackColor = Color.White;
            tabPage.Tag = AuthorizationType.BasicAuthorization;
            tabPage.Controls.Add(this.pnBasicAuthorization);

            this.tpPages.Add(AuthorizationType.BasicAuthorization, tabPage);
            this.tcAuthTypes.Controls.Add(tabPage);

            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;
            this.TabStop = true;
            this.TabIndex = 0;

            base.ResumeLayout(false);
            this.tcAuthTypes.ResumeLayout(false);
            this.pnContent.ResumeLayout(false);
            this.pnContent.PerformLayout();
            this.pnHeader.ResumeLayout(false);
            this.pnHeader.PerformLayout();
            this.pnApiKey.ResumeLayout(false);
            this.pnApiKey.PerformLayout();
            this.pnBearerToken.ResumeLayout(false);
            this.pnBearerToken.PerformLayout();
        }

        private Boolean CanShowRemarks(AuthorizationType type)
        {
            return !this.HideRemarks && type != AuthorizationType.NoAuthorization && type != AuthorizationType.InheritFromParent;
        }

        private TabPage CreateLabeledTabPage(String label, AuthorizationType type)
        {
            TabPage tpPage = new TabPage();
            Label lbLabel = new Label();

            tpPage.Tag = type;
            tpPage.BackColor = Color.White;
            tpPage.Margin = new Padding(0);
            tpPage.Padding = new Padding(0);

            lbLabel.Dock = DockStyle.Fill;
            lbLabel.TextAlign = ContentAlignment.MiddleCenter;
            lbLabel.BackColor = Color.Transparent;
            lbLabel.Text = label;
            tpPage.Controls.Add(lbLabel);

            return tpPage;
        }

        #endregion

        #region Private Classes

        private class ApiKeyPanel : Panel
        {
            #region Private Fields

            private Label lbKey;
            private Label lbValue;
            private Label lbUsage;
            private TextBox txKey;
            private TextBox txValue;
            private ComboBox cbUsage;

            #endregion

            #region Construction

            public ApiKeyPanel()
                : base()
            {
                this.InitializeComponent();
            }

            #endregion

            #region Public Properties

            public String Key
            {
                get
                {
                    return this.txKey.Text;
                }
                set
                {
                    this.txKey.Text = value;
                }
            }

            public String Value
            {
                get
                {
                    return this.txValue.Text;
                }
                set
                {
                    this.txValue.Text = value;
                }
            }

            public UsageType Usage
            {
                get
                {
                    return (UsageType)this.cbUsage.SelectedItem;
                }
                set
                {
                    this.cbUsage.SelectedItem = value;
                }
            }

            #endregion

            #region Private Methods

            private void InitializeComponent()
            {
                base.SuspendLayout();

                this.lbKey = new Label();
                this.lbKey.AutoSize = true;
                this.lbKey.Location = new Point(10, 16);
                this.lbKey.Margin = new Padding(3);
                this.lbKey.Size = new Size(26, 15);
                this.lbKey.TabIndex = 0;
                this.lbKey.Text = "&Key";

                this.txKey = new TextBox();
                this.txKey.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                this.txKey.Location = new Point(80, 13);
                this.txKey.Size = new Size(0, 23);
                this.txKey.TabIndex = 1;

                this.lbValue = new Label();
                this.lbValue.AutoSize = true;
                this.lbValue.Location = new Point(10, 45);
                this.lbValue.Margin = new Padding(3);
                this.lbValue.Size = new Size(35, 15);
                this.lbValue.TabIndex = 2;
                this.lbValue.Text = "&Value";

                this.txValue = new TextBox();
                this.txValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                this.txValue.Location = new Point(80, 42);
                this.txValue.Size = new Size(0, 23);
                this.txValue.TabIndex = 3;

                this.lbUsage = new Label();
                this.lbUsage.AutoSize = true;
                this.lbUsage.Location = new Point(10, 74);
                this.lbUsage.Margin = new Padding(3);
                this.lbUsage.Size = new Size(35, 15);
                this.lbUsage.TabIndex = 4;
                this.lbUsage.Text = "&Usage";

                this.cbUsage = new ComboBox();
                this.cbUsage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                this.cbUsage.DropDownStyle = ComboBoxStyle.DropDownList;
                this.cbUsage.FormattingEnabled = true;
                this.cbUsage.Location = new Point(80, 71);
                this.cbUsage.Size = new Size(0, 23);
                this.cbUsage.TabIndex = 5;
                this.cbUsage.DataSource = Enum.GetValues(typeof(UsageType));

                base.Controls.Add(this.cbUsage);
                base.Controls.Add(this.txValue);
                base.Controls.Add(this.lbUsage);
                base.Controls.Add(this.lbValue);
                base.Controls.Add(this.lbKey);
                base.Controls.Add(this.txKey);

                base.AutoScroll = true;
                base.BackColor = Color.Transparent;
                base.Dock = DockStyle.Fill;
                base.Location = new Point(62, 60);
                base.Padding = new Padding(10);
                base.Size = new Size(100, 100);
                base.TabIndex = 0;

                base.ResumeLayout(false);
                base.PerformLayout();
            }

            #endregion
        }

        private class BearerTokenPanel : Panel
        {
            #region Private Fields

            private Label lbToken;
            private TextBox txToken;

            #endregion

            #region Construction

            public BearerTokenPanel()
                : base()
            {
                this.InitializeComponent();
            }

            #endregion

            #region Public Properties

            public String Token
            {
                get
                {
                    return this.txToken.Text;
                }
                set
                {
                    this.txToken.Text = value;
                }
            }

            #endregion

            #region Private Methods

            private void InitializeComponent()
            {
                base.SuspendLayout();

                this.lbToken = new Label();
                this.lbToken.AutoSize = true;
                this.lbToken.Location = new Point(10, 16);
                this.lbToken.Margin = new Padding(3);
                this.lbToken.Size = new Size(26, 15);
                this.lbToken.TabIndex = 0;
                this.lbToken.Text = "&Token";

                this.txToken = new TextBox();
                this.txToken.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                this.txToken.Location = new Point(80, 13);
                this.txToken.Size = new Size(0, 23);
                this.txToken.TabIndex = 1;

                base.Controls.Add(this.lbToken);
                base.Controls.Add(this.txToken);

                base.AutoScroll = true;
                base.BackColor = Color.Transparent;
                base.Dock = DockStyle.Fill;
                base.Location = new Point(62, 60);
                base.Padding = new Padding(10);
                base.Size = new Size(100, 100);
                base.TabIndex = 0;

                base.ResumeLayout(false);
                base.PerformLayout();
            }

            #endregion
        }

        private class BasicAuthorizationPanel : Panel
        {
            #region Private Fields

            private Label lbUsername;
            private Label lbPassword;
            private TextBox txUsername;
            private TextBox txPassword;
            private CheckBox cbVisible;
            private Label lbEncoding;
            private ComboBox cbEncoding;

            #endregion

            #region Construction

            public BasicAuthorizationPanel()
                : base()
            {
                this.InitializeComponent();

                this.cbVisible.Checked = !this.txPassword.UseSystemPasswordChar;
                this.cbVisible.CheckedChanged += this.OnVisibleCheckedChanged;
            }

            #endregion

            #region Public Properties

            public String Username
            {
                get
                {
                    return this.txUsername.Text;
                }
                set
                {
                    this.txUsername.Text = value;
                }
            }

            public String Password
            {
                get
                {
                    return this.txPassword.Text;
                }
                set
                {
                    this.txPassword.Text = value;
                }
            }

            public Encoding Encoding
            {
                get
                {
                    return (Encoding)this.cbEncoding.SelectedValue;
                }
                set
                {
                    this.cbEncoding.SelectedValue = value;
                }
            }

            #endregion

            #region Event Handlers

            private void OnVisibleCheckedChanged(Object? sender, EventArgs args)
            {
                this.txPassword.UseSystemPasswordChar = !this.cbVisible.Checked;
                this.cbVisible.Image = this.cbVisible.Checked ? Resources.EyeDisabled : Resources.EyeEnabled;
            }

            #endregion

            #region Private Methods

            private void InitializeComponent()
            {
                this.txUsername = new TextBox();
                this.lbUsername = new Label();
                this.txPassword = new TextBox();
                this.lbPassword = new Label();
                this.cbEncoding = new ComboBox();
                this.lbEncoding = new Label();
                this.cbVisible = new CheckBox();

                base.SuspendLayout();

                this.txUsername.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                this.txUsername.Location = new Point(80, 13);
                this.txUsername.Size = new Size(165, 23);
                this.txUsername.TabIndex = 1;

                this.lbUsername.AutoSize = true;
                this.lbUsername.Location = new Point(10, 16);
                this.lbUsername.Margin = new Padding(3);
                this.lbUsername.Size = new Size(60, 15);
                this.lbUsername.TabIndex = 0;
                this.lbUsername.Text = "&Username";

                this.txPassword.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                this.txPassword.Location = new Point(80, 42);
                this.txPassword.Size = new Size(136, 23);
                this.txPassword.TabIndex = 3;
                this.txPassword.UseSystemPasswordChar = true;

                this.lbPassword.AutoSize = true;
                this.lbPassword.Location = new Point(10, 45);
                this.lbPassword.Margin = new Padding(3);
                this.lbPassword.Size = new Size(57, 15);
                this.lbPassword.TabIndex = 2;
                this.lbPassword.Text = "&Password";

                this.cbEncoding.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                this.cbEncoding.DropDownStyle = ComboBoxStyle.DropDownList;
                this.cbEncoding.FormattingEnabled = true;
                this.cbEncoding.Location = new Point(80, 71);
                this.cbEncoding.Size = new Size(165, 23);
                this.cbEncoding.TabIndex = 6;
                this.cbEncoding.DisplayMember = nameof(ComboBoxItem<Encoding>.Label);
                this.cbEncoding.ValueMember = nameof(ComboBoxItem<Encoding>.Value);
                this.cbEncoding.DataSource = this.CreateDataSource(this.GetSupportedEncodingRelations());

                this.lbEncoding.AutoSize = true;
                this.lbEncoding.Location = new Point(10, 74);
                this.lbEncoding.Margin = new Padding(3);
                this.lbEncoding.Size = new Size(57, 15);
                this.lbEncoding.TabIndex = 5;
                this.lbEncoding.Text = "&Encoding";

                this.cbVisible.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                this.cbVisible.Appearance = Appearance.Button;
                this.cbVisible.Cursor = Cursors.Hand;
                this.cbVisible.FlatAppearance.BorderSize = 0;
                this.cbVisible.FlatAppearance.MouseDownBackColor = Color.FromArgb(unchecked((Int32)0xFFCCD5F0));
                this.cbVisible.FlatAppearance.MouseOverBackColor = Color.FromArgb(unchecked((Int32)0xFFD8EAF9));
                this.cbVisible.FlatStyle = FlatStyle.Flat;
                this.cbVisible.Image = Resources.EyeEnabled;
                this.cbVisible.Location = new Point(221, 41);
                this.cbVisible.Size = new Size(24, 24);
                this.cbVisible.TabIndex = 4;
                this.cbVisible.UseVisualStyleBackColor = true;

                base.AutoScroll = true;
                base.BackColor = Color.Transparent;
                base.Dock = DockStyle.Fill;
                base.Location = new Point(62, 60);
                base.Padding = new Padding(10);
                base.Size = new Size(257, 450);
                base.TabIndex = 0;

                this.Controls.Add(this.cbEncoding);
                this.Controls.Add(this.cbVisible);
                this.Controls.Add(this.lbEncoding);
                this.Controls.Add(this.lbPassword);
                this.Controls.Add(this.txPassword);
                this.Controls.Add(this.lbUsername);
                this.Controls.Add(this.txUsername);

                base.ResumeLayout(false);
                base.PerformLayout();
            }

            private BindingList<ComboBoxItem<Encoding>> CreateDataSource(IEnumerable<(String CharSet, Encoding Encoding)> values)
            {
                return new BindingList<ComboBoxItem<Encoding>>(values.Select(x => new ComboBoxItem<Encoding>(x)).ToList());
            }

            #endregion
        }

        #endregion
    }
}
