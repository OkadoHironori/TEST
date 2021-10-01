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
	//   X線制御160kV（L10711）
	//
	//   Date        Version     Designed/Changed
	//   2015/09/29  1.00        (KS1)Chouno
	//
	//***********************************************************************************
	internal class clsCtrlXrayL10711 : IDisposable
	{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		Private mfrmCtrlXray                As frmXray                      ' X線制御用フォーム

		Private WithEvents mtmrCtrlXray     As Timer                        ' タイマ
		Private WithEvents mcomCtrlXray     As MSComm                       ' 通信
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private ThreadEx mtmrCtrlXray;						// スレッド

		private SerialPort mcomCtrlXray;					// 通信

		private const int Interval = 20;					// スレッドインターバル

		private string mstrCommFlg;							// 通信ハンドシェイク用
		private int mintSovOver;							// 過負荷保護機能状態


		private const int COMM_PORT = 1;					// 通信ポート

        //追加2014/06/09_hata
        private bool CommEndFlg = false;	        // 通信停止要求用
        private bool CtrlXrayFlg = false;	        // スレッドONフラグ
        private ManualResetEvent SerialRead = new ManualResetEvent(false);        //通信read時の待ち認識用
        //追加2015/02/07hata
        private bool CommLineError = false;	    // 通信ラインのエラー有無
        private bool CommDataError = false;	    // 通信データのエラー有無
        private bool CommTimeOutError = false;	// 通信のタイムアウトエラー有無       

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		Private Const COMM_SETTINGS         As String = "38400,n,8,1"       ' ボーレート
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//Private Const COMM_TIMEOUT          As Integer = 10                 ' 返信タイムアウト時間[ms]
        //v19.40 送受信の仕組みを変更　ここから 2013/10/17 by長野
        //時間内に受信できなかったら、次の送信へ行く。
        //上記処理を一定時間繰り返すようであれば、タイムアウトにする。
		private const int COMM_TIMEOUT = 10;				// 返信タイムアウト時間[s]
		private const int COMM_REPEAT_TIME = 2;				//繰り返し時間
        //追加2015/02/07hata
        private const int COMM_LINE_TIMEOUT = 4;			// CommラインOFFによるタイムアウト時間[s]

        private bool RecieveSts;							//返信状態フラグ(True:正常、False:異常)
		private bool SendSts;								//送信状態フラグ(True:正常、False:異常)
		private bool LogWriteRFlg;
		private bool LogWriteSFlg;
        private string TimeOutCommandString;    //TimeOut時のCommand            

		private const int COMM_RETRY = 3;					// 通信リトライ回数


		private string mstrRecvBuf;

		//Private mAvailTimer                 As DelayType                    ' Availタイマ
		//Private mAutoOffTimer               As DelayType                    ' AutoOffタイマ

		//Private gintOldErrXrayComm          As Integer                      '通信異常フラグ
		//Private mblnCheckComm               As Boolean                      '通信確認用
		private int mintSendPointer;						//制御用

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private bool mblnGetType;				//X線タイプ取得済フラグ
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//----------------------------------------------------------------------------------------------
		//   X線装置コマンド
		//----------------------------------------------------------------------------------------------

		//----- コマンドポインタ -----
		private const int P_STS = 1;					// 状態確認
		private const int P_SAR = 2;					// 状態一括確認
		private const int P_SNR = 3;					// 「NOT RADY」一括確認
		private const int P_SHV = 4;					// 出力電圧確認
		private const int P_SCU = 5;					// 出力電流確認
		private const int P_SPV = 6;					// 管電圧プリセット確認
		private const int P_SPC = 7;					// 管電流プリセット確認
		private const int P_SVI = 8;					// 設定管電圧管電流値確認
		private const int P_STL = 9;					// 動作値一括確認
		private const int P_STG = 10;					// ターゲット電流確認
		private const int P_TGS = 11;					// ターゲット電流制限状態確認
		private const int P_SFC = 12;					// 焦点モード確認
		private const int P_SOB = 13;					// フォーカス値確認
		private const int P_SAD = 14;					// アライメントモニタ
		private const int P_SBX = 15;					// X軸方向アライメント確認
		private const int P_SBY = 16;					// Y軸方向アライメント確認
		private const int P_SVC = 17;					// 真空状態確認
		private const int P_SIN = 18;					// インターロック確認
		private const int P_FLM = 19;					// フィラメント状態確認
		private const int P_STM = 20;					// X線源通電時間確認
		private const int P_SXT = 21;					// X線照射時間確認
		private const int P_SHT = 22;					// フィラメントオン時間確認
		private const int P_SER = 23;					// 制御基板異常確認
		private const int P_SSA = 24;					// ステータス自動送信確認
		private const int P_TYP = 25;					// 型名確認
		private const int P_SVV = 26;					// 真空計モニタ
		private const int P_SAT = 27;					// 自動X線OFF時間
		private const int P_SHS = 28;					// フィラメント設定電圧確認(Wモード)
		private const int P_SHM = 29;					// フィラメント入力確認
		private const int P_SWE = 30;					// ウォームアップ状態
		private const int P_ZT1 = 31;					// ターゲット部の温度モニタ
		private const int P_SWU = 32;					// ウォーミングアップ管電圧上昇下降パラメータ確認
		private const int P_SMV = 33;					// 使用上限管電圧読み出し'v15.10追加 byやまおか 2009/11/12

        private const int P_SAP = 34;                   // アパーチャ電流確認 L10711対応 20080710
        private const int P_SCX = 35;                   // コンデンサX方向アライメント確認 L10711対応 20080710
        private const int P_SCY = 36;                   // コンデンサY方向アライメント確認 L10711対応 20080710
        private const int P_SMD = 37;                   // フィラメントモード確認 L10711対応 20080710
        private const int P_SVS = 38;                   // Sモードフィラメント設定電圧確認 L10711対応 20080715
        private const int P_SCW = 39;                   // コンデンサフォーカス値確認 L10711対応 20080822
       
		private const int P_CTRL = 0;					// コントロールコマンド
		//Private Const P_MAX                 As Integer = 32                 ' コマンド数

		private const int P_MAX = 39;					// コマンド数    'v15.10変更 byやまおか 2009/11/12

		//----- コントロールコマンド -----
		private const string CTRL_HIVOLT = "HIV ";		// 管電圧設定[V]    (0～230)
		private const string CTRL_CURRENT = "CUR ";		// 管電流設定[uA]   (0～1000)
		private const string CTRL_XRAY_ON = "XON";		// Ｘ線出射開始
		private const string CTRL_XRAY_OFF = "XOF";		// Ｘ線出射停止
		private const string CTRL_WARMUP = "WUP";		// ウォーミングアップ開始
		private const string CTRL_MODE_FOCUS = "CFS ";	// フォーカスモードセット  (0～2)
		private const string CTRL_OBJ = "OBJ ";			// フォーカス値設定（0～23000）
		private const string CTRL_SAV = "SAV";			// フォーカス調整値保存/アライメント値の保存
		private const string CTRL_ADA = "ADA";			// 一括ビームアライメント
		private const string CTRL_ADJ = "ADJ";			// ビームアライメント
		private const string CTRL_OBX = "OBX ";			// X方向アライメント設定
		private const string CTRL_OBY = "OBY ";			// Y方向アライメント設定
		private const string CTRL_STP = "STP";			// ビームアライメント中断
//2009/08/21_未使用_hata
		//Private Const CTRL_CHT              As String = "CHT"               ' フィラメントオン時間リセット
		private const string CTRL_RST = "RST";			// 過負荷保護解除
		private const string CTRL_ARS = "ARS";			// ステータス自動送信設定（0～10）
		private const string CTRL_WARMUPEND = "WUE";	// ウォーミングアップ強制終了
		private const string CTRL_DDL = "DDL";			// デフォルト値読み出し
		private const string CTRL_CMV = "CMV ";			// 使用上限管電圧制限(0～230)    'v15.10追加 byやまおか 2009/11/12

        private const string CTRL_CAX = "CAX ";         // コンデンサX方向アライメント設定 L10711対応 20080710
        private const string CTRL_CAY = "CAY ";         // コンデンサY方向アライメント設定 L10711対応 20080710
        private const string CTRL_MDE = "MDE ";         // フィラメントモード設定 L10711対応 20080710
        private const string CTRL_WUE = "WUE";          // ウォームアップ強制終了 L11091対応 20110814

		//----- ステータスコマンド -----
		private const string CTRL_STS = "STS";			// 状態確認
		private const string CTRL_SAR = "SAR";			// 状態一括確認
		private const string CTRL_SNR = "SNR";			// 「NOT RADY」一括確認
		private const string CTRL_SHV = "SHV";			// 出力電圧確認
		private const string CTRL_SCU = "SCU";			// 出力電流確認
		private const string CTRL_SPV = "SPV";			// 管電圧プリセット確認
		private const string CTRL_SPC = "SPC";			// 管電流プリセット確認
		private const string CTRL_SVI = "SVI";			// 設定管電圧管電流値確認
		private const string CTRL_STL = "STL";			// 動作値一括確認
		private const string CTRL_STG = "STG";			// ターゲット電流確認
//2009/08/21_未使用_hata
		//Private Const CTRL_TGS              As String = "TGS"               ' ターゲット電流制限状態確認
		private const string CTRL_SFC = "SFC";			// 焦点モード確認
		private const string CTRL_SOB = "SOB";			// フォーカス値確認
		private const string CTRL_SAD = "SAD";			// アライメントモニタ
		private const string CTRL_SBX = "SBX";			// X方向アライメント確認
		private const string CTRL_SBY = "SBY";			// Y方向アライメント確認
		private const string CTRL_SVC = "SVC";			// 真空状態確認
		private const string CTRL_SIN = "SIN";			// インターロック確認
		private const string CTRL_FLM = "FLM";			// フィラメント状態確認
		private const string CTRL_STM = "STM";			// X線源通電時間確認
		private const string CTRL_SXT = "SXT";			// X線照射時間確認
		private const string CTRL_SHT = "SHT";			// フィラメントオン時間確認
		private const string CTRL_SER = "SER";			// 制御基板異常確認
		private const string CTRL_SSA = "SSA";			// ステータス自動送信確認
		private const string CTRL_TYP = "TYP";			// 型名確認
		private const string CTRL_SVV = "SVV";			// 真空計モニタ
		private const string CTRL_SAT = "SAT";			// 自動X線OFF時間
		private const string CTRL_SHS = "SHS";			// Wモードフィラメント設定電圧確認
        
        private const string CTRL_SVS = "SVS";          // Sモードフィラメント設定電圧確認 L10711対応 20080715
		
        private const string CTRL_SHM = "SHM";			// フィラメント入力確認
		private const string CTRL_SWE = "SWE";			// ウォームアップ状態
		private const string CTRL_ZT1 = "ZT1";			// ターゲット部の温度モニタ
		private const string CTRL_SWU = "SWU";			// ウォーミングアップ管電圧上昇下降パラメータ確認
		private const string CTRL_SMV = "SMV";			// 使用上限管電圧読み出し    'v15.10追加 byやまおか 2009/11/12

        private const string CTRL_SAP = "SAP";          // アパーチャ電流確認 L10711対応 20080710
        private const string CTRL_SCX = "SCX";          // コンデンサX方向アライメント確認 L10711対応 20080710
        private const string CTRL_SCY = "SCY";          // コンデンサY方向アライメント確認 L10711対応 20080710
        private const string CTRL_SMD = "SMD";          // フィラメントモード確認 L10711対応 20080710
        private const string CTRL_SCW = "SCW";          // コンデンサフォーカス値確認 L10711対応 2008082

		//------------------------------------------------------------------------------------------

		//** ここは使わないかも？
		//フォーカスモード
		//メイン画面からの設定は1～3、制御機からの戻り値は0～2
		//X線制御機L9931対応で、制御機からの戻り値が変わってしまったため。
		//制御機からの戻り値
		private const int mcintStsSFC0 = 0;				//小焦点
		private const int mcintStsSFC1 = 1;				//中焦点
		private const int mcintStsSFC2 = 2;				//大焦点
		//メイン画面からの設定値
		private const int mcintSetSFC0 = 1;				//小焦点
		private const int mcintSetSFC1 = 2;				//中焦点
		private const int mcintSetSFC2 = 3;				//大焦点

		private const int mcintMaxCfs = 3;				//設定ﾌｫｰｶｽ最大値
		private const int mcintMinCfs = 1;				//設定ﾌｫｰｶｽ最小値

        //---L10711対応 20080710
        //制御機からの戻り値
        private const int mcintStsMDE1 = 0;//Wカソード
        private const int mcintStsMDE2 = 1;//Sカソード
        //メイン画面からの設定値
        private const int mcintSetMDE1 = 0;//Wカソード
        private const int mcintSetMDE2 = 1;//Sカソード
        //L10711対応 20080710---

		//------------------------------------------------------------------------------------------
		//アライメントモニタ
		private const int mcintStsSAD0 = 0;				//ビームアライメント停止状態
		private const int mcintStsSAD1 = 1;				//（一括）ビームアライメント中
		private const int mcintStsSAD2 = 2;				//ビームアライメント中に「XOF」以外の理由でX線照射が停止し、正常に終了しなかった。
		//------------------------------------------------------------------------------------------
		//アライメント状態
		private const int mcintSadOff = 0;				//停止
		private const int mcintSadRun = 1;				//動作中
		private const int mcintSadAll = 2;				//一括ビームアライメント中
		private const int mcintSadErr = 3;				//ビームアライメント中にXOF以外の理由でX線照射が停止
		//------------------------------------------------------------------------------------------
		//真空度
		private const int mcintStsSVC0 = 0;				//6.41～9.99（旧「Lo」）
		private const int mcintStsSVC1 = 1;				//6.11～6.40（旧「V1」）
		private const int mcintStsSVC2 = 2;				//5.61～6.10（旧「V2」）
		private const int mcintStsSVC3 = 3;				//5.31～5.60（旧「V3」）
		private const int mcintStsSVC4 = 4;				//4.61～5.30（旧「V4」）
		private const int mcintStsSVC5 = 5;				//1.01～4.60（旧「V5」）
		private const int mcintStsSVC6 = 6;				//0.00～1.00（旧「NG」）
		private const int mcintStsSVC7 = 7;				//真空度が「0」のまま10分以上経過した場合
		//------------------------------------------------------------------------------------------
		//インターロック
		private const int mcintStsSIN_ON = 0;			//インターロックが閉じています。
		private const int mcintStsSIN_OFF = 1;			//インターロックが開いています。
		//------------------------------------------------------------------------------------------
		//ウォームアップ状態
		private const int mcintStsSweEnd = 0;			//WARMUP END
		private const int mcintStsSweOn = 1;			//WARMUP ON
		private const int mcintStsSweYet = 2;			//WARMUP YET
		private const int mcintStsSweTime = 3;			//WARMUP TIME OVER

		//------------------------------------------------------------------------------------------
		private const string mcstrStsERR = "ERR";		//コマンドやパラメータに誤りがあったとき、または、コマンドを受け付けない状態の時に返信される

		private const int mcintMaxOBJ = 15000;			//ﾌｫｰｶｽ値 最大値
		
        private const int mcintMinOBJ = 0;				//ﾌｫｰｶｽ値 最小値
		private const int mcintMaxOBX = 1000;			//X方向ｱﾗｲﾒﾝﾄ 最大値
		private const int mcintMinOBX = -1000;			//X方向ｱﾗｲﾒﾝﾄ 最小値
		private const int mcintMaxOBY = 1000;			//Y方向ｱﾗｲﾒﾝﾄ 最大値
		private const int mcintMinOBY = -1000;			//Y方向ｱﾗｲﾒﾝﾄ 最小値
		private const int mcintMaxADJ = 3;				//ｱﾗｲﾒﾝﾄ 最大値
		private const int mcintMinADJ = 2;				//ｱﾗｲﾒﾝﾄ 最小値

        // L8321-01/L10711用 //L10711追加 20080710
        // コンデンサX方向アライメント値
        private const int mcintMaxCAX = 1000;
        private const int mcintMinCAX = -1000;
        // コンデンサY方向アライメント値
        private const int mcintMaxCAY = 1000;
        private const int mcintMinCAY = -1000;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private int mintInputMode;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//
		// mcomCtrlXray_OnComm イベントで使用するスタティックメンバ
		//
		private static int[] mcomCtrlXray_OnComm_intERR = new int[6];
		private static TimeSpan mcomCtrlXray_OnComm_sngTimCont1;
		private static TimeSpan mcomCtrlXray_OnComm_sngTimCont2;
		private static TimeSpan mcomCtrlXray_OnComm_sngTimCont3;
		private static TimeSpan mcomCtrlXray_OnComm_sngTimCont4;
		private static TimeSpan mcomCtrlXray_OnComm_sngTimCont5;
		private static TimeSpan mcomCtrlXray_OnComm_sngTimCont6;

		//
		// mtmrCtrlXray_Tick イベントで使用するスタティックメンバ
		//
		private static bool mtmrCtrlXray_Tick_blnTmrFlg;
		private static DateTime mtmrCtrlXray_Tick_varSendTime;	// 送信タイムアウト用 'v14.14変更 by 間々田 2008/02/20
		private static DateTime? mtmrCtrlXray_Tick_CommTimeOut;	// タイムアウト用

        //追加2015/04/03hata
        private static DateTime mtmrCtrlXray_LineErrTime;	// 送信タイムアウト用 'v14.14変更 by 間々田 2008/02/20
        private static int LineCount = 0 ;

        //
		// AutoOff メソッドで使用するスタティックメンバ
		//
		private static int AutoOff_intOldXrayOn;
		private static int AutoOff_intTimeStart;
		private static DateTime AutoOff_varTimXray;

		//
		// Avail メソッドで使用するスタティックメンバ
		//
		private static bool Avail_blnXrayOnOff;
		private static bool Avail_blnOldXrayOnOff;
		private static int Avail_intOldXrayOnAvail;
		private static int Avail_intXrayAvailOnRun;
		private static int Avail_intXrayAvailInsideRun;
		private static int Avail_intXrayAvailTime;
		private static int Avail_intXrayAvailOk;
		private static int Avail_intOldXrayVoltage;
		private static int Avail_intOldXrayCurent;
		private static int Avail_intOldXrayVoltageSPV;
		private static int Avail_intOldXrayCurentSPC;
		private static DateTime Avail_varXrayAvailStartTime;

		/// <summary>
		/// 
		/// </summary>
		public clsCtrlXrayL10711()
		{
			lock (cCtrlm.gLock)
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
            //lock (cCtrlm.gLock)
            //{
            //    Class_Terminate();
            //}
            Class_Terminate();
		}

		/// <summary>
		/// クラスイニシャライズ
		/// </summary>
		private void Class_Initialize()
		{
			modXray.udtXrayMethodValue udtXrayMIni = new modXray.udtXrayMethodValue();
			modXray.udtXrayProperty udtXrayPIni = new modXray.udtXrayProperty();
			//Dim mCntTime        As Single           '追加2009/08/31(KSS)hata_L10801 同時設定対応   'v16.20削除(ファームが対応した) byやまおか 2010/04/05

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
					mintInputMode = mcomCtrlXray.InputMode
					mcomCtrlXray.InputMode = comInputModeText
			        
					mcomCtrlXray.PortOpen = True
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

                    mcomCtrlXray.Open();

					//初期化
					modXray.gintOBJ_Set = 0;
					modXray.gintOBX_Set = 0;
					modXray.gintOBY_Set = 0;

                    //L10711 対応 by長野 2015/10/05
                    modXray.gintCAJ_Set = 0;
                    modXray.gintCAX_Set = 0;
                    modXray.gintCAY_Set = 0;
                    //L10711 対応 by長野 2015/10/05

					modXray.gudtXrayP = udtXrayPIni;
					modXray.gudtXrayMTmp = udtXrayMIni;
					modXray.gudtXrayM = udtXrayMIni;

					//----- タイマ設定 -----

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
					Set mtmrCtrlXray = mfrmCtrlXray.tmrXray
'					mtmrCtrlXray.Enabled = True
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
                    //ModuleCTRLM.ipX_type = 3;		//X線ﾀｲﾌﾟ(160kV nano)
                    ModuleCTRLM.ipX_type = modIniFiles.XRAY_TYPE_NO_160KV_2;

					mintSendPointer = P_CTRL;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//					mblnGetType = false;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
				}

				modData.gintXrayClsCnt = modData.gintXrayClsCnt + 1;
			}
			catch
			{
				//----- エラー処理 -----
				//削除2009/08/21_hata
				//    gintStatusNoValue = STS_R_XRAY_INITIALIZ
                //Debug.WriteLine("ClssInitialize Err");

			}
		}

		/// <summary>
		/// クラスターミネイト
		/// </summary>
		private void Class_Terminate()
		{
            int CntEnd = 3000;  //待ち時間

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
                        //シリアルポートがReadから離れるまで待つ
                        SerialRead.WaitOne(CntEnd);

                        mcomCtrlXray.Close();
                        mcomCtrlXray.Dispose();

                    }

                    //----- タイマ終了 -----
                    if (mtmrCtrlXray != null)
                    {
                        mtmrCtrlXray.Stop();
                    }
                    mcomCtrlXray = null;
                    mtmrCtrlXray = null;
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

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
					'----- フォーム終了 -----
					Unload mfrmCtrlXray
					Set mfrmCtrlXray = Nothing
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				}
			}
			catch
			{
				//----- エラー処理 -----
//削除2009/08/21_hata
				//    gintStatusNoValue = STS_R_XRAY_INITIALIZ
                //Debug.WriteLine("ClasTerminate Err");

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
                        data ="DSR On";
                    }
                    else
                    { 
                        //DSR off
                        data = "DSR Off";
                        eDSRFlg = true;    //追加2015/02/06hata
                    }

                    break;
                case SerialPinChange.CDChanged:
                   
                    ////CD (Carrier Detect) シグナルの状態が変化しました。
                    ////このシグナルは、モデムが動作中の電話回線に接続され、
                    ////データ キャリア シグナルが検出されるかどうかを示すために使用されます。

                    //コメントにしておく 2015/03/02hata
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
                    //return;

                case SerialPinChange.Break:
                    //入力時にブレークが検出されました。

                    //Breakが検出された
                    data = "Break On";
                    break;

                case SerialPinChange.Ring:
                    //リング インジケーターが検出されました。モデム動作

                    //コメントにしておく 2015/03/02hata
                    //Ringが検出された
                    data = "Ring On";
                    break;
                    //return;
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
            if(LineCount == 0) mtmrCtrlXray_LineErrTime = DateTime.Now;
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
                    //if (finfo.Length > 1000)  //変更2015/04/03hata
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
                //Debug.WriteLine("PinChanged Err");
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
                //Debug.WriteLine("ErrorReceived Err");
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
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
// 			float sngData = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			string strData = null;
			string strCommand = null;
			int intCnt = 0;
			int intBuf = 0;
			string strBuf = null;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
'			Static intERR(3 To 6)       As Integer
'			Static intOldErr(3 To 6)    As Integer
			Static intERR(1 To 6)       As Integer
			Static intOldErr(1 To 6)    As Integer
			Static sngTimCont1  As Single       '追加2010/02/24(KSS)hata
			Static sngTimCont2  As Single       '追加2010/02/24(KSS)hata
			Static sngTimCont3  As Single       '追加2010/02/24(KSS)hata
			Static sngTimCont4  As Single       '追加2010/02/24(KSS)hata
			Static sngTimCont5  As Single       '追加2010/02/24(KSS)hata
			Static sngTimCont6  As Single       '追加2010/02/24(KSS)hata
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			TimeSpan sngTime = TimeSpan.Zero;	//追加2010/02/24(KSS)hata
			float sngDelayTime = 0;				//追加2010/02/24(KSS)hata

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			Dim fileno As Integer 'v19.40 追加 by長野 2013/10/23
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			int iVal = 0;
			float fVal = 0;

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

                    //コマンド送信していないのに受信したデータは捨てる。
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
						mstrRecvBuf = mstrRecvBuf & mcomCtrlXray.Input
				        
						gintOldErrXrayComm = CTRL_OFF    '通信異常フラグクリア
				        
				'        Debug.Print "受信[" & mstrRecvBuf & "]"
				        
						'キャリッジリターン削除
				'2005-02-03 Shibui
				'        If Right$(mstrRecvBuf, Len(vbLf)) = vbLf Then
				'            intBuf = InStr(mstrRecvBuf, vbLf)
				'            If intBuf < Len(mstrRecvBuf) Then
				'                mstrRecvBuf = Mid(mstrRecvBuf, intBuf + 1)
				'            End If
				'            If Len(mstrRecvBuf) > Len(vbCr) + Len(vbLf) Then
				'                mstrRecvBuf = Left$(mstrRecvBuf, Len(mstrRecvBuf) - Len(vbCr) - Len(vbLf))
				'            End If
						If Right$(mstrRecvBuf, Len(vbCr)) = vbCr Then
				        
							intCnt = InStr(mstrRecvBuf, vbCr)
							If intCnt < Len(mstrRecvBuf) Then
								mstrRecvBuf = Mid(mstrRecvBuf, intCnt + 1)
							End If
							If Len(mstrRecvBuf) > Len(vbCr) Then
								mstrRecvBuf = Left$(mstrRecvBuf, Len(mstrRecvBuf) - Len(vbCr))
							End If
				            
							'管電圧値送信完了確認
							If gblnRecvPHV = True Then
								If Left(mstrRecvBuf, 3) = Left(CTRL_HIVOLT, 3) Then
									gblnRecvPHV = False
								End If
							End If
				        
							'管電流値送信完了確認
							If gblnRecvPCU = True Then
								If Left(mstrRecvBuf, 3) = Left(CTRL_CURRENT, 3) Then
									gblnRecvPCU = False
								End If
							End If
				    
							If Left(mstrRecvBuf, 3) = Left(CTRL_HIVOLT, 3) Then
								ipcndVolt = val(Mid$(mstrRecvBuf, 5, 3))                             '設定管電圧
								GoTo ExitHandler
							End If
							If Left(mstrRecvBuf, 3) = Left(CTRL_CURRENT, 3) Then
								ipcndAmp = val(Mid$(mstrRecvBuf, 5, 3))                             '設定管電流
								GoTo ExitHandler
							End If
				            
							If mstrRecvBuf = mstrCommFlg Then
								'ｺﾏﾝﾄﾞをそのまま返してくるので無視
								GoTo ExitHandler
				'                mstrRecvBuf = ""
				'                Exit Sub
							End If
						Else
							Exit Sub
						End If
				        
				'        Debug.Print "受信[" & mstrRecvBuf & "]"
				    
					Case comEvSend
				        
						'v19.40 追加 by 長野 2013/10/23
						fileno = FreeFile()
						Open "C:\CTUSER\XrayOnCommERR.log" For Output As fileno
				            
						Print #fileno, Now
				            
						Close fileno
				        
				'        'Debug.Print "送信バッファ内のバイト数が、SThreshold プロパティで指定された値よりも少なくなりました。"
						Exit Sub
					Case comEvCTS
				    
						'v19.40 追加 by 長野 2013/10/23
						fileno = FreeFile()
						Open "C:\CTUSER\XrayOnCommERR.log" For Output As fileno
				            
						Print #fileno, Now
				            
						Close fileno

						'Debug.Print "CTS ﾗｲﾝの状態が変化しました。"
						Exit Sub
					Case comEvDSR
				    
						'v19.40 追加 by 長野 2013/10/23
						fileno = FreeFile()
						Open "C:\CTUSER\XrayOnCommERR.log" For Output As fileno
				            
						Print #fileno, Now
				            
						Close fileno

						'Debug.Print "DSR ﾗｲﾝの状態が変化しました。"
						Exit Sub
					Case comEvCD
				        
						'v19.40 追加 by 長野 2013/10/23
						fileno = FreeFile()
						Open "C:\CTUSER\XrayOnCommERR.log" For Output As fileno
				            
						Print #fileno, Now
				            
						Close fileno

						'Debug.Print "CD ﾗｲﾝの状態が変化しました。"
						Exit Sub
					Case comEvRing
				    
						'v19.40 追加 by 長野 2013/10/23
						fileno = FreeFile()
						Open "C:\CTUSER\XrayOnCommERR.log" For Output As fileno
				            
						Print #fileno, Now
				            
						Close fileno

						'Debug.Print "呼び出し信号を検出しました。"
						Exit Sub
					Case comEvEOF
				    
					   'v19.40 追加 by 長野 2013/10/23
						fileno = FreeFile()
						Open "C:\CTUSER\XrayOnCommERR.log" For Output As fileno
				            
						Print #fileno, Now
				            
						Close fileno

						'Debug.Print "EOF を受信しました。"
						Exit Sub
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
				'削除2009/08/21_hata
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
                    
					modXray.gintOldErrXrayComm = modData.CTRL_OFF;

                    //追加2014/06/09_hata
                    SerialRead.Set();

                    //Debug.WriteLine("command0 = " + mstrRecvBuf);

                    if (mstrRecvBuf.EndsWith("\r") == true)
                    {
                        intCnt = mstrRecvBuf.IndexOf("\r");
                        if (intCnt < mstrRecvBuf.Length - 1)
                        {
                            mstrRecvBuf = mstrRecvBuf.Substring(intCnt + 1);
                        }
                        if (mstrRecvBuf.Length > "\r".Length)
                        {
                            mstrRecvBuf = mstrRecvBuf.TrimEnd('\r');
                        }
                        //Debug.WriteLine("command1 = " + mstrRecvBuf);

                        //管電圧値送信完了確認
                        if (modXray.gblnRecvPHV == true)
                        {
                            if (mstrRecvBuf.StartsWith(CTRL_HIVOLT.Substring(0, 3)) == true)
                            {
                                modXray.gblnRecvPHV = false;
                            }
                        }

                        //管電流値送信完了確認
                        if (modXray.gblnRecvPCU == true)
                        {
                            if (mstrRecvBuf.StartsWith(CTRL_CURRENT.Substring(0, 3)) == true)
                            {
                                modXray.gblnRecvPCU = false;
                            }
                        }

                        if (mstrRecvBuf.StartsWith(CTRL_HIVOLT.Substring(0, 3)) == true)
                        {
                            //変更2014/11/12hata
                            //float.TryParse(mstrRecvBuf.Substring(4, 3), out ModuleCTRLM.ipcndVolt);		//設定管電圧
                            //throw new Exception()
                            if (mstrRecvBuf.Length > CTRL_HIVOLT.Length)
                            {
                                float.TryParse(mstrRecvBuf.Substring(4, mstrRecvBuf.Length - 4), out ModuleCTRLM.ipcndVolt);		//設定管電圧
                                mstrCommFlg = "";
                                mstrRecvBuf = "";
                                return;
                            }
                        }
                        if (mstrRecvBuf.StartsWith(CTRL_CURRENT.Substring(0, 3)) == true)
                        {
                            //変更2014/11/12hata
                            //float.TryParse(mstrRecvBuf.Substring(4, 3), out ModuleCTRLM.ipcndAmp);		//設定管電流
                            //throw new Exception();
                            if (mstrRecvBuf.Length > CTRL_CURRENT.Length)
                            {
                                float.TryParse(mstrRecvBuf.Substring(4, mstrRecvBuf.Length - 4), out ModuleCTRLM.ipcndAmp);		//設定管電流                         
                                mstrCommFlg = "";
                                mstrRecvBuf = "";
                                return;
                            }

                        }

                        if (mstrRecvBuf == mstrCommFlg)
                        {
                            //ｺﾏﾝﾄﾞをそのまま返してくるので無視
                            //変更2014/11/12hata
                            //throw new Exception();
                            mstrCommFlg = "";
                            mstrRecvBuf = "";
                            return;

                        }
                    }
                    else
                    {
                        return;
                    }

					//----- 受信待ち分岐 -----
					switch (mstrCommFlg)
					{
						//--------------------------------------------------------
						//   コントロールコマンド
						//--------------------------------------------------------
						//コントロールコマンドの返信は、コマンド文字列をそのまま返すだけなので、返信データに対する処理は行わない。
						case CTRL_HIVOLT:
							// 管電圧設定[V]    (0～230)
							//エラーの場合は、現在値でイベントを発生させる
							if (mstrRecvBuf.StartsWith(mcstrStsERR) == true)
							{
								modXray.gintXraySetValueError = modData.CTRL_ON;
								modXray.gblnRecvPHV = false;
								modXray.gblnSendHIV = false;
							}
							break;

						case CTRL_CURRENT:
							// 管電流設定[uA]   (0～1000)
							//エラーの場合は、現在値でイベントを発生させる
							if (mstrRecvBuf.StartsWith(mcstrStsERR) == true)
							{
                                modXray.gintXraySetValueError = modData.CTRL_ON;
                                modXray.gblnRecvPHV = false;
                                modXray.gblnSendHIV = false;
								modXray.gintXraySetValueError = modData.CTRL_ON;
								modXray.gblnRecvPCU = false;
								modXray.gblnSendCUR = false;
							}
							break;

						case CTRL_XRAY_ON:		// Ｘ線出射開始
							break;
						case CTRL_XRAY_OFF:		// Ｘ線出射停止
							break;
						case CTRL_MODE_FOCUS:	// ﾌｫｰｶｽﾓｰﾄﾞｾｯﾄ  (1～4)
							break;
						case CTRL_RST:			// 過負荷保護解除
							break;
						case CTRL_WARMUP:		// ウォーミングアップ開始
							break;
						case CTRL_OBJ:			// ﾌｫｰｶｽ値設定（0～23000）
							break;
						case CTRL_SAV:			// ﾌｫｰｶｽ調整値保存
							break;
//						Case CTRL_OST          　' ﾌｫｰｶｽ値自動取得
						case CTRL_OBX:			// X方向ｱﾗｲﾒﾝﾄ設定
							break;
						case CTRL_OBY:			// Y方向ｱﾗｲﾒﾝﾄ設定
							break;
						case CTRL_ADJ:			// ﾋﾞｰﾑｱﾗｲﾒﾝﾄ
							break;
						case CTRL_ADA:			// 一括ﾋﾞｰﾑｱﾗｲﾒﾝﾄ
							break;
						case CTRL_STP:			// ﾋﾞｰﾑｱﾗｲﾒﾝﾄ中断
							break;
						case CTRL_ARS:			// ステータス自動送信設定
							break;
						case CTRL_DDL:			// デフォルト値読み出し
							break;

						case CTRL_CMV:			// 使用最大管電圧制限    'v15.10追加 byやまおか 2009/11/13
							break;

                        case CTRL_CAX: // ｺﾝﾃﾞﾝｻX方向ｱﾗｲﾒﾝﾄ設定 L10711対応 20080710
                            break;
                        case CTRL_CAY: // ｺﾝﾃﾞﾝｻY方向ｱﾗｲﾒﾝﾄ設定 L10711対応 20080710
                            break;
                        case CTRL_MDE: // ﾌｨﾗﾒﾝﾄﾓｰﾄﾞ設定 L10711対応 20080711
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

							if (mstrRecvBuf.StartsWith(CTRL_STS) == true)			//コマンド確認
							{
								strBuf = mstrRecvBuf.Substring(mstrRecvBuf.IndexOf(" ") + 1, 1);	//データ取得

								int val = 0;

								if (int.TryParse(strBuf, out val) == true)			//数値チェック
								{
									switch (val)
									{
										case 0:	//WARMUP YET
											modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
											ModuleCTRLM.ipWarmup = 0;				//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ待機中
											ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線OFF中
											ModuleCTRLM.ipXStatus = 5;
											ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
											break;
										case 1:	//WARMUP
											modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
											ModuleCTRLM.ipWarmup = 1;				//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ中
											ModuleCTRLM.ipX_On = modData.CTRL_ON;	//X線ON中
											ModuleCTRLM.ipXStatus = 3;
											ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
											break;
										case 2:	//STANDBY
											modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
											ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線照射待機中
											ModuleCTRLM.ipWarmup = 2;				//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ完了
											ModuleCTRLM.ipXStatus = 6;
											ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
											break;
										case 3:	//XON
											modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
											ModuleCTRLM.ipX_On = modData.CTRL_ON;	//X線ON中
											ModuleCTRLM.ipWarmup = 2;
											ModuleCTRLM.ipXStatus = 4;
											ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
											break;
										case 4:	//OVER
											modXray.gblnXrayOver = false;			//荷保護機能動作中ﾌﾗｸﾞ
											ModuleCTRLM.ipXStatus = 7;
											ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
											mintSovOver = modData.CTRL_ON;			//解除指令                      2007/05/08追加 by 間々田 過負荷時Ｘ線オフしない現象への対策
											ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線が照射できない状態         2007/05/08追加 by 間々田 過負荷時Ｘ線オフしない現象への対策
											break;
										case 5:	//NOT READY
											modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
											ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線が照射できない状態
											ModuleCTRLM.ipXStatus = 2;
											ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
											break;
										default:
											ModuleCTRLM.ipXStatus = 0;
											break;
									}

									if (ModuleCTRLM.ipWarmup == 2)					//ウォームアップ完了
									{
										ModuleCTRLM.ipWarmup_Mode = -1;				//不要
									}
									else
									{
										ModuleCTRLM.ipWarmup_Mode = 1;				//必要
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

                                    //削除2009/08/25hata ----->
                                    //gstrXrayDisp(DSP_STS) = mstrRecvBuf 'X線情報プロパティ用
								}
							}
							break;

						case CTRL_SAR:

							//状態一括確認
							// STS  :状態
							// SHV  :出力電圧
							// SUC  :出力電流
							// STG  :ﾀｰｹﾞｯﾄ電流
							// FLM  :ﾌｨﾗﾒﾝﾄ状態
							// SAD  :ｱﾗｲﾒﾝﾄ状態
							// Nono :未使用

							//OK ×SIN  :ｲﾝﾀﾛｯｸ確認
							//OK ×SFC  :焦点モード確認
							//OK ×SVC  :真空ﾚﾍﾞﾙ確認
							//   ×     :ターゲット電流制限値到達(TargetLimit)
							//OK ×     :制御基板異常状態

							if (mstrRecvBuf.StartsWith(CTRL_SAR) == true)	//コマンド確認
							{
								//----- STS 状態確認 -----
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;		//データ取得
								strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf) - intBuf);
								if (int.TryParse(strBuf, out iVal) == true)			//数値チェック
								{
									switch (iVal)
									{
										case 0:	//WARMUP YET
											modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
											ModuleCTRLM.ipWarmup = 0;				//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ待機中
											ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線OFF中
											ModuleCTRLM.ipXStatus = 5;
											ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
											break;
										case 1:	//WARMUP
											modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
											ModuleCTRLM.ipWarmup = 1;				//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ中
											ModuleCTRLM.ipX_On = modData.CTRL_ON;	//X線ON中
											ModuleCTRLM.ipXStatus = 3;
											ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
											break;
										case 2:	//STANDBY
											modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
											ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線照射待機中
											ModuleCTRLM.ipWarmup = 2;				//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ完了
											ModuleCTRLM.ipXStatus = 6;
											ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
											break;
										case 3:	//XON
											modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
											ModuleCTRLM.ipX_On = modData.CTRL_ON;	//X線ON中
											ModuleCTRLM.ipWarmup = 2;
											ModuleCTRLM.ipXStatus = 4;
											ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
											break;
										case 4:	//OVER
											modXray.gblnXrayOver = false;			//荷保護機能動作中ﾌﾗｸﾞ
											ModuleCTRLM.ipXStatus = 7;
											ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
											mintSovOver = modData.CTRL_ON;			//解除指令                      2007/05/08追加 by 間々田 過負荷時Ｘ線オフしない現象への対策
											ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線が照射できない状態         2007/05/08追加 by 間々田 過負荷時Ｘ線オフしない現象への対策
											break;
										case 5:	//NOT READY
											modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
											ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線が照射できない状態
											ModuleCTRLM.ipXStatus = 2;
											ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
											break;
										default:
											ModuleCTRLM.ipXStatus = 0;
											break;
									}

									if (ModuleCTRLM.ipWarmup == 2)		//ウォームアップ完了
									{
										ModuleCTRLM.ipWarmup_Mode = -1;	//不要
									}
									else
									{
										ModuleCTRLM.ipWarmup_Mode = 1;	//必要
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
								}

								//----- SHV 出力電圧確認 -----
								intBuf = mstrRecvBuf.IndexOf(" ", intBuf) + 1;			//データ取得
								strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf) - intBuf);
								if (float.TryParse(strBuf, out fVal) == true)			//数値チェック
								{
									ModuleCTRLM.ipX_Volt = fVal;						//データセット
								}

								//----- SCU 出力電流確認 -----
								intBuf = mstrRecvBuf.IndexOf(" ", intBuf) + 1;			//データ取得
								strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf) - intBuf);
								if (float.TryParse(strBuf, out fVal) == true)			//数値チェック
								{
									ModuleCTRLM.ipX_Amp = fVal;							//データセット
								}

								//X線OFFで管電流=1の時に管電流=0とする。
								if (ModuleCTRLM.ipX_On == modXray.RET_XRAY_OFF && ModuleCTRLM.ipX_Amp == 1)
								{
									ModuleCTRLM.ipX_Amp = 0;
								}

								//----- STG ﾀｰｹﾞｯﾄ電流 -----
								intBuf = mstrRecvBuf.IndexOf(" ", intBuf) + 1;			//データ取得
								strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf) - intBuf);
								if (float.TryParse(strBuf, out fVal) == true)			//数値チェック
								{
									modXray.gudtXrayP.sngTargetInfSTG = fVal;			//データセット
								}

								//----- FLM ﾌｨﾗﾒﾝﾄ状態 -----
								intBuf = mstrRecvBuf.IndexOf(" ", intBuf) + 1;			//データ取得
								strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf) - intBuf);
								if (int.TryParse(strBuf, out iVal) == true)				//数値チェック
								{
									modXray.gudtXrayP.intFLM = iVal;
									if (modXray.gudtXrayP.intFLM == 2)
									{
										ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR5;	//ﾌｨﾗﾒﾝﾄ断線
										modXray.gudtXrayP.intSER = 5;					//v16.02追加 byやまおか 2010/03/03
									}
								}

								//----- SAD ｱﾗｲﾒﾝﾄ状態 -----
								intBuf = mstrRecvBuf.IndexOf(" ", intBuf) + 1;			//データ取得
								strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf) - intBuf);
								if (int.TryParse(strBuf, out iVal) == true)				//数値チェック
								{
									switch (iVal)
									{
										case mcintSadOff:
											modXray.gudtXrayP.intSAD = modXray.RET_SAD_OFF;
											break;
										case mcintSadRun:
											modXray.gudtXrayP.intSAD = modXray.RET_SAD_1;
											break;
										case mcintSadAll:
											modXray.gudtXrayP.intSAD = modXray.RET_SAD_ALL;
											break;
										default:
											modXray.gudtXrayP.intSAD = modXray.RET_SAD_OFF;
											break;
									}
								}
							}
							break;

						case CTRL_SNR:

							//「NOT READY」一括確認
							if (mstrRecvBuf.StartsWith(CTRL_SNR) == true)	//コマンド確認
							{
								//-----(1) 制御基板異常確認 -----
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;		//データ取得
								strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf));
								if (int.TryParse(strBuf, out iVal) == true)
								{
									//gudtXrayP.intSER = CInt(strData)
									if (modXray.gudtXrayP.intFLM == 2)
									{
										modXray.gudtXrayP.intSER = 5;
									}
									else
									{
										modXray.gudtXrayP.intSER = Convert.ToInt32(strData);
									}
								}
								else
								{
									if (strData == "F")
									{
										modXray.gudtXrayP.intSER = 5;
									}
								}

//								変更2010/02/24（KSS)hata １回しかError表示されないのため ----->
//								Select Case gudtXrayP.intSER
//								Case 1: ipErrsts = ERR_XRAY_ERR1        '電池切れ
//								Case 2: ipErrsts = ERR_XRAY_ERR2        'ｲﾝﾀｰﾛｯｸ2用ﾘﾚｰ故障
//								Case 3: ipErrsts = ERR_XRAY_ERR3        '高圧ﾊﾟﾜｰﾘﾚｰ駆動用ﾄﾗﾝｼﾞｽﾀ故障
//								Case 4: ipErrsts = ERR_XRAY_ERR4        '高圧ﾊﾟﾜｰﾘﾚｰ故障
//								Case 5: ipErrsts = ERR_XRAY_ERR5        'ﾌｨﾗﾒﾝﾄ断線
//								Case 209: ipErrsts = ERR_XRAY_ERR209    '温度ｴﾗｰ
//								End Select
								switch (modXray.gudtXrayP.intSER)
								{
									case 1:			//電池切れ
										mcomCtrlXray_OnComm_intERR[0] = modData.CTRL_ON;
										if (mcomCtrlXray_OnComm_sngTimCont1 == TimeSpan.Zero)
										{
											mcomCtrlXray_OnComm_sngTimCont1 = DateTime.Now.TimeOfDay;
										}
										break;
									case 2:			//ｲﾝﾀｰﾛｯｸ2用ﾘﾚｰ故障
										mcomCtrlXray_OnComm_intERR[1] = modData.CTRL_ON;
										if (mcomCtrlXray_OnComm_sngTimCont2 == TimeSpan.Zero)
										{
											mcomCtrlXray_OnComm_sngTimCont2 = DateTime.Now.TimeOfDay;
										}
										break;
									case 3:			//高圧ﾊﾟﾜｰﾘﾚｰ駆動用ﾄﾗﾝｼﾞｽﾀ故障
										mcomCtrlXray_OnComm_intERR[2] = modData.CTRL_ON;
										if (mcomCtrlXray_OnComm_sngTimCont3 == TimeSpan.Zero)
										{
											mcomCtrlXray_OnComm_sngTimCont3 = DateTime.Now.TimeOfDay;
										}
										break;
									case 4:			//高圧ﾊﾟﾜｰﾘﾚｰ故障
										mcomCtrlXray_OnComm_intERR[3] = modData.CTRL_ON;
										if (mcomCtrlXray_OnComm_sngTimCont4 == TimeSpan.Zero)
										{
											mcomCtrlXray_OnComm_sngTimCont4 = DateTime.Now.TimeOfDay;
										}
										break;
									case 5:			//ﾌｨﾗﾒﾝﾄ断線
										mcomCtrlXray_OnComm_intERR[4] = modData.CTRL_ON;
										if (mcomCtrlXray_OnComm_sngTimCont5 == TimeSpan.Zero)
										{
											mcomCtrlXray_OnComm_sngTimCont5 = DateTime.Now.TimeOfDay;
										}
										break;
									case 209:		//温度ｴﾗｰ
										mcomCtrlXray_OnComm_intERR[5] = modData.CTRL_ON;
										if (mcomCtrlXray_OnComm_sngTimCont6 == TimeSpan.Zero)
										{
											mcomCtrlXray_OnComm_sngTimCont6 = DateTime.Now.TimeOfDay;
										}
										break;
									default:
										mcomCtrlXray_OnComm_intERR[0] = modData.CTRL_OFF;
										mcomCtrlXray_OnComm_intERR[1] = modData.CTRL_OFF;
										mcomCtrlXray_OnComm_intERR[2] = modData.CTRL_OFF;
										mcomCtrlXray_OnComm_intERR[3] = modData.CTRL_OFF;
										mcomCtrlXray_OnComm_intERR[4] = modData.CTRL_OFF;
										mcomCtrlXray_OnComm_intERR[5] = modData.CTRL_OFF;
										mcomCtrlXray_OnComm_sngTimCont1 = TimeSpan.Zero;
										mcomCtrlXray_OnComm_sngTimCont2 = TimeSpan.Zero;
										mcomCtrlXray_OnComm_sngTimCont3 = TimeSpan.Zero;
										mcomCtrlXray_OnComm_sngTimCont4 = TimeSpan.Zero;
										mcomCtrlXray_OnComm_sngTimCont5 = TimeSpan.Zero;
										mcomCtrlXray_OnComm_sngTimCont6 = TimeSpan.Zero;
										break;
								}

								sngDelayTime = 20;			//20秒毎に表示
								sngTime = DateTime.Now.TimeOfDay;
								if ((sngTime - mcomCtrlXray_OnComm_sngTimCont1).TotalSeconds > sngDelayTime && mcomCtrlXray_OnComm_intERR[0] == modData.CTRL_ON)
								{
									//Err表示
									ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR1;
									mcomCtrlXray_OnComm_sngTimCont1 = TimeSpan.Zero;
								}
								else if (sngTime < mcomCtrlXray_OnComm_sngTimCont1)
								{
									mcomCtrlXray_OnComm_sngTimCont1 = TimeSpan.Zero;
								}
								if ((sngTime - mcomCtrlXray_OnComm_sngTimCont2).TotalSeconds > sngDelayTime && mcomCtrlXray_OnComm_intERR[1] == modData.CTRL_ON)
								{
									//Err表示
									ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR2;
									mcomCtrlXray_OnComm_sngTimCont2 = TimeSpan.Zero;
								}
								else if (sngTime < mcomCtrlXray_OnComm_sngTimCont2)
								{
									mcomCtrlXray_OnComm_sngTimCont2 = TimeSpan.Zero;
								}
								if ((sngTime - mcomCtrlXray_OnComm_sngTimCont3).TotalSeconds > sngDelayTime && mcomCtrlXray_OnComm_intERR[2] == modData.CTRL_ON)
								{
									//Err表示
									ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR3;
									mcomCtrlXray_OnComm_sngTimCont3 = TimeSpan.Zero;
								}
								else if (sngTime < mcomCtrlXray_OnComm_sngTimCont3)
								{
									mcomCtrlXray_OnComm_sngTimCont3 = TimeSpan.Zero;
								}
								if ((sngTime - mcomCtrlXray_OnComm_sngTimCont4).TotalSeconds > sngDelayTime && mcomCtrlXray_OnComm_intERR[3] == modData.CTRL_ON)
								{
									//Err表示
									ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR4;
									mcomCtrlXray_OnComm_sngTimCont4 = TimeSpan.Zero;
								}
								else if (sngTime < mcomCtrlXray_OnComm_sngTimCont4)
								{
									mcomCtrlXray_OnComm_sngTimCont4 = TimeSpan.Zero;
								}
								if ((sngTime - mcomCtrlXray_OnComm_sngTimCont5).TotalSeconds > sngDelayTime && mcomCtrlXray_OnComm_intERR[4] == modData.CTRL_ON)
								{
									//Err表示
									ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR5;
									mcomCtrlXray_OnComm_sngTimCont5 = TimeSpan.Zero;
								}
								else if (sngTime < mcomCtrlXray_OnComm_sngTimCont5)
								{
									mcomCtrlXray_OnComm_sngTimCont5 = TimeSpan.Zero;
								}
								if ((sngTime - mcomCtrlXray_OnComm_sngTimCont6).TotalSeconds > sngDelayTime && mcomCtrlXray_OnComm_intERR[5] == modData.CTRL_ON)
								{
									//Err表示
									ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR209;
									mcomCtrlXray_OnComm_sngTimCont6 = TimeSpan.Zero;
								}
								else if (sngTime < mcomCtrlXray_OnComm_sngTimCont6)
								{
									mcomCtrlXray_OnComm_sngTimCont6 = TimeSpan.Zero;
								}
								//変更2010/02/24（KSS)hata １回しかError表示されないのため -----<

								//-----(2) ｲﾝﾀﾛｯｸ確認 -----
								intBuf = mstrRecvBuf.IndexOf(" ", intBuf) + 1;			//データ取得
								strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf) - intBuf);
								if (int.TryParse(strBuf, out iVal) == true)				//数値チェック
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

								//-----(3) 設定なし -----
								intBuf = mstrRecvBuf.IndexOf(" ", intBuf) + 1;			//データ取得
								strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf) - intBuf);
								if (int.TryParse(strBuf, out iVal) == true)				//数値チェック
								{
									//SPH
								}

								//-----(4)　真空状態確認 SVC -----
								intBuf = mstrRecvBuf.IndexOf(" ", intBuf) + 1;			//データ取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								if (int.TryParse(strBuf, out iVal) == true)				//数値チェック
								{
									//SVC
									switch (iVal)
									{
										case mcintStsSVC0:
											modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_LO;
											break;
										case mcintStsSVC1:
											modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V1;
											break;
										case mcintStsSVC2:
											modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V2;
											break;
										case mcintStsSVC3:
											modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V3;
											break;
										case mcintStsSVC4:
											modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V4;
											break;
										case mcintStsSVC5:
											modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V5;
											break;
										case mcintStsSVC6:
											modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V6;
											break;
										case mcintStsSVC7:
											modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_NG;
											break;
									}
								}
							}
							break;

						case CTRL_SHV:

							//出力管電圧確認
							if (mstrRecvBuf.StartsWith(CTRL_SHV) == true)						//コマンド確認
							{
								strBuf = mstrRecvBuf.Substring(mstrRecvBuf.IndexOf(" ") + 1);	//データ取得
								if (float.TryParse(strBuf, out fVal) == true)					//数値チェック
								{
									ModuleCTRLM.ipX_Volt = fVal;								//データセット
								}
							}
							break;

						case CTRL_SCU:

							//出力管電流確認
							if (mstrRecvBuf.StartsWith(CTRL_SCU) == true)						//コマンド確認
							{
								strBuf = mstrRecvBuf.Substring(mstrRecvBuf.IndexOf(" ") + 1);	//データ取得
								if (float.TryParse(strBuf, out fVal) == true)					//数値チェック
								{
									ModuleCTRLM.ipX_Amp = fVal;									//データセット
								}
							}
							break;

						case CTRL_SPV:

							//管電圧プリセット確認
							if (mstrRecvBuf.StartsWith(CTRL_SPV) == true)						//コマンド確認
							{
								strBuf = mstrRecvBuf.Substring(mstrRecvBuf.IndexOf(" ") + 1);	//データ取得
								if (modXray.gblnRecvPHV == false)
								{
									if (float.TryParse(strBuf, out fVal) == true)				//数値チェック
									{
										ModuleCTRLM.ipcndVolt = fVal;							//設定管電圧
									}
								}
							}
							break;

						case CTRL_SPC:

							//管電流プリセット確認
							if (mstrRecvBuf.StartsWith(CTRL_SPC) == true)						//コマンド確認
							{
								strBuf = mstrRecvBuf.Substring(mstrRecvBuf.IndexOf(" ") + 1);	//データ取得

								if (modXray.gblnRecvPCU == false)
								{
									if (float.TryParse(strBuf, out fVal) == true)				//数値チェック
									{
										ModuleCTRLM.ipcndAmp = fVal;							//設定管電流
									}
								}
							}
							break;

						case CTRL_SVI:

							//設定管電圧電流値確認
							if (mstrRecvBuf.StartsWith(CTRL_SVI) == true)			//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;				//データ開始位置取得
								if (modXray.gblnRecvPHV == false && modXray.gblnRecvPCU == false)
								{
									//設定管電圧
									strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf) - intBuf);	//データ取得
									if (float.TryParse(strBuf, out fVal) == true)	//数値チェック
									{
										ModuleCTRLM.ipcndVolt = fVal;				//設定管電圧
									}

									//設定管電流
									strBuf = mstrRecvBuf.Substring(mstrRecvBuf.IndexOf(" ", intBuf) + 1);
									if (float.TryParse(strBuf, out fVal) == true)	//数値チェック
									{
										ModuleCTRLM.ipcndAmp = fVal;				//設定管電流
									}
								}
								if (modXray.gblnXrayValue == true)
								{
									modXray.gintXrayValueDisp = 1;
									modXray.gblnXrayValue = false;
								}
								modXray.gstrXrayDisp[modXray.DSP_SVI] = mstrRecvBuf;		//X線情報プロパティ用
							}
							break;

						case CTRL_STL:

							// 動作値一括確認
							if (mstrRecvBuf.StartsWith(CTRL_STL) == true)			//コマンド確認
							{
								//"STL 管電圧 管電流 ターゲット電流"
								//管電圧、管電流、ターゲット電流の動作値が、それぞれスペースで区切って返される。

								//管電圧
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;				//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf) - intBuf);	//データ取得
								if (float.TryParse(strBuf, out fVal) == true)		//数値チェック
								{
									ModuleCTRLM.ipX_Volt = fVal;					//データセット
								}

								//管電流
								intBuf = mstrRecvBuf.IndexOf(" ", intBuf) + 1;		//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf, mstrRecvBuf.IndexOf(" ", intBuf) - intBuf);	//データ取得
								if (float.TryParse(strBuf, out fVal) == true)		//数値チェック
								{
									ModuleCTRLM.ipX_Amp = fVal;						//データセット
								}

								//ターゲット電流
								intBuf = mstrRecvBuf.IndexOf(" ", intBuf) + 1;		//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);				//データ取得
								if (float.TryParse(strBuf, out fVal) == true)		//数値チェック
								{
									modXray.gudtXrayP.sngTargetInfSTG = fVal;		//データセット
								}

								//X線OFFで管電流=1の時に管電流=0とする。
								if (ModuleCTRLM.ipX_On == modXray.RET_XRAY_OFF && ModuleCTRLM.ipX_Amp == 1)
								{
									ModuleCTRLM.ipX_Amp = 0;
								}
							}
							break;

                        case CTRL_SAP: // アパーチャ電流
                            //if (strRecvBuf.Left(CTRL_SAP.GetLength()) == CTRL_SAP)
                            if(mstrRecvBuf.StartsWith(CTRL_SAP) == true)
                            {
                                intBuf = mstrRecvBuf.IndexOf(" ") + 1;				//データ開始位置取得
                                strBuf = mstrRecvBuf.Substring(intBuf);
                                if (float.TryParse(strBuf, out fVal) == true)
                                {
                                    modXray.gudtXrayP.sngCurrentSAP = fVal;		// ﾀｰｹﾞｯﾄ電流確認
                                }
                            }
                            break;

						case CTRL_STG:

							//ターゲット電流
							if (mstrRecvBuf.StartsWith(CTRL_STG) == true)			//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;				//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								if (float.TryParse(strBuf, out fVal) == true)
								{
									modXray.gudtXrayP.sngTargetInfSTG = fVal;		// ﾀｰｹﾞｯﾄ電流確認
								}
							}
							break;

//2009/08/21_未使用_hata
//						Case CTRL_TGS
//
//							'ターゲット電流制限状態確認
//							If Left(mstrRecvBuf, Len(CTRL_TGS)) = CTRL_TGS Then 'コマンド確認
//'								intBuf = InStr(1, mstrRecvBuf, " ") + 1        'データ開始位置取得
//'								strBuf = Mid(mstrRecvBuf, intBuf)
//								strBuf = Mid(mstrRecvBuf, 4)
//								If IsNumeric(strBuf) = True Then
//									gudtXrayStatus1Value.E_XrayTargetLimit = CInt(strBuf)
//								End If
//								gstrXrayDisp(DSP_TGS) = mstrRecvBuf 'X線情報プロパティ用
//							End If

						case CTRL_SWE:
							//ウォーミングアップ状態確認
							//ipWarmup= 0    :ｳｫｰﾐﾝｸﾞｱｯﾌﾟ待機中
							//ipWarmup= 1    :ｳｫｰﾐﾝｸﾞｱｯﾌﾟ中
							//ipWarmup = 2   :ｳｫｰﾐﾝｸﾞｱｯﾌﾟ完了

							//コマンド確認
							if (mstrRecvBuf.StartsWith(CTRL_SWE) == true)
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

							//フォーカスモード確認
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

						case CTRL_SOB:

							//フォーカス値確認
							if (mstrRecvBuf.StartsWith(CTRL_SOB) == true)	//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;		//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								if (float.TryParse(strBuf, out fVal) == true)
								{
									float.TryParse(mstrRecvBuf.Substring(4), out modXray.gudtXrayP.sngSOB);	// ﾌｫｰｶｽ値確認
								}
							}
							break;

                        //<---L10711対応 20080822
                        case CTRL_SCW:
                            //フォーカス値確認
                            if (mstrRecvBuf.StartsWith(CTRL_SCW) == true)	//コマンド確認
                            {
                                intBuf = mstrRecvBuf.IndexOf(" ") + 1;		//データ開始位置取得
                                strBuf = mstrRecvBuf.Substring(intBuf);
                                if (float.TryParse(strBuf, out fVal) == true)
                                {
                                    float.TryParse(mstrRecvBuf.Substring(4), out modXray.gudtXrayP.sngCOB);	// ﾌｫｰｶｽ値確認
                                }
                            }
                            break;
		                //L10711対応 20080822--->

						case CTRL_SAD:

							//アライメントモニタ
							if (mstrRecvBuf.StartsWith(CTRL_SAD) == true)	//コマンド確認
							{
								strBuf = mstrRecvBuf.Substring(3);
								if (int.TryParse(strBuf, out iVal) == true)
								{
									switch (iVal)
									{
										case mcintSadOff:
											modXray.gudtXrayP.intSAD = modXray.RET_SAD_OFF;
											break;
										case mcintSadRun:
											modXray.gudtXrayP.intSAD = modXray.RET_SAD_1;
											break;
										case mcintSadAll:
											modXray.gudtXrayP.intSAD = modXray.RET_SAD_ALL;
											break;
										default:
											modXray.gudtXrayP.intSAD = modXray.RET_SAD_OFF;
											break;
									}
								}
							}
							break;

						case CTRL_SBX:

							//X方向アライメント確認
							if (mstrRecvBuf.StartsWith(CTRL_SBX) == true)		//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								if (int.TryParse(strBuf, out iVal) == true)
								{
									modXray.gudtXrayP.intSBX = iVal;			// X方向ｱﾗｲﾒﾝﾄ確認
								}
							}
							break;

						case CTRL_SBY:

							//Y方向アライメント確認
							if (mstrRecvBuf.StartsWith(CTRL_SBY) == true)		//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								if (int.TryParse(strBuf, out iVal) == true)
								{
									modXray.gudtXrayP.intSBY = iVal;			// X方向ｱﾗｲﾒﾝﾄ確認
								}
							}
							break;

                        //---L10711対応 20080710
		                case CTRL_SCX:

                            //X方向アライメント確認
							if (mstrRecvBuf.StartsWith(CTRL_SCX) == true)		//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								if (int.TryParse(strBuf, out iVal) == true)
								{
									modXray.gudtXrayP.intSCX = iVal;			// X方向ｱﾗｲﾒﾝﾄ確認
								}
							}
							break;

                        case CTRL_SCY:
                            //Y方向アライメント確認
                            if (mstrRecvBuf.StartsWith(CTRL_SCY) == true)		//コマンド確認
                            {
                                intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
                                strBuf = mstrRecvBuf.Substring(intBuf);
                                if (int.TryParse(strBuf, out iVal) == true)
                                {
                                    modXray.gudtXrayP.intSCY = iVal;			// X方向ｱﾗｲﾒﾝﾄ確認
                                }
                            }
                            break;
                        //L10711対応 20080710---

						case CTRL_SVC:

							//真空状態確認
							if (mstrRecvBuf.StartsWith(CTRL_SVC) == true)		//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								if (int.TryParse(strBuf, out iVal) == true)
								{
									switch (Convert.ToInt32(strBuf))
									{
										case mcintStsSVC0:
											modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_LO;
											break;
										case mcintStsSVC1:
											modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V1;
											break;
										case mcintStsSVC2:
											modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V2;
											break;
										case mcintStsSVC3:
											modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V3;
											break;
										case mcintStsSVC4:
											modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V4;
											break;
										case mcintStsSVC5:
											modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V5;
											break;
										case mcintStsSVC6:
											modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V6;
											break;
										case mcintStsSVC7:
											modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_NG;
											break;
									}
								}
							}
							break;

						case CTRL_SIN:

							//インターロック確認
							if (mstrRecvBuf.StartsWith(CTRL_SIN) == true)		//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								if (int.TryParse(strBuf, out iVal) == true)
								{
									if (iVal == modXray.STA_INTERLOCK_ON)
									{
										ModuleCTRLM.ipInterlock = 1;
									}
									else
									{
										ModuleCTRLM.ipInterlock = 0;
									}
								}
							}
							break;

						case CTRL_FLM:

							//フィラメント状態確認
							if (mstrRecvBuf.StartsWith(CTRL_FLM) == true)		//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								if (int.TryParse(strBuf, out iVal) == true)
								{
									modXray.gudtXrayP.intFLM = iVal;
								}
							}
							break;

                        //---L10711対応 20080710
                        case CTRL_SMD:

                            //フィラメントモード確認
                            if (mstrRecvBuf.StartsWith(CTRL_SMD) == true)		//コマンド確認
                            {
                                intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
                                strBuf = mstrRecvBuf.Substring(intBuf);
                                if (int.TryParse(strBuf, out iVal) == true)
                                {
                                    switch (iVal)
                                    {
                                        case mcintStsMDE1:
                                            modXray.gudtXrayP.intSMD = iVal;
                                            break;
                                        case mcintStsMDE2:
                                            modXray.gudtXrayP.intSMD = iVal;
                                            break;
                                    }
                                }
                            }
                            break;
                       //L10711対応 20080710---

						case CTRL_STM:

							//----- X線源通電時間確認 -----
							if (mstrRecvBuf.StartsWith(CTRL_STM) == true)		//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								if (int.TryParse(strBuf, out iVal) == true)
								{
									modXray.gudtXrayP.lngSTM = iVal;
								}
							}
							break;
						case CTRL_SXT:

							//----- X線照射時間確認 -----
							if (mstrRecvBuf.StartsWith(CTRL_SXT) == true)		//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								if (int.TryParse(strBuf, out iVal) == true)
								{
									//"hhhhhh"→"hhhhhhmm"のﾌｫｰﾏｯﾄにする
									modXray.gudtXrayP.lngSXT = iVal * 100;
								}
							}
							break;

						case CTRL_SHT:

							//フィラメントオン時間
							if (mstrRecvBuf.StartsWith(CTRL_SHT) == true)		//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								if (int.TryParse(strBuf, out iVal) == true)
								{
									//"hhhhhh"→"hhhhhhmm"のﾌｫｰﾏｯﾄにする
									modXray.gudtXrayP.lngSHT = iVal * 100;
								}
							}
							break;

						case CTRL_SER:

							//----- 制御基板異常確認 -----
							strCommand = mstrRecvBuf.Substring(0, 3);
							if (strCommand == CTRL_SER)
							{
								strData = mstrRecvBuf.Substring(4, 1);
								if (int.TryParse(strData, out iVal) == true)
								{
									//'gudtXrayP.intSER = CInt(strData)
									if (modXray.gudtXrayP.intFLM == 2)
									{
										modXray.gudtXrayP.intSER = 5;
									}
									else
									{
										modXray.gudtXrayP.intSER = iVal;
									}
								}
								else
								{
									if (strData == "F")
									 {
										modXray.gudtXrayP.intSER = 5;
									}
								}

//変更2010/02/24（KSS)hata １回しかError表示されないのため ----->
//								Select Case gudtXrayP.intSER
//								Case 2: ipErrsts = ERR_XRAY_ERR2        'ｲﾝﾀｰﾛｯｸ2用ﾘﾚｰ故障
//								Case 3: ipErrsts = ERR_XRAY_ERR3        '高圧ﾊﾟﾜｰﾘﾚｰ駆動用ﾄﾗﾝｼﾞｽﾀ故障
//								Case 4: ipErrsts = ERR_XRAY_ERR4        '高圧ﾊﾟﾜｰﾘﾚｰ故障
//								Case 5: ipErrsts = ERR_XRAY_ERR5        'ﾌｨﾗﾒﾝﾄ断線
//								Case 1: ipErrsts = ERR_XRAY_ERR1        '電池切れ
//								Case 209: ipErrsts = ERR_XRAY_ERR209    '温度ｴﾗｰ
//								End Select
								switch (modXray.gudtXrayP.intSER)
								{
									case 1:			//電池切れ
										mcomCtrlXray_OnComm_intERR[0] = modData.CTRL_ON;
										if (mcomCtrlXray_OnComm_sngTimCont1 == TimeSpan.Zero)
										{
											mcomCtrlXray_OnComm_sngTimCont1 = DateTime.Now.TimeOfDay;
										}
										break;
									case 2:			//ｲﾝﾀｰﾛｯｸ2用ﾘﾚｰ故障
										mcomCtrlXray_OnComm_intERR[1] = modData.CTRL_ON;
										if (mcomCtrlXray_OnComm_sngTimCont2 == TimeSpan.Zero)
										{
											mcomCtrlXray_OnComm_sngTimCont2 = DateTime.Now.TimeOfDay;
										}
										break;
									case 3:			//高圧ﾊﾟﾜｰﾘﾚｰ駆動用ﾄﾗﾝｼﾞｽﾀ故障
										mcomCtrlXray_OnComm_intERR[2] = modData.CTRL_ON;
										if (mcomCtrlXray_OnComm_sngTimCont3 == TimeSpan.Zero)
										{
											mcomCtrlXray_OnComm_sngTimCont3 = DateTime.Now.TimeOfDay;
										}
										break;
									case 4:			//高圧ﾊﾟﾜｰﾘﾚｰ故障
										mcomCtrlXray_OnComm_intERR[3] = modData.CTRL_ON;
										if (mcomCtrlXray_OnComm_sngTimCont4 == TimeSpan.Zero)
										{
											mcomCtrlXray_OnComm_sngTimCont4 = DateTime.Now.TimeOfDay;
										}
										break;
									case 5:			//ﾌｨﾗﾒﾝﾄ断線
										mcomCtrlXray_OnComm_intERR[4] = modData.CTRL_ON;
										if (mcomCtrlXray_OnComm_sngTimCont5 == TimeSpan.Zero)
										{
											mcomCtrlXray_OnComm_sngTimCont5 = DateTime.Now.TimeOfDay;
										}
										break;
									case 209:		//温度ｴﾗｰ
										mcomCtrlXray_OnComm_intERR[5] = modData.CTRL_ON;
										if (mcomCtrlXray_OnComm_sngTimCont6 == TimeSpan.Zero)
										{
											mcomCtrlXray_OnComm_sngTimCont6 = DateTime.Now.TimeOfDay;
										}
										break;
									default:
										mcomCtrlXray_OnComm_intERR[0] = modData.CTRL_OFF;
										mcomCtrlXray_OnComm_intERR[1] = modData.CTRL_OFF;
										mcomCtrlXray_OnComm_intERR[2] = modData.CTRL_OFF;
										mcomCtrlXray_OnComm_intERR[3] = modData.CTRL_OFF;
										mcomCtrlXray_OnComm_intERR[4] = modData.CTRL_OFF;
										mcomCtrlXray_OnComm_intERR[5] = modData.CTRL_OFF;
										mcomCtrlXray_OnComm_sngTimCont1 = TimeSpan.Zero;
										mcomCtrlXray_OnComm_sngTimCont2 = TimeSpan.Zero;
										mcomCtrlXray_OnComm_sngTimCont3 = TimeSpan.Zero;
										mcomCtrlXray_OnComm_sngTimCont4 = TimeSpan.Zero;
										mcomCtrlXray_OnComm_sngTimCont5 = TimeSpan.Zero;
										mcomCtrlXray_OnComm_sngTimCont6 = TimeSpan.Zero;
										break;
								}
								sngDelayTime = 20;			//20秒毎に表示
								sngTime = DateTime.Now.TimeOfDay;
								if ((sngTime - mcomCtrlXray_OnComm_sngTimCont1).TotalSeconds > sngDelayTime && mcomCtrlXray_OnComm_intERR[0] == modData.CTRL_ON)
								{
									//Err表示
									ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR1;
									mcomCtrlXray_OnComm_sngTimCont1 = TimeSpan.Zero;
								}
								else if (sngTime < mcomCtrlXray_OnComm_sngTimCont1)
								{
									mcomCtrlXray_OnComm_sngTimCont1 = TimeSpan.Zero;
								}
								if ((sngTime - mcomCtrlXray_OnComm_sngTimCont2).TotalSeconds > sngDelayTime && mcomCtrlXray_OnComm_intERR[1] == modData.CTRL_ON)
								{
									//Err表示
									ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR2;
									mcomCtrlXray_OnComm_sngTimCont2 = TimeSpan.Zero;
								}
								else if (sngTime < mcomCtrlXray_OnComm_sngTimCont2)
								{
									mcomCtrlXray_OnComm_sngTimCont2 = TimeSpan.Zero;
								}
								if ((sngTime - mcomCtrlXray_OnComm_sngTimCont3).TotalSeconds > sngDelayTime && mcomCtrlXray_OnComm_intERR[2] == modData.CTRL_ON)
								{
									//Err表示
									ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR3;
									mcomCtrlXray_OnComm_sngTimCont3 = TimeSpan.Zero;
								}
								else if (sngTime < mcomCtrlXray_OnComm_sngTimCont3)
								{
									mcomCtrlXray_OnComm_sngTimCont3 = TimeSpan.Zero;
								}
								if ((sngTime - mcomCtrlXray_OnComm_sngTimCont4).TotalSeconds > sngDelayTime && mcomCtrlXray_OnComm_intERR[3] == modData.CTRL_ON)
								{
									//Err表示
									ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR4;
									mcomCtrlXray_OnComm_sngTimCont4 = TimeSpan.Zero;
								}
								else if (sngTime < mcomCtrlXray_OnComm_sngTimCont4)
								{
									mcomCtrlXray_OnComm_sngTimCont4 = TimeSpan.Zero;
								}
								if ((sngTime - mcomCtrlXray_OnComm_sngTimCont5).TotalSeconds > sngDelayTime && mcomCtrlXray_OnComm_intERR[4] == modData.CTRL_ON)
								{
									//Err表示
									ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR5;
									mcomCtrlXray_OnComm_sngTimCont5 = TimeSpan.Zero;
								}
								else if (sngTime < mcomCtrlXray_OnComm_sngTimCont5)
								{
									mcomCtrlXray_OnComm_sngTimCont5 = TimeSpan.Zero;
								}
								if ((sngTime - mcomCtrlXray_OnComm_sngTimCont6).TotalSeconds > sngDelayTime && mcomCtrlXray_OnComm_intERR[5] == modData.CTRL_ON)
								{
									//Err表示
									ModuleCTRLM.ipErrsts = modData.ERR_XRAY_ERR209;
									mcomCtrlXray_OnComm_sngTimCont6 = TimeSpan.Zero;
								}
								else if (sngTime < mcomCtrlXray_OnComm_sngTimCont6)
								{
									mcomCtrlXray_OnComm_sngTimCont6 = TimeSpan.Zero;
								}
								//変更2010/02/24（KSS)hata １回しかError表示されないのため -----<

							}
							break;

						case CTRL_SSA:

							//----- ステータス自動送信確認 -----
							if (mstrRecvBuf.StartsWith(CTRL_SHT) == true)		//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								if (int.TryParse(strBuf, out iVal) == true)
								{
									modXray.gudtXrayP.intSSA = iVal;
								}
							}
							break;

						case CTRL_TYP:

							//----- 型名確認 -----
							if (mstrRecvBuf.StartsWith(CTRL_TYP) == true)		//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								modXray.gudtXrayP.strTYP = strBuf;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//								mblnGetType = true;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
							}
							break;

						case CTRL_SVV:

							//----- 真空計モニタ -----
							if (mstrRecvBuf.StartsWith(CTRL_SVV) == true)		//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								float.TryParse(strBuf, out modXray.gudtXrayP.sngSVV);		// 真空計ﾓﾆﾀ
							}
							break;

						case CTRL_SAT:

							//----- 自動X線OFF時間 -----
							if (mstrRecvBuf.StartsWith(CTRL_SAT) == true)		//コマンド確認
							{
								strBuf = mstrRecvBuf.Substring(3);
								int.TryParse(strBuf, out modXray.gudtXrayP.intSAT);
							}
							break;

						case CTRL_SHM:
							//----- フィラメント入力確認 -----
							if (mstrRecvBuf.StartsWith(CTRL_SHM) == true)		//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								int.TryParse(strBuf, out modXray.gudtXrayP.intSHM);
							}
							break;

						case CTRL_SHS:

							//----- フィラメント設定電圧確認 -----
							if (mstrRecvBuf.StartsWith(CTRL_SHS) == true)		//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								float.TryParse(strBuf, out modXray.gudtXrayP.sngSHS);
							}
							break;

                        //---L10711対応 20080715
                        case CTRL_SVS:
                            //Sモードフィラメント設定電圧確認
                            //----- フィラメント設定電圧確認 -----
                            if (mstrRecvBuf.StartsWith(CTRL_SVS) == true)		//コマンド確認
                            {
                                intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
                                strBuf = mstrRecvBuf.Substring(intBuf);
                                float.TryParse(strBuf, out modXray.gudtXrayP.sngSVS);
                            }
                            break;

                        //L10711対応 20080715---

						case CTRL_ZT1:

							//----- ターゲットの温度モニタ -----
							if (mstrRecvBuf.StartsWith(CTRL_ZT1) == true)		//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								int.TryParse(strBuf, out modXray.gudtXrayP.intZT1);			// 温度ﾓﾆﾀ
							}
							break;

						case CTRL_SWU:

							//----- ウォーミングアップ管電圧上昇下降パラメータ確認 -----
							if (mstrRecvBuf.StartsWith(CTRL_SWU) == true)		//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;			//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								modXray.gudtXrayP.strSWU = strBuf;
							}
							break;

//削除2009/08/24(KSS)hata_L10801対応に伴う変更 ---------->
//前に記述あり
//						Case CTRL_SWE
//							'ウォーミングアップ終了確認
//							gstrXrayDisp(DSP_SWE) = mstrRecvBuf  '表示用
//							If Left(mstrRecvBuf, Len(CTRL_SWE)) = CTRL_SWE Then 'コマンド確認
//								intBuf = InStr(1, mstrRecvBuf, " ") + 1        'データ開始位置取得
//								strBuf = Mid(mstrRecvBuf, intBuf)
//								If IsNumeric(strBuf) = True Then
//									With gudtXrayPropety2
//										Select Case val(strBuf)
//										Case mcintStsSweEnd
//											.E_XrayWarmupSWE = RET_SWE_WARMUP_END
//											.E_XrayWarmupSWR = RET_SWR_WARMUP_OFF
//										Case mcintStsSweOn
//											.E_XrayWarmupSWE = RET_SWE_WARMUP_YET
//											.E_XrayWarmupSWR = RET_SWR_WARMUP_ON
//										Case mcintStsSweYet
//											.E_XrayWarmupSWE = RET_SWE_WARMUP_YET
//											.E_XrayWarmupSWR = RET_SWR_WARMUP_OFF
//										Case mcintStsSweTime
//											.E_XrayWarmupSWE = RET_SWE_WARMUP_YET
//											.E_XrayWarmupSWR = RET_SWR_WARMUP_OFF
//										End Select
//									End With
//								End If
//							End If
//削除2009/08/24(KSS)hata_L10801対応に伴う変更 ----------<

						//v15.10追加（ここから） byやまおか 2009/11/12
						case CTRL_SMV:

							//----- 使用上限管電圧読み出し -----
							if (mstrRecvBuf.StartsWith(CTRL_SMV) == true)	//コマンド確認
							{
								intBuf = mstrRecvBuf.IndexOf(" ") + 1;	//データ開始位置取得
								strBuf = mstrRecvBuf.Substring(intBuf);
								modXray.gudtXrayP.intSMV = Convert.ToInt32(strBuf);
							}
							break;
						//v15.10追加（ここまで） byやまおか 2009/11/12

						case mcstrStsERR:
							//コマンドやパラメータに誤りがあったとき、または、コマンドを受け付けない状態の時に返信される
							//処理は行わない。
							break;

					}
				}
				catch
				{
                    //Debug.WriteLine("OnComm Err = " + mstrRecvBuf　);
                    //追加2014/11/12hata
                    mstrCommFlg = "";
                    mstrRecvBuf = "";
                }
				finally
				{
                    //削除2014/11/12hata
                    //retunでもここに入るため、ここはそのまま抜ける
                    //mstrCommFlg = "";
                    //mstrRecvBuf = "";
				}

                //追加2014/11/12hata
                mstrCommFlg = "";
                mstrRecvBuf = "";


			}

        }		// lock(gLock)

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
                //Debug.WriteLine("CommOut Err");

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
			//while(!mtmrCtrlXray.Sleep(Interval))
            while (CtrlXrayFlg)
            {
                int intRet = 0;
				bool blnRet = false;
				string strData = null;
				string strCommand = null;
				//送信文字列
				//Static blnSame          As Boolean  '追加2009/08/31(KSS)hata_L10801 同時設定対応   'v16.20削除(ファームが対応した) byやまおか 2010/04/05
				//Static sngTime          As Single   '追加2009/08/31(KSS)hata_L10801 同時設定対応   'v16.20削除(ファームが対応した) byやまおか 2010/04/05
				//Dim sngCntTime          As Single   '追加2009/08/31(KSS)hata_L10801 同時設定対応   'v16.20削除(ファームが対応した) byやまおか 2010/04/05

                mtmrCtrlXray.Sleep(Interval);//追加2014/06/09_hata
                if (!CtrlXrayFlg) break;   //追加2014/06/09_hata

                if (mtmrCtrlXray == null) return;
                if (mtmrCtrlXray.Stoped) return;
                if (!mtmrCtrlXray.IsAlive) return;

                lock(cCtrlm.gLock)
				{
                    try
                    {
                        if (mtmrCtrlXray_Tick_blnTmrFlg == true)
                        {
                            continue;
                        }

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
                            ModuleCTRLM.ifErrrst = 0;		//追加2010/02/24（KSS)hata　１回しかError表示されないのため
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
                        //変更2015/03/13hata
                        //追加2015/02/07_hata
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
                                //ModuleCTRLM.ipXStatus = 2;
                                //Rev20.01 test ここに入ってくるのは通信エラーを示すので0に変更 by長野 2015/05/21
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
                            //----- 送信タイムアウト -----
                            //							If DateDiff("s", varSendTime, Now) >= COMM_TIMEOUT Then             'v14.14追加 by 間々田 2008/02/20
                            if ((DateTime.Now - mtmrCtrlXray_Tick_varSendTime).TotalSeconds >= COMM_REPEAT_TIME)		//v14.14追加 by 間々田 2008/02/20
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
                                    //Debug.WriteLine("TimeOut");

                                    RecieveSts = false;
                                    //変更2015/02/07hata
                                    //throw new Exception("TimeOut");
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

                        
                        //Debug.WriteLine("CommTimeOut = " + mtmrCtrlXray_Tick_CommTimeOut.ToString());

                        //削除2009/08/25hata ----->
                        //					'起動時に一回通信を行う
                        //					If mblnCheckComm = False Then
                        //						If strCommand = "" Then
                        //							strCommand = CTRL_CHECK_COMM
                        //							mstrCommFlg = CTRL_CHECK_COMM
                        //							blnRet = CommOutput(strCommand)             'コマンド送信
                        //							If blnRet = False Then GoTo ErrorHandler
                        //							lngSendTime = Timer '送信した時刻を格納
                        //						End If
                        //						GoTo ExitHandler
                        //					End If
                        //削除2009/08/25hata -----<

                        //----- 異常確認 -----
                        intRet = ChkXray();
                        if (intRet != 0)
                        {
                            modData.gintStatusNoValue = intRet;		//ステータスNoセット
                            ModuleCTRLM.ifXrayonoff_Set = 2;		//X線をOFFする
                        }

                        AutoOff();				//自動X線OFF
                        Avail();				//アベイラブル
                        WattageCheck();			//電力制限         '追加2011/09/15(KS1)hata_X線電力制限に対応

                        //----- ステータスモニタ -----
                        switch (mintSendPointer)
                        {
                            case P_STS:		//状態確認
                                mstrCommFlg = CTRL_STS;
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

                            case P_SVI:		//設定電圧値電流値確認
                                mstrCommFlg = CTRL_SVI;
                                strCommand = mstrCommFlg;
                                break;

                            case P_STL:		// 動作値一括確認
                                mstrCommFlg = CTRL_STL;
                                strCommand = mstrCommFlg;
                                break;

                            case P_STG:		// ﾀｰｹﾞｯﾄ電流確認
                                mstrCommFlg = CTRL_STG;
                                strCommand = mstrCommFlg;
                                break;

                            //2009/08/21_未使用_hata
                            //							Case P_TGS  'ターゲット電流制限状態確認
                            //								mstrCommFlg = CTRL_TGS
                            //								strCommand = mstrCommFlg

                            case P_SFC:		//焦点モード確認
                                mstrCommFlg = CTRL_SFC;
                                strCommand = mstrCommFlg;
                                break;

                            case P_SOB:		// ﾌｫｰｶｽ値確認
                                mstrCommFlg = CTRL_SOB;
                                strCommand = mstrCommFlg;
                                break;

                            case P_SAD:		// ｱﾗｲﾒﾝﾄﾓﾆﾀ
                                mstrCommFlg = CTRL_SAD;
                                strCommand = mstrCommFlg;
                                break;

                            case P_SBX:		// X軸方向ｱﾗｲﾒﾝﾄ確認
                                mstrCommFlg = CTRL_SBX;
                                strCommand = mstrCommFlg;
                                break;

                            case P_SBY:		// Y軸方向ｱﾗｲﾒﾝﾄ確認
                                mstrCommFlg = CTRL_SBY;
                                strCommand = mstrCommFlg;
                                break;

                            case P_SVC:		// 真空状態確認
                                mstrCommFlg = CTRL_SVC;
                                strCommand = mstrCommFlg;
                                break;

                            case P_SIN:		//インタロック確認
                                mstrCommFlg = CTRL_SIN;
                                strCommand = mstrCommFlg;
                                break;

                            case P_FLM:		//フィラメント状態確認
                                mstrCommFlg = CTRL_FLM;
                                strCommand = mstrCommFlg;
                                break;

                            case P_STM:		//X線源通電時間確認
                                mstrCommFlg = CTRL_STM;
                                strCommand = mstrCommFlg;
                                break;

                            case P_SXT:		//X線照射時間確認
                                mstrCommFlg = CTRL_SXT;
                                strCommand = mstrCommFlg;
                                break;

                            case P_SHT:		// ﾌｨﾗﾒﾝﾄｵﾝ時間確認
                                mstrCommFlg = CTRL_SHT;
                                strCommand = mstrCommFlg;
                                break;

                            case P_SER:		//制御基板異常確認
                                mstrCommFlg = CTRL_SER;
                                strCommand = mstrCommFlg;
                                break;

                            case P_SSA:		//ステータス自動送信確認
                                mstrCommFlg = CTRL_SSA;
                                strCommand = mstrCommFlg;
                                break;

                            case P_TYP:		//型名確認
                                mstrCommFlg = CTRL_TYP;
                                strCommand = mstrCommFlg;
                                break;

                            case P_SVV:		//真空計モニタ
                                mstrCommFlg = CTRL_SVV;
                                strCommand = mstrCommFlg;
                                break;

                            case P_SAT:		//自動X線OFF時間
                                mstrCommFlg = CTRL_SAT;
                                strCommand = mstrCommFlg;
                                break;

                            case P_SHM:		//フィラメント入力
                                mstrCommFlg = CTRL_SHM;
                                strCommand = mstrCommFlg;
                                break;

                            case P_SHS:		//フィラメント設定電圧確認
                                mstrCommFlg = CTRL_SHS;
                                strCommand = mstrCommFlg;
                                break;

                            // ---L10711対応 20080710
                            case P_SVS: //Sモードフィラメント設定電圧確認 20080715
                                mstrCommFlg = CTRL_SVS;
                                strCommand = mstrCommFlg;
                                break;
                            case P_SAP: //アパーチャ電流確認
                                mstrCommFlg = CTRL_SAP;
                                strCommand = mstrCommFlg;
                                break;
                            case P_SCX: //コンデンサX方向アライメント確認
                                mstrCommFlg = CTRL_SCX;
                                strCommand = mstrCommFlg;
                                break;
                            case P_SCY: //コンデンサY方向アライメント確認
                                mstrCommFlg = CTRL_SCY;
                                strCommand = mstrCommFlg;
                                break;
                            case P_SMD: //フィラメントモード確認
                                mstrCommFlg = CTRL_SMD;
                                strCommand = mstrCommFlg;
                                break;
                            case P_SCW: //オブジェクトフォーカス値確認 20080822
                                mstrCommFlg = CTRL_SCW;
                                strCommand = mstrCommFlg;
                                break;
                            // L10711対応 20080710 ---

                            case P_SWE:		//ウォームアップ状態
                                mstrCommFlg = CTRL_SWE;
                                strCommand = mstrCommFlg;
                                break;

                            case P_ZT1:		//ターゲット部の温度モニタ
                                mstrCommFlg = CTRL_ZT1;
                                strCommand = mstrCommFlg;
                                break;

                            case P_SWU:		//ウォーミングアップ管電圧上昇下降パラメータ確認
                                mstrCommFlg = CTRL_SWU;
                                strCommand = mstrCommFlg;
                                break;

                            case P_SMV:		//使用上限管電圧読み出し  'v15.10追加 byやまおか 2009/11/12
                                mstrCommFlg = CTRL_SMV;
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

                                //----- ﾋﾞｰﾑｱﾗｲﾒﾝﾄ中断 -----
                                if (modXray.gudtXrayM.intSTP == modData.CTRL_ON)
                                {
                                    //ｺﾏﾝﾄﾞ文字列作成
                                    modXray.gudtXrayM.intSTP = modData.CTRL_OFF;
                                    strCommand = CTRL_STP;
                                    mstrCommFlg = CTRL_STP;

                                    goto SensCommand;
                                }

                                //---L10711対応 20080710
                                // フィラメントモード設定(Wカソード,Sカソード)
                                // WUP開始前のみ設定変更可能
                                if (ModuleCTRLM.ipX_On != modXray.RET_XRAY_ON &&
                                    ModuleCTRLM.ipWarmup_Mode == 1 &&
                                    ModuleCTRLM.ipWarmup != 1 &&
                                    modXray.gudtXrayP.intSAD != modXray.RET_SAD_ALL &&
                                    modXray.gudtXrayP.intSAD != modXray.RET_SAD_1 &&
                                    modXray.gudtXrayP.intSAD != modXray.RET_SAD_2 &&
                                    modXray.gudtXrayP.intSAD != modXray.RET_SAD_3 
                                    )
                                {

                                    if (modXray.gintMDE_Set == 1)
                                    {
                                        //ｺﾏﾝﾄﾞ文字列作成
                                        switch (modXray.gudtXrayM.intMDE)
                                        {
                                            case mcintSetMDE1:
                                                //intBuf = mcintStsMDE1;
                                                strCommand = CTRL_MDE + Convert.ToString(mcintSetMDE1);
                                                break;
                                            case mcintSetMDE2:
                                                //intBuf = mcintStsMDE2;
                                                strCommand = CTRL_MDE + Convert.ToString(mcintSetMDE2);                                              
                                                break;
                                            default:
                                                //strCommand = "";
                                                //mlngCommFlg = MODE_NULL;
                                                strCommand = CTRL_MDE + " " + Convert.ToString(0);
                                                modXray.gintMDE_Set = 0;
                                                goto SensCommand;
                                        }

                                        modXray.gudtXrayMTmp.intMDE = modXray.gudtXrayM.intMDE;
                                        mstrCommFlg = CTRL_MDE;
                                        modXray.gintMDE_Set = 0;

                                        //gintXrayCathoOld = gintXrayCatho;
                                        //strData.Format("%d", (int)intBuf);
                                        //strCommand = CTRL_MDE + strData;
                                        //mlngCommFlg = MODE_MDE;

                                        goto SensCommand;
                                    }
                                }
                                //L10711対応 20080710---

                                if (ModuleCTRLM.ipWarmup != 1 &&
                                    modXray.gudtXrayP.intSAD != modXray.RET_SAD_ALL &&
                                    modXray.gudtXrayP.intSAD != modXray.RET_SAD_1 &&
                                    modXray.gudtXrayP.intSAD != modXray.RET_SAD_2 &&
                                    modXray.gudtXrayP.intSAD != modXray.RET_SAD_3)
                                {
                                    //ウォームアップ中は実行不可

                                    //----- X線ONの送信 -----
                                    if (ModuleCTRLM.ifXrayonoff_Set == 1 && ModuleCTRLM.ipWarmup != 0)
                                    {
                                        strCommand = CTRL_XRAY_ON;
                                        mstrCommFlg = CTRL_XRAY_ON;
                                        ModuleCTRLM.ifXrayonoff_Set = 0;
                                        modXray.gudtXrayM.intWarmUp = 0;	//追加10/05/12(KS1)hata_L9421-02対応

                                        goto SensCommand;
                                    }
                                    else if ((ModuleCTRLM.ifXrayonoff_Set == 1 && ModuleCTRLM.ipWarmup == 0) ||
                                             (modXray.gudtXrayM.intWarmUp >= 1 && modXray.gudtXrayM.intWarmUp <= 3))
                                    {
                                        //----- ウォームアップ開始コマンド送信 -----
                                        if (modXray.gudtXrayM.intWarmUp > 1)
                                        {
                                            //WUP1,WUP2
                                            strCommand = CTRL_WARMUP + " " + (modXray.gudtXrayM.intWarmUp - 1);
                                        }
                                        else
                                        {
                                            //WUP
                                            strCommand = CTRL_WARMUP;
                                        }
                                        mstrCommFlg = CTRL_WARMUP;
                                        ModuleCTRLM.ifXrayonoff_Set = 0;
                                        modXray.gudtXrayM.intWarmUp = 0;

                                        goto SensCommand;
                                    }

                                    //変更2010/02/23(KSS)hata
                                    //ElseIf (ipX_On = CTRL_OFF And ipWarmup = 0 And ipXStatus = 5) Or ifWarmup_Reset = 1 Then
                                    else if (ModuleCTRLM.ifWarmup_Reset == 1)
                                    {
                                        //----- ウォームアップ強制終了コマンド送信 -----
                                        strCommand = CTRL_WARMUPEND;
                                        mstrCommFlg = CTRL_WARMUPEND;
                                        ModuleCTRLM.ifWarmup_Reset = 0;

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
                                    if ((modXray.gintTmpVol != ModuleCTRLM.ifX_Volt || modXray.gblnSendHIV == true) && modXray.gblnRecvPHV == false)
                                    {
                                        //v16.20削除(ここから) ファームが対応したため byやまおか 2010/04/05
                                        //										'追加2009/08/31(KSS)hata_L10801 同時設定対応
                                        //										If ifX_Amp <> gintTmpAmp Then
                                        //											blnSame = True
                                        //											sngTime = Timer
                                        //										Else
                                        //											blnSame = False
                                        //										End If
                                        //v16.20削除(ここまで) ファームが対応したため byやまおか 2010/04/05

                                        WattageCheck();		//電力制限         '追加2011/09/15(KS1)hata_X線電力制限に対応

                                        modXray.gblnRecvPHV = true;
                                        modXray.gblnSendHIV = false;

                                        strData = ModuleCTRLM.ifX_Volt.ToString("000");		// 設定値取得(V)
                                        strCommand = CTRL_HIVOLT + strData;					// 電圧値の設定コマンド生成
                                        mstrCommFlg = CTRL_HIVOLT;
                                        modXray.gintTmpVol = (int)ModuleCTRLM.ifX_Volt;
                                        modXray.gblnXrayValue = true;

                                        goto SensCommand;
                                    }

                                    //----- 管電流値の送信 -----
                                    if ((ModuleCTRLM.ifX_Amp != modXray.gintTmpAmp || modXray.gblnSendCUR == true) && modXray.gblnRecvPCU == false)
                                    {
                                        //v16.20削除(ここから) ファームが対応したため byやまおか 2010/04/05
                                        //										'変更2009/08/31(KSS)hata_L10801 同時設定対応 ----->
                                        //'										gblnRecvPCU = True
                                        //'										gblnSendCUR = False
                                        //'
                                        //'										strData = Format(ifX_Amp, "000")        ' 設定値取得(V)
                                        //'										strCommand = CTRL_CURRENT & strData                     ' 電流値の設定コマンド生成
                                        //'										mstrCommFlg = CTRL_CURRENT
                                        //'										gintTmpAmp = ifX_Amp
                                        //'										gblnXrayValue = True
                                        //'
                                        //'										GoTo SensCommand
                                        //
                                        //										If blnSame And ipX_On = 1 Then
                                        //											'5秒間管電流設定を待つ
                                        //											sngCntTime = Timer
                                        //											If sngTime < (sngCntTime - 5) Then
                                        //												blnSame = False
                                        //
                                        //											ElseIf sngTime > sngCntTime Then
                                        //												sngTime = sngCntTime
                                        //											End If
                                        //										Else
                                        //											blnSame = False
                                        //
                                        //											gblnRecvPCU = True
                                        //											gblnSendCUR = False
                                        //
                                        //											strData = Format(ifX_Amp, "000")        ' 設定値取得(V)
                                        //											strCommand = CTRL_CURRENT & strData                     ' 電流値の設定コマンド生成
                                        //											mstrCommFlg = CTRL_CURRENT
                                        //											gintTmpAmp = ifX_Amp
                                        //											gblnXrayValue = True
                                        //
                                        //											GoTo SensCommand
                                        //										End If
                                        //										'変更2009/08/31(KSS)hata_L10801 同時設定対応 -----<
                                        //v16.20削除(ここまで) ファームが対応したため byやまおか 2010/04/05

                                        WattageCheck();		//電力制限         '追加2011/09/15(KS1)hata_X線電力制限に対応

                                        //v16.20追加(ここから) byやまおか 2010/04/05
                                        //浜ホトファームが同時設定に対応したため元に戻す
                                        modXray.gblnRecvPCU = true;
                                        modXray.gblnSendCUR = false;

                                        strData = ModuleCTRLM.ifX_Amp.ToString("000");		// 設定値取得(V)
                                        strCommand = CTRL_CURRENT + strData;				// 電流値の設定コマンド生成
                                        mstrCommFlg = CTRL_CURRENT;
                                        modXray.gintTmpAmp = (int)ModuleCTRLM.ifX_Amp;
                                        modXray.gblnXrayValue = true;

                                        goto SensCommand;
                                        //v16.20追加(ここまで) byやまおか 2010/04/05

                                    }

                                    if (ModuleCTRLM.ipX_On != modXray.RET_XRAY_ON)
                                    {
                                        if ((ModuleCTRLM.ifFocussize != ModuleCTRLM.ipFocussize))
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
                                    }

                                    //----- ﾌｫｰｶｽ値設定 -----
                                    if (modXray.gintOBJ_Set == 1)
                                    {
                                        //If (gudtXrayM.sngOBJ <> gudtXrayMTmp.sngOBJ) Then
                                        if (modXray.gudtXrayM.sngOBJ > mcintMaxOBJ || modXray.gudtXrayM.sngOBJ < mcintMinOBJ)
                                        {
                                            //範囲外
                                        }
                                        else
                                        {
                                            //ｺﾏﾝﾄﾞ文字列作成
                                            strCommand = CTRL_OBJ + Convert.ToString(modXray.gudtXrayM.sngOBJ);
                                            mstrCommFlg = CTRL_OBJ;
                                            modXray.gudtXrayMTmp.sngOBJ = modXray.gudtXrayM.sngOBJ;
                                            modXray.gintOBJ_Set = 0;

                                            goto SensCommand;
                                        }
                                    }

                                    //----- ﾌｫｰｶｽ調整値保存 -----
                                    if (modXray.gudtXrayM.intSAV == modData.CTRL_ON)
                                    {
                                        //ｺﾏﾝﾄﾞ文字列作成
                                        modXray.gudtXrayM.intSAV = modData.CTRL_OFF;
                                        strCommand = CTRL_SAV;
                                        mstrCommFlg = CTRL_SAV;

                                        goto SensCommand;
                                    }

                                    //削除2009/08/25(KSS)hata ----->
                                    //L10801には無い
                                    //----- ﾌｫｰｶｽ値自動取得 -----
                                    //									If gintXrayOST = CTRL_ON Then
                                    //										'ｺﾏﾝﾄﾞ文字列作成
                                    //										gintXrayOST = CTRL_OFF
                                    //										strCommand = CTRL_OST
                                    //										mstrCommFlg = CTRL_OST

                                    //										GoTo SensCommand
                                    //									End If
                                    //削除2009/08/25(KSS)hata -----<

                                    //---L10711対応 20080710
                                    //----- コンデンサX方向ｱﾗｲﾒﾝﾄ設定 -----
                                    if (modXray.gintCAX_Set == 1)
                                    {
                                        if ((modXray.gudtXrayM.intCAX > mcintMaxCAX) || (modXray.gudtXrayM.intCAX < mcintMinCAX))
                                        {
                                            //範囲外
                                        }
                                        else
                                        {
                                            //ｺﾏﾝﾄﾞ文字列作成
                                            modXray.gudtXrayMTmp.intCAX = modXray.gudtXrayM.intCAX;
                                            strCommand = CTRL_CAX + Convert.ToString(modXray.gudtXrayM.intCAX);
                                            mstrCommFlg = CTRL_CAX;
                                            modXray.gintCAX_Set = 0;

                                            goto SensCommand;
                                        }
                                    }

                                    //----- コンデンサY方向ｱﾗｲﾒﾝﾄ設定 -----
                                    if (modXray.gintCAY_Set == 1)
                                    {
                                        if ((modXray.gudtXrayM.intCAY > mcintMaxCAY) || (modXray.gudtXrayM.intCAY < mcintMinCAY))
                                        {
                                            //範囲外
                                        }
                                        else
                                        {
                                            //ｺﾏﾝﾄﾞ文字列作成
                                            modXray.gudtXrayMTmp.intCAY = modXray.gudtXrayM.intCAY;
                                            strCommand = CTRL_CAY + Convert.ToString(modXray.gudtXrayM.intCAY);
                                            mstrCommFlg = CTRL_CAY;
                                            modXray.gintCAY_Set = 0;

                                            goto SensCommand;
                                        }
                                    }

                                    //L10711対応 20080710---

                                    //----- X方向ｱﾗｲﾒﾝﾄ設定 -----
                                    if (modXray.gintOBX_Set == 1)
                                    {
                                        //If gudtXrayM.intOBX <> gudtXrayMTmp.intOBX Then
                                        if (modXray.gudtXrayM.intOBX > mcintMaxOBX || modXray.gudtXrayM.intOBX < mcintMinOBX)
                                        {
                                            //範囲外
                                        }
                                        else
                                        {
                                            //ｺﾏﾝﾄﾞ文字列作成
                                            modXray.gudtXrayMTmp.intOBX = modXray.gudtXrayM.intOBX;
                                            strCommand = CTRL_OBX + Convert.ToString(modXray.gudtXrayM.intOBX);
                                            mstrCommFlg = CTRL_OBX;
                                            modXray.gintOBX_Set = 0;

                                            goto SensCommand;
                                        }
                                    }

                                    //----- Y方向ｱﾗｲﾒﾝﾄ設定 -----
                                    if (modXray.gintOBY_Set == 1)
                                    {
                                        //If gudtXrayM.intOBY <> gudtXrayMTmp.intOBY Then
                                        if (modXray.gudtXrayM.intOBY > mcintMaxOBY || modXray.gudtXrayM.intOBY < mcintMinOBY)
                                        {
                                            //範囲外
                                        }
                                        else
                                        {
                                            //ｺﾏﾝﾄﾞ文字列作成
                                            modXray.gudtXrayMTmp.intOBY = modXray.gudtXrayM.intOBY;
                                            strCommand = CTRL_OBY + Convert.ToString(modXray.gudtXrayM.intOBY);
                                            mstrCommFlg = CTRL_OBY;
                                            modXray.gintOBY_Set = 0;

                                            goto SensCommand;
                                        }
                                    }

                                    //----- ｱﾗｲﾒﾝﾄ調整値保存 -----
                                    if (modXray.gudtXrayM.intADJ_SAV == modData.CTRL_ON)
                                    {
                                        //ｺﾏﾝﾄﾞ文字列作成
                                        modXray.gudtXrayM.intADJ_SAV = modData.CTRL_OFF;
                                        strCommand = CTRL_SAV + " 1";
                                        mstrCommFlg = CTRL_SAV;

                                        goto SensCommand;
                                    }

                                    //----- ﾋﾞｰﾑｱﾗｲﾒﾝﾄ -----
                                    if (0 != modXray.gudtXrayM.intADJ)
                                    {
                                        //ｺﾏﾝﾄﾞ文字列作成
                                        strCommand = CTRL_ADJ;
                                        modXray.gudtXrayM.intADJ = 0;
                                        mstrCommFlg = CTRL_ADJ;

                                        goto SensCommand;
                                    }

                                    //----- 一括ﾋﾞｰﾑｱﾗｲﾒﾝﾄ -----
                                    if (modXray.gudtXrayM.intADA == modData.CTRL_ON)
                                    {
                                        //ｺﾏﾝﾄﾞ文字列作成
                                        modXray.gudtXrayM.intADA = modData.CTRL_OFF;
                                        strCommand = CTRL_ADA;
                                        mstrCommFlg = CTRL_ADA;

                                        goto SensCommand;
                                    }

                                    //									'----- ﾌｨﾗﾒﾝﾄON時間ﾘｾｯﾄ -----
                                    //									If .intCHT = CTRL_ON Then
                                    //										'ｺﾏﾝﾄﾞ文字列作成
                                    //										.intCHT = CTRL_OFF
                                    //										strCommand = CTRL_CHT
                                    //										mstrCommFlg = CTRL_CHT
                                    //										GoTo SensCommand
                                    //									End If
                                    //
                                    //									'----- ｵｰﾄﾘﾌﾞｰﾄ時間設定 -----
                                    //									If .intSRO = CTRL_ON Then
                                    //										'ｺﾏﾝﾄﾞ文字列作成
                                    //										.intSRO = CTRL_OFF
                                    //										strCommand = CTRL_SRO
                                    //										mstrCommFlg = CTRL_SRO
                                    //										GoTo SensCommand
                                    //									End If
                                    //
                                    //----- ﾘｾｯﾄ -----
                                    if (modXray.gudtXrayM.intRST == modData.CTRL_ON)
                                    {
                                        //ｺﾏﾝﾄﾞ文字列作成
                                        modXray.gudtXrayM.intRST = modData.CTRL_OFF;
                                        strCommand = CTRL_RST;
                                        mstrCommFlg = CTRL_RST;
                                        mintSovOver = modData.CTRL_OFF;		//追加2010/02/23(KSS)hata

                                        goto SensCommand;
                                    }

                                    //----- デフォルト値読み出し-----
                                    if (modXray.gudtXrayM.intDDL == modData.CTRL_ON)
                                    {
                                        //ｺﾏﾝﾄﾞ文字列作成
                                        int.TryParse(CTRL_DDL, out modXray.gudtXrayM.intDDL);
                                        strCommand = CTRL_DDL;
                                        mstrCommFlg = CTRL_DDL;

                                        goto SensCommand;
                                    }

                                    //----- 使用管電圧制限設定 -----     'v15.10追加 byやまおか 2009/11/12
                                    if (modXray.gintCMV_Set == 1)
                                    {
                                        //ｺﾏﾝﾄﾞ文字列作成
                                        strCommand = CTRL_CMV + Convert.ToString(modXray.gudtXrayM.intCMV);
                                        mstrCommFlg = CTRL_CMV;
                                        modXray.gudtXrayMTmp.intCMV = modXray.gudtXrayM.intCMV;
                                        modXray.gintCMV_Set = 0;

                                        goto SensCommand;
                                    }
                                }

                                //----- 過負荷保護解除 -----         'コメントから復活 by 間々田 2007/05/08 過負荷時Ｘ線オフしない現象への対策
                                if (mintSovOver == modData.CTRL_ON)	//コメントから復活 by 間々田 2007/05/08 過負荷時Ｘ線オフしない現象への対策
                                {
                                    mintSovOver = modData.CTRL_OFF;	//追加             by 間々田 2007/05/08 過負荷時Ｘ線オフしない現象への対策 この行がないと復帰した際、Ｘ線オンしてもすぐにオフしてしまう
                                    strCommand = CTRL_RST;			//コメントから復活 by 間々田 2007/05/08 過負荷時Ｘ線オフしない現象への対策
                                    mstrCommFlg = CTRL_RST;			//コメントから復活 by 間々田 2007/05/08 過負荷時Ｘ線オフしない現象への対策
                                    goto SensCommand;				//コメントから復活 by 間々田 2007/05/08 過負荷時Ｘ線オフしない現象への対策
                                }									//コメントから復活 by 間々田 2007/05/08 過負荷時Ｘ線オフしない現象への対策
                                break;
                        }

                    SensCommand:

                        //Debug.WriteLine("SensCommand " + strCommand);

                        if (!CtrlXrayFlg) break;    //追加2014/06/09_hata

                        //----- コマンド送信 -----
                        if (!string.IsNullOrEmpty(strCommand))
                        {
                            blnRet = CommOutput(strCommand);		//コマンド送信
                            //v19.40 送信失敗しても次の送信を行う by長野 2013/10/17
                            SendSts = blnRet;
                            //v19.40   'If blnRet = False Then GoTo ErrorHandler
                            //							If blnRet = False Then GoTo ErrorHandler
                            //							lngSendTime = Timer '送信した時刻を格納
                            mtmrCtrlXray_Tick_varSendTime = DateTime.Now;	//送信した時刻を格納  'v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
                        }

                        //----- ポインタ管理 次に送信するコマンドにポインタ移動 -----
                        switch (mintSendPointer)
                        {
                            case P_CTRL:
                                mintSendPointer = P_SAR;				//状態一括確認
                                break;
                            case P_SAR:
                                mintSendPointer = P_SNR;				//「NOT RADY」一括確認
                                break;
                            case P_SNR:
                                mintSendPointer = P_SVI;				//設定管電圧・管電流値
                                break;
                            case P_SVI:
                                mintSendPointer = P_SFC;				// 焦点モード確認
                                break;
                            case P_SFC:
                                mintSendPointer = P_SOB;				// フォーカス値確認
                                break;
                            case P_SOB:
                                mintSendPointer = P_SBX;				// X軸方向アライメント確認
                                break;
                            case P_SBX:
                                mintSendPointer = P_SBY;				// Y軸方向アライメント確認
                                break;
                            case P_SBY:
                                mintSendPointer = P_STM;				// X線通電時間
                                break;
                            case P_STM:
                                mintSendPointer = P_SXT;				// X線照射時間
                                break;
                            case P_SXT:
                                mintSendPointer = P_SHT;				// フィラメントON時間
                                break;
                            case P_SHT:
                                mintSendPointer = P_SSA;				// ステータス自動送信確認
                                break;
                            case P_SSA:
                                mintSendPointer = P_TYP;				// 型名確認
                                break;
                            case P_TYP:
                                mintSendPointer = P_SVV;				// 真空計モニタ
                                break;
                            case P_SVV:
                                mintSendPointer = P_SAT;				// 自動X線OFF時間
                                break;
                            case P_SAT:
                                mintSendPointer = P_SHS;				// フィラメント設定電圧確認
                                break;
                            case P_SHS:
                                mintSendPointer = P_SHM;				// フィラメント入力確認
                                break;
                            case P_SHM:
                                //mintSendPointer = P_SWE;				// ウォームアップ状態
                                mintSendPointer = P_SCX;                //Rev23.10 L10711 対応 by長野 2015/10/05
                                break;

                            //--- L10711対応 20080711
                            case P_SCX:  // コンデンサX方向アライメント確認
                                mintSendPointer = P_SCY;
                                break;
                            case P_SCY:  // コンデンサY方向アライメント確認
                                mintSendPointer = P_SMD;
                                break;
                            case P_SMD:  // フィラメントモード確認
                                mintSendPointer = P_FLM;
                                break;
                            case P_FLM:  // フィラメント状態確認
                                mintSendPointer = P_SCW;
                                break;
                            case P_SCW:  // コンデンサフォーカス値確認 20080822
                                mintSendPointer = P_SVS;
                                break;
                            // L10711対応 20080711 ---
                            //--- L10711対応 追加 (KS1)hata 2011.08.13
                            case P_SVS:  // オートX線OFF時間確認
                                mintSendPointer = P_SWE;
                                break;
                            // L10711対応 追加 (KS1)hata 2011.08.13 ---

                            case P_SWE:
                                mintSendPointer = P_ZT1;				// ターゲット温度モニタ
                                break;
                            //							Case P_ZT1: mintSendPointer = P_CTRL       ' コントロールコマンド
                            case P_ZT1:
                                mintSendPointer = P_SWU;				// ウォーミングアップ管電圧上昇下降パラメータ確認
                                break;
                            //Case P_SWU: mintSendPointer = P_CTRL        ' コントロールコマンド
                            case P_SWU:
                                mintSendPointer = P_SMV;				// 使用上限管電圧読み出し    'v15.10追加 byやまおか 2009/11/12
                                break;
                            case P_SMV:
                                mintSendPointer = P_CTRL;				// コントロールコマンド      'v15.10変更 byやまおか 2009/11/12
                                break;
                            default:
                                mintSendPointer = P_CTRL;
                                mstrCommFlg = "";
                                break;
                        }

                        #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                        //ExitHandler:
                        #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                        mtmrCtrlXray_Tick_blnTmrFlg = false;

                        //						gintTmrCtrl = 0
                        //'						intOldErrXrayComm = CTRL_OFF

                        //Debug.Print "X線out(" + Str(Now) + ")"
                    }
                    catch			//エラー処理
                    {
                        //変更2015/02/07hata
                        CommTimeOutError = true;
                    //    //Debug.WriteLine("mtmrCtrlXray_Timer Err");

                    //    if (modXray.gintOldErrXrayComm == modData.CTRL_OFF)
                    //    {
                    //        //Resume Next '削除2010/02/24_hata
                    //        //削除2009/08/21_hata
                    //        //							gintStatusNoValue = STS_XRAY_COMM   '通信エラー
                    //        modXray.gintOldErrXrayComm = modData.CTRL_ON;
                    //        ModuleCTRLM.ipErrsts = modData.ERR_XRAY_COMM;
                    //    }

                    //    StreamWriter sw;

                    //    if (LogWriteRFlg == false)
                    //    {
                    //        //v19.40 ログを残す by長野 2013/10/17
                    //        if (RecieveSts == false)
                    //        {
                    //            sw = new StreamWriter(@"C:\CTUSER\XrayCommERR.log", true);
                    //            sw.WriteLine(string.Format("{0}-{1}-{2}", DateTime.Now, mtmrCtrlXray_Tick_CommTimeOut, mstrCommFlg));
                    //            sw.Close();
                    //        }

                    //        LogWriteRFlg = true;
                    //    }

                    //    if (LogWriteSFlg == false)
                    //    {
                    //        if (SendSts == false)
                    //        {
                    //            sw = new StreamWriter(@"C:\CTUSER\XrayCommERR.log", true);
                    //            sw.WriteLine(string.Format("{0}-{1}-{2}", DateTime.Now, mtmrCtrlXray_Tick_CommTimeOut, mstrCommFlg));
                    //            sw.Close();
                    //        }

                    //        LogWriteSFlg = true;
                    //    }

                    //    mtmrCtrlXray_Tick_blnTmrFlg = false;

                    //    //2003-10-15 Shibui
                    //    //						gintTmrCtrl = 0

                    //    //Debug.Print "X線out(" + Str(Now) + ")"
                    
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
                            try
                            {
                                FileInfo finfoS = new FileInfo(file + ext);
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
                    //追加2015/03/16hata
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

		/// <summary>
		/// X線ON中エラー確認
		/// </summary>
		/// <returns></returns>
		//変更2012/03/25(KS1)hata_違う文字が戻る（入る）ため
		private int ChkXray()
		//Private Function ChkXray()
		{
			//削除2009/08/21_hata
			//使用していない
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
			//    If gudtXrayStatus1Value.E_XrayOnSTX <> RET_XRAY_ON Then
			//        intAns = STS_OK
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
			//ExitHandler:
			//
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

			try
			{
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
							AutoOff_varTimXray = DateTime.Now; //v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
							AutoOff_intTimeStart = modData.CTRL_ON;
						}

						//経過時間確認
						//If intTimeStart = CTRL_ON And (Timer - sngTimXray) > ifXtimer Then
						if (AutoOff_intTimeStart == modData.CTRL_ON && (DateTime.Now - AutoOff_varTimXray).TotalSeconds > ModuleCTRLM.ifXtimer) //v14.14変更 by 間々田 2008/02/20
						{
							//X線OFF
							ModuleCTRLM.ifXrayonoff_Set = 2;
							AutoOff_intTimeStart = modData.CTRL_OFF;
						}
					}

					AutoOff_intOldXrayOn = ModuleCTRLM.ipX_On;		//X線の状態を保持
				}
				ModuleCTRLM.ipXcont_Mode = ModuleCTRLM.ifXcont_Mode;
				ModuleCTRLM.ipXtimer = ModuleCTRLM.ifXtimer;
			}
			catch
			{
                //Debug.WriteLine("AutoOff Err");
            }
		}

		/// <summary>
		/// アベイラブル
		/// </summary>
		private void Avail()
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Static blnXrayOnOff             As Boolean
			Static blnOldXrayOnOff          As Boolean
			Static intOldXrayOnAvail        As Integer
			Static intOldVoltageAvail       As Integer
			Static intOldCurrentAvail       As Integer
			Static intXrayAvailOnRun        As Integer
			Static intXrayAvailInsideRun    As Integer
			Static intXrayAvailTime         As Integer
			Static intXrayAvailOk           As Integer
			Static intOldXrayVoltage        As Integer
			Static intOldXrayCurent         As Integer
			Static intOldXrayVoltageSPV     As Integer
			Static intOldXrayCurentSPC      As Integer
			'Static sngXrayAvailStartTime    As Single
			Static varXrayAvailStartTime    As Variant 'v14.14変更 by 間々田 2008/02/20
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//----- アベイラブル -----
			if (ModuleCTRLM.ipX_On == 1 && ModuleCTRLM.ipWarmup != 1)
			{
				if (ModuleCTRLM.ifXrayonoff_Set == 1)
				{
					Avail_blnXrayOnOff = true;
				}
				else if (ModuleCTRLM.ifXrayonoff_Set == 2)
				{
					Avail_blnXrayOnOff = false;
				}

				//X線ON監視
				if (Avail_intOldXrayOnAvail != ModuleCTRLM.ipX_On || Avail_blnXrayOnOff != Avail_blnOldXrayOnOff)
				{
					Avail_intXrayAvailOnRun = modData.CTRL_ON;								//X線ONアベイラブル判定スタート
					Avail_intXrayAvailTime = modData.gudtXraySystemValue.E_XrayAvailTimeOn;	//X線ONアベイラブル時間をセット
					Avail_intXrayAvailOk = modData.CTRL_OFF;
					Avail_intOldXrayOnAvail = ModuleCTRLM.ipX_On;
					Avail_intOldXrayVoltage = (int)ModuleCTRLM.ipcndVolt;	//X線ON時には、管電圧・管電流アベイラブル判定を行わない
					Avail_intOldXrayCurent = (int)ModuleCTRLM.ipcndAmp;		//X線ON時には、管電圧・管電流アベイラブル判定を行わない
					Avail_blnOldXrayOnOff = Avail_blnXrayOnOff;
				}

				//設定値変更監視
				if (Avail_intOldXrayVoltage != ModuleCTRLM.ifX_Volt ||
					Avail_intOldXrayCurent != ModuleCTRLM.ifX_Amp ||
					modXray.gblnXrayValueSet == true ||
					Avail_intOldXrayVoltageSPV != ModuleCTRLM.ipcndVolt ||
					Avail_intOldXrayCurentSPC != ModuleCTRLM.ipcndAmp)
				{

					modXray.gblnXrayValueSet = false;

					Avail_intXrayAvailInsideRun = modData.CTRL_ON;								//設定値変更アベイラブルスタート
					Avail_intXrayAvailTime = modData.gudtXraySystemValue.E_XrayAvailTimeInside;	//設定値変更アベイラブル時間をセット
					Avail_intXrayAvailOk = modData.CTRL_OFF;
					Avail_intOldXrayVoltage = (int)ModuleCTRLM.ifX_Volt;
					Avail_intOldXrayCurent = (int)ModuleCTRLM.ifX_Amp;
					Avail_intOldXrayVoltageSPV = (int)ModuleCTRLM.ipcndVolt;
					Avail_intOldXrayCurentSPC = (int)ModuleCTRLM.ipcndAmp;
				}

				//アベイラブル範囲判定
				if ((Avail_intXrayAvailOnRun == modData.CTRL_ON || Avail_intXrayAvailInsideRun == modData.CTRL_ON) &&
					(ModuleCTRLM.ipX_Volt <= ModuleCTRLM.ipcndVolt + modData.gudtXraySystemValue.E_XrayAvailkV) &&
					(ModuleCTRLM.ipX_Volt >= ModuleCTRLM.ipcndVolt - modData.gudtXraySystemValue.E_XrayAvailkV) &&
					(ModuleCTRLM.ipX_Amp <= ModuleCTRLM.ipcndAmp + modData.gudtXraySystemValue.E_XrayAvailuA) &&
					(ModuleCTRLM.ipX_Amp >= ModuleCTRLM.ipcndAmp - modData.gudtXraySystemValue.E_XrayAvailuA) &&
					modXray.gblnSendCUR == false && modXray.gblnSendHIV == false && modXray.gblnRecvPHV == false && modXray.gblnRecvPCU == false)
				{

					if (Avail_intXrayAvailOk != modData.CTRL_ON)
					{
						Avail_varXrayAvailStartTime = DateTime.Now;	//v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
					}

					Avail_intXrayAvailOk = modData.CTRL_ON;
				}

				//アベイラブル時間経過判定

				//v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
				if (Avail_intXrayAvailOk == modData.CTRL_ON && (DateTime.Now - Avail_varXrayAvailStartTime).TotalSeconds >= Avail_intXrayAvailTime)
				{

					ModuleCTRLM.ipXAvail = 1;		//アベイラブルON
					Avail_intXrayAvailOnRun = modData.CTRL_OFF;			//アベイラブル判定停止
					Avail_intXrayAvailInsideRun = modData.CTRL_OFF;		//アベイラブル判定停止
				}
				else
				{
					ModuleCTRLM.ipXAvail = 0;	//アベイラブルOFF
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
		
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			ExitHandler:
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
		}


		//追加2011/09/15(KS1)hata_X線電力制限に対応
		/// <summary>
		/// 電力値が制限電力値を超えてる場合に、電流値を制限する
		/// 制限電力値で計算した最大電流値に設定する
		/// </summary>
		private void WattageCheck()
		{
			float mVolt = 0;
			float mfVolt = 0;			//設定値電圧値
			float mfAmp = 0;			//設定値電流値
			float mpVolt = 0;			//出力電流値
			float mpAmp = 0;			//出力電流値
			float WatLimitVal = 0;		//制限電力値
			float CalWatVal = 0;		//設定値で計算した電力
			float CalWatOutVal = 0;		//出力値で計算した電力
			float CalLimitAmp = 0;		//制限電力値から計算した電流制限値
			float CalVal = 0;			//計算した電力


			mfVolt = ModuleCTRLM.ifX_Volt;			//設定値電圧
			mfAmp = ModuleCTRLM.ifX_Amp;			//設定値電流
			mpVolt = ModuleCTRLM.ipX_Volt;			//設定値電圧
			mpAmp = ModuleCTRLM.ipX_Amp;			//設定値電流

			WatLimitVal = modData.gudtXraySystemValue.E_XrayWattageLimit;
			CalWatVal = (float)(mfVolt / 1000000.0 * mfAmp * 1000.0);
			CalWatOutVal = (float)(mpVolt / 1000000.0 * mpAmp * 1000.0);

			if (CalWatOutVal > CalWatVal)
			{
				//出力値を使用
				CalVal = CalWatOutVal;
				mVolt = mpVolt;
			}
			else
			{
				//設定値を使用
				CalVal = CalWatVal;
				mVolt = mfVolt;
			}

			//電流制限値
			CalLimitAmp = (float)(WatLimitVal / mVolt * 1000.0);

    		//電力制限値を超えていたら、電流制限値に設定する
	    	if (CalVal > WatLimitVal)
		    {
			    ModuleCTRLM.ifX_Amp = (float)Math.Floor(CalLimitAmp);
			}
		}
	}
}
