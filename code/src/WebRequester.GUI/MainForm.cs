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
using Plexdata.WebRequester.GUI.Definitions;
using Plexdata.WebRequester.GUI.Events;
using Plexdata.WebRequester.GUI.Extensions;
using Plexdata.WebRequester.GUI.Helpers;
using Plexdata.WebRequester.GUI.Interfaces;
using Plexdata.WebRequester.GUI.Models.Settings;
using WeifenLuo.WinFormsUI.Docking;

namespace Plexdata.WebRequester.GUI
{
    internal partial class MainForm : Form
    {
        private readonly ILogger logger = null;
        private readonly IFactory factory = null;
        private readonly ISettings<ApplicationSettings> settings = null;
        private readonly IProjectExplorer projects = null;

        public MainForm(ILogger logger, IFactory factory, ISettings<ApplicationSettings> settings)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

            this.settings.SaveSettings += this.OnSettingsSaveSettings;

            this.InitializeComponent();

            this.dpContainer.Theme = new VS2015LightTheme();
            this.projects = this.factory.Create<IProjectExplorer>();
            this.projects.ShowRequestEntityDocument += this.OnProjectExplorerShowRequestEntityDocument;
            this.projects.ShowSectionEntityDocument += this.OnProjectsExplorerShowSectionEntityDocument;

            this.AssignDockingProperties(this.projects, this.OnProjectExplorerVisibleChanged);
            this.AssignDockingProperties(this.logger, this.OnLoggingControlVisibleChanged);
        }

        #region Event Handlers

        protected override void OnHandleCreated(EventArgs args)
        {
            base.OnHandleCreated(args);

            using (new WaitCursor(this))
            {
                this.LoadSettings();

                base.StartPosition = FormStartPosition.Manual;
                base.WindowState = FormWindowState.Normal;
                base.DesktopBounds = this.settings.Value.WindowSettings.DesktopBounds;

                this.EnsureScreenLocation();
            }
        }

        protected override void OnLoad(EventArgs args)
        {
            base.OnLoad(args);

            using (new WaitCursor(this))
            {
                this.miProject.Checked = this.settings.Value.ProjectSettings.CanCheckMenuItem();
                this.projects.DockState = this.settings.Value.ProjectSettings.DockState;

                this.miLogging.Checked = this.settings.Value.LoggerSettings.CanCheckMenuItem();
                ((IDockableControl)this.logger).DockState = this.settings.Value.LoggerSettings.DockState;

                this.projects.OpenLastDocuments();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs args)
        {
            using (new WaitCursor(this))
            {
                this.SaveSettings();
            }

            base.OnFormClosing(args);
        }

        protected override void OnResizeEnd(EventArgs args)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.settings.Value.WindowSettings.DesktopBounds = base.DesktopBounds;
            }

            base.OnResizeEnd(args);
        }

        private void OnSettingsSaveSettings(Object sender, EventArgs args)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.settings.Value.WindowSettings.DesktopBounds = base.DesktopBounds;
            }
        }

        private void OnProjectExplorerVisibleChanged(Object sender, EventArgs args)
        {
            if (sender is IDockableControl control)
            {
                this.miProject.Checked = control.CanCheckMenuItem();
            }
        }

        private void OnLoggingControlVisibleChanged(Object sender, EventArgs args)
        {
            if (sender is IDockableControl control)
            {
                this.miLogging.Checked = control.CanCheckMenuItem();
            }
        }

        private void OnOpenMenuItemClicked(Object sender, EventArgs args)
        {
            this.OpenRequesterProjects();
        }

        private void OnSaveMenuItemClicked(Object sender, EventArgs args)
        {
            this.SaveRequesterProjects();
        }

        private void OnImportMenuItemClicked(Object sender, EventArgs args)
        {
            if (sender == this.miRequesterProjects)
            {
                this.ImportRequesterProjects();
                return;
            }

            if (sender == this.miPostmanCollection)
            {
                this.ImportPostmanCollection();
                return;
            }
        }

        private void OnExportMenuItemClicked(Object sender, EventArgs args)
        {
            this.ExportRequesterProjects();
        }

        private void OnExitMenuClicked(Object sender, EventArgs args)
        {
            base.Close();
        }

        private void OnLoggingMenuCheckedChanged(Object sender, EventArgs args)
        {
            if (sender is not ToolStripMenuItem menu)
            {
                return;
            }

            if (this.logger is IDockableControl control)
            {
                if (menu.Checked)
                {
                    control.Show(this.dpContainer);
                }
                else
                {
                    control.Hide();
                }
            }
        }

        private void OnProjectExplorerMenuCheckedChanged(Object sender, EventArgs args)
        {
            if (sender is not ToolStripMenuItem menu)
            {
                return;
            }

            if (this.projects is IDockableControl control)
            {
                if (menu.Checked)
                {
                    control.Show(this.dpContainer);
                }
                else
                {
                    control.Hide();
                }
            }
        }

        private void OnAboutMenuClicked(Object sender, EventArgs args)
        {
            this.factory.Create<IAboutDialog>().ShowDialog(this);
        }

        private void OnProjectExplorerShowRequestEntityDocument(Object? sender, ShowRequestEntityDocumentEventArgs args)
        {
            try
            {
                using (new WaitCursor(this))
                {
                    using (new UpdateLocker(this))
                    {
                        if (args.Entity.Document == null)
                        {
                            args.Entity.Document = this.factory.Create<IRequestDocument>();
                            args.Entity.Document.Entity = args.Entity;
                            args.Entity.Document.CloseAllDocuments += this.OnDocumentCloseAllDocuments;
                            args.Entity.Document.Show(this.dpContainer);
                        }
                        else
                        {
                            args.Entity.Document.Activate();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
        }

        private void OnProjectsExplorerShowSectionEntityDocument(Object? sender, ShowSectionEntityDocumentEventArgs args)
        {
            try
            {
                using (new WaitCursor(this))
                {
                    using (new UpdateLocker(this))
                    {
                        if (args.Entity.Document == null)
                        {
                            args.Entity.Document = this.factory.Create<ISectionDocument>();
                            args.Entity.Document.Entity = args.Entity;
                            args.Entity.Document.CloseAllDocuments += this.OnDocumentCloseAllDocuments;
                            args.Entity.Document.Show(this.dpContainer);
                        }
                        else
                        {
                            args.Entity.Document.Activate();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }
        }

        private void OnDocumentCloseAllDocuments(Object? sender, EventArgs args)
        {
            this.projects.CloseAllDocuments();
        }

        #endregion

        #region Private Methods

        private void LoadSettings()
        {
            try
            {
                ((ISettingsManager)this.settings).Load(this.GetSettingsFilename());
            }
            catch (Exception exception)
            {
                this.logger.Error("Loading settings failed unexpectedly.", exception);
                this.ShowError("Loading settings failed unexpectedly.", this.Text, exception);
            }
        }

        private void SaveSettings()
        {
            try
            {
                ((ISettingsManager)this.settings).Save(this.GetSettingsFilename());
            }
            catch (Exception exception)
            {
                this.logger.Error("Saving settings failed unexpectedly.", exception);
                this.ShowError("Saving settings failed unexpectedly.", this.Text, exception);
            }
        }

        private void EnsureScreenLocation()
        {
            Rectangle desktop = base.DesktopBounds;

            foreach (Screen screen in Screen.AllScreens)
            {
                // Manually moving the window stops exactly at the boundaries of the visible
                // area. In such a case, method IntersectsWith() returns true and the window
                // still remains outside the visible area. The solution to this problem is
                // to reduce the visible area slightly.

                Rectangle working = screen.WorkingArea;

                working.Inflate(-2, -2);

                if (working.IntersectsWith(desktop))
                {
                    return;
                }
            }

            Rectangle primary = Screen.PrimaryScreen.WorkingArea;

            Int32 x = primary.Left + (primary.Width - desktop.Width) / 2;
            Int32 y = primary.Top + (primary.Height - desktop.Height) / 2;

            desktop.X = x;
            desktop.Y = y;

            base.DesktopBounds = desktop;
        }

        private void AssignDockingProperties(Object dockableControl, EventHandler visibleChangedHandler)
        {
            if (dockableControl is IDockableControl control)
            {
                control.DockPanel = this.dpContainer;
                control.VisibleChanged += visibleChangedHandler;
            }
        }

        private void OpenRequesterProjects()
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                FilterIndex = 1,
                Filter = "Project Files (*.wrp)|*.wrp|Backup Files (*.bak)|*.bak|JSON Files (*.json)|*.json|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Multiselect = false,
                RestoreDirectory = true,
                InitialDirectory = this.GetLocation(LocationType.Open),
                Title = "Open Projects"
            };

            if (DialogResult.OK == dialog.ShowDialog())
            {
                try
                {
                    using (new WaitCursor(this))
                    {
                        this.projects.BackupFile(this.GetBackupFilename());
                        this.projects.LoadFile(dialog.FileName);
                    }
                }
                catch (Exception exception)
                {
                    this.logger.Error("Open projects failed.", exception, ("Filename", dialog.FileName));
                    this.ShowError("Opening project file failed unexpectedly. See logging messages for more details.", this.Text);
                }
            }
        }

        private void SaveRequesterProjects()
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                FilterIndex = 1,
                Filter = "Project Files (*.wrp)|*.wrp|Backup Files (*.bak)|*.bak|JSON Files (*.json)|*.json|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                RestoreDirectory = true,
                InitialDirectory = this.GetLocation(LocationType.Save),
                FileName = this.GetSaveFilename(),
                Title = "Save Projects"
            };

            if (DialogResult.OK == dialog.ShowDialog())
            {
                try
                {
                    using (new WaitCursor(this))
                    {
                        this.projects.SaveFile(dialog.FileName);
                    }
                }
                catch (Exception exception)
                {
                    this.logger.Error("Saving projects failed.", exception, ("Filename", dialog.FileName));
                    this.ShowError("Saving project file failed unexpectedly. See logging messages for more details.", this.Text);
                }
            }
        }

        private void ImportRequesterProjects()
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                FilterIndex = 1,
                Filter = "Project Files (*.wrp)|*.wrp|Backup Files (*.bak)|*.bak|JSON Files (*.json)|*.json|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Multiselect = false,
                RestoreDirectory = true,
                InitialDirectory = this.GetLocation(LocationType.Import),
                Title = "Import Project"
            };

            if (DialogResult.OK == dialog.ShowDialog())
            {
                try
                {
                    using (new WaitCursor(this))
                    {
                        this.projects.ImportFile(dialog.FileName, ImportType.Standard);
                    }
                }
                catch (Exception exception)
                {
                    this.logger.Error("Project import failed.", exception, ("Filename", dialog.FileName));
                    this.ShowError("Importing projects failed unexpectedly. See logging messages for more details.", this.Text);
                }
            }
        }

        private void ImportPostmanCollection()
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                FilterIndex = 1,
                Filter = "Postman Collections (*.json)|*.json|All Files (*.*)|*.*",
                Multiselect = false,
                RestoreDirectory = true,
                InitialDirectory = null,
                Title = "Postman Collection"
            };

            if (DialogResult.OK == dialog.ShowDialog())
            {
                try
                {
                    using (new WaitCursor(this))
                    {
                        this.projects.ImportFile(dialog.FileName, ImportType.Postman);
                    }
                }
                catch (Exception exception)
                {
                    this.logger.Error("Postman import failed.", exception, ("Filename", dialog.FileName));
                    this.ShowError("Importing Postman collection failed unexpectedly. See logging messages for more details.", this.Text);
                }
            }
        }

        private void ExportRequesterProjects()
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                FilterIndex = 3,
                Filter = "Project Files (*.wrp)|*.wrp|Backup Files (*.bak)|*.bak|JSON Files (*.json)|*.json|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                RestoreDirectory = true,
                InitialDirectory = this.GetLocation(LocationType.Export),
                FileName = this.GetExportFilename(),
                Title = "Export Project"
            };

            if (DialogResult.OK == dialog.ShowDialog())
            {
                try
                {
                    using (new WaitCursor(this))
                    {
                        this.projects.ExportFile(dialog.FileName);
                    }
                }
                catch (Exception exception)
                {
                    this.logger.Error("Project export failed.", exception, ("Filename", dialog.FileName));
                    this.ShowError("Exporting project failed unexpectedly. See logging messages for more details.", this.Text);
                }
            }
        }

        #endregion
    }
}