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
using CT30K.Common;
using CTAPI;
using TransImage;
//using CT30K.Controls;
//using CT30K.Modules;
using CT30K.Properties;

using System.Text.RegularExpressions; //Rev21.00 追加 by長野 2015/03/08

namespace CT30K
{
	public partial class frmImageInfo : Form
	{
		//private modImageInfo.ImageInfoStruct myInfoRec = default(modImageInfo.ImageInfoStruct);		//現在表示している付帯情報レコード	
        private ImageInfo myInfoRec;
        private string myTarget = string.Empty;										//現在表示している付帯情報のファイル名（拡張子なし）
		private bool myDetailOn = false;

		//項目のインデクスを格納する変数
		private int IdxScanPos = 0;			//スキャン位置
		private int IdxScanDate = 0;		//スキャン年月日
		private int IdxScanTime = 0;		//スキャン時刻
		private int IdxDataAcq = 0;			//データ収集時間
		private int IdxReconTime = 0;		//再構成時間
		private int IdxTubeVolt = 0;		//管電圧
		private int IdxTubeCurrent = 0;		//管電流
        private int IdxXfocus = 0;          //焦点       'v18.00追加 byやまおか 2011/03/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05   //追加2014/10/07hata_v19.51反映
        private int IdxXfilter = 0;         //X線フィルタ'v18.00追加 byやまおか 2011/03/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05    //追加2014/10/07hata_v19.51反映
		private int IdxViews = 0;			//ビュー数
		private int IdxIntegNum = 0;		//積算枚数
		private int IdxIIField = 0;			//I.I.視野
		//Dim IdxMaxScanArea  As Integer      '最大スキャンエリア
		private int IdxFOV = 0;				//FOV                    'v15.0変更 最大スキャンエリア→FOVに変更 2009/07/24 by 間々田
		private int IdxSliceWidth = 0;		//スライス厚
		private int IdxSystemName = 0;		//システム名
		private int IdxMatrix = 0;			//画像サイズ
		//Dim IdxWorkshopName As Integer      '事業所名              'v15.0削除 表示しないことになった 2009/07/24 by 間々田
		private int IdxComment = 0;			//コメント
		private int IdxScanMode = 0;		//スキャンモード
		private int IdxFilterFunc = 0;		//フィルタ関数
		private int IdxFilterProc = 0;		//フィルタ処理
		private int IdxRFC = 0;				//RFC
		private int IdxBHCTable = 0;		//v19.00 BHCテーブル(電S2)永井
		private int IdxImageBias = 0;		//画像バイアス
		private int IdxImageSlope = 0;		//画像スロープ
		//Dim IdxConebeam     As Integer      'コーンビーム  'v15.10削除 byやまおか 2009/11/26
		private int IdxDataMode = 0;		//データモード   'v15.10追加 byやまおか 2009/11/26
		private int IdxDirection = 0;		//断面像方向
		private int IdxFID = 0;				//FID
		private int IdxFCD = 0;				//FCD
        private int IdxTableXPos = 0;       //Y軸(AxisName(1))  'v18.00追加 byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05 //追加2014/10/07hata_v19.51反映
		private int IdxWindowLevel = 0;		//ウィンドウレベル
		private int IdxWindowWidth = 0;		//ウィンドウ幅
		private int IdxPixelSize = 0;		//1画素サイズ
		private int IdxMagnify = 0;			//拡大倍率
		private int IdxScale = 0;			//スケール
		private int IdxFpdGain = 0;			//FPDゲイン      'v17.00追加 byやまおか 2010/02/17
		private int IdxFpdInteg = 0;		//FPD積分時間    'v17.00追加 byやまおか 2010/02/17
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		Dim IdxGamma        As Single       'ガンマ補正値   'v19.00追加 by長野     2012/02/21
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
		private int IdxGamma = 0;			//ガンマ補正値   'v19.00追加 by長野     2012/02/21

        private int IdxTableRotation = 0;   //Rev20.00 追加 by長野 テーブル回転(0:ステップ、1:連続)

        private int IdxMultiTube = 0;       //Rev23.10 追加 by長野 2015/11/04 X線タイプ

        private int IdxW_Scan = 0;          //Rev25.00 WスキャンON by長野 2016/07/05
        private int IdxBHCPhantomless = 0;  //Rev26.00 ファントムレスBHC表示 by井上 2017/01/19

		private List<Label> lblColon = null;
		private List<Label> lblContext = null;
		private List<Label> lblItem = null;

		private static frmImageInfo _Instance = null;

		public frmImageInfo()
		{
			InitializeComponent();

			lblColon = new List<Label> { null, lblColon1 };
			lblContext = new List<Label> { null, lblContext1 };
			lblItem = new List<Label> { null, lblItem1 };
		}

		public static frmImageInfo Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmImageInfo();
				}

				return _Instance;
			}
		}


		//*******************************************************************************
		//機　　能： 「詳細」（または「簡易」）ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		//Private Sub mnuDetailMode_Click()
		private void cmdDetailMode_Click(object sender, EventArgs e)		//コマンドボタンに変更 by 間々田 2009/07/21
		{
			DetailOn = !DetailOn;
		}


		//*******************************************************************************
		//機　　能： DetailOnプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 詳細・簡易表示用
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private bool DetailOn
		{
			get { return myDetailOn; }
			set
			{
				myDetailOn = value;

				//mnuDetailMode.Caption = IIf(myDetailOn, "簡易表示", "詳細表示")
				//ストリングテーブル化 'v17.60 by長野 2011/05/22
				//cmdDetailMode.Caption = IIf(myDetailOn, "<<簡易表示", "詳細表示>>") 'コマンドボタンに変更 by 間々田 2009/07/21
				cmdDetailMode.Text = (myDetailOn ? CTResources.LoadResString(12086) : CTResources.LoadResString(12093));

				int Index = 0;
				int itemTop = 0;
				bool IsVisible = false;

				int newTop = 0;			//v15.10追加 byやまおか 2009/11/26
				int newHeight = 0;		//v15.10追加 byやまおか 2009/11/26
				int oldTop = 0;			//v15.10追加 byやまおか 2009/11/26
				int oldHeight = 0;		//v15.10追加 byやまおか 2009/11/26

				int LBound = 0;
				for (LBound = 0; LBound < lblItem.Count; LBound++)
				{
					if (lblItem[LBound] != null) break;
				}

				itemTop = lblItem[LBound].Top;

				for (Index = LBound; Index <= lblItem.Count - 1; Index++)
				{
					//表示するか？
					IsVisible = (myDetailOn || Convert.ToBoolean(lblItem[Index].Tag));

					lblItem[Index].Visible = IsVisible;
					lblItem[Index].Top = itemTop;

					//同じ行の他の項目にも反映
					lblColon[Index].Visible = IsVisible;
					lblColon[Index].Top = itemTop;
					lblContext[Index].Visible = IsVisible;
					lblContext[Index].Top = itemTop;

					//次に表示する位置
					if (IsVisible) itemTop = itemTop + lblItem[Index].Height + 1;
				}

				//スケールバー
				//fraScale.Visible = myDetailOn      '削除 by 間々田 2009/07/21

				oldTop = this.Top;				//v15.10追加 byやまおか 2009/11/26
				oldHeight = this.Height;		//v15.10追加 byやまおか 2009/11/26
				this.Hide();					//v15.10追加 byやまおか 2009/11/26

				//フォームの高さ調整
				//Me.Height = (28 + itemTop + IIf(myDetailOn, fraScale.Height + 4, 0)) * Screen.TwipsPerPixelY
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				//Me.Height = (32 + itemTop + fraScale.Height) * Screen.TwipsPerPixelY '変更 by 間々田 2009/07/21
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
				this.Height = 32 + itemTop + fraScale.Height;				//変更 by 間々田 2009/07/21

				//フォーム高さ調整で画面をはみ出すときは位置移動
				newHeight = this.Height;					//v15.10追加 byやまおか 2009/11/26
				newTop = oldTop + (oldHeight - newHeight);	//v15.10追加 byやまおか 2009/11/26
				this.Top = (newTop < 0 ? 0 : newTop);		//v15.10追加 byやまおか 2009/11/26
				this.Show();								//v15.10追加 byやまおか 2009/11/26
			}
		}


		//*******************************************************************************
		//機　　能： Targetプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 現在表示中のファイル名（拡張子なし）
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public string Target
		{
			get { return myTarget; }
			set
			{
				int i = 0;
				//int j = 0;

                //Rev21.00 スキャノかどうかのフラグ 2015/03/08
                bool scanoflg = false;
                string BaseName = null;

				if (value != myTarget)
				{
					int LBound = 0;
					for (LBound = 0; LBound < lblContext.Count; LBound++)
					{
						if (lblContext[LBound] != null) break;
					}

					if (string.IsNullOrEmpty(myTarget))
					{
						for (i = LBound; i <= lblContext.Count - 1; i++)
						{
							lblContext[i].Enabled = true;
						}
					}
					else if (string.IsNullOrEmpty(value))
					{
						for (i = LBound; i <= lblContext.Count - 1; i++)
						{
							lblContext[i].Text = "";
							lblContext[i].Enabled = false;
						}
					}
				}

				//    For j = lblItem.LBound To lblItem.UBound
				//
				//        '項目名用ラベル
				//        Unload lblItem(j)
				//
				//    Next j

				myTarget = value;

				//詳細・簡易ボタンの使用可能・不可の制御     '追加 by 間々田 2009/07/21
				cmdDetailMode.Enabled = (!string.IsNullOrEmpty(myTarget));

				//ヌルが設定された場合
				if (string.IsNullOrEmpty(myTarget)) return;

                //Rev21.00 スキャノ画像であるかの判定 by長野 2015/02/24
                //ベース名取得
                BaseName = Path.GetFileNameWithoutExtension(myTarget);

                //スキャノの場合、スキャノ画像であることのフラグを立てる
                if (Regex.IsMatch(BaseName.ToLower(), @".+-s\d{4}$"))
                {
                    scanoflg = true;
                }

				//画像情報ファイルの読み込み
				//if (!modImageInfo.ReadImageInfo(ref myInfoRec, myTarget))
                if (!ImageInfo.ReadImageInfo(ref myInfoRec.Data, myTarget))
                {
				    return;
				}

				//デフォルトパス名の設定
				modFileIO.SaveDefaultFolder(CTResources.LoadResString(StringTable.IDS_CTImage), Path.GetDirectoryName(myTarget));

                //Rev21.00 生データ選択・スキャン条件ファイルのデフォルト名も変更する by長野 2015/03/13
                modFileIO.SaveDefaultFolder(CTResources.LoadResString(StringTable.IDS_RawFile), Path.GetDirectoryName(myTarget));
                modFileIO.SaveDefaultFolder(CTResources.LoadResString(StringTable.IDS_ConeRawFile), Path.GetDirectoryName(myTarget));

                //途中エラーが発生しても処理を続行する
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				On Error Resume Next
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				try
				{
                    //Rev20.00 追加 by長野 2015/01/29
                    float info_ver = 0;         // 19.50 追加 by長野 2013/11/20 '内容を表示するかのフラグ   //追加2014/10/07hata_v19.51反映
                    //Rev20.00 追加 by長野 2015/01/29 
                    info_ver = modImageInfo.GetImageInfoVerNumber(myInfoRec.Data.version.GetString());
                    
                    lblContext[IdxScanPos].Text = myInfoRec.Data.d_tablepos.GetString();							//スキャン位置（絶対座標）
                    lblContext[IdxScanDate].Text = myInfoRec.Data.d_date.GetString();							//スキャン年月日
                    lblContext[IdxScanTime].Text = myInfoRec.Data.start_time.GetString();						//スキャン時刻

                    if (IdxMultiTube > 0) //Rev23.10 追加 by長野 2015/11/04 
                    {
                        lblContext[IdxMultiTube].Text = myInfoRec.Data.tube.GetString();
                    }
                    
                    double volt = 0;
                    double.TryParse(myInfoRec.Data.volt.GetString(), out volt);
					lblContext[IdxTubeVolt].Text = volt.ToString("0.0");									//管電圧(kV)
					double anpere = 0;
                    double.TryParse(myInfoRec.Data.anpere.GetString(), out anpere);
					lblContext[IdxTubeCurrent].Text = anpere.ToString("0.000");								//管電流(μA)

                    //Rev20.00 条件式追加 by長野 2014/11/04
                    if (IdxXfocus > 0)
                    {
                        lblContext[IdxXfocus].Text = myInfoRec.Data.xfocus_c.GetString();                       //焦点       'v18.00追加 byやまおか 2011/06/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05 //追加2014/10/07hata_v19.51反映
                    }
                    if (IdxXfilter > 0)
                    {
                        lblContext[IdxXfilter].Text = myInfoRec.Data.xfilter_c.GetString();                     //フィルタ   'v18.00追加 byやまおか 2011/06/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05 //追加2014/10/07hata_v19.51反映
                    }

                    //lblContext[IdxViews].Text = myInfoRec.Data.scan_view.GetString().Trim();						//ビュー数
                    //lblContext[IdxIntegNum].Text = myInfoRec.Data.integ_number.GetString().Trim();				//積算枚数
                    //Rev21.00 条件追加 by長野 2015/02/24
                    if (scanoflg == false)
                    {
                        lblContext[IdxViews].Text = myInfoRec.Data.scan_view.GetString().Trim();			//ビュー数
                        //Rev21.00 移動 by長野 2015/03/16
                        lblContext[IdxIntegNum].Text = myInfoRec.Data.integ_number.GetString().Trim();				//積算枚数
                    }
                    else
                    {
                        lblContext[IdxViews].Text = myInfoRec.Data.mscanopt.ToString();						//ビュー数
                        //Rev21.00 移動 by長野 2015/03/16
                        lblContext[IdxIntegNum].Text = myInfoRec.Data.mscano_integ_number.GetString().Trim();				//積算枚数
                    }

                    //Rev20.00 追加 by長野 2015/01/29
                    //Rev21.00 条件変更 by長野 2015/03/09
                    //if (info_ver >= 20.00)
                    if(info_ver >= 20.00 && scanoflg == false)
                    {
                        if (myInfoRec.Data.table_rotation == 0)
                        {
                            lblContext[IdxTableRotation].Text = CTResources.LoadResString(12383);
                        }
                        else
                        {
                            lblContext[IdxTableRotation].Text = CTResources.LoadResString(12386);
                        }
                    }
                    //else //Rev21.00 条件式変更 by長野 2015/03/09
                    else if (info_ver < 20.00 || scanoflg == true)
                    {
                        lblContext[IdxTableRotation].Text = "";
                    }
					
                    //v17.30 付帯情報から読み込んだ検出器種類に変更 by 長野 2010-09-26
					//        If (DetType = DetTypeII) Or (DetType = DetTypeHama) Then  'v17.00修正 byやまおか 2010/03/04
					//            lblContext(IdxIIField).Caption = .iifield                           'I.I.視野
					//        End If
                    //Rev20.00 AddItemsの条件に合わせる by長野 2014/11/10
                    //if ((myInfoRec.Data.detector == 0) || (myInfoRec.Data.detector == 1))
                    if(IdxIIField>0)
                    {
                    lblContext[IdxIIField].Text = myInfoRec.Data.iifield.GetString();						//I.I.視野
                    }
                    //else
                    //{
                    //    lblContext[IdxIIField].Text = "";													//I.I.視野
                    //}
					//lblContext(IdxMaxScanArea).Caption = Format$(.max_mscan_area, "0.000")  '最大スキャンエリア(mm)
                    //Rev21.00 条件式追加 by長野 2015/02/24
                    if (scanoflg == false)
                    {
                        lblContext[IdxFOV].Text = myInfoRec.Data.mscan_area.ToString("0.000");						//FOV(mm)                　  'v15.0変更 最大スキャンエリア→FOV、max_mscan_area→mscan_areaに変更 2009/07/24 by 間々田
                        double width = 0;
                        double.TryParse(myInfoRec.Data.width.GetString(), out width);
                        lblContext[IdxSliceWidth].Text = width.ToString("0.000");		 //スライス厚(mm)
                    }
                    else
                    {
                        double width = 0;
                        double.TryParse(myInfoRec.Data.mscano_width.GetString(), out width);
                        lblContext[IdxSliceWidth].Text = width.ToString("0.000");		 //スライス厚(mm)
                        lblContext[IdxFOV].Text = myInfoRec.Data.mscano_area.ToString("0.000");						//FOV(mm)                　  'v15.0変更 最大スキャンエリア→FOV、max_mscan_area→mscan_areaに変更 2009/07/24 by 間々田
                    }

                    lblContext[IdxSystemName].Text = myInfoRec.Data.system_name.GetString();									//システム名
                    lblContext[IdxMatrix].Text = myInfoRec.Data.matsiz.GetString();											//画像サイズ
					//lblContext(IdxWorkshopName).Caption = .workshop                         '事業所名                  'v15.0削除 表示しないことになった 2009/07/24 by 間々田
					//lblContext(IdxComment).Caption = Trim$(.Comment) & Space$(20)           'コメント
                    lblContext[IdxComment].Text = modLibrary.RemoveNull(myInfoRec.Data.comment.GetString()) + new string(' ', 20);	//コメント                   'v15.10変更 byやまおか 2009/12/03

                    //lblContext[IdxScanMode].Text = myInfoRec.Data.full_mode.GetString();												//スキャンモード

                    //Rev21.00 条件式追加 by長野 2015/02/24
                    if (scanoflg == false)
                    {
                        lblContext[IdxFilterFunc].Text = myInfoRec.Data.fc.GetString();													//フィルタ関数
                        //lblContext[IdxW_Scan].Text = myInfoRec.Data.w_scan == 1 ? "ON" : "OFF";             //Wスキャン(0:なし,1:あり,-:スキャノ)Rev25.00 追加 by長野 2016/07/05
                        //if (CTSettings.scaninh.Data.w_scan == 0) lblContext[IdxW_Scan].Text = myInfoRec.Data.w_scan == 1 ? "ON" : "OFF";             //Wスキャン(0:なし,1:あり,-:スキャノ)Rev25.00 追加 by長野 2016/07/05 //Rev25.10 修正 by chouno 2017/09/12
                        //Rev26.10 change シフトスキャンの名称が"Wスキャン"の場合はWスキャン表示 by chouno 2018/01/16
                        if (CTSettings.scaninh.Data.w_scan == 0 || CTSettings.infdef.Data.scan_mode[3].GetString() == CTResources.LoadResString(25009))
                        {
                            //lblContext[IdxW_Scan].Text = myInfoRec.Data.w_scan == 1 ? "ON" : "OFF";             //Wスキャン(0:なし,1:あり,-:スキャノ)Rev25.00 追加 by長野 2016/07/05 //Rev25.10 修正 by chouno 2017/09/12
                            if (myInfoRec.Data.w_scan == 1 || myInfoRec.Data.full_mode.GetString() == CTSettings.ctinfdef.Data.full_mode[3].GetString())
                            {
                                lblContext[IdxW_Scan].Text = "ON";            
                            }
                            else
                            {
                                lblContext[IdxW_Scan].Text = "OFF";
                            }
                        }
                        lblContext[IdxScanMode].Text = myInfoRec.Data.full_mode.GetString();				//スキャンモード
                        lblContext[IdxImageBias].Text = myInfoRec.Data.image_bias.ToString("0.0");							//画像バイアス
                        lblContext[IdxImageSlope].Text = myInfoRec.Data.image_slope.ToString("0.0");						//画像スロープ
                        //lblContext(IdxConebeam).Caption = IIf(.bhc = 1, LoadResString(IDS_ConeBeam), "") 'コーンビーム 1バイト　1:"コーンビーム", 1以外:""
                        lblContext[IdxDataMode].Text = (myInfoRec.Data.bhc == 1 ? CTResources.LoadResString(StringTable.IDS_ConeBeam) : CTResources.LoadResString(StringTable.IDS_Scan));	//データモード 1バイト　1:"コーンビーム", 1以外:"スキャン"  'v15.10変更 byやまおか 2009/11/26

                        //断画像方向 8バイト 番号を文字化する  1:"下から見た画像", 1以外:"上から見た画像"
                        lblContext[IdxDirection].Text = CTResources.LoadResString(myInfoRec.Data.image_direction == 1 ? StringTable.IDS_DirectionBottom : StringTable.IDS_DirectionTop);
                        lblContext[IdxDirection].Text = lblContext[IdxDirection].Text.Replace(" view", "");		        //英語環境対策
                    }
                    else
                    {
                        lblContext[IdxFilterFunc].Text = "－";													//フィルタ関数
                        if (CTSettings.scaninh.Data.w_scan == 0) lblContext[IdxW_Scan].Text = "－";                                                 //Wスキャン(0:なし,1:あり,-:スキャノ) Rev25.00 追加 by長野 2016/07/05 //Rev25.10 修正 by chouno 2017/09/12
                        lblContext[IdxScanMode].Text = "－";												//スキャンモード
                        lblContext[IdxImageBias].Text = myInfoRec.Data.mscano_bias.ToString("0.0");							//画像バイアス
                        lblContext[IdxImageSlope].Text = myInfoRec.Data.mscano_slope.ToString("0.0");						//画像スロープ
                        lblContext[IdxDataMode].Text = CTResources.LoadResString(StringTable.IDS_Scano);

                        //断画像方向 8バイト 番号を文字化する  1:"下から見た画像", 1以外:"上から見た画像"
                        lblContext[IdxDirection].Text = "－";
                    }

                    //変更2014/10/07hata_v19.51反映
                    //lblContext[IdxFID].Text = (myInfoRec.Data.fid - myInfoRec.Data.fid_offset).ToString("0.0");		        //FID
                    //Rev23.10 計測CT対応 by長野 2015/10/18
                    if (CTSettings.scaninh.Data.cm_mode == 0)
                    {
                        //lblContext[IdxFCD].Text = (myInfoRec.Data.fcd - myInfoRec.Data.fcd_offset).ToString("0.000");             //FCD 'v18.00変更 FCD⇔FID入れ替え byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから
                        //lblContext[IdxFID].Text = (myInfoRec.Data.fid - myInfoRec.Data.fid_offset).ToString("0.000");             //FID 'v18.00変更 FCD⇔FID入れ替え byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから
                        //lblContext[IdxTableXPos].Text = (myInfoRec.Data.table_x_pos).ToString("0.000");                          //Y軸(AxisName(1))  'v18.00追加 byやまおか 2011/07/30　'V19,50 v19.41とv18.02の統合 by長野 2013/11/17
                        //Rev26.14 change by chouno 2018/09/05
                        lblContext[IdxFCD].Text = (myInfoRec.Data.fcd - myInfoRec.Data.fcd_offset).ToString("0.00");             //FCD 'v18.00変更 FCD⇔FID入れ替え byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから
                        lblContext[IdxFID].Text = (myInfoRec.Data.fid - myInfoRec.Data.fid_offset).ToString("0.00");             //FID 'v18.00変更 FCD⇔FID入れ替え byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから
                        lblContext[IdxTableXPos].Text = (myInfoRec.Data.table_x_pos).ToString("0.00");                          //Y軸(AxisName(1))  'v18.00追加 byやまおか 2011/07/30　'V19,50 v19.41とv18.02の統合 by長野 2013/11/17
                    }
                    else
                    {
                        lblContext[IdxFCD].Text = (myInfoRec.Data.fcd - myInfoRec.Data.fcd_offset).ToString("0.0");             //FCD 'v18.00変更 FCD⇔FID入れ替え byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから
                        lblContext[IdxFID].Text = (myInfoRec.Data.fid - myInfoRec.Data.fid_offset).ToString("0.0");             //FID 'v18.00変更 FCD⇔FID入れ替え byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから
                        lblContext[IdxTableXPos].Text = (myInfoRec.Data.table_x_pos).ToString("0.00");                          //Y軸(AxisName(1))  'v18.00追加 byやまおか 2011/07/30　'V19,50 v19.41とv18.02の統合 by長野 2013/11/17
                    }
                    lblContext[IdxWindowLevel].Text = myInfoRec.Data.wl.GetString();										//ウィンドウレベル
                    lblContext[IdxWindowWidth].Text = myInfoRec.Data.ww.GetString();										//ウィンドウ幅
					
                    //ガンマ補正値 追加 by長野 2012/02/21
					double gamma = 0;
                    double.TryParse(myInfoRec.Data.gamma.GetString(), out gamma);
					if (gamma == 0)
					{
                        myInfoRec.Data.gamma.SetString("1.00");     //ガンマ補正値を持たない旧バージョンの画像は初期値1とする
                        gamma = 1.00;
                    }
                    //lblContext[IdxGamma].Text = string.Format(myInfoRec.Data.gamma.GetString(),"0.00");
                    lblContext[IdxGamma].Text = string.Format("{0:0.00}", gamma);
                    lblContext[IdxDataAcq].Text = (myInfoRec.Data.data_acq_time <= 0 ? "" : myInfoRec.Data.data_acq_time.ToString("0.0"));	//ﾃﾞｰﾀ収集時間 追加 byやまおか 2007/08/09

                    //Rev21.00 条件追加 by長野 2015/02/26
                    if (scanoflg == false)
                    {
                        lblContext[IdxReconTime].Text = (myInfoRec.Data.recon_time <= 0 ? "" : myInfoRec.Data.recon_time.ToString("0.0"));		//再構成時間   追加 byやまおか 2007/08/09

                        if (IdxRFC > 0)
                        {
                            lblContext[IdxRFC].Text = myInfoRec.Data.rfc_char.GetString().Trim();					//RFC 追加 byやまおか 2007/06/26
                        }

                        //v19.00 BHC　(電S2)永井
                        if (IdxBHCTable > 0)
                        {
                            if (myInfoRec.Data.mbhc_flag == 1)
                            {
                                lblContext[IdxBHCTable].Text = modLibrary.AddExtension(modLibrary.RemoveNull(myInfoRec.Data.mbhc_name.GetString()), ".csv");
                            }
                            else
                            {
                                lblContext[IdxBHCTable].Text = "";
                            }
                        }

                        //Rev26.00 ファントムレスBHC 追加 by 井上 2017/02/11
                        if (IdxBHCPhantomless > 0)
                        {
                            lblContext[IdxBHCPhantomless].Text = myInfoRec.Data.mbhc_phantomless_c.GetString();
                        }

                        if (IdxFilterProc > 0)
                        {
                            lblContext[IdxFilterProc].Text = myInfoRec.Data.filter_process.GetString().Trim();		//フィルタ処理 追加 byやまおか 2007/06/26
                        }
                    }
                    else
                    {
                        lblContext[IdxReconTime].Text = "－";		//再構成時間   追加 byやまおか 2007/08/09

                        if (IdxRFC > 0)
                        {
                            lblContext[IdxRFC].Text = "－";					//RFC 追加 byやまおか 2007/06/26
                        }
                        //v19.00 BHC　(電S2)永井
                        if (IdxBHCTable > 0)
                        {
                            lblContext[IdxBHCTable].Text = "－";
                        }
                        if (IdxFilterProc > 0)
                        {
                            lblContext[IdxFilterProc].Text = "－";		//フィルタ処理 追加 byやまおか 2007/06/26
                        }
                    }

					//1画素サイズ(mm)
					double matsiz = 0;
                    double.TryParse(myInfoRec.Data.matsiz.GetString(), out matsiz);
					if (matsiz == 0)
					{
						lblContext[IdxPixelSize].Text = "";
					}
					else
					{
						lblContext[IdxPixelSize].Text = (ScaleValue / matsiz).ToString("##0.0000000000");
					}

					//If DetType = DetTypePke Then    'v17.00追加(ここから) byやまおか 2010/02/17
					//v17.30 条件式を追加　by 長野 2010-09-26
					//If (DetType = DetTypePke) And (.detector = 2) Then
					//    lblContext(ldxFpdGain).Caption = IIf(GetImageInfoVerNumber(.version) >= 17, GetFpdGainStr(.fpd_gain), "")       'FPDゲイン
					//    lblContext(ldxFpdInteg).Caption = IIf(GetImageInfoVerNumber(.version) >= 17, GetFpdIntegStr(.fpd_integ), "")    'FPD積分時間
					//End If                          'v17.00追加(ここまで) byやまおか 2010/02/17
					//v17.60修正 byやまおか 2011/06/07
                    if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
					{
                        bool disp_flg = false;		//内容を表示するかのフラグ

                        //追加2014/10/07hata_v19.51反映
                        info_ver = modImageInfo.GetImageInfoVerNumber(myInfoRec.Data.version.GetString());
                        //disp_flg = (modImageInfo.GetImageInfoVerNumber(myInfoRec.Data.version.GetString()) >= 17) && (myInfoRec.Data.detector == (int)DetectorConstants.DetTypePke);		//Ver17以降でPkeFPDの場合はTrue
                        //v19.50 変更 by長野 2013/11/20//変更2014/10/07hata_v19.51反映
                        disp_flg = (info_ver >= 17) & (myInfoRec.Data.detector == (int)DetectorConstants.DetTypePke);  //Ver17以降でPkeFPDの場合はTrue
                        
                        //変更2014/10/07hata_v19.51反映
                        //lblContext[IdxFpdGain].Text = (disp_flg ? modCT30K.GetFpdGainStr(myInfoRec.Data.fpd_gain) : "");						//FPDゲイン
                        //lblContext[IdxFpdInteg].Text = (disp_flg ? modCT30K.GetFpdIntegStr(myInfoRec.Data.fpd_integ) : "");					//FPD積分時間
					    //v19.50 v18系以外とv19.50未満はゲインと積分時間をindexでもっているため場合わけが必要 by長野 2013/11/20
					    if (((info_ver >= 19.5) | (info_ver >= 18.0 & info_ver < 19.0))) 
                        {
						    lblContext[IdxFpdGain].Text = modImageInfo.GetImageInfoFpdGain(ref myInfoRec.Data);                 //FPDゲイン      'v18.00変更 byやまおか 2011/07/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                            lblContext[IdxFpdInteg].Text = modImageInfo.GetImageInfoFpdInteg(ref myInfoRec.Data);		        //FPD積分時間    'v18.00変更 byやまおか 2011/07/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
					    } 
                        else 
                        {
                            lblContext[IdxFpdGain].Text = (disp_flg ? modCT30K.GetFpdGainStr(myInfoRec.Data.fpd_gain, CTSettings.t20kinf.Data.pki_fpd_type) : "");        //FPDゲイン
                            lblContext[IdxFpdInteg].Text = (disp_flg ? modCT30K.GetFpdIntegStr(myInfoRec.Data.fpd_integ) : "");     //FPD積分時間
					    }                   
                    }
                
                }
				catch
				{
					// Nothing
				}

				try
				{
					//v16.10 4096対応　scansel.disp_sizeと画像の拡大縮小を、ここで更新する　by 長野 2010/02/16
					UpDateDispSize();
				}
				catch
				{
					// Nothing
				}

				try
				{
					//v16.10 4096対応  "拡大・縮小"キャプションを更新する by 長野 2010/03/08
					frmScanImage.Instance.UpdateMagnifyCaption();
				}
				catch
				{
					// Nothing
				}

				try
				{
					//スケールの更新
					UpdateScale();
				}
				catch
				{
					// Nothing
				}

				try
				{
					//付帯情報修正中の場合、付帯情報修正フォームを最新の状態にする
					//if (modLibrary.IsExistForm(frmPictureRetouch.Instance))
                    if (modLibrary.IsExistForm("frmPictureRetouch"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
                    {
						frmPictureRetouch.Instance.Target = myTarget;
					}
				}
				catch
				{
					// Nothing
                }

                #region	    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //骨塩定量測定キャリブROI測定済みフラグのクリア added by 山本 2002-10-19
                //    GFlg_MaesureCalibRoi = 0
                #endregion  //'v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            }
		}


		//*******************************************************************************
		//機　　能： UpDateDispSize
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V16.10  10/02/26   (検SS)長野   新規作成
		//*******************************************************************************
		private void UpDateDispSize()
		{
			int matrix_size = 0;
			//modScansel.GetScansel(ref modScansel.scansel);
            CTSettings.scansel.Load();

			//今まで表示していた画像が、拡大か縮小かを確認する。
			if (frmScanImage.Instance.hsbImage.Visible == true)
			{
				matrix_size = this.Matrix;

				//今まで表示していた画像が拡大の場合は、表示しようとしている画像も拡大で表示させるため、マトリクスサイズに従い、disp_sizeを決める。
				switch (matrix_size)
				{
					case (2048):
                        CTSettings.scansel.Data.disp_size = 1;
						break;
					case (4096):
                        CTSettings.scansel.Data.disp_size = 2;
						break;
					default:
                        CTSettings.scansel.Data.disp_size = 0;
						break;
				}
			}
			else
			{
				//今まで表示していた画像が縮小だった場合、表示しようとしている画像も縮小で表示する。
                CTSettings.scansel.Data.disp_size = 0;
			}

		    // scansel 書き込み
        	//modScansel.PutScansel(ref modScansel.scansel);
            CTSettings.scansel.Write();
        }


		//*******************************************************************************
		//機　　能： Targetプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public string Comment
		{
			get { return lblContext[IdxComment].Text.Trim(); }
			set
			{
				//付帯情報へ書き込み
				//modLibrary.SetField(value, ref myInfoRec.Comment);
				//modImageInfo.WriteImageInfo(ref myInfoRec, myTarget);
                myInfoRec.Data.comment.SetString(value);
                ImageInfo.WriteImageInfo(ref myInfoRec.Data, myTarget ,".inf");

				//コメントを表示するコントロールを更新
				lblContext[IdxComment].Text = value + new string(' ', 20);
			}
		}


		//*******************************************************************************
		//機　　能： SizePerPixelプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： １画素あたりの長さ(mm)
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public double SizePerPixel
		{
			get
			{
				double value = 0;
				double.TryParse(lblContext[IdxPixelSize].Text, out value);
				return value;
			}
		}


		//*******************************************************************************
		//機　　能： SliceNameプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： スライス名（.img付き）
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public string SliceName
		{
			get { return Path.GetFileName(modLibrary.AddExtension(myTarget, ".img")); }
		}


		//*******************************************************************************
		//機　　能： FullSliceNameプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V14.10  07/08/21   やまおか      新規作成
		//*******************************************************************************
		public string FullSliceName
		{
			get { return modLibrary.AddExtension(myTarget, ".img"); }
		}


		//*******************************************************************************
		//機　　能： ScanDateプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： スキャン年月日
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public string ScanDate
		{
			get { return lblContext[IdxScanDate].Text.Trim(); }
		}


		//*******************************************************************************
		//機　　能： SlicePosプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： スライス位置（mm付き文字列）
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public string SlicePos
		{
            get { return modLibrary.RemoveNull(myInfoRec.Data.table_pos.GetString()) + " mm"; }
		}


		//*******************************************************************************
		//機　　能： Matrixプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： マトリクスサイズ
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public int Matrix
		{
			get
			{
				int value = 0;
                int.TryParse(myInfoRec.Data.matsiz.GetString(), out value);
				return value;
			}
		}


		//*******************************************************************************
		//機　　能： ScaleValueプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 現在表示中の画像のスケール値
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public double ScaleValue
		{
			get
			{
				double value = 0;
                double.TryParse(myInfoRec.Data.scale.GetString(), out value);
				return (value / 1000);
			}
		}


		//*******************************************************************************
		//機　　能： IsConeBeamプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： コーンビーム画像か？
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public bool IsConeBeam
		{
			get { return (myInfoRec.Data.bhc == 1); }
		}


		//*******************************************************************************
		//機　　能： ReconStartAngleプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 表示している画像回転角
		//
		//履　　歴： V14.1  07/08/01   ????????      新規作成
		//*******************************************************************************
		public float ReconStartAngle
		{
			get { return myInfoRec.Data.recon_start_angle; }
		}


		//*******************************************************************************
		//機　　能： ウィンドウレベル
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.0 2008/11/25 (SI1)間々田    新規作成
		//*******************************************************************************
		public int WindowLevel
		{
			get
			{
				int value = 0;
                int.TryParse(myInfoRec.Data.wl.GetString(), out value);
				return value;
			}
			set
			{
				if (string.IsNullOrEmpty(myTarget)) return;

				//付帯情報へ書き込み
				//modLibrary.SetField(Convert.ToString(value), ref myInfoRec.WL);
                myInfoRec.Data.wl.SetString(value.ToString());
                ImageInfo.WriteImageInfo(ref myInfoRec.Data, myTarget, ".inf");

				//該当するコントロールを更新
				lblContext[IdxWindowLevel].Text = Convert.ToString(value);
			}
		}


		//*******************************************************************************
		//機　　能： ウィンドウ幅
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.0 2008/11/25 (SI1)間々田    新規作成
		//*******************************************************************************
		public int WindowWidth
		{
			get
			{
				int value = 0;
				int.TryParse(myInfoRec.Data.ww.GetString(), out value);
				return value;
			}
			set
			{
				if (string.IsNullOrEmpty(myTarget)) return;

				//付帯情報へ書き込み
				//modLibrary.SetField(Convert.ToString(value), ref myInfoRec.ww);
				//modImageInfo.WriteImageInfo(ref myInfoRec, myTarget);
                myInfoRec.Data.ww.SetString(value.ToString());
                ImageInfo.WriteImageInfo(ref myInfoRec.Data, myTarget, ".inf");

                //該当するコントロールを更新
				lblContext[IdxWindowWidth].Text = Convert.ToString(value);
			}
		}


		//*******************************************************************************
		//機　　能： ガンマ補正値
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v17.99 2012/02/21 (検S1)長野    新規作成
		//*******************************************************************************
		public float gamma
		{
			get
			{
				float value = 0;
                float.TryParse(myInfoRec.Data.gamma.GetString(), out value);
				return value;
			}
			set
			{
				if (string.IsNullOrEmpty(myTarget)) return;

				//付帯情報へ書き込み
				//modLibrary.SetField(Convert.ToString(value), ref myInfoRec.gamma);
				//modImageInfo.WriteImageInfo(ref myInfoRec, myTarget);
                myInfoRec.Data.gamma.SetString(value.ToString());
                ImageInfo.WriteImageInfo(ref myInfoRec.Data, myTarget, ".inf");

				//該当するコントロールを更新
				lblContext[IdxGamma].Text = Convert.ToString(value);
			}
		}


		//*******************************************************************************
		//機　　能： 「ROI処理あり」を設定する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public void DoRoi()
		{
			//dispinf読み込み
			//modDispinf.GetDispinf(ref modDispinf.dispinf);
            CTSettings.dispinf.Load();


			//付帯情報へ書き込み
			//myInfoRec.roicaltable_dir = modDispinf.dispinf.d_exam;
			//modLibrary.SetField(modLibrary.RemoveNull(modDispinf.dispinf.d_id) + "-ROI.csv", ref myInfoRec.roical_table);
			//myInfoRec.roical = 1;
            myInfoRec.Data.roicaltable_dir = CTSettings.dispinf.Data.d_exam;
            myInfoRec.Data.roical_table.SetString(CTSettings.dispinf.Data.d_id.GetString() + "-ROI.csv");
            myInfoRec.Data.roical = 1;

            ImageInfo.WriteImageInfo(ref myInfoRec.Data, myTarget, ".inf");
		}


		//*******************************************************************************
		//機　　能： 「プロフィールディスタンス処理あり」を設定する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public void DoPRD()
		{
			//dispinf読み込み
			//modDispinf.GetDispinf(ref modDispinf.dispinf);
            CTSettings.dispinf.Load();

			//付帯情報へ書き込み
			//myInfoRec.pdtable_dir = modDispinf.dispinf.d_exam;
			//modLibrary.SetField(modLibrary.RemoveNull(modDispinf.dispinf.d_id) + "-PRD.csv", ref myInfoRec.pd_table);
			//myInfoRec.pd = 1;
            myInfoRec.Data.pdtable_dir = CTSettings.dispinf.Data.d_exam;
            myInfoRec.Data.pd_table.SetString(CTSettings.dispinf.Data.d_id.GetString() + "-PRD.csv");
            myInfoRec.Data.pd = 1;

			ImageInfo.WriteImageInfo(ref myInfoRec.Data, myTarget, ".inf");
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
		//履　　歴： v15.00 2008/11/25 (SI1)間々田   新規作成
		//*******************************************************************************
		private void frmImageInfo_Load(object sender, EventArgs e)
		{
			//'表示位置：スキャン画像の近く  'v15.10削除 byやまおか 2009/11/26
			//With frmScanImage
			//    'Me.Move .Left + 30 - Me.width / 2, 645
			//    Me.Move .Left + frmCTMenu.Toolbar1.width + 60, 645 '変更 by 間々田 2009/07/21
			//End With

			//このフォームのキャプション
			this.Text = CTResources.LoadResString(12800);		//画像情報

			//v17.60 英語用レイアウト調整 by長野 2011/05/23
			if (modCT30K.IsEnglish == true)
			{
				EnglishAdjustLayout();
			}

            //追加2014/06/16(検S1)hata
            myInfoRec = new ImageInfo();
            myInfoRec.Data.Initialize();

			//項目の初期化
			AddItems();

			//    '拡大・縮小ボタンのキャプションの設定
			//    UpdateMagnifyCaption

			//コメントに関してクリックが可能であることをユーザーに知らせるための設定
			//なお、このフォームのロード時の Enabled プロパティは False
			lblContext[IdxComment].Cursor = Cursors.Help;									//マウスを近づけると？付き矢印となるようにする
            ToolTip.SetToolTip(lblContext[IdxComment], CTResources.LoadResString(12126));		//クリックすると値を入力できます。

            //初期表示：簡易表示モード
			DetailOn = false;

			//表示位置： スキャン画像の左下  'v15.10追加 byやまおか 2009/11/26
            // Mod Start 2018/10/29 M.Oyama V26.40 Windows10対応
            //this.Location = new Point(frmScanImage.Instance.Left + frmCTMenu.Instance.Toolbar1.Width + 4, 
            //                          frmScanImage.Instance.Top + frmScanImage.Instance.Height - this.Height - 16);
            this.Location = new Point(frmScanImage.Instance.Left + frmCTMenu.Instance.Toolbar1.Width + 2,
                                      frmScanImage.Instance.Top + frmScanImage.Instance.Height - this.Height - 6);
            // Mod End 2018/10/29
		}


		//*******************************************************************************
		//機　　能： QueryUnloadイベント時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/25 (SI1)間々田   新規作成
		//*******************************************************************************
		private void frmImageInfo_FormClosing(object sender, FormClosingEventArgs e)
		{
			CloseReason UnloadMode = e.CloseReason;

			//フォームのコントロールメニューから「閉じる」コマンドが選択された場合
			if (UnloadMode == CloseReason.UserClosing)
			{
				//非表示にする
				this.Hide();

				//アンロードさせない
				e.Cancel = true;
			}
		}


		//*******************************************************************************
		//機　　能： フォームリサイズ時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/25 (SI1)間々田   新規作成
		//*******************************************************************************
		private void frmImageInfo_Resize(object sender, EventArgs e)
		{
			fraScale.Top = this.ClientRectangle.Height - fraScale.Height - 2;
			cmdDetailMode.Top = fraScale.Top;									//追加 by 間々田 2009/07/21

			//v17.60 英語用レイアウト調整 by長野 2011/05/25
			if (modCT30K.IsEnglish == true)
			{
				cmdDetailMode.Top = fraScale.Top - 5;
			}
		}


		//*******************************************************************************
		//機　　能： 項目を追加する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/25 (SI1)間々田   新規作成
		//*******************************************************************************
		private void AddItems()
		{
			//スキャン位置
            IdxScanPos = AddItem(CTResources.LoadResString(StringTable.IDS_ScanPos) + "(mm)", true);

			//スキャン年月日
            IdxScanDate = AddItem(CTResources.LoadResString(StringTable.IDS_ScanDate), true);

			//スキャン時刻
            IdxScanTime = AddItem(CTResources.LoadResString(StringTable.IDS_ScanTime), true);

			//データ収集時間
			IdxDataAcq = AddItem(CTResources.LoadResString(StringTable.IDS_DataAcqTime));

			//再構成時間
			IdxReconTime = AddItem(CTResources.LoadResString(StringTable.IDS_ReconTime));

            //Rev23.10 追加 by長野 2015/11/04
            if (CTSettings.scaninh.Data.multi_tube == 0)
            {
                IdxMultiTube = AddItem(CTResources.LoadResString(StringTable.IDS_XrayTube));
            }

			//管電圧(kV)
			IdxTubeVolt = AddItem(CTResources.LoadResString(StringTable.IDS_TubeVoltage) + "(kV)", true);

			//管電流(μA)：東芝 EXM2-150の場合はmA
			IdxTubeCurrent = AddItem(CTResources.LoadResString(StringTable.IDS_TubeCurrent) + "(" + modXrayControl.CurrentUni + ")", true);

            //追加2014/10/07hata_v19.51反映
            //v19.50 産業用CTモードの場合のみ表示 by長野 2013/11/16
            if ((CTSettings.scaninh.Data.avmode == 0))
            {
                ////焦点       'v18.00追加 byやまおか 2011/03/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                //IdxXfocus = AddItem(CTResources.LoadResString(StringTable.IDS_Focus));

                //フィルタ   'v18.00追加 byやまおか 2011/03/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                IdxXfilter = AddItem(CTResources.LoadResString(StringTable.IDS_Filter));
            }

            //Rev23.10 変更 by長野 2015/11/04
            if (CTSettings.scaninh.Data.xfocus[0] == 0 || CTSettings.scaninh.Data.xfocus[1] == 0 || CTSettings.scaninh.Data.xfocus[2] == 0 || CTSettings.scaninh.Data.xfocus[3] == 0)
            {
                //焦点       'v18.00追加 byやまおか 2011/03/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                IdxXfocus = AddItem(CTResources.LoadResString(StringTable.IDS_Focus));
            }

			//ビュー数
			IdxViews = AddItem(CTResources.LoadResString(StringTable.IDS_ViewNum), true);

			//積算枚数
			IdxIntegNum = AddItem(CTResources.LoadResString(StringTable.IDS_IntegNum), true);

            //Rev20.00 追加 by長野 2015/01/29
            //テーブル回転
            IdxTableRotation = AddItem(StringTable.BuildResStr(StringTable.IDS_Rotate, StringTable.IDS_Table), true);

			//I.I.視野：FPDの場合I.I.視野の個所にビニングモードを表示する
			//v17.30 付帯情報にビニングモードをもっていないので、フラットパネルの場合はI.I.視野サイズ(ビニングモード)は表示しない by 長野 2010-09-26
			//IdxIIField = AddItem(IIf(Use_FlatPanel, LoadResString(IDS_BinningMode), LoadResString(IDS_IIField)), True)
            if (!CTSettings.detectorParam.Use_FlatPanel)
			{
				IdxIIField = AddItem(CTResources.LoadResString(StringTable.IDS_IIField), true);
			}

			//FPDゲイン      'v17.00追加 byやまおか 2010/02/17
			//If (DetType = DetTypePke) Then ldxFpdGain = AddItem("FPDゲイン(pF)")
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) IdxFpdGain = AddItem(CTResources.LoadResString(20015) + "(pF)");		//ストリングテーブル化　'v17.60 by 長野 2011/5/22

			//FPD積分時間      'v17.00追加 byやまおか 2010/02/17
			//If (DetType = DetTypePke) Then ldxFpdGain = AddItem("FPD積分時間(ms)")
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) IdxFpdInteg = AddItem(CTResources.LoadResString(20016) + "(ms)");		//ストリングテーブル化 '17.60 by 長野 2011/05/22

			//'最大ｽｷｬﾝｴﾘｱ(mm)
			//IdxMaxScanArea = AddItem(LoadResString(IDS_MaxScanArea) & "(mm)", True)

			//FOV(mm)                'v15.0変更 最大スキャンエリア→FOVに変更 2009/07/24 by 間々田
			IdxFOV = AddItem("FOV(mm)", true);

			//スライス厚(mm)
			IdxSliceWidth = AddItem(CTResources.LoadResString(StringTable.IDS_SliceWidth) + "(mm)", true);

			//システム名
			IdxSystemName = AddItem(CTResources.LoadResString(StringTable.IDS_SystemName));

			//マトリクスサイズ
			IdxMatrix = AddItem(CTResources.LoadResString(StringTable.IDS_Matrix), true);

			//'事業所名
			//IdxWorkshopName = AddItem(LoadResString(IDS_WorkShopName)) 'v15.0削除 表示しないことになった 2009/07/24 by 間々田

			//コメント
			IdxComment = AddItem(CTResources.LoadResString(StringTable.IDS_Comment));

            //Wスキャン(0:なし,1:あり) //Rev25.00 追加 by長野 2016/07/05
            //Rev25.10 add by chouno 2017/09/11
            //if (CTSettings.scaninh.Data.w_scan == 0)
            //Rev26.10 change シフトスキャンの名称が"Wスキャン"になっている場合は表示 by chouno 2018/01/16
            if (CTSettings.scaninh.Data.w_scan == 0 || CTSettings.infdef.Data.scan_mode[3].GetString() == CTResources.LoadResString(25009))
            {
                IdxW_Scan = AddItem(CTResources.LoadResString(StringTable.IDS_W_Scan));
            }

			//スキャンモード
			IdxScanMode = AddItem(CTResources.LoadResString(StringTable.IDS_ScanMode), true);

			//フィルタ関数
			IdxFilterFunc = AddItem(CTResources.LoadResString(StringTable.IDS_FilterFunc));

			//フィルタ処理
			if ((CTSettings.scaninh.Data.filter_process[0] == 0) && (CTSettings.scaninh.Data.filter_process[1] == 0))
			{
				IdxFilterProc = AddItem(CTResources.LoadResString(StringTable.IDS_FilterProc));
			}

			//RFC
			if (CTSettings.scaninh.Data.rfc == 0)
			{
				IdxRFC = AddItem(CTResources.LoadResString(StringTable.IDS_RFC));
			}

			//v19.00 BHC (電S2)永井
			if (CTSettings.scaninh.Data.mbhc == 0)
			{
				//BHC有効
				IdxBHCTable = AddItem(CTResources.LoadResString(StringTable.IDS_BHCFile));
			}

            //Rev26.00 ファントムレスBHC有効 by井上 2017/01/20
            if (CTSettings.scaninh.Data.mbhc_phantomless == 0)
            {
                //IdxBHCPhantomless = AddItem(CTResources.LoadResString(21003));
                IdxBHCPhantomless = AddItem(CTResources.LoadResString(StringTable.IDS_BHC));
            }

			//画像バイアス
			IdxImageBias = AddItem(CTResources.LoadResString(StringTable.IDS_ImageBias));

			//画像スロープ
			IdxImageSlope = AddItem(CTResources.LoadResString(StringTable.IDS_ImageSlope));

			//'コーンビーム      'v15.10削除 byやまおか 2009/11/26
			//IdxConebeam = AddItem(LoadResString(IDS_ConeBeam), True)

			//データモード       'v15.10追加 byやまおか 2009/11/26
			IdxDataMode = AddItem(CTResources.LoadResString(StringTable.IDS_DataMode), true);

			//断面像方向
			IdxDirection = AddItem(CTResources.LoadResString(StringTable.IDS_ImageDirection));

			//FID
            //IdxFID = AddItem(CTSettings.gStrFidOrFdd);
            IdxFID = AddItem(CTResources.LoadResString(StringTable.IDS_FID) + "(mm)"); //Rev26.00 change by chouno 2017/10/31

			//FCD
			//IdxFCD = AddItem(CTResources.LoadResString(StringTable.IDS_FCD));
            IdxFCD = AddItem(CTResources.LoadResString(StringTable.IDS_FCD) + "(mm)"); //Rev26.00 change by chouno 2017/10/31

            //追加2014/10/07hata_v19.51反映
            //Y軸(AxisName(1))   'v18.00追加 byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //IdxTableXPos = AddItem(CTSettings.AxisName[1]); 
            IdxTableXPos = AddItem(CTSettings.AxisName[1] + "(mm)"); //Rev26.00 change by chouno 2017/10/31

			//ウィンドウレベル
			IdxWindowLevel = AddItem(CTResources.LoadResString(StringTable.IDS_WindowLevel));

			//ウィンドウ幅
			IdxWindowWidth = AddItem(CTResources.LoadResString(StringTable.IDS_WindowWidth));

			//ガンマ補正値 '19.00追加 by長野 2012/02/21
			IdxGamma = AddItem(CTResources.LoadResString(StringTable.IDS_Gamma));

			//1画素サイズ(mm)
			IdxPixelSize = AddItem(CTResources.LoadResString(StringTable.IDS_PixelSize));

			//拡大倍率
			IdxMagnify = AddItem(CTResources.LoadResString(StringTable.IDS_Magnification));

			//スケール
			IdxScale = AddItem(CTResources.LoadResString(StringTable.IDS_Scale));
		}


		//*******************************************************************************
		//機　　能： 項目を追加する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： itemString      [I/ ] String    項目名
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/25 (SI1)間々田   新規作成
		//*******************************************************************************
		private int AddItem(string itemString, bool isDetail = false)
		{
			int itemTop = 0;
			int Index = 0;

			Index = lblItem.Count - 1;

			if (!string.IsNullOrEmpty(lblItem[Index].Text))
			{
				itemTop = lblItem[Index].Top + lblItem[Index].Height + 1;

				Index = lblItem.Count;

				//項目名用ラベル
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				Load lblItem(Index)
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
				lblItem.Add(new Label());
				lblItem[Index].AutoSize = lblItem1.AutoSize;
				lblItem[Index].Enabled = lblItem1.Enabled;
				lblItem[Index].Location = lblItem1.Location;
				lblItem[Index].Name = "lblItem" + Index.ToString();
				lblItem[Index].Text = "";
				this.Controls.Add(lblItem[Index]);

				lblItem[Index].Visible = true;
				lblItem[Index].Top = itemTop;


				//コロン用ラベル
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				Load lblColon(Index)
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
				lblColon.Add(new Label());
				lblColon[Index].AutoSize = lblColon1.AutoSize;
				lblColon[Index].Enabled = lblColon1.Enabled;
				lblColon[Index].Location = lblColon1.Location;
				lblColon[Index].Name = "lblColon" + Index.ToString();
				lblColon[Index].Text = lblColon1.Text;
				this.Controls.Add(lblColon[Index]);

				lblColon[Index].Visible = true;
				lblColon[Index].Top = itemTop;


				//項目内容用ラベル
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				Load lblContext(Index)
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
				lblContext.Add(new Label());
				lblContext[Index].AutoSize = lblContext1.AutoSize;
				lblContext[Index].Enabled = lblContext1.Enabled;
				lblContext[Index].Location = lblContext1.Location;
				lblContext[Index].Name = "lblContext" + Index.ToString();
				lblContext[Index].Text = "";
				lblContext[Index].TextChanged += new System.EventHandler(this.lblContext_TextChanged);
				lblContext[Index].Click += new System.EventHandler(this.lblContext_Click);
				this.Controls.Add(lblContext[Index]);

				lblContext[Index].Visible = true;
				lblContext[Index].Top = itemTop;
			}

			//項目名用ラベルに項目名をセット
			lblItem[Index].Text = itemString;
			lblItem[Index].Tag = Convert.ToString(isDetail);

			//戻り値セット
			return Index;
		}


		//*******************************************************************************
		//機　　能： 項目変更時時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/25 (SI1)間々田   新規作成
		//*******************************************************************************
		private void lblContext_TextChanged(object sender, EventArgs e)
		{
			if (sender as Label == null) return;
			int Index = lblContext.IndexOf((Label)sender);
			if (Index < 0) return;


			if (Index == IdxScale)			//スケール
			{
				double value = 0;
				double.TryParse(lblContext[Index].Text , out value);

				//拡大倍率の更新
                if (value == 0)
                {
                    lblContext[IdxMagnify].Text = "";
                }
                else
                {
                    //lblContext[IdxMagnify].Text = (CTSettings.scancondpar.Data.magnify_para / value).ToString("##0.00000");
                    //Rev23.10 変更 by長野 2015/11/04
                    if (myInfoRec != null)
                    {
                        lblContext[IdxMagnify].Text = (CTSettings.scancondpar.Data.magnify_para[myInfoRec.Data.multi_tube] / value).ToString("##0.00000");
                    }
                    else
                    {
                        lblContext[IdxMagnify].Text = (CTSettings.scancondpar.Data.magnify_para[0] / value).ToString("##0.00000");
                    }
                }
			}
		}


		//*******************************************************************************
		//機　　能： 項目クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void lblContext_Click(object sender, EventArgs e)
		{
			if (sender as Label == null) return;
			int Index = lblContext.IndexOf((Label)sender);
			if (Index < 0) return;


			//コメント入力ダイアログ表示
			if (lblContext[Index].Cursor == Cursors.Help)
			{
				//            theComment = frmComment.Dialog("ＣＴ画像に対するコメントを入力してください：", _
				//'                                           GetResString(IDS_InputCommentOf, Me.Caption), _
				//'                                           Me.Comment)
				//ストリングテーブル化 'v17.60 by 長野 2011/05/22
				string theComment = frmComment.Dialog(CTResources.LoadResString(20018), 
																StringTable.GetResString(StringTable.IDS_InputCommentOf, this.Text), 
																this.Comment);
				if (!string.IsNullOrEmpty(theComment)) this.Comment = theComment;
			}

			//v19.00 BHC情報表示 (電S2)永井
			if (Index == IdxBHCTable)		
			{
				//BHCパラメータ情報表示
				for (int i = 1; i <= 6; i++)
				{
					frmBHCMessage.Instance.lblInfBHC[i].Text = myInfoRec.Data.mbhc_a[i].ToString("0.######0");
					frmBHCMessage.Instance.lblPriod[i].Text = "a" + i.ToString() + ": ";
				}

				frmBHCMessage.Instance.ShowDialog();
			}
		}


		//*******************************************************************************
		//機　　能： スケールの更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/25 (SI1)間々田   新規作成
		//*******************************************************************************
		public void UpdateScale()
		{
			//スケール
			//v16.10 4096対応のためcaptionの決め方を変更する by 長野 2010/02/15
			//lblContext(IdxScale).Caption = CStr(ScaleValue / 10 / IIf((scansel.disp_size = 1) And frmScanImage.mnuEnlarge.Enabled, 2, 1))

			if (frmScanImage.Instance.mnuEnlarge.Enabled == true)
			{
				//scansel.disp_sizeを見て，4096と2048の場合のスケールを変更する。
				switch (CTSettings.scansel.Data.disp_size)
				{
					case 0:
						lblContext[IdxScale].Text = Convert.ToString(ScaleValue / 10);
						break;
					case 1:
						lblContext[IdxScale].Text = Convert.ToString(ScaleValue / 10 / 2);
						break;
					case 2:
						lblContext[IdxScale].Text = Convert.ToString(ScaleValue / 10 / 4);
						break;
				}
			}
			else
			{
				lblContext[IdxScale].Text = Convert.ToString(ScaleValue / 10);
			}
		}


		//削除ここから by 間々田 2009/07/21 右クリックによるポップアップメニューの表示をやめ，ボタンによる切り替えに変更した
		//Private Sub fraScale_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
		//    Form_MouseDown Button, Shift, x, y
		//End Sub
		//
		//Private Sub lblColon_MouseDown(Index As Integer, Button As Integer, Shift As Integer, x As Single, y As Single)
		//    Form_MouseDown Button, Shift, x, y
		//End Sub
		//
		//Private Sub lblContext_MouseDown(Index As Integer, Button As Integer, Shift As Integer, x As Single, y As Single)
		//    Form_MouseDown Button, Shift, x, y
		//End Sub
		//
		//Private Sub lblItem_MouseDown(Index As Integer, Button As Integer, Shift As Integer, x As Single, y As Single)
		//    Form_MouseDown Button, Shift, x, y
		//End Sub
		//
		//Private Sub Form_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
		//
		//    '右マウスボタン
		//    If Button = vbRightButton Then
		//
		//        'ポップアップメニューを表示
		//        Me.PopupMenu mnuPopUp, vbPopupMenuRightButton
		//
		//    End If
		//
		//End Sub
		//削除ここまで by 間々田 2009/07/21 右クリックによるポップアップメニューの表示をやめ，ボタンによる切り替えに変更した


		//*******************************************************************************
		//機　　能： 画面のリロード
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 現在、未使用。
		//           検出器切替にフォームのアンロード→ロードというやり方を採用しているが
		//           代わりにこのメソッドを使っても同じ効果が得られる
		//
		//履　　歴： v17.50 2011/03/24 (電S1)間々田   新規作成
		//*******************************************************************************
		public void Reload()
		{
			int i = 0;

			//動的に追加したコントロールをいったん削除する

			//項目名用ラベル
			for (i = 2; i <= lblItem.Count - 1; i++)
			{
				lblItem[i].Dispose();
				lblItem[i] = null;
			}
			if (lblItem.Count > 2) lblItem.RemoveRange(2, lblItem.Count - 2);

			//コロン用ラベル
			for (i = 2; i <= lblColon.Count - 1; i++)
			{
				lblColon[i].Dispose();
				lblColon[i] = null;
			}
			if (lblColon.Count > 2) lblColon.RemoveRange(2, lblColon.Count - 2);

			//項目内容用ラベル
			for (i = 2; i <= lblContext.Count - 1; i++)
			{
				lblContext[i].Dispose();
				lblContext[i] = null;
			}
			if (lblContext.Count > 2) lblContext.RemoveRange(2, lblContext.Count - 2);

			//最初の項目（内容をクリア）
			lblContext1.Text = string.Empty;

			//メンバ変数を初期化する
			//modImageInfo.ImageInfoStruct temp = default(modImageInfo.ImageInfoStruct);		//現在表示している付帯情報レコード
            CTstr.IMAGEINFO temp = default(CTstr.IMAGEINFO);
            temp.Initialize();

            myInfoRec.Data = temp;
			myTarget = string.Empty;							//現在表示している付帯情報のファイル名（拡張子なし）

			//項目のインデクスを格納する変数
			IdxScanPos = 0;			//スキャン位置
			IdxScanDate = 0;		//スキャン年月日
			IdxScanTime = 0;		//スキャン時刻
			IdxDataAcq = 0;			//データ収集時間
			IdxReconTime = 0;		//再構成時間
			IdxTubeVolt = 0;		//管電圧
			IdxTubeCurrent = 0;		//管電流
			IdxViews = 0;			//ビュー数
			IdxIntegNum = 0;		//積算枚数
			IdxIIField = 0;			//I.I.視野
			IdxFOV = 0;				//FOV
			IdxSliceWidth = 0;		//スライス厚
			IdxSystemName = 0;		//システム名
			IdxMatrix = 0;			//画像サイズ
			IdxComment = 0;			//コメント
			IdxScanMode = 0;		//スキャンモード
			IdxFilterFunc = 0;		//フィルタ関数
			IdxFilterProc = 0;		//フィルタ処理
			IdxRFC = 0;				//RFC
			IdxBHCTable = 0;		//BHCテーブル v19.00 (電S2)永井
			IdxImageBias = 0;		//画像バイアス
			IdxImageSlope = 0;		//画像スロープ
			IdxDataMode = 0;		//データモード
			IdxDirection = 0;		//断面像方向
			IdxFID = 0;				//FID
			IdxFCD = 0;				//FCD
			IdxWindowLevel = 0;		//ウィンドウレベル
			IdxWindowWidth = 0;		//ウィンドウ幅
			IdxPixelSize = 0;		//1画素サイズ
			IdxMagnify = 0;			//拡大倍率
			IdxScale = 0;			//スケール
			IdxFpdGain = 0;			//FPDゲイン
			IdxFpdInteg = 0;		//FPD積分時間
			IdxGamma = 0;			//v19.00 追加 by長野 2012/02/21
            IdxTableRotation = 0;			//v20.00 追加 by長野 2015/01/29
            IdxBHCPhantomless = 0;  //Rev26.00 ファントムレスBHC by井上 2017/01/19

			//フォームロード処理
			frmImageInfo_Load(this, EventArgs.Empty);
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
		//履　　歴： v17.60 2011/05/25 (検S1)長野   新規作成
		//*******************************************************************************
		private void EnglishAdjustLayout()
		{
			lblItem1.Left = 3;
			lblColon1.Left = lblColon1.Left + 9;
			lblContext1.Left = lblContext1.Left + 9;
            //Rev20.01 変更 by長野 2015/05/19
			//cmdDetailMode.Height = 30;
            //cmdDetailMode.Height = 33;
            cmdDetailMode.Left = cmdDetailMode.Left - 7;
            cmdDetailMode.Width = 100;

            cmdDetailMode.Font = new Font(cmdDetailMode.Font.Name, (float)6.5, cmdDetailMode.Font.Style, cmdDetailMode.Font.Unit);
        }

        //追加2014/05hata
        private void frmImageInfo_Activated(object sender, EventArgs e)
        {
            ////描画を強制する
            //if (this.Visible && this.Enabled) this.Refresh();
        }

	}
}
