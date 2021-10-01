using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
//
using CTAPI;
using CT30K.Common;

namespace CT30K
{
    //このダイアログは使わない（FolderBrowserDialogクラスを使用する)



	public class CTFolderBrowserDialog
	{
        // SHBrowseForFolder 関数
        [DllImport("SHELL32.DLL", EntryPoint = "SHBrowseForFolder", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern int SHBrowseForFolder(ref TypeBrowseInfo lpBrowseInfo);

        // SHGetPathFromIDList 関数
        [DllImport("SHELL32.DLL", EntryPoint = "SHGetPathFromIDList", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = false)]
        private static extern int SHGetPathFromIDList(int pidl, string pszPath);

        // CoTaskMemFree 関数
        [DllImport("OLE32.DLL", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern void CoTaskMemFree(int pv_Renamed);

        // BrowseInfo 構造体
		private struct TypeBrowseInfo
		{
			public int OwnerHandle;
			public int Root;
			public string DisplayName;
			public string Description;
			public BifOptions Flags;
			public int lpfn;
			public string lParam;
			public int iImage;
		}

        // FolderBrowseDialog 設定用の列挙体
		private enum BifOptions
		{
            ReturnOnlyFileSystemDirectories = 0x1,  // コントロールパネル・プリンタ・ブリーフケース内は選択不可
            HideNetworkResource = 0x2,              // ネットワーク内のリソースを非表示
            StatusText = 0x4,                       // テキスト文字列を表示 (設定は Callback 関数で行う)
            OnlyNetworkResource = 0x8,              // ネットワーク内のリソースのみ選択可能
            ShowEditBox = 0x10,                     // フォルダ名を編集する TextBox を表示
            Validate = 0x20,                        // 検証を実行する
            NewDialogStyle = 0x40,                  // 新しいフォルダの作成を表示 (Winodws 2000 以降から有効)
            BrowseForComputer = 0x1000,             // ネットワークコンピュータ内のリソースのみ選択可
            BrowseForPrinter = 0x2000,              // ネットワークプリンタのみ選択可
            BrowseIncludeFiles = 0x4000             // フォルダ内のファイル名も表示 (Windows 98 以降)
		}

        // プロパティ 変数
		private string m_SelectedPath;
		private TypeBrowseInfo BrowseInfo;

        /// <summary>
        /// NULL終端文字
        /// </summary>
        private const char vbNullChar = '\0';


        //*******************************************************************************
        //機　　能： SelectedPath プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.0  2008/10/03  (SS1)間々田  新規作成
        //*******************************************************************************
		public string SelectedPath
        {
			get
            {
                return m_SelectedPath;
            }
			set
            {
                m_SelectedPath = value;
            }
		}

        //*******************************************************************************
        //機　　能： Description プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.0  2008/10/03  (SS1)間々田  新規作成
        //*******************************************************************************
		public string Description
        {
			get
            {
                return BrowseInfo.Description;
            }
			set
            {
                BrowseInfo.Description = value;
            }
		}

        //*******************************************************************************
        //機　　能： ShowNewFolderButton プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.0  2008/10/03  (SS1)間々田  新規作成
        //*******************************************************************************
		public bool ShowNewFolderButton
        {
			get
            {
                if ((BrowseInfo.Flags & BifOptions.NewDialogStyle) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
			set
            {
				if (value)
                {
					BrowseInfo.Flags = (BrowseInfo.Flags | BifOptions.NewDialogStyle);
				}
                else
                {
					BrowseInfo.Flags = (BrowseInfo.Flags & ~BifOptions.NewDialogStyle);
				}
			}
		}

        //このダイアログは使わない（FolderBrowserDialogクラスを使用する)
        ////*******************************************************************************
        ////機　　能： 「フォルダの参照」ダイアログを表示する
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： v15.0  2008/10/03  (SS1)間々田  新規作成
        ////*******************************************************************************
        //public bool ShowDialog(long hOwnerHandle = 0)
        //{
        //    bool functionReturnValue = false;

        //    int lReturn = 0;

        //    //戻り値初期化
        //    functionReturnValue = false;

        //    //親ハンドル
        //    BrowseInfo.OwnerHandle = (int)hOwnerHandle;
        //    BrowseInfo.lpfn = FARPROC(modFileIO.BrowseCallbackProc);    //コールバックに使用するメソッド
        //    BrowseInfo.lParam = m_SelectedPath;                         //初期フォルダのパス名

        //    //「フォルダの参照」ダイアログを呼び出す
        //    lReturn = SHBrowseForFolder(ref BrowseInfo);

        //    // OK が押下された場合
        //    string stPath = null;
        //    if (lReturn != 0)
        //    {
        //        stPath = new string(vbNullChar, 65536);

        //        SHGetPathFromIDList(lReturn, stPath);
        //        CoTaskMemFree(lReturn);

        //        m_SelectedPath = stPath.Substring(0, stPath.IndexOf(vbNullChar));

        //        functionReturnValue = true;
        //    }

        //    return functionReturnValue;
        //}

		private long FARPROC(long pfn)
		{
			// AddressOfは標準モジュールのプロシージャを指定しなければならないので、
			// ダミーのプロシージャを実装する。
			return pfn;
		}
	}
}
