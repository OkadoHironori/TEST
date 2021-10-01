using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace CT30K
{
    /* ************************************************************************** */
    /* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    /* 客先　　　　： ?????? 殿                                                   */
    /* プログラム名： frmRoiMessage.frm                                    　　　 */
    /* 処理概要　　： ROIメッセージ表示ダイアログ                                 */
    /* 注意事項　　： なし                                                        */
    /* -------------------------------------------------------------------------- */
    /* 適用計算機　： DOS/V PC                                                    */
    /* ＯＳ　　　　： Windows XP  (SP2)                                           */
    /* コンパイラ　： VB 6.0                                                      */
    /* -------------------------------------------------------------------------- */
    /* VERSION     DATE        BY                  CHANGE/COMMENT                 */
    /*                                                                            */
    /* v15.0       09/04/01    (SI1)    間々田     リニューアル    　　　　       */
    /* v19.00      12/02/21    H.Nagai             BHC対応
    /*                                                                            */
    /* -------------------------------------------------------------------------- */
    /* ご注意：                                                                   */
    /* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    /*                                                                            */
    /* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2009                 */
    /* ************************************************************************** */
    public partial class frmRoiMessage : Form
    {
        #region インスタンスを返すプロパティ

        // frmRoiMessageのインスタンス
        private static frmRoiMessage _Instance = null;

        /// <summary>
        /// frmRoiMessageのインスタンスを返す
        /// </summary>
        public static frmRoiMessage Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmRoiMessage();
                }

                return _Instance;
            }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmRoiMessage()
        {
            InitializeComponent();
        }

        #endregion

        //*******************************************************************************
        //機　　能： 「Exit」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V15.10  09/11/30    やまおか    新規作成
        //*******************************************************************************
        private void cmdExit_Click(object sender, EventArgs e)
        {
            //'非表示
            //変更2015/1/17hata_非表示のときにちらつくため
            //this.Hide();
            modCT30K.FormHide(this);

            Application.DoEvents();

            //'Exit処理
            frmScanImage.Instance.ExitRoi();
        }

        //*******************************************************************************
        //機　　能： 「Go」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V15.10  09/11/30    やまおか    新規作成
        //*******************************************************************************
        private void cmdGo_Click(object sender, EventArgs e)
        {
            //'Go処理
            frmScanImage.Instance.GoRoi();
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
        //履　　歴： V15.10  09/11/30    やまおか    新規作成
        //*******************************************************************************
        private void frmRoiMessage_Load(object sender, EventArgs e)
        {
            //フォームの位置設定
            modCT30K.SetPosNormalForm(this);
            //this.Location = new Point(this.Left, frmScanControl.Instance.Top - this.Height);
            this.SetBounds(this.Left, frmScanControl.Instance.Top - this.Height, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            //タイトルを表示
            //Me.Caption = frmScanImage.ImageProc
            //英語化対応
            this.Text = CTResources.LoadResString(12454);

            //表示コメントを設定     'v15.10追加　byやまおか 2009/11/26
            //lblComment.Caption = "断面像上にROIを描画してから" + vbCrLf + _
            //                     "Goボタンを押してください。"
            lblComment.Text = CTResources.LoadResString(20059);   // 'ストリングテーブル化 'v17.60 by 長野  2011/05/22

            //削除2014/12/22hata_dNet
            //Rev20.00 追加 by長野 2014/12/04
            //this.TopMost = true;
        }
    }
}
