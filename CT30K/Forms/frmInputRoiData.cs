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
    ///* プログラム名： frmInputRoiData.frm                                         */
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
    ///* V4.0        01/03/01    (ITC)    鈴山　修   ﾓｰﾀﾞﾙﾌｫｰﾑ→MDI子ﾌｫｰﾑに変更     */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
    public partial class frmInputRoiData : Form
    {
        #region インスタンスを返すプロパティ

        // frmInputRoiDataのインスタンス
        private static frmInputRoiData _Instance = null;

        /// <summary>
        /// frmInputRoiDataのインスタンスを返す
        /// </summary>
        public static frmInputRoiData Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmInputRoiData();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmInputRoiData()
        {
            InitializeComponent();
        }

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

        private frmTransImage _frmTransImage = null;
        public frmTransImage myTransImage
        {
            get { return _frmTransImage; }
            set
            {
                if (_frmTransImage != null)
                {
                    _frmTransImage.RoiChanged -= frmTransImage_RoiChanged;
                }

                _frmTransImage = value;
                if (_frmTransImage != null)
                {
                    _frmTransImage.RoiChanged += new EventHandler(frmTransImage_RoiChanged);
                }
            }
        }

        //正方形をサポートする？
        private bool myIsSquare;

        //*******************************************************************************
        //機　　能： ImageProcプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.0　2009/03/25   (SI1)間々田   リニューアル
        //*******************************************************************************
        public bool IsSquare
        {
            get { return myIsSquare; }

            set { myIsSquare = value; }
        }

        //*******************************************************************************
        //機　　能： ROI形状コンボボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        private void cmbRoiShape_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbRoiShape.SelectedIndex + 1)
            {
                case (int)RoiData.RoiShape.ROI_CIRC:
                    SetFraCircle();
                    break;
                case (int)RoiData.RoiShape.ROI_RECT:
                    SetFraRectangle();
                    break;
                case (int)RoiData.RoiShape.ROI_SQR:
                    SetFraCircle();
                    break;
                case (int)RoiData.RoiShape.ROI_TRACE:
                    SetFraTrace();
                    break;
                default:
                    break;
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
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            //フォームの消去
            //Me.hide
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
        private void cmdIndicate_Click(object sender, EventArgs e)
        {
            DataChange();
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
        private void cmdOK_Click(object sender, EventArgs e)
        {
            //If DataChange() Then Me.hide
            if (DataChange())
            {
                this.Close();
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
        private void frmInputRoiData_Load(object sender, EventArgs e)
        {
            //キャプションのセット
            SetCaption();

            //フォームの位置の調整
            //Me.Move FmStdLeft, FmStdTop
            //if (modLibrary.IsExistForm(frmRoiTool.Instance))
            if (modLibrary.IsExistForm("frmRoiTool"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            {
                this.SetBounds(frmRoiTool.Instance.Left, (frmRoiTool.Instance.Top + frmRoiTool.Instance.Height), 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            }
            else
            {
                modCT30K.SetPosNormalForm(this);    //v15.0変更 by 間々田 2009/02/20
            }

            //コンボボックスの設定
            cmbRoiShape.Items.Clear();
            cmbRoiShape.Items.Add(CTResources.LoadResString(StringTable.IDS_RoiCircle));      //円
            cmbRoiShape.Items.Add(CTResources.LoadResString(StringTable.IDS_RoiRect));        //長方形
            cmbRoiShape.Items.Add(CTResources.LoadResString(StringTable.IDS_RoiTrace));       //トレース

            //v9.7 if文追加 by 間々田 2004/11/10
            if (IsSquare)
            {
                cmbRoiShape.Items.Add(CTResources.LoadResString(StringTable.IDS_RoiSquare));  //正方形    'Roi処理に正方形はない delete by 間々田 2004/05/12 '→ 復活 by 間々田 2004/10/12
            }

            //ROI情報に合わせてフォームを更新する
            if (DrawRoi.roi.TargetRoi > 0)
            {
                MyUpdate(DrawRoi.roi.TargetRoi);
            }
            else if (DrawRoi.roi.NumOfRois < DrawRoi.roi.RoiMaxNum)
            {
                MyUpdate(DrawRoi.roi.NumOfRois + 1);
            }
            else
            {
                MyUpdate(DrawRoi.roi.NumOfRois);
            }

            //ＣＴ画像フォームの参照設定
            //if (modLibrary.IsExistForm(frmScanImage.Instance))
            if (modLibrary.IsExistForm("frmScanImage"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            {
                //_frmScanImage = frmScanImage.Instance;
                myScanImage = frmScanImage.Instance;

            }

            //if (modLibrary.IsExistForm(frmTransImage.Instance))
            if (modLibrary.IsExistForm("frmTransImage"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            {
                //_frmTransImage = frmTransImage.Instance;
                myTransImage = frmTransImage.Instance;

            }
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
        private void frmInputRoiData_FormClosed(object sender, FormClosedEventArgs e)
        {
            //ＣＴ画像フォームの参照破棄
            //_frmScanImage = null;
            //_frmTransImage = null;
            //Rev20.01 修正 by長野 2015/05/20
            myScanImage = null;
            myTransImage = null;
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

            this.Text = StringTable.GetResString(StringTable.IDS_CoordinateInput, "ROI");       //ROI座標入力
            lblRoiNo.Text = StringTable.LoadResStringWithColon(StringTable.IDS_RoiNo);          //ROI番号：
            lblRoiShape.Text = StringTable.LoadResStringWithColon(StringTable.IDS_RoiShape);    //ROI形状：
        }

        //********************************************************************************
        //機    能  ：  ???????
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //********************************************************************************
        private bool DataChange()
        {
            bool functionReturnValue = false;

            //2014/11/06hata キャストの修正
            int xc = Convert.ToInt32(ntbValue0.Value);
            int yc = Convert.ToInt32(ntbValue1.Value);
            int xl = Convert.ToInt32(ntbValue2.Value);
            int yl = Convert.ToInt32(ntbValue3.Value);

            switch (cmbRoiShape.SelectedIndex + 1)
            {
                case (int)RoiData.RoiShape.ROI_CIRC:
                    functionReturnValue = DataChangeCircle(xc, yc, xl);
                    break;
                case (int)RoiData.RoiShape.ROI_RECT:
                    functionReturnValue = DataChangeRectangle(xc, yc, xl, yl);
                    break;
                case (int)RoiData.RoiShape.ROI_SQR:
                    functionReturnValue = DataChangeSquare(xc, yc, xl);
                    break;
                default:
                    functionReturnValue = true;
                    break;
            }

            return functionReturnValue;
        }

        //********************************************************************************
        //機    能  ：  ???????
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //********************************************************************************
        private bool DataChangeCircle(int xc, int yc, int r)
        {
            //戻り値初期化
             bool functionReturnValue = false;

            if (r <= 0)
            {
                //メッセージ表示：入力された半径は，正しくありません。
                MessageBox.Show(StringTable.BuildResStr(9978, StringTable.IDS_Radius), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!DrawRoi.roi.CheckCircleInArea(xc, yc, r))
            {
                //メッセージ表示：描画範囲に入りません。
                MessageBox.Show(CTResources.LoadResString(StringTable.IDS_InputRoiOut), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //2014/11/06hata キャストの修正
            else if (DrawRoi.roi.GetRoiShape(Convert.ToInt32(CWRoiNo.Value)) == RoiData.RoiShape.ROI_CIRC)
            {
                //2014/11/06hata キャストの修正
                DrawRoi.roi.SetCircleShape(Convert.ToInt32(CWRoiNo.Value), xc, yc, r);   //        roi.TargetRoi = CWRoiNo.Value                      'v9.7削除 by 間々田 2004/11/10
                DrawRoi.roi.SelectRoi(Convert.ToInt32(CWRoiNo.Value));                   //v9.7追加 by 間々田 2004/11/10
                
                functionReturnValue = true;

                //v9.7削除ここから by 間々田 2004/11/10
                //    ElseIf roi.AddCircleShape(CWRoiNo.Value, xc, yc, r) Then
                //
                //        roi.TargetRoi = CWRoiNo.Value
                //        DataChangeCircle = True
                //v9.7削除ここまで by 間々田 2004/11/10

                //v9.7追加ここから by 間々田 2004/11/10
            }
            else
            {
                CWRoiNo.Value = DrawRoi.roi.AddCircleShape(xc, yc, r);
                if (CWRoiNo.Value > 0)
                {
                    //2014/11/06hata キャストの修正
                    DrawRoi.roi.SelectRoi(Convert.ToInt32(CWRoiNo.Value));
                    functionReturnValue = true;
                }
                //v9.7追加ここまで by 間々田 2004/11/10
            }

            return functionReturnValue;
        }

        //********************************************************************************
        //機    能  ：  ???????
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //********************************************************************************
        private bool DataChangeRectangle(int xc, int yc, int xl, int yl)
        {
            int x1 = 0;
            int x2 = 0;
            int y1 = 0;
            int y2 = 0;

            //戻り値初期化
            bool functionReturnValue = false;

            if (xl <= 0)
            {
                //メッセージ表示：X方向の大きさが不正です
                MessageBox.Show(CTResources.LoadResString(StringTable.IDS_InputRoiInvalidX), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return functionReturnValue;
            }
            else if (yl <= 0)
            {
                //メッセージ表示：Y方向の大きさが不正です
                MessageBox.Show(CTResources.LoadResString(StringTable.IDS_InputRoiInvalidY), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return functionReturnValue;
            }

            x1 = xc - xl;
            x2 = xc + xl;
            y1 = yc - yl;
            y2 = yc + yl;

            //領域内？
            if (!DrawRoi.roi.CheckRectInArea(x1, y1, x2, y2))
            {
                //メッセージ表示：描画範囲に入りません。
                MessageBox.Show(CTResources.LoadResString(StringTable.IDS_InputRoiOut), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //2014/11/06hata キャストの修正
            else if (DrawRoi.roi.GetRoiShape(Convert.ToInt32(CWRoiNo.Value)) == RoiData.RoiShape.ROI_RECT)
            {
                //2014/11/06hata キャストの修正
                DrawRoi.roi.SetRectangleShape(Convert.ToInt32(CWRoiNo.Value), x1, y1, x2, y2);//        roi.TargetRoi = CWRoiNo.Value      'v9.7削除 by 間々田 2004/11/10
                DrawRoi.roi.SelectRoi(Convert.ToInt32(CWRoiNo.Value));//v9.7追加 by 間々田 2004/11/10
                functionReturnValue = true;

                //v9.7削除ここから by 間々田 2004/11/10
                //    ElseIf roi.AddRectangleShape(CWRoiNo.Value, x1, y1, x2, y2) Then
                //
                //        roi.TargetRoi = CWRoiNo.Value
                //        DataChangeRectangle = True
                //v9.7削除ここまで by 間々田 2004/11/10

                //v9.7追加ここから by 間々田 2004/11/10
            }
            else
            {
                CWRoiNo.Value = DrawRoi.roi.AddRectangleShape(x1, y1, x2, y2);
                if (CWRoiNo.Value > 0)
                {
                    //2014/11/06hata キャストの修正
                    DrawRoi.roi.SelectRoi(Convert.ToInt32(CWRoiNo.Value));
                    functionReturnValue = true;
                }
                //v9.7追加ここまで by 間々田 2004/11/10
            }

            return functionReturnValue;
        }

        //********************************************************************************
        //機    能  ：  ???????
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //********************************************************************************
        private bool DataChangeSquare(int xc, int yc, int r)
        {
            //戻り値の初期化
            bool functionReturnValue = false;

            if (r <= 0)
            {
                //メッセージ表示：入力された半径は，正しくありません。
                MessageBox.Show(StringTable.BuildResStr(9978, StringTable.IDS_Radius), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!DrawRoi.roi.CheckCircleInArea(xc, yc, r))
            {
                //メッセージ表示：描画範囲に入りません。
                MessageBox.Show(CTResources.LoadResString(StringTable.IDS_InputRoiOut), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                //ROIがすでに存在する場合
            }
            //2014/11/06hata キャストの修正
            else if (DrawRoi.roi.GetRoiShape(Convert.ToInt32(CWRoiNo.Value)) == RoiData.RoiShape.ROI_SQR)
            {
                //2014/11/06hata キャストの修正
                DrawRoi.roi.SetSquareShape(Convert.ToInt32(CWRoiNo.Value), xc, yc, r);   //        roi.TargetRoi = CWRoiNo.Value      'v9.7削除 by 間々田 2004/11/10
                DrawRoi.roi.SelectRoi(Convert.ToInt32(CWRoiNo.Value));                   //v9.7追加 by 間々田 2004/11/10
                
                functionReturnValue = true;

                //v9.7削除ここから by 間々田 2004/11/10
                //    ElseIf roi.AddSquareShape(CWRoiNo.Value, xc, yc, r) Then
                //
                //        roi.TargetRoi = CWRoiNo.Value
                //        DataChangeSquare = True
                //v9.7削除ここまで by 間々田 2004/11/10

                //v9.7追加ここから by 間々田 2004/11/10
            }
            else
            {
                CWRoiNo.Value = DrawRoi.roi.AddSquareShape(xc, yc, r);
                if (CWRoiNo.Value > 0)
                {
                    //2014/11/06hata キャストの修正
                    DrawRoi.roi.SelectRoi(Convert.ToInt32(CWRoiNo.Value));
                    functionReturnValue = true;
                }
                //v9.7追加ここまで by 間々田 2004/11/10
            }

            return functionReturnValue;
        }

        //********************************************************************************
        //機    能  ：  ???????
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //********************************************************************************
        private void SetFraCircle()
        {
            fraRoiInf.Visible = true;
            ntbValue2.Caption = CTResources.LoadResString(StringTable.IDS_lblInputRo);
            //change StrRo → LoadResString(IDS_lblInputRo) by 間々田 2003/07/09 リソース化
            ntbValue3.Visible = false;

            cmdOK.Enabled = true;
            cmdIndicate.Enabled = true;
        }

        //********************************************************************************
        //機    能  ：  ???????
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //********************************************************************************
        private void SetFraRectangle()
        {
            fraRoiInf.Visible = true;
            ntbValue2.Caption = CTResources.LoadResString(StringTable.IDS_lblInputXL);
            //change StrX1 → LoadResString(IDS_lblInputRo) by 間々田 2003/07/09 リソース化
            ntbValue3.Visible = true;

            cmdOK.Enabled = true;
            cmdIndicate.Enabled = true;
        }

        //********************************************************************************
        //機    能  ：  ???????
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //********************************************************************************
        private void SetFraTrace()
        {
            fraRoiInf.Visible = false;
            cmdOK.Enabled = false;
            cmdIndicate.Enabled = false;
        }

        //
        //   編集ROI番号が変更された場合の処理
        //
        private void CWRoiNo_ValueChanged(object sender, EventArgs e)
        {
            //2014/11/06hata キャストの修正
            int roiNoValue = Convert.ToInt32(CWRoiNo.Value);

            MyUpdate(roiNoValue);

            //画像フォーム上のカレントROIの更新
            //    roi.TargetRoi = Value                  'v9.7削除 by 間々田 2004/11/10
            DrawRoi.roi.SelectRoi(roiNoValue);     //v9.7追加 by 間々田 2004/11/10
        }

        private void SetValue(RoiData.RoiShape theShape)
        {
            int xc = 1;
            int yc = 1;
            int xl = 1;
            int yl = 1;

            switch (theShape)
            {
                case RoiData.RoiShape.ROI_CIRC:
                    //2014/11/06hata キャストの修正
                    DrawRoi.roi.GetCircleShape(Convert.ToInt32(CWRoiNo.Value), ref xc, ref yc, ref xl);
                    break;
                case RoiData.RoiShape.ROI_RECT:
                    //2014/11/06hata キャストの修正
                    DrawRoi.roi.GetRectangleShape2(Convert.ToInt32(CWRoiNo.Value), ref xc, ref yc, ref xl, ref yl);
                    break;
                case RoiData.RoiShape.ROI_SQR:
                    //2014/11/06hata キャストの修正
                    DrawRoi.roi.GetSquareShape(Convert.ToInt32(CWRoiNo.Value), ref xc, ref yc, ref xl);
                    break;
                default:
                    break;
            }

            ntbValue0.Value = xc;
            ntbValue1.Value = yc;
            ntbValue2.Value = xl;
            ntbValue3.Value = yl;
        }

        //*******************************************************************************
        //機　　能： 画像のROI情報に基づき、フォームを更新させる
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.0  2008/10/03  (SS1)間々田  新規作成
        //*******************************************************************************
        private void MyUpdate(int no)
        {
            CWRoiNo.Maximum = modLibrary.MinVal(DrawRoi.roi.NumOfRois + 1, DrawRoi.roi.RoiMaxNum);
            CWRoiNo.Value = no;

            RoiData.RoiShape theShape = default(RoiData.RoiShape);

            //ROI形状の取得
            //2014/11/06hata キャストの修正
            theShape = DrawRoi.roi.GetRoiShape(Convert.ToInt32(CWRoiNo.Value));

            //if (theShape == RoiData.RoiShape.NO_ROI)
            //Rev20.01 条件変更 by長野 2015/05/20
            if (theShape == RoiData.RoiShape.NO_ROI)
            {
                if (IsSquare)
                {
                    cmbRoiShape.Enabled = false;
                    cmbRoiShape.SelectedIndex = (int)RoiData.RoiShape.ROI_SQR - 1;
                    SetValue(RoiData.RoiShape.ROI_SQR);
                }
                else
                {
                    cmbRoiShape.Enabled = true;
                    cmbRoiShape.SelectedIndex = 0;
                    SetValue(RoiData.RoiShape.NO_ROI);
                }
            }
            else if (theShape == RoiData.RoiShape.ROI_LINE)
            {
                //Rev20.01 LINEでここに入ってきた場合は、そのまま抜ける by長野 2015/05/20
            }
            else
            {
                cmbRoiShape.Enabled = false;
                cmbRoiShape.SelectedIndex = (int)theShape - 1;
                SetValue(theShape);
            }
        }

        //*******************************************************************************
        //機　　能： ＣＴ画像上のROI変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.0  2008/10/03  (SS1)間々田  新規作成
        //*******************************************************************************
        private void frmScanImage_RoiChanged(object sender, EventArgs e)
        {
            //フォームの更新
            MyUpdate(DrawRoi.roi.TargetRoi);
        }

        //*******************************************************************************
        //機　　能： 透視画像上のROI変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.0  2008/10/03  (SS1)間々田  新規作成
        //*******************************************************************************
        private void frmTransImage_RoiChanged(object sender, EventArgs e)
        {
            //フォームの更新
            MyUpdate(DrawRoi.roi.TargetRoi);
        }
    }
}
