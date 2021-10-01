using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{

	///* ************************************************************************** */
	///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： frmSliceplan.frm                                            */
	///* 処理概要　　： スライスプラン                                              */
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
	///* V4.0        01/02/22    (ITC)    鈴山　修   ﾒﾆｭｰﾊﾞｰ削除                    */
	///*                                                                            */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
	///* ************************************************************************** */
	public partial class frmSliceplan : Form
	{

		private ListViewItem ClickedItem = null;

		//変更フラグ：テーブル内容に変更有り？
		private bool Changed = false;

		//スライスプランコメント
		private string myComment = "";


		private static frmSliceplan _Instance = null;

		public frmSliceplan()
		{
			InitializeComponent();
		}

		public static frmSliceplan Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmSliceplan();
				}

				return _Instance;
			}
		}


		//*************************************************************************************************
		//機　　能： Target プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v9.0  2007/09/13  (WEB)間々田  新規作成
		//*************************************************************************************************
		private string Target
		{
            //Rev20.00 修正 by長野 2014/12/15
            //get{ return this.Text.Substring((CTResources.LoadResString(StringTable.IDS_SlicePlan) + " - ").Length); }
			get
            {
                int tmplen1 = (CTResources.LoadResString(StringTable.IDS_SlicePlan) + " - ").Length;
                int tmplen2 = this.Text.Length;
                if (tmplen1 <= tmplen2)
                {
                    return this.Text.Substring((CTResources.LoadResString(StringTable.IDS_SlicePlan) + " - ").Length);
                }
                else
                {
                    return "";
                }
            }
			set
			{
				//スライスプランテーブル名をタイトルバーに表示
				this.Text = CTResources.LoadResString(StringTable.IDS_SlicePlan);
				if (!string.IsNullOrEmpty(value)) this.Text = this.Text + " - " + value;

				//スライスプランテーブル変更フラグリセット
				Changed = false;
			}
		}


		//*************************************************************************************************
		//機　　能： 指定したファイル名の付帯情報からスライスプランテーブル用のレコード（文字列）を取得する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v9.0  2007/09/13  (WEB)間々田  新規作成
		//*************************************************************************************************
		private string GetSlicePlanRec(string FileName)
		{
            //modImageInfo.ImageInfoStruct InfRec = default(modImageInfo.ImageInfoStruct);
            ImageInfo InfRec = new ImageInfo();
            InfRec.Data.Initialize();
            
            string buf = null;

			//戻り値初期化
			string functionReturnValue = "";

			//付帯情報取得
			//if (!modImageInfo.ReadImageInfo(ref InfRec, modLibrary.RemoveExtension(FileName, ".img")))
            if (!ImageInfo.ReadImageInfo(ref InfRec.Data, modLibrary.RemoveExtension(FileName, ".img")))
            {
				//メッセージ表示：付帯情報が見つかりません。
				MessageBox.Show(FileName + "\r" + "\r" + StringTable.BuildResStr(StringTable.IDS_NotFound, StringTable.IDS_ImageInfo), 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return functionReturnValue;
			}

			//画像付帯情報をレコードに記録
			buf = modLibrary.AddExtension(Path.GetDirectoryName(FileName), "\\");						//画像ディレクトリ名 末尾に\をつける
			buf = buf + "," + Path.GetFileName(FileName);												//画像ファイル名
			double d_tablepos = 0;
            double.TryParse(InfRec.Data.d_tablepos.GetString(), out d_tablepos);
			buf = buf + "," + d_tablepos.ToString("0.000");												//絶対位置(mm)
			buf = buf + "," + InfRec.Data.mscan_area.ToString("0.000");										//スキャンエリア
			double Width = 0;
            double.TryParse(InfRec.Data.width.GetString(), out Width);
			buf = buf + "," + Width.ToString("0.000");													//スライス厚(mm)
			double scan_view = 0;
            double.TryParse(InfRec.Data.scan_view.GetString(), out scan_view);
			buf = buf + "," + scan_view.ToString("0");													//ビュー数
			double integ_number = 0;
            double.TryParse(InfRec.Data.integ_number.GetString(), out integ_number);
			buf = buf + "," + integ_number.ToString("0");												//積算枚数
			double matsiz = 0;
            double.TryParse(InfRec.Data.matsiz.GetString(), out matsiz);
			buf = buf + "," + matsiz.ToString("0");														//マトリクスサイズ
            buf = buf + "," + Convert.ToString(InfRec.Data.zooming);											//ズーミング
            buf = buf + "," + (InfRec.Data.zooming == 1 ? modLibrary.RemoveNull(InfRec.Data.zoom_dir.GetString()) : "");		//ズーミングテーブルディレクトリ
            buf = buf + "," + (InfRec.Data.zooming == 1 ? modLibrary.RemoveNull(InfRec.Data.zoom_name.GetString()) : "");		//ズーミングテーブル
            buf = buf + "," + Convert.ToString(InfRec.Data.data_mode);										//データモード
            buf = buf + "," + InfRec.Data.z0.ToString("0.000");												//コーンテーブル位置
            buf = buf + "," + Convert.ToString(InfRec.Data.cone_image_mode);									//コーン画質
            buf = buf + "," + InfRec.Data.delta_z.ToString("0.000");											//コーンスライスピッチ
            buf = buf + "," + Convert.ToString(InfRec.Data.k);												//コーンスライス枚数
            buf = buf + "," + Convert.ToString(InfRec.Data.fine_table_x);									//微調テーブルＸ有無
            buf = buf + "," + InfRec.Data.ftable_x_pos.ToString("0.00");										//微調テーブルＸ座標
            buf = buf + "," + Convert.ToString(InfRec.Data.fine_table_y);									//微調テーブルＹ有無
            buf = buf + "," + InfRec.Data.ftable_y_pos.ToString("0.00");										//微調テーブルＹ座標
            buf = buf + "," + InfRec.Data.table_rotation.ToString("0");                                     //Rev20.00 テーブル回転追加 by長野 2015/01/29

			//戻り値セット
			functionReturnValue = buf;
			return functionReturnValue;
		}


		//*************************************************************************************************
		//機　　能： スライスプランテーブルの保存
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private bool TrySaveSlicePlanTable()
		{
			int i = 0;
			string FileName = null;			//スライスプランテーブルのファイル名（＊－ＳＰＬ．ＣＳＶ）

			//戻り値初期化
			bool functionReturnValue = false;

			//画面のデータモードがコンビームでリストにコーンビームの画像が含まれている場合
			if (optConeBeam.Checked)
			{
				for (i = 0; i < lvwTable.Items.Count; i++)
				{
					if (lvwTable.Items[i].SubItems["Cone"].Text != "*")
					{
						//メッセージ表示：
						//   コーンビーム用スライスプランテーブルにコーンビーム以外の画像ファイルを含めることはできません。
						MessageBox.Show(CTResources.LoadResString(9540), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return functionReturnValue;
					}
				}
			}

			string SubExtensionStr = null;
			SubExtensionStr = (optConeBeam.Checked ? "-CSP" : "-SPL");

			string InitFileName = null;
			InitFileName = Target;
			InitFileName = modLibrary.RemoveExtension(InitFileName, "-spl.csv");
			InitFileName = modLibrary.RemoveExtension(InitFileName, "-csp.csv");

			//保存ファイル名指定ダイアログ
			FileName = modFileIO.GetFileName(StringTable.IDS_Save, CTResources.LoadResString(StringTable.IDS_SlicePlanTable), SubExtension: SubExtensionStr, InitFileName: InitFileName);
			if (string.IsNullOrEmpty(FileName)) return functionReturnValue;

			//ファイルの書き込み
			functionReturnValue = SaveSlicePlanTable(FileName);
			return functionReturnValue;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
FileError:

			'エラーメッセージ表示
			MsgBox Error(Err.Number), vbCritical

			return functionReturnValue;
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

        }


		//*************************************************************************************************
		//機　　能： スライスプランテーブルを保存する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private bool SaveSlicePlanTable(string FileName)
		{
			int i = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Dim fileNo          As Integer  'ファイル番号
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//string theSlicePlanRec = null;
			int Count = 0;

			//戻り値初期化
			bool functionReturnValue = false;

			//エラー時の設定
			try
			{
				StreamWriter sw = null;

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
					//sw = new StreamWriter(FileName);
                    //Rev20.00 shift-jisでエンコード by長野 2014/12/15
                    sw = new StreamWriter(FileName, false, System.Text.Encoding.GetEncoding("shift-jis"));

					//ヘッダを書き込む
					//スライス番号,
					//パラメータ名,
					//画像ﾃﾞｨﾚｸﾄﾘ名,
					//画像ﾌｧｲﾙ名,
					//絶対位置(mm),
					//ｽｷｬﾝｴﾘｱ,
					//ｽﾗｲｽ厚(mm),
					//ﾋﾞｭｰ数,
					//積算枚数,
					//ﾏﾄﾘｸｽｻｲｽﾞ,
					//ｽﾞｰﾐﾝｸﾞ有無,
					//ｽﾞｰﾐﾝｸﾞﾃｰﾌﾞﾙﾃﾞｨﾚｸﾄﾘ名,
					//ｽﾞｰﾐﾝｸﾞﾃｰﾌﾞﾙ名,
					//データモード,
					//コーンテーブル位置,
					//コーン画質,
					//コーンスライスピッチ,
					//コーンスライス枚数,
					//微調テーブルＸ有無,
					//微調テーブルＸ座標,
					//微調テーブルＹ有無,
					//微調テーブルＹ座標
					//Print #fileNo, LoadResString(12293)                    'v7.0 リソース対応 by 間々田 2003/08/25

					//v14.24変更 メカ軸の表示にコモンを使用することにした 2009/03/10 → リソース化はあとで
					//    Print #fileNo, GetCsvRec("スライス番号", _
					//'                             LoadResString(IDS_ParameterName), _
					//'                             "画像ﾃﾞｨﾚｸﾄﾘ名", _
					//'                             "画像ﾌｧｲﾙ名", _
					//'                             "絶対位置(mm)", _
					//'                             "ｽｷｬﾝｴﾘｱ", _
					//'                             "ｽﾗｲｽ厚(mm)", _
					//'                             "ﾋﾞｭｰ数", _
					//'                             "積算枚数", _
					//'                             "ﾏﾄﾘｸｽｻｲｽﾞ", _
					//'                             "ｽﾞｰﾐﾝｸﾞ有無", _
					//'                             "ｽﾞｰﾐﾝｸﾞﾃｰﾌﾞﾙﾃﾞｨﾚｸﾄﾘ名", _
					//'                             "ｽﾞｰﾐﾝｸﾞﾃｰﾌﾞﾙ名", _
					//'                             "ﾃﾞｰﾀﾓｰﾄﾞ", _
					//'                             "ｺｰﾝﾃｰﾌﾞﾙ位置", _
					//'                             "ｺｰﾝ画質", _
					//'                             "ｺｰﾝｽﾗｲｽﾋﾟｯﾁ", _
					//'                             "ｺｰﾝｽﾗｲｽ枚数", _
					//'                             GetResString(12124, RemoveNull(infdef.m_axis_name(1))), _
					//'                             GetResString(12125, RemoveNull(infdef.m_axis_name(1))), _
					//'                             GetResString(12124, RemoveNull(infdef.m_axis_name(0))), _
					//'                             GetResString(12125, RemoveNull(infdef.m_axis_name(0))))
					//
					//ストリングテーブル化 'v17.60 by長野 2011/05/22
					sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(12918),
                                                      CTResources.LoadResString(StringTable.IDS_ParameterName),
                                                      CTResources.LoadResString(20079),
                                                      CTResources.LoadResString(20080),
                                                      CTResources.LoadResString(20081),
                                                      CTResources.LoadResString(12029),
                                                      CTResources.LoadResString(12812),
                                                      CTResources.LoadResString(12808),
                                                      CTResources.LoadResString(12809),
                                                      CTResources.LoadResString(12814),
                                                      CTResources.LoadResString(20082),
                                                      CTResources.LoadResString(20083),
                                                      CTResources.LoadResString(20084),
                                                      CTResources.LoadResString(9014),
                                                      CTResources.LoadResString(20085),
                                                      CTResources.LoadResString(20086),
                                                      CTResources.LoadResString(20087),
                                                      CTResources.LoadResString(20088),
                                                      StringTable.GetResString(12124, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[1].GetString())),
                                                      StringTable.GetResString(12125, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[1].GetString())),
                                                      StringTable.GetResString(12124, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[0].GetString())),
                                                      StringTable.GetResString(12125, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[0].GetString())),
                                                      StringTable.BuildResStr(StringTable.IDS_Rotate, StringTable.IDS_Table) //Rev20.00 テーブル回転追加 by長野 2015/01/29
                                                      ));

					Count = 0;
					string rec = null;

					for (i = 0; i < lvwTable.Items.Count; i++)
					{
						//指定したファイル名の付帯情報からスライスプランテーブル用のレコード（文字列）を取得する
						rec = GetSlicePlanRec(lvwTable.Items[i].SubItems["CTImage"].Text);
						if (!string.IsNullOrEmpty(rec))
						{
							//ファイルに書き込む
							Count = Count + 1;
							sw.WriteLine(modLibrary.GetCsvRec(Count.ToString(), StringTable.FormatStr("sliceplan[%1]", Count - 1), rec));
						}
					}


					//フッタを書き込む
                    sw.WriteLine(CTResources.LoadResString(StringTable.IDS_SliceNum) + "," + Convert.ToString(Count));		//スライス数
                    sw.WriteLine(CTResources.LoadResString(StringTable.IDS_Comment) + "," + myComment);						//コメント
				}
				catch
				{
					throw;
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

				//編集中スライスプランテーブル名の設定
				Target = FileName;

				//戻り値セット（成功）
				functionReturnValue = true;
			}
			catch (Exception ex)
			{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				'エラーメッセージ表示
				MsgBox Err.Description, vbCritical
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				//エラーメッセージ表示
				MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return functionReturnValue;
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
		private void cmdExit_Click(object sender, EventArgs e)
		{
			//テーブル内容に変更箇所があるか？
			if (Changed && (lvwTable.Items.Count > 0))
			{
				//保存の確認：現在の内容を保存しますか？
                DialogResult result = MessageBox.Show(CTResources.LoadResString(9989), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				switch (result)
				{
					case DialogResult.Yes:			//[はい]を選択
						
						//保存処理
						if (!TrySaveSlicePlanTable()) return;
						break;

					case DialogResult.Cancel:		//[キャンセル]を選択
						return;
				}
			}

			//フォームを閉じる
			this.Close();
		}


		//*******************************************************************************
		//機　　能： 参照ボタンをクリック
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdImgSelect_Click(object sender, EventArgs e)
		{
            List<string> FileList = null;


            //追加2014/10/07hata_v19.51反映
            //V19.20 使用不可追加 by Inaba 2012/11/01 'v19.50 v19.41との統合 by長野 2013/11/17
            //削除ボタンを使用不可にする
            cmdImgDelete.Enabled = false;
            //検索ボタンを使用不可にする
            cmdImgSearch.Enabled = false;

			//枚数を超えていないか
			if (lvwTable.Items.Count >= 200)
			{
				//MsgBox "これ以上は追加できません。", vbCritical
                MessageBox.Show(CTResources.LoadResString(14301), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);		//ストリングテーブル化 '17.60 by長野 2011/5/22
				return;
			}

            FileList = new List<string>();
            string[] theFiles = null;
            
			//コモンダイアログによるファイル選択（複数選択可）
            //変更2014/10/07hata_v19.51反映
            //V19.20 コーンビーム用ファイル種類変更 by Inaba 2012/11/01 'v19.50 v19.41との統合 by長野 2013/11/17
            ////FileList = modFileIO.GetFileListFromFileDialog(CTResources.LoadResString(StringTable.IDS_CTImage));
            //theFiles = modFileIO.GetFileListFromFileDialog(CTResources.LoadResString(StringTable.IDS_CTImage));
            //V19.20 コーンビーム用ファイル種類変更 by Inaba 2012/11/01 'v19.50 v19.41との統合 by長野 2013/11/17
            if (optConeBeam.Checked)
            {
                theFiles = modFileIO.GetFileListFromFileDialog(CTResources.LoadResString(StringTable.IDS_CTImage), "-0001.img");
            }
            else
            {
                theFiles = modFileIO.GetFileListFromFileDialog(CTResources.LoadResString(StringTable.IDS_CTImage));
            }


			//上記のダイアログにてキャンセルがクリックされた
			//if (FileList == null) return;
            //Rev20.00 条件式変更 by長野 2014/12/15
            if (theFiles == null) return;

			//スライスプランに追加
            //foreach (string theFile in FileList)
            //{
            //    AddToSlicePlan(theFile);
            //}
            foreach (string theFile in theFiles)
            {
                FileList.Add(theFile);
                AddToSlicePlan(theFile);
            }

			//追加できなかったファイルがあれば表示する
			//if (modLibrary.IsExistForm(frmInvalidFileList.Instance))
            if (modLibrary.IsExistForm("frmInvalidFileList"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            {
				frmInvalidFileList.Instance.ShowDialog();
			}
		}


		//*******************************************************************************
		//機　　能： スライスプランリストにファイルを追加する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void AddToSlicePlan(string FileName, string theDataMode = null)
		{
			//modImageInfo.ImageInfoStruct theInfoRec = default(modImageInfo.ImageInfoStruct);			//画像付帯情報構造体変数
            ImageInfo theInfoRec = new ImageInfo();
            theInfoRec.Data.Initialize();

			if (theDataMode == null)
			{
				//枚数を超えていないか
				if (lvwTable.Items.Count >= 200)
				{
					//エラー内容：これ以上は追加できません。
					//frmInvalidFileList.AddFile FileName, "これ以上は追加できません。"
                    frmInvalidFileList.Instance.AddFile(FileName, CTResources.LoadResString(14301));		//ストリングテーブル化 '17.60 by長野 2011/5/22
					return;
				}
				//拡張子チェック
				else if (!Regex.IsMatch(FileName.ToUpper(), "^.+[.]IMG$"))
				{
					//エラー内容：拡張子が異なります。
					//frmInvalidFileList.AddFile FileName, "拡張子が異なります。"
                    frmInvalidFileList.Instance.AddFile(FileName, CTResources.LoadResString(20089));		//ストリングテーブル化 '17.60 by長野 2011/5/22
					return;
				}
				//付帯情報ファイルは存在するか？
				else if (!File.Exists(modFileIO.ChangeExtension(FileName, ".inf")))
				{
					//エラー内容：付帯情報が見つかりません。
					frmInvalidFileList.Instance.AddFile(FileName, StringTable.BuildResStr(StringTable.IDS_NotFound, StringTable.IDS_InfoFile));
					return;
				}
				//付帯情報の取得
				//else if (!modImageInfo.ReadImageInfo(ref theInfoRec, modLibrary.RemoveExtension(FileName, ".img")))
                else if (!ImageInfo.ReadImageInfo(ref  theInfoRec.Data, modLibrary.RemoveExtension(FileName, ".img")))
                {
					//エラー内容：付帯情報読み込みエラー
					//frmInvalidFileList.AddFile FileName, "付帯情報読み込みエラー"
                    frmInvalidFileList.Instance.AddFile(FileName, CTResources.LoadResString(20090));		//ストリングテーブル化 '17.60 by長野 2011/5/22
					return;
				}
				//データモードがコーンビームの時、コーンビーム以外の画像ファイルを含めない
				else if (optConeBeam.Checked && (theInfoRec.Data.data_mode != (int)ScanSel.DataModeConstants.DataModeCone))
				{
					//エラー内容：コーンビーム以外の画像です。
					//frmInvalidFileList.AddFile FileName, "コーンビーム以外の画像です。"
                    frmInvalidFileList.Instance.AddFile(FileName, CTResources.LoadResString(20091));		//ストリングテーブル化 '17.60 by長野 2011/5/22
					return;
				}

                theDataMode = (theInfoRec.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone ? "*" : "");
			}

			ListViewItem theItem = null;

			//新規項目追加
			theItem = lvwTable.Items.Add(Convert.ToString(lvwTable.Items.Count + 1).PadLeft(4));

			if (theItem != null)		//TODO ListView SubItems Key
			{
				
                ////追加した項目のSubItemを生成
                //theItem.SubItems.Add(FileName);
                //theItem.SubItems[1].Name = "CTImage";
                //theItem.SubItems.Add(theDataMode);
                //theItem.SubItems[2].Name ="Cone";

                ListViewItem.ListViewSubItem SubItem0 = new ListViewItem.ListViewSubItem();
                SubItem0.Name = "CTImage";
                SubItem0.Text = FileName;
                theItem.SubItems.Add(SubItem0);

                ListViewItem.ListViewSubItem SubItem1 = new ListViewItem.ListViewSubItem();
                //Rev20.00 修正 by長野 2014/12/15
                SubItem1.Name = "Cone";
                //SubItem1.Name = "Conee";
                SubItem1.Text = theDataMode;
                theItem.SubItems.Add(SubItem1);
                
            }

			//テーブル内容に変更有り
			Changed = true;
		}


		//*******************************************************************************
		//機　　能： 開くボタンをクリック
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private bool OpenSlicePlanTable(string FileName)
		{
			string Data = null;				//スライスプランテーブルから読み込んだレコード

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Dim fileNo      As Integer  'ファイル番号   '追加 by 鈴山 '97-11-18
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			string[] strCell = null;
			string ConeBeamStr = null;

			//戻り値初期化
			bool functionReturnValue = false;

			//リストボックス内の項目を全て削除する
			lvwTable.Items.Clear();

			//削除ボタンを使用不可にする
			cmdImgDelete.Enabled = false;
            //検索ボタンを使用不可にする 'V19.20 追加 by Inaba 2012/11/01 'v19.50 v19.4との統合 by長野 2013/11/17    //追加2014/10/07hata_v19.51反映
            cmdImgSearch.Enabled = false;

			//エラー時の設定
			try
			{
				StreamReader sr = null;

				try
				{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
					'ファイルオープン
					fileNo = FreeFile()
					Open FileName For Input As #fileNo

					Do While Not EOF(fileNo)

						'ファイルから１行読み込む
						Line Input #fileNo, Data
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

					//ファイルオープン
					//sr = new StreamReader(FileName);
                    //Rev20.00 長野 shift-jisでデコード by長野 2014/12/15
                    sr = new StreamReader(FileName,System.Text.Encoding.GetEncoding("shift-jis"));

					double IsNumeric = 0;

					while ((Data = sr.ReadLine()) != null)		//ファイルから１行読み込む
					{
						if (!string.IsNullOrEmpty(Data))
						{
							//コンマで切り出し配列に格納
							strCell = Data.Split(',');

							//コメントを取り出す
							if (strCell[0] == CTResources.LoadResString(StringTable.IDS_Comment))
							{
								if (strCell.GetUpperBound(0) == 0)
								{
									myComment = "";
								}
								else
								{
									myComment = strCell[1];
								}
							}
							//先頭列の文字が数字なら情報を取り出す
							else if (double.TryParse(strCell[0], out IsNumeric))
							{
								if (strCell.GetUpperBound(0) < 3)
								{ }
								else
								{
									ConeBeamStr = "";
									if (strCell.GetUpperBound(0) >= 13)
									{
										if (strCell[13] == "4") ConeBeamStr = "*";
									}
									//リストボックス内に項目を追加
									//lstImgFile.AddItem ConeBeamStr & strCell(2) & strCell(3)
									AddToSlicePlan(strCell[2] + strCell[3], ConeBeamStr);
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
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
					'ファイルクローズ
					Close #fileNo
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

					//ファイルクローズ
					if (sr != null)
					{
						sr.Close();
						sr = null;
					}
				}

				//編集中スライスプランテーブル名の設定
				Target = FileName;

				//戻り値セット（成功）
				functionReturnValue = true;
			}
			catch (Exception ex)
			{
				//エラーメッセージ表示
				MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： 変更ボタンをクリック
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdTableSelect_Click(object sender, EventArgs e)
		{
			string FileName = null;			//ファイル名（フルパス）
			//string PathName = null;			//パス名
			//string TableName = null;		//テーブル名（拡張子を取り除いたタイプ）

            //追加2014/10/07hata_v19.51反映
            //V19.20 使用不可追加 by Inaba 2012/11/01 'v19.50 v19.41との統合 by長野 2013/11/17
            //削除ボタンを使用不可にする
            cmdImgDelete.Enabled = false;
            //検索ボタンを使用不可にする
            cmdImgSearch.Enabled = false;

			FileName = modFileIO.GetFileName(StringTable.IDS_Select, CTResources.LoadResString(StringTable.IDS_SlicePlanTable), SubExtension: (optConeBeam.Checked ? "-csp" : "-spl"), InitFileName: txtSliceplan.Text);
			if (string.IsNullOrEmpty(FileName)) return;

			//開いたスライスプランテーブルのデータモードに応じてスライスプラン画面のデータモードを設定する     'v11.4追加 by 間々田 2006/03/16
			if (FileName.ToUpper().EndsWith("-CSP.CSV"))
			{
				optConeBeam.Checked = true;
			}
			else
			{
				optScan.Checked = true;
			}
            var _with6 = CTSettings.scansel;

			//選択したテーブル名をコモンに書き込む   v11.5変更 by 間々田 2006/04/21
			if (optConeBeam.Checked)
			{
                //if文追加2014/10/07hata_v19.51反映
                if (frmScanControl.Instance.optDataMode[(int)ScanSel.DataModeConstants.DataModeScan].Checked)   //V19.20 書込み条件追加 by Inaba 2012/11/01
                {
                    //modLibrary.SetField(modLibrary.AddExtension(Path.GetDirectoryName(FileName), "\\"), ref modScansel.scansel.cone_sliceplan_dir);		//コーンビーム用ディレクトリ
				    //modLibrary.SetField(Path.GetFileNameWithoutExtension(FileName), ref modScansel.scansel.cone_slice_plan);							//コーンビーム用テーブル名
                    _with6.Data.cone_sliceplan_dir.SetString(modLibrary.AddExtension(Path.GetDirectoryName(FileName), "\\"));       //コーンビーム用ディレクト
                    _with6.Data.cone_slice_plan.SetString(Path.GetFileNameWithoutExtension(FileName));                              //コーンビーム用テーブル名
                }              
                
            }
			else
			{
                //if文追加2014/10/07hata_v19.51反映
                if (frmScanControl.Instance.optDataMode[(int)ScanSel.DataModeConstants.DataModeCone].Checked)   //V19.20 書込み条件追加 by Inaba 2012/11/01
                {
                    //modLibrary.SetField(modLibrary.AddExtension(Path.GetDirectoryName(FileName), "\\"), ref modScansel.scansel.sliceplan_dir);		//パス名
				    //modLibrary.SetField(Path.GetFileNameWithoutExtension(FileName), ref modScansel.scansel.slice_plan);								//テーブル名
                    _with6.Data.sliceplan_dir.SetString(modLibrary.AddExtension(Path.GetDirectoryName(FileName), "\\"));            //パス名
                    _with6.Data.slice_plan.SetString(Path.GetFileNameWithoutExtension(FileName));                                   //テーブル名
                 }
                
            }

			//書き込み
			//modScansel.PutScansel(ref modScansel.scansel);
            _with6.Write();

			//現在選択されているスライスプランテーブルファイル名を「使用するテーブル」欄に表示
            //変更2014/10/07hata_v19.51反映
            //txtSliceplan.Text = modCT30K.GetSlicePlanTable((optConeBeam.Checked));
            txtSliceplan.Text = FileName;            //vV19.20 変更 by Inaba 2012/11/01 'v19.50 v19.41との統合 by長野 2013/11/17

            //追加2014/10/07hata_v19.51反映
            //現在選択されているスライスプランテーブルファイル名をスキャン条件の「スライスプランテーブル」欄に表示 'V19.20 追加 by Inaba 2012/11/01 'v19.50 v19.41との統合 by長野 2013/11/17
            if (optConeBeam.Checked == frmScanControl.Instance.optDataMode[(int)ScanSel.DataModeConstants.DataModeCone].Checked | 
                optScan.Checked == frmScanControl.Instance.optDataMode[(int)ScanSel.DataModeConstants.DataModeScan].Checked)
            {
                //frmScanCondition.Instance.txtSlicePlanDir.Text = modFileIO.FSO.GetParentFolderName(FileName);
                //frmScanCondition.Instance.txtSlicePlanName.Text = modFileIO.FSO.GetFileName(FileName);
                frmScanCondition.Instance.txtSlicePlanDir.Text = Path.GetDirectoryName(FileName);
                frmScanCondition.Instance.txtSlicePlanName.Text = Path.GetFileName(FileName);
            }

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
		private void frmSliceplan_Load(object sender, EventArgs e)
		{
			//実行時はフラグをセット
			modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTSlicePlan;

			//変数初期化
			myComment = "";

			//編集中ファイル名を初期化
			Target = "";

			//フォームの位置・サイズの設定
			modCT30K.SetPosNormalForm(this);

			//v7.0 リソース対応 by 間々田 2003/08/23
			SetCaption();

			//データモードフレームの表示（scaninh.cone_multiscan_mode[2]=0 の時だけ）v8.0 added by 間々田 2003/12/05
            fraDataMode.Visible = (CTSettings.scaninh.Data.cone_multiscan_mode[2] == 0);

			//現在選択されているスライスプランテーブルファイル名を「使用するテーブル」欄に表示
            //変更2014/10/07hata_v19.51反映
            //txtSliceplan.Text = modCT30K.GetSlicePlanTable(optConeBeam.Checked);
            txtSliceplan.Text = Path.Combine(frmScanCondition.Instance.txtSlicePlanDir.Text, frmScanCondition.Instance.txtSlicePlanName.Text);      //V19.20 変更 by Inaba 2012/11/01 'v19.50 v19.41との統合 by長野 2013/11/17

            //追加2014/10/07hata_v19.51反映
            //スキャン条件のデータモードに応じてスライスプラン画面のデータモードを設定する 'V19.20 追加 by Inaba 2012/11/01
            if (frmScanControl.Instance.optDataMode[(int)ScanSel.DataModeConstants.DataModeCone].Checked)
            {
                optConeBeam.Checked = true;
                lvwTable.Height = 165; //V19.20 コーンの推奨スライスプラン数は10個 by Inaba 2012/11/26
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
		private void frmSliceplan_FormClosed(object sender, FormClosedEventArgs e)
		{
			//終了時はフラグをリセット
			modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTSlicePlan);
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

			//ツールバー上のボタンのToolTipTextの設定
            Toolbar1.Items["tsbtnOpen"].ToolTipText = StringTable.BuildResStr(StringTable.IDS_Open, StringTable.IDS_SlicePlanTable);		//スライスプランテーブルを開く
            Toolbar1.Items["tsbtnSave"].ToolTipText = StringTable.BuildResStr(StringTable.IDS_Save, StringTable.IDS_SlicePlanTable);		//スライスプランテーブルの保存
            Toolbar1.Items["tsbtnComment"].ToolTipText = StringTable.BuildResStr(StringTable.IDS_Input, StringTable.IDS_Comment);		//コメント入力
		}


		//*******************************************************************************
		//機　　能： データモードフレーム内コーンビームオプションボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void optConeBeam_CheckedChanged(object sender, EventArgs e)
		{
			if (optConeBeam.Checked)
			{
				//現在選択されているスライスプランテーブルファイル名を「使用するテーブル」欄に表示
                //変更2014/10/07hata_v19.51反映
                //txtSliceplan.Text = modCT30K.GetSlicePlanTable(optConeBeam.Checked);
                if (frmScanControl.Instance.optDataMode[(int)ScanSel.DataModeConstants.DataModeCone].Checked)
                {
                    txtSliceplan.Text = Path.Combine(frmScanCondition.Instance.txtSlicePlanDir.Text, frmScanCondition.Instance.txtSlicePlanName.Text);      //V19.20 変更 by Inaba 2012/11/01 'v19.50 v19.41との統合 by長野 2013/11/17
                }
                else
                {
                    txtSliceplan.Text = modCT30K.GetSlicePlanTable((optConeBeam.Checked));
                }
                //V19.20 コーンの推奨スライスプラン数は10個 by Inaba 2012/11/26
                lvwTable.Height = 165;

            }
		}

		//*******************************************************************************
		//機　　能： データモードフレーム内スキャンオプションボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void optScan_CheckedChanged(object sender, EventArgs e)
		{
			if (optScan.Checked)
			{
				//現在選択されているスライスプランテーブルファイル名を「使用するテーブル」欄に表示
                //変更2014/10/07hata_v19.51反映
                //txtSliceplan.Text = modCT30K.GetSlicePlanTable(optConeBeam.Checked);
                if (frmScanControl.Instance.optDataMode[(int)ScanSel.DataModeConstants.DataModeScan].Checked)
                {
                    txtSliceplan.Text = Path.Combine(frmScanCondition.Instance.txtSlicePlanDir.Text, frmScanCondition.Instance.txtSlicePlanName.Text);      //V19.20 変更 by Inaba 2012/11/01 'v19.50 v19.41との統合 by長野 2013/11/17
                }
                else
                {
                    txtSliceplan.Text = modCT30K.GetSlicePlanTable((optConeBeam.Checked));
                }
                //V19.20 スキャンの最大スライスプラン数は200個 by Inaba 2012/11/26
                lvwTable.Height = 325;
            }
		}


		//*******************************************************************************
		//機　　能： ツールバー上のボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
* 		Private Sub Toolbar1_ButtonClick(ByVal Button As MSComctlLib.Button)
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
		private void Toolbar1_ButtonClick(object sender, EventArgs e)
		{
			if (sender as ToolStripButton == null) return;
			ToolStripButton Button = (ToolStripButton)sender;

			string FileName = null;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Select Case Button.key
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			switch (Button.Name)
			{
				//スライスプランテーブルを開く
                case "tsbtnOpen":

					//ファイル選択ダイアログ表示
					FileName = modFileIO.GetFileName(Description: CTResources.LoadResString(StringTable.IDS_SlicePlanTable),　SubExtension: (optConeBeam.Checked ? "-csp" : "-spl"), InitFileName: Target);
					if (string.IsNullOrEmpty(FileName)) return;

					//ファイルの読み込み
					if (OpenSlicePlanTable(FileName))
					{
						//開いたスライスプランテーブルのデータモードに応じてスライスプラン画面のデータモードを設定する
						if (Regex.IsMatch(FileName.ToUpper(), "^.+-CSP[.]CSV$"))
						{
							optConeBeam.Checked = true;
						}
						else
						{
							optScan.Checked = true;
						}
					}
					break;

				//スライスプランテーブルの保存
                case "tsbtnSave":

					//リストボックスに１枚も画像がなければ保存しない。 '追加 by 鈴山 '97-10-20
					if (lvwTable.Items.Count < 1)
					{
						//エラー表示
						modCT30K.ErrMessage(1213,Icon: MessageBoxIcon.Error);
						return;
					}

					//スライスプランテーブル保存処理
					TrySaveSlicePlanTable();
					break;

				//コメント入力
                case "tsbtnComment":

					//コメント入力ダイアログ表示
					string theComment = null;
					theComment = frmComment.Dialog(StringTable.LoadResStringWithColon(StringTable.IDS_SlicePlanTable) + "\r" + Target, 
															StringTable.GetResString(StringTable.IDS_InputCommentOf, CTResources.LoadResString(StringTable.IDS_SlicePlanTable)), 
															myComment);
					if (theComment != null)
					{
						myComment = theComment;
						Changed = true;				//テーブル内容に変更有り
					}
					break;
			}
		}


		//*******************************************************************************
		//機　　能： リストビュークリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void lvwTable_Click(object sender, EventArgs e)
		{
            //Rev20.00 ★要確認 この処理は不要では？フォーカスされているリストは検索機能に使用するはず・・・。by長野 2014/12/15
            //if (ClickedItem != null)
            //{
            //    ClickedItem = null;
            //}
            //else if (lvwTable.FocusedItem != null)
            //{
            //    lvwTable.FocusedItem.Selected = false;
            //    lvwTable.FocusedItem = null;
            //}

            //削除ボタンの使用可・不可を設定
            cmdImgDelete.Enabled = (lvwTable.FocusedItem != null);

            //追加2014/10/07hata_v19.51反映
            //検索ボタンの使用可・不可を設定 'V19.20 追加 by Inaba 2012/11/01 'v19.50 v19.41との統合 by長野 2013/11/17
            cmdImgSearch.Enabled = (lvwTable.FocusedItem != null);
        }


		//*******************************************************************************
		//機　　能： リストビューキーダウン時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void lvwTable_KeyDown(object sender, KeyEventArgs e)
		{
			//キーを判定
			switch (e.KeyCode)
			{
				//[Del]キーで削除
				case Keys.Delete:

					//[削除]ボタンを表示していたら実行
					if (cmdImgDelete.Enabled)
					{
						cmdImgDelete_Click(cmdImgDelete, EventArgs.Empty);
					}
					break;

				//[Esc]キーで元の状態に戻す
				case Keys.Escape:

					//全てを非選択にする
					foreach (ListViewItem theItem in lvwTable.Items)
					{
						theItem.Selected = false;
					}

					lvwTable_Click(lvwTable, EventArgs.Empty);
					break;
			}
		}


		//*******************************************************************************
		//機　　能： リストビュー項目クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
* 		Private Sub lvwTable_ItemClick(ByVal Item As MSComctlLib.ListItem)
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
		private void lvwTable_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (sender as ListViewItem == null) return;
			ListViewItem Item = (ListViewItem)sender;

			if (Item.Selected == false) return;

			//クリックした項目を記憶
			ClickedItem = Item;
		}


		//*******************************************************************************
		//機　　能： 削除ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdImgDelete_Click(object sender, EventArgs e)
		{
			int i = 0;

			//選択されている項目を削除
			for (i = lvwTable.Items.Count - 1; i >= 0; i--)
			{
				if (lvwTable.Items[i].Selected)
				{
					lvwTable.Items.RemoveAt(i);
				}
			}

			//再度ナンバリング
			for (i = 0; i < lvwTable.Items.Count; i++)
			{
				lvwTable.Items[i].Text = Convert.ToString(i).PadLeft(8);
			}

			//選択されている項目がないようにするため以下の処理を実行
			lvwTable_Click(lvwTable, EventArgs.Empty);
		}

        //*******************************************************************************
        //機　　能： 検索ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V19.20  2012/11/01   Inaba      新規作成
        //*******************************************************************************
        private void cmdImgSearch_Click(System.Object eventSender, System.EventArgs eventArgs)
        {

            int i = 0;
            string rec1 = null;
            string rec2 = null;

            var _with10 = lvwTable.Items;

            //選択された最初のファイルル名の付帯情報からスライスプラン用の撮影条件を取得する
            for (i = 0; i < _with10.Count; i += 1)
            {
                if (_with10[i].Selected)
                {
                    rec1 = GetScanCondition(_with10[i].SubItems["CTImage"].Text);
                    if (!string.IsNullOrEmpty(rec1))
                        break; // TODO: might not be correct. Was : Exit For
                }
            }

            //同じ撮影条件（管電圧、管電流、FCD、FDD、Y軸）のファイルを検索
            for (i = _with10.Count - 1; i >= 0; i += -1)
            {
                rec2 = GetScanCondition(_with10[i].SubItems["CTImage"].Text);
                if (rec1 != rec2)
                {
                    _with10.RemoveAt(i);
                }
            }

            //再度ナンバリング
            for (i = 0; i < _with10.Count; i++)
            {
                //_with10[i].Text = Strings.Right(Strings.Space(8) + Convert.ToString(i), 8);
                //_with10[i].Text = Convert.ToString(i).PadLeft(8);
                //Rev20.00 1オリジンに変更 by長野 2014/12/15
                _with10[i].Text = Convert.ToString(i + 1).PadLeft(4); 
            }

            //選択されている項目がないようにするため以下の処理を実行
            lvwTable_Click(lvwTable, new System.EventArgs());
  
        }

        //*************************************************************************************************
        //機　　能： 指定したファイル名の付帯情報からスライスプラン用のスキャン条件を取得する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V19.20  2012/11/01   Inaba      新規作成
        //*************************************************************************************************
        private string GetScanCondition(string FileName)
        {
            string functionReturnValue = null;

            //ImageInfoStruct InfRec = default(ImageInfoStruct);
            ImageInfo InfRec = new ImageInfo();
            InfRec.Data.Initialize();

            string buf = null;

            //戻り値初期化
            functionReturnValue = "";

            //付帯情報取得
            //if (!modImageInfo.ReadImageInfo(InfRec, modLibrary.RemoveExtension(FileName, ".img")))
            if (!ImageInfo.ReadImageInfo(ref InfRec.Data, modLibrary.RemoveExtension(FileName, ".img")))
            {
                //メッセージ表示：付帯情報が見つかりません。
                //Interaction.MsgBox(FileName + Constants.vbCr + Constants.vbCr + StringTable.BuildResStr(StringTable.IDS_NotFound, StringTable.IDS_ImageInfo), MsgBoxStyle.Exclamation);
                MessageBox.Show(FileName + "\r" + "\r" + StringTable.BuildResStr(StringTable.IDS_NotFound, StringTable.IDS_ImageInfo),
                                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return functionReturnValue;
            }

            //画像付帯情報をレコードに記録
            //var _with11 = InfRec;
            //管電圧(kV)
            //buf = Conversion.Val(_with11.Data.volt), "0.0");  //管電圧(kV)
            double d_volt = 0;
            double.TryParse(InfRec.Data.volt.GetString(), out d_volt);
            buf = d_volt.ToString("0.0");

            //管電流(μA)
            //buf = buf + "," + Microsoft.VisualBasic.Compatibility.VB6.Support.Format(Conversion.Val(_with11.anpere), "0.000");    //管電流(μA)
            double d_anpere = 0;
            double.TryParse(InfRec.Data.anpere.GetString(), out d_anpere);
            buf = buf + "," + d_anpere.ToString("0.000");

            //FID
            //buf = buf + "," + Microsoft.VisualBasic.Compatibility.VB6.Support.Format(_with11.Data.fid - _with11.Data.fid_offset, "0.0");            //FID
            buf = buf + "," + (InfRec.Data.fid - InfRec.Data.fid_offset).ToString("0.0");            //FID
           
            //FCD
            //buf = buf + "," + Microsoft.VisualBasic.Compatibility.VB6.Support.Format(_with11.FCD - _with11.fcd_offset, "0.0");            //FCD
            buf = buf + "," + (InfRec.Data.fcd - InfRec.Data.fcd_offset).ToString("0.0");            //FCD
            
            //試料テーブル(光軸と垂直方向)座標mm
            //buf = buf + "," + Microsoft.VisualBasic.Compatibility.VB6.Support.Format(Conversion.Val(Convert.ToString(_with11.Data.table_x_pos)), "0.00");            //試料テーブル(光軸と垂直方向)座標mm
            buf = buf + "," + InfRec.Data.table_x_pos.ToString("0.00");            //試料テーブル(光軸と垂直方向)座標mm

            if (InfRec.Data.detector == 0)
            {
                buf = buf + "," + InfRec.Data.iifield;                //I.I.視野
            }
            else if (InfRec.Data.detector == 2)
            {
                buf = buf + "," + modCT30K.GetFpdGainStr(InfRec.Data.fpd_gain, CTSettings.t20kinf.Data.pki_fpd_type);                //FPDゲイン(pF)
                buf = buf + "," + modCT30K.GetFpdIntegStr(InfRec.Data.fpd_integ);                //FPD積分時間(ms)
            }

            //戻り値セット
            functionReturnValue = buf;
            return functionReturnValue;

        }

	}
}
