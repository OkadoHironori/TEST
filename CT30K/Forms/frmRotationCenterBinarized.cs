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
    ///* プログラム名： frmRotationCenterBinarized.frm                              */
    ///* 処理概要　　： 回転中心校正２値化画像                                      */
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
    ///*             00/02/28    (TOSFEC) 鈴山　修   ビュー切り替え処理の改良       */
    ///* V3.0        00/08/01    (TOSFEC) 鈴山　修   ｺｰﾝﾋﾞｰﾑCT対応                  */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
    public partial class frmRotationCenterBinarized : Form
    {
        private BitmapImageControl BmpICtrl; 

        //public event TransImageChangedEventHandler TransImageChanged;
        //public delegate void TransImageChangedEventHandler();

        //透視画像が変更された
        //public event CaptureOnOffChangedEventHandler CaptureOnOffChanged;
        //キャプチャオンオフ変更時イベント
        public delegate void CaptureOnOffChangedEventHandler(bool IsOn);


        #region インスタンスを返すプロパティ

        // frmRotationCenterBinarizedのインスタンス
        private static frmRotationCenterBinarized _Instance = null;

        /// <summary>
        /// frmRotationCenterBinarizedのインスタンスを返す
        /// </summary>
        public static frmRotationCenterBinarized Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmRotationCenterBinarized();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmRotationCenterBinarized()
        {
            InitializeComponent();

            BmpICtrl = new BitmapImageControl();
            BmpICtrl.MirrorOn = false;
            BmpICtrl.SetLTSize(LookupTableSize.LT16Bit);
            BmpICtrl.WindowLevel = 2048;
            BmpICtrl.WindowWidth = 4096;

            ctlTransImage.MirrorOn = BmpICtrl.MirrorOn;

            ctlTransImage.Parent = picFrame;
            ctlTransImage.Location = new Point(0, 0);            //追加2014/12/22hata_dNet

        }

        //*************************************************************************************************
        //  共通データ宣言
        //*************************************************************************************************

        //メンバ変数
        //入力結果
        bool OK;

        //*************************************************************************************************
        //機    能  ：  ２値化画像のサイズ設定
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  設定に応じて、各コントロールのサイズ・位置を調整する。
        //              基準となる項目は、下記の通り。
        //
        //                横サイズ                     → H_SIZE
        //                ビュー数                     → VIEW_N
        //'
        //履    歴  ：  V2.0   00/02/28  (SI1)鈴山       新規作成
        //*************************************************************************************************
        private void InitControls()
        {
            int sft_val = 0;//追加2014/10/07hata_v19.51反映

            //v18.00シフト対応 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05//追加2014/10/07hata_v19.51反映
            //シフトスキャンでーンビーム用の場合
            //if ((ScanCorrect.IsShiftScan() & (this.Text == StringTable.BuildResStr(StringTable.IDS_BinaryImage, StringTable.IDS_CorRot) + " - " + CTResources.LoadResString(12390))))
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/08/08
            if (((ScanCorrect.IsShiftScan() || ScanCorrect.IsW_Scan())& (this.Text == StringTable.BuildResStr(StringTable.IDS_BinaryImage, StringTable.IDS_CorRot) + " - " + CTResources.LoadResString(12390))))
            {
                sft_val = CTSettings.scancondpar.Data.det_sft_pix;
            }
            else
            {
                sft_val = 0;
            }
            
            //ctlTransImage.SizeX = CTSettings.detectorParam.h_size;
            ctlTransImage.SizeX = CTSettings.detectorParam.h_size + sft_val;    //v18.00シフト対応 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            ctlTransImage.SizeY = ScanCorrect.VIEW_N;
            //ctlTransImage.Width = (int)(ctlTransImage.SizeX / CTSettings.detectorParam.phm);
            //2014/11/07hata キャストの修正
            //ctlTransImage.Width = (int)Math.Round(ctlTransImage.SizeX / CTSettings.detectorParam.phm + 0.5 ,MidpointRounding.AwayFromZero); //小数点切り上げ処理 'v18.00変更 byやまおか 2011/07/24 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            
            //Rev25.00 SizeX/detectorParam.phmがディスプレイの半分を3/4を超えたら半分にする
            int magnify = 1;
            if ((int)((float)Screen.PrimaryScreen.Bounds.Width * (3.0f / 4.0f)) < (Convert.ToInt32(ctlTransImage.SizeX / CTSettings.detectorParam.phm) + 0.5))
            {
                magnify = 2;
            }
            ctlTransImage.Width = Convert.ToInt32(ctlTransImage.SizeX / CTSettings.detectorParam.phm / magnify + 0.5); //小数点切り上げ処理 'v18.00変更 byやまおか 2011/07/24 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            
            ctlTransImage.Height = ctlTransImage.SizeY;
            
            //BmpICtrl.ImageSize = new Size(CTSettings.detectorParam.h_size, ScanCorrect.VIEW_N);
            //Rev23.10 シフト対応 by長野 2015/10/21
            BmpICtrl.ImageSize = new Size(CTSettings.detectorParam.h_size + sft_val, ScanCorrect.VIEW_N);
            

            //垂直スクロールバー
            sldVimg1.Left = ctlTransImage.Width;
            sldVimg1.Height = modLibrary.MinVal(ctlTransImage.Height, 600);

            //一度に表示する高さは600ビューまで
            sldVimg1.Enabled = (ctlTransImage.Height > sldVimg1.Height);
            sldVimg1.Maximum = ctlTransImage.Height - sldVimg1.Height + sldVimg1.LargeChange - 1;

            //ピクチャボックス
            picFrame.Width = ctlTransImage.Width + sldVimg1.Width + 4;
            picFrame.Height = sldVimg1.Height + 4;

            //ビュー数ラベルの位置
            lblVtop.SetBounds(picFrame.Left + picFrame.Width, picFrame.Top, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            lblVbtm.SetBounds(picFrame.Left + picFrame.Width, picFrame.Top + picFrame.Height - lblVbtm.Height, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

            //コントロールフレーム
            fraControl.Top = picFrame.Top + picFrame.Height;

            //フォーム
            this.Width = modLibrary.MaxVal(lblVtop.Left + lblVtop.Width, fraControl.Width) + 8;
            this.Height = picFrame.Height + fraControl.Height + 25;

            //画像の移動とビュー数の表示
            //sldVimg1_Change(0);
            sldVimg1.Value = 0;
        }

        //*************************************************************************************************
        //機　　能： ＯＫボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void cmdOk_Click(object sender, EventArgs e)
        {
            //押したボタンの種類を通知
            OK = true;

            //フォームを消去
            //変更2015/1/17hata_非表示のときにちらつくため
            //Hide();
            modCT30K.FormHide(this);

            //追加2015/1/20hata
            Application.DoEvents(); 

        }

        //*************************************************************************************************
        //機　　能： キャンセルボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            //フォームを消去
            //変更2015/1/17hata_非表示のときにちらつくため
            //Hide();
            modCT30K.FormHide(this);

        }

        //*************************************************************************************************
        //機　　能： フォームロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void frmRotationCenterBinarized_Load(object sender, EventArgs e)
        {

            //削除2015/01/28hata_Dialogで行う
            ////キャプションのセット   'v7.0 リソース対応 by 間々田
            //SetCaption();

            ////各コントロールの初期化
            //InitControls();   //'v18.00削除 Dialog処理で行う byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05  //変更2014/10/07hata_v19.51反映

            //追加2014/12/22hata_dNet
            //Maxの設定
            int integmax = 255;
            sldImg1.Maximum = integmax + sldImg1.LargeChange - 1;

            //Dialogから移動、FormLoadが後になるため
            //----------------------------------------------------
            //２値化画像閾値セット
            sldImg1.Value = ScanCorrect.Threshold255_R / 256;

            //変換対象となる画像の配列を登録
            BmpICtrl.SetImage(ScanCorrect.RC_IMAGE);
            ctlTransImage.Picture = BmpICtrl.Picture;
            //----------------------------------------------------

            //Rev20.00 test by長野 2014/12/15
            //ビュー数を表示
            lblVtop.Text = Convert.ToString(sldVimg1.Value + 1);
            lblVbtm.Text = Convert.ToString(sldVimg1.Value + sldVimg1.Height);

        }

        //sldImg1_ValueChangedに変更
        //*************************************************************************************************
        //機　　能： ２値化閾値スライダー変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        //private void sldImg1_Change(int newScrollValue)
        //{
        //    //閾値の表示
        //    lblBi.Text = Convert.ToString(newScrollValue);

        //    //'２値化画像のしきい値を保存する
        //    //Threshold255_R = CLng(sldImg1.Value) * CLng(256)
        //    //
        //    //Call BinarizeImage(RC_IMAGE(0), BIN_IMAGE(0), h_size, VIEW_N, Threshold255_R, phm, 1)
        //    //Image1.Put BIN_IMAGE, , , , , Image1.SizeX, Image1.SizeY

        //    //２値化画像の場合：ウィンドウ幅を１固定とし、ウィンドウレベルに値をセット
        //    //transImageCtrl.WindowLevel = Convert.ToInt32(256) * Convert.ToInt32(newScrollValue);
        //    //BmpICtrl.WindowWidth = 1;
        //    BmpICtrl.WindowLevel = 256 * newScrollValue;
        //    ctlTransImage.Picture = BmpICtrl.Picture;
        //    ctlTransImage.Invalidate();
        //}
        ////*************************************************************************************************
        ////機　　能： ２値化閾値スライダースクロール時処理
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： V1.00  99/XX/XX   ????????      新規作成
        ////*************************************************************************************************
        //private void sldImg1_Scroll(int newScrollValue)
        //{
        //    //sldImg1_Change(0);
        //    sldImg1_Change(newScrollValue);
        //}
        private void sldImg1_ValueChanged(object sender, EventArgs e)
        {
            //閾値の表示
            lblBi.Text = Convert.ToString(sldImg1.Value);

            //２値化画像の場合：ウィンドウ幅を固定とし、ウィンドウレベルに値をセット
            BmpICtrl.WindowLevel = 256 * sldImg1.Value;
            ctlTransImage.Picture = BmpICtrl.Picture;
            //ctlTransImage.Invalidate();
            ctlTransImage.Refresh();
        }

        //sldVimg1_ValueChangedに変更
        //*************************************************************************************************
        //機　　能： ビュー用スライダー変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        //private void sldVimg1_Change(int newScrollValue)
        //{
        //    //画像の移動
        //    ctlTransImage.Top -= -sldVimg1.Value;

        //    //ビュー数を表示
        //    lblVtop.Text = Convert.ToString(sldVimg1.Value + 1);
        //    lblVbtm.Text = Convert.ToString(sldVimg1.Value + sldVimg1.Height);
        //}

        ////*************************************************************************************************
        ////機　　能： ビュー用スライダースクロール時処理
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： V1.00  99/XX/XX   ????????      新規作成
        ////*************************************************************************************************
        //private void sldVimg1_Scroll(int newScrollValue)
        //{
        //    //画像の移動とビュー数の表示
        //    //sldVimg1_Change(0);
        //    sldVimg1_Change(newScrollValue);
        //}
        private void sldVimg1_ValueChanged(object sender, EventArgs e)
        {   
            //画像の移動
            //ctlTransImage.Top -= -sldVimg1.Value;
            //Rev20.00 以下に変更 by長野 2014/12/15
            ctlTransImage.Top = -sldVimg1.Value;

            //ビュー数を表示
            lblVtop.Text = Convert.ToString(sldVimg1.Value + 1);
            lblVbtm.Text = Convert.ToString(sldVimg1.Value + sldVimg1.Height);

        }

        //*************************************************************************************************
        //機　　能： 各コントロールのキャプションに文字列をセットする
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void SetCaption()
        {
            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            this.Text = StringTable.BuildResStr(StringTable.IDS_BinaryImage, StringTable.IDS_CorRot);//回転中心校正２値化画像
        }

        //*************************************************************************************************
        //機　　能： ダイアログ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： strInfo         [I/ ] String    フォームのキャプションに付加する文字列
        //戻 り 値：                 [ /O] Boolean   True:「ＯＫ」・False:「キャンセル」
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        public bool Dialog(string strInfo = "")
        {
            bool functionReturnValue = false;

            //戻り値用変数初期化
            OK = false;

            //追加2015/01/28hata
            //キャプションのセット   'v7.0 リソース対応 by 間々田
            SetCaption();

            //フォームのキャプションに情報を加える
            if (!string.IsNullOrEmpty(strInfo))
            {
                this.Text = this.Text + " - " + strInfo;
            }

            //追加2014/10/07hata_v19.51反映
            //フォーム初期化(Form_Loadではなく、ここで実行する)
            InitControls(); //v18.00追加 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05


            //この部分の処理はFormLoadに移す
            //--------------------------------------------------
            ////２値化画像閾値セット
            //sldImg1.Value = ScanCorrect.Threshold255_R / 256;

            ////変換対象となる画像の配列を登録
            ////transImageCtrl.SetImage(ScanCorrect.RC_IMAGE);
            //BmpICtrl.SetImage(ScanCorrect.RC_IMAGE,CTSettings.detectorParam.h_size,ScanCorrect.VIEW_N);
            //ctlTransImage.Picture = BmpICtrl.Picture;
            //--------------------------------------------------

            //モーダル表示
            //変更2014/12/22hata_dNet_オーナーフォームを指定する
            //this.ShowDialog();
            this.ShowDialog(frmCTMenu.Instance);

            //戻り値をセット
            functionReturnValue = OK;

            //結果を格納
            if (OK)
            {
                ScanCorrect.Threshold255_R = BmpICtrl.WindowLevel;
            }

            //アンロード
            this.Close();
            return functionReturnValue;
        }

        //使用しない
        ///// <summary>
        ///// ユーザーがスクロール ボックスを移動したときに発生するイベント
        ///// </summary>
        ///// <param name="sender">イベントのソース</param>
        ///// <param name="e">イベントデータを格納しているオブジェクト</param>
        //private void sldImg1_Scroll(object sender, ScrollEventArgs e)
        //{
        //    switch (e.Type)
        //    {
        //        case System.Windows.Forms.ScrollEventType.ThumbTrack:
        //            sldImg1_Scroll(e.NewValue);
        //            break;
        //        case System.Windows.Forms.ScrollEventType.EndScroll:
        //            sldImg1_Change(e.NewValue);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        ///// <summary>
        ///// ユーザーがスクロール ボックスを移動したときに発生するイベント
        ///// </summary>
        ///// <param name="sender">イベントのソース</param>
        ///// <param name="e">イベントデータを格納しているオブジェクト</param>
        //private void sldVimg1_Scroll(object sender,ScrollEventArgs e)
        //{
        //    switch (e.Type)
        //    {
        //        case System.Windows.Forms.ScrollEventType.ThumbTrack:
        //            sldVimg1_Scroll(e.NewValue);
        //            break;
        //        case System.Windows.Forms.ScrollEventType.EndScroll:
        //            sldVimg1_Change(e.NewValue);
        //            break;
        //        default:
        //            break;
        //    }
        //}



    }
}
