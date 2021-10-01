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
    public partial class frmFEdge : Form
    {
        ///* ************************************************************************** */
        ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
        ///* 客先　　　　： ?????? 殿                                                   */
        ///* プログラム名： frmFEdge.frm                                                */
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
        private static frmFEdge myForm = null;
        #endregion

        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************
        float[] EFilter1;
        float[] EFilter2;
        float[] EFilter3;
        float[] EFilter4;
        float[] EFilter5;
        float[] EFilter6;
        float[] EFilter7;
        float[] EFilter8;

        #region サポートしているイベント
        /// <summary>
        /// サポートしているイベント
        /// </summary>
        public event UnloadedEventHandler Unloaded;
        public delegate void UnloadedEventHandler();        //アンロードされた
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmFEdge()
        {
            InitializeComponent();
        }
        #endregion

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmFEdge Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmFEdge();
                }

                return myForm;
            }
        }
        #endregion    

        //*******************************************************************************
        //機　　能： エッジフィルタ番号スクロールバー・値変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void hsbEdgeFilter_Change(int newScrollValue)
        {

            //値の表示
            lblEdgeFilter.Text = Convert.ToString(newScrollValue);

        }

        //hsbDiffFilterのスクロールイベント
        private void hsbEdgeFilter_Scroll(System.Object eventSender, System.Windows.Forms.ScrollEventArgs eventArgs)
        {
            switch (eventArgs.Type)
            {
                case System.Windows.Forms.ScrollEventType.EndScroll:
                    //スクロールを止めた位置で実行
                    hsbEdgeFilter_Change(eventArgs.NewValue);
                    break;
            }
        }


        private void cmdExe_Click(object sender, EventArgs e)
        {
            //マウスを砂時計にする
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //透視画像フォームに対して
            var _with1 = frmTransImage.Instance;

            //空間フィルタをかける
            switch (hsbEdgeFilter.Value)
            {
                case 1:
                    _with1.IP_Filter(ref EFilter1);
                    break;
                case 2:
                    _with1.IP_Filter(ref EFilter2);
                    break;
                case 3:
                    _with1.IP_Filter(ref EFilter3);
                    break;
                case 4:
                    _with1.IP_Filter(ref EFilter4);
                    break;
                case 5:
                    _with1.IP_Filter(ref EFilter5);
                    break;
                case 6:
                    _with1.IP_Filter(ref EFilter6);
                    break;
                case 7:
                    _with1.IP_Filter(ref EFilter7);
                    break;
                case 8:
                    _with1.IP_Filter(ref EFilter8);
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
        private void frmFEdge_Load(object sender, EventArgs e)
        {

            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            //エッジフィルタ番号の設定
            hsbEdgeFilter.Value = CTSettings.scanParam.EdgeFilterNo;

            EFilter1 = new float[5 * 5];
            EFilter2 = new float[5 * 5];
            EFilter3 = new float[5 * 5];
            EFilter4 = new float[7 * 7];
            EFilter5 = new float[7 * 7];
            EFilter6 = new float[7 * 7];
            EFilter7 = new float[9 * 9];
            EFilter8 = new float[9 * 9];

            //空間フィルタをセットする
            SetEdgeFilter();

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
        private void frmFEdge_FormClosed(object sender, FormClosedEventArgs e)
        {
            //エッジフィルタ番号の保存
            CTSettings.scanParam.EdgeFilterNo = hsbEdgeFilter.Value;

            //イベント生成（アンロードされた）
            if (Unloaded != null)
            {
                Unloaded();
            }

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
        private void SetEdgeFilter()
        {

            // 0, 0,-5, 0, 0
            // 0,-4, 0,-4, 0
            //-5, 0,40, 0,-5
            // 0,-4, 0,-4, 0
            // 0, 0,-5, 0, 0
            EFilter1[0] = 0;
            EFilter1[1] = 0;
            EFilter1[2] = -5;
            EFilter1[3] = 0;
            EFilter1[4] = 0;
            EFilter1[5] = 0;
            EFilter1[6] = -4;
            EFilter1[7] = 0;
            EFilter1[8] = -4;
            EFilter1[9] = 0;
            EFilter1[10] = -5;
            EFilter1[11] = 0;
            EFilter1[12] = 40;
            EFilter1[13] = 0;
            EFilter1[14] = -5;
            EFilter1[15] = 0;
            EFilter1[16] = -4;
            EFilter1[17] = 0;
            EFilter1[18] = -4;
            EFilter1[19] = 0;
            EFilter1[20] = 0;
            EFilter1[21] = 0;
            EFilter1[22] = -5;
            EFilter1[23] = 0;
            EFilter1[24] = 0;

            // 0,-2,-2,-2, 0
            //-2,-6, 0,-6,-2
            //-2, 0,60, 0,-2
            //-2,-6, 0,-6,-2
            // 0,-2,-2,-2, 0
            EFilter2[0] = 0;
            EFilter2[1] = -2;
            EFilter2[2] = -2;
            EFilter2[3] = -2;
            EFilter2[4] = 0;
            EFilter2[5] = -2;
            EFilter2[6] = -6;
            EFilter2[7] = 0;
            EFilter2[8] = -6;
            EFilter2[9] = -2;
            EFilter2[10] = -2;
            EFilter2[11] = 0;
            EFilter2[12] = 60;
            EFilter2[13] = 0;
            EFilter2[14] = -2;
            EFilter2[15] = -2;
            EFilter2[16] = -6;
            EFilter2[17] = 0;
            EFilter2[18] = -6;
            EFilter2[19] = -2;
            EFilter2[20] = 0;
            EFilter2[21] = -2;
            EFilter2[22] = -2;
            EFilter2[23] = -2;
            EFilter2[24] = 0;

            // 0,-2,-4,-2, 0
            //-2,-8, 0,-8,-2
            //-4, 0,80, 0,-4
            //-2,-8, 0,-8,-2
            // 0,-2,-4,-2, 0
            EFilter3[0] = 0;
            EFilter3[1] = -2;
            EFilter3[2] = -4;
            EFilter3[3] = -2;
            EFilter3[4] = 0;
            EFilter3[5] = -2;
            EFilter3[6] = -8;
            EFilter3[7] = 0;
            EFilter3[8] = -8;
            EFilter3[9] = -2;
            EFilter3[10] = -4;
            EFilter3[11] = 0;
            EFilter3[12] = 80;
            EFilter3[13] = 0;
            EFilter3[14] = -4;
            EFilter3[15] = -2;
            EFilter3[16] = -8;
            EFilter3[17] = 0;
            EFilter3[18] = -8;
            EFilter3[19] = -2;
            EFilter3[20] = 0;
            EFilter3[21] = -2;
            EFilter3[22] = -4;
            EFilter3[23] = -2;
            EFilter3[24] = 0;

            // 0, 0, 0,-2, 0, 0, 0
            // 0,-2,-2,-2,-2,-2, 0
            // 0,-2, 0, 8, 0,-2, 0
            //-2,-2, 8,20, 8,-2,-2
            // 0,-2, 0, 8, 0,-2, 0
            // 0,-2,-2,-2,-2,-2, 0
            // 0, 0, 0,-2, 0, 0, 0
            EFilter4[0] = 0;
            EFilter4[1] = 0;
            EFilter4[2] = 0;
            EFilter4[3] = -2;
            EFilter4[4] = 0;
            EFilter4[5] = 0;
            EFilter4[6] = 0;
            EFilter4[7] = 0;
            EFilter4[8] = -2;
            EFilter4[9] = -2;
            EFilter4[10] = -2;
            EFilter4[11] = -2;
            EFilter4[12] = -2;
            EFilter4[13] = 0;
            EFilter4[14] = 0;
            EFilter4[15] = -2;
            EFilter4[16] = 0;
            EFilter4[17] = 8;
            EFilter4[18] = 0;
            EFilter4[19] = -2;
            EFilter4[20] = 0;
            EFilter4[21] = -2;
            EFilter4[22] = -2;
            EFilter4[23] = 8;
            EFilter4[24] = 20;
            EFilter4[25] = 8;
            EFilter4[26] = -2;
            EFilter4[27] = -2;
            EFilter4[28] = 0;
            EFilter4[29] = -2;
            EFilter4[30] = 0;
            EFilter4[31] = 8;
            EFilter4[32] = 0;
            EFilter4[33] = -2;
            EFilter4[34] = 0;
            EFilter4[35] = 0;
            EFilter4[36] = -2;
            EFilter4[37] = -2;
            EFilter4[38] = -2;
            EFilter4[39] = -2;
            EFilter4[40] = -2;
            EFilter4[41] = 0;
            EFilter4[42] = 0;
            EFilter4[43] = 0;
            EFilter4[44] = 0;
            EFilter4[45] = -2;
            EFilter4[46] = 0;
            EFilter4[47] = 0;
            EFilter4[48] = 0;

            // 0, 0,-2,-2,-2, 0, 0
            // 0,-2,-4,-4,-4,-2, 0
            //-2,-4, 2,10, 2,-4,-2
            //-2,-4,10,50,10,-4,-2
            //-2,-4, 2,10, 2,-4,-2
            // 0,-2,-4,-4,-4,-2, 0
            // 0, 0,-2,-2,-2, 0, 0
            EFilter5[0] = 0;
            EFilter5[1] = 0;
            EFilter5[2] = -2;
            EFilter5[3] = -2;
            EFilter5[4] = -2;
            EFilter5[5] = 0;
            EFilter5[6] = 0;
            EFilter5[7] = 0;
            EFilter5[8] = -2;
            EFilter5[9] = -4;
            EFilter5[10] = -4;
            EFilter5[11] = -4;
            EFilter5[12] = -2;
            EFilter5[13] = 0;
            EFilter5[14] = -2;
            EFilter5[15] = -4;
            EFilter5[16] = 2;
            EFilter5[17] = 10;
            EFilter5[18] = 2;
            EFilter5[19] = -4;
            EFilter5[20] = -2;
            EFilter5[21] = -2;
            EFilter5[22] = -4;
            EFilter5[23] = 10;
            EFilter5[24] = 50;
            EFilter5[25] = 10;
            EFilter5[26] = -4;
            EFilter5[27] = -2;
            EFilter5[28] = -2;
            EFilter5[29] = -4;
            EFilter5[30] = 2;
            EFilter5[31] = 10;
            EFilter5[32] = 2;
            EFilter5[33] = -4;
            EFilter5[34] = -2;
            EFilter5[35] = 0;
            EFilter5[36] = -2;
            EFilter5[37] = -4;
            EFilter5[38] = -4;
            EFilter5[39] = -4;
            EFilter5[40] = -2;
            EFilter5[41] = 0;
            EFilter5[42] = 0;
            EFilter5[43] = 0;
            EFilter5[44] = -2;
            EFilter5[45] = -2;
            EFilter5[46] = -2;
            EFilter5[47] = 0;
            EFilter5[48] = 0;

            // 0, 0,-2,-2,-2, 0, 0
            // 0,-2,-4,-4,-4,-2, 0
            //-2,-4, 2,10, 2,-4,-2
            //-2,-4,10,50,10,-4,-2
            //-2,-4, 2,10, 2,-4,-2
            // 0,-2,-4,-4,-4,-2, 0
            // 0, 0,-2,-2,-2, 0, 0
            EFilter6[0] = 0;
            EFilter6[1] = 0;
            EFilter6[2] = -2;
            EFilter6[3] = -2;
            EFilter6[4] = -2;
            EFilter6[5] = 0;
            EFilter6[6] = 0;
            EFilter6[7] = 0;
            EFilter6[8] = -2;
            EFilter6[9] = -6;
            EFilter6[10] = -6;
            EFilter6[11] = -6;
            EFilter6[12] = -2;
            EFilter6[13] = 0;
            EFilter6[14] = -2;
            EFilter6[15] = -6;
            EFilter6[16] = 2;
            EFilter6[17] = 14;
            EFilter6[18] = 2;
            EFilter6[19] = -6;
            EFilter6[20] = -2;
            EFilter6[21] = -2;
            EFilter6[22] = -6;
            EFilter6[23] = 14;
            EFilter6[24] = 60;
            EFilter6[25] = 14;
            EFilter6[26] = -6;
            EFilter6[27] = -2;
            EFilter6[28] = -2;
            EFilter6[29] = -6;
            EFilter6[30] = 2;
            EFilter6[31] = 14;
            EFilter6[32] = 2;
            EFilter6[33] = -6;
            EFilter6[34] = -2;
            EFilter6[35] = 0;
            EFilter6[36] = -2;
            EFilter6[37] = -6;
            EFilter6[38] = -6;
            EFilter6[39] = -6;
            EFilter6[40] = -2;
            EFilter6[41] = 0;
            EFilter6[42] = 0;
            EFilter6[43] = 0;
            EFilter6[44] = -2;
            EFilter6[45] = -2;
            EFilter6[46] = -2;
            EFilter6[47] = 0;
            EFilter6[48] = 0;

            // 0, 0, 0,-1,-1,-1, 0, 0, 0
            // 0,-1,-1,-2,-2,-2,-1,-1, 0
            // 0,-1,-2,-1, 0,-1,-2,-1, 0
            //-1,-2,-1, 6, 9, 6,-1,-2,-1
            //-1,-2, 0, 9,20, 9, 0,-2,-1
            //-1,-2,-1, 6, 9, 6,-1,-2,-1
            // 0,-1,-2,-1, 0,-1,-2,-1, 0
            // 0,-1,-1,-2,-2,-2,-1,-1, 0
            // 0, 0, 0,-1,-1,-1, 0, 0, 0
            EFilter7[0] = 0;
            EFilter7[1] = 0;
            EFilter7[2] = 0;
            EFilter7[3] = -1;
            EFilter7[4] = -1;
            EFilter7[5] = -1;
            EFilter7[6] = 0;
            EFilter7[7] = 0;
            EFilter7[8] = 0;
            EFilter7[9] = 0;
            EFilter7[10] = -1;
            EFilter7[11] = -1;
            EFilter7[12] = -2;
            EFilter7[13] = -2;
            EFilter7[14] = -2;
            EFilter7[15] = -1;
            EFilter7[16] = -1;
            EFilter7[17] = 0;
            EFilter7[18] = 0;
            EFilter7[19] = -1;
            EFilter7[20] = -2;
            EFilter7[21] = -1;
            EFilter7[22] = 0;
            EFilter7[23] = -1;
            EFilter7[24] = -2;
            EFilter7[25] = -1;
            EFilter7[26] = 0;
            EFilter7[27] = -1;
            EFilter7[28] = -2;
            EFilter7[29] = -1;
            EFilter7[30] = 6;
            EFilter7[31] = 9;
            EFilter7[32] = 6;
            EFilter7[33] = -1;
            EFilter7[34] = -2;
            EFilter7[35] = -1;
            EFilter7[36] = -1;
            EFilter7[37] = -2;
            EFilter7[38] = 0;
            EFilter7[39] = 9;
            EFilter7[40] = 20;
            EFilter7[41] = 9;
            EFilter7[42] = 0;
            EFilter7[43] = -2;
            EFilter7[44] = -1;
            EFilter7[45] = -1;
            EFilter7[46] = -2;
            EFilter7[47] = -1;
            EFilter7[48] = 6;
            EFilter7[49] = 9;
            EFilter7[50] = 6;
            EFilter7[51] = -1;
            EFilter7[52] = -2;
            EFilter7[53] = -1;
            EFilter7[54] = 0;
            EFilter7[55] = -1;
            EFilter7[56] = -2;
            EFilter7[57] = -1;
            EFilter7[58] = 0;
            EFilter7[59] = -1;
            EFilter7[60] = -2;
            EFilter7[61] = -1;
            EFilter7[62] = 0;
            EFilter7[63] = 0;
            EFilter7[64] = -1;
            EFilter7[65] = -1;
            EFilter7[66] = -2;
            EFilter7[67] = -2;
            EFilter7[68] = -2;
            EFilter7[69] = -1;
            EFilter7[70] = -1;
            EFilter7[71] = 0;
            EFilter7[72] = 0;
            EFilter7[73] = 0;
            EFilter7[74] = 0;
            EFilter7[75] = -1;
            EFilter7[76] = -1;
            EFilter7[77] = -1;
            EFilter7[78] = 0;
            EFilter7[79] = 0;
            EFilter7[80] = 0;

            // 0, 0, 0, 0,-2, 0, 0, 0, 0
            // 0, 0,-2,-2,-2,-2,-2, 0, 0
            // 0,-2,-2,-2, 0,-2,-2,-2, 0
            // 0,-2,-2, 6,12, 6,-2,-2, 0
            //-2,-2, 0,12,30,12, 0,-2,-2
            // 0,-2,-2, 6,12, 6,-2,-2, 0
            // 0,-2,-2,-2, 0,-2,-2,-2, 0
            // 0, 0,-2,-2,-2,-2,-2, 0, 0
            // 0, 0, 0, 0,-2, 0, 0, 0, 0
            EFilter8[0] = 0;
            EFilter8[1] = 0;
            EFilter8[2] = 0;
            EFilter8[3] = 0;
            EFilter8[4] = -2;
            EFilter8[5] = 0;
            EFilter8[6] = 0;
            EFilter8[7] = 0;
            EFilter8[8] = 0;
            EFilter8[9] = 0;
            EFilter8[10] = 0;
            EFilter8[11] = -2;
            EFilter8[12] = -2;
            EFilter8[13] = -2;
            EFilter8[14] = -2;
            EFilter8[15] = -2;
            EFilter8[16] = 0;
            EFilter8[17] = 0;
            EFilter8[18] = 0;
            EFilter8[19] = -2;
            EFilter8[20] = -2;
            EFilter8[21] = -2;
            EFilter8[22] = 0;
            EFilter8[23] = -2;
            EFilter8[24] = -2;
            EFilter8[25] = -2;
            EFilter8[26] = 0;
            EFilter8[27] = 0;
            EFilter8[28] = -2;
            EFilter8[29] = -2;
            EFilter8[30] = 6;
            EFilter8[31] = 12;
            EFilter8[32] = 6;
            EFilter8[33] = -2;
            EFilter8[34] = -2;
            EFilter8[35] = 0;
            EFilter8[36] = -2;
            EFilter8[37] = -2;
            EFilter8[38] = 0;
            EFilter8[39] = 12;
            EFilter8[40] = 30;
            EFilter8[41] = 12;
            EFilter8[42] = 0;
            EFilter8[43] = -2;
            EFilter8[44] = -2;
            EFilter8[45] = 0;
            EFilter8[46] = -2;
            EFilter8[47] = -2;
            EFilter8[48] = 6;
            EFilter8[49] = 12;
            EFilter8[50] = 6;
            EFilter8[51] = -2;
            EFilter8[52] = -2;
            EFilter8[53] = 0;
            EFilter8[54] = 0;
            EFilter8[55] = -2;
            EFilter8[56] = -2;
            EFilter8[57] = -2;
            EFilter8[58] = 0;
            EFilter8[59] = -2;
            EFilter8[60] = -2;
            EFilter8[61] = -2;
            EFilter8[62] = 0;
            EFilter8[63] = 0;
            EFilter8[64] = 0;
            EFilter8[65] = -2;
            EFilter8[66] = -2;
            EFilter8[67] = -2;
            EFilter8[68] = -2;
            EFilter8[69] = -2;
            EFilter8[70] = 0;
            EFilter8[71] = 0;
            EFilter8[72] = 0;
            EFilter8[73] = 0;
            EFilter8[74] = 0;
            EFilter8[75] = 0;
            EFilter8[76] = -2;
            EFilter8[77] = 0;
            EFilter8[78] = 0;
            EFilter8[79] = 0;
            EFilter8[80] = 0;

            //'空間フィルタの係数の合計を求める
            //Sum_F = 0
            //For i = 0 To 5 * 5
            //    Sum_F = Sum_F + EFilter1(i)
            //Next
            //
            //'空間フィルタを足して１になるようにする
            //For i = 0 To 5 * 5
            //    EFilter1(i) = EFilter1(i) / Sum_F
            //Next
            //
            //'空間フィルタの係数の合計を求める
            //Sum_F = 0
            //For i = 0 To 5 * 5
            //    Sum_F = Sum_F + EFilter2(i)
            //Next
            //
            //'空間フィルタを足して１になるようにする
            //For i = 0 To 5 * 5
            //    EFilter2(i) = EFilter2(i) / Sum_F
            //Next
            //
            //'空間フィルタの係数の合計を求める
            //Sum_F = 0
            //For i = 0 To 5 * 5
            //    Sum_F = Sum_F + EFilter3(i)
            //Next
            //
            //'空間フィルタを足して１になるようにする
            //For i = 0 To 5 * 5
            //    EFilter3(i) = EFilter3(i) / Sum_F
            //Next
            //
            //'空間フィルタの係数の合計を求める
            //Sum_F = 0
            //For i = 0 To 7 * 7
            //    Sum_F = Sum_F + EFilter4(i)
            //Next
            //
            //'空間フィルタを足して１になるようにする
            //For i = 0 To 7 * 7
            //    EFilter4(i) = EFilter4(i) / Sum_F
            //Next
            //
            //'空間フィルタの係数の合計を求める
            //Sum_F = 0
            //For i = 0 To 7 * 7
            //    Sum_F = Sum_F + EFilter5(i)
            //Next
            //
            //'空間フィルタを足して１になるようにする
            //For i = 0 To 7 * 7
            //    EFilter5(i) = EFilter5(i) / Sum_F
            //Next
            //
            //'空間フィルタの係数の合計を求める
            //Sum_F = 0
            //For i = 0 To 7 * 7
            //    Sum_F = Sum_F + EFilter6(i)
            //Next
            //
            //'空間フィルタを足して１になるようにする
            //For i = 0 To 7 * 7
            //    EFilter6(i) = EFilter6(i) / Sum_F
            //Next
            //
            //'空間フィルタの係数の合計を求める
            //Sum_F = 0
            //For i = 0 To 9 * 9
            //    Sum_F = Sum_F + EFilter7(i)
            //Next
            //
            //'空間フィルタを足して１になるようにする
            //For i = 0 To 9 * 9
            //    EFilter7(i) = EFilter7(i) / Sum_F
            //Next
            //
            //'空間フィルタの係数の合計を求める
            //Sum_F = 0
            //For i = 0 To 9 * 9
            //    Sum_F = Sum_F + EFilter8(i)
            //Next
            //
            //'空間フィルタを足して１になるようにする
            //For i = 0 To 9 * 9
            //    EFilter8(i) = EFilter8(i) / Sum_F
            //Next

            //空間フィルタの再計算（足して１になるようにする）上記と同じ処理
            ReCalc(ref EFilter1);
            ReCalc(ref EFilter2);
            ReCalc(ref EFilter3);
            ReCalc(ref EFilter4);
            ReCalc(ref EFilter5);
            ReCalc(ref EFilter6);
            ReCalc(ref EFilter7);
            ReCalc(ref EFilter8);

        }

        //
        //   空間フィルタの再計算（足して１になるようにする）
        //
        private void ReCalc(ref float[] theFilter)
        {

            float Sum = 0;
            int i = 0;

            //空間フィルタの係数の合計を求める
            Sum = 0;


            for (i = theFilter.GetLowerBound(0); i <= theFilter.GetUpperBound(0); i++)
            {
                Sum = Sum + theFilter[i];
            }

            if (Sum == 0)
                return;

            //空間フィルタを足して１になるようにする
            for (i = theFilter.GetLowerBound(0); i <= theFilter.GetUpperBound(0); i++)
            {
                theFilter[i] = theFilter[i] / Sum;
            }

        }
    }
}
