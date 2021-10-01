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
	static class mod2ndDetctor
	{
        ///* ************************************************************************** */
        ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
        ///* 客先　　　　： ?????? 殿                                                   */
        ///* プログラム名： mod2ndDetctor.bas                                           */
        ///* 処理概要　　： 検出器切替機能関連モジュール                                */
        ///* 注意事項　　： なし                                                        */
        ///* -------------------------------------------------------------------------- */
        ///* 適用計算機　： DOS/V PC                                                    */
        ///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
        ///* コンパイラ　： VB 6.0                                                      */
        ///* -------------------------------------------------------------------------- */
        ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
        ///*                                                                            */
        ///* v17.20      10/08/30    (検S1)長野          新規作成                       */
        ///*                                                                            */
        ///* -------------------------------------------------------------------------- */
        ///* ご注意：                                                                   */
        ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
        ///*                                                                            */
        ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2010                 */
        ///* ************************************************************************** */


        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************

        //CT30K起動判定 v17.20 追加 by 長野 10-08-30
		private static bool IsDetStartUp;

        //シーケンサ動作停止要求フラグ           'v17.50削除 by 間々田 2011/03/17 frmMechaAllResetMoveに移動
        //Public MechaResetStopflg As Boolean

        //検出器切替の場合，非常停止が発生したことを示すmsgboxでＯＫをクリックしたことを示すフラグを用意
        //通常はTrueにしておき，非常停止ボタンが押されたときだけFalseに変更
		public static bool MsgBoxOK;
        
        //CTソフトを再表示中、アンロードしたフォームを参照しにいかないようにフラグで管理する。
		public static bool ReloadFlag;
        
        //検出器の状態定数
		public enum DetModeConstants
		{
			DetMode_Det1 = 0,		//検出器１
			DetMode_Det2 = 1,		//検出器２
			DetMode_None = 2        //いずれでもない
		}
		private static DetModeConstants myDetMode;


        //*************************************************************************************************
        //機　　能： 起動時の処理
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.20 2010/09/08 (検S1)長野   新規作成
        //*************************************************************************************************
        //Public Sub SecondDetctorStartUpProcess()
		public static bool SecondDetctorStartUpProcess()
		{
			bool functionReturnValue = false;
			//v17.51変更 by 間々田 2011/03/23

			//戻り値初期化                       'v17.51追加 by 間々田 2011/03/23
			functionReturnValue = false;

			IsDetStartUp = false;
			short det_no = 0;
			MsgBoxOK = true;
			var _with1 = modSeqComm.MySeq;


			//現在の検出器位置とコモン（scansel.com_detector_no）が異なる場合は、全校正未完とする。
			if (CTSettings.scansel.Data.com_detector_no != (int)DetMode) {

				ResetCorrectStatus();

			}

			//検出器の位置を確認
			//検出器１の時
			if (_with1.stsCTIIPos) {
				det_no = 0;
				DetMode = DetModeConstants.DetMode_Det1;
				CTSettings.scansel.Data.com_detector_no = 0;
			//検出器２の時
			} else if (_with1.stsTVIIPos) {
				det_no = 1;
				DetMode = DetModeConstants.DetMode_Det2;
				CTSettings.scansel.Data.com_detector_no = 1;
				//ElseIf Not (.stsCTIIDrive Or .stsCTIIPos Or .stsTVIIDrive Or .stsTVIIPos) Then '検出器位置不定の場合なにもしない
			//v17.51変更 by 間々田 2011/03/23 検出器の状態が必ず検出器１／検出器２／不定のいずれかにするように変更
			} else {
				DetMode = DetModeConstants.DetMode_None;
				DetNoneLimitControl((DetMode));
				IsDetStartUp = true;

                //検出器位置不定の場合はメカオールリセットを促す    'v17.51追加 by 間々田 2011/03/23
				//CT30K.My.MyProject.Forms.frmMechaAllResetMove.ShowAtStartup();
                frmMechaAllResetMove.Instance.ShowAtStartup();
				
                return functionReturnValue;
                //ここで抜ける

            }

            //現在の検出器にコモンを入れ替える。
			ComLib.get_det_com(det_no);

			//X線更新用タイマーオフ
			frmXrayControl.Instance.tmrUpdate.Enabled = false;

			//入れ替えた直後にコモンを読み込んでおく
			modCT30K.GetCommon();

			//検出器に合わせてGUIを更新する。
			ReloadForms();

            frmXrayControl.Instance.tmrUpdate.Enabled = true;

			ReloadFlag = true;
			IsDetStartUp = true;

			//戻り値セット                    'v17.51追加 by 間々田 2011/03/23
			functionReturnValue = true;
			return functionReturnValue;

		}
        //*******************************************************************************
        //機　　能： DetModeプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.20  10/08/30  長野      新規作成
        //*******************************************************************************
		public static DetModeConstants DetMode {


			get { return myDetMode; }
			set
            {

                //検出器切替機能が無効の時は、常に検出器１
				if (!CTSettings.SecondDetOn) {
					myDetMode = DetModeConstants.DetMode_Det1;
					return;
				}

				//検出器が切り替わったらコモンの入れ替えとメニュー画面の更新をする
                if (CTSettings.SecondDetOn & (DetMode != value) & IsDetStartUp & !(value == DetModeConstants.DetMode_None))
                {

					//内部変数に記憶
					myDetMode = value;

					ComLib.get_det_com((int)value);

					//入れ替え後のコモンを読み込む。
					modCT30K.GetCommon();

					//全校正を未完にする
					ResetCorrectStatus();

					//メニュー画面を更新する
					ReloadForms();

 
				}
                

				//検出器状態が不定の場合､フォームにロックをかける
				DetNoneLimitControl((value));

				//内部変数に記憶
				myDetMode = value;

			}
		}

        //*************************************************************************************************
        //機　　能： 検出器１の状態かチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:CT   False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v17.20 2010/08/30 (検S1)長野   新規作成
        //*************************************************************************************************
		public static bool IsDet1mode {


			get { return (DetMode == DetModeConstants.DetMode_Det1); }
		}
        //*************************************************************************************************
        //機　　能： 検出器２の状態かチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:高速撮影   False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v17.20 2010/08/30 (検S1)長野   新規作成
        //*************************************************************************************************
		public static bool IsDet2mode {


			get { return (DetMode == DetModeConstants.DetMode_Det2); }
		}

        //*************************************************************************************************
        //機　　能： 検出器位置不定かチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:不定   False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v17.20 2010/08/30 (検S1)長野   新規作成
        //*************************************************************************************************
		public static bool IsUnknownDetMode {


			get { return (DetMode == DetModeConstants.DetMode_None); }
		}
        //*************************************************************************************************
        //機　　能： 検出器１，２いずれかが，撮影位置にあるかチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:撮影位置にある   False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v17.20 2010/08/30 (検S1)長野   新規作成
        //*************************************************************************************************
		public static bool IsOKDetPos {
			get {
				bool functionReturnValue = false;
				var _with2 = modSeqComm.MySeq;


				//IsOKIIPos = (IsCTmode And (.stsCTIIPos)) Or _
				//'            (IsHSCmode And (.stsTVIIPos))
				//シーケンサソフトが古い(stsCTIIPos,stsTVIIPosがない)場合はTrue  'v17.10変更 byやまおか 2010/07/28
				//If (scaninh.second_detector = 0) Or DetShiftOn Then 'v18.00変更 byやまおか 2011/08/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
				//v19.50 条件追加 by長野 2014/02/15
                if (CTSettings.scaninh.Data.second_detector == 0)
                {
					functionReturnValue = (IsDet1mode & (_with2.stsCTIIPos)) | (IsDet2mode & (_with2.stsTVIIPos));

                //Rev25.00 Wスキャン追加 by長野 2016/06/19
				//} else if (CTSettings.DetShiftOn == true) {
                }else if(CTSettings.DetShiftOn || CTSettings.W_ScanOn){
					//functionReturnValue = _with2.stsCTIIPos | _with2.stsTVIIPos;
                    //Rev23.20 追加 by長野 2015/12/21
                    functionReturnValue = _with2.stsCTIIPos | _with2.stsTVIIPos | _with2.stsFPDLShiftPos;

				} else {

					functionReturnValue = true;

				}
				return functionReturnValue;


			}
		}

        //*************************************************************************************************
        //機　　能： 検出器切替動作中かチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:切替中   False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v17.20 2010/08/30 (検S1)長野   新規作成
        //*************************************************************************************************
		public static bool IsSwtichingDet {
			get {
				bool functionReturnValue = false;
				var _with3 = modSeqComm.MySeq;

				//functionReturnValue = (_with3.stsCTIIDrive | _with3.stsTVIIDrive);
                //Rev23.10 条件追加 X線切替軸がシフト軸と同じため by長野 2015/09/20
                if(CTSettings.scaninh.Data.cm_mode == 0)
                {
                    functionReturnValue = (_with3.stsMicroFPDShiftBusy | _with3.stsNanoFPDShiftBusy);
                }
                else
                {
                    functionReturnValue = (_with3.stsCTIIDrive | _with3.stsTVIIDrive);
                }

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
        //履　　歴： v17.20 2010/08/30 (検S1)長野   新規作成
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
        //機　　能： 検出器移動可否
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:移動可能   False:移動禁止
        //
        //補　　足： なし
        //
        //履　　歴： v17.20 2010/08/30 (検S1)長野   新規作成
        //*************************************************************************************************
        public static bool IsSwitchDet()
        {
            bool functionReturnValue = false;

            functionReturnValue = false;

            //CTBusy状態なら無効
            //If CTBusy Then Exit Function
            //シフトスキャンの場合はCTBusyでも切り替え可能 'v18.00変更 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //If (Flg_GainShiftScan <> vbChecked) Then
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //if (!CTSettings.DetShiftOn)
            if (!(CTSettings.DetShiftOn || CTSettings.W_ScanOn))
            {
                if (Convert.ToBoolean(modCTBusy.CTBusy))
                    return functionReturnValue;
            }

            //切替中なら無効
            if (IsSwtichingDet)
                return functionReturnValue;

            //X線ON中は不可
            //If frmXrayControl.MecaXrayOn = OnStatus Then Exit Function
            //シフトスキャンの場合はON中でも切り替え可能 'v18.00変更 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //if (!CTSettings.DetShiftOn)
            if (!(CTSettings.DetShiftOn || CTSettings.W_ScanOn))
            {
                if (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus)
                    return functionReturnValue;
            }

            functionReturnValue = true;
            return functionReturnValue;

        }

        //*************************************************************************************************
        //機　　能： 検出器切替処理
        //
        //           変数名          [I/O] 型            内容
        //引　　数： theMode         IIModeConstants     I.I.状態
        //
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.20 2010/08/30 (検S1)長野   新規作成
        //           v17.50 2010/08/30 (豊川)間々田 廃止。modSeqCommに移動
        //*************************************************************************************************
        //Public Function SwitchDet(ByVal theMode As DetModeConstants) As Boolean
        //
        //    Dim Is_Det1 As Boolean
        //    Dim Is_Det2 As Boolean
        //
        //    Dim theCommand As String
        //
        //    '初期値設定
        //    SwitchDet = False
        //
        //    Is_Det1 = (theMode = DetMode_Det1)
        //    Is_Det2 = (theMode = DetMode_Det2)
        //
        //    'I.I.の移動可否判定
        //    If Not IsSwitchDet() Then Exit Function
        //
        //    'シーケンサに送るコマンドをセット
        //    If Is_Det1 Then
        //        theCommand = "CTIISet"
        //    ElseIf Is_Det2 Then
        //        theCommand = "TVIISet"
        //    End If
        //
        //    '撮影位置から外れる検出器電源をOFF(フラットパネルの場合、電源をOFFにしない)
        //    If Is_Det1 Then
        //
        //        If Not Use_FlatPanel Then
        //            Call SeqBitWrite("TVIIPowerOff", True)
        //        End If
        //
        //    ElseIf Is_Det2 Then
        //
        //        If Not Use_FlatPanel Then
        //            Call SeqBitWrite("IIPowerOff", True)
        //        End If
        //
        //    End If
        //
        //    '検出器切替
        //    If Not ChangeDet(theCommand) Then Exit Function
        //
        //    '切替成功
        //    SwitchDet = True
        //
        //End Function

        //*************************************************************************************************
        //機　　能： 校正ステータスを全て未完する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.20 2010/08/30 (検S1)長野   新規作成
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
        //機　　能： 検出器切替完了後，フォームをリロードする。
        //
        //           変数名          [I/O] 型            内容
        //引　　数：
        //戻 り 値： なし
        //
        //補　　足： この処理ではフォームのリロードおよび再表示を
        //
        //履　　歴： v17.20 2010/08/31 (検S1)長野   新規作成
        //*************************************************************************************************
        //Public Function ReloadForms()
		public static object ReloadForms(bool WarningDisp = true)
		{

#region <この処理はデバッグが必要です。正常動作しないかもしれない   2014/10/15hata

            object functionReturnValue = null;
			//v17.51変更 by 間々田 2011/03/25

			//↓v17.51追加 by 間々田 2011/03/24
			//注意： この処理ではフォームのリロードおよび再表示を行っている。
			//       そのため、モーダル表示しているとエラーとなる。
			//       それを回避するため、モーダル表示している画面があれば、それを閉じるよう促す

			//エラー時の扱い
			 // ERROR: Not supported in C#: OnErrorStatement


            //エラー・クリア
			//Err().Clear();
            try
            {
			    //以下の処理でエラーが発生する場合、モーダルで表示している画面があるということ
			    frmCTMenu.Instance.Show();
            }
            catch
            {
			    //if (Err().Number != 0) {

				//閉じるよう促すメッセージを以前表示していなければ表示する
				if (WarningDisp) {
					//            MsgBox "検出器を切り替えたので、画面のリフレッシュを行います。" & vbCr & _
					//'                   "現在、実行している処理を中止し、表示しているダイアログ・メッセージボックス等を閉じてください。", vbExclamation
					//v17.60 ストリングテーブル化 by長野 2011/05/30
                    MessageBox.Show(CTResources.LoadResString(20178) + "\r\n" + CTResources.LoadResString(20179), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

				//あとでこの関数を呼べるようにタイマー起動
				frmMechaControl.Instance.tmrTryReloadForms.Enabled = true;
				return functionReturnValue;

			}

			//↑v17.51追加 by 間々田 2011/03/24

			//マウスポインタを砂時計にする前にバックアップを取る     'v17.50追加 by 間々田 2011/03/18
			Cursor MousePointerBak = null;
			MousePointerBak = System.Windows.Forms.Cursor.Current;

			//マウスポインタを砂時計に変更
			Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

			ReloadFlag = false;

			//↓v17.51削除 by 間々田 2011/03/24  上で同じことをしてるので不要になった
			//'v17.20 非常停止メッセージボックスが表示されている場合，メッセージボックスはモーダルのため
			//'閉じないでフォームをロードをするとソフトがおちる。メッセージボックスを閉じるまでロードをさせないようにした
			//While Not MsgBoxOK
			//
			//     PauseForDoEvents 1
			//
			//Wend
			//↑v17.51削除 by 間々田 2011/03/24

			//リロードする前に一旦、両方のボタンを消灯
			frmMechaControl.Instance.cwbtnChangeDet[0].Value = false;
			frmMechaControl.Instance.cwbtnChangeDet[0].BlinkInterval = CWSpeeds.cwSpeedOff;
            frmMechaControl.Instance.cwbtnChangeDet[1].Value = false;
            frmMechaControl.Instance.cwbtnChangeDet[1].BlinkInterval = CWSpeeds.cwSpeedOff;

			//ここでUseFlatPanelを先行して更新しておく
            CTSettings.detectorParam.Use_FlatPanel = Convert.ToBoolean(CTSettings.scancondpar.Data.detector);

			//CTmenuに全体に制限
			frmCTMenu.Instance.Enabled = false;

            //追加2015/01/09hata
            modCT30K.CT30KSetup = false; //起動未完了

			//ここから 透視画像関係のフォームをアンロード
			frmTransImageControl.Instance.Close();
			frmTransImageInfo.Instance.Close();
			frmTransImage.Instance.Close();

			//スキャン校正画面を一旦アンロード
			frmCorrectionStatus.Instance.Close();
			frmScanControl.Instance.Close();

			//付帯情報表示アンロード
			frmImageInfo.Instance.Close();

			//メカコントロール画面をアンロード
			//Unload frmMechaControl

			//スキャン画像フォームをアンロード
			frmScanImage.Instance.Close();


            //起動から変更しないパラメータを再読込み
			CTSettings.SetFixedVariable();

			modCommon.GetMyScansel();

			//グロバール変数再読み込み
			modCT30K.InitGlobalVariables();

			//既存の文字列定数をリソースから読み込む
			StringTable.SetConstString();

			//v15.0追加 by 間々田 2009/04/10
			ScanCorrect.OptValueGet_Cor();

            //画面ロード
            //スキャン画像フォームをロード
            //Load(frmScanImage);
            frmCTMenu.Instance.ShowChild(frmScanImage.Instance, frmScanControl.Instance.Right, 0);
            Application.DoEvents();

			//スキャンコントロールをロード
            //Load(frmScanControl);
            frmCTMenu.Instance.ShowChild(frmScanControl.Instance, 0, frmXrayControl.Instance.Bottom);
            Application.DoEvents();

			//メカコントロールをロード
			//Load frmMechaControl
            frmMechaControl.Instance.Visible = true;
            frmMechaControl.Instance.SetCaption();
            frmMechaControl.Instance.InitControls();
            frmMechaControl.Instance.InitSeqComm();

            Application.DoEvents();

            //v17.30 FPD切り替えの場合は、ここで二次元幾何歪補正をする。 by 長野 2010-09-26
			//frmMechaControl.FPD_DistorsionCorrect
			if (CTSettings.detectorParam.Use_FlatPanel)
				ScanCorrect.FPD_DistorsionCorrect();
			//v18.00変更 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

            ////画面ロード
            ////frmScanImage.Instance.Show();
            ////frmScanControl.Instance.Show();
            ////frmMechaControl.Instance.Show();

			//透視画面ロード
            frmTransImage.Instance.Show(frmCTMenu.Instance);
            frmTransImageInfo.Instance.Show(frmCTMenu.Instance);
            frmTransImageControl.Instance.Show(frmCTMenu.Instance);
            
			//付帯情報ロード
			//Load(frmImageInfo);
			frmImageInfo.Instance.Show(frmCTMenu.Instance);
            frmImageInfo.Instance.Refresh();

			//撮影位置に移動した検出器電源をON
			if (IsDet1mode) {
				modSeqComm.SeqBitWrite("IIPowerOn", true);
			} else if (IsDet2mode) {
				modSeqComm.SeqBitWrite("TVIIPowerOn", true);
			}

			ReloadFlag = true;

			//v17.20 試料テーブル制限解除ボタンのステータスは、起動時一回しか取得しないため切替時にもう一度取得する。 by 長野 10-08-31
            frmMechaControl.Instance.cmdTableMovePermit.BackColor = (modSeqComm.MySeq.stsTableMovePermit ?Color.Lime : modCT30K.DarkGreen);         //解除
            frmMechaControl.Instance.cmdTableMoveRestrict.BackColor = (modSeqComm.MySeq.stsTableMovePermit ? modCT30K.DarkGreen :　Color.Lime);     //制限

			//Ｘ線条件設定処理
			int XrayCondIndex = 0;
            if (modLibrary.IsExistForm(frmXrayControl.Instance))
            {
                XrayCondIndex = modLibrary.GetCmdButton(frmXrayControl.Instance.cmdCondition);
				//frmXrayControl.SetXrayCondition XrayCondIndex, myIIField, frmMechaControl.FIDWithOffset
                frmXrayControl.Instance.SetXrayCondition(XrayCondIndex, modSeqComm.GetIINo(), frmMechaControl.Instance.FIDWithOffset);
                frmXrayControl.Instance.SetFilter(XrayCondIndex, modSeqComm.GetIINo());  //追加 2009/08/19
			}

			//メニュー画面更新
            frmCTMenu.Instance.InitControls();
            frmCTMenu.Instance.Toolbar1.Refresh();

            //追加2015/01/09hata
            modCT30K.CT30KSetup = true; //起動完了
            modMechaControl.Flg_SeqCommUpdate = true;
            modMechaControl.Flg_MechaControlUpdate = true;

			//CTmenuに全体の制限を解除
            frmCTMenu.Instance.Enabled = true;

			//マウスポインタを矢印に戻す
			//Screen.MousePointer = vbDefault
			Cursor.Current = MousePointerBak;
       
            return functionReturnValue;			//v17.50変更 by 間々田 2011/03/18 バックアップしたマウスポインタに戻す

#endregion

        
        }

        //*************************************************************************************************
        //機　　能： 検出器不定の時、コントロールに制限を設ける(検出器切替用)
        //
        //           変数名          [I/O] 型            内容
        //引　　数： theMode         DetModeConstants     I.I.の状態
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
		public static void DetNoneLimitControl(DetModeConstants theMode)
		{

			//short i = 0;//ループ用変数
			bool IsNotUnknown = false;
			IsNotUnknown = !(theMode == DetModeConstants.DetMode_None);

			//CT30Kが起動するまでは無効
			//If Not IsDetStartUp Then Exit Sub

			//エラーは無視する
			 // ERROR: Not supported in C#: OnErrorStatement


			//CTメニューツールバー
			var _with5 = frmCTMenu.Instance.Toolbar1;

            _with5.Items[1].Enabled = IsNotUnknown;			//X線
			//.Buttons(3).Enabled = True    'ドアロック(常に有効に)
			_with5.Items[5].Enabled = IsNotUnknown;			//I.I.視野切替
			_with5.Items[5].Visible = IsNotUnknown;			//I.I.視野切替
			_with5.Items[9].Enabled = IsNotUnknown;			//カメラ電源
			_with5.Items[9].Visible = IsNotUnknown;			//カメラ電源
			//v17.20 画像、メンテ関係は使用可能とする by 長野 2010/09/20
			//        .Buttons(11).Enabled = IsNotUnknown     '開く
			//        .Buttons(13).Enabled = IsNotUnknown     '印刷
			//        .Buttons(15).Enabled = IsNotUnknown     'ダブルオブリーク
			//        .Buttons(17).Enabled = IsNotUnknown     'メンテナンス


			//v17.20 校正とスキャン条件設定画面を使用不可能に変更 by 長野 2010/09/20
			//スキャンコントロール
			//LimitControlOnForm frmScanControl, IsNotUnknown
			//frmScanControl.Enabled = IsNotUnknown
            //変更2015/01/27hata			       
            //frmScanControl.Instance.SSTab1.TabPages[0].Enabled = IsNotUnknown;
            //frmScanControl.Instance.SSTab1.TabPages[1].Enabled = IsNotUnknown;
            //frmScanControl.Instance.SSTab1.TabPages[2].Enabled = IsNotUnknown;
            frmScanControl.Instance.InitTabEnable(0, false);
            frmScanControl.Instance.InitTabEnable(1, false);
            frmScanControl.Instance.InitTabEnable(2, false);

			//v17.20 コントロールが制限されたらタブのアクティブインデックスは3（再々構成）に移す by 長野 2010/09/20
			if (theMode == DetModeConstants.DetMode_None) {

                frmScanControl.Instance.SSTab1.SelectedIndex = 3;

			} else {

                frmScanControl.Instance.SSTab1.SelectedIndex = 0;
			}

			//スキャンスタートボタンは非表示に
            frmScanControl.Instance.ctbtnScanStart.Visible = IsNotUnknown;

			//v17.20 使用可能とする by 長野 2010/09/20
			//画像サーチ&階調変換
			//    LimitControlOnForm frmImageControl, IsNotUnknown
			//    frmImageControl.Enabled = IsNotUnknown

			//v17.20 使用可能とする by 長野 2010/09/20
			//スキャン画像
			//    LimitControlOnForm frmScanImage, IsNotUnknown
			//    frmScanImage.Enabled = IsNotUnknown

			//v17.20 使用可能とする by 長野 2010/09/20
			//画像付帯情報
			//    LimitControlOnForm frmImageInfo, IsNotUnknown
			//    frmImageInfo.Enabled = IsNotUnknown
			//If Not IsD Then frmImageInfo.hide

			//v17.20 メカコントロールの使用も可能とする by 長野 2010/09/20
			//    'メカコントロール
			//    LimitControlOnForm frmMechaControl, IsNotUnknown
			//    With frmMechaControl
			//
			//        '検出器切替ボタンは常に有効
			//        .fraChangeDetector.Enabled = True
			//        For i = .cwbtnChangeDet.LBound To .cwbtnChangeDet.UBound
			//            .cwbtnChangeDet(i).Enabled = True
			//        Next
			//
			//        '詳細ボタンは常に有効
			//        .cmdDetails.Enabled = True
			//
			//        .cmdMechaAllReset.Enabled = True
			//
			//    End With

			//   'v17.20 メカ詳細(各軸メカリセット関係など)も使用可能とする by 長野 2010/09/20
			//メカ詳細
			//    LimitControlOnForm frmMechaReset, IsNotUnknown
			//    With frmMechaReset
			//
			//        '検出器切替リセットボタンは常に有効
			//        .fraDetChangeReset.Enabled = True
			//        .cmdDetChangeReset.Enabled = True
			//
			//        '閉じるボタンは常に有効
			//        .cmdClose.Enabled = True
			//
			//    End With

            //X線コントロール
            #region	    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            /*
            modHighSpeedCamera.LimitControlOnForm(ref frmXrayControl, IsNotUnknown);
            */
            #endregion

			frmXrayControl.Instance.Enabled = IsNotUnknown;
			frmXrayControl.Instance.cmdDetail.Enabled = IsNotUnknown;

			//v17.20 操作可能に変更 by 長野 2010/09/20
			//透視画面
			//    frmTransImage.Enabled = IsNotUnknown
			//    frmTransImageControl.Enabled = IsNotUnknown
			//    frmTransImageInfo.Enabled = IsNotUnknown
			//    LimitControlOnForm frmTransImageControl, IsNotUnknown

		}

        //v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここから
        //*******************************************************************************
        //機　　能： 検出器が移動完了するのを待つ
        //
        //           変数名          [I/O] 型        内容
        //引　　数：                 [I/ ]
        //
        //戻 り 値：                 [ /O] Boolean   True:移動成功   False:失敗
        //
        //補　　足： なし
        //
        //履　　歴： v18.00 2011/07/03   やまおか    新規作成
        //*******************************************************************************
		public static bool WaitForChangeDet(modDetShift.DetShiftConstants detpos)
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

    }
}
