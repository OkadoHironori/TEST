using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;
//
using CTAPI;
using CT30K.Common;

namespace CT30K
{
	///* ************************************************************************** */
	///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： ImagePrint.bas                                              */
	///* 処理概要　　： 画像印刷モジュール                                          */
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
	///*                                                                            */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
	///* ************************************************************************** */
	internal static class ImagePrint
	{
		private static Image _PrintImage = null;

		//*******************************************************************************
		//機　　能： 印刷処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		//public static void DoPrint()
		public static void DoPrint(int AfterScan = 0) //Rev20.00 引数追加 by長野 2015/02/06
        {
			//実行時はフラグをセット
			modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTImagePrint;

			//画面全体を印刷
			//PrintPictureToFitPage CaptureScreen()

			//v15.0以下に変更 マルチモニタの場合，印刷画面を指定できるようにした by 間々田 2009/08/20

			//モニタの数が複数ではない，もしくは透視画像フォームが存在しない
            //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            //if (Winapi.GetSystemMetrics(Winapi.SM_CMONITORS) < 2 || !modLibrary.IsExistForm(frmTransImage.Instance))
            //if (Winapi.GetSystemMetrics(Winapi.SM_CMONITORS) < 2 || !modLibrary.IsExistForm("frmTransImage"))
            if (Winapi.GetSystemMetrics(Winapi.SM_CMONITORS) < 2 || !modLibrary.IsExistForm("frmTransImage"))
            {
				//従来どおりの印刷
				PrintPictureToFitPage(modImgProc.CaptureScreen());

            }

			//モニタの数が複数ではない
			//else if (frmImagePrint.Instance.Dialog())
			//Rev20.00 スキャン後の場合はダイアログを出さずに即印刷 by長野 2015/02/06
            else if (AfterScan == 0)
            {
                //Rev26.14 修正 by chouno
                //frmImagePrint.Instance.Dialog()
                if (frmImagePrint.Instance.Dialog())
                {

                    //ダイアログを確実に消去するため，DoEventsをコール
                    Application.DoEvents();

                    //ダイアログにて「メインモニター画面」がチェックされている場合
                    if (modImgProc.IsPrint1stMonitor)
                    {
                        //従来どおりの印刷
                        PrintPictureToFitPage(modImgProc.CaptureScreen());
                    }

                    //ダイアログにて「透視画像モニター画面」がチェックされている場合
                    if (modImgProc.IsPrint2ndMonitor)
                    {
                        //透視画像モニター画面左上のＸ座標
                        int theLeft = Winapi.GetSystemMetrics(Winapi.SM_XVIRTUALSCREEN);
                        //デスクトップ左上のＸ座標
                        if (theLeft >= 0)
                        {
                            theLeft = Winapi.GetSystemMetrics(Winapi.SM_CXSCREEN);
                        }

                        //透視画像モニター画面の幅
                        int theWidth = Winapi.GetSystemMetrics(Winapi.SM_CXVIRTUALSCREEN) - Winapi.GetSystemMetrics(Winapi.SM_CXSCREEN);

                        //透視画像モニター画面の高さ
                        int theHeight = Winapi.GetSystemMetrics(Winapi.SM_CYVIRTUALSCREEN);

                        //指定された領域の印刷
                        PrintPictureToFitPage(modImgProc.CaptureScreen(theLeft, 0, theWidth, theHeight));
                    }
                }
			}
            else if(AfterScan == 1)
            {
                //メイン画面の印刷
                PrintPictureToFitPage(modImgProc.CaptureScreen());

                //透視画像の印刷
                //透視画像モニター画面左上のＸ座標
                int theLeft = Winapi.GetSystemMetrics(Winapi.SM_XVIRTUALSCREEN);
                //デスクトップ左上のＸ座標
                if (theLeft >= 0)
                {
                    theLeft = Winapi.GetSystemMetrics(Winapi.SM_CXSCREEN);
                }

                //透視画像モニター画面の幅
                int theWidth = Winapi.GetSystemMetrics(Winapi.SM_CXVIRTUALSCREEN) - Winapi.GetSystemMetrics(Winapi.SM_CXSCREEN);

                //透視画像モニター画面の高さ
                int theHeight = Winapi.GetSystemMetrics(Winapi.SM_CYVIRTUALSCREEN);

                //指定された領域の印刷
                PrintPictureToFitPage(modImgProc.CaptureScreen(theLeft, 0, theWidth, theHeight));
            }

			//終了時はフラグをリセット
			modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTImagePrint);
		}

		//*******************************************************************************
		//機　　能： Pictureオブジェクトを用紙サイズに合わせて可能な限り大きく印刷します
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Pic             [I/ ] Picture   Pictureオブジェクト
		//戻 り 値： なし
		//
		//補　　足：
		//
		//履　　歴： v15.0  2009/04/14 (SS1)間々田   新規作成
		//*******************************************************************************
		private static void PrintPictureToFitPage(Image Pic)
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Dim PicRatio      As Double
			Dim PrnWidth      As Double
			Dim PrnHeight     As Double
			Dim PrnRatio      As Double
			Dim PrnPicWidth   As Double
			Dim PrnPicHeight  As Double
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			try
			{
				PrintDocument printer = new PrintDocument();

				//「印刷中...」ダイアログ表示
				//frmMessage.lblMessage.Caption = GetResString(IDS_lblNowPrinting, .DeviceName)  '印刷中...(出力先：%1)
				//frmMessage.Refresh
				modCT30K.ShowMessage(StringTable.GetResString(StringTable.IDS_lblNowPrinting, printer.PrinterSettings.PrinterName));	//変更 by 間々田 2009/08/24

				//用紙の向き
				if (Pic.Height >= Pic.Width)
				{
					printer.DefaultPageSettings.Landscape = false;	//縦
				}
				else
				{
					printer.DefaultPageSettings.Landscape = true;	//横
				}

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				.ForeColor = vbWhite
        
				'印字原点のリセット
				.ScaleLeft = 0
				.ScaleTop = 0
        
				'縦横比
				PicRatio = Pic.Width / Pic.Height
        
				' Calculate the dimentions of the printable area in HiMetric
				PrnWidth = .ScaleX(.ScaleWidth, .ScaleMode, vbHimetric)
				PrnHeight = .ScaleY(.ScaleHeight, .ScaleMode, vbHimetric)
        
				' Calculate device independent Width to Height ratio for printer
				PrnRatio = PrnWidth / PrnHeight
        
				' Scale the output to the printable area
				If PicRatio >= PrnRatio Then
					' Scale picture to fit full width of printable area
					PrnPicWidth = .ScaleX(PrnWidth, vbHimetric, .ScaleMode)
					PrnPicHeight = .ScaleY(PrnWidth / PicRatio, vbHimetric, .ScaleMode)
				Else
					' Scale picture to fit full height of printable area
					PrnPicHeight = .ScaleY(PrnHeight, vbHimetric, .ScaleMode)
					PrnPicWidth = .ScaleX(PrnHeight * PicRatio, vbHimetric, .ScaleMode)
				End If
        
				'中央に印刷する
				.PaintPicture Pic, .ScaleWidth / 2 - PrnPicWidth / 2, .ScaleHeight / 2 - PrnPicHeight / 2, PrnPicWidth, PrnPicHeight, , , , , vbSrcCopy
                
				.EndDoc
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                //Rev25.03/Rev25.02 add by chouno 2017/02/05
                printer.DefaultPageSettings.Margins.Top = CT30K.Properties.Settings.Default.PrintMarginTop;
                printer.DefaultPageSettings.Margins.Bottom = CT30K.Properties.Settings.Default.PrintMarginBottom;
                printer.DefaultPageSettings.Margins.Right = CT30K.Properties.Settings.Default.PrintMarginRight;
                printer.DefaultPageSettings.Margins.Left = CT30K.Properties.Settings.Default.PrintMarginLeft;

				printer.PrintPage += new PrintPageEventHandler(printer_PrintPage);

				_PrintImage = Pic;

				//印刷する
				printer.Print();
			}
			catch (Exception ex)
			{
				_PrintImage = null;

				//エラーが発生している場合、エラーメッセージを表示
				MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

//ExitHandler:

			//「印刷中...」ダイアログアンロード
			//Unload frmMessage
			modCT30K.HideMessage();			//変更 by 間々田 2009/08/24
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		static void printer_PrintPage(object sender, PrintPageEventArgs e)
		{
			try
			{
				//縦横比
                //2014/11/13hata キャストの修正
                //double PicRatio = _PrintImage.Width / _PrintImage.Height;
                double PicRatio =Convert.ToDouble(_PrintImage.Width / (double)_PrintImage.Height);

				// Calculate the dimentions of the printable area in HiMetric
				int PrnWidth =  e.MarginBounds.Width;
				int PrnHeight = e.MarginBounds.Height;

				// Calculate device independent Width to Height ratio for printer
                //2014/11/13hata キャストの修正
                //double PrnRatio = PrnWidth / PrnHeight;
                double PrnRatio =Convert.ToDouble( PrnWidth / (double)PrnHeight);

				int PrnPicWidth = 0;
				int PrnPicHeight = 0;

				// Scale the output to the printable area
				if (PicRatio >= PrnRatio)
				{
					// Scale picture to fit full width of printable area
					PrnPicWidth = PrnWidth;
                    //2014/11/13hata キャストの修正
                    //PrnPicHeight = (int)(PrnWidth / PicRatio);
                    PrnPicHeight = Convert.ToInt32(PrnWidth / PicRatio);
                }
				else
				{
                    // Scale picture to fit full height of printable area
					PrnPicHeight = PrnHeight;
                    //2014/11/13hata キャストの修正
					//PrnPicWidth = (int)(PrnHeight * PicRatio);
                    PrnPicWidth = Convert.ToInt32(PrnHeight * PicRatio);
				}

				//中央に印刷する
                //2014/11/13hata キャストの修正
                //e.Graphics.DrawImage(_PrintImage,
                //                     e.MarginBounds.Width / 2 - PrnPicWidth / 2,
                //                     e.MarginBounds.Height / 2 - PrnPicHeight / 2,
                //                     PrnPicWidth,
                //                     PrnPicHeight);
                e.Graphics.DrawImage(_PrintImage,
                                     Convert.ToInt32(e.MarginBounds.Width / 2F - PrnPicWidth / 2F),
                                     Convert.ToInt32(e.MarginBounds.Height / 2F - PrnPicHeight / 2F),
                                     PrnPicWidth,
                                     PrnPicHeight);

            }
			catch
			{
			}
			finally
			{
				e.HasMorePages = false;

				_PrintImage = null;
			}
		}
	}
}
