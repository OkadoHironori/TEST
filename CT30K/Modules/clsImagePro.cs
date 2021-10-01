using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
// Add Start 2018/08/24 M.Oyama 中国語対応
using System.Threading;
// Add End 2018/08/24

namespace CT30K
{
    public class clsImagePro
	{
        //ヘッダ幅
		private int myHeaderWidth;

        //カレントポイント
		//private Ipc32v5.POINTAPI myCurrentPoint;
        private Point myCurrentPoint;
        
        //行ピッチ
		private int myLinePitch;

        //ImageProのプロセス用
        private Process IPprocess;
        //private ProcessStartInfo IPpsInfo;
        private IntPtr IPptr = IntPtr.Zero;


        //*******************************************************************************
        //機　　能： カレントポイント（Ｘ座標）の取得
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
		public int GetCurrentPointX
        {
			get
            {
                return myCurrentPoint.X;
            }
		}

        //*******************************************************************************
        //機　　能： カレントポイント（Ｙ座標）の取得
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
		public int GetCurrentPointY
        {
			get
            {
                return myCurrentPoint.Y;
            }
		}

        //*******************************************************************************
        //機　　能： LinePitch プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
		public int LinePitch
        {
			get
            {
                return myLinePitch;
            }
			set
            {
                myLinePitch = value;
            }
		}

        //*******************************************************************************
        //機　　能： HeaderWidth プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
		public int HeaderWidth
        {
			get
            {
                return myHeaderWidth;
            }
			set
            {
                myHeaderWidth = value;
            }
		}

        //*******************************************************************************
        //機　　能： カレントポイントの設定
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
		public void SetCurrentPoint(int x, int y)
		{
			myCurrentPoint.X = x;
			myCurrentPoint.Y = y;
		}

        //*******************************************************************************
        //機　　能： テキスト描画
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
        //Public Sub DrawText(ByVal Header As String, Optional ByVal Text As String)
		//v17.60 改行が入れれるように第2引数をOptionalに変更 by 長野 2011/06/01
		public void DrawText(string Header, string Text = "")
		{
			string workStr = "";
            //int ret = 0;

            // Unicode → S-JIS(Byte列) 変換する
            // Mod Start 2018/08/24 M.Oyama 中国語対応
            //Encoding sjis = Encoding.GetEncoding("shift-jis");
            //string space = new string(' ', HeaderWidth);
            //string spaceHeader = Header + space;
            //byte[] spaceHeaderBytes = sjis.GetBytes(spaceHeader);

            ////書き込む文字列
            //if (string.IsNullOrEmpty(Text))
            //{
            //    workStr = sjis.GetString(spaceHeaderBytes, 0, HeaderWidth);
            //}
            //else
            //{
            //    workStr = sjis.GetString(spaceHeaderBytes, 0, HeaderWidth) + ":" + Text;
            //}

            ////最大255文字までとする
            //byte[] workStrByte = sjis.GetBytes(workStr);
            ////workStr = sjis.GetString(workStrByte, 0, 255 - 1);  // ※最後の文字が全角途中で切れている場合を想定してカット
            //if (workStrByte.GetUpperBound(0) > 255)
            //    workStr = sjis.GetString(workStrByte, 0, workStrByte.GetUpperBound(0) - 1);  // ※最後の文字が全角途中で切れている場合を想定してカット
            Encoding encode = null;
            if (Thread.CurrentThread.CurrentCulture.Name.StartsWith("zh-CN") == true)
            {
                // 中国語対応
                encode = Encoding.GetEncoding("gb2312");
            }
            else
            {
                // 日本語/英語対応
                encode = Encoding.GetEncoding("shift-jis");
            }

            string space = new string(' ', HeaderWidth);
            string spaceHeader = Header + space;
            byte[] spaceHeaderBytes = encode.GetBytes(spaceHeader);

            //書き込む文字列
            if (string.IsNullOrEmpty(Text))
            {
                workStr = encode.GetString(spaceHeaderBytes, 0, HeaderWidth);
            }
            else
            {
                workStr = encode.GetString(spaceHeaderBytes, 0, HeaderWidth) + ":" + Text;
            }

            //最大255文字までとする
            byte[] workStrByte = encode.GetBytes(workStr);
            if (workStrByte.GetUpperBound(0) > 255)
                workStr = encode.GetString(workStrByte, 0, workStrByte.GetUpperBound(0) - 1);  // ※最後の文字が全角途中で切れている場合を想定してカット
            // Mod End 2018/08/24

            Debug.Print(" workStr " + workStr); 
            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            /*
            ret = Ipc32v5.IpAnCreateObj(Ipc32v5.GO_OBJ_TEXT);                                                               //注釈オブジェクトの作成
			ret = Ipc32v5.IpAnMove(0, myCurrentPoint.X, myCurrentPoint.Y);                                                  //オブジェクトの移動
			ret = Ipc32v5.IpAnText(workStr);                                                                                //注釈オブジェクトにテキストの書込み
			ret = Ipc32v5.IpAnSet(Ipc32v5.GO_ATTR_FONTSIZE, 12);                                                            //フォントサイズの設定：12ポイント
			ret = Ipc32v5.IpAnSetFontName(Properties.Resources.IDS_FImageInfo0);                                            //フォント名の設定：ＭＳ ゴシック
            ret = Ipc32v5.IpAnSet(Ipc32v5.GO_ATTR_TEXTCOLOR, ColorTranslator.ToOle(Color.White));                           //テキストの色指定
            ret = Ipc32v5.IpAnMove(5, myCurrentPoint.X + CTAPI.Winapi.lstrlen(workStr) * 12 / 2 - 1, myCurrentPoint.y + 11);  //注釈ｵﾌﾞｼﾞｪｸﾄのｻｲｽﾞ指定（右下端座標）
			ret = Ipc32v5.IpAnBurn();                                                                                       //テキストを画像に書込む
            */
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            CallImageProFunction.CallDrawText(myCurrentPoint.X, myCurrentPoint.Y, workStr, 12, CTResources.LoadResString(StringTable.IDS_FImageInfo0));
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


            myCurrentPoint.Y = myCurrentPoint.Y + myLinePitch;
		}

        //*******************************************************************************
        //機　　能： Image-Proから指定された領域のデータを取得する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
		//public bool GetWordImage(short[] theImage, int OffsetX = 0, int OffsetY = 0, int theWidth = 1, int theHeight = 1)
		public bool GetWordImage(ref ushort[] theImage, int OffsetX = 0, int OffsetY = 0, int theWidth = 1, int theHeight = 1)
		{
			//戻り値初期化
			bool functionReturnValue = false;

            //配列領域の確保
            theImage = new ushort[theWidth * theHeight];
            
            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            /*
            //領域の設定
			Ipc32v5.ipRect.Left_Renamed = OffsetX;
			Ipc32v5.ipRect.Top = OffsetY;
			Ipc32v5.ipRect.Right_Renamed = OffsetX + theWidth - 1;
			Ipc32v5.ipRect.Bottom = OffsetY + theHeight - 1;

			//Image-Proから指定された領域のデータを取得する
			if (Ipc32v5.IpDocGetArea(Ipc32v5.DOCSEL_ACTIVE, Ipc32v5.ipRect, theImage[0], 0) != 0)
            {
				return functionReturnValue;
            }
            */
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            if (CallImageProFunction.CallGetWordImage(theImage, theHeight, theWidth, OffsetX, OffsetY) != 0)
            {
                return functionReturnValue;
            }
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


            //戻り値セット
			functionReturnValue = true;
			return functionReturnValue;
		}

        //*******************************************************************************
        //機　　能： Image-Proから指定された領域のデータを取得する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
		//public bool GetByteImage(ref byte[] theImage, int OffsetX = 0, int OffsetY = 0, int theWidth = 1, int theHeight = 1)
		public bool GetByteImage(ref byte[] theImage, int OffsetX = 0, int OffsetY = 0, int theWidth = 1, int theHeight = 1)
		{
			//戻り値初期化
			bool functionReturnValue = false;

            //配列領域の確保
            theImage = new byte[theHeight * theWidth]; 
            
            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            /*
            //領域の設定
			Ipc32v5.ipRect.Left_Renamed = OffsetX;
			Ipc32v5.ipRect.Top = OffsetY;
			Ipc32v5.ipRect.Right_Renamed = OffsetX + theWidth - 1;
			Ipc32v5.ipRect.Bottom = OffsetY + theHeight - 1;

			//Image-Proから指定された領域のデータを取得する
			if (Ipc32v5.IpDocGetArea(Ipc32v5.DOCSEL_ACTIVE, Ipc32v5.ipRect, theImage[0], Ipc32v5.CPROG) != 0)
            {
				return functionReturnValue;
            }
            */
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            if (CallImageProFunction.CallGetByteImage(theImage, theHeight, theWidth, OffsetX, OffsetY) != 0)
            {
                return functionReturnValue;
            }
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


            //戻り値セット
			functionReturnValue = true;
			return functionReturnValue;
		}

        //*******************************************************************************
        //機　　能： 画像データをImage-Proの画像に書き込む
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
		public bool DrawWordImage(short[] theImage, int OffsetX = 0, int OffsetY = 0, int theWidth = 1, int theHeight = 1, bool IsWsCreate = false)
		{
            //戻り値初期化
			bool functionReturnValue = false;
            //int ret = 0;

            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            /*
            //領域の設定
			Ipc32v5.ipRect.Left_Renamed = OffsetX;
			Ipc32v5.ipRect.Top = OffsetY;
			Ipc32v5.ipRect.Right_Renamed = OffsetX + theWidth - 1;
			Ipc32v5.ipRect.Bottom = OffsetY + theHeight - 1;

			//新規画像ウィンドウに書き込む場合
			if (IsWsCreate)
            {
				//画像ウィンドウをすべて閉じる
				ret = Ipc32v5.IpAppCloseAll();

				//新規画像ウィンドウ作成
				if (Ipc32v5.IpWsCreate(Ipc32v5.ipRect.Right_Renamed + 1, Ipc32v5.ipRect.Bottom + 1, 300, Ipc32v5.IMC_GRAY16) < 0)
                {
					return functionReturnValue;
				}

                //新規画像ウィンドウを黒く塗りつぶす
				ret = Ipc32v5.IpWsFill(0, 3, 0);
			}

			//ユーザが作成した画像データをImage-Proの画像に書き込む
			if (Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, Ipc32v5.ipRect, theImage[0], 0) != 0)
            {
				return functionReturnValue;
            }
			//アクティブな画像を再描画する
			ret = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);
            */
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            if (CallImageProFunction.CallDrawWordImage(theImage, theHeight, theHeight, OffsetX, OffsetY, Convert.ToInt32(IsWsCreate)) != 0)
            {
                return functionReturnValue;
            }
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

            
            //戻り値セット
			functionReturnValue = true;
			return functionReturnValue;
		}

        //*******************************************************************************
        //機　　能： 画像データをImage-Proの画像に書き込む
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
        public bool DrawWordImage(ushort[] theImage, int OffsetX = 0, int OffsetY = 0, int theWidth = 1, int theHeight = 1, bool IsWsCreate = false)
        {
            //戻り値初期化
            bool functionReturnValue = false;
            //int ret = 0;

            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            /*
            //領域の設定
			Ipc32v5.ipRect.Left_Renamed = OffsetX;
			Ipc32v5.ipRect.Top = OffsetY;
			Ipc32v5.ipRect.Right_Renamed = OffsetX + theWidth - 1;
			Ipc32v5.ipRect.Bottom = OffsetY + theHeight - 1;

			//新規画像ウィンドウに書き込む場合
			if (IsWsCreate)
            {
				//画像ウィンドウをすべて閉じる
				ret = Ipc32v5.IpAppCloseAll();

				//新規画像ウィンドウ作成
				if (Ipc32v5.IpWsCreate(Ipc32v5.ipRect.Right_Renamed + 1, Ipc32v5.ipRect.Bottom + 1, 300, Ipc32v5.IMC_GRAY16) < 0)
                {
					return functionReturnValue;
				}

                //新規画像ウィンドウを黒く塗りつぶす
				ret = Ipc32v5.IpWsFill(0, 3, 0);
			}

			//ユーザが作成した画像データをImage-Proの画像に書き込む
			if (Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, Ipc32v5.ipRect, theImage[0], 0) != 0)
            {
				return functionReturnValue;
            }
			//アクティブな画像を再描画する
			ret = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);
            */
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            if (CallImageProFunction.CallDrawWordImage(theImage, theHeight, theWidth, OffsetX, OffsetY, Convert.ToInt32(IsWsCreate)) != 0)
            {
                return functionReturnValue;
            }
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


            //戻り値セット
            functionReturnValue = true;
            return functionReturnValue;
        }

        //*******************************************************************************
        //機　　能： 画像データをImage-Proの画像に書き込む
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
		public bool DrawByteImage(byte[] theImage, int OffsetX = 0, int OffsetY = 0, int theWidth = 1, int theHeight = 1, bool IsWsCreate = false)
		{
			//戻り値初期化
            bool functionReturnValue = false;

            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            /*
            //領域の設定
			Ipc32v5.ipRect.Left_Renamed = OffsetX;
			Ipc32v5.ipRect.Top = OffsetY;
			Ipc32v5.ipRect.Right_Renamed = OffsetX + theWidth - 1;
			Ipc32v5.ipRect.Bottom = OffsetY + theHeight - 1;

			//新規画像ウィンドウに書き込む場合
			if (IsWsCreate)
            {
				//画像ウィンドウをすべて閉じる
				ret = Ipc32v5.IpAppCloseAll();

				//新規画像ウィンドウ作成
				if (Ipc32v5.IpWsCreate(Ipc32v5.ipRect.Right_Renamed + 1, Ipc32v5.ipRect.Bottom + 1, 300, Ipc32v5.IMC_GRAY) < 0)
                {
					return functionReturnValue;
				}

                //新規画像ウィンドウを黒く塗りつぶす
				ret = Ipc32v5.IpWsFill(0, 3, 0);
			}

			//ユーザが作成した画像データをImage-Proの画像に書き込む
			if (Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, Ipc32v5.ipRect, theImage[0], Ipc32v5.CPROG) != 0)
            {
				return functionReturnValue;
            }
			//アクティブな画像を再描画する
			ret = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);
            */
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            if (CallImageProFunction.CallDrawByteImage(theImage, theHeight, theWidth, OffsetX, OffsetY, Convert.ToInt32(IsWsCreate)) != 0)
            {
                return functionReturnValue;
            }
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//



			//戻り値セット
			functionReturnValue = true;
			return functionReturnValue;
		}

        //*******************************************************************************
        //機　　能： クラス初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
		private void Class_Initialize_Renamed()
		{
			//カレントポイントの初期化
			SetCurrentPoint(0, 0);

			//ヘッダ幅のデフォルト値
			myHeaderWidth = 17;

			//行ピッチのデフォルト値
			myLinePitch = 19;
		}

        /// <summary>
        /// clsImageProクラスのコンストラクタ
        /// </summary>
        public clsImagePro() : base()
		{
			Class_Initialize_Renamed();
		}

        //ImageProサーバーexeを起動
        public IntPtr StartImageProServer()
        {
            IntPtr ptr = IntPtr.Zero;
        
            //追加2015/01/17hata
            bool bret = false;
            //イメージプロServerの起動確認をする
            Process[] ps = Process.GetProcessesByName("ImageProServer");
            foreach (Process p in ps)
            {
                //起動中
                bret = true;
            }
            if (!bret) ptr = ReStartImageProServer();


            if (IPprocess == null)
            {
                ptr = ReStartImageProServer();
            }
            else
            {
                ptr = IPprocess.Handle;
            }
            return ptr;
        }

        //ImageProサーバーexeを起動
        public IntPtr ReStartImageProServer()
        {

            if (IPptr != IntPtr.Zero)
            {
                IPprocess.Close();
            }
            //IPprocess = new Process();
            //IPprocess.StartInfo.FileName = @"C:\CT\COMMAND\ImageProServer.exe";
            //IPprocess.Start();
            //IPptr = process.Handle;


            //イメージプロServerの起動確認をする
            Process[] ps = Process.GetProcessesByName("ImageProServer");
            foreach (Process p in ps)
            {
                //起動中の場合は終了させる
                p.CloseMainWindow();
                p.WaitForExit(2000);
                p.Dispose();
            }

            //イメージプロServerの起動
            Process pIp = null;
            try
            {
                IPprocess = new Process();
                IPprocess.StartInfo.FileName = @"C:\CT\COMMAND\ImageProServer.exe";
                IPprocess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                IPprocess.Start();
                IPptr = IPprocess.Handle;

            }
            finally
            {
                if (pIp != null)
                {
                    pIp.Dispose();
                    pIp = null;
                }
            }

            //// コンソールウィンドウを表示しないように設定する
            //ProcessStartInfo IPpsInfo = new ProcessStartInfo();
            //IPpsInfo.FileName = @"C:\CT\COMMAND\ImageProServer.exe";
            //IPpsInfo.Arguments = "0";
            //IPpsInfo.CreateNoWindow = true;
            //IPpsInfo.UseShellExecute = false;
            //// サーバーexe起動
            //IPprocess = Process.Start(IPpsInfo);
            //IPprocess.WaitForExit(1000);
            //IPptr = IPprocess.Handle;

            // Image-Proの起動
            CallImageProFunction.CallIpStart();

            // 共有メモリ作成
            CallImageProFunction.CallCreateShareMem();

            return IPptr;

        }

        //ImageProサーバーexeを起動
        public bool CloseImageProServer()
        {
            bool bsts = false;

            if (IPprocess != null)
            {
                try
                {
                    bsts = IPprocess.CloseMainWindow();
                }
                catch
                {
                }
            }
            else
            { 
                bsts = true;
            }
            return bsts;
        }

    }
}
