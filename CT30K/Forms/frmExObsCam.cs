using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SensorTechnology;

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;

using CT30K.Properties;
using CT30K.Common;
using CTAPI;

namespace CT30K
{
    public partial class frmExObsCam : Form
    {
        private static frmExObsCam _Instance = null;

        /// <summary>
        /// 
        /// </summary>
        public static frmExObsCam Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmExObsCam();
                }

                return _Instance;
            }
        }

        #region フィールド

        //ROI制御オブジェクト参照用
        private RoiData myRoi;

        //ROI制御タイプ
        public enum ImageProcType
        {
            RoiNone,
            RoiAutoPos,
        }

        //ROI制御タイプ変数
        private ImageProcType myImageProc;

        //イベント宣言
        public event EventHandler RoiChanged;

        //センテックカメラ制御用オブジェクト
        public modExObsCam ExObsCam = null;
        private CStCamera stCamera = null;
        
        //カメラハンドル
        private IntPtr m_CamHandle = IntPtr.Zero;
   
        //コールバック関数
        private StCam.fStCamPreviewBitmapCallbackFunc m_fncPreviewBitmapCallback = null;
        uint dwCallbackNo = 0;

        //最後にクリックしたボタン
        private MouseButtons LastButton;

        //前回押下されたRoiボタン
        private ToolStripButton LastRoiButton;
        private bool DoubleClickOn;				//ダブルクリック制御用

        //ズームフラグ
        private int zoomflg = 0;//(0:非ズーム,1;ズーム)

        //描画オブジェクト
        private Graphics grfx;
        //描画用bmp
        private Bitmap mybmp;

        //描画マトリクス
        private int mymatrix = 1024;
        //ROI描画限界領域(pix)
        private int myLimitWidth;
        private int myLimitHeight;

        //カメラ画像データからアプリ用に切り出すサイズ(ini依存)
        private int OffsetMaskX = 0;
        private int OffsetMaskY = 0;
        private int OffsetMaskWidth = 960;
        private int OffsetMaskHeight = 960;

        private int ZoomOffsetMaskX = 0;
        private int ZoomOffsetMaskY = 0;
        private int ZoomOffsetMaskWidth = 960;
        private int ZoomOffsetMaskHeight = 960;

        //拡大クリック時の倍率(ini依存)
        private float ZoomMagnify = 1;

        //表示領域設定用
        private RectangleF srcRectF;
        private RectangleF dstRectF;

        //微調の設置判定(機能有無込み)
        private bool myftableOn = false;

        //表示モード 
        private int myDispFlg; //0:透視画面左,1:CT画像の上

        //フォームの移動ロックフラグ
        private int myFrmLockFlg = 0;//0:アンロック,1:ロック

        //ROI描画ONフラグ 通常ONだが、frmMechaMoveが自動位置開始を制御し始めたらROIを消したい。
        private bool myROIDraw = true;

        internal float beforeAutoPosTableUD = CTSettings.GValUpperLimit; //Rev23.30 自動位置指定前の昇降位置 by長野 2016/03/02
        internal int beforeAutoPosTableRot = 0; //Rev23.30 自動位置指定前の回転角度 by長野 2016/03/02

        //Rev26.00 [ガイド]タブ スキャンエリア設定用のROI情報 add by chouno 2016/12/27 --->
        private float x1;
        private float x2;
        private float y1;
        private float y2;
        private Pen myScanAreaROIPen = new Pen(Color.Lime, 1);
        private bool myScanAreaROIOn = false;
        public bool ScanAreaROIOn
        {
            get
            {
                return myScanAreaROIOn;
            }
            set
            {
                myScanAreaROIOn = value;
            }
        }
        //<---

        #endregion

        # region プロパティ
        //フォームの移動ロックフラグ
        public int FrmLockFlg
        {
            get
            {
                return myFrmLockFlg;
            }
            set
            {
                myFrmLockFlg = value;
            }
        }

        //表示モード
        public int DispFlg
        {
            get
            {
                return myDispFlg; 
            }
            set
            {
                myDispFlg = value;
            }
        }

        //マトリクス
        public int matrix
        {
            get
            {
                return mymatrix;
            }
        }

        //画素サイズ
        public float pixsize
        {
            get
            {
                if (ftableOn == true)
                {
                    return (CTSettings.scancondpar.Data.ExObsCamPixSizeOnFTable[zoomflg]);
                }
                else
                {
                    return (CTSettings.scancondpar.Data.ExObsCamPixSize[zoomflg]);
                }
            }
        }

        //ROI描画限界サイズ
        public int LimitWidth
        {
            get
            {
                return myLimitWidth;
            }
        }
        //ROI描画限界サイズ
        public int LimitHeight
        {
            get
            {
                return myLimitHeight;
            }
        }

        //カメラハンドル
        public IntPtr CamHandle
        {
            get
            {
                return (m_CamHandle);
            }
        }

        //微調のOn/oFf
        public bool ftableOn
        {
            get
            {
                return (CTSettings.scaninh.Data.fine_table == 0 && !((CTSettings.mecainf.Data.ystg_l_limit == 1) && (CTSettings.mecainf.Data.ystg_u_limit == 1) && (CTSettings.mecainf.Data.xstg_l_limit == 1) && (CTSettings.mecainf.Data.xstg_u_limit == 1)));
            }
        }

        //描画ONフラグ
        public bool ROIDraw
        {
            get
            {
                return myROIDraw;
            }
            set
            {
                myROIDraw = value;
            }
        }

        # endregion

        #region コンストラクタ
        //コンストラクタ
        public frmExObsCam()
        {
            InitializeComponent();

            ExObsCam = new modExObsCam();
            stCamera = new CStCamera();
        }
        #endregion

        #region メソッド
        //オープン関数
        public bool OpenCamera()
        {
            bool ret = false;
            uint uintret = 0;

            ret = ExObsCam.mOpenAllCamera();

            if (ret == false)
            {
                m_CamHandle = IntPtr.Zero;
                return (ret);
            }

            stCamera = ExObsCam.OpendCamera;
                       
            uintret = ExObsCam.LoadSettingFile(stCamera);

            //オープンができて、かつ、設定ファイルをロードできたら成功
            if (ret == true && uintret == 0)
            {
                ret = true;
                m_CamHandle = stCamera.mGetCamHandle(stCamera);
            }
            else
            {
                ret = false;
                m_CamHandle = IntPtr.Zero;
            }

            return ret;
        }

        public bool CloseCamera()
        {
            bool ret = false;

            ret = ExObsCam.mCloseCamera();

            m_CamHandle = IntPtr.Zero;

            return ret;
        }

        //キャプチャ開始
        public void CaptureStart()
        {
            //ツールバーのアイコンも変更
            frmCTMenu.Instance.Toolbar1.Items["tsbtnExObsCam"].Image = frmCTMenu.Instance.ImageList1.Images["ExObsCamON"];

            //コールバック関数登録--->
            m_fncPreviewBitmapCallback = new StCam.fStCamPreviewBitmapCallbackFunc(Draw);

            StCam.AddPreviewBitmapCallback(CamHandle, m_fncPreviewBitmapCallback, IntPtr.Zero, out dwCallbackNo);
            //<---

            Application.DoEvents();

            stCamera.StartTransfer();
        }

        //キャプチャ終了
        public void CaptureStop()
        {
            //ツールバーのアイコンも変更
            frmCTMenu.Instance.Toolbar1.Items["tsbtnExObsCam"].Image = frmCTMenu.Instance.ImageList1.Images["ExObsCamOFF"];

            //コールバック関数解除
            StCam.RemovePreviewBitmapCallback(CamHandle, dwCallbackNo);

            stCamera.StopTransfer();
        }

        private void frmExObsCam_Load(object sender, EventArgs e)
        {
            //グラフィックスオブジェクト登録
            grfx = this.CreateGraphics();

            //最近傍法で描画(補間を使うと遅い)
            grfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            //InitControl();
            InitControl(myDispFlg);

            SetCaption();
   
        }
        
        private void Draw(System.IntPtr pbyteBitmap, uint dwBufferSize, uint dwWidth, uint dwHeight, uint dwFrameNo, uint dwPreviewPixelFormat, System.IntPtr lpContext, System.IntPtr lpReserved)
        {
            mybmp = new Bitmap((int)dwWidth, (int)dwHeight, (int)dwWidth * 3, PixelFormat.Format24bppRgb, pbyteBitmap);

            //if (bmp != null)
            //{
            //    grfx.DrawImage(bmp, dstRectF, srcRectF, GraphicsUnit.Pixel);
            //}

            this.Invalidate();

            //Roi制御なしの場合、何もしない
            if (myRoi == null)
            {
                return;
            }
            else
            {
                //Roiの表示
                //myRoi.DispRoi(grfx);
                //Rev23.30 引数追加 by長野 2016/02/06
                if (myROIDraw == true)
                {
                    myRoi.DispRoi(grfx);
                }
            }

        }
        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="e"></param>
        //protected override void OnPaint(PaintEventArgs e)
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            ////補間モード
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            if (mybmp != null)
            {
                e.Graphics.DrawImage(mybmp, dstRectF, srcRectF, GraphicsUnit.Pixel);

                //iniのメンテフラグがONの場合はセンターに十字を描画
                if (CTSettings.iniValue.ExObsCamMaintCrossLine == 1)
                {
                    if (DispFlg == 0)
                    {
                        e.Graphics.DrawLine(Pens.Red, 0.0f, (float)(this.ClientRectangle.Height - 1) / 2.0f, (float)this.ClientRectangle.Width, (float)(this.ClientRectangle.Height - 1) / 2.0f);
                        e.Graphics.DrawLine(Pens.Red, (float)(this.ClientRectangle.Width - 1) / 2.0f, 0.0f, (float)(this.ClientRectangle.Width - 1) / 2.0f, (float)this.ClientRectangle.Height);
                    }
                    else
                    {
                        e.Graphics.DrawLine(Pens.Red, 0.0f, (float)(1024 - 1) / 2.0f, (float)this.ClientRectangle.Width, (float)((1024 - 1)) / 2.0f);
                        e.Graphics.DrawLine(Pens.Red, (float)((1024 - 1)) / 2.0f, 0.0f, (float)((1024 - 1)) / 2.0f, 1024);
                    }
                }

                if (myScanAreaROIOn == true)
                {
                    e.Graphics.DrawEllipse(myScanAreaROIPen, x1, y1, x2 - x1 + 1, y2 - y1 + 1);
                    e.Graphics.DrawRectangle(myScanAreaROIPen, x1, y1, x2 - x1 + 1, y2 - y1 + 1);
                }
            }
        }

        //*************************************************************************************************
        //機　　能： [ガイド]タブ スキャンエリア用ROI情報セット
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.30  16/02/06   (検S1)長野   新規作成
        //*************************************************************************************************
        public void ScanAreaROISet(int Width, int Height, float Center,int Size ,RoiData.RoiShape roishape)
        {
            switch (roishape)
            {
                case RoiData.RoiShape.ROI_CIRC:
                    x1 = Center - Size - 1;
                    y1 = Center - Size - 1;
                    x2 = Center + Size;
                    y2 = Center + Size;
                    break;
                default:
                    break;

            }
        }

        //*************************************************************************************************
        //機　　能： 各コントロールの初期設定
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.30  16/02/06   (検S1)長野   新規作成
        //*************************************************************************************************
        private void InitControl(int DispFlg)
        {
            ExObsCam.ChangeSizeAndLocation(DispFlg);

            mnuRoiInput.Enabled = false;	
      
            //ImageProcプロパティ初期化
            myImageProc = ImageProcType.RoiNone;

            mybmp = new Bitmap(1280, 960 , PixelFormat.Format24bppRgb);

            // 描画スタイルの設定
            this.SetStyle(
                ControlStyles.DoubleBuffer |         // 描画をバッファで実行する
                ControlStyles.UserPaint |            // 描画は（ＯＳでなく）独自に行う
                ControlStyles.AllPaintingInWmPaint,  // WM_ERASEBKGND を無視する( UserPaint ビットが true に設定されている場合に適用） 
                 true                                // 指定したスタイルを適用「する」
                );

            //画像切り出し位置・サイズ・拡大倍率
            OffsetMaskX = CTSettings.iniValue.ExObsCamOffsetMaskX;
            OffsetMaskY = CTSettings.iniValue.ExObsCamOffsetMaskY;
            OffsetMaskWidth = CTSettings.iniValue.ExObsCamOffsetMaskWidth;
            OffsetMaskHeight = CTSettings.iniValue.ExObsCamOffsetMaskHeight;
            ZoomMagnify = CTSettings.iniValue.ExObsCamZoomMagnify;

            //画像切り出し位置・サイズ(ズーム時)
            ZoomOffsetMaskWidth = (int)(OffsetMaskWidth / ZoomMagnify);
            ZoomOffsetMaskHeight = (int)(OffsetMaskHeight / ZoomMagnify);

            ZoomOffsetMaskX = OffsetMaskX + (OffsetMaskWidth - ZoomOffsetMaskWidth) / 2;
            ZoomOffsetMaskY = OffsetMaskY + (OffsetMaskHeight - ZoomOffsetMaskHeight) / 2;

            //起動時表示領域用rect設定
            int startX = OffsetMaskX;
            int endX = OffsetMaskX + OffsetMaskWidth;
            int startY = OffsetMaskY;
            int endY = OffsetMaskY + OffsetMaskHeight;
            srcRectF = RectangleF.FromLTRB(startX, startY, endX - 1, endY -1);
            //dstRectF = new RectangleF(0, 0, 1024, 1024);
            if (DispFlg == 0)
            {
                dstRectF = new RectangleF(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            }
            else
            {
                dstRectF = new RectangleF(0, 0, 1024, 1024);
            }

            //ROI描画限界領域初期値
            int SelectScanMode = 0;
            //float Magnify = 0.0f;

            if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeFull || CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeHalf)
            {
                //SelectScanMode = CTSettings.scansel.Data.s_scan == 1 ? 0 + 3 : 0;
                //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                SelectScanMode = CTSettings.scansel.Data.w_scan == 1? 0 + 3 : 0;
            }
            //else if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift)
            //Rev25.00 修正 by長野 2016/07/07
            else if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeOffset)
            {
                //SelectScanMode = 1;
                //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                SelectScanMode = CTSettings.scansel.Data.w_scan == 1? 1 + 3 : 1;
            }
            else if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift)
            {
                //SelectScanMode = 2;
                //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                SelectScanMode = CTSettings.scansel.Data.w_scan == 1? 2 + 3 : 2;
            }
            else
            {
                SelectScanMode = 1;
            }

            //if (zoomflg == 0)
            //{
            //    Magnify = 1;
            //}
            //else
            //{
            //    Magnify = ZoomMagnify;
            //}

            //myLimitWidth = (int)((float)CTSettings.scancondpar.Data.ExObsCamMaxArea[zoomflg] / CTSettings.scancondpar.Data.ExObsCamPixSize[zoomflg]);
            //myLimitHeight = (int)((float)CTSettings.scancondpar.Data.ExObsCamMaxArea[zoomflg] / CTSettings.scancondpar.Data.ExObsCamPixSize[zoomflg]);

            //微調テーブル有無を考慮
            if (ftableOn == true)
            {
                myLimitWidth = (int)((float)CTSettings.scancondpar.Data.ExObsCamMaxArea[SelectScanMode] / CTSettings.scancondpar.Data.ExObsCamPixSizeOnFTable[zoomflg]);
                myLimitWidth = (myLimitWidth % 2 == 0 ? myLimitWidth - 1 : myLimitWidth);
                myLimitHeight = (int)((float)CTSettings.scancondpar.Data.ExObsCamMaxArea[SelectScanMode] / CTSettings.scancondpar.Data.ExObsCamPixSizeOnFTable[zoomflg]);
                myLimitHeight = (myLimitHeight % 2 == 0 ? myLimitHeight - 1 : myLimitHeight);
            }
            else
            {
                myLimitWidth = (int)((float)CTSettings.scancondpar.Data.ExObsCamMaxArea[SelectScanMode] / CTSettings.scancondpar.Data.ExObsCamPixSize[zoomflg]);
                myLimitWidth = (myLimitWidth % 2 == 0 ? myLimitWidth - 1 : myLimitWidth);
                myLimitHeight = (int)((float)CTSettings.scancondpar.Data.ExObsCamMaxArea[SelectScanMode] / CTSettings.scancondpar.Data.ExObsCamPixSize[zoomflg]);
                myLimitHeight = (myLimitHeight % 2 == 0 ? myLimitHeight - 1 : myLimitHeight);
            }
        }

        //*************************************************************************************************
        //機　　能： 各コントロールのキャプションに文字列をセットする
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.30  16/02/06   (検S1)長野   新規作成
        //*************************************************************************************************
        private void SetCaption()
        {
            this.Text = CTResources.LoadResString(24012);
        }

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
            this.Text = CTResources.LoadResString(24012);

            //Roi情報付加
            if (!string.IsNullOrEmpty(RoiInfo))
            {
                this.Text = this.Text + " " + RoiInfo;
            }
        }

        //*************************************************************************************************
        //機　　能： クローズ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.30  16/02/06   (検S1)長野   新規作成
        //*************************************************************************************************
        private void frmExObsCam_FormClosed(object sender, FormClosedEventArgs e)
        {
            CaptureStop();

            grfx.Dispose();
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

            if (!string.IsNullOrEmpty(RoiInfo) && myRoi.RoiMaxNum > 1)
            {
                workStr = Convert.ToString(myRoi.TargetRoi) + " " + workStr;
            }

            //キャプション更新
            UpdateCaption(workStr);

            if (myRoi.TargetRoi == 0)
            {
                mnuRoiInput.Enabled = false;
            }
            else if (myRoi.GetRoiShape(myRoi.TargetRoi) == RoiData.RoiShape.ROI_LINE)
            {
                mnuRoiInput.Enabled = true;
            }
            else
            {
                mnuRoiInput.Enabled = myRoi.IsSizable(myRoi.TargetRoi);
            }

            //ROI変更時処理
            if (!string.IsNullOrEmpty(RoiInfo))
            {
                if (RoiChanged != null)
                {
                    //RoiChanged();    
                    RoiChanged(RoiInfo, EventArgs.Empty);
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
        private bool ToolBarEnabled
        {
            set
            {
                //削除2015/01/22hata_ボタンを先に変更する
                //Toolbar1.Enabled = value;

                if (value)
                {
                    //変更2014/07/25(検S1)hata
                    //foreach (ToolStripButton Button in Toolbar1.Items)
                    //{
                    //	Button.Enabled = Convert.ToBoolean(Button.Tag);
                    //}
                    foreach (ToolStripItem Item in Toolbar1.Items)
                    {
                        if (Item.GetType() == typeof(ToolStripButton))
                        {
                            ToolStripButton btn = Item as ToolStripButton;
                            btn.Enabled = Convert.ToBoolean(btn.Tag);
                        }
                    }
                }
                else
                {
                    //変更2014/07/25(検S1)hata
                    //foreach (ToolStripButton Button in Toolbar1.Items)
                    //{
                    //	Button.Tag = Convert.ToString(Button.Enabled);
                    //	Button.Enabled = false;
                    //}
                    foreach (ToolStripItem Item in Toolbar1.Items)
                    {
                        if (Item.GetType() == typeof(ToolStripButton))
                        {
                            ToolStripButton btn = Item as ToolStripButton;
                            btn.Tag = Convert.ToString(btn.Enabled);
                            btn.Enabled = false;
                        }
                    }
                }
                //追加2015/01/22hata_ボタンを先に変更する
                Toolbar1.Enabled = value;

                //ROIメッセージフォームを有効にする
                frmRoiMessage.Instance.Enabled = value;
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
        public ImageProcType ImageProc
        {
            get { return myImageProc; }

            set
            {
                if (myImageProc != ImageProcType.RoiNone)
                {
                    modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTImageProcessing);
                }

                //処理の記憶
                myImageProc = value;

                if (myImageProc != ImageProcType.RoiNone)
                {
                    modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTImageProcessing;
                }

                //roiオブジェクトが存在する場合
                if (DrawRoi.roi != null)
                {
                    DrawRoi.roi.DeleteAllRoiData();		//いったんすべてのROIを削除
                    DrawRoi.roi = null;					//オブジェクト破棄

                    //追加2014/07/24hata
                    if (myRoi != null)
                    {
                        //Roi描画用イベント
                        myRoi.Changed -= new RoiData.ChangedEventHandler(myRoi_Changed);
                        myRoi = null;
                    }
                    this.Cursor = Cursors.Default;		//追加 2009/08/17
                    frmRoiMessage.Instance.Close();		//ROIメッセージをアンロード  'v15.10 追加 byやまおか 2009/11/30
                }

                //ROI制御スタートさせる場合
                if (myImageProc != ImageProcType.RoiNone)
                {
                    //ROIクラスの生成
                    DrawRoi.roi = new RoiData();

                    //描画対象フォームを設定
                    //DrawRoi.roi.SetTarget(this);
                    //Rev20.00 変更 by長野 2015/02/06
                    DrawRoi.roi.SetTarget(this, 1);

                    //追加2014/07/24hata
                    //roiの参照設定
                    myRoi = DrawRoi.roi;

                    //追加2014/07/24hata
                    //Roi描画用イベント
                    myRoi.Changed += new RoiData.ChangedEventHandler(myRoi_Changed);

                }

                //画像処理ごとの設定
                switch (myImageProc)
                {
                    //自動スキャン位置指定
                    case ImageProcType.RoiAutoPos:

                        //ROI描画フラグON
                        myROIDraw = true;

                        //変更2015/01/22hata
                        //tsbtnGo.Tag = CTResources.LoadResString(15204);		//自動スキャン位置指定
                        tsbtnGo.Text = CTResources.LoadResString(15204);		//自動スキャン位置指定
                        tsbtnCircle.Enabled = true;							//円
                        ClickToolBarButton(tsbtnCircle);					//「円」ボタンをクリックした状態にする
                        break;

                    default:
                        mnuRoiInput.Enabled = false;				//ROI座標入力
                        break;

                    #region 外観では不要のため抜いておく Rev23.21 by長野 2016/02/01
                    ////拡大処理
                    //case ImageProcType.roiEnlarge:
                    //    //変更2015/01/22hata
                    //    //tsbtnGo.Tag = CTResources.LoadResString(StringTable.IDS_EnlargeSimple);
                    //    tsbtnGo.Text = CTResources.LoadResString(StringTable.IDS_EnlargeSimple);
                    //    modImgProc.EnlargeRatio = 1;						//拡大率の初期化
                    //    if (!File.Exists(AppValue.OTEMPIMG))
                    //    {
                    //        File.Delete(AppValue.OTEMPIMG);
                    //    }
                    //    tsbtnSquare.Enabled = true;							//正方形
                    //    ClickToolBarButton(tsbtnSquare);					//正方形ボタンをクリックした状態にする
                    //    break;

                    ////CT値表示
                    //case ImageProcType.RoiCTDump:
                    //    //変更2015/01/22hata
                    //    //tsbtnGo.Tag = CTResources.LoadResString(StringTable.IDS_CTNumberDisp);	//ＣＴ値表示
                    //    tsbtnGo.Text = CTResources.LoadResString(StringTable.IDS_CTNumberDisp);	//ＣＴ値表示
                    //    myRoi.ManualCut = false;							//削除不可
                    //    tsbtnRectangle.Enabled = true;						//長方形
                    //    ClickToolBarButton(tsbtnRectangle);					//長方形ボタンをクリックした状態にする
                    //    if (hsbImage.Visible)
                    //    {
                    //        myRoi.SelectRoi(myRoi.AddRectangleShape2(511 + hsbImage.Value, 511 + vsbImage.Value, 8, 12, false));
                    //    }
                    //    else
                    //    {
                    //        myRoi.SelectRoi(myRoi.AddRectangleShape2(511, 511, 8, 12, false));
                    //    }
                    //    break;

                    ////ROI処理
                    //case ImageProcType.roiProcessing:
                    //    //変更2015/01/22hata
                    //    //tsbtnGo.Tag = CTResources.LoadResString(StringTable.IDS_ROIProcessing);
                    //    tsbtnGo.Text = CTResources.LoadResString(StringTable.IDS_ROIProcessing);
                    //    myRoi.RoiMaxNum = 20;								//描画可能数を最大値(=20)に設定
                    //    tsbtnCircle.Enabled = true;							//円
                    //    tsbtnRectangle.Enabled = true;						//長方形
                    //    //.Buttons("Square").Enabled = True                 '正方形     'v15.10追加 byやまおか 2009/11/30 'v15.10 ImageProcには正方形の処理が無いためコメントアウト by 長野
                    //    tsbtnTrace.Enabled = true;							//トレース
                    //    tsbtnOpen.Enabled = true;							//ROIテーブルを開く
                    //    tsbtnComment.Enabled = true;						//コメント
                    //    //変更2015/01/22hata
                    //    //tsbtnOpen.Tag = CTResources.LoadResString(StringTable.IDS_ROITable);	//ROIテーブルを開く
                    //    tsbtnOpen.Text = CTResources.LoadResString(StringTable.IDS_ROITable);	//ROIテーブルを開く

                    //    //ClickToolBarButton .Buttons("Rectangle")          '長方形ボタンをクリックした状態にする   'v15.10追加 byやまおか 2009/11/30 'v15.10 コメントを正方形→長方形に修正 by 長野 2010/2/25
                    //    //v19.11 ソフト起動中はROI形状を記憶する by長野 2013/02/20
                    //    switch (DrawRoi.RoiCalRoiNo)
                    //    {
                    //        case 1:
                    //            ClickToolBarButton(tsbtnCircle);
                    //            break;

                    //        case 2:
                    //            ClickToolBarButton(tsbtnRectangle);
                    //            break;

                    //        case 6:
                    //            ClickToolBarButton(tsbtnTrace);
                    //            break;

                    //        default:
                    //            ClickToolBarButton(tsbtnRectangle);
                    //            break;
                    //    }
                    //    break;

                    ////v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                    ////					'骨塩定量解析
                    ////					Case RoiBoneDensity
                    ////						.Buttons("Go").Description = LoadResString(IDS_BoneAnalysis)
                    ////						myRoi.RoiMaxNum = 5                                         'ROI描画可能数を５個に設定
                    ////						.Buttons("Circle").Enabled = True                           '円
                    ////						.Buttons("Rectangle").Enabled = True                        '長方形
                    ////						'.Buttons("Square").Enabled = True                           '正方形     'v15.10追加 byやまおか 2009/11/30 'v15.10 ImageProcには正方形の処理が無いためコメントアウト by 長野
                    ////						.Buttons("Trace").Enabled = True                            'トレース
                    ////						.Buttons("Open").Enabled = True                             'ROIテーブルを開く
                    ////						.Buttons("Comment").Enabled = True                          'コメント
                    ////						.Buttons("Open").Description = LoadResString(IDS_ROITable)  'ROIテーブルを開く
                    ////						'ClickToolBarButton .Buttons("Rectangle")                    '長方形ボタンをクリックした状態にする   'v15.10追加 byやまおか 2009/11/30
                    ////						'v19.11 ソフト起動中はROI形状を記憶する by長野 2013/02/20
                    ////						Select Case BoneDensityRoiNo
                    ////
                    ////						Case 1
                    ////
                    ////							ClickToolBarButton .Buttons("Circle")
                    ////
                    ////						Case 2
                    ////
                    ////							ClickToolBarButton .Buttons("Rectangle")
                    ////
                    ////						Case 6
                    ////
                    ////							ClickToolBarButton .Buttons("Trace")
                    ////
                    ////						Case Default
                    ////
                    ////							ClickToolBarButton .Buttons("Rectangle")
                    ////
                    ////						End Select
                    ////v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                    ////ヒストグラム
                    //case ImageProcType.roiHistgram:
                    //    //変更2015/01/22hata
                    //    //tsbtnGo.Tag = CTResources.LoadResString(StringTable.IDS_Histogram);
                    //    tsbtnGo.Text = CTResources.LoadResString(StringTable.IDS_Histogram);
                    //    modImgProc.CT_Bias = 0;										//CT値中央値
                    //    modImgProc.CT_Int = 10 * 45;								//1メモリ初期値（初期値450）
                    //    tsbtnCircle.Enabled = true;									//円
                    //    tsbtnRectangle.Enabled = true;								//長方形
                    //    tsbtnTrace.Enabled = true;									//トレース
                    //    //ClickToolBarButton .Buttons("Rectangle")                    '長方形ボタンをクリックした状態にする   'v15.10追加 byやまおか 2009/11/30
                    //    //v19.11 ソフト起動中はROI形状を記憶する by長野 2013/02/20
                    //    switch (DrawRoi.HistRoiNo)
                    //    {
                    //        case 1:
                    //            ClickToolBarButton(tsbtnCircle);
                    //            break;

                    //        case 2:
                    //            ClickToolBarButton(tsbtnRectangle);
                    //            break;

                    //        case 6:
                    //            ClickToolBarButton(tsbtnTrace);
                    //            break;

                    //        default:
                    //            ClickToolBarButton(tsbtnRectangle);
                    //            break;
                    //    }
                    //    break;

                    ////プロフィール
                    //case ImageProcType.roiProfile:
                    //    //変更2015/01/22hata
                    //    //tsbtnGo.Tag = CTResources.LoadResString(StringTable.IDS_Profile);
                    //    tsbtnGo.Text = CTResources.LoadResString(StringTable.IDS_Profile);
                    //    modImgProc.CT_Bias = 0;							//CT値中央値
                    //    modImgProc.CT_Int = 10 * 25;					//１目盛初期値（初期値250）
                    //    tsbtnLine.Enabled = true;						//線
                    //    tsbtnHLine.Enabled = true;						//水平線
                    //    tsbtnVLine.Enabled = true;						//垂直線
                    //    ClickToolBarButton(tsbtnLine);					//線ボタンをクリックした状態にする   'v15.10追加 byやまおか 2009/11/30
                    //    break;

                    ////プロフィールディスタンス
                    //case ImageProcType.RoiProfileDistance:
                    //    //変更2015/01/22hata
                    //    //tsbtnGo.Tag = CTResources.LoadResString(StringTable.IDS_ProfileDistance);
                    //    tsbtnGo.Text = CTResources.LoadResString(StringTable.IDS_ProfileDistance);
                    //    //CT_Low = 0                                                      'CT値最小初期値（初期値は0）
                    //    modImgProc.CT_Low = 100;										//CT値最小初期値（初期値は300）  'v15.10変更 byやまおか 2009/12/01
                    //    modImgProc.CT_High = 3000;										//CT値最大初期値（初期値は3000）
                    //    modImgProc.CT_Unit = 0;											//CT値連結幅初期値（初期値は0）
                    //    modImgProc.CT_Bias = 0;											//CT値中央値
                    //    modImgProc.CT_Int = 10 * 25;									//１目盛初期値（初期値250）
                    //    modImgProc.P1.x = 511;
                    //    modImgProc.P1.y = 511;
                    //    tsbtnLine.Enabled = true;										//線
                    //    tsbtnPoint.Enabled = true;										//点
                    //    tsbtnOpen.Enabled = true;										//プロフィールディスタンスを開く
                    //    //変更2015/01/22hata
                    //    //tsbtnOpen.Tag = CTResources.LoadResString(StringTable.IDS_PDTable);	//プロフィールディスタンステーブル
                    //    tsbtnOpen.Text = CTResources.LoadResString(StringTable.IDS_PDTable);	//プロフィールディスタンステーブル						
                    //    tsbtnComment.Enabled = true;									//コメント
                    //    ClickToolBarButton(tsbtnLine);									//線ボタンをクリックした状態にする   'v15.10追加 byやまおか 2009/11/30
                    //    break;

                    ////ズーミング
                    //case ImageProcType.roiZooming:
                    //    //変更2015/01/22hata
                    //    //tsbtnGo.Tag = CTResources.LoadResString(StringTable.IDS_Zooming);
                    //    tsbtnGo.Text = CTResources.LoadResString(StringTable.IDS_Zooming);
                    //    myRoi.RoiMaxNum = (frmImageInfo.Instance.IsConeBeam ? 1 : 20);			//コーンビーム画像の場合、ROIは1個だけしか描けなくする（通常は20個まで描ける）
                    //    tsbtnSquare.Enabled = true;										//正方形
                    //    tsbtnOpen.Enabled = true;										//ズーミングテーブルを開く
                    //    //変更2015/01/22hata
                    //    //tsbtnOpen.Tag = CTResources.LoadResString(StringTable.IDS_ZoomingTable);	//ズーミングテーブル
                    //    tsbtnOpen.Text = CTResources.LoadResString(StringTable.IDS_ZoomingTable);	//ズーミングテーブル
                    //    tsbtnComment.Enabled = true;									//コメント
                    //    ClickToolBarButton(tsbtnSquare);								//正方形ボタンをクリックした状態にする
                    //    break;

                    ////寸法測定
                    //case ImageProcType.roiDistance:
                    //    //変更2015/01/22hata
                    //    //tsbtnGo.Tag = CTResources.LoadResString(StringTable.IDS_SizeMeasurement);
                    //    tsbtnGo.Text = CTResources.LoadResString(StringTable.IDS_SizeMeasurement);
                    //    myRoi.RoiMaxNum = 20;											//ROI描画可能数は20個
                    //    tsbtnLine.Enabled = true;										//線
                    //    tsbtnComment.Enabled = true;									//コメント
                    //    ClickToolBarButton(tsbtnLine);									//「線」ボタンをクリックした状態にする
                    //    break;

                    ////角度測定
                    //case ImageProcType.RoiAngle:
                    //    //変更2015/01/22hata
                    //    //tsbtnGo.Tag = CTResources.LoadResString(12835);					//角度測定
                    //    tsbtnGo.Text = CTResources.LoadResString(12835);					//角度測定
                    //    tsbtnLine.Enabled = true;										//線
                    //    ClickToolBarButton(tsbtnLine);									//「線」ボタンをクリックした状態にする
                    //    break;

                    ////ROI制御終了
                    //default:
                    //    //変更2015/01/22hata
                    //    //tsbtnGo.Tag = "";
                    //    tsbtnGo.Text = "";
                    //    modImgProc.PRDPoint = 0;
                    //    //指定点数クリア

                    //    //座標入力フォームアンロード
                    //    frmInputRoiData.Instance.Close();
                    //    frmLineInput.Instance.Close();
                    //    HoriVertForm.Instance.Close();

                    //    //結果フォームをアンロード
                    //    frmResult.Instance.Close();

                    //    //ツールバー上のボタンをオフ・使用不可状態にする
                    //    foreach (ToolStripItem Item in Toolbar1.Items)
                    //    {
                    //        if (Item.GetType() == typeof(ToolStripButton))
                    //        {
                    //            ToolStripButton btn = Item as ToolStripButton;
                    //            btn.Checked = false;
                    //            btn.Enabled = false;
                    //        }
                    //    }

                    //    //右クリック時のメニューの調整
                    //    mnuROIEditCopy.Enabled = false;				//ｺﾋﾟｰ(&C)
                    //    mnuROIEditPaste.Enabled = false;			//貼り付け(&P)
                    //    mnuROIEditDelete.Enabled = false;			//ROI削除(&D)
                    //    mnuROIEditAllDelete.Enabled = false;		//すべてのROI削除(&A)
                    //    mnuRoiInput.Enabled = false;				//ROI座標入力
                    //    break;
                    #endregion
                }

                //共通設定
                if (myRoi != null)
                {
                    //変更2015/01/22hata
                    //tsbtnGo.ToolTipText = StringTable.GetResString(StringTable.IDS_Exe, tsbtnGo.Tag.ToString());
                    tsbtnGo.ToolTipText = StringTable.GetResString(StringTable.IDS_Exe, tsbtnGo.Text);
                    tsbtnArrow.Enabled = (myRoi.RoiMaxNum > 1);		//ROI選択
                    if (tsbtnOpen.Enabled)
                    {
                        //変更2015/01/22hata
                        //tsbtnSave.Tag = tsbtnOpen.Tag;
                        //tsbtnOpen.ToolTipText = StringTable.GetResString(StringTable.IDS_Open, Convert.ToString(tsbtnOpen.Tag));	//～を開く
                        //tsbtnSave.ToolTipText = StringTable.GetResString(StringTable.IDS_Save, Convert.ToString(tsbtnOpen.Tag));	//～の保存
                        tsbtnSave.Text = tsbtnOpen.Text;
                        tsbtnOpen.ToolTipText = StringTable.GetResString(StringTable.IDS_Open, tsbtnOpen.Text);	//～を開く
                        tsbtnSave.ToolTipText = StringTable.GetResString(StringTable.IDS_Save, tsbtnOpen.Text);	//～の保存
                    }
                }

                //「実行」ボタン、「キャンセル」ボタン、メッセージ欄の設定
                switch (myImageProc)
                {
                    case ImageProcType.RoiAutoPos:
                        //						cmdRoiOk.Enabled = False
                        //						cmdRoiCancel.Enabled = False
                        lblPrompt.Text = "";
                        break;

                    default:
                        //						cmdRoiOk.Enabled = True
                        //						cmdRoiCancel.Enabled = True
                        tsbtnGo.Enabled = true;
                        tsbtnExit.Enabled = true;
                        //変更2015/01/22hata
                        //lblPrompt.Text = Convert.ToString(tsbtnGo.Tag);
                        lblPrompt.Text = tsbtnGo.Text;
                        break;
                }
            }
        }
		//*******************************************************************************
		//機　　能： 「拡大」（もしくは「縮小」）選択処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 右クリック時に表示されるプルダウンメニュー処理
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public void mnuEnlarge_Click(object sender, EventArgs e)
		{
            //起動時表示領域用rect設定
            int startX = 0;
            int endX = 0;
            int startY = 0;
            int endY = 0;

            int SelectScanMode = 0;
            //float Magnify = 0;

            if (zoomflg == 0)
            {
                zoomflg = 1;
                mnuEnlarge.Text = CTResources.LoadResString(StringTable.IDS_btnReduction);
                startX = ZoomOffsetMaskX;
                endX = ZoomOffsetMaskX + ZoomOffsetMaskWidth;
                startY = ZoomOffsetMaskY;
                endY = ZoomOffsetMaskY + ZoomOffsetMaskHeight;
                srcRectF = RectangleF.FromLTRB(startX, startY - 1, endX, endY - 1);
                //ROI描画限界領域変更
                //if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeFull || CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeHalf)
                //{
                //    SelectScanMode = 0;
                //}
                //else if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift)
                //{
                //    SelectScanMode = 1;
                //}
                //else if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift)
                //{
                //    SelectScanMode = 2;
                //}
                //else
                //{
                //    SelectScanMode = 1;
                //}

                //Rev25.00 Wスキャン対応 by長野 2016/07/11

                if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeFull || CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeHalf)
                {
                    //SelectScanMode = CTSettings.scansel.Data.s_scan == 1 ? 0 + 3 : 0;
                    //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                    SelectScanMode = CTSettings.scansel.Data.w_scan == 1 ? 0 + 3 : 0;
                }
                //else if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift)
                //Rev25.00 修正 by長野 2016/07/07
                else if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeOffset)
                {
                    //SelectScanMode = 1;
                    //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                    SelectScanMode = CTSettings.scansel.Data.w_scan == 1 ? 1 + 3 : 1;
                }
                else if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift)
                {
                    //SelectScanMode = 2;
                    //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                    SelectScanMode = CTSettings.scansel.Data.w_scan == 1 ? 2 + 3 : 2;
                }
                else
                {
                    SelectScanMode = 1;
                }

                //if (zoomflg == 0)
                //{
                //    Magnify = 1;
                //}
                //else
                //{
                //    Magnify = ZoomMagnify;
                //}
                
                //微調テーブル有無を考慮
                if (ftableOn == true)
                {
                    myLimitWidth = (int)((float)CTSettings.scancondpar.Data.ExObsCamMaxArea[SelectScanMode] / CTSettings.scancondpar.Data.ExObsCamPixSizeOnFTable[zoomflg]);
                    myLimitWidth = (myLimitWidth % 2 == 0 ? myLimitWidth - 1 : myLimitWidth);
                    myLimitHeight = (int)((float)CTSettings.scancondpar.Data.ExObsCamMaxArea[SelectScanMode] / CTSettings.scancondpar.Data.ExObsCamPixSizeOnFTable[zoomflg]);
                    myLimitHeight = (myLimitHeight % 2 == 0 ? myLimitHeight - 1 : myLimitHeight);
                }
                else
                {
                    myLimitWidth = (int)((float)CTSettings.scancondpar.Data.ExObsCamMaxArea[SelectScanMode] / CTSettings.scancondpar.Data.ExObsCamPixSize[zoomflg]);
                    myLimitWidth = (myLimitWidth % 2 == 0 ? myLimitWidth - 1 : myLimitWidth);
                    myLimitHeight = (int)((float)CTSettings.scancondpar.Data.ExObsCamMaxArea[SelectScanMode] / CTSettings.scancondpar.Data.ExObsCamPixSize[zoomflg]);
                    myLimitHeight = (myLimitHeight % 2 == 0 ? myLimitHeight - 1 : myLimitHeight);
                }
            }
            else
            {
                zoomflg = 0;
                mnuEnlarge.Text = CTResources.LoadResString(StringTable.IDS_btnEnlarge);
                startX = OffsetMaskX;
                endX = OffsetMaskX + OffsetMaskWidth;
                startY = OffsetMaskY;
                endY = OffsetMaskY + OffsetMaskHeight;
                srcRectF = RectangleF.FromLTRB(startX, startY - 1, endX, endY - 1);
                //ROI描画限界領域変更
                //if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeFull || CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeHalf)
                //{
                //    SelectScanMode = 0;
                //}
                //else if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift)
                //{
                //    SelectScanMode = 1;
                //}
                //else if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift)
                //{
                //    SelectScanMode = 2;
                //}
                //else
                //{
                //    SelectScanMode = 1;
                //}

                //if (zoomflg == 0)
                //{
                //    Magnify = 1;
                //}
                //else
                //{
                //    Magnify = ZoomMagnify;
                //}

                //Rev25.00 Wスキャン対応 by長野 2016/07/11
                if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeFull || CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeHalf)
                {
                    //SelectScanMode = CTSettings.scansel.Data.s_scan == 1 ? 0 + 3 : 0;
                    //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                    SelectScanMode = CTSettings.scansel.Data.w_scan == 1 ? 0 + 3 : 0;
                }
                //else if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift)
                //Rev25.00 修正 by長野 2016/07/07
                else if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeOffset)
                {
                    //SelectScanMode = 1;
                    //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                    SelectScanMode = CTSettings.scansel.Data.w_scan == 1 ? 1 + 3 : 1;
                }
                else if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift)
                {
                    //SelectScanMode = 2;
                    //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                    SelectScanMode = CTSettings.scansel.Data.w_scan == 1 ? 2 + 3 : 2;
                }
                else
                {
                    SelectScanMode = 1;
                }

                //微調テーブル有無を考慮
                if (ftableOn == true)
                {
                    myLimitWidth = (int)((float)CTSettings.scancondpar.Data.ExObsCamMaxArea[SelectScanMode] / CTSettings.scancondpar.Data.ExObsCamPixSizeOnFTable[zoomflg]);
                    myLimitWidth = (myLimitWidth % 2 == 0 ? myLimitWidth - 1 : myLimitWidth);
                    myLimitHeight = (int)((float)CTSettings.scancondpar.Data.ExObsCamMaxArea[SelectScanMode] / CTSettings.scancondpar.Data.ExObsCamPixSizeOnFTable[zoomflg]);
                    myLimitHeight = (myLimitHeight % 2 == 0 ? myLimitHeight - 1 : myLimitHeight);
                }
                else
                {
                    myLimitWidth = (int)((float)CTSettings.scancondpar.Data.ExObsCamMaxArea[SelectScanMode] / CTSettings.scancondpar.Data.ExObsCamPixSize[SelectScanMode]);
                    myLimitWidth = (myLimitWidth % 2 == 0 ? myLimitWidth - 1 : myLimitWidth);
                    myLimitHeight = (int)((float)CTSettings.scancondpar.Data.ExObsCamMaxArea[SelectScanMode] / CTSettings.scancondpar.Data.ExObsCamPixSize[SelectScanMode]);
                    myLimitHeight = (myLimitHeight % 2 == 0 ? myLimitHeight - 1 : myLimitHeight);
                }
            }
		}

		//*******************************************************************************
		//機　　能： 「ROI座標入力」選択処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 右クリック時に表示されるプルダウンメニュー処理
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public void mnuRoiInput_Click(object sender, EventArgs e)
		{
			//ROI座標入力処理
            frmInputRoiData.Instance.IsSquare = tsbtnSquare.Enabled;
            if (!modLibrary.IsExistForm("frmInputRoiData"))	//追加2015/01/30hata_if文追加
            {
                frmInputRoiData.Instance.Show(frmCTMenu.Instance);
            }
            else
            {
                frmInputRoiData.Instance.WindowState = FormWindowState.Normal;
                frmInputRoiData.Instance.Visible = true;
            }
          
		}
        //*************************************************************************************************
		//機　　能： ツールバー上のボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*************************************************************************************************
		//private void Toolbar1_ButtonClick(object sender, ToolStripItemClickedEventArgs e)
		private void Toolbar1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
            ToolStripButton Button = e.ClickedItem as ToolStripButton;

            if (Button == null)
            {
                return;
            }

            string FileName = null;
            bool IsOK = false;

            //追加2014/07/24(検S1)hata
            //ROIグループのボタンの場合，前回クリックしたボタン戻す
            if (Button.CheckOnClick == true)
            {
                if (LastRoiButton != null) LastRoiButton.Checked = false;
            }

            switch (Button.Name)
            {
                //矢印ROI制御
                case "tsbtnArrow":
                    myRoi.ModeToPaint(RoiData.RoiShape.NO_ROI);
                    break;

                //円形ROI制御
                case "tsbtnCircle":
                    myRoi.ModeToPaint(RoiData.RoiShape.ROI_CIRC);
                    break;

                case "tsbtnGo":
                    GoRoi();
                    break;

                case "tsbtnExit":
                    ExitRoi();
                    break;
            }

            //ROIグループのボタンの場合，今回クリックしたボタンを記憶
            if (Button.CheckOnClick == true)
            {
                LastRoiButton = Button;
            }
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Button"></param>
        public void ClickToolBarButton(ToolStripButton button)
        {
            if (!button.CheckOnClick) button.Checked = true;
            button.PerformClick();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Button"></param>
        public void ClickToolBarButton(string ToolStripButtonKyeName)
        {
            int a = Toolbar1.Items.IndexOfKey(ToolStripButtonKyeName);
            ToolStripItem[] button1 = Toolbar1.Items.Find(ToolStripButtonKyeName, true);

            if (button1.Count() > 0)
            {
                ToolStripButton button;
                button = (ToolStripButton)button1[0];
                if (!button.CheckOnClick) button.Checked = true;
                button.PerformClick();
            }
        }

        //ツールバーのボタンがCheckされているか確認する。
        public bool GetToolBarChecked(string ToolStripButtonKyeName)
        {
            bool ret = false;
            int a = Toolbar1.Items.IndexOfKey(ToolStripButtonKyeName);
            ToolStripItem[] button1 = Toolbar1.Items.Find(ToolStripButtonKyeName, true);

            if (button1.Count() > 0)
            {
                ToolStripButton button;
                button = (ToolStripButton)button1[0];
                if (button.Checked)
                {
                    ret = true;
                }
            }
            return ret;
        }

        //*******************************************************************************
        //機　　能： picObj上のX座標位置を求める
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        public int GetXOnPicObj(float x)
        {
            int result = 0;

            //2014/11/07hata キャストの修正
            //result = (int)x;
            result = Convert.ToInt32(x);

            return result;
        }

        //*******************************************************************************
        //機　　能： picObj上のY座標位置を求める
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        public int GetYOnPicObj(float y)
        {
            int result = 0;

            //2014/11/07hata キャストの修正
            //result = (int)y;
            result = Convert.ToInt32(y);
  
            return result;
        }

        //*******************************************************************************
        //機　　能： Form上のX座標位置を求める
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        public float GetXOnForm(int x)
        {
            float resulte = 0;

            resulte = x;
            
            return resulte;
        }

        //*******************************************************************************
        //機　　能： Form上のY座標位置を求める
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        public float GetYOnForm(int y)
        {
            float result = 0;

            result = y;
           
            return result;
        }

        //*******************************************************************************
        //機　　能： 処理続行するかどうかの確認ダイアログを表示
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        //Private Sub GoRoi()
        public void GoRoi()
        {
            //測定するROIが登録されているか？
            if (myRoi.NumOfRois < 1)
            {
                //メッセージ表示：ROIが設定されていません。
                MessageBox.Show(CTResources.LoadResString(StringTable.IDS_NotFoundROI),
                                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //座標入力フォームをアンロード
            frmInputRoiData.Instance.Close();
            frmLineInput.Instance.Close();
            HoriVertForm.Instance.Close();

            //処理ごとに分岐
            switch (myImageProc)
            {
                case ImageProcType.RoiAutoPos:
                    DoAutoPos();
                    break;
            }
        }


        //*******************************************************************************
        //機　　能： 自動スキャン位置移動処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void DoAutoPos()
        {
            int xc = 0;
            int yc = 0;
            int r = 0;
            //Dim RoiCircle(2)    As Long
            //Dim PosFx           As Single
            //Dim PosFy           As Single
            //Dim PosFCD          As Single
            //Dim PosTableY       As Single
            //int err_sts = 0;
            float K = 0;

            bool IsOK = false;

            //変更2015/1/17hata_非表示のときにちらつくため
            //frmAutoScanPos.Instance.Hide();
            modCT30K.FormHide(frmAutoScanPos.Instance);

            //K = (float)frmImageInfo.Instance.Matrix / ((float)myPicWidth / (float)myMagnify);
            K = (float)1.0;

            //ツールバー上のボタンを触れないようにする
            ToolBarEnabled = false;

            //ROI座標取得
            myRoi.GetCircleShape(1, ref xc, ref yc, ref r);

            //'ROI設定
            //RoiCircle(0) = xc * K
            //RoiCircle(1) = yc * K
            //RoiCircle(2) = r * K

            //テーブル移動計算関数呼び出し
            //err_sts = autotbl_set(RoiCircle(0), PosFx, PosFy, PosFCD, PosTableY)
            //err_sts = autotbl_set(RoiCircle(0), PosFy, PosFx, PosFCD, PosTableY)    'v15.0変更 by 間々田 2009/06/16

            //変更 by 間々田 2009/07/09
            //2014/11/07hata キャストの修正
            //IsOK = frmMechaMove.Instance.MechaMoveForAutoScanPos((int)(xc * K), (int)(yc * K), (int)(r * K));
            //IsOK = frmMechaMove.Instance.MechaMoveForAutoScanPos_ExObsCam(Convert.ToInt32(xc * K), Convert.ToInt32(yc * K), Convert.ToInt32(r * K),1024,CTSettings.scancondpar.Data.ExObsCamPixSize[zoomflg]);
            
            IsOK = frmMechaMove.Instance.MechaMoveForAutoScanPos_ExObsCam(Convert.ToInt32(xc * K), Convert.ToInt32(yc * K), Convert.ToInt32(r * K), 1024, pixsize);


            //			'自動スキャン位置移動（微調テーブルのみ移動）
            //			ElseIf frmMechaMove.MechaMove(PosFx, PosFy, , , , , , True) Then
            //
            //			'テーブルのＸＹ座標を求める                                           '追加 by 間々田 2009/07/09
            //			err_sts = auto_tbl_set(RoiCircle(0), _
            //'                               PosFy, PosFx, _
            //'                               frmMechaControl.ntbFTablePosY.value, _
            //'                               frmMechaControl.ntbFTablePosX.value, _
            //'                               fcd1, table_h_xray1, _
            //'                               fcd2, table_h_xray2)
            //				'エラー？
            //				If err_sts <> 0 Then
            //
            //					ErrMessage err_sts
            //
            //				Else
            //
            //					'IsOK = frmMechaMove.MechaMove(PosFx, PosFy, PosFCD - scancondpar.fcd_offset(GetFcdOffsetIndex()), , PosTableY, , True)
            //					IsOK = frmMechaMove.MechaMove(, , fcd1 - scancondpar.fcd_offset(GetFcdOffsetIndex()), , table_h_xray1, , , True)
            //
            //				End If
            //
            //			End If

            //Rev26.40 add by chouno 2019/02/17
            frmMechaMove.Instance.Dispose();

            //ツールバー上のボタンを触れるようにする
            ToolBarEnabled = true;

            if (IsOK)
            {
                //自動スキャン位置フォームをアンロード
                frmAutoScanPos.Instance.Close();
            }
            else
            {
                //途中で自動移動を止めた場合は描画を再開する
                myROIDraw = true;

                if (!modLibrary.IsExistForm("frmAutoScanPos"))	//追加2015/01/30hata_if文追加
                {
                    frmAutoScanPos.Instance.Show(frmCTMenu.Instance);
                }
                else
                {
                    frmAutoScanPos.Instance.WindowState = FormWindowState.Normal;
                    frmAutoScanPos.Instance.Visible = true;
                }
            }
        }

        //*******************************************************************************
        //機　　能： 処理続行するかどうかの確認ダイアログを表示
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        //Private Sub ExitRoi()
        public void ExitRoi()		//15.10変更 byやまおか 2009/11/30
        {
            try
            {
                //ROI制御終了
                this.ImageProc = ImageProcType.RoiNone;
            }
            catch
            {
            }

            //マウスポインタを元に戻す
            this.Cursor = Cursors.Default;
        }

        private void frmExObsCam_DoubleClick(object sender, EventArgs e)
        {
            DoubleClickOn = (LastButton == MouseButtons.Left);
        }

        private void frmExObsCam_MouseUp(object sender, MouseEventArgs e)
        {
            //Roi制御なしの場合、何もしない
            if (myRoi == null)
            {
                return;
            }

            //座標補正
            //int X = modLibrary.CorrectInRange(e.X, 0, hsbImage.Width - 1);
            //int Y = modLibrary.CorrectInRange(e.Y, 0, vsbImage.Height - 1);
            int X = modLibrary.CorrectInRange(e.X, 0, 1023);
            int Y = modLibrary.CorrectInRange(e.Y, 0, 1023);

            //テスト追加2014/07/14hata_<変更>
            //int shift = 0;
            //if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            //{
            //    shift = 1;
            //}

            //Roi制御時のマウスアップ処理
            //テスト追加2014/07/14hata_<変更>
            myRoi.MouseUp(e.Button, Control.ModifierKeys, GetXOnPicObj(X), GetYOnPicObj(Y), DoubleClickOn);

            //今回クリックしたマウスボタン（ダブルクリック時に使用）
            LastButton = e.Button;

            //ダブルクリックフラグオフ
            DoubleClickOn = false;
        }

        private void frmExObsCam_MouseDown(object sender, MouseEventArgs e)
        {
            //Roi制御なしの場合、何もしない
            if (myRoi == null)
            {
                //右クリック時
                if (e.Button == MouseButtons.Right)
                {
                    //ポップアップメニューを表示
                    // スキャン制御画面のスクリーン上での領域を求める
                    var p1 = this.PointToScreen(new Point(e.X, e.Y));
                    mnuPopUp.Show(p1);
                }
            }
            else
            {
                //テスト追加2014/07/14hata_<変更>
                //int shift = 0;
                //if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                //{
                //    shift = 1;
                //}				
                //Roi制御時のマウスダウン処理
                //myRoi.MouseDown(e.Button, shift, GetXOnPicObj(e.X), GetYOnPicObj(e.Y));
                myRoi.MouseDown(e.Button, Control.ModifierKeys, GetXOnPicObj(e.X), GetYOnPicObj(e.Y));

            }
        }

        private void frmExObsCam_MouseMove(object sender, MouseEventArgs e)
        {
            //Roi制御なしの場合、何もしない
            if (myRoi == null)
            {
                return;
            }

            //座標補正
            //int X = modLibrary.CorrectInRange(e.X, 0, hsbImage.Width - 1);
            //int Y = modLibrary.CorrectInRange(e.Y, 0, vsbImage.Height - 1);
            int X = modLibrary.CorrectInRange(e.X, 0, 1023);
            int Y = modLibrary.CorrectInRange(e.Y, 0, 1023);

            //Roi制御時のマウスムーブ処理
            myRoi.MouseMove(GetXOnPicObj(X), GetYOnPicObj(Y));
        }

        private void frmExObsCam_Paint(object sender, PaintEventArgs e)
        {

            //Roi制御なしの場合、何もしない
            if (myRoi == null) return;

            Graphics g = e.Graphics;

            //補間モード
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            if (myROIDraw == true)
            {
                //Roiの表示
                myRoi.DispRoi(g);
            }
            //Graphics g = e.Graphics;

            //////補間モード
            ////g.InterpolationMode = InterpolationMode.NearestNeighbor;

            ////Roi制御なしの場合、何もしない
            //if (myRoi == null)
            //{
            //    return;
            //}
            ////Roiの表示
            //myRoi.DispRoi(g);


            //画像の描画
            //DoPaintPicture(e.Graphics);

            ////Roi制御なしの場合、何もしない
            //if (myRoi == null)
            //{
            //    return;
            //}

            ////Roiの表示
            //myRoi.DispRoi(g);
        }

        private void Toolbar1_MouseEnter(object sender, EventArgs e)
        {
            if (Toolbar1.Visible && Toolbar1.Enabled)
                Toolbar1.Focus();
        }


        /// <summary>
        /// WndProc メソッドをオーバーライドする：フォームを移動させないための措置
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;
            const int SC_MASK = 0xFFF0;

            if (m.Msg == WM_SYSCOMMAND)
            {
                int status = m.WParam.ToInt32() & SC_MASK;

                //ロックフラグON場合のみ監視
                if (myFrmLockFlg == 1)
                {
                    //フォームの移動を捕捉したら以降の制御をカットする
                    if (status == SC_MOVE) return;
                }
            }

            //基本クラスのメソッドを実行する
            base.WndProc(ref m);
        }

        ////private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        ////{
        ////    DoubleClickOn = (LastButton == MouseButtons.Left);
        ////}
        //[System.Security.Permissions.PermissionSet(
        // System.Security.Permissions.SecurityAction.Demand,
        // Name = "FullTrust")]
        //protected override void WndProc(ref Message m)
        //{
        //    switch (m.Msg)
        //    {
        //        case StCam.WM_STCAM_TRANSFER_START:
        //            m_bStatusTransfer = true;
        //            //StatusChanged();
        //            break;
        //        case StCam.WM_STCAM_TRANSFER_FINISH:
        //            m_bStatusTransfer = false;
        //            //StatusChanged();
        //            break;
        //        case StCam.WM_STCAM_AVI_FILE_START:
        //            m_bStatusAVIFile = true;
        //            //StatusChanged();
        //            break;
        //        case StCam.WM_STCAM_AVI_FILE_FINISH:
        //            m_bStatusAVIFile = false;
        //            //StatusChanged();
        //            break;
        //        case StCam.WM_STCAM_PREVIEW_WINDOW_CREATE:
        //            m_bStatusPreviewWnd = true;
        //            //StatusChanged();
        //            break;
        //        case StCam.WM_STCAM_PREVIEW_WINDOW_CLOSE:
        //            m_bStatusPreviewWnd = false;
        //            //StatusChanged();
        //            break;
        //        case StCam.WM_STCAM_UPDATED_PREVIEW_IMAGE:
        //            this.UpdateA();
        //            break;
        //        default:
        //            base.WndProc(ref m);
        //            break;
        //    }
        //}
    }
        #endregion
}
