using System;

using System.Runtime.InteropServices;   //for DllImport

namespace Itc.Common.WinApiImport
{
    public static class User32
    {
        const string DllPath = "user32.dll";

        public const uint SC_CLOSE = 0;

        [DllImport(DllPath, SetLastError=true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport(DllPath, CharSet=CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport(DllPath, CharSet = CharSet.Auto)]
        public static extern IntPtr PostMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport(DllPath)]
        [return:MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport(DllPath)]
        private static extern int ShowWindow(IntPtr hWnd, uint Msg);

        public static void ShowWindow(IntPtr hWnd)
        {
            const int SW_RESTORE = 0x09;

            ShowWindow(hWnd, SW_RESTORE);
        }

        [DllImport(DllPath)]
        public static extern bool IsIconic(IntPtr hWnd);
    }
}
