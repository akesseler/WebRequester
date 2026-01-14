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

using Plexdata.WebRequester.GUI.Definitions;
using Plexdata.WebRequester.GUI.Events;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Plexdata.WebRequester.GUI.Controls.General;

internal class TextEditor : Panel
{
    #region Public Events

    public event EventHandler<FormatPayloadEventArgs> FormatPayload
    {
        add
        {
            this.editor.FormatPayload += value;
        }
        remove
        {
            this.editor.FormatPayload -= value;
        }
    }

    public event EventHandler<EventArgs> ExportPayload
    {
        add
        {
            this.editor.ExportPayload += value;
        }
        remove
        {
            this.editor.ExportPayload -= value;
        }
    }

    #endregion

    #region Public Types

    [Flags]
    public enum ContextMenuItems
    {
        None = 0x0000,
        Find = 0x0001,
        Undo = 0x0002,
        Cut = 0x0004,
        Copy = 0x0008,
        Paste = 0x0010,
        Remove = 0x0020,
        Select = 0x0040,
        Format = 0x0080,
        Export = 0x0100,
        Default = Find | Cut | Copy | Paste | Select,
        ReadOnly = Find | Copy | Select,
        Editable = Find | Undo | Cut | Copy | Paste | Remove | Select,
        Minimal = Cut | Copy | Paste,
        Maximal = Find | Undo | Cut | Copy | Paste | Remove | Select | Format | Export,
    }

    #endregion

    #region Private Fields

    private readonly InternalEditor editor;

    #endregion

    #region Construction

    public TextEditor()
        : base()
    {
        this.editor = new InternalEditor();

        this.SuspendLayout();
        this.editor.SuspendLayout();

        this.editor.BorderStyle = BorderStyle.None;
        this.editor.Dock = DockStyle.Fill;
        this.editor.HideSelection = false;
        this.editor.Location = new Point(0, 0);
        this.editor.Multiline = true;
        this.editor.ScrollBars = ScrollBars.Both;
        this.editor.Size = new Size(0, 0);
        this.editor.TabIndex = 0;

        base.Controls.Add(this.editor);

        this.editor.ResumeLayout(false);
        this.editor.PerformLayout();

        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    #region Public Properties

    public override Color BackColor
    {
        get
        {
            return this.editor.BackColor;
        }
        set
        {
            this.editor.BackColor = value;
        }
    }

    [Browsable(false)]
    [DefaultValue(ContextMenuItems.Default)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ContextMenuItems ContextMenuSelection
    {
        get
        {
            return this.editor.ContextMenuSelection;
        }
        set
        {
            this.editor.ContextMenuSelection = value;
        }
    }

    public override Font Font
    {
        get
        {
            return base.Font;
        }
        set
        {
            base.Font = value;
            this.editor.Font = value;
        }
    }

    public override String Text
    {
        get
        {
            return this.editor.Text;
        }
        set
        {
            this.editor.Text = value;
            this.editor.SelectionLength = 0;
            this.editor.SelectionStart = 0;
        }
    }

    public String[] Lines
    {
        get
        {
            return this.editor.Lines;
        }
        set
        {
            this.editor.Lines = value;
            this.editor.SelectionLength = 0;
            this.editor.SelectionStart = 0;
        }
    }

    public Boolean ReadOnly
    {
        get
        {
            return this.editor.ReadOnly;
        }
        set
        {
            this.editor.ReadOnly = value;
        }
    }

    #endregion

    #region Private Classes

    private class InternalEditor : TextBox
    {
        #region Public Events

        public event EventHandler<FormatPayloadEventArgs> FormatPayload;

        public event EventHandler<EventArgs> ExportPayload;

        #endregion

        #region Private Fields

        private readonly FindPanel findPanel = null;

        private ContextMenuItems contextMenuSelection = ContextMenuItems.None;

        #endregion

        #region Construction

        public InternalEditor()
            : base()
        {
            // Mouse selection in single line mode may cause flickering. But this seems
            // to be a bug in the Framework, because it can be reproduced by choosing
            // Multiline = false and applying Lines instead of setting Text property.
            // Note that this issue can't be solved by enabling DoubleBuffered mode.

            base.DoubleBuffered = true;
            base.ContextMenuStrip = new ContextMenuStrip();
            this.ContextMenuSelection = ContextMenuItems.Default;
            base.HideSelection = false;
            base.Multiline = true;
            base.WordWrap = false;
            base.ScrollBars = ScrollBars.Both;

            this.findPanel = new FindPanel(this);
        }

        #endregion

        #region Public Properties

        public ContextMenuItems ContextMenuSelection
        {
            get
            {
                return this.contextMenuSelection;
            }
            set
            {
                if (this.contextMenuSelection == value)
                {
                    return;
                }

                if ((ContextMenuItems.Maximal & value) != value)
                {
                    throw new ArgumentException($"Value of '{(Int32)value}' is undefined.", nameof(this.ContextMenuSelection));
                }

                this.contextMenuSelection = value;
                this.AssignContextMenu();
            }
        }

        public Boolean CanFind
        {
            get
            {
                return base.Multiline;
            }
        }

        public Boolean CanCut
        {
            get
            {
                return !base.ReadOnly && base.SelectionLength > 0;
            }
        }

        public Boolean CanCopy
        {
            get
            {
                return base.SelectionLength > 0;
            }
        }

        public Boolean CanPaste
        {
            get
            {
                return !base.ReadOnly && Clipboard.ContainsText();
            }
        }

        public Boolean CanRemove
        {
            get
            {
                return !base.ReadOnly && base.SelectionLength > 0;
            }
        }

        public Boolean CanFormat
        {
            get
            {
                return base.TextLength > 0;
            }
        }

        public Boolean CanExport
        {
            get
            {
                return base.TextLength > 0;
            }
        }

        #endregion

        #region Protected Methods

        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                this.findPanel.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override void WndProc(ref Message message)
        {
            const Int32 EM_SETSEL = 0x00B1;
            const Int32 WM_PASTE = 0x0302;

            switch (message.Msg)
            {
                case EM_SETSEL:
                    this.findPanel.Offset = message.WParam.ToInt32();
                    break; // Execute default processing!
                case WM_PASTE:
                    this.OnPasteMenuItemClicked(this, EventArgs.Empty);
                    return; // Prevent default processing!
            }

            base.WndProc(ref message);
        }

        protected override Boolean ProcessCmdKey(ref Message message, Keys keys)
        {
            const Int32 WM_KEYDOWN = 0x100;

            if (message.Msg == WM_KEYDOWN)
            {
                if ((keys & Keys.Modifiers) == Keys.Control && (keys & Keys.KeyCode) == Keys.F)
                {
                    if (this.findPanel.Visible)
                    {
                        this.findPanel.Activate();
                    }
                    else
                    {
                        this.findPanel.Show();
                    }
                }
                else if ((keys & Keys.KeyCode) == Keys.F3)
                {
                    if ((keys & Keys.Modifiers) == Keys.Control)
                    {
                        this.findPanel.FindFirst(this.SelectedText, this.SelectionStart);
                    }
                    else if ((keys & Keys.Modifiers) == Keys.Shift)
                    {
                        this.findPanel.FindPrev(this.SelectionStart - this.SelectionLength);
                    }
                    else
                    {
                        this.findPanel.FindNext(this.SelectionStart + this.SelectionLength);
                    }
                }
                else if (keys == Keys.Escape)
                {
                    this.findPanel.Hide();
                    this.Select();
                }
            }

            return base.ProcessCmdKey(ref message, keys);
        }

        #endregion

        #region Protected Events

        protected override void OnMouseUp(MouseEventArgs args)
        {
            base.OnMouseUp(args);
            this.findPanel.Offset = this.SelectionStart;
        }

        protected override void OnKeyUp(KeyEventArgs args)
        {
            base.OnKeyUp(args);
            this.findPanel.Offset = this.SelectionStart;
        }

        #endregion

        #region Private Events

        private void OnContextMenuOpening(Object sender, CancelEventArgs args)
        {
            if (sender is ContextMenuStrip menu)
            {
                foreach (Object temp in menu.Items)
                {
                    if (temp is ToolStripMenuItem item)
                    {
                        switch ((ContextMenuItems)item.Tag)
                        {
                            case ContextMenuItems.None:
                                break;
                            case ContextMenuItems.Find:
                                item.Enabled = this.CanFind;
                                break;
                            case ContextMenuItems.Undo:
                                item.Enabled = base.CanUndo;
                                break;
                            case ContextMenuItems.Export:
                                item.Enabled = this.CanExport;
                                break;
                            case ContextMenuItems.Cut:
                                item.Enabled = this.CanCut;
                                break;
                            case ContextMenuItems.Copy:
                                item.Enabled = this.CanCopy;
                                break;
                            case ContextMenuItems.Paste:
                                item.Enabled = this.CanPaste;
                                break;
                            case ContextMenuItems.Remove:
                                item.Enabled = this.CanRemove;
                                break;
                            case ContextMenuItems.Select:
                                item.Enabled = base.CanSelect;
                                break;
                            case ContextMenuItems.Format:
                                item.Enabled = this.CanFormat;
                                break;
                            default:
                                throw new NotSupportedException("Fix this missing context menu item type.");
                        }
                    }
                }
            }
        }

        private void OnFindMenuItemClicked(Object sender, EventArgs args)
        {
            if (this.findPanel.Visible)
            {
                this.findPanel.Hide();
            }
            else
            {
                this.findPanel.Show();
            }
        }

        private void OnUndoMenuItemClicked(Object sender, EventArgs args)
        {
            try
            {
                base.Undo();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private void OnCutMenuItemClicked(Object sender, EventArgs args)
        {
            try
            {
                base.Cut();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private void OnCopyMenuItemClicked(Object sender, EventArgs args)
        {
            try
            {
                base.Copy();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private void OnPasteMenuItemClicked(Object sender, EventArgs args)
        {
            try
            {
                foreach (TextDataFormat format in Enum.GetValues<TextDataFormat>())
                {
                    if (Clipboard.ContainsText(format))
                    {
                        base.SelectedText = Clipboard.GetText(format);
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private void OnRemoveMenuItemClicked(Object sender, EventArgs args)
        {
            try
            {
                base.Text = base.Text.Remove(base.SelectionStart, base.SelectionLength);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private void OnSelectMenuItemClicked(Object sender, EventArgs args)
        {
            try
            {
                base.SelectAll();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private void OnFormatMenuItemClicked(Object sender, EventArgs _)
        {
            try
            {
                if (sender is not ToolStripMenuItem item)
                {
                    return;
                }

                if (item.Tag is not FormatType format)
                {
                    return;
                }

                FormatPayloadEventArgs args = new FormatPayloadEventArgs(format, base.Text);
                this.FormatPayload?.Invoke(this, args);
                base.Text = args.Result;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private void OnExportMenuItemClicked(Object sender, EventArgs args)
        {
            try
            {
                this.ExportPayload?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        #endregion

        #region Private Methods

        private void AssignContextMenu()
        {
            ContextMenuItems selection = this.ContextMenuSelection;

            if (selection == ContextMenuItems.None)
            {
                this.ContextMenuStrip = null;
                return;
            }

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item = null;
            Boolean separator = false;

            menu.Opening += this.OnContextMenuOpening;

            if (selection.HasFlag(ContextMenuItems.Find))
            {
                item = new ToolStripMenuItem();
                item.Tag = ContextMenuItems.Find;
                item.Text = "Find";
                item.Click += this.OnFindMenuItemClicked;
                menu.Items.Add(item);

                separator = true;
            }

            if (selection.HasFlag(ContextMenuItems.Undo))
            {
                item = new ToolStripMenuItem();
                item.Tag = ContextMenuItems.Undo;
                item.Text = "Undo";
                item.Click += this.OnUndoMenuItemClicked;
                menu.Items.Add(item);

                separator = true;
            }

            if (selection.HasFlag(ContextMenuItems.Export))
            {
                item = new ToolStripMenuItem();
                item.Tag = ContextMenuItems.Export;
                item.Text = "Export";
                item.Click += this.OnExportMenuItemClicked;
                menu.Items.Add(item);

                separator = true;
            }

            if (separator)
            {
                separator = false;

                ContextMenuItems excluded = ContextMenuItems.Find | ContextMenuItems.Undo | ContextMenuItems.Export;
                ContextMenuItems included = selection & ~excluded;

                if (included != ContextMenuItems.None)
                {
                    menu.Items.Add(new ToolStripSeparator());
                }
            }

            if (selection.HasFlag(ContextMenuItems.Cut))
            {
                item = new ToolStripMenuItem();
                item.Tag = ContextMenuItems.Cut;
                item.Text = "Cut";
                item.Click += this.OnCutMenuItemClicked;
                menu.Items.Add(item);

                separator = true;
            }

            if (selection.HasFlag(ContextMenuItems.Copy))
            {
                item = new ToolStripMenuItem();
                item.Tag = ContextMenuItems.Copy;
                item.Text = "Copy";
                item.Click += this.OnCopyMenuItemClicked;
                menu.Items.Add(item);

                separator = true;
            }

            if (selection.HasFlag(ContextMenuItems.Paste))
            {
                item = new ToolStripMenuItem();
                item.Tag = ContextMenuItems.Paste;
                item.Text = "Paste";
                item.Click += this.OnPasteMenuItemClicked;
                menu.Items.Add(item);

                separator = true;
            }

            if (separator)
            {
                separator = false;

                ContextMenuItems excluded = ContextMenuItems.Find | ContextMenuItems.Undo | ContextMenuItems.Export | ContextMenuItems.Cut | ContextMenuItems.Copy | ContextMenuItems.Paste;
                ContextMenuItems included = selection & ~excluded;

                if (included != ContextMenuItems.None)
                {
                    menu.Items.Add(new ToolStripSeparator());
                }
            }

            if (selection.HasFlag(ContextMenuItems.Remove))
            {
                item = new ToolStripMenuItem();
                item.Tag = ContextMenuItems.Remove;
                item.Text = "Remove";
                item.Click += this.OnRemoveMenuItemClicked;
                menu.Items.Add(item);

                separator = true;
            }

            if (selection.HasFlag(ContextMenuItems.Select))
            {
                item = new ToolStripMenuItem();
                item.Tag = ContextMenuItems.Select;
                item.Text = "Select All";
                item.Click += this.OnSelectMenuItemClicked;
                menu.Items.Add(item);

                separator = true;
            }

            if (separator)
            {
                separator = false;

                ContextMenuItems excluded = ContextMenuItems.Find | ContextMenuItems.Undo | ContextMenuItems.Export | ContextMenuItems.Cut | ContextMenuItems.Copy | ContextMenuItems.Paste | ContextMenuItems.Remove | ContextMenuItems.Select;
                ContextMenuItems included = selection & ~excluded;

                if (included != ContextMenuItems.None)
                {
                    menu.Items.Add(new ToolStripSeparator());
                }
            }

            if (selection.HasFlag(ContextMenuItems.Format))
            {
                item = new ToolStripMenuItem();
                item.Tag = ContextMenuItems.Format;
                item.Text = "Format";
                item.DropDownItems.AddRange([
                    new ToolStripMenuItem(FormatType.Json.ToString().ToUpper(), null, this.OnFormatMenuItemClicked) { Tag = FormatType.Json },
                    new ToolStripMenuItem(FormatType.Xml.ToString().ToUpper(),  null, this.OnFormatMenuItemClicked) { Tag = FormatType.Xml  },
                    new ToolStripMenuItem(FormatType.Html.ToString().ToUpper(), null, this.OnFormatMenuItemClicked) { Tag = FormatType.Html }
                ]);

                menu.Items.Add(item);
            }

            this.ContextMenuStrip = menu;
        }

        #endregion

        #region Private Classes

        private class FindPanel : Panel
        {
            #region Private Constants

            private const String caseButtonImage =
                "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8" +
                "YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAFaSURBVDhPpZK9K0VxGMePREqMivwJ2MzKcHKk81anTDoZ" +
                "zmQgiYVOSWQ6FptFDExm3cEdxEDKZBGTibC4Cvf6PLf7O/0O53odPj3P9/u8/H7nxYjj+F/kmr/hkxEE" +
                "QbNhGA0f/XpkhOu6JlRgW/e/IiNkEC4cxylZltWu1+qRJrZttzH85HneAPEBQr0xiqImaiOwAHMc0it+" +
                "2oAxxtCVPD9xAwqqJqB34JDhFeIuvPi+36837LNkWXKaBtFlGrpVnbxT5QL1Y/qWqoLrd2G8EftE8yUa" +
                "0Tc0zOhD6A4OGSaOUj+BRG2bhjKFIvGgxi2cq2EGZ9H3xC1YI7+EdMEZbLJAXlIV9ARU5FamabaSv+I7" +
                "aiF6DxK5Vo9qVEWh9jKvOW01DMMW8meYFJ+ZIfJHSORqU3CU9/fRMA+nkjM0Tn5Hb4lYgHVYzAx8hxwi" +
                "/0PG08VfyDV/Tmy8Axn3dn2mMMu4AAAAAElFTkSuQmCC";

            private const String wordButtonImage =
                "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8" +
                "YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAE3SURBVDhPY2hoaKAIg4GDgwMLORisOTAwMCggIOAcORik" +
                "F2xIaGgoMzkYrBnZP0ATfYAmuyGLAfmn/fz8pJDFoOJ7goODVeECQBPZgIJ3gfghkM8EEwcaeiUoKEgG" +
                "xodhf3//I0ADNOACQFv8gIpnAA3YD2Q6wsRBBgDFnIF0H1BTCcgirAYAFa0CYgegwlQgPRfNgG1A2h5I" +
                "bwBqbMQwAGgqP1DyHihgvL29BYHsV0A2J5IBClBN5kD2KQwDgILJQPwaJAjCQPZnoMZQmAGwMADKaQHl" +
                "LmEzYD9QoQ2IDdUUDcQboWyQC9Kg7CIgez6IDTcA6FQhoOBZoCA85IFiPECxG76+vlxAehdQcReQvgDE" +
                "J4BsWahh64AGKIHTAsjv5GCwZqBJlCdlbBmFGAzWDPM3ebiBAQCkbFH+kiuaxAAAAABJRU5ErkJggg==";

            private const String backButtonImage =
                "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8" +
                "YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAADASURBVDhPrdJNDgFBEAXgidhyAgdwBiyt2QpbicQp+gy9" +
                "6b/0AgfjDByAV4lqlBI9YfElMy/1Kj0/jTHmJ2rYhhqSEMIQNrC21va1GaKGzrkxime4khjjVpsjb4FS" +
                "3iHvyDn2cnMvX7gMB+Qfy6RcYHgiykdY4gQLyXs/R6dbFqSUBqL8FRZN/7eAIJSPcMLQio8tzNB5PAKT" +
                "XwDqXyJTluyR131GhiOOnpfgvv5HYiiWXznn3NNmiBq2oYb1THMDsiPYYcqSUJgAAAAASUVORK5CYII=";

            private const String nextButtonImage =
                "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8" +
                "YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAC2SURBVDhPY2hoaKAIYxUkBWMVBOGZM2fyz5o1K3XGjBkZ" +
                "s2fP1sSmBoSxCoIw0IBsIP4PxZ+nT59ujU0dhgAMr1q1ihmocTEhQ1A46BhqyFJkQ4DYBlkNmAAKsgJx" +
                "ANDPEegYGAYxQLkHQAwz5AsQww2BGeCGpIAY/GXq1KmyVDWAFehUfyK9gBIOYAIXxhaI6DGBogEZgzQD" +
                "bV+CTzMIo3CQMVADZQkJqImypEwsxipIPG5gAACKw9ruKIradQAAAABJRU5ErkJggg==";

            private const String quitButtonImage =
                "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8" +
                "YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAACjSURBVDhPpZLdDcQgDIM7UVc5dZD2OgJs3A1aW0qi/PDQ" +
                "Ox4+AU5sELC01qYYir+QhbX3viXNQO2DcfWab2DhAjc4RfMcgDX2WIg1yM5sUCwEta/Tb39KCxBoyiEj" +
                "zTzerGSDJ5hJWDhGIcVMiiCUALmH0luEfGEJvkToDwugT6XwJOE02GDHaB6boMBPks1ay8/I3hgA5j6S" +
                "MPWV/2IovqctD9xiwijHRDg5AAAAAElFTkSuQmCC";

            #endregion

            #region Private Types

            private enum Direction { None, First, Next, Previous };

            #endregion

            #region Private Fields

            private readonly TextBox parent = null;
            private List<Int32> offsets = null;
            private Boolean dirty = false;

            private IContainer components;
            private TextBox txFind;
            private CheckBox cbCase;
            private CheckBox cbWord;
            private Button btBack;
            private Button btNext;
            private Button btQuit;

            #endregion

            #region Construction

            public FindPanel(TextBox parent)
                : base()
            {
                this.parent = parent ?? throw new ArgumentNullException(nameof(parent));

                this.InitializeComponent();
            }

            #endregion

            #region Public Properties

            public Int32 Offset { get; set; }

            #endregion

            #region Public Methods

            public void Activate()
            {
                this.Select();
                this.txFind.SelectAll();
                this.txFind.Select();
            }

            public void FindFirst(String value, Int32 offset)
            {
                if (String.IsNullOrEmpty(value))
                {
                    return;
                }

                this.Offset = offset;
                this.Value = value;

                if (!this.Visible)
                {
                    this.Show();
                }

                this.ExecuteFindOperation(Direction.First);
            }

            public void FindNext(Int32 offset)
            {
                this.Offset = offset < 0 ? 0 : offset;

                this.ExecuteFindOperation(Direction.Next);
            }

            public void FindPrev(Int32 offset)
            {
                this.Offset = offset < 0 ? 0 : offset;

                this.ExecuteFindOperation(Direction.Previous);
            }

            #endregion

            #region Protected Events

            protected override void OnVisibleChanged(EventArgs args)
            {
                base.OnVisibleChanged(args);

                if (base.Visible)
                {
                    this.AdjustPanelLocation();
                    this.txFind.Select();
                }
            }

            #endregion

            #region Protected Methods

            protected override void Dispose(Boolean disposing)
            {
                if (disposing)
                {
                    this.cbCase.Image.Dispose();
                    this.cbWord.Image.Dispose();
                    this.btBack.Image.Dispose();
                    this.btNext.Image.Dispose();
                    this.btQuit.Image.Dispose();
                    this.components.Dispose();
                }

                base.Dispose(disposing);
            }

            #endregion

            #region Private Events

            private void OnParentResize(Object sender, EventArgs args)
            {
                this.AdjustPanelLocation();
            }

            private void OnHandleControlKeyDown(Object sender, KeyEventArgs args)
            {
                switch (args.KeyCode)
                {
                    case Keys.Escape:
                        {
                            this.btQuit.PerformClick();
                            args.Handled = true;
                            return;
                        }
                    case Keys.F3:
                        {
                            switch (args.Modifiers)
                            {
                                case Keys.Shift:
                                    this.btBack.PerformClick();
                                    args.Handled = true;
                                    return;
                                case Keys.None:
                                    this.btNext.PerformClick();
                                    args.Handled = true;
                                    return;
                                default:
                                    args.Handled = false;
                                    return;
                            }
                        }
                    case Keys.Space:
                        {
                            // Enter key is already handled on buttons.

                            if (sender == this.btBack)
                            {
                                this.btBack.PerformClick();
                                args.Handled = true;
                                return;
                            }

                            if (sender == this.btNext)
                            {
                                this.btNext.PerformClick();
                                args.Handled = true;
                                return;
                            }

                            if (sender == this.btQuit)
                            {
                                this.btQuit.PerformClick();
                                args.Handled = true;
                                return;
                            }

                            args.Handled = false;
                            return;
                        }
                    case Keys.Enter:
                        {
                            // Space key is already handled on checkboxes.

                            if (sender == this.cbCase)
                            {
                                this.cbCase.Checked = !this.cbCase.Checked;

                                args.Handled = true;
                                return;
                            }

                            if (sender == this.cbWord)
                            {
                                this.cbWord.Checked = !this.cbWord.Checked;

                                args.Handled = true;
                                return;
                            }

                            args.Handled = false;
                            return;
                        }
                    default:
                        {
                            args.Handled = false;
                            return;
                        }
                }
            }

            private void OnParentTextChanged(Object sender, EventArgs args)
            {
                this.dirty = true;
            }

            private void OnFindTextChanged(Object sender, EventArgs args)
            {
                this.dirty = true;
            }

            private void OnCheckButtonCheckedChanged(Object sender, EventArgs args)
            {
                this.dirty = true;

                if (sender is CheckBox check)
                {
                    check.FlatAppearance.BorderSize = check.Checked ? 1 : 0;
                }
            }

            private void OnQuitButtonClicked(Object sender, EventArgs args)
            {
                this.Hide();
                this.parent.Select();
            }

            private void OnFindButtonClicked(Object sender, EventArgs args)
            {
                if (sender == this.btBack)
                {
                    this.ExecuteFindOperation(this.dirty ? Direction.First : Direction.Previous);
                }
                else if (sender == this.btNext)
                {
                    this.ExecuteFindOperation(this.dirty ? Direction.First : Direction.Next);
                }
            }

            #endregion

            #region Private Properties

            private Boolean MatchCase
            {
                get
                {
                    return this.cbCase.Checked;
                }
            }

            private Boolean MatchWord
            {
                get
                {
                    return this.cbWord.Checked;
                }
            }

            private String Value
            {
                get
                {
                    return this.txFind.Text;
                }
                set
                {
                    this.txFind.Text = value;
                }
            }

            private Int32 Length
            {
                get
                {
                    return this.txFind.TextLength;
                }
            }

            private String Input
            {
                get
                {
                    return this.parent.Text;
                }
            }

            private String Pattern
            {
                get
                {
                    return this.MatchWord ? String.Format("\\b{0}\\b", this.Value) : this.Value;
                }
            }

            private RegexOptions Options
            {
                get
                {
                    return (this.MatchCase ? RegexOptions.None : RegexOptions.IgnoreCase) | RegexOptions.CultureInvariant;
                }
            }

            #endregion

            #region Private Methods

            private void AdjustPanelLocation()
            {
                Size size = this.Size;
                Rectangle bounds = this.parent.ClientRectangle;

                this.Location = new Point(bounds.Right - size.Width, 0);
            }

            private void RenewFindOffsets()
            {
                MatchCollection matches = Regex.Matches(this.Input, this.Pattern, this.Options);

                this.offsets = matches.Where(x => x.Success).Select(x => x.Index).ToList();

                this.dirty = false;
            }

            private Int32 GetMatchingOffset(Int32 offset, List<Int32> offsets, Direction direction)
            {
                if (direction == Direction.Previous)
                {
                    for (Int32 index = offsets.Count - 1; index >= 0; index--)
                    {
                        if (offsets[index] > offset)
                        {
                            continue;
                        }

                        return offsets[index];
                    }

                    return offsets[offsets.Count - 1];
                }
                else
                {
                    for (Int32 index = 0; index < offsets.Count; index++)
                    {
                        if (offsets[index] < offset)
                        {
                            continue;
                        }

                        return offsets[index];
                    }

                    return offsets[0];
                }
            }

            private void ExecuteFindOperation(Direction direction)
            {
                if (this.Length < 1)
                {
                    return;
                }

                if (direction == Direction.First || this.dirty == true || this.offsets == null)
                {
                    this.RenewFindOffsets();
                }

                if (this.offsets.Count < 1)
                {
                    return;
                }

                switch (direction)
                {
                    case Direction.First:
                        {
                            this.Offset = this.GetMatchingOffset(this.Offset, this.offsets, direction);
                            break;
                        }
                    case Direction.Next:
                        {
                            Int32 shift = this.parent.SelectionLength > 0 ? 1 : 0;
                            this.Offset = this.GetMatchingOffset(this.Offset + shift, this.offsets, direction);
                            break;
                        }
                    case Direction.Previous:
                        {
                            Int32 shift = this.parent.SelectionLength > 0 ? 1 : 0;
                            this.Offset = this.GetMatchingOffset(this.Offset - shift, this.offsets, direction);
                            break;
                        }
                    default:
                        return;
                }

                this.parent.Select(this.Offset, this.Length);
                this.parent.ScrollToCaret();
                this.dirty = false;
            }

            private void InitializeComponent()
            {
                const Int32 size = 23;
                const Int32 gap = 3;
                const Int32 pad = 6;

                this.components = new Container();

                TableLayoutPanel layout = new TableLayoutPanel();
                ToolTip tooltip = new ToolTip(this.components);

                this.SuspendLayout();
                layout.SuspendLayout();

                layout.Dock = DockStyle.Fill;
                layout.ColumnCount = 6;
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                layout.RowCount = 1;
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                layout.Size = new Size(0, 0);
                layout.Location = new Point(0, 0);
                layout.Padding = new Padding(pad, pad, pad, pad);
                layout.Margin = new Padding(0, 0, 0, 0);

                this.txFind = new TextBox();
                this.txFind.Dock = DockStyle.Fill;
                this.txFind.Margin = new Padding();
                this.txFind.Location = new Point(0, 0);
                this.txFind.Size = new Size(0, size);
                this.txFind.Margin = new Padding(0, 0, 0, 0);
                this.txFind.PlaceholderText = "Find...";
                this.txFind.Font = SystemFonts.MenuFont;
                this.txFind.AutoSize = false;
                this.txFind.KeyDown += this.OnHandleControlKeyDown;
                this.txFind.TextChanged += this.OnFindTextChanged;

                this.cbCase = new CheckBox();
                this.cbCase.Cursor = Cursors.Default;
                this.cbCase.Dock = DockStyle.Fill;
                this.cbCase.Image = this.GetImage(FindPanel.caseButtonImage);
                this.cbCase.Size = new Size(size, size);
                this.cbCase.Location = new Point(0, 0);
                this.cbCase.Margin = new Padding(gap, 0, 0, 0);
                this.cbCase.FlatStyle = FlatStyle.Flat;
                this.cbCase.FlatAppearance.BorderSize = 0;
                this.cbCase.FlatAppearance.BorderColor = Color.FromArgb(unchecked((Int32)0xFFCCA870));
                this.cbCase.FlatAppearance.MouseDownBackColor = Color.FromArgb(unchecked((Int32)0xFFFFEDC8));
                this.cbCase.FlatAppearance.MouseOverBackColor = Color.FromArgb(unchecked((Int32)0xFFFFFFFF));
                this.cbCase.FlatAppearance.CheckedBackColor = Color.FromArgb(unchecked((Int32)0xFFFFEDC8));
                this.cbCase.Appearance = Appearance.Button;
                this.cbCase.KeyDown += this.OnHandleControlKeyDown;
                this.cbCase.CheckedChanged += this.OnCheckButtonCheckedChanged;

                this.cbWord = new CheckBox();
                this.cbWord.Cursor = Cursors.Default;
                this.cbWord.Dock = DockStyle.Fill;
                this.cbWord.Image = this.GetImage(FindPanel.wordButtonImage);
                this.cbWord.Size = new Size(size, size);
                this.cbWord.Location = new Point(0, 0);
                this.cbWord.Margin = new Padding(gap, 0, 0, 0);
                this.cbWord.FlatStyle = FlatStyle.Flat;
                this.cbWord.FlatAppearance.BorderSize = 0;
                this.cbWord.FlatAppearance.BorderColor = Color.FromArgb(unchecked((Int32)0xFFCCA870));
                this.cbWord.FlatAppearance.MouseDownBackColor = Color.FromArgb(unchecked((Int32)0xFFFFEDC8));
                this.cbWord.FlatAppearance.MouseOverBackColor = Color.FromArgb(unchecked((Int32)0xFFFFFFFF));
                this.cbWord.FlatAppearance.CheckedBackColor = Color.FromArgb(unchecked((Int32)0xFFFFEDC8));
                this.cbWord.Appearance = Appearance.Button;
                this.cbWord.KeyDown += this.OnHandleControlKeyDown;
                this.cbWord.CheckedChanged += this.OnCheckButtonCheckedChanged;

                this.btBack = new Button();
                this.btBack.Cursor = Cursors.Default;
                this.btBack.Dock = DockStyle.Fill;
                this.btBack.Image = this.GetImage(FindPanel.backButtonImage);
                this.btBack.Size = new Size(size, size);
                this.btBack.Location = new Point(0, 0);
                this.btBack.Margin = new Padding(gap, 0, 0, 0);
                this.btBack.FlatStyle = FlatStyle.Flat;
                this.btBack.FlatAppearance.BorderSize = 0;
                this.btBack.FlatAppearance.MouseDownBackColor = Color.FromArgb(unchecked((Int32)0xFFFFEDC8));
                this.btBack.FlatAppearance.MouseOverBackColor = Color.FromArgb(unchecked((Int32)0xFFFFFFFF));
                this.btBack.Click += this.OnFindButtonClicked;
                this.btBack.KeyDown += this.OnHandleControlKeyDown;

                this.btNext = new Button();
                this.btNext.Cursor = Cursors.Default;
                this.btNext.Dock = DockStyle.Fill;
                this.btNext.Image = this.GetImage(FindPanel.nextButtonImage);
                this.btNext.Size = new Size(size, size);
                this.btNext.Location = new Point(0, 0);
                this.btNext.Margin = new Padding(gap, 0, 0, 0);
                this.btNext.FlatStyle = FlatStyle.Flat;
                this.btNext.FlatAppearance.BorderSize = 0;
                this.btNext.FlatAppearance.MouseDownBackColor = Color.FromArgb(unchecked((Int32)0xFFFFEDC8));
                this.btNext.FlatAppearance.MouseOverBackColor = Color.FromArgb(unchecked((Int32)0xFFFFFFFF));
                this.btNext.Click += this.OnFindButtonClicked;
                this.btNext.KeyDown += this.OnHandleControlKeyDown;

                this.btQuit = new Button();
                this.btQuit.Cursor = Cursors.Default;
                this.btQuit.Dock = DockStyle.Fill;
                this.btQuit.Image = this.GetImage(FindPanel.quitButtonImage);
                this.btQuit.Location = new Point(0, 0);
                this.btQuit.Size = new Size(size, size);
                this.btQuit.Margin = new Padding(gap, 0, 0, 0);
                this.btQuit.FlatStyle = FlatStyle.Flat;
                this.btQuit.FlatAppearance.BorderSize = 0;
                this.btQuit.FlatAppearance.MouseDownBackColor = Color.FromArgb(unchecked((Int32)0xFFFFEDC8));
                this.btQuit.FlatAppearance.MouseOverBackColor = Color.FromArgb(unchecked((Int32)0xFFFFFFFF));
                this.btQuit.Click += this.OnQuitButtonClicked;
                this.btQuit.KeyDown += this.OnHandleControlKeyDown;

                tooltip.SetToolTip(this.txFind, "Search Term");
                tooltip.SetToolTip(this.cbCase, "Match Case");
                tooltip.SetToolTip(this.cbWord, "Match Whole Word");
                tooltip.SetToolTip(this.btBack, "Find Previous (SHIFT+F3)");
                tooltip.SetToolTip(this.btNext, "Find Next (F3)");
                tooltip.SetToolTip(this.btQuit, "Close (ESC)");

                layout.Controls.Add(this.txFind, 0, 0);
                layout.Controls.Add(this.cbCase, 1, 0);
                layout.Controls.Add(this.cbWord, 2, 0);
                layout.Controls.Add(this.btBack, 3, 0);
                layout.Controls.Add(this.btNext, 4, 0);
                layout.Controls.Add(this.btQuit, 5, 0);

                this.Controls.Add(layout);

                this.Visible = false;
                this.Size = new Size(280, size + (2 * pad) + 2); // Plus 2 respects the border width.
                this.BackColor = Color.FromArgb(unchecked((Int32)0xFFF5F5F5));
                this.BorderStyle = BorderStyle.FixedSingle;

                this.parent.Resize += this.OnParentResize;
                this.parent.Controls.Add(this);

                layout.ResumeLayout(false);
                layout.PerformLayout();
                this.ResumeLayout(false);
                this.PerformLayout();

                this.parent.TextChanged += this.OnParentTextChanged;
            }

            private Image GetImage(String source)
            {
                using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(source)))
                {
                    return Image.FromStream(stream);
                }
            }

            #endregion
        }

        #endregion
    }

    #endregion
}
