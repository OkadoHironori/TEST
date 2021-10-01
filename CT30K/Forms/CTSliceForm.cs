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
    ///* プログラム名： 閾値入力.frm                                                */
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
    public partial class CTSliceForm : Form
    {
        private NumTextBox[] ntbValue = null ;

        private static CTSliceForm _Instance = null;    // CTSliceFormのインスタンス

        //コントロールのインデックス値
        public enum ValueIndexType
        {
            IndexLower = 0,
            IndexUpper = 1,
            IndexUnit = 2
        }

        //イベント宣言
        public event ClickedEventHandler Clicked;
        public delegate void ClickedEventHandler(modImgProc.CTButtonConstants button);

        //２値化画像閾値入力モードか
        public bool myBinaryImageThreshold;

        //前回値
        private decimal PreviousValue;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CTSliceForm()
        {
            InitializeComponent();

            #region コントロールのインスタンス作成し、プロパティを設定する

            this.SuspendLayout();

            //追加2014/12/22hata_dNet
            ntbValue = new NumTextBox[3];

            for (int i = 0; i < this.ntbValue.Length; i++)
            {
                this.ntbValue[i] = new NumTextBox();

                this.ntbValue[i].BorderStyle = BorderStyle.Fixed3D;
                this.ntbValue[i].CaptionAlignment = ContentAlignment.MiddleRight;
                this.ntbValue[i].CaptionFont = new Font("ＭＳ Ｐゴシック", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(128)));
                this.ntbValue[i].CaptionWidth = 156;
                this.ntbValue[i].DiscreteInterval = 1F;
                this.ntbValue[i].Font = new Font("ＭＳ Ｐゴシック", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(128)));
                this.ntbValue[i].IncDecButton = false;
                this.ntbValue[i].Name = "ntbBias" + i.ToString();
                this.ntbValue[i].Unit = "";
                this.ntbValue[i].TabIndex = i + 1;
                this.ntbValue[i].Size = new Size(253, 24);
                this.ntbValue[i].Location = new Point(-4, 16 + i * 40);

                switch (i)
                {
                    case 0:
                        this.ntbValue[i].Caption = "#下限閾値";
                        this.ntbValue[i].Tag = "12505";
                        this.ntbValue[i].Max = new decimal(new int[] { 8191, 0, 0, 0 });
                        this.ntbValue[i].Min = new decimal(new int[] { 8192, 0, 0, -2147483648 });
                        this.ntbValue[i].Value = new decimal(new int[] { 8192, 0, 0, -2147483648 });
                        break;
                    case 1:
                        this.ntbValue[i].Caption = "#上限閾値";
                        this.ntbValue[i].Tag = "12506";
                        this.ntbValue[i].Max = new decimal(new int[] { 8191, 0, 0, 0 });
                        this.ntbValue[i].Min = new decimal(new int[] { 8192, 0, 0, -2147483648 });
                        this.ntbValue[i].Value = new decimal(new int[] { 8191, 0, 0, 0 });
                        break;
                    case 2:
                        this.ntbValue[i].Caption = "#連結幅";
                        this.ntbValue[i].Tag = "12507";
                        this.ntbValue[i].Max = new decimal(new int[] { 2147483647, 0, 0, 0 });
                        this.ntbValue[i].Min = new decimal(new int[] { 0, 0, 0, 0 });
                        this.ntbValue[i].Value = new decimal(new int[] { 0, 0, 0, 0 });
                        break;
                    default:
                        break;
                }

                //イベントハンドラに関連付け
                this.ntbValue[i].ValueChanged += new NumTextBox.ValueChangedEventHandler(ntbValue_ValueChanged);

                //フォームにコントロールを追加
                this.Controls.Add(this.ntbValue[i]);
            }

            this.ResumeLayout(false);

            #endregion
        }

        #region インスタンスを返すプロパティ

        /// <summary>
        /// CTSliceFormのインスタンスを返す
        /// </summary>
        public static CTSliceForm Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new CTSliceForm();
                }

                return _Instance;
            }
        }

        //*******************************************************************************
        //機　　能： ２値化画像閾値入力モードプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.0　2009/03/25   (SI1)間々田   リニューアル
        //*******************************************************************************
        public bool BinaryImageThreshold
        {
            get
            {
                return myBinaryImageThreshold;
            }
            set
            {
                myBinaryImageThreshold = value;
            }
        }

        #endregion

        //*************************************************************************************************
        //機　　能： ＯＫボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void cmdOk_Click(object sender, EventArgs e)
        {
            //入力値を取得する
            GetControls();

            //Clickedイベント通知
            if (Clicked != null)
            {
                Clicked(modImgProc.CTButtonConstants.btnCTOK);
            }
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
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            //Clickedイベント通知
            if (Clicked != null)
            {
                Clicked(modImgProc.CTButtonConstants.btnCTCancel);
            }
        }

        //*************************************************************************************************
        //機　　能： 表示ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void cmdDisp_Click(object sender, EventArgs e)
        {
            //入力値を取得する
            GetControls();

            //Clickedイベント通知
            if (Clicked != null)
            {
                Clicked(modImgProc.CTButtonConstants.btnCTDisp);
            }

            PreviousValue = ntbValue[(int)ValueIndexType.IndexUnit].Value;
        }

        //*************************************************************************************************
        //機　　能： 入力値を取得する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void GetControls()
        {
            //'２値化画像閾値入力モードか
            if (myBinaryImageThreshold)
            {
                //2014/11/06hata キャストの修正
                modImgProc.CT_Low1Point = Convert.ToInt16(ntbValue[(int)ValueIndexType.IndexLower].Value);
                modImgProc.CT_High1Point = Convert.ToInt16(ntbValue[(int)ValueIndexType.IndexUpper].Value);
            }
            else
            {
                //2014/11/06hata キャストの修正
                modImgProc.CT_Low = Convert.ToInt16(ntbValue[(int)ValueIndexType.IndexLower].Value);
                modImgProc.CT_High = Convert.ToInt16(ntbValue[(int)ValueIndexType.IndexUpper].Value);
                modImgProc.CT_Unit = Convert.ToInt16(ntbValue[(int)ValueIndexType.IndexUnit].Value);
            }
        }

        //*************************************************************************************************
        //機　　能： コントロールにデフォルト値を設定する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void SetControls()
        {
            //'２値化画像閾値入力モードか
            if (myBinaryImageThreshold)
            {
                ntbValue[(int)ValueIndexType.IndexLower].Value = (decimal)modImgProc.CT_Low1Point;
                ntbValue[(int)ValueIndexType.IndexUpper].Value = (decimal)modImgProc.CT_High1Point;
            }
            else
            {
                ntbValue[(int)ValueIndexType.IndexLower].Value = (decimal)modImgProc.CT_Low;
                ntbValue[(int)ValueIndexType.IndexUpper].Value = (decimal)modImgProc.CT_High;

                //'連結幅
                ntbValue[(int)ValueIndexType.IndexUnit].Max = frmScanImage.Instance.PicWidth - 1;
                ntbValue[(int)ValueIndexType.IndexUnit].Value = modImgProc.CT_Unit;
                ntbValue[(int)ValueIndexType.IndexUnit].Visible = true;
            }
        }

        //*************************************************************************************************
        //機　　能： 値変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void ntbValue_ValueChanged(object sender, NumTextBox.ValueChangedEventArgs e)
        {
            int Index = -1;

            foreach (int i in Enum.GetValues(typeof(ValueIndexType)))
            {
                if (sender.Equals(this.ntbValue[i]))
                {
                    Index = i;
                    break;
                }
            }
            
            if ((Index != (int)ValueIndexType.IndexUnit) &&
                (ntbValue[(int)ValueIndexType.IndexLower].Value > ntbValue[(int)ValueIndexType.IndexUpper].Value))
            {
                //メッセージ表示：下限閾値が上限閾値を越えています。
                MessageBox.Show(StringTable.BuildResStr(9943, StringTable.IDS_MinThreshold, StringTable.IDS_MaxThreshold), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                //変更2015/01/24hata
                //decimal previousValue = 0;
                //if (decimal.TryParse(((UpDownBase)sender).Text, out previousValue))
                //{
                //    ntbValue[Index].Value = previousValue;
                //}
                ntbValue[Index].Value = e.PreviousValue;
            }
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
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void CTSliceForm_Load(object sender, EventArgs e)
        {
            //フォームを標準位置に移動
            modCT30K.SetPosNormalForm(this);

            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            //コントロールにデフォルト値を設定する
            SetControls();
            //下限閾値と上限閾値の配置を入れ替える   'v15.10追加 byやまおか 2009/11/30
            ntbValue[1].Top = 16;     //'上限値を上段に表示
            //labels[1].Top = 16 + 1;
            ntbValue[0].Top = 56 - 4; //'下限値を中段に表示
            //labels[0].Top = 56 - 1;
            ntbValue[1].TabIndex = 0;       //'タブインデックス0
            ntbValue[0].TabIndex = 1;       //'タブインデックス1
            
            //表示ボタンをクリック
            this.cmdDisp_Click(sender, e);
        }


    }
}
