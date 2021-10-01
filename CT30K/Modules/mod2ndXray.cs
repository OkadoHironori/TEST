using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using CTAPI;
using CT30K.Common;


namespace CT30K
{
    static class mod2ndXray
    {
        ///* ************************************************************************** */
        ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
        ///* 客先　　　　： ?????? 殿                                                   */
        ///* プログラム名： mod2ndDetctor.bas                                           */
        ///* 処理概要　　： X線切替機能関連モジュール                                   */
        ///* 注意事項　　： なし                                                        */
        ///* -------------------------------------------------------------------------- */
        ///* 適用計算機　： PC                                                          */
        ///* ＯＳ　　　　： Windows 7 (SP1) 64bit                                       */
        ///* コンパイラ　： VS 2010                                                     */
        ///* -------------------------------------------------------------------------- */
        ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
        ///*                                                                            */
        ///* v23.10      15/09/20    (検S1)長野          新規作成                       */
        ///*                                                                            */
        ///* -------------------------------------------------------------------------- */
        ///* ご注意：                                                                   */
        ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
        ///*                                                                            */
        ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2015                 */
        ///* ************************************************************************** */


        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************

        //CT30K起動判定
		private static bool IsXrayStartUp;

        //検出器切替の場合，非常停止が発生したことを示すmsgboxでＯＫをクリックしたことを示すフラグを用意
        //通常はTrueにしておき，非常停止ボタンが押されたときだけFalseに変更
		public static bool MsgBoxOK;
        
        //CTソフトを再表示中、アンロードしたフォームを参照しにいかないようにフラグで管理する。
		public static bool ReloadFlag;
        
        //検出器の位置状態定数
		public enum XrayModeConstants
		{
			XrayMode_Xray1 = 0,		//X線１
			XrayMode_Xray2 = 1,		//X線２
			XrayMode_None = 2        //いずれでもない
		}
		private static XrayModeConstants myXrayMode;

        private static bool IsWarmUpBusy = false;

        private static bool changeXrayIsBusy = false;

        private static bool myPackageWUPFlg = false;
        public static bool PackageWUPFlg
        {
            get
            {
                return myPackageWUPFlg;
            }
            set
            {
                myPackageWUPFlg = value;
            }
           
        }

        //*************************************************************************************************
        //機　　能： 起動時の処理
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v23.10 2015/09/20 (検S1)長野   新規作成
        //*************************************************************************************************
    	public static bool SecondXrayStartUpProcess()
		{
			bool functionReturnValue = false;
			//v17.51変更 by 間々田 2011/03/23

			//戻り値初期化                       'v17.51追加 by 間々田 2011/03/23
			functionReturnValue = false;

			IsXrayStartUp = false;
			short xray_no = 0;
			MsgBoxOK = true;
			var _with1 = modSeqComm.MySeq;

            int xray_no_old = 0;
            xray_no_old = CTSettings.scansel.Data.multi_tube;
            ////現在の検出器位置とコモン（scansel.com_detector_no）が異なる場合は、全校正未完とする。
            //if (CTSettings.scansel.Data.multi_tube != (int)DetMode) {

            //    ResetCorrectStatus();

            //}

			//X線の位置を確認
			//X線１の時
			if (_with1.stsMicroFPDPos || _with1.stsMicroFPDShiftPos) {
				xray_no = 0;
				XrayMode = XrayModeConstants.XrayMode_Xray1;
			//X線２の時
			} else if (_with1.stsNanoFPDPos || _with1.stsNanoFPDShiftPos) {
				xray_no = 1;
                XrayMode = XrayModeConstants.XrayMode_Xray2;
				//ElseIf Not (.stsCTIIDrive Or .stsCTIIPos Or .stsTVIIDrive Or .stsTVIIPos) Then '検出器位置不定の場合なにもしない
			//v17.51変更 by 間々田 2011/03/23 検出器の状態が必ず検出器１／検出器２／不定のいずれかにするように変更
			} else {
                XrayMode = XrayModeConstants.XrayMode_None;
                xray_no = -1;
                IsXrayStartUp = true;
                XrayNoneLimitControl(XrayMode);

                return functionReturnValue;
                //ここで抜ける

            }

            //scanselに現在のX線をセット
            CTSettings.scansel.Data.multi_tube = xray_no;

            //現在の検出器にコモンを入れ替える。
            //途中でソフトが落ちてしまった場合にコモンが入れ違いになりやすいので常に入れ替える
            ComLib.change_xray_com(xray_no);

            CTSettings.scansel.Write();

            //scanselにセットした値と機構部が示すX線の種類が同じ場合は初回の切り替えは行わない
            if (xray_no_old != xray_no)
            {
                //現在の検出器にコモンを入れ替える。
                ComLib.change_xray_com(xray_no);

                if (myXrayMode == XrayModeConstants.XrayMode_Xray1)
                {
                    modXrayControl.XrayType = (modXrayControl.XrayTypeConstants)Convert.ToInt32(CTSettings.t20kinf.Data.system_type.GetString());
                }
                else if (myXrayMode == XrayModeConstants.XrayMode_Xray2)
                {
                    modXrayControl.XrayType = (modXrayControl.XrayTypeConstants)Convert.ToInt32(CTSettings.t20kinf.Data.system_type2.GetString());
                }

                //入れ替えた直後にコモンを読み込んでおく
                modCT30K.GetCommon();

                CTSettings.scansel.Write();

                //X線に合わせてGUIおよび内部処理も切り替えるる。
                ReloadForms();

                //ReloadForms中は、frmCTMenuにロックをかけておりfraFocusのEnableが変わらないのでここで変更
                frmXrayControl.Instance.fraFocus.Enabled = (modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete) && (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OffStatus);
                int xfocus = modXrayControl.XrayControl.Up_Focussize - 1;
                if (xfocus >= 0)
                    modLibrary.SetCmdButton(frmXrayControl.Instance.cmdFocus, Convert.ToInt32(xfocus), ControlEnabled: true);     //v11.41変更 by 間々田 2006/03/29

                ReloadFlag = true;
            }

            //CTSettings.scansel.Data.multi_tube = xray_no;

            //CTSettings.scansel.Write();

    //frmXrayControl.Instance.QueryWarmup();

            ScanCorrect.OptValueGet_Cor();

            //FCD,FDDを更新
            frmMechaControl.Instance.ntbFCD_ValueChanged(null, new NumTextBox.ValueChangedEventArgs(frmMechaControl.Instance.ntbFCD.Value));
            frmMechaControl.Instance.ntbFID_ValueChanged(null, new NumTextBox.ValueChangedEventArgs(frmMechaControl.Instance.ntbFID.Value));

            XrayNoneLimitControl(XrayMode);

            IsXrayStartUp = true;
			//戻り値セット                    'v17.51追加 by 間々田 2011/03/23
			functionReturnValue = true;
			return functionReturnValue;

		}
        //*******************************************************************************
        //機　　能： XrayModeプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v23.10  15/09/20  長野      新規作成
        //*******************************************************************************
        public static XrayModeConstants XrayMode
        {


			get { return myXrayMode; }
			set
            {
                if (!modCT30K.CT30KSetup) //起動開始中
                    return;

                if (changeXrayIsBusy == true)
                {
                    return;
                }
                else
                {
                    changeXrayIsBusy = true;

                    CTSettings.scansel.Load();

                    //X線切替機能が無効の時は、常に検出器１
                    if (!CTSettings.ChangeXrayOn)
                    {
                        //X線ONのときはOFFする 'v17.10 条件追加 byやまおか 2010/08/26
                        if (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus)
                        {
                            if (CTSettings.scaninh.Data.xray_remote == 0)		//v17.02改良(ここから) byやまおか 2010/06/14
                            {
                                //X線OFF
                                modXrayControl.XrayOff();
                            }												//v17.02改良(ここまで) byやまおか 2010/06/14
                                modCT30K.PauseForDoEvents(1);		//1秒待つ
                        }

                        myXrayMode = XrayModeConstants.XrayMode_Xray1;
                        CTSettings.scansel.Data.multi_tube = 0;
                        CTSettings.scansel.Write();
                        return;
                    }

                    //検出器が切り替わったらコモンの入れ替えとメニュー画面の更新をする
                    if (CTSettings.ChangeXrayOn & (myXrayMode != value) & IsXrayStartUp & !(value == XrayModeConstants.XrayMode_None))
                    {
                        //X線ONのときはOFFする 'v17.10 条件追加 byやまおか 2010/08/26
                        if (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus)
                        {
                            if (CTSettings.scaninh.Data.xray_remote == 0)		//v17.02改良(ここから) byやまおか 2010/06/14
                            {
                                //X線OFF
                                modXrayControl.XrayOff();
                            }												//v17.02改良(ここまで) byやまおか 2010/06/14
                            modCT30K.PauseForDoEvents(1);		//1秒待つ
                        }
                        //内部変数に記憶
                        myXrayMode = value;

                        if (myXrayMode == XrayModeConstants.XrayMode_Xray1)
                        {
                            modXrayControl.XrayType = (modXrayControl.XrayTypeConstants)Convert.ToInt32(CTSettings.t20kinf.Data.system_type.GetString());
                        }
                        else if (myXrayMode == XrayModeConstants.XrayMode_Xray2)
                        {
                            modXrayControl.XrayType = (modXrayControl.XrayTypeConstants)Convert.ToInt32(CTSettings.t20kinf.Data.system_type2.GetString());
                        }

                        //Rev23.10 順番変更 by長野 2015/11/24
                        ////X線更新用タイマーオフ
                        frmXrayControl.Instance.tmrUpdate.Enabled = false;

                        ComLib.change_xray_com((int)value);

                        ////X線更新用タイマーオフ
                        //frmXrayControl.Instance.tmrUpdate.Enabled = false;

                        //入れ替え後のコモンを読み込む。
                        modCT30K.GetCommon();

                        //未完にしない。校正要求の条件にX線管の種類がある
                        //全校正を未完にする
                        //ResetCorrectStatus();

                        //メニュー画面を更新する
                        ReloadForms();

                        //ReloadForms中は、frmCTMenuにロックをかけておりfraFocusのEnableが変わらないのでここで変更
                        frmXrayControl.Instance.fraFocus.Enabled = (modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete) && (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OffStatus);
                        int xfocus = modXrayControl.XrayControl.Up_Focussize - 1;
                        if (xfocus >= 0)
                            modLibrary.SetCmdButton(frmXrayControl.Instance.cmdFocus, Convert.ToInt32(xfocus), ControlEnabled: true);     //v11.41変更 by 間々田 2006/03/29

                        //Rev23.10 追加 by長野 2015/11/24
                        ComLib.change_xray_com((int)value);

                        ////X線更新用タイマーオフ
                        //frmXrayControl.Instance.tmrUpdate.Enabled = false;

                        //入れ替え後のコモンを読み込む。
                        modCT30K.GetCommon();

                        frmXrayControl.Instance.tmrUpdate.Enabled = true;
                    }

                    if((int)value == 2)
                    {
                        //不定の場合は、X線1とする。
                        CTSettings.scansel.Data.multi_tube = 0;
                    }
                    else
                    {
                        CTSettings.scansel.Data.multi_tube = (int)value;
                        //Rev23.10 追加 by長野 2015/11/24
                        ComLib.change_xray_com((int)value);
                        modCT30K.PauseForDoEvents(0.1f);
                    }

                    CTSettings.scansel.Write();

                    //frmXrayControl.Instance.QueryWarmup();

                    //内部変数に記憶
                    myXrayMode = value;

                    changeXrayIsBusy = false;

                    frmMechaControl.Instance.cwbtnChangeXray[0].BackColor = (myXrayMode == XrayModeConstants.XrayMode_Xray1 ? Color.Lime : Color.Green);
                    frmMechaControl.Instance.cwbtnChangeXray[1].BackColor = (myXrayMode == XrayModeConstants.XrayMode_Xray2 ? Color.Lime : Color.Green);

                    XrayNoneLimitControl(value);

                }

            }
		}

        #region //Rev23.10 不要 by長野 2015/09/20
        //*************************************************************************************************
        //機　　能： X線1の状態かチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:CT   False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v23.10 2015/09/20 (検S1)長野   新規作成
        //*************************************************************************************************
		public static bool IsXray1mode {


			get { return (XrayMode == XrayModeConstants.XrayMode_Xray1); }
		}
        //*************************************************************************************************
        //機　　能： X線２の状態かチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:高速撮影   False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v23.10 2015/09/20 (検S1)長野   新規作成
        //*************************************************************************************************
		public static bool IsXray2mode {


            get { return (XrayMode == XrayModeConstants.XrayMode_Xray2); }
		}

        //*************************************************************************************************
        //機　　能： X線位置不定かチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:不定   False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v17.20 2010/08/30 (検S1)長野   新規作成
        //*************************************************************************************************
		public static bool IsUnknownXraytMode {


            get { return (XrayMode == XrayModeConstants.XrayMode_None); }
		}
        #endregion
        //*************************************************************************************************
        //機　　能： X線１，２いずれかが，撮影位置にあるかチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:撮影位置にある   False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v23.10 2015/09/20 (検S1)長野   新規作成
        //*************************************************************************************************
		public static bool IsOKXrayPos {
			
            get {

				bool functionReturnValue = false;
				var _with2 = modSeqComm.MySeq;

                if (CTSettings.ChangeXrayOn == true)
                {
                    functionReturnValue = ((_with2.stsMicroFPDPos) || (_with2.stsMicroFPDShiftPos) || (_with2.stsNanoFPDPos) || (_with2.stsNanoFPDShiftPos));
                }
                //Rev23.11 条件追加 by長野 2015/12/28 --->
                else
                {
                    functionReturnValue = true;
                }
                //<---
                return functionReturnValue;
			}
		}

        //*************************************************************************************************
        //機　　能： X線切替動作中かチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:切替中   False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v23.10 2015/09/20 (検S1)長野   新規作成
        //*************************************************************************************************
		public static bool IsChangingXray {
			
            get
            {
				bool functionReturnValue = false;
				var _with3 = modSeqComm.MySeq;

				functionReturnValue = (_with3.stsNanoFPDBusy || _with3.stsMicroFPDBusy);
				return functionReturnValue;
			}
		}

        //*************************************************************************************************
        //機　　能： 他のダイアログが開いていないかチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v23.10 2015/09/20 (検S1)長野   新規作成
        //*************************************************************************************************
		public static bool IsAllCloseFrm {
			get {
				bool functionReturnValue = false;

				functionReturnValue = false;

				//const short MAX_LENGTH = 30;

				string msgStr = null;
				System.Windows.Forms.Form theForm = null;

				msgStr = "";

				foreach (Form theForm_loopVariable in Application.OpenForms)
                {
					theForm = theForm_loopVariable;
					//フォームのオブジェクト名（インデントを揃える）・キャプションをセット
					msgStr = theForm.Name;

					if (msgStr == "frmCTMenu" | 
                        msgStr == "frmTransImageInfo" | 
                        msgStr == "frmTransImageControl" | 
                        msgStr == "frmTransImage" | 
                        msgStr == "frmMechaControl" | 
                        msgStr == "frmScanControl" | 
                        msgStr == "frmScanImage" | 
                        msgStr == "frmXrayControl" | 
                        msgStr == "frmImageControl" | 
                        msgStr == "frmVirtualSeqComm" |
                        msgStr == "frmVirtualXrayControl" | 
                        msgStr == "frmScanImage" | 
                        msgStr == "frmStatus" | 
                        msgStr == "frmCorrectionStatus" | 
                        msgStr == "frmImageInfo" | 
                        msgStr == "frmXrayWarning" | 
                        msgStr == "frmAdjAlignment" | 
                        msgStr == "frmMechaReset" | 
                        msgStr == "frmMechaAllResetMove") {
					
                    
                    } else {
						return functionReturnValue;
						//Exit Property
					}

				}

				//    '表示
				//    MsgBox msgStr, , App.Title & " - " & mnuDebugSub(0).Caption

				functionReturnValue = true;
				return functionReturnValue;

			}
		}

        //*************************************************************************************************
        //機　　能： X線切替可否
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:移動可能   False:移動禁止
        //
        //補　　足： なし
        //
        //履　　歴： v23.10 2015/09/20 (検S1)長野   新規作成
        //*************************************************************************************************
		public static bool IsChangeXray()
		{
			bool functionReturnValue = false;

			functionReturnValue = false;

			//CTBusy状態なら無効
            if (!CTSettings.ChangeXrayOn)
            {
				if (Convert.ToBoolean(modCTBusy.CTBusy))
					return functionReturnValue;
			}

			//切替中なら無効
			if (IsChangingXray)
				return functionReturnValue;

            //X線ON中は切替不可
            if (!CTSettings.ChangeXrayOn)
            {
				if (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus)
					return functionReturnValue;
			}

			functionReturnValue = true;
			return functionReturnValue;

		}

        //*************************************************************************************************
        //機　　能： 校正ステータスを全て未完する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v23.10 2015/09/20 (検S1)長野   新規作成
        //*************************************************************************************************
		public static void ResetCorrectStatus()
		{
			var _with4 = CTSettings.mecainf.Data;

			_with4.normal_rc_cor = 0;
			//回転中心校正ステータス（ノーマル）
			_with4.cone_rc_cor = 0;
			//回転中心校正ステータス（コーン）
			_with4.gain_iifield = -1;
			//ゲイン校正I.I.視野
			_with4.gain_kv = 0.0f;
			//ゲイン校正管電圧
			_with4.gain_date = 0;
			//ゲイン校正年月日
			_with4.gain_mt = -1;
			//ゲイン校正Ｘ線管
			_with4.gain_filter = 0;
			//ゲイン校正フィルタ
			_with4.gain_bin = -1;
			//ゲイン校正実行時のビニングモード
			_with4.sp_iifield = -1;
			//スキャン位置校正I.I.視野
			_with4.sp_mt = -1;
			//スキャン位置校正Ｘ線管
			_with4.sp_bin = -1;
			//スキャン位置校正実行時のビニングモード
			_with4.ver_iifield = -1;
			//幾何歪校正I.I.視野
			_with4.ver_mt = -1;
			//幾何歪校正Ｘ線管
			_with4.ver_bin = -1;
			//幾何歪校正実行時のビニングモード
			_with4.off_date = 0;
			//オフセット校正年月日
			_with4.off_bin = -1;
			//オフセット校正実行時のビニングモード
			_with4.dc_iifield = -1;
			//寸法校正I.I.視野
			_with4.dc_mt = -1;
			//寸法校正Ｘ線管
			_with4.dc_bin = -1;
			//寸法校正実行時のビニングモード
			_with4.dc_rs = -1;
			//寸法校正回転選択ステータス
			_with4.dc_date = 0;
			//寸法校正年月日
			_with4.dc_time = 0;
			//寸法校正時間

			//modMecainf.PutMecainf(ref modMecainf.mecainf);
            CTSettings.mecainf.Write();

			//校正ステータスの ***移動をすべて移動ありにするための処理
			modSeqComm.SeqBitWrite("GainIIChangeSet", true);
			modSeqComm.SeqBitWrite("VerIIChangeSet", true);
			modSeqComm.SeqBitWrite("RotXChangeSet", true);
			modSeqComm.SeqBitWrite("RotYChangeSet", true);
			modSeqComm.SeqBitWrite("RotIIChangeSet", true);
			modSeqComm.SeqBitWrite("DisIIChangeSet", true);
			modSeqComm.SeqBitWrite("DisXChangeSet", true);
			modSeqComm.SeqBitWrite("DisYChangeSet", true);
			modSeqComm.SeqBitWrite("SPIIChangeSet", true);

			//スキャン校正画面の更新
			frmCorrectionStatus.Instance.MyUpdate();
            frmCorrectionStatus.Instance.UpdateRCStatus();

		}

        //*************************************************************************************************
        //機　　能： X線切替完了後，フォームをリロードする。
        //
        //           変数名          [I/O] 型            内容
        //引　　数：
        //戻 り 値： なし
        //
        //補　　足： この処理ではフォームのリロードおよび再表示を
        //
        //履　　歴： v23.10 2015/09/20 (検S1)長野   新規作成
        //*************************************************************************************************
        //Public Function ReloadForms()
		public static object ReloadForms(bool WarningDisp = true)
		{

#region <この処理はデバッグが必要です。正常動作しないかもしれない   2014/10/15hata

            object functionReturnValue = null;

			//↓v17.51追加 by 間々田 2011/03/24
			//注意： この処理ではフォームのリロードおよび再表示を行っている。
			//       そのため、モーダル表示しているとエラーとなる。
			//       それを回避するため、モーダル表示している画面があれば、それを閉じるよう促す

			//エラー時の扱い
			 // ERROR: Not supported in C#: OnErrorStatement

            //try
            //{
            //    //以下の処理でエラーが発生する場合、モーダルで表示している画面があるということ
            //    frmCTMenu.Instance.Show();
            //}
            //catch
            //{

            //    //閉じるよう促すメッセージを以前表示していなければ表示する
            //    if (WarningDisp) {
            //        //            MsgBox "検出器を切り替えたので、画面のリフレッシュを行います。" & vbCr & _
            //        //'                   "現在、実行している処理を中止し、表示しているダイアログ・メッセージボックス等を閉じてください。", vbExclamation
            //        //v17.60 ストリングテーブル化 by長野 2011/05/30
            //        MessageBox.Show(CTResources.LoadResString(20178) + "\r\n" + CTResources.LoadResString(20179), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    }

            //    //あとでこの関数を呼べるようにタイマー起動
            //    frmMechaControl.Instance.tmrTryReloadForms.Enabled = true;
            //    return functionReturnValue;

            //}

            frmXrayControl.Instance.tmrUpdate.Enabled = false;
            modMechaControl.Flg_SeqCommUpdate = false;
            modMechaControl.Flg_MechaControlUpdate = false;
            if (frmAdjAlignment.Instance != null)
            {
                frmAdjAlignment.Instance.tmrAdjAlignment.Enabled = false;
                frmAdjAlignment.Instance.Dispose();
            }
            if (frmMechaControl.Instance != null)
            {
                frmMechaControl.Instance.tmrMecainfSeqComm.Enabled = false;
            }
            


			//マウスポインタを砂時計にする前にバックアップを取る     'v17.50追加 by 間々田 2011/03/18
			Cursor MousePointerBak = null;
			MousePointerBak = System.Windows.Forms.Cursor.Current;

			//マウスポインタを砂時計に変更
			Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

			ReloadFlag = false;

            ////リロードする前に一旦、両方のボタンを消灯
            //frmMechaControl.Instance.cwbtnChangeXray[0].Value = false;
            //frmMechaControl.Instance.cwbtnChangeXray[0].BlinkInterval = CWSpeeds.cwSpeedOff;
            //frmMechaControl.Instance.cwbtnChangeXray[1].Value = false;
            //frmMechaControl.Instance.cwbtnChangeXray[1].BlinkInterval = CWSpeeds.cwSpeedOff;

			//CTmenuに全体に制限
			frmCTMenu.Instance.Enabled = false;

            //追加2015/01/09hata
            modCT30K.CT30KSetup = false; //起動未完了

			//ここから 透視画像関係のフォームをアンロード
            modXrayControl.XrayControlStart(false);
            modCT30K.PauseForDoEvents(0.5f);
            frmXrayControl.Instance.Close();
			frmXrayControl.Instance.Dispose();

            modCT30K.PauseForDoEvents(3);

            //ここから X線関係のフォームをロード
            //frmXrayControl.Instance.Form_Load();

#if XrayDebugOn

            //デバッグ時は仮想Ｘ線制御とする by 間々田 2004/11/29
			modXrayControl.XrayControl = new frmVirtualXrayControl();
#else
            modXrayControl.XrayControl = new XrayCtrl.clsTActiveX();
#endif

            modCT30K.HideMessage();

            modCT30K.ShowMessage(CTResources.LoadResString(23012));

            modCT30K.PauseForDoEvents(3);

            try
            {
                frmCTMenu.Instance.ShowChild(frmXrayControl.Instance, 0, frmMechaControl.Instance.Bottom);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            modCT30K.HideMessage();

            modCT30K.PauseForDoEvents(1);

			ReloadFlag = true;

			//Ｘ線条件設定処理
			int XrayCondIndex = 0;
            if (modLibrary.IsExistForm(frmXrayControl.Instance))
            {
                XrayCondIndex = modLibrary.GetCmdButton(frmXrayControl.Instance.cmdCondition);
				//frmXrayControl.SetXrayCondition XrayCondIndex, myIIField, frmMechaControl.FIDWithOffset
                frmXrayControl.Instance.SetXrayCondition(XrayCondIndex, modSeqComm.GetIINo(), frmMechaControl.Instance.FIDWithOffset);
                frmXrayControl.Instance.SetFilter(XrayCondIndex, modSeqComm.GetIINo());  //追加 2009/08/19
			}

            //追加2015/01/09hata
            modCT30K.CT30KSetup = true; //起動完了
            modMechaControl.Flg_SeqCommUpdate = true;
            modMechaControl.Flg_MechaControlUpdate = true;
            frmXrayControl.Instance.tmrUpdate.Enabled = true;
            if (frmMechaControl.Instance != null)
            {
                frmMechaControl.Instance.tmrMecainfSeqComm.Enabled = true;
            }
            

			//CTmenuに全体の制限を解除
            frmCTMenu.Instance.Enabled = true;

			//マウスポインタを矢印に戻す
			//Screen.MousePointer = vbDefault
			Cursor.Current = MousePointerBak;
       
            return functionReturnValue;			//v17.50変更 by 間々田 2011/03/18 バックアップしたマウスポインタに戻す

#endregion

        
        }

        //*******************************************************************************
        //機　　能： X線が移動完了するのを待つ
        //
        //           変数名          [I/O] 型        内容
        //引　　数：                 [I/ ]
        //
        //戻 り 値：                 [ /O] Boolean   True:移動成功   False:失敗
        //
        //補　　足： なし
        //
        //履　　歴： v23.10 2015/09/20   (検S1)長野    新規作成
        //*******************************************************************************
		public static bool WaitForChangeXray(modDetShift.DetShiftConstants detpos)
		{
			bool functionReturnValue = false;

            DateTime StartTime;			//開始時間(秒)
            double PauseTime = 0;		//待ち時間(秒)

			//初期値設定
			functionReturnValue = false;

			//タイムアウトの設定(秒)
			PauseTime = 30;

			//開始時間を設定
			StartTime = DateTime.Now;

			//切り替え完了待ち
			do {

				//1秒おきにチェック
				modCT30K.PauseForDoEvents(1);

				//タイムアウト
                //if (DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Second, StartTime, DateAndTime.Now) > PauseTime)
                if ((DateTime.Now - StartTime).TotalSeconds > PauseTime)
					return functionReturnValue;

				//指定位置に達している
				if ((modDetShift.IsDetShiftPos == detpos))
					break; // TODO: might not be correct. Was : Exit Do

			} while (true);

			//移動完了
			functionReturnValue = true;
			return functionReturnValue;

		}
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここまで
        //*******************************************************************************
        //機　　能： ２台のX線を順にウォームアップ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.10  15/10/06  (検S1)長野    新規作成
        //*******************************************************************************
        public static int PackageWarmup()
        {
            //２台のX線を順にウォームアップします。
            bool Xray1WUP_sucess = false;
            bool Xray2WUP_sucess = false;

            DialogResult result = MessageBox.Show(CTResources.LoadResString(23022), Application.ProductName,
                                             MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            //DialogResult result = MessageBox.Show(StringTable.GetResString(9609, cwneWarmupSetVolt.Text), Application.ProductName,
            //                                     MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No)
            {
                return -1;
            }

            myPackageWUPFlg = true;

            if (IsWarmUpBusy == false)
            {
                IsWarmUpBusy = true;
                
                mod2ndXray.XrayModeConstants xray_no_org = 0;//実行時のX線
                mod2ndXray.XrayModeConstants xray_no_new = 0;//２番目にWUPを行うX線

                xray_no_org = mod2ndXray.XrayMode;

                if (xray_no_org == mod2ndXray.XrayModeConstants.XrayMode_None)
                {
                    myPackageWUPFlg = false;
                    return -1;
                }

                if (xray_no_org == mod2ndXray.XrayModeConstants.XrayMode_Xray1)
                {
                    xray_no_new = mod2ndXray.XrayModeConstants.XrayMode_Xray2;
                }
                else
                {
                    xray_no_new = mod2ndXray.XrayModeConstants.XrayMode_Xray1;
                }

                if (frmAdjAlignment.Instance != null)
                {
                    frmAdjAlignment.Instance.Close();
                    frmAdjAlignment.Instance.Dispose();
                }

                if (modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpNotComplete)
                {

                    //230kV or 300kVはステップWUP
                    //if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721) || (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801))
                    //{
                    //    frmXrayControl.Instance.chkStepWU.Checked = true;
                    //}

                    frmXrayControl.Instance.cmdWarmupStart_Click(null, EventArgs.Empty);

                    //ステータスがWUP中から変化したタイミングが終了となるので、WUP中になるまで少し待つ
                    modCT30K.PauseForDoEvents(10);

                    while (modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpNow)
                    {
                        modCT30K.PauseForDoEvents(1);
                    }

                    modCT30K.PauseForDoEvents(1);

                    if (modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete)
                    {
                        Xray1WUP_sucess = true;
                    }
                }
                else if (modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete)
                {
                    Xray1WUP_sucess = true;
                }

                //１台目が成功のときだけ２台目を連続して行う。
                if (Xray1WUP_sucess == true)
                {

                    //制御を２台目に移す
                    mod2ndXray.XrayMode = xray_no_new;

                    modCT30K.PauseForDoEvents(1);

                    if (modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpNotComplete)
                    {
                        //230kV or 300kVはステップWUP
                        if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721) || (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801))
                        {
                            frmXrayControl.Instance.chkStepWU.Checked = true;
                        }
                        frmXrayControl.Instance.cmdWarmupStart_Click(null, EventArgs.Empty);

                        //ステータスがWUP中から変化したタイミングが終了となるので、WUP中になるまで少し待つ
                        modCT30K.PauseForDoEvents(5);

                        while (modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpNow)
                        {
                            modCT30K.PauseForDoEvents(1);
                        }

                        modCT30K.PauseForDoEvents(1);

                        if (modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete)
                        {
                            Xray2WUP_sucess = true;
                        }
                        //if (Xray2WUP_sucess == false)
                        //{
                        //    //失敗の場合はメッセージ表示
                        //    MessageBox.Show(StringTable.GetResString(23020, CTSettings.infdef.Data.multi_tube[CTSettings.scansel.Data.multi_tube].GetString()));
                        //    return -1;
                        //}
                    }
                    else if (modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpComplete)
                    {
                        Xray2WUP_sucess = true;
                    }
                }
                //else
                //{
                //    //失敗の場合はメッセージ表示
                //    MessageBox.Show(StringTable.GetResString(23020,CTSettings.infdef.Data.multi_tube[CTSettings.scansel.Data.multi_tube].GetString()));
                //    return -1;
                //}

                //Rev23.10 制御を最初のX線に戻す by長野 2015/10/05
                //制御を２台目に移す
                //処理の結果にかかわらず、必ず最初に戻す
                mod2ndXray.XrayMode = xray_no_org;

                modCT30K.PauseForDoEvents(3);

                myPackageWUPFlg = false;
                IsWarmUpBusy = false;

                return 0;
            }
            return -1;
        }
        //*************************************************************************************************
        //機　　能： X線不定の時、コントロールに制限を設ける(X線切替用)
        //
        //           変数名          [I/O] 型            内容
        //引　　数： theMode         DetModeConstants     I.I.の状態
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v23.10 2015/11/08 (検S1)長野   新規作成
        //*************************************************************************************************
        public static void XrayNoneLimitControl(XrayModeConstants theMode)
        {

            //short i = 0;//ループ用変数
            bool IsNotUnknown = false;
            IsNotUnknown = !(theMode == XrayModeConstants.XrayMode_None);

            //CTメニューツールバー
            var _with5 = frmCTMenu.Instance.Toolbar1;

            _with5.Items["tsbtnXrayAllWup"].Enabled = IsNotUnknown;
            _with5.Items["tsbtnXray"].Enabled = IsNotUnknown;
            _with5.Items["tsbtnLiveImage"].Enabled = IsNotUnknown;

            frmScanControl.Instance.Enabled = IsNotUnknown;
            frmScanControl.Instance.SSTab1.Enabled = IsNotUnknown;

            //frmScanControl.Instance.InitTabEnable(0, IsNotUnknown);
            //frmScanControl.Instance.InitTabEnable(1, IsNotUnknown);
            //frmScanControl.Instance.InitTabEnable(2, IsNotUnknown);

            //スキャンスタートボタンは非表示に
            frmScanControl.Instance.ctbtnScanStop.Enabled = IsNotUnknown;
            frmScanControl.Instance.ctbtnScanStart.Enabled = IsNotUnknown;

            frmXrayControl.Instance.Enabled = IsNotUnknown;
            frmXrayControl.Instance.cmdDetail.Enabled = IsNotUnknown;

        }
    }
}
