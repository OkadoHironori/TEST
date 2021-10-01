using System;
using System.Threading;
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
using System.Text;
using System.Globalization;

namespace XrayCtrl
{
    //***********************************************************************************
    //
    //   X線制御160kV（L9191）
    //
    //   Date        Version     Designed/Changed
    //   2004/01/14  1.00        (NSD)Shibui
    //
    //***********************************************************************************
    public class clsCtrlXrayL9191 : IDisposable
    {    
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*    
        Private mfrmCtrlXray                As frmXray                      ' X線制御用フォーム

        Private WithEvents mtmrCtrlXray     As Timer                        ' タイマ
        Private WithEvents mcomCtrlXray     As MSComm                       ' 通信
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

        private ThreadEx mtmrCtrlXray;		// スレッド
        private SerialPort mcomCtrlXray;    // 通信
        
        private string mstrCommFlg;         // 通信ハンドシェイク用        
        private int mintSovOver;            // 過負荷保護機能状態       'コメントから復活 by 間々田 2007/05/08
                
		private const int Interval = 20;	// スレッド周期

        private const int COMM_PORT = 1;    // 通信ポート

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
/*            
        private const string COMM_SETTINGS = "115200,n,8,1";    // ボーレート
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

        //Private Const COMM_TIMEOUT          As Integer = 10                 ' 返信タイムアウト時間[ms]
        //v19.40 送受信の仕組みを変更　ここから 2013/10/17 by長野
        //時間内に受信できなかったら、次の送信へ行く。
        //上記処理を一定時間繰り返すようであれば、タイムアウトにする。
        private const int COMM_TIMEOUT = 10;        // 返信タイムアウト時間[s]        
        private const int COMM_REPEAT_TIME = 2;     //繰り返し時間        
        //追加2015/02/07hata
        private const int COMM_LINE_TIMEOUT = 4;			// CommラインOFFによるタイムアウト時間[s]

        private bool RecieveSts;                    //返信状態フラグ(True:正常、False:異常)        
        private bool SendSts;                       //送信状態フラグ(True:正常、False:異常)
        private bool LogWriteRFlg;
        private bool LogWriteSFlg;
        
        private const int COMM_RETRY = 3;           // 通信リトライ回数
        
        private string mstrRecvBuf;
        //2004-09-29 Shibui
        //Private mAvailTimer                 As DelayType                    ' Availタイマ
        //Private mAutoOffTimer               As DelayType                    ' AutoOffタイマ

        //Private gintOldErrXrayComm          As Integer                      '通信異常フラグ

        //2003-11-18 Shibui        
        private bool mblnCheckComm;         //通信確認用

        //2004-07-08 Shibui        
        private int mintSendPointer;        //制御用

        //----------------------------------------------------------------------------------------------
        //   X線装置コマンド
        //----------------------------------------------------------------------------------------------

        //----- コマンドポインタ -----	
        private const int P_STX = 1;				// X線照射確認
        private const int P_SHV = 2;				// 出力電圧確認
        private const int P_SCU = 3;				// 出力電流確認
        private const int P_SPV = 4;				// 管電圧プリセット確認
        private const int P_SPC = 5;				// 管電流プリセット確認
        private const int P_SOV = 6;				// 過負荷保護機能確認
        private const int P_SIN = 7;				// インターロック確認
        private const int P_SWR = 8;				// ウォーミングアップ状態確認
        private const int P_STI = 9;				// 状態確認
        private const int P_SXT = 10;				// X線照射時間確認
        private const int P_SWE = 11;				// ウォーミングアップ終了確認
        private const int P_SFC = 12;				// 焦点モード確認
        private const int P_STM = 13;				// X線源通電時間確認
        private const int P_TYP = 14;				// 型名確認
        private const int P_SER = 15;				// 制御基板異常確認
        private const int P_SVI = 16;				// 設定管電圧管電流値確認

        private const int P_STG = 17;				// ﾀｰｹﾞｯﾄ電流確認
        private const int P_SOB = 18;				// ﾌｫｰｶｽ値確認
        private const int P_SBX = 19;				// X軸方向ｱﾗｲﾒﾝﾄ確認
        private const int P_SBY = 20;				// Y軸方向ｱﾗｲﾒﾝﾄ確認
        private const int P_SAD = 21;				// ｱﾗｲﾒﾝﾄﾓﾆﾀ
        private const int P_SVV = 22;				// 真空度ﾓﾆﾀ
        private const int P_SVC = 23;				// 真空状態確認
        private const int P_STA = 24;				// 状態一括確認
        private const int P_STL = 25;				// 動作値一括確認
        private const int P_SHM = 26;				// ﾌｨﾗﾒﾝﾄ入力確認
        private const int P_SHS = 27;				// ﾌｨﾗﾒﾝﾄ設定電圧確認
        private const int P_SHT = 28;				// ﾌｨﾗﾒﾝﾄｵﾝ時間確認
        private const int P_SAT = 29;				// 自動X線停止時間

        private const int P_CTRL = 0;				// コントロールコマンド

        //----- コントロールコマンド -----					

        //L9181,L9191共用
        private const string CTRL_HIVOLT = "HIV ";					// 管電圧設定[V]    (0～160)
        private const string CTRL_CURRENT = "CUR ";					// 管電流設定[uA]   (0～200)
        private const string CTRL_XRAY_ON = "XON";					// Ｘ線出射開始
        private const string CTRL_XRAY_OFF = "XOF";					// Ｘ線出射停止
        private const string CTRL_MODE_FOCUS = "CFS ";				// ﾌｫｰｶｽﾓｰﾄﾞｾｯﾄ  (1～4)
        //Private Const CTRL_AUTO_STOP        As String = "AST"               ' 自動X線停止時間設定
        private const string CTRL_RST = "RST";					// 過負荷保護解除
        private const string CTRL_WARMUP = "WUP";					// ウォーミングアップ開始

        //L9191専用
        private const string CTRL_OBJ = "OBJ ";					// ﾌｫｰｶｽ値設定（0～15000）
        private const string CTRL_SAV = "SAV";					// ﾌｫｰｶｽ調整値保存
        private const string CTRL_OST = "OST";					// ﾌｫｰｶｽ値自動取得
        private const string CTRL_OBX = "OBX ";					// X方向ｱﾗｲﾒﾝﾄ設定
        private const string CTRL_OBY = "OBY ";					// Y方向ｱﾗｲﾒﾝﾄ設定
        private const string CTRL_ADJ = "ADJ ";					// ﾋﾞｰﾑｱﾗｲﾒﾝﾄ
        private const string CTRL_ADA = "ADA";					// 一括ﾋﾞｰﾑｱﾗｲﾒﾝﾄ
        private const string CTRL_STP = "STP";					// ﾋﾞｰﾑｱﾗｲﾒﾝﾄ中断
        //Private Const CTRL_CHT              As String = "CHT"               ' ﾌｨﾗﾒﾝﾄｵﾝ時間ﾘｾｯﾄ					
        //Private Const CTRL_SRO              As String = "SRO "              ' ｵｰﾄﾘﾌﾞｰﾄ時間設定 0/5～60					
        //Private Const CTRL_RMT              As String = "RMT"               ' ﾘﾓｰﾄ					

        //----- ステータスコマンド -----
        private const string CTRL_STX = "STX";					// X線照射確認

        private const string CTRL_SHV = "SHV";					// 出力電圧確認
        private const string CTRL_SCU = "SCU";					// 出力電流確認
        private const string CTRL_SPV = "SPV";					// 管電圧プリセット確認
        private const string CTRL_SPC = "SPC";					// 管電流プリセット確認
        private const string CTRL_SOV = "SOV";					// 過負荷保護機能確認

        private const string CTRL_SIN = "SIN";					// インターロック確認
        private const string CTRL_SWR = "SWR";					// ウォーミングアップ状態確認
        private const string CTRL_STI = "STI";					// 状態確認
        private const string CTRL_SXT = "SXT";					// X線照射時間確認
        //Private Const CTRL_SWS              As String = "SWS"               ' ウォーミングアップステップ確認
        private const string CTRL_SWE = "SWE";					// ウォーミングアップ終了確認
        private const string CTRL_SFC = "SFC";					// 焦点モード確認
        private const string CTRL_STM = "STM";					// X線源通電時間確認

        //2004-09-15 Shibui					
        //Private Const CTRL_SAT              As String = "SAT"               ' 自動X線停止機能確認
        private const string CTRL_SAT = "SRT";					// 自動X線停止機能確認

        private const string CTRL_TYP = "TYP";					// 型名確認
        private const string CTRL_SER = "SER";					// 制御基板異常確認
        //Private Const CTRL_STC              As String = "STC"               ' 一括状態確認					
        //Private Const CTRL_STT              As String = "STT"               ' 動作管電圧管電流カソード値確認
        private const string CTRL_SVI = "SVI";					// 設定管電圧管電流値確認

        //L9191専用
        private const string CTRL_STG = "STG";					// ﾀｰｹﾞｯﾄ電流確認
        private const string CTRL_STA = "STA";					// 一括状態確認
        private const string CTRL_STL = "STL";					// 動作値一括確認
        private const string CTRL_SOB = "SOB";					// ﾌｫｰｶｽ値確認
        private const string CTRL_SBX = "SBX";					// X方向ｱﾗｲﾒﾝﾄ確認
        private const string CTRL_SBY = "SBY";					// Y方向ｱﾗｲﾒﾝﾄ確認
        private const string CTRL_SAD = "SAD";					// ｱﾗｲﾒﾝﾄﾓﾆﾀ
        private const string CTRL_SVV = "SVV";					// 真空計ﾓﾆﾀ
        private const string CTRL_SVC = "SVC";					// 真空状態確認
        //Private Const CTRL_SRT              As String = "SRT"               ' ｵｰﾄﾘﾌﾞｰﾄ時間確認				
        private const string CTRL_SHM = "SHM";					// ﾌｨﾗﾒﾝﾄ入力確認
        private const string CTRL_SHS = "SHS";					// ﾌｨﾗﾒﾝﾄ設定電圧確認
        private const string CTRL_SHT = "SHT";					// ﾌｨﾗﾒﾝﾄｵﾝ時間確認

        //2003-11-18 Shibui
        private const string CTRL_CHECK_COMM = "Enter";					// 通信確認用


        //----- X線制御機から返されるデータ -----				
        //Private Const STAT_XRAY_ON          As String = "X-RAY ON"          ' X線照射確認 X線ON				
        //Private Const STAT_XRAY_OFF         As String = "X-RAY OFF"         ' X線照射確認 X線OFF				
        //Private Const STAT_SHV              As String = "HIV"               ' 出力管電圧				
        //Private Const STAT_SCU              As String = "CUR"               ' 出力管電流				
        //Private Const STAT_SPV              As String = "PHV"               ' 設定管電圧				
        //Private Const STAT_SPC              As String = "PCU"               ' 設定管電流				
        //Private Const STAT_OVER             As String = "OVER"              ' 過負荷保護機能 動作中				
        //Private Const STAT_NORMAL           As String = "NORMAL"            ' 過負荷保護機能 動作なし				
        //Private Const STAT_RET_INTER_LOCK_ON    As String = "INTER LOCK ON"     ' インタロック閉じている				
        //Private Const STAT_RET_INTER_LOCK_OFF   As String = "INTER LOCK OFF"    ' インタロック開いている				
        //Private Const STAT_WARMUP_ON        As String = "WARMUP ON"         ' ウォーミングアップ中				
        //Private Const STAT_WARMUP_OFF       As String = "WARMUP OFF"        ' ウォーミングアップ中ではない				
        //Private Const STAT_PREHEAT          As String = "PREHEAT"           ' プリヒート中				
        //Private Const STAT_NOT_READY        As String = "NOT READY"         ' X線が照射できない状態				
        //Private Const STAT_WARMUP           As String = "WARMUP"            ' ウォーミングアップ動作中				
        //Private Const STAT_XON              As String = "XON"               ' X線照射中				
        //Private Const STAT_WARMUP_YET       As String = "WARMUP YET"        ' ウォーミングアップ待機中				
        //Private Const STAT_STANDBY          As String = "STANDBY"           ' X線照射待機中				
        //Private Const STAT_SWS              As String = "WARMUP"            ' ウォーミングアップのモードとステップ	
        private const string STAT_WARMUP_END = "WARMUP END";	            // ウォーミングアップ終了
        //Private Const STAT_SFC              As String = "FCS"               ' 焦点				
        //Private Const STAT_SFC_1            As String = "FCS 1"             ' F1				
        //Private Const STAT_SFC_2            As String = "FCS 2"             ' F2				
        //Private Const STAT_SFC_3            As String = "FCS 3"             ' F3				
        //Private Const STAT_SFC_4            As String = "FCS 4"             ' F4				
        //Private Const STAT_SAT              As String = "Auto Off Time"     ' 自動X線停止機能確認
        private const string STAT_SER = "ERR";				                // 制御基板異常確認
        //Private Const STAT_STC              As String = "STC"               ' 一括状態確認				
        //Private Const STAT_STT              As String = "STT"               ' 動作管電圧管電流カソード値確認
        private const string STAT_SVI = "SVI";				// 設定電圧値電流値確認

        //Private Const STAT_OBJ              As String = "OBJ"               ' ﾌｫｰｶｽ値設定				
        //Private Const STAT_OST              As String = "OST"               ' ﾌｫｰｶｽ値設定				

        //Private Const STAT_SBX              As String = "OBX"               ' X軸方向のｱﾗｲﾒﾝﾄ値				
        //Private Const STAT_SBY              As String = "OBY"               ' Y軸方向のｱﾗｲﾒﾝﾄ値
        private const string STAT_SAD_OFF = "ADJ OFF";				// ﾋﾞｰﾑｱﾗｲﾒﾝﾄを行っていない状態
        private const string STAT_SAD_2 = "ADJ 2";				    // ﾋﾞｰﾑｱﾗｲﾒﾝﾄ中
        private const string STAT_SAD_3 = "ADJ 3";				    // ﾋﾞｰﾑｱﾗｲﾒﾝﾄ中
        private const string STAT_SAD_1_3 = "ADJ 1/3";				// 一括ﾋﾞｰﾑｱﾗｲﾒﾝﾄ中

        //Private Const STAT_SOB              As String = "OBJ"               ' 電磁ﾚﾝｽﾞのﾌｫｰｶｽ値				
        //Private Const STAT_SVV              As String = "V"                 ' 真空計ﾓﾆﾀの数値を返す（?.?? V）
        private const string STAT_SVC_LO = "VAC Lo";				// 低真空状態
        private const string STAT_SVC_NG = "VAC NG";				// 真空計が正常に動作していない状態
        private const string STAT_SVC_V1 = "VAC V1";				// 数字が大きくなるほど高真空状態（V2以上になると、X線は操作可能になる）
        private const string STAT_SVC_V2 = "VAC V2";				// 数字が大きくなるほど高真空状態（V2以上になると、X線は操作可能になる）
        private const string STAT_SVC_V3 = "VAC V3";				// 数字が大きくなるほど高真空状態（V2以上になると、X線は操作可能になる）
        private const string STAT_SVC_V4 = "VAC V4";				// 数字が大きくなるほど高真空状態（V2以上になると、X線は操作可能になる）
        private const string STAT_SVC_V5 = "VAC V5";				// 数字が大きくなるほど高真空状態（V2以上になると、X線は操作可能になる）

        //Private Const STAT_STA              As String = "STA"               ' X線管の状態を一括で返す（0x00000000）				
        //Private Const STAT_STL              As String = "STL"               ' 動作管電圧、動作管電流、ﾀｰｹﾞｯﾄ電流を一括で返す（000000000）				
        //Private Const STAT_STM              As String = ""                  ' X線ｺﾝﾄﾛｰﾙﾕﾆｯﾄの電源ON時間を返す（000000）				
        //Private Const STAT_SXT              As String = ""                  ' X線照射時間				
        //Private Const STAT_SHM              As String = "HTR"               ' ﾌｨﾗﾒﾝﾄ通電のための入力電流を返します（[HTR N 0mA]H:X線ｵﾝ時、L:X線OFF時、A:ｳｫｰﾐﾝｸﾞｱｯﾌﾟ時、N:?）				
        //Private Const STAT_SHS              As String = ""                  ' ﾌｨﾗﾒﾝﾄ印加電圧の設定値を返します（[H:1.65 V/L:0.0 V]H:X線ｵﾝ時、L:X線OFF時）				
        //Private Const STAT_SHT              As String = ""                  ' ﾌｨﾗﾒﾝﾄ通電時間

        private const string STAT_ERROR = "ERROR";				//コマンドやパラメータに誤りがあったとき、または、コマンドを受け付けない状態の時に返信される

        //復活2009/08/24(KSS)hata_L10801対応に伴う変更 ---------->				
        //L10801追加のためｸﾞﾛｰﾊﾞﾙからﾌﾟﾗｲﾍﾞｰﾄへ				
        //Private Const MAX_OBJ   As Integer = 15000  'ﾌｫｰｶｽ値 最大値
        private const float MAX_OBJ = 1500;				//ﾌｫｰｶｽ値 最大値
        private const int MIN_OBJ = 0;				    //ﾌｫｰｶｽ値 最小値
        private const int MAX_OBX = 1000;				//X方向ｱﾗｲﾒﾝﾄ 最大値
        private const int MIN_OBX = -1000;				//X方向ｱﾗｲﾒﾝﾄ 最小値
        private const int MAX_OBY = 1000;				//Y方向ｱﾗｲﾒﾝﾄ 最大値
        private const int MIN_OBY = -1000;				//Y方向ｱﾗｲﾒﾝﾄ 最小値
        //復活2009/08/24(KSS)hata_L10801対応に伴う変更 ----------<

        private const int MAX_ADJ = 3;				//ｱﾗｲﾒﾝﾄ 最大値
        private const int MIN_ADJ = 2;				//ｱﾗｲﾒﾝﾄ 最小値
        //Private Const MAX_CFS   As Integer = 4      'ﾌｫｰｶｽ最大値				
        //Private Const MIN_CFS   As Integer = 1      'ﾌｫｰｶｽ最小値				

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//        private int mintInputMode;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//
		//	mcomCtrlXray_OnComm イベントで使用する static フィールド
		//
        private static int[] mcomCtrlXray_OnComm_intERR = new int[6];           //intERR(1 To 6)       As Integer
        private static int[] mcomCtrlXray_OnComm_intOldErr = new int[6];        //intOldErr(1 To 6)    As Integer
        private static TimeSpan mcomCtrlXray_OnComm_sngTimCont2;               //追加2010/02/24(KSS)hata
        private static TimeSpan mcomCtrlXray_OnComm_sngTimCont3;               //追加2010/02/24(KSS)hata
        private static TimeSpan mcomCtrlXray_OnComm_sngTimCont4;               //追加2010/02/24(KSS)hata
        private static TimeSpan mcomCtrlXray_OnComm_sngTimCont5;               //追加2010/02/24(KSS)hata

		//
		// mtmrCtrlXray_Tick イベントで使用するスタティックメンバ
		//
        private static bool mtmrCtrlXray_Timer_blnTmrFlg;

        private static DateTime mtmrCtrlXray_Timer_varSendTime;	    // 送信タイムアウト用 'v14.14変更 by 間々田 2008/02/20
        private static DateTime? mtmrCtrlXray_Timer_CommTimeOut;	// タイムアウト用
        //追加2015/04/03hata
        private static DateTime mtmrCtrlXray_LineErrTime;	// 送信タイムアウト用 'v14.14変更 by 間々田 2008/02/20
        private static int LineCount = 0;

		//
		// AutoOff メソッドで使用するスタティックメンバ
		//
        private static int AutoOff_intOldXrayOn;
        private static int AutoOff_intTimeStart;
        private static DateTime AutoOff_varTimXray;

		//
		// Avail メソッドで使用するスタティックメンバ
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
		/// コンストラクタ
		/// </summary>
		public clsCtrlXrayL9191()
		{
			lock(cCtrlm.gLock)
			{
				Class_Initialize();
			}
		}

		/// <summary>
		/// 終了処理
		/// </summary>
		public void Dispose()
		{
            //変更2014/06/09_hata
            //排他処理しない
            //lock(cCtrlm.gLock)
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
            //追加2009/10/08(KSS)hata_L10801対応
            modXray.udtXrayProperty udtXrayPIni = new modXray.udtXrayProperty();
            modXray.udtXrayMethodValue udtXrayMIni = new modXray.udtXrayMethodValue();

            //Debug.Print("Initialize in");

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
                
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版                
                
                if (modData.gintXrayClsCnt == 0)
				{
                    //----- 通信の初期設定 -----
					mcomCtrlXray = new SerialPort();
                    //mcomCtrlXray.PortName = string.Format("COM{0}", COMM_PORT);
                    //Rev23.10 COMポートを指定できるように改造 by長野 2015/09/29
                    mcomCtrlXray.PortName = string.Format("COM{0}", modData.portNo);
                    mcomCtrlXray.BaudRate = 115200;
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
                                   
                    //追加2009/10/08(KSS)hata_L10801対応 ---------->
                    modXray.gintOBJ_Set = 0;
                    modXray.gintOBX_Set = 0;
                    modXray.gintOBY_Set = 0;
                    modXray.gudtXrayP = udtXrayPIni;
                    modXray.gudtXrayMTmp = udtXrayMIni;
                    modXray.gudtXrayM = udtXrayMIni;
                    //追加2009/10/08(KSS)hata_L10801対応 ----------<

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
                    mtmrCtrlXray_Timer_blnTmrFlg = false;

                    mtmrCtrlXray.Start();

					modXray.gblnXrayOver = true;

                    //見直し書き方を変更_2014/10/07hata
                    //ModuleCTRLM.ipX_type = 2;	    //X線ﾀｲﾌﾟ
                    ModuleCTRLM.ipX_type = modIniFiles.XRAY_TYPE_NO_160KV;

                    mintSendPointer = P_CTRL;
				}
                modData.gintXrayClsCnt++;
            }
            catch
            {
                //----- エラー処理 -----
                //    gintStatusNoValue = STS_R_XRAY_INITIALIZ
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
                modData.gintXrayClsCnt--;

                if (modData.gintXrayClsCnt == 0)
                {
                    ////----- タイマ終了 -----
                    //if (mtmrCtrlXray != null)
                    //{
                    //    mtmrCtrlXray.Stop();
                    //    mtmrCtrlXray = null;
                    //}

                    ////'2004-09-09 Shibui
                    ////  Call CommOutput(CTRL_XRAY_OFF)  'X線OFF

                    ////----- 通信終了 -----
                    //mcomCtrlXray.Close();
                    //mcomCtrlXray = null;

                    //追加2014/06/09_hata
                    CommEndFlg = true;
                    CtrlXrayFlg = false;

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
                //    gintStatusNoValue = STS_R_XRAY_INITIALIZ 
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
        /// <概略>
        /// OnComm ｲﾍﾞﾝﾄを使用して、通信ｲﾍﾞﾝﾄおよびｴﾗｰをﾄﾗｯﾌﾟします。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mcomCtrlXray_OnComm(object sender, EventArgs e)
        {

#region //デバッグ用（実際には実行されない）追加ここから by 間々田 2007/05/08
        #if DebugOn
            Const fileName As String = "c:\ct\command\sta_over.txt"
        #endif
        //デバッグ用（実際には実行されない）追加ここまで by 間々田 2007/05/08
#endregion

            string strData = null;
            string strCommand = null;
            int intBuf = 0;

            TimeSpan sngTime = TimeSpan.Zero;   //追加2010/02/24(KSS)hata            
            float sngDelayTime = 0;				//追加2010/02/24(KSS)hata        

			int iVal = 0;
			float fVal = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*                        
            int intCnt = 0;
            float sngData = 0;

            int fileno = 0;         //v19.40 追加 by長野 2013/10/23
*/
#endregion

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
                    
                    if (string.IsNullOrEmpty(mstrCommFlg)) return;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
					'CommEvent ﾌﾟﾛﾊﾟﾃｨの値に従って処理を行います。
					Select Case mcomCtrlXray.CommEvent
				                
					' ｲﾍﾞﾝﾄ ﾒｯｾｰｼﾞ
					Case comEvReceive
				        
						'データ受信
						mstrRecvBuf = mstrRecvBuf & mcomCtrlXray.Input
				        
						gintOldErrXrayComm = CTRL_OFF    '通信異常フラグクリア
				        
						'キャリッジリターン削除
						If Right$(mstrRecvBuf, Len(vbLf)) = vbLf Then
				        
							intBuf = InStr(mstrRecvBuf, vbLf)
							If intBuf < Len(mstrRecvBuf) Then
								mstrRecvBuf = Mid(mstrRecvBuf, intBuf + 1)
							End If
							If Len(mstrRecvBuf) > Len(vbCr) + Len(vbLf) Then
								mstrRecvBuf = Left$(mstrRecvBuf, Len(mstrRecvBuf) - Len(vbCr) - Len(vbLf))
							End If
				            
							If mstrRecvBuf = CTRL_HIVOLT _
							Or mstrRecvBuf = CTRL_CURRENT _
							Or mstrRecvBuf = CTRL_XRAY_ON _
							Or mstrRecvBuf = CTRL_XRAY_OFF _
							Or mstrRecvBuf = CTRL_MODE_FOCUS _
							Or mstrRecvBuf = CTRL_RST _
							Or mstrRecvBuf = CTRL_WARMUP _
							Or mstrRecvBuf = CTRL_OBJ _
							Or mstrRecvBuf = CTRL_SAV _
							Or mstrRecvBuf = CTRL_OST _
							Or mstrRecvBuf = CTRL_OBX _
							Or mstrRecvBuf = CTRL_OBY _
							Or mstrRecvBuf = CTRL_ADJ _
							Or mstrRecvBuf = CTRL_ADA _
							Or mstrRecvBuf = CTRL_STP Then
								GoTo ExitHandler
							End If
				            
							If mstrRecvBuf = mstrCommFlg Then
								'ｺﾏﾝﾄﾞをそのまま返してくるので無視
								mstrRecvBuf = ""

								Exit Sub
							End If
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
#endregion
                
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
					if (mstrRecvBuf.EndsWith("\n") == true)
					{
						intBuf = mstrRecvBuf.IndexOf("\n");
						if (intBuf < mstrRecvBuf.Length - 1)
						{
							mstrRecvBuf = mstrRecvBuf.Substring(intBuf + 1);
						}
						if (mstrRecvBuf.Length > "\r\n".Length)
						{
							mstrRecvBuf = mstrRecvBuf.TrimEnd(new char[]{'\r', '\n'});
						}

						if (mstrRecvBuf == CTRL_HIVOLT
						 || mstrRecvBuf == CTRL_CURRENT 
						 || mstrRecvBuf == CTRL_XRAY_ON 
						 || mstrRecvBuf == CTRL_XRAY_OFF 
						 || mstrRecvBuf == CTRL_MODE_FOCUS 
						 || mstrRecvBuf == CTRL_RST 
						 || mstrRecvBuf == CTRL_WARMUP
						 || mstrRecvBuf == CTRL_OBJ
						 || mstrRecvBuf == CTRL_SAV
						 || mstrRecvBuf == CTRL_OST
						 || mstrRecvBuf == CTRL_OBX
						 || mstrRecvBuf == CTRL_OBY
						 || mstrRecvBuf == CTRL_ADJ
						 || mstrRecvBuf == CTRL_ADA
						 || mstrRecvBuf == CTRL_STP)
						{
							throw new Exception();
						}

						if (mstrRecvBuf == mstrCommFlg)
						{
							//ｺﾏﾝﾄﾞをそのまま返してくるので無視
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
						//----- コントロールコマンド ------------------------------
						//コントロールコマンドの返信は、コマンド文字列をそのまま返すだけなので、返信データに対する処理は行わない。
						case CTRL_HIVOLT:
							// 管電圧設定[V]    (0～160)

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

						case CTRL_XRAY_ON:      // Ｘ線出射開始	
							break;
						case CTRL_XRAY_OFF:     // Ｘ線出射停止
							break;
						case CTRL_MODE_FOCUS:   // ﾌｫｰｶｽﾓｰﾄﾞｾｯﾄ  (1～4)
							break;
						//    Case CTRL_AUTO_STOP
						case CTRL_RST:          // 過負荷保護解除
							break;
						case CTRL_WARMUP:       // ウォーミングアップ開始
							break;
						case CTRL_STX:          //----- X線照射確認 -----
							break;
						case CTRL_SHV:          //----- 出力電圧確認 -----
							break;
						case CTRL_SCU:          //----- 出力管電流確認 -----
							break;
						case CTRL_SPV:          //----- 管電圧プリセット確認 -----
							break;
						case CTRL_SPC:          //----- 管電流プリセット確認 -----
							break;
						case CTRL_SOV:          //----- 過負荷保護機能確認 -----
							break;
						case CTRL_SIN:          //----- インタロック確認 -----
							break;
						case CTRL_SWR:          //----- ウォーミングアップ状態確認 -----
							break;
						case CTRL_STI:          //----- 状態確認 -----
							break;
						case CTRL_SXT:
							//----- X線照射時間確認 -----
							//2004-09-29 Shibui
							//        If IsNumeric(mstrRecvBuf) = True Then
							//            gudtXrayP.lngSXT = Val(mstrRecvBuf)
							//        End If				

							if (int.TryParse(mstrRecvBuf.Substring(0, 9), out iVal) == true)
							{
								modXray.gudtXrayP.lngSXT = iVal;
							}
							break;

						//9191にはない
						//    Case CTRL_SWS
						//
						//        //----- ウォーミングアップステップ確認 -----
						//        strCommand = Mid$(mstrRecvBuf, 1, 6)
						//        If strCommand = STAT_WARMUP Then
						//            strData = Mid$(mstrRecvBuf, 8, 1) & Mid$(mstrRecvBuf, 10, 1)
						//        End If
						//

						case CTRL_SWE:
							//----- ウォーミングアップ終了確認 -----
							if (mstrRecvBuf == STAT_WARMUP_END)
							{
								ModuleCTRLM.ipWarmup = 2;    //完
							}
							break;

						case CTRL_SFC:          //----- 焦点モード確認 -----
							break;

						case CTRL_STM:
							//----- X線源通電時間確認 -----
							//2004-09-29 Shibui
							//        If IsNumeric(mstrRecvBuf) = True Then
							//            gudtXrayP.lngSTM = CLng(mstrRecvBuf)
							//        End If
							if (int.TryParse(mstrRecvBuf.Substring(0, 9), out iVal) == true)
							{
								modXray.gudtXrayP.lngSTM = iVal;
							}
							break;

						case CTRL_SAT:
							//'---- 自動X線停止時間 -----
							//'Serial wait time 0 seconds
							if (mstrRecvBuf.IndexOf(" seconds") != -1)
							{
								if (int.TryParse(mstrRecvBuf.Substring(17, mstrRecvBuf.IndexOf(" seconds") - 17), out iVal) == true)
								{
									modXray.gudtXrayP.intSAT = iVal;
								}
							}
							// If IsNumeric(mstrRecvBuf) = True Then
							//     gudtXrayP.intSAT = CInt(mstrRecvBuf)
							// End If
							break;

						case CTRL_TYP:
							break;

						case CTRL_SER:

							//----- 制御基板異常確認 -----
							strCommand = mstrRecvBuf.Substring(0, 3);
							if (strCommand == STAT_SER)
							{
								strData = mstrRecvBuf.Substring(4, 1);

								if (int.TryParse(strData, out iVal) == true)
								{
									modXray.gudtXrayP.intSER = iVal;
								}
								else
								{
									if (strData == "F") modXray.gudtXrayP.intSER = 5;
								}
							}

							//変更2010/02/24（KSS)hata １回しかError表示されないのため ----->
							//Select Case gudtXrayP.intSER
							//Case 2: ipErrsts = ERR_XRAY_ERR2    'ｲﾝﾀｰﾛｯｸ2用ﾘﾚｰ故障
							//Case 3: ipErrsts = ERR_XRAY_ERR3    '高圧ﾊﾟﾜｰﾘﾚｰ駆動用ﾄﾗﾝｼﾞｽﾀ故障
							//Case 4: ipErrsts = ERR_XRAY_ERR4    '高圧ﾊﾟﾜｰﾘﾚｰ故障
							//Case 5: ipErrsts = ERR_XRAY_ERR5    'ﾌｨﾗﾒﾝﾄ断線
							//End Select

							switch (modXray.gudtXrayP.intSER)
							{
								case 2:         //ｲﾝﾀｰﾛｯｸ2用ﾘﾚｰ故障
									mcomCtrlXray_OnComm_intERR[1] = modData.CTRL_ON;
									if (mcomCtrlXray_OnComm_sngTimCont2 == TimeSpan.Zero)
									{
										mcomCtrlXray_OnComm_sngTimCont2 = DateTime.Now.TimeOfDay;
									}
									break;
								case 3:         //高圧ﾊﾟﾜｰﾘﾚｰ駆動用ﾄﾗﾝｼﾞｽﾀ故障
									mcomCtrlXray_OnComm_intERR[2] = modData.CTRL_ON;
									if (mcomCtrlXray_OnComm_sngTimCont3 == TimeSpan.Zero)
									{
										mcomCtrlXray_OnComm_sngTimCont3 = DateTime.Now.TimeOfDay;
									}
									break;
								case 4:         //高圧ﾊﾟﾜｰﾘﾚｰ故障
									mcomCtrlXray_OnComm_intERR[3] = modData.CTRL_ON;
									if (mcomCtrlXray_OnComm_sngTimCont4 == TimeSpan.Zero)
									{
										mcomCtrlXray_OnComm_sngTimCont4 = DateTime.Now.TimeOfDay;
									}
									break;
								case 5:         //ﾌｨﾗﾒﾝﾄ断線
									mcomCtrlXray_OnComm_intERR[4] = modData.CTRL_ON;
									if (mcomCtrlXray_OnComm_sngTimCont5 == TimeSpan.Zero)
									{
										mcomCtrlXray_OnComm_sngTimCont5 = DateTime.Now.TimeOfDay;
									}
									break;
								default:
									mcomCtrlXray_OnComm_intERR[1] = modData.CTRL_OFF;
									mcomCtrlXray_OnComm_intERR[2] = modData.CTRL_OFF;
									mcomCtrlXray_OnComm_intERR[3] = modData.CTRL_OFF;
									mcomCtrlXray_OnComm_intERR[4] = modData.CTRL_OFF;
									mcomCtrlXray_OnComm_sngTimCont2 = TimeSpan.Zero;
									mcomCtrlXray_OnComm_sngTimCont3 = TimeSpan.Zero;
									mcomCtrlXray_OnComm_sngTimCont4 = TimeSpan.Zero;
									mcomCtrlXray_OnComm_sngTimCont5 = TimeSpan.Zero;
									break;
							}

							sngDelayTime = 20;      //20秒毎に表示
							sngTime = DateTime.Now.TimeOfDay;
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
							//変更2010/02/24（KSS)hata １回しかError表示されないのため -----<
							break;

						case CTRL_SVI:
							//----- 設定管電圧電流値確認 -----
							//管電圧､管電流の設定値をそれぞれ3桁ずつで返します。
							modXray.gstrXrayDisp[modXray.DSP_SVI] = mstrRecvBuf;    //表示用
							strCommand = mstrRecvBuf.Substring(0, 3);
							strData = mstrRecvBuf.Substring(4);
							if (strCommand == STAT_SVI)
							{
								if (strCommand == STAT_SVI)
								{
									//2004-09-29 Shibui 数値チェックを追加
									if (float.TryParse(strData.Substring(0, 3), out fVal) == true)
									{
										ModuleCTRLM.ipcndVolt = fVal;	//設定管電圧
									}
									if (float.TryParse(strData.Substring(3, 3), out fVal) == true)
									{
										ModuleCTRLM.ipcndAmp = fVal;	//設定管電流
									}
									//2004-03-23 Shibui
									//          ifX_Volt = ipcndVolt
									//          ifX_Amp = ipcndAmp
									//          gintTmpVol = ifX_Volt
									//          gintTmpAmp = ifX_Amp
								}
								//@_@ ⇒ この処理必要かチェック!!　----->
								//2004-03-23 Shibui
								if (modXray.gblnXrayValue == true)
								{
									modXray.gintXrayValueDisp = 1;
									modXray.gblnXrayValue = false;
								}
								//@_@ ⇒ この処理必要かチェック!!　-----<
							}
							break;

						case CTRL_OBJ:  // ﾌｫｰｶｽ値設定（0～15000）
							break;
						case CTRL_SAV:  // ﾌｫｰｶｽ調整値保存
							break;
						case CTRL_OST:  // ﾌｫｰｶｽ値自動取得
							break;
						case CTRL_OBX:  // X方向ｱﾗｲﾒﾝﾄ設定
							break;
						case CTRL_OBY:  // Y方向ｱﾗｲﾒﾝﾄ設定
							break;
						case CTRL_ADJ:  // ﾋﾞｰﾑｱﾗｲﾒﾝﾄ
							break;
						case CTRL_ADA:  // 一括ﾋﾞｰﾑｱﾗｲﾒﾝﾄ
							break;
						case CTRL_STP:  // ﾋﾞｰﾑｱﾗｲﾒﾝﾄ中断
							break;
						//    Case CTRL_CHT ' ﾌｨﾗﾒﾝﾄｵﾝ時間ﾘｾｯﾄ
						//    Case CTRL_SRO ' ｵｰﾄﾘﾌﾞｰﾄ時間設定 0/5～60
						//    Case CTRL_RMT ' ﾘﾓｰﾄ

						case CTRL_STG:
							//2004-09-29 Shibui 数値チェックを追加
							if (float.TryParse(mstrRecvBuf.Substring(5), out fVal) == true)
							{
								modXray.gudtXrayP.sngTargetInfSTG = fVal;    // ﾀｰｹﾞｯﾄ電流確認
							}
							break;

						case CTRL_STA:  // 一括状態確認
							// STI  :状態
							// SIN  :ｲﾝﾀﾛｯｸ確認
							// SFC  :焦点モード確認
							// SVC  :真空ﾚﾍﾞﾙ確認
							// SAD  :ﾋﾞｰﾑｱﾗｲﾒﾝﾄ状態
							//      :一括ﾋﾞｰﾑｱﾗｲﾒﾝﾄ状態
							//      :ターゲット電流制限値到達
							//      :制御基板異常状態

							strCommand = mstrRecvBuf.Substring(0, 3);
							if (strCommand != CTRL_STA)
							{
								//STAコマンドの返信ではない
								throw new Exception();
							}

							NumberStyles style = NumberStyles.Integer;

							//ﾃﾞｰﾀ取得
							strData = mstrRecvBuf.Substring(4);
							if (strData.StartsWith("0x") == true)
							{
								style = NumberStyles.HexNumber;
								strData = strData.Substring(2);
							}

							//2004-09-29 Shibui 数値チェックを追加
							if (int.TryParse(strData, style, null, out iVal) == false)
							{
								throw new Exception();
							}

							//Debug.Print strData

							#region             //デバッグ用（実際には実行されない）追加ここから by 間々田 2007/05/08
	#if DebugOn
						Dim temp As Long
						If Dir(fileName) <> "" Then
							temp = val(strData) And &HFFFF8
							strData = "&H" & Hex$(temp Or STA_OVER)
						End If                        
	#endif
							//デバッグ用（実際には実行されない）追加ここまで by 間々田 2007/05/08
							#endregion

							// 状態確認
							switch (iVal & modXray.STC_STI_MASK)
							{
								case modXray.STA_WARMUP_YET:
									modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
									ModuleCTRLM.ipWarmup = 0;				//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ待機中
									ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線OFF中
									ModuleCTRLM.ipXStatus = 5;
									ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
									break;
								case modXray.STA_WARMUP:
									modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
									ModuleCTRLM.ipWarmup = 1;				//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ中
									ModuleCTRLM.ipX_On = modData.CTRL_ON;	//X線ON中
									ModuleCTRLM.ipXStatus = 3;
									ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
									break;
								case modXray.STA_STANDBY:
									modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
									ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線照射待機中
									ModuleCTRLM.ipWarmup = 2;				//ｳｫｰﾐﾝｸﾞｱｯﾌﾟ完了
									ModuleCTRLM.ipXStatus = 6;
									ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
									break;
								case modXray.STA_XON:
									modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
									ModuleCTRLM.ipX_On = modData.CTRL_ON;	//X線ON中
									ModuleCTRLM.ipWarmup = 2;
									ModuleCTRLM.ipXStatus = 4;
									ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
									break;
								case modXray.STA_OVER:
									modXray.gblnXrayOver = false;			//荷保護機能動作中ﾌﾗｸﾞ
									//2004-09-27 Shibui
									//ipXStatus = 0
									ModuleCTRLM.ipXStatus = 7;
									ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
									mintSovOver = modData.CTRL_ON;			//解除指令                      2007/05/08追加 by 間々田 過負荷時Ｘ線オフしない現象への対策
									ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線が照射できない状態         2007/05/08追加 by 間々田 過負荷時Ｘ線オフしない現象への対策
									break;
								case modXray.STA_NOT_READY:
									modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
									ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//X線が照射できない状態
									ModuleCTRLM.ipXStatus = 2;
									ModuleCTRLM.ipXPermitWarmup = 1;		//ﾌﾟﾘﾋｰﾄ
									break;
								case modXray.STA_PREHEAT:
									modXray.gblnXrayOver = true;			//荷保護機能動作中ﾌﾗｸﾞ
									ModuleCTRLM.ipX_On = modData.CTRL_OFF;	//ﾌﾟﾘﾋｰﾄ中
									ModuleCTRLM.ipXStatus = 1;
									ModuleCTRLM.ipXPermitWarmup = 0;
									break;
								//2004-09-27 Shibui
								default:
									ModuleCTRLM.ipXStatus = 0;
									break;
							}

							//2004-09-21 Shibui
							if (ModuleCTRLM.ipWarmup == 2)		//ウォームアップ完了
							{
								ModuleCTRLM.ipWarmup_Mode = -1;	//不要
							}
							else
							{
								ModuleCTRLM.ipWarmup_Mode = 1;	//必要
							}

							//ｲﾝﾀﾛｯｸ確認
							switch (iVal & modXray.STA_SIN_MASK)
							{
								case modXray.STA_INTERLOCK_ON:
									ModuleCTRLM.ipInterlock = 1;
									break;
								case modXray.STA_INTERLOCK_OFF:
									ModuleCTRLM.ipInterlock = 0;
									break;
							}

							//焦点モード確認
							switch (iVal & modXray.STA_SFC_MASK)
							{
								case modXray.STA_F1:
									ModuleCTRLM.ipFocussize = 0;
									break;
								case modXray.STA_F2:
									ModuleCTRLM.ipFocussize = 1;
									break;
								case modXray.STA_F3:
									ModuleCTRLM.ipFocussize = 2;
									break;
								case modXray.STA_F4:
									ModuleCTRLM.ipFocussize = 3;
									break;
							}

							//真空ﾚﾍﾞﾙ確認
							switch (iVal & modXray.STA_SVC_MASK)
							{
								case modXray.STA_LO:
									modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_LO;
									break;
								case modXray.STA_V1:
									modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V1;
									break;
								case modXray.STA_V2:
									modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V2;
									break;
								case modXray.STA_V3:
									modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V3;
									break;
								case modXray.STA_V4:
									modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V4;
									break;
								case modXray.STA_V5:
									modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V5;
									break;
								case modXray.STA_NG:
									modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_NG;
									break;
							}

							//ｱｼﾞｬｽﾄ状態
							switch (iVal & modXray.STA_ADJ_MASK)
							{
								case modXray.STA_ADJ_OFF:
									modXray.gudtXrayP.intSAD = modXray.RET_SAD_OFF;
									break;
								case modXray.STA_ADJ_1:
									modXray.gudtXrayP.intSAD = modXray.RET_SAD_1;
									break;
								case modXray.STA_ADJ_2:
									modXray.gudtXrayP.intSAD = modXray.RET_SAD_2;
									break;
								case modXray.STA_ADJ_3:
									modXray.gudtXrayP.intSAD = modXray.RET_SAD_3;
									break;
							}

							//一括ｱｼﾞｬｽﾄ状態
							switch (iVal & modXray.STA_ADA_MASK)
							{
								case modXray.STA_ADA_OFF:
									break;
								case modXray.STA_ADA_ON:
									modXray.gudtXrayP.intSAD = modXray.RET_SAD_ALL;
									break;
							}

							//ターゲット電流制限値到達
							switch (iVal & modXray.STA_STG_MASK)
							{
								case modXray.STA_STG_OFF:
									modXray.gudtXrayP.intTargetLimit = modData.CTRL_OFF;
									break;
								case modXray.STA_STG_ON:
									modXray.gudtXrayP.intTargetLimit = modData.CTRL_ON;
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

							break;

						case CTRL_STL:  //動作値一括確認

							float fVal2 = 0;
							float fVal3 = 0;

							if (float.TryParse(mstrRecvBuf.Substring(4, 3), out fVal) == true
								&& float.TryParse(mstrRecvBuf.Substring(7, 3), out fVal2) == true
								&& float.TryParse(mstrRecvBuf.Substring(10, 3), out fVal3) == true)
							{
								ModuleCTRLM.ipX_Volt = fVal;
								ModuleCTRLM.ipX_Amp = fVal2;
								modXray.gudtXrayP.sngTargetInfSTG = fVal3 * 0.1f;

								//X線OFFで管電流=1の時に管電流=0とする。
								if (ModuleCTRLM.ipX_On == modXray.RET_XRAY_OFF && ModuleCTRLM.ipX_Amp == 1)
								{
									ModuleCTRLM.ipX_Amp = 0;
								}
							}
							break;

						case CTRL_SOB:
							//2004-09-27 Shibui
							if ("OBJ" == mstrRecvBuf.Substring(0, 3))
							{
								if (float.TryParse(mstrRecvBuf.Substring(4), out fVal) == true)
								{
									modXray.gudtXrayP.sngSOB = fVal;		// ﾌｫｰｶｽ値確認                                
								}
							}
							break;

						case CTRL_SBX:
							//2004-09-29 Shibui
							//        If IsNumeric(Mid(mstrRecvBuf, 5)) = True Then
							//            gudtXrayP.intSBX = Val(Mid(mstrRecvBuf, 5)) ' X方向ｱﾗｲﾒﾝﾄ確認
							//        End If                        
							if (int.TryParse(mstrRecvBuf.Substring(4, 5), out iVal) == true)
							{
								modXray.gudtXrayP.intSBX = iVal;			// X方向ｱﾗｲﾒﾝﾄ確認                                
							}
							break;

						case CTRL_SBY:
							//2004-09-29 Shibui
							//        If IsNumeric(Mid(mstrRecvBuf, 5)) = True Then
							//            gudtXrayP.intSBY = Val(Mid(mstrRecvBuf, 5)) ' Y方向ｱﾗｲﾒﾝﾄ確認
							//        End If                        
							if (int.TryParse(mstrRecvBuf.Substring(4, 5), out iVal) == true)
							{
								modXray.gudtXrayP.intSBY = iVal;			// Y方向ｱﾗｲﾒﾝﾄ確認                                
							}
							break;

						case CTRL_SAD:
							switch (mstrRecvBuf)
							{
								case STAT_SAD_OFF:
									modXray.gudtXrayP.intSAD = modXray.RET_SAD_OFF;
									break;
								case STAT_SAD_2:
									modXray.gudtXrayP.intSAD = modXray.RET_SAD_1;
									break;
								case STAT_SAD_3:
									modXray.gudtXrayP.intSAD = modXray.RET_SAD_2;
									break;
								case STAT_SAD_1_3:
									modXray.gudtXrayP.intSAD = modXray.RET_SAD_3;
									break;
							}
							break;

						case CTRL_SVV:
							if (float.TryParse(mstrRecvBuf.Substring(3), out fVal) == true)
							{
								modXray.gudtXrayP.sngSVV = fVal;			// 真空計ﾓﾆﾀ                            
							}
							break;

						case CTRL_SVC:  //真空状態確認
							switch (mstrRecvBuf.Substring(5))
							{
								case STAT_SVC_LO:
									modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_LO;
									break;
								case STAT_SVC_NG:
									modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_NG;
									break;
								case STAT_SVC_V1:
									modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V1;
									break;
								case STAT_SVC_V2:
									modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V2;
									break;
								case STAT_SVC_V3:
									modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V3;
									break;
								case STAT_SVC_V4:
									modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V4;
									break;
								case STAT_SVC_V5:
									modXray.gudtXrayP.strVacuumSVC = modXray.RET_SVC_V5;
									break;
							}
							break;

						case CTRL_SHM:
							if (mstrRecvBuf.IndexOf("mA") != -1)
							{
								if (int.TryParse(mstrRecvBuf.Substring(6, mstrRecvBuf.IndexOf("mA") - 6), out iVal) == true)
								{
									modXray.gudtXrayP.intSHM = iVal;
								}
							}
							break;

						case CTRL_SHS:
							if (mstrRecvBuf.IndexOf("V/L:") != -1)
							{
								if (ModuleCTRLM.ipX_On == modData.CTRL_ON)
								{
									if (float.TryParse(mstrRecvBuf.Substring(2, mstrRecvBuf.IndexOf("V/L:") - 2), out fVal) == true)
									{
										modXray.gudtXrayP.sngSHS = fVal;
									}
								}
								else
								{
									if (float.TryParse(mstrRecvBuf.Substring(Convert.ToInt32(mstrRecvBuf.IndexOf("V/L:") + 3), Convert.ToInt32(mstrRecvBuf.IndexOf("V", mstrRecvBuf.IndexOf("V/L:") + 3) - (mstrRecvBuf.IndexOf("V/L:") + 3))), out fVal) == true)
									{
										modXray.gudtXrayP.sngSHS = fVal;
									}
								}
							}
							break;

						case CTRL_SHT:
							//2004-09-29 Shibui
							//        If IsNumeric(mstrRecvBuf) = True Then
							//            gudtXrayP.intSHT = Val(mstrRecvBuf)
							//        End If
							if (int.TryParse(mstrRecvBuf.Substring(0, 4), out iVal) == true)
							{
								//変更2009/08/25hata_L10801対応 ---------->
								//            gudtXrayP.intSHT = val(Mid(mstrRecvBuf, 1, 4))
								modXray.gudtXrayP.lngSHT = iVal;
								//変更2009/08/25hata_L10801対応 ----------<
							}
							break;

						case CTRL_CHECK_COMM:
							if (mstrRecvBuf == STAT_ERROR) mblnCheckComm = true;
							break;

						case STAT_ERROR:
							//コマンドやパラメータに誤りがあったとき、または、コマンドを受け付けない状態の時に返信される
							//処理は行わない。
							break;
					}
				}
				catch
				{
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

            }		//lock(cCtrlm.gLock)
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

                //Debug.Print "送信[" & Buf & "]"

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
		///  ステータスモニタ用タイマ
		/// </summary>
		private void mtmrCtrlXray_Timer()
        {

            //変更2014/06/09_hata
            //while(!mtmrCtrlXray.Sleep(Interval))
            while (CtrlXrayFlg)
            {
				//Debug.Print "tmrCtrlXray_Timer"
				int intRet = 0;
				bool blnRet = false;
				string strData = null;
				string strCommand = null;   //送信文字列

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				string strBuff = null;

				'2004-07-08 Shibui
				'    Static mintSendPointer       As Integer
					'Static lngSendTime         As Long     '送信タイムアウト用
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
						if (mtmrCtrlXray_Timer_blnTmrFlg == true)
						{
							continue;
						}

						mtmrCtrlXray_Timer_blnTmrFlg = true;

						//ｴﾗｰﾘｾｯﾄ
						if (ModuleCTRLM.ifErrrst == 1)
						{
                            //追加2015/02/06_hata
                            if (ModuleCTRLM.ipErrsts == modData.ERR_XRAY_COMM)
                            {
                                modXray.gintOldErrXrayComm = modData.CTRL_OFF;
                                mtmrCtrlXray_Timer_CommTimeOut = null;
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
                        //追加2015/02/07_hata
                        //変更2015/03/13hata
                        //if (CommLineError | CommDataError | (!mcomCtrlXray.CtsHolding) | (mcomCtrlXray.DtrEnable & !mcomCtrlXray.DsrHolding))
                        if (CommLineError | CommDataError | (mcomCtrlXray.DtrEnable & !mcomCtrlXray.DsrHolding))
                        {
                            //----- CommLineタイムアウト -----
                            //ここからタイムアウトのカウンタ(s)を計測
                            if (mtmrCtrlXray_Timer_CommTimeOut == null)
                            {
                                mtmrCtrlXray_Timer_CommTimeOut = DateTime.Now;
                            }
                            if ((mtmrCtrlXray_Timer_CommTimeOut != null) && (DateTime.Now - mtmrCtrlXray_Timer_CommTimeOut.Value).TotalSeconds >= COMM_LINE_TIMEOUT)
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
                            //Debug.WriteLine("CommLineTime=" + (DateTime.Now - mtmrCtrlXray_Timer_CommTimeOut.Value).TotalSeconds.ToString());
                            mtmrCtrlXray_Timer_blnTmrFlg = false;
                            continue;
                        }

                        //if (!string.IsNullOrEmpty(mstrCommFlg))   //変更2015/02/06_hata
                        else if (!string.IsNullOrEmpty(mstrCommFlg))
						{
							//        '----- タイムアウト -----
							//        'lngReceiveTime = (Timer - lngSendTime) / 60 * 60    '受信完了時間  'v14.14削除 by 間々田 2008/02/20
							//        'If lngReceiveTime >= COMM_TIMEOUT Then                             'v14.14削除 by 間々田 2008/02/20
							//        If DateDiff("s", varSendTime, Now) >= COMM_TIMEOUT Then             'v14.14追加 by 間々田 2008/02/20
							//            'タイムアウト
							//            mstrCommFlg = ""
							//            GoTo ErrorHandler
							//        End If
							//        blnTmrFlg = False
							//        Exit Sub
							//    End If

							//----- タイムアウト -----
							//  If DateDiff("s", varSendTime, Now) >= COMM_TIMEOUT Then             'v14.14追加 by 間々田 2008/02/20
							if ((DateTime.Now - mtmrCtrlXray_Timer_varSendTime).TotalSeconds >= COMM_REPEAT_TIME)		//v14.14追加 by 間々田 2008/02/20
							{
                                TimeOutCommandString = mstrCommFlg; //追加2015/03/02hata

                                mstrCommFlg = "";
								//v19.40 次の送信に行けるように削除 by長野 2013/10/17
								//GoTo ErrorHandler
								//v19.40 ここからタイムアウトのカウンタ(s)を計測
								if (mtmrCtrlXray_Timer_CommTimeOut == null)
								{
									mtmrCtrlXray_Timer_CommTimeOut = mtmrCtrlXray_Timer_varSendTime;
								}

								if ((mtmrCtrlXray_Timer_CommTimeOut != null) && (DateTime.Now - mtmrCtrlXray_Timer_CommTimeOut.Value).TotalSeconds > COMM_TIMEOUT)
								{
									//ここを本当のタイムアウトにする

                                    RecieveSts = false;
                                    //変更2015/02/07hata
                                    //throw new Exception();
                                    CommTimeOutError = true;
                                    goto ErrorHandler;
                                }
							}

							mtmrCtrlXray_Timer_blnTmrFlg = false;
							continue;
							//v19.40 正常に送受信できたらタイムアウト用のカウンタを初期化 by長野 2013/10/17
							//End If
						}
						else
						{
							mtmrCtrlXray_Timer_CommTimeOut = null;
                            CommTimeOutError = false;   //追加2015/02/07hata
                        }

						//2003-11-18 Shibui 起動時に一回通信を行う

						if (mblnCheckComm == false)
						{
							if (string.IsNullOrEmpty(strCommand))
							{
								strCommand = CTRL_CHECK_COMM;
								mstrCommFlg = CTRL_CHECK_COMM;
								blnRet = CommOutput(strCommand);        //コマンド送信
								if (blnRet == false) throw new Exception();
								//lngSendTime = Timer '送信した時刻を格納
								mtmrCtrlXray_Timer_varSendTime = DateTime.Now;   //送信した時刻を格納  'v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
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

						//  Call XrayDisp   'X線情報表示
						AutoOff();				//自動X線OFF
						Avail();				//アベイラブル

						//----- ステータスモニタ -----
						switch (mintSendPointer)
						{
							case P_STX:     //X線照射確認
								mstrCommFlg = CTRL_STX;
								strCommand = mstrCommFlg;
								break;

							case P_SHV:     //出力電圧確認
								mstrCommFlg = CTRL_SHV;
								strCommand = mstrCommFlg;
								break;

							case P_SCU:     //出力電流確認
								mstrCommFlg = CTRL_SCU;
								strCommand = mstrCommFlg;
								break;

							case P_SPV:     //管電圧プリセット確認
								mstrCommFlg = CTRL_SPV;
								strCommand = mstrCommFlg;
								break;

							case P_SPC:     //管電流プリセット確認
								mstrCommFlg = CTRL_SPC;
								strCommand = mstrCommFlg;
								break;

							case P_SOV:     //過負荷保護機能確認
								mstrCommFlg = CTRL_SOV;
								strCommand = mstrCommFlg;
								break;

							case P_SIN:     //インタロック確認
								mstrCommFlg = CTRL_SIN;
								strCommand = mstrCommFlg;
								break;

							case P_SWR:     //ウォーミングアップ状態確認
								mstrCommFlg = CTRL_SWR;
								strCommand = mstrCommFlg;
								break;

							case P_STI:     //状態確認
								mstrCommFlg = CTRL_STI;
								strCommand = mstrCommFlg;
								break;

							case P_SXT:     //X線照射時間確認
								mstrCommFlg = CTRL_SXT;
								strCommand = mstrCommFlg;
								break;

							//    Case P_SWS  'ウォーミングアップステップ確認
							//        mstrCommFlg = CTRL_SWS
							//        strCommand = mstrCommFlg
							//
							case P_SWE:     //ウォーミングアップ終了確認
								mstrCommFlg = CTRL_SWE;
								strCommand = mstrCommFlg;
								break;

							case P_SFC:     //焦点モード確認
								mstrCommFlg = CTRL_SFC;
								strCommand = mstrCommFlg;
								break;

							case P_STM:     //X線源通電時間確認
								mstrCommFlg = CTRL_STM;
								strCommand = mstrCommFlg;
								break;

							case P_SAT:     //自動X線停止機能確認
								mstrCommFlg = CTRL_SAT;
								strCommand = mstrCommFlg;
								break;

							case P_TYP:     //型名確認                        
								mstrCommFlg = CTRL_TYP;
								strCommand = mstrCommFlg;
								break;

							case P_SER:     //制御基板異常確認                        
								mstrCommFlg = CTRL_SER;
								strCommand = mstrCommFlg;
								break;

							//    Case P_STC '一括状態確認
							//        mstrCommFlg = CTRL_STC
							//        strCommand = mstrCommFlg
							//
							//    Case P_STT '動作管電圧管電流カソード値確認
							//        mstrCommFlg = CTRL_STT
							//        strCommand = mstrCommFlg
							//
							case P_SVI:     //設定電圧値電流値確認                        
								mstrCommFlg = CTRL_SVI;
								strCommand = mstrCommFlg;
								break;

							case P_STG:     // ﾀｰｹﾞｯﾄ電流確認                        
								mstrCommFlg = CTRL_STG;
								strCommand = mstrCommFlg;
								break;

							case P_SOB:     // ﾌｫｰｶｽ値確認                        
								mstrCommFlg = CTRL_SOB;
								strCommand = mstrCommFlg;
								break;

							case P_SBX:     // X軸方向ｱﾗｲﾒﾝﾄ確認                        
								mstrCommFlg = CTRL_SBX;
								strCommand = mstrCommFlg;
								break;

							case P_SBY:     // Y軸方向ｱﾗｲﾒﾝﾄ確認                        
								mstrCommFlg = CTRL_SBY;
								strCommand = mstrCommFlg;
								break;

							case P_SAD:     // ｱﾗｲﾒﾝﾄﾓﾆﾀ                        
								mstrCommFlg = CTRL_SAD;
								strCommand = mstrCommFlg;
								break;

							case P_SVV:     // 真空度ﾓﾆﾀ                       
								mstrCommFlg = CTRL_SVV;
								strCommand = mstrCommFlg;
								break;

							case P_SVC:     // 真空状態確認                        
								mstrCommFlg = CTRL_SVC;
								strCommand = mstrCommFlg;
								break;

// すでに定義済みのためコメントアウト
//		                    case P_STM:     // 電源ON通電時間                        
//				                  mstrCommFlg = CTRL_STM;
//						          strCommand = mstrCommFlg;
//								  break;

							case P_SHM:     // ﾌｨﾗﾒﾝﾄ入力確認                        
								mstrCommFlg = CTRL_SHM;
								strCommand = mstrCommFlg;
								break;

							case P_SHS:     // ﾌｨﾗﾒﾝﾄ設定電圧確認                        
								mstrCommFlg = CTRL_SHS;
								strCommand = mstrCommFlg;
								break;

							case P_SHT:     // ﾌｨﾗﾒﾝﾄｵﾝ時間確認                        
								mstrCommFlg = CTRL_SHT;
								strCommand = mstrCommFlg;
								break;

							case P_STA:     // 状態一括確認                        
								mstrCommFlg = CTRL_STA;
								strCommand = mstrCommFlg;
								break;

							case P_STL:     // 動作値一括確認                        
								mstrCommFlg = CTRL_STL;
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

								//'        If gudtXrayStatus2Value.E_XrayStatusSTI <> RET_WARMUP _
								//And gudtXrayStatus3Value.E_XrayStatusSAD <> RET_SAD_ALL _
								//And gudtXrayStatus3Value.E_XrayStatusSAD <> RET_SAD_1 _
								//And gudtXrayStatus3Value.E_XrayStatusSAD <> RET_SAD_2 _
								//And gudtXrayStatus3Value.E_XrayStatusSAD <> RET_SAD_3 Then
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
									else if ((ModuleCTRLM.ifXrayonoff_Set == 1 && ModuleCTRLM.ipWarmup == 0) || modXray.gudtXrayM.intWarmUp == 1)
									{
										//----- ウォームアップ開始コマンド送信 -----
										strCommand = CTRL_WARMUP;
										mstrCommFlg = CTRL_WARMUP;
										ModuleCTRLM.ifXrayonoff_Set = 0;
										modXray.gudtXrayM.intWarmUp = 0;

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
									//2004-09-27 Shibui
									//  If ifX_Volt <> ipcndVolt And gintTmpVol <> ifX_Volt Then
									if (modXray.gintTmpVol != ModuleCTRLM.ifX_Volt)
									{
										//設定管電圧値に変化がある時（X線発生装置から取得した設定管電圧値と比較）
										strData = ModuleCTRLM.ifX_Volt.ToString("000");		// 設定値取得(V)
										strCommand = CTRL_HIVOLT + strData;					// 電圧値の設定コマンド生成
										mstrCommFlg = CTRL_HIVOLT;

										modXray.gintTmpVol = (int)ModuleCTRLM.ifX_Volt;
										modXray.gblnXrayValue = true;

										goto SensCommand;
									}

									//----- 管電流値の送信 -----
									//2004-09-27 Shibui
									//  If ifX_Amp <> ipcndAmp Then
									if (ModuleCTRLM.ifX_Amp != modXray.gintTmpAmp)
									{
										//設定管電流値に変化がある時（X線発生装置から取得した設定管電流値と比較）
										strData = ModuleCTRLM.ifX_Amp.ToString("000");		// 設定値取得(V)
										strCommand = CTRL_CURRENT + strData;				// 電流値の設定コマンド生成
										mstrCommFlg = CTRL_CURRENT;

										modXray.gintTmpAmp = (int)ModuleCTRLM.ifX_Amp;
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
											strCommand = CTRL_MODE_FOCUS + Convert.ToString(ModuleCTRLM.ifFocussize + 1);
											mstrCommFlg = CTRL_MODE_FOCUS;
										}

										goto SensCommand;
									}

									//----- ﾌｫｰｶｽ値設定 -----
									if (modXray.gintOBJ_Set == 1)
									{
										//If (gudtXrayM.sngOBJ <> gudtXrayMTmp.sngOBJ) Then
										if (modXray.gudtXrayM.sngOBJ > MAX_OBJ || modXray.gudtXrayM.sngOBJ < MIN_OBJ)
										{
											//範囲外
										}
										else
										{
											//ｺﾏﾝﾄﾞ文字列作成
											//2004-09-27 Shibui
											//  strCommand = CTRL_OBJ & CStr(gudtXrayM.sngOBJ)
											strCommand = CTRL_OBJ + Convert.ToString(modXray.gudtXrayM.sngOBJ * 10);

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

									//----- ﾌｫｰｶｽ値自動取得 -----
									if (modXray.gudtXrayM.intOST == modData.CTRL_ON)
									{
										//ｺﾏﾝﾄﾞ文字列作成
										modXray.gudtXrayM.intOST = modData.CTRL_OFF;
										strCommand = CTRL_OST;
										mstrCommFlg = CTRL_OST;

										goto SensCommand;
									}

									//----- X方向ｱﾗｲﾒﾝﾄ設定 -----
									if (modXray.gintOBX_Set == 1)
									{
										//If gudtXrayM.intOBX <> gudtXrayMTmp.intOBX Then
										if (modXray.gudtXrayM.intOBX > MAX_OBX || modXray.gudtXrayM.intOBX < MIN_OBX)
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
										if (modXray.gudtXrayM.intOBY > MAX_OBY || modXray.gudtXrayM.intOBY < MIN_OBY)
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

									//----- ﾋﾞｰﾑｱﾗｲﾒﾝﾄ -----
									if (0 != modXray.gudtXrayM.intADJ)
									{
										if (modXray.gudtXrayM.intADJ > MAX_ADJ || modXray.gudtXrayM.intADJ < MIN_ADJ) 
										{
											//範囲外
										}
										else 
										{
											//ｺﾏﾝﾄﾞ文字列作成
											strCommand = CTRL_ADJ + Convert.ToString(modXray.gudtXrayM.intADJ);
											modXray.gudtXrayM.intADJ = 0;
											mstrCommFlg = CTRL_ADJ;

											goto SensCommand;
										}
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

									//                '----- ﾌｨﾗﾒﾝﾄON時間ﾘｾｯﾄ -----
									//                If gudtXrayM.intCHT = CTRL_ON Then
									//                    'ｺﾏﾝﾄﾞ文字列作成
									//                    gudtXrayM.intCHT = CTRL_OFF
									//                    strCommand = CTRL_CHT
									//                    mstrCommFlg = CTRL_CHT
									//                    GoTo SensCommand
									//                End If
									//
									//                '----- ｵｰﾄﾘﾌﾞｰﾄ時間設定 -----
									//                If .intSRO = CTRL_ON Then
									//                    'ｺﾏﾝﾄﾞ文字列作成
									//                    .intSRO = CTRL_OFF
									//                    strCommand = CTRL_SRO
									//                    mstrCommFlg = CTRL_SRO
									//                    GoTo SensCommand
									//                End If

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
								}

								//----- 過負荷保護解除 -----            'コメントから復活 by 間々田 2007/05/08 過負荷時Ｘ線オフしない現象への対策
								if (mintSovOver == modData.CTRL_ON)	    //コメントから復活 by 間々田 2007/05/08 過負荷時Ｘ線オフしない現象への対策
								{
									mintSovOver = modData.CTRL_OFF;	    //追加             by 間々田 2007/05/08 過負荷時Ｘ線オフしない現象への対策 この行がないと復帰した際、Ｘ線オンしてもすぐにオフしてしまう
									strCommand = CTRL_RST;			    //コメントから復活 by 間々田 2007/05/08 過負荷時Ｘ線オフしない現象への対策
									mstrCommFlg = CTRL_RST;			    //コメントから復活 by 間々田 2007/05/08 過負荷時Ｘ線オフしない現象への対策
									goto SensCommand;				    //コメントから復活 by 間々田 2007/05/08 過負荷時Ｘ線オフしない現象への対策
								}									    //コメントから復活 by 間々田 2007/05/08 過負荷時Ｘ線オフしない現象への対策
								break;
						}

SensCommand:
                        if (!CtrlXrayFlg) break;    //追加2014/06/09_hata

                        //----- コマンド送信 -----
						if (!string.IsNullOrEmpty(strCommand))
						{
							blnRet = CommOutput(strCommand);		//コマンド送信
						//v19.40 送信失敗しても次の送信を行う by長野 2013/10/17
							SendSts = blnRet;
						//v19.40    'If blnRet = False Then GoTo ErrorHandler
						//			If blnRet = False Then GoTo ErrorHandler
						//			lngSendTime = Timer '送信した時刻を格納
							mtmrCtrlXray_Timer_varSendTime = DateTime.Now;	//送信した時刻を格納  'v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
						}

						//----- ポインタ管理 次に送信するコマンドにポインタ移動 -----
						{
							switch (mintSendPointer)
							{
                                								//次のｺﾏﾝﾄﾞは･･･
								case P_CTRL:    			    //ウォーミングアップステップ確認
									mintSendPointer = P_SWE;                            
									break;
								case P_SWE:			            //型名
									mintSendPointer = P_TYP;                            
									break;
								case P_TYP:			            //設定電圧値電流値確認
									mintSendPointer = P_SVI;                            
									break;
								case P_SVI:			            //ﾌｫｰｶｽ値
									mintSendPointer = P_SOB;                            
									break;
								case P_SOB:			            //X軸方向ｱﾗｲﾒﾝﾄ確認
									mintSendPointer = P_SBX;                            
									break;
								case P_SBX:			            //Y軸方向ｱﾗｲﾒﾝﾄ確認
									mintSendPointer = P_SBY;                            
									break;
								case P_SBY:			            //真空度ﾓﾆﾀ
									mintSendPointer = P_SVV;                            
									break;
								case P_SVV:			            //状態一括確認
									mintSendPointer = P_STA;                            
									break;
								case P_STA:			            //動作値一括確認
									mintSendPointer = P_STL;                            
									break;
								case P_STL:			            //エラー情報
									mintSendPointer = P_SER;                            
									break;
								case P_SER:			            //
									mintSendPointer = P_STM;                            
									break;
                    			//  Case P_STL:     mintSendPointer = P_STM     '電源ON時間確認
								case P_STM:			            //X線照射時間確認
									mintSendPointer = P_SXT;                            
									break;
								case P_SXT:			            //ﾌｨﾗﾒﾝﾄ入力確認
									mintSendPointer = P_SHM;                            
									break;
								case P_SHM:			            //ﾌｨﾗﾒﾝﾄ設定電圧確認
									mintSendPointer = P_SHS;                            
									break;
								case P_SHS:			            //ﾌｨﾗﾒﾝﾄｵﾝ時間確認
									mintSendPointer = P_SHT;                            
									break;
								case P_SHT:			            //自動X線OFF時間
									mintSendPointer = P_SAT;                            
									break;
								case P_SAT:			            //ｺﾝﾄﾛｰﾙｺﾏﾝﾄﾞ
									mintSendPointer = P_CTRL;                            
									break;
								default:
									mintSendPointer = P_CTRL;
									mstrCommFlg = "";
									break;
							}
						}

ExitHandler:
						mtmrCtrlXray_Timer_blnTmrFlg = false;
//						gintTmrCtrl = 0
					}
					catch			//エラー処理
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
//                                sw = new StreamWriter(@"C:\\CTUSER\\XrayCommR.log", true);
//                                sw.WriteLine(string.Format("{0}-{1}-{2}", DateTime.Now, mtmrCtrlXray_Timer_CommTimeOut, mstrCommFlg));
//                                sw.Close();
//                            }

//                            LogWriteRFlg = true;
//                        }

//                        if (LogWriteSFlg == false)
//                        {
//                            if (SendSts == false)
//                            {
//                                sw = new StreamWriter(@"C:\\CTUSER\\XrayCommS.log", true);
//                                sw.WriteLine(string.Format("{0}-{1}-{2}", DateTime.Now, mtmrCtrlXray_Timer_CommTimeOut, mstrCommFlg));
//                                sw.Close();
//                            }

//                            LogWriteSFlg = true;
//                        }

//                        mtmrCtrlXray_Timer_blnTmrFlg = false;

//						gintTmrCtrl = 0
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
                            //sw.WriteLine(string.Format("{0}-{1}-{2}", DateTime.Now, mtmrCtrlXray_Timer_CommTimeOut, mstrCommFlg));
                            sw.WriteLine(string.Format("{0}; {1} - {2}", DateTime.Now, mtmrCtrlXray_Timer_CommTimeOut, TimeOutCommandString));
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
                            //sw.WriteLine(string.Format("{0}-{1}-{2}", DateTime.Now, mtmrCtrlXray_Timer_CommTimeOut, mstrCommFlg));
                            sw.WriteLine(string.Format("{0}; {1} - {2}", DateTime.Now, mtmrCtrlXray_Timer_CommTimeOut, TimeOutCommandString));
                            sw.Close();
                        }

                        LogWriteSFlg = true;
                    }

                    mtmrCtrlXray_Timer_blnTmrFlg = false;
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
            mtmrCtrlXray_Timer_blnTmrFlg = false;//追加2014/06/09_hata
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
			}
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
                
                On Error GoTo ExitHandler
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            try
            {   
                //----- アベイラブル -----
                if (ModuleCTRLM.ipX_On == 1 && ModuleCTRLM.ipWarmup != 1)
                {
                    //X線ON中 AND ウォームアップ中ではない
    
                //        With gudtXrayStatus1Value

                    //X線ON監視
                    if (Avail_intOldXrayOnAvail != ModuleCTRLM.ipX_On)
                    {
                        Avail_intXrayAvailOnRun = modData.CTRL_ON;                                  //X線ONアベイラブル判定スタート
                        Avail_intXrayAvailTime = modData.gudtXraySystemValue.E_XrayAvailTimeOn;     //X線ONアベイラブル時間をセット
                        Avail_intXrayAvailOk = modData.CTRL_OFF;

                        Avail_intOldXrayOnAvail = ModuleCTRLM.ipX_On;

                        Avail_intOldXrayVoltage = (int)ModuleCTRLM.ipcndVolt;   //X線ON時には、管電圧・管電流アベイラブル判定を行わない
                        Avail_intOldXrayCurent = (int)ModuleCTRLM.ipcndAmp;     //X線ON時には、管電圧・管電流アベイラブル判定を行わない
                    }

                    //設定値変更監視
                    if (Avail_intOldXrayVoltage != ModuleCTRLM.ifX_Volt || Avail_intOldXrayCurent != ModuleCTRLM.ifX_Amp)
                    {
                        Avail_intXrayAvailInsideRun = modData.CTRL_ON;                              //設定値変更アベイラブルスタート
                        Avail_intXrayAvailTime = modData.gudtXraySystemValue.E_XrayAvailTimeInside;	//設定値変更アベイラブル時間をセット
                        Avail_intXrayAvailOk = modData.CTRL_OFF;

                        Avail_intOldXrayVoltage = (int)ModuleCTRLM.ifX_Volt;
                        Avail_intOldXrayCurent = (int)ModuleCTRLM.ifX_Amp;
                    }

                    //アベイラブル範囲判定
                    if ((Avail_intXrayAvailOnRun == modData.CTRL_ON || Avail_intXrayAvailInsideRun == modData.CTRL_ON)
                        && (ModuleCTRLM.ipX_Volt <= ModuleCTRLM.ipcndVolt + modData.gudtXraySystemValue.E_XrayAvailkV)
                        && (ModuleCTRLM.ipX_Volt >= ModuleCTRLM.ipcndVolt - modData.gudtXraySystemValue.E_XrayAvailkV)
                        && (ModuleCTRLM.ipX_Amp <= ModuleCTRLM.ipcndAmp + modData.gudtXraySystemValue.E_XrayAvailuA)
                        && (ModuleCTRLM.ipX_Amp >= ModuleCTRLM.ipcndAmp - modData.gudtXraySystemValue.E_XrayAvailuA))
                    {
                        if (Avail_intXrayAvailOk != modData.CTRL_ON)
                        {
                            //sngXrayAvailStartTime = Timer 'アベイラブル範囲内になった時、カウントスタート
                            Avail_varXrayAvailStartTime = DateTime.Now; //v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
                        }
                        Avail_intXrayAvailOk = modData.CTRL_ON;
                    }

                    //アベイラブル時間経過判定
                    //If intXrayAvailOk = CTRL_ON _
                    //And (Timer - sngXrayAvailStartTime) >= intXrayAvailTime Then

                    //v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
                    if (Avail_intXrayAvailOk == modData.CTRL_ON && (DateTime.Now - Avail_varXrayAvailStartTime).TotalSeconds >= Avail_intXrayAvailTime)
                    {
                        ModuleCTRLM.ipXAvail = 1;       //アベイラブルON
                        Avail_intXrayAvailOnRun = modData.CTRL_OFF;			//アベイラブル判定停止
                        Avail_intXrayAvailInsideRun = modData.CTRL_OFF;     //アベイラブル判定停止
                    }
                    else
                    {
                        ModuleCTRLM.ipXAvail = 0;       //アベイラブルOFF
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
            catch
            {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			ExitHandler:
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
            }
        }
    }
}
