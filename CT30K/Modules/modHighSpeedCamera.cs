//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;


namespace CT30K
{
	static class modHighSpeedCamera
	{
        ///* ************************************************************************** */
        ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
        ///* 客先　　　　： ?????? 殿                                                   */
        ///* プログラム名： modHighSpeedCamera.bas                                      */
        ///* 処理概要　　： 高速撮影切替機能関連モジュール                              */
        ///* 注意事項　　： なし                                                        */
        ///* -------------------------------------------------------------------------- */
        ///* 適用計算機　： DOS/V PC                                                    */
        ///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
        ///* コンパイラ　： VB 6.0                                                      */
        ///* -------------------------------------------------------------------------- */
        ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
        ///*                                                                            */
        ///* v16.01      10/02/02    (検SS)山影          新規作成                       */
        ///*                                                                            */
        ///* -------------------------------------------------------------------------- */
        ///* ご注意：                                                                   */
        ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
        ///*                                                                            */
        ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2009                 */
        ///* ************************************************************************** */


        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************

        //CT30K起動判定 v16.01 追加 by 山影 10-02-18
		static bool IsStartUp;

        //I.I.の状態定数
		public enum IIModeConstants
		{
            IIMode_CT = 0,      //CT
            //IIMode_HSC,			//高速透視
            //IIMode_None			//いずれでもない
            //Rev23.40 by長野 2016/04/05 //Rev23.21 変更 by長野 2016/03/02
            IIMode_HSC = 1,			//高速透視
            IIMode_DROP_TEST = 2,         //高速透視,落下試験機 //Rev26.40 add by chouno 2019/02/12
            IIMode_None = 3			//いずれでもない //Rev26.40 2->3に変更 by chouno 2019/02/12
		}
        //private static IIModeConstants myIIMode =IIModeConstants.IIMode_None;
        //Rev23.40/23/21 by長野 2016/04/05
        private static IIModeConstants myIIMode;

        //*******************************************************************************
        //機　　能： IIModeプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v16.01  10/02/04   山影      新規作成
        //*******************************************************************************

		public static IIModeConstants IIMode {


			get { return myIIMode; }
			set {

				//高速度透視撮影が無効の時は、常にCT
				if (!CTSettings.HscOn) {
					myIIMode = IIModeConstants.IIMode_CT;
					return;
				}

				//内部変数に記憶
				myIIMode = value;

				//高速撮影の時は、校正ステータスをクリア
				//if (myIIMode == IIModeConstants.IIMode_HSC)
                //Rev26.40 高速透視or落下試験 by chouno 2019/02/12
                if (myIIMode == IIModeConstants.IIMode_HSC || myIIMode == IIModeConstants.IIMode_DROP_TEST)
                    ResetCorrectStatus();

                //frmMechaControl.Instance.cwbtnChangeMode[0].BackColor = (myIIMode == IIModeConstants.IIMode_CT ? Color.Lime : Color.Green);
                //frmMechaControl.Instance.cwbtnChangeMode[1].BackColor = (myIIMode == IIModeConstants.IIMode_HSC ? Color.Lime : Color.Green);

                //Rev23.40 by長野 2016/04/05
                //コントロールに制限を設ける
                LimitControl(myIIMode);

                frmMechaControl.Instance.cwbtnChangeMode[0].BackColor = (value == IIModeConstants.IIMode_CT ? Color.Lime : Color.Green);
                //frmMechaControl.Instance.cwbtnChangeMode[1].BackColor = (value == IIModeConstants.IIMode_HSC ? Color.Lime : Color.Green);
                //Rev26.40 change by chouno 2019/02/17
                frmMechaControl.Instance.cwbtnChangeMode[1].BackColor = ((value == IIModeConstants.IIMode_HSC || value == IIModeConstants.IIMode_DROP_TEST) ? Color.Lime : Color.Green);

                //Rev26.40 位置次第で高速透視ボタン側の文字列を入れ替える by chouno 2019/02/12
                if (value == IIModeConstants.IIMode_HSC)
                {
                    frmMechaControl.Instance.cwbtnChangeMode[1].Caption = CTResources.LoadResString(StringTable.IDS_HSCmode);
                }
                else if (value == IIModeConstants.IIMode_DROP_TEST)
                {
                    frmMechaControl.Instance.cwbtnChangeMode[1].Caption = CTResources.LoadResString(StringTable.IDS_DropTestMode);
                }
			}
		}

        //*************************************************************************************************
        //機　　能： CT状態かチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:CT   False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
		public static bool IsCTmode {


			get { return (IIMode == IIModeConstants.IIMode_CT); }
		}
        //*************************************************************************************************
        //機　　能： 高速撮影状態かチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:高速撮影   False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
		public static bool IsHSCmode 
        {
			get { return (IIMode == IIModeConstants.IIMode_HSC); }
        }
        //*************************************************************************************************
        //機　　能： 高速撮影(落下試験機構)状態かチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:高速撮影   False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v26.40 2019/02/12 (検S1)長野   新規作成
        //*************************************************************************************************
        public static bool IsDropTestmode
        {
            get { return (IIMode == IIModeConstants.IIMode_DROP_TEST); }
        }
        //*************************************************************************************************
        //機　　能： I.I.位置不定かチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:不定   False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
		public static bool IsUnknownMode {


			get { return (IIMode == IIModeConstants.IIMode_None); }
		}
        //*************************************************************************************************
        //機　　能： I.I.が撮影位置にあるかチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:撮影位置にある   False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
		public static bool IsOKIIPos {
			get {
				bool functionReturnValue = false;
				var _with1 = modSeqComm.MySeq;
                var myscaninh = CTSettings.scaninh.Data;


				//IsOKIIPos = (IsCTmode And (.stsCTIIPos)) Or _
				//'            (IsHSCmode And (.stsTVIIPos))
				//シーケンサソフトが古い(stsCTIIPos,stsTVIIPosがない)場合はTrue  'v17.10変更 byやまおか 2010/07/28
                if ((myscaninh.high_speed_camera == 0))
                {
					//functionReturnValue = (IsCTmode & (_with1.stsCTIIPos)) | (IsHSCmode & (_with1.stsTVIIPos));
                    //Rev26.40 落下試験器の場合を追加 by chouno 2019/02/12
                    functionReturnValue = (IsCTmode & (_with1.stsCTIIPos)) | (IsHSCmode & (_with1.stsTVIIPos) || (IsDropTestmode & (_with1.stsFPDLShiftPos)));
                } 
                else 
                {
					functionReturnValue = true;
				}
				return functionReturnValue;

            }
		}

        //*************************************************************************************************
        //機　　能： I.I.切替動作中かチェック
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:切替中   False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
		public static bool IsIIChanging {
			get {
				bool functionReturnValue = false;
				var _with2 = modSeqComm.MySeq;

				functionReturnValue = (_with2.stsCTIIDrive ^ _with2.stsTVIIDrive);
				return functionReturnValue;


			}
		}

        //*************************************************************************************************
        //機　　能： X線I.I.移動可否
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値：                 Boolean             True:移動可能   False:移動禁止
        //
        //補　　足： なし
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
		public static bool IsOKIIMove()
		{
			bool functionReturnValue = false;

			functionReturnValue = false;

			//CTBusy状態なら無効
            if (modCTBusy.CTBusy != 0)
				return functionReturnValue;

            //Rev26.40 タッチパネル操作の場合は見ない by chouno 2019/02/21
            if (CTSettings.iniValue.HSCSettingType == 0)
            {
                //切替中なら無効
                if (IsIIChanging)
                    return functionReturnValue;
            }

			//X線ON中は不可
			if (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus)
				return functionReturnValue;

            //Rev23.40/23.21 by長野 2016/04/05
            //キャプチャON中は不可
            if (frmTransImage.Instance.CaptureOn == true)
            {
                return functionReturnValue;
            }

			functionReturnValue = true;
			return functionReturnValue;

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
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
		public static void HscStartUpProcess()
		{

			//CT30K起動
			IsStartUp = true;

			//CT30K起動時は高速カメラ電源オフ
			HSCameraPower(modCT30K.OnOffStatusConstants.OffStatus);

			//CT30K起動時は高速I.I.電源オフ
			HSIIPower(modCT30K.OnOffStatusConstants.OffStatus);
			var _with3 = modSeqComm.MySeq;


			//検出器の位置を確認
			//CT位置の時
			if (_with3.stsCTIIPos)
            {
				IIMode = IIModeConstants.IIMode_CT;
			//高速位置の時
			}
            else if (_with3.stsTVIIPos)
            {
				IIMode = IIModeConstants.IIMode_HSC;
				HSCameraPower(modCT30K.OnOffStatusConstants.OnStatus);
				//v16.02 追加 by 山影 10-03-02
			//いずれでもない時
			}
            else if (_with3.stsFPDLShiftPos) //Rev26.40 add by chouno 2019/02/17
            {
                if (CTSettings.iniValue.HSCModeType == 1)
                {
                    IIMode = IIModeConstants.IIMode_DROP_TEST;
                    HSCameraPower(modCT30K.OnOffStatusConstants.OnStatus);
                }
                else
                {
                    IIMode = IIModeConstants.IIMode_HSC;
                    HSCameraPower(modCT30K.OnOffStatusConstants.OnStatus);
                }
            }
            else
            {
                IIMode = IIModeConstants.IIMode_None;
            }


		}

        //*************************************************************************************************
        //機　　能： 終了時の処理
        //
        //           変数名          [I/O] 型            内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
		public static void HSC_ShutdownProcess()
		{

			//CT30K終了時は高速カメラ電源オフ
			HSCameraPower(modCT30K.OnOffStatusConstants.OffStatus);

			//CT30K終了時は高速I.I.電源オフ
			HSIIPower(modCT30K.OnOffStatusConstants.OffStatus);

		}

        //*************************************************************************************************
        //機　　能： I.I.切替処理
        //
        //           変数名          [I/O] 型            内容
        //引　　数： theMode         IIModeConstants     I.I.状態
        //           IsMechaMove     Boolean             True:移動あり False:移動なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
		public static bool SwitchII(IIModeConstants theMode)
		{
			bool functionReturnValue = false;

			bool err_sts = false;
			bool Is_HSC = false;
			bool Is_CT = false;

			string theCommand = null;

			//初期値設定
			functionReturnValue = false;

			Is_CT = (theMode == IIModeConstants.IIMode_CT);
			Is_HSC = (theMode == IIModeConstants.IIMode_HSC);

            //既に指定位置なら抜ける
            //Rev23.40 by長野 2016/04/05 //Rev23.21 不要 by長野 2016/02/22
            //if (IsOKIIPos)
            //    return functionReturnValue;


			//I.I.の移動可否判定
			if (!IsOKIIMove())
				return functionReturnValue;

			//短残光I.I.用カメラ電源(高速撮影時:ON それ以外:OFF)
			err_sts = HSCameraPower((Is_HSC ? modCT30K.OnOffStatusConstants.OnStatus : modCT30K.OnOffStatusConstants.OffStatus));

			//    '高速撮影でなければ、I.I.絞りを全て閉じる → シーケンサ側で対応したためコメントアウト
			//    If Not Is_HSC Then Call CloseALLIris

			if (Is_CT) {
				theCommand = "CTIISet";
			} else if (Is_HSC) {
				theCommand = "TVIISet";
			}

			//I.I.切替
			if (!modSeqComm.ChangeII(theCommand))
				return functionReturnValue;

			//切替成功
			functionReturnValue = true;
			return functionReturnValue;

		}

        //*************************************************************************************************
        //機　　能： '短残光I.I.電源ON/OFF
        //
        //           変数名          [I/O] 型                内容
        //引　　数： theStatus       OnOffStatusConstants    オンオフステータス → Boolの方が良い？
        //戻 り 値：                 Boolean                 True:成功   False:失敗
        //
        //補　　足： なし
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
		public static bool HSIIPower(modCT30K.OnOffStatusConstants theStatus)
		{
			bool functionReturnValue = false;

			functionReturnValue = false;

			switch (theStatus) {

				case modCT30K.OnOffStatusConstants.OnStatus:
					//短残光I.I.電源ON
					modSeqComm.SeqBitWrite("TVIIPowerOn", true);
					break;

				case modCT30K.OnOffStatusConstants.OffStatus:
					//短残光I.I.電源OFF
					modSeqComm.SeqBitWrite("TVIIPowerOff", true);
					break;

				case modCT30K.OnOffStatusConstants.UnknownStatus:
					//エラー処理
                    return functionReturnValue;
			}

			functionReturnValue = true;
			return functionReturnValue;

		}

        //*************************************************************************************************
        //機　　能： 短残光I.I.用カメラ電源ON/OFF
        //
        //           変数名          [I/O] 型                内容
        //引　　数： theStatus       OnOffStatusConstants    オンオフステータス → Boolの方が良い？
        //戻 り 値：                 Boolean                 True:成功   False:失敗
        //
        //補　　足： なし
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
		public static bool HSCameraPower(modCT30K.OnOffStatusConstants theStatus)
		{
			bool functionReturnValue = false;

			functionReturnValue = false;

			switch (theStatus) {

				case modCT30K.OnOffStatusConstants.OnStatus:
					//短残光I.I.用カメラ電源ON
					modSeqComm.SeqBitWrite("CameraPowerOn", true);
					break;

				case modCT30K.OnOffStatusConstants.OffStatus:
					//短残光I.I.用カメラ電源OFF
					modSeqComm.SeqBitWrite("CameraPowerOff", true);
					break;

				case modCT30K.OnOffStatusConstants.UnknownStatus:
					//エラー処理
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
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
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
			_with4.gain_kv = 0.0F;
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

			//modMecainf.PutMecainf(ref modMecainf.mecainf);
            //CTSettings.mecainf.Write();
            //Rev23.40//Rev23.21 by長野 2016/04/05
            CTSettings.mecainf.Put(_with4);

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
        //機　　能： フォーム上のコントロール全てに制限を設ける
        //
        //           変数名          [I/O] 型        内容
        //引　　数： theForm         Form            制限するフォーム
        //           Value           Boolean         True:有効、False:無効
        //戻 り 値： なし
        //
        //補　　足： タイマーコントロールは対象外
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
		public static void LimitControlOnForm(Form theForm, bool Value)
		{

			System.Windows.Forms.Control theControl = null;

			//エラーは無視する
			 // ERROR: Not supported in C#: OnErrorStatement


			//    '自身に制限
			//    theForm.Enabled = Value

			foreach (Control theControl_loopVariable in theForm.Controls)
            {
				theControl = theControl_loopVariable;

				//タイマーコントロールは対象外とする
                //if (!(Information.TypeName(theControl) == "Timer"))
                if (theControl.GetType().Name == "Timer")
                {
					theControl.Enabled = Value;
				}

			}

		}

        //*************************************************************************************************
        //機　　能： コントロールに制限を設ける
        //
        //           変数名          [I/O] 型            内容
        //引　　数： theMode         IIModeConstants     I.I.の状態
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
		public static void LimitControl(IIModeConstants theMode)
        {
            #region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''

            //short i = 0;
            ////ループ用変数
            //bool IsNotUnknown = false;
            //IsNotUnknown = !IsUnknownMode;

            ////CT30Kが起動するまでは無効
            //if (!IsStartUp)
            //    return;

            ////透視画面の表示/非表示を切替
            //if (IsHSCmode)
            //    DisplayTransImage(System.Windows.Forms.FormWindowState.Minimized);

            //if (IsCTmode)
            //    DisplayTransImage(System.Windows.Forms.FormWindowState.Normal);

            ////エラーは無視する
            // // ERROR: Not supported in C#: OnErrorStatement


            ////CTメニューツールバー
            //var _with5 = frmCTMenu.Instance.Toolbar1;
            //_with5.Items[1].Enabled = IsNotUnknown;        //X線
            ////        .Buttons(3).Enabled = True                //'ドアロック(常に有効に)
            //_with5.Items[5].Enabled = IsNotUnknown;		//I.I.視野切替
            //_with5.Items[9].Enabled = IsCTmode;			//カメラ電源
            //_with5.Items[11].Enabled = IsCTmode;			//開く
            //_with5.Items[13].Enabled = IsCTmode;			//印刷
            //_with5.Items[15].Enabled = IsCTmode;			//ダブルオブリーク
            //_with5.Items[17].Enabled = IsCTmode;			//メンテナンス


            ////スキャンコントロール
            //LimitControlOnForm(frmScanControl.Instance, IsCTmode);
            //frmScanControl.Instance.Enabled = IsCTmode;
            ////スキャンスタートボタンは非表示に
            //frmScanControl.Instance.ctbtnScanStart.Visible = IsCTmode;

            ////画像サーチ&階調変換
            //LimitControlOnForm(frmImageControl.Instance, IsCTmode);
            //frmImageControl.Instance.Enabled = IsCTmode;

            ////スキャン画像
            //LimitControlOnForm(frmScanImage.Instance, IsCTmode);
            //frmScanImage.Instance.Enabled = IsCTmode;

            ////画像付帯情報
            //LimitControlOnForm(frmImageInfo.Instance, IsCTmode);
            //frmImageInfo.Instance.Enabled = IsCTmode;
            //if (!IsCTmode)
            //    frmImageInfo.Instance.Hide();


            ////メカコントロール
            //LimitControlOnForm(frmMechaControl.Instance, IsNotUnknown);
            //var _with6 = frmMechaControl.Instance;

            ////CT/II切替ボタンは常に有効
            //_with6.fraHighSpeedCamera.Enabled = true;
            //for (i = _with6.cwbtnChangeMode.LBound; i <= _with6.cwbtnChangeMode.UBound; i++) {
            //    _with6.cwbtnChangeMode(i).Enabled = true;
            //}

            ////詳細ボタンは常に有効
            //_with6.cmdDetails.Enabled = true;

            ////自動位置指定ボタンは高速撮影時無効
            //_with6.fraAutoScanPos.Enabled = IsCTmode;
            //_with6.cmdFromSlice.Enabled = IsCTmode;
            //_with6.cmdFromTrans.Enabled = IsCTmode;

            ////I.I.絞りの表示/非表示
            //_with6.fraIris.Visible = IsHSCmode;


            ////メカ詳細
            //LimitControlOnForm(frmMechaReset.Instance, IsNotUnknown);
            //var _with7 = frmMechaReset.Instance;

            ////I.I.切替リセットボタンは常に有効
            //_with7.fraIIChangeReset.Enabled = true;
            //_with7.cmdIIChangeReset.Enabled = true;

            ////閉じるボタンは常に有効
            //_with7.cmdClose.Enabled = true;


            ////X線コントロール
            //LimitControlOnForm(frmXrayControl.Instance, IsNotUnknown);
            //frmXrayControl.Instance.Enabled = IsNotUnknown;

            ////透視画面
            //frmTransImage.Instance.Enabled = IsCTmode;
            //frmTransImageControl.Instance.Enabled = IsCTmode;
            //frmTransImageInfo.Instance.Enabled = IsCTmode;
            //LimitControlOnForm(frmTransImageControl.Instance, IsCTmode);

            #endregion
            //short i = 0;//ループ用変数
            bool IsNotUnknown = false;
            IsNotUnknown = !(theMode == modHighSpeedCamera.IIModeConstants.IIMode_None);

            //透視画面の表示/非表示を切替
            //if (IsHSCmode)
            //add by chouno 2019/02/12 Rev26.20
            if (IsHSCmode || IsDropTestmode)
            {
                modHighSpeedCamera.DisplayTransImage(System.Windows.Forms.FormWindowState.Minimized);
            }
            if (IsCTmode)
            {
                modHighSpeedCamera.DisplayTransImage(System.Windows.Forms.FormWindowState.Normal);
            }

            //CTメニューツールバー
            var _with5 = frmCTMenu.Instance.Toolbar1;

            _with5.Items["tsbtnXray"].Enabled = IsNotUnknown;			//X線
            //.Buttons(3).Enabled = True    'ドアロック(常に有効に)
            //_with5.Items["tsbtnIIField"].Enabled = (CTSettings.detectorParam.Use_FlatPanel ? IsHSCmode : IsNotUnknown);			//I.I.視野切替
            //_with5.Items["tsbtnIIField"].Visible = (CTSettings.detectorParam.Use_FlatPanel ? IsHSCmode : IsNotUnknown);			//I.I.視野切替
            //Rev26.20 change by chouno 2019/02/12
            _with5.Items["tsbtnIIField"].Enabled = (CTSettings.detectorParam.Use_FlatPanel ? (IsHSCmode || IsDropTestmode) : IsNotUnknown);			//I.I.視野切替
            _with5.Items["tsbtnIIField"].Visible = (CTSettings.detectorParam.Use_FlatPanel ? (IsHSCmode || IsDropTestmode) : IsNotUnknown);			//I.I.視野切替

            _with5.Items["tsbtnLiveImage"].Enabled = IsCTmode;
            _with5.Items["tsbtnLiveImage"].Visible = IsCTmode;
            _with5.Items["tsbtnOpen"].Enabled = IsCTmode;
            _with5.Items["tsbtnOpen"].Visible = IsCTmode;
            _with5.Items["tsbtnPrint"].Enabled = IsCTmode;
            _with5.Items["tsbtnPrint"].Visible = IsCTmode;
            _with5.Items["tsbtnDoubleOblique"].Enabled = IsCTmode;
            _with5.Items["tsbtnDoubleOblique"].Visible = IsCTmode;
            _with5.Items["tsbtnMainte"].Enabled = IsCTmode;
            _with5.Items["tsbtnMainte"].Visible = IsCTmode;
            _with5.Items["tsbtnExObsCam"].Enabled = IsNotUnknown;//Rev26.40 add by chouno 2019/02/19

            frmScanControl.Instance.Enabled = IsCTmode;

            //スキャンスタートボタンは非表示に
            frmScanControl.Instance.ctbtnScanStop.Enabled = IsCTmode;
            frmScanControl.Instance.ctbtnScanStart.Enabled = IsCTmode;

            frmImageControl.Instance.Enabled = IsCTmode;

            frmScanImage.Instance.Enabled = IsCTmode;

            frmImageInfo.Instance.Enabled = IsCTmode;
            //切替ボタンのみ有効
            frmMechaControl.Instance.fraAutoScanPos.Enabled = IsCTmode;
            frmMechaControl.Instance.fraCollimator.Enabled = IsNotUnknown;
            frmMechaControl.Instance.fraIris.Enabled = IsNotUnknown;
            
            //frmMechaControl.Instance.fraUpDown.Enabled = IsNotUnknown;
            //Rev26.40 条件変更 by chouno 2019/02/12
            frmMechaControl.Instance.fraUpDown.Enabled = IsNotUnknown && !IsDropTestmode;
            //frmMechaControl.Instance.fraMechaPos.Enabled = IsNotUnknown;
            frmMechaControl.Instance.fraMechaPos.Enabled = IsNotUnknown && !IsDropTestmode;
            frmMechaControl.Instance.fraMechaControl.Enabled = IsNotUnknown;
            frmMechaControl.Instance.ctchkIIMove[0].Enabled = IsNotUnknown && !IsDropTestmode;
            frmMechaControl.Instance.ctchkIIMove[1].Enabled = IsNotUnknown && !IsDropTestmode;
            frmMechaControl.Instance.ctchkIIMove[2].Enabled = IsNotUnknown && !IsDropTestmode;
            frmMechaControl.Instance.cmdDetails.Enabled = IsNotUnknown;//Rev26.40 add by chouno 2019/02/19

            //Rev26.40 条件追加 by chouno 2019/02/12
            foreach (CWButton cb in frmMechaControl.Instance.cwbtnFineTable)
            {
                if (cb != null)
                {
                    cb.Enabled = IsNotUnknown && !IsDropTestmode;
                }
            }
            foreach (CWButton cb in frmMechaControl.Instance.cwbtnMove)
            {
                if (cb != null)
                {
                    cb.Enabled = IsNotUnknown && !IsDropTestmode;
                }
            }
            foreach (CWButton cb in frmMechaControl.Instance.cwbtnRotate)
            {
                if (cb != null)
                {
                    cb.Enabled = IsNotUnknown && !IsDropTestmode;
                }
            }
            //frmMechaControl.Instance.fraIIMove.Enabled = IsNotUnknown && !IsDropTestmode;

            //frmMechaControl.Instance.fraIris.Visible = IsHSCmode;
            //Rev26.40 by chouno 2019/02/12
            frmMechaControl.Instance.fraIris.Visible = IsHSCmode || IsDropTestmode;

            frmScanControl.Instance.Enabled = IsCTmode;
            frmScanControl.Instance.SSTab1.Enabled = IsCTmode;

            //スキャンスタートボタンは非表示に
            frmScanControl.Instance.ctbtnScanStop.Enabled = IsCTmode;
            frmScanControl.Instance.ctbtnScanStart.Enabled = IsCTmode;

            frmXrayControl.Instance.Enabled = IsNotUnknown;
            frmXrayControl.Instance.cmdDetail.Enabled = IsNotUnknown;

            //透視画面
            frmTransImage.Instance.Enabled = IsNotUnknown;
            frmTransImageControl.Instance.Enabled = IsNotUnknown;
            frmTransImageInfo.Instance.Enabled = IsNotUnknown;


		}

        //*************************************************************************************************
        //機　　能： '透視画面の表示／非表示を切替
        //
        //           変数名          [I/O] 型                    内容
        //引　　数： theState        FormWindowStateConstants    vbNormal    :元の大きさに
        //                                                       vbMinimized :最小化
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
		public static void DisplayTransImage(System.Windows.Forms.FormWindowState theState)
		{

			//最大化は無効
			if (theState == System.Windows.Forms.FormWindowState.Maximized)
				return;

            frmTransImageControl.Instance.WindowState = theState;
            frmTransImageInfo.Instance.WindowState = theState;
            frmTransImage.Instance.WindowState = theState;
            frmTransImage.Instance.ResizeImage();
            
			//v16.02 追加 Modal表示中はフォーカスできない by 山影 10-03-03
           if (!frmMechaReset.Instance.Visible)
           {
				//CTメニューにフォーカスする
               frmCTMenu.Instance.Activate();				//v16.02追加 byやまおか 2010/03/02
			}

		}


        //*************************************************************************************************
        //機　　能： X線WU中であるかチェック
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 Boolean         True:WU中、False:それ以外
        //
        //補　　足： なし
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
		public static bool IsXrayWarmUpNow()
		{

			return (modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpNow);

		}

        //*************************************************************************************************
        //機　　能： 短残光I.I.絞りを動作限まで閉じる
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： I.I.絞りの開閉限の信号が得られないため、一定時間動かす
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
		public static void CloseALLIris()
		{

			const short SendTime = 10;			//I.I.絞り閉動作時間(仮)
			string[] theCommand = null;			//シーケンサコマンド用配列
			int sIndex = 0;			            //配列添字最小値
			int eIndex = 0;			            //配列添字最大値
			int i = 0;			                //ループ用変数

			//シーケンサコマンドを設定
			theCommand = new string[] {
				"IrisLClose",
				"IrisRClose",
				"IrisUClose",
				"IrisDClose"
			};

			//配列の添字最小値、最大値
			sIndex = theCommand.GetLowerBound(0);
            eIndex = theCommand.GetUpperBound(0);

			//運転送信
			for (i = sIndex; i <= eIndex; i++) {
				modSeqComm.SeqBitWrite(theCommand[i], true);
			}

			modCT30K.PauseForDoEvents(SendTime);

			//停止送信
			for (i = sIndex; i <= eIndex; i++) {
				modSeqComm.SeqBitWrite(theCommand[i], false);
			}

		}
	}
}
