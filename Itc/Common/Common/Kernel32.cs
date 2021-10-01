using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;   //for DllImport
using Microsoft.Win32.SafeHandles;

namespace Itc.Common.WinApiImport
{
    /* 
     * kernel32.dll のP/Invoke群
     * 必要なAPIを順次追加していく
     * ※Handleは.Net2.0からSafeHandleでラップすることを推奨している。
     */
    public static class Kernel32
    {
        const string DllPath = "kernel32.dll";

        #region WaitResult
        
        //↓どちらか

        public enum WaitResult : uint
        {
            Singnaled = 0x0,
            Abandoned = 0x80,
            Timeout = 0x102,
            Failed = 0xFFFFFFFF,
        }

        //↑どちらか↓

        public const uint WAIT_OBJECT_0 = 0x0;
        public const uint WAIT_ABANDONED = 0x80;
        public const uint WAIT_TIMEOUT = 0x102;
        public const uint WAIT_FAILED = 0xFFFFFFFF;

        //↑どちらか

        #endregion

        [DllImport(DllPath)]
        public static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

        //[DllImport("kernel32.dll")]
        //public static extern SafeWaitHandle CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

        [DllImport(DllPath, SetLastError = true, ExactSpelling = true)]
        public static extern Int32 WaitForSingleObject(IntPtr handle, Int32 miliseconds);

        [DllImport(DllPath, SetLastError = true, ExactSpelling = true)]
        //public static extern Int32 WaitForSingleObject(SafeWaitHandle handle, Int32 miliseconds);
        public static extern WaitResult WaitForSingleObject(SafeWaitHandle handle, Int32 miliseconds);

        [DllImport(DllPath)]
        public static extern Int32 CloseHandle(IntPtr handle);

        [DllImport(DllPath)]
        public static extern void CopyMemory(IntPtr dst, IntPtr src, int size);

        [DllImport(DllPath)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport(DllPath)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport(DllPath)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport(DllPath)]
        public static extern bool AttachConsole(uint dwProcessId);

        [DllImport(DllPath)]
        public static extern bool FreeConsole();
    }
}
