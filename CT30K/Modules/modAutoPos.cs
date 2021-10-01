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
	internal static class modAutoPos
	{
		//********************************************************************************
		//  構造体宣言
		//********************************************************************************

		//SIZE構造体
		[StructLayout(LayoutKind.Sequential)]
		public struct SIZE
		{
			public int CX;
			public int CY;
		}

        //追加2014/09/20(検S1)hata
        //posPoint構造体
        [StructLayout(LayoutKind.Sequential)]
        public struct PosPoint 
        {
            public short X;
            public short Y;
        }

        //矩形ROI構造体
		[StructLayout(LayoutKind.Sequential)]
		public struct RECTROI
		{
            //public modLibrary.Points pt;	//左上座標          //変更2014/09/20(検S1)hata
            public PosPoint pt;	            //左上座標
			public SIZE sz;					//サイズ
		}

		//試料テーブル位置構造体
		[StructLayout(LayoutKind.Sequential)]
		public struct SampleTable
		{
			public float FCD;	//FCD方向
			public float lr;	//光軸直交方向
			public float ud;	//昇降方向
		}

		//微調テーブル位置構造体
		[StructLayout(LayoutKind.Sequential)]
		public struct FineTable
		{			
			public float x;		//x軸 (光軸方向)
			public float y;		//y軸 (光軸直交方向)
		}

		//検出器情報構造体
		[StructLayout(LayoutKind.Sequential)]
		public struct Detector_Info
		{				
			public float FDD;		//検出器位置				
			public float pitchH;	//検出器横方向ピッチ				
			public float pitchV;	//検出器縦方向ピッチ				
			public short DetType;	//X線検出器の種類    v16.20/v17.00 追加 by 山影 10-03-04
		}

		//キャプチャ進行用プログレスバー
		private static ProgressBar myProgressBar;

		//バックアップ用変数
		private static bool BackupLiveCamera;
		private static int BackupSeqcommInterval;
		private static int BackupXSpeed;

		//Public IsLRInverse As Boolean      '透視画像左右反転フラグ v16.20/v17.00 追加 by 山影 10-03-04 'v17.50 modGlobalへ移動 byやまおか 2010/02/02	
		public static bool ParacllaticAngleOn;		//視差角使用 v16.2(CT30K統合) 追加 by 山影 10-04-01

		//
		// MilCaptureCallbackForAutoPosTrans コールバック関数のデリゲート
		//
        public delegate void MilCaptureCallbackForAutoPosTransDelegate(int parm);
 
        //********************************************************************************
        //  外部関数宣言
        //********************************************************************************

        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetCaptureSumImage(int hMil, int hPke, int ViewNum, int IntegNum, ref ushort TransImage, ref int SumImage, MilCaptureCallbackForAutoPosTransDelegate hCT30K);		//v17.66 引数追加 by 長野 2011/12/28



        //微調テーブル＆昇降位置の最適位置取得
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int AutoFinetableSet_Trans(ref short beforeImage, 
        //                                                ref short afterImage, 
        //                                                ref SIZE imageSize, 
        //                                                ref RECTROI roiRect, 
        //                                                ref FineTable curFTpos, 
        //                                                ref SampleTable curTablePos, 
        //                                                ref Detector_Info detectorInfo, 
        //                                                double rotationAngle, 
        //                                                float rcPos, 
        //                                                float scanpos,
        //                                                int movePixel, 
        //                                                float moveRefPix, 
        //                                                ref FineTable optFTpos, 
        //                                                ref float optUD, 
        //                                                ref float fod);		
        public static extern int AutoFinetableSet_Trans(ref ushort beforeImage,
                                                        ref ushort afterImage,
                                                        ref SIZE imageSize,
                                                        ref RECTROI roiRect,
                                                        ref FineTable curFTpos,
                                                        ref SampleTable curTablePos,
                                                        ref Detector_Info detectorInfo,
                                                        double rotationAngle,
                                                        float rcPos,
                                                        float scanpos,
                                                        int movePixel,
                                                        float moveRefPix,
                                                        ref FineTable optFTpos,
                                                        ref float optUD,
                                                        ref float fod);
	
        //微調＆昇降の位置修正とFCD＆光軸直交方向の最適位置取得
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int AutoTableSet_Trans(ref SIZE imageSize, 
                                                    ref RECTROI roiRect, 
                                                    ref SampleTable curTablePos, 
                                                    ref Detector_Info detectorInfo, 
                                                    ref FineTable curFTpos, 
                                                    ref FineTable optFTpos, 
                                                    float optUD, 
                                                    float fod, 
                                                    float scanpos, 
                                                    ref SampleTable optTablePos1st,
                                                    ref SampleTable optTablePos2nd);
        

        //*******************************************************************************
        //機　　能： 自動スキャン位置移動前処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： IntegNum        (I)             積算枚数
        //           dPixel          (I)             微少移動量(pixel)
        //           beforeImage     (O)             移動前透視画像
        //           afterImage      (O)             移動後透視画像
        //戻 り 値： なし
        //
        //補　　足：     テーブルY方向微少移動処理
        //               キャプチャ移動前～移動～キャプチャ移動後～元の位置へ
        //
        //履　　歴： V15.00  09/07/17   YAMAKAGE      新規作成
        //*******************************************************************************
		public static bool AutoPos_PreProc(int IntegNum, int dPixel,
										   ref ushort[] beforeImage, ref ushort[] afterImage, 
										   ProgressBar pgbBefore, ProgressBar pgbAfter, ref float dRefPix)
		//Public Function AutoPos_PreProc(ByVal IntegNum As Integer, ByRef dPixel As Integer, _
		                                //ByRef beforeImage() As Integer, ByRef afterImage() As Integer, _
		                                //pgbBefore As ProgressBar, pgbAfter As ProgressBar, ByRef dRefPix As Single, ByVal fai As Double) As Boolean
		{
			float Dpi = 0;          //検出器横方向ピッチ
			int dYmm = 0;			//微少移動量
			int PosY = 0;			//現在位置
			//int Result = 0;

            bool functionReturnValue = false;

            var _mechpara = CTSettings.mechapara.Data;
            var _scanselpara = CTSettings.scansel.Data;
            var _scancondpara = CTSettings.scancondpar.Data;
            var _mechinf = CTSettings.mecainf.Data;

			//X線OFFの時だけONする v16.20/v17.00 by 山影 10-03-04
			//Roi設定後にX線オフしている場合に備えて
			if ((frmXrayControl.Instance.MecaXrayOn != modCT30K.OnOffStatusConstants.OnStatus) &&
                (CTSettings.detectorParam.DetType != DetectorConstants.DetTypePke))
			{
				//ライブON
				frmTransImage.Instance.CaptureOn = true;
			}

			//テーブルX軸速度のバックアップ
			BackupXSpeed = modSeqComm.MySeq.stsXSpeed;

			//テーブルX軸速度を一時的に変更する
            if (!modSeqComm.SeqWordWrite("XSpeed", (CTSettings.mechapara.Data.tbl_y_speed[(int)frmMechaControl.SpeedConstants.SpeedSlow] * 10).ToString("0"), false)) goto ErrorHandler;
			Application.DoEvents();

			//メカ操作禁止
			MechaControlLock();

			//移動方向・距離を計算する
            Dpi = 10 / _scancondpara.b[1];
            dYmm = Convert.ToInt32(dPixel * Dpi * (_scanselpara.fcd / _scanselpara.fid) * 100);

			//    Dim dYmmNew As Double
			//    dYmmNew = scansel.FCD * Tan(fai) * (dYmm / Abs(dYmm)) * 100
			//    If ParacllaticAngleOn Then
			//        dYmm = CLng(dYmmNew)
			//        dPixel = CInt(dYmmNew / scansel.FCD / Dpi * scansel.Fid / 100)
			//    End If

			//If IsLRInverse Then dYmm = -dYmm  '左右反転
            if (CTSettings.detectorParam.Use_FlatPanel) dYmm = -dYmm;      //左右反転   'v17.50変更 byやまおか 2011/02/27

            //2014/11/13hata キャストの修正
            //PosY = (int)(_mechinf.table_x_pos * 100);
            PosY = Convert.ToInt32(_mechinf.table_x_pos * 100);

			//テーブル移動量を反映させたずらし量(pixel)を計算
            //2014/11/13hata キャストの修正
            //dRefPix = dYmm / Dpi / _scanselpara.fcd * _scanselpara.fid / 100;
            dRefPix = dYmm / Dpi / _scanselpara.fcd * _scanselpara.fid / 100F;
			//If IsLRInverse Then dRefPix = -dRefPix  '左右反転
            if (CTSettings.detectorParam.Use_FlatPanel) dRefPix = -dRefPix;    //左右反転 'v17.50変更 byやまおか 2011/02/27

			//メカのあそびに寄る誤差を回避するため
			int d = 0;
            d = (dYmm > 0 ? -100 : 100);        //ずらす方向と逆方向に1mm

            if (!modSeqComm.MoveXpos(PosY + d)) goto ErrorHandler;
			Application.DoEvents();

            if (!modSeqComm.MoveXpos(PosY)) goto ErrorHandler;
			Application.DoEvents();
            
			//キャプチャ(移動前)
            if (!AutoPos_Capture(IntegNum, ref beforeImage, pgbBefore)) goto ErrorHandler;
			Application.DoEvents();

			//微少移動
            if (!modSeqComm.MoveXpos(PosY + dYmm)) goto ErrorHandler;
			Application.DoEvents();

			//キャプチャ(移動後)
            if (!AutoPos_Capture(IntegNum, ref afterImage, pgbAfter)) goto ErrorHandler;
			Application.DoEvents();

			//元の位置へ
            if (!modSeqComm.MoveXpos(PosY)) goto ErrorHandler;
			Application.DoEvents();

			//テーブルX軸速度を元に戻す
            if (!modSeqComm.SeqWordWrite("XSpeed", Convert.ToString(BackupXSpeed), false)) goto ErrorHandler;
			Application.DoEvents();

			functionReturnValue = true;

ErrorHandler:
			//メカ操作禁止解除
			MechaControlUnlock();

			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： 透視画像キャプチャ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： IntegNum        (I)             積算枚数
        //           Image           (O)             透視画像
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V15.00  09/07/17   YAMAKAGE      新規作成
        //*******************************************************************************
		private static bool AutoPos_Capture(int IntegNum, ref ushort[] Image, ProgressBar theProgressBar)
		{
			bool functionReturnValue = false;

			int rc = 0;

			int IntegCount = 0;			//積算回数チェック
            ushort[] WorkTemp = null;

            ScanCorrect.SUM_IMAGE = new int[modScanCorrectNew.TransImage.GetUpperBound(0) + 1];
            WorkTemp = new ushort[modScanCorrectNew.TransImage.GetUpperBound(0) + 1];
            ScanCorrect.OFFSET_IMAGE = new double[modScanCorrectNew.TransImage.GetUpperBound(0) + 1];

            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)		//v16.20/v17.00追加(ここから)　山本 2009-09-19
			{
				if (!ScanCorrect.GetGainImage()) goto ExitHandler;

				//ゲイン画像データの平均値を求める
				ScanCorrect.Cal_Mean_short(ref ScanCorrect.GAIN_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, ref ScanCorrect.GainMean);
				frmTransImage.Instance.adv = 0;
				//int RotFlag;
				//RotFlag = 0;
			}																		//v16.20/v17.00追加(ここまで)　山本 2009-09-19

			functionReturnValue = false;

			//キャプチャ開始
			CaptureStartForAutoPosTrans(ref theProgressBar, IntegNum);

			//Select Case DetType     'v16.20/v17.00追加(ここから) byやまおか 2010/02/18
			//
			//Case DetTypeII, DetTypeHama
			//    IntegCount = MilCaptureStart2(frmTransImage.hMil, _
			//'                            AddressOf MilCaptureCallbackForAutoPosTrans, _
			//'                            TransImage(0), _
			//'                            SUM_IMAGE(0), _
			//'                            IntegNum _
			//'                            )
			//    Case DetTypePke
			//        IntegCount = PkeCaptureStart2(hPke, _
			//'                               AddressOf MilCaptureCallbackForAutoPosTrans, _
			//'                               DestImage(0), TransImage(0), _
			//'                               SUM_IMAGE(0), _
			//'                               IntegNum, RotFlag _
			//'                               )
			//
			//End Select              'v16.20/v17.00追加(ここまで) byやまおか 2010/02/18

			//v17.50以下に変更 2011/01/05 by 間々田
			//IntegCount = IIf(GetCaptureSumImage(frmTransImage.hMil, hPke, IntegNum, TransImage(0), SUM_IMAGE(0), AddressOf MilCaptureCallbackForAutoPosTrans) = 0, IntegNum, 0)
			//ダイアログバーと実際の進行状況が合わない不具合を修正
			MilCaptureCallbackForAutoPosTransDelegate milCaptureCallbackForAutoPosTrans = new MilCaptureCallbackForAutoPosTransDelegate(MilCaptureCallbackForAutoPosTrans);

            modScanCorrectNew.TransImage = CTSettings.transImageControl.GetImage();
            IntegCount = (GetCaptureSumImage((int)Pulsar.hMil, (int)Pulsar.hPke, 1, IntegNum, ref modScanCorrectNew.TransImage[0], ref ScanCorrect.SUM_IMAGE[0], milCaptureCallbackForAutoPosTrans) == 0 ? IntegNum : 0);     //v17.66 引数追加 by長野 2011/12/27
            CTSettings.transImageControl.SetTransImage(modScanCorrectNew.TransImage);	
            
            if (IntegCount > 0)
            {
                ScanCorrect.DivImage_short(ref ScanCorrect.SUM_IMAGE[0], ref Image[0], IntegCount, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
            }

			//キャプチャ終了
			CaptureEndForAutoPosTrans();

			//I.I.の時だけ v16.20/v17.00 by 山影 10-03-04
            if (!CTSettings.detectorParam.Use_FlatPanel)
            {
                //オフセット補正
                //ファイルからオフセットデータを読み込み
                rc = (ScanCorrect.DoubleImageOpen(ref ScanCorrect.OFFSET_IMAGE[0], ScanCorrect.OFF_CORRECT, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size) == 1 ? -1 : 0);

                int i;
                for (i = 1; i <= modScanCorrectNew.TransImage.GetUpperBound(0); i++)
                {
                    WorkTemp[i] = (ushort)(Image[i] - ScanCorrect.OFFSET_IMAGE[i]);
                }
                WorkTemp.CopyTo(Image, 0);

                //２次元幾何歪の場合
                if (CTSettings.scaninh.Data.full_distortion == 0)
                {
                    //２次元幾何歪補正
                    modScanCorrectNew.DistortionCorrect(ref Image);
                }
            }

			functionReturnValue = true;
            ExitHandler:

			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： 自動スキャン位置移動に必要な箇所のコモン更新
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //補　　足： なし
        //履　　歴： V15.00  09/07/17   YAMAKAGE      新規作成
        //*******************************************************************************
		public static void UpdateForAutoPos()
		{
			//v15.0追加 by 間々田 2009/07/16
			//modMecainf.GetMecainf(ref modMecainf.mecainf);
            CTSettings.mecainf.Load();

            CTSettings.mecainf.Data.iifield = modSeqComm.GetIINo();		//I.I.視野
            CTSettings.mecainf.Data.table_x_pos = Convert.ToSingle(frmMechaControl.Instance.ntbTableXPos.Value);		//試料テーブル（光軸と垂直方向)座標[mm] （従来のＸ軸座標）も書き込む
			
            //modMecainf.PutMecainf(ref modMecainf.mecainf);
            CTSettings.mecainf.Put(CTSettings.mecainf.Data);

			//FID/FCDをコモンに書き込む  'v15.0追加 by 間々田 2009/06/16
            CTSettings.scansel.Data.fid = frmMechaControl.Instance.FIDWithOffset;			//FID
            CTSettings.scansel.Data.fcd = frmMechaControl.Instance.FCDWithOffset;			//FCD

			//modScansel.PutScansel(ref modScansel.scansel);
            CTSettings.scansel.Write();

		}

        //modScanCorrectNewから引用
		private static void CaptureStartForAutoPosTrans(ref ProgressBar pb, int CaptureCount)
		{
			//シーケンサの監視インターバルをバックアップ
			//BackupSeqcommInterval = frmMechaControl.Instance.tmrSeqComm.Interval;
            //v19.50 タイマーの統合 by長野 2013/12/17
            BackupSeqcommInterval = frmMechaControl.Instance.tmrMecainfSeqComm.Interval;


			//ライブ画像処理を停止
			BackupLiveCamera = frmTransImage.Instance.CaptureOn;
			frmTransImage.Instance.CaptureOn = false;

			//DLLによるキャプチャ処理を優先させるため、CT30K側のタイマ処理をオフにする
            //v19.50 タイマーの統合 by長野 2013/12/17
            frmCTMenu.Instance.tmrStatus.Enabled = false;
			//frmMechaControl.Instance.tmrMecainf.Enabled = false;
			//frmMechaControl.Instance.tmrSeqComm.Interval = 2000;		//シーケンサについては監視インターバルを長くする
            modMechaControl.Flg_MechaControlUpdate = false;
            frmMechaControl.Instance.tmrMecainfSeqComm.Interval = 2000;

			//プログレスバーの表示
			myProgressBar = pb;
			//プログレスバーの初期化
            if ((myProgressBar != null))
            {
                myProgressBar.Maximum = CaptureCount;
                myProgressBar.Value = 0;
                myProgressBar.Visible = true;
            }
		}

        //modScanCorrectNewから引用
		private static void CaptureEndForAutoPosTrans()
		{
			//CT30K側のタイマ処理を元に戻す
            //v19.50 タイマーの統合 by長野 2013/12/17_2014/10/07hata_v19.51反映
            frmMechaControl.Instance.tmrMecainfSeqComm.Interval = BackupSeqcommInterval;
			//frmMechaControl.Instance.tmrSeqComm.Interval = BackupSeqcommInterval;
            //frmMechaControl.Instance.tmrMecainf.Enabled = true;
            modMechaControl.Flg_MechaControlUpdate = true;
            frmCTMenu.Instance.tmrStatus.Enabled = true;

            //ライブ画像処理を元に戻す
			//Select Case DetType
			//    Case DetTypeII, DetTypeHama
			//        frmTransImage.CaptureOn = BackupLiveCamera
			//End Select
			frmTransImage.Instance.CaptureOn = BackupLiveCamera;		//v17.02変更 byやまおか 2010/07/16
		}

        //modScanCorrectNewから引用
        //   キャプチャ中にコールバックする関数
		public static void MilCaptureCallbackForAutoPosTrans(int parm)
		{
            if (parm > modScanCorrectNew.NullAddress)
            {
                //frmTransImage.Update False, vbSrcCopy  'v17.02無用 byやまおか 2010/07/23

                if (!(myProgressBar == null))
                {
                    if (myProgressBar.Value < myProgressBar.Maximum)
                    {
                        myProgressBar.Value = myProgressBar.Value + 1;
                    }
                }
            }

            if ((modCTBusy.CTBusy & modCTBusy.CTScanStart) != 0)
            {
            }
            else
            {
                Application.DoEvents();
            }
		}

        //キャプチャ中のメカ操作を禁止する
        //メカ操作禁止
		private static void MechaControlLock()
		{
			MechaControlSelect(true);
		}

        //メカ操作禁止解除
		private static void MechaControlUnlock()
		{
			MechaControlSelect(false);
		}

        //メカ操作禁止/解除切り替え  True:禁止、False:解除
		private static void MechaControlSelect(bool Value)
		{
			//タッチパネル操作
			modSeqComm.SeqBitWrite("PanelInhibit", Value);

			//メカコントロール操作
			frmMechaControl.Instance.Enabled = !Value;
		}

        //Roi座標の変換
        public static void RoiCoordinateTransform(int roiXC, int roiYC, int roiXL, int roiYL, ref RECTROI theRoi)
		{
			int kv = 0;
            //2014/11/13hata キャストの修正
            //kv = (int)(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);
            kv = Convert.ToInt32(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);

			//縮小比率計算
			float zoom_h = 0;
			float zoom_v = 0;

            //変更2014/09/20(検S1)hata
            //zoom_h = frmTransImage.Instance.ctlTransImage.SizeX / frmTransImage.Instance.ctlTransImage.Width;
            //zoom_v = frmTransImage.Instance.ctlTransImage.SizeY / frmTransImage.Instance.ctlTransImage.Height * kv;		//v16.2(CT30K統合) 修正 by 山影 10-04-01
            zoom_h = (float)frmTransImage.Instance.ctlTransImage.SizeX / (float)frmTransImage.Instance.ctlTransImage.Width;
            zoom_v = (float)frmTransImage.Instance.ctlTransImage.SizeY / (float)frmTransImage.Instance.ctlTransImage.Height * kv;		//v16.2(CT30K統合) 修正 by 山影 10-04-01

			//ROIサイズ×比率
            int xc = 0;
            int yc = 0;
            int xl = 0;
            int yl = 0;

            //2014/11/13hata キャストの修正
            //2014/11/13hata キャストの修正
            //xc = (int)((float)roiXC * zoom_h);
            //yc = (int)((float)roiYC * zoom_v);
            //xl = (int)((float)roiXL * zoom_h);
            //yl = (int)((float)roiYL * zoom_v);
            xc = Convert.ToInt32((float)roiXC * zoom_h);
            yc = Convert.ToInt32((float)roiYC * zoom_v);
            xl = Convert.ToInt32((float)roiXL * zoom_h);
            yl = Convert.ToInt32((float)roiYL * zoom_v);

            //short Left = 0;
            //short Right = 0;
            //short Top = 0;
            //short Bottom = 0;
            int Left = 0;
            int Right = 0;
            int Top = 0;
            int Bottom = 0;

            Left = (xc - xl < CTSettings.scancondpar.Data.ist ? CTSettings.scancondpar.Data.ist : xc - xl);
            Right = (xc + xl > CTSettings.scancondpar.Data.ied ? CTSettings.scancondpar.Data.ied : xc + xl);
            Top = (yc - yl < CTSettings.scancondpar.Data.jst * kv ? CTSettings.scancondpar.Data.jst * kv : yc - yl);
            Bottom = (yc + yl > CTSettings.scancondpar.Data.jed * kv ? CTSettings.scancondpar.Data.jed * kv : yc + yl);

            //theRoi.pt.X = Left;               
            theRoi.pt.X = (short)Left;  //変更2014/09/20(検S1)hata
            //2014/11/13hata キャストの修正
            //theRoi.pt.Y = (short)(Top / kv);
            theRoi.pt.Y = Convert.ToInt16(Top / (float)kv);
            
            theRoi.sz.CX = Right - Left + 1;

            //2014/11/13hata キャストの修正
            //theRoi.sz.CY = (Bottom - Top + 1) / kv;
            theRoi.sz.CY = Convert.ToInt32((Bottom - Top + 1) / (float)kv);
        }
	}
}
