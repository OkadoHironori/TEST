using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text;
using System.IO;
//
using CTAPI;
//using CT30K.Common;

namespace CT30K
{
	internal static class modDoubleOblique
	{
        // メッセージ処理関数用デリゲート
        private delegate int WindowProc(int hwnd, int msg, int wParam, int lParam);

        //WinApiを使う
        //[DllImport("user32", EntryPoint = "GetWindowTextA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int GetWindowText(int hWnd, StringBuilder lpString, int cch);
        
        //[DllImport("user32", EntryPoint = "RegisterWindowMessageA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int RegisterWindowMessage(string lpString);

        //[DllImport("user32", EntryPoint = "PostMessageA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int PostMessage(int hWnd, int wMsg, int wParam, int lParam);

        //[DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int GetTickCount();
		
        //[DllImport("user32", EntryPoint = "CallWindowProcA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int CallWindowProc(int lpPrevWndFunc, int hWnd, int Msg, int wParam, int lParam);

        //[DllImport("user32", EntryPoint = "SetWindowLongA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //extern static int SetWindowLong(int hwnd, int nIndex, int dwNewLong);
     
        //[DllImport("user32", EntryPoint = "SetWindowLongA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //private extern static int SetWindowLong(int hwnd, int nIndex, WindowProc dwNewLong);


        //ウィンドウを列挙する関数の宣言
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);

        // EnumWindowsから呼び出されるコールバック関数WNDENUMPROCのデリゲート
        //private delegate bool WNDENUMPROC(IntPtr hWnd, IntPtr lParam);
        private delegate bool WNDENUMPROC(int hWnd, int lParam);

        //ダブルオブリークウィンドウ情報
		private static string DouleObliqueProject;			//ウィンドウタイトルに表示されている処理中のプロジェクト名
		private static int DouleObliqueHandle;				//ウィンドウハンドル

        //ダブルオブリーク用ウィンドウメッセージID
		public static int WM_DOUBLEOBLIQUE;
        
        //ダブルオブリークに送信する要求メッセージの定義
        //Private Const WM_CLOSE      As Long = &H10          '終了要求（システムによる定義）'v17.50 Win32api.bas に移動 by 間々田 2011/02/28
		private const int WM_DOUBLEOBLIQUE_CLEAR = 0;		//読込みＣＴ画像クリア要求
		private const int WM_DOUBLEOBLIQUE_IMPORT = 1;		//ＣＴ画像のインポート要求
		private const int WM_DOUBLEOBLIQUE_MINIMIZE = 2;	//ダブルオブリーク最小化要求
		private const int WM_DOUBLEOBLIQUE_CLOSE = 3;		//ダブルオブリーク終了要求       'v15.0追加 by 間々田 2009/05/11

        //ダブルオブリーク実行ファイル
		public static string DoubleObliquePath ="";				//ct20k.iniから設定する

        //ダブルオブリークリターンコード
		//private enum DoubleObliqueResultType
        private enum DoubleObliqueResultType
		{
			DORESULT_UNKNOWN = -1,
			DORESULT_CANCEL = 0,
			DORESULT_OK = 1
		}
        private static DoubleObliqueResultType DOResult;

        private const short GWL_WNDPROC = -4;
        private const int WM_CLOSE = 16;
        //private static int OldWndProc;    //使っていないので削除2015/02/02hata


        //*************************************************************************************************
		//機　　能： ダブルオブリークからのメッセージを制御する
		//
		//           変数名          [I/O] 型            内容
		//引　　数：
		//戻 り 値：
		//
		//補　　足： なし
		//
		//履　　歴： v13.0 2007/04/04 (WEB)間々田    新規作成
		//*************************************************************************************************
        //変更2014/09/11(検S1)hata_CTMenuでメッセージ受ける。ここは実施。
        //public static int WndProc(int hwindow, int Message, int wParam, int lParam)
        //{
        //    //戻り値初期化
        //    int result = 0;
        //
        //    //メッセージ処理
        //    if (Message == WM_DOUBLEOBLIQUE)
        //    {
        //        switch (wParam)
        //        {
        //            case 0:
        //                DOResult = DoubleObliqueResultType.DORESULT_CANCEL;
        //                break;
        //            case 1:
        //                DOResult = DoubleObliqueResultType.DORESULT_OK;
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        //デフォルトメッセージ処理
        //        result = Winapi.CallWindowProc(OldWndProc, hwindow, Message, wParam, lParam);
        //    }
        //
        //    return result;
        //}
        public static int DoubleObliqueWndProc(ref Message m)
        {
            //戻り値初期化
            int result = 0;

            //メッセージ処理
            if (m.Msg == WM_DOUBLEOBLIQUE)
            {
                switch ((int)m.WParam)
                {
                    case 0:
                        DOResult = DoubleObliqueResultType.DORESULT_CANCEL;
                        break;
                    case 1:
                        DOResult = DoubleObliqueResultType.DORESULT_OK;
                        break;
                }
            }
            return result;
        }


		//*************************************************************************************************
		//機　　能： ダブルオブリークの起動
		//
		//           変数名          [I/O] 型            内容
		//引　　数： WindowStyle     [I/ ] VbAppWinStyle 起動時のウィンドウ状態（デフォルトは最小化で起動）
		//戻 り 値：                 [ /O] Boolean       True: 起動成功，False: 起動失敗
		//
		//補　　足： なし
		//
		//履　　歴： v13.0 2007/03/19 (WEB)間々田    新規作成
		//*************************************************************************************************
		//public static bool StartDoubleOblique(AppWinStyle WindowStyle = AppWinStyle.MinimizedNoFocus)
		public static bool StartDoubleOblique(ProcessWindowStyle WindowStyle = ProcessWindowStyle.Minimized)
		{
			//ダブルオブリーク・ウィンドウハンドルを取得時のタイムアウト時間(ms)
			int WaitTime = 5000;

			//戻り値初期化
			bool result = false;

			//ダブルオブリーク用ウィンドウメッセージID 登録
            WM_DOUBLEOBLIQUE = Winapi.RegisterWindowMessage("WM_DOUBLEOBLIQUE");
            
			try
			{
				//const int SW_SHOWMINIMIZED = 2;

				//ダブルオブリークのプロセスが存在しない場合のみ起動する
                if (!modCT30K.IsAlive(CTSettings.iniValue.DoubleObliquePath))
				{
					//ダブルオブリークの起動
					ProcessStartInfo psInfo = new ProcessStartInfo();
                    psInfo.FileName = CTSettings.iniValue.DoubleObliquePath;

					psInfo.UseShellExecute = false;
					psInfo.WindowStyle = WindowStyle;
					Process ProcDoubleOblique = Process.Start(psInfo);
                    
					//引数にvbMaximizedFocusが指定されている場合，
					//ダブルオブリーク側に画像読み込み要求をする処理が後に続くが
					//ダブルオブリークの起動直後は要求が無視されてしまう場合があるので
					//ここでウエイトをかける
					if (WindowStyle == ProcessWindowStyle.Maximized)
					{
						int WaitTime2 = 1000;
						TimeOut(ref WaitTime2);
					}
				}
        
				//ダブルオブリーク・ウィンドウハンドルを取得
                bool bNoStart = false;  //追加2015/01/30hata
                while (!GetDoubleObliqueWindow())
				{
					//タイムアウトエラーを監視
					if (TimeOut(ref WaitTime))
					{
                        bNoStart = true;

                        //変更2015/01/30hata_下に移動
                        ////メッセージ表示：
                        ////   ダブルオブリークを起動できませんでした。
                        ////   ダブルオブリークプログラムに問題があります。
                        //string s = StringTable.GetResString(9935, StringTable.GetResString(StringTable.IDS_DoubleOblique)) +
                        //                                    "\r\n" + "\r\n" +
                        //                                    StringTable.GetResString(17483);

                        //MessageBox.Show(s, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //return result;
					}
				}

                //追加2015/01/30hata
                //見つからない場合はプロセス名で探す
                if (bNoStart)
                {
                    string strFilePath =　CTSettings.iniValue.DoubleObliquePath;
                    string strFileName = Path.GetFileNameWithoutExtension(strFilePath);

                    foreach (Process p in Process.GetProcesses())
                    {
                        if (p.ProcessName == strFileName)
                        {
                            //if (p.MainModule.FileName == strFilePath)
                            //{
                                bNoStart = false;
                                break;
                            //}
                        }
                    }
                }

                //追加2015/01/30hata
                //タイムアウトエラーを監視
                if (bNoStart)
                {
                    //メッセージ表示：
                    //   ダブルオブリークを起動できませんでした。
                    //   ダブルオブリークプログラムに問題があります。
                    string s = StringTable.GetResString(9935, StringTable.GetResString(StringTable.IDS_DoubleOblique)) +
                                                        "\r\n" + "\r\n" +
                                                        StringTable.GetResString(17483);

                    MessageBox.Show(s, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return result;
                }


                //最小化が指定されている場合：ダブルオブリーク側を最小化する 'v15.0追加 by 間々田 2009/01/19
				if (WindowStyle == ProcessWindowStyle.Minimized)
				{
                    Winapi.PostMessage(DouleObliqueHandle, WM_DOUBLEOBLIQUE, WM_DOUBLEOBLIQUE_MINIMIZE, 0);
                }

				//'起動に成功したら２ndモニターに移動     'v15.0追加 by 間々田 2009/01/19    'v15.0削除 by 間々田 2009/01/19 ダブルオブリーク側で対応することにした
				//Dim sts As Long
				//Dim flag As Long
				//flag = IIf(IsExistForm(frmCTMenu), SWP_FRAMECHANGED, SWP_HIDEWINDOW)
				//If GetSystemMetrics(SM_XVIRTUALSCREEN) < 0 Then
				//    sts = SetWindowPos(DouleObliqueHandle, HWND_BOTTOM, -1608, 0, 1608, 1178, flag)
				//Else
				//    sts = SetWindowPos(DouleObliqueHandle, HWND_BOTTOM, GetSystemMetrics(SM_CXSCREEN), 0, 1608, 1178, flag)
				//End If

				//戻り値：起動成功
				result = true;
			}
			catch (Exception ex)
			{
				// //メッセージ表示：ダブルオブリークを起動できませんでした。
				string s = StringTable.GetResString(9935, StringTable.GetResString(StringTable.IDS_DoubleOblique)) +
													"\r\n" + "\r\n" +
                                                    ex.Message + "(" + CTSettings.iniValue.DoubleObliquePath + ")";

				MessageBox.Show(s, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return result;
		}

		//*************************************************************************************************
		//機　　能： ダブルオブリークの終了
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v13.0 2007/03/19 (WEB)間々田    新規作成
		//*************************************************************************************************
		public static void EndDoubleOblique()
		{
			//ダブルオブリーク・ウィンドウハンドルを取得時のタイムアウト時間(ms)
			//int WaitTime = 5000;

			//ダブルオブリークのウィンドウハンドルを取得
			if (GetDoubleObliqueWindow())
			{
				//ダブルオブリークに終了命令を送る
				//PostMessage DouleObliqueHandle, WM_CLOSE, 0, 0
                Winapi.PostMessage(DouleObliqueHandle, Winapi.WM_CLOSE, WM_DOUBLEOBLIQUE_CLOSE, 0);
                
				//v15.0削除 by 間々田 ダブルオブリークが終了したかどうかみないことにした 2009/05/11
				//'ダブルオブリーク・ウィンドウハンドルを取得
				//Do While GetDoubleObliqueWindow()
				//    'タイムアウトエラーを監視
				//    If TimeOut(WaitTime) Then
				//        'メッセージ表示：ダブルオブリークを終了できませんでした。
				//        MsgBox GetResString(9934, LoadResString(IDS_DoubleOblique)), vbCritical
				//        Exit Do
				//    End If
				//Loop
			}
		}

		//*************************************************************************************************
		//機　　能： ウインドウのサーチ関数
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Handle          [I/ ] Long      ウィンドウハンドル
		//           Palam           [I/ ] Long      ウィンドウパラメータ
		//戻 り 値：                 [ /O] Boolean
		//
		//補　　足： EnumWindows のコールバック関数
		//
		//履　　歴： v13.0 2007/03/19 (WEB)間々田    新規作成
		//*************************************************************************************************
		public static bool SearchWindow(int Handle, int Palam)
		{
			//ダブルオブリーク
			string DouleObliqueTitle = StringTable.GetResString(StringTable.IDS_DoubleOblique);

			//バッファ確保
			StringBuilder sb = new StringBuilder(255);
            
			//名前を取得する
            int ret = Winapi.GetWindowText(Handle, sb, sb.Capacity);

            if (DouleObliqueTitle.Length <= sb.Length )
            {
                string name =null;   
                name = sb.ToString();
 
			    //取得したウィンドウタイトルの最初の文字が「ダブルオブリーク」ならば，
			    //ダブルオブリークのウィンドウとみなす
                if (name.StartsWith(DouleObliqueTitle))
			    {
				    //ウィンドウハンドルを保持
				    DouleObliqueHandle = Handle;
                    DouleObliqueProject = "";
                    if (DouleObliqueTitle.Length + 4 <= name.Length)
                    {
                        //ダブルオブリークのプロジェクト名を保持
                        DouleObliqueProject = modLibrary.RemoveNull(name.Substring(DouleObliqueTitle.Length + 4));
                    }
                }
            
            }

			//次にいく
			return true;
		}

		//*************************************************************************************************
		//機　　能： ダブルオブリークのウィンドウを取得
		//
		//           変数名          [I/O] 型            内容
		//引　　数： なし
		//戻 り 値：                 [ /O] Boolean       True: 取得成功，False: 取得失敗
		//
		//補　　足： なし
		//
		//履　　歴： v13.0 2007/03/19 (WEB)間々田    新規作成
		//*************************************************************************************************
		private static bool GetDoubleObliqueWindow()
		{
			DouleObliqueHandle = 0;
			DouleObliqueProject = "";
			
            //int ret = EnumWindows(SearchWindow, 0);
            EnumWindows(SearchWindow, 0);

			return (DouleObliqueHandle != 0);
		}

		//*************************************************************************************************
		//機　　能： ダブルオブリークにて画像が読み込まれているかどうかチェックする
		//
		//           変数名          [I/O] 型            内容
		//引　　数： なし
		//戻 り 値：                 [ /O] Boolean       True : 画像が読み込まれている
		//                                               False: 画像が読み込まれていない
		//
		//補　　足： なし
		//
		//履　　歴： v13.0 2007/03/19 (WEB)間々田    新規作成
		//*************************************************************************************************
		private static bool IsDoubleObliqueImageLoaded()
		{
			//ダブルオブリークのウィンドウが見つからなかった場合
			if (!GetDoubleObliqueWindow())
			{
				return false;;
			}

			//ダブルオブリークで画像を読み込んでいない場合：ダブルオブリーク側のプロジェクトの存在で判別する
			return !string.IsNullOrEmpty(DouleObliqueProject);
		}

		//*************************************************************************************************
		//機　　能： ダブルオブリークにて画像が読み込まれているかどうかチェックし，
		//           読み込まれていた場合，ユーザに対する問い合わせを行う
		//
		//           変数名          [I/O] 型            内容
		//引　　数： ReasonForClear  [I/ ] String        この関数呼出し後に実行する作業名称
		//戻 り 値：                 [ /O] Boolean       True : 画像が読み込まれたままの状態なのでビジー
		//                                               False: 画像が読み込まれていない状態なので非ビジー
		//
		//補　　足： なし
		//
		//履　　歴： v13.0 2007/03/19 (WEB)間々田    新規作成
		//*************************************************************************************************
		public static bool IsDoubleObliqueBusy(string ReasonForClear)
		{
			//タイムアウト時間用(ms)
			int WaitTime = 0;

			//戻り値初期化：画像が読み込まれていない状態なので非ビジー
			bool result = false;

			//ダブルオブリークにて画像が読み込まれていない場合
			if (!IsDoubleObliqueImageLoaded()) return result;

			//問い合わせ：
			//   %1にメモリを使用するため，ダブルオブリークで読み込んだ画像をクリアしますが，よろしいですか？
			if (DialogResult.Cancel == MessageBox.Show(StringTable.GetResString(17480, ReasonForClear),
														Application.ProductName,
														MessageBoxButtons.OKCancel,
														MessageBoxIcon.Exclamation))
			{
				//画像が読み込まれたままの状態なのでビジー
				result = true;
			}
			else
			{
				//ダブルオブリークの応答フラグを初期化
				DOResult = DoubleObliqueResultType.DORESULT_UNKNOWN;

                //削除2014/09/11(検S1)hata_CTMenuでメッセージ受けるため削除
                //サブクラスコントロール開始
                //OldWndProc = SetWindowLong(frmCTMenu.Instance.Handle.ToInt32(), GWL_WNDPROC, WndProc);

				//ダブルオブリークにクリア命令を送る
                Winapi.PostMessage(DouleObliqueHandle, WM_DOUBLEOBLIQUE, WM_DOUBLEOBLIQUE_CLEAR, frmCTMenu.Instance.Handle.ToInt32());
                
				//ダブルオブリークの応答を待つ
                WaitTime = 60000;
                while (DOResult == DoubleObliqueResultType.DORESULT_UNKNOWN)
                {
                    if (TimeOut(ref WaitTime)) break;
                }

                //削除2014/09/11(検S1)hata_CTMenuでメッセージ受けるため削除
				//サブクラスコントロール終了
                //Winapi.SetWindowLong(frmCTMenu.Instance.Handle.ToInt32(), GWL_WNDPROC, OldWndProc);

				//ダブルオブリーク側を最小化する
                Winapi.PostMessage(DouleObliqueHandle, WM_DOUBLEOBLIQUE, WM_DOUBLEOBLIQUE_MINIMIZE, 0);
                
				//ダブルオブリークの応答フラグにより分岐
				switch (DOResult)
				{
					case DoubleObliqueResultType.DORESULT_UNKNOWN:

						//メッセージを表示：ダブルオブリーク側が無応答状態です。処理を中止します。(警告メッセージ)
						MessageBox.Show(StringTable.GetResString(17482),
										Application.ProductName,
										MessageBoxButtons.OK,
										MessageBoxIcon.Error);

						//画像が読み込まれたままの状態なのでビジー
						result = true;
						break;

					case DoubleObliqueResultType.DORESULT_CANCEL:

						//画像が読み込まれたままの状態なのでビジー
						result = true;
						break;

					case DoubleObliqueResultType.DORESULT_OK:

						//再度ダブルオブリークにて画像が読み込まれているかどうかチェック
						WaitTime = 5000;
						while (IsDoubleObliqueImageLoaded())
						{
							//タイムアウトエラーを監視
							if (TimeOut(ref WaitTime))
							{
								//メッセージを表示：ダブルオブリーク側の読み込み画像のクリアに失敗しました。
								MessageBox.Show(StringTable.GetResString(17481),
												Application.ProductName,
												MessageBoxButtons.OK,
												MessageBoxIcon.Error);

								//画像が読み込まれたままの状態なのでビジー
								result = true;

								break;
							}

						}
						break;
				}

			}

			return result;
		}

		//*************************************************************************************************
		//機　　能： ダブルオブリークをアクティブにして，現在表示中のＣＴ画像のパスをダブルオブリーク側に送信します
		//
		//           変数名          [I/O] 型            内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//
		//補　　足： なし
		//
		//履　　歴： v13.0 2007/03/19 (WEB)間々田    新規作成
		//*************************************************************************************************
		public static void ActivateDoubleOblique()
		{
			//ダブルオブリーク起動
			if (!StartDoubleOblique(ProcessWindowStyle.Minimized)) return;

			//ターゲットパス
			string TargetPath = null;

			if (string.IsNullOrEmpty(frmScanImage.Instance.Target))
			{
				TargetPath = modFileIO.GetDefaultFolder(StringTable.GetResString(StringTable.IDS_CTImage));
			}
			else
			{
				TargetPath = Path.GetDirectoryName(frmScanImage.Instance.Target);
			}

			//現在表示中のＣＴ画像のパスをクリップボードにコピー
			Clipboard.Clear();
			Clipboard.SetText(TargetPath);

			//ダブルオブリークに画像読み込み要求を送る
            Winapi.PostMessage(DouleObliqueHandle, WM_DOUBLEOBLIQUE, WM_DOUBLEOBLIQUE_IMPORT, 0);
        }

		//*************************************************************************************************
		//機　　能： 一定時間待つ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： WaitTime        [I/O] Long      残り待ち時間（ms）
		//           Interval        [I/ ] Long      この関数内での待ち時間（ms）
		//戻 り 値：                 [ /O] Boolean   True : 指定した時間（WaitTime）が経過した
		//                                           False: 指定した時間（WaitTime）が経過していない
		//
		//補　　足： なし
		//
		//履　　歴： v13.0 2007/03/19 (WEB)間々田    新規作成
		//*************************************************************************************************
		public static bool TimeOut(ref int WaitTime, int Interval = 1000)
		{
			int StartTime = 0;
			//int NowTime = 0;

			//開始時間を設定
			StartTime = Winapi.GetTickCount();

			//一定時間待つ
            while (Winapi.GetTickCount() < StartTime + Interval)
			{
				Application.DoEvents();
			}

			//待ち時間の更新
			WaitTime = WaitTime - Interval;

			//タイムアウトか？
			return (WaitTime < 1);
		}
	}
}
