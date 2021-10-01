using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.IO;

using CTAPI;
using CT30K.Common;

namespace CT30K
{
    ///* *************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver9.7                */
    ///* 客先　　　　： ?????? 殿                                                    */
    ///* プログラム名： DrawRoi.bas                                                  */
    ///* 処理概要　　： ROIテーブルの読み込み・書き込み等                            */
    ///* 注意事項　　：                                                              */
    ///* --------------------------------------------------------------------------- */
    ///* ＯＳ　　　　： Windows XP Professional (SP1)                                */
    ///* コンパイラ　： VB 6.0 (SP5)                                                 */
    ///* --------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                  */
    ///*                                                                             */
    ///* V1.00       99/XX/XX    (TOSFEC) ????????   新規作成                        */
    ///*                                                                             */
    ///* --------------------------------------------------------------------------- */
    ///* ご注意：                                                                    */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。    */
    ///*                                                                             */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2004                  */
    ///* *************************************************************************** */
    public static class DrawRoi
    {
        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************

        //ROI データオブジェクト
        public static RoiData roi;

        //v19.11 追加 by長野　ソフト起動中のみROI処理、ヒストグラム、骨塩定量解析のROI形状を記憶させる
        public static int RoiCalRoiNo;
        public static int HistRoiNo;
        public static int BoneDensityRoiNo;

        /// <summary>
        /// リソース参照用の固定文字列
        /// </summary>
        private const string STR = "String";

        /// <summary>
        /// 改行コード
        /// </summary>
        private const string CRLF = "\r\n";
        private const string CR = "\r";

        //*************************************************************************************************
        //機　　能： ROI情報をコモンにセットします
        //
        //           変数名          [I/O] 型        内容
        //引　　数： RoiNo           [I/ ] Integer   ROI番号
        //           RoiInfoStr      [ /O] Variant   ROI情報
        //戻 り 値：                 [ /O] Boolean   結果（True:成功、False:失敗）
        //
        //補　　足： なし
        //
        //履　　歴： V7.00  XX/XX/XX  (SI4)間々田    2048画素対応
        //*************************************************************************************************
        public static bool SetROIToCommon(int RoiNo, ref string RoiInfoStr)
		{
			bool functionReturnValue = false;

			int x1 = 0;
			int y1 = 0;
			int x2 = 0;
			int y2 = 0;
			int xc = 0;
			int yc = 0;
			int xl = 0;
			int yl = 0;
			int r = 0;
			int i = 0;
			int N = 0;

			//戻り値初期化
			functionReturnValue = true;

            //追加2014/07/30(検S1)hata
            //初期化
            modRoikey.roikey.Initialize();

			int m = 0;
			int PicWidth = 0;
            PicWidth = frmScanImage.Instance.PicWidth;

			//v16.10 4096対応 mの決め方を変更する  by 長野 2010/02/09
			//m = IIf(frmScanImage.hsbImage.Visible, 2, 1)
            if (frmScanImage.Instance.hsbImage.Visible == true)
            {
				switch (PicWidth)
                {
					case 2048:
						m = 2;
						break;

					case 4096:
						m = 4;
						break;
				}
			}
            else
            {
				m = 1;
			}

			//ROI種別を取得
			switch (roi.GetRoiShape(RoiNo))
            {
				//矩形ROI処理
				case RoiData.RoiShape.ROI_RECT:

					//ROI座標取得
					roi.GetRectangleShape(RoiNo, ref x1, ref y1, ref x2, ref y2);

                    x1 = x1 * frmScanImage.Instance.Magnify;
                    y1 = y1 * frmScanImage.Instance.Magnify;
                    x2 = x2 * frmScanImage.Instance.Magnify;
                    y2 = y2 * frmScanImage.Instance.Magnify;

					//コモン登録用ROI情報作成
					//xc = x1 + (x2 - x1) \ 2
					//yc = y1 + (y2 - y1) \ 2
					//xl = ((x2 - x1) \ 2) * 2
					//yl = ((y2 - y1) \ 2) * 2

					//ROIモードと座標をセット
					//putcommon_long "roikey", "imgroi", 1
					//putcommon_long "roikey", "roi_x", CLng(xc)
					//putcommon_long "roikey", "roi_y", CLng(yc)
					//putcommon_long "roikey", "roi_xsize", CLng(xl)
					//putcommon_long "roikey", "roi_ysize", CLng(yl)

					//v11.5以下に変更 by 間々田 2006/04/24
                    modRoikey.roikey.imgroi = 1;
                    modRoikey.roikey.roi_x = x1 + (x2 - x1) / 2;
                    modRoikey.roikey.roi_y = y1 + (y2 - y1) / 2;
                    modRoikey.roikey.roi_xsize = ((x2 - x1) / 2) * 2;
                    modRoikey.roikey.roi_ysize = ((y2 - y1) / 2) * 2;
                    modRoikey.PutRoikey(ref modRoikey.roikey);

					//ROITable保存用
					if ((RoiInfoStr != null))
                    {
						//ROI座標取得
						roi.GetRectangleShape(RoiNo, ref x1, ref y1, ref x2, ref y2);

						//コモン登録用ROI情報作成
						xc = x1 + (x2 - x1) / 2;
						yc = y1 + (y2 - y1) / 2;
						xl = ((x2 - x1) / 2);
						yl = ((y2 - y1) / 2);

                        //2014/11/13hata キャストの修正
                        //RoiInfoStr = CTResources.LoadResString(12003) + "," +
                        //            String.Format("{0:f1}", xc / m) + "(XC)," +
                        //            String.Format("{0:f1}", yc / m) + "(YC)," +
                        //            String.Format("{0:f1}", xl / m) + "(XL)," +
                        //            String.Format("{0:f1}", yl / m) + "(YL)";
                        RoiInfoStr = CTResources.LoadResString(12003) + "," +
                                    String.Format("{0:f1}", xc / (float)m) + "(XC)," +
                                    String.Format("{0:f1}", yc / (float)m) + "(YC)," +
                                    String.Format("{0:f1}", xl / (float)m) + "(XL)," +
                                    String.Format("{0:f1}", yl / (float)m) + "(YL)";
                    
                    }
					break;

				//円形ROI処理
				case RoiData.RoiShape.ROI_CIRC:

					//ROI座標取得
					roi.GetCircleShape(RoiNo, ref xc, ref yc, ref r);

					//With frmScanImage
					//
					//    'ROIモードと座標をコモンにセット
					//    putcommon_long "roikey", "imgroi", 2
					//    putcommon_long "roikey", "roi_x", CLng(xc * .Magnify)
					//    putcommon_long "roikey", "roi_y", CLng(yc * .Magnify)
					//    putcommon_long "roikey", "roi_xsize", CLng(R * .Magnify)
					//    putcommon_long "roikey", "roi_ysize", CLng(R * .Magnify)
					//
					//End With

					//v11.5以下に変更 by 間々田 2006/04/24
                    modRoikey.roikey.imgroi = 2;
                    modRoikey.roikey.roi_x = xc * frmScanImage.Instance.Magnify;
                    modRoikey.roikey.roi_y = yc * frmScanImage.Instance.Magnify;
                    modRoikey.roikey.roi_xsize = r * frmScanImage.Instance.Magnify;
                    modRoikey.roikey.roi_ysize = r * frmScanImage.Instance.Magnify;
					modRoikey.PutRoikey(ref modRoikey.roikey);

					//ROITable保存用
					if ((RoiInfoStr != null))
                    {
                        //2014/11/13hata キャストの修正
                        //RoiInfoStr = CTResources.LoadResString(12002) + "," +
                        //            String.Format("{0:f1}", xc / m) + "(XC)," +
                        //            String.Format("{0:f1}", yc / m) + "(YC)," +
                        //            String.Format("{0:f1}", r / m) + "(RO)";
                        RoiInfoStr = CTResources.LoadResString(12002) + "," +
                                    String.Format("{0:f1}", xc / (float)m) + "(XC)," +
                                    String.Format("{0:f1}", yc / (float)m) + "(YC)," +
                                    String.Format("{0:f1}", r / (float)m) + "(RO)";
                    }
					break;

				//トレースROI処理
				case RoiData.RoiShape.ROI_TRACE:

					//点の個数を取得
					N = roi.NumOfTracePoints(RoiNo);

					//ROITable保存用
					if ((RoiInfoStr != null))
                    {
                        RoiInfoStr = CTResources.LoadResString(12000) + "," + N.ToString("000") + "(N0)";
					}

					//'ﾄﾚｰｽROI情報をｺﾓﾝ登録用一時ﾊﾞｯﾌｧに保持
					//For i = 1 To N
					//    '各座標の取得
					//    Call roi.GetTracePoint(RoiNo, i, xc, yc)
					//
					//    With frmScanImage
					//
					//        px(i - 1) = xc * .Magnify
					//        py(i - 1) = yc * .Magnify
					//
					//        'ROITable保存用
					//        If Not IsMissing(RoiInfoStr) Then
					//            RoiInfoStr = RoiInfoStr & IIf((i Mod 10) = 1, ",", ";")
					//            RoiInfoStr = RoiInfoStr & _
					//'                         Format$(xc / m, "0.0") & "(X" & CStr(i) & ");" & _
					//'                         Format$(yc / m, "0.0") & "(Y" & CStr(i) & ")"
					//        End If
					//
					//    End With
					//
					//Next
					//px(N) = -1
					//py(N) = -1
					//
					//'ﾄﾚｰｽROIの情報をｺﾓﾝ登録
					//putcommon_long "roikey", "imgroi", 6
					//Call Rw_ArrayCommon(0, "roikey", "trace_pos", px(0), py(0))

                    
					//v11.5以下に変更 by 間々田 2006/04/24
                    modRoikey.roikey.imgroi = 6;

                    //追加2015/02/05 //Rev20.00 追加 by長野
                    //CTSettings.roikey.Data.Initialize();
                    CTSettings.roikey.Data.imgroi = modRoikey.roikey.imgroi;
                    CTSettings.roikey.Data.roi_mode = modRoikey.roikey.roi_mode;
                    CTSettings.roikey.Data.roi_x = modRoikey.roikey.roi_x;
                    CTSettings.roikey.Data.roi_xsize = modRoikey.roikey.roi_xsize;
                    CTSettings.roikey.Data.roi_y = modRoikey.roikey.roi_y;
                    CTSettings.roikey.Data.roi_ysize = modRoikey.roikey.roi_ysize;

					for (i = 1; i <= N; i++)
                    {
						//各座標の取得
						roi.GetTracePoint(RoiNo, i, ref xc, ref yc);

                        modRoikey.roikey.trace_pos[0, i - 1] = xc * frmScanImage.Instance.Magnify;
                        modRoikey.roikey.trace_pos[1, i - 1] = yc * frmScanImage.Instance.Magnify;

                        //追加2015/02/05 //Rev20.00 追加 by長野
                        CTSettings.roikey.Data.trace_pos[0 + (i - 1) * 2] = xc * frmScanImage.Instance.Magnify;
                        CTSettings.roikey.Data.trace_pos[1 + (i - 1)* 2] = yc * frmScanImage.Instance.Magnify;

						//ROITable保存用
						if ((RoiInfoStr != null))
                        {
							RoiInfoStr = RoiInfoStr + ((i % 10) == 1 ? "," : ";");

                            //2014/11/13hata キャストの修正
                            //RoiInfoStr = RoiInfoStr + 
                            //         String.Format("{0:f1}", xc / m) + "(X" + Convert.ToString(i) + ");" +
                            //             String.Format("{0:f1}", yc / m) + "(Y" + Convert.ToString(i) + ")";
                            RoiInfoStr = RoiInfoStr +
                                         String.Format("{0:f1}", xc / (float)m) + "(X" + Convert.ToString(i) + ");" +
                                         String.Format("{0:f1}", yc / (float)m) + "(Y" + Convert.ToString(i) + ")";
						}
					}

                    modRoikey.roikey.trace_pos[0, N] = -1;
                    modRoikey.roikey.trace_pos[1, N] = -1;

                    //追加2015/02/05 //Rev20.00 追加 by長野
                    CTSettings.roikey.Data.trace_pos[0 + N * 2] = -1;
                    CTSettings.roikey.Data.trace_pos[1 + N * 2] = - 1;

                    //変更2015/02/05 //Rev20.00 追加 by長野
                    //modRoikey.PutRoikey(ref modRoikey.roikey);
                    CTSettings.roikey.Write();
  
                    break;

				default:
					//警告メッセージ表示：ROIが設定されていません。
                    //変更2014/11/18hata_MessageBox確認
                    //MessageBox.Show(CTResources.LoadResString(9974), "",  MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    MessageBox.Show(CTResources.LoadResString(StringTable.IDS_NotFoundROI), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

					functionReturnValue = false;
					break;

			}

			return functionReturnValue;
		}


        //*************************************************************************************************
        //機　　能： ROIテーブルの読み出し
        //
        //           変数名          [I/O] 型        内容
        //引　　数： fileName        [I/ ] String    ROIテーブルのファイル名
        //戻 り 値：                 [ /O] Boolean   結果（True:成功、False:失敗）
        //
        //補　　足： なし
        //
        //履　　歴： V7.00  XX/XX/XX  (SI4)間々田    2048画素対応
        //*************************************************************************************************
        public static bool LoadRoiTable(string FileName)
		{
			bool functionReturnValue = false;

            //int fileNo = 0;
			string[] strCell = null;
			string strBuf = null;
			string strWork = null;
			int CommaPos = 0;
            int RoiCount = 0;
            int i = 0;
            int RoiId = 0;
            int xc = 0;
            int yc = 0;
            int xl = 0;
            int yl = 0;
            int r = 0;

			//戻り値初期化
			functionReturnValue = false;

			//Roiの削除
			roi.DeleteAllRoiData();

			int m = 0;
            int PicWidth = 0;
            PicWidth = frmScanImage.Instance.PicWidth;

			//v16.10 4096対応，mの決め方を変更する  by 長野 2010/02/09
			//m = IIf(frmScanImage.hsbImage.Visible, 2, 1)
			if (frmScanImage.Instance.hsbImage.Visible == true)
            {
				switch ((PicWidth))
                {
					case 2048:
						m = 2;
						break;

					case 4096:
						m = 4;
						break;
				}
			} 
            else
            {
				m = 1;
			}

			RoiCount = 0;

            int work3; int work4;
            int work5; int work6;
            int workA; int workB;

            //ファイルオープン
            StreamReader file = null;
            
            try
            {
                //変更2015/01/22hata
                //file = new StreamReader(FileName);
                file = new StreamReader(FileName, Encoding.GetEncoding("shift-jis"));

                while ((strWork = file.ReadLine()) != null) //1行読み込み
                {
                    strBuf = strWork.Replace(";", ",");
                   
                    if (!string.IsNullOrEmpty(strBuf))
                    {
                        //文字列配列に分割
                        strCell = strBuf.Split(',');

                        if (strCell.GetUpperBound(0) >= 2)
                        {
                            if (strCell[2] == CTResources.LoadResString(12003)) //矩形
                            {
                                //変更2015/01/22hata
                                //if (!int.TryParse(strCell[3], out work3))
                                //{
                                //    continue;
                                //}
                                //if (!int.TryParse(strCell[4], out work4))
                                //{
                                //    continue;
                                //}
                                //if (!int.TryParse(strCell[5], out work5))
                                //{
                                //    continue;
                                //}
                                //if (!int.TryParse(strCell[6], out work6))
                                //{
                                //    continue;
                                //}                                
                                work3 = Convert.ToInt32(StringsToValue(strCell[3]));
                                work4 = Convert.ToInt32(StringsToValue(strCell[4]));
                                work5 = Convert.ToInt32(StringsToValue(strCell[5]));
                                work6 = Convert.ToInt32(StringsToValue(strCell[6]));

                                RoiCount = RoiCount + 1;

                                xc = work3 * m;
                                yc = work4 * m;
                                xl = work5 * m;
                                yl = work6 * m;

                                //ROI描画登録関数の呼び出し
                                // Call roi.AddRectangleShape2(RoiId, xc, yc, xl, yl) 'v9.7削除 by 間々田 2004/11/01
                                roi.AddRectangleShape2(xc, yc, xl, yl);
                                //v9.7追加 by 間々田 2004/11/01
                            }
                            //else if (strCell[1] == CTResources.LoadResString(12002))  // 円形
                            //Rev20.00 変更 by長野 2015/02/16
                            else if (strCell[2] == CTResources.LoadResString(12002))  // 円形
                            {
                                //変更2015/01/22hata
                                //if (!int.TryParse(strCell[3], out work3))
                                //{
                                //    continue;
                                //}
                                //if (!int.TryParse(strCell[4], out work4))
                                //{
                                //    continue;
                                //}
                                //if (!int.TryParse(strCell[5], out work5))
                                //{
                                //    continue;
                                //}
                                work3 = Convert.ToInt32(StringsToValue(strCell[3]));
                                work4 = Convert.ToInt32(StringsToValue(strCell[4]));
                                work5 = Convert.ToInt32(StringsToValue(strCell[5]));

                                RoiCount = RoiCount + 1;

                                xc = work3 * m;
                                yc = work4 * m;
                                r = work5 * m;

                                //ROI描画登録関数の呼び出し
                                // Call roi.AddCircleShape(RoiId, xc, yc, r)  'v9.7削除 by 間々田 2004/11/01
                                roi.AddCircleShape(xc, yc, r);
                                //v9.7追加 by 間々田 2004/11/01
                            }
                            //else if (strCell[1] == CTResources.LoadResString(12000))  // トレース
                            //Rev20.00 変更 by長野 2015/02/16
                            else if (strCell[2] == CTResources.LoadResString(12000))  // トレース
                            {
                                //変更2015/01/22hata
                                //if (!int.TryParse(strCell[3], out work3))
                                //{
                                //    continue;
                                //}
                                work3 = Convert.ToInt32(StringsToValue(strCell[3]));
                                
                                RoiCount = RoiCount + 1;

                                // If roi.AddTraceShape(RoiId) Then   'v9.7削除 by 間々田 2004/11/01
                                RoiId = roi.AddTraceShape();
                                //v9.7追加 by 間々田 2004/11/01
                                //v9.7追加 by 間々田 2004/11/01
                                if (RoiId > 0)
                                {
                                    for (i = 1; i <= work3; i++)
                                    {
                                        //変更2015/01/22hata
                                        //if (!int.TryParse(strCell[4 + 2 * (i - 1)], out workA))
                                        //{
                                        //    continue;
                                        //}
                                        //if (!int.TryParse(strCell[4 + 2 * (i - 1) + 1], out workB))
                                        //{
                                        workA = Convert.ToInt32(StringsToValue(strCell[4 + 2 * (i - 1)]));
                                        workB = Convert.ToInt32(StringsToValue(strCell[4 + 2 * (i - 1) + 1]));
                                        //    continue;
                                        //}
                                        
                                        xc = workA * m;
                                        yc = workB * m;

                                        roi.AddTracePoint(RoiId, xc, yc);
                                    }
                                }
                            }
                        }

                        //コメントの扱い
                        if (strCell[0] == CTResources.LoadResString(12816))
                        {
                            CommaPos = strWork.IndexOf(",");
                            roi.Comment = strWork.Substring(CommaPos + 1);
                            roi.Comment = roi.Comment.Replace("  ", CRLF);
                        }
                    }
                }

                functionReturnValue = true;
            }
            catch (Exception exp)
            {
                //エラーメッセージ表示
                MessageBox.Show(exp.Message + CR + CR + CTResources.LoadResString(9363), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                //ファイルクローズ
                if (file != null)
                {
                    file.Close();
                    file = null;
                }
            }

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//骨塩定量解析:読み込んだRoiの数がすでに６個の時
			//    If IsExistForm(frmBoneDensity) Then
			//        If RoiCount > 6 Then
			//            'メッセージ表示：
			//            '   選択したROIテーブルファイルには%1個のROIが登録されています。
			//            '   骨塩定量解析の際には最初の６個がロードされます。
			//            MsgBox GetResString(9595, CStr(RoiCount)), vbExclamation
			//        End If
			//    End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//最後に描画したROIを選択する
			//    roi.TargetRoi = roi.NumOfRois
			roi.SelectRoi(roi.NumOfRois);
			//v9.7変更 by 間々田 2004/11/01

			return functionReturnValue;
		}

        //*************************************************************************************************
        //機　　能： ROIテーブルの保存
        //
        //           変数名          [I/O] 型        内容
        //引　　数： fileName        [I/ ] String    ROIテーブルのファイル名
        //戻 り 値：                 [ /O] Boolean   結果（True:成功、False:失敗）
        //
        //補　　足： なし
        //
        //履　　歴： V7.00  XX/XX/XX  (SI4)間々田    リソース対応
        //*************************************************************************************************
        public static bool SaveRoiTable(string FileName)
		{
			bool functionReturnValue = false;

			int i = 0;
            //変更2015/01/22hata
            //string RoiInfoStr = null;
            string RoiInfoStr = "";

			//戻り値初期化
			functionReturnValue = false;

			//ファイルオープン
            StreamWriter file = null;

            try
            {
                //変更2015/01/22hata
                //file = new StreamWriter(FileName, false);
                file = new StreamWriter(FileName, false, Encoding.GetEncoding("shift-jis"));

			    //ヘッダの書き込み：番号,パラメータ名,ROI形状,ROI座標
                file.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(12616), 
                                                    CTResources.LoadResString(12395), 
                                                    CTResources.LoadResString(12606),
                                                    CTResources.LoadResString(12614)));

			    //ROI個数分書き込み
			    for (i = 1; i <= roi.NumOfRois; i++)
                {
				    //ROI情報をコモンにセットします(RoiInfoStrに文字列をセットするためコール)
				    SetROIToCommon(i, ref RoiInfoStr);

                    file.WriteLine(modLibrary.GetCsvRec(i, String.Format("ROICAL[{0:d}]", i - 1), RoiInfoStr));
			    }

			    //フッタの書き込み：コメントについては改行コードがあればスペースに置換する
                file.WriteLine(CTResources.LoadResString(12615) + "," + Convert.ToString(roi.NumOfRois));   //ROI個数
                file.WriteLine(CTResources.LoadResString(12802) + "," + frmImageInfo.Instance.SliceName);            //スライス名
                file.WriteLine(CTResources.LoadResString(12816) + "," + modLibrary.RemoveCRLF(roi.Comment));//コメント（改行コードをスペースに置換）
			    
                //戻り値セット
                functionReturnValue = true;
            }
            catch (Exception exp)
            {
                //エラーメッセージ表示
                MessageBox.Show(exp.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                //ファイルクローズ
                if (file != null)
                {
                    file.Close();
                    file = null;
                }
            }

			return functionReturnValue;
		}

        //*************************************************************************************************
        //機　　能： ズーミングテーブルを開く
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        public static bool OpenZoomTable(string FileName)
		{
			bool functionReturnValue = false;

			string buf = null;
			string[] strCell = null;
            int CommaPos = 0;

			//戻り値初期化
			functionReturnValue = false;

			//v16.10 4096対応のため，mの決め方を変更する  by 長野 2010/02/09
			//m = IIf(frmScanImage.hsbImage.Visible, 2, 1)

			int m = 0;
			int PicWidth = 0;
			PicWidth = frmScanImage.Instance.PicWidth;

			if (frmScanImage.Instance.hsbImage.Visible == true)
            {
				switch ((PicWidth))
                {
					case 2048:
						m = 2;
						break;

					case 4096:
						m = 4;
						break;
				}
			}
            else
            {
				m = 1;
			}

			//コメント初期化
			roi.Comment = "";

			//ROIの削除
			roi.DeleteAllRoiData();

			//ファイルオープン
            StreamReader file = null;
            try
            {
                //変更2015/01/22hata
                //file = new StreamReader(FileName);
                file = new StreamReader(FileName, Encoding.GetEncoding("shift-jis"));

                //while (!FileSystem.EOF(fileNo))
                while ((buf = file.ReadLine()) != null)
                {
                    //１行読み込む
                    if (!string.IsNullOrEmpty(buf))
                    {
                        //カンマで区切って配列に格納
                        strCell = buf.Split(',');

                        //コメントか？
                        if (strCell[0].Trim() == CTResources.LoadResString(12816))
                        {
                            CommaPos = buf.IndexOf(',');

                            if (CommaPos > 0)
                            {
                                roi.Comment = buf.Substring(CommaPos + 1);
                            }
                        }
                        //データ？
                        else if (char.IsNumber(strCell[0], 0) && strCell.GetUpperBound(0) >= 4)
                        {
                            int x;
                            int y;
                            int z;

                            if (int.TryParse(strCell[2], out x) && int.TryParse(strCell[3], out y) && int.TryParse(strCell[4], out z))
                            {
                                roi.AddSquareShape(x * m, y * m, z * m);
                            }
                        }
                    }
                }

                //戻り値セット
			    functionReturnValue = true;
            }
            catch
            {
            }
            finally
            {
                //ファイルクローズ
                if (file != null)
                {
                    file.Close();
                    file = null;
                }
            }

			//Roi表示
            //テスト表示2014/07/14hata
            //roi.IndicateRoi(g);
            //frmScanImage.Instance.Invalidate();
            //roi設定中
            roi.RoiFlg = 2;          
            frmScanImage.Instance.Refresh();


			return functionReturnValue;
		}

        //*************************************************************************************************
        //機　　能： ズーミングテーブルを保存
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        public static bool SaveZoomTable(string FileName)
		{
			bool functionReturnValue = false;

            int i = 0;
            int x = 0;
            int y = 0;
            int r = 0;
            int no = 0;

			//戻り値初期化
			functionReturnValue = false;

			//v16.10 4096対応のため，mの決め方を変更する  by 長野 2010/02/09
			//m = IIf(frmScanImage.hsbImage.Visible, 2, 1)

            int m = 0;
            int PicWidth = 0;
			PicWidth = frmScanImage.Instance.PicWidth;

			if (frmScanImage.Instance.hsbImage.Visible == true)
            {
				switch ((PicWidth))
                {
					case 2048:
						m = 2;
						break;

					case 4096:
						m = 4;
						break;
				}
			}
            else
            {
				m = 1;
			}

			//ファイルオープン
            StreamWriter file = null;

            try
            {
                //変更2015/01/22hata
                //file = new StreamWriter(FileName, false);
                file = new StreamWriter(FileName, false, Encoding.GetEncoding("shift-jis"));

			    //ヘッダの書き込み：名称,パラメータ名,Ｘセンター,Ｙセンター,ROIサイズ
                file.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(12045),
                                                    CTResources.LoadResString(12395),
                                                    CTResources.LoadResString(12602),
                                                    CTResources.LoadResString(12603),
                                                    CTResources.LoadResString(12601)));

			    no = 0;
			    for (i = 1; i <= roi.NumOfRois; i++)
                {
				    if (roi.GetSquareShape(i, ref x, ref y, ref r))
                    {
					    no = no + 1;
                        //2014/11/13hata キャストの修正
                        //file.WriteLine(modLibrary.GetCsvRec(no, String.Format("Zoomtable[{0:d}]", no - 1), x / m, y / m, r / m));
                        file.WriteLine(modLibrary.GetCsvRec(no, String.Format("Zoomtable[{0:d}]", no - 1), x / (double)m, y / (double)m, r / (double)m));
                    }
			    }

			    //フッター
                file.WriteLine(CTResources.LoadResString(12604) + "," + Convert.ToString(no));  //ｽﾞｰﾐﾝｸﾞ個数
                file.WriteLine(CTResources.LoadResString(12816) + "," + roi.Comment);           //コメント

                //戻り値セット
			    functionReturnValue = true;
            }
            catch
            {
            }
            finally
            {
                //ファイルクローズ
                if (file != null)
                {
                    file.Close();
                    file = null;
                }
            }

			return functionReturnValue;
		}

        //*************************************************************************************************
        //機　　能： プロフィールディスタンス・テーブルファイル読み込み
        //
        //           変数名          [I/O] 型        内容
        //引　　数： fileName        [I/ ] String    テーブルファイル名
        //戻 り 値：                 [ /O] Boolean   True:成功 False:失敗
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        public static bool LoadPRDTable(string FileName)
		{
			bool functionReturnValue = false;

			//int fileNo = 0;
			string[] strCell = null;
			string strWork = null;
            int CommaPos = 0;

			//戻り値初期化
			functionReturnValue = false;

			//v16.10 4096対応のため，mの決め方を変更する  by 長野 2010/02/09
			//m = IIf(frmScanImage.hsbImage.Visible, 2, 1)

            int m = 0;
            int PicWidth = 0;
			PicWidth = frmScanImage.Instance.PicWidth;

			if (frmScanImage.Instance.hsbImage.Visible == true)
            {
				switch ((PicWidth))
                {
					case 2048:
						m = 2;
						break;

					case 4096:
						m = 4;
						break;
				}
			}
            else
            {
				m = 1;
			}

            int work2; int work3; int work4;
            int work5; int work6; int work7;
            int work8; int work9; int work10;
            int work11; int work12; int work13;

            //ファイルオープン
            StreamReader file = null;

            try
            {
                //変更2015/01/22hata
                //file = new StreamReader(FileName);
                file = new StreamReader(FileName, Encoding.GetEncoding("shift-jis"));

                while ((strWork = file.ReadLine()) != null)
                {
				    if (!string.IsNullOrEmpty(strWork))
                    {
					    //文字列配列に分割
                        strCell = strWork.Replace(";", ",").Split(',');

                        if ((strCell.GetUpperBound(0) >= 6) && (strCell[0].Trim() == "1"))
                        {
                            #region String→数値 変換処理

                            if (!int.TryParse(strCell[2], out work2))
                            {
                                continue;
                            }
                            if (!int.TryParse(strCell[3], out work3))
                            {
                                continue;
                            }
                            if (!int.TryParse(strCell[4], out work4))
                            {
                                continue;
                            }
                            if (!int.TryParse(strCell[5], out work5))
                            {
                                continue;
                            }
                            if (!int.TryParse(strCell[6], out work6))
                            {
                                continue;
                            }
                            if (!int.TryParse(strCell[7], out work7))
                            {
                                continue;
                            }
                            if (!int.TryParse(strCell[8], out work8))
                            {
                                continue;
                            }
                            if (!int.TryParse(strCell[9], out work9))
                            {
                                continue;
                            }
                            if (!int.TryParse(strCell[10], out work10))
                            {
                                continue;
                            }
                            if (!int.TryParse(strCell[11], out work11))
                            {
                                continue;
                            }
                            if (!int.TryParse(strCell[12], out work12))
                            {
                                continue;
                            }
                            if (!int.TryParse(strCell[13], out work13))
                            {
                                continue;
                            }
                            #endregion

						    //１点指定時
                            if (work2 == 1)
                            {
							    //線分の初期化
                                modImgProc.P1.x = work3 * m;
                                modImgProc.P1.y = work4 * m;

							    //１点モードにする
                                //frmScanImage.Instance.ClickToolBarButton(frmScanImage.Instance.Toolbar1.Items["Point"]);
                                frmScanImage.Instance.ClickToolBarButton("tsbtnPoint");
                                
                            }
                            //２点指定時
                            else
                            {
							    //２点モードにする
                                //frmScanImage.Instance.ClickToolBarButton(frmScanImage.Instance.Toolbar1.Items["Line"]);
                                frmScanImage.Instance.ClickToolBarButton("tsbtnLine");


							    //線分を登録
                                roi.AddLineShape(work3 * m, work4 * m, work5 * m, work6 * m);
						    }

						    //その他の項目も読み出す 追加（中央値と１目盛は Ver.10.2からファイルに追加）by 間々田 2005/06/28
                            if (strCell.GetUpperBound(0) >= 7)      //下限閾値
                            {
                                modImgProc.CT_Low = (short)work7;
                            }

                            if ((strCell.GetUpperBound(0) >= 8))     //上限閾値
                            {
                                modImgProc.CT_High = (short)work8;
                            }

                            if ((strCell.GetUpperBound(0) >= 9))     //連結幅
                            {
                                modImgProc.CT_Unit = (short)work9;
                            }

                            if ((strCell.GetUpperBound(0) >= 10))    //１点指定下限閾値
                            {
                                modImgProc.CT_Low1Point = (short)work10;
                            }

                            if ((strCell.GetUpperBound(0) >= 11))    //１点指定上限閾値
                            {
                                modImgProc.CT_High1Point = (short)work11;
                            }

                            if ((strCell.GetUpperBound(0) >= 12))    //中央値
                            {
                                modImgProc.CT_Bias = (short)work12;
                            }

                            if ((strCell.GetUpperBound(0) >= 13))   //１目盛
                            {
                                modImgProc.CT_Int = (short)work13;
                            }
					    }

					    //コメントの読み込み
					    if (strCell[0] == CTResources.LoadResString(12816))
                        {
                            CommaPos = strWork.IndexOf(',');
                            roi.Comment = strWork.Substring(CommaPos + 1);
                            roi.Comment = roi.Comment.Replace("  ", CRLF);
					    }
				    }
			    }

                //戻り値セット
                functionReturnValue = true;
            }
            catch (Exception exp)
            {
                //エラーメッセージ表示
                MessageBox.Show(exp.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                //ファイルクローズ
                if (file != null)
                {
                    file.Close();
                    file = null;
                }
            }

			//最後に描画したROIを選択する        '追加 2005/04/15 by 間々田
			roi.SelectRoi(roi.NumOfRois);

			return functionReturnValue;
		}

        //*************************************************************************************************
        //機　　能： プロフィールディスタンス・テーブルファイル保存
        //
        //           変数名          [I/O] 型        内容
        //引　　数： fileName        [I/ ] String    テーブルファイル名
        //戻 り 値：                 [ /O] Boolean   True:成功 False:失敗
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        public static bool SavePRDTable(string FileName)
		{
			bool functionReturnValue = false;

			//int fileNo = 0;
            int x1 = 0;
            int y1 = 0;
            int x2 = 0;
            int y2 = 0;
            int PointNum = 0;

			//戻り値初期化
			functionReturnValue = false;

			//v16.10 4096対応のため，mの決め方を変更する  by 長野 2010/02/09
			//m = IIf(frmScanImage.hsbImage.Visible, 2, 1)

			int m = 0;
			int PicWidth = 0;
            PicWidth = frmScanImage.Instance.PicWidth;

            if (frmScanImage.Instance.hsbImage.Visible == true)
            {
				switch ((PicWidth))
                {
					case 2048:
						m = 2;
						break;

					case 4096:
						m = 4;
						break;
				}
			}
            else
            {
				m = 1;
			}

			//処理が行なわれていない場合，現在のROIを取得
            if (modImgProc.PRDPoint == 0)
            {
				roi.GetLineShape(1, ref x1, ref y1, ref x2, ref y2);
				PointNum = 2;
			}
            else
            {
                x1 = modImgProc.P1.x;
                y1 = modImgProc.P1.y;
                x2 = modImgProc.P2.x;
                y2 = modImgProc.P2.y;
                PointNum = modImgProc.PRDPoint;
			}

            //ファイルオープン
            StreamWriter file = null;

            try
            {
                //変更2015/01/22hata
                //file = new StreamWriter(FileName, false);
                file = new StreamWriter(FileName, false, Encoding.GetEncoding("shift-jis"));

			    //ヘッダの書き込み：番号,パラメータ名,指定点数,X1,Y1,X2,Y2,下限閾値,上限閾値,連結幅,１点指定下限閾値,１点指定上限閾値,中央値,１目盛
                file.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(12616),
                                                    CTResources.LoadResString(12395),
                                                    CTResources.LoadResString(12626), 
                                                    "X1", "Y1", "X2", "Y2",
                                                    CTResources.LoadResString(12505),
                                                    CTResources.LoadResString(12506), 
                                                    CTResources.LoadResString(12507),
			                                        CTResources.LoadResString(12627),
                                                    CTResources.LoadResString(12628),
                                                    CTResources.LoadResString(12403), 
                                                    CTResources.LoadResString(12401)));

			    //データ書き込み
                //2014/11/13hata キャストの修正
                //file.WriteLine(modLibrary.GetCsvRec(1, "PRD[0]", 
                //                                    PointNum, x1 / m, y1 / m, (PointNum == 1 ? -1 : x2 / m),
                //                                    (PointNum == 1 ? -1 : y2 / m),
                //                                    modImgProc.CT_Low,
                //                                    modImgProc.CT_High,
                //                                    modImgProc.CT_Unit,
                //                                    modImgProc.CT_Low1Point,
                //                                    modImgProc.CT_High1Point,
                //                                    modImgProc.CT_Bias,
                //                                    modImgProc.CT_Int));
                file.WriteLine(modLibrary.GetCsvRec(1, "PRD[0]",
                                                    PointNum, 
                                                    Convert.ToInt32(x1 / (float)m), 
                                                    Convert.ToInt32(y1 / (float)m), 
                                                    (PointNum == 1 ? -1 : Convert.ToInt32(x2 / (float)m)),
                                                    (PointNum == 1 ? -1 : Convert.ToInt32(y2 / (float)m)),
                                                    modImgProc.CT_Low,
                                                    modImgProc.CT_High,
                                                    modImgProc.CT_Unit,
                                                    modImgProc.CT_Low1Point,
                                                    modImgProc.CT_High1Point,
                                                    modImgProc.CT_Bias,
                                                    modImgProc.CT_Int));
			    
                //フッタの書き込み：コメントについては改行コードがあればスペースに置換する
                file.WriteLine(CTResources.LoadResString(12625) + "," + Convert.ToString(1));   //線分数
                file.WriteLine(CTResources.LoadResString(12802) + "," + frmImageInfo.Instance.SliceName);    //スライス名
                file.WriteLine(CTResources.LoadResString(12816) + "," + modLibrary.RemoveCRLF(roi.Comment));    //コメント

			    //戻り値セット
			    functionReturnValue = true;
            }
            catch (Exception exp)
            {
                //エラーメッセージ表示
                MessageBox.Show(exp.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                //ファイルクローズ
                if (file != null)
                {
                    file.Close();
                    file = null;
                }
            }
		
            return functionReturnValue;
        }

        //追加2015/01/22hata
        //文字列から数値を抜き出す(VB6.0のVal関数と同じ)
        //先頭から検索。数字以外の文字が見つかったとこで中止。
        public static decimal StringsToValue(string ValText)
        {
            string strBuf = "";
            string strSel = "";
            int i = 0;
            int work = 0;
            decimal retVal = 0;

            int len = ValText.Length;

            for (i = 0; i < len; ++i)
            {
                work = 0;
                strSel = ValText.Substring(i, 1);
                if (!int.TryParse(strSel, out work))
                {
                    if (!strSel.Equals(".")) break;
                }
                strBuf += strSel;
            }
            if (!decimal.TryParse(strBuf, out retVal))
            {
                retVal = 0;
            }

            return retVal;                                
        }

    }
}
