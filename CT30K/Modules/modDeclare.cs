using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;

namespace CT30K
{
	static class modDeclare
	{
///* ************************************************************************** */
///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
///* 客先　　　　： ?????? 殿                                                   */
///* プログラム名： modDeclare.bas                                              */
///* 処理概要　　： コモンアクセス共通モジュール                                */
///* 注意事項　　： なし                                                        */
///* -------------------------------------------------------------------------- */
///* 適用計算機　： DOS/V PC                                                    */
///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
///* コンパイラ　： VB 6.0                                                      */
///* -------------------------------------------------------------------------- */
///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
///*                                                                            */
///* V1.00       99/XX/XX    (TOSFEC) ????????                                  */
///* V2.0        00/02/08    (TOSFEC) 鈴山　修   V1.00を改造                    */
///* V3.0        00/08/01    (TOSFEC) 鈴山　修   ｺｰﾝﾋﾞｰﾑCT対応                  */
///*                                                                            */
///* -------------------------------------------------------------------------- */
///* ご注意：                                                                   */
///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
///*                                                                            */
///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
///* ************************************************************************** */

        //CTAPI.Winapiに移行
        /*
        //Virtual Desktop sizes                  'v15.0追加 by 間々田 2009/03/09	
		public const short SM_XVIRTUALSCREEN = 76;  //Virtual Left
		public const short SM_YVIRTUALSCREEN = 77;  //Virtual Top
		public const short SM_CXVIRTUALSCREEN = 78; //Virtual Width
		public const short SM_CYVIRTUALSCREEN = 79; //Virtual Height
		public const short SM_CXSCREEN = 0;
		public const short SM_CYSCREEN = 1;
		public const short SM_CMONITORS = 80;

        //指定されたファイルが見つかりません。
		const short ERROR_FILE_NOT_FOUND = 2;

        //指定されたパスが見つかりません。
		const short ERROR_PATH_NOT_FOUND = 3;
        */

        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************

		//メカ制御デバイスハンドル格納先ポインタ
		public static int hDevID1;
		
        //メカ制御ボードエラーステータス                 'V4.0 append by 鈴山 2001/02/01
		public static int mrc;


        //CTAPI.Winapiに移行
        /*
        //********************************************************************************
        //  外部関数宣言
        //********************************************************************************
        //v29.99 この関数だけ必要なのでmodMemoryStatから移動 by長野 2013/04/08'''''ここから'''''
		[DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int GetSystemMetrics(int nIndex);
        //v29.99 この関数だけ必要なのでmodMemoryStatから移動 by長野 2013/04/08'''''ここまで'''''

        //Declare Function GetDriveType Lib "kernel32" Alias "GetDriveTypeA" (ByVal nDrive As String) As Long    'v17.50削除 未使用 2011/02/28 by 間々田
		
        [DllImport("kernel32", EntryPoint = "lstrlenA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int lstrlen(string lpString);
		
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int GetTickCount();
		
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void Sleep(int dwMilliseconds);
		
        [DllImport("kernel32", EntryPoint = "GetDiskFreeSpaceA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int GetDiskFreeSpace(string lpRootPathName, ref int lpSectorsPerCluster, ref int lpBytesPerSector, ref int lpNumberOfFreeClusters, ref int lpTtoalNumberOfClusters);

        //ディスク空き容量情報取得ＡＰＩ関数の宣言
        //v9.5 added by 間々田 2004/09/21
        //Declare Function WinExec Lib "kernel32" (ByVal lpCmdLine As String, ByVal nCmdShow As Long) As Long                                                    'v17.50削除 未使用 2011/02/28 by 間々田
        //Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Long                       'v17.50削除 未使用 2011/02/28 by 間々田
        //Declare Function GetClassName Lib "user32" Alias "GetClassNameA" (ByVal hWnd As Long, ByVal lpClassName As String, ByVal nMaxCount As Long) As Long    'v17.50削除 未使用 2011/02/28 by 間々田
        */

 
        //[DllImport("kernel32", EntryPoint = "GetPrivateProfileStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//public static extern int GetPrivateProfileString(string lpApplicationName, Any lpKeyName, string lpDefault, string lpReturnedString, int nSize, string lpFileName);        //v10.01 added by 間々田 2005/03/11
		
        [DllImport("kernel32", EntryPoint = "GetPrivateProfileIntA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int GetPrivateProfileInt(string lpApplicationName, string lpKeyName, int nDefault, string lpFileName);//v17.50追加 2011/01/20 by 間々田

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public extern static uint GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public extern static uint WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);

        [DllImport("psapi", EntryPoint = "GetModuleFileNameExA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetModuleFileNameEx(int hProcess, int hMod, string szFileName, int nSize);

        /*
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetCursorPos(int x, int y);

        //
        //   Conereconlib.dll    v7.0 added by 間々田 2003/09/24
        //
        //                       v11.2 full_distortion,ist,iedを追加 by 間々田 2005-10-06
        //
		[DllImport("Conereconlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void cone_setup_iicrct(ref float ic, ref float jc, ref float theta_s, ref float dpm, ref int nstart, ref int nend, ref int mstart, ref int mend, ref float delta_theta, ref float n0,
		ref float m0, ref float delta_im, ref float delta_jm, ref float ir0, ref float jr0, ref float kix, ref float kjx, float b, float scan_posi_a, float scan_posi_b,
		int h_size, int v_size, float FDD, int nd, int md, int mc, ref float hizumi, ref int js, ref int je, ref float delta_ix,
		ref float delta_jx, float hm, float vm, int pure_flag, int full_distortion, int ist, int ied, int detector);
		//v17.00 引数追加 byやまおか 2010/02/26

        //
        //   condlib.dll
        //
        //Public Declare Function d_code Lib "condlib.dll" () As Long     '追加 by SUZU '97-07-18    'v11.2削除 by 間々田 2005/11/28 マイクロＣＴは無関係
        //Public Declare Function e_code Lib "condlib.dll" () As Long                                'v11.2削除 by 間々田 2005/11/28 マイクロＣＴは無関係
        //Public Declare Function f_code Lib "condlib.dll" () As Long                                'v11.2削除 by 間々田 2005/11/28 マイクロＣＴは無関係
        //Public Declare Function c_scan Lib "condlib.dll" (ByVal Mode As Long) As Long   'condlib.dllをインストーラに含めるため追加 'v11.5削除 by 間々田 2006/07/03 マイクロＣＴは無関係

        //
        //   RevTools.dll
        //
		[DllImport("RevTools.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int ReversByte(string InImgName, string OutImgName);
		
 		//added by 稲葉 98-5-21
        //Public Declare Function GetImgSize Lib "RevTools.dll" (ByVal IMGName As String) As Long                                 'added by 稲葉 98-5-21 'v11.3削除 by 間々田 2006/02/20

        //v9.1 メール送信 by 間々田 2004/05/13
        //Public Declare Function SendMail Lib "bsmtp" _ 'changed by makifuchi 2004-06-16
        //      (szServer As String, szTo As String, szFrom As String, _
        //'      szSubject As String, szBody As String, szFile As String) As String

        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //Public Declare Function SendMail Lib "bsmtp.dll" _
        //'      (szServer As String, szTo As String, szFrom As String, _
        //'      szSubject As String, szBody As String, szFile As String) As String
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

        //v9.7追加ここから by 間々田 2004/12/09
        [DllImport("psapi", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int EnumProcesses(ref int lpdwPIDs, int dwSize2, ref int dwSize);
		
        [DllImport("psapi", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int EnumProcessModules(int hProcess, ref int hMod, int sizehMod, ref int dwlpdwPIDsize);
		
        [DllImport("psapi", EntryPoint = "GetModuleFileNameExA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetModuleFileNameEx(int hProcess, int hMod, string szFileName, int nSize);

		public const int PROCESS_QUERY_INFORMATION = 0x400;
		public const int PROCESS_VM_READ = 0x10;
		
        //Public Const WM_CLOSE = &H10'v17.50 Win32api.bas に移動 by 間々田 2011/02/28

        //v17.50 Win32api.bas に移動 by 間々田 2011/02/28
        //Public Declare Function SendMessage Lib "user32" Alias _
        //'"SendMessageA" (ByVal hWnd As Long, ByVal wMsg As Long, _
        //'               ByVal wParam As Long, _
        //'               ByVal lParam As Long) As Long

        //Declare Function PostMessage Lib "user32" Alias "PostMessageA" (ByVal hWnd As Long, ByVal wMsg As Long, ByVal wParam As Long, ByVal lParam As Long) As Long 'v17.50削除 2011/02/28 by 間々田
        //v9.7追加ここまで by 間々田 2004/12/09

        //v11.5追加 by 間々田 2006/06/15
        [DllImport("Reconlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int sig_chk();

        //v11.5移動 by 間々田 2006/06/15 modSyslib.basより移動
		[DllImport("syslib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int info_rdwt(int flag, ref ImageInfoStruct theInf, string DirName, string FileName);
	    */

    }
}
