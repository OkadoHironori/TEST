using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
//
using CT30K.Common;
using CTAPI;
using TransImage;
using CT30K.Properties;
//using CT30K.Controls;
//using CT30K.Modules;

namespace CT30K
{
    public partial class frmTransImageInfo : Form
    {
        #region メンバ変数
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmTransImageInfo myForm = null;

        /// <summary>
        //ピクチャオブジェクト
        /// </summary>
        //private Image myPicture;
        private Bitmap myBitmap = null;
        #endregion


        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmTransImageInfo()
        {
            InitializeComponent();

            // イベント定義
            InitializeEventHandler();
        }
        #endregion

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmTransImageInfo Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmTransImageInfo();
                }

                return myForm;
            }
        }
        #endregion

        #region イベント定義
        /// <summary>
        /// イベント定義
        /// </summary>
        private void InitializeEventHandler()
        {

            //*************************************************************************************************
            //機　　能： フォームロード時の処理
            //
            //           変数名          [I/O] 型        内容
            //引　　数： なし
            //戻 り 値： なし
            //
            //補　　足： なし
            //
            //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
            //*************************************************************************************************
            this.Load += (sender, e) =>
            {

                //ストリングテーブルからフォームのCaptionにセット
                this.Text = CTResources.LoadResString(StringTable.IDS_ImageInfo);
                //this.Text = CT30K.My.Resources.ResourceManager.GetString("str" + Convert.ToString(StringTable.IDS_ImageInfo));

                //付帯情報
                //v17.02追加 縦サイズを大きくする byやまおか 2010/07/22
                //this.Height = Microsoft.VisualBasic.Compatibility.VB6.Support.TwipsToPixelsY(5500);
                //ピクセルで設定
                this.Height = 367;

                //ピクチャオブジェクトを生成
                //Image myPicture = modImgProc.CreatePicture((int)this.Handle, ClientRectangle.Width, ClientRectangle.Height);
                //myBitmap = new Bitmap(myPicture);
                //myPicture.Dispose();
                if (myBitmap != null)
                {
                    if (myBitmap.Width != ClientRectangle.Width || myBitmap.Height != ClientRectangle.Height)
                    {
                        myBitmap.Dispose();
                    }
                }
                myBitmap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height, PixelFormat.Format8bppIndexed);
                Bitmap mBmppaley = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);

                //パレット取得
                ColorPalette palette = mBmppaley.Palette;

                //パレットをグレースケール256階調用にする
                for (int i = 0; i < 256; i++)
                {
                    palette.Entries[i] = Color.FromArgb(i, i, i);
                }
                myBitmap.Palette = palette;
                mBmppaley.Dispose();

            };        
        }
        #endregion

        //*******************************************************************************
        //機　　能： 画像データ（配列）のセット
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Image()         [I/ ] Byte      オリジナル画像配列
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
		public void GetImage(ref byte[] image)
		{
            if (myBitmap == null) return;

            ////ビットマップ取得
            modImgProc.GetBitmapBits(myBitmap, ref image);
        
        }

        //*******************************************************************************
        //機　　能： 画像データ（配列）のセット
        //
        //           変数名          [I/O] 型        内容
        //引　　数： image()         [I/ ] Byte      オリジナル画像配列
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
		public void SetImage(ref byte[] image)
		{
            if (myBitmap == null) return;
            //int raster = 0;

            //LocBitsを使用してデータを書き換える
            Rectangle rect = new Rectangle(0, 0, myBitmap.Width, myBitmap.Height);
            //ビットマップデータをロックする
            BitmapData bmpData = myBitmap.LockBits(rect, ImageLockMode.ReadWrite, myBitmap.PixelFormat);
            //ビットマップデータに変換する
            Pulsar.DrawBitmap(image, bmpData.Scan0, bmpData.Width, bmpData.Height, bmpData.Stride);
            //Pulsar.DrawBitmap2(image, bmpData.Scan0, bmpData.Width, bmpData.Height, bmpData.Stride, raster);
            //ビットマップデータに対するロックを解除
            myBitmap.UnlockBits(bmpData);

            ////ビットマップhandle取得
            //IntPtr HBit = myBitmap.GetHbitmap();
            
            ////ビットマップ更新
            ////SetBitmapBits myPicture.Handle, IMAGE()
            ////vcの関数に変更 2009-06-11
            //CTAPI.ImgProc.SetByteToBitmap(HBit, image);

			//表示
			//AutoRedraw = true;
            //PaintPicture(myPicture, 0, 0, ClientRectangle.Width, ClientRectangle.Height);
			//AutoRedraw = false;
            this.Refresh();

		}

        

 
 //*******************************************************************************
 //機　　能： 更新
 //
 //           変数名          [I/O] 型        内容
 //引　　数： IntegNum        [I/ ] Long      積算枚数
 //戻 り 値： なし
 //
 //補　　足： なし
 //
 //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
 //*******************************************************************************
        public void MyUpdate(int IntegNum)
        {

            float sc = 0;
            float mdtpitch = 0;
            byte[] theImage = null;         //透視画像付帯情報用配列
            int ret = 0;
            //short Magnify = 0;              //v19.50 追加 by長野 2014/01/28 //追加2014/10/07hata_v19.51反映
            float Magnify = 0;                //v23.50 変更float by長野 2016/09/12(Rev26.40 Mod Start 2018/10/29 M.Oyama Windows10対応)

            //幾何歪校正ステータスが準備完了の場合コモンのチャンネルピッチを使う
            //If frmScanControl.lblStatus(2).Caption = GC_STS_STANDBY_OK Then
            //フラットパネルの場合は常にscancondparを使う    'v17.00変更 byやまおか 2010/03/02
            int no = 0;
            if ((frmScanControl.Instance.lblStatus[2].Text == StringTable.GC_STS_STANDBY_OK) & (!CTSettings.detectorParam.Use_FlatPanel))
            {
                mdtpitch = CTSettings.scancondpar.Data.mdtpitch[2];

                //幾何歪み校正ステータスが準備未完了の場合
            }
            else
            {

                no = modSeqComm.GetIINo();

                switch (no)
                {
                    case 0:
                    case 1:
                    case 2:
                        //II視野ごとの固定値を使う（コモンから取得する）
                        //mdtpitch = CTSettings.scancondpar.Data.detector_pitch[no];
                        //Rev23.10 変更 by長野 2015/11/04
                        mdtpitch = CTSettings.scancondpar.Data.detector_pitch[ScanCorrect.GFlg_MultiTube + no * 3];
                        break;

                    default:
                        mdtpitch = 0;
                        break;
                }
            }

            //追加2014/10/07hata_v19.51反映
            //v19.50 追加 by長野 2014/01/28
            //Rev23.50 修正 by長野 2016/09/12(Rev26.40 Mod Start 2018/10/29 M.Oyama Windows10対応)
            //if (frmTransImage.Instance.ctlTransImage.Width >= 2048)
            //{
            //    Magnify = 2;
            //}
            //else
            //{
            //    Magnify = 1;
            //}

            //Rev23.50 修正 by長野 2016/09/12(Rev26.40 Mod Start 2018/10/29 M.Oyama Windows10対応)
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke || CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
                if (frmTransImage.Instance.ctlTransImage.Width == 2048)
                {
                    Magnify = 2;
                }
                else if (frmTransImage.Instance.ctlTransImage.Width == 1024)
                {
                    Magnify = 1;
                }
                else if (frmTransImage.Instance.ctlTransImage.Width == 512)
                {
                    Magnify = 0.5f;
                }
            }
            else
            {
                if (frmTransImage.Instance.ctlTransImage.Width == CTSettings.detectorParam.h_size)
                {
                    Magnify = 1;
                }
                else if (frmTransImage.Instance.ctlTransImage.Width == (CTSettings.detectorParam.h_size / 2))
                {
                    Magnify = 0.5f;
                }
            }

            //v10.0変更 by 間々田 2005/02/09
            //sc = MdtPitch * GVal_ImgHsize * (Fcd + MyScansel.FcdOffset) / (Fid + MyScansel.FidOffse) / 10
            //sc = mdtpitch * CTSettings.detectorParam.h_size * (frmMechaControl.Instance.FCDFIDRate) / 10;
            //2014/11/07hata キャストの修正
            //v19.50 2048表示だとスケールがはみ出るため、スケールを小さくする by長野 2014/01/28     //変更2014/10/07hata_v19.51反映
            //sc = mdtpitch * CTSettings.detectorParam.h_size * (frmMechaControl.Instance.FCDFIDRate) / (10 * Magnify);   //v10.0変更 by 間々田 2005/02/09
            sc = mdtpitch * CTSettings.detectorParam.h_size * (frmMechaControl.Instance.FCDFIDRate) / (float)(10 * Magnify);   //v10.0変更 by 間々田 2005/02/09

            //付帯情報をイメージプロの画像に書込む
            //イメージプロ起動
            if (!modCT30K.StartImagePro())
            {
                return;
            }
            System.Windows.Forms.Application.DoEvents();

            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            /*
        //イメージプロで画像を開く
        ret = Ipc32v5.IpAppCloseAll();
        //画像ウィンドウをすべて閉じる
        ret = Ipc32v5.IpWsCreate(ClientRectangle.Width, ClientRectangle.Height, 300, Ipc32v5.IMC_GRAY);
        //新規の画像ウィンドウを開く
        ret = Ipc32v5.IpWsFill(0, 3, 0);
        */
            //
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            ret = CallImageProFunction.CallFormatConvertStep1(ClientRectangle.Height, ClientRectangle.Width, 0);
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


            //ImageProテキスト描画オブジェクト                                                       'v15.0追加ここから by 間々田 2009/01/19
            //オフセット位置セット
            //.SetCurrentPoint 10, 10
            CTSettings.IPOBJ.SetCurrentPoint(5, 10);
            //v17.02変更 byやまおか 2010/06/15

            //行ピッチ
            //.LinePitch = 19
            CTSettings.IPOBJ.LinePitch = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke ? 17 : 19);
            //v17.00変更 byやまおか 2010/03/03

            //ヘッダ幅
            //.HeaderWidth = 14
            CTSettings.IPOBJ.HeaderWidth = 16;        //v17.02変更 byやまおか 2010/06/15

            DateTime dt = DateTime.Now;
            //CTSettings.IPOBJ.DrawText(CTResources.LoadResString(9101), dt.ToLongDateString());        //撮影年月日
            //Rev20.01 はみ出さないように短い形式にする by長野 2015/05/19
            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(9101), dt.ToShortDateString());        //撮影年月日
            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(9102), dt.ToLongTimeString());        //撮影時刻
            //CTSettings.IPOBJ.DrawText(Resources.STR_09103, Microsoft.VisualBasic.Compatibility.VB6.Support.Format(CTSettings.mecainf.Data.udab_pos, "##0.000"));

            //Rev20.00 条件式追加 by長野 2015/02/25
            if (CTSettings.t20kinf.Data.ud_type == 0)
            {
                //Rev23.10 計測CT対応 by長野 2015/10/16
                //CTSettings.IPOBJ.DrawText(CTResources.LoadResString(9103), CTSettings.mecainf.Data.udab_pos.ToString("##0.000"));   //ﾃｰﾌﾞﾙ高さ(mm)
                CTSettings.IPOBJ.DrawText(CTResources.LoadResString(9103), frmMechaControl.Instance.Udab_Pos.ToString("##0.000"));   //ﾃｰﾌﾞﾙ高さ(mm)
            }
            else{
                //Rev23.10 計測CT対応 by長野 2015/10/16
                //CTSettings.IPOBJ.DrawText(CTResources.LoadResString(9104), CTSettings.mecainf.Data.udab_pos.ToString("##0.000"));   //X線・検出器高さ(mm)
                CTSettings.IPOBJ.DrawText(CTResources.LoadResString(9104), frmMechaControl.Instance.Udab_Pos.ToString("##0.000"));   //X線・検出器高さ(mm)
            }
            //.DrawText LoadResString(IDS_TubeVoltage) & "(kV)", CStr(frmXrayControl.ntbSetVolt.Value)           '管電圧(kV)
            //.DrawText LoadResString(IDS_TubeCurrent) & "(" & CurrentUni & ")", CStr(frmXrayControl.ntbSetCurrent.Value)  '管電流(μA)

            //管電圧(kV)/管電流(μA)     'v15.10変更 byやまおか 2009/10/29
            //Ｘ線制御なし＆メカ制御ありの場合
            if ((CTSettings.scaninh.Data.xray_remote == 1) & (CTSettings.scaninh.Data.mechacontrol == 0))
            {
                CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_TubeVoltage) + "(kV)", Convert.ToString(frmScanControl.Instance.cwneKV.Value));
                CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_TubeCurrent) + "(μV)", Convert.ToString(frmScanControl.Instance.cwneMA.Value));
            }
            else
            {
                CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_TubeVoltage) + "(kV)", Convert.ToString(frmXrayControl.Instance.ntbSetVolt.Value));
                CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_TubeCurrent) + "(" + modXrayControl.CurrentUni + ")", Convert.ToString(frmXrayControl.Instance.ntbSetCurrent.Value));
            }

            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_IntegNum), Convert.ToString(IntegNum));        //積算枚数

            if (CTSettings.detectorParam.Use_FlatPanel)
            {
                //.DrawText LoadResString(IDS_BinningMode), GetBinningStr(scansel.binning)            'ビニングモード(FPDの場合) 'v17.61削除 byやまおか 2011/07/30
                //ひとまず表示しないことにした。
                //実際にはビニング1x1だが、16インチFPD(2048x2048)の場合など透視表示は1024x1024だったりするとユーザーが混乱するため。
            }
            else
            {

                CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_IIField), modCT30K.GetIIStr(modSeqComm.GetIINo())); //I.I.視野
            }

            //v17.00追加 byやまおか 2010/03/03
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))
            {
                //.DrawText "FPDゲイン(pF)", GetFpdGainStr(scansel.fpd_gain)                          'FPDゲイン(pF)
                CTSettings.IPOBJ.DrawText(CTResources.LoadResString(20015) + "(pF)", modCT30K.GetFpdGainStr(CTSettings.scansel.Data.fpd_gain, CTSettings.t20kinf.Data.pki_fpd_type));
                //ストリングテーブル化 'v17.60 by長野 2011/05/22
                //.DrawText "FPD積分時間(ms)", GetFpdGainStr(scansel.fpd_integ)                       'FPD積分時間(ms)
                //v17.60　ストリングテーブル化、英語の場合は"FPD積分時間"を2行に分けて表記する by長野 2011/06/01
                if (modCT30K.IsEnglish)
                {
                    CTSettings.IPOBJ.DrawText("FPD integration", modCT30K.GetFpdIntegStr(CTSettings.scansel.Data.fpd_integ));
                    CTSettings.IPOBJ.DrawText("time" + "(ms)");

                }
                else
                {
                    CTSettings.IPOBJ.DrawText(CTResources.LoadResString(20016) + "(ms)", modCT30K.GetFpdIntegStr(CTSettings.scansel.Data.fpd_integ));
                }
            }


            //フィルタ
            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_Filter), modCT30K.GetFilterStr(modSeqComm.GetFilterIndex()));

            //システム名
            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_SystemName), modLibrary.RemoveNull(CTSettings.t20kinf.Data.system_name.GetString()));
            //.DrawText LoadResString(IDS_WorkShopName), RemoveNull(workshopinf.workshop)             '事業所名   'v15.0削除 表示しないことになった 2009/07/24 by 間々田
            //.DrawText LoadResString(IDS_Comment), GetFirstItem(RemoveNull(scansel.Comment), vbCrLf) 'コメント

            //コメント
            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_Comment), modLibrary.GetFirstItem(modLibrary.RemoveNull(CTSettings.scansel.Data.comment.GetString()), "\r\n"));

            //ＦＩＤ(mm)
            //CTSettings.IPOBJ.DrawText(modGlobal.gStrFidOrFdd + "(mm)", Microsoft.VisualBasic.Compatibility.VB6.Support.Format(ScanCorrect.GVal_Fid, "##0.0"));
            //CTSettings.IPOBJ.DrawText(CTSettings.gStrFidOrFdd + "(mm)", ScanCorrect.GVal_Fid.ToString("##0.0"));
            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_FID) + "(mm)", ScanCorrect.GVal_Fid.ToString("##0.0")); //Rev26.00 change by chouno 2017/10/31 

            //ＦＣＤ(mm)
            //CTSettings.IPOBJ.DrawText(Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_FCD)) + "(mm)", Microsoft.VisualBasic.Compatibility.VB6.Support.Format(ScanCorrect.GVal_Fcd, "##0.0"));
            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_FCD) + "(mm)", ScanCorrect.GVal_Fcd.ToString("##0.0"));

            //ウィンドウレベル(透視) 'v17.02追加 byやまおか 2010/07/22
            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_WindowLevel), Convert.ToString(frmScanControl.Instance.cwneWL.Value));

            //ウィンドウ幅(透視)     'v17.02追加 byやまおか 2010/07/22
            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_WindowWidth), Convert.ToString(frmScanControl.Instance.cwneWW.Value));

            //スケール(mm)
            //CTSettings.IPOBJ.DrawText(Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_Scale)), Microsoft.VisualBasic.Compatibility.VB6.Support.Format(sc, "##0.0000"));
            CTSettings.IPOBJ.DrawText(CTResources.LoadResString(StringTable.IDS_Scale), sc.ToString("##0.0000"));

            //スケールバー描画
            DrawScaleBar();

            //イメージプロから画像データ取り出し
            //CTSettings.IPOBJ.GetByteImage(ref theImage, , , ClientRectangle.Width, ClientRectangle.Height);
            theImage = new byte[ClientRectangle.Width * ClientRectangle.Height];
            CTSettings.IPOBJ.GetByteImage(ref theImage, 0, 0, ClientRectangle.Width, ClientRectangle.Height);


            //上記画像データをこのフォームに描画
            SetImage(ref theImage);

            this.Tag = "1";

            //スケールバーを更新する
            //    UpdateScaleBar

        }


        //*******************************************************************************
        //機　　能： クリアー
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.02  2010/07/16  やまおか    新規作成
        //*******************************************************************************
        public void Clear()
        {
            byte[] theImage = null;
            //透視画像付帯情報用配列
            theImage = new byte[ClientRectangle.Width * ClientRectangle.Height];
            //空の画像をセット
            SetImage(ref theImage);
        }


        //*******************************************************************************
        //機　　能： スケールバーの描画
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： 
        //*******************************************************************************
        private void DrawScaleBar()
        {
            //追加2014/10/07hata_v19.51反映
            //short Magnify = 0;  //v19.50 追加 by長野 2014/01/28
            float Magnify = 0.5f;  //v22.00 修正 by長野 2015/07/09

            // Mod Start 2018/10/29 M.Oyama Rev26.40
            //if (CTSettings.detectorParam.Use_FlatPanel == true)
            //{
            //    //Rev23.40 by長野 2016/04/05/ //Rev23.12 Magnify修正 by長野 2016/02/23
            //    if (frmTransImage.Instance.ctlTransImage.Width == 2048)
            //    {
            //        Magnify = 2;
            //    }
            //    else if (frmTransImage.Instance.ctlTransImage.Width == 1024)
            //    {
            //        Magnify = 1;
            //    }
            //    else if (frmTransImage.Instance.ctlTransImage.Width == 512)
            //    {
            //        Magnify = 0.5f;
            //    }

            //}
            //else
            //{
            //    if (frmTransImage.Instance.ctlTransImage.Width == 1384)
            //    {
            //        Magnify = 1;
            //    }
            //    else if (frmTransImage.Instance.ctlTransImage.Width == 692)
            //    {
            //        Magnify = 0.5f;
            //    }
            //}
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke || CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
                if (frmTransImage.Instance.ctlTransImage.Width == 2048)
                {
                    Magnify = 2;
                }
                else if (frmTransImage.Instance.ctlTransImage.Width == 1024)
                {
                    Magnify = 1;
                }
                else if (frmTransImage.Instance.ctlTransImage.Width == 512)
                {
                    Magnify = 0.5f;
                }
            }
            else
            {
                if (frmTransImage.Instance.ctlTransImage.Width == CTSettings.detectorParam.h_size)
                {
                    Magnify = 1;
                }
                else if (frmTransImage.Instance.ctlTransImage.Width == (CTSettings.detectorParam.h_size / 2))
                {
                    Magnify = 0.5f;
                }
            }
            // Mod End 2018/10/29

            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            /*
            short x1 = 0;
            short x2 = 0;
            short y1 = 0;
            short y2 = 0;

            //変更2014/10/07hata_v19.51反映
            //x1 = Convert.ToInt16(ClientRectangle.Width / 2 - frmTransImage.Instance.ctlTransImage.Width / 10 / 2);
            //x2 = Convert.ToInt16(ClientRectangle.Width / 2 + frmTransImage.Instance.ctlTransImage.Width / 10 / 2);
			//v19.50 2048表示だとスケールがはみ出るための対策 by長野 2014/01/28　
			x1 = Convert.ToInt16(ClientRectangle.Width / 2 - frmTransImage.Instance.ctlTransImage.Width / (10 * Magnify) / 2);
			x2 = Convert.ToInt16(ClientRectangle.Width / 2 + frmTransImage.Instance.ctlTransImage.Width / (10 * Magnify) / 2);

            //y1 = 280
            y1 = Convert.ToInt16(ClientRectangle.Height - 25);
            //v17.02変更 byやまおか 2010/07/22
            y2 = Convert.ToInt16(y1 + 5);

             rc = Ipc32v5.IpListPts(ref Ipc32v5.Pts[0], StringTable.FormatStr("%1 %2 %3 %4 %5 %6 %7 %8", x1, y1, x1, y2, x2, y2, x2, y1));
            rc = Ipc32v5.IpAnCreateObj(Ipc32v5.GO_OBJ_POLY);
            //線分オブジェクトの作成
            rc = Ipc32v5.IpAnPolyAddPtArray(ref Ipc32v5.Pts[0], 4);
            rc = Ipc32v5.IpAnSet(Ipc32v5.GO_ATTR_PENCOLOR, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White));
            //色の指定
            rc = Ipc32v5.IpAnBurn();
            //書込む
            rc = Ipc32v5.IpAnDeleteObj();
            */
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            int x1 = 0;
            int x2 = 0;
            int y1 = 0;
            int y2 = 0;
            int rc = 0;

            //変更2014/10/07hata_v19.51反映
            //x1 = Convert.ToInt32(ClientRectangle.Width / 2 - frmTransImage.Instance.ctlTransImage.Width / 10 / 2);
            //x2 = Convert.ToInt32(ClientRectangle.Width / 2 + frmTransImage.Instance.ctlTransImage.Width / 10 / 2);
            //v19.50 2048表示だとスケールがはみ出るための対策 by長野 2014/01/28　
            //2014/11/07hata キャストの修正
            //x1 = Convert.ToInt16(ClientRectangle.Width / 2 - frmTransImage.Instance.ctlTransImage.Width / (10 * Magnify) / 2);
            //x2 = Convert.ToInt16(ClientRectangle.Width / 2 + frmTransImage.Instance.ctlTransImage.Width / (10 * Magnify) / 2);
            x1 = Convert.ToInt16(ClientRectangle.Width / 2F - frmTransImage.Instance.ctlTransImage.Width / (float)(10 * Magnify) / 2F);
            x2 = Convert.ToInt16(ClientRectangle.Width / 2F + frmTransImage.Instance.ctlTransImage.Width / (float)(10 * Magnify) / 2F);

            y1 = Convert.ToInt32(ClientRectangle.Height - 25);
            y2 = Convert.ToInt32(y1 + 5);
            rc = CallImageProFunction.CallDrawScaleBar(x1, y1, x2, y2);
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

        }

        //*******************************************************************************
        //機　　能： スケールバーの更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： 
        //*******************************************************************************
        public void UpdateScaleBar()
        {

            if (string.IsNullOrEmpty(Convert.ToString(this.Tag)))
            {
                return;
            }
            int rc = 0;

            //現在描画しているデータをImage-Proの画像に書き込む
            byte[] theImage = null;
            theImage = new byte[ClientRectangle.Width * ClientRectangle.Height];
            GetImage(ref theImage);

            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            /*
            Ipc32v5.ipRect.Left_Renamed = 0;
            Ipc32v5.ipRect.Top = 0;
            Ipc32v5.ipRect.Right_Renamed = ClientRectangle.Width - 1;
            //ipRect.Bottom = 280 - 1
            Ipc32v5.ipRect.Bottom = ClientRectangle.Height - 25 - 1;
            //v17.50修正 スケール行が一部欠ける不具合を修正 by 間々田 2011/02/27

            rc = Ipc32v5.IpWsCreate(ClientRectangle.Width, ClientRectangle.Height, 300, Ipc32v5.IMC_GRAY);
            rc = Ipc32v5.IpWsFill(0, 3, 0);
            //新規の画像ウィンドウを黒く塗りつぶす
            rc = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref Ipc32v5.ipRect, ref theImage[0], Ipc32v5.CPROG);
            */
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            rc = CallImageProFunction.CallUpdateScaleBar(theImage, ClientRectangle.Height, ClientRectangle.Width);
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


            //スケールバーを新たに描画
            DrawScaleBar();

            //イメージプロから画像データ取り出し
            CTSettings.IPOBJ.GetByteImage(ref theImage, 0, 0, ClientRectangle.Width, ClientRectangle.Height);

            //上記画像データをこのフォームに描画
            SetImage(ref theImage);

            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            //画像ウィンドウを閉じる
            //rc = Ipc32v5.IpDocClose();
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            rc = CallImageProFunction.CallIpDocClose();
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

        }


        #region オーバーライドされたメソッド
        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {

            base.OnPaintBackground(e);

            if (myBitmap != null)
            {
                e.Graphics.DrawImage(myBitmap, this.ClientRectangle);
                //e.Graphics.DrawImage(myBitmap, this.ClientRectangle.Width, this.ClientRectangle.Height);
            }
        }
        #endregion

    }
}
