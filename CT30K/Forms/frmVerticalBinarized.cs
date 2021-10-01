using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TransImage;

namespace CT30K
{
    public partial class frmVerticalBinarized : Form
    {
        //表示画像クラス
        private BitmapImageControl BmpICtrl; 

        //public event TransImageChangedEventHandler TransImageChanged;
        //public delegate void TransImageChangedEventHandler();

        //透視画像が変更された
        //public event CaptureOnOffChangedEventHandler CaptureOnOffChanged;
        //キャプチャオンオフ変更時イベント
        //public delegate void CaptureOnOffChangedEventHandler(bool IsOn);


        #region インスタンスを返すプロパティ

        // frmVerticalBinarizedのインスタンス
        private static frmVerticalBinarized _Instance = null;

        /// <summary>
        /// frmVerticalBinarizedのインスタンスを返す
        /// </summary>
        public static frmVerticalBinarized Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmVerticalBinarized();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmVerticalBinarized()
        {
            InitializeComponent();

            BmpICtrl = new BitmapImageControl();
            BmpICtrl.MirrorOn = false;
            BmpICtrl.SetLTSize(LookupTableSize.LT16Bit);
            BmpICtrl.WindowLevel = 32768;
            BmpICtrl.WindowWidth = 1;
            ctlTransImage.MirrorOn = BmpICtrl.MirrorOn;
        
        }

        //*************************************************************************************************
        //  共通データ宣言
        //*************************************************************************************************

        //メンバ変数
        private bool OK;    //入力結果

        //*************************************************************************************************
        //機　　能： 水平スクロールバー・値変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void sldImg1_Change(int newScrollValue)
        {
            //値をラベルに表示
            lblBi.Text = Convert.ToString(newScrollValue);

            //２値化画像の場合：ウィンドウ幅を１固定とし、ウィンドウレベルに値をセット
            //ctlTransImage.WindowLevel = Convert.ToInt32(256) * Convert.ToInt32(newScrollValue);
            BmpICtrl.WindowLevel = Convert.ToInt32(256) * Convert.ToInt32(newScrollValue);
            ctlTransImage.Picture = BmpICtrl.Picture;
            //ctlTransImage.Invalidate();       
            ctlTransImage.Refresh();

        }

        //*************************************************************************************************
        //機　　能： 水平スクロールバー・スクロール時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void sldImg1_Scroll(int newScrollValue)
        {
            //sldImg1_Change(0);
            sldImg1_Change(newScrollValue);
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
        private void frmVerticalBinarized_Load(object sender, EventArgs e)
        {
            //キャプションのセット
            SetCaption();

            //コントロールの初期化
            InitControls();

            //変換対象となる画像の配列を登録
            //ctlTransImage.SetImage(ScanCorrect.CVRT_IMAGE);

            BmpICtrl.SetImage(ScanCorrect.CVRT_IMAGE);
            ctlTransImage.Picture = BmpICtrl.Picture;
            //ctlTransImage.Invalidate();
            ctlTransImage.Refresh();

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

            this.Text = StringTable.BuildResStr(StringTable.IDS_BinaryImage, StringTable.IDS_CorDistortion);    //幾何歪校正２値化画像
        }

        //*************************************************************************************************
        //機　　能： コントロールの初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void InitControls()
        {
            //透視画像表示コントロール
            ctlTransImage.Top = 0;
            ctlTransImage.Left = 0;
            ctlTransImage.SizeX = CTSettings.detectorParam.h_size;
            ctlTransImage.SizeY = CTSettings.detectorParam.v_size;
            //2014/11/07hata キャストの修正
            //ctlTransImage.Width = (int)(CTSettings.detectorParam.h_size / CTSettings.detectorParam.phm);
            //ctlTransImage.Height = (int)(CTSettings.detectorParam.v_size / CTSettings.detectorParam.pvm);
            ctlTransImage.Width = Convert.ToInt32(CTSettings.detectorParam.h_size / CTSettings.detectorParam.phm);
            ctlTransImage.Height = Convert.ToInt32(CTSettings.detectorParam.v_size / CTSettings.detectorParam.pvm);           
            
            BmpICtrl.ImageSize = new Size(CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);

            //値をラベルに表示
            lblBi.Text = Convert.ToString(sldImg1.Value);

            //フォーム
            this.Width = modLibrary.MaxVal(ctlTransImage.Width, fraControl.Width) + 4;
            this.Height = ctlTransImage.Height + fraControl.Height + 25;

            //コントロールフレーム
            //2014/11/07hata キャストの修正
            //fraControl.SetBounds(ClientRectangle.Width / 2 - fraControl.Width / 2, (ctlTransImage.Height), 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            fraControl.SetBounds(Convert.ToInt32(ClientRectangle.Width / 2F - fraControl.Width / 2F), ctlTransImage.Height, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

        }

        //*************************************************************************************************
        //機　　能： ダイアログ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [ /O] Boolean   True:「ＯＫ」・False:「キャンセル」
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        public bool Dialog()
        {
            bool functionReturnValue = false;

            //戻り値用変数初期化
            OK = false;

            //追加2014/12/22hata_dNet
            sldImg1.Maximum = 255 + sldImg1.LargeChange - 1;

            //水平スクロールバーの初期値
            sldImg1.Value = (sldImg1.Minimum + (sldImg1.Maximum - sldImg1.LargeChange + 1)) / 2;
            
            //モーダル表示
            //変更2014/12/22hata_dNet_オーナーフォームを指定する
            //ShowDialog();
            this.ShowDialog(frmCTMenu.Instance);

            //結果を格納
            if (OK)
            {
                //ScanCorrect.Threshold255_V = ctlTransImage.WindowLevel;
                ScanCorrect.Threshold255_V = BmpICtrl.WindowLevel;
            }

            //戻り値をセット
            functionReturnValue = OK;

            //アンロード
            this.Close();
            return functionReturnValue;
        }
        /// <summary>
        /// ユーザーがスクロールボックスを移動したときに発生するイベント
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
