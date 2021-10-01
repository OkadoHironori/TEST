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
    ///* プログラム名： Ct値入力.frm                                                */
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
    public partial class CTInputForm : Form
    {
        //イベント宣言
        public event ClickedEventHandler Clicked;
        public delegate void ClickedEventHandler(modImgProc.CTButtonConstants button);

        #region インスタンスを返すプロパティ

        // CTInputFormのインスタンス
        private static CTInputForm _Instance = null;

        /// <summary>
        /// CTInputFormのインスタンスを返す
        /// </summary>
        public static CTInputForm Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new CTInputForm();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CTInputForm()
        {
            InitializeComponent();
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
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        private void cmdOk_Click(object sender, EventArgs e)
        {
            //'入力値を取得する
            GetControls();

            //'Clickedイベント通知
            if (Clicked != null)
            {
                Clicked(modImgProc.CTButtonConstants.btnCTOK);
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
            //'Clickedイベント通知
            if (Clicked != null)
            {
                Clicked(modImgProc.CTButtonConstants.btnCTCancel);
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
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        private void cmdDisp_Click(object sender, EventArgs e)
        {
            //'入力値を取得する
            GetControls();

            //'Clickedイベント通知
            if (Clicked != null)
            {
                Clicked(modImgProc.CTButtonConstants.btnCTDisp);
            }
        }

        //*******************************************************************************
        //機　　能： 入力値を取得する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        private void GetControls()
        {
            //'入力されたBiasとIntervalを取得
            //2014/11/06hata キャストの修正
            modImgProc.CT_Bias = Convert.ToInt16(ntbBias.Value);
            modImgProc.CT_Int = Convert.ToInt16(ntbInterval.Value);
        }

        //*******************************************************************************
        //機　　能： コントロールにデフォルト値を設定する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        private void SetControls()
        {
            ntbBias.Value = modImgProc.CT_Bias;
            ntbInterval.Value = modImgProc.CT_Int;
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
        private void Form_Load(object sender, EventArgs e)
        {
            //'フォームを標準位置に移動
            modCT30K.SetPosNormalForm(this);

            //'Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            //プロパティがCaptionのため設定できないので直接設定する
            ntbBias.Caption = CTResources.LoadResString(12403);
            ntbInterval.Caption = CTResources.LoadResString(12401);
                        
            //'コントロールにデフォルト値を設定する
            SetControls();
        }
    }
}
