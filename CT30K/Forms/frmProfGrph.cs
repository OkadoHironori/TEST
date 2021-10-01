using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{
    //* ************************************************************************** */
    //* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    //* 客先　　　　： ?????? 殿                                                   */
    //* プログラム名： frmProfGrph.frm                                             */
    //* 処理概要　　： プロフィール表示                                            */
    //* 注意事項　　： なし                                                        */
    //* -------------------------------------------------------------------------- */
    //* 適用計算機　： DOS/V PC                                                    */
    //* ＯＳ　　　　： Windows 2000  (SP4)                                         */
    //* コンパイラ　： VB 6.0                                                      */
    //* -------------------------------------------------------------------------- */
    //* VERSION     DATE        BY                  CHANGE/COMMENT                 */
    //*                                                                            */
    //* V1.00       99/XX/XX    (TOSFEC) ????????                                  */
    //* V2.0        00/02/08    (TOSFEC) 鈴山　修   V1.00を改造                    */
    //* V3.0        00/08/01    (TOSFEC) 鈴山　修   ｺｰﾝﾋﾞｰﾑCT対応                  */
    //*                                                                            */
    //* -------------------------------------------------------------------------- */
    //* ご注意：                                                                   */
    //* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    //*                                                                            */
    //* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    //* ************************************************************************** */
    public partial class frmProfGrph : Form
    {
        public RadioButton[] optScale = new RadioButton[3];    //ラジオボタンコントロール配列
        //private int x1 = 0; //線描画用
        //private int y1 = 0;
        
        // 撮影クラスのインスタンス
        private TransImageControl myTransImageCtrl;

        #region インスタンスを返すプロパティ

        // frmProfGrphのインスタンス
        private static frmProfGrph _Instance = null;

        /// <summary>
        /// frmProfGrphのインスタンスを返す
        /// </summary>
        public static frmProfGrph Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmProfGrph();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmProfGrph()
        {
            InitializeComponent();

            #region フォームにコントロールを追加

            this.SuspendLayout();

            for (int i = 0; i < optScale.Length; i++)
            {
                this.optScale[i] = new RadioButton();
                this.optScale[i].Font = new Font("ＭＳ ゴシック", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(128)));
                this.optScale[i].Location = new Point(20, 23 + i * 24);
                this.optScale[i].Name = "optScale" + i.ToString();
                this.optScale[i].Size = new Size(125, 21);
                this.optScale[i].TabIndex = i;
                this.optScale[i].TabStop = true;
                this.optScale[i].UseVisualStyleBackColor = true;
                switch (i)
                {
                    case 0:
                        this.optScale[i].Text = "1/ 1 倍";
                        break;
                    case 1:
                        this.optScale[i].Text = "1/ 4 倍";
                        break;
                    case 2:
                        this.optScale[i].Text = "1/16 倍";
                        break;
                    default:
                        break;
                }
                this.optScale[i].CheckedChanged += new EventHandler(optScale_CheckedChanged);
                this.fraScale.Controls.Add(this.optScale[i]);
            }

            this.ResumeLayout(false);

            myTransImageCtrl = CTSettings.transImageControl;
            
           
            #endregion
        }

        //オフセット画像の配列
        //private short[] myOffsetImage;
        private ushort[] myOffsetImage;
        private ushort[] myImage;			//透視画像の配列（表示用）

        //表示する画像サイズ
        private int SizeX;      //v17.02追加 byやまおか 2010/06/15
        private int SizeY;      //v17.02追加 byやまおか 2010/06/15

        //画像サイズの端数
        private int mod_SizeX;  //v17.02追加 byやまおか 2010/06/15
        private int mod_SizeY;  //v17.02追加 byやまおか 2010/06/15

        //サポートしているイベント
        public event UnloadedEventHandler Unloaded;     //アンロードされた   'v17.50追加 by 間々田 2011/02/02
        public delegate void UnloadedEventHandler();

        //コントロール更新用のデリゲート
        delegate void TransImageUpdateDelegate();

        //追加2014/11/28hata_v19.51_dnet
        //目盛りのTextの右位置
        private int CWSlide1maxRight = 0;
        private int CWSlide1minRight = 0;
        private int CWSlide2maxRight = 0;
        private int CWSlide2minRight = 0;

        //*******************************************************************************
        //機　　能： オフセットプロフィールオプションボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V17.10  10/08/10    やまおか    新規作成
        //*******************************************************************************
        private void chkProf_CheckStateChanged(object sender, EventArgs e)
        {
            if (frmScanControl.Instance.chkDspPosi.CheckState == CheckState.Checked)
            {
                UpdateGraph();
            }
        }

        ////*************************************************************************************************
        ////機　　能： 透視画像変更時処理
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： V1.00  99/XX/XX   ????????      新規作成
        ////*************************************************************************************************
        //private void myTransImage_TransImageChanged()
        //{
        //    if (frmScanControl.Instance.chkDspPosi.CheckState == CheckState.Checked)
        //    {
        //        UpdateGraph();
        //    }
        //}
       //描画用イベント処理
        void myTransImageCtrl_TransImageChanged(object sender, EventArgs e)
        {        
            ////撮影画像
            //myImage = CTSettings.transImageControl.GetImage();
            ////modScanCorrectNew.TransImage = CTSettings.transImageControl.GetImage();
            //if (frmScanControl.Instance.chkDspPosi.CheckState == CheckState.Checked)
            //{
            //    UpdateGraph();
            //}

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
        }

        //X線MechData更新
        private void TransImageUpdate()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new TransImageUpdateDelegate(TransImageUpdate));
                return;
            }

            try
            {
                //撮影画像
                myImage = CTSettings.transImageControl.GetImage();
                //modScanCorrectNew.TransImage = CTSettings.transImageControl.GetImage();
                if (frmScanControl.Instance.chkDspPosi.CheckState == CheckState.Checked)
                {
                    UpdateGraph();
                }
 
            }
            catch
            {
            }

        }


        //*******************************************************************************
        //機　　能： 縦倍率オプションボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [I/ ] 型        1:1/１倍 2:1/４倍 3:1/16倍
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void optScale_CheckedChanged(object sender, EventArgs e)
        {
            ////縦倍率ラジオボタン1(Index=0), 2(Index=1), 3(Index=2)のどれがチェックされたか判定
            //int Index = -1;
            //for (int i = 0; i < optScale.Length; i++)
            //{
            //    if (sender.Equals(optScale[i]))
            //    {
            //        Index = i;
            //        break;
            //    }
            //}

            RadioButton r = sender as RadioButton;

            if (r == null) return;

            //unCheckのときは何もしない
            if (!r.Checked) return;

            int Index = Array.IndexOf(optScale, r);


            //透視画像画面のコントラスト調整のスクロールバーの最大値の調整
            //WLWWMax = Choose(Index, 1024, 2048, 4096)
            //CWSlide2.Axis.Maximum = WLWWMax             'プロフィールの縦軸の最大値表示

            //下記に変更 by 間々田 2005/01/07
            modCT30K.FimageBitIndex = Index;
            
            //CWSlide2.Axis.Maximum = Choose(Index + 1, 256, 1024, 4096) 'v17.02削除 byやまおか 2010/06/14
            switch (CTSettings.detectorParam.DetType)
            {
                //v17.02追加(ここから) byやまおか 2010/06/14
                case DetectorConstants.DetTypeII:
                case DetectorConstants.DetTypeHama:
                    // 【C#コントロールで代用】
                    if (Index + 1 == 1)
                    {
                        CWSlide2.Maximum = 256;
                        CWSlide2Max.Text = CWSlide2.Maximum.ToString();
                    }
                    else if (Index + 1 == 2)
                    {
                        CWSlide2.Maximum = 1024;
                        CWSlide2Max.Text = CWSlide2.Maximum.ToString();
                    }
                    else if (Index + 1 == 3)
                    {
                        CWSlide2.Maximum = 4096;
                        CWSlide2Max.Text = CWSlide2.Maximum.ToString();
                    }
                    break;
                case DetectorConstants.DetTypePke:
                    // 【C#コントロールで代用】
                    //if (Index + 1 == 4096)
                    //Rev20.00 修正 by長野 2014/12/04
                    if (Index + 1 == 1)
                    {
                        CWSlide2.Maximum = 4096;
                        CWSlide2Max.Text = CWSlide2.Maximum.ToString();
                    }
                    //else if (Index + 1 == 16384)
                    //Rev20.00 修正 by長野 2014/12/04
                    else if (Index + 1 == 2)
                    {
                        CWSlide2.Maximum = 16384;
                        CWSlide2Max.Text = CWSlide2.Maximum.ToString();
                    }
                    //else if (Index + 1 == 65536)
                    //Rev20.00 修正 by長野 2014/12/04
                    else if (Index + 1 == 3)
                    {
                        CWSlide2.Maximum = 65536;
                        CWSlide2Max.Text = CWSlide2.Maximum.ToString();
                    }
                    break;
                default:
                    break;
            }
            // 【C#コントロールで代用】
            //CWSlide2.Axis.Ticks.MinorDivisions = 64;    //v17.02追加(ここまで) byやまおか 2010/06/14

            //変更2014/11/28hata_v19.51_dnet
            //CWSlide2.TickFrequency = 64;
            CWSlide2.TickFrequency = (int)(CWSlide2.Maximum / 8);

            //追加2014/11/28hata_v19.51_dnet
            //目盛りのTextの位置を設定
            CWSlide1Max.Location = new Point(CWSlide1maxRight - CWSlide1Max.Width, CWSlide1Max.Top);
            CWSlide1Min.Location = new Point(CWSlide1minRight - CWSlide1Min.Width, CWSlide1Min.Top);
            CWSlide2Max.Location = new Point(CWSlide2maxRight - CWSlide2Max.Width, CWSlide2Max.Top);
            CWSlide2Min.Location = new Point(CWSlide2minRight - CWSlide2Min.Width, CWSlide2Min.Top);


            //透視画像フォームのウィンドウレベル最大値・ウィンドウ幅最大値の更新
            //frmPulsar.SetWLWWMax

            modLibrary.SetOption(frmScanControl.Instance.optScale, Index);    //v15.0変更 by 間々田 2009/01/13

            //グラフの更新
            UpdateGraph();  //v9.7追加 by 間々田 2004/12/07
        }

        //*******************************************************************************
        //機　　能： 「閉じる」ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdClose_Click(object sender, EventArgs e)
        {
            //Public Sub cmdClose_Click() 'v17.02変更 byやまおか 2010/07/23  'v17.50元に戻す by 間々田 2011/02/02

            //透視画像フォームのプロフィールボタンを有効にする           'v17.50削除 by 間々田 2011/02/02 frmScanControl.myProfGraph_Unloadedで実施することにした
            //frmScanControl.cmdShowProf.Enabled = True
            //frmScanControl.cmdShowProf.BackColor = vbButtonFace     'v15.10追加 byやまおか 2010/01/21

            //プロフィール表示フォームをアンロードする
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
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void frmProfGrph_Load(object sender, EventArgs e)
        {

            //ファイルの大きさ v17.20 追加 by 長野 2010/09/06
            int FileLength = 0;

            //画像サイズの端数   'v17.02追加 byやまおか 2010/06/15
            switch (CTSettings.detectorParam.DetType)
            {
                case DetectorConstants.DetTypePke:    //(2048なら48)
                    mod_SizeX = CTSettings.detectorParam.h_size % 100;
                    mod_SizeY = CTSettings.detectorParam.v_size % 100;
                    break;
                default:
                    mod_SizeX = 0;
                    mod_SizeY = 0;
                    break;
            }

            //表示画像サイズ
            SizeX = CTSettings.detectorParam.h_size - mod_SizeX; //v17.02追加 byやまおか 2010/06/15
            SizeY = CTSettings.detectorParam.v_size - mod_SizeY; //v17.02追加 byやまおか 2010/06/15

            //if (modScanCorrectNew.TransImage == null)
            //{
            //    modScanCorrectNew.TransImage = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
            //}
            //if (modScanCorrectNew.TransImage.Length != CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size)
            //{
            //    modScanCorrectNew.TransImage = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
            //}

            myImage = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
           
            
            //キャプションのセット
            SetCaption();

            //コントロールの初期化
            InitControls();

            //    ReDim myCurrentImage(h_size * v_size)   'v9.7追加 by 間々田 2004/12/07

            //オフセット校正画像の取得

            //オフセット校正画像のファイルサイズチェック
            FileInfo file = new FileInfo(ScanCorrect.OFF_CORRECT);
            FileLength = (int)file.Length;

            //   検出器切替用に条件式を変更 by 長野 2010/09/06
            //    If FileLen(OFF_CORRECT) <> h_size * v_size * 8 Then
            //
            if (FileLength != CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size * 8)
            {
                //メッセージ表示：オフセット校正画像のファイルサイズが正しくありません。
                MessageBox.Show(StringTable.GetResString(StringTable.IDS_FileSizeError, CTResources.LoadResString(12746)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                //「オフセットプロフィール」チェックボックスを使用不可にする 'v9.7追加 by 間々田 2004/12/27
                chkProf.Enabled = false;
            }
            else
            {
                //オフセット配列初期化（Double）
                ScanCorrect.OFFSET_IMAGE = new double[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];   //v15.0変更 -1した by 間々田 2009/06/03

                //オフセット校正ファイルをオープン
                ScanCorrect.DoubleImageOpen(ref ScanCorrect.OFFSET_IMAGE[0], ScanCorrect.OFF_CORRECT, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);

                UpdateOffset(); //v9.7追加 by 間々田 2004/12/07

                //「オフセットプロフィール」チェックボックスにチェックを入れる（chkProf_Clickがコールされます。デフォルト）
                chkProf.CheckState = CheckState.Checked;
            }

            //ウィンドウレベル・ウィンドウ幅の最大値に基づき、オプションボタンを設定
            //SetWLWWMax
            modLibrary.SetOption(optScale, modCT30K.FimageBitIndex);    //変更 by 間々田 2005/01/07
           
            //描画用イベント
            myTransImageCtrl.TransImageChanged += new EventHandler(myTransImageCtrl_TransImageChanged);
            myTransImageCtrl.WithSetImageOn = true;
            
            //グラフを表示                       'v17.50追加 by 間々田 2011/02/02
            UpdateGraph();
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
        private void frmProfGrph_FormClosed(object sender, FormClosedEventArgs e)
        {
            //描画用イベントを解放
            if (myTransImageCtrl != null) //追加201501/26hata_if文追加
            {
                myTransImageCtrl.WithSetImageOn = false;
                myTransImageCtrl.TransImageChanged -= new EventHandler(myTransImageCtrl_TransImageChanged);

                //撮影クラスへの参照解放
                myTransImageCtrl = null;
            }

            //イベント生成（アンロードされた）   'v17.50追加 by 間々田 2011/02/02
            if (Unloaded != null)
            {
                Unloaded();
            }
        }

        //*******************************************************************************
        //機　　能： 各コントロールのキャプションに文字列をセットする
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void SetCaption()
        {
            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            //optScale(0).Caption = GetResString(IDS_Times, "1/ 1")   '1/ 1 倍    ' 4 倍 → 1 倍に変更 by 間々田 2005/01/07
            //optScale(1).Caption = GetResString(IDS_Times, "1/ 4")   '1/ 4 倍    ' 8 倍 → 4 倍に変更 by 間々田 2005/01/07
            //optScale(2).Caption = GetResString(IDS_Times, "1/16")   '1/16 倍
            optScale[0].Text = " 1 / 16";   //v17.10変更 by やまおか 2010/08/26
            optScale[1].Text = " 1 / 4";    //v17.10変更 by やまおか 2010/08/26
            optScale[2].Text = " 1 / 1";    //v17.10変更 by やまおか 2010/08/26
        }

        //*******************************************************************************
        //機　　能： 各コントロールの位置・サイズ等の初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void InitControls()
        {
            //縦倍率フレームの表示・非表示
            fraScale.Visible = ((CTSettings.scancondpar.Data.fimage_bit == 2) && CTSettings.detectorParam.Use_FlatPanel);

            //    Picture1.Width = MinVal(h_size / phm, 640) * 15
            ///    pic0.width = frmPulsar.Display1.width               'change by 間々田 2004/11/04 frmPulsar の Display1 と同じ幅にする
            //↑あとで

            //Rev20.01追加 by長野 2015/05/19
            if (modCT30K.IsEnglish == true)
            {
                fraScale.Font = new Font(fraScale.Font.Name, (float)8.5, fraScale.Font.Style, fraScale.Font.Unit);
            }

            //追加2014/11/28hata_v19.51_dnet
            //目盛りのTextの現在の右位置を取得
            CWSlide1maxRight = CWSlide1Max.Right;
            CWSlide1minRight = CWSlide1Min.Right;
            CWSlide2maxRight = CWSlide2Max.Right;
            CWSlide2minRight = CWSlide2Min.Right;

            // 【C#コントロールで代用】
            //    CWSlide1.Width = pic0.Width + 240
            //2014/11/07hata キャストの修正
            //CWSlide1.Width = pic0.Width + (360 / 15);           //change by 間々田 2004/12/04

            //削除2014/11/28hata_v19.51_dnet
            //CWSlide1.Width = pic0.Width + 24;           //change by 間々田 2004/12/04

            //追加2014/11/28hata_v19.51_dnet
            CWSlide1.TickFrequency = (int)(CWSlide1.Maximum / 16);

            // 【C#コントロールで代用】
            //CWSlide1.Axis.Maximum = h_size
            CWSlide1.Maximum = SizeX;                           //v17.02変更 byやまおか 2010/06/15
            //2014/11/07hata キャストの修正
            //CWSlide2.Height = pic0.Height + (240 / 15);         //v17.02追加 byやまおか 2010/07/07
            //削除2014/11/28hata_v19.51_dnet
            //CWSlide2.Height = pic0.Height + 16;         //v17.02追加 byやまおか 2010/07/07
            CWSlide1Max.Text = CWSlide1.Maximum.ToString();
            //表示項目フレームの位置
            //fraView.Left = frmPulsar.fraView.Left
            //fraView.left = pic0.left + pic0.width
            //2014/11/07hata キャストの修正
            //fraView.Left = pic0.Left + pic0.Width + (120 / 15); //v17.02変更 byやまおか 2010/06/15
            fraView.Left = pic0.Left + pic0.Width + 8; //v17.02変更 byやまおか 2010/06/15

            //PkeFPDの場合はオフセット/ゲインの関係がI.I.のようにならないので非表示
            //v17.00追加 byやまおか
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
                chkProf.CheckState = CheckState.Unchecked;      //未チェック
                fraView.Visible = false;                        //フレームを隠す
            }

            //縦倍率フレームの位置
            fraScale.Left = fraView.Left;

            //「閉じる」ボタンの位置
            //cmdClose.left = fraView.left + fraView.width / 2 - cmdClose.width / 2
            //2014/11/07hata キャストの修正
            //cmdClose.Left = fraView.Left + fraView.Width / 2 - cmdClose.Width / 2;
            cmdClose.Left = fraView.Left + Convert.ToInt32(fraView.Width / 2F) - Convert.ToInt32(cmdClose.Width / 2F);

            //表示位置調整(透視画像の下に表示)   'v15.10追加 byやまおか 2009/11/26
            this.SetBounds(frmTransImageControl.Instance.Left - this.Width, Screen.PrimaryScreen.Bounds.Height - this.Height, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

        }

        //削除ここから by 間々田 2005/01/07
        //'
        //'   ウィンドウレベル・ウィンドウ幅の最大値に基づき、オプションボタンを設定
        //'
        //Public Sub SetWLWWMax()
        //
        //    Select Case WLWWMax
        //        Case 1024
        //            optScale(1).Value = True
        //        Case 2048
        //            optScale(2).Value = True
        //        Case 4096
        //            optScale(3).Value = True
        //    End Select
        //
        //End Sub
        //削除ここまで by 間々田 2005/01/07

        /// <summary>
        /// UpdateOffset
        /// </summary>
        public void UpdateOffset()
        {
            int rc = 0;

            myOffsetImage = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];

            //配列のコピー（Double→Integer）
            rc = ScanCorrect.ImageCopyDtoUS(ref ScanCorrect.OFFSET_IMAGE[0], 
                                            ref myOffsetImage[0], 
                                            CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);

            //ゲイン補正
            if(CTSettings.scanParam.FPGainOn)
            {
                //2014/11/07hata キャストの修正
                //ScanCorrect.FpdGainCorrect(ref myOffsetImage[0], 
                //                           ref ScanCorrect.GAIN_IMAGE[0],
                //                           CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, (int)ScanCorrect.GainMean);
                ScanCorrect.FpdGainCorrect(ref myOffsetImage[0],
                                           ref ScanCorrect.GAIN_IMAGE[0],
                                           CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, Convert.ToInt32(ScanCorrect.GainMean));

            }
            //v17.00削除 byやまおか 2010/01/20

            //欠陥補正
            //If FPDefOn Then Call FpdDefCorrect_short(myOffsetImage(0), Def_IMAGE(0), h_size, v_size, 0, v_size - 1)                'v17.00削除 byやまおか 2010/01/20
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //If (DetType = DetTypeHama) Then     'v17.00追加 byやまおか 2010/01/20
            //    Call FpdDefCorrect_short(myOffsetImage(0), Def_IMAGE(0), h_size, v_size, 0, v_size - 1)
            //End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
        }

        //v9.7追加ここから by 間々田 2004/12/07
        //*************************************************************************************************
        //機　　能： 更新処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void UpdateGraph()
        {
            //Public Sub UpdateGraph()   'v17.02変更 byやまおか 2010/07/07       'v17.50元に戻す by 間々田 2011/02/02

            //Dim i       As Long                                                'v17.50削除ここから by 間々田 2011/02/02
            //Dim x       As Long
            //Dim yMax    As Long
            //Dim yPos    As Long             'v17.02追加 byやまおか 2010/06/14
            //Dim TransData_Int   As Integer  'v17.02追加 byやまおか 2010/06/15
            //Dim TransData_Long  As Long     'v17.02追加 byやまおか 2010/06/15
            //
            //yMax = CWSlide2.Axis.Maximum                                       'v17.50削除ここまで by 間々田 2011/02/02

            if (pic0.Image != null)
            {
                pic0.Image.Dispose();
                pic0.Image = null;
            }

            //PkeFPDの場合はオフセットグラフを描かない   'v17.02追加 byやまおか 2010/06/14
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
                chkProf.CheckState = CheckState.Unchecked;
            }

            //オフセットグラフの方を先に描く
            if (chkProf.CheckState == CheckState.Checked)
            {
                //.CurrentX = 0
                //.CurrentY = .ScaleHeight - .ScaleHeight * myOffsetImage(h_size * GetYPos(0) + 0) / yMax
                //.CurrentY = .ScaleHeight - .ScaleHeight * myOffsetImage(h_size * GetYPos(0 + mod_SizeX / 2 + 1) + 0 + mod_SizeX / 2 + 1) / yMax 'v17.02変更 byやまおか 2010/06/15
                //.CurrentY = .ScaleHeight - .ScaleHeight * myOffsetImage(h_size * GetYPos(0 + mod_SizeX / 2) + 0 + mod_SizeX / 2) / yMax   'v17.10修正 byやまおか 2010/08/10
                //For i = 1 To .ScaleWidth - 1
                //    'x = i * h_size / .ScaleWidth
                //    x = i * SizeX / .ScaleWidth    'v17.02変更 byやまおか 2010/06/15
                //    'pic0.Line -(i, .ScaleHeight - .ScaleHeight * myOffsetImage(h_size * GetYPos(x) + x) / yMax), vbMagenta
                //    'pic0.Line -(i, .ScaleHeight - .ScaleHeight * myOffsetImage(h_size * GetYPos(x + mod_SizeX / 2 + 1) + (x + mod_SizeX / 2 + 1)) / yMax), vbMagenta  'v17.02変更 byやまおか 2010/06/15
                //    pic0.Line -(i, .ScaleHeight - .ScaleHeight * myOffsetImage(h_size * GetYPos(x + mod_SizeX / 2) + (x + mod_SizeX / 2)) / yMax), vbMagenta  'v17.10修正 byやまおか 2010/08/10
                //Next

                //スライスライン上のオフセット画像のプロフィール値を描画 v17.50上記を関数化  by 間々田 2011/02/01
                //2014/11/07hata キャストの修正
                //DrawGraph(ref myOffsetImage, SizeX / pic0.ClientRectangle.Width, mod_SizeX / 2, Color.Magenta);
                DrawGraph(ref myOffsetImage, (float)SizeX / (float)pic0.ClientRectangle.Width, (float)mod_SizeX / 2F, Color.Magenta);
            }

            #region コメントアウト

            //.CurrentX = 0
            //.CurrentY = .ScaleHeight - .ScaleHeight * TransImage(h_size * GetYPos(0) + 0) / yMax
            //.CurrentY = .ScaleHeight - .ScaleHeight * TransImage(h_size * GetYPos(0 + mod_SizeX / 2 + 1) + 0 + mod_SizeX / 2 + 1) / yMax  'v17.02変更 byやまおか 2010/06/15
            //.CurrentY = .ScaleHeight - .ScaleHeight * TransImage(h_size * GetYPos(0 + mod_SizeX / 2) + 0 + mod_SizeX / 2) / yMax    'v17.10修正 byやまおか 2010/08/10
            //For i = 1 To .ScaleWidth - 1
            //    'x = i * h_size / .ScaleWidth
            //    x = i * SizeX / .ScaleWidth     'v17.02変更 byやまおか 2010/06/15
            //
            //    'pic0.Line -(i, .ScaleHeight - .ScaleHeight * TransImage(h_size * GetYPos(x) + x) / yMax), vbGreen  'v17.02削除 byやまおか 2010/06/14
            //
            //    'v17.02追加(ここから) byやまおか 2010/06/15
            //    '線を描く前にデータを確認する
            //
            //    '符号付きInt型(16bit)で透視データを受け取る
            //    'TransData_Int = TransImage(h_size * GetYPos(x) + x)
            //    'TransData_Int = TransImage(h_size * GetYPos(x + mod_SizeX / 2 + 1) + (x + mod_SizeX / 2 + 1))
            //    TransData_Int = TransImage(h_size * GetYPos(x + mod_SizeX / 2) + (x + mod_SizeX / 2))   'v17.10修正 byやまおか 2010/08/10
            //
            //    'VB6には符号なしInt型がないため負値のときは65536(=2^16)加算してLong型に格納する
            //    TransData_Long = IIf(TransData_Int < 0, CLng(TransData_Int) + 65536, CLng(TransData_Int))
            //
            //    'スケールオーバーしているときは切り詰める
            //    yPos = .ScaleHeight * TransData_Long / yMax
            //    yPos = CorrectInRangeLong(yPos, 1, .ScaleHeight)
            //
            //    '描画
            //    pic0.Line -(i, .ScaleHeight - yPos), vbGreen
            //    'v17.02追加(ここまで) byやまおか 2010/06/15
            //
            //Next

            #endregion
            
            //スライスライン上の透視画像のプロフィール値を描画 v17.50上記を関数化 by 間々田 2011/02/01
            //DrawGraph(ref modScanCorrectNew.TransImage, 
            //          SizeX / pic0.ClientRectangle.Width, mod_SizeX / 2, Color.Lime);
            //2014/11/07hata キャストの修正
            //DrawGraph(ref myImage, SizeX / pic0.ClientRectangle.Width, mod_SizeX / 2, Color.Lime);
            DrawGraph(ref myImage, (float)SizeX / (float)pic0.ClientRectangle.Width, (float)mod_SizeX / 2F, Color.Lime);

            //v17.65 描画より早く更新されるため一定時間待つ by長野 2011/11/25
            //modCT30K.PauseForDoEvents(0.02f);
        }

        /// <summary>
        /// GetYPos
        /// </summary>
        /// <param name="x"></param>
        private int GetYPos(int x)
        {
            //2014/11/07hata キャストの修正
            //return modLibrary.CorrectInRange((short)(CTSettings.detectorParam.v_size / 2 + ScanCorrect.GVal_ScanPosiA[2] * (x - CTSettings.detectorParam.h_size / 2) + ScanCorrect.GVal_ScanPosiB[2]), 
            //                                 0,
            //                                 (short)(CTSettings.detectorParam.v_size - 1));
            return modLibrary.CorrectInRange(Convert.ToInt16(CTSettings.detectorParam.v_size / 2F + 
                                              ScanCorrect.GVal_ScanPosiA[2] * (x - CTSettings.detectorParam.h_size / 2F) + 
                                              ScanCorrect.GVal_ScanPosiB[2]),
                                             0,
                                             Convert.ToInt16(CTSettings.detectorParam.v_size - 1));

        }
        //v9.7追加ここまで by 間々田 2004/12/07


        //*************************************************************************************************
        //機　　能： グラフ描画処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.50  2011/02/01 (電S1)間々田 新規作成
        //*************************************************************************************************
        //private void DrawGraph(ref short[] p, float a, float b, Color drawColor)
        private void DrawGraph(ref ushort[] p, float a, float b, Color drawColor)
        {
            int i = 0;  //
	        int x = 0;  //グラフ上でのＸ座標
	        int y = 0;  //グラフ上でのＹ座標
	        int xx = 0; //画像配列上でのＸ座標
	        int yy = 0; //画像配列上でのＹ座標
            int x1 = 0; //線描画用
            int y1 = 0; //線描画用
        	
            //透視画像を参照できない場合、何もしない
            if (myTransImageCtrl == null)
            {
                return;
            }

            if (pic0.Image == null)
            {
                //Imageの生成
                pic0.Image = new Bitmap(pic0.Width, pic0.Height);
            }

            //Graphics オブジェクトの取得
            Graphics graphic = Graphics.FromImage(pic0.Image);
            
            //Penオブジェクトの作成(幅3黒色)
            Pen pen = new Pen(drawColor, 1);

	        for (i = 0; i <= pic0.ClientRectangle.Width - 1; i++)
            {
		        //透視画像を左右反転表示している場合はプロフィールグラフも左右反転させる
                x = (myTransImageCtrl.MirrorOn ? pic0.ClientRectangle.Width - 1 - i : i);
 
		        //透視画像を左右反転表示している場合はプロフィールグラフも左右反転させる
		        //If myTransImage.ctlTransImage.MirrorOn Then
		        //    xx = a * (.ScaleWidth - 1 - x) + b
		        //Else
                //2014/11/07hata キャストの修正
                //xx = (int)(a * x + b);
                xx = Convert.ToInt32(a * x + b);

		        //End If

		        yy = GetYPos(xx);

		        //y = pic0.ClientRectangle.Height - modLibrary.CorrectInRangeLong(pic0.ClientRectangle.Height * ConvertIntegerToLong((short)p[CTSettings.detectorParam.h_size * yy + xx]) / CWSlide2.Maximum, 
                //                                                                1, pic0.ClientRectangle.Height);
                //2014/11/07hata キャストの修正
                //y = pic0.ClientRectangle.Height - 
                //    modLibrary.CorrectInRange(pic0.ClientRectangle.Height * ConvertIntegerToLong((short)p[CTSettings.detectorParam.h_size * yy + xx]) / CWSlide2.Maximum,
                //                                                                1, pic0.ClientRectangle.Height);
                y = pic0.ClientRectangle.Height - 
                    modLibrary.CorrectInRange(Convert.ToInt32(pic0.ClientRectangle.Height * ConvertIntegerToLong((short)p[CTSettings.detectorParam.h_size * yy + xx]) / (float)CWSlide2.Maximum),
                                              1, 
                                              pic0.ClientRectangle.Height);
                
                //折れ線を描画
                //if (i == 0)
                //{
                //    pic0.CurrentX = i;
                //    pic0.CurrentY = y;
                //}
                //else
                //{
                    if(i > 0) graphic.DrawLine(pen, x1, y1, i, y);
                    x1 = i; y1 = y;
                
                //}
	        }
            x1 = 0; y1 = 0;

            //リソース解放
            pen.Dispose();
            graphic.Dispose();
        }

        //*************************************************************************************************
        //機　　能： 実際には unsigned short 型 の Integer 型 数値を Long型 数値に変換する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.50  2011/02/01 (電S1)間々田 新規作成
        //*************************************************************************************************
        private int ConvertIntegerToLong(short Target)
        {
            //VB6には符号なしInt型がないため負値のときは65536(=2^16)加算してLong型に格納する
            return (Target < 0 ? Convert.ToInt32(Target) + 65536 : Convert.ToInt32(Target));
        }
    }
}
