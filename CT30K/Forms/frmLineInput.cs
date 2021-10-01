using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： 寸法座標入力.frm                                            */
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
    public partial class frmLineInput : Form
    {
        private NumericUpDown[] cwnePoint = new NumericUpDown[4];

        #region インスタンスを返すプロパティ

        // frmLineInputのインスタンス
        private static frmLineInput _Instance = null;

        /// <summary>
        /// frmLineInputのインスタンスを返す
        /// </summary>
        public static frmLineInput Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmLineInput();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmLineInput()
        {
            InitializeComponent();

            #region フォームにコントロールを追加

            this.SuspendLayout();

            for (int i = 0; i < cwnePoint.Length; i++)
            {
                this.cwnePoint[i] = new NumericUpDown();

                this.cwnePoint[i].Font = new Font("ＭＳ Ｐゴシック", 12F);
                this.cwnePoint[i].Maximum = new decimal(new int[] { 32767, 0, 0, 0 });
                this.cwnePoint[i].Name = "cwnePoint" + i.ToString();
                this.cwnePoint[i].Size = new Size(65, 23);
                this.cwnePoint[i].TabIndex = i + 1;
                this.cwnePoint[i].TextAlign = HorizontalAlignment.Right;

                switch (i)
                {
                    case 0:
                        this.cwnePoint[i].Location = new Point(112, 33);
                        break;
                    case 1:
                        this.cwnePoint[i].Location = new Point(195, 33);
                        break;
                    case 2:
                        this.cwnePoint[i].Location = new Point(112, 67);
                        break;
                    case 3:
                        this.cwnePoint[i].Location = new Point(195, 67);
                        break;
                    default:
                        break;
                }

                this.fraCoordinate.Controls.Add(this.cwnePoint[i]);
            }

            this.ResumeLayout(false);

            #endregion
        }

        //ＣＴ画像フォーム
        private frmScanImage _frmScanImage;
        public frmScanImage myScanImage
        {
            get { return _frmScanImage; }
            set
            {
                if (_frmScanImage != null)
                {
                    _frmScanImage.RoiChanged -= myScanImage_RoiChanged;
                }

                _frmScanImage = value;
                if (_frmScanImage != null)
                {
                    _frmScanImage.RoiChanged += new EventHandler(myScanImage_RoiChanged);
                }
            }
        }

        private string LastText;

        /// <summary>
        /// 線分番号欄クリック時処理
        /// </summary>
        /// <param name="sender">イベントのソース</param>
        /// <param name="e">イベントデータを格納しているオブジェクト</param>
        private void cboDistLNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            int x1 = 0;
            int y1 = 0;
            int x2 = 0;
            int y2 = 0;

            int.TryParse(cboDistLNo.Text, out i);

            if (modLibrary.InRange(i, 1, DrawRoi.roi.NumOfRois))
            {
                if (DrawRoi.roi.TargetRoi == i)
                {
                    DrawRoi.roi.GetLineShape(i, ref x1, ref y1, ref x2, ref y2);
                    //変更2015/02/02hata_Max/Min範囲のチェック
                    //cwnePoint[0].Value = x1;
                    //cwnePoint[1].Value = y1;
                    //cwnePoint[2].Value = x2;
                    //cwnePoint[3].Value = y2;
                    cwnePoint[0].Value = modLibrary.CorrectInRange(x1, cwnePoint[0].Minimum, cwnePoint[0].Maximum);
                    cwnePoint[1].Value = modLibrary.CorrectInRange(y1, cwnePoint[1].Minimum, cwnePoint[1].Maximum);
                    cwnePoint[2].Value = modLibrary.CorrectInRange(x2, cwnePoint[2].Minimum, cwnePoint[2].Maximum);
                    cwnePoint[3].Value = modLibrary.CorrectInRange(y2, cwnePoint[3].Minimum, cwnePoint[3].Maximum);
                }
                else
                {
                    DrawRoi.roi.SelectRoi(i);
                }
            }
        }

        /// <summary>
        /// 線分番号欄フォーカス取得時処理
        /// </summary>
        /// <param name="sender">イベントのソース</param>
        /// <param name="e">イベントデータを格納しているオブジェクト</param>
        private void cboDistLNo_Enter(object sender, EventArgs e)
        {
            //フォーカスを得たとき、修正前の値を記憶しておく
            LastText = cboDistLNo.Text;
        }

        /// <summary>
        /// 線分番号欄KeyPress処理
        /// </summary>
        /// <param name="sender">イベントのソース</param>
        /// <param name="e">イベントデータを格納しているオブジェクト</param>
        private void cboDistLNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                //数字キーと削除キー
                case (char)Keys.D0:
                case (char)Keys.D1:
                case (char)Keys.D2:
                case (char)Keys.D3:
                case (char)Keys.D4:
                case (char)Keys.D5:
                case (char)Keys.D6:
                case (char)Keys.D7:
                case (char)Keys.D8:
                case (char)Keys.D9:
                case (char)Keys.Back:
                case (char)Keys.Return:
                    cboDistLNo_SelectedIndexChanged(cboDistLNo, new EventArgs());
                    break;
                default:
                    e.KeyChar = (char)0;
                    //e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// 線分番号欄の入力チェック
        /// </summary>
        /// <param name="sender">イベントのソース</param>
        /// <param name="e">イベントデータを格納しているオブジェクト</param>
        private void cboDistLNo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool Cancel = e.Cancel;

            int cboDistLNoValue = 0;
            int.TryParse(cboDistLNo.Text, out cboDistLNoValue);

            if (cboDistLNoValue < 1)
            {
                Cancel = true;
            }
            else if (cboDistLNoValue > DrawRoi.roi.RoiMaxNum)   //'v9.7変更 by 間々田 2004/11/01
            {
                Cancel = true;
            }
            else if (cboDistLNoValue > DrawRoi.roi.NumOfRois + 1)   //'v9.7変更 by 間々田 2004/11/01
            {
                Cancel = true;
            }

            if (Cancel)
            {
                //警告メッセージ表示：線分番号の指定が不正です。
                MessageBox.Show(CTResources.LoadResString(9533), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                cboDistLNo.Text = LastText;
                cboDistLNo_SelectedIndexChanged(cboDistLNo, new System.EventArgs());
            }

            e.Cancel = Cancel;
        }

        //*******************************************************************************
        //機　　能： 表示処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private bool DispRoi()
        {
            //戻り値初期化
            bool functionReturnValue = false;

            //2014/11/06hata キャストの修正
            int x1 = Convert.ToInt32(cwnePoint[0].Value);
            int y1 = Convert.ToInt32(cwnePoint[1].Value);
            int x2 = Convert.ToInt32(cwnePoint[2].Value);
            int y2 = Convert.ToInt32(cwnePoint[3].Value);

            //    If (x1 = x2) And (y1 = y2) Then
            //        MsgBox "指定点１と指定点２では異なる座標にする必要があります。", vbExclamation
            //    End If

            //線分番号の入力チェック
            int no = 0;
            int.TryParse(cboDistLNo.Text, out no);

            //新規番号が入力された場合
            if (no == DrawRoi.roi.NumOfRois + 1)
            {
                //コンボボックスに追加
                cboDistLNo.Items.Add(Convert.ToString(no));

                //線分を追加
                no = DrawRoi.roi.AddLineShape(x1, y1, x2, y2);
                if (no > 0)
                {
                    DrawRoi.roi.SelectRoi(no);
                }

                //既存の線分の座標を修正する
            }
            else if (DrawRoi.roi.SetLineShape(no, x1, y1, x2, y2))
            {
                DrawRoi.roi.SelectRoi(no);
            }

            //戻り値セット
            functionReturnValue = true;
            return functionReturnValue;
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
        private void cmdDisp_Click(object sender, EventArgs e)
        {
            //表示処理
            DispRoi();
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
        private void cmdOk_Click(object sender, EventArgs e)
        {
            //表示処理を行ない、フォームをアンロードする
            if (DispRoi())
            {
                this.Close();
            }
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
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            //フォームのアンロード
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
        //履　　歴： v1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void frmLineInput_Load(object sender, EventArgs e)
        {
            //キャプションのセット
            SetCaption();

            //最大値のセット
            int i = 0;
            for (i = cwnePoint.GetLowerBound(0); i <= cwnePoint.GetUpperBound(0); i++)
            {
                cwnePoint[i].Maximum = frmScanImage.Instance.PicWidth - 1;
            }

            //線分を１本しか描画できない場合：線分番号欄を消して上に詰める
            if (DrawRoi.roi.RoiMaxNum == 1)
            {
                fraLineNo.Visible = false;
                fraCoordinate.Top = fraCoordinate.Top - fraLineNo.Height;
                this.Height = this.Height - fraLineNo.Height;
            }

            //表示中のROIと同期させる
            MyUpdate();

            //ＣＴ画像フォームの参照設定
            myScanImage = frmScanImage.Instance;
        }

        //*************************************************************************************************
        //機　　能： フォームアンロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        private void frmLineInput_FormClosed(object sender, FormClosedEventArgs e)
        {
            //ＣＴ画像フォームの参照破棄
            myScanImage = null;
        }

        //*******************************************************************************
        //機　　能： フォームリサイズ時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void frmLineInput_Resize(object sender, EventArgs e)
        {
            //ボタンの再配置
            //2014/11/06hata
            const int BottomMargin = 17;
            cmdOk.Top = (this.ClientRectangle.Height - cmdOk.Height - BottomMargin);
            cmdCancel.Top = cmdOk.Top;
            cmdDisp.Top = cmdOk.Top;
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
        //履　　歴： V7.00  03/08/25 (SI4)間々田     新規作成
        //*******************************************************************************
        private void SetCaption()
        {
            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            this.Text = StringTable.BuildResStr(StringTable.IDS_CoordinateInput, StringTable.IDS_LineSegment);  //線分座標入力
        }

        //*******************************************************************************
        //機　　能： 表示中のROIと同期させる
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void MyUpdate()
        {
            int i = 0;

            //コンボボックスクリア
            cboDistLNo.Items.Clear();

            //ROI数分コンボボックスに登録
            for (i = 1; i <= DrawRoi.roi.NumOfRois; i++)
            {
                cboDistLNo.Items.Add(Convert.ToString(i));
            }

            //フォーカスROIが存在する場合
            //v9.7変更 by 間々田 2004/11/01
            if (DrawRoi.roi.TargetRoi > 0)
            {
                cboDistLNo.SelectedIndex = DrawRoi.roi.TargetRoi - 1;   //cboDistLNo_Clickがコールされる
            }

            for (i = cwnePoint.GetLowerBound(0); i <= cwnePoint.GetUpperBound(0); i++)
            {
                cwnePoint[i].Enabled = (DrawRoi.roi.TargetRoi > 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">イベントのソース</param>
        /// <param name="e">イベントデータを格納しているオブジェクト</param>
        private void myScanImage_RoiChanged(object sender, EventArgs e)
        {
            MyUpdate();
        }

        //*******************************************************************************
        //機　　能： 座標入力時のエラー処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        //private void cwnePoint_Error(object sender, AxCWUIControlsLib._DCWNumEditEvents_ErrorEvent e)
        //{
        //    int Index = cwnePoint.GetIndex(e);

        //    //コンポーネントワークス側のメッセージを表示しないようにする
        //    e.cancelDisplay = true;
        //}
    }
}
