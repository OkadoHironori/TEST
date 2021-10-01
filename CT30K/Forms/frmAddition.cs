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
    ///* プログラム名： 画像加算.frm                                                */
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
    public partial class frmAddition : Form
    {
        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************
        private string SaveFileName;    //'保存ファイル名

        private CTListBox lstImageFile;

        #region インスタンスを返すプロパティ

        // frmAdditionのインスタンス
        private static frmAddition _Instance = null;

        /// <summary>
        /// frmAdditionのインスタンスを返す
        /// </summary>
        public static frmAddition Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmAddition();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmAddition()
        {
            InitializeComponent();

            #region lstImageFileコントロールのインスタンス作成し、プロパティを設定する

            this.SuspendLayout();

            lstImageFile = new CTListBox();
            lstImageFile.Font = new Font("ＭＳ Ｐゴシック", 12F);
            lstImageFile.Location = new Point(16, 32);
            lstImageFile.Name = "lstImageFile";
            lstImageFile.Size = new Size(353, 216);
            lstImageFile.TabIndex = 1;
            lstImageFile.Max = 100;

            Controls.Add(lstImageFile);

            this.ResumeLayout(false);

            #endregion
        }

        //'*******************************************************************************
        //'機　　能： 終了ボタンクリック時処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*******************************************************************************
        private void addEnd_Click(object sender, EventArgs e)
        {
            //'加算処理フォームをアンロードする
            this.Close();
        }

        //'*******************************************************************************
        //'機　　能： 実行ボタンクリック時処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*******************************************************************************
        private void AddGo_Click(object sender, EventArgs e)
        {
            string fileNameList = null;
            int i = 0;

            //加算する画像が指定されているかチェック
            if (lstImageFile.ListCount < 2)
            {
                //メッセージ表示：対象画像が２枚以上指定されていません。
                
                MessageBox.Show(CTResources.LoadResString(StringTable.IDS_DoSelectImages), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //FileNameList = .List(0) & " "
            fileNameList = lstImageFile.List(0);

            //選択している画像ファイル名を１行にまとめる
            for (i = 1; i <= lstImageFile.ListCount - 1; i++)
            {
                //FileNameList = FileNameList & .List(i) & " "
                fileNameList = fileNameList + "|" + lstImageFile.List(i);   //v10.01変更 区切り文字を空白から"|"に変更した by 間々田 2005/03/03
            }                                                               //理由：空白もファイル名に使用されることがあるので。

            //マウスポインタを砂時計にする
            Cursor.Current = Cursors.WaitCursor;

            //加算処理実行：成功した場合、保存ファイル名（候補）を設定
            SaveFileName = (ImgProc.CtAdd(fileNameList) ? lstImageFile.List(lstImageFile.ListCount - 1) : "");

            //マウスポインタを元に戻す
            Cursor.Current = Cursors.Default;

            if (string.IsNullOrEmpty(SaveFileName))
            {
                //メッセージ表示：加算処理に失敗しました。指定された画像ファイルがないかも知れません。
                string msg = StringTable.GetResString(9449, CTResources.LoadResString(12481));

                MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);  
            }
            else
            {
                //処理画像を表示
                frmScanImage.Instance.DispTempImage();
            }
        }

        //'*******************************************************************************
        //'機　　能： フォームロード時の処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*******************************************************************************
        private void frmAddition_Load(object sender, EventArgs e)
        {
            //'実行時はフラグをセット
            modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTImageProcessing;

            //'フォームを標準位置に移動
            modCT30K.SetPosNormalForm(this);

            //'Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);
                                     
            //'各コントロールの初期化
            InitControls();
        }

        //'*******************************************************************************
        //'機　　能： QueryUnload イベント処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v1.00  99/XX/XX   ????????      新規作成
        //'*******************************************************************************
        private void frmAddition_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseReason UnloadMode = e.CloseReason;

            //画像表示フォームが変更されている場合
            //if (frmScanImage.Instance.Changed &&
            //    (UnloadMode == CloseReason.ApplicationExitCall || UnloadMode == CloseReason.UserClosing))
            if (frmScanImage.Instance.Changed && (UnloadMode == CloseReason.UserClosing))
            {
                //結果保存ダイアログ
                if (!frmImageSave.Instance.Dialog(SaveFileName, CTResources.LoadResString(10701)))//和画像
                {
                    e.Cancel = true;
                }
            }
        }

        //'*******************************************************************************
        //'機　　能： フォームアンロード時処理（イベント処理）
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： Cancel          [ /O] Integer   True（0以外）: アンロードをキャンセル
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v1.00  99/XX/XX   ????????      新規作成
        //'*******************************************************************************
        private void frmAddition_FormClosed(object sender, FormClosedEventArgs e)
        {
            //終了時はフラグをリセット
            modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTImageProcessing);
        }

        //'*******************************************************************************
        //'機　　能： 各コントロールの初期化
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //'*******************************************************************************
        private void InitControls()
        {
            //'リストコントロールボックス

            //'項目削除用のボタンの設定
            lstImageFile.DeleteButton = cmdImgDelete;

            //'リンクする「参照」ボタンの設定
            lstImageFile.ReferenceButton = cmdRef;
        
            //'項目数表示ラベルの設定
            lstImageFile.CountLabel = lblCount;
    
            //'このリストの内容：画像ファイル
            lstImageFile.Description = CTResources.LoadResString(StringTable.IDS_CTImage);

            //'拡張子の設定   //追加2014/07/16hata
            lstImageFile.Extension = ".img";

            //'最大表示枚数の表示
            lblMaxNum.Text = StringTable.GetResString(StringTable.IDS_FramesWithMax, Convert.ToString(lstImageFile.Max));

        }
    }
}
