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

using Plexdata.WebRequester.GUI.Interfaces;
using Plexdata.WebRequester.GUI.Models.Settings;

namespace Plexdata.WebRequester.GUI.Helpers
{
    internal class SettingsManager : ISettings<ApplicationSettings>, ISettingsManager
    {
        #region Public Events

        public event EventHandler SaveSettings;

        #endregion

        #region Private Fields

        private readonly ISerializer serializer = null;

        #endregion

        #region Construction

        public SettingsManager(ISerializer serializer)
        {
            this.Value = new ApplicationSettings();
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        #endregion

        #region Public Properties

        public ApplicationSettings Value { get; private set; }

        #endregion

        #region Public Methods

        public void ForceSaveSettings()
        {
            this.SaveSettings?.Invoke(this, EventArgs.Empty);
        }

        public void Load(String filename)
        {
            if (String.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            this.serializer.LoadFile<ApplicationSettings>(filename, out ApplicationSettings value);

            this.Value = value ?? new ApplicationSettings();
        }

        public void Save(String filename)
        {
            if (String.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            this.SaveSettings?.Invoke(this, EventArgs.Empty);

            this.serializer.SaveFile(filename, this.Value);
        }

        #endregion
    }
}
