using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Collections.Generic;

//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{
	//************************************************************************** */
	//* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
	//* 客先　　　　： ?????? 殿                                                   */
	//* プログラム名： frmCTMenu.frm                                               */
	//* 処理概要　　： ＣＴメインメニュー                                          */
	//* 注意事項　　： なし                                                        */
	//* -------------------------------------------------------------------------- */
	//* 適用計算機　： DOS/V PC                                                    */
	//* ＯＳ　　　　： Windows 2000  (SP4)                                         */
	//* コンパイラ　： VB 6.0                                                      */
	//* -------------------------------------------------------------------------- */
	//* VERSION     DATE        BY                  CHANGE/COMMENT                 */
	//*                                                                            */
	//* V1.00       99/XX/XX    (TOSFEC) ????????                                  */
	//* V2.0        00/02/08    (TOSFEC) 鈴山　修   V1.00を改造                    */
	//* V3.0        00/08/01    (TOSFEC) 鈴山　修   ｺｰﾝﾋﾞｰﾑCT対応                  */
	//* V8.0        03/12/15    (SI4)    松井　修   分散処理対応                   */
	//* v10.02      05/05/24    (SI4)    間々田     テーブルズーミング削除         */
	//* v19.00      12/02/20    H.Nagai             BHC対応
	//*                                                                            */
	//* -------------------------------------------------------------------------- */
	//* ご注意：                                                                   */
	//* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	//*                                                                            */
	//* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
	//* ************************************************************************** */
    internal partial class frmCTMenu : Form
    {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		Option Explicit

//		'大文字、小文字の区別をしない
//		Option Compare Text
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//********************************************************************************
		//  共通データ宣言
		//********************************************************************************

		//分散処理時のｾﾏﾌｫの宣言 05-01-31 by IWASAWA
		private int mainmap;		//共有メモリマップハンドル
		private int mainsem;		//セマフォハンドル
		private int mainmem;		//共有メモリ先頭アドレス
		private int mainsemflg;		//セマフォ取得したら1たてる

		//Ｘ線オンオフステータスプロパティ用定数
		public enum XrayOnOffStatusConstants
		{
			XrayOnAvail = 1,
			XrayOnNotAvail,
			XrayOffAvail,
			XrayOffNotAvail
		}

		//Ｘ線オンオフステータスプロパティ用変数
		XrayOnOffStatusConstants myXrayOnOffStatus;

		//扉電磁ロックプロパティ用定数
		public enum DoorStatusConstants
		{
			DoorOpened = 1,
			DoorClosed,
			DoorLocked
		}

		//扉電磁ロックプロパティ用変数
		private DoorStatusConstants myDoorStatus;

		//空き容量
        //private object myDiskFreeSpace;
		private long myDiskFreeSpace;

		//スキャン可能回数
		private int myScanAvairableNum;

		//１回のスキャンで消費する容量
        //private object myScanImageSize;
        private double myScanImageSize;

		//I.I.視野
        private object myIIField;
        //private int myIIField;

		//ユーザ停止要求領域（共有メモリ）のハンドル →キャプチャの停止処理に使用する    v17.50追加 by 間々田 2011/01/05
		//private int hMapUserStop;

		private bool tmrStatus_Tick_BUSYNOW = false;

		private UpdReceiver socCT30K = null;

		private static frmCTMenu _Instance = null;

        private TransImageControl transImageCtrl;

        //コントロール更新用のデリゲート
        delegate void ControlUpdateDelegate();

        private bool toolBar1ItemEventLock = false;


		/// <summary>
		/// 
		/// </summary>
		private frmCTMenu()
		{
			InitializeComponent();

			Toolbar1.ImageList = ImageList1;
			StatusBar1.ImageList = ImageList2;

			_Instance = this;


            //Scan終了イベント
            transImageCtrl = CTSettings.transImageControl;
            transImageCtrl.CaptureScanEnd += new TransImageControl.ScanEndHandler(transImageCtrl_CaptureScanEnd);

		}

		/// <summary>
		/// 
		/// </summary>
		public static frmCTMenu Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmCTMenu();
				}

				return _Instance;
			}
		}

        //Scan終了イベント処理
        void transImageCtrl_CaptureScanEnd(int Error)
        {
            //ControlUpdate();
        }

        //画像更新
        private void ControlUpdate()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ControlUpdateDelegate(ControlUpdate));
                return;
            }

            try
            {
                if (CTSettings.scaninh.Data.xray_remote == 0)
                {
                    modXrayControl.XrayOff();
                }

            }
            catch
            {
            }

        }


		//*******************************************************************************
		//機　　能： IIFieldプロパティ（設定のみ）
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		public object iifield
		{
			set
			{
				//変更時のみ設定する
				if (myIIField != null)
				{
					if (value == myIIField)
					{
						return;
					}
				}

				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
				//I.I.位置不定の時は更新しない v16.01 追加 by 山影 10-02-17
//				If IsUnknownMode Then Exit Property
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

				//内部変数に記憶
				myIIField = value;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				With Toolbar1.Buttons("I.I.Field")
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

					//tsbtnIIField.Image = Convert.ToInt32(modCT30K.GetIIStr(myIIField));
                    //tsbtnIIField.Image = ImageList1.Images["II" + modCT30K.GetIIStr((int)myIIField) + ".ico"];
                    tsbtnIIField.Image = ImageList1.Images[modCT30K.GetIIStr((int)myIIField)];					

                tsbtnIIField.Enabled = true;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				End With
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				//Ｘ線条件設定処理
				//if (modLibrary.IsExistForm(frmXrayControl.Instance))
                if (modLibrary.IsExistForm("frmXrayControl"))  //Openしていないとloadしてしまうため　2014/06/05(検S1)hata
                {
                    int XrayCondIndex = modLibrary.GetCmdButton(frmXrayControl.Instance.cmdCondition);
                    modXrayControl.TempSetCurrent = -1;
                    modXrayControl.TempSetVolt = -1;
                    frmXrayControl.Instance.SetXrayCondition(XrayCondIndex, (int)myIIField, frmMechaControl.Instance.FIDWithOffset);
                    frmXrayControl.Instance.UpdateXrayCondition();
                    frmXrayControl.Instance.SetFilter(XrayCondIndex, (int)myIIField);					//追加 2009/08/19
				}
			}
		}

		//*******************************************************************************
		//機　　能： RunReadyプロパティ（設定のみ）
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		public bool RunReady
		{
			set
			{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				With StatusBar1.Panels("RunReady")
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

					tslblRunReady.Text = CTResources.LoadResString((value ? 12759 : 12758));
					//運転準備完了/運転準備未完
					tslblRunReady.Image = ImageList2.Images[value ? "Green" : "DarkGreen"];

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				End With
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
			}
		}

		//*******************************************************************************
		//機　　能： DoorInterlockプロパティ（設定のみ）
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		public bool DoorInterlock
		{
			set
			{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				With StatusBar1.Panels("DoorInterlock")
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

					tslblDoorInterlock.Enabled = value;
					tslblDoorInterlock.Image = ImageList2.Images[value ? "Green" : "DarkGreen"];

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				End With
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
			}
		}

		//*******************************************************************************
		//機　　能： Emergencyプロパティ（設定のみ）
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		public bool emergency
		{
			set
			{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				With StatusBar1.Panels("Emergency")
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				tslblEmergency.Enabled = value;
				tslblEmergency.Image = ImageList2.Images[value ? "Red" : "DarkRed"];

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				End With
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
			}
		}

		//*******************************************************************************
		//機　　能： Ｘ線オンオフステータスプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		public XrayOnOffStatusConstants XrayOnOffStatus
		{
			get { return myXrayOnOffStatus; }
			set
			{
				//変更時のみ設定する
				if (value == myXrayOnOffStatus)
				{
					return;
				}

				//内部変数に記憶
				myXrayOnOffStatus = value;

				//ツールバー上のボタンに反映
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				Toolbar1.Buttons("Xray").Image = Choose(XrayOnOffStatus, "XrayOnAvail", "XrayOnNotAvail", "XrayOffAvail", "XrayOffNotAvail")
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				string[] names = new string[] {"XrayOnAvail", "XrayOnNotAvail", "XrayOffAvail", "XrayOffNotAvail"};

				tsbtnXray.Image = ImageList1.Images[names[(int)myXrayOnOffStatus - 1]];

				//Ｘ線オン時（ウォームアップ中も含む）は、必ずＸ線オフできるようにＸ線ボタンは使用可にしておく   'v17.51追加 by 間々田 2011/03/25
				if (myXrayOnOffStatus == XrayOnOffStatusConstants.XrayOnAvail || myXrayOnOffStatus == XrayOnOffStatusConstants.XrayOnNotAvail)
				{
					tsbtnXray.Enabled = true;
				}
			}
		}

		//*******************************************************************************
		//機　　能： 扉電磁ロックプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		public DoorStatusConstants DoorStatus
		{
			get { return myDoorStatus; }
			set
			{
				//変更時のみ設定する
				if (value == myDoorStatus)
				{
					return;
				}

				//内部変数に記憶
				myDoorStatus = value;

				//ツールバー上のボタンに反映
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				Toolbar1.Buttons("DoorLock").Image = Choose(myDoorStatus, "CannotLock", "Unlocked", "Locked")
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				string[] names = new string[] {"CannotLock", "Unlocked", "Locked"};

				tsbtnDoorLock.Image = ImageList1.Images[names[(int)myDoorStatus - 1]];
			}
		}

		//*******************************************************************************
		//機　　能： DiskFreeSpace プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 単位はKバイト
		//
		//履　　歴： v15.0  2008/10/03  (SS1)間々田  新規作成
		//*******************************************************************************
		private long DiskFreeSpace
		{
			set
			{
				//変更時のみ設定する
                if (value == (long)myDiskFreeSpace)
				{
					return;
				}

				myDiskFreeSpace = value;

				//ディスクの空き容量を(MB)で表示
                //2014/11/06hata キャストの修正
                tslblDiskFreeSpace.Text = CTResources.LoadResString(12303) + " " + ((long)myDiskFreeSpace < 0 ? "???" : Convert.ToInt64(myDiskFreeSpace / 1024D).ToString()) + " MB";	//空きﾃﾞｨｽｸ容量

				//スキャン可能回数を更新
				UpdateScanAvairableNum();
			}
		}

		//*******************************************************************************
		//機　　能： ScanImageSize プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 単位はKバイト
		//
		//履　　歴： v15.0  2008/10/03  (SS1)間々田  新規作成
		//*******************************************************************************
		public double ScanImageSize
		{
			get { return myScanImageSize; }
			set
			{
				//変更時のみ設定する
				if (value == myScanImageSize)
				{
					return;
				}

				myScanImageSize = value;

				//スキャン可能回数を更新
				UpdateScanAvairableNum();
			}
		}

		//*******************************************************************************
		//機　　能： ScanAvairableNum プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.0  2008/10/03  (SS1)間々田  新規作成
		//*******************************************************************************
		public int ScanAvairableNum
		{
			get { return myScanAvairableNum; }
			set
			{
				//変更時のみ設定する
				if (value == myScanAvairableNum)
				{
					return;
				}

				myScanAvairableNum = value;

				//ｽｷｬﾝ可能回数 ～ 回
				tslblScanAvairableNum.Text = CTResources.LoadResString(12301) + " " + Convert.ToString(myScanAvairableNum) + " " + CTResources.LoadResString(10817);

				//スキャン可能回数が０になった場合　空き容量不足の状態に変化したらメッセージを表示
				if (!(myScanAvairableNum > 0))
				{
					//空き容量が不明の場合
                    if ((long)myDiskFreeSpace < 0)
					{
						//メッセージの表示：画像保存先のドライブ（ドライブ %1）が接続されていません。
						MessageBox.Show(StringTable.GetResString(12923, AppValue.MyDrive), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					//空き容量不足の場合
					else
					{
                        modCT30K.ErrMessage(1301, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
		}

		//*******************************************************************************
		//機　　能： ソケット通信により受信したデータに基づいて処理を行う
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.5 06/06/21 (WEB)間々田      新規作成
		//*******************************************************************************
		private void socCT30K_DataArrival(object sender, ReceivedEventArgs e)
		{
			if (InvokeRequired)
			{
                //Invoke(new ReceiveCollBack(socCT30K_DataArrival), new object[] { sender, e });
                BeginInvoke(new ReceiveCollBack(socCT30K_DataArrival), new object[] { sender, e });
				return;
			}

			string buf = null;
			string[] Cell = null;
			float kv = 0;
			float fVal = 0;
			int iVal = 0;
            int sts = 0;
                        
			//ソケット通信によりデータを受信
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			socCT30K.GetData(buf);
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            //buf = Encoding.ASCII.GetString(e.ReceiveData);
            buf = Encoding.Default.GetString(e.ReceiveData);

			modCT30K.logOut(this.Name + ":socCT30K_DataArrival : " + buf);

			if (!string.IsNullOrEmpty(buf))
			{
				//セルに格納（区切り文字はスペース）
				//Cell() = Split(buf, " ")
				Cell = buf.Split('|');

				switch (Cell[0])
				{
					//透視画像保存
					case "TransImageSave":
						//GetTransImage TransImage(0), UBound(TransImage) + 1
						ScanCorrect.GetTransImage(ref modScanCorrectNew.TransImage[0]);
						//v17.50変更 by 間々田 2010/12/22
						//frmTransImage.Update False
						//frmTransImage.Update False, vbSrcCopy '変更 by 間々田 2009/08/21
                        switch (CTSettings.detectorParam.DetType)		//v17.00追加(ここから) byやまおか 2010/02/04
						{
							case DetectorConstants.DetTypeII:
							case DetectorConstants.DetTypeHama:
								//frmTransImage.MyUpdate(false, vbSrcCopy);
                                frmTransImage.Instance.TransImageCtrl.Update(false, 0);
								break;
							case DetectorConstants.DetTypePke:
								//frmTransImage.MyUpdate(true, vbSrcCopy);
								frmTransImage.Instance.TransImageCtrl.Update(true, 0);
								break;
						}								//v17.00追加(ここまで) byやまおか 2010/02/04
						break;

//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
//					'スキャン開始時
//					Case "ScanUpdate"
//						If UBound(Cell) < 1 Then Exit Sub
//						frmScanStatus.Update Val(Cell(1))
//
//					'マルチスキャンカウントアップ
//					Case "MultiScanCountUp"
//						If UBound(Cell) < 2 Then Exit Sub
//						frmScanStatus.MultiScanCountUp Val(Cell(1)), Val(Cell(2))
//
//					'マルチスライスカウントアップ
//					Case "MultiSliceCountUp"
//						If UBound(Cell) < 2 Then Exit Sub
//						frmScanStatus.MultiSliceCountUp Val(Cell(1)), Val(Cell(2))
//
//					'待ちステータス表示
//					Case "WaitProcessing"
//						If UBound(Cell) < 1 Then Exit Sub
//						frmScanStatus.WaitProcessing Cell(1)
//
//					'キャプチャカウントアップ
//					Case "CaptureCountUp"
//						If UBound(Cell) < 2 Then Exit Sub
//						frmScanStatus.CaptureCountUp Val(Cell(1)), Val(Cell(2))
//
//					'コーンビーム生データ保存枚数カウントアップ
//					Case "ImageSaveCountUp"
//						If UBound(Cell) < 2 Then Exit Sub
//						frmScanStatus.ImageSaveCountUp Val(Cell(1)), Val(Cell(2))
//
//					'オートズーム枚数カウントアップ
//					Case "AutoZoomCountUp"
//						If UBound(Cell) < 2 Then Exit Sub
//						frmScanStatus.AutoZoomCountUp Val(Cell(1)), Val(Cell(2))
//
//					'再構成枚数カウントアップ
//					Case "ReconCountUp"
//						If UBound(Cell) < 2 Then Exit Sub
//						frmScanStatus.ReconCountUp Val(Cell(1)), Val(Cell(2))
//
//					'スキャン実行時の処理割合（％）
//					Case "ScanPercent"
//						If UBound(Cell) < 1 Then Exit Sub
//						frmScanStatus.UpdateScanPercent Val(Cell(1))
//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

					//ＣＴ画像保存
					case "ImageSaved":
						if (Cell.GetUpperBound(0) < 1)
						{
							return;
						}

						//ＣＴ画像コレクションに追加
						if (modCT30K.ImportedCTImages == null)
						{
                            modCT30K.ImportedCTImages = new List<string>();
						}

						modCT30K.ImportedCTImages.Add(Cell[1]);

						//画像階調最適化
						//If scansel.contrast_fitting = 1 Then
                        if (CTSettings.scansel.Data.contrast_fitting == 1 && CTSettings.scansel.Data.operation_mode == (int)ScanSel.OperationModeConstants.OP_SCAN)	//v15.0変更 by 間々田 2009/08/24
						{
							modCT30K.ContrastFitting(Cell[1]);
						}
						break;

					//画像表示
					case "ImageReload":

						if (DrawRoi.roi != null)
						{
							DrawRoi.roi.DeleteAllRoiData();			//ROIを削除
						}

//						'dispinfの読み込み
//						GetDispinf dispinf
//
//						'画像表示
//						frmScanImage.Target = AddExtension(FSO.BuildPath(dispinf.d_exam, dispinf.d_id), ".img")

						//if (modCT30K.ImportedCTImages.Count > 0)
                        //Rev23.30 by長野 2016/03/02
                        if (modCT30K.ImportedCTImages != null && modCT30K.ImportedCTImages.Count > 0)
                        {
                            Debug.Print("file = " + modCT30K.ImportedCTImages[modCT30K.ImportedCTImages.Count - 1]);

							frmScanImage.Instance.Target = modCT30K.ImportedCTImages[modCT30K.ImportedCTImages.Count-1];
						}
						break;

					//Ｘ線オン
					case "XrayOn":

						if (Cell.GetUpperBound(0) >= 2)
						{
							if (!float.TryParse(Cell[1], out kv))
							{
								return;
							}

							if (!float.TryParse(Cell[2], out fVal))
							{
								return;
							}

							//指定された初期管電圧を設定
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//							kv = Val(Cell(1))
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

							if (kv <= CTSettings.scansel.Data.scan_kv)
							{
								//SetKVMA KV
								modXrayControl.SetVolt(kv);			//v15.0変更 by 間々田 2009/04/07
								modCT30K.PauseForDoEvents(1);		//管電圧が設定されるのを待つ
							}
						}

						modXrayControl.XrayOn();

						if (Cell.GetUpperBound(0) >= 2)
						{
							//設定管電圧になるまでループ
                            while (kv + 1 <= CTSettings.scansel.Data.scan_kv)
							{
								modCT30K.PauseForDoEvents(fVal);
								kv = kv + 1.0f;
								//SetKVMA KV
								modXrayControl.SetVolt(kv);		//v15.0変更 by 間々田 2009/04/07
							}
						}
						break;

					//Ｘ線オフ
					case "XrayOff":
						//XrayOff
                        if (CTSettings.scaninh.Data.xray_remote == 0)
						{
							modXrayControl.XrayOff();			//v15.10変更 byやまおか 2009/11/10
						}
						break;

					//再構成後の処理
					case "ReconmstEnd":
					case "ConereconEnd":

                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                        //frmScanStatus.Close();
                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

						int.TryParse(Cell[1], out iVal);

						if (Cell.GetUpperBound(0) >= 1)
						{
							modCT30K.EndProcess(iVal);
						}
						else
						{
							modCT30K.EndProcess();
						}
						break;

                    //スキャン後の処理
                    case "ScanoEnd":          //Rev21.00 追加 by長野 2015/02/23
                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                        //frmScanStatus.Close();
                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                        int.TryParse(Cell[1], out iVal);

                        if (Cell.GetUpperBound(0) >= 1)
                        {
                            frmStatus.Instance.EndProcessScano(iVal);
                        }
                        else
                        {
                            frmStatus.Instance.EndProcessScano();
                        }
                        break;

					//スキャン後の処理
					case "ScanEnd":
					case "ConebeamEnd":
                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                        //frmScanStatus.Close();
                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

						int.TryParse(Cell[1], out iVal);

						if (Cell.GetUpperBound(0) >= 1)
						{
							frmStatus.Instance.EndProcessScan(iVal);
						}
						else
						{
                            frmStatus.Instance.EndProcessScan();
						}
						break;

					//ファントムオン
					case "PhmOn":
						modMechaControl.MecaPhmOn();
						break;

					//ファントムオフ
					case "PhmOff":
						modMechaControl.MecaPhmOff();
						break;
                    //Rev20.00 追加 by長野 2014/12/15 
                    case "ScanStart":

                        if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone)
                        {
                            if (int.TryParse(Cell[1], out iVal))
                            {
                                modScanCondition.ScanJs = iVal;
                            }

                            if (int.TryParse(Cell[2], out iVal))
                            {
                                modScanCondition.ScanJe = iVal;
                            }
                        }

                        modScanCondition.ScanStartFlg = true;
                        break;

                    //追加2014/08/08(検S1)hata---ここから--->
					//マルチスキャンのScan再スタート
                    case "NextMultiScan":
                        //Rev20.00 X線がONできないので応急処置
                        //Rev20.00 マルチの中でX線ONできていないための応急処置、本番では、もっと手前の処理でマルチのループをする by長野 2014/08/27
                        if (CTSettings.scaninh.Data.xray_remote == 0)
                        {
                            //Ｘ線ＯＮ処理(アベイラブル待ち）
                            sts = modXrayControl.TryXrayOn();
                        }
                        //Rev20.00 共有メモリの破棄を行う
                        CTSettings.transImageControl.DestroyCTScanObj();

                        
                        //Rev20.00 条件追加 by長野 2015/01/24
                        if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone)
                        {
                            if (int.TryParse(Cell[1], out iVal))
                            {
                                modScanCondition.ScanJs = iVal;
                            }

                            if (int.TryParse(Cell[2], out iVal))
                            {
                                modScanCondition.ScanJe = iVal;
                            }
                        }

                        //Rev20.00 追加 by長野 2015/01/24
                        if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScan)
                        {
                            if (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeHalf)
                            {
                                if (int.TryParse(Cell[1], out iVal))
                                {
                                    modScanCondition.HalfNoAutoCenteringFlg = iVal;
                                }
                            }

                        }

                        //Rev20.00 追加 by長野 2015/02/28
                        GC.Collect();

                        //Rev20.00 追加 by長野 2014/09/11
                        //次回スキャンのメモリ確保と撮像条件をセット（次回スキャンの条件はスキャンソフト側でコモンを書き換え済み）
                        modScanCondition.ScanPreparation();

                        transImageCtrl.ResstartScan();
                        break;

                    //スライスプランのScan再スタート
					case "NextSlicePlan":

                        //Rev20.00 X線がONできないので応急処置
                        //Rev20.00 マルチの中でX線ONできていないための応急処置、本番では、もっと手前の処理でマルチのループをする by長野 2014/08/27
                        if (CTSettings.scaninh.Data.xray_remote == 0)
                        {
                            //Ｘ線ＯＮ処理(アベイラブル待ち）
                            sts = modXrayControl.TryXrayOn();
                        }
                        //Rev20.00 共有メモリの破棄を行う
                        CTSettings.transImageControl.DestroyCTScanObj();


                        //Rev20.00 条件追加 by長野 2015/01/29
                        if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone)
                        {
                            if (int.TryParse(Cell[1], out iVal))
                            {
                                modScanCondition.ScanJs = iVal;
                            }

                            if (int.TryParse(Cell[2], out iVal))
                            {
                                modScanCondition.ScanJe = iVal;
                            }
                        }

                        //Rev20.00 追加 by長野 2015/02/28
                        GC.Collect();

                        //Rev20.00 追加 by長野 2014/09/11
                        //次回スキャンのメモリ確保と撮像条件をセット（次回スキャンの条件はスキャンソフト側でコモンを書き換え済み）
                        modScanCondition.ScanPreparation();

                        transImageCtrl.ResstartScan();
                        break;

                    //微調Xの動作
                    case "FTblXIndex":
					    if (float.TryParse(Cell[1], out fVal))
						{
                            //実際の動作指令
                            modMechaControl.MecaXStgIndex(fVal);
                        }
						break;

					///微調Yの動作
                    case "FTblYIndex":
					    if (float.TryParse(Cell[1], out fVal))
						{
                            //実際の動作指令
                            modMechaControl.MecaYStgIndex(fVal);
                        }
						break;

                    //昇降の動作
                    case "TblUdIndexMltScan":
                        if (float.TryParse(Cell[1], out fVal))
                        {
                            //実際の動作指令
                            modMechaControl.MechaUdIndex_MultiScan(fVal);
                        }
                        break;
                    //追加2014/08/08(検S1)hata---ここまで---<
                    //昇降の動作 //Rev20.00 マルチスキャンは相対移動・スライスプランは絶対移動のため追加 by長野 2014/09/11
                    case "TblUdIndexSlP":
                        if (float.TryParse(Cell[1], out fVal))
                        {
                            //実際の動作指令
                            modMechaControl.MechaUdIndex(fVal);
                        }
                        break;

                    //追加2014/10/07hata_v19.51反映
                    //検出器シフト(基準位置)     'v18.00追加 byやまおか 2011/07/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    case "DetShiftOrg":
                        //ShiftDet (DetShift_origin)
                        //modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_origin, modDetShift.UNSET_GAIN);    //ゲインをセットしない   'v18.00変更 byやまおか 2011/07/04
                        //Rev23.20 修正 by長野 2016/01/13
                        modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_origin, modDetShift.SET_GAIN);    //ゲインをセットしない   'v18.00変更 byやまおか 2011/07/04
                 
                        break;
                    //追加2014/10/07hata_v19.51反映
                    //検出器シフト(シフト位置)   'v18.00追加 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    case "DetShift":
                        //ShiftDet (DetShift_forward)
                        //modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_forward, modDetShift.UNSET_GAIN);   //ゲインをセットしない   'v18.00変更 byやまおか 2011/07/04
                        //Rev23.20 修正 by長野 2016/01/13
                        modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_forward, modDetShift.SET_GAIN);   //ゲインをセットしない   'v18.00変更 byやまおか 2011/07/04
                        break;

                    //検出器シフト(基準位置) スキャン中の呼び出し限定 //Rev23.20追加 by長野 2015/11/19
                    case "DetShiftOrgScan":

                        //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                        //if (CTSettings.scaninh.Data.lr_sft == 0)
                        if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
                        {
                            //modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_backward, modDetShift.UNSET_GAIN);    //ゲインをセットしない   'v18.00変更 byやまおか 2011/07/04
                            //Rev23.20 修正 by長野 2016/01/13
                            modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_backward, modDetShift.SET_GAIN);    //ゲインをセットしない   'v18.00変更 byやまおか 2011/07/04
                        }
                        else
                        {
                            //modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_origin, modDetShift.UNSET_GAIN);    //ゲインをセットしない   'v18.00変更 byやまおか 2011/07/04
                            //Rev23.20 修正 by長野 2016/01/13
                            modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_origin, modDetShift.SET_GAIN);    //ゲインをセットしない   'v18.00変更 byやまおか 2011/07/04
                        }
                        break;
                    //検出器シフト(シフト位置) スキャン中の呼び出し限定 //Rev23.20追加 by長野 2015/11/19
                    case "DetShiftScan":
                        //modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_forward, modDetShift.UNSET_GAIN);   //ゲインをセットしない   'v18.00変更 byやまおか 2011/07/04
                        //Rev23.20 修正 by長野 2016/01/13
                        modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_forward, modDetShift.SET_GAIN);   //ゲインをセットしない   'v18.00変更 byやまおか 2011/07/04
                        break;

                    case "SaveFluoroImage":
                        //Rev20.00 シングルスキャンの画像保存 by長野 2015/01/15
                        File.Move(@"C:\\CT\Temp\\SaveFluoroTemp.img",Cell[1]);
                        break;
                    case "TableRotOrigin":
                        //Rev20.00 テーブル原点復帰追加 by長野 2015/01/24
                        if (CTSettings.scansel.Data.table_rotation == 1)
                        {
                            MechaControl.RotateSlowStop(modDeclare.hDevID1, null);
                        }
                        MechaControl.RotateIndex(modDeclare.hDevID1, 0, 0, null);
                        break;
                    case "Mechastatus_check"://Rev20.00 メカステータスチェック追加 by長野 2015/03/05
                        modMechaControl.Mechastatus_check(modDeclare.hDevID1);
                        break;

                    case "UpdateDispMechaData"://Rev26.00 機構部位置更新 by chouno 2017/06/28
                        CTSettings.mecainf.Load();
                        frmMechaControl.Instance.UpdateMecha();
                        Application.DoEvents();
                        break;
 				}
			}
		}

		//********************************************************************************
		//機    能  ：  終了確認を行う
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//引    数  ：  iMode            [I/ ] Boolean   何か実行中に強制終了(True:する,False:しない)
		//戻 り 値  ：                   [ /O] Boolean   結果  True:終了  False:キャンセル
		//補    足  ：  終了できない場合、その理由を表示する。
		//
		//履    歴  ：  V1.00  97/XX/XX  (SI3)鈴山       新規作成
		//              V1.00  97/10/20  (SI3)鈴山       終了できない場合、その理由を表示するように変更
		//              V4.0   01/02/09  (SI1)鈴山       引数(iMode)を追加
		//              V4.0   01/02/19  (SI1)鈴山       何か実行中に強制終了するときのメッセージを変更
		//              V9.0   04/02/06  (SI4)間々田     引数(iMode)を廃止
		//********************************************************************************
		private bool QuitCT30K()
		{
			string Msg = null;		//メッセージ

			//動作中フラグを調べる
			if ((modCTBusy.CTBusy & modCTBusy.CTImagePrint) != 0)
			{
				Msg = StringTable.GetResString(StringTable.IDS_Processing, CTResources.LoadResString(10502));					//画像印刷処理
			}
			else if ((modCTBusy.CTBusy & modCTBusy.CTScanCondition) != 0)
			{
				Msg = StringTable.GetResString(StringTable.IDS_Processing, CTResources.LoadResString(10503));					//スキャン条件設定処理
			}
			else if ((modCTBusy.CTBusy & modCTBusy.CTSlicePlan) != 0)
			{
                Msg = StringTable.BuildResStr(StringTable.IDS_Processing, StringTable.IDS_SlicePlan);						//スライスプラン処理
			}
			else if ((modCTBusy.CTBusy & modCTBusy.CTScanCorrect) != 0)
			{
				Msg = StringTable.BuildResStr(StringTable.IDS_Processing, StringTable.IDS_ScanCorrect);						//スキャン校正処理
			}
			else if ((modCTBusy.CTBusy & modCTBusy.CTTableAutoMove) != 0)
			{
				Msg = StringTable.GetResString(StringTable.IDS_Processing, CTResources.LoadResString(10522));					//自動テーブル移動処理
			}
			else if ((modCTBusy.CTBusy & modCTBusy.CTZooming) != 0)
			{
                Msg = StringTable.BuildResStr(StringTable.IDS_Processing, StringTable.IDS_Zooming);							//ズーミング処理
			}
			else if ((modCTBusy.CTBusy & modCTBusy.CTReconstruct) != 0)
			{
                Msg = StringTable.BuildResStr(StringTable.IDS_Processing, StringTable.IDS_Reconst);							//再構成リトライ処理
			}
			else if ((modCTBusy.CTBusy & modCTBusy.CTImageProcessing) != 0)
			{
                //if (!string.IsNullOrEmpty(frmScanImage.Instance.tsbtnGo.Description))
                //{
                //    Msg = StringTable.GetResString(StringTable.IDS_Processing, frmScanImage.Instance.tsbtnGo.Description);	//
                //}
                //変更2015/01/22hata
                //if (!string.IsNullOrEmpty(frmScanImage.Instance.tsbtnGo.AccessibleDescription))
                //{
                //    Msg = StringTable.GetResString(StringTable.IDS_Processing, frmScanImage.Instance.tsbtnGo.AccessibleDescription);	//
                //}
                if (!string.IsNullOrEmpty(frmScanImage.Instance.tsbtnGo.Text))
                {
                    Msg = StringTable.GetResString(StringTable.IDS_Processing, frmScanImage.Instance.tsbtnGo.Text);	//
                }
                else
				{
                    //Msg = ResourceManager.GetString("str" + Convert.ToString(StringTable.IDS_ImageProcessing));	//画像処理
                    Msg = CTResources.LoadResString(StringTable.IDS_ImageProcessing);	//画像処理
				}
			}
			else if ((modCTBusy.CTBusy & modCTBusy.CTFormatTransfer) != 0)
			{
				Msg = StringTable.BuildResStr(StringTable.IDS_Processing, StringTable.IDS_FormatConvert);		//画像フォーマット変換処理
			}
			else if ((modCTBusy.CTBusy & modCTBusy.CTMaintenance) != 0)
			{
				Msg = StringTable.GetResString(StringTable.IDS_Processing, CTResources.LoadResString(10520));					//メンテナンス処理
			}
			//ElseIf XrayControl.Up_Warmup = 1 Then
			else if (modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpNow)
			{
				Msg = StringTable.BuildResStr(StringTable.IDS_Processing, StringTable.IDS_Warmup);				//ウォームアップ処理
			}
			else if ((modCTBusy.CTBusy & modCTBusy.CTScanStart) != 0)
			{
                //Msg = Resources.ResourceManager.GetString("str" + Convert.ToString(StringTable.IDS_Scan));	//スキャン
                Msg = CTResources.LoadResString(StringTable.IDS_Scan);	//スキャン
			}
			else
			{
				return true;
			}

			//確認ダイアログ表示：
			//   ～を実行中です。
			//   本当にCT30Kを終了しますか？
			return MessageBox.Show(StringTable.GetResString(9325, Msg), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes;
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
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//           v11.2 05/10/19 (SI3)間々田      scaninh構造体変数を使用
		//           v13.0 2007/03/19 (WEB)間々田    ダブルオブリーク対応
		//*******************************************************************************
		//Private Sub InitControls()
		//v17.20 検出器切替用にpublic化
		public void InitControls()
		{
			//メニューバーの表示／非表示を設定する
            var with = CTSettings.scaninh.Data;

            tsbtnDoorLock.Visible = (with.door_lock == 0) & (with.seqcomm == 0);		//扉電磁ロック
			//Toolbar1.Buttons("I.I.Field").Visible = (.iifield = 0) And (Not Use_FlatPanel)  'I.I.視野：検出器がFPDの場合、非表示
            tsbtnIIField.Visible = (with.mechacontrol == 0) & (with.iifield == 0) & (!CTSettings.detectorParam.Use_FlatPanel);	//I.I.視野：検出器がFPDの場合、非表示
			//Toolbar1.Buttons("DoubleOblique").Visible = (.double_oblique = 0)               'ダブルオブリーク対応 2007/03/19追加 by 間々田
			tsbtnDoubleOblique.Visible = (with.mechacontrol != 0) & (with.double_oblique == 0);		//DOボタン(メカ制御がないときはここに表示)   'v15.0追加 byやまおか 2009/07/29
			tsbtnMainte.Visible = (with.mechacontrol == 0);			//メンテナンス
			tsbtnXray.Visible = (with.xray_remote == 0);				//X線ボタン          'v15.0追加 byやまおか 2009/07/29
			tsbtnLiveImage.Visible = (with.mechacontrol == 0);			//ライブ画像ボタン   'v15.0追加 byやまおか 2009/07/29

            tsbtnXrayAllWup.Visible = (with.multi_tube == 0); //一括ウォームアップボタン //Rev23.10 追加 by長野 2015/10/19

			//メカ制御が可能な場合のみ、スキャン可能回数を表示する
			tslblScanAvairableNum.Visible = (with.mechacontrol == 0);

            //メカ制御が可能な場合のみ、HDD残り容量を表示 Rev26.01/Rev25.101 by chouno 2017/11/16
            tslblDiskFreeSpace.Visible = (with.mechacontrol == 0);

            //メカ制御が可能な場合のみ、運転準備を表示 Rev26.01/Rev25.101 by chouno 201/11/16
            tslblEmergency.Visible = (with.mechacontrol == 0);

            //メカ制御が可能な場合のみ、インターロックを表示 Rev26.01/Rev25.101 by chouno 201/11/16
            tslblDoorInterlock.Visible = (with.seqcomm == 0 && with.mechacontrol == 0);

			//メカ制御が可能な場合のみ、マトリクス欄を表示する
			tslblMatrixSize.Visible = (with.mechacontrol == 0);

			//ビニングモード
			tslblBinningMode.Visible = (with.binning == 0);

            //Rev23.30 by長野 2016/02/23
            tsbtnExObsCam.Visible = (with.ExObsCamera == 0);

			//非常停止ステータスを初期化
			this.emergency = false;

#if DebugOn
            //デバッグ用 
            mnuDebugSub0.Visible = true;
			mnuDebugSub1.Visible = true;
			mnuDebugSub2.Visible = true;
			mnuDebugSub3.Visible = true;
			mnuDebugSub4.Visible = true;
#endif

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
		private void frmCTMenu_Load(object sender, EventArgs e)
		{
            //追加2014//11/07hata 
            this.Enabled = false;   //無効にしておく
            
            //起動タイムアウト時間       'v15.10追加 byやまおか 2009/11/02
			const int TimeOutSec = 15;	//15秒

			//起動開始時間               'v15.10追加 byやまおか 2009/11/02
			DateTime StartTime = DateTime.Now;

            //'タイムアウトフラグの初期化 'v15.10削除 byやまおか 2009/11/02
			//Dim IsTimeOut As Boolean
			//IsTimeOut = False

			//起動画面表示
			modCT30K.CT30kNowStartingFlg = true;		//v17.20追加 byやまおか 2010/09/16

            //変更2014/10/07hata_v19.51反映
            //v18.0追加 byやまおか 2011/08/31 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //string command = Environment.CommandLine?? string.Empty;
            //if (command.IndexOf("-debug", StringComparison.CurrentCultureIgnoreCase) >= 0)		//デバッグ時は表示しない
            //{
	            frmStart.Instance.Show(this);
                frmStart.Instance.Activate();
            //}     //v18.0追加 byやまおか 2011/08/31 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05


            //追加2015/02/10hata
            //フォームのキャプションのセット
            //フォームのキャプション：システム名とソフトウェアバージョン情報を表記
            this.Text = modLibrary.RemoveNull(CTSettings.t20kinf.Data.system_name.GetString()) + " " +
                        modLibrary.RemoveNull(CTSettings.t20kinf.Data.version.GetString());



			//clsTActiveXのオブジェクトを生成
            if (CTSettings.scaninh.Data.xray_remote == 0)		//v15.10条件追加 byやまおか 2009/10/28
            {

//#if DebugOn
//Rev23.10 変更 by長野 2015/10/02
#if XrayDebugOn

            //デバッグ時は仮想Ｘ線制御とする by 間々田 2004/11/29
			modXrayControl.XrayControl = new frmVirtualXrayControl();
#else
            modXrayControl.XrayControl = new XrayCtrl.clsTActiveX();
#endif
			}

			//シーケンスクラスオブジェクト作成
            if (CTSettings.scaninh.Data.seqcomm == 0)
			{
				//起動画面表示更新
				frmStart.Instance.Display("Starting SeqComm.exe ...");

#if DebugOn
                //'v11.2デバッグ時は仮想シーケンサとする by 間々田 2005/10/19
                modSeqComm.MySeq =new  frmVirtualSeqComm();              
#else
                modSeqComm.MySeq = new SeqComm.Seq();
#endif

                modSeqComm.MySeq.StatusRead();
				do
				{
					Application.DoEvents();

					//タイムアウト監視   'v15.10追加 byやまおか 2009/11/02
					if ((DateTime.Now - StartTime).TotalSeconds  > TimeOutSec)
					{
                       
                        //IsTimeOut = True
						//ストリングテーブル化　'v17.60 by 長野
						//If MsgBox("SeqComm.exeの起動に失敗しました。" & vbCrLf & "シーケンサとの接続を確認してください。", vbOKOnly) = vbOK Then
						if (MessageBox.Show(CTResources.LoadResString(20009) + "\r\n" + CTResources.LoadResString(20010), this.Text, MessageBoxButtons.OK) == DialogResult.OK)
						{
							this.Close();
						}
						return;
					}

				} while (!modSeqComm.MySeq.IsReady);
			}
            
			//オプションの設定
			frmStart.Instance.Display("Loading CT Option ...");
			modOPTION.GetCTOption();

            //キャプションのセット
            SetCaption();

			//コントロールの初期化
			InitControls();

            try
            {
                //ユーザ停止要求領域作成   v17.50追加 by 間々田 2011/01/05
                //hMapUserStop = ScanCorrect.CreateUserStopMap();
                PulsarHelper.hStopMap = PulsarHelper.CreateUserStopMap();
            }
            catch
            {
            }

			//アプリケーションのパスに移動  Rev 0-4 by 中島
			//Directory.SetCurrentDirectory(Application.Info.DirectoryPath);
            Directory.SetCurrentDirectory(Application.StartupPath);
            
            //CT30K実行状態を書き込む。
			//    構造体名：t20kinf
			//    コモン名：ct30k_running
			//    コモン値：0(実行中でない),1(実行中)
			//Call putcommon_long("t20kinf", "ct30k_running", 1) 'v10.0削除 by 間々田 2005/01/28 このコモンは未使用

			//不要なファイルを削除   '追加 by 鈴山 '97-10-31
			//If Dir(ACCESS_BIN, vbNormal) <> "" Then Kill ACCESS_BIN    'v15.0削除 by 間々田 2009/03/15

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
//			'コーン分散処理対応の場合、c:\ct\temp 下の *.cob, *.cmn を削除 by 間々田 2003/12/22
//			If (scaninh.cone_distribute = 0) Or (scaninh.cone_distribute2 = 0) Then
//				'If Dir(CtTmpDir & "*.cob", vbNormal) <> "" Then Kill CtTmpDir & "*.cob"
//				'If Dir(CtTmpDir & "*.cmn", vbNormal) <> "" Then Kill CtTmpDir & "*.cmn"
//				'関数化
//				DeleteConeDistributeFile
//			End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//セマフォフラグ初期化 05-01-31 by IWASAWA
			mainsemflg = 0;

			//メカ制御画面のロード
            if (CTSettings.scaninh.Data.mechacontrol == 0)
			{
				frmStart.Instance.Display("Getting Mecha Status...");
                //frmMechaControl.Instance.Show();
                //Load(frmMechaControl);
                ShowChild(frmMechaControl.Instance, 0, 0);

                //frmMechaControl.Instance.InitializeEventHandler();
                //Application.DoEvents();
            }

            //v19.17 少し待つ by長野　2013/09/13   //追加2014/10/07hata_v19.51反映
            modCT30K.PauseForDoEvents(1);

			//Ｘ線制御画面のロード
            if (CTSettings.scaninh.Data.xray_remote == 0)
			{
                frmStart.Instance.Display("Getting Xray Status...");
                 //frmXrayControl.Instance.Show();
                //Load(frmXrayControl);
                ShowChild(frmXrayControl.Instance, 0, frmMechaControl.Instance.Bottom);
                //Application.DoEvents();
            }

            //v19.17 少し待つ by長野　2013/09/13   //追加2014/10/07hata_v19.51反映
            modCT30K.PauseForDoEvents(1);

            //スキャン条件画面のロード
            frmStart.Instance.Display("Getting Other Status...");
            //frmScanControl.Instance.Show();
            //Load(frmScanControl);
            ShowChild(frmScanControl.Instance, 0, frmXrayControl.Instance.Bottom);
            //Application.DoEvents();

            //v19.17 少し待つ by長野　2013/09/13   //追加2014/10/07hata_v19.51反映
            modCT30K.PauseForDoEvents(1);

            //階調変換／画像サーチ画面のロード
            frmStart.Instance.Display();
            //frmImageControl.Instance.Show();
			//Load(frmImageControl);
            ShowChild(frmImageControl.Instance, 0, frmScanControl.Instance.Bottom);
            Application.DoEvents();

            //画像表示フォームのロード
            frmStart.Instance.Display();
            //frmScanImage.Instance.Show();
			//Load(frmScanImage);
            ShowChild(frmScanImage.Instance, frmScanControl.Instance.Right, 0);
            Application.DoEvents();

			//ステータスフォームのロード
            frmStart.Instance.Display();
            //frmStatus.Instance.Show();
			//Load(frmStatus);
            ShowChild(frmStatus.Instance, frmScanImage.Instance.Left, frmScanImage.Instance.Bottom);
            frmStatus.Instance.Width = frmScanImage.Instance.Width;
            Application.DoEvents();


            //ここで表示する

//            //エラーは無視する
//#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
////			On Error Resume Next

////			Dim theForm As Form
//#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            //foreach (Form theForm in Application.OpenForms)
            //{
            //    try
            //    {
            //        if (theForm.IsMdiChild)
            //        {
            //            theForm.Show();
            //            Application.DoEvents();
            //        }
            //    }
            //    catch
            //    {
            //    }
            //}
           
            
			try
			{
				//画像付帯情報フォームの表示
                //frmImageInfo.Instance.Owner = this;   // メインフォームをオーナーとする
                frmImageInfo.Instance.Show(this);
                Application.DoEvents();
			}
			catch
			{
			}

            //ここで初期化しない？全FormのLaodが終わっていない
            //起動フラグを初期化
            //modCTBusy.CTBusy = 0;

            try
			{
				//起動画面のスクロールバーを進める：90%
                frmStart.Instance.Display();
			}
			catch
			{
			}

			try
			{
				//各ＰＣステータスフォームの表示                         'V8.0 append by 間々田 2003/11/20
				//If Use_ConeDistribute Then frmDistributeStatus.Show
                if (CTSettings.scaninh.Data.cone_distribute == 0 || CTSettings.scaninh.Data.cone_distribute2 == 0)
				{
                    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                    //if (modLibrary.IsExistForm("frmDistributeStatus"))
                    //{
                    //    frmDistributeStatus.Instance.Show(this);		//v10.0変更 by 間々田 2005/01/24
                    //}
                    //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
                }
			}
			catch
			{
			}

			try
			{
				//子プロセスからの送信を受け付ける
				//socCT30K.Bind 7010
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				socCT30K.Bind(CT30KPort);			//v17.50変更 2011/01/20 by 間々田
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				socCT30K = new UpdReceiver(socCT30K_DataArrival);
				IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, CTSettings.iniValue.CT30KPort);
				socCT30K.Start(endpoint);
			}
			catch
			{
			}

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//ビニングモードの表示
			//    UpdateBinningMode
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //try
            //{
            //    //ユーザ停止要求領域作成                                                                  v17.50追加 by 間々田 2011/01/05
            //    //hMapUserStop = ScanCorrect.CreateUserStopMap();
            //    PulsarHelper.hStopMap = PulsarHelper.CreateUserStopMap();
            //}
            //catch
            //{
            //}


        }

		//*******************************************************************************
		//機　　能： アンロード（終了）確認処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void frmCTMenu_FormClosing(object sender, FormClosingEventArgs e)
		{

            //分岐
            switch (e.CloseReason)
            {

                //フォームのコントロールメニューから [閉じる] コマンドが選択された場合　および
                //コードから Unload メソッドが起動された場合
                #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                //		        Case vbFormControlMenu, vbFormCode
                #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                case CloseReason.UserClosing:

                    //×ボタンのときは何か実行中でも強制終了する（確認メッセージは表示する）
                    if (!QuitCT30K())
                    {
                        e.Cancel = true;
                        return;
                    }
                    break;
            }

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //			'05-01-31 CT30K終了時に必ずｾﾏﾌｫを開放するための処理を追加する by IWASAWA
            //			'05-01-31 分散処理時、ここでｾﾏﾌｫが取得できるまで子ﾌｫｰﾑをunloadしないための処理　by IWASAWA
            //			'If (cancel = False) And (0 = GetCommonLong("scaninh", "cone_distribute")) Then
            //			If (scaninh.cone_distribute = 0) Or (scaninh.cone_distribute2 = 0) Then   '変更 by 間々田 2005/02/01
            //				If iPCNo = PCForScan Then
            //					If Not GetScanPCSem(mainmem, mainsem, mainmap) Then Exit Sub
            //				Else
            //					If Not GetImagePCSem(mainmem, mainsem, mainmap) Then Exit Sub
            //				End If
            //				mainsemflg = 1  'セマフォ取得成功
            //			End If
            //05-01-31 by IWASAWA END!!!
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //メインフォームを消去
            this.Hide();

            modCT30K.RequestExit = true;

            //終了要求オン   'v16.01/v17.00変更 byやまおか 2010/02/02
            //If scaninh.SeqComm = 0 Then
            //    'タイマーを止める
            //    frmMechaControl.tmrSeqComm.Enabled = False
            //    frmMechaControl.tmrPIOCheck.Enabled = False
            //    End If
            //'        frmStatus.tmrStatus.Enabled = False
            //End If

            //終了要求オン   'v16.01/v17.00変更 byやまおか 2010/02/02
            if (CTSettings.scaninh.Data.seqcomm == 0)
            {
                //SeqCommが立ち上がっていればタイマーを止める
                if (modSeqComm.MySeq.IsReady)
                {   
                    //変更2014/10/07hata_v19.51反映
                    //v19.50 タイマーの統合 by長野 2013/12/17
                    //frmMechaControl.Instance.tmrSeqComm.Enabled = false;
                    modMechaControl.Flg_SeqCommUpdate = false;
                    modMechaControl.Flg_MechaControlUpdate = false;//Rev26.00 add by chouno 2017/03/13

                    frmMechaControl.Instance.tmrPIOCheck.Enabled = false;
                }
            }

            if (CTSettings.scaninh.Data.ExObsCamera == 0)
            {
                //Rev23.30 外観カメラをCapureStop by長野 2016/02/06
                frmExObsCam.Instance.CaptureStop();
            }

            //メッセージ表示
            //frmMessage.lblMessage = "処理を終了しています..."
            //ストリングテーブル化 'v17.60 by長野 2011/05/22
            //ShowMessage "処理を終了しています..."          '変更 by 間々田 2009/08/24
            modCT30K.ShowMessage(CTResources.LoadResString(20011));

            //eventArgs.Cancel = cancel;
		}

		//*******************************************************************************
		//機　　能： フォームアンロード時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//           v13.0 2007/03/19 (WEB)間々田   ダブルオブリーク対応
		//*******************************************************************************
		private void frmCTMenu_FormClosed(object sender, FormClosedEventArgs e)
		{
			//コモン読み書き用の変数宣言
			//int error_sts = 0;			//戻り値


			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
			//連続回転コーンの場合に、
//			If (smooth_rot_cone_flg = True) Then
//
//				clearRamDiskFile
//
//			End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //Rev26.00 add by chouno 2017/03/13
            modSeqComm.SeqBitWrite("PanelInhibit", false);

            //残っているフォームは消す
            try
            {
                bool bFrm = false;
                string fname = "";
                while (!bFrm)
                {
                    fname = "";
                    foreach (Form frm in Application.OpenForms)
                    {
                        //校正詳細フォーム
                        if (frm != this)
                        {
                            fname = frm.Name;
                            break;
                        }
                    }

                    if (fname != "")
                    {
                        //追加2014/08/23(検S1)hata
                        //frmCTMenu以外を消去
                        if (fname != this.Name)
                        {
                            //Application.OpenForms[fname].Close();
                            Application.OpenForms[fname].Dispose();
                        }
                    }
                    else
                    {
                        break;
                    }
                }

            }
            catch
            {
            }
            //catch(Exception ex)
            //{
            //    //Debug.Print("EX CTMenu_FormClosed_1 >> " + ex.Message);
            //    MessageBox.Show(this, "CTMenu_FormClosed_1 >> " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}


			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//05-01-31 分散処理時、MDIForm_QueryUnloadで取得したｾﾏﾌｫを解放　by IWASAWA
//			If (0 = GetCommonLong("scaninh", "cone_distribute")) And (mainsemflg = 1) Then
//			If ((scaninh.cone_distribute = 0) Or (scaninh.cone_distribute2 = 0)) And (mainsemflg = 1) Then             '変更 by 間々田 2005/02/01
//				If Not RlsSem(mainmem, mainsem, mainmap) Then
//					Exit Sub
//				End If
//				mainsemflg = 0  'セマフォ開放成功
//			End If
//			'05-01-31 by IWASAWA END!!!
//
//			'If Use_ConeDistribute And (iPCNo = PCForScan) Then
//			'If (Use_ConeDistribute Or UseConeDistribute2) And (iPCNo = PCForScan) Then  'v10.0変更 by 間々田 2005/01/24
//			If (scaninh.cone_distribute = 0) And (iPCNo = PCForScan) Then                           '分散処理２の場合は表示しない by 間々田 2005/02/16
//				'問い合わせメッセージ表示：他のPCをシャットダウンさせますか？
//				If vbYes = MsgBox(LoadResString(12912), vbYesNo + vbExclamation + vbDefaultButton2) Then
//					Call modMemoryStat.ShutdownOtherPC
//				End If
//			End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


			//    'Ｘ線ON中ならオフする   added by 山本　2002-9-10
//			If Use_XrayRemote Then
//				'Kevex(t20kinf.system_type=1)の場合はＸ線をOFFする→浜ホト(t20kinf.system_type=2)の場合もＸ線をOFFするように変更 by 間々田 2004/02/03
//				Select Case XrayType
//'					Case XrayTypeKevex, XrayTypeHamaL9181
//					Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191 'v9.5 修正 by 間々田 2004/09/16 浜ホト160kV対応
//						XrayControl.Xrayonoff_Set 2             'Ｘ線をオフする
//						Sleep 150                            'Ｘ線オフ処理が完了するまでのスリープ
//				End Select
//			End If

			// 子プロセスとの接続を閉じる
            if (socCT30K != null)
                socCT30K.Stop();

			//SeqComm終了
			if (modSeqComm.MySeq != null)
			{
				//With frmMessage.lblMessage
				//    .Caption = "シーケンサソフトを終了させています..."
				//    .Refresh
				//End With
				try
				{
    				//ストリングテーブル化 'v17.60 by長野 2011/05/22			
				    //ShowMessage "シーケンサソフトを終了させています..."  '変更 by 間々田 2009/08/24
				    modCT30K.ShowMessage(CTResources.LoadResString(20012));

				    //シーケンサオブジェクトを開放
				    modSeqComm.MySeq.Dispose();
				    modSeqComm.MySeq = null;

					//SeqComm.exeが死ぬまで待つ：タイムアウトは10秒
					modCT30K.WaitWhileAlive("C:\\CT\\ActiveX\\SeqComm.exe", 10000);

                    //Rev26.00 add by chouno 2017/03/13
                    modCT30K.ShowMessage(CTResources.LoadResString(20011));
				}
                catch
                {
                }
                //catch (Exception ex)
                //{
                //    //Debug.Print("EX CTMenu_FormClosed_3 >> " + ex.Message);
                //    MessageBox.Show(this, "CTMenu_FormClosed_3 >> " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //}
            }

#if (!IpClose)  //'v19.50 v19.41とv18.02の統合 by長野 2013/11/05  //追加2014/10/07hata_v19.51反映

            try
			{
				//イメージプロ終了
                if (!string.IsNullOrEmpty(CTSettings.iniValue.ImageProExe))		//v11.5 if文のみ追加 by 間々田 2006/08/25
                {
                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    //error_sts = Ipc32v5.IpAppExit();					//added by 山本　2004-3-20　イメージプロのフリーズ対策としてCT30K終了時のみ終了する
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）

                    //変更2015/02/26hata_イメージプロを先には終了させる
                    //ImageProServerが起動していないと消えないので
                    if (!AppValue.IsDebug)
                    {
                        string ipf = Path.GetFileNameWithoutExtension(CTSettings.iniValue.ImageProExe);
                        //イメージプロServerの起動確認をする
                        Process[] ps = Process.GetProcessesByName(ipf);
                        foreach (Process p in ps)
                        {
                            //起動中なら終了する
                            p.CloseMainWindow();
                            p.WaitForExit(1000);
                            p.Dispose();
                            break;
                        }
                    }
                    CTSettings.IPOBJ.CloseImageProServer();

                    //削除2015/02/26hata_イメージプロを先には終了させる
                    //if (!AppValue.IsDebug)
                    //{
                    //    //イメージプロ終了
                    //    CallImageProFunction.CallIpEnd();
                    //} 
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//
                }
            }
			catch
            {
            }
            //catch(Exception ex)
            //{
            //    //Debug.Print("EX CTMenu_FormClosed_4 >> " + ex.Message);
            //    MessageBox.Show(this, "CTMenu_FormClosed_4 >> " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}
#endif
			
            
            //Unload frmMessage
            
            //Rev26.00 change by chouno 2017/03/13
            //try
            //{
            //    modCT30K.HideMessage();			//変更 by 間々田 2009/08/24
            //}
            //catch
            //{
            //}
            
            //catch(Exception ex)
            //{
            //    //Debug.Print("EX CTMenu_FormClosed_5 >> " + ex.Message);
            //    MessageBox.Show(this, "CTMenu_FormClosed_5 >> " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}

			try
			{
				//ダブルオブリークの終了                 'v13.0追加 by 間々田 2007/03/19
				if (CTSettings.scaninh.Data.double_oblique == 0)
				{
					modDoubleOblique.EndDoubleOblique();
				}
			}
            catch
            {
            }

            //Rev26.00 ReciUDP.exeが存在する場合は削除
            try
            {
                if (modCT30K.IsAlive(AppValue.RECONMST) || modCT30K.IsAlive(AppValue.CONEBEAM) || modCT30K.IsAlive(AppValue.SCANAV) || 
                    modCT30K.IsAlive(AppValue.SCANOAV) || modCT30K.IsAlive(AppValue.CONERECON)) 
                {
                    modCT30K.CallUserStopSet();

                    modCT30K.WaitWhileAlive(AppValue.RECONMST, 30000);
                    modCT30K.WaitWhileAlive(AppValue.CONERECON, 30000);
                    modCT30K.WaitWhileAlive(AppValue.CONEBEAM, 30000);
                    modCT30K.WaitWhileAlive(AppValue.SCANAV, 30000);
                    modCT30K.WaitWhileAlive(AppValue.SCANOAV, 30000);
                }
            }
            catch
            { 
            }

            //catch(Exception ex)
            //{
            //    //Debug.Print("EX CTMenu_FormClosed_6 >> " + ex.Message);
            //    MessageBox.Show(this, "CTMenu_FormClosed_6 >> " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}

			//'透視画像サーバを終了させる             'v17.50追加 by 間々田 2010/12/17   'v17.50削除 by 間々田 2011/02/28 TerminateProcessは勧められていないので使用しない
			//If TransImageServerID <> 0 Then
			//
			//    Dim hProcess As Long
			//
			//    'プロセスハンドル取得
			//    hProcess = OpenProcess(SYNCHRONIZE Or PROCESS_TERMINATE, True, TransImageServerID)
			//
			//    '指定されたハンドルプロセスを終了
			//    TerminateProcess hProcess, 0
			//
			//    'ハンドルクローズ
			//    CloseHandle hProcess
			//
			//End If

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
			//透視画像サーバーのウィンドウハンドルを求める   'v17.50追加 by 間々田 2011/02/28 上記のTerminateProcessの代わりの方法
			//    Dim TransImageServerWindowHandle As Long
			//    TransImageServerWindowHandle = GetWindowHandleByProcess(TransImageServerID)
			//
			//    '透視画像サーバを終了させる
			//    If TransImageServerWindowHandle <> 0 Then       'v17.50追加 by 間々田 2011/02/28 上記のTerminateProcessの代わりの方法
			//        Call SendMessage(TransImageServerWindowHandle, WM_CLOSE, 0&, 0&)
			//    End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			try
			{
                //ユーザ停止要求領域破棄                     'v17.50追加 by 間々田 2011/01/05
                if ((PulsarHelper.hStopMap != IntPtr.Zero) && (PulsarHelper.hStopMap.ToInt32() != 0))
                {
                    PulsarHelper.DestroyUserStopMap(PulsarHelper.hStopMap);
                }
            }
			catch
            {
            }
            //catch(Exception ex)
            //{
            //    //Debug.Print("EX CTMenu_FormClosed_7 >> " + ex.Message);
            //    MessageBox.Show(this, "CTMenu_FormClosed_7 >> " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}

            try
			{
                //共有メモリを解放
                CTSettings.ExitSharedCTCommon();
            }
            catch
            {
            }
            //catch(Exception ex)
            //{
            //   // Debug.Print("EX CTMenu_FormClosed_8 >> " + ex.Message);
            //    MessageBox.Show(this, "CTMenu_FormClosed_8 >> " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}

            //CT30K終了
            //Environment.Exit(0);

            //Rev26.00 移動 by chouno 2017/03/13
            //Unload frmMessage
            try
            {
                modCT30K.HideMessage();			//変更 by 間々田 2009/08/24
            }
            catch
            {
            }

		}

		//'*******************************************************************************
		//'機　　能： 「階調変換」メニュー選択時処理
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//'*******************************************************************************
		//Private Sub mnuWlww_Click()
		//
		//    'ｶﾗｰ処理実行中OnOff処理
		//    'If ColorOnOff = 99 Then
		//    If IsExistForm(frmColor) Then       'v10.2変更 by 間々田 2005/06/14
		//
		//        'メッセージ表示：ｶﾗｰ処理実行中は、階調変換は行えません。
		//        MsgBox LoadResString(9391), vbExclamation
		//
		//    '画像ﾊﾟﾚｯﾄのﾓｰﾄﾞをﾁｪｯｸ
		//    ElseIf GetCommonLong("dispinf", "color_max") <> 8191 Then
		//
		//        'メッセージ表示：ｵﾘｼﾞﾅﾙｶﾗｰ(CT値)でﾊﾟﾚｯﾄ設定されているので、階調変換は行えません。
		//        MsgBox LoadResString(9390), vbExclamation
		//
		//    Else
		//
		//        '階調変換フォームを表示
		//        WLWWForm.Show
		//
		//    End If
		//
		//End Sub

		//*******************************************************************************
		//機　　能： 空き容量監視タイマ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//           v13.0  2007/03/19 (WEB)間々田   ダブルオブリーク対応
		//*******************************************************************************
		private void tmrStatus_Tick(object sender, EventArgs e)
		{
			//タイマーの２重呼び出し防止
			if (tmrStatus_Tick_BUSYNOW)			//状態(True:実行中,False:停止中)
			{
				return;
			}

			tmrStatus_Tick_BUSYNOW = true;

			//ディスクの空き容量の計算(MB)と表示
			DiskFreeSpace = modLibrary.GetDiskFreeKByte(AppValue.MyDrive);

			//元の状態に戻す
			tmrStatus_Tick_BUSYNOW = false;
		}

		//*******************************************************************************
		//機　　能： I.I.視野切替処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v16.01  10/02/17   (検SS)山影    新規作成
		//*******************************************************************************
		private void SwitchIIField()
		{
			int i = 0;
			int ii = 0;
			int Count = 0;

			Count = CTSettings.scaninh.Data.iifield_char.Length;

            ii = (int)myIIField;
			for (i = 1; i <= Count; i++)
			{
				ii = (ii + 1) % Count;
				if (CTSettings.scaninh.Data.iifield_char[ii] == 0)
				{
					break;
				}
			}

            if (ii == (int)myIIField)
			{
				return;
			}

			//I.I.視野切り替えボタンを一旦使用不可にする
			tsbtnIIField.Enabled = false;

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
//			'I.I.視野を設定
//			If IsHSCmode Then
//				Call SeqBitWrite(Choose(ii + 1, "TVII9", "TVII6", "TVII4"), True)
//			Else
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //23.40 by長野 2016/04/05 / //Rev23.21 変更 by長野 2016/02/22
            string[] s;
            //if (modHighSpeedCamera.IsHSCmode)
            //change by chouno 2019/02/12
            if (modHighSpeedCamera.IsHSCmode || modHighSpeedCamera.IsDropTestmode)
            //				Call SeqBitWrite(Choose(ii + 1, "TVII9", "TVII6", "TVII4"), True)
            //			Else
            {
                s = new string[] { "TVII9", "TVII6", "TVII4" };
            }
            else
            {
                s = new string[] { "II9", "II6", "II4" };
            }
            modSeqComm.SeqBitWrite(s[ii], true);

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
//			End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

		}

		//*******************************************************************************
		//機　　能： ツールバー選択時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//           v13.0  2007/03/19 (WEB)間々田   ダブルオブリーク対応
		//*******************************************************************************
		private void Toolbar1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			this.Activate();	//v15.10追加 byやまおか 2009/11/19

            if (toolBar1ItemEventLock == true)
                return;

            toolBar1ItemEventLock = true;

			switch (e.ClickedItem.Name)
			{
                #region//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
//				Case "Wizard"
//
//					'ウィザード表示
//					frmWizard.Show vbModal
                #endregion				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                case "tsbtnXray":

					//Ｘ線ＯＮ／ＯＦＦ設定
					switch (myXrayOnOffStatus)
					{
						case XrayOnOffStatusConstants.XrayOffAvail:
						case XrayOnOffStatusConstants.XrayOffNotAvail:

                            #region//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                            //Ｘ線オンと連動して透視ライブをオンする     'v15.0追加 by 間々田 2009/07/21
                            //							If IsCTmode Then        'v16.02 条件追加 by 山影 10-03-02
                            #endregion//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                            //産業用CTモードの場合   'v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                            //Rev23.10 操作パネルONの機能を分離 by長野 2015/10
                            //if ((CTSettings.scaninh.Data.avmode == 0))
                            if ((CTSettings.scaninh.Data.avmode == 0) || (CTSettings.scaninh.Data.op_panel == 0))
                            {
                                //メッセージ表示：
                                //操作パネルがONなら
                                if (modSeqComm.MySeq.PcInhibit)
                                {
                                    //操作パネルがONのため動作しません。
                                    MessageBox.Show(CTResources.LoadResString(17513), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    toolBar1ItemEventLock = false;
                                    return;
                                }
                            }

                            //Rev26.40 add by chouno 2019/02/17
                            if (CTSettings.scaninh.Data.high_speed_camera == 0 && CTSettings.iniValue.HSCSettingType == 1)
                            {
                                //メッセージ表示：
                                //動作許可が不許可なら
                                if (modSeqComm.MySeq.PcInhibit)
                                {
                                    //動作許可がOFFのため動作しません。
                                    MessageBox.Show(CTResources.LoadResString(27000), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    toolBar1ItemEventLock = false;
                                    return;
                                }
                            }

                            //Rev23.40 b長野 2016/04/05 / //Rev23.21 高速度ONの場合はX線だけを使用 by長野 2016/02/23
                            if (modHighSpeedCamera.IsCTmode)
                            {
                                frmTransImage.Instance.CaptureOn = true;
                            }
                            else
                            {
                                modXrayControl.XrayOn();
                            }

                            break;

                            #region//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                            //ElseIf IsHSCmode Then   'v16.02 条件追加  by 山影 10-03-02
                            #endregion//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                                //'Ｘ線停止要求クリア   'v11.5追加 by 間々田 2006/06/26
								//UserStopClear
								//
								//'連続回転コーンビーム＋高速再構成の時は、RAMディスクのscanstopを使う v17.40 追加 by 長野
								//If smooth_rot_cone_flg = True Then
								//
								//    UserStopClear_rmdsk
								//
                                //End If

                                #region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''

                                //停止要求フラグをクリアする             'v17.50上記の処理を関数化 by 間々田 2011/02/17
								//CallUserStopClear

								#endregion //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

								//v17.00削除　山本 2010-02-03　上のfrmTransImage.CaptureOn内でX線ONしているため削除　パーキンエルマーFPDの時再ONしてしまうため
								//'Ｘ線オン命令
								//If scaninh.xray_remote = 0 Then   'v15.10条件追加 byやまおか 2009/10/28
								//    TryXrayOn , , False, True 'v11.5変更 2006/06/26
                                //End If

                            #region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                            //							End If
                            #endregion //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                        case XrayOnOffStatusConstants.XrayOnAvail:
						case XrayOnOffStatusConstants.XrayOnNotAvail:

                            //Ｘ線オフと連動して透視ライブをオフする     'v15.0追加 by 間々田 2009/07/21

                            #region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                            //If IsCTmode Then    'v16.02 条件追加 by 山影 10-03-02
                            #endregion //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                            //Rev23.40 by長野 2016/04/05 / //Rev23.21 高速度ONの場合は、キャプチャは使わない by長野 2016/02/22
                            if (modHighSpeedCamera.IsCTmode)
                            {
                                frmTransImage.Instance.CaptureOn = false;
                            }


							#region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                            //End If
                            #endregion //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                            #region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                            //フィラメント調整を実施するためのフラグをリセット   'v12.01追加 by 間々田 2006/12/04
							//If XrayType = XrayTypeViscom Then   'v15.10条件追加 byやまおか 2009/10/28
							//    FilamentAdjustAfterWarmup = False
                            //End If
                            #endregion //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

                            //Ｘ線オフ命令
							//v15.10条件追加 byやまおか 2009/10/28
							if (CTSettings.scaninh.Data.xray_remote == 0)
							{
							    modXrayControl.XrayOff();
							}

							// 'Ｘ線停止要求   'v11.5追加 by 間々田 2006/06/26
							// UserStopSet
							//
							//'連続回転コーンビーム＋高速再構成の時は、RAMディスクのscanstopを使う v17.40 追加 by 長野
							// If smooth_rot_cone_flg = True Then
							//
							//     UserStopSet_rmdsk
							//
							// End If

							//実行中の処理に対して停止要求をする     'v17.50上記の処理を関数化 by 間々田 2011/02/17
                            modCT30K.CallUserStopSet();
							break;
					}
					break;

                case "tsbtnDoorLock":
                    
                    //追加2014/10/07hata_v19.51反映
                    //メカが動ける(パネルがOFF)かチェック     'v18.00追加 byやまおか 2011/07/02
                    //if (!modMechaControl.IsOkMechaMove())
                    if (!modMechaControl.IsOkMechaMove(doorLock: false))//Rev25.10 引数追加 by 長野 2017/09/11
                    {
                        toolBar1ItemEventLock = false;
                        return;
                    }


					//扉電磁ロックＯＮ／ＯＦＦ設定
					switch (myDoorStatus)
					{
						case DoorStatusConstants.DoorClosed:
                            modSeqComm.SeqBitWrite("DoorLockOn", true);
							break;
						case DoorStatusConstants.DoorLocked:
                            modSeqComm.SeqBitWrite("DoorLockOff", true);
							break;
						default:		//v17.44追加 byやまおか 2011/02/16
							//キー挿入がない場合はドアロックを実行してみる
							//ロックできなかった場合は何もしない
							if (CTSettings.scaninh.Data.door_keyinput != 0)
							{
                                modSeqComm.SeqBitWrite("DoorLockOn", true);
							}
							break;
					}
					break;

				//'I.I.電源オンオフボタン            '削除 by 間々田 2009/07/21 I.I.電源ボタンはメカ詳細画面に移動
				//Case "I.I.Power"
				//
				//    'I.I.電源オンオフボタンを一旦使用不可にする
				//    Button.Enabled = False
				//
				//    'I.I.電源を設定
				//    SeqBitWrite IIf(Button.Image = "I.I.Off", "IIPowerOn", "IIPowerOff"), True

				//I.I.視野切り替えボタン
                //case "tsbtnI.I.Field":
                //Rev20.01 変更 by長野 2015/05/29
                case "tsbtnIIField":

					//v16.01 I.I.視野切替処理を関数化し、同処理をコメントアウト by 山影 10-02-17
					SwitchIIField();
					break;

//					Dim i   As Integer
//					Dim ii  As Integer
//					Dim Count As Integer
//
//					Count = UBound(scaninh.iifield_char) - LBound(scaninh.iifield_char) + 1
//
//					ii = myIIField
//					For i = 1 To Count
//						ii = (ii + 1) Mod Count
//						If scaninh.iifield_char(ii) = 0 Then Exit For
//					Next
//
//					If ii <> myIIField Then
//
//						'I.I.視野切り替えボタンを一旦使用不可にする
//						Button.Enabled = False
//
//						'I.I.視野を設定
//						SeqBitWrite Choose(ii + 1, "II9", "II6", "II4"), True
//
//					End If

				//ライブ画像処理オンオフ         '削除 by 間々田 2009/07/21 ライブ画像ボタンは削除
                case "tsbtnLiveImage":

                    //追加2014/10/07hata_v19.51反映
                    //メカが動ける(パネルがOFF)かチェック     'v18.00追加 byやまおか 2011/07/02
                    if (!modMechaControl.IsOkMechaMove())
                    {
                        toolBar1ItemEventLock = false;
                        return;
                    }

					//PkeFPDの場合はゲイン補正をしない   'v17.00追加 byやまおか 2010/02/04
                    if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
					{
                        CTSettings.scanParam.FPGainOn = false;
					}

                    //産業用CTモードの場合   'v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    //Rev23.10 操作パネルONの機能を分離 by長野 2015/10
                    //if ((CTSettings.scaninh.Data.avmode == 0))
                    if ((CTSettings.scaninh.Data.avmode == 0) || (CTSettings.scaninh.Data.op_panel == 0))
                    {
                        //メッセージ表示：
                        //操作パネルがONなら
                        if (modSeqComm.MySeq.PcInhibit)
                        {
                            //操作パネルがONのため動作しません。
                            MessageBox.Show(CTResources.LoadResString(17513), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            toolBar1ItemEventLock = false;
                            return;
                        }
                    }

                    //Rev26.40 add by chouno 2019/02/17
                    if (CTSettings.scaninh.Data.high_speed_camera == 0 && CTSettings.iniValue.HSCSettingType == 1)
                    {
                        //メッセージ表示：
                        //動作許可が不許可なら
                        if (modSeqComm.MySeq.PcInhibit)
                        {
                            //動作許可がOFFのため動作しません。
                            MessageBox.Show(CTResources.LoadResString(27000), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            toolBar1ItemEventLock = false;
                            return;
                        }
                    }

					//ライブ画像オンオフを切り替える
                    frmTransImage.Instance.CaptureOn = !frmTransImage.Instance.CaptureOn;
					break;

                case "tsbtnOpen":

					//画像ファイル選択ダイアログ表示
					string FileName = modFileIO.GetFileName(StringTable.IDS_Open, CTResources.LoadResString(StringTable.IDS_CTImage), ".img");

					if (!string.IsNullOrEmpty(FileName))
					{
						frmScanImage.Instance.Target = FileName;
					}
					break;

                case "tsbtnPrint":

					//印刷を実行
					ImagePrint.DoPrint();
					break;

                case "tsbtnDoubleOblique":

					//ダブルオブリークをアクティブにする
					modDoubleOblique.ActivateDoubleOblique();
					break;

                case "tsbtnMainte":

					//パスワード入力フォームを表示：正しいパスワードが入力されたらメンテナンスフォームを表示
                    if (frmMtncpass.Dialog())
					{
                        if (!modLibrary.IsExistForm("frmMaint"))	//追加2015/01/30hata_if文追加
                        {
                            frmMaint.Instance.Show(this);
                        }
                        else
                        {
                            frmMaint.Instance.WindowState = FormWindowState.Normal;
                            frmMaint.Instance.Visible = true;
                        }
                    
                    }
					break;

                case "tsbtnXrayAllWup":

                    //機構部が動作可能かチェック（上記を関数化）
                    if (!modMechaControl.IsOkMechaMove())   //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                        return;

                    //産業用CTモードの場合   'v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    //Rev23.10 操作パネルONの機能を分離 by長野 2015/10
                    //if ((CTSettings.scaninh.Data.avmode == 0))
                    if ((CTSettings.scaninh.Data.avmode == 0) || (CTSettings.scaninh.Data.op_panel == 0))
                    {
                        //メッセージ表示：
                        //操作パネルがONなら
                        if (modSeqComm.MySeq.PcInhibit)
                        {
                            //操作パネルがONのため動作しません。
                            MessageBox.Show(CTResources.LoadResString(17513), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            toolBar1ItemEventLock = false;
                            return;
                        }
                    }
                    //Rev26.40 add by chouno 2019/02/17
                    if (CTSettings.scaninh.Data.high_speed_camera == 0 && CTSettings.iniValue.HSCSettingType == 1)
                    {
                        //メッセージ表示：
                        //動作許可が不許可なら
                        if (modSeqComm.MySeq.PcInhibit)
                        {
                            //動作許可がOFFのため動作しません。
                            MessageBox.Show(CTResources.LoadResString(27000), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            toolBar1ItemEventLock = false;
                            return;
                        }
                    }

                    mod2ndXray.PackageWarmup();

                    break;

                case "tsbtnExObsCam":

                    frmExObsCam.Instance.ExObsCam.ExObsCamProcessStart(DispFlg:0);

                    break;
			}
            toolBar1ItemEventLock = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private void Toolbar1_MouseDown(object s, MouseEventArgs e)
		{
			//テストモード時：左側にあるツールバー上でマウスクリックをしたときにポップアップメニューが表示されるようにする
            //if (modCT30K.IsTestMode)
            if (AppValue.IsTestMode)
			{
				//右マウスボタン
				if (e.Button == MouseButtons.Right)
				{
					//ポップアップメニューを表示
					this.ContextMenuStrip = mnuPopUp;
				}
			}
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
		//Private Sub SetCaption()
		public void SetCaption()		//v17.20 検出器切替用にpublic化 by 長野 2010/09/06
		{
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

			//フォームのキャプション：システム名とソフトウェアバージョン情報を表記
			this.Text = modLibrary.RemoveNull(CTSettings.t20kinf.Data.system_name.GetString()) + " " +
                        modLibrary.RemoveNull(CTSettings.t20kinf.Data.version.GetString());

            tsbtnOpen.ToolTipText = StringTable.BuildResStr(StringTable.IDS_Open, StringTable.IDS_CTImage);	//画像ファイルを開く
			//.Buttons("I.I.Power").ToolTipText = GetResString(IDS_PowerSupply, GStrIIOrFPD)  'I.I.電源（またはFPD電源） '削除 by 間々田 2009/07/21 I.I.電源ボタンはメカ詳細画面に移動

            tslblDoorInterlock.Text = CTResources.LoadResString(12761);	//インターロック
			tslblEmergency.Text = CTResources.LoadResString(9200);			//非常停止

			//v17.60 英語版対応 by長野 2011/05/25
			if (modCT30K.IsEnglish == true)
			{
				tslblEmergency.AutoSize = true;
			}

		}

		//*******************************************************************************
		//機　　能： マトリクスサイズ表示の更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.5  2006/06/08   ????????      新規作成
		//*******************************************************************************
		public void UpdateMatrixSize()
		{
            var _scansel = CTSettings.scansel.Data;

			//マトリクスサイズの表示
            //変更2014/11/26hata_v19.51dnet
            //tslblMatrixSize.Text = CTResources.LoadResString(_scansel.data_mode == (int)ScanSel.DataModeConstants.DataModeCone ? StringTable.IDS_ConeBeam : StringTable.IDS_Scan) + "(%1 x %1)".Replace("%1", Convert.ToString(Math.Pow(_scansel.matrix_size + 7, 2)));
            tslblMatrixSize.Text = CTResources.LoadResString(_scansel.data_mode == (int)ScanSel.DataModeConstants.DataModeCone ? StringTable.IDS_ConeBeam : StringTable.IDS_Scan) + "(%1 x %1)".Replace("%1", Convert.ToString(Math.Pow(2, (_scansel.matrix_size + 7))));

        }

        //追加2014/10/07hata_v19.51反映
        //*******************************************************************************
		//機　　能： ビニングモード表示の更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： タイマー処理からはずした
		//
		//履　　歴： v11.4  2006/03/13   ????????      新規作成
		//*******************************************************************************
        public void UpdateBinningMode()
        {
            //StatusBar1.Panels("BinningMode").Text = CTResources.LoadResString((StringTable.IDS_BinningMode) & " " & GetBinningStr(scansel.binning)   'ビニングモード
            tslblBinningMode.Text = CTResources.LoadResString(StringTable.IDS_BinningMode) +  " " + modCT30K.GetBinningStr(CTSettings.scansel.Data.binning);  // 'ビニングモード
        }

		//*******************************************************************************
		//機　　能： スキャン可能回数を更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.5  2006/03/13   ????????      新規作成
		//*******************************************************************************
		public void UpdateScanAvairableNum()
		{
			double theSize = 0;
			double FreeMB = 0;

			theSize = myScanImageSize;

			if ((theSize > 0) && (myDiskFreeSpace != 0))
			{
                //使用できるディスク容量(単位：MB)   最低空き容量：Ｃドライブの場合 HD_LIMIT その他 0
                //2014/11/06hata キャストの修正
                FreeMB = myDiskFreeSpace / 1024D - (string.Compare(AppValue.MyDrive, "C:", true) == 0 ? modCT30K.HD_LIMIT : 0);

				//コーンビームの場合は生データ保存なしでも一時的にファイル保存することを考慮する 'v11.5追加 by 間々田 2006/06/29
				if ((CTSettings.scansel.Data.rawdata_save == 0) && (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone))
				{
                    //2014/11/06hata キャストの修正
                    FreeMB = FreeMB - modCT30K.GValRawFileSize / 1024D / 1024D;
				}

				//スキャン可能回数の計算と表示：
				//   スキャン可能回数＝（空き容量－最低空き容量）÷ スキャンに必要なディスク容量
				ScanAvairableNum = (int)modLibrary.MaxVal(Math.Floor(FreeMB / theSize), 0);
			}
		}

		//*******************************************************************************
		//機　　能： メッセージ表示用のタイマー処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		private void tmrMessage_Tick(object sender, EventArgs e)
		{
			//タイマー停止
			tmrMessage.Enabled = false;

			//メッセージ表示
			//検出器切替用に条件式を追加 メッセージボックスでＯＫを押したかどうかの判定
            //変更2014/10/07hata_v19.51反映
            if (CTSettings.SecondDetOn)
            {
                if (MessageBox.Show(tmrMessage.Tag.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation) == DialogResult.OK)
                    mod2ndDetctor.MsgBoxOK = true;
            }
            else
            {

                MessageBox.Show(tmrMessage.Tag.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
		}

        //*******************************************************************************
		//機　　能： frmCTMenuからのfrmXrayControl呼び出し
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V23.10  15/09/29  (検S1)長野      新規作成
		//*******************************************************************************
        private void Call_frmXrayControl()
        {
            //Ｘ線制御画面のロード
            if (CTSettings.scaninh.Data.xray_remote == 0)
            {
                //frmXrayControl.Instance.Show();
                //Load(frmXrayControl);
                ShowChild(frmXrayControl.Instance, 0, frmMechaControl.Instance.Bottom);
                //Application.DoEvents();
            } 
        }

		//'*******************************************************************************
		//'機　　能： メッセージ表示用のタイマー処理
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： v15.00 2009/08/24 (SS1)間々田   リニューアル
		//'*******************************************************************************
		//Private Sub tmrMessageNoButton_Timer()
		//
		//    'タイマー停止
		//    tmrMessageNoButton.Enabled = False
		//
		//    '表示するメッセージ
		//    frmMessage.lblMessage.Caption = tmrMessageNoButton.tag
		//    frmMessage.lblMessage.Refresh
		//
		//    '表示
		//    If Not frmMessage.Visible Then
		//        If RequestExit Then
		//            frmMessage.Show , Me
		//        Else
		//            frmMessage.Show vbModal
		//        End If
		//    End If
		//
		//End Sub
		//*******************************************************************************
		//機　　能： RAMディスク生成チェック用タイマー
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 5秒に一回、RAMディスク（Rドライブ）が生成されているか確認する。
		//
		//履　　歴： V17.40 2010/10/21 (検S1)長野    新規作成
		//*******************************************************************************
		//Private Sub ramDiskChk_Timer()

		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
		//Private Sub tmrRamDiskChk_Timer()   'v17.40名称変更 byやまおか 2010/10/26
		//    Dim TargetDriveName As String
		//    Dim TargetFileName As String
		//    Dim TargetFolderName As String
		//
		//    Dim Error As Long
		//
		//    TargetDriveName = "R:"
		//    TargetFolderName = "R:\CT\COMMON"
		//    TargetFileName = "R:\CT\COMMON\scanstop"
		//
		//    'ファイルシステムオブジェクトを生成
		//    Dim FSO As FileSystemObject
		//    Set FSO = CreateObject("Scripting.FileSystemObject")
		//
		//    tmrRamDiskChk.Interval = 3000   'v17.40追加 byやまおか 2010/10/26
		//
		//    'RAMディスクが生成されているか、または、すでに連続回転コーンビーム実行可能フラグがたっているかをチェック
		//    If (smooth_rot_cone_flg = True) Or (FSO.DriveExists(TargetDriveName)) Then
		//
		//        '生成されている場合、Rドライブの全ファイル削除(フォルダ以外)
		//        ramdiskclear
		//
		//        'R:\CT\COMMON\scanstopの有無を確認
		//        If Not FSO.FileExists(TargetFileName) Then
		//
		//        'R:\CT\COMMON\scanstopが存在しない場合scanstopを生成する
		//
		//            'scanstopを入れるフォルダが存在しない場合は先にフォルダを生成
		//            If Not FSO.FolderExists(TargetFolderName) Then
		//
		//                FSO.CreateFolder ("R:\CT")
		//                FSO.CreateFolder ("R:\CT\COMMON")
		//
		//            End If
		//
		//            'scanstopファイル生成
		//            Error = scanstop_set_rmdsk
		//
		//            If Error <> 0 Then
		//
		//                Call ErrMessage(Error, vbCritical)
		//
		//                'ファイル生成に失敗した場合はタイマーをOFFにする
		//                'ramDiskChk.Enabled = False
		//                tmrRamDiskChk.Enabled = False   'v17.40名称変更 byやまおか 2010/10/26
		//
		//                'オブジェクトを破棄
		//                Set FSO = Nothing
		//
		//                Exit Sub
		//
		//            End If
		//
		//        End If
		//
		//        '連続回転コーン実行可能フラグをONにする
		//         smooth_rot_cone_flg = True
		//
		//        'RAMディスクができたらキャプチャオープンする    'v17.40追加 byやまおか 2010/10/26
		//        frmTransImage.CaptureOpen
		//
		//        'getMACaddressをここで実行する(RAMディスク作成中にVC側でコールすると止まってしまうため)
		//        getMACaddress   'v17.40追加 byやまおか 2010/10/26
		//
		//        'フラグがONになったらタイマーをOFFにする
		//        'ramDiskChk.Enabled = False
		//        tmrRamDiskChk.Enabled = False   'v17.40名称変更 byやまおか 2010/10/26
		//
		//    End If
		//
		//    'オブジェクトを破棄
		//    Set FSO = Nothing
		//
		//    'キャプチャステータスを更新 'v17.40追加 byやまおか 2010/10/26
		//    frmStatus.UpdateCaptureStatus

		//End Sub
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

		//*******************************************************************************
		//機　　能： RAMディスク内のファイル削除用関数
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： RAMディスク（Rドライブ）を構築している場合は、中身を削除する
		//
		//履　　歴： V17.40 2010/10/21 (検S1)長野    新規作成
		//*******************************************************************************
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//Public Function clearRamDiskFile()
		//
		//    Dim TargetDriveName As String
		//
		//    TargetDriveName = "R:"
		//
		//    'ファイルシステムオブジェクトを生成
		//    Dim FSO As FileSystemObject
		//    Set FSO = CreateObject("Scripting.FileSystemObject")
		//
		//    'RAMディスクが生成されているかをチェック
		//    If FSO.DriveExists(TargetDriveName) Then
		//
		//        '生成されている場合、Rドライブの全ファイル削除(フォルダ以外)
		//        ramdiskclear
		//
		//    End If
		//
		//    'オブジェクトを破棄
		//    Set FSO = Nothing
		//
		//End Function
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''

		//********************************************************************************
		//   ここからデバッグ用
		//********************************************************************************

		//*******************************************************************************
		//機　　能： 「デバッグ」メニュー選択時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public void mnuDebugSub_Click(object sender, EventArgs e)
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Select Case Index
            
				'フォーム一覧
				Case 0
					'ロードされているフォームの一覧を表示する
					DispForms
                        
				'仮想Ｘ線制御
				Case 1
					frmVirtualXrayControl.Show , frmCTMenu

				'仮想シーケンサ通信
				Case 2
					frmVirtualSeqComm.Show , frmCTMenu

				'ErrMessage関数のテスト
				Case 3
					Dim strWork As String
					'ストリングテーブル化 'v17.60 by 長野 2011/05/22
					'strWork = InputBox("エラー番号：", "ErrMessage関数のテスト")
					strWork = InputBox(LoadResString(20013), LoadResString(20014))
					If IsNumeric(strWork) Then ErrMessage Val(strWork)
            
				'タイマーチェック
				Case 4
					frmCheckTimer.Show , frmCTMenu

			End Select
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

#if (!DegugOn)
			//フォーム一覧
			if (sender == mnuDebugSub0)
			{
				//ロードされているフォームの一覧を表示する
				DispForms();
			}

			//仮想Ｘ線制御
			else if (sender == mnuDebugSub1)
			{
                if (!modLibrary.IsExistForm("frmVirtualXrayControl"))	//追加2015/01/30hata_if文追加
                {
                    frmVirtualXrayControl.Instance.Show(this);
                }
                else
                {
                    frmVirtualXrayControl.Instance.WindowState = FormWindowState.Normal;
                    frmVirtualXrayControl.Instance.Visible = true;
                }

            }

			//仮想シーケンサ通信
			else if (sender == mnuDebugSub2)
			{
                if (!modLibrary.IsExistForm("frmVirtualSeqComm"))	//追加2015/01/30hata_if文追加
                {
#if DebugOn
                    //frmVirtualSeqComm.Instance.Show(this); 
                    modSeqComm.MySeq.Show();//Rev26.00 change by chouno 2017/01/18
#endif
                }
                else
                {
#if DebugOn
                    //Rev26.00 change by chouno 2017/01/18
                    modSeqComm.MySeq.WindowState = FormWindowState.Normal;
                    modSeqComm.MySeq.Visible = true;
                    //frmVirtualSeqComm.Instance.WindowState = FormWindowState.Normal;
                    //frmVirtualSeqComm.Instance.Visible = true;
#endif
                }

			}

			//ErrMessage関数のテスト
			else if (sender == mnuDebugSub3)
			{
				string strWork = frmInputBox.InputBox(CTResources.LoadResString(20013),CTResources.LoadResString(20014));

				double d = 0;
				if (double.TryParse(strWork, out d))
				{
					modCT30K.ErrMessage((int)d);
				}
			}

			//タイマーチェック
			else if (sender == mnuDebugSub4)
			{
                if (!modLibrary.IsExistForm("frmCheckTimer"))	//追加2015/01/30hata_if文追加
                {
                    frmCheckTimer.Instance.Show(this);
                }
                else
                {
                    frmCheckTimer.Instance.WindowState = FormWindowState.Normal;
                    frmCheckTimer.Instance.Visible = true;
                }
            }
#endif
		}

		//********************************************************************************
		//機    能  ：  ロードされているフォームの一覧を表示する
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V4.0   01/03/01  (SI1)鈴山       新規作成
		//********************************************************************************
		private void DispForms()
		{

			const short MAX_LENGTH = 30;

			string msgStr = null;

			msgStr = "";

			foreach (Form theForm in Application.OpenForms)
			{
				//フォームのオブジェクト名（インデントを揃える）・キャプションをセット
				msgStr = msgStr + string.Format("{0}{1}", theForm.Name, new string(' ', MAX_LENGTH)).Substring(0, MAX_LENGTH) + "\t" + theForm.Text + "\r";
			}

			//表示
			MessageBox.Show(msgStr, this.Text + " - " + mnuDebugSub0.Text, MessageBoxButtons.OK);
		}

        #region フォームを子フォームとして指定された位置に表示する
        /// <summary>
        /// フォームを子フォームとして標準位置に表示する
        /// </summary>
        /// <param name="form"></param>
        public void ShowChild(Form form)
        {
            // オーナーフォームの設定
            form.Owner = this;

            // スキャン制御画面のスクリーン上での領域を求める
            var p1 = new Point(frmScanControl.Instance.Width + Toolbar1.Width, frmScanControl.Instance.Bounds.Top);
            var p2 = frmCTMenu.Instance.PointToScreen(p1);

            int left = Math.Max(p2.X - form.Width, 0);
            int top = Math.Max(p2.Y - form.Height, 0);
            form.Location = new Point(left, top);
            form.Show();
        }

        public Point GetNormalFormLocation(Form form)
        {
            // スキャン制御画面のスクリーン上での領域を求める
            var p1 = new Point(frmScanControl.Instance.Width + Toolbar1.Width, frmScanControl.Instance.Bounds.Top);
            var p2 = frmCTMenu.Instance.PointToScreen(p1);

            int left = Math.Max(p2.X - form.Width, 0);
            int top = Math.Max(p2.Y - form.Height, 0);

            return new Point(left, top);
        }
        
        /// <summary>
        /// フォームを子フォームとして指定された位置に表示する
        /// </summary>
        /// <param name="form"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        public void ShowChild(Form form, int left, int top)
        {
            form.MdiParent = this;
            form.Location = new Point(left, top);
            form.Show();
            //form.Hide();

        }
        #endregion

        //Toolbarが反応しないための処理　//2014/07/08(検S1)hata
        private void Toolbar1_MouseEnter(object sender, EventArgs e)
        {
            //変更2015/01/09hata_
            //削除2014/11/28hata_v19.51_dnet
            //他の方法の変更
            //if (this.Visible && this.Enabled) 
            //    this.Activate();   
         
            if (Toolbar1.Visible && Toolbar1.Enabled)
                Toolbar1.Focus();
        }


        //追加2014/09/11(検S1)hata
        /// <summary>
        /// WndProc メソッドをオーバーライドする：ダブルオブリークからのメッセージ処理する
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == modDoubleOblique.WM_DOUBLEOBLIQUE && modDoubleOblique.WM_DOUBLEOBLIQUE != 0)
            {
                //ダブルオブリークからのメッセージ処理
                modDoubleOblique. DoubleObliqueWndProc(ref m);
            }

            //基本クラスのメソッドを実行する
            base.WndProc(ref m);
        }

    }
}
