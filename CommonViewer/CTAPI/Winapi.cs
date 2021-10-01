using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System.Drawing;

namespace CTAPI
{

    public class Winapi
    {
        //----------------------------------------------------------------- 
        //	windows defininition 
        //----------------------------------------------------------------- 
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
            public RECT(Rectangle rect)
                : this(rect.Left, rect.Top, rect.Right, rect.Bottom)
            {
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct POINTAPI
        {
            public int x;
            public int y;
            public POINTAPI(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        };

        public const short SM_XVIRTUALSCREEN = 76;
        public const short SM_YVIRTUALSCREEN = 77;
        public const short SM_CXVIRTUALSCREEN = 78;
        public const short SM_CYVIRTUALSCREEN = 79;
        public const short SM_CXSCREEN = 0;
        public const short SM_CYSCREEN = 1;
        public const short SM_CMONITORS = 80;

        public const int PROCESS_QUERY_INFORMATION = 0x400;
        public const int PROCESS_VM_READ = 0x10;

        //  外部関数宣言
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int OpenProcess(int dwDesiredAccess, int bInheritHandle, int dwProcessId);

        [DllImport("kernel32", EntryPoint = "GetDriveTypeA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetDriveType(string nDrive);

        [DllImport("kernel32", EntryPoint = "lstrlenA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int lstrlen(string lpString);

        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetTickCount();

        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void Sleep(int dwMilliseconds);

        //Rev23.00 追加 by長野 2015/09/14
        [DllImport("kernel32", EntryPoint = "GetDiskFreeSpaceEx", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetDiskFreeSpaceEx(string lpDirectoryName, ref ulong lpFreeBytesAvailable, ref ulong lpTotalNumberOfBytes, ref ulong lpNumberOfFreeBytes);

        [DllImport("kernel32", EntryPoint = "GetDiskFreeSpaceA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetDiskFreeSpace(string lpRootPathName, ref int lpSectorsPerCluster, ref int lpBytesPerSector, ref int lpNumberOfFreeClusters, ref int lpTtoalNumberOfClusters);

        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int WinExec(string lpCmdLine, int nCmdShow);

        [DllImport("user32", EntryPoint = "FindWindowA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32", EntryPoint = "GetClassNameA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetClassName(int hwnd, string lpClassName, int nMaxCount);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetCursorPos(int x, int y);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32", EntryPoint = "SendMessageA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32", EntryPoint = "PostMessageA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int PostMessage(int hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetWindowDC(int hwnd);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ReleaseDC(int hwnd, int hdc);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetDesktopWindow();

        [DllImport("psapi", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int EnumProcesses(ref int lpdwPIDs, int dwSize2, ref int dwSize);

        [DllImport("psapi", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int EnumProcessModules(int hProcess, ref int hMod, int sizehMod, ref int dwlpdwPIDsize);

        [DllImport("psapi", EntryPoint = "GetModuleFileNameExA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int GetModuleFileNameEx(int hProcess, int hMod, string szFileName, int nSize);
        public static extern int GetModuleFileNameEx(int hProcess, int hMod, StringBuilder szFileName, int nSize);

        public struct BITMAPINFOHEADER
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
        }

        public struct RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }

        public struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;

            [VBFixedArray(255)]
            public RGBQUAD[] bmiColors;

            public static BITMAPINFO Initialize()
            {
                BITMAPINFO bitmapinfo = new BITMAPINFO();
                bitmapinfo.bmiColors = new RGBQUAD[256];
                return bitmapinfo;
            }
        }

        public struct BITMAP
        {
            public int bmType;
            public int bmWidth;
            public int bmHeight;
            public int bmWidthBytes;
            public short bmPlanes;
            public short bmBitsPixel;
            public int bmBits;
        }

        public struct PALETTEENTRY
        {
            public byte peRed;
            public byte peGreen;
            public byte peBlue;
            public byte peFlags;
        }

        public struct LOGPALETTE
        {
            public short palVersion;
            public short palNumEntries;
            [VBFixedArray(255)]
            // Enough for 256 colors.
            public PALETTEENTRY[] palPalEntry;

            public static LOGPALETTE Initialize()
            {
				LOGPALETTE logpalette = new LOGPALETTE();
				logpalette.palPalEntry = new PALETTEENTRY[256];
				return logpalette;

//                palPalEntry = new PALETTEENTRY[256];
            }
        }

        [DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int CreateCompatibleDC(int hdc);

        [DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int CreateCompatibleBitmap(int hdc, int nWidth, int nHeight);

        [DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int CreateDIBSection(int hdc, ref BITMAPINFO pBitmapInfo, int un, ref int lplpVoid, int Handle, int dw);

        [DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetDeviceCaps(int hdc, int iCapabilitiy);

        [DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetSystemPaletteEntries(int hdc, int wStartIndex, int wNumEntries, ref PALETTEENTRY lpPaletteEntries);

        [DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int CreatePalette(ref LOGPALETTE lpLogPalette);

        [DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SelectPalette(int hdc, int hPalette, int bForceBackground);

        [DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int RealizePalette(int hdc);
        
        [DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int DeleteDC(int hdc);

        [DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SelectObject(int hdc, int hObject);

        [DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int StretchBlt(int hdc, int x, int y, int nWidth, int nHeight, int hSrcDC, int xSrc, int ySrc, int nSrcWidth, int nSrcHeight, int dwRop);
        
        [DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(int hDestDC, int x, int y, int nWidth, int nHeight, int hSrcDC, int xSrc, int ySrc, int dwRop);
        
        [DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetStretchBltMode(int hdc, int nStretchMode);
            
        [DllImport("gdi32", EntryPoint = "GetObjectA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int GetObject(int hObject, int nCount, ref Any lpObject);
        public static extern int GetObject(IntPtr hObject, int nCount, ref BITMAP lpObject);					//TODO As Any
    
        [DllImport("kernel32", EntryPoint = "RtlMoveMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern void CopyMemory(ref Any Destination, ref Any Source, int length);
        //public static extern void CopyMemory(ref IntPtr Destination, ref IntPtr Source, int length);		//TODO As Any
        public static extern void CopyMemory(IntPtr Destination, IntPtr Source, int length);		//TODO As Any

        //OleCreatePictureIndirectに渡すための構造体（本来は共用体）
        public struct PicBmp
        {
            //この構造体のサイズ
            public int SIZE;
            //ピクチャーのタイプ
            public int Type;
            //ビットマップのハンドル
            public int hBmp;
            //パレットのハンドル
            public int hPal;
            public int Reserved;
        }

        //GUIDを格納するための構造体
        public struct GUID
        {
            public int Data1;
            public short Data2;
            public short Data3;
           [VBFixedArray(7)]
            public byte[] Data4;

            public static GUID Initialize()
            {
                GUID guid = new GUID();
                guid.Data4 = new byte[8];
                return guid;
            }
        }
 
        //削除2015/01/21hata_32bit用のため
        //[DllImport("olepro32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int OleCreatePictureIndirect(ref PicBmp PicDesc, ref GUID RefIID, int fPictureOwnsHandle, ref System.Drawing.Image IPic); 

        //タスク強制終了用の宣言
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int CloseHandle(int hObject);
        
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int WaitForSingleObject(int hHandle, int dwMilliseconds);
        
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetExitCodeProcess(int hProcess, ref int lpExitCode);


        //----------------------------------------
        //modDoubleObliqueで使用するもの
        //----------------------------------------
        public const short GWL_WNDPROC = -4;
      
        [DllImport("user32", EntryPoint = "GetWindowTextA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetWindowText(int hWnd, StringBuilder lpString , int cch);
        //public static extern int GetWindowText(int hWnd, string lpString, int cch);
        
        [DllImport("user32", EntryPoint = "RegisterWindowMessageA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int RegisterWindowMessage(string lpString);
        
       
        [DllImport("user32", EntryPoint = "SetWindowLongA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetWindowLong(int hWnd, int nIndex, int dwNewLong);
        
        [DllImport("user32", EntryPoint = "CallWindowProcA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int CallWindowProc(int lpPrevWndFunc, int hWnd, int Msg, int wParam, int lParam);



        //終了要求（システムによる定義）
        public const int WM_CLOSE = 0x10;

        //ウィンドウのプロセスIDとスレッドIDを取得する関数の宣言
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetWindowThreadProcessId(int hWnd, ref int lpdwProcessId);
        

        //親ハンドルを取得する関数の宣言
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetParent(int hWnd);
        

        //ウィンドウを列挙する関数の宣言
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int EnumWindows(int lpEnumFunc, int lParam);
        

        //--------------------------------------------------
        //画像処理共通.basで使用するもの　（定数）
        //--------------------------------------------------
        const int RASTERCAPS = 38;
        const int RC_PALETTE = 0x100;
        const int SIZEPALETTE = 104;

        const short BLACKONWHITE = 1;
        const short WHITEONBLACK = 2;
        const short COLORONCOLOR = 3;
        const short HALFTONE = 4;
        const short MAXSTRETCHBLTMODE = 4;


        //--------------------------------------------------
        //modLibrry.basで使用するもの
        //--------------------------------------------------
        //ウィンドウポジション   'V4.0 append by 鈴山 2001/02/14
        //'ウインドウを最後方に
        public const short HWND_BOTTOM = 1;
        //'常に最全面のウィンドウに設定
        public const short HWND_TOPMOST = -1;
        //'最上位ウィンドウの背面に
        public const short HWND_NOTOPMOST = -2;
        //'ウィンドウを一番手前に
        public const short HWND_TOP = 0;
        //'現在の位置を保つフラグ
        public const int SWP_NOMOVE = 0x2;
        //'現在のサイズを保つフラグ
        public const int SWP_NOSIZE = 0x1;
        //v15.0追加 by 間々田 2009/01/18
        public const int SWP_FRAMECHANGED = 0x20;
        //v15.0追加 by 間々田 2009/01/18
        public const int SWP_HIDEWINDOW = 0x80;
        public const int SWP_SHOWWINDOW = 0x40;

        //ウィンドウポジション   'V4.0 append by 鈴山 2001/02/14
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetWindowPos(int hWnd, int hwndInsertAfter, int x, int y, int CX, int CY, int wFlags);
 
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetWindowRect(int hWnd, ref RECT lpRect);

        // Win32APIの呼び出し宣言
        [DllImport("KERNEL32.DLL")]
        public static extern uint 
            GetPrivateProfileString(string lpAppName,
            string lpKeyName, string lpDefault,
            StringBuilder lpReturnedString, uint nSize,
            string lpFileName);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringA")]
        public static extern uint
            GetPrivateProfileStringByByteArray(string lpAppName,
            string lpKeyName, string lpDefault,
            byte[] lpReturnedString, uint nSize,
            string lpFileName);

        [DllImport("KERNEL32.DLL")]
        public static extern uint
            GetPrivateProfileInt(string lpAppName,
            string lpKeyName, int nDefault, string lpFileName);

        [DllImport("KERNEL32.DLL")]
        public static extern uint WritePrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpString,
            string lpFileName);

        //追加2014/11/28hata_v19.51_dnet        
        public const int SW_SHOWNORMAL = 1;
        public const int SW_SHOW = 5;
        public const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);


    }
}
