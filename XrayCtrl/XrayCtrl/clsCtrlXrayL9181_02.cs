using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace XrayCtrl
{
    //***********************************************************************************
    //
    //   X線制御１３０ｋＶ（L91811-02）
    //
    //       <備考>
    //           制御方法は90kV制御機（L9421）/130kV制御機（L9181）を
    //           新コマンド体系で制御する。
    //
    //   Date        Version     Designed/Changed
    //   2014/10/07  1.00        (KS1)Hata
    //
    //***********************************************************************************
    internal class clsCtrlXrayL9181_02 : IDisposable
    {
		private ThreadEx mtmrCtrlXray;				// スレッド
		private SerialPort mcomCtrlXray;			// 通信

		private string mstrCommFlg;					// 通信ハンドシェイク用
		private int mintSovOver;					// 過負荷保護機能状態

		private const int Interval = 20;			// スレッド周期

		private const int COMM_PORT = 1;			// 通信ポート

        //追加2014/06/09_hata
        private bool CommEndFlg = false;	        // 通信停止要求用
        private bool CtrlXrayFlg = false;	        // スレッドONフラグ
        private ManualResetEvent SerialRead = new ManualResetEvent(false);        //通信read時の待ち認識用
        //追加2015/02/07hata
        private bool CommLineError = false;	    // 通信ラインのエラー有無
        private bool CommDataError = false;	    // 通信データのエラー有無
        private bool CommTimeOutError = false;	// 通信のタイムアウトエラー有無       
        private string TimeOutCommandString;    //TimeOut時のCommand            


#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		Private Const COMM_SETTINGS         As String = "38400,n,8,1"       ' ボーレート
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//Private Const COMM_TIMEOUT          As Integer = 3                  ' 返信タイムアウト時間[ms]
		//v19.40 送受信の仕組みを変更　ここから 2013/10/17 by長野
		//時間内に受信できなかったら、次の送信へ行く。
		//上記処理を一定時間繰り返すようであれば、タイムアウトにする。
		private const int COMM_TIMEOUT = 10;		// 返信タイムアウト時間[s]
		private const int COMM_REPEAT_TIME = 2;		//繰り返し時間
        //追加2015/02/07hata
        private const int COMM_LINE_TIMEOUT = 4;			// CommラインOFFによるタイムアウト時間[s]

        private bool RecieveSts;					//返信状態フラグ(True:正常、False:異常)
		private bool SendSts;						//送信状態フラグ(True:正常、False:異常)
		private bool LogWriteRFlg;
		private bool LogWriteSFlg;

		private const int COMM_RETRY = 3;			// 通信リトライ回数

		private string mstrRecvBuf;

		//Private mAvailTimer                 As DelayType                    ' Availタイマ
		//Private mAutoOffTimer               As DelayType                    ' AutoOffタイマ

		//Private gintOldErrXrayComm          As Integer                      '通信異常フラグ

		private bool mblnCheckComm;					//通信確認用


		//----------------------------------------------------------------------------------------------
		//   X線装置コマンド
		//----------------------------------------------------------------------------------------------

		//----- コマンドポインタ
		private const int P_STS = 1;			// 状態確認

		private const int P_SHV = 2;			// 出力電圧確認
		private const int P_SCU = 3;			// 出力電流確認
		private const int P_SPV = 4;			// 管電圧プリセット確認
		private const int P_SPC = 5;			// 管電流プリセット確認
		private const int P_SIN = 6;			// インターロック確認
		private const int P_SXT = 7;			// X線照射時間確認
		private const int P_SWS = 8;			// ウォーミングアップステップ確認
		private const int P_SWE = 9;			// ウォーミングアップ終了確認
		private const int P_SFC = 10;			// 焦点モード確認
		private const int P_STM = 11;			// X線源通電時間確認
		private const int P_SAT = 12;			// 自動X線停止機能確認
		private const int P_TYP = 13;			// 型名確認
		private const int P_SER = 14;			// 制御基板異常確認
		private const int P_SVI = 15;			// 設定管電圧管電流値確認

		private const int P_SAR = 16;			// 状態一括確認
		private const int P_SNR = 17;			// 「NOT RADY」一括確認
		private const int P_SPH = 18;			// プリヒート確認
		private const int P_SBT = 19;			// バッテリー確認

		private const int P_CTRL = 0;			// コントロールコマンド

		//----- コントロールコマンド -----
		private const string CTRL_HIVOLT = "HIV ";				// 管電圧設定[V]    (0～160)
		private const string CTRL_CURRENT = "CUR ";				// 管電流設定[uA]   (0～200)
		private const string CTRL_XRAY_ON = "XON";				// Ｘ線出射開始
		private const string CTRL_XRAY_OFF = "XOF";				// Ｘ線出射停止
		private const string CTRL_MODE_FOCUS = "CFS ";			// ﾌｫｰｶｽﾓｰﾄﾞｾｯﾄ  (1～4)
		private const string CTRL_AUTO_STOP = "AST";			// 自動X線停止時間設定
		private const string CTRL_RST = "RST";					// 過負荷保護解除
		private const string CTRL_WARMUP = "WUP";				// ウォーミングアップ開始

		//----- ステータスコマンド -----

		private const string CTRL_STS = "STS";					// 状態確認

		private const string CTRL_SHV = "SHV";					// 出力電圧確認
		private const string CTRL_SCU = "SCU";					// 出力電流確認
		private const string CTRL_SPV = "SPV";					// 管電圧プリセット確認
		private const string CTRL_SPC = "SPC";					// 管電流プリセット確認
		private const string CTRL_SIN = "SIN";					// インターロック確認
		private const string CTRL_SVI = "SVI";					// 設定管電圧管電流値確認
		private const string CTRL_SAR = "SAR";					// 状態一括確認
		private const string CTRL_SNR = "SNR";					// 「NOT RADY」一括確認

		private const string CTRL_SXT = "SXT";					// X線照射時間確認
		private const string CTRL_SWS = "SWS";					// ウォーミングアップステップ確認
		private const string CTRL_SWE = "SWE";					// ウォーミングアップ終了確認
		private const string CTRL_SFC = "SFC";					// 焦点モード確認
		private const string CTRL_STM = "STM";					// X線源通電時間確認
		private const string CTRL_SAT = "SAT";					// 自動X線停止機能確認
		private const string CTRL_TYP = "TYP";					// 型名確認
		private const string CTRL_SER = "SER";					// 制御基板異常確認
		private const string CTRL_SPH = "SPH";					// プリヒート確認
		private const string CTRL_SBT = "SBT";					// バッテリー確認

		private const string CTRL_CHECK_COMM = "Enter";			// 通信確認用
		private const string STAT_ERROR = "ERR";				//コマンドやパラメータに誤りがあったとき、または、コマンドを受け付けない状態の時に返信される


        //------------------------------------------------------------------------------------------
		//インターロック
		private const int mcintStsSIN_ON = 0;					//インターロックが閉じています。
		private const int mcintStsSIN_OFF = 1;					//インターロックが開いています。
		//------------------------------------------------------------------------------------------
		//プリヒート
		private const int mcintStsSPH_ON = 1;					//プリヒート中
		private const int mcintStsSPH_OFF = 0;					//プリヒート終了

		//
		// mcomCtrlXray_OnComm イベントで使用する static フィールド
		//
		private static TimeSpan mcomCtrlXray_OnComm_sngTimCont;
		private static int mcomCtrlXray_OnComm_intSetSER;

		//
		// mtmrCtrlXray_Tick イベントで使用する static フィールド
		//
		private static bool mtmrCtrlXray_Tick_blnTmrFlg;
		private static int mtmrCtrlXray_Tick_intSendPointer;
		private static DateTime mtmrCtrlXray_Tick_varSendTime;
		private static DateTime? mtmrCtrlXray_Tick_CommTimeOut;
        //追加2015/04/03hata
        private static DateTime mtmrCtrlXray_LineErrTime;	// 送信タイムアウト用 'v14.14変更 by 間々田 2008/02/20
        private static int LineCount = 0;

		//
		// AutoOff メソッドで使用する static フィールド
		//
		private static int AutoOff_intOldXrayOn;
		private static int AutoOff_intTimeStart;
		private static DateTime AutoOff_varTimXray;

		//
		// Avail メソッドで使用する static フィールド
		//
		private static int Avail_intOldXrayOnAvail;
		private static int Avail_intXrayAvailTime;
		private static int Avail_intXrayAvailOnRun;
		private static int Avail_intXrayAvailInsideRun;
		private static int Avail_intXrayAvailOk;
		private static int Avail_intOldXrayVoltage;
		private static int Avail_intOldXrayCurent;
		private static DateTime Avail_varXrayAvailStartTime;

		/// <summary>
		/// 
		/// </summary>
        public clsCtrlXrayL9181_02()
		{
			lock(cCtrlm.gLock)
			{
				Class_Initialize();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
            //変更2014/06/09_hata
            //排他処理しない
            //lock(cCtrlm.gLock)
			//{
			//	Class_Terminate();
			//}
            Class_Terminate();
		}

		/// <summary>
		/// クラスイニシャライズ
		/// </summary>
		private void Class_Initialize()
		{
			modXray.udtXrayProperty udtXrayPIni = new modXray.udtXrayProperty();

			try
			{
				//v19.40 送受信状態フラグを初期化 by長野 2013/10/17
				RecieveSts = true;
				SendSts = true;
				LogWriteRFlg = false;
				LogWriteSFlg = false;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				If mfrmCtrlXray Is Nothing Then

					Set mfrmCtrlXray = New frmXray
					Load mfrmCtrlXray

					'----- 通信の初期設定 -----
					Set mcomCtrlXray = mfrmCtrlXray.comXray

					mcomCtrlXray.CommPort = COMM_PORT
					mcomCtrlXray.Settings = COMM_SETTINGS
					mcomCtrlXray.PortOpen = True
					mintInputMode = mcomCtrlXray.InputMode
					mcomCtrlXray.InputMode = comInputModeText   'ﾃｷｽﾄﾓｰﾄﾞ
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				if (modData.gintXrayClsCnt == 0)
				{
					mcomCtrlXray = new SerialPort();
                    //mcomCtrlXray.PortName = string.Format("COM{0}", COMM_PORT);
                    //Rev23.10 COMポートを指定できるように改造 by長野 2015/09/29
                    mcomCtrlXray.PortName = string.Format("COM{0}", modData.portNo);
					mcomCtrlXray.BaudRate = 38400;
					mcomCtrlXray.Parity = Parity.None;
					mcomCtrlXray.DataBits = 8;
					mcomCtrlXray.StopBits = StopBits.One;
					mcomCtrlXray.WriteBufferSize = 512;
					mcomCtrlXray.ReadBufferSize = 1024;
					mcomCtrlXray.DtrEnable = true;
					mcomCtrlXray.RtsEnable = true;
					mcomCtrlXray.DataReceived += new SerialDataReceivedEventHandler(mcomCtrlXray_OnComm);

                    //追加2014/10/06hata_V1951反映
                    mcomCtrlXray.ErrorReceived += new SerialErrorReceivedEventHandler(mcomCtrlXray_ErrorReceived);
                    //Rev20.01 コメントアウト CDOffなど無視していいステータスまで入ってきてしまうため by長野 2015/04/10
                    //mcomCtrlXray.PinChanged += new SerialPinChangedEventHandler(mcomCtrlXray_PinChanged);

                    if(!mcomCtrlXray.IsOpen) mcomCtrlXray.Open();

					//初期化
					modXray.gudtXrayP = udtXrayPIni;

					//----- タイマ設定 -----

					mtmrCtrlXray = new ThreadEx(new ThreadStart(mtmrCtrlXray_Timer));
                    mtmrCtrlXray.Name = this.ToString();

                    //追加2014/06/09_hata
                    CtrlXrayFlg = true;
                    CommEndFlg = false;
                    mtmrCtrlXray_Tick_blnTmrFlg = false;

                    mtmrCtrlXray.Start();

					modXray.gblnXrayOver = true;

                    //見直し書き方を変更_2014/10/07hata
                    ModuleCTRLM.ipX_type = modIniFiles.XRAY_TYPE_NO_130KV;

				}

				modData.gintXrayClsCnt = modData.gintXrayClsCnt + 1;
			}
			//catch(Exception ex)
			catch
			{
				//----- エラー処理 -----
//				gintStatusNoValue = STS_R_XRAY_INITIALIZ
                //Debug.Print(ex.Message);
			}
		}

		/// <summary>
		/// クラスターミネイト
		/// </summary>
		private void Class_Terminate()
		{
            //int sTime = 500;              //追加2014/06/09_hata
            //int cntTime = 0;              //追加2014/06/09_hata
            int CntEnd = 3000;  //待ち時間  //追加2014/06/09_hata
            
            try
			{
				modData.gintXrayClsCnt = modData.gintXrayClsCnt - 1;
				if (modData.gintXrayClsCnt == 0)
				{

                    //追加2014/06/09_hata
                    CommEndFlg = true;
                    CtrlXrayFlg = false;

                    //----- 通信終了 -----
                    if (mcomCtrlXray.IsOpen)
                    {
             
                        //追加2014/06/09_hata
                       //OnCommFlgがfalseになるまで待つ
                        //cntTime = 0;
                        //while (OnCommFlg)
                        //{
                        //    Thread.Sleep(sTime);
                        //    if (cntTime > CntEnd)
                        //    {
                        //        break;
                        //    }
                        //    cntTime += sTime;
                        //}
                        //シリアルポートがReadから離れるまで待つ
                        SerialRead.WaitOne(CntEnd);

                        mcomCtrlXray.Close();
                        mcomCtrlXray.Dispose();  //追加2014/06/09_hata
                    }

                    //----- タイマ終了 -----
                    if (mtmrCtrlXray != null)
                    {
                        mtmrCtrlXray.Stop();
                    }
                    
                    mcomCtrlXray = null;
                    mtmrCtrlXray = null;
                    CtrlXrayFlg = false; //追加2014/06/09_hata

				}

            }
			catch
			{
				//----- エラー処理 -----
//				gintStatusNoValue = STS_R_XRAY_INITIALIZ
                //Debug.Print("L9181-02 TryError ");
			}
		}


        //追加2014/10/06hata_V1951反映
        //シリアル通信の信号が変更された
        void mcomCtrlXray_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            string data = "";
            bool eCTSFlg = false;  //追加2015/02/06hata
            bool eDSRFlg = false;  //追加2015/02/06hata

            //ピンチェンジの判別
            switch (e.EventType)
            {
                case SerialPinChange.CtsChanged:
                    //CTSのPinが変更された
                    if (mcomCtrlXray.CtsHolding)
                    {    //CTS On
                        data = "CTS On";
                    }
                    else
                    {    //CTS Off
                        data = "CTS Off";
                        eCTSFlg = true;    //追加2015/02/06hata
                    }
                    break;

                case SerialPinChange.DsrChanged:
                    //DSRのPinが変更された
                    if (mcomCtrlXray.DsrHolding)
                    {
                        //DSR On
                        data = "DSR On";
                    }
                    else
                    {
                        //DSR off
                        data = "DSR Off";
                        eDSRFlg = true;    //追加2015/02/06hata
                    }

                    break;
                case SerialPinChange.CDChanged:
                    //CDのPinが変更された
                    if (mcomCtrlXray.CDHolding)
                    {
                        //CD On
                        data = "CD On";
                    }
                    else
                    {
                        //CD off
                        data = "CD Off";
                    }
                    break;

                case SerialPinChange.Break:
                    //Breakが検出された
                    data = "Break On";
                    break;

                case SerialPinChange.Ring:
                    //Ringが検出された
                    data = "Ring On";
                    break;

            }

            //追加2015/04/03hata
            if ((DateTime.Now - mtmrCtrlXray_LineErrTime).TotalSeconds > 5 & LineCount <= 100)		//v14.14追加 by 間々田 2008/02/20
            {
                LineCount = 0;
            }
            if ((DateTime.Now - mtmrCtrlXray_LineErrTime).TotalSeconds >= 5 & LineCount > 100)		//v14.14追加 by 間々田 2008/02/20
            {
                modXray.gintOldErrXrayComm = modData.CTRL_ON;
                ModuleCTRLM.ipErrsts = modData.ERR_XRAY_COMM_LINE;
                return;
            }
            if (LineCount == 0) mtmrCtrlXray_LineErrTime = DateTime.Now;
            LineCount++;

            //追加2015/03/04hata
            string file = @"C:\CTUSER\XrayOnCommERR";
            string ext = ".log";
            bool app = true;
            try
            {
                FileInfo finfo = new FileInfo(file + ext);
                if (finfo.Exists)
                {
                    if (finfo.Length > 10000000)
                    {
                        //10Mを超える場合は名前の後ろに日付を付けて保管
                        string st = DateTime.Now.ToString("yyyyMMdd'-'HH'.'mm'.'ss'.'fffffff");
                        finfo.MoveTo(file + "_" + st + ext);
                        app = false;
                    }
                }
            }
            catch
            {
            }

            //変更2015/02/06hata
            //StreamWriter sw = new StreamWriter(@"C:\CTUSER\XrayCommR.log", true);
            //変更2015/03/04hata
            //StreamWriter sw = new StreamWriter(@"C:\CTUSER\XrayOnCommERR.log", true);
            StreamWriter sw = new StreamWriter(file + ext, app);
            try
            {
                //変更2015/03/18hata
                //sw.WriteLine(DateTime.Now.ToString() + " " + data);
                sw.WriteLine(DateTime.Now.ToString() + "; " + "PinChanged" + " - " + data);
            }
            catch
            {
            }
            finally
            {
                sw.Close();
                sw.Dispose();

                //追加2015/02/06hata
                if (eCTSFlg | eDSRFlg)
                    CommLineError = true;
                else
                    CommLineError = false;

            }

        }

        //追加2014/10/06hata_V1951反映
        //シリアル通信エラー
        void mcomCtrlXray_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            string data = e.EventType.ToString();

            //追加2015/03/04hata
            string file = @"C:\CTUSER\XrayOnCommERR";
            string ext = ".log";
            bool app = true;
            try
            {
                FileInfo finfo = new FileInfo(file + ext);
                if (finfo.Exists)
                {
                    if (finfo.Length > 10000000)
                    {
                        //10Mを超える場合は名前の後ろに日付を付けて保管
                        string st = DateTime.Now.ToString("yyyyMMdd'-'HH'.'mm'.'ss'.'fffffff");
                        finfo.MoveTo(file + "_" + st + ext);
                        app = false;
                    }
                }
            }
            catch
            {
            }
                   
            //変更2015/02/06hata
            //StreamWriter sw = new StreamWriter(@"C:\CTUSER\XrayCommR.log", true); ;
            //変更2015/03/04hata
            //StreamWriter sw = new StreamWriter(@"C:\CTUSER\XrayOnCommERR.log", true); ;
            StreamWriter sw = new StreamWriter(file + ext, app);           
            try
            {
                //変更2015/03/18hata
                //sw.WriteLine(DateTime.Now.ToString() + " " + data);
                sw.WriteLine(DateTime.Now.ToString() + "; " + "ErrorReceived" + " - " + data);
            }
            catch
            {
            }
            finally
            {
                sw.Close();
                sw.Dispose();

                //追加2015/02/06hata
                switch (e.EventType)
                {
                    case SerialError.Frame:	    //フレームエラー
                    case SerialError.RXParity:	//パリティエラー
                    case SerialError.RXOver:	//入力(受信)バッファのオーバーフロー
                    case SerialError.TXFull:	//出力(送信)バッファのオーバーフロー
                        CommDataError = true;
                        break;
                    default:
                        break;
                }

            }
        }


		/// <summary>
		/// 状態監視
		///   <概略>
		///   OnComm ｲﾍﾞﾝﾄを使用して、通信ｲﾍﾞﾝﾄおよびｴﾗｰをﾄﾗｯﾌﾟします。
		/// </summary>
		/// <param name="eventSender"></param>
		/// <param name="eventArgs"></param>
		private void mcomCtrlXray_OnComm(object sender, EventArgs e)
		{
			string strData = null;
			string strCommand = null;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			int intCnt = 0;
//			float sngData = 0;
//			int i = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			int intBuf = 0;
			string strBuf = null;
			TimeSpan sngTime = TimeSpan.Zero;
			float sngDelayTime = 0;
			int intSERCnt = 0;
			bool bErrDisp = false;

			int iVal = 0;
			float fVal = 0;


#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			Dim fileno As Integer 'v19.40 追加 by長野 2013/10/23
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版


            //追加2014/06/09_hata
            if (CommEndFlg) return;
 
            lock(cCtrlm.gLock)
			{
                try
                {
                    if (mtmrCtrlXray == null) return;
                    if (mtmrCtrlXray.Stoped) return;
                    if (mcomCtrlXray == null) return;
                    if (!mcomCtrlXray.IsOpen) return;

                    if (string.IsNullOrEmpty(mstrCommFlg))
                    {
                        return;
                    }

                    #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                    /*
				    'CommEvent ﾌﾟﾛﾊﾟﾃｨの値に従って処理を行います。
				    Select Case mcomCtrlXray.CommEvent
			        
				    ' ｲﾍﾞﾝﾄ ﾒｯｾｰｼﾞ
				    Case comEvReceive
			        
					    'データ受信
					    mstrRecvBuf = mstrRecvBuf + mcomCtrlXray.Input
			        
					    gintOldErrXrayComm = CTRL_OFF    '通信異常フラグクリア
			        
					    'キャリッジリターン削除
					    If Right$(mstrRecvBuf, Len(vbCr)) = vbCr Then
						    mstrRecvBuf = Left$(mstrRecvBuf, Len(mstrRecvBuf) - Len(vbCr))
					    Else
						    Exit Sub
					    End If
			        
			    '        Debug.Print "受信[" & mstrRecvBuf & "]"
			    
				    Case comEvSend:
			    
					    'v19.40 追加 by 長野 2013/10/23
					    fileno = FreeFile()
					    Open "C:\CTUSER\XrayOnCommERR.log" For Output As fileno
			            
					    Print #fileno, Now
			            
					    Close fileno

				    Exit Sub    'Debug.Print "送信バッファ内のバイト数が、SThreshold プロパティで指定された値よりも少なくなりました。"
				    Case comEvCTS:
			    
					    'v19.40 追加 by 長野 2013/10/23
					    fileno = FreeFile()
					    Open "C:\CTUSER\XrayOnCommERR.log" For Output As fileno
			            
					    Print #fileno, Now
			            
					    Close fileno

				    Exit Sub    'Debug.Print "CTS ﾗｲﾝの状態が変化しました。"
				    Case comEvDSR:
			    
					    'v19.40 追加 by 長野 2013/10/23
					    fileno = FreeFile()
					    Open "C:\CTUSER\XrayOnCommERR.log" For Output As fileno
			            
					    Print #fileno, Now
			            
					    Close fileno

				    Exit Sub    'Debug.Print "DSR ﾗｲﾝの状態が変化しました。"
				    Case comEvCD:
			        
					    'v19.40 追加 by 長野 2013/10/23
					    fileno = FreeFile()
					    Open "C:\CTUSER\XrayOnCommERR.log" For Output As fileno
			            
					    Print #fileno, Now
			            
					    Close fileno
			    
				    Exit Sub    'Debug.Print "CD ﾗｲﾝの状態が変化しました。"
				    Case comEvRing:
			    
					    'v19.40 追加 by 長野 2013/10/23
					    fileno = FreeFile()
					    Open "C:\CTUSER\XrayOnCommERR.log" For Output As fileno
			            
					    Print #fileno, Now
			            
					    Close fileno

				    Exit Sub    'Debug.Print "呼び出し信号を検出しました。"
				    Case comEvEOF:
			    
					    'v19.40 追加 by 長野 2013/10/23
					    fileno = FreeFile()
					    Open "C:\CTUSER\XrayOnCommERR.log" For Output As fileno
			            
					    Print #fileno, Now
			            
					    Close fileno

				    Exit Sub    'Debug.Print "EOF を受信しました。"

			    '''    ' ｴﾗｰ ﾒｯｾｰｼﾞ
			    '''    Case comBreak
			    '''        'Debug.Print "ﾌﾞﾚｰｸ信号を検出しました。"
			    '''    Case comCDTO
			    '''        'Debug.Print "CD ﾀｲﾑｱｳﾄを検出しました。"
			    '''    Case comCTSTO
			    '''        'Debug.Print "CTS ﾀｲﾑｱｳﾄを検出しました。"
			    '''    Case comDCB
			    '''        'Debug.Print "DCB 取得時にｴﾗｰが発生しました。"
			    '''    Case comDSRTO
			    '''        'Debug.Print "DSR ﾀｲﾑｱｳﾄを検出しました。"
			    '''    Case comFrame
			    '''        'Debug.Print "ﾌﾚｰﾐﾝｸﾞ ｴﾗｰが発生しました。"
			    '''    Case comOverrun
			    '''        'Debug.Print "ｵｰﾊﾞｰﾗﾝ ｴﾗｰが発生しました。"
			    '''    Case comRxOver
			    '''        'Debug.Print "受信ﾊﾞｯﾌｧがｵｰﾊﾞｰﾌﾛｰしました。"
			    '''    Case comRxParity
			    '''        'Debug.Print "ﾊﾟﾘﾃｨ ｴﾗｰが発生しました。"
			    '''    Case comTxFull
			    '''        'Debug.Print "送信ﾊﾞｯﾌｧ ﾌﾙ ｴﾗｰが発生しました。"
				    Case Else
			        
					    'v19.40 追加 by 長野 2013/10/23
					    fileno = FreeFile()
					    Open "C:\CTUSER\XrayOnCommERR.log" For Output As fileno
			            
					    Print #fileno, Now
			            
					    Close fileno

					    'Debug.Print "未定義のｴﾗｰまたはｲﾍﾞﾝﾄが発生しました。"
			    '        gintStatusNoValue = STS_XRAY_COMM   '通信エラー
					    Exit Sub
			    
				    End Select
    */
                    #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                    //データ受信
                    int len = mcomCtrlXray.BytesToRead;
                    byte[] buff = new byte[len];

                    //追加2014/06/09_hata
                    SerialRead.Reset();

                    mcomCtrlXray.Read(buff, 0, len);
                    mstrRecvBuf += Encoding.ASCII.GetString(buff);

                    modXray.gintOldErrXrayComm = modData.CTRL_OFF;			//通信異常フラグクリア

                    //追加2014/06/09_hata
                    SerialRead.Set();

                    //キャリッジリターン削除
                    if (mstrRecvBuf.EndsWith("\r") == true)
                    {
                        mstrRecvBuf = mstrRecvBuf.TrimEnd('\r');
                    }
                    else
                    {
                        return;
                    }

                    //				Debug.Print "受信[" & mstrRecvBuf & "]"

                    //----- 受信待ち分岐 -----
                    switch (mstrCommFlg)
                    {
                        //--------------------------------------------------------
                        //   コントロールコマンド
                        //--------------------------------------------------------
                        //コントロールコマンドの返信は、コマンド文字列をそのまま返すだけなので、返信データに対する処理は行わない。
                        case CTRL_HIVOLT:
                            // 管電圧設定[V]    (0～90)

                            //エラーの場合は、現在値でイベントを発生させる
                            if (mstrRecvBuf.StartsWith(STAT_ERROR) == true)
                            {
                                modXray.gintXraySetValueError = modData.CTRL_ON;
                            }
                            break;

                        case CTRL_CURRENT:
                            // 管電流設定[uA]   (0～200)

                            //エラーの場合は、現在値でイベントを発生させる
                            if (mstrRecvBuf.StartsWith(STAT_ERROR) == true)
                            {
                                modXray.gintXraySetValueError = modData.CTRL_ON;
                            }
                            break;

                        case CTRL_XRAY_ON:		// Ｘ線出射開始
                            break;
                        case CTRL_XRAY_OFF:		// Ｘ線出射停止
                            break;
                        case CTRL_MODE_FOCUS:	// ﾌｫｰｶｽﾓｰﾄﾞｾｯﾄ  (1～4)
                            break;
                        case CTRL_AUTO_STOP:	// 自動X線停止時間設定
                            break;
                        case CTRL_RST:			// 過負荷保護解除
                            break;
                        case CTRL_WARMUP:		// ウォーミングアップ開始
                            break;

                        //--------------------------------------------------------
                        //   ステータスコマンド
                        //--------------------------------------------------------
                        case CTRL_STS:

                            //状態確認
                            //   0 : WARMUP YET
                            //   1 : WARMUP
                            //   2 : STANDBY
                            //   3 : XON
                            //   4 : OVER
                            //   5 : NOT_READY

                            //コマンド確認
                            if (mstrRecvBuf.StartsWith(CTRL_STS) == true)
                            {
                                strBuf = mstrRecvBuf.Substring(mstrRecvBuf.IndexOf(" ") + 1, 1);	//データ取得
                                if (int.TryParse(strBuf, out iVal) == true)							//数値チェック
                                {
                                    switch (iVal)
                                    {
                                        case 0:		//WARMUP YET
                                            modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
                                            ModuleCTRLM.ipWarmup = 0;				//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ待機中
                                            ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線OFF中
                                            ModuleCTRLM.ipXStatus = 5;
                                            //ipXPermitWarmup = 1					'ﾌﾟﾘﾋｰﾄ
                                            break;
                                        case 1:		//WARMUP
                                            modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
                                            ModuleCTRLM.ipWarmup = 1;				//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ中
                                            ModuleCTRLM.ipX_On = modData.CTRL_ON;	//X線ON中
                                            ModuleCTRLM.ipXStatus = 3;
                                            //ipXPermitWarmup = 1					'ﾌﾟﾘﾋｰﾄ
                                            break;
                                        case 2:		//STANDBY
                                            modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
                                            ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線照射待機中
                                            ModuleCTRLM.ipWarmup = 2;				//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ完了
                                            ModuleCTRLM.ipXStatus = 6;
                                            //ipXPermitWarmup = 1					'ﾌﾟﾘﾋｰﾄ
                                            break;
                                        case 3:		//XON
                                            modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
                                            ModuleCTRLM.ipX_On = modData.CTRL_ON;	//X線ON中
                                            ModuleCTRLM.ipWarmup = 2;
                                            ModuleCTRLM.ipXStatus = 4;
                                            //ipXPermitWarmup = 1					'ﾌﾟﾘﾋｰﾄ
                                            break;
                                        case 4:		//OVER
                                            modXray.gblnXrayOver = false;			//荷保護機能動作中ﾌﾗｸﾞ
                                            ModuleCTRLM.ipXStatus = 7;
                                            ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
                                            mintSovOver = modData.CTRL_ON;			//解除指令                      2007/05/08追加 by 間々田 過負荷時Ｘ線オフしない現象への対策
                                            //ipX_On = CTRL_OFF						'X線が照射できない状態         2007/05/08追加 by 間々田 過負荷時Ｘ線オフしない現象への対策
                                            break;
                                        case 5:		//NOT READY
                                            modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
                                            ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線が照射できない状態
                                            ModuleCTRLM.ipXStatus = 2;
                                            //ipXPermitWarmup = 1					'ﾌﾟﾘﾋｰﾄ
                                            break;
                                        case 6:		//SELFTEST
                                            modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
                                            ModuleCTRLM.ipX_On = modData.CTRL_ON;	//X線ON中
                                            ModuleCTRLM.ipXStatus = 8;				//ｾﾙﾌﾃｽﾄ中
                                            //ipXPermitWarmup = 1					'ﾌﾟﾘﾋｰﾄ
                                            break;
                                        default:
                                            ModuleCTRLM.ipXStatus = 0;
                                            break;
                                    }

                                    if (ModuleCTRLM.ipWarmup == 2)			//ウォームアップ完了
                                    {
                                        ModuleCTRLM.ipWarmup_Mode = -1;		//不要
                                    }
                                    //Rev20.01 ここは不要 by長野 2015/05/20
                                    //else
                                    //{
                                    //    ModuleCTRLM.ipWarmup_Mode = 1;		//必要
                                    //}

                                    //ｵｰﾊﾞｰ
                                    //ｳｫｰﾑｱｯﾌﾟ中は無視する
                                    if (ModuleCTRLM.ipWarmup != 1)
                                    {
                                        if (modXray.gblnXrayOver == false)
                                        {
                                            modXray.gudtXrayP.intSOV = 1;
                                        }
                                        else
                                        {
                                            modXray.gudtXrayP.intSOV = 0;
                                        }
                                    }
                                    else
                                    {
                                        modXray.gudtXrayP.intSOV = 0;
                                    }
                                }
                            }
                            break;

                        case CTRL_SAR:
                            //状態一括確認
                            // STS  :状態
                            // SHV  :出力電圧
                            // SUC  :出力電流
                            // Nono :0
                            // Nono :0
                            // Nono :0
                            // Nono :0

                            //コマンド確認
                            if (mstrRecvBuf.StartsWith(CTRL_SAR) == true)
                            {
                                //----- STS 状態確認 -----
                                intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ取得
                                strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf) - intBuf);
                                if (int.TryParse(strBuf, out iVal) == true)		//数値チェック
                                {
                                    switch (iVal)
                                    {
                                        case 0:		//WARMUP YET
                                            modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
                                            ModuleCTRLM.ipWarmup = 0;				//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ待機中
                                            ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線OFF中
                                            ModuleCTRLM.ipXStatus = 5;
                                            //ipXPermitWarmup = 1					'ﾌﾟﾘﾋｰﾄ
                                            break;
                                        case 1:		//WARMUP
                                            modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
                                            ModuleCTRLM.ipWarmup = 1;				//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ中
                                            ModuleCTRLM.ipX_On = modData.CTRL_ON;	//X線ON中
                                            ModuleCTRLM.ipXStatus = 3;
                                            //ipXPermitWarmup = 1					'ﾌﾟﾘﾋｰﾄ
                                            break;
                                        case 2:		//STANDBY
                                            modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
                                            ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線照射待機中
                                            ModuleCTRLM.ipWarmup = 2;				//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ完了
                                            ModuleCTRLM.ipXStatus = 6;
                                            //ipXPermitWarmup = 1					'ﾌﾟﾘﾋｰﾄ
                                            break;
                                        case 3:		//XON
                                            modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
                                            ModuleCTRLM.ipX_On = modData.CTRL_ON;	//X線ON中
                                            ModuleCTRLM.ipWarmup = 2;
                                            ModuleCTRLM.ipXStatus = 4;
                                            //ipXPermitWarmup = 1					'ﾌﾟﾘﾋｰﾄ
                                            break;
                                        case 4:		//OVER
                                            modXray.gblnXrayOver = false;			//荷保護機能動作中ﾌﾗｸﾞ
                                            ModuleCTRLM.ipXStatus = 7;
                                            mintSovOver = modData.CTRL_ON;			//解除指令                      2007/05/08追加 by 間々田 過負荷時Ｘ線オフしない現象への対策
                                            ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線が照射できない状態         2007/05/08追加 by 間々田 過負荷時Ｘ線オフしない現象への対策
                                            //ipXPermitWarmup = 1					'ﾌﾟﾘﾋｰﾄ
                                            break;
                                        case 5:		//NOT READY
                                            modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
                                            ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線が照射できない状態
                                            ModuleCTRLM.ipXStatus = 2;
                                            //ipXPermitWarmup = 1					'ﾌﾟﾘﾋｰﾄ
                                            break;
                                        case 6:		//SELFTEST
                                            modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
                                            ModuleCTRLM.ipX_On = modData.CTRL_ON;	//X線ON中
                                            ModuleCTRLM.ipXStatus = 8;				//ｾﾙﾌﾃｽﾄ中
                                            //ipXPermitWarmup = 1     'ﾌﾟﾘﾋｰﾄ
                                            break;
                                        default:
                                            ModuleCTRLM.ipXStatus = 0;
                                            break;
                                    }

                                    if (ModuleCTRLM.ipWarmup == 2)			//ウォームアップ完了
                                    {
                                        ModuleCTRLM.ipWarmup_Mode = -1;		//不要
                                    }
                                    //Rev20.01 ここは不要 by長野 2015/05/20
                                    //else
                                    //{
                                    //    ModuleCTRLM.ipWarmup_Mode = 1;		//必要
                                    //}

                                    //ｵｰﾊﾞｰ
                                    //ｳｫｰﾑｱｯﾌﾟ中は無視する
                                    if (ModuleCTRLM.ipWarmup != 1)
                                    {
                                        if (modXray.gblnXrayOver == false)
                                        {
                                            modXray.gudtXrayP.intSOV = 1;
                                        }
                                        else
                                        {
                                            modXray.gudtXrayP.intSOV = 0;
                                        }
                                    }
                                    else
                                    {
                                        modXray.gudtXrayP.intSOV = 0;
                                    }
                                }

                                //----- SHV 出力電圧確認 -----
                                intBuf = mstrRecvBuf.IndexOf(" ", intBuf) + 1;		//データ取得
                                strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf) - intBuf);
                                if (float.TryParse(strBuf, out fVal) == true)		//数値チェック
                                {
                                    ModuleCTRLM.ipX_Volt = fVal;					//データセット
                                }

                                //----- SCU 出力電流確認 -----
                                intBuf = mstrRecvBuf.IndexOf(" ", intBuf) + 1;		//データ取得
                                strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf) - intBuf);
                                if (float.TryParse(strBuf, out fVal) == true)		//数値チェック
                                {
                                    ModuleCTRLM.ipX_Amp = fVal;						//データセット
                                }
                            }
                            break;

                        case CTRL_SNR:

                            //「NOT READY」一括確認
                            //   SER  :制御基板異常確認
                            //   SIN  :インタロック確認
                            //   SPH  :プリヒート確認
                            //   Nono :0

                            //コマンド確認
                            if (mstrRecvBuf.StartsWith(CTRL_SNR) == true)
                            {
                                //-----(1) 制御基板異常確認 -----
                                intBuf = mstrRecvBuf.IndexOf(" ") + 1;		//データ取得
                                strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf));
                                if (int.TryParse(strBuf, out iVal) == true)
                                {
                                    modXray.gudtXrayP.intSER = iVal;
                                }

                                //制御基板異常確認
                                switch (modXray.gudtXrayP.intSER)
                                {
                                    case 3:
                                        intSERCnt = 1;		//制御基板異常1
                                        break;
                                    case 4:
                                        intSERCnt = 2;		//制御基板異常2
                                        break;
                                    case 200:
                                        intSERCnt = 3;		//送風ファン異常
                                        break;
                                    case 201:
                                        intSERCnt = 4;		//入力電源電圧異常1
                                        break;
                                    case 202:
                                        intSERCnt = 5;		//入力電源電圧異常2
                                        break;
                                    case 203:
                                        intSERCnt = 6;		//制御基板異常6
                                        break;
                                    case 204:
                                        intSERCnt = 7;		//制御基板異常3
                                        break;
                                    case 206:
                                        intSERCnt = 8;		//制御基板異常4
                                        break;
                                    case 207:
                                        intSERCnt = 9;		//制御基板異常5
                                        break;
                                    case 208:
                                        intSERCnt = 10;		//入力電源電圧異常3
                                        break;
                                    case 209:
                                        intSERCnt = 11;		//温度エラー
                                        break;
                                    default:
                                        mcomCtrlXray_OnComm_intSetSER = 0;
                                        intSERCnt = 0;
                                        mcomCtrlXray_OnComm_sngTimCont = TimeSpan.Zero;
                                        break;
                                }

                                //１回しかError表示されないのため、数秒毎に表示させる
                                if (intSERCnt > 0 && intSERCnt < 12)
                                {
                                    bErrDisp = false;
                                    if (intSERCnt == mcomCtrlXray_OnComm_intSetSER)
                                    {
                                        if (mcomCtrlXray_OnComm_sngTimCont == TimeSpan.Zero)
                                        {
                                            mcomCtrlXray_OnComm_sngTimCont = DateTime.Now.TimeOfDay;
                                        }
                                        sngDelayTime = 15;		//20秒毎に表示
                                        sngTime = DateTime.Now.TimeOfDay;

                                        if ((sngTime - mcomCtrlXray_OnComm_sngTimCont).TotalSeconds > -sngDelayTime)
                                        {
                                            //Err表示
                                            bErrDisp = true;
                                            mcomCtrlXray_OnComm_sngTimCont = TimeSpan.Zero;
                                        }
                                        else if (sngTime < mcomCtrlXray_OnComm_sngTimCont)
                                        {
                                            mcomCtrlXray_OnComm_sngTimCont = TimeSpan.Zero;
                                        }
                                    }
                                    else
                                    {
                                        //1回目はErr表示
                                        mcomCtrlXray_OnComm_intSetSER = intSERCnt;
                                        mcomCtrlXray_OnComm_sngTimCont = TimeSpan.Zero;
                                        bErrDisp = true;
                                    }

                                    if (bErrDisp)
                                    {
                                        switch (mcomCtrlXray_OnComm_intSetSER)
                                        {
                                            case 1:
                                                ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR3;	//ipErr=21 制御基板異常1
                                                break;
                                            case 2:
                                                ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR4;	//ipErr=22 制御基板異常2
                                                break;
                                            case 3:
                                                ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR200;	//ipErr=41 送風ファン異常
                                                break;
                                            case 4:
                                                ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR201;	//ipErr=42 入力電源電圧異常1
                                                break;
                                            case 5:
                                                ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR202;	//ipErr=43 入力電源電圧異常2
                                                break;
                                            case 6:
                                                ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR203;	//ipErr=44 制御基板異常6
                                                break;
                                            case 7:
                                                ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR204;	//ipErr=45 制御基板異常3
                                                break;
                                            case 8:
                                                ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR206;	//ipErr=46 制御基板異常4
                                                break;
                                            case 9:
                                                ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR207;	//ipErr=47 制御基板異常5
                                                break;
                                            case 10:
                                                ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR208;	//ipErr=48 入力電源電圧異常3
                                                break;
                                            case 11:
                                                ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR209;	//ipErr=27 温度エラー
                                                break;
                                        }
                                        bErrDisp = false;
                                    }
                                }

                                //-----(2) インタロック確認 -----
                                intBuf = mstrRecvBuf.IndexOf(" ", intBuf) + 1;		//データ取得
                                strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf) - intBuf);
                                if (int.TryParse(strBuf, out iVal) == true)			//数値チェック
                                {
                                    //SIN
                                    if (iVal == mcintStsSIN_ON)
                                    {
                                        ModuleCTRLM.ipInterlock = 1;
                                    }
                                    else if (iVal == mcintStsSIN_OFF)
                                    {
                                        ModuleCTRLM.ipInterlock = 0;
                                    }
                                }

                                //-----(3) プリヒート確認 -----
                                intBuf = mstrRecvBuf.IndexOf(" ", intBuf) + 1;		//データ取得
                                strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf) - intBuf);
                                if (int.TryParse(strBuf, out iVal) == true)			//数値チェック
                                {
                                    //SPH
                                    if (iVal == mcintStsSPH_ON)
                                    {
                                        //プリヒート中
                                        ModuleCTRLM.ipXPermitWarmup = 0;
                                    }
                                    else if (iVal == mcintStsSPH_OFF)
                                    {
                                        //プリヒート終了
                                        ModuleCTRLM.ipXPermitWarmup = 1;
                                    }
                                }
                            }
                            break;

                        //					Case CTRL_STX           '----- X線照射確認 -----
                        case CTRL_SHV:			//----- 出力電圧確認 -----
                            break;
                        case CTRL_SCU:			//----- 出力管電流確認 -----
                            break;
                        case CTRL_SPV:			//----- 管電圧プリセット確認 -----
                            break;
                        case CTRL_SPC:			//----- 管電流プリセット確認 -----
                            break;
                        //					Case CTRL_SOV           '----- 過負荷保護機能確認 -----
                        case CTRL_SIN:			//----- インタロック確認 -----
                            break;
                        //					Case CTRL_SWR           '----- ウォーミングアップ状態確認 -----
                        //					Case CTRL_STI           '----- 状態確認 -----

                        case CTRL_SXT:
                            //----- X線照射時間確認 -----
                            if (mstrRecvBuf.StartsWith(CTRL_SXT) == true)		//コマンド確認
                            {
                                intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
                                strBuf = mstrRecvBuf.Substring(intBuf);
                                if (int.TryParse(strBuf, out iVal) == true)
                                {
                                    //"hhhhhh"→"hhhhhhmm"のﾌｫｰﾏｯﾄにする
                                    modXray.gudtXrayP.lngSXT = Convert.ToInt32(strBuf) * 100;
                                }
                            }
                            break;

                        case CTRL_SWS:

                            //----- ウォーミングアップステップ確認 -----
                            strCommand = mstrRecvBuf.Substring(0, 3);
                            if (strCommand == CTRL_SWS)
                            {
                                strData = mstrRecvBuf.Substring(4, 1) + mstrRecvBuf.Substring(6, 1);

                                int.TryParse(strData, out modData.gintSWS);

                                int.TryParse(mstrRecvBuf.Substring(4, 1), out iVal);

                                //ｳｫｰﾑｱｯﾌﾟﾓｰﾄﾞ
                                switch (iVal)
                                {
                                    case 0:
                                        ModuleCTRLM.ipWarmup_Mode = -1;		//なし
                                        break;
                                    case 1:
                                        ModuleCTRLM.ipWarmup_Mode = 1;		//8H
                                        break;
                                    case 2:
                                        ModuleCTRLM.ipWarmup_Mode = 2;		//1M
                                        break;
                                    case 3:
                                        ModuleCTRLM.ipWarmup_Mode = 3;		//3M
                                        break;
                                }
                            }
                            break;

                        case CTRL_SWE:

                            //----- ウォーミングアップ終了確認 -----
                            //ウォーミングアップ状態確認
                            //ipWarmup= 0    :ｳｫｰﾐﾝｸﾞｱｯﾌﾟ待機中
                            //ipWarmup= 1    :ｳｫｰﾐﾝｸﾞｱｯﾌﾟ中
                            //ipWarmup = 2   :ｳｫｰﾐﾝｸﾞｱｯﾌﾟ完了

                            if (mstrRecvBuf.StartsWith(CTRL_SWE) == true)	//コマンド確認
                            {
                                strBuf = mstrRecvBuf.Substring(3);
                                if (int.TryParse(strBuf, out iVal) == true)
                                {
                                    switch (iVal)
                                    {
                                        case modXray.gcintSweEnd:		//完了
                                            ModuleCTRLM.ipWarmup = 2;	//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ完了
                                            break;
                                        case modXray.gcintSweRun:		//動作中
                                            ModuleCTRLM.ipWarmup = 1;	//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ中
                                            break;
                                        case modXray.gcintSweYet:		//未完
                                            ModuleCTRLM.ipWarmup = 0;	//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ待機中
                                            break;
                                        default:
                                            ModuleCTRLM.ipWarmup = 0;	//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ待機中
                                            break;
                                    }
                                }
                                modXray.gudtXrayP.intSWE = iVal;		//ウォーミングアップ状態
                            }
                            break;

                        case CTRL_SFC:
                            //----- 焦点モード確認 -----
                            if (mstrRecvBuf.StartsWith(CTRL_SFC) == true)	//コマンド確認
                            {
                                intBuf = mstrRecvBuf.IndexOf(" ") + 1;		//データ開始位置取得
                                strBuf = mstrRecvBuf.Substring(intBuf);
                                if (int.TryParse(strBuf, out iVal) == true)
                                {
                                    ModuleCTRLM.ipFocussize = iVal;
                                }
                            }
                            break;

                        case CTRL_STM:
                            //----- X線源通電時間確認 -----
                            if (mstrRecvBuf.StartsWith(CTRL_STM) == true)	//コマンド確認
                            {
                                intBuf = mstrRecvBuf.IndexOf(" ") + 1;		//データ開始位置取得
                                strBuf = mstrRecvBuf.Substring(intBuf);
                                if (int.TryParse(strBuf, out iVal) == true)
                                {
                                    modXray.gudtXrayP.lngSTM = iVal;
                                }
                            }
                            break;

                        case CTRL_SAT:
                            //----- 自動X線OFF時間 -----
                            if (mstrRecvBuf.StartsWith(CTRL_SAT) == true)	//コマンド確認
                            {
                                strBuf = mstrRecvBuf.Substring(3);
                                int.TryParse(strBuf, out modXray.gudtXrayP.intSAT);
                            }
                            break;

                        case CTRL_TYP:

                            //----- 型名確認 -----
                            if (mstrRecvBuf.StartsWith(CTRL_TYP) == true)	//コマンド確認
                            {
                                intBuf = mstrRecvBuf.IndexOf(" ") + 1;		//データ開始位置取得
                                strBuf = mstrRecvBuf.Substring(intBuf);
                                modXray.gudtXrayP.strTYP = strBuf;

                                #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                                //							mblnGetType = true;
                                #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
                            }
                            break;

                        case CTRL_SER:

                            //						----- 制御基板異常確認 -----
                            //						strCommand = Mid$(mstrRecvBuf, 1, 3)
                            //						If strCommand = CTRL_SER Then
                            //							strData = Mid$(mstrRecvBuf, 5, 1)
                            //
                            //'							Select Case CInt(strData)
                            //'							Case 0  '異常なし
                            //'							Case 2: gintErrNoValue = ERR_XRAY_ERR2  'ｲﾝﾀﾛｯｸ用ﾘﾚｰ故障
                            //'							Case 3: gintErrNoValue = ERR_XRAY_ERR3  'トランジスタ故障
                            //'							Case 4: gintErrNoValue = ERR_XRAY_ERR4  '高圧電源供給異常
                            //'							End Select
                            //
                            //						End If
                            break;


                        //10/05/12削除L9421-02対応
                        //    Case CTRL_STC
                        //
                        //        '----- 一括状態確認 -----
                        //        'X線源やインターロックなどの状態をまとめて返します。
                        //        strCommand = Mid$(mstrRecvBuf, 1, 3)
                        //        strData = Mid$(mstrRecvBuf, 5)
                        //        If Left$(strData, 2) = "0x" Then
                        //            strData = "&H" & Mid$(strData, 3)
                        //        End If
                        //
                        //        If strCommand <> CTRL_STC Then
                        //            'STCコマンドの返信ではない
                        //            GoTo ExitHandler
                        //        End If
                        //
                        //        '状態確認
                        //        Select Case (val(strData) And STC_STI_MASK)
                        //        Case STC_WARMUP_YET
                        //            gblnXrayOver = True    '荷保護機能動作中ﾌﾗｸﾞ
                        //            ipWarmup = 0            'ｳｫｰﾐﾝｸﾞｱｯﾌﾟ待機中
                        //            ipX_On = CTRL_OFF       'X線OFF中
                        //            ipXStatus = 5
                        //            ipXPermitWarmup = 1     'ﾌﾟﾘﾋｰﾄ
                        //        Case STC_WARMUP
                        //            gblnXrayOver = True    '荷保護機能動作中ﾌﾗｸﾞ
                        //            ipWarmup = 1            'ｳｫｰﾐﾝｸﾞｱｯﾌﾟ中
                        //            ipX_On = CTRL_ON        'X線ON中
                        //            ipXStatus = 3
                        //            ipXPermitWarmup = 1     'ﾌﾟﾘﾋｰﾄ
                        //        Case STC_STANDBY
                        //            gblnXrayOver = True    '荷保護機能動作中ﾌﾗｸﾞ
                        //            ipX_On = CTRL_OFF       'X線照射待機中
                        //            ipWarmup = 2            'ｳｫｰﾐﾝｸﾞｱｯﾌﾟ完了
                        //            ipXStatus = 6
                        //            ipXPermitWarmup = 1     'ﾌﾟﾘﾋｰﾄ
                        //        Case STC_XON
                        //            gblnXrayOver = True    '荷保護機能動作中ﾌﾗｸﾞ
                        //            ipX_On = CTRL_ON        'X線ON中
                        //            ipWarmup = 2
                        //            ipXStatus = 4
                        //            ipXPermitWarmup = 1     'ﾌﾟﾘﾋｰﾄ
                        //        Case STC_OVER
                        //            gblnXrayOver = False    '荷保護機能動作中ﾌﾗｸﾞ
                        //
                        //'2004-09-27 Shibui
                        //'            ipXStatus = 0
                        //            ipXStatus = 7
                        //
                        //            ipXPermitWarmup = 1     'ﾌﾟﾘﾋｰﾄ
                        //            mintSovOver = CTRL_ON   '解除指令
                        //
                        //        Case STC_NOT_READY
                        //            gblnXrayOver = True    '荷保護機能動作中ﾌﾗｸﾞ
                        //            ipX_On = CTRL_OFF       'X線が照射できない状態
                        //            ipXStatus = 2
                        //            ipXPermitWarmup = 1     'ﾌﾟﾘﾋｰﾄ
                        //        Case STC_PREHEAT
                        //            gblnXrayOver = True    '荷保護機能動作中ﾌﾗｸﾞ
                        //            ipX_On = CTRL_OFF       'ﾌﾟﾘﾋｰﾄ中
                        //            ipXStatus = 1
                        //            ipXPermitWarmup = 0
                        //
                        //'2004-09-27 Shibui
                        //        Case Else
                        //            ipXStatus = 0
                        //
                        //        End Select
                        //
                        //        'ｲﾝﾀﾛｯｸ確認
                        //        Select Case (val(strData) And STC_SIN_MASK)
                        //        Case STC_INTERLOCK_ON
                        //            ipInterlock = 1
                        //        Case STC_INTERLOCK_OFF
                        //            ipInterlock = 0
                        //        End Select
                        //
                        //        '焦点モード確認
                        //        Select Case (val(strData) And STC_SFC_MASK)
                        //        Case STC_FCS_1: ipFocussize = 0
                        //        Case STC_FCS_2: ipFocussize = 1
                        //        Case STC_FCS_3: ipFocussize = 2
                        //        End Select
                        //
                        //        '制御基板異常確認
                        //        Select Case val(strData) And STC_SER_MASK
                        //        Case STC_ERR3
                        //            intErr3 = CTRL_ON
                        //            If sngTimCont3 = 0 Then sngTimCont3 = Timer '追加2010/02/24（KSS)hata
                        //        Case STC_ERR4
                        //            intErr4 = CTRL_ON
                        //            If sngTimCont4 = 0 Then sngTimCont4 = Timer '追加2010/02/24（KSS)hata
                        //        Case Else
                        //            intErr2 = CTRL_OFF
                        //            intErr3 = CTRL_OFF
                        //            intErr4 = CTRL_OFF
                        //            sngTimCont3 = 0 '追加2010/02/24（KSS)hata
                        //            sngTimCont4 = 0 '追加2010/02/24（KSS)hata
                        //        End Select
                        //        '変更2010/02/24（KSS)hata １回しかError表示されないのため ----->
                        //'        If intErr3 <> intOldErr3 And intErr3 = CTRL_ON Then
                        //'            intOldErr3 = intErr3
                        //'            ipErrsts = ERR_XRAY_ERR3
                        //'        Else
                        //'            intOldErr3 = intErr3
                        //'        End If
                        //'        If intErr4 <> intOldErr4 And intErr4 = CTRL_ON Then
                        //'            intOldErr4 = intErr4
                        //'            ipErrsts = ERR_XRAY_ERR4
                        //'        Else
                        //'            intOldErr4 = intErr4
                        //'        End If
                        //        sngDelayTime = 15 '20秒毎に表示
                        //        sngTime = Timer
                        //        If sngTimCont3 < (sngTime - sngDelayTime) And intErr3 = CTRL_ON Then
                        //            'Err表示
                        //            ipErrsts = ERR_XRAY_ERR3
                        //            sngTimCont3 = 0
                        //        ElseIf sngTime - sngTimCont3 < 0 Then
                        //            sngTimCont3 = 0
                        //        End If
                        //        If sngTimCont4 < (sngTime - sngDelayTime) And intErr4 = CTRL_ON Then
                        //            'Err表示
                        //            ipErrsts = ERR_XRAY_ERR4
                        //            sngTimCont4 = 0
                        //        ElseIf sngTime - sngTimCont4 < 0 Then
                        //            sngTimCont4 = 0
                        //        End If
                        //        '変更2010/02/24（KSS)hata １回しかError表示されないのため ----->
                        //
                        //        'ｵｰﾊﾞｰ
                        //        'ｳｫｰﾑｱｯﾌﾟ中は無視する
                        //        If ipWarmup <> 1 Then
                        //            If gblnXrayOver = False Then
                        //                gudtXrayP.intSOV = 1
                        //            Else
                        //                gudtXrayP.intSOV = 0
                        //            End If
                        //        Else
                        //            gudtXrayP.intSOV = 0
                        //        End If

                        //10/05/12削除L9421-02対応
                        //    Case CTRL_STT
                        //
                        //        '----- 動作管電圧管電流カソード値確認 -----
                        //        '管電圧､管電流､カソードの動作値をそれぞれ3桁ずつで返します。
                        //        strCommand = Mid$(mstrRecvBuf, 1, 3)
                        //        strData = Mid$(mstrRecvBuf, 5)
                        //        If strCommand = STAT_STT Then
                        //            ipX_Volt = val(Mid$(strData, 1, 3))     '管電圧値
                        //            ipX_Amp = val(Mid$(strData, 4, 3))      '管電流値
                        //        End If

                        case CTRL_SVI:

                            //						'管電圧･管電流 両方をｾｯﾄした後の、設定管電圧･管電流を取得する。
                            //						If gintTmpVol = ifX_Volt And gintTmpAmp = ifX_Amp Then
                            //						'----- 設定管電圧電流値確認 -----
                            //							'管電圧､管電流の設定値をそれぞれ3桁ずつで返します。
                            //							strCommand = Mid$(mstrRecvBuf, 1, 3)
                            //							strData = Mid$(mstrRecvBuf, 5)
                            //							If strCommand = CTRL_SVI Then
                            //								ipcndVolt = val(Mid$(strData, 1, 3)) '設定管電圧
                            //								ipcndAmp = val(Mid$(strData, 4, 3))  '設定管電流
                            //
                            //								ifX_Volt = ipcndVolt
                            //								ifX_Amp = ipcndAmp
                            //								gintTmpVol = ifX_Volt
                            //								gintTmpAmp = ifX_Amp
                            //
                            //							End If
                            //
                            //							If gblnXrayValue = True Then
                            //								gintXrayValueDisp = 1
                            //								gblnXrayValue = False
                            //							End If
                            //
                            //						End If
                            //設定管電圧電流値確認
                            if (mstrRecvBuf.StartsWith(CTRL_SVI) == true)				//コマンド確認
                            {
                                intBuf = mstrRecvBuf.IndexOf(" ") + 1;					//データ開始位置取得
                                if (modXray.gintTmpVol == ModuleCTRLM.ifX_Volt && modXray.gintTmpAmp == ModuleCTRLM.ifX_Amp)
                                {
                                    //設定管電圧
                                    strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf) - intBuf);	//データ取得
                                    if (float.TryParse(strBuf, out fVal) == true)		//数値チェック
                                    {
                                        ModuleCTRLM.ipcndVolt = fVal;					//設定管電圧
                                        ModuleCTRLM.ifX_Volt = ModuleCTRLM.ipcndVolt;
                                        modXray.gintTmpVol = (int)ModuleCTRLM.ifX_Volt;
                                    }

                                    //設定管電流
                                    strBuf = mstrRecvBuf.Substring(mstrRecvBuf.IndexOf(" ", intBuf) + 1);
                                    if (float.TryParse(strBuf, out fVal) == true)		//数値チェック
                                    {
                                        ModuleCTRLM.ipcndAmp = fVal;					//設定管電流
                                        ModuleCTRLM.ifX_Amp = ModuleCTRLM.ipcndAmp;
                                        modXray.gintTmpAmp = (int)ModuleCTRLM.ifX_Amp;
                                    }
                                }
                                if (modXray.gblnXrayValue == true)
                                {
                                    modXray.gintXrayValueDisp = 1;
                                    modXray.gblnXrayValue = false;
                                }
                                modXray.gstrXrayDisp[modXray.DSP_SVI] = mstrRecvBuf;	//X線情報プロパティ用
                            }
                            break;

                        case CTRL_SPH:

                            //プリヒート確認
                            if (mstrRecvBuf.StartsWith(CTRL_SPH) == true)			//コマンド確認
                            {
                                intBuf = mstrRecvBuf.IndexOf(" ") + 1;				//データ開始位置取得
                                strBuf = mstrRecvBuf.Substring(intBuf);
                                if (int.TryParse(strBuf, out iVal) == true)
                                {
                                    //SPH
                                    if (iVal == mcintStsSPH_ON)
                                    {
                                        //プリヒート中
                                        ModuleCTRLM.ipXPermitWarmup = 0;
                                    }
                                    else if (iVal == mcintStsSPH_OFF)
                                    {
                                        //プリヒート終了
                                        ModuleCTRLM.ipXPermitWarmup = 1;
                                    }
                                }
                            }
                            break;

                        case CTRL_SBT:

                            //バッテリー確認
                            if (mstrRecvBuf.StartsWith(CTRL_SBT) == true)			//コマンド確認
                            {
                                intBuf = mstrRecvBuf.IndexOf(" ") + 1;				//データ開始位置取得
                                strBuf = mstrRecvBuf.Substring(intBuf);
                                if (int.TryParse(strBuf, out iVal) == true)
                                {
                                    modXray.ipXrayBatterySBT = iVal;
                                }
                            }
                            break;

                        case CTRL_CHECK_COMM:
                            if (mstrRecvBuf.StartsWith(STAT_ERROR) == true)
                            {
                                //							If mstrRecvBuf = STAT_ERROR Then
                                mblnCheckComm = true;
                            }
                            break;

                        case STAT_ERROR:
                            //コマンドやパラメータに誤りがあったとき、または、コマンドを受け付けない状態の時に返信される
                            //処理は行わない。
                            break;
                    }
                
                }
                catch
                {
                    
                }
                
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//ExitHandler:
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				mstrCommFlg = "";
				mstrRecvBuf = "";
			}	// lock(cCtrlm.gLock)

        }

		/// <summary>
		/// コマンド送信
		/// </summary>
		/// <param name="Buf"></param>
		/// <returns></returns>
		private bool CommOutput(string Buf)
		{
			bool result = false;
			int intRetryCnt = 0;

Retry:
			try
			{
				mcomCtrlXray.DiscardInBuffer();
				mcomCtrlXray.Write(Buf + "\r");

//				'----- 全てのデータが送信されるまで待機します -----
//				Do
//					If Err Then
//						Exit Do
//					End If
//
//					DoEvents
//				Loop Until .OutBufferCount = 0

				result = true;
			}
			catch
			{
				result = false;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				If intRetryCnt > COMM_RETRY Then
					CommOutput = False
				Else
					intRetryCnt = intRetryCnt + 1
					GoTo Retry
				End If
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
			}

			if (result == false && intRetryCnt <= COMM_RETRY)
			{
				intRetryCnt = intRetryCnt + 1;
				goto Retry;
			}

			return result;
		}

		/// <summary>
		/// ステータスモニタ用タイマ
		/// </summary>
		private void mtmrCtrlXray_Timer()
		{
            //変更2014/06/09_hata
            //while (!mtmrCtrlXray.Sleep(Interval))
            while (CtrlXrayFlg)
            {
				int intRet = 0;
				bool blnRet = false;
				string strData = null;
				string strCommand = null;
                                

                mtmrCtrlXray.Sleep(Interval);//追加2014/06/09_hata
                if (!CtrlXrayFlg) break;   //追加2014/06/09_hata

                if (mtmrCtrlXray == null) break;
                if (mtmrCtrlXray.Stoped) break;
                if (!mtmrCtrlXray.IsAlive) break;
               
                lock(cCtrlm.gLock)
				{
					try
					{
//2004-02-14 Shibui 不要
//'2003-11-18 Shibui 運転準備OFF時の処理
//						If glngDioIn(DI_BOX_READY) = DIO_OFF Then
//							Exit Sub
//						End If
//
						if (mtmrCtrlXray_Tick_blnTmrFlg == true)
						{
							continue;
						}

//2003-10-15 Shibui
//						If gintTmrCtrl <> CTRL_ON Then Exit Sub

						mtmrCtrlXray_Tick_blnTmrFlg = true;

						//ｴﾗｰﾘｾｯﾄ
						if (ModuleCTRLM.ifErrrst == 1)
						{
                            //追加2015/02/06_hata
                            if (ModuleCTRLM.ipErrsts == modData.ERR_XRAY_COMM)
                            {
                                modXray.gintOldErrXrayComm = modData.CTRL_OFF;
                                mtmrCtrlXray_Tick_CommTimeOut = null;
                            }
                            CommTimeOutError = false;

                            ModuleCTRLM.ipErrsts = 0;
							ModuleCTRLM.ifErrrst = 0;	//追加2010/02/24（KSS)hata　１回しかError表示されないのため
						}

                        //追加2015/03/17hata_先頭に追加
                        //----- 過負荷保護解除 -----
                        if (mintSovOver == modData.CTRL_ON || modXray.gudtXrayM.intRST == modData.CTRL_ON)
                        {
                            modXray.gudtXrayM.intRST = modData.CTRL_OFF;
                            mintSovOver = modData.CTRL_OFF;
                            strCommand = CTRL_RST;
                            mstrCommFlg = CTRL_RST;

                            goto SensCommand;
                        }

                        //----- X線OFFの送信 -----
						if (ModuleCTRLM.ifXrayonoff_Set == 2 || ModuleCTRLM.ifWarmup_Cancel == 1)
						{
							strCommand = CTRL_XRAY_OFF;
							mstrCommFlg = CTRL_XRAY_OFF;
							ModuleCTRLM.ifXrayonoff_Set = 0;
							ModuleCTRLM.ifWarmup_Cancel = 0;

							goto SensCommand;
						}

						//----- コマンド実行中（返信待ち） -----
                        //追加2015/02/07_hata
                        //変更2015/03/13hata
                        //if (CommLineError | CommDataError | (!mcomCtrlXray.CtsHolding) | (mcomCtrlXray.DtrEnable & !mcomCtrlXray.DsrHolding))
                        if (CommLineError | CommDataError | (mcomCtrlXray.DtrEnable & !mcomCtrlXray.DsrHolding))
                        {
                            //----- CommLineタイムアウト -----
                            //ここからタイムアウトのカウンタ(s)を計測
                            if (mtmrCtrlXray_Tick_CommTimeOut == null)
                            {
                                mtmrCtrlXray_Tick_CommTimeOut = DateTime.Now;
                            }
                            if ((mtmrCtrlXray_Tick_CommTimeOut != null) && (DateTime.Now - mtmrCtrlXray_Tick_CommTimeOut.Value).TotalSeconds >= COMM_LINE_TIMEOUT)
                            {
                                //変更2015/03/04_hata
                                //TimeOutCommandString = mstrCommFlg; //追加2015/03/02hata
                                string strErr = "";
                                //変更2015/03/13hata
                                //if ((CommLineError) | (!mcomCtrlXray.CtsHolding) | (mcomCtrlXray.DtrEnable & !mcomCtrlXray.DsrHolding)) strErr = "PinChanged";
                                if ((CommLineError) | (mcomCtrlXray.DtrEnable & !mcomCtrlXray.DsrHolding)) strErr = "PinChanged";
                                
                                if (CommDataError) strErr = strErr + " " + "ErrorReceived";
                                TimeOutCommandString = strErr + "_" + mstrCommFlg; 
                                
                                //強制的にNOT READYにする
                                //NOT READY
                                ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線が照射できない状態
                                ModuleCTRLM.ipXStatus = 2;
                                ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
                                mstrCommFlg = "";

                                //X線OFFの送信
                                strCommand = "";

                                //X線OFFにしておく
                                //mstrCommFlg = CTRL_XRAY_OFF;
                                ModuleCTRLM.ifXrayonoff_Set = 0;
                                ModuleCTRLM.ifWarmup_Cancel = 0;
                                
                                //タイムアウトにする
                                //Debug.WriteLine("CommLineTimeOut");
                                RecieveSts = false;
                                CommTimeOutError = true;
                                goto ErrorHandler;

                            }
                            //Debug.WriteLine("CommLineTime=" + (DateTime.Now - mtmrCtrlXray_Tick_CommTimeOut.Value).TotalSeconds.ToString());
                            mtmrCtrlXray_Tick_blnTmrFlg = false;
                            continue;
                        }

                        //if (!string.IsNullOrEmpty(mstrCommFlg))   //変更2015/02/06_hata
                        else if (!string.IsNullOrEmpty(mstrCommFlg))
						{
//							'----- タイムアウト -----
//							'lngReceiveTime = (Timer - lngSendTime) / 60 * 60    '受信完了時間  'v14.14削除 by 間々田 2008/02/20
//							'If lngReceiveTime >= COMM_TIMEOUT Then                             'v14.14削除 by 間々田 2008/02/20
//							If DateDiff("s", varSendTime, Now) >= COMM_TIMEOUT Then             'v14.14追加 by 間々田 2008/02/20
//								'タイムアウト
//								mstrCommFlg = ""
//								GoTo ErrorHandler
//							End If
//							blnTmrFlg = False
//							Exit Sub
//						End If

							//----- タイムアウト -----
//							If DateDiff("s", varSendTime, Now) >= COMM_TIMEOUT Then             'v14.14追加 by 間々田 2008/02/20
							if ((DateTime.Now - mtmrCtrlXray_Tick_varSendTime).TotalSeconds >= COMM_REPEAT_TIME)	//v14.14追加 by 間々田 2008/02/20
							{
                                TimeOutCommandString = mstrCommFlg; //追加2015/03/02hata
                                
                                mstrCommFlg = "";
								//v19.40 次の送信に行けるように削除 by長野 2013/10/17
								//GoTo ErrorHandler
								//v19.40 ここからタイムアウトのカウンタ(s)を計測
								if (mtmrCtrlXray_Tick_CommTimeOut == null)
								{
									mtmrCtrlXray_Tick_CommTimeOut = mtmrCtrlXray_Tick_varSendTime;
								}

								if (mtmrCtrlXray_Tick_CommTimeOut != null && (DateTime.Now - mtmrCtrlXray_Tick_CommTimeOut.Value).TotalSeconds > COMM_TIMEOUT)
								{
									//ここを本当のタイムアウトにする

									RecieveSts = false;
                                    //変更2015/02/07hata
                                    //throw new Exception();
                                    CommTimeOutError = true;
                                    goto ErrorHandler;
                                }
							}

							mtmrCtrlXray_Tick_blnTmrFlg = false;
							continue;
						//v19.40 正常に送受信できたらタイムアウト用のカウンタを初期化 by長野 2013/10/17
						//End If
						}
						else
						{
							mtmrCtrlXray_Tick_CommTimeOut = null;
                            CommTimeOutError = false;   //追加2015/02/07hata
                        }

//2003-11-18 Shibui 起動時に一回通信を行う
						if (mblnCheckComm == false)
						{
							if (string.IsNullOrEmpty(strCommand))
							{
								strCommand = CTRL_CHECK_COMM;
								mstrCommFlg = CTRL_CHECK_COMM;
								blnRet = CommOutput(strCommand);	//コマンド送信
								if (blnRet == false)
								{
									throw new Exception();
								}
								//lngSendTime = Timer '送信した時刻を格納
								mtmrCtrlXray_Tick_varSendTime = DateTime.Now;	//送信した時刻を格納  'v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
							}
							goto ExitHandler;
						}

						//----- 異常確認 -----
						intRet = ChkXray();
						if (intRet != 0)
						{
							modData.gintStatusNoValue = intRet;		//ステータスNoセット
							ModuleCTRLM.ifXrayonoff_Set = 2;		//X線をOFFする
						}

//						Call XrayDisp   'X線情報表示
						AutoOff();		//自動X線OFF
						Avail();		//アベイラブル

						//----- ステータスモニタ -----
						switch (mtmrCtrlXray_Tick_intSendPointer)
						{
//10/05/12変更L9421-02対応 ----->
//							Case P_STX  'X線照射確認
//								mstrCommFlg = CTRL_STX
//								strCommand = mstrCommFlg
							case P_STS:		//状態確認
								mstrCommFlg = CTRL_STS;
								strCommand = mstrCommFlg;
								break;
//10/05/12変更L9421-02対応 -----<

							case P_SHV:		//出力電圧確認
								mstrCommFlg = CTRL_SHV;
								strCommand = mstrCommFlg;
								break;

							case P_SCU:		//出力電流確認
								mstrCommFlg = CTRL_SCU;
								strCommand = mstrCommFlg;
								break;

							case P_SPV:		//管電圧プリセット確認
								mstrCommFlg = CTRL_SPV;
								strCommand = mstrCommFlg;
								break;

							case P_SPC:		//管電流プリセット確認
								mstrCommFlg = CTRL_SPC;
								strCommand = mstrCommFlg;
								break;

//10/05/12削除L9421-02対応
//							Case P_SOV    '過負荷保護機能確認
//								mstrCommFlg = CTRL_SOV
//								strCommand = mstrCommFlg

							case P_SIN:		//インタロック確認
								mstrCommFlg = CTRL_SIN;
								strCommand = mstrCommFlg;
								break;

//10/05/12削除L9421-02対応
//							Case P_SWR  'ウォーミングアップ状態確認
//								mstrCommFlg = CTRL_SWR
//								strCommand = mstrCommFlg

//10/05/12削除L9421-02対応
//							Case P_STI  '状態確認
//								mstrCommFlg = CTRL_STI
//								strCommand = mstrCommFlg

							case P_SXT:		//X線照射時間確認
								mstrCommFlg = CTRL_SXT;
								strCommand = mstrCommFlg;
								break;

							case P_SWS:		//ウォーミングアップステップ確認
								mstrCommFlg = CTRL_SWS;
								strCommand = mstrCommFlg;
								break;

							case P_SWE:		//ウォーミングアップ終了確認
								mstrCommFlg = CTRL_SWE;
								strCommand = mstrCommFlg;
								break;

							case P_SFC:		//焦点モード確認
								mstrCommFlg = CTRL_SFC;
								strCommand = mstrCommFlg;
								break;

							case P_STM:		//X線源通電時間確認
								mstrCommFlg = CTRL_STM;
								strCommand = mstrCommFlg;
								break;

							case P_SAT:		//自動X線停止機能確認
								mstrCommFlg = CTRL_SAT;
								strCommand = mstrCommFlg;
								break;

							case P_TYP:		//型名確認
								mstrCommFlg = CTRL_TYP;
								strCommand = mstrCommFlg;
								break;

							case P_SER:		//制御基板異常確認
								mstrCommFlg = CTRL_SER;
								strCommand = mstrCommFlg;
								break;

//10/05/12削除L9421-02対応
//							Case P_STC '一括状態確認
//								mstrCommFlg = CTRL_STC
//								strCommand = mstrCommFlg

//10/05/12削除L9421-02対応
//							Case P_STT '動作管電圧管電流カソード値確認
//								mstrCommFlg = CTRL_STT
//								strCommand = mstrCommFlg

//10/05/12追加L9421-02対応 ----->
							case P_SVI:		//設定値一括状態確認
								mstrCommFlg = CTRL_SVI;
								strCommand = mstrCommFlg;
								break;

							case P_SAR:		//状態一括確認
								mstrCommFlg = CTRL_SAR;
								strCommand = mstrCommFlg;
								break;

							case P_SNR:		//「Not Ready」一括確認
								mstrCommFlg = CTRL_SNR;
								strCommand = mstrCommFlg;
								break;

							case P_SPH:		// プリヒート確認
								mstrCommFlg = CTRL_SPH;
								strCommand = mstrCommFlg;
								break;

							case P_SBT:		// バッテリー確認
								mstrCommFlg = CTRL_SBT;
								strCommand = mstrCommFlg;
								break;
//10/05/12追加L9421-02対応 -----<

							case P_CTRL:	//コントロールコマンド

								//----- X線OFFの送信 -----
								if (ModuleCTRLM.ifXrayonoff_Set == 2 || ModuleCTRLM.ifWarmup_Cancel == 1)
								{
									strCommand = CTRL_XRAY_OFF;
									mstrCommFlg = CTRL_XRAY_OFF;
									ModuleCTRLM.ifXrayonoff_Set = 0;
									ModuleCTRLM.ifWarmup_Cancel = 0;

									goto SensCommand;
								}

								if (ModuleCTRLM.ipX_On == 1)
								{
									if (ModuleCTRLM.ipX_Amp != 0)
									{
										ModuleCTRLM.ipX_Watt = (float)(ModuleCTRLM.ipX_Volt / 1000000.0 * ModuleCTRLM.ipX_Amp * 1000.0);
									}
								}
								else
								{
									ModuleCTRLM.ipX_Watt = 0;
								}

								if (ModuleCTRLM.ipWarmup != 1)
								{
									//ウォームアップ中は実行不可

									if (ModuleCTRLM.ifXrayonoff_Set == 1 && ModuleCTRLM.ipWarmup != 0)
									{
										//----- X線ONの送信 -----
										strCommand = CTRL_XRAY_ON;
										mstrCommFlg = CTRL_XRAY_ON;
										ModuleCTRLM.ifXrayonoff_Set = 0;
										modXray.gudtXrayM.intWarmUp = 0;	//追加10/05/12(KS1)hata_L9421-02対応

										goto SensCommand;

//									ElseIf (ifXrayonoff_Set = 1 And ipWarmup = 0) Or _
//'											(ifXrayonoff_Set = 0 And ipWarmup = 0 And gudtXrayM.intWarmUp > 0) Then
									}
									else if (ModuleCTRLM.ifXrayonoff_Set == 1 && ModuleCTRLM.ipWarmup == 0)
									{
										//----- ウォームアップ開始コマンド送信 -----
										strCommand = CTRL_WARMUP;
										mstrCommFlg = CTRL_WARMUP;
										ModuleCTRLM.ifXrayonoff_Set = 0;
										modXray.gudtXrayM.intWarmUp = 0;	//追加10/05/12(KS1)hata_L9421-02対応

										goto SensCommand;
									}

									//----- 管電圧値上昇 -----
									if (ModuleCTRLM.ifX_Volt_Up == 1)
									{
										ModuleCTRLM.ifX_Volt_Up = 0;

										//管電圧最大値未満の時に実行
										if (ModuleCTRLM.ipcndVolt < modData.gudtXraySystemValue.E_XraySetMaxkV)
										{
											ModuleCTRLM.ifX_Volt = ModuleCTRLM.ipcndVolt + 1;
										}
									}

									//----- 管電圧値下降 -----
									if (ModuleCTRLM.ifX_Volt_Down == 1)
									{
										ModuleCTRLM.ifX_Volt_Down = 0;

										//管電圧最大値未満の時に実行
										if (ModuleCTRLM.ipcndVolt > modData.gudtXraySystemValue.E_XraySetMinkV)
										{
											ModuleCTRLM.ifX_Volt = ModuleCTRLM.ipcndVolt - 1;
										}
									}

									//----- 管電流値上昇 -----
									if (ModuleCTRLM.ifX_Amp_Up == 1)
									{
										ModuleCTRLM.ifX_Amp_Up = 0;

										//管電流最大値未満の時に実行
										if (ModuleCTRLM.ipcndAmp < modData.gudtXraySystemValue.E_XraySetMaxuA)
										{
											ModuleCTRLM.ifX_Amp = ModuleCTRLM.ipcndAmp + 1;
										}
									}

									//----- 管電流値下降 -----
									if (ModuleCTRLM.ifX_Amp_Down == 1)
									{
										ModuleCTRLM.ifX_Amp_Down = 0;

										//管電流最大値未満の時に実行
										if (ModuleCTRLM.ipcndAmp > modData.gudtXraySystemValue.E_XraySetMinuA)
										{
											ModuleCTRLM.ifX_Amp = ModuleCTRLM.ipcndAmp - 1;
										}
									}

									//----- 管電圧値の送信 -----
//2004-03-23 Shibui
//									If ifX_Volt <> ipcndVolt Then
									if (ModuleCTRLM.ifX_Volt != ModuleCTRLM.ipcndVolt && modXray.gintTmpVol != ModuleCTRLM.ifX_Volt)
									{
										//設定管電圧値に変化がある時（X線発生装置から取得した設定管電圧値と比較）
										strData = ModuleCTRLM.ifX_Volt.ToString("000");	// 設定値取得(V)
										strCommand = CTRL_HIVOLT + strData;				// 電圧値の設定コマンド生成
										mstrCommFlg = CTRL_HIVOLT;

//2004-03-23 Shibui
										modXray.gintTmpVol = (int)ModuleCTRLM.ifX_Volt;
										modXray.gblnXrayValue = true;

										goto SensCommand;
									}

									//----- 管電流値の送信 -----
									if (ModuleCTRLM.ifX_Amp != ModuleCTRLM.ipcndAmp)
									{
										//設定管電流値に変化がある時（X線発生装置から取得した設定管電流値と比較）
										strData = ModuleCTRLM.ifX_Amp.ToString("000");	// 設定値取得(V)
										strCommand = CTRL_CURRENT + strData;			// 電流値の設定コマンド生成
										mstrCommFlg = CTRL_CURRENT;

//2004-03-23 Shibui
										modXray.gintTmpAmp = (int)ModuleCTRLM.ifX_Amp;
										modXray.gblnXrayValue = true;

										goto SensCommand;
									}

									//----- フォーカスモード送信 -----
									if (ModuleCTRLM.ifFocussize != ModuleCTRLM.ipFocussize)
									{
										if (ModuleCTRLM.ifFocussize < 0 || ModuleCTRLM.ifFocussize > 3)
										{
											ModuleCTRLM.ifFocussize = 1;
										}
										else
										{   
                                            //strCommand = CTRL_MODE_FOCUS + Convert.ToString(ModuleCTRLM.ifFocussize + 1); //変更2015/05/29hata
                                            strCommand = CTRL_MODE_FOCUS + Convert.ToString(ModuleCTRLM.ifFocussize);
											mstrCommFlg = CTRL_MODE_FOCUS;
										}

										goto SensCommand;
									}

									//追加2010/05/21(KS1)hata_L9421-02対応 ----->
									//----- 過負荷保護解除（ﾘｾｯﾄ） -----
									if (modXray.gudtXrayM.intRST == modData.CTRL_ON)
									{
										//ｺﾏﾝﾄﾞ文字列作成
										modXray.gudtXrayM.intRST = modData.CTRL_OFF;
										strCommand = CTRL_RST;
										mstrCommFlg = CTRL_RST;
										mintSovOver = modData.CTRL_OFF;

										goto SensCommand;
									}
									//追加2010/05/21(KS1)hata_L9421-02対応 -----<
								}

								//----- 過負荷保護解除 -----
								if (mintSovOver == modData.CTRL_ON)
								{
									mintSovOver = modData.CTRL_OFF;	//追加2010/02/23(KSS)hata
									strCommand = CTRL_RST;
									mstrCommFlg = CTRL_RST;

									goto SensCommand;
								}
								break;
						}

                    SensCommand:

                        if (!CtrlXrayFlg) break;    //追加2014/06/09_hata

                        //----- コマンド送信 -----
						if (!string.IsNullOrEmpty(strCommand))
						{
							blnRet = CommOutput(strCommand);	//コマンド送信
//v19.40 送信失敗しても次の送信を行う by長野 2013/10/17
							SendSts = blnRet;
//v19.40					'If blnRet = False Then GoTo ErrorHandler
//							If blnRet = False Then GoTo ErrorHandler
//							lngSendTime = Timer '送信した時刻を格納
							mtmrCtrlXray_Tick_varSendTime = DateTime.Now;	//送信した時刻を格納  'v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
						}

						//----- ポインタ管理 次に送信するコマンドにポインタ移動 -----
//						If gblnfrmXray = True Then
//
//							'----- X線情報表示フォーム表示中 -----
//							intSendPointer = intSendPointer + 1
//							If intSendPointer > P_SVI Then
//								intSendPointer = P_CTRL
//							End If
//
//						Else
							//----- X線情報表示フォーム非表示中 -----
							switch (mtmrCtrlXray_Tick_intSendPointer)
							{
								case P_CTRL:
									mtmrCtrlXray_Tick_intSendPointer = P_SWS;	//ウォーミングアップステップ確認
									break;
								case P_SWS:
									mtmrCtrlXray_Tick_intSendPointer = P_SWE;
									break;
								case P_SWE:
									mtmrCtrlXray_Tick_intSendPointer = P_TYP;	//型名
									break;

								case P_TYP:
									mtmrCtrlXray_Tick_intSendPointer = P_SAR;	//一括状態確認
									break;
								case P_SAR:
									mtmrCtrlXray_Tick_intSendPointer = P_SNR;	//NotReady一括状態確認
									break;
								case P_SNR:
									mtmrCtrlXray_Tick_intSendPointer = P_SBT;	//バッテリー確認
									break;

								case P_SBT:
									mtmrCtrlXray_Tick_intSendPointer = P_SVI;	//設定電圧値電流値確認
									break;
								case P_SVI:
									mtmrCtrlXray_Tick_intSendPointer = P_SFC;	//異常
									break;

//								Case P_SFC:     intSendPointer = P_CTRL 'コントロールコマンド
								case P_SFC:
									mtmrCtrlXray_Tick_intSendPointer = P_STM;	//電源ON通電時間
									break;
								case P_STM:
									mtmrCtrlXray_Tick_intSendPointer = P_SXT;	//X線照射時間
									break;
								case P_SXT:
									mtmrCtrlXray_Tick_intSendPointer = P_CTRL;	//コントロールコマンド
									break;

								default:
									mtmrCtrlXray_Tick_intSendPointer = P_CTRL;
									mstrCommFlg = "";
									break;
							}
//						End If

ExitHandler:

						mtmrCtrlXray_Tick_blnTmrFlg = false;

//2003-10-15 Shibui
//						gintTmrCtrl = 0
//						intOldErrXrayComm = CTRL_OFF

//Debug.Print "X線out(" + Str(Now) + ")"
					}
					catch	//エラー処理
					{
                        //変更2015/02/07hata
                        CommTimeOutError = true;

//                        if (modXray.gintOldErrXrayComm == modData.CTRL_OFF)
//                        {
////							gintStatusNoValue = STS_XRAY_COMM   '通信エラー
//                            modXray.gintOldErrXrayComm = modData.CTRL_ON;
//                            ModuleCTRLM.ipErrsts = modData.ERR_XRAY_COMM;
//                        }

//                        StreamWriter sw = null;
//                        if (LogWriteRFlg == false)
//                        {
//                            //v19.40 ログを残す by長野 2013/10/17
//                            if (RecieveSts == false)
//                            {
//                                sw = new StreamWriter(@"C:\CTUSER\XrayCommR.log", true);
//                                sw.WriteLine(string.Format("{0}-{1}-{2}", DateTime.Now, mtmrCtrlXray_Tick_CommTimeOut, mstrCommFlg));
//                                sw.Close();
//                            }

//                            LogWriteRFlg = true;
//                        }

//                        if (LogWriteSFlg == false)
//                        {
//                            if (SendSts == false)
//                            {
//                                sw = new StreamWriter(@"C:\CTUSER\XrayCommS.log", true);
//                                sw.WriteLine(string.Format("{0}-{1}-{2}", DateTime.Now, mtmrCtrlXray_Tick_CommTimeOut, mstrCommFlg));
//                                sw.Close();
//                            }

//                            LogWriteSFlg = true;
//                        }

//                        mtmrCtrlXray_Tick_blnTmrFlg = false;

//2003-10-15 Shibui
//						gintTmrCtrl = 0

//Debug.Print "X線out(" + Str(Now) + ")"
					}
				}		// lock(cCtrlm.gLock)

            //追加2015/02/07hata
            ErrorHandler:

                StreamWriter sw;
                string file = null;     //追加2015/03/04hata
                string ext = ".log";    //追加2015/03/04hata
                bool app = true;        //追加2015/03/04hata

                if (CommLineError | CommDataError | CommTimeOutError)
                {
                    //Debug.WriteLine("mtmrCtrlXray_Timer Err");

                    if (modXray.gintOldErrXrayComm == modData.CTRL_OFF)
                    {
                        //通信エラー
                        modXray.gintOldErrXrayComm = modData.CTRL_ON;
                        ModuleCTRLM.ipErrsts = modData.ERR_XRAY_COMM;
                    }
                   
                    if (LogWriteRFlg == false)
                    {
                        //v19.40 ログを残す by長野 2013/10/17
                        if (RecieveSts == false)
                        {
                            //追加2015/03/04hata
                            file = @"C:\CTUSER\XrayCommR";
                            try
                            {
                                FileInfo finfoR = new FileInfo(file + ext);
                                if (finfoR.Exists)
                                {
                                    if (finfoR.Length > 10000000)
                                    {
                                        //10Mを超える場合は名前の後ろに日付を付けて保管
                                        string st = DateTime.Now.ToString("yyyyMMdd'-'HH mm ss fffffff");
                                        finfoR.MoveTo(file + "_" + st + ext);
                                        app = false;
                                    }
                                }
                            }
                            catch
                            {
                            }

                            //変更2015/03/04hata
                            //sw = new StreamWriter(@"C:\CTUSER\XrayCommR.log", true);
                            sw = new StreamWriter(file + ext, app);

                            //変更2015/03/18hata
                            //sw.WriteLine(string.Format("{0}-{1}-{2}", DateTime.Now, mtmrCtrlXray_Tick_CommTimeOut, mstrCommFlg));
                            sw.WriteLine(string.Format("{0}; {1} - {2}", DateTime.Now, mtmrCtrlXray_Tick_CommTimeOut, TimeOutCommandString));
                            sw.Close();
                        }

                        LogWriteRFlg = true;
                    }

                    if (LogWriteSFlg == false)
                    {
                        if (SendSts == false)
                        {
                            //追加2015/03/04hata
                            file = @"C:\CTUSER\XrayCommS";
                            try
                            {
                                FileInfo finfoS = new FileInfo(file + ext);
                                if (finfoS.Exists)
                                {
                                    if (finfoS.Length > 10000000)
                                    {
                                        //10Mを超える場合は名前の後ろに日付を付けて保管
                                        string st = DateTime.Now.ToString("yyyyMMdd'-'HH mm ss fffffff");
                                        finfoS.MoveTo(file + "_" + st + ext);
                                        app = false;
                                    }
                                }
                            }
                            catch
                            {
                            }
                            
                            //変更2015/03/04hata
                            //sw = new StreamWriter(@"C:\CTUSER\XrayCommS.log", true);
                            sw = new StreamWriter(file + ext, app);
            
                            //変更2015/03/18hata
                            //sw.WriteLine(string.Format("{0}-{1}-{2}", DateTime.Now, mtmrCtrlXray_Tick_CommTimeOut, mstrCommFlg));
                            sw.WriteLine(string.Format("{0}; {1} - {2}", DateTime.Now, mtmrCtrlXray_Tick_CommTimeOut, TimeOutCommandString));
                            sw.Close();
                        }

                        LogWriteSFlg = true;
                    }

                    mtmrCtrlXray_Tick_blnTmrFlg = false;
                    CommTimeOutError = false;   //追加2015/02/07hata

                }
                //追加2015/03/16hata
                //過負荷解除のLog記録
                if (mstrCommFlg == CTRL_RST)
                {
                    //追加2015/03/04hata
                    file = @"C:\CTUSER\XrayCommOver";
                    try
                    {
                        FileInfo finfoR = new FileInfo(file + ext);
                        if (finfoR.Exists)
                        {
                            if (finfoR.Length > 10000000)
                            {
                                //10Mを超える場合は名前の後ろに日付を付けて保管
                                string st = DateTime.Now.ToString("yyyyMMdd'-'HH'.'mm'.'ss'.'fffffff");
                                finfoR.MoveTo(file + "_" + st + ext);
                                app = false;
                            }
                        }
                    }
                    catch
                    {
                    }

                    sw = new StreamWriter(file + ext, app);

                    sw.WriteLine(string.Format("{0}; {1} - {2}", DateTime.Now, "XrayOver_Reset", "kV=" + ModuleCTRLM.ipcndVolt.ToString() + "/Amp=" + ModuleCTRLM.ipcndAmp.ToString()));
                    sw.Close();

                }              

                if (!CtrlXrayFlg) break;   //追加2014/06/09_hata
                if (mtmrCtrlXray == null) break;
                if (!mtmrCtrlXray.IsAlive) break;
                if (mtmrCtrlXray.Stoped) break;
            }
            mtmrCtrlXray_Tick_blnTmrFlg = false;//追加2014/06/09_hata

        }

		//変更2012/03/25(KS1)hata_違う文字が戻る（入る）ため
		/// <summary>
		/// X線ON中エラー確認
		/// </summary>
		/// <returns></returns>
		private int ChkXray()
		//Private Function ChkXray()
		{
			int intAns = 0;
			//    Static intStsREmergency     As Integer
			//    Static intOldStsREmergency  As Integer
			//    Static intStsRInterlock     As Integer
			//    Static intOldStsRInterlock  As Integer
			//    Static intStsRDoorLock      As Integer
			//    Static intOldStsRDoorLock   As Integer
			//    Static intStsREDoorLock     As Integer
			//    Static intOldStsREDoorLock  As Integer
			//    Static intStsRReady         As Integer
			//    Static intOldStsRReady      As Integer
			//    Static intStsRPower         As Integer
			//    Static intOldStsRPower      As Integer
			//
			//    'X線ON中のみ確認を行う
			//    If ipX_On <> 1 Then
			//        intAns = 0
			//        GoTo ExitHandler
			//    End If
			//
			//    '----- 非常停止 -----
			//    If glngDioIn(DI_EMERGENCY) = DIO_OFF Then
			//        intStsREmergency = CTRL_ON
			//    Else
			//        intStsREmergency = CTRL_OFF
			//    End If
			//    If intStsREmergency <> intOldStsREmergency And intStsREmergency = CTRL_ON Then
			//        intOldStsREmergency = intStsREmergency
			//        intAns = STS_R_EMERGENCY
			//        GoTo ExitHandler
			//    Else
			//        intOldStsREmergency = intStsREmergency
			//    End If
			//
			//    '----- インタロック扉未完 -----
			//    If glngDioIn(DI_INTER_LOCK) = DIO_OFF Then
			//        intStsRInterlock = CTRL_ON
			//    Else
			//        intStsRInterlock = CTRL_OFF
			//    End If
			//    If intStsRInterlock <> intOldStsRInterlock And intStsRInterlock = CTRL_ON Then
			//        intOldStsRInterlock = intStsRInterlock
			//        intAns = STS_R_INTERLOCK
			//        GoTo ExitHandler
			//    Else
			//        intOldStsRInterlock = intStsRInterlock
			//    End If
			//
			//    '----- 試料扉インタロック未完 -----
			//    If glngDioIn(DI_DOOR_LOCK) = DIO_OFF Then
			//        intStsRDoorLock = CTRL_ON
			//    Else
			//        intStsRDoorLock = CTRL_OFF
			//    End If
			//    If intStsRDoorLock <> intOldStsRDoorLock And intStsRDoorLock = CTRL_ON Then
			//        intOldStsRDoorLock = intStsRDoorLock
			//        intAns = STS_R_DOOR_LOCK
			//        GoTo ExitHandler
			//    Else
			//        intOldStsRDoorLock = intStsRDoorLock
			//    End If
			//
			//    '----- 試料扉電磁ロック未完 -----
			//    If gintDoorLockValue = DOOR_LOCK_OFF Then
			//        intStsREDoorLock = CTRL_ON
			//    Else
			//        intStsREDoorLock = CTRL_OFF
			//    End If
			//    If intStsREDoorLock <> intOldStsREDoorLock And intStsREDoorLock = CTRL_ON Then
			//        intOldStsREDoorLock = intStsREDoorLock
			//        intAns = STS_R_EDOOR_LOCK
			//        GoTo ExitHandler
			//    Else
			//        intOldStsREDoorLock = intStsREDoorLock
			//    End If
			//
			//    '----- 運転準備OFF -----
			//    If glngDioIn(DI_BOX_READY) = DIO_OFF Then
			//        intStsRReady = CTRL_ON
			//    Else
			//        intStsRReady = CTRL_OFF
			//    End If
			//    If intStsRReady <> intOldStsRReady And intStsRReady = CTRL_ON Then
			//        intOldStsRReady = intStsRReady
			//        intAns = STS_R_READY
			//        GoTo ExitHandler
			//    Else
			//        intOldStsRReady = intStsRReady
			//    End If
			//
			//    '----- 装置電源OFF -----
			//    If glngDioIn(DI_CHK_ON) = DIO_OFF Then
			//        intStsRPower = CTRL_ON
			//    Else
			//        intStsRPower = CTRL_OFF
			//    End If
			//    If intStsRPower <> intOldStsRPower And intStsRPower = CTRL_ON Then
			//        intOldStsRPower = intStsRPower
			//        intAns = STS_R_POWER
			//        GoTo ExitHandler
			//    Else
			//        intOldStsRPower = intStsRPower
			//    End If
			//
			//    ChkXray = STS_OK
			//    Exit Function
			//

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//ExitHandler:
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			return intAns;
		}


		/// <summary>
		/// 自動X線OFF
		/// </summary>
		private void AutoOff()
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Static intOldXrayOn     As Integer
			Static intOldMotorMove  As Integer
			Static intTimeStart     As Integer
			'Static sngTimXray       As Single
			Static varTimXray       As Variant   'v14.14変更 by 間々田 2008/02/20
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//----- 自動X線OFF機能 -----
			if (ModuleCTRLM.ifXcont_Mode == modData.CTRL_ON)
			{
				//自動X線OFF機能あり

				if (ModuleCTRLM.ipX_On == 1 && ModuleCTRLM.ipWarmup != 1)
				{
					//X線ON中 AND ウォームアップ中ではない

					//X線ON開始時に時間カウントスタート
					if (AutoOff_intOldXrayOn != ModuleCTRLM.ipX_On)
					{
						//sngTimXray = Timer
						AutoOff_varTimXray = DateTime.Now;		//v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
						AutoOff_intTimeStart = modData.CTRL_ON;
					}

					//経過時間確認
					//If intTimeStart = CTRL_ON And (Timer - sngTimXray) > ifXtimer Then
					if (AutoOff_intTimeStart == modData.CTRL_ON && (DateTime.Now - AutoOff_varTimXray).TotalSeconds > ModuleCTRLM.ifXtimer)	//v14.14変更 by 間々田 2008/02/20
					{
						//X線OFF
						ModuleCTRLM.ifXrayonoff_Set = 2;
						AutoOff_intTimeStart = modData.CTRL_OFF;
					}
				}

				AutoOff_intOldXrayOn = ModuleCTRLM.ipX_On;	//X線の状態を保持
			}
			ModuleCTRLM.ipXcont_Mode = ModuleCTRLM.ifXcont_Mode;
			ModuleCTRLM.ipXtimer = ModuleCTRLM.ifXtimer;
		}

		/// <summary>
		/// アベイラブル
		/// </summary>
		private void Avail()
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Static intOldXrayOnAvail        As Integer
			Static intOldVoltageAvail       As Integer
			Static intOldCurrentAvail       As Integer
			Static intXrayAvailOnRun        As Integer
			Static intXrayAvailInsideRun    As Integer
			Static intXrayAvailTime         As Integer
			Static intXrayAvailOk           As Integer
			Static intOldXrayVoltage        As Integer
			Static intOldXrayCurent         As Integer
			'Static sngXrayAvailStartTime   As Single
			Static varXrayAvailStartTime    As Variant 'v14.14変更 by 間々田 2008/02/20
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//----- アベイラブル -----
			if (ModuleCTRLM.ipX_On == 1 && ModuleCTRLM.ipWarmup != 1)
			{
				//X線ON中 AND ウォームアップ中ではない

//				With gudtXrayStatus1Value

				//X線ON監視
				if (Avail_intOldXrayOnAvail != ModuleCTRLM.ipX_On)
				{
					Avail_intXrayAvailOnRun = modData.CTRL_ON;								//X線ONアベイラブル判定スタート
					Avail_intXrayAvailTime = modData.gudtXraySystemValue.E_XrayAvailTimeOn;	//X線ONアベイラブル時間をセット
					Avail_intXrayAvailOk = modData.CTRL_OFF;

					Avail_intOldXrayOnAvail = ModuleCTRLM.ipX_On;

					Avail_intOldXrayVoltage = (int)ModuleCTRLM.ipcndVolt;	//X線ON時には、管電圧・管電流アベイラブル判定を行わない
					Avail_intOldXrayCurent = (int)ModuleCTRLM.ipcndAmp;		//X線ON時には、管電圧・管電流アベイラブル判定を行わない
				}

				//設定値変更監視
				if (Avail_intOldXrayVoltage != ModuleCTRLM.ipcndVolt || Avail_intOldXrayCurent != ModuleCTRLM.ipcndAmp)
				{
					Avail_intXrayAvailInsideRun = modData.CTRL_ON;								//設定値変更アベイラブルスタート
					Avail_intXrayAvailTime =modData. gudtXraySystemValue.E_XrayAvailTimeInside;	//設定値変更アベイラブル時間をセット
					Avail_intXrayAvailOk = modData.CTRL_OFF;

					Avail_intOldXrayVoltage = (int)ModuleCTRLM.ipcndVolt;
					Avail_intOldXrayCurent = (int)ModuleCTRLM.ipcndAmp;
				}

				//アベイラブル範囲判定
				if ((Avail_intXrayAvailOnRun == modData.CTRL_ON || Avail_intXrayAvailInsideRun == modData.CTRL_ON) &&
					(ModuleCTRLM.ipX_Volt <= ModuleCTRLM.ipcndVolt + modData.gudtXraySystemValue.E_XrayAvailkV) &&
					(ModuleCTRLM.ipX_Volt >= ModuleCTRLM.ipcndVolt - modData.gudtXraySystemValue.E_XrayAvailkV) &&
					(ModuleCTRLM.ipX_Amp <= ModuleCTRLM.ipcndAmp + modData.gudtXraySystemValue.E_XrayAvailuA) &&
					(ModuleCTRLM.ipX_Amp >= ModuleCTRLM.ipcndAmp - modData.gudtXraySystemValue.E_XrayAvailuA))
				{
					if (Avail_intXrayAvailOk != modData.CTRL_ON)
					{
						//sngXrayAvailStartTime = Timer 'アベイラブル範囲内になった時、カウントスタート
						Avail_varXrayAvailStartTime = DateTime.Now;	//v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
					}

					Avail_intXrayAvailOk = modData.CTRL_ON;
				}

//				End With

				//アベイラブル時間経過判定
				//If intXrayAvailOk = CTRL_ON _
				//'And (Timer - sngXrayAvailStartTime) >= intXrayAvailTime Then

				//v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
				if (Avail_intXrayAvailOk == modData.CTRL_ON &&
					(DateTime.Now - Avail_varXrayAvailStartTime).TotalSeconds >= Avail_intXrayAvailTime)
				{
					ModuleCTRLM.ipXAvail = 1;		//アベイラブルON
					Avail_intXrayAvailOnRun = modData.CTRL_OFF;		//アベイラブル判定停止
					Avail_intXrayAvailInsideRun = modData.CTRL_OFF;	//アベイラブル判定停止
				}
				else
				{
					ModuleCTRLM.ipXAvail = 0;		//アベイラブルOFF
				}
			}
			else
			{
				//X線OFF中はアベイラブルOFF
				Avail_intXrayAvailOnRun = modData.CTRL_OFF;
				Avail_intXrayAvailInsideRun = modData.CTRL_OFF;
				ModuleCTRLM.ipXAvail = 0;
				Avail_intOldXrayOnAvail = modData.CTRL_OFF;
				Avail_intOldXrayVoltage = 0;
				Avail_intOldXrayCurent = 0;
			}
		}


    }
}
