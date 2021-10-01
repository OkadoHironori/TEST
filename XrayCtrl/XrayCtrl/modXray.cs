using System;

namespace XrayCtrl
{
	internal static class modXray
	{
		//*******************************************************************************
		//
		//   X線制御に必要な変数を宣言する
		//
		//   Date        Version     Designed/Changed
		//   2003/05/28  2.00        (NSD)Shibui
		//   2004/09/07  2.10        (NSD)Shibui 9191対応
		//
		//*******************************************************************************

		//Public gintXrayErr                  As Integer      'X線装置の異常状況

		//----- X線装置の異常状況変数用 -----
		//Public Const XRAY_OK                As Integer = 0
		//Public Const XRAY_ERR1              As Integer = 1
		//Public Const XRAY_ERR2              As Integer = 2
		//Public Const XRAY_ERR3              As Integer = 3
		//Public Const XRAY_ERR4              As Integer = 4

		//------------------------------------------------------------------------------------------
		//   X線装置用
		//
		//------------------------------------------------------------------------------------------

		//ﾏｽｸ
		//Public Const STA_STI_MASK           As Long = &H7       ' 状態確認
		public const int STA_SIN_MASK = 0x10;			// ｲﾝﾀﾛｯｸ確認
		public const int STA_SFC_MASK = 0x60;			// ﾌｫｰｶｽﾓｰﾄﾞ確認
		public const int STA_SVC_MASK = 0x700;			// 真空ﾚﾍﾞﾙ確認
		public const int STA_ADJ_MASK = 0x3000;			// ｱｼﾞｬｽﾄ状態確認
		public const int STA_ADA_MASK = 0x4000;			// 一括ｱｼﾞｬｽﾄ状態確認
		public const int STA_STG_MASK = 0x8000;			// ﾀｰｹﾞｯﾄ電流制限値到達
		//Public Const STA_ERR_MASK           As Long = &HF40000  ' ｴﾗｰ
		//Public Const STA_ERR2_MASK          As Long = &H40000   ' ｲﾝﾀｰﾛｯｸ2用ﾘﾚｰの異常
		//Public Const STA_ERR3_MASK          As Long = &H100000  ' 高圧ﾊﾟﾜｰﾘﾚｰ駆動用ﾄﾗﾝｼﾞｽﾀの異常
		//Public Const STA_ERR4_MASK          As Long = &H200000  ' 高圧ﾊﾟﾜｰﾘﾚｰの異常
		//Public Const STA_ERR5_MASK          As Long = &H400000  ' ﾌｨﾗﾒﾝﾄ切れｴﾗｰ
		//Public Const STA_ERR6_MASK          As Long = &H800000  ' 真空ｹﾞｰｼﾞｴﾗｰ

		//状態確認
		public const int STA_WARMUP_YET = 0x0;
		public const int STA_WARMUP = 0x1;
		public const int STA_STANDBY = 0x2;
		public const int STA_XON = 0x3;
		public const int STA_OVER = 0x4;
		public const int STA_NOT_READY = 0x5;
		public const int STA_PREHEAT = 0x6;

		//ｲﾝﾀｰﾛｯｸ
		public const int STA_INTERLOCK_ON = 0x0;
		public const int STA_INTERLOCK_OFF = 0x10;

		//ﾌｫｰｶｽﾓｰﾄﾞ
		public const int STA_F1 = 0x0;
		public const int STA_F2 = 0x20;
		public const int STA_F3 = 0x40;
		public const int STA_F4 = 0x60;

		//真空ﾚﾍﾞﾙ
		public const int STA_LO = 0x0;
		public const int STA_V1 = 0x100;
		public const int STA_V2 = 0x200;
		public const int STA_V3 = 0x300;
		public const int STA_V4 = 0x400;
		public const int STA_V5 = 0x500;
			//真空度ｴﾗｰ
		public const int STA_NG = 0x600;

		//ｱｼﾞｬｽﾄ状態
		public const int STA_ADJ_OFF = 0x0;
		public const int STA_ADJ_1 = 0x1000;
		public const int STA_ADJ_2 = 0x2000;
		public const int STA_ADJ_3 = 0x3000;

		//一括ｱｼﾞｬｽﾄ状態
		public const int STA_ADA_OFF = 0x0;
		public const int STA_ADA_ON = 0x4000;

		//ﾀｰｹﾞｯﾄ電流制限値到達
		public const int STA_STG_OFF = 0x0;
		public const int STA_STG_ON = 0x8000;


		//----- STCコマンド用 -----
		public const int STC_STI_MASK = 0x7;			// 状態確認マスク
		public const int STC_SIN_MASK = 0x10;			// インタロック確認マスク
		public const int STC_SFC_MASK = 0x60;			// 焦点モード確認マスク
		public const int STC_SER_MASK = 0x3000;			// 焦点モード確認マスク

		public const int STC_OVER = 0x4;				// 荷保護機能動作中
		public const int STC_PREHEAT = 0x6;				// プリヒート中
		public const int STC_NOT_READY = 0x5;			// X線が照射できない状態
		public const int STC_WARMUP = 0x1;				// ウォーミングアップ動作中
		public const int STC_XON = 0x3;					// X線照射中
		public const int STC_WARMUP_YET = 0x0;			// ウォーミングアップ待機中
		public const int STC_STANDBY = 0x2;				// X線照射待機中
		public const int STC_INTERLOCK_ON = 0x0;		// インタロックON
		public const int STC_INTERLOCK_OFF = 0x10;		// インタロックOFF
		public const int STC_FCS_1 = 0x0;				// 小焦点
		public const int STC_FCS_2 = 0x20;				// 中焦点
		public const int STC_FCS_3 = 0x40;				// 大焦点
		public const int STC_FCS_4 = 0x80;				// 大焦点
		public const int STC_ERR3 = 0x1000;				// ERR3
		public const int STC_ERR4 = 0x2000;				// ERR4

		//------------------------------------------------------------------------------------------
		//   プロパティ・イベント用
		//
		//------------------------------------------------------------------------------------------

		//----- X線 -----
		public const int RET_XRAY_ON = 1;				// ON中
		public const int RET_XRAY_OFF = 0;				// OFF

		//----- STI -----
		public const int RET_OVER = 0;					// 荷保護機能動作中
		public const int RET_PREHEAT = 1;				// プリヒート中
		public const int RET_NOT_READY = 2;				// X線が照射できない状態
		public const int RET_WARMUP = 3;				// ウォーミングアップ動作中
		public const int RET_XON = 4;					// X線照射中
		public const int RET_WARMUP_YET = 5;			// ウォーミングアップ待機中
		public const int RET_STANDBY = 6;				// X線照射待機中

		//----- インタロック -----
		public const int RET_INTER_LOCK_ON = 1;			// ON
		public const int RET_INTER_LOCK_OFF = 0;		// OFF

		//----- ウォームアップ -----
		public const int RET_SWE_WARMUP_END = 0;		// 完了
		public const int RET_SWE_WARMUP_YET = 1;		// 未完
		public const int RET_SWR_WARMUP_ON = 1;			// 動作中
		public const int RET_SWR_WARMUP_OFF = 0;		// 処理なし

//追加2009/08/25(KSS)hata_L10801対応 ---------->
		//ウォームアップ（L10801用）
		public const int gcintSweEnd = 0;				//完了
		public const int gcintSweRun = 1;				//動作中
		public const int gcintSweYet = 2;				//未完
		public const int gcintSweTimeOver = 3;			//40分を経過しても完了しなかった。
//追加2009/08/25(KSS)hata_L10801対応 ----------<

		//----- 焦点 -----
		public const int RET_FCS1 = 1;					// 小焦点
		public const int RET_FCS2 = 2;					// 中焦点
		public const int RET_FCS3 = 3;					// 大焦点
		public const int RET_FCS4 = 4;					//

		//----- 真空度 -----
		public const string RET_SVC_LO = "Lo";
		public const string RET_SVC_V1 = "V1";
		public const string RET_SVC_V2 = "V2";
		public const string RET_SVC_V3 = "V3";
		public const string RET_SVC_V4 = "V4";
		public const string RET_SVC_V5 = "V5";
		public const string RET_SVC_V6 = "V6";
		public const string RET_SVC_NG = "NG";

		//----- ｱｼﾞｬｽﾄ状態 -----
		public const int RET_SAD_OFF = 0;
		public const int RET_SAD_1 = 1;
		public const int RET_SAD_2 = 2;
		public const int RET_SAD_3 = 3;
		public const int RET_SAD_ALL = 4;

		//---- 異常 ----
		//Public Const RET_SER_1              As Integer = 1
		//Public Const RET_SER_2              As Integer = 2
		//Public Const RET_SER_3              As Integer = 3
		//Public Const RET_SER_4              As Integer = 4
		//Public Const RET_SER_5              As Integer = 5
		//Public Const RET_SER_6              As Integer = 6
		//Public Const RET_SER_OK             As Integer = 0

		//------------------------------------------------------------------------------------------
		//   メソッド用
		//
		//------------------------------------------------------------------------------------------
		//X線ON/OFF
		public const int XRAY_ON_ON = 1;			//X線ON実行
		public const int XRAY_ON_OFF = 0;			//X線ON処理なし
		public const int XRAY_OFF_ON = 1;			//X線OFF実行
		public const int XRAY_OFF_OFF = 0;			//X線OFF処理なし

		//ウォームアップ
		public const int WARMUP_ON_ON = 1;			//実行
		public const int WARMUP_ON_OFF = 0;			//処理なし
		public const int WARMUP_CANCEL_ON = 1;		//キャンセル実行
		public const int WARMUP_CANCEL_OFF = 0;		//キャンセル処理なし
		public const int WARMUP_QUIT_ON = 1;		//強制終了実行
		public const int WARMUP_QUIT_OFF = 0;		//強制終了処理なし

		//フォーカス
		public const int FOCUS_OFF = 0;				//処理なし
		public const int FOCUS_F1 = 1;				//F1
		public const int FOCUS_F2 = 2;				//F2
		public const int FOCUS_F3 = 3;				//F3
		public const int FOCUS_F4 = 4;				//F4
		//自動X線OFF時間
		public const int AUTO_OFF_TIME_MAX = 99;	//最大値
		public const int AUTO_OFF_TIME_MIN = 1;		//最小値


//削除2009/08/24(KSS)hata_L10801対応に伴う変更 ------------------------->
//L10801追加のためｸﾞﾛｰﾊﾞﾙからﾌﾟﾗｲﾍﾞｰﾄへ
//'2004-05-17 Shibui L9191用----------------------------------
		//'フォーカス値
		//'2004-09-27 Shibui
		//'Public Const MAX_OBJ            As Integer = 15000  '最大値
		//Public Const MAX_OBJ            As Single = 1500   '最大値
		//
		//Public Const MIN_OBJ            As Integer = 0      '最小値
		//
		//'X方向アライメント値
		//Public Const MAX_OBX            As Integer = 1000   '最大値
		//Public Const MIN_OBX            As Integer = -1000  '最小値
		//
		//'Y方向アライメント値
		//Public Const MAX_OBY            As Integer = 1000   '最大値
		//Public Const MIN_OBY            As Integer = -1000  '最小値
//'------------------------------------------------------------
//削除2009/08/24(KSS)hata_L10801対応に伴う変更 -------------------------<

		//X線情報表示用
		//Public Const DSP_STI            As Integer = 1
		//Public Const DSP_STX            As Integer = 2
		//Public Const DSP_SOV            As Integer = 3
		//Public Const DSP_SHV            As Integer = 4
		//Public Const DSP_SCU            As Integer = 5
		//Public Const DSP_SPV            As Integer = 6
		//Public Const DSP_SPC            As Integer = 7
		//Public Const DSP_SWR            As Integer = 8
		//Public Const DSP_SWE            As Integer = 9
		//Public Const DSP_SWS            As Integer = 10
		//Public Const DSP_SFC            As Integer = 11
		//Public Const DSP_SIN            As Integer = 12
		//Public Const DSP_STM            As Integer = 13
		//Public Const DSP_SXT            As Integer = 14
		//Public Const DSP_SAT            As Integer = 15
		//Public Const DSP_TYP            As Integer = 16
		//Public Const DSP_SER            As Integer = 17

		//Public Const DSP_STG            As Integer = 18
		//Public Const DSP_SBX            As Integer = 19
		//Public Const DSP_SBY            As Integer = 20
		//Public Const DSP_SAD            As Integer = 21
		//Public Const DSP_SOB            As Integer = 22
		//Public Const DSP_SVV            As Integer = 23
		//Public Const DSP_SVC            As Integer = 24
		//Public Const DSP_STA            As Integer = 25
		//Public Const DSP_STL            As Integer = 26
		public const int DSP_SVI = 27;
		//Public Const DSP_SHM            As Integer = 28
		//Public Const DSP_SHS            As Integer = 29
		//Public Const DSP_SHT            As Integer = 30


		public const int DSP_XRAY_INFO_MAX = 30;			//X線情報の数

		public static string[] gstrXrayDisp = new string[DSP_XRAY_INFO_MAX];	//X線情報表示用
		//Public gstrXrayDispTmp(1 To DSP_XRAY_INFO_MAX)  As String				'状態保持用

//2003-09-17 Shibui
		//管電圧・管電流設定でエラー（ワット範囲超過等）が発生した場合、現在の値でイベントを発生させる。
		public static int gintXraySetValueError = 0;

//2003-10-15 Shibui
		public static bool gblnXrayOver = false;

		public static int gintOldErrXrayComm = 0;				//通信異常フラグ

//2004-03-23 Shibui
		public static int gintTmpVol = 0;
		public static int gintTmpAmp = 0;
		public static bool gblnXrayValue = false;
		public static int gintXrayValueDisp = 0;
//追加2010/02/24(KSS)hata_L8601対応 ---------->
		public static float gsngTmpVol = 0;
		public static float gsngTmpAmp = 0;
//追加2010/02/24(KSS)hata_L8601対応 ----------<


//追加2009/10/08(KSS)hata_L10801対応 ---------->
		public static int gintOBJ_Set = 0;			//ﾌｫｰｶｽ設定ﾌﾗｸﾞ
		public static int gintOBX_Set = 0;			//Xｱﾗｲﾒﾝﾄ設定ﾌﾗｸﾞ
		public static int gintOBY_Set = 0;			//Yｱﾗｲﾒﾝﾄ設定ﾌﾗｸﾞ
		public static int gintCMV_Set = 0;			//上限管電圧制限 'v15.10追加 byやまおか 2009/11/12
//追加2009/10/08(KSS)hata_L10801対応 ----------<

//追加2015/10/05(KS1)長野_L10711対応 ----------> 
        public static int gintCAJ_Set = 0;			//ﾌｫｰｶｽ設定ﾌﾗｸﾞ
        public static int gintCAX_Set = 0;			//Xｱﾗｲﾒﾝﾄ設定ﾌﾗｸﾞ
        public static int gintCAY_Set = 0;			//Yｱﾗｲﾒﾝﾄ設定ﾌﾗｸﾞ
        public static int gintMDE_Set = 0;        //フィラメントモード設定
//追加2015/10/05(KS1)長野_L10711対応 ----------<



//2004-09-07 Shibui 9191対応------------------------------------------------------------->
//メソッド指示格納バッファ
//名称変更2009/10/08(KSS)hata_L10801対応 ---------->
		//Public Type udtXrayL9191M
		public struct udtXrayMethodValue
		{
//名称変更2009/10/08(KSS)hata_L10801対応 ---------->
//2004-09-27 Shibui Integer → Single
//			intOBJ      As Integer  'フォーカス値
			public float sngOBJ;				//フォーカス値
			public int intSAV;					//フォーカス値を保存する
			public int intOST;					//フォーカス値を自動的に決定する
			public int intOBX;					//電子ビームのX方向位置を調整する
			public int intOBY;					//電子ビームのY方向位置を調整する
			public int intADJ;					//電子ビームのビームアライメントを調整する
			public int intADA;					//電子ビームのビームアライメント調整を一括で実施する
			public int intSTP;					//電子ビームのビームアライメント調整を中止する
			public int intRST;					//過負荷保護機能を解除する
			public int intWarmUp;				//ウォームアップ完了状態時にウォームアップを開始する

//追加2009/10/08(KSS)hata_L10801対応 ---------->
			public int intADJ_SAV;				//アライメント値を保存する
			public int intDDL;					//デフォルト値読み出し
			public int intCMV;					//上限管電圧制限 'v15.10追加 byやまおか 2009/11/12
//追加2009/10/08(KSS)hata_L10801対応 ----------<

//追加2015/10/05(KS1)長野_L10711対応 ---------->
            public int intCAX;					//電子ビームのX方向位置を調整する
            public int intCAY;					//電子ビームのY方向位置を調整する
            public int intMDE;                   //フィラメントモードの設定
//追加2015/10/05(KS1)長野_L10711対応 ----------<

		}
//変更2009/10/08(KSS)hata_L10801対応 ---------->
		//Public gudtXrayL9191MTmp As udtXrayMethodValue
		//Public gudtXrayL9191M    As udtXrayMethodValue
		public static udtXrayMethodValue gudtXrayMTmp = new udtXrayMethodValue();
		public static udtXrayMethodValue gudtXrayM = new udtXrayMethodValue();
//変更200910//08/(KSS)hata_L10801対応 ----------<

//L9191用プロパティ情報格納バッファ
//名称変更2009/10/08(KSS)hata_L10801対応 ---------->
		//Public Type udtXrayL9191P
		public struct udtXrayProperty
		{
//名称変更2009/10/08(KSS)hata_L10801対応 ---------->

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			public int intTargetInf;			//ターゲット電流ステータスの有無
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			public float sngTargetInfSTG;		//ターゲット電流
			public int intTargetLimit;			//ターゲット電流値到達
            public float sngCurrentSAP;         //アパーチャ電流 L10711 対応 Rev23.10  by長野 2015/10/05

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			public int intVacuumInf;			//真空度情報の有無
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			public string strVacuumSVC;			//真空度
			public int intSBX;					//X軸方向アライメント確認
			public int intSBY;					//Y軸方向アライメント確認

//変更2009/08/25(KSS)hata_L10801対応 ---------->
            public int intSCX;					//X軸方向アライメント確認(コンデンサ) L10711対応 Rev23.10 by長野 2015/10/05
            public int intSCY;					//Y軸方向アライメント確認(コンデンサ) L10711対応 Rev23.10 by長野 2015/10/05
            public int intSMD;                  //フィラメントモード Rev23.10 by長野 2015/10/05
            public float sngSVS;                //Sモードフィラメント設定電圧確認
//変更2009/08/25(KSS)hata_L10801対応 ----------<

            public int intSAD;					//アライメントモニタ
			public float sngSOB;				//フォーカス値
            public float sngCOB;                //コンデンサフォーカス値 Rev23.10 L10711対応 by長野 2015/10/05
			public float sngSVV;				//真空計値
			public int lngSTM;					//電源ON通電時間
			public int lngSXT;					//X線照射時間
			public int intSHM;					//フィラメント入力確認
			public float sngSHS;				//フィラメント設定電圧確認
//変更2009/08/25(KSS)hata_L10801対応 ---------->
//			intSHT          As Integer  'フィラメント通電時間
			public int lngSHT;					//フィラメント通電時間
//変更2009/08/25(KSS)hata_L10801対応 ----------<
			public int intSAT;					//自動X線停止時間
			public int intSOV;					//過負荷保護機構
			public int intSER;					//制御基板異常

//追加2009/08/25(KSS)hata_L10801対応 ---------->
			public int intFLM;					//フィラメント状態確認（0:異常なし、1:ﾗｲﾌｴﾝﾄﾞ近し、2:断線）
			public int intSSA;					//ステータス自動送信確認（0-10）
			public int intZT1;					//温度の確認（0-150）
			public string strTYP;				//X線装置型名
			public int intSWE;					//ウォーミングアップ状態
			public string strSWU;				//ウォーミングアップ管電圧上昇下降パラメータ確認
			public int intSMV;					//使用上限管電圧読み出し（20-230）   'v15.10追加 byやまおか 2009/11/12
//追加2009/08/25(KSS)hata_L10801対応 ----------<


		}
//変更2009/10/08(KSS)hata_L10801対応 ---------->
//		Public gudtXrayL9191P   As udtXrayProperty
		public static udtXrayProperty gudtXrayP = new udtXrayProperty();
//変更2009/10/08(KSS)hata_L10801対応 ----------<
//2004-09-07 Shibui 9191対応-------------------------------------------------------------<

//追加2009/08/31(KSS)hata_L10801対応 ---------->
		public static bool gblnXrayValueSet = false;
		public static bool gblnSendHIV = false;			//管電圧値送信フラグ
		public static bool gblnSendCUR = false;			//管電流値送信フラグ
		public static bool gblnRecvPHV = false;			//設定管電圧値読み込みフラグ
		public static bool gblnRecvPCU = false;			//設定管電流値読み込みフラグ
//追加2009/08/31(KSS)hata_L10801対応 ----------<

//追加2010/02/22(KSS)hata_L8601用 ---------->
		public static int ipXrayOperateSRL = 0;		//ｵﾍﾟﾚｰﾄｽｲｯﾁ状態確認(0：OFF、1:REMOTE、2:LOCAL)
		public static int ipXrayRemoteSRB = 0;		//ﾘﾓｰﾄ動作状態確認(0:BUSY、1:READY)

//追加2010/05/13(KS1)hata_L9421-02対応
		public static int ipXrayBatterySBT = 0;		//バッテリー確認(0：正常、1:Low)


		//'----------------------------------------------------------------------------------------------------
		//'
		//'   X線情報表示（frmDspXray）
		//'
		//'----------------------------------------------------------------------------------------------------
		//Public Sub XrayDisp()
		//'    Dim intCnt  As Integer
		//'    Dim intRow  As Integer
		//'
		//'    '----- X線 -----
		//'    If gblnfrmXray = True Then  'フォームが表示されている時のみ実行
		//'        For intCnt = 1 To DSP_XRAY_INFO_MAX
		//            If gintDspXrayOperation = OPERATION_SERVICE _
		//'            Or (intCnt <> DSP_STM And intCnt <> DSP_SXT And intCnt <> DSP_TYP) Then         'ユーザモード時には表示しない項目
		//'                If frmDspXray.lblDspXray(intCnt).Caption <> gstrXrayDisp(intCnt) Then       '状態変化あり
		//'                    frmDspXray.lblDspXray(intCnt).Caption = gstrXrayDisp(intCnt)            '表示
		//                    gstrXrayDispTmp(intCnt) = gstrXrayDisp(intCnt)                         '状態保持
		//'                End If
		//            End If
		//'        Next intCnt
		//'    End If
		//End Sub
		//
		//Option Explicit
		//
		//'*******************************************************************************
		//'
		//'   X線制御に必要な変数を宣言する
		//'
		//'   Date        Version     Designed/Changed
		//'   2003/05/28  2.00        (NSD)Shibui
		//'
		//'*******************************************************************************
		//
		//Public gintXrayErr                  As Integer      'X線装置の異常状況
		//
		//'----- X線装置の異常状況変数用 -----
		//Public Const XRAY_OK                As Integer = 0
		//Public Const XRAY_ERR1              As Integer = 1
		//Public Const XRAY_ERR2              As Integer = 2
		//Public Const XRAY_ERR3              As Integer = 3
		//Public Const XRAY_ERR4              As Integer = 4
		//
		//'------------------------------------------------------------------------------------------
		//'   X線装置用
		//'
		//'------------------------------------------------------------------------------------------
		//
		//'----- STCコマンド用 -----
		//Public Const STC_STI_MASK           As Integer = &H7                ' 状態確認マスク
		//Public Const STC_SIN_MASK           As Integer = &H10               ' インタロック確認マスク
		//Public Const STC_SFC_MASK           As Integer = &H60               ' 焦点モード確認マスク
		//Public Const STC_SER_MASK           As Integer = &H3000             ' 焦点モード確認マスク
		//
		//Public Const STC_OVER               As Integer = &H4                ' 荷保護機能動作中
		//Public Const STC_PREHEAT            As Integer = &H6                ' プリヒート中
		//Public Const STC_NOT_READY          As Integer = &H5                ' X線が照射できない状態
		//Public Const STC_WARMUP             As Integer = &H1                ' ウォーミングアップ動作中
		//Public Const STC_XON                As Integer = &H3                ' X線照射中
		//Public Const STC_WARMUP_YET         As Integer = &H0                ' ウォーミングアップ待機中
		//Public Const STC_STANDBY            As Integer = &H2                ' X線照射待機中
		//Public Const STC_INTERLOCK_ON       As Integer = &H0                ' インタロックON
		//Public Const STC_INTERLOCK_OFF      As Integer = &H10               ' インタロックOFF
		//Public Const STC_FCS_1              As Integer = &H0                ' 小焦点
		//Public Const STC_FCS_2              As Integer = &H20               ' 中焦点
		//Public Const STC_FCS_3              As Integer = &H40               ' 大焦点
		//Public Const STC_ERR3               As Integer = &H1000             ' ERR3
		//Public Const STC_ERR4               As Integer = &H2000             ' ERR4
		//
		//'------------------------------------------------------------------------------------------
		//'   プロパティ・イベント用
		//'
		//'------------------------------------------------------------------------------------------
		//
		//'----- X線 -----
		//Public Const RET_XRAY_ON           As Integer = 1                  ' ON中
		//Public Const RET_XRAY_OFF          As Integer = 0                  ' OFF
		//
		//'----- STI -----
		//Public Const RET_OVER              As Integer = 0                  ' 荷保護機能動作中
		//Public Const RET_PREHEAT           As Integer = 1                  ' プリヒート中
		//Public Const RET_NOT_READY         As Integer = 2                  ' X線が照射できない状態
		//Public Const RET_WARMUP            As Integer = 3                  ' ウォーミングアップ動作中
		//Public Const RET_XON               As Integer = 4                  ' X線照射中
		//Public Const RET_WARMUP_YET        As Integer = 5                  ' ウォーミングアップ待機中
		//Public Const RET_STANDBY           As Integer = 6                  ' X線照射待機中
		//
		//'----- インタロック -----
		//Public Const RET_INTER_LOCK_ON     As Integer = 1                   ' ON
		//Public Const RET_INTER_LOCK_OFF    As Integer = 0                   ' OFF
		//
		//'----- ウォームアップ -----
		//Public Const RET_SWE_WARMUP_END    As Integer = 0                   ' 完了
		//Public Const RET_SWE_WARMUP_YET    As Integer = 1                   ' 未完
		//Public Const RET_SWR_WARMUP_ON     As Integer = 1                   ' 動作中
		//Public Const RET_SWR_WARMUP_OFF    As Integer = 0                   ' 処理なし
		//
		//'----- 焦点 -----
		//Public Const RET_FCS1              As Integer = 1                   ' 小焦点
		//Public Const RET_FCS2              As Integer = 2                   ' 中焦点
		//Public Const RET_FCS3              As Integer = 3                   ' 大焦点
		//
		//'------------------------------------------------------------------------------------------
		//'   メソッド用
		//'
		//'------------------------------------------------------------------------------------------
		//'X線ON/OFF
		//Public Const XRAY_ON_ON         As Integer = 1  'X線ON実行
		//Public Const XRAY_ON_OFF        As Integer = 0  'X線ON処理なし
		//Public Const XRAY_OFF_ON        As Integer = 1  'X線OFF実行
		//Public Const XRAY_OFF_OFF       As Integer = 0  'X線OFF処理なし
		//
		//'ウォームアップ
		//Public Const WARMUP_ON_ON       As Integer = 1  '実行
		//Public Const WARMUP_ON_OFF      As Integer = 0  '処理なし
		//Public Const WARMUP_CANCEL_ON   As Integer = 1  'キャンセル実行
		//Public Const WARMUP_CANCEL_OFF  As Integer = 0  'キャンセル処理なし
		//Public Const WARMUP_QUIT_ON     As Integer = 1  '強制終了実行
		//Public Const WARMUP_QUIT_OFF    As Integer = 0  '強制終了処理なし
		//
		//'フォーカス
		//Public Const FOCUS_OFF          As Integer = 0  '処理なし
		//Public Const FOCUS_F1           As Integer = 1  'F1
		//Public Const FOCUS_F2           As Integer = 2  'F2
		//Public Const FOCUS_F3           As Integer = 3  'F3
		//Public Const FOCUS_F4           As Integer = 4  'F4
		//
		//'自動X線OFF時間
		//Public Const AUTO_OFF_TIME_MAX  As Integer = 99 '最大値
		//Public Const AUTO_OFF_TIME_MIN  As Integer = 1  '最小値
		//
		//'X線情報表示用
		//Public Const DSP_STI            As Integer = 1
		//Public Const DSP_STX            As Integer = 2
		//Public Const DSP_SOV            As Integer = 3
		//Public Const DSP_SHV            As Integer = 4
		//Public Const DSP_SCU            As Integer = 5
		//Public Const DSP_SPV            As Integer = 6
		//Public Const DSP_SPC            As Integer = 7
		//Public Const DSP_SWR            As Integer = 8
		//Public Const DSP_SWE            As Integer = 9
		//Public Const DSP_SWS            As Integer = 10
		//Public Const DSP_SFC            As Integer = 11
		//Public Const DSP_SIN            As Integer = 12
		//Public Const DSP_STM            As Integer = 13
		//Public Const DSP_SXT            As Integer = 14
		//Public Const DSP_SAT            As Integer = 15
		//Public Const DSP_TYP            As Integer = 16
		//Public Const DSP_SER            As Integer = 17
		//Public Const DSP_XRAY_INFO_MAX  As Integer = 17 'X線情報の数
		//
		//Public gstrXrayDisp(1 To 17)    As String   'X線情報表示用
		//Public gstrXrayDispTmp(1 To 17) As String   '状態保持用
		//
		//'2003-09-17 Shibui
		//'管電圧・管電流設定でエラー（ワット範囲超過等）が発生した場合、現在の値でイベントを発生させる。
		//Public gintXraySetValueError    As Integer
		//
		//'2003-10-15 Shibui
		//Public gblnXrayOver             As Boolean
		//
		//'2004-03-23 Shibui
		//Public gintTmpVol               As Integer
		//Public gintTmpAmp               As Integer
		//Public gblnXrayValue            As Boolean
		//Public gintXrayValueDisp        As Integer
		//
		//'----------------------------------------------------------------------------------------------------
		//'
		//'   X線情報表示（frmDspXray）
		//'
		//'----------------------------------------------------------------------------------------------------
		//'Public Sub XrayDisp()
		//'    Dim intCnt  As Integer
		//'    Dim intRow  As Integer
		//'
		//'    '----- X線 -----
		//'    If gblnfrmXray = True Then  'フォームが表示されている時のみ実行
		//'        For intCnt = 1 To DSP_XRAY_INFO_MAX
		//            If gintDspXrayOperation = OPERATION_SERVICE _
		//'            Or (intCnt <> DSP_STM And intCnt <> DSP_SXT And intCnt <> DSP_TYP) Then         'ユーザモード時には表示しない項目
		//'                If frmDspXray.lblDspXray(intCnt).Caption <> gstrXrayDisp(intCnt) Then       '状態変化あり
		//'                    frmDspXray.lblDspXray(intCnt).Caption = gstrXrayDisp(intCnt)            '表示
		//                    gstrXrayDispTmp(intCnt) = gstrXrayDisp(intCnt)                         '状態保持
		//'                End If
		//            End If
		//'        Next intCnt
		//'    End If
		//'End Sub
		//
	}
}
