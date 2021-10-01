using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
	///* プログラム名： FormMaint.frm                                               */
	///* 処理概要　　： メンテナンス                                                */
	///* 注意事項　　： なし                                                        */
	///* -------------------------------------------------------------------------- */
	///* 適用計算機　： DOS/V PC                                                    */
	///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
	///* コンパイラ　： VB 6.0                                                      */
	///* -------------------------------------------------------------------------- */
	///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
	///*                                                                            */
	///* V1.00       99/XX/XX    (TOSFEC) ????????   新規作成                       */
	///* V3.0        00/08/01    (TOSFEC) 鈴山　修   ｺｰﾝﾋﾞｰﾑCT対応                  */
	///* V3.0        00/09/08    (TOSFEC) 鈴山　修   "Option Explicit"を指定        */
	///*                                                                            */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
	///* ************************************************************************** */
	public partial class frmMaint : Form
	{

		//********************************************************************************
		//  共通データ宣言
		//********************************************************************************
        //追加2014/10/07hata_v19.51反映
        //v18.00追加 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        private const short FFT_SIZE = 8192;

        //変更2014/10/07hata_v19.51反映
        //private float[] FC1 = new float[4096];		//FC1ﾊﾞｯﾌｧ
        //private float[] FC2 = new float[4096];		//FC2ﾊﾞｯﾌｧ
        //private float[] FC3 = new float[4096];		//FC3ﾊﾞｯﾌｧ
        //private float[] FC4 = new float[4096];		//FC4ﾊﾞｯﾌｧ
        //private float[] FC5 = new float[4096];		//FC5ﾊﾞｯﾌｧ
        //private float[] FC6 = new float[4096];		//FC6ﾊﾞｯﾌｧ
        //private float[] FC7 = new float[4096];		//FC7ﾊﾞｯﾌｧ
        //private float[] FC8 = new float[4096];		//FC8ﾊﾞｯﾌｧ
        //private float[] FC9 = new float[4096];		//FC9ﾊﾞｯﾌｧ
        ////V13.00追加 2007/02/20 やまおか
        //private float[] FC1n = new float[4096];		//FC1nﾊﾞｯﾌｧ
        //private float[] FC2n = new float[4096];		//FC2nﾊﾞｯﾌｧ
        //private float[] FC3n = new float[4096];		//FC3nﾊﾞｯﾌｧ
        //private float[] FC4n = new float[4096];		//FC4nﾊﾞｯﾌｧ
        //private float[] FC5n = new float[4096];		//FC5nﾊﾞｯﾌｧ
        //private float[] FC6n = new float[4096];		//FC6nﾊﾞｯﾌｧ
        //private float[] FC7n = new float[4096];		//FC7nﾊﾞｯﾌｧ
        //private float[] FC8n = new float[4096];		//FC8nﾊﾞｯﾌｧ
        //private float[] FC9n = new float[4096];		//FC9nﾊﾞｯﾌｧ
        private float[] FC1 = new float[FFT_SIZE];		//FC1ﾊﾞｯﾌｧ
        private float[] FC2 = new float[FFT_SIZE];		//FC2ﾊﾞｯﾌｧ
        private float[] FC3 = new float[FFT_SIZE];		//FC3ﾊﾞｯﾌｧ
        private float[] FC4 = new float[FFT_SIZE];		//FC4ﾊﾞｯﾌｧ
        private float[] FC5 = new float[FFT_SIZE];		//FC5ﾊﾞｯﾌｧ
        private float[] FC6 = new float[FFT_SIZE];		//FC6ﾊﾞｯﾌｧ
        private float[] FC7 = new float[FFT_SIZE];		//FC7ﾊﾞｯﾌｧ
        private float[] FC8 = new float[FFT_SIZE];		//FC8ﾊﾞｯﾌｧ
        private float[] FC9 = new float[FFT_SIZE];		//FC9ﾊﾞｯﾌｧ
        private float[] FC10 = new float[FFT_SIZE];     //FC10ﾊﾞｯﾌｧ  'v18.00追加 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        private float[] FC11 = new float[FFT_SIZE];     //FC11ﾊﾞｯﾌｧ  'v18.00追加 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        private float[] FC12 = new float[FFT_SIZE];     //FC12ﾊﾞｯﾌｧ  'v18.00追加 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

        private float[] FC1n = new float[FFT_SIZE];		//FC1nﾊﾞｯﾌｧ
        private float[] FC2n = new float[FFT_SIZE];		//FC2nﾊﾞｯﾌｧ
        private float[] FC3n = new float[FFT_SIZE];		//FC3nﾊﾞｯﾌｧ
        private float[] FC4n = new float[FFT_SIZE];		//FC4nﾊﾞｯﾌｧ
        private float[] FC5n = new float[FFT_SIZE];		//FC5nﾊﾞｯﾌｧ
        private float[] FC6n = new float[FFT_SIZE];		//FC6nﾊﾞｯﾌｧ
        private float[] FC7n = new float[FFT_SIZE];		//FC7nﾊﾞｯﾌｧ
        private float[] FC8n = new float[FFT_SIZE];		//FC8nﾊﾞｯﾌｧ
        private float[] FC9n = new float[FFT_SIZE];		//FC9nﾊﾞｯﾌｧ
        private float[] FC10n = new float[FFT_SIZE];    //FC10nﾊﾞｯﾌｧ  'v18.00追加 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        private float[] FC11n = new float[FFT_SIZE];    //FC11nﾊﾞｯﾌｧ  'v18.00追加 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        private float[] FC12n = new float[FFT_SIZE];    //FC12nﾊﾞｯﾌｧ  'v18.00追加 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

		private int FCsetflg = 0;		//FC設定ﾌﾗｸﾞ  1:設定   0:未設定

		//V6.0 append by 間々田 2002/07/10 START
		private float[] FCD= new float[6];			//ＦＣＤ位置（ｍｍ）
		private float y_incli= 0;					//Ｙ軸傾斜角度
		//V6.0 append by 間々田 2002/07/10 END

		//V7.0 append by 巻渕 2002/12/09 START
		private float x_offset = 0;					//Ｘオフセット（ｍｍ）
		private float ref_fid = 0;					//基準fid（ｍｍ）
		//V7.0 append by 巻渕 2002/12/09 END

        //追加2014/10/07hata_v19.51反映
        public enum enDetShift
        {
            //v18.xx追加 byやまかげ byやまおか 2011/02/04
            Base = 0,
            Shift = 1
        }
        //v18.xx追加 byやまかげ byやまおか 2011/02/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        private const double JudgeRange = 0.1;


		private TextBox[] txtFCD = null;
        private Button[] cmdRef = null;                 //追加2014/10/07hata_v19.51反映
        private TextBox[] txtFilepath = null;           //追加2014/10/07hata_v19.51反映
        private RadioButton[] optWarmupMode = null;     //追加2014/10/07hata_v19.51反映
        private RadioButton[] optMaxVoltDisp = null;    //追加2014/10/07hata_v19.51反映

        private modMechaControl.MyCallbackDelegate myCallback = null;

		private static frmMaint _Instance = null;

		public frmMaint()
		{
			InitializeComponent();

			txtFCD = new TextBox[] { txtFCD0, txtFCD1, txtFCD2, txtFCD3, txtFCD4, txtFCD5 };

            //追加2014/10/07hata_v19.51反映
            cmdRef = new Button[] { _cmdRef_0, _cmdRef_1 };
            txtFilepath = new TextBox[] { _txtFilepath_0, _txtFilepath_1};
            optWarmupMode = new RadioButton[] { _optWarmupMode_0, _optWarmupMode_1, _optWarmupMode_2, _optWarmupMode_3, _optWarmupMode_4, _optWarmupMode_5, _optWarmupMode_6, _optWarmupMode_7 };
            optMaxVoltDisp = new RadioButton[] { _optMaxVoltDisp_0, _optMaxVoltDisp_1};

            //デリゲートを設定
            if (myCallback != null)
                myCallback = new modMechaControl.MyCallbackDelegate(modMechaControl.MyCallback);

        }

		public static frmMaint Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmMaint();
				}

				return _Instance;
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
		//              v7.00  03/07/11  (SI4)間々田     Private化
		//********************************************************************************
		//Private Sub FcRead()
		private void FcRead(string FileName, ref float[] theFC)				//v11.2変更 by 間々田 2006/01/12
		{
			//v11.2削除ここまで by 間々田 2006/01/12
			//Open "C:\Ct\filter\FC1.dat" For Binary Access Read Lock Write As #1
			//Open "C:\Ct\filter\FC2.dat" For Binary Access Read Lock Write As #2
			//Open "C:\Ct\filter\FC3.dat" For Binary Access Read Lock Write As #3
			//Open "C:\Ct\filter\FC4.dat" For Binary Access Read Lock Write As #4
			//Open "C:\Ct\filter\FC5.dat" For Binary Access Read Lock Write As #5
			//Open "C:\Ct\filter\FC6.dat" For Binary Access Read Lock Write As #6
			//Open "C:\Ct\filter\FC7.dat" For Binary Access Read Lock Write As #7
			//Open "C:\Ct\filter\FC8.dat" For Binary Access Read Lock Write As #8
			//Open "C:\Ct\filter\FC9.dat" For Binary Access Read Lock Write As #9
			//
			//
			//Get #1, , FC1
			//Get #2, , FC2
			//Get #3, , FC3
			//Get #4, , FC4
			//Get #5, , FC5
			//Get #6, , FC6
			//Get #7, , FC7
			//Get #8, , FC8
			//Get #9, , FC9
			//
			//Close #1
			//Close #2
			//Close #3
			//Close #4
			//Close #5
			//Close #6
			//Close #7
			//Close #8
			//Close #9
			//v11.2削除ここまで by 間々田 2006/01/12

			//v11.2追加ここから by 間々田 2006/01/12
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
			/*
			Dim fileNo      As Integer

			'エラー時の扱い
			On Error GoTo ErrorHandler

			'ファイルオープン
			fileNo = FreeFile()
			Open FileName For Binary Access Read Lock Write As #fileNo

			'データ取得
			Get #fileNo, , theFC

			'ファイルクローズ
			Close #fileNo
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			FileStream fs = null;
			BinaryReader br = null;

			try 
			{
				//ファイルオープン
				fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				br = new BinaryReader(fs);

				//データ取得
				int index = 0;
				while (fs.Position < fs.Length)
				{
					theFC[index] = br.ReadSingle();
					index++;
				}
			}
			catch (Exception ex)
			{
				//エラーメッセージの表示：以下のファイルの読み込み時にエラーが発生しました。
				MessageBox.Show(CTResources.LoadResString(9965) + "\r" + "\r" + FileName + "\r" + "\r" + ex.Message, 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);			
			}
			finally
			{
				//ファイルクローズ
				if (br != null)
				{
					br.Close();
					br = null;
				}

				if (fs != null)
				{
					fs.Close();
					fs = null;
				}
			}
			//v11.2追加ここまで by 間々田 2006/01/12
		}


		//********************************************************************************
		//機    能  ：  FC用Binaryﾌｧｲﾙ作成
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
		//              v7.00  03/07/11  (SI4)間々田     Private化
		//********************************************************************************
		//Private Sub FcWrite()
		private void FcWrite(string FileName, float[] theFC)				//v11.2変更 by 間々田 2006/01/12
		{
			//v11.2削除ここから by 間々田 2006/01/12
			//On Error GoTo FileError
			//
			//Open "C:\Ct\filter\FC1.dat" For Binary Access Write Lock Write As #1
			//Open "C:\Ct\filter\FC2.dat" For Binary Access Write Lock Write As #2
			//Open "C:\Ct\filter\FC3.dat" For Binary Access Write Lock Write As #3
			//Open "C:\Ct\filter\FC4.dat" For Binary Access Write Lock Write As #4
			//Open "C:\Ct\filter\FC5.dat" For Binary Access Write Lock Write As #5
			//Open "C:\Ct\filter\FC6.dat" For Binary Access Write Lock Write As #6
			//Open "C:\Ct\filter\FC7.dat" For Binary Access Write Lock Write As #7
			//Open "C:\Ct\filter\FC8.dat" For Binary Access Write Lock Write As #8
			//Open "C:\Ct\filter\FC9.dat" For Binary Access Write Lock Write As #9
			//
			//    Put #1, , FC1
			//    Put #2, , FC2
			//    Put #3, , FC3
			//    Put #4, , FC4
			//    Put #5, , FC5
			//    Put #6, , FC6
			//    Put #7, , FC7
			//    Put #8, , FC8
			//    Put #9, , FC9
			//
			//Close #1
			//Close #2
			//Close #3
			//Close #4
			//Close #5
			//Close #6
			//Close #7
			//Close #8
			//Close #9
			//'Close #4
			//
			//Exit Sub
			//
			//FileError:
			//'    MsgBox "ﾌｧｲﾙｴﾗｰ", vbExclamation, "ﾌｨﾙﾀ関数"    'ﾌｧｲﾙｴﾗｰﾒｯｾｰｼﾞ
			//MsgBox Err.Description, vbExclamation               'v7.0 change by 間々田 2003/08/12
			//Resume Next
			//v11.2削除ここまで by 間々田 2006/01/12

			//v11.2追加ここから by 間々田 2006/01/12
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
			/*
			Dim fileNo      As Integer

			'エラー時の扱い
			On Error GoTo ErrorHandler

			'ファイルオープン
			fileNo = FreeFile()
			Open FileName For Binary Access Write Lock Write As #fileNo

			'データ取得
			Put #fileNo, , theFC

			'ファイルクローズ
			Close #fileNo
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			FileStream fs = null;
			BinaryWriter bw = null;

			try 
			{
				//ファイルオープン
				fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write);
				bw = new BinaryWriter(fs);

				//データ取得
				foreach (float theFCitem in theFC)
				{
					bw.Write(theFCitem);
				}
			}
			catch (Exception ex)
			{
				//エラーメッセージの表示：以下のファイルの書き込み時にエラーが発生しました。
				MessageBox.Show(CTResources.LoadResString(9968) + FileName + "\r" + "\r" +ex.Message, 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				//ファイルクローズ
				if (bw != null)
				{
					bw.Close();
					bw = null;
				}

				if (fs != null)
				{
					fs.Close();
					fs = null;
				}
			}
			//v11.2追加ここまで by 間々田 2006/01/12
		}


		//********************************************************************************
		//機    能  ：  Filter関数ｸﾞﾗﾌ表示する
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V1.00  99/09/14  J.IWASAWA       初版
		//                     00/02/17  J.IWASAWA       ｵﾌｾｯﾄｽｷｬﾝ、高解像度ｶﾒﾗ対応(FC4～FC6追加)
		//                     03/07/11  (SI4)間々田     Private化
		//********************************************************************************
		private void FcGraph()			//ﾌｨﾙﾀFC1,2,3をｸﾞﾗﾌ表示
		{
			int i = 0;				//ﾙｰﾌﾟｶｳﾝﾀ
            
            //変更2014/10/07hata_v19.51反映
            //const int we = 4095;
            const int we = FFT_SIZE - 1;        //v18.00変更 4095→FFT_SIZE-1 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05


#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			'グラフをいったん消去
			picFilter.Cls

			'座標系変更
			picFilter.Scale (0, 2)-(we, 0)
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//グラフをいったん消去
            if (picFilter.Image != null)
            {
                picFilter.Image.Dispose();
                picFilter.Image = null;
            }

			// グラフを描画する Graphics オブジェクトを作成
			picFilter.Image = new Bitmap(picFilter.Width, picFilter.Height);
			Graphics g = Graphics.FromImage(picFilter.Image);


			//座標系変更

			int x1 = 0;
			int y1 = 2;
			int x2 = we;
			int y2 = 0;
			float dX = x2 - x1;
			float dY = y2 - y1;

			// グラフのプロットを PictureBox上の座標に変換する係数
			float paramX = (picFilter.ClientSize.Width - 1) / dX;
			float paramY = (picFilter.ClientSize.Height - 1) / dY;

            g.ResetTransform();

			// 座標系に合せてグラフの原点を変更する
			g.TranslateTransform(dX < 0 ? (picFilter.ClientSize.Width - 1) * Math.Abs(x1 / dX) : 0,
								 dY < 0 ? (picFilter.ClientSize.Height - 1) * Math.Abs(y1 / dY) : 0);


			Pen penLine1 = new Pen(Line1.BackColor);
			Pen penLine2 = new Pen(Line2.BackColor);
			Pen penLine3 = new Pen(Line3.BackColor);
			Pen penLine4 = new Pen(Line4.BackColor);
			Pen penLine5 = new Pen(Line5.BackColor);
			Pen penLine6 = new Pen(Line6.BackColor);
			Pen penLine7 = new Pen(Line7.BackColor);
			Pen penLine8 = new Pen(Line8.BackColor);
			Pen penLine9 = new Pen(Line9.BackColor);
			Pen penLine10 = new Pen(Line10.BackColor);//追加2014/10/07hata_v19.51反映
			Pen penLine11 = new Pen(Line11.BackColor);//追加2014/10/07hata_v19.51反映
			Pen penLine12 = new Pen(Line12.BackColor);//追加2014/10/07hata_v19.51反映

            //変更2014/10/07hata_v19.51反映
            //for (i = 1; i <= we; i++)
            //{
            //    g.DrawLine(penLine1, paramX * (i - 1), paramY * FC1[i - 1], paramX * i, paramY * FC1[i]);
            //    g.DrawLine(penLine2, paramX * (i - 1), paramY * FC2[i - 1], paramX * i, paramY * FC2[i]);
            //    g.DrawLine(penLine3, paramX * (i - 1), paramY * FC3[i - 1], paramX * i, paramY * FC3[i]);
            //    g.DrawLine(penLine4, paramX * (i - 1), paramY * FC4[i - 1], paramX * i, paramY * FC4[i]);
            //    g.DrawLine(penLine5, paramX * (i - 1), paramY * FC5[i - 1], paramX * i, paramY * FC5[i]);
            //    g.DrawLine(penLine6, paramX * (i - 1), paramY * FC6[i - 1], paramX * i, paramY * FC6[i]);
            //    g.DrawLine(penLine7, paramX * (i - 1), paramY * FC7[i - 1], paramX * i, paramY * FC7[i]);
            //    g.DrawLine(penLine8, paramX * (i - 1), paramY * FC8[i - 1], paramX * i, paramY * FC8[i]);
            //    g.DrawLine(penLine9, paramX * (i - 1), paramY * FC9[i - 1], paramX * i, paramY * FC9[i]);
            //}
            //終端が0でないFC関数のときはFC*nの方を採用する  'v18.00追加ここから 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (CTSettings.scaninh.Data.fcend_not0 == 0)
            {
                for (i = 1; i <= we; i++)
                {
                    g.DrawLine(penLine1, paramX * (i - 1), paramY * FC1n[i - 1], paramX * i, paramY * FC1n[i]);
                    g.DrawLine(penLine2, paramX * (i - 1), paramY * FC2n[i - 1], paramX * i, paramY * FC2n[i]);
                    g.DrawLine(penLine3, paramX * (i - 1), paramY * FC3n[i - 1], paramX * i, paramY * FC3n[i]);
                    g.DrawLine(penLine4, paramX * (i - 1), paramY * FC4n[i - 1], paramX * i, paramY * FC4n[i]);
                    g.DrawLine(penLine5, paramX * (i - 1), paramY * FC5n[i - 1], paramX * i, paramY * FC5n[i]);
                    g.DrawLine(penLine6, paramX * (i - 1), paramY * FC6n[i - 1], paramX * i, paramY * FC6n[i]);
                    g.DrawLine(penLine7, paramX * (i - 1), paramY * FC7n[i - 1], paramX * i, paramY * FC7n[i]);
                    g.DrawLine(penLine8, paramX * (i - 1), paramY * FC8n[i - 1], paramX * i, paramY * FC8n[i]);
                    g.DrawLine(penLine9, paramX * (i - 1), paramY * FC9n[i - 1], paramX * i, paramY * FC9n[i]);
                    g.DrawLine(penLine10, paramX * (i - 1), paramY * FC10n[i - 1], paramX * i, paramY * FC10n[i]);
                    g.DrawLine(penLine11, paramX * (i - 1), paramY * FC11n[i - 1], paramX * i, paramY * FC11n[i]);
                    g.DrawLine(penLine12, paramX * (i - 1), paramY * FC12n[i - 1], paramX * i, paramY * FC12n[i]);
                }

            //v18.00追加ここまで 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            }
            else
            {
                for (i = 1; i <= we; i++)
                {
                    g.DrawLine(penLine1, paramX * (i - 1), paramY * FC1[i - 1], paramX * i, paramY * FC1[i]);
                    g.DrawLine(penLine2, paramX * (i - 1), paramY * FC2[i - 1], paramX * i, paramY * FC2[i]);
                    g.DrawLine(penLine3, paramX * (i - 1), paramY * FC3[i - 1], paramX * i, paramY * FC3[i]);
                    g.DrawLine(penLine4, paramX * (i - 1), paramY * FC4[i - 1], paramX * i, paramY * FC4[i]);
                    g.DrawLine(penLine5, paramX * (i - 1), paramY * FC5[i - 1], paramX * i, paramY * FC5[i]);
                    g.DrawLine(penLine6, paramX * (i - 1), paramY * FC6[i - 1], paramX * i, paramY * FC6[i]);
                    g.DrawLine(penLine7, paramX * (i - 1), paramY * FC7[i - 1], paramX * i, paramY * FC7[i]);
                    g.DrawLine(penLine8, paramX * (i - 1), paramY * FC8[i - 1], paramX * i, paramY * FC8[i]);
                    g.DrawLine(penLine9, paramX * (i - 1), paramY * FC9[i - 1], paramX * i, paramY * FC9[i]);
                    g.DrawLine(penLine10, paramX * (i - 1), paramY * FC10[i - 1], paramX * i, paramY * FC10[i]);
                    g.DrawLine(penLine11, paramX * (i - 1), paramY * FC11[i - 1], paramX * i, paramY * FC11[i]);
                    g.DrawLine(penLine12, paramX * (i - 1), paramY * FC12[i - 1], paramX * i, paramY * FC12[i]);
                }
            }
			g.Dispose();
		}


		//*******************************************************************************************
		//機    能  ：  Filter関数を作成する
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V1.00  99/09/14  J.IWASAWA       初版
		//                     00/02/17  J.IWASAWA       ｵﾌｾｯﾄｽｷｬﾝ、高解像度ｶﾒﾗ対応(FC4～FC6追加)
		//                     00/04/25  J.IWASAWA       Y=aX2ﾌｨﾙﾀ追加(FC7～FC9)追加
		//                     01/04/17  J.IWASAWA       Y=aX2ﾌｨﾙﾀ追加(FC7～FC9)をHighResoﾌｨﾙﾀに変更
		//              v7.00  03/07/11  (SI4)間々田     Private化
		//             v13.00  07/02/21  R.YAMAOKA       終端が0でないFC関数(FC1n～FC9n)を追加
		//********************************************************************************************
		private void FcSet()
		{
            //変更2014/10/07hata_v19.51反映
            //---　全面書き換え　-------------------------------ここから           
            //int w = 0;		//ﾙｰﾌﾟｶｳﾝﾀ
            //int i = 0;		//ﾙｰﾌﾟｶｳﾝﾀ

            //int we1 = 0;
            //int we2 = 0;
            //int we3 = 0;
            //int we4 = 0;
            //int we5 = 0;
            //int we6 = 0;
            //int we7 = 0;
            //int we8 = 0;
            //int we9 = 0;
            //int Wn1 = 0;
            //int Wn2 = 0;
            //int Wn3 = 0;
            //int Wn4 = 0;
            //int Wn5 = 0;
            //int Wn6 = 0;
            //int Wn7 = 0;
            //int Wn8 = 0;
            //int Wn9 = 0;

            ////輪郭強調ﾌｨﾙﾀ
            ////    Dim iw, omegaA, omegaD, omegaR, RR As Double                   'delete by 間々田 2003/07/16  定数化（以下のConst文を参照）

            ////Wn = 2047
            ////we = 4095
            ////we = 1023
            ////Wn = we / 2

            //we1 = Maint.FCRec[1].SIZE - 1;			//change by 間々田 2003/07/16  FC1pr(1) → FCRec(1).size
            //we2 = Maint.FCRec[2].SIZE - 1;			//change by 間々田 2003/07/16  FC2pr(1) → FCRec(2).size
            //we3 = Maint.FCRec[3].SIZE - 1;			//change by 間々田 2003/07/16  FC3pr(1) → FCRec(3).size
            //we4 = Maint.FCRec[4].SIZE - 1;			//change by 間々田 2003/07/16  FC4pr(1) → FCRec(4).size
            //we5 = Maint.FCRec[5].SIZE - 1;			//change by 間々田 2003/07/16  FC5pr(1) → FCRec(5).size
            //we6 = Maint.FCRec[6].SIZE - 1;			//change by 間々田 2003/07/16  FC6pr(1) → FCRec(6).size
            //we7 = Maint.FCRec[7].SIZE - 1;			//change by 間々田 2003/07/16  FC7pr(1) → FCRec(7).size
            //we8 = Maint.FCRec[8].SIZE - 1;			//change by 間々田 2003/07/16  FC8pr(1) → FCRec(8).size
            //we9 = Maint.FCRec[9].SIZE - 1;			//change by 間々田 2003/07/16  FC9pr(1) → FCRec(9).size
            //Wn1 = we1 / 2;
            //Wn2 = we2 / 2;
            //Wn3 = we3 / 2;
            //Wn4 = we4 / 2;
            //Wn5 = we5 / 2;
            //Wn6 = we6 / 2;
            //Wn7 = we7 / 2;
            //Wn8 = we8 / 2;
            //Wn9 = we9 / 2;

            //const float slope  = 0.5F;            //sheppフィルタの傾きに掛ける係数
            //const double iw = 10.0;
            //const double omegaA  = 125.0 / 1024.0;
            //const double omegaD  = 250.0 / 1024.0;
            //const double omegaR = 50.0 / 1024.0;

            ////HighResoをより強調の強いﾌｨﾙﾀにしたい場合は"RR"を6,7等にする。HighResoをより強調の弱いﾌｨﾙﾀにしたい場合は"RR"を3,4等にする。
            //const double RR = 5.0;


            ////change by 間々田 2003/07/16  以下の文で 変数 FCna → FCRec(n).a (n は1～9の数字) の変更を施した

            ////FC1(0 ～ FFT_SIZE) 作成
            //switch (Maint.FCRec[1].Kind)		//change by 間々田 2003/07/16  FC1pr(0) → FCRec(1).kind
            //{
            //    //Laks
            //    case 0:

            //        for (w = 0; w <= Wn1; w++)
            //        {
            //            //FC1h1 = Abs(w)     'delete by 間々田 2003/07/16 未使用

            //            //FC1(w) = FC1pr(0) * FC1h1 * FC1h2 * FC1h3
            //            FC1[w] = Maint.FCRec[1].a * w / Wn1;
            //            FC1[we1 - w] = FC1[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn1; w++)
            //        {
            //            FC1n[w] = Maint.FCRec[1].a * w / Wn1;
            //        }
            //        for (w = 0; w <= Wn1 - 1; w++)
            //        {
            //            FC1n[we1 - w] = FC1n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we1 + 1; i <= 4095; i++)
            //        {
            //            FC1[i] = 0;
            //            FC1n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //Shepp
            //    case 1:

            //        for (w = 0; w <= Wn1; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC1[w] = 0;
            //            }
            //            else
            //            {
            //                //FC1(w) = FC1pr(3) * Abs((2 * Wn1 / pai) * Sin((pai * w) / (2 * Wn1))) * ((Sin((pai * w) / (2 * Wn1))) / ((pai * w) / (2 * Wn1))) ^ 2
            //                FC1[w] = (float)(Maint.FCRec[1].a * slope * (1.0 / Wn1) * Math.Abs((2 * Wn1 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn1))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn1))) / ((ScanCorrect.Pai * w) / (2 * Wn1))), 2));
            //            }

            //            FC1[we1 - w] = FC1[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn1; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC1n[w] = 0;
            //            }
            //            else
            //            {
            //                FC1n[w] = (float)(Maint.FCRec[1].a * slope * (1.0 / Wn1) * Math.Abs((2 * Wn1 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn1))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn1))) / ((ScanCorrect.Pai * w) / (2 * Wn1))), 2));
            //            }
            //        }
            //        for (w = 0; w <= Wn1 - 1; w++)
            //        {
            //            FC1n[we1 - w] = FC1n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we1 + 1; i <= 4095; i++)
            //        {
            //            FC1[i] = 0;
            //            FC1n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //HighReso
            //    case 2:

            //        for (w = 0; w <= Wn1; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC1[w] = Maint.FCRec[1].a * w / Wn1;
            //            }
            //            else if (w < (omegaD * we1))
            //            {
            //                FC1[w] = (float)((Maint.FCRec[1].a * w / Wn1) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we1), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we1), 2)))));
            //            }
            //            else
            //            {
            //                FC1[w] = (float)((Maint.FCRec[1].a * w / Wn1) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we1), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we1), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we1)), 2) / Math.Pow((omegaR * we1), 2))));
            //            }

            //            //If FC1(w) > 1# Then
            //            //    FC1(w) = 1#
            //            //End If

            //            FC1[we1 - w] = FC1[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn1; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC1n[w] = Maint.FCRec[1].a * w / Wn1;
            //            }
            //            else if (w < (omegaD * we1))
            //            {
            //                FC1n[w] = (float)((Maint.FCRec[1].a * w / Wn1) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we1), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we1), 2)))));
            //            }
            //            else
            //            {
            //                FC1n[w] = (float)((Maint.FCRec[1].a * w / Wn1) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we1), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we1), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we1)), 2) / Math.Pow((omegaR * we1), 2))));
            //            }
            //        }
            //        for (w = 0; w <= Wn1 - 1; w++)
            //        {
            //            FC1n[we1 - w] = FC1n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we1 + 1; i <= 4095; i++)
            //        {
            //            FC1[i] = 0;
            //            FC1n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    default:
            //        //            MsgBox " フィルタ種類エラー"
            //        MessageBox.Show(CTResources.LoadResString(StringTable.IDS_FilterTypeError), Application.ProductName, MessageBoxButtons.OK);		//v7.0 リソース対応 by 間々田 2003/08/12
            //        break;
            //}


            ////FC2(0 ～ FFT_SIZE) 作成
            //switch (Maint.FCRec[2].Kind)		//change by 間々田 2003/07/16  FC2pr(0) → FCRec(2).kind
            //{
            //    //Laks
            //    case 0:

            //        for (w = 0; w <= Wn2; w++)
            //        {
            //            //                FC2h1 = Abs(w)         'delete by 間々田 2003/07/16 未使用

            //            //FC2(w) = FC2pr(0) * FC2h1 * FC2h2 * FC2h3
            //            FC2[w] = Maint.FCRec[2].a * w / Wn2;
            //            FC2[we2 - w] = FC2[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn2; w++)
            //        {
            //            FC2n[w] = Maint.FCRec[2].a * w / Wn2;
            //        }
            //        for (w = 0; w <= Wn2 - 1; w++)
            //        {
            //            FC2n[we2 - w] = FC2n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we2 + 1; i <= 4095; i++)
            //        {
            //            FC2[i] = 0;
            //            FC2n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //Shepp
            //    case 1:

            //        for (w = 0; w <= Wn2; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC2[w] = 0;
            //            }
            //            else
            //            {
            //                //FC2(w) = FC2pr(3) * Abs((2 * Wn2 / pai) * Sin((pai * w) / (2 * Wn2))) * ((Sin((pai * w) / (2 * Wn2))) / ((pai * w) / (2 * Wn2))) ^ 2
            //                FC2[w] = (float)(Maint.FCRec[2].a * slope * (1.0 / Wn2) * Math.Abs((2 * Wn2 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn2))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn2))) / ((ScanCorrect.Pai * w) / (2 * Wn2))), 2));
            //            }

            //            FC2[we2 - w] = FC2[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn2; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC2n[w] = 0;
            //            }
            //            else
            //            {
            //                FC2n[w] = (float)(Maint.FCRec[2].a * slope * (1.0 / Wn2) * Math.Abs((2 * Wn2 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn2))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn2))) / ((ScanCorrect.Pai * w) / (2 * Wn2))), 2));
            //            }
            //        }
            //        for (w = 0; w <= Wn2 - 1; w++)
            //        {
            //            FC2n[we2 - w] = FC2n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we2 + 1; i <= 4095; i++)
            //        {
            //            FC2[i] = 0;
            //            FC2n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //HighReso
            //    case 2:

            //        for (w = 0; w <= Wn2; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC2[w] = Maint.FCRec[2].a * w / Wn2;
            //            }
            //            else if (w < (omegaD * we2))
            //            {
            //                FC2[w] = (float)((Maint.FCRec[2].a * w / Wn2) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we2), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we2), 2)))));
            //            }
            //            else
            //            {
            //                FC2[w] = (float)((Maint.FCRec[2].a * w / Wn2) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we2), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we2), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we2)), 2) / Math.Pow((omegaR * we2), 2))));
            //            }

            //            //If FC2(w) > 1# Then
            //            //    FC2(w) = 1#
            //            //End If

            //            FC2[we2 - w] = FC2[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn2; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC2n[w] = Maint.FCRec[2].a * w / Wn2;
            //            }
            //            else if (w < (omegaD * we2))
            //            {
            //                FC2n[w] = (float)((Maint.FCRec[2].a * w / Wn2) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we2), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we2), 2)))));
            //            }
            //            else
            //            {
            //                FC2n[w] = (float)((Maint.FCRec[2].a * w / Wn2) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we2), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we2), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we2)), 2) / Math.Pow((omegaR * we2), 2))));
            //            }
            //        }
            //        for (w = 0; w <= Wn2 - 1; w++)
            //        {
            //            FC2n[we2 - w] = FC2n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we2 + 1; i <= 4095; i++)
            //        {
            //            FC2[i] = 0;
            //            FC2n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    default:
            //        //            MsgBox " フィルタ種類エラー"
            //        MessageBox.Show(CTResources.LoadResString(StringTable.IDS_FilterTypeError), Application.ProductName, MessageBoxButtons.OK);		//v7.0 リソース対応 by 間々田 2003/08/12
            //        break;
            //}


            ////FC3(0 ～ FFT_SIZE) 作成
            //switch (Maint.FCRec[3].Kind)		//change by 間々田 2003/07/16  FC3pr(0) → FCRec(3).kind
            //{
            //    //Laks
            //    case 0:

            //        for (w = 0; w <= Wn3; w++)
            //        {
            //            //                FC3h1 = Abs(w)         'delete by 間々田 2003/07/16 未使用

            //            //FC3(w) = FC3pr(0) * FC3h1 * FC3h2 * FC3h3
            //            FC3[w] = Maint.FCRec[3].a * w / Wn3;
            //            FC3[we3 - w] = FC3[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn3; w++)
            //        {
            //            FC3n[w] = Maint.FCRec[3].a * w / Wn3;
            //        }
            //        for (w = 0; w <= Wn3 - 1; w++)
            //        {
            //            FC3n[we3 - w] = FC3n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we3 + 1; i <= 4095; i++)
            //        {
            //            FC3[i] = 0;
            //            FC3n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //Shepp
            //    case 1:

            //        for (w = 0; w <= Wn3; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC3[w] = 0;
            //            }
            //            else
            //            {
            //                //FC3(w) = FC3pr(3) * Abs((2 * Wn3 / pai) * Sin((pai * w) / (2 * Wn3))) * ((Sin((pai * w) / (2 * Wn3))) / ((pai * w) / (2 * Wn3))) ^ 2
            //                FC3[w] = (float)(Maint.FCRec[3].a * slope * (1.0 / Wn3) * Math.Abs((2 * Wn3 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn3))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn3))) / ((ScanCorrect.Pai * w) / (2 * Wn3))), 2));
            //            }

            //            FC3[we3 - w] = FC3[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn3; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC3n[w] = 0;
            //            }
            //            else
            //            {
            //                FC3n[w] = (float)(Maint.FCRec[3].a * slope * (1.0 / Wn3) * Math.Abs((2 * Wn3 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn3))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn3))) / ((ScanCorrect.Pai * w) / (2 * Wn3))), 2));
            //            }
            //        }
            //        for (w = 0; w <= Wn3 - 1; w++)
            //        {
            //            FC3n[we3 - w] = FC3n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we3 + 1; i <= 4095; i++)
            //        {
            //            FC3[i] = 0;
            //            FC3n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //HighReso
            //    case 2:

            //        for (w = 0; w <= Wn3; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC3[w] = Maint.FCRec[3].a * w / Wn3;
            //            }
            //            else if (w < (omegaD * we3))
            //            {
            //                FC3[w] = (float)((Maint.FCRec[3].a * w / Wn3) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we3), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we3), 2)))));
            //            }
            //            else
            //            {
            //                FC3[w] = (float)((Maint.FCRec[3].a * w / Wn3) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we3), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we3), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we3)), 2) / Math.Pow((omegaR * we3), 2))));
            //            }

            //            //If FC3(w) > 1# Then
            //            //    FC3(w) = 1#
            //            //End If

            //            FC3[we3 - w] = FC3[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn3; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC3n[w] = Maint.FCRec[3].a * w / Wn3;
            //            }
            //            else if (w < (omegaD * we3))
            //            {
            //                FC3n[w] = (float)((Maint.FCRec[3].a * w / Wn3) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we3), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we3), 2)))));
            //            }
            //            else
            //            {
            //                FC3n[w] = (float)((Maint.FCRec[3].a * w / Wn3) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we3), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we3), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we3)), 2) / Math.Pow((omegaR * we3), 2))));
            //            }
            //        }
            //        for (w = 0; w <= Wn3 - 1; w++)
            //        {
            //            FC3n[we3 - w] = FC3n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we3 + 1; i <= 4095; i++)
            //        {
            //            FC3[i] = 0;
            //            FC3n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    default:
            //        //            MsgBox " フィルタ種類エラー"
            //        MessageBox.Show(CTResources.LoadResString(StringTable.IDS_FilterTypeError), Application.ProductName, MessageBoxButtons.OK);		//v7.0 リソース対応 by 間々田 2003/08/12
            //        break;
            //}


            ////FC4(0 ～ FFT_SIZE) 作成
            //switch (Maint.FCRec[4].Kind)		//change by 間々田 2003/07/16  FC4pr(0) → FCRec(4).kind
            //{
            //    //Laks
            //    case 0:

            //        for (w = 0; w <= Wn4; w++)
            //        {
            //            //                FC4h1 = Abs(w)         'delete by 間々田 2003/07/16 未使用

            //            FC4[w] = Maint.FCRec[4].a * w / Wn4;
            //            FC4[we4 - w] = FC4[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn4; w++)
            //        {
            //            FC4n[w] = Maint.FCRec[4].a * w / Wn4;
            //        }
            //        for (w = 0; w <= Wn4 - 1; w++)
            //        {
            //            FC4n[we4 - w] = FC4n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we4 + 1; i <= 4095; i++)
            //        {
            //            FC4[i] = 0;
            //            FC4n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //Shepp
            //    case 1:

            //        for (w = 0; w <= Wn4; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC4[w] = 0;
            //            }
            //            else
            //            {
            //                FC4[w] = (float)(Maint.FCRec[4].a * slope * (1.0 / Wn4) * Math.Abs((2 * Wn4 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn4))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn4))) / ((ScanCorrect.Pai * w) / (2 * Wn4))), 2));
            //            }

            //            FC4[we4 - w] = FC4[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn4; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC4n[w] = 0;
            //            }
            //            else
            //            {
            //                FC4n[w] = (float)(Maint.FCRec[4].a * slope * (1.0 / Wn4) * Math.Abs((2 * Wn4 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn4))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn4))) / ((ScanCorrect.Pai * w) / (2 * Wn4))), 2));
            //            }
            //        }
            //        for (w = 0; w <= Wn4 - 1; w++)
            //        {
            //            FC4n[we4 - w] = FC4n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we4 + 1; i <= 4095; i++)
            //        {
            //            FC4[i] = 0;
            //            FC4n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //HighReso
            //    case 2:

            //        for (w = 0; w <= Wn4; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC4[w] = Maint.FCRec[4].a * w / Wn4;
            //            }
            //            else if (w < (omegaD * we4))
            //            {
            //                FC4[w] = (float)((Maint.FCRec[4].a * w / Wn4) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we4), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we4), 2)))));
            //            }
            //            else
            //            {
            //                FC4[w] = (float)((Maint.FCRec[4].a * w / Wn4) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we4), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we4), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we4)), 2) / Math.Pow((omegaR * we4), 2))));
            //            }

            //            //If FC4(w) > 1# Then
            //            //    FC4(w) = 1#
            //            //End If

            //            FC4[we4 - w] = FC4[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn4; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC4n[w] = Maint.FCRec[4].a * w / Wn4;
            //            }
            //            else if (w < (omegaD * we4))
            //            {
            //                FC4n[w] = (float)((Maint.FCRec[4].a * w / Wn4) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we4), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we4), 2)))));
            //            }
            //            else
            //            {
            //                FC4n[w] = (float)((Maint.FCRec[4].a * w / Wn4) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we4), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we4), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we4)), 2) / Math.Pow((omegaR * we4), 2))));
            //            }
            //        }
            //        for (w = 0; w <= Wn4 - 1; w++)
            //        {
            //            FC4n[we4 - w] = FC4n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we4 + 1; i <= 4095; i++)
            //        {
            //            FC4[i] = 0;
            //            FC4n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    default:
            //        //            MsgBox " フィルタ種類エラー"
            //        MessageBox.Show(CTResources.LoadResString(StringTable.IDS_FilterTypeError), Application.ProductName, MessageBoxButtons.OK);		//v7.0 リソース対応 by 間々田 2003/08/12
            //        break;
            //}


            ////FC5(0 ～ FFT_SIZE) 作成
            //switch (Maint.FCRec[5].Kind)		//change by 間々田 2003/07/16  FC5pr(0) → FCRec(5).kind
            //{
            //    //Laks
            //    case 0:

            //        for (w = 0; w <= Wn5; w++)
            //        {
            //            //                FC5h1 = Abs(w)         'delete by 間々田 2003/07/16 未使用

            //            FC5[w] = Maint.FCRec[5].a * w / Wn5;
            //            FC5[we5 - w] = FC5[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn5; w++)
            //        {
            //            FC5n[w] = Maint.FCRec[5].a * w / Wn5;
            //        }
            //        for (w = 0; w <= Wn5 - 1; w++)
            //        {
            //            FC5n[we5 - w] = FC5n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we5 + 1; i <= 4095; i++)
            //        {
            //            FC5[i] = 0;
            //            FC5n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //Shepp
            //    case 1:

            //        for (w = 0; w <= Wn5; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC5[w] = 0;
            //            }
            //            else
            //            {
            //                FC5[w] = (float)(Maint.FCRec[5].a * slope * (1.0 / Wn5) * Math.Abs((2 * Wn5 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn5))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn5))) / ((ScanCorrect.Pai * w) / (2 * Wn5))), 2));
            //            }

            //            FC5[we5 - w] = FC5[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn5; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC5n[w] = 0;
            //            }
            //            else
            //            {
            //                FC5n[w] = (float)(Maint.FCRec[5].a * slope * (1.0 / Wn5) * Math.Abs((2 * Wn5 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn5))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn5))) / ((ScanCorrect.Pai * w) / (2 * Wn5))), 2));
            //            }
            //        }
            //        for (w = 0; w <= Wn5 - 1; w++)
            //        {
            //            FC5n[we5 - w] = FC5n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we5 + 1; i <= 4095; i++)
            //        {
            //            FC5[i] = 0;
            //            FC5n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //HighReso
            //    case 2:

            //        for (w = 0; w <= Wn5; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC5[w] = Maint.FCRec[5].a * w / Wn5;
            //            }
            //            else if (w < (omegaD * we5))
            //            {
            //                FC5[w] = (float)((Maint.FCRec[5].a * w / Wn5) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we5), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we5), 2)))));
            //            }
            //            else
            //            {
            //                FC5[w] = (float)((Maint.FCRec[5].a * w / Wn5) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we5), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we5), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we5)), 2) / Math.Pow((omegaR * we5), 2))));
            //            }

            //            //If FC5(w) > 1# Then
            //            //    FC5(w) = 1#
            //            //End If

            //            FC5[we5 - w] = FC5[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn5; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC5n[w] = Maint.FCRec[5].a * w / Wn5;
            //            }
            //            else if (w < (omegaD * we5))
            //            {
            //                FC5n[w] = (float)((Maint.FCRec[5].a * w / Wn5) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we5), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we5), 2)))));
            //            }
            //            else
            //            {
            //                FC5n[w] = (float)((Maint.FCRec[5].a * w / Wn5) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we5), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we5), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we5)), 2) / Math.Pow((omegaR * we5), 2))));
            //            }
            //        }
            //        for (w = 0; w <= Wn5 - 1; w++)
            //        {
            //            FC5n[we5 - w] = FC5n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we5 + 1; i <= 4095; i++)
            //        {
            //            FC5[i] = 0;
            //            FC5n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    default:
            //        //            MsgBox " フィルタ種類エラー"
            //        MessageBox.Show(CTResources.LoadResString(StringTable.IDS_FilterTypeError), Application.ProductName, MessageBoxButtons.OK);		//v7.0 リソース対応 by 間々田 2003/08/12
            //        break;
            //}


            ////FC6(0 ～ FFT_SIZE) 作成
            //switch (Maint.FCRec[6].Kind)		//change by 間々田 2003/07/16  FC6pr(0) → FCRec(6).kind
            //{
            //    //Laks
            //    case 0:

            //        for (w = 0; w <= Wn6; w++)
            //        {
            //            //                FC6h1 = Abs(w)         'delete by 間々田 2003/07/16 未使用

            //            FC6[w] = Maint.FCRec[6].a * w / Wn6;
            //            FC6[we6 - w] = FC6[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn6; w++)
            //        {
            //            FC6n[w] = Maint.FCRec[6].a * w / Wn6;
            //        }
            //        for (w = 0; w <= Wn6 - 1; w++)
            //        {
            //            FC6n[we6 - w] = FC6n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we6 + 1; i <= 4095; i++)
            //        {
            //            FC6[i] = 0;
            //            FC6n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //Shepp
            //    case 1:

            //        for (w = 0; w <= Wn6; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC6[w] = 0;
            //            }
            //            else
            //            {
            //                FC6[w] = (float)(Maint.FCRec[6].a * slope * (1.0 / Wn6) * Math.Abs((2 * Wn6 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn6))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn6))) / ((ScanCorrect.Pai * w) / (2 * Wn6))), 2));
            //            }

            //            FC6[we6 - w] = FC6[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn6; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC6n[w] = 0;
            //            }
            //            else
            //            {
            //                FC6n[w] = (float)(Maint.FCRec[6].a * slope * (1.0 / Wn6) * Math.Abs((2 * Wn6 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn6))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn6))) / ((ScanCorrect.Pai * w) / (2 * Wn6))), 2));
            //            }
            //        }
            //        for (w = 0; w <= Wn6 - 1; w++)
            //        {
            //            FC6n[we6 - w] = FC6n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we6 + 1; i <= 4095; i++)
            //        {
            //            FC6[i] = 0;
            //            FC6n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //HighReso
            //    case 2:

            //        for (w = 0; w <= Wn6; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC6[w] = Maint.FCRec[6].a * w / Wn6;
            //            }
            //            else if (w < (omegaD * we6))
            //            {
            //                FC6[w] = (float)((Maint.FCRec[6].a * w / Wn6) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we6), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we6), 2)))));
            //            }
            //            else
            //            {
            //                FC6[w] = (float)((Maint.FCRec[6].a * w / Wn6) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we6), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we6), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we6)), 2) / Math.Pow((omegaR * we6), 2))));
            //            }

            //            //If FC6(w) > 1# Then
            //            //    FC6(w) = 1#
            //            //End If

            //            FC6[we6 - w] = FC6[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn6; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC6n[w] = Maint.FCRec[6].a * w / Wn6;
            //            }
            //            else if (w < (omegaD * we6))
            //            {
            //                FC6n[w] = (float)((Maint.FCRec[6].a * w / Wn6) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we6), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we6), 2)))));
            //            }
            //            else
            //            {
            //                FC6n[w] = (float)((Maint.FCRec[6].a * w / Wn6) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we6), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we6), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we6)), 2) / Math.Pow((omegaR * we6), 2))));
            //            }
            //        }
            //        for (w = 0; w <= Wn6 - 1; w++)
            //        {
            //            FC6n[we6 - w] = FC6n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we6 + 1; i <= 4095; i++)
            //        {
            //            FC6[i] = 0;
            //            FC6n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    default:
            //        //            MsgBox " フィルタ種類エラー"
            //        MessageBox.Show(CTResources.LoadResString(StringTable.IDS_FilterTypeError), Application.ProductName, MessageBoxButtons.OK);		//v7.0 リソース対応 by 間々田 2003/08/12
            //        break;
            //}


            ////FC7(0 ～ FFT_SIZE) 作成
            //switch (Maint.FCRec[7].Kind)		//change by 間々田 2003/07/16  FC7pr(0) → FCRec(7).kind
            //{
            //    //Laks
            //    case 0:

            //        for (w = 0; w <= Wn7; w++)
            //        {
            //            //                FC7h1 = Abs(w)         'delete by 間々田 2003/07/16 未使用

            //            FC7[w] = Maint.FCRec[7].a * w / Wn7;
            //            FC7[we7 - w] = FC7[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn7; w++)
            //        {
            //            FC7n[w] = Maint.FCRec[7].a * w / Wn7;
            //        }
            //        for (w = 0; w <= Wn7 - 1; w++)
            //        {
            //            FC7n[we7 - w] = FC7n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we7 + 1; i <= 4095; i++)
            //        {
            //            FC7[i] = 0;
            //            FC7n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //Shepp
            //    case 1:

            //        for (w = 0; w <= Wn7; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC7[w] = 0;
            //            }
            //            else
            //            {
            //                FC7[w] = (float)(Maint.FCRec[7].a * slope * (1.0 / Wn7) * Math.Abs((2 * Wn7 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn7))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn7))) / ((ScanCorrect.Pai * w) / (2 * Wn7))), 2));
            //            }

            //            FC7[we7 - w] = FC7[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn7; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC7n[w] = 0;
            //            }
            //            else
            //            {
            //                FC7n[w] = (float)(Maint.FCRec[7].a * slope * (1.0 / Wn7) * Math.Abs((2 * Wn7 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn7))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn7))) / ((ScanCorrect.Pai * w) / (2 * Wn7))), 2));
            //            }
            //        }
            //        for (w = 0; w <= Wn7 - 1; w++)
            //        {
            //            FC7n[we7 - w] = FC7n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we7 + 1; i <= 4095; i++)
            //        {
            //            FC7[i] = 0;
            //            FC7n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //HighReso
            //    case 2:

            //        for (w = 0; w <= Wn7; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC7[w] = Maint.FCRec[7].a * w / Wn7;
            //            }
            //            else if (w < (omegaD * we7))
            //            {
            //                FC7[w] = (float)((Maint.FCRec[7].a * w / Wn7) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we7), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we7), 2)))));
            //            }
            //            else
            //            {
            //                FC7[w] = (float)((Maint.FCRec[7].a * w / Wn7) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we7), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we7), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we7)), 2) / Math.Pow((omegaR * we7), 2))));
            //            }

            //            //If FC7(w) > 1# Then
            //            //    FC7(w) = 1#
            //            //End If

            //            FC7[we7 - w] = FC7[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn7; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC7n[w] = Maint.FCRec[7].a * w / Wn7;
            //            }
            //            else if (w < (omegaD * we7))
            //            {
            //                FC7n[w] = (float)((Maint.FCRec[7].a * w / Wn7) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we7), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we7), 2)))));
            //            }
            //            else
            //            {
            //                FC7n[w] = (float)((Maint.FCRec[7].a * w / Wn7) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we7), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we7), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we7)), 2) / Math.Pow((omegaR * we7), 2))));
            //            }
            //        }
            //        for (w = 0; w <= Wn7 - 1; w++)
            //        {
            //            FC7n[we7 - w] = FC7n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we7 + 1; i <= 4095; i++)
            //        {
            //            FC7[i] = 0;
            //            FC7n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    default:
            //        //            MsgBox " フィルタ種類エラー"
            //        MessageBox.Show(CTResources.LoadResString(StringTable.IDS_FilterTypeError), Application.ProductName, MessageBoxButtons.OK);		//v7.0 リソース対応 by 間々田 2003/08/12
            //        break;
            //}


            ////FC8(0 ～ FFT_SIZE) 作成
            //switch (Maint.FCRec[8].Kind)		//change by 間々田 2003/07/16  FC8pr(0) → FCRec(8).kind
            //{
            //    //Laks
            //    case 0:

            //        for (w = 0; w <= Wn8; w++)
            //        {
            //            //                FC8h1 = Abs(w)         'delete by 間々田 2003/07/16 未使用

            //            FC8[w] = Maint.FCRec[8].a * w / Wn8;
            //            FC8[we8 - w] = FC8[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn8; w++)
            //        {
            //            FC8n[w] = Maint.FCRec[8].a * w / Wn8;
            //        }
            //        for (w = 0; w <= Wn8 - 1; w++)
            //        {
            //            FC8n[we8 - w] = FC8n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we8 + 1; i <= 4095; i++)
            //        {
            //            FC8[i] = 0;
            //            FC8n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //Shepp
            //    case 1:

            //        for (w = 0; w <= Wn8; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC8[w] = 0;
            //            }
            //            else
            //            {
            //                FC8[w] = (float)(Maint.FCRec[8].a * slope * (1.0 / Wn8) * Math.Abs((2 * Wn8 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn8))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn8))) / ((ScanCorrect.Pai * w) / (2 * Wn8))), 2));
            //            }

            //            FC8[we8 - w] = FC8[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn8; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC8n[w] = 0;
            //            }
            //            else
            //            {
            //                FC8n[w] = (float)(Maint.FCRec[8].a * slope * (1.0 / Wn8) * Math.Abs((2 * Wn8 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn8))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn8))) / ((ScanCorrect.Pai * w) / (2 * Wn8))), 2));
            //            }
            //        }
            //        for (w = 0; w <= Wn8 - 1; w++)
            //        {
            //            FC8n[we8 - w] = FC8n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we8 + 1; i <= 4095; i++)
            //        {
            //            FC8[i] = 0;
            //            FC8n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //HighReso
            //    case 2:

            //        for (w = 0; w <= Wn8; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC8[w] = Maint.FCRec[8].a * w / Wn8;
            //            }
            //            else if (w < (omegaD * we8))
            //            {
            //                FC8[w] = (float)((Maint.FCRec[8].a * w / Wn8) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we8), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we8), 2)))));
            //            }
            //            else
            //            {
            //                FC8[w] = (float)((Maint.FCRec[8].a * w / Wn8) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we8), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we8), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we8)), 2) / Math.Pow((omegaR * we8), 2))));
            //            }

            //            //If FC8(w) > 1# Then
            //            //    FC8(w) = 1#
            //            //End If

            //            FC8[we8 - w] = FC8[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn8; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC8n[w] = Maint.FCRec[8].a * w / Wn8;
            //            }
            //            else if (w < (omegaD * we8))
            //            {
            //                FC8n[w] = (float)((Maint.FCRec[8].a * w / Wn8) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we8), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we8), 2)))));
            //            }
            //            else
            //            {
            //                FC8n[w] = (float)((Maint.FCRec[8].a * w / Wn8) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we8), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we8), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we8)), 2) / Math.Pow((omegaR * we8), 2))));
            //            }
            //        }
            //        for (w = 0; w <= Wn8 - 1; w++)
            //        {
            //            FC8n[we8 - w] = FC8n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we8 + 1; i <= 4095; i++)
            //        {
            //            FC8[i] = 0;
            //            FC8n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    default:
            //        //            MsgBox " フィルタ種類エラー"
            //        MessageBox.Show(CTResources.LoadResString(StringTable.IDS_FilterTypeError), Application.ProductName, MessageBoxButtons.OK);		//v7.0 リソース対応 by 間々田 2003/08/12
            //        break;
            //}


            ////FC9(0 ～ FFT_SIZE) 作成
            //switch (Maint.FCRec[9].Kind)		//change by 間々田 2003/07/16  FC9pr(0) → FCRec(9).kind
            //{
            //    //Laks
            //    case 0:

            //        for (w = 0; w <= Wn9; w++)
            //        {
            //            //                FC9h1 = Abs(w)         'delete by 間々田 2003/07/16 未使用

            //            FC9[w] = Maint.FCRec[9].a * w / Wn9;
            //            FC9[we9 - w] = FC9[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn9; w++)
            //        {
            //            FC9n[w] = Maint.FCRec[9].a * w / Wn9;
            //        }
            //        for (w = 0; w <= Wn9 - 1; w++)
            //        {
            //            FC9n[we9 - w] = FC9n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we9 + 1; i <= 4095; i++)
            //        {
            //            FC9[i] = 0;
            //            FC9n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //Shepp
            //    case 1:

            //        for (w = 0; w <= Wn9; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC9[w] = 0;
            //            }
            //            else
            //            {
            //                FC9[w] = (float)(Maint.FCRec[9].a * slope * (1.0 / Wn9) * Math.Abs((2 * Wn9 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn9))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn9))) / ((ScanCorrect.Pai * w) / (2 * Wn9))), 2));
            //            }

            //            FC9[we9 - w] = FC9[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn9; w++)
            //        {
            //            if (w == 0)
            //            {
            //                FC9n[w] = 0;
            //            }
            //            else
            //            {
            //                FC9n[w] = (float)(Maint.FCRec[9].a * slope * (1.0 / Wn9) * Math.Abs((2 * Wn9 / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * w) / (2 * Wn9))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * w) / (2 * Wn9))) / ((ScanCorrect.Pai * w) / (2 * Wn9))), 2));
            //            }
            //        }
            //        for (w = 0; w <= Wn9 - 1; w++)
            //        {
            //            FC9n[we9 - w] = FC9n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we9 + 1; i <= 4095; i++)
            //        {
            //            FC9[i] = 0;
            //            FC9n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    //HighReso
            //    case 2:

            //        for (w = 0; w <= Wn9; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC9[w] = Maint.FCRec[9].a * w / Wn9;
            //            }
            //            else if (w < (omegaD * we9))
            //            {
            //                FC9[w] = (float)((Maint.FCRec[9].a * w / Wn9) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we9), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we9), 2)))));
            //            }
            //            else
            //            {
            //                FC9[w] = (float)((Maint.FCRec[9].a * w / Wn9) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we9), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we9), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we9)), 2) / Math.Pow((omegaR * we9), 2))));
            //            }

            //            //If FC9(w) > 1# Then
            //            //    FC9(w) = 1#
            //            //End If

            //            FC9[we9 - w] = FC9[w];
            //        }

            //        //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
            //        for (w = 0; w <= Wn9; w++)
            //        {
            //            if (w < iw)
            //            {
            //                FC9n[w] = Maint.FCRec[9].a * w / Wn9;
            //            }
            //            else if (w < (omegaD * we9))
            //            {
            //                FC9n[w] = (float)((Maint.FCRec[9].a * w / Wn9) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we9), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we9), 2)))));
            //            }
            //            else
            //            {
            //                FC9n[w] = (float)((Maint.FCRec[9].a * w / Wn9) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we9), 2) / (Math.Pow((w - iw), 2) + Math.Pow((omegaA * we9), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((w - (omegaD * we9)), 2) / Math.Pow((omegaR * we9), 2))));
            //            }
            //        }
            //        for (w = 0; w <= Wn9 - 1; w++)
            //        {
            //            FC9n[we9 - w] = FC9n[w + 1];
            //        }
            //        //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

            //        for (i = we9 + 1; i <= 4095; i++)
            //        {
            //            FC9[i] = 0;
            //            FC9n[i] = 0;		//v13.00追加 2007/02/21 やまおか
            //        }

            //        break;


            //    default:
            //        //            MsgBox " フィルタ種類エラー"
            //        MessageBox.Show(CTResources.LoadResString(StringTable.IDS_FilterTypeError), Application.ProductName, MessageBoxButtons.OK);		//v7.0 リソース対応 by 間々田 2003/08/12
            //        break;
            //}
            //---　全面書き換え　-------------------------------ここまで

            //上記の処理を関数化し，FFTサイズ8192に対応させる    'v18.00追加 FFTサイズ8192対応 2011/03/10 by 間々田
            Maint.MakeFCData(ref Maint.FCRec[1], ref FC1, ref FC1n);
            Maint.MakeFCData(ref Maint.FCRec[2], ref FC2, ref FC2n);
            Maint.MakeFCData(ref Maint.FCRec[3], ref FC3, ref FC3n);
            Maint.MakeFCData(ref Maint.FCRec[4], ref FC4, ref FC4n);
            Maint.MakeFCData(ref Maint.FCRec[5], ref FC5, ref FC5n);
            Maint.MakeFCData(ref Maint.FCRec[6], ref FC6, ref FC6n);
            Maint.MakeFCData(ref Maint.FCRec[7], ref FC7, ref FC7n);
            Maint.MakeFCData(ref Maint.FCRec[8], ref FC8, ref FC8n);
            Maint.MakeFCData(ref Maint.FCRec[9], ref FC9, ref FC9n);
            Maint.MakeFCData(ref Maint.FCRec[10], ref FC10, ref FC10n);
            Maint.MakeFCData(ref Maint.FCRec[11], ref FC11, ref FC11n);
            Maint.MakeFCData(ref Maint.FCRec[12], ref FC12, ref FC12n);

        }


		//*******************************************************************************
		//機　　能： キャンセルボタンクリック時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			//フォームのアンロード
			this.Close();
		}


		//*******************************************************************************
		//機　　能： 欠陥画像作成ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V7.0   2003/08/12  (SI4)間々田   新規作成
		//*******************************************************************************
		private void cmdDefImage_Click(object sender, EventArgs e)
		{
			int rc = 0;

			float thre = 0;
			float thre_n = 0;
			float thre_line = 0;
			int BlockSize = 0;

			//マウスポインタを砂時計にする
			this.Cursor = Cursors.WaitCursor;

			//パラメータの取得
			float.TryParse(txtParam0.Text, out thre);
			float.TryParse(txtParam1.Text, out thre_n);
			double txtParam2Value = 0;
			double.TryParse(txtParam2.Text, out txtParam2Value);
			thre_line = (float)(txtParam2Value / 100);
			BlockSize = (optBlockSize300.Checked ? 300 : 156);

			ScanCorrect.OptValueGet_Cor();

			ScanCorrect.GAIN_IMAGE = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size - 1 + 1];		//v15.0変更 -1した by 間々田 2009/06/03
			ScanCorrect.OFFSET_IMAGE = new double[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size - 1 + 1];		//v15.0変更 -1した by 間々田 2009/06/03
			ScanCorrect.Def_IMAGE = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size - 1 + 1];			//v15.0変更 -1した by 間々田 2009/06/03

			//オフセット校正画像とゲイン校正画像を配列に読み込む
			rc = ScanCorrect.ImageOpen(ref ScanCorrect.GAIN_IMAGE[0], ScanCorrect.GAIN_CORRECT, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
			rc = ScanCorrect.DoubleImageOpen(ref ScanCorrect.OFFSET_IMAGE[0], ScanCorrect.OFF_CORRECT, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);

			//欠陥画像を作成する
			//    Call FpdDefDetect(GAIN_IMAGE(0), OFFSET_IMAGE(0), Def_IMAGE(0), H_SIZE, V_SIZE, thre, thre_n, thre_line, BlockSize)
            //2014/11/07hata キャストの修正
            //ScanCorrect.FpdDefDetect(ref ScanCorrect.GAIN_IMAGE[0], ref ScanCorrect.OFFSET_IMAGE[0], ref ScanCorrect.Def_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, thre, thre_n, thre_line, (int)(BlockSize / (Math.Pow(2, CTSettings.scaninh.Data.binning))));
            ScanCorrect.FpdDefDetect(ref ScanCorrect.GAIN_IMAGE[0], 
                                     ref ScanCorrect.OFFSET_IMAGE[0], 
                                     ref ScanCorrect.Def_IMAGE[0], 
                                     CTSettings.detectorParam.h_size, 
                                     CTSettings.detectorParam.v_size, 
                                     thre, 
                                     thre_n, 
                                     thre_line,
                                     Convert.ToInt32(BlockSize / (Math.Pow(2, CTSettings.scaninh.Data.binning))));

			//メンテナンス画面を非表示
			//変更2015/1/17hata_非表示のときにちらつくため
            //this.Hide();
            modCT30K.FormHide(this);

			//欠陥画像を表示する
            if (!modLibrary.IsExistForm("frmDefectImage"))	//追加2015/01/30hata_if文追加
            {
                frmDefectImage.Instance.Show(frmCTMenu.Instance);
            }
            else
            {
                frmDefectImage.Instance.WindowState = FormWindowState.Normal;
                frmDefectImage.Instance.Visible = true;
            }
            
            //ゲイン画像を表示する
            if (!modLibrary.IsExistForm("frmGainImageForMaint"))	//追加2015/01/30hata_if文追加
            {
                frmGainImageForMaint.Instance.Show(frmCTMenu.Instance);
            }

            else
            {
                frmGainImageForMaint.Instance.WindowState = FormWindowState.Normal;
                frmGainImageForMaint.Instance.Visible = true;
            }
            
            //マウスポインタを元に戻す
			this.Cursor = Cursors.Default;
		}


		//*******************************************************************************
		//機　　能： ＯＫボタンクリック時の処理
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
			if (FCsetflg == 1)
			{
				//確認ダイアログ表示：ﾌｨﾙﾀ関数を変更します。
				DialogResult result = MessageBox.Show(CTResources.LoadResString(12704), 
													  CTResources.LoadResString(StringTable.IDS_FilterSetup), 
													  MessageBoxButtons.OKCancel, 
													  MessageBoxIcon.Information);				//リソース対応 2003/08/12
				if (result == DialogResult.OK)
				{
					this.Cursor = Cursors.WaitCursor;

					//v11.2削除ここから by 間々田 2006/01/12
					//Fcwrite
					//FcRead              'FCﾌｧｲﾙ読込
					//Fcgraph             'ﾌｨﾙﾀFC1,2,3をｸﾞﾗﾌ表示
					//Me.MousePointer = vbDefault
					//v11.2削除ここから by 間々田 2006/01/12

					//v11.2追加ここから by 間々田 2006/01/12
					FcWrite(@"C:\Ct\filter\FC1.dat", FC1);
					FcWrite(@"C:\Ct\filter\FC2.dat", FC2);
					FcWrite(@"C:\Ct\filter\FC3.dat", FC3);
					FcWrite(@"C:\Ct\filter\FC4.dat", FC4);
					FcWrite(@"C:\Ct\filter\FC5.dat", FC5);
					FcWrite(@"C:\Ct\filter\FC6.dat", FC6);
					FcWrite(@"C:\Ct\filter\FC7.dat", FC7);
					FcWrite(@"C:\Ct\filter\FC8.dat", FC8);
					FcWrite(@"C:\Ct\filter\FC9.dat", FC9);
					//v11.2追加ここまで by 間々田 2006/01/12

                    //追加2014/10/07hata_v19.51反映
                    FcWrite(@"C:\Ct\filter\FC10.dat", FC10);        //v18.00追加 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    FcWrite(@"C:\Ct\filter\FC11.dat", FC11);        //v18.00追加 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    FcWrite(@"C:\Ct\filter\FC12.dat", FC12);        //v18.00追加 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

					//v13.00追加 2007/02/20 やまおか
					FcWrite(@"C:\Ct\filter\FC1n.dat", FC1n);
					FcWrite(@"C:\Ct\filter\FC2n.dat", FC2n);
					FcWrite(@"C:\Ct\filter\FC3n.dat", FC3n);
					FcWrite(@"C:\Ct\filter\FC4n.dat", FC4n);
					FcWrite(@"C:\Ct\filter\FC5n.dat", FC5n);
					FcWrite(@"C:\Ct\filter\FC6n.dat", FC6n);
					FcWrite(@"C:\Ct\filter\FC7n.dat", FC7n);
					FcWrite(@"C:\Ct\filter\FC8n.dat", FC8n);
					FcWrite(@"C:\Ct\filter\FC9n.dat", FC9n);
                    //追加2014/10/07hata_v19.51反映
                    FcWrite(@"C:\Ct\filter\FC10n.dat", FC10n);      //v18.00追加 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    FcWrite(@"C:\Ct\filter\FC11n.dat", FC11n);      //v18.00追加 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    FcWrite(@"C:\Ct\filter\FC12n.dat", FC12n);      //v18.00追加 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

                }
			}

			//フォームのアンロード
			this.Close();
		}


		//*******************************************************************************
		//機　　能： 自動スキャン位置指定調整要領書　ファイル選択クリック時
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V15.00  09/08/26   (検SS) 長野      新規作成
		//*******************************************************************************
		private void cmdRefGo_Click(object sender, EventArgs e)
		{
			//modImageInfo.ImageInfoStruct theInfo = default(modImageInfo.ImageInfoStruct);		//付帯情報構造体変数
            ImageInfo theInfo = new ImageInfo();
            
            float CenterCh = 0;				//回転中心ch
			float DetectorPitch = 0;		//検出器ピッチ
            float PosY = 0;                 //Y軸位置    'v18.00追加 byやまおか 2011/02/04

            theInfo.Data.Initialize();


			//コモンダイアログの表示
			string FileName = modFileIO.GetFileName(StringTable.IDS_Open, CTResources.LoadResString(StringTable.IDS_CTImage), ".img");

			//付帯情報が無い場合
			if (string.IsNullOrEmpty(FileName))
			{
				return;
			}

			txtImgFilename.Text = FileName;



			//付帯情報を取得
			//if (!modImageInfo.ReadImageInfo(ref theInfo, modLibrary.RemoveExtension(FileName, ".img"))) return;
            //if (ImageInfo.ReadImageInfo(ref theInfo.Data, FileName, ".img")) return;
            //rev20.00 修正 by長野 2015/01/16
            string InfFileName = FileName.Substring(0, FileName.Length - 4);
            if (!(ImageInfo.ReadImageInfo(ref theInfo.Data, InfFileName))) return;

			//v17.00 FPD対応 by 山影 10-03-04

			//CenterCh = Format(theInfo.nc, "000.000")
            CenterCh = Convert.ToSingle(((theInfo.Data.detector == (int)DetectorConstants.DetTypePke) ? (theInfo.Data.mainch - 1 - theInfo.Data.nc) : theInfo.Data.nc).ToString("000.000"));

			//検出器ピッチは、小数点７位以下を切り捨て
			//If theInfo.B1 = 0 Then
            if (theInfo.Data.b1 == 0)
			{
				//MsgBox ("Ver15以降で撮影した画像を選択してください")
				MessageBox.Show(CTResources.LoadResString(20020), Application.ProductName, MessageBoxButtons.OK);		//ストリングテーブル化 'v17.60 by 長野 2011/05/22
				return;
			}
			else
			{
				//DetectorPitch = Format((10 / theInfo.B1), "0.000000")
                DetectorPitch = Convert.ToSingle((10 / theInfo.Data.b1).ToString("0.000000"));
			}

            //追加2014/10/07hata_v19.51反映
            //Y軸位置    'v18.00追加 byやまおか 2011/02/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            PosY = Convert.ToSingle(theInfo.Data.table_x_pos.ToString("0.00"));

			txtCenterCh.Text = CenterCh.ToString();
			txtDetectorPitch.Text = DetectorPitch.ToString();

            //追加2014/10/07hata_v19.51反映
            txtPosY.Text = Convert.ToString(PosY);            //v18.00追加 byやまおか 2011/02/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

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
		private void frmMaint_Load(object sender, EventArgs e)
		{
			int i = 0;

			//実行時はフラグをセット
			modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTMaintenance;

			//各コントロールのキャプションにリソースから取得した文字列をセットする
			SetCaption();

			//v17.60 英語用レイアウト調整 by長野 2011/05/25
			if (modCT30K.IsEnglish)
			{
				EnglishAdjustLayout();
			}

			//フォームの設定
			modCT30K.SetPosNormalForm(this);

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//ビニングモード                                                 'v7.0 append by 間々田 2003/08/12 フラットパネル対応
			//    lblBinningMode.Caption = GetBinningStr(scansel.binning)
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//コーンビーム調整用  added by　山本　2002-8-31
			optNormal.Checked = (Maint.ConeAdjustFlg == 0);
			optCone.Checked = (Maint.ConeAdjustFlg == 1);		//added by 間々田 2004/03/02

			//v29.99 今のところ不要のためfalseに変更 by長野 2013/04/08'''''ここから'''''
			//Viscomメンテナンス用  added by　山本　2006-12-27
			//    optNormalViscom.Value = (ViscomMaintFlg = 0)
			//    optMaintViscom.Value = (ViscomMaintFlg = 1)
			optNormalViscom.Checked = false;
			optMaintViscom.Checked = false;
			//v29.99 今のところ不要のためfalseに変更 by長野 2013/04/08'''''ここまで'''''

			//フィルタ関数設定：Ｙ軸のラベル位置
			lblPttop.Top = picFilter.Top - 6;
            //2014/11/07hata キャストの修正
            //lblPtmid.Top = picFilter.Top + picFilter.Height / 2 - 6;
            lblPtmid.Top = picFilter.Top + Convert.ToInt32(picFilter.Height / 2F) - 6;
            lblPt0.Top = picFilter.Top + picFilter.Height - 6;

			//FC設定フラグ初期化
			FCsetflg = 0;

			//FCﾌｧｲﾙ読込
			//FcRead

			//v11.2以下に変更ここから by 間々田 2006/01/12
			FcRead(@"C:\Ct\filter\FC1.dat", ref FC1);
			FcRead(@"C:\Ct\filter\FC2.dat", ref FC2);
			FcRead(@"C:\Ct\filter\FC3.dat", ref FC3);
			FcRead(@"C:\Ct\filter\FC4.dat", ref FC4);
			FcRead(@"C:\Ct\filter\FC5.dat", ref FC5);
			FcRead(@"C:\Ct\filter\FC6.dat", ref FC6);
			FcRead(@"C:\Ct\filter\FC7.dat", ref FC7);
			FcRead(@"C:\Ct\filter\FC8.dat", ref FC8);
			FcRead(@"C:\Ct\filter\FC9.dat", ref FC9);
            //追加2014/10/07hata_v19.51反映
            FcRead(@"C:\Ct\filter\FC10.dat", ref FC10);            //v18.00追加 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            FcRead(@"C:\Ct\filter\FC11.dat", ref FC11);            //v18.00追加 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            FcRead(@"C:\Ct\filter\FC12.dat", ref FC12);            //v18.00追加 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //v11.2以下に変更ここまで by 間々田 2006/01/12

            //終端が0でないFC関数のときはFC*nの方を採用する  'v18.00追加ここから 2011/03/10 by 間々田
            if (CTSettings.scaninh.Data.fcend_not0 == 0)
            {
                FcRead(@"C:\Ct\filter\FC1n.dat", ref FC1n);
                FcRead(@"C:\Ct\filter\FC2n.dat", ref FC2n);
                FcRead(@"C:\Ct\filter\FC3n.dat", ref FC3n);
                FcRead(@"C:\Ct\filter\FC4n.dat", ref FC4n);
                FcRead(@"C:\Ct\filter\FC5n.dat", ref FC5n);
                FcRead(@"C:\Ct\filter\FC6n.dat", ref FC6n);
                FcRead(@"C:\Ct\filter\FC7n.dat", ref FC7n);
                FcRead(@"C:\Ct\filter\FC8n.dat", ref FC8n);
                FcRead(@"C:\Ct\filter\FC9n.dat", ref FC9n);
                FcRead(@"C:\Ct\filter\FC10n.dat", ref FC10n);
                FcRead(@"C:\Ct\filter\FC11n.dat", ref FC11n);
                FcRead(@"C:\Ct\filter\FC12n.dat", ref FC12n);
            }
            //v18.00追加ここまで 2011/03/10 by 間々田

			//ﾌｨﾙﾀFCをｸﾞﾗﾌ表示
			FcGraph();

			//コントロールの初期化
            //panelコントロールのためBorderStyleの設定なし。これは不要　//変更2014/10/07hata_v19.51反映
            //fraMenu1.BorderStyle = BorderStyle.None;
            //fraMenu2.BorderStyle = BorderStyle.None;
            //fraMenu3.BorderStyle = BorderStyle.None;
            //fraMenu4.BorderStyle = BorderStyle.None;
            //fraMenu5.BorderStyle = BorderStyle.None;			//added by 山本 2006-12-27
            ////v19.50 v18.02とv19.41の統合 by長野 2013/12/19
            ////v18.00変更 byやまおか 2011/02/04
            //for (i = fraMenu.LBound(); i <= fraMenu.UBound(); i++)
            //{
            //    fraMenu[i].BorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //}

			//V6.0 append by 間々田 2002/07/10
			sstMenu.SelectedIndex = 0;

			//FCDの初期化    V6.0 append by 間々田 2002/07/10
			for (i = 0; i <= 5; i++)
			{
				float.TryParse(txtFCD[i].Text, out FCD[i]);
			}

        
 		//◆◆◆◆◆ 検出器シフト調整タブ ◆◆◆◆◆ 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

		//初期設定
		for (i = txtFilepath.GetLowerBound(0); i <= txtFilepath.GetUpperBound(0); i++) {
			txtFilepath[i].Text = "";
		}

		ntbDetShiftLength.Value = 0;
		ntbFpdPitch.Value = 0;
		ntbDistance.Value = 0;

		ntbGradient.Value = 0;
		ntbVertical.Value = 0;
		ntbHolizontal.Value = 0;
		ntbCalculatedShiftLength.Value = 0;

		//lblJudgeGradient.Caption = "NG"
		//lblJudgeVertical.Caption = "NG"
		//lblJudgeHolizontal.Caption = "NG"
		//v18.00変更 byやまおか 2011/09/14
		lblJudgeGradient.Text = "-";
		lblJudgeVertical.Text = "-";
		lblJudgeHolizontal.Text = "-";
		lblJudgeGradient.ForeColor = System.Drawing.Color.Black;
		lblJudgeVertical.ForeColor = System.Drawing.Color.Black;
		lblJudgeHolizontal.ForeColor = System.Drawing.Color.Black;

		//パラメータ読み込み
		//cwnDebugShiftLength.Value = 1
        //変更2015/02/02hata_Max/Min範囲のチェック
		//cwnDebugShiftLength.Value = Convert.ToDecimal(CTSettings.scancondpar.Data.det_sft_length);
        cwnDebugShiftLength.Value = modLibrary.CorrectInRange(Convert.ToDecimal(CTSettings.scancondpar.Data.det_sft_length), cwnDebugShiftLength.Minimum, cwnDebugShiftLength.Maximum);

		//v18.00修正 byやまおか 2011/03/07
		SetParameter(Convert.ToDouble(cwnDebugShiftLength.Value));
        //変更2015/02/02hata_Max/Min範囲のチェック
        //cwnMoveDx.Value = CTSettings.scancondpar.Data.det_sft_pix;
        cwnMoveDx.Value = modLibrary.CorrectInRange(CTSettings.scancondpar.Data.det_sft_pix, cwnMoveDx.Minimum, cwnMoveDx.Maximum);

#if DebugOn //'v18.00修正 byやまおか 2011/03/21
		cwnDebugShiftLength.Visible = true;
#else
		cwnDebugShiftLength.Visible = false;
#endif
	
        fraRoi.Visible = true;
		fraOverlay.Visible = true;

		//ROIの初期値    'v18.00追加 byやまおか 2011/09/14
        cwnRoiXs.Value = (modDetShift.Roi_Xs != 0 ? modDetShift.Roi_Xs : 400);
        cwnRoiXe.Value = (modDetShift.Roi_Xe != 0 ? modDetShift.Roi_Xe : CTSettings.scancondpar.Data.h_size - 400);
        cwnRoiYs.Value = (modDetShift.Roi_Ys != 0 ? modDetShift.Roi_Ys : 200);
        cwnRoiYe.Value = (modDetShift.Roi_Ye != 0 ? modDetShift.Roi_Ye : CTSettings.scancondpar.Data.v_size - 200);

		//透視左右反転   'v18.00追加 byやまおか 2011/09/14
        cwbtnTransDispLRInv.Value = (CTSettings.scaninh.Data.transdisp_lr_inv == 0);


		//◆◆◆◆◆ X線装置タブ ◆◆◆◆◆  'v18.00追加 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

		//ロード時の選択は[表示する]
        optMaxVoltDisp[0].Checked = true;

		//ロード時の選択は[2週間以上]
        optWarmupMode[0].Checked = true;

		//ロード時は[外部制御]
		modTitan.Ti_SetTitanKBLock(0);
		stsRemoteControl.Status = StringTable.GC_STS_REMOTE;

        //◆◆◆◆◆ X線、検出器昇降タブ ◆◆◆◆◆  'Rev20.00 追加 by長野 2015/02/16
        tmrUdStatus.Enabled = true;

        //追加2015/01/27hata
        //使用していないTabは非表示にする
        if (sstMenu.TabPages.Contains(tabPage2)) sstMenu.TabPages.Remove(tabPage2);   //コーンビーム調整
        if (sstMenu.TabPages.Contains(tabPage3)) sstMenu.TabPages.Remove(tabPage3);   //フラットパネル
        if (sstMenu.TabPages.Contains(tabPage4)) sstMenu.TabPages.Remove(tabPage4);   //Ｙ軸傾斜角度測定
        if (sstMenu.TabPages.Contains(tabPage5)) sstMenu.TabPages.Remove(tabPage5);   //Viscomメンテナンス
		//シフト調整を最初に表示する   'v18.00追加 byやまおか 2011/03/14
		sstMenu.SelectedIndex = 6;

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
		private void frmMaint_FormClosed(object sender, FormClosedEventArgs e)
		{
			//フィルタ関数パラメータフォームのアンロード
			frmFCpara.Instance.Close();

			//終了時はフラグをリセット
			modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTMaintenance);

            //追加2014/10/07hata_v19.51反映
            //デリゲートを破棄
            if (myCallback != null) myCallback = null;

		}


		//
		//フィルタ関数設定関連イベント処理
		//
		//*******************************************************************************
		//機　　能： FC設定ボタンクリック時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdSet_Click(object sender, EventArgs e)
		{
            if (frmFCpara.Instance.Visible) return;
            cmdChange.Enabled = false;

            //マウスポインタを砂時計にする
			this.Cursor = Cursors.WaitCursor;

			//FC設定ﾌﾗｸﾞ立てる
            FCsetflg = 1;                
            
            frmFCpara.Instance.WindowState = FormWindowState.Minimized;

            //変更2015/01/17hata
            //frmFCpara.Instance.Show();
            if (!modLibrary.IsExistForm("frmFCpara"))	//追加2015/01/30hata_if文追加
            {
                frmFCpara.Instance.Show(frmCTMenu.Instance);
            }
            else
            {
                frmFCpara.Instance.WindowState = FormWindowState.Normal;
                frmFCpara.Instance.Visible = true;
            }

            //FCパラメータ設定
            frmFCpara.Instance.FCparaset();

			//フィルタ関数作成
			FcSet();

			//フィルタ関数グラフ表示
			FcGraph();

			//マウスポインタを元に戻す
			this.Cursor = Cursors.Default;

            frmFCpara.Instance.Close();
            
            cmdChange.Enabled = true;

		}

		//*******************************************************************************
		//機　　能： パラメータボタンクリック時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdChange_Click(object sender, EventArgs e)
		{
            if (frmFCpara.Instance.Visible) return;

            //フィルタ関数パラメータ設定フォーム表示
            if (!modLibrary.IsExistForm("frmFCpara"))	//追加2015/01/30hata_if文追加
            {
                frmFCpara.Instance.Show(frmCTMenu.Instance);
            }
            else
            {
                frmFCpara.Instance.WindowState = FormWindowState.Normal;
                frmFCpara.Instance.Visible = true;
            }

        }


		//
		//コーンビーム調整関連イベント処理
		//
		private void optCone_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null) return;
			if (((RadioButton)sender).Checked == false) return;

			Maint.ConeAdjustFlg = 1;
		}

		private void optNormal_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null) return;
			if (((RadioButton)sender).Checked == false) return;

			Maint.ConeAdjustFlg = 0;
		}


		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//'
		//'Viscomメンテナンス関連イベント処理     added by 山本　2006-12-27
		//'
		//Private Sub optNormalViscom_Click()
		//
		//    ViscomMaintFlg = 0
		//
		//End Sub
		//
		//Private Sub optMaintViscom_Click()
		//
		//    ViscomMaintFlg = 1
		//
		//End Sub
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
		//
		//Y軸傾斜角度測定関連イベント処理
		//
		// V6.0 append by 間々田 2002/07/10 START
		private void txtFCD_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (sender as TextBox == null) return;
			int Index = Array.IndexOf(txtFCD, sender);
			if (Index < 0) return;


			bool Cancel = false;

			//入力キーを限定
			switch (e.KeyChar)
			{
				case (char)Keys.D0:
				case (char)Keys.D1:
				case (char)Keys.D2:
				case (char)Keys.D3:
				case (char)Keys.D4:
				case (char)Keys.D5:
				case (char)Keys.D6:
				case (char)Keys.D7:
				case (char)Keys.D8:
				case (char)Keys.D9:
				case (char)Keys.Back:
				case (char)Keys.Escape:
					break;

				case (char)Keys.Return:
					CancelEventArgs CancelEventArgs = new CancelEventArgs(Cancel);
					txtFCD_Validating(txtFCD[Index], CancelEventArgs);
					Cancel = CancelEventArgs.Cancel;
					if ((!Cancel) && Index < 5) txtFCD[Index + 1].Focus();
					break;

				default:
					e.KeyChar = (char)0;
					e.Handled = true;
					System.Media.SystemSounds.Beep.Play();
					break;
			}
		}


		private void txtFCD_Validating(object sender, CancelEventArgs e)
		{
			if (sender as TextBox == null) return;
			int Index = Array.IndexOf(txtFCD, sender);
			if (Index < 0) return;

			bool OK = false;
			float N = 0;
			float.TryParse(txtFCD[Index].Text, out N);

			//入力した値をチェック
			switch (Index)
			{
				case 0:
					if ((0 < N) && (N < FCD[Index + 1])) OK = true;
					break;

				case 5:
					if (FCD[Index - 1] < N) OK = true;
					break;

				default:
					if ((FCD[Index - 1] < N) && (N < FCD[Index + 1])) OK = true;
					break;
			}

			if (OK)
			{
				FCD[Index] = N;
			}
			else
			{
				//メッセージ表示：正の数で昇順となるように入力してください。
				MessageBox.Show(CTResources.LoadResString(9524), Application.ProductName, MessageBoxButtons.OK);
				txtFCD[Index].Text = Convert.ToString(FCD[Index]);
				e.Cancel = true;
				txtFCD[Index].Focus();
			}
		}
		// V6.0 append by 間々田 2002/07/10 END


		//********************************************************************************
		//機    能  ：  Ｙ軸傾斜角度測定：「実行」ボタンクリック時処理
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V6.0   02/08/23  (SI4)間々田       新規作成
		//********************************************************************************
		private void cmdDrawYincli_Click(object sender, EventArgs e)
		{
			int i = 0;
			float a = 0;
			float b = 0;
			float[] x = new float[6];
			float[] wc = new float[6];
			int sts = 0;

			// append by 巻渕 2002-10-29 Start
			//Ｘオフセット値を算出する
			float xr = 0;
			// append by 巻渕 2002-10-29 end

			//実行ボタンを使用不可にする
			cmdDrawYincli.Enabled = false;

			//フォームに対するイベントを禁止
			this.Enabled = false;

			//マウスカーソルを砂時計にする
			Cursor.Current = Cursors.WaitCursor;

			//タッチパネル操作を禁止
			modSeqComm.SeqBitWrite("PanelInhibit", true);

			//コモンファイルの SCANSEL から各値を読みとる
			ScanCorrect.OptValueGet_Cor();

			//'Ｘ線停止要求クリア   'v11.5追加 by 間々田 2006/10/23
			//UserStopClear
			//
			//'連続回転コーンビーム＋高速再構成の時は、RAMディスクのscanstopを使う v17.40 追加 by 長野
			//If smooth_rot_cone_flg = True Then
			//
			//    UserStopClear_rmdsk
			//
			//End If

			//停止要求フラグをクリアする             'v17.50上記の処理を関数化 by 間々田 2011/02/17
			modCT30K.CallUserStopClear();

			//Ｘ線ＯＮ処理
			sts = modXrayControl.TryXrayOn();

			//Ｘ線がOFF状態の場合
			//If (sts <> 0) Or (XrayControl.Up_X_On = 0) Then
			//If (sts <> 0) Or (Not IsXrayOn) Then 'v11.3変更 by 間々田 2006/02/17
			if ((sts != 0) || (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OffStatus))			//v11.5変更 by 間々田 2006/06/21
			{
				Cursor.Current = Cursors.Default;

				//エラーメッセージ表示：Ｘ線ＯＮ処理に失敗しました。
				MessageBox.Show(CTResources.LoadResString(9371), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				goto ExitHandler;
			}

			//イメージプロ起動
			modCT30K.StartImagePro();

			//配列の再構成
			ScanCorrect.W_IMAGE = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];		//v15.0変更 -1した by 間々田 2009/06/03

			//タイマーを一時的にストップ
			//frmStatus.StopTimer        'v11.5削除 by 間々田 2006/06/12

			//各FCDに対して処理
			for (i = 0; i <= 5; i++)
			{
				//指定ＦＣＤ位置まで試料テーブルを移動
                //2014/11/07hata キャストの修正
                //if (modSeqComm.MoveFCD((int)Math.Truncate(FCD[i] * 10)))
                //if (modSeqComm.MoveFCD(Convert.ToInt32(Math.Truncate(FCD[i] * 10))))
                if (modSeqComm.MoveFCD(Convert.ToInt32(Math.Truncate(FCD[i] * modMechaControl.GVal_FCD_SeqMagnify))))//Rev23.10 変更 by長野 2015/09/18
                {
					// changed by 巻渕 2002/10/28 START 回転中心を求めるよう変更
					//'            '透視画像取り込み２０枚積算
					//'            Call II_Data_Acquire(H_SIZE, V_SIZE, 20, 1, IMG1(0), W_IMAGE(0), 0, GVal_v_mag)
					//'
					//'            '回転中心のファントムのワイヤ位置 wc(i) の求出
					//'            Call Get_WirePosition(wc(i))

					//回転中心を求める
					if (!CenteringForMaint())
					{
						Cursor.Current = Cursors.Default;
						//メッセージ表示：回転中心求出に失敗しました。もう一度やり直してください。
						MessageBox.Show(CTResources. LoadResString(9464), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
						break;
					}

					wc[i] = (float)ScanCorrect.xlc[2];  // changed by 巻渕 2002/10/28 END   回転中心を求めるよう変更
				}
				//移動できなかった場合
				else
				{
					Cursor.Current = Cursors.Default;
					//メッセージ表示：指定ＦＣＤ位置まで試料テーブルを移動することができませんでした。
					MessageBox.Show(CTResources.LoadResString(9504), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				}
			}

			//frmStatusのタイマを元に戻す
			//frmStatus.RestartTimer         'v11.5削除 by 間々田 2006/06/12

			//Ｘ線ＯＦＦ
            if (CTSettings.scaninh.Data.xray_remote == 0) modXrayControl.XrayOff();

			//各FCD位置でのワイヤのｘ座標(i)を求める
			for (i = 0; i<= 5; i++)
			{
				//        x(i) = (wc(i) - Int(h_size / 2)) * (10 / A1(2)) * ((FCD(i) + GVal_FcdOffset(GFlg_MultiTube)) / (GVal_Fid + GVal_FidOffset(GFlg_MultiTube)))
                //2014/11/07hata キャストの修正
                //x[i] = (wc[i] - (float)(((int)(CTSettings.detectorParam.h_size / 2)) * (10 / ScanCorrect.A1[2]) * ((FCD[i] + CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()]) / (ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube]))));		//v9.0 change Fid/Fcdオフセットの変更に伴う by 間々田 2004/02/03
                x[i] = (float)((wc[i] - (Math.Floor(CTSettings.detectorParam.h_size / 2F))) * 
                               (10 / ScanCorrect.A1[2]) * 
                               ((FCD[i] + CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()]) / (ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube])));		//v9.0 change Fid/Fcdオフセットの変更に伴う by 間々田 2004/02/03
            }

			//y(i),x(i)からフィッティング計算によりＹ軸傾斜の１次式 x=ay+b を求める
			Get_Primary_Fomula(x, FCD, ref a, ref b);

			//フォームにＹ軸傾斜の１次式等を描画する
			Draw_Yincli(x, FCD, ref a, ref b);

			//Ｙ軸傾斜角度を算出する
			y_incli = (float)Math.Atan(a);

			// append by 巻渕 2002-10-29 Start
            ref_fid = ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube];

			//'校正処理データの読み込み（シーケンサ通信が可能な場合）
			//If MySeq Is Nothing Then
			//    Screen.MousePointer = vbDefault
			//    'エラー表示：シーケンサ通信を可能に設定してください。
			//    MsgBox LoadResString(12726), vbExclamation  '
			//    Exit Sub
			//End If

			//       xr = iSeqCor.stsXPosition / 10    '削除 巻渕 2003-03-03 ''旧バージョン6.3用
            //2014/11/07hata キャストの修正
            //xr = modSeqComm.MySeq.stsXPosition / 100;		//追加 巻渕 2003-03-03   小数点以下2桁まで対応
            //xr = Convert.ToInt32(modSeqComm.MySeq.stsXPosition / 100F);		//追加 巻渕 2003-03-03   小数点以下2桁まで対応
            xr = Convert.ToInt32(modSeqComm.MySeq.stsXPosition / (float)modMechaControl.GVal_TableX_SeqMagnify);//Rev23.10 変更 by長野 2015/09/18

			//Ｘオフセット値を算出する
			x_offset = b - xr;
			// append by 巻渕 2002-10-29 end
			//フォーム左下にＹ軸傾斜角度を度単位で表示する(ラジアン値から度に変更 by 間々田 2002-09-24）
			//    lblYincli.Caption = CStr(Format(y_incli * 180 / Pai, "0.0000")) & " 度"
            //2014/11/07hata キャストの修正
            //lblYincli.Text = (y_incli * 180 / ScanCorrect.Pai).ToString("0.0000") + " " + CTResources.LoadResString(10816);		//v7.0 リソース対応 by 間々田 2003/08/12
            lblYincli.Text = (y_incli * 180F / ScanCorrect.Pai).ToString("0.0000") + " " + CTResources.LoadResString(10816);		//v7.0 リソース対応 by 間々田 2003/08/12
            lblYincli.Visible = true;
			// append by 巻渕 2002-10-28 start
			lblXoffset.Text = x_offset.ToString("0.0000") + " mm";
			lblXoffset.Visible = true;
			// append by 巻渕 2002-10-28 end

			//保存ボタンを使用可にする
			cmdSave.Enabled = true;

		ExitHandler:

			//タッチパネル操作を許可
			modSeqComm.SeqBitWrite("PanelInhibit", false);

			//マウスカーソルを元に戻す
			Cursor.Current = Cursors.Default;

			//実行ボタンを使用可にする
			cmdDrawYincli.Enabled = true;

			//フォームに対するイベントを許可
			this.Enabled = true;
		}


		//********************************************************************************
		//機    能  ：  Ｙ軸傾斜角度測定：保存ボタンが押された時の処理
		//
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//
		//補    足  ：
		//
		//履    歴  ：  V6.0   02/07/09  (SI4)間々田      新規作成
		//********************************************************************************
		private void cmdSave_Click(object sender, EventArgs e)
		{
			string[] buf = new string[3];

			//scancondpar（コモン）の更新 v11.5変更 by 間々田 2006/04/26
            CTSettings.scancondpar.Data.y_incli = y_incli;		//Ｙ軸傾斜角度をコモンに保存する
            CTSettings.scancondpar.Data.x_offset = x_offset;		//Ｘオフセットをコモンに保存する
            CTSettings.scancondpar.Data.ref_fid = ref_fid;		//基準fid

			//modScancondpar.CallPutScancondpar();
            CTSettings.scancondpar.Write();

			//以下 setfile に保存する処理
			try
			{
				StreamReader sr = null;
				StreamWriter sw = null;

				try
				{
					//CSVファイルオープン
                    //変更2015/01/22hata
                    //sr = new StreamReader(AppValue.SCANPOSI_CSV);
                    sw = new StreamWriter(AppValue.SCANPOSI_CSV, false, Encoding.GetEncoding("shift-jis"));

					buf[0] = sr.ReadLine();
					buf[1] = sr.ReadLine();
					buf[2] = sr.ReadLine();
				}
				catch
				{
					throw;
				}
				finally
				{
					if (sr != null)
					{
						sr.Close();
						sr = null;
					}
				}

				try
				{
					//CSVファイルオープン
                    //変更2015/01/22hata
                    //sw = new StreamWriter(AppValue.SCANPOSI_CSV);
                    sw = new StreamWriter(AppValue.SCANPOSI_CSV, false, Encoding.GetEncoding("shift-jis"));

					sw.WriteLine(buf[0]);
					sw.WriteLine(buf[1]);
					sw.WriteLine(buf[2]);
					sw.WriteLine(lblYAxisAngleHeader.Text + ",y_incli," + y_incli.ToString("0.0000000"));								//Ｙ軸傾斜角度
					sw.WriteLine(lblXOffsetHeader.Text + ",x_offset," + x_offset.ToString("0.0000000"));								//Ｘオフセット
					sw.WriteLine(CTResources.LoadResString(StringTable.IDS_RefFid) + ",ref_fid," + ref_fid.ToString("0.0000000"));		//基準fid
				}
				catch
				{
					throw;
				}
				finally
				{
					if (sw != null)
					{
						sw.Close();
						sw = null;
					}
				}

				//メッセージ表示：Ｙ軸傾斜角度を保存しました。
				MessageBox.Show(StringTable.GetResString(StringTable.IDS_Saved, lblYAxisAngleHeader.Text), 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\r\n" + "\r\n" + AppValue.SCANPOSI_CSV, 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		//********************************************************************************
		//機    能  ：  Ｙ軸傾斜１次式の等の描画
		//              変数名           [I/O] 型        内容
		//引    数  ：  x()              [I/ ] Single    x軸座標の配列
		//          ：  y()              [I/ ] Single    y軸座標の配列
		//          ：  a                [I/ ] Single    y=ax+b の a
		//          ：  b                [I/ ] Single    y=ax+b の b
		//戻 り 値  ：                   [ /O] Boolean   結果(True:正常,False:異常)
		//補    足  ：
		//
		//履    歴  ：  V6.0   02/07/11  (SI4)間々田       新規作成
		//********************************************************************************
		private bool Draw_Yincli(float[] x, float[] y, ref float a, ref float b)
		{
			bool functionReturnValue = false;

			const int x_max = 100;
			const int x_min = -100;
			const int y_max = 1000;
			const int y_min = 0;

			float Pa = 0;
			float px = 0;
			float py = 0;
			int i = 0;

			try
			{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				'クリア
				picYincli.Cls

				'座標系設定
				picYincli.Scale (y_min, x_max)-(y_max, x_min)
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				//クリア
				if (picYincli.Image != null) picYincli.Image.Dispose();

				// グラフを描画する Graphics オブジェクトを作成
				picYincli.Image = new Bitmap(picYincli.Width, picYincli.Height);
				Graphics g = Graphics.FromImage(picYincli.Image);


				//座標系設定
				int x1 = y_min;
				int y1 = x_max;
				int x2 = y_max;
				int y2 = x_min;
				float dX = x2 - x1;
				float dY = y2 - y1;

				// グラフのプロットを PictureBox上の座標に変換する係数
				float paramX = (picYincli.ClientSize.Width - 1) / dX;
				float paramY = (picYincli.ClientSize.Height - 1) / dY;

				// 座標系に合せてグラフの原点を変更する
				g.TranslateTransform(dX < 0 ? (picYincli.ClientSize.Width - 1) * Math.Abs(x1 / dX) : 0,
									 dY < 0 ? (picYincli.ClientSize.Height - 1) * Math.Abs(y1 / dY) : 0);


				//Ｘ軸のラベル：位置の調整
                //2014/11/07hata キャストの修正
                //lblX.Location = new Point((picYincli.Left - lblX.Width - 3), (picYincli.Top - lblX.Height / 2));
                lblX.Location = new Point((picYincli.Left - lblX.Width - 3), (picYincli.Top - Convert.ToInt32(lblX.Height / 2F)));

				//Ｙ軸のラベル：位置の調整
                //2014/11/07hata キャストの修正
                //lblY.Location = new Point((picYincli.Left + picYincli.Width - lblY.Width / 2), (picYincli.Top + picYincli.Height + 3));
                lblY.Location = new Point((picYincli.Left + picYincli.Width - Convert.ToInt32(lblY.Width / 2F)), (picYincli.Top + picYincli.Height + 3));

				//Ｘ軸の中央ラベル（原点）：位置の調整
                //2014/11/07hata キャストの修正
                //lblX0Y0.Location = new Point((picYincli.Left - lblX0Y0.Width - 3),
                //                             (picYincli.Top - (int)(picYincli.ClientRectangle.Top * picYincli.Height / (float)picYincli.ClientRectangle.Height) - lblX0Y0.Height / 2));
                lblX0Y0.Location = new Point((picYincli.Left - lblX0Y0.Width - 3),
                                             (picYincli.Top - Convert.ToInt32((float)picYincli.ClientRectangle.Top * (float)picYincli.Height / (float)picYincli.ClientRectangle.Height) - Convert.ToInt32(lblX0Y0.Height / 2F)));

				//Ｘ＝０を引く
				g.DrawLine(Pens.Black, paramX * y_min, 0, paramX * y_max, 0);

				//原点からＰ1(py,px),Ｐ2(py,-px)を結ぶ線分を引くための傾きPaを求める
                //2014/11/07hata キャストの修正
                //px = (float)(10 / ScanCorrect.A1[2]) * (CTSettings.detectorParam.h_size / 2);
                px = (float)(10F / ScanCorrect.A1[2]) * (CTSettings.detectorParam.h_size / 2F);
                py = ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube];
				Pa = px / py;

				//原点からＰ1,Ｐ2を結ぶ線分を引く
				g.DrawLine(Pens.Cyan, 0, 0, paramX * y_max, paramY * (Pa * y_max));
				g.DrawLine(Pens.Cyan, 0, 0, paramX * y_max, paramY * (- Pa * y_max));

				//Ｙ軸傾斜１次式 x=ay+b を描画
				g.DrawLine(Pens.Blue, 0, paramY * b, paramX * y_max, paramY * (a * y_max + b));

				//測定点(y(i),x(i))をプロット
				float radius = paramX * 10;
				for (i = x.GetLowerBound(0); i <= x.GetUpperBound(0); i++)
				{
					g.DrawEllipse(Pens.Blue, paramX * y[i] - radius, paramY * x[i] - radius, radius * 2, radius * 2);
				}

				g.Dispose();

				functionReturnValue = true;
			}
			catch (Exception)
			{
				modLibrary.ErrorDescription(StringTable.GetResString(9904, this.Name, "Draw_Yincli"));
				functionReturnValue = false;
			}

			return functionReturnValue;
		}


		//********************************************************************************
		//機    能  ：  Ｙ軸傾斜１次式の求出関数
		//              変数名           [I/O] 型        内容
		//引    数  ：  x()              [I/ ] Single    x軸座標の配列
		//          ：  y()              [I/ ] Single    y軸座標の配列
		//          ：  a                [ /O] Single    y=ax+b の a
		//          ：  b                [ /O] Single    y=ax+b の b
		//戻 り 値  ：                   [ /O] Boolean   結果(True:正常,False:異常)
		//補    足  ：
		//
		//履    歴  ：  V6.0   02/07/11  (SI4)間々田       新規作成
		//********************************************************************************
		private bool Get_Primary_Fomula(float[] x, float[] y, ref float a, ref float b)
		{
			bool functionReturnValue = false;

			float Sigma_x = 0;
			float Sigma_y = 0;
			float Sigma_y2 = 0;
			float Sigma_xy = 0;
			float c = 0;

			int j = 0;
			int N = 0;

			N = x.GetUpperBound(0) - x.GetLowerBound(0) + 1;

			//カウンタの初期化
			Sigma_x = 0;
			Sigma_y = 0;
			Sigma_y2 = 0;
			Sigma_xy = 0;

			for (j = x.GetLowerBound(0); j <= x.GetUpperBound(0); j++)
			{
				Sigma_x = Sigma_x + x[j];
				Sigma_y = Sigma_y + y[j];
				Sigma_y2 = Sigma_y2 + y[j] * y[j];
				Sigma_xy = Sigma_xy + x[j] * y[j];
			}

			//a,b算出用の共通分母
			c = N * Sigma_y2 - Sigma_y * Sigma_y;

			if (Math.Abs(c) > 0)
			{
				a = (N * Sigma_xy - Sigma_x * Sigma_y) / c;
				b = (Sigma_x * Sigma_y2 - Sigma_y * Sigma_xy) / c;
				functionReturnValue = true;
			}
			else
			{
				functionReturnValue = false;
			}

			return functionReturnValue;
		}


		//********************************************************************************
		//機    能  ：  回転中心求出関数
		//              変数名           [I/O] 型        内容
		//戻 り 値  ：                   [ /O] Boolean   結果(true:正常,false:異常)
		//補    足  ：  400View/積算枚数1枚で回転中心を求める
		//
		//履    歴  ：  V6.0   02/10/28  (CATS1)巻渕       新規作成
		//********************************************************************************
		private bool CenteringForMaint()
		{
			//Dim temp_GVal_View      As Integer
			//Dim temp_GVal_ScnInteg  As Integer
			//Dim temp_GVal_ScnSlWid  As Integer
			float fFID = 0;			//FID(FID+FIDｵﾌｾｯﾄ)
			float fFCD = 0;			//FCD(FCD+FCDｵﾌｾｯﾄ)
			float fMDT = 0;			//検出器ﾋﾟｯﾁ(mm/画素)
			int iVMG = 0;			//縦倍率
			float fSLW = 0;			//ｽﾗｲｽ厚(mm)
			float fPIX = 0;			//スライス厚(画素)
			int rc = 0;

			//戻り値初期化
			bool functionReturnValue = false;

			try
			{
				//'グローバル変数退避
				//temp_GVal_View = GVal_View
				//temp_GVal_ScnInteg = GVal_ScnInteg
				//temp_GVal_ScnSlWid = GVal_ScnSlWid

				//現在値読み込み
                fFID = ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube];
                fFCD = ScanCorrect.GVal_Fcd + CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()];		//v9.0 change Fid/Fcdオフセットの変更に伴う by 間々田 2004/02/03
				fMDT = ScanCorrect.GVal_mdtpitch[2];
                //2014/11/07hata キャストの修正
                //iVMG = (int)(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);				//V7.0 FPD対応 by 間々田 2003/09/09
                iVMG = Convert.ToInt32(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);				//V7.0 FPD対応 by 間々田 2003/09/09

				fPIX = 20.0F;
				fSLW = ScanCorrect.Trans_PixToWid(fPIX, fFID, fFCD, fMDT, iVMG);

				//'View数、積分枚数、スライス厚指定
				//GVal_View = 200
				//GVal_ScnInteg = 1
				//GVal_ScnSlWid = fSLW

				//マウスカーソルを砂時計にする
				//Screen.MousePointer = vbHourglass
				Cursor.Current = Cursors.AppStarting;		//v10.0変更 by 間々田 2005/02/09 矢印つきの砂時計にする

				//回転中心校正画像を配列に読み込む
				//If Not Get_RotationCenter_ImageForMaint(0) Then
				//If Not GetImageRotationCenterCorrect(0) Then               'v9.7変更 by 間々田 2004/12/03
				if (!modScanCorrectNew.GetImageRotationCenterCorrect(0, 200, 1, fSLW, null, null))		//v10.0変更 by 間々田 2005/02/04
				{
					//マウスカーソルを元に戻す
					Cursor.Current = Cursors.Default;

					//'グローバル変数を元に戻す
					//GVal_View = temp_GVal_View
					//GVal_ScnInteg = temp_GVal_ScnInteg
					//GVal_ScnSlWid = temp_GVal_ScnSlWid

					//終了手続き
					return functionReturnValue;
				}

				//回転中心パラメータ計算
				//rc = Get_RotationCenter_Parameter_Ex(0)
				rc = ScanCorrect.Get_RotationCenter_Parameter_Ex(0, 200);			//v10.0変更 by 間々田 2005/02/04

				//マウスカーソルを元に戻す
				Cursor.Current = Cursors.Default;

				//'グローバル変数を元に戻す
				//GVal_View = temp_GVal_View
				//GVal_ScnInteg = temp_GVal_ScnInteg
				//GVal_ScnSlWid = temp_GVal_ScnSlWid

				//エラー判定
				if (rc == 0)
				{
					//メカ制御ボードの初期化
					rc = modMechaControl.RotateInit(modDeclare.hDevID1);
				}
				else if (rc == 1)
				{ }
				else
				{
					//メカ制御ボードの初期化
					rc = modMechaControl.RotateInit(modDeclare.hDevID1);

					//終了手続き
					return functionReturnValue;
				}

				functionReturnValue = true;
			}
			//例外処理
			catch (Exception)
			{
				//メッセージ表示：
				//   校正処理に失敗しました。
				//   回転中心校正を再度行ってください。
				MessageBox.Show(StringTable.BuildResStr(9319, StringTable.IDS_CorRot),
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			return functionReturnValue;
		}


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
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

			//タブ
			//sstMenu.TabCaption(3) = LoadResString(IDS_YAxisAngleMeas)           'Y軸傾斜角度測定
            sstMenu.TabPages[3].Text = StringTable.GetResString(StringTable.IDS_YAxisAngleMeas, CTSettings.AxisName[0]);		//v14.24変更 by 間々田 2009/03/10 コモンを使用

			//Y軸傾斜角度測定タブ
			//lblYAxisAngleHeader.Caption = LoadResString(IDS_YAxisAngle) & " :"  'Ｙ軸傾斜角度
            lblYAxisAngleHeader.Text = StringTable.GetResString(StringTable.IDS_YAxisAngle, CTSettings.AxisName[0]) + " :";	//v14.24変更 by 間々田 2009/03/10 コモンを使用

			//lblXOffsetHeader.Caption = LoadResString(IDS_XOffset) & " :"        'Ｘオフセット
            //変更2014/10/07hata_v19.51反映
            //lblXOffsetHeader.Text = StringTable.GetResString(StringTable.IDS_Offset, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[1].GetString())) + " :";	//v14.24変更 by 間々田 2009/03/10 コモンを使用
            lblXOffsetHeader.Text = StringTable.GetResString(StringTable.IDS_AxisOffset, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[1].GetString())) + " :";            //v18.00変更 IDS_Offset→IDS_AxisOffset byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

		    if (modCT30K.IsEnglish) lblYAxisAnglePrompt.Font = new Font(lblYAxisAnglePrompt.Font.Name, 10);			//英語環境の場合、フォントサイズ調整

			//v14.24追加 by 間々田 2009/03/10 コモンを使用
            lblX.Text = modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[1].GetString()) + "(mm)";
			//lblY.Caption = RemoveNull(infdef.m_axis_name(0)) & "(FCD) (mm)"
            lblY.Text = modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[0].GetString()) + "(" + CTResources.LoadResString(12824) + ")(mm)";		//ストリングテーブル化 'v17.60 by 長野 2011/5/22

            //削除2015/01/28hata
            //Rev20.00 追加 自動スキャン位置指定調整タブ 2015/01/16 by長野 2015/01/16
            //txtPosY.Text = StringTable.GetResString(12160, CTSettings.AxisName[1]);
            


            //追加2014/10/07hata_v19.51反映
            //◆◆◆◆◆ 検出器シフト調整タブ ◆◆◆◆◆ 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //単位設定
            string mm = null;
            string pixel = null;
            string agree = null;

            mm = "mm";
            pixel = "画素";
            agree = "度　";
            ntbDetShiftLength.Unit = mm;
            ntbFpdPitch.Unit = mm;
            ntbDistance.Unit = pixel;

            ntbGradient.Unit = agree;
            ntbVertical.Unit = pixel;
            ntbHolizontal.Unit = pixel;

            ntbCalculatedShiftLength.Unit = mm;

            //判定基準   'v18.00追加 byやまおか 2011/07/24
            lblStdGradient.Text = "XYに依存";
            lblStdVertical.Text = "±" + Convert.ToString(JudgeRange);
            lblStdHolizontal.Text = "小数部が" + "\r\n" + "±" + Convert.ToString(JudgeRange);

            //傾き調整方向
            switch (CTSettings.scancondpar.Data.detector)
            {
                case  (int)DetectorConstants.DetTypeII:
                    lblCw.Text = "+";
                    lblCcw.Text = "-";
                    break;
                case (int)DetectorConstants.DetTypeHama:
                case  (int)DetectorConstants.DetTypePke:
                    lblCw.Text = "-";
                    lblCcw.Text = "+";
                    break;
            }

            lblShift.Text = "Shift";    //追加2014/11/05hata
            lblDescrption.Text = "基準位置(シフト前)とシフト位置(シフト後)の２枚の透視画像からシフト量を算出して表示します。";            //追加2014/11/05hata
            ntbDetShiftLength.Caption = "L=";        //追加2014/11/05hata
            ntbFpdPitch.Caption = "⊿p=";            //追加2014/11/05hata
            ntbDistance.Caption = "S= L/⊿p=";       //追加2014/11/05hata
            lblTransDispLRInv.Text = "左右反転:";    //追加2014/11/05hata
            lblRoiStart.Text = "左上";               //追加2014/11/05hata
            lblRoiEnd.Text = "右下";                 //追加2014/11/05hata
            lblSelectTitle.Text = "透視画像の選択";  //追加2014/11/05hata
            lblPosition0.Text = "基準画像";          //追加2014/11/05hata
            lblPosition1.Text = "シフト画像";        //追加2014/11/05hata

            lblStdTitle.Text = "基準";               //追加2014/11/05hata
            lblJudgeTiltle.Text = "判定";            //追加2014/11/05hata

            ntbGradient.Caption = "傾き";            //追加2014/11/05hata
            ntbGradient.Unit = agree;                //追加2014/11/05hata
            ntbVertical.Caption = "Y方向(⊿j)";      //追加2014/11/05hata
            ntbVertical.Unit = pixel;                //追加2014/11/05hata
            ntbHolizontal.Caption = "X方向(⊿i)";    //追加2014/11/05hata
            ntbHolizontal.Unit = pixel;              //追加2014/11/05hata
            ntbCalculatedShiftLength.Caption = "予測シフト量⊿i/⊿p=";  //追加2014/11/05hata
            ntbCalculatedShiftLength.Unit = mm;      //追加2014/11/05hata

            cmdRef[0].Text = "参照";                 //追加2014/11/05hata
            cmdRef[1].Text = "参照";                 //追加2014/11/05hata
            cmdCalculate.Text = "計算";              //追加2014/11/05hata


            //◆◆◆◆◆ Ｘ線装置タブ ◆◆◆◆◆     'v18.00追加 byやまおか 2011/03/06 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            optWarmupMode[0].Text = CTResources.LoadResString(16222);
            optWarmupMode[1].Text = CTResources.LoadResString(16223);
            optWarmupMode[2].Text = CTResources.LoadResString(16224);
            optWarmupMode[3].Text = CTResources.LoadResString(16225);
            optWarmupMode[4].Text = CTResources.LoadResString(16227);
            optWarmupMode[5].Text = CTResources.LoadResString(16228);
            optWarmupMode[6].Text = CTResources.LoadResString(16229);
            optWarmupMode[7].Text = CTResources.LoadResString(StringTable.IDS_None);

            fraWarmupMode.Text = "X線ウォームアップの設定";      //追加2014/11/05hata
            fraRemoteControl.Text = "X線外部制御";               //追加2014/11/05hata
            fraMaxVoltDisp.Text = "最大管電圧表示";              //追加2014/11/05hata
            stsRemoteControl.Caption = "X線制御";              　//追加2014/11/05hata

            optMaxVoltDisp[0].Text = "表示する";               　//追加2014/11/05hata
            optMaxVoltDisp[1].Text = "表示しない";               //追加2014/11/05hata

            cmdWarmupMode.Text = "変更";                          //追加2014/11/05hata
            cmdRemoteControl.Text = "変更";                       //追加2014/11/05hata
            cmdMaxVoltDisp.Text = "変更";                         //追加2014/11/05hata

        }

		//
		//   フラットパネルタブ内：テキストボックス・変更処理                        '新規作成 by 間々田 2003/10/01
		//
		private void txtParam_TextChanged(object sender, EventArgs e)
		{
			double IsNumeric = 0;

			//テキストボックスの値がすべて数値の時、欠陥画像作成ボタンを使用可とする
			cmdDefImage.Enabled = double.TryParse(txtParam0.Text, out IsNumeric) &&
								  double.TryParse(txtParam1.Text, out IsNumeric) &&
								  double.TryParse(txtParam2.Text, out IsNumeric);
		}


        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから

        //◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆
        //◆◆◆◆◆◆◆◆◆◆ 検出器シフト調整タブ ◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆
        //◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆



        //********************************************************************************
        //機    能  ：  画像読み込み
        //              変数名           [I/O] 型        内容
        //引    数  ：  FileName         [I]String       画像ファイル名
        //              Info             [0]IPDOCINFO    画像情報
        //              WorkImage        [0]Integer()    画像バッファ
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V18.00  11/02/08  山影           新規作成
        //
        //********************************************************************************
        private bool LoadImage(string FileName, ref Maint.IPDOCINFO Info, ref ushort[] WorkImage)
	    {

		    bool functionReturnValue = false;

		    bool IsOK = false;
            int ret = 0;

        
		    //ファイル存在チェック
		    //IsOK = FSO.FileExists(FileName);
            IsOK = File.Exists(FileName);
		    if (!IsOK) {
			    //Interaction.MsgBox("ファイルを選んでください。");
                MessageBox.Show("ファイルを選んでください。");
                return functionReturnValue;
		    }

		    //拡張子チェック
		    string Ext = null;
		    //Ext = FSO.GetExtensionName(FileName);
            Ext = Path.GetExtension(FileName);
            Ext = Path.GetExtension(FileName).TrimStart('.');//Rev23.10 修正 by長野 2015/10/19
            switch (Ext.ToLower())
            {
                case "tif":
                case "tiff":
                    break;
			    default:
				    //Interaction.MsgBox("TIFを選択してください。");
                    MessageBox.Show("TIFを選択してください。");

                    return functionReturnValue;
		    }

		    //ファイル情報取得
		    //イメージプロ起動
		    if (!modCT30K.StartImagePro())
			    return functionReturnValue;

            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            /*
            //画像ウィンドウをすべて閉じる
		    ret = IpAppCloseAll();

		    //イメージプロで画像を開く
		    ret = IpWsLoad(FileName, Ext);

		    //ドキュメント情報を取得
		    ret = IpDocGet(GETDOCINFO, DOCSEL_ACTIVE, Info);
            */
            float[] Result = new float[10];
            ret = CallImageProFunction.CallLoadImageStep1(FileName, Ext, Result);
            //2014/11/07hata キャストの修正
            //Info.Width = (int)Result[0];
            //Info.Height = (int)Result[1];
            //Info.iClass = (int)Result[2];
            Info.Width = Convert.ToInt32(Result[0]);
            Info.Height = Convert.ToInt32(Result[1]);
            Info.iClass = Convert.ToInt32(Result[2]);
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

		    //付帯情報チェック
		    bool InfoOn = false;
		    short transwidth = 0;
		    //transwidth = 1024;
            //Rev23.0 修正 by長野 2015/10/19
            //transwidth = 2048;
            //Rev25.00 setfileからの読み込みに変更 by長野 2016/08/03
            transwidth = (short)CTSettings.scancondpar.Data.mainch[0];


            //Rev26.00 修正 by chouno 2018/01/13
            //InfoOn = (Info.Width != transwidth);
            //
		    //int ImageWidth = 0;
		    //ImageWidth = (InfoOn ? Info.Width - frmTransImageInfo.Instance.ClientRectangle.Width : Info.Width);
            int ImageWidth = Info.Width;

		    //透視画像領域のデータ取得
		    //ushort[] WordImage = null;
		    //byte[] ByteImage = null;
            //Rev23.10 修正 by長野 2015/10/19
            ushort[] WordImage = new ushort[ImageWidth * Info.Height];
            WorkImage = new ushort[ImageWidth * Info.Height];
            byte[] ByteImage = new byte[ImageWidth * Info.Height];

            //switch (Info.Class)
            switch (Info.iClass)
            {
                case 6:     //IMC_GRAY16:
                    //CTSettings.IPOBJ.GetWordImage(WordImage, 0, 0, Info.Width, Info.Height);
                    CTSettings.IPOBJ.GetWordImage(ref WordImage, 0, 0, Info.Width, Info.Height);

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    //ret = IpWsConvertImage(IMC_GRAY, CONV_SCALE, 0, 0, 0, 0);
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    ret = CallImageProFunction.CallIpWsConvertImage(0);
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

                    break;
                default:
                    //CTSettings.IPOBJ.GetByteImage(ByteImage,0 ,0 , Info.Width, Info.Height);
                    CTSettings.IPOBJ.GetByteImage(ref ByteImage, 0, 0, Info.Width, Info.Height);
                    break;
            }

		    //イメージプロのドキュメントをクローズ
            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            /*
                 ret = IpDocClose();
            */
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            ret = CallImageProFunction.CallIpDocClose();
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

		    //WorkImage = VB6.CopyArray(WordImage);
            WordImage.CopyTo(WorkImage, 0);
        

            functionReturnValue = true;

            //ErrorHandler: //使っていないのでコメント//変更2014/10/07hata_v19.51反映

		    
            return functionReturnValue;

		    //    Erase WordImage()
		    //    Erase ByteImage()

	}

        //********************************************************************************
        //機    能  ：  Y方向(⊿j)判定
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V18.00  11/02/08  山影           新規作成

        //********************************************************************************
        private bool JudgeDeltaJ(double dj)
        {

            //return InRangeFloat(dj, -JudgeRange, JudgeRange);
            return modLibrary.InRange(dj, -JudgeRange, JudgeRange);

        }

        //********************************************************************************
        //機    能  ：  X方向(⊿i)判定
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V18.00  11/02/08  山影           新規作成

        //********************************************************************************
        private bool JudgeDeltaI(double di)
        {

            int di_int = 0;
            di_int = Convert.ToInt32(Math.Truncate(di));

            //return InRangeFloat(di - di_int, -JudgeRange, JudgeRange);
            return modLibrary.InRange(di - di_int, -JudgeRange, JudgeRange);

        }

        //********************************************************************************
        //機    能  ：  判定ラベル設定
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V18.00  11/02/08  山影           新規作成

        //********************************************************************************
        private void SetJudgeResult(ref Label lbl, bool judge)
        {

            //OK→青、NG→赤
            lbl.Text = (judge ? "OK" : "NG");
            //lbl.ForeColor = IIf(judge, vbBlack, vbRed)
            lbl.ForeColor =(judge ? Color.Blue : Color.Red);

        }


        //********************************************************************************
        //機    能  ：  シフト量計算
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V18.00  11/02/08  山影           新規作成
        //
        //********************************************************************************
        private void cmdCalculate_Click(object sender, EventArgs e)
        {

            Maint.IPDOCINFO baseInfo = default(Maint.IPDOCINFO);
            Maint.IPDOCINFO shiftInfo = default(Maint.IPDOCINFO);
            bool IsOK = false;

            ushort[] baseImage = null;      //透視画像用配列(移動前)
            ushort[] shiftImage = null;     //透視画像用配列(移動後)
            //    '画像配列領域確保
            //    ReDim baseImage(baseInfo.width * baseInfo.Height)
            //    ReDim shiftImage(UBound(baseImage))

            //基準位置(シフト前)画像読み込み
            IsOK = LoadImage(txtFilepath[(int)enDetShift.Base].Text, ref baseInfo, ref baseImage);
            if (!IsOK)
            {
                return;
            }

            //シフト位置(シフト後)画像読み込み
            IsOK = LoadImage(txtFilepath[(int)enDetShift.Shift].Text, ref shiftInfo, ref shiftImage);
            if (!IsOK)
            {
                return;
            }
            
            //画像情報比較
            if ((baseInfo.Width != shiftInfo.Width) | (baseInfo.Height != shiftInfo.Height))
            {
                //エラー処理
                //Interaction.MsgBox("画像サイズが違います");
                MessageBox.Show("画像サイズが違います。");
                return;
            }

            cmdCalculate.Enabled = false;
            //v18.00追加 byやまおか 2011/03/07

            modAutoPos.SIZE imgSize = default(modAutoPos.SIZE);
            //Size imgSize = default(Size);
            imgSize.CX = baseInfo.Width;
            imgSize.CY = baseInfo.Height;

            modAutoPos.RECTROI roiRect = default(modAutoPos.RECTROI);
            //    roiRect.pt.x = 0
            //    roiRect.pt.y = 0
            //    roiRect.sz = imgSize
            roiRect.pt.X = Convert.ToInt16(cwnRoiXs.Value);
            roiRect.pt.Y = Convert.ToInt16(cwnRoiYs.Value);
            roiRect.sz.CX = Convert.ToInt32(cwnRoiXe.Value - cwnRoiXs.Value);
            roiRect.sz.CY = Convert.ToInt32(cwnRoiYe.Value - cwnRoiYs.Value);

            modDetShift.Search_Info searchH = default(modDetShift.Search_Info);
            modDetShift.Search_Info searchV = default(modDetShift.Search_Info);

            searchH.Base =  Convert.ToDouble(ntbDistance.Value);
            searchH.pitch = 1;
            searchH.Range = 20;

            searchV.Base = 0;
            searchV.pitch = 1;
            searchV.Range = 5;

            double minPosH = 0;
            double minPosV = 0;
            minPosH = Convert.ToDouble(ntbDistance.Value);
            minPosV = searchV.Base;

            int err_sts = 0;

            cmdCalculate.Text = "計算中";      //v18.00追加 byやまおか 2011/03/07

            //マッチング処理
            //マッチング計算中の処理はどうするか？
            err_sts = modDetShift.CalculateDetectorShiftAmount(ref baseImage[0], 
                                                               ref shiftImage[0], 
                                                               ref imgSize, 
                                                               ref roiRect, 
                                                               ref searchH, 
                                                               ref searchV, 
                                                               ref minPosH, 
                                                               ref minPosV);

            ntbVertical.Value = Convert.ToDecimal(minPosV);
            ntbHolizontal.Value = Convert.ToDecimal(minPosH);
            if ((minPosH == 0))
            {
                ntbGradient.Value = 0;
            }
            else
            {
                ntbGradient.Value = Convert.ToDecimal(minPosV / minPosH);
            }

            bool IsOkY = false;
            bool IsOkX = false;

            //X方向ずれ判定
            IsOkX = JudgeDeltaI(minPosH);
            SetJudgeResult(ref lblJudgeHolizontal, IsOkX);

            //Y方向ずれ判定
            IsOkY = JudgeDeltaJ(minPosV);
            SetJudgeResult(ref lblJudgeVertical, IsOkY);

            //傾き判定
            SetJudgeResult(ref lblJudgeGradient, IsOkY);

            //予測シフト量計算
            ntbCalculatedShiftLength.Visible = !IsOkX;
            //ntbCalculatedShiftLength.Value = minPosH * ntbFpdPitch.Value
            //v19.50 修正（式の考え方が違う) by長野 2014/02/08
            double realDetPitch = 0;
            short iminPosH = 0;
            //2枚の画像から検出器ピッチ（実測値）を出す。
            realDetPitch = CTSettings.scancondpar.Data.det_sft_length / minPosH;
            //minPosHを小数点第1位で四捨五入して整数にする
            //2014/11/07hata キャストの修正            
            //iminPosH = Convert.ToInt16(minPosH + 0.5);
            iminPosH = Convert.ToInt16(Math.Floor(minPosH + 0.5));

            ntbCalculatedShiftLength.Value = Convert.ToDecimal(realDetPitch * iminPosH);

            cmdCalculate.Text = "計算";            //v18.00追加 byやまおか 2011/03/07
            cmdCalculate.Enabled = true;            //v18.00追加 byやまおか 2011/03/07

            //    Erase baseImage
            //    Erase shiftImage
            
        }

        //********************************************************************************
        //機    能  ：  画像選択ボタンクリック
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V18.00  11/02/08  山影           新規作成
        //
        //********************************************************************************
        private void cmdRef_Click(object sender, EventArgs e)
        {
            if (sender as Button == null) return;
            int Index = Array.IndexOf(cmdRef, sender);
            if (Index < 0) return;

            string FileName = null;

            //画像ファイル選択ダイアログ表示
            FileName = modFileIO.GetFileName(StringTable.IDS_Open, CTResources.LoadResString(StringTable.IDS_CTImage), ".tif");
 
            if (!string.IsNullOrEmpty(FileName))
            {
                txtFilepath[Index].Text = FileName;
            }

        }

        //********************************************************************************
        //機    能  ：  コモンからパラメータ読み込み->設定
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V18.00  11/02/08  山影           新規作成
        //
        //********************************************************************************
        private void SetParameter(double detShiftLength)
        {

            //検出器シフト量
            ntbDetShiftLength.Value = Convert.ToDecimal(detShiftLength);

            //FPDピッチ
            ntbFpdPitch.Value = Convert.ToDecimal(CTSettings.scancondpar.Data.fpd_pitch);

            //移動量算出
            double movePixel = 0;
            movePixel = detShiftLength / CTSettings.scancondpar.Data.fpd_pitch;
            ntbDistance.Value = Convert.ToDecimal(movePixel);

        }

        //********************************************************************************
        //以下、デバッグ用
        //********************************************************************************
        //シフト量入力
        private void cwnDebugShiftLength_ValueChanged(object sender, EventArgs e)
        {
            double DebugShiftLength = 0;
            DebugShiftLength = Convert.ToDouble(cwnDebugShiftLength.Value);

            SetParameter((DebugShiftLength));

        }

        //画像重ね合わせ
        private void cmdOverlay_Click(object sender, EventArgs e)
	    {
            int ret= 0;
            string Filepath0 = null;
            string Filepath1 = null;
		    string FileName = null;
            int iPixel = 0;
		
            Filepath0 = txtFilepath[0].Text;
            Filepath1 = txtFilepath[1].Text;
            FileName = "";
		    iPixel = Convert.ToInt32(cwnMoveDx.Value);
 
		    //画像ファイルが選ばれていなければ終了   'v18.00追加 byやまおか 2011/03/07
		    if (string.IsNullOrEmpty(Filepath0) | string.IsNullOrEmpty(Filepath1))
			    return;

            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            /*
            //画像ウィンドウをすべて閉じる
		    ret = IpAppCloseAll();

		    Base_ID = IpWsLoad(txtFilepath[(int)enDetShift.Base].Text, "TIF");
            Shift_ID = IpWsLoad(txtFilepath[(int)enDetShift.Shift].Text, "TIF");

		    //ドキュメント情報を取得
            Maint.IPDOCINFO baseInfo = default(Maint.IPDOCINFO);
            Maint.IPDOCINFO shiftInfo = default(Maint.IPDOCINFO);
            ret = IpDocGet(GETDOCINFO, Base_ID, baseInfo);
		    ret = IpDocGet(GETDOCINFO, Base_ID, shiftInfo);

		    if ((baseInfo.Width != shiftInfo.Width | baseInfo.Height != shiftInfo.Height)) {
			    Interaction.MsgBox("画像サイズが異なる");
			    return;
		    }

		    short d = 0;
		    d = (baseInfo.Width % 100) / 2;

		    ipRect.Left_Renamed = d;
		    ipRect.Top = d;
		    ipRect.Right_Renamed = baseInfo.Width - 1 - d;
		    ipRect.Bottom = baseInfo.Height - 1 - d;

		    //額縁を取り払う
		    ret = IpAppSelectDoc(Base_ID);
		    ret = IpAoiCreateBox(ipRect);
		    ret = IpWsCopy();
		    BaseCut_ID = IpWsCreateFromClipboard();

		    ret = IpAppSelectDoc(Shift_ID);
		    ret = IpAoiCreateBox(ipRect);
		    ret = IpWsCopy();
		    ShiftCut_ID = IpWsCreateFromClipboard();

		    IPDOCINFO newImageInfo = default(IPDOCINFO);
		    ret = IpDocGet(GETDOCINFO, BaseCut_ID, newImageInfo);

		    //重ね合わせ
		    Overlay_ID = IpWsCreate(newImageInfo.Width + pixel, newImageInfo.Height, 300, IMC_GRAY16);
		    ret = IpWsOverlayEx(BaseCut_ID, pixel, 0, 100, 0);
		    ret = IpWsOverlayEx(ShiftCut_ID, 0, 0, 100, 0);
            */
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            ret = CallImageProFunction.CallOverlayImage(Filepath0 ,Filepath1, "", iPixel);
            if (ret == -4 )
            {
   			    //Interaction.MsgBox("画像サイズが異なる");
                MessageBox.Show("画像サイズが異なる");
			    return;
            }
            else if (ret < 0 )
            {
                return;
            }
            #endregion
        
            //コモンダイアログの表示
		    FileName = modFileIO.GetFileName(StringTable.IDS_Save, CTResources.LoadResString(StringTable.IDS_TransImage), ".tif","" ,"" ,CTResources.LoadResString(StringTable.IDS_DetShift));
		    //v18.00変更 byやまおか 2011/03/07

		    //保存(->ImageProでしか読み込めない) 'v18.00条件追加 byやまおか 2011/03/07
		    if ((!string.IsNullOrEmpty(FileName)))
            {
			    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                /*
                ret = IpWsSaveAs(FileName, "TIF");
                */
                //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                ret = CallImageProFunction.CallIpWsSaveAs(FileName, "TIF");
                #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//
            }

	    }

        //*******************************************************************************
        //機　　能： ROI 開始X座標 変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V18.00  2011/03/07  やまおか    新規作成
        //*******************************************************************************
        private void cwnRoiXs_ValueChanged(object sender, EventArgs e)
        {

            //終了座標より大きい場合の処理
            if ((cwnRoiXs.Value > cwnRoiXe.Value))
            {   
                //変更2015/02/02hata_Max/Min範囲のチェック
                //cwnRoiXs.Value = cwnRoiXe.Value - 1;
                cwnRoiXs.Value = modLibrary.CorrectInRange(cwnRoiXe.Value - 1, cwnRoiXs.Minimum, cwnRoiXs.Maximum);
            }
   
            //Publicに保存
            modDetShift.Roi_Xs = Convert.ToInt32(cwnRoiXs.Value);

        }

        //*******************************************************************************
        //機　　能： ROI 終了X座標 変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V18.00  2011/03/07  やまおか    新規作成
        //*******************************************************************************
        private void cwnRoiXe_ValueChanged(object sender, EventArgs e)
        {

            //終了座標より小さい場合の処理
            if ((cwnRoiXe.Value <= cwnRoiXs.Value))
            {
                //変更2015/02/02hata_Max/Min範囲のチェック
                //cwnRoiXe.Value = cwnRoiXs.Value + 1;
                cwnRoiXe.Value = modLibrary.CorrectInRange(cwnRoiXs.Value + 1, cwnRoiXe.Minimum, cwnRoiXe.Maximum);
             
            }
            //Publicに保存
            modDetShift.Roi_Xe = Convert.ToInt32(cwnRoiXe.Value);

        }

        //*******************************************************************************
        //機　　能： ROI 開始Y座標 変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V18.00  2011/03/07  やまおか    新規作成
        //*******************************************************************************
        private void cwnRoiYs_ValueChanged(object sender, EventArgs e)
        {

            //終了座標より大きい場合の処理
            if ((cwnRoiYs.Value > cwnRoiYe.Value))
            {
                //変更2015/02/02hata_Max/Min範囲のチェック
                //cwnRoiYs.Value = cwnRoiYe.Value - 1;
                cwnRoiYs.Value = modLibrary.CorrectInRange(cwnRoiYe.Value - 1, cwnRoiYs.Minimum, cwnRoiYs.Maximum);
            }
            //Publicに保存
            modDetShift.Roi_Ys = Convert.ToInt32(cwnRoiYs.Value);

        }

        //*******************************************************************************
        //機　　能： ROI 終了Y座標 変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V18.00  2011/03/07  やまおか    新規作成
        //*******************************************************************************
        private void cwnRoiYe_ValueChanged(object sender, EventArgs e)
        {

            //終了座標より小さい場合の処理
            if ((cwnRoiYe.Value <= cwnRoiYs.Value))
            {
                //変更2015/02/02hata_Max/Min範囲のチェック
                //cwnRoiYe.Value = cwnRoiYs.Value + 1;
                cwnRoiYe.Value = modLibrary.CorrectInRange(cwnRoiYs.Value + 1, cwnRoiYe.Minimum, cwnRoiYe.Maximum);
            }
            //Publicに保存
            modDetShift.Roi_Ye = Convert.ToInt32(cwnRoiYe.Value);

        }

        //'*******************************************************************************
        //'機　　能： 左右反転ありなしボタン 変更時処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V18.00  2011/09/14  やまおか    新規作成
        //'*******************************************************************************
        //Private Sub cwbtnTransDispLRInv_ValueChanged(ByVal Value As Boolean)
        //
        //    'コモンに書き込む
        //    putcommon_long "scaninh", "transdisp_lr_inv", IIf(Value, 1, 0)
        //
        //End Sub



        //◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆
        //◆◆◆◆◆◆◆◆◆◆ Ｘ線装置タブ ◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆
        //◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆


        //********************************************************************************
        //機    能  ：  X線外部制御ボタン クリック時処理
        //              変数名           [I/O] 型        内容
        //引    数  ：                   [I/ ]
        //戻 り 値  ：                   [ /O]
        //補    足  ：
        //
        //履    歴  ：  V18.00   2011/03/21  やまおか    新規作成
        //********************************************************************************
	    private void cmdRemoteControl_Click(object sender, EventArgs e)
	    {


		    if ((stsRemoteControl.Status == StringTable.GC_STS_REMOTE)) 
            {
#if (!DebugOn)
			    modTitan.Ti_SetTitanKBLock(1);
#endif
			    stsRemoteControl.Status = StringTable.GC_STS_MANUAL;

		    } 
            else
            {

#if (!DebugOn)	
                modTitan.Ti_SetTitanKBLock(0);

#endif
                stsRemoteControl.Status = StringTable.GC_STS_REMOTE;

		    }

	    }


        //********************************************************************************
        //機    能  ：  最大管電圧表示ボタン クリック時処理
        //              変数名           [I/O] 型        内容
        //引    数  ：                   [I/ ]
        //戻 り 値  ：                   [ /O]
        //補    足  ：
        //
        //履    歴  ：  V18.00   2011/03/26  やまおか    新規作成
        //********************************************************************************
        private void cmdMaxVoltDisp_Click(object sender, EventArgs e)
        {
            int flg = 0;
            int real_max = 0;
            int real_min = 0;

            //選ばれているオプションボタンを取得
            flg = modLibrary.GetOption(optMaxVoltDisp);

            //「表示する」の場合
            //if ((flg == 0) & (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan))
            //Rev25.03/Rev25.02 by chouno 2017/02/05
            if ((flg == 0) & ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) || (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman)))
            {
                modTitan.Ti_GetMaxMinVoltage(ref real_max, ref real_min);     
                
            //「表示しない」の場合
            }
            else
            {
                real_min =Convert.ToInt32(modXrayControl.XrayMinVolt());
                real_max = Convert.ToInt32(modXrayControl.XrayMaxVolt());

                //制御器に制限された最大値を設定する
                modTitan.Ti_SetXrayVoltage_UnChk(real_max);
            }

            var _with3 = frmXrayControl.Instance;

            //最小値・最大値を設定
            //_with3.cwneKV.SetMinMax(real_min, real_max);
            if (_with3.cwneKV.Value < real_min) _with3.cwneKV.Value = real_min;
            if (_with3.cwneKV.Value > real_max) _with3.cwneKV.Value = real_max;
            _with3.cwneKV.Minimum = real_min;
            _with3.cwneKV.Maximum = real_max;

            //Rev25.00/Rev24.00 スライダーのレンジとcwneKVのレンジが一致しない不具合の修正 by長野 2016/07/11
            int tmpLargeChange = _with3.cwsldKV.LargeChange;
            int tmpSmallChange = _with3.cwsldKV.SmallChange;

            //スライダコントロールに反映
            //_with3.cwsldKV.SetMinMax(_with3.cwneKV.Minimum, _with3.cwneKV.Maximum);
            if (_with3.cwsldKV.Value < real_min) _with3.cwsldKV.Value = real_min;
            if (_with3.cwsldKV.Value > real_max) _with3.cwsldKV.Value = real_max;
            //_with3.cwsldKV.Minimum = real_min;
            //_with3.cwsldKV.Maximum = real_max;
            //Rev25.00/Rev24.00 スライダーのレンジとcwneKVのレンジが一致しない不具合の修正 by長野 2016/07/11
            _with3.cwsldKV.Minimum = real_min;
            //_with3.cwsldKV.Maximum = real_max;
            _with3.cwsldKV.LargeChange = tmpLargeChange;
            _with3.cwsldKV.SmallChange = tmpSmallChange;
            _with3.cwsldKV.Maximum = Convert.ToInt32(_with3.cwneKV.Maximum) + tmpLargeChange - 1;

            //リフレッシュ
            _with3.Refresh();
            _with3.cwsldKV.Refresh();

        }
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここまで

        //********************************************************************************
        //機    能  ：  ウォームアップ設定ボタン クリック時処理
        //              変数名           [I/O] 型        内容
        //引    数  ：                   [I/ ]
        //戻 り 値  ：                   [ /O]
        //補    足  ：
        //
        //履    歴  ：  V18.00   2011/03/26  やまおか    新規作成
        //********************************************************************************
        private void cmdWarmupMode_Click(object sender, EventArgs e)
        {

            //選ばれているオプションボタンを取得
            if ((CTSettings.scaninh.Data.xray_remote == 0))
            {

                //'最大管電圧を取得する
                //Dim maxvolt As Long
                //Dim minvolt As Long
                //Ti_GetMaxMinVoltage maxvolt, minvolt

                //モードを切り替える
                //Ti_UpdateWarmupMode GetOption(optWarmupMode), maxvolt
                //画面に表示されている最大管電圧までのウォームアップを変更する   'v18.00変更 byやまおか 2011/09/05

                //modTitan.WarmupConstants
                //modL GetOption(optWarmupMode);

                //modTitan.Ti_UpdateWarmupMode((modTitan.WarmupConstants)modLibrary.GetOption(optWarmupMode), Convert.ToInt32(frmXrayControl.Instance.cwneKV.Maximum));
                //Rev23.20 条件追加 by長野 2016/01/25
                 modTitan.Ti_UpdateWarmupMode((modTitan.WarmupConstants)modLibrary.GetOption(optWarmupMode), Convert.ToInt32(frmXrayControl.Instance.cwneKV.Maximum));
            }
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
		//履　　歴： V17.60  11/05/25  (検S1)長野     新規作成
		//*******************************************************************************
		private void EnglishAdjustLayout()
		{
			int margin = 10;

			frmMaint.Instance.Width = Convert.ToInt32(frmMaint.Instance.Width * 1.25);
            sstMenu.Width = Convert.ToInt32(sstMenu.Width * 1.3);

            lbl3.Height = Convert.ToInt32(lbl3.Height * 1.3);
			txtCenterCh.Top = lbl3.Top + lbl3.Height + margin;
			lblch.Top = txtCenterCh.Top + margin;

            lbl4.Height = Convert.ToInt32(lbl4.Height * 1.3);
			txtDetectorPitch.Top = lbl4.Top + lbl4.Height + margin;
			lblmm.Top = txtDetectorPitch.Top + margin;
        }

        //追加2014/10/07hata_v19.51反映
        //*************************************************************************************************
        //機　　能： X線、検出器昇降処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： TETS  14/02/22   inaba      新規作成
        //*************************************************************************************************
        private void cmdXrayUdOrigin_Click(object sender, EventArgs e)
        {
            int error_sts = 0;

            //X線昇降軸原点復帰
            error_sts = modMechaControl.XrayUdOrigin(myCallback);

        }

        private void cmdUdXrayUp_MouseDown(object sender, MouseEventArgs e)
        {
            int error_sts = 0;

            //X線昇降軸上昇
            error_sts = modMechaControl.XrayUdManual(1, Convert.ToSingle(txtUdXraySpeed.Text));

        }

        private void cmdUdXrayUp_MouseUp(object sender, MouseEventArgs e)
        {
            int error_sts = 0;

            //X線昇降軸停止
            error_sts = modMechaControl.MechaUdStop();

        }

        private void cmdUdXrayDown_MouseDown(object sender, MouseEventArgs e)
        {
            int error_sts = 0;

            //X線昇降軸下降
            error_sts = modMechaControl.XrayUdManual(0, Convert.ToSingle(txtUdXraySpeed.Text));

        }

        private void cmdUdXrayDown_MouseUp(object sender, MouseEventArgs e)
        {
            int error_sts = 0;

            //X線昇降軸停止
            error_sts = modMechaControl.MechaUdStop();

        }

        private void cmdDetUdOrigin_Click(object sender, EventArgs e)
        {
            int error_sts = 0;

            //検出器昇降軸原点復帰
            error_sts = modMechaControl.DetUdOrigin(myCallback);

        }

        private void cmdUdDetUp_MouseDown(object sender, MouseEventArgs e)
        {
            int error_sts = 0;

            //検出器昇降軸上昇
            error_sts = modMechaControl.DetUdManual(1, Convert.ToSingle(txtUdDetSpeed.Text));

        }

        private void cmdUdDetUp_MouseUp(object sender, MouseEventArgs e)
        {
            int error_sts = 0;

            //検出器昇降軸停止
            error_sts = modMechaControl.MechaUdStop();

        }

        private void cmdUdDetDown_MouseDown(object sender, MouseEventArgs e)
        {
            int error_sts = 0;

            //検出器昇降軸下降
            error_sts = modMechaControl.DetUdManual(0, Convert.ToSingle(txtUdDetSpeed.Text));

        }

        private void cmdUdDetDown_MouseUp(object sender, MouseEventArgs e)
        {
            int error_sts = 0;

            //検出器昇降軸停止
            error_sts = modMechaControl.MechaUdStop();

        }

        private void tmrUdStatus_Tick(object sender, EventArgs e)
        {

            //mecainfType theMecainf = default(mecainfType);
            MecaInf theMecainf = new MecaInf();
            theMecainf.Data.Initialize();

            //mecainf（コモン）取得
            //GetMecainf(theMecainf);
            theMecainf.Load();

            var _with4 = theMecainf.Data;
            txtUdXrayPos.Text = Convert.ToString(_with4.udab_xray_pos);
            txtUdDetPos.Text = Convert.ToString(_with4.udab_det_pos);

        }

	}
}
