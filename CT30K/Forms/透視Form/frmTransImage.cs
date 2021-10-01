using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
 
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmTransImage myForm = null;

        //透視画像の配列（「元に戻す」用）
        short[] FImageOrg;
        //作業用配列
        short[] WorkImage;
        //シャープフィルタ用配列
        float[] SharpFilter;

        //Dim adv                 As Long
        //v17.00追加　山本 2009-09-29
        public int adv;

        //このフォームで使用する線
        public enum LineConstants
        {
            ScanLine,
            //スキャンライン
            UpperLine,
            //コーンビーム時の上端ライン
            LowerLine,
            //コーンビーム時の下端ライン
            CenterLine,
            //中心線（縦）
            CenterLineH
            //中心線（横）   'v15.10追加 byやまおか 2009/10/22
        }

        //線
        //v15.10変更 byやまおか 2009/10/22
        //LineStruct[] myLine = new LineStruct[LineConstants.CenterLineH + 1];

        //ハンドル対象ライン
        LineConstants myHandleLine;

        Point myHandleLineP1;
        Point myHandleLineP2;

        //マウスダウン時のポイント座標
        Point MouseDownPoint;

        //MILのハンドル
        //public int hMil;

        //透視画像領域（共有メモリ）のハンドル
        private int hMap;
 
        /*
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
        */

        //ROI制御タイプ
        public enum TransImageProcType
        {
            TransRoiNone,
            TransRoiAutoPos
        }

        //ROI制御タイプ変数
        TransImageProcType myTransImageProc;

        //Target プロパティ用
        string myTarget;

        //サポートしているイベント
        public event RoiChangedEventHandler RoiChanged;
        public delegate void RoiChangedEventHandler();
        public event TransImageChangedEventHandler TransImageChanged;
        public delegate void TransImageChangedEventHandler();
        //透視画像が変更された
        public event CaptureOnOffChangedEventHandler CaptureOnOffChanged;
        //キャプチャオンオフ変更時イベント
        public delegate void CaptureOnOffChangedEventHandler(bool IsOn);

        


        //ズーミング比率 1:同倍 2:1/2
        int myZoomScale;

        //キャプキャ画像のデータサイズ
        int myDataSize;

        //フレームレート
        int myFrameRate;       
        
        #endregion


        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmTransImage()
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

            #region フォームロード時の処理
            this.Load += (sender, e) =>
            {
                //コントロールの初期化
                InitControls();
            };

            #endregion        
        
        }
        #endregion


        #region コントロールの初期化
        /// <summary>
        /// コントロールの初期化
        /// </summary>
        public void InitControls()
        {
            int fWidth = this.Width - this.ClientSize.Width;
            int fHeight = this.Height - this.ClientSize.Height;

            this.Width = fWidth + CTSettings.detectorParam.h_size;
            this.Height = fHeight + CTSettings.detectorParam.v_size;

        }

        public void Init(DetectorParam param)
        {
            transImageCtrl = new TransImageControl(param);
            //描画用イベント
            transImageCtrl.TransImageChanged += new EventHandler(transImageCtrl_TransImageChanged);

            ctImageCanvas1.Width = transImageCtrl.Detector.h_size;
            ctImageCanvas1.Height = transImageCtrl.Detector.v_size;
            ctImageCanvas1.Top = 0;
            ctImageCanvas1.Left = 0;
        }
        #endregion

        //描画用イベント処理
        void transImageCtrl_TransImageChanged(object sender, EventArgs e)
        {
            // 描画
            ctImageCanvas1.DispPicture(transImageCtrl.Picture);
        }


        /// <summary>
        /// 透視画像処理
        /// </summary>
        public TransImageControl TransImageCtrl { get { return transImageCtrl; } }



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
	public int FrameRate {
        
		get { return myFrameRate; }
		set 
        {

			myFrameRate = value;

/* 工事中         
            //常時動作しているタイマーを調整する
			frmCTMenu.tmrStatus.Enabled = (myFrameRate == 15);
			frmMechaControl.tmrSeqComm.Interval = (myFrameRate > 15 ? 2000 : 500);
			frmMechaControl.tmrMecainf.Interval = (myFrameRate > 15 ? 2000 : 1000);
*/
			//同期・非同期の設定
			 transImageCtrl.CaptureOpen();

			#if NoCamera  //v17.00追加(ここから) byやまおか 2010/01/19
			#else
			
            if ((transImageCtrl.Detector.DetType == DetectorConstants.DetTypeII) | (transImageCtrl.Detector.DetType == DetectorConstants.DetTypeHama)) {
				int mode =  ((CTSettings.scanParam.gFrameRateIndex  == 1) ? Pulsar.M_ASYNCHRONOUS : Pulsar.M_SYNCHRONOUS);
                Pulsar.MilSetGrabMode(Pulsar.hMil, mode);
				
			}
			#endif //v17.00追加(ここまで) byやまおか 2010/01/19

		}
	}

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
                transImageCtrl.CaptureStart();
            }
            else
            {
                transImageCtrl.CaptureStop();
            }

        }
    }


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

            //ファイル名をタイトルバーに表示
            //UpdateCaption();

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

            //ちらつき防止のため 'v17.10追加 byやまおか 2010/08/25
            this.Hide();
/* 工事中
            frmTransImageInfo.Hide();
            frmTransImageControl.Hide();

            //コントロールの再設定                       '追加 by 山本　2005-8-24
            ResizeImage();

            //ちらつき防止のため 'v17.10追加 byやまおか 2010/08/25
            this.Show();
            frmTransImageInfo.Show();
            frmTransImageControl.Show();

            //付帯情報のスケールバーを調整する
            if (IsExistForm(frmTransImageInfo))
                frmTransImageInfo.UpdateScaleBar();
 */
        }
 
    }






















        ///// <summary>
        ///// ＊＊＊　使用しない
        ///// </summary>
        //public bool CaptureOn { get; set; }

        ///// <summary>
        ///// 工事中
        ///// </summary>
        //public int ZoomScale { get; set; }

        /// <summary>
        /// 工事中
        /// </summary>
        //public string Target
        //{ 
        //    get;
        //    set{UndateCaption();}
        //}    

        /// <summary>
        /// 工事中
        /// </summary>
        public void UndateCaption() 
        {
        
        
        }

        /// <summary>
        /// 工事中
        /// </summary>
        public void LoadFromFile(string fileName) { }

        /// <summary>
        /// 工事中
        /// </summary>
        public void SaveToFile(string fileName, bool infSave) { }

        /// <summary>
        /// 工事中
        /// </summary>
        public void Backup() { }

        /// <summary>
        /// 工事中
        /// </summary>
        public void Inverse() { }

        /// <summary>
        /// 工事中
        /// </summary>
        public void Undo() { }


        ///// <summary>
        ///// 工事中
        ///// </summary>
        //public void SetLineVisible(LineConstants line, bool dummy) { }

    }
}
