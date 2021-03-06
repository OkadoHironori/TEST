using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

//
using CTAPI;
using CT30K.Common;
using TransImage;

namespace CT30K
{
	///* ************************************************************************** */
	///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver9.7               */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： modScanCorrectNew.bas                                       */
	///* 処理概要　　： キャプチャ関連                                              */
	///* 注意事項　　： なし                                                        */
	///* -------------------------------------------------------------------------- */
	///* 適用計算機　： DOS/V PC                                                    */
	///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
	///* コンパイラ　： VB 6.0                                                      */
	///* -------------------------------------------------------------------------- */
	///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
	///*                                                                            */
	///* V9.7        04/11/29    (SI4)    間々田     新規作成                       */
	///*                                                                            */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2004                 */
	///* ************************************************************************** */
	internal static class modScanCorrectNew
	{
		private static ProgressBar myProgressBar;		//v10.0追加 by 間々田 2005/01/31 PCフリーズ対策処理 改良版対応

		public static bool IIMovedAtAutoSpCorrect;		//自動スキャン位置校正用I.I.移動フラグ   v11.21追加 by 間々田 2006/02/10

		public const int NullAddress = 0;
		public static ushort[] TransImage;			//透視画像の配列（表示用）
		public static short[] DestImage;			//FPDキャプチャー用画像の配列　'v17.00追加 山本 2009-09-29

		//v19.00 追加 by長野 2012/05/10
		public static float GainCurrent;	//ゲイン撮影時の管電流（開放管はターゲット電流、密封管はフィードバック値とする）

		private static bool BackupLiveCamera;

        [DllImport("kernel32", EntryPoint = "RtlMoveMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void CopyMemory(ushort[] Image, int Source, int length);		//TODO As Any

        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int Gain_Data_Acquire(int hMil, int hPke, ref ushort TransImage, int Itgnum, MilCaptureCallback2Delegate CallBackAddress, ref ushort Image, ref int ima2, int hDevID, ref int mrc, int View,
        int table_rotation, int detector, float FrameRate);		//v17.50変更 DestImage削除 by 間々田 2011/01/14

        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int GetCaptureSumImage(int hMil, int hPke, int ViewNum, int IntegNum, ref ushort TransImage, ref int SumImage, MilCaptureCallback2Delegate hCT30K);		//v17.66 引数追加 by 長野 2011/12/28

        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PkeCaptureGain(int hPke, MilCaptureCallback2Delegate CallBackAddress, ref ushort TransImage, ref uint Gain_Image_L, int ViewNum, int IntegNum);		//v17.66 引数変更 by 長野 2011/12/28

        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int AutoScanpositionEntry(int hMil, int hPke, ref ushort TransImage, int hDevID, ref int mrc, float fud_step, float rud_step, ref float fud_start, ref float fud_end, float rud_start,
        float rud_end, int detector, ref ushort GAIN_IMAGE, ref ushort Def_IMAGE, MilCaptureCallbackDelegate CallBackAddress);		//v17.50変更 DestImage削除 by 間々田 2011/01/14
        
        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PkeCaptureOffset(int hPke, MilCaptureCallback2Delegate CallBackAddress, ref ushort TransImage, ref double OffsetImage, int IntegNum);

        //回転中心校正画像取込み関数
        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int RotationCenter_Data_Acquire(int hMil, int hPke, ref ushort TransImage, int h_size, int v_size, int Itgnum,
                                                             ref ushort RC_IMAGE0,
                                                             ref ushort RC_IMAGE1,
                                                             ref ushort RC_IMAGE2,
                                                             ref ushort RC_IMAGE3,
                                                             ref ushort RC_IMAGE4,
                                                             int hDevID, ref int mrc, int View, int multislice, int table_rotation, float SW, int connect,
                                                             ref ushort RC_CONE,
                                                             ref ushort Def_IMAGE,
                                                             ref ushort GAIN_IMAGE,
                                                             int detector, float FrameRate, int RotateSelect, int c_cw_ccw, int xrot_stop_pos, MilCaptureCallbackDelegate CallBackAddress);		//v17.50引数変更 by 間々田 2011/01/13 DestImage, dcf削除
        //

        //
		// MilCaptureCallback コールバック関数のデリゲート
		//
		private delegate void MilCaptureCallbackDelegate(int parm);

		//
		// MilCaptureCallback2 コールバック関数のデリゲート
		//
		private delegate void MilCaptureCallback2Delegate(int parm);


        //Rev25.00 test by長野 2016/08/17
        public static float Rmean = 0.0f;
        public static float Lmean = 0.0f;

		//*************************************************************************************************
		//機　　能： 校正時キャプキャ開始時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private static void CaptureStartForScanCorrect(CTStatus lbl, ProgressBar pb, int CaptureCount)
		{
			//ライブ画像処理を停止
			BackupLiveCamera = frmTransImage.Instance.CaptureOn;
			frmTransImage.Instance.CaptureOn = false;

			//DLLによるキャプチャ処理を優先させるため、CT30K側のタイマ処理をオフにする
			frmCTMenu.Instance.tmrStatus.Enabled = false;
		    //'v19.50 タイマーの統合 by長野 2013/12/17
            //frmMechaControl.Instance.tmrMecainf.Enabled = false;
            //frmMechaControl.Instance.tmrSeqComm.Interval = 2000;	//シーケンサについては監視インターバルを長くする
            modMechaControl.Flg_MechaControlUpdate = false;
            frmMechaControl.Instance.tmrMecainfSeqComm.Interval = 2000;

			//「データ収集中」と表示
			if (lbl != null) 
			{
				lbl.Status = StringTable.GC_STS_CAPTURE;
			}

			//プログレスバーの表示
			myProgressBar = pb;

			if (myProgressBar != null)
			{
				myProgressBar.Maximum = CaptureCount;
				myProgressBar.Value = 0;
				myProgressBar.Visible = true;
			}
		}


		//*************************************************************************************************
		//機　　能： 校正時キャプキャ終了時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private static void CaptureEndForScanCorrect()
		{
			//プログレスバーの非表示
			if (myProgressBar != null)
			{
				myProgressBar.Visible = false;
				myProgressBar = null;
			}

			//CT30K側のタイマ処理を元に戻す
            //'v19.50 タイマーの統合 by長野 2013/12/17
			//frmMechaControl.Instance.tmrSeqComm.Interval = 500;
			//frmMechaControl.Instance.tmrMecainf.Enabled = true;
            modMechaControl.Flg_MechaControlUpdate = true;
            frmMechaControl.Instance.tmrMecainfSeqComm.Interval = 1000;
            
            frmCTMenu.Instance.tmrStatus.Enabled = true;

			//ライブ画像処理を元に戻す
			//frmTransImage.CaptureOn = BackupLiveCamera     'v17.00削除 byやまおか 2010/02/08
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypeII ||
                CTSettings.detectorParam.DetType == DetectorConstants.DetTypeHama)	//v17.00修正 byやまおか 2010/03/04
			{
				frmTransImage.Instance.CaptureOn = BackupLiveCamera;
			}
		}


		//********************************************************************************
		//機    能  ：  ゲイン校正画像を配列に読み込む
		//              変数名           [I/O] 型        内容
		//引    数  ：  StatusLabel      [I/ ] Label     進行状況を表示するラベル
		//              ViewNum          [I/ ] Long      ビュー数
		//              IntegNum         [I/ ] Long      積算枚数
		//              TableRot         [I/ ] Long      テーブル回転（0:なし、1:あり）
		//              ma               [I/ ] Variant   管電流
		//              DownTableDist    [I/ ] Single    テーブル下降収集の距離
		//戻 り 値  ：                   [ /O] Boolean   結果(True:正常,False:異常)
		//補    足  ：  なし
		//
		//履    歴  ：  V9.7   04/11/19  (SI4)間々田     新規作成
		//********************************************************************************
		//public static bool GetImageForGainCorrect(CTStatus StatusLabel, ProgressBar theProgressBar, int ViewNum, int IntegNum, CheckState TableRot, float MA, float DownTableDist = 0)		//v10.0変更 by 間々田 2005/01/31 PCフリーズ対策処理 改良版対応 第２引数追加
		//public static bool GetImageForGainCorrect(CTStatus StatusLabel, ProgressBar theProgressBar, int ViewNum, int IntegNum, CheckState TableRot, float MA, float DownTableDist = 0,float yAxisTalbeDist = 0)		//Rev20.00 引数追加 by長野 2015/02/16
        //public static bool GetImageForGainCorrect(CTStatus StatusLabel, ProgressBar theProgressBar, int ViewNum, int IntegNum, CheckState TableRot, float MA, float DownTableDist = 0, float yAxisTalbeDist = 0, float xAxisTableDist = 0,CTStatus StatusLabel2 = null)		//Rev23.40 引数追加 by長野 2016/06/19
        public static bool GetImageForGainCorrect(CTStatus StatusLabel, ProgressBar theProgressBar, int ViewNum, int IntegNum, CheckState TableRot, float MA, float DownTableDist = 0, float yAxisTalbeDist = 0, float xAxisTableDist = 0,CTStatus StatusLabel2 = null)//Rev26.00 引数追加 by長野 2017/04/28
        {
            bool brc = false;
			int rc = 0;
			int Result = 0;					//データ収集結果
			float Val_udab_pos = 0;			//現在の昇降絶対位置
			bool IsOkDownTable = false;		//テーブル下降が実行されたかどうかのフラグ
			bool IsAutoCorrection = false;
			//int RotFlag = 0;				//回転フラグ 'v17.00追加 byやまおか 2010/02/08

			uint[] GainImageTemp = null;			//v19.00 追加 by長野 2012/05/12
			int i = 0;						//v19.00 追加 by長野 2012/05/12
            int ret = 0;

			//戻り値初期化
			bool functionReturnValue = false;

			//変数等初期化
			IsOkDownTable = false;

            //Rev20.00 追加 by長野 2015/02/16
            float Val_yAxis_pos = 0; //現在のY軸位置
            bool IsOkYAxisMoveTable; //Rev20.00 テーブルY軸が移動できたかどうかのフラグ by長野 2015/2/16
            int TargetYAxisPos = 0;
            IsOkYAxisMoveTable = false;

            //Rev23.40 追加 by長野 2016/06/19
            float Val_xAxis_pos = 0; //現在のFCD軸位置
            bool IsOkXAxisMoveTable; //テーブルFCD軸が移動できたかどうかのフラグ
            int TargetXAxisPos = 0;
            IsOkXAxisMoveTable = false;

            //Rev26.00 add FCD軸速度のバックアップ by chouno 2017/10/31
            int OrgFcdSpeed = 0;

            //

			//全自動校正？
            //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            //IsAutoCorrection = modLibrary.IsExistForm(frmAutoCorrection.Instance);
            IsAutoCorrection = modLibrary.IsExistForm("frmAutoCorrection");

            //Rev26.131 / Rev26.14 add by chouno 2018/09/05
            //Ｘ線管干渉制限を解除
            if ((CTSettings.scaninh.Data.table_restriction == 0) && (!modSeqComm.MySeq.stsTableMovePermit))
            {
                modSeqComm.SeqBitWrite("TableMovePermit", true);
                Application.DoEvents();
            }

			//自動校正時以外の処理
			if (!IsAutoCorrection) 
			{
				//タッチパネル操作を禁止（シーケンサ通信が可能な場合）
				modSeqComm.SeqBitWrite("PanelInhibit", true);
			}

			//テーブル下降収集ありの場合  append by 間々田 2003-03-03
			if (DownTableDist != 0) 
			{
                //現在の昇降絶対位置の取得
                //Rev23.10 分岐追加 by長野 2015/11/14
                if (CTSettings.scaninh.Data.cm_mode == 0)
                {
                    Val_udab_pos = CTSettings.mecainf.Data.ud_linear_pos;
                }
                else
                {
                    Val_udab_pos = CTSettings.mecainf.Data.udab_pos;
                }

                //テーブルを下降
                if (modMechaControl.MechaUdIndex(Val_udab_pos + DownTableDist, StatusLabel) != 0)
                {
                    //Rev21.00 追加 by長野 2015/03/08
                    if (CTSettings.t20kinf.Data.ud_type == 0)
                    {
                        //メッセージ表示：テーブル下降に失敗しています。
                        MessageBox.Show(CTResources.LoadResString(9428), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        //メッセージ表示：X線管・検出器昇降に失敗しています。
                        MessageBox.Show(CTResources.LoadResString(22006), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    IsOkDownTable = true;
                }
			}

            //Rev26.41 修正 Yより先にFCDを動かす by chouno 2019/06/25
            //Rev23.40 テーブルFCD軸収集ありの場合  追加 by 長野 2016-05-09
            if (xAxisTableDist != 0)
            {
                //Rev26.00 FCD軸を最高速度に変更 by chouno 2017/10/31
                OrgFcdSpeed = modSeqComm.MySeq.stsYSpeed;
                modSeqComm.SeqWordWrite("YSpeed", modSeqComm.MySeq.stsYMaxSpeed.ToString("0"));

                //現在のFCD軸位置の取得
                Val_xAxis_pos = (float)frmMechaControl.Instance.ntbFCD.Value;
                TargetXAxisPos = (int)((Val_xAxis_pos + xAxisTableDist) * modMechaControl.GVal_FCD_SeqMagnify);
                if (!modSeqComm.MoveFCD(TargetXAxisPos))
                {
                    string buf = null;
                    buf = "";
                    if (modSeqComm.MySeq.stsColdBoxDoorClose == true) //Rev21.00 追加 by長野 2015/06/15
                    {
                        buf = CTResources.LoadResString(22008);

                        MessageBox.Show(buf, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //タッチパネル操作を許可（シーケンサ通信が可能な場合）
                        if (!IsAutoCorrection) modSeqComm.SeqBitWrite("PanelInhibit", false);
                        {
                            //FCD速度を元の速度に戻す //Rev26.00 add by chouno 2017/10/31
                            modSeqComm.SeqWordWrite("YSpeed", OrgFcdSpeed.ToString("0"));
                            return functionReturnValue;
                        }
                    }
                    else if (modSeqComm.MySeq.stsColdBoxPosOK == true) //Rev21.00 追加 by長野 2015/06/15
                    {
                        buf = CTResources.LoadResString(22009);

                        MessageBox.Show(buf, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //タッチパネル操作を許可（シーケンサ通信が可能な場合）
                        if (!IsAutoCorrection) modSeqComm.SeqBitWrite("PanelInhibit", false);
                        {
                            //FCD速度を元の速度に戻す //Rev26.00 add by chouno 2017/10/31
                            modSeqComm.SeqWordWrite("YSpeed", OrgFcdSpeed.ToString("0"));
                            return functionReturnValue;
                        }
                    }
                    else
                    {
                        //エラーの場合：指定されたFCD軸位置まで試料テーブルを移動させることができませんでした。
                        buf = buf + "* " + StringTable.GetResString(StringTable.IDS_MoveErr, StringTable.GetResString(StringTable.IDS_Axis, CTResources.LoadResString(StringTable.IDS_FCD), CTResources.LoadResString(StringTable.IDS_SampleTable))) + "\r";
                    }
                    MessageBox.Show(buf, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    IsOkXAxisMoveTable = true;
                }
            }

            //Rev20.00 テーブルY軸収集ありの場合  追加 by 長野 2015-02-16
            if (yAxisTalbeDist != 0)
            {
                //現在のY軸位置の取得
                Val_yAxis_pos = (float)frmMechaControl.Instance.ntbTableXPos.Value;
                //TargetYAxisPos = (int)((Val_yAxis_pos + yAxisTalbeDist) * 100);
                TargetYAxisPos = (int)((Val_yAxis_pos + yAxisTalbeDist) * modMechaControl.GVal_TableX_SeqMagnify);//Rev23.10 変更 by長野 2015/09/18
                if (!modSeqComm.MoveXpos(TargetYAxisPos))
                {   
                    string buf = null;
                    buf = "";
                    if (modSeqComm.MySeq.stsColdBoxDoorClose == true) //Rev21.00 追加 by長野 2015/06/15
                    {
                        buf = CTResources.LoadResString(22008);

                        MessageBox.Show(buf, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //タッチパネル操作を許可（シーケンサ通信が可能な場合）
                        if (!IsAutoCorrection) modSeqComm.SeqBitWrite("PanelInhibit", false);
                        return functionReturnValue;
                    }
                    else if (modSeqComm.MySeq.stsColdBoxPosOK == true) //Rev21.00 追加 by長野 2015/06/15
                    {
                        buf = CTResources.LoadResString(22009);

                        MessageBox.Show(buf, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //タッチパネル操作を許可（シーケンサ通信が可能な場合）
                        if (!IsAutoCorrection) modSeqComm.SeqBitWrite("PanelInhibit", false);
                        return functionReturnValue;
                    }
                    else
                    {
                        //エラーの場合：指定されたY軸位置まで試料テーブルを移動させることができませんでした。
                        buf = buf + "* " + StringTable.GetResString(StringTable.IDS_MoveErr, CTSettings.AxisName[1], CTResources.LoadResString(StringTable.IDS_SampleTable)) + "\r";
                    }
                    MessageBox.Show(buf, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    IsOkYAxisMoveTable = true;
                }
            }

            ////Rev23.40 テーブルFCD軸収集ありの場合  追加 by 長野 2016-05-09
            //if (xAxisTableDist != 0)
            //{
            //    //Rev26.00 FCD軸を最高速度に変更 by chouno 2017/10/31
            //    OrgFcdSpeed = modSeqComm.MySeq.stsYSpeed;
            //    modSeqComm.SeqWordWrite("YSpeed",modSeqComm.MySeq.stsYMaxSpeed.ToString("0"));

            //    //現在のFCD軸位置の取得
            //    Val_xAxis_pos = (float)frmMechaControl.Instance.ntbFCD.Value;
            //    TargetXAxisPos = (int)((Val_xAxis_pos + xAxisTableDist) * modMechaControl.GVal_FCD_SeqMagnify);
            //    if (!modSeqComm.MoveFCD(TargetXAxisPos))
            //    {
            //        string buf = null;
            //        buf = "";
            //        if (modSeqComm.MySeq.stsColdBoxDoorClose == true) //Rev21.00 追加 by長野 2015/06/15
            //        {
            //            buf = CTResources.LoadResString(22008);

            //            MessageBox.Show(buf, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            //タッチパネル操作を許可（シーケンサ通信が可能な場合）
            //            if (!IsAutoCorrection) modSeqComm.SeqBitWrite("PanelInhibit", false);
            //            {
            //                //FCD速度を元の速度に戻す //Rev26.00 add by chouno 2017/10/31
            //                modSeqComm.SeqWordWrite("YSpeed", OrgFcdSpeed.ToString("0"));
            //                return functionReturnValue;
            //            }
            //        }
            //        else if (modSeqComm.MySeq.stsColdBoxPosOK == true) //Rev21.00 追加 by長野 2015/06/15
            //        {
            //            buf = CTResources.LoadResString(22009);

            //            MessageBox.Show(buf, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            //タッチパネル操作を許可（シーケンサ通信が可能な場合）
            //            if (!IsAutoCorrection) modSeqComm.SeqBitWrite("PanelInhibit", false);
            //            {   
            //                //FCD速度を元の速度に戻す //Rev26.00 add by chouno 2017/10/31
            //                modSeqComm.SeqWordWrite("YSpeed", OrgFcdSpeed.ToString("0"));
            //                return functionReturnValue;
            //            }
            //        }
            //        else
            //        {
            //            //エラーの場合：指定されたFCD軸位置まで試料テーブルを移動させることができませんでした。
            //            buf = buf + "* " + StringTable.GetResString(StringTable.IDS_MoveErr, StringTable.GetResString(StringTable.IDS_Axis, CTResources.LoadResString(StringTable.IDS_FCD), CTResources.LoadResString(StringTable.IDS_SampleTable))) + "\r";
            //        }
            //        MessageBox.Show(buf, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //    else
            //    {
            //        IsOkXAxisMoveTable = true;
            //    }
            //}

            //追加2014/10/07hata_v19.51反映 
            //'検出器が基準位置にいない場合は基準位置に移動する   'v18.00追加 byやまおか 2011/02/10 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //if (CTSettings.DetShiftOn )
            if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
            {
                if (modDetShift.DetShift != modDetShift.DetShiftConstants.DetShift_origin)
                {
                    //'rc = ShiftDet(DetShift_origin)
                    brc = modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_origin, modDetShift.SET_GAIN); //'ゲインをセットする 'v18.00変更 byやまおか 2011/07/04
                }
		    }
            

            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)	//v17.00追加(ここから) byやまおか 2010/02/08
			{
                if (CTSettings.scanParam.FPGainOn == true) 
				{
                    CTSettings.scanParam.FPGainOn = false;
					modCT30K.tmp_on = 1;
				} 
				else 
				{
					modCT30K.tmp_on = 0;
				}
                //RotFlag = 0;		//回転させない
				ScanCorrect.Gain_Image_L = new uint[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
				GainImageTemp = new uint[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
                
                //'シフトスキャン収集の場合   'v18.00追加 byやまおか 2011/03/26
			    if(modScanCorrect.Flg_GainShiftScan == CheckState.Checked )
			    {
                    //Rev23.20 左右シフト対応 by長野 2015/11/19
                    //ScanCorrect.Gain_Image_L_SFT_R = new uint[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size]; //'v18.00追加 byやまおか 2011/02/12
                    ScanCorrect.Gain_Image_L_SFT_R = new uint[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
                    ScanCorrect.Gain_Image_L_SFT_L = new uint[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
                }
		        else
			    {
                   //Rev23.20 左右シフト対応 by長野 2015/11/19
                   //ScanCorrect.Gain_Image_L_SFT = new uint[0];
                   ScanCorrect.Gain_Image_L_SFT_R = new uint[0];
                   ScanCorrect.Gain_Image_L_SFT_L = new uint[0];
                }
            
            }																	//v17.00追加(ここまで) byやまおか 2010/02/08

			//パラメータの設定

			//ビュー数
			ScanCorrect.VIEW_N = ViewNum;
			modScanCorrect.ViewNumAtGain = ViewNum;			//v10.0追加 変数に保存 by 間々田 2005/01/24

			//積算枚数
			modScanCorrect.IntegNumAtGain = IntegNum;		//v10.0追加 変数に保存 by 間々田 2005/01/24

			modScanCorrect.GFlg_GainTableRot = TableRot;

			float maBack = 0;

			//エラー時の扱い
			try 
			{
				//配列の領域確保
				ScanCorrect.GAIN_IMAGE = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];

                //Rev26.00 add by chouno 2017/01/07
                ScanCorrect.GAIN_AIR_IMAGE = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
                ScanCorrect.GAIN_AIR_IMAGE_SFT_L = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
                ScanCorrect.GAIN_AIR_IMAGE_SFT_R = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];

        		//'シフトスキャン収集の場合   'v18.00追加 byやまおか 2011/03/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                if(modScanCorrect.Flg_GainShiftScan == CheckState.Checked )
                {
                    //ScanCorrect.GAIN_IMAGE_SFT = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size]; //'v18.00追加 byやまおか 2011/02/08
                    //Rev23.20 左右シフト対応 by長野 2015/11/19
                    ScanCorrect.GAIN_IMAGE_SFT_R = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
                    ScanCorrect.GAIN_IMAGE_SFT_L = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
                }
		        else
			    {
                    //ScanCorrect.GAIN_IMAGE_SFT = new ushort[0];
                    //Rev23.20 左右シフト対応 by長野 2015/11/19
                    ScanCorrect.GAIN_IMAGE_SFT_R = new ushort[0];
                    ScanCorrect.GAIN_IMAGE_SFT_L = new ushort[0];
                }
                
				//v15.0変更 -1した by 間々田 2009/06/03

				//    '管電流値（入力値）をＸ線制御器にセット
				//    If scaninh.xray_remote = 0 Then SetKVMA , MA

				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
				//
				//    '外部トリガ取込みかつテーブル回転が連続回転の時             'V7.0追加 by 間々田 2003/09/10
				//    'If Use_ExtTrig And TableRotOn Then ExtTrigOn
				//    If (scaninh.ext_trig = 0) And TableRotOn Then ExtTrigOn , ViewNum, IntegNumAtGain


				//    'Ｘ線検出器がフラットパネルの場合                           'V7.0追加 by 間々田 2003/09/25
				//    'If Use_FlatPanel Then
				//    'Ｘ線検出器が浜ホトフラットパネルの場合
				//    If (DetType = DetTypeHama) Then     'v17.00条件変更 byやまおか 2010/02/08
				//        If Not GetDefImage() Then
				//            Result = -1                                         '追加 by 間々田 2005/01/07
				//            GoTo ExitHandler
				//        End If
				//    End If
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
                if (CTSettings.scaninh.Data.xray_remote == 0)	//v15.10条件追加 byやまおか 2009/10/29
				{
					//現在の管電流のバックアップ
					maBack = (float)frmXrayControl.Instance.ntbSetCurrent.Value;

					//「Ｘ線アベイラブル待ち」と表示
					StatusLabel.Status = StringTable.GC_STS_X_AVAIL;

					//Ｘ線ＯＮ処理
					Result = modXrayControl.TryXrayOn(MA: MA);
					if (Result != 0) throw new Exception();
				}

                //'v19.16 修正 by 長野 2013/08/19
                //v19.00 ゲイン撮影時の管電圧と管電流を変数にセットしておく by長野 2012/05/10
				//GainCurrent = (float)frmXrayControl.Instance.ntbSetCurrent.Value;
                if (CTSettings.scaninh.Data.xray_remote == 0)
		        {	
    				GainCurrent = (float)frmXrayControl.Instance.ntbSetCurrent.Value;
		        }
                else
		        {	
                    GainCurrent = (float)frmXrayControl.Instance.cwneMA.Value;
		        }
                
				//キャプチャ開始
				//CaptureStartForScanCorrect StatusLabel, theProgressBar, ViewNumAtGain * IntegNumAtGain
				CaptureStartForScanCorrect(StatusLabel, theProgressBar, modScanCorrect.ViewNumAtGain);		//v17.50変更 by 間々田 2011/01/27 ビュー単位とする

//カメラなしの環境（デバッグ時）の場合：
#if NoCamera
				//デバッグ時はファイルから画像を読み込む
				//Result = ImageOpen(Gain_Image(0), GAIN_CORRECT, h_size, v_size)
				//v17.00変更 byやまおか 2010/02/18
				if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
				{
					Result = ScanCorrect.ImageOpen_long(ref ScanCorrect.Gain_Image_L[0], ScanCorrect.GAIN_CORRECT_L, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                    //追加2014/10/07hata_v19.51反映 
                    if(modScanCorrect.Flg_GainShiftScan == CheckState.Checked )
                        //Result = ScanCorrect.ImageOpen_long(ref ScanCorrect.Gain_Image_L_SFT[0], ScanCorrect.GAIN_CORRECT_L_SFT, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);   //v18.00追加 byやまおか 2011/02/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        //Rev23.20 左右シフト対応 by長野 2015/11/20
                        Result = ScanCorrect.ImageOpen_long(ref ScanCorrect.Gain_Image_L_SFT_R[0], ScanCorrect.GAIN_CORRECT_L_SFT_R, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);   //v18.00追加 byやまおか 2011/02/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        Result = ScanCorrect.ImageOpen_long(ref ScanCorrect.Gain_Image_L_SFT_L[0], ScanCorrect.GAIN_CORRECT_L_SFT_L, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);   //v18.00追加 byやまおか 2011/02/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                }
				else
				{
					Result = ScanCorrect.ImageOpen(ref ScanCorrect.GAIN_IMAGE[0], ScanCorrect.GAIN_CORRECT, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
				}

				Result = (Result == 0 ? 0 : 1902);		//v17.02追加 byやまおか 2010/07/28

//カメラありの環境（通常）の場合：
#else

                int Count = 0;
				ScanCorrect.SUM_IMAGE = new int[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];		//v15.0変更 -1した by 間々田 2009/06/03

				ushort[] tmpTransImage = null;													//v17.02追加 byやまおか 2010/07/06
				tmpTransImage = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];				//v17.02追加 byやまおか 2010/07/06

                //ushort[] tmpTransImage_sft = null;  //v18.00追加 byやまおか 2011/02/12
                ushort[] tmpTransImage_sft_R = null;  //Rev23.20 左右シフト対応 by長野 2015/11/19
                ushort[] tmpTransImage_sft_L = null;  //Rev23.20 左右シフト対応 by長野 2015/11/19

                //'シフトスキャン収集の場合   'v18.00追加 byやまおか 2011/03/26
		        if(modScanCorrect.Flg_GainShiftScan == CheckState.Checked )
                {
                    //Rev23.20 左右シフト対応 by長野 2015/11/19
                    //tmpTransImage_sft = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];//'v18.00追加 byやまおか 2011/02/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    tmpTransImage_sft_R = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
                    tmpTransImage_sft_L = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
                }
                else
                {
                    //Rev23.20 左右シフト対応 by長野 2015/11/19
                    //tmpTransImage_sft = new ushort[0];
                    tmpTransImage_sft_R = new ushort[0];
                    tmpTransImage_sft_L = new ushort[0];
                }

				MilCaptureCallback2Delegate milCaptureCallback2 = new MilCaptureCallback2Delegate(MilCaptureCallback2);

				//ビデオキャプチャから画像を取り込む		
				if (modScanCorrect.GFlg_GainTableRot == CheckState.Checked)		//テーブル回転有無により切替える added by 山本 2002-10-5
				{
					//Result = Gain_Data_Acquire(h_size, v_size, GVal_ScnInteg, 1, IMG1(0), GAIN_IMAGE(0), hDevID1, mrc, VIEW_N, CLng(IIf(TableRotOn, 1, 0)), DCF(IIf(TableRotOn And Use_ExtTrig, 1, 0)), CLng(IIf(Use_FlatPanel, 1, 0)), FR(0)) 'v7.0 FPD対応 by 間々田 2003/09/25
					//PCフリーズ対策処理 改良版  v10.0変更 by 間々田 2005/01/31
					//Result = Gain_Data_Acquire(h_size, v_size, IntegNumAtGain, AddressOf MyCallback, GAIN_IMAGE(0), hDevID1, mrc, ViewNumAtGain, CLng(IIf(TableRotOn, 1, 0)), DCF(IIf(TableRotOn And Use_ExtTrig, 1, 0)), CLng(IIf(Use_FlatPanel, 1, 0)), FR(0)) 'v7.0 FPD対応 by 間々田 2003/09/25
					//Result = Gain_Data_Acquire(h_size, v_size, IntegNumAtGain, AddressOf MyCallback, GAIN_IMAGE(0), hDevID1, mrc, ViewNumAtGain, CLng(IIf(TableRotOn, 1, 0)), dcf(IIf(TableRotOn And (scaninh.ext_trig = 0), 1, 0)), scancondpar.detector, FR(0))  'v11.2変更 by 間々田 2005/10/19
					//Result = Gain_Data_Acquire(frmTransImage.hMil, IntegNumAtGain, AddressOf MilCaptureCallback, GAIN_IMAGE(0), hDevID1, mrc, ViewNumAtGain, CLng(IIf(TableRotOn, 1, 0)), scancondpar.detector, FR(0))  'v11.2変更 by 間々田 2005/10/19
					//Result = Gain_Data_Acquire(frmTransImage.hMil, IntegNumAtGain, AddressOf MilCaptureCallback, SUM_IMAGE(0), hDevID1, mrc, ViewNumAtGain, CLng(IIf(TableRotOn, 1, 0)), scancondpar.detector, FR(0))  'v11.2変更 by 間々田 2005/10/19
					//Result = Gain_Data_Acquire(frmTransImage.hMil, IntegNumAtGain, AddressOf MilCaptureCallback2, TransImage(0), SUM_IMAGE(0), hDevID1, mrc, ViewNumAtGain, CLng(IIf(TableRotOn, 1, 0)), scancondpar.detector, FR(0))  'v11.2変更 by 間々田 2005/10/19
					//Result = Gain_Data_Acquire(frmTransImage.hMil, hPke, DestImage(0), TransImage(0), IntegNumAtGain, AddressOf MilCaptureCallback2, TransImage(0), SUM_IMAGE(0), hDevID1, mrc, ViewNumAtGain, CLng(IIf(TableRotOn, 1, 0)), scancondpar.detector, FR(0))  'v17.00changed by 山本　2009-09-16

                    //Result = Gain_Data_Acquire(frmTransImage.Instance.hMil, modCT30K.hPke, ref TransImage[0], modScanCorrect.IntegNumAtGain, milCaptureCallback2,
                    //                                    ref TransImage[0], ref ScanCorrect.SUM_IMAGE[0], modDeclare.hDevID1, ref modDeclare.mrc, modScanCorrect.ViewNumAtGain,
                    //                                    Convert.ToInt32(modCT30K.TableRotOn ? 1 : 0), CTSettings.scancondpar.Data.detector, modCT30K.FR[0]);							//v17.50変更 引数DestImage(0)削除 by 間々田 2011/01/14

                    TransImage = CTSettings.transImageControl.GetImage();
					Result = Gain_Data_Acquire((int)Pulsar.hMil, (int)Pulsar.hPke, ref TransImage[0], modScanCorrect.IntegNumAtGain, milCaptureCallback2,
                                                        ref TransImage[0], ref ScanCorrect.SUM_IMAGE[0], modDeclare.hDevID1, ref modDeclare.mrc, modScanCorrect.ViewNumAtGain,
                                                        Convert.ToInt32(modCT30K.TableRotOn ? 1 : 0), CTSettings.scancondpar.Data.detector, CTSettings.detectorParam.FR[0]);							//v17.50変更 引数DestImage(0)削除 by 間々田 2011/01/14

					Count = modScanCorrect.IntegNumAtGain * modScanCorrect.ViewNumAtGain;
				} 
				else 
				{
					//Result = II_Data_Acquire(h_size, v_size, VIEW_N * GVal_ScnInteg, 1, IMG1(0), GAIN_IMAGE(0), 0, DCF(0), CLng(IIf(Use_FlatPanel, 1, 0)), FR(0)) 'changed by 山本　2004-8-3　引数にFR(0)追加
					//PCフリーズ対策処理 改良版  v10.0変更 by 間々田 2005/01/31
					//Result = II_Data_Acquire(h_size, v_size, ViewNumAtGain * IntegNumAtGain, AddressOf MyCallback, GAIN_IMAGE(0), 0, dcf(0), scancondpar.detector, FR(0))  'changed by 山本　2004-8-3　引数にFR(0)追加

					//v15.0変更 by 間々田 2009/01/29
					//Count = MilCaptureStart(frmTransImage.hMil, AddressOf MilCaptureCallback, SUM_IMAGE(0), ViewNumAtGain * IntegNumAtGain)
					//Count = MilCaptureStart2(frmTransImage.hMil, AddressOf MilCaptureCallback2, TransImage(0), SUM_IMAGE(0), ViewNumAtGain * IntegNumAtGain)
                    switch (CTSettings.detectorParam.DetType) 
					{
						//v17.00追加(ここから) byやまおか 2010/02/08
						case DetectorConstants.DetTypeII:
						case DetectorConstants.DetTypeHama:
							//Count = MilCaptureStart2(frmTransImage.hMil, AddressOf MilCaptureCallback2, TransImage(0), SUM_IMAGE(0), ViewNumAtGain * IntegNumAtGain)

							//v17.50以下に変更 2011/01/05 by 間々田
							//Count = IIf(GetCaptureSumImage(frmTransImage.hMil, hPke, ViewNumAtGain * IntegNumAtGain, TransImage(0), SUM_IMAGE(0), AddressOf MilCaptureCallback2) = 0, ViewNumAtGain * IntegNumAtGain, 0)
							//v17.66 プログレスバーと実際の進行状況が会わないため修正　by 長野 2012/12/28
                            TransImage = CTSettings.transImageControl.GetImage();
					        Count = (GetCaptureSumImage((int)Pulsar.hMil, (int)Pulsar.hPke, modScanCorrect.ViewNumAtGain, modScanCorrect.IntegNumAtGain, ref TransImage[0], ref ScanCorrect.SUM_IMAGE[0], milCaptureCallback2) == 0 ? modScanCorrect.ViewNumAtGain * modScanCorrect.IntegNumAtGain : 0);
                            
                            //CTSettings.transImageControl.SetTransImage(TransImage);	
                            break;

						case DetectorConstants.DetTypePke:
							//Count = PkeCaptureStart2(frmTransImage.hPke, AddressOf MilCaptureCallback2, DestImage(0), TransImage(0), SUM_IMAGE(0), ViewNumAtGain * IntegNumAtGain, RotFlag)
							//Count = PkeCaptureGain(hPke, AddressOf MilCaptureCallback2, DestImage(0), TransImage(0), Gain_Image_L(0), ViewNumAtGain * IntegNumAtGain)     '変更　山本 2009-10-20
							//PkeFPDは透視画像とゲイン画像が一致しないため表示用に1枚だけTransImageを更新して、本当のゲインはtmpTransImageを使う   'v17.02変更 byやまおか 2010/07/13
							ScanCorrect.CaptureSetup((int)Pulsar.hMil, (int)Pulsar.hPke);			//v17.50追加 by 間々田 2011/01/05
							//PkeCapture hPke, DestImage(0), TransImage(0)
							ScanCorrect.PkeCapture((int)Pulsar.hPke, ref TransImage[0]);						//v17.50変更 by 間々田 2011/01/05
							//Count = PkeCaptureGain(hPke, AddressOf MilCaptureCallback2, DestImage(0), tmpTransImage(0), Gain_Image_L(0), ViewNumAtGain * IntegNumAtGain)
							//Count = PkeCaptureGain(hPke, AddressOf MilCaptureCallback2, tmpTransImage(0), Gain_Image_L(0), ViewNumAtGain * IntegNumAtGain) 'v17.50変更 by 間々田 2010/12/28
                            
                            //Rev26.12 修正 by chouno 2018/04/05
                            modCT30K.PauseForDoEvents(1);
                            ScanCorrect.CaptureSeqStop((int)Pulsar.hMil, (int)Pulsar.hPke);

                            //仮りテスト
                            //TransImage = CTSettings.transImageControl.GetImage(); 

                            //'v19.50 透視画像更新を追加 by長野 2014/01/27
                            CTSettings.transImageControl.SetTransImage(TransImage);
                            frmTransImage.Instance.TransImageCtrl.Update(false, 0);

                            //プログレスバーと実際の進行状況が合わない不具合を修正							
                            Count = PkeCaptureGain((int)Pulsar.hPke, milCaptureCallback2, ref tmpTransImage[0], ref ScanCorrect.Gain_Image_L[0], modScanCorrect.ViewNumAtGain, modScanCorrect.IntegNumAtGain);		//v17.66 引数変更 by 長野 2011/12/28

                            //仮りテスト
                            //CTSettings.transImageControl.SetTransImage(TransImage);	
                            
   					        //'v19.50 最後まで完了したら確実にプログレスバーを最大にする。 by長野 2013/12/17
					        theProgressBar.Value = modScanCorrect.ViewNumAtGain; //'ビュー単位の更新なのでViewNumAtGainでOK

                            System.Windows.Forms.Application.DoEvents();

                            //Rev23.20 Result追加 by長野 2016/01/23
                            Result = (Count == modScanCorrect.ViewNumAtGain * modScanCorrect.IntegNumAtGain ? 0 : 1902);

					        //'シフトスキャン収集ありなら     'v18.00追加 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
					        //if (modScanCorrect.Flg_GainShiftScan == CheckState.Checked)
                            if (modScanCorrect.Flg_GainShiftScan == CheckState.Checked && Result == 0) //Rev23.20 通常位置が問題なければシフトも収拾 by長野 2016/01/21
                            {
                                //Rev26.10  add シフトスキャンの場合は、基準位置を左シフト位置と考える。 by chouno 2018/01/13
                                //基準位置でのエアデータを持っておく。
                                ScanCorrect.CaptureSetup((int)Pulsar.hMil, (int)Pulsar.hPke);					//v17.50追加 by 間々田 2011/01/05
                                ScanCorrect.PkeCapture((int)Pulsar.hPke, ref ScanCorrect.GAIN_AIR_IMAGE_SFT_L[0]);			//v26.00 change by chouno 2017/01/06
                                
                                //Rev26.12 修正 by chouno 2018/04/05
                                modCT30K.PauseForDoEvents(1);
                                ScanCorrect.CaptureSeqStop((int)Pulsar.hMil, (int)Pulsar.hPke);

                                ScanCorrect.ImageSave(ref ScanCorrect.GAIN_AIR_IMAGE_SFT_L[0], ScanCorrect.GAIN_CORRECT_AIR_SFT_L, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                                //<---

						        //'シフトする
						        //'If ShiftDet(DetShift_forward) Then
						        if( modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_forward, modDetShift.UNSET_GAIN))   //'ゲインをセットしない 'v18.00変更 byやまおか 2011/07/04
						        {
                                    //'シフト成功なら収集開始
							        //'frmGainCor.Caption = LoadResString(IDS_CorGain) & " - " & "シフトスキャン収集"
							        //frmGainCor.Instance.Text = CTResources.LoadResString(StringTable.IDS_CorGain) + " - " + CTResources.LoadResString(17519); //'v19.50 ストリングテーブル化 by長野 2013/11/21
                                    //Rev25.00 Wスキャン変更 by長野 2016/08/03
                                    frmGainCor.Instance.Text = CTResources.LoadResString(StringTable.IDS_CorGain) + " - " + (CTSettings.W_ScanOn == true ? CTResources.LoadResString(25006) : CTResources.LoadResString(17519)); //'v19.50 ストリングテーブル化 by長野 2013/11/21
                                    
                                    //'キャプチャ開始準備
							        CaptureStartForScanCorrect(StatusLabel, theProgressBar, modScanCorrect.ViewNumAtGain * modScanCorrect.IntegNumAtGain);
							
                                    //'ゲインデータをメモリからプリロードする     'v18.00追加 byやまおか 2011/02/26
							        //Rev23.20 左右シフト対応 by長野 2015/11/19
                                    //ret = ScanCorrect.PkeSetGainData((int)Pulsar.hPke, ref ScanCorrect.Gain_Image_L_SFT[0], 1, ScanCorrect.GAIN_CORRECT_L_SFT);
                                    ret = ScanCorrect.PkeSetGainData((int)Pulsar.hPke, ref ScanCorrect.Gain_Image_L_SFT_R[0], 1, ScanCorrect.GAIN_CORRECT_L_SFT_R);
							
                                    //'透視画面の更新(1枚だけ)
							        ScanCorrect.CaptureSetup((int)Pulsar.hMil, (int)Pulsar.hPke); //'v17.50追加 by 間々田 2011/01/05 'v19.50 追加 by長野 2013/12/16
							
                                    //'PkeCapture hPke, DestImage(0), TransImage(0)
							        ScanCorrect.PkeCapture((int)Pulsar.hPke, ref TransImage[0]); //'v19.50 v19.41とv18.02の統合 2013/11/07 by長野

                                    //Rev26.12 修正 by chouno 2018/04/05
                                    modCT30K.PauseForDoEvents(1);
                                    ScanCorrect.CaptureSeqStop((int)Pulsar.hMil, (int)Pulsar.hPke);

                                    //'v19.50 透視画像更新を追加 by長野 2014/01/27
                                    CTSettings.transImageControl.SetTransImage(TransImage);
                                    frmTransImage.Instance.TransImageCtrl.Update(false, 0);
							
							        //'ゲイン収集(シフトスキャン収集)
							        //'Count = PkeCaptureGain(hPke, AddressOf MilCaptureCallback2, DestImage(0), tmpTransImage_sft(0), Gain_Image_L_SFT(0), ViewNumAtGain * IntegNumAtGain)
							        //Rev23.20 左右シフト対応 by長野 2015/11/19
                                    //Count = PkeCaptureGain((int)Pulsar.hPke, milCaptureCallback2, ref tmpTransImage_sft[0],ref ScanCorrect.Gain_Image_L_SFT[0], modScanCorrect.ViewNumAtGain, modScanCorrect.IntegNumAtGain); //'v17.66 引数変更 by 長野 2011/12/28'v19.50 v19.41とv18.02の統合 2013/11/07 by長野
							        //Rev23.20 左右シフト対応 by長野 2015/11/19
                                    //Count = PkeCaptureGain((int)Pulsar.hPke, milCaptureCallback2, ref tmpTransImage_sft[0],ref ScanCorrect.Gain_Image_L_SFT_R[0], modScanCorrect.ViewNumAtGain, modScanCorrect.IntegNumAtGain); //'v17.66 引数変更 by 長野 2011/12/28'v19.50 v19.41とv18.02の統合 2013/11/07 by長野
                                    Count = PkeCaptureGain((int)Pulsar.hPke, milCaptureCallback2, ref tmpTransImage_sft_R[0],ref ScanCorrect.Gain_Image_L_SFT_R[0], modScanCorrect.ViewNumAtGain, modScanCorrect.IntegNumAtGain); //'v17.66 引数変更 by 長野 2011/12/28'v19.50 v19.41とv18.02の統合 2013/11/07 by長野
							
							        //'v19.50 最後まで完了したら確実にプログレスバーを最大にする。 by長野 2013/12/17
							        theProgressBar.Value = modScanCorrect.IntegNumAtGain * modScanCorrect.ViewNumAtGain;
						
                                    //Rev25.00 test by長野 2016/08/18 --->
                                    //ret = ScanCorrect.PkeSetGainData((int)Pulsar.hPke, ref ScanCorrect.Gain_Image_L_SFT_R[0], 1, "");
                                    ////v19.00 ゲイン校正後のエアデータを持っておく。
                                    //ScanCorrect.CaptureSetup((int)Pulsar.hMil, (int)Pulsar.hPke);					//v17.50追加 by 間々田 2011/01/05
                                    ////PkeCapture hPke, DestImage(0), TransImage(0)
                                    //ScanCorrect.PkeCapture((int)Pulsar.hPke, ref TransImage[0]);					//v17.50変更 by 間々田 2011/01/05
                                    //ScanCorrect.Cal_Mean_short2(ref TransImage[0], 1024, 1024, 12, 12, ref Rmean);
                                    //<---

                                    //Rev25.00 ゲイン後のエアデータを1枚撮影　---> 2016/09/24 by Chouno
                                    for (i = 0; i <= CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size - 1; i++)
                                    {
                                        GainImageTemp[i] = ScanCorrect.Gain_Image_L_SFT_R[i];
                                    }

                                    ret = ScanCorrect.PkeSetGainData((int)Pulsar.hPke, ref GainImageTemp[0], 1, ""); //'v19.50 v18.02とv19.41との統合 by長野 2013/12/17

                                    //ストリングテーブル化　'v17.60 by 長野　2011/05/22
                                    //If ret = 1 Then MsgBox "ゲイン校正データをセットできませんでした。", vbCritical
                                    if (ret == 1) MessageBox.Show(CTResources.LoadResString(20004), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    //v19.00 ゲイン校正後のエアデータを持っておく。
                                    ScanCorrect.CaptureSetup((int)Pulsar.hMil, (int)Pulsar.hPke);					//v17.50追加 by 間々田 2011/01/05
                                    //PkeCapture hPke, DestImage(0), TransImage(0)
                                    
                                    //ScanCorrect.PkeCapture((int)Pulsar.hPke, ref TransImage[0]);					//v17.50変更 by 間々田 2011/01/05
                                    ScanCorrect.PkeCapture((int)Pulsar.hPke, ref ScanCorrect.GAIN_AIR_IMAGE_SFT_R[0]);			//v26.00 change by chouno 2017/01/06

                                    //Rev26.12 修正 by chouno 2018/04/05
                                    modCT30K.PauseForDoEvents(1);
                                    ScanCorrect.CaptureSeqStop((int)Pulsar.hMil, (int)Pulsar.hPke);

                                    //v19.00 画像を保存しておく
                                    //ScanCorrect.ImageSave(ref TransImage[0], @"C:\CT\TEMP\WSCAN_R_AIR.cor", CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                                    //Rev26.00 change by chouno 2017/01/06
                                    ScanCorrect.ImageSave(ref ScanCorrect.GAIN_AIR_IMAGE_SFT_R[0], ScanCorrect.GAIN_CORRECT_AIR_SFT_R, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                                    //<---

                                    //'基準位置に戻す
							        //'If Not ShiftDet(DetShift_origin) Then Count = 0     'シフト失敗ならエラーとする
                                    //Rev23.20 左右シフト対応 by長野 2015/11/19
                                    //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                                    //if (CTSettings.scaninh.Data.lr_sft == 0) //左シフト位置へ移動
                                    if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn) //左シフト位置へ移動
                                    {
                                        if (!modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_backward, modDetShift.UNSET_GAIN)) Count = 0; //'シフト失敗ならエラーとする  'ゲインをセットしない 'v18.00変更 byやまおか 2011/07/04
                                    }
                                    else
                                    {
                                        if (!modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_origin, modDetShift.UNSET_GAIN)) Count = 0; //'シフト失敗ならエラーとする  'ゲインをセットしない 'v18.00変更 byやまおか 2011/07/04
                                    }

                                }
                                else
						        {	//'シフト失敗ならエラーとする
							        Count = 0;
						        }

                                //Rev23.20 Result追加 by長野 2016/01/23
                                Result = (Count == modScanCorrect.ViewNumAtGain * modScanCorrect.IntegNumAtGain ? 0 : 1902);

                                //Rev23.20 左右シフト対応 by長野 2015/11/19
                                //if (CTSettings.scaninh.Data.lr_sft == 0 && Count != 0)
                                //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                                //if (CTSettings.scaninh.Data.lr_sft == 0 && Result == 0) //Rev23.20 Resultを条件に追加 by長野 2016/01/21
                                if ((CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn) && Result == 0) //Rev23.20 Resultを条件に追加 by長野 2016/01/21
                                {
                                    //'シフト成功なら収集開始
                                    //'frmGainCor.Caption = LoadResString(IDS_CorGain) & " - " & "シフトスキャン収集"
                                    //frmGainCor.Instance.Text = CTResources.LoadResString(StringTable.IDS_CorGain) + " - " + CTResources.LoadResString(17519); //'v19.50 ストリングテーブル化 by長野 2013/11/21
                                    //Rev25.00 Wスキャン変更 by長野 2016/08/03
                                    frmGainCor.Instance.Text = CTResources.LoadResString(StringTable.IDS_CorGain) + " - " + (CTSettings.W_ScanOn == true? CTResources.LoadResString(25006):CTResources.LoadResString(17519)); //'v19.50 ストリングテーブル化 by長野 2013/11/21
                                    
                                    //'キャプチャ開始準備
                                    CaptureStartForScanCorrect(StatusLabel, theProgressBar, modScanCorrect.ViewNumAtGain * modScanCorrect.IntegNumAtGain);

                                    //'ゲインデータをメモリからプリロードする     'v18.00追加 byやまおか 2011/02/26
                                    //Rev23.20 左右シフト対応 by長野 2015/11/19
                                    //ret = ScanCorrect.PkeSetGainData((int)Pulsar.hPke, ref ScanCorrect.Gain_Image_L_SFT[0], 1, ScanCorrect.GAIN_CORRECT_L_SFT);
                                    ret = ScanCorrect.PkeSetGainData((int)Pulsar.hPke, ref ScanCorrect.Gain_Image_L_SFT_L[0], 1, ScanCorrect.GAIN_CORRECT_L_SFT_L);

                                    //'透視画面の更新(1枚だけ)
                                    ScanCorrect.CaptureSetup((int)Pulsar.hMil, (int)Pulsar.hPke); //'v17.50追加 by 間々田 2011/01/05 'v19.50 追加 by長野 2013/12/16

                                    //'PkeCapture hPke, DestImage(0), TransImage(0)
                                    ScanCorrect.PkeCapture((int)Pulsar.hPke, ref TransImage[0]); //'v19.50 v19.41とv18.02の統合 2013/11/07 by長野

                                    //Rev26.12 修正 by chouno 2018/04/05
                                    modCT30K.PauseForDoEvents(1);
                                    ScanCorrect.CaptureSeqStop((int)Pulsar.hMil, (int)Pulsar.hPke);

                                    //'v19.50 透視画像更新を追加 by長野 2014/01/27
                                    CTSettings.transImageControl.SetTransImage(TransImage);
                                    frmTransImage.Instance.TransImageCtrl.Update(false, 0);

                                    //'ゲイン収集(シフトスキャン収集)
                                    //'Count = PkeCaptureGain(hPke, AddressOf MilCaptureCallback2, DestImage(0), tmpTransImage_sft(0), Gain_Image_L_SFT(0), ViewNumAtGain * IntegNumAtGain)
                                    //Rev23.20 左右シフト対応 by長野 2015/11/19
                                    //Count = PkeCaptureGain((int)Pulsar.hPke, milCaptureCallback2, ref tmpTransImage_sft[0],ref ScanCorrect.Gain_Image_L_SFT[0], modScanCorrect.ViewNumAtGain, modScanCorrect.IntegNumAtGain); //'v17.66 引数変更 by 長野 2011/12/28'v19.50 v19.41とv18.02の統合 2013/11/07 by長野
                                    //Rev23.20 左右シフト対応 by長野 2015/11/19
                                    //Count = PkeCaptureGain((int)Pulsar.hPke, milCaptureCallback2, ref tmpTransImage_sft[0],ref ScanCorrect.Gain_Image_L_SFT_R[0], modScanCorrect.ViewNumAtGain, modScanCorrect.IntegNumAtGain); //'v17.66 引数変更 by 長野 2011/12/28'v19.50 v19.41とv18.02の統合 2013/11/07 by長野
                                    Count = PkeCaptureGain((int)Pulsar.hPke, milCaptureCallback2, ref tmpTransImage_sft_L[0], ref ScanCorrect.Gain_Image_L_SFT_L[0], modScanCorrect.ViewNumAtGain, modScanCorrect.IntegNumAtGain); //'v17.66 引数変更 by 長野 2011/12/28'v19.50 v19.41とv18.02の統合 2013/11/07 by長野

                                    //'v19.50 最後まで完了したら確実にプログレスバーを最大にする。 by長野 2013/12/17
                                    theProgressBar.Value = modScanCorrect.IntegNumAtGain * modScanCorrect.ViewNumAtGain;

                                    //Rev25.00 ゲイン後のエアデータを1枚撮影　---> 2016/09/24 by Chouno
                                    for (i = 0; i <= CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size - 1; i++)
                                    {
                                        GainImageTemp[i] = ScanCorrect.Gain_Image_L_SFT_L[i];
                                    }

                                    ret = ScanCorrect.PkeSetGainData((int)Pulsar.hPke, ref GainImageTemp[0], 1, ""); //'v19.50 v18.02とv19.41との統合 by長野 2013/12/17

                                    //ストリングテーブル化　'v17.60 by 長野　2011/05/22
                                    //If ret = 1 Then MsgBox "ゲイン校正データをセットできませんでした。", vbCritical
                                    if (ret == 1) MessageBox.Show(CTResources.LoadResString(20004), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    //v19.00 ゲイン校正後のエアデータを持っておく。
                                    ScanCorrect.CaptureSetup((int)Pulsar.hMil, (int)Pulsar.hPke);					//v17.50追加 by 間々田 2011/01/05
                                    //PkeCapture hPke, DestImage(0), TransImage(0)
                                    //ScanCorrect.PkeCapture((int)Pulsar.hPke, ref TransImage[0]);					//v17.50変更 by 間々田 2011/01/05
                                    //Rev26.00
                                    ScanCorrect.PkeCapture((int)Pulsar.hPke, ref ScanCorrect.GAIN_AIR_IMAGE_SFT_L[0]);	//v26.00 change by chouno 2017/01/06

                                    //Rev26.12 修正 by chouno 2018/04/05
                                    modCT30K.PauseForDoEvents(1);
                                    ScanCorrect.CaptureSeqStop((int)Pulsar.hMil, (int)Pulsar.hPke);

                                    //v19.00 画像を保存しておく
                                    //ScanCorrect.ImageSave(ref TransImage[0], @"C:\CT\TEMP\WSCAN_L_AIR.cor", CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                                    //Rev26.00 change by chouno 2017/01/06
                                    ScanCorrect.ImageSave(ref ScanCorrect.GAIN_AIR_IMAGE_SFT_L[0], ScanCorrect.GAIN_CORRECT_AIR_SFT_L, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                                    //<---

                                    //Rev25.00 test by長野 2016/08/18 --->
                                    //ret = ScanCorrect.PkeSetGainData((int)Pulsar.hPke, ref ScanCorrect.Gain_Image_L_SFT_L[0], 1, "");
                                    ////v19.00 ゲイン校正後のエアデータを持っておく。
                                    //ScanCorrect.CaptureSetup((int)Pulsar.hMil, (int)Pulsar.hPke);					//v17.50追加 by 間々田 2011/01/05
                                    ////PkeCapture hPke, DestImage(0), TransImage(0)
                                    //ScanCorrect.PkeCapture((int)Pulsar.hPke, ref TransImage[0]);					//v17.50変更 by 間々田 2011/01/05
                                    //ScanCorrect.Cal_Mean_short2(ref TransImage[0], 1024, 1024, 12, 12, ref Lmean);
                                    //<---

                                    //'基準位置に戻す
                                    //'If Not ShiftDet(DetShift_origin) Then Count = 0     'シフト失敗ならエラーとする
                                    if (!modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_origin, modDetShift.UNSET_GAIN)) Count = 0; //'シフト失敗ならエラーとする  'ゲインをセットしない 'v18.00変更 byやまおか 2011/07/04
                                }
					        }
                            break;
                    
                    }				//v17.00追加(ここまで) byやまおか 2010/02/08

					Result = (Count == modScanCorrect.ViewNumAtGain * modScanCorrect.IntegNumAtGain ? 0 : 1902);
				}

				//If (Count > 0) Then DivImage_short SUM_IMAGE(0), GAIN_IMAGE(0), Count, h_size, v_size      '削除　山本 2009-10-20
				//v17.00修正 byやまおか 2010/03/04
                if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypeII || CTSettings.detectorParam.DetType == DetectorConstants.DetTypeHama) 
				{
					if (Count > 0) ScanCorrect.DivImage_short(ref ScanCorrect.SUM_IMAGE[0], ref ScanCorrect.GAIN_IMAGE[0], Count, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
				} 
				else 
				{
					//    rc = ImageCopy_short(TransImage(0), GAIN_IMAGE(0), h_size, v_size)    'パーキンエルマーFPDの場合　v17.00 この関数を使うと落ちるため下記VBでコピーすることにした
					//For i = 0 To h_size * v_size - 1
					//    GAIN_IMAGE(i) = TransImage(i)
					//Next i

                    //Rev23.20 左右シフト対応 by長野 2015/11/19
                    if (modScanCorrect.Flg_GainShiftScan == CheckState.Checked)
                    {
                        //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                        //if (CTSettings.scaninh.Data.lr_sft == 0)
                        if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
                        {
                            //v19.00 GAIN_IMAGE_Lをtempファイルへコピーする by長野 2012/05/12
                            for (i = 0; i <= CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size - 1; i++)
                            {
                                GainImageTemp[i] = ScanCorrect.Gain_Image_L_SFT_R[i];
                            }
                            rc = ScanCorrect.ImageCopy_short(ref tmpTransImage[0], ref ScanCorrect.GAIN_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                            rc = ScanCorrect.ImageCopy_short(ref tmpTransImage_sft_L[0], ref ScanCorrect.GAIN_IMAGE_SFT_L[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                            rc = ScanCorrect.ImageCopy_short(ref tmpTransImage_sft_R[0], ref ScanCorrect.GAIN_IMAGE_SFT_R[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                
                        }
                        else
                        {
                            //v19.00 GAIN_IMAGE_Lをtempファイルへコピーする by長野 2012/05/12
                            for (i = 0; i <= CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size - 1; i++)
                            {
                                GainImageTemp[i] = ScanCorrect.Gain_Image_L[i];
                            }
                            rc = ScanCorrect.ImageCopy_short(ref tmpTransImage[0], ref ScanCorrect.GAIN_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                            rc = ScanCorrect.ImageCopy_short(ref tmpTransImage_sft_R[0], ref ScanCorrect.GAIN_IMAGE_SFT_R[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                        }
                    }
                    else
                    {
                        //v19.00 GAIN_IMAGE_Lをtempファイルへコピーする by長野 2012/05/12
                        for (i = 0; i <= CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size - 1; i++)
                        {
                            GainImageTemp[i] = ScanCorrect.Gain_Image_L[i];
                        }
                        rc = ScanCorrect.ImageCopy_short(ref tmpTransImage[0], ref ScanCorrect.GAIN_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                    }

                    if (modScanCorrect.Flg_GainHaFuOfScan == CheckState.Checked)
                    {
                        rc = ScanCorrect.ImageCopy_short(ref tmpTransImage[0], ref ScanCorrect.GAIN_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                    }
                    //Rev23.20 左右シフト対応 by長野 2015/11/19
					//rc = ScanCorrect.ImageCopy_short(ref tmpTransImage[0], ref ScanCorrect.GAIN_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);		//関数化  'v17.10変更 byやまおか 2010/08/19

                    ////追加2014/10/07hata_v19.51反映 
                    //if( modScanCorrect.Flg_GainShiftScan == CheckState.Checked) //'v18.00追加 byやまおか 2011/02/08 'v18.02とv19.41との統合 by長野 2013/12/17
                    //{
                    //    //rc = ScanCorrect.ImageCopy_short(ref tmpTransImage_sft[0],ref ScanCorrect.GAIN_IMAGE_SFT[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);    //'関数化  'v17.10変更 byやまおか 2010/08/19
                    //    //Rev23.20 左右シフト対応 by長野 2015/11/19
                    //    rc = ScanCorrect.ImageCopy_short(ref tmpTransImage_sft_R[0], ref ScanCorrect.GAIN_IMAGE_SFT_R[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                    //    rc = ScanCorrect.ImageCopy_short(ref tmpTransImage_sft_L[0], ref ScanCorrect.GAIN_IMAGE_SFT_L[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                    //}

                    //'v19.50 v19.00改造時のコメントを追加 by長野 2013/12/17
			        //'FPDの時、エアデータが必要になるため、ここで一枚キャプチャしておく
					//一時ファイルを検出器にセット
                    //変更2014/10/07hata_v19.51反映
                    //ret = ScanCorrect.PkeSetGainData((int)Pulsar.hPke, 0, ref GainImageTemp[0], 1);
                    ret = ScanCorrect.PkeSetGainData((int)Pulsar.hPke, ref GainImageTemp[0], 1, ""); //'v19.50 v18.02とv19.41との統合 by長野 2013/12/17

					//ストリングテーブル化　'v17.60 by 長野　2011/05/22
					//If ret = 1 Then MsgBox "ゲイン校正データをセットできませんでした。", vbCritical
					if (ret == 1) MessageBox.Show(CTResources.LoadResString(20004), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

					//v19.00 ゲイン校正後のエアデータを持っておく。
                    ScanCorrect.CaptureSetup((int)Pulsar.hMil, (int)Pulsar.hPke);					//v17.50追加 by 間々田 2011/01/05
					//PkeCapture hPke, DestImage(0), TransImage(0)
					//ScanCorrect.PkeCapture((int)Pulsar.hPke, ref TransImage[0]);					//v17.50変更 by 間々田 2011/01/05
                    //Rev26.00 change by chouno 2017/01/06
                    ScanCorrect.PkeCapture((int)Pulsar.hPke, ref ScanCorrect.GAIN_AIR_IMAGE[0]);

                    //Rev26.12 修正 by chouno 2018/04/05
                    modCT30K.PauseForDoEvents(1);
                    ScanCorrect.CaptureSeqStop((int)Pulsar.hMil, (int)Pulsar.hPke);

                    //v19.00 画像を保存しておく
					//ScanCorrect.ImageSave(ref TransImage[0], ScanCorrect.GAIN_CORRECT_AIR, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                    //Rev26.00 change by chouno 2017/01/06
                    //ScanCorrect.ImageSave(ref ScanCorrect.AIR_IMAGE[0], ScanCorrect.GAIN_CORRECT_AIR, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                    ScanCorrect.ImageSave(ref ScanCorrect.GAIN_AIR_IMAGE[0], ScanCorrect.GAIN_CORRECT_AIR, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);

					//v19.00 ゲイン校正を確定させてはいないので、校正前のデータをセットしておく。by長野 2012/05/10
                    //変更2014/10/07hata_v19.51反映
                    //ret = ScanCorrect.PkeSetGainData((int)Pulsar.hPke, 1, ref ScanCorrect.Gain_Image_L[0], 1);
                    //ret = ScanCorrect.PkeSetGainData((int)Pulsar.hPke, ref ScanCorrect.Gain_Image_L[0], 1, ScanCorrect.GAIN_CORRECT_L); //'v19.50 'v18.02とv19.41との統合 by長野 2013/12/17
                    //Rev20.00 修正 by長野 2014/12/04
                    ret = ScanCorrect.PkeSetGainData((int)Pulsar.hPke, ref GainImageTemp[0], 1, ScanCorrect.GAIN_CORRECT_L); //'v19.50 'v18.02とv19.41との統合 by長野 2013/12/17
                    
                    //ストリングテーブル化　'v17.60 by 長野　2011/05/22
					//If ret = 1 Then MsgBox "ゲイン校正データをセットできませんでした。", vbCritical
					if (ret == 1) MessageBox.Show(CTResources.LoadResString(20004), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				tmpTransImage = null;				//v17.02追加 byやまおか 2010/07/13
           		//if(modScanCorrect.Flg_GainShiftScan == CheckState.Checked) tmpTransImage_sft = null; //'v18.00追加 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                //Rev23.20 左右シフト対応 by長野 2015/11/19
                if (modScanCorrect.Flg_GainShiftScan == CheckState.Checked)
                {
                    tmpTransImage_sft_R = null;
                    tmpTransImage_sft_L = null;
                }

#endif

				//v17.00追加 byやまおか 2010/02/08
                if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
				{
                    if (modCT30K.tmp_on == 1) CTSettings.scanParam.FPGainOn = true;		//ゲイン補正フラッグを元に戻す 2009-09-30
				}

				//キャプチャ終了
				CaptureEndForScanCorrect();

				//Ｘ線をＯＦＦ：ただし、自動校正中の場合はＸ線をＯＦＦしない
				//if (!IsAutoCorrection)
                //Rev26.00 全自動校正、かつ、次の校正がスキャン位置校正の場合OFFしない by chouno 2017/04/28
                if (!IsAutoCorrection || (StatusLabel2.Status != StringTable.GC_STS_STANDBY_NG))
                {
                    if (CTSettings.scaninh.Data.xray_remote == 0) modXrayControl.XrayOff();
				}

			}
			catch
			{
                //処理無し	
			}
          
			//Ｘ線制御器の管電流値を元の値に戻す
            if (CTSettings.scaninh.Data.xray_remote == 0)
			{
				//SetKVMA , scansel.scan_ma
				modXrayControl.SetCurrent(maBack);
			}

			//外部トリガ取込みかつテーブル回転が連続回転の時             'V7.0 append by 間々田 2003/09/10
            if ((CTSettings.scaninh.Data.ext_trig == 0) && modCT30K.TableRotOn) modSeqComm.SeqBitWrite("TrgReq", false);

			//テーブル回転が連続回転の時             'V7.0 append by 間々田 2003/09/10
			//条件(GFlg_GainTableRot = 1)「テーブル回転収集を行った時」を追加 by 間々田 2003/10/27
			if (modCT30K.TableRotOn && (modScanCorrect.GFlg_GainTableRot == CheckState.Checked)) 
			{
				//回転軸原点復帰
				rc = modMechaControl.MecaRotateOrigin(true);
			}

            //Rev23.40 変更 成否にかかわらず戻す by長野 2016/06/19
            //Rev20.00 追加 by長野 2015/02/16
            //Rev25.00 条件を移動量に変更 by長野 2016/08/03
            //if (IsOkYAxisMoveTable)
            if(yAxisTalbeDist != 0)
            {
                //if (!modSeqComm.MoveXpos((int)(TargetYAxisPos - (yAxisTalbeDist * 100))))
                if (!modSeqComm.MoveXpos((int)(TargetYAxisPos - (yAxisTalbeDist * modMechaControl.GVal_TableX_SeqMagnify))))//Rev23.10 変更 by長野 2015/09/18
                {
                    //Rev26.00 add by chouno 2017/04/28
                    if (CTSettings.scaninh.Data.xray_remote == 0)
                    {
                        modXrayControl.XrayOff();
                        Application.DoEvents();
                    }

                    string buf = null;
                    buf = "";
                    //エラーの場合：指定されたY軸位置まで試料テーブルを移動させることができませんでした。
                    buf = buf + "* " + StringTable.GetResString(StringTable.IDS_MoveErr, CTSettings.AxisName[1], CTResources.LoadResString(StringTable.IDS_SampleTable)) + "\r";
                    MessageBox.Show(CTResources.LoadResString(9427), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); //Rev21.00 追加 by長野 2015/06/15 
                }
                else
                {
                }
            }

            //Rev23.40 変更 成否にかかわらず戻す by長野 2016/06/19
            //Rev25.00 条件を移動量に変更 by長野 2016/08/03
            //if (IsOkXAxisMoveTable)
            if(xAxisTableDist != 0)
            {
                //if (!modSeqComm.MoveXpos((int)(TargetYAxisPos - (yAxisTalbeDist * 100))))
                if (!modSeqComm.MoveFCD((int)(TargetXAxisPos - (xAxisTableDist * modMechaControl.GVal_FCD_SeqMagnify))))
                {
                    //Rev26.00 add by chouno 2017/04/28
                    if (CTSettings.scaninh.Data.xray_remote == 0)
                    {
                        modXrayControl.XrayOff();
                        Application.DoEvents();
                    }

                    string buf = null;
                    buf = "";
                    //エラーの場合：指定されたY軸位置まで試料テーブルを移動させることができませんでした。
                    buf = buf + "* " + StringTable.GetResString(StringTable.IDS_MoveErr, StringTable.GetResString(StringTable.IDS_Axis, CTResources.LoadResString(StringTable.IDS_FCD)), CTResources.LoadResString(StringTable.IDS_SampleTable)) + "\r";
                    //MessageBox.Show(CTResources.LoadResString(9427), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); //Rev21.00 追加 by長野 2015/06/15 
                    //Rev26.00 修正 by chouno 2017/10/31
                    MessageBox.Show(CTResources.LoadResString(9426), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); //Rev21.00 追加 by長野 2015/06/15 
                }
                else
                {
                }
                //FCD速度を元の速度に戻す //Rev26.00 add by chouno 2017/10/31
                modSeqComm.SeqWordWrite("YSpeed", OrgFcdSpeed.ToString("0"));
            }

			//テーブル下降収集ありの場合  append by 間々田 2003-03-03
			//If IsOkDownTable Then
			//v15.0変更 by 間々田 2009/05/12
			if (IsOkDownTable && (!IsAutoCorrection))
			{
				//テーブルを元の位置に戻す
				//v10.0変更 by 間々田 2005/02/10
				if (modMechaControl.MechaUdIndex(Val_udab_pos, StatusLabel) != 0) 
				{
                    //Rev26.00 add by chouno 2017/04/28
                    if (CTSettings.scaninh.Data.xray_remote == 0)
                    {
                        modXrayControl.XrayOff();
                        Application.DoEvents();
                    }

					//メッセージ表示：テーブルを元の位置に戻せませんでした。
					MessageBox.Show(CTResources.LoadResString(9427), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			//結果判定
			if (Result < 0)
			{
				//中断されたとみなす
				//MsgBox LoadResString(1902), vbCritical    'v10.0削除 by 間々田 2005/02/09
			}
			else if (Result > 0)
			{
                //Rev26.00 add by chouno 2017/04/28
                if (CTSettings.scaninh.Data.xray_remote == 0)
                {
                    modXrayControl.XrayOff();
                    Application.DoEvents();
                }

				//対応するメッセージを表示
				MessageBox.Show(CTResources.LoadResString(Result), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else	//=0
			{
				ScanCorrect.Get_GainCor_Parameter_Ex();
				functionReturnValue = true;		//成功
			}

			//タッチパネル操作を許可（シーケンサ通信が可能な場合）
			if (!IsAutoCorrection) modSeqComm.SeqBitWrite("PanelInhibit", false);		
			
            return functionReturnValue;
		}


		//********************************************************************************
		//機    能  ：  スキャン位置校正画像を配列に読み込む
		//              変数名           [I/O] 型        内容
		//引    数  ：  StatusLabel      [I/ ] Label     進行状況を表示するラベル
		//              IntegNum         [I/ ] Long      積算枚数
		//              Auto             [I/ ] Boolean   モード(True:自動,False:手動)
		//戻 り 値  ：                   [ /O] Boolean   結果(True:正常,False:異常)
		//補    足  ：  なし
		//
		//履    歴  ：  V9.7   04/11/19  (SI4)間々田     新規作成
		//********************************************************************************
		//Public Function GetImageForScanPositionCorrect(StatusLabel As Label, ByVal IntegNum As Long, Optional ByVal Auto As Boolean = True) As Boolean
		public static bool GetImageForScanPositionCorrect(CTStatus StatusLabel, ProgressBar theProgressBar, int IntegNum, bool Auto = true)		//v10.0追変更 by 間々田 2005/01/31 PCフリーズ対策処理 改良版対応
		{
			int rc = 0;
			float iUdab = 0;			//昇降位置(mm)
			int Result = 0;				//戻り値用変数
			bool IsOkMove = false;
			bool IIChangedAtGain = false;		//I.I.移動状態保持用変数（ゲイン校正）       v11.21追加 by 間々田 2006/02/10
			bool IIChangedAtSp = false;			//I.I.移動状態保持用変数（スキャン位置校正） v11.21追加 by 間々田 2006/02/10
			bool IIChangedAtVer = false;		//I.I.移動状態保持用変数（幾何歪校正）       v11.21追加 by 間々田 2006/02/10
			bool IIChangedAtRot = false;		//I.I.移動状態保持用変数（回転中心校正）     v11.21追加 by 間々田 2006/02/10
			bool IIChangedAtDis = false;		//I.I.移動状態保持用変数（寸法校正）         v11.21追加 by 間々田 2006/02/10
			bool IsAutoCorrection = false;


			float OrgFCD = 0;			//元のFCD（mm）
			int OrgFcdSpeed = 0;		//元のFCD速度(×10mm/s)
			float OrgFID = 0;			//元のFID（mm）          'v15.11追加 byやまおか 2010/02/04
			int OrgFidSpeed = 0;		//元のFID速度(×10mm/s)  'v15.11追加 byやまおか 2010/02/04
			float OrgRotPos = 0;		//元の回転位置（度）     'v16.20追加 byやまおか 2010/04/21

			int WaitTime = 0;			//タイムアウト           'v16.20追加 byやまおか 2010/04/21

			//FCDのバックアップ
            //OrgFCD = ScanCorrect.GVal_Fcd;
            //Rev23.10 変更 by長野 2015/10/27
            //if (CTSettings.scaninh.Data.cm_mode == 0)
            //{
            //    OrgFCD = modSeqComm.MySeq.stsLinearFCD / modMechaControl.GVal_FCD_SeqMagnify;
            //}
            //else
            //{
            //    OrgFCD = ScanCorrect.GVal_Fcd;
            //}
            //Rev23.10 元に戻す by長野 2015/11/03
            OrgFCD = ScanCorrect.GVal_Fcd;

			//FCD速度のバックアップ
			OrgFcdSpeed = modSeqComm.MySeq.stsYSpeed;

			//FIDのバックアップ      'v15.11追加 byやまおか 2010/02/04
			OrgFID = ScanCorrect.GVal_Fid;

			//FID速度のバックアップ  'v15.11追加 byやまおか 2010/02/04
			OrgFidSpeed = modSeqComm.MySeq.stsXSpeed;

			//回転位置のバックアップ 'v16.20追加 byやまおか 2010/04/21
            OrgRotPos = Convert.ToSingle(CTSettings.mecainf.Data.rot_pos);

			//戻り値初期化
			bool functionReturnValue = false;

			//変数等初期化
			IsOkMove = false;

			//全自動校正？
            //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            //IsAutoCorrection = modLibrary.IsExistForm(frmAutoCorrection.Instance);
            IsAutoCorrection = modLibrary.IsExistForm("frmAutoCorrection");

			//自動の場合
			if (Auto)
			{
				//scancondparのチェック

				//下記条件のいずれか一つを満たす場合はエラーとし、終了する
                if ((CTSettings.scancondpar.Data.fud_step == 0) ||
                    (CTSettings.scancondpar.Data.rud_step == 0) ||
                    ((CTSettings.scancondpar.Data.rud_start == 0) && (CTSettings.scancondpar.Data.rud_end == 0)))
				{
					//メッセージ表示：昇降移動量または昇降開始、終了位置が異常なためエラー終了します。
					MessageBox.Show(CTResources.LoadResString(9519), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return functionReturnValue;
				}
                //else if (CTSettings.scancondpar.Data.fcd_limit <= 0)		//V9.6 change by 間々田 2004/10/15
                //Rev25.10 change by chouno 2017/09/11
                else if (modSeqComm.GetFCDLimit() <= 0)		//V9.6 change by 間々田 2004/10/15
                {
					//メッセージ表示：限界FCD値が異常なためエラー終了します。
					MessageBox.Show(CTResources.LoadResString(9490), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return functionReturnValue;
				}
			}

			//積算枚数を記憶
			ScanCorrect.IntegNumAtPos = IntegNum;

			//自動校正時以外の処理
			if (!IsAutoCorrection)
			{
				//タッチパネル操作を禁止（シーケンサ通信が可能な場合）
				modSeqComm.SeqBitWrite("PanelInhibit", true);
			}

			//自動の場合
			//int RotFlag = 0;
			if (Auto)
			{
				//「テーブル移動中」と表示           'v11.2追加 by 間々田 2005/12/05
				StatusLabel.Status = StringTable.GC_STS_TABLE_MOVING;

				//昇降位置(mm)を取得
                //Rev23.10 計測CTモード対応 by長野 2015
                if (CTSettings.scaninh.Data.cm_mode == 0)
                {
                    iUdab = CTSettings.mecainf.Data.ud_linear_pos;
                }
                else
                {
                    iUdab = CTSettings.mecainf.Data.udab_pos;
                }

				//''        'テーブルがIIと干渉しないか？ added by 山本 2002-9-24
				//''        If (GVal_FcdLimit > 250) And (GVal_Fid < 650) Then
				//''            If Not MoveFID(Fix(650 * 10)) Then
				//''                'メッセージ表示：試料テーブルと干渉しない位置にI.I.を移動しようとしましたが、失敗しました。
				//'''                MsgBox LoadResString(9512), vbCritical
				//'''                Exit Function
				//''                Result = 9512
				//''                GoTo ExitHandler
				//''            End If
				//''        End If
				//''
				//''        'テーブルがIIと干渉しないか？ added by 山本 2005-11-23
				//''        If GVal_Fid < 400 Then
				//''            If Not MoveFID(Fix(400 * 10)) Then
				//''                'メッセージ表示：試料テーブルと干渉しない位置にI.I.を移動しようとしましたが、失敗しました。
				//'''                MsgBox LoadResString(9512), vbCritical
				//'''                Exit Function
				//''                Result = 9512
				//''                GoTo ExitHandler
				//''            End If
				//''        End If

				//v11.21追加ここから by 間々田 2006/02/10

                ////テーブルがIIと干渉しないための限界FID値をFcdLimitごとに求める
                //float theFidLimit = 0;

                //if (CTSettings.GVal_FcdLimit < 150)
                //{
                //    theFidLimit = 400;
                //}
                //else if (CTSettings.GVal_FcdLimit < 250)
                //{
                //    theFidLimit = 500;
                //}
                //else if (CTSettings.GVal_FcdLimit < 350)
                //{
                //    theFidLimit = 650;
                //}
                //else
                //{
                //    theFidLimit = 820;
                //}
                //Rev25.10 change by chouno 2017/09/11 
                float theFidLimit = 0;

                if (modSeqComm.GetFCDLimit() < 150)
                {
                    theFidLimit = 400;
                }
                else if (modSeqComm.GetFCDLimit() < 250)
                {
                    theFidLimit = 500;
                }
                else if (modSeqComm.GetFCDLimit() < 350)
                {
                    theFidLimit = 650;
                }
                else
                {
                    theFidLimit = 820;
                }

				if (ScanCorrect.GVal_Fid < theFidLimit)
				{
					//自動スキャン位置校正時I.I.移動フラグをセット
					IIMovedAtAutoSpCorrect = true;

					//I.I.を移動する前に現在の校正ごとのI.I.移動あり/なしを記憶しておく
					//   理由：後でI.I.を元の位置に戻しても、いったんI.I.を移動するとシーケンサ側が
					//         すべての校正のI.I.移動を「あり」にしてしまうため。その対策。
					IIChangedAtGain = modSeqComm.MySeq.stsGainIIChange;			//ゲイン校正
					IIChangedAtSp = modSeqComm.MySeq.stsSPIIChange;				//スキャン位置校正
					IIChangedAtVer = modSeqComm.MySeq.stsVerIIChange;			//幾何歪校正
					IIChangedAtRot = modSeqComm.MySeq.stsRotIIChange;			//回転中心校正
					IIChangedAtDis = modSeqComm.MySeq.stsDisIIChange;			//寸法校正

					//FID速度を最速に変更    'v15.11追加 byやまおか 2010/02/04
					modSeqComm.SeqWordWrite("XSpeed", modSeqComm.MySeq.stsXMaxSpeed.ToString("0"));
					Application.DoEvents();
                    
                    //DebugOn追加_hata_2014/09/17　
#if !DebugOn        //'v19.50 v19.41とv18.02の統合　by長野　2013/02/04

                    //2014/11/13hata キャストの修正
                    //if (!modSeqComm.MoveFID((int)(theFidLimit * 10)))
                    //if (!modSeqComm.MoveFID(Convert.ToInt32(Math.Truncate(theFidLimit * 10))))
                    if (!modSeqComm.MoveFID(Convert.ToInt32(Math.Truncate(theFidLimit * modMechaControl.GVal_FDD_SeqMagnify))))//Rev23.10 変更 by長野 2015/09/18
                    {
						Result = 9512;						//試料テーブルと干渉しない位置にI.I.を移動しようとしましたが、失敗しました。
						goto ExitHandler;
					}
#endif

                }

				//v11.21追加ここまで by 間々田 2006/02/10

				//テーブルがＸ線管と干渉しない位置にあるか？
                //if (ScanCorrect.GVal_Fcd < CTSettings.GVal_FcdLimit)
                //Rev25.10 change by chouno 2017/09/11
                if (ScanCorrect.GVal_Fcd < modSeqComm.GetFCDLimit())
				{
					//FCD速度を最速に変更    'v15.0追加 by 間々田 2009/07/25
					modSeqComm.SeqWordWrite("YSpeed", modSeqComm.MySeq.stsYMaxSpeed.ToString("0"));
					Application.DoEvents();

                //DebugOn追加_hata_2014/09/17　
#if !DebugOn    //'v19.50 v19.41とv18.02の統合　by長野　2013/02/04
                    
					//FCD = fcd_limit となるようにテーブルを移動させる   V6.0 append by 間々田 2002/09/18
                    //2014/11/13hata キャストの修正
                    //if (!modSeqComm.MoveFCD((int)(CTSettings.GVal_FcdLimit * 10)))
                    //if (!modSeqComm.MoveFCD(Convert.ToInt32(Math.Truncate(CTSettings.GVal_FcdLimit * 10))))
                    //if (!modSeqComm.MoveFCD(Convert.ToInt32(Math.Truncate(CTSettings.GVal_FcdLimit * modMechaControl.GVal_FCD_SeqMagnify))))//Rev23.10 変更 by長野 2015/09/18
                    //Rev25.10 change by chouno 2017/09/11
                    if (!modSeqComm.MoveFCD(Convert.ToInt32(Math.Truncate(modSeqComm.GetFCDLimit() * modMechaControl.GVal_FCD_SeqMagnify))))//Rev23.10 変更 by長野 2015/09/18
                    {
						//メッセージ表示：X線管と干渉しない位置にテーブルを移動しようとしましたが、失敗しました。
						//                MsgBox LoadResString(9375), vbCritical
						//                Exit Function
						Result = 9375;
						goto ExitHandler;
					}
#endif

                }

				//If Use_FlatPanel Then  'v17.10変更 byやまおか 2010/08/05
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
				//        If (DetType = DetTypeHama) Then
				//            ReDim Def_IMAGE(1)  'ダミーを設定   'v17.10追加 byやまおか 2010/08/19
				//            If Not GetDefImage() Then GoTo ExitHandler 'Exit Function → GoTo ExitHandler
				//            If GainCorFlag = 0 Then     '自動校正時にはファイルから読まない（ゲイン校正時に作成した最新のゲイン校正配列を使う）
				//                If Not GetGainImage() Then GoTo ExitHandler 'Exit Function → GoTo ExitHandler
				//            End If
				//        Else
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)		//v17.00追加 byやまおか 2010/02/08
				{
					ScanCorrect.Def_IMAGE = new ushort[2];		//ダミーを設定   'v17.10追加 byやまおか 2010/08/19
					frmTransImage.Instance.adv = 0;
					//RotFlag = 0;							//回転させる
					if (ScanCorrect.GainCorFlag == 0)		//自動校正時にはファイルから読まない（ゲイン校正時に作成した最新のゲイン校正配列を使う）
					{
						if (!ScanCorrect.GetGainImage()) goto ExitHandler;
					}
				} 
				else
				{
					ScanCorrect.Def_IMAGE = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];					//v15.0変更 -1した by 間々田 2009/06/03
					if (ScanCorrect.GainCorFlag == 0)		//自動校正時にはゲイン校正をした時はRedimしない
					{
						ScanCorrect.GAIN_IMAGE = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];				//v15.0変更 -1した by 間々田 2009/06/03
					}
				}
				IsOkMove = true;
			}


			//エラー時の扱い
			try 
			{
				//配列の領域確保
				ScanCorrect.POSITION_IMAGE = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];		//v15.0変更 -1した by 間々田 2009/06/03

				//自動の場合
				if (Auto)
				{

                    //DebugOn追加_hata_2014/09/17
#if !DebugOn        //v19.50　v19.41とv18.02の統合　by長野2013/11/07 

                    //テーブルを開始位置に上昇させる
                    rc = modMechaControl.MechaUdIndex(CTSettings.scancondpar.Data.rud_start);
#endif

                    //v16.20追加（ここから） byやまおか 2010/04/21
					//テーブル回転角度が０でない場合
					if (!frmMechaControl.Instance.ctchkRotate[0].Value)
					{
						//回転角度を０度にする           'v16.20追加 byやまおか 2010/04/21
						rc = modMechaControl.MecaRotateIndex(0);

						//エラーの場合はここで抜ける
						if (rc != 0) modCT30K.ErrMessage(rc, Icon: MessageBoxIcon.Exclamation);		//v16.20追加 byやまおか 2010/04/21

						WaitTime = 30 * 1000;
						while (!frmMechaControl.Instance.ctchkRotate[0].Value)
						{
							if (modDoubleOblique.TimeOut(ref WaitTime))
							{
								//MsgBox "テーブル回転角度を原点の位置に戻すことができませんでした。", vbExclamation
								//v17.60 ストリングテーブル化 by長野 2011/05/25
								MessageBox.Show(CTResources.LoadResString(20093), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								goto ExitHandler;
							}
						}
					}
					//v16.20追加（ここまで） byやまおか 2010/04/21
				}

				//v16.30条件追加 byやまおか 2010/05/21
                if (CTSettings.scaninh.Data.xray_remote == 0)
				{
					//「Ｘ線アベイラブル待ち」と表示
					StatusLabel.Status = StringTable.GC_STS_X_AVAIL;

					//Ｘ線ＯＮ処理
					Result = modXrayControl.TryXrayOn();
					if (Result != 0) goto ExitHandler;
				}

//カメラなしの環境（デバッグ時）の場合：
#if NoCamera

				//ファイルから画像を読み込む
				Result = ScanCorrect.ImageOpen(ref ScanCorrect.POSITION_IMAGE[0], ScanCorrect.SCPOSI_CORRECT, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);	//v17.10修正 byやまおか 2010/07/28

//カメラありの環境（通常）の場合：
#else

				//自動の場合
				if (Auto)
				{
					//PCフリーズ対策処理 改良版  v10.0変更 by 間々田 2005/01/31 末尾にAddressOf MyCallbackを付加
					//Result = AutoScanpositionEntry(h_size, v_size, IntegNumAtPos, GVal_ScanPosiA(2), GVal_ScanPosiB(2), hDevID1, mrc, GVal_FudStep, GVal_RudStep, GVal_FudStart, GVal_FudEnd, GVal_RudStart, GVal_RudEnd, POSITION_IMAGE(0), dcf(0), Image_bit, detector, GAIN_IMAGE(0), Def_IMAGE(0), FR(0), AddressOf MyCallback)

					//v11.2以下に変更 by 間々田 2005/10/06

					//Result = AutoScanpositionEntry(h_size, v_size, IntegNumAtPos, GVal_ScanPosiA(2), GVal_ScanPosiB(2), hDevID1, mrc, _
					//'                                .fud_step, .rud_step, .fud_start, .fud_end, .rud_start, .rud_end, _
					//'                                POSITION_IMAGE(0), dcf(0), .fimage_bit, .detector, GAIN_IMAGE(0), Def_IMAGE(0), FR(0), AddressOf MyCallback, _
					//'                                .ist, .ied, .jst, .jed, gi(0), git(0), gj(0), gjt(0), Qidjd(0), Qidp1jd(0), Qidjdp1(0), Qidp1jdp1(0), scaninh.full_distortion)
					//変更 by 間々田 2006/01/11
					//            Result = AutoScanpositionEntry(h_size, v_size, IntegNumAtPos, hDevID1, mrc, _
					//'                                            .fud_step, .rud_step, .fud_start, .fud_end, .rud_start, .rud_end, _
					//'                                            POSITION_IMAGE(0), dcf(0), .fimage_bit, .detector, GAIN_IMAGE(0), Def_IMAGE(0), FR(0), AddressOf MyCallback, _
					//'                                            scaninh.full_distortion)


					//「テーブル移動中」と表示
					StatusLabel.Status = StringTable.GC_STS_TABLE_MOVING;
					//v16.20追加 byやまおか 2010/04/21

					MilCaptureCallbackDelegate milCaptureCallback = new MilCaptureCallbackDelegate(MilCaptureCallback);

                    TransImage = CTSettings.transImageControl.GetImage();

					//v15.0変更 by 間々田 2009/01/28
					//Result = AutoScanpositionEntry(frmTransImage.hMil, hDevID1, mrc,   'changed by 山本 2009-09-16
					//                                .fud_step, .rud_step, .fud_start, .fud_end, .rud_start, .rud_end, _
					//'                                .detector, Gain_Image(0), Def_IMAGE(0), AddressOf MilCaptureCallback)
					//'v17.00changed hPke,DestImage(0),TransImage(0)追加 by 山本 2009-09-16
					//Result = AutoScanpositionEntry(frmTransImage.hMil, _
					//'                                hPke, DestImage(0), TransImage(0), _
					//'                                hDevID1, mrc, _
					//'                                .fud_step, .rud_step, .fud_start, .fud_end, .rud_start, .rud_end, _
					//'                                .detector, GAIN_IMAGE(0), Def_IMAGE(0), AddressOf MilCaptureCallback)   'changed by 山本 2009-09-16
					//v17.50変更 DestImage(0)削除 by 間々田 2011/01/14
                    Result = AutoScanpositionEntry((int)Pulsar.hMil,
                                                               (int)Pulsar.hPke, ref TransImage[0],
															   modDeclare.hDevID1, ref modDeclare.mrc,
                                                               CTSettings.scancondpar.Data.fud_step, CTSettings.scancondpar.Data.rud_step,
                                                               ref CTSettings.scancondpar.Data.fud_start, ref CTSettings.scancondpar.Data.fud_end,
                                                               CTSettings.scancondpar.Data.rud_start, CTSettings.scancondpar.Data.rud_end,
                                                               CTSettings.scancondpar.Data.detector, ref ScanCorrect.GAIN_IMAGE[0], ref ScanCorrect.Def_IMAGE[0],
															   milCaptureCallback);				//changed by 山本 2009-09-16

					if ((Result > 0)) goto ExitHandler;

				//手動の場合
				//Else
				}

				//キャプチャ開始
				CaptureStartForScanCorrect(StatusLabel, theProgressBar, ScanCorrect.IntegNumAtPos);

				//ビデオキャプチャから画像を取り込む
				//Result = II_Data_Acquire(h_size, v_size, IntegNumAtPos, AddressOf MyCallback, POSITION_IMAGE(0), 0, dcf(0), scancondpar.detector, FR(0))

				//v15.0変更 by 間々田 2009/01/29
				//int Count = 0;
				ScanCorrect.SUM_IMAGE = new int[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];			//v15.0変更 -1した by 間々田 2009/06/03
				//    'Count = MilCaptureStart(frmTransImage.hMil, AddressOf MilCaptureCallback, SUM_IMAGE(0), IntegNumAtPos)
				//    'Count = MilCaptureStart2(frmTransImage.hMil, AddressOf MilCaptureCallback2, TransImage(0), SUM_IMAGE(0), IntegNumAtPos)
				//    Select Case DetType     'v17.00追加(ここから) byやまおか 2010/02/08
				//        Case DetTypeII, DetTypeHama
				//            Count = MilCaptureStart2(frmTransImage.hMil, AddressOf MilCaptureCallback2, TransImage(0), SUM_IMAGE(0), IntegNumAtPos)
				//        Case DetTypePke
				//            Count = PkeCaptureStart2(hPke, AddressOf MilCaptureCallback2, DestImage(0), TransImage(0), SUM_IMAGE(0), IntegNumAtPos, RotFlag) 'changed by 山本 2009-09-16
				//    End Select              'v17.00追加(ここまで) byやまおか 2010/02/08
				//
				//    If (Count > 0) Then DivImage_short SUM_IMAGE(0), POSITION_IMAGE(0), Count, h_size, v_size
				//    Result = IIf(Count = IntegNumAtPos, 0, 1902)

				MilCaptureCallback2Delegate milCaptureCallback2 = new MilCaptureCallback2Delegate(MilCaptureCallback2);

				//v17.50以下に変更 2011/01/05 by 間々田
				//Result = GetCaptureSumImage(frmTransImage.hMil, hPke, IntegNumAtPos, TransImage(0), SUM_IMAGE(0), AddressOf MilCaptureCallback2)
				//プログレスバーと実際の進行状況が合わない不具合を修正

                TransImage = CTSettings.transImageControl.GetImage();
                Result = GetCaptureSumImage((int)Pulsar.hMil, (int)Pulsar.hPke, 1, ScanCorrect.IntegNumAtPos, ref TransImage[0], ref ScanCorrect.SUM_IMAGE[0], milCaptureCallback2);		//'v17.66 引数追加 by長野 2011/12/28
                //CTSettings.transImageControl.SetTransImage(TransImage);	

				if (Result == 0) ScanCorrect.DivImage_short(ref ScanCorrect.SUM_IMAGE[0], ref ScanCorrect.POSITION_IMAGE[0], ScanCorrect.IntegNumAtPos, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);

				//End If

				//キャプチャ終了
				CaptureEndForScanCorrect();

#endif

			}
			catch
			{
				goto ExitHandler;
			}


ExitHandler:			//v17.50下から移動 by 間々田 2011/02/27　上記AutoScanpositionEntryで停止してもＸ線がオンのままになってしまうことへの対策

			//Ｘ線をＯＦＦ：ただし、自動校正中の場合はX線をOFFしない
			//If Not IsAutoCorrection Then
			//v15.0変更 by 間々田 '２次元幾何歪の場合、全自動校正時、次の校正はオフセット校正なのでここでＸ線オフ 2009/07/25
            if ((!IsAutoCorrection) || (CTSettings.scaninh.Data.full_distortion == 0))
			{
                if (CTSettings.scaninh.Data.xray_remote == 0) modXrayControl.XrayOff();
			}

			//ExitHandler:   'v17.50上に移動 by 間々田 2011/02/27

			//自動の場合
			if (IsOkMove)
			{
				//v16.20追加（ここから） byやまおか 2010/04/21
				//「テーブル移動中」と表示
				StatusLabel.Status = StringTable.GC_STS_TABLE_MOVING;			//v16.20追加 byやまおか 2010/04/21

				//テーブル回転角度を動かした場合
                if (CTSettings.mecainf.Data.rot_pos != Convert.ToInt32(OrgRotPos))
				{
					//元の角度に戻す
					//rc = modMechaControl.MecaRotateIndex(Convert.ToSingle(OrgRotPos) / 100);
                    //Rev26.14 修正 by chouno 2018/09/04
                    rc = modMechaControl.MecaRotateIndex(Convert.ToSingle(OrgRotPos) / modMechaControl.GVal_Rot_SeqMagnify);

					//エラーの場合ここで抜ける
					if (rc != 0) modCT30K.ErrMessage(rc, Icon: MessageBoxIcon.Exclamation);

					//タイムアウト30秒
					WaitTime = 30 * 1000;
                    while (CTSettings.mecainf.Data.rot_pos != Convert.ToInt32(OrgRotPos))
					{
						if (modDoubleOblique.TimeOut(ref WaitTime))
						{
							//メッセージ表示：テーブルを元の位置に戻せませんでした。
							MessageBox.Show(CTResources.LoadResString(9427), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							//v17.20 無限ループで抜けれなくなるため追加 by 長野 10/09/15
							break;
						}
					}
				}
				//v16.20追加（ここまで） byやまおか 2010/04/21

				//テーブルを元の高さに戻す
				rc = modMechaControl.MechaUdIndex(iUdab, StatusLabel);
				if (rc != 0) modCT30K.ErrMessage(rc, Icon: MessageBoxIcon.Exclamation);

                //Rev23.10 昇降移動に成功した場合のみFCDを戻す by長野 2015/11/14
                if (rc == 0)
                {
                    //テーブルを元の FCD の位置に戻す V6.0 append by 間々田 2002/09/18
                    //2014/11/13hata キャストの修正
                    //if (!modSeqComm.MoveFCD((int)(OrgFCD * 10)))
                    //if (!modSeqComm.MoveFCD(Convert.ToInt32(Math.Truncate(OrgFCD * 10))))
                    //Rev26.10 add
                    //if (!modSeqComm.MoveFCD(Convert.ToInt32(Math.Truncate(OrgFCD * modMechaControl.GVal_FCD_SeqMagnify))))//Rev23.10 変更 by長野 2015/09/18
                    if (!modSeqComm.MoveFCD(Convert.ToInt32(Math.Truncate(OrgFCD * modMechaControl.GVal_FCD_SeqMagnify)),(OrgFCD < modSeqComm.GetFCDLimit())))//Rev23.10 変更 by長野 2015/09/18
                    {
                        //メッセージ表示：テーブルを元の FCD の位置に戻せませんでした。
                        MessageBox.Show(CTResources.LoadResString(9426), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

				//FCD速度を元の速度に戻す
				modSeqComm.SeqWordWrite("YSpeed", OrgFcdSpeed.ToString("0"));

				//I.I.を移動させている場合
				//v11.21追加 by 間々田 2006/02/10
				if (IIMovedAtAutoSpCorrect)
				{
					//added by 山本 2002-9-27
					//If Not MoveFID(Fix(GVal_Fid * 10)) Then			
                    //2014/11/13hata キャストの修正
                    //if (!modSeqComm.MoveFID((int)(OrgFID * 10)))		//v15.11修正 byやまおか 2010/02/04
                    //if (!modSeqComm.MoveFID(Convert.ToInt32(Math.Truncate(OrgFID * 10))))		//v15.11修正 byやまおか 2010/02/04
                    if (!modSeqComm.MoveFID(Convert.ToInt32(Math.Truncate(OrgFID * modMechaControl.GVal_FDD_SeqMagnify))))//Rev23.10 変更 by長野 2015/09/18
                    {
						//メッセージ表示：I.I.を元の FID の位置に戻せませんでした。
                        //Rev23.10 リソース9354を変更→検出器を元のFDDの位置に戻せませんでした。 by長野 2015/12/29
                        MessageBox.Show(CTResources.LoadResString(9354), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

						//FID速度を元の速度に戻す                'v15.11変更 byやまおか 2010/02/04
						modSeqComm.SeqWordWrite("XSpeed", OrgFidSpeed.ToString("0"));
					} 
					//v11.21追加ここから by 間々田 2006/02/10
					else
					{
						//I.I.の移動有無を元の状態に戻す
						if (!IIChangedAtGain) modSeqComm.SeqBitWrite("GainIIChangeReset", true);			//ゲイン校正
						if (!IIChangedAtSp) modSeqComm.SeqBitWrite("SPIIChangeReset", true);				//スキャン位置校正
						if (!IIChangedAtVer) modSeqComm.SeqBitWrite("VerIIChangeReset", true);				//幾何歪校正
						if (!IIChangedAtRot) modSeqComm.SeqBitWrite("RotIIChangeReset", true);				//回転中心校正
						if (!IIChangedAtDis) modSeqComm.SeqBitWrite("DisIIChangeReset", true);				//寸法校正
						//v11.21追加ここまで by 間々田 2006/02/10
					}

					//自動スキャン位置校正時I.I.移動フラグをリセット
					IIMovedAtAutoSpCorrect = false;					//v11.21追加 by 間々田 2006/02/10
				}													//v11.21追加 by 間々田 2006/02/10
			}

			//タッチパネル操作を許可（シーケンサ通信が可能な場合）
			if (!IsAutoCorrection) modSeqComm.SeqBitWrite("PanelInhibit", false);

			//結果判定
			if (Result < 0)
			{
				//中断されたとみなす：Interrupt end
				//MsgBox LoadResString(1902), vbCritical 'v10.0削除 by 間々田 2005/02/09
			}
			else if(Result > 0)
			{		
				//対応するメッセージを表示
				MessageBox.Show(CTResources.LoadResString(Result), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else	//=0
			{
				functionReturnValue = true;		//成功
			}

			return functionReturnValue;
		}


		//********************************************************************************
		//機    能  ：  オフセット校正画像を配列に読み込む
		//              変数名           [I/O] 型        内容
		//引    数  ：  StatusLabel      [I/ ] Label     進行状況を表示するラベル
		//戻 り 値  ：                   [ /O] Boolean   結果(True:正常,False:異常)
		//補    足  ：  なし
		//
		//履    歴  ：  V9.7   04/11/19  (SI4)間々田     新規作成
		//********************************************************************************
		//Public Function GetImageForOffsetCorrect(StatusLabel As Label, ByVal IntegNum As Long) As Boolean
		//public static bool GetImageForOffsetCorrect(CTStatus StatusLabel, ProgressBar theProgressBar, int IntegNum)			//v10.0変更 by 間々田 2005/01/31
		public static bool GetImageForOffsetCorrect(CTStatus StatusLabel, ProgressBar theProgressBar, int IntegNum, bool AutoJdgResult = false)//v26.00 change by 長野 2017/01/05
        {
			bool Result = false;
			bool IsAutoCorrection = false;
			//int RotFlag = 0;				//v17.00追加 byやまおか 2010/02/08

			//戻り値用変数初期化
			Result = false;

			//積算枚数を記憶
			ScanCorrect.IntegNumAtOff = IntegNum;

			//全自動校正？
            //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            //IsAutoCorrection = modLibrary.IsExistForm(frmAutoCorrection.Instance);
            IsAutoCorrection = modLibrary.IsExistForm("frmAutoCorrection");

			//全自動校正時以外の処理
			if (!IsAutoCorrection)
			{
				//タッチパネル操作を禁止（シーケンサ通信が可能な場合）
				modSeqComm.SeqBitWrite("PanelInhibit", true);
			}

			//v17.00追加(ここから) byやまおか 2010/02/08
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))
			{
				if (!ScanCorrect.GetGainImage()) goto ExitHandler;

                if (CTSettings.scanParam.FPGainOn == true)
				{
                    CTSettings.scanParam.FPGainOn = false;
					modCT30K.tmp_on = 1;
				}
				else
				{
					modCT30K.tmp_on = 0;
				}
			}				//v17.00追加(ここまで) byやまおか 2010/02/08

			//エラー時の扱い
			try 
			{
				//配列の領域確保
				ScanCorrect.OFFSET_IMAGE = new double[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];			//v15.0変更 -1した by 間々田 2009/06/03

				//Ｘ線ＯＦＦ処理（オフセット校正ではＯＮしない）
				//If scaninh.xray_remote = 0 Then XrayOff    'v17.02削除 byやまおか 2010/06/14

				//X線ONのときはOFFする 'v17.10 条件追加 byやまおか 2010/08/26
				if (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus)
				{
                    if (CTSettings.scaninh.Data.xray_remote == 0)		//v17.02改良(ここから) byやまおか 2010/06/14
					{
						//X線OFF
						modXrayControl.XrayOff();
						//「X線アベイラブル待ち」と表示
						if (StatusLabel != null)
						{
							//StatusLabel.Status = StringTable.GC_STS_X_AVAIL;
                            //'v19.50 変更 by長野 2014/01/27
                            StatusLabel.Status = StringTable.GC_STS_WAITCAPTURE;
                        }
						//'管電圧と管電流が0になるまで待つ(タイムアウト10秒) 'v17.10削除 byやまおか 2010/08/26
						//WaitXrayUnderCurrent 0
						//WaitXrayUnderVoltage 10
					}												//v17.02改良(ここまで) byやまおか 2010/06/14

					//残像が消えるのを待つ   'v17.10追加 byやまおか 2010/08/26
                    if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
					{
                        //modCT30K.PauseForDoEvents(5);		//5秒待つ
                        //modCT30K.PauseForDoEvents((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) ? 10 : 5);// '待つ   'v18.00変更 byやまおか 2011/07/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        //Rev25.03/Rev25.02 change by chouno 2017/02/05
                        modCT30K.PauseForDoEvents((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan || modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman) ? 10 : 5);// '待つ   'v18.00変更 byやまおか 2011/07/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    }
					else
					{
						modCT30K.PauseForDoEvents(2);		//2秒待つ
					}
				}

				//キャプチャ開始
				CaptureStartForScanCorrect(StatusLabel, theProgressBar, ScanCorrect.IntegNumAtOff);

//カメラなしの環境（デバッグ時）の場合：
#if NoCamera

				Result = (ScanCorrect.DoubleImageOpen(ref ScanCorrect.OFFSET_IMAGE[0], ScanCorrect.OFF_CORRECT, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size) == 1);

//カメラありの環境（通常）の場合：
#else

				//ビデオキャプチャから画像を取り込む
				//Result = (Offset_Data_Acquire(h_size, v_size, IntegNumAtOff, AddressOf MyCallback, OFFSET_IMAGE(0), dcf(0), scancondpar.detector, FR(0)) = 0)

				int Count = 0;
				ScanCorrect.SUM_IMAGE = new int[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];			//v15.0変更 -1した by 間々田 2009/06/03
				//Count = MilCaptureStart(frmTransImage.hMil, AddressOf MilCaptureCallback, SUM_IMAGE(0), IntegNumAtOff)
				//Count = MilCaptureStart2(frmTransImage.hMil, AddressOf MilCaptureCallback2, TransImage(0), SUM_IMAGE(0), IntegNumAtOff)
				//If (Count > 0) Then DivImage_double SUM_IMAGE(0), OFFSET_IMAGE(0), Count, h_size, v_size

				MilCaptureCallback2Delegate milCaptureCallback2 = new MilCaptureCallback2Delegate(MilCaptureCallback2);

                switch (CTSettings.detectorParam.DetType)		//v17.00追加(ここから) byやまおか 2010/02/08
				{		
					case DetectorConstants.DetTypeII:
					case DetectorConstants.DetTypeHama:
						//Count = MilCaptureStart2(frmTransImage.hMil, AddressOf MilCaptureCallback2, TransImage(0), SUM_IMAGE(0), IntegNumAtOff)
						//Count = IIf(GetCaptureSumImage(frmTransImage.hMil, hPke, IntegNumAtOff, TransImage(0), SUM_IMAGE(0), AddressOf MilCaptureCallback2) = 0, IntegNumAtOff, 0) 'v17.50変更 2011/01/05 by 間々田
						//プログレスバーと実際の進行状況が合わない不具合を修正

                        TransImage = CTSettings.transImageControl.GetImage();
						Count = (GetCaptureSumImage((int)Pulsar.hMil,(int)Pulsar.hPke, 1, ScanCorrect.IntegNumAtOff, ref TransImage[0], ref ScanCorrect.SUM_IMAGE[0], milCaptureCallback2) == 0 ? ScanCorrect.IntegNumAtOff : 0);		//v17.66 引数追加 2011/12/28 by 長野
                        //CTSettings.transImageControl.SetTransImage(TransImage);                        
                        if ((Count > 0)) ScanCorrect.DivImage_double(ref ScanCorrect.SUM_IMAGE[0], ref ScanCorrect.OFFSET_IMAGE[0], Count, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
	
                        break;

					case DetectorConstants.DetTypePke:
						//Count = PkeCaptureOffset(hPke, AddressOf MilCaptureCallback2, DestImage(0), TransImage(0), OFFSET_IMAGE(0), IntegNumAtOff)  'changed by 山本 2009-10-24
                        TransImage = CTSettings.transImageControl.GetImage();
                        Count = PkeCaptureOffset((int)Pulsar.hPke, milCaptureCallback2, ref TransImage[0], ref ScanCorrect.OFFSET_IMAGE[0], ScanCorrect.IntegNumAtOff);		//v17.50変更 by 間々田 2010/12/28
						//CTSettings.transImageControl.SetTransImage(TransImage);                        
                        break;
				}								//v17.00追加(ここまで) byやまおか 2010/02/08

				Result = (Count == ScanCorrect.IntegNumAtOff);

#endif

                if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)		//v17.00追加(ここから) byやまおか 2010/02/08
				{
					if (modCT30K.tmp_on == 1)
					{
                        CTSettings.scanParam.FPGainOn = true;		//ゲイン補正フラッグを元に戻す 2009-10-05
					}
				}																		//v17.00追加(ここまで) byやまおか 2010/02/08
				
				//キャプチャ終了
				CaptureEndForScanCorrect();
			}
			catch
			{
				goto ExitHandler;
			}

ExitHandler:

			//自動校正時以外の処理
            if (!IsAutoCorrection)
            {
                //タッチパネル操作を許可（シーケンサ通信が可能な場合）
                modSeqComm.SeqBitWrite("PanelInhibit", false);
            }

			//
			if (Result)
			{
				//イメージプロに表示（必ずしもこの処理は必要ない）
				ScanCorrect.Get_Offset_Parameter_Ex();
			}
			else
			{
				//中断された場合メッセージを表示
				//MsgBox "処理が中止されました。", vbCritical
				//v17.60 ストリングテーブル化 by長野 2011/05/25
				MessageBox.Show(CTResources.LoadResString(20172), Application.ProductName);
			}

			//戻り値をセット
			return Result;
		}


		//********************************************************************************
		//機    能  ：  幾何歪校正画像を配列に読み込む
		//              変数名           [I/O] 型        内容
		//引    数  ：  StatusLabel      [I/ ] Label     進行状況を表示するラベル
		//              IntegNum         [I/ ] Long      積算枚数
		//              Shading          [I/ ] Long      テーブル回転（0:なし、1:あり）
		//              ma               [I/ ] Variant   管電流
		//              DownTableDist    [I/ ] Single    テーブル下降収集の距離
		//戻 り 値  ：                   [ /O] Boolean   結果(True:正常,False:異常)
		//補    足  ：  なし
		//
		//履    歴  ：  V9.7   04/11/19  (SI4)間々田     新規作成
		//********************************************************************************
		public static bool GetImageForVerticalCorrect(CTStatus StatusLabel, ProgressBar theProgressBar, int IntegNum, int Shading, float MA, float DownTableDist = 0)
		{
			int Result = 0;				//関数の戻り値
			float Val_udab_pos = 0;		//現在の昇降絶対位置
			bool IsOkDownTable = false;
			bool IsOkSetPhantom = false;
			bool IsAutoCorrection = false;

			//戻り値初期化
			bool functionReturnValue = false;

			//変数等初期化
			IsOkDownTable = false;
			IsOkSetPhantom = false;
			Result = -1;
            float maBack = 0f;	

			//全自動校正？
            //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
			//IsAutoCorrection = modLibrary.IsExistForm(frmAutoCorrection.Instance);
            IsAutoCorrection = modLibrary.IsExistForm("frmAutoCorrection");

			//自動校正以外の場合
			if (!IsAutoCorrection)
			{
				//タッチパネル操作を禁止（シーケンサ通信が可能な場合）
				modSeqComm.SeqBitWrite("PanelInhibit", true);
			}

			//パラメータの記憶
			modScanCorrect.IntegNumAtVer = IntegNum;			//積算枚数
			modScanCorrect.GFlg_Shading_Ver = Shading;			//シェーディング補正の有無

			//１スライスのとき、最大同時スキャン枚数はコモンに０と書込む
			if (!ScanCorrect.MultiSliceMode)
			{
				//最大同時スキャン枚数/同時スキャン枚数：0(1ｽﾗｲｽ),1(3ｽﾗｲｽ),2(5ｽﾗｲｽ)
                CTSettings.scansel.Data.max_multislice = 0;
                CTSettings.scansel.Data.multislice = 0;
				//modScansel.PutScansel(ref modScansel.scansel);
                CTSettings.scansel.Write();

            }

			//テーブル下降収集ありの場合         V7.0 append by 間々田 2003-03-03
			if (DownTableDist != 0)
			{
				//現在の昇降絶対位置の取得
                Val_udab_pos = CTSettings.mecainf.Data.udab_pos;

				//テーブルを下降
				if (modMechaControl.MechaUdIndex(Val_udab_pos + DownTableDist, StatusLabel) != 0)
				{
                    //Rev21.00 追加 by長野 2015/03/08
                    if (CTSettings.t20kinf.Data.ud_type == 0)
                    {
                        //メッセージ表示：テーブル下降に失敗しています。
                        MessageBox.Show(CTResources.LoadResString(9428), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        //メッセージ表示：X線管・検出器昇降に失敗しています。
                        MessageBox.Show(CTResources.LoadResString(22006), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
				}
				else
				{
					IsOkDownTable = true;
				}
			}

			//「ファントム移動中」と表示
			StatusLabel.Status = StringTable.GC_STS_PHANTOM_MOVING;

			//校正用ファントムを出す
			if (ScanCorrect.Set_Vertical_Phantom(1) != 0)
			{
				//メッセージ表示：
				//   幾何歪補正ファントム位置決め動作異常です。
				//   機構部の動作確認後、幾何歪校正を再度行ってください。
				//        MsgBox LoadResString(9485), vbExclamation
				//        Exit Function
				Result = 9485;
				goto ExitHandler;
			}
			else
			{
				IsOkSetPhantom = true;
			}

			//int RotFlag = 0;
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)		//v17.00追加(ここから) byやまおか 2010/02/08
			{
				if (!ScanCorrect.GetGainImage()) goto ExitHandler;
				//ゲイン画像データの平均値を求める
				IICorrect.Cal_Mean_short(ScanCorrect.GAIN_IMAGE, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, ref ScanCorrect.GainMean);
				frmTransImage.Instance.adv = 0;
				//RotFlag = 0;
			}																		//v17.00追加(ここまで) byやまおか 2010/02/08

			//エラー時の扱い
			try 
			{
				//配列の領域確保
				ScanCorrect.VRT_IMAGE = new ushort[CTSettings.detectorParam.v_size * CTSettings.detectorParam.h_size];			//垂直線原画像データの配列 'v15.0変更 -1した by 間々田 2009/06/03

				//    '管電流値（入力値）をＸ線制御器にセット
				//    If scaninh.xray_remote = 0 Then SetKVMA , MA

                if (CTSettings.scaninh.Data.xray_remote == 0)		//v15.10条件追加 byやまおか 2009/10/29
				{
					//現在の管電流のバックアップ
					maBack = (float)frmXrayControl.Instance.ntbSetCurrent.Value;

					//「Ｘ線アベイラブル待ち」と表示
					StatusLabel.Status = StringTable.GC_STS_X_AVAIL;

					//Ｘ線ＯＮ処理
					Result = modXrayControl.TryXrayOn(MA: MA);
					if (Result != 0) goto ExitHandler;
				}

				//キャプチャ開始
				CaptureStartForScanCorrect(StatusLabel, theProgressBar, modScanCorrect.IntegNumAtVer);

//カメラなしの環境（デバッグ時）の場合：
#if NoCamera

				//ファイルから画像を読み込む
				Result = ScanCorrect.ImageOpen(ref ScanCorrect.VRT_IMAGE[0], ScanCorrect.VERT_CORRECT, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);

//カメラありの環境（通常）の場合：
#else

				//ビデオキャプチャから画像を取り込む
				//Result = II_Data_Acquire(h_size, v_size, IntegNumAtVer, AddressOf MyCallback, VRT_IMAGE(0), 0, dcf(0), scancondpar.detector, FR(0))

				//v15.0変更 by 間々田 2009/01/29
				//int Count = 0;
				ScanCorrect.SUM_IMAGE = new int[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
				//v15.0変更 -1した by 間々田 2009/06/03
				//'Count = MilCaptureStart(frmTransImage.hMil, AddressOf MilCaptureCallback, SUM_IMAGE(0), IntegNumAtVer)
				//'Count = MilCaptureStart2(frmTransImage.hMil, AddressOf MilCaptureCallback2, TransImage(0), SUM_IMAGE(0), IntegNumAtVer)
				//Select Case DetType     'v17.00追加(ここから) byやまおか 2010/02/08
				//    Case DetTypeII, DetTypeHama
				//        Count = MilCaptureStart2(frmTransImage.hMil, AddressOf MilCaptureCallback2, TransImage(0), SUM_IMAGE(0), IntegNumAtVer)
				//    Case DetTypePke
				//        Count = PkeCaptureStart2(hPke, AddressOf MilCaptureCallback2, DestImage(0), TransImage(0), SUM_IMAGE(0), IntegNumAtVer, RotFlag)  'changed by 山本 2009-09-16
				//End Select              'v17.00追加(ここまで) byやまおか 2010/02/08
				//
				//If (Count > 0) Then DivImage_short SUM_IMAGE(0), VRT_IMAGE(0), Count, h_size, v_size
				//Result = IIf(Count = IntegNumAtVer, 0, 1902)

				MilCaptureCallback2Delegate milCaptureCallback2 = new MilCaptureCallback2Delegate(MilCaptureCallback2);

				//v17.50以下に変更 2011/01/05 by 間々田
				//Result = GetCaptureSumImage(frmTransImage.hMil, hPke, IntegNumAtVer, TransImage(0), SUM_IMAGE(0), AddressOf MilCaptureCallback2)
				//プログレスバーと実際の進行状況が合わない不具合を修正

                TransImage = CTSettings.transImageControl.GetImage();
                Result = GetCaptureSumImage((int)Pulsar.hMil, (int)Pulsar.hPke, 1, modScanCorrect.IntegNumAtVer, ref TransImage[0], ref ScanCorrect.SUM_IMAGE[0], milCaptureCallback2);		//v17.66 引数追加 2011/12/28 by 長野
                //CTSettings.transImageControl.SetTransImage(TransImage);	

				if (Result == 0) ScanCorrect.DivImage_short(ref ScanCorrect.SUM_IMAGE[0], ref ScanCorrect.VRT_IMAGE[0], modScanCorrect.IntegNumAtVer, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);

#endif

				//キャプチャ終了
				CaptureEndForScanCorrect();

				//Ｘ線をＯＦＦ：ただし、自動校正中の場合はX線をOFFしない
				if (!IsAutoCorrection)
				{
                    if (CTSettings.scaninh.Data.xray_remote == 0) modXrayControl.XrayOff();
				}
			}
			catch
			{
				goto ExitHandler;
			}


ExitHandler:

			//Ｘ線制御器の管電流値を元のコモン値に戻しておく
            if (CTSettings.scaninh.Data.xray_remote == 0)
			{
				//SetKVMA , scansel.scan_ma
				modXrayControl.SetCurrent(maBack);
			}

			//校正用ファントムを入れる
			if (IsOkSetPhantom)
			{
				//「ファントム移動中」と表示
				StatusLabel.Status = StringTable.GC_STS_PHANTOM_MOVING;
				ScanCorrect.Set_Vertical_Phantom(0);
			}

			//タッチパネル操作を許可（シーケンサ通信が可能な場合）
			if (!IsAutoCorrection) modSeqComm.SeqBitWrite("PanelInhibit", false);

			//テーブル下降収集ありの場合         V7.0 append by 間々田 2003-03-03
			//If IsOkDownTable Then
			//v15.0変更 by 間々田 2009/05/12
			if (IsOkDownTable && (!IsAutoCorrection))
			{
				//テーブルを元の位置に戻す
				if (modMechaControl.MechaUdIndex(Val_udab_pos, StatusLabel) != 0)
				{
					//メッセージ表示：テーブルを元の位置に戻せませんでした。
					MessageBox.Show(CTResources.LoadResString(9427), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			//結果判定
			if (Result < 0)
			{
				//中断されたとみなす：Interrupt end
				//MsgBox LoadResString(1902), vbCritical 'v10.0削除 by 間々田 2005/02/09
			}
			else if (Result > 0)
			{
				//対応するメッセージを表示
				MessageBox.Show(CTResources.LoadResString(Result), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else	//=0
			{
				functionReturnValue = true;		//成功
			}

			return functionReturnValue;
		}


		//********************************************************************************
		//機    能  ：  回転中心校正画像を配列に読み込む
		//              変数名           [I/O] 型        内容
		//引    数  ：  iMSL             [I/ ] Long      同時スキャン枚数(0:1ｽﾗｲｽ,1:3ｽﾗｲｽ,2:5ｽﾗｲｽ)
		//戻 り 値  ：                   [ /O] Boolean   結果(True:正常,False:異常)
		//補    足  ：
		//
		//履    歴  ：  V4.0   01/01/29  (SI1)鈴山       新規作成
		//********************************************************************************
		//Public Function GetImageRotationCenterCorrect(ByVal iMSL As Long) As Boolean
		public static bool GetImageRotationCenterCorrect(int iMSL, int theView, int theIntegNum, float theSliceWidth, CTStatus StatusLabel, ProgressBar pb)		//v10.0変更 by 間々田 2005/02/05
		{
            int rc = 0;			//関数の戻り値(汎用)
            bool brc = false;			//関数の戻り値(汎用)
			int tmpL = 0;
            int tmpL_sft =0;    //'(シフト用) 'v18.00追加 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

            bool IsMaintenance = false;
			int Result = 0;						//関数の戻り値
            //int theRotateDirection = 0;			//回転方向   v9.0 added by 間々田 2004/02/16
			int theRotateSelect = 0;			//回転選択   v9.0 added by 間々田 2004/02/16

			//戻り値初期化
			bool functionReturnValue = false;

			//変数等初期化
			Result = -1;

			//メンテナンス実行中か？
            //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
			//IsMaintenance = modLibrary.IsExistForm(frmMaint.Instance);
            IsMaintenance = modLibrary.IsExistForm("frmMaint");

			if (!IsMaintenance)
			{
				//タッチパネル操作を禁止（シーケンサ通信が可能な場合）
				modSeqComm.SeqBitWrite("PanelInhibit", true);
			}

			ScanCorrect.VIEW_N = theView;
            tmpL_sft = theView * (CTSettings.detectorParam.h_size + CTSettings.scancondpar.Data.det_sft_pix); //'(シフト用) 'v18.00追加 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07


			//画像読み込み準備
			tmpL = theView * CTSettings.detectorParam.h_size;
			ScanCorrect.RC_IMAGE0 = new ushort[tmpL];			//回転中心校正画像(No.1読込用) 'v15.0変更 -1した by 間々田 2009/06/03
			ScanCorrect.RC_IMAGE1 = new ushort[tmpL];			//回転中心校正画像(No.2読込用) 'v15.0変更 -1した by 間々田 2009/06/03
			ScanCorrect.RC_IMAGE2 = new ushort[tmpL];			//回転中心校正画像(No.3読込用) 'v15.0変更 -1した by 間々田 2009/06/03
			ScanCorrect.RC_IMAGE3 = new ushort[tmpL];			//回転中心校正画像(No.4読込用) 'v15.0変更 -1した by 間々田 2009/06/03
			ScanCorrect.RC_IMAGE4 = new ushort[tmpL];			//回転中心校正画像(No.5読込用) 'v15.0変更 -1した by 間々田 2009/06/03
			//ScanCorrect.RC_CONE = new ushort[tmpL];				//回転中心校正画像(ｺｰﾝﾋﾞｰﾑCT用) 'v15.0変更 -1した by 間々田 2009/06/03
            //'回転中心校正画像(ｺｰﾝﾋﾞｰﾑCT用)  'v18.00変更 シフト対応 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //if (ScanCorrect.IsShiftScan())
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/08/08
            if (ScanCorrect.IsShiftScan() || ScanCorrect.IsW_Scan())
            {
                ScanCorrect.RC_CONE = new ushort[tmpL_sft];
		    }
             else
            {
                ScanCorrect.RC_CONE = new ushort[tmpL];
		    }
		
        	//'v18.00追加 byやまおか 2011/02/10 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //if (CTSettings.DetShiftOn )
            if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
            {
                //'シフトスキャンの場合
			    //if (modScanCorrect.Flg_RCShiftScan)
                //Rev25.00 Wスキャンを条件に追加 by長野 2016/08/08
                //if (modScanCorrect.Flg_RCShiftScan || ScanCorrect.IsW_Scan())
                //Rev26.10 変更 by chouno 2018/01/13 //WスキャンONかつオフセットにした
                if (modScanCorrect.Flg_RCShiftScan || (ScanCorrect.IsW_Scan() && CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeOffset))
                {    //'検出器がシフト位置にいない場合はシフト位置に移動する
				    //'If (DetShift <> DetShift_forward) Then rc = ShiftDet(DetShift_forward)
				    if (modDetShift.DetShift != modDetShift.DetShiftConstants.DetShift_forward) 
                        brc = modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_forward, modDetShift.SET_GAIN); //'ゲインをセットする 'v18.00変更 byやまおか 2011/07/04
				
                //'シフトスキャンではない場合
			    }
                else
				{
                    //'検出器が基準位置にいない場合は基準位置に移動する
				    //'If (DetShift <> DetShift_origin) Then rc = ShiftDet(DetShift_origin)
                    if (modDetShift.DetShift != modDetShift.DetShiftConstants.DetShift_origin)
                        brc = modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_origin, modDetShift.SET_GAIN); //'ゲインをセットする 'v18.00変更 byやまおか 2011/07/04
			    }
		    }

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//外部トリガ取込みかつテーブル回転が連続回転の時             'V7.0 FPD対応 by 間々田 2003/09/10
			//    If (scaninh.ext_trig = 0) And TableRotOn Then
			//
			//        'Call ExtTrigOn(, theView, theIntegNum)
			//        Call ExtTrigOn(, theView, theIntegNum)      'v10.0変更 by 間々田 2005/02/14
			//
			//        'ExtTrigOnで求めたStartAngleをコモンのscancondpar.scan_start_angleに書き込む    added by 間々田 2004/05/20
			//        'Call putcommon_float("scancondpar", "scan_start_angle", StartAngle)
			//        scancondpar.scan_start_angle = StartAngle    'v11.5変更 by 間々田 2006/04/26
			//        CallPutScancondpar                          'v11.5追加 by 間々田 2006/04/26
			//    End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//Ｘ線検出器がフラットパネルの場合   'V7.0 added by 間々田 03/09/25
            if (CTSettings.detectorParam.Use_FlatPanel)
			{
				//If Not GetDefImage() Then GoTo ExitHandler
				//If Not GetGainImage() Then GoTo ExitHandler
				//v17.00追加(ここから) byやまおか 2010/02/08
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
				//        If DetType = DetTypeHama Then
				//            If Not GetDefImage() Then GoTo ExitHandler
				//        End If
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
				if (!ScanCorrect.GetGainImage()) goto ExitHandler;

                if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
				{
					ScanCorrect.Def_IMAGE = new ushort[tmpL];					//added by 山本　2009-09-19 'v16.0変更
					IICorrect.Cal_Mean_short(ScanCorrect.GAIN_IMAGE, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, ref ScanCorrect.GainMean);	//added by 山本　2009-09-19 'v16.0
					frmTransImage.Instance.adv = 0;							//added by 山本　2009-09-19 'v16.0
				}
				//v17.00追加(ここまで) byやまおか 2010/02/08	
			}
			else		//added by 山本　2003-10-29  IIの場合でも配列はRedimしておかないとエラーするため
			{
                ScanCorrect.Def_IMAGE = new ushort[tmpL];				//added by 山本　2003-10-29 'v15.0変更 -1した by 間々田 2009/06/03
				ScanCorrect.GAIN_IMAGE = new ushort[tmpL];			//added by 山本　2003-10-29 'v15.0変更 -1した by 間々田 2009/06/03
			}

//カメラなしの環境（デバッグ時）の場合：
#if NoCamera

			//デバッグ時はファイルから画像を読み込む
			if (IICorrect.ImageOpen(ref ScanCorrect.RC_IMAGE0[0], ScanCorrect.RC01_CORRECT, CTSettings.detectorParam.h_size, theView) != 0) goto ExitHandler;
			if (IICorrect.ImageOpen(ref ScanCorrect.RC_IMAGE1[0], ScanCorrect.RC02_CORRECT, CTSettings.detectorParam.h_size, theView) != 0) goto ExitHandler;
			if (IICorrect.ImageOpen(ref ScanCorrect.RC_IMAGE2[0], ScanCorrect.RC03_CORRECT, CTSettings.detectorParam.h_size, theView) != 0) goto ExitHandler;
			if (IICorrect.ImageOpen(ref ScanCorrect.RC_IMAGE3[0], ScanCorrect.RC04_CORRECT, CTSettings.detectorParam.h_size, theView) != 0) goto ExitHandler;
			if (IICorrect.ImageOpen(ref ScanCorrect.RC_IMAGE4[0], ScanCorrect.RC05_CORRECT, CTSettings.detectorParam.h_size, theView) != 0) goto ExitHandler;
			if (IICorrect.ImageOpen(ref ScanCorrect.RC_CONE[0], ScanCorrect.RC01_CORRECT, CTSettings.detectorParam.h_size, theView) != 0) goto ExitHandler;
			Result = 0;

//カメラありの環境（通常）の場合：
#else

            //Rev25.00 キャプチャONなら確実にOFFにする by長野 2016/08/08
            frmTransImage.Instance.CaptureOn = false;

            int theRotateDirection = 0;			//回転方向   v9.0 added by 間々田 2004/02/16

			//v16.30条件追加 byやまおか 2010/05/21
			if (CTSettings.scaninh.Data.xray_remote == 0)
			{
				//「Ｘ線アベイラブル待ち」と表示
				if (StatusLabel != null)
				{
                    //StatusLabel.Status = StringTable.GC_STS_X_AVAIL;
                    //'StatusLabel.Status = IIf(XrayType = XrayTypeGeTitan, "データ収集待ち", GC_STS_X_AVAIL) 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    //StatusLabel.Status = ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) ? CTResources.LoadResString(12391) : StringTable.GC_STS_X_AVAIL); // 'v19.50 ストリングテーブル by長野 2013/11/20
                    //Rev25.03/Rev25.02 change by chouno 2017/02/05
                    StatusLabel.Status = ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan || modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman) ? CTResources.LoadResString(12391) : StringTable.GC_STS_X_AVAIL); // 'v19.50 ストリングテーブル by長野 2013/11/20
                }

				//Ｘ線ＯＮ処理
				if (!IsMaintenance)
				{
					Result = modXrayControl.TryXrayOn();
					if (Result != 0) goto ExitHandler;
				}
			}

			//RotationCenter_Data_Acquire の引数の準備

			//コモンの回転選択が可能でかつ回転中心校正画面でＸ線管を選択した場合
			if ((CTSettings.scaninh.Data.rotate_select == 0) && (ScanCorrect.GFlg_MultiTube_R == 1))
			{
				theRotateSelect = 1;
				theRotateDirection = modSeqComm.GetRotateDirection();
			}
			//その他
			else
			{
				theRotateSelect = 0;		//回転方向　デフォルトは 0（=cw）
				theRotateDirection = 0;		//回転選択　デフォルトは 0（=テーブル回転）
			}

			int XRotStopPos = 0;
			if (theRotateDirection == 1)
			{
				XRotStopPos = CTSettings.scancondpar.Data.xrot_start_pos;	//CCW
			}
			else
			{
				XRotStopPos = CTSettings.scancondpar.Data.xrot_end_pos;		//CW
			}

			//キャプチャ開始
			//CaptureStartForScanCorrect StatusLabel, pb, theIntegNum * theView
			CaptureStartForScanCorrect(StatusLabel, pb, theView);			//v17.50変更 by 間々田 2011/01/13 ビュー単位とする

			//ビデオキャプチャから画像を取り込む（dllをコール）
			//Result = RotationCenter_Data_Acquire(h_size, v_size, theIntegNum, _
			//'            RC_IMAGE0(0), RC_IMAGE1(0), RC_IMAGE2(0), RC_IMAGE3(0), RC_IMAGE4(0), _
			//'            hDevID1, mrc, theView, iMSL, CLng(IIf(TableRotOn, 1, 0)), theSliceWidth, CLng(IIf(Use_DataMode3, 0, 1)), RC_CONE(0), _
			//'            Def_IMAGE(0), DCF(IIf(TableRotOn And Use_ExtTrig, 1, 0)), GAIN_IMAGE(0), CLng(IIf(Use_FlatPanel, 1, 0)), FR(0), _
			//'            theRotateSelect, theRotateDirection, XRotStopPos, AddressOf MyCallback) 'v10.0末尾に AddressOf MyCallbackを指定

			//v11.2変更 by 間々田 2005/10/19
			//Result = RotationCenter_Data_Acquire(frmTransImage.hMil, h_size, v_size, theIntegNum,   'changed by 山本　2009-09-16
			//            RC_IMAGE0(0), RC_IMAGE1(0), RC_IMAGE2(0), RC_IMAGE3(0), RC_IMAGE4(0), _
			//'            hDevID1, mrc, theView, iMSL, CLng(IIf(TableRotOn, 1, 0)), theSliceWidth, scaninh.data_mode(2), RC_CONE(0), _
			//'            Def_IMAGE(0), dcf(IIf(TableRotOn And (scaninh.ext_trig = 0), 1, 0)), Gain_Image(0), scancondpar.detector, FR(0), _
			//'            theRotateSelect, theRotateDirection, XRotStopPos, AddressOf MilCaptureCallback)
			//v17.00引数変更 byやまおか 2010/02/08
			//Result = RotationCenter_Data_Acquire(frmTransImage.hMil, hPke, DestImage(0), TransImage(0), h_size, v_size, theIntegNum, _
			//'            RC_IMAGE0(0), RC_IMAGE1(0), RC_IMAGE2(0), RC_IMAGE3(0), RC_IMAGE4(0), _
			//'            hDevID1, mrc, theView, iMSL, CLng(IIf(TableRotOn, 1, 0)), theSliceWidth, scaninh.data_mode(2), RC_CONE(0), _
			//'            Def_IMAGE(0), dcf(IIf(TableRotOn And (scaninh.ext_trig = 0), 1, 0)), GAIN_IMAGE(0), scancondpar.detector, FR(0), _
			//'            theRotateSelect, theRotateDirection, XRotStopPos, AddressOf MilCaptureCallback)
            //v17.50引数変更 by 間々田 2011/01/13 DestImage削除
            
			MilCaptureCallbackDelegate milCaptureCallback = new MilCaptureCallbackDelegate(MilCaptureCallback);

            TransImage = CTSettings.transImageControl.GetImage();
            Result = RotationCenter_Data_Acquire((int)Pulsar.hMil, (int)Pulsar.hPke, ref TransImage[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, theIntegNum,
                                                        ref ScanCorrect.RC_IMAGE0[0], ref ScanCorrect.RC_IMAGE1[0], ref ScanCorrect.RC_IMAGE2[0], ref ScanCorrect.RC_IMAGE3[0], ref ScanCorrect.RC_IMAGE4[0],
                                                        modDeclare.hDevID1, ref modDeclare.mrc, theView, iMSL, Convert.ToInt32(modCT30K.TableRotOn ? 1 : 0), theSliceWidth, CTSettings.scaninh.Data.data_mode[2], ref ScanCorrect.RC_CONE[0],
                                                        ref ScanCorrect.Def_IMAGE[0], ref ScanCorrect.GAIN_IMAGE[0], CTSettings.scancondpar.Data.detector, CTSettings.detectorParam.FR[0],
                                                        theRotateSelect, theRotateDirection, XRotStopPos, milCaptureCallback);
 
            //CTSettings.transImageControl.SetTransImage(TransImage);	

			//Ｘ線ＯＦＦ処理
			if (!IsMaintenance)
			{
				if (CTSettings.scaninh.Data.xray_remote == 0) modXrayControl.XrayOff();
			}

#endif

        ExitHandler:

			//キャプチャ終了
			CaptureEndForScanCorrect();

			//外部トリガ取込みかつテーブル回転が連続回転の時             'V7.0 append by 間々田 2003/09/10
        if ((CTSettings.scaninh.Data.ext_trig == 0) && modCT30K.TableRotOn) modSeqComm.SeqBitWrite("TrgReq", false);

			//テーブル回転が連続回転の時                                 'V7.0 append by 間々田 2003/09/10
			if (modCT30K.TableRotOn)
			{
				//原点復帰：テーブル回転時のみ原点復帰の前に原点センサを抜ける
				rc = modMechaControl.MecaRotateOrigin(theRotateSelect == 0);
			}

			//タッチパネル操作を許可（シーケンサ通信が可能な場合）
			if (!IsMaintenance) modSeqComm.SeqBitWrite("PanelInhibit", false);

			//結果判定
			if (Result < 0)
			{
				//中断されたとみなす：Interrupt end
				//MsgBox LoadResString(1902), vbCritical 'v10.0削除 by 間々田 2005/02/09
			}
			else if (Result > 0)
			{
				//対応するメッセージを表示
				MessageBox.Show(CTResources.LoadResString(Result), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else	//=0
			{
				functionReturnValue = true;		//成功
			}

			return functionReturnValue;
		}


		public static void ScanCorrectStart()
		{
			//実行時はフラグをセット
			modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTScanCorrect;

			//イメージプロ起動
			modCT30K.StartImagePro();
		}


		//
		//   校正dll実行中にコールバックする関数     v10.0追加 by 間々田 2005/01/31
		//
		public static void MilCaptureCallback(int parm)
		{
			if (parm > NullAddress) 
			{
				//透視画像領域にコピー
				//v17.00修正 byやまおか 2010/03/04
                if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypeII || CTSettings.detectorParam.DetType == DetectorConstants.DetTypeHama)
				{
                    //modImgProc.CopyMemory(ref TransImage[0], ref parm, CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size * 2);
                    CopyMemory(TransImage, parm, CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size * sizeof(ushort));                  
                 }

				//透視画像を更新
				//frmTransImage.Update False
                //frmTransImage.Instance.MyUpdate(false, 0);		//変更 by 間々田 2009/08/21

                //Rev20.00 復活 by長野 2015/02/06
                //削除2014/10/07hata_v19.51反映 
                CTSettings.transImageControl.SetTransImage(TransImage);
                frmTransImage.Instance.TransImageCtrl.Update(false, 0);

                //Rev20.04 メカも明示的にupdate by長野 2015/08/20
                frmMechaControl.Instance.UpdateMecha();

				if (myProgressBar != null)
				{
					if (myProgressBar.Value < myProgressBar.Maximum)
					{
						myProgressBar.Value = myProgressBar.Value + 1;
					}
				}
			}

			if ((modCTBusy.CTBusy & modCTBusy.CTScanStart) != 0)
			{  }
			else
			{
                //Application.DoEvents();
                //Rev20.00 16inchで更新されない対策 by長野 2015/02/16
                if (myProgressBar != null)
                {
                    myProgressBar.Refresh();
                }
                //modCT30K.PauseForDoEvents(0.02f);
                modCT30K.PauseForDoEvents(0.1f);
            }
		}


		//
		//   校正dll実行中にコールバックする関数     v10.0追加 by 間々田 2005/01/31
		//
		public static void MilCaptureCallback2(int parm)
		{
			if (parm > NullAddress)
			{
				//'透視画像領域にコピー
				//Call CopyMemory(TransImage(0), ByVal parm, h_size * v_size * 2)
                
				//透視画像を更新
				//frmTransImage.Update False
                //frmTransImage.Instance.MyUpdate(false, 0xCC0020);		//変更 by 間々田 2009/08/21

			    //'PkeFPDの場合は1枚目しか更新されないのでUpdateしない    'v18.00変更 byやまおか 2011/03/26
                if (CTSettings.detectorParam.DetType != DetectorConstants.DetTypePke)
                {
                    CTSettings.transImageControl.SetTransImage(TransImage);
                    frmTransImage.Instance.TransImageCtrl.Update(false, 0);
                }

				if (myProgressBar != null)
				{
					if (myProgressBar.Value < myProgressBar.Maximum)
					{
						myProgressBar.Value = myProgressBar.Value + 1;
					}
				}
			}

			if ((modCTBusy.CTBusy & modCTBusy.CTScanStart) != 0)
			{  }
			else
			{
                //Application.DoEvents();
                //Rev20.00 16inchで更新されない対策 by長野 2015/02/16
                if (myProgressBar != null)
                {
                    myProgressBar.Refresh();
                }
                //modCT30K.PauseForDoEvents(0.02f);
                modCT30K.PauseForDoEvents(0.1f);
            }
		}


		//*******************************************************************************
		//機　　能： 透視画像 ２次元幾何歪校正処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数：
		//           Image           (I/O)           透視画像配列
		//戻 り 値： boolean
		//
		//補　　足： frmScanPositionResult.frmのGetSlopeAndBias()から抜き出し
		//
		//履　　歴： V15.00  09/07/10   YAMAKAGE      新規作成
		//*******************************************************************************
		public static bool DistortionCorrect(ref ushort[] Image)
		{
			//戻り値初期化
			bool functionReturnValue = false;

			int Upper = 0;
			
            Upper = CTSettings.detectorParam.v_size * CTSettings.detectorParam.h_size - 1;
            
            //Rev23.10 幾何歪補正用の配列はシフト分も考慮する by長野 2015/10/18
            //VBでは配列を越えても動いていたが、.NET化後は動かない。
            int sft_val = 0;//追加2014/10/07hata_v19.51反映
            //v18.00シフト対応 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05//追加2014/10/07hata_v19.51反映
            //シフトスキャンでーンビーム用の場合
            //if (ScanCorrect.IsShiftScan())
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/08/08
            if (ScanCorrect.IsShiftScan() || ScanCorrect.IsW_Scan())
            {
                sft_val = CTSettings.scancondpar.Data.det_sft_pix;
            }
            else
            {
                sft_val = 0;
            }

			//２次元幾何歪補正パラメータ計算用
			float[] gi = null;
			float[] git = null;
			float[] gj = null;
			float[] gjt = null;
			int[] Qidjd = null;
			int[] Qidp1jd = null;
			int[] Qidjdp1 = null;
			int[] Qidp1jdp1 = null;
			ushort[] WorkTemp = null;

			gi = new float[Upper + 1];
			git = new float[Upper + 1];
			gj = new float[Upper + 1];
			gjt = new float[Upper + 1];
			Qidjd = new int[Upper + 1];
			Qidp1jd = new int[Upper + 1];
			Qidjdp1 = new int[Upper + 1];
			Qidp1jdp1 = new int[Upper + 1];
			WorkTemp = new ushort[Upper + 1];

            ////２次元幾何歪の場合
            ////２次元幾何歪パラメータ計算を行なう
            //ScanCorrect.make_2d_fullindiv_table(CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size,
            //                                    CTSettings.scancondpar.Data.ist, CTSettings.scancondpar.Data.ied, CTSettings.scancondpar.Data.jst, CTSettings.scancondpar.Data.jed,
            //                                    Convert.ToInt32(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm), ref CTSettings.scancondpar.Data.alk[0], ref CTSettings.scancondpar.Data.blk[0],
            //                                    ref gi[0], ref git[0], ref gj[0], ref gjt[0], ref Qidjd[0], ref Qidp1jd[0], ref Qidjdp1[0], ref Qidp1jdp1[0]);

            ////２次元幾何歪補正を行なう
            //ScanCorrect.cone_fullindiv_crct(CTSettings.detectorParam.h_size, CTSettings.scancondpar.Data.ist, CTSettings.scancondpar.Data.ied, CTSettings.scancondpar.Data.jst, CTSettings.scancondpar.Data.jed,
            //                                    ref gi[0], ref git[0], ref gj[0], ref gjt[0], ref Qidjd[0], ref Qidp1jd[0], ref Qidjdp1[0], ref Qidp1jdp1[0],
            //                                    ref Image[0], ref WorkTemp[0]);

            //Rev23.10 シフトスキャンの場合は、シフト分を引き算する(UI側では合成したデータの補正は行わない) by長野 2015/10/19
            //２次元幾何歪の場合
            //２次元幾何歪パラメータ計算を行なう
            ScanCorrect.make_2d_fullindiv_table(CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size,
                                                CTSettings.scancondpar.Data.ist, CTSettings.scancondpar.Data.ied - sft_val, CTSettings.scancondpar.Data.jst, CTSettings.scancondpar.Data.jed,
                                                Convert.ToInt32(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm), ref CTSettings.scancondpar.Data.alk[0], ref CTSettings.scancondpar.Data.blk[0],
                                                ref gi[0], ref git[0], ref gj[0], ref gjt[0], ref Qidjd[0], ref Qidp1jd[0], ref Qidjdp1[0], ref Qidp1jdp1[0]);

            //２次元幾何歪補正を行なう
            ScanCorrect.cone_fullindiv_crct(CTSettings.detectorParam.h_size, CTSettings.scancondpar.Data.ist, CTSettings.scancondpar.Data.ied - sft_val, CTSettings.scancondpar.Data.jst, CTSettings.scancondpar.Data.jed,
                                                ref gi[0], ref git[0], ref gj[0], ref gjt[0], ref Qidjd[0], ref Qidp1jd[0], ref Qidjdp1[0], ref Qidp1jdp1[0],
                                                ref Image[0], ref WorkTemp[0]);


            WorkTemp.CopyTo(Image, 0);

			functionReturnValue = true;
			return functionReturnValue;
		}
 	}
}
