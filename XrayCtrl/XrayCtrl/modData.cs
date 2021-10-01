using System;

namespace XrayCtrl
{
	internal static class modData
	{
		//-----------------------------------------------------------------------------------------
		//   定数宣言
		//-----------------------------------------------------------------------------------------

		//----- 処理あり/なし -----
		public const int CTRL_ON = 1;						//処理あり
		public const int CTRL_OFF = 0;						//処理なし

		//----- EventValue_Setﾒｿｯﾄﾞのﾊﾟﾗﾒｰﾀ -----
		public const int EVENT_NO = 0;						//処理なし
		public const int EVENT_FEIN_FOCUS = 1;				//FeinFocus
		public const int EVENT_STOP = 2;					//ｲﾍﾞﾝﾄ停止
		public const int EVENT_KEVEX = 3;					//Kevex
		//2004-09-07 Shibui
		public const int EVENT_L9421 = 4;					//浜ﾎﾄ（90kV L9421）
		public const int EVENT_L9181 = 5;					//浜ﾎﾄ（130kV L9181）
		public const int EVENT_L9191 = 6;					//浜ﾎﾄ（160kV L9191）
		public const int EVENT_L10801 = 7;					//浜ﾎﾄ（230kV L10801）     //追加2009/08/19(KSS)hata_L10801対応
        public const int EVENT_L8601 = 8;					//浜ﾎﾄ（90kV分離型 L8601） //追加2010/02/19(KSS)hata_L8601対応
		public const int EVENT_L9421_02 = 9;				//浜ﾎﾄ（90kV L9421-02T）   //追加2010/05/11(KS1)hata_L9421-02T対応
		public const int EVENT_L8121_02 = 11;				//浜ﾎﾄ（150kV L8121-02）   //追加2012/03/20(KS1)hata_L8121-02対応
        public const int EVENT_L9181_02 = 12;				//浜ﾎﾄ（130kV L9181-02）   //追加2014/10/07(検S1)hata_64bit対応
        public const int EVENT_L12721 = 13;				    //浜ﾎﾄ（300kV L12721）     //Rev23.10 追加2015/10/01(検S1) by長野
        public const int EVENT_L10711 = 14;				    //浜ﾎﾄ（130kV L9181-02）   //Rev23.10 追加2014/10/07(検S1) by長野

		//-----------------------------------------------------------------------------------------
		//   変数宣言
		//-----------------------------------------------------------------------------------------
		//2004-09-07 Shibui
		public static clsCtrlXrayL9421S gclsCtrlXrayL9421S;		//X線制御用 浜ﾎﾄ（ 90kV L9421）
		public static clsCtrlXrayL9181S gclsCtrlXrayL9181S;		//X線制御用 浜ﾎﾄ（130kV L9181）
		public static clsCtrlXrayL9191 gclsCtrlXrayL9191;		//X線制御用 浜ﾎﾄ（160kV L9191）
		public static clsCtrlXrayL10801 gclsCtrlXrayL10801;		//X線制御用 浜ﾎﾄ（230kV L10801）          //追加2009/08/19(KSS)hata_L10801対応
		public static clsCtrlXrayL8601 gclsCtrlXrayL8601;		//X線制御用 浜ﾎﾄ（90kV分離型 L8601）      //追加2010/02/19(KSS)hata_L8601対応
        public static clsCtrlXrayL9421_02 gclsCtrlXrayL9421_02;	//X線制御用 浜ﾎﾄ（90kV一体型 L9421-02）   //追加2010/05/11(KS1)hata_L9421-02T対応 
        public static clsCtrlXrayL8121_02 gclsCtrlXrayL8121_02;	//X線制御用 浜ﾎﾄ（150kV分離型 L8121-02）  //追加2012/03/20(KS1)hata_L8121ｰ02対応
        public static clsCtrlXrayL9181_02 gclsCtrlXrayL9181_02;	//X線制御用 浜ﾎﾄ（130kV一体型 L9181-02）  //追加2014/10/07(検S1)hata_64bit対応
        public static clsCtrlXrayL12721 gclsCtrlXrayL12721;	//X線制御用 浜ﾎﾄ（300kV L12721）  // Rev23.10　追加 2015/10/01 (検S1)長野 Rev23.10
        public static clsCtrlXrayL10711 gclsCtrlXrayL10711;	//X線制御用 浜ﾎﾄ（160kV L10711）  // Rev23.10　追加 2014/10/07 (検S1)長野 Rev23.10

        public static int gintXrayClsCnt;							//ｸﾗｽｲﾆｼｬﾗｲｽﾞｶｳﾝﾀ（X線制御用）

		public static int gintSWS;									//ｳｫｰﾑｱｯﾌﾟｽﾃｯﾌﾟ
		//public gintErrNoValue           As Integer		//ｴﾗｰNo
		public static int gintStatusNoValue;						//ｽﾃｰﾀｽNo

        //Rev23.10 追加 by長野 2015/09/29
        public static int portNo = 0;

		//-----------------------------------------------------------------------------------------
		//   ｴﾗｰNo.
		//-----------------------------------------------------------------------------------------
		public const int ERR_XRAY_ERR3 = 21;				//X線発生装置ERR No3（ﾄﾗﾝｼﾞｽﾀ故障）
		public const int ERR_XRAY_ERR4 = 22;				//X線発生装置ERR No4（高圧電源供給異常）
		public const int ERR_XRAY_ERR2 = 23;				//X線発生装置ERR No2（ｲﾝﾀｰﾛｯｸ2用ﾘﾚｰ故障）
		public const int ERR_XRAY_ERR5 = 24;				//X線発生装置ERR No5（ﾌｨﾗﾒﾝﾄ断線）
		public const int ERR_XRAY_COMM = 25;				//通信ERR
		//Public Const ERR_XRAY_FILE              As Integer = 26 //INIファイル読込みERR
		//変更2009/08/25(KSS)hata_L10801対応 ---------->
		public const int ERR_XRAY_ERR1 = 26;				//X線発生装置ERR No1（電池切れ）
		public const int ERR_XRAY_ERR209 = 27;				//X線発生装置ERR No209（温度ｴﾗｰ）
		//変更2009/08/25(KSS)hata_L10801対応 ----------<

		//追加2010/02/25(KSS)hata_L8601対応 ---------->
		public const int ERR_XRAY_ERR31 = 31;				//X線発生装置ERR No1（ﾘﾚｰ故障）
		public const int ERR_XRAY_ERR32 = 32;				//X線発生装置ERR No2（ﾘﾚｰ故障）
		public const int ERR_XRAY_ERR33 = 33;				//X線発生装置ERR No3（ﾘﾚｰ故障）
		public const int ERR_XRAY_ERR34 = 34;				//X線発生装置ERR No4（ﾘﾚｰ故障）
		//追加2010/02/25(KSS)hata_L8601対応 ----------<


		//追加2010/05/13(KS1)hata_L9421-02対応 ---------->
		public const int ERR_XRAY_ERR200 = 41;				//送風ファン異常
		public const int ERR_XRAY_ERR201 = 42;				//入力電源電圧異常1
		public const int ERR_XRAY_ERR202 = 43;				//入力電源電圧異常2
		public const int ERR_XRAY_ERR203 = 44;				//制御基板異常6
		public const int ERR_XRAY_ERR204 = 45;				//制御基板異常3
		public const int ERR_XRAY_ERR206 = 46;				//制御基板異常4
		public const int ERR_XRAY_ERR207 = 47;				//制御基板異常5
		public const int ERR_XRAY_ERR208 = 48;				//入力電源電圧異常3
		//追加2010/05/13(KS1)hata_L9421-02対応 ----------<

        public const int ERR_XRAY_COMM_LINE = 51;		    //通信ラインエラー              //追加2015/04/03hata
		


		//-----------------------------------------------------------------------------------------
		//X線装置情報
		//-----------------------------------------------------------------------------------------
		public struct XraySystemValue
		{
			public int E_XrayScaleMaxkV;		//設定可能最大管電圧(ｋV)（設定フォームMAX値）
			public int E_XrayScaleMaxuA;		//設定可能最大管電流(μA)（設定フォームMAX値）
			public int E_XrayScaleMinkV;		//設定可能最小管電圧(ｋV)（設定フォームMIN値）
			public int E_XrayScaleMinuA;		//設定可能最小管電流(μA)（設定フォームMIN値）
			public int E_XraySetMaxkV;			//設定最大管電圧(ｋV)
			public int E_XraySetMaxuA;			//設定最大管電流(μA)
			public int E_XraySetMinkV;			//設定最小管電圧(ｋV)
			public int E_XraySetMinuA;			//設定最小管電流(μA)

			public float E_XrayMaxPower;		//最大POWER制限が最大（W)かタゲット電流の把握
			public float E_XrayF1MaxPower;		//フォーカスF1時の最大(W)又はターゲット電流(μA)
			public float E_XrayF2MaxPower;		//フォーカスF2時の最大(W)又はターゲット電流(μA)
			public float E_XrayF3MaxPower;		//フォーカスF3時の最大(W)又はターゲット電流(μA)
			public float E_XrayF4MaxPower;		//フォーカスF4時の最大(W)又はターゲット電流(μA)

			public int E_XrayFocusNumber;		//X線フォーカス選択数
			public int E_XrayAvailkV;			//アベイラブル管電圧範囲(kV)
			public int E_XrayAvailuA;			//アベイラブル管電圧範囲(μA)
			public int E_XrayAvailTimeInside;	//X線ON中に設定値を変更した場合のアベイラブル待ち時間
			public int E_XrayAvailTimeOn;		//X戦OFF→ON時のアベイラブル待ち時間

			public int E_XrayWarmupTime;		//ウォーミングアップパターンの時間表示有り無し
			public int E_XrayWarmup1Time;		//ウォーミングアップパターンの時間表示有り無し
			public int E_XrayWarmup2Time;		//ウォーミングアップパターンの時間表示有り無し
			public int E_XrayWarmup3Time;		//ウォーミングアップパターンの時間表示有り無し
		    
			public int E_XrayTargetInfSTG;		//ターゲット電流ステータスの有無
			public int E_XrayVacuumInf;			//真空度情報の有無

			//追加2011/09/15(KS1)hata_X線の電力制限
			public int E_XrayWattageLimit;		//X線の電力制限値
		}
		public static XraySystemValue gudtXraySystemValue;
	}
}
