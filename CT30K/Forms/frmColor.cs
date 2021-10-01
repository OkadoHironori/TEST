using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.IO;
//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： カラー処理.frm                                              */
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
    public partial class frmColor : Form
    {
        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************

        //Private ColorMin    As Long        'v15.0削除 by 間々田 2009/02/12
        //Private ColorMax    As Long        'v15.0削除 by 間々田 2009/02/12
        private int ColorVal;                      //カラースケール色数
        private int[] SetColor = new int[8];      //設定カラースケール色（ＲＧＢ値）
        private int[] SpSetPR = new int[8];       //閾値設定範囲（パーセント：下限、上限）
        private int[] SpSetCT = new int[9];       //閾値設定範囲（ＣＴ値：下限、上限）
        private string LastText;
        //private modDispinf.dispinfType dispinfBack; //dispinfのバックアップ
        private DispInf dispinfBack; //dispinfのバックアップ

        //追加2014/11/28hata_v19.51_dnet
        private Cursor DragCur; 

        #region コントロール

        private TabPageCtrl tabColorPage;

        private RadioButton[] optColorExe = new RadioButton[3];

        private PictureBox[] picColorPalet = new PictureBox[8];
        private PictureBox[] picColorScale = new PictureBox[16];

        private RadioButton[] optThreshold = new RadioButton[2];
        private PictureBox[] picColorPDmy = new PictureBox[8];
        private TextBox[] txtLower = new TextBox[8];
        private TextBox[] txtUpper = new TextBox[8];

        #endregion

        // ドラッグ元情報変数
        private DragSource dragSource;

        #region インスタンスを返すプロパティ

        // frmColorのインスタンス
        private static frmColor _Instance = null;

        /// <summary>
        /// frmColorのインスタンスを返す
        /// </summary>
        public static frmColor Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmColor();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmColor()
        {
            InitializeComponent();

            #region フォームにコントロールを追加

            this.SuspendLayout();

            #region タブ１内コントロールのインスタンス作成し、プロパティを設定する

            for (int i = 0; i < optColorExe.Length; i++)
            {
                optColorExe[i] = new RadioButton();
                optColorExe[i].AutoSize = true;
                switch (i)
                {
                    case 1:
                        optColorExe[i].Location = new Point(16, 16);
                        optColorExe[i].Text = "#レインボーカラー";
                        optColorExe[i].Name = "optColorExe1";
                        optColorExe[i].Tag = "12428";
                        break;
                    case 2:
                        optColorExe[i].Location = new Point(16, 40);
                        optColorExe[i].Text = "#オリジナルカラー";
                        optColorExe[i].Name = "optColorExe2";
                        optColorExe[i].Tag = "12423";
                        break;
                    case 0:
                        optColorExe[i].Location = new Point(16, 64);
                        optColorExe[i].Text = "#モノクロ";
                        optColorExe[i].Name = "optColorExe0";
                        optColorExe[i].Tag = "12427";
                        break;
                    default:
                        break;
                }
                optColorExe[i].Size = new Size(86, 20);
                optColorExe[i].TabIndex = i;
                optColorExe[i].TabStop = true;
                optColorExe[i].UseVisualStyleBackColor = true;

                optColorExe[i].CheckedChanged += new EventHandler(optColorExe_CheckedChanged);
            }

            #endregion

            #region タブ２内コントロールのインスタンス作成し、プロパティを設定する

            for (int i = 0; i < picColorPalet.Length; i++)
            {
                picColorPalet[i] = new PictureBox();
                picColorPalet[i].BorderStyle = BorderStyle.Fixed3D;
                picColorPalet[i].Location = new Point(19 + i * 56, 72);
                picColorPalet[i].Name = "picColorPalet" + i.ToString();
                picColorPalet[i].Size = new Size(50, 41);
                picColorPalet[i].TabIndex = i;
                picColorPalet[i].TabStop = false;
                groupBox2.Controls.Add(this.picColorPalet[i]);

                //picColorScale[i].MouseDown += new MouseEventHandler(picColorScale_MouseDown);
                //picColorScale[i].MouseMove += new MouseEventHandler(picColorScale_MouseMove);
                //picColorScale[i].MouseUp += new MouseEventHandler(picColorScale_MouseUp);
            }
            for (int i = 0; i < picColorScale.Length; i++)
            {
                picColorScale[i] = new PictureBox();
                #region カラー指定
                switch (i)
                {
                    case 0:
                        picColorScale[i].BackColor = ColorTranslator.FromWin32(0xFF);
                        break;
                    case 1:
                        picColorScale[i].BackColor = ColorTranslator.FromWin32(0x5AFF);
                        break;
                    case 2:
                        picColorScale[i].BackColor = ColorTranslator.FromWin32(0xB4FF);
                        break;
                    case 3:
                        picColorScale[i].BackColor = ColorTranslator.FromWin32(0xFFEF);
                        break;
                    case 4:
                        picColorScale[i].BackColor = ColorTranslator.FromWin32(0xFF95);
                        break;
                    case 5:
                        picColorScale[i].BackColor = ColorTranslator.FromWin32(0xFF3B);
                        break;
                    case 6:
                        picColorScale[i].BackColor = ColorTranslator.FromWin32(0x20FF00);
                        break;
                    case 7:
                        picColorScale[i].BackColor = ColorTranslator.FromWin32(0x85FF00);
                        break;
                    case 8:
                        picColorScale[i].BackColor = ColorTranslator.FromWin32(0xD5FF00);
                        break;
                    case 9:
                        picColorScale[i].BackColor = ColorTranslator.FromWin32(0xFFD000);
                        break;
                    case 10:
                        picColorScale[i].BackColor = ColorTranslator.FromWin32(0xFF7600);
                        break;
                    case 11:
                        picColorScale[i].BackColor = ColorTranslator.FromWin32(0xFF1B00);
                        break;
                    case 12:
                        picColorScale[i].BackColor = ColorTranslator.FromWin32(0xFF0040);
                        break;
                    case 13:
                        picColorScale[i].BackColor = ColorTranslator.FromWin32(0xFF009B);
                        break;
                    case 14:
                        picColorScale[i].BackColor = ColorTranslator.FromWin32(0xFF00F5);
                        break;
                    case 15:
                        picColorScale[i].BackColor = ColorTranslator.FromWin32(0xB000FF);
                        break;
                    default:
                        break;
                }
                #endregion
                picColorScale[i].BorderStyle = BorderStyle.Fixed3D;
                picColorScale[i].Location = new Point(21 + i * 26, 137);
                picColorScale[i].Name = "picColorScale" + i.ToString();
                picColorScale[i].Size = new Size(25, 25);
                picColorScale[i].TabIndex = i;
                picColorScale[i].TabStop = false;

                picColorScale[i].MouseDown += new MouseEventHandler(picColorScale_MouseDown);
                picColorScale[i].MouseMove += new MouseEventHandler(picColorScale_MouseMove);
                picColorScale[i].MouseUp += new MouseEventHandler(picColorScale_MouseUp);
            }

            #endregion

            #region タブ３内コントロールのインスタンス作成し、プロパティを設定する

            for (int i = 0; i < optThreshold.Length; i++)
            {
                optThreshold[i] = new RadioButton();

                optThreshold[i].AutoSize = true;
                optThreshold[i].Location = new Point(24, 16 + i * 24);
                optThreshold[i].Name = "optThreshold" + i.ToString();
                optThreshold[i].Size = new Size(104, 20);
                optThreshold[i].TabIndex = i;
                optThreshold[i].TabStop = true;
                switch (i)
                {
                    case 0:
                        optThreshold[i].Text = "#パーセント";
                        optThreshold[i].Tag = "12426";
                        break;
                    case 1:
                        optThreshold[i].Text = "#ＣＴ値";
                        optThreshold[i].Tag = "12431";
                        break;
                    default:
                        break;
                }
                optThreshold[i].UseVisualStyleBackColor = true;

                optThreshold[i].CheckedChanged += new EventHandler(optThreshold_CheckedChanged);
            }

            for (int i = 0; i < picColorPDmy.Length; i++)
            {
                picColorPDmy[i] = new PictureBox();
                picColorPDmy[i].BackColor = SystemColors.Control;
                picColorPDmy[i].BorderStyle = BorderStyle.Fixed3D;
                picColorPDmy[i].Location = new Point(19 + i * 56, 72);
                picColorPDmy[i].Name = "picColorPDmy" + i.ToString();
                picColorPDmy[i].Size = new Size(50, 41);
                picColorPDmy[i].TabIndex = i;
                picColorPDmy[i].TabStop = false;
            }

            for (int i = 0; i < txtLower.Length; i++)
            {
                txtLower[i] = new TextBox();
                txtLower[i].Location = new Point(19 + i * 56, 123);
                txtLower[i].Name = "txtLower" + i.ToString();
                txtLower[i].Size = new Size(49, 23);
                txtLower[i].TabIndex = i;
                txtLower[i].Text = "0";
                txtLower[i].TextAlign = HorizontalAlignment.Center;
                txtLower[i].BorderStyle = BorderStyle.None;
                txtLower[i].ReadOnly = true;

                txtLower[i].Enter += new EventHandler(txtLower_Enter);
                txtLower[i].KeyPress += new KeyPressEventHandler(txtLower_KeyPress);
                txtLower[i].Validating += new CancelEventHandler(txtLower_Validate);
            }
         
            for (int i = 0; i < txtUpper.Length; i++)
            {
                txtUpper[i] = new TextBox();
                txtUpper[i].Location = new Point(19 + i * 56, 180);
                txtUpper[i].Name = "txtUpper" + i.ToString();
                txtUpper[i].Size = new Size(49, 23);
                txtUpper[i].TabIndex = i;
                txtUpper[i].Text = "0";
                txtUpper[i].TextAlign = HorizontalAlignment.Center;

                txtUpper[i].Enter += new EventHandler(txtUpper_Enter);
                txtUpper[i].KeyPress += new KeyPressEventHandler(txtUpper_KeyPress);
                txtUpper[i].Validating += new CancelEventHandler(txtUpper_Validate);
            }

            #endregion

            groupBox1.Controls.AddRange(optColorExe);
            groupBox2.Controls.AddRange(picColorScale);
            groupBox3.Controls.AddRange(optThreshold);
            groupBox3.Controls.AddRange(picColorPDmy);
            groupBox3.Controls.AddRange(txtLower);
            groupBox3.Controls.AddRange(txtUpper);

            //TaPageの表示／非表示切り替えのため、TabPageCtrlオブジェクトを作成
            tabColorPage = new TabPageCtrl(this.tabColor);

            //追加2014/11/28hata_v19.51_dnet
            //カスタムカーソルの設定
            MemoryStream ms = new MemoryStream(Resources.DRAGMOVE);
            DragCur = new Cursor(ms);

            this.ResumeLayout(false);

            #endregion
        }

        //*******************************************************************************
        //機　　能： キャンセルボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdColorCancel_Click(object sender, EventArgs e)
        {
            //dispinf を元に戻す
            //modDispinf.PutDispinf(dispinfBack);
            dispinfBack.Put();


            //画像が変更されているなら元に戻す
            if (frmScanImage.Instance.Changed)
            {
                frmScanImage.Instance.DispOrgImage();
            }

            //フォームのアンロード
            this.Close();
        }

        //*******************************************************************************
        //機　　能： ＯＫボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdColorOk_Click(object sender, EventArgs e)
        {
            //カラーパレット設定処理（現在の設定値をコモンへ登録）
            //モード＝２
            //if (!modImgProc.MyProcOCX.Color(2, modLibrary.GetOption(optColorExe), modLibrary.GetOption(optThreshold), ColorVal, SetColor[0], SpSetCT[0], SpSetPR[0]))
            if (!ImgProc.CtColor(2, modLibrary.GetOption(optColorExe), modLibrary.GetOption(optThreshold),ref ColorVal, SetColor, SpSetCT, SpSetPR))
            {
                //メッセージ表示：設定処理で異常が発生しました。
                MessageBox.Show(CTResources.LoadResString(9982), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        
            //画像の再表示
            frmScanImage.Instance.DispOrgImage();

            //フォームのアンロード
            this.Close();
        }

        //*******************************************************************************
        //機　　能： 表示ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdColorSet_Click(object sender, EventArgs e)
        {
            //カラーパレット設定処理（現在の値を表示画像へ反映）
            //モード＝１
            if (!ImgProc.CtColor(1, modLibrary.GetOption(optColorExe), modLibrary.GetOption(optThreshold),ref ColorVal, SetColor, SpSetCT, SpSetPR))
            {
                //メッセージ表示：設定処理で異常が発生しました。
                MessageBox.Show(CTResources.LoadResString(9982), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
    
            //処理結果画像を表示
            frmScanImage.Instance.DispTempImage();
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
        private void frmColor_Load(object sender, EventArgs e)
        {
            //実行時はフラグをセット
            modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTImageProcessing;

            //フォームを標準位置に移動
            modCT30K.SetPosNormalForm(this);
        
            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);
                                                        
            //dispinfのバックアップ
            //modDispinf.GetDispinf(dispinfBack);
            dispinfBack = new DispInf();
            dispinfBack.Data.Initialize();
            dispinfBack.Load();
                                        
            //カラーパレット設定処理(現在の設定値をコモンから取得)
            //モード＝０
            //modImgProc.MyProcOCX.Color(0, 0, 0, ColorVal, SetColor[0], SpSetCT[0], SpSetPR[0]);
            ImgProc.CtColor(0, 0, 0,ref ColorVal, SetColor, SpSetCT, SpSetPR);

            //カラー処理モードオプションボタンの設定
            modLibrary.SetOption(optColorExe, dispinfBack.Data.colormode, 0);
    
            //８色固定
            ColorVal = 8;
            
            //カラーパレット用ピクチャボックス初期化
            int cnt;
            for (cnt = 0; cnt <= ColorVal - 1; cnt++)
            {
                if (modLibrary.InRange((int)SetColor[cnt], picColorScale.GetLowerBound(0), picColorScale.GetUpperBound(0)))
                {
                    picColorPalet[cnt].BackColor = picColorScale[SetColor[cnt]].BackColor;
                    picColorPDmy[cnt].BackColor = picColorPalet[cnt].BackColor;
                }
            }
 
            //閾値設定：パーセントを選択
            optThreshold[(dispinfBack.Data.color_max == 8191 ? 0 : 1)].Checked = true;

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
        private void frmColor_FormClosed(object sender, FormClosedEventArgs e)
        {
            //終了時はフラグをリセット
            modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTImageProcessing);
        }

        //*******************************************************************************
        //機　　能： カラー処理タブ：カラー種別オプションボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        void optColorExe_CheckedChanged(object sender, EventArgs e)
        {
            //オリジナルを選択したときのみ、「カラースケール作成」タブ、「閾値設定」タブを表示する
            tabColorPage.TabVisible(1, optColorExe[2].Checked); //「カラースケール作成」タブ
            tabColorPage.TabVisible(2, optColorExe[2].Checked); //「閾値設定」タブ
        }

        //*******************************************************************************
        //機　　能： カラースケール作成タブ：カラーパレットドラッグドロップ時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void picColorPalet_DragDrop(object sender, DragEventArgs e)
        {
            //【C#コントロールで代用】
        }

        //*******************************************************************************
        //機　　能： 閾値テキストボックスの入力可・不可をコントロールする（外観も変更）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v10.2  2005/07/14  (SI3)間々田 新規作成
        //*******************************************************************************
        private void ControlTextBox(TextBox theTextBox, bool OK)
        {
            if (OK)
            {
                theTextBox.BackColor = System.Drawing.SystemColors.Window;
                //変更2015/01/23hata
                //theTextBox.Enabled = true;
                theTextBox.ReadOnly = false;
                theTextBox.BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                theTextBox.BackColor = System.Drawing.SystemColors.Control;
                //変更2015/01/23hata
                //theTextBox.Enabled = false;
                theTextBox.ReadOnly = true;
                theTextBox.BorderStyle = BorderStyle.None;
            }
        }

        //*******************************************************************************
        //機　　能： 閾値の種別（％かCT値か）オプションボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v10.2  2005/07/14  (SI3)間々田 新規作成
        //*******************************************************************************
        private void optThreshold_CheckedChanged(object sender, EventArgs e)
        {
            int i = 0;

            ControlTextBox(txtLower[0], optThreshold[1].Checked);
            ControlTextBox(txtUpper[ColorVal - 1], optThreshold[1].Checked);

            lblLowerPercent.Visible = optThreshold[0].Checked;
            lblUpperPercent.Visible = optThreshold[0].Checked;

            //'閾値データ表示：%表示時
            if (optThreshold[0].Checked)
            {
                txtLower[0].Text = "0";
                for (i = 0; i <= ColorVal - 2; i++)
                {
                    txtUpper[i].Text = SpSetPR[i].ToString();
                    txtLower[i + 1].Text = txtUpper[i].Text;
                }
                txtUpper[ColorVal - 1].Text = "100";
            }
            //'閾値データ表示：CT値表示時
            else
            {
                txtLower[0].Text = SpSetCT[0].ToString();
                for (i = 0; i <= ColorVal - 2; i++)
                {
                    txtUpper[i].Text = SpSetCT[i + 1].ToString();
                    txtLower[i + 1].Text = txtUpper[i].Text;
                }

                txtUpper[ColorVal - 1].Text = (SpSetCT[ColorVal]).ToString();
            }
        }

        //*******************************************************************************
        //機　　能： 閾値テキストボックス（下限値）フォーカス取得時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v10.2  2005/07/14  (SI3)間々田 新規作成
        //*******************************************************************************
        private void txtLower_Enter(object sender, EventArgs e)
        {
            int Index = GetIndex(sender, txtLower);

            //それまで表示されていた文字列を記憶
            LastText = txtLower[Index].Text;
        }

        //*******************************************************************************
        //機　　能： 閾値テキストボックス（下限値）キー入力時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v10.2  2005/07/14  (SI3)間々田 新規作成
        //*******************************************************************************
        private void txtLower_KeyPress(object sender, KeyPressEventArgs e)
        {
            int Index = GetIndex(sender, txtLower);

            //Enterキー（Returnキー）を押した場合、入力チェックを行なう
            bool dummy = false;
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtLower_Validate(txtLower[Index], new CancelEventArgs(dummy));
                e.Handled = true;
            }
        }

        //'*******************************************************************************
        //'機　　能： 閾値テキストボックス（下限値）入力チェック処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v10.2  2005/07/14  (SI3)間々田 新規作成
        //'*******************************************************************************
        private void txtLower_Validate(object sender, CancelEventArgs e)
        {
            //（Index-1の）閾値テキストボックス（上限値）入力チェック処理と同じ
            int Index = GetIndex(sender, txtLower);
            
            //変更2015/1/24hata
            //txtUpper_Validate(txtUpper[Index - 1], new CancelEventArgs(false));
            CancelEventArgs ce = new CancelEventArgs(false);
            txtUpperValidate(Index - 1, (Index - 1 < 0) ? txtUpper[0] : txtUpper[Index - 1], ref ce);
        }

        //'*******************************************************************************
        //'機　　能： 閾値テキストボックス（下限値）入力チェック処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v10.2  2005/07/14  (SI3)間々田 新規作成
        //'*******************************************************************************
        private void txtUpper_Enter(object sender, EventArgs e)
        {
            int Index = GetIndex(sender, txtUpper);

            //それまで表示されていた文字列を記憶
            LastText = txtUpper[Index].Text;
        }

        //'*******************************************************************************
        //'機　　能： 閾値テキストボックス（上限値）キー入力時処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v10.2  2005/07/14  (SI3)間々田 新規作成
        //'*******************************************************************************
        private void txtUpper_KeyPress(object sender, KeyPressEventArgs e)
        {
            int Index = GetIndex(sender, txtUpper);

            //Enterキー（Returnキー）を押した場合、入力チェックを行なう
            bool dummy = false;
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtUpper_Validate(txtUpper[Index], new CancelEventArgs(dummy));
                e.Handled = true;
            }
        }

        //'*******************************************************************************
        //'機　　能： 閾値テキストボックス（上限値）入力チェック処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v10.2  2005/07/14  (SI3)間々田 新規作成
        //'*******************************************************************************
        //'*******************************************************************************
        //'機　　能： 閾値テキストボックス（上限値）入力チェック処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v10.2  2005/07/14  (SI3)間々田 新規作成
        //'*******************************************************************************
        private void txtUpper_Validate(object sender, CancelEventArgs e)
        {
            //変更2015/1/24hata
            //TextBox theTextBox = null;
            //int Index = GetIndex(sender, txtUpper);
            //if (Index < 0)
            //{
            //    theTextBox = txtLower[0];
            //}
            //else
            //{
            //    theTextBox = txtUpper[Index];
            //}

            //if (theTextBox.Text == "")
            //{
            //    //メッセージ表示：未入力の項目があります。
            //    MessageBox.Show(CTResources.LoadResString(9903), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    e.Cancel = true;
            //}
            //else if (!IsNumeric(theTextBox.Text))
            //{
            //    //メッセージ表示：入力値が不正です。
            //    //ストリングテーブル化　//v17.60 by 長野 2011/05/22
            //    MessageBox.Show(CTResources.LoadResString(20006), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    e.Cancel = true;
            //}
            //else if (optThreshold[0].Checked && (!modLibrary.InRange(Val(theTextBox.Text), 0, 100)))
            //{
            //    //メッセージ表示：入力値の範囲が不正です。
            //    MessageBox.Show(CTResources.LoadResString(9944), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    e.Cancel = true;
            //}
            //else if (optThreshold[1].Checked && (!modLibrary.InRange(Val(theTextBox.Text), -8192, 8191)))
            //{
            //    //メッセージ表示：入力値の範囲が不正です。
            //    MessageBox.Show(CTResources.LoadResString(9944), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    e.Cancel = true;
            //}

            //if (e.Cancel)
            //{
            //    //キャンセル時はフォーカス前の文字列に戻す
            //    theTextBox.Text = LastText;
            //}
            //else
            //{
            //    if (Index < txtUpper.GetUpperBound(0))
            //    {
            //        //入力された上限値が右隣の上限値より大きい場合
            //        if (Val(theTextBox.Text) > Val(txtUpper[Index + 1].Text))
            //        {
            //            txtUpper[Index + 1].Text = theTextBox.Text; //その入力値を右隣の上限値とする
            //            txtUpper_Validate(sender, e);               //変更した右隣の上限値もチェック（再帰呼び出し）
            //        }

            //        //入力された上限値を右隣の下限値とする
            //        txtLower[Index + 1].Text = theTextBox.Text;
            //    }

            //    if (Index > txtUpper.GetLowerBound(0))
            //    {
            //        //入力された上限値が左隣の上限値より小さい場合
            //        if (Val(theTextBox.Text) < Val(txtUpper[Index - 1].Text))
            //        {
            //            txtUpper[Index - 1].Text = theTextBox.Text; //その入力値を左隣の上限値とする
            //            txtUpper_Validate(sender, e);               //変更した左隣の上限値もチェック（再帰呼び出し）
            //        }
            //    }

            //    //入力された閾値を配列変数に保持：%表示時
            //    if (optThreshold[0].Checked)
            //    {
            //        SpSetPR[Index] = Val(theTextBox.Text);
            //    }
            //    //入力された閾値を配列変数に保持：CT値表示時
            //    else
            //    {
            //        SpSetCT[Index + 1] = Val(theTextBox.Text);
            //    }
            //}
            int Index = GetIndex(sender, txtUpper);
            txtUpperValidate(Index, sender, ref  e);
        }

        //追加2015/1/24hata
        //txtUpperの更新
        private void txtUpperValidate(int Index, object sender, ref CancelEventArgs e)
        {

            if (sender as TextBox == null) return;

            TextBox theTextBox = null;

            if (Index < 0)
            {
                theTextBox = txtLower[0];
            }
            else
            {
                theTextBox = txtUpper[Index];
            }

            if (theTextBox.Text == "")
            {
                //メッセージ表示：未入力の項目があります。
                MessageBox.Show(CTResources.LoadResString(9903), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
            else if (!IsNumeric(theTextBox.Text))
            {
                //メッセージ表示：入力値が不正です。
                //ストリングテーブル化　//v17.60 by 長野 2011/05/22
                MessageBox.Show(CTResources.LoadResString(20006), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
            else if (optThreshold[0].Checked && (!modLibrary.InRange(Val(theTextBox.Text), 0, 100)))
            {
                //メッセージ表示：入力値の範囲が不正です。
                MessageBox.Show(CTResources.LoadResString(9944), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
            else if (optThreshold[1].Checked && (!modLibrary.InRange(Val(theTextBox.Text), -8192, 8191)))
            {
                //メッセージ表示：入力値の範囲が不正です。
                MessageBox.Show(CTResources.LoadResString(9944), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }

            if (e.Cancel)
            {
                //キャンセル時はフォーカス前の文字列に戻す
                theTextBox.Text = LastText;
            }
            else
            {
                if (Index < txtUpper.GetUpperBound(0))
                {
                    //入力された上限値が右隣の上限値より大きい場合
                    if (Val(theTextBox.Text) > Val(txtUpper[Index + 1].Text))
                    {
                        txtUpper[Index + 1].Text = theTextBox.Text; //その入力値を右隣の上限値とする
                        CancelEventArgs ce1 = new CancelEventArgs(false);
                        txtUpper_Validate(txtUpper[Index + 1], ce1);               //変更した右隣の上限値もチェック（再帰呼び出し）
                    }

                    //入力された上限値を右隣の下限値とする
                    txtLower[Index + 1].Text = theTextBox.Text;
                }

                if (Index > txtUpper.GetLowerBound(0))
                {
                    //入力された上限値が左隣の上限値より小さい場合
                    if (Val(theTextBox.Text) < Val(txtUpper[Index - 1].Text))
                    {
                        txtUpper[Index - 1].Text = theTextBox.Text; //その入力値を左隣の上限値とする
                        CancelEventArgs ce2 = new CancelEventArgs(false);
                        txtUpper_Validate(txtUpper[Index - 1], ce2);               //変更した左隣の上限値もチェック（再帰呼び出し）
                    }
                }

                //入力された閾値を配列変数に保持：%表示時
                if (optThreshold[0].Checked)
                {
                    SpSetPR[Index] = Val(theTextBox.Text);
                }
                //入力された閾値を配列変数に保持：CT値表示時
                else
                {
                    SpSetCT[Index + 1] = Val(theTextBox.Text);
                }
            }
        }


        #region ドラッグが開始されたときに発生するイベント

        /// <summary>
        /// ドラッグが開始されたときに発生するイベント
        /// </summary>
        /// <param name="sender">イベントのソース</param>
        /// <param name="e">イベントデータを格納しているオブジェクト</param>
        private void picColorScale_MouseDown(object sender, MouseEventArgs e)
        {
            int Index = -1;
            for (int i = 0; i < picColorScale.Length; i++)
            {
                if (sender.Equals(picColorScale[i]))
                {
                    Index = i;
                    break;
                }
            }

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            //ドラッグ元情報の記録
            dragSource = new DragSource();

            //移動開始位置の記録
            Rectangle startRect = new Rectangle();
            startRect.X = e.X;
            startRect.Y = e.Y;
            dragSource.StartRect = startRect;

            //背景色の記録
            dragSource.BackColor = picColorScale[Index].BackColor;
        }

        #endregion

        #region ドラッグ中に発生するイベント

        /// <summary>
        /// ドラッグ中に発生するイベント
        /// </summary>
        /// <param name="sender">イベントのソース</param>
        /// <param name="e">イベントデータを格納しているオブジェクト</param>
        private void picColorScale_MouseMove(object sender, MouseEventArgs e)
        {
            int Index = -1;
            for (int i = 0; i < picColorScale.Length; i++)
            {
                if (sender.Equals(picColorScale[i]))
                {
                    Index = i;
                    break;
                }
            }

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            //ドラッグ中はマウスカーソル変更
            //変更2014/11/28hata_v19.51_dnet
            //this.Cursor = Cursors.Hand;
            this.Cursor = DragCur;


            //削除2014/11/28hata_v19.51_dnet
            //---ここから
            ////移動後予測表示位置の描画
            //this.Refresh();

            //Control c = (Control)sender;
            //Pen borderPen = new Pen(Color.Black);
            //Graphics g = null;
            //Rectangle rect = new Rectangle();

            //if (this.dragSource != null)
            //{
            //    rect.X = c.Left + e.X - this.dragSource.StartRect.X;
            //    rect.Y = c.Top + e.Y - this.dragSource.StartRect.Y;
            //    rect.Width = c.Width;
            //    rect.Height = c.Height;
            //}

            //g = picColorScale[Index].Parent.CreateGraphics();
            //g.DrawRectangle(borderPen, rect);

            //borderPen.Dispose();
            //g.Dispose();
            //---ここまで

            Point cp = picColorScale[Index].Parent.PointToClient(Cursor.Position); //画面座標をクライアント座標で取得
            int x = cp.X;   //X座標を取得
            int y = cp.Y;   //Y座標を取得


            if ((x < 0) || (x > groupBox2.ClientSize.Width) || (y < 0) || (y > groupBox2.ClientSize.Height))
            {
                this.Cursor = Cursors.No;
            }
        }

        #endregion

        #region ドロップ後に発生するイベント

        /// <summary>
        /// ドロップ後に発生するイベント
        /// </summary>
        /// <param name="sender">イベントのソース</param>
        /// <param name="e">イベントデータを格納しているオブジェクト</param>
        private void picColorScale_MouseUp(object sender, MouseEventArgs e)
        {
            int Index = -1;
            for (int i = 0; i < picColorScale.Length; i++)
            {
                if (sender.Equals(picColorScale[i]))
                {
                    Index = i;
                    break;
                }
            }

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            //ドロップ後の座標を取得
            Point cp = picColorScale[Index].Parent.PointToClient(Cursor.Position);//画面座標をクライアント座標で取得
            int x = cp.X;   //X座標を取得
            int y = cp.Y;   //Y座標を取得

            //パレット内にドロップされたか判定
            for (int i = 0; i < picColorPalet.Length; i++)
            {
                if ((x > picColorPalet[i].Location.X) && (x < picColorPalet[i].Location.X + picColorPalet[i].Width) &&
                    (y > picColorPalet[i].Location.Y) && (y < picColorPalet[i].Location.Y + picColorPalet[i].Height))
                {
                    picColorPalet[i].BackColor = dragSource.BackColor;
                    //追加2014/11/28hata_v19.51_dnet
                    picColorPDmy[i].BackColor = dragSource.BackColor;
                    SetColor[i] = Index;

                    break;
                }
            }

            this.Refresh();

            //マウスカーソルを元に戻す
            this.Cursor = Cursors.Default;
        }

        #endregion

        #region テキストボックス(配列型)のインデックス番号を取得

        /// <summary>
        /// テキストボックス(配列型)のインデックス番号を取得
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="textBoxs"></param>
        private static int GetIndex(object sender, TextBox[] textBoxs)
        {
            int Index = -1;

            for (int i = 0; i < textBoxs.Length; i++)
            {
                if (sender.Equals(textBoxs[i]))
                {
                    Index = i;
                    break;
                }
            }

            return Index;
        }

        #endregion

        #region 文字列→数値

        /// <summary>
        /// 文字列→数値
        /// </summary>
        /// <param name="stTarget">対象となる文字列</param>
        /// <returns>数値</returns>
        public static int Val(string text)
        {
            int result = 0;

            if (IsNumeric(text))
            {
                int.TryParse(text, out result);
            }

            return result;
        }

        #endregion

        #region 文字列が数値であるかどうか判定

        /// <summary>
        /// 文字列が数値であるかどうか判定
        /// </summary>
        /// <param name="stTarget">検査対象となる文字列</param>
        /// <returns>true=指定した文字列が数値, false=数値以外</returns>
        public static bool IsNumeric(string text)
        {
            double result;
            return double.TryParse(text, System.Globalization.NumberStyles.Any, null, out result);
        }

        #endregion
    }

    //変更2015/01/27hata
    //単独のクラス（TabPageCtrl.cs）に移動
    //#region TabControlのTabPage表示／非表示切り替えクラス

    ///// <summary>
    ///// TabControlのTabPage表示／非表示切り替えクラス
    ///// </summary>
    //public class TabPageCtrl
    //{
    //    private class TabPageInfo
    //    {
    //        public TabPage Page;
    //        public bool Visible;

    //        public TabPageInfo(TabPage page, bool isVisible)
    //        {
    //            this.Page = page;
    //            this.Visible = isVisible;
    //        }
    //    }
    //    private TabPageInfo[] tabPageInfos;
    //    private TabControl tabControl;
        
    //    /// <summary>
    //    /// コンストラクタ
    //    /// </summary>
    //    /// <param name="tabCtrl">対象のTabControlオブジェクト</param>
    //    public TabPageCtrl(TabControl tabCtrl)
    //    {
    //        tabControl = tabCtrl;
    //        tabPageInfos = new TabPageInfo[tabControl.TabPages.Count];

    //        for (int i = 0; i < tabControl.TabPages.Count; i++)
    //        {
    //            tabPageInfos[i] = new TabPageInfo(tabControl.TabPages[i], true);
    //        }
    //    }

    //    /// <summary>
    //    /// TabPageを表示／非表示
    //    /// </summary>
    //    /// <param name="index">対象のTabPage番号</param>
    //    /// <param name="isVisible">true=表示, false=非表示</param>
    //    public void TabVisible(int pageNo, bool isVisible)
    //    {
    //        if (tabPageInfos[pageNo].Visible == isVisible)
    //        {
    //            return;
    //        }
    //        tabPageInfos[pageNo].Visible = isVisible;

    //        // コントロールのレイアウト
    //        tabControl.SuspendLayout();

    //        //変更2014/07/27(検S1)hata
    //        //TabPages全体をClearすると、表示がちらつくのでPageごとに表示/非表示を設定する
    //        //tabControl.TabPages.Clear();
    //        //for (int i = 0; i < tabPageInfos.Length; i++)
    //        //{
    //        //    if (tabPageInfos[i].Visible)
    //        //    {
    //        //        tabControl.TabPages.Add(tabPageInfos[i].Page);
    //        //    }
    //        //}
    //        for (int i = 0; i < tabPageInfos.Length; i++)
    //        {
    //            if (tabPageInfos[i].Visible)
    //            {
    //                //表示
    //                if (!tabControl.TabPages.ContainsKey(tabPageInfos[i].Page.Name))
    //                {
    //                    tabControl.TabPages.Insert(i, tabPageInfos[i].Page);
    //                }
    //            }
    //            else
    //            {
    //                //非表示
    //                if (tabControl.TabPages.ContainsKey(tabPageInfos[i].Page.Name))
    //                {
    //                    tabControl.TabPages.Remove(tabPageInfos[i].Page);
    //                }
    //            }
    //        }

    //        tabControl.ResumeLayout();
    //    }

    //}

    //#endregion

    #region ドラッグ元情報クラス

    /// <summary>
    /// ドラッグ元情報クラス
    /// </summary>
    public class DragSource
    {
        // 移動座標保存用変数
        private Rectangle startRect;

        // 背景色
        private Color backColor;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DragSource()
        {
        }

        #region プロパティ

        /// <value>移動座標保存用変数の取得</value>
        public Rectangle StartRect
        {
            get
            {
                return startRect;
            }
            set
            {
                startRect = value;
            }
        }

        /// <value>背景色の取得</value>
        public Color BackColor
        {
            get
            {
                return backColor;
            }
            set
            {
                backColor = value;
            }
        }

        #endregion
    }

    #endregion
}
