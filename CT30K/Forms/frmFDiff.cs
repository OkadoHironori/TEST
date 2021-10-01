using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CT30K
{
    public partial class frmFDiff : Form
    {
        ///* ************************************************************************** */
        ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
        ///* 客先　　　　： ?????? 殿                                                   */
        ///* プログラム名： frmFDiff.frm                                                 */
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

        #region メンバ変数
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmFDiff myForm = null;
        #endregion

        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************
        float[] DFilter1;
        float[] DFilter2;
        float[] DFilter3;
        float[] DFilter4;
        float[] DFilter5;

        #region サポートしているイベント
        /// <summary>
        /// サポートしているイベント
        /// </summary>
        public event UnloadedEventHandler Unloaded;
        public delegate void UnloadedEventHandler();    //アンロードされ
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmFDiff()
        {
            InitializeComponent();
        }
        #endregion

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmFDiff Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmFDiff();
                }

                return myForm;
            }
        }
        #endregion    

        //*******************************************************************************
        //機　　能： 微分フィルタ番号スクロールバー・値変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void hsbDiffFilter_Change(int newScrollValue)
        {
            //値の表示
            lblDiffFilter.Text = Convert.ToString(newScrollValue);


        }

        //hsbDiffFilterのスクロールイベント
        private void hsbDiffFilter_Scroll(System.Object eventSender, System.Windows.Forms.ScrollEventArgs eventArgs)
        {
            switch (eventArgs.Type)
            {
                case System.Windows.Forms.ScrollEventType.EndScroll:
                    //スクロールを止めた位置で実行
                    hsbDiffFilter_Change(eventArgs.NewValue);
                    break;
            }
        }

        //*******************************************************************************
        //機　　能： 「実行」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdExe_Click(object sender, EventArgs e)
        {
            //マウスを砂時計にする
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //透視画像フォームに対して
            var _with1 = frmTransImage.Instance;
            //空間フィルタをかける
            switch (hsbDiffFilter.Value)
            {
                case 1:
                    _with1.IP_Filter(ref DFilter1);
                    break;
                case 2:
                    _with1.IP_Filter(ref DFilter2);
                    break;
                case 3:
                    _with1.IP_Filter(ref DFilter3);
                    break;
                case 4:
                    _with1.IP_Filter(ref DFilter4);
                    break;
                case 5:
                    _with1.IP_Filter(ref DFilter5);
                    break;
            }

            //マウスを元に戻す
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        //*******************************************************************************
        //機　　能： 「閉じる」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
       private void cmdEnd_Click(object sender, EventArgs e)
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
       //履　　歴： V1.00  99/XX/XX   ????????      新規作成
       //*******************************************************************************
       private void frmFDiff_Load(object sender, EventArgs e)
       {

           //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
           StringTable.LoadResStrings(this);

           //微分フィルタ番号の設定
           hsbDiffFilter.Value = CTSettings.scanParam.DiffFilterNo;

           DFilter1 = new float[3 * 3];
           DFilter2 = new float[5 * 5];
           DFilter3 = new float[5 * 5];
           DFilter4 = new float[7 * 7];
           DFilter5 = new float[7 * 7];

           //空間フィルタをセットする
           SetDiffFilter();

       }

       //*******************************************************************************
       //機　　能： フォームアンロード時の処理
       //
       //           変数名          [I/O] 型        内容
       //引　　数： なし
       //戻 り 値： なし
       //
       //補　　足： なし
       //
       //履　　歴： V1.00  99/XX/XX   ????????      新規作成
       //*******************************************************************************
       private void frmFDiff_FormClosed(object sender, FormClosedEventArgs e)
       {

           //微分フィルタ番号の保存
           CTSettings.scanParam.DiffFilterNo = hsbDiffFilter.Value;

           //イベント生成（アンロードされた）
           if (Unloaded != null)
           {
               Unloaded();
           }

       }

//********************************************************************************
//機    能  ：  微分フィルタ用パラメータのセット
//              変数名           [I/O] 型        内容
//引    数  ：  なし
//戻 り 値  ：  なし
//補    足  ：  なし
//
//履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
//********************************************************************************
       private void SetDiffFilter()
       {

           //2, 2,-2
           //2, 1,-2
           //2,-2,-2
           DFilter1[0] = 2;
           DFilter1[1] = 2;
           DFilter1[2] = -2;
           DFilter1[3] = 2;
           DFilter1[4] = 1;
           DFilter1[5] = -2;
           DFilter1[6] = 2;
           DFilter1[7] = -2;
           DFilter1[8] = -2;

           //2, 0, 2, 0,-2
           //0, 0, 0, 0, 0
           //2, 0, 1, 0,-2
           //0, 0, 0, 0, 0
           //2, 0,-2, 0,-2
           DFilter2[0] = 2;
           DFilter2[1] = 0;
           DFilter2[2] = 2;
           DFilter2[3] = 0;
           DFilter2[4] = -2;
           DFilter2[5] = 0;
           DFilter2[6] = 0;
           DFilter2[7] = 0;
           DFilter2[8] = 0;
           DFilter2[9] = 0;
           DFilter2[10] = 2;
           DFilter2[11] = 0;
           DFilter2[12] = 1;
           DFilter2[13] = 0;
           DFilter2[14] = -2;
           DFilter2[15] = 0;
           DFilter2[16] = 0;
           DFilter2[17] = 0;
           DFilter2[18] = 0;
           DFilter2[19] = 0;
           DFilter2[20] = 2;
           DFilter2[21] = 0;
           DFilter2[22] = -2;
           DFilter2[23] = 0;
           DFilter2[24] = -2;

           //2, 0, 2, 0,-2
           //0, 2, 0,-2, 0
           //2, 0, 1, 0,-2
           //0, 2, 0,-2, 0
           //2, 0,-2, 0,-2
           DFilter3[0] = 2;
           DFilter3[1] = 0;
           DFilter3[2] = 2;
           DFilter3[3] = 0;
           DFilter3[4] = -2;
           DFilter3[5] = 0;
           DFilter3[6] = 2;
           DFilter3[7] = 0;
           DFilter3[8] = -2;
           DFilter3[9] = 0;
           DFilter3[10] = 2;
           DFilter3[11] = 0;
           DFilter3[12] = 1;
           DFilter3[13] = 0;
           DFilter3[14] = -2;
           DFilter3[15] = 0;
           DFilter3[16] = 2;
           DFilter3[17] = 0;
           DFilter3[18] = -2;
           DFilter3[19] = 0;
           DFilter3[20] = 2;
           DFilter3[21] = 0;
           DFilter3[22] = -2;
           DFilter3[23] = 0;
           DFilter3[24] = -2;

           //2, 0, 0, 2, 0, 0,-2
           //0, 0, 0, 0, 0, 0, 0
           //0, 0, 0, 0, 0, 0, 0
           //2, 0, 0, 1, 0, 0,-2
           //0, 0, 0, 0, 0, 0, 0
           //0, 0, 0, 0, 0, 0, 0
           //2, 0, 0,-2, 0, 0,-2
           DFilter4[0] = 2;
           DFilter4[1] = 0;
           DFilter4[2] = 0;
           DFilter4[3] = 2;
           DFilter4[4] = 0;
           DFilter4[5] = 0;
           DFilter4[6] = -2;
           DFilter4[7] = 0;
           DFilter4[8] = 0;
           DFilter4[9] = 0;
           DFilter4[10] = 0;
           DFilter4[11] = 0;
           DFilter4[12] = 0;
           DFilter4[13] = 0;
           DFilter4[14] = 0;
           DFilter4[15] = 0;
           DFilter4[16] = 0;
           DFilter4[17] = 0;
           DFilter4[18] = 0;
           DFilter4[19] = 0;
           DFilter4[20] = 0;
           DFilter4[21] = 2;
           DFilter4[22] = 0;
           DFilter4[23] = 0;
           DFilter4[24] = 1;
           DFilter4[25] = 0;
           DFilter4[26] = 0;
           DFilter4[27] = -2;
           DFilter4[28] = 0;
           DFilter4[29] = 0;
           DFilter4[30] = 0;
           DFilter4[31] = 0;
           DFilter4[32] = 0;
           DFilter4[33] = 0;
           DFilter4[34] = 0;
           DFilter4[35] = 0;
           DFilter4[36] = 0;
           DFilter4[37] = 0;
           DFilter4[38] = 0;
           DFilter4[39] = 0;
           DFilter4[40] = 0;
           DFilter4[41] = 0;
           DFilter4[42] = 2;
           DFilter4[43] = 0;
           DFilter4[44] = 0;
           DFilter4[45] = -2;
           DFilter4[46] = 0;
           DFilter4[47] = 0;
           DFilter4[48] = -2;

           //2, 0, 2, 0, 2, 0,-2
           //0, 0, 0, 0, 0, 0, 0
           //2, 0, 2, 0,-2, 0,-2
           //0, 0, 0, 1, 0, 0, 0
           //2, 0, 2, 0,-2, 0,-2
           //0, 0, 0, 0, 0, 0, 0
           //2, 0,-2, 0,-2, 0,-2
           DFilter5[0] = 2;
           DFilter5[1] = 0;
           DFilter5[2] = 2;
           DFilter5[3] = 0;
           DFilter5[4] = 2;
           DFilter5[5] = 0;
           DFilter5[6] = -2;
           DFilter5[7] = 0;
           DFilter5[8] = 0;
           DFilter5[9] = 0;
           DFilter5[10] = 0;
           DFilter5[11] = 0;
           DFilter5[12] = 0;
           DFilter5[13] = 0;
           DFilter5[14] = 2;
           DFilter5[15] = 0;
           DFilter5[16] = 2;
           DFilter5[17] = 0;
           DFilter5[18] = -2;
           DFilter5[19] = 0;
           DFilter5[20] = -2;
           DFilter5[21] = 0;
           DFilter5[22] = 0;
           DFilter5[23] = 0;
           DFilter5[24] = 1;
           DFilter5[25] = 0;
           DFilter5[26] = 0;
           DFilter5[27] = 0;
           DFilter5[28] = 2;
           DFilter5[29] = 0;
           DFilter5[30] = 2;
           DFilter5[31] = 0;
           DFilter5[32] = -2;
           DFilter5[33] = 0;
           DFilter5[34] = -2;
           DFilter5[35] = 0;
           DFilter5[36] = 0;
           DFilter5[37] = 0;
           DFilter5[38] = 0;
           DFilter5[39] = 0;
           DFilter5[40] = 0;
           DFilter5[41] = 0;
           DFilter5[42] = 2;
           DFilter5[43] = 0;
           DFilter5[44] = -2;
           DFilter5[45] = 0;
           DFilter5[46] = -2;
           DFilter5[47] = 0;
           DFilter5[48] = -2;

        }
    }
}
