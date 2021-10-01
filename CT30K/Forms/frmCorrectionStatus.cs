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
    ///* プログラム名： frmCorrectionStatus.frm                                     */
    ///* 処理概要　　： 校正ステータス表示画面                                      */
    ///* 注意事項　　： なし                                                        */
    ///* -------------------------------------------------------------------------- */
    ///* 適用計算機　： DOS/V PC                                                    */
    ///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
    ///* コンパイラ　： VB 6.0                                                      */
    ///* -------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
    ///*                                                                            */
    ///* V4.0        01/01/22    (ITC)    鈴山　修   新規作成                       */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
    public partial class frmCorrectionStatus : Form
    {
        //画面表示値
        string strSeparator;                //区切り文字
        string[] strMove = new string[2];   //0:移動なし, 1:移動あり

        //bool fLoad = false;                 //InitFormを呼び出しflg

        #region コントロール

        // グループ1 - #ゲイン校正（実行時／現在値）
        //”焦点”追加　'v19.50 v19.41とv18.02の統合 by長野 2013/11/05   //追加2014/10/07hata_v19.51反映
        //private Label[] lblItemGain = new Label[10];
        //private Label[] lblColonGain = new Label[10];
        //private Label[] lblStatusGain = new Label[10];
        public Label[] lblItemGain = new Label[11];
        private Label[] lblColonGain = new Label[11];
        private Label[] lblStatusGain = new Label[11];

        // グループ2 - #回転中心校正（実行時／現在値）
        //”焦点”追加　'v19.50 v19.41とv18.02の統合 by長野 2013/11/05   //追加2014/10/07hata_v19.51反映
        //public Label[] lblItemRot = new Label[10];
        //private Label[] lblColonRot = new Label[10];
        //private Label[] lblStatusRot = new Label[10];
        public Label[] lblItemRot = new Label[11];
        private Label[] lblColonRot = new Label[11];
        private Label[] lblStatusRot = new Label[11];

        // グループ3 - #ｽｷｬﾝ位置校正（実行時／現在値）
        public Label[] lblItemSp = new Label[5];
        private Label[] lblColonSp = new Label[5];
        private Label[] lblStatusSp = new Label[5];

        // グループ4 - #オフセット校正（実行時／現在値）
        private Label[] lblItemOff = new Label[4];
        private Label[] lblColonOff = new Label[4];
        private Label[] lblStatusOff = new Label[4];

        // グループ5 - #幾何歪校正（実行時／現在値）
        private Label[] lblItemVer = new Label[4];
        private Label[] lblColonVer = new Label[4];
        private Label[] lblStatusVer = new Label[4];

        // グループ6 - #寸法校正（実行時／現在値）
        public Label[] lblItemDist = new Label[9];
        private Label[] lblColonDist = new Label[9];
        private Label[] lblStatusDist = new Label[9];

        // グループ7 - #シフトスキャン用（実行時／現在値）   //シフトスキャン追加　'v19.50 v19.41とv18.02の統合 by長野 2013/11/05   //追加2014/10/07hata_v19.51反映
        public Label[] lblItemGainShift = new Label[11];
        private Label[] lblColonGainShift = new Label[11];
        private Label[] lblStatusGainShift = new Label[11];

        // ボタン
        private Button[] cmdCorrect = new Button[8];

        #endregion

        #region インスタンスを返すプロパティ

        // frmCorrectionStatusのインスタンス
        private static frmCorrectionStatus _Instance = null;

        /// <summary>
        /// frmCorrectionStatusのインスタンスを返す
        /// </summary>
        public static frmCorrectionStatus Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmCorrectionStatus();
                }

                return _Instance;
            }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmCorrectionStatus()
        {
            InitializeComponent();

            this.SuspendLayout();

            #region グループ1 - #ゲイン校正（実行時／現在値）

            for (int i = 0; i < lblItemGain.Length; i++)
            {
                #region lblItemGain

                this.lblItemGain[i] = new Label();
                this.lblItemGain[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblItemGain[i].BackColor = Color.Cyan;
                this.lblItemGain[i].Location = new Point(8, 24 + i * 16);
                this.lblItemGain[i].Name = "lblItemGain" + i.ToString();
                this.lblItemGain[i].TextAlign = ContentAlignment.MiddleLeft;
                this.lblItemGain[i].Size = new Size(100, 15);
                this.lblItemGain[i].TabIndex = i;
                switch (i)
                {
                    case 0:
                        this.lblItemGain[i].Tag = "12810";
                        this.lblItemGain[i].Text = "#I.I.視野";
                        break;
                    case 1:
                        this.lblItemGain[i].Text = "管電圧(kV)";
                        break;
                    case 2:
                        this.lblItemGain[i].Tag = "12094";
                        this.lblItemGain[i].Text = "#年月日";
                        break;
                    case 3:
                        this.lblItemGain[i].Tag = "12218";
                        this.lblItemGain[i].Text = "#X線管";
                        break;
                    case 4:
                        this.lblItemGain[i].Tag = "12163";
                        this.lblItemGain[i].Text = "#ﾌｨﾙﾀ";
                        break;
                    case 5:
                        this.lblItemGain[i].Tag = "20122";
                        this.lblItemGain[i].Text = "#I.I.移動";
                        break;
                    case 6:
                        this.lblItemGain[i].Tag = "12737";
                        this.lblItemGain[i].Text = "#ビニング";
                        break;
                    case 7:
                        this.lblItemGain[i].Tag = "20015";
                        this.lblItemGain[i].Text = "#FPDゲイン";
                        break;
                    case 8:
                        this.lblItemGain[i].Tag = "20016";
                        this.lblItemGain[i].Text = "#FPD積分時間";
                        break;
                    case 9:
                        this.lblItemGain[i].Tag = "20121";
                        this.lblItemGain[i].Text = "#ｵﾌｾｯﾄ実行時刻";
                        break;

                    //”焦点”追加　'v19.50 v19.41とv18.02の統合 by長野 2013/11/05   //追加2014/10/07hata_v19.51反映
                    case 10:
                        this.lblItemGain[i].Tag = "12164";
                        this.lblItemGain[i].Text = "焦点";
                        break;
                    default:
                        break;
                }

                #endregion

                #region lblColonGain

                this.lblColonGain[i] = new Label();
                this.lblColonGain[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblColonGain[i].AutoSize = true;
                this.lblColonGain[i].Name = "lblColonGain" + i.ToString();
                this.lblColonGain[i].Size = new Size(7, 12);
                this.lblColonGain[i].Location = new Point(111, 27 + i * 16);
                this.lblColonGain[i].TabIndex = i;
                this.lblColonGain[i].Text = "：";
                this.lblColonGain[i].TextAlign = ContentAlignment.MiddleLeft;

                #endregion

                #region lblStatusGain

                this.lblStatusGain[i] = new System.Windows.Forms.Label();
                this.lblStatusGain[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblStatusGain[i].AutoSize = true;
                this.lblStatusGain[i].Location = new Point(120, 25 + i * 16);
                this.lblStatusGain[i].Name = "lblStatusGain" + i.ToString(); ;
                this.lblStatusGain[i].Size = new Size(93, 12);
                this.lblStatusGain[i].TabIndex = 20;
                this.lblStatusGain[i].Text = "#lblStatusGain(" + i.ToString() + ")";
                this.lblStatusGain[i].TextAlign = ContentAlignment.MiddleLeft;
                this.lblStatusGain[i].TextChanged += new EventHandler(lblStatusGain_Change);

                #endregion

                this.fraGain.Controls.Add(this.lblItemGain[i]);
                this.fraGain.Controls.Add(this.lblColonGain[i]);
                this.fraGain.Controls.Add(this.lblStatusGain[i]);
            }

            #endregion

            #region グループ2 - #回転中心校正（実行時／現在値）

            for (int i = 0; i < lblItemRot.Length; i++)
            {
                #region lblItemRot

                this.lblItemRot[i] = new Label();
                this.lblItemRot[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblItemRot[i].BackColor = Color.Cyan;
                this.lblItemRot[i].Location = new Point(8, 24 + i * 16);
                this.lblItemRot[i].Name = "lblItemRot" + i.ToString();
                this.lblItemRot[i].Size = new Size(100, 15);
                this.lblItemRot[i].TabIndex = i;
                this.lblItemRot[i].TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                switch (i)
                {
                    case 0:
                        this.lblItemRot[i].Text = "管電圧(kV)";
                        break;
                    case 1:
                        this.lblItemRot[i].Text = "昇降位置(mm)";
                        break;
                    case 2:
                        this.lblItemRot[i].Tag = "12810";
                        this.lblItemRot[i].Text = "#I.I.視野";
                        break;
                    case 3:
                        this.lblItemRot[i].Tag = "12218";
                        this.lblItemRot[i].Text = "#X線管";
                        break;
                    case 4:
                        this.lblItemRot[i].Tag = "20125";
                        this.lblItemRot[i].Text = "#Ｘ軸移動";
                        break;
                    case 5:
                        this.lblItemRot[i].Tag = "20126";
                        this.lblItemRot[i].Text = "#テーブル移動";
                        break;
                    case 6:
                        this.lblItemRot[i].Tag = "20127";
                        this.lblItemRot[i].Text = "#Ｘ線管Ｘ軸移動";
                        break;
                    case 7:
                        this.lblItemRot[i].Tag = "20128";
                        this.lblItemRot[i].Text = "#Ｘ線管Ｙ軸移動";
                        break;
                    case 8:
                        this.lblItemRot[i].Tag = "20122";
                        this.lblItemRot[i].Text = "#I.I.移動";
                        break;
                    case 9:
                        this.lblItemRot[i].Tag = "12737";
                        this.lblItemRot[i].Text = "#ビニング";
                        break;

                    //”焦点”追加　'v19.50 v19.41とv18.02の統合 by長野 2013/11/05   //追加2014/10/07hata_v19.51反映
                    case 10:
                        this.lblItemRot[i].Tag = "12164";
                        this.lblItemRot[i].Text = "焦点";
                        break;
                    default:
                        break;
                }

                #endregion

                #region lblColonRot

                this.lblColonRot[i] = new Label();
                this.lblColonRot[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblColonRot[i].AutoSize = true;
                this.lblColonRot[i].Location = new System.Drawing.Point(111, 24 + i * 16);
                this.lblColonRot[i].Name = "lblColonRot" + i.ToString();
                this.lblColonRot[i].Size = new Size(7, 12);
                this.lblColonRot[i].TabIndex = 10;
                this.lblColonRot[i].Text = "：";
                this.lblColonRot[i].TextAlign = ContentAlignment.MiddleLeft;

                #endregion

                #region lblStatusRot

                this.lblStatusRot[i] = new Label();
                this.lblStatusRot[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblStatusRot[i].AutoSize = true;
                this.lblStatusRot[i].Location = new Point(120, 25 + i * 16);
                this.lblStatusRot[i].Name = "lblStatusRot" + i.ToString();
                this.lblStatusRot[i].Size = new Size(88, 12);
                this.lblStatusRot[i].TabIndex = 20;
                this.lblStatusRot[i].Text = "#lblStatusRot(" + i.ToString() + ")";
                this.lblStatusRot[i].TextAlign = ContentAlignment.MiddleLeft;
                this.lblStatusRot[i].TextChanged += new EventHandler(lblStatusRot_Change);

                #endregion

                this.fraRotate.Controls.Add(this.lblItemRot[i]);
                this.fraRotate.Controls.Add(this.lblColonRot[i]);
                this.fraRotate.Controls.Add(this.lblStatusRot[i]);
            }

            #endregion

            #region グループ3 - #ｽｷｬﾝ位置校正（実行時／現在値）

            for (int i = 0; i < lblItemSp.Length; i++)
            {
                #region lblItemSp

                this.lblItemSp[i] = new Label();

                this.lblItemSp[i].BackColor = Color.Cyan;
                this.lblItemSp[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblItemSp[i].Location = new Point(8, 24 + i * 16);
                this.lblItemSp[i].Name = "lblItemSp" + i.ToString();
                this.lblItemSp[i].Size = new Size(100, 15);
                this.lblItemSp[i].TabIndex = i;
                this.lblItemSp[i].TextAlign = ContentAlignment.MiddleLeft;

                switch (i)
                {
                    case 0:
                        this.lblItemSp[i].Tag = "12810";
                        this.lblItemSp[i].Text = "#I.I.視野";
                        break;
                    case 1:
                        this.lblItemSp[i].Tag = "12218";
                        this.lblItemSp[i].Text = "#X線管";
                        break;
                    case 2:
                        this.lblItemSp[i].Tag = "20122";
                        this.lblItemSp[i].Text = "#I.I.移動";
                        break;
                    case 3:
                        this.lblItemSp[i].Tag = "12737";
                        this.lblItemSp[i].Text = "#ビニング";
                        break;
                    case 4:
                        this.lblItemSp[i].Tag = "20123";
                        this.lblItemSp[i].Text = "#実行年月日時刻";
                        break;
                    default:
                        break;
                }

                #endregion

                #region lblColonSp

                this.lblColonSp[i] = new Label();
                this.lblColonSp[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblColonSp[i].AutoSize = true;
                this.lblColonSp[i].Location = new System.Drawing.Point(111, 24 + i * 16);
                this.lblColonSp[i].Name = "lblColonSp" + i.ToString();
                this.lblColonSp[i].Size = new System.Drawing.Size(7, 12);
                this.lblColonSp[i].TabIndex = i;
                this.lblColonSp[i].Text = "：";
                this.lblColonSp[i].TextAlign = ContentAlignment.MiddleLeft;

                #endregion

                #region lblStatusSp

                this.lblStatusSp[i] = new System.Windows.Forms.Label();
                this.fraScanPosi.Controls.Add(this.lblStatusSp[i]);
                this.lblStatusSp[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblStatusSp[i].AutoSize = true;
                this.lblStatusSp[i].Location = new Point(120, 24 + i * 16);
                this.lblStatusSp[i].Name = "lblStatusSp" + i.ToString();
                this.lblStatusSp[i].Size = new Size(83, 12);
                this.lblStatusSp[i].TabIndex = i;
                this.lblStatusSp[i].Text = "#lblStatusSp(" + i.ToString() + ")";
                this.lblStatusSp[i].TextAlign = ContentAlignment.MiddleLeft;
                this.lblStatusSp[i].TextChanged += new EventHandler(lblStatusSp_Change);

                #endregion

                this.fraScanPosi.Controls.Add(this.lblItemSp[i]);
                this.fraScanPosi.Controls.Add(this.lblColonSp[i]);
                this.fraScanPosi.Controls.Add(this.lblStatusSp[i]);
            }

            #endregion

            #region グループ4 - #オフセット校正（実行時／現在値）

            for (int i = 0; i < lblItemOff.Length; i++)
            {
                #region lblItemOff

                this.lblItemOff[i] = new Label();
                this.lblItemOff[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblItemOff[i].BackColor = Color.Cyan;
                this.lblItemOff[i].Location = new Point(8, 24 + i * 16);
                this.lblItemOff[i].Name = "lblItemOff" + i.ToString();
                this.lblItemOff[i].Size = new Size(100, 15);
                this.lblItemOff[i].TabIndex = i;
                this.lblItemOff[i].TextAlign = ContentAlignment.MiddleLeft;
                switch (i)
                {
                    case 0:
                        this.lblItemOff[i].Tag = "12094";
                        this.lblItemOff[i].Text = "#年月日";
                        break;
                    case 1:
                        this.lblItemOff[i].Tag = "12737";
                        this.lblItemOff[i].Text = "#ビニング";
                        break;
                    case 2:
                        this.lblItemOff[i].Tag = "20015";
                        this.lblItemOff[i].Text = "#FPDゲイン";
                        break;
                    case 3:
                        this.lblItemOff[i].Tag = "20016";
                        this.lblItemOff[i].Text = "#FPD積分時間";
                        break;
                    default:
                        break;
                }

                #endregion

                #region lblColonOff

                this.lblColonOff[i] = new Label();
                this.lblColonOff[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblColonOff[i].AutoSize = true;
                this.lblColonOff[i].Location = new Point(111, 24 + i * 16);
                this.lblColonOff[i].Name = "lblColonOff" + i.ToString();
                this.lblColonOff[i].Size = new Size(7, 12);
                this.lblColonOff[i].TabIndex = i;
                this.lblColonOff[i].Text = "：";
                this.lblColonOff[i].TextAlign = ContentAlignment.MiddleLeft;
                
                #endregion

                #region lblStatusOff

                this.lblStatusOff[i] = new Label();
                this.lblStatusOff[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblStatusOff[i].AutoSize = true;
                this.lblStatusOff[i].Location = new Point(120, 25 + i * 16);
                this.lblStatusOff[i].Name = "lblStatusOff" + i.ToString();
                this.lblStatusOff[i].Size = new Size(86, 12);
                this.lblStatusOff[i].TabIndex = i;
                this.lblStatusOff[i].Text = "#lblStatusOff(" + i.ToString() + ")";
                this.lblStatusOff[i].TextAlign = ContentAlignment.MiddleLeft;
                this.lblStatusOff[i].TextChanged += new EventHandler(lblStatusOff_Change);

                #endregion

                this.fraOffset.Controls.Add(this.lblItemOff[i]);
                this.fraOffset.Controls.Add(this.lblColonOff[i]);
                this.fraOffset.Controls.Add(this.lblStatusOff[i]);
            }

            #endregion

            #region グループ5 - #幾何歪校正（実行時／現在値）

            for (int i = 0; i < lblItemVer.Length; i++)
            {
                #region lblItemVer

                this.lblItemVer[i] = new Label();
                this.lblItemVer[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblItemVer[i].BackColor = Color.Cyan;
                this.lblItemVer[i].Location = new Point(8, 24 + i * 16);
                this.lblItemVer[i].Name = "lblItemVer" + i.ToString();
                this.lblItemVer[i].Size = new Size(100, 15);
                this.lblItemVer[i].TabIndex = i;
                this.lblItemVer[i].TextAlign = ContentAlignment.MiddleLeft;
                switch (i)
                {
                    case 0:
                        this.lblItemVer[i].Tag = "12810";
                        this.lblItemVer[i].Text = "#I.I.視野";
                        break;
                    case 1:
                        this.lblItemVer[i].Tag = "12218";
                        this.lblItemVer[i].Text = "#X線管";
                        break;
                    case 2:
                        this.lblItemVer[i].Tag = "20122";
                        this.lblItemVer[i].Text = "#I,I,移動";
                        break;
                    case 3:
                        this.lblItemVer[i].Tag = "12737";
                        this.lblItemVer[i].Text = "#ビニング";
                        break;
                    default:
                        break;
                }

                #endregion

                #region lblColonVer

                this.lblColonVer[i] = new System.Windows.Forms.Label();
                this.lblColonVer[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblColonVer[i].AutoSize = true;
                this.lblColonVer[i].Location = new Point(111, 24 + i * 16);
                this.lblColonVer[i].Name = "lblColonVer" + i.ToString();
                this.lblColonVer[i].Size = new Size(7, 12);
                this.lblColonVer[i].TabIndex = i;
                this.lblColonVer[i].Text = "：";
                this.lblColonVer[i].TextAlign = ContentAlignment.MiddleLeft;

                #endregion

                #region lblStatusVer0

                this.lblStatusVer[i] = new Label();
                this.lblStatusVer[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblStatusVer[i].AutoSize = true;
                this.lblStatusVer[i].Location = new Point(120, 25 + 16 * i);
                this.lblStatusVer[i].Name = "lblStatusVer" + i.ToString();
                this.lblStatusVer[i].Size = new Size(86, 12);
                this.lblStatusVer[i].TabIndex = i;
                this.lblStatusVer[i].Text = "#lblStatusVer(" + i.ToString() + ")";
                this.lblStatusVer[i].TextAlign = ContentAlignment.MiddleLeft;
                this.lblStatusVer[i].TextChanged += new EventHandler(lblStatusVer_Change);

                #endregion

                this.fraVertical.Controls.Add(this.lblItemVer[i]);
                this.fraVertical.Controls.Add(this.lblColonVer[i]);
                this.fraVertical.Controls.Add(this.lblStatusVer[i]);
            }

            #endregion

            #region グループ6 - #寸法校正（実行時／現在値）

            for (int i = 0; i < lblItemDist.Length; i++)
            {
                #region lblItemDist

                this.lblItemDist[i] = new Label();
                this.lblItemDist[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblItemDist[i].BackColor = Color.Cyan;
                this.lblItemDist[i].Location = new Point(8, 24 + i * 16);
                this.lblItemDist[i].Name = "lblItemDist" + i.ToString();
                this.lblItemDist[i].Size = new Size(100, 15);
                this.lblItemDist[i].TabIndex = 0;
                this.lblItemDist[i].TextAlign = ContentAlignment.MiddleLeft;

                switch (i)
                {
                    case 0:
                        this.lblItemDist[i].Tag = "12810";
                        this.lblItemDist[i].Text = "#I.I.視野";
                        break;
                    case 1:
                        this.lblItemDist[i].Text = "#X線管";
                        break;
                    case 2:
                        this.lblItemDist[i].Tag = "20122";
                        this.lblItemDist[i].Text = "#Ｘ軸移動";
                        break;
                    case 3:
                        this.lblItemDist[i].Tag = "20126";
                        this.lblItemDist[i].Text = "#テーブル移動";
                        break;
                    case 4:
                        this.lblItemDist[i].Tag = "20127";
                        this.lblItemDist[i].Text = "#Ｘ線管Ｘ軸移動";
                        break;
                    case 5:
                        this.lblItemDist[i].Tag = "20128";
                        this.lblItemDist[i].Text = "#Ｘ線管Ｙ軸移動";
                        break;
                    case 6:
                        this.lblItemDist[i].Tag = "20122";
                        this.lblItemDist[i].Text = "#I.I.移動";
                        break;
                    case 7:
                        this.lblItemDist[i].Tag = "12737";
                        this.lblItemDist[i].Text = "#ビニング";
                        break;
                    case 8:
                        this.lblItemDist[i].Tag = "20123";
                        this.lblItemDist[i].Text = "#実行年月日時刻";
                        break;
                    default:
                        break;
                }


                #endregion

                #region lblColonDist

                this.lblColonDist[i] = new System.Windows.Forms.Label();
                this.lblColonDist[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblColonDist[i].AutoSize = true;
                this.lblColonDist[i].Location = new System.Drawing.Point(111, 24 + i * 16);
                this.lblColonDist[i].Name = "lblColonDist" + i.ToString();
                this.lblColonDist[i].Size = new Size(7, 12);
                this.lblColonDist[i].TabIndex = i;
                this.lblColonDist[i].Text = "：";
                this.lblColonDist[i].TextAlign = ContentAlignment.MiddleLeft;

                #endregion

                #region lblStatusDist

                this.lblStatusDist[i] = new Label();
                this.lblStatusDist[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblStatusDist[i].AutoSize = true;
                this.lblStatusDist[i].Location = new Point(120, 25 + i * 16);
                this.lblStatusDist[i].Name = "lblStatusDist" + i.ToString();
                this.lblStatusDist[i].Size = new Size(91, 12);
                this.lblStatusDist[i].TabIndex = i;
                this.lblStatusDist[i].Text = "#lblStatusDist(" + i.ToString() + ")";
                this.lblStatusDist[i].TextAlign = ContentAlignment.MiddleLeft;
                this.lblStatusDist[i].TextChanged += new EventHandler(lblStatusDist_Change);

                #endregion

                this.fraDistance.Controls.Add(this.lblItemDist[i]);
                this.fraDistance.Controls.Add(this.lblColonDist[i]);
                this.fraDistance.Controls.Add(this.lblStatusDist[i]);
            }

            #endregion

            #region グループ7 - #シフトスキャン用（実行時／現在値）

            for (int i = 0; i < lblItemGainShift.Length; i++)
            {
                #region lblItemGainShift

                this.lblItemGainShift[i] = new Label();
                this.lblItemGainShift[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblItemGainShift[i].BackColor = Color.Cyan;
                this.lblItemGainShift[i].Location = new Point(8, 24 + i * 16);
                this.lblItemGainShift[i].Name = "lblItemGainShift" + i.ToString();
                this.lblItemGainShift[i].TextAlign = ContentAlignment.MiddleLeft;
                this.lblItemGainShift[i].Size = new Size(100, 15);
                this.lblItemGainShift[i].TabIndex = i;
                switch (i)
                {
                    case 0:
                        this.lblItemGainShift[i].Tag = "12810";
                        this.lblItemGainShift[i].Text = "#I.I.視野";
                        break;
                    case 1:
                        this.lblItemGainShift[i].Text = "管電圧(kV)";
                        break;
                    case 2:
                        this.lblItemGainShift[i].Tag = "12094";
                        this.lblItemGainShift[i].Text = "#年月日";
                        break;
                    case 3:
                        this.lblItemGainShift[i].Tag = "12218";
                        this.lblItemGainShift[i].Text = "#X線管";
                        break;
                    case 4:
                        this.lblItemGainShift[i].Tag = "12163";
                        this.lblItemGainShift[i].Text = "#ﾌｨﾙﾀ";
                        break;
                    case 5:
                        this.lblItemGainShift[i].Tag = "20122";
                        this.lblItemGainShift[i].Text = "#I.I.移動";
                        break;
                    case 6:
                        this.lblItemGainShift[i].Tag = "12737";
                        this.lblItemGainShift[i].Text = "#ビニング";
                        break;
                    case 7:
                        this.lblItemGainShift[i].Tag = "20015";
                        this.lblItemGainShift[i].Text = "#FPDゲイン";
                        break;
                    case 8:
                        this.lblItemGainShift[i].Tag = "20016";
                        this.lblItemGainShift[i].Text = "#FPD積分時間";
                        break;
                    case 9:
                        this.lblItemGainShift[i].Tag = "20121";
                        this.lblItemGainShift[i].Text = "#ｵﾌｾｯﾄ実行時刻";
                        break;

                    //”焦点”追加　'v19.50 v19.41とv18.02の統合 by長野 2013/11/05   //追加2014/10/07hata_v19.51反映
                    case 10:
                        this.lblItemGainShift[i].Tag = "12164";
                        this.lblItemGainShift[i].Text = "焦点";
                        break;
                    default:
                        break;
                }

                #endregion

                #region lblColonGainShift

                this.lblColonGainShift[i] = new Label();
                this.lblColonGainShift[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblColonGainShift[i].AutoSize = true;
                this.lblColonGainShift[i].Name = "lblColonGainShift" + i.ToString();
                this.lblColonGainShift[i].Size = new Size(7, 12);
                this.lblColonGainShift[i].Location = new Point(111, 27 + i * 16);
                this.lblColonGainShift[i].TabIndex = i;
                this.lblColonGainShift[i].Text = "：";
                this.lblColonGainShift[i].TextAlign = ContentAlignment.MiddleLeft;

                #endregion

                #region lblStatusGainShift

                this.lblStatusGainShift[i] = new System.Windows.Forms.Label();
                this.lblStatusGainShift[i].Font = new Font("ＭＳ Ｐゴシック", 9F);
                this.lblStatusGainShift[i].AutoSize = true;
                this.lblStatusGainShift[i].Location = new Point(120, 25 + i * 16);
                this.lblStatusGainShift[i].Name = "lblStatusGainShift" + i.ToString(); ;
                this.lblStatusGainShift[i].Size = new Size(93, 12);
                this.lblStatusGainShift[i].TabIndex = 20;
                this.lblStatusGainShift[i].Text = "#lblStatusGainShift(" + i.ToString() + ")";
                this.lblStatusGainShift[i].TextAlign = ContentAlignment.MiddleLeft;
                this.lblStatusGainShift[i].TextChanged += new EventHandler(lblStatusGainShift_Change);

                #endregion

                this.fraGainShift.Controls.Add(this.lblItemGainShift[i]);
                this.fraGainShift.Controls.Add(this.lblColonGainShift[i]);
                this.fraGainShift.Controls.Add(this.lblStatusGainShift[i]);
            }

            #endregion


            #region ボタン

            for (int i = 1; i < cmdCorrect.Length; i++)
            {
                
                cmdCorrect[i] = new Button();
                cmdCorrect[i].Font = new Font("ＭＳ Ｐゴシック", 11F);
                cmdCorrect[i].Location = new Point(16, 24 + (i - 1) * 36);
                cmdCorrect[i].Name = "cmdCorrect1";
                cmdCorrect[i].Size = new Size(161, 27);
                cmdCorrect[i].TabIndex = i;
                cmdCorrect[i].UseVisualStyleBackColor = true;
                cmdCorrect[i].Click += new EventHandler(cmdCorrect_Click);
                
                switch (i)
                {
                    case 0:
                        break;
                    case 1:
                        cmdCorrect[i].Tag = "10904";
                        cmdCorrect[i].Text = "#ゲイン校正";
                        break;
                    case 2:
                        cmdCorrect[i].Tag = "10901";
                        cmdCorrect[i].Text = "#スキャン位置校正";
                        break;
                    case 3:
                        cmdCorrect[i].Tag = "10902";
                        cmdCorrect[i].Text = "#幾何歪校正";
                        break;
                    case 4:
                        cmdCorrect[i].Tag = "10903";
                        cmdCorrect[i].Text = "#回転中心校正";
                        break;
                    case 5:
                        cmdCorrect[i].Tag = "10905";
                        cmdCorrect[i].Text = "#オフセット校正";
                        break;
                    case 6:
                        cmdCorrect[i].Tag = "10906";
                        cmdCorrect[i].Text = "#寸法校正";
                        break;
                    case 7:
                        cmdCorrect[i].Tag = "10907";
                        cmdCorrect[i].Text = "#マルチスライス校正";
                        break;
                    default:
                        break;
                }
                fraScanCorrect.Controls.Add(cmdCorrect[i]);
            }

            #endregion

            this.ResumeLayout(false);

        }
        #endregion


        #region コントロールの初期化
        //*******************************************************************************
        //機　　能： コントロールの初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void InitControls()
		{
            //const int DEF_LEFT = 8;         //Ｘ座標の初期値     'v18.00追加 byやまおか 2011/02/10 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            const int DEF_TOP = 24;         //Ｙ座標の初期値     'V4.0 append by 鈴山 2001/04/02
            const int INC_TOP = 16;         //Ｙ座標の加算値     'V4.0 append by 鈴山 2001/04/02
            int iTop = 0;                   //Ｙ座標             'V4.0 append by 鈴山 2001/04/02
            int i = 0;                      //汎用               'V4.0 append by 鈴山 2001/04/02

            //変更2014/11/28hata_v19.51_dnet
            //フォント(System)が無い。ゴシックで対応のためオフセットは"0"とする。　
            //const int ColonOffset = -5;      //v7.0 追加 英語化対応 by 間々田 2003/08/21
            const int ColonOffset = 0;      //v7.0 追加 英語化対応 by 間々田 2003/08/21
            
            int iTop_last = 0;            //最後の項目のＹ座標 'v18.00追加 byやまおか 2011/02/10 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05


			//幾何歪校正ボタン：Ｘ線検出器がフラットパネルの場合は、使用不可
            if (CTSettings.detectorParam.Use_FlatPanel)
            {
				cmdCorrect[3].Text = "";
			}

			//v29.99 今のところ不要のため必ず表示しないようにする by長野 2013/04/08'''''ここから'''''
			//cmdCorrect().Caption=""のときに非表示になるようにしてある
			//マルチスライスボタン
			//If scaninh.multislice <> 0 Then
			cmdCorrect[7].Text = "";
			//End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			int theTop = 0;
            //ボタンの配置（キャプションがヌルに設定されている場合、ボタンを非表示にする）
            theTop = cmdCorrect[1].Top;

            for (i = 1; i <= 7; i++)
            {
                if (!string.IsNullOrEmpty(cmdCorrect[i].Text))
                {
                    cmdCorrect[i].Top = theTop;
                    theTop = theTop + 42;
                }
                else
                {
                    cmdCorrect[i].Visible = false;
                }
            }

            //２次元幾何歪の場合，スキャン位置校正と幾何歪校正の配置を入れ替える
            //If scaninh.full_distortion = 0 Then                            'v17.00削除 byやまおか 2010/02/02
            //v17.00追加 byやまおか 2010/02/02
            if ((CTSettings.scaninh.Data.full_distortion == 0) && (!CTSettings.detectorParam.Use_FlatPanel))
            {
                theTop = cmdCorrect[3].Top;
                cmdCorrect[3].Top = cmdCorrect[2].Top;
                cmdCorrect[2].Top = theTop;
            }

			//FPDの場合、幾何歪校正ステータスは表示しない added by 間々田 2003/10/20
            fraVertical.Visible = !CTSettings.detectorParam.Use_FlatPanel;

			//スキャン位置校正の表示／非表示
            lblItemSp[0].Visible = (CTSettings.scaninh.Data.iifield == 0) && (!CTSettings.detectorParam.Use_FlatPanel);
            lblColonSp[0].Visible = (CTSettings.scaninh.Data.iifield == 0) && (!CTSettings.detectorParam.Use_FlatPanel);
            lblStatusSp[0].Visible = (CTSettings.scaninh.Data.iifield == 0) && (!CTSettings.detectorParam.Use_FlatPanel);
            
            lblItemSp[1].Visible = (CTSettings.scaninh.Data.multi_tube == 0);
            lblColonSp[1].Visible = (CTSettings.scaninh.Data.multi_tube == 0);
            lblStatusSp[1].Visible = (CTSettings.scaninh.Data.multi_tube == 0);
            
            lblItemSp[2].Visible = (CTSettings.scaninh.Data.ii_move == 0);
            lblColonSp[2].Visible = (CTSettings.scaninh.Data.ii_move == 0);
            lblStatusSp[2].Visible = (CTSettings.scaninh.Data.ii_move == 0);
            
            lblItemSp[3].Visible = (CTSettings.scaninh.Data.binning == 0);
            lblColonSp[3].Visible = (CTSettings.scaninh.Data.binning == 0);
			lblStatusSp[3].Visible = (CTSettings.scaninh.Data.binning == 0);
			
            lblItemSp[4].Visible = true;//v17.02追加 byやまおか 2010/07/08
			lblColonSp[4].Visible = true;//v17.02追加 byやまおか 2010/07/08
			lblStatusSp[4].Visible = true;//v17.02追加 byやまおか 2010/07/08
			
			//幾何歪校正の表示／非表示
            lblItemVer[0].Visible = (CTSettings.scaninh.Data.iifield == 0) && (!CTSettings.detectorParam.Use_FlatPanel);
            lblColonVer[0].Visible = (CTSettings.scaninh.Data.iifield == 0) && (!CTSettings.detectorParam.Use_FlatPanel);
            lblStatusVer[0].Visible = (CTSettings.scaninh.Data.iifield == 0) && (!CTSettings.detectorParam.Use_FlatPanel);
            lblItemVer[1].Visible = (CTSettings.scaninh.Data.multi_tube == 0);
            lblColonVer[1].Visible = (CTSettings.scaninh.Data.multi_tube == 0);
            lblStatusVer[1].Visible = (CTSettings.scaninh.Data.multi_tube == 0);
            lblItemVer[2].Visible = (CTSettings.scaninh.Data.ii_move == 0);      //v9.6 追加 by 間々田 2004/10/20
            lblColonVer[2].Visible = (CTSettings.scaninh.Data.ii_move == 0);     //v9.6 追加 by 間々田 2004/10/20
            lblStatusVer[2].Visible = (CTSettings.scaninh.Data.ii_move == 0);    //v9.6 追加 by 間々田 2004/10/20
            lblItemVer[3].Visible = (CTSettings.scaninh.Data.binning == 0);
            lblColonVer[3].Visible = (CTSettings.scaninh.Data.binning == 0);
            lblStatusVer[3].Visible = (CTSettings.scaninh.Data.binning == 0);

			//回転中心校正の表示／非表示
            lblItemRot[0].Visible = (CTSettings.scaninh.Data.xray_remote == 0);
            lblColonRot[0].Visible = (CTSettings.scaninh.Data.xray_remote == 0);
            lblStatusRot[0].Visible = (CTSettings.scaninh.Data.xray_remote == 0);
			lblItemRot[1].Visible = false;			//v13.0追加 by 間々田 2007/04/16 昇降位置を非表示とする
			lblColonRot[1].Visible = false;			//v13.0追加 by 間々田 2007/04/16 昇降位置を非表示とする
			lblStatusRot[1].Visible = false;        //v13.0追加 by 間々田 2007/04/16 昇降位置を非表示とする
            lblItemRot[2].Visible = (CTSettings.scaninh.Data.iifield == 0) && (!CTSettings.detectorParam.Use_FlatPanel);    //change by 間々田 2003/10/09
            lblColonRot[2].Visible = (CTSettings.scaninh.Data.iifield == 0) && (!CTSettings.detectorParam.Use_FlatPanel);	//change by 間々田 2003/10/09
            lblStatusRot[2].Visible = (CTSettings.scaninh.Data.iifield == 0) && (!CTSettings.detectorParam.Use_FlatPanel);	//change by 間々田 2003/10/09
            lblItemRot[3].Visible = (CTSettings.scaninh.Data.multi_tube == 0) || (CTSettings.scaninh.Data.rotate_select == 0);	//v9.0 変更 by 間々田 2004/02/06
            lblColonRot[3].Visible = (CTSettings.scaninh.Data.multi_tube == 0) || (CTSettings.scaninh.Data.rotate_select == 0);   //v9.0 変更 by 間々田 2004/02/06
            lblStatusRot[3].Visible = (CTSettings.scaninh.Data.multi_tube == 0) || (CTSettings.scaninh.Data.rotate_select == 0);  //v9.0 変更 by 間々田 2004/02/06
            lblItemRot[6].Visible = (CTSettings.scaninh.Data.rotate_select == 0);	//v9.0 追加 by 間々田 2004/02/06
            lblColonRot[6].Visible = (CTSettings.scaninh.Data.rotate_select == 0);	//v9.0 追加 by 間々田 2004/02/06
            lblStatusRot[6].Visible = (CTSettings.scaninh.Data.rotate_select == 0);	//v9.0 追加 by 間々田 2004/02/06
            lblItemRot[7].Visible = (CTSettings.scaninh.Data.rotate_select == 0);	//v9.0 追加 by 間々田 2004/02/06
            lblColonRot[7].Visible = (CTSettings.scaninh.Data.rotate_select == 0);	//v9.0 追加 by 間々田 2004/02/06
            lblStatusRot[7].Visible = (CTSettings.scaninh.Data.rotate_select == 0);	//v9.0 追加 by 間々田 2004/02/06
            lblItemRot[8].Visible = (CTSettings.scaninh.Data.ii_move == 0);
            lblColonRot[8].Visible = (CTSettings.scaninh.Data.ii_move == 0);
            lblStatusRot[8].Visible = (CTSettings.scaninh.Data.ii_move == 0);
            lblItemRot[9].Visible = (CTSettings.scaninh.Data.binning == 0);
            lblColonRot[9].Visible = (CTSettings.scaninh.Data.binning == 0);
            lblStatusRot[9].Visible = (CTSettings.scaninh.Data.binning == 0);
            //追加2014/10/07hata_v19.51反映
            //lblItemRot[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan);      //焦点 'v19.50追加 by長野 2013/11/08
            //lblColonRot[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan);     //焦点 'v19.50追加 by長野 2011/11/08
            //lblStatusRot[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan);    //焦点 'v19.50追加 by長野 2011/11/08
            //Rev25.03 change by chouno 2017/02/05
            lblItemRot[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & (Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman));
            lblColonRot[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & (Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman));
            lblStatusRot[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & (Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman));

			//ゲイン校正の表示／非表示
            lblItemGain[0].Visible = (CTSettings.scaninh.Data.iifield == 0) && (!CTSettings.detectorParam.Use_FlatPanel);
            lblColonGain[0].Visible = (CTSettings.scaninh.Data.iifield == 0) && (!CTSettings.detectorParam.Use_FlatPanel);
            lblStatusGain[0].Visible = (CTSettings.scaninh.Data.iifield == 0) && (!CTSettings.detectorParam.Use_FlatPanel);
            lblItemGain[1].Visible = (CTSettings.scaninh.Data.xray_remote == 0);
            lblColonGain[1].Visible = (CTSettings.scaninh.Data.xray_remote == 0);
            lblStatusGain[1].Visible = (CTSettings.scaninh.Data.xray_remote == 0);
			//lblItemGain(2).Visible = (.xray_remote = 0)
			//lblColonGain(2).Visible = (.xray_remote = 0)
			//lblStatusGain(2).Visible = (.xray_remote = 0)
			//v11.2変更 by 間々田 2006/01/31 ゲインの管電流の項目を削除
			//lblItemGain(2).Visible = False     'v12.01削除 この位置に年月日を表示させることにした by 間々田 2006/12/04
			//lblColonGain(2).Visible = False    'v12.01削除 この位置に年月日を表示させることにした by 間々田 2006/12/04
			//lblStatusGain(2).Visible = False   'v12.01削除 この位置に年月日を表示させることにした by 間々田 2006/12/04

            lblItemGain[3].Visible = (CTSettings.scaninh.Data.multi_tube == 0);
            lblColonGain[3].Visible = (CTSettings.scaninh.Data.multi_tube == 0);
            lblStatusGain[3].Visible = (CTSettings.scaninh.Data.multi_tube == 0);
            lblItemGain[4].Visible = (CTSettings.scaninh.Data.filter == 0);
            lblColonGain[4].Visible = (CTSettings.scaninh.Data.filter == 0);
            lblStatusGain[4].Visible = (CTSettings.scaninh.Data.filter == 0);

            lblItemGain[5].Visible = (CTSettings.scaninh.Data.ii_move == 0);
            lblColonGain[5].Visible = (CTSettings.scaninh.Data.ii_move == 0);
            lblStatusGain[5].Visible = (CTSettings.scaninh.Data.ii_move == 0);
            lblItemGain[6].Visible = (CTSettings.scaninh.Data.binning == 0);
            lblColonGain[6].Visible = (CTSettings.scaninh.Data.binning == 0);
            lblStatusGain[6].Visible = (CTSettings.scaninh.Data.binning == 0);

            lblItemGain[7].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);     //FPDゲイン  'v17.00追加 byやまおか 2010/02/17
            lblColonGain[7].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);    //FPDゲイン  'v17.00追加 byやまおか 2010/02/17
            lblStatusGain[7].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);   //FPDゲイン  'v17.00追加 byやまおか 2010/02/17
            lblItemGain[8].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);     //FPD積分時間'v17.00追加 byやまおか 2010/02/17
            lblColonGain[8].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);    //FPD積分時間'v17.00追加 byやまおか 2010/02/17
            lblStatusGain[8].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);   //FPD積分時間'v17.00追加 byやまおか 2010/02/17
            lblItemGain[9].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);     //オフセット校正'v17.00追加 byやまおか 2010/03/04
            lblColonGain[9].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);    //オフセット校正'v17.00追加 byやまおか 2010/03/04
            lblStatusGain[9].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);   //オフセット校正'v17.00追加 byやまおか 2010/03/04

            //追加2014/10/07hata_v19.51反映
            //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから
            //追加2014/10/07hata_v19.51反映
            //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから
            //lblItemGain[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan);     //焦点 'v18.00追加 byやまおか 2011/03/11
            //lblColonGain[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan);    //焦点 'v18.00追加 byやまおか 2011/03/11
            //lblStatusGain[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan);   //焦点 'v18.00追加 byやまおか 2011/03/11
            //Rev25.03 change by chouno 2017/02/05
            lblItemGain[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & (Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman));    //焦点 'v18.00追加 byやまおか 2011/03/11
            lblColonGain[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & (Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman));    //焦点 'v18.00追加 byやまおか 2011/03/11
            lblStatusGain[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & (Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman));  //焦点 'v18.00追加 byやまおか 2011/03/11

            //ゲイン校正（シフトスキャン）の表示／非表示     'v18.00追加 byやまおか 2011/02/10
            lblItemGainShift[0].Visible = (CTSettings.scaninh.Data.iifield == 0) & (!CTSettings.detectorParam.Use_FlatPanel);            //I.I.視野
            lblColonGainShift[0].Visible = (CTSettings.scaninh.Data.iifield == 0) & (!CTSettings.detectorParam.Use_FlatPanel);
            lblStatusGainShift[0].Visible = (CTSettings.scaninh.Data.iifield == 0) & (!CTSettings.detectorParam.Use_FlatPanel);
            lblItemGainShift[1].Visible = (CTSettings.scaninh.Data.xray_remote == 0);            //管電圧
            lblColonGainShift[1].Visible = (CTSettings.scaninh.Data.xray_remote == 0);
            lblStatusGainShift[1].Visible = (CTSettings.scaninh.Data.xray_remote == 0);

            lblItemGainShift[3].Visible = (CTSettings.scaninh.Data.multi_tube == 0);            //X線管
            lblColonGainShift[3].Visible = (CTSettings.scaninh.Data.multi_tube == 0);
            lblStatusGainShift[3].Visible = (CTSettings.scaninh.Data.multi_tube == 0);
            lblItemGainShift[4].Visible = (CTSettings.scaninh.Data.filter == 0);            //フィルタ
            lblColonGainShift[4].Visible = (CTSettings.scaninh.Data.filter == 0);
            lblStatusGainShift[4].Visible = (CTSettings.scaninh.Data.filter == 0);

            lblItemGainShift[5].Visible = (CTSettings.scaninh.Data.ii_move == 0);            //検出器移動
            lblColonGainShift[5].Visible = (CTSettings.scaninh.Data.ii_move == 0);
            lblStatusGainShift[5].Visible = (CTSettings.scaninh.Data.ii_move == 0);
            lblItemGainShift[6].Visible = (CTSettings.scaninh.Data.binning == 0);            //ビニング
            lblColonGainShift[6].Visible = (CTSettings.scaninh.Data.binning == 0);
            lblStatusGainShift[6].Visible = (CTSettings.scaninh.Data.binning == 0);

            lblItemGainShift[7].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);            //FPDゲイン
            lblColonGainShift[7].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);
            lblStatusGainShift[7].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);
            lblItemGainShift[8].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);            //FPD積分時間
            lblColonGainShift[8].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);
            lblStatusGainShift[8].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);

            lblItemGainShift[9].Visible = false;            //オフセット校正(共通で使用するため無効)
            lblColonGainShift[9].Visible = false;
            lblStatusGainShift[9].Visible = false;

            //lblItemGainShift[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan);            //焦点 'v18.00追加 byやまおか 2011/03/11
            //lblColonGainShift[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan);            //焦点 'v18.00追加 byやまおか 2011/03/11
            //lblStatusGainShift[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan);            //焦点 'v18.00追加 byやまおか 2011/03/11
            //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここまで
            //Rev25.03 change by chouno 2017/02/05
            lblItemGainShift[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & (Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman));            //焦点 'v18.00追加 byやまおか 2011/03/11
            lblColonGainShift[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & (Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman));           //焦点 'v18.00追加 byやまおか 2011/03/11
            lblStatusGainShift[10].Visible = Convert.ToBoolean(CTSettings.scaninh.Data.focus_change == 0) & (Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || Convert.ToBoolean(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman));           //焦点 'v18.00追加 byやまおか 2011/03/11

			//オフセット校正の表示／非表示
			lblItemOff[1].Visible = (CTSettings.scaninh.Data.binning == 0);
            lblColonOff[1].Visible = (CTSettings.scaninh.Data.binning == 0);
            lblStatusOff[1].Visible = (CTSettings.scaninh.Data.binning == 0);
            lblItemOff[2].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);
			//FPDゲイン  'v17.00追加 byやまおか 2010/02/17
            lblColonOff[2].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);
			//FPDゲイン  'v17.00追加 byやまおか 2010/02/17
            lblStatusOff[2].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);
			//FPDゲイン  'v17.00追加 byやまおか 2010/02/17
            lblItemOff[3].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);
			//FPD積分時間'v17.00追加 byやまおか 2010/02/17
            lblColonOff[3].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);
			//FPD積分時間'v17.00追加 byやまおか 2010/02/17
            lblStatusOff[3].Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);
			//FPD積分時間'v17.00追加 byやまおか 2010/02/17

			//寸法校正の表示／非表示
			//lblItemDist(0).Visible = (.iifield = 0) And (Not Use_FlatPanel)    'change by 間々田 2003/10/09
			//lblColonDist(0).Visible = (.iifield = 0) And (Not Use_FlatPanel)   'change by 間々田 2003/10/09
			//lblStatusDist(0).Visible = (.iifield = 0) And (Not Use_FlatPanel)  'change by 間々田 2003/10/09
			//lblItemDist(1).Visible = (.multi_tube = 0) Or (.rotate_select = 0)      'v9.0 変更 by 間々田 2004/02/06
			//lblColonDist(1).Visible = (.multi_tube = 0) Or (.rotate_select = 0)     'v9.0 変更 by 間々田 2004/02/06
			//lblStatusDist(1).Visible = (.multi_tube = 0) Or (.rotate_select = 0)    'v9.0 変更 by 間々田 2004/02/06
			//lblItemDist(4).Visible = (.rotate_select = 0)                       'v9.0 追加 by 間々田 2004/02/06
			//lblColonDist(4).Visible = (.rotate_select = 0)                      'v9.0 追加 by 間々田 2004/02/06
			//lblStatusDist(4).Visible = (.rotate_select = 0)                     'v9.0 追加 by 間々田 2004/02/06
			//lblItemDist(5).Visible = (.rotate_select = 0)                       'v9.0 追加 by 間々田 2004/02/06
			//lblColonDist(5).Visible = (.rotate_select = 0)                      'v9.0 追加 by 間々田 2004/02/06
			//lblStatusDist(5).Visible = (.rotate_select = 0)                     'v9.0 追加 by 間々田 2004/02/06
			//lblItemDist(6).Visible = (.ii_move = 0)
			//lblColonDist(6).Visible = (.ii_move = 0)
			//lblStatusDist(6).Visible = (.ii_move = 0)
			//lblItemDist(7).Visible = (.binning = 0)
			//lblColonDist(7).Visible = (.binning = 0)
			//lblStatusDist(7).Visible = (.binning = 0)
			//寸法校正方法の変更によりステータスは見ないことにした   'v17.00変更 byやまおか 2010/03/02
			for (i = 0; i <= 7; i++)
            {
				lblItemDist[i].Visible = false;
				lblColonDist[i].Visible = false;
				lblStatusDist[i].Visible = false;
			}
			//何もないとわからないので常に完了で実行した日付だけを表示する   'v17.00追加 byやまおか 2010/03/02
			lblItemDist[8].Visible = true;
			lblColonDist[8].Visible = true;
			lblStatusDist[8].Visible = true;

            //Rev23.20 2世代・3世代兼用の場合は、テーブル移動有だけ表示 by長野 2016/01/23
            //if (CTSettings.scaninh.Data.lr_sft == 0)
            //Rev25.00 修正 by長野 2016/07/07
            if (CTSettings.scaninh.Data.ct_gene2and3 == 0)
            {
                lblItemDist[2].Visible = true;
                lblColonDist[2].Visible = true;
                lblStatusDist[2].Visible = true;
                lblItemDist[3].Visible = true;
                lblColonDist[3].Visible = true;
                lblStatusDist[3].Visible = true;
            }
        
			//'v10.03追加 寸法校正のテーブル移動項目は削除    by 間々田 2005/06/08   'v11.2復活 by 間々田 2006/01/11
			//lblItemDist(3).Visible = False
			//lblColonDist(3).Visible = False
			//lblStatusDist(3).Visible = False


			//スキャン位置校正（実行時／現在値）
			iTop = DEF_TOP;
			for (i = lblItemSp.GetLowerBound(0); i <= lblItemSp.GetUpperBound(0); i++)
            {
				if (lblItemSp[i].Visible)
                {
					lblItemSp[i].Top = iTop;
					lblColonSp[i].Top = iTop + ColonOffset;
					lblStatusSp[i].Top = iTop;
					iTop = iTop + INC_TOP;
				}
			}

			//幾何歪校正（実行時／現在値）
			iTop = DEF_TOP;
            for (i = lblItemVer.GetLowerBound(0); i <= lblItemVer.GetUpperBound(0); i++)
            {
				if (lblItemVer[i].Visible)
                {
					lblItemVer[i].Top = iTop;
					//            lblColonVer(i).Top = iTop
					lblColonVer[i].Top = iTop + ColonOffset;
					//v7.0 変更 英語化対応 by 間々田 2003/08/21
					lblStatusVer[i].Top = iTop;
					iTop = iTop + INC_TOP;
				}
			}


			//回転中心校正（実行時／現在値）
			iTop = DEF_TOP;
			for (i = lblItemRot.GetLowerBound(0); i <= lblItemRot.GetUpperBound(0); i++)
            {
				if (lblItemRot[i].Visible)
                {
					lblItemRot[i].Top = iTop;
					//            lblColonRot(i).Top = iTop
					lblColonRot[i].Top = iTop + ColonOffset;
					//v7.0 変更 英語化対応 by 間々田 2003/08/21
					lblStatusRot[i].Top = iTop;
					iTop = iTop + INC_TOP;
				}
			}

			//ゲイン校正（実行時／現在値）
			iTop = DEF_TOP;
            for (i = lblItemGain.GetLowerBound(0); i <= lblItemGain.GetUpperBound(0); i++)
            {
				if (lblItemGain[i].Visible)
                {
					lblItemGain[i].Top = iTop;
					//            lblColonGain(i).Top = iTop
					lblColonGain[i].Top = iTop + ColonOffset;					//v7.0 変更 英語化対応 by 間々田 2003/08/21
					lblStatusGain[i].Top = iTop;
					iTop = iTop + INC_TOP;

                    //追加2014/10/07hata_v19.51反映
                    iTop_last = iTop;                    //v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                }
			}

            //追加2014/10/07hata_v19.51反映
            //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから
            //ゲイン校正（実行時／現在値）（シフトスキャン） 'v18.00追加 byやまおか 2011/02/10
            //iTop = DEF_TOP - 120;
            iTop = DEF_TOP;
            for (i = lblItemGainShift.GetLowerBound(0); i <= lblItemGainShift.GetUpperBound(0); i++)
            {
                if (lblItemGainShift[i].Visible)
                {
                    lblItemGainShift[i].Top = iTop;
                    lblColonGainShift[i].Top = iTop + ColonOffset;
                    lblStatusGainShift[i].Top = iTop;
                    iTop = iTop + INC_TOP;
                }
            }
            //fraGainShift.Visible = CTSettings.DetShiftOn;
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            fraGainShift.Visible = (CTSettings.DetShiftOn || CTSettings.W_ScanOn);
            fraGainShift.Left = fraGain.Left + 4;
            fraGainShift.Top = DEF_TOP + iTop_last;
            fraGainShift.Height = iTop + 4;
            fraGain.Height = iTop_last + fraGainShift.Height + 4;
            //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここまで

			//オフセット校正（実行時／現在値）   'v17.00追加(ここから) byやまおか 2010/02/17
			iTop = DEF_TOP;
            for (i = lblItemOff.GetLowerBound(0); i <= lblItemOff.GetUpperBound(0); i++)
            {
				if (lblItemOff[i].Visible)
                {
					lblItemOff[i].Top = iTop;
					lblColonOff[i].Top = iTop + ColonOffset;
					lblStatusOff[i].Top = iTop;
					iTop = iTop + INC_TOP;
				}
			}
			//v17.00追加(ここまで) byやまおか 2010/02/17

			//寸法校正（実行時／現在値）
			iTop = DEF_TOP;
            for (i = lblItemDist.GetLowerBound(0); i <= lblItemDist.GetUpperBound(0); i++)
            {
				if (lblItemDist[i].Visible) {
					lblItemDist[i].Top = iTop;
					//            lblColonDist(i).Top = iTop
					lblColonDist[i].Top = iTop + ColonOffset;
					//v7.0 変更 英語化対応 by 間々田 2003/08/21
					lblStatusDist[i].Top = iTop;
					iTop = iTop + INC_TOP;
				}
			}

			//２次元幾何歪の場合，スキャン位置校正と幾何歪校正の配置を入れ替える     v11.2追加 by 間々田 2005/10/07
			if (CTSettings.scaninh.Data.full_distortion == 0)
            {
				theTop = fraScanPosi.Top;
				fraScanPosi.Top = fraVertical.Top;
				fraVertical.Top = theTop;
			}

			//PkeFPDの場合はオフセット校正を先頭にする   'v17.00追加 byやまおか 2010/03/01
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
				cmdCorrect[5].Top = cmdCorrect[1].Top;
				for (i = 1; i <= 4; i++)
                {
					cmdCorrect[i].Top = cmdCorrect[i].Top + 42;
				}
			}
		}

        #endregion

        #region 「閉じる」ボタンクリック時処理

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
		private void cmdClose_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			//校正ステータスフォームのアンロード
			//Unload Me
			
            //this.Hide();    //v11.2変更 by 間々田 2006/01/11 アンロードせず、非表示にするだけにした
            this.WindowState = FormWindowState.Minimized;
            pnlCorStatus.Parent = this;

            //モーダル用のFormを閉じる
            frmCorrectionStatusModal.Instance.Close();

        }

        #endregion

        #region フォームロード時の処理

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
		private void frmCorrectionStatus_Load(object sender, EventArgs e)
		{
			//キャプションのセット
			SetCaption();

			//コントロールの初期化
			InitControls();

			//文字列配列の初期化
            strMove[0] = CTResources.LoadResString(12091);   //移動なし
            strMove[1] = CTResources.LoadResString(12090);   //移動あり
            strSeparator = CTResources.LoadResString(10804); //／	
		}

        #endregion

        #region 各コントロールのキャプションに文字列をセットする

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
			string strMoveII;
			string strMoveTable;

			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

			//スキャン校正フレーム：英語の場合、" correction"を取り除く
			int i = 0;
			for (i = 1; i <= 7; i++)
            {
				//.Caption = Replace$(.Caption, " correction", "")
				//v17.60　取り除く文字列を"calibrationに変更" by 長野 2011/06/09
                cmdCorrect[i].Text = cmdCorrect[i].Text.Replace(" calibration", "");
			}

			//I.I.移動（またはFPD移動）
            strMoveII = StringTable.GetResString(StringTable.IDS_Move, CTSettings.GStrIIOrFPD);

			//テーブル移動（またはＸ線管移動）
            strMoveTable = CTSettings.GStrTableOrXray;

			this.Text = StringTable.BuildResStr(StringTable.IDS_Details, StringTable.IDS_Correction);
			//校正－詳細

			//詳細表示：ゲイン校正
            lblItemGain[1].Text = CTResources.LoadResString(StringTable.IDS_TubeVoltage) + "(kV)";			//管電圧(kV)
			//lblItemGain(2).Caption = LoadResString(9004)                              '管電流(μA)       'v11.4削除 by 間々田 2006/03/03
			//    lblItemGain(2).Caption = LoadResString(IDS_Date)                        '年月日             'v12.01追加 by 間々田 2006/12/04
			//    lblItemGain(3).Caption = LoadResString(IDS_XrayTube)                    'Ｘ線管
			//    lblItemGain(4).Caption = LoadResString(IDS_Filter)                      'フィルタ
			lblItemGain[5].Text = strMoveII;			//I.I.移動

            //追加2014/10/07hata_v19.51反映
            //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから
            //詳細表示：ゲイン校正（シフトスキャン用）
            
            //Rev25.00 従来シフトかWスキャンかで、表示する文字列を変える
            if (CTSettings.W_ScanOn)
            {
                fraGainShift.Text = CTResources.LoadResString(StringTable.IDS_W_Scan);                //Wスキャン
            }
            else
            {
                fraGainShift.Text = CTResources.LoadResString(StringTable.IDS_ShiftScan);            //シフトスキャン 'v18.00追加 byやまおか 2011/07/09
            }

            lblItemGainShift[1].Text = CTResources.LoadResString(StringTable.IDS_TubeVoltage) + "(kV)";            //管電圧(kV)     'v18.00追加 byやまおか 2011/02/10
            lblItemGainShift[5].Text = strMoveII;            //I.I.移動       'v18.00追加 byやまおか 2011/02/10
            //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここまで

			//詳細表示：スキャン位置校正
			//    lblItemSp(1).Caption = LoadResString(IDS_XrayTube)                      'Ｘ線管
    		lblItemSp[2].Text = strMoveII;	            //I.I.移動           'v11.2追加 by 間々田 2005/10/07

			//詳細表示：幾何歪校正
			//    lblItemVer(1).Caption = LoadResString(IDS_XrayTube)                     'Ｘ線管
			lblItemVer[2].Text = strMoveII;			//I.I.移動

			//詳細表示：回転中心校正
            lblItemRot[0].Text = CTResources.LoadResString(StringTable.IDS_TubeVoltage) + "(kV)";			//管電圧(kV)
			lblItemRot[1].Text = CTResources.LoadResString(12190) + "(mm)";			//昇降位置(mm)
			//lblItemRot(3).Caption = LoadResString(12218)                           'X線管
            if (CTSettings.scaninh.Data.rotate_select == 0)
            {
                lblItemRot[3].Text = CTResources.LoadResString(12331);
            }
            else
            {
                lblItemRot[3].Text = CTResources.LoadResString(StringTable.IDS_XrayTube);
            }
			//12331:回転選択/12219:X線管 v9.0変更 by 間々田 2004/02/06
			//lblItemRot(4).Caption = GetResString(IDS_Move, LoadResString(12160))       'Ｘ軸移動
            lblItemRot[4].Text = StringTable.GetResString(StringTable.IDS_Move, CTSettings.AxisName[1]);
			//v14.24変更 by 間々田 2009/03/10 コモンを使用

			lblItemRot[5].Text = strMoveTable;			//テーブル移動
			//lblItemRot(6).Caption = LoadResString(12332)                            'Ｘ線管Ｘ軸移動
            lblItemRot[6].Text = StringTable.GetResString(StringTable.IDS_XrayMove, CTSettings.AxisName[1].Replace("-axis", ""));
			//v14.24変更 by 間々田 2009/03/10 コモンを使用 英語の場合は文字が長くなるので一部省略
			//lblItemRot(7).Caption = LoadResString(12333)                            'Ｘ線管Ｙ軸移動
            lblItemRot[7].Text = StringTable.GetResString(StringTable.IDS_XrayMove, CTSettings.AxisName[0].Replace("-axis", ""));
			//v14.24変更 by 間々田 2009/03/10 コモンを使用 英語の場合は文字が長くなるので一部省略
			lblItemRot[8].Text = strMoveII;			//I.I.移動

			//昇降位置(mm)=Table Up/Down position(mm)  長いので "Table " の部分を削除、"position"→"pos."にする
			lblItemRot[1].Text = lblItemRot[1].Text.Replace("Table ", "");
			lblItemRot[1].Text = lblItemRot[1].Text.Replace("position", "pos.");

			//詳細表示：オフセット校正
			//    lblItemOff(0).Caption = LoadResString(IDS_Date)                         '年月日

			//詳細表示：寸法校正
			//lblItemDist(1).Caption = LoadResString(12218)                          'X線管
            if (CTSettings.scaninh.Data.rotate_select == 0)
            {
                lblItemDist[1].Text = CTResources.LoadResString(12331);
            }
            else
            {
                lblItemDist[1].Text = CTResources.LoadResString(StringTable.IDS_XrayTube);
            }
            //12331:回転選択/12219:X線管 v9.0変更 by 間々田 2004/02/06
			//lblItemDist(2).Caption = GetResString(IDS_Move, LoadResString(12160))   'Ｘ軸移動
			lblItemDist[2].Text = StringTable.GetResString(StringTable.IDS_Move, CTSettings.AxisName[1]);
			//v14.24変更 by 間々田 2009/03/10 コモンを使用
			lblItemDist[3].Text = strMoveTable;
			//テーブル移動   'v10.03削除 by 間々田 2005/06/08 'v11.2復活 by 間々田 2006/01/11
			//lblItemDist(4).Caption = LoadResString(12332)                               'Ｘ線管Ｘ軸移動
            lblItemDist[4].Text = StringTable.GetResString(StringTable.IDS_XrayMove, CTSettings.AxisName[1].Replace("-axis", ""));
			//v14.24変更 by 間々田 2009/03/10 コモンを使用 英語の場合は文字が長くなるので一部省略
			//lblItemDist(5).Caption = LoadResString(12333)                               'Ｘ線管Ｙ軸移動
            lblItemDist[5].Text = StringTable.GetResString(StringTable.IDS_XrayMove, CTSettings.AxisName[0].Replace("-axis", ""));
			//v14.24変更 by 間々田 2009/03/10 コモンを使用 英語の場合は文字が長くなるので一部省略
			lblItemDist[6].Text = strMoveII;
			//I.I.移動

			//英語環境の場合
			if (modCT30K.IsEnglish)
            {
				//（実行時／現在値）を注釈コメントの形で表示する
				lblComment.Visible = true;

				//ラベルコントロールに使用するフォントをArialにする
				//SetLabelFont Me
			}
            //日本語環境の場合
            else
            {
                fraRotate.Text = fraRotate.Text + CTResources.LoadResString(9323);        //回転中心校正（実行時／現在値）
                fraVertical.Text = fraVertical.Text + CTResources.LoadResString(9323);    //幾何歪校正（実行時／現在値）
                fraScanPosi.Text = fraScanPosi.Text + CTResources.LoadResString(9323);    //スキャン位置校正（実行時／現在値）
                fraGain.Text = fraGain.Text + CTResources.LoadResString(9323);            //ゲイン校正（実行時／現在値）
                fraOffset.Text = fraOffset.Text + CTResources.LoadResString(9323);        //オフセット校正（実行時／現在値）
                fraDistance.Text = fraDistance.Text + CTResources.LoadResString(9323);    //寸法校正（実行時／現在値）
		
            }
		}

        #endregion

        #region 校正ステータス更新処理
        
        //*******************************************************************************
        //機　　能： 校正ステータス更新処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.2  2006/01/11 (SI3)間々田   新規作成
        //*******************************************************************************
		public void MyUpdate()
		{
			string buf;

            if (modSeqComm.MySeq == null)
            {
                return;
            }

			//自動スキャン位置校正時のI.I.移動時には校正ステータス画面を更新しない
            if (modScanCorrectNew.IIMovedAtAutoSpCorrect)
            {
                return;
            }

			//■詳細情報を表示する

			//I.I.視野（I.I.視野切替が可能な場合）
			if (CTSettings.scaninh.Data.iifield == 0)
            {
				buf = strSeparator + modCT30K.GetIIStr(modSeqComm.GetIINo());

                lblStatusGain[0].Text = modCT30K.GetIIStr(CTSettings.mecainf.Data.gain_iifield) + buf;   //ゲイン校正
                //追加2014/10/07hata_v19.51反映
                //if (CTSettings.DetShiftOn)  //      'v18.00追加 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
                if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)  //      'v18.00追加 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                {
                    lblStatusGainShift[0].Text = modCT30K.GetIIStr(CTSettings.mecainf.Data.gain_iifield_sft) + buf;     //'ゲイン校正(シフト用)
                }
 
                lblStatusSp[0].Text = modCT30K.GetIIStr(CTSettings.mecainf.Data.sp_iifield) + buf;       //スキャン位置校正
                lblStatusVer[0].Text = modCT30K.GetIIStr(CTSettings.mecainf.Data.ver_iifield) + buf;     //幾何歪校正
                lblStatusRot[2].Text = modCT30K.GetIIStr(CTSettings.mecainf.Data.rc_iifield) + buf;      //回転中心校正
				//lblStatusDist(0).Caption = GetIIStr(.dc_iifield) & buf              '寸法校正  'v17.00削除 byやまおか 2010/03/02
            }

            #region コメントアウト //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //Rev23.10 復活 by長野　2015/10/05
            //Ｘ線管（Ｘ線管切替が可能な場合）
            if (CTSettings.scaninh.Data.multi_tube == 0)
            {
                buf = strSeparator + modCT30K.GetXrayStr(CTSettings.scansel.Data.multi_tube);
                lblStatusGain[3].Text = modCT30K.GetXrayStr(CTSettings.mecainf.Data.gain_mt) + buf; //'ゲイン校正
                //追加2014/10/07hata_v19.51反映
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
                //if (CTSettings.DetShiftOn)  //      'v18.00追加 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                {
                    lblStatusGainShift[3].Text = modCT30K.GetXrayStr(CTSettings.mecainf.Data.gain_mt_sft) + buf;     //'ゲイン校正(シフト用)
                }
                lblStatusSp[1].Text = modCT30K.GetXrayStr(CTSettings.mecainf.Data.sp_mt) + buf;     //'スキャン位置校正
                lblStatusVer[1].Text = modCT30K.GetXrayStr(CTSettings.mecainf.Data.ver_mt) + buf;   //'幾何歪校正
                lblStatusRot[3].Text = modCT30K.GetXrayStr(CTSettings.mecainf.Data.rc_mt) + buf;    //'回転中心校正
                //            'lblStatusDist(1).Caption = GetXrayStr(.dc_mt) & buf                 '寸法校正  'v17.00削除 byやまおか 2010/03/02

            //'回転選択が可能な場合   'v9.0 added by 間々田 2004/02/06
            }
            else if (CTSettings.scaninh.Data.rotate_select == 0)
            {
                //'現在の回転選択値
                buf = strSeparator + modCT30K.GetRotateStr(CTSettings.scansel.Data.rotate_select);

                lblStatusRot[3].Text = modCT30K.GetRotateStr(CTSettings.mecainf.Data.rc_rs) + buf;  //'回転中心校正回転選択ステータス
                //            'lblStatusDist(1).Caption = GetRotateStr(.dc_rs) & buf               '寸法校正回転選択ステータス    'v17.00削除 byやまおか 2010/03/02

                lblStatusRot[6].Text = strMove[(modSeqComm.MySeq.stsRotXrayXCh ? 1 : 0)];   //'回転中心校正Ｘ線管Ｘ軸移動
                lblStatusRot[7].Text = strMove[(modSeqComm.MySeq.stsRotXrayYCh ? 1 : 0)];   //'回転中心校正Ｘ線管Ｙ軸移動
                //            'lblStatusDist(4).Caption = strMove(IIf(MySeq.stsDisXrayXCh, 1, 0))  '寸法校正Ｘ線管Ｘ軸移動    'v17.00削除 byやまおか 2010/03/02
                //            'lblStatusDist(5).Caption = strMove(IIf(MySeq.stsDisXrayYCh, 1, 0))  '寸法校正Ｘ線管Ｙ軸移動    'v17.00削除 byやまおか 2010/03/02
            }
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            #endregion

            //管電圧・管電流（Ｘ線外部制御可能な場合）
			if (CTSettings.scaninh.Data.xray_remote == 0)
            {
				//管電圧(KV)：小数点以下0桁目まで有効
				//buf = strSeparator & Format$(scansel.scan_kv, "0")
				buf = strSeparator + frmXrayControl.Instance.ntbSetVolt.Value.ToString("0");
                lblStatusGain[1].Text = CTSettings.mecainf.Data.gain_kv.ToString("0") + buf;     //ゲイン校正管電圧

                //追加2014/10/07hata_v19.51反映
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
                //if (CTSettings.DetShiftOn)  //      'v18.00追加 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)  //      'v18.00追加 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                {                
                    lblStatusGainShift[1].Text = CTSettings.mecainf.Data.gain_kv_sft.ToString("0") + buf; //'ゲイン校正管電圧(シフト用)
                }
                
                lblStatusRot[0].Text = CTSettings.mecainf.Data.rc_kv.ToString("0") + buf;        //回転中心校正管電圧
				
				//v11.2削除ここから by 間々田 2006/01/31 ゲインの管電流の項目を削除
				//'管電流(μA)：小数点以下0桁目まで有効
				//buf = strSeparator & Format$(scansel.scan_ma, "0")
				//
				//lblStatusGain(2).Caption = Format$(.gain_ma, "0") & buf             'ゲイン校正管電流
				//v11.2削除ここまで by 間々田 2006/01/31 ゲインの管電流の項目を削除
            }

			//ゲイン校正フィルタ（フィルタが可能な場合）
			if (CTSettings.scaninh.Data.filter == 0)
            {
				//現在のフィルタの文字列
				buf = strSeparator + modCT30K.GetFilterStr(modSeqComm.GetFilterIndex());
                                
                lblStatusGain[4].Text = modCT30K.GetFilterStr(CTSettings.mecainf.Data.gain_filter) + buf;    //ゲイン校正用

                //追加2014/10/07hata_v19.51反映
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
                //if (CTSettings.DetShiftOn)  //      'v18.00追加 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                {
                    lblStatusGainShift[4].Text = modCT30K.GetFilterStr(CTSettings.mecainf.Data.gain_filter_sft) + buf; //ゲイン校正用(シフト用)
                }
            }

			//回転中心校正昇降位置：小数点以下3桁目まで有効
            //lblStatusRot[1].Text = CTSettings.mecainf.Data.rc_udab_pos.ToString("0.000") + strSeparator + CTSettings.mecainf.Data.udab_pos.ToString("0.000");
            //Rev23.10 計測CT対応 by長野 2015/10/16
            lblStatusRot[1].Text = CTSettings.mecainf.Data.rc_udab_pos.ToString("0.000") + strSeparator + frmMechaControl.Instance.Udab_Pos.ToString("0.000");

			//オフセット校正年月日
            //変更2014/10/07hata_v19.51反映
            //lblStatusOff[0].Text = Convert.ToString(CTSettings.mecainf.Data.off_date) + strSeparator + DateTime.Now.ToString("yyyyMMdd");
            //lblStatusOff[0].Text = CTSettings.mecainf.Data.off_date.ToString("00000000") + strSeparator + DateTime.Now.ToString("YYYYMMDD");            //'v18.00変更 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev20.00 yyyyMMddに変更 by長野 2014/11/04
            lblStatusOff[0].Text = CTSettings.mecainf.Data.off_date.ToString("00000000") + strSeparator + DateTime.Now.ToString("yyyyMMdd");            //'v18.00変更 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

			//ゲイン校正年月日               '12.01追加 by 間々田 2006/12/04
            //変更2014/10/07hata_v19.51反映
            //lblStatusGain[2].Text = Convert.ToString(CTSettings.mecainf.Data.gain_date) + strSeparator + DateTime.Now.ToString("yyyyMMdd");
            //lblStatusGain[2].Text = CTSettings.mecainf.Data.gain_date.ToString("00000000") + strSeparator + DateTime.Now.ToString("YYYYMMDD");            //'v18.00変更 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev20.00 yyyyMMddに変更 by長野 2014/11/04           
            lblStatusGain[2].Text = CTSettings.mecainf.Data.gain_date.ToString("00000000") + strSeparator + DateTime.Now.ToString("yyyyMMdd");            //'v18.00変更 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //追加2014/10/07hata_v19.51反映
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //if (CTSettings.DetShiftOn)  //      'v18.00追加 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
            {
                //'lblStatusGainShift(2).Caption = CStr(.gain_date_sft) & strSeparator & Format$(Now, "YYYYMMDD")  '(シフト用)
                //lblStatusGainShift[2].Text = CTSettings.mecainf.Data.gain_date_sft.ToString("00000000") + strSeparator + DateTime.Now.ToString("YYYYMMDD"); //'(シフト用)
                //Rev20.00 yyyyMMddに変更 by長野 2014/11/04
                lblStatusGainShift[2].Text = CTSettings.mecainf.Data.gain_date_sft.ToString("00000000") + strSeparator + DateTime.Now.ToString("yyyyMMdd"); //'(シフト用)
            }
 
            //PkeFPDの場合はオフセット校正実行時刻を表示   'v17.00追加 byやまおか 2010/03/04  'v17.00修正 byやまおか 2010/07/12
			//If DetType = DetTypePke Then lblStatusGain(9).Caption = Format$(.off_time, "000000") & strSeparator & Format$(.gain_time, "000000")
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
                lblStatusGain[9].Text = CTSettings.mecainf.Data.gain_time.ToString("000000") + strSeparator + CTSettings.mecainf.Data.off_time.ToString("000000");
                //追加2014/10/07hata_v19.51反映
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
                //if (CTSettings.DetShiftOn)  //      'v18.00追加 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                {
                    lblStatusGainShift[9].Text = CTSettings.mecainf.Data.gain_time_sft.ToString("000000") + strSeparator + CTSettings.mecainf.Data.off_time.ToString("000000"); //'(シフト用)
                }
            }

			//寸法校正 実行年月日時刻     'v17.00追加 byやまおか 2010/03/04
            lblStatusDist[8].Text = CTSettings.mecainf.Data.dc_date.ToString("00000000") + " " + CTSettings.mecainf.Data.dc_time.ToString("000000");
			//v17.02 dc_dateの桁数変更 byやまおか 2010/07/08

			//スキャン位置校正 実行年月日時刻     'v17.02追加 byやまおか 2010/07/08
            lblStatusSp[4].Text = CTSettings.mecainf.Data.sp_date.ToString("00000000") + " " + CTSettings.mecainf.Data.sp_time.ToString("000000");

			//テーブルＸ移動・テーブルＹ移動
			lblStatusRot[4].Text = strMove[(modSeqComm.MySeq.stsRotXChange ? 1 : 0)];   //回転中心校正テーブルＸ移動
			lblStatusRot[5].Text = strMove[(modSeqComm.MySeq.stsRotYChange ? 1 : 0)];   //回転中心校正テーブルＹ移動
			
            //Rev23.20 復活 by長野 2016/01/24
            //lblStatusDist(2).Caption = strMove(IIf(MySeq.stsDisXChange, 1, 0))      '寸法校正テーブルＸ移動    'v17.00削除 byやまおか 2010/03/02
			//lblStatusDist(3).Caption = strMove(IIf(MySeq.stsDisYChange, 1, 0))      '寸法校正テーブルＹ移動 'v10.03削除 by 間々田 2005/06/08  'v11.2復活 by 間々田 2006/01/11  'v17.00削除 byやまおか 2010/03/02
            lblStatusDist[2].Text = strMove[(modSeqComm.MySeq.stsDisXChange ? 1 : 0)];
            lblStatusDist[3].Text = strMove[(modSeqComm.MySeq.stsDisYChange ? 1 : 0)];

			//I.I.移動
			lblStatusGain[5].Text = strMove[(modSeqComm.MySeq.stsGainIIChange ? 1 : 0)];    //ゲイン校正
            //追加2014/10/07hata_v19.51反映
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //if (CTSettings.DetShiftOn)  //      'v18.00追加 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
            {
                lblStatusGainShift[5].Text = strMove[(modSeqComm.MySeq.stsGainIIChange ? 1 : 0)];
            }
            
            lblStatusSp[2].Text = strMove[(modSeqComm.MySeq.stsSPIIChange ? 1 : 0)];        //スキャン位置校正   'v11.2追加 by 間々田 2005/10/07
			lblStatusVer[2].Text = strMove[(modSeqComm.MySeq.stsVerIIChange ? 1 : 0)];      //幾何歪校正
			lblStatusRot[8].Text = strMove[(modSeqComm.MySeq.stsRotIIChange ? 1 : 0)];      //回転中心校正
			//lblStatusDist(6).Caption = strMove(IIf(MySeq.stsDisIIChange, 1, 0))     '寸法校正  'v17.00削除 byやまおか 2010/03/02

            //変更2014/10/07hata_v19.51反映
            //'ビニングモード（ビニングが使用可能の場合）   v7.0 フラットパネル対応 by 間々田 2003/08/21
            if (CTSettings.scaninh.Data.binning == 0)
            {
                buf = strSeparator + modCT30K.GetBinningStr(CTSettings.scansel.Data.binning);

                lblStatusGain[6].Text = modCT30K.GetBinningStr(CTSettings.mecainf.Data.gain_bin) + buf; //'ゲイン校正
                //追加2014/10/07hata_v19.51反映
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
                //if (CTSettings.DetShiftOn)  //      'v18.00追加 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                {
                    lblStatusGainShift[6].Text = modCT30K.GetBinningStr(CTSettings.mecainf.Data.gain_bin_sft) + buf;  //'ゲイン校正(シフト用)
                }
                lblStatusSp[3].Text = modCT30K.GetBinningStr(CTSettings.mecainf.Data.sp_bin) + buf;     //'スキャン位置校正
                lblStatusVer[3].Text = modCT30K.GetBinningStr(CTSettings.mecainf.Data.ver_bin) + buf;   //'幾何歪校正
                //追加2014/10/07hata_v19.51反映
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
                //if (CTSettings.DetShiftOn)  //      'v18.00追加 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                {
                    buf = strSeparator + modCT30K.GetFpdGainStr(CTSettings.scansel.Data.fpd_gain,CTSettings.t20kinf.Data.pki_fpd_type);                            //FPDゲイン(現在値)(シフト用)
                    lblStatusGainShift[7].Text = modCT30K.GetFpdGainStr(CTSettings.mecainf.Data.gain_fpd_gain_sft, CTSettings.t20kinf.Data.pki_fpd_type) + buf;     //FPDゲイン(ゲイン校正を行ったとき)(シフト用)
                    buf = strSeparator + modCT30K.GetFpdIntegStr(CTSettings.scansel.Data.fpd_integ);                          //FPD積分時間(現在値)
                    lblStatusGainShift[8].Text = modCT30K.GetFpdIntegStr(CTSettings.mecainf.Data.gain_fpd_integ_sft) + buf;   //FPD積分時間(オフセット校正を行ったとき)シフト用)
                }
            }

            //フラットパネル設定(ゲイン/積分時間)が可能な場合    'v17.00追加(ここから) byやまおか 2010/02/17
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
                buf = strSeparator + modCT30K.GetFpdGainStr(CTSettings.scansel.Data.fpd_gain, CTSettings.t20kinf.Data.pki_fpd_type);                   //FPDゲイン(現在値)
                lblStatusGain[7].Text = modCT30K.GetFpdGainStr(CTSettings.mecainf.Data.gain_fpd_gain, CTSettings.t20kinf.Data.pki_fpd_type) + buf;     //FPDゲイン(ゲイン校正を行ったとき)
                lblStatusOff[2].Text = modCT30K.GetFpdGainStr(CTSettings.mecainf.Data.off_fpd_gain, CTSettings.t20kinf.Data.pki_fpd_type) + buf;       //FPDゲイン(オフセット校正を行ったとき)
				buf = strSeparator + modCT30K.GetFpdIntegStr(CTSettings.scansel.Data.fpd_integ);                 //FPD積分時間(現在値)
                lblStatusGain[8].Text = modCT30K.GetFpdIntegStr(CTSettings.mecainf.Data.gain_fpd_integ) + buf;   //FPD積分時間(ゲイン校正を行ったとき)
                lblStatusOff[3].Text = modCT30K.GetFpdIntegStr(CTSettings.mecainf.Data.off_fpd_integ) + buf;     //FPD積分時間(オフセット校正を行ったとき)

                //追加2014/10/07hata_v19.51反映
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
                //if (CTSettings.DetShiftOn)  //      'v18.00追加 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                {
                    buf = strSeparator + modCT30K.GetFpdGainStr(CTSettings.scansel.Data.fpd_gain, CTSettings.t20kinf.Data.pki_fpd_type);                              //FPDゲイン(現在値)(シフト用)
                    lblStatusGainShift[7].Text = modCT30K.GetFpdGainStr(CTSettings.mecainf.Data.gain_fpd_gain_sft, CTSettings.t20kinf.Data.pki_fpd_type) + buf;       //FPDゲイン(ゲイン校正を行ったとき)(シフト用)
                    buf = strSeparator + modCT30K.GetFpdIntegStr(CTSettings.scansel.Data.fpd_integ);                            //FPD積分時間(現在値)
                    lblStatusGainShift[8].Text = modCT30K.GetFpdIntegStr(CTSettings.mecainf.Data.gain_fpd_integ_sft) + buf;     //FPD積分時間(オフセット校正を行ったとき)シフト用)
                }
            }
            //v17.00追加(ここから) byやまおか 2010/02/17

            //追加2014/10/07hata_v19.51反映
            //'焦点切替   'v18.00追加 byやまおか 2011/06/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //if ((CTSettings.scaninh.Data.focus_change == 0) & (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan))
            //Rev25.03 change by chouno 2017/02/05
            if ((CTSettings.scaninh.Data.focus_change == 0) & ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman)))
            {
                buf = strSeparator + modCT30K.GetFocusStr(CTSettings.mecainf.Data.xfocus);                  //'焦点(現在値)
                lblStatusGain[10].Text = modCT30K.GetFocusStr(CTSettings.mecainf.Data.gain_focus) + buf;    //'焦点(ゲイン校正を行ったとき)
                buf = strSeparator + modCT30K.GetFocusStr(CTSettings.mecainf.Data.xfocus);                  //'焦点(現在値)
                lblStatusRot[10].Text = modCT30K.GetFocusStr(CTSettings.mecainf.Data.rc_focus) + buf;       //'焦点(回転中心校正を行ったとき)
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
                //if (CTSettings.DetShiftOn)  //      'v18.00追加 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                {
                    buf = strSeparator + modCT30K.GetFocusStr(CTSettings.mecainf.Data.xfocus);                          //'焦点(現在値)
                    lblStatusGainShift[10].Text = modCT30K.GetFocusStr(CTSettings.mecainf.Data.gain_focus_sft) + buf;   //'焦点(ゲイン校正を行ったとき)
                }
            }

            #region コメントアウト

            //v11.5追加ここから by 間々田 frmStatusから移動してきた

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//自動テーブル移動ステータスの設定（自動テーブル移動ありの場合）
			//    If (scaninh.table_auto_move = 0) Then
			//
			//        Static LastXpos As Long
			//        Static LastFCD  As Long
			//        Static NotFirst As Boolean
			//
			//        With MySeq
			//
			//            '起動時のみ行なう処理
			//            If Not NotFirst Then
			//
			//                NotFirst = True
			//
			//                '回転中心校正ステータスの変化の要因が自動テーブル移動によるものかどうかを判別するためのフラグ。初期値はFalse
			//                byTableAutoMove = False
			//
			//                '自動テーブル移動ステータスが移動完了となっている場合
			//                If mecainf.table_auto_move = IIf(scansel.scan_mode = ScanModeOffset, 1, 0) Then
			//
			//                   '回転中心校正を要する座標の変化がある場合
			//                    If .stsRotXChange Or .stsRotYChange Then
			//
			//                        '自動テーブル移動座標をコモンから取得
			//                        Dim x   As Long
			//                        Dim y   As Long
			//                        x = Fix(100 * IIf(scansel.scan_mode = ScanModeOffset, mecainf.auto_move_xo, mecainf.auto_move_xf))
			//                        y = Fix(10 * IIf(scansel.scan_mode = ScanModeOffset, mecainf.auto_move_yo, mecainf.auto_move_yf))
			//
			//                        '現在の位置が自動テーブル移動座標と同一となっている場合→それは自動テーブル移動によるものと判断する
			//                        If (x = .stsXPosition) And (y = .stsFCD) Then
			//                             byTableAutoMove = True
			//                        End If
			//
			//                    End If
			//
			//                End If
			//
			//            End If
			//
			//            '回転中心校正未完了の要因がテーブル移動によるもので、かつ自動テーブル移動の時
			//            Dim i As Integer
			//            Dim NG As Boolean
			//            NG = False
			//            For i = lblItemRot.LBound To lblItemRot.UBound
			//                If (lblItemRot(i).BackColor = vbYellow) And (i <> 4) And (i <> 5) Then
			//                    NG = True   '未完了の要因がテーブル移動以外の理由
			//                    Exit For
			//                End If
			//            Next
			//
			//            '未完了の要因がテーブル移動だけの場合、それが自動テーブル移動によるものなら「移動不可」とはしない
			//            If Not NG Then
			//                If byTableAutoMove Or (.stsFCD = LastFCD And .stsXPosition = LastXpos) Then
			//                    byTableAutoMove = False '自動テーブル移動要因フラグクリア
			//                Else
			//                    NG = True
			//                End If
			//            End If
			//
			//            '自動テーブル移動ステータスを３（移動不可）にする
			//            If NG Then
			//                If SetCommonLong("mecainf", "table_auto_move", 3) Then
			//                    If IsExistForm(frmTableAutoMove) Then
			//                        If frmTableAutoMove.MousePointer = vbDefault Then frmTableAutoMove.Update   '移動中は更新しない
			//                    End If
			//                End If
			//            End If
			//
			//            LastXpos = .stsXPosition
			//            LastFCD = .stsFCD
			//
			//        End With
			//
			//    End If
			//v11.5追加ここまで by 間々田
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            #endregion
        }


        #endregion

        #region ゲイン校正フレーム内ステータス（詳細）変更時処理

        //*******************************************************************************
        //機　　能： ゲイン校正フレーム内ステータス（詳細）変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： ステータス文字列に基づき，表示色を設定します
        //
        //履　　歴： v11.2  2005/12/28 (SI3)間々田   新規作成
        //*******************************************************************************
        private void lblStatusGain_Change(object sender, EventArgs e)
		{
            int Index = -1;
            for (int i = 0; i < lblStatusGain.Length; i++)
            {
                if (sender.Equals(lblStatusGain[i]))
                {
                    Index = i;
                    break;
                }
            }
            if (Index < 0)
            {
                return;
            }

			//変更された項目の表示色を更新
			//lblItemGain(Index).BackColor = GetBackColor(lblStatusGain(Index).Caption)
			//lblItemGain(Index).BackColor = GetBackColor(lblStatusGain(Index).Caption, (Index = 4)) 'v11.21変更 by 間々田 2006/02/15 フィルタに関しては - / - の状態でも完了とみなすための措置
			switch (Index)  //v17.00変更(ここから) byやまおか 2010/03/04
            {
				//（区切り無し）
				case 9:
					//オフセット校正時間
					//lblItemGain(Index).BackColor = GetBackColor_ChckAfter(CStr(mecainf.gain_date) & CStr(mecainf.gain_time), CStr(mecainf.off_date) & CStr(mecainf.off_time)) '年月日時間を比較する
					lblItemGain[Index].BackColor = GetBackColor_ChckAfter(Convert.ToString(CTSettings.mecainf.Data.gain_date) + Convert.ToString(CTSettings.mecainf.Data.gain_time), 
                                                                          Convert.ToString(CTSettings.mecainf.Data.off_date) + Convert.ToString(CTSettings.mecainf.Data.off_time));   //年月日時間を比較する
					break;
				//（実行値／現在値）
				default:
					lblItemGain[Index].BackColor = GetBackColor(lblStatusGain[Index].Text, Index == 4);//フィルタに関しては - / - の状態でも完了とみなすための措置
					break;
            }//v17.00変更(ここまで) byやまおか 2010/03/04

            //Debug.Print(this.Visible.ToString());
            //Debug.Print(lblItemGain[Index].Visible.ToString());
            
            //ゲイン校正ステータス（簡易表示）の更新
			UpdateStatus(lblItemGain, ref frmScanControl.Instance.lblStatus[0]);

            //追加2014/10/07hata_v19.51反映
            //v18.00変更 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev25.00 Wスキャンを追加 by長野 2016/07/07
			if ((CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift) || (CTSettings.scansel.Data.w_scan == 1)) {
				UpdateStatus(lblItemGain, ref frmScanControl.Instance.lblStatus[0], "" , lblItemGainShift);
			} else {
				UpdateStatus(lblItemGain,ref frmScanControl.Instance.lblStatus[0]);
			}
        }

        #endregion

        #region ゲイン校正フレーム(シフト用)内ステータス（詳細）変更時処理

        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから
        //*******************************************************************************
        //機　　能： ゲイン校正フレーム(シフト用)内ステータス（詳細）変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： ステータス文字列に基づき，表示色を設定します
        //
        //履　　歴： v18.00  2011/02/11  やまおか    新規作成
        //*******************************************************************************
        private void lblStatusGainShift_Change(object sender, EventArgs e)
        {
            int Index = -1;
            for (int i = 0; i < lblStatusGainShift.Length; i++)
            {
                //if (sender.Equals(lblStatusGain[i]))
                if (sender.Equals(lblStatusGainShift[i]))
                {
                    Index = i;
                    break;
                }
            }
            if (Index < 0)
            {
                return;
            }

			//変更された項目の表示色を更新
			switch (Index) {

				//（区切り無し）
				case 9:
					//オフセット校正時間
					lblItemGainShift[Index].BackColor = GetBackColor_ChckAfter(Convert.ToString(CTSettings.mecainf.Data.gain_date_sft) + Convert.ToString(CTSettings.mecainf.Data.gain_time_sft), Convert.ToString(CTSettings.mecainf.Data.off_date) + Convert.ToString(CTSettings.mecainf.Data.off_time));
					//年月日時間を比較する
					break;

				//（実行値／現在値）
				default:
					lblItemGainShift[Index].BackColor = GetBackColor(lblStatusGainShift[Index].Text, Index == 4);
					//フィルタに関しては - / - の状態でも完了とみなすための措置
					break;

			}

			//ゲイン校正ステータス（簡易表示）の更新 （シフトスキャンのときだけ）
            //Rev25.00 Wスキャンを追加 by長野 2016/07/07
			if ((CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift) || (CTSettings.scansel.Data.w_scan == 1)) {
				UpdateStatus(lblItemGainShift, ref frmScanControl.Instance.lblStatus[0],"" , lblItemGain);
			}

		}
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここまで
        #endregion

        #region スキャン位置校正フレーム内ステータス（詳細）変更時処理

        //*******************************************************************************
        //機　　能： スキャン位置校正フレーム内ステータス（詳細）変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： ステータス文字列に基づき，表示色を設定します
        //
        //履　　歴： v11.2  2005/12/28 (SI3)間々田   新規作成
        //*******************************************************************************
        private void lblStatusSp_Change(object sender, EventArgs e)
		{
            int Index = -1;
            for (int i = 0; i < lblStatusSp.Length; i++)
            {
                if (sender.Equals(lblStatusSp[i]))
                {
                    Index = i;
                    break;
                }
            }
            if (Index < 0)
            {
                return;
            }

			//変更された項目の表示色を更新
			lblItemSp[Index].BackColor = GetBackColor(lblStatusSp[Index].Text);

			//スキャン位置校正ステータス（簡易表示）の更新（チェック対象のときだけ更新する）
			UpdateStatus(lblItemSp, ref frmScanControl.Instance.lblStatus[1], 
                         (frmScanControl.Instance.chkInhibit[1].CheckState == CheckState.Unchecked ? StringTable.GC_STS_IGNORE : ""));
		}

        #endregion

        #region 幾何歪校正フレーム内ステータス（詳細）変更時処理

        //*******************************************************************************
        //機　　能： 幾何歪校正フレーム内ステータス（詳細）変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： ステータス文字列に基づき，表示色を設定します
        //
        //履　　歴： v11.2  2005/12/28 (SI3)間々田   新規作成
        //*******************************************************************************
        private void lblStatusVer_Change(object sender, EventArgs e)
		{
            int Index = -1;
            for (int i = 0; i < lblStatusVer.Length; i++)
            {
                if (sender.Equals(lblStatusVer[i]))
                {
                    Index = i;
                    break;
                }
            }
            if (Index < 0)
            {
                return;
            }

			//変更された項目の表示色を更新
			lblItemVer[Index].BackColor = GetBackColor(lblStatusVer[Index].Text);

			//幾何歪校正ステータス（簡易表示）の更新
			UpdateStatus(lblItemVer,ref frmScanControl.Instance.lblStatus[2]);
		}

        #endregion

        #region 回転中心校正フレーム内ステータス（詳細）変更時処理

        //*******************************************************************************
        //機　　能： 回転中心校正フレーム内ステータス（詳細）変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： ステータス文字列に基づき，表示色を設定します
        //
        //履　　歴： v11.2  2005/12/28 (SI3)間々田   新規作成
        //*******************************************************************************
        private void lblStatusRot_Change(object sender, EventArgs e)
		{
            int Index = -1;
            for (int i = 0; i < lblStatusRot.Length; i++)
            {
                if (sender.Equals(lblStatusRot[i]))
                {
                    Index = i;
                    break;
                }
            }
            if (Index < 0)
            {
                return;
            }

			//変更された項目の表示色を更新
			lblItemRot[Index].BackColor = GetBackColor(lblStatusRot[Index].Text);

			//Ｘ軸移動, テーブル移動, Ｘ線管Ｘ軸移動, Ｘ線管Ｙ軸移動, I.I.移動があった場合   '追加 by 間々田 2006/01/20
			if (lblStatusRot[Index].Text == strMove[1])
            {
				CTSettings.mecainf.Data.normal_rc_cor = 0;
				CTSettings.mecainf.Data.cone_rc_cor = 0;
				//modMecainf.PutMecainf(ref CTSettings.mecainf.Data);
                CTSettings.mecainf.Put(CTSettings.mecainf.Data);
			}

			//回転中心校正ステータス（簡易表示）の更新
			UpdateStatus(lblItemRot, ref frmScanControl.Instance.lblStatus[3], 
                         (frmScanControl.Instance.chkInhibit[3].CheckState == CheckState.Unchecked ? StringTable.GC_STS_AutoCentering : ""));
		}

        #endregion

        #region オフセット校正フレーム内ステータス（詳細）変更時処理

        //*******************************************************************************
        //機　　能： オフセット校正フレーム内ステータス（詳細）変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： ステータス文字列に基づき，表示色を設定します
        //
        //履　　歴： v11.2  2005/12/28 (SI3)間々田   新規作成
        //*******************************************************************************
        private void lblStatusOff_Change(object sender, EventArgs e)
		{
            int Index = -1;
            for (int i = 0; i < lblStatusOff.Length; i++)
            {
                if (sender.Equals(lblStatusOff[i]))
                {
                    Index = i;
                    break;
                }
            }
            if (Index < 0)
            {
                return;
            }

			//変更された項目の表示色を更新
			lblItemOff[Index].BackColor = GetBackColor(lblStatusOff[Index].Text);

			//オフセット校正ステータス（簡易表示）の更新
			UpdateStatus(lblItemOff,ref frmScanControl.Instance.lblStatus[4]);
		}

        #endregion

        #region 寸法校正フレーム内ステータス（詳細）変更時処理

        //*******************************************************************************
        //機　　能： 寸法校正フレーム内ステータス（詳細）変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： ステータス文字列に基づき，表示色を設定します
        //
        //履　　歴： v11.2  2005/12/28 (SI3)間々田   新規作成
        //*******************************************************************************
        private void lblStatusDist_Change(object sender, EventArgs e)
		{
            int Index = -1;
            for (int i = 0; i < lblStatusDist.Length; i++)
            {
                if (sender.Equals(lblStatusDist[i]))
                {
                    Index = i;
                    break;
                }
            }
            if (Index < 0)
            {
                return;
            }

			//変更された項目の表示色を更新
			lblItemDist[Index].BackColor = GetBackColor(lblStatusDist[Index].Text);
			//v17.00元に戻した byやまおか 2010/03/08

			//寸法校正ステータス（簡易表示）の更新（チェック対象のときだけ更新する）
			UpdateStatus(lblItemDist, ref frmScanControl.Instance.lblStatus[5], 
                         (frmScanControl.Instance.chkInhibit[5].CheckState == CheckState.Unchecked ? StringTable.GC_STS_IGNORE : ""));
		}

        #endregion

        #region 詳細ステータス文字列に基づき，表示色を設定します

        //*******************************************************************************
        //機　　能： 詳細ステータス文字列に基づき，表示色を設定します
        //
        //           変数名              [I/O] 型                内容
        //引　　数： theString           [I/ ] String
        //           IgnoreUnknownValue  [I/ ] Boolean           - / - のときは緑色（完了）とする場合、Trueをセットする
        //                                                       - / - のときは黄色（未完了）とする場合、Falseをセットする（デフォルト）
        //戻 り 値：                     [ /O] ColorConstants    vbYellow（黄色）：未完了
        //                                                       vbGreen（緑色） ：完了
        //補　　足： なし
        //
        //履　　歴： v11.2  2005/12/28 (SI3)間々田   新規作成
        //           v11.21 2006/02/15 (SI3)間々田   引数IgnoreUnknownValue追加
        //*******************************************************************************
        //Private Function GetBackColor(ByVal theString As String) As ColorConstants
		private Color GetBackColor(string theString, bool IgnoreUnknownValue = false)   //v11.21変更 by 間々田 2006/02/15
		{
			string[] strCell = null;
            char strChar = Convert.ToChar(strSeparator);

			//戻り値初期化
			Color functionReturnValue = Color.Yellow;

			//区切り文字で文字を抽出
            //strCell = Strings.Split(theString, strSeparator);
            strCell = theString.Split(strChar);

            if (strCell.GetUpperBound(0) > 0)
            {
				if (strCell[0] != strCell[1]) { return functionReturnValue; }   //前回値と現在値が異なる場合
	
				if (!IgnoreUnknownValue)    //v11.21追加 by 間々田 2006/02/15
                {
					if (strCell[0] == " - ")    //前回値と現在値が共に不定の場合
                    {
						return functionReturnValue;
                    }
				}   //v11.21追加 by 間々田 2006/02/15
				
				if (strCell[0] == "000000") { return functionReturnValue; } //コモン初期化後のゲイン校正"ｹﾞｲﾝ/ｵﾌｾｯﾄ校正"を未完にするため   'v17.00追加 byやまおか 2010/03/09
            }
            else
            {
				if (theString == strMove[1])
                {
					return functionReturnValue;
                }
				//移動ありの場合
				//If theString = "000000 000000" Then Exit Function   'コモン初期化後の寸法校正"年月日日時"を未完にするため   'v17.00追加 byやまおか 2010/03/09
                if (theString == "00000000 000000") { return functionReturnValue; } //桁数変更   'v17.02変更 byやまおか 2010/07/08
            }

			//戻り値セット
			functionReturnValue = Color.Lime;
			return functionReturnValue;
		}

        #endregion

        #region 詳細ステータス文字列に基づき，表示色を設定します

        //*******************************************************************************
        //機　　能： 詳細ステータス文字列に基づき，表示色を設定します
        //
        //           変数名              [I/O] 型                内容
        //引　　数： theTimeString1      [I/ ] String            比較元
        //           theTimeString2      [I/ ] String            比較相手
        //戻 り 値：                     [ /O] ColorConstants    vbYellow（黄色）：未完了
        //                                                       vbGreen（緑色） ：完了
        //補　　足： なし
        //
        //履　　歴： v17.00  2010/03/04  やまおか    新規作成
        //*******************************************************************************
		private Color GetBackColor_ChckAfter(string theTimeString1, string theTimeString2)  //v11.21変更 by 間々田 2006/02/15
		{
			double dtime1 = 0;
			double dtime2 = 0;

			//戻り値初期化(未完了)
            Color functionReturnValue = Color.Yellow;

			dtime1 = Convert.ToDouble(theTimeString1);
			dtime2 = Convert.ToDouble(theTimeString2);

			//theTimeString1よりtheTimeString2が後なら抜ける(未完了)
			//If (dtime1 <= dtime2) Then Exit Function    'v17.00変更 byやまおか 2010/03/09
            //変更2014/10/07hata_v19.51反映
            //if (dtime1 < dtime2) { return functionReturnValue; }    //v17.65修正 byやまおか 2011/11/02
            //v19.50 コモン初期化後は両引数方とも0になるため、それを考慮した条件に変更 by長野 2013/12/16
            if ((dtime1 < dtime2) | (dtime1 == 0 & dtime2 == 0))    //v17.65修正 byやまおか 2011/11/02
                return functionReturnValue;
            
			//戻り値セット(完了)
			functionReturnValue = Color.Lime;
			return functionReturnValue;
		}

        #endregion

        #region ステータス（簡易表示側）更新処理

        //*******************************************************************************
        //機　　能： ステータス（簡易表示側）更新処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //掲載
        //補　　足： 詳細ステータス文字列に基づき，表示色を設定します
        //
        //履　　歴： v11.2  2005/12/28 (SI3)間々田   新規作成
        //*******************************************************************************
        //public void UpdateStatus(object[] optLabel, ref Label TargetLabel, string strIgnore = "")
        public void UpdateStatus(object[] optLabel, ref Label TargetLabel, string strIgnore = "", object[] optLabel2 = null)//'v18.00変更 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05//変更2014/10/07hata_v19.51反映
		{
			Label theLabel = null;
			bool IsReady = false;
            
            //除外する文字列
			if (!string.IsNullOrEmpty(strIgnore))
            {
				TargetLabel.Text = strIgnore;
				return;
			}

            //要素となるステータスラベル(詳細画面)のチェック
			IsReady = true;
			foreach (Label theLabel_loopVariable in optLabel)
            {
				theLabel = theLabel_loopVariable;
				if (!theLabel.Visible)
                {
				}
                //else if (ColorTranslator.ToOle(theLabel.BackColor) == ColorTranslator.ToOle(Color.Cyan))
                else if (theLabel.BackColor == Color.Cyan)
                {
					return;
				}
                //else if (ColorTranslator.ToOle(theLabel.BackColor) == ColorTranslator.ToOle(Color.Yellow))
                else if (theLabel.BackColor == Color.Yellow)
                {
					IsReady = false;
				}
			}

            //追加2014/10/07hata_v19.51反映
            //要素となるステータスラベル(詳細画面)その２のチェック   'v18.00追加 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if ((optLabel2 != null))
            {
                //if (optLabel2 is Label)
                //{
                //    theLabel = (Label)optLabel2;
                foreach (Label theLabel_loopVariable in optLabel2)
                {
                    theLabel = theLabel_loopVariable;
				
                    if (!theLabel.Visible)
                    {
                    }
                    else if (theLabel.BackColor == Color.Cyan)
                    {
                        return;
                    }
                    else if (theLabel.BackColor == Color.Yellow)
                    {
                        IsReady = false;
                    }
                }
            }

			//ターゲットが回転中心ステータスの場合
			if (object.ReferenceEquals(TargetLabel, frmScanControl.Instance.lblStatus[3]))
            {
				if (!IsReady)
                {
					TargetLabel.Text = StringTable.GC_STS_STANDBY_NG;   //準備未完了
				}
                else if (CTSettings.mecainf.Data.normal_rc_cor == 1 & CTSettings.mecainf.Data.cone_rc_cor == 1)
                {
					TargetLabel.Text = StringTable.GC_STS_STANDBY_OK;   //準備完了
				}
                else if (CTSettings.mecainf.Data.normal_rc_cor == 1)
                {
					TargetLabel.Text = StringTable.GC_STS_NORMAL_OK;    //ﾉｰﾏﾙ準備完了
				}
                else
                {
					TargetLabel.Text = StringTable.GC_STS_CONE_OK;      //ｺｰﾝ準備完了
				}
			}
            else
            {
				TargetLabel.Text = (IsReady ? StringTable.GC_STS_STANDBY_OK : StringTable.GC_STS_STANDBY_NG);
			}
		}

        #endregion



        #region ステータス（簡易表示側）更新処理

        //*******************************************************************************
        //機　　能： ステータス（簡易表示側）更新処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //掲載
        //補　　足： 詳細ステータス文字列に基づき，表示色を設定します
        //
        //履　　歴： v11.2  2005/12/28 (SI3)間々田   新規作成
        //*******************************************************************************
		public void UpdateRCStatus()
		{
			//幾何歪校正ステータス（簡易表示）の更新
			UpdateStatus(lblItemRot, ref frmScanControl.Instance.lblStatus[3],
                         (frmScanControl.Instance.chkInhibit[3].CheckState == CheckState.Unchecked ? StringTable.GC_STS_AutoCentering : ""));
		}

        #endregion

        #region 各校正ボタン・クリック時処理

        //*************************************************************************************************
        //機　　能： 各校正ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdCorrect_Click(object sender, EventArgs e)
		{
            int Index = -1;
            for (int i = 0; i < cmdCorrect.Length; i++)
            {
                if (sender.Equals(cmdCorrect[i]))
                {
                    Index = i;
                    break;
                }
            }

            //Rev26.00 各校正完了までは、[ガイド]タブのスキャン条件・設定完了フラグは変更しない add by chouno 2017/01/16
            frmScanControl.Instance.setScanAreaAndCondIgnoreFlg(true);

			switch (Index)
            {
				//「ゲイン校正」ボタン
				//Case 1: frmGainCor.Show , frmCTMenu             'ゲイン校正フォームを表示する
				case 1:

                    //Rev26.00 add by chouno 2017/03/13
                    if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
                    {
                        return;
                    }

                    frmGainCor.Instance.ShowDialog(frmCTMenu.Instance);          //v17.00 Modal化 byやまおか 2010/02/10
                    break;

				//「スキャン位置校正」ボタン
				//Case 2: frmScanPositionEntry.Show , frmCTMenu   'スキャン位置校正フォームを表示する
				case 2:

                    //Rev26.00 add by chouno 2017/03/13
                    if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
                    {
                        return;
                    }
                    
                    frmScanPositionEntry.Instance.ShowDialog(frmCTMenu.Instance);//v17.00 Modal化 byやまおか 2010/02/10
                    break;
				
                //「幾何歪校正」ボタン
				//Case 3: frmVertical.Show , frmCTMenu            '幾何歪校正フォームを表示する
				case 3:
                    frmVertical.Instance.ShowDialog(frmCTMenu.Instance);         //v17.00 Modal化 byやまおか 2010/02/10
					break;
				
                //「回転中心校正」ボタン
				//Case 4: frmRotationCenter.Show , frmCTMenu      '回転中心校正フォームを表示する
				case 4:

                    //Rev26.00 add by chouno 2017/03/13
                    if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
                    {
                        return;
                    }

                    frmRotationCenter.Instance.ShowDialog(frmCTMenu.Instance);   //v17.00 Modal化 byやまおか 2010/02/10
                    //メモリを解放する
                    frmRotationCenter.Instance.Dispose();
                    break;
				
                //「オフセット校正」ボタン
				//Case 5: frmOffset.Show , frmCTMenu              'オフセット校正フォームを表示する
				case 5:
                    frmOffset.Instance.ShowDialog(frmCTMenu.Instance);           //v17.00 Modal化 byやまおか 2010/02/10
                    break;
				
                //「寸法校正」ボタン
				//Case 6: frmDistanceCorrect.Show , frmCTMenu     '寸法校正フォームを表示する
				case 6:
                    //Rev23.12 追加 by長野 2015/12/28--->
                    //frmDistanceCorrect.Instance.ShowDialog(frmCTMenu.Instance);  //v17.00 Modal化 byやまおか 2010/02/10
                    frmDistanceCorrectNew.Instance.ShowDialog(frmCTMenu.Instance);

                    break;
                default:
                    break;

				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
				//「マルチスライス校正」ボタン
				//        Case 7:
				//
				//            '幾何歪校正ステータスが準備完了でない場合
				//             'If Not frmScanControl.IsOkVertical() Then
				//             If (Not frmScanControl.IsOkVertical()) And (Not Use_FlatPanel) Then    'v17.00変更 byやまおか 2010/02/24
				//                 'メッセージ表示：
				//                 '   幾何歪校正が準備完了でないため、処理を中止します。
				//                 '   事前に幾何歪校正を実施してください。
				//                 MsgBox BuildResStr(IDS_CorNotReady, IDS_CorDistortion), vbCritical
				//                 Exit Sub
				//             End If
				//
				//            'frmMultiSlicePre.Show , frmCTMenu       'マルチスライス校正フォームを表示する
				//            frmMultiSlicePre.Show vbModal, frmCTMenu       'v17.00 Modal化 byやまおか 2010/02/10

				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
			}

            //Rev26.00 各校正完了までは、[ガイド]タブのスキャン条件・設定完了フラグは変更しない add by chouno 2017/01/16
            frmScanControl.Instance.setScanAreaAndCondIgnoreFlg(false);
        }

        #endregion
    }
}
