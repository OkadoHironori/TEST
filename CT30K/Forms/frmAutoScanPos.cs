using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace CT30K
{
    public partial class frmAutoScanPos : Form
    {
        #region インスタンスを返すプロパティ

        // frmAutoScanPosのインスタンス
        private static frmAutoScanPos _Instance = null;

        /// <summary>
        /// frmAutoScanPosのインスタンスを返す
        /// </summary>
        public static frmAutoScanPos Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmAutoScanPos();
                }

                return _Instance;
            }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmAutoScanPos()
        {
            InitializeComponent();
        }

        //Rev23.30 位置指定モード by長野 2016/02/06
        private int myAutoScanPosMode = 0; //(0:CT画像,1:外観カメラ画像)
        public int AutoScanPosMode
        {
            get
            {
                return myAutoScanPosMode;
            }
            set
            {
                myAutoScanPosMode = value;
            }
        }

        #endregion

        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************
        int myUnloadMode = 0;   //アンロードモード

        //*******************************************************************************
        //機　　能： 「キャンセル」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdCancel_Click(object eventSender, EventArgs e)
        {
            //アンロード処理
            this.Close();
        }

        //*******************************************************************************
        //機　　能： 「ＯＫ」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdOk_Click(object eventSender, EventArgs e)
        {
            //v15.0追加 by 間々田 2009/07/16
            //modMecainf.GetMecainf(ref modMecainf.mecainf);
            CTSettings.mecainf.Load();

            CTSettings.mecainf.Data.iifield = modSeqComm.GetIINo();                  //I.I.視野
            CTSettings.mecainf.Data.table_x_pos = (float)frmMechaControl.Instance.ntbTableXPos.Value;   //試料テーブル（光軸と垂直方向)座標[mm] （従来のＸ軸座標）も書き込む
            
            //modMecainf.PutMecainf(ref modMecainf.mecainf);
            CTSettings.mecainf.Write();

            //FID/FCDをコモンに書き込む  'v15.0追加 by 間々田 2009/06/16
            CTSettings.scansel.Data.fid = frmMechaControl.Instance.FIDWithOffset;    //FID
            CTSettings.scansel.Data.fcd = frmMechaControl.Instance.FCDWithOffset;    //FCD
            
            //modScansel.PutScansel(ref modScansel.scansel);
            CTSettings.scansel.Write();

            //Rev23.30 モードに従い、ROI処理を実行 by長野 2016/02/06
            if (this.AutoScanPosMode == 0)
            {
                frmScanImage.Instance.GoRoi();
            }
            else
            {
                frmExObsCam.Instance.GoRoi();
            }
        }

        //*******************************************************************************
        //機　　能： フォームロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void frmAutoScanPos_Load(object sender, EventArgs e)
        {
            //フォームの位置設定
            //SetPosToolForm Me
            modCT30K.SetPosNormalForm(this);    //v15.10変更 byやまおか 2009/12/01
            this.SetBounds(this.Left, frmScanControl.Instance.Top - this.Height, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            //表示コメントを設定     'v15.10追加　byやまおか 2009/11/26
            //ストリングテーブル化　'v17.60 by 長野 2011/05/22
            //lblComment.Caption = "断面像上にROIを描画してから" + vbNewLine + "OKボタンを押してください。"
            //Rev23.32 修正 by長野 2016/03/30
            //lblComment.Text = CTResources.LoadResString(20005);

            //Rev23.30 モードに従いROI制御スタート by長野 2016/02/06
            if (this.AutoScanPosMode == 0)
            {
                //Rev23.32 修正 by長野 2016/03/30
                lblComment.Text = CTResources.LoadResString(20005);

                //CT画像上でROI制御スタート（自動スキャン位置指定モードにする）スタート
                frmScanImage.Instance.ImageProc = frmScanImage.ImageProcType.RoiAutoPos;
            }
            else
            {
                //Rev23.32 修正 by長野 2016/03/30
                lblComment.Text = CTResources.LoadResString(24015);

                //ROI制御中は、外観制御enableをfalse
                frmCTMenu.Instance.tsbtnExObsCam.Enabled = false;

                //外観カメラ画像上でROI制御スタート（自動スキャン位置指定モードにする）スタート
                frmExObsCam.Instance.ImageProc = frmExObsCam.ImageProcType.RoiAutoPos;
            }
        }

        //*******************************************************************************
        //機　　能： QueryUnload イベント処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void frmAutoScanPos_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool Cancel = e.Cancel;
            CloseReason UnloadMode = e.CloseReason;

            //アンロードモードの保持
            myUnloadMode = (int)UnloadMode;

            e.Cancel = Cancel;
        }

        //*******************************************************************************
        //機　　能： フォームアンロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void frmAutoScanPos_FormClosed(object sender, FormClosedEventArgs e)
        {
            //プログラム終了要求によるアンロードの場合は以後の処理は行わない
            //if (!(myUnloadMode == (int)CloseReason.ApplicationExitCall) || (myUnloadMode == (int)CloseReason.UserClosing))
            //{
            //    return;
            //}
            if (!(myUnloadMode == (int)CloseReason.UserClosing))
            {
                return;
            }
            
            //Rev23.30 モードに従い、終了させる by長野 2016/02/03
            if (this.AutoScanPosMode == 0)
            {
                //CT画像上でROI制御終了
                frmScanImage.Instance.ImageProc = frmScanImage.ImageProcType.RoiNone;
            }
            else
            {
                //外観カメラ画像上でROI制御終了
                frmExObsCam.Instance.ImageProc = frmExObsCam.ImageProcType.RoiNone;

                //del Rev26.40 by chouno 2019/03/12
                //Rev25.00 付帯情報画面表示 by長野 2016/08/22
                //frmImageInfo.Instance.Show(frmCTMenu.Instance);

                //Rev23.30 追加 外観カメラ起動 by長野 2016/02/06
                if (frmExObsCam.Instance.CamHandle != IntPtr.Zero)
                {
                    frmExObsCam.Instance.CaptureStop();

                    Application.DoEvents();

                    frmExObsCam.Instance.CloseCamera();

                    Application.DoEvents();

                    frmExObsCam.Instance.Close();
                    frmExObsCam.Instance.Dispose();
                }

                frmExObsCam.Instance.ExObsCam.ExObsCamProcessStart(DispFlg: 0);

                //ROI制御後は外観制御enableをtrue
                frmCTMenu.Instance.tsbtnExObsCam.Enabled = true;
            }
        }
    }
}
