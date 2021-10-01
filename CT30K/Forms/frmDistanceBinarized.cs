using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TransImage;

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： frmDistanceBinarized.frm                                    */
    ///* 処理概要　　： 寸法校正２値化画像                                          */
    ///* 注意事項　　： なし                                                        */
    ///* -------------------------------------------------------------------------- */
    ///* 適用計算機　： DOS/V PC                                                    */
    ///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
    ///* コンパイラ　： VB 6.0                                                      */
    ///* -------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
    ///*                                                                            */
    ///* V1.00       99/XX/XX    (TOSFEC) ????????                                  */
    ///* V2.0        00/02/08    (TOSFEC) 鈴山　修   V1.00を改造                    */
    ///* V3.0        00/08/01    (TOSFEC) 鈴山　修   ｺｰﾝﾋﾞｰﾑCT対応                  */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
    public partial class frmDistanceBinarized : Form
    {
        private BitmapImageControl BmpICtrl; 


        #region インスタンスを返すプロパティ

        // frmDistanceBinarizedのインスタンス
        private static frmDistanceBinarized _Instance = null;

        /// <summary>
        /// frmDistanceBinarizedのインスタンスを返す
        /// </summary>
        public static frmDistanceBinarized Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmDistanceBinarized();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmDistanceBinarized()
        {
            InitializeComponent();

            BmpICtrl = new BitmapImageControl();
            BmpICtrl.MirrorOn = false;
            BmpICtrl.RasterOp = 0;
            BmpICtrl.SetLTSize(LookupTableSize.LTScanImg);
            BmpICtrl.WindowLevel = 0;
            BmpICtrl.WindowWidth = 1;
            ctlTransImage.MirrorOn = BmpICtrl.MirrorOn;

        }

        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************

        //メンバ変数
        //入力結果
        private bool OK;

        //*******************************************************************************
        //機　　能： 水平スクロールバー・値変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void sldImg1_Change(int newScrollValue)
        {
            //    sldImg1_Scroll
            //
            //'    Call BinarizeImage(DISTANCE_IMAGE(0), BIN_IMAGE(0), Xsize_D, Ysize_D, CLng(sldImg1.Value), Xsize_D / Image1.SizeX, Ysize_D / Image1.SizeX)
            //    Call BinarizeImage_signed(DISTANCE_IMAGE(0), BIN_IMAGE(0), Xsize_D, Ysize_D, CLng(sldImg1.Value), Xsize_D / Image1.SizeX, Ysize_D / Image1.SizeX) 'v9.7変更 by 間々田 2004-12-09 符号付Short型配列対応
            //
            //'    Image1.Put BIN_IMAGE, imPlanar, imAllBands, 0, 0, Xsize_D, Ysize_D         'changed by 山本 MIL LITE 7.0対応
            //'    Image1.Put BIN_IMAGE, imPlanar, imAllBands, 0, 0, MATRIX_SIZE, MATRIX_SIZE 'changed by 巻渕 1024×1024画素の時にエラーする対策 2003-01-06
            //    Image1.Put BIN_IMAGE, , , , , Image1.SizeX, Image1.SizeY                    'v9.7変更 by 間々田 2004/11/11

            //値をラベルに表示
            lblBi.Text = Convert.ToString(newScrollValue);

            //２値化画像の場合：ウィンドウ幅を１固定とし、ウィンドウレベルに値をセット
            //ctlTransImage.WindowLevel = newScrollValue;
            BmpICtrl.WindowLevel = newScrollValue;
            ctlTransImage.Picture = BmpICtrl.Picture;
            //ctlTransImage.Invalidate();
            ctlTransImage.Refresh();
        }

        //*******************************************************************************
        //機　　能： 水平スクロールバー・スクロール時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void sldImg1_Scroll(int newScrollValue)
        {
            //sldImg1_Change(0);
            sldImg1_Change(newScrollValue);
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
            //クリックボタンの種類を通知
            OK = true;

            //フォームを非表示にする
            //変更2015/1/17hata_非表示のときにちらつくため
            //Hide();
            modCT30K.FormHide(this);

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
            //フォームを非表示にする
            //変更2015/1/17hata_非表示のときにちらつくため
            //Hide();
            modCT30K.FormHide(this);
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
        private void frmDistanceBinarized_Load(object sender, EventArgs e)
        {
            //キャプションのセット
            SetCaption();

            //ReDim BIN_IMAGE(Image1.SizeX * Image1.SizeY - 1) 'v15.0削除 by 間々田 2009/06/03
            //変換対象となる画像の配列を登録
            ctlTransImage.SizeX = ScanCorrect.Xsize_D;
            ctlTransImage.SizeY = ScanCorrect.Ysize_D;
            //Rev20.00 ctlTransImageに対してbmpを縮小するので変更しない(プロパティの値をそのまま使う) by長野 2014/12/04
            //ctlTransImage.Width = ctlTransImage.SizeX;
            //ctlTransImage.Height = ctlTransImage.SizeY;
            BmpICtrl.ImageSize = new Size(ctlTransImage.SizeY, ctlTransImage.SizeX);

            //変換対象となる画像の配列を登録
            //ctlTransImage.SetImage(ScanCorrect.DISTANCE_IMAGE);
            BmpICtrl.SetImage(ScanCorrect.DISTANCE_IMAGE);
            ctlTransImage.Picture = BmpICtrl.Picture;
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

            this.Text = StringTable.BuildResStr(StringTable.IDS_BinaryImage, StringTable.IDS_CorSize);  //寸法校正２値化画像
        }

        //*******************************************************************************
        //機　　能： ダイアログ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： theMin          [I/ ] Long      閾値の最小値
        //           theMax          [I/ ] Long      閾値の最大値
        //           theThreshold    [ /O] Long      設定された閾値
        //戻 り 値：                 [ /O] Boolean   True:「ＯＫ」・False:「キャンセル」
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        //Public Function Dialog(ByVal theMin As Long, ByVal theMax As Long, ByRef theThreshold As Long) As Boolean
        public bool Dialog(int theMin, int theMax, ref int theThreshold, string strInfo = "")
        {
            bool functionReturnValue = false;   //v17.10変更 byやまおか 2010/08/09

            //戻り値用変数初期化
            OK = false;

            //水平スクロールバーの設定
            sldImg1.Maximum = modLibrary.CorrectInRange((short)theMax, modLibrary.IntMin, modLibrary.IntMax) + sldImg1.LargeChange - 1;
            sldImg1.Minimum = modLibrary.CorrectInRange((short)theMin, modLibrary.IntMin, modLibrary.IntMax);
            sldImg1.Value = ((sldImg1.Maximum - sldImg1.LargeChange + 1) + sldImg1.Minimum) / 2;
            //Rev20.00 追加 by長野 2014/12/04
            sldImg1_Change(sldImg1.Value);

            //フォームのキャプションに情報を加える   'v17.10追加 byやまおか 2010/08/09
            if (!string.IsNullOrEmpty(strInfo))
            {
                this.Text = this.Text + " - " + strInfo;
            }

            //モーダル表示
            //変更2014/12/22hata_dNet_オーナーフォームを指定する
            //ShowDialog();
            ShowDialog(frmCTMenu.Instance);

            //戻り値をセット
            functionReturnValue = OK;
            if (OK)
            {
                theThreshold = sldImg1.Value;
            }

            //フォームのアンロード
            this.Close();
            return functionReturnValue;
        }

        /// <summary>
        /// ユーザーがスクロール ボックスを移動したときに発生するイベント
        /// </summary>
        /// <param name="sender">イベントのソース</param>
        /// <param name="e">イベントデータを格納しているオブジェクト</param>
        private void sldImg1_Scroll(object sender, ScrollEventArgs e)
        {
            switch (e.Type)
            {
                case ScrollEventType.ThumbTrack:
                    sldImg1_Scroll(e.NewValue);
                    break;
                case ScrollEventType.EndScroll:
                    sldImg1_Change(e.NewValue);
                    break;
                default:
                    break;
            }
        }
    }
}
