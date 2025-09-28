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

using Plexdata.WebRequester.GUI.Definitions;
using System.Drawing;

namespace Plexdata.WebRequester.GUI.Extensions
{
    internal static class LocationExtension
    {
        private const String company = "Plexdata";
        private const String product = "WebRequester";
        private const String filename = $"{LocationExtension.company}.{LocationExtension.product}";

        public static String GetCompany(this Object _)
        {
            return LocationExtension.company;
        }

        public static String GetProduct(this Object _)
        {
            return LocationExtension.product;
        }

        public static String GetSettingsFilename(this Object value)
        {
            return Path.Combine(value.GetLocation(LocationType.Settings), $"{LocationExtension.filename}.settings");
        }

        public static String GetBackupFilename(this Object value)
        {
            return Path.Combine(value.GetLocation(LocationType.Backup), $"{LocationExtension.filename}.Backup.{DateTime.UtcNow:yyyyMMddHHmmss}.bak");
        }

        public static String GetExportFilename(this Object value)
        {
            return $"{LocationExtension.filename}.Export.{DateTime.UtcNow:yyyyMMddHHmmss}.json";
        }

        public static String GetSaveFilename(this Object value)
        {
            return $"{LocationExtension.filename}.Projects.{DateTime.UtcNow:yyyyMMddHHmmss}.wrp";
        }

        public static String GetLocation(this Object _, LocationType location)
        {
            if (!Enum.IsDefined(typeof(LocationType), location))
            {
                throw new ArgumentOutOfRangeException(nameof(location), $"Location '{location}' is undefined.");
            }

            switch (location)
            {
                case LocationType.Settings:
                    return LocationExtension.GetFolderPathAndEnsureDirectory(Environment.SpecialFolder.LocalApplicationData);
                case LocationType.Open:
                case LocationType.Save:
                    return LocationExtension.GetFolderPathAndEnsureDirectory(Environment.SpecialFolder.MyDocuments);
                case LocationType.Import:
                case LocationType.Export:
                    return LocationExtension.GetFolderPathAndEnsureDirectory(Environment.SpecialFolder.MyDocuments);
                case LocationType.Backup:
                    return LocationExtension.GetFolderPathAndEnsureDirectory(Environment.SpecialFolder.MyDocuments);
            }

            throw new NotSupportedException("Unreachable and should never be seen.");
        }

        private static String GetFolderPathAndEnsureDirectory(Environment.SpecialFolder folder)
        {
            String directory = Path.Combine(Environment.GetFolderPath(folder), LocationExtension.company, LocationExtension.product);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }
    }
}
