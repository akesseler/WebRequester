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

using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Extensions;
using Plexdata.WebRequester.GUI.Controls.Composed;
using Plexdata.WebRequester.GUI.Definitions;
using Plexdata.WebRequester.GUI.Dialogs;
using Plexdata.WebRequester.GUI.Events;
using Plexdata.WebRequester.GUI.Extensions;
using Plexdata.WebRequester.GUI.Helpers;
using Plexdata.WebRequester.GUI.Interfaces;
using Plexdata.WebRequester.GUI.Models.Execution;
using Plexdata.WebRequester.GUI.Models.Projects;
using Plexdata.WebRequester.GUI.Models.Settings;
using System.ComponentModel;
using System.Web;
using WeifenLuo.WinFormsUI.Docking;

namespace Plexdata.WebRequester.GUI.Controls;

internal partial class RequestDocument : DockContent, IRequestDocument
{
    #region Public Events

    public event EventHandler CloseAllDocuments;

    #endregion

    #region Private Fields

    private readonly ILogger logger = null;
    private readonly ISettings<ApplicationSettings> settings = null;
    private readonly IVariablesReplacer replacer = null;
    private readonly IRequestExecutor executor = null;
    private readonly IPayloadFormatter formatter = null;
    private readonly ActionEntitiesEditorPanel edRequestQueries = null;
    private readonly SecurityEntityEditorPanel edRequestSecurity = null;
    private readonly ActionEntitiesEditorPanel edRequestHeaders = null;
    private readonly PayloadEntityEditorPanel edRequestPayload = null;

    private RequestEntity entity = null;

    #endregion

    #region Construction

    public RequestDocument(ILogger logger, ISettings<ApplicationSettings> settings, IVariablesReplacer replacer, IRequestExecutor executor, IPayloadFormatter formatter)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        this.replacer = replacer ?? throw new ArgumentNullException(nameof(replacer));
        this.executor = executor ?? throw new ArgumentNullException(nameof(executor));
        this.formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

        this.settings.SaveSettings += this.OnSettingsSaveSettings;

        this.InitializeComponent();

        this.lbNotes.Text =
            "The usage of variables is possible. For this purpose, define all variables and there values in the " +
            "parent section beforehand. After that, these variables are referenced here using their names in the " +
            "form of <<value_name>>. Finally, please note that all variables are replaced using simple string " +
            "substitution, which is case-sensitive.";

        this.txUrl.Pasted += this.OnUrlTextPasted;

        this.edRequestQueries = new ActionEntitiesEditorPanel();
        this.edRequestQueries.Heading = "Query Parameters";
        this.tpRequestQueries.Controls.Add(this.edRequestQueries);

        this.edRequestSecurity = new SecurityEntityEditorPanel();
        this.edRequestSecurity.Heading = "Authorization and Security";
        this.tpRequestSecurity.Controls.Add(this.edRequestSecurity);

        this.edRequestSecurity.ShowQueriesPanel += this.OnRequestSecurityShowQueriesPanel;
        this.edRequestSecurity.ShowHeadersPanel += this.OnRequestSecurityShowHeadersPanel;

        this.edRequestHeaders = new ActionEntitiesEditorPanel();
        this.edRequestHeaders.Heading = "Header Values";
        this.tpRequestHeaders.Controls.Add(this.edRequestHeaders);

        this.edRequestPayload = new PayloadEntityEditorPanel();
        this.edRequestPayload.Heading = "Request Payload";
        this.tpRequestPayload.Controls.Add(this.edRequestPayload);

        base.DockAreas = DockAreas.Float | DockAreas.Document;
        base.ShowHint = DockState.Document;

        this.cbMethods.Items.Clear();
        this.cbMethods.Items.AddRange(this.AllowedHttpMethods());

        this.pnResponseDetails.FormatPayload += this.OnResponseDetailsFormatPayload;
    }

    #endregion

    #region Public Properties

    public RequestEntity Entity
    {
        get
        {
            return this.entity;
        }
        set
        {
            this.entity = value ?? throw new ArgumentNullException(nameof(this.Entity), $"Property '{nameof(this.Entity)}' must not be null.");

            this.entity.PropertyChanged += this.OnEntityPropertyChanged;

            this.LoadEntityValues();
        }
    }

    #endregion

    #region Public Methods

    public Boolean CanCheckMenuItem()
    {
        throw new NotSupportedException();
    }

    public void SaveEntityValues()
    {
        try
        {
            if (this.entity == null) { return; }

            // Method is set already!
            this.entity.Url = this.txUrl.Text;

            this.entity.Query.Parameters = this.edRequestQueries.Value;
            this.entity.Security = this.edRequestSecurity.Value;
            this.entity.Header.Headers = this.edRequestHeaders.Value;
            this.entity.Payload = this.edRequestPayload.Value;
        }
        catch (Exception exception)
        {
            this.logger.Error("Saving request entity values failed unexpectedly.", exception, ("Source", nameof(RequestDocument)));
        }
    }

    #endregion

    #region Event Handlers

    protected override void OnShown(EventArgs args)
    {
        base.OnShown(args);

        if (this.entity == null) { return; }

        this.entity.Visible = true;
    }

    protected override void OnClosed(EventArgs args)
    {
        base.OnClosed(args);

        if (this.entity == null) { return; }

        this.SaveEntityValues();

        this.entity.PropertyChanged -= this.OnEntityPropertyChanged;
        this.entity.Visible = false;
        this.entity.Document = null;
        this.entity = null;
    }

    private void OnSettingsSaveSettings(Object sender, EventArgs args)
    {
        this.SaveEntityValues();
    }

    private void OnEntityPropertyChanged(Object? sender, PropertyChangedEventArgs args)
    {
        if (String.Equals(args.PropertyName, nameof(this.entity.Label)))
        {
            base.Text = this.entity.Label;
        }
    }

    private void OnMethodsComboBoxSelectedIndexChanged(Object sender, EventArgs args)
    {
        try
        {
            String method = this.cbMethods.Items[this.cbMethods.SelectedIndex] as String;

            if (method != this.entity.Method)
            {
                this.entity.Method = method;
                this.entity.TreeNode.Text = this.entity.ToDisplay();
            }
        }
        catch (Exception exception)
        {
            this.logger.Error("Handling methods combo box selected index changed event failed unexpectedly.", exception);
        }
    }

    private void OnUrlTextPasted(Object sender, ClipboardEventArgs args)
    {
        // Example: http://example.org/path/subpath?query1=value1&query2=value2&query3=value3

        try
        {
            String value = args.ClipboardText.Trim();

            // Only way to ensure a valid URL.
            Uri _ = new Uri(value);

            String[] pieces = value.Split('?');

            if (pieces.Length > 0)
            {
                this.txUrl.Text = pieces[0];
            }

            if (pieces.Length > 1)
            {
                List<ActionEntity> actionEntities = new List<ActionEntity>();

                String[] queryItems = pieces[1].Split('&');

                foreach (String queryItem in queryItems)
                {
                    String[] querySlices = queryItem.Split('=');

                    String queryLabel = String.Empty;
                    String queryValue = String.Empty;

                    if (querySlices.Length > 0)
                    {
                        queryLabel = HttpUtility.UrlDecode(querySlices[0]);
                    }

                    if (querySlices.Length > 1)
                    {
                        queryValue = HttpUtility.UrlDecode(querySlices[1]);
                    }

                    if (!String.IsNullOrWhiteSpace(queryLabel))
                    {
                        actionEntities.Add(new ActionEntity(true, queryLabel, queryValue));
                    }
                }

                if (actionEntities.Count > 0)
                {
                    this.edRequestQueries.Value = actionEntities;
                }
            }
        }
        catch
        {
            this.txUrl.Replace(args.ClipboardText);
        }
    }

    private void OnResponseDetailsPanelHeightDecreased(Object sender, PanelHeightEventArgs args)
    {
        try
        {
            this.spContainer.SplitterDistance += args.Height;
            this.spContainer.FixedPanel = FixedPanel.Panel2;
            this.spContainer.IsSplitterFixed = true;
        }
        catch (Exception exception)
        {
            this.logger.Error("Decreasing response details view failed unexpectedly.", exception);
        }
    }

    private void OnResponseDetailsPanelHeightIncreased(Object sender, PanelHeightEventArgs args)
    {
        try
        {
            this.spContainer.FixedPanel = FixedPanel.None;
            this.spContainer.IsSplitterFixed = false;

            if (this.spContainer.SplitterDistance - args.Height > 0)
            {
                this.spContainer.SplitterDistance -= args.Height;
            }
            else
            {
                this.spContainer.SplitterDistance = this.spContainer.ClientSize.Height / 2;
            }
        }
        catch (Exception exception)
        {
            this.logger.Error("Increasing response details view failed unexpectedly.", exception);
        }
    }

    private async void OnButtonSendClicked(Object sender, EventArgs args)
    {
        using (new WaitCursor(this))
        {
            using (CancellationTokenSource cancellation = new CancellationTokenSource())
            {
                try
                {
                    ExecutionPendingDialog.Display(this, cancellation);

                    this.pnResponseDetails.Content = null;

                    this.SaveEntityValues();

                    SectionEntity section = this.entity.TreeNode.Parent.Tag as SectionEntity;
                    section.SaveValues();

                    RequestEntity request = (RequestEntity)this.entity.Clone();

                    if (request.Security.AuthorizationType == AuthorizationType.InheritFromParent)
                    {
                        request.Security = (SecurityEntity)section.Security.Clone();
                    }

                    request = this.replacer.Replace(request, section.Variables);

                    ResultEntity response = await this.executor.ExecuteAsync(request, cancellation.Token);

                    this.pnResponseDetails.Content = response;
                }
                catch (Exception exception)
                {
                    this.logger.Error("Executing request failed.", exception);
                }
                finally
                {
                    ExecutionPendingDialog.Dismiss();
                }
            }
        }
    }

    private void OnRequestSecurityShowQueriesPanel(Object? sender, EventArgs args)
    {
        this.tcRequestDetails.SelectedTab = this.tpRequestQueries;
    }

    private void OnRequestSecurityShowHeadersPanel(Object? sender, EventArgs args)
    {
        this.tcRequestDetails.SelectedTab = this.tpRequestHeaders;
    }

    private void OnDocumentContextMenuItemClicked(Object sender, EventArgs args)
    {
        if (sender == this.miCloseTab)
        {
            this.Close();
        }
        else if (sender == this.miCloseAllTabs)
        {
            this.CloseAllDocuments?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnResponseDetailsFormatPayload(Object? sender, FormatPayloadEventArgs args)
    {
        try
        {
            using (new WaitCursor(this))
            {
                args.ChangeResult(this.formatter.Format(args.Format, args.Source));
            }
        }
        catch (Exception exception)
        {
            this.logger.Error("Formatting payload failed unexpectedly.", exception, ("Source", nameof(RequestDocument)));
        }
    }

    #endregion

    #region Private Methods

    private void LoadEntityValues()
    {
        try
        {
            base.Text = this.entity.Label;

            this.cbMethods.SelectedItem = this.entity.Method;
            this.txUrl.Text = this.entity.Url;

            this.edRequestQueries.Value = this.entity.Query.Parameters;
            this.edRequestSecurity.Value = this.entity.Security;
            this.edRequestHeaders.Value = this.entity.Header.Headers;
            this.edRequestPayload.Value = this.entity.Payload;
        }
        catch (Exception exception)
        {
            this.logger.Error("Loading request entity values failed unexpectedly.", exception, ("Source", nameof(RequestDocument)));
        }
    }

    #endregion
}
