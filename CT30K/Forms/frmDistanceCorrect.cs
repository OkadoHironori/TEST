using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
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
    ///* プログラム名： frmDistanceCorrect.frm                                      */
    ///* 処理概要　　： 寸法校正                                                    */
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
    ///* V4.0        01/01/30    (ITC)    鈴山　修   ﾓｰﾀﾞﾙﾌｫｰﾑ→MDI子ﾌｫｰﾑに変更     */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
    public partial class frmDistanceCorrect : Form
    {
        private NumericUpDown[] cwne2R = new NumericUpDown[3];
        private TextBox[] txtImgFileName = new TextBox[3];
        private Button[] cmdDialog = new Button[3];

        #region インスタンスを返すプロパティ

        // frmDistanceCorrectのインスタンス
        private static frmDistanceCorrect _Instance = null;

        /// <summary>
        /// frmDistanceCorrectのインスタンスを返す
        /// </summary>
        public static frmDistanceCorrect Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmDistanceCorrect();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmDistanceCorrect()
        {
            InitializeComponent();

            #region フォームにコントロールを追加

            this.SuspendLayout();

            // cwne2R
            for (int i = 1; i < cwne2R.Length; i++)
            {
                this.cwne2R[i] = new NumericUpDown();
                this.cwne2R[i].DecimalPlaces = 4;
                this.cwne2R[i].Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 13F);
                this.cwne2R[i].Increment = new decimal(new int[] {1, 0, 0, 196608});
                this.cwne2R[i].Location = new Point(296, 72 + (i - 1) * 112);
                this.cwne2R[i].Maximum = new decimal(new int[] {32767, 0, 0, 0});
                this.cwne2R[i].Name = "cwne2R_" + i.ToString();
                this.cwne2R[i].Size = new Size(89, 25);
                this.cwne2R[i].TabIndex = i+1;
                this.cwne2R[i].TextAlign = HorizontalAlignment.Right;
                this.Controls.Add(this.cwne2R[i]);
            }

            // txtImgFileName
            for (int i = 1; i < txtImgFileName.Length; i++)
            {
                this.txtImgFileName[i] = new TextBox();
                this.txtImgFileName[i].Font = new Font("ＭＳ Ｐゴシック", 13F);
                this.txtImgFileName[i].Location = new Point(16, 40 + (i - 1) * 112);
                this.txtImgFileName[i].Name = "txtImgFileName" + i.ToString(); ;
                this.txtImgFileName[i].Size = new Size(369, 25);
                this.txtImgFileName[i].TabIndex = i+1;
                this.txtImgFileName[i].TextChanged += new EventHandler(txtImgFileName_TextChanged);
                this.Controls.Add(this.txtImgFileName[i]);
            }

            //cmdDialog
            for (int i = 1; i < cmdDialog.Length; i++)
            {
                this.cmdDialog[i] = new Button();
                this.cmdDialog[i].Font = new Font("ＭＳ Ｐゴシック", 12F);
                this.cmdDialog[i].Location = new Point(408, 40 + (i - 1) * 112);
                this.cmdDialog[i].Name = "cmdDialog" + i.ToString();
                this.cmdDialog[i].Size = new Size(33, 25);
                this.cmdDialog[i].TabIndex = i+1;
                this.cmdDialog[i].Text = ">>";
                this.cmdDialog[i].UseVisualStyleBackColor = true;
                this.cmdDialog[i].Click += new EventHandler(cmdDialog_Click);
                this.Controls.Add(this.cmdDialog[i]);
            }

            this.ResumeLayout(false);

            #endregion
        }

        //*******************************************************************************
        //機　　能： >>（参照）ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void cmdDialog_Click(object sender, EventArgs e)
		{
            int Index = -1;
            for (int i = 0; i < cmdDialog.Length; i++)
            {
                if (sender.Equals(cmdDialog[i]))
                {
                    Index = i;
                    break;
                }
            }

			//Dim FileName As String
			//FileName = GetFileName(IDS_Select, LoadResString(12105), ".img")
			//If FileName <> "" Then txtImgFileName(Index).Text = FileName

			//v16.20/v17.00変更(ここから) byやまおか 2010/03/04
			string FileName = null;
            //modImageInfo.ImageInfoStruct theInfoRec = default(modImageInfo.ImageInfoStruct);
            ImageInfo theInfoRec = new ImageInfo();
            theInfoRec.Data.Initialize();

            //FileName = modFileIO.GetFileName(StringTable.IDS_Select, CTResources.LoadResString(StringTable.IDS_Select), ".img");
            FileName = modFileIO.GetFileName(StringTable.IDS_Select, CTResources.LoadResString(12105), ".img");

			if (!string.IsNullOrEmpty(FileName))
            {
				//if (!modImageInfo.ReadImageInfo(ref theInfoRec, modLibrary.RemoveExtension(FileName, ".img")))
                //if (ImageInfo.ReadImageInfo(ref theInfoRec.Data, modLibrary.RemoveExtension(FileName, ".img")))
                //Rev20.00 判定を逆に変更 by長野 2014/12/04
                if (!ImageInfo.ReadImageInfo(ref theInfoRec.Data, modLibrary.RemoveExtension(FileName, ".img")))
                {
					//メッセージ表示：付帯情報のある画像ファイルを選択してください。
                    MessageBox.Show(CTResources.LoadResString(8127), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}

				//If StrComp(RemoveNull(theInfoRec.full_mode), "FULL") Then
				//v16.20/v17.00修正 byやまおか 2010/03/08
                if (string.Compare(modLibrary.RemoveNull(theInfoRec.Data.full_mode.GetString()), "FULL", true) != 0)
                {
					//メッセージ表示：スキャンモードがフルスキャンの画像を選択してください。
                    MessageBox.Show(CTResources.LoadResString(9336), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}

                txtImgFileName[Index].Text = FileName;
			}
			//v16.20/v17.00変更(ここまで) byやまおか 2010/03/04
		}

        //*******************************************************************************
        //機　　能： 終了ボタンクリック時処理
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
			//寸法校正フォームをアンロードする
            this.Close();　   //復活2015/01/26hata　Dispose前にCloseする
            //Rev20.00 showdialogで呼ばれているのでdisposeを使ってリソース破棄 by長野 2014/12/04
            this.Dispose();
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
			//Dim i As Integer   '//added by Isshiki
			int Index = 0;      //added by Isshiki

			    //寸法校正ファントム     V4.0 append by 鈴山 2001/02/01

			//added by 一色
			    for (Index = 1; Index <= 2; Index++)
                {
				    //GVal_DistVal = cwne2R(Index).Value
				    ScanCorrect.GVal_DistVal[Index] = (float)cwne2R[Index].Value;   //Rev16.2/17.0 修正 2010/02/23 by Iwasawa
			    }
			//

			//ボタンを使用不可にする
			cmdOK.Enabled = false;
			cmdEnd.Enabled = false;

			//マウスポインタを砂時計にする
			this.Cursor = Cursors.WaitCursor;

			//寸法校正画像を配列に読み込む
			//    If Get_DistanceCorrect_Image() Then
			//
			//        '寸法校正パラメータ計算
			//        If Get_DistanceCorrect_Parameter_Ex() Then
			//
			//            '寸法校正フォームをアンロードする
			//            Unload Me
			//
			//            Exit Sub
			//
			//        End If
			//
			//    End If

			//v9.7変更
			//If DoDistanceCorrect(Trim$(txtImgFileName.Text)) Then  '
			////v16.2/17.0 変更 by Isshiki/Iwasawa
			if (DoDistanceCorrect(txtImgFileName[2].Text.Trim()))
            {
				//寸法校正フォームをアンロードする
                this.Close();　   //復活2015/01/26hata　Dispose前にCloseする
                //Rev20.00 showDialogから呼ばれているので、リソース破棄のためdispose使用 by長野 2014/12/04
                this.Dispose();
                return;
			}

			//ボタンの状態を元に戻す
			cmdOK.Enabled = true;
			cmdEnd.Enabled = true;

			//マウスポインタを元に戻す
			this.Cursor = Cursors.Default;
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
		private void frmDistanceCorrect_Load(object sender, EventArgs e)
		{
			int Index = 0;  //v16.2/17.0 added by 一色

			//実行時はフラグをセット
			modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTScanCorrect;

			//フォームの設定
			//Me.Move FmStdLeft, FmStdTop

			//v7.0 リソース対応 by 間々田 2003/08/22
			this.Text = CTResources.LoadResString(StringTable.IDS_CorSize);   //寸法校正
			cmdEnd.Text = CTResources.LoadResString(StringTable.IDS_btnEnd);  //終　了
			cmdOK.Text = CTResources.LoadResString(StringTable.IDS_btnOK);    //ＯＫ

			//lblTitle2(1).Caption = LoadResStringWithColon(12104)   '寸法校正ファントム：  'Rev16.2/17.0 ファントム2つに対応 2010/2/23 by Isshiki/Iwasawa
			//lblTitle2(2).Caption = LoadResStringWithColon(12104)   '寸法校正ファントム：  'Rev16.2/17.0 ファントム2つに対応 2010/2/23by Isshiki/Iwasawa
			lblTitle2_1.Text = CTResources.LoadResString(12104);             //寸法校正ファントム 円柱直径：  'Rev16.2/17.0 ファントム2つに対応 2010/2/23 by Isshiki/Iwasawa
			lblTitle2_2.Text = CTResources.LoadResString(12104);             //寸法校正ファントム 円柱直径：  'Rev16.2/17.0 ファントム2つに対応 2010/2/23by Isshiki/Iwasawa
			//lblTitle1.Caption = LoadResStringWithColon(12105)      '寸法校正用画像：
			lblTitle1_0.Text = CTResources.LoadResString(12105) + "１：";      //寸法校正用画像1：  'Rev16.2/17.0 ファントム2つに対応 2010/3/11 byやまおか
            lblTitle1_1.Text = CTResources.LoadResString(12105) + "２：";      //寸法校正用画像2：  'Rev16.2/17.0 ファントム2つに対応 2010/3/11 byやまおか
            //Rev20.01 プロパティtagを参照にしたのでコメントアウト by長野 2015/05/19
            //Rev20.02 リソースから読む必要が無いのでデザインだけを変更 by長野 2015/06/20
            //lblFidOffsetName.Text = CTSettings.gStrFidOrFdd + " ：";          //FIDまたはFDDオフセット 'Rev16.20/v17.00 FPD対応 2010/03/12 byやまおか
            
            //変更2014/10/07hata_v19.51反映
            //fraFidFcdOffset.Text = StringTable.GetResString(StringTable.IDS_Offset, CTSettings.gStrFidOrFdd + "/" + CTResources.LoadResString(StringTable.IDS_FCD));  //FIDまたはFDD/FCDオフセット 'Rev16.20/v17.00 FPD対応 2010/03/12 byやまおか
            fraFidFcdOffset.Text = StringTable.GetResString(StringTable.IDS_AxisOffset,CTSettings.gStrFidOrFdd + "/" + CTResources.LoadResString(StringTable.IDS_FCD)); //v18.00変更 IDS_Offset→IDS_AxisOffset byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

			//現在のコモン内容を取り出す
			ScanCorrect.OptValueGet_Cor();

			//寸法校正ファントム

			//v16.2/17.0 added by Isshiki
			for (Index = 1; Index <= 2; Index++)
            {
                //変更2015/02/02hata_Max/Min範囲のチェック
				//cwne2R[Index].Value = (decimal)ScanCorrect.GVal_DistVal[Index];
                cwne2R[Index].Value = modLibrary.CorrectInRange((decimal)ScanCorrect.GVal_DistVal[Index], cwne2R[Index].Minimum, cwne2R[Index].Maximum);
            }
			// v16.2/17.0

			//現在のFDD・FCDオフセットを表示する     'v15.0追加 by 間々田 従来のスキャン条件で表示していたものをここに移動
			//FDD/FCDオフセットを表示
            //Rev23.10 計測CT対応 by長野 2015/10/18
            if (CTSettings.scaninh.Data.cm_mode == 0)//FCD,FDDの表示桁が増えた分、寸法校正の表示桁も増やす
            {
                lblFidOffset.Text = CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube].ToString("0.0000");
                lblFcdOffset.Text = CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()].ToString("0.000000");		//FDDオフセットは小数点以下４桁とする by 間々田 2005/11/29
            }
            else
            {
                lblFidOffset.Text = CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube].ToString("0.00");
                lblFcdOffset.Text = CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()].ToString("0.0000");		//FDDオフセットは小数点以下４桁とする by 間々田 2005/11/29
            }
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
		private void frmDistanceCorrect_FormClosed(object sender, FormClosedEventArgs e)
		{
			//終了時はフラグをリセット
			modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTScanCorrect);
		}

        //*******************************************************************************
        //機　　能： 寸法校正用画像テキストボックス・チェンジイベント処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void txtImgFileName_TextChanged(object sender, EventArgs e)
		{
			cmdOK.Enabled = (!string.IsNullOrEmpty(txtImgFileName[1].Text)) & (!string.IsNullOrEmpty(txtImgFileName[2].Text));  //Rev16.20/v17.00 変更 by Isshiki
		}

        //********************************************************************************
        //機    能  ：  寸法校正実行
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：                   [ /O] Boolean   結果(True:正常,False:異常)
        //補    足  ：  ScanCorrect.bas の Get_DistanceCorrect_Parameter_Exをリニューアルした
        //
        //履    歴  ：  V9.7   04/11/11  (SI4)間々田     　　新規作成
        //             V16.2/17.0   10/02/23  Isshiki/Iwasawa     2つの位置の画像による寸法校正でFCD,FDDｵﾌｾｯﾄ値を求めるように変更
        //********************************************************************************
		private bool DoDistanceCorrect(string FileName)
		{
			int rc = 0;
            //Ipc32v5.RECT tmpReg = default(Ipc32v5.RECT);
			int isize = 0;
			float icount = 0;
			int cnt = 0;
			string clptxt = null;               //ImageProのクリップボード経由の測定データ
			string swork = null;                //カウント値をclptxtから取り出したもの
			int iMode = 0;                      //データモード（0:ﾊｰﾌ 1:ﾌﾙ 2:ｵﾌｾｯﾄ）
			float max_scan_area = 0;            //寸法校正画像の最大スキャンエリア
			float scan_area = 0;                //寸法校正画像のスキャンエリア
			int Max1 = 0;                       //画像の最大値（1～16384）
			int Min1 = 0;                       //画像の最小値（-8192～8191）
			//   Dim theInfoRec      As ImageInfoStruct  'Rev16.20/v17.00 配列にする 2010/02/14 by Iwasawa
            int DotPos = 0;
            //int ConeiMode = 0;                  //データモード（0:ﾊｰﾌ,ﾌﾙ 1:ｵﾌｾｯﾄ）   'added by 山本 2007-2-12

//Rev16.2/17.0 added by Isshiki
			float[] fcd_offset = new float[3];  //FCDのｵﾌｾｯﾄ値
			float[] fdd_offset = new float[3];  //FDDのｵﾌｾｯﾄ値
			float[] mecaFCD = new float[3];     //FCD値(ｵﾌｾｯﾄ値を含まない)
			float[] mecaFDD = new float[3];     //FDD値(ｵﾌｾｯﾄ値を含まない)
			float[] FCD = new float[3];         //FCD値(ｵﾌｾｯﾄ値を含む)
			float[] FDD = new float[3];         //FDD値(ｵﾌｾｯﾄ値を含む)
			float[] d = new float[3];           //2値化した画像から求めた寸法校正用ファントムの半径
			float[] PixelSize = new float[3];   //1画素サイズ
			string[] iifield = new string[3];   //I.I.視野
			int i = 0;                          //カウンタ

            //modImageInfo.ImageInfoStruct[] theInfoRec = new modImageInfo.ImageInfoStruct[3];  //付帯情報 2010/02/24 by added by Iwasawa
            CTstr.IMAGEINFO[] theInfoRec = new CTstr.IMAGEINFO[3]; ;
            for (i = 0; i <= 2; i++)
            {
                theInfoRec[i].Initialize();
			}
            

			//戻り値初期化
            bool functionReturnValue = false;

			//エラー時の処理
			 // ERROR: Not supported in C#: OnErrorStatement

			//Rev16.2/17.0 入力画像チェック 2つの画像で、FCDが異なる ＆ 同一の寸法校正状態で撮影されている(fcd_offsetとfid_offsetがそれぞれ等しい)　2010/02/24 by Iwasawa
			for (i = 1; i <= 2; i++)
            {
				//if (!modImageInfo.ReadImageInfo(ref theInfoRec[i], modLibrary.RemoveExtension(txtImgFileName[i].Text, ".img")))
                //if (ImageInfo.ReadImageInfo(ref theInfoRec[i], modLibrary.RemoveExtension(txtImgFileName[i].Text, ".img")))
                //Rev20.00 判定を逆に変更 by長野 2014/12/04
                if (!ImageInfo.ReadImageInfo(ref theInfoRec[i], modLibrary.RemoveExtension(txtImgFileName[i].Text, ".img")))
                {
					//メッセージ表示：
					//付帯情報のある画像ファイルを選択してください。
                    MessageBox.Show(CTResources.LoadResString(8127), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return functionReturnValue;
				}
			}

			if (!(theInfoRec[1].fcd_offset == theInfoRec[2].fcd_offset) && theInfoRec[1].fid_offset == theInfoRec[2].fid_offset)
            {
				//メッセージ表示：
				//2つの画像を撮影した間に、寸法校正が1回以上行われています。同一の寸法校正状態で撮影した画像を選択してください。
                MessageBox.Show(CTResources.LoadResString(9327), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //9327はRev16.2/17.0で追加
				return functionReturnValue;
			}

			if (theInfoRec[1].fcd == theInfoRec[2].fcd)
            {
				//メッセージ表示：
				//2つの画像はFCDが同じです。FCDを変えて撮影した画像を選択してください。
                MessageBox.Show(CTResources.LoadResString(9334), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //9334はRev16.2/17.0で追加
				return functionReturnValue;	
			}
			//チェック完

			//v17.30 追加　by 長野　2010-09-27
			if (theInfoRec[1].detector != theInfoRec[2].detector)
            {
				//メッセージ表示
				//異なる検出器で撮影した画像が選択されました。同一の検出器で撮影した画像を選択してください。
                MessageBox.Show(CTResources.LoadResString(9337), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return functionReturnValue;
			}

            //変更2014/10/07hata_v19.51反映
            if (CTSettings.SecondDetOn)
            {
                if (((int)CTSettings.detectorParam.DetType != theInfoRec[1].detector))
                {
                    if (mod2ndDetctor.IsDet1mode)
                    {
                        //検出器１で撮影した画像を選択してください。
                        //Interaction.MsgBox(CT30K.My.Resources.str9338, MsgBoxStyle.Exclamation);
                        MessageBox.Show(CTResources.LoadResString(9338), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return functionReturnValue;
                    }
                    if (mod2ndDetctor.IsDet2mode)
                    {
                        //メッセージ表示
                        //検出器２で撮影した画像を選択してください。
                        //Interaction.MsgBox(CT30K.My.Resources.str9339, MsgBoxStyle.Exclamation);
                        MessageBox.Show(CTResources.LoadResString(9339), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return functionReturnValue;
                    }
                }
            }

            ////added by 一色
			//値を保存する配列を作成する。　　'//V16.2/17.0追加　2009/10/27 by Isshiki/Iwasawa
			//Dim i As Integer
			//i = 1
			for (i = 1; i <= 2; i++)
            {
//
				//寸法校正画像を開く
                if (!File.Exists(txtImgFileName[i].Text))
                {
					//メッセージ表示：
					//   付帯情報のある画像ファイルを選択してください。
					//MsgBox LoadResString(8127), vbExclamation

					//   画像ファイル読み込みができません。
					//   ファイルの有無を確認後、寸法校正を再度行ってください。
                    MessageBox.Show(CTResources.LoadResString(9455), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return functionReturnValue;
				}

				//入力画像のサイズからレイアウトを判定する
                FileInfo fileInfo = new FileInfo(txtImgFileName[i].Text);
                //2014/11/06hata キャストの修正
                ScanCorrect.Xsize_D = Convert.ToInt32(Math.Sqrt(fileInfo.Length / 2F));
				ScanCorrect.Ysize_D = ScanCorrect.Xsize_D;

				//領域確保
				ScanCorrect.DISTANCE_IMAGE = new ushort[ScanCorrect.Xsize_D * ScanCorrect.Ysize_D];

				if (ScanCorrect.ImageOpen(ref ScanCorrect.DISTANCE_IMAGE[0], 
                                          txtImgFileName[i].Text, ScanCorrect.Xsize_D, ScanCorrect.Ysize_D) != 0)
                {
					//メッセージ表示：
					//   画像ファイル読み込みができません。
					//   ファイルの有無を確認後、寸法校正を再度行ってください。
                    MessageBox.Show(CTResources.LoadResString(9455), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return functionReturnValue;
				}

				//マウスカーソルを元に戻す
				this.Cursor = Cursors.Default;

				//画像の最大値、最小値を求める
				ScanCorrect.GetMaxMin(ref ScanCorrect.DISTANCE_IMAGE[0], 
                                      ScanCorrect.Xsize_D, ScanCorrect.Ysize_D, ref Min1, ref Max1);

				//２値化画像を表示する
				//If Not frmDistanceBinarized.Dialog(Min1, Max1, Threshold255_D) Then
				//v17.10変更 Captionに○/2を追加 byやまおか 2010/08/09
				if (!frmDistanceBinarized.Instance.Dialog(Min1, Max1, ref ScanCorrect.Threshold255_D, Convert.ToString(i) + "/2"))
                {
					functionReturnValue = true;
					return functionReturnValue;
				}

				//マウスカーソルを砂時計にする
				this.Cursor = Cursors.WaitCursor;

				//画像の２値化      'commented by 山本 99-6-5
				//☆☆２値化画像に変換(CT用)

				ScanCorrect.BIN_IMAGE = new ushort[ScanCorrect.Xsize_D * ScanCorrect.Ysize_D];
				//v15.0変更 -1した by 間々田 2009/06/03
				//       Call BinarizeImage(DISTANCE_IMAGE(0), BIN_IMAGE(0), Xsize_D, Ysize_D, Threshold255_D, 1, 1)
				ScanCorrect.BinarizeImage_signed(ref ScanCorrect.DISTANCE_IMAGE[0], ref ScanCorrect.BIN_IMAGE[0], 
                                                 ScanCorrect.Xsize_D, ScanCorrect.Ysize_D, ScanCorrect.Threshold255_D, 1, 1);   //v9.7変更 by 間々田 2004-12-09 符号付Short型配列対応


                #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                /*
                tmpReg.Left = 0;
				tmpReg.Top = 0;
				tmpReg.Right = ScanCorrect.Xsize_D - 1;
				tmpReg.Bottom = ScanCorrect.Ysize_D - 1;

				//開いている全ての画像ｳｨﾝﾄﾞを閉じる
				rc = Ipc32v5.IpAppCloseAll();

				//v14.00追加 ImagePro空間較正の影響を回避(ImageProVer3の場合) append by 山影 2007-7-27
#if ImageProV3
				rc = Ipc32v5.IpSCalShow(1);
				rc = Ipc32v5.IpSCalSelect("(none)");
				rc = Ipc32v5.IpSCalShow(0);
#endif

				//空の画像ウィンドウを生成（Gray Scale 16形式）
                rc = Ipc32v5.IpWsCreate((short)ScanCorrect.Xsize_D, (short)ScanCorrect.Ysize_D, 300, (short)Ipc32v5.IMC_GRAY16);

				//ユーザが作成した画像ﾃﾞｰﾀをImage-Proの画像に書込む
                rc = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, 
                                          ref ScanCorrect.BIN_IMAGE[0],
                                          Ipc32v5.CPROG);
				//画像ウィンドの再描画
				rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);

				//円柱の直径の測定   'added by 山本 99-6-5
				rc = Ipc32v5.IpBlbSetAttr(Ipc32v5.BLOB_AUTORANGE, 1);
				rc = Ipc32v5.IpBlbSetAttr(Ipc32v5.BLOB_BRIGHTOBJ, 1);
				//    rc = IpBlbEnableMeas(BLBM_MEANFERRET, 1)       'deleted by 山本　2005-10-22
				rc = Ipc32v5.IpBlbEnableMeas(Ipc32v5.BLBM_AREA, 1);     //added by 山本　2005-10-22
				rc = Ipc32v5.IpBlbSetAttr(Ipc32v5.BLOB_FILLHOLES, 1);   //added by 山本　2005-10-22
				//    rc = IpBlbSetFilterRange(BLBM_MEANFERRET, 15#, 1000000#) '追加 by 中島 '99-08-26     'deleted by 山本　2005-10-22
				//rc = IpBlbSetFilterRange(BLBM_AREA, 200#, 1000000#)         'added by 山本　2005-10-22
				//rc = IpBlbSetFilterRange(BLBM_AREA, 200#, CLng(2048) * CLng(2048)) 'v14.23変更 by 間々田 2008/12/15 2048画素画像の場合、1000000だと足らないので
                rc = Ipc32v5.IpBlbSetFilterRange(Ipc32v5.BLBM_AREA, 200.0f, Convert.ToInt32(ScanCorrect.Xsize_D) * Convert.ToInt32(ScanCorrect.Ysize_D));   //v16.20変更 4096対応 byやまおか 2010/04/05
				
                rc = Ipc32v5.IpBlbMeasure();
				rc = Ipc32v5.IpBlbCount();
				rc = Ipc32v5.IpBlbUpdate(0);

				rc = Ipc32v5.IpBlbShowData(1);
				rc = Ipc32v5.IpBlbSaveData("", Ipc32v5.S_CLIPBOARD + Ipc32v5.S_Y_AXIS);
				rc = Ipc32v5.IpBlbShowData(0);
				rc = Ipc32v5.IpBlbShow(0);
                */
                rc = CallImageProFunction.CallDoDistanceCorrectStep1(ScanCorrect.BIN_IMAGE, ScanCorrect.Ysize_D, ScanCorrect.Xsize_D);
                #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

                //クリップボード・データの取得
                IDataObject datObj = Clipboard.GetDataObject();
                if (datObj.GetDataPresent(DataFormats.Text))
                {
                    clptxt = (string)datObj.GetData(DataFormats.Text);
                }

				//クリップボードから得た測定値の中の、データ部を取り出す
				swork = "";
                for (cnt = 1; cnt <= clptxt.Length; cnt++)
                {
                    char ch = clptxt.Substring(cnt - 1, 1)[0];  // String → char
                    if ((int)ch < 0x20)
                    {
						//CRを検出したら、その直前のデータがデータ部
                        if ((int)ch == 13)
                        {
							break;
						}
						swork = "";
					}
                    else
                    {
						//制御記号が出てくるまでは文字を加算する
                        swork = swork + clptxt.Substring(cnt - 1, 1);
					}
				}
                float.TryParse(swork.Trim(), out icount);
				//icount = icount * 1.0048999   'イメージプロによる直径測定に誤差のための調整 99-9-28 by 山本    'deleted by 山本　2005-10-22
				icount = (float)(2 * Math.Sqrt(icount / ScanCorrect.Pai));
				//面積から直径を求める　added by 山本　2005-10-22
                if (icount <= 0)
                {
                    goto Err_Process;
                }

				ScanCorrect.GVal_2R_D = icount;//V4.0 append by 鈴山 2001/02/05
                DotPos = txtImgFileName[i].Text.LastIndexOf('.') + 1;
				//画像情報の読み込み
				//If ReadImageInfo(theInfoRec, Left$(FileName, DotPos) & "inf") Then
				//If ReadImageInfo(theInfoRec, Left$(txtImgFileName(i).Text, DotPos - 1)) Then   'v10.2変更 by 間々田 2005/06/29 'Rev16.2/17.0 付帯情報は先に読み込むので削除 2010/02/24 by Iwasawsa

				//ｽｷｬﾝﾓｰﾄﾞ
				//'Select Case Trim$(theInfoRec.full_mode)
				//Select Case RemoveNull(theInfoRec.full_mode) 'v9.7変更 by 間々田 2004/12/09 Nullを取り除く
				//    Case "HALF":    iMode = 0
				//    Case "FULL":    iMode = 1
				//    Case "OFFSET":  iMode = 2
				//    Case Else:      iMode = 0
				//End Select

				//v11.4変更 by 間々田 2006/03/16
				//iMode = GetIndexByStr(RemoveNull(theInfoRec.full_mode), MyCtinfdef.full_mode())
				//iMode = modLibrary.GetIndexByStr(modLibrary.RemoveNull(theInfoRec[i].full_mode.GetString()), modCommon.MyCtinfdef.full_mode);				//Rev16.20/v17.00 付帯情報配列化
                iMode = modCommon.MyCtinfdef.full_mode.GetIndexByStr(modLibrary.RemoveNull(theInfoRec[i].full_mode.GetString()), 0);

				ScanCorrect.GFlg_ScanMode_D = (short)(iMode + 1);
				//V4.0 append by 鈴山 2001/02/05

				//最大スキャンエリア
				//max_scan_area = theInfoRec.max_mscan_area
				max_scan_area = theInfoRec[i].max_mscan_area;           //Rev16.2/17.0 変更 2010/02/25 by Iwasawa

				//スキャンエリア
				//scan_area = Val(theInfoRec.scale) / 1000
				//scan_area = Conversion.Val(theInfoRec[i].scale) / 1000; //Rev16.2/17.0 変更 2010/02/25 by Iwasawa
                float scale = 0.0F;
                float.TryParse(theInfoRec[i].scale.GetString(), out scale);
                //2014/11/06hata キャストの修正
                scan_area = scale / 1000F;    //Rev16.2/17.0 変更 2010/02/25 by Iwasawa

				//付帯情報の回転選択可否が可の場合、付帯情報の回転選択の値（0:テーブル回転 1:Ｘ線管回転）を代入  'added by 間々田 2004/02/18
				//        If theInfoRec.rotate_select_inh = 0 Then GFlg_MultiTube_D = theInfoRec.rotate_select
                if (CTSettings.scaninh.Data.rotate_select == 0)
                {
					//GFlg_MultiTube_D = theInfoRec.rotate_select 'change by 間々田 2004/06/03 コモンscaninh.rotate_selectを参照する
					ScanCorrect.GFlg_MultiTube_D = (short)theInfoRec[i].rotate_select; //Rev16.2/17.0 変更 2010/02/25 by Iwasawa
				}
                else
                {
					//X線管(0:160kV,1:225kV)                     'added by 山本 2000-8-19 三菱対応
					//GFlg_MultiTube_D = Val(theInfoRec.Focus)
                    int.TryParse(theInfoRec[i].focus.GetString(), out ScanCorrect.GFlg_MultiTube_D);    //Rev16.2/17.0 変更 2010/02/25 by Iwasawa
				}

				//必ず０か１にするための措置 by 間々田 2006/01/18
                if (ScanCorrect.GFlg_MultiTube_D != 1)
                {
                    ScanCorrect.GFlg_MultiTube_D = 0;
                }

				//End If 'Rev16.2/17.0 付帯情報は先に読み込むので削除したif文のカッコ 2010/02/24 by Iwasawsa

				isize = ScanCorrect.Xsize_D;

				//ＦＣＤ=(最大スキャンエリア / (2*sin(fanangle[0]/2))
				//最大スキャンエリア = (isize / icount)　* 実円柱直径 * (元の最大スキャンエリア / 元のスキャンエリア)
				switch (iMode)
                {
					//校正画像のデータモード
					case 0:
					case 1:
						//GVal_ScnAreaMax(0) = (isize / icount) * GVal_DistVal * max_scan_area / scan_area    'V4.0 change by 鈴山 2001/02/01 (cwne2R.Value → GVal_DistVal)
						//GVal_ScnAreaMax(1) = (isize / icount) * GVal_DistVal * max_scan_area / scan_area    'V4.0 change by 鈴山 2001/02/01 (cwne2R.Value → GVal_DistVal)
                        //2014/11/06hata キャストの修正
                        ScanCorrect.GVal_ScnAreaMax[0] = ((float)isize / icount) * ScanCorrect.GVal_DistVal[i] * max_scan_area / scan_area;//v16.2/17.0 GVal_DistValを配列化 2010/02/25 by Iwasawa
						ScanCorrect.GVal_ScnAreaMax[1] = ((float)isize / icount) * ScanCorrect.GVal_DistVal[i] * max_scan_area / scan_area;//v16.2/17.0 GVal_DistValを配列化 2010/02/25 by Iwasawa
						
                        //未使用のため削除　2014/06/17hata
                        //ConeiMode = 0;    //added by 山本 2007-2-12
						break;
					case 2:
						//GVal_ScnAreaMax(2) = (isize / icount) * GVal_DistVal * max_scan_area / scan_area    'V4.0 change by 鈴山 2001/02/01 (cwne2R.Value → GVal_DistVal)
                        //2014/11/06hata キャストの修正
                        ScanCorrect.GVal_ScnAreaMax[2] = ((float)isize / icount) * ScanCorrect.GVal_DistVal[i] * max_scan_area / scan_area;//v16.2/17.0 GVal_DistValを配列化 2010/02/25 by Iwasawa

                        //未使用のため削除　2014/06/17hata
                        //ConeiMode = 1;    //added by 山本 2007-2-12
						break;
                    default:
                        break;
				}

				//スキャンエリアの設定（現在のスキャン条件のデータモードでの最大スキャンエリアを書込む）
                ScanCorrect.GVal_MScnArea = ScanCorrect.GVal_ScnAreaMax[CTSettings.scansel.Data.scan_mode - 1];

				//ＦＣＤ=(最大スキャンエリア / (2*sin(fanangle[0]/2))
				//If GVal_Mfanangle(2, iMode) = 0 Then


				//Rev16.2/17.0 ************新ソフトにおいて必要なし*************** by Isshiki

				//changed by 山本 2007-2-12      '
				//    If theInfoRec.bhc = 0 Then '通常スキャン時
				//        If scancondpar.mfanangle(iMode, 2) = 0 Then 'ファン角scancondpar.mfanangle[2][iMode]のこと
				//メッセージ表示：
				//   寸法校正用スキャン後、条件が変わっています。
				//   再度寸法校正画像を収集してから、他の操作をせずにすぐに寸法校正を実行してください。
				//            MsgBox LoadResString(9331), vbExclamation
				//            Exit Function
				//        End If
				//    Else                    'コーンビーム時
				//        If scancondpar.theta0(ConeiMode) = 0 Then 'ファン角scancondpar.theta0(ConeiMode)のこと
				//メッセージ表示：
				//   寸法校正用スキャン後、条件が変わっています。
				//   再度寸法校正画像を収集してから、他の操作をせずにすぐに寸法校正を実行してください。
				//            MsgBox LoadResString(9331), vbExclamation
				//        End If
				//            Exit Function
				//        End If
				//    End If
				//***************************************************

				////Rev16.2/17.0 added by Isshiki 付帯情報チェック時取得済みのため削除 by Iwasawa
				//付帯情報の取得
				//If Not ReadImageInfo(theInfoRec, RemoveExtension(txtImgFileName(i).Text, ".img")) Then
				//End If

                fcd_offset[i] = theInfoRec[i].fcd_offset;                  //FCDのオフセット値
                fdd_offset[i] = theInfoRec[i].fid_offset;                  //FDDのオフセット値
				//            mecaFCD(i) = .FCD                            'FCD値
				//            mecaFDD(i) = .Fid                            'FDD値
				//            FCD(i) = .FCD - .fcd_offset                  'FCDメカの読値
				//            FDD(i) = .Fid - .fid_offset                  'FDDメカの読値
                mecaFCD[i] = theInfoRec[i].fcd - theInfoRec[i].fcd_offset;        //FCDメカの読値 修正 by Iwasawa
                mecaFDD[i] = theInfoRec[i].fid - theInfoRec[i].fid_offset;        //FCDメカの読値 修正 by Iwasawa
                FCD[i] = theInfoRec[i].fcd;                                //FCD値(ｵﾌｾｯﾄを含む) 修正 by Iwasawa
                FDD[i] = theInfoRec[i].fid;                                //FDD値(ｵﾌｾｯﾄを含む) 修正 by Iwasawa
                //2014/11/06hata キャストの修正
                //PixelSize[i] = (float)(Convert.ToDouble(theInfoRec[i].scale) / 1000D / Convert.ToDouble(theInfoRec[i].matsiz));	//1画素サイズ(mm)
                //Rev20.00 GetString追加 by長野 2014/12/04
                PixelSize[i] = (float)(Convert.ToDouble(theInfoRec[i].scale.GetString()) / 1000D / Convert.ToDouble(theInfoRec[i].matsiz.GetString()));	//1画素サイズ(mm)
                
                iifield[i] = theInfoRec[i].iifield.GetString();                         //I.I.視野
//
				//GVal_Fcd_D = GVal_ScnAreaMax(iMode) / (2 * Sin((GVal_Mfanangle(2, iMode) * Pai / 180 / 2)))
				//GVal_Fcd_D = GVal_ScnAreaMax(iMode) / (2 * Sin(scancondpar.mfanangle(iMode, 2) * Pai / 180 / 2))

				////Rev16.2/17.0 寸法校正方法変更のため削除 by Isshiki/Iwasawa
				//changed by 山本 2007-2-12
				//   If theInfoRec.bhc = 0 Then
				//GVal_Fcd_D = GVal_ScnAreaMax(iMode) / (2 * Sin(fanangle(i) * Pai / 180 / 2))       '通常スキャン時の計算
				//GVal_Fcd_O = FCD(1) * FCD(2) * (GVal_ScnAreaMax(iMode, 1) - GVal_ScnAreaMax(iMode, 2)) / (FCD(1) * GVal_ScnAreaMax(iMode, 2) - FCD(2) * GVal_ScnAreaMax(iMode, 1))
				//GVal_Fid_O = FID(1) * (FCD(1) * GVal_ScnAreaMax(iMode, 2) * (GVal_ScnAreaMax(iMode, 1) - GVal_DistVal) - FCD(2) * GVal_ScnAreaMax(iMode, 2) * (GVal_ScnAreaMax(iMode, 2) - GVal_DistVal)) / (GVal_DistVal * (FCD(1) * GVal_ScnAreaMax(iMode, 2) - FCD(2) * GVal_ScnAreaMax(iMode, 1)))
				//      Else
				//        GVal_Fcd_D = GVal_ScnAreaMax(iMode) / (2 * Sin(scancondpar.theta0(ConeiMode) / 2))                      'コーンビーム時の計算
				//    End If
				////

            //Rev16.2/17.0 added by Isshiki
				//GVal_Fcd = FCD(i)
				ScanCorrect.GVal_Fcd2 = FCD[i];
				//v17.60変更 byやまおか 2011/06/10
				//以下は他でも使用のためあえて配列にしない 2010/02/25 コメント by Iwasawa
				if (i == 1)     //1個目の各値を保持する
                {
					ScanCorrect.GVal_ScnAreaMax1[iMode] = ScanCorrect.GVal_ScnAreaMax[iMode];
					ScanCorrect.GVal_2R_D1 = ScanCorrect.GVal_2R_D;
					ScanCorrect.GVal_Fcd1 = FCD[1];
				}
            //

				//Image-Pro 画像データの取得
				if (ScanCorrect.IMGPRODBG == 1)
                {
                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*
					tmpReg.Left = 0;
					tmpReg.Top = 0;
					tmpReg.Right = ScanCorrect.Xsize_D - 1;
					tmpReg.Bottom = ScanCorrect.Ysize_D - 1;
                    rc = Ipc32v5.IpDocGetArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, 
                                              ref ScanCorrect.DISTANCE_IMAGE[0], 
                                              Ipc32v5.CPROG);                       //☆☆ユーザが作成した画像ﾃﾞｰﾀをのImage-Proの画像に書込む
					rc = Ipc32v5.IpHstEqualize(Ipc32v5.EQ_BESTFIT);
                    */
                    rc = CallImageProFunction.CallDoDistanceCorrectStep2(ScanCorrect.DISTANCE_IMAGE, ScanCorrect.Ysize_D, ScanCorrect.Xsize_D);
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//
                }

				functionReturnValue = true;

				//寸法校正フォームを隠す
				//Me.hide
				//1枚目の画像のときは隠さない
                if (i == 2) //v16.20/v17.00変更 byやまおか 2010/03/01
                { 
                    //変更2015/1/17hata_非表示のときにちらつくため
                    //this.Hide();
                    modCT30K.FormHide(this);
                }    
				

			}   //added by 一色
			

			////added by Isshiki
			//Rev16.2/17.0 付帯情報チェックは先頭で行うため削除 2010/02/25
			//付帯情報から寸法校正が出来る画像であるか判断する。
			//If iifield(1) = iifield(2) And _
			//'   fcd_offset(1) = fcd_offset(2) And _
			//'   FDD(1) = (2) And _
			//'   fdd_offset(1) = fdd_offset(2) And _
			//'   FCD(1) <> FCD(2) Then
			//Else
			//メッセージ表示：
			//選択した画像のFCD、FID、I.I.視野の付帯情報の条件を確認して、再度画像を選択してください。
			//    MsgBox LoadResString(9327), vbExclamation
			//    Exit Function
			//End If
			//削除END by Iwasawa

			//画像から求めた円柱ファントムの直径を計算する
			d[1] = ScanCorrect.GVal_2R_D1 * PixelSize[1];
			d[2] = ScanCorrect.GVal_2R_D * PixelSize[2];

			//Rev16.2/17.0 オフセット値の誤差を計算する。 2つの画像でFCD,FDD,ファントム直径がそれぞれ異なっていても求まる計算式 by Isshiki
			//        2つの画像のｵﾌｾｯﾄを含むFCD,FDDをメカ読み値として計算し、求まったｵﾌｾｯﾄから元画像のｵﾌｾｯﾄ値を引く
            ScanCorrect.GVal_Fcd_O = FCD[1] * FCD[2] * (FDD[1] * (float)cwne2R[2].Value * (d[1] - (float)cwne2R[1].Value) - FDD[2] * (float)cwne2R[1].Value * (d[2] - (float)cwne2R[2].Value)) / (FCD[1] * FDD[2] * (float)cwne2R[1].Value * d[2] - FCD[2] * FDD[1] * d[1] * (float)cwne2R[2].Value);
            ScanCorrect.GVal_Fid_O = FDD[1] * FDD[2] * (FCD[1] * d[2] * (d[1] - (float)cwne2R[1].Value) - FCD[2] * d[1] * (d[2] - (float)cwne2R[2].Value)) / (FCD[1] * FDD[2] * (float)cwne2R[1].Value * d[2] - FCD[2] * FDD[1] * d[1] * (float)cwne2R[2].Value);

			//Rev16.2/17.0 2つの画像でFCD,FDD,ファントム直径がそれぞれ異なっていても求まる計算式にしておく 2010/02/25 by Iwasawa
			//        2つの画像のｵﾌｾｯﾄを含むFCD,FDDをメカ読み値として計算し、求まったｵﾌｾｯﾄから元画像のｵﾌｾｯﾄ値を引く


			//真のFCDオフセット、FDDオフセット値 1、2どちらの値を使っても良い
			//GVal_Fcd_O = fcd_offset(2) + GVal_Fcd_O
			//GVal_Fid_O = fid_offset(2) + GVal_Fid_O

			ScanCorrect.GVal_Fcd_O = fcd_offset[2] + ScanCorrect.GVal_Fcd_O;
			ScanCorrect.GVal_Fid_O = fdd_offset[2] + ScanCorrect.GVal_Fid_O;


			//オフセット値を含んだFCD,FDDの値を計算　求まった値がFDD>FCDであることのチェック用なので1、2どちらの値を使っても良い
			ScanCorrect.GVal_Fcd_D = mecaFCD[2] + ScanCorrect.GVal_Fcd_O;
			ScanCorrect.GVal_Fid_D = mecaFDD[2] + ScanCorrect.GVal_Fid_O;

//

			//寸法校正結果フォームを表示する
			frmDistanceCorrectResult.Instance.ShowDialog();
			return functionReturnValue;

Err_Process:
			//メッセージ表示：
			//   寸法校正ファントムの２値化抽出に失敗しました。
			//   事前に必要な校正を正しく行っていない可能性があります。
			//   幾何歪校正・回転中心校正を実行後、寸法校正を再度行ってください。
            //変更2014/11/18hata_MessageBox確認
            //MessageBox.Show(CTResources.LoadResString(9523), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            MessageBox.Show(CTResources.LoadResString(9523), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

			return functionReturnValue;
        }

        #region コメントアウト

        //Private Function DoDistanceCorrect(ByVal FileName As String) As Boolean

        //    Dim rc              As Long
        //    Dim tmpReg          As RECT
        //    Dim isize           As Long
        //    Dim icount          As Single
        //    Dim cnt             As Long
        //    Dim clptxt          As String   'ImageProのクリップボード経由の測定データ
        //    Dim swork           As String   'カウント値をclptxtから取り出したもの
        //    Dim iMode           As Integer  'データモード（0:ﾊｰﾌ 1:ﾌﾙ 2:ｵﾌｾｯﾄ）
        //    Dim max_scan_area   As Single   '寸法校正画像の最大スキャンエリア
        //    Dim scan_area       As Single   '寸法校正画像のスキャンエリア
        //    Dim Max1            As Long     '画像の最大値（1～16384）
        //    Dim Min1            As Long     '画像の最小値（-8192～8191）
        //    Dim theInfoRec      As ImageInfoStruct
        //    Dim DotPos          As Integer
        //    Dim ConeiMode       As Integer  'データモード（0:ﾊｰﾌ,ﾌﾙ 1:ｵﾌｾｯﾄ）   'added by 山本 2007-2-12

        //戻り値初期化
        //    DoDistanceCorrect = False

        //エラー時の処理
        //    On Error GoTo Err_Process

        //値を保存する配列を作成する。　　追加　一色

        //    Dim i As Integer    '追加　一色
        //    For i = 1 To 2      '追加　一色


        //寸法校正画像を開く
        //        If Not FSO.FileExists(FileName) Then
        //メッセージ表示：
        //   画像ファイル読み込みができません。
        //   ファイルの有無を確認後、寸法校正を再度行ってください。
        //            MsgBox LoadResString(9455), vbExclamation
        //            Exit Function
        //            End If
        //    Next i

        //入力画像のサイズからレイアウトを判定する
        //        Xsize_D = Sqr(FileLen(FileName) / 2)
        //        Ysize_D = Xsize_D

        //領域確保
        //        ReDim DISTANCE_IMAGE(Xsize_D * Ysize_D - 1)

        //        If ImageOpen(DISTANCE_IMAGE(0), FileName, Xsize_D, Ysize_D) <> 0 Then
        //メッセージ表示：
        //   画像ファイル読み込みができません。
        //   ファイルの有無を確認後、寸法校正を再度行ってください。
        //            MsgBox LoadResString(9455), vbExclamation
        //            Exit Function
        //        End If

        //マウスカーソルを元に戻す
        //        Me.MousePointer = vbDefault

        //画像の最大値、最小値を求める
        //        Call GetMaxMin(DISTANCE_IMAGE(0), Xsize_D, Ysize_D, Min1, Max1)

        //２値化画像を表示する
        //        If Not frmDistanceBinarized.Dialog(Min1, Max1, Threshold255_D) Then
        //            DoDistanceCorrect = True
        //            Exit Function
        //        End If

        //マウスカーソルを砂時計にする
        //        Me.MousePointer = vbHourglass

        //画像の２値化      'commented by 山本 99-6-5
        //☆☆２値化画像に変換(CT用)

        //        ReDim BIN_IMAGE(Xsize_D * Ysize_D - 1) 'v15.0変更 -1した by 間々田 2009/06/03
        //'       Call BinarizeImage(DISTANCE_IMAGE(0), BIN_IMAGE(0), Xsize_D, Ysize_D, Threshold255_D, 1, 1)
        //        Call BinarizeImage_signed(DISTANCE_IMAGE(0), BIN_IMAGE(0), Xsize_D, Ysize_D, Threshold255_D, 1, 1) 'v9.7変更 by 間々田 2004-12-09 符号付Short型配列対応

        //        tmpReg.Left = 0
        //        tmpReg.Top = 0
        //        tmpReg.Right = Xsize_D - 1
        //        tmpReg.Bottom = Ysize_D - 1

        //開いている全ての画像ｳｨﾝﾄﾞを閉じる
        //        rc = IpAppCloseAll()

        //v14.00追加 ImagePro空間較正の影響を回避(ImageProVer3の場合) append by 山影 2007-7-27
        //#If ImageProV3 Then
        //    rc = IpSCalShow(1)
        //    rc = IpSCalSelect("(none)")
        //    rc = IpSCalShow(0)
        //#End If

        //空の画像ウィンドウを生成（Gray Scale 16形式）
        //    rc = IpWsCreate(Xsize_D, Ysize_D, 300, IMC_GRAY16)

        //ユーザが作成した画像ﾃﾞｰﾀをImage-Proの画像に書込む
        //    rc = IpDocPutArea(DOCSEL_ACTIVE, tmpReg, BIN_IMAGE(0), CPROG)

        //画像ウィンドの再描画
        //    rc = IpAppUpdateDoc(DOCSEL_ACTIVE)

        //円柱の直径の測定   'added by 山本 99-6-5
        //    rc = IpBlbSetAttr(BLOB_AUTORANGE, 1)
        //    rc = IpBlbSetAttr(BLOB_BRIGHTOBJ, 1)
        //'    rc = IpBlbEnableMeas(BLBM_MEANFERRET, 1)       'deleted by 山本　2005-10-22
        //    rc = IpBlbEnableMeas(BLBM_AREA, 1)              'added by 山本　2005-10-22
        //    rc = IpBlbSetAttr(BLOB_FILLHOLES, 1)            'added by 山本　2005-10-22
        //'    rc = IpBlbSetFilterRange(BLBM_MEANFERRET, 15#, 1000000#) '追加 by 中島 '99-08-26     'deleted by 山本　2005-10-22
        //    'rc = IpBlbSetFilterRange(BLBM_AREA, 200#, 1000000#)         'added by 山本　2005-10-22
        //    rc = IpBlbSetFilterRange(BLBM_AREA, 200#, CLng(2048) * CLng(2048)) 'v14.23変更 by 間々田 2008/12/15 2048画素画像の場合、1000000だと足らないので

        //    rc = IpBlbMeasure()
        //    rc = IpBlbCount()
        //    rc = IpBlbUpdate(0)

        //    rc = IpBlbShowData(1)
        //    rc = IpBlbSaveData("", S_CLIPBOARD + S_Y_AXIS)
        //    rc = IpBlbShowData(0)
        //    rc = IpBlbShow(0)
        //    clptxt = Clipboard.GetText(vbCFText)

        //クリップボードから得た測定値の中の、データ部を取り出す
        //    swork = ""
        //    For cnt = 1 To Len(clptxt)
        //        If Asc(Mid(clptxt, cnt, 1)) < &H20 Then
        //CRを検出したら、その直前のデータがデータ部
        //            If Asc(Mid(clptxt, cnt, 1)) = 13 Then
        //                Exit For
        //            End If
        //            swork = ""
        //        Else
        //制御記号が出てくるまでは文字を加算する
        //            swork = swork & Mid(clptxt, cnt, 1)
        //        End If
        //    Next
        //    icount = Val(Trim(swork))
        //icount = icount * 1.0048999   'イメージプロによる直径測定に誤差のための調整 99-9-28 by 山本    'deleted by 山本　2005-10-22
        //    icount = 2 * Sqr(icount / Pai)     '面積から直径を求める　added by 山本　2005-10-22
        //    If icount <= 0 Then GoTo Err_Process
        //    GVal_2R_D = icount      'V4.0 append by 鈴山 2001/02/05

        //    DotPos = InStrRev(FileName, ".")

        //画像情報の読み込み
        //If ReadImageInfo(theInfoRec, Left$(FileName, DotPos) & "inf") Then
        //    If ReadImageInfo(theInfoRec, Left$(FileName, DotPos - 1)) Then          'v10.2変更 by 間々田 2005/06/29

        //ｽｷｬﾝﾓｰﾄﾞ
        //'Select Case Trim$(theInfoRec.full_mode)
        //Select Case RemoveNull(theInfoRec.full_mode) 'v9.7変更 by 間々田 2004/12/09 Nullを取り除く
        //    Case "HALF":    iMode = 0
        //    Case "FULL":    iMode = 1
        //    Case "OFFSET":  iMode = 2
        //    Case Else:      iMode = 0
        //End Select

        //v11.4変更 by 間々田 2006/03/16
        //        iMode = GetIndexByStr(RemoveNull(theInfoRec.full_mode), MyCtinfdef.full_mode())

        //        GFlg_ScanMode_D = iMode + 1                 'V4.0 append by 鈴山 2001/02/05

        //最大スキャンエリア
        //        max_scan_area = theInfoRec.max_mscan_area

        //スキャンエリア
        //        scan_area = Val(theInfoRec.scale) / 1000

        //付帯情報の回転選択可否が可の場合、付帯情報の回転選択の値（0:テーブル回転 1:Ｘ線管回転）を代入  'added by 間々田 2004/02/18
        //'        If theInfoRec.rotate_select_inh = 0 Then GFlg_MultiTube_D = theInfoRec.rotate_select
        //        If scaninh.rotate_select = 0 Then
        //            GFlg_MultiTube_D = theInfoRec.rotate_select 'change by 間々田 2004/06/03 コモンscaninh.rotate_selectを参照する
        //        Else
        //X線管(0:160kV,1:225kV)                     'added by 山本 2000-8-19 三菱対応
        //            GFlg_MultiTube_D = Val(theInfoRec.Focus)
        //        End If

        //必ず０か１にするための措置 by 間々田 2006/01/18
        //        If GFlg_MultiTube_D <> 1 Then GFlg_MultiTube_D = 0

        //    End If

        //    isize = Xsize_D

        //ＦＣＤ=(最大スキャンエリア / (2*sin(fanangle[0]/2))
        //最大スキャンエリア = (isize / icount)　* 実円柱直径 * (元の最大スキャンエリア / 元のスキャンエリア)
        //    Select Case iMode   '校正画像のデータモード
        //        Case 0, 1
        //            GVal_ScnAreaMax(0) = (isize / icount) * GVal_DistVal * max_scan_area / scan_area    'V4.0 change by 鈴山 2001/02/01 (cwne2R.Value → GVal_DistVal)
        //            GVal_ScnAreaMax(1) = (isize / icount) * GVal_DistVal * max_scan_area / scan_area    'V4.0 change by 鈴山 2001/02/01 (cwne2R.Value → GVal_DistVal)
        //            ConeiMode = 0                                                                       'added by 山本 2007-2-12
        //        Case 2
        //            GVal_ScnAreaMax(2) = (isize / icount) * GVal_DistVal * max_scan_area / scan_area    'V4.0 change by 鈴山 2001/02/01 (cwne2R.Value → GVal_DistVal)
        //            ConeiMode = 1                                                                       'added by 山本 2007-2-12
        //    End Select

        //スキャンエリアの設定（現在のスキャン条件のデータモードでの最大スキャンエリアを書込む）
        //    GVal_MScnArea = GVal_ScnAreaMax(scansel.scan_mode - 1)

        //ＦＣＤ=(最大スキャンエリア / (2*sin(fanangle[0]/2))
        //If GVal_Mfanangle(2, iMode) = 0 Then

        //changed by 山本 2007-2-12      '
        //    If theInfoRec.bhc = 0 Then '通常スキャン時
        //        If scancondpar.mfanangle(iMode, 2) = 0 Then 'ファン角scancondpar.mfanangle[2][iMode]のこと
        //メッセージ表示：
        //   寸法校正用スキャン後、条件が変わっています。
        //   再度寸法校正画像を収集してから、他の操作をせずにすぐに寸法校正を実行してください。
        //            MsgBox LoadResString(9331), vbExclamation
        //            Exit Function
        //        End If
        //    Else                    'コーンビーム時
        //        If scancondpar.theta0(ConeiMode) = 0 Then 'ファン角scancondpar.theta0(ConeiMode)のこと
        //メッセージ表示：
        //   寸法校正用スキャン後、条件が変わっています。
        //   再度寸法校正画像を収集してから、他の操作をせずにすぐに寸法校正を実行してください。
        //            MsgBox LoadResString(9331), vbExclamation
        //            Exit Function
        //        End If
        //    End If

        //GVal_Fcd_D = GVal_ScnAreaMax(iMode) / (2 * Sin((GVal_Mfanangle(2, iMode) * Pai / 180 / 2)))
        //GVal_Fcd_D = GVal_ScnAreaMax(iMode) / (2 * Sin(scancondpar.mfanangle(iMode, 2) * Pai / 180 / 2))

        //changed by 山本 2007-2-12
        //   If theInfoRec.bhc = 0 Then
        //       GVal_Fcd_D = GVal_ScnAreaMax(iMode) / (2 * Sin(scancondpar.mfanangle(iMode, 2) * Pai / 180 / 2))        '通常スキャン時の計算
        //   Else
        //       GVal_Fcd_D = GVal_ScnAreaMax(iMode) / (2 * Sin(scancondpar.theta0(ConeiMode) / 2))                      'コーンビーム時の計算
        //   End If

        //Image-Pro 画像データの取得
        //   If IMGPRODBG = 1 Then
        //       tmpReg.Left = 0
        //       tmpReg.Top = 0
        //       tmpReg.Right = Xsize_D - 1
        //       tmpReg.Bottom = Ysize_D - 1
        //       rc = IpDocGetArea(DOCSEL_ACTIVE, tmpReg, DISTANCE_IMAGE(0), CPROG)    '☆☆ユーザが作成した画像ﾃﾞｰﾀをのImage-Proの画像に書込む
        //       rc = IpHstEqualize(EQ_BESTFIT)
        //   End If

        //   DoDistanceCorrect = True

        //寸法校正フォームを隠す
        //   Me.hide

        //寸法校正結果フォームを表示する
        //   frmDistanceCorrectResult.Show vbModal

        //   Exit Function

        //Err_Process:
        //メッセージ表示：
        //   寸法校正ファントムの２値化抽出に失敗しました。
        //   事前に必要な校正を正しく行っていない可能性があります。
        //   幾何歪校正・回転中心校正を実行後、寸法校正を再度行ってください。
        //    MsgBox LoadResString(9523), vbExclamation

        //End Function

        #endregion
    }
}
