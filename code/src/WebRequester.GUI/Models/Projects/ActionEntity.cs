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

using Newtonsoft.Json;
using Plexdata.WebRequester.GUI.Definitions;
using System.Diagnostics;

namespace Plexdata.WebRequester.GUI.Models.Projects;

[DebuggerDisplay("{GetDebuggerDisplay(),nq}")]
internal class ActionEntity : LabeledEntity, ICloneable
{
    #region Private Fields

    private Boolean apply;
    private String label;
    private String value;

    #endregion

    #region Construction

    public ActionEntity()
        : this(false, null, null, null)
    {
    }

    public ActionEntity(Boolean apply, String label, String value)
        : this(apply, label, value, null)
    {
    }

    public ActionEntity(Boolean apply, String label, String value, String notes)
        : base("Unnamed Action")
    {
        this.Apply = apply;
        this.Label = label;
        this.Value = value;
        base.Notes = notes;
    }

    private ActionEntity(ActionEntity other)
        : this(other.Apply, other.Label, other.Value, other.Notes)
    {
    }

    #endregion

    #region Public Properties

    [JsonProperty(Order = 0)]
    public Boolean Apply
    {
        get
        {
            return this.apply;
        }
        set
        {
            this.apply = value;

            base.RaisePropertyChanged();
        }
    }

    [JsonProperty(Order = 1)]
    public override String Label
    {
        get
        {
            return this.label;
        }
        set
        {
            this.label = value ?? String.Empty;

            base.RaisePropertyChanged();
        }
    }

    [JsonProperty(Order = 2)]
    public String Value
    {
        get
        {
            return this.value;
        }
        set
        {
            this.value = value ?? String.Empty;

            base.RaisePropertyChanged();
        }
    }

    [JsonProperty(Order = 3)]
    public override String Notes
    {
        get
        {
            return base.Notes;
        }
        set
        {
            base.Notes = value;
        }
    }

    #endregion

    #region Public Methods

    public static ActionEntity Parse(String value)
    {
        ActionEntity result = new();

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

        return result; ;
    }

    public Object Clone()
    {
        return new ActionEntity(this);
    }

    #endregion

    #region Debugging Helper

    protected override String GetDebuggerDisplay()
    {
        return $"{nameof(this.Apply)}: {this.Apply}, {nameof(this.Label)}: '{this.Label}', {nameof(this.Value)}: '{this.Value}'";
    }

    #endregion
}
