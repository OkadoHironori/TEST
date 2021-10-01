using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver9.7               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： 測定結果.frm                                                */
    ///* 処理概要　　： 指定された測定結果の文字列を表示する                        */
    ///* 注意事項　　： 寸法測定結果およびROI結果のフォームを１つにまとめた         */
    ///* -------------------------------------------------------------------------- */
    ///* ＯＳ　　　　： Windows XP Professional (SP1)                               */
    ///* コンパイラ　： VB 6.0 (SP5)                                                */
    ///* -------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
    ///*                                                                            */
    ///* V9.7        2004/11/01  (SI4)間々田         新規作成                       */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2004                 */
    ///* ************************************************************************** */
    public partial class frmResult : Form
    {
        #region インスタンスを返すプロパティ

        // frmResultのインスタンス
        private static frmResult _Instance = null;

        /// <summary>
        /// frmResultのインスタンスを返す
        /// </summary>
        public static frmResult Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmResult();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmResult()
        {
            InitializeComponent();
        }

        //'*******************************************************************************
        //'機　　能： フォームロード時の処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*******************************************************************************
        private void frmResult_Load(object sender, EventArgs e)
        {
			//英語環境時：文字を揃えるためにフォントをCourier Newにする
			if (modCT30K.IsEnglish)
            {
                txtResult.Font = new Font("Courier New", 9F);
				txtResult.Width = txtResult.Width + 16;
				
                //幅を広くする
				this.Width = this.Width + 16;
				this.Left = this.Left - 16;
			}

			//v17.60 英語用レイアウト調整 by長野 2011/05/25
			if (modCT30K.IsEnglish == true)
            {
				EnglishAdjustLayout();
			}

			//フォームを指定位置に移動
			modCT30K.SetPosNormalForm(this);

			//ROIメッセージの下に表示    'v15.10追加 byやまおか 2009/12/01
			this.Height = frmScanControl.Instance.Height;
			this.SetBounds(this.Left, frmRoiMessage.Instance.Top + frmRoiMessage.Instance.Height, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

			//フォームのキャプション
            //this.Text = StringTable.GetResString(StringTable.IDS_Result, frmScanImage.Instance.Toolbar1.Tag.ToString()); //～の結果
            string _tag = ((frmScanImage.Instance.Toolbar1.Tag == null) ? "" : frmScanImage.Instance.Toolbar1.Tag.ToString());   
            this.Text = StringTable.GetResString(StringTable.IDS_Result, _tag); //～の結果

			//ツールバー上の保存ボタン
            Toolbar1.Items["tsbtnSave"].ToolTipText = StringTable.GetResString(StringTable.IDS_Save, this.Text);             //～の保存

			switch (frmScanImage.Instance.ImageProc)
            {
				case frmScanImage.ImageProcType.roiProcessing:
					Toolbar1.Visible = true;    //ROI処理結果保存
					break;
				
                case frmScanImage.ImageProcType.RoiProfileDistance:
					Toolbar1.Visible = true;    //ﾌﾟﾛﾌｨｰﾙﾃﾞｨｽﾀﾝｽ結果保存
					break;
				
                case frmScanImage.ImageProcType.roiDistance:
					Toolbar1.Visible = true;    //寸法測定結果保存
					break;
				
                default:
					Toolbar1.Visible = false;
					break;
			}

			////フォームの表示
            //this.Show(frmCTMenu.Instance);    
        
        }

        //'*************************************************************************************************
        //'機　　能： フォームリサイズ時処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //'*************************************************************************************************
        private void frmResult_Resize(object sender, EventArgs e)
        {
            txtResult.Top = (Toolbar1.Visible ? Toolbar1.Height + 4 : 4);
            txtResult.Width = ClientRectangle.Width - 8;
            //txtResult.Height = ScaleHeight - 8
            txtResult.Height = ClientRectangle.Height - 8 - (Toolbar1.Visible ? Toolbar1.Height : 0);
        }

        //'*************************************************************************************************
        //'機　　能： ツールバー上のボタンクリック時処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //'*************************************************************************************************
        private void Save_Click(object sender, EventArgs e)
        {
            string SubExtension= null;
            string FileName;

			//サブ拡張子の設定
			switch (frmScanImage.Instance.ImageProc)
            {
				case frmScanImage.ImageProcType.roiProcessing:
                    SubExtension = CTResources.LoadResString(10400);  //-ROI結果
					break;
				case frmScanImage.ImageProcType.RoiProfileDistance:
                    SubExtension =  CTResources.LoadResString(10401); //-PRD結果
					break;
				case frmScanImage.ImageProcType.roiDistance:
					SubExtension = CTResources.LoadResString(10402);  //-寸法測定結果
					break;
                default:
                    break;
			}

			//保存ダイアログ処理
            //変更2015/01/22hata
			//FileName = modFileIO.GetFileName(StringTable.IDS_Save, this.Text, "", SubExtension, frmImageInfo.Instance.Target);
            FileName = modFileIO.GetFileName(StringTable.IDS_Save, this.Text, ".csv", SubExtension, frmImageInfo.Instance.Target);
            if (string.IsNullOrEmpty(FileName))
            {
				return;
            }

			switch (frmScanImage.Instance.ImageProc)
            {
				case frmScanImage.ImageProcType.roiProcessing:
					SaveRoiResult(FileName);    //ROI処理結果保存
					break;
				case frmScanImage.ImageProcType.RoiProfileDistance:
					SavePRDResult(FileName);    //ﾌﾟﾛﾌｨｰﾙﾃﾞｨｽﾀﾝｽ結果保存
					break;
				case frmScanImage.ImageProcType.roiDistance:
					SaveDistResult(FileName);   //寸法測定結果保存
					break;
                default:
                    break;
			}
        }

        //*************************************************************************************************
        //機　　能： ROI処理結果保存
        //
        //           変数名          [I/O] 型        内容
        //引　　数： FileName        [I/ ] String    保存ファイル名
        //戻 り 値：                 [ /O] Boolean   結果（True:成功、False:失敗）
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private bool SaveRoiResult(string FileName)
        {
            //int fileNo = 0;
            int i = 0;

            //変更2015/01/22hata
            //string RoiInfoStr = null;
            string RoiInfoStr = "";

            //戻り値初期化
            bool functionReturnValue = false;

            StreamWriter sw = null;

            try
            {
                //ファイルオープン (これまでの内容は消去され上書きするモード)
                //変更2015/01/22hata
                //sw = new StreamWriter(FileName, false);
                sw = new StreamWriter(FileName, false, Encoding.GetEncoding("shift-jis"));

                //ヘッダの書き込み：番号,平均値,標準偏差,面積(mm),ROI形状,ROI座標
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_Number),
                                                  CTResources.LoadResString(12617), 
                                                  CTResources.LoadResString(12618),
                                                  CTResources.LoadResString(12619),
                                                  CTResources.LoadResString(StringTable.IDS_RoiShape),
                                                  CTResources.LoadResString(StringTable.IDS_RoiCoordinate)));
                //ROI個数分書き込み
                for (i = 1; i <= DrawRoi.roi.NumOfRois; i++)
                {
                    //ROI情報をコモンにセットします(RoiInfoStrに文字列をセットするためコール)
                    DrawRoi.SetROIToCommon(i, ref RoiInfoStr);

                    //結果保存
                    sw.WriteLine(modLibrary.GetCsvRec(i.ToString(), modImgProc.Ave[i].ToString("0.00"),
                                                                    modImgProc.Sd[i].ToString("0.00"),
                                                                    modImgProc.area[i].ToString("0.00"), RoiInfoStr));
                }

                //フッタの書き込み：コメントについては改行コードがあればスペースに置換する
                sw.WriteLine(CTResources.LoadResString(StringTable.IDS_RoiCount) + "," + Convert.ToString(DrawRoi.roi.NumOfRois));    //ROI個数
                sw.WriteLine(CTResources.LoadResString(StringTable.IDS_SliceName) + "," + frmImageInfo.Instance.SliceName);           //スライス名
                sw.WriteLine(CTResources.LoadResString(StringTable.IDS_Date) + "," + DateTime.Now.ToString("yyyy/MM/dd"));            //年月日（現在の日付）
                sw.WriteLine(CTResources.LoadResString(StringTable.IDS_SlicePos) + "," + frmImageInfo.Instance.SlicePos);             //スライス位置
                sw.WriteLine(CTResources.LoadResString(StringTable.IDS_Comment) + "," + modLibrary.RemoveCRLF(DrawRoi.roi.Comment));  //コメント
            }
            catch (Exception exp)
            {
                //エラーメッセージ表示
                MessageBox.Show(exp.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                //ファイルクローズ
                if (sw != null)
                {
                    sw.Close();
                    sw = null;
                }
            }

            //戻り値セット
            functionReturnValue = true;
            return functionReturnValue;
        }


        //*************************************************************************************************
        //機　　能： 寸法測定結果保存
        //
        //           変数名          [I/O] 型        内容
        //引　　数： FileName        [I/ ] String    保存ファイル名
        //戻 り 値：                 [ /O] Boolean   結果（True:成功、False:失敗）
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private bool SaveDistResult(string FileName)
        {
            //int fileNo = 0;
            int i = 0;
            int K = 0;

            //戻り値初期化
            bool functionReturnValue = false;

            //2048画素表示の場合を考慮するための係数kの取得
            //v16.10 4096対応に伴い，Kの決め方を変更 by 長野 2010/02/16
            //K = IIf(frmScanImage.hsbImage.Visible, 2, 1)
            int PicWidth = 0;
            PicWidth = frmScanImage.Instance.PicWidth;

            if (frmScanImage.Instance.hsbImage.Visible == true)
            {
                switch ((PicWidth))
                {
                    case 2048:
                        K = 2;
                        break;
                    case 4096:
                        K = 4;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                K = 1;
            }

            StreamWriter sw = null;

            try
            {
                //ファイルオープン
                //変更2015/01/22hata
                //sw = new StreamWriter(FileName, false);
                sw = new StreamWriter(FileName, false, Encoding.GetEncoding("shift-jis"));

                //ヘッダの書き込み：番号,長さ(mm),Ｘθ,Ｙθ,X1,Y1,X2,Y2
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_Number), 
                                                  CTResources.LoadResString(StringTable.IDS_Length), 
                                                  CTResources.LoadResString(StringTable.IDS_Xtheta), 
                                                  CTResources.LoadResString(StringTable.IDS_Ytheta), 
                                                  "X1", "Y1", "X2", "Y2"));
                //結果保存
                for (i = modImgProc.DistanceInfo.GetLowerBound(0); i <= modImgProc.DistanceInfo.GetUpperBound(0); i++)
                {
                    //変更2015/01/22hata
                    //2014/11/07hata キャストの修正
                    //sw.WriteLine(modLibrary.GetCsvRec(i.ToString(), modImgProc.DistanceInfo[i].Dist.ToString("0.#000"),
                    //                                                modImgProc.DistanceInfo[i].AngleX.ToString("0.00"),
                    //                                                modImgProc.DistanceInfo[i].AngleY.ToString("0.00"),
                    //                                               (modImgProc.DistanceInfo[i].x1 / K).ToString(), 
                    //                                               (modImgProc.DistanceInfo[i].y1 / K).ToString(), 
                    //                                               (modImgProc.DistanceInfo[i].x2 / K).ToString(), 
                    //                                               (modImgProc.DistanceInfo[i].y2 / K).ToString()));
                    //sw.WriteLine(modLibrary.GetCsvRec(i.ToString(), modImgProc.DistanceInfo[i].Dist.ToString("0.#000"),
                    //                                               modImgProc.DistanceInfo[i].AngleX.ToString("0.00"),
                    //                                               modImgProc.DistanceInfo[i].AngleY.ToString("0.00"),
                    //                                              ((double)modImgProc.DistanceInfo[i].x1 / K).ToString(),
                    //                                              ((double)modImgProc.DistanceInfo[i].y1 / K).ToString(),
                    //                                              ((double)modImgProc.DistanceInfo[i].x2 / K).ToString(),
                    //                                              ((double)modImgProc.DistanceInfo[i].y2 / K).ToString()));
                    sw.WriteLine(modLibrary.GetCsvRec((i + 1).ToString(), modImgProc.DistanceInfo[i].Dist.ToString("0.#000"),
                                                                    modImgProc.DistanceInfo[i].AngleX.ToString("0.00"),
                                                                    modImgProc.DistanceInfo[i].AngleY.ToString("0.00"),
                                                                   ((double)modImgProc.DistanceInfo[i].x1 / K).ToString(),
                                                                   ((double)modImgProc.DistanceInfo[i].y1 / K).ToString(),
                                                                   ((double)modImgProc.DistanceInfo[i].x2 / K).ToString(),
                                                                   ((double)modImgProc.DistanceInfo[i].y2 / K).ToString()));
                    
               }

                //フッタの書き込み：コメントについては改行コードがあればスペースに置換する
                sw.WriteLine(CTResources.LoadResString(StringTable.IDS_LineCount) + "," + Convert.ToString(DrawRoi.roi.NumOfRois));   //線分数
                sw.WriteLine(CTResources.LoadResString(StringTable.IDS_SliceName) + "," + frmImageInfo.Instance.SliceName);           //スライス名
                sw.WriteLine(CTResources.LoadResString(StringTable.IDS_Date) + "," + DateTime.Now.ToString("yyyy/MM/dd"));            //年月日(現在の日付)
                sw.WriteLine(CTResources.LoadResString(StringTable.IDS_SlicePos) + "," + frmImageInfo.Instance.SlicePos);             //スライス位置
                sw.WriteLine(CTResources.LoadResString(StringTable.IDS_Comment) + "," + modLibrary.RemoveCRLF(DrawRoi.roi.Comment));  //コメント
            }
            catch (Exception exp)
            {
                //エラーメッセージ表示
                MessageBox.Show(exp.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                //ファイルクローズ
                if (sw != null)
                {
                    sw.Close();
                    sw = null;
                }
            }

            //戻り値セット
            functionReturnValue = true;
            return functionReturnValue;
        }

        //*************************************************************************************************
        //機　　能： プロフィールディスタンス結果保存
        //
        //           変数名          [I/O] 型        内容
        //引　　数： FileName        [I/ ] String    保存ファイル名
        //戻 り 値：                 [ /O] Boolean   結果（True:成功、False:失敗）
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private bool SavePRDResult(string FileName)
        {
            double[] TempXS = new double[201];
            double[] TempYS = new double[201];
            double[] TempXE = new double[201];
            double[] TempYE = new double[201];
            int[] TempLow = new int[201];
            int[] TempUp = new int[201];
            int[] TempUnit = new int[201];
            double[] TempDist = new double[201];
            double[] TempAngleX = new double[201];
            double[] TempAngleY = new double[201];

            //int fileNo = 0;
            int cnt = 0;
            int DistNum = 0;
            //v10.2追加 by 間々田 2005/07/14

            //戻り値初期化
            bool functionReturnValue = false;

            int m = 0;
            //m = IIf((.PicWidth = 2048) And (.Magnify = 1), 2, 1)
            //m = IIf(.PicWidth = 2048, 2, 1)                         'v10.2変更 by 間々田 2005/06/28
            //v16.10 4096対応に伴い，mの決め方を変更する。by 長野 2010/02/10
            switch (frmScanImage.Instance.PicWidth)
            {
                case 2048:
                    m = 2;
                    break;
                case 4096:
                    m = 4;
                    break;
                default:
                    m = 1;
                    break;
            }

            //エラー時の扱い
            // ERROR: Not supported in C#: OnErrorStatement


            //プロフィールディスタンスの測定結果ファイルの読み込み
            //ProcOCX1.LoadPRDTemp TempXS(0), TempYS(0), TempXE(0), TempYE(0), _
            //'                     TempLow(0), TempUp(0), TempUnit(0), TempLow1(0), TempUp1(0), _
            //'                     TempDist(0), TempAngleX(0), TempAngleY(0)

            //v10.2変更 by 間々田 2005/07/14
            DistNum = ImgProc.PRDTempLoad(ref TempXS[0], ref TempYS[0], ref TempXE[0], ref TempYE[0], 
                                          ref TempLow[0], ref TempUp[0], ref TempUnit[0], ref TempDist[0], ref TempAngleX[0], ref TempAngleY[0]);


            //ファイルオープン
            StreamWriter sw = null;

            try
            {
                //変更2015/01/22hata
                //sw = new StreamWriter(FileName, false);
                sw = new StreamWriter(FileName, false, Encoding.GetEncoding("shift-jis"));

                //ヘッダの書き込み：番号,長さ(mm),指定点数,Ｘθ,Ｙθ,X1,Y1,X2,Y2,下限閾値,上限閾値,連結幅,１点指定下限閾値,１点指定上限閾値
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_Number),
                                                  CTResources.LoadResString(StringTable.IDS_Length),
                                                  CTResources.LoadResString(StringTable.IDS_PointCount),
                                                  CTResources.LoadResString(StringTable.IDS_Xtheta),
                                                  CTResources.LoadResString(StringTable.IDS_Ytheta),
                                                  "X1", "Y1", "X2", "Y2",
                                                  CTResources.LoadResString(StringTable.IDS_MinThreshold),
                                                  CTResources.LoadResString(StringTable.IDS_MaxThreshold),
                                                  CTResources.LoadResString(StringTable.IDS_ConnectionWidth),
                                                  CTResources.LoadResString(StringTable.IDS_OnePointLower),
                                                  CTResources.LoadResString(StringTable.IDS_OnePointUpper)));

                for (cnt = 0; cnt <= DistNum - 1; cnt++)
                {
                    //Print #fileNo, CStr(cnt + 1) & "," & _
                    //'               Right$("   " & Format$(TempDist(cnt) * frmScanImage.Magnify / 1000, "0.00"), 7) & "," & _
                    //'               CStr(PRDPoint) & "," & _
                    //'               Right$("   " & Format$(TempAngleX(cnt), "0.00"), 7) & "," & _
                    //'               Right$("   " & Format$(TempAngleY(cnt), "0.00"), 7) & "," & _
                    //'               CStr(TempXS(cnt) / m) & "," & _
                    //'               CStr(TempYS(cnt) / m) & "," & _
                    //'               CStr(TempXE(cnt) / m) & "," & _
                    //'               CStr(TempYE(cnt) / m) & "," & _
                    //'               CStr(CT_Low) & "," & _
                    //'               CStr(CT_High) & "," & _
                    //'               CStr(TempUnit(cnt)) & "," & _
                    //'               CStr(CT_Low1Point) & "," & _
                    //'               CStr(CT_High1Point)

                    sw.WriteLine(modLibrary.GetCsvRec((cnt + 1).ToString(),
                                                      (TempDist[cnt] / 1000).ToString("0.#000"),
                                                      modImgProc.PRDPoint.ToString(),
                                                      TempAngleX[cnt].ToString("0.00"),
                                                      TempAngleY[cnt].ToString("0.00"),
                                                      (TempXS[cnt] / m).ToString(),
                                                      (TempYS[cnt] / m).ToString(),
                                                      (TempXE[cnt] / m).ToString(),
                                                      (TempYE[cnt] / m).ToString(),
                                                      modImgProc.CT_Low.ToString(),
                                                      modImgProc.CT_High.ToString(),
                                                      TempUnit[cnt].ToString(),
                                                      modImgProc.CT_Low1Point.ToString(),
                                                      modImgProc.CT_High1Point.ToString()));
                }

                //フッタの書き込み：コメントについては改行コードがあればスペースに置換する
                sw.WriteLine(CTResources.LoadResString(StringTable.IDS_LineCount) + "," + Convert.ToString(DistNum));                 //線分数
                sw.WriteLine(CTResources.LoadResString(StringTable.IDS_SliceName) + "," + frmImageInfo.Instance.SliceName);           //スライス名
                sw.WriteLine(CTResources.LoadResString(StringTable.IDS_Date) + "," + DateTime.Now.ToString("yyyy/MM/dd"));            //年月日
                sw.WriteLine(CTResources.LoadResString(StringTable.IDS_SlicePos) + "," + frmImageInfo.Instance.SlicePos);             //スライス位置
                sw.WriteLine(CTResources.LoadResString(StringTable.IDS_Comment) + "," + modLibrary.RemoveCRLF(DrawRoi.roi.Comment));  //コメント
            }
            catch (Exception exp)
            {
                //エラーメッセージ表示
                MessageBox.Show(exp.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                //ファイルクローズ
                if (sw != null)
                {
                    sw.Close();
                    sw = null;
                }
            }

            //戻り値セット
            functionReturnValue = true;
            return functionReturnValue;
        }

        //*******************************************************************************
        //機　　能： 英語用レイアウト調整
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V17.60  11/05/25  (検S１)長野      新規作成
        //*******************************************************************************
        private void EnglishAdjustLayout()
        {
            //2014/11/07hata キャストの修正
            //this.Width = (int)(this.Width * 1.2);
            this.Width = Convert.ToInt32(this.Width * 1.2);
        }

        // テキストBoxに書き込む
        public void SetText(string text)
        {
            if (!this.Visible)
                this.Show(frmCTMenu.Instance);
            
            txtResult.Text = text;
            
        }

        //追加2014/12/22hata_dnet
        private void Toolbar1_MouseEnter(object sender, EventArgs e)
        {
            Toolbar1.Focus(); 
        }

    }
}
