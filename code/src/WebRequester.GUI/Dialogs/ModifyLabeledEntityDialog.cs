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

using Plexdata.WebRequester.GUI.Extensions;
using Plexdata.WebRequester.GUI.Interfaces;
using Plexdata.WebRequester.GUI.Models.Projects;

namespace Plexdata.WebRequester.GUI.Dialogs
{
    internal partial class ModifyLabeledEntityDialog : Form, IModifyLabeledEntity
    {
        public ModifyLabeledEntityDialog(LabeledEntity entity, Boolean create)
            : base()
        {
            this.Entity = entity ?? throw new ArgumentNullException(nameof(entity));

            this.InitializeComponent();

            this.Text = create ? "Create" : "Modify";
            this.txLabel.Text = this.Entity.Label;
            this.txNotes.Text = this.Entity.Notes;
        }

        public LabeledEntity Entity { get; }

        protected override void OnFormClosing(FormClosingEventArgs args)
        {
            if (base.DialogResult == DialogResult.OK)
            {
                args.Cancel = String.IsNullOrWhiteSpace(this.txLabel.Text);

                if (args.Cancel)
                {
                    this.ShowWarning("The label is required and cannot be left blank.");
                }
            }

            base.OnFormClosing(args);
        }

        protected override void OnFormClosed(FormClosedEventArgs args)
        {
            if (base.DialogResult == DialogResult.OK)
            {
                this.Entity.Label = this.txLabel.Text;
                this.Entity.Notes = this.txNotes.Text;
            }

            base.OnFormClosed(args);
        }
    }
}
