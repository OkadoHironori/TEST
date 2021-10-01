using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
//
using CTAPI;
using CT30K.Common;

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： modStsFlg.bas                                               */
    ///* 処理概要　　： 起動フラグ管理                                              */
    ///* 注意事項　　： なし                                                        */
    ///* -------------------------------------------------------------------------- */
    ///* 適用計算機　： DOS/V PC                                                    */
    ///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
    ///* コンパイラ　： VB 6.0                                                      */
    ///* -------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
    ///*                                                                            */
    ///* V4.0        01/03/26    (ITC)    鈴山　修   新規作成                       */
    ///* V19.00      12/02/21    H.Nagai             BHC対応                        */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
	
    internal static class modCTBusy
	{
        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************

        //ＣＴ動作チェック用フラグ(各ビットについて 1:起動中, 0:停止中)			
		public const int CTImagePrint = 0x1;            //画像印刷		
		public const int CTScanCondition = 0x2;	        //スキャン条件詳細画面		
		public const int CTSlicePlan = 0x4;	            //スライスプラン		
		public const int CTMechaBusy = 0x10;	        //メカ動作中		
		public const int CTIntegImage = 0x20;	        //透視画像積算処理		
		public const int CTSaveMovie = 0x40;	        //動画保存処理		
		public const int CTZooming = 0x400;	            //ズーミング		
		public const int CTReconstruct = 0x800;	        //再構成リトライ		
		public const int CTImageProcessing = 0x1000;	//画像処理		
		public const int CTFormatTransfer = 0x2000;	    //画像フォーマット変換		
		public const int CTMaintenance = 0x4000;	    //メンテナンスツール		
		public const int CTScanStart = 0x8000;	        //スキャンスタート    '末尾に&を付けないとマイナス値となってしまう		
		public const int CTReconStart = 0x20000;	    //ズーミング、再構成リトライによるreconmst.exeの実行		
		public const int CTScanCorrect = 0x80000;	    //スキャン校正		
		public const int CTTableAutoMove = 0x200000;	//自動テーブル移動
        public const int CTScanoCondition = 0x400000;	//スキャノ条件詳細画面 by長野 2015/02/19 Rev21.00 追加 by長野 2015/02/19		

        //v19.00 産業用からコピー(値変更)
        //Public Const CTBHCTable         As Long = &H80000   'BHCテーブル作成        'v8.0追加 by Ohkado 2006/12/28
		public const int CTBHCTable = 0x400000;		//BHCテーブル作成        'v8.0追加 by Ohkado 2006/12/28

        //ＣＴ起動状態変数（内部変数）
		private static int myCTBusy;

        #region サポートしているイベント
        public static Action StatusChanged;
        #endregion


        //*******************************************************************************
        //機　　能： CTBusyプロパティ（取得）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  99/XX/XX   ????????      新規作成
        //*******************************************************************************

        //*******************************************************************************
        //機　　能： CTBusyプロパティ（設定）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		public static int CTBusy
        {
			get { return myCTBusy; }
			set {

				myCTBusy = value;

                #region //<CTBusyのイベント　後で検討する>
                /*
                // イベント生成
                if (StatusChanged != null)
                {
                    StatusChanged();
                }
                */
                #endregion

                bool IsImageProcessingBusy = false;
				IsImageProcessingBusy = Convert.ToBoolean(myCTBusy & CTImageProcessing) ||
                                        Convert.ToBoolean(myCTBusy & CTFormatTransfer) ||
                                        Convert.ToBoolean(myCTBusy & CTZooming);

				bool IsReconstructBusy = false;
				IsReconstructBusy = Convert.ToBoolean(myCTBusy & CTReconstruct);


				//メカ制御画面
                //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
				//frmMechaControl frmMechaControl = frmMechaControl.Instance;
                //if (modLibrary.IsExistForm(frmMechaControl.Instance))
                if (modLibrary.IsExistForm("frmMechaControl"))
                {
					//スキャン中は使用不可 →メカ動作中の場合は不可
					//.Enabled = Not CBool(myCTBusy And CTScanStart)
                    frmMechaControl.Instance.Enabled = !Convert.ToBoolean(myCTBusy & CTMechaBusy);	//変更 by 間々田 2009/06/16

					//何か作業中の場合、詳細ボタンは使用不可とする   '追加 by 間々田 2009/06/17
                    frmMechaControl.Instance.cmdDetails.Enabled = !Convert.ToBoolean(myCTBusy);

                    frmMechaControl.Instance.cmdFromSlice.Enabled = !IsImageProcessingBusy;

                    frmMechaControl.Instance.cmdFromExObsCam.Enabled = !IsImageProcessingBusy; //Rev26.00 修正 by chouno 2017/10/20

                    //.cmdFromTrans.Enabled = Not IsImageProcessingBusy
					//v17.20 原点復帰が完了していることが自動スキャン位置を使用できる条件とする。 by 長野 2010/09/20
					//自動スキャン位置指定(透視)は透視画面のサイズが標準のときだけ有効にする 'v18.00変更 byやまおか 2011/02/10
					//.cmdFromTrans.Enabled = (Not IsImageProcessingBusy) And CBool(frmTransImage.ZoomScale = 1)
                    //frmMechaControl.Instance.cmdFromTrans.Enabled = (!IsImageProcessingBusy) && Convert.ToBoolean(frmTransImage.Instance.ZoomScale == 1) && 
                    //                                                          (!modSeqComm.MySeq.stsXOrgReq && !modSeqComm.MySeq.stsYOrgReq && !modSeqComm.MySeq.stsIIOrgReq && !modSeqComm.MySeq.stsIIChgOrgReq) &&
                    //                                                          !(modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpNow);		//ウォームアップ中を追加 'v17.60/v18.00変更 byやまおか 2011/03/06
                    frmMechaControl.Instance.cmdFromTrans.Enabled = (!IsImageProcessingBusy) & 
                                                                    Convert.ToBoolean(frmTransImage.Instance.ZoomScale == (int)frmTransImage.TransImageDispSizeConstants.MediumSize) & 
                                                                    (!modSeqComm.MySeq.stsXOrgReq & !modSeqComm.MySeq.stsYOrgReq & !modSeqComm.MySeq.stsIIOrgReq & !modSeqComm.MySeq.stsIIChgOrgReq) & 
                                                                    !(modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpNow);     //ウォームアップ中を追加 'v17.4X/v18.00変更 byやまおか 2011/04/27 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

					if (!IsImageProcessingBusy) 
                    {
                        frmMechaControl.Instance.cmdFromSlice.BackColor = SystemColors.Control;
                        frmMechaControl.Instance.cmdFromTrans.BackColor = SystemColors.Control;
                        frmMechaControl.Instance.cmdFromExObsCam.BackColor = SystemColors.Control; //Rev23.30 追加 by長野 2016/02/06
                    
                        // Add Start 2018/10/29 M.Oyama V26.40 Windows10対応
                        frmMechaControl.Instance.cmdFromSlice.UseVisualStyleBackColor = true;
                        frmMechaControl.Instance.cmdFromTrans.UseVisualStyleBackColor = true;
                        frmMechaControl.Instance.cmdFromExObsCam.UseVisualStyleBackColor = true;
                        // Add End 2018/10/29
                    }
				}

				//Ｘ線制御画面
				//if (modLibrary.IsExistForm(frmXrayControl.Instance)) 
                if (modLibrary.IsExistForm("frmXrayControl"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
                {
					//スキャン中は使用不可 →メカ動作中の場合は不可
                    //.Enabled = Not CBool(myCTBusy And CTScanStart)
                    frmXrayControl.Instance.Enabled = !Convert.ToBoolean(myCTBusy & CTMechaBusy);	//変更 by 間々田 2009/06/16

                    //追加2014/10/07hata_v19.51反映 
                    //スキャン条件設定中は使用不可   'v18.00追加 byやまおか 2011/03/14 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    //frmXrayControl.Instance.fraFocus.Enabled = !Convert.ToBoolean(CTScanCondition);
                    //Rev20.01 修正 by長野 2015/06/02
                    frmXrayControl.Instance.fraFocus.Enabled = !Convert.ToBoolean(myCTBusy & CTScanCondition);
                }

				//スキャン条件画面
				//frmScanControl frmScanControl = frmScanControl.Instance;
                //if (modLibrary.IsExistForm(frmScanControl))
                if (modLibrary.IsExistForm("frmScanControl"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
                {
					frmScanControl.Instance.SSTab1.Enabled = !Convert.ToBoolean(myCTBusy);

					//スキャンスタートボタン
					//.cmdScanStart.Enabled = Not CBool(myCTBusy And (Not CTScanStart))
					//.cmdScanStart.Enabled = Not CBool(myCTBusy And (Not CTScanStart) And (Not CTMechaBusy)) 'v15.0変更 by 間々田 2009/06/16
					//.ctbtnScanStart.Enabled = Not CBool(myCTBusy And (Not CTScanStart) And (Not CTMechaBusy)) 'v15.0変更 by 間々田 2009/06/30  'v15.10削除 frmScanStatusで管理する byやまおか 2010/01/21
					//.cmdScanStart.Caption = LoadResString(IIf(myCTBusy And CTScanStart, IDS_btnScanStop, IDS_btnScanStart))
                    frmScanControl.Instance.ctbtnScanStart.Caption = CTResources.LoadResString(Convert.ToBoolean(myCTBusy & CTScanStart) ? StringTable.IDS_btnScanStop : StringTable.IDS_btnScanStart);

                    int i = 0;
                    //for (i = frmScanControl.cmdImageProc.lGetLowerBound(0); i <= frmScanControl.cmdImageProc.GetUpperBound(0); i++)
                    for (i = 0; i < frmScanControl.Instance.cmdImageProc.Count; i++)
                    
                    {
                        frmScanControl.Instance.cmdImageProc[i].Enabled = !IsImageProcessingBusy;
                        if (!IsImageProcessingBusy)
                        {
                            frmScanControl.Instance.cmdImageProc[i].BackColor = SystemColors.Control;
                        }
                    }

                    if (!IsImageProcessingBusy)
                    {
                        frmScanControl.Instance.cmdZooming.BackColor = SystemColors.Control;
                    }

                    if (!IsReconstructBusy)
                    {
                        frmScanControl.Instance.cmdReconst.BackColor = SystemColors.Control;
                        frmScanControl.Instance.cmdPostConeReconst.BackColor = SystemColors.Control;
                    }
				}

				//透視画像画面
				//if (modLibrary.IsExistForm(frmTransImage.Instance)) 
                if (modLibrary.IsExistForm("frmTransImage"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
                {
					//スキャン中は使用不可
					frmTransImage.Instance.Enabled = !Convert.ToBoolean(myCTBusy & CTScanStart);
				}

				//追加 by 間々田 2009/08/17
				//if (modLibrary.IsExistForm(frmTransImageControl.Instance))
                if (modLibrary.IsExistForm("frmTransImageControl"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
                {
                    //透視画像ボタン
					frmTransImageControl.Instance.cmdTransImage.Enabled = !Convert.ToBoolean(myCTBusy);

                    //ダブルオブリークボタン：スキャン中・再構成リトライ中・コーン後再構成中・ズーミング中は不可
					//frmTransImageControl.Instance.Toolbar1.Items["DoubleOblique"].Enabled = !Convert.ToBoolean(myCTBusy & (CTScanStart | CTReconstruct | CTZooming));
                    //コマンドボタンに変更
                    frmTransImageControl.Instance.cmdDoubleOblique.Enabled = !Convert.ToBoolean(myCTBusy & (CTScanStart | CTReconstruct | CTZooming));
                
                }

				//スキャン中は使用不可 →メカ動作中の場合は不可
				//.Enabled = Not CBool(myCTBusy And CTScanStart)
				frmCTMenu.Instance.Toolbar1.Enabled = !Convert.ToBoolean(myCTBusy & CTMechaBusy);		//変更 by 間々田 2009/06/16

				//'ウィザードボタン：スキャン中は使用不可
				//.Buttons("Wizard").Enabled = Not CBool(myCTBusy)
				//
				//'I.I.視野ボタン：スキャン中は使用不可
				//.Buttons("I.I.Field").Enabled = Not CBool(myCTBusy And CTScanStart)
				//
				//'扉電磁ロックボタン：スキャン中は使用不可
				//.Buttons("DoorLock").Enabled = Not CBool(myCTBusy And CTScanStart)

				//メンテナンスボタン
                frmCTMenu.Instance.Toolbar1.Items["tsbtnMainte"].Enabled = !Convert.ToBoolean(myCTBusy);

				//ダブルオブリーク：スキャン中・再構成リトライ中・コーン後再構成中・ズーミング中は不可
                frmCTMenu.Instance.Toolbar1.Items["tsbtnDoubleOblique"].Enabled = !Convert.ToBoolean(myCTBusy & (CTScanStart | CTReconstruct | CTZooming));
                
				//ステータス画面（frmStatus）
				//if (modLibrary.IsExistForm(frmStatus.Instance))
                if (modLibrary.IsExistForm("frmStatus"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
                {
					frmStatus.Instance.UpdateCTBusyStatus();
                }
			}
        }

        #region　<CTBusyのイベント　後で検討する>
        /*
        /// <summary>
        /// 画像処理中？
        /// </summary>
        public static bool IsImageProcessingBusy
        {
            get
            {
                return Convert.ToBoolean(myCTBusy & CTImageProcessing) |
                       Convert.ToBoolean(myCTBusy & CTFormatTransfer) |
                       Convert.ToBoolean(myCTBusy & CTZooming);
            }
        }

        /// <summary>
        /// 再構成中？
        /// </summary>
        public static bool IsReconstructBusy
        {
            get
            {
                return Convert.ToBoolean(myCTBusy & CTReconstruct);
            }
        }
        */
        #endregion
	}
}
