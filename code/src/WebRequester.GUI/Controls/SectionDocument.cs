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

using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Extensions;
using Plexdata.WebRequester.GUI.Controls.Composed;
using Plexdata.WebRequester.GUI.Definitions;
using Plexdata.WebRequester.GUI.Extensions;
using Plexdata.WebRequester.GUI.Interfaces;
using Plexdata.WebRequester.GUI.Models.Projects;
using Plexdata.WebRequester.GUI.Models.Settings;
using System.ComponentModel;
using WeifenLuo.WinFormsUI.Docking;

namespace Plexdata.WebRequester.GUI.Controls;

internal partial class SectionDocument : DockContent, ISectionDocument
{
    #region Public Events

    public event EventHandler CloseAllDocuments;

    #endregion

    #region Private Fields

    private readonly ILogger logger = null;
    private readonly ISettings<ApplicationSettings> settings = null;
    private readonly ActionEntitiesEditorPanel edVariables = null;
    private readonly SecurityEntityEditorPanel edSecurity = null;

    private SectionEntity entity = null;

    #endregion

    #region Construction

    public SectionDocument(ILogger logger, ISettings<ApplicationSettings> settings)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

        this.settings.SaveSettings += this.OnSettingsSaveSettings;

        this.InitializeComponent();

        this.lbNotes.Text =
            "On the Variables tab you can define all needed variables." +
            Environment.NewLine +
            "On the Security tab you specify the authorization mode to be inherited.";

        this.lbVariables.Text =
            "It is recommended to use only the name of a variable, which means without any formatting instructions " +
            "such as << and >>. Furthermore, ensure that all variable names are unique. Otherwise, only the first " +
            "variable is used, if there are multiple occurrences of one and the same variable name. Finally, avoid " +
            "whitespaces as well as special characters in all variable names. Acceptable characters for variable " +
            "names are [a-z], [A-Z], [0-9], underscore and hyphen.";

        this.lbSecurity.Text =
            $"An authorization defined here is used for every request that uses \"{AuthorizationType.InheritFromParent.ToDisplay()}\". " +
            "The other way round, authorizations defined in subordinate requests overwrite an authorization defined here.";

        this.edVariables = new ActionEntitiesEditorPanel();
        this.edVariables.Heading = "Variable Replacements";
        this.edVariables.AllowBulkEdit = false;
        this.pnVariables.Controls.Add(this.edVariables);


        this.edSecurity = new SecurityEntityEditorPanel();
        this.edSecurity.Heading = "Authorization and Security";
        this.edSecurity.HideRemarks = true;
        this.edSecurity.CanInherit = false;

        this.pnSecurity.Controls.Add(this.edSecurity);


        base.DockAreas = DockAreas.Float | DockAreas.Document;
        base.ShowHint = DockState.Document;
    }

    #endregion

    #region Public Properties

    public SectionEntity Entity
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

            this.entity.Variables = this.edVariables.Value.Select(x => this.FromActionEntity(x));
            this.entity.Security = this.edSecurity.Value;
        }
        catch (Exception exception)
        {
            this.logger.Error("Saving section entity values failed unexpectedly.", exception, ("Source", nameof(SectionDocument)));
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

    #endregion

    #region Private Methods

    private void LoadEntityValues()
    {
        try
        {
            base.Text = this.entity.Label;
            this.edVariables.Value = this.entity.Variables.Select(x => this.IntoActionEntity(x));
            this.edSecurity.Value = this.entity.Security;
        }
        catch (Exception exception)
        {
            this.logger.Error("Loading section entity values failed unexpectedly.", exception, ("Source", nameof(SectionDocument)));
        }
    }

    private VariableEntity FromActionEntity(ActionEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), $"Value of {nameof(entity)}' must not be null");
        }

        // Unfortunately, this conversion is necessary because ActionEntity
        // does not have a value encryption like VariableEntity.

        return new VariableEntity()
        {
            Apply = entity.Apply,
            Label = entity.Label,
            Value = entity.Value,
            Notes = entity.Notes,
        };
    }

    private ActionEntity IntoActionEntity(VariableEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), $"Value of {nameof(entity)}' must not be null");
        }

        // Unfortunately, this conversion is necessary because ActionEntity
        // does not have a value encryption like VariableEntity.

        return new ActionEntity()
        {
            Apply = entity.Apply,
            Label = entity.Label,
            Value = entity.Value,
            Notes = entity.Notes,
        };
    }

    #endregion
}
