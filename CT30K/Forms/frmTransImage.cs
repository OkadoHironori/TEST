using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.PowerPacks;
using System.IO;
using System.Drawing.Drawing2D;
//
using CT30K.Common;
using CTAPI;
using TransImage;
using CT30K.Properties;
//using CT30K.Controls;
//using CT30K.Modules;

namespace CT30K
{
    public partial class frmTransImage : Form
    {
        ///* *************************************************************************** */
        ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver15.0               */
        ///* 客先　　　　： ?????? 殿                                                    */
        ///* プログラム名： frmTransmissionImage.frm                                     */
        ///* 処理概要　　：                                                              */
        ///* 注意事項　　：                                                              */
        ///* --------------------------------------------------------------------------- */
        ///* ＯＳ　　　　： Windows XP Professional (SP1)                                */
        ///* コンパイラ　： VB 6.0 (SP5)                                                 */
        ///* --------------------------------------------------------------------------- */
        ///* VERSION     DATE        BY                  CHANGE/COMMENT                  */
        ///*                                                                             */
        ///* V15.00    2008/12/25    (SI1) 間々田         　　                           */
        ///*                                                                             */
        ///* --------------------------------------------------------------------------- */
        ///* ご注意：                                                                    */
        ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。    */
        ///*                                                                             */
        ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2008                  */
        ///* *************************************************************************** */

        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************

        #region メンバ変数
        private TransImageControl transImageCtrl;

        //削除_Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
        //public RectangleShape[] myShape = null;

        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmTransImage myForm = null;

        //透視画像の配列（「元に戻す」用）
        ushort[] FImageOrg;
        //作業用配列
        ushort[] WorkImage;
        //シャープフィルタ用配列
        float[] SharpFilter;

        //Dim adv                 As Long
        //v17.00追加　山本 2009-09-29
        public int adv;

        ////このフォームで使用する線
        //public enum LineConstants
        //{
        //    ScanLine,
        //    //スキャンライン
        //    UpperLine,
        //    //コーンビーム時の上端ライン
        //    LowerLine,
        //    //コーンビーム時の下端ライン
        //    CenterLine,
        //    //中心線（縦）
        //    CenterLineH
        //    //中心線（横）   'v15.10追加 byやまおか 2009/10/22
        //}

        //追加2014/10/07hata_v19.51反映
        //'透視画像表示サイズ定数                     'v17.4X/v18.00追加 by 間々田 2011/03/15_追加2014/10/07hata_v19.51反映 
        public enum TransImageDispSizeConstants
        {
            SmallSize,  //'縮小サイズ
            MediumSize, //'標準サイズ
            LargeSize   //'拡大サイズ
        }

        //線
        //v15.10変更 byやまおか 2009/10/22
        //modImgProc.LineStruct[] myLine = new modImgProc.LineStruct[(int)LineConstants.CenterLineH + 1];
        private Dictionary<LineConstants, TransLine> myTlines = new Dictionary<LineConstants, TransLine>();

        //ハンドル対象ライン
        LineConstants myHandleLine;

        //2014/11/11hata Point型を変更？？
        PointF myHandleLineP1;
        PointF myHandleLineP2;
        //Point myHandleLineP1;
        //Point myHandleLineP2;

        //マウスダウン時のポイント座標
        Point MouseDownPoint;

        //MILのハンドル
        //public IntPtr hMil;

        //透視画像領域（共有メモリ）のハンドル
        //private int hMap;
        //private IntPtr hMap = IntPtr.Zero;

        private int tmr_Tick_Count;

        //ROI制御オブジェクト参照用
        private RoiData withEventsField_myRoi;
        public RoiData myRoi
        {
            get { return withEventsField_myRoi; }
            set
            {
                if (withEventsField_myRoi != null)
                {
                    withEventsField_myRoi.Changed -= myRoi_Changed;
                }
                withEventsField_myRoi = value;
                if (withEventsField_myRoi != null)
                {
                    withEventsField_myRoi.Changed += myRoi_Changed;
                }
            }
        }

        //ROI制御タイプ
        public enum TransImageProcType
        {
            TransRoiNone,
            TransRoiAutoPos
        }

        //ROI制御タイプ変数
        public TransImageProcType myTransImageProc;

        //Target プロパティ用
        string myTarget;

        //サポートしているイベント
        //イベント宣言
        //public event EventHandler RoiChanged;

        //イベント宣言
        public event EventHandler RoiChanged;
        //public event RoiChangedEventHandler RoiChanged;
        //public delegate void RoiChangedEventHandler();

        //透視画像が変更された
        public event TransImageChangedEventHandler TransImageChanged;
        public delegate void TransImageChangedEventHandler();

        //キャプチャオンオフ変更時イベント
        public event CaptureOnOffChangedEventHandler CaptureOnOffChanged;
        public delegate void CaptureOnOffChangedEventHandler(bool IsOn);

        //コントロール更新用のデリゲート
        delegate void TransImageUpdateDelegate();

        //ズーミング比率 1:同倍 2:1/2
        int myZoomScale;

        //キャプキャ画像のデータサイズ
        int myDataSize;

        //フレームレート
        float myFrameRate;

        //追加2014/10/07hata_v19.51反映
        //透視画像表示サイズ                         'v17.4X/v18.00追加 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        public TransImageDispSizeConstants myTransImageDispSize;

        //追加2014/10/07hata_v19.51反映
        //一度でもCaptureOnを実行したかどうかのフラグ    //v19.50 追加
        bool FlgCaptureOnFirstDone;

        //Rev25.00 透視プロファイル用に追加 by長野 2016/08/08 --->
        private Dictionary<StringConstants, TransString> myTStrings = new Dictionary<StringConstants, TransString>();
        int myLProfVPos = 0;
        int OldLProfVPos = -1;
        public int LProfVPos
        {
            get
            {
                return myLProfVPos;
            }
            set
            {
                myLProfVPos = value;
            }
        }
        int myLProfHPos = 0;
        int OldLProfHPos = -1;
        public int LProfHPos
        {
            get
            {
                return myLProfHPos;
            }
            set
            {
                myLProfHPos = value;
            }
        }

        //Rev25.03/Rev25.02 2ndだけ4K対応 by chouno 2017/02/07
        int myMonitor4KOn = 0;
        public int monitor4KOn
        {
            get
            {
                return myMonitor4KOn;
            }
            set
            {
                myMonitor4KOn = value;
            }
        }
        private int V0Pos = CTSettings.iniValue.FImageLProfileV0Pos;
        private int V100Pos = CTSettings.iniValue.FImageLProfileV100Pos;
        private int H0Pos = CTSettings.iniValue.FImageLProfileH0Pos;
        private int H100Pos = CTSettings.iniValue.FImageLProfileH100Pos;

        //Rev25.00 ラインプロファイルデータ 追加 by長野 2016/08/08;
        ushort[] myLProfileH = new ushort[CTSettings.detectorParam.h_size];
        ushort[] myLProfileV = new ushort[CTSettings.detectorParam.v_size];
        //private int old_mabiki_v = -1;
        //private int old_mabiki_h = -1;
        //Rev25.13 修正 by chouno 2017/11/18
        private float old_mabiki_v = -1.0f;
        private float old_mabiki_h = -1.0f;
        //<---
        #endregion

        //int MauseX = 0;
        //int MauseY = 0;



        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmTransImage()
        {
            InitializeComponent();

            // イベント定義
            InitializeEventHandler();

            InitControls();
        }
        #endregion

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmTransImage Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmTransImage();
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
            // CTBusyステータス変更時処理
            modCTBusy.StatusChanged += delegate
            {
                // スキャン中の場合は操作不可
                this.Enabled = !Convert.ToBoolean(modCTBusy.CTBusy & modCTBusy.CTScanStart);
            };

        }
        #endregion

        #region コントロールの初期化
        /// <summary>
        /// コントロールの初期化
        /// </summary>
        public void InitControls()
        {

            //ターゲットプロパティリセット
            Target = "";

            transImageCtrl = CTSettings.transImageControl;

            ctlTransImage.SizeX = transImageCtrl.Detector.h_size;
            ctlTransImage.SizeY = transImageCtrl.Detector.v_size;
            //ctlTransImage.ImageSize = new Size(transImageCtrl.Detector.h_size, transImageCtrl.Detector.v_size);

            int fWidth = this.Width - this.ClientSize.Width;
            int fHeight = this.Height - this.ClientSize.Height;

            this.Width = fWidth + CTSettings.detectorParam.h_size;
            this.Height = fHeight + CTSettings.detectorParam.v_size;


            ctlTransImage.SetBounds(0, 0, transImageCtrl.Detector.h_size, transImageCtrl.Detector.v_size);

            //削除_Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
            //myShape = new RectangleShape[] { myShape0, myShape1 };
            //myShape[0].Parent.Parent = ctlTransImage;
            //myShape[1].Parent.Parent = ctlTransImage;
            //myShape[0].Top = 0;
            //myShape[0].Left = 0;
            //myShape[1].Top = 50;
            //myShape[1].Left = 50;
            //myShape[0].Visible = false;
            //myShape[1].Visible = false;

            //描画用イベント
            transImageCtrl.TransImageChanged += new EventHandler(transImageCtrl_TransImageChanged);


            // 線の設定
            myTlines.Add(LineConstants.ScanLine, new TransLine(LineConstants.ScanLine));
            myTlines.Add(LineConstants.UpperLine, new TransLine(LineConstants.UpperLine));
            myTlines.Add(LineConstants.LowerLine, new TransLine(LineConstants.LowerLine));
            myTlines.Add(LineConstants.CenterLine, new TransLine(LineConstants.CenterLine));
            myTlines.Add(LineConstants.CenterLineH, new TransLine(LineConstants.CenterLineH));
            myTlines.Add(LineConstants.ProfilePosV, new TransLine(LineConstants.ProfilePosV)); //Rev25.00 透視プロファイル 垂直方向 by長野 2016/08/08
            myTlines.Add(LineConstants.ProfilePosH, new TransLine(LineConstants.ProfilePosH)); //Rev25.00 透視プロファイル 垂直方向 by長野 2016/08/08
            myTlines.Add(LineConstants.ProfileV, new TransLine(LineConstants.ProfileV)); //Rev25.00 透視プロファイル 垂直方向 by長野 2016/08/08
            myTlines.Add(LineConstants.ProfileH, new TransLine(LineConstants.ProfileH)); //Rev25.00 透視プロファイル 垂直方向 by長野 2016/08/08
            transImageCtrl.LProfileHPos = CTSettings.iniValue.FImageLProfileHPos;
            transImageCtrl.LProfileVPos = CTSettings.iniValue.FImageLProfileVPos;

            //ラインデータの参照を設定
            ctlTransImage.GetLines(ref myTlines);

            //文字列の設定
            myTStrings.Add(StringConstants.Profile0PosH, new TransString(StringConstants.Profile0PosH));
            myTStrings.Add(StringConstants.Profile100PosH, new TransString(StringConstants.Profile100PosH));
            myTStrings.Add(StringConstants.Profile0PosV, new TransString(StringConstants.Profile0PosV));
            myTStrings.Add(StringConstants.Profile100PosV, new TransString(StringConstants.Profile100PosV));
            SetTransString();

            //文字列データの参照を設定
            ctlTransImage.GetStrings(ref myTStrings);

            //Rev25.03/Rev25.02 2ndモニタが4k判断 add by chouno 2017/02/07
            foreach (Screen scr in Screen.AllScreens)
            {
                if (scr.Bounds.Height > 1200)
                {
                    monitor4KOn = 1;
                }
            }
        }
        #endregion


        /// <summary>
        /// 透視画像処理
        /// </summary>
        public TransImageControl TransImageCtrl { get { return transImageCtrl; } }

        //描画用イベント処理
        void transImageCtrl_TransImageChanged(object sender, EventArgs e)
        {

            try
            {
                TransImageUpdate();
            }
            catch
            {
            }
            finally
            {
            }

            //            int CurrentTime = 0;

            //            if (AppValue.IsTestMode)
            //            {
            //                CurrentTime = Winapi.GetTickCount();
            //                if (tmr_Tick_Count != 0)
            //                {
            //                    frmCTMenu.Instance.tslblCPU.Text = modCT30K.IntegTime(CurrentTime - tmr_Tick_Count).ToString("0.00");
            //                }
            //                tmr_Tick_Count = CurrentTime;
            //            }

            //#if NoCamera  //v17.00追加(ここから) byやまおか 2010/01/19

            //            string FileName = null;
            //            if (tmr_Tick_Count < 300)
            //            {
            //                tmr_Tick_Count = tmr_Tick_Count + 1;
            //            }
            //            else
            //            {
            //                tmr_Tick_Count = 1;
            //            }
            //            FileName = Path.Combine("e:\\デバッグ用透視画像1392×520", "Trans" + tmr_Tick_Count.ToString("000") + ".dat");
            //            //ImageOpen TransImage(0), FileName, h_size, v_size
            //            IICorrect.ImageOpen(ref modScanCorrectNew.TransImage[0], FileName, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);

            //#else
            //#endif 
            //            // 描画
            //            //ctlTransImage.SetImage(transImageCtrl.GetImage());
            //            //ctlTransImage.Invalidate();
            //            ctlTransImage.Picture = transImageCtrl.Picture;

        }

        //画像更新
        private void TransImageUpdate()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new TransImageUpdateDelegate(TransImageUpdate));
                return;
            }

            try
            {

                int CurrentTime = 0;

                if (AppValue.IsTestMode)
                {
                    CurrentTime = Winapi.GetTickCount();
                    if (tmr_Tick_Count != 0)
                    {
                        frmCTMenu.Instance.tslblCPU.Text = modCT30K.IntegTime(CurrentTime - tmr_Tick_Count).ToString("0.00");
                    }
                    tmr_Tick_Count = CurrentTime;
                }

#if NoCamera  //v17.00追加(ここから) byやまおか 2010/01/19

            string FileName = null;
            if (tmr_Tick_Count < 300)
            {
                tmr_Tick_Count = tmr_Tick_Count + 1;
            }
            else
            {
                tmr_Tick_Count = 1;
            }
            FileName = Path.Combine("e:\\デバッグ用透視画像1392×520", "Trans" + tmr_Tick_Count.ToString("000") + ".dat");
            //ImageOpen TransImage(0), FileName, h_size, v_size
            IICorrect.ImageOpen(ref modScanCorrectNew.TransImage[0], FileName, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);

#else
#endif
                //Rev25.00 ラインプロファイルONの場合は、ここでラインプロファイル描画用データを作成
                if (CTSettings.scanParam.LineProfOn)
                {
                    SetTransProfile();
                }

                // 描画
                //ctlTransImage.SetImage(transImageCtrl.GetImage());
                //ctlTransImage.Invalidate();
                ctlTransImage.Picture = transImageCtrl.Picture;


                //イベントによる通知
                if (TransImageChanged != null)
                {
                    TransImageChanged();  		//v16.20 上から移動した byやまおか 2010/04/06
                }

            }
            catch
            {
            }

        }

        //*******************************************************************************
        //機　　能： FrameRate プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        public float FrameRate
        {

            get { return myFrameRate; }
            set
            {

                myFrameRate = value;
                transImageCtrl.Detector.FrameRate = myFrameRate;

                //常時動作しているタイマーを調整する
                frmCTMenu.Instance.tmrStatus.Enabled = (myFrameRate == 15);
                //変更2014/10/07hata_v19.51反映
                //v19.50 タイマーの統合 by長野 2013/12/17
                //frmMechaControl.Instance.tmrSeqComm.Interval = (myFrameRate > 15 ? 2000 : 500);
                //frmMechaControl.Instance.tmrMecainf.Interval = (myFrameRate > 15 ? 2000 : 1000);
                frmMechaControl.Instance.tmrMecainfSeqComm.Interval = (myFrameRate > 15 ? 2000 : 1000);


                CTSettings.scanParam.fpd_gain = CTSettings.scansel.Data.fpd_gain;
                CTSettings.scanParam.fpd_integ = CTSettings.scansel.Data.fpd_integ;



                //同期・非同期の設定
                //transImageCtrl.CaptureOpen();
                CaptureOpen();

#if NoCamera  //v17.00追加(ここから) byやまおか 2010/01/19
#else

                if ((transImageCtrl.Detector.DetType == DetectorConstants.DetTypeII) | (transImageCtrl.Detector.DetType == DetectorConstants.DetTypeHama))
                {
                    int mode = ((CTSettings.scanParam.gFrameRateIndex == 1) ? Pulsar.M_ASYNCHRONOUS : Pulsar.M_SYNCHRONOUS);
                    Pulsar.MilSetGrabMode(Pulsar.hMil, mode);

                }
#endif //v17.00追加(ここまで) byやまおか 2010/01/19

            }
        }


        #region //CaptureOnプロパティは後で合わせる
        //*******************************************************************************
        //機　　能： CaptureOn プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        public bool CaptureOn
        {

            get { return transImageCtrl.CaptureOn; }
            set
            {

                //CT以外の時は無効にする v16.01 追加 by 山影 10-02-17
                //If Not IsCTmode Then Exit Property     'v16.02 CaptureOnの外で判定する by 山影 10-03-02

                //If NewValue = tmrLive.Enabled Then Exit Property   'v16.02削除 byやまおか 2010/03/03

                //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //RAMディスクが構築されているかどうか  'v17.40追加 byやまおか 2010/10/26
                //If UseRamDisk And (Not RamDiskIsReady) Then Exit Property
                //    If UseRamDisk Then      'v17.42修正 byやまおか 2010/11/04
                //        If (Not RamDiskIsReady) Then Exit Property
                //    End If
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


                if (value)
                {
                    //transImageCtrl.CaptureStart();
                    CaptureStart();
                }
                else
                {
                    //transImageCtrl.CaptureStop();
                    CaptureStop();
                }

            }
        }
        #endregion

        //追加2014/10/07hata_v19.51反映
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから
        //*******************************************************************************
        //機　　能： TransImageDispSize プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.4X/v18.00 2011/03/15 (豊川)間々田  新規作成
        //*******************************************************************************
        public TransImageDispSizeConstants TransImageDispSize
        {
            get { return myTransImageDispSize; }
            set
            {
                myTransImageDispSize = value;

                switch (myTransImageDispSize)
                {

                    //縮小
                    case TransImageDispSizeConstants.SmallSize:
                        modCT30K.hScale = CTSettings.detectorParam.fphm;
                        modCT30K.vScale = CTSettings.detectorParam.fpvm;
                        ZoomScale = 2;
                        break;
                    //TransScale = 4  'v18.00追加 byやまおか 2011/07/06 'v19.50 削除 転送した画像の1画素サイズの修正に使用しているが、すでに17.60で別方法による修正済み。by長野 2013/11/17

                    //標準
                    case TransImageDispSizeConstants.MediumSize:
                        modCT30K.hScale = CTSettings.detectorParam.fphm;
                        modCT30K.vScale = CTSettings.detectorParam.fpvm;
                        ZoomScale = 1;
                        break;
                    //TransScale = 2  'v18.00追加 byやまおか 2011/07/06 'v19.50 削除 転送した画像の1画素サイズの修正に使用しているが、すでに17.60で別方法による修正済み。by長野 2013/11/17

                    //拡大
                    case TransImageDispSizeConstants.LargeSize:
                        modCT30K.hScale = 1;
                        modCT30K.vScale = 1;
                        ZoomScale = 1;
                        break;
                    //TransScale = 1  'v18.00追加 byやまおか 2011/07/06 'v19.50 削除 転送した画像の1画素サイズの修正に使用しているが、すでに17.60で別方法による修正済み。by長野 2013/11/17

                }
                frmTransImageInfo.Instance.UpdateScaleBar();
            }
        }
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここまで

        //*******************************************************************************
        //機　　能： Target プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************

        public string Target
        {


            get { return myTarget; }
            set
            {

                myTarget = value;

                //Rev23.12 Valueが""以外の場合は、透視画像のオートコントラスト機能は使用不可とする by長野 2015/12/12
                if (frmScanControl.Instance != null)
                {
                    if (value != "")
                    {
                        frmScanControl.Instance.cmdWLWWAuto.Enabled = false;
                    }
                    else
                    {
                        frmScanControl.Instance.cmdWLWWAuto.Enabled = true;
                    }
                }

                //ファイル名をタイトルバーに表示
                UpdateCaption();

            }
        }

        //*******************************************************************************
        //機　　能： ZoomScale プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        public int ZoomScale
        {


            get { return myZoomScale; }
            set
            {

                //値を保持
                myZoomScale = value;

                if (modCT30K.CT30KSetup) //変更2014/10/28hata_起動時にzoomScaleで表示されるため、条件を付加
                {
                    //ちらつき防止のため 'v17.10追加 byやまおか 2010/08/25
                    this.Hide();

                    frmTransImageInfo.Instance.Hide();
                    frmTransImageControl.Instance.Hide();

                    //コントロールの再設定                       '追加 by 山本　2005-8-24
                    ResizeImage();

                    //ちらつき防止のため 'v17.10追加 byやまおか 2010/08/25
                    this.Show();

                    frmTransImageInfo.Instance.Show(frmCTMenu.Instance);
                    frmTransImageControl.Instance.Show(frmCTMenu.Instance);
                }
                else
                {
                    //コントロールの再設定                       '追加 by 山本　2005-8-24
                    ResizeImage();
                }

                //付帯情報のスケールバーを調整する
                //変更2014/10/07hata_v19.51反映
                ////if (modLibrary.IsExistForm(frmTransImageInfo.Instance))
                //if (modLibrary.IsExistForm("frmTransImageInfo"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
                //    frmTransImageInfo.Instance.UpdateScaleBar();
                //v19.50 2048表示の場合、スケールがはみ出すだめの対策 by長野 2014/01/28
                if (modLibrary.IsExistForm("frmTransImageInfo"))
                {
                    if (FlgCaptureOnFirstDone == true)
                    {

                        //            '付帯情報に表示
                        //            If IntegOn Then
                        //
                        //                frmTransImageInfo.Update FIntegNum
                        //
                        //            Else
                        //
                        //                frmTransImageInfo.Update 1
                        //
                        //            End If

                        //v19.51 積分枚数は記憶させた値を使う by長野 2014/03/19
                        frmTransImageInfo.Instance.MyUpdate(CTSettings.scanParam.bakIntegNum);

                    }
                }
            }
        }

        //*******************************************************************************
        //機　　能： ImageProcプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.0　2009/03/25   (SI1)間々田   リニューアル
        //*******************************************************************************
        public TransImageProcType TransImageProc
        {
            get { return myTransImageProc; }
            set
            {
                //前回のCTBusyフラグをリセット
                if (myTransImageProc != TransImageProcType.TransRoiNone)
                {
                    //modCTBusy.CTBusy = modCTBusy.CTBusy & (!modCTBusy.CTImageProcessing);
                    modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTImageProcessing);
                }

                //処理の記憶
                myTransImageProc = value;

                //CTBusyフラグをセット
                if (myTransImageProc != TransImageProcType.TransRoiNone)
                {
                    modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTImageProcessing;
                }

                //roiオブジェクトが存在する場合
                if ((DrawRoi.roi != null))
                {
                    DrawRoi.roi.DeleteAllRoiData();		//いったんすべてのROIを削除
                    DrawRoi.roi = null;				    //オブジェクト破棄
                }

                //ROI制御スタートさせる場合
                if (myTransImageProc != TransImageProcType.TransRoiNone)
                {

                    //ROIクラスの生成
                    DrawRoi.roi = new RoiData();

                    //描画対象フォームを設定
                    //削除_Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
                    //DrawRoi.roi.SetTarget(ctlTransImage, myShape[0], myShape[1]);
                    //DrawRoi.roi.SetTarget(ctlTransImage);
                    //Rev20.00 引数追加 by長野 2015/02/05
                    DrawRoi.roi.SetTarget(ctlTransImage,1);

                }

                //roiの参照設定
                myRoi = DrawRoi.roi;

                System.Windows.Forms.ToolStripButton Button = null;
                var _with1 = frmRoiTool.Instance.Toolbar1;

                //画像処理ごとの設定
                switch (myTransImageProc)
                {

                    //自動スキャン位置指定
                    case TransImageProcType.TransRoiAutoPos:
                        //変更2015/01/22hata
                        ////_with1.Items["Go"].Description = CTResources.LoadResString(15204);
                        //_with1.Items["tsbtnGo"].AccessibleDescription = CTResources.LoadResString(15204);
                        _with1.Items["tsbtnGo"].Text = CTResources.LoadResString(15204);
                        
                        //自動スキャン位置指定
                        _with1.Items["tsbtnRectangle"].Enabled = true;
                        //長方形
                        ((ToolStripButton)_with1.Items["tsbtnRectangle"]).Checked = true;
                        //「長方形」ボタンをクリックした状態にする
                        myRoi.ModeToPaint(RoiData.RoiShape.ROI_RECT);

                        break;

                    //ROI制御終了
                    default:

                        //変更2015/01/22hata
                        ////_with1.Items["Go"].Description = "";
                        //_with1.Items["tsbtnGo"].AccessibleDescription = "";
                        _with1.Items["tsbtnGo"].Text = "";

                        //座標入力フォームアンロード
                        frmInputRoiData.Instance.Close();

                        //ツールバー上のボタンをオフ・使用不可状態にする
                        foreach (ToolStripButton Button_loopVariable in _with1.Items)
                        {
                            Button = Button_loopVariable;
                            Button.Checked = false;
                            Button.Enabled = false;
                        }

                        break;

                    //'右クリック時のメニューの調整
                    //mnuROIEditCopy.Enabled = False          'ｺﾋﾟｰ(&C)
                    //mnuROIEditPaste.Enabled = False         '貼り付け(&P)
                    //mnuROIEditDelete.Enabled = False        'ROI削除(&D)
                    //mnuROIEditAllDelete.Enabled = False     'すべてのROI削除(&A)
                    //mnuRoiInput.Enabled = False             'ROI座標入力

                }

                //共通設定
                if ((myRoi != null))
                {
                    //変更2015/01/22hata
                    ////_with1.Items["Go"].ToolTipText = StringTable.GetResString(StringTable.IDS_Exe, _with1.Items["Go"].Description);
                    //_with1.Items["tsbtnGo"].ToolTipText = StringTable.GetResString(StringTable.IDS_Exe, _with1.Items["tsbtnGo"].AccessibleDescription);
                    _with1.Items["tsbtnGo"].ToolTipText = StringTable.GetResString(StringTable.IDS_Exe, _with1.Items["tsbtnGo"].Text);

                    //If .Buttons("Open").Enabled Then
                    //    .Buttons("Save").Description = .Buttons("Open").Description
                    //    .Buttons("Open").ToolTipText = GetResString(IDS_Open, .Buttons("Open").Description)  '～を開く
                    //    .Buttons("Save").ToolTipText = GetResString(IDS_Save, .Buttons("Open").Description)  '～の保存
                    //End If
                }

                //「実行」ボタン、「キャンセル」ボタン、メッセージ欄の設定
                switch (myTransImageProc)
                {
                    case TransImageProcType.TransRoiNone:
                        //lblPrompt.Caption = ""
                        frmRoiTool.Instance.Close();
                        break;
                    default:
                        _with1.Items["tsbtnGo"].Enabled = true;
                        _with1.Items["tsbtnExit"].Enabled = true;
                        //lblPrompt.Caption = .Buttons("Go").Description
                        //VB6.ShowForm(frmRoiTool.Instance, , this);
                        //if (!modLibrary.IsExistForm(frmRoiTool.Instance))//変更2015/01/30hata
                        if (!modLibrary.IsExistForm("frmRoiTool"))
                        {
                            frmRoiTool.Instance.Show(this);
                        }
                        else
                        {
                            frmRoiTool.Instance.WindowState = FormWindowState.Normal;
                            frmRoiTool.Instance.Visible = true;
                        }

                        break;

                }
            }
        }

        //TransImageCotrolに移行
        ////*******************************************************************************
        ////機　　能： 透視画像更新処理
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： Captured        [I/ ] Boolean   True:CT30Kでキャプチャされた画像である
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： V1.00  99/XX/XX   ????????      新規作成
        ////*******************************************************************************
        ////Public Sub Update(Optional ByVal Captured As Boolean = True)
        ////変更 by 間々田 2009/08/21
        //public void MyUpdate(bool Captured = true, int RasterOp = 0)
        //{


        //    //'時間計測時
        //    //If IsTestMode Then
        //    //    Dim StartTime As Long
        //    //    StartTime = GetTickCount()
        //    //End If

        //    // ERROR: Not supported in C#: OnErrorStatement

        //    //v17.00added by 山本 2009-09-25

        //    if (Captured)
        //    {

        //        //Ｘ線検出器がフラットパネルの場合
        //        if (CTSettings.detectorParam.Use_FlatPanel)
        //        {

        //            //ゲイン補正
        //            if (CTSettings.scanParam.FPGainOn)
        //                IICorrect.FpdGainCorrect(TransImage[0], GAIN_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, adv);

        //            //欠陥補正
        //            //If FPDefOn Then FpdDefCorrect_short TransImage(0), Def_IMAGE(0), h_size, v_size, 0, v_size - 1     'v17.00削除 byやまおか 2010/01/19
        //            if ((modGlobal.DetType == modGlobal.DetectorConstants.DetTypeHama))
        //                IICorrect.FpdDefCorrect_short(TransImage[0], Def_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, 0, CTSettings.detectorParam.v_size - 1);
        //            //v17.00追加 byやまおか 2010/01/19

        //        }

        //        //積分処理時はアベレージング処理やシャープ処理は実施しない
        //        if (!CTSettings.transImageControl.IntegOn)
        //        {

        //            if (CTSettings.scanParam.AverageOn)
        //            {
        //                ImgProc.Averaging(TransImage, WorkImage, myDataSize, CTSettings.scanParam.AverageNum);
        //                if (CTSettings.scanParam.SharpOn)
        //                {
        //                    //Call SpatialFilter(WorkImage(0), TransImage(0), SharpFilter(0), h_size, v_size, 3)
        //                    //Sharpen WorkImage(0), TransImage(0), h_size, v_size 'v15.0変更 by 間々田 2009/06/16
        //                    ImgProc.Sharpen(WorkImage, TransImage, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, ctlTransImage.Max);
        //                    //v17.10変更 画像の最大輝度値を追加 byやまおか 2010/08/10
        //                }
        //                else
        //                {
        //                    //TransImage = VB6.CopyArray(WorkImage);
        //                    Array.Copy(WorkImage, TransImage, TransImage.Length);

        //                }
        //            }
        //            else if (CTSettings.scanParam.SharpOn)
        //            {
        //                //Call SpatialFilter(TransImage(0), WorkImage(0), SharpFilter(0), h_size, v_size, 3)
        //                //Sharpen TransImage(0), WorkImage(0), h_size, v_size 'v15.0変更 by 間々田 2009/06/16
        //                ImgProc.Sharpen(TransImage[0], WorkImage[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, ctlTransImage.Max);
        //                //v17.10変更 画像の最大輝度値を追加 byやまおか 2010/08/10
        //                //TransImage = VB6.CopyArray(WorkImage);
        //                Array.Copy(WorkImage, TransImage, TransImage.Length);
        //            }

        //        }

        //    }

        //    //描画オペレーションが指定されている
        //    if ((RasterOp != null))
        //    {
        //        ctlTransImage.RasterOp = RasterOp;
        //    }

        //    //変換対象となる画像の配列を登録
        //    if (tmrLive.Enabled)
        //    {
        //        //取り込んだ透視画像データを元に直接ピクチャを作成（透視画像コントロールに登録はしない）
        //        ctlTransImage.MakePictute(ref TransImage);
        //    }
        //    else
        //    {
        //        //透視画像コントロールに透視画像データを登録
        //        ctlTransImage.SetImage(ref TransImage[0]);
        //    }

        //    //イベント生成：透視画像が更新された
        //    if (TransImageChanged != null)
        //    {
        //        TransImageChanged();
        //    }

        //    //時間計測時
        //    //If AppValue.IsTestMode Then
        //    //    frmCTMenu.StatusBar1.Panels("CPU").Text = Format$(IntegTime(GetTickCount() - StartTime), "0.00")
        //    //End If

        //}


        //*******************************************************************************
        //機　　能： フォームのキャプションを更新する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void UpdateCaption(string RoiInfo = "")
        {

            //基本のキャプション
            this.Text = CTResources.LoadResString(StringTable.IDS_TransImage);
            //透視画像

            //表示画像ファイル名を付加
            if (!string.IsNullOrEmpty(myTarget))
            {
                this.Text = this.Text + " - " + myTarget;
            }

            //Roi情報付加
            if (!string.IsNullOrEmpty(RoiInfo))
            {
                this.Text = this.Text + " " + RoiInfo;
            }

        }


        /// <summary>
        /// 指定されたファイル名の画像をロードする
        /// </summary>
        public void LoadFromFile(string FileName)
        {

            string Ext = null;
            //int i = 0;
            //short theBinning = 0;

            //変更 64ﾋﾞｯﾄ対応（ImageProHelperを使用）(ここから)-------------//
            //Ipc32v5.IPDOCINFO dInfo = default(Ipc32v5.IPDOCINFO); 
            int dInfoWidth;
            int dInfoHeight;
            int dInfoClass;
            //変更 64ﾋﾞｯﾄ対応（ImageProHelperを使用）(ここまで)-------------//

            byte[] ByteImage = null;
            ushort[] WordImage = null;
            int ret = 0;

            //エラー時の扱い                 'v17.50追加 by 間々田 2011/02/15
            // ERROR: Not supported in C#: OnErrorStatement

            try
            {

                //画像フォーマット（拡張子）のチェック
                //拡張子先頭のピリオドを消す
                Ext = Path.GetExtension(FileName).TrimStart('.');
                switch (Ext.ToLower())
                {
                    case "bmp":
                    case "jpg":
                    case "tif":
                        break;
                    default:
                        //メッセージ表示：BMP、JPG、TIF以外の画像を開くことはできません。
                        //Interaction.MsgBox(CTResources.LoadResString(StringTable.IDS_msgFluoroIPOpenErr), MsgBoxStyle.Critical);
                        MessageBox.Show(CTResources.LoadResString(StringTable.IDS_msgFluoroIPOpenErr), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    //break;
                }

                //イメージプロ起動
                if (!modCT30K.StartImagePro())
                    return;

                #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                /*
		        //画像ウィンドウをすべて閉じる
		        ret = Ipc32v5.IpAppCloseAll();
		        //イメージプロで画像を開く
		        ret = Ipc32v5.IpWsLoad(FileName, Ext);
		        //ドキュメント情報を取得
		        ret = Ipc32v5.IpDocGet(GETDOCINFO, DOCSEL_ACTIVE, dInfo);
                */
                float[] Result = new float[10];
                ret = CallImageProFunction.CallLoadImageStep1(FileName, Ext, Result);
                //2014/11/07hata キャストの修正
                //dInfoWidth = (int)Result[0];
                //dInfoHeight = (int)Result[1];
                //dInfoClass = (int)Result[2];
                dInfoWidth = Convert.ToInt32(Result[0]);
                dInfoHeight = Convert.ToInt32(Result[1]);
                dInfoClass = Convert.ToInt32(Result[2]);
                #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


                //PkeFPDの場合の額縁幅を計算する 'v17.53追加 byやまおか 2011/05/12
                int modX = 0;
                int modY = 0;
                if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) & (!CTSettings.detectorParam.Use_FpdAllpix))
                {
                    //2014/11/07hata キャストの修正
                    //modX = (int)((ctlTransImage.SizeX % 100) / CTSettings.detectorParam.fpvm / myZoomScale);
                    //modY = (int)((ctlTransImage.SizeY % 100) / CTSettings.detectorParam.fphm / myZoomScale);
                    modX = Convert.ToInt32((ctlTransImage.SizeX % 100) / CTSettings.detectorParam.fpvm / (float)myZoomScale);
                    modY = Convert.ToInt32((ctlTransImage.SizeY % 100) / CTSettings.detectorParam.fphm / (float)myZoomScale);
                }
                else
                {
                    modX = 0;
                    modY = 0;
                }

                //額縁を付加した透視画像だけのサイズ   'v17.53追加 byやまおか 2011/05/12
                int imgWidth = 0;
                int imgHeight = 0;
                //変更 64ﾋﾞｯﾄ対応（ImageProHelperを使用）(ここから)-------------//
                //imgWidth = dInfo.Width - frmTransImageInfo.Instance.ClientRectangle.Width + modX;
                //imgHeight = dInfo.Height + modY;
                imgWidth = dInfoWidth - frmTransImageInfo.Instance.ClientRectangle.Width + modX;
                imgHeight = dInfoHeight + modY;
                //変更 64ﾋﾞｯﾄ対応（ImageProHelperを使用）(ここまで)-------------//

                //ビニング使用可能な場合、透視画像のビニングモードと現在のビニングモードが同一であるか調べる
                if (CTSettings.scaninh.Data.binning == 0)
                {

                    //とりあえずあとで
                    ///        theBinning = -1
                    ///        For i = 0 To 2
                    ///            'vm の取得
                    ///            If scancondpar.v_size = dInfo.Height * scancondpar.v_mag(i) Then theBinning = i
                    ///        Next
                    ///
                    ///        If theBinning = -1 Then
                    ///
                    ///            'メッセージ表示：
                    ///            '   開こうとしている透視画像のサイズが不適切です。
                    ///            MsgBox LoadResString(9478), vbCritical
                    ///
                    ///            Exit Sub
                    ///
                    ///        ElseIf theBinning <> scansel.binning Then
                    ///
                    ///            'メッセージ表示：
                    ///            '   開こうとしている透視画像のビニングモードと現在のビニングモードが異なります。
                    ///            '   画面サイズを変えるためにビニングモードを変更しますか？
                    ///            '   （保存してある透視画像のビニングモードとは必ずしも一致しません）
                    ///            If MsgBox(LoadResString(12750), vbExclamation + vbOKCancel) = vbCancel Then Exit Sub
                    ///
                    ///            'scansel更新
                    ///            scansel.binning = theBinning
                    ///            PutScansel scansel
                    ///
                    ///            GetBinning
                    ///            frmCTMenu.UpdateBinningMode                                     'v11.4追加 by 間々田 2006/03/13
                    ///
                    ///            'コントロールの再設定
                    ///            Display1.Free
                    ///            ImageBuffer.Free
                    ///            InitControls
                    ///
                    ///        End If

                    //    'ロードしようとする画像の高さが現在のディスプレイコントロールの高さより大きい場合
                    //    ElseIf dInfo.Height > Display1.SizeY Then
                    //        Display1.Zoom 1, 1, True                'ディスプレイコントロールを拡大する
                    //
                    //    'ロードしようとする画像の高さが現在のディスプレイコントロールの高さより小さい場合
                    //    ElseIf dInfo.Height < Display1.SizeY Then
                    //        Display1.Zoom -1, -1, True              'ディスプレイコントロールを縮小する

                    //以下に変更 by 間々田 2005/02/16
                }
                else
                {

                    var _with19 = ctlTransImage;
                    //v17.53追加 byやまおか 2011/05/12
                    //ロードしようとする画像の高さとディスプレイコントロールの高さを比較
                    //変更 64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    ////switch (dInfo.Height) {
                    //switch (dInfoHeight)
                    //{
                    //    //高さが同じ
                    //    //Case ctlTransImage.Height
                    //    case _with19.Height:
                    //    case _with19.Height - modY:
                    //        //PkeFPD額縁除去に対応   'v17.53変更 byやまおか 2011/05/12
                    //        break;

                    //    //何もしない

                    //    //ロードしようとする画像がディスプレイコントロールの２倍の場合
                    //    //Case ctlTransImage.Height * 2
                    //    case _with19.Height * 2:
                    //    case _with19.Height * 2 - modY:
                    //        //PkeFPD額縁除去に対応   'v17.53変更 byやまおか 2011/05/12

                    //        //ディスプレイコントロールを（２倍に）拡大する
                    //        //Display1.Zoom 1, 1, True
                    //        ZoomScale = 1;
                    //        break;

                    //    //Case ctlTransImage.Height / 2
                    //    case _with19.Height / 2:
                    //    case _with19.Height / 2 - modY:
                    //        //PkeFPD額縁除去に対応   'v17.53変更 byやまおか 2011/05/12

                    //        //ディスプレイコントロールを縮小する
                    //        //Display1.Zoom -1, -1, True
                    //        ZoomScale = 2;
                    //        break;

                    //    default:
                    //        //メッセージ表示：
                    //        //   開こうとしている透視画像のサイズが不適切です。
                    //        //Interaction.MsgBox(CT30K.My.Resources.str9478, MsgBoxStyle.Critical);
                    //        MessageBox.Show(CTResources.LoadResString(9478), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //        return;

                    //        break;
                    //}
                    if ((dInfoHeight == _with19.Height) || (dInfoHeight == (_with19.Height - modY)))
                    {
                        //何もしない
                    }
                    else if ((dInfoHeight == _with19.Height * 2) || (dInfoHeight == (_with19.Height * 2 - modY)))
                    {
                        //PkeFPD額縁除去に対応   'v17.53変更 byやまおか 2011/05/12

                        //ディスプレイコントロールを（２倍に）拡大する
                        //Display1.Zoom 1, 1, True
                        ZoomScale = 1;
                    }
                    //2014/11/07hata キャストの修正
                    //else if ((dInfoHeight == _with19.Height / 2) || (dInfoHeight == (_with19.Height / 2 - modY)))
                    else if ((dInfoHeight == Convert.ToInt32(_with19.Height / 2F)) || (dInfoHeight == Convert.ToInt32(_with19.Height / 2F - modY)))
                    {
                        //PkeFPD額縁除去に対応   'v17.53変更 byやまおか 2011/05/12

                        //ディスプレイコントロールを縮小する
                        //Display1.Zoom -1, -1, True
                        ZoomScale = 2;
                    }
                    else
                    {
                        //メッセージ表示：
                        //   開こうとしている透視画像のサイズが不適切です。
                        //Interaction.MsgBox(CT30K.My.Resources.str9478, MsgBoxStyle.Critical);
                        MessageBox.Show(CTResources.LoadResString(9478), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                }


                //次の条件式が真ならば付帯情報付の画像とみなす
                bool InfoOn = false;
                //InfoOn = (Convert.ToInt32(dInfo.Height) * Convert.ToInt32(ctlTransImage.Width) != Convert.ToInt32(ctlTransImage.Height) * Convert.ToInt32(dInfo.Width));
                InfoOn = (Convert.ToInt32(dInfoHeight) * Convert.ToInt32(ctlTransImage.Width) != Convert.ToInt32(ctlTransImage.Height) * Convert.ToInt32(dInfoWidth));


                //透視画像の横幅
                int ImageWidth = 0;
                //ImageWidth = (InfoOn ? dInfo.Width - frmTransImageInfo.Instance.ClientRectangle.Width : dInfo.Width);
                ImageWidth = (InfoOn ? dInfoWidth - frmTransImageInfo.Instance.ClientRectangle.Width : dInfoWidth);

                //Image-Proから透視画像領域のデータを取得
                //IMC_GRAY = 1;IMC_GRAY16 = 6;
                //if (dInfo.Class_Renamed == Ipc32v5.IMC_GRAY16) {
                if (dInfoClass == 6)
                {
                    //CTSettings.IPOBJ.GetWordImage(WordImage,0 ,0 , ImageWidth, dInfo.Height);
                    CTSettings.IPOBJ.GetWordImage(ref WordImage, 0, 0, ImageWidth, dInfoHeight);

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    //ret = Ipc32v5.IpWsConvertImage(Ipc32v5.IMC_GRAY, Ipc32v5.CONV_SCALE, 0, 0, 0, 0);
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    ret = CallImageProFunction.CallIpWsConvertImage(0);
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


                    //} else if (dInfo.Class_Renamed != Ipc32v5.IMC_GRAY) {     //IMC_GRAYでない画像データに対してIPOBJ.GetByteImageByteImageを実行するとアプリが落ちるのでこうする 'v17.50追加 by 間々田 2011/02/28
                }
                else if (dInfoClass != 1)
                {
                    //IMC_GRAYでない画像データに対してIPOBJ.GetByteImageByteImageを実行するとアプリが落ちるのでこうする 'v17.50追加 by 間々田 2011/02/28

                    //ret = Ipc32v5.IpWsConvertImage(Ipc32v5.IMC_GRAY, Ipc32v5.CONV_SCALE, 0, 0, 0, 0);			    //画像データを配列に入れる
                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    ret = CallImageProFunction.CallIpWsConvertImage(0);
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

                    //CTSettings.IPOBJ.GetByteImage(ByteImage, 0, 0, ImageWidth, dInfo.Height);
                    CTSettings.IPOBJ.GetByteImage(ref ByteImage, 0, 0, ImageWidth, dInfoHeight);

                }
                else
                {
                    //画像データを配列に入れる
                    //CTSettings.IPOBJ.GetByteImage(ByteImage, 0, 0, ImageWidth, dInfo.Height);
                    CTSettings.IPOBJ.GetByteImage(ref ByteImage, 0, 0, ImageWidth, dInfoHeight);
                }

                //付帯情報の扱い
                byte[] theImage = null;
                var _with20 = frmTransImageInfo.Instance;


                //付帯情報付の画像の場合
                if (InfoOn)
                {
                    //付帯情報を切り出す
                    CTSettings.IPOBJ.GetByteImage(ref theImage, ImageWidth, 0, _with20.ClientRectangle.Width, _with20.ClientRectangle.Height);

                    //付帯情報付の画像でない場合、透視画像付帯情報フォームをクリア
                }
                else
                {
                    theImage = new byte[_with20.ClientRectangle.Width * _with20.ClientRectangle.Height];
                    _with20.Tag = "";
                }

                //画像配列を付帯情報表示用イメージバッファに入れる
                _with20.SetImage(ref theImage);

                //新たにImage-Proの画像ウィンドウを生成し，上で取得した画像データを描画
                //if (dInfo.Class_Renamed == IMC_GRAY16) {
                if (dInfoClass == 6)
                {
                    //CTSettings.IPOBJ.DrawWordImage(WordImage,0 ,0 , ImageWidth, dInfo.Height, true);
                    CTSettings.IPOBJ.DrawWordImage(WordImage, 0, 0, ImageWidth, dInfoHeight, true);
                }
                else
                {
                    //CTSettings.IPOBJ.DrawWordImage(WordImage, 0, 0, ImageWidth, dInfo.Height, true);
                    CTSettings.IPOBJ.DrawByteImage(ByteImage, 0, 0, ImageWidth, dInfoHeight, true);

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    //ret = Ipc32v5.IpWsConvertImage(Ipc32v5.IMC_GRAY16, Ipc32v5.CONV_SCALE, 0, 0, 0, 0);
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    ret = CallImageProFunction.CallIpWsConvertImage(1);
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//
                }

                //表示前に額縁処理を行う(ここから)   'v17.53追加 byやまおか 2011/05/12
                #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                /*           
                //画像コピー
                ret = Ipc32v5.IpWsCopy();
                //新規画像ウィンドウ作成
		        //If IpWsCreate(ImageWidth, imgHeight, 300, IMC_GRAY16) < 0 Then Exit Sub
                if (Ipc32v5.IpWsCreate(ImageWidth + modX, imgHeight, 300, Ipc32v5.IMC_GRAY16) < 0)
			        return;
		        //v17.53変更 byやまおか 2011/05/12
                //新規画像ウィンドウに貼り付ける
                ret = Ipc32v5.IpWsPaste(modX / 2 - 1, modY / 2 - 1);
                //アクティブな画像を再描画する
                ret = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);
		        //表示前に額縁処理を行う(ここまで)   'v17.53追加 byやまおか 2011/05/12
                //指定した大きさにリサイズ
                ret = Ipc32v5.IpWsScale(CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, 1);
                */
                //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                //ret = CallImageProFunction.CallLoadImageStep3(dInfoHeight, ImageWidth, modX, modY, imgHeight, ImageWidth);
                //Rev20.00 修正 by長野 2014/12/04
                ret = CallImageProFunction.CallLoadImageStep3(ctlTransImage.SizeY, ctlTransImage.SizeX, modX, modY, imgHeight, ImageWidth);
                
                if (ret < 0) return;
                #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


                //再度画像データをリサイズされた配列に入れる
                CTSettings.IPOBJ.GetWordImage(ref WordImage, 0, 0, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                
                                //I.I.の場合(透視画像が12bit以下の場合)  'v17.10追加 byやまおか 2010/08/31
                if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypeII))
                {
                    //FimageBitIndexに基づき、16ビットを 8/10/12ビットにする
                    ScanCorrect.ChangeToCTImage(ref WordImage[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, 0, (int)(Math.Pow(2, (8 + modCT30K.FimageBitIndex * 2)) - 1));
                }
                else
                {
                    //16ビットにする
                    ScanCorrect.ChangeToCTImage(ref WordImage[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, 0, 65535);
                }

                //左右反転対応        'v17.50追加 2011/02/04 by 間々田
                if (transImageCtrl.Detector.IsLRInverse)
                {
                    ImgProc.ConvertMirror(ref WordImage[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                }

                //透視画像にコピー
                //TransImage = CopyArray(WordImage);
                //transImageCtrl.SetImage(WordImage);
                //処理画像をTransImageにセットする
                transImageCtrl.SetTransImage(WordImage);

                //イメージプロのドキュメントをクローズ
                #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                /*
                ret = Ipc32v5.IpDocClose();
                */
                //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                ret = CallImageProFunction.CallIpDocClose();
                #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

                //'v11.3以下に変更ここまで
                //
                //'FimageBitIndexに基づき、8ビットを 8/10/12ビットにする
                //For i = LBound(TransImage) To UBound(TransImage)
                //    TransImage(i) = theImage(i) / 255 * (2 ^ (8 + FimageBitIndex * 2) - 1)
                //Next

                //コントラストをリセットする
                frmScanControl.Instance.cmdWLWWReset_Click(null, new System.EventArgs());

                //更新：白黒反転させない
                ////MyUpdate(false, vbSrcCopy);
                //MyUpdate(false, 0);
                //更新
                transImageCtrl.Update(false);
                //ctlTransImage.Invalidate();
                ctlTransImage.Refresh();

                //キャプションにファイル名をセット
                Target = FileName;

                //return;

                //ErrorHandler:		    //v17.50追加 by 間々田 2011/02/15

            }
            catch
            {

                //v17.50追加 by 間々田 2011/02/15
                //MsgBox "透視画像読み込み時にエラーが発生しました。指定された透視画像ファイルが正しくない可能性があります。", vbCritical
                //v17.60
                //Interaction.MsgBox(CT30K.My.Resources.str20177, MsgBoxStyle.Critical);
                MessageBox.Show(CTResources.LoadResString(20177), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// 表示画像を保存する
        /// </summary>
        public void SaveToFile(string FileName, bool SaveInfomation)
        {


            byte[] ByteImage = null;
            ushort[] WordImage = null;
            //RECT ipRect = default(RECT);		    //v17.00追加 byやまおか 2010/03/04
            int ret = 0;

            //イメージプロ起動
            if (!modCT30K.StartImagePro())
                return;

            //画像ウィンドウをすべて閉じる   'v17.00追加 byやまおか 2010/03/04
            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            //ret = IpAppCloseAll();
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            ret = CallImageProFunction.CallIpAppCloseAll();
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

            //拡張子先頭のピリオドを消す
            string Extension = Path.GetExtension(FileName).TrimStart('.');

            //TIFフォーマット？
            bool IsTif = false;
            //IsTif = (Strings.UCase(Path.GetExtension((FileName)) == "TIF");
            IsTif = (Extension.ToLower() == "TIF");

            int modX = 0;
            int modY = 0;
            int imgWidth = 0;
            int imgHeight = 0;
            int Min = 0;
            int Max = 0;
            byte[] tmpWordImage = null;
            byte[] tmpByteImage = null;
            byte[] ByteImage2 = null;
            var _with17 = ctlTransImage;

            //PkeFPDの場合の額縁幅を計算する 'v17.53追加 byやまおか 2011/05/12
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) & (!CTSettings.detectorParam.Use_FpdAllpix))
            {
                modX = (_with17.SizeX % 100);
                modY = (_with17.SizeY % 100);
            }
            else
            {
                modX = 0;
                modY = 0;
            }

            //保存する画像のサイズ   'v17.53追加 byやまおか 2011/05/12
            //2014/11/07hata キャストの修正
            //imgWidth = (int)(_with17.Width - modX / CTSettings.detectorParam.fpvm / myZoomScale);
            //imgHeight = (int)(_with17.Height - modY / CTSettings.detectorParam.fphm / myZoomScale);
            imgWidth = Convert.ToInt32(_with17.Width - modX / CTSettings.detectorParam.fpvm / (float)myZoomScale);
            imgHeight = Convert.ToInt32(_with17.Height - modY / CTSettings.detectorParam.fphm / (float)myZoomScale);


            if (IsTif)
            {
                //Integer画像を取得（配列に格納）
                WordImage = new ushort[_with17.SizeX * _with17.SizeY];
                //_with17.GetImage(WordImage);
                WordImage = transImageCtrl.GetImage();

                //左右反転対応        'v17.53追加 2011/05/13 byやまおか
                if (transImageCtrl.Detector.IsLRInverse)
                {
                    ImgProc.ConvertMirror(ref WordImage[0], _with17.SizeX, _with17.SizeY);
                }

                //画像を16ビットフルレンジにする
                //Min = ctlTransImage.WindowLevel - ctlTransImage.WindowWidth / 2;
                //Max = ctlTransImage.WindowLevel + ctlTransImage.WindowWidth / 2;
                //2014/11/07hata キャストの修正
                //Min = transImageCtrl.WindowLevel - transImageCtrl.WindowWidth / 2;
                //Max = transImageCtrl.WindowLevel + transImageCtrl.WindowWidth / 2;
                Min = Convert.ToInt32(transImageCtrl.WindowLevel - transImageCtrl.WindowWidth / 2F);
                Max = Convert.ToInt32(transImageCtrl.WindowLevel + transImageCtrl.WindowWidth / 2F);

                //ChangeFullRange_Short WordImage(0), .SizeX, .SizeY, Min, Max
                ScanCorrect.ChangeFullRange_UShort(ref WordImage[0], _with17.SizeX, _with17.SizeY, Min, Max);
                //v17.20変更 透視画像16bitに対応 byやまおか 2010/09/16

                //白黒反転して描画している場合   '追加 2009/08/21
                //if (_with17.RasterOp == vbNotSrcCopy)
                if (transImageCtrl.RasterOp != 0)
                    //ReverseWordImage(WordImage[0], Information.UBound(WordImage) + 1);
                    ImgProc.ReverseWordImage(ref WordImage[0], WordImage.GetUpperBound(0) + 1);

                //イメージプロの新規ウィンドウ作成し，画像データ（配列）を書込む
                CTSettings.IPOBJ.DrawWordImage(WordImage, 0, 0, _with17.SizeX, _with17.SizeY, true);

                //'PkeFPDの場合、額縁をつける 'v17.00追加(ここから) byやまおか 2010/03/04
                //'If (DetType = DetTypePke) Then
                //If (DetType = DetTypePke) And (Not Use_FpdAllpix) Then  'v17.22変更 byやまおか 2010/10/19
                //
                //    Dim tmpWordImage()  As Byte
                //    ReDim tmpWordImage(.SizeX * .SizeY - 1)
                //    .GetByteImage tmpWordImage
                //
                //    '一回り小さくする
                //    ipRect.Left = (.SizeX Mod 100) / 2 - 1
                //    ipRect.Top = (.SizeY Mod 100) / 2 - 1
                //    ipRect.Right = .SizeX - (.SizeX Mod 100) / 2 - 1
                //    ipRect.Bottom = .SizeY - (.SizeY Mod 100) / 2 - 1
                //    ret = IpAoiCreateBox(ipRect)
                //    ret = IpWsCopy()
                //    '新規画像ウィンドウ作成
                //    'If IpWsCreate(.SizeX, .SizeY, 300, IMC_GRAY) < 0 Then Exit Sub
                //    If IpWsCreate(.SizeX, .SizeY, 300, IMC_GRAY16) < 0 Then Exit Sub    'v17.10修正 byやまおか 2010/08/31
                //    '新規画像ウィンドウを黒く塗りつぶす
                //    ret = IpWsFill(0, 3, 0)
                //    '新規画像ウィンドウに貼り付ける
                //    ret = IpWsPaste(ipRect.Top, ipRect.Left)
                //    'アクティブな画像を再描画する
                //    ret = IpAppUpdateDoc(DOCSEL_ACTIVE)
                //
                //    WordImage(0) = tmpWordImage(0)
                //
                //End If                      'v17.00追加(ここまで) byやまおか 2010/03/04
                //
                //PkeFPDの場合、額縁をつけずに切り出すことにする 'v17.53変更(ここから) byやまおか 2011/05/12
                //v17.22変更 byやまおか 2010/10/19
                //if ((modGlobal.DetType == modGlobal.DetectorConstants.DetTypePke) & (!Use_FpdAllpix)) {
                if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) & (!CTSettings.detectorParam.Use_FpdAllpix))
                {

                    tmpWordImage = new byte[(_with17.SizeX - modX) * (_with17.SizeY - modY)];
                    //_with17.GetByteImage(tmpWordImage);
                    tmpWordImage = transImageCtrl.GetByteImage();

                    //一回り小さくする

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*
				    ipRect.Left_Renamed = modX / 2 - 1;
				    ipRect.Top = modY / 2 - 1;
				    ipRect.Right_Renamed = _with17.SizeX - modX / 2 - 1;
				    ipRect.Bottom = _with17.SizeY - modY / 2 - 1;
				    ret = IpAoiCreateBox(ipRect);
				    ret = IpWsCopy();
				    //新規画像ウィンドウ作成
				    if (IpWsCreate(_with17.SizeX - modX, _with17.SizeY - modY, 300, IMC_GRAY16) < 0)
					    return;
				    //v17.10修正 byやまおか 2010/08/31
				    //新規画像ウィンドウに貼り付ける
				    ret = IpWsPaste(0, 0);
				    //アクティブな画像を再描画する
				    ret = IpAppUpdateDoc(DOCSEL_ACTIVE);
                    */

                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    ret = CallImageProFunction.CallTrimImage(_with17.SizeY, _with17.SizeX, modX, modY, 1);
                    if (ret < 0) return;
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

                    //これは不要
                    //画像をコピーする(サイズが違う)
                    //WordImage[0] = tmpWordImage[0];

                }
                //v17.53追加(ここまで) byやまおか 2011/05/12

            }
            else
            {
                //BYTE画像を取得（配列に格納）
                ByteImage = new byte[_with17.SizeX * _with17.SizeY];
                //_with17.GetByteImage(ByteImage);
                ByteImage = transImageCtrl.GetByteImage();


                //白黒反転して描画している場合   '追加 2009/08/21
                //if (_with17.RasterOp == vbNotSrcCopy)
                if (transImageCtrl.RasterOp != 0)
                    //ReverseByteImage(ByteImage[0], Information.UBound(ByteImage) + 1);
                    //ImgProc.ReverseByteImage(ref ByteImage[0], ByteImage.GetUpperBound(0) + 1);
                    ImgProc.ReverseByteImage(ref ByteImage[0], ByteImage.Length);

                //イメージプロの新規ウィンドウ作成し，画像データ（配列）を書込む
                CTSettings.IPOBJ.DrawByteImage(ByteImage, 0, 0, _with17.SizeX, _with17.SizeY, true);

                //'PkeFPDの場合、額縁をつける 'v17.00追加(ここから) byやまおか 2010/03/04
                //'If (DetType = DetTypePke) Then
                //If (DetType = DetTypePke) And (Not Use_FpdAllpix) Then  'v17.22変更 byやまおか 2010/10/19
                //
                //    Dim tmpByteImage()  As Byte
                //    ReDim tmpByteImage(.SizeX * .SizeY - 1)
                //    .GetByteImage tmpByteImage
                //
                //    '一回り小さくする
                //    ipRect.Left = (.SizeX Mod 100) / 2 - 1
                //    ipRect.Top = (.SizeY Mod 100) / 2 - 1
                //    ipRect.Right = .SizeX - (.SizeX Mod 100) / 2 - 1
                //    ipRect.Bottom = .SizeY - (.SizeY Mod 100) / 2 - 1
                //    ret = IpAoiCreateBox(ipRect)
                //    ret = IpWsCopy()
                //    '新規画像ウィンドウ作成
                //    If IpWsCreate(.SizeX, .SizeY, 300, IMC_GRAY) < 0 Then Exit Sub
                //    '新規画像ウィンドウを黒く塗りつぶす
                //    ret = IpWsFill(0, 3, 0)
                //    '新規画像ウィンドウに貼り付ける
                //    ret = IpWsPaste(ipRect.Top, ipRect.Left)
                //    'アクティブな画像を再描画する
                //    ret = IpAppUpdateDoc(DOCSEL_ACTIVE)
                //
                //    ByteImage(0) = tmpByteImage(0)
                //
                //End If                      'v17.00追加(ここまで) byやまおか 2010/03/04
                //
                //PkeFPDの場合、額縁をつけずに切り出すことにする 'v17.53変更(ここから) byやまおか 2011/05/12
                //v17.22変更 byやまおか 2010/10/19

                //if ((modGlobal.DetType == modGlobal.DetectorConstants.DetTypePke) & (!Use_FpdAllpix)) {
                if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) & (!CTSettings.detectorParam.Use_FpdAllpix))
                {

                    tmpByteImage = new byte[(_with17.SizeX - modX) * (_with17.SizeY - modY)];
                    //_with17.GetByteImage(tmpByteImage);
                    tmpByteImage = transImageCtrl.GetByteImage();

                    //一回り小さくする
                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*
				    ipRect.Left_Renamed = modX / 2 - 1;
				    ipRect.Top = modY / 2 - 1;
				    ipRect.Right_Renamed = _with17.SizeX - modX / 2 - 1;
				    ipRect.Bottom = _with17.SizeY - modY / 2 - 1;
				    ret = IpAoiCreateBox(ipRect);
				    ret = IpWsCopy();
				    //新規画像ウィンドウ作成
				    if (IpWsCreate(_with17.SizeX - modX, _with17.SizeY - modY, 300, IMC_GRAY) < 0)
					    return;
				    //新規画像ウィンドウに貼り付ける
				    ret = IpWsPaste(0, 0);
				    //アクティブな画像を再描画する
				    ret = IpAppUpdateDoc(DOCSEL_ACTIVE);
                    */
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    ret = CallImageProFunction.CallTrimImage(_with17.SizeY, _with17.SizeX, modX, modY, 0);
                    if (ret < 0) return;
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

                    //これは不要
                    //画像をコピーする(サイズが違う)
                    //ByteImage[0] = tmpByteImage[0];

                }
                //v17.53追加(ここまで) byやまおか 2011/05/12

            }

            //ドキュメントのサイズを変更する
            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            /*
            //ret = IpWsScale(.Width, .Height, 1)
            ret = IpWsScale(imgWidth, imgHeight, 1);		    //v17.53変更 byやまおか 2011/05/12
            */
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            ret = CallImageProFunction.CallIpWsScale(imgHeight, imgWidth);
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


            //付帯情報を保存する場合
            if (SaveInfomation)
            {

                //画像を配列に入れる
                if (IsTif)
                {
                    //IPOBJ.GetWordImage WordImage(), , , .Width, .Height
                    CTSettings.IPOBJ.GetWordImage(ref WordImage, 0, 0, imgWidth, imgHeight);
                    //v17.53変更 byやまおか 2011/05/12
                }
                else
                {
                    //IPOBJ.GetByteImage ByteImage(), , , .Width, .Height
                    CTSettings.IPOBJ.GetByteImage(ref ByteImage, 0, 0, imgWidth, imgHeight);
                    //v17.53変更 byやまおか 2011/05/12
                }

                #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                /*
                //ret = IpWsCreate(.Width + frmTransImageInfo.ScaleWidth, .Height, 300, IMC_GRAY)
                ret = IpWsCreate(imgWidth + frmTransImageInfo.Instance.ClientRectangle.Width, imgHeight, 300, IMC_GRAY);    //v17.53変更 byやまおか 2011/05/12
                ret = IpWsFill(0, 3, 0);    //新規の画像ウィンドウを黒く塗りつぶす
                */
                //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                ret = CallImageProFunction.CallIpWsCreate(imgHeight, imgWidth + frmTransImageInfo.Instance.ClientRectangle.Width);
                #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//



                //透視画像の付帯情報からデータを読み取り，イメージプロに書き込む
                var _with18 = frmTransImageInfo.Instance;

                //BYTE画像を取得（配列に格納）
                ByteImage2 = new byte[_with18.ClientRectangle.Width * _with18.ClientRectangle.Height];
                _with18.GetImage(ref ByteImage2);

                //画像データをイメージプロの画像に書込む
                //IPOBJ.DrawByteImage ByteImage2(), ctlTransImage.Width, , .ScaleWidth, .ScaleHeight
                CTSettings.IPOBJ.DrawByteImage(ByteImage2, imgWidth, 0, _with18.ClientRectangle.Width, _with18.ClientRectangle.Height); //v17.53変更 byやまおか 2011/05/12


                //透視画像データをImage-Proに書き込む
                if (IsTif)
                {

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*
                    ret = IpWsConvertImage(IMC_GRAY16, CONV_SCALE, 0, 0, 0, 0);
                    */
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    ret = CallImageProFunction.CallIpWsConvertImage(1);
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

                    //IPOBJ.DrawWordImage WordImage(), , , .Width, .Height
                    CTSettings.IPOBJ.DrawWordImage(WordImage, 0, 0, imgWidth, imgHeight);   //v17.53変更 byやまおか 2011/05/12
                }
                else
                {
                    //IPOBJ.DrawByteImage ByteImage(), , , .Width, .Height
                    CTSettings.IPOBJ.DrawByteImage(ByteImage, 0, 0, imgWidth, imgHeight);   //v17.53変更 byやまおか 2011/05/12
                }

            }

            //ファイルに保存
            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            /*
            ret = IpWsSaveAs(FileName, Path.GetExtension(FileName));
            */
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            ret = CallImageProFunction.CallIpWsSaveAs(FileName, Extension);
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

            //キャプションにファイル名をセット
            Target = FileName;

        }

        //*******************************************************************************
        //機　　能： キャプチャ用ドライバのオープン
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        public void CaptureOpen()
        {

//#if __DebugOn
#if NoCamera //'v18.00変更 byやまおか 2011/02/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            return;
#else

            int ret = 0;

            //v17.00削除 byやまおか 2010/01/19
            //If hMil = NullAddress Then
            //    hMil = MilOpen(dcf(0), IMAGE_BIT16, M_ASYNCHRONOUS)    '非同期でオープン
            //    hMil = MilOpen(dcf(0), IMAGE_BIT16, M_SYNCHRONOUS)      '同期でオープン
            //    If hMil = NullAddress Then
            //        MsgBox "Milをオープンできませんでした。", vbCritical
            //    End If
            //End If

            //int ret = 0;

            ScanCorrect.OFFSET_IMAGE = new double[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
            ScanCorrect.Gain_Image_L = new uint[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
            //追加2014/10/07hata_v19.51反映
            //ScanCorrect.Gain_Image_L_SFT = new uint[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];     //v18.00追加 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev23.20 左右シフト対応 by長野 2015/11/19
            ScanCorrect.Gain_Image_L_SFT_R = new uint[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
            ScanCorrect.Gain_Image_L_SFT_L = new uint[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
            //Rev26.00 add by chouno 2017/01/06
            ScanCorrect.GAIN_AIR_IMAGE = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
            ScanCorrect.GAIN_AIR_IMAGE_SFT_L = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
            ScanCorrect.GAIN_AIR_IMAGE_SFT_R = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];

            CTSettings.scanParam.fpd_gain = CTSettings.scansel.Data.fpd_gain;
            CTSettings.scanParam.fpd_integ = CTSettings.scansel.Data.fpd_integ;

            switch (CTSettings.detectorParam.DetType)
            {
                //v17.00追加(ここから) byやまおか 2010/01/19

                case DetectorConstants.DetTypeII:
                case DetectorConstants.DetTypeHama:
                    if (Pulsar.hMil == IntPtr.Zero)
                    {
                        ////hMil = MilOpen(dcf(0), IMAGE_BIT16, M_ASYNCHRONOUS)    '非同期でオープン
                        //Pulsar.hMil = Pulsar.MilOpen(transImageCtrl.Detector.dcf[0], Pulsar.IMAGE_BIT16, Pulsar.M_SYNCHRONOUS);
                        ////同期でオープン
                        //if (Pulsar.hMil == IntPtr.Zero)

                        if (transImageCtrl.CaptureOpen() == CaptureResult.OpenErr)
                        {
                            //MsgBox "Milをオープンできませんでした。", vbCritical
                            //ストリングテーブル化 'v17.60 by長野 2011/5/22
                            //修正 by長野 2013/09/23 v19.19
                            //Interaction.MsgBox(GetResString(Convert.ToInt32(CT30K.My.Resources.str20094), CT30K.My.Resources.str20159), MsgBoxStyle.Critical);
                            MessageBox.Show(StringTable.GetResString(20094, CTResources.LoadResString(20159)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                            this.Close();                   //v17.02追加 byやまおか 2010/07/06
                            frmCTMenu.Instance.Close();     //v17.02追加 byやまおか 2010/07/06
                        }

                        Pulsar.hPke = IntPtr.Zero;          //v17.00追加 byやまおか 2010/02/24

                    }
                    break;

                case DetectorConstants.DetTypePke:
                    //changed by 山本 2009-09-12
                    if (Pulsar.hPke == IntPtr.Zero)
                    {

                        Pulsar.hMil = IntPtr.Zero;          //v17.00追加 byやまおか 2010/02/24

                        //変更2014/10/07hata_v19.51反映
                        //FPD電源をＯＮにする   'v17.10追加 byやまおか 2010/08/31
                        //v17.20 検出器切替用に条件を追加 by 長野 2010/09/06
                        if (CTSettings.SecondDetOn & mod2ndDetctor.IsDet2mode)
                        {
                            modSeqComm.SeqBitWrite("TVIIPowerOn", true);
                        }
                        else
                        {
                            modSeqComm.SeqBitWrite("IIPowerOn", true);
                        }

                        //画像取込みがエラーするため0.2秒待つ   'v17.10追加 byやまおか 2010/08/31
                        //PauseForDoEvents 0.2
                        //v17.20 1秒に変更 by 長野　2010-09-01
                        //modCT30K.PauseForDoEvents(1);
                        //変更2014/10/07hata_v19.51反映
                        //v19.17 2秒に変更 by長野　2013/09/12
                        modCT30K.PauseForDoEvents(2);

                        ////hPke = PkeOpen(DestImage(0), scansel.fpd_gain, scansel.fpd_integ, t20kinf.v_capture_type)   'v17.10変更 byやまおか 2010/08/25
                        //Pulsar.hPke = Pulsar.PkeOpen(CTSettings.scansel.Data.fpd_gain, CTSettings.scansel.Data.fpd_integ, CTSettings.t20kinf.Data.v_capture_type);    //v17.50変更 by 間々田 2010/12/22
                        //if (Pulsar.hPke == IntPtr.Zero)

                        //変更2014/10/07hata_v19.51反映
                        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                        //CaptureResult capRes = transImageCtrl.CaptureOpen();
                        CaptureResult capRes;
                        //if (Convert.ToBoolean(modDetShift.IsDetShiftPos == modDetShift.DetShiftConstants.DetShift_forward) |
                        //    Convert.ToBoolean(modDetShift.IsDetShiftPos == modDetShift.DetShiftConstants.DetShift_backward))
                        //{
                        //    //検出器シフトあり
                        //    capRes = transImageCtrl.CaptureOpen(ref ScanCorrect.OFFSET_IMAGE, ref ScanCorrect.Gain_Image_L_SFT, true);
                        //}
                        //else
                        //{
                        //    //検出器シフトなし
                        //    capRes = transImageCtrl.CaptureOpen(ref ScanCorrect.OFFSET_IMAGE, ref ScanCorrect.Gain_Image_L, false);
                        //}

                        //Rev23.20 左右シフト対応 by長野 2015/11/19
                        if (Convert.ToBoolean(modDetShift.IsDetShiftPos == modDetShift.DetShiftConstants.DetShift_forward)) //右シフト
                        {
                            //検出器シフトあり
                            capRes = transImageCtrl.CaptureOpen(ref ScanCorrect.OFFSET_IMAGE, ref ScanCorrect.Gain_Image_L_SFT_R, (int)modDetShift.DetShift);
                        }
                        else if(Convert.ToBoolean(modDetShift.IsDetShiftPos == modDetShift.DetShiftConstants.DetShift_backward)) //左シフト
                        {
                            capRes = transImageCtrl.CaptureOpen(ref ScanCorrect.OFFSET_IMAGE, ref ScanCorrect.Gain_Image_L_SFT_L, (int)modDetShift.DetShift);
                        }
                        else
                        {
                            //検出器シフトなし
                            capRes = transImageCtrl.CaptureOpen(ref ScanCorrect.OFFSET_IMAGE, ref ScanCorrect.Gain_Image_L, (int)modDetShift.DetShift);
                        }


                        //追加2014/10/07hata_v19.51反映
                        //v19.17 1秒待つ by長野　2013/09/12
                        modCT30K.PauseForDoEvents(1);

                        if (capRes == CaptureResult.OpenErr)
                        {
                            //MsgBox "Pkeをオープンできませんでした。", vbCritical
                            //ストリングテーブル化 'v17.60 by長野 2011/5/22
                            //修正 by長野 2013/09/23 v19.19
                            //Interaction.MsgBox(GetResString(Convert.ToInt32(CT30K.My.Resources.str20094), CT30K.My.Resources.str20160), MsgBoxStyle.Critical);
                            MessageBox.Show(StringTable.GetResString(20094, CTResources.LoadResString(20160)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                            this.Close();       //v17.02変更 byやまおか 2010/07/06
                            frmCTMenu.Instance.Close();  //v17.02変更 byやまおか 2010/07/06

                        }
                        else
                        {
                            //Pkeオープン成功したときだけ以下を行う  //v17.02追加 byやまおか 2010/07/06
                            // transImageCtrl.CaptureOpenで実施しているので判断だけ行う
                            //ret = Pulsar.PkeSetOffsetData(Pulsar.hPke, 0, ScanCorrect.OFFSET_IMAGE, 0);
                            //if (ret == 1)
                            if (capRes == CaptureResult.OffsetErr)
                            {
                                //MsgBox "オフセット校正データをセットできませんでした。", vbCritical
                                //Interaction.MsgBox(CT30K.My.Resources.str20004, MsgBoxStyle.Critical);  //v17.60 ストリングテーブル化 by長野 2011/05/25
                                MessageBox.Show(CTResources.LoadResString(20004), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            // transImageCtrl.CaptureOpenで実施しているので判断だけ行う
                            //ret = Pulsar.PkeSetGainData(Pulsar.hPke, 0, ScanCorrect.Gain_Image_L, 0);
                            //if (ret == 1)
                            if (capRes == CaptureResult.GainErr)
                            {
                                //MsgBox "ゲイン校正データをセットできませんでした。", vbCritical
                                //Interaction.MsgBox(CT30K.My.Resources.str20003, MsgBoxStyle.Critical);   //v17.60　ストリングテーブル化 by長野 2011/05/25
                                MessageBox.Show(CTResources.LoadResString(20003), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            // transImageCtrl.CaptureOpenで実施しているので判断だけ行う
                            //ret = Pulsar.PkeSetPixelMap(Pulsar.hPke, 1);
                            //if (ret == 1)
                            if (capRes == CaptureResult.PixelErr)
                            {
                                //MsgBox "欠陥マップをセットできませんでした。", vbCritical
                                //Interaction.MsgBox(CT30K.My.Resources.str20095, MsgBoxStyle.Critical);  //v17.60 ストリングテーブル化 by長野 2011/05/25
                                MessageBox.Show(CTResources.LoadResString(20095), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            //if ((ret == 1))
                            if ((capRes == CaptureResult.OffsetErr) || (capRes == CaptureResult.GainErr) || (capRes == CaptureResult.PixelErr))
                                frmCTMenu.Instance.Close();  //v17.10追加 byやまおか 2010/08/25

                            //追加2014/10/07hata_v19.51反映
                            ret = frmScanControl.Instance.SetFpdGainInteg(CTSettings.scansel.Data.fpd_gain, CTSettings.scansel.Data.fpd_integ);   //v18.00追加 byやまおか 2011/07/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

                        }
                        //v17.02上から移動 byやまおか 2010/07/06

                        //Rev22.00 透視画像・動画のコメント追加にMILを使用するため追加 by長野 2015/07/03
                        //ホストメモリをオープンする
                        if (Pulsar.hMil == IntPtr.Zero)
                        {
                            //frmTransImage.hMil = MilHostOpen(IMAGE_BIT8)
                            Pulsar.hMil = Pulsar.MilHostOpen(ctlTransImage.SizeX,ctlTransImage.SizeY);
                            //v17.50変更 by 間々田 2011/01/20
                        }

                        //追加2014/10/07hata_v19.51反映
                        //v19.17 1秒待つ by長野　2013/09/12
                        modCT30K.PauseForDoEvents(1);
                    }
                    break;

            }
            //v17.00追加(ここまで) byやまおか 2010/01/19

            //透視画像の表示の仕方（ミラーリングするか）を決定  'v17.50追加 by 間々田 2010/12/28 画像左右反転（Pke時）対応
            ctlTransImage.MirrorOn = transImageCtrl.Detector.IsLRInverse;
#endif

        }

        //*******************************************************************************
        //機　　能： キャプチャを開始する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void CaptureStart()
        {

            //キャプションをリセット
            Target = "";

            //I.I.（またはFPD）電源のチェック  条件追加 by 間々田 2003/11/06
            if (!modSeqComm.PowerSupplyOK())
            {
                //If Use_FlatPanel Then Exit Sub      'I.I.の場合、電源がＯＦＦでも次に進ませる by 間々田 2004/12/28
                if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypeHama))
                    return;			//v17.02変更 byやまおか 2010/07/06
            }

            //Ｘ線検出器がフラットパネルの場合   'V7.0 added by 間々田 03/09/25
            if (CTSettings.detectorParam.Use_FlatPanel)
            {

                if (!ScanCorrect.GetDefGainOffset(ref adv))
                {
                    return;
                }
                else
                {
                    //欠陥データとGainデータをtransImageCtrlへセットする
                    transImageCtrl.SetDefGain(ScanCorrect.Def_IMAGE, ScanCorrect.GAIN_IMAGE, adv);

                    //transImageCtrlでは何もしないので不要か？  2014/12/15
                    //追加2014/11/28hata_v19.51_dnet
                    //if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypeHama)
                    //{

                    //    if (Convert.ToBoolean(modDetShift.IsDetShiftPos == modDetShift.DetShiftConstants.DetShift_forward) |
                    //        Convert.ToBoolean(modDetShift.IsDetShiftPos == modDetShift.DetShiftConstants.DetShift_backward))
                    //    {
                    //        //欠陥データとGainデータをtransImageCtrlへセットする
                    //        transImageCtrl.SetPkeOffsetGain(ScanCorrect.OFFSET_IMAGE, ScanCorrect.Gain_Image_L_SFT);
                    //    }
                    //    else
                    //    {
                    //        //欠陥データとGainデータをtransImageCtrlへセットする
                    //        transImageCtrl.SetPkeOffsetGain(ScanCorrect.OFFSET_IMAGE, ScanCorrect.Gain_Image_L);
                    //    }
                    //}
                }
                //'FPDの場合、透視画像表示中はFPD電源をOFFできなくする    '削除 by 間々田 2009/07/21 I.I.電源ボタンはメカ詳細画面に移動
                //With frmCTMenu.Toolbar1.Buttons("I.I.Power")
                //    If .Image = "I.I.On" Then .Enabled = False
                //End With

            }

            //'イベントによる通知
            //RaiseEvent CaptureOnOffChanged(True)   'v16.20 下へ移動 byやまおか 2010/04/06

            //スキャン時・校正時以外はＸ線をオンにする
            if (!Convert.ToBoolean(modCTBusy.CTBusy & modCTBusy.CTMechaBusy))
            {

                //'Ｘ線停止要求クリア
                //UserStopClear
                //
                //'連続回転コーンビーム＋高速再構成の時は、RAMディスクのscanstopを使う v17.40 追加 by 長野
                //If smooth_rot_cone_flg = True Then
                //
                //    UserStopClear_rmdsk
                //
                //End If

                //停止要求フラグをクリアする             'v17.50上記の処理を関数化 by 間々田 2011/02/17
                modCT30K.CallUserStopClear();

                //v17.00条件追加 byやまおか 2010/02/02
                if (CTSettings.scaninh.Data.xray_remote == 0)
                {
                    ///''If (scaninh.xray_remote = 0) And Not IsExistForm(frmFInteg) Then   '画像積分の時はONしない 'v17.10変更(仮) byやまおか 2010/09/02

                    //Ｘ線オン命令
                    //TryXrayOn , , False, True
                    //オンできなかった場合はここで抜ける(キャプチャーオンしない)
                    if ((modXrayControl.TryXrayOn(null, null, false, true) != 0))
                        return;
                    //v16.20変更 byやまおか 2010/04/06
                }

            }

            //イベントによる通知
            if (CaptureOnOffChanged != null)
            {
                CaptureOnOffChanged(true);  		//v16.20 上から移動した byやまおか 2010/04/06
            }

#if NoCamera
#else
            //キャプチャ用ドライバのオープン
            CTSettings.scanParam.fpd_gain = CTSettings.scansel.Data.fpd_gain;
            CTSettings.scanParam.fpd_integ = CTSettings.scansel.Data.fpd_integ;
            //transImageCtrl.CaptureOpen();
            CaptureOpen();


            ////'空読み
            ////'MilCapture hMil, TransImage(0) 'v17.00削除 byやまおか 2010/01/19
            ////Select Case DetType 'v17.00追加(ここから) byやまおか 2010/01/19
            ////    Case DetTypeII, DetTypeHama
            ////        MilCapture hMil, TransImage(0)
            ////    Case DetTypePke
            ////        'PkeCapture hPke, DestImage(0), TransImage(0)      'changed by 山本 2009-09-12
            ////        'パーキンエルマーFPDの場合空読み不要 2010-01-25 山本
            ////        'PkeCaptureTransImage hPke, AddressOf MilCaptureCallback2, DestImage(0), TransImage(0)     'changed by 山本 2009-09-12
            ////        PkeCaptureSetup hPke, DestImage(0), TransImage(0)   'キャプチャ準備 'v17.02追加 byやまおか 2010/07/15
            ////End Select          'v17.00追加(ここまで) byやまおか 2010/01/19

            ////v17.50以下に変更 by 間々田 2010/12/22

            ////キャプチャ準備
            //Pulsar.CaptureSetup(Pulsar.hMil, Pulsar.hPke);

            ////キャプチャ開始（Milの場合、空読み）
            //Pulsar.CaptureSeqStart(Pulsar.hMil, Pulsar.hPke);

            transImageCtrl.CaptureStart();


#endif

            //階調変換による画像自動更新をオフにする
            //ctlTransImage.AutoUpdate = false;

            //描画オペレーションはノーマルコピー '追加 by 間々田 2009/08/21
            //ctlTransImage.RasterOp = vbSrcCopy;
            transImageCtrl.RasterOp = 0;

            //キャプチャを開始（タイマーを利用：キャプチャを停止するまで実行する）
            //tmrLive.Interval = 1000 / FR(0)
            //tmrLive.Interval = 10;

            //tmrLive.Interval = 40          '１秒間に25フレーム
            //tmrLive.Enabled = true;
            //    DoLiveLoop

            frmTransImageInfo.Instance.Clear();
            //v17.02追加 付帯情報をクリアする byやまおか 2010/07/16

        }

        //*******************************************************************************
        //機　　能： キャプチャを停止する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void CaptureStop()
        {
            //キャプチャ停止
            transImageCtrl.CaptureStop();

            //付帯情報に表示
            if (transImageCtrl.IntegOn)
            {
                frmTransImageInfo.Instance.MyUpdate(frmFInteg.Instance.IntegNum);
                //追加2014/10/07hata_v19.51反映
                //v19.51 キャプチャしたときの積分枚数を記憶 追加 by長野 2014/03/19
                CTSettings.scanParam.bakIntegNum = frmFInteg.Instance.IntegNum;
                //Rev20.00 X線を落とす by長野 2015/02/25
                modXrayControl.XrayOff();
            }
            else
            {
                frmTransImageInfo.Instance.MyUpdate(1);
                //追加2014/10/07hata_v19.51反映
                //v19.51 キャプチャしたときの積分枚数を記憶 追加 by長野 2014/03/19
                CTSettings.scanParam.bakIntegNum = 1;
            }

            //Rev20.00 TransImageContro.csから移動 by長野 2015/02/25
            transImageCtrl.IntegOn = false;

            //イベントによる通知
            if (CaptureOnOffChanged != null)
            {
                CaptureOnOffChanged(false);
            }

        }


        //*******************************************************************************
        //機　　能： 表示画像のバックアップ（「元に戻す」処理時用）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        public void Backup()
        {
            //配列TransImageからFImageOrgにコピーする
            //FImageOrg = new ushort[Information.UBound(TransImage) + 1];
            //FImageOrg = TransImage;
            FImageOrg = new ushort[transImageCtrl.ImageSize.Height * transImageCtrl.ImageSize.Width];
            FImageOrg = transImageCtrl.GetImage();
        }


        //*******************************************************************************
        //機　　能： フィルター処理を実行する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        public void IP_Filter(ref float[] filter_Renamed)
        {

            ushort[] mytransimage;
            mytransimage = new ushort[transImageCtrl.ImageSize.Height * transImageCtrl.ImageSize.Width];
            //mytransimage = transImageCtrl.GetImage();

            int Matrix = 0;
            //Matrix = System.Math.Sqrt(Information.UBound(filter_Renamed) - Information.LBound(filter_Renamed) + 1);
            int upper = filter_Renamed.GetUpperBound(0);
            int lower = filter_Renamed.GetLowerBound(0);
            Matrix = Convert.ToInt32(System.Math.Sqrt(upper - lower + 1));

            //空間フィルタ処理
            //SpatialFilter(FImageOrg[0], mytransimage[0], filter_Renamed[0], h_size, v_size, Matrix);
            ImgProc.SpatialFilter(FImageOrg, mytransimage, filter_Renamed, transImageCtrl.Detector.h_size, transImageCtrl.Detector.v_size, Matrix);

            //処理画像をTransImageにセットする
            transImageCtrl.SetTransImage(mytransimage);

            //「元に戻す」ボタンを使用可にする
            frmScanControl.Instance.cmdUndo.Enabled = true;

            //更新
            transImageCtrl.Update(false);
            //ctlTransImage.Invalidate();
            ctlTransImage.Refresh();

        }


        //*******************************************************************************
        //機　　能： 白黒反転処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        public void Inverse()
        {
            ////変更 by 間々田 2009/08/21
            ////var _with21 = ctlTransImage;
            //_with21.RasterOp = (_with21.RasterOp == vbSrcCopy ? vbNotSrcCopy : vbSrcCopy);
            //_with21.DispPicture();

            transImageCtrl.RasterOp = (transImageCtrl.RasterOp == 1 ? 0 : 1);
            //ctlTransImage.Invalidate();
            ctlTransImage.Refresh();

        }

        //*******************************************************************************
        //機　　能： 画像処理を元に戻す
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        public void Undo()
        {

            //配列FImageOrgからTransImageにコピーする
            //TransImage = VB6.CopyArray(FImageOrg);
            transImageCtrl.SetTransImage(FImageOrg);

            //更新
            //MyUpdate(false);
            transImageCtrl.Update(false);
            //ctlTransImage.Invalidate();
            ctlTransImage.Refresh();

            //「元に戻す」ボタンを使用不可にする
            frmScanControl.Instance.cmdUndo.Enabled = false;

        }


        #region フォームロード時の処理
        private void frmTransImage_Load(object sender, EventArgs e)
        {
            //積算中か
            //FluoroIP.IntegOn = false;
            transImageCtrl.IntegOn = false;

            //一度でもCaptureOnを実行したか(load直後はfalse) 'v19.50 by長野 2014/01/28
            FlgCaptureOnFirstDone = false;

            //ターゲットファイル
            myTarget = "";

            //拡大比率：モニタが１台の時は1/2とする
            myZoomScale = (Winapi.GetSystemMetrics(Winapi.SM_CMONITORS) < 2 ? 2 : 1);            //1:同倍 2:1/2

            //キャプキャ画像のデータサイズ
            myDataSize = CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size;

            modScanCorrectNew.TransImage = new ushort[myDataSize];
            WorkImage = new ushort[myDataSize];

            //-->v17.50削除 by 間々田 2011/01/14
            //'If DetType = DetTypePke Then ReDim DestImage(myDataSize - 1)    'v17.00追加　山本　2009-09-30
            //If (DetType = DetTypePke) Then  'v17.10変更 byやまおか 2010/07/28
            //    ReDim DestImage(myDataSize - 1)
            //Else
            //    ReDim DestImage(1)
            //End If
            //<--v17.50削除 by 間々田 2011/01/14


            ////コントロールの初期化
            ////ctlTransImage.InitControls();
            //InitControls();

            //透視画像表示サイズ：初期状態は標準サイズ                        'v17.4X/v18.00追加 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            TransImageDispSize = TransImageDispSizeConstants.MediumSize;

            ResizeImage();

            //シャープ用フィルタをセットする
            // 0,-1, 0
            //-1, 5,-1
            // 0,-1, 0
            SharpFilter = new float[9];
            SharpFilter[0] = 0;
            SharpFilter[1] = -1;
            SharpFilter[2] = 0;
            SharpFilter[3] = -1;
            SharpFilter[4] = 5;
            SharpFilter[5] = -1;
            SharpFilter[6] = 0;
            SharpFilter[7] = -1;
            SharpFilter[8] = 0;

#if ! TRANS_IMAGE_SERVER_USE

            //透視画像領域作成（透視画像サーバーを使用する場合は透視画像サーバー側で作成）           v17.50変更 by 間々田 2011/01/05
            PulsarHelper.hMap = PulsarHelper.CreateTransImageMap(transImageCtrl.Detector.h_size, transImageCtrl.Detector.v_size);

#endif

            //ミルオープン
            //CaptureOpen    'v17.40削除 byやまおか 2010/10/26

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //RAMディスクを使用する場合は、ここでオープンしない。    'v17.40追加 byやまおか 2010/10/26
            //    If Not UseRamDisk Then
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            CTSettings.scanParam.fpd_gain = CTSettings.scansel.Data.fpd_gain;
            CTSettings.scanParam.fpd_integ = CTSettings.scansel.Data.fpd_integ;
            //transImageCtrl.CaptureOpen();
            CaptureOpen();

#if !NoCamera
            //Rev22.00 追加 by長野 2015/08/18
            Pulsar.Mil9AddCommentInit(20, 128);
#endif
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //v17.20 確実にWWとWLを取得できるようにcmdWLWWReset_Clickを呼ぶ by 長野 10/09/19
            //frmScanControl.cmdWLWWReset_Click(null, new System.EventArgs());
            frmScanControl.Instance.cmdWLWWReset.PerformClick();

            //ctlTransImage.WindowLevel = frmScanControl.Instance.WindowLevel;
            //ctlTransImage.WindowWidth = frmScanControl.Instance.WindowWidth;
            transImageCtrl.WindowLevel = frmScanControl.Instance.WindowLevel;
            transImageCtrl.WindowWidth = frmScanControl.Instance.WindowWidth;

            mnuRoiInput.Text = StringTable.GetResString(StringTable.IDS_CoordinateInput, "ROI");            //ROI座標入力

            //フレームレート
            myFrameRate = 15;
            transImageCtrl.Detector.FrameRate = myFrameRate;

            //Rev25.00 透視プロファイル位置の初期値をセット by長野 2016/08/08
            myLProfHPos = CTSettings.iniValue.FImageLProfileHPos;
            myLProfVPos = CTSettings.iniValue.FImageLProfileVPos;

            //透視画面まで表示されたら立ち上がり完了とする
            modCT30K.CT30kNowStartingFlg = false;   //v17.53追加 byやまおか 2011/05/13

        }
        #endregion

        //*******************************************************************************
        //機　　能： フォームアンロード時処理（イベント処理）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Cancel          [ /O] Integer   True（0以外）: アンロードをキャンセル
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        private void frmTransImage_FormClosed(System.Object eventSender, System.Windows.Forms.FormClosedEventArgs eventArgs)
        {

            // ERROR: Not supported in C#: OnErrorStatement


            //透視画像取り込みオフ
            //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            //if (modLibrary.IsExistForm(frmScanControl.Instance)) {
            if (modLibrary.IsExistForm("frmScanControl"))
            {

                CaptureOn = false;

#if ! NoCamera //v17.00条件追加 byやまおか 2010/02/02
                //# else 
                //'MilCaptureStop     'v17.00削除 byやまおか 2010/01/19
                //Select Case DetType 'v17.00追加(ここから) byやまおか 2010/01/19
                //    Case DetTypeII, DetTypeHama
                //        MilCaptureStop
                //    Case DetTypePke
                //        PkeCaptureStop (Pulsar.hPke)       'changed by 山本　2009-09-16
                //End Select          'v17.00追加(ここまで) byやまおか 2010/01/19

                //キャプチャストップ v17.50変更 by 間々田 2011/01/05
                Pulsar.CaptureSeqStop(Pulsar.hMil, Pulsar.hPke);
#endif

            }

            //Rev23.10 追加 by長野 2015/11/05
            if (CaptureOn == true)
            {
                //Pulsar.CaptureSeqStop(Pulsar.hMil, Pulsar.hPke);
                CaptureStop();
            }

            frmSaveMovie.Instance.Close();

            //DestroyTransImageMap PulsarHelper.hMap

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //#if ! TRANS_IMAGE_SERVER_USE

            //                //透視画像領域破棄（透視画像サーバーを使用する場合は透視画像サーバー側で破棄）           v17.50変更 by 間々田 2011/01/05
            //            PulsarHelper.DestroyTransImageMap(PulsarHelper.hMap);

            //#endif
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //ミルクローズ
#if !NoCamera   //'v17.4X/v18.00変更 by 間々田 2011/03/15

            transImageCtrl.CaptureClose();
#endif
 
        }

        private void frmTransImage_Resize(object sender, EventArgs e)
        {

            //付帯情報の位置の調整
            var _with2 = frmTransImageInfo.Instance;
            if (_with2.WindowState == FormWindowState.Normal)
            {
                _with2.SetBounds(this.Left + this.Width, this.Top, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
                // Mod Start 2018/12/07 M.Oyama Windows10対応
                //frmTransImageControl.Instance.SetBounds(_with2.Left, _with2.Top + _with2.Height, _with2.Width, this.Height - _with2.Height);
                frmTransImageControl.Instance.SetBounds(_with2.Left, _with2.Top + _with2.Height, _with2.Width, this.Height - _with2.Height + 2);
                // Mod End 2018/12/07
            }

            //削除2014/10/07hata_v19.51反映
            ////ボタンのキャプションの設定（縮小/拡大）
            //frmScanControl.Instance.cmdFZoom.Text = CTResources.LoadResString((2 * this.Width < frmCTMenu.Instance.ClientRectangle.Width ? StringTable.IDS_btnEnlarge : StringTable.IDS_btnReduction));            //拡大/縮小

            //追加2014/10/07hata_v19.51反映
            //スクロールバー関連のコントロールの位置     'v17.4X/v18.00追加 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            hsbImage.SetBounds(0, ClientRectangle.Height - hsbImage.Height, ClientRectangle.Width - vsbImage.Width, 0, BoundsSpecified.X | BoundsSpecified.Y | BoundsSpecified.Width);
            hsbImage.BringToFront();
            vsbImage.SetBounds(ClientRectangle.Width - vsbImage.Width, 0, vsbImage.Width, ClientRectangle.Height - hsbImage.Height);
            vsbImage.BringToFront();
            fraSpace.SetBounds(vsbImage.Left, hsbImage.Top, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            fraSpace.BringToFront();

        }

        //private void ResizeImage()
        //Rev26.40 change by chouno 2019/02/12
        public void ResizeImage()
        {
            //仮想画面の左端の座標（ピクセル値）を取得
            int VirtualLeft = 0;
            VirtualLeft = Winapi.GetSystemMetrics(Winapi.SM_XVIRTUALSCREEN);

            //このモニタの幅と高さ（ピクセル値）を取得
            int ScreenWidth = 0;
            ScreenWidth = Winapi.GetSystemMetrics(Winapi.SM_CXSCREEN);
            int ScreenHeight = 0;
            ScreenHeight = Winapi.GetSystemMetrics(Winapi.SM_CYSCREEN);

            //int mod_SizeX = 0;            //v17.00追加 byやまおか 2010/02/19
            //int mod_SizeY = 0;            //v17.00追加 byやまおか 2010/02/19

            //透視画像表示コントロール
            int theLeft = 0;
            int theTop = 0;
            float hError = 0;                   //追加2014/10/07hata_v19.51反映
            float vError = 0;                   //追加2014/10/07hata_v19.51反映
            float AvailableControlWidth = 0;    //追加2014/10/07hata_v19.51反映
            float AvailableControlHeight = 0;   //追加2014/10/07hata_v19.51反映
            float AvailableDispWidth = 0;       //追加2014/10/07hata_v19.51反映
            float AvailableDispHeight = 0;      //追加2014/10/07hata_v19.51反映
            bool hVisible = false;              //追加2014/10/07hata_v19.51反映
            bool vVisible = false;              //追加2014/10/07hata_v19.51反映
            var _with3 = ctlTransImage;

            //変更2014/10/07hata_v19.51反映
            //-->v17.4X/v18.00削除 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //_with3.Width = Convert.ToInt32(_with3.SizeX / transImageCtrl.Detector.fphm / myZoomScale);
            //_with3.Height = Convert.ToInt32(_with3.SizeY / transImageCtrl.Detector.fpvm / myZoomScale);

            ////フォーム
            //this.Width = _with3.Width + 6;
            //this.Height = _with3.Height + 25;

            ////v17.10修正(ここから) byやまおか 2010/07/28
            ////PkeFPDの場合、周りを表示しない(TransImageを画像からはみ出させる)
            ////If (DetType = DetTypePke) Then
            ////v17.22変更 byやまおか 2010/10/19
            //if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) & (!transImageCtrl.Detector.Use_FpdAllpix))
            //{

            //    //画像サイズの端数(2048なら48)
            //    mod_SizeX = Convert.ToInt32((_with3.SizeX % 100) / transImageCtrl.Detector.fphm);
            //    mod_SizeY = Convert.ToInt32((_with3.SizeY % 100) / transImageCtrl.Detector.fpvm);

            //    //フォームサイズを一回り小さくする
            //    this.Width = this.Width -  (mod_SizeX / myZoomScale);
            //    this.Height = this.Height - (mod_SizeY / myZoomScale);

            //    //表示位置をはみ出させる(原点をマイナス値にする)
            //    _with3.Left = Convert.ToInt32(-mod_SizeX / 2 / myZoomScale);
            //    _with3.Top = Convert.ToInt32(-mod_SizeY / 2 / myZoomScale);

            //}
            ////v17.00追加(ここまで) byやまおか 2010/02/19
            //<--v17.4X/v18.00削除 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //-->v17.4X/v18.00追加 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //透視画像コントロールの大きさ
            //2014/11/07hata キャストの修正
            //_with3.Width = Convert.ToInt32(_with3.SizeX / modCT30K.hScale / myZoomScale);
            //_with3.Height = Convert.ToInt32(_with3.SizeY / modCT30K.vScale / myZoomScale);
            _with3.Width = Convert.ToInt32((float)_with3.SizeX / modCT30K.hScale / (float)myZoomScale);
            _with3.Height = Convert.ToInt32((float)_with3.SizeY / modCT30K.vScale / (float)myZoomScale);

            //はみでる領域

            //PkeFPDの場合、周りを表示しない(TransImageを画像からはみ出させる)
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) & (!transImageCtrl.Detector.Use_FpdAllpix))
            {
                //画像サイズの端数(2048なら48)
                //2014/11/07hata キャストの修正
                //hError = (_with3.SizeX % 100) / modCT30K.hScale / myZoomScale;
                //vError = (_with3.SizeY % 100) / modCT30K.vScale / myZoomScale;
                hError = (_with3.SizeX % 100) / modCT30K.hScale / (float)myZoomScale;
                vError = (_with3.SizeY % 100) / modCT30K.vScale / (float)myZoomScale;
            }
            else
            {
                //普通は０
                hError = 0;
                vError = 0;
            }

            //有効なコントロール領域
            AvailableControlWidth = _with3.Width - hError;
            AvailableControlHeight = _with3.Height - vError;

            //有効なコントロール表示領域
            //v19.50 I.I.の場合は周辺画素も表示する by長野 2013/11/26
            //        AvailableDispWidth = (.SizeX - (.SizeX Mod 100)) / fphm / myZoomScale
            //        AvailableDispHeight = (.SizeY - (.SizeY Mod 100)) / fpvm / myZoomScale
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) & (!transImageCtrl.Detector.Use_FpdAllpix))
            {
                //2014/11/07hata キャストの修正
                //AvailableDispWidth = (_with3.SizeX - (_with3.SizeX % 100)) / transImageCtrl.Detector.fphm / myZoomScale;
                //AvailableDispHeight = (_with3.SizeY - (_with3.SizeY % 100)) / transImageCtrl.Detector.fpvm / myZoomScale;
                AvailableDispWidth = (_with3.SizeX - (_with3.SizeX % 100)) / transImageCtrl.Detector.fphm / (float)myZoomScale;
                AvailableDispHeight = (_with3.SizeY - (_with3.SizeY % 100)) / transImageCtrl.Detector.fpvm / (float)myZoomScale;

            }
            else
            {
                //2014/11/07hata キャストの修正
                //AvailableDispWidth = _with3.SizeX / transImageCtrl.Detector.fphm / myZoomScale;
                //AvailableDispHeight = _with3.SizeY / transImageCtrl.Detector.fpvm / myZoomScale;
                AvailableDispWidth = _with3.SizeX / transImageCtrl.Detector.fphm / (float)myZoomScale;
                AvailableDispHeight = _with3.SizeY / transImageCtrl.Detector.fpvm / (float)myZoomScale;
            }
            //フォーム
            //変更2014/11/28hata_v19.51_dnet
            //this.Width = Convert.ToInt32(AvailableDispWidth + 6);
            //this.Height = Convert.ToInt32(AvailableDispHeight + 25);
            int WinBoderWidth = this.Width - this.ClientSize.Width;
            int WinBoderHeight = this.Height - this.ClientSize.Height;
            this.Width = Convert.ToInt32(AvailableDispWidth + WinBoderWidth);
            this.Height = Convert.ToInt32(AvailableDispHeight + WinBoderHeight);

            //透視画像コントロール表示位置を制御するスクロールバーの設定：表示設定
            hVisible = AvailableControlWidth > AvailableDispWidth;
            vVisible = AvailableControlHeight > AvailableDispHeight;
            hsbImage.Visible = hVisible;
            vsbImage.Visible = vVisible;
            fraSpace.Visible = hVisible & vVisible;

            //透視画像コントロール表示位置を制御するスクロールバーの設定：最大値・最小値
            if (hVisible)
            {
                hsbImage.Minimum = Convert.ToInt32(hError / 2);
                hsbImage.Maximum = Convert.ToInt32(hsbImage.Minimum + AvailableControlWidth - hsbImage.Width + hsbImage.LargeChange - 1);
                _with3.Left = -hsbImage.Value;
            }
            else
            {
                _with3.Left = Convert.ToInt32(-hError / 2);
            }

            if (vVisible)
            {
                vsbImage.Minimum = Convert.ToInt32(vError / 2);
                vsbImage.Maximum = Convert.ToInt32(vsbImage.Minimum + AvailableControlHeight - vsbImage.Height + vsbImage.LargeChange - 1);
                _with3.Top = -vsbImage.Value;
            }
            else
            {
                _with3.Top = Convert.ToInt32(-vError / 2);
            }
            //<--v17.4X/v18.00追加 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05


            //モニタの数が複数ではない
            if (Winapi.GetSystemMetrics(Winapi.SM_CMONITORS) < 2)
            {
                //theLeft = ScreenWidth - (.width + 6) - 5
                //変更2014/10/07hata_v19.51反映
                //v17.4X/v18.00変更 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                //theLeft = ScreenWidth - (_with3.Width + 6) - (frmTransImageControl.Instance.ClientRectangle.Width + 6);
                //theTop = ScreenHeight - (_with3.Height + 25) - 95;
                theLeft = Convert.ToInt32(ScreenWidth - (AvailableDispWidth + 6) - (frmTransImageControl.Instance.ClientRectangle.Width + 6));
                theTop = Convert.ToInt32(ScreenHeight - (AvailableDispHeight + 25) - 95);

                //モニタが複数：透視画像フォームは２ndモニターに移動
                //仮想画面の左端の座標が負の場合
            }
            else if (VirtualLeft < 0)
            {
                //theLeft = VirtualLeft
                theLeft = -(this.Width + frmTransImageControl.Instance.Width);  //左画面に右寄せ 'v17.10変更 byやまおか 2010/08/25
                theTop = 0;
            }
            else
            {
                theLeft = ScreenWidth;
                theTop = 0;
            }

            //移動
            // Mod Start 2018/10/29 M.Oyama V26.40 Windows10対応
            //this.SetBounds(theLeft, theTop, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            //frmTransImageInfo.Instance.SetBounds(this.Left + this.Width, this.Top, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);            //v17.10追加 byやまおか 2010/08/25
            //frmTransImageControl.Instance.SetBounds(this.Left + this.Width, this.Top + frmTransImageInfo.Instance.Height, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);         //v17.10追加 byやまおか 2010/08/
            this.SetBounds(theLeft + 10, theTop, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            frmTransImageInfo.Instance.SetBounds(this.Left + this.Width - 7, this.Top, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);            //v17.10追加 byやまおか 2010/08/25
            frmTransImageControl.Instance.SetBounds(this.Left + this.Width - 7, this.Top + frmTransImageInfo.Instance.Height - 2, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);         //v17.10追加 byやまおか 2010/08/25
            int Height = frmTransImageControl.Instance.Height;
            
            
            // Mod End 2018/10/29

            //Rev25.00 追加 by長野 2016/08/08 --->
            //プロファイルの追加
            SetTransProfile();
            //<---

            //線の描画
            //ctlTransImage.SetLine();
            SetLine();

            //Rev25.00 追加 by長野 2016/08/08 --->
            //文字列の設定
            SetTransString();
            //<---
        }


        //*******************************************************************************
        //機　　能： フレームレート(計算値)を求める関数
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： ret             [ /O] Single    フレームレート
        //
        //補　　足：
        //
        //履　　歴： V17.02  10/07/22    やまおか    新規作成
        //*******************************************************************************
        public float GetCurrentFR()
        {
            float functionReturnValue = 0;

            switch (CTSettings.detectorParam.DetType)
            {
                case DetectorConstants.DetTypeII:
                case DetectorConstants.DetTypeHama:
                    functionReturnValue = CTSettings.detectorParam.FR[0];
                    break;
                case DetectorConstants.DetTypePke:
                    //GetCurrentFR = FR(0) * CSng(GetFpdIntegStr(0)) / CSng(GetFpdIntegStr(scansel.fpd_integ))
                    //v17.10変更 byやまおか 2010/08/21
                    functionReturnValue = CTSettings.detectorParam.FR[0] * Convert.ToSingle(modCT30K.GetFpdIntegDouble(0)) / Convert.ToSingle(modCT30K.GetFpdIntegDouble(CTSettings.scansel.Data.fpd_integ));
                    break;
            }
            return functionReturnValue;

        }

        //*************************************************************************************************
        //機　　能： 座標が指定された直線上に存在するか
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 直線が左右反転表示されている場合に対応
        //
        //履　　歴： v17.50  2011/02/01 (電S1)間々田 新規作成
        //*************************************************************************************************
        private bool PointOnLineMirror(short px, short py, short x1, short y1, short x2, short y2)
        {
            bool functionReturnValue = false;

            //左右反転表示時は直線のＹ座標を入れ替える
            if (transImageCtrl.MirrorOn)
            {
                functionReturnValue = Figure.PointOnLine(px, py, x1, y2, x2, y1);
            }
            else
            {
                functionReturnValue = Figure.PointOnLine(px, py, x1, y1, x2, y2);
            }
            return functionReturnValue;

        }


        ////*******************************************************************************
        ////機　　能： コントロールの初期化
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        ////*******************************************************************************
        //private void InitControls()
        //{

        //    //ターゲットプロパティリセット
        //    Target = "";

        //    var _with4 = ctlTransImage;
        //    _with4.SizeX = transImageCtrl.Detector.h_size;
        //    _with4.SizeY = transImageCtrl.Detector.v_size;

        //    //スキャン位置の線
        //    var _with5 = myLine[(int)LineConstants.ScanLine];
        //    _with5.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Lime);
        //    _with5.Visible = true;

        //    //スライス厚の上側の線
        //    var _with6 = myLine[(int)LineConstants.UpperLine];
        //    _with6.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
        //    _with6.Visible = true;

        //    //スライス厚の下側の線
        //    var _with7 = myLine[(int)LineConstants.LowerLine];
        //    _with7.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
        //    _with7.Visible = true;

        //    //中心線（縦）
        //    var _with8 = myLine[(int)LineConstants.CenterLine];
        //    _with8.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Cyan);
        //    _with8.Visible = true;

        //    //中心線（横）   'v15.10追加 byやまおか 2009/10/22
        //    var _with9 = myLine[(int)LineConstants.CenterLineH];
        //    _with9.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Cyan);
        //    _with9.Visible = false;

        //}

        //*******************************************************************************
        //機　　能： 描画用プロファイルデータの作成
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        public void SetTransProfile()
        {
            //Rev25.13 修正 by chouno 2017/11/17
            float mabiki_v = 0;
            float mabiki_h = 0;

            int magnify = 0;
            //v17.4X/v18.00以下に変更 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev25.03/Rev25.02 2ndだけ4K対応 add by chouno 2017/02/07
            if (monitor4KOn == 1)
            {
                switch (TransImageDispSize)
                {
                    case frmTransImage.TransImageDispSizeConstants.SmallSize:
                        magnify = 2;
                        break;
                    case frmTransImage.TransImageDispSizeConstants.MediumSize:
                        magnify = 1;
                        break;
                    case frmTransImage.TransImageDispSizeConstants.LargeSize:
                        magnify = 1;
                        break;
                }
            }
            else
            {
                switch (TransImageDispSize)
                {
                    case frmTransImage.TransImageDispSizeConstants.SmallSize:
                        magnify = (CTSettings.detectorParam.h_size == 2048 && CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) ? 4 : 2;
                        break;
                    case frmTransImage.TransImageDispSizeConstants.MediumSize:
                        magnify = (CTSettings.detectorParam.h_size == 2048 && CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) ? 2 : 1;
                        break;
                    case frmTransImage.TransImageDispSizeConstants.LargeSize:
                        magnify = 1;
                        break;
                }
            }

            //透視画像拡大・縮小に対応
            //mabiki_v = (int)(CTSettings.detectorParam.v_size / ctlTransImage.Height);
            //mabiki_h = (int)(CTSettings.detectorParam.h_size / ctlTransImage.Width);
            //Rev25.13 修正 by chouno 2017/11/17
            mabiki_v = (float)((float)CTSettings.detectorParam.v_size / (float)ctlTransImage.Height);
            mabiki_h = (float)((float)CTSettings.detectorParam.h_size / (float)ctlTransImage.Width);

            if (old_mabiki_h != mabiki_h || old_mabiki_v != mabiki_v)
            {
                myLProfHPos = (int)((float)myLProfHPos / ((float)mabiki_h / (float)Math.Abs(old_mabiki_h)));
                myLProfVPos = (int)((float)myLProfVPos / ((float)mabiki_v / (float)Math.Abs(old_mabiki_v)));
                ctlTransImage.SetLinePointSize(LineConstants.ProfileV, ctlTransImage.Height);
                ctlTransImage.SetLinePointSize(LineConstants.ProfileH, ctlTransImage.Width);
            }

            old_mabiki_h = mabiki_h;
            old_mabiki_v = mabiki_v;

            //取得したいプロファイル位置を設定
            switch (CTSettings.detectorParam.DetType)
            {
                case (DetectorConstants.DetTypePke):
                case (DetectorConstants.DetTypeHama):
                    transImageCtrl.LProfileHPos = myLProfHPos * magnify;
                    transImageCtrl.LProfileVPos = CTSettings.detectorParam.h_size - (myLProfVPos * magnify);
                    break;
                default:
                    transImageCtrl.LProfileHPos = myLProfHPos * magnify;
                    transImageCtrl.LProfileVPos = myLProfVPos * magnify;
                    break;
            }

            //キャプチャoffかつスキャン中でなければ、ここでプロファイルデータ更新
            if (transImageCtrl.CaptureOn == false && !(Convert.ToBoolean(modCTBusy.CTBusy & modCTBusy.CTScanStart)))
            {
                transImageCtrl.UpdateProfile();
            }

            //プロファイル取得
            transImageCtrl.GetLProfile(ref myLProfileH, ref myLProfileV);

            //ctlTransImage.SetLineProfH(myLProfileH, mabiki_h,myLProfHPos,(CTSettings.detectorParam.DetType == DetectorConstants.DetTypeII? 4096:65536),CTSettings.iniValue.FImageLProfileH100Pos,CTSettings.iniValue.FImageLProfileH0Pos,ctlTransImage.Height,ctlTransImage.Width);
            //ctlTransImage.SetLineProfV(myLProfileV, mabiki_v,myLProfVPos,(CTSettings.detectorParam.DetType == DetectorConstants.DetTypeII? 4096:65536),CTSettings.iniValue.FImageLProfileV100Pos,CTSettings.iniValue.FImageLProfileV0Pos,ctlTransImage.Height,ctlTransImage.Width);
            //Rev25.03/Rev25.02 change by chouno 2017/02/09 
            ctlTransImage.SetLineProfH(myLProfileH, mabiki_h, myLProfHPos, (CTSettings.detectorParam.DetType == DetectorConstants.DetTypeII ? 4096 : 65536), H100Pos, H0Pos, ctlTransImage.Height, ctlTransImage.Width);
            ctlTransImage.SetLineProfV(myLProfileV, mabiki_v, myLProfVPos, (CTSettings.detectorParam.DetType == DetectorConstants.DetTypeII ? 4096 : 65536), V100Pos, V0Pos, ctlTransImage.Height, ctlTransImage.Width);
        }

        //*******************************************************************************
        //機　　能： 透視画像への文字列描画
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        public void SetTransString()
        {
            bool bDisp = false;
            PointF p1 = default(PointF);
            
            int zeroStringPosVX = 0;
            int zeroStringPosVY = 0;
            int zeroStringPosHX = 0;
            int zeroStringPosHY = 0;

            int magnify = 1;//Rev25.02/25.02 add by chouno 2017/02/09

            //v17.4X/v18.00以下に変更 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev25.03/Rev25.02 2ndだけ4K対応 by chouno 2017/02/09
            if (monitor4KOn == 0)
            {
                switch (TransImageDispSize)
                {
                    case frmTransImage.TransImageDispSizeConstants.SmallSize:
                        zeroStringPosVX = 0;
                        zeroStringPosVY = 5;
                        zeroStringPosHX = 5;
                        zeroStringPosHY = 10;
                        break;
                    case frmTransImage.TransImageDispSizeConstants.MediumSize:
                        zeroStringPosVX = 5;
                        zeroStringPosVY = 10;
                        zeroStringPosHX = 10;
                        zeroStringPosHY = 15;
                        break;
                    case frmTransImage.TransImageDispSizeConstants.LargeSize:
                        zeroStringPosVX = 10;
                        zeroStringPosVY = 10;
                        zeroStringPosHX = 5;
                        zeroStringPosHY = 5;
                        break;
                }
            }
            else
            {
                switch (TransImageDispSize)
                {
                    case frmTransImage.TransImageDispSizeConstants.SmallSize:
                        zeroStringPosVX = 5;
                        zeroStringPosVY = 10;
                        zeroStringPosHX = 10;
                        zeroStringPosHY = 20;
                        magnify = 1;
                        break;
                    case frmTransImage.TransImageDispSizeConstants.MediumSize:
                        zeroStringPosVX = 15;
                        zeroStringPosVY = 25;
                        zeroStringPosHX = 25;
                        zeroStringPosHY = 35;
                        magnify = 2;
                        break;
                    case frmTransImage.TransImageDispSizeConstants.LargeSize:
                        zeroStringPosVX = 10;
                        zeroStringPosVY = 10;
                        zeroStringPosHX = 5;
                        zeroStringPosHY = 5;
                        break;
                }
            }

            //Rev25.02 add by chouno 2017/02/09
            V0Pos = CTSettings.iniValue.FImageLProfileV0Pos;
            V100Pos = CTSettings.iniValue.FImageLProfileV100Pos * magnify;
            H0Pos = CTSettings.iniValue.FImageLProfileH0Pos;
            H100Pos = CTSettings.iniValue.FImageLProfileH100Pos * magnify;

            //水平0%
            p1.X = zeroStringPosHX;
            //p1.Y = ctlTransImage.Height - CTSettings.iniValue.FImageLProfileH0Pos - zeroStringPosHY;
            p1.Y = ctlTransImage.Height - H0Pos - zeroStringPosHY;//Rev25.03/Rev25.02 change by chouno 2017/02/09
            bDisp = frmScanControl.Instance.chkLineProfile.Checked;
            ctlTransImage.SetStringCaption(StringConstants.Profile0PosH, "0");
            ctlTransImage.SetStringPoint(StringConstants.Profile0PosH, ref p1, bDisp);

            //水平100%
            p1.X = zeroStringPosHX;
            //p1.Y = ctlTransImage.Height - CTSettings.iniValue.FImageLProfileH100Pos;
            p1.Y = ctlTransImage.Height - H100Pos;//Rev25.03/Rev25.02 change by chouno 2017/02/09
            bDisp = frmScanControl.Instance.chkLineProfile.Checked;
            ctlTransImage.SetStringCaption(StringConstants.Profile100PosH, "100");
            ctlTransImage.SetStringPoint(StringConstants.Profile100PosH, ref p1, bDisp);

            //垂直0%
            //p1.X = CTSettings.iniValue.FImageLProfileV0Pos + zeroStringPosVX;
            p1.X = V0Pos + zeroStringPosVX; //Rev25.03/Rev25.02 change by chouno 2017/02/09
            p1.Y = zeroStringPosVY;
            bDisp = frmScanControl.Instance.chkLineProfile.Checked;
            ctlTransImage.SetStringCaption(StringConstants.Profile0PosV, "0");
            ctlTransImage.SetStringPoint(StringConstants.Profile0PosV, ref p1, bDisp);

            //垂直100%
            //p1.X = CTSettings.iniValue.FImageLProfileV100Pos;
            p1.X = V100Pos;//Rev25.03/Rev25.02 change by chouno 2017/02/09
            p1.Y = zeroStringPosVY;
            bDisp = frmScanControl.Instance.chkLineProfile.Checked;
            ctlTransImage.SetStringCaption(StringConstants.Profile100PosV, "100");
            ctlTransImage.SetStringPoint(StringConstants.Profile100PosV, ref p1, bDisp);
        }

        //*******************************************************************************
        //機　　能： スキャン位置・スライス厚の上側・スライス厚の下側・中心線を描画する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        public void SetLine(double? theSliceWidth = null, double? theSliceNum = null, double? theSlicePitch = null)
        {

            float a = 0;
            float b = 0;
            float Swad = 0;
            float r = 0;
            int Shift = 0;    //v18.00追加 byやまおか 2011/02/07 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //エラー時の扱い
            // ERROR: Not supported in C#: OnErrorStatement


            try
            {

                //FCD/FID比
                r = frmMechaControl.Instance.FCDFIDRate;

                //If (r = 0) Or (GVal_mdtpitch(2) = 0) Then                                                      'v15.0削除ここから by 間々田 2009/04/10
                //
                //    'メッセージ表示：幾何歪み校正をしていないため、データ収集エリア表示を正しく行えません。
                //    'MsgBox LoadResString(9482), vbExclamation      '削除 by 山本　2006-11-3
                //    Swad = 10
                //
                //Else                                                                                            'v15.0削除ここまで by 間々田 2009/04/10

                //Swad = (scansel.mscan_width / 2 + scansel.multislice * scansel.multislice_pitch) * theFID / GVal_mdtpitch(2) / (phm / pvm) / theFCD    'v14.00下記変更 byやまおか 2007/07/06
                //コーンの場合       'v14.00追加 byやまおか 2007/07/06
                if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone)
                {

                    if ((theSliceWidth == null))
                        theSliceWidth = CTSettings.scansel.Data.delta_msw * r * CTSettings.scancondpar.Data.dpm;

                    if ((theSliceNum == null))
                        theSliceNum = CTSettings.scansel.Data.k;
                    //If IsMissing(theSlicePitch) Then theSlicePitch = scansel.delta_z

                    if ((theSlicePitch == null))
                        theSlicePitch = CTSettings.scansel.Data.delta_z * CTSettings.scansel.Data.delta_msw / CTSettings.scansel.Data.cone_scan_width * r * CTSettings.scancondpar.Data.dpm;

                    //Swad = (theSlicePitch * (theSliceNum - 1) + theSliceWidth) / 2 / r / GVal_mdtpitch(2) / (fphm / fpvm)
                    //Swad = (theSlicePitch * (theSliceNum - 1) + theSliceWidth) / 2 / r / scancondpar.dpm / (fphm / fpvm)    'v15.0変更 by 間々田 2009/04/10

                    //2014/11/07hata キャストの修正
                    //Swad = ((float)theSlicePitch * ((float)theSliceNum - 1) + (float)theSliceWidth) / 2 / r / CTSettings.scancondpar.Data.dpm;                //v15.0変更 by 間々田 2009/04/10
                    Swad = ((float)theSlicePitch * ((float)theSliceNum - 1) + (float)theSliceWidth) / 2F / r / CTSettings.scancondpar.Data.dpm;                //v15.0変更 by 間々田 2009/04/10

                }
                //Rev21.00 条件追加 by長野 2015/03/09
                else if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScan)

                {
                    //スキャンの場合     'v14.00追加 byやまおか 2007/07/06

                    if ((theSliceWidth == null))
                        theSliceWidth = CTSettings.scansel.Data.mscan_width / CTSettings.scansel.Data.min_slice_wid * r * CTSettings.scancondpar.Data.mdtpitch[2] * CTSettings.detectorParam.vm / CTSettings.detectorParam.hm;
                    if ((theSliceNum == null))
                        theSliceNum = CTSettings.scansel.Data.multislice;
                    if ((theSlicePitch == null))
                        theSlicePitch = CTSettings.scansel.Data.multislice_pitch;

                    //Swad = (theSliceWidth / 2 + theSliceNum * theSlicePitch) / r / GVal_mdtpitch(2) / (fphm / fpvm)
                    //2014/11/07hata キャストの修正
                    //Swad = (float)((theSliceWidth / 2 + theSliceNum * theSlicePitch) / r / CTSettings.scancondpar.Data.mdtpitch[2] / (CTSettings.detectorParam.fphm / CTSettings.detectorParam.fpvm));                //変更 by 間々田 2009/06/17
                    Swad = (float)((theSliceWidth / 2F + theSliceNum * theSlicePitch) / r / CTSettings.scancondpar.Data.mdtpitch[2] / (CTSettings.detectorParam.fphm / CTSettings.detectorParam.fpvm));                //変更 by 間々田 2009/06/17
                }
                else if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScano)
                {
                    //スキャノの場合     'v14.00追加 byやまおか 2007/07/06

                    if ((theSliceWidth == null))
                        theSliceWidth = CTSettings.scansel.Data.mscano_width / CTSettings.scansel.Data.min_slice_wid * r * CTSettings.scancondpar.Data.mdtpitch[2] * CTSettings.detectorParam.vm / CTSettings.detectorParam.hm;
                    if ((theSliceNum == null))
                        theSliceNum = 1;
                    if ((theSlicePitch == null))
                        theSlicePitch = 0;

                    //Swad = (theSliceWidth / 2 + theSliceNum * theSlicePitch) / r / GVal_mdtpitch(2) / (fphm / fpvm)
                    //2014/11/07hata キャストの修正
                    //Swad = (float)((theSliceWidth / 2 + theSliceNum * theSlicePitch) / r / CTSettings.scancondpar.Data.mdtpitch[2] / (CTSettings.detectorParam.fphm / CTSettings.detectorParam.fpvm));                //変更 by 間々田 2009/06/17
                    Swad = (float)((theSliceWidth / 2F + theSliceNum * theSlicePitch) / r / CTSettings.scancondpar.Data.mdtpitch[2] / (CTSettings.detectorParam.fphm / CTSettings.detectorParam.fpvm));                //変更 by 間々田 2009/06/17
                }
            }

            catch
            {
                //上記処理でエラーしている場合   'v15.0追加 by 間々田 2009/04/10
                Swad = 10;
            }
            if (float.IsNaN(Swad) || float.IsInfinity(Swad))
            {
                //不定値の場合
                Swad = 10;
            }

            //a = GVal_ScanPosiA(2) * h_size / fpvm / myZoomScale
            //b = (2 * GVal_ScanPosiB(2) + v_size) / fpvm / myZoomScale

            //以下に変更 by 間々田 2009/06/17
            var _with10 = CTSettings.scancondpar.Data;

            //コーンの場合
            //If scansel.data_mode = DataModeCone Then
            //    a = .cone_scan_posi_a
            //    b = .cone_scan_posi_b
            //Else
            //Rev21.00 条件追加 by長野 2015/03/09
            if (CTSettings.scansel.Data.data_mode != (int)ScanSel.DataModeConstants.DataModeScano)
            {
                a = _with10.scan_posi_a[2];
                b = _with10.scan_posi_b[2];
            }
            else
            {
                a = 0;
                b = 0;
            }
            //End If

            //2014/11/11hata Point型を変更
            PointF Scan_p1 = default(PointF);
            PointF Scan_p2 = default(PointF);
            PointF p1 = default(PointF);
            PointF p2 = default(PointF);
            //Point Scan_p1 = default(Point);
            //Point Scan_p2 = default(Point);
            //Point p1 = default(Point);
            //Point p2 = default(Point);

            bool bDisp = false;

            //スキャン位置の線
            Scan_p1.X = 0;
            //変更2014/10/07hata_v19.51反映
            //Scan_p1.Y = ((transImageCtrl.Detector.v_size - 1) - a * (transImageCtrl.Detector.h_size - 1) + 2 * b) / 2 / transImageCtrl.Detector.fpvm / myZoomScale;
            //2014/11/07hata キャストの修正
            //Scan_p1.Y = ((transImageCtrl.Detector.v_size - 1) - a * (transImageCtrl.Detector.h_size - 1) + 2 * b) / 2 / modCT30K.vScale / myZoomScale;            //v17.4X/v18.00変更 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            Scan_p1.Y = Convert.ToInt32(((transImageCtrl.Detector.v_size - 1) - a * (transImageCtrl.Detector.h_size - 1) + 2 * b) / 2F / (float)modCT30K.vScale / (float)myZoomScale);            //v17.4X/v18.00変更 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            Scan_p2.X = ctlTransImage.Width - 1;
            //変更2014/10/07hata_v19.51反映
            //Scan_p2.Y = ((transImageCtrl.Detector.v_size - 1) + a * (transImageCtrl.Detector.h_size - 1) + 2 * b) / 2 / transImageCtrl.Detector.fpvm / myZoomScale;
             //2014/11/07hata キャストの修正
           //Scan_p2.Y = ((transImageCtrl.Detector.v_size - 1) + a * (transImageCtrl.Detector.h_size - 1) + 2 * b) / 2 / modCT30K.vScale / myZoomScale;            //v17.4X/v18.00変更 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            Scan_p2.Y = Convert.ToInt32(((transImageCtrl.Detector.v_size - 1) + a * (transImageCtrl.Detector.h_size - 1) + 2 * b) / 2F / (float)modCT30K.vScale / (float)myZoomScale);            //v17.4X/v18.00変更 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            bDisp = frmScanControl.Instance.chkDspPosi.Checked;
            ctlTransImage.SetLinePoint(LineConstants.ScanLine, ref Scan_p1, ref Scan_p2, bDisp);

            //スライス厚の上側の線
            p1.X = 0;
            //変更2014/10/07hata_v19.51反映
            //p1.Y = Scan_p1.Y - Swad / transImageCtrl.Detector.fpvm / myZoomScale;
            //2014/11/07hata キャストの修正
            //p1.Y = Scan_p1.Y - Swad / modCT30K.vScale / myZoomScale;            //v17.4X/v18.00変更 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            p1.Y = Convert.ToInt32(Scan_p1.Y - Swad / modCT30K.vScale / (float)myZoomScale);            //v17.4X/v18.00変更 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            
            p2.X = ctlTransImage.Width - 1;
            //変更2014/10/07hata_v19.51反映
            //p2.Y = Scan_p2.Y - Swad / transImageCtrl.Detector.fpvm / myZoomScale;
            //2014/11/07hata キャストの修正
            //p2.Y = Scan_p2.Y - Swad / modCT30K.vScale / myZoomScale;            //v17.4X/v18.00変更 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            p2.Y = Convert.ToInt32(Scan_p2.Y - Swad / modCT30K.vScale / (float)myZoomScale);            //v17.4X/v18.00変更 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            bDisp = frmScanControl.Instance.chkDspSW.Checked;
            ctlTransImage.SetLinePoint(LineConstants.UpperLine, ref p1, ref p2, bDisp);

            //スライス厚の下側の線
            p1.X = 0;
            //変更2014/10/07hata_v19.51反映
            //p1.Y = Scan_p1.Y + Swad / transImageCtrl.Detector.fpvm / myZoomScale;
            //2014/11/07hata キャストの修正
            //p1.Y = Scan_p1.Y + Swad / modCT30K.vScale / myZoomScale;            //v17.4X/v18.00変更 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            p1.Y = Convert.ToInt32(Scan_p1.Y + Swad / modCT30K.vScale / (float)myZoomScale);            //v17.4X/v18.00変更 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            
            p2.X = ctlTransImage.Width - 1;
            //変更2014/10/07hata_v19.51反映
            //p2.Y = Scan_p2.Y + Swad / transImageCtrl.Detector.fpvm / myZoomScale;
            //2014/11/07hata キャストの修正
            //p2.Y = Scan_p2.Y + Swad / modCT30K.vScale / myZoomScale;            //v17.4X/v18.00変更 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            p2.Y = Convert.ToInt32(Scan_p2.Y + Swad / modCT30K.vScale / (float)myZoomScale);            //v17.4X/v18.00変更 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            ctlTransImage.SetLinePoint(LineConstants.LowerLine, ref p1, ref p2, bDisp);

            //中心線（縦）
            p1.Y = 0;
            p2.Y = ctlTransImage.Height - 1;

            //変更2014/10/07hata_v19.51反映
            //p1.X = ctlTransImage.Width / 2;
            //検出器シフトに対応     'v18.00変更 byやまおか 2011/02/07 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //_with14.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Cyan);
            ctlTransImage.SetUserLineColorUsed(LineConstants.LowerLine, false);
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //if (CTSettings.DetShiftOn)
            if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
            {
                //基準位置かシフト位置なら、その場所をセットする。
                if (modDetShift.IsOKShiftPos)
                {
                    //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                    //if (CTSettings.scaninh.Data.lr_sft == 0)
                    if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
                    {
                        if ((modDetShift.DetShift == modDetShift.DetShiftConstants.DetShift_forward))
                        {
                            Shift = Convert.ToInt32(CTSettings.scancondpar.Data.det_sft_pix_r / transImageCtrl.Detector.fphm / myZoomScale * (transImageCtrl.Detector.IsLRInverse ? -1 : 1));
                        }
                        else if ((modDetShift.DetShift == modDetShift.DetShiftConstants.DetShift_backward))
                        {
                            Shift = (-1) * Convert.ToInt32(CTSettings.scancondpar.Data.det_sft_pix_l / transImageCtrl.Detector.fphm / myZoomScale * (transImageCtrl.Detector.IsLRInverse ? -1 : 1));
                        }
                        else
                        {
                            Shift = 0;
                        }
                    }
                    else
                    {
                        //Shift = IIf(DetShift = DetShift_origin, 0, scancondpar.det_sft_pix / fphm / myZoomScale)
                        if ((modDetShift.DetShift != modDetShift.DetShiftConstants.DetShift_origin))
                        {
                            Shift = Convert.ToInt32(CTSettings.scancondpar.Data.det_sft_pix / transImageCtrl.Detector.fphm / myZoomScale * (transImageCtrl.Detector.IsLRInverse ? -1 : 1));
                        }
                        else
                        {
                            Shift = 0;
                        }
                    }

                //それ以外(シフト中)は中間をセットする
                }
                else
                {
                    //Shift = scancondpar.det_sft_pix / fphm / myZoomScale / 2
                    //Shift = scancondpar.det_sft_pix / fphm / myZoomScale / 2 * IIf(IsLRInverse, -1, 1)
                    //不定の場合、基準位置である可能性が高いため、縦線の位置は基準位置とする。
                    Shift = 0;
                    //v18.00変更 byやまおか 2011/09/05 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    //_with14.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                    //ctlTransImage.SetUserLineColor(LineConstants.LowerLine, Color.Yellow);
                    //ctlTransImage.SetUserLineColorUsed(LineConstants.LowerLine, true);
                    //Rev23.10 修正 by長野 2015/09/20
                    //ctlTransImage.SetUserLineColor(LineConstants.CenterLine, Color.Yellow);
                    //ctlTransImage.SetUserLineColorUsed(LineConstants.CenterLine, false);
                }
            }
            p1.X = Convert.ToInt32(ctlTransImage.Width / 2F) + Shift;
            
            p2.X = p1.X;
            bDisp = frmScanControl.Instance.chkCenterLine.Checked;
            ctlTransImage.SetLinePoint(LineConstants.CenterLine, ref p1, ref p2, bDisp);

            //中心線（横）   'v15.10追加 byやまおか 2009/10/22
            //2014/11/07hata キャストの修正
            //p1.Y = ctlTransImage.Height / 2;
            p1.Y = Convert.ToInt32(ctlTransImage.Height / 2F);
            p2.Y = p1.Y;
            p1.X = 0;
            p2.X = ctlTransImage.Width - 1;
            bDisp = frmScanControl.Instance.chkCenterLineH.Checked;
            ctlTransImage.SetLinePoint(LineConstants.CenterLineH, ref p1, ref p2, bDisp);

            //Rev25.00 ラインプロファイル(赤) 水平方向位置 by長野 2016/08/08
            p1.Y = myLProfHPos;
            p2.Y = p1.Y;
            p1.X = 0;
            p2.X = ctlTransImage.Width - 1;
            bDisp = frmScanControl.Instance.chkLineProfile.Checked;
            ctlTransImage.SetLinePoint(LineConstants.ProfilePosH, ref p1, ref p2, bDisp);
            //Rev25.00 ラインプロファイル(赤) 垂直方向位置 by長野 2016/08/08
            p1.Y = 0;
            p2.Y = ctlTransImage.Height - 1;
            p1.X = myLProfVPos;
            p2.X = p1.X;
            bDisp = frmScanControl.Instance.chkLineProfile.Checked;
            ctlTransImage.SetLinePoint(LineConstants.ProfilePosV, ref p1, ref p2, bDisp);

            //線の更新のため透視画像コントロールをリフレッシュ
            ctlTransImage.Refresh();

        }

        //*******************************************************************************
        //機　　能： ROI変更時イベント処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void myRoi_Changed(string RoiInfo)
        {

            string workStr = null;
            workStr = RoiInfo;

            if ((!string.IsNullOrEmpty(RoiInfo)) & (myRoi.RoiMaxNum > 1))
            {
                workStr = Convert.ToString(myRoi.TargetRoi) + " " + workStr;
            }

            //キャプション更新
            UpdateCaption(workStr);

            //右クリック時のメニューの調整
            //mnuROIEditCopy.Enabled = myRoi.IsExistSelectedRoi And myRoi.ManualCut   '選択されているROIが存在するか？
            //mnuROIEditPaste.Enabled = myRoi.IsExistRoi() And myRoi.ManualCut        'クリップボードにROIが存在するか？
            //mnuROIEditDelete.Enabled = mnuROIEditCopy.Enabled And myRoi.ManualCut   '選択されているROIが存在するか？
            //mnuROIEditAllDelete.Enabled = (myRoi.NumOfRois > 0) And myRoi.ManualCut 'ROIが存在するか？
            if (myRoi.TargetRoi == 0)
            {
                mnuRoiInput.Enabled = false;
            }
            else
            {
                mnuRoiInput.Enabled = myRoi.IsSizable(myRoi.TargetRoi);
            }

            //'ツールバー上のボタン
            //With Toolbar1
            //    .Buttons("Cut").Enabled = mnuROIEditDelete.Enabled And .Enabled 'ROIの切り取り
            //    .Buttons("Copy").Enabled = mnuROIEditCopy.Enabled And .Enabled   'ROIのコピー
            //    .Buttons("Paste").Enabled = mnuROIEditPaste.Enabled And .Enabled 'ROIの貼り付け
            //    .Buttons("Save").Enabled = .Buttons("Open").Enabled And ((myRoi.NumOfRois > 0) Or (PRDPoint > 0)) And .Enabled
            //End With

            //ROI変更時処理
            if (!string.IsNullOrEmpty(RoiInfo))
                if (RoiChanged != null)
                {
                    //RoiChanged();
                    RoiChanged(RoiInfo, EventArgs.Empty);
                }

        }

        private void ctlTransImage_MouseDown(object sender, MouseEventArgs e)
        {
            MouseButtons Button = e.Button;
            int x = e.X;
            int y = e.Y;

            //テスト追加2014/07/14hata_<変更>
            //int Shift = 0;
            //if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            //{
            //    Shift = 1;
            //}			


            //Roi制御ありの場合
            if ((myRoi != null))
            {
                //テスト追加2014/07/14hata_<変更>
                //myRoi.MouseDown(Button, Shift, x, y);   //スライス厚ラインをハンドルした状態でマウスダウンされている
                myRoi.MouseDown(Button, Control.ModifierKeys, x, y);   //スライス厚ラインをハンドルした状態でマウスダウンされている

            }
            else if ((myHandleLine == LineConstants.LowerLine) | (myHandleLine == LineConstants.UpperLine))
            {


                //マウスダウン時のポインタ座標を記憶
                MouseDownPoint.X = x;
                MouseDownPoint.Y = y;
                //var _with22 = myLine[(int)myHandleLine];
                //myHandleLineP1.X = _with22.x1;
                //myHandleLineP1.Y = _with22.y1;
                //myHandleLineP2.X = _with22.x2;
                //myHandleLineP2.Y = _with22.y2;
                //var _with22 = ctlTransImage.myTLines;
                var _with22 = myTlines;
                myHandleLineP1 = _with22[myHandleLine].P1;
                myHandleLineP2 = _with22[myHandleLine].P2;

            }

        }

        private void ctlTransImage_MouseMove(object sender, MouseEventArgs e)
        {
            MouseButtons Button = e.Button;
            int x = e.X;
            int y = e.Y;

            //var myLine = ctlTransImage.myTLines;
            var myLine = myTlines;

            bool IsCone = false;

            //使っていないのでコメント
            //int Shift = 0;
            //if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            //{
            //    Shift = 1;
            //}			

            //'開発環境時のみマウスポインタの座標を表示
            //If InStr(LCase$(command()), "-debug") > 0 Then
            //    frmCTMenu.StatusBar1.Panels("Scan").Text = "X : " & CStr(X) & "  Y: " & CStr(y)
            //End If

            //Roi制御ありの場合
            if ((myRoi != null))
            {
                //x = CorrectInRange(x, 0, VB6.PixelsToTwipsX(ctlTransImage.Width) - 1);
                //y = CorrectInRange(y, 0, VB6.PixelsToTwipsY(ctlTransImage.Height) - 1);
                x = modLibrary.CorrectInRange(x, 0, ctlTransImage.Width - 1);
                y = modLibrary.CorrectInRange(y, 0, ctlTransImage.Height - 1);
                myRoi.MouseMove(x, y);

                //テスト追加2014/07/14hata
                //myRoi.IndicateRoi();
                //ctlTransImage.Invalidate();
                //roi設定中
                myRoi.RoiFlg = 2;
                ctlTransImage.Refresh();

                return;
            }

            //コーンビーム時以外はラインをハンドルさせないのでここで抜ける
            //if (scansel.data_mode != modScansel.DataModeConstants.DataModeCone)
            //if (CTSettings.scansel.Data.data_mode != (int)ScanSel.DataModeConstants.DataModeCone)
            //return;
            //Rev25.00 ラインプロファイルは操作できるように変更 by長野 2016/08/16
            IsCone = (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone) ? true : false;
           
            //スライス厚ラインをハンドルした状態でマウスダウンされている
            int dY = 0;
            LineConstants otherHandleLine = default(LineConstants);
            //if (((myHandleLine == LineConstants.LowerLine) | (myHandleLine == LineConstants.UpperLine)) & (Button == VB6.MouseButtonConstants.LeftButton))
            //if (((myHandleLine == LineConstants.LowerLine) | (myHandleLine == LineConstants.UpperLine)) & (Button == MouseButtons.Left))
            //Rev25.00 cone以外は無視 by長野 2016/08/16
            if (((myHandleLine == LineConstants.LowerLine) | (myHandleLine == LineConstants.UpperLine)) & (Button == MouseButtons.Left) && IsCone)
            {

                dY = y - MouseDownPoint.Y;

                if (myHandleLine == LineConstants.UpperLine)
                {
                    //2014/11/07hata キャストの修正
                    //dY = (int)modLibrary.CorrectInRange(dY, -modLibrary.MinVal(myHandleLineP1.Y, myHandleLineP2.Y), myLine[LineConstants.ScanLine].P1.Y - myHandleLineP1.Y);
                    dY = Convert.ToInt32(modLibrary.CorrectInRange(dY, -modLibrary.MinVal(myHandleLineP1.Y, myHandleLineP2.Y), myLine[LineConstants.ScanLine].P1.Y - myHandleLineP1.Y));
                }
                else
                {
                    //2014/11/07hata キャストの修正
                    //dY = (int)modLibrary.CorrectInRange(dY, myLine[LineConstants.ScanLine].P1.Y - myHandleLineP1.Y, (ctlTransImage.Height - 1) - modLibrary.MaxVal(myHandleLineP1.Y, myHandleLineP2.Y));
                    dY = Convert.ToInt32(modLibrary.CorrectInRange(dY, myLine[LineConstants.ScanLine].P1.Y - myHandleLineP1.Y, (ctlTransImage.Height - 1) - modLibrary.MaxVal(myHandleLineP1.Y, myHandleLineP2.Y)));
                }

                //2014/11/11hata Point型を変更？？
                PointF pA1 = new Point();
                PointF pA2 = new Point();
                PointF pB1 = new Point();
                PointF pB2 = new Point();
                //Point pA1 = new Point();
                //Point pA2 = new Point();
                //Point pB1 = new Point();
                //Point pB2 = new Point();

                //ハンドルしているラインを移動
                //var _with23 = myLine[(int)myHandleLine];
                //_with23.y1 = Convert.ToInt16(myHandleLineP1.Y + dY);
                //_with23.y2 = Convert.ToInt16(myHandleLineP2.Y + dY);
                pA1.X = myLine[myHandleLine].P1.X;
                //2014/11/11hata Point型を変更
                pA1.Y = myHandleLineP1.Y + (float)dY;
                //pA1.Y = myHandleLineP1.Y + dY;
                pA2.X = myLine[myHandleLine].P2.X;
                //2014/11/11hata Point型を変更？？
                pA2.Y = myHandleLineP2.Y + (float)dY;
                //pA2.Y = myHandleLineP2.Y + dY;
                ctlTransImage.SetLinePoint(myHandleLine, ref pA1, ref pA2);

                //もう一方のラインも移動
                otherHandleLine = (myHandleLine == LineConstants.UpperLine ? LineConstants.LowerLine : LineConstants.UpperLine);

                //var _with24 = myLine[(int)otherHandleLine];
                //_with24.y1 = Convert.ToInt16(2 * myLine[(int)LineConstants.ScanLine].y1 - myLine[(int)myHandleLine].y1);
                //_with24.y2 = Convert.ToInt16(2 * myLine[(int)LineConstants.ScanLine].y2 - myLine[(int)myHandleLine].y2);
                pB1.X = myLine[otherHandleLine].P1.X;
                pB1.Y = (2 * myLine[LineConstants.ScanLine].P1.Y - myLine[myHandleLine].P1.Y); ;
                pB2.X = myLine[otherHandleLine].P2.X;
                pB2.Y = (2 * myLine[LineConstants.ScanLine].P2.Y - myLine[myHandleLine].P2.Y); ;
                ctlTransImage.SetLinePoint(otherHandleLine, ref pB1, ref pB2);

                //線を更新するために透視画像コントロールをリフレッシュ
                ctlTransImage.Refresh();

                //マウスポインタが上側スライス厚ライン上にある
                //ElseIf PointOnLine(x, y, _
                //'                   myLine(UpperLine).x1, myLine(UpperLine).y1, _
                //'                   myLine(UpperLine).x2, myLine(UpperLine).y2) Then
                //v17.50変更 左右反転時対応 by 間々田 2011/02/02
            }
            //Rev25.00 ラインプロファイルを追加 by長野 2016/08/08
            else if ((myHandleLine == LineConstants.ProfilePosH) && (Button == MouseButtons.Left))
            {
                int correct_y = 0;

                correct_y = Convert.ToInt32(modLibrary.CorrectInRange(y, 0 ,ctlTransImage.Height - 1));

                PointF pA1 = new Point();
                PointF pA2 = new Point();

                //ハンドルしているラインを移動
                pA1.X = myLine[myHandleLine].P1.X;
                pA1.Y = correct_y;
                pA2.X = myLine[myHandleLine].P2.X;
                pA2.Y = correct_y;
                ctlTransImage.SetLinePoint(myHandleLine, ref pA1, ref pA2);
 
                myLProfHPos = correct_y;
                
                SetTransProfile();
               
                //線を更新するために透視画像コントロールをリフレッシュ
                ctlTransImage.Refresh();
            }
            else if((myHandleLine == LineConstants.ProfilePosV) && (Button == MouseButtons.Left))
            {
                int correct_x = 0;

                correct_x = Convert.ToInt32(modLibrary.CorrectInRange(x, 0,ctlTransImage.Width - 1));

                PointF pA1 = new Point();
                PointF pA2 = new Point();

                //ハンドルしているラインを移動
                pA1.X = correct_x;
                pA1.Y = myLine[myHandleLine].P1.Y;
                pA2.X = correct_x;
                pA2.Y = myLine[myHandleLine].P2.Y;
                ctlTransImage.SetLinePoint(myHandleLine, ref pA1, ref pA2);

                myLProfVPos = correct_x;

                SetTransProfile();

                //線を更新するために透視画像コントロールをリフレッシュ
                ctlTransImage.Refresh();
            }
            else if ((PointOnLineMirror((short)x,
                                       (short)y,
                                       (short)myLine[LineConstants.UpperLine].P1.X,
                                       (short)myLine[LineConstants.UpperLine].P1.Y,
                                       (short)myLine[LineConstants.UpperLine].P2.X,
                                       (short)myLine[LineConstants.UpperLine].P2.Y)) && IsCone)//Rev25.00 cone以外は無視 by長野 2016/08/18
            {

                //Rev25.00 chk有効時のみ実行 by長野 2016/08/08
                if (frmScanControl.Instance.chkDspSW.Checked == true)
                {
                    //上側スライス厚ラインをハンドル
                    myHandleLine = LineConstants.UpperLine;

                    //マウスポインタを上下移動カーソルとする
                    this.Cursor = System.Windows.Forms.Cursors.SizeNS;
                }

                //マウスポインタが下側スライス厚ライン上にある
                //ElseIf PointOnLine(x, y, _
                //'                   myLine(LowerLine).x1, myLine(LowerLine).y1, _
                //'                   myLine(LowerLine).x2, myLine(LowerLine).y2) Then
                //v17.50変更 左右反転時対応 by 間々田 2011/02/02
            }
            else if ((PointOnLineMirror((short)x,
                                       (short)y,
                                       (short)myLine[LineConstants.LowerLine].P1.X,
                                       (short)myLine[LineConstants.LowerLine].P1.Y,
                                       (short)myLine[LineConstants.LowerLine].P2.X,
                                       (short)myLine[LineConstants.LowerLine].P2.Y)) && IsCone)//Rev25.00 cone以外は無視 by長野 2016/08/16
            {
                //Rev25.00 chk有効時のみ実行 by長野 2016/08/08
                if (frmScanControl.Instance.chkDspSW.Checked == true)
                {
                    //下側スライス厚ラインをハンドル
                    myHandleLine = LineConstants.LowerLine;

                    //マウスポインタを上下移動カーソルとする
                    this.Cursor = System.Windows.Forms.Cursors.SizeNS;
                }

            }
            else if (PointOnLineMirror((short)x,
                                       (short)y,
                                       (short)myLine[LineConstants.ProfilePosH].P1.X,
                                       (short)myLine[LineConstants.ProfilePosH].P1.Y,
                                       (short)myLine[LineConstants.ProfilePosH].P2.X,
                                       (short)myLine[LineConstants.ProfilePosH].P2.Y)) //Rev25.00 ラインプロファイル 水平 by長野 2016/08/08 
            {
                //Rev25.00 chk有効時のみ実行 by長野 2016/08/08
                if (frmScanControl.Instance.chkLineProfile.Checked == true)
                {
                    //水平ラインをハンドル
                    myHandleLine = LineConstants.ProfilePosH;

                    //マウスポインタを上下移動カーソルとする
                    this.Cursor = System.Windows.Forms.Cursors.SizeNS;
                }
            }
            else if (PointOnLineMirror((short)x,
                                       (short)y,
                                       (short)myLine[LineConstants.ProfilePosV].P1.X,
                                       (short)myLine[LineConstants.ProfilePosV].P1.Y,
                                       (short)myLine[LineConstants.ProfilePosV].P2.X,
                                       (short)myLine[LineConstants.ProfilePosV].P2.Y)) //Rev25.00 ラインプロファイル 垂直 by長野 2016/08/08 
            {
                //Rev25.00 chk有効時のみ実行 by長野 2016/08/08
                if (frmScanControl.Instance.chkLineProfile.Checked == true)
                {
                    //下側スライス厚ラインをハンドル
                    myHandleLine = LineConstants.ProfilePosV;

                    //マウスポインタを左右移動カーソルとする
                    this.Cursor = System.Windows.Forms.Cursors.SizeWE;
                }
            }
            else
            {

                //何もハンドルしていない
                myHandleLine = 0;

                //マウスポインタを元に戻す
                this.Cursor = System.Windows.Forms.Cursors.Default;

            }
        }

        private void ctlTransImage_MouseUp(object sender, MouseEventArgs e)
        {
            //int Shift = 0;
            //if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            //{
            //    Shift = 1;
            //}			

            MouseButtons Button = e.Button;
            int x = e.X;
            int y = e.Y;

            //var myLine = ctlTransImage.myTLines;
            var myLine = myTlines;

            //Roi制御ありの場合
            int K = 0;
            float pitch = 0;
            float SW = 0;
            bool IsExistScanCondition = false;
            if ((myRoi != null))
            {

                //x = CorrectInRange(x, 0, VB6.PixelsToTwipsX(ctlTransImage.Width) - 1);
                //y = CorrectInRange(y, 0, VB6.PixelsToTwipsY(ctlTransImage.Height) - 1);
                x = modLibrary.CorrectInRange(x, 0, ctlTransImage.Width - 1);
                y = modLibrary.CorrectInRange(y, 0, ctlTransImage.Height - 1);

                //テスト追加2014/07/14hata_<変更>
                //myRoi.MouseUp(Button, Shift, x, y);
                myRoi.MouseUp(Button, Control.ModifierKeys, x, y);

                //テスト追加2014/07/14hata
                //myRoi.IndicateRoi();
                //ctlTransImage.Invalidate();
                //roi設定中
                myRoi.RoiFlg = 2;
                ctlTransImage.Refresh();

                //削除_Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
                //myShape[1].BringToFront();

                //スライス厚ラインをハンドルした状態の場合
            }
            else if ((myHandleLine == LineConstants.LowerLine) | (myHandleLine == LineConstants.UpperLine))
            {


                //「スキャン条件－詳細」が表示されているか
                //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
                //IsExistScanCondition = modLibrary.IsExistForm(frmScanCondition.Instance);
                IsExistScanCondition = modLibrary.IsExistForm("frmScanCondition");

                //「スキャン条件－詳細」が表示されている場合
                if (IsExistScanCondition)
                {
                    var _with25 = frmScanCondition.Instance;
                    SW = (float)_with25.cwneSlice.Value;
                    pitch = (float)_with25.cwneDelta_z.Value;

                }
                else
                {
                    var _with26 = CTSettings.scansel.Data;
                    SW = _with26.delta_msw * frmMechaControl.Instance.FCDFIDRate * CTSettings.scancondpar.Data.dpm;
                    pitch = _with26.delta_z / _with26.cone_scan_width * SW;
                }

                //枚数計算
                //K = (2 * Abs(myLine(ScanLine).y1 - myLine(myHandleLine).y1) * myZoomScale * frmMechaControl.FCDFIDRate * scancondpar.dpm * fpvm - SW) / pitch + 1
                //v17.68 v17.99 コーンの枚数が常に1枚おおい by長野　2012-02-28
                K = Convert.ToInt32((2 * System.Math.Abs(myLine[LineConstants.ScanLine].P1.Y - myLine[myHandleLine].P1.Y) * myZoomScale * frmMechaControl.Instance.FCDFIDRate * CTSettings.scancondpar.Data.dpm * CTSettings.detectorParam.fpvm - SW) / pitch + 1);


                //「スキャン条件－詳細」が表示されている場合
                if (IsExistScanCondition)
                {
                    var _with27 = frmScanCondition.Instance;

                    //変更2015/02/02hata_Max/Min範囲のチェック
                    //_with27.cwneK.Value = K;
                    _with27.cwneK.Value = modLibrary.CorrectInRange(K, _with27.cwneK.Minimum, _with27.cwneK.Maximum);

                    SetLine((double)_with27.cwneSlice.Value, (double)_with27.cwneK.Value, (double)_with27.cwneDelta_z.Value);   //v15.01追加 2009/09/02 線を必ず再描画する
                }
                else
                {
                    //frmScanCondition.Instance.Setup(, , K);
                    frmScanCondition.Instance.Setup(false, null, K);
                }

                //Rev26.00 add by chouno 2017/03/03
                //[ガイド]タブで設定した内容は、設定時のFCD,FDD,Y軸位置でのみ有効なため、変更されたらフラグOFF
                if (frmScanControl.Instance.scanAreaSetCmpFlg == true || frmScanControl.Instance.scanCondSetCmpFlg == true)
                {
                    frmScanControl.Instance.scanAreaSetCmpFlg = false;
                    frmScanControl.Instance.scanCondSetCmpFlg = false;
                }

                //[条件]タブの画像からの復元は、設定時のFCD,FDD,Y軸位置でのみ有効なため、変更されたらフラグOFF
                if (frmScanControl.Instance.scanCondImgSetCmpFlg == true)
                {
                    frmScanControl.Instance.scanCondImgSetCmpFlg = false;
                }


                //何もハンドルしていない
                myHandleLine = 0;

            }
        }

        private void ctlTransImage_Paint(object sender, PaintEventArgs e)
        {

            //Roi制御なしの場合、何もしない
            if (myRoi == null) return;

            Graphics g = e.Graphics;

            //補間モード
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            //Roiの表示
            myRoi.DispRoi(g);

        }

        //追加2014/10/07hata_v19.51反映
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから
        //*******************************************************************************
        //機　　能： 水平スライダー変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足：
        //
        //履　　歴： v17.48 2011/03/15 (豊川)間々田  新規作成
        //*******************************************************************************
         private void hsbImage_ValueChanged(object sender, EventArgs e)
        {
            //透視画像コントロールの配置
            ctlTransImage.SetBounds(-hsbImage.Value, -vsbImage.Value, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

        }

        ////*******************************************************************************
        ////機　　能： 水平スライダースクロール時処理
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： V18.00  11/07/30    やまおか    新規作成
        ////*******************************************************************************
        //private void hsbImage_Scroll_Renamed(int newScrollValue)
        //{

        //    //画像の移動
        //    hsbImage_Change(0);

        //}

        //*******************************************************************************
        //機　　能： 垂直スライダー変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足：
        //
        //履　　歴： v17.48 2011/03/15 (豊川)間々田  新規作成
        //*******************************************************************************
        private void vsbImage_ValueChanged(object sender, EventArgs e)
        {

            //透視画像コントロールの配置
            ctlTransImage.SetBounds(-hsbImage.Value, -vsbImage.Value, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

        }

        ////*******************************************************************************
        ////機　　能： 垂直スライダースクロール時処理
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： V18.00  11/07/30    やまおか    新規作成
        ////*******************************************************************************
        //private void vsbImage_Scroll_Renamed(int newScrollValue)
        //{

        //    //画像の移動
        //    vsbImage_Change(0);

        //}

        //private void hsbImage_Scroll(System.Object eventSender, System.Windows.Forms.ScrollEventArgs eventArgs)
        //{
        //    switch (eventArgs.Type)
        //    {
        //        case System.Windows.Forms.ScrollEventType.ThumbTrack:
        //            hsbImage_Scroll_Renamed(eventArgs.NewValue);
        //            break;
        //        case System.Windows.Forms.ScrollEventType.EndScroll:
        //            hsbImage_Change(eventArgs.NewValue);
        //            break;
        //    }
        //}
        //private void vsbImage_Scroll(System.Object eventSender, System.Windows.Forms.ScrollEventArgs eventArgs)
        //{
        //    switch (eventArgs.Type)
        //    {
        //        case System.Windows.Forms.ScrollEventType.ThumbTrack:
        //            vsbImage_Scroll_Renamed(eventArgs.NewValue);
        //            break;
        //        case System.Windows.Forms.ScrollEventType.EndScroll:
        //            vsbImage_Change(eventArgs.NewValue);
        //            break;
        //    }
        //}
        ////v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここまで


 
    }
}
