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

//Rev20.00 追加 by長野 2014/12/15
using System.Diagnostics;

namespace CT30K
{
	///* ************************************************************************** */
	///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： 付帯情報修正.frm                                            */
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
	///* V19.00      12/02/21    H.Nagai             BHC対応　　　　　　　　　　    */
	///*                                                                            */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
	///* ************************************************************************** */
	public partial class frmPictureRetouch : Form
	{

		//********************************************************************************
		//  共通データ宣言
		//********************************************************************************

		//編集中の付帯情報ファイル名（拡張子を除いたタイプ）
		private string myTarget;

		//付帯情報レコード
		//private modImageInfo.ImageInfoStruct ImageInfoRec = default(modImageInfo.ImageInfoStruct);
        ImageInfo ImageInfoRec = new ImageInfo();

		//項目のインデクスを格納する変数
		private int IdxProductName = 0;		//スキャン位置
		private int IdxSliceName = 0;		//スキャン年月日
		private int IdxScanPos = 0;			//スキャン位置
		private int IdxScanDate = 0;		//スキャン年月日
		private int IdxScanTime = 0;		//スキャン時刻
		private int IdxDataAcq = 0;			//データ収集時間
		private int IdxReconTime = 0;		//再構成時間
		private int IdxTubeVolt = 0;		//管電圧
		private int IdxTubeCurrent = 0;		//管電流
		private int IdxViews = 0;			//ビュー数
		private int IdxIntegNum = 0;		//積算枚数
		private int IdxIIField = 0;			//I.I.視野
		//Dim IdxMaxScanArea  As Integer  '最大スキャンエリア
		private int IdxFOV = 0;				//FOV                    'v15.0変更 最大スキャンエリア→FOVに変更 2009/07/24 by 間々田
		private int IdxSliceWidth = 0;		//スライス厚
		private int IdxSystemName = 0;		//システム名
		//Dim IdxWorkshopName As Integer  '事業所名                  'v15.0削除 表示しないことになった 2009/07/24 by 間々田
		private int IdxComment = 0;			//コメント
		private int IdxScanMode = 0;		//スキャンモード
		private int IdxFilterFunc = 0;		//フィルタ関数
		private int IdxFilterProc = 0;		//フィルタ処理
		private int IdxRFC = 0;				//RFC
		private int IdxBHCTable = 0;		//BHCテーブル v19.00
		private int IdxImageBias = 0;		//画像バイアス
		private int IdxImageSlope = 0;		//画像スロープ
		private int IdxDirection = 0;		//断面像方向
		private int IdxFID = 0;				//FID
		private int IdxFCD = 0;				//FCD

        //追加2014/07/23hata_v19.51反映
        private int IdxTableXPos = 0;           //'Y軸(AxisName(1))   'v18.00追加 byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/17

        private int IdxWindowLevel = 0;		//ウィンドウレベル
		private int IdxWindowWidth = 0;		//ウィンドウ幅
		private int IdxScale = 0;			//スケール
		private int IdxGamma = 0;			//ガンマ補正 ｖ19.00　by長野 2012/02/21


		private List<Label> lblPtouch = null;
		private List<Label> lblPtouchColum = null;
		private List<TextBox> txtPtouch = null;

		private static frmPictureRetouch _Instance = null;

		public frmPictureRetouch()
		{
			InitializeComponent();

			lblPtouch = new List<Label> {lblPtouch0};
			lblPtouchColum = new List<Label> {lblPtouchColum0};
			txtPtouch = new List<TextBox>{txtPtouch0};
		}

		public static frmPictureRetouch Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmPictureRetouch();
				}

				return _Instance;
			}
		}


		//*******************************************************************************
		//機　　能： フォームロード時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void frmPictureRetouch_Load(object sender, EventArgs e)
		{
            ImageInfoRec.Data.Initialize();
           
            //実行時はフラグをセット
			modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTImageProcessing;

			//v17.60 英語用レイアウト調整　by長野 2011/05/25
			if (modCT30K.IsEnglish)
			{
				EnglishAdjustLayout();
			}

			//フォームを標準位置に移動
			modCT30K.SetPosNormalForm(this);

			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

			//コントロール初期化
			InitControls();			//v14.00追加 byやまおか 2007/06/29

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
		private void frmPictureRetouch_FormClosed(object sender, FormClosedEventArgs e)
		{
			//終了時はフラグをリセット
			modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTImageProcessing);
		}


		//*******************************************************************************
		//機　　能： 閉じるボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdPtouchCancel_Click(object sender, EventArgs e)
		{
			//付帯情報修正フォームをアンロード
			this.Close();
		}


		//*******************************************************************************
		//機　　能： 更新ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdPtouchOK_Click(object sender, EventArgs e)
		{
			//付帯情報への書き込み
			if (ModifyImageInfo())
			{
				//メッセージ：付帯情報を保存しました。
				MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_Saved, StringTable.IDS_ImageInfo), 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				//メッセージ：付帯情報ファイルの書き込みに失敗しました。
				MessageBox.Show(StringTable.GetResString(StringTable.IDS_WentWrong, StringTable.BuildResStr(StringTable.IDS_Writing, StringTable.IDS_InfoFile)), 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}


		//*******************************************************************************
		//機　　能： 付帯情報修正処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00   99/XX/XX   ????????     新規作成
		//           v10.2 2005/07/04  (SI3)間々田   image_infoを使用しないことにした
		//*******************************************************************************
		private bool ModifyImageInfo()
		{
			//戻り値初期化
			bool functionReturnValue = false;

			//エラー発生時の扱い
			try
			{
				//ビュー数を変更すると更新のときに警告を出す     'v14.0追加 byやまおか 2007/08/09
				double ViewsText = 0;
				double ViewsTag = 0;
				double.TryParse(txtPtouch[IdxViews].Text, out ViewsText);
				double.TryParse(Convert.ToString(txtPtouch[IdxViews].Tag), out ViewsTag);
				if (ViewsText != ViewsTag)
				{
					DialogResult result = MessageBox.Show(StringTable.GetResString(9929, lblPtouch[IdxViews].Text), Application.ProductName, MessageBoxButtons.YesNo);
					if (result == DialogResult.No) txtPtouch[IdxViews].Text = Convert.ToString(txtPtouch[IdxViews].Tag);
				}

				//FIDを変更すると更新のときに警告を出す          'v14.0追加 byやまおか 2007/08/09
				double FIDText = 0;
				double FIDTag = 0;
				double.TryParse(txtPtouch[IdxFID].Text, out FIDText);
				double.TryParse(Convert.ToString(txtPtouch[IdxFID].Tag), out FIDTag);
				if (FIDText != FIDTag)
				{
					DialogResult result = MessageBox.Show(StringTable.GetResString(9929, lblPtouch[IdxFID].Text), Application.ProductName, MessageBoxButtons.YesNo);
					if (result == DialogResult.No) txtPtouch[IdxFID].Text = Convert.ToString(txtPtouch[IdxFID].Tag);
				}

				//FCDを変更すると更新のときに警告を出す          'v14.0追加 byやまおか 2007/08/09
				double FCDText = 0;
				double FCDTag = 0;
				double.TryParse(txtPtouch[IdxFCD].Text, out FCDText);
				double.TryParse(Convert.ToString(txtPtouch[IdxFCD].Tag), out FCDTag);
				if (FCDText != FCDTag)
				{
					DialogResult result = MessageBox.Show(StringTable.GetResString(9929, lblPtouch[IdxFCD].Text), Application.ProductName, MessageBoxButtons.YesNo);
					if (result == DialogResult.No) txtPtouch[IdxFCD].Text = Convert.ToString(txtPtouch[IdxFCD].Tag);
				}

                //modLibrary.SetField(Convert.ToString(value), ref myInfoRec.gamma);
                //modImageInfo.WriteImageInfo(ref myInfoRec, myTarget);
                
                //v11.43変更 by 間々田 2006/04/21 文字列の場合、末尾をスペースではなくヌルで埋めるようにした
				//modLibrary.SetField(txtPtouch[IdxScanPos].Text, ref ImageInfoRec.d_tablepos);									//スキャン位置(mm)（絶対座標）
                ImageInfoRec.Data.d_tablepos.SetString(txtPtouch[IdxScanPos].Text);
                
                //modLibrary.SetField(txtPtouch[IdxScanDate].Text, ref ImageInfoRec.d_date);										//スキャン年月日
                ImageInfoRec.Data.d_date.SetString(txtPtouch[IdxScanDate].Text);
                
                //modLibrary.SetField(txtPtouch[IdxScanTime].Text, ref ImageInfoRec.start_time);									//スキャン時刻
                ImageInfoRec.Data.start_time.SetString(txtPtouch[IdxScanTime].Text);
                
                //modLibrary.SetField(txtPtouch[IdxTubeVolt].Text, ref ImageInfoRec.volt);										//管電圧(kV)
                ImageInfoRec.Data.volt.SetString(txtPtouch[IdxTubeVolt].Text);
                
                //modLibrary.SetField(txtPtouch[IdxTubeCurrent].Text, ref ImageInfoRec.anpere);									//管電流(μA)
                ImageInfoRec.Data.anpere.SetString(txtPtouch[IdxTubeCurrent].Text);
                
                //modLibrary.SetField(txtPtouch[IdxViews].Text, ref ImageInfoRec.scan_view);										//ビュー数
                ImageInfoRec.Data.scan_view.SetString(txtPtouch[IdxViews].Text);
                
                //modLibrary.SetField(txtPtouch[IdxIntegNum].Text, ref ImageInfoRec.integ_number);								//積算枚数
                ImageInfoRec.Data.integ_number.SetString(txtPtouch[IdxIntegNum].Text);
                
                //modLibrary.SetField(txtPtouch[IdxIIField].Text, ref ImageInfoRec.iifield);										//I.I.視野
                ImageInfoRec.Data.iifield.SetString(txtPtouch[IdxIIField].Text);

                //.mscan_area = Val(txtPtouch(IdxMaxScanArea).Text)                                       '最大ｽｷｬﾝｴﾘｱ(mm)
				double mscan_area = 0;
				double.TryParse(txtPtouch[IdxFOV].Text, out mscan_area);
				ImageInfoRec.Data.mscan_area = (float)mscan_area;																	//FOV(mm)            'v15.0変更 最大スキャンエリア→FOVに変更 2009/07/24 by 間々田
				
                
                //modLibrary.SetField(txtPtouch[IdxSliceWidth].Text, ref ImageInfoRec.Width);										//スライス厚(mm)
                ImageInfoRec.Data.width.SetString(txtPtouch[IdxSliceWidth].Text);
                
                //modLibrary.SetField(txtPtouch[IdxSystemName].Text, ref ImageInfoRec.system_name);								//システム名
                ImageInfoRec.Data.system_name.SetString(txtPtouch[IdxSystemName].Text);
                
                //SetField txtPtouch(IdxWorkshopName).Text, .workshop                                     '事業所名          'v15.0削除 表示しないことになった 2009/07/24 by 間々田
				//modLibrary.SetField(txtPtouch[IdxComment].Text, ref ImageInfoRec.Comment);										//コメント
                ImageInfoRec.Data.comment.SetString(txtPtouch[IdxComment].Text);
                
                //modLibrary.SetField(txtPtouch[IdxScanMode].Text, ref ImageInfoRec.full_mode);									//スキャンモード
                ImageInfoRec.Data.full_mode.SetString(txtPtouch[IdxScanMode].Text);
                
                //modLibrary.SetField(txtPtouch[IdxFilterFunc].Text, ref ImageInfoRec.fc);										//フィルタ関数
                ImageInfoRec.Data.fc.SetString(txtPtouch[IdxFilterFunc].Text);
                
                double image_bias = 0;
				double.TryParse(txtPtouch[IdxImageBias].Text, out image_bias);
                //2014/11/07hata キャストの修正
                //ImageInfoRec.Data.image_bias = (int)image_bias;																		//画像バイアス
                ImageInfoRec.Data.image_bias = Convert.ToInt32(image_bias);																		//画像バイアス
                double image_slope = 0;
				double.TryParse(txtPtouch[IdxImageSlope].Text, out image_slope);
                ImageInfoRec.Data.image_slope = (float)image_slope;																	//画像スロープ
                ImageInfoRec.Data.image_direction = ((txtPtouch[IdxDirection].Text.Trim().ToUpper() == "BOTTOM") ? 1 : 0);			//断面像方向
				double Fid = 0;
				double.TryParse(txtPtouch[IdxFID].Text, out Fid);
                ImageInfoRec.Data.fid = (float)Fid + ImageInfoRec.Data.fid_offset;														//FID
				double FCD = 0;
				double.TryParse(txtPtouch[IdxFCD].Text, out FCD);
                ImageInfoRec.Data.fcd = (float)FCD + ImageInfoRec.Data.fcd_offset;																//FCD

                //追加2014/07/23hata_v19.51反映
                double X_Pos = 0;
                double.TryParse(txtPtouch[IdxTableXPos].Text, out X_Pos);
                ImageInfoRec.Data.table_x_pos = (float)X_Pos;   //'Y軸(AxisName(1))   'v18.00追加 byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/17

                //modLibrary.SetField(txtPtouch[IdxWindowLevel].Text, ref ImageInfoRec.WL);										//ウィンドウレベル
                ImageInfoRec.Data.wl.SetString(txtPtouch[IdxWindowLevel].Text);

                //modLibrary.SetField(txtPtouch[IdxWindowWidth].Text, ref ImageInfoRec.ww);										//ウィンドウ幅
                ImageInfoRec.Data.ww.SetString(txtPtouch[IdxWindowWidth].Text);
                                
                //modLibrary.SetField(txtPtouch[IdxGamma].Text, ref ImageInfoRec.GAMMA);											//ガンマ補正値 v19.00 追加 by長野 2012/2/21
                ImageInfoRec.Data.gamma.SetString(txtPtouch[IdxGamma].Text);
                             
                double Scale = 0;
				double.TryParse(txtPtouch[IdxScale].Text, out Scale);
				//modLibrary.SetField(Convert.ToString(Scale * 10 * 1000), ref ImageInfoRec.scale);								//スケール 1/10してミクロンからmmにした数値を表示していたので
                ImageInfoRec.Data.scale.SetString(Convert.ToString(Scale * 10 * 1000));
                
                //modLibrary.SetField(txtPtouch[IdxRFC].Text, ref ImageInfoRec.rfc_char);											//RFC                'v14.00追加 byやまおか 2007/07/03
                ImageInfoRec.Data.rfc_char.SetString(txtPtouch[IdxRFC].Text);
                
                //modLibrary.SetField(txtPtouch[IdxFilterProc].Text, ref ImageInfoRec.filter_process);							//フィルタ処理       'v14.00追加 byやまおか 2007/07/03
                ImageInfoRec.Data.filter_process.SetString(txtPtouch[IdxFilterProc].Text);

				//ﾃﾞｰﾀ収集時間       'v14.00追加 byやまおか 2007/08/09
				if (string.IsNullOrEmpty(txtPtouch[IdxDataAcq].Text))
				{
					ImageInfoRec.Data.data_acq_time = -1;		//値がなければ-1を書き込む
				}
				else
				{
					float value = 0;
					float.TryParse(txtPtouch[IdxDataAcq].Text, out value);
                    ImageInfoRec.Data.data_acq_time = (value == 0 ? 0.001F : value);		//0は0.001に置換
				}
				//再構成時間         'v14.00追加 byやまおか 2007/08/09
				if (string.IsNullOrEmpty(txtPtouch[IdxReconTime].Text))
				{
                    ImageInfoRec.Data.recon_time = -1;			//値がなければ-1を書き込む
				}
				else
				{
					float value = 0;
					float.TryParse(txtPtouch[IdxReconTime].Text, out value);
                    ImageInfoRec.Data.recon_time = (value == 0 ? 0.001F : value);			//0は0.001に置換
				}

				//v19.00 BHC '(電S2)永井
				//modLibrary.SetField(txtPtouch[IdxBHCTable].Text, ref ImageInfoRec.mbhc_name);
                ImageInfoRec.Data.mbhc_name.SetString(txtPtouch[IdxBHCTable].Text);

				//書き込み
				//functionReturnValue = modImageInfo.WriteImageInfo(ImageInfoRec, myTarget);
                functionReturnValue = ImageInfo.WriteImageInfo(ref ImageInfoRec.Data, myTarget, ".inf");
            
            }
			catch
			{
                Debug.Print("Inf Read Err!!!");
            }

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： 編集ファイル名の指定
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public string Target
		{
			set
			{
				//メンバ変数に記憶
				myTarget = value;

				//表示
				this.Show(frmCTMenu.Instance);
			}
		}


		//*******************************************************************************
		//機　　能： 入力項目の妥当性チェック処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v10.2  2005/05/06 (SI3)間々田    新規作成
		//*******************************************************************************
		private void txtPtouch_Validating(object sender, CancelEventArgs e)
		{
			if (sender as TextBox == null) return;
			int Index = txtPtouch.IndexOf((TextBox)sender);
			if (Index < 0) return;


			//入力文字バイト数チェック
			if (Index == IdxProductName || Index == IdxSliceName)		//v11.4追加 by 間々田 2006/03/14
			{
				return;													//v11.4追加 by 間々田 2006/03/14
			}
			//Case IdxMaxScanArea, IdxImageBias, IdxImageSlope, IdxDirection, IdxFID, IdxFCD
			else if (Index == IdxFOV || Index == IdxImageBias || Index == IdxImageSlope || Index == IdxDirection || Index == IdxFID || Index == IdxFCD)		//v15.0変更 最大スキャンエリア→FOVに変更 2009/07/24 by 間々田
			{ }
			else
			{
                e.Cancel = (Winapi.lstrlen(txtPtouch[Index].Text) > txtPtouch[Index].MaxLength);
			}

			if (e.Cancel)
			{
				//メッセージ表示：入力された%1は、入力文字数の範囲を超えています。
				MessageBox.Show(StringTable.GetResString(9919, lblPtouch[Index].Text), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				//値を元に戻す
				txtPtouch[Index].Text = Convert.ToString(txtPtouch[Index].Tag);
				return;
			}

			//入力必須項目のチェック

			//I.I.視野・システム名・事業所名・コメント
			//Case 9, 12 To 14, 25, 26   'v14.0 25:ﾃﾞｰﾀ収集時間,26:再構成時間を追加 byやまおか 2007/08/09
			//Case IdxIIField, IdxSystemName, IdxWorkshopName, IdxComment, IdxDataAcq, IdxReconTime, IdxRFC  'v14.1変更 27:RFCを追加 byやまおか 2007/08/22
			//v19.00 BHC追加（電S2）永井
			//Case IdxIIField, IdxSystemName, IdxComment, IdxDataAcq, IdxReconTime, IdxRFC 'v15.0変更 事業所名削除 2009/07/24 by 間々田
			if (Index == IdxIIField || Index == IdxSystemName || Index == IdxComment || Index == IdxDataAcq || Index == IdxReconTime || Index == IdxRFC || Index == IdxBHCTable)
			{ }
			//その他
			else
			{
				e.Cancel = (string.IsNullOrEmpty(txtPtouch[Index].Text));
			}

			if (e.Cancel)
			{
				//メッセージ表示：%1は必ず入力してください。
				MessageBox.Show(StringTable.GetResString(9918, lblPtouch[Index].Text), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				//値を元に戻す
				txtPtouch[Index].Text = Convert.ToString(txtPtouch[Index].Tag);
				return;
			}

			//数値のチェック

			//スキャン位置・管電圧・管電流・ビュー数・積算枚数・最大スキャンエリア・スライス厚・画像バイアス・画像スロープ・FID・FCD・ウィンドウレベル・ウィンドウ幅・スケール
			//Case IdxScanPos, IdxTubeVolt, IdxTubeCurrent, IdxViews, IdxIntegNum, IdxMaxScanArea, IdxSliceWidth, IdxImageBias, IdxImageSlope, IdxFID, IdxFCD, IdxWindowLevel, IdxWindowWidth, IdxScale
			if (Index == IdxScanPos || Index == IdxTubeVolt || Index == IdxTubeCurrent || Index == IdxViews || Index == IdxIntegNum || 
				Index == IdxFOV || Index == IdxSliceWidth || Index == IdxImageBias || Index == IdxImageSlope || Index == IdxFID || 
				Index == IdxFCD || Index == IdxWindowLevel || Index == IdxWindowWidth || Index == IdxScale)								//v15.0変更 最大スキャンエリア→FOVに変更 2009/07/24 by 間々田
			{
				double value = 0;
				e.Cancel = !double.TryParse(txtPtouch[Index].Text, out value);
			}
			else if (Index == IdxDataAcq || Index == IdxReconTime)			//v14.00 25:ﾃﾞｰﾀ収集時間,26:再構成時間を追加 byやまおか 2007/08/09
			{
				//表示がなければスルー
				if (!string.IsNullOrEmpty(txtPtouch[Index].Text))
				{
					double value = 0;
					e.Cancel = !double.TryParse(txtPtouch[Index].Text, out value);
					txtPtouch[Index].Text = value.ToString("0.0");			//値を下1桁に書き換える
				}
			}
			else if (Index == IdxGamma)										//v19.00 ガンマを追加 by長野 2012/02/21
			{
				if (!string.IsNullOrEmpty(txtPtouch[Index].Text))
				{
					double value = 0;
					e.Cancel = !double.TryParse(txtPtouch[Index].Text, out value);
					txtPtouch[Index].Text = value.ToString("0.00");
				}
			}

			if (e.Cancel)
			{
				//メッセージ表示：入力された%1には、数値以外の文字が入力されています。
				MessageBox.Show(StringTable.GetResString(9921, lblPtouch[Index].Text), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				//値を元に戻す
				txtPtouch[Index].Text = Convert.ToString(txtPtouch[Index].Tag);
				return;
			}


			//項目に固有のチェック

			//スキャン年月日
			if (Index == IdxScanDate)
			{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				If (Not IsDate(.Text & " " & Time$)) Or (lstrlen(.Text) <> .MaxLength) Then
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				DateTime dt;
				if (!DateTime.TryParse(txtPtouch[Index].Text, out dt) || (Winapi.lstrlen(txtPtouch[Index].Text) != txtPtouch[Index].MaxLength))
				{
					//メッセージ表示：入力された%1には、不正な年/月/日を入力しています。
					MessageBox.Show(StringTable.GetResString(9924, lblPtouch[Index].Text), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
				}
			}
			//スキャン時間
			else if (Index == IdxScanTime)
			{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				If (Not IsDate(Date$ & " " & .Text)) Or (lstrlen(.Text) <> .MaxLength) Then
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				DateTime dt;
                if (!DateTime.TryParse(txtPtouch[Index].Text, out dt) || (Winapi.lstrlen(txtPtouch[Index].Text) != txtPtouch[Index].MaxLength))
				{
					//メッセージ表示：入力された%1には、不正な時/分/秒を入力しています。
					MessageBox.Show(StringTable.GetResString(9922, lblPtouch[Index].Text), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
				}
			}
			//ビュー数
			else if (Index == IdxViews)
			{
				int value = 0;
				int.TryParse(txtPtouch[Index].Text, out value);
                if (!modLibrary.InRange(value, CTSettings.GVal_ViewMin, CTSettings.GVal_ViewMax))
				{
					//メッセージ表示：入力された%1は、入力範囲を超えています。（範囲：%2～%3）
                    MessageBox.Show(StringTable.GetResString(9926, lblPtouch[Index].Text, Convert.ToString(CTSettings.GVal_ViewMin), Convert.ToString(CTSettings.GVal_ViewMax)), 
									Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
				}
			}
			//積算枚数
			else if (Index == IdxIntegNum)
			{
				int value = 0;
				int.TryParse(txtPtouch[Index].Text, out value);
                if (!modLibrary.InRange(value, CTSettings.GValIntegNumMin, CTSettings.GValIntegNumMax))
				{
					//メッセージ表示：入力された%1は、入力範囲を超えています。（範囲：%2～%3）
                    MessageBox.Show(StringTable.GetResString(9926, lblPtouch[Index].Text, Convert.ToString(CTSettings.GValIntegNumMin), Convert.ToString(CTSettings.GValIntegNumMax)), 
									Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
				}
			}
			//スキャンモード
			else if (Index == IdxScanMode)
			{
                //If Not IsScanMode(.text) Then
				//v11.4変更 by 間々田 2006/03/14
				//if (modLibrary.GetIndexByStr(txtPtouch[Index].Text, modCommon.MyCtinfdef.full_mode, -1) < 0)
				if( modCommon.MyCtinfdef.full_mode.GetIndexByStr(txtPtouch[Index].Text,  -1) < 0)
				{
					//メッセージ表示：入力された%1は、不正です。（ｺﾓﾝに登録されていません。）
					MessageBox.Show(StringTable.GetResString(9920, lblPtouch[Index].Text), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
				}
			}
			//画像バイアス・画像スロープ
			else if (Index == IdxImageBias || Index == IdxImageSlope)
			{
				double value = 0;
				double.TryParse(txtPtouch[Index].Text, out value);
				if (value > 65534)
				{
					//メッセージ表示：入力された%1は、入力範囲を超えています。（範囲：%2～%3）
					MessageBox.Show(StringTable.GetResString(9926, lblPtouch[Index].Text, " ", "65534"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
				}
			}
			//FID, FCD
            //変更2014/10/07hata_v19.51反映
            //'FID, FCD, Y軸(AxisName(1))     'v18.00 Y軸追加 byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/17
            //else if (Index == IdxFID || Index == IdxFCD)
            else if (Index == IdxFID || Index == IdxFCD || Index == IdxTableXPos)
			{
                //変更2015/01/23hata
                //int value = 0;
                //int.TryParse(txtPtouch[Index].Text, out value);
                double value = 0;
                double.TryParse(txtPtouch[Index].Text, out value);
				
                if (!modLibrary.InRange(value, 1, 32767))
				{
					//メッセージ表示：入力された%1は、入力範囲を超えています。（範囲：%2～%3）
					MessageBox.Show(StringTable.GetResString(9926, lblPtouch[Index].Text, "1", "32767"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
				}
			}
			//ウィンドウレベル
			else if (Index == IdxWindowLevel)
			{
				int value = 0;
				int.TryParse(txtPtouch[Index].Text, out value);
				if (!modLibrary.InRange(value, -8192, 8191))
				{
					//メッセージ表示：入力された%1は、入力範囲を超えています。（範囲：%2～%3）
					MessageBox.Show(StringTable.GetResString(9926, lblPtouch[Index].Text, "-8192", "8191"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
				}
			}
			//ウィンドウ幅
			else if (Index == IdxWindowWidth)
			{
				int value = 0;
				int.TryParse(txtPtouch[Index].Text, out value);
				if (!modLibrary.InRange(value, 1, 16384))
				{
					//メッセージ表示：入力された%1は、入力範囲を超えています。（範囲：%2～%3）
					MessageBox.Show(StringTable.GetResString(9926, lblPtouch[Index].Text, "1", "16384"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
				}
			}
			//ガンマ補正値 'v19.00 追加 by長野 2012-02-21
			else if (Index == IdxGamma)
			{
				float value = 0;
				float.TryParse(txtPtouch[Index].Text, out value);
				//if (!modLibrary.InRangeFloat(value, 0.1F, 10))
                if (!modLibrary.InRange(value, 0.1F, 10))
                {
					//メッセージ表示：入力された%1は、入力範囲を超えています。（範囲：%2～%3）
					MessageBox.Show(StringTable.GetResString(9926, lblPtouch[Index].Text, "0.1", "10"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
				}
			}
			//スケール
			else if (Index == IdxScale)
			{
				double value = 0;
				double.TryParse(txtPtouch[Index].Text, out value);
				if (!(value > 0 && value < 2048))
				{
					//メッセージ表示：入力された%1は、入力範囲を超えています。（範囲：%2～%3）
					MessageBox.Show(StringTable.GetResString(9926, lblPtouch[Index].Text, "0.001", "2047.999"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
				}
			}
			//データ収集時間     'v14.0追加 byやまおか 2007/08/09
			else if (Index == IdxDataAcq)
			{
				double value = 0;
				double.TryParse(txtPtouch[Index].Text, out value);
				if (!(value >= 0 && value <= 999999))
				{
					//メッセージ表示：入力された%1は、入力範囲を超えています。（範囲：%2～%3）
					MessageBox.Show(StringTable.GetResString(9926, lblPtouch[Index].Text, "0", "999999"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
				}
			}
			//再構成収集時間     'v14.0追加 byやまおか 2007/08/09
			else if (Index == IdxReconTime)
			{
				double value = 0;
				double.TryParse(txtPtouch[Index].Text, out value);
				if (!(value >= 0 && value <= 999999))
				{
					//メッセージ表示：入力された%1は、入力範囲を超えています。（範囲：%2～%3）
					MessageBox.Show(StringTable.GetResString(9926, lblPtouch[Index].Text, "0", "999999"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
				}
			}

			//値を元に戻す
			if (e.Cancel) txtPtouch[Index].Text = Convert.ToString(txtPtouch[Index].Tag);
		}


		//*******************************************************************************
		//機　　能： 項目ごとに表示・非表示の設定、および表示位置の設定を行う
		//
		//           変数名          [I/O] 型        内容
		//引　　数： DispIndex           I           表示位置(表示順)
		//           Index               I           ラベルのIndex
		//           IsVisible           I           表示/非表示の判定式
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v14.00  07/06/26  やまおか     CT20Kv8.2から移植&改良
		//*******************************************************************************
		private int AddItem(string theCaption, string Default, int? MaxLength = null, string ToolTipText = null)
		{
			int itemTop = 0;
			int Index = 0;

			//Indexの最大値
			Index = lblPtouch.Count - 1;

			if (!string.IsNullOrEmpty(lblPtouch[Index].Text))
			{
				//表示位置
				itemTop = lblPtouch[Index].Top + lblPtouch[Index].Height + 5;

				//Indexカウントアップ
				Index = lblPtouch.Count - 1 + 1;

				//項目名用ラベル
				lblPtouch.Add(new Label());
				lblPtouch[Index].AutoSize = lblPtouch0.AutoSize;
				lblPtouch[Index].Font = lblPtouch0.Font;
				lblPtouch[Index].Location = lblPtouch0.Location;
				lblPtouch[Index].Name = "lblPtouch" + Index.ToString();
				lblPtouch[Index].Size = lblPtouch0.Size;
				lblPtouch[Index].Text = "";
				this.Controls.Add(lblPtouch[Index]);

				lblPtouch[Index].Visible = true;
				lblPtouch[Index].Top = itemTop;


				//コロン用ラベル
				lblPtouchColum.Add(new Label());
				lblPtouchColum[Index].AutoSize = lblPtouchColum0.AutoSize;
				lblPtouchColum[Index].Font = lblPtouchColum0.Font;
				lblPtouchColum[Index].Location = lblPtouchColum0.Location;
				lblPtouchColum[Index].Name = "lblPtouchColum" + Index.ToString();
				lblPtouchColum[Index].Size = lblPtouchColum0.Size;
				lblPtouchColum[Index].Text = lblPtouchColum0.Text;
				this.Controls.Add(lblPtouchColum[Index]);

				lblPtouchColum[Index].Visible = true;
				lblPtouchColum[Index].Top = itemTop;


				//項目内容用ラベル
				txtPtouch.Add(new TextBox());
				txtPtouch[Index].Location = txtPtouch0.Location;
				txtPtouch[Index].MaxLength = txtPtouch0.MaxLength;
				txtPtouch[Index].Name = "txtPtouch" + Index.ToString();
				txtPtouch[Index].Size = txtPtouch0.Size;
				txtPtouch[Index].Validating += new System.ComponentModel.CancelEventHandler(this.txtPtouch_Validating);
				this.Controls.Add(txtPtouch[Index]);

				txtPtouch[Index].Visible = true;
				txtPtouch[Index].Top = itemTop;
			}

			//項目名用ラベルに項目名をセット
			lblPtouch[Index].Text = theCaption;


			//デフォルト値
			txtPtouch[Index].Tag = Default;
			txtPtouch[Index].Text = Convert.ToString(txtPtouch[Index].Tag).Trim();

			//最大入力文字数
			if (MaxLength == null)
			{
                txtPtouch[Index].MaxLength = Default.Length;
			}
			else
			{
				txtPtouch[Index].MaxLength = (int)MaxLength;
			}

			//プロダクト名をグレーアウト
			if (txtPtouch[Index].MaxLength == 0)
			{
				txtPtouch[Index].BackColor = SystemColors.Control;
				txtPtouch[Index].ReadOnly = true;
				txtPtouch[Index].TabStop = false;
			}
			else
			{
				txtPtouch[Index].BackColor = SystemColors.Window;
				txtPtouch[Index].ReadOnly = false;
				txtPtouch[Index].TabStop = true;
			}

			//最大入力文字数
			if (!string.IsNullOrEmpty(ToolTipText))
			{
				ToolTip.SetToolTip(txtPtouch[Index], ToolTipText);
			}

			//戻り値セット
			return Index;
		}


		//*******************************************************************************
		//機　　能： コントロールの初期化
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v14.00  07/06/26  やまおか     CT20Kv8.2から移植&改良
		//*******************************************************************************
		private void InitControls()
		{
			//modImageInfo.ReadImageInfo(ref ImageInfoRec, myTarget);
            ImageInfo.ReadImageInfo(ref ImageInfoRec.Data, myTarget);

            //Rev20.00 追加 by長野 2014/12/15
            try
            {
                //プロダクト名：編集は不可
                IdxProductName = AddItem(CTResources.LoadResString(StringTable.IDS_ProductName), Path.GetDirectoryName(myTarget), 0);

                //スライス名：編集は不可
                IdxSliceName = AddItem(CTResources.LoadResString(StringTable.IDS_SliceName), Path.GetFileName(myTarget) + ".img", 0);

                //スキャン位置(mm)
                //変更2015/01/23hata
                //IdxScanPos = AddItem(CTResources.LoadResString(StringTable.IDS_ScanPos) + "(mm)", ImageInfoRec.Data.table_pos.GetString());
                IdxScanPos = AddItem(CTResources.LoadResString(StringTable.IDS_ScanPos) + "(mm)", ImageInfoRec.Data.table_pos.GetString(),ImageInfoRec.Data.table_pos.Buf.Length);

                //スキャン年月日 書式：'YYYY/MM/DD'
                IdxScanDate = AddItem(CTResources.LoadResString(StringTable.IDS_ScanDate), ImageInfoRec.Data.d_date.GetString(), ToolTipText: CTResources.LoadResString(12503));

                //スキャン時刻   書式：'HH:MM:SS'
                IdxScanTime = AddItem(CTResources.LoadResString(StringTable.IDS_ScanTime), ImageInfoRec.Data.start_time.GetString(), 8, CTResources.LoadResString(12501));

                //データ収集時間(秒)
                IdxDataAcq = AddItem(CTResources.LoadResString(StringTable.IDS_DataAcqTime), (ImageInfoRec.Data.data_acq_time > 0 ? ImageInfoRec.Data.data_acq_time.ToString("0.0") : ""), 8);

                //再構成時間(秒)
                IdxReconTime = AddItem(CTResources.LoadResString(StringTable.IDS_ReconTime), (ImageInfoRec.Data.recon_time > 0 ? ImageInfoRec.Data.recon_time.ToString("0.0") : ""), 8);

                //管電圧(kV)
                //変更2015/01/23hata
                //IdxTubeVolt = AddItem(CTResources.LoadResString(StringTable.IDS_TubeVoltage) + "(kV)", ImageInfoRec.Data.volt.GetString());
                IdxTubeVolt = AddItem(CTResources.LoadResString(StringTable.IDS_TubeVoltage) + "(kV)", ImageInfoRec.Data.volt.GetString(), ImageInfoRec.Data.volt.Buf.Length);

                //管電流(uA)
                //変更2015/01/23hata
                //IdxTubeCurrent = AddItem(CTResources.LoadResString(StringTable.IDS_TubeCurrent) + "(" + modXrayControl.CurrentUni + ")", ImageInfoRec.Data.anpere.GetString());
                IdxTubeCurrent = AddItem(CTResources.LoadResString(StringTable.IDS_TubeCurrent) + "(" + modXrayControl.CurrentUni + ")", ImageInfoRec.Data.anpere.GetString(),ImageInfoRec.Data.anpere.Buf.Length);

                //ビュー数
                //変更2015/01/23hata
                //IdxViews = AddItem(CTResources.LoadResString(StringTable.IDS_ViewNum), ImageInfoRec.Data.scan_view.GetString());
                IdxViews = AddItem(CTResources.LoadResString(StringTable.IDS_ViewNum), ImageInfoRec.Data.scan_view.GetString(),ImageInfoRec.Data.scan_view.Buf.Length);

                //積算枚数
                if (modCT30K.IsEnglish)
                {
                    //変更2015/01/23hata
                    //IdxIntegNum = AddItem(CTResources.LoadResString(StringTable.IDS_IntegNum), ImageInfoRec.Data.integ_number.GetString());
                    IdxIntegNum = AddItem(CTResources.LoadResString(StringTable.IDS_IntegNum), ImageInfoRec.Data.integ_number.GetString(),ImageInfoRec.Data.integ_number.Buf.Length);
                }
                else
                {
                    //変更2015/01/23hata
                    //IdxIntegNum = AddItem(CTResources.LoadResString(StringTable.IDS_IntegNum), ImageInfoRec.Data.integ_number.GetString());
                    IdxIntegNum = AddItem(CTResources.LoadResString(StringTable.IDS_IntegNum), ImageInfoRec.Data.integ_number.GetString(), ImageInfoRec.Data.integ_number.Buf.Length);
                }

                //I.I.視野：FPDの場合、ビニングモードを表示する
                //v17.30 条件式を付帯情報から読み込んだ検出器種類に変更 by 長野 2010-09-26
                //        IdxIIField = AddItem(LoadResString(IIf(Use_FlatPanel, IDS_BinningMode, IDS_IIField)), .iifield)
                if (ImageInfoRec.Data.detector == 0)
                {
                    //変更2015/01/23hata
                    //IdxIIField = AddItem(CTResources.LoadResString(StringTable.IDS_IIField), ImageInfoRec.Data.iifield.GetString());
                    IdxIIField = AddItem(CTResources.LoadResString(StringTable.IDS_IIField), ImageInfoRec.Data.iifield.GetString(), ImageInfoRec.Data.iifield.Buf.Length);

                    //        Else
                    //            IdxIIField = AddItem(LoadResString(IDS_BinningMode), .iifield)
                }
                //        IdxIIField = AddItem(LoadResString(IIf(Use_FlatPanel, IDS_BinningMode, IDS_IIField)), .iifield)

                //'最大ｽｷｬﾝｴﾘｱ(mm)
                //IdxMaxScanArea = AddItem(LoadResString(IDS_MaxScanArea) & "(mm)", .mscan_area, 8)

                //FOV(mm)                                                                'v15.0変更 最大スキャンエリア→FOVに変更 2009/07/24 by 間々田
                IdxFOV = AddItem("FOV(mm)", ImageInfoRec.Data.mscan_area.ToString(), 8);

                //スライス厚(mm)
                //変更2015/01/23hata
                //IdxSliceWidth = AddItem(CTResources.LoadResString(StringTable.IDS_SliceWidth) + "(mm)", ImageInfoRec.Data.width.GetString());
                IdxSliceWidth = AddItem(CTResources.LoadResString(StringTable.IDS_SliceWidth) + "(mm)", ImageInfoRec.Data.width.GetString(), ImageInfoRec.Data.width.Buf.Length);

                //システム名
                //変更2015/01/23hata
                //IdxSystemName = AddItem(CTResources.LoadResString(StringTable.IDS_SystemName), ImageInfoRec.Data.system_name.GetString());
                IdxSystemName = AddItem(CTResources.LoadResString(StringTable.IDS_SystemName), ImageInfoRec.Data.system_name.GetString(), ImageInfoRec.Data.system_name.Buf.Length);

                //'事業所名
                //IdxWorkshopName = AddItem(LoadResString(IDS_WorkShopName), .workshop)  'v15.0削除 表示しないことになった 2009/07/24 by 間々田

                //コメント
                //変更2015/01/23hata
                //IdxComment = AddItem(CTResources.LoadResString(StringTable.IDS_Comment), ImageInfoRec.Data.comment.GetString());
                IdxComment = AddItem(CTResources.LoadResString(StringTable.IDS_Comment), ImageInfoRec.Data.comment.GetString(),ImageInfoRec.Data.comment.Buf.Length);

                //スキャンモード
                //変更2015/01/23hata
                //IdxScanMode = AddItem(CTResources.LoadResString(StringTable.IDS_ScanMode), ImageInfoRec.Data.full_mode.GetString());
                IdxScanMode = AddItem(CTResources.LoadResString(StringTable.IDS_ScanMode), ImageInfoRec.Data.full_mode.GetString(), ImageInfoRec.Data.full_mode.Buf.Length);

                //フィルタ関数
                //変更2015/01/23hata
                //IdxFilterFunc = AddItem(CTResources.LoadResString(StringTable.IDS_FilterFunc), ImageInfoRec.Data.fc.GetString());
                IdxFilterFunc = AddItem(CTResources.LoadResString(StringTable.IDS_FilterFunc), ImageInfoRec.Data.fc.GetString(),ImageInfoRec.Data.fc.Buf.Length);

                //フィルタ処理
                if ((CTSettings.scaninh.Data.filter_process[0] == 0) && (CTSettings.scaninh.Data.filter_process[1] == 0))
                {
                    //変更2015/01/23hata
                    //IdxFilterProc = AddItem(CTResources.LoadResString(StringTable.IDS_FilterProc), ImageInfoRec.Data.filter_process.GetString());
                    IdxFilterProc = AddItem(CTResources.LoadResString(StringTable.IDS_FilterProc), ImageInfoRec.Data.filter_process.GetString(),ImageInfoRec.Data.filter_process.Buf.Length);
                }

                //RFC
                if (CTSettings.scaninh.Data.rfc == 0)
                {
                    //変更2015/01/23hata
                    //IdxRFC = AddItem(CTResources.LoadResString(StringTable.IDS_RFC), ImageInfoRec.Data.rfc_char.GetString());
                    IdxRFC = AddItem(CTResources.LoadResString(StringTable.IDS_RFC), ImageInfoRec.Data.rfc_char.GetString(),ImageInfoRec.Data.rfc_char.Buf.Length);
                }

                //v19.00 BHC（電S2）永井
                //BHCファイル
                if (CTSettings.scaninh.Data.mbhc == 0)
                {
                    //変更2015/01/23hata
                    //IdxBHCTable = AddItem(CTResources.LoadResString(StringTable.IDS_BHCFile), ImageInfoRec.Data.mbhc_name.GetString());
                    IdxBHCTable = AddItem(CTResources.LoadResString(StringTable.IDS_BHCFile), ImageInfoRec.Data.mbhc_name.GetString(), ImageInfoRec.Data.mbhc_name.Buf.Length);
                }

                //画像バイアス
                IdxImageBias = AddItem(CTResources.LoadResString(StringTable.IDS_ImageBias), ImageInfoRec.Data.image_bias.ToString(), 6);

                //画像スロープ
                IdxImageSlope = AddItem(CTResources.LoadResString(StringTable.IDS_ImageSlope), ImageInfoRec.Data.image_slope.ToString(), 8);

                //断面像方向 書式：'TOP','BOTTOM
                IdxDirection = AddItem(CTResources.LoadResString(StringTable.IDS_ImageDirection), (ImageInfoRec.Data.image_direction == 0 ? "TOP" : "BOTTOM"), 6, CTResources.LoadResString(12502));

                //FID
                IdxFID = AddItem(CTResources.LoadResString(StringTable.IDS_FID), (ImageInfoRec.Data.fid - ImageInfoRec.Data.fid_offset).ToString("0.000"), 6);

                //FCD
                IdxFCD = AddItem(CTResources.LoadResString(StringTable.IDS_FCD), (ImageInfoRec.Data.fcd - ImageInfoRec.Data.fcd_offset).ToString("0.000"), 6);

                //ウィンドウレベル
                //変更2015/01/23hata
                //IdxWindowLevel = AddItem(CTResources.LoadResString(StringTable.IDS_WindowLevel), ImageInfoRec.Data.wl.GetString());
                IdxWindowLevel = AddItem(CTResources.LoadResString(StringTable.IDS_WindowLevel), ImageInfoRec.Data.wl.GetString(),ImageInfoRec.Data.wl.Buf.Length);

                //ウィンドウ幅
                //変更2015/01/23hata
                //IdxWindowWidth = AddItem(CTResources.LoadResString(StringTable.IDS_WindowWidth), ImageInfoRec.Data.ww.GetString());
                IdxWindowWidth = AddItem(CTResources.LoadResString(StringTable.IDS_WindowWidth), ImageInfoRec.Data.ww.GetString(), ImageInfoRec.Data.ww.Buf.Length);

                //ガンマ補正値 'v19.00 追加 by長野 2012-02-21
                //IdxGamma = AddItem(CTResources.LoadResString(StringTable.IDS_Gamma), Convert.ToDouble(ImageInfoRec.Data.gamma).ToString("0.00"), 5);
                //Rev20.00 修正 by長野 2014/12/15
                IdxGamma = AddItem(CTResources.LoadResString(StringTable.IDS_Gamma), ImageInfoRec.Data.gamma.GetString(),5);

                //スケール(mm)
                double scale = 0;
                double.TryParse(ImageInfoRec.Data.scale.GetString(), out scale);
                //変更2015/01/23hata
                //IdxScale = AddItem(CTResources.LoadResString(StringTable.IDS_Scale), (scale / 10 / 1000).ToString(), ImageInfoRec.Data.scale.GetString().Length);
                IdxScale = AddItem(CTResources.LoadResString(StringTable.IDS_Scale), (scale / 10 / 1000).ToString(), ImageInfoRec.Data.scale.Buf.Length);
            }
            catch
            {
                Debug.Print("InitControls Err!!!");
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
		//履　　歴： V17.60  11/05/25  (検S１)長野      新規作成
		//*******************************************************************************
		private void EnglishAdjustLayout()
		{
            //2014/11/07hata キャストの修正
            //this.Width = (int)(this.Width * 1.15);
            this.Width = Convert.ToInt32(this.Width * 1.15);

			lblPtouch0.AutoSize = true;
            //2014/11/07hata キャストの修正
            //lblPtouch0.Width = (int)(lblPtouch0.Width * 1.1);
            lblPtouch0.Width = Convert.ToInt32(lblPtouch0.Width * 1.1);

			lblPtouchColum0.Left = 190;
			txtPtouch0.Left = 200;

            //2014/11/07hata キャストの修正
            //cmdPtouchOK.Left = (int)((this.ClientRectangle.Width - cmdPtouchOK.Width - cmdPtouchCancel.Width) / 3);
            //cmdPtouchCancel.Left = (cmdPtouchOK.Left + cmdPtouchOK.Width) 
            //                     + (int)((this.ClientRectangle.Width - cmdPtouchOK.Width - cmdPtouchCancel.Width) / 3);
            cmdPtouchOK.Left = Convert.ToInt32((this.ClientRectangle.Width - cmdPtouchOK.Width - cmdPtouchCancel.Width) / 3F);
            cmdPtouchCancel.Left = (cmdPtouchOK.Left + cmdPtouchOK.Width)
                                 + Convert.ToInt32((this.ClientRectangle.Width - cmdPtouchOK.Width - cmdPtouchCancel.Width) / 3F);

        }


	}
}
