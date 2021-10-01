using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
                this.Text = Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_ImageInfo));
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
                if (myBitmap.Width != ClientRectangle.Width || myBitmap.Height != ClientRectangle.Height)
                {
                    myBitmap.Dispose();
                }
                myBitmap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height, PixelFormat.Format8bppIndexed);


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

            //ビットマップhandle取得
            IntPtr HBit = myBitmap.GetHbitmap();
            //ビットマップ取得
            modImgProc.GetBitmapBits(HBit, ref image);
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

            //ビットマップhandle取得
            IntPtr HBit = myBitmap.GetHbitmap();
            
            //ビットマップ更新
            //SetBitmapBits myPicture.Handle, IMAGE()
			//vcの関数に変更 2009-06-11
            CTAPI.ImgProc.SetByteToBitmap(HBit, image);

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
        public void Update(int IntegNum)
        {

        float sc = 0;
        float mdtpitch = 0;
        byte[] theImage = null;
        //透視画像付帯情報用配列

        //幾何歪校正ステータスが準備完了の場合コモンのチャンネルピッチを使う
        //If frmScanControl.lblStatus(2).Caption = GC_STS_STANDBY_OK Then
        //フラットパネルの場合は常にscancondparを使う    'v17.00変更 byやまおか 2010/03/02
        int no = 0;
        if ((frmScanControl.Instance.lblStatus2.Text == StringTable.GC_STS_STANDBY_OK) & (!CTSettings.detectorParam.Use_FlatPanel))
        {
            mdtpitch = CTSettings.scancondpar.Data.mdtpitch[2];

        //幾何歪み校正ステータスが準備未完了の場合
        } else {


            #region <　工事中！！　>
            /*
            no = modSeqComm.GetIINo();
            */
            #endregion
   
                switch (no) {
                    case 0:
                    case 1:
                    case 2:
                    //II視野ごとの固定値を使う（コモンから取得する）
                    mdtpitch = CTSettings.scancondpar.Data.detector_pitch[no];
                    break;
                    default:
                    mdtpitch = 0;
                    break;
                }
        }

        //v10.0変更 by 間々田 2005/02/09
        //sc = MdtPitch * GVal_ImgHsize * (Fcd + MyScansel.FcdOffset) / (Fid + MyScansel.FidOffse) / 10
        sc = mdtpitch * CTSettings.detectorParam.h_size * (frmMechaControl.Instance.FCDFIDRate) / 10;
        

        //付帯情報をイメージプロの画像に書込む
        //イメージプロ起動
        if (!modCT30K.StartImagePro())
        {
            return;
        }
        System.Windows.Forms.Application.DoEvents();


        #region <　工事中！！　イメージプロx64版を入れる>
        /*
        //イメージプロで画像を開く
        Ipc32v5.ret = Ipc32v5.IpAppCloseAll();
        //画像ウィンドウをすべて閉じる
        Ipc32v5.ret = Ipc32v5.IpWsCreate(ClientRectangle.Width, ClientRectangle.Height, 300, Ipc32v5.IMC_GRAY);
        //新規の画像ウィンドウを開く
        Ipc32v5.ret = Ipc32v5.IpWsFill(0, 3, 0);
        */
        #endregion

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
        CTSettings.IPOBJ.HeaderWidth = 16;
        //v17.02変更 byやまおか 2010/06/15

        DateTime dt = DateTime.Now;
        CTSettings.IPOBJ.DrawText(Resources.STR_09101, dt.ToLongDateString());
        //撮影年月日
        CTSettings.IPOBJ.DrawText(Resources.STR_09102, dt.ToLongTimeString());
        //撮影時刻
        //CTSettings.IPOBJ.DrawText(Resources.STR_09103, Microsoft.VisualBasic.Compatibility.VB6.Support.Format(CTSettings.mecainf.Data.udab_pos, "##0.000"));
        CTSettings.IPOBJ.DrawText(Resources.STR_09103, CTSettings.mecainf.Data.udab_pos.ToString("##0.000"));
             
        //ﾃｰﾌﾞﾙ高さ(mm)
        //.DrawText LoadResString(IDS_TubeVoltage) & "(kV)", CStr(frmXrayControl.ntbSetVolt.Value)           '管電圧(kV)
        //.DrawText LoadResString(IDS_TubeCurrent) & "(" & CurrentUni & ")", CStr(frmXrayControl.ntbSetCurrent.Value)  '管電流(μA)

        //管電圧(kV)/管電流(μA)     'v15.10変更 byやまおか 2009/10/29
        //Ｘ線制御なし＆メカ制御ありの場合

        #region <　工事中！！　>
        /*
        if ((CTSettings.scaninh.Data.xray_remote == 1) & (CTSettings.scaninh.Data.mechacontrol == 0)){
            CTSettings.IPOBJ.DrawText( Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_TubeVoltage)) + "(kV)", Convert.ToString(frmScanControl.Instance.cwneKV.Value));
            CTSettings.IPOBJ.DrawText( Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_TubeCurrent)) + "(μV)", Convert.ToString(frmScanControl.Instance.cwneMA.Value));
        } else {
            CTSettings.IPOBJ.DrawText( Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_TubeVoltage)) + "(kV)", Convert.ToString(frmXrayControl.Instance.ntbSetVolt.Value));
            CTSettings.IPOBJ.DrawText( Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_TubeCurrent)) + "(" + XrayControl.CurrentUni + ")", Convert.ToString(frmXrayControl.Instance.ntbSetCurrent.Value));
        }

        CTSettings.IPOBJ.DrawText(Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_IntegNum)), Convert.ToString(IntegNum));
        //積算枚数

        if (CTSettings.detectorParam.Use_FlatPanel) {
            //.DrawText LoadResString(IDS_BinningMode), GetBinningStr(scansel.binning)            'ビニングモード(FPDの場合) 'v17.61削除 byやまおか 2011/07/30
            //ひとまず表示しないことにした。
            //実際にはビニング1x1だが、16インチFPD(2048x2048)の場合など透視表示は1024x1024だったりするとユーザーが混乱するため。
        } else {
            //I.I.視野
            CTSettings.IPOBJ.DrawText(Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_IIField)), modCT30K.GetIIStr(modSeqComm.GetIINo()));
        }

        //v17.00追加 byやまおか 2010/03/03
        if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)) {
            //.DrawText "FPDゲイン(pF)", GetFpdGainStr(scansel.fpd_gain)                          'FPDゲイン(pF)
            CTSettings.IPOBJ.DrawText(Resources.STR_20015 + "(pF)", modCT30K.GetFpdGainStr(CTSettings.scansel.Data.fpd_gain));
            //ストリングテーブル化 'v17.60 by長野 2011/05/22
            //.DrawText "FPD積分時間(ms)", GetFpdGainStr(scansel.fpd_integ)                       'FPD積分時間(ms)
            //v17.60　ストリングテーブル化、英語の場合は"FPD積分時間"を2行に分けて表記する by長野 2011/06/01
            if (modCT30K.IsEnglish) {
                CTSettings.IPOBJ.DrawText("FPD integration", modCT30K.GetFpdIntegStr(CTSettings.scansel.Data.fpd_integ));
                CTSettings.IPOBJ.DrawText("time" + "(ms)");

            } else {
                CTSettings.IPOBJ.DrawText(Resources.STR_20016 + "(ms)", modCT30K.GetFpdIntegStr(CTSettings.scansel.Data.fpd_integ));
            }
        }

            
        //フィルタ
        CTSettings.IPOBJ.DrawText(Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_Filter)), modCT30K.GetFilterStr(modSeqComm.GetFilterIndex()));
             
        //システム名
        CTSettings.IPOBJ.DrawText(Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_SystemName)), Library.RemoveNull(CTSettings.t20kinf.Data.system_name.ToString()));
        //.DrawText LoadResString(IDS_WorkShopName), RemoveNull(workshopinf.workshop)             '事業所名   'v15.0削除 表示しないことになった 2009/07/24 by 間々田
        //.DrawText LoadResString(IDS_Comment), GetFirstItem(RemoveNull(scansel.Comment), vbCrLf) 'コメント

        //コメント
        CTSettings.IPOBJ.DrawText(Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_Comment)), Library.GetFirstItem(Library.RemoveNull(CTSettings.scansel.Data.comment.ToString()), "\r\n"));
            
        //ＦＩＤ(mm)
        //CTSettings.IPOBJ.DrawText(modGlobal.gStrFidOrFdd + "(mm)", Microsoft.VisualBasic.Compatibility.VB6.Support.Format(ScanCorrect.GVal_Fid, "##0.0"));
        CTSettings.IPOBJ.DrawText(CTSettings.gStrFidOrFdd + "(mm)", ScanCorrect.GVal_Fid.ToString("##0.0"));
             
        //ＦＣＤ(mm)
        //CTSettings.IPOBJ.DrawText(Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_FCD)) + "(mm)", Microsoft.VisualBasic.Compatibility.VB6.Support.Format(ScanCorrect.GVal_Fcd, "##0.0"));
        CTSettings.IPOBJ.DrawText(Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_FCD)) + "(mm)", ScanCorrect.GVal_Fcd.ToString("##0.0"));

        //ウィンドウレベル(透視) 'v17.02追加 byやまおか 2010/07/22
        CTSettings.IPOBJ.DrawText(Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_WindowLevel)), Convert.ToString(frmScanControl.Instance.cwneWL._Value));

        //ウィンドウ幅(透視)     'v17.02追加 byやまおか 2010/07/22
        CTSettings.IPOBJ.DrawText(Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_WindowWidth)), Convert.ToString(frmScanControl.Instance.cwneWW._Value));

        //スケール(mm)
        //CTSettings.IPOBJ.DrawText(Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_Scale)), Microsoft.VisualBasic.Compatibility.VB6.Support.Format(sc, "##0.0000"));
        CTSettings.IPOBJ.DrawText(Resources.ResourceManager.GetString("STR_" + Convert.ToString(IDSNum.IDS_Scale)), sc.ToString("##0.0000"));
        */
        #endregion

        //スケールバー描画
        DrawScaleBar();


        //イメージプロから画像データ取り出し
        //CTSettings.IPOBJ.GetByteImage(ref theImage, , , ClientRectangle.Width, ClientRectangle.Height);
        CTSettings.IPOBJ.GetByteImage(theImage,0 ,0 , ClientRectangle.Width, ClientRectangle.Height);

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

            short x1 = 0;
            short x2 = 0;
            short y1 = 0;
            short y2 = 0;

            #region <　工事中！！　>
            /*
            x1 = ClientRectangle.Width / 2 - frmTransImage.Instance.ctlTransImage.Width / 10 / 2;
            x2 = ClientRectangle.Width / 2 + frmTransImage.Instance.ctlTransImage.Width / 10 / 2;
            */
            #endregion

            //y1 = 280
            y1 = Convert.ToInt16(ClientRectangle.Height - 25);
            //v17.02変更 byやまおか 2010/07/22
            y2 = Convert.ToInt16(y1 + 5);

            #region <　工事中！！　イメージプロx64版を入れる>
            /*
            Ipc32v5.ret = Ipc32v5.IpListPts(ref Ipc32v5.Pts[0], StringTable.FormatStr("%1 %2 %3 %4 %5 %6 %7 %8", x1, y1, x1, y2, x2, y2, x2, y1));
            Ipc32v5.ret = Ipc32v5.IpAnCreateObj(Ipc32v5.GO_OBJ_POLY);
            //線分オブジェクトの作成
            Ipc32v5.ret = Ipc32v5.IpAnPolyAddPtArray(ref Ipc32v5.Pts[0], 4);
            Ipc32v5.ret = Ipc32v5.IpAnSet(Ipc32v5.GO_ATTR_PENCOLOR, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White));
            //色の指定
            Ipc32v5.ret = Ipc32v5.IpAnBurn();
            //書込む
            Ipc32v5.ret = Ipc32v5.IpAnDeleteObj();
            */

            #endregion

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

        if (string.IsNullOrEmpty(this.Tag.ToString()))
        {    
            return;
        }
            //現在描画しているデータをImage-Proの画像に書き込む
            byte[] theImage = null;
            theImage = new byte[ClientRectangle.Width * ClientRectangle.Height];
            GetImage(ref theImage);
            
            #region <　工事中！！　イメージプロx64版を入れる>
            /*
            Ipc32v5.ipRect.Left_Renamed = 0;
            Ipc32v5.ipRect.Top = 0;
            Ipc32v5.ipRect.Right_Renamed = ClientRectangle.Width - 1;
            //ipRect.Bottom = 280 - 1
            Ipc32v5.ipRect.Bottom = ClientRectangle.Height - 25 - 1;
            //v17.50修正 スケール行が一部欠ける不具合を修正 by 間々田 2011/02/27

            Ipc32v5.ret = Ipc32v5.IpWsCreate(ClientRectangle.Width, ClientRectangle.Height, 300, Ipc32v5.IMC_GRAY);
            Ipc32v5.ret = Ipc32v5.IpWsFill(0, 3, 0);
            //新規の画像ウィンドウを黒く塗りつぶす
            Ipc32v5.ret = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref Ipc32v5.ipRect, ref theImage[0], Ipc32v5.CPROG);
            */
            #endregion <　工事中！！　イメージプロx64版を入れる>
   
            //スケールバーを新たに描画
            DrawScaleBar();

            //イメージプロから画像データ取り出し
            CTSettings.IPOBJ.GetByteImage(theImage, 0, 0 , ClientRectangle.Width, ClientRectangle.Height);
           
            //上記画像データをこのフォームに描画
            SetImage(ref theImage);


            #region "<　工事中！！　イメージプロx64版を入れる>
            /*
            //画像ウィンドウを閉じる
            Ipc32v5.ret = Ipc32v5.IpDocClose();
            */
            #endregion

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
            }
        }
        #endregion

    }
}
