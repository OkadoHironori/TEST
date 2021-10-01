using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
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
    ///* プログラム名： 画像差分.frm                                                */
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
    public partial class frmSubtraction : Form
    {
        //********************************************************************************
        // 共通データ宣言
        //'********************************************************************************
        private string SaveFileName;    //保存ファイル名

        private const string fontName = "ＭＳ Ｐゴシック";

        private Label[] lblImg;
        private TextBox[] txtImg;
        private Button[] RefGo;

        #region インスタンスを返すプロパティ

        // frmSubtractionのインスタンス
        private static frmSubtraction _Instance = null;

        /// <summary>
        /// frmSubtractionのインスタンスを返す
        /// </summary>
        public static frmSubtraction Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmSubtraction();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmSubtraction()
        {
            InitializeComponent();

            #region ボタンコントロールのインスタンス作成し、プロパティを設定する

            //ラベル＆数値入力 コントロール配列の作成
            lblImg = new Label[2];
            txtImg = new TextBox[2];
            RefGo = new Button[2];

            this.SuspendLayout();

            for (int i = 0; i < 2; i++)
            {
                //インスタンス作成
                lblImg[i] = new Label();
                txtImg[i] = new TextBox();
                RefGo[i] = new Button();

                //プロパティ設定
                lblImg[i].Location = new Point(6, 28 + i * 38);
                lblImg[i].Font = new Font(fontName, 12F);
                lblImg[i].AutoSize = true;
                switch (i)
                {
                    case 0:
                        lblImg[i].Text = "#画像1：";
                        lblImg[i].Tag = "12441";
                        break;
                    case 1:
                        lblImg[i].Text = "#画像2：";
                        lblImg[i].Tag = "12442";
                        break;
                    default:
                        break;
                }

                txtImg[i].Size = new Size(393, 25);
                txtImg[i].Location = new Point(73, 24 + i * 38);
                txtImg[i].Font = new Font(fontName, 12F);
                txtImg[i].TextAlign = HorizontalAlignment.Left;

                RefGo[i].Size = new Size(33, 25);
                RefGo[i].Location = new Point(472, 23 + i * 38);
                RefGo[i].Font = new Font(fontName, 12F);
                RefGo[i].TextAlign = ContentAlignment.MiddleCenter;
                RefGo[i].Text = ">>";

                Controls.Add(lblImg[i]);
                Controls.Add(txtImg[i]);
                Controls.Add(RefGo[i]);

                //イベントハンドラに関連付け
                this.txtImg[i].TextChanged += new EventHandler(txtImg_TextChanged);
                this.txtImg[i].Enter += new EventHandler(txtImg_Enter);
                RefGo[i].Click += new EventHandler(RefGo_Click);
            }


            this.ResumeLayout(false);

            #endregion
        }

        //*******************************************************************************
        //機　　能： 画像ファイル名欄変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 画像ファイル名欄に何も指定されていない時、実行ボタンは使用不可
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        void txtImg_TextChanged(object sender, EventArgs e)
        {
            cmdExe.Enabled = ((txtImg[0].Text != "") & (txtImg[1].Text != ""));
        }

        //*******************************************************************************
        //機　　能： フォーカス取得時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        void txtImg_Enter(object sender, EventArgs e)
        {
            //テキストを反転表示にする       'V4.0 append by 鈴山 2001/03/02 (TOSCANERも同様)
            int Index = -1;
            for(int i = 0; i < txtImg.Length; i++)
            {
                if (sender.Equals(this.txtImg[i]))
                {
                    Index = i;
                    break;
                }
            }

            txtImg[Index].SelectionStart = 0;
            txtImg[Index].SelectionLength = txtImg[Index].Text.Length;
        }

        //*******************************************************************************
        //機　　能： 参照ボタン（>>）クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        void RefGo_Click(object sender, EventArgs e)
        {
            //コモンダイアログによるファイル選択
            string FileName = modFileIO.GetFileName(StringTable.IDS_Select, CTResources.LoadResString(StringTable.IDS_CTImage), ".img");

            //選択したファイル名を該当するテキストボックスに設定
            int Index = -1;
            for (int i = 0; i < RefGo.Length; i++)
            {
                if (sender.Equals(this.RefGo[i]))
                {
                    Index = i;
                    break;
                }
            }

            if (FileName != "")
            {
                txtImg[Index].Text = FileName;
            }
        }

        //*******************************************************************************
        //機　　能： 終了ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdEnd_Click(object sender, EventArgs e)
        {
            //差分処理フォームをアンロード
            this.Close();
        }

        //*******************************************************************************
        //機　　能： 実行ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdExe_Click(object sender, EventArgs e)
        {
            //'v10.2追加ここから by 間々田 2005/07/15
            if (!ImgProc.CtSub(txtImg[0].Text, txtImg[1].Text))
            {
                //メッセージ表示：差分処理に失敗しました。指定された画像ファイルがないかも知れません。
                MessageBox.Show(StringTable.GetResString(9449, CTResources.LoadResString(12482)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //'v10.2追加ここまで by 間々田 2005/07/15
        
            //'処理画像を表示
            frmScanImage.Instance.DispTempImage();
    
            //'保存ファイル名の候補
            SaveFileName = txtImg[1].Text;
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
        private void frmSubtraction_Load(object sender, EventArgs e)
        {
            //'実行時はフラグをセット
            modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTImageProcessing;

            //'v17.60 英語用にレイアウト調整 by長野 2011/05/25
            if (modCT30K.IsEnglish == true)
            {
                EnglishAdjustLayout();
            }

            //'フォームを標準位置に移動
            modCT30K.SetPosNormalForm(this);

            //'Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);
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
        private void frmSubtraction_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseReason UnloadMode = e.CloseReason;

            //画像表示フォームが変更されている場合
            //if (frmScanImage.Instance.Changed &&
            //    (UnloadMode == CloseReason.ApplicationExitCall || UnloadMode == CloseReason.UserClosing))
            if (frmScanImage.Instance.Changed && (UnloadMode == CloseReason.UserClosing))
            {
                //結果保存ダイアログ
                if (!frmImageSave.Instance.Dialog(SaveFileName, CTResources.LoadResString(10702))) //'差画像
                {
                    e.Cancel = true;
                }
            }
        }

        //*******************************************************************************
        //機　　能： フォームアンロード時処理（イベント処理）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Cancel          [ /O] Integer   True（0以外）: アンロードをキャンセル
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void frmSubtraction_FormClosed(object sender, FormClosedEventArgs e)
        {
            //'終了時はフラグをリセット
            modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTImageProcessing);
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
        //履　　歴： V17.60  11/05/25  (検S１)長野      新規作成
        //*******************************************************************************
        private void EnglishAdjustLayout()
        {
            //2014/11/07hata キャストの修正
            //this.Width = (int)(this.Width * 1.1);
            this.Width = Convert.ToInt32(this.Width * 1.1);
            //Rev20.01 変更 by長野 2015/05/19
            //lblImg[0].Left = 40;
            //txtImg[0].Left = lblImg[0].Width + lblImg[0].Left;
            //RefGo[0].Left = txtImg[01].Left + txtImg[0].Width + 2;
            //lblImg[1].Left = 40;
            //txtImg[1].Left = lblImg[1].Width + lblImg[1].Left;
            //RefGo[1].Left = txtImg[1].Left + txtImg[1].Width + 2;

            //lblImg[0].Left = 0;
            txtImg[0].Left = txtImg[0].Left + 30;
            RefGo[0].Left = RefGo[0].Left + 30;
            //lblImg[1].Left = 0;
            txtImg[1].Left = txtImg[1].Left + 30;
            RefGo[1].Left = RefGo[1].Left + 30;

        }
    }
}
