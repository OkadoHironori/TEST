using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{

    public partial class frmRoiTool : Form
    {
        private RadioButton[] optDPixel = new RadioButton[2];       //ラジオボタンコントロール配列
        private NumericUpDown[] cwneDPixel = new NumericUpDown[2];  //数値入力コントロール配列

        #region インスタンスを返すプロパティ

        // frmRoiToolのインスタンス
        private static frmRoiTool _Instance = null;

        /// <summary>
        /// frmRoiToolのインスタンスを返す
        /// </summary>
        public static frmRoiTool Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmRoiTool();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmRoiTool()
        {
            InitializeComponent();

            #region フォームにコントロールを追加

            this.SuspendLayout();

            #region ラジオボタン

            for (int i = 0; i < optDPixel.Length; i++)
            {
                this.optDPixel[i] = new RadioButton();
                this.optDPixel[i].AutoSize = true;
                this.optDPixel[i].Location = new Point(12, 168 + i * 24);
                this.optDPixel[i].Name = "optDPixel" + i.ToString();
                this.optDPixel[i].Size = new Size(96, 16);
                this.optDPixel[i].TabIndex = i+1;
                switch (i)
                {
                    case 0:
                        this.optDPixel[i].Text = "ずらし量指定：";
                        break;
                    case 1:
                        this.optDPixel[i].Text = "視差角指定　：";
                        this.optDPixel[i].Checked = true;
                        break;
                    default:
                        break;
                }
                this.optDPixel[i].UseVisualStyleBackColor = true;
                this.Controls.Add(this.optDPixel[i]);
            }

            #endregion

            #region 数値入力

            for (int i = 0; i < cwneDPixel.Length; i++)
            {
                this.cwneDPixel[i] = new NumericUpDown();
                this.cwneDPixel[i].Location = new Point(112, 167);
                this.cwneDPixel[i].Name = "cwneDPixel" + i.ToString();
                this.cwneDPixel[i].Size = new Size(59, 19);
                this.cwneDPixel[i].TabIndex = i;
                this.cwneDPixel[i].TextAlign = HorizontalAlignment.Right;
                switch (i)
                {
                    case 0:
                        this.cwneDPixel[i].Location = new Point(112, 167);
                        this.cwneDPixel[i].Maximum = new decimal(new int[] { 500, 0, 0, 0 });
                        this.cwneDPixel[i].Minimum = new decimal(new int[] { 1, 0, 0, 0 });
                        this.cwneDPixel[i].Value = new decimal(new int[] { 100, 0, 0, 0 });
                        break;
                    case 1:
                        this.cwneDPixel[i].Increment = new decimal(new int[] { 1, 0, 0, 65536 });
                        this.cwneDPixel[i].Location = new Point(112, 190);
                        this.cwneDPixel[i].Maximum = new decimal(new int[] { 10, 0, 0, 0 });
                        this.cwneDPixel[i].Value = new decimal(new int[] { 10, 0, 0, 65536 });
                        break;
                    default:
                        break;
                }
                this.Controls.Add(this.cwneDPixel[i]);
            }

            #endregion

            this.ResumeLayout(false);

            #endregion
        }

        //*****************************************************************************************
        //機　　能： フォームロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*****************************************************************************************
		private void frmRoiTool_Load(object sender, EventArgs e)
		{
			//int Result = 0;

			//v17.60 ストリングテーブル化 by長野 2011/05/25
			StringTable.LoadResStrings(this);

			//v17.60 英語用レイアウト調整
			if (modCT30K.IsEnglish)
            {
				EnglishAdjustLayout();
			}

			//X線ON v17.00 by 山影 10-03-04
			//If (frmXrayControl.MecaXrayOn <> OnStatus) And (DetType <> DetTypePke) Then
			//v17.02変更 PkeでもONする byやまおか 2010/07/15
			if ((frmXrayControl.Instance.MecaXrayOn != modCT30K.OnOffStatusConstants.OnStatus))
            {
				//ライブON
				frmTransImage.Instance.CaptureOn = true;
			}

			//進捗表示をクリアする   'v17.10追加 byやまおか
			lblProcess.Text = "";
            
			//表示位置
            //2014/11/07hata キャストの修正
            //this.SetBounds(frmTransImage.Instance.Left + frmTransImage.Instance.Width - this.Width, (315 / 15), 0, 0, 
            //               BoundsSpecified.X | BoundsSpecified.Y);
            this.SetBounds(frmTransImage.Instance.Left + frmTransImage.Instance.Width - this.Width, 21, 0, 0,
                           BoundsSpecified.X | BoundsSpecified.Y);

			//ツールバー上のボタンのToolTipText
            this.tsbtnRectangle.ToolTipText = CTResources.LoadResString(StringTable.IDS_RoiRect);
            
            //長方形
			//.Buttons("Cut").ToolTipText = GetResString(IDS_Cut, "ROI")      'ROIの切り取り
			//.Buttons("Copy").ToolTipText = GetResString(IDS_Copy, "ROI")    'ROIのコピー
			//.Buttons("Paste").ToolTipText = GetResString(IDS_Paste, "ROI")  'ROIの貼り付け
			//.Buttons("Comment").ToolTipText = LoadResString(IDS_Comment)    'コメント
            this.tsbtnExit.ToolTipText = CTResources.LoadResString(StringTable.IDS_Return);  //戻る
            //変更2015/02/02hata_Max/Min範囲のチェック
			//cwneInteg.Value = 50;               //積算回数
            //cwneDPixel[0].Value = 100;          //ずらし量(pixel)
            //cwneDPixel[1].Value = (decimal)1.0; //視差角(°)
            cwneInteg.Value = modLibrary.CorrectInRange(50, cwneInteg.Minimum, cwneInteg.Maximum);               //積算回数
            cwneDPixel[0].Value = modLibrary.CorrectInRange(100, cwneDPixel[0].Minimum, cwneDPixel[0].Maximum);          //ずらし量(pixel)
            cwneDPixel[1].Value = modLibrary.CorrectInRange((decimal)1.0, cwneDPixel[1].Minimum, cwneDPixel[1].Maximum); //視差角(°)

            //デバッグモードの時だけ表示する
#if (!DebugOn)


            //ずらし量
			for (int i = optDPixel.GetLowerBound(0); i <= optDPixel.GetUpperBound(0); i++)
            {
				optDPixel[i].Visible = false;
				cwneDPixel[i].Visible = false;
			}
			//積算枚数
			lblInteg.Visible = false;
			cwneInteg.Visible = false;

            ////フォーム高さ調整
            //this.Height = this.Height - optDPixel[0].Height + optDPixel[1].Height + cwneInteg.Height;
            //Rev20.00 微調整 by長野 2015/02/06
            this.Height = this.Height - (int)((float)optDPixel[0].Height * 1.4f) - (int)((float)optDPixel[1].Height * 1.4f) - (int)((float)cwneInteg.Height * 1.4f);
            //this.Height = 185;

#endif

            //追加2014/11/28hata_v19.51_dnet
            //if (Toolbar1.Visible && Toolbar1.Enabled)
            //    Toolbar1.Focus();

		}

        //*****************************************************************************************
        //機　　能： フォームアンロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*****************************************************************************************
		private void frmRoiTool_FormClosed(object sender, FormClosedEventArgs e)
		{
			//ROI制御終了
			if (modLibrary.IsExistForm(frmTransImage.Instance))
            {
				if (frmTransImage.Instance.TransImageProc != frmTransImage.TransImageProcType.TransRoiNone)
                {
					frmTransImage.Instance.TransImageProc = frmTransImage.TransImageProcType.TransRoiNone;
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
        private void Toolbar1_ButtonClick(object sender, ToolStripItemClickedEventArgs e)
		{
            ToolStripButton Button = e.ClickedItem as ToolStripButton;
			string FileName = "";
            string theComment = null;

			//ツールバーの処理実行
			switch (Button.Name)
            {
				//矩形ROI制御
                case "tsbtnRectangle":
					DrawRoi.roi.ModeToPaint(RoiData.RoiShape.ROI_RECT);
					break;

                case "tsbtnCut":
					//ROIの切り取り
					DrawRoi.roi.CurrentRoiCut();
					break;

                case "tsbtnCopy":
					//ROIのコピー
					DrawRoi.roi.DataToClipBoard();
					break;

                case "tsbtnPaste":
					//ROIの貼り付け
					DrawRoi.roi.RoiPaste();
					break;

                case "tsbtnOpen":
					//オープンダイアログ処理
					FileName = modFileIO.GetFileName(Operation : StringTable.IDS_Open, 
                                                     Description : Button.Text, 
                                                     InitFileName : "-ROI",
                                                     Purpose : DrawRoi.roi.Table);
                    if (!string.IsNullOrEmpty(FileName))
                    {
						if (DrawRoi.LoadRoiTable(FileName))
                        {
							DrawRoi.roi.Table = FileName;
                        }
						//ROIテーブル読み込み
					}
					break;

                case "tsbtnSave":
					//保存ダイアログ処理
					FileName = modFileIO.GetFileName(Operation : StringTable.IDS_Save,
                                                     Description : Button.Text,
                                                     InitFileName : "-ROI",
                                                     Purpose : DrawRoi.roi.Table);
					if (!string.IsNullOrEmpty(FileName))
                    {
						if (DrawRoi.SaveRoiTable(FileName))
                        {
							DrawRoi.roi.Table = FileName;   //ROIテーブル保存
                        }
					}
					break;

                case "tsbtnComment":
					//コメント入力ダイアログ表示
					theComment = frmComment.Dialog(StringTable.GetResString(9917, CTResources.LoadResString(StringTable.IDS_Comment)), 
                                                            StringTable.GetResString(StringTable.IDS_InputCommentOf, this.tsbtnGo.Text),
                                                            (DrawRoi.roi.Comment));
					if (theComment != "\0")
                    {
						DrawRoi.roi.Comment = theComment;
                    }
					break;

                case "tsbtnGo":
					GoRoi();
					break;

                case "tsbtnExit":
					//ROI制御終了
					frmTransImage.Instance.TransImageProc = frmTransImage.TransImageProcType.TransRoiNone;
					break;

                default:
                    break;
			}
		}

        //Toolbarが反応しないための処理　//2014/09/18(検S1)hata
        private void Toolbar1_MouseEnter(object sender, EventArgs e)
        {
            //削除2014/11/28hata_v19.51_dnet
            //他の方法の変更
            //if (this.Visible && this.Enabled)
            //    this.Activate();

            //追加2015/01/26hata
            if (Toolbar1.Visible && Toolbar1.Enabled)
                Toolbar1.Focus();


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
		private void GoRoi()
		{
			//測定するROIが登録されているか？
			if (DrawRoi.roi.NumOfRois < 1)
            {
				//メッセージ表示：ROIが設定されていません。
                MessageBox.Show(CTResources.LoadResString(StringTable.IDS_NotFoundROI), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
			}

			//座標入力フォームをアンロード
			frmInputRoiData.Instance.Close();

			//自動スキャン位置移動処理
			DoAutoPos();
		}

        //*******************************************************************************
        //機　　能： 透視上ROI指定自動スキャン位置移動処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V15.00  09/07/08   YAMAKAGE      新規作成
        //*******************************************************************************
		private void DoAutoPos()
		{
			//int err_sts = 0;
			bool IsOK = false;
			IsOK = false;

			modAutoPos.ParacllaticAngleOn = optDPixel[1].Checked;   //v16.2(CT30K統合) 追加 by 山影 10-04-01

			//ツールバー上のボタンを触れないようにする
			RoitoolEnabled(false);
			//v16.2(CT30K統合) 変更 by 山影 10-04-01
			//    ToolBarEnabled = False
			//    cwneInteg.Enabled = False
			//    cwneDPixel.Enabled = False
			//    frmTransImage.Enabled = False
			//    Me.Enabled = False

			//マウスポインタを砂時計にする
			Cursor.Current = Cursors.WaitCursor;    //v16.30追加 byやまおか 2010/05/21

			//コモン更新(I.I.視野、FCD、FID)
			modAutoPos.UpdateForAutoPos();

			ushort[] beforeImage = null; //透視画像用配列(移動前)
			ushort[] afterImage = null;  //透視画像用配列(移動後)

            //2014/11/07hata キャストの修正
            //int IntegNum = (int)cwneInteg.Value;//積算枚数
            int IntegNum = Convert.ToInt32(cwneInteg.Value);//積算枚数
           
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
                //2014/11/07hata キャストの修正
                //IntegNum = Convert.ToInt16(IntegNum * frmTransImage.Instance.GetCurrentFR() / CTSettings.detectorParam.FR[0]);
                IntegNum = Convert.ToInt32(IntegNum * frmTransImage.Instance.GetCurrentFR() / CTSettings.detectorParam.FR[0]);
            }   //v17.10追加 byやまおか 2010/08/20
			
			short dPixel = 0;   //ずらし量(pixel)    'v16.2(CT30K統合) 追加 by 山影 10-04-01
            //2014/11/07hata キャストの修正
            //dPixel = (short)cwneDPixel[0].Value;
            dPixel = Convert.ToInt16(cwneDPixel[0].Value);

			double fai = 0;
			//視差角φ(rad)
			fai = (double)cwneDPixel[1].Value * ScanCorrect.Pai / 180;

			if (modAutoPos.ParacllaticAngleOn)
            {
                //2014/11/07hata キャストの修正
                //dPixel = (short)(CTSettings.scansel.Data.fid * Math.Tan(fai) / (10 / CTSettings.scancondpar.Data.b[1]));
                dPixel = Convert.ToInt16(CTSettings.scansel.Data.fid * Math.Tan(fai) / (10F / CTSettings.scancondpar.Data.b[1]));
            }

			float dRefPix = 0;//テーブル移動量を反映させたずらし量(pixel)

			//画像配列領域確保
			beforeImage = new ushort[modScanCorrectNew.TransImage.GetUpperBound(0) + 1];
			afterImage = new ushort[modScanCorrectNew.TransImage.GetUpperBound(0) + 1];

			//透視画面左右反転
			//IsLRInverse = (DetType = DetTypePke)   'v17.50削除 modGlobalへ移動 byやまおか 2011/02/02

			//矩形ROI座標取得用
            int roiXC = 0;
            int roiYC = 0;
			int roiXL = 0;
			int roiYL = 0;

			//ROI座標取得
			DrawRoi.roi.GetRectangleShape2(1, ref roiXC, ref roiYC, ref roiXL, ref roiYL);

			//ROI位置に応じて微少移動方向を変更
            //2014/11/07hata キャストの修正
            //if (roiXC > frmTransImage.Instance.ctlTransImage.Width / 2)
            if (roiXC > Convert.ToInt32(frmTransImage.Instance.ctlTransImage.Width / 2F))
            {
                dPixel = (short)(-dPixel);
            }

			//左右反転している場合はX座標を入れ替える    'v17.5追加 byやまおか 2011/02/27
            if (CTSettings.detectorParam.IsLRInverse)
            {
				roiXC = (short)(frmTransImage.Instance.ctlTransImage.Width - roiXC);
				dPixel = (short)(-dPixel);
			}

			//進捗表示   'v17.10追加 byやまおか 2010/08/20
			//lblProcess.Caption = "データ収集中..."
			lblProcess.Text = CTResources.LoadResString(12195);   //ストリングテーブル化 'v17.60 by 長野 2011/05/22

			//前処理 'キャプチャ(移動前)～移動～キャプチャ(移動後)～元の位置へ
			//    IsOK = AutoPos_PreProc(IntegNum, dPixel, beforeImage, afterImage, pgbBefore, pgbAfter, dRefPix, fai)    'v16.2(CT30K統合) 引数(fai)追加 by 山影 10-04-01
			IsOK = modAutoPos.AutoPos_PreProc(IntegNum, dPixel, 
                                              ref beforeImage, 
                                              ref afterImage, 
                                              pgbBefore, pgbAfter, ref dRefPix);
			if (!IsOK)
            {
				goto ErrorHandler;
			}

			//進捗表示   'v17.10追加 byやまおか 2010/08/20
			//lblProcess.Caption = "テーブル位置 計算中..."
			lblProcess.Text = CTResources.LoadResString(20060);   //ストリングテーブル化 'v17.60 by長野 2011/05/22

            //追加2014/09/20(検S1)hata
            Application.DoEvents();

			//タイマーが実行できるようにイベントを取る
			modCT30K.PauseForDoEvents(1.0f);

			//自動テーブル移動
			IsOK = frmMechaMove.Instance.MechaMoveForAutoScanPos_Trans(roiXC, 
                                                                       roiYC, 
                                                                       roiXL, 
                                                                       roiYL, 
                                                                       dPixel, 
                                                                       dRefPix, 
                                                                       ref beforeImage, 
                                                                       ref afterImage);
            frmMechaMove.Instance.Dispose();

			//マウスポインタを元に戻す
			Cursor.Current = Cursors.Default;

//ExitHandler:
			//v16.30追加 byやまおか 2010/05/21

			//ErrorHandler:
			//v17.10変更 byやまおか 2010/08/09

			//画像配列領域解放
			beforeImage = null;
			afterImage = null;

			//ツールバー上のボタンを触れるようにする
			RoitoolEnabled((true));
			//v16.2(CT30K統合) 変更 by 山影 10-04-01
			//    ToolBarEnabled = True
			//    cwneInteg.Enabled = True
			//    cwneDPixel.Enabled = True
			//    frmTransImage.Enabled = True
			//    Me.Enabled = True

			if (IsOK)
            {
				//アンロード
				this.Close();
			}
            else
            {
                if (!modLibrary.IsExistForm(frmTransImage.Instance))
                {
                    this.Show(frmTransImage.Instance);
                }
                else
                {
                    this.Visible = true;
                }

				pgbBefore.Value = 0;
				pgbAfter.Value = 0;
			}

			return;

ErrorHandler:
			//画像配列領域解放
			beforeImage = null;
			afterImage = null;

			//アンロード
			this.Close();
		}

        /// <summary>
        /// 画面上のコントロール有効／無効 切り替え
        /// </summary>
        /// <param name="Value">true=有効, false=無効</param>
		private void RoitoolEnabled(bool Value)
		{
			ToolBarEnabled = Value;
			cwneInteg.Enabled = Value;
			optDPixel[0].Enabled = Value;
			optDPixel[1].Enabled = Value;
			cwneDPixel[0].Enabled = Value;
			cwneDPixel[1].Enabled = Value;
			frmTransImage.Instance.Enabled = Value;
			lblInteg.Enabled = Value;
			this.Enabled = Value;
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
				//Toolbar1.Enabled = value;

				if (value)
                {
                    //変更2014/07/25(検S1)hata
                    //foreach (ToolStripButton Button in Toolbar1.Items)
                    //{
                    //    Button.Enabled = Convert.ToBoolean(Button.Tag);
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
                    //    Button.Tag = Button.Enabled;
                    //    Button.Enabled = false;
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
                Toolbar1.Enabled = value;

            }
		}

        //*****************************************************************************************
        //機　　能： 英語用レイアウト調整
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.60 2011/05/25 (検S1)長野　  新規作成
        //*****************************************************************************************
		private void EnglishAdjustLayout()
		{
            //Rev20.01 コメントアウトする by長野 2015/05/19
            ////2014/11/07hata キャストの修正
            ////this.Height = (int)(this.Height * 1.1);
            ////lblProcess.Height = (int)(lblProcess.Height * 1.3);
            //this.Height = Convert.ToInt32(this.Height * 1.1);
            //lblProcess.Height = Convert.ToInt32(lblProcess.Height * 1.3);

            //for (int i = optDPixel.GetLowerBound(0); i <= optDPixel.GetUpperBound(0); i++)
            //{
            //    optDPixel[i].Top = lblProcess.Top + lblProcess.Height + (i - 1) * optDPixel[i].Height;
            //    cwneDPixel[i].Top = lblProcess.Top + lblProcess.Height + (i - 1) * cwneDPixel[i].Height;
            //}

            ////積算枚数
            //lblInteg.Top = optDPixel[optDPixel.GetUpperBound(0)].Top + optDPixel[optDPixel.GetUpperBound(0)].Height;
            //cwneInteg.Top = optDPixel[optDPixel.GetUpperBound(0)].Top + optDPixel[optDPixel.GetUpperBound(0)].Height;
        }

    }
}
