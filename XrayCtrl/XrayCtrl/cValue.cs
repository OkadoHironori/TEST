using System;

namespace XrayCtrl
{
	public class cValue
	{
		//未使用2009/10/08hata
		//9191用メソッド指示格納バッファ
		//名称変更2009/10/08(KSS)hata_L10801対応 ---------->
		public struct udtXrayM
		{
			//public Type udtL9191M
			//名称変更2009/10/08(KSS)hata_L10801対応 ----------<
			public float sngOBJ;		//フォーカス値
			public int intSAV;			//フォーカス値を保存する
			public int intOST; 			//フォーカス値を自動的に決定する
			public int intOBX;			//電子ビームのX方向位置を調整する
			public int intOBY;			//電子ビームのY方向位置を調整する
			public int intADJ;			//電子ビームのビームアライメントを調整する
			public int intADA;			//電子ビームのビームアライメント調整を一括で実施する
			public int intSTP;			//電子ビームのビームアライメント調整を中止する
			public int intRST;			//過負荷保護機能を解除する
			public int intWarmUp;		//ウォームアップ完了状態時にウォームアップを開始する
			//追加2009/08/25(KSS)hata_L10801対応 ----------->
			public int intADJ_SAV;		//アライメント値を保存する
			public int intDDL;			//デフォルト値読み出し
			public int intCMV;			//使用上限管電圧を制限する   //v15.10追加 byやまおか 2009/11/12
			//追加2009/08/25(KSS)hata_L10801対応 -----------<
		}
		
		//9191用プロパティ情報格納バッファ
		//名称変更2009/10/08(KSS)hata_L10801対応 ---------->
		public struct udtXrayP
		{
			//public Type udtL9191P
			//名称変更2009/10/08(KSS)hata_L10801対応 ----------<
			public int intTargetInf;		//ターゲット電流ステータスの有無
			public float sngTargetInfSTG;	//ターゲット電流
			public int intTargetLimit;		//ターゲット電流値到達
			public int intVacuumInf;		//真空度情報の有無
			public string strVacuumSVC;		//真空度
			public int intSBX;				//X軸方向アライメント確認
			public int intSBY;				//Y軸方向アライメント確認
			public int intSAD;				//アライメントモニタ
			public int intSAT;				//自動X線停止時間
			public float sngSOB;			//フォーカス値
			public float sngSVV;			//真空計値
			public int lngSTM;				//電源ON通電時間
			public int lngSXT;				//X線照射時間
			public int intSHM;				//フィラメント入力確認
			public float sngSHS;			//フィラメント設定電圧確認
			//変更L10801_2009/08/25(KSS)hata_L10801対応 ----------->
			//    intSHT          As Integer  //フィラメント通電時間
			public int lngSHT;				//フィラメント通電時間
			//変更L10801_2009/08/25(KSS)hata_L10801対応 -----------<
			public int intSOV;				//過負荷保護機構
			public int intSER;				//制御基板異常
			//追加2009/08/25(KSS)hata_L10801対応 ----------->
			public int intFLM;				//フィラメント状態確認（0:異常なし、1:ﾗｲﾌｴﾝﾄﾞ近し、2:断線）
			public int intSSA;				//ステータス自動送信確認（0-10）
			public int intZT1;				//温度の確認（0-150）
			public string strTYP;			//X線装置型名
			//追加2009/08/25(KSS)hata_L10801対応 -----------<
			//追加2009/10/08(KSS)hata_L10801対応 ----------->
			public int intSWE;				//ウォーミングアップ状態
			public string strSWU;			//ウォーミングアップ管電圧上昇下降パラメータ確認
			public int intSMV;				//使用上限管電圧設定読み出し //v15.10追加 byやまおか 2009/11/12
			//追加2009/10/08(KSS)hata_L10801対応 -----------<
			//追加2011/09/15(KS1)hata_L10801X線の電力制限に対応 ----------->
			public int intWattageLimit; 
			//追加2011/09/15(KS1)hata_L10801X線の電力制限に対応 -----------<

            //Rev23.10 L10711 対応 by長野 2015/10/05 ---------->
            public int intSCX;					//X軸方向アライメント確認(コンデンサ) L10711対応 Rev23.10 by長野 2015/10/05
            public int intSCY;					//Y軸方向アライメント確認(コンデンサ) L10711対応 Rev23.10 by長野 2015/10/05
            public int intSMD;                  //フィラメントモード Rev23.10 by長野 2015/10/05
            public float sngSVS;                //Sモードフィラメント設定電圧確認
            public float sngCOB;                //フォーカス値(コンデンサ) L10711 対応 Rev23.10 by長野 2015/10/05
            //Rev23.10 L10711 対応 by長野 2015/10/05 ----------<
		}
		
		//ｲﾍﾞﾝﾄ発生開始/停止 X線種の選択を行う
		//
		//   0:処理なし
		//   1:FeinFocus
		//   2:ｲﾍﾞﾝﾄ停止
		//   3:Kevex
        //   4:浜ﾎﾄ(90kV一体型、L9421)
        //   5:浜ﾎﾄ(130kV一体型、L9181)
		//   6:浜ﾎﾄ(160kV、L9191)
		//   7:浜ﾎﾄ(230kV、L10801)
		//   8:浜ﾎﾄ(90kV分離型、L8601)
        //   9:浜ﾎﾄ(90kV一体型、L9421-02)
        //   10:450kV用
        //   11:浜ﾎﾄ(150kV分離型、L8121-02)
        //   12:浜ﾎﾄ(130kV一体型、L9181-02)
        //
		public int fEventvalue
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					//追加2010/05/20(KS1)hata_L9421-02対応
					//初期化
					InitMethodData();

					ModuleCTRLM.ifEventValue = value;
				}
			}
		}
		
		//X線ON/OFF
		////2000-07-12 T.Shibui
		//   引数
		//   1=X線ON、2=X線OFF
		public int Xrayonoff_Set
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					if ((ModuleCTRLM.ifEventValue == 0 || ModuleCTRLM.ifEventValue == 2) && value == 1)	return;

					//ウォームアップ中はONさせない   //追加10/05/12(KS1)hata_L9421-02対応
					if (value == 1 && ModuleCTRLM.ipWarmup == 1) return;
					
					ModuleCTRLM.ifXrayonoff_Set = value;
				}
			}
		}
		
		//X線アベイラブル
		public int pXAvail
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipXAvail;
				}
			}
		}
		
		//最大管電圧
		public float pXRMaxkV
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipXRMaxkV;
				}
			}
		}
		
		//最大管電流
		public float pXRMaxmA
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipXRMaxmA;
				}
			}
		}
		
		//最小管電圧
		public float pXRMinkV
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipXRMinkV;
				}
			}
		}
		
		//最小管電流
		public float pXRMinmA
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipXRMinmA;
				}
			}
		}
		
		//非常停止
		//未使用
		public int pEmergency
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipEmergency;
				}
			}
		}
		
		//ｴﾗｰNo
		public int pErrsts
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					if (ModuleCTRLM.ifErrrst == 0)
					{
						return ModuleCTRLM.ipErrsts;
					}
					else
					{
						return  0;
					}
				}
			}
		}
		
		//ｴﾗｰﾘｾｯﾄ
		public int fErrrst
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					ModuleCTRLM.ifErrrst = value;
				}
			}
		}
		
		//XRAY
		//X線型式
		public int pX_type
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipX_type;
				}
			}
		}
		
		//管電圧
		public float pX_Volt
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipX_Volt;
				}
			}
		}
		
		//管電圧 上昇
		public int fX_Volt_Up
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					ModuleCTRLM.ifX_Volt_Up = value;
				}
			}
		}
		
		//管電圧 下降
		public int fX_Volt_Down
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					ModuleCTRLM.ifX_Volt_Down = value;
				}
			}
		}
		
		//管電圧値設定
		public int fX_Volt
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					ModuleCTRLM.ifX_Volt = value;
				}
			}
		}
		
		//管電流値
		public float pX_Amp
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipX_Amp;
				}
			}
		}
		
		//管電流 上昇
		public int fX_Amp_Up
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					ModuleCTRLM.ifX_Amp_Up = value;
				}
			}
		}
		
		//管電流 下降
		public int fX_Amp_Down
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					ModuleCTRLM.ifX_Amp_Down = value;
				}
			}
		}
		
		//管電流値設定
		public int fX_Amp
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					ModuleCTRLM.ifX_Amp = value;
				}
			}
		}
		
		public int pX_Fine
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipX_Fine;
				}
			}
		}
		
		public float pX_Watt
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipX_Watt;
				}
			}
		}
		
		public int pX_On
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipX_On;
				}
			}
		}
		
		public int fX_On
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					ModuleCTRLM.ifX_On = value;
				}
			}
		}

		public int pStandby
		{
			//2000-09-14 T.Shibui
			//    pStandby = ipStandby
			get { return 1; }
		}
		
		public int pInterlock
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipInterlock;
				}
			}
		}
		
		public int pFocussize
		{
			get
			{
				lock (cCtrlm.gLock)
				{				

//1999-10-20 T.Shibui
////90kVの時は設定無し					//v14.13削除 07-11-19 byやまおか at現地
//				If ipX_type = 0 Then    //v14.13削除 07-11-19 byやまおか at現地
//					pFocussize = 0		//v14.13削除 07-11-19 byやまおか at現地
//				Else                    //v14.13削除 07-11-19 byやまおか at現地
//2000-03-07 T.Shibui
					if (ModuleCTRLM.ipFocussize == 0)
					{
						return 1;
					}
					else if (ModuleCTRLM.ipFocussize == 1)
					{
						return 2;
					}
					else if (ModuleCTRLM.ipFocussize == 2)
					{
						return 3;
					}
					else if (ModuleCTRLM.ipFocussize == 3)
					{
						return 4;
					}
					else
					{
						return 0;
					}
//						pFocussize = 3
//				End If                 //v14.13削除 07-11-19 byやまおか at現地
				}
			}
		}
		
		//1999-09-25 T.Shibui
		public int fFocussize
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					if (value == 1)
					{
						ModuleCTRLM.ifFocussize = 0;
					}
					else if (value == 2)
					{
						ModuleCTRLM.ifFocussize = 1;
					}
					else if (value == 3)
					{
						ModuleCTRLM.ifFocussize = 2;
					}
					else if (value == 4)
					{
						ModuleCTRLM.ifFocussize = 3;
					}
				}
			}
		}
		
		//ｳｫｰﾑｱｯﾌﾟ
		public int pWarmup
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipWarmup;
				}
			}
		}
		
		//X線OFF日時
		public string pLastOff
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleXRAYM.gXraymOffDate.ToString("yyyy-mm-dd hh:mm");
				}
			}
		}
		
		//X線正常
		public int pXStatus
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipXStatus;
				}
			}
		}
		
		//ﾌﾟﾘﾋｰﾄ
		public int pXPermitWarmup
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipXPermitWarmup;
				}
			}
		}
		
		//ｳｫｰﾑｱﾌﾟﾓｰﾄﾞ
		//   戻り値
		//   0=未完、1=中、2=完了
		public int pWarmup_Mode
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipWarmup_Mode;
				}
			}
		}
		
		//ｳｫｰﾑｱｯﾌﾟ残時間（分）
		public int pWrest_timeM
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipWrest_timeM;
				}
			}
		}
		
		//ｳｫｰﾑｱｯﾌﾟ残時間（秒）
		public int pWrest_timeS
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipWrest_timeS;
				}
			}
		}
		
		public int pXcont_Mode
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipXcont_Mode;
				}
			}
		}
		
		//X線ﾀｲﾏﾓｰﾄﾞ
		//1:ﾀｲﾏ、0:連続
		public int fXcont_Mode
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					ModuleCTRLM.ifXcont_Mode = value;
				}
			}
		}
		
		public int pXtimer
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipXtimer;
				}
			}
		}
		
		//X線ﾀｲﾏ設定時間
		public int fXtimer
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					ModuleCTRLM.ifXtimer = value;
				}
			}
		}
		
		//設定管電圧値
		public float pcnd_Volt
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipcndVolt;
				}
			}
		}
		
		//設定管電流値
		public float pcnd_Amp
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return ModuleCTRLM.ipcndAmp;
				}
			}
		}

		//2004-02-20 Shibui 浜ﾎﾄ製130kV密閉管L9181対応
		//============================================================================
		//   X線発生器ウォームアップモードステップ確認
		//============================================================================
		public int pXrayWarmupSWS
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gintSWS;
				}
			}
		}
		
		//============================================================================
		//   設定可能最大管電圧(kV)
		//============================================================================
		public int P_XrayScaleMaxkV
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayScaleMaxkV;
				}
			}
		}
		
		//============================================================================
		//   設定可能最大管電流(μA)
		//============================================================================
		public int P_XrayScaleMaxuA
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayScaleMaxuA;
				}
			}
		}
		
		//============================================================================
		//   設定可能最小管電圧(kV)
		//============================================================================
		public int P_XrayScaleMinkV
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayScaleMinkV;
				}
			}
		}
		
		//============================================================================
		//   設定可能最小管電流(μA)
		//============================================================================
		public int P_XrayScaleMinuA
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayScaleMinkV;
				}
			}
		}
		
		//============================================================================
		//   最大POWER制限が最大（W)かタゲット電流の把握
		//============================================================================
		public int P_XrayMaxPower
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return (int)modData.gudtXraySystemValue.E_XrayMaxPower;
				}
			}
		}
		
		//============================================================================
		//   フォーカスF1時の最大(W)又はターゲット電流
		//============================================================================
		public float P_XrayF1MaxPower
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayF1MaxPower;
				}
			}
		}
		
		//============================================================================
		//   フォーカスF2時の最大(W)又はターゲット電流
		//============================================================================
		public float P_XrayF2MaxPower
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayF2MaxPower;
				}
			}
		}
		
		//============================================================================
		//   フォーカスF3時の最大(W)又はターゲット電流
		//============================================================================
		public float P_XrayF3MaxPower
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayF3MaxPower;
				}
			}
		}
		
		//============================================================================
		//   フォーカスF4時の最大(W)又はターゲット電流
		//============================================================================
		public float P_XrayF4MaxPower
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayF4MaxPower;
				}
			}
		}
		
		//============================================================================
		//   ウォーミングアップパターンの時間表示有無
		//============================================================================
		public int P_XrayWarmupTime
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayWarmupTime;
				}
			}
		}
		
		//============================================================================
		//   ウォーミングアップパターン１のウォームアップ時間（分）
		//============================================================================
		public int P_XrayWarmup1Time
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayWarmup1Time;
				}
			}
		}
		
		//============================================================================
		//   ウォーミングアップパターン２のウォームアップ時間（分）
		//============================================================================
		public int P_XrayWarmup2Time
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayWarmup2Time;
				}
			}
		}
		
		//============================================================================
		//   ウォーミングアップパターン３のウォームアップ時間（分）
		//============================================================================
		public int P_XrayWarmup3Time
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayWarmup3Time;
				}
			}
		}
		
		//2004-03-23 Shibui
		//============================================================================
		//   管電圧・管電流送信後に、設定値の受信が完了した
		//============================================================================
		public int P_XrayValueDisp
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					if (modXray.gintXrayValueDisp == 1)
					{
						modXray.gintXrayValueDisp = 0;
						return 1;
					}
					else
					{
						return 0;
					}
				}
			}
		}
		
		//***********************************************************
		//   L9191装置用メソッド
		//
		//   2004-09-07 Shibui
		//***********************************************************
		//public Property Let gmXrayL9191(udtData As udtXrayM)
		//    With gudtXrayM
		//        .sngOBJ = udtData.sngOBJ
		//        .intSAV = udtData.intSAV
		//        .intOST = udtData.intOST
		//        .intOBX = udtData.intOBX
		//        .intOBY = udtData.intOBY
		//        .intADJ = udtData.intADJ
		//        .intADA = udtData.intADA
		//        .intSTP = udtData.intSTP
		//        .intRST = udtData.intRST
		//        .intWarmUp = udtData.intWarmUp
		//    End With
		//}
		
		//2004-09-27 Shibui
		//public Property Let gmsngOBJ(val As Integer)
		public float gmsngOBJ
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					modXray.gintOBJ_Set = 1;	 //追加2009/10/08(KSS)hata_L10801対応
					modXray.gudtXrayM.sngOBJ = value;
				}
			}	
		}

		public int gmintSAV
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					modXray.gudtXrayM.intSAV = value;
				}
			}
		}

		public int gmintOST
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					modXray.gudtXrayM.intOST = value;
				}
			}
		}

		public int gmintOBX
		{
			set
			{ 
				lock (cCtrlm.gLock)
				{				
					modXray.gintOBX_Set = 1;	//追加2009/10/08(KSS)hata_L10801対応
					modXray.gudtXrayM.intOBX = value;
				}
			}
		}

		public int gmintOBY
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					modXray.gintOBY_Set = 1;	//追加2009/10/08(KSS)hata_L10801対応
					modXray.gudtXrayM.intOBY = value;
				}
			}
		}

        //Rev23.10 L10711 対応 by長野 2015/10/05 ------------------->
        public int gmintCAX
        {
            set
            {
                lock (cCtrlm.gLock)
                {
                    modXray.gintCAX_Set = 1;	//追加2009/10/08(KSS)hata_L10801対応
                    modXray.gudtXrayM.intCAX = value;
                }
            }
        }

        public int gmintCAY
        {
            set
            {
                lock (cCtrlm.gLock)
                {
                    modXray.gintCAY_Set = 1;	//追加2009/10/08(KSS)hata_L10801対応
                    modXray.gudtXrayM.intCAY = value;
                }
            }
        }
        //Rev23.10 L10711 対応 by長野 2015/10/05 <-------------------

		public int gmintADJ
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					modXray.gudtXrayM.intADJ = value;
				}
			}
		}

		public int gmintADA
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					modXray.gudtXrayM.intADA = value;
				}
			}
		}

		public int gmintSTP
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					modXray.gudtXrayM.intSTP = value;
				}
			}
		}

		public int gmintRST
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					modXray.gudtXrayM.intRST = value;
				}
			}
		}

		public int gmintWarmUp
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					modXray.gudtXrayM.intWarmUp = value;
					ModuleCTRLM.ifXrayonoff_Set = modXray.gudtXrayM.intWarmUp;	//追加10/05/12(KS1)hata_L9421-02対応
				}
			}
		}
		
		//追加2009/10/08(KSS)(KSS)hata_L10801対応 ---------->
		//アライメント値の保存
		public int gmintAlignmentSAV
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					modXray.gudtXrayM.intADJ_SAV = value;
				}
			}
		}

		//デフォルト値読み出し
		public int gmintDDL
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					modXray.gudtXrayM.intDDL = value;
				}
			}
		}

		//上限管電圧の制限   //v15.10追加 byやまおか 2009/11/12
		public int gmintCMV
		{
			set
			{
				lock (cCtrlm.gLock)
				{				
					modXray.gintCMV_Set = 1;
					modXray.gudtXrayM.intCMV = value;
				}
			}
		}
		//追加2009/10/08(KSS)(KSS)hata_L10801対応 ----------<

        //Rev23.10 L10711 対応 フィラメントモード設定 by長野 2015/10/05
        public int gmintMDE
        {
            set
            {
                lock (cCtrlm.gLock)
                {
                    modXray.gintMDE_Set = 1;
                    modXray.gudtXrayM.intMDE = value;
                }
            }
        }

		//***********************************************************
		//   L9191/L10801装置用プロパティ
		//
		//   2004-09-07 Shibui
		//名称変更2009/10/08(KSS)hata_L10801対応
		//***********************************************************
		//public Property Get gpXrayL9191() As udtXrayP
		//    With gpXrayL9191
		public udtXrayP gpudtXrayStatus
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					udtXrayP udtXrayP = new udtXrayP()
					{
						intSAD = modXray.gudtXrayP.intSAD,
						intSAT = modXray.gudtXrayP.intSAT,
						intSER = modXray.gudtXrayP.intSER,
						intSOV = modXray.gudtXrayP.intSOV,
						intTargetInf = modData.gudtXraySystemValue.E_XrayTargetInfSTG,
						intTargetLimit = modXray.gudtXrayP.intTargetLimit,
						intVacuumInf = modData.gudtXraySystemValue.E_XrayVacuumInf,
						intSBX = modXray.gudtXrayP.intSBX,
						intSBY = modXray.gudtXrayP.intSBY,
						intSHM = modXray.gudtXrayP.intSHM,
						sngSHS = modXray.gudtXrayP.sngSHS,
						
						//変更2009/08/25(KSS)hata_L10801対応 ---------->
						//        .intSHT = gudtXrayP.intSHT
						lngSHT = modXray.gudtXrayP.lngSHT,
						//変更2009/08/25(KSS)hata_L10801対応 ----------<
						sngSOB = modXray.gudtXrayP.sngSOB,
						
						lngSTM = modXray.gudtXrayP.lngSTM,
						sngSVV = modXray.gudtXrayP.sngSVV,
						lngSXT = modXray.gudtXrayP.lngSXT,
						sngTargetInfSTG = modXray.gudtXrayP.sngTargetInfSTG,
						strVacuumSVC = modXray.gudtXrayP.strVacuumSVC,
						
						//追加2009/08/25(KSS)hata_L10801対応 ---------->
						intFLM = modXray.gudtXrayP.intFLM,
						strTYP = modXray.gudtXrayP.strTYP,
						intZT1 = modXray.gudtXrayP.intZT1,
						intSSA = modXray.gudtXrayP.intSSA,
						intSWE = modXray.gudtXrayP.intSWE,
						strSWU = modXray.gudtXrayP.strSWU,
						intSMV = modXray.gudtXrayP.intSMV, //v15.10追加 byやまおか 2009/11/12
						//追加2009/08/25(KSS)hata_L10801対応 ----------<
						
						//追加2011/09/15(KS1)hata_L10801X線の電力制限に対応 ----------->
						intWattageLimit = modData.gudtXraySystemValue.E_XrayWattageLimit,
						//追加2011/09/15(KS1)hata_L10801X線の電力制限に対応 -----------<

                        //Rev23.10 L10711 対応 by長野 2015/10/05 --------------->
                        intSCX = modXray.gudtXrayP.intSCX,
                        intSCY = modXray.gudtXrayP.intSCY,
                        intSMD = modXray.gudtXrayP.intSMD,
                        sngSVS = modXray.gudtXrayP.sngSVS,
                        sngCOB = modXray.gudtXrayP.sngCOB,
                        //Rev23.10 L10711 対応 by長野 2015/10/05 ---------------<
					};

					return udtXrayP;
				}
			}
		}
		
		//***********************************************************
		//   ターゲット電流ステータスの有無
		//***********************************************************
		public int gpXrayTargetInfSTG
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayTargetInfSTG;
				}
			}
		}
		
		//***********************************************************
		//   真空度情報の有無
		//***********************************************************
		public int gpXrayVacuumInf
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayVacuumInf;
				}
			}
		}
		
		//***********************************************************
		//   X線焦点切り替えモード
		//***********************************************************
		public int gpXrayFocusNumber
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayFocusNumber;
				}
			}
		}
		
		//***********************************************************
		//   アベイラブル管電圧範囲
		//***********************************************************
		public int gpXrayAvailkV
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayAvailkV;
				}
			}
		}
		
		//***********************************************************
		//   アベイラブル管電流範囲
		//***********************************************************
		public int gpXrayAvailuA
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayAvailuA;
				}
			}
		}
		
		//***********************************************************
		//   X線ON中に設定値を変更した場合のアベイラブル時間
		//***********************************************************
		public int gpXrayAvailTimeInside
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayAvailTimeInside;
				}
			}
		}
		
		//***********************************************************
		//   X線ON中OFF→ON時のアベイラブル時間
		//***********************************************************
		public int gpXrayAvailTimeOn
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayAvailTimeOn;
				}
			}
		}
		
		//追加2010/02/22(KSS)hata_L8601対応
		//***********************************************************
		//ｵﾍﾟﾚｰﾄｽｲｯﾁ状態確認(0：OFF、1:REMOTE、2:LOCAL)
		//L8601用
		//***********************************************************
		public int pXrayOperateSRL
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modXray.ipXrayOperateSRL;
				}
			}
		}
		
		//追加2010/02/22(KSS)hata_L8601対応
		//***********************************************************
		//ﾘﾓｰﾄ動作状態確認(0:BUSY、1:READY)
		//L8601用
		//***********************************************************
		public int pXrayRemoteSRB
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modXray.ipXrayRemoteSRB;
				}
			}
		}
		
		//追加2010/05/14(KS1)hata_L9421-02対応
		//***********************************************************
		//バッテリー状態確認(0:正常、1:Low)
		//L9421-02/L9181-02用
		//***********************************************************
		public int pXrayBatterySBT
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modXray.ipXrayBatterySBT;
				}
			}
		}
		
		//追加2011/09/15(KS1)hata_X線電力制限に対応
		//***********************************************************
		//X線の電力制限
		//L10811用
		//***********************************************************
		public int gpXrayWattageLimit
		{
			get
			{
				lock (cCtrlm.gLock)
				{				
					return modData.gudtXraySystemValue.E_XrayWattageLimit;
				}
			}
		}
		
		//ｳｫｰﾑｱｯﾌﾟｷｬﾝｾﾙ
		public int fWarmupCancel(int val)
		{
			lock (cCtrlm.gLock)
			{				
				ModuleCTRLM.ifWarmup_Cancel = val;
				return 0;
			}
		}
		
		//ｳｫｰﾑｱｯﾌﾟ強制終了
		public int fWarmupReset(int val)
		{
			lock (cCtrlm.gLock)
			{				
				ModuleCTRLM.ifWarmup_Reset = val;
				return 0;
			}
		}

		//追加2010/05/20(KS1)hata_L9421-02対応
		private void InitMethodData()
		{
			lock (cCtrlm.gLock)
			{				
				//初期化
				ModuleCTRLM.ifXrayonoff_Set = 0;		//Xon
				ModuleCTRLM.ifWarmup_Cancel = 0;		//WarmUpcancel
				ModuleCTRLM.ipErrsts = 0;				//ErrNo
				ModuleCTRLM.ifErrrst = 0;				//ErrNo
				ModuleCTRLM.ifX_Volt_Up = 0;			//管電圧UP
				ModuleCTRLM.ifX_Volt_Down = 0;			//管電圧Down
				ModuleCTRLM.ifX_Amp_Up = 0;				//管電流UP
				ModuleCTRLM.ifX_Amp_Down = 0;			//管電流Down
				modXray.gintOBJ_Set = 0;
				modXray.gintOBX_Set = 0;
				modXray.gintOBY_Set = 0;
				modXray.gintCMV_Set = 0;
				modXray.gudtXrayM.intSAV = 0;
				modXray.gudtXrayM.intOST = 0;
				modXray.gudtXrayM.intADJ = 0;
				modXray.gudtXrayM.intADA = 0;
				modXray.gudtXrayM.intSTP = 0;
				modXray.gudtXrayM.intRST = 0;
				modXray.gudtXrayM.intADJ_SAV = 0;
				modXray.gudtXrayM.intDDL = 0;
				modXray.gudtXrayM.intWarmUp = 0;		//WarmUp
                modXray.gudtXrayM.intCAX = 0;           //Rev23.10 L10711 対応 by長野 2015/10/05
                modXray.gudtXrayM.intCAY = 0;           //Rev23.10 L10711 対応 by長野 2015/10/05
                modXray.gudtXrayM.intMDE = 0;           //Rev23.10 L10711 対応 by長野 2015/10/05
			}
		}	
	}
}
