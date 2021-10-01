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
    ///* プログラム名： 垂直数値.frm                                                */
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
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
    public partial class HoriVertForm : Form
    {
        #region インスタンスを返すプロパティ

        // frmInputRoiDataのインスタンス
        private static HoriVertForm _Instance = null;

        /// <summary>
        /// frmInputRoiDataのインスタンスを返す
        /// </summary>
        public static HoriVertForm Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new HoriVertForm();
                }

                return _Instance;
            }
        }

        #endregion

        
        public HoriVertForm()
        {
            InitializeComponent();
        }


        //水平線？
        bool IsHorizon;

        //ＣＴ画像フォーム
        private frmScanImage _frmScanImage = null;
        public frmScanImage myScanImage
        {
            get { return _frmScanImage; }
            set
            {
                if (_frmScanImage != null)
                {
                    _frmScanImage.RoiChanged -= frmScanImage_RoiChanged;
                }

                _frmScanImage = value;
                if (_frmScanImage != null)
                {
                    _frmScanImage.RoiChanged += new EventHandler(frmScanImage_RoiChanged);
                }
            }
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
            int x1 = 0;
            int y1 = 0;
            int x2 = 0;
            int y2 = 0;

            //線分座標取得
            if (DrawRoi.roi.GetLineShape(1, ref x1, ref y1, ref x2, ref y2))
            {
                //水平線ROI選択時
                if (IsHorizon)
                {
                    //2014/11/07hata キャストの修正
                    //DrawRoi.roi.SetLineShape(1, x1, (int)cwneLine.Value, x2, (int)cwneLine.Value);
                    DrawRoi.roi.SetLineShape(1, x1, Convert.ToInt32(cwneLine.Value), x2, Convert.ToInt32(cwneLine.Value));
                }
                //垂直線ROI選択時
                else
                {
                    //2014/11/07hata キャストの修正
                    //DrawRoi.roi.SetLineShape(1, (int)cwneLine.Value, y1, (int)cwneLine.Value, y2);
                    DrawRoi.roi.SetLineShape(1, Convert.ToInt32(cwneLine.Value), y1, Convert.ToInt32(cwneLine.Value), y2);
                }

                //Roi表示            'v10.01追加 by 間々田 2005/03/03 Roiが正しく表示されないことの対策
                
                //テスト表示2014/07/14hata
                //DrawRoi.roi.IndicateRoi(g);
                //frmScanImage.Instance.Invalidate();
                //roi設定中
                DrawRoi.roi.RoiFlg = 2;
                frmScanImage.Instance.Refresh();
            }
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
            //ROI数値入力処理
            cmdDisp_Click(cmdDisp, new System.EventArgs());

            //フォームをアンロード
            this.Close();
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
            //フォームをアンロード
            this.Close();
        }

        //これは不要
        ////*******************************************************************************
        ////機　　能： 座標入力時のエラー処理
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： V1.00  99/XX/XX   ????????      新規作成
        ////*******************************************************************************
        //private void cwneLine_Error(object sender, AxCWUIControlsLib._DCWNumEditEvents_ErrorEvent e)
        //{
        //    //コンポーネントワークス側のメッセージを表示しないようにする
        //    e.cancelDisplay = true;
        //}
        
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
        private void HoriVertForm_Load(object sender, EventArgs e)
        {
            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            //プロフィールフォームの選択ROI種により水平線座標入力か垂直線座標入力を決める
            //IsHorizon = (frmScanImage.Instance.Toolbar1.Items["HLine"].Checked == true);
            IsHorizon = (frmScanImage.Instance.GetToolBarChecked("tsbtnHLine"));

            if (IsHorizon)
            {
                this.Text = StringTable.BuildResStr(StringTable.IDS_CoordinateInput, StringTable.IDS_Horizon);      //水平線 座標入力
                lblHeader.Text = "Y :";
            }
            else
            {
                this.Text = StringTable.BuildResStr(StringTable.IDS_CoordinateInput, StringTable.IDS_VerticalLine); //垂直線 座標入力
                lblHeader.Text = "X :";
            }

            //画像処理画面の線分情報に基づいて座標を表示
            //変更2015/01/23hata
            //Update();
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
        private void HoriVertForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //ＣＴ画像フォームの参照破棄
            myScanImage = null;
        }

        //*******************************************************************************
        //機　　能： 画像処理画面の線分情報に基づいて座標を表示
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void MyUpdate()
        {
            int x1 = 0;
            int y1 = 0;
            int x2 = 0;
            int y2 = 0;
            decimal max = 0;

            //線分座標取得
            if (DrawRoi.roi.GetLineShape(1, ref x1, ref y1, ref x2, ref y2))
            {
                if (IsHorizon)
                {
                    //変更2015/01/23hata
                    //cwneLine.Value = y1;
                    //cwneLine.Maximum = frmScanImage.Instance.PicHeight / frmScanImage.Instance.Magnify - 1;
                    max = frmScanImage.Instance.PicHeight / frmScanImage.Instance.Magnify - 1;
                    if (max < cwneLine.Value) cwneLine.Value = cwneLine.Minimum;
                    cwneLine.Maximum = max;
                    cwneLine.Value = y1;
                }
                else
                {
                    //変更2015/01/23hata
                    //cwneLine.Value = x1;
                    //cwneLine.Maximum = frmScanImage.Instance.PicWidth / frmScanImage.Instance.Magnify - 1;
                    max = frmScanImage.Instance.PicWidth / frmScanImage.Instance.Magnify - 1;
                    if (max < cwneLine.Value) cwneLine.Value = cwneLine.Minimum;
                    cwneLine.Maximum = max;
                    cwneLine.Value = x1;

                }
                lblRange.Text = "0 - " + Convert.ToString(cwneLine.Maximum);
            }
        }

        private void frmScanImage_RoiChanged(object sender, EventArgs e)
        {
            MyUpdate();
        }
    }
}
