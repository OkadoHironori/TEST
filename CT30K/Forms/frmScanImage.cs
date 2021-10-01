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
using System.Diagnostics;
using System.Drawing.Drawing2D;
//
using CT30K.Common;
using CTAPI;
using TransImage;
//using CT30K.Controls;
//using CT30K.Modules;
using CT30K.Properties;

namespace CT30K
{
	/* ************************************************************************** */
	/* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
	/* 客先　　　　： ?????? 殿                                                   */
	/* プログラム名： 画像処理表示.frm                                            */
	/* 処理概要　　： 画像処理中の断面像                                          */
	/* 注意事項　　： なし                                                        */
	/* -------------------------------------------------------------------------- */
	/* 適用計算機　： DOS/V PC                                                    */
	/* ＯＳ　　　　： Windows 2000  (SP4)                                         */
	/* コンパイラ　： VB 6.0                                                      */
	/* -------------------------------------------------------------------------- */
	/* VERSION     DATE        BY                  CHANGE/COMMENT                 */
	/*                                                                            */
	/* V1.00       99/XX/XX    (TOSFEC) ????????                                  */
	/* V4.0        01/02/22    (ITC)    鈴山　修   Moveableﾌﾟﾛﾊﾟﾃｨを"False"に変更 */
	/* V9.0        04/02/27    (SI4)    間々田     線分関連のロジックを手直し  　 */
	/*                                                                            */
	/* -------------------------------------------------------------------------- */
	/* ご注意：                                                                   */
	/* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	/*                                                                            */
	/* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
	/* ************************************************************************** */

    //public partial class frmScanImage : Form
	public partial class frmScanImage : FixedForm
	{
		//********************************************************************************
		//  共通データ宣言
		//********************************************************************************
		private Image picObj;					//画像オブジェクト
		private int myPicWidth;					//画像幅
		private int myPicHeight;				//画像高さ
		private int myMagnify;					//1:原寸表示 2: 縦横1/2に圧縮して表示
		private bool myChanged;					//オリジナル画像に変更があったか
		private string myTarget;				//現在表示中のＣＴ画像ファイル名
		private int myWindowLevel;				//ウィンドウレベル
		private int myWindowWidth;				//ウィンドウ幅
		private float myGamma;					//v19.00 追加 by長野 2012/02/21

		private bool byEvent;
		//private Points offset;					//ダブルクリック制御用
        private Winapi.POINTAPI offset;					//ダブルクリック制御用
        private bool DoubleClickOn;				//ダブルクリック制御用
		private MouseButtons LastButton;

		//ROI制御オブジェクト参照用
		private RoiData myRoi;

		//ROI制御タイプ
		public enum ImageProcType
		{
			RoiNone,
			roiEnlarge,
			RoiCTDump,
			roiProcessing,
			roiProfile,
			RoiProfileDistance,
			roiDistance,
			RoiAngle,
			roiZooming,
			roiHistgram,
			RoiBoneDensity,
			RoiAutoPos
		}

		//ROI制御タイプ変数
		private ImageProcType myImageProc;

		//前回押下されたRoiボタン
		private ToolStripButton LastRoiButton;

		//ＣＴ値入力フォーム
		CTInputForm myCTInputForm;

		//閾値入力フォーム
		CTSliceForm myCTSliceForm;

		//イベント宣言
		public event EventHandler RoiChanged;

		private int DoProfile_x1;
		private int DoProfile_y1;
		private int DoProfile_x2;
		private int DoProfile_y2;

		private static frmScanImage _Instance = null;

 


		/// <summary>
		/// 
		/// </summary>
		public frmScanImage()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 
		/// </summary>
		public static frmScanImage Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmScanImage();
				}

				return _Instance;
			}
		}

		//*************************************************************************************************
		//機　　能： 画像の幅
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*************************************************************************************************
		public int PicWidth
		{
			//戻り値セット
			get { return myPicWidth; }
		}

		//*************************************************************************************************
		//機　　能： 画像の高さ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*************************************************************************************************
		public int PicHeight
		{
			//戻り値セット
			get { return myPicHeight; }
		}

		//*************************************************************************************************
		//機　　能： 倍率
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 1:原寸表示 2: 縦横1/2に圧縮して表示
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*************************************************************************************************
		public int Magnify
		{
			//戻り値セット
			get { return myMagnify; }
		}

		//*************************************************************************************************
		//機　　能： オリジナルの画像から変更されているか
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*************************************************************************************************
		public bool Changed
		{
			//戻り値セット
			get { return myChanged; }
		}

		//*************************************************************************************************
		//機　　能： 画面上にROIが描画されているか
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*************************************************************************************************
		public bool IsExistRoi
		{
			get
			{
				//戻り値初期化
				bool result = false;

				if (myRoi != null)
				{
					result = (myRoi.NumOfRois > 0);
				}

				return result;
			}
		}

		//*******************************************************************************
		//機　　能： ＣＴ値入力フォームでのボタンクリック時処理（イベント処理）
		//
		//           変数名          [I/O] 型        内容
		//引　　数： theButton       [I/ ] CTButtonConstants クリックされたボタン
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		private void myCTInputForm_Clicked(modImgProc.CTButtonConstants theButton)
		{
			//「表示」または「ＯＫ」ボタンがクリックされた場合
			if (theButton == modImgProc.CTButtonConstants.btnCTOK || theButton == modImgProc.CTButtonConstants.btnCTDisp)
			{
				if (myImageProc == ImageProcType.roiHistgram)
				{
					//ヒストグラム処理実行：失敗した場合ここで抜ける
					if (!DoHistgram())
					{
						return;
					}
				}
				else if (myImageProc == ImageProcType.roiProfile || myImageProc == ImageProcType.RoiProfileDistance)
				{
					//プロフィール処理実行：失敗した場合ここで抜ける
					if (!DoProfile())
					{
						return;
					}
				}

				//「表示」の場合はここで抜ける
				if (theButton == modImgProc.CTButtonConstants.btnCTDisp)
				{
					return;
				}
			}

			//ＣＴ値入力フォームアンロード


            //CTinputFormのイベントの破棄
            myCTInputForm.Clicked -= new CTInputForm.ClickedEventHandler(myCTInputForm_Clicked);

			myCTInputForm.Close();
			myCTInputForm = null;

			//ツールバーを使用可にする
			ToolBarEnabled = true;

			//プロフィールディスタンス処理時で「ＯＫ」ボタンがクリックされた場合
			if (myImageProc == ImageProcType.RoiProfileDistance && theButton == modImgProc.CTButtonConstants.btnCTOK)
			{
				//ツールバーを使用不可にする
				ToolBarEnabled = false;

				//閾値入力フォーム表示
				myCTSliceForm = CTSliceForm.Instance;
				myCTSliceForm.BinaryImageThreshold = false;

                //追加2014/12/22hata_dNet
                //CTSliceFormのイベント設定
                myCTSliceForm.Clicked += new CTSliceForm.ClickedEventHandler(myCTSliceForm_Clicked);

				//myCTSliceForm.Show , frmCTMenu
				myCTSliceForm.ShowDialog(frmCTMenu.Instance);	//v15.10変更 byやまおか 2009/12/01
				return;
			}

			//プロフィール、PD、ヒストグラム処理で「キャンセル」ボタンがクリックされた場合 'v15.10追加 byやまおか 2009/12/01
			else if ((myImageProc == ImageProcType.roiProfile || myImageProc == ImageProcType.RoiProfileDistance || myImageProc == ImageProcType.roiHistgram) && (theButton == modImgProc.CTButtonConstants.btnCTCancel))
			{
				//画像を元に戻す
				DispOrgImage();
                modImgProc.PRDPoint = 0;
			}
			else if (myImageProc != ImageProcType.roiHistgram)
			{
				//ROIを選択していない状態にする
				tsbtnLine.Checked = false;
				tsbtnHLine.Checked = false;
				tsbtnVLine.Checked = false;
				myRoi.ModeToPaint(RoiData.RoiShape.NO_ROI);
			}

			//このフォームを使用可にする
			this.Enabled = true;
		}

		//*******************************************************************************
		//機　　能： 閾値入力フォームでのボタンクリック時処理（イベント処理）
		//
		//           変数名          [I/O] 型                内容
		//引　　数： theButton       [I/ ] CTButtonConstants クリックされたボタン
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		private void myCTSliceForm_Clicked(modImgProc.CTButtonConstants theButton)
		{
			bool BinaryImageThreshold = false;
			BinaryImageThreshold = myCTSliceForm.BinaryImageThreshold;

			//「表示」または「ＯＫ」ボタンがクリックされた場合
			if (theButton == modImgProc.CTButtonConstants.btnCTOK || theButton == modImgProc.CTButtonConstants.btnCTDisp)
			{
				//２値化画像閾値入力モードか
				if (BinaryImageThreshold)
				{
                    if (!DoDispBitImage(modImgProc.CT_Low1Point, modImgProc.CT_High1Point))
					{
						return;
					}
				}
				else
				{
					//プロフィール処理実行：失敗した場合ここで抜ける
					if (!DoProfile(true))
					{
						return;
					}
				}

				//「表示」の場合もここで抜ける
				if (theButton == modImgProc.CTButtonConstants.btnCTDisp)
				{
					return;
				}
			}

			//閾値入力フォームアンロード
			myCTSliceForm.Close();

            //追加2014/12/22hata_dNet
            //CTSliceFormのイベント破棄
            myCTSliceForm.Clicked -= new CTSliceForm.ClickedEventHandler(myCTSliceForm_Clicked);
            myCTSliceForm.Dispose();

			//閾値入力フォームに対する参照を破棄
			myCTSliceForm = null;

			//ツールバーを使用可にする
			ToolBarEnabled = true;

			//ROI１点指定時（つまり２値化画像閾値フォーム表示時）
			if (BinaryImageThreshold)
			{
				//「OK」をクリックした時
				if (theButton == modImgProc.CTButtonConstants.btnCTOK)
				{
					//測定２点を求める
					if (GetRoi2Points())
					{
						//１点指定フラグをセット
                        modImgProc.PRDPoint = 1;

						//プロフィール処理
						DoProfile();
						return;
					}
				}

				//ROI１点指定を解除
				tsbtnPoint.Checked = false;
				myRoi.ModeToPaint(RoiData.RoiShape.NO_ROI);

				//ROI削除
				myRoi.DeleteAllRoiData();

				//オリジナル画像の表示
				DispOrgImage();
                modImgProc.PRDPoint = 0;
			}
			else
			{
				//「OK」をクリックした時
				if (theButton == modImgProc.CTButtonConstants.btnCTOK)
				{
					DoPRD();
				}

				//Toolbar1.Buttons("Line").Value = tbrUnpressed  'v15.10削除 byやまおか 2009/12/01
     		    //削除2014/12/22hata_キャンセル時にRECTの設定なってしまうため削除 
				//myRoi.ModeToPaint(RoiData.RoiShape.NO_ROI);

				//PD処理で「キャンセル」ボタンがクリックされた場合 'v15.10追加 byやまおか 2009/12/01
				if (myImageProc == ImageProcType.RoiProfileDistance && theButton == modImgProc.CTButtonConstants.btnCTCancel)
				{
					//画像を元に戻す
					DispOrgImage();
				}
				else
				{
                    //追加2014/12/22hata
                    myRoi.ModeToPaint(RoiData.RoiShape.NO_ROI);
					
                    tsbtnLine.Checked = false;
				}

//				'制御を行えるようにする
//				myRoi.RoiMaxNum = 1
//
//				'線分をペースト
//				myRoi.RoiPaste
			}

			//このフォームを使用可にする
			this.Enabled = true;
		}

		//*******************************************************************************
		//機　　能： ImageProcプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.0　2009/03/25   (SI1)間々田   リニューアル
		//*******************************************************************************
		private bool ToolBarEnabled
		{
			set
			{
                //削除2015/01/22hata_ボタンを先に変更する
                //Toolbar1.Enabled = value;

                if (value)
                {
                    //変更2014/07/25(検S1)hata
                    //foreach (ToolStripButton Button in Toolbar1.Items)
                    //{
                    //	Button.Enabled = Convert.ToBoolean(Button.Tag);
                    //}
                    foreach (ToolStripItem Item in Toolbar1.Items)
                    {
                        if (Item.GetType() == typeof(ToolStripButton))
                        {
                            ToolStripButton btn = Item as ToolStripButton;
                            btn.Enabled = Convert.ToBoolean(btn.Tag);
                        }
                    }
                }
                else
                {
                    //変更2014/07/25(検S1)hata
                    //foreach (ToolStripButton Button in Toolbar1.Items)
                    //{
                    //	Button.Tag = Convert.ToString(Button.Enabled);
                    //	Button.Enabled = false;
                    //}
                    foreach (ToolStripItem Item in Toolbar1.Items)
                    {
                        if (Item.GetType() == typeof(ToolStripButton))
                        {
                            ToolStripButton btn = Item as ToolStripButton;
                            btn.Tag = Convert.ToString(btn.Enabled);
                            btn.Enabled = false;
                        }
                    }
                }
                //追加2015/01/22hata_ボタンを先に変更する
                Toolbar1.Enabled = value;

				//ROIメッセージフォームを有効にする
				frmRoiMessage.Instance.Enabled = value;
			}
		}

		//*******************************************************************************
		//機　　能： ImageProcプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.0　2009/03/25   (SI1)間々田   リニューアル
		//*******************************************************************************
		public ImageProcType ImageProc
		{
			get { return myImageProc; }

			set
			{
				//前回のCTBusyフラグをリセット
				if (myImageProc == ImageProcType.roiZooming)
				{
					modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTZooming);
				}
				else if (myImageProc != ImageProcType.RoiNone)
				{
					modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTImageProcessing);
				}

				//処理の記憶
				myImageProc = value;

				//CTBusyフラグをセット
				if (myImageProc == ImageProcType.roiZooming)
				{
					modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTZooming;
				}
				else if (myImageProc != ImageProcType.RoiNone)
				{
					modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTImageProcessing;
				}

				//roiオブジェクトが存在する場合
				if (DrawRoi.roi != null)
				{
                    DrawRoi.roi.DeleteAllRoiData();		//いったんすべてのROIを削除
                    DrawRoi.roi = null;					//オブジェクト破棄

                    //追加2014/07/24hata
                    if (myRoi != null)
                    {
                        //Roi描画用イベント
                        myRoi.Changed -= new RoiData.ChangedEventHandler(myRoi_Changed);
                        myRoi =null;
                    }
                    this.Cursor = Cursors.Default;		//追加 2009/08/17
					frmRoiMessage.Instance.Close();		//ROIメッセージをアンロード  'v15.10 追加 byやまおか 2009/11/30
				}

				//ROI制御スタートさせる場合
				if (myImageProc != ImageProcType.RoiNone)
				{
					//ROIクラスの生成
                    DrawRoi.roi = new RoiData();

					//描画対象フォームを設定
                    //DrawRoi.roi.SetTarget(this);
                    //Rev20.00 変更 by長野 2015/02/06
                    DrawRoi.roi.SetTarget(this, 1);

                    //追加2014/07/24hata
                    //roiの参照設定
                    myRoi = DrawRoi.roi;

                    //追加2014/07/24hata
                    //Roi描画用イベント
                    myRoi.Changed += new RoiData.ChangedEventHandler(myRoi_Changed);

                }    

                //画像処理ごとの設定
				switch (myImageProc)
				{
					//自動スキャン位置指定
					case ImageProcType.RoiAutoPos:

                        //変更2015/01/22hata
                        //tsbtnGo.Tag = CTResources.LoadResString(15204);		//自動スキャン位置指定
                        tsbtnGo.Text = CTResources.LoadResString(15204);		//自動スキャン位置指定
						tsbtnCircle.Enabled = true;							//円
						ClickToolBarButton(tsbtnCircle);					//「円」ボタンをクリックした状態にする
						break;

					//拡大処理
					case ImageProcType.roiEnlarge:
                        //変更2015/01/22hata
                        //tsbtnGo.Tag = CTResources.LoadResString(StringTable.IDS_EnlargeSimple);
                        tsbtnGo.Text = CTResources.LoadResString(StringTable.IDS_EnlargeSimple);
						modImgProc.EnlargeRatio = 1;						//拡大率の初期化
                        if (!File.Exists(AppValue.OTEMPIMG))
						{
                            File.Delete(AppValue.OTEMPIMG);
						}
						tsbtnSquare.Enabled = true;							//正方形
						ClickToolBarButton(tsbtnSquare);					//正方形ボタンをクリックした状態にする
						break;

					//CT値表示
					case ImageProcType.RoiCTDump:
                        //変更2015/01/22hata
                        //tsbtnGo.Tag = CTResources.LoadResString(StringTable.IDS_CTNumberDisp);	//ＣＴ値表示
						tsbtnGo.Text = CTResources.LoadResString(StringTable.IDS_CTNumberDisp);	//ＣＴ値表示
						myRoi.ManualCut = false;							//削除不可
						tsbtnRectangle.Enabled = true;						//長方形
						ClickToolBarButton(tsbtnRectangle);					//長方形ボタンをクリックした状態にする
						if (hsbImage.Visible)
						{
							myRoi.SelectRoi(myRoi.AddRectangleShape2(511 + hsbImage.Value, 511 + vsbImage.Value, 8, 12, false));
						}
						else
						{
							myRoi.SelectRoi(myRoi.AddRectangleShape2(511, 511, 8, 12, false));
						}
						break;

					//ROI処理
					case ImageProcType.roiProcessing:
                        //変更2015/01/22hata
                        //tsbtnGo.Tag = CTResources.LoadResString(StringTable.IDS_ROIProcessing);
                        tsbtnGo.Text = CTResources.LoadResString(StringTable.IDS_ROIProcessing);
						myRoi.RoiMaxNum = 20;								//描画可能数を最大値(=20)に設定
						tsbtnCircle.Enabled = true;							//円
						tsbtnRectangle.Enabled = true;						//長方形
						//.Buttons("Square").Enabled = True                 '正方形     'v15.10追加 byやまおか 2009/11/30 'v15.10 ImageProcには正方形の処理が無いためコメントアウト by 長野
						tsbtnTrace.Enabled = true;							//トレース
						tsbtnOpen.Enabled = true;							//ROIテーブルを開く
						tsbtnComment.Enabled = true;						//コメント
                        //変更2015/01/22hata
						//tsbtnOpen.Tag = CTResources.LoadResString(StringTable.IDS_ROITable);	//ROIテーブルを開く
						tsbtnOpen.Text = CTResources.LoadResString(StringTable.IDS_ROITable);	//ROIテーブルを開く
						
                        //ClickToolBarButton .Buttons("Rectangle")          '長方形ボタンをクリックした状態にする   'v15.10追加 byやまおか 2009/11/30 'v15.10 コメントを正方形→長方形に修正 by 長野 2010/2/25
						//v19.11 ソフト起動中はROI形状を記憶する by長野 2013/02/20
						switch (DrawRoi.RoiCalRoiNo)
						{	
							case 1:
								ClickToolBarButton(tsbtnCircle);
								break;
								
							case 2:
								ClickToolBarButton(tsbtnRectangle);
								break;
								
							case 6:
								ClickToolBarButton(tsbtnTrace);
								break;
								
							default:
								ClickToolBarButton(tsbtnRectangle);
								break;
						}
                        break;

//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
//					'骨塩定量解析
//					Case RoiBoneDensity
//						.Buttons("Go").Description = LoadResString(IDS_BoneAnalysis)
//						myRoi.RoiMaxNum = 5                                         'ROI描画可能数を５個に設定
//						.Buttons("Circle").Enabled = True                           '円
//						.Buttons("Rectangle").Enabled = True                        '長方形
//						'.Buttons("Square").Enabled = True                           '正方形     'v15.10追加 byやまおか 2009/11/30 'v15.10 ImageProcには正方形の処理が無いためコメントアウト by 長野
//						.Buttons("Trace").Enabled = True                            'トレース
//						.Buttons("Open").Enabled = True                             'ROIテーブルを開く
//						.Buttons("Comment").Enabled = True                          'コメント
//						.Buttons("Open").Description = LoadResString(IDS_ROITable)  'ROIテーブルを開く
//						'ClickToolBarButton .Buttons("Rectangle")                    '長方形ボタンをクリックした状態にする   'v15.10追加 byやまおか 2009/11/30
//						'v19.11 ソフト起動中はROI形状を記憶する by長野 2013/02/20
//						Select Case BoneDensityRoiNo
//
//						Case 1
//
//							ClickToolBarButton .Buttons("Circle")
//
//						Case 2
//
//							ClickToolBarButton .Buttons("Rectangle")
//
//						Case 6
//
//							ClickToolBarButton .Buttons("Trace")
//
//						Case Default
//
//							ClickToolBarButton .Buttons("Rectangle")
//
//						End Select
//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

					//ヒストグラム
					case ImageProcType.roiHistgram:
                        //変更2015/01/22hata
                        //tsbtnGo.Tag = CTResources.LoadResString(StringTable.IDS_Histogram);
						tsbtnGo.Text = CTResources.LoadResString(StringTable.IDS_Histogram);
						modImgProc.CT_Bias = 0;										//CT値中央値
						modImgProc.CT_Int = 10 * 45;								//1メモリ初期値（初期値450）
						tsbtnCircle.Enabled = true;									//円
						tsbtnRectangle.Enabled = true;								//長方形
						tsbtnTrace.Enabled = true;									//トレース
						//ClickToolBarButton .Buttons("Rectangle")                    '長方形ボタンをクリックした状態にする   'v15.10追加 byやまおか 2009/11/30
						//v19.11 ソフト起動中はROI形状を記憶する by長野 2013/02/20
						switch (DrawRoi.HistRoiNo)
						{
							case 1:
								ClickToolBarButton(tsbtnCircle);
								break;
								
							case 2:
								ClickToolBarButton(tsbtnRectangle);
								break;
								
							case 6:
								ClickToolBarButton(tsbtnTrace);
								break;
								
							default:
								ClickToolBarButton(tsbtnRectangle);
								break;
						}
                        break;

					//プロフィール
					case ImageProcType.roiProfile:
                        //変更2015/01/22hata
                        //tsbtnGo.Tag = CTResources.LoadResString(StringTable.IDS_Profile);
                        tsbtnGo.Text = CTResources.LoadResString(StringTable.IDS_Profile);
						modImgProc.CT_Bias = 0;							//CT値中央値
						modImgProc.CT_Int = 10 * 25;					//１目盛初期値（初期値250）
						tsbtnLine.Enabled = true;						//線
						tsbtnHLine.Enabled = true;						//水平線
						tsbtnVLine.Enabled = true;						//垂直線
						ClickToolBarButton(tsbtnLine);					//線ボタンをクリックした状態にする   'v15.10追加 byやまおか 2009/11/30
						break;

					//プロフィールディスタンス
					case ImageProcType.RoiProfileDistance:
                        //変更2015/01/22hata
                        //tsbtnGo.Tag = CTResources.LoadResString(StringTable.IDS_ProfileDistance);
                        tsbtnGo.Text = CTResources.LoadResString(StringTable.IDS_ProfileDistance);
						//CT_Low = 0                                                      'CT値最小初期値（初期値は0）
						modImgProc.CT_Low = 100;										//CT値最小初期値（初期値は300）  'v15.10変更 byやまおか 2009/12/01
						modImgProc.CT_High = 3000;										//CT値最大初期値（初期値は3000）
						modImgProc.CT_Unit = 0;											//CT値連結幅初期値（初期値は0）
						modImgProc.CT_Bias = 0;											//CT値中央値
						modImgProc.CT_Int = 10 * 25;									//１目盛初期値（初期値250）
						modImgProc.P1.x = 511;
						modImgProc.P1.y = 511;
						tsbtnLine.Enabled = true;										//線
						tsbtnPoint.Enabled = true;										//点
						tsbtnOpen.Enabled = true;										//プロフィールディスタンスを開く
                        //変更2015/01/22hata
                        //tsbtnOpen.Tag = CTResources.LoadResString(StringTable.IDS_PDTable);	//プロフィールディスタンステーブル
						tsbtnOpen.Text = CTResources.LoadResString(StringTable.IDS_PDTable);	//プロフィールディスタンステーブル						
                        tsbtnComment.Enabled = true;									//コメント
						ClickToolBarButton(tsbtnLine);									//線ボタンをクリックした状態にする   'v15.10追加 byやまおか 2009/11/30
						break;

					//ズーミング
					case ImageProcType.roiZooming:
                        //変更2015/01/22hata
                        //tsbtnGo.Tag = CTResources.LoadResString(StringTable.IDS_Zooming);
                        tsbtnGo.Text = CTResources.LoadResString(StringTable.IDS_Zooming);
						myRoi.RoiMaxNum = (frmImageInfo.Instance.IsConeBeam ? 1 : 20);			//コーンビーム画像の場合、ROIは1個だけしか描けなくする（通常は20個まで描ける）
						tsbtnSquare.Enabled = true;										//正方形
						tsbtnOpen.Enabled = true;										//ズーミングテーブルを開く
                        //変更2015/01/22hata
                        //tsbtnOpen.Tag = CTResources.LoadResString(StringTable.IDS_ZoomingTable);	//ズーミングテーブル
                        tsbtnOpen.Text = CTResources.LoadResString(StringTable.IDS_ZoomingTable);	//ズーミングテーブル
						tsbtnComment.Enabled = true;									//コメント
						ClickToolBarButton(tsbtnSquare);								//正方形ボタンをクリックした状態にする
						break;

					//寸法測定
					case ImageProcType.roiDistance:
                        //変更2015/01/22hata
                        //tsbtnGo.Tag = CTResources.LoadResString(StringTable.IDS_SizeMeasurement);
						tsbtnGo.Text = CTResources.LoadResString(StringTable.IDS_SizeMeasurement);
						myRoi.RoiMaxNum = 20;											//ROI描画可能数は20個
						tsbtnLine.Enabled = true;										//線
						tsbtnComment.Enabled = true;									//コメント
						ClickToolBarButton(tsbtnLine);									//「線」ボタンをクリックした状態にする
						break;

					//角度測定
					case ImageProcType.RoiAngle:
                        //変更2015/01/22hata
                        //tsbtnGo.Tag = CTResources.LoadResString(12835);					//角度測定
                        tsbtnGo.Text = CTResources.LoadResString(12835);					//角度測定
						tsbtnLine.Enabled = true;										//線
						ClickToolBarButton(tsbtnLine);									//「線」ボタンをクリックした状態にする
						break;

					//ROI制御終了
					default:
                        //変更2015/01/22hata
                        //tsbtnGo.Tag = "";
						tsbtnGo.Text = "";
						modImgProc.PRDPoint = 0;
						//指定点数クリア

                        //座標入力フォームアンロード
                        frmInputRoiData.Instance.Close();
                        frmLineInput.Instance.Close();
                        HoriVertForm.Instance.Close();

                        //結果フォームをアンロード
                        frmResult.Instance.Close();

                        //ツールバー上のボタンをオフ・使用不可状態にする
						foreach (ToolStripItem Item in Toolbar1.Items)
                        {
                            if (Item.GetType() == typeof(ToolStripButton))
                            {
                                ToolStripButton btn = Item as ToolStripButton;
                                btn.Checked = false;
                                btn.Enabled = false;
                            }
                        }

						//右クリック時のメニューの調整
						mnuROIEditCopy.Enabled = false;				//ｺﾋﾟｰ(&C)
						mnuROIEditPaste.Enabled = false;			//貼り付け(&P)
						mnuROIEditDelete.Enabled = false;			//ROI削除(&D)
						mnuROIEditAllDelete.Enabled = false;		//すべてのROI削除(&A)
						mnuRoiInput.Enabled = false;				//ROI座標入力
						break;
				}

				//共通設定
				if (myRoi != null)
				{   
                    //変更2015/01/22hata
                    //tsbtnGo.ToolTipText = StringTable.GetResString(StringTable.IDS_Exe, tsbtnGo.Tag.ToString());
                    tsbtnGo.ToolTipText = StringTable.GetResString(StringTable.IDS_Exe, tsbtnGo.Text);
					tsbtnArrow.Enabled = (myRoi.RoiMaxNum > 1);		//ROI選択
					if (tsbtnOpen.Enabled)
					{
                        //変更2015/01/22hata
						//tsbtnSave.Tag = tsbtnOpen.Tag;
                        //tsbtnOpen.ToolTipText = StringTable.GetResString(StringTable.IDS_Open, Convert.ToString(tsbtnOpen.Tag));	//～を開く
                        //tsbtnSave.ToolTipText = StringTable.GetResString(StringTable.IDS_Save, Convert.ToString(tsbtnOpen.Tag));	//～の保存
                        tsbtnSave.Text = tsbtnOpen.Text;
                        tsbtnOpen.ToolTipText = StringTable.GetResString(StringTable.IDS_Open, tsbtnOpen.Text);	//～を開く
                        tsbtnSave.ToolTipText = StringTable.GetResString(StringTable.IDS_Save, tsbtnOpen.Text);	//～の保存
                    }
				}

				//「実行」ボタン、「キャンセル」ボタン、メッセージ欄の設定
				switch (myImageProc)
				{
					case ImageProcType.RoiNone:
					case ImageProcType.RoiCTDump:
					case ImageProcType.RoiBoneDensity:
					case ImageProcType.RoiAutoPos:
//						cmdRoiOk.Enabled = False
//						cmdRoiCancel.Enabled = False
						lblPrompt.Text = "";
						break;

					default:
//						cmdRoiOk.Enabled = True
//						cmdRoiCancel.Enabled = True
						tsbtnGo.Enabled = true;
						tsbtnExit.Enabled = true;
                        //変更2015/01/22hata
                        //lblPrompt.Text = Convert.ToString(tsbtnGo.Tag);
                        lblPrompt.Text = tsbtnGo.Text;
						break;
				}
			}
		}

		//*******************************************************************************
		//機　　能： 画像処理等で生成した一時的な画像ファイルを表示する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v10.2 2005/08/22 (SI3)間々田    新規作成
		//*******************************************************************************
		public void DispTempImage(bool UseDispImage = false)
		{
			if (UseDispImage)
			{
				DoDispImage(AppValue.OTEMPIMG);
			}
			else
			{
				//BMPの表示
				ChangePicture();
			}

			//変更フラグセット
			myChanged = true;
		}

		//*******************************************************************************
		//機　　能： 元画像を表示
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v10.2 2005/08/22 (SI3)間々田    新規作成
		//*******************************************************************************
		public void DispOrgImage()
		{
            UpdateDispinf(myTarget, frmImageInfo.Instance.WindowLevel, frmImageInfo.Instance.WindowWidth);
		}

		//*******************************************************************************
		//機　　能： Targetプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 現在表示中の画像ファイル
		//
		//履　　歴： v10.2 2005/08/22 (SI3)間々田    新規作成
		//*******************************************************************************
		public string Target
		{
			get { return myTarget; }
			set
			{
				myTarget = value;

				//スキャン画像フォームの更新
				frmImageInfo.Instance.Target = modLibrary.RemoveExtension(myTarget, ".img");
                float? gamma = null;

				//コモンを書き換え後、画像表示
				if (string.IsNullOrEmpty(value))
				{
					//modDispIinf.GetDispinf(dispinf);
                    CTSettings.dispinf.Load();

                    //UpdateDispinf , dispinf.level, dispinf.Width
                    gamma = CTSettings.dispinf.Data.Gamma;
                    UpdateDispinf(null, CTSettings.dispinf.Data.level, CTSettings.dispinf.Data.width, ref gamma);	//v19.00 引数追加 by長野 2012/02/22
                    if (gamma.HasValue)
                    {
                        CTSettings.dispinf.Data.Gamma = gamma.Value;
                    }
                }
				else
				{
                    ////UpdateDispinf FileName, frmImageInfo.WindowLevel, frmImageInfo.WindowWidth
                    //UpdateDispinf(value, frmImageInfo.Instance.WindowLevel, frmImageInfo.Instance.WindowWidth, ref frmImageInfo.Instance.gamma);
                    gamma = frmImageInfo.Instance.gamma;
                    UpdateDispinf(value, frmImageInfo.Instance.WindowLevel, frmImageInfo.Instance.WindowWidth, ref gamma);
                    if (gamma.HasValue)
                    {
                        frmImageInfo.Instance.gamma = gamma.Value;
                    }
                }

				//カレント画像名を画像フォームのタイトルバーに表示
				UpdateCaption();

				//画像サーチ画面を更新
                if (frmImageControl.Instance != null)
				    frmImageControl.Instance.Target = myTarget;
			}
		}

		//*******************************************************************************
		//機　　能： WindowLevelプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v10.2 2005/08/22 (SI3)間々田    新規作成
		//*******************************************************************************
		public int WindowLevel
		{
			get { return myWindowLevel; }
			set
			{
				if (value == myWindowLevel)
				{
					return;
				}

				//コモンを書き換え後、画像表示
				UpdateDispinf(null, value);

				//付帯情報を更新する
				if (!myChanged)
				{
					frmImageInfo.Instance.WindowLevel = value;
				}
			}
		}

		//*******************************************************************************
		//機　　能： WindowLevelプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v10.2 2005/08/22 (SI3)間々田    新規作成
		//*******************************************************************************
		public int WindowWidth
		{
			get { return myWindowWidth; }
			set
			{
				if (value == myWindowWidth)
				{
					return;
				}

				//コモンを書き換え後、画像表示
				UpdateDispinf(null, null, value);

				//付帯情報を更新する
				if (!myChanged)
				{
					frmImageInfo.Instance.WindowWidth = value;
				}
			}
		}

		//*******************************************************************************
		//機　　能： Gammaプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v17.99 2012/02/21 (検S1)長野    新規作成
		//*******************************************************************************
		public float GAMMA
		{
			get { return myGamma; }
			set
			{
				if (value == myGamma)
				{
					return;
				}

				//コモンを書き換え後、画像表示
                float? gamma = value;
                UpdateDispinf(null, null, null, ref gamma);
                if (gamma.HasValue)
                {
                    value = gamma.Value;
                }

				//付帯情報を更新する
				if (!myChanged)
				{
					frmImageInfo.Instance.gamma = (float)Math.Round(value, 2, MidpointRounding.AwayFromZero);
				}
			}
		}

		//*******************************************************************************
		//機　　能： コモンを書き換え後、画像表示
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v10.2 2005/08/22 (SI3)間々田    新規作成
		//*******************************************************************************
		//Private Sub UpdateDispinf(Optional ByVal theTarget As Variant, Optional ByVal theLevel As Variant, Optional ByVal theWidth As Variant)
		//private void UpdateDispinf(string theTarget = null, int? theLevel = null, int? theWidth = null, ref float? theGamma = null)		//v19.00 引数追加 by長野 2012/02/22
        private void UpdateDispinf(string theTarget = null, int? theLevel = null, int? theWidth = null)		//v19.00 引数追加 by長野 2012/02/22
        {
            float? gamma = null;
            UpdateDispinf(theTarget, theLevel, theWidth ,ref gamma);
        }
        
        private void UpdateDispinf(string theTarget, int? theLevel, int? theWidth, ref float? theGamma)		//v19.00 引数追加 by長野 2012/02/22
		{
			//dispinf読み込み
			//modDispinf.GetDispinf(CTSettings.dispinf.Data);
            var _with = CTSettings.dispinf;
            _with.Load();

			//コモンに階調(Wl/WW)書き込み
			if (theLevel != null)
			{
                //追加2014/07/19hata
                if (frmImageControl.Instance.cwneWindowLevel.Maximum < theLevel)
                    //2014/11/07hata キャストの修正
                    //theLevel = (int)frmImageControl.Instance.cwneWindowLevel.Maximum;
                    theLevel = Convert.ToInt32(frmImageControl.Instance.cwneWindowLevel.Maximum);
                if (frmImageControl.Instance.cwneWindowLevel.Minimum > theLevel)
                    //2014/11/07hata キャストの修正
                    //theLevel = (int)frmImageControl.Instance.cwneWindowLevel.Minimum;
                    theLevel = Convert.ToInt32(frmImageControl.Instance.cwneWindowLevel.Minimum);

                _with.Data.level = theLevel.Value;
				myWindowLevel = theLevel.Value;
				frmImageControl.Instance.cwneWindowLevel.Value = theLevel.Value;
			}

            if (theWidth != null )
            {
                //追加2014/07/19hata
                if (frmImageControl.Instance.cwneWindowWidth.Maximum < theWidth.Value)
                    //2014/11/07hata キャストの修正
                    //theWidth = (int)frmImageControl.Instance.cwneWindowWidth.Maximum;
                    theWidth = Convert.ToInt32(frmImageControl.Instance.cwneWindowWidth.Maximum);
                if (frmImageControl.Instance.cwneWindowWidth.Minimum > theWidth.Value)
                    //2014/11/07hata キャストの修正
                    //theWidth = (int)frmImageControl.Instance.cwneWindowWidth.Minimum;
                    theWidth = Convert.ToInt32(frmImageControl.Instance.cwneWindowWidth.Minimum);

                _with.Data.width = theWidth.Value;
				myWindowWidth = theWidth.Value;
				frmImageControl.Instance.cwneWindowWidth.Value = theWidth.Value;
			}
            
			//v19.00 ガンマを追加　by長野 2012-02-21
			if (theGamma != null)
			{
				if (theGamma.Value == 0)
				{
					theGamma = 1.0f;
				}

                _with.Data.Gamma = theGamma.Value;
				myGamma = theGamma.Value;
				frmImageControl.Instance.cwneGamma.Value = Convert.ToDecimal(Math.Round(theGamma.Value, 2, MidpointRounding.AwayFromZero));
			}

			//ｶﾗｰ処理時（ｵﾘｼﾞﾅﾙｶﾗｰ(%)かﾚｲﾝﾎﾞｰｶﾗｰ時）のみ、CT値の下限を設定
            if (_with.Data.colormode != 0)
			{
                //2014/11/07hata キャストの修正
                //_with.Data.color_min = _with.Data.level - _with.Data.width / 2;
                _with.Data.color_min = _with.Data.level - Convert.ToInt32(_with.Data.width / 2F);
            }

			if (theTarget != null)
            {
				//パス名とファイル名をコモンに登録
                //変更2014/07/19hata
                ////modLibrary.SetField(modLibrary.AddExtension(Directory.GetParent(theTarget), "\\"), _with.d_exam);
                ////modLibrary.SetField(Path.GetFileNameWithoutExtension(theTarget), _with.d_id);
                //_with.Data.d_exam.SetString(modLibrary.AddExtension(Path.GetDirectoryName(theTarget), "\\"));
                //_with.Data.d_id.SetString(Path.GetFileNameWithoutExtension(theTarget));
                string path = "";
                try
                {
                    path = Path.GetDirectoryName(theTarget);
                    _with.Data.d_exam.SetString(modLibrary.AddExtension(path, "\\"));
                    _with.Data.d_id.SetString(Path.GetFileNameWithoutExtension(theTarget));
                }
                catch
                {
                    _with.Data.d_exam.SetString("");
                    _with.Data.d_id.SetString("");

                }

 				myTarget = theTarget;
				myChanged = false;
			}

			//dispinf書き込み
			//modDispinf.PutDispinf(CTSettings.dispinf.Data);
            _with.Put();

			//画像を表示
            DoDispImage(myChanged ? AppValue.OTEMPIMG : myTarget);
		}

		//*******************************************************************************
		//機　　能： picObj上のX座標位置を求める
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public int GetXOnPicObj(float x)
		{
			int result = 0;

			if (hsbImage.Visible)
			{
                //2014/11/07hata キャストの修正
                //result = (int)x + hsbImage.Value;
                result = Convert.ToInt32(x) + hsbImage.Value;
            }
			else
			{
                //2014/11/07hata キャストの修正
                //result = (int)x;
                result = Convert.ToInt32(x);
			}

			return result;
		}

		//*******************************************************************************
		//機　　能： picObj上のY座標位置を求める
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public int GetYOnPicObj(float y)
		{
			int result = 0;

			if (vsbImage.Visible)
			{
                //2014/11/07hata キャストの修正
                //result = (int)y + vsbImage.Value;
                result = Convert.ToInt32(y) + vsbImage.Value;
            }
			else
			{
                //2014/11/07hata キャストの修正
                //result = (int)y;
                result = Convert.ToInt32(y);
            }

			return result;
		}

		//*******************************************************************************
		//機　　能： Form上のX座標位置を求める
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public float GetXOnForm(int x)
		{
			float resulte = 0;

			if (hsbImage.Visible)
			{
				resulte = x - hsbImage.Value;
			}
			else
			{
				resulte = x;
			}

            return resulte;
		}

		//*******************************************************************************
		//機　　能： Form上のY座標位置を求める
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public float GetYOnForm(int y)
		{
			float result = 0;

			if (vsbImage.Visible)
			{
				result = y - vsbImage.Value;
			}
			else
			{
				result = y;
			}

			return result;
		}

		//*************************************************************************************************
		//機　　能： フォームロード時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void frmScanImage_Load(object sender, EventArgs e)
		{
			//変数初期化
			myChanged = false;

			//各コントロールのキャプションにリソースから取得した文字列をセットする
			SetCaption();

			//フォームの表示位置：階調変換／画像サーチ画面の右側
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			With frmImageControl
				'Me.Move .Left + .width, 0, 15450, 15735
				'Me.Move .Left + .width, 0, 15450, (1024 + 25) * Screen.TwipsPerPixelY
				Me.Move .Left + .Width, 0, 15450, (1024 + Toolbar1.Height + 25) * Screen.TwipsPerPixelY
			End With
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            this.Location = new Point(frmImageControl.Instance.Left + frmImageControl.Instance.Width);
			this.Size = new Size(1030, 1024 + Toolbar1.Height + 25);

			byEvent = true;

			//ProcOCXコントロールへの参照設定                'v10.2追加 by 間々田 2005/07/04
			//modImageProc.MyProcOCX = CTImageProc;

			//ウィンドウレベル・ウィンドウ幅の初期値を取得
			//modDispinf.GetDispinf(dispinf);
            CTSettings.dispinf.Load();
            myWindowLevel = CTSettings.dispinf.Data.level;
			myWindowWidth = CTSettings.dispinf.Data.width;
			//v19.00 追加 by長野 2012/06/19
			myGamma = CTSettings.dispinf.Data.Gamma;

			//ターゲットプロパティ初期化
			Target = "";
            
            //Toolbar1とfraPrompt位置
            pnlControl.SetBounds(hsbImage.Left, hsbImage.Top + hsbImage.Height, this.Width, Toolbar1.Height);
            Toolbar1.Parent = pnlControl;
            fraPrompt.Parent = pnlControl;
            fraPrompt.SetBounds(Toolbar1.Left + Toolbar1.Width, Toolbar1.Top, pnlControl.Width - Toolbar1.Width, Toolbar1.Height);

            //2014/11/07hata キャストの修正
            //lblPrompt.Top = fraPrompt.Height / 2 - lblPrompt.Height / 2;
            lblPrompt.Top = Convert.ToInt32(fraPrompt.Height / 2F) - Convert.ToInt32(lblPrompt.Height / 2F);
            lblPrompt.Text = "";
			//fraPrompt.Location = new Point(Toolbar1.Left + Toolbar1.Width, Toolbar1.Top);

			//ImageProcプロパティ初期化
			myImageProc = ImageProcType.RoiNone;

			//拡大・縮小ボタンのキャプションの設定
			UpdateMagnifyCaption();

        }

		//*******************************************************************************
		//機　　能： 拡大・縮小ボタンのキャプションの設定
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/25 (SI1)間々田   新規作成
		//*******************************************************************************
		public void UpdateMagnifyCaption()
		{
			//4096対応 ボタンのキャプションの設定方法を変更 v16.10 by 長野
			//ボタンのキャプションの設定（縮小/拡大）
			//mnuEnlarge.Caption = LoadResString(IIf(scansel.disp_size = 1, IDS_btnReduction, IDS_btnEnlarge))

			if (CTSettings.scansel.Data.disp_size >= 1)
			{
				mnuEnlarge.Text = CTResources.LoadResString(StringTable.IDS_btnReduction);
			}
			else
			{
				mnuEnlarge.Text = CTResources.LoadResString(StringTable.IDS_btnEnlarge);
			}
		}

		//*************************************************************************************************
		//機　　能： フォーム描画処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void frmScanImage_Paint(object sender, PaintEventArgs e)
		{

            Graphics g = e.Graphics;

            //補間モード
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            //画像の描画
            DoPaintPicture(e.Graphics);
            
            //Roi制御なしの場合、何もしない
            if (myRoi == null)
            {
                return;
            }

            //Roiの表示
            myRoi.DispRoi(g);
            
        }

        ////protected override void OnPaint(PaintEventArgs e)
        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    base.OnPaintBackground(e);
            
        //    DoPaintPicture(e.Graphics);

        //     //Roi制御なしの場合、何もしない
        //    if (myRoi == null)
        //    {
        //        return;
        //    }

        //    //Roiの表示
        //    myRoi.IndicateRoi();
            
        //}
 
		//*************************************************************************************************
		//機　　能： フォーム上でダブルクリックした時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void frmScanImage_DoubleClick(object sender, EventArgs e)
		{
			DoubleClickOn = (LastButton == MouseButtons.Left);
		}

		//*************************************************************************************************
		//機　　能： マウスボタンを押した時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void frmScanImage_MouseDown(object sender, MouseEventArgs e)
		{
			//Roi制御なしの場合、何もしない
			if (myRoi == null)
			{
				//右クリック時
				if (e.Button == MouseButtons.Right)
				{
					//ポップアップメニューを表示
                    // スキャン制御画面のスクリーン上での領域を求める
                    var p1 = this.PointToScreen(new Point(e.X, e.Y));
                    mnuPopUp.Show(p1);
				}
			}
			else
			{
                //テスト追加2014/07/14hata_<変更>
                //int shift = 0;
                //if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                //{
                //    shift = 1;
                //}				
                //Roi制御時のマウスダウン処理
                //myRoi.MouseDown(e.Button, shift, GetXOnPicObj(e.X), GetYOnPicObj(e.Y));
                myRoi.MouseDown(e.Button, Control.ModifierKeys, GetXOnPicObj(e.X), GetYOnPicObj(e.Y));

			}
		}

		//*************************************************************************************************
		//機　　能： マウスポインタが画面を移動した時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void frmScanImage_MouseMove(object sender, MouseEventArgs e)
		{
			//Roi制御なしの場合、何もしない
			if (myRoi == null)
			{
				return;
			}

			//座標補正
			int X = modLibrary.CorrectInRange(e.X, 0, hsbImage.Width - 1);
			int Y = modLibrary.CorrectInRange(e.Y, 0, vsbImage.Height - 1);

			//Roi制御時のマウスムーブ処理
			myRoi.MouseMove(GetXOnPicObj(X), GetYOnPicObj(Y));
		}

		//*************************************************************************************************
		//機　　能： マウスボタンを離した時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void frmScanImage_MouseUp(object sender, MouseEventArgs e)
		{
			//Roi制御なしの場合、何もしない
			if (myRoi == null)
			{
				return;
			}

			//座標補正
			int X = modLibrary.CorrectInRange(e.X, 0, hsbImage.Width - 1);
			int Y = modLibrary.CorrectInRange(e.Y, 0, vsbImage.Height - 1);

            //テスト追加2014/07/14hata_<変更>
            //int shift = 0;
            //if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            //{
            //    shift = 1;
            //}

            //Roi制御時のマウスアップ処理
            //テスト追加2014/07/14hata_<変更>
            myRoi.MouseUp(e.Button, Control.ModifierKeys, GetXOnPicObj(X), GetYOnPicObj(Y), DoubleClickOn);

			//今回クリックしたマウスボタン（ダブルクリック時に使用）
			LastButton = e.Button;

			//ダブルクリックフラグオフ
			DoubleClickOn = false;
		}

		//*******************************************************************************
		//機　　能： フォームのキャプションを更新する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void UpdateCaption(string RoiInfo = "")
		{
			//基本のキャプション
			this.Text = CTResources.LoadResString(12253);	//断面像

			//表示画像ファイル名を付加
			if (!string.IsNullOrEmpty(myTarget))
			{
				this.Text = this.Text + " - " + myTarget;
			}

			//Roi情報付加
			if (!string.IsNullOrEmpty(RoiInfo))
			{
				this.Text = this.Text + " " + RoiInfo;
			}
		}

		//*******************************************************************************
		//機　　能： 水平スライダー変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足：
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void hsbImage_ValueChanged(object sender, EventArgs e)
		{
			if (byEvent)
            {
                #region <確認！！>　2014/06/17
                //再描画　
				//DoPaintPicture();
    		    //this.Invalidate();
                this.Refresh();
                #endregion
            }

			offset.x = hsbImage.Value;
		}

		//*******************************************************************************
		//機　　能： 垂直スライダー変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足：
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void vsbImage_ValueChange(object sender, EventArgs e)
		{
			if (byEvent)
            {
                #region <確認！！>　2014/06/17
                //再描画
                //DoPaintPicture();
                //this.Invalidate();
                this.Refresh();
                #endregion 
            }

            offset.y = vsbImage.Value;
		}

        #region <確認！！>　2014/06/17
        //*******************************************************************************
		//機　　能： Picture オブジェクトを描画します
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足：
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		//private void DoPaintPicture
		private void DoPaintPicture(Graphics g)
		{
            //	Me.AutoRedraw = True

			//    If hsbImage.Visible Then
			//        Me.PaintPicture picObj, 0, 0, , , hsbImage.value, vsbImage.value, hsbImage.width, vsbImage.Height
			//    Else
			//        Me.PaintPicture picObj, 0, 0, Me.ScaleWidth, Me.ScaleHeight, 0, 0, PicWidth, PicHeight
			//    End If

            if (picObj != null)
            {
                //Graphics g = this.CreateGraphics();

                //補間モード
                g.InterpolationMode = InterpolationMode.NearestNeighbor;

                if (hsbImage.Visible)
                {
                    //'Me.PaintPicture picObj, 0, 0, 1024, 1024, hsbImage.value, vsbImage.value, hsbImage.width, vsbImage.Height
                    //Me.PaintPicture picObj, 0, 0, , , hsbImage.Value, vsbImage.Value, hsbImage.Width, vsbImage.Height

                    Rectangle rDst = new Rectangle(0, 0, hsbImage.Width, vsbImage.Height);
                    Rectangle rSrc = new Rectangle(hsbImage.Value, vsbImage.Value, hsbImage.Width, vsbImage.Height);

                    g.DrawImage(picObj, rDst, rSrc, GraphicsUnit.Pixel);
                }
                else
                {
                    //Me.PaintPicture picObj, 0, 0, 1024, 1024

                    Rectangle rDst = new Rectangle(0, 0, 1024, 1024);
                    
                    g.DrawImage(picObj, rDst);

                }

                //g.Dispose();

                //Me.AutoRedraw = False
            }
           
           //追加2014/07/19hata
           else
            {
                //黒で塗る
                g.FillRectangle(Brushes.Black, ClientRectangle);

            }


        }
        #endregion

        //*******************************************************************************
		//機　　能： フォームに画像を表示します。
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足：
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void DispPicture()
		{
			//水平・垂直スライダーの設定
//			hsbImage.Visible = (PicWidth > Me.ScaleWidth) And (theMagnify = 1)
//			vsbImage.Visible = (PicHeight > Me.ScaleHeight) And (theMagnify = 1)
			//v16.10 4096画素対応のためコメントアウト by 長野
			//hsbImage.Visible = (myPicWidth > 1024) And (scansel.disp_size = 1)
			//vsbImage.Visible = (myPicHeight > 1024) And (scansel.disp_size = 1)
			//v16.10 4096対応のため，条件を変更
			hsbImage.Visible = (myPicWidth > 1024) && (CTSettings.scansel.Data.disp_size >= 1);
            vsbImage.Visible = (myPicHeight > 1024) && (CTSettings.scansel.Data.disp_size >= 1);

			lblSpace.Visible = hsbImage.Visible & vsbImage.Visible;
//			hsbImage.width = IIf(hsbImage.Visible, Me.ScaleWidth - vsbImage.width, Me.ScaleWidth)
//			vsbImage.Height = IIf(vsbImage.Visible, Me.ScaleHeight - hsbImage.Height, Me.ScaleHeight)
			hsbImage.Width = (hsbImage.Visible? 1024 - vsbImage.Width : 1024);
			vsbImage.Height = (vsbImage.Visible? 1024 - hsbImage.Height : 1024);

			//変更2014/12/22hata_dNet
　　　　　　//hsbImage.Maximum = myPicWidth - hsbImage.Width;
　　　　　　//vsbImage.Maximum = myPicHeight - vsbImage.Height;
  　　　　　hsbImage.Maximum = myPicWidth - hsbImage.Width + hsbImage.LargeChange - 1;
    　　　　vsbImage.Maximum = myPicHeight - vsbImage.Height + vsbImage.LargeChange - 1;

			//v16.10 4096対応のためコメントアウト 長野 10/01/29
//			myMagnify = 1
//			If (scansel.disp_size = 0) And (myPicWidth = 2048) Then
//				myMagnify = 2
//			End If
			//v16.10 4096対応のため，画像の拡大率の決め方を変更する by 長野 10/01/29

			if (CTSettings.scansel.Data.disp_size == 0)
			{
				switch (myPicWidth)
				{
					case 2048:
						myMagnify = 2;
						break;

					case 4096:
						myMagnify = 4;
						break;

					default:
						myMagnify = 1;
						break;
				}
			}
			else
			{
				myMagnify = 1;
			}

            #region <確認！！>　2014/06/17
			//画像描画処理
            //DoPaintPicture();
           // this.Invalidate();
            this.Refresh();
            #endregion 

        }

		//*******************************************************************************
		//機　　能： フォームの Picture オブジェクトを更新します
		//
		//           変数名          [I/O] 型        内容
		//引　　数： TargetBMP       [I/ ] String    処理対象BMPファイル
		//戻 り 値： なし
		//
		//補　　足： 処理対象BMPファイルのデフォルト→C:\CT\IMAGE\TEMP\IMAGEDISPLAY.BMP
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		//Private Sub ChangePicture(Optional ByVal TargetBMP As String = DISPBMP)'v15.0変更 by 間々田 2009/03/16
		private void ChangePicture()
		{
			int StartTime = 0;
            StartTime = Winapi.GetTickCount();

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			On Error Resume Next
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            picObj = null;
            
			try
			{
				do
				{
					try
					{
						//BMPファイルをPictureオブジェクトに取り込み
                        //Set picObj = LoadPicture(TargetBMP)//v15.0変更 by 間々田 2009/03/16
                        picObj = CreateImage(AppValue.DISPBMP);
                    
                    }
					catch
					{
                    }
				} while (picObj == null && (Winapi.GetTickCount() - StartTime < 300));
			}
			catch
			{
			}

			if (picObj == null)
			{
				return;
			}

			//画像の横縦の画素サイズを求めておく
			myPicWidth = picObj.Width;
			myPicHeight = picObj.Height;

			//2048画素以上の場合でRoiが描画されていない時、拡大・縮小を使用可とする　'v16.10 条件を2048画素以上とした by 長野 2010/03/02
			mnuEnlarge.Enabled = (myPicWidth >= 2048) && (!IsExistRoi);

			//If myPicWidth = 2048 Then
			//v19.12　4096が抜けているため修正 by長野 2013/03/12
			if (myPicWidth == 2048 || myPicWidth == 4096)
			{
				hsbImage.Value = offset.x;
				vsbImage.Value = offset.y;
			}
			else
			{
				//水平・垂直スライダーの位置の初期化
				byEvent = false;
				hsbImage.Value = 0;
				vsbImage.Value = 0;
				byEvent = true;
			}

			//表示
			DispPicture();
		}

		//*******************************************************************************
		//機　　能： 指定されたＣＴ画像ファイルからBMPファイルを生成する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： FileName
		//戻 り 値： なし
		//
		//補　　足：
		//
		//履　　歴： V1.00  2005/07/04 (SI3)間々田   新規作成
		//*******************************************************************************
		public bool DoDispImage(string FileName = "")
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			'戻り値初期化
//			DoDispImage = False
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//ヌル文字が指定されている
			if (string.IsNullOrEmpty(FileName))
			{
				//BMPファイルを消去
				this.BackgroundImage = null;
                
                //黒で塗る  //追加2014/07/19hata
                picObj = null;
                //this.Invalidate();
                this.Refresh();

			}

			//画像表示処理実行（ビットマップ変換）
			else if (ImgProc.DispImage(FileName))
			{
				//BMPの表示
				ChangePicture();

			}
			else
			{
				//メッセージ表示：画像表示に失敗しました。指定された画像ファイルがないかも知れません。
				MessageBox.Show(StringTable.GetResString(9449, CTResources.LoadResString(12480)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}

			//戻り値セット
			return true;
		}

		//*******************************************************************************
		//機　　能： ２値化画像表示
		//
		//           変数名          [I/O] 型        内容
		//引　　数： fileName        [I/ ] String    処理対象ファイル
		//           LowValue        [I/ ] Integer   閾値
		//           HighValue       [I/ ] Integer   閾値
		//戻 り 値：                 [ /O] Boolean   True:成功　False:失敗
		//
		//補　　足：
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public bool DoDispBitImage(int LowValue, int HighValue)
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			'戻り値初期化
//			DoDispBitImage = False
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//画像２値化
			if (!ImgProc.DispBitImage(LowValue, HighValue, myTarget))
			{
				//メッセージ表示：画像２値化処理に失敗しました。
				MessageBox.Show(CTResources.LoadResString(9450), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			//一時的な画像ファイルを表示する
			DispTempImage();

			//変更フラグを立てない
			myChanged = false;

			//戻り値セット
			return true;
		}

		//*******************************************************************************
		//機　　能： プロフィール実行処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： DistanceOn      [I/ ] Boolean   ディスタンス処理 True:あり False:なし
		//戻 り 値：                 [ /O] Boolean   True:成功　False:失敗
		//
		//補　　足：
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private bool DoProfile(bool DistanceOn = false)
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Static x1           As Integer
			Static y1           As Integer
			Static x2           As Integer
			Static y2           As Integer
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            //Dim ProfTbl(2047)   As Long     'プロフィール＆ディスタンスラインバッファ
			//v16.10 4096対応 ラインプロフィールのバッファ 10/02/09
			int[] ProfTbl = new int[4096];		//プロフィール＆ディスタンスラインバッファ
			int PSNum = 0;						//プロフィール＆ディスタンス測定データ数
			int PS = 0;							//プロフィール＆ディスタンス測定開始位置
			int DispMode = 0;					//プロフィール測定方向フラグ

			//戻り値初期化
			bool result = false;

			//座標取得：取得できない場合は前回値を使用
			if (myRoi.GetLineShape(1, ref DoProfile_x1, ref DoProfile_y1, ref DoProfile_x2,ref  DoProfile_y2))
			{
				if (modImgProc.PRDPoint == 2)
				{
                    //modImgProc.P1.x = static_DoProfile_x1;
                    //modImgProc.P1.y = static_DoProfile_y1;
                    //modImgProc.P2.x = static_DoProfile_x2;
                    //modImgProc.P2.y = static_DoProfile_y2;
                    modImgProc.P1.x = DoProfile_x1;
                    modImgProc.P1.y = DoProfile_y1;
                    modImgProc.P2.x = DoProfile_x2;
                    modImgProc.P2.y = DoProfile_y2;
                }
				DoProfile_x1 = myMagnify * DoProfile_x1;
				DoProfile_y1 = myMagnify * DoProfile_y1;
				DoProfile_x2 = myMagnify * DoProfile_x2;
				DoProfile_y2 = myMagnify * DoProfile_y2;
			}

			//座標チェック
			if (DoProfile_x1 == DoProfile_x2 && DoProfile_y1 == DoProfile_y2)
			{
				//メッセージ表示：
				//MsgBox "始点と終点の座標が同じため，プロフィール処理に失敗しました。", vbExclamation
				MessageBox.Show(CTResources.LoadResString(20075), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);	//ストリングテーブル化 'v17.60 by長野 2011/05/22
				return result;
			}

			//マウスポインタを砂時計に変更
			//Screen.MousePointer = vbHourglass
			this.Cursor = Cursors.WaitCursor;		//v15.10変更 byやまおか 2009/12/01

			//測定２点のプロフィールグラフを作成
           if (!ImgProc.GetProfile(DoProfile_x1, DoProfile_y1, DoProfile_x2, DoProfile_y2, (int)modImgProc.CT_Bias, (int)modImgProc.CT_Int, ref DispMode, ref PS, ref PSNum, ProfTbl))
			{
				//マウスポインタを元に戻す
				//Screen.MousePointer = vbDefault
				this.Cursor = Cursors.Default;		//v15.10変更 byやまおか 2009/12/01
				//メッセージ表示：プロフィールに失敗しました。
				MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_WentWrong, StringTable.IDS_Profile), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return result;
			}

			if (DistanceOn)
			{
				//測定２点の閾値内を太線で描画して，距離＆角度算出
				if (!ImgProc.K3Pro(DoProfile_x1, DoProfile_y1, DoProfile_x2, DoProfile_y2, modImgProc.CT_Bias, modImgProc.CT_Int, DispMode, PS, PSNum, ProfTbl, modImgProc.CT_Low, modImgProc.CT_High, modImgProc.CT_Unit))
				{
					//マウスポインタを元に戻す
					//Screen.MousePointer = vbDefault
					this.Cursor = Cursors.Default;		//v15.10変更 byやまおか 2009/12/01
					//メッセージ表示：ﾌﾟﾛﾌｨｰﾙ&ﾃﾞｨｽﾀﾝｽ閾値処理に失敗しました。
					MessageBox.Show(CTResources.LoadResString(9435), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return result;
				}
			}
			else if (myCTInputForm == null)
			{
				//ツールバーを使用不可にする
				ToolBarEnabled = false;

				//線分をいったんカット
				//myRoi.DeleteAllRoiData     'v15.10削除 byやまおか 2009/12/01

//				'制御を行えないようにする
//				myRoi.RoiMaxNum = 0

				//このフォームを使用不可にする
				this.Enabled = false;

				//CT値入力（IntervalとBias）フォーム表示
				myCTInputForm = CTInputForm.Instance;

                //CTinputFormのイベント設定
                myCTInputForm.Clicked += new CTInputForm.ClickedEventHandler(myCTInputForm_Clicked);

				myCTInputForm.Show(frmCTMenu.Instance);
			}

			//一時的な画像ファイルを表示する
			DispTempImage();

			//マウスポインタを元に戻す
			//Screen.MousePointer = vbDefault
			this.Cursor = Cursors.Default;		//v15.10変更 byやまおか 2009/12/01

			//戻り値の設定
			return true;
		}

		//*******************************************************************************
		//機　　能： 拡大処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値：                 [ /O] Boolean   True:成功　False:失敗
		//
		//補　　足：
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public bool DoEnlarge()
		{
			//戻り値初期化
			bool result = false;

			int x = 0;
			int y = 0;
			int r = 0;

			//ROI座標取得処理
			if (!myRoi.GetSquareShape(1,ref x,ref y,ref r))
			{
				//メッセージ表示：測定するROIが設定されていません。
				MessageBox.Show(CTResources.LoadResString(StringTable.IDS_NotFoundROI),
								Application.ProductName,
								MessageBoxButtons.OK,
								MessageBoxIcon.Exclamation);
				return result;
			}

			//2048画素対応               'added by 間々田 2004/04/29
			x = x * myMagnify;
			y = y * myMagnify;
			r = r * myMagnify;

			//string FileName = (myChanged ? modFileIO.OTEMPIMG : myTarget);
            string FileName = (myChanged ?  AppValue.OTEMPIMG : myTarget);
            
            if (!ImgProc.CtEnlarge(FileName, x - r, y - r, x + r, y + r))
			//if (!modImgProc.MyProcOCX.Enlarge(FileName, x - r, y - r, x + r, y + r))
			{
				//メッセージ表示：単純拡大に失敗しました。
				MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_WentWrong, StringTable.IDS_EnlargeSimple),
								Application.ProductName,
								MessageBoxButtons.OK,
								MessageBoxIcon.Exclamation);
				return result;
			}

			//一時的な画像ファイルを表示する
			DispTempImage();

			//ROIの消去
			myRoi.DeleteAllRoiData();

			//拡大率の計算   added by 山本 97－12－17
			//EnlargeRatio = EnlargeRatio * r / 512
            //2014/11/07hata キャストの修正
            //modImgProc.EnlargeRatio = modImgProc.EnlargeRatio * r * 2 / myPicWidth;
            modImgProc.EnlargeRatio = modImgProc.EnlargeRatio * r * 2 / (float)myPicWidth;
			//change by 間々田 2004/03/22

			//戻り値の設定
            result = true;

			return result;
		}

		//*******************************************************************************
		//機　　能： 「画像情報表示」選択処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 右クリック時に表示されるプルダウンメニュー処理
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public void mnuImageInfo_Click(object sender, EventArgs e)
		{
			//画像情報表示
            //戻す2015/01/24hata_オーナーフォームを指定しない
            //変更2014/12/22hata_dNet_オーナーフォームを指定する
            //frmImageInfo.Instance.Show();
            //frmImageInfo.Instance.Show(frmCTMenu.Instance);
           // frmImageInfo.Instance.Show();
            if (modLibrary.IsExistForm("frmImageInfo"))
            {
                frmImageInfo.Instance.WindowState = FormWindowState.Normal;
                frmImageInfo.Instance.Visible = true;
            }
            else
            {
                frmImageInfo.Instance.Show(frmCTMenu.Instance);
            }
        }

		//*******************************************************************************
		//機　　能： 「拡大」（もしくは「縮小」）選択処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 右クリック時に表示されるプルダウンメニュー処理
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public void mnuEnlarge_Click(object sender, EventArgs e)
		{
			//scanselの更新
			//GetScansel scansel
			//v16.10　4096対応のためコメントアウト by 長野 10/01/29
			//scansel.disp_size = 1 - scansel.disp_size

			//scanselの更新
			//modScansel.GetScansel(scansel);
            CTSettings.scansel.Load();

			//v16.10 表示していた画像が，拡大か縮小かを判断する方法を変更  by 長野 10/02/09

			if (this.hsbImage.Visible == true)
			{
				CTSettings.scansel.Data.disp_size = 0;
			}
			else
			{
				switch (myPicWidth)
				{
					case 2048:
						CTSettings.scansel.Data.disp_size = 1;
						break;

					case 4096:
						CTSettings.scansel.Data.disp_size = 2;
						break;
				}
			}

			//modScansel.PutScansel(CTSettings.scansel.Data);
            CTSettings.scansel.Write();

			//拡大・縮小ボタンのキャプションの設定
			UpdateMagnifyCaption();

			//スケール（および拡大倍率）の更新
			frmImageInfo.Instance.UpdateScale();

			//画像の更新
			DispPicture();
		}

		//*******************************************************************************
		//機　　能： 「ROI全て削除」選択処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 右クリック時に表示されるプルダウンメニュー処理
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public void mnuROIEditAllDelete_Click(object sender, EventArgs e)
		{
			//ROI全て削除処理
			myRoi.DeleteAllRoiData();
		}

		//*******************************************************************************
		//機　　能： 「ROIコピー」選択処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 右クリック時に表示されるプルダウンメニュー処理
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public void mnuROIEditCopy_Click(object sender, EventArgs e)
		{
			//ROIコピー処理
			myRoi.DataToClipBoard();
		}

		//*******************************************************************************
		//機　　能： 「ROIペースト」選択処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 右クリック時に表示されるプルダウンメニュー処理
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public void mnuROIEditDelete_Click(object sender, EventArgs e)
		{
			//ROI削除処理
			myRoi.CurrentRoiCut();
		}

		//*******************************************************************************
		//機　　能： 「ROI削除」選択処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 右クリック時に表示されるプルダウンメニュー処理
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public void mnuROIEditPaste_Click(object sender, EventArgs e)
		{
			//ROIペースト処理
			myRoi.RoiPaste();
		}

		//*******************************************************************************
		//機　　能： 「ROI座標入力」選択処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 右クリック時に表示されるプルダウンメニュー処理
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public void mnuRoiInput_Click(object sender, EventArgs e)
		{
			//水平線/垂直線ROI選択時
			if (tsbtnHLine.Checked == true || tsbtnVLine.Checked == true)
			{
				//水平線・垂直線座標入力フォーム表示
                if (!modLibrary.IsExistForm("HoriVertForm"))	//追加2015/01/30hata_if文追加
                {
                    HoriVertForm.Instance.Show(frmCTMenu.Instance);
                }
                else
                {
                    HoriVertForm.Instance.WindowState = FormWindowState.Normal;
                    HoriVertForm.Instance.Visible = true;
                }
            }
			else if (myRoi.GetRoiShape(myRoi.TargetRoi) == RoiData.RoiShape.ROI_LINE)
			{
				//線分座標入力フォーム表示
                if (!modLibrary.IsExistForm("frmLineInput"))	//追加2015/01/30hata_if文追加
                {
                    frmLineInput.Instance.Show(frmCTMenu.Instance);
                }
                else
                {
                    frmLineInput.Instance.WindowState = FormWindowState.Normal;
                    frmLineInput.Instance.Visible = true;
                }
            }
			else
			{
				//ROI座標入力処理
                frmInputRoiData.Instance.IsSquare = tsbtnSquare.Enabled;
                if (!modLibrary.IsExistForm("frmInputRoiData"))	//追加2015/01/30hata_if文追加
                {
                    frmInputRoiData.Instance.Show(frmCTMenu.Instance);
                }
                else
                {
                    frmInputRoiData.Instance.WindowState = FormWindowState.Normal;
                    frmInputRoiData.Instance.Visible = true;
                }
            }
		}

		//*******************************************************************************
		//機　　能： ROI変更時イベント処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void myRoi_Changed(string RoiInfo)
		{
			string workStr = null;
			workStr = RoiInfo;

			if (!string.IsNullOrEmpty(RoiInfo) && myRoi.RoiMaxNum > 1)
			{
				workStr = Convert.ToString(myRoi.TargetRoi) + " " + workStr;
			}

			//水平線/垂直線ROI選択時
            if (tsbtnHLine.Checked == true)
			{
				workStr = workStr.Replace(CTResources.LoadResString(StringTable.IDS_LineSegment), CTResources.LoadResString(StringTable.IDS_Horizon));
			}
			else if (tsbtnVLine.Checked == true)
			{
				workStr = workStr.Replace(CTResources.LoadResString(StringTable.IDS_LineSegment), CTResources.LoadResString(StringTable.IDS_VerticalLine));
			}

			//キャプション更新
			UpdateCaption(workStr);

			//右クリック時のメニューの調整
			mnuROIEditCopy.Enabled = myRoi.IsExistSelectedRoi & myRoi.ManualCut;	//選択されているROIが存在するか？
			mnuROIEditPaste.Enabled = myRoi.IsExistRoi() & myRoi.ManualCut;			//クリップボードにROIが存在するか？
			mnuROIEditDelete.Enabled = mnuROIEditCopy.Enabled & myRoi.ManualCut;	//選択されているROIが存在するか？
			mnuROIEditAllDelete.Enabled = (myRoi.NumOfRois > 0) & myRoi.ManualCut;	//ROIが存在するか？
			if (myRoi.TargetRoi == 0)
			{
				mnuRoiInput.Enabled = false;
			}
			else if (myRoi.GetRoiShape(myRoi.TargetRoi) == RoiData.RoiShape.ROI_LINE)
			{
				mnuRoiInput.Enabled = true;
			}
			else
			{
				mnuRoiInput.Enabled = myRoi.IsSizable(myRoi.TargetRoi);
			}

			//ツールバー上のボタン
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			With Toolbar1
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
            tsbtnCut.Enabled = mnuROIEditDelete.Enabled & Toolbar1.Enabled;			//ROIの切り取り
            tsbtnCopy.Enabled = mnuROIEditCopy.Enabled & Toolbar1.Enabled;			//ROIのコピー
            tsbtnPaste.Enabled = mnuROIEditPaste.Enabled & Toolbar1.Enabled;			//ROIの貼り付け
				tsbtnSave.Enabled = tsbtnOpen.Enabled & (myRoi.NumOfRois > 0 || modImgProc.PRDPoint > 0) & Toolbar1.Enabled;
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			End With
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//4096対応のためコメントアウト v16.10 by 長野 01/01/29
			//2048画素の場合でRoiが描画されていない時、拡大・縮小を使用可とする
			//mnuEnlarge.Enabled = (myPicWidth = 2048) And (Not IsExistRoi)

			//v16.10 2048,4096画素の場合でRoiが描画されていない時，拡大・縮小を使用可とする by 長野　10/01/29
			mnuEnlarge.Enabled = (myPicWidth == 2048 || myPicWidth == 4096) & (!IsExistRoi);

			//ROI変更時処理
			if (!string.IsNullOrEmpty(RoiInfo))
			{
				if (RoiChanged != null)
				{
                    //RoiChanged();    
                    RoiChanged(RoiInfo, EventArgs.Empty);
				}
			}
		}

		//*************************************************************************************************
		//機　　能： ツールバー上のボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*************************************************************************************************
		//private void Toolbar1_ButtonClick(object sender, ToolStripItemClickedEventArgs e)
		private void Toolbar1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			ToolStripButton Button = e.ClickedItem as ToolStripButton;

			if (Button == null)
			{
				return;
			}

			string FileName = null;
			bool IsOK = false;

			//サブ拡張子の設定
			string SubExtension = null;
			switch (myImageProc)
			{
				case ImageProcType.roiProcessing:
				case ImageProcType.RoiBoneDensity:
					SubExtension = "-ROI";
					break;
				case ImageProcType.RoiProfileDistance:
					SubExtension = "-PRD";
					break;
				case ImageProcType.roiZooming:
					SubExtension = "-zom";
					break;
			}

			//ツールバーの各処理実行
			switch (Button.Name)
			{
                case "tsbtnOpen":
                case "tsbtnLine":
                case "tsbtnHLine":
                case "tsbtnVLine":
                case "tsbtnPoint":
                case "tsbtnPaste":

					//画像が変更され、かつ保存されていない場合の処理
					if (myChanged)
					{
						//処理を続ける？
						if (!DoContinue())
						{
							//ボタンの状態を元に戻す
							if (Button.CheckOnClick == true)
							{
								LastRoiButton.Checked = true;
								Button.Checked = false;
							}
							return;
						}

						//結果フォームをアンロード
						frmResult.Instance.Close();
					}
					break;
			}

            //追加2014/07/24(検S1)hata
            //ROIグループのボタンの場合，前回クリックしたボタン戻す
            if (Button.CheckOnClick == true)
            {
                if (LastRoiButton != null) LastRoiButton.Checked = false;
            }
            
            //v19.11 ROI形状、ヒストグラム、骨塩定量解析のROI形状を記憶する by長野 2013/02/20
			switch (myImageProc)
			{
				case ImageProcType.roiProcessing:

					switch (Button.Name)
                    {
                        case "tsbtnCircle":
							DrawRoi.RoiCalRoiNo = 1;
							break;

                        case "tsbtnRectangle":
							DrawRoi.RoiCalRoiNo = 2;
							break;

                        case "tsbtnTrace":
							DrawRoi.RoiCalRoiNo = 6;
							break;
					}
                    break;

				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
//				Case RoiBoneDensity
//
//					Select Case Button.key
//
//						Case "Circle"
//
//							BoneDensityRoiNo = 1
//
//						Case "Rectangle"
//
//							BoneDensityRoiNo = 2
//
//						Case "Trace"
//
//							BoneDensityRoiNo = 6
//
//					End Select
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

				case ImageProcType.roiHistgram:

					switch (Button.Name)
					{
                        case "tsbtnCircle":
							DrawRoi.HistRoiNo = 1;
                            break;

                        case "tsbtnRectangle":
							DrawRoi.HistRoiNo = 2;
							break;

                        case "tsbtnTrace":
							DrawRoi.HistRoiNo = 6;
							break;
					}
                    break;

            }

			//ツールバーの処理実行
			string theComment = null;
			switch (Button.Name)
			{
				//矢印ROI制御
				case "tsbtnArrow":
					myRoi.ModeToPaint(RoiData.RoiShape.NO_ROI);
					break;

				//円形ROI制御
                case "tsbtnCircle":
					myRoi.ModeToPaint(RoiData.RoiShape.ROI_CIRC);
					break;

				//矩形ROI制御
                case "tsbtnRectangle":
					myRoi.ModeToPaint(RoiData.RoiShape.ROI_RECT);
					break;

				//トレースROI制御
                case "tsbtnTrace":
					myRoi.ModeToPaint(RoiData.RoiShape.ROI_TRACE);
					break;

				//正方形ROI制御
                case "tsbtnSquare":
					myRoi.ModeToPaint(RoiData.RoiShape.ROI_SQR);
					break;

				//線分ROI制御
                case "tsbtnLine":
					myRoi.ModeToPaint(RoiData.RoiShape.ROI_LINE);
					myRoi.ManualCut = true;							//削除可
					break;

				//線分ROI制御
                case "tsbtnHLine":
					//ROI種類を水平に設定
					myRoi.ModeToPaint(RoiData.RoiShape.ROI_LINE);
					myRoi.ManualCut = false;						//削除不可
					myRoi.SelectRoi(myRoi.AddLineShape(0, 511, myPicWidth / myMagnify - 1, 511, false));
					break;

				//線分ROI制御
                case "tsbtnVLine":
					//ROI種類を垂直線に設定
					myRoi.ModeToPaint(RoiData.RoiShape.ROI_LINE);
					myRoi.ManualCut = false;						//削除不可
					myRoi.SelectRoi(myRoi.AddLineShape(511, 0, 511, myPicHeight / myMagnify - 1, false));
					break;

				//点
                case "tsbtnPoint":
					myRoi.ManualCut = false;						//削除不可

					//ROIを１点入力に設定
					myRoi.ModeToPaint(RoiData.RoiShape.ROI_POINT);
					myRoi.SelectRoi(myRoi.AddPointShape(modImgProc.P1.x, modImgProc.P1.y));

					//ツールバーを使用不可にする
					ToolBarEnabled = false;

					//２値化画像閾値入力フォーム表示
					myCTSliceForm = CTSliceForm.Instance;
					myCTSliceForm.BinaryImageThreshold = true;

                    //追加2014/12/22hata_dNet
                    //CTSliceFormのイベント設定
                    myCTSliceForm.Clicked += new CTSliceForm.ClickedEventHandler(myCTSliceForm_Clicked);

					myCTSliceForm.Show(frmCTMenu.Instance);
					break;

                case "tsbtnCut":
					//ROIの切り取り
					myRoi.CurrentRoiCut();
					break;

                case "tsbtnCopy":
					//ROIのコピー
					myRoi.DataToClipBoard();
					break;

                case "tsbtnPaste":
					//ROIの貼り付け
					myRoi.RoiPaste();
					break;

                case "tsbtnOpen":
					//オープンダイアログ処理
                    //変更2015/01/22hata
                    //FileName = modFileIO.GetFileName(StringTable.IDS_Open, Convert.ToString(Button.Tag),".csv" ,SubExtension, myRoi.Table);
					FileName = modFileIO.GetFileName(StringTable.IDS_Open, Button.Text,".csv" ,SubExtension, myRoi.Table);
					if (string.IsNullOrEmpty(FileName))
					{
						return;
					}

					switch (myImageProc)
					{
						case ImageProcType.roiProcessing:
						case ImageProcType.RoiBoneDensity:
							if (myImageProc == ImageProcType.RoiBoneDensity)
							{
								myRoi.RoiMaxNum = 6;
							}
							IsOK = DrawRoi.LoadRoiTable(FileName);				//ROIテーブル読み込み
							break;
						case ImageProcType.RoiProfileDistance:
							IsOK = DrawRoi.LoadPRDTable(FileName);				//ﾌﾟﾛﾌｨｰﾙﾃﾞｨｽﾀﾝｽテーブル読み込み
							break;
						case ImageProcType.roiZooming:
							IsOK = DrawRoi.OpenZoomTable(FileName);				//ズーミングテーブル読み込み
							break;
						default:
							IsOK = false;
							break;
					}

					//後処理
					if (IsOK)
					{
						myRoi.Table = FileName;
						//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
//						Select Case myImageProc
//							Case RoiBoneDensity
//								Select Case myRoi.NumOfRois
//									Case 6
//										frmBoneDensity.Phase = BoneDensityPhase2nd
//									Case Else
//										frmBoneDensity.Phase = BoneDensityPhase1st
//									End Select
//						End Select
						//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
					}
					break;

                case "tsbtnSave":
					//保存ダイアログ処理
                    //変更2015/01/22hata
                    //FileName = modFileIO.GetFileName(StringTable.IDS_Save, Convert.ToString(Button.Description),".csv" ,SubExtension, myRoi.Table);
					//FileName = modFileIO.GetFileName(StringTable.IDS_Save, Convert.ToString(Button.AccessibleDescription),".csv" ,SubExtension, myRoi.Table);
					FileName = modFileIO.GetFileName(StringTable.IDS_Save, Button.Text,".csv" ,SubExtension, myRoi.Table);
					if (string.IsNullOrEmpty(FileName))
					{
						return;
					}

					switch (myImageProc)
					{
						case ImageProcType.roiProcessing:
						case ImageProcType.RoiBoneDensity:
							IsOK = DrawRoi.SaveRoiTable(FileName);			//ROIテーブル保存
							break;
						case ImageProcType.RoiProfileDistance:
							IsOK = DrawRoi.SavePRDTable(FileName);			//ﾌﾟﾛﾌｨｰﾙﾃﾞｨｽﾀﾝｽテーブル保存
							break;
						case ImageProcType.roiZooming:
							IsOK = DrawRoi.SaveZoomTable(FileName);			//ズーミングテーブル保存
							break;
						default:
							IsOK = false;
							break;
					}

					//後処理
					if (IsOK)
					{
						myRoi.Table = FileName;
					}
					break;

                case "tsbtnComment":
					//コメント入力ダイアログ表示

                    //変更2015/01/22hata
                    //theComment = frmComment.Dialog(StringTable.GetResString(9917, CTResources.LoadResString(StringTable.IDS_Comment)),
                    //                               StringTable.GetResString(StringTable.IDS_InputCommentOf, Convert.ToString(tsbtnGo.Description)), myRoi.Comment);
                    //theComment = frmComment.Dialog(StringTable.GetResString(9917, CTResources.LoadResString(StringTable.IDS_Comment)),
                    //                               StringTable.GetResString(StringTable.IDS_InputCommentOf, Convert.ToString(tsbtnGo.AccessibleDescription)), myRoi.Comment);
                    theComment = frmComment.Dialog(StringTable.GetResString(9917, CTResources.LoadResString(StringTable.IDS_Comment)),
                                                   StringTable.GetResString(StringTable.IDS_InputCommentOf, Convert.ToString(tsbtnGo.Text)), myRoi.Comment);
					                                                 
                    if (!string.IsNullOrEmpty(theComment))
					{
						myRoi.Comment = theComment;
					}
					break;

                case "tsbtnGo":
					GoRoi();
					break;

                case "tsbtnExit":
					ExitRoi();
					break;
			}

			//ROIグループのボタンの場合，今回クリックしたボタンを記憶
			if (Button.CheckOnClick == true)
			{
				LastRoiButton = Button;
			}
		}

		//*************************************************************************************************
		//機　　能： リソースから取得した文字列を各コントロールのキャプションにセット
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*************************************************************************************************
		private void SetCaption()
		{
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            //CTResources.LoadResString(this.Tag);
            int id;
            if (int.TryParse((string)this.Tag, out id))
            {
                CTResources.LoadResString(id);
            }

            //追加2014/07/14(検S1)hata
            //ポップアップメニュー
            mnuImageInfo.Text = CTResources.LoadResString(Convert.ToInt32(mnuImageInfo.Text));                  //画像情報          
            mnuROIEditCopy.Text = CTResources.LoadResString(Convert.ToInt32(mnuROIEditCopy.Text));              //コピー(&C)
            mnuROIEditPaste.Text = CTResources.LoadResString(Convert.ToInt32(mnuROIEditPaste.Text));            //貼り付け(&P)
            mnuROIEditDelete.Text = CTResources.LoadResString(Convert.ToInt32(mnuROIEditDelete.Text));          //ROI削除(&D)
            mnuROIEditAllDelete.Text = CTResources.LoadResString(Convert.ToInt32(mnuROIEditAllDelete.Text));    //すべてのROI削除(&A)

			//右クリック時のプルダウンメニュー項目
			mnuRoiInput.Text = StringTable.GetResString(StringTable.IDS_CoordinateInput, "ROI");	//ROI座標入力

			//ツールバー上のボタンのToolTipText
            tsbtnArrow.ToolTipText = StringTable.GetResString(StringTable.IDS_Select, "ROI");		//ROIの選択
	
        }

		//*******************************************************************************
		//機　　能： 処理続行するかどうかの確認ダイアログを表示
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private bool DoContinue()
		{
			//戻り値初期化
			bool result = false;

			//確認ダイアログ表示：前回の～グラフをクリアしますか？（クリアすると画像としてセーブする事はできません。）
            //変更2015/01/22hata
            ////switch (MessageBox.Show(StringTable.GetResString(9932, Convert.ToString(tsbtnGo.Description)),
            ////                        Application.ProductName,
            ////                        MessageBoxButtons.YesNo,
            ////                        MessageBoxIcon.Exclamation))
            //switch (MessageBox.Show(StringTable.GetResString(9932, Convert.ToString(tsbtnGo.AccessibleDescription)),
            //                        Application.ProductName,
            //                        MessageBoxButtons.YesNo,
            //                        MessageBoxIcon.Exclamation))
            switch (MessageBox.Show(StringTable.GetResString(9932, tsbtnGo.Text),
                                    Application.ProductName,
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Exclamation))
            {
				//はい選択
				case DialogResult.Yes:

					//画像を元に戻す
					DispOrgImage();
					modImgProc.PRDPoint = 0;
					result = true;
					break;

			}

			return result;
		}

		//*******************************************************************************
		//機　　能： 処理続行するかどうかの確認ダイアログを表示
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		//Private Sub GoRoi()
		public void GoRoi()
		{
			//測定するROIが登録されているか？
			if (myRoi.NumOfRois < 1)
			{
				//メッセージ表示：ROIが設定されていません。
				MessageBox.Show(CTResources.LoadResString(StringTable.IDS_NotFoundROI),
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//座標入力フォームをアンロード
			frmInputRoiData.Instance.Close();
			frmLineInput.Instance.Close();
            HoriVertForm.Instance.Close();

			//処理ごとに分岐
			switch (myImageProc)
			{
				case ImageProcType.roiEnlarge:
					DoEnlarge();					//拡大処理
					break;
				case ImageProcType.roiProcessing:
					DoRoi();						//ROI処理
					break;
				case ImageProcType.roiZooming:
					DoZooming();					//ズーミング
					break;
				case ImageProcType.roiDistance:
					DoDistance();					//寸法測定
					break;
				case ImageProcType.RoiAngle:
					DoDistance();					//角度測定
					break;
				case ImageProcType.roiHistgram:
					DoHistgram();					//ヒストグラム
					break;
				case ImageProcType.roiProfile:
					DoProfile();					//プロフィール
					break;
				case ImageProcType.RoiProfileDistance:
					modImgProc.PRDPoint = 2;		//２点指定
					DoProfile();					//プロフィール＆ディスタンス
					break;
				case ImageProcType.RoiAutoPos:
					DoAutoPos();
					break;
			}
		}

		//*******************************************************************************
		//機　　能： 処理続行するかどうかの確認ダイアログを表示
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		//Private Sub ExitRoi()
		public void ExitRoi()		//15.10変更 byやまおか 2009/11/30
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			'エラー時の扱い
//			On Error Resume Next
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//画像表示フォームが変更されている場合
			if (myChanged)
			{
				//サブ拡張子の設定
				string SubExtension = string.Empty;

				//サブ拡張子の設定
				switch (myImageProc)
				{
					case ImageProcType.roiEnlarge:
						SubExtension = CTResources.LoadResString(10703);			//拡大画像
						break;
					case ImageProcType.roiHistgram:
						SubExtension = CTResources.LoadResString(10704);			//ﾋｽﾄｸﾞﾗﾑ
						break;
					case ImageProcType.roiProfile:
						SubExtension = CTResources.LoadResString(10705);			//ﾌﾟﾛﾌｨｰﾙ
						break;
					case ImageProcType.RoiProfileDistance:
						SubExtension = CTResources.LoadResString(10706);			//PD
						break;
				}

				try
				{
					//結果保存ダイアログ
					if (!frmImageSave.Instance.Dialog(Target, SubExtension))
					{
						return;
					}
				}
				catch
				{
				}
			}

			//角度測定時     'v14.1追加(↓↓↓ここから↓↓↓) byやまおか 2007/07/31
			else if (myImageProc == ImageProcType.RoiAngle)
			{                
                Form theForm = null;
                //if (modLibrary.IsExistForm(frmScanCondition.Instance))
                if (modLibrary.IsExistForm("frmScanCondition"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
                {
                    theForm = frmScanCondition.Instance;
                    //Rev20.00 frmScanConditionの場合はshowは不要。by長野 2014/12/04
                    //frmScanCondition.Instance.Show(frmCTMenu.Instance);	//v16.01/v17.00追加 byやまおか 2010/02/24
                    //追加2015/02/05hata
                    theForm.WindowState = FormWindowState.Normal;
                    theForm.Visible = true;
                }
                //else if (modLibrary.IsExistForm(frmRetryCondition.Instance))	//リトライ条件画面
                //else if (modLibrary.IsExistForm("frmRetryCondition"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
                else if (frmRetryCondition.Instance.IsDisposed == false)  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
                {
                    if(modLibrary.IsExistForm("frmRetryCondition") || frmRetryCondition.Instance.IsExRoi == true)
                    {
                        theForm = frmRetryCondition.Instance;
                        frmRetryCondition.Instance.IsExRoi = false; 
                        frmRetryCondition.Instance.Show(frmCTMenu.Instance);		//v16.01/v17.00追加 byやまおか 2010/02/24
                    }
                }

                if (theForm != null)
                {
					//結果フォームが存在している？
					//if (modLibrary.IsExistForm(frmResult.Instance))
                    if (modLibrary.IsExistForm("frmResult"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
                    {
                        //問い合わせ：測定角度を画像回転角度に設定しますか？
						//Select Case MsgBox(LoadResString(12840), vbYesNoCancel + vbExclamation)
						switch (MessageBox.Show(CTResources.LoadResString(12840), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))	//v16.01/v17.00変更 byやまおか 2010/02/24
						{
 							case  DialogResult.Yes:
                                if (theForm.Name == frmScanCondition.Instance.Name)
                                {
                                    frmScanCondition.Instance.cwneImageRotateAngle.Value = Convert.ToDecimal(GetRotAngle());

                                    //削除2014/12/22hata_dNet
                                    //Rev20.00 追加 by長野 2014/12/04
                                    //frmScanCondition.Instance.TopMost = true;

                                    frmScanCondition.Instance.WindowState = FormWindowState.Normal;
                                }
                                else if (theForm.Name == frmRetryCondition.Instance.Name)	//リトライ条件画面
                                {
                                    frmRetryCondition.Instance.cwneImageRotateAngle.Value = Convert.ToDecimal(GetRotAngle());
                                }
                                //theForm.cwneImageRotateAngle.Value = GetRotAngle();
								break;
                            
                            case DialogResult.No:
								break;
							//Case vbCancel:  Exit Sub   'v16.01/v17.00削除 byやまおか 2010/02/24
						}
					}

					//「画像から測定」ボタンを使用可能にする
					//theForm.cmdMeasureFrImg.Enabled = True
					theForm.Enabled = true;

					//このフォームにフォーカスを設定
					theForm.Activate();

					//参照破棄
					theForm = null;
				}
				//v14.1追加(↑↑↑ここまで↑↑↑) byやまおか 2007/07/31
			}

			try
			{
				//ROI制御終了
				this.ImageProc = ImageProcType.RoiNone;
			}
			catch
			{
			}

			//マウスポインタを元に戻す
			this.Cursor = Cursors.Default;
		}

		//*******************************************************************************
		//機　　能： 角度測定時の角度計算
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private float GetRotAngle()
		{
			float RotAngle = 0;

			//測定角度に表示画像の回転角度を足す
            //2014/11/07hata キャストの修正
            //RotAngle = (float)modImgProc.DistanceInfo[0].AngleX - (int)(((float)modImgProc.DistanceInfo[0].AngleX + 45) / 90) * 90;
            RotAngle = (float)modImgProc.DistanceInfo[0].AngleX - Convert.ToInt32(Math.Floor(((float)modImgProc.DistanceInfo[0].AngleX + 45) / 90F)) * 90;
			RotAngle = RotAngle + frmImageInfo.Instance.ReconStartAngle;

			//-180＜θ≦180に補正
			if (RotAngle > 180)
			{
				RotAngle = RotAngle - 360;
			}
			else if (RotAngle <= -180)
			{
				RotAngle = RotAngle + 360;
			}

			return RotAngle;
		}

		//*******************************************************************************
		//機　　能： ROI処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void DoRoi()
		{
			int i = 0;
			string buf = null;	//ROI結果用文字列
            
			buf = "";
            
            //変更2015/01/22hata
            //string RoiInfoStr = null;
            string RoiInfoStr = "";

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			'エラー時の扱い
//			On Error Resume Next
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//マウスポインタを砂時計に変更する。
			this.Cursor = Cursors.WaitCursor;

			//ROI処理
			for (i = 1; i <= myRoi.NumOfRois; i++)
			{
				//ROI情報をコモンにセットします
				//Call SetROIToCommon(i, RoiInfoStr)
				try
				{
                    DrawRoi.SetROIToCommon(i,ref RoiInfoStr);					//v11.5変更 by 間々田 2006/04/24
				}
				catch
				{
				}

				bool result = false;
				
				try
				{
					//ROI処理実行
                    result = ImgProc.CtRoi(ref modImgProc.area[i], ref  modImgProc.Ave[i], ref  modImgProc.Sd[i]);
				}
				catch
				{
				}

				if (!result)
				{
					//マウスポインタを元に戻す。
					this.Cursor = Cursors.Default;
					//メッセージ表示：ROI処理に失敗しました。
					MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_WentWrong, StringTable.IDS_ROIProcessing),
									Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}

				modImgProc.area[i] = modImgProc.area[i] / (1000.0 * 1000.0);		//ミクロンからｍｍ2に変換

				buf = buf + "\r\n";
				buf = buf + "*ROI - " + Convert.ToString(i) + "*" + "\r\n";
				buf = buf + CTResources.LoadResString(12416) + modImgProc.Ave[i].ToString("0.00") + "\r\n";			//LoadResString(12416):平均値   :
				buf = buf + CTResources.LoadResString(12415) + modImgProc.Sd[i].ToString("0.00") + "\r\n";			//LoadResString(12415):標準偏差 :
				buf = buf + CTResources.LoadResString(12417) + modImgProc.area[i].ToString("0.0000") + "\r\n";		//LoadResString(12417):面積(mm) :
			}

			//結果フォームを表示
			frmResult.Instance.SetText(buf);

			try
			{
				//ROI-Tableを保存する
				DrawRoi.SaveRoiTable(frmImageInfo.Instance.Target + "-ROI.csv");
			}
			catch
			{
			}

			try
			{
				//ROI-Tableを保存するﾊﾟｽを付帯情報に登録
                frmImageInfo.Instance.DoRoi();
			}
			catch
			{
			}

			//マウスポインタを元に戻す。
			this.Cursor = Cursors.Default;
		}

		//*******************************************************************************
		//機　　能： ズーミング処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void DoZooming()
		{
			//コーンビーム画像か？
            if (frmImageInfo.Instance.IsConeBeam)
			{
				//コーンビームが使用不可のとき、コーンビーム画像のズーミングを禁止
				if (!(CTSettings.scaninh.Data.data_mode[2] == 0))
				{
					//メッセージ表示：
					//   コーンビームスキャン画像です。
					//   この画像のズーミング処理は実行できません。
                    //変更2014/11/18hata_MessageBox確認
                    //MessageBox.Show(CTResources.LoadResString(9396), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    MessageBox.Show(CTResources.LoadResString(9396), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
				}

				//コーンビーム画像の場合、ROIは1個だけしか描けなくする            'V4.0 append by 鈴山 2001/02/28
				else if (myRoi.NumOfRois > 1)
				{
					//メッセージ表示：コーンビーム画像でズーミングを行う場合は、ROIを1個にしてください。
					MessageBox.Show(CTResources.LoadResString(StringTable.IDS_msgZoomingError2),
									Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			//実行ファイル
            string ExeFileName = (frmImageInfo.Instance.IsConeBeam ? AppValue.CONERECON : AppValue.RECONMST);

			//実行ファイルの有無をチェック
			if (!File.Exists(ExeFileName))
			{
				//メッセージ表示：～が見つかりません。ズーミングを中止します。
				MessageBox.Show(StringTable.GetResString(StringTable.IDS_NotFound, ExeFileName) + "\r\n\r\n" + 
								StringTable.BuildResStr(StringTable.IDS_Interrupted, StringTable.IDS_Zooming),
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//付帯情報の読み込み：グローバル画像情報構造体にセット
            ImageInfo.ReadImageInfo(ref CTSettings.gImageinfo.Data, frmImageInfo.Instance.Target);



			//生データディレクトリ名をコモンから読み、書き込む
			//ディレクトリ名はコーンビームモードに関係なくコモンから読み込む

			string BaseName = null;			//生ﾃﾞｰﾀﾌｧｲﾙのﾌｧｲﾙ名
			//生データファイル名をコモンから読み、書き込む added by 山本 97－10－23
            //if (gImageInfo.bhc == 1)
            if (CTSettings.gImageinfo.Data.bhc == 1)
			{
				//ｺｰﾝﾋﾞｰﾑﾌﾗｸﾞ=1の場合、付帯情報から読み込む
                //BaseName = CTSettings.gImageinfo.Data.bhc_name.Replace('\0', "").Trim();
                BaseName = CTSettings.gImageinfo.Data.bhc_name.GetString().Replace("\0", "").Trim();
			}
			else
			{
				//ｺｰﾝﾋﾞｰﾑﾌﾗｸﾞ=0の場合
				BaseName = Path.GetFileNameWithoutExtension(frmImageInfo.Instance.Target);
			}

			//v10.2追加 by 間々田 2005/07/07
			if (BaseName.Length < 5)
			{
				//メッセージ表示：生データが一式ありません。
				MessageBox.Show(CTResources.LoadResString(StringTable.IDS_msgZoomingError4),
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// 正規表現でパターンマッチング（VBでいう LIKE "*-Z###"）
			Regex reg = new Regex("^.*-Z\\d{3}$");


			//ズーミング画像の場合はファイル名から-Z00*をとる added by 山本 97-11-18 ----------------------------------------
			if (reg.IsMatch(BaseName.ToUpper()))
			{
				BaseName = BaseName.Substring(0, BaseName.Length - "-Z###".Length);
			}
			//---------------------------------------------------------------------------------------------------------------

			//ズーミングに使用する生データファイル名（拡張子なし）
			string myRawName = Path.Combine(Path.GetDirectoryName(frmImageInfo.Instance.Target), BaseName);

			//生データをチェック                             'v10.2追加 by 間々田 2005/07/07
			if (!File.Exists(myRawName + (CTSettings.gImageinfo.Data.bhc == 1? ".cob" : ".raw")))
			{
				//メッセージ表示：生データがありません。
				MessageBox.Show(CTResources.LoadResString(StringTable.IDS_msgZoomingError5),
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//描画してあるROIを編集不可にするための措置                              'v11.5追加 by 間々田 2006/07/06
			this.Enabled = false;

			//テンポラリなズーミングテーブルを保存する
			DrawRoi.SaveZoomTable(AppValue.ZOOMTMPCSV);

			//再構成リトライ画面を表示
			//frmRetryCondition.ShowDialog_Renamed(myRawName, true);
            frmRetryCondition.Instance.Dialog(myRawName, true);
            frmRetryCondition.Instance.Dispose();
        }

		//*******************************************************************************
		//機　　能： 寸法測定処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v1.00  99/XX/XX   ????????      新規作成
		//           v10.2  2005/06/29 (SI3)間々田   全角文字ではなく半角文字で結果を表示するように変更
		//                                           後で結果を保存できるように結果を配列に格納することにした
		//           v14.1  2007/08/28 やまおか      角度測定に対応
		//*******************************************************************************
		private void DoDistance()
		{
			int i = 0;
			string buf = null;

			buf = "";

            modImgProc.DistanceInfo = new modImgProc.DistanceInfoType[myRoi.NumOfRois];

			for (i = 1; i <= myRoi.NumOfRois; i++)
			{
                //Rev20.00 2014/12/04 別オブジェクトの扱いになり、modImgProc.DistanceInfoには何も入らないため変更 by長野
				//var _with = modImgProc.DistanceInfo[i-1];

				//線分の座標を取得
                myRoi.GetLineShape(i, ref modImgProc.DistanceInfo[i - 1].x1, ref modImgProc.DistanceInfo[i - 1].y1, ref modImgProc.DistanceInfo[i - 1].x2, ref modImgProc.DistanceInfo[i - 1].y2);

				//線分２点間の距離・角度を算出
                if (GetDistance((float)modImgProc.DistanceInfo[i - 1].x1, (float)modImgProc.DistanceInfo[i - 1].y1, (float)modImgProc.DistanceInfo[i - 1].x2, (float)modImgProc.DistanceInfo[i - 1].y2, ref modImgProc.DistanceInfo[i - 1].Dist, ref modImgProc.DistanceInfo[i - 1].AngleX, ref modImgProc.DistanceInfo[i - 1].AngleY))
				{
					buf = buf + "\r\n";
                        
					//測定結果（距離）表示部
                    buf = buf + StringTable.GetResString(12469, Convert.ToString(i)) + "=" + modImgProc.DistanceInfo[i - 1].Dist.ToString("0.#000") + "mm" + "\r\n";

					//測定結果（角度Ｘ）表示部
                    buf = buf + CTResources.LoadResString(StringTable.IDS_Xtheta) + "=" + modImgProc.DistanceInfo[i - 1].AngleX.ToString("0.#0") + "\r\n";

					//測定結果（角度Ｙ）表示部
                    buf = buf + CTResources.LoadResString(StringTable.IDS_Ytheta) + "=" + modImgProc.DistanceInfo[i - 1].AngleY.ToString("0.#0") + "\r\n";
				}

            }

			//測定結果フォームに結果を表示
			frmResult.Instance.SetText(buf);
		}

		//*******************************************************************************
		//機　　能： 二点の距離・角度を測定する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//           V14.1  07/07/27   やまおか      角度範囲を変更(0<θ<180)⇒(-180<θ<180)
		//                                           Yθの符号を逆にした(CW:-,CCW:+)⇒(CW:+,CCW:-)
		//*******************************************************************************
		private bool GetDistance(float XS, float YS, float XE, float YE, ref double Distance, ref double AngleX, ref double AngleY)
		{
			double theMatrix = 0;
			//v11.0追加 by 間々田 2005/09/29

			//戻り値・引数初期化
			bool result = false;
			AngleX = 0;
			AngleY = 0;

			theMatrix = myPicWidth;		//v11.0追加 by 間々田 2005/09/29

			//画素レベルでの距離
			Distance = Math.Sqrt((XS - XE) * (XS - XE) + (YS - YE) * (YS - YE));

			if (Distance == 0)
			{
				return result;
			}

			//'v14.1変更（↓↓↓ここから↓↓↓） byやまおか 2007/07/26
			//'      'Ｘ角度計算
			//'       AngleX = ArcSin(CDbl(Abs(YS - YE)) / Distance) * 180# / Pai
			//'
			//'       'Ｙ角度計算
			//'       AngleY = AngleX + IIf(AngleX < 0#, 90#, -90#)
			//'
			//Ｘ角度計算(距離とY座標からXθを求める)
            AngleX = modLibrary.ArcSin((YS - YE) / Distance) * 180.0 / Math.PI;
			//-180＜Xθ≦180 に補正
			if (XE - XS < 0)
			{
				if (YE - YS == 0)
				{
					AngleX = 180.0;
				}
				else if (YE - YS < 0)
				{
					AngleX = 180.0 - AngleX;
				}
				else
				{
					AngleX = -180.0 - AngleX;
				}
			}

			//Ｙ角度計算(XθからYθを求める)(!!!v14.1からはYθの符号が逆になるようにした!!!)
			//-180＜Xθ≦180 に補正
			if (AngleX >= -90 && AngleX <= 180)
			{
				AngleY = -(AngleX - 90);
			}
			else if (AngleX > -180 && AngleX < -90)
			{
				AngleY = -(AngleX + 270);
			}
			//'v14.1追加（↑↑↑ここまで↑↑↑） byやまおか 2007/07/26

			//mmでの距離
			//Distance = Distance * frmScanImage.Magnify * frmInformation.SizePerPixel
//			Distance = Distance * frmScanImage.Magnify * Val(frmInformation.lblContext(29).tag) * 10 / theMatrix  'v11.0変更 by 間々田 2005/09/29
			Distance = Distance * myMagnify * frmImageInfo.Instance.ScaleValue / theMatrix;			//v15.0変更 by 間々田 2007/08/29

			//戻り値セット
			result = true;

			return result;
		}

		//*******************************************************************************
		//機　　能： ヒストグラム処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private bool DoHistgram()
		{
			//戻り値初期化
			bool result = false;
            //変更2015/01/22hata
            //string RoiInfoStr =null;
            string RoiInfoStr = "";

			//ROI情報をコモンにセットします
            if (!DrawRoi.SetROIToCommon(1, ref RoiInfoStr))
			{
				return result;
			}

			//if (!MyProcOCX.Histgram(modImgProc.CT_Bias, modImgProc.CT_Int))
            if (!ImgProc.CtHist(modImgProc.CT_Bias, modImgProc.CT_Int))
            {
				//メッセージ表示：ヒストグラムに失敗しました。
				MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_WentWrong, StringTable.IDS_Histogram),
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return result;
			}

			//処理画像を表示
			DispTempImage();

			//ＣＴ値入力（IntervalとBias）フォームが表示されていない場合
			if (myCTInputForm == null)
			{
				//ツールバーを使用不可にする
				ToolBarEnabled = false;

				//CT値入力フォーム表示
				myCTInputForm = CTInputForm.Instance;

                //CTinputFormのイベント設定
                myCTInputForm.Clicked += new CTInputForm.ClickedEventHandler(myCTInputForm_Clicked);

                myCTInputForm.Show(frmCTMenu.Instance);

				//目盛り間隔は強制的に45の倍数とする
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				With myCTInputForm.ntbInterval
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
				var with = myCTInputForm.ntbInterval;
				with.DiscreteInterval = 45;
				with.SetMinMax(Convert.ToDecimal(with.DiscreteInterval), Convert.ToDecimal(with.DiscreteInterval) * 40);
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				End With
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				//このフォームを使用不可にする
				this.Enabled = false;
			}

			//戻り値セット
			result = true;

			return result;
		}

		//*******************************************************************************
		//機　　能： 測定２点取得処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private bool GetRoi2Points()
		{
			int x1 = 0;
			int y1 = 0;
			int x2 = 0;
			int y2 = 0;

			//戻り値初期化
			bool result = false;

			//ROI（点）座標の取得
			myRoi.GetPointShape(0, ref modImgProc.P1.x,ref  modImgProc.P1.y);

			int N = 0;
			N = myMagnify;			//1:通常  2:縮小 added by 間々田 2004/03/25

			//マウスポインタを砂時計に変更
			//Screen.MousePointer = vbHourglass
			this.Cursor = Cursors.WaitCursor;		//v15.10変更 byやまおか 2009/12/01

			//測定２点取得処理
			//if (!ImgProc.GetRoiPosition(modImgProc.P1.x * N, modImgProc.P1.y * N, modImgProc.CT_Low1Point, modImgProc.CT_High1Point, x1, y1, x2, y2))
            if (!ImgProc.GetRoiPoint(modImgProc.P1.x * N, modImgProc.P1.y * N, modImgProc.CT_Low1Point, modImgProc.CT_High1Point,ref x1,ref y1,ref x2,ref y2))
            {
				//マウスポインタを元に戻す
				//Screen.MousePointer = vbDefault
				this.Cursor = Cursors.Default;				//v15.10変更 byやまおか 2009/12/01
				//メッセージ表示：測定２点取得処理に失敗しました。
				MessageBox.Show(CTResources.LoadResString(9973), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return result;
			}

			//線ROI制御モードにし，算出した２点をROI情報に登録

			//ROI１点指定を解除
			ClickToolBarButton(tsbtnLine);
//			Toolbar1.Buttons("Point").value = tbrUnpressed
//			myRoi.ModeToPaint NO_ROI
//
//			'ROI削除
//			myRoi.DeleteAllRoiData

			//算出した２点をROI情報に登録
            //2014/11/07hata キャストの修正
            //myRoi.AddLineShape(x1 / N, y1 / N, x2 / N, y2 / N);
            myRoi.AddLineShape(Convert.ToInt32(x1 / (float)N), Convert.ToInt32(y1 / (float)N), Convert.ToInt32(x2 / (float)N), Convert.ToInt32(y2 / (float)N));

			//マウスポインタを元に戻す
			//Screen.MousePointer = vbDefault
			this.Cursor = Cursors.Default;			//v15.10変更 byやまおか 2009/12/01

			//戻り値セット
			result = true;

			return result;
		}

		//*******************************************************************************
		//機　　能： プロフィールディスタンス・実行
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void DoPRD()
		{
			int cnt = 0;
			int[] TempLow = new int[201];
			int[] TempUp = new int[201];
			int[] TempUnit = new int[201];
			double[] TempXS = new double[201];
			double[] TempYS = new double[201];
			double[] TempXE = new double[201];
			double[] TempYE = new double[201];
			double[] TempDist = new double[201];
			double[] TempAngleX = new double[201];
			double[] TempAngleY = new double[201];
			string buf = null;
			int DistNum = 0;			//v10.2追加 by 間々田 2005/07/14

			//縮小表示を考慮する added by 間々田 2004/05/06
			//Dim m As Double                                    'v10.2削除 by 間々田 2005/06/17
			//m = frmScanImage.Magnify                           'v10.2削除 by 間々田 2005/06/17

			//プロフィールﾃﾞｨｽﾀﾝｽの測定結果ﾌｧｲﾙの読み込み
			//ProcOCX1.LoadPRDTemp TempXS(0), TempYS(0), TempXE(0), TempYE(0), TempLow(0), TempUp(0), _
			//'                     TempUnit(0), TempLow1(0), TempUp1(0), TempDist(0), TempAngleX(0), TempAngleY(0)

			//v10.2変更 by 間々田 2005/07/14
			//DistNum = MyProcOCX.LoadPRDTemp(TempXS, TempYS, TempXE, TempYE, TempLow, TempUp, TempUnit, TempDist, TempAngleX, TempAngleY);
            DistNum = ImgProc.PRDTempLoad(ref TempXS[0], ref TempYS[0], ref TempXE[0], ref TempYE[0], 
                                          ref TempLow[0], ref TempUp[0], ref TempUnit[0], ref TempDist[0], ref TempAngleX[0], ref TempAngleY[0]);

			buf = "";

			//処理結果の表示処理
			for (cnt = 0; cnt < DistNum; cnt++)
			{
				if (cnt == 10)
				{
					break;
				}

				//ミクロンをｍｍにする
				//TempDist(cnt) = TempDist(cnt) / 1000#
				//TempDist(cnt) = m * TempDist(cnt) / 1000#   '縮小表示を考慮する added by 間々田 2004/05/06
				//TempDist(cnt) = TempDist(cnt) / 1000#        'v10.2変更 by 間々田 2005/06/17

				buf = buf + "\r\n";

				//buf = buf & GetResString(12469, chr$(Asc("A") + cnt)) & vbCrLf 'リソース12469:線分
				buf = buf + StringTable.GetResString(12469, (cnt + 1).ToString()) + "\r\n";		//リソース12469:線分 'A,B,C→1,2,3に変更 by 間々田 2005/06/28

				//buf = buf & "        = " & CStr(TempDist(cnt)) & "mm" & vbCrLf
				buf = buf + "        = " + (TempDist[cnt] / 1000.0).ToString("0.#000") + "mm" + "\r\n";		//v10.2変更 by 間々田 2005/06/17

				//LoadResString(IDS_MaxThreshold):上限閾値
				buf = buf + CTResources.LoadResString(StringTable.IDS_MaxThreshold) + " = " + TempUp[cnt].ToString() + "\r\n";

				//LoadResString(IDS_MinThreshold):下限閾値
				buf = buf + CTResources.LoadResString(StringTable.IDS_MinThreshold) + " = " + TempLow[cnt].ToString() + "\r\n";
			}

			//処理結果フォームを表示
			frmResult.Instance.SetText(buf);

			//ROI-Tableを保存するﾊﾟｽを付帯情報に登録
			frmImageInfo.Instance.DoPRD();

			//プロフィール＆ディスタンステーブルファイル書き込み
			DrawRoi.SavePRDTable(frmImageInfo.Instance.Target + "-PRD.csv");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Button"></param>
		public void ClickToolBarButton(ToolStripButton button)
		{
			if(!button.CheckOnClick) button.Checked = true;
            button.PerformClick();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Button"></param>
        public void ClickToolBarButton(string ToolStripButtonKyeName)
        {
            int a = Toolbar1.Items.IndexOfKey(ToolStripButtonKyeName);
            ToolStripItem[] button1 = Toolbar1.Items.Find(ToolStripButtonKyeName, true);

            if(button1.Count() > 0)
            {
                ToolStripButton button;
                button = (ToolStripButton)button1[0];
                if (!button.CheckOnClick) button.Checked = true;
                button.PerformClick();

               

            }
        }

        //ツールバーのボタンがCheckされているか確認する。
        public bool GetToolBarChecked(string ToolStripButtonKyeName)
        {
            bool ret = false;
            int a = Toolbar1.Items.IndexOfKey(ToolStripButtonKyeName);
            ToolStripItem[] button1 = Toolbar1.Items.Find(ToolStripButtonKyeName, true);

            if (button1.Count() > 0)
            {
                ToolStripButton button;
                button = (ToolStripButton)button1[0];
                if (button.Checked)
                {
                    ret = true;
                }
            }
            return ret;
        }



		//*******************************************************************************
		//機　　能： 自動スキャン位置移動処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void DoAutoPos()
		{
			int xc = 0;
			int yc = 0;
			int r = 0;
			//Dim RoiCircle(2)    As Long
			//Dim PosFx           As Single
			//Dim PosFy           As Single
			//Dim PosFCD          As Single
			//Dim PosTableY       As Single
			//int err_sts = 0;
			float K = 0;

			bool IsOK = false;

			//変更2015/1/17hata_非表示のときにちらつくため
            //frmAutoScanPos.Instance.Hide();
            modCT30K.FormHide(frmAutoScanPos.Instance);

            K = (float)frmImageInfo.Instance.Matrix / ((float)myPicWidth / (float)myMagnify);
           
			//ツールバー上のボタンを触れないようにする
			ToolBarEnabled = false;

			//ROI座標取得
			myRoi.GetCircleShape(1, ref xc, ref yc, ref r);

			//'ROI設定
			//RoiCircle(0) = xc * K
			//RoiCircle(1) = yc * K
			//RoiCircle(2) = r * K

			//テーブル移動計算関数呼び出し
			//err_sts = autotbl_set(RoiCircle(0), PosFx, PosFy, PosFCD, PosTableY)
			//err_sts = autotbl_set(RoiCircle(0), PosFy, PosFx, PosFCD, PosTableY)    'v15.0変更 by 間々田 2009/06/16

			//変更 by 間々田 2009/07/09
            //2014/11/07hata キャストの修正
            //IsOK = frmMechaMove.Instance.MechaMoveForAutoScanPos((int)(xc * K), (int)(yc * K), (int)(r * K));
            IsOK = frmMechaMove.Instance.MechaMoveForAutoScanPos(Convert.ToInt32(xc * K), Convert.ToInt32(yc * K), Convert.ToInt32(r * K));

//			'自動スキャン位置移動（微調テーブルのみ移動）
//			ElseIf frmMechaMove.MechaMove(PosFx, PosFy, , , , , , True) Then
//
//			'テーブルのＸＹ座標を求める                                           '追加 by 間々田 2009/07/09
//			err_sts = auto_tbl_set(RoiCircle(0), _
//'                               PosFy, PosFx, _
//'                               frmMechaControl.ntbFTablePosY.value, _
//'                               frmMechaControl.ntbFTablePosX.value, _
//'                               fcd1, table_h_xray1, _
//'                               fcd2, table_h_xray2)
//				'エラー？
//				If err_sts <> 0 Then
//
//					ErrMessage err_sts
//
//				Else
//
//					'IsOK = frmMechaMove.MechaMove(PosFx, PosFy, PosFCD - scancondpar.fcd_offset(GetFcdOffsetIndex()), , PosTableY, , True)
//					IsOK = frmMechaMove.MechaMove(, , fcd1 - scancondpar.fcd_offset(GetFcdOffsetIndex()), , table_h_xray1, , , True)
//
//				End If
//
//			End If

            //Rev26.40 add by chouno 2019/02/17
            frmMechaMove.Instance.Dispose();

			//ツールバー上のボタンを触れるようにする
			ToolBarEnabled = true;

			if (IsOK)
			{
				//自動スキャン位置フォームをアンロード
				frmAutoScanPos.Instance.Close();
			}
			else
			{
                if (!modLibrary.IsExistForm("frmAutoScanPos"))	//追加2015/01/30hata_if文追加
                {
                    frmAutoScanPos.Instance.Show(frmCTMenu.Instance);
                }
                else
                {
                    frmAutoScanPos.Instance.WindowState = FormWindowState.Normal;
                    frmAutoScanPos.Instance.Visible = true;
                }
            }
		}

        //追加2014/05hata
        private void frmScanImage_Activated(object sender, EventArgs e)
        {
            ////描画を強制する
            //if (this.Visible && this.Enabled) this.Refresh();
        }

        /// <summary>
        /// 指定したファイルをロックせずに、Imageを作成する。
        /// </summary>
        /// <param name="filename">作成元のファイルのパス</param>
        /// <returns>作成したSystem.Drawing.Image。</returns>
        public static Image CreateImage(string filename)
        {
            FileStream fs = null;
            Image img = null;
            try
            {
                fs = new System.IO.FileStream(
                     filename,
                     System.IO.FileMode.Open,
                     System.IO.FileAccess.Read);

                //img = Image.FromStream(fs,true,true);
                img = Image.FromStream(fs);

            }
            catch
            { 
            }
            fs.Close();
            return img;
        }

        //Toolbarが反応しないための処理　//2015/01/21(検S1)hata
        private void Toolbar1_MouseEnter(object sender, EventArgs e)
        {
           if (Toolbar1.Visible && Toolbar1.Enabled)
                Toolbar1.Focus();
        }
 
	}
}
