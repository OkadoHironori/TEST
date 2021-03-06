using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
//
using CTAPI;
using CT30K.Common;
using TransImage;

using System.Text; //Rev20.00 追加 by長野 2014/09/11
using System.Collections.Generic;//Rev26.00 add by chouno 2017/08/31 

namespace CT30K
{
	internal static class modScanCondition
	{

        public static int ScanJe = 0;
        public static int ScanJs = 0;
        public static bool ScanStartFlg = false;
        public static bool ExScanStartAbortFlg = false; //Rev20.00 追加 by長野 2015/02/21
        public static int HalfNoAutoCenteringFlg = 0; //Rev20.00 追加 by長野 2015/01/24
        public static float bakScanoUdPos = 0; //Rev21.00 追加 by長野 2015/03/09

        public static List<string> PresetName = new List<string>();//Rev26.00 add by chouno 2017/08/31
        public static List<string> PresetPath = new List<string>();//Rev26.00 add by chouno 2017/08/31
      
        public static int PresetSelectedIndex = -1;//Rev26.00 add by chouno 2017/08/31

        //*************************************************************************************************
		//機　　能： スキャン条件ファイルの読み込み
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//          V19.00  2012/03/15 H.Nagai       BHC設定を追加
		//*************************************************************************************************
		public static bool LoadSCFile(string FileName)
		{
			
			string[] strCell = null;
			string strBuf = null;
			bool IsPreset = false;

			double? delta_msw = null;
			double? delta_z_pix = null;
			string sp_table_dir = null;			//追加 2009/08/21 by 間々田
			string sp_table = null;				//追加 2009/08/20 by 間々田
			int? data_mode = null;				//v15.02追加 by 間々田 2009/09/14
			float? mscan_area = null;			//v15.02追加 by 間々田 2009/09/14
			float? cone_scan_area = null;		//v15.02追加 by 間々田 2009/09/14
            int? w_scan = null;                 //Rev25.00 追加 by長野 2016/07/07
            int? mbhc_phantomless = null;       //Rev26.00 追加 by井上 2017/01/20

			//v19.00 BHC設定
			int mbhc_flag = 0;
			string mbhc_dir = null;
			string mbhc_name = null;

			delta_msw = null;			//追加 2009/08/20 by 間々田
			delta_z_pix = null;			//追加 2009/08/20 by 間々田
			sp_table_dir = null;		//追加 2009/08/21 by 間々田
			sp_table = null;			//追加 2009/08/21 by 間々田
			data_mode = null;			//v15.02追加 by 間々田 2009/09/14
			mscan_area = null;			//v15.02追加 by 間々田 2009/09/14
			cone_scan_area = null;		//v15.02追加 by 間々田 2009/09/14
            w_scan = null;              //Rev25.00 追加 by長野 2016/07/07

			//戻り値初期化
			bool functionReturnValue = false;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			'ファイル名チェック
			If Not (UCase$(FileName) Like "*-SC.CSV") Then Exit Function
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//ファイル名チェック
			if (!Regex.IsMatch(FileName.ToUpper(), "^.+-SC[.]CSV$")) return functionReturnValue;

			//プリセットファイル？
			//Select Case UCase$(FSO.GetBaseName(FileName))
			//    Case "QUICK-SC", "ROUGH-SC", "NORMAL-SC", "FINE-SC"
			//        'IsPreset = False
			//        IsPreset = (UCase$(FSO.GetParentFolderName(FileName)) <> UCase$(pathSetFile)) '変更 by 間々田 2009/08/21
			//    Case Else
			//        IsPreset = True
			//End Select
			IsPreset = IsPresetFile(FileName);			//v15.02関数化 by 間々田 2009/09/14

			//最新 scancel（コモン）取得
			//modScansel.GetScansel(ref modScansel.scansel);
            CTSettings.scansel.Load();

			StreamReader sr = null;

			//エラー時の扱い
			try 
			{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
					'ファイルオープン
					fileNo = FreeFile()
					Open FileName For Input As #fileNo

					Do While Not EOF(fileNo)

						'１行読み込み
						Line Input #fileNo, strBuf
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				try 
				{
					//ファイルオープン
                    //変更2015/01/22hata
                    //sr = new StreamReader(FileName);
                    sr = new StreamReader(FileName, Encoding.GetEncoding("shift-jis"));

					while ((strBuf = sr.ReadLine()) != null)	//１行読み込み
					{
						if (!string.IsNullOrEmpty(strBuf))
						{
							//文字列配列に分割
							strCell = strBuf.Split(',');

							if (strCell.GetUpperBound(0) >= 2)
							{
								int intParseValue = 0;
								float floatParseValue = 0;
								double doubleParseValue = 0;

								switch (strCell[1].Trim().ToLower())
								{
									//データモード（1:スキャン, 4:コーンビーム）'v15.02追加 プリセット時、データモードも反映させる by 間々田 2009/09/14							
									case "data_mode":
										if (IsPreset)
										{
											data_mode = int.TryParse(strCell[2], out intParseValue) ? intParseValue : data_mode;
										}
										break;

									//マルチスキャンモード（1:シングル, 3:マルチ, 5:スライスプラン）
									case "multiscan_mode":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.multiscan_mode);
										break;

									//マルチスキャン用ピッチ                     'v15.02追加 プリセット時マルチスキャンのピッチを保存する by 間々田 2009/09/14
									case "pitch":
										if (IsPreset)
										{
                                            float.TryParse(strCell[2], out CTSettings.scansel.Data.pitch);
										}
										break;

									//マルチスキャン用スライス枚数               'v15.02追加 プリセット時マルチスキャンのスライス枚数を保存する by 間々田 2009/09/14
									case "multinum":
										if (IsPreset)
										{
                                            int.TryParse(strCell[2], out CTSettings.scansel.Data.multinum);
										}
										break;

									//スキャンモード（1:フル, 2:ハーフ, 3:オフセット）
									case "scan_mode":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.scan_mode);
										break;

                                    //Wスキャン Rev25.00 追加 by長野 2016/07/07
                                    case "w_scan":
                                        if (IsPreset)
                                        {
                                            w_scan = int.TryParse(strCell[2], out intParseValue) ? intParseValue : w_scan;
                                        }
                                        break;

									//マトリクスサイズ（1:256×256, 2:512×512, 3:1024×1024, 3:2048×2048, 4:4096×4096) 'v16.10 コメントに4:を追加 by 長野 10/01/29
									case "matrix_size":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.matrix_size);
										break;

									//ビュー数
									case "scan_view":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.scan_view);
										break;

									//画像積算枚数
									case "scan_integ_number":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.scan_integ_number);
										break;

									//スキャンエリア
									case "mscan_area":
										if (IsPreset)	//v15.02 if部分のみ追加 画質選択時（プリセット以外）はCSVに含まれていても無視する by 間々田 2009/09/14
										{
											mscan_area = float.TryParse(strCell[2], out floatParseValue) ? floatParseValue : mscan_area;
										}
										break;

									//コーンビーム用スキャンエリア           'v15.02追加 プリセット時、コーンビーム用スキャンエリアをロードする by 間々田 2009/09/14
									case "cone_scan_area":
										if (IsPreset)
										{
											cone_scan_area = float.TryParse(strCell[2], out floatParseValue) ? floatParseValue : cone_scan_area;
										}
										break;

									//スライス厚（画素数, コモン設定時はmmに変更する）
									case "delta_msw":
										//delta_msw = double.TryParse(strCell[2], out doubleParseValue) ? doubleParseValue : delta_msw;
                                        //Rev20.00 変更 by長野 2015/01/26
                                        if (double.TryParse(strCell[2], out doubleParseValue))
                                        {
                                            delta_msw = doubleParseValue;
                                        }
                                        break;

									//バイアス
									case "mscan_bias":
                                        float.TryParse(strCell[2], out CTSettings.scansel.Data.mscan_bias);
										break;

									//スロープ
									case "mscan_slope":
										float.TryParse(strCell[2], out CTSettings.scansel.Data.mscan_slope);
										break;

									//フィルタ関数（1:Laks, 2:shepp, 3:Sharpen）
									case "filter":
										int.TryParse(strCell[2], out CTSettings.scansel.Data.filter);
										break;

									//スキャン中再構成（0:しない, 1:する）
									case "scan_and_view":
										int.TryParse(strCell[2], out CTSettings.scansel.Data.scan_and_view);
										break;

									//画像方向（0:上から見た画像, 1:下から見た画像）
									case "image_direction":
										int.TryParse(strCell[2], out CTSettings.scansel.Data.image_direction);
										break;

									//マルチスライス（0:1スライス, 1:3スライス, 2:5スライス）
									case "multislice":
										int.TryParse(strCell[2], out CTSettings.scansel.Data.multislice);
										break;

									//オートズーミング（0:なし, 1:あり）
									case "auto_zoomflag":
										int.TryParse(strCell[2], out CTSettings.scansel.Data.auto_zoomflag);
										break;

									//オートズームファイルのディレクトリ
									case "autozoom_dir":
										//modLibrary.SetField(strCell[2].Trim(), ref CTSettings.scansel.Data.autozoom_dir);
                                        CTSettings.scansel.Data.autozoom_dir.SetString(strCell[2].Trim());
                                        break;

									//オートズームファイル名
									case "auto_zoom":
										//modLibrary.SetField(strCell[2].Trim(), ref CTSettings.scansel.Data.auto_zoom);
                                        CTSettings.scansel.Data.auto_zoom.SetString(strCell[2].Trim());
										
                                        break;

									//生データ保存（0:なし, 1:あり）
									case "rawdata_save":
										int.TryParse(strCell[2], out CTSettings.scansel.Data.rawdata_save);
										break;

									//rfc（0:なし, 1:弱, 2:中, 3:強）
									case "rfc":
										int.TryParse(strCell[2], out CTSettings.scansel.Data.rfc);
										break;

									//オートプリント（0:なし, 1:あり）
									case "auto_print":
										int.TryParse(strCell[2], out CTSettings.scansel.Data.auto_print);
										break;

									//透視画像保存（0:なし, 1:あり）
									case "fluoro_image_save":
										int.TryParse(strCell[2], out CTSettings.scansel.Data.fluoro_image_save);
										break;

									//アーティファクト低減（0:なし, 1:あり）
									case "artifact_reduction":
										int.TryParse(strCell[2], out CTSettings.scansel.Data.artifact_reduction);
										break;

									//再構成形状（0:正方形, 1:円）
									case "recon_mask":
										int.TryParse(strCell[2], out CTSettings.scansel.Data.recon_mask);
										break;

									//画像階調最適化（0:なし, 1:あり）
									case "contrast_fitting":
										int.TryParse(strCell[2], out CTSettings.scansel.Data.contrast_fitting);
										break;

									//画像回転角度
									case "image_rotate_angle":
										float.TryParse(strCell[2], out CTSettings.scansel.Data.image_rotate_angle);
										break;

									//テーブル回転（0:ステップ送り, 1:連続回転）
									case "table_rotation":
										int.TryParse(strCell[2], out CTSettings.scansel.Data.table_rotation);
										break;

									//'オートセンタリング（0:なし, 1:あり）      'v15.01削除 by 間々田 2009/08/28
									//Case "auto_centering"
									//    .auto_centering = Val(strCell(2))

									//ビニング（0:1x1, 1:2x2, 2:4x4）
									case "binning":
										int.TryParse(strCell[2], out CTSettings.scansel.Data.binning);
										break;

									//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
									//                        'オーバースキャン（0:なし, 1:あり）
									//                        Case "over_scan"
									//                            .over_scan = Val(strCell(2))
									//
									//                        'メール送信（0:なし, 1:あり）
									//                        Case "mail_send"
									//                            .mail_send = Val(strCell(2))
									//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

									//画質（0:標準, 1:精細, 2:標準&高速, 3:精細&高速）
									case "cone_image_mode":
										int.TryParse(strCell[2], out CTSettings.scansel.Data.cone_image_mode);
										break;

									//スライスピッチ（画素数, コモン設定時はmmに変更する）
									case "delta_z_pix":
										//double value = 0;
										//delta_z_pix = double.TryParse(strCell[2], out doubleParseValue) ? doubleParseValue : delta_z_pix;
                                        //Rev20.00 変更 by長野 2015/01/26
                                        if (double.TryParse(strCell[2], out doubleParseValue))
                                        {
                                            delta_z_pix = doubleParseValue;
                                        }
                                        break;

									//スライス枚数
									case "k":
										if (IsPreset)
										{
											int.TryParse(strCell[2], out CTSettings.scansel.Data.k);
										}
										break;

									//登録条件のときは動くべきではない
									//Rev17.61削除(ここから) byやまおか 2011/06/28
									//'Ｘ線管電圧（[kV]）
									//Case "scan_kv"
									//
									//    If IsPreset Then
									//        SetVolt Val(strCell(2))
									//        '.scan_kv = frmXrayControl.cwneMA.value
									//   End If
									//
									//'Ｘ線管電流（[μA]）
									//Case "scan_ma"
									//
									//    If IsPreset Then
									//        '東芝 EXM2-150の場合は、単位を mA に変換
									//        If XrayType = XrayTypeToshibaEXM2_150 Then
									//            SetCurrent Val(strCell(2)) / 1000
									//        Else
									//            SetCurrent Val(strCell(2))
									//        End If
									//        '.scan_ma = frmXrayControl.cwneMA.value
									//    End If
									//
									//'FCD
									//Case "fcd"
									//    If IsPreset Then
									//        .FCD = Val(strCell(2))
									//    End If
									//
									//'FID
									//Case "fid"
									//    If IsPreset Then
									//        .Fid = Val(strCell(2))
									//    End If
									//Rev17.61削除(ここまで) byやまおか 2011/06/28

									//スライスプランテーブルのディレクトリ名 '追加 by 間々田 2009/08/21
									case "sliceplan_dir":
										sp_table_dir = strCell[2].Trim();
										break;

									//スライスプランテーブルのテーブル名     '追加 by 間々田 2009/08/21
									case "sliceplan":
										sp_table = strCell[2].Trim();
										break;

									//v19.00 BHC設定追加 ->(電S2)永井
									case "mbhc_flag":
										mbhc_flag = int.TryParse(strCell[2], out intParseValue) ? intParseValue : mbhc_flag;
										break;

									case "mbhc_dir":
										mbhc_dir = strCell[2].Trim();
										break;

									case "mbhc_name":
										mbhc_name = strCell[2].Trim();
										break;
									//<- v19.00
                                    case "mbhc_phantomless": //Rev26.00 追加 by 井上 2017/02/09 
                                        if (IsPreset)
                                        {
                                            int.TryParse(strCell[2], out CTSettings.scansel.Data.mbhc_phantomless);
                                        }
                                        break;
								}
							}
						}
					}
				}
				catch
				{
					throw;
				}
				finally
				{
					//ファイルクローズ
					if (sr != null)
					{
						sr.Close();
						sr = null;
					}
				}

				//登録条件の場合は動くべきではない
				//'プリセットの場合、所定のFCD/FID位置に移動  'v17.61削除 byやまおか 2011/06/28
				//If IsPreset Then
				//    frmMechaMove.MechaMove , , .FCD - scancondpar.fcd_offset(GetFcdOffsetIndex()), .Fid - scancondpar.fid_offset(GFlg_MultiTube)
				//End If

				//プリセット
				if (IsPreset)
				{
					//データモードが指定されていれば                                         'v15.02追加ここから by 間々田 2009/09/14
					if (data_mode != null)
					{
						CTSettings.scansel.Data.data_mode = (int)data_mode;

						//スキャンエリアが指定されていれば
						if ((mscan_area != null) && (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScan))
						{
							CTSettings.scansel.Data.mscan_area = (float)mscan_area;
						}

						//コーンビーム用エリアが指定されていれば
						if ((cone_scan_area != null) && (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone))
						{
							CTSettings.scansel.Data.cone_scan_area = (float)cone_scan_area;
						}
					}
					//データモードが不明
					else
					{
						//スキャンエリアが指定されていれば
						if (mscan_area != null)
						{
							CTSettings.scansel.Data.mscan_area = (float)mscan_area;
							CTSettings.scansel.Data.cone_scan_area = (float)mscan_area;			//コーンビーム側にも入れておく
						}

						//コーンビーム用エリアが指定されていれば
						if (cone_scan_area != null)
						{
							CTSettings.scansel.Data.cone_scan_area = (float)cone_scan_area;
						}
					}																		//v15.02追加ここまで by 間々田 2009/09/14

                    //Rev25.00 Wスキャン 追加 by長野 2016/07/07
                    if (w_scan != null)
                    {
                        CTSettings.scansel.Data.w_scan = (int)w_scan;
                    }

					//スライスプランの場合   '追加 by 間々田 2009/08/21
					if ((CTSettings.scansel.Data.multiscan_mode == (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan) &&
						(sp_table_dir != null) && 
						(sp_table != null) && 
						(!string.IsNullOrEmpty(sp_table_dir)) && 
						(!string.IsNullOrEmpty(sp_table)))
					{

						if (Regex.IsMatch(sp_table.ToUpper(), "^.+-CSP$"))
						{
							//modLibrary.SetField(sp_table_dir, ref CTSettings.scansel.Data.cone_sliceplan_dir);
                            CTSettings.scansel.Data.cone_sliceplan_dir.SetString(sp_table_dir);

                            //modLibrary.SetField(sp_table, ref CTSettings.scansel.Data.cone_slice_plan);
                            CTSettings.scansel.Data.cone_slice_plan.SetString(sp_table);

						}
						else
						{
							//modLibrary.SetField(sp_table_dir, ref CTSettings.scansel.Data.sliceplan_dir);
                            CTSettings.scansel.Data.sliceplan_dir.SetString(sp_table_dir);
							
                            //modLibrary.SetField(sp_table, ref CTSettings.scansel.Data.slice_plan);
                            CTSettings.scansel.Data.slice_plan.SetString(sp_table);
                        }
					}
				}

				//frmScanCondition.Setupで設定することにした by 間々田 2009/08/20
				//'後処理：スライス厚（画素→mm）
				//If Not IsEmpty(delta_msw) Then
				//    .delta_msw = delta_msw
				//    .mscan_width = Trans_PixToWid(delta_msw, .Fid, .FCD, scancondpar.mdtpitch(2), vm / hm)
				//    .cone_scan_width = delta_msw * .FCD / .Fid * scancondpar.dpm
				//    'If .max_cone_slice_width < .cone_scan_width Then .max_cone_slice_width = .cone_scan_width '追加 2009/08/20 by 間々田
				//End If
				//
				//'後処理：スライスピッチ（画素→mm）
				//If Not IsEmpty(delta_z_pix) Then
				//    '.delta_z = Trans_PixToWid(delta_z_pix, .FID, .FCD, scancondpar.mdtpitch(2), vm / hm)
				//    .delta_z = delta_z_pix * .FCD / .Fid * scancondpar.dpm
				//End If

				//マトリクスの調整
                var optdatamode1 = frmScanControl.Instance.optDataMode1;
                var optdatamode4 = frmScanControl.Instance.optDataMode4;

				//if ((frmScanControl.Instance.optDataMode[(int)ScanSel.DataModeConstants.DataModeScan].Checked &&
                if ((optdatamode1.Checked &&
                     (CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize256)))
				{
					//メッセージ表示：
					//   シングルスキャンではマトリクスサイズ=256には対応していません。
					//   強制的にマトリクスサイズ=512に変更します。
					//MessageBox.Show(StringTable.GetResString(15202, frmScanControl.Instance.optDataMode[(int)ScanSel.DataModeConstants.DataModeScan].Text, "256", "512"),
                    MessageBox.Show(StringTable.GetResString(15202, optdatamode1.Text, "256", "512"),
									Application.ProductName,MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					CTSettings.scansel.Data.matrix_size = (int)ScanSel.MatrixSizeConstants.MatrixSize512;
				}
				//4096対応のためコメントアウト v16.10 by 長野 10/01/29
				//ElseIf frmScanControl.optDataMode(DataModeCone).value And (.matrix_size = MatrixSize2048) Then
                //Rev20.01 修正 by長野 2015/05/19
                //else if (optdatamode4.Checked && 
                //        ((CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize2048) || (CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize4096)))
                //{
                //    //メッセージ表示：
                //    //コーンビームスキャンではマトリクスサイズ=2048以上には対応していません。
                //    //強制的にマトリクスサイズ=1024に変更します。
                //    MessageBox.Show(StringTable.GetResString(15212, optdatamode4.Text, "2048", "1024"),
                //                    Application.ProductName,MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    CTSettings.scansel.Data.matrix_size = (int)ScanSel.MatrixSizeConstants.MatrixSize1024;
                //}
                else if (optdatamode4.Checked &&
                        //((CTSettings.scaninh.Data.cone_matrix[3] == 1) || (CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize2048)))
                        //Rev20.02 修正 by長野 2015/06/25
                        ((CTSettings.scaninh.Data.cone_matrix[3] == 1) && (CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize2048)))
                {
                    //メッセージ表示：
                    //コーンビームスキャンではマトリクスサイズ=2048以上には対応していません。
                    //強制的にマトリクスサイズ=1024に変更します。
                    MessageBox.Show(StringTable.GetResString(15212, optdatamode4.Text, "2048", "1024"),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    CTSettings.scansel.Data.matrix_size = (int)ScanSel.MatrixSizeConstants.MatrixSize1024;
                }
                else if (optdatamode4.Checked &&
                        //((CTSettings.scaninh.Data.cone_matrix[4] == 1) || (CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize4096)))
                        //Rev20.02 修正 by長野 2015/6/25 
                        ((CTSettings.scaninh.Data.cone_matrix[4] == 1) && (CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize4096)))
                {
                    //メッセージ表示：
                    //コーンビームスキャンではマトリクスサイズ=4096以上には対応していません。
                    //強制的にマトリクスサイズ=1024に変更します。
                    MessageBox.Show(StringTable.GetResString(15212, optdatamode4.Text, "4096", "1024"),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    CTSettings.scansel.Data.matrix_size = (int)ScanSel.MatrixSizeConstants.MatrixSize1024;
                }

				//v19.00 BHC設定(電S2)永井
				CTSettings.scansel.Data.mbhc_flag = mbhc_flag;
				//CTSettings.scansel.Data.mbhc_dir = mbhc_dir;
                CTSettings.scansel.Data.mbhc_dir.SetString((string.IsNullOrEmpty(mbhc_dir)? "" : mbhc_dir));

				//CTSettings.scansel.Data.mbhc_name = mbhc_name;
                CTSettings.scansel.Data.mbhc_name.SetString((string.IsNullOrEmpty(mbhc_name)? "" : mbhc_name));

                if (!string.IsNullOrEmpty(CTSettings.scansel.Data.mbhc_dir.GetString()))
                {
                    if (CTSettings.scansel.Data.mbhc_dir.GetString().Substring(0, 1) != "\\")
                    {
                        //CTSettings.scansel.Data.mbhc_dir = CTSettings.scansel.Data.mbhc_dir.GetString() + "\\";
                        CTSettings.scansel.Data.mbhc_dir.SetString(CTSettings.scansel.Data.mbhc_dir.GetString() + "\\");
                    }
                }

                //Rev26.00 ファントムレスBHC 追加 by 井上 2017/02/10
                if (mbhc_phantomless != null)
                {
                    CTSettings.scansel.Data.mbhc_phantomless = (int)mbhc_phantomless;
                }

				//scancel（コモン）更新
				//modScansel.PutScansel(ref modScansel.scansel);
                CTSettings.scansel.Write();

                //スキャン条件設定   v15.0変更 by 間々田 2009/06/17
                if (IsPreset)
                {
                    //frmScanCondition.Setup
                    //frmScanCondition.Instance.Setup(swPix: delta_msw, delta_z_pix: delta_z_pix);												//変更 2009/08/20 by 間々田
                    frmScanCondition.Instance.Setup(false , null, 0, (double)delta_msw, (double)delta_z_pix);												//変更 2009/08/20 by 間々田
                }
                else
                {
                    //frmScanCondition.Setup , , scancondpar.klimit   'Q/R/N/Fを選んだ時はKを最大値にする 2009/07/25
                    //frmScanCondition.Instance.Setup(K: CTSettings.scancondpar.Data.klimit, delta_z_pix: delta_msw, delta_z_pix: delta_z_pix);	//変更 2009/08/20 by 間々田
                    frmScanCondition.Instance.Setup(false ,null , CTSettings.scancondpar.Data.klimit, (double)delta_msw, (double)delta_z_pix);	//変更 2009/08/20 by 間々田
                }

				//戻り値セット
				functionReturnValue = true;
				return functionReturnValue;
			}
			catch
			{
				//    MsgBox Err.Description, vbExclamation
			}
			return functionReturnValue;
		}


		//*************************************************************************************************
		//機　　能： スキャン条件ファイルの保存
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		//public static bool SaveSCFile(string FileName, CTAPI.CTstr.SCANSEL theScansel)
		public static bool SaveSCFile(string FileName, CTAPI.CTstr.SCANSEL theScansel,string comment)//Rev26.00 change by chouno 2017/08/31
        {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			Dim fileNo      As Integer
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
			
			bool IsPreset = false;
			string Item = null;

			//戻り値初期化
			bool functionReturnValue = false;

			//プリセットファイル？
			//Select Case UCase$(FSO.GetBaseName(FileName))
			//    Case "QUICK-SC", "ROUGH-SC", "NORMAL-SC", "FINE-SC"
			//        'IsPreset = False
			//        IsPreset = (UCase$(FSO.GetParentFolderName(FileName)) <> UCase$(pathSetFile)) '変更 by 間々田 2009/08/21
			//    Case Else
			//        IsPreset = True
			//End Select
			IsPreset = IsPresetFile(FileName);			//v15.02関数化 by 間々田 2009/09/14

			StreamWriter sw = null;

			//エラー時の扱い
			try
			{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				'ファイルオープン
				fileNo = FreeFile()
				Open FileName For Output As #fileNo
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				//ファイルオープン
                //変更2015/01/22hata
                //sw = new StreamWriter(FileName);
                sw = new StreamWriter(FileName, false, Encoding.GetEncoding("shift-jis"));

				//ヘッダの書き込み：名称,パラメータ名,値
				sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_Name),
												  CTResources.LoadResString(StringTable.IDS_ParameterName),
												  CTResources.LoadResString(StringTable.IDS_Value)));

				//各項目の書き込み

				//データモード                               'v15.02追加 プリセット時データモードを保存する by 間々田 2009/09/14
				if (IsPreset)
				{
					//データモード
					//sw.WriteLine( modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_DataMode)), "data_mode", theScansel.data_mode.ToString());
                    //Rev20.00 修正 by長野 2014/12/04
                    sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_DataMode), "data_mode", theScansel.data_mode.ToString()));

                }

				//マルチスキャンモード
				//sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_MultiScanMode)), "multiscan_mode", theScansel.multiscan_mode.ToString());
                //Rev20.00 修正 by長野 2014/12/04
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_MultiScanMode), "multiscan_mode", theScansel.multiscan_mode.ToString()));

                //マルチスキャン用ピッチ・スライス枚数   'v15.02追加 プリセット時マルチスキャンのピッチ・スライス枚数を保存する by 間々田 2009/09/14
				if (IsPreset)
				{
					//            'マルチスキャン用ピッチ
					//            Print #fileNo, GetCsvRec("マルチスキャン用ピッチ", _
					//'                                     "pitch", _
					//'                                     .pitch)
					//            'マルチスキャン用スライス枚数
					//            Print #fileNo, GetCsvRec("マルチスキャン用スライス枚数", _
					//'                                     "multinum", _
					//'                                     .multinum)
					//v17.60 ストリングテーブル化 by長野 2011/06/13
					sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(20112), "pitch", theScansel.pitch.ToString()));

					//マルチスキャン用スライス枚数
					sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(20113), "multinum", theScansel.multinum.ToString()));
				}

                //Wスキャン Rev25.00 追加 by長野 2016/07/07
                //プリセット登録時のみ記憶(-SCでは記録しない)
                if (IsPreset)
                {
                    sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_W_Scan), "w_scan", theScansel.w_scan.ToString()));
                }

				//スキャンモード
				//sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ScanMode)), "scan_mode", theScansel.scan_mode.ToString());
                //Rev20.00 修正 by長野 2014/12/04
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ScanMode), "scan_mode", theScansel.scan_mode.ToString()));

				//マトリクスサイズ
				//sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_Matrix)), "matrix_size", theScansel.matrix_size.ToString());
                //Rev20.00 修正 by長野 2014/12/04
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_Matrix), "matrix_size", theScansel.matrix_size.ToString()));

				//ビュー数
                //Rev20.00 修正 by長野 2014/12/04
				//sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ViewNum)), "scan_view", theScansel.scan_view.ToString());
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ViewNum), "scan_view", theScansel.scan_view.ToString()));

                //テーブル回転 Rev26.00 by chouno 2017/09/01
                sw.WriteLine(modLibrary.GetCsvRec(StringTable.BuildResStr(StringTable.IDS_Rotate, StringTable.IDS_Table),"table_rotation",theScansel.table_rotation.ToString()));

                //FPDゲイン Rev26.00 by chouno 2017/09/01
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(20015),"fpd_gain",theScansel.fpd_gain.ToString()));

                //FPD積分時間 Rev26.00 by chouno 2017/09/01
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(20016),"fpd_integ",theScansel.fpd_integ.ToString()));

				//画像積算枚数
				//sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ImageIntegNum)), "scan_integ_number", theScansel.scan_integ_number.ToString());
                //Rev20.00 修正 by長野 2014/12/04
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ImageIntegNum), "scan_integ_number", theScansel.scan_integ_number.ToString()));

				//スキャンエリア
				//v15.02 画質選択時（プリセット以外）は保存しない・コーン用も追加 by 間々田 2009/09/14
				if (IsPreset)
				{
                    //Rev20.00 修正 by長野 2014/12/04
					//sw.WriteLine( modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ScanArea)), "mscan_area", theScansel.mscan_area.ToString());
                    sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ScanArea), "mscan_area", theScansel.mscan_area.ToString()));
                    //            Print #fileNo, GetCsvRec("コーン用スキャンエリア", _
					//'                                 "cone_scan_area", _
					//'                                 .cone_scan_area)
					//v17.60 ストリングテーブル化 by　長野 2011/06/13
					sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(20114), "cone_scan_area", theScansel.cone_scan_area.ToString()));
                }

				//スライス厚（画素数）
				float swPix = 0;
				swPix = (theScansel.data_mode == (int)ScanSel.DataModeConstants.DataModeCone) ? theScansel.delta_msw : theScansel.mscan_width / theScansel.min_slice_wid;
				//Rev20.00 修正 by長野 2014/12/04
                //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_SliceWidth)), "delta_msw", swPix.ToString("0"));
                //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_SliceWidth), "delta_msw", swPix.ToString("0")));
                //Rev26.30/Rev26.15 change by chouno 2018/10/15
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_SliceWidth), "delta_msw", swPix.ToString("F7")));

				//バイアス
				//sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_Vias)), "mscan_bias", theScansel.mscan_bias.ToString());
                //Rev20.00 修正 by長野 2014/12/04
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_Vias), "mscan_bias", theScansel.mscan_bias.ToString()));

				//スロープ
				//Rev20.00 修正 by長野 2014/12/04
                //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_Slope)), "mscan_slope", theScansel.mscan_slope.ToString());
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_Slope), "mscan_slope", theScansel.mscan_slope.ToString()));

				//フィルタ関数
				//Rev20.00 修正 by長野 2014/12/04
                //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_FilterFunc)), "filter", theScansel.filter.ToString());
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_FilterFunc), "filter", theScansel.filter.ToString()));

				//スキャン中再構成
				//Rev20.00 修正 by長野 2014/12/04
                //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ScanAndView)), "scan_and_view", theScansel.scan_and_view.ToString());
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ScanAndView), "scan_and_view", theScansel.scan_and_view.ToString()));

				//画像方向
				//Rev20.00 修正 by長野 2014/12/04
                //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_Direction)), "image_direction", theScansel.image_direction.ToString());
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_Direction), "image_direction", theScansel.image_direction.ToString()));

				//マルチスライス
				//Rev20.00 修正 by長野 2014/12/04
                //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_MultiSlice)), "multislice", theScansel.multislice.ToString());
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_MultiSlice), "multislice", theScansel.multislice.ToString()));

				//オートズーミング
				//sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_AutoZooming)), "auto_zoomflag", theScansel.auto_zoomflag.ToString());
                //Rev20.00 修正 by長野 2014/12/04
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_AutoZooming), "auto_zoomflag", theScansel.auto_zoomflag.ToString()));

				//オートズームファイルのディレクトリ名
				sw.WriteLine(modLibrary.GetCsvRec(StringTable.BuildResStr(StringTable.IDS_DirNameOf, StringTable.IDS_AutoZoomFile), "autozoom_dir", modLibrary.RemoveNull(theScansel.autozoom_dir.GetString())));
                
				//オートズームファイル名
				sw.WriteLine(modLibrary.GetCsvRec(StringTable.BuildResStr(StringTable.IDS_TableNameOf, StringTable.IDS_AutoZoomFile), "auto_zoom", modLibrary.RemoveNull(theScansel.auto_zoom.GetString())));

				//生データの保存
				sw.WriteLine(modLibrary.GetCsvRec(StringTable.BuildResStr(StringTable.IDS_Save, StringTable.IDS_RawData), "rawdata_save", theScansel.rawdata_save.ToString()));

				//RFC
				//Rev20.00 修正 by長野 2014/12/04
                //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_RFC)), "rfc", theScansel.rfc.ToString());
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_RFC), "rfc", theScansel.rfc.ToString()));

				//オートプリント
				//Rev20.00 修正 by長野 2014/12/04
                //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_AutoPrint)), "auto_print", theScansel.auto_print.ToString());
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_AutoPrint), "auto_print", theScansel.auto_print.ToString()));

				//透視画像の保存
				sw.WriteLine(modLibrary.GetCsvRec(StringTable.BuildResStr(StringTable.IDS_Save, StringTable.IDS_TransImage), "fluoro_image_save", theScansel.fluoro_image_save.ToString()));

				//アーティファクト低減
				//Rev20.00 修正 by長野 2014/12/04
                //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ArtifactReduction)), "artifact_reduction", theScansel.artifact_reduction.ToString());
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ArtifactReduction), "artifact_reduction", theScansel.artifact_reduction.ToString()));

				//再構成形状
				//sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ReconMask)), "recon_mask", theScansel.recon_mask.ToString());
                //Rev20.00 修正 by長野 2014/12/04
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ReconMask), "recon_mask", theScansel.recon_mask.ToString()));

				//画像階調最適化
				//sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ContrastFitting)), "contrast_fitting", theScansel.contrast_fitting.ToString());
                //Rev20.00 修正 by長野 2014/12/04
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ContrastFitting), "contrast_fitting", theScansel.contrast_fitting.ToString()));

				//画像回転角度
				//sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ImageRotAngle)), "image_rotate_angle", theScansel.image_rotate_angle.ToString());
                //Rev20.00 修正 by長野 2014/12/04
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_ImageRotAngle), "image_rotate_angle", theScansel.image_rotate_angle.ToString()));

				//テーブル回転
				sw.WriteLine(modLibrary.GetCsvRec(StringTable.BuildResStr(StringTable.IDS_Rotate, StringTable.IDS_Table), "table_rotation", theScansel.table_rotation.ToString()));

				//'オートセンタリング                                            'v15.01削除 by 間々田 2009/08/28
				//Print #fileNo, GetCsvRec(LoadResString(IDS_AutoCentering), _
				//'                         "auto_centering", _
				//'                         .auto_centering)

				//ビニング
				//sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_Binning)), "binning", theScansel.binning.ToString());
                //Rev20.00 修正 by長野 2014/12/04
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_Binning), "binning", theScansel.binning.ToString()));

				//オーバースキャン
				//Rev20.00 修正 by長野 2014/12/04
                //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_OverScan)), "over_scan", theScansel.over_scan.ToString());
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_OverScan), "over_scan", theScansel.over_scan.ToString()));

				//メール送信
				//sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_SendMail)), "mail_send", theScansel.mail_send.ToString());
                //Rev20.00 修正 by長野 2014/12/04
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_SendMail), "mail_send", theScansel.mail_send.ToString()));

				//画質
				//sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_Quality)), "cone_image_mode", theScansel.cone_image_mode.ToString());
                //Rev20.00 修正 by長野 2014/12/04
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_Quality), "cone_image_mode", theScansel.cone_image_mode.ToString()));

				//スライスピッチ（画素）
				//Rev20.00 by長野 2014/12/04
                //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_SlicePitch)), "delta_z_pix", (theScansel.delta_z / theScansel.min_cone_slice_width).ToString("0.000"));
                //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_SlicePitch), "delta_z_pix", (theScansel.delta_z / theScansel.min_cone_slice_width).ToString("0.000")));
                //Rev26.30/Rev26.15 change by chouno 2018/10/15
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_SlicePitch), "delta_z_pix", ((double)theScansel.delta_z / (double)theScansel.min_cone_slice_width).ToString("F14")));

				//プリセットの場合
				if (IsPreset)
				{
					//スライス枚数
					//Rev20.00 修正 by長野 2014/12/04
                    //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_SliceNumber)), "k", theScansel.k.ToString());
                    sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_SliceNumber), "k", theScansel.k.ToString()));

					//管電圧
					//Rev20.00 修正 by長野 2014/12/04
                    //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_TubeVoltage)), "scan_kv", frmXrayControl.Instance.cwneKV.Text);
                    sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_TubeVoltage), "scan_kv", frmXrayControl.Instance.cwneKV.Text));

                    //変更2014/10/07hata_v19.51反映
                    ////管電流：東芝 EXM2-150の場合は、単位を μ→mA に変換
                    //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_TubeCurrent)), "scan_ma", (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150 ? (frmXrayControl.Instance.cwneMA.Value * 1000).ToString() : frmXrayControl.Instance.cwneMA.Text));
                    //管電流：東芝EXM2-150,Titanの場合は、単位を μ→mA に変換 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    //Rev20.00 修正 by長野 2014/12/04
                    //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_TubeCurrent)), "scan_ma", ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150) | (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) ? (frmXrayControl.Instance.cwneMA.Value * 1000).ToString() : frmXrayControl.Instance.cwneMA.Text));
                    //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_TubeCurrent), "scan_ma", ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150) | (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) ? (frmXrayControl.Instance.cwneMA.Value * 1000).ToString() : frmXrayControl.Instance.cwneMA.Text)));
                    //Rev25.03/Rev25.02 add by chouno 2017/02/05
                    sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_TubeCurrent), "scan_ma", ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150) | (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) | (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman) ? (frmXrayControl.Instance.cwneMA.Value * 1000).ToString() : frmXrayControl.Instance.cwneMA.Text)));

					//FCD（オフセット込み）
					//Rev20.00 修正 by長野 2014/12/04
                    //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_FCD)), "fcd", frmMechaControl.Instance.FCDWithOffset.ToString());
                    sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_FCD), "fcd", frmMechaControl.Instance.FCDWithOffset.ToString()));

					//FID（オフセット込み）
					//Rev20.00 修正 by長野 2014/12/04
                    //sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_FID)), "fid", frmMechaControl.Instance.FIDWithOffset.ToString());
                    sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_FID), "fid", frmMechaControl.Instance.FIDWithOffset.ToString()));
                    
                    //スライスプランテーブルのディレクトリ名                     '追加 by 間々田 2009/08/21
					if (theScansel.multiscan_mode == (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan)
					{
						Item = modLibrary.RemoveNull((theScansel.data_mode == (int)ScanSel.DataModeConstants.DataModeCone) ? theScansel.cone_sliceplan_dir.GetString() : theScansel.sliceplan_dir.GetString());
					}
					else
					{
						Item = "";
					}
					sw.WriteLine(modLibrary.GetCsvRec(StringTable.BuildResStr(StringTable.IDS_DirNameOf, StringTable.IDS_SlicePlanTable), "sliceplan_dir", Item));

					//スライスプランテーブルのテーブル名                         '追加 by 間々田 2009/08/21
					if (theScansel.multiscan_mode == (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan)
					{
                        Item = modLibrary.RemoveNull((theScansel.data_mode == (int)ScanSel.DataModeConstants.DataModeCone) ? theScansel.cone_slice_plan.GetString() : theScansel.slice_plan.GetString());
					}
					else
					{
						Item = "";
					}
					sw.WriteLine(modLibrary.GetCsvRec(StringTable.BuildResStr(StringTable.IDS_TableNameOf, StringTable.IDS_SlicePlanTable), "sliceplan", Item));
				}

			    //v19.00 BHC設定追加 ->(電S2)永井
			    sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(21003), "mbhc_flag", theScansel.mbhc_flag.ToString()));
				sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(21003) + CTResources.LoadResString(12226), "mbhc_dir", modLibrary.RemoveNull(theScansel.mbhc_dir.GetString())));
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(21003) + CTResources.LoadResString(12227), "mbhc_name", modLibrary.RemoveNull(theScansel.mbhc_name.GetString())));
				//<- v19.00

                //Rev26.00 ファントムレスBHC条件追加　by井上 2017/01/18
                sw.WriteLine(modLibrary.GetCsvRec("mbhc_phantomless", "mbhc_phantomless", theScansel.mbhc_phantomless.ToString()));
                //
                
                //Rev26.00 コメント by chouno 2017/08/31
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(12816),"comment",comment));


				//戻り値セット
				functionReturnValue = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			finally
			{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				'ファイルクローズ
				Close #fileNo
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				//ファイルクローズ
				if (sw != null)
				{
					sw.Close();
					sw = null;
				}
			}
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： 画像ファイルからスキャン条件を設定する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static bool LoadSCImageFile(string FileName)
		{
			//modImageInfo.ImageInfoStruct theInfoRec = default(modImageInfo.ImageInfoStruct);
            CTAPI.CTstr.IMAGEINFO theInfoRec = default(CTAPI.CTstr.IMAGEINFO);

			bool IsCone = false;			//v17.61追加 byやまおか 2011/06/29

			//戻り値初期化
			bool functionReturnValue = false;

			//ファイル名チェック

			if (!Regex.IsMatch(FileName.ToLower(), "^.+[.]img$")) return functionReturnValue;

			//v17.60 画像に対応する付帯情報が存在するかチェック by長野 2011/05/31
			//    If FSO.FileExists(FSO.BuildPath(RemoveExtension(FileName, ".img"), "inf")) Then
			//
			//        MsgBox (LoadResString(1308))
			//
			//        Exit Function
			//
			//    End If
			//
			//付帯情報を取得
			if (!ImageInfo.ReadImageInfo(ref theInfoRec, modLibrary.RemoveExtension(FileName, ".img"))) return functionReturnValue;

            //Rev21.00 追加 by長野 2015/03/06
            if (theInfoRec.d_recokind.GetString() == "SCANO")
            {
                MessageBox.Show(CTResources.LoadResString(22005), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return functionReturnValue;
            }

            //Rev26.00 add by chouno 2017/03/13
            //Rev26.10 change by chouno 2017/01/05
            //if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
            //{
            //    return functionReturnValue;
            //}

            //Rev26.10 AVモード用追加 by chouno 
            //回転大テーブル装着、かつ、ロードした付帯情報が未装着なら、大テーブルを装着させるようにする
            if (CTSettings.scaninh.Data.avmode == 0)
            {
                //if (modSeqComm.GetLargeRotTableSts() == 1 && theInfoRec.largetRotTable == 0)
                if ((modSeqComm.GetLargeRotTableSts() == 1 && CTSettings.t20kinf.Data.ftable_type == 0) && theInfoRec.largetRotTable == 0)
                {
                    //MessageBox.Show(CTResources.LoadResString(21363), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Rev26.14 修正 by chouno 2018/09/10
                    MessageBox.Show(CTResources.LoadResString(21366), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return functionReturnValue;
                }
            }
            else if(CTSettings.scaninh.Data.guide_mode == 0)
            {
                //Rev26.10 change by chouno 2017/09/11 
                //Rev26.20 微調テーブルタイプを見る by chouno 2019/02/11
                if (CTSettings.t20kinf.Data.ftable_type == 0)
                {
                    if ((modMechaControl.IsOkMechaMoveWithLargeTable() == false) && theInfoRec.largetRotTable == 1)
                    {
                        MessageBox.Show(CTResources.LoadResString(21363), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return functionReturnValue;
                    }
                }
            }

            if (CTSettings.scaninh.Data.multi_tube == 0)
            {
                //Rev23.10 設定中のX線と付帯情報のX線が異なる場合は処理を中止 by長野 2015/11/04
                if (theInfoRec.multi_tube != (int)mod2ndXray.XrayMode)
                {
                    MessageBox.Show(CTResources.LoadResString(23021), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return functionReturnValue;
                }
            }

            //Rev23.40 by長野 2016/04/05 //Rev23.21 移動先の機構部位置が干渉エリア内＆昇降制限を超える場合は中止 by長野 2016/03/10
            float TargetUdPos = 0.0f;
            float TargetFCD = 0.0f;
            float.TryParse(theInfoRec.d_tablepos.GetString(), out TargetUdPos);
            TargetFCD = theInfoRec.fcd - theInfoRec.fcd_offset;
            if (!modMechaControl.chkTablePosByAutoPos(TargetUdPos, TargetFCD))
            {
                //メッセージの表示：移動後のテーブル昇降位置が、干渉エリア内での制限位置を越えるため、処理を中止します。
                //MsgBox LoadResString(IDS_CorReadyAlready), vbCritical
                MessageBox.Show(CTResources.LoadResString(24100), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                return functionReturnValue;
            }

            //マウスポインタを砂時計にする
			Cursor.Current = Cursors.WaitCursor;

			//まずはX線と検出器を設定する    'v17.10追加 byやまおか 2010/08/26
			//最新 scancel（コモン）取得
			//modScansel.GetScansel(ref modScansel.scansel);
            CTSettings.scansel.Load();

			//スキャン条件のセット
			
			//コーンビームか？   'v17.61追加 byやまおか 2011/06/29
			IsCone = (theInfoRec.bhc == 1);

            ////管電圧     'v17.10 下から移動 byやまおか 2010/08/26
            //float volt = 0;
            //float.TryParse(theInfoRec.volt.GetString(), out volt);
            //modXrayControl.SetVolt(volt);

            ////Titanでもプリセットの場合はcwneKVへ反映する    'v18.00追加 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //frmXrayControl.Instance.cwneKV.Value = Convert.ToDecimal(volt);
            
            //kV/uAを連続設定すると無視される対策
			//modCT30K.PauseForDoEvents(1);			//v17.10追加 byやまおか 2010/09/01

            //Rev25.02 change 焦点設定後に管電圧と管電流設定 by chouno 2017/02/17
            ////管電圧     'v17.10 下から移動 byやまおか 2010/08/26
            //float volt = 0;
            //float.TryParse(theInfoRec.volt.GetString(), out volt);
            //modXrayControl.SetVolt(volt);

            ////Titanでもプリセットの場合はcwneKVへ反映する    'v18.00追加 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //frmXrayControl.Instance.cwneKV.Value = Convert.ToDecimal(volt);

            ////kV/uAを連続設定すると無視される対策
            //modCT30K.PauseForDoEvents(1);			//v17.10追加 byやまおか 2010/09/01

            ////管電流     'v17.10 下から移動 byやまおか 2010/08/26
            //float anpere = 0;
            //float.TryParse(theInfoRec.anpere.GetString(), out anpere);
            //modXrayControl.SetCurrent(anpere);


            //焦点切り替え   'v18.00追加 byやまおか 2011/07/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan))
            //Rev23.10 条件追加 by長野 2015/11/04 
            //if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan)|
            //    (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10711)
            //    )
            //Rev25.03/Rev25.02 add by chouno 2017/02/05
            if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan) |
               (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10711 |
               (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman))
                )
            {
                modCT30K.PauseForDoEvents(1);
                //Clickイベントを発生させる
                //frmXrayControl.Instance.cmdFocus[theInfoRec.xfocus + 1].PerformClick(); //1:大焦点 2:小焦点
                //Rev25.02 change by chouno 2017/02/15
                frmXrayControl.Instance.cmdFocus[theInfoRec.xfocus].PerformClick(); //1:大焦点 2:小焦点
            }

            //rev25.03/Rev25.02 change by chouno 2017/02/15 
            if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman)
            {
                modCT30K.PauseForDoEvents(3);
            }

            //管電圧     'v17.10 下から移動 byやまおか 2010/08/26
            float volt = 0;
            float.TryParse(theInfoRec.volt.GetString(), out volt);
            modXrayControl.SetVolt(volt);

            //Rev25.03/Rev25.02 change by chouno 2017/02/15 
            if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman)
            {
                modCT30K.PauseForDoEvents(1);
            }

            //Titanでもプリセットの場合はcwneKVへ反映する    'v18.00追加 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            frmXrayControl.Instance.cwneKV.Value = Convert.ToDecimal(volt);

            //kV/uAを連続設定すると無視される対策
            modCT30K.PauseForDoEvents(1);			//v17.10追加 byやまおか 2010/09/01

            //管電流     'v17.10 下から移動 byやまおか 2010/08/26
            float anpere = 0;
            float.TryParse(theInfoRec.anpere.GetString(), out anpere);
            modXrayControl.SetCurrent(anpere);

            if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman ||
               modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan)
            {

                modCT30K.PauseForDoEvents(1);
                //Rev24.00 X線制御コントロール更新
                frmXrayControl.Instance.UpdateGeCwneKVMA(volt, anpere);
            }

            //電動フィルタ   'v18.00追加 byやまおか 2011/07/05 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            if (CTSettings.scaninh.Data.shutterfilter == 0)
            {
                //指定のフィルタに切り替える
                //modSeqComm.SeqBitWrite("Filter" + theInfoRec.xfilter.ToString(), true);
                //Rev23.40 変更 by長野 2016/06/19
                modSeqComm.ChangeFilter(theInfoRec.xfilter);
            }

			//PkeFPDの場合   'v17.10追加 byやまおか 2010/08/26
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
			{
				//FPDゲイン/FPD積分時間
				frmScanControl.Instance.SetFpdGainInteg(theInfoRec.fpd_gain, theInfoRec.fpd_integ);
				frmScanControl.Instance.cmbGain.SelectedIndex = theInfoRec.fpd_gain;
				frmScanControl.Instance.cmbInteg.SelectedIndex = theInfoRec.fpd_integ;
			}

			//データモード
			CTSettings.scansel.Data.data_mode = (int)(theInfoRec.bhc == 1 ? ScanSel.DataModeConstants.DataModeCone : ScanSel.DataModeConstants.DataModeScan);

            //Wスキャン 追加 Rev25.00 by長野 2016/07/07
            CTSettings.scansel.Data.w_scan = theInfoRec.w_scan;

			//スキャンモード
			//CTSettings.scansel.Data.scan_mode = modLibrary.GetIndexByStr(modLibrary.RemoveNull(theInfoRec.full_mode), modCommon.MyCtinfdef.full_mode) + 1;
            CTSettings.scansel.Data.scan_mode = modCommon.MyCtinfdef.full_mode.GetIndexByStr(modLibrary.RemoveNull(theInfoRec.full_mode.GetString()), 0) + 1;

			//マトリクスサイズ
			//CTSettings.scansel.Data.matrix_size = modLibrary.GetIndexByStr(modLibrary.RemoveNull(theInfoRec.matsiz), modCommon.MyCtinfdef.matsiz, 2);
            CTSettings.scansel.Data.matrix_size = modCommon.MyCtinfdef.matsiz.GetIndexByStr(modLibrary.RemoveNull(theInfoRec.matsiz.GetString()), 2);

            //Rev20.00 テーブル回転追加 by長野 2015/01/29
            if (theInfoRec.table_rotation == 0)
            {
                CTSettings.scansel.Data.table_rotation = 0;
            }
            else
            {
                CTSettings.scansel.Data.table_rotation = 1;
            }

            //ビュー数
			int.TryParse(theInfoRec.scan_view.GetString(), out CTSettings.scansel.Data.scan_view);

			//画像積算枚数
            int.TryParse(theInfoRec.integ_number.GetString(), out CTSettings.scansel.Data.scan_integ_number);

			//スキャンエリア
			//scansel.mscan_area = .mscan_area

			//v15.02変更 by 間々田 2009/09/14
			if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone)
			{
				CTSettings.scansel.Data.cone_scan_area = theInfoRec.mscan_area;
			}
			else
			{
				CTSettings.scansel.Data.mscan_area = theInfoRec.mscan_area;
			}

			//スライス厚
            float.TryParse(theInfoRec.width.GetString(), out CTSettings.scansel.Data.mscan_width);

			//バイアス設定
			CTSettings.scansel.Data.mscan_bias = Convert.ToSingle(theInfoRec.image_bias);

			//スロープ設定
			CTSettings.scansel.Data.mscan_slope = theInfoRec.image_slope;

			//フィルタ関数設定
			//CTSettings.scansel.Data.filter = modLibrary.GetIndexByStr(modLibrary.RemoveNull(theInfoRec.fc), modCommon.MyCtinfdef.fc) + 1;
            CTSettings.scansel.Data.filter = modCommon.MyCtinfdef.fc.GetIndexByStr(modLibrary.RemoveNull(theInfoRec.fc.GetString()),0) + 1;

			//画像方向設定
			CTSettings.scansel.Data.image_direction = theInfoRec.image_direction;

			//再構成形状
			CTSettings.scansel.Data.recon_mask = theInfoRec.recon_mask;

			//画像回転角度
			CTSettings.scansel.Data.image_rotate_angle = theInfoRec.recon_start_angle;

			//フィルタ処理設定
			//CTSettings.scansel.Data.filter_process = modLibrary.GetIndexByStr(modLibrary.RemoveNull(theInfoRec.filter_process), modCommon.MyCtinfdef.filter_process, 0);
            CTSettings.scansel.Data.filter_process = modCommon.MyCtinfdef.filter_process.GetIndexByStr(modLibrary.RemoveNull(theInfoRec.filter_process.GetString()), 0);

			//RFC処理可否
			CTSettings.scansel.Data.rfc = theInfoRec.rfc;

			//コメント
			CTSettings.scansel.Data.comment = theInfoRec.comment;

			//        'FCD
			//        scansel.FCD = .FCD
			//
			//        'FID
			//        scansel.FCD = .FID

			//v17.10 上へ移動 byやまおか 2010/08/26
			//'管電圧
			//SetVolt Val(.volt)
			//
			//'管電流
			//SetCurrent Val(.anpere)

			//コーンビームフラグ=1の場合
			//If .bhc = 1 Then
			//v17.61変更 byやまおか 2011/06/29
			if ((IsCone))
			{
				//ヘリカルモード：0(非ﾍﾘｶﾙ),1(ﾍﾘｶﾙ)
				CTSettings.scansel.Data.inh = theInfoRec.inh;

				//画質：0(標準),1(精細),2(高速)
				CTSettings.scansel.Data.cone_image_mode = theInfoRec.cone_image_mode;

				//スライス厚(mm)
				CTSettings.scansel.Data.cone_scan_width = Convert.ToSingle(theInfoRec.width.GetString());

				//スライスピッチ(mm)=軸方向Boxelｻｲｽﾞ(mm)
				CTSettings.scansel.Data.delta_z = theInfoRec.delta_z;

				//スライス枚数
				CTSettings.scansel.Data.k = theInfoRec.k;

				//再構成開始位置(mm)
				CTSettings.scansel.Data.zs = theInfoRec.zs0;

				//再構成終了位置(mm)
				CTSettings.scansel.Data.ze = theInfoRec.ze0;

				//画面上のｽﾗｲｽ幅(mm)
				CTSettings.scansel.Data.delta_msw = CTSettings.scansel.Data.cone_scan_width
											* (theInfoRec.fid / theInfoRec.fcd) 
											* (theInfoRec.b1 / 10) 
											* (float)(Math.Sqrt(1 + theInfoRec.scan_posi_a * theInfoRec.scan_posi_a) / (theInfoRec.kv == 0 ? 1 : theInfoRec.kv));
			}

			//I.I.視野を設定
			int ii = 0;
			//ii = modLibrary.GetIndexByStr(modLibrary.RemoveNull(theInfoRec.iifield), modCommon.MyCtinfdef.iifield);
            ii = modCommon.MyCtinfdef.iifield.GetIndexByStr(modLibrary.RemoveNull(theInfoRec.iifield.GetString()),0);

            //modSeqComm.SeqBitWrite(new string[]{ "II9", "II6", "II4" }[ii + 1], true);
            //Rev20.01 修正 by長野 2015/06/02
            modSeqComm.SeqBitWrite(new string[] { "II9", "II6", "II4" }[ii], true);

			//マウスポインタを元に戻す
			Cursor.Current = Cursors.Default;

			//テーブル移動
			float d_tablepos = 0;
			float.TryParse(theInfoRec.d_tablepos.GetString(), out d_tablepos);

            //Rev24.00 フィルタ移動 追加 by長野 2016/05/13
            decimal filter = 0;
            if (theInfoRec.xfilter_c.GetString() == CTSettings.infdef.Data.xfilter_c[0].GetString())
            {
                filter = -9999;
            }
            else if (theInfoRec.xfilter_c.GetString() == CTSettings.infdef.Data.xfilter_c[5].GetString())
            {
                filter = 9999;
            }
            else
            {
                if (!(decimal.TryParse(theInfoRec.xfilter_c.GetString().Replace("mm", ""), out filter)))
                {
                    filter = -1;
                }
            }

            frmMechaMove.Instance.MechaMove((decimal)theInfoRec.ftable_y_pos,
                                            (decimal)theInfoRec.ftable_x_pos,
                                            theInfoRec.fcd - theInfoRec.fcd_offset,
                                            theInfoRec.fid - theInfoRec.fid_offset,
                                            (decimal)theInfoRec.table_x_pos,
                                            (decimal)(theInfoRec.bhc == 1 ? theInfoRec.z0 : d_tablepos));

			//v19.00 BHC設定(電S2)永井
            CTSettings.scansel.Data.mbhc_flag = theInfoRec.mbhc_flag;
            CTSettings.scansel.Data.mbhc_dir = theInfoRec.mbhc_dir;
            CTSettings.scansel.Data.mbhc_name = theInfoRec.mbhc_name;

            //Rev26.00 ファントムレスBHC by 井上 20174/02/10
            CTSettings.scansel.Data.mbhc_phantomless = theInfoRec.mbhc_phantomless;
            CTSettings.scansel.Data.mbhc_phantomless_c = theInfoRec.mbhc_phantomless_c;
            CTSettings.scansel.Data.mbhc_phantomless_colli_on = theInfoRec.mbhc_phantomless_colli_on;

			//scancel（コモン）更新
			//modScansel.PutScansel(ref modScansel.scansel);
            CTSettings.scansel.Write();

			//スキャン条件設定   v15.0変更 by 間々田 2009/06/17
			frmScanCondition.Instance.Setup();

			//v17.61追加 byやまおか 2011/06/29
			if (IsCone)
			{
				//上記Setupでスライス厚がpixで補正されてしまうので、ここで再入力する
				//modScansel.GetScansel(ref modScansel.scansel);
 				//modScansel.scansel.cone_scan_width = Convert.ToSingle(theInfoRec.Width);
				//modScansel.PutScansel(ref modScansel.scansel);
                CTSettings.scansel.Load();
                //CTSettings.scansel.Data.cone_scan_width = Convert.ToSingle(theInfoRec.width);
                
                //Rev20.00 修正 by長野 2014/12/15
                CTSettings.scansel.Data.cone_scan_width = Convert.ToSingle(theInfoRec.width.GetString());
               
                CTSettings.scansel.Write();
                
                frmScanCondition.Instance.Setup();

				//I.I.視野が切り替わったときは警告を出す
                if ((CTSettings.mecainf.Data.ver_iifield != ii) && (CTSettings.detectorParam.DetType == DetectorConstants.DetTypeII))
				{
					//プリセット操作により検出器の視野サイズが変わりました。
					//スライス枚数、スライスピッチ、スライス厚が正しくない可能性があります。
					//幾何歪校正を実施してから、再度、プリセット画像を選び直してください。
					MessageBox.Show(CTResources.LoadResString(15214), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			else
			{  }

			//戻り値セット
			functionReturnValue = true;

//ErrorHandler:

            //Rev26.00 成功時は、完了フラグをONにしておく by chouno 2017/09/01
            frmScanControl.Instance.scanCondImgSetCmpFlg = true;

			//マウスポインタを元に戻す
			Cursor.Current = Cursors.Default;
			return functionReturnValue;
		}


		//*************************************************************************************************
		//機　　能： 指定されたファイルがプリセットファイルか
		//
		//           変数名          [I/O] 型        内容
		//引　　数： FileName        [I/ ] String    チェックするファイル名
		//戻 り 値：                 [ /O] Boolean   True:  プリセットファイルである
		//                                           False: プリセットファイルではない
		//
		//補　　足： なし
		//
		//履　　歴： v15.02 2009/09/14  (SS1)間々田  新規作成
		//*************************************************************************************************
		public static bool IsPresetFile(string FileName)
		{
			//戻り値初期化
			bool functionReturnValue = false;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			'*-SC.CSではない
			If Not (UCase$(FileName) Like "*-SC.CSV") Then Exit Function
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//*-SC.CSではない
			if (!Regex.IsMatch(FileName.ToUpper(), "^.+-SC[.]CSV$")) return functionReturnValue;

            //switch (modFileIO.FSO.GetBaseName(FileName).ToUpper())
            switch (Path.GetFileNameWithoutExtension(FileName).ToUpper())
            {
				//画質を指定するファイル名の場合
				case "QUICK-SC":
				case "ROUGH-SC":
				case "NORMAL-SC":
				case "FINE-SC":
					//SETFILE下のファイルでなければプリセットファイルである
					//functionReturnValue = (modFileIO.FSO.GetParentFolderName(FileName).ToUpper() != modFileIO.pathSetFile.ToUpper());

                    Debug.Print(Path.GetDirectoryName(FileName).ToUpper().ToString());
                    
                    
                    
                    functionReturnValue = (Path.GetDirectoryName(FileName).ToUpper() != AppValue.PathSetFile.ToUpper());
                    
                    
                    
                    
                    break;

				//上記以外
				default:
					//プリセットファイルである
					functionReturnValue = true;
					break;
			}
			return functionReturnValue;
		}

        //*************************************************************************************************
        //機　　能： [ガイド]タブのスキャン条件ファイルの読み込み
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V26.00 2016/12/28 (検S1)長野    新規作成
        //*************************************************************************************************
        public static bool LoadScanCondFile(string FileName)
        {

            string[] strCell = null;
            string strBuf = null;
            bool IsPreset = false;

            double? delta_msw = null;
            double? delta_z_pix = null;
            string sp_table_dir = null;			//追加 2009/08/21 by 間々田
            string sp_table = null;				//追加 2009/08/20 by 間々田
            int? data_mode = null;				//v15.02追加 by 間々田 2009/09/14
            float? mscan_area = null;			//v15.02追加 by 間々田 2009/09/14
            float? cone_scan_area = null;		//v15.02追加 by 間々田 2009/09/14
            int? w_scan = null;                 //Rev25.00 追加 by長野 2016/07/07
            float sw_magnify = 0.0f;
            int pl_mbhc = 0;
            int? K = null;

            //v19.00 BHC設定
            int mbhc_flag = 0;
            string mbhc_dir = null;
            string mbhc_name = null;

            delta_msw = null;			//追加 2009/08/20 by 間々田
            delta_z_pix = null;			//追加 2009/08/20 by 間々田
            sp_table_dir = null;		//追加 2009/08/21 by 間々田
            sp_table = null;			//追加 2009/08/21 by 間々田
            data_mode = null;			//v15.02追加 by 間々田 2009/09/14
            mscan_area = null;			//v15.02追加 by 間々田 2009/09/14
            cone_scan_area = null;		//v15.02追加 by 間々田 2009/09/14
            w_scan = null;              //Rev25.00 追加 by長野 2016/07/07
            K = null;
        
            //戻り値初期化
            bool functionReturnValue = false;

            int? fpd_gain = null;
            int? fpd_integ = null;
            fpd_gain = null;
            fpd_integ = null;

            //ファイル名チェック
            if (!Regex.IsMatch(FileName.ToUpper(), "^.+-SC[.]CSV$")) return functionReturnValue;

            //最新 scancel（コモン）取得
            //modScansel.GetScansel(ref modScansel.scansel);
            CTSettings.scansel.Load();

            StreamReader sr = null;

            //エラー時の扱い
            try
            {
                try
                {
                    //ファイルオープン
                    sr = new StreamReader(FileName, Encoding.GetEncoding("shift-jis"));

                    while ((strBuf = sr.ReadLine()) != null)	//１行読み込み
                    {
                        if (!string.IsNullOrEmpty(strBuf))
                        {
                            //文字列配列に分割
                            strCell = strBuf.Split(',');

                            if (strCell.GetUpperBound(0) >= 2)
                            {
                                int intParseValue = 0;
                                float floatParseValue = 0;
                                double doubleParseValue = 0;

                                switch (strCell[1].Trim().ToLower())
                                {
                                    //FPD積分時間							
                                    case "fpd_integ":
                                        fpd_integ = int.TryParse(strCell[2], out intParseValue) ? intParseValue : fpd_integ;
                                        break;

                                    //FPDゲイン							
                                    case "fpd_gain":
                                        fpd_gain = int.TryParse(strCell[2], out intParseValue) ? intParseValue : fpd_gain;
                                        break;

                                    //データモード（1:スキャン, 4:コーンビーム）'v15.02追加 プリセット時、データモードも反映させる by 間々田 2009/09/14							
                                    case "data_mode":
                                        data_mode = int.TryParse(strCell[2], out intParseValue) ? intParseValue : data_mode;
                                        break;

                                    //マルチスキャンモード（1:シングル, 3:マルチ, 5:スライスプラン）
                                    case "multiscan_mode":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.multiscan_mode);
                                        break;

                                    //マルチスキャン用ピッチ                     'v15.02追加 プリセット時マルチスキャンのピッチを保存する by 間々田 2009/09/14
                                    case "pitch":
                                        if (IsPreset)
                                        {
                                            float.TryParse(strCell[2], out CTSettings.scansel.Data.pitch);
                                        }
                                        break;

                                    //マルチスキャン用スライス枚数               'v15.02追加 プリセット時マルチスキャンのスライス枚数を保存する by 間々田 2009/09/14
                                    case "multinum":
                                        if (IsPreset)
                                        {
                                            int.TryParse(strCell[2], out CTSettings.scansel.Data.multinum);
                                        }
                                        break;

                                    //スキャンモード（1:フル, 2:ハーフ, 3:オフセット）
                                    case "scan_mode":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.scan_mode);
                                        break;

                                    //Wスキャン Rev25.00 追加 by長野 2016/07/07
                                    case "w_scan":
                                        w_scan = int.TryParse(strCell[2], out intParseValue) ? intParseValue : w_scan;
                                        break;

                                    //マトリクスサイズ（1:256×256, 2:512×512, 3:1024×1024, 3:2048×2048, 4:4096×4096) 'v16.10 コメントに4:を追加 by 長野 10/01/29
                                    case "matrix_size":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.matrix_size);
                                        break;

                                    //ビュー数
                                    case "scan_view":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.scan_view);
                                        break;

                                    //画像積算枚数
                                    case "scan_integ_number":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.scan_integ_number);
                                        break;

                                    //スキャンエリア
                                    case "mscan_area":
                                        if (IsPreset)	//v15.02 if部分のみ追加 画質選択時（プリセット以外）はCSVに含まれていても無視する by 間々田 2009/09/14
                                        {
                                            mscan_area = float.TryParse(strCell[2], out floatParseValue) ? floatParseValue : mscan_area;
                                        }
                                        break;

                                    //コーンビーム用スキャンエリア           'v15.02追加 プリセット時、コーンビーム用スキャンエリアをロードする by 間々田 2009/09/14
                                    case "cone_scan_area":
                                        if (IsPreset)
                                        {
                                            cone_scan_area = float.TryParse(strCell[2], out floatParseValue) ? floatParseValue : cone_scan_area;
                                        }
                                        break;

                                    //Rev26.15 スライス厚追加 by chouno 2018/10/15
                                    //スライス厚（画素数, コモン設定時はmmに変更する）
                                    case "delta_msw":
                                        //delta_msw = double.TryParse(strCell[2], out doubleParseValue) ? doubleParseValue : delta_msw;
                                        //Rev20.00 変更 by長野 2015/01/26
                                        if (double.TryParse(strCell[2], out doubleParseValue))
                                        {
                                            delta_msw = doubleParseValue;
                                        }
                                        break;

                                    //バイアス
                                    case "mscan_bias":
                                        float.TryParse(strCell[2], out CTSettings.scansel.Data.mscan_bias);
                                        break;

                                    //スロープ
                                    case "mscan_slope":
                                        float.TryParse(strCell[2], out CTSettings.scansel.Data.mscan_slope);
                                        break;

                                    //フィルタ関数（1:Laks, 2:shepp, 3:Sharpen）
                                    case "filter":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.filter);
                                        break;

                                    //スキャン中再構成（0:しない, 1:する）
                                    case "scan_and_view":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.scan_and_view);
                                        break;

                                    //画像方向（0:上から見た画像, 1:下から見た画像）
                                    case "image_direction":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.image_direction);
                                        break;

                                    //マルチスライス（0:1スライス, 1:3スライス, 2:5スライス）
                                    case "multislice":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.multislice);
                                        break;

                                    //オートズーミング（0:なし, 1:あり）
                                    case "auto_zoomflag":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.auto_zoomflag);
                                        break;

                                    //オートズームファイルのディレクトリ
                                    case "autozoom_dir":
                                        //modLibrary.SetField(strCell[2].Trim(), ref CTSettings.scansel.Data.autozoom_dir);
                                        CTSettings.scansel.Data.autozoom_dir.SetString(strCell[2].Trim());
                                        break;

                                    //オートズームファイル名
                                    case "auto_zoom":
                                        //modLibrary.SetField(strCell[2].Trim(), ref CTSettings.scansel.Data.auto_zoom);
                                        CTSettings.scansel.Data.auto_zoom.SetString(strCell[2].Trim());

                                        break;

                                    //生データ保存（0:なし, 1:あり）
                                    case "rawdata_save":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.rawdata_save);
                                        break;

                                    //rfc（0:なし, 1:弱, 2:中, 3:強）
                                    case "rfc":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.rfc);
                                        break;

                                    //オートプリント（0:なし, 1:あり）
                                    case "auto_print":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.auto_print);
                                        break;

                                    //透視画像保存（0:なし, 1:あり）
                                    case "fluoro_image_save":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.fluoro_image_save);
                                        break;

                                    //アーティファクト低減（0:なし, 1:あり）
                                    case "artifact_reduction":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.artifact_reduction);
                                        break;

                                    //再構成形状（0:正方形, 1:円）
                                    case "recon_mask":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.recon_mask);
                                        break;

                                    //画像階調最適化（0:なし, 1:あり）
                                    case "contrast_fitting":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.contrast_fitting);
                                        break;

                                    //画像回転角度
                                    case "image_rotate_angle":
                                        float.TryParse(strCell[2], out CTSettings.scansel.Data.image_rotate_angle);
                                        break;

                                    //テーブル回転（0:ステップ送り, 1:連続回転）
                                    case "table_rotation":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.table_rotation);
                                        break;

                                    //'オートセンタリング（0:なし, 1:あり）      'v15.01削除 by 間々田 2009/08/28
                                    //Case "auto_centering"
                                    //    .auto_centering = Val(strCell(2))

                                    //ビニング（0:1x1, 1:2x2, 2:4x4）
                                    case "binning":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.binning);
                                        break;

                                    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                                    //                        'オーバースキャン（0:なし, 1:あり）
                                    //                        Case "over_scan"
                                    //                            .over_scan = Val(strCell(2))
                                    //
                                    //                        'メール送信（0:なし, 1:あり）
                                    //                        Case "mail_send"
                                    //                            .mail_send = Val(strCell(2))
                                    //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                                    //画質（0:標準, 1:精細, 2:標準&高速, 3:精細&高速）
                                    case "cone_image_mode":
                                        int.TryParse(strCell[2], out CTSettings.scansel.Data.cone_image_mode);
                                        break;

                                    //スライスピッチ（画素数, コモン設定時はmmに変更する）
                                    case "delta_z_pix":
                                        //double.TryParse(strCell[2], out doubleParseValue);
                                        //Rev26.15 修正 by chouno 2018/10/15
                                        if (double.TryParse(strCell[2], out doubleParseValue))
                                        {
                                            delta_z_pix = doubleParseValue;
                                        }
                                        break;

                                    //スライス枚数
                                    case "k":
                                        //int.TryParse(strCell[2], out CTSettings.scansel.Data.k);
                                        if(int.TryParse(strCell[2], out intParseValue))
                                        {
                                            K = intParseValue; //Rev26.00 2017/02/18
                                        }
                                        break;

                                    //スライスプランテーブルのディレクトリ名 '追加 by 間々田 2009/08/21
                                    case "sliceplan_dir":
                                        sp_table_dir = strCell[2].Trim();
                                        break;

                                    //スライスプランテーブルのテーブル名     '追加 by 間々田 2009/08/21
                                    case "sliceplan":
                                        sp_table = strCell[2].Trim();
                                        break;

                                    //v19.00 BHC設定追加 ->(電S2)永井
                                    case "mbhc_flag":
                                        mbhc_flag = int.TryParse(strCell[2], out intParseValue) ? intParseValue : mbhc_flag;
                                        break;

                                    case "mbhc_dir":
                                        mbhc_dir = strCell[2].Trim();
                                        break;

                                    case "mbhc_name":
                                        mbhc_name = strCell[2].Trim();
                                        break;
                                    //<- v19.00

                                    case "pl_mbhc": //ファントムレスBHC
                                        int.TryParse(strCell[2], out pl_mbhc);
                                        break;

                                    case "sw_magnify": //スライス厚係数(ピッチに対する)
                                        float.TryParse(strCell[2], out sw_magnify);
                                        break;

                                }
                            }
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    //ファイルクローズ
                    if (sr != null)
                    {
                        sr.Close();
                        sr = null;
                    }
                }

                //データモードが指定されていれば                                         'v15.02追加ここから by 間々田 2009/09/14
                if (data_mode != null)
                {
                    CTSettings.scansel.Data.data_mode = (int)data_mode;

                    //スキャンエリアが指定されていれば
                    if ((mscan_area != null) && (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScan))
                    {
                        CTSettings.scansel.Data.mscan_area = (float)mscan_area;
                    }

                    //コーンビーム用エリアが指定されていれば
                    if ((cone_scan_area != null) && (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone))
                    {
                        CTSettings.scansel.Data.cone_scan_area = (float)cone_scan_area;
                    }
                }
                //データモードが不明
                else
                {
                    //スキャンエリアが指定されていれば
                    if (mscan_area != null)
                    {
                        CTSettings.scansel.Data.mscan_area = (float)mscan_area;
                        CTSettings.scansel.Data.cone_scan_area = (float)mscan_area;			//コーンビーム側にも入れておく
                    }

                    //コーンビーム用エリアが指定されていれば
                    if (cone_scan_area != null)
                    {
                        CTSettings.scansel.Data.cone_scan_area = (float)cone_scan_area;
                    }
                }																		//v15.02追加ここまで by 間々田 2009/09/14

                //Rev25.00 Wスキャン 追加 by長野 2016/07/07
                if (w_scan != null)
                {
                    CTSettings.scansel.Data.w_scan = (int)w_scan;
                }

                //マトリクスの調整
                var optdatamode1 = frmScanControl.Instance.optDataMode1;
                var optdatamode4 = frmScanControl.Instance.optDataMode4;

                //if ((frmScanControl.Instance.optDataMode[(int)ScanSel.DataModeConstants.DataModeScan].Checked &&
                if ((optdatamode1.Checked &&
                     (CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize256)))
                {
                    //メッセージ表示：
                    //   シングルスキャンではマトリクスサイズ=256には対応していません。
                    //   強制的にマトリクスサイズ=512に変更します。
                    //MessageBox.Show(StringTable.GetResString(15202, frmScanControl.Instance.optDataMode[(int)ScanSel.DataModeConstants.DataModeScan].Text, "256", "512"),
                    MessageBox.Show(StringTable.GetResString(15202, optdatamode1.Text, "256", "512"),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    CTSettings.scansel.Data.matrix_size = (int)ScanSel.MatrixSizeConstants.MatrixSize512;
                }
                else if (optdatamode4.Checked &&
                    //((CTSettings.scaninh.Data.cone_matrix[3] == 1) || (CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize2048)))
                    //Rev20.02 修正 by長野 2015/06/25
                        ((CTSettings.scaninh.Data.cone_matrix[3] == 1) && (CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize2048)))
                {
                    //メッセージ表示：
                    //コーンビームスキャンではマトリクスサイズ=2048以上には対応していません。
                    //強制的にマトリクスサイズ=1024に変更します。
                    MessageBox.Show(StringTable.GetResString(15212, optdatamode4.Text, "2048", "1024"),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    CTSettings.scansel.Data.matrix_size = (int)ScanSel.MatrixSizeConstants.MatrixSize1024;
                }
                else if (optdatamode4.Checked &&
                    //((CTSettings.scaninh.Data.cone_matrix[4] == 1) || (CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize4096)))
                    //Rev20.02 修正 by長野 2015/6/25 
                        ((CTSettings.scaninh.Data.cone_matrix[4] == 1) && (CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize4096)))
                {
                    //メッセージ表示：
                    //コーンビームスキャンではマトリクスサイズ=4096以上には対応していません。
                    //強制的にマトリクスサイズ=1024に変更します。
                    MessageBox.Show(StringTable.GetResString(15212, optdatamode4.Text, "4096", "1024"),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    CTSettings.scansel.Data.matrix_size = (int)ScanSel.MatrixSizeConstants.MatrixSize1024;
                }

                //v19.00 BHC設定(電S2)永井
                CTSettings.scansel.Data.mbhc_flag = mbhc_flag;
                //CTSettings.scansel.Data.mbhc_dir = mbhc_dir;
                //CTSettings.scansel.Data.mbhc_dir.SetString((string.IsNullOrEmpty(mbhc_dir) ? "" : mbhc_dir));

                ////CTSettings.scansel.Data.mbhc_name = mbhc_name;
                //CTSettings.scansel.Data.mbhc_name.SetString((string.IsNullOrEmpty(mbhc_name) ? "" : mbhc_name));

                if (CTSettings.scansel.Data.mbhc_flag == 1)
                {
                    //Rev26.00 change by chouno 2017/02/08
                    if (!string.IsNullOrEmpty(mbhc_dir))
                    {
                        CTSettings.scansel.Data.mbhc_dir.SetString(mbhc_dir);
                    }

                    if (!string.IsNullOrEmpty(mbhc_name))
                    {
                        CTSettings.scansel.Data.mbhc_name.SetString(mbhc_name);
                    }


                    if (!string.IsNullOrEmpty(CTSettings.scansel.Data.mbhc_dir.GetString()))
                    {
                        if (CTSettings.scansel.Data.mbhc_dir.GetString().Substring(0, 1) != "\\")
                        {
                            //CTSettings.scansel.Data.mbhc_dir = CTSettings.scansel.Data.mbhc_dir.GetString() + "\\";
                            CTSettings.scansel.Data.mbhc_dir.SetString(CTSettings.scansel.Data.mbhc_dir.GetString() + "\\");
                        }
                    }
                }

                CTSettings.scansel.Data.mbhc_phantomless = pl_mbhc;

                CTSettings.scansel.Data.auto_centering = 1;

                CTSettings.scansel.Write();

                //FPD積分時間、ゲイン設定
                if (fpd_gain != null && fpd_integ != null)
                {
                    frmScanControl.Instance.SetFpdGainInteg((int)fpd_gain, (int)fpd_integ);
                    frmScanControl.Instance.cmbGain.SelectedIndex = (int)fpd_gain;
                    frmScanControl.Instance.cmbInteg.SelectedIndex = (int)fpd_integ;
                }

                //実行前にスキャンエリア（概算）更新
                frmMechaControl.Instance.UpdateScanarea();

                //frmScanCondition.Setup , , scancondpar.klimit   'Q/R/N/Fを選んだ時はKを最大値にする 2009/07/25
                //frmScanCondition.Instance.Setup(K: CTSettings.scancondpar.Data.klimit, delta_z_pix: delta_msw, delta_z_pix: delta_z_pix);	//変更 2009/08/20 by 間々田
                //frmScanCondition.Instance.Setup(false, null, K, (double)0.001, (double)0.001);	//変更 2009/08/20 by 間々田

                //Rev26.00 ボクセルピッチをXYZで近づけたいので、概算の1画素ピッチをスライスピッチとする。
                float TargetSLPitch = frmMechaControl.Instance.flblPixSize;

                decimal TargetSLPitchPix = (decimal)TargetSLPitch / (decimal)(Math.Round(CTSettings.scansel.Data.fcd / CTSettings.scansel.Data.fid * CTSettings.scancondpar.Data.dpm, 3));		//v17.64追加 v17.63は反映漏れ byやまおか 2011/10/21
               
                //最初に枚数変更
                if (K != null)
                {
                    //はじめにスライス枚数優先でスキャン条件設定
                    frmScanCondition.Instance.Setup(false, null, (int)K, (double)0.001, (double)0.001);	//変更 2009/08/20 by 間々田
                    //frmScanCondition.Instance.Setup(false, null,  delta_z_pix: 1.0f, scanMat: true, sw_magnify: sw_magnify);	//変更 2009/08/20 by 間々田
                    //Rev26.00 上記は枚数優先かつスライスピッチを1画素として、高さ方向を全領域使用
                    //以下は、枚数優先かつスライスピッチをCT画像1画素に近づけて、高さ方向を全領域使用
                    frmScanCondition.Instance.Setup(false, null, delta_z_pix: (double)TargetSLPitchPix, scanCond: true, sw_magnify: sw_magnify);	//変更 2009/08/20 by 間々田
                }
                else
                {
                    frmScanCondition.Instance.Setup(false, true, CTSettings.scansel.Data.k, null, delta_z_pix: 1.0f, scanCond: true, sw_magnify: sw_magnify);	//変更 2009/08/20 by 間々田
                }
              
                frmScanControl.Instance.scanCondSetCmpFlg = true;

                //ここまで

                //戻り値セット
                functionReturnValue = true;
                return functionReturnValue;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //    MsgBox Err.Description, vbExclamation
            }
            return functionReturnValue;
        }

        //*************************************************************************************************
        //機　　能： スキャン準備
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： 成功：0
        //           失敗：対応するエラーコード
        //補　　足： スキャン条件を基に、スキャン時のメモリ確保等を行います。
        //           コモンの値を使うため、必ず撮影スレッド起動直前で実行して下さい。
        //
        //履　　歴： v20.00  2014/09/11 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static int ScanPreparation()
        {
            TransImageControl transImageCtrl; //Rev20.00 追加 by長野 2014/09/11
            transImageCtrl = CTSettings.transImageControl; //Rev20.00 追加 by長野 2014/09/11

            //Rev20.00 初めにスキャンソフト側で更新されたscanselとscanconparを読み込んでおく by長野 2014/09/11
            CTSettings.scansel.Load();
            CTSettings.scancondpar.Load();
            //Rev23.12 reconinfのLoad追加 by長野 2015/12/28
            CTSettings.reconinf.Load();

            int sts = 0;

            //Rev20.00 スキャン用にフレームレートをここで取得する() by長野 2014/09/11
            //v19.10 PKEとその他の検出器でframerate取得方法を変える 2012/09/06 by長野
            StringBuilder dummy = new StringBuilder(256);
            if (CTSettings.scancondpar.Data.detector == 2)
            {
                if (modDeclare.GetPrivateProfileString("Timings", "Timing_" + Convert.ToString(frmScanControl.Instance.cmbInteg.SelectedIndex), "", dummy, (uint)dummy.Capacity, AppValue.XisIniFileName) > 0)
                {
                    double dummyValue = 0;
                    double.TryParse(dummy.ToString(), out dummyValue);
                    transImageCtrl.Detector.FrameRateForScan = 1000f / (float)(dummyValue / 1000f);
                }
            }
            else
            {
                transImageCtrl.Detector.FrameRateForScan = frmTransImage.Instance.GetCurrentFR();
            }

            //
            //Scan撮影動作
            //
            int view = CTSettings.scansel.Data.scan_view;
            double delta_fai_m = 2 * Math.PI / view; //（pai =(double)3.141592654）

            int js = modScanCondition.ScanJs;
            int je = modScanCondition.ScanJe;

            //Rev20.00 オートセンタリングが必要かどうかのフラグ（ハーフ用) by長野 2015/01/24
            int HalfNoAutoCenteringFlg = modScanCondition.HalfNoAutoCenteringFlg;

            int m_alpha = 0;		//(O) ｵｰﾊﾞｰﾗｯﾌﾟα°分のﾋﾞｭｰ数
            uint cone_acq_view = 0;			//(O) 実際に収集するﾋﾞｭｰ数
            int cob_view = 0;		//(O) つの生ﾃﾞｰﾀﾌｧｲﾙに入る基本view数(最後のﾌｧｲﾙは余りが入る)
            int cob_num = 0;        //(O) 生ﾃﾞｰﾀ分割数
            int mainch = 0;
            int md = 0;
            int Itgnum = CTSettings.scansel.Data.scan_integ_number; ;
            int table_rotation = CTSettings.scansel.Data.table_rotation;
            int thinning_frame_num = CTSettings.scancondpar.Data.thinning_frame_num;
            //Rev20.00 追加 by長野 2015/02/09
            int saveFluoroImage = CTSettings.scansel.Data.fluoro_image_save;

            //追加 シングルScanの撮影に必要な変数 ここから by長野 2014/08/27 ////////////////////////////////////////////////////
            float[] SPA;                        //スキャン位置校正用の変数
            float[] SPB;                        //スキャン位置校正用の変数
            float[] SPB_sft;                    //スキャン位置校正用(シフト)の変数 Rev23.10 by長野 2015/10/06
            float[] Delta_Ysw;                  //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            float[] Delta_Ysw_dash;             //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            float[] Delta_Ysw_2dash;            //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            SPA = new float[5];
            SPB = new float[5];
            SPB_sft = new float[5];             //スキャン位置校正用(シフト)の変数 Rev23.10 by長野 2015/10/06
            Delta_Ysw = new float[5];
            Delta_Ysw_dash = new float[5];
            Delta_Ysw_2dash = new float[5];
            int vs = 0;                         //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            int ve = 0;                         //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            int vs_sft = 0;                     //ﾗｲﾝﾃﾞｰﾀ化に必要な変数シフト用 Rev23.10 by長野 2015/10/06                 
            int ve_sft = 0;                     //ﾗｲﾝﾃﾞｰﾀ化に必要な変数シフト用 Rev23.10 by長野 2015/10/06
            int ud_direction = 0;               //マルチスキャン時のテーブル移動方向
            int maxviewnum = 0;                 //ビュー数の計算に使用する変数
            uint max_mov = 0;                   //ビュー数の計算に使用する変数
            int dummy_js = 0;                   //Getmsetup_acq_view用のダミー変数
            int dummy_je = 0;                   //Getmsetup_acq_view用のダミー変数
            int dummy_m_alpha = 0;              //Getmsetup_acq_view用のダミー変数
            int dummy_mycob_view = 0;           //Getmsetup_acq_view用のダミー変数
            int dummy_cob_num = 0;              //Getmsetup_acq_view用のダミー変数
            int mal_view = 0;                   //ビュー数の計算に使用する変数
            int edata = 0;                      //ビュー数の計算に使用する変数
            int tmpedata = 0;                   //ビュー数の計算に使用する変数
            //追加 シングルScanの撮影に必要な変数 ここまで by長野 2014/08/18 ////////////////////////////////////////////////////
            
            //Rev20.00 スライスプラン回数を 

            //rev23.10 シフトスキャンに必要な変数
            int sft_mainch = 0;//シフト用mainch
            int cal_mainch = 0;//計算にしよう一時的なmainch
            int det_sft_pix = 0;//移動量(画素)
            //生データサイズの横方向の画素数を計算 by長野 2014/08/18
            long tmp_scancondpar_mainch = 0;
            long line_size = 0;
            
            //Rev23.10 シフトスキャン対応 シフトスキャン関係の計算を、ここで行う by長野 2015/10/06
            tmp_scancondpar_mainch = CTSettings.scancondpar.Data.mainch[0] / (long)CTSettings.scancondpar.Data.h_mag[CTSettings.scansel.Data.binning];
            //Rev23.10 シフト用mainchを計算 by長野 2015/10/05
            sft_mainch = (int)tmp_scancondpar_mainch + CTSettings.scancondpar.Data.det_sft_pix;
            mainch =(int)tmp_scancondpar_mainch;
            //if (CTSettings.scansel.Data.scan_mode == 4)
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
            if (CTSettings.scansel.Data.scan_mode == 4 || CTSettings.scansel.Data.w_scan == 1)
            {
                cal_mainch = sft_mainch;
                det_sft_pix = CTSettings.scancondpar.Data.det_sft_pix;
                modCommon.sftpar_set();//シフト用パラメータの計算
            }
            else
            {
                cal_mainch = mainch;
            }

            //Rev25.01 追加 by長野 2016/12/16
            //Rev25.14 修正 by chouno 2017/12/01
            if (CTSettings.scansel.Data.scan_mode == 4 || CTSettings.scansel.Data.w_scan == 1)
            {
                ScanCorrect.calShiftImageMagVal();
            }

            //Rev23.12 //純生データ保存処理追加 by長野 2015/12/11--->
            string tmpName = "";
            string purRawDataBaseName = "";
            if (CTSettings.scansel.Data.pur_rawdata_save == 1)
            {
                if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone)
                {
                    tmpName = CTSettings.scansel.Data.pro_code.GetString() + CTSettings.reconinf.Data.raw_name.GetString();
                    purRawDataBaseName = tmpName.Substring(0, tmpName.Length - 5) + "-";
                }
                else if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScan)
                {
                    purRawDataBaseName = CTSettings.scansel.Data.pro_code.GetString() + CTSettings.reconinf.Data.raw_name.GetString() + "-";
                }
                else if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScano)
                {
                    purRawDataBaseName = CTSettings.scansel.Data.pro_code.GetString() + CTSettings.reconinf.Data.raw_name.GetString() + "-";
                }
            }
            //<---

            //コーンとシングルで分岐 by長野 2014/08/18
            if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone)
            {
                //Scan条件の取得
                modCommon.Getmsetup_acq_view(
                    view,			    //(I) 再構成ﾋﾞｭｰ数
                    delta_fai_m,	    //(I) X線管の1ビュー当たりの回転角(radian)
                    js,	                //(I) 純生データの縦開始位置
                    je,	                //(I) 純生データの縦終了位置
                    ref m_alpha,		//(O) ｵｰﾊﾞｰﾗｯﾌﾟα°分のﾋﾞｭｰ数
                    ref cone_acq_view,			//(O) 実際に収集するﾋﾞｭｰ数
                    ref cob_view,		//(O) つの生ﾃﾞｰﾀﾌｧｲﾙに入る基本view数(最後のﾌｧｲﾙは余りが入る)
                    ref cob_num,        //(O) 生ﾃﾞｰﾀ分割数
                    //ref mainch,
                    ref cal_mainch,     //Rev23.10 シフトスキャン対応 by長野 2015/10/06
                    ref md
                );

                //スキャン撮影用メモリの設定
                //int sts = CTSettings.transImageControl.CaptureScanMemset(
                sts = CTSettings.transImageControl.CaptureScanMemset(
                    mainch,                 // チャンネル
                    sft_mainch,             // Rev23.10 シフト用mainch by長野 2015/10/06
                    CTSettings.scansel.Data.multiscan_mode,//Rev26.00/Rev23.32/25.02 add by chouno 2017/02/13 //Rev26.10 修正 by chouno 2018/01/13
                    CTSettings.scansel.Data.scan_mode, //Rev23.10 スキャンモード追加 by長野 2015/10/05
                    //CTSettings.scansel.Data.multiscan_mode,//Rev26.00/Rev23.32/25.02 add by chouno 2017/02/13
                    CTSettings.scansel.Data.w_scan,    //Rev25.00 Wスキャン 追加 by長野 2016/007/07
                    md,                     // 　
                    js,                     //
                    je,                     //
                    cob_num,                // 分割数
                    (int)cone_acq_view,     // データ取り込みを行うビュー数
                    cob_view,               // ビュー数（360°あたり）
                    Itgnum,                 // 積算枚数
                    view,					// ビュー数（360°あたり）
                    modDeclare.hDevID1,		// メカ制御ボードハンドル
                    modDeclare.mrc,         // メカ制御ボードエラーステータス            
                    table_rotation,         // テーブル回転モード 0:ステップ 1:連続
                    thinning_frame_num,     // 透視画像表示時の間引きフレーム数
                    CTSettings.iniValue.SharedMemSize,   // スキャン・リトライに使用するデータ用メモリサイズ //Rev20.00 追加 by長野 2014/09/11
                    saveFluoroImage,        // 透視画像を保存するかどうかのフラグ 追加 by長野 2015/02/09
                    det_sft_pix,             // 移動量(画素) Rev23.10 by長野 2015/10/06 
                    CTSettings.scansel.Data.pur_rawdata_save,//Rev23.12 純生データ保存 by長野 2015/12/11
                    purRawDataBaseName,                       //Rev23.12 純生データ ベースファイル名 by長野 2015/12/1
                    ScanCorrect.ShiftFImageMagVal,   //Rev25.00 追加 by長野 2016/09/24
                    ScanCorrect.ShiftFImageMagValL,
                    ScanCorrect.ShiftFImageMagValR
                    );
            }
            else if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScan)//Rev21.00 条件式追加 by長野 2015/02/19
            //シングル
            {
                //Scan条件の取得
                modCommon.Getmsetup_acq_view(
                    view,			        //(I) 再構成ﾋﾞｭｰ数
                    delta_fai_m,	        //(I) X線管の1ビュー当たりの回転角(radian)
                    dummy_js,	            //(I) 純生データの縦開始位置
                    dummy_je,	            //(I) 純生データの縦終了位置
                    ref dummy_m_alpha,		//(O) ｵｰﾊﾞｰﾗｯﾌﾟα°分のﾋﾞｭｰ数
                    ref max_mov,			//(O) 実際に収集するﾋﾞｭｰ数
                    ref dummy_mycob_view,	//(O) つの生ﾃﾞｰﾀﾌｧｲﾙに入る基本view数(最後のﾌｧｲﾙは余りが入る)
                    ref dummy_cob_num,      //(O) 生ﾃﾞｰﾀ分割数
                    //ref mainch,
                    ref cal_mainch,     //Rev23.10 シフトスキャン対応 by長野 2015/10/06
                    ref md
                );

                maxviewnum = view;

                if (CTSettings.scansel.Data.scan_mode == 1)	// ﾊｰﾌｽｷｬﾝ
                {
                    tmpedata = (int)max_mov;
                    edata = (int)max_mov - 1;
                }
                else// ﾉｰﾏﾙ or ｵﾌｾｯﾄ
                {
                    tmpedata = (int)max_mov;
                    edata = (int)max_mov - 1;
                }

                if (maxviewnum > tmpedata)	// ﾊｰﾌｽｷｬﾝ
                {
                    mal_view = maxviewnum;
                }
                else// ﾉｰﾏﾙ or ｵﾌｾｯﾄ
                {
                    mal_view = tmpedata;
                }

                if (CTSettings.scansel.Data.multiscan_mode == 3)
                {
                    if (CTSettings.scansel.Data.pitch >= (float)0.0)
                    {
                        ud_direction = 0;
                    }
                    else
                    {
                        ud_direction = 1;
                    }
                }

                //ここから Scanav.slnの中のmct_paracalと同じ処理を行い、シングルスキャンに必要な変数を得る by長野 2014/08/18
                long siz;	//H_SIZEが640の時siz=1、1300の時siz=2   Rev3.00追加　　//REV7.04 v_mag / h_mag に変更

                SPA[0] = CTSettings.scancondpar.Data.scan_posi_a[0];
                SPA[1] = CTSettings.scancondpar.Data.scan_posi_a[1];
                SPA[2] = CTSettings.scancondpar.Data.scan_posi_a[2];
                SPA[3] = CTSettings.scancondpar.Data.scan_posi_a[3];
                SPA[4] = CTSettings.scancondpar.Data.scan_posi_a[4];
                SPB[0] = CTSettings.scancondpar.Data.scan_posi_b[0];
                SPB[1] = CTSettings.scancondpar.Data.scan_posi_b[1];
                SPB[2] = CTSettings.scancondpar.Data.scan_posi_b[2];
                SPB[3] = CTSettings.scancondpar.Data.scan_posi_b[3];
                SPB[4] = CTSettings.scancondpar.Data.scan_posi_b[4];

                //Rev18.00 ｼﾌﾄｽｷｬﾝのときはｼﾌﾄ位置でﾃﾞｰﾀ収集する切片(基準位置よりもマイナス方向になる)を求める 11-03-21 by IWASAWA //v19.50 v19.41とv18.02の統合 by長野 2013/11/01 
                //Rev23.20 左右シフト対応 by長野 2015/11/20
                //if (CTSettings.scansel.Data.scan_mode == 4)
                //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                //if (CTSettings.scansel.Data.scan_mode == 4 && CTSettings.scaninh.Data.lr_sft == 1)
                
                //Rev25.00 Wスキャンを追加 by長野 2016/07/07
                if (CTSettings.W_ScanOn && (CTSettings.scansel.Data.w_scan == 1))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        SPB_sft[i] = SPB[i] - ((float)SPA[i] * (float)CTSettings.scancondpar.Data.det_sft_pix);
                    }
                }
                else
                {
                    if (CTSettings.scansel.Data.scan_mode == 4 && CTSettings.scaninh.Data.lr_sft == 1)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            //SPB_sft[i] = SPB[i] - SPA[i] * CTSettings.scancondpar.Data.det_sft_pix;
                            //Rev23.20 キャスト追加 by長野 2015/11/20
                            SPB_sft[i] = SPB[i] - ((float)SPA[i] * (float)CTSettings.scancondpar.Data.det_sft_pix);
                        }
                    }
                    else if (CTSettings.scansel.Data.scan_mode == 4 && CTSettings.scaninh.Data.lr_sft == 0)
                    {
                        float[] tmpSPB = new float[5];
                        for (int i = 0; i < 5; i++)
                        {
                            tmpSPB[i] = SPB[i] + ((float)SPA[i] * (float)CTSettings.scancondpar.Data.det_sft_pix_l);
                            SPB_sft[i] = tmpSPB[i] - ((float)SPA[i] * (float)CTSettings.scancondpar.Data.det_sft_pix);

                        }
                    }
                }

                Itgnum = CTSettings.scansel.Data.scan_integ_number;		// by iwa
          
                siz = (long)(CTSettings.scancondpar.Data.v_mag[CTSettings.scansel.Data.binning] / CTSettings.scancondpar.Data.h_mag[CTSettings.scansel.Data.binning]);
                //Delta_Ysw[5], Delta_Ysw_dash[5], Delta_Ysw_2dash[5] を求める	by iwa
                for (long i = 0; i < 5; i++)
                {
                    //if (FCDX > 0) 
                    if (CTSettings.scansel.Data.fcd > 0)	//v11.5変更 by 間々田 2006/07/19
                    {
                        //	Delta_Ysw[i] = SW * FIDX * A[i][1] / FCDX / (float)10 / (float)vmag ;
                        //Delta_Ysw[i] = SW * FIDX * A[i][1] / FCDX / (float)10 / (float)siz ;		//Rev3.00変更
                        //Delta_Ysw[i] = CTSettings.scansel.Data.mscan_width * CTSettings.scansel.Data.fid * CTSettings.scancondpar.Data.a[i * 6] / CTSettings.scansel.Data.fcd / (float)10 / (float)siz;		//v11.5変更 by 間々田 2006/07/19
                        //Rev20.00 修正 by長野 2014/12/04
                        Delta_Ysw[i] = CTSettings.scansel.Data.mscan_width * CTSettings.scansel.Data.fid * CTSettings.scancondpar.Data.a[i * 6 + 1] / CTSettings.scansel.Data.fcd / (float)10 / (float)siz;		//v11.5変更 by 間々田 2006/07/19
                    }
                    else
                    {
                        Delta_Ysw[i] = 1;
                    }
                    if (Delta_Ysw[i] <= 1)
                    {
                        Delta_Ysw[i] = 1;
                    }
                    Delta_Ysw_dash[i] = (float)(Delta_Ysw[i] / (float)2 - (float)0.5);
                    Delta_Ysw_2dash[i] = (float)(Delta_Ysw_dash[i] + (float)1);
                }

                //Rev23.10 シフトスキャン対応 by長野 2015/10/06
                //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                if (CTSettings.scansel.Data.scan_mode == 4 || CTSettings.scansel.Data.w_scan == 1)
                {
                    //Scanav.slnの中のScan_Divpara_Setと同じ処理を行い、シングルスキャンに必要な変数を得る by長野 2014/08/18
                    sts = modCommon.GetScan_Divpara_Set(
                        CTSettings.detectorParam.h_size,	//透視画像横サイズ
                        CTSettings.detectorParam.v_size,	//透視画像縦サイズ
                        ref SPA,					        //スキャン位置を表す１次直線の傾き
                        ref SPB_sft,					        //スキャン位置を表す１次直線の切片
                        ref Delta_Ysw,                      //画像上のスライス厚
                        ref vs_sft,						    //積算除算処理開始y座標(シフト)
                        ref ve_sft,						    //積算除算処理終了y座標(シフト)
                        0,					                //縦方向の歪分				 
                        CTSettings.scansel.Data.multislice);						//複数同時スライス数 0:1枚 1:3枚 2:5枚
                    //
                }

                //Scanav.slnの中のScan_Divpara_Setと同じ処理を行い、シングルスキャンに必要な変数を得る by長野 2014/08/18
                sts = modCommon.GetScan_Divpara_Set(
                    CTSettings.detectorParam.h_size,	//透視画像横サイズ
                    CTSettings.detectorParam.v_size,	//透視画像縦サイズ
                    ref SPA,					        //スキャン位置を表す１次直線の傾き
                    ref SPB,					        //スキャン位置を表す１次直線の切片
                    ref Delta_Ysw,                      //画像上のスライス厚
                    ref vs,						        //積算除算処理開始y座標
                    ref ve,						        //積算除算処理終了y座標
                    0,					                //縦方向の歪分				 
                    CTSettings.scansel.Data.multislice);						//複数同時スライス数 0:1枚 1:3枚 2:5枚
                //
       
                ////生データサイズの横方向の画素数を計算 by長野 2014/08/18
                //long tmp_scancondpar_mainch = 0;
                //long line_size = 0;
                //tmp_scancondpar_mainch = CTSettings.scancondpar.Data.mainch[0] / (long)CTSettings.scancondpar.Data.h_mag[CTSettings.scansel.Data.binning];
                ////Rev23.10 シフト用mainchを計算 by長野 2015/10/05
                //sft_mainch = tmp_scancondpar_mainch + CTSettings.scancondpar.Data.det_sft_pix;

                //Rev23.10 シフトの条件を追加 by長野 2015/10/05
                //if (CTSettings.scansel.Data.scan_mode == 4)//シフトスキャン
                //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                if (CTSettings.scansel.Data.scan_mode == 4)//シフトスキャン
                {
                    line_size = sft_mainch * 2;
                }
                else if (CTSettings.scansel.Data.scan_mode == 3)//オフセットスキャン
                {
                    //Rev25.00 Wスキャンを条件に追加 by長野 2016/08/08
                    //line_size = tmp_scancondpar_mainch * 2;
                    line_size = CTSettings.scansel.Data.w_scan == 1? sft_mainch * 2 : tmp_scancondpar_mainch * 2;
                }
                else
                {
                    //Rev25.00 Wスキャンを条件に追加 by長野 2016/08/08
                    line_size = CTSettings.scansel.Data.w_scan == 1 ? sft_mainch : tmp_scancondpar_mainch;
                    //line_size = tmp_scancondpar_mainch;
                }

                //Rev17.10 1024のとき引き伸ばし0になるとFFT後の画像の端がおかしくなるため変更(100画素は伸ばす) 10-09-15 by IWASAWA
                //パーキンエルマーがぴったりの値になってしまう
                //if (line_size <= (1024 - 100))
                //    line_size = 1024;
                //else if ((1024 - 100) < line_size && line_size <= (2048 - 100))
                //    line_size = 2048;
                //else if ((2048 - 100) < line_size && line_size <= (4096 - 100))
                //    line_size = 4096;
                //else if ((4096 - 100) < line_size && line_size <= 8192)	//Rev18.00 8192FFT追加 11-02-16 by IWASAWA //v19.50 v19.41とv18.02の統合 by長野 2013/11/01 
                //    line_size = 8192;
                //Rev17.10 END
                //Rev25.00 100→300 変更 by長野 2016/08/18
                if (line_size <= (1024 - 300))
                    line_size = 1024;
                else if ((1024 - 300) < line_size && line_size <= (2048 - 300))
                    line_size = 2048;
                else if ((2048 - 300) < line_size && line_size <= (4096 - 300))
                    line_size = 4096;
                else if ((4096 - 300) < line_size && line_size <= 8192) 
                    line_size = 8192;

                //スキャン撮影用メモリの設定
                sts = CTSettings.transImageControl.CaptureSingleScanMemset(
                    mal_view,                           // ビュー数
                    tmpedata,                           // 実際に取り込むビュー数
                    line_size,
                    CTSettings.scancondpar.Data.h_size,    //透視画像横サイズ
                    CTSettings.scancondpar.Data.v_size,      //投資画像縦サイズ
                    CTSettings.scansel.Data.multislice,
                    mainch,                 // チャンネル
                    (int)sft_mainch,        // シフト用mainch Rev23.10 追加 by長野 2015/10/05
                    CTSettings.scansel.Data.scan_mode, //Rev23.10 スキャンモード追加 by長野 2015/10/05
                    CTSettings.scansel.Data.multiscan_mode, //Rev25.03 add by chouno 2017/02/16
                    CTSettings.scansel.Data.w_scan,    //Rev25.00 Wスキャン 追加 by長野 2016/07/07
                    Itgnum,                 // 積算枚数
                    modDeclare.hDevID1,		// メカ制御ボードハンドル
                    modDeclare.mrc,			// メカ制御ボードエラーステータス            
                    table_rotation,         // テーブル回転モード 0:ステップ 1:連続
                    thinning_frame_num,      // 透視画像表示時の間引きフレーム数
                    Delta_Ysw,
                    Delta_Ysw_dash,
                    Delta_Ysw_2dash,
                    SPA,
                    SPB,
                    SPB_sft,                 //Rev23.10 シフト用SPB追加 by長野 2015/10/06
                    (int)vs,
                    (int)ve,
                    (int)vs_sft,             //Rev23.10 シフト用SPB追加 by長野 2015/10/06
                    (int)ve_sft,             //Rev23.10 シフト用SPB追加 by長野 2015/10/06
                    ud_direction,
                    HalfNoAutoCenteringFlg,//Rev20.00 追加 by長野 2015/01/24
                    saveFluoroImage,         // 透視画像を保存するかどうかのフラグ 追加 by長野 2015/02/09
                    CTSettings.scansel.Data.pur_rawdata_save,//Rev23.12 純生データ保存 by長野 2015/12/11
                    purRawDataBaseName,                       //Rev23.12 純生データ ベースファイル名 by長野 2015/12/1
                    ScanCorrect.ShiftFImageMagVal,   //REv25.00 左右のシフト画像の輝度値調整用係数 by長野 2016/09/24
                    ScanCorrect.ShiftFImageMagValL,
                    ScanCorrect.ShiftFImageMagValR
                    );
            }
            else if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScano) //Rev21.00 スキャノ追加 by長野 2015/03/08
            {
                int mscano_widthPix = 0;
                mscano_widthPix = (int)(CTSettings.scansel.Data.mscano_width / CTSettings.scansel.Data.min_slice_wid) + 1;

                //Rev21.00 束ねたデータ作成は、シングルのラインデータプログラムを使う。(傾き0、切片0)
                //ここから Scanav.slnの中のmct_paracalと同じ処理を行い、シングルスキャンに必要な変数を得る by長野 2014/08/18
                long siz;	//H_SIZEが640の時siz=1、1300の時siz=2   Rev3.00追加　　//REV7.04 v_mag / h_mag に変更

                SPA[0] = 0;
                SPA[1] = 0;
                SPA[2] = 0;
                SPA[3] = 0;
                SPA[4] = 0;
                SPB[0] = 0;
                SPB[1] = 0;
                SPB[2] = 0;
                SPB[3] = 0;
                SPB[4] = 0;
                //Rev18.00 ｼﾌﾄｽｷｬﾝのときはｼﾌﾄ位置でﾃﾞｰﾀ収集する切片(基準位置よりもマイナス方向になる)を求める 11-03-21 by IWASAWA //v19.50 v19.41とv18.02の統合 by長野 2013/11/01
                //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                //if (CTSettings.scansel.Data.scan_mode == 4)
                if (CTSettings.scansel.Data.scan_mode == 4 || CTSettings.scansel.Data.w_scan == 1)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        SPB_sft[i] = SPB[i] - SPA[i] * CTSettings.scancondpar.Data.det_sft_pix;
                    }
                }

                Itgnum = CTSettings.scansel.Data.mscano_integ_number;

                siz = (long)(CTSettings.scancondpar.Data.v_mag[CTSettings.scansel.Data.binning] / CTSettings.scancondpar.Data.h_mag[CTSettings.scansel.Data.binning]);
                //Delta_Ysw[5], Delta_Ysw_dash[5], Delta_Ysw_2dash[5] を求める	by iwa
                for (long i = 0; i < 5; i++)
                {
                    //if (FCDX > 0) 
                    if (CTSettings.scansel.Data.fcd > 0)	//v11.5変更 by 間々田 2006/07/19
                    {
                        //	Delta_Ysw[i] = SW * FIDX * A[i][1] / FCDX / (float)10 / (float)vmag ;
                        //Delta_Ysw[i] = SW * FIDX * A[i][1] / FCDX / (float)10 / (float)siz ;		//Rev3.00変更
                        //Delta_Ysw[i] = CTSettings.scansel.Data.mscan_width * CTSettings.scansel.Data.fid * CTSettings.scancondpar.Data.a[i * 6] / CTSettings.scansel.Data.fcd / (float)10 / (float)siz;		//v11.5変更 by 間々田 2006/07/19
                        //Rev20.00 修正 by長野 2014/12/04
                        Delta_Ysw[i] = CTSettings.scansel.Data.mscano_width * CTSettings.scansel.Data.fid * CTSettings.scancondpar.Data.a[i * 6 + 1] / CTSettings.scansel.Data.fcd / (float)10 / (float)siz;		//v11.5変更 by 間々田 2006/07/19
                    }
                    else
                    {
                        Delta_Ysw[i] = 1;
                    }
                    if (Delta_Ysw[i] <= 1)
                    {
                        Delta_Ysw[i] = 1;
                    }
                    Delta_Ysw_dash[i] = (float)(Delta_Ysw[i] / (float)2 - (float)0.5);
                    Delta_Ysw_2dash[i] = (float)(Delta_Ysw_dash[i] + (float)1);
                }

                //Scanav.slnの中のScan_Divpara_Setと同じ処理を行い、シングルスキャンに必要な変数を得る by長野 2014/08/18
                sts = modCommon.GetScan_Divpara_Set(
                    CTSettings.detectorParam.h_size,	//透視画像横サイズ
                    CTSettings.detectorParam.v_size,	//透視画像縦サイズ
                    ref SPA,					        //スキャン位置を表す１次直線の傾き
                    ref SPB,					        //スキャン位置を表す１次直線の切片
                    ref Delta_Ysw,                      //画像上のスライス厚
                    ref vs,						        //積算除算処理開始y座標
                    ref ve,						        //積算除算処理終了y座標
                    0,					                //縦方向の歪分				 
                    0);						            //複数同時スライス数 0:1枚 1:3枚 2:5枚
                //

                //スキャノ撮影用メモリの設定
                sts = CTSettings.transImageControl.CaptureScanoMemset(
                    CTSettings.scansel.Data.mscanopt,             // 撮影回数
                    CTSettings.scansel.Data.mscano_real_mscanopt, // 撮影回数(最小スライスピッチ時)
                    CTSettings.scansel.Data.mscano_integ_number,  // 積算枚数
                    CTSettings.scansel.Data.mscano_udpitch,       // スキャノ昇降ピッチ（スキャノピッチ）
                    CTSettings.scansel.Data.mscano_width,         // スキャノ厚
                    CTSettings.scansel.Data.min_slice_wid,        // 最小スキャノ厚
                    mscano_widthPix,                              // スキャノ厚(画素)
                    CTSettings.scancondpar.Data.h_size,           // 透視画像横サイズ
                    CTSettings.scancondpar.Data.v_size,           // 透視画像縦サイズ
                    modDeclare.hDevID1,                           // メカ制御ボードハンドル
                    modDeclare.mrc,                               // メカ制御ボードエラーステータス
                    CTSettings.iniValue.SharedMemSize,            // スキャン・リトライに使用するデータ用メモリサイズ //Rev20.00 追加 by長野 2014/09/11
                    CTSettings.t20kinf.Data.ud_type,              // 昇降タイプ
                    Delta_Ysw,
                    Delta_Ysw_dash,
                    Delta_Ysw_2dash,
                    SPA,
                    SPB,
                    (int)vs,
                    (int)ve,
                    CTSettings.scansel.Data.pur_rawdata_save,//Rev23.12 純生データ保存 by長野 2015/12/11
                    purRawDataBaseName                       //Rev23.12 純生データ ベースファイル名 by長野 2015/12/1
                    );
            }
            return (sts);
        }
        //*************************************************************************************************
        //機　　能： スキャン位置取得用(シングル)
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： プロパティ
        //補　　足： 左右シフト対応。スキャンモードにより切片が変わるため。
        //             
        //
        //履　　歴： v23.20  2015/11/19 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static float real_scan_posi_b
        {
            get
            {
                float ret_b = 0.0f;
                //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                //if (ScanCorrect.IsShiftScan() && CTSettings.scaninh.Data.lr_sft == 0)
                if ((ScanCorrect.IsShiftScan() || ScanCorrect.IsW_Scan()) && (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)) 
                {
                    ret_b = CTSettings.scancondpar.Data.scan_posi_b[2] + ((float)CTSettings.scancondpar.Data.scan_posi_a[2] * (float)CTSettings.scancondpar.Data.det_sft_pix_l);
                }
                else
                {
                    ret_b = CTSettings.scancondpar.Data.scan_posi_b[2];
                }
                return ret_b;
            }
        }
        //*************************************************************************************************
        //機　　能： スキャン位置取得用(コーン)
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： プロパティ
        //補　　足： 左右シフト対応。スキャンモードにより切片が変わるため。
        //             
        //
        //履　　歴： v23.20  2015/11/19 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static float real_cone_scan_posi_b
        {
            get
            {
                float ret_b = 0.0f;
                //Rev25.00 Wスキャンを追加 by長野 2016/07/07 
                //if (ScanCorrect.IsShiftScan() && CTSettings.scaninh.Data.lr_sft == 0)
                if ((ScanCorrect.IsShiftScan() || ScanCorrect.IsW_Scan()) && (CTSettings.scaninh.Data.lr_sft == 0 && CTSettings.W_ScanOn)) 
                {
                    ret_b = CTSettings.scancondpar.Data.cone_scan_posi_b - ((float)CTSettings.scancondpar.Data.cone_scan_posi_a + (float)CTSettings.scancondpar.Data.det_sft_pix_r);
                }
                else
                {
                    ret_b = CTSettings.scancondpar.Data.cone_scan_posi_b;
                }
                return ret_b;
            }
        }
        //*************************************************************************************************
        //機　　能： [ガイド]タブで選択したスキャンエリアを取得
        //
        //           変数名          [I/O] 型        内容
        //引　　数： 押されたラジオボタンのインデックス
        //戻 り 値： スキャンエリア半径(mm)
        //補　　足： 
        //             
        //
        //履　　歴： v26.00  2016/12/26 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static float getScanAreaForGuide(int Index)
        {
            float ScanArea = 0.0f;

            //システムによって返す値を変更
            if (!CTSettings.detectorParam.Use_FlatPanel)//I.I.
            {
                int IIno = modSeqComm.GetIINo();

                // 9/6/4.5
                if (CTSettings.infdef.Data.iifield[0].GetString().Substring(0) == "9")//1文字目が9なら9inchI.I.
                {
                    switch (IIno)
                    {
                        case 0:
                            ScanArea = CTSettings.iniValue.ScanArea9inchII[Index];
                            break;
                        case 1:
                            ScanArea = CTSettings.iniValue.ScanArea6inchII[Index];
                            break;
                        case 2:
                            ScanArea = CTSettings.iniValue.ScanArea4_5inchII[Index];
                            break;
                        default://ありえない
                            ScanArea = -1.0f;
                            break;
                    }
                }
                // 4/2
                else if (CTSettings.infdef.Data.iifield[0].GetString().Substring(0) == "4")//1文字目が4なら4inchI.I.
                {
                    switch (IIno)
                    {
                        case 0:
                            ScanArea = CTSettings.iniValue.ScanArea4inchII[Index];
                            break;
                        case 2:
                            ScanArea = CTSettings.iniValue.ScanArea2inchII[Index];
                            break;
                        default:
                            ScanArea = -1.0f;
                            break;//ありえない
                    }
                }
            }
            else//FPD
            {
                if (CTSettings.detectorParam.h_size == 2048)//16inch
                {
                    ScanArea = CTSettings.iniValue.ScanArea16inchFPD[Index];
                }
                else
                {
                    ScanArea = CTSettings.iniValue.ScanArea8inchFPD[Index];
                }
            }

            return ScanArea / 2.0f;
        }
        //*************************************************************************************************
        //機　　能： [ガイド]タブで選択したスキャンエリアに対応するスキャンモードとWスキャン有無を取得
        //
        //           変数名          [I/O] 型        内容
        //引　　数： 押されたラジオボタンのインデックス
        //戻 り 値： スキャンモード、Wスキャン有無
        //補　　足： 
        //             
        //
        //履　　歴： v26.00  2016/12/26 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static void getScanModeForScanArea(int Index,ref int ScanModeIndex,ref int WScanFlg)
        {
            ScanModeIndex = -1;
            WScanFlg = -1;

            //システムによって返す値を変更
            if (!CTSettings.detectorParam.Use_FlatPanel)//I.I.
            {
                int IIno = modSeqComm.GetIINo();

                // 9/6/4.5
                if (CTSettings.infdef.Data.iifield[0].GetString().Substring(0) == "9")//1文字目が9なら9inchI.I.
                {
                    switch (IIno)
                    {
                        case 0:
                            ScanModeIndex = CTSettings.iniValue.ScanMode9inchII[Index];
                            WScanFlg = CTSettings.iniValue.WScanMode9inchII[Index];
                            break;
                        case 1:
                            ScanModeIndex = CTSettings.iniValue.ScanMode6inchII[Index];
                            WScanFlg = CTSettings.iniValue.WScanMode6inchII[Index];
                            break;
                        case 2:
                            ScanModeIndex = CTSettings.iniValue.ScanMode4_5inchII[Index];
                            WScanFlg = CTSettings.iniValue.WScanMode4_5inchII[Index];
                            break;
                        default://ありえない
                            ScanModeIndex = -1;
                            WScanFlg = -1;
                            break;
                    }
                }
                // 4/2
                else if (CTSettings.infdef.Data.iifield[0].GetString().Substring(0) == "4")//1文字目が4なら4inchI.I.
                {
                    switch (IIno)
                    {
                        case 0:
                            ScanModeIndex = CTSettings.iniValue.ScanMode4inchII[Index];
                            WScanFlg = CTSettings.iniValue.WScanMode4inchII[Index];
                            break;
                        case 2:
                            ScanModeIndex = CTSettings.iniValue.ScanMode2inchII[Index];
                            WScanFlg = CTSettings.iniValue.WScanMode2inchII[Index];
                            break;
                        default:
                            ScanModeIndex = -1;
                            WScanFlg = -1;
                            break;//ありえない
                    }
                }
            }
            else//FPD
            {
                if (CTSettings.detectorParam.h_size == 2048)//16inch
                {
                    ScanModeIndex = CTSettings.iniValue.ScanMode16inchFPD[Index];
                    WScanFlg = CTSettings.iniValue.WScanMode16inchFPD[Index];
                }
                else
                {
                    ScanModeIndex = CTSettings.iniValue.ScanMode8inchFPD[Index];
                    WScanFlg = CTSettings.iniValue.WScanMode8inchFPD[Index];
                }
            }

            return;
        }
        //*************************************************************************************************
        //機　　能： [ガイド]タブで選択したスキャンエリアに対応する機構部位置(FDDとテーブルZ軸位置)を取得
        //
        //           変数名          [I/O] 型        内容
        //引　　数： 押されたラジオボタンのインデックス
        //戻 り 値： FDD(mm)、Z軸位置(mm)
        //補　　足： 
        //             
        //
        //履　　歴： v26.00  2017/01/18 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static bool getMechaPosForScanArea(int Index, ref float fdd, ref float tableZ)
        {
            bool ret = false;

            try
            {
                //システムによって返す値を変更
                if (!CTSettings.detectorParam.Use_FlatPanel)//I.I.
                {
                    int IIno = modSeqComm.GetIINo();

                    // 9/6/4.5
                    if (CTSettings.infdef.Data.iifield[0].GetString().Substring(0) == "9")//1文字目が9なら9inchI.I.
                    {
                        switch (IIno)
                        {
                            case 0:
                                fdd = CTSettings.iniValue.fdd9inchII[Index];
                                tableZ = CTSettings.iniValue.tableZ9inchII[Index];
                                break;
                            case 1:
                                fdd = CTSettings.iniValue.fdd6inchII[Index];
                                tableZ = CTSettings.iniValue.tableZ6inchII[Index];
                                break;
                            case 2:
                                fdd = CTSettings.iniValue.fdd4_5inchII[Index];
                                tableZ = CTSettings.iniValue.tableZ4_5inchII[Index];
                                break;
                            default://ありえない
                                throw new Exception();
                        }
                    }
                    // 4/2
                    else if (CTSettings.infdef.Data.iifield[0].GetString().Substring(0) == "4")//1文字目が4なら4inchI.I.
                    {
                        switch (IIno)
                        {
                            case 0:
                                fdd = CTSettings.iniValue.fdd4inchII[Index];
                                tableZ = CTSettings.iniValue.tableZ4inchII[Index];
                                break;
                            case 2:
                                fdd = CTSettings.iniValue.fdd2inchII[Index];
                                tableZ = CTSettings.iniValue.tableZ2inchII[Index];
                                break;
                            default://ありえない
                                throw new Exception();
                        }
                    }
                }
                else//FPD
                {
                    if (CTSettings.detectorParam.h_size == 2048)//16inch
                    {
                        fdd = CTSettings.iniValue.fdd16inchFPD[Index];
                        tableZ = CTSettings.iniValue.tableZ16inchFPD[Index];
                    }
                    else
                    {
                        fdd = CTSettings.iniValue.fdd8inchFPD[Index];
                        tableZ = CTSettings.iniValue.tableZ16inchFPD[Index];
                    }
                }
            }
            catch
            {
                ret = false;
                return ret;
            }

            ret = true;

            return ret;
        }
        //*************************************************************************************************
        //機　　能： プリセットファイルリスト取得
        //
        //           変数名          [I/O] 型        内容
        //引　　数： プリセット名、プリセットファイルのリスト
        //戻 り 値： true:成功,false:失敗
        //補　　足： 
        //             
        //
        //履　　歴： v26.00  2017/08/31 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static bool getPresetList(ref List<string> PresetName,ref List<string> PresetPath)
        {
            bool ret = false;

            StreamReader sr = null;
            string buf = "";
            string[] strCell;

            try
            {
                sr = new StreamReader(Path.Combine(AppValue.PathScanCondPresetFile,@"list.csv"), Encoding.GetEncoding("shift-jis"));

                //while (!FileSystem.EOF(fileNo))
                while ((buf = sr.ReadLine()) != null)
                {
                    //１行読み込む
                    if (!string.IsNullOrEmpty(buf))
                    {
                        //カンマで区切って配列に格納
                        strCell = buf.Split(',');

                        if (strCell[0] != "No")//１行目無視
                        {
                            PresetName.Add(strCell[1]);//プリセット名
                            PresetPath.Add(strCell[2]);//プリセットファイルフルパス
                        }
                    }
                }
                
                ret = true;

            }
            catch
            {
                ret = false;
                return ret;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Dispose();
                    sr = null;
                }
            }

            return ret;
        }
        //*************************************************************************************************
        //機　　能： プリセットファイルリスト書き込み
        //
        //           変数名          [I/O] 型        内容
        //引　　数： プリセット名、プリセットファイルのリスト
        //戻 り 値： true:成功,false:失敗
        //補　　足： 
        //             
        //
        //履　　歴： v26.00  2017/08/31 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static bool writePresetList(List<string> PresetName, List<string> PresetPath)
        {
            bool ret = false;
            string buf = null;

            StreamWriter sw = null;

            try
            {
                //sw = new StreamWriter(FileName, false,Encoding.Default);
                sw = new StreamWriter(Path.Combine(AppValue.PathScanCondPresetFile, @"list.csv"), false, Encoding.GetEncoding("shift-jis"));

                //ヘッダの書き込み
                buf = "No,PresetName,PresetPath";
                sw.WriteLine(buf);

                //各リストの書き込み
                int PresetCnt = PresetName.Count;
                for (int cnt = 0; cnt < PresetCnt; cnt++)
                {
                    buf = (cnt + 1).ToString() + "," + PresetName[cnt] + "," + PresetPath[cnt];
                    sw.WriteLine(buf);
                }

                ret = true;

            }
            catch
            {
                ret = false;
                return ret;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Dispose();
                    sw = null;
                }
            }

            return ret;
        }
        //*************************************************************************************************
        //機　　能： プルセットファイルの中から指定の項目を取得
        //
        //           変数名          [I/O] 型        内容
        //引　　数： プリセット名、プリセットファイルのリスト
        //戻 り 値： true:成功,false:失敗
        //補　　足： 
        //             
        //
        //履　　歴： v26.00  2017/08/31 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static bool getPresetFileItem(string FileName,string Target, ref string result)
        {
            bool ret = false;

            StreamReader sr = null;
            string buf = "";
            string[] strCell;

            try
            {
                sr = new StreamReader(FileName, Encoding.GetEncoding("shift-jis"));

                //while (!FileSystem.EOF(fileNo))
                while ((buf = sr.ReadLine()) != null)
                {
                    //１行読み込む
                    if (!string.IsNullOrEmpty(buf))
                    {
                        //カンマで区切って配列に格納
                        strCell = buf.Split(',');

                        if (strCell[1] == Target)//１行目無視
                        {
                            result = strCell[2];
                        }
                    }
                }

                ret = true;

            }
            catch
            {
                ret = false;
                return ret;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Dispose();
                    sr = null;
                }
            }

            return ret;
        }
        //*************************************************************************************************
        //機　　能： プリセットファイルリスト削除
        //           削除後は上詰
        //
        //           変数名          [I/O] 型        内容
        //引　　数： 削除したいプリセット名
        //戻 り 値： true:成功,false:失敗
        //補　　足： 
        //             
        //
        //履　　歴： v26.00  2017/08/31 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static bool deletePresetList(string TargetPresetName)
        {
            bool ret = false;
            int Index = modScanCondition.PresetName.IndexOf(TargetPresetName);

            if (Index >= 0)
            {
                modScanCondition.PresetName.RemoveAt(Index);
                modScanCondition.PresetPath.RemoveAt(Index);
            }

            return ret;
        }
        //*************************************************************************************************
        //機　　能： [ガイド]タブのスキャン条件ファイル名を取得
        //
        //           変数名          [I/O] 型        内容
        //引　　数： 押されたラジオボタンのインデックス
        //戻 り 値： スキャン条件ファイル名
        //補　　足： 
        //             
        //
        //履　　歴： v26.00  2016/12/26 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static string getScanCondFileName(int ScanCondIndex)
        {
            string fileNameFullPass = "";

            int ResoNo = ScanCondIndex / 4;
            int SNNo = ScanCondIndex % 4 + 1;
            string SNStr = SNNo.ToString();
            string ResoStr = "";

            switch (ResoNo)
            {
                case 0:
                    ResoStr = "A";
                    break;
                case 1:
                    ResoStr = "B";
                    break;
                case 2:
                    ResoStr = "C";
                    break;
                case 3:
                    ResoStr = "D";
                    break;
                default:
                    ResoStr = "";
                    break;
            }

            string detSystem = "";

            //システムによって読みだすファイルを変更
            if (!CTSettings.detectorParam.Use_FlatPanel)//I.I.
            {
                int IIno = modSeqComm.GetIINo();

                // 9/6/4.5
                if (CTSettings.infdef.Data.iifield[0].GetString().Substring(0) == "9")//1文字目が9なら9inchI.I.
                {
                    switch (IIno)
                    {
                        case 0:
                            detSystem = "9II";
                            break;
                        case 1:
                            detSystem = "6II";
                            break;
                        case 2:
                            detSystem = "4_5II";
                            break;
                        default://ありえない
                            detSystem = "";
                            break;
                    }
                }
                // 4/2
                else if (CTSettings.infdef.Data.iifield[0].GetString().Substring(0) == "4")//1文字目が4なら4inchI.I.
                {
                    switch (IIno)
                    {
                        case 0:
                            detSystem = "4II";
                            break;
                        case 2:
                            detSystem = "2II";
                            break;
                        default://ありえない
                            detSystem = "";
                            break;
                    }
                }
            }
            else//FPD
            {
                if (CTSettings.detectorParam.h_size == 2048)//16inch
                {
                    detSystem = "16FPD";
                }
                else
                {
                    detSystem = "8FPD";
                }
            }

            string baseDirName = (ScanCondIndex < 16) ? Path.Combine(AppValue.PathScanCondFile, detSystem) : AppValue.PathScanCondPresetFile;

            string fileName = "";
            if(ScanCondIndex < 16)
            {
                fileName = "ScanMat_" + ResoStr + "_" + SNStr + "-SC.csv";
            }
            else
            {
                fileName = PresetName[PresetSelectedIndex] + "-SC.csv"; ;
            }
            //string fileName = CondStr + "-SC.csv";

            fileNameFullPass = Path.Combine(baseDirName, fileName);

            if (!File.Exists(fileNameFullPass))
            {
                fileNameFullPass = "";
            }

            return fileNameFullPass;
        }
   
        //*************************************************************************************************
        //機　　能： 純生データ削除
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： 成功：なし
        //           失敗：なし
        //補　　足： 純生データ保存ありでスキャンし、途中終了した場合は保存済のデータを削除
        //           
        //
        //履　　歴： v23.12  2015/12/11 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static void DeletePurData()
        {
            string DeleteBaseFileName = "";
            string DeleteFileName = "";
            string purExt = ".pur";
            int view = 0;
            int MaxViewCnt = 0;

            string tmpName = "";
            if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone)
            {
                tmpName = CTSettings.scansel.Data.pro_code.GetString() + CTSettings.reconinf.Data.raw_name.GetString();
                DeleteBaseFileName = tmpName.Substring(0, tmpName.Length - 5) + "-";
            }
            else if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScan)
            {
                DeleteBaseFileName = CTSettings.scansel.Data.pro_code.GetString() + CTSettings.reconinf.Data.raw_name.GetString() + "-";
            }
            else if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScano)
            {
                DeleteBaseFileName = CTSettings.scansel.Data.pro_code.GetString() + CTSettings.reconinf.Data.raw_name.GetString() + "-";
            }

            //設定可能な最大ビューまでファイルの有無を確認
            //(シフトの場合は倍まで見る)
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
            //if (CTSettings.scansel.Data.scan_mode == 4)
            if (CTSettings.scansel.Data.scan_mode == 4 || CTSettings.scansel.Data.w_scan == 1)
            {
                MaxViewCnt = CTSettings.GVal_ViewMax * 2;
            }
            else if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScano)
            {
                MaxViewCnt = CTSettings.scansel.Data.mscanopt;
            }
            else
            {
                MaxViewCnt = CTSettings.GVal_ViewMax;
            }

            for (int i = 0; i < MaxViewCnt; i++)
            {
                DeleteFileName = DeleteBaseFileName + i.ToString() + purExt;
                if (File.Exists(DeleteFileName) == true)
                {
                    File.Delete(DeleteFileName);
                }
            }

            return;
        }
        //*************************************************************************************************
        //機　　能： X線減スペクトルデータ存在チェック
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： 成功：0
        //           失敗：0以外
        //補　　足： 
        //           
        //
        //履　　歴： v26.00 2017/04/17 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static int ChkXraySpectrumDataExists(int system_type)
        {
            int ret = 0;

            switch(system_type)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    ret = 1219;
                    break;
                case 7:
                    if (!modLibrary.IsValidFileName2("C:\\CT\\ScanCorrect\\phantomlessBHC", "xray_spectrum_data_L10801_50kV.csv") ||
                    !modLibrary.IsValidFileName2("C:\\CT\\ScanCorrect\\phantomlessBHC", "xray_spectrum_data_L10801_100kV.csv") ||
                    !modLibrary.IsValidFileName2("C:\\CT\\ScanCorrect\\phantomlessBHC", "xray_spectrum_data_L10801_150kV.csv") ||
                    !modLibrary.IsValidFileName2("C:\\CT\\ScanCorrect\\phantomlessBHC", "xray_spectrum_data_L10801_200kV.csv") ||
                    !modLibrary.IsValidFileName2("C:\\CT\\ScanCorrect\\phantomlessBHC", "xray_spectrum_data_L10801_230kV.csv"))
                    {
                        ret = 1219;
                    }
                    break;
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    ret = 1219;
                    break;
                case 13:
                    if (!modLibrary.IsValidFileName2("C:\\CT\\ScanCorrect\\phantomlessBHC", "xray_spectrum_data_L12721_100kV.csv") ||
                     !modLibrary.IsValidFileName2("C:\\CT\\ScanCorrect\\phantomlessBHC", "xray_spectrum_data_L12721_150kV.csv") ||
                     !modLibrary.IsValidFileName2("C:\\CT\\ScanCorrect\\phantomlessBHC", "xray_spectrum_data_L12721_200kV.csv") ||
                     !modLibrary.IsValidFileName2("C:\\CT\\ScanCorrect\\phantomlessBHC", "xray_spectrum_data_L12721_250kV.csv") ||
                     !modLibrary.IsValidFileName2("C:\\CT\\ScanCorrect\\phantomlessBHC", "xray_spectrum_data_L12721_300kV.csv"))
                    {
                        ret = 1219;
                    }
                    break;
                case 14:
                    //Rev26.10 add Spellman用 by chouno 2018/01/13
                    if (!modLibrary.IsValidFileName2("C:\\CT\\ScanCorrect\\phantomlessBHC", "xray_spectrum_data_isovolt_260kV.csv") ||
                     !modLibrary.IsValidFileName2("C:\\CT\\ScanCorrect\\phantomlessBHC", "xray_spectrum_data_isovolt_300kV.csv") ||
                    !modLibrary.IsValidFileName2("C:\\CT\\ScanCorrect\\phantomlessBHC", "xray_spectrum_data_isovolt_400kV.csv"))
                    {
                        ret = 1219;
                    }
                    break;
                case 15: //Rev26.10 add Spellman用 by chouno 2018/01/13
                    if (!modLibrary.IsValidFileName2("C:\\CT\\ScanCorrect\\phantomlessBHC", "xray_spectrum_data_isovolt_260kV.csv") ||
                     !modLibrary.IsValidFileName2("C:\\CT\\ScanCorrect\\phantomlessBHC", "xray_spectrum_data_isovolt_300kV.csv") ||
                     !modLibrary.IsValidFileName2("C:\\CT\\ScanCorrect\\phantomlessBHC", "xray_spectrum_data_isovolt_400kV.csv"))
                    {
                        ret = 1219;
                    }
                    break;

                default:
                    ret = 1219;
                    break;
            }
            return ret;
        }
	}
}
