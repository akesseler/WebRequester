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

using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.WebRequester.GUI.Extensions;
using Plexdata.WebRequester.GUI.Helpers;
using Plexdata.WebRequester.GUI.Interfaces;
using Plexdata.WebRequester.GUI.Models.Settings;
using System.Diagnostics;
using WeifenLuo.WinFormsUI.Docking;

namespace Plexdata.WebRequester.GUI.Controls
{
    internal partial class LoggingInspector : DockContent, ILogger, IDockableControl
    {
        private readonly IFactory factory = null;
        private readonly ISettings<ApplicationSettings> settings = null;
        private readonly ISerializer serializer = null;
        private readonly IList<ILoggerEntity> storage = null;
        private readonly IList<ILoggerEntity> applied = null;

        public LoggingInspector(IFactory factory, ISettings<ApplicationSettings> settings)
            : base()
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.serializer = this.factory.Create<ISerializer>();

            this.settings.SaveSettings += this.OnSettingsSaveSettings;

            this.storage = new List<ILoggerEntity>();
            this.applied = new List<ILoggerEntity>();

            this.InitializeComponent();
            this.InitializeFiltering();
            this.UpdateControls();

            base.DockAreas = DockAreas.Float | DockAreas.DockBottom;
            base.ShowHint = DockState.DockBottom;
            base.DockStateChanged += this.OnControlDockStateChanged;

            this.lvItems.ChangeDoubleBuffering(true);

            Int32 width = 300;
            this.colMessage.Width = width;
            this.colException.Width = width;
        }

        #region Public Methods

        public Boolean CanCheckMenuItem()
        {
            return base.DockState != DockState.Unknown && base.DockState != DockState.Hidden;
        }

        #endregion

        #region From Implementation

        private void OnSettingsSaveSettings(Object sender, EventArgs args)
        {
            // Filters are saved already!
            this.settings.Value.LoggerSettings.DockState = base.DockState;
        }

        private void OnControlDockStateChanged(Object sender, EventArgs args)
        {
            this.sbMain.SizingGrip = base.DockState == DockState.Float;
        }

        private void OnFormVisibleChanged(Object sender, EventArgs args)
        {
            if (base.Visible && this.applied.Count > 0)
            {
                this.lvItems.EnsureVisible(this.applied.Count - 1);
            }
        }

        private void OnButtonClearClicked(Object sender, EventArgs args)
        {
            this.storage.Clear();
            this.applied.Clear();
            this.UpdateControls();
        }

        private void OnButtonSaveClicked(Object sender, EventArgs args)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                FilterIndex = 1,
                Filter = "Logging Files (*.log)|*.log|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (DialogResult.OK == dialog.ShowDialog())
            {
                try
                {
                    using (new WaitCursor(this))
                    {
                        this.serializer.SaveFile(dialog.FileName, this.storage.Select(x => x.ToSerializable()));
                    }
                }
                catch (Exception exception)
                {
                    this.ShowError("Saving all logging messages failed unexpectedly.", this.Text, exception);
                }
            }
        }

        private void OnRetrieveVirtualItem(Object sender, RetrieveVirtualItemEventArgs args)
        {
            Int32 index = args.ItemIndex;

            if (index >= 0 && index < this.applied.Count)
            {
                args.Item = this.applied[index].ToListViewItem();
                return;
            }

            throw new IndexOutOfRangeException();
        }

        private void OnListViewMouseDoubleClick(Object sender, MouseEventArgs args)
        {
            if ((sender as ListView)?.HitTest(args.X, args.Y)?.Item is not ILoggerEntity entity)
            {
                return;
            }

            this.factory.Create<ILoggerEntityDetails>(entity, this.serializer).ShowDialog(this);
        }

        private void OnFilterItemCheckedChanged(Object sender, EventArgs args)
        {
            try
            {
                if (sender is ToolStripMenuItem item)
                {
                    this.settings.Value.LoggerSettings.Filters[(LogLevel)item.Tag] = item.Checked;
                }

                this.ApplyFiltering();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
            }
        }

        private void InitializeFiltering()
        {
            this.btFilter.DropDownItems.Clear();

            foreach (KeyValuePair<LogLevel, Boolean> filter in this.settings.Value.LoggerSettings.Filters)
            {
                ToolStripMenuItem item = new ToolStripMenuItem()
                {
                    Tag = filter.Key,
                    Text = filter.Key.GetDisplayText(),
                    Checked = filter.Value,
                    CheckOnClick = true
                };

                item.CheckedChanged += this.OnFilterItemCheckedChanged;

                this.btFilter.DropDownItems.Add(item);
            }
        }

        private void ApplyFiltering()
        {
            this.applied.Clear();

            foreach (ILoggerEntity entity in this.storage)
            {
                if (this.IsFilterApplied(entity.Level))
                {
                    this.applied.Add(entity);
                }
            }

            this.UpdateControls();
        }

        private Boolean IsFilterApplied(LogLevel level)
        {
            try
            {
                return this.settings.Value.LoggerSettings.Filters[level];
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
                return false;
            }
        }

        private void UpdateControls()
        {
            if (this.storage.Count > 0)
            {
                this.btClear.Enabled = true;
                this.btSave.Enabled = true;
                this.slMain.Text = String.Format("{0:#,##0}/{1:#,##0}", this.storage.Count, this.applied.Count);
            }
            else
            {
                this.btClear.Enabled = false;
                this.btSave.Enabled = false;
                this.slMain.Text = "0";
            }

            this.sbMain.Refresh();
            this.tbMain.Refresh();

            this.lvItems.VirtualListSize = this.applied.Count;
        }

        #endregion

        #region ILogger Implementation

        public Boolean IsDisabled { get { return false; } }

        public IDisposable BeginScope<TScope>(TScope scope)
        {
            throw new NotSupportedException();
        }

        public Boolean IsEnabled(LogLevel level)
        {
            return true;
        }

        public void Write(LogLevel level, String message)
        {
            this.Write(level, message, null, null);
        }

        public void Write(LogLevel level, String message, params (String Label, Object Value)[] details)
        {
            this.Write(level, message, null, details);
        }

        public void Write(LogLevel level, Exception exception)
        {
            this.Write(level, null, exception, null);
        }

        public void Write(LogLevel level, Exception exception, params (String Label, Object Value)[] details)
        {
            this.Write(level, null, exception, details);
        }

        public void Write(LogLevel level, String message, Exception exception)
        {
            this.Write(level, message, exception, null);
        }

        public void Write(LogLevel level, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            ILoggerEntity entity = this.factory.Create<ILoggerEntity>(level, message, exception, details);

            this.storage.Add(entity);

            if (this.IsFilterApplied(level))
            {
                this.applied.Add(entity);
            }

            this.UpdateControls();

            if (this.applied.Count > 0)
            {
                this.lvItems.EnsureVisible(this.applied.Count - 1);
            }
        }

        public void Write<TScope>(TScope scope, LogLevel level, String message)
        {
            throw new NotSupportedException();
        }

        public void Write<TScope>(TScope scope, LogLevel level, String message, params (String Label, Object Value)[] details)
        {
            throw new NotSupportedException();
        }

        public void Write<TScope>(TScope scope, LogLevel level, Exception exception)
        {
            throw new NotSupportedException();
        }

        public void Write<TScope>(TScope scope, LogLevel level, Exception exception, params (String Label, Object Value)[] details)
        {
            throw new NotSupportedException();
        }

        public void Write<TScope>(TScope scope, LogLevel level, String message, Exception exception)
        {
            throw new NotSupportedException();
        }

        public void Write<TScope>(TScope scope, LogLevel level, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
