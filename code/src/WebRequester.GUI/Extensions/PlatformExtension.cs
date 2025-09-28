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

using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Plexdata.WebRequester.GUI.Extensions
{
    internal static class PlatformExtension
    {
        public static Boolean IsVistaOrHigher(this Object _)
        {
            OSVERSIONINFO version = new OSVERSIONINFO();
            version.size = Marshal.SizeOf(typeof(OSVERSIONINFO));
            if (!PlatformExtension.GetVersionEx(ref version))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            // A system is running under Vista or higher as soon 
            // as the major version is greater or equal 6.
            return version.major >= 6;
        }

        #region Win32 Helpers

        [StructLayout(LayoutKind.Sequential)]
        private struct OSVERSIONINFO
        {
            public Int32 size;
            public Int32 major;
            public Int32 minor;
            public Int32 build;
            public Int32 platform;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public String servicepack;
        }

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern Boolean GetVersionEx([In, Out] ref OSVERSIONINFO version);

        #endregion 
    }
}
