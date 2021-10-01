using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{
    public partial class frmDistanceCorrectPhantomFree : Form
    {
        // frmDistanceCorrectByCmModeのインスタンス
        private static frmDistanceCorrectPhantomFree _Instance = null;

        private Button[] btnRef;
        private TextBox[] txtImgFileName;
        private NumericUpDown[] cwne2R_CT;
        private NumericUpDown[] cwne2R_Measure;

        /// <summary>
        /// frmDistanceCorrectPhantomFreeのインスタンスを返す
        /// </summary>
        public static frmDistanceCorrectPhantomFree Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmDistanceCorrectPhantomFree();
                }

                return _Instance;
            }
        }

        public frmDistanceCorrectPhantomFree()
        {
            InitializeComponent();

            //コントロールの配列化
            btnRef = new Button[] {null, btnRef0, btnRef1 };
            txtImgFileName = new TextBox[]{null,txtImgFileName0,txtImgFileName1};
            cwne2R_CT = new NumericUpDown[] { null,cwne2R_ByCT0, cwne2R_ByCT1 };
            cwne2R_Measure = new NumericUpDown[] { null,cwne2R_ByMeasure0, cwne2R_ByMeasure1 };
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
        //履　　歴： V23.12  15/12/12   (検S1)長野   新規作成
        //*******************************************************************************
        private void frmDistanceCorrectByCmMode_Load(object sender, EventArgs e)
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

            lblTitle1_0.Text = CTResources.LoadResString(12105) + "１：";      //寸法校正用画像1：  'Rev16.2/17.0 ファントム2つに対応 2010/3/11 byやまおか
            lblTitle2_0.Text = CTResources.LoadResString(12105) + "２：";      //寸法校正用画像2：  'Rev16.2/17.0 ファントム2つに対応 2010/3/11 byやまおか

            frDist1.Text = CTResources.LoadResString(24000);
            lblTitle1_1.Text = CTResources.LoadResString(24001);
            lblTitle1_2.Text = CTResources.LoadResString(24002);
            
            frDist2.Text = CTResources.LoadResString(24000);
            lblTitle2_1.Text = CTResources.LoadResString(24001);
            lblTitle2_2.Text = CTResources.LoadResString(24002);
           
            //変更2014/10/07hata_v19.51反映
            fraFidFcdOffset.Text = StringTable.GetResString(StringTable.IDS_AxisOffset, CTSettings.gStrFidOrFdd + "/" + CTResources.LoadResString(StringTable.IDS_FCD)); //v18.00変更 IDS_Offset→IDS_AxisOffset byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //現在のコモン内容を取り出す
            ScanCorrect.OptValueGet_Cor();

            //寸法校正ファントム

            for (Index = 1; Index <= 2; Index++)
            {
                //変更2015/02/02hata_Max/Min範囲のチェック
                cwne2R_CT[Index].Value = modLibrary.CorrectInRange((decimal)ScanCorrect.GVal_DistVal[Index], cwne2R_CT[Index].Minimum, cwne2R_CT[Index].Maximum);
                cwne2R_Measure[Index].Value = modLibrary.CorrectInRange((decimal)ScanCorrect.GVal_DistValMeasure[Index], cwne2R_Measure[Index].Minimum, cwne2R_Measure[Index].Maximum);
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

            //標準校正ファントム以外を使用する機能のOn/Offに従い、タブの表示を制御
            if (CTSettings.scaninh.Data.distancecor_phantomfree == 0)
            {
 
            }
        }
        //*******************************************************************************
        //機　　能： OKボタンクリック時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.12  15/12/12   (検S1)長野   新規作成
        //*******************************************************************************
        private void cmdOK_Click(object sender, EventArgs e)
        {
            //Dim i As Integer   '//added by Isshiki
            int Index = 0;      //added by Isshiki

            //寸法校正ファントム     V4.0 append by 鈴山 2001/02/01

            //added by 一色
            for (Index = 1; Index <= 2; Index++)
            {
                ScanCorrect.GVal_DistVal[Index] = (float)cwne2R_CT[Index].Value;   //Rev16.2/17.0 修正 2010/02/23 by Iwasawa
                ScanCorrect.GVal_DistValMeasure[Index] = (float)cwne2R_Measure[Index].Value;   //Rev16.2/17.0 修正 2010/02/23 by Iwasawa
            }
            //

            //ボタンを使用不可にする
            cmdOK.Enabled = false;
            cmdEnd.Enabled = false;

            //マウスポインタを砂時計にする
            this.Cursor = Cursors.WaitCursor;

            if (DoDistanceCorrectByPhantomFree())
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
        //********************************************************************************
        //機    能  ：  寸法校正実行（寸法校正ファントムなし）
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：                   [ /O] Boolean   結果(True:正常,False:異常)
        //補    足  ：  ScanCorrect.bas の Get_DistanceCorrect_Parameter_Exをリニューアルした
        //
        //履    歴  ： V23.12  15/12/12   (検S1)長野   新規作成
        //            
        //********************************************************************************
        private bool DoDistanceCorrectByPhantomFree()
        {
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

            CTstr.IMAGEINFO[] theInfoRec = new CTstr.IMAGEINFO[3]; ;
            for (i = 0; i <= 2; i++)
            {
                theInfoRec[i].Initialize();
			}
           
			//戻り値初期化
            bool functionReturnValue = false;

			//Rev16.2/17.0 入力画像チェック 2つの画像で、FCDが異なる ＆ 同一の寸法校正状態で撮影されている(fcd_offsetとfid_offsetがそれぞれ等しい)　2010/02/24 by Iwasawa
			for (i = 1; i <= 2; i++)
            {
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

			//値を保存する配列を作成する。　　'//V16.2/17.0追加　2009/10/27 by Isshiki/Iwasawa
			for (i = 1; i <= 2; i++)
            {
				//付帯情報の回転選択可否が可の場合、付帯情報の回転選択の値（0:テーブル回転 1:Ｘ線管回転）を代入  'added by 間々田 2004/02/18
			    if (CTSettings.scaninh.Data.rotate_select == 0)
                {
					ScanCorrect.GFlg_MultiTube_D = (short)theInfoRec[i].rotate_select; //Rev16.2/17.0 変更 2010/02/25 by Iwasawa
				}
                else
                {
					//X線管(0:160kV,1:225kV)                     'added by 山本 2000-8-19 三菱対応
                    int.TryParse(theInfoRec[i].focus.GetString(), out ScanCorrect.GFlg_MultiTube_D);    //Rev16.2/17.0 変更 2010/02/25 by Iwasawa
				}

				//必ず０か１にするための措置 by 間々田 2006/01/18
                if (ScanCorrect.GFlg_MultiTube_D != 1)
                {
                    ScanCorrect.GFlg_MultiTube_D = 0;
                }

                fcd_offset[i] = theInfoRec[i].fcd_offset;                  //FCDのオフセット値
                fdd_offset[i] = theInfoRec[i].fid_offset;                  //FDDのオフセット値
		        mecaFCD[i] = theInfoRec[i].fcd - theInfoRec[i].fcd_offset;        //FCDメカの読値 修正 by Iwasawa
                mecaFDD[i] = theInfoRec[i].fid - theInfoRec[i].fid_offset;        //FCDメカの読値 修正 by Iwasawa
                FCD[i] = theInfoRec[i].fcd;                                //FCD値(ｵﾌｾｯﾄを含む) 修正 by Iwasawa
                FDD[i] = theInfoRec[i].fid;                                //FDD値(ｵﾌｾｯﾄを含む) 修正 by Iwasawa
          
                if(i == 1)
                {
                    ScanCorrect.GVal_Fcd1 = FCD[i];
                }
                else if(i == 2)
                {
                    ScanCorrect.GVal_Fcd2 = FCD[i];
                }

				functionReturnValue = true;
			}
		
			//CT画像を使って計測した値を代入
            d[1] = (float)cwne2R_CT[1].Value;
            d[2] = (float)cwne2R_CT[2].Value;

			//Rev16.2/17.0 オフセット値の誤差を計算する。 2つの画像でFCD,FDD,ファントム直径がそれぞれ異なっていても求まる計算式 by Isshiki
			//        2つの画像のｵﾌｾｯﾄを含むFCD,FDDをメカ読み値として計算し、求まったｵﾌｾｯﾄから元画像のｵﾌｾｯﾄ値を引く
            ScanCorrect.GVal_Fcd_O = FCD[1] * FCD[2] * (FDD[1] * (float)cwne2R_Measure[2].Value * (d[1] - (float)cwne2R_Measure[1].Value) - FDD[2] * (float)cwne2R_Measure[1].Value * (d[2] - (float)cwne2R_Measure[2].Value)) / (FCD[1] * FDD[2] * (float)cwne2R_Measure[1].Value * d[2] - FCD[2] * FDD[1] * d[1] * (float)cwne2R_Measure[2].Value);
            ScanCorrect.GVal_Fid_O = FDD[1] * FDD[2] * (FCD[1] * d[2] * (d[1] - (float)cwne2R_Measure[1].Value) - FCD[2] * d[1] * (d[2] - (float)cwne2R_Measure[2].Value)) / (FCD[1] * FDD[2] * (float)cwne2R_Measure[1].Value * d[2] - FCD[2] * FDD[1] * d[1] * (float)cwne2R_Measure[2].Value);

			ScanCorrect.GVal_Fcd_O = fcd_offset[2] + ScanCorrect.GVal_Fcd_O;
			ScanCorrect.GVal_Fid_O = fdd_offset[2] + ScanCorrect.GVal_Fid_O;

			//オフセット値を含んだFCD,FDDの値を計算　求まった値がFDD>FCDであることのチェック用なので1、2どちらの値を使っても良い
			ScanCorrect.GVal_Fcd_D = mecaFCD[2] + ScanCorrect.GVal_Fcd_O;
			ScanCorrect.GVal_Fid_D = mecaFDD[2] + ScanCorrect.GVal_Fid_O;

			//寸法校正結果フォームを表示する
			frmDistanceCorrectResultPhantomFree.Instance.ShowDialog();
			
            return functionReturnValue;

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
        //履　　歴： V23.12  15/12/12   (検S1)長野   新規作成
        //*******************************************************************************
        private void cmdEnd_Click(object sender, EventArgs e)
        {
            //寸法校正フォームをアンロードする
            this.Close();　   //復活2015/01/26hata　Dispose前にCloseする
            //Rev20.00 showdialogで呼ばれているのでdisposeを使ってリソース破棄 by長野 2014/12/04
            this.Dispose();
        }

        //*******************************************************************************
        //機　　能： 参照ボタンクリック時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.12  15/12/12   (検S1)長野   新規作成
        //*******************************************************************************
        private void btnRef_Click(object sender, EventArgs e)
        {
            int Index = -1;
            for (int i = 0; i < btnRef.Length; i++)
            {
                if (sender.Equals(btnRef[i]))
                {
                    Index = i;
                    break;
                }
            }

            //v16.20/v17.00変更(ここから) byやまおか 2010/03/04
            string FileName = null;
            ImageInfo theInfoRec = new ImageInfo();
            theInfoRec.Data.Initialize();

            FileName = modFileIO.GetFileName(StringTable.IDS_Select, CTResources.LoadResString(12105), ".img");

            if (!string.IsNullOrEmpty(FileName))
            {
                //Rev20.00 判定を逆に変更 by長野 2014/12/04
                if (!ImageInfo.ReadImageInfo(ref theInfoRec.Data, modLibrary.RemoveExtension(FileName, ".img")))
                {
                    //メッセージ表示：付帯情報のある画像ファイルを選択してください。
                    MessageBox.Show(CTResources.LoadResString(8127), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //v16.20/v17.00修正 byやまおか 2010/03/08
                if (string.Compare(modLibrary.RemoveNull(theInfoRec.Data.full_mode.GetString()), "FULL", true) != 0)
                {
                    //メッセージ表示：スキャンモードがフルスキャンの画像を選択してください。
                    MessageBox.Show(CTResources.LoadResString(9336), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                ////シングルの画像は不可とする
                //if (theInfoRec.Data.bhc != 1)
                //{
                //    //メッセージ表示：コーンビームの画像を選択してください。
                //    MessageBox.Show(CTResources.LoadResString(24003), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    return;
                //}

                txtImgFileName[Index].Text = FileName;
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
        //履　　歴： v23.12  15/12/11   (検S1)長野  新規作成
        //*******************************************************************************
        private void frmDistanceCorrectPhantomFree_FormClosed(object sender, FormClosedEventArgs e)
        {
			//終了時はフラグをリセット
			modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTScanCorrect);
        }
    }
}
