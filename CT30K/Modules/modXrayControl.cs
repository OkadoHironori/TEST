using System;
using System.Windows.Forms;
using System.Threading;
using XrayCtrl;
//
using CTAPI;
using CT30K.Common;
using CT30K.Properties;

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： modXrayControl.bas                                          */
    ///* 処理概要　　： Ｘ線関連モジュール                                          */
    ///* 注意事項　　： なし                                                        */
    ///* -------------------------------------------------------------------------- */
    ///* 適用計算機　： DOS/V PC                                                    */
    ///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
    ///* コンパイラ　： VB 6.0                                                      */
    ///* -------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
    ///*                                                                            */
    ///* V15.0      09/04/07    (SI1)間々田          新規作成                       */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2009                 */
    ///* ************************************************************************** */
	public static class modXrayControl
	{
        //Ｘ線のタイプ
		public enum XrayTypeConstants
		{
			XrayTypeFeinFocus,			//0
			XrayTypeKevex,			    //1
			XrayTypeHamaL9181,			//2  浜ホト 130kV L9181用
			XrayTypeHamaL9191,			//3  浜ホト 160kV L9191用    'v9.5 追加 by 間々田 2004/09/13
			XrayTypeHamaL9421,			//4  浜ホト  90kV L9421用    'v9.6 追加 by 間々田 2004/10/15
			XrayTypeToshibaEXM2_150,    //5  東芝 EXM2-150用         'v11.3追加 by 間々田 2006/02/17
			XrayTypeViscom,			    //6  Viscom XT9225-DED-IT用  'v11.5追加 by 間々田 2006/04/04
			XrayTypeHamaL10801,			//7  浜ホト 230kV L10801用   'v15.10追加 byやまおか 2009/10/06
			XrayTypeHamaL8601,			//8  浜ホト  90kV L8601用    'v16.03/v16.20追加 byやまおか 2010/03/03
			XrayTypeHamaL9421_02T,		//9  浜ホト  90kV L9421-02T用'v16.30追加 byやまおか 2010/05/21
			//GE,			            //10 GESIT  HP管450kV         '17.71 追加 by長野 2012/03/14 将来450kV+FPDのCTソフト(Ver18.00)との統合を考慮し，構造体に追加（実際の処理は追加しない）
            XrayTypeGeTitan,            //GEからXrayTypeGeTitanに変更2014/10/07hata_v19.51反映
            XrayTypeHamaL8121_02,		//11 浜ホト 150kV L8121-02用 'v17.71 追加 by長野 2012/03/14 将来450kV+FPDのCTソフト(Ver18.00)との統合を考慮し，11とする
            XrayTypeHamaL9181_02,		//12 浜ホト 130kV L9181-02用 //将来用に追加する。
            XrayTypeHamaL12721,         //13 浜ホト 300kV L12721用   //Rev23.10 追加 by長野 2015/10/01
            XrayTypeHamaL10711,          //14 浜ホト 160kV L10711用   //Rev23.10 追加 by長野 2015/10/01
            XrayTypeSpellman            //15 Spellman 450kV用        //Rev25.03/Rev25.02 add by chouno 2017/02/05
        }

		public static XrayTypeConstants XrayType;

        //Ｘ線のウォームアップ                                   'v11.3追加 by 間々田 2006/02/17
		public enum XrayWarmUpConstants
		{
			XrayWarmUpNotComplete,	//0  ウォームアップ未完
			XrayWarmUpNow,			//1  ウォームアップ中
			XrayWarmUpComplete,		//2  ウォームアップ完了
			XrayWarmUpFailed,		//3  ウォームアップ失敗      'v11.5追加 by 間々田 2006/04/04
		}

//#if DebugOn     //デバッグ時は仮想Ｘ線制御とする by 間々田 2004/11/29
//Rev23.10 変更 by長野 23.10 by長野
#if XrayDebugOn
        public static frmVirtualXrayControl XrayControl;
#else
		public static XrayCtrl.clsTActiveX XrayControl;     //X線制御            'V9.5 追加 by 間々田 2004/09/17 modCT30Kから移動
#endif
		
		public static string CurrentUni;                //管電流の単位       'v11.3追加 by 間々田 2006/02/17
		public static int XrayToolTimerCount;			
		public static float BackXrayVoltSet;            //設定管電圧のバックアップ                                           'v11.4追加 by 間々田 2006/03/06
		public static bool FilamentAdjustAfterWarmup;   //ウォームアップ後フィラメント調整を開始する場合のフラグ(Viscom専用) 'v11.5追加 by 間々田 2006/05/08
		public static bool XrayWarmUpCanceled;          //Ｘ線ウォームアップ中断フラグ(Viscom/L10801専用)　                  'v11.5追加 by 間々田 2006/05/08
		public static int XrayVoltAtWarmingup;          //Ｘ線ウォームアップ開始時の設定管電圧(Viscom/L10801専用)            'v11.5追加 by 間々田 2006/05/08
		public static bool XrayCenteringManual;         //Ｘ線手動センタリングフラグ(Viscom専用)                             'v11.5追加 by 間々田 2006/05/08
		public static int XrayMaxFeedBackVolt;
		public static bool ViscomFocusChanged;
		public static bool ViscomCenteringChanged;
		public static bool WarmupStartAuto;
		public static bool FilamentAdjustAuto;
		public static bool IsWarmupAvailable;           //ウォームアップ開始が有効か？　'追加 2009/08/01
        public static int WUP_No;                       //ウォームアップの種類(浜ホトL10801用 1:WUP,2:WUP1,3:WUP2)   'v16.01/v17.00追加 byやまおか 2010/02/18
		public static bool WUP_Start;                   //ウォームアップ開始しますか？はい・いいえ(浜ホトL10801用)   'v16.01/v17.00追加 byやまおか 2010/03/02

        //追加2014/10/07hata_v19.51反映 
        public static int XrayHamaTimerCount;           //X線照射時間(sec) 'v19.50 by長野 2013/11/26       
        public static bool XrayHamaTimerEndFlg;         //浜ホト版X線タイマー終了フラグ(このフラグがtrueにならずにX線ＯＦＦしたら、X線異常でタイマー停止したことを示す)'v19.50 by長野 2013/11/26
        public static bool TiOpenFlg;                   //TitanのX線制御器がオープンしているかどうか　by長野 2014/01/31


        //アベイラブル待ち時間(秒)
		private const int GC_X_AVAIL_WAIT_TIME = 60;

        //Ｘ線制御器に設定中		
		private static bool myBusy;	    //v15.0追加 by 間々田 2009/05/12

#region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //VISCOM 設定管電流15μA時 Ｘ線オン後何秒でアベイラブルとするかの判定値（秒）追加 by 間々田 2009/08/17
        //Public Const XrayOnElapsedTime As Single = 10
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

        //Ｘ線をオンした時刻                     追加 by 間々田 2009/08/17
		public static DateTime TimeAtXrayOn;

        ////段階ウォームアップ用変数   'v17.72/v19.02追加 byやまおか 2012/05/16			
        //public static int STEPWU_NUM;       //ステップ数			
        //public static int[] STEPWU_KV = new int[6];     //各段階の管電圧(5段階を想定、(0)は使わない)

        //追加2014/11/26hata
        //private static float TmpSetVolt = -1;      //SetVoltの記憶用
        //private static float TmpSetCurrent = -1;   //SetCurrentの記憶用
        //Rev20.01 変更 by長野
        private static float myTempSetVolt = -1;
        public static float TempSetVolt
        {
            get
            {
                return myTempSetVolt;
            }
            set
            {
                myTempSetVolt = value;
            }
        }
        private static float myTempSetCurrent = -1;
        public static float TempSetCurrent
        {
            get
            {
                return myTempSetCurrent;
            }
            set
            {
                myTempSetCurrent = value;
            }
        }

        //********************************************************************************
        //機    能  ：  CT30K立ち上げ時の処理
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  Ｘ線外部制御化に伴う一連の処理を実行
        //
        //履    歴  ：  V3.0   00/08/01  (SI1)鈴山       新規作成
        //              V4.0   01/01/30  (SI1)鈴山       Private ステートメントを追加
        //              V4.0   01/03/26  (SI1)鈴山       frmCTMenuから移し、Public関数化
        //              V6.0   02/08/19  (SI4)間々田     使用しているＸ線制御器のタイプに応じてEventValue_Setの引数を設定
        //********************************************************************************
        //Public Sub Call_Feinfocus_Start(Optional ByVal WarmUpMessageOn As Boolean = True) //Functionに変更 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
	    //ublic static void XrayControlStart(bool WarmUpMessageOn = true)//変更2014/10/07hata_v19.51反映 
        public static bool XrayControlStart(bool WarmUpMessageOn = true)
        {
            bool functionReturnValue = false;            //v18.00追加 byやまおか 2011/06/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            int ret = 0;
            
            myBusy = false;			//v15.0追加 by 間々田 2009/05/12


			//処理開始メソッド
			switch (XrayType)
            {
                case XrayTypeConstants.XrayTypeFeinFocus:
                    XrayControl.EventValue_Set(1);
                    WaitXrayReady();                    //Up_XR_Status プロパティが０以外に変化するまで待つ
                    break;

                case XrayTypeConstants.XrayTypeKevex:
                    XrayControl.EventValue_Set(3);
                    WaitXrayReady();                    //Up_XR_Status プロパティが０以外に変化するまで待つ
                    break;

                case XrayTypeConstants.XrayTypeHamaL9181:
                    //v9.0追加 by 間々田 2004/02/03 浜ホト130kV追加
                    XrayControl.EventValue_Set(5);
                    //V9.6変更 by 間々田 2004/10/15 4 → 5
                    WaitXrayReady();                    //Up_XR_Status プロパティが０以外に変化するまで待つ
                    break;

                case XrayTypeConstants.XrayTypeHamaL9191:
                    //v9.5追加 by 間々田 2004/09/16 浜ホト160kV追加
                    XrayControl.EventValue_Set(6);
                    WaitXrayReady();                    //Up_XR_Status プロパティが０以外に変化するまで待つ
                    break;

                case XrayTypeConstants.XrayTypeHamaL9421:
                    //v9.6追加 by 間々田 2004/10/15 浜ホト90kV追加
                    XrayControl.EventValue_Set(4);
                    WaitXrayReady();                    //Up_XR_Status プロパティが０以外に変化するまで待つ
                    break;

                case XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    //v11.3追加 by 間々田 2006/02/20
                    break;                              //XrayControl.EventValue_Set 0   'v11.5変更 by 間々田 2006/04/10 何もしないことにした


#region     //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //case XrayTypeConstants.XrayTypeViscom:
                //    //v11.5追加 by 間々田 2006/04/10
                //    WarmupStartAuto = true;
                //    FilamentAdjustAuto = true;
                //    ViscomFocusChanged = false;
                //    ViscomCenteringChanged = false;

                //    modViscom.StartViscom();                    //Ｘ線ドライバ動作開始
                //    WaitUntilViscomDrvReady(10);                    //ウエイト0.01秒

                //    SetVolt(CTSettings.scansel.Data.scan_kv);
                 
                //    modCT30K.PauseForDoEvents(1);   //v19.19 '少し待つ by 長野 2013/09/23

                //    SetCurrent(CTSettings.scansel.Data.scan_ma);
                //    //設定管電圧、管電流のセット（そうしないと毎回ゼロになる）'追加 by 間々田 2006/04/21
                //    //GetDrvErrors ViscomError                    'エラーステータス取得
                //    //GetDrvState ViscomState1, ViscomState2      'その他ステータス取得
                //    //GetNextWarmupTime gCondTime, gFilamentTime  'v12.01追加 by 間々田 2006/12/12 次のウォームアップ/フィラメント調整までの時間

                //    //v12.01変更 by 間々田 2006/12/13
                //    modViscom.GetViscom();

                //    if (modCT30K.RequestExit)
                //        return functionReturnValue;
                //    break;                
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

                case XrayTypeConstants.XrayTypeHamaL10801:  //v15.10追加 byやまおか 2009/10/13
					XrayControl.EventValue_Set(7);
					WaitXrayReady();    //Up_XR_Status プロパティが０以外に変化するまで待つ
					break;

                case XrayTypeConstants.XrayTypeHamaL8601:
                    //v16.03/v16.20追加 byやまおか 2010/03/03
                    XrayControl.EventValue_Set(8);
                    WaitXrayReady();                    //Up_XR_Status プロパティが０以外に変化するまで待つ
                    break;

                case XrayTypeConstants.XrayTypeHamaL9421_02T:   //v16.30追加 byやまおか 2010/05/21
					XrayControl.EventValue_Set(9);
					WaitXrayReady();    //Up_XR_Status プロパティが０以外に変化するまで待つ
					break;

				case XrayTypeConstants.XrayTypeHamaL8121_02:    //v17.71 追加 by長野 2012/03/14
					XrayControl.EventValue_Set(11);
					WaitXrayReady();    //Up_XR_Status プロパティが０以外に変化するまで待つ
					break;

                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    // --- 10 ---
                    //v18.00追加 byやまおか 2011/03/14 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    //If (Ti_Open <> 0) Then Exit Sub
                    //v19.50 すでにオープン済みなら何もしない 長野 2014/01/31
#if XrayDebuOn                
 
#else
                    if ((TiOpenFlg == false))
                    {
                        //v18.00変更 byやまおか 2011/07/01
                        ret = modTitan.Ti_Open();
                        modCT30K.PauseForDoEvents(3);
                        if ((ret != 0))
                        {
                            modCT30K.RequestExit = true;
                            return functionReturnValue;
                        }
                        TiOpenFlg = true;           //v19.50 追加 by長野 2014/01/31
                        if (IsXrayOn()) XrayOff();  //X線がONしていたらOFFする

                        //Rev23.20 2世代・3世代共通の場合は、シーケンサから2世代のX線OFF時刻・管電圧を取得する by長野 2016/01/19
                        if (CTSettings.scaninh.Data.ct_gene2and3 == 0)
                        {
                            int XOffHTimeY = 0;
                            int XOffHTimeMON = 0;
                            int XOffHTimeD = 0;
                            int XOffHTimeH = 0;
                            int XOffHTimeMIN = 0;

                            int XOffMTimeY = 0;
                            int XOffMTimeMON = 0;
                            int XOffMTimeD = 0;
                            int XOffMTimeH = 0;
                            int XOffMTimeMIN = 0;

                            int cnt = 0;
                            while (modSeqComm.MySeq.stsXrayHOffTimeY == 0 && cnt < 30)
                            {
                                cnt++;
                                modCT30K.PauseForDoEvents(1);
                            }
                            if (modSeqComm.MySeq.stsXrayHOffTimeY == 0)
                            {
                                ret = -1;//シーケンサから値が貰えなかったとして、エラー扱いにする。
                                return functionReturnValue;
                            }
                            else
                            {
                                string tmpString1;

                                //H
                                //年
                                XOffHTimeY = Convert.ToInt32(modSeqComm.MySeq.stsXrayHOffTimeY.ToString("x4"));

                                //月
                                tmpString1 = modSeqComm.MySeq.stsXrayHOffTimeMD.ToString("x4");
                                XOffHTimeMON = Convert.ToInt32(tmpString1.Substring(0, 2));
                                //日                           
                                XOffHTimeD = Convert.ToInt32(tmpString1.Substring(2, 2));
                                
                                //時間
                                tmpString1 = modSeqComm.MySeq.stsXrayHOffTimeHM.ToString("x4");
                                XOffHTimeH = Convert.ToInt32(tmpString1.Substring(0, 2));
                                //分
                                XOffHTimeMIN = Convert.ToInt32(tmpString1.Substring(2, 2));
                                
                                //M
                                //年
                                XOffMTimeY = Convert.ToInt32(modSeqComm.MySeq.stsXrayMOffTimeY.ToString("x4"));
                                //月
                                tmpString1 = modSeqComm.MySeq.stsXrayMOffTimeMD.ToString("x4");
                                XOffHTimeMON = Convert.ToInt32(tmpString1.Substring(0, 2));
                                //日                           
                                XOffHTimeD = Convert.ToInt32(tmpString1.Substring(2, 2));

                                //時間
                                tmpString1 = modSeqComm.MySeq.stsXrayMOffTimeHM.ToString("x4");
                                XOffHTimeH = Convert.ToInt32(tmpString1.Substring(0, 2));
                                //分
                                XOffHTimeMIN = Convert.ToInt32(tmpString1.Substring(2, 2));

                                //この内容をX線OFF時間管理ファイルに書き込む
                                ret = modTitan.Ti_SetXOffTime(430, XOffHTimeY, XOffHTimeMON, XOffHTimeD, XOffHTimeH, XOffHTimeMIN);
                                
                                modCT30K.PauseForDoEvents(0.1f);

                                ret = modTitan.Ti_SetXOffTime(400, XOffMTimeY, XOffMTimeMON, XOffMTimeD, XOffMTimeH, XOffMTimeMIN);

                                modCT30K.PauseForDoEvents(0.1f);

                                if (ret != 0)
                                {
                                    ret = -1;
                                    return functionReturnValue;
                                }
                                
                            }

                        }
                    }

#endif
                    break;

                case XrayTypeConstants.XrayTypeHamaL9181_02:   //追加2014/10/07hata
                    // --- 12 ---
                    XrayControl.EventValue_Set(12);
                    WaitXrayReady();    //Up_XR_Status プロパティが０以外に変化するまで待つ
                    break;

                case XrayTypeConstants.XrayTypeHamaL12721:   //Rev23.10 追加 by長野 2015/10/01
                    // --- 13 ---
                    XrayControl.EventValue_Set(13);
                    WaitXrayReady();    //Up_XR_Status プロパティが０以外に変化するまで待つ
                    break;

                case XrayTypeConstants.XrayTypeHamaL10711:   //Rev23.10 追加 by長野 2015/10/01
                    // --- 14 ---
                    XrayControl.EventValue_Set(14);
                    WaitXrayReady();    //Up_XR_Status プロパティが０以外に変化するまで待つ
                    break;

            }

			//'ウォームアップ実施要求のメッセージボックス表示
			//If WarmUpMessageOn Then    '追加 by 山本 2002-3-21
			//
			//    Select Case XrayType
			//
			//        'Ｘ線のタイプがFEINFOCUSの場合
			//        Case XrayTypeFeinFocus
			//
			//            'メッセージ表示：Ｘ線制御器にてウォームアップを実施してください。
			//            MsgBox LoadResString(9378), vbInformation
			//
			//        'Ｘ線のタイプがKEVEX、浜ホトの場合
			//        Case Else
			//
			//            'ウォームアップが未完了の時メッセージを表示
			//            'If .Up_Wrest_Mode <> -1 Then
			//            If XrayWarmUp <> XrayWarmUpComplete Then                'v11.3変更 by 間々田 2006/02/20
			//                'メッセージ表示：メカ準備画面からウォームアップを実施してください。
			//                MsgBox LoadResString(9443), vbInformation
			//            End If
			//
			//    End Select
			//
            //End If

            //Ｘ線のタイプがFEINFOCUSの場合、ウォームアップ強制完了  'V6.0 append by 間々田 2002/08/30
            if (XrayType == XrayTypeConstants.XrayTypeFeinFocus)
            {
                XrayControl.WarmUpQuit_Set(1);
                WaitXrayReadyAll();
                //v9.7追加 by 間々田 2004/12/09
            }

            //状態要求メソッド
            switch (XrayType)
            {
                case XrayTypeConstants.XrayTypeToshibaEXM2_150:
                case XrayTypeConstants.XrayTypeViscom:
                    //何もしない
                    break;
                case XrayTypeConstants.XrayTypeGeTitan: //追加2014/10/07hata_v19.51反映
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    //何もしない 'v18.00追加 byやまおか 2011/03/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    break;

                default:
                    XrayControl.X_AllEventRaise_Set(1);
                    break;
            }

            functionReturnValue = true;
            return functionReturnValue;
		}


        //********************************************************************************
        //機    能  ：  CT30K終了時の処理
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  Ｘ線外部制御化に伴う一連の処理を実行
        //
        //履    歴  ：  V3.0   00/08/01  (SI1)鈴山       新規作成
        //              V4.0   01/01/30  (SI1)鈴山       Private ステートメントを追加
        //              V4.0   01/03/26  (SI1)鈴山       frmCTMenuから移し、Public関数化
        //               v11.3 06/02/20 (SI3)間々田      関数名変更。東芝 EXM2-150の場合何もしない
        //********************************************************************************
        //Public Sub Call_Feinfocus_Stop()
		public static void XrayControlStop()
        {
            #region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            /*
            string strExePath = null;
            */
            #endregion
            
            //ここで生じるエラーは無視
            try
            {
			    //string strExePath = null;

			    switch (XrayType)
                {
                    case XrayTypeConstants.XrayTypeToshibaEXM2_150:
                        //東芝 EXM2-150の場合何もしない
                        break;

#region         //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                    //case XrayTypeConstants.XrayTypeViscom:
                    //
                    //    //パスの設定　'added by 山本　2006-9-9
                    //    strExePath = "c:\\viscom\\project\\xrc\\bin\\";
                    //    modViscom.SetCurrentDirectory(strExePath);
                    //
                    //    //Viscom Ｘ線ドライバ停止
                    //    modViscom.DeleteDrv();
                    //
                    //    //確実にドライバを停止させるため待つ：タイムアウトは10秒
                    //    modCT30K.WaitWhileAlive("C:\\Viscom\\Project\\XrC\\Bin\\si_xray_drv.exe", 10000);
                    //    break;
                    //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion 

                    case XrayTypeConstants.XrayTypeGeTitan: //追加2014/10/07hata_v19.51反映
                    case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                        //v18.00追加 byやまおか 2011/03/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        //v18.00追加 byやまおか 2011/06/29
                        if (modCT30K.RequestExit)
                        {
                            modTitan.Ti_ComClose();
                            modCT30K.PauseForDoEvents(2);
                            return;
                        }
                        modTitan.Ti_Close();
                        modCT30K.PauseForDoEvents(2);
                        break;

                    default:
					    XrayControl.EventValue_Set(2);  //イベント処理終了メソッド
					    break;
			    }
            }
            catch
            {                
                // Nothing
            }
        }


        //*******************************************************************************
        //機　　能： 管電圧をＸ線制御器にセットする
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.5  2006/04/04  (WEB)間々田  Viscom対応
        //           v15.00 2008/12/01  (SS1)間々田  リニューアル
        //*******************************************************************************
		//public static void SetVolt(float theValue)
		public static void SetVolt(float theValue,bool forced = false)//Rev23.20 前回入力値と一緒でも設定するフラグを追加 by長野 2016/01/22
        {
            //追加2014/11/26hata
            //同じ場合は抜ける
            //if (TmpSetVolt == theValue) return;
            //TmpSetVolt = theValue;
            //Rev20.01 変更 by長野 2015/06/03
            //if (myTempSetVolt == theValue) return;
            //Rev23.20 条件変更 by長野 2016/01/21
            if (myTempSetVolt == theValue && forced == false) return;

            myTempSetVolt = theValue;

            int sts = 0;

            //設定中フラグセット                             'v15.0追加 by 間々田 2009/05/12
			myBusy = true;

            clsTActiveX.XrayValueSet Val1 = new clsTActiveX.XrayValueSet();     //v16.20変更 byやまおか 2010/04/21
			
			switch (XrayType) 
            {
                //東芝 EXM2-150用
                case XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    //modSeqComm.SeqWordWrite("EXMTVSet", Microsoft.VisualBasic.Compatibility.VB6.Support.Format(theValue, "0"));
                    modSeqComm.SeqWordWrite("EXMTVSet", theValue.ToString("0"));
                    break;

 #region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                ////Viscom製用
                //case XrayTypeConstants.XrayTypeViscom:
                //    //SetTargetU theValue
                //    modViscom.SetTargetU(Convert.ToInt32(Microsoft.VisualBasic.Compatibility.VB6.Support.Format(theValue, "0")));
                //    //v15.01変更 by 間々田 2009/09/02
                //    break;
                ////WaitUntilViscomDrvReady 2000           'v15.0削除 by 間々田 2009/04/23
#endregion

                //Titan用 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    //v18.00追加 byやまおか 2011/03/02
                    //管電圧設定(ウォームアップチェックあり)
                    sts = modTitan.Ti_SetXrayVoltage(Convert.ToInt32(theValue));

                    //ウォームアップ完了ならここで設定される
                    //ウォームアップ未完の場合
                    //If (sts <> WU_READY) Then   '※返るのはWU_READYではない
                    //v18.00変更 byやまおか 2011/03/26
                    if ((sts != 0))
                    {
                        //Rev23.20 Doevent追加
                        modCT30K.PauseForDoEvents(0.1f);
                        //X線OFF状態なら
                        if ((!IsXrayOn()))
                        {
                            //強制的に管電圧設定
                            modTitan.Ti_SetXrayVoltage_UnChk(Convert.ToInt32(theValue));
                            //frmXrayControl.Updateへ移動    'v18.00変更 byやまおか 2011/03/26
                            //'ウォームアップ時間を計算
                            //Ti_GetWarmupRestSec (CLng(theValue))    'v18.00追加 byやまおか 2011/03/10
                            //X線ON状態なら設定せずに終了
                        }
                        else
                        {
                            goto ExitHandler;
                        }
                    }
                    break;


				//その他
				default:
					//FeinFocus.exeのメソッドを呼び出す
					//Dim Val1    As FeinFocus.XrayValueSet
					Val1.m_kVSet = theValue;
					Val1.m_mASet = XrayControl.Up_XR_CurrentSet;
					XrayControl.XrayValue_Set(Val1);

                    //v17.71 L8121_02で確実に管電流、管電圧を変更するための対策 by長野 2012-03-26
					//if ((XrayType == XrayTypeConstants.XrayTypeHamaL8121_02))
                    if ((XrayType == XrayTypeConstants.XrayTypeHamaL8121_02)) 
                    {
						Thread.Sleep(1500);
					}
					break;
            }

			//実際に設定されるまで待つ（タイムアウト５秒）   'v15.0追加 by 間々田 2009/04/23
			//Dim StartTime As Long
			//StartTime = GetTickCount()
			//Const theTimeOut As Long = 5000
			//
			//Do While XrayVoltSet() <> theValue
			//    DoEvents
			//    If GetTickCount() - StartTime > theTimeOut Then Exit Do
			//Loop

			//以下に変更 by 間々田 2009/08/24
			//Const TimeOutSec As Long = 5
			int TimeOutSec = 0;     //v15.11変更 byやまおか 2010/02/12

			//FineFocus.exeメソッドの可否でタイムアウトを決める  'v15.11追加 byやまおか 2010/02/12
			switch (XrayType)
            {

#region			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //        'FineFocus.exeメソッドでない場合は待つ
				//        'Case XrayTypeToshibaEXM2_150, XrayTypeViscom
				//        Case XrayTypeToshibaEXM2_150    'v17.701/v19.02変更 byやまおか 2012/03/28
				//            TimeOutSec = 5
				//        'VISCOMは待たない   'v17.701変更 byやまおか 2012/03/28
				//        Case XrayTypeViscom
				//            TimeOutSec = 5
				//        'FineFocusの場合は少し待つ  'v17.44追加 byやまおか 2011/02/16
				//        Case XrayTypeFeinFocus
				//            'TimeOutSec = 1
				//            TimeOutSec = 2  'v17.48/v17.53変更 byやまおか 2011/03/24
				//        'FineFocus.exeメソッドの場合は待たない
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

                default:
					TimeOutSec = 0;
					break;
			}

			//開始時間
			DateTime StartTime = default(DateTime);
            StartTime = DateTime.Now;

			bool IsTimeOut = false;
			IsTimeOut = false;

			//v16.20 下記に変更(ここから) byやまおか 2010/04/06
			//'Do While CLng(XrayVoltSet()) <> theValue
			//Do While CLng(XrayVoltSet()) <> CLng(Format$(theValue, "0")) 'v15.01変更 by 間々田 2009/09/02
			//
			//    'cwneKVを連打すると表示のタイミングで下記タイムアウトに入ってしまうため
			//    Sleep 5     '5ms        'v15.03追加  byやまおか 2009/11/17
			//
			//    If DateDiff("s", StartTime, Now) > TimeOutSec Then
			//        IsTimeOut = True
			//        Exit Do
			//    End If
			//    DoEvents
			//Loop
			//
			//FineFocus.exeメソッドでない場合
			//'If XrayType = (XrayTypeViscom Or XrayTypeToshibaEXM2_150) Then  'v15.11 if追加 byやまおか 2010/02/12
			//If (XrayType = XrayTypeViscom) Or (XrayType = XrayTypeToshibaEXM2_150) Then 'v16.03修正 byやまおか 2010/03/03
			//
			//    'タイムアウト
			//    If IsTimeOut Then
			//        If frmXrayControl.XrayStatus <> GC_Xray_Error Then
			//            'If XrayType = XrayTypeViscom Then
			//            If (XrayType = XrayTypeViscom) And (CLng(XrayVoltSet()) < CLng(Format$(theValue, "0"))) Then 'v15.01変更 by 間々田 2009/09/02
			//                'MsgBoxAsync "ウォームアップが未完了のため、指定された管電圧に設定できませんでした。"
			//                MsgBoxAsync "指定された管電圧に設定できませんでした。"      'v15.03変更 byやまおか 2009/11/17
			//            End If
			//        End If
			//    End If
            //v16.20 下記に変更(ここまで) byやまおか 2010/04/06

#region	    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //VISCOMの場合は待たない(DoEventsしない)。Windows7で反応が返ってこない対策。 'v17.701/v19.02変更 byやまおか 2012/03/28
			//    If (XrayType <> XrayTypeViscom) Then
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion
            
            //v16.20 上記を変更(ここから) byやまおか
			while (!IsTimeOut)
            {

                //v16.20追加 byやまおか 2010/04/06
                //EXM2の場合、X線ON中はフィードバックを見る
                //（ウォームアップ完了値以上に設定できてしまう。実際には上がらない。アベイラブルが来ない。）
                if ((XrayType == XrayTypeConstants.XrayTypeToshibaEXM2_150))
                {
                    //通常は設定値を見る
                    IsTimeOut = !((frmXrayControl.Instance.XrayStatus == StringTable.GC_Xray_On) & (Convert.ToInt32(XrayVoltFeedback()) != Convert.ToInt32(theValue.ToString("0"))));
                }
                else
                {
                    //IsTimeOut = !(Convert.ToInt32(XrayVoltSet()) != Convert.ToInt32(theValue.ToString("0")));
                    IsTimeOut = !((Convert.ToInt32(XrayVoltSet()) != Convert.ToInt32(theValue.ToString("0"))));                    //v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                }


				//cwneKVを連打すると表示のタイミングで下記タイムアウトに入ってしまうため
				Thread.Sleep(5);   //5ms        'v15.03追加  byやまおか 2009/11/17
                				
                if ((DateTime.Now - StartTime).TotalSeconds > TimeOutSec) 
                {
					IsTimeOut = true;
					break;
				}
                Application.DoEvents();
            }

			//FineFocus.exeメソッドでない場合
			//タイムアウト
			if (IsTimeOut) 
            {
                if (frmXrayControl.Instance.XrayStatus != StringTable.GC_Xray_Error) 
                {
					switch (XrayType) 
                    {
                        //EXM2の場合、X線ON中はフィードバックを見る
                        //（ウォームアップ完了値以上に設定できてしまう。実際には上がらない。アベイラブルが来ない。）
                        case XrayTypeConstants.XrayTypeToshibaEXM2_150:
                            if ((frmXrayControl.Instance.XrayStatus == StringTable.GC_Xray_On) & (Convert.ToInt32(XrayVoltFeedback()) < Convert.ToInt32(theValue.ToString("0"))))
                            {
                                //MsgBoxAsync "指定された管電圧に設定できませんでした。"
                                //v17.60 ストリングテーブル化 by長野 2011/05/25
                                modCT30K.MsgBoxAsync(CTResources.LoadResString(20115));
                            }
                            break;

#region				    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                        ////VISCOMは設定値を見る
                        //case XrayTypeConstants.XrayTypeViscom:
                        //    if (((Convert.ToInt32(XrayVoltSet()) < Convert.ToInt32(theValue.ToString("0")))))
                        //    {
                        //        //MsgBoxAsync "指定された管電圧に設定できませんでした。"
                        //        //v17.60 ストリングテーブル化 by長野 2011/05/25
                        //        modCT30K.MsgBoxAsync(CTResources.LoadResString(20115));
                        //    }
                        //    break;
                        ////v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

						//FeinFocus.exeメソッドの場合は何もしない
						default:
							break;
					}
				}
			}
			//v16.20 上記を変更(ここまで) byやまおか

#region	    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//    End If  'v17.701/v19.02条件追加 byやまおか 2012/03/28
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion


        ExitHandler:    //v17.701/v19.02条件追加 byやまおか 2012/03/28

            //設定中フラグリセット                           'v15.0追加 by 間々田 2009/05/12
			myBusy = false;
		}


        //*******************************************************************************
        //機　　能： 管電流をＸ線制御器にセットする
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.5  2006/04/04  (WEB)間々田  Viscom対応
        //           v15.00 2008/12/01  (SS1)間々田  リニューアル
        //*******************************************************************************
		//public static void SetCurrent(float theValue)
  		public static void SetCurrent(float theValue,bool forced = false)      //Rev23.20 前回入力値と一緒でも設定するフラグを追加 by長野 2016/01/22
        {
            //追加2014/11/26hata
            //同じ場合は抜ける
            //if (TmpSetCurrent == theValue) return;
            //TmpSetCurrent = theValue;
            //Rev20.01 変更 by長野 2015/06/03
            //if (myTempSetCurrent == theValue) return;
            //Rev23.20 変更 by長野 2016/01/21
            if (myTempSetCurrent == theValue && forced == false) return;
            myTempSetCurrent = theValue;

            //設定中フラグセット                             'v15.0追加 by 間々田 2009/05/12
			myBusy = true;

            switch (XrayType)
            {
                //東芝 EXM2-150用
                case XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    //modSeqComm.SeqWordWrite("EXMTCSet", Microsoft.VisualBasic.Compatibility.VB6.Support.Format(theValue * 100, "0"));
                    modSeqComm.SeqWordWrite("EXMTCSet", (theValue * 100).ToString("0"));
                    break;

                #region			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                ////Viscom製用
                //case XrayTypeConstants.XrayTypeViscom:
                //    modViscom.SetTargetI(theValue);
                //    break;
                ////WaitUntilViscomDrvReady 5000           'v15.0追加 by 間々田 2009/04/23
                ////Sleep 100   'v17.701/v19.02追加 byやまおか 2012/03/28
                //
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
                #endregion

                //Titan用
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    //v18.00追加 byやまおか 2011/03/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    modTitan.Ti_SetXrayCurrent(Convert.ToInt16(theValue * 1000));
                    //uA単位で設定
                    break;


                //その他
                default:
                    //FeinFocus.exeのメソッドを呼び出す
                    //Dim Val1    As FeinFocus.XrayValueSet
                    clsTActiveX.XrayValueSet Val1 = new clsTActiveX.XrayValueSet();    //v16.20変更 byやまおか 2010/04/21
                    Val1.m_kVSet = XrayControl.Up_XR_VoltSet;
                    Val1.m_mASet = theValue;
                    XrayControl.XrayValue_Set(Val1);

                    //v17.71 L8121_02で確実に管電流、管電圧を変更するための対策 by長野 2012-03-26
                    if ((XrayType == XrayTypeConstants.XrayTypeHamaL8121_02))
                    {
                        Thread.Sleep(1500);
                    }

                    break;
            }

            //実際に設定されるまで待つ（タイムアウト５秒）   'v15.0追加 by 間々田 2009/04/23
            //Dim StartTime As Long
            //StartTime = GetTickCount()
            //Const theTimeOut As Long = 5000
            //
            //Do While XrayCurrentSet() <> theValue
            //    DoEvents
            //    If GetTickCount() - StartTime > theTimeOut Then Exit Do
            //Loop

            //以下に変更 by 間々田 2009/09/02
            //Const TimeOutSec As Long = 5
            int TimeOutSec = 0;			//v15.10変更 byやまおか 2009/10/19

            //FineFocus.exeメソッドの可否でタイムアウトを決める  'v15.10追加 byやまおか 2009/10/19
            switch (XrayType)
            {

                #region			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //        'FineFocus.exeメソッドでない場合は待つ
                //        'Case XrayTypeToshibaEXM2_150, XrayTypeViscom
                //        Case XrayTypeToshibaEXM2_150    'v17.701/v19.02変更 byやまおか 2012/03/28
                //            TimeOutSec = 5
                //        'VISCOMは待たない   'v17.701/v19.02変更 byやまおか 2012/03/28
                //        Case XrayTypeViscom
                //            TimeOutSec = 5
                //        'FineFocusの場合は少し待つ  'v17.44/v17.53追加 byやまおか 2011/02/16
                //        Case XrayTypeFeinFocus
                //            'TimeOutSec = 1
                //            TimeOutSec = 2  'v17.48/v17.53変更 byやまおか 2011/03/24
                //        'FineFocus.exeメソッドの場合は待たない
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
                #endregion

                default:
                    TimeOutSec = 0;
                    break;
            }

            //開始時間
            DateTime StartTime = default(DateTime);
            StartTime = DateTime.Now;

            bool IsTimeOut = false;
            IsTimeOut = false;

            int K = 0;

            //変更2014/10/07hata_v19.51反映
            //K = (XrayType == XrayTypeConstants.XrayTypeToshibaEXM2_150 ? 100 : 1);
            switch (XrayType)
            {
                //v18.00変更 byやまおか 2011/03/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case XrayTypeConstants.XrayTypeToshibaEXM2_150:
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    K = 100;
                    break;
                default:
                    K = 1;
                    break;
            }


            //VISCOMの場合は待たない(DoEventsしない)。Windows7で反応が返ってこない対策。 'v17.701/v19.02条件追加 byやまおか 2012/03/28
            if ((XrayType != XrayTypeConstants.XrayTypeViscom))
            {
                while (Convert.ToInt32(XrayCurrentSet() * K) != Convert.ToInt32((theValue * K).ToString("0")))
                {
                    if ((DateTime.Now - StartTime).TotalSeconds > TimeOutSec)
                    {
                        IsTimeOut = true;
                        break;
                    }
                    Application.DoEvents();
                }

                //タイムアウト
                if (IsTimeOut)
                {
                    if (frmXrayControl.Instance.XrayStatus != StringTable.GC_Xray_Error)
                    {
                        //特に処理はない
                    }
                }
            }   //v17.701/v19.02条件追加 byやまおか 2012/03/28

 
			//設定中フラグリセット                           'v15.0追加 by 間々田 2009/05/12
			myBusy = false;
		}


        //*******************************************************************************
        //機　　能： Ｘ線設定管電圧
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [I/O] Single
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/17  (SI3)間々田  新規作成
        //           v11.5  2006/04/04  (WEB)間々田  Viscom対応
        //*******************************************************************************
		public static float XrayVoltSet()
		{
			float functionReturnValue = 0;

			switch (XrayType)
            {

                //東芝 EXM2-150用
                case XrayTypeConstants.XrayTypeToshibaEXM2_150:

                    //SeqComm.exeのプロパティを呼び出す
                    functionReturnValue = modSeqComm.MySeq.stsEXMTVSet;
                    break;

#region		    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //Viscom製用         'v11.5追加 by 間々田 2006/04/04
                //case XrayTypeConstants.XrayTypeViscom:
                //    functionReturnValue = modViscom.GetTargetU();
                //    break;
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

                //Titan用            'v18.00追加 byやまおか 2011/03/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    //XrayVoltSet = Ti_GetSetpointVoltage / 1000
                    functionReturnValue = Convert.ToSingle(modTitan.Ti_GetTargetVoltage());
                    //v18.00変更 byやまおか 2011/03/26
                    break;

                //その他
				default:
					//FeinFocus.exeのプロパティを呼び出す
					functionReturnValue = XrayControl.Up_XR_VoltSet;
					break;
			}
			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： Ｘ線設定管電流を取得する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [I/O] Single
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/17  (SI3)間々田  新規作成
        //           v11.5  2006/04/04  (WEB)間々田  Viscom対応
        //*******************************************************************************
		public static float XrayCurrentSet()
		{
			float functionReturnValue = 0;

			switch (XrayType)
            {

                //東芝 EXM2-150用
                case XrayTypeConstants.XrayTypeToshibaEXM2_150:

                    //SeqComm.exeのプロパティを呼び出す
                    functionReturnValue = (float)modSeqComm.MySeq.stsEXMTCSet / 100f;
                    break;

#region         //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                ////Viscom製用         'v11.5追加 by 間々田 2006/04/04
                //case XrayTypeConstants.XrayTypeViscom:
                //    functionReturnValue = modViscom.GetTargetI();
                //    break;
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

                //Titan用            'v18.00追加 byやまおか 2011/03/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    //XrayCurrentSet = Ti_GetSetpointCurrent / 1000
                    functionReturnValue =modTitan.Ti_GetTargetCurrent() / 1000f;
                    //v18.00変更 byやまおか 2011/03/26
                    break;

                //その他
				default:
					//FeinFocus.exeのプロパティを呼び出す
					functionReturnValue = XrayControl.Up_XR_CurrentSet;
					break;
			}
			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： Ｘ線最大管電圧を取得する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [I/O] Single
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/17  (SI3)間々田  新規作成
        //           v11.5  2006/04/04  (WEB)間々田  Viscom対応
        //*******************************************************************************
		public static float XrayMaxVolt()
		{
			float functionReturnValue = 0;

            int theMax = 0;             //v18.00下から移動 byやまおか 2011/03/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            int theMin = 0;             //v18.00下から移動 byやまおか 2011/03/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            int theOperateMax = 0;      //v19.50 30K上で実際に出力可能な最大管電圧

			switch (XrayType)
            {

                //東芝 EXM2-150用
                case XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    //SeqComm.exeのプロパティを呼び出す
                    functionReturnValue = modSeqComm.MySeq.stsEXMMaxTV;
                    break;

#region         //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                ////Viscom製用         'v11.5追加 by 間々田 2006/04/04
                //case XrayTypeConstants.XrayTypeViscom:
                //    //Dim theMax As Long 'v18.00上へ移動 byやまおか 2011/03/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                //    //Dim theMin As Long 'v18.00上へ移動 byやまおか 2011/03/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                //    modViscom.GetMaxMinKv(ref theMax, ref theMin);
                //    functionReturnValue = theMax;
                //    break;
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

                //Titan用            'v18.00追加 byやまおか 2011/03/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    modTitan.Ti_GetMaxMinVoltage(ref theMax, ref theMin);
                    //v19.50 30Kで実際に設定可能な最大管電圧を取得する by長野 2013/12/19
                    modTitan.Ti_GetMaxOpVoltage(ref theOperateMax);
                    //XrayMaxVolt = theMax - 20   '最大値を20kV下げる
                    //v19.50 XrayVoltはtheOperateMaxを優先する。
                    if ((theMax >= theOperateMax))
                    {
                        functionReturnValue = theOperateMax;
                    }
                    else if ((theMax < theOperateMax))
                    {
                        functionReturnValue = theMax;
                    }
                    break;

				//その他
				default:
					//FeinFocus.exeのプロパティを呼び出す
					functionReturnValue = XrayControl.Up_XR_Max_kV;
					break;
			}
			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： Ｘ線最大管電流を取得する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [I/O] Single
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/17  (SI3)間々田  新規作成
        //           v11.5  2006/04/04  (WEB)間々田  Viscom対応
        //*******************************************************************************
		public static float XrayMaxCurrent()
		{
			float functionReturnValue = 0;

            int theMax = 0;            //v18.00下から移動 byやまおか 2011/03/02
            int theMin = 0;            //v18.00下から移動 byやまおか 2011/03/02

			switch (XrayType)
            {

                //東芝 EXM2-150用
                case XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    //SeqComm.exeのプロパティを呼び出す
                    functionReturnValue = modSeqComm.MySeq.stsEXMMaxTC / 100f;
                    break;

#region		    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                ////Viscom製用         'v11.5追加 by 間々田 2006/04/04
                //case XrayTypeConstants.XrayTypeViscom:
                //    //Dim theMax As Long 'v18.00上へ移動 byやまおか 2011/03/02
                //    //Dim theMin As Long 'v18.00上へ移動 byやまおか 2011/03/02
                //    modViscom.GetMaxMinUa(ref theMax, ref theMin);
                //    functionReturnValue = theMax;
                //    break;
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

                //Titan用            'v18.00追加 byやまおか 2011/03/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    modTitan.Ti_GetMaxMinCurrent(ref theMax, ref theMin);
                    functionReturnValue = theMax / 10000f;
                    break;

                //その他
				default:
					//FeinFocus.exeのプロパティを呼び出す
					functionReturnValue = XrayControl.Up_XR_Max_mA;
					break;
			}
			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： Ｘ線最小管電圧を取得する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [I/O] Single
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/17  (SI3)間々田  新規作成
        //           v11.5  2006/04/04  (WEB)間々田  Viscom対応
        //*******************************************************************************
		public static float XrayMinVolt()
		{
			float functionReturnValue = 0;

            int theMax = 0;            //v18.00下から移動 byやまおか 2011/03/02
            int theMin = 0;            //v18.00下から移動 byやまおか 2011/03/02
            
            //X線のタイプにより設定
			switch (XrayType)
            {

                case XrayTypeConstants.XrayTypeFeinFocus:
                    functionReturnValue = 0;
                    break;

                case XrayTypeConstants.XrayTypeKevex:
                    functionReturnValue = 40;
                    break;

                case XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    functionReturnValue = modSeqComm.MySeq.stsEXMMinTV;
                    break;


#region			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                ////Viscom製用         'v11.5追加 by 間々田 2006/04/04
                //case XrayTypeConstants.XrayTypeViscom:
                //    //Dim theMax As Long 'v18.00上へ移動 byやまおか 2011/03/02
                //    //Dim theMin As Long 'v18.00上へ移動 byやまおか 2011/03/02
                //    modViscom.GetMaxMinKv(ref theMax, ref theMin);
                //    functionReturnValue = theMin;
                //    break;
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

                //Titan用            'v18.00追加 byやまおか 2011/03/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    modTitan.Ti_GetMaxMinVoltage(ref theMax, ref theMin);
                    functionReturnValue = theMin;
                    break;  

                default:	//浜ホトタイプ					
					functionReturnValue = XrayControl.Up_XR_Min_kV;
					break;

            }
			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： Ｘ線最小管電流を取得する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [I/O] Single
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/17  (SI3)間々田  新規作成
        //           v11.5  2006/04/04  (WEB)間々田  Viscom対応
        //*******************************************************************************
		public static float XrayMinCurrent()
		{
			float functionReturnValue = 0;

            int theMax = 0;            //v18.00下から移動 byやまおか 2011/03/02
            int theMin = 0;            //v18.00下から移動 byやまおか 2011/03/02

			//X線のタイプにより設定
			switch (XrayType)
            {

                case XrayTypeConstants.XrayTypeFeinFocus:
                    functionReturnValue = 0;
                    break;

                case XrayTypeConstants.XrayTypeKevex:
                    functionReturnValue = 30;
                    break;

                case XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    functionReturnValue = modSeqComm.MySeq.stsEXMMinTC / 100f;
                    break;

#region         //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
				//Viscom製用         'v11.5追加 by 間々田 2006/04/04
                //case XrayTypeConstants.XrayTypeViscom:
                //    //Dim theMax As Long 'v18.00上へ移動 byやまおか 2011/03/02
                //    //Dim theMin As Long 'v18.00上へ移動 byやまおか 2011/03/02
                //    modViscom.GetMaxMinUa(ref theMax, ref theMin);
                //    functionReturnValue = theMin;
                //    break;
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion				

                //Titan用            'v18.00追加 byやまおか 2011/03/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    modTitan.Ti_GetMaxMinCurrent(ref theMax, ref theMin);
                    functionReturnValue = theMin / 1000f;
                    break;

                default:	//浜ホトタイプ					
					functionReturnValue = XrayControl.Up_XR_Min_mA;
					break;
			}
			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： Ｘ線出力管電圧
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [I/O] Single
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/17  (SI3)間々田  新規作成
        //           v11.5  2006/04/04  (WEB)間々田  Viscom対応
        //*******************************************************************************
		public static float XrayVoltFeedback()
		{
			float functionReturnValue = 0;

			switch (XrayType)
            {
                //東芝 EXM2-150用
                case XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    //SeqComm.exeのプロパティを呼び出す
                    functionReturnValue = modSeqComm.MySeq.stsEXMTV;
                    break;

#region		    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //Viscom製用         'v11.5追加 by 間々田 2006/04/04
                //case XrayTypeConstants.XrayTypeViscom:
                //    functionReturnValue = modViscom.GetFeedbackU();
                //    break;
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

                //Titan用            'v18.00追加 byやまおか 2011/03/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    //XrayVoltFeedback = Ti_GetAcutualVoltage / 1000
                    functionReturnValue = Convert.ToSingle(modTitan.Ti_GetAcutualVoltage());
                    //v18.00変更 byやまおか 2011/03/26
                    break;

                //その他
				default:
					//FeinFocus.exeのプロパティを呼び出す
					functionReturnValue = XrayControl.Up_XR_VoltFeedback;
					break;
			}
			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： Ｘ線出力管電流
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [I/O] Single
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/17  (SI3)間々田  新規作成
        //           v11.5  2006/04/04  (WEB)間々田  Viscom対応
        //*******************************************************************************
		public static float XrayCurrentFeedback()
		{
			float functionReturnValue = 0;

			switch (XrayType)
            {

                //東芝 EXM2-150用
                case XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    //SeqComm.exeのプロパティを呼び出す
                    functionReturnValue = modSeqComm.MySeq.stsEXMTC / 100f;
                    break;

#region         //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //Viscom製用         'v11.5追加 by 間々田 2006/04/04
                //case XrayTypeConstants.XrayTypeViscom:
                //    functionReturnValue = modViscom.GetFeedbackI();
                //    break;
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

                //Titan用            'v18.00追加 byやまおか 2011/03/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    functionReturnValue = modTitan.Ti_GetAcutualCurrent() / 1000f;
                    break;
				
                //その他
				default:
					//FeinFocus.exeのプロパティを呼び出す
					functionReturnValue = XrayControl.Up_XR_CurrentFeedback;
					break;
			}
			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： Ｘ線ON/OFF状態
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/17  (SI3)間々田
        //           v11.5  2006/04/04  (WEB)間々田  Viscom対応
        //*******************************************************************************
		public static bool IsXrayOn()
		{
			bool functionReturnValue = false;

			switch (XrayType) 
            {


                //東芝 EXM2-150用
                case XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    //SeqComm.exeのプロパティを呼び出す
                    functionReturnValue = modSeqComm.MySeq.stsEXMOn;
                    break;

#region			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //Viscom製用         'v11.5追加 by 間々田 2006/04/04
                //case XrayTypeConstants.XrayTypeViscom:
                //    functionReturnValue = modViscom.ViscomState2 & modViscom.XST2_XON;
                //    break;
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

                //Titan用            'v18.00追加 byやまおか 2011/03/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    functionReturnValue = Convert.ToBoolean(modTitan.Ti_CheckXrayON() == 1);
//#if DebugOn
//rev23.10 変更 by長野　2015/10/02
//Rev23.20 コメントアウト by長野 2015/12/21
//#if XrayDebugOn
//                functionReturnValue = (XrayControl.Up_X_On == 1);
//#endif
                    break;

                
                //その他
				default:
					//FeinFocus.exeのプロパティを呼び出す
					functionReturnValue = (XrayControl.Up_X_On == 1);
					break;
			}
			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： Ｘ線レディ／スタンバイ状態
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [ /O] Boolean   True:レディ状態 , False: スタンバイ状態
        //
        //補　　足： なし
        //
        //履　　歴： v11.5  2006/04/10  (SI3)間々田  新規作成
        //*******************************************************************************
		public static bool IsXrayReady()
		{
			bool functionReturnValue = false;

			switch (XrayType)
            {
                //東芝 EXM2-150用
                case XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    functionReturnValue = modSeqComm.MySeq.stsEXMReady;
                    break;

#region			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //Viscom製用
                //case XrayTypeConstants.XrayTypeViscom:
                //    functionReturnValue = modViscom.ViscomState2 & modViscom.XST2_INT_KEY;
                //    break;
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

                //Titan用            'v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    switch (modTitan.Ti_GetErrorCode())
                    {
                        //エラーなし
                        case 0:
                            functionReturnValue = true;
                            break;
                        //異常にしないエラーコード
                        case 70:
                        case 106:
                        case 109:
                        case 119:
                        case 121:
                            functionReturnValue = true;
                            break;
                        //ドアオープンのエラーコード
                        case 63:
                        case 64:
                        case 65:
                            //ドアインターロックがOKのときはエラーをクリアーする
                            //Rev25.03/Rev24.00 インターロックON、かつ、運転準備ONでリセットをかけにいく
                            if (modSeqComm.MySeq.stsDoorInterlock == false && modSeqComm.MySeq.stsRunReadySW == true)
                                modTitan.Ti_SetTitanReset();
                            functionReturnValue = false;
                            break;
                        //それ以外のエラーコード
                        default:
                            functionReturnValue = false;
                            break;
                    }
                    break;


                default:
					functionReturnValue = (XrayControl.Up_InterLock == 1);
					break;
			}
			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： Ｘ線インターロック状態
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [I/O] Boolean   True:閉, False:開
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/17  (SI3)間々田  新規作成
        //           v11.5  2006/04/04  (WEB)間々田  Viscom対応
        //*******************************************************************************
		public static bool IsXrayInterLock()
		{
			bool functionReturnValue = false;

			switch (XrayType) 
            {

                //東芝 EXM2-150用, Viscom製用
                //Case XrayTypeToshibaEXM2_150
                case XrayTypeConstants.XrayTypeToshibaEXM2_150:
                case XrayTypeConstants.XrayTypeViscom:
                    //v11.5変更 by 間々田 2006/04/04

                    //SeqComm.exeのプロパティを呼び出す
                    functionReturnValue = !modSeqComm.MySeq.stsDoorInterlock;
                    //シーケンサは True:開 False:閉
                    break;

                //Titan用            'v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    functionReturnValue = !modSeqComm.MySeq.stsDoorInterlock;
                    //シーケンサは True:開 False:閉
                    break;

                //その他
				default:
					//FeinFocus.exeのプロパティを呼び出す
					functionReturnValue = (XrayControl.Up_InterLock == 1);
					break;
			}
			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： Ｘ線エラー状態
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/17  (SI3)間々田  新規作成
        //           v11.5  2006/04/04  (WEB)間々田  Viscom対応
        //*******************************************************************************
		public static bool IsXrayError()
		{
			bool functionReturnValue = false;

			switch (XrayType) 
            {
				//浜ホトタイプ
				//Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421
				//Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801    'v15.10変更 byやまおか 2009/10/13
				//Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801, XrayTypeHamaL8601     'v16.03/v16.20追加 byやまおか 2010/03/03
				//Case XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL10801, XrayTypeHamaL8601, XrayTypeHamaL9421_02T  'v16.30追加 byやまおか 2010/05/21
				case XrayTypeConstants.XrayTypeHamaL9181:
				case XrayTypeConstants.XrayTypeHamaL9191:
				case XrayTypeConstants.XrayTypeHamaL9421:
				case XrayTypeConstants.XrayTypeHamaL10801:
				case XrayTypeConstants.XrayTypeHamaL8601:
				case XrayTypeConstants.XrayTypeHamaL9421_02T:
				case XrayTypeConstants.XrayTypeHamaL8121_02:		//v17.71 追加 by長野 2012/03/14
                case XrayTypeConstants.XrayTypeHamaL9181_02:		//追加2014/10/07hata_v19.51反映
                case XrayTypeConstants.XrayTypeHamaL12721://Rev23.10 追加 by長野 2015/10/01
                case XrayTypeConstants.XrayTypeHamaL10711://Rev23.10 追加 by長野 2015/10/01

					switch (XrayControl.Up_XR_Status) 
                    {
						case 2:
							functionReturnValue = (XrayControl.Up_InterLock == 1);
							break;
						case 7:     //過負荷
							functionReturnValue = true;
							break;
						default:
							functionReturnValue = false;
							break;
					}
					break;

                //東芝 EXM2-150用
                case XrayTypeConstants.XrayTypeToshibaEXM2_150:
                    functionReturnValue = !modSeqComm.MySeq.stsEXMNormal1;
                    break;

                //Viscom製用         'v11.5追加 by 間々田 2006/04/04
                case XrayTypeConstants.XrayTypeViscom:
                    functionReturnValue = false;
                    break;

                //Titan用            'v18.00追加 byやまおか 2011/03/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    switch (modTitan.Ti_GetErrorCode())
                    {
                        //エラーなし
                        case 0:
                            functionReturnValue = false;
                            break;
                        //異常にしないエラーコード
                        case 63:
                        case 64:
                        case 65:
                        case 70:
                        case 106:
                        case 109:
                        case 119:
                        case 121:
                            //Ti_SetTitanReset
                            functionReturnValue = false;
                            break;
                        //それ以外のエラーコード
                        default:
                            functionReturnValue = true;
                            break;
                    }
                    switch (modTitan.Ti_GetCommErrorCode())//Rev23.20 追加 by長野 通信のエラーチェック 2016/01/11
                    {
                        case 0:
                            functionReturnValue = false;
                            break;

                        default://0以外は全てエラー、エラーの内容はX線詳細で確認
                            functionReturnValue = true;
                            break;
                    }
                    break;

                //上記以外
				default:
					functionReturnValue = (XrayControl.Up_XR_Status != 1);
					break;
			}
			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： Ｘ線ウォームアップ状態
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [I/O] XrayWarmUpConstants
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/17  (SI3)間々田  新規作成
        //           v11.5  2006/04/04  (WEB)間々田  Viscom対応
        //*******************************************************************************
		public static XrayWarmUpConstants XrayWarmUp()
		{
			XrayWarmUpConstants functionReturnValue = default(XrayWarmUpConstants);

			switch (XrayType)
            {

                //東芝 EXM2-150用
                case XrayTypeConstants.XrayTypeToshibaEXM2_150:

                    //シーケンサ通信なしの場合ウォームアップ完了とみなす
                    if (modSeqComm.MySeq == null)
                    {
                        functionReturnValue = XrayWarmUpConstants.XrayWarmUpComplete;
                        //SeqComm.exeのプロパティを呼び出す
                    }
                    else if (modSeqComm.MySeq.stsEXMWU)
                    {
                        functionReturnValue = XrayWarmUpConstants.XrayWarmUpComplete;
                    }
                    else if (modSeqComm.MySeq.stsEXMOn)
                    {
                        functionReturnValue = XrayWarmUpConstants.XrayWarmUpNow;
                    }
                    else
                    {
                        functionReturnValue = XrayWarmUpConstants.XrayWarmUpNotComplete;
                    }
                    break;

#region			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                ////Viscom製用         'v11.5追加 by 間々田 2006/04/04
                //case XrayTypeConstants.XrayTypeViscom:
                //    if (modViscom.ViscomState1 & modViscom.XST1_WUP_RUNNING)
                //    {
                //        functionReturnValue = XrayWarmUpConstants.XrayWarmUpNow;            //ウォームアップ中
                //    }
                //    else if (modViscom.ViscomState1 & modViscom.XST1_WUP_FAILED)
                //    {
                //        functionReturnValue = XrayWarmUpConstants.XrayWarmUpFailed;         //ウォームアップ失敗
                //    }
                //    else if (modViscom.ViscomState1 & modViscom.XST1_WUP_READY)
                //    {
                //        functionReturnValue = XrayWarmUpConstants.XrayWarmUpComplete;       //ウォームアップ完了
                //    }
                //    else if (XrayWarmUpCanceled)
                //    {
                //        functionReturnValue = XrayWarmUpConstants.XrayWarmUpComplete;       //ウォームアップ完了 'ウォームアップが中断されている場合、ウォームアップ完了とみなす
                //    }
                //    else
                //    {
                //        functionReturnValue = XrayWarmUpConstants.XrayWarmUpNotComplete;    //ウォームアップ未完
                //    }
                //    break;
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

                //Titan用            'v18.00追加 byやまおか 2011/03/07 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05

                    if ((CTSettings.scaninh.Data.xray_remote == 0))
                    {
                        if ((modTitan.Ti_CheckXrayON() == 1) & (modTitan.Ti_CheckXrayReady() == 1) & modTitan.Flg_TiWarmingUp)
                        {
                            functionReturnValue = XrayWarmUpConstants.XrayWarmUpNow;
                            //ウォームアップ中
                            //ElseIf (Ti_CheckXrayON = 0) And (Ti_CheckWarmUpStatus(CLng(frmXrayControl.cwneKV.Value)) = WU_READY) Then
                            //v19.50 条件追加 by長野 2013/12/19 X線ONかつウォームアップ完了かつウォームアップ中じゃない場合も、ウォームアップ完了なため
                        }
                        //else if ((modTitan.Ti_CheckXrayON() == 0) & (modTitan.Ti_CheckWarmUpStatus(Convert.ToInt32(frmXrayControl.Instance.cwneKV.Value)) == modTitan.WarmupConstants.WU_READY) | ((modTitan.Ti_CheckXrayON() == 1) & (modTitan.Ti_CheckWarmUpStatus(Convert.ToInt32(frmXrayControl.Instance.cwneKV.Value)) == modTitan.WarmupConstants.WU_READY) & !modTitan.Flg_TiWarmingUp))
                        //Rev23.20 修正 入力値ではなく設定値を使用する。条件変更(ウォームアップ中フラグを優先) by長野 2016/01/13
                        //else if ((modTitan.Ti_CheckXrayON() == 0) & (modTitan.Ti_CheckWarmUpStatus(Convert.ToInt32(frmXrayControl.Instance.ntbSetVolt.Value)) == modTitan.WarmupConstants.WU_READY) | ((modTitan.Ti_CheckXrayON() == 1) & (modTitan.Ti_CheckWarmUpStatus(Convert.ToInt32(frmXrayControl.Instance.ntbSetVolt.Value)) == modTitan.WarmupConstants.WU_READY) & !modTitan.Flg_TiWarmingUp))
                        else if ((modTitan.Ti_CheckWarmUpStatus(Convert.ToInt32(frmXrayControl.Instance.ntbSetVolt.Value)) == modTitan.WarmupConstants.WU_READY) & (!modTitan.Flg_TiWarmingUp))
                        {
                            functionReturnValue = XrayWarmUpConstants.XrayWarmUpComplete;
                            //ウォームアップ完了
                        }
                        else
                        {
                            functionReturnValue = XrayWarmUpConstants.XrayWarmUpNotComplete;
                            //ウォームアップ未完
                        }
                    }
                    break;

                //その他
				default:
					if (XrayControl == null) 
                    {
						//Ｘ線外部制御なしの場合ウォームアップ完了とみなす
						functionReturnValue = XrayWarmUpConstants.XrayWarmUpComplete;
					} 
                    else 
                    {
						//FeinFocus.exeのプロパティを呼び出す
						functionReturnValue = (XrayWarmUpConstants)XrayControl.Up_Warmup;
					}
					break;
			}
			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： 浜ホト用Ｘ線ウォームアップ残時間を返す関数
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v9.0  2004/02/12  (SI4)間々田  新規作成
        //           v9.6  2004/10/15  (SI4)間々田  XrayTypeHamaL9421 を追加
        //           v16.3 2010/05/21  (検S1)やまお XrayTypeHamaL9421_02T を追加
        //*******************************************************************************
		public static int GetWrestTimeHama()
		{
			int functionReturnValue = 0;
            int offset = 0;

			switch (XrayType) 
            {
                case XrayTypeConstants.XrayTypeHamaL9181:
				case XrayTypeConstants.XrayTypeHamaL9421:
                case XrayTypeConstants.XrayTypeHamaL9421_02T:	//v16.30 02T追加 byやまおか 2010/05/21
                case XrayTypeConstants.XrayTypeHamaL9181_02:    //追加2014/11/05hata L9181-02に対応
                
                    //追加2014/06/27(検S1)hata
                    //02タイプは数値が10から始まりため、+1する
				    if ((XrayType != 0) && (XrayType == XrayTypeConstants.XrayTypeHamaL9421_02T))
                        offset = 1;

                    switch (XrayControl.Up_XrayWarmupSWS + offset) 
                    {
						case 0:
							functionReturnValue = 0;
							break;
						case 10:
						case 11:
							functionReturnValue = 15;
							break;
						case 12:
							functionReturnValue = 14;
							break;
						case 13:
							functionReturnValue = 13;
							break;
                        case 14:
						case 26:
						case 36:
							functionReturnValue = 10;
							break;
						
                        case 15:
							functionReturnValue = 7;
							break;
						case 16:
							functionReturnValue = 4;
							break;
						case 20:
						case 21:
							functionReturnValue = 40;
							break;
						case 22:
							functionReturnValue = 35;
							break;
						case 23:
						case 35:
							functionReturnValue = 30;
							break;
						case 24:
							functionReturnValue = 24;
							break;
						case 25:
							functionReturnValue = 17;
							break;
						case 30:
						case 31:
							functionReturnValue = 120;
							break;
						case 32:
							functionReturnValue = 110;
							break;
						case 33:
							functionReturnValue = 80;
							break;
						case 34:
							functionReturnValue = 60;
							break;
					}
					break;
			}
			return functionReturnValue;
		}


#region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //'*******************************************************************************
        //'機 能: ViscomX線ドライバがBusyか
        //'
        //'           変数名          [I/O] 型        内容
        //'引 数: なし
        //'戻 り 値： なし
        //'
        //'補 足: なし
        //'
        //'履　　歴： v11.5  2006/04/04  (WEB)間々田  新規作成
        //'*******************************************************************************
        //private static bool IsViscomDrvBusy()
        //{
        //
        //    int ViscomStatus1 = 0;
        //    int ViscomStatus2 = 0;
        //
        //    modViscom.GetDrvState(ref ViscomStatus1, ref ViscomStatus2);
        //
        //    return ViscomStatus1 & modViscom.XST1_DRV_BUSY;
        //
        //}

        //'*******************************************************************************
        //'機 能: ViscomX線ドライバがReadyか
        //'
        //'           変数名          [I/O] 型        内容
        //'引 数: なし
        //'戻 り 値： なし
        //'
        //'補 足: なし
        //'
        //'履　　歴： v11.5  2006/04/04  (WEB)間々田  新規作成
        //'*******************************************************************************
        //private static bool IsViscomDrvReady()
        //{
        //
        //    int ViscomStatus1 = 0;
        //    int ViscomStatus2 = 0;
        //
        //    modViscom.GetDrvState(ref ViscomStatus1, ref ViscomStatus2);
        //
        //    return ViscomStatus1 & modViscom.XST1_DRV_READY;
        //
        //}
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion


        //*******************************************************************************
        //機　　能： ViscomＸ線ドライバがReadyか
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.0  2006/04/04  (WEB)間々田  新規作成
        //*******************************************************************************
		public static bool IsDrvReady()
		{
			bool functionReturnValue = false;

			switch (XrayType)
            {

#region			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //Viscom製用
				//        Case XrayTypeViscom
				//
				//            IsDrvReady = IsViscomDrvReady
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion
				
                //その他
				default:
					functionReturnValue = !myBusy;
					break;
			}
			return functionReturnValue;
		}


#region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //'*******************************************************************************
        //'機　　能： ViscomＸ線ドライバがReady状態になるまで待つ
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： theTimeOut      [I/ ] Long      タイムアウト時間（ms）
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v11.5  2006/04/04  (WEB)間々田  新規作成
        //'*******************************************************************************
        //private static void WaitUntilViscomDrvReady(int theTimeOut)
        //{
        //
        //    int StartTime = 0;
        //    StartTime = Winapi.GetTickCount();
        //
        //    while (!IsViscomDrvReady())
        //    {
        //        if (modCT30K.RequestExit)
        //            return;
        //        if (Winapi.GetTickCount() - StartTime > theTimeOut)
        //            return;
        //
        //        //DoEvents
        //        modCT30K.PauseForDoEvents(0.3f);
        //    }
        //
        //}
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

        //
        //   Up_XR_Statusプロパティが０以外に変化するまで待つ
        //
		private static void WaitXrayReady()
		{
			int StartTime = 0;
			StartTime = Winapi.GetTickCount();

			//Up_XR_Statusプロパティが０以外に変化するまで待つ
            while ((XrayControl.Up_XR_Status == 0) && (Winapi.GetTickCount() - StartTime < Convert.ToInt32(60) * Convert.ToInt32(1000)))
            {
				//マウスポインタを砂時計にする
				Cursor.Current = Cursors.WaitCursor;
				Application.DoEvents();    //v9.7追加 by 間々田 2004/09/27
            }

			//マウスポインタを元に戻す
			Cursor.Current = Cursors.Default;
		}

        //
        //   Up_XR_Status/Up_Standby/Up_InterLock/Up_Warmupが０以外に変化するまで待つ       'v9.7追加 by 間々田 2004/12/09
        //
		private static void WaitXrayReadyAll()
		{
			int StartTime = 0;
            StartTime = Winapi.GetTickCount();

			//各プロパティが０以外に変化するまで待つ（タイムアウトは３秒）
            while (((XrayControl.Up_XR_Status == 0) || (XrayControl.Up_Standby == 0) || (XrayControl.Up_InterLock == 0) || (XrayControl.Up_Warmup == 0))
                && (Winapi.GetTickCount() - StartTime < Convert.ToInt32(3) * Convert.ToInt32(1000)))
            {
                Application.DoEvents();
            }
		}

        //
        //   Up_XrayStatusCMVプロパティがValueに変化するまで待つ    'v15.11追加 byやまおか 2010/02/16
        //
        //Public Sub WaitXrayCMV_Ready(ByVal Value As Integer)
		public static bool WaitXrayCMV_Ready(int Value)
		{
            int StartTime = 0;
			bool TimeOut = false;	//v16.01追加 byやまおか 2010/02/25
            TimeOut = false;		//v16.01追加 byやまおか 2010/02/25
			bool functionReturnValue = true;	//v16.01 Function化 byやまおか 2010/02/25

            StartTime = Winapi.GetTickCount();

			//Up_XrayStatusSMVプロパティがValueに変化するまで待つ（タイムアウトは５秒）
			//Do While (.Up_XrayStatusSMV <> Value) And (GetTickCount() - StartTime < CLng(5) * CLng(1000))			
            while ((XrayControl.Up_XrayStatusSMV != Value) && (!TimeOut))    //v16.01変更 byやまおか 2010/02/25
            {
				//マウスポインタを砂時計にする
				Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();
                TimeOut = ((Winapi.GetTickCount() - StartTime) > (Convert.ToInt32(5) * Convert.ToInt32(1000))); //v16.01追加 byやまおか 2010/02/25
                Thread.Sleep(1000);     //v16.01追加 byやまおか 2010/02/25
			}

			//タイムアウトならFalseを返す
            if (TimeOut) functionReturnValue = false;	//v16.01追加 byやまおか 2010/02/25

			//マウスポインタを元に戻す
			Cursor.Current = Cursors.Default;
			return functionReturnValue;
		}

        //
        //   Up_XrayStatusSMVプロパティがReady(0以外)に変化するまで待つ    'v15.11追加 byやまおか 2010/02/16
        //
		public static void WaitXraySMV_Ready()
		{
			int StartTime = 0;

            StartTime = Winapi.GetTickCount();

			//Up_XrayStatusSMVプロパティがReady(0以外)に変化するまで待つ
            while ((XrayControl.Up_XrayStatusSMV == 0) && (Winapi.GetTickCount() - StartTime < Convert.ToInt32(5) * Convert.ToInt32(1000)))
            {
				//マウスポインタを砂時計にする
				Cursor.Current = Cursors.WaitCursor;
				Application.DoEvents();
			}

			//マウスポインタを元に戻す
			Cursor.Current = Cursors.Default;
		}


        //
        //   Up_XR_CurrentFeedbackがValue以下になるまで待つ    'v16.30/v17.00追加 byやまおか 2010/03/01
        //
		public static void WaitXrayUnderCurrent(int Value)
		{
			int StartTime = 0;

            StartTime = Winapi.GetTickCount();

			//Up_XR_CurrentFeedbackがValue以下になるまで待つ（１０秒）
            while ((XrayControl.Up_XR_CurrentFeedback > Value) && (Winapi.GetTickCount() - StartTime < Convert.ToInt32(10) * Convert.ToInt32(1000)))
            {
				//マウスポインタを砂時計にする
				Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();
			}

			//マウスポインタを元に戻す
			Cursor.Current = Cursors.Default;
		}

        //
        //   Up_XR_VoltageFeedbackがValue以下になるまで待つ    'v17.02追加 byやまおか 2010/06/14
        //
		public static void WaitXrayUnderVoltage(int Value)
		{
			int StartTime = 0;

            StartTime = Winapi.GetTickCount();
            
			//Up_XStartTimeR_CurrentFeedbackがValue以下になるまで待つ（１０秒）
            while ((XrayControl.Up_XR_VoltFeedback > Value) && (Winapi.GetTickCount() - StartTime < Convert.ToInt32(10) * Convert.ToInt32(1000)))
            {
				//マウスポインタを砂時計にする
				Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();
			}

			//マウスポインタを元に戻す
            Cursor.Current = Cursors.Default;
		}


        //*******************************************************************************
        //機　　能： Ｘ線ＯＮ処理（Ｘ線アベイラブルになるまで待つ）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [ /O] Long      結果(0:成功, 0以外:失敗)
        //
        //補　　足： なし
        //
        //履　　歴： V3.0   00/08/10   (SI1)鈴山     新規作成
        //           v11.4  2006/03/03 (SI3)間々田   東芝 EXM2-150対応
        //           v11.5  2006/06/21 (WEB)間々田   Viscom対応
        //*******************************************************************************
        //Public Function Call_XrayON(Optional ByVal DoSetKVMA As Boolean = True) As Long
        //Public Function TryXrayOn(Optional ByVal DoSetKVMA As Boolean = True, _
        //'                          Optional ByVal WaitUntilAvailable As Boolean = True, _
        //'                          Optional ByVal QueryWarmup As Boolean = False, _
        //'                          Optional ByVal WaitUntilXrayOn As Boolean = True) As Long    'v11.5変更 by 間々田 2006/10/20 WaitUntilXrayOnフラグ追加

        //v15.0削除 by 間々田 2009/03/18 WaitUntilXrayOnフラグ削除
        //Public Function TryXrayOn(Optional ByVal DoSetKVMA As Boolean = True, _
        //'                          Optional ByVal WaitUntilAvailable As Boolean = True, _
        //'                          Optional ByVal QueryWarmup As Boolean = False) As Long
		public static int TryXrayOn(float? kv = null, float? MA = null, bool WaitUntilAvailable = true, bool IsQueryWarmup = false)
        //v17.51変更 QueryWarmup→IsQueryWarmup by 間々田 2011/03/24 他でQueryWarmup関数を使用しており混乱を避けるため
		{
            DateTime StartTime;	    //開始時間(秒)   'v14.14変更 Single→Variant by 間々田 2008/02/20
			modCT30K.OnOffStatusConstants LastXrayOn = default(modCT30K.OnOffStatusConstants);		//Ｘ線ＯＮ状態

			//戻り値の初期値
            int functionReturnValue = 0;

			//ＰＣからの制御ができない場合、ここで抜ける
            if (CTSettings.scaninh.Data.xray_remote != 0) return functionReturnValue;

			//設定管電圧が指定されている場合
            if ((kv != null)) SetVolt((float)kv);

			//設定管電流が指定されている場合
            if ((MA != null)) SetCurrent((float)MA);

            //Rev21.00/Rev20.00 by長野 2015/04/13
            //アベイラブルが落ちるよりも先に処理が進んでしまうため、少し待つ by長野 2015/03/19
            modCT30K.PauseForDoEvents(1);

			//Ｘ線がOFF状態の場合
			bool Cancel = false;
			const int TimeOutSec = 30;  //追加2014/10/07hata_v19.51反映 
			bool IsTimeOut = false;     //追加2014/10/07hata_v19.51反映 
            if (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OffStatus) 
            {
				//電磁ロックが開の場合、自動的に電磁ロックを閉とする 'v15.0追加 by 間々田 2009/07/21
                if (frmCTMenu.Instance.DoorStatus == frmCTMenu.DoorStatusConstants.DoorClosed) 
                {					
                    modSeqComm.SeqBitWrite("DoorLockOn", true);
					modCT30K.PauseForDoEvents(2);		//変更 1→2 by 間々田 2009/08/24
				}

                //産業用CTモードの場合   'v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                //Rev23.10 操作パネル機能を分離 by長野 2015/10/16
                if ((CTSettings.scaninh.Data.op_panel == 0)||(CTSettings.scaninh.Data.avmode == 0))
                {
                    //メッセージ表示：
                    //操作パネルがONなら
                    if (modSeqComm.MySeq.PcInhibit)
                    {
                        //操作パネルがONのため動作しません。
                        functionReturnValue = 17513;
                        return functionReturnValue;
                    }
                }

                //Rev26.40 add by chouno 2019/02/17
                if (CTSettings.scaninh.Data.high_speed_camera == 0 && CTSettings.iniValue.HSCSettingType == 1)
                {
                    //メッセージ表示：
                    //動作許可が不許可なら
                    if (modSeqComm.MySeq.PcInhibit)
                    {
                        //動作許可がOFFのため動作しません。
                        functionReturnValue = 27000;
                        return functionReturnValue;
                    }
                }

				//インターロックチェック
				if (!IsXrayInterLock()) 
                {
					//メッセージ表示：電磁ロックが開なので、Ｘ線をオンできません。
					//TryXrayOn = 9369
					//v15.03変更 リソース化&メッセージ変更　by やまおか 2009/11/17
                    if (!modSeqComm.MySeq.stsDoorLock && CTSettings.scaninh.Data.door_lock == 0) 
                    {
						//電磁ロックが開なので、Ｘ線オンできません。
						functionReturnValue = 9614;
					} 
                    else 
                    {
						//扉が開なので、Ｘ線オンできません。
						functionReturnValue = 9615;
					}
					return functionReturnValue;					
				}               
                //スタンバイモードチェック
                else if (!IsXrayReady()) 
                {
					//メッセージ表示：Ｘ線がスタンバイモードのため、Ｘ線をオンできません。
					functionReturnValue = 9370;
					return functionReturnValue;					
				}
                //Ｘ線ウォームアップ未完了の場合
                else if (XrayWarmUp() == XrayWarmUpConstants.XrayWarmUpNotComplete) 
                {
					//ウォームアップするかどうかをここで尋ねない場合					
					//if (!IsQueryWarmup)     //v17.51変更 QueryWarmup→IsQueryWarmup by 間々田 2011/03/24 他でQueryWarmup関数を使用しており混乱を避けるため
                    //if ((!IsQueryWarmup) | (XrayType == XrayTypeConstants.XrayTypeGeTitan)) //v18.00 Titan追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    //Rev25.03/Rev25.02 change by chouno 2017/02/05
                    if ((!IsQueryWarmup) | (XrayType == XrayTypeConstants.XrayTypeGeTitan) | (XrayType == XrayTypeConstants.XrayTypeSpellman))
                    {
						//メッセージ表示：Ｘ線が準備完了となっていないためＸ線をONできません。
						functionReturnValue = 9373;
						return functionReturnValue;
                    }
                    //ウォームアップするかどうかをここで尋ねる場合
#region				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                    //東芝 EXM2-150の場合
					//            ElseIf XrayType = XrayTypeToshibaEXM2_150 Then
					//
					//                'ダイアログ表示：
					//                '   最大管電圧までウォームアップしますか？
					//                '   「はい」をクリックすると最大管電圧まで、「いいえ」をクリックすると設定管電圧までウォームアップします。
					//                Select Case MsgBox(LoadResString(16106), vbYesNoCancel + vbQuestion)
					//
					//                    '「はい」の場合は、最大管電圧に設定してウォームアップする
					//                    Case vbYes
					//                        'BackXrayVoltSet = XrayVoltSet
					//                        BackXrayVoltSet = frmXrayControl.ntbSetVolt.Value  'v11.5変更 by 間々田 2006/07/10
					//                        'SetKVMA XrayMaxVolt
					//                        'SetKVMA frmXrayControl.cwneKV.Maximum               'v11.5変更 by 間々田 2006/07/10
					//                        SetVolt frmXrayControl.cwneKV.Maximum                'v15.0変更 by 間々田 2009/04/07
					//                        TryXrayOn = 9623    'ウォームアップ開始 'v16.20追加 byやまおか 2010/04/06
					//
					//                    '「いいえ」の場合は、現在の設定管電圧までウォームアップする
					//                    Case vbNo                                   'v16.20追加 byやまおか 2010/04/06
					//                        TryXrayOn = 9623    'ウォームアップ開始 'v16.20追加 byやまおか 2010/04/06
					//
					//                    '「キャンセル」の場合は、Ｘ線OFFとしてここで抜ける（ウォームアップしない）
					//                    Case vbCancel
					//                        TryXrayOn = 1902    'v16.20追加 byやまおか 2010/04/06
					//                        Exit Function
					//
					//                End Select
					//
					//            'Viscomの場合                           'v11.5追加 by 間々田 2006/05/08
					//            ElseIf XrayType = XrayTypeViscom Then
					//
					//                'ダイアログ表示：
					//                '   ～までウォームアップを実行します。
					//                '   よろしいですか？
					//                If MsgBox(GetResString(9609, CStr(XrayVoltAtWarmingup)), vbExclamation Or vbYesNo Or vbDefaultButton2) = vbYes Then
					//
					//                    'ウォームアップ完了後のフィラメント調整を実施するためのフラグをセット
					//                    'FilamentAdjustAfterWarmup = True
					//                    If frmXrayControl.chkFilament.Value = vbChecked Then FilamentAdjustAfterWarmup = True      'changed by 山本 2006-12-13
					//
					//                    'ウォームアップ開始
					//                    SendCode CODE_XRAY_WCON
					//
					//                    'ウォームアップ中断フラグをリセット
					//                    XrayWarmUpCanceled = False
					//
					//                    'ウォームアップ中の最大出力管電圧をクリア
					//                    XrayMaxFeedBackVolt = 0
					//
					//                End If
					//
					//                Exit Function
                    //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion
                    
                    //浜ホト230kVの場合  'v16.01追加 byやまおか 2010/02/25
                    //else if (XrayType == XrayTypeConstants.XrayTypeHamaL10801) 
                    else if (XrayType == XrayTypeConstants.XrayTypeHamaL10801 || XrayType == XrayTypeConstants.XrayTypeHamaL12721) //Rev23.10 追加 by長野 2015/10/01
                    {
                        frmXrayControl.Instance.cmdWarmupStart_Click(null, new EventArgs());
						//開始しますか？いいえを押しても開始してしまう不具合対策
                        if (!WUP_Start) return functionReturnValue;		//v16.01/v17.00追加 byやまおか 2010/03/02
					}
				}


                //Ｘ線が準備完了の場合

#region			//v15.0削除ここから by 間々田 2009/04/07
                //'コモン値を読み取り、管電圧／管電流をＸ線制御器にセットする
				//'   管電圧：scansel.scan_kv
				//'   管電流：scansel.scan_ma
				//If DoSetKVMA Then SetKVMA scansel.scan_kv, scansel.scan_ma
                //v15.0削除ここまで by 間々田 2009/04/07
#endregion
                
                //Ｘ線をオンする
				XrayOn();

				Cancel = false;

                //Viscomの場合
                if (XrayType == XrayTypeConstants.XrayTypeViscom)
                {

                    //Ｘ線がオンかつウォーミングアップ完了かつフィラメント調整完了になるまで待つ
                    while (!((frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus & frmXrayControl.Instance.lblWarmupStatus.Text == CTResources.LoadResString(12060) & frmXrayControl.Instance.FilamentAdjustStatus == StringTable.GC_STS_STANDBY_OK) | Cancel))
                    {

                        //Ｘ線が実際にオンになるまで待つ
                        while (!((frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus) | Cancel))
                        {
                            modCT30K.PauseForDoEvents(0.5f);    //changed by 山本　2006-10-23 1->0.5
                            Cancel = (CTLib.sig_chk() != 0);
                        }

                        //ウォームアップが完了になるまで待つ
                        while (!((frmXrayControl.Instance.lblWarmupStatus.Text == CTResources.LoadResString(12060)) | Cancel))
                        {
                            //frmMessage.lblMessage = "ウォームアップが必要になりましたので完了するまでしばらくお待ち下さい。"
                            modCT30K.PauseForDoEvents(0.5f);                            //changed by 山本　2006-10-23 1->0.5
                            Cancel = (CTLib.sig_chk() != 0);
                        }

                        //Ｘ線が実際にオンになるまで待つ
                        while (!((frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus) | Cancel))
                        {
                            modCT30K.PauseForDoEvents(0.5f);    //changed by 山本　2006-10-23 1->0.5
                            Cancel = (CTLib.sig_chk() != 0);
                        }

                        //フィラメント調整が準備完了になるまで待つ
                        while (!((frmXrayControl.Instance.FilamentAdjustStatus == StringTable.GC_STS_STANDBY_OK) | Cancel))
                        {
                            //frmMessage.lblMessage = "フィラメント調整が必要になりましたので完了するまでしばらくお待ち下さい。"
                            modCT30K.PauseForDoEvents(0.5f);    //changed by 山本　2006-10-23 1->0.5
                            Cancel = (CTLib.sig_chk() != 0);
                        }

                    }

                    //メッセージフォームが表示されている場合、アンロードする
                    //If IsExistForm(frmMessage) Then Unload frmMessage
                    modCT30K.HideMessage();     //変更 by 間々田 2009/08/24

                }
                //else if ((XrayType == XrayTypeConstants.XrayTypeGeTitan))
                //Rev25.03/Rev25.02 change by chouno 2017/02/05
                else if ((XrayType == XrayTypeConstants.XrayTypeGeTitan) || (XrayType == XrayTypeConstants.XrayTypeSpellman))
                {   　
                    //Titanの場合    'v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07_追加2014/10/07hata_v19.51反映 

                    //タイムアウト時間

                    //開始時間
                    //StartTime = DateAndTime.Now;
                    StartTime = DateTime.Now;

                    //タイムアウトフラグ
                    IsTimeOut = false;

                    //Ｘ線が実際にオンになるまで待つ
                    while (!IsTimeOut)
                    {
                        IsTimeOut = (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus) | Cancel;
                        //if (DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Second, StartTime, DateAndTime.Now) > TimeOutSec)
                        if ((DateTime.Now - StartTime).TotalSeconds > TimeOutSec)  
                        {
                            IsTimeOut = true;
                            Cancel = true;
                            break; // TODO: might not be correct. Was : Exit Do
                        }
                        //v19.50 0.1秒から1秒に変更 by長野 2013/12/17
                        //PauseForDoEvents 0.1
                        modCT30K.PauseForDoEvents(1);
                        //v19.50 念のため X線のupdateタイマーを実行させる by長野 2013/12/17
                        frmXrayControl.Instance.MyUpdate();
                        Cancel = (CTLib.sig_chk() != 0);
                    }

                    //元に戻す   'v15.0変更 by 間々田 2009/03/18
                }
                else
                {
                    //ElseIf WaitUntilXrayOn Then 'v11.5変更 by 間々田 2006/10/20 透視画像表示時のＸ線オン時は待たないことにした

                    //Ｘ線が実際にオンになるまで待つ
                    while (!((frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus) || Cancel))
                    {
                        modCT30K.PauseForDoEvents(0.1F);	//v11.5変更 by 間々田 2006/10/20 1→0.1に変更
                        Cancel = (CTLib.sig_chk() != 0);
                    }
                
                }

                if (Cancel) 
                {
					XrayOff();		//Ｘ線をOFFする
					functionReturnValue = 1902;
					return functionReturnValue;
				}
			}

			//Ｘ線アベイラブルになるまで待つ場合
			//v11.5追加 if文のみ追加 by 間々田 2006/06/09
			if (WaitUntilAvailable) 
            {
                LastXrayOn = modCT30K.OnOffStatusConstants.OffStatus;

				//開始時間を設定
				//StartTime = Timer
				StartTime = DateTime.Now;	//v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20

				//Ｘ線アベイラブルになるまで待つ
				//v11.5変更 by 間々田 2006/06/13
                while (!(frmXrayControl.Instance.MecaXrayAvailable == modCT30K.OnOffStatusConstants.OnStatus)) 
                {
					//停止要求があるか
					//If IsRequestToStop Then
					if (CTLib.sig_chk() != 0) 
                    {
						XrayOff();	//Ｘ線をOFFする
						functionReturnValue = 1902;
						return functionReturnValue;						
					} 
                    //Ｘ線が途中でＯＦＦされた
					//ElseIf LastXrayOn And (Not IsXrayOn) Then
                    else if ((LastXrayOn == modCT30K.OnOffStatusConstants.OnStatus) && 
                             (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OffStatus))
                    {
                        functionReturnValue = 1902;
                        return functionReturnValue;                        
                    }
                    //タイムアウト：Ｘ線出力が設定値に達しません。
                    //ElseIf Timer > StartTime + GC_X_AVAIL_WAIT_TIME Then
                    else if ((DateTime.Now - StartTime).TotalSeconds > GC_X_AVAIL_WAIT_TIME)    //v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
                    {               
                        functionReturnValue = 9377;
                        return functionReturnValue;
                    }

                    LastXrayOn = frmXrayControl.Instance.MecaXrayOn;
					Application.DoEvents();
				}

                //追加2014/10/07hata_v19.51反映 
                //Titanの場合は      'v18.00追加 byやまおか 2011/07/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
				//if ((XrayType == XrayTypeConstants.XrayTypeGeTitan))
                //Rev25.03/Rev25.02 change by chouno 2017/02/05
                if ((XrayType == XrayTypeConstants.XrayTypeGeTitan) || (XrayType == XrayTypeConstants.XrayTypeSpellman))
                {
					//x_off.csvを更新する
					modTitan.Ti_UpdateXoffcsv();
				}

			}
			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： Ｘ線オン処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/17  (SI3)間々田  新規作成
        //           v11.5  2006/04/04  (WEB)間々田  Viscom対応
        //*******************************************************************************
		public static void XrayOn()
		{
			//すでにＸ線オンの場合何もしない
            if (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus) return;    //v11.5変更 by 間々田 2006/06/21

			///    Dim count As Long
            ///    count = 0

//#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //ビープ音を鳴らす(500ms間隔で5回)   'v17.00追加 byやまおか 2010/01/19
			//    If scaninh.xrayon_beep = 0 Then
			//'        SoundBeep 5, 500
			//        PlayXrayOnWarningSound
			//    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            
            //Rev23.00 ビープ音.NET対応 by長野 2015/09/24
            if (CTSettings.scaninh.Data.xrayon_beep == 0)
            {
                modSound.PlayXrayOnWarningSound();
            }

//#endregion

            switch (XrayType)
            {
                //東芝 EXM2-150用
                case XrayTypeConstants.XrayTypeToshibaEXM2_150:

                    modCT30K.PauseForDoEvents(0.1f);
                    //added by 山本　2007-3-14  シーケンサ通信エラーが発生する対策

                    //SeqComm.exeのメソッドを呼び出す
                    modSeqComm.SeqBitWrite("EXMOn", true);

                    modCT30K.PauseForDoEvents(0.3f);
                    //added by 山本　2007-3-14  シーケンサ通信エラーが発生する対策
                    break;

#region			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                ////Viscom製用         'v11.5追加 by 間々田 2006/04/04
                //case XrayTypeConstants.XrayTypeViscom:
                //    modViscom.SendCode(modViscom.CODE_XRAY_ON, 1);
                //    Thread.Sleep(500);  //v17.701/v19.02追加 byやまおか 2012/03/28
                //    break;
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

                //Titan用            'v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07_追加2014/10/07hata_v19.51反映 
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    modTitan.Ti_XrayOn();
                    break;


                //その他
				default:
					//FeinFocus.exeのメソッドを呼び出す
					XrayControl.Xrayonoff_Set(1);
					break;
            }

#region		//v11.5削除ここから by 間々田 2006/10/20 TryXrayOnでチェックするので不要
            ///    'Ｘ線が実際にオンするまで待つ（タイムアウトは10秒）
			///    Do Until frmMechaReset2.MecaXrayOn = OnStatus 'v11.5変更 by 間々田 2006/06/21
			///        PauseForDoEvents 0.5
			///        count = count + 1: If count > 20 Then Exit Do
			///    Loop
            //v11.5削除ここまで by 間々田 2006/10/20
#endregion

            //Ｘ線をオンした時刻                     追加 by 間々田 2009/08/17
			TimeAtXrayOn = DateTime.Now;

#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //X線警告機能ON v16.01 追加 by 山影 10-02-15
			//    frmXrayWarning.XWarningOn = OnStatus
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

            //Rev23.40 by長野 2016/04/05 //Rev23.20 X線警告機能ON by長野 2016/02/22
            if (CTSettings.scaninh.Data.high_speed_camera == 0)
            {
                frmXrayWarning.Instance.XWarningOn = modCT30K.OnOffStatusConstants.OnStatus;
            }
        }


        //*******************************************************************************
        //機　　能： Ｘ線オフ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 以前のCall_XrayOFFと同じ処理
        //
        //履　　歴： v11.3  2006/02/17  (SI3)間々田  新規作成
        //           v11.5  2006/04/04  (WEB)間々田  Viscom対応
        //*******************************************************************************
		public static void XrayOff()
		{
			int Count = 0;
			Count = 0;

			switch (XrayType)
            {

                //東芝 EXM2-150用
                case XrayTypeConstants.XrayTypeToshibaEXM2_150:

                    modCT30K.PauseForDoEvents(0.1f);
                    //added by 山本　2007-3-14  シーケンサ通信エラーが発生する対策

                    //SeqComm.exeのメソッドを呼び出す
                    modSeqComm.SeqBitWrite("EXMOff", true);

                    modCT30K.PauseForDoEvents(0.3f);
                    //added by 山本　2007-3-14  シーケンサ通信エラーが発生する対策
                    break;

#region			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                ////Viscom製用         'v11.5追加 by 間々田 2006/04/04
                //case XrayTypeConstants.XrayTypeViscom:

                //    //Ｘ線ウォームアップ中の場合、Ｘ線ウォームアップ強制中断フラグオン
                //    if (XrayWarmUp() == XrayWarmUpConstants.XrayWarmUpNow) XrayWarmUpCanceled = true;

                //    //'If IsViscomDrvBusy Then        'v11.5削除 by 間々田 2006/10/06
                //    //'    SetXrayStop                'v11.5削除 by 間々田 2006/10/06
                //    //'Else                           'v11.5削除 by 間々田 2006/10/06
                //    //    SendCode CODE_XRAY_ON, 0
                //    //'End If                         'v11.5削除 by 間々田 2006/10/06

                //    //v11.5以下に変更 by 間々田 2006/10/18
                //    //   ウォームアップ中かつドライバビジーのとき SetXrayStopをコール
                //    //   ドライバビジーの時、SetXrayStopをコールする必要があるが、
                //    //   スキャンスタートしてすぐにスキャンストップした時はSetXrayStopをコールするとかえってＸ線をオフできない。
                //    //   一方、ウォームアップ中、SendCode CODE_XRAY_ON, 0をコールするとDLLからCT30Kに応答が戻らなくなる
                //    //   よって以下のようにした。
                //    //If (XrayWarmUp = XrayWarmUpNow) And IsViscomDrvBusy Then
                //    //V11.5変更 BY 間々田 2006/10/30 フィラメント調整中にキャンセルする場合もこちら
                //    if (((XrayWarmUp() == XrayWarmUpConstants.XrayWarmUpNow) & modXrayControl.IsViscomDrvBusy()) | (modViscom.ViscomState1 & modViscom.XST1_FLM_RUNNING))
                //    {
                //        modViscom.SetXrayStop();
                //    }
                //    else
                //    {
                //        modViscom.SendCode(modViscom.CODE_XRAY_ON, 0);
                //    }
                //
                //    Thread.Sleep(1000);    //ちょっと待つ   'v17.72/v19.02追加 byやまおか 2012/05/16
                //   
                //    break;
                //    //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion


                //Titan用            'v18.00追加 byやまおか 2011/03/07 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                case XrayTypeConstants.XrayTypeGeTitan:
                case XrayTypeConstants.XrayTypeSpellman: //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    //X線OFF
                    modTitan.Ti_XrayOff();

                    //Rev23.20 追加 2世代・3世代兼用の場合は、シーケンサに400以上or430以上のX線が出せるかどうかを通知する by長野 2016/01/18
                    //ウォームアップ中は、設定管電圧の-20にする
                    if (modTitan.Flg_TiWarmingUp == true)
                    {
                        if (CTSettings.scaninh.Data.ct_gene2and3 == 0)
                        {
                            modTitan.SendSeq_Ti_XoffVolt_ByValue((int)modXrayControl.XrayVoltSet() - 20);
                        }
                    }
                    else
                    {
                        if (CTSettings.scaninh.Data.ct_gene2and3 == 0)
                        {
                            modTitan.SendSeq_Ti_XoffVolt();
                        }
                    }

                    //ウォームアップ中をフラグをOFFする
                    //Rev23.20 ここでフラグを落とすのは間違いなので削除 by長野 2016/01/21
                    //modTitan.Flg_TiWarmingUp = false;

                    //v19.50 念のため frmXrayControl.Updateを一度実行しておく by長野 2013/12/17
                    frmXrayControl.Instance.MyUpdate();
                    break;


                //その他
				default:
					//FeinFocus.exeのメソッドを呼び出す
					XrayControl.Xrayonoff_Set(2);
					
				    //'管電流が10μ以下になるまで待つ 'v16.30/v17.00追加 byやまおか 2010/03/01
				    //WaitXrayUnderCurrent 10    'v17.10削除 ここでは待たない byやまおか 2010/08/26
                    break;
			}

			//Ｘ線が実際にオフするまで待つ（タイムアウトは10秒）'v11.5 削除 by 間々田 2006/10/24
			//v11.5変更 by 間々田 2006/06/21
            while (!(frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OffStatus)) 
            {
                modCT30K.PauseForDoEvents(0.1F);    //changed by 山本　2006-10-23 0.5->0.1
				Count = Count + 1;
				//if (Count > 10)	break; 				//changed by 山本　2006-10-23 20->10
                if (Count > 100) break; 				//v18.00変更 byやまおか 2011/09/05 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            }

			//ステータス表示がX線ONのままになってしまうことがあるため
			//いったんここで準備完了にする 'v17.46追加 byやまおか 2011/02/27
            frmXrayControl.Instance.XrayStatus = StringTable.GC_STS_STANDBY_OK;

			//メカコントロール欄のX線にも反映    'v17.72/v19.02追加 byやまおか 2012/05/16
            //frmMechaControl.Instance.ImgXrayTube.Image = frmMechaControl.Instance.ImageList1.Images["XrayTubeOFF"];
            frmMechaControl.Instance.ImgXrayTube.Image = Resources.XrayTubeG.ToBitmap();

#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //X線警告機能OFF v16.01 追加 by 山影 10-02-15
			//    frmXrayWarning.XWarningOn = OffStatus
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

            //Rev23.40 by長野 2016/04/05 //Rev23.20 X線警告機能ON by長野 2016/02/22
            if (CTSettings.scaninh.Data.high_speed_camera == 0)
            {
                frmXrayWarning.Instance.XWarningOn = modCT30K.OnOffStatusConstants.OffStatus;
            }

            //追加2014/10/07hata_v19.51反映 
            //Ti_XrayOff内でウォームアップ中フラグを落とすと     'v18.00追加 byやまおか 2011/09/05 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //中断したときにx_off.csvを現在の管電圧で書き換えてしまうため
            //ここでTitan.dllのウォームアップ中フラグを落とすことにする
            //if ((XrayType == XrayTypeConstants.XrayTypeGeTitan))
            //Rev25.03/Rev25.02 add by chouno 2017/02/05 
            if ((XrayType == XrayTypeConstants.XrayTypeGeTitan) || (XrayType == XrayTypeConstants.XrayTypeSpellman))
            {
                modTitan.Ti_SetWarmingupFlag(0);
            }

            if (frmScanControl.Instance.XrayAuto == true)
            {
                modXrayAutoVI.XrayAutoVICancel();
                frmTransImage.Instance.CaptureOn = false;
            }
        }

        //*******************************************************************************
        //機　　能： Ｘ線タイマー処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：
        //
        //補　　足： 以前のCall_XrayOFFと同じ処理
        //
        //履　　歴： v19.50 2013/11/26  (検S1)長野  新規作成
        //*******************************************************************************
        public static bool XrayHamaTimer(ref int TimerCount)
        {
            bool functionReturnValue = false;

            functionReturnValue = false;

            bool TimeOut = false;
            TimeOut = false;

            //object StartTime = null;
            //object RestTime = null;
            DateTime StartTime = default(DateTime);
            double RestTime = 0;

            StartTime = DateTime.Now;
            
            int Count = 0;
            Count = 0;

            while (!TimeOut)
            {
                modCT30K.PauseForDoEvents(0.5f);

                if (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OffStatus) break;
                if (XrayHamaTimerEndFlg == true) break; 
                
                //RestTime = TimerCount - DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Second, StartTime, DateAndTime.Now);
                RestTime = Convert.ToDouble(TimerCount) - ((DateTime.Now - StartTime).TotalSeconds);

                if (RestTime < 0)
                {
                    TimeOut = true;
                    XrayHamaTimerEndFlg = true;
                }

                //更新周期を最初と最後の1回またはループ5回につき1回にする
                Count = Count + 1;
                if ((Count % 5 == 0) | Count == 1 | TimeOut == true)
                {
                    //変更2014/11/28hata_v19.51_dnet
                    //frmAdjAlignment.Instance.cwneHamaTimerMM.Text = Convert.ToInt32(RestTime / 60).ToString();
                    frmAdjAlignment.Instance.cwneHamaTimerMM.Text = ((int)(RestTime / 60)).ToString();
                    frmAdjAlignment.Instance.cwneHamaTimerSS.Text = Convert.ToInt32(RestTime % 60).ToString();
                    
                    frmAdjAlignment.Instance.cwneHamaTimerSS.Text = Convert.ToInt32(RestTime % 60).ToString();
                }

            }

            //このフラグは、停止ボタンクリックか正常終了時にのみTrue
            if ((XrayHamaTimerEndFlg == true)) functionReturnValue = true;

            return functionReturnValue;

        }

	}
}
