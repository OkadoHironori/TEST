using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using CT30K.Properties;
using CTAPI;
using TransImage;

namespace CT30K
{

    //public partial class frmFInteg : BaseDialog
    //Rev20.00 変更 by長野 2015/02/06
    public partial class frmFInteg : Form
    {
     ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： frmFInteg.frm                                               */
    ///* 処理概要　　： ??????????????????????????????                              */
    ///* 注意事項　　： なし                                                        */
    ///* -------------------------------------------------------------------------- */
    ///* 適用計算機　： DOS/V PC                                                    */
    ///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
    ///* コンパイラ　： VB 6.0                                                      */
    ///* -------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
    ///*                                                                            */
    ///* V1.00       99/XX/XX    (TOSFEC) ????????   新規作成                       */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
        #region メンバ変数
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmFInteg myForm = null;

        //積分用配列
        private int[] WorkImageLong;
        private ushort[] WorkImage;

        //フォーム参照用
        private frmTransImage myTransImage;      //透視画像
        private TransImageControl myTransCtrl;
        //private ScanParam myScanParam;

        //IsBusyプロパティ用変数
        bool myBusy;

        //Rev20.00 追加 by長野 2015/02/24
        private int myFInteg;
        
        #endregion

        #region サポートしているイベント
        //サポートしているイベント
        public event UnloadedEventHandler Unloaded;
        public delegate void UnloadedEventHandler();        //アンロードされた     
        delegate void IntegralUpdateDelegate();

        #endregion


        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmFInteg()
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
        public static frmFInteg Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmFInteg();
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
            //cmdEnd.Click += (sender, e) =>
            //{
            //    this.Close();
            //};

            //this.Load += (sender, e) =>
            //{
            //    //フォームの参照
            //    myTransImage = frmTransImage.Instance;	//メカ制御
            //    myTransImage.TransImageChanged += myTransImage_TransImageChanged;
            //};

        }
        #endregion



        //*************************************************************************************************
        //機　　能： IsBusyプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************

        private bool IsBusy
        {


            get { return myBusy; }
            set
            {

                //設定値を保存
                myBusy = value;

                //各コントロールのEnabledプロパティを制御する
                cmdExe.Enabled = !myBusy;
                //実行ボタン
                cmdEnd.Enabled = !myBusy;
                //閉じるボタン
                hsbInteg.Enabled = !myBusy;
                //スクロールバー

                //「積分中」ラベル
                lblIntegStatus.Visible = myBusy;

                //プログレスバー
                if (myBusy)
                {
                    progressBar1.Value = 0;
                    progressBar1.Maximum = hsbInteg.Value;
                    progressBar1.Visible = true;
                }
                else
                {
                    progressBar1.Visible = false;
                }

                //マウスポインタの制御
                this.Cursor = (myBusy ? Cursors.WaitCursor : Cursors.Default);

            }
        }

        //*************************************************************************************************
        //機　　能： IntegNumプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        public int IntegNum
        {
            //get { return hsbInteg.Value; }
            //Rev20.00 変更 by長野 2015/02/24
            get { return myFInteg; }
            set { myFInteg = hsbInteg.Value; }
        }

        //*******************************************************************************
        //機　　能： 積分枚数スクロールバー・スクロール時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
         private void hsbInteg_Scroll(object sender, ScrollEventArgs e)
        {
            //値の表示
            lblInteg.Text = StringTable.GetResString(StringTable.IDS_Frames, e.NewValue.ToString());

        }

        //*******************************************************************************
        //機　　能： 積分枚数スクロールバー・スクロール時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void hsbInteg_ValueChanged(object sender, EventArgs e)
        {
            //値の表示
            lblInteg.Text = StringTable.GetResString(StringTable.IDS_Frames, hsbInteg.Value.ToString());
        }


        //*******************************************************************************
        //機　　能： 「実行」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void cmdExe_Click(System.Object eventSender, System.EventArgs eventArgs)
        {

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //    'RAMディスクが構築されているかどうか  'v17.40追加 byやまおか 2010/10/26
            //    'If UseRamDisk And (Not RamDiskIsReady) Then Exit Sub
            //    If UseRamDisk Then      'v17.42修正 byやまおか 2010/11/04
            //        If (Not RamDiskIsReady) Then Exit Sub
            //    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //積分用配列初期化 
            //Debug.Print(myTransImage.TransImageCtrl.ImageSize.Width.ToString());

            int iSize = myTransImage.TransImageCtrl.ImageSize.Width * myTransImage.TransImageCtrl.ImageSize.Height;
            WorkImageLong = new int[iSize];
            WorkImage = new ushort[iSize];

            //取り込み開始
            //Rev20.00 追加 by長野 2015/02/25
            IntegNum = hsbInteg.Value;
            //myTransCtrl.S.FIntegNum = hsbInteg.Value;
            CTSettings.scanParam.FIntegNum = hsbInteg.Value;
            myTransImage.TransImageCtrl.SetScanParam(CTSettings.scanParam);

            ////FluoroIP.IntegOn = true;
            //myTransImage.TransImageCtrl.IntegOn = true;

            //My.MyProject.Forms.frmTransImage.CaptureOn = true;
            myTransImage.TransImageCtrl.CaptureStart();

            //FluoroIP.IntegOn = true;
            myTransImage.TransImageCtrl.IntegOn = true;


            progressBar1.Value = 0;

            //ビジープロパティをオン
            IsBusy = true;

        }

        //*******************************************************************************
        //機　　能： 「閉じる」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void cmdEnd_Click(System.Object eventSender, System.EventArgs eventArgs)
        {

            //アンロードする
            this.Close();

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
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void frmFInteg_Load(System.Object eventSender, System.EventArgs eventArgs)
        {

            
            //実行時はフラグをセット
            modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTIntegImage;
            
            //英語用レイアウト調整 by長野 2011/05/25
            if (modCT30K.IsEnglish)
            {
                EnglishAdjustLayout();
            }
            
            //Rev20.00 falseに変更 by長野 2015/02/06
            //this.ControlBox = true;
            this.ControlBox = false;

            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            //this.Text= StringTable.GetResString("STR_" + (string)this.Tag);
            StringTable.LoadResStrings(this);

            //追加2014/12/22hata_dNet
            //Maxの設定
            int integmax = 256;
            hsbInteg.Maximum = integmax + hsbInteg.LargeChange - 1;

            //積分枚数の設定
            //hsbInteg.Value = FluoroIP.FIntegNum;

            //変更2014/12/22hata_dNet
            //if (CTSettings.scanParam.FIntegNum > hsbInteg.Maximum) CTSettings.scanParam.FIntegNum = hsbInteg.Maximum;
            if (CTSettings.scanParam.FIntegNum > integmax) CTSettings.scanParam.FIntegNum = integmax;

            if (CTSettings.scanParam.FIntegNum < hsbInteg.Minimum) CTSettings.scanParam.FIntegNum = hsbInteg.Minimum;
            hsbInteg.Value = CTSettings.scanParam.FIntegNum;
            
            //透視画像フォームへの参照を設定
            myTransImage = frmTransImage.Instance;	//メカ制御
            myTransCtrl = myTransImage.TransImageCtrl;
            
            //描画用イベント
            //myTransCtrl.TransImageChanged += new EventHandler(myTransImage_TransImageChanged);
            myTransCtrl.IntegralCountUp += new TransPicture.IntegralCountUpHandler( myTransImage_IntegralCountUp);

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
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void frmFInteg_FormClosed(System.Object eventSender, System.Windows.Forms.FormClosedEventArgs eventArgs)
        {
            
            //積算枚数保存
            //FluoroIP.FIntegNum = hsbInteg.Value;
            CTSettings.scanParam.FIntegNum = hsbInteg.Value;
            
            //終了時はフラグをリセット
            //modCTBusy.CTBusy = modCTBusy.CTBusy & (!modCTBusy.CTIntegImage);
            modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTIntegImage);

            //描画用イベント
            //myTransCtrl.TransImageChanged -= new EventHandler(myTransImage_TransImageChanged);
            myTransCtrl.IntegralCountUp -= new TransPicture.IntegralCountUpHandler(myTransImage_IntegralCountUp);
            //myTransImage.Dispose();
           
            //透視画像フォームへの参照を破棄
            myTransImage = null;
 
            //イベント生成（アンロードされた）
            if (Unloaded != null)
            {
                Unloaded();
            }

        }


        //*******************************************************************************
        //機　　能： 英語用レイアウト調整
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.60  2011/05/25 (検S1)長野   新規作成
        //*******************************************************************************
        private void EnglishAdjustLayout()
        {

            this.Width = 400;                   //6000 / 15;

            //Rev20.01 変更 by長野 2015/05/19
            //lblInteg.Left = lblInteg.Left + 33; //500/15
            //hsbInteg.Left = hsbInteg.Left + 40; //600/15

            //lblMin.Left = hsbInteg.Left + 11;
            //lblMax.Left = hsbInteg.Left + hsbInteg.Width - lblMax.Width;

            lblInteg.Left = lblInteg.Left + 36; //500/15
            hsbInteg.Left = hsbInteg.Left + 43; //600/15

            lblMin.Left = hsbInteg.Left + 11;
            lblMax.Left = hsbInteg.Left + hsbInteg.Width - lblMax.Width + 3;

            lblInteg.TextAlign = ContentAlignment.TopRight;

            //2014/11/06hata キャストの修正
            lblInteg.Width = Convert.ToInt32(lblInteg.Width * 1.2F);
            int margin = Convert.ToInt32((this.ClientRectangle.Width - cmdExe.Width - cmdEnd.Width) / 3F);

            cmdExe.Left = margin;
            cmdEnd.Left = cmdExe.Left + cmdExe.Width + margin;
            //lblIntegStatus.Height = lblIntegStatus.Height * 2;
            //Rev20.01 変更 by長野 2015/05/20
            lblIntegStatus.Height = (int)((float)lblIntegStatus.Height * (float)1.8);
            lblIntegStatus.Top = lblIntegStatus.Top - 6;    //100/15
        }

        //スレッド間動作のため、myTransImage_IntegralCountUpに変更
        //*******************************************************************************
        //機　　能： 積分処理時のイベント
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        //private void myTransImage_TransImageChanged(object sender, EventArgs e)
        //{

        //    //エラー時は無視する
        //     // ERROR: Not supported in C#: OnErrorStatement

        //    try
        //    {
        //        //プログレスバーが非表示の場合は無視
        //        if (!progressBar1.Visible)
        //        {
        //            return;
        //        }

        //        //int iSize = myTransImage.TransImageCtrl.ImageSize.Width * myTransImage.TransImageCtrl.ImageSize.Height;
        //        //ushort[] WorkImage = myTransImage.TransImageCtrl.GetImage();

        //        ////積算
        //        ////FluoroIP.AddImageWord(ref ref modScanCorrectNew.TransImage[0], ref ref WorkImageLong[0], Information.UBound(modScanCorrectNew.TransImage) + 1);
        //        //ImgProc.AddImageWord(myTransImage.TransImageCtrl.GetImage(), WorkImageLong, WorkImageLong.Length);


        //        //スレッド間で操作できないため、カウントアップ処理を移す
        //        IntegralUpdate();
        //        /*
        //        //カウントアップ
        //        //progressBar1.Value = val + 1;

              　
        //        //所定の枚数に達したら
        //        if (Convert.ToInt32(progressBar1.Value) >= Convert.ToInt32(progressBar1.Maximum))
        //        {

        //            ////ビジープロパティをオフ
        //            //IsBusy = false;

        //            //取り込み終了
        //            //CT30K.My.MyProject.Forms.frmTransImage.CaptureOn = false;
        //            myTransImage.TransImageCtrl.CaptureStop();

        //            WorkImage = myTransImage.TransImageCtrl.GetImage();

        //            //積算した画像データを所定枚数で平均化する
        //            //FluoroIP.DivImage(ref ref WorkImageLong[0], ref myTransImage.TransImageCtrl.GetImage(), ScanCorrect.h_size, ScanCorrect.v_size, Convert.ToInt32(progressBar1.Value));
        //            int hsize = myTransImage.TransImageCtrl.Detector.h_size;
        //            int vsize = myTransImage.TransImageCtrl.Detector.v_size;
        //            int num = progressBar1.Value;
        //            IICorrect.DivImage(ref WorkImageLong[0], ref WorkImage[0], hsize, vsize, num);
        //            myTransImage.TransImageCtrl.SetImage(WorkImage);

        //            //積算画像を表示
        //            //CT30K.My.MyProject.Forms.frmTransImage.Update_Renamed(false);
        //            myTransImage.TransImageCtrl.Update(false);

        //        }
        //        */
        //    }
        //    catch{
            
        //    }

        //    finally{
            
        //    }
        
        //}


        //*******************************************************************************
        //機　　能： 積分処理時カウントアップのイベント
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void myTransImage_IntegralCountUp(int Count, int CountEnd)
        {
            try
            {
                //プログレスバーが非表示の場合は無視
                if (!progressBar1.Visible)
                {
                    return;
                }

                //スレッド間で操作できないため、カウントアップ処理を移す
                IntegralUpdate();
            }
            catch
            {
            }
            finally
            {
            }
        
        }


        void IntegralUpdate()
        {
            if (InvokeRequired)
            {
                Invoke(new IntegralUpdateDelegate(IntegralUpdate));
                //BeginInvoke(new IntegralUpdateDelegate(IntegralUpdate));
                return;
            }


            try
            {
                //カウントアップ
                int count = progressBar1.Value;
                if (count < progressBar1.Maximum)
                {
                    progressBar1.Value = count + 1;

                }
                else
                {
                    if (count == progressBar1.Maximum)
                        progressBar1.Value = count;

                    //ビジープロパティをオフ
                    IsBusy = false;

                    //取り込み終了
                    //CT30K.My.MyProject.Forms.frmTransImage.CaptureOn = false;
                    
                    //スレッド内で止めると戻らないため他で行う
                    //myTransImage.TransImageCtrl.CaptureStop();
                    
                    
                    ////WorkImage = myTransImage.TransImageCtrl.GetImage();
                    //ushort[] WorkImage1 = new ushort[WorkImageLong.Length];

                    ////積算した画像データを所定枚数で平均化する
                    ////FluoroIP.DivImage(ref ref WorkImageLong[0], ref myTransImage.TransImageCtrl.GetImage(), ScanCorrect.h_size, ScanCorrect.v_size, Convert.ToInt32(progressBar1.Value));
                    //int hsize = myTransImage.TransImageCtrl.Detector.h_size;
                    //int vsize = myTransImage.TransImageCtrl.Detector.v_size;
                    //int num = progressBar1.Value;
                    //IICorrect.DivImage(ref WorkImageLong[0], ref WorkImage1[0], hsize, vsize, num);
                    //myTransImage.TransImageCtrl.SetImage(WorkImage1);

                    this.Enabled = true;
                    timer1.Enabled = true;
                }

            }
            catch
            {
            }
             
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            myTransImage.CaptureOn = false;
　                    
        }

    }
}
