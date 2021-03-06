using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace XrayCtrl
{
	//***********************************************************************************
	//
	//   X線制御１５０ｋＶ　分離型（L8121-02）
	//
	//       <備考>
	//           L8601から流用。
	//
	//   Date        Version     Designed/Changed
	//   2012/03/20  1.00        (KS1)hata
	//
	//***********************************************************************************
	internal class clsCtrlXrayL8121_02 : IDisposable
	{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		Private mfrmCtrlXray                As frmXray                  ' X線制御用フォーム

		Private WithEvents mtmrCtrlXray     As Timer                        ' タイマ
		Private WithEvents mcomCtrlXray     As MSComm                       ' 通信
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

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
//		Private Const COMM_SETTINGS         As String = "9600,n,8,1"       ' ボーレート
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

//2003-11-18 Shibui
		private bool mblnCheckComm;					//通信確認用

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
//2004-07-14 Shibui
		private int mintSendPointer;				//制御用
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//----------------------------------------------------------------------------------------------
		//   X線装置コマンド
		//----------------------------------------------------------------------------------------------

		//----- コマンドポインタ
		private const int P_STX = 1;										// X線照射確認
		private const int P_SHV = 2;										// 出力電圧確認
		private const int P_SCU = 3;										// 出力電流確認
		private const int P_SPV = 4;										// 管電圧プリセット確認
		private const int P_SPC = 5;										// 管電流プリセット確認
		private const int P_SOV = 6;										// 過負荷保護機能確認
		private const int P_SIN = 7;										// インターロック確認
		//Private Const P_SWR                 As Integer = 8                  ' ウォーミングアップ状態確認
		private const int P_SAG = 8;										// ウォーミングアップ状態確認
		//Private Const P_STI                 As Integer = 9                  ' 状態確認
		private const int P_STS = 9;										// 状態確認
		//Private Const P_SXT                 As Integer = 10                 ' X線照射時間確認
		//Private Const P_SWS                 As Integer = 11                 ' ウォーミングアップステップ確認
		//Private Const P_SWE                 As Integer = 12                 ' ウォーミングアップ終了確認
		private const int P_DAG = 12;										// ウォーミングアップ終了確認
		//Private Const P_SFC                 As Integer = 13                 ' 焦点モード確認
		//Private Const P_STM                 As Integer = 14                 ' X線源通電時間確認
		//Private Const P_SAT                 As Integer = 15                 ' 自動X線停止機能確認
		//Private Const P_TYP                 As Integer = 16                 ' 型名確認
		private const int P_SER = 17;										// 制御基板異常確認
		//Private Const P_STC                 As Integer = 18                 ' 一括状態確認
		//Private Const P_STT                 As Integer = 19                 ' 動作管電圧管電流カソード値確認
		//Private Const P_SVI                 As Integer = 20                 ' 設定管電圧管電流値確認
		private const int P_SRL = 21;										// ｵﾍﾟﾚｰﾄｽｲｯﾁ状態確認
		private const int P_SRB = 22;										// リモート動作状態確認
		private const int P_SFS = 23;										// 焦点モード確認
		private const int P_CTRL = 0;										// コントロールコマンド


		//----- コントロールコマンド -----
		private const string CTRL_HIVOLT = "HIV ";							// X線ON中の管電圧設定[V](0～90)
		private const string CTRL_PHVOLT = "PHV ";							// X線OFF中の管電圧設定[V](0～90)
		private const string CTRL_CURRENT = "CUR ";							// X線ON中の管電流設定[uA](0～250)
		private const string CTRL_PCURRENT = "PCU ";						// X線OFF中の管電流設定[uA](0～250)

		private const string CTRL_XRAY_ON = "XON";							// Ｘ線出射開始
		private const string CTRL_XRAY_OFF = "XOF";							// Ｘ線出射停止
		//設定なし
		//Private Const CTRL_MODE_FOCUS        As String = "CFS "             ' ﾌｫｰｶｽﾓｰﾄﾞｾｯﾄ  (1～4)
		//Private Const CTRL_AUTO_STOP         As String = "AST"              ' 自動X線停止時間設定

		private const string CTRL_RST = "RST";								// 過負荷保護解除
		//Private Const CTRL_WARMUP            As String = "WUP"→"XON"　　    ' ウォーミングアップ開始

		private const string CTRL_MODE_FOCUS_S = "CFS";						// ﾌｫｰｶｽﾓｰﾄﾞｾｯﾄ  (SMAll)
		private const string CTRL_MODE_FOCUS_M = "CFM";						// ﾌｫｰｶｽﾓｰﾄﾞｾｯﾄ  (MIDDLE)
		private const string CTRL_MODE_FOCUS_L = "CFL";						// ﾌｫｰｶｽﾓｰﾄﾞｾｯﾄ  (LARGE)


		//----- ステータスコマンド -----
		private const string CTRL_STX = "STX";									// X線照射確認
		private const string CTRL_SHV = "SHV";									// 出力電圧確認
		private const string CTRL_SCU = "SCU";									// 出力電流確認
		private const string CTRL_SPV = "SPV";									// 管電圧プリセット確認
		private const string CTRL_SPC = "SPC";									// 管電流プリセット確認
		private const string CTRL_SOV = "SOV";									// 過負荷保護機能確認
		private const string CTRL_SIN = "SIN";									// インターロック確認
		//Private Const CTRL_SWR               As String = "SWR"              ' ウォーミングアップ状態確認
		private const string CTRL_SAG = "SAG";									// ウォーミングアップ状態確認
		//Private Const CTRL_STI               As String = "STI"              ' 状態確認
		private const string CTRL_STS = "STS";									// 状態確認
		//Private Const CTRL_SXT               As String = "SXT"              ' X線照射時間確認
		//Private Const CTRL_SWS               As String = "SWS"              ' ウォーミングアップステップ確認
		//Private Const CTRL_SWE               As String = "SWE"              ' ウォーミングアップ終了確認
		private const string CTRL_DAG = "DAG";									// ウォーミングアップ終了確認

		//Private Const CTRL_SFC               As String = "SFC"              ' 焦点モード確認
		private const string CTRL_SFS = "SFS";									// 焦点モード確認

		//Private Const CTRL_STM               As String = "STM"              ' X線源通電時間確認
		//Private Const CTRL_SAT               As String = "SAT"              ' 自動X線停止機能確認
		//Private Const CTRL_TYP               As String = "TYP"              ' 型名確認

		private const string CTRL_SER = "SER";									// 制御基板異常確認
		//Private Const CTRL_STC               As String = "STC"              ' 一括状態確認
		//Private Const CTRL_STT               As String = "STT"              ' 動作管電圧管電流カソード値確認
		//Private Const CTRL_SVI               As String = "SVI"              ' 設定管電圧管電流値確認
		private const string CTRL_SRL = "SRL";									// ｵﾍﾟﾚｰﾄｽｲｯﾁ状態確認
		private const string CTRL_SRB = "SRB";									// リモート動作状態確認


//2003-11-18 Shibui
		private const string CTRL_CHECK_COMM = "Enter";		// 通信確認用


		//----- X線制御機から返されるデータ -----
		//Private Const STAT_XRAY_ON          As String = "X-RAY ON"          ' X線照射確認 X線ON
		//Private Const STAT_XRAY_OFF         As String = "X-RAY OFF"         ' X線照射確認 X線OFF
		private const string STAT_SHV = "HIV";									// 出力管電圧
		private const string STAT_SCU = "CUR";									// 出力管電流
		private const string STAT_SPV = "PHV";									// 設定管電圧
		private const string STAT_SPC = "PCU";									// 設定管電流
		private const string STAT_OVER = "OVER";								// 過負荷保護機能 動作中
		//Private Const STAT_NORMAL           As String = "NORMAL"            ' 過負荷保護機能 動作なし
		private const string STAT_RET_INTER_LOCK_ON = "INTER LOCK ON";			// インタロック閉じている
		private const string STAT_RET_INTER_LOCK_OFF = "INTER LOCK OFF";		// インタロック開いている
		//Private Const STAT_WARMUP_ON        As String = "WARMUP ON"         ' ウォーミングアップ中
		//Private Const STAT_WARMUP_OFF       As String = "WARMUP OFF"        ' ウォーミングアップ中ではない
		//Private Const STAT_PREHEAT          As String = "PREHEAT"           ' プリヒート中
		private const string STAT_NOT_READY = "NOT READY";						// X線が照射できない状態
		//Private Const STAT_WARMUP           As String = "WARMUP"            ' ウォーミングアップ動作中
		//Private Const STAT_XON              As String = "XON"               ' X線照射中
		//Private Const STAT_WARMUP_YET       As String = "WARMUP YET"        ' ウォーミングアップ待機中
		//Private Const STAT_STANDBY          As String = "STANDBY"           ' X線照射待機中
		//Private Const STAT_SWS              As String = "WARMUP"            ' ウォーミングアップのモードとステップ
		//Private Const STAT_WARMUP_END       As String = "WARMUP END"        ' ウォーミングアップ終了

		private const string STAT_AGING = "AGING";								// ウォーミングアップ動作中
		private const string STAT_XON = "XON";									// X線照射中
		private const string STAT_STANDBY = "STANDBY";							// X線照射待機中
		private const string STAT_YET = "YET";									// ウォーミングアップ必要
		private const string STAT_END = "END";									// ウォーミングアップ終了

		//Private Const STAT_SFC              As String = "FCS"               ' 焦点
		//Private Const STAT_SFC_1            As String = "FCS 1"             ' 小焦点
		//Private Const STAT_SFC_2            As String = "FCS 2"             ' 中焦点
		//Private Const STAT_SFC_3            As String = "FCS 3"             ' 大焦点
		private const string STAT_SFC_1 = "SMALL";								// 小焦点
		private const string STAT_SFC_2 = "MIDDLE";								// 中焦点
		private const string STAT_SFC_3 = "LARGE";								// 大焦点

		//Private Const STAT_SAT              As String = "Auto Off Time"     ' 自動X線停止機能確認

		private const string STAT_SER = "ERR";									// 制御基板異常確認
		//Private Const STAT_STC              As String = "STC"               ' 一括状態確認
		//Private Const STAT_STT              As String = "STT"               ' 動作管電圧管電流カソード値確認
		//Private Const STAT_SVI              As String = "SVI"               ' 設定電圧値電流値確認

		private const string STAT_OFF = "OFF";									// ｵﾍﾟﾚｰﾄｽｲｯﾁ状態確認用
		private const string STAT_REMOTE = "REMOTE";							// ｵﾍﾟﾚｰﾄｽｲｯﾁ状態確認用
		private const string STAT_LOCAL = "LOCAL";								// ｵﾍﾟﾚｰﾄｽｲｯﾁ状態確認用
		private const string STAT_READY = "READY";								// ﾘﾓｰﾄ動作状態確認用
		private const string STAT_BUSY = "BUSY";								// ﾘﾓｰﾄ動作状態確認用

		private const string STAT_ERROR = "ERROR";								//コマンドやパラメータに誤りがあったとき、または、コマンドを受け付けない状態の時に返信される

		private int iWarmUPEND;													// ウォーミングアップ終了ステータス（内部）

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private int mintInputMode;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//
		// mcomCtrlXray_OnComm イベントで使用する static フィールド
		//
		private static int mcomCtrlXray_OnComm_intErr1;
		private static int mcomCtrlXray_OnComm_intErr2;
		private static int mcomCtrlXray_OnComm_intErr3;
		private static int mcomCtrlXray_OnComm_intErr4;
		private static TimeSpan mcomCtrlXray_OnComm_sngTimCont1;
		private static TimeSpan mcomCtrlXray_OnComm_sngTimCont2;
		private static TimeSpan mcomCtrlXray_OnComm_sngTimCont3;
		private static TimeSpan mcomCtrlXray_OnComm_sngTimCont4;

		//
		// mtmrCtrlXray_Tick イベントで使用する static 変数
		//
		private static bool mtmrCtrlXray_Tick_blnTmrFlg;
		private static int mtmrCtrlXray_Tick_intSendPointer;
		private static DateTime mtmrCtrlXray_Tick_varSendTime;		//送信タイムアウト用 'v14.14変更 by 間々田 2008/02/20
		private static DateTime? mtmrCtrlXray_Tick_CommTimeOut;		//タイムアウト用
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
		private static int Avail_intXrayAvailOnRun;
		private static int Avail_intXrayAvailInsideRun;
		private static int Avail_intXrayAvailTime;
		private static int Avail_intXrayAvailOk;
		private static int Avail_intOldXrayVoltage;
		private static int Avail_intOldXrayCurent;
		private static DateTime Avail_varXrayAvailStartTime;

		/// <summary>
		/// 
		/// </summary>
		public clsCtrlXrayL8121_02()
		{
			lock (cCtrlm.gLock)
			{
				Class_Initialize_();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
            //変更2014/06/09_hata
            //排他処理しない
            //lock (cCtrlm.gLock)
            //{
            //    Class_Terminate();
            //}
            Class_Terminate();
		}

		/// <summary>
		/// クラスイニシャライズ
		/// </summary>
		private void Class_Initialize_()
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
				'----- X線装置制御用フォームの設定 -----
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
					mcomCtrlXray.BaudRate = 9600;
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

                    mcomCtrlXray.Open();

					//初期化
					modXray.gudtXrayP = udtXrayPIni;

					//----- タイマ設定 -----

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
					Set mtmrCtrlXray = mfrmCtrlXray.tmrXray
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

					mtmrCtrlXray = new ThreadEx(new ThreadStart(mtmrCtrlXray_Timer));
                    mtmrCtrlXray.Name = this.ToString();

                    //追加2014/06/09_hata
                    CtrlXrayFlg = true;
                    CommEndFlg = false;
                    mtmrCtrlXray_Tick_blnTmrFlg = false;

                    mtmrCtrlXray.Start();

					modXray.gblnXrayOver = true;

                    //見直し書き方を変更_2014/10/07hata
					//ModuleCTRLM.ipX_type = 10;			//X線ﾀｲﾌﾟ(L8121-02)
                    ModuleCTRLM.ipX_type = modIniFiles.XRAY_TYPE_NO_150KV;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//					mintSendPointer = P_CTRL;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
				}

				modData.gintXrayClsCnt = modData.gintXrayClsCnt + 1;
			}
			catch
			{
				//----- エラー処理 -----
//				gintStatusNoValue = STS_R_XRAY_INITIALIZ
			}
		}

		/// <summary>
		/// クラスターミネイト
		/// </summary>
		private void Class_Terminate()
		{

            int CntEnd = 3000;  //待ち時間  //追加2014/06/09_hata

			try
			{
				modData.gintXrayClsCnt = modData.gintXrayClsCnt - 1;
				if (modData.gintXrayClsCnt == 0)
				{
					//X線ON逃げするため、X線終了させる
					CommOutput(CTRL_XRAY_OFF);		//X線OFF


                    //追加2014/06/09_hata
                    CommEndFlg = true;
                    CtrlXrayFlg = false;

                    ////----- タイマ終了 -----
                    //if (mtmrCtrlXray != null)
                    //{
                    //    mtmrCtrlXray.Stop();
                    //    mtmrCtrlXray = null;
                    //}

                    ////----- 通信終了 -----
                    //mcomCtrlXray.Close();
                    //mcomCtrlXray = null;

                    //----- 通信終了 -----
                    if (mcomCtrlXray.IsOpen)
                    {
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
                    //CD (Carrier Detect) シグナルの状態が変化しました。
                    //このシグナルは、モデムが動作中の電話回線に接続され、
                    //データ キャリア シグナルが検出されるかどうかを示すために使用されます。

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
                    //入力時にブレークが検出されました。
                    //Breakが検出された
                    data = "Break On";
                    break;

                case SerialPinChange.Ring:
                    //リング インジケーターが検出されました。モデム動作
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
            FileInfo finfo = new FileInfo(file + ext);
            try
            {
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

            //変更2015/03/04hata
            ////変更2015/02/06hata
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
            FileInfo finfo = new FileInfo(file + ext);
            try
            {

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

            
            //変更2015/03/04hata
            //変更2015/02/06hata
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
		///   状態監視
		///   <概略>
		///   OnComm ｲﾍﾞﾝﾄを使用して、通信ｲﾍﾞﾝﾄおよびｴﾗｰをﾄﾗｯﾌﾟします。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mcomCtrlXray_OnComm(object sender, EventArgs e)
		{
			string strData = null;
			string strCommand = null;
			TimeSpan sngTime = TimeSpan.Zero;
			float sngDelayTime = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			Dim fileno As Integer 'v19.40 追加 by長野 2013/10/23
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			int iVal = 0;

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

				    modXray.gintOldErrXrayComm = modData.CTRL_OFF;		//通信異常フラグクリア

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

				    //----- 受信待ち分岐 -----
				    switch (mstrCommFlg)
				    {
					    //----- コントロールコマンド ------------------------------
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
						    // 管電流設定[uA]   (0～250)

						    //エラーの場合は、現在値でイベントを発生させる
						    if (mstrRecvBuf.StartsWith(STAT_ERROR) == true)
						    {
							    modXray.gintXraySetValueError = modData.CTRL_ON;
						    }
						    break;


					    case CTRL_SRL:
						    //----- ｵﾍﾟﾚｰﾄｽｲｯﾁ状態確認 -----

						    strData = mstrRecvBuf;
						    switch (strData)
						    {
							    case STAT_OFF:
								    modXray.ipXrayOperateSRL = 0;			//OFF
								    break;
							    case STAT_REMOTE:
								    modXray.ipXrayOperateSRL = 1;			//REMOTE
								    break;
							    case STAT_LOCAL:
								    modXray.ipXrayOperateSRL = 2;			//LOCAL
								    break;
							    default:
								    modXray.ipXrayOperateSRL = 0;
								    break;
						    }
						    break;

					    case CTRL_SRB:
						    //----- リモート動作状態確認 -----

						    strData = mstrRecvBuf;
						    switch (strData)
						    {
							    case STAT_BUSY:
								    modXray.ipXrayRemoteSRB = 0;			//BUSY
								    break;
							    case STAT_READY:
								    modXray.ipXrayRemoteSRB = 1;			//READY
								    break;
							    default:
								    modXray.ipXrayRemoteSRB = 0;			//BUSY
								    break;
						    }
						    break;


					    case CTRL_XRAY_ON:					//----- Ｘ線出射開始 -----
						    break;
					    case CTRL_XRAY_OFF:					//----- Ｘ線出射停止 -----
						    break;
					    case CTRL_RST:						//----- 過負荷保護解除 -----
						    break;
					    case CTRL_STX:						//----- X線照射確認 -----
						    break;
					    case CTRL_SOV:						//----- 過負荷保護機能確認 -----
						    break;
					    case CTRL_SAG:						//----- ウォーミングアップ状態確認 -----
						    break;
					    case CTRL_MODE_FOCUS_S:				//----- ﾌｫｰｶｽﾓｰﾄﾞｾｯﾄ  (SMALL)  -----
						    break;
					    case CTRL_MODE_FOCUS_M:				//----- ﾌｫｰｶｽﾓｰﾄﾞｾｯﾄ  (MIDDLE) -----
						    break;
					    case CTRL_MODE_FOCUS_L:				//----- ﾌｫｰｶｽﾓｰﾄﾞｾｯﾄ  (LARGE)  -----
						    break;


					    case CTRL_SHV:

						    //----- 動作管電圧値確認 -----
						    if (mstrRecvBuf.StartsWith(STAT_ERROR) == true)
						    {
							    goto ExitHandler;
						    }

						    //追加2012/03/25(KS1)hata_違う文字が戻る（入る）ため
						    //Debug.Print(mstrCommFlg + "  " + mstrRecvBuf);
						    if (mstrRecvBuf.IndexOf(STAT_SHV) != 0)
						    {
							    goto ExitHandler;
						    }

						    //管電圧の動作値を0.0kv～90.0kVで返します。
						    strCommand = mstrRecvBuf.Substring(0, 3);
						    strData = mstrRecvBuf.Substring(4, mstrRecvBuf.Length - 6);
						    if (strCommand == STAT_SHV)
						    {
							    float.TryParse(strData, out ModuleCTRLM.ipX_Volt);			//管電圧値
						    }
						    break;

					    case CTRL_SCU:

						    //----- 動作管電流値確認 -----
						    if (mstrRecvBuf.StartsWith(STAT_ERROR) == true)
						    {
							    goto ExitHandler;
						    }

						    //追加2012/03/25(KS1)hata_違う文字が戻る（入る）ため
						    //Debug.Print(mstrCommFlg + "  " + mstrRecvBuf);
						    if (mstrRecvBuf.IndexOf(STAT_SCU) != 0)
						    {
							    goto ExitHandler;
						    }

						    //管電流の動作値を0kV～250uAで返します。
						    strCommand = mstrRecvBuf.Substring(0, 3);
						    strData = mstrRecvBuf.Substring(4, mstrRecvBuf.Length - 6);
						    if (strCommand == STAT_SCU)
						    {
							    float.TryParse(strData, out ModuleCTRLM.ipX_Amp);		//管電流値
						    }
						    break;

					    case CTRL_SPV:

						    //----- 管電圧プリセット確認 -----
						    if (mstrRecvBuf.StartsWith(STAT_ERROR) == true)
						    {
							    goto ExitHandler;
						    }

						    //追加2012/03/25(KS1)hata_違う文字が戻る（入る）ため
						    //        Debug.Print mstrCommFlg; "  "; mstrRecvBuf
						    if (mstrRecvBuf.IndexOf(STAT_SPV) != 0)
						    {
							    goto ExitHandler;
						    }

						    //設定管電圧を取得する。
						    if (modXray.gsngTmpVol == ModuleCTRLM.ifX_Volt)
						    {
							    //----- 設定管電圧確認 -----
							    //管電圧の動作値を0.0kv～90.0kVで返します。

							    strCommand = mstrRecvBuf.Substring(0, 3);
							    strData = mstrRecvBuf.Substring(4, mstrRecvBuf.Length - 6);

							    if (strCommand == STAT_SPV)
							    {
								    float.TryParse(strData, out ModuleCTRLM.ipcndVolt);		//設定管電圧
								    ModuleCTRLM.ifX_Volt = ModuleCTRLM.ipcndVolt;
								    modXray.gsngTmpVol = ModuleCTRLM.ifX_Volt;
							    }

							    if (modXray.gblnXrayValue == true)
							    {
								    modXray.gintXrayValueDisp = 1;
								    modXray.gblnXrayValue = false;
							    }
						    }
						    break;

					    case CTRL_SPC:

						    //----- 管電流プリセット確認 -----
						    if (mstrRecvBuf.StartsWith(STAT_ERROR) == true)
						    {
							    goto ExitHandler;
						    }

						    //追加2012/03/25(KS1)hata_違う文字が戻る（入る）ため
						    //        Debug.Print mstrCommFlg; "  "; mstrRecvBuf
						    if (mstrRecvBuf.IndexOf(STAT_SPC) != 0)
						    {
							    goto ExitHandler;
						    }

						    //設定管電流を取得する。
						    if (modXray.gsngTmpAmp == ModuleCTRLM.ifX_Amp)
						    {
							    //管電流の動作値を0kV～250uAで返します。

							    strCommand = mstrRecvBuf.Substring(0, 3);
							    strData = mstrRecvBuf.Substring(4, mstrRecvBuf.Length - 6);

							    if (strCommand == STAT_SPC)
							    {
								    float.TryParse(strData, out ModuleCTRLM.ipcndAmp);		//設定管電流
								    ModuleCTRLM.ifX_Amp = ModuleCTRLM.ipcndAmp;
								    modXray.gsngTmpAmp = ModuleCTRLM.ifX_Amp;
							    }

							    if (modXray.gblnXrayValue == true)
							    {
								    modXray.gintXrayValueDisp = 1;
								    modXray.gblnXrayValue = false;
							    }
						    }
						    break;

					    case CTRL_SIN:
						    //----- インタロック確認 -----

						    strCommand = mstrRecvBuf;
						    switch (strCommand)
						    {
							    case STAT_RET_INTER_LOCK_ON:
								    ModuleCTRLM.ipInterlock = 1;
								    break;
							    case STAT_RET_INTER_LOCK_OFF:
								    ModuleCTRLM.ipInterlock = 0;
								    break;
						    }
						    break;

					    case CTRL_DAG:

						    //----- ウォーミングアップステップと完了確認 -----
						    if (mstrRecvBuf.StartsWith(STAT_ERROR) == true)
						    {
							    goto ExitHandler;
						    }

						    //追加2012/03/25(KS1)hata_違う文字が戻る（入る）ため
						    //        Debug.Print mstrCommFlg; "  "; mstrRecvBuf
						    if (mstrRecvBuf.IndexOf(STAT_AGING) != 0)
						    {
							    goto ExitHandler;
						    }

						    strCommand = mstrRecvBuf.Substring(0, 5);
						    if (strCommand == STAT_AGING)
						    {
							    strData = mstrRecvBuf.Substring(6, 1) + mstrRecvBuf.Substring(8, 1);
							    int.TryParse(strData, out modData.gintSWS);

							    int.TryParse(mstrRecvBuf.Substring(6, 1), out iVal);

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

							    //ｳｫｰﾑｱｯﾌﾟ終了確認
							    strData = mstrRecvBuf.Substring(10, 3);
							    if (strData == STAT_END)
							    {
								    iWarmUPEND = 1;				//WARMUP_END
							    }
							    else if (strData == STAT_YET)
							    {
								    iWarmUPEND = 0;				//WARMUP_YET
							    }
						    }
						    break;

					    case CTRL_STS:

						    //----- 状態確認 -----
						    strData = mstrRecvBuf;
						    switch (strData)
						    {
							    case STAT_AGING:
								    modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
								    ModuleCTRLM.ipWarmup = 1;				//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ中
								    ModuleCTRLM.ipX_On = modData.CTRL_ON;	//X線ON中
								    ModuleCTRLM.ipXStatus = 3;				//WARMUP中
								    ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ完了
								    break;

							    case STAT_STANDBY:
								    modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
								    ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線OFF中
								    ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ完了
								    if (iWarmUPEND == 1)
								    {
									    //ｳｫｰﾐﾝｸﾞｱｯﾌﾟENDの場合
									    ModuleCTRLM.ipWarmup = 2;
									    ModuleCTRLM.ipXStatus = 6;			//STANDBY
								    }
								    else
								    {
									    //ｳｫｰﾐﾝｸﾞｱｯﾌﾟYETの場合
									    ModuleCTRLM.ipWarmup = 0;
									    ModuleCTRLM.ipXStatus = 5;			//WARMUP_YET
								    }
								    break;

							    case STAT_XON:
								    modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
								    ModuleCTRLM.ipX_On = modData.CTRL_ON;	//X線ON中
								    ModuleCTRLM.ipWarmup = 2;				//WARMUP完了
								    ModuleCTRLM.ipXStatus = 4;				//X線ON中
								    ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ完了
								    break;

							    case STAT_OVER:
								    modXray.gblnXrayOver = false;			//荷保護機能動作中ﾌﾗｸﾞ
								    ModuleCTRLM.ipXStatus = 7;
								    ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ完了
								    mintSovOver = modData.CTRL_ON;			//解除指令
								    break;

							    case STAT_NOT_READY:
								    modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
								    ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線が照射できない状態
								    ModuleCTRLM.ipXStatus = 2;				//NOT_READY
								    ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ完了
								    break;

							    case STAT_ERROR:
								    modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
								    ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//ﾌﾟﾘﾋｰﾄ中
								    ModuleCTRLM.ipXStatus = 1;				//ﾌﾟﾘﾋｰﾄ中
								    ModuleCTRLM.ipXPermitWarmup = 0;		//ﾌﾟﾘﾋｰﾄ中
								    break;

							    default:
								    ModuleCTRLM.ipXStatus = 0;
								    break;

						    }

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

						    //ウォーミングアップ状態
						    switch (ModuleCTRLM.ipWarmup)
						    {
							    case 2:
								    modXray.gudtXrayP.intSWE = 0;		//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ完了
								    break;
							    case 1:
								    modXray.gudtXrayP.intSWE = 1;		//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ中
								    break;
							    case 0:
								    modXray.gudtXrayP.intSWE = 2;		//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ未実施
								    break;
							    default:
								    modXray.gudtXrayP.intSWE = 2;
								    break;
						    }
						    break;

    //削除2012/03/20(KS1)hata_L8121-02対応
    //						'焦点モードは固定
    //						If ifFocussize <> 0 Then
    //							ifFocussize = 0
    //							ifFocussize = ifFocussize
    //						End If

					    //追加2012/03/20(KS1)hata_L8121-02対応
					    case CTRL_SFS:
						    //----- 焦点モード確認 -----
						    if (mstrRecvBuf.StartsWith(STAT_ERROR) == true)
						    {
							    goto ExitHandler;
						    }
						    strData = mstrRecvBuf;
						    switch (strData)
						    {
							    case STAT_SFC_1:
								    ModuleCTRLM.ipFocussize = 0;
								    break;
							    case STAT_SFC_2:
								    ModuleCTRLM.ipFocussize = 1;
								    break;
							    case STAT_SFC_3:
								    ModuleCTRLM.ipFocussize = 2;
								    break;
						    }
						    break;

					    case CTRL_SER:

						    //----- 制御基板異常確認 -----
						    if (mstrRecvBuf.StartsWith(STAT_ERROR) == true)
						    {
							    goto ExitHandler;
						    }

						    //追加2012/03/25(KS1)hata_違う文字が戻る（入る）ため
                            //Debug.Print mstrCommFlg; "  "; mstrRecvBuf
						    if (mstrRecvBuf.IndexOf(STAT_SER) != 0)
						    {
							    goto ExitHandler;
						    }

						    strCommand = mstrRecvBuf.Substring(0, 3);
						    if (strCommand == STAT_SER)
						    {
							    strData = mstrRecvBuf.Substring(4, 1);

							    if (int.TryParse(strData, out iVal) == true)
							    {
								    modXray.gudtXrayP.intSER = iVal;
							    }

							    //0:正常
							    //1～4:ﾘﾚｰまたは内部故障1
							    switch (modXray.gudtXrayP.intSER)
							    {
								    case 1:		//ﾘﾚｰまたは内部故障1
									    mcomCtrlXray_OnComm_intErr1 = modData.CTRL_ON;
									    if (mcomCtrlXray_OnComm_sngTimCont1 == TimeSpan.Zero)
									    {
										    mcomCtrlXray_OnComm_sngTimCont1 = DateTime.Now.TimeOfDay;
									    }
									    break;
								    case 2:		//ﾘﾚｰまたは内部故障2
									    mcomCtrlXray_OnComm_intErr2 = modData.CTRL_ON;
									    if (mcomCtrlXray_OnComm_sngTimCont2 == TimeSpan.Zero)
									    {
										    mcomCtrlXray_OnComm_sngTimCont2 = DateTime.Now.TimeOfDay;
									    }
									    break;
								    case 3:		//ﾘﾚｰまたは内部故障3
									    mcomCtrlXray_OnComm_intErr3 = modData.CTRL_ON;
									    if (mcomCtrlXray_OnComm_sngTimCont3 == TimeSpan.Zero)
									    {
										    mcomCtrlXray_OnComm_sngTimCont3 = DateTime.Now.TimeOfDay;
									    }
									    break;
								    case 4:		//ﾘﾚｰまたは内部故障4
									    mcomCtrlXray_OnComm_intErr4 = modData.CTRL_ON;
									    if (mcomCtrlXray_OnComm_sngTimCont4 == TimeSpan.Zero)
									    {
										    mcomCtrlXray_OnComm_sngTimCont4 = DateTime.Now.TimeOfDay;
									    }
									    break;
								    default:
									    mcomCtrlXray_OnComm_intErr1 = modData.CTRL_OFF;
									    mcomCtrlXray_OnComm_intErr2 = modData.CTRL_OFF;
									    mcomCtrlXray_OnComm_intErr3 = modData.CTRL_OFF;
									    mcomCtrlXray_OnComm_intErr4 = modData.CTRL_OFF;
									    mcomCtrlXray_OnComm_sngTimCont1 = TimeSpan.Zero;
									    mcomCtrlXray_OnComm_sngTimCont2 = TimeSpan.Zero;
									    mcomCtrlXray_OnComm_sngTimCont3 = TimeSpan.Zero;
									    mcomCtrlXray_OnComm_sngTimCont4 = TimeSpan.Zero;
									    break;
							    }

							    sngDelayTime = 20;		//20秒毎に表示
							    sngTime = DateTime.Now.TimeOfDay;
							    if ((sngTime - mcomCtrlXray_OnComm_sngTimCont1).TotalSeconds > sngDelayTime && mcomCtrlXray_OnComm_intErr1 == modData.CTRL_ON)
							    {
								    //Err表示
								    ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR31;
								    mcomCtrlXray_OnComm_sngTimCont1 = TimeSpan.Zero;
							    }
							    else if (sngTime < mcomCtrlXray_OnComm_sngTimCont1)
							    {
								    mcomCtrlXray_OnComm_sngTimCont1 = TimeSpan.Zero;
							    }
							    if ((sngTime - mcomCtrlXray_OnComm_sngTimCont2).TotalSeconds > sngDelayTime && mcomCtrlXray_OnComm_intErr2 == modData.CTRL_ON)
							    {
								    //Err表示
								    ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR32;
								    mcomCtrlXray_OnComm_sngTimCont2 = TimeSpan.Zero;
							    }
							    else if (sngTime < mcomCtrlXray_OnComm_sngTimCont2)
							    {
								    mcomCtrlXray_OnComm_sngTimCont2 = TimeSpan.Zero;
							    }
							    if ((sngTime - mcomCtrlXray_OnComm_sngTimCont3).TotalSeconds > sngDelayTime && mcomCtrlXray_OnComm_intErr3 == modData.CTRL_ON)
							    {
								    //Err表示
								    ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR33;
								    mcomCtrlXray_OnComm_sngTimCont3 = TimeSpan.Zero;
							    }
							    else if (sngTime < mcomCtrlXray_OnComm_sngTimCont3)
							    {
								    mcomCtrlXray_OnComm_sngTimCont3 = TimeSpan.Zero;
							    }
							    if ((sngTime - mcomCtrlXray_OnComm_sngTimCont4).TotalSeconds > sngDelayTime && mcomCtrlXray_OnComm_intErr4 == modData.CTRL_ON)
							    {
								    //Err表示
								    ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR34;
								    mcomCtrlXray_OnComm_sngTimCont4 = TimeSpan.Zero;
							    }
							    else if (sngTime < mcomCtrlXray_OnComm_sngTimCont4)
							    {
								    mcomCtrlXray_OnComm_sngTimCont4 = TimeSpan.Zero;
							    }
						    }
						    break;

					    case CTRL_CHECK_COMM:
						    if (mstrRecvBuf == STAT_ERROR)
						    {
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


ExitHandler:

				mstrCommFlg = "";
				mstrRecvBuf = "";
			}		// lock(cCtrlm.gLock)
		}

		//=======================================================================================
		//   コマンド送信
		//
		//=======================================================================================
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

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				string strBuff = null;

				Static blnTmrFlg    As Boolean

				Static intSendPointer       As Integer
				'Static lngSendTime          As Long     '送信タイムアウト用
				Static varSendTime          As Variant  '送信タイムアウト用 'v14.14変更 by 間々田 2008/02/20
				Static lngReceiveTime       As Long     '受信タイムアウト用
				Static CommTimeOut      As Variant  'タイムアウト用
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                mtmrCtrlXray.Sleep(Interval);//追加2014/06/09_hata
                if (!CtrlXrayFlg) break;   //追加2014/06/09_hata

                if (mtmrCtrlXray == null) return;
                if (mtmrCtrlXray.Stoped) return;
                if (!mtmrCtrlXray.IsAlive) return;

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
                                //TimeOutCommandString =  mstrCommFlg; //追加2015/03/02hata
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
//					'----- タイムアウト -----
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

                            //----- 送信タイムアウト -----
//							If DateDiff("s", varSendTime, Now) >= COMM_TIMEOUT Then             'v14.14追加 by 間々田 2008/02/20
							//v14.14追加 by 間々田 2008/02/20
							if ((DateTime.Now - mtmrCtrlXray_Tick_varSendTime).TotalSeconds >= COMM_REPEAT_TIME)
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

								if ((mtmrCtrlXray_Tick_CommTimeOut != null) && (DateTime.Now - mtmrCtrlXray_Tick_CommTimeOut.Value).TotalSeconds > COMM_TIMEOUT)
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
//						End If
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
								blnRet = CommOutput(strCommand);		//コマンド送信
								if (blnRet == false)
								{
									throw new Exception();
								}
								//lngSendTime = Timer '送信した時刻を格納
								mtmrCtrlXray_Tick_varSendTime = DateTime.Now;		//送信した時刻を格納  'v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
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
							case P_STX:		//X線照射確認
								mstrCommFlg = CTRL_STX;
								strCommand = mstrCommFlg;
								break;

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

							case P_SOV:		//過負荷保護機能確認
								mstrCommFlg = CTRL_SOV;
								strCommand = mstrCommFlg;
								break;

							case P_SIN:		//インタロック確認
								mstrCommFlg = CTRL_SIN;
								strCommand = mstrCommFlg;
								break;

							case P_SAG:		//ウォーミングアップ状態確認
								mstrCommFlg = CTRL_SAG;
								strCommand = mstrCommFlg;
								break;
//							Case P_SWR  'ウォーミングアップ状態確認
//								mstrCommFlg = CTRL_SWR
//								strCommand = mstrCommFlg

							case P_STS:		//状態確認
								mstrCommFlg = CTRL_STS;
								strCommand = mstrCommFlg;
								break;
//							Case P_STI  '状態確認
//								mstrCommFlg = CTRL_STI
//								strCommand = mstrCommFlg

//							Case P_SXT  'X線照射時間確認
//								mstrCommFlg = CTRL_SXT
//								strCommand = mstrCommFlg

							case P_DAG:		//ウォーミングアップステップ確認と終了確認
								mstrCommFlg = CTRL_DAG;
								strCommand = mstrCommFlg;
								break;
//							Case P_SWS  'ウォーミングアップステップ確認
//								mstrCommFlg = CTRL_SWS
//								strCommand = mstrCommFlg
//
//							Case P_SWE  'ウォーミングアップ終了確認
//								mstrCommFlg = CTRL_SWE
//								strCommand = mstrCommFlg
//
//							Case P_SFC '焦点モード確認
//								mstrCommFlg = CTRL_SFC
//								strCommand = mstrCommFlg
//
							case P_SFS:		//焦点モード確認
								mstrCommFlg = CTRL_SFS;
								strCommand = mstrCommFlg;
								break;

//							Case P_STM 'X線源通電時間確認
//								mstrCommFlg = CTRL_STM
//								strCommand = mstrCommFlg
//
//							Case P_SAT '自動X線停止機能確認
//								mstrCommFlg = CTRL_SAT
//								strCommand = mstrCommFlg
//
//							Case P_TYP '型名確認
//								mstrCommFlg = CTRL_TYP
//								strCommand = mstrCommFlg

							case P_SER:		//制御基板異常確認
								mstrCommFlg = CTRL_SER;
								strCommand = mstrCommFlg;
								break;

//							Case P_STC '一括状態確認
//								mstrCommFlg = CTRL_STC
//								strCommand = mstrCommFlg
//
//							Case P_STT '動作管電圧管電流カソード値確認
//								mstrCommFlg = CTRL_STT
//								strCommand = mstrCommFlg
//
//							Case P_SVI '設定電圧値電流値確認
//								mstrCommFlg = CTRL_SVI
//								strCommand = mstrCommFlg

							case P_SRL:		//ｵﾍﾟﾚｰﾄｽｲｯﾁ状態確認
								mstrCommFlg = CTRL_SRL;
								strCommand = mstrCommFlg;
								break;

							case P_SRB:		//リモート動作状態確認
								mstrCommFlg = CTRL_SRB;
								strCommand = mstrCommFlg;
								break;

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

									}
									else if ((ModuleCTRLM.ifXrayonoff_Set == 1 && ModuleCTRLM.ipWarmup == 0) || 
											 (ModuleCTRLM.ifXrayonoff_Set == 0 && ModuleCTRLM.ipWarmup == 0 && modXray.gudtXrayM.intWarmUp > 0))
									{
										//----- ウォームアップ開始コマンド送信 -----
										strCommand = CTRL_XRAY_ON;
										mstrCommFlg = CTRL_XRAY_ON;
										ModuleCTRLM.ifXrayonoff_Set = 0;
										modXray.gudtXrayM.intWarmUp = 0;

										goto SensCommand;
									}
									else if (ModuleCTRLM.ipWarmup == 2 && modXray.gudtXrayM.intWarmUp > 0)
									{
										modXray.gudtXrayM.intWarmUp = 0;
										ModuleCTRLM.ifXrayonoff_Set = 0;		//追加10/05/12(KS1)hata_L9421-02対応
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
									if (ModuleCTRLM.ifX_Volt != ModuleCTRLM.ipcndVolt && modXray.gsngTmpVol != ModuleCTRLM.ifX_Volt)
									{
										//Debug.Print("管電圧");

										//設定管電圧値に変化がある時（X線発生装置から取得した設定管電圧値と比較）
										strData = Convert.ToString(ModuleCTRLM.ifX_Volt * 10);				// 設定値取得(kV)
										if (ModuleCTRLM.ipX_On == modData.CTRL_ON)
										{
											strCommand = CTRL_HIVOLT + strData;					// X線ON中の電圧値の設定コマンド生成
											mstrCommFlg = CTRL_HIVOLT;
										}
										else
										{
											strCommand = CTRL_PHVOLT + strData;					// X線OFF中の電圧値の設定コマンド生成
											mstrCommFlg = CTRL_PHVOLT;
										}
										modXray.gsngTmpVol = ModuleCTRLM.ifX_Volt;
										modXray.gblnXrayValue = true;
										goto SensCommand;
									}

									//----- 管電流値の送信 -----
									if (ModuleCTRLM.ifX_Amp != ModuleCTRLM.ipcndAmp)
									{
										//Debug.Print("管電流");

										//設定管電流値に変化がある時（X線発生装置から取得した設定管電流値と比較）
										strData = Convert.ToString(ModuleCTRLM.ifX_Amp);
										// 設定値取得(uA)
										if (ModuleCTRLM.ipX_On == modData.CTRL_ON)
										{
											strCommand = CTRL_CURRENT + strData;				// X線ON中の電流値の設定コマンド生成
											mstrCommFlg = CTRL_CURRENT;
										}
										else
										{
											strCommand = CTRL_PCURRENT + strData;				// X線OFF中の電流値の設定コマンド生成
											mstrCommFlg = CTRL_PCURRENT;
										}
										modXray.gsngTmpAmp = ModuleCTRLM.ifX_Amp;
										modXray.gblnXrayValue = true;
										goto SensCommand;
									}

									//----- フォーカスモード送信 -----
									if ((ModuleCTRLM.ifFocussize != ModuleCTRLM.ipFocussize))
									{
										if (ModuleCTRLM.ifFocussize < 0 || ModuleCTRLM.ifFocussize > 3)
										{
											ModuleCTRLM.ifFocussize = 1;
										}
										else
										{
											switch (ModuleCTRLM.ifFocussize)
											{
												case 0:
													strCommand = CTRL_MODE_FOCUS_S;
													break;
												case 1:
													strCommand = CTRL_MODE_FOCUS_M;
													break;
												case 2:
													strCommand = CTRL_MODE_FOCUS_L;
													break;
												default:
													strCommand = CTRL_MODE_FOCUS_S;
													break;
											}
											mstrCommFlg = strCommand;

											//Debug.Print("フォーカスモード" + strCommand);
										}

										goto SensCommand;
									}
								}

								//----- 過負荷保護解除 -----
								if (mintSovOver == modData.CTRL_ON || modXray.gudtXrayM.intRST == modData.CTRL_ON)
								{
									modXray.gudtXrayM.intRST = modData.CTRL_OFF;
									mintSovOver = modData.CTRL_OFF;
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
							mtmrCtrlXray_Tick_varSendTime = DateTime.Now;		//送信した時刻を格納  'v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
						}

						//----- ポインタ管理 次に送信するコマンドにポインタ移動 -----
//						If gblnfrmXray = True Then
//
//							'----- X線情報表示フォーム表示中 -----
//						intSendPointer = intSendPointer + 1
//							If intSendPointer > P_SVI Then
//								intSendPointer = P_CTRL
//							End If
//
//						Else
							//----- X線情報表示フォーム非表示中 -----
							switch (mtmrCtrlXray_Tick_intSendPointer)
							{
								case P_CTRL:
									mtmrCtrlXray_Tick_intSendPointer = P_SRL;	// ｵﾍﾟﾚｰﾄｽｲｯﾁ状態確認
									break;
								case P_SRL:
									mtmrCtrlXray_Tick_intSendPointer = P_SRB;	// リモート動作状態確認
									break;
								case P_SRB:
									mtmrCtrlXray_Tick_intSendPointer = P_DAG;	// ウォーミングアップ終了確認
									break;
								case P_DAG:
									mtmrCtrlXray_Tick_intSendPointer = P_SAG;	// ウォーミングアップ状態確認
									break;
								case P_SAG:
									mtmrCtrlXray_Tick_intSendPointer = P_STS;	// 状態確認
									break;
								case P_STS:
									mtmrCtrlXray_Tick_intSendPointer = P_SHV;	// 出力電圧確認
									break;
								case P_SHV:
									mtmrCtrlXray_Tick_intSendPointer = P_SCU;	// 出力電流確認
									break;
								case P_SCU:
									mtmrCtrlXray_Tick_intSendPointer = P_SPV;	// 管電圧プリセット確認
									break;
								case P_SPV:
									mtmrCtrlXray_Tick_intSendPointer = P_SPC;	// 管電流プリセット確認
									break;
								case P_SPC:
									mtmrCtrlXray_Tick_intSendPointer = P_SIN;	// インターロック確認
									break;

								case P_SIN:
									mtmrCtrlXray_Tick_intSendPointer = P_SFS;	// フォーカスモード確認
									break;
								case P_SFS:
									mtmrCtrlXray_Tick_intSendPointer = P_SER;	// 制御基板異常確認
									break;

//								Case P_SIN:     intSendPointer = P_STX  ' Ｘ線照射確認
//								Case P_STX:     intSendPointer = P_SOV  ' 過負荷保護機能確認
//								Case P_SOV:     intSendPointer = P_SER  ' 制御基板異常確認

								case P_SER:
									mtmrCtrlXray_Tick_intSendPointer = P_CTRL;	// コントロールコマンド
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
					catch		//エラー処理
					{
                        //変更2015/02/07hata
                        CommTimeOutError = true;

//                        if (modXray.gintOldErrXrayComm == modData.CTRL_OFF)
//                        {
////							gintStatusNoValue = STS_XRAY_COMM   '通信エラー
//                            modXray.gintOldErrXrayComm = modData.CTRL_ON;
//                            ModuleCTRLM.ipErrsts = modData.ERR_XRAY_COMM;
//                        }

//                        StreamWriter sw;
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
                            FileInfo finfoR = new FileInfo(file + ext);
                            try
                            {
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
                            FileInfo finfoS = new FileInfo(file + ext);
                            try
                            {
                                if (finfoS.Exists)
                                {
                                    if (finfoS.Length > 10000000)
                                    {
                                        //10Mを超える場合は名前の後ろに日付を付けて保管
                                        string st = DateTime.Now.ToString("yyyyMMdd'-'HH'.'mm'.'ss'.'fffffff");
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
                if (mtmrCtrlXray == null) return;
                if (!mtmrCtrlXray.IsAlive) return;
                if (mtmrCtrlXray.Stoped) return;

			}
            mtmrCtrlXray_Tick_blnTmrFlg = false;//追加2014/06/09_hata

		}

		//==============================================================================
		//
		//   X線ON中エラー確認
		//
		//==============================================================================
		//変更2012/03/25(KS1)hata_違う文字が戻る（入る）ため
		private int ChkXray()
//		Private Function ChkXray()
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

		//==============================================================================
		//
		//   自動X線OFF
		//
		//==============================================================================
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
						AutoOff_varTimXray = DateTime.Now;	//v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
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

		//==============================================================================
		//
		//   アベイラブル
		//
		//==============================================================================
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
					Avail_intXrayAvailOnRun = modData.CTRL_ON;										//X線ONアベイラブル判定スタート
					Avail_intXrayAvailTime = modData.gudtXraySystemValue.E_XrayAvailTimeOn;	//X線ONアベイラブル時間をセット
					Avail_intXrayAvailOk = modData.CTRL_OFF;

					Avail_intOldXrayOnAvail = ModuleCTRLM.ipX_On;

					Avail_intOldXrayVoltage = (int)ModuleCTRLM.ipcndVolt;		//X線ON時には、管電圧・管電流アベイラブル判定を行わない
					Avail_intOldXrayCurent = (int)ModuleCTRLM.ipcndAmp;		//X線ON時には、管電圧・管電流アベイラブル判定を行わない
				}

				//設定値変更監視
				if (Avail_intOldXrayVoltage != ModuleCTRLM.ipcndVolt || Avail_intOldXrayCurent != ModuleCTRLM.ipcndAmp)
				{
					Avail_intXrayAvailInsideRun = modData.CTRL_ON;								//設定値変更アベイラブルスタート
					Avail_intXrayAvailTime = modData.gudtXraySystemValue.E_XrayAvailTimeInside;	//設定値変更アベイラブル時間をセット
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

//			End With

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			With gudtXraySystemValue
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				//アベイラブル時間経過判定
				//If intXrayAvailOk = CTRL_ON _
				//'And (Timer - sngXrayAvailStartTime) >= intXrayAvailTime Then

				//v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
				if (Avail_intXrayAvailOk == modData.CTRL_ON &&
					(DateTime.Now - Avail_varXrayAvailStartTime).TotalSeconds >= Avail_intXrayAvailTime)
				{
					ModuleCTRLM.ipXAvail = 1;	//アベイラブルON
					Avail_intXrayAvailOnRun = modData.CTRL_OFF;			//アベイラブル判定停止
					Avail_intXrayAvailInsideRun = modData.CTRL_OFF;		//アベイラブル判定停止

				}
				else
				{
					ModuleCTRLM.ipXAvail = 0;	//アベイラブルOFF
				}

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				End With
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

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
