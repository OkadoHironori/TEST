using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.IO;

using System.Drawing.Drawing2D; //Rev26.00 add by chouno 2016/12/28
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
	/* プログラム名： frmScanControl.frm                                    　　　*/
	/* 処理概要　　： スキャンコントロール                                        */
	/* 注意事項　　： なし                                                        */
	/* -------------------------------------------------------------------------- */
	/* 適用計算機　： DOS/V PC                                                    */
	/* ＯＳ　　　　： Windows XP  (SP2)                                           */
	/* コンパイラ　： VB 6.0                                                      */
	/* -------------------------------------------------------------------------- */
	/* VERSION     DATE        BY                  CHANGE/COMMENT                 */
	/*                                                                            */
	/* v15.0       09/04/01    (SI1)    間々田     リニューアル    　　　　       */
	/* v19.00      12/02/21    H.Nagai             BHC対応
	/*                                                                            */
	/* -------------------------------------------------------------------------- */
	/* ご注意：                                                                   */
	/* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	/*                                                                            */
	/* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2009                 */
	/* ************************************************************************** */
    public partial class frmScanControl : FixedForm
    //public partial class frmScanControl : Form
    {
        //画質定数
		public enum ScanQualityConstants
		{
			ScanQualityManual,			//マニュアル
			ScanQualityQuick,			//Quick
			ScanQualityRough,			//Rough
			ScanQualityNormal,			//Normal
			ScanQualityFine,			//Fine
			ScanQualityPreset			//プリセット
		}

		private bool byEvent;
        private bool bySetup = false;

        //画像処理タブ内コマンドボタン：インデクスの定義
		private int ImageProcAdd;		//和画像
		private int ImageProcSub;		//差画像
		private int ImageProcEnlarge;	//単純拡大
		private int ImageProcZooming;	//ズーミング
		private int ImageProcRoi;		//ROI処理
		private int ImageProcProfile;	//プロフィール	
		private int ImageProcDist;		//寸法測定		
		private int ImageProcPD;	    //プロフィールディスタンス
		private int ImageProcHist;		//ヒストグラム
		private int ImageProcCTDump;	//ＣＴ値表示	
		private int ImageProcColor;		//疑似カラー		
		private int ImageProcMulti;	    //マルチフレーム		
		private int ImageProcInfo;	    //付帯情報修正		
		private int ImageProcFilter;	//フィルタ処理		
		private int ImageProcBone;	    //骨塩定量測定	
		private int ImageProcFormat;	//フォーマット変換		
		public int ImageProcRoiAngle;   //角度測定   'v16.01/v17.00追加 byやまおか 2010/02/24

        private char presskye = (char)0;            //Keypressの値 //追加2014/12/19hata

        //フォーム参照用
        private frmMechaControl myMechaControl;     //メカ制御
        private frmTransImage myTransImage;         //透視画像
        private frmFInteg myIntegForm;              //画像積算
        private frmFEdge myEdgeForm;                //エッジ強調
        private frmFDiff myDiffForm;                //微分
        private frmSaveMovie myMovieForm;           //動画保存
        private frmProfGrph myProfGraph;            //プロフィールグラフ 'v17.50追加 by 間々田 2011/02/02
        private frmScanCondition myScanCondition;   //スキャン条件   'v18.00追加 byやまおか 2011/02/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05 //追加2014/10/07hata_v19.51反映
        private frmScanoCondition myScanoCondition; //スキャノ条件   'Rev21.00追加 by長野 2015/02/19

		private Label[] lblColon = null;
		public Label[] lblStatus = null;
//		private GroupBox[] fraScanCondition = null;
        private Panel[] fraScanCondition = null;

        
        public RadioButton[] optQuality = null;
        public RadioButton[] optDataMode = null;
		private Button[] cmdCorrect = null;
		public CheckBox[] chkInhibit = null;
        public RadioButton[] optScale = null;
        public List<Button> cmdImageProc = null;
        public RadioButton[] optScanMode = null;    //追加2014/10/07hata_v19.51反映

        //Rev26.00 add by chouno 2016/12/26 --->
        public RadioButton[] optScanArea = null;
        public RadioButton[] optScanMat = null;
        public RadioButton[] optScanCond = null;
        public Label[] lblScanArea = null;
        private Image scanCondImg0 = null;
        private Image scanCondImg1 = null;
        private Image scanCondImg2 = null;
        private Image scanCondImg3 = null;
        private Image scanCondImg4 = null;
        //<---

        //追加2015/01/27hata
        private TabPageCtrl tabPage;

		private static frmScanControl _Instance = null;

        //Rev20.00 追加 by長野 2015/02/09
        //optScaleをラジオボタンのクリックで変更してもWW,WLのtextボックスの内容を保持しておくための措置
        private bool WLWWEvntLock = false;
        private bool ResetWLWWProcessingFlg = false;


        //Rev26.00 add by chouno 2016/12/28 --->
        private Pen scanMatAxisPen = null;

        //Rev26.00 add by chouno 2016/12/28 --->
        private bool myScanAreaSetCmpFlg = false; //[ガイド]タブのスキャンエリア設定後に立てるフラグ、メカが移動したら落とす
        public bool scanAreaSetCmpFlg
        {
            get
            {
                return myScanAreaSetCmpFlg;
            }
            set
            {
                if (myScanAreaChangedIgnore == true)
                {
                    return;
                }
                myScanAreaSetCmpFlg = value;
                if (value == false)
                {
                    foreach (RadioButton rdb in optScanArea)
                    {
                        rdb.Checked = false;
                    }
                    foreach (RadioButton rdb in optScanMat)
                    {
                        rdb.Checked = false;
                    }
                }
            }
        }
        private bool myScanAreaChangedIgnore = false; //校正中などのやむを得ない事情によるメカ移動に、optScanAreaをfalseにさせないため
        public bool scanAreaChangedIgnore
        {
            get
            {
                return myScanAreaChangedIgnore;
            }
            set
            {
                myScanAreaChangedIgnore = value;
            }
        }
        private bool myScanCondImgSetCmpFlg = false; //[条件]タブの画像からの復元設定後に立てるフラグ、メカが移動したら落とす
        public bool scanCondImgSetCmpFlg
        {
            get
            {
                return myScanCondImgSetCmpFlg;
            }
            set
            {
                if (myScanCondChangeIgnore == true)
                {
                    return;
                }
                myScanCondImgSetCmpFlg = value;
                if (value == false)
                {
                    txtPresetImgFile.Text = "";
                }
            }
        }
        private bool myScanCondSetCmpFlg = false; //[ガイド]タブのスキャン条件設定後に立てるフラグ、スキャン条件詳細画面で条件を設定したらorメカが移動したら落とす
        public bool scanCondSetCmpFlg
        {
            get
            {
                return myScanCondSetCmpFlg;
            }
            set
            {
                if (myScanCondChangeIgnore == true)
                {
                    return;
                }
                myScanCondSetCmpFlg = value;
                if (value == false)
                {
                    foreach (RadioButton rdb in optScanCond)
                    {
                        rdb.Checked = false;
                    }
                }
            }
        }
        private bool myScanCondChangeIgnore = false;//校正中などのやむを得ない事情によるメカ移動に、optScanCondをfalseにさせないため
        public bool scanCondChangeIgnore
        {
            get
            {
                return myScanCondChangeIgnore;
            }
            set
            {
                myScanCondChangeIgnore = value;
            }
        }
        //<---
        //Rev26.00 スキャン開始前に未完了の校正を実施するフラグ
        private bool myAutoCorBeforeScanFlg = false;
        public bool autoCorBeforeScanFlg
        {
            get
            {
                return myAutoCorBeforeScanFlg;
            }
            set
            {
                myAutoCorBeforeScanFlg = value;
            }
        }

        //Rev26.00 add by chouno 2017/10/31
        private bool bXrayAuto = false;
        public bool XrayAuto
        {
            get
            {
                return bXrayAuto;
            }
            set
            {
                bXrayAuto = value;
            }
        }

		/// <summary>
		/// 
		/// </summary>
        public frmScanControl()
        {
            InitializeComponent();

			lblColon = new Label[] {lblColon0, lblColon1, lblColon2, lblColon3, lblColon4, lblColon5};

			lblStatus = new Label[] {lblStatus0, lblStatus1, lblStatus2, lblStatus3, lblStatus4, lblStatus5};

            //fraScanCondition = new Panel[] { fraScanCondition0, fraScanCondition1, fraScanCondition2, fraScanCondition3, fraScanCondition4 };
            //Rev26.00 add by chouno 2016/12/27
            fraScanCondition = new Panel[] { fraScanCondition0, fraScanCondition5,fraScanCondition1, fraScanCondition2, fraScanCondition3, fraScanCondition4};

            //Rev.26.10 Zの場合 by井上 2017/12/18
            if (CTSettings.scaninh.Data.guide_mode == 1)
            {
                optQuality = new RadioButton[] { optQuality0, optQuality1, optQuality2, optQuality3, optQuality4, optQuality5 };
            }

            //optDataMode = new RadioButton[] { null, optDataMode1, null, null, optDataMode4 };
            //Rev21.00 スキャノ追加 by長野 2015/02/19
            optDataMode = new RadioButton[] { null, optDataMode1, null, optDataMode3, optDataMode4 };
            
            cmdCorrect = new Button[] {null, null, null, null, null, null, null, null, cmdCorrect8 };

			chkInhibit = new CheckBox[] {null, chkInhibit1, null, chkInhibit3, null, chkInhibit5};

			optScale = new RadioButton[] {optScale0, optScale1, optScale2};

            optScanMode = new RadioButton[] { null, optScanMode1, optScanMode2, optScanMode3, optScanMode4 };//追加2014/10/07hata_v19.51反映

			cmdImageProc = new List<Button>();
			cmdImageProc.Add(cmdImageProc1);

            optScanArea = new RadioButton[] { optScanArea0, optScanArea1 ,optScanArea2, optScanArea3, optScanArea4 }; //Rev26.00 add by chouno 2016/12/26
            optScanCond = new RadioButton[]{optScanCond0,optScanCond1,optScanCond2,optScanCond3,optScanCond4};//Rev26.00 add by chouno 2016/12/26

            optScanMat = new RadioButton[]{optScanMat_A_1,optScanMat_A_2,optScanMat_A_3,optScanMat_A_4,optScanMat_B_1,optScanMat_B_2,optScanMat_B_3,optScanMat_B_4,
                                           optScanMat_C_1,optScanMat_C_2,optScanMat_C_3,optScanMat_C_4,optScanMat_D_1,optScanMat_D_2,optScanMat_D_3,optScanMat_D_4};//Rev26.00 add by chouno 2016/12/26


            scanMatAxisPen = new Pen(Color.CornflowerBlue, 4);    //Rev26.00 add by chouno 2016/12/28
            scanMatAxisPen.EndCap = LineCap.ArrowAnchor; //Rev26.00 add by chouno 2016/12/28

            //追加2015/01/27hata
            //TaPageの表示／非表示切り替えのため、TabPageCtrlオブジェクトを作成
            tabPage = new TabPageCtrl(this.SSTab1);

        }

		/// <summary>
		/// 
		/// </summary>
		public static frmScanControl Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmScanControl();

                }

				return _Instance;
			}
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
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
		private void frmScanControl_Load(object sender, EventArgs e)
		{
            //追加2014/09/11_dNet対応_hata
            bySetup = false;

            //'フォームの表示位置：Ｘ線制御画面の真下    'v15.0削除 InitControlsで処理する byやまおか 2009/07/29
			//With frmXrayControl
			//    Me.Move .Left, .Top + .Height, FmControlWidth
			//End With

			//キャプションのセット
			SetCaption();

			//英語版レイアウト調整 'v17.60 by 長野 2011/05/23
			if (modCT30K.IsEnglish == true)
            {
				EnglishLayout();
			}
 			//コントロールの初期設定
			InitControls();

            if (CTSettings.scaninh.Data.mechacontrol == 0) 
            {
				//校正詳細画面のロード
				if (CTSettings.scaninh.Data.cor_status == 0) 
                {
                    //Load関数がないので次で対応(ShowInTaskbar = false / WindowState = FormWindowState.Minimized)
                    //HideをするとVisivleがFalseになってしまう。
                    frmCorrectionStatus.Instance.ShowInTaskbar = false;
                    frmCorrectionStatus.Instance.WindowState = FormWindowState.Minimized;
                    if (!modLibrary.IsExistForm("frmCorrectionStatus"))	//追加2015/01/30hata_if文追加
                        frmCorrectionStatus.Instance.Show();
 
                    //frmCorrectionStatus.Instance.ShowDialog(frmCTMenu.Instance);
                    //frmCorrectionStatus.Instance.ShowDialog();
                }
				//フォームの参照
				myMechaControl = frmMechaControl.Instance;	//メカ制御
				myMechaControl.MecainfChanged += myMechaControl_MecainfChanged;

                myMechaControl.FCDChanged += myMechaControl_SeqAxisChanged;
                myMechaControl.FIDChanged += myMechaControl_SeqAxisChanged;
                myMechaControl.YChanged += myMechaControl_SeqAxisChanged;

                myTransImage = frmTransImage.Instance;		//透視画像
                //キャプチャーOnOffイベント
                myTransImage.CaptureOnOffChanged += new frmTransImage.CaptureOnOffChangedEventHandler(myTransImage_CaptureOnOffChanged);

                //追加2014/10/07hata_v19.51反映
                myScanCondition = frmScanCondition.Instance;      //スキャン条件   'v18.00追加 byやまおか 2011/02/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05


                ////積分のClosedイベント
                //frmFInteg.Instance.Unloaded += new frmFInteg.UnloadedEventHandler(myIntegForm_Unloaded);

                ////エッジ強調のClosedイベント
                //frmFEdge.Instance.Unloaded += new frmFEdge.UnloadedEventHandler(myEdgeForm_Unloaded);

            }

			//スキャン条件タブ内の設定値のロード
			//LoadScanCondition
			if (CTSettings.scaninh.Data.mechacontrol == 0) LoadScanCondition();			//v15.0変更 byやまおか 2009/07/30

            //追加2014/09/11_dNet対応_hata
            bySetup = true;
 
        }


        //*************************************************************************************************
        //機　　能： フォームアンロード時処理（イベント処理）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Cancel          [ /O] Integer   True（0以外）: アンロードをキャンセル
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void frmScanControl_FormClosed(object sender, FormClosedEventArgs e)
		{
			//フォームの参照破棄
			if (CTSettings.scaninh.Data.mechacontrol == 0) 
            {
                //メカ制御画面への参照破棄
                if (modLibrary.IsExistForm(myMechaControl))
                {
                    myMechaControl.MecainfChanged -= myMechaControl_MecainfChanged;
                    myMechaControl = null;		//メカ制御
                }

                //透視画面への参照破棄
                if (modLibrary.IsExistForm(myTransImage))
                {
                    myTransImage = null;
                }

                //追加2014/10/07hata_v19.51反映
                //スキャン条件   'v18.00追加 byやまおか 2011/02/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                //透視画面への参照破棄
                if (modLibrary.IsExistForm(myScanCondition))
                {
                    myScanCondition = null;
                }

            }

			//スキャン条件タブ内の設定値の保存
			SaveScanCondition(false);
        }


        //*************************************************************************************************
        //機　　能： 各コントロールのキャプションに文字列をセットする
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        //Private Sub SetCaption()
		public void SetCaption()		//v17.20 検出器切替改造に伴いpublic化
		{
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

            //Rev20.00 追加 by長野 2015/02/21
            ctbtnScanStop.Caption = CTResources.LoadResString(StringTable.IDS_btnScanStop);
			
            //■「スキャン条件」タブ関連

			//スキャンモードフレーム
			//optDataMode(1).Caption = infdef.data_mode(0)                            'ｽｷｬﾝ
			//optDataMode(4).Caption = infdef.data_mode(2)                            'コーンビームスキャン
			optDataMode1.Text = CTResources.LoadResString(StringTable.IDS_Scan);        //スキャン       'v17.43リソースから byやまおか 2011/01/30
			optDataMode4.Text = CTResources.LoadResString(StringTable.IDS_ConeBeam);    //コーンビーム   'v17.43リソースから byやまおか 2011/01/30
            optDataMode3.Text = CTResources.LoadResString(StringTable.IDS_Scano);       //スキャノ   　　'Rev21.00 追加 by長野 2015/02/19

			//生データ保存フレーム
			fraOrgSave.Text = CTSettings.infdef.Data.raw_save.GetString();			                                    //生データ保存
			chkSave.Text = StringTable.BuildResStr(StringTable.IDS_DoSave, StringTable.IDS_RawData);	//生データを保存する
            chkPurSave.Text = StringTable.BuildResStr(StringTable.IDS_DoSave, StringTable.IDS_PurRawData); //Rev23.10 追加 純生データ保存 by長野 2015/12/28

            //追加2014/10/07hata_v19.51反映
            //スキャンモードフレーム     'v18.00追加 byやまおか 2011/02/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            optScanMode[1].Text = CTSettings.infdef.Data.scan_mode[0].GetString();            //ﾊｰﾌｽｷｬﾝ
            optScanMode[2].Text = CTSettings.infdef.Data.scan_mode[1].GetString();            //ﾌﾙｽｷｬﾝ
            optScanMode[3].Text = CTSettings.infdef.Data.scan_mode[2].GetString();            //ｵﾌｾｯﾄｽｷｬﾝ
            optScanMode[4].Text = CTSettings.infdef.Data.scan_mode[3].GetString();            //ｼﾌﾄｽｷｬﾝ

            //Rev25.01 追加 by長野 2016/06/19
            chkW_Scan.Text = CTResources.LoadResString(25001);                                 //Wスキャン

            //Rev26.10 Zの場合 by井上 2017/12/18
            if (CTSettings.scaninh.Data.guide_mode == 1)
            {
                fraCondImg.Text = CTResources.LoadResString(15207);
            }

            //■「透視」タブ関連

			//透視画像処理フレーム
			//    cmdFImageOpen.Caption = BuildResStr(IDS_Open, IDS_TransImage)           '透視画像を開く
			//    cmdFImageSave.Caption = BuildResStr(IDS_Save, IDS_TransImage)           '透視画像の保存
			//v17.60 ストリングテーブルのIDを変更　by長野 2011/05/25
			cmdFImageOpen.Text = StringTable.BuildResStr(20184);			                                //透視画像を開く
			cmdFImageSave.Text = StringTable.BuildResStr(20185);			                                //透視画像の保存
			//chkInfSave.Text = StringTable.BuildResStr(StringTable.IDS_Save, StringTable.IDS_ImageInfo);		//付帯情報の保存
            //Rev20.01 変更 by長野 2015/05/19
            chkInfSave.Text = CTResources.LoadResString(12577);		//付帯情報の保存


			//コントラストフレーム
			//optScale(0).Caption = GetResString(IDS_Times, " 1")                     ' 4 倍 → 1 倍に変更 by 間々田 2005/01/07
			//optScale(1).Caption = GetResString(IDS_Times, " 4")                     ' 8 倍 → 4 倍に変更 by 間々田 2005/01/07
			//optScale(2).Caption = GetResString(IDS_Times, "16")                     '16 倍
			optScale0.Text = " 1 / 16";			//v17.10変更 byやまおか 2010/08/26
			optScale1.Text = " 1 / 4";			//v17.10変更 byやまおか 2010/08/26
			optScale2.Text = " 1 / 1";			//v17.10変更 byやまおか 2010/08/26

			//フラットパネル設定フレーム 'v17.00追加 byやまおか 2010/02/17
			//If (DetType = DetTypePke) Then 'v17.10 if追加 byやまおか 2010/08/31
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) && (CTSettings.scaninh.Data.mechacontrol == 0))        //v17.20修正 byやまおか 2010/09/16
            {
#if !NoCamera   // v17.48/v17.53でバグ用 2011/13/24
				//PkeGetIntegListFromINI hPke, fpd_integlist(0)

				//v17.50変更 iniファイルからの読み込み方法を変更 WinAPIを利用する by 間々田 2010/12/24
				int i = 0;
                StringBuilder dummy = new StringBuilder(256);
				bool XisIniError = false;
				XisIniError = false;

                for (i = 0; i <= modCT30K.FPD_INTEGLIST_MAX - 1; i++)
                {
                    if (modDeclare.GetPrivateProfileString("Timings", "Timing_" + i.ToString(), "", dummy, (uint)dummy.Capacity, AppValue.XisIniFileName) > 0)
                    {
                        double dummyValue = 0;
						double.TryParse(Convert.ToString(dummy), out dummyValue);
                        modCT30K.fpd_integlist[i] = dummyValue / 1000;
                    }
                    else
                    {
                        modCT30K.fpd_integlist[i] = 999;        //失敗した場合
                        XisIniError = true;
                    }
                }

                if (XisIniError)
                {
                    //MsgBox "以下のファイルの読み込み時にエラーが発生しています。" & vbCr & vbCr & XisIniFileName, vbExclamation
                    //v17.60 ストリングテーブル化 by長野 2011/05/30
                    MessageBox.Show(CTResources.LoadResString(20176) + "\r" + "\r" + AppValue.XisIniFileName, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
#else
				modCT30K.fpd_integlist[0] = 66;
				modCT30K.fpd_integlist[1] = 99;
				modCT30K.fpd_integlist[2] = 133;
				modCT30K.fpd_integlist[3] = 166;
				modCT30K.fpd_integlist[4] = 199;
				modCT30K.fpd_integlist[5] = 249;
				modCT30K.fpd_integlist[6] = 498;
				modCT30K.fpd_integlist[7] = 999;
				modCT30K.fpd_integlist[8] = 1999;
#endif
                if (CTSettings.t20kinf.Data.pki_fpd_type == 1 || CTSettings.t20kinf.Data.pki_fpd_type == 2 ||
                   CTSettings.t20kinf.Data.pki_fpd_type == 3)
                {
                    cmbGain.Items[0] = modCT30K.GetFpdGainStr(0,CTSettings.t20kinf.Data.pki_fpd_type);
                    cmbGain.Items[1] = modCT30K.GetFpdGainStr(1, CTSettings.t20kinf.Data.pki_fpd_type);
                    cmbGain.Items[2] = modCT30K.GetFpdGainStr(2, CTSettings.t20kinf.Data.pki_fpd_type);
                    cmbGain.Items[3] = modCT30K.GetFpdGainStr(3, CTSettings.t20kinf.Data.pki_fpd_type);
                    cmbGain.Items[4] = modCT30K.GetFpdGainStr(4, CTSettings.t20kinf.Data.pki_fpd_type);
                    cmbGain.Items[5] = modCT30K.GetFpdGainStr(5, CTSettings.t20kinf.Data.pki_fpd_type);
                }
                else
                {
                    cmbGain.Items.Clear();
                    cmbGain.Items.Add(modCT30K.GetFpdGainStr(0, CTSettings.t20kinf.Data.pki_fpd_type));
                    cmbGain.Items.Add(modCT30K.GetFpdGainStr(1, CTSettings.t20kinf.Data.pki_fpd_type));
                    cmbGain.Items.Add(modCT30K.GetFpdGainStr(2, CTSettings.t20kinf.Data.pki_fpd_type));
                    cmbGain.Items.Add(modCT30K.GetFpdGainStr(3, CTSettings.t20kinf.Data.pki_fpd_type));
                    cmbGain.Items.Add(modCT30K.GetFpdGainStr(4, CTSettings.t20kinf.Data.pki_fpd_type));
                }
				cmbInteg.Items[0] = modCT30K.GetFpdIntegStr(0);
				cmbInteg.Items[1] = modCT30K.GetFpdIntegStr(1);
				cmbInteg.Items[2] = modCT30K.GetFpdIntegStr(2);
				cmbInteg.Items[3] = modCT30K.GetFpdIntegStr(3);
				cmbInteg.Items[4] = modCT30K.GetFpdIntegStr(4);
				cmbInteg.Items[5] = modCT30K.GetFpdIntegStr(5);
				cmbInteg.Items[6] = modCT30K.GetFpdIntegStr(6);
				cmbInteg.Items[7] = modCT30K.GetFpdIntegStr(7);

                //追加2014/10/07hata_v19.51反映
                //従来の「拡大」（縮小）ボタンを「サイズ変更」ボタンにする   'v17.4X/v18.00追加 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                cmdFZoom.Text = CTResources.LoadResString(StringTable.IDA_btnResize);

            }

            //■「ガイド」タブ関連
            ToolTip1.SetToolTip(optScanArea0, "φ" + CTSettings.iniValue.ScanArea16inchFPD_SS.ToString() + "mm");
            ToolTip1.SetToolTip(optScanArea1, "φ" + CTSettings.iniValue.ScanArea16inchFPD_S.ToString() + "mm");
            ToolTip1.SetToolTip(optScanArea2, "φ" + CTSettings.iniValue.ScanArea16inchFPD_M.ToString() + "mm");
            ToolTip1.SetToolTip(optScanArea3, "φ" + CTSettings.iniValue.ScanArea16inchFPD_L.ToString() + "mm");
            ToolTip1.SetToolTip(optScanArea4, "φ" + CTSettings.iniValue.ScanArea16inchFPD_LL.ToString() + "mm");

            lblScanTime0.Text = CTSettings.iniValue.scanTime[CTSettings.iniValue.guideImgQualityIndex[0]].ToString();
            lblScanTime1.Text = CTSettings.iniValue.scanTime[CTSettings.iniValue.guideImgQualityIndex[1]].ToString();
            lblScanTime2.Text = CTSettings.iniValue.scanTime[CTSettings.iniValue.guideImgQualityIndex[2]].ToString();
            lblScanTime3.Text = CTSettings.iniValue.scanTime[CTSettings.iniValue.guideImgQualityIndex[3]].ToString();

            for (int cnt = 0; cnt < optScanMat.Length; cnt++)
            {
                optScanMat[cnt].Text = CTSettings.iniValue.scanTime[cnt].ToString();
            }

            string MatSizeIndex = "";
            string MatrixSize = "";
            string SliceNum = "";
            string ViewNum = "";
            string IntegNum = "";
            string IntegTimeIndex = "";
            string IntegTime = "";
            string CondFileName = "";
            string Space1 = "        ";
            string Space2 = "    ";
            string Space3 = "  ";
            string Colon = "：";

            for (int cnt = 0; cnt < optScanCond.Length - 1; cnt++)
            {
                int Index = 0;

                switch (cnt)
                {
                    case 0:
                        Index = CTSettings.iniValue.guideImgQualityIndex[0];
                        break;
                    case 1:
                        Index = CTSettings.iniValue.guideImgQualityIndex[1];
                        break;
                    case 2:
                        Index = CTSettings.iniValue.guideImgQualityIndex[2];
                        break;
                    case 3:
                        Index = CTSettings.iniValue.guideImgQualityIndex[3];
                        break;
                    case 4:
                        Index = 16;
                        break;
                    default:
                        return;
                }

                CondFileName = modScanCondition.getScanCondFileName(Index);
                modScanCondition.getPresetFileItem(CondFileName, "matrix_size", ref MatSizeIndex);
                switch (Convert.ToInt32(MatSizeIndex))
                {
                    case 1:
                        MatrixSize = "512";
                        break;
                    case 2:
                        MatrixSize = "512";
                        break;
                    case 3:
                        MatrixSize = "1024";
                        break;
                    case 4:
                        MatrixSize = "2048";
                        break;
                    case 5:
                        MatrixSize = "4096";
                        break;
                }			//v16.10 4096を追加 by 長野　2010/02/23
                modScanCondition.getPresetFileItem(CondFileName, "scan_view", ref ViewNum);
                modScanCondition.getPresetFileItem(CondFileName, "scan_integ_number", ref IntegNum);
                modScanCondition.getPresetFileItem(CondFileName, "k", ref SliceNum);
                modScanCondition.getPresetFileItem(CondFileName, "fpd_integ", ref IntegTimeIndex);
                IntegTime = Math.Floor((modCT30K.fpd_integlist[Convert.ToInt32(IntegTimeIndex)])).ToString("0");

                toolTip2.SetToolTip(optScanCond[cnt], CTResources.LoadResString(12814) + Colon + MatrixSize + "\n" +
                                                      CTResources.LoadResString(9059) + Space2 + Colon + SliceNum + "\n" +
                                                      CTResources.LoadResString(12808) + Space1 + Colon + ViewNum + "\n" +
                                                      CTResources.LoadResString(12809) + Space1 + Colon + IntegNum + "\n" +
                                                      CTResources.LoadResString(20164) + "(msec)" + Space3 + Colon + IntegTime);
            }
            for (int cnt = 0; cnt < optScanMat.Length; cnt++)
            {
                CondFileName = modScanCondition.getScanCondFileName(cnt);
                modScanCondition.getPresetFileItem(CondFileName, "matrix_size", ref MatSizeIndex);
                switch (Convert.ToInt32(MatSizeIndex))
                {
                    case 1:
                        MatrixSize = "512";
                        break;
                    case 2:
                        MatrixSize = "512";
                        break;
                    case 3:
                        MatrixSize = "1024";
                        break;
                    case 4:
                        MatrixSize = "2048";
                        break;
                    case 5:
                        MatrixSize = "4096";
                        break;
                }			//v16.10 4096を追加 by 長野　2010/02/23
                modScanCondition.getPresetFileItem(CondFileName, "scan_view", ref ViewNum);
                modScanCondition.getPresetFileItem(CondFileName, "scan_integ_number", ref IntegNum);
                modScanCondition.getPresetFileItem(CondFileName, "k", ref SliceNum);
                modScanCondition.getPresetFileItem(CondFileName, "fpd_integ", ref IntegTimeIndex);
                IntegTime = Math.Floor((modCT30K.fpd_integlist[Convert.ToInt32(IntegTimeIndex)])).ToString("0");

                toolTip2.SetToolTip(optScanMat[cnt], CTResources.LoadResString(12814) + Colon + MatrixSize + "\n" +
                                                      CTResources.LoadResString(9059) + Space2 + Colon + SliceNum + "\n" +
                                                      CTResources.LoadResString(12808) + Space1 + Colon + ViewNum + "\n" +
                                                      CTResources.LoadResString(12809) + Space1 + Colon + IntegNum + "\n" +
                                                      CTResources.LoadResString(20164) + "(msec)" + Space3 + Colon + IntegTime);
            }
        }


        //*************************************************************************************************
        //機　　能： 各コントロールの初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        //Private Sub InitControls()
		public void InitControls()		//v17.20 検出器切り替え改造のためpublicに変更
		{
			int theTop = 0;
            int i = 0;

            int me_top = 0;
            int me_left = 0;
            int me_width = 0;

			frmCTMenu frmCTMenu = frmCTMenu.Instance;

			//フォームの表示位置(2ndPC画面対応)  'v15.0追加 byやまおか
			//ツールバーの位置を基準とする
			me_left = frmCTMenu.Toolbar1.Left;			//左端

			//メカ制御とX線制御があるときは、X線制御画面の下
            if ((CTSettings.scaninh.Data.mechacontrol == 0) && (CTSettings.scaninh.Data.xray_remote == 0))
            {
                me_top = frmCTMenu.Toolbar1.Top + frmMechaControl.Instance.Height + frmXrayControl.Instance.Height;
                me_width = modCT30K.FmControlWidth;
            }                
            //メカ制御がある(X線制御がない)ときは、メカ制御画面の下
            else if (CTSettings.scaninh.Data.mechacontrol == 0)
            {
                me_top = frmCTMenu.Toolbar1.Top + frmMechaControl.Instance.Height;
                me_width = modCT30K.FmControlWidth;                                
            }
            //メカ制御とX線制御がないときは一番上
            else
            {
                me_top = frmCTMenu.Toolbar1.Top;
                me_width = modCT30K.FmControlWidth2nd;                //幅狭
            }

			this.SetBounds(me_left, me_top, me_width, 0, BoundsSpecified.X | BoundsSpecified.Y | BoundsSpecified.Width);	//指定位置に移動

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			'v17.60 英語用レイアウト調整 by長野 2011/06/01
			If IsEnglish And Not (scaninh.mechacontrol = 0) Then
        
				SSTab1.TabsPerRow = 3
        
			End If
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//タブ
            //SSTab1.TabPages[0].Visible = (CTSettings.scaninh.Data.mechacontrol == 0);			//校正
            //SSTab1.TabPages[1].Visible = (CTSettings.scaninh.Data.mechacontrol == 0);			//スキャン条件
            //SSTab1.TabPages[2].Visible = (CTSettings.scaninh.Data.mechacontrol == 0);			//透視   'v15.0追加 byやまおか 2009/07/29

            //Rev26.10 ガイド機能無しの場合、タブタイトルの削除　by井上 2017/12/12
            if (CTSettings.scaninh.Data.guide_mode == 1)
            {
                SSTab1.TabPages.Remove(tabPage6);
            }

            //Rev26.12 change by chouno 2018/04/05
            if (CTSettings.scaninh.Data.guide_mode == 1)
            {
                //Rev26.10 プリセットのラジオボタンを押さない限りプリセットのフレームは表示されない by井上 2018/1/15
                fraCondImg.Visible = false;
            }
            else
            {
                //Rev26.10(特)修正 by chouno 2018/03/21
                fraCondImg.Visible = true;
            }

			//フォーム幅に応じて、幅を調整する          
            SSTab1.Width = this.Width - 40;    			        //全タブの幅             'v15.0追加 byやまおか 2009/07/29
            fraScanCondition[4].Width = SSTab1.Width - 20;		//再々構成フレームの幅   'v15.0追加 byやまおか 2009/07/29
            fraScanCondition[5].Width = SSTab1.Width - 20;		//画像処理フレームの幅   'v15.0追加 byやまおか 2009/07/29

            if (CTSettings.scaninh.Data.mechacontrol == 1)
            {
                if (SSTab1.TabPages.Contains(tabPage1)) SSTab1.TabPages.Remove(tabPage1);   //校正
                if (SSTab1.TabPages.Contains(tabPage2)) SSTab1.TabPages.Remove(tabPage2);   //スキャン条件
                if (SSTab1.TabPages.Contains(tabPage3)) SSTab1.TabPages.Remove(tabPage3);   //透視

                fraScanCondition[4].Parent = SSTab1.TabPages[0];
                fraScanCondition[4].Location = new Point(1, 2);

                fraScanCondition[5].Parent = this;
                fraScanCondition[5].Top = SSTab1.Top + SSTab1.ItemSize.Height;
                fraScanCondition[5].Left = SSTab1.Left;
            }
            else
            {
                //選択されていないTabのVisibleがFalse になるための処置
                SSTab1.SelectedIndex = 0;
                for (int ii = 1; ii <= fraScanCondition.GetUpperBound(0); ii++)
                {
                    fraScanCondition[ii].Parent = this;
                    fraScanCondition[ii].Top = SSTab1.Top + SSTab1.ItemSize.Height;
                    fraScanCondition[ii].Left = SSTab1.Left;
                }
            }
            SSTab1.ItemSize = new Size((int)((SSTab1.ClientSize.Width - 4) / SSTab1.TabCount), SSTab1.ItemSize.Height);

           

			//■「スキャン条件」タブ関連

            //データモード
            optDataMode1.Enabled = (CTSettings.scaninh.Data.data_mode[0] == 0);	//スキャン
            optDataMode4.Enabled = (CTSettings.scaninh.Data.data_mode[2] == 0);	//コーンビームスキャン
            optDataMode3.Enabled = (CTSettings.scaninh.Data.data_mode[1] == 0); //スキャノ //Rev21.00 追加 by長野 2015/02/19

            optDataMode1.Visible = (CTSettings.scaninh.Data.data_mode[0] == 0);	//スキャン //Rev21.00 追加 by長野 2015/03/08
            optDataMode4.Visible = (CTSettings.scaninh.Data.data_mode[2] == 0);	//コーンビームスキャン //Rev21.00 追加 by長野 2015/03/08
            optDataMode3.Visible = (CTSettings.scaninh.Data.data_mode[1] == 0); //スキャノ //Rev21.00 追加 by長野 2015/02/19

            modLibrary.RePosVisibleOption2(optDataMode,4); //Rev22.00 追加 by長野 2015/09/07

            //追加2014/10/07hata_v19.51反映
            //スキャンモード         'v18.00追加 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //v19.50 産業用CTモードのみの表示に変更 by長野 2013/11/07
            //if (CTSettings.scaninh.Data.avmode == 0)
            //Rev25.00 Wスキャン追加 by長野 2016/07/05
            if (CTSettings.scaninh.Data.avmode == 0 || CTSettings.W_ScanOn)
            {
                fraScanMode.Visible = true;
            }
            //Rev25.00 変更 by長野 2016/07/05
            //else if (CTSettings.scaninh.Data.avmode == 1)
            else
            {
                fraScanMode.Visible = false;
            }

            //Rev25.00 不要なのでコメントアウト by長野 2016/07/05 --->

            modLibrary.RePosVisibleOption2(optScanMode, 0); //Rev23.20 追加 by長野 2015/09/07

            //Rev25.00 WスキャンON時は、シフトを隠してWスキャン表示
            if (CTSettings.W_ScanOn)
            {
                frmScanControl.Instance.optScanMode[4].Visible = false;
                frmScanControl.Instance.chkW_Scan.Visible = true;
                frmScanControl.Instance.chkW_Scan.Location = new Point(frmScanControl.Instance.optScanMode[1].Left, frmScanControl.Instance.optScanMode[1].Top);
                for (int cnt = 1; cnt < 4; cnt++)
                {
                    frmScanControl.Instance.optScanMode[cnt].Location = new Point(frmScanControl.Instance.optScanMode[cnt + 1].Left, frmScanControl.Instance.optScanMode[cnt + 1].Top);
                }
            }

            optScanMode[1].Enabled = (CTSettings.scaninh.Data.scan_mode[0] == 0);            //ハーフ
            optScanMode[2].Enabled = (CTSettings.scaninh.Data.scan_mode[1] == 0);            //フル
            optScanMode[3].Enabled = (CTSettings.scaninh.Data.scan_mode[2] == 0);            //オフセット
            //optScanMode[4].Enabled = (CTSettings.scaninh.Data.scan_mode[3] == 0);            //シフトオフセット
            //Rev25.00 変更 by長野 2016/06/19
            //optScanMode[4].Enabled = (CTSettings.scaninh.Data.scan_mode[3] == 0 && CTSettings.W_ScanOn);
            //Rev26.10 変更 by chouno 2018/01/13
            optScanMode[4].Enabled = (CTSettings.scaninh.Data.scan_mode[3] == 0 && !CTSettings.W_ScanOn);

            optScanMode[1].Visible = (CTSettings.scaninh.Data.scan_mode[0] == 0);            //ハーフ
            optScanMode[2].Visible = (CTSettings.scaninh.Data.scan_mode[1] == 0);            //フル
            optScanMode[3].Visible = (CTSettings.scaninh.Data.scan_mode[2] == 0);            //オフセット
            //optScanMode[4].Visible = (CTSettings.scaninh.Data.scan_mode[3] == 0);            //シフトオフセット
            //Rev25.00 変更 by長野 2016/06/19
            //optScanMode[4].Visible = (CTSettings.scaninh.Data.scan_mode[3] == 0 && CTSettings.scaninh.Data.w_scan == 1);
            //Rev26.10 変更 by chouno 2018/01/13
            optScanMode[4].Visible = (CTSettings.scaninh.Data.scan_mode[3] == 0 && !CTSettings.W_ScanOn);
            //<---

            //プリセット     'v18.00追加 byやまおか 2011/07/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //cmdPresetRef.Enabled = false;
            cmdPresetRef.Enabled = true;//Rev26.00 change by chouno 2017/08/31
            txtPresetImgFile.Text = "";

            //生データ保存   'v19.50 統合に伴う追加 by長野 2013/11/07
            //産業用CTモードOFFの場合は、データモードの下に来るように変更
            //if ((CTSettings.scaninh.Data.avmode == 1))
            //Rev25.00 Wスキャンを条件に入れる by長野 2016/07/05
            //if ((CTSettings.scaninh.Data.avmode == 1) || CTSettings.W_ScanOn)
            //Rev25.01 修正 by長野 2016/12/16
            if ((CTSettings.scaninh.Data.avmode == 1) && !CTSettings.W_ScanOn)
            {
                fraOrgSave.Top = 92;    // 1380/15
            }
            //Rev25.00 Wスキャンを条件に入れる by長野 2016/07/05
            else if ((CTSettings.scaninh.Data.avmode == 0) || CTSettings.W_ScanOn)
            {
                fraOrgSave.Top = 204;   // 3060/15
            }

            //Rev23.12 純生データ保存を生かさない場合は、chkPueSaveのVisibleをfalseに変更してフレームのサイズも変える
            if (CTSettings.scaninh.Data.save_purdata == 1)
            {
                chkPurSave.Visible = false;
                fraOrgSave.Height = 63;
            }

            //Rev26.10 Zの場合 by井上 2017/12/18
            if (CTSettings.scaninh.Data.guide_mode == 1)
            {
                fraQuality.SetBounds(4, 12, 130, 143);
                fraQuality.Visible = true;

                fraCondImg.SetBounds(172, 40, 341, 77);
                fraCondImg.Top = fraQuality.Top + fraQuality.Height - fraCondImg.Height;
                fraCondImg.Left = fraQuality.Left + fraQuality.Width + 10;
                txtPresetImgFile.SetBounds(12, 44, 317, 22);
                cmdPresetRef.SetBounds(252, 16, 73, 25);
                chkUserCondition.SetBounds(12, 19, 85, 17);
                chkUserCondition.Visible = true;
                chkImageCondition.SetBounds(112,20,59,17);
                chkImageCondition.Visible = true;
                fraComment.SetBounds(4, 257, 517, 69);
                fraImgSave.SetBounds(4, 160, 517, 91);                
            }


            //■「ガイド」タブ関連 //Rev26.00 add by chouno 2017/08/31
            scanCondImg0 = Image.FromFile(Path.Combine(AppValue.CTPATH,@"Png\256x256_Quick.png"));
            scanCondImg1 = Image.FromFile(Path.Combine(AppValue.CTPATH,@"Png\256x256_Rough.png"));
            scanCondImg2 = Image.FromFile(Path.Combine(AppValue.CTPATH,@"Png\256x256_Normal.png"));
            scanCondImg3 = Image.FromFile(Path.Combine(AppValue.CTPATH,@"Png\256x256_Fine.png"));
            scanCondImg4 = Image.FromFile(Path.Combine(AppValue.CTPATH,@"Png\User.png"));

            //cmbPreset初期化
            modScanCondition.getPresetList(ref modScanCondition.PresetName, ref modScanCondition.PresetPath);
            setCmbPresetList(modScanCondition.PresetName);

            tabControl1.Size = new Size(443, 269);

            //Rev26.00 add by chouno 2017/10/31
            cmdXrayAutoVILow.IsIgnorePerformClick = true;
            cmdXrayAutoVIHigh.IsIgnorePerformClick = true;
            cmdXrayAutoVIMid.IsIgnorePerformClick = true;

			//■「校正」タブ関連
			//全自動校正ボタン
            if (CTSettings.scaninh.Data.auto_cor != 0)
            {
                cmdCorrect8.Visible = false;
            }

            //scaninhによってスキャン前の自動校正チェックボックスの表示・非表示 by井上 2017/12/12 
            if (CTSettings.scaninh.Data.auto_correct_before_scan == 1)
            {
                chkAutoCorrectBeforeScan.Visible = false;
            }

			//校正ステータスフレーム
			fraCorrectStatus.Visible = (CTSettings.scaninh.Data.cor_status == 0);

			//FPDの場合、幾何歪校正ステータスは表示しない added by 間々田 2003/10/20
            lblItem2.Visible = !CTSettings.detectorParam.Use_FlatPanel;
            lblColon2.Visible = !CTSettings.detectorParam.Use_FlatPanel;
            lblStatus2.Visible = !CTSettings.detectorParam.Use_FlatPanel;

			//FPDの場合、幾何歪校正より下のステータス位置を上にずらす  'v16.20/v17.00追加 byやまおか 2010/01/20
            if (CTSettings.detectorParam.Use_FlatPanel)
            {
			    int posUp = 0;
                posUp = chkInhibit1.Top - lblItem2.Top;             //ずらす量
                chkInhibit3.Top = chkInhibit3.Top + posUp;          //回転中心校正
                lblItem4.Top = lblItem4.Top + posUp;                //オフセット校正
                chkInhibit5.Top = chkInhibit5.Top + posUp;          //寸法校正
                for (i = 3; i <= 5; i++)
                {
                    lblColon[i].Top = lblColon[i].Top + posUp;          //：
                    lblStatus[i].Top = lblStatus[i].Top + posUp;        //ステータス
                }
            }

			//２次元幾何歪の場合，スキャン位置校正と幾何歪校正の配置を入れ替える     v11.2追加 by 間々田 2005/10/07
			//If scaninh.full_distortion = 0 Then                            'v16.20/v17.00削除 byやまおか 2010/01/20	
            if ((CTSettings.scaninh.Data.full_distortion == 0) && (!CTSettings.detectorParam.Use_FlatPanel))		//v16.20/v17.00追加 byやまおか 2010/01/20
            {
                //ラベル
                theTop = chkInhibit1.Top;
                chkInhibit1.Top = lblItem2.Top;
                lblItem2.Top = theTop;

                //コロン
                theTop = lblColon1.Top;
                lblColon1.Top = lblColon2.Top;
                lblColon2.Top = theTop;

                //ステータス
                theTop = lblStatus1.Top;
                lblStatus1.Top = lblStatus2.Top;
                lblStatus2.Top = theTop;
            }

			//PkeFPDの場合はオフセット校正を先頭にする   'v16.20/v17.00追加 byやまおか 2010/03/01
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
				//ラベル
                //変更2014/11/21hata
                //lblItem4.Top = lblItem0.Top;			    //(↑Up)オフセット
                //lblItem0.Top = lblItem0.Top + 41;			//(↓Down)ゲイン
                //chkInhibit1.Top = chkInhibit1.Top + 41;	    //(↓Down)スキャン位置
                //lblItem2.Top = lblItem2.Top + 41;			//(↓Down)幾何歪
                //chkInhibit3.Top = chkInhibit3.Top + 41;	    //(↓Down)回転中心

                ////コロンとステータス
                //lblColon4.Top = lblColon0.Top;
                //lblStatus4.Top = lblStatus0.Top;
                //for (i = 0; i <= 3; i++)
                //{
                //    lblColon[i].Top = lblColon[i].Top + 41;
                //    lblStatus[i].Top = lblStatus[i].Top + 41;
                //}

                int posWid = 41;
				lblItem4.Top = lblItem0.Top;			    //(↑Up)オフセット
                lblItem0.Top = lblItem4.Top + posWid;			//(↓Down)ゲイン
                chkInhibit1.Top = lblItem0.Top + posWid;	    //(↓Down)スキャン位置
                lblItem2.Top = chkInhibit1.Top;			        //(↓Down)幾何歪
                chkInhibit3.Top = chkInhibit1.Top + posWid;	    //(↓Down)回転中心
                chkInhibit5.Top = chkInhibit3.Top + posWid;	    //(↓Down)寸法校正
                
				//コロンとステータス
				lblColon4.Top = lblColon0.Top;
				lblStatus4.Top = lblStatus0.Top;
                lblColon0.Top = lblItem0.Top;
                lblStatus0.Top = lblItem0.Top;
                lblColon1.Top = chkInhibit1.Top;
                lblStatus1.Top = chkInhibit1.Top;
                lblColon2.Top = lblItem2.Top;
                lblStatus2.Top = lblItem2.Top;
                lblColon3.Top = chkInhibit3.Top;
                lblStatus3.Top = chkInhibit3.Top;
                lblColon5.Top = chkInhibit5.Top;
                lblStatus5.Top = chkInhibit5.Top;

			}

			//■「透視」タブ関連

			//倍率のフレームの表示・非表示
            fraScale.Visible = (CTSettings.scancondpar.Data.fimage_bit == 2) && CTSettings.detectorParam.Use_FlatPanel;

			//    'ウィンドウレベル・ウィンドウ幅の最大値の設定
			//    SetOption optScale, FimageBitIndex

            //追加2015/01/08hata_dNet
            //UpDownボタンをTextBoxの裏に隠す
            cwneWL.Location = txtWL.Location;
            cwneWW.Location = txtWW.Location;

			//ウィンドウレベル・ウィンドウリセット
			cmdWLWWReset_Click(this.cmdWLWWReset, EventArgs.Empty);

            //上記の処理ではPointerValueChangedイベントは発生しないので以下の処理を行なう
            cwsldLevel_ValueChanged(cwsldLevel, EventArgs.Empty);
            cwsldWidth_ValueChanged(cwsldWidth, EventArgs.Empty);

            //Ｘ線検出器がフラットパネルでかつＦＰＤ処理フレーム表示ありの場合のみフラットパネル処理フレームを表示する
            fraFlatPanel.Visible = CTSettings.detectorParam.Use_FlatPanel && (CTSettings.scaninh.Data.fpd_frame == 0);			//v11.2変更 by 間々田 2005/10/19

			//フラットパネル対応
            if (CTSettings.detectorParam.Use_FlatPanel) chkGainCalib.CheckState = CheckState.Checked;			//ゲイン補正
            if (CTSettings.detectorParam.Use_FlatPanel) chkPixelDefectCalib.CheckState = CheckState.Checked;	//画素欠陥補正

			//PkeFPDのゲイン/積分時間をセットする    'v17.10追加 byやまおか 2010/07/28
            fraFpdGainIntegSet.Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);
            fraFpdGainIntegSet.Visible = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);
			//If (DetType = DetTypePke) Then	
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) && (CTSettings.scaninh.Data.mechacontrol == 0))		//v17.20修正 byやまおか 2010/09/16
            {
                //fraFpdGainIntegSet.Visible = True  '外へ
                cmbGain.SelectedIndex = CTSettings.scansel.Data.fpd_gain;        //FPDのゲイン設定
                cmbInteg.SelectedIndex = CTSettings.scansel.Data.fpd_integ;      //FPDの積分時間設定
            }

            //Rev20.00 追加 by長野 2015/02/06
            hsbAverage.Value = 5;

			//■「再々構成」タブ関連

			//コーン後再構成
			cmdPostConeReconst.Visible = (CTSettings.scaninh.Data.post_cone_reconstruction == 0);

			//v19.00 BHC有効/無効 (電S2)永井
			fraBHC.Visible = (CTSettings.scaninh.Data.mbhc == 0);

			//■「画像処理」タブ関連

			//「画像処理」タブ内コントロールの初期化
			InitImageProc();

			//■その他

			//スキャンスタートボタン
			//cmdScanStart.Visible = (scaninh.mechacontrol = 0)
            ctbtnScanStart.Visible = (CTSettings.scaninh.Data.mechacontrol == 0);

            //Rev20.00 追加 by長野 2015/02/21
            ctbtnScanStop.Visible = false;

			//タブ枠フレームの線を消去
            for (i = fraScanCondition.GetLowerBound(0); i <= fraScanCondition.GetUpperBound(0); i++)
            {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				fraScanCondition(i).BorderStyle = vbBSNone
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				// .NET ではフレーム枠を「なし」にはできないのでせめてテキストだけでも「なし」にしておく
                fraScanCondition[i].Text = string.Empty;
            }

			//'初期表示タブ：校正タブ
			//SSTab1.Tab = 0
			//fraScanCondition(0).Enabled = True

			//初期表示タブ                           'v15.0変更 2ndPC対応 byやまおか 2009/07/29
            if (CTSettings.scaninh.Data.mechacontrol == 0)
            {
                SSTab1.SelectedIndex = 0;                //校正タブ
                fraScanCondition[0].Enabled = true;
            }
            else
            {
                SSTab1.SelectedIndex = 5;                //画像処理タブ
                fraScanCondition[5].Enabled = true;
            }

			//Ｘ線条件フレームの表示     'v15.10追加 byやまおか
			fraXrayVolCur.Visible = (CTSettings.scaninh.Data.xray_remote == 1) && (CTSettings.scaninh.Data.mechacontrol == 0);		//Ｘ線制御なし＆メカ制御ありの場合

			//PkeFPDのときはフラットパネル設定を表示 'v16.20/v17.00追加 byやまおか 2010/02/17
			//If DetType = DetTypePke Then		
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) && (CTSettings.scaninh.Data.mechacontrol == 0))	//v17.20修正 byやまおか 2010/09/16
            {
                //fraFpdGainInteg.Visible = True
                fraFpdGainInteg.Visible = (CTSettings.scaninh.Data.mechacontrol == 0);       //v17.10変更 byやまおか 2010/09/02
                //ntbFpdGain.Value = GetFpdGainStr(scansel.fpd_gain)
                //ntbFpdInteg.Value = GetFpdIntegStr(scansel.fpd_integ)

                //フラットパネル設定欄 更新処理  v17.50関数を使用するように変更 by 間々田 2011/02/16
                UpdateFpdGainInteg();
            }
        }


        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //
        //   「スキャン条件」タブ内の処理
        //
        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        //Zの場合　by井上 2017/12/18
        //*************************************************************************************************
        //機　　能： 画質オプションボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void optQuality_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton r = sender as RadioButton;

            if (r == null) return;

            //unCheckのときは何もしない
            if (!r.Checked) return;

            int Index = Array.IndexOf(optQuality, r);

            //unCheckのときは何もしない
            //if (!optQuality[Index].Checked) return;


            //プリセットフレーム
            //fraPreset.Visible = (Index == (int)ScanQualityConstants.ScanQualityPreset);

            switch (Index)
            {
                //Quick, Rough, Normal, Fine
                case (int)ScanQualityConstants.ScanQualityQuick:
                case (int)ScanQualityConstants.ScanQualityRough:
                case (int)ScanQualityConstants.ScanQualityNormal:
                case (int)ScanQualityConstants.ScanQualityFine:

                    //プリセットで画像のファイルパスが指定されていると、プリセット以外を選んで
                    //再度プリセットをクリックしたときに、いきなりメカ移動ダイアログが表示されてしまうため
                    //プリセット以外を選んだときに、画像のチェックを外す。ファイルパスはそのまま。

                    chkImageCondition.CheckState = CheckState.Unchecked;        //v16.20追加 byやまおか 2010/04/21

                    //プリセットのフレームを非表示、チェックボックスにチェックがあれば外す by井上 2018/1/15
                    fraCondImg.Visible = false;
                    chkUserCondition.CheckState = CheckState.Unchecked;
                    chkImageCondition.CheckState = CheckState.Unchecked;

                    //選択した画質に対応する *-SC.csv からスキャン条件をセットする
                    //if (modScanCondition.LoadSCFile(modFileIO.FSO.BuildPath(modFileIO.pathSetFile, optQuality[Index].Text + "-SC.CSV")))
                    if (modScanCondition.LoadSCFile(Path.Combine(AppValue.PathSetFile, optQuality[Index].Text + "-SC.CSV")))
                    {
                        //スキャン条件の内容を画面に反映
                        LoadScanCondition();
                    }
                    else
                    {
                        //失敗した場合メッセージを表示
                        //   指定された画質のスキャン条件を設定できませんでした。
                        MessageBox.Show(CTResources.LoadResString(9952), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    break;

                //プリセット
                case (int)ScanQualityConstants.ScanQualityPreset:

                    fraCondImg.Visible = true;
                
                    //'登録条件もしくは画像がチェックされている
                    //If chkUserCondition.Value = vbChecked Then
                    //
                    //    'プリセット「登録条件」チェックボックスクリック時処理と同じ処理
                    //    chkUserCondition_Click
                    //
                    //'画像がチェックされている
                    //ElseIf chkImageCondition.Value = vbChecked Then
                    //
                    //    'プリセット「画像」チェックボックスクリック時処理と同じ処理
                    //    chkImageCondition_Click
                    //
                    //End If

                    //登録条件もしくは画像がチェックされており，ファイル名が指定されている場合 '上記の処理変更 2009/06/17
                    if ((chkUserCondition.CheckState == CheckState.Checked || chkImageCondition.CheckState == CheckState.Checked) &&
                        //(!string.IsNullOrEmpty(txtPresetFile.Text)))
                        (!string.IsNullOrEmpty(txtPresetImgFile.Text)))
                    {
                        //プリセットファイルをロードする
                        LoadPreset();
                    }
                    break;

                //それ以外(マニュアル)   'v17.61追加 byやまおか 2011/06/28
                default:

                    //プリセット欄のチェックを外す   'v17.61追加 byやまおか 2011/06/28
                    chkUserCondition.CheckState = CheckState.Unchecked;
                    chkImageCondition.CheckState = CheckState.Unchecked;
                    break;
            }

        }

        //Zの場合　by井上 2017/12/18
        //*************************************************************************************************
        //機　　能： プリセット「登録条件」チェックボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void chkUserCondition_Click(object sender, EventArgs e)
        {
            //「参照」ボタンの使用可・不可の設定
            cmdPresetRef.Enabled = (chkUserCondition.CheckState == CheckState.Checked) || (chkImageCondition.CheckState == CheckState.Checked);
            //txtPresetFile.Enabled = (chkUserCondition.CheckState == CheckState.Checked) || (chkImageCondition.CheckState == CheckState.Checked);
            txtPresetImgFile.Enabled = (chkUserCondition.CheckState == CheckState.Checked) || (chkImageCondition.CheckState == CheckState.Checked);

            //チェックされた場合
            if (chkUserCondition.CheckState == CheckState.Checked)
            {
                //「プリセット－画像」が同時にチェックされないようにする
                if (chkImageCondition.CheckState == CheckState.Checked)
                {
                    chkImageCondition.CheckState = CheckState.Unchecked;
                }

                //ファイルが指定されていなければ
                //if (!Regex.IsMatch(txtPresetFile.Text.ToLower(), "^.+-sc[.]csv$"))
                if (!Regex.IsMatch(txtPresetImgFile.Text.ToLower(), "^.+-sc[.]csv$"))
                {
                    //ファイル欄をクリア
                    //txtPresetFile.Text = "";
                    txtPresetImgFile.Text = "";

                    //「参照」ボタンをクリックした時と同じ処理
                    cmdPresetRef_Click(this.cmdPresetRef, EventArgs.Empty);

                    //'それでもファイルが指定されなかった場合、チェックを外す    '外さないことにした 2009/06/17
                    //If Not (LCase$(txtPresetFile.Text) Like "*-sc.csv") Then
                    //    chkUserCondition.Value = vbUnchecked
                    //End If
                }
                else
                {
                    //登録条件を読み込むための処置
                    LoadPreset();
                }
            }
            //v17.60 チェックを外すときは、ファイル欄をクリアするように変更 by長野 2011/06/14
            //End If
            else
            {
                //ファイル欄をクリア
                //txtPresetFile.Text = "";
                txtPresetImgFile.Text = "";
            }
        }

        //Zの場合　by井上 2017/12/18
        //*************************************************************************************************
        //機　　能： プリセット「画像」チェックボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void chkImageCondition_Click(object sender, EventArgs e)
        {
            //「参照」ボタンの使用可・不可の設定
            cmdPresetRef.Enabled = (chkUserCondition.CheckState == CheckState.Checked) || (chkImageCondition.CheckState == CheckState.Checked);
            //txtPresetFile.Enabled = (chkUserCondition.CheckState == CheckState.Checked) || (chkImageCondition.CheckState == CheckState.Checked);
            txtPresetImgFile.Enabled = (chkUserCondition.CheckState == CheckState.Checked) || (chkImageCondition.CheckState == CheckState.Checked);

            //チェックされた場合
            if (chkImageCondition.CheckState == CheckState.Checked)
            {
                //「プリセット－登録条件」が同時にチェックされないようにする
                if (chkUserCondition.CheckState == CheckState.Checked)
                {
                    chkUserCondition.CheckState = CheckState.Unchecked;
                }

                //ファイルが指定されていなければ
                //if (!Regex.IsMatch(txtPresetFile.Text.ToLower(), "^.+[.]img$"))
                if (!Regex.IsMatch(txtPresetImgFile.Text.ToLower(), "^.+[.]img$"))
                {
                    //ファイル欄をクリア
                    //txtPresetFile.Text = "";
                    txtPresetImgFile.Text = "";

                    //「参照」ボタンをクリックした時と同じ処理
                    cmdPresetRef_Click(this.cmdPresetRef, new EventArgs());

                    //'それでもファイルが指定されなかった場合、チェックを外す    '外さないことにした 2009/06/17
                    //If Not (LCase$(txtPresetFile.Text) Like "*.img") Then
                    //    chkImageCondition.Value = vbUnchecked
                    //End If
                }
                else
                {
                    //登録条件を読み込むための処置
                    LoadPreset();
                }
            }
            //v17.60 ファイル欄に画像ファイル名が残っている状態で、その画像の付帯情報を消して、
            //もう一度チェックすると移動距離が全て０になるバグを修正 by長野 2011/06/14
            //チェックを外すと、ファイル欄を必ずクリアするようにした。
            //End If
            else
            {
                //ファイル欄をクリア
                //txtPresetFile.Text = "";
                txtPresetImgFile.Text = "";
            }
        }


        //*************************************************************************************************
        //機　　能： プリセットフレーム内「参照」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdPresetRef_Click(object sender, EventArgs e)
		{
			string FileName = null;

            // Mod Start 2018/12/04 M.Oyama V26.40 Windows10対応
            ////チェックの状態によって参照するファイルの拡張子を変更する by井上 2018/1/15
            //if(chkImageCondition.CheckState == CheckState.Checked)
            //{
            //    FileName = modFileIO.GetFileName(StringTable.IDS_Select, CTResources.LoadResString(StringTable.IDS_CTImage), ".img");
            //}
            //else if(chkUserCondition.CheckState == CheckState.Checked)
            //{
            //    FileName = modFileIO.GetFileName(StringTable.IDS_Select, CTResources.LoadResString(StringTable.IDS_CondFile), SubExtension: "-SC");
            //}
            if (CTSettings.scaninh.Data.guide_mode == 0)
            {
                FileName = modFileIO.GetFileName(StringTable.IDS_Select, CTResources.LoadResString(StringTable.IDS_CTImage), ".img");
            }
            else
            {
                //指定されたファイルをファイル名欄に表示
                if (!string.IsNullOrEmpty(FileName))
                {
                    txtPresetImgFile.Text = FileName;
                    LoadPreset();
                }
                //チェックの状態によって参照するファイルの拡張子を変更する by井上 2018/1/15
                if (chkImageCondition.CheckState == CheckState.Checked)
                {
                    FileName = modFileIO.GetFileName(StringTable.IDS_Select, CTResources.LoadResString(StringTable.IDS_CTImage), ".img");
                }
                else if (chkUserCondition.CheckState == CheckState.Checked)
                {
                    FileName = modFileIO.GetFileName(StringTable.IDS_Select, CTResources.LoadResString(StringTable.IDS_CondFile), SubExtension: "-SC");
                }
            }
            // Mod End 2018/12/04

			//指定されたファイルをファイル名欄に表示
            if (!string.IsNullOrEmpty(FileName))
            {
                txtPresetImgFile.Text = FileName;
                LoadPreset();
            }
        }

        //*************************************************************************************************
        //機　　能： プリセットファイル変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void LoadPreset()
		{
            //追加2014/10/07hata_v19.51反映
            int oldScanmode = 0;            //v18.00追加 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            int nowScanmode = 0;            //v18.00追加 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //追加2014/10/07hata_v19.51反映
            //前のスキャンモード     'v18.00追加 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            oldScanmode = modLibrary.GetOption(optScanMode);

            //Rev25.00 Wスキャン対応 by長野 2016/07/11
            CheckState old_W_ScanCheckState = (CTSettings.scansel.Data.w_scan == 1 ? CheckState.Checked : CheckState.Unchecked);
            CheckState now_W_ScanCheckState = CheckState.Unchecked;

            //未指定の場合何もしない
            if (string.IsNullOrEmpty(txtPresetImgFile.Text)) return;

			//画像ファイルが指定された
            if (Regex.IsMatch(txtPresetImgFile.Text.ToLower(), "^.+[.]img$"))
            {
 				//画像ファイルからスキャン条件を設定
				if (!modScanCondition.LoadSCImageFile(txtPresetImgFile.Text)) 
                {
                    //失敗した場合メッセージを表示：
					//   指定された画像ファイルからスキャン条件を設定できませんでした。
                    MessageBox.Show(StringTable.BuildResStr(9951, StringTable.IDS_CTImage), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
                }
            }
            //スキャン条件ファイルが指定された
            else if (Regex.IsMatch(txtPresetImgFile.Text.ToLower(), "^.+-sc[.]csv$"))
            {
                //スキャン条件ファイルからスキャン条件を設定
                if (!modScanCondition.LoadSCFile(txtPresetImgFile.Text))
                {
                    //失敗した場合メッセージを表示
                    //   指定されたスキャン条件ファイルからスキャン条件を設定できませんでした。
                    MessageBox.Show(StringTable.BuildResStr(9951, StringTable.IDS_CondFile), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            //それ以外
            else
            {
                return;
            }

			//スキャン条件の内容を画面に反映
			LoadScanCondition();

            //追加2014/10/07hata_v19.51反映
            //今のスキャンモード     'v18.00追加 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            nowScanmode = modLibrary.GetOption(optScanMode);
            //今のWスキャン //Rev25.00 追加 by長野 2016/07/11
            now_W_ScanCheckState = chkW_Scan.CheckState;

            //シフトスキャン⇔非シフトスキャン間の変更をしたとき 'v18.00追加 byやまおか 2011/07/23
            //Wスキャンを条件に追加 Rev25.00 by長野 2016/07/11
            if ((((oldScanmode == (int)ScanSel.ScanModeConstants.ScanModeShift) && (nowScanmode != (int)ScanSel.ScanModeConstants.ScanModeShift)) ||
                ((oldScanmode != (int)ScanSel.ScanModeConstants.ScanModeShift) && (nowScanmode == (int)ScanSel.ScanModeConstants.ScanModeShift))) ||
                (old_W_ScanCheckState != now_W_ScanCheckState))
            {

                //オートセンタリングをありにする
                chkInhibit[3].CheckState = System.Windows.Forms.CheckState.Unchecked;
                //★★★フラットパネルの幾何歪   'v18.00追加 byやまおか 2011/07/09
                if (CTSettings.detectorParam.Use_FlatPanel)
                    ScanCorrect.FPD_DistorsionCorrect();
            
            }

        }


        //*************************************************************************************************
        //機　　能： スキャンモード（シングルスキャン・コーンビームスキャン）オプションボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        private void optDataMode_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton r = sender as RadioButton;

            if (r == null) return;

            //unCheckのときは何もしない
            if (!r.Checked) return;

            //int Index = Array.IndexOf(optDataMode, sender);
            int Index = Array.IndexOf(optDataMode, r);

            //unCheckのときは何もしない
            //if (!optDataMode[Index].Checked) return;  

            if (Index < 0) return;

            //透視タブ内の「表示項目」のデータ収集エリア名称を切り替える 'v14.00追加 byやまおか 2007/07/06
            //   シングルスキャン時　：スライス厚
            //   コーンビームスキャン：コーン再構成エリア
            chkDspSW.Text = CTResources.LoadResString(Index == (int)ScanSel.DataModeConstants.DataModeCone ? 12286 : StringTable.IDS_SliceWidth);

            //最新scansel取得
            //modScansel.GetScansel(ref CTSettings.scansel.Data);
            CTSettings.scansel.Load();

            CTSettings.scansel.Data.data_mode = Index;

            //マトリクスの調整
            if ((Index == (int)ScanSel.DataModeConstants.DataModeScan) && (CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize256))
            {
                //メッセージ表示：
                //   シングルスキャンではマトリクスサイズ=256には対応していません。
                //   強制的にマトリクスサイズ=512に変更します。
                MessageBox.Show(StringTable.GetResString(15202, optDataMode[Index].Text, "256", "512"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                CTSettings.scansel.Data.matrix_size = (int)ScanSel.MatrixSizeConstants.MatrixSize512;
            }
            //Rev20.00 コメントアウト by長野 2015/01/16
            //4096対応のためコメントアウト by 長野 v16.10 10/01/29
            //ElseIf (Index = DataModeCone) And (.matrix_size = MatrixSize2048) Then                
            //else if ((Index == (int)ScanSel.DataModeConstants.DataModeCone) &&
            //         ((CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize2048) ||
            //          (CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize4096)))
            //else if ((Index == (int)ScanSel.DataModeConstants.DataModeCone) &&
            //         ((CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize4096)))
            //{
            //    //メッセージ表示：
            //    //コーンビームスキャンではマトリクスサイズ=2048以上には対応していません。
            //    //強制的にマトリクスサイズ=1024に変更します。
            //    MessageBox.Show(StringTable.GetResString(15212, optDataMode[(int)ScanSel.DataModeConstants.DataModeCone].Text, "2048", "1024"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    CTSettings.scansel.Data.matrix_size = (int)ScanSel.MatrixSizeConstants.MatrixSize1024;
            //}
            //Rev20.01 修正 by長野 2015/05/19
            else if ((Index == (int)ScanSel.DataModeConstants.DataModeCone) &&
                     ((CTSettings.scaninh.Data.cone_matrix[3] == 1 && CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize2048)))
            {
                //メッセージ表示：
                //コーンビームスキャンではマトリクスサイズ=2048以上には対応していません。
                //強制的にマトリクスサイズ=1024に変更します。
                MessageBox.Show(StringTable.GetResString(15212, optDataMode[(int)ScanSel.DataModeConstants.DataModeCone].Text, "2048", "1024"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                CTSettings.scansel.Data.matrix_size = (int)ScanSel.MatrixSizeConstants.MatrixSize1024;
            }
            else if ((Index == (int)ScanSel.DataModeConstants.DataModeCone) &&
                    ((CTSettings.scaninh.Data.cone_matrix[4] == 1 && CTSettings.scansel.Data.matrix_size == (int)ScanSel.MatrixSizeConstants.MatrixSize4096)))
            {
                //メッセージ表示：
                //コーンビームスキャンではマトリクスサイズ=4096以上には対応していません。
                //強制的にマトリクスサイズ=1024に変更します。
                MessageBox.Show(StringTable.GetResString(15212, optDataMode[(int)ScanSel.DataModeConstants.DataModeCone].Text, "4096", "1024"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                CTSettings.scansel.Data.matrix_size = (int)ScanSel.MatrixSizeConstants.MatrixSize1024;
            }


            //コーンビームスキャンから通常のスキャンに切り替えたときは、
            //「スキャン中再構成を行う」に強制的に戻す  'V3.0 append by 鈴山 2000/09/26
            if ((Index == (int)ScanSel.DataModeConstants.DataModeScan) &&
                (ActiveControl == optDataMode[(int)ScanSel.DataModeConstants.DataModeScan]) &&
                (CTSettings.scansel.Data.scan_and_view == 0))
            {
                //メッセージ表示：
                //   コーンビームスキャンから通常のスキャンに切り替えたときは、
                //   強制的に「スキャン中再構成を行う」に設定します。
                MessageBox.Show(CTResources.LoadResString(15203), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                CTSettings.scansel.Data.scan_and_view = 1;
            }

            //スキャンモードを切り替えたとき、スライスプランが選ばれていたらscaninhibitで有効/無効を確認する。
            //無効の場合は強制的にシングルに戻す。   'v17.43追加 byやまおか 2011/01/30
            if (((Index == (int)ScanSel.DataModeConstants.DataModeCone) &&
                    (ActiveControl == optDataMode[(int)ScanSel.DataModeConstants.DataModeCone]) &&
                    (CTSettings.scansel.Data.multiscan_mode == (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan) &&
                    (CTSettings.scaninh.Data.cone_multiscan_mode[2] != 0)) ||
                ((Index == (int)ScanSel.DataModeConstants.DataModeScan) &&
                    (ActiveControl == optDataMode[(int)ScanSel.DataModeConstants.DataModeScan]) &&
                    (CTSettings.scansel.Data.multiscan_mode == (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan) &&
                    (CTSettings.scaninh.Data.multiscan_mode[2] != 0)))
            {
                //メッセージ表示：
                //   コーンビーム or スキャンではスライスプランに対応していません。強制的にシングルに変更します。
                //MessageBox.Show(StringTable.GetResString(15213, optDataMode[Index].Text, CTResources.LoadResString(StringTable.IDS_SlicePlan)),
                //                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //CTSettings.scansel.Data.multiscan_mode = (int)ScanSel.MultiScanModeConstants.MultiScanModeSingle;
                //Rev23.40 変更 by長野 2016/06/19
                MessageBox.Show(StringTable.GetResString(15213, optDataMode[Index].Text, CTResources.LoadResString(StringTable.IDS_SlicePlan), CTResources.LoadResString(10500)),
                   Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                CTSettings.scansel.Data.multiscan_mode = (int)ScanSel.MultiScanModeConstants.MultiScanModeSingle;

            }

            //If False Then    'v16.2 連続回転コーンありの場合はFalse、なしの場合はTrueに変更する（コモン追加するまで） by山影 2010/01/19
            //        If True Then    'v16.3 とりあえず連続回転は無効にする byやまおか 2010/05/25
            //            '外部トリガ不可・コーンビームの場合、強制的にステップ回転とする 'v9.0 added by 間々田 2004/06/16
            //            If (Index = DataModeCone) And (scaninh.ext_trig <> 0) And (.table_rotation = 1) Then
            //                'メッセージ表示：外部トリガ不可・コーンビームの場合、強制的にステップ回転とします。
            //                MsgBox LoadResString(9521), vbExclamation
            //                .table_rotation = 0
            //            End If
            //        End If

            if (true) 		//v16.3 とりあえず連続回転は無効にする byやまおか 2010/05/25
            {
                //外部トリガ不可・コーンビームの場合、強制的にステップ回転とする 'v9.0 added by 間々田 2004/06/16
                //17.40 条件を追加 by 長野 2010/10/21
                if ((Index == (int)ScanSel.DataModeConstants.DataModeCone) &&
                    (CTSettings.scaninh.Data.ext_trig != 0) &&
                    (CTSettings.scansel.Data.table_rotation == 1) &&
                    (CTSettings.scaninh.Data.smooth_rot_cone == 1))
                {
                    //メッセージ表示：外部トリガ不可・コーンビームの場合、強制的にステップ回転とします。
                    MessageBox.Show(CTResources.LoadResString(9521), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    CTSettings.scansel.Data.table_rotation = 0;
                }
            }

            //scansel書き換え
            //modScansel.PutScansel(ref CTSettings.scansel.Data);
            CTSettings.scansel.Write();

            //ノーマル準備完了/コーン準備完了でデータモードが切り替えられた時、
            //オートセンタリングをオンにする                     v11.2追加 by 間々田 2006/01/19

            if ((Index == (int)ScanSel.DataModeConstants.DataModeScan) &&
                (ActiveControl == optDataMode[(int)ScanSel.DataModeConstants.DataModeScan]) &&
                (lblStatus3.Text == StringTable.GC_STS_CONE_OK))      //ｺｰﾝ準備完了
            {
                //.auto_centering = 1
                chkInhibit3.CheckState = CheckState.Unchecked;		//オートセンタリング

            }
            else if ((Index == (int)ScanSel.DataModeConstants.DataModeCone) &&
                     (ActiveControl == optDataMode[(int)ScanSel.DataModeConstants.DataModeCone]) &&
                     (lblStatus3.Text == StringTable.GC_STS_NORMAL_OK))   //ﾉｰﾏﾙ準備完了
            {
                //.auto_centering = 1
                chkInhibit3.CheckState = CheckState.Unchecked;		//オートセンタリング
            }

            //    'ステータス画面の「スキャン」「コーンビーム」の切り替え
            //    frmStatus.UpdateDataMode

            //    'マトリクスサイズ表示の更新
            //    frmCTMenu.UpdateMatrixSize
            //
            //    '保存画像サイズの計算
            //    Cal_SaveImageSize

            //    '透視画像フォームが表示されている場合、スライスライン位置を更新する
            //    If IsExistForm(frmTransImage) Then
            //        frmTransImage.SetLine
            //    End If

            //frmScanCondition.Setup

            //Zの場合 by井上 2017/12/18
            if (CTSettings.scaninh.Data.guide_mode == 1)
            {
                int QualityIndex = modLibrary.GetOption(optQuality);

                switch (QualityIndex)
                {
                    case (int)ScanQualityConstants.ScanQualityQuick:
                    case (int)ScanQualityConstants.ScanQualityRough:
                    case (int)ScanQualityConstants.ScanQualityNormal:
                    case (int)ScanQualityConstants.ScanQualityFine:
                        //optQuality_Click(optQuality[QualityIndex], new System.EventArgs());
                        optQuality_CheckedChanged(optQuality[QualityIndex], new System.EventArgs());
                        break;

                    default:

                        //frmScanCondition.Instance.ShowInTaskbar = false;
                        //frmScanCondition.Instance.WindowState = FormWindowState.Minimized;
                        //Rev21.00 追加 by長野 2015/03/09
                        if ((Index != (int)ScanSel.DataModeConstants.DataModeScano))
                        {
                            frmScanCondition.Instance.Setup();
                        }
                        else
                        {
                            frmScanoCondition.Instance.Setup();
                        }
                        //frmScanoCondition.Instance.Setup();

                        break;
                }
            }

            if ((Index != (int)ScanSel.DataModeConstants.DataModeScano))
            {
                frmScanCondition.Instance.Setup();
            }
            else
            {
                frmScanoCondition.Instance.Setup();
            }
        }

        //追加2014/10/07hata_v19.51反映
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから
        //*************************************************************************************************
        //機　　能： スキャンモード（ハーフ、フル、オフセット、シフト）オプションボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v18.00  2011/02/09  やまおか    新規作成
        //*************************************************************************************************
        public void optScanMode_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton r = sender as RadioButton;

            if (r == null) return;

            int Index = Array.IndexOf(optScanMode, r); 
            
            if (r.Checked)
            {
                //最新scansel取得
                //modScansel.GetScansel(ref modScansel.scansel);
                CTSettings.scansel.Load();

                int oldIndex = 0;
                //クリックする前のscan_mode  'v18.00追加 byやまおか 2011/07/05

                //前のスキャンモードをセット     'v18.00追加 byやまおか 2011/07/05
                oldIndex = CTSettings.scansel.Data.scan_mode;

                //スキャンモードをセット         'v18.00追加 byやまおか 2011/07/05
                CTSettings.scansel.Data.scan_mode = Index;

                //Rev26.00 ハーフ、フル⇔オフセット(シフト)の場合、かつ、[ガイド]タブでエリア指定済みの場合はフラグを落とす。by chouno 2017/01/16
                if (oldIndex != Index)
                {
                    if (Index == (int)ScanSel.ScanModeConstants.ScanModeFull || Index == (int)ScanSel.ScanModeConstants.ScanModeHalf)
                    {
                        if (oldIndex == (int)ScanSel.ScanModeConstants.ScanModeOffset || oldIndex == (int)ScanSel.ScanModeConstants.ScanModeShift)
                        {
                            scanAreaSetCmpFlg = false;
                        }
                    }
                    else
                    {
                        if (oldIndex == (int)ScanSel.ScanModeConstants.ScanModeFull || oldIndex == (int)ScanSel.ScanModeConstants.ScanModeHalf)
                        {
                            scanAreaSetCmpFlg = false;
                        }
                    }
                }

                //シフトスキャン⇔非シフトスキャン間の変更をしたとき 'v18.00追加 byやまおか 2011/07/15
                if (((oldIndex == (int)ScanSel.ScanModeConstants.ScanModeShift) & (Index != (int)ScanSel.ScanModeConstants.ScanModeShift)) |
                    ((oldIndex != (int)ScanSel.ScanModeConstants.ScanModeShift) & (Index == (int)ScanSel.ScanModeConstants.ScanModeShift)))
                {
                    //オートセンタリングをありにする
                    chkInhibit[3].CheckState = System.Windows.Forms.CheckState.Unchecked;
                    //★★★フラットパネルの幾何歪   'v18.00追加 byやまおか 2011/07/09
                    if (CTSettings.detectorParam.Use_FlatPanel)
                        ScanCorrect.FPD_DistorsionCorrect();
                }

                //シフトスキャンの場合、強制的にステップ回転とする
                if ((Index == (int)ScanSel.ScanModeConstants.ScanModeShift) & (CTSettings.scansel.Data.table_rotation == 1) & (CTSettings.scaninh.Data.smooth_rot_cone == 1))
                {
                    //メッセージ表示：シフトスキャンの場合、強制的にステップ回転とします。
                    //Interaction.MsgBox(CT30K.My.Resources.str9522, MsgBoxStyle.Exclamation);
                    MessageBox.Show(CTResources.LoadResString(9522), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    CTSettings.scansel.Data.table_rotation = 0;
                }

                //Wスキャンはステップ回転のみ v25.01 追加 by長野 2016/12/16 --->
                if (CTSettings.scansel.Data.w_scan == 1 && ((CTSettings.scansel.Data.table_rotation == 1) & (CTSettings.scaninh.Data.smooth_rot_cone != 0)))
                {
                    //メッセージ表示：Wスキャンの場合、強制的にステップ回転とします。
                    MessageBox.Show(CTResources.LoadResString(25008), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    CTSettings.scansel.Data.table_rotation = 0;
                }
                //<---

                //    With frmScanCondition
                //
                //        'スキャンエリア最大値の更新
                //        .UpdateScanAreaMax
                //
                //        'スキャンモードを変えたら必ず最大スキャンエリアとする
                //        .cwneArea.Value = .cwneArea.Maximum
                //
                //'ハーフのオートセンタリングができないバージョンの場合
                //#If NoHalfAutoCentering Then
                //
                //        'オートセンタリングを選択可・不可を設定する
                //        AutoCenteringEnabled
                //
                //#End If
                //
                //        'コーン時：データ収集ビュー数の更新
                //        If (scansel.data_mode = DataModeCone) Then .UpdateAcqView
                //
                //        'KSWの更新
                //        If (scansel.data_mode = DataModeCone) Then .UpdateKSW
                //
                //   End With

                //上記処理を関数化        'v18.00変更 byやまおか 2011/02/11
                //スキャンモード変更による値の更新
                frmScanCondition.Instance.UpdateCondition();

                //scansel書き換え
                //modScansel.PutScansel(ref modScansel.scansel);
                CTSettings.scansel.Write();
 
                //スキャン条件設定
                //frmScanCondition.Setup
                //スキャン条件画面から更新されたときはSetupしない    'v18.00変更 byやまおか 2011/02/11
                //変更2014/10/28hata_起動時に呼び出してしまうため
                //if (!frmScanCondition.Instance.Visible)   
                if (!frmScanCondition.Instance.Visible & modCT30K.CT30KSetup)
                    frmScanCondition.Instance.Setup();

            }
        }
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここまで

        //*******************************************************************************
        //機　　能： Wスキャンチェックボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V25.00  16/07/11   (検S1)長野   新規作成
        //*******************************************************************************
        private void chkW_Scan_CheckedChanged(object sender, EventArgs e)
        {
            if (sender as CheckBox == null) return;

            //CheckState chkS_Scan_oldCheckState = (CTSettings.scansel.Data.s_scan == 1 ? CheckState.Checked : CheckState.Unchecked);

            //通常スキャン⇔Wスキャン間の変更をしたときは、オートセンタリングON
            if (chkW_Scan.CheckState != (CTSettings.scansel.Data.w_scan == 1 ? CheckState.Checked : CheckState.Unchecked))
            {
                chkInhibit[3].CheckState = System.Windows.Forms.CheckState.Unchecked;
                //★★★フラットパネルの幾何歪   'v18.00追加 byやまおか 2011/07/09
                if (CTSettings.detectorParam.Use_FlatPanel)
                    ScanCorrect.FPD_DistorsionCorrect();

                //Rev26.00 [ガイド]タブのエリア指定完了フラグを落とす add by chouno 2017/01/16 
                scanAreaSetCmpFlg = false;
            }

            //前のWスキャンモードを記憶
            //chkS_Scan_oldCheckState = chkS_Scan.CheckState;

            //Wスキャンはステップ回転のみ   
            if (chkW_Scan.CheckState == CheckState.Checked)
            {
                //Rev25.01 by長野 2016/08/22
                if (CTSettings.scansel.Data.table_rotation == 1)
                {
                    //メッセージ表示：Wスキャンの場合、強制的にステップ回転とします。
                    MessageBox.Show(CTResources.LoadResString(25008), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    CTSettings.scansel.Data.table_rotation = 0;
                }
            }

            //コントロールの値をコモンにセーブする。
            CTSettings.scansel.Data.w_scan = (chkW_Scan.CheckState == CheckState.Checked) ? 1 : 0;

            //scansel書き換え
            //modScansel.PutScansel(ref modScansel.scansel);
            CTSettings.scansel.Write();

            //上記処理を関数化        'v18.00変更 byやまおか 2011/02/11
            //スキャンモード変更による値の更新
            frmScanCondition.Instance.UpdateCondition();

            //スキャン条件設定
            //frmScanCondition.Setup
            //スキャン条件画面から更新されたときはSetupしない    'v18.00変更 byやまおか 2011/02/11
            //変更2014/10/28hata_起動時に呼び出してしまうため
            //if (!frmScanCondition.Instance.Visible)   
            if (!frmScanCondition.Instance.Visible & modCT30K.CT30KSetup)
                frmScanCondition.Instance.Setup();

        }

        //*************************************************************************************************
        //機　　能： 「生データをセーブする」チェックボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void chkSave_Click(object sender, EventArgs e)
		{
			//コーンビーム時
			if (optDataMode4.Checked)
            {
				//コーン分散処理有効時は、生データ保存ありを選択できない
				//If optConeDistribute(1).Value And (chkSave.Value = vbChecked) Then			
				if ((CTSettings.scansel.Data.cone_distribute == 1) && 
                    (chkSave.CheckState == CheckState.Checked) && 
                    (CTSettings.scaninh.Data.cone_distribute == 0)) 	//v10.0変更 by 間々田 2005/02/02 コーン分散処理２の場合、設定できるようにした
                {
					//メッセージ表示：コーン分散処理有効時は、生データ保存ありを選択できません。強制的に「生データ保存なし」に設定します。
                    MessageBox.Show(CTResources.LoadResString(12919), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					chkSave.CheckState = CheckState.Unchecked;
				}				
			} 
            //コーンビーム時以外：スキャン中再構成を行わない場合は必ず生データを保存する
            else if ((CTSettings.scansel.Data.scan_and_view == 0) && (chkSave.CheckState == CheckState.Unchecked)) 
            {
				//メッセージ表示：
				//   スキャン中再構成を行わない場合は強制的に生データが保存されます。
				//   生データを保存しない場合は、先にスキャン中再構成を行うに設定してください。
                MessageBox.Show(CTResources.LoadResString(9411), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				//強制的にチェックにして抜ける
				chkSave.CheckState = CheckState.Checked;
			}

			if (ActiveControl == chkSave)
            {
				//scansel（コモン）取得
				//modScansel.GetScansel(ref CTSettings.scansel.Data);
                CTSettings.scansel.Load();

				//生データをセーブする
                CTSettings.scansel.Data.rawdata_save = (chkSave.CheckState == CheckState.Checked) ? 1 : 0;

				//scansel（コモン）書き込み
				//modScansel.PutScansel(ref CTSettings.scansel.Data);
                CTSettings.scansel.Write();
			}
        }

        //*************************************************************************************************
        //機　　能： 「純生データをセーブする」チェックボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.12  15/12/11   (検S1)長野   新規作成
        //*************************************************************************************************
        private void chkPurSave_Click(object sender, EventArgs e)
        {
            //コーンビーム時
            if (CTSettings.scansel.Data.table_rotation == 1)
            {
                //連続時は不可。
                //メッセージ表示：テーブル連続回転時は、純生データ保存ありを選択できません。強制的に「純生データ保存なし」に設定します。
                MessageBox.Show(CTResources.LoadResString(24004), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                chkPurSave.CheckState = CheckState.Unchecked;
            }

            if (ActiveControl == chkPurSave)
            {
                //scansel（コモン）取得
                //modScansel.GetScansel(ref CTSettings.scansel.Data);
                CTSettings.scansel.Load();

                //生データをセーブする
                CTSettings.scansel.Data.pur_rawdata_save = (chkPurSave.CheckState == CheckState.Checked) ? 1 : 0;

                //scansel（コモン）書き込み
                //modScansel.PutScansel(ref CTSettings.scansel.Data);
                CTSettings.scansel.Write();
            }
        }

        //*************************************************************************************************
        //機　　能： 画像保存先：ディレクトリ名・変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void txtImgDir_TextChanged(object sender, EventArgs e)
		{
			//全角文字を２文字とみなした文字数チェック   by 間々田 2003/11/18
			modCT30K.ChangeTextBox(txtImgDir);

			//ディレクトリ名に禁止文字が含まれている     'v16.20/v17.00追加 byやまおか 2010/03/02
			if (!modLibrary.DirNameProhibitionCheck(txtImgDir.Text)) 
            {
				//メッセージ表示：
				//   スライス名に以下の禁止文字を使用しないでください。
				//   . \ / : * ? < > | " Space
                MessageBox.Show(CTResources.LoadResString(9422) + "\r" + "\r" + "/ : * ? < > | ", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
			}

			//保存先のドライブ名更新
			//modFileIO.MyDrive = modFileIO.FSO.GetDriveName(txtImgDir.Text);
            AppValue.MyDrive = Path.GetPathRoot(txtImgDir.Text);

			//scansel（コモン）取得
			//modScansel.GetScansel(ref CTSettings.scansel.Data);
            CTSettings.scansel.Load();

			//画像保存先
            //modLibrary.SetField(modLibrary.AddExtension(txtImgDir.Text, "\\"), ref CTSettings.scansel.Data.pro_code);    //ディレクトリ名：末尾に\を付加
            CTSettings.scansel.Data.pro_code.SetString(modLibrary.AddExtension(txtImgDir.Text, "\\"));

			//scansel（コモン）書き込み
			//modScansel.PutScansel(ref CTSettings.scansel.Data);
            CTSettings.scansel.Write();
        }


        //*************************************************************************************************
        //機　　能： 画像保存先：スライス名・変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void txtImgFile_TextChanged(object sender, EventArgs e)
		{
			//全角文字を２文字とみなした文字数チェック   by 間々田 2003/11/18
			modCT30K.ChangeTextBox(txtImgFile);

			//スライス名に禁止文字が含まれている     'v16.20/v17.00追加 byやまおか 2010/03/02
			if (!modLibrary.FileNameProhibitionCheck(txtImgFile.Text)) 
            {
				//メッセージ表示：
				//   スライス名に以下の禁止文字を使用しないでください。
				//   . \ / : * ? < > | " Space
                MessageBox.Show(CTResources.LoadResString(9421) + "\r" + "\r" + ". \\ / : * ? < > | " + (char)34 + " Space",
                                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);                
                return;
			}
        }


        //*************************************************************************************************
        //機　　能： 画像付帯情報コメント欄編集終了時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void txtImgFile_Validating(object sender, CancelEventArgs e)
		{
			//スライス名を指定していない
            if(string.IsNullOrEmpty(txtImgFile.Text.Trim()))
            {
				//メッセージ表示：スライス名が指定されていません。
                MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_NotSpecified, StringTable.IDS_SliceName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
			}           
  			//スライス名に禁止文字が含まれている          
            else if (!modLibrary.FileNameProhibitionCheck(txtImgFile.Text)) 
            {
				//メッセージ表示：
				//   スライス名に以下の禁止文字を使用しないでください。
				//   \ / : * ? < > | " Space
                MessageBox.Show(CTResources.LoadResString(9421) + "\r" + "\r" + "\\ / : * ? < > | " + (char)34 + " Space", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
			}

            //TODO If Not scansel Then
#region
/*     
            If Not scansel Then

                'scansel（コモン）取得
                GetScansel scansel

                '画像保存先
                SetField txtImgFile.Text, scansel.pro_name
                                
                'scansel（コモン）書き込み
                PutScansel scansel

            End If
*/
#endregion
        
			//scansel（コモン）取得
			//modScansel.GetScansel(ref CTSettings.scansel.Data);
            CTSettings.scansel.Load();

			//画像保存先
			//modLibrary.SetField(txtImgFile.Text, ref CTSettings.scansel.Data.pro_name);
            CTSettings.scansel.Data.pro_name.SetString(txtImgFile.Text);

			//scansel（コモン）書き込み
			//modScansel.PutScansel(ref CTSettings.scansel.Data);
            CTSettings.scansel.Write();
        }


        //*************************************************************************************************
        //機　　能： 参照ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdImgSelect_Click(object sender, EventArgs e)
		{
			string FileName = null;

			//ダイアログ表示
            FileName = modFileIO.GetFileName(StringTable.IDS_FileSpecify,
                                             CTResources.LoadResString(StringTable.IDS_CTImage),
                                             ".img",
                                             InitFileName: Path.Combine(txtImgDir.Text, txtImgFile.Text),
                                             Purpose: fraImgSave.Text);


            if (!string.IsNullOrEmpty(FileName))
            {

#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //Rドライブへの保存を禁止する    'v17.40追加 byやまおか 2010/10/26
			//    If UseRamDisk And (StrComp("R:", Left$(FSO.GetParentFolderName(FileName), 2)) = 0) Then
			//        'メッセージ表示：
			//        '   ディレクトリ名に以下の禁止文字を使用しないでください。
			//        '   R:
			//        MsgBox LoadResString(9422) & vbCr & vbCr & "R:\" & chr(34), vbCritical
			//        Exit Sub
			//    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

                //ディレクトリ名に禁止文字が含まれている     'v16.20/v17.00追加 byやまおか 2010/03/02
                //if (!modLibrary.DirNameProhibitionCheck(modFileIO.FSO.GetParentFolderName(FileName)))
                if (!modLibrary.DirNameProhibitionCheck(Path.GetDirectoryName(FileName)))
                {
                    //メッセージ表示：
                    //   スライス名に以下の禁止文字を使用しないでください。
                    //   . \ / : * ? < > | " Space
                    MessageBox.Show(CTResources.LoadResString(9422) + "\r" + "\r" + "/ : * ? < > | " + (char)34, 
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

			    //スライス名に禁止文字が含まれている     'v16.20/v17.00追加 byやまおか 2010/03/02
                if (!modLibrary.FileNameProhibitionCheck(Path.GetFileNameWithoutExtension(FileName)))
                {
                    //メッセージ表示：
                    //   スライス名に以下の禁止文字を使用しないでください。
                    //   . \ / : * ? < > | " Space
                    MessageBox.Show(CTResources.LoadResString(9421) + "\r" + "\r" + ". \\ / : * ? < > | " + (char)34 + " Space", 
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //画像保存先に設定
                //txtImgDir.Text = modFileIO.FSO.GetParentFolderName(FileName);       //ディレクトリ名
                txtImgDir.Text = Path.GetDirectoryName(FileName);       //ディレクトリ名
                txtImgFile.Text = modFileIO.GetSliceBaseName(FileName);				//スライス名
                bool dummy = false;
                txtImgFile_Validating(txtImgFile, new System.ComponentModel.CancelEventArgs(dummy));	//scanselに書き込む
            }
        }


        //*************************************************************************************************
        //機　　能： 画像付帯情報コメント欄キー入力処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void txtComment_KeyPress(object sender, KeyPressEventArgs e)
		{
            //リタ―ンキーを無視する
            switch (e.KeyChar)
            {
                case (char)Keys.Return:
                    e.KeyChar = (char)0;
                    e.Handled = true;
                    break;
            }
        }


        //*************************************************************************************************
        //機　　能： 画像付帯情報コメント欄変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void txtComment_TextChanged(object sender, EventArgs e)
		{
			//全角文字を２文字とみなした文字数チェック   by 間々田 2003/11/18
			modCT30K.ChangeTextBox(txtComment);
        }


        //*************************************************************************************************
        //機　　能： 画像付帯情報コメント欄編集終了時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void txtComment_Validating(object sender, CancelEventArgs e)
		{
			//scansel（コモン）取得
			//modScansel.GetScansel(ref CTSettings.scansel.Data);
            CTSettings.scansel.Load();

			//画像コメント
			//modLibrary.SetField(txtComment.Text, ref CTSettings.scansel.Data.comment);
            CTSettings.scansel.Data.comment.SetString(txtComment.Text);

			//scansel（コモン）書き込み
			//modScansel.PutScansel(ref CTSettings.scansel.Data);
            CTSettings.scansel.Write();
        }


        //*************************************************************************************************
        //機　　能： 「詳細...」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdConditionDetails_Click(object sender, EventArgs e)
		{
			//いったんこのフォームの設定値をscansel（コモン）に保存
			//If Not SaveScanCondition() Then Exit Sub   '削除 by 間々田 2009/06/16

			//スキャン条件のフォームを表示
			//frmScanCondition.Show , frmCTMenu

            ////2014/07/04追加(検S1)hata
            ////スキャン条件formが開いていたら閉じる
            //if (modLibrary.IsExistForm("frmScanCondition")) frmScanCondition.Instance.Close();
            //Application.DoEvents();

            ////スキャン条件設定   v15.0変更 by 間々田 2009/06/17
            ////frmScanCondition.Instance.WindowState = FormWindowState.Normal;
            //frmScanCondition.Instance.Setup(true);
            //Rev21.00 条件式追加 by長野 2015/2/19
            if (optDataMode1.Checked == true || optDataMode4.Checked == true)
            {
                //2014/07/04追加(検S1)hata
                //スキャン条件formが開いていたら閉じる
                if (modLibrary.IsExistForm("frmScanCondition")) frmScanCondition.Instance.Close();
                Application.DoEvents();

                //スキャン条件設定   v15.0変更 by 間々田 2009/06/17
                //frmScanCondition.Instance.WindowState = FormWindowState.Normal;
                frmScanCondition.Instance.Setup(true);
            }
            else if (optDataMode3.Checked == true)
            {
                //Rev21.00 2015/02/19 追加(検S1)長野
                //スキャノ条件formが開いていたら閉じる
                if (modLibrary.IsExistForm("frmScanoCondition")) frmScanoCondition.Instance.Close();
                Application.DoEvents();

                //スキャン条件設定   v15.0変更 by 間々田 2009/06/17
                //frmScanCondition.Instance.WindowState = FormWindowState.Normal;
                frmScanoCondition.Instance.Setup(true);
            }
        }


        //*************************************************************************************************
        //機　　能： スキャン条件タブ内の設定値のロード
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*************************************************************************************************
		public void LoadScanCondition()
		{
			//scansel（コモン）取得
			//modScansel.GetScansel(ref CTSettings.scansel.Data);
            CTSettings.scansel.Load();

			//画像保存先
            //txtImgDir.Text = modFileIO.FSO.GetParentFolderName(modFileIO.FSO.BuildPath(CTSettings.scansel.Data.pro_code, "*"));	//ディレクトリ名
            txtImgDir.Text = Path.GetDirectoryName(Path.Combine(CTSettings.scansel.Data.pro_code.GetString(), "*"));	//ディレクトリ名
			txtImgFile.Text = CTSettings.scansel.Data.pro_name.GetString();			//スライス名

			//画像コメント
			txtComment.Text = CTSettings.scansel.Data.comment.GetString();

			//生データ保存
			if (CTSettings.scaninh.Data.raw_save == 0) 
            {
				fraOrgSave.Visible = true;
				chkSave.CheckState = (CTSettings.scansel.Data.rawdata_save == 1 ? CheckState.Checked : CheckState.Unchecked);
			}

            //Rev23.12 追加 by長野 --->
            if (CTSettings.scaninh.Data.save_purdata == 0)
            {
                chkPurSave.CheckState = (CTSettings.scansel.Data.pur_rawdata_save == 1 ? CheckState.Checked : CheckState.Unchecked);
            }
            //<---

			//データモード：1（スキャン）,4(コーンビームスキャン)
            //modLibrary.SetOption(ref optDataMode, CTSettings.scansel.Data.data_mode, ScanSel.DataModeConstants.DataModeScan);
            modLibrary.SetOption(optDataMode, CTSettings.scansel.Data.data_mode, (int)ScanSel.DataModeConstants.DataModeScan);

			//オートセンタリングなしの場合は、校正タブの「回転中心校正」にチェックを入れる
			byEvent = false;
			chkInhibit3.CheckState = (CTSettings.scansel.Data.auto_centering == 0 ? CheckState.Checked : CheckState.Unchecked);
			byEvent = true;
        }


        //*************************************************************************************************
        //機　　能： スキャン条件タブ内の設定値の保存
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*************************************************************************************************
		public bool SaveScanCondition(bool CheckOn = true)
		{
			string DestNew = null;			//画像保存先（変更後）

			//戻り値初期化
			bool functionReturnValue = false;
            
			//チェックありの場合
			if (CheckOn) 
            {
				//ディレクトリが存在しない
				//if (!modFileIO.FSO.FolderExists(txtImgDir.Text)) 
                if (! Directory.Exists(txtImgDir.Text))
                {
					//メッセージ表示：指定された画像保存先のディレクトリがありません。
                    MessageBox.Show(CTResources.LoadResString(9505), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return functionReturnValue;
				}                
      			//スライス名を指定していない          
                else if (string.IsNullOrEmpty(txtImgFile.Text.Trim())) 
                {
					//メッセージ表示：スライス名が指定されていません。
                    MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_NotSpecified, StringTable.IDS_SliceName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return functionReturnValue;
				}

				//スライス名に禁止文字が含まれている
				if (!modLibrary.FileNameProhibitionCheck(txtImgFile.Text)) 
                {
					//メッセージ表示：
					//   スライス名に以下の禁止文字を使用しないでください。
					//   \ / : * ? < > | " Space
                    MessageBox.Show(CTResources.LoadResString(9421) + "\r" + "\r" 
                                    + "\\ / : * ? < > | " + (char)34 + " Space", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);                    
                    return functionReturnValue;
				}
			}

			//scansel（コモン）取得
			//modScansel.GetScansel(ref CTSettings.scansel.Data);
            CTSettings.scansel.Load();
			
            //データモード：1（スキャン）, 4(コーンビームスキャン)
			CTSettings.scansel.Data.data_mode = modLibrary.GetOption(optDataMode);

			//画像保存先
			//modLibrary.SetField(modLibrary.AddExtension(txtImgDir.Text, "\\"), ref CTSettings.scansel.Data.pro_code);	//ディレクトリ名：末尾に\を付加
			//modLibrary.SetField(txtImgFile.Text, ref CTSettings.scansel.Data.pro_name);			                        //スライス名
            CTSettings.scansel.Data.pro_code.SetString(modLibrary.AddExtension(txtImgDir.Text, "\\"));
            CTSettings.scansel.Data.pro_name.SetString(txtImgFile.Text);
			
            //変更後画像保存先を求めておく
            //DestNew = modFileIO.FSO.BuildPath(CTSettings.scansel.Data.pro_code, CTSettings.scansel.Data.pro_name);
            DestNew = Path.Combine(CTSettings.scansel.Data.pro_code.GetString(), CTSettings.scansel.Data.pro_name.GetString());

			//画像コメント
			//modLibrary.SetField(txtComment.Text, ref CTSettings.scansel.Data.Comment);
            CTSettings.scansel.Data.comment.SetString(txtComment.Text);
			
            //生データ保存：0(行わない),1(行う)
			if (CTSettings.scaninh.Data.raw_save == 0) 
            {
                CTSettings.scansel.Data.rawdata_save = (chkSave.CheckState == CheckState.Checked) ? 1 : 0;
			}

			//scansel（コモン）書き込み
			//modScansel.PutScansel(ref CTSettings.scansel.Data);
            CTSettings.scansel.Write();

#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //画像保存先が変更されている場合
			//    If UCase$(DestOld) <> UCase$(DestNew) Then
			//
			//        'scancondparの取得
			//        CallGetScancondpar
			//
			//        With scancondpar
			//
			//            '検査ＩＤをカウントアップ
			//            .study_id = .study_id + 1
			//
			//            'setfile 側もカウントアップ
			//            UpdateCsv FSO.BuildPath(pathSetFile, "DICOM.csv"), "study_id", CStr(.study_id)
			//
			//            'スキャン済みフラッグを未スキャンとする
			//            .scan_comp = 0
			//
			//            'シリーズ番号を１にリセット
			//            .series_num = 1
			//
			//            '収集番号を１にリセット
			//            .acq_num = 1
			//
			//        End With
			//
			//        'scancondparの書き込み
			//        CallPutScancondpar
			//
			//    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

            functionReturnValue = true;
			return functionReturnValue;
        }


        //*************************************************************************************************
        //機　　能： フラットパネル設定欄 更新処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.00  2010/02/18  やまおか    新規作成
        //*************************************************************************************************
        //Public Sub UpdateFpdGainInteg()
		private void UpdateFpdGainInteg()		//v17.50変更 by 間々田 2011/02/16
		{
            ntbFpdGain.Value = Convert.ToDecimal(modCT30K.GetFpdGainStr(CTSettings.scansel.Data.fpd_gain, CTSettings.t20kinf.Data.pki_fpd_type));			//FPDゲインの更新
			ntbFpdInteg.Value = Convert.ToDecimal(modCT30K.GetFpdIntegStr(CTSettings.scansel.Data.fpd_integ));		//FPD積分時間の更新
        }


        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //
        //   「校正」タブ内の処理
        //
        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //*************************************************************************************************
        //機　　能： 各校正ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdCorrect_Click(object sender, EventArgs e)
		{
			if (sender as Button == null) return;

            int Index = Array.IndexOf(cmdCorrect, sender);

			if (Index < 0) return;

			switch (Index)
            {

#region			//削除ここから by 間々田 2009/07/21 全自動校正以外のボタンは「校正－詳細」画面に移動
                //'「ゲイン校正」ボタン
				//Case 1: frmGainCor.Show , frmCTMenu             'ゲイン校正フォームを表示する
				//
				//'「スキャン位置校正」ボタン
				//Case 2: frmScanPositionEntry.Show , frmCTMenu   'スキャン位置校正フォームを表示する
				//
				//'「幾何歪校正」ボタン
				//Case 3: frmVertical.Show , frmCTMenu            '幾何歪校正フォームを表示する
				//
				//'「回転中心校正」ボタン
				//Case 4: frmRotationCenter.Show , frmCTMenu      '回転中心校正フォームを表示する
				//
				//'「オフセット校正」ボタン
				//Case 5: frmOffset.Show , frmCTMenu              'オフセット校正フォームを表示する
				//
				//'「寸法校正」ボタン
				//Case 6: frmDistanceCorrect.Show , frmCTMenu     '寸法校正フォームを表示する
				//
				//'「マルチスライス校正」ボタン
				//Case 7: frmMultiSlicePre.Show , frmCTMenu       'マルチスライス校正フォームを表示する
                //削除ここまで by 間々田 2009/07/21 全自動校正以外のボタンは「校正－詳細」画面に移動
#endregion

                //「全自動校正」ボタン
				//Case 8: frmAutoCorrection.Show vbModal          '自動校正フォームを表示する
				//Case 8: frmAutoCorrection.Show , frmCTMenu      '自動校正フォームを表示する 'v15.0変更 by 間々田 2009/06/15
				case 8:

                    //Rev26.00 add by chouno 2017/03/13
                    if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
                    {
                        return;
                    }

                    //Rev26.00 全自動校正中は、[ガイド]タブの設定完了状態は変更しない add by chouno 2017/01/16
                    setScanAreaAndCondIgnoreFlg(true);

                    frmAutoCorrection.Instance.ShowDialog(frmCTMenu.Instance);	//自動校正フォームを表示する 'v16.20/v17.00 Modal化 byやまおか 2010/02/10
                    frmAutoCorrection.Instance.Dispose();

                    //Rev26.00 全自動構成終了後は元に戻す
                    setScanAreaAndCondIgnoreFlg(false);

                    break;                    
			}
		}


        //*************************************************************************************************
        //機　　能： ゲイン校正が準備完了か？
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                                 True: 完了, False: 準備未完了
        //
        //補　　足： なし
        //
        //履　　歴： v11.2  2006/01/13   (SI3)間々田      新規作成
        //*************************************************************************************************
		public bool IsOkGain 
        {
			get { return (lblStatus0.Text == StringTable.GC_STS_STANDBY_OK); }
        }


        //*************************************************************************************************
        //機　　能： スキャン位置校正が準備完了か？
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                                 True: 完了, False: 準備未完了
        //
        //補　　足： なし
        //
        //履　　歴： v11.2  2006/01/13   (SI3)間々田      新規作成
        //*************************************************************************************************
		public bool IsOkScanPosition 
        {
			get { return (lblStatus1.Text != StringTable.GC_STS_STANDBY_NG); }
		}


        //*************************************************************************************************
        //機　　能： 幾何歪校正が準備完了か？
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                                 True: 完了, False: 準備未完了
        //
        //補　　足： なし
        //
        //履　　歴： v11.2  2006/01/13   (SI3)間々田      新規作成
        //*************************************************************************************************
		public bool IsOkVertical 
        {
			get { return (lblStatus2.Text != StringTable.GC_STS_STANDBY_NG); }
		}


        //*************************************************************************************************
        //機　　能： 回転中心校正が準備完了か？
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                                 True: 完了, False: 準備未完了
        //
        //補　　足： なし
        //
        //履　　歴： v11.2  2006/01/13   (SI3)間々田      新規作成
        //*************************************************************************************************
		public bool IsOkRotationCenter 
        {
            get
            {
                bool functionReturnValue = false;
                string status = lblStatus3.Text;

                //準備完了, ｵｰﾄｾﾝﾀﾘﾝｸﾞあり
                if (status == StringTable.GC_STS_STANDBY_OK || status == StringTable.GC_STS_AutoCentering)
                {
                    functionReturnValue = true;
                }
                //ﾉｰﾏﾙ準備完了
                else if (status == StringTable.GC_STS_NORMAL_OK)
                {
                    functionReturnValue = (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScan);
                }
                //ｺｰﾝ準備完了
                else if (status == StringTable.GC_STS_CONE_OK)
                {
                    functionReturnValue = (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone);
                }
                //準備未完了
                else
                {
                    functionReturnValue = false;
                }

                return functionReturnValue;
            }
		}


        //*************************************************************************************************
        //機　　能： オフセット校正が準備完了か？
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                                 True: 完了, False: 準備未完了
        //
        //補　　足： なし
        //
        //履　　歴： v11.2  2006/01/13   (SI3)間々田      新規作成
        //*************************************************************************************************
		public bool IsOkOffset 
        {
			get { return (lblStatus4.Text == StringTable.GC_STS_STANDBY_OK); }
		}


        //*************************************************************************************************
        //機　　能： 寸法校正が準備完了か？
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                                 True: 完了, False: 準備未完了
        //
        //補　　足： なし
        //
        //履　　歴： v11.2  2006/01/13   (SI3)間々田      新規作成
        //*************************************************************************************************
		public bool IsOkDistance 
        {
			get { return (lblStatus5.Text != StringTable.GC_STS_STANDBY_NG); }
		}


        //*************************************************************************************************
        //機　　能： 校正がすべて準備完了か？
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                                 True: 完了, False: 準備未完了
        //
        //補　　足： なし
        //
        //履　　歴： v11.2  2006/01/13   (SI3)間々田      新規作成
        //*************************************************************************************************
		public bool IsOkAll 
        {
            get
            {
                //戻り値初期化
                bool functionReturnValue = false;

                if (!IsOkGain) return functionReturnValue;                                          //ゲイン校正
                if (!IsOkScanPosition) return functionReturnValue;                                  //スキャン位置校正
                //    If Not IsOkVertical() Then Exit Property       '幾何歪校正
                if ((!IsOkVertical) && (!CTSettings.detectorParam.Use_FlatPanel)) return functionReturnValue;      //幾何歪校正 v16.20/v17.00 変更 by 山影 10-03-04
                if (!IsOkRotationCenter) return functionReturnValue;                                //回転中心校正
                if (!IsOkOffset) return functionReturnValue;                                        //オフセット校正
                if (!IsOkDistance) return functionReturnValue;                                      //寸法校正

                //戻り値初期化
                functionReturnValue = true;
                return functionReturnValue;
            }
		}
        

        //*************************************************************************************************
        //機　　能： チェックボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void chkInhibit_Click(object sender, EventArgs e)
		{
			if (sender as CheckBox == null) return;

			int Index = Array.IndexOf(chkInhibit, sender);

			if (Index < 0) return;

			int long_data = 0;

			//イベント以外の呼び出しは実行しない
			//If Not (Me.ActiveControl Is chkInhibit(Index)) Then Exit Sub
			if (!byEvent) return;

			//チェックボックスの状態を読み取る
			long_data = (chkInhibit[Index].CheckState == CheckState.Checked ? 0 : 1);

			//各校正のインヒビットを更新
            switch (Index)
            {
                case 1:
                    //スキャン位置校正
                    //modCommon.putcommon_long("mecainf", "scanpos_cor_inh", long_data);
                     ComLib.putcommon_long("mecainf", "scanpos_cor_inh", long_data);
                   
                    //スキャン位置校正ステータス（簡易表示）の更新（チェック対象のときだけ更新する）
                     frmCorrectionStatus.Instance.UpdateStatus(frmCorrectionStatus.Instance.lblItemSp,
                                                     ref lblStatus[Index],
                                                     (chkInhibit[Index].CheckState == CheckState.Unchecked ? StringTable.GC_STS_IGNORE : ""));
                    break;

                case 3:
                    //オートセンタリング
                    CTSettings.scansel.Data.auto_centering = long_data;
                    //modScansel.PutScansel(ref CTSettings.scansel.Data);
                    CTSettings.scansel.Write();
                
                
                    //回転中心校正ステータス（簡易表示）の更新（チェック対象のときだけ更新する）
                    frmCorrectionStatus.Instance.UpdateStatus(frmCorrectionStatus.Instance.lblItemRot,
                                                     ref lblStatus[Index],
                                                     (chkInhibit[Index].CheckState == CheckState.Unchecked ? StringTable.GC_STS_AutoCentering : ""));
                    break;

                case 5:
                    //寸法校正
                    ComLib.putcommon_long("mecainf", "distance_cor_inh", long_data);
                    //寸法校正ステータス（簡易表示）の更新（チェック対象のときだけ更新する）
                    frmCorrectionStatus.Instance.UpdateStatus(frmCorrectionStatus.Instance.lblItemDist,
                                                     ref lblStatus[Index],
                                                     (chkInhibit[Index].CheckState == CheckState.Unchecked ? StringTable.GC_STS_IGNORE : ""));
                    break;
            }
		}


        //*************************************************************************************************
        //機　　能： 簡易表示ステータス変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： ステータス文字列に基づき，表示色を設定します
        //
        //履　　歴： v11.2  2005/12/28 (SI3)間々田   新規作成
        //*************************************************************************************************
		private void lblStatus_TextChanged(object sender, EventArgs e)
		{
			if (sender as Label == null) return;

            int Index = Array.IndexOf(lblStatus, sender);

			if (Index < 0) return;

            string status = lblStatus[Index].Text;

            //対象外, ｵｰﾄｾﾝﾀﾘﾝｸﾞあり
            if (status == StringTable.GC_STS_IGNORE || status == StringTable.GC_STS_AutoCentering)
            {
                lblStatus[Index].BackColor = Color.Cyan;
            }
            //準備未完了
            else if(status == StringTable.GC_STS_STANDBY_NG)
            {
                //回転中心校正ステータスが準備完了→準備未完了に変化した場合
                if ((lblStatus[Index].BackColor == Color.Lime) && (Index == 3))
                {
                    //コーンのヘリカル以外で昇降位置が移動なしの時だけオートセンタリングをありに設定する
                    //If (Not ((scansel.data_mode = DataModeCone) And (scansel.inh = 1))) And (lblItemRot(1).BackColor = vbGreen) Then					
                    if (!((CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone) && (CTSettings.scansel.Data.inh == 1)))	    //v13.0変更 by 間々田 2007/04/16 昇降位置のチェックがなくなった
                    {
                        //オートセンタリング
                        chkInhibit3.CheckState = CheckState.Unchecked;
                        return;
                    }
                }
                lblStatus[Index].BackColor = Color.Yellow;
            }
            //準備完了, ﾉｰﾏﾙ準備完了, ｺｰﾝ準備完了
            else if(status == StringTable.GC_STS_STANDBY_OK || status == StringTable.GC_STS_NORMAL_OK || status == StringTable.GC_STS_CONE_OK)
            {
                //準備未完了→準備完了に変化した場合
                if (lblStatus[Index].BackColor == Color.Yellow)
                {
                    //校正ステータスが回転中心校正だけのために準備未完了となっている場合は、スキャン条件のオートセンタリングをありにする
                    if ((lblStatus3.Text == StringTable.GC_STS_STANDBY_NG) &&
                        (lblStatus0.Text != StringTable.GC_STS_STANDBY_NG) &&
                        (lblStatus1.Text != StringTable.GC_STS_STANDBY_NG) &&
                        (lblStatus2.Text != StringTable.GC_STS_STANDBY_NG) &&
                        (lblStatus4.Text != StringTable.GC_STS_STANDBY_NG) &&
                        (lblStatus5.Text != StringTable.GC_STS_STANDBY_NG))
                    {
                        //オートセンタリング
                        chkInhibit3.CheckState = CheckState.Unchecked;
                    }
                }
                lblStatus[Index].BackColor = Color.Lime;
            }

			lblStatus[Index].Refresh();

			//frmStatusの校正ステータスを更新する
			frmStatus.Instance.UpdateCorrectStatus();
		}


        //*************************************************************************************************
        //機　　能： 「詳細...」ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdCorrectDetails_Click(object sender, EventArgs e)
		{
			//校正ステータスフォームの表示
			//frmCorrectionStatus.Show , frmCTMenu

            //別Formにモーダルで表示する//
            frmCorrectionStatusModal.Instance.Text = frmCorrectionStatus.Instance.Text;
            frmCorrectionStatusModal.Instance.Size = new Size(frmCorrectionStatus.Instance.Width, frmCorrectionStatus.Instance.Height);
            frmCorrectionStatusModal.Instance.ClientSize = frmCorrectionStatus.Instance.pnlCorStatus.Size;
            //コントロールを移動
            frmCorrectionStatus.Instance.pnlCorStatus.Parent = frmCorrectionStatusModal.Instance;
            //モーダルで表示
            frmCorrectionStatusModal.Instance.ShowDialog(frmCTMenu.Instance);

        }


        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //
        //   「透視」タブ内の処理
        //
        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        //*************************************************************************************************
        //機　　能： 透視画像キャプチャ開始／終了時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void myTransImage_CaptureOnOffChanged(bool IsOn)
		{
			//透視画像処理ボタンの使用可・不可を設定する
            cmdLive.BackColor = (IsOn ? Color.Lime : SystemColors.Control);
			cmdEdge.Enabled = !IsOn;                //エッジ強調
			cmdDiff.Enabled = !IsOn;			    //微分
			cmdInverse.Enabled = !IsOn;			    //白黒反転
			cmdFImageOpen.Enabled = !IsOn;		    //透視画像を開く
			cmdFImageSave.Enabled = !IsOn;		    //透視画像保存
			cmdFImageTrans.Enabled = !IsOn;		    //透視画像転送 'v17.40追加 byやまおか 2010/10/22
			cmdFZoom.Enabled = !IsOn;			    //縮小/拡大
			//cmdUndo.Enabled = Not IsOn           '元に戻す
            if (IsOn) cmdUndo.Enabled = false;
            
            //ツールバーのアイコンも変更
            frmCTMenu.Instance.Toolbar1.Items["tsbtnLiveImage"].Image = frmCTMenu.Instance.ImageList1.Images[(IsOn ? "VideoOn" : "VideoOff")];
		}


        //*************************************************************************************************
        //機　　能： 「ライブ画像」ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdLive_Click(object sender, EventArgs e)
		{
			//ライブ画像オンオフを切り替える
			frmTransImage.Instance.CaptureOn = !frmTransImage.Instance.CaptureOn;

		}


        //*************************************************************************************************
        //機　　能： 「画像積算」ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdInteg_Click(object sender, EventArgs e)
		{
			//ボタンの色を緑にする
			cmdInteg.BackColor = Color.Lime;

            //画像積分フォームを表示
            myIntegForm = frmFInteg.Instance;
            myIntegForm.Show(frmCTMenu.Instance);
            
            //積分のClosedイベント追加
            myIntegForm.Unloaded += new frmFInteg.UnloadedEventHandler(myIntegForm_Unloaded);

 
        }


        //*************************************************************************************************
        //機　　能： 「画像積分」フォームアンロード時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void myIntegForm_Unloaded()
		{
            //積分のClosedイベント削除
            myIntegForm.Unloaded -= new frmFInteg.UnloadedEventHandler(myIntegForm_Unloaded);

            //画像積分フォーム参照破棄
			myIntegForm = null;

			//ボタンの色を元に戻す
			cmdInteg.BackColor = SystemColors.Control;

            // Add Start 2018/10/29 M.Oyama V26.40 Windows10対応
            cmdInteg.UseVisualStyleBackColor = true;
            // Add End 2018/10/29
		}


        //*************************************************************************************************
        //機　　能： 「動画保存」ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdSaveMovie_Click(object sender, EventArgs e)
		{
			//ボタンの色を緑にする
			cmdSaveMovie.BackColor = Color.Lime;

			//動画保存フォームを表示
			myMovieForm = frmSaveMovie.Instance;
            myMovieForm.Show(frmCTMenu.Instance);


            //「動画保存」のClosedイベント
            myMovieForm.Unloaded += new frmSaveMovie.UnloadedEventHandler(myMovieForm_Unloaded);

        }


        //*************************************************************************************************
        //機　　能： 「動画保存」フォームアンロード時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void myMovieForm_Unloaded()
		{
            //「動画保存」のClosedイベント
            myMovieForm.Unloaded -= new frmSaveMovie.UnloadedEventHandler(myMovieForm_Unloaded);

            //動画保存フォーム参照破棄
			myMovieForm = null;

			//ボタンの色を元に戻す
			cmdSaveMovie.BackColor = SystemColors.Control;

            // Add Start 2018/10/29 M.Oyama V26.40 Windows10対応
            cmdSaveMovie.UseVisualStyleBackColor = true;
            // Add End 2018/10/29
		}


        //*************************************************************************************************
        //機　　能： 「拡  大」ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdFZoom_Click(object sender, EventArgs e)
		{
			//'透視画像処理フォームを消去し、拡大フォームを表示
			//Me.hide
			//frmFZoom.Show

            //変更2014/10/07hata_v19.51反映
            //拡大なら　gZoomScale=1  縮小なら　gZoomScale=2
            //frmTransImage.Instance.ZoomScale = (frmTransImage.Instance.ZoomScale == 2 ? 1 : 2);     //v11.3追加 by 間々田 2006/02/23

            //v17.4X/v18.00以下に変更 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            switch (frmTransImage.Instance.TransImageDispSize)
            {
                case frmTransImage.TransImageDispSizeConstants.SmallSize:
                    frmTransImage.Instance.TransImageDispSize = frmTransImage.TransImageDispSizeConstants.MediumSize;
                    break;
                case frmTransImage.TransImageDispSizeConstants.MediumSize:

                    //v19.50 検出器がI.I.もしくは1024画素以下の場合は、midiumとsmallのみする by長野 2013/11/17
                    if ((!(CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) | (CTSettings.scancondpar.Data.mainch[0] <= 1024)))
                    {

                        frmTransImage.Instance.TransImageDispSize = frmTransImage.TransImageDispSizeConstants.SmallSize;

                    }
                    else
                    {

                        frmTransImage.Instance.TransImageDispSize = frmTransImage.TransImageDispSizeConstants.LargeSize;

                    }
                    break;

                case frmTransImage.TransImageDispSizeConstants.LargeSize:
                    frmTransImage.Instance.TransImageDispSize = frmTransImage.TransImageDispSizeConstants.SmallSize;
                    break;
            }
        
        }


        //*************************************************************************************************
        //機　　能： 「透視画像を開く」ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdFImageOpen_Click(object sender, EventArgs e)
		{
			//ボタンの色を緑にする
			cmdFImageOpen.BackColor = Color.Lime;

			//コモンダイアログの表示
			string FileName = null;
			FileName = modFileIO.GetFileName(StringTable.IDS_Open, CTResources.LoadResString(StringTable.IDS_TransImage));

			//透視画像フォームにロード
            if (!string.IsNullOrEmpty(FileName)) frmTransImage.Instance.LoadFromFile(FileName);

			//ボタンの色を元に戻す
			cmdFImageOpen.BackColor = SystemColors.Control;

            // Add Start 2018/10/29 M.Oyama V26.40 Windows10対応
            cmdFImageOpen.UseVisualStyleBackColor = true;
            // Add End 2018/10/29
		}


        //*************************************************************************************************
        //機　　能： 「透視画像の保存」ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdFImageSave_Click(object sender, EventArgs e)
		{
			//ボタンの色を緑にする
			cmdFImageSave.BackColor = Color.Lime;

			//コモンダイアログの表示
			string FileName = null;
			FileName = modFileIO.GetFileName(StringTable.IDS_Save, CTResources.LoadResString(StringTable.IDS_TransImage));

			//透視画像フォームにロード
            if (!string.IsNullOrEmpty(FileName)) frmTransImage.Instance.SaveToFile(FileName, (chkInfSave.CheckState == CheckState.Checked));

			//ボタンの色を元に戻す
			cmdFImageSave.BackColor = SystemColors.Control;

            // Add Start 2018/10/29 M.Oyama V26.40 Windows10対応
            cmdFImageSave.UseVisualStyleBackColor = true;
            // Add End 2018/10/29
		}


        //*************************************************************************************************
        //機　　能： 「透視画像の転送」ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V17.4   10/10/22    やまおか    新規作成
        //*************************************************************************************************
		private void cmdFImageTrans_Click(object sender, EventArgs e)
		{
			ushort[] WordImage = null;

			//Ipc32v5.RECT reg = new Ipc32v5.RECT();		//Image-Pro用エリア
			int rc = 0;			                        //関数の戻り値(汎用)
			int iino = 0;			                    //II視野番号
			float mdtpitch = 0;			                //データピッチ

			//幾何歪校正ステータスが準備完了の場合コモンのチャンネルピッチを使う
            if ((lblStatus2.Text == StringTable.GC_STS_STANDBY_OK) && (!CTSettings.detectorParam.Use_FlatPanel)) 
            {
				mdtpitch = CTSettings.scancondpar.Data.mdtpitch[2];
			}
			//幾何歪み校正ステータスが準備未完了の場合、フラットパネルの場合
            else
            {
				iino = modSeqComm.GetIINo();
				switch (iino) 
                {
					case 0:
					case 1:
					case 2:
						//II視野ごとの固定値を使う（コモンから取得する）
						//mdtpitch = CTSettings.scancondpar.Data.detector_pitch[iino];
                        //Rev23.10 変更 by長野 2015/11/04
                        mdtpitch = CTSettings.scancondpar.Data.detector_pitch[ScanCorrect.GFlg_MultiTube + iino * 3];
                        break;
					default:
						mdtpitch = 1;
						break;
				}
			}

			//イメージプロ起動
			if (!modCT30K.StartImagePro()) return;
           
			//PkeFPDの場合の額縁幅を計算する 'v17.53追加 byやまおか 2011/05/13
			int modX = 0;
			int modY = 0;
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) && (!CTSettings.detectorParam.Use_FpdAllpix)) 
            {
				modX = (frmTransImage.Instance.ctlTransImage.SizeX % 100);
                modY = (frmTransImage.Instance.ctlTransImage.SizeY % 100);
			} 
            else 
            {
				modX = 0;
				modY = 0;
			}

			//転送する画像のサイズ   'v17.53追加 byやまおか 2011/05/13
			int imgWidth = 0;
			int imgHeight = 0;
            //2014/11/07hata キャストの修正
            //imgWidth = (int)(frmTransImage.Instance.ctlTransImage.Width - modX / CTSettings.detectorParam.fpvm / frmTransImage.Instance.ZoomScale);
            //imgHeight = (int)(frmTransImage.Instance.ctlTransImage.Height - modY / CTSettings.detectorParam.fphm / frmTransImage.Instance.ZoomScale);
            imgWidth = Convert.ToInt32(frmTransImage.Instance.ctlTransImage.Width - modX / CTSettings.detectorParam.fpvm / (float)frmTransImage.Instance.ZoomScale);
            imgHeight = Convert.ToInt32(frmTransImage.Instance.ctlTransImage.Height - modY / CTSettings.detectorParam.fphm / (float)frmTransImage.Instance.ZoomScale);

			//Integer画像を取得（配列に格納）
            //WordImage = new int[frmTransImage.Instance.ctlTransImage.SizeX * frmTransImage.Instance.ctlTransImage.SizeY];
            WordImage = new ushort[frmTransImage.Instance.TransImageCtrl.ImageSize.Width * frmTransImage.Instance.TransImageCtrl.ImageSize.Height];
            //frmTransImage.Instance..ctlTransImage.GetImage(WordImage);
            WordImage = frmTransImage.Instance.TransImageCtrl.GetImage();
			
            //左右反転対応        'v17.50追加 2011/02/15 by 間々田
            if (CTSettings.detectorParam.IsLRInverse) 
            {
                ImgProc.ConvertMirror(ref WordImage[0], frmTransImage.Instance.ctlTransImage.SizeX, frmTransImage.Instance.ctlTransImage.SizeY);
			}

			//WL/WWから表示階調を調整する
			int Min = 0;
			int Max = 0;
            //2014/11/07hata キャストの修正
            ////Min = frmTransImage.Instance.ctlTransImage.WindowLevel - frmTransImage.Instance.ctlTransImage.WindowWidth / 2;
            ////Max = frmTransImage.Instance.ctlTransImage.WindowLevel + frmTransImage.Instance.ctlTransImage.WindowWidth / 2;
            //Min = frmTransImage.Instance.TransImageCtrl.WindowLevel - frmTransImage.Instance.TransImageCtrl.WindowWidth / 2;
            //Max = frmTransImage.Instance.TransImageCtrl.WindowLevel + frmTransImage.Instance.TransImageCtrl.WindowWidth / 2;
            Min = frmTransImage.Instance.TransImageCtrl.WindowLevel - Convert.ToInt32(frmTransImage.Instance.TransImageCtrl.WindowWidth / 2F);
            Max = frmTransImage.Instance.TransImageCtrl.WindowLevel + Convert.ToInt32(frmTransImage.Instance.TransImageCtrl.WindowWidth / 2F);
            
            //追加2014/11/28hata_v19.51_dnet
            //イメージProに(-)で設定するとエラーするので制限する
            if (Min < 0) Min = 0;
            if (Max > 65535) Max = 65535;

			//ChangeFullRange_Short WordImage(0), .SizeX, .SizeY, Min, Max   '画像を16ビットフルレンジにする 'しない
			int[] ipLArray = new int[2];
			ipLArray[0] = Min;
			ipLArray[1] = Max;

            //白黒反転して描画している場合          
            //if (frmTransImage.Instance.ctlTransImage.RasterOp == 0x330008)  //ラスターオペレーションコード: NOTSRCCOPY = 3342344 (0x330008)
            if (frmTransImage.Instance.TransImageCtrl.RasterOp == 1)  //
            {
                ImgProc.ReverseWordImage(ref WordImage[0], WordImage.GetUpperBound(0) + 1);
				ipLArray[0] = 65535 - Max;
				ipLArray[1] = 65535 - Min;
			}

			if (ScanCorrect.IMGPRODBG == 1)
            {
                #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                /*
                //Image-Pro 画像データ表示
				rc = Ipc32v5.IpAppCloseAll();				//開いている全てのドキュメントを閉じる
                rc = Ipc32v5.IpWsCreate(frmTransImage.Instance.ctlTransImage.SizeX,
                                        frmTransImage.Instance.ctlTransImage.SizeY, 300, Ipc32v5.IMC_GRAY16);     //空の画像ウィンドウを生成（Gray Scale 16形式）
				reg.Left = 0;
				reg.Top = 0;
				reg.Right = frmTransImage.ctlTransImage.SizeX - 1;
				reg.Bottom = frmTransImage.ctlTransImage.SizeY - 1;
				rc = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref reg, WordImage, Ipc32v5.CPROG);		//Image-Proに書込む
				rc = Ipc32v5.IpDocMinimize();				                    //閉じない代わりに最小化する 'v17.53追加 byやまおか 2011/05/13
				//rc = IpWsScale(.Width, .Height, 1)      'サイズを変更する(コピー)
				//額縁除去に対応するため下記に変更   'v17.53変更 byやまおか 2011/05/13
				Ipc32v5.ipRect.Left = modX / 2 - 1;
				Ipc32v5.ipRect.Top = modY / 2 - 1;
				Ipc32v5.ipRect.Right = frmTransImage.ctlTransImage.SizeX - modX / 2 - 1;
				Ipc32v5.ipRect.Bottom = frmTransImage.ctlTransImage.SizeY - modY / 2 - 1;
                rc = Ipc32v5.IpAoiCreateBox(ref Ipc32v5.ipRect);				//一回り小さくする
                rc = Ipc32v5.IpWsScale(imgWidth, imgHeight, 1);				    //指定した大きさにリサイズ   'v17.60追加 byやまおか 2011/06/13
				rc = Ipc32v5.IpWsCopy();				                        //アクティブをコピー
				rc = Ipc32v5.IpDocMinimize();				                    //閉じない代わりに最小化する     'v17.60追加 byやまおか 2011/06/13
                if (Ipc32v5.IpWsCreate(imgWidth, imgHeight, 300, Ipc32v5.IMC_GRAY16) < 0) return;	//新規画像ウィンドウ作成
				rc = Ipc32v5.IpWsPaste(0, 0);				                    //新規画像ウィンドウに貼り付ける
				//rc = IpDrSet(DR_BEST, 0, IPNULL)    '表示階調を最適化  'しない

				//rc = IpDocCloseEx(0)                   '元の画像を閉じる  '何故か2回目の測定でImage-Proがエラーするため閉じない
				rc = Ipc32v5.IpDrSet(Ipc32v5.DR_RANGE, 0, ipLArray);	    //表示階調をセット
				rc = Ipc32v5.IpWsZoom(100);				                        //100%で表示
                
                //int ProcessID = 0;
                //ProcessID = Shell(ImageProExe)          'Image-Proを前面に出すためにipwin32.exeを叩く
                
                Process p = new Process();
                p.StartInfo.FileName = modCT30K.ImageProExe;
                p.Start();	                                            //Image-Proを前面に出すためにipwin32.exeを叩く
                ProcessID = p.Id;
                p.Dispose();
				rc = Ipc32v5.IpDocMaximize();				            //Image-Pro内でドキュメントを最大化

				//新規較正データを作成
                int ipLVal = 0;
				ipLVal = Ipc32v5.IpSCalCreate();
				rc = Ipc32v5.IpSCalSetStr(ipLVal, Ipc32v5.SCAL_UNITS, "millimeters");		//単位を表示
				rc = Ipc32v5.IpSCalSetStr(ipLVal, Ipc32v5.SCAL_NAME, "temp");				//較正データの名前の設定
				rc = Ipc32v5.IpSCalSelect("temp");
				rc = Ipc32v5.IpSCalSetLong(ipLVal, Ipc32v5.SCAL_ADD_TO_REF, 1);				//カレント画像に較正を適用

				//単位(mm)あたりのピクセル数の設定
                float PixelsPerUnit = 0;
				//v17.601 検出器ch数(横)を透視画像横サイズで割った値をmdtpichにかけるように修正 by長野 2011/12/21
               int SizeXFluoroRate = 0;
                SizeXFluoroRate = frmTransImage.Instance.ctlTransImage.SizeX / frmTransImage.Instance.ctlTransImage.Width;
 				//PixelsPerUnit = 1 / (mdtpitch * (frmMechaControl.FCDFIDRate))
				PixelsPerUnit = 1 / (mdtpitch * SizeXFluoroRate * (frmMechaControl.Instance.FCDFIDRate));
				rc = Ipc32v5.IpSCalSetUnit(PixelsPerUnit, PixelsPerUnit);				        //単位を設定
				rc = Ipc32v5.IpSCalSetLong(Ipc32v5.SCAL_CURRENT_CAL, Ipc32v5.SCAL_APPLY, 1);	//カレント画像に設定を適用
                */
                
                //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                int height = frmTransImage.Instance.ctlTransImage.SizeY;
                int width = frmTransImage.Instance.ctlTransImage.SizeX;
                //単位(mm)あたりのピクセル数の設定
                float PixelsPerUnit = 0;
                int SizeXFluoroRate = 0;
                //2014/11/07hata キャストの修正
                //SizeXFluoroRate = frmTransImage.Instance.ctlTransImage.SizeX / frmTransImage.Instance.ctlTransImage.Width;
                SizeXFluoroRate = Convert.ToInt32(frmTransImage.Instance.ctlTransImage.SizeX / (float)frmTransImage.Instance.ctlTransImage.Width);
                PixelsPerUnit = 1 / (mdtpitch * SizeXFluoroRate * (frmMechaControl.Instance.FCDFIDRate));

                rc = CallImageProFunction.CallFImageTrans(WordImage, height, width, modX, modY, imgHeight, imgWidth, ipLArray[0], ipLArray[1], PixelsPerUnit);

                #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

            }
        }


        //*************************************************************************************************
        //機　　能： 「エッジ強調」ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdEdge_Click(object sender, EventArgs e)
		{
			//ボタンの色を緑にする
			cmdEdge.BackColor = Color.Lime;

			//表示画像をバックアップしておく
            frmTransImage.Instance.Backup();

			//エッジ強調フォームを表示
			myEdgeForm = frmFEdge.Instance;
            myEdgeForm.Show(frmCTMenu.Instance);

            //エッジ強調のClosedイベント
            myEdgeForm.Unloaded += new frmFEdge.UnloadedEventHandler(myEdgeForm_Unloaded);

        }


        //*************************************************************************************************
        //機　　能： 「エッジ強調」フォームアンロード時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void myEdgeForm_Unloaded()
		{
            //エッジ強調のClosedイベント
            myEdgeForm.Unloaded -= new frmFEdge.UnloadedEventHandler(myEdgeForm_Unloaded);

            //エッジ強調フォーム参照破棄
			myEdgeForm = null;

			//ボタンの色を元に戻す
			cmdEdge.BackColor = SystemColors.Control;

            // Add Start 2018/10/29 M.Oyama V26.40 Windows10対応
            cmdEdge.UseVisualStyleBackColor = true;
            // Add End 2018/10/29
		}


        //*************************************************************************************************
        //機　　能： 「微分」ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdDiff_Click(object sender, EventArgs e)
		{
			//ボタンの色を緑にする
			cmdDiff.BackColor = Color.Lime;

			//表示画像をバックアップしておく
            frmTransImage.Instance.Backup();

			//微分フォームを表示
			myDiffForm = frmFDiff.Instance;
            myDiffForm.Show(frmCTMenu.Instance);

            //エッジ強調のClosedイベント
            myDiffForm.Unloaded += new frmFDiff.UnloadedEventHandler(myDiffForm_Unloaded);
        }


        //*************************************************************************************************
        //機　　能： 「微分」フォームアンロード時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void myDiffForm_Unloaded()
		{
            //エッジ強調のClosedイベント
            myDiffForm.Unloaded -= new frmFDiff.UnloadedEventHandler(myDiffForm_Unloaded);
            
            //微分フォーム参照破棄
			myDiffForm = null;

			//ボタンの色を元に戻す
			cmdDiff.BackColor = SystemColors.Control;

            // Add Start 2018/10/29 M.Oyama V26.40 Windows10対応
            cmdDiff.UseVisualStyleBackColor = true;
            // Add End 2018/10/29
		}


        //*************************************************************************************************
        //機　　能： 「白黒反転」ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdInverse_Click(object sender, EventArgs e)
		{
			//白黒反転を実行する
            frmTransImage.Instance.Inverse();
		}


        //*************************************************************************************************
        //機　　能： 「元に戻す」ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdUndo_Click(object sender, EventArgs e)
		{
			//アンドゥー処理
            frmTransImage.Instance.Undo();
		}


        //*************************************************************************************************
        //機　　能： 「アベレージング」チェックボックス・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void chkAverage_Click(object sender, EventArgs e)
        {
			//FluoroIP.AverageOn = (chkAverage.CheckState == CheckState.Checked);
            CTSettings.scanParam.AverageOn = (chkAverage.CheckState == CheckState.Checked);
        }


        //*************************************************************************************************
        //機　　能： 「シャープ」チェックボックス・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void chkSharp_Click(object sender, EventArgs e)
		{
            //FluoroIP.SharpOn = (chkSharp.CheckState == CheckState.Checked);
            CTSettings.scanParam.SharpOn = (chkSharp.CheckState == CheckState.Checked);
		}


        //*************************************************************************************************
        //機　　能： アベレージング・スクロールバー値変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void hsbAverage_ValueChanged(object sender, EventArgs e)
		{
            //追加2014/12/15hata
            if (hsbAverage.Capture)
            {
                lblAverage.Text = StringTable.GetResString(StringTable.IDS_Frames, hsbAverage.Value.ToString());     //枚
                return;
            }

            //FluoroIP.AverageNum = hsbAverage.Value;
            CTSettings.scanParam.AverageNum = hsbAverage.Value;
		}


        //*************************************************************************************************
        //機　　能： アベレージング・スクロール時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        //削除2014/12/15hata hsbAverage_MouseCaptureChangedを使用する
        //private void hsbAverage_Scroll(object sender, ScrollEventArgs e)
        //{
        //    //lblAverage.Text = StringTable.GetResString(StringTable.IDS_Frames, hsbAverage.Value.ToString());     //枚
        //}

        private void hsbAverage_MouseCaptureChanged(object sender, EventArgs e)
        {
            //追加2014/12/15hata
            if (!hsbAverage.Capture)
            {
                lblAverage.Text = StringTable.GetResString(StringTable.IDS_Frames, hsbAverage.Value.ToString());     //枚
                CTSettings.scanParam.AverageNum = hsbAverage.Value;
            }
        }


        //*******************************************************************************
        //機　　能： （透視画像用の）ウィンドウレベルプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
        public int WindowLevel
        {
            get { return cwsldLevel.Value; }
            set { cwsldLevel.Value = value; }
        }

        //*******************************************************************************
        //機　　能： （透視画像用の）ウィンドウ幅プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
        public int WindowWidth
        {
            get { return cwsldWidth.Value; }
            set { cwsldWidth.Value = value; }
        }


        //*******************************************************************************
        //機　　能： ウィンドウレベルスライダー変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void cwsldLevel_ValueChanged(object sender, EventArgs e)
		{
            //Rev20.00 追加 by長野 2015/02/09
            if (WLWWEvntLock) return;

			//値をラベルに表示
			//lblWL.Caption = CStr(Value)
			cwneWL.Value = cwsldLevel.Value;			//v17.02変更 byやまおか 2010/07/22

			//階調変換を実行
            if (myTransImage == null) return;
            myTransImage.TransImageCtrl.WindowLevel = cwsldLevel.Value;
            //myTransImage.ctlTransImage.WindowLevel = cwsldLevel.Value;
        }


        //*******************************************************************************
        //機　　能： ウィンドウ幅スライダー変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void cwsldWidth_ValueChanged(object sender, EventArgs e)
	    {
            //Rev20.00 追加 by長野 2015/02/09
            if (WLWWEvntLock) return;

			//値をラベルに表示
			//lblWW.Caption = CStr(Value)
            cwneWW.Value = (decimal)cwsldWidth.Value;			//v17.02変更 byやまおか 2010/07/22

			//階調変換を実行
            if (myTransImage == null) return;
            //myTransImage.ctlTransImage.WindowWidth = (int)cwneWW.Value;
            //2014/11/07hata キャストの修正
            //myTransImage.TransImageCtrl.WindowWidth = (int)cwneWW.Value;
            myTransImage.TransImageCtrl.WindowWidth = Convert.ToInt32(cwneWW.Value);
        }


        //*******************************************************************************
        //機　　能： ウィンドウレベル変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V17.02  10/07/21    やまおか    新規作成
        //*******************************************************************************
		private void cwneWL_ValueChanged(object sender, EventArgs e)
		{
            //Rev20.00 追加 by長野 2015/02/09
            if (WLWWEvntLock) return;

			//スライダーコントロールに値を反映させる
            //2014/11/07hata キャストの修正
            //if (ActiveControl == cwsldLevel) cwsldLevel.Value = (int)cwneWL.Value;
            //変更2014/12/15hata_逆になっている　
            //if (ActiveControl == cwsldLevel) cwsldLevel.Value = Convert.ToInt32(cwneWL.Value);
            //if (ActiveControl != cwsldLevel) cwsldLevel.Value = Convert.ToInt32(cwneWL.Value);
            if (ActiveControl != cwsldLevel) cwsldLevel.Value = Convert.ToInt32(cwneWL.Value);

            //追加2014/12/15hata
            txtWL.Text = cwneWL.Value.ToString();

        }


        //*******************************************************************************
        //機　　能： ウィンドウ幅変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V17.02  10/07/21    やまおか    新規作成
        //*******************************************************************************
		private void cwneWW_ValueChanged(object sender, EventArgs e)
		{
            //Rev20.00 追加 by長野 2015/02/09
            if (WLWWEvntLock) return;

            //スライダーコントロールに値を反映させる
            //2014/11/07hata キャストの修正
            //if (ActiveControl == cwsldWidth) cwsldWidth.Value = (int)cwneWW.Value;
            //変更2014/12/15hata_逆になっている　
            //if (ActiveControl == cwsldWidth) cwsldWidth.Value = Convert.ToInt32(cwneWW.Value);
            if (ActiveControl != cwsldWidth) cwsldWidth.Value = Convert.ToInt32(cwneWW.Value);

            //追加2014/12/15hata
            txtWW.Text = cwneWW.Value.ToString();
        }


        //*************************************************************************************************
        //機　　能： 倍率オプションボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [I/ ] 型        1:１倍 2:４倍 3:16倍
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void optScale_CheckedChanged(object sender, EventArgs e)
        {
            //変更2014/11/28hata_v19.51_dnet
            //if (sender as RadioButton == null) return;
            RadioButton r = sender as RadioButton;

            if (r == null) return;

            //追加2014/11/28hata_v19.51_dnet
            //unCheckのときは何もしない
            if (!r.Checked) return;

            //変更2014/11/28hata_v19.51_dnet
            //int Index = Array.IndexOf(optScale, sender);
            int Index = Array.IndexOf(optScale, r);

            if (Index < 0) return;

            //Rev20.00 追加 by長野 2015/02/09
            if (ResetWLWWProcessingFlg == false)
            {
                WLWWEvntLock = true;
            }
            else
            {
                WLWWEvntLock = false;
            }

            //スクロールバーの最大値の調整
            //cwsldWidth.Axis.Maximum = Choose(Index + 1, 256, 1024, 4096)
            switch (CTSettings.detectorParam.DetType)      //v17.00追加(ここから) byやまおか 2010/02/08
            {
                case DetectorConstants.DetTypeII:
                case DetectorConstants.DetTypeHama:
                    //cwsldWidth.Axis.Maximum = (new int[]{256, 1024, 4096})[Index];
                    cwsldWidth.Maximum = (new int[] { 256, 1024, 4096 })[Index];

                    break;
                case DetectorConstants.DetTypePke:
                    //cwsldWidth.Axis.Maximum = Choose(Index + 1, 4096, 16384, 50000)     'changed by　山本　2009-11-13
                    //cwsldWidth.Axis.Maximum = (new int[]{4096, 16384, 65536})[Index];			//v17.02変更 byやまおか 2010/06/14
                    cwsldWidth.Maximum = (new int[] { 4096, 16384, 65536 })[Index];			//v17.02変更 byやまおか 2010/06/14

                    break;
            }                   			//v17.00追加(ここまで) byやまおか 2010/02/08

            //cwsldLevel.Axis.Maximum = cwsldWidth.Axis.Maximum - 1;
            cwsldLevel.Maximum = cwsldWidth.Maximum - 1;
            
            //2014/12/15hata_削除
            ////追加2014/06/24(検S1)hata
            //WLMin.Text = Convert.ToString(cwsldLevel.Minimum);
            //WLMax.Text = Convert.ToString(cwsldLevel.Maximum);
            //WDMin.Text = Convert.ToString(cwsldWidth.Minimum);
            //WDMax.Text = Convert.ToString(cwsldWidth.Maximum);
            ////追加2014/11/26(検S1)hata
            //WDMax.Left = cwsldWidth.Right - WDMax.Width;
            //WLMax.Left = cwsldLevel.Right - WLMax.Width;

            //数値入力ボックスにも反映   'v17.10削除 byやまおか 2010/08/20
            //cwneWW.Maximum = cwsldWidth.Axis.Maximum    'v17.02追加 byやまおか 2010/07/23
            //cwneWL.Maximum = cwsldLevel.Axis.Maximum    'v17.02追加 byやまおか 2010/07/23
            //cwneWW.Minimum = cwsldWidth.Axis.Minimum    'v17.02追加 byやまおか 2010/07/23
            //cwneWL.Minimum = cwsldLevel.Axis.Minimum    'v17.02追加 byやまおか 2010/07/23

            //プロフィールフォームのウィンドウレベル最大値・ウィンドウ幅最大値の更新 'v17.02追加 byやまおか 2010/07/05
            
            if (modLibrary.IsExistForm(myProfGraph))
            //if (modLibrary.IsExistForm("frmProfGrph"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            {
                modLibrary.SetOption(frmProfGrph.Instance.optScale, Index);
            }

            //Index値を記憶
            modCT30K.FimageBitIndex = Index;

            //Rev20.00 追加 by長野 2015/02/09
            WLWWEvntLock = false;
        }


        //*************************************************************************************************
        //機　　能： 「リセット」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		public void cmdWLWWReset_Click(object sender, EventArgs e)
		{
            //Rev20.00 追加 by長野 2015/02/09
            ResetWLWWProcessingFlg = true;

			//ウィンドウレベル・ウィンドウ幅の最大値の設定
			//SetOption optScale, scancondpar.fimage_bit
			//SetOption optScale, IIf(DetType = DetTypePke, 0, scancondpar.fimage_bit)   'v17.00変更　山本 2009-10-09  Pke起動時は1倍とする
            modLibrary.SetOption(optScale, (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke ? 2 : CTSettings.scancondpar.Data.fimage_bit));    //v17.10変更 byやまおか 2010-08-24  Pke起動時は16倍とする
            
            
            cwneWL.Maximum = cwsldLevel.Maximum;			//v17.10追加 byやまおか 2010-08-24
			cwneWL.Minimum = cwsldLevel.Minimum;			//v17.10追加 byやまおか 2010-08-24
			cwneWW.Maximum = cwsldWidth.Maximum;			//v17.10追加 byやまおか 2010-08-24
			cwneWW.Minimum = cwsldWidth.Minimum;			//v17.10追加 byやまおか 2010-08-24

			//ウィンドウレベル／ウィンドウ幅を初期値に戻す
            //2014/11/07hata キャストの修正
            ////cwsldLevel.Value = CInt(cwsldLevel.Axis.Maximum / 2)
            //cwsldLevel.Value = Convert.ToInt32(cwsldLevel.Maximum / 2);			//v17.10変更 byやまおか 2010/08/21
            cwsldLevel.Value = Convert.ToInt32(cwsldLevel.Maximum / 2F);			//v17.10変更 byやまおか 2010/08/21
            cwsldWidth.Value = cwsldWidth.Maximum;

			//プロフィールグラフが表示されている場合
            //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            if (modLibrary.IsExistForm(myProfGraph)) modLibrary.SetOption(frmProfGrph.Instance.optScale, modCT30K.FimageBitIndex);
            //if (modLibrary.IsExistForm("frmProfGrph")) modLibrary.SetOption(frmProfGrph.Instance.optScale, modCT30K.FimageBitIndex);
           
            //Rev20.00 追加 by長野 2015/02/09
            ResetWLWWProcessingFlg = false;
            
        }


        //*************************************************************************************************
        //機　　能： 「中心線」チェックボックス・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V9.7  04/12/27  (SI4)間々田     新規作成
        //*************************************************************************************************
		private void chkCenterLine_Click(object sender, EventArgs e)
		{
			//中心線（縦線）の表示・非表示を設定
            frmTransImage.Instance.ctlTransImage.SetLineVisible(LineConstants.CenterLine, (chkCenterLine.CheckState == CheckState.Checked));
		
           
        }


        //*************************************************************************************************
        //機　　能： 「中心線（横）」チェックボックス・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V15.10  09/10/22  やまおか     新規作成
        //*************************************************************************************************
		private void chkCenterLineH_Click(object sender, EventArgs e)
		{
			//中心線（横線）の表示・非表示を設定
            frmTransImage.Instance.ctlTransImage.SetLineVisible(LineConstants.CenterLineH, (chkCenterLineH.CheckState == CheckState.Checked));
		}


        //*************************************************************************************************
        //機　　能： 「スキャン位置」チェックボックス・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void chkDspPosi_Click(object sender, EventArgs e)
		{
			//水平線の表示・非表示を設定
            frmTransImage.Instance.ctlTransImage.SetLineVisible(LineConstants.ScanLine, (chkDspPosi.CheckState == CheckState.Checked));
		}


        //*************************************************************************************************
        //機　　能： 「データ収集エリア」チェックボックス・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void chkDspSW_Click(object sender, EventArgs e)
		{
			//スライス厚（上下線）の表示・非表示を設定
            frmTransImage.Instance.ctlTransImage.SetLineVisible(LineConstants.LowerLine, (chkDspSW.CheckState == CheckState.Checked));
            frmTransImage.Instance.ctlTransImage.SetLineVisible(LineConstants.UpperLine, (chkDspSW.CheckState == CheckState.Checked));
		}

        //*************************************************************************************************
        //機　　能： 「ラインプロファイル」チェックボックス・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V25.00  16/08/08  (検S1)長野    新規作成
        //*************************************************************************************************
        private void chkLineProfile_CheckedChanged(object sender, EventArgs e)
        {
            //ラインプロファイルの初期設定
            frmTransImage.Instance.SetTransProfile();

            //ラインプロファイルの表示・非表示を設定
            frmTransImage.Instance.ctlTransImage.SetLineVisible(LineConstants.ProfilePosV, (chkLineProfile.CheckState == CheckState.Checked));
            frmTransImage.Instance.ctlTransImage.SetLineVisible(LineConstants.ProfilePosH, (chkLineProfile.CheckState == CheckState.Checked));
            frmTransImage.Instance.ctlTransImage.SetLineVisible(LineConstants.ProfileV, (chkLineProfile.CheckState == CheckState.Checked));
            frmTransImage.Instance.ctlTransImage.SetLineVisible(LineConstants.ProfileH, (chkLineProfile.CheckState == CheckState.Checked));

            frmTransImage.Instance.ctlTransImage.SetStringVisible(StringConstants.Profile0PosH, (chkLineProfile.CheckState == CheckState.Checked));
            frmTransImage.Instance.ctlTransImage.SetStringVisible(StringConstants.Profile100PosH, (chkLineProfile.CheckState == CheckState.Checked));
            frmTransImage.Instance.ctlTransImage.SetStringVisible(StringConstants.Profile0PosV, (chkLineProfile.CheckState == CheckState.Checked));
            frmTransImage.Instance.ctlTransImage.SetStringVisible(StringConstants.Profile100PosV, (chkLineProfile.CheckState == CheckState.Checked));

            CTSettings.scanParam.LineProfOn = (chkLineProfile.CheckState == CheckState.Checked);
        }

        //*************************************************************************************************
        //機　　能： 「プロフィール」ボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdShowProf_Click(object sender, EventArgs e)
		{
			//'ボタンの色を緑にする   'v15.10追加 byやまおか 2010/01/21
			//cmdShowProf.BackColor = vbGreen
			//
			//'プロフィールボタンを無効にする
			//cmdShowProf.Enabled = False
			//
			//'プロフィールグラフ画面表示
			//frmProfGrph.Show , frmCTMenu
			//
			//'グラフの更新
			//frmProfGrph.UpdateGraph 'v17.02追加 byやまおか 2010/07/07

			//'プロフィールボタンでも表示・非表示を切り替えられるようにした   'v17.02変更 byやまおか 2010/07/23
			//If IsExistForm(frmProfGrph) Then
			//
			//    'プロフィールを閉じる
			//    frmProfGrph.cmdClose_Click
			//
			//Else
			//
			//    'ボタンの色を緑にする   'v15.10追加 byやまおか 2010/01/21
			//    cmdShowProf.BackColor = vbGreen
			//
			//    'プロフィールグラフ画面表示
			//    frmProfGrph.Show , frmCTMenu
			//
			//    'グラフの更新
			//    frmProfGrph.UpdateGraph 'v17.02追加 byやまおか 2010/07/07
			//
			//End If

			//v17.50上記を以下に変更 by 間々田 2011/02/02
			if (myProfGraph == null) 
            {
				//プロフィールボタンの色を緑にする
				cmdShowProf.BackColor = Color.Lime;

				//プロフィールグラフ画面表示
				myProfGraph = frmProfGrph.Instance;
                myProfGraph.Show(frmCTMenu.Instance);

                //プロフィールのClosedイベント追加
                myProfGraph.Unloaded += new frmProfGrph.UnloadedEventHandler(myProfGraph_Unloaded);

			} 
            else 
            {
				//プロフィールグラフ画面をアンロード
				myProfGraph.Close();
			}
		}


        //*************************************************************************************************
        //機　　能： 「プロフィールグラフ」フォームアンロード時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.50  2011/02/01 (電S1)間々田 新規作成
        //*************************************************************************************************
        private void myProfGraph_Unloaded()
        //private void myProfGraph_FormClosed(object sender, FormClosedEventArgs e)
		{
            //プロフィールのClosedイベント追加
            if (myProfGraph != null) //追加201501/26hata_if文追加
            {
                myProfGraph.Unloaded -= new frmProfGrph.UnloadedEventHandler(myProfGraph_Unloaded);

                //プロフィールグラフフォーム参照破棄
                myProfGraph = null;
            }

			//プロフィールボタンの色を元に戻す
			if (modLibrary.IsExistForm(_Instance))
            //if (modLibrary.IsExistForm("frmScanControl"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            {
				cmdShowProf.BackColor = SystemColors.Control;

                // Add Start 2018/10/29 M.Oyama V26.40 Windows10対応
                cmdShowProf.UseVisualStyleBackColor = true;
                // Add End 2018/10/29
			}
		}


        //*************************************************************************************************
        //機　　能： ゲイン補正チェックボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void chkGainCalib_Click(object sender, EventArgs e)
		{
			//Pkeの場合、CT30ｋ起動時にtrueに常になってしまうので    'v17.00　山本　2010-01-27
			//v17.00修正 byやまおか 2010/03/04
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypeII) || 
                (CTSettings.detectorParam.DetType == DetectorConstants.DetTypeHama)) 
            {
                CTSettings.scanParam.FPGainOn = (chkGainCalib.CheckState == CheckState.Checked);
			}

			//プロフィールグラフが表示されている場合、更新する
            //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            if (modLibrary.IsExistForm(myProfGraph)) frmProfGrph.Instance.UpdateOffset();
            //if (modLibrary.IsExistForm("frmProfGrph")) frmProfGrph.Instance.UpdateOffset();
        }


        //*************************************************************************************************
        //機　　能： 画素欠陥補正チェックボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void chkPixelDefectCalib_Click(object sender, EventArgs e)
		{
			modCT30K.FPDefOn = (chkPixelDefectCalib.CheckState == CheckState.Checked);

			//プロフィールグラフが表示されている場合、更新する
            //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            if (modLibrary.IsExistForm(myProfGraph)) frmProfGrph.Instance.UpdateOffset();
            //if (modLibrary.IsExistForm("frmProfGrph")) frmProfGrph.Instance.UpdateOffset();
 		}


        //*************************************************************************************************
        //機　　能： フラットパネル設定「ゲイン」コンボボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V17.02  2010/07/15  やまおか    新規作成
        //*************************************************************************************************
		private void cmbGain_SelectedIndexChanged(object sender, EventArgs e)
		{
            //PkeFPDにゲインと積分時間をセットする
            SetFpdGainInteg(cmbGain.SelectedIndex, cmbInteg.SelectedIndex);
		}


        //*************************************************************************************************
        //機　　能： フラットパネル設定「積分時間」コンボボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V17.02  2010/07/15  やまおか    新規作成
        //*************************************************************************************************
		private void cmbInteg_SelectedIndexChanged(object sender, EventArgs e)
		{
			int Err = 0;	//v17.10追加 byやまおか 2010/08/25
			//FPD積分時間が大きいときは警告を表示する   'v17.10追加 byやまおか 2010/09/16
            //変更2014/10/07hata_v19.51反映
            //v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) && (this.cmbInteg.SelectedIndex > 3) && !modCT30K.CT30kNowStartingFlg) 
   			if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) & (this.cmbInteg.SelectedIndex > 3) & (!modCT30K.CT30kNowStartingFlg))
            {
				//        MsgBox "フラットパネル設定の積分時間が大きい状態ででライブ画像を表示すると" + vbCrLf _
				//'        + "画面の反応が悪くなります｡　注意して操作を行ってください｡ ", vbExclamation
                MessageBox.Show(CTResources.LoadResString(20073) + "\r\n" 
                              + CTResources.LoadResString(20074), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);	//ストリングテーブル化 'v17.60 by長野 2011/05/22
			}

			//PkeFPDにゲインと積分時間をセットする
			//SetFpdGainInteg cmbGain.ListIndex, cmbInteg.ListIndex
			Err = SetFpdGainInteg(cmbGain.SelectedIndex, cmbInteg.SelectedIndex);	//v17.10変更 byやまおか 2010/08/25
			if (Err == 0) 			//v17.10追加 byやまおか 2010/08/25
            {

                //校正処理の初期値を変更する     'v17.02追加 byやまおか 2010/07/23
                //繰り上げる //2014/07/07(検S1)hata
                //modScanCorrect.IntegNumAtVer = Convert.ToInt32(frmTransImage.Instance.GetCurrentFR() * 4);
                //ScanCorrect.IntegNumAtOff = Convert.ToInt32(frmTransImage.Instance.GetCurrentFR() * 4);
                //ScanCorrect.IntegNumAtPos = Convert.ToInt32(frmTransImage.Instance.GetCurrentFR() * 4);
                modScanCorrect.IntegNumAtVer = Convert.ToInt32(frmTransImage.Instance.GetCurrentFR() + 0.5) * 4;
                ScanCorrect.IntegNumAtOff = Convert.ToInt32(frmTransImage.Instance.GetCurrentFR() + 0.5) * 4;
                ScanCorrect.IntegNumAtPos = Convert.ToInt32(frmTransImage.Instance.GetCurrentFR() + 0.5) * 4;
                
                //2014/11/07hata キャストの修正
                //ScanCorrect.ViewNumAtRot = (int)((int)((frmTransImage.Instance.GetCurrentFR() * 20 + 99) * 0.01) / 0.01);		//10の位で切り上げをする
                ScanCorrect.ViewNumAtRot = Convert.ToInt32(Math.Floor((frmTransImage.Instance.GetCurrentFR() * 20 + 99) * 0.01) / 0.01);		//10の位で切り上げをする
                
                //繰り上げる //2014/07/07(検S1)hata
                //modScanCorrect.ViewNumAtGain = Convert.ToInt32(frmTransImage.Instance.GetCurrentFR() * 20);
                modScanCorrect.ViewNumAtGain = Convert.ToInt32(frmTransImage.Instance.GetCurrentFR() + 0.5) * 20;
                
                modScanCorrect.ViewNumAtGain = (modScanCorrect.ViewNumAtGain < CTSettings.GVal_ViewMin ? CTSettings.GVal_ViewMin : modScanCorrect.ViewNumAtGain);

                //追加2014/10/07hata_v19.51反映
                //v19.50 FPDの場合、オフセットのデフォルト枚数をビュー数と同じ。
                //スキャン位置構成の積算枚数を半分にする by長野 2014/01/28
                if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
                {
                    //2014/11/07hata キャストの修正
                    //ScanCorrect.IntegNumAtPos = (int)Math.Round(ScanCorrect.IntegNumAtPos / 2f, MidpointRounding.AwayFromZero);
                    //ScanCorrect.IntegNumAtOff = (int)Math.Round(modScanCorrect.ViewNumAtGain / 100d, MidpointRounding.AwayFromZero) * 100;
                    ScanCorrect.IntegNumAtPos = Convert.ToInt32(ScanCorrect.IntegNumAtPos / 2D);
                    ScanCorrect.IntegNumAtOff = Convert.ToInt32(Math.Round(modScanCorrect.ViewNumAtGain / 100D) * 100);

                    //v19.50 IntegNumが0以下になった場合を追加 by長野　2014/02/14
                    if (ScanCorrect.IntegNumAtOff <= 0)
                    {
                        ScanCorrect.IntegNumAtOff = 10;
                    }

                }

                if (optDataMode[3].Checked == true)
                {
                    //Rev22.00 Rev21.01の反映 by長野 2015/07/31
                    frmScanoCondition.Instance.Setup();
                }
            }
		}


        //*************************************************************************************************
        //機　　能： フラットパネル設定「ゲイン」「積分時間」更新処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V17.02  2010/07/15  やまおか    新規作成
        //*************************************************************************************************
        //Private Function SetFpdGainInteg(ByVal FpdGainIndex As Long, ByVal FpdIntegIndex As Long) As Long
		public int SetFpdGainInteg(int FpdGainIndex, int FpdIntegIndex)		//v17.10変更 byやまおか 2010/08/26
		{
			int functionReturnValue = 0;
            int ret = 0;

			//最新 scancel（コモン）取得
			//modScansel.GetScansel(ref CTSettings.scansel.Data);			//v17.10追加 byやまおか 2010/08/26
            CTSettings.scansel.Load();

			//PkeFPD用のゲインと積分時間をセットする
            if (((int)Pulsar.hPke != modScanCorrectNew.NullAddress)) 
            {
                bool LiveON_flg = false;

				//キャプチャ中だとゲイン/積分時間を設定できないため
				//PkeCaptureStop (hPke)   'ライブではなくPkeを止める

				//キャプチャストップ v17.50変更 by 間々田 2011/01/05
                ScanCorrect.CaptureSeqStop((int)Pulsar.hMil, (int)Pulsar.hPke);

				if (frmTransImage.Instance.CaptureOn == true) 
                {
                    //変更2014/10/07hata_v19.51反映
                    //PkeCaptureStop (hPke)   'ライブではなくPkeを止める 'v17.10外へ出す byやまおか 2010/08/25
                    //キャプチャ中ならライブを止める  'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    frmTransImage.Instance.CaptureOn = false;
 
                    //ライブ中＆X線ON中なら、ライブ再開フラグを立てる(ライブ中＆X線OFFは再開しない)
                    //LiveON_flg = true;
                    if ((frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus))
                        LiveON_flg = true;      //v19.50 v19.41とv18.02の統合 by長野 2013/11/05

                }

				//ゲインと積分時間をセットする
                //ret = ScanCorrect.PkeSetGainFrameRate((int)Pulsar.hPke, FpdGainIndex, FpdIntegIndex);
                //Rev26.00 change by chouno 2017/04/24
                if (CTSettings.t20kinf.Data.pki_fpd_type == 0)
                {
                    ret = ScanCorrect.PkeSetGainFrameRate((int)Pulsar.hPke, FpdGainIndex + 1, FpdIntegIndex);
                }
                else
                {
                    ret = ScanCorrect.PkeSetGainFrameRate((int)Pulsar.hPke, FpdGainIndex, FpdIntegIndex);
                }


				//設定するためにOFFした場合は再度ONする
				if (LiveON_flg == true) 
                {
					frmTransImage.Instance.CaptureOn = true;
					LiveON_flg = false;
				}

				//セット成功
				if (ret == 0) 
                {

                    //最新 scansel(コモン)取得
                    //modScansel.GetScansel(ref modScansel.scansel);            //v18.00追加 byやまおか 2011/07/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    CTSettings.scansel.Load();
                  
                    
                    //scanselのゲインと積分時間を更新
					//scansel.fpd_gain = cmbGain.ListIndex
					//scansel.fpd_integ = cmbInteg.ListIndex
					//v17.20 条件式を追加 by 長野 2010/09/16
					if (FpdGainIndex >= 0) 
                    {
						CTSettings.scansel.Data.fpd_gain = FpdGainIndex;        //v17.10修正 byやまおか 2010/08/26
                        //追加2014/10/07hata_v19.51反映
                        CTSettings.scansel.Data.fpd_gain_f = Convert.ToSingle(modCT30K.GetFpdGainStr(FpdGainIndex, CTSettings.t20kinf.Data.pki_fpd_type));        //v18.00追加 byやまおか 2011/07/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    }

					//v17.20 条件式を追加 by 長野 2010/09/16
					if (FpdIntegIndex >= 0) 
                    {
						CTSettings.scansel.Data.fpd_integ = FpdIntegIndex;		//v17.10修正 byやまおか 2010/08/26
                        //追加2014/10/07hata_v19.51反映
                        CTSettings.scansel.Data.fpd_integ_f = Convert.ToSingle(modCT30K.GetFpdIntegStr(FpdIntegIndex));          //v18.00追加 byやまおか 2011/07/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    }
					//modScansel.PutScansel(ref CTSettings.scansel.Data);
                    CTSettings.scansel.Write();

					//フラットパネル設定の表示更新
					UpdateFpdGainInteg();

					//正常
					functionReturnValue = 0;					
				} 
                //セット失敗
                else 
                {
					//異常
					functionReturnValue = 1;
				}
			}
			return functionReturnValue;
        }


        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //
        //   「再々構成」タブ内の処理
        //
        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        //*************************************************************************************************
        //機　　能： 「再構成リトライ」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： FileName        [I/ ] String    生データファイル名
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  新規作成
        //*************************************************************************************************
		private void cmdReconst_Click(object sender, EventArgs e)
		{
			//ボタンの色を緑にする
			cmdReconst.BackColor = Color.Lime;

			//ダブルオブリークで画像用にメモリが読み込まれたままの場合は中止　'v13.0追加 by 間々田 2007/04/24
			if (CTSettings.scaninh.Data.double_oblique == 0) 
            {
				if (modDoubleOblique.IsDoubleObliqueBusy(CTResources.LoadResString(StringTable.IDS_Reconst)))
                {
					//ボタンの色を元に戻す
					cmdReconst.BackColor = SystemColors.Control;

                    // Add Start 2018/10/29 M.Oyama V26.40 Windows10対応
                    cmdReconst.UseVisualStyleBackColor = true;
                    // Add End 2018/10/29
					return;
				}
			}

			//生データファイル選択ダイアログを表示
			string FileName = null;
            FileName = modFileIO.GetFileName(StringTable.IDS_Select,
                                             CTResources.LoadResString(StringTable.IDS_RawFile),
                                             ".raw",
                                             Purpose: CTResources.LoadResString(StringTable.IDS_Reconst));

			//再構成リトライ処理
            if (!string.IsNullOrEmpty(FileName)) Reconstruct(FileName);

            // Mod Start 2018/10/29 M.Oyama V26.40 Windows10対応
            //if (!Convert.ToBoolean(modCTBusy.CTBusy & modCTBusy.CTReconstruct)) cmdReconst.BackColor = SystemColors.Control;
            if (!Convert.ToBoolean(modCTBusy.CTBusy & modCTBusy.CTReconstruct))
            {
                cmdReconst.BackColor = SystemColors.Control;
                cmdReconst.UseVisualStyleBackColor = true;
            }
            // Mod End 2018/10/29
        }


        //*************************************************************************************************
        //機　　能： 再構成リトライ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： FileName        [I/ ] String    生データファイル名
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  新規作成
        //*************************************************************************************************
		private void Reconstruct(string FileName)
		{
			//再構成時に使用する生データファイル（拡張子なし）
			string myRawName = null;
            //myRawName = modFileIO.FSO.BuildPath(modFileIO.FSO.GetParentFolderName(FileName), modFileIO.FSO.GetBaseName(FileName));
            myRawName = Path.Combine(Path.GetDirectoryName(FileName), Path.GetFileNameWithoutExtension(FileName));

			//生データファイル名のチェック
            if (!((Regex.IsMatch(myRawName, @".+-\d\d\d") || Regex.IsMatch(myRawName, @".+-\d\d\d\d")) && myRawName.Length > 7))
            {
				//メッセージ表示：生データファイル名が異常なので、再構成リトライ処理を中止します。
                MessageBox.Show(CTResources.LoadResString(9530), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
            }

			//付帯情報をチェック：
			//   付帯情報ファイルがない場合、ズーミング画像の付帯情報ファイルもサーチし、
			//   その付帯情報をコピーして使う
            if (!modCT30K.SearchImageInfo(myRawName, true))
            {
                //メッセージ表示：付帯情報ファイルが見つかりません。
                MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_NotFound, StringTable.IDS_InfoFile), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

			string ExeFileName = null;
            //拡張子先頭のピリオドを消す
            string Ext = Path.GetExtension(FileName).TrimStart('.');
            //実行ファイル名
            ExeFileName = Ext.ToLower() == "cob" ? AppValue.CONERECON : AppValue.RECONMST;

			//実行ファイルがなければ中止
            if (!File.Exists(ExeFileName))
            {
                //メッセージ表示：～が見つかりません。再構成リトライを中止します。
                MessageBox.Show(StringTable.GetResString(StringTable.IDS_NotFound, ExeFileName) + "\r\n" + "\r\n" 
                                                       + StringTable.BuildResStr(StringTable.IDS_Interrupted, StringTable.IDS_Reconst), 
                                                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

			//再構成画像の有無をチェック
            if ((File.Exists(myRawName + ".img")) && (ExeFileName == AppValue.RECONMST))
            {
                //ダイアログ表示：XXX.img が存在します。上書きしますか？
                DialogResult result = MessageBox.Show(StringTable.GetResString(9915, myRawName + ".img"),
                                                      Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No) return;
            }


			//v17.60 上書きしますか?のダイアログの表示中に付帯情報ファイルがなくなった場合、再構成リトライ条件でOKを押すと
			//ソフトが落ちるため、ここでもう一度、付帯情報の有無をチェック by長野 2011/06/14

			//付帯情報をチェック：
			//   付帯情報ファイルがない場合、ズーミング画像の付帯情報ファイルもサーチし、
			//   その付帯情報をコピーして使う
            if (!modCT30K.SearchImageInfo(myRawName, true))
            {
                //メッセージ表示：付帯情報ファイルが見つかりません。
                MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_NotFound, StringTable.IDS_InfoFile), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


			//付帯情報ファイルから再構成リトライ条件のデフォルト値を読み取る
            //modImageInfo.ReadImageInfo(ref modImageInfo.gImageInfo, myRawName);
            ImageInfo.ReadImageInfo(ref CTSettings.gImageinfo.Data, myRawName);


			//v16.30/v17.00追加(ここから) byやまおか 2010/02/19

			//'Ver17以降のコーン生データなら（Ver17以降がコーン生データ分割に対応している）
			//If (GetImageInfoVerNumber(.version) >= 17#) And (FSO.GetExtensionName(FileName) = "cob") Then
			//Ver16.30以降のコーン生データなら（コーン生データ分割に対応している）
            //if ((modImageInfo.GetImageInfoVerNumber(modImageInfo.gImageInfo.version) >= 16.3) && (Path.GetExtension(FileName) == "cob"))
            if ((modImageInfo.GetImageInfoVerNumber(CTSettings.gImageinfo.Data.version.GetString()) >= 16.3) && (Path.GetExtension(FileName).TrimStart('.') == "cob"))
            {
			    int i = 0;
			    int scb_No = 0;                 //生データファイル数
			    string scb_FileName = null;     //生データファイル名

				//生データの数をチェックする
                if ((CTSettings.gImageinfo.Data.acq_view % CTSettings.gImageinfo.Data.cob_view) == 0)
                {
                    scb_No = CTSettings.gImageinfo.Data.acq_view / CTSettings.gImageinfo.Data.cob_view;   //余り切捨て
                }
                else
                {
                    scb_No = CTSettings.gImageinfo.Data.acq_view / CTSettings.gImageinfo.Data.cob_view + 1;
                }

				//ファイルがそろっているかチェックする
				for (i = 1; i <= scb_No - 1; i++) 
                {
					scb_FileName = myRawName + i.ToString("-00") + ".scb";
					if (!File.Exists(scb_FileName)) 
                    {
						//メッセージ表示：～が見つかりません。再構成リトライを中止します。
                        MessageBox.Show(StringTable.GetResString(StringTable.IDS_NotFound, ExeFileName) + "\r\n" + "\r\n"
                                       + StringTable.BuildResStr(StringTable.IDS_Interrupted, StringTable.IDS_Reconst),
                                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);                        
                        return;
					}
				}
			}
			//v16.30/v17.00追加(ここまで) byやまおか 2010/02/19

			//再構成リトライ画面を表示
			//frmRetryCondition.Instance.ShowDialog(myRawName);
            frmRetryCondition.Instance.Dialog(myRawName);
            //Rev20.00 不要 by長野 2014/12/04
            //frmRetryCondition.Instance.Dispose();
		}


        //*************************************************************************************************
        //機　　能： 「コーン後再構成」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdPostConeReconst_Click(object sender, EventArgs e)
		{
			//ボタンの色を緑にする
			cmdPostConeReconst.BackColor = Color.Lime;

			//ダブルオブリークで画像用にメモリが読み込まれたままの場合は中止　'v13.0追加 by 間々田 2007/04/24
			if (CTSettings.scaninh.Data.double_oblique == 0) 
            {
                if (modDoubleOblique.IsDoubleObliqueBusy(CTResources.LoadResString(StringTable.IDS_PostConeRetry)))
                {
                    //ボタンの色を元に戻す
                    cmdPostConeReconst.BackColor = SystemColors.Control;

                    // Add Start 2018/10/29 M.Oyama V26.40 Windows10対応
                    cmdPostConeReconst.UseVisualStyleBackColor = true;
                    // Add End 2018/10/29

                    return;
                }
			}

			//コーン後再構成画面表示
			//frmPostConeReconstruction.Show , frmCTMenu         'v15.10削除 byやまおか 2010/01/21
            frmPostConeReconstruction.Instance.ShowDialog(frmCTMenu.Instance);      //v15.10 Modal化 byやまおか 2010/01/21
            frmPostConeReconstruction.Instance.Dispose();
        }


        //*************************************************************************************************
        //機　　能： 「ズーミング」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
		private void cmdZooming_Click(object sender, EventArgs e)
		{
			//「画像処理」タブ内の「ズーミング」クリック時と同じ処理
			cmdImageProc_Click(cmdImageProc[ImageProcZooming], EventArgs.Empty);
        }


        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //
        //   「画像処理」タブ内の処理
        //
        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        //*************************************************************************************************
        //機　　能： 「画像処理」タブ内コントロールの初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  新規作成
        //*************************************************************************************************
		private void InitImageProc()
		{
            if (modOPTION.OptAdditionFlag) 
            {
				ImageProcAdd = AddImageProcItem(StringTable.IDS_AddImage);				//和画像
			}

			if (modOPTION.OptSubtractionFlag) 
            {
				ImageProcSub = AddImageProcItem(StringTable.IDS_SubImage);				//差画像
			}

			if (modOPTION.OptEnlargeFlag) 
            {
				ImageProcEnlarge = AddImageProcItem(StringTable.IDS_EnlargeSimple);		//単純拡大
			}

			ImageProcZooming = AddImageProcItem(StringTable.IDS_Zooming);			    //ズーミング

			if (modOPTION.OptROIFlag) 
            {
				ImageProcRoi = AddImageProcItem(StringTable.IDS_ROIProcessing);			//ROI処理
			}

			if (modOPTION.OptProfileFlag) 
            {
				ImageProcProfile = AddImageProcItem(StringTable.IDS_Profile);			//プロフィール
			}

			if (modOPTION.OptDistanceFlag) 
            {
				ImageProcDist = AddImageProcItem(StringTable.IDS_SizeMeasurement);		//寸法測定
			}

			if (modOPTION.OptPRDFlag) 
            {
				ImageProcPD = AddImageProcItem(StringTable.IDS_ProfileDistance2);		//プロフィールディスタンス
			}

			if (modOPTION.OptHistgramFlag) 
            {
                ImageProcHist = AddImageProcItem(StringTable.IDS_Histogram);			//ヒストグラム
			}

			if (modOPTION.OptCTDumpFlag) 
            {
				ImageProcCTDump = AddImageProcItem(StringTable.IDS_CTNumberDisp);		//CT値表示
			}

			if (modOPTION.OptColorFlag) 
            {
				ImageProcColor = AddImageProcItem(StringTable.IDS_PseudoColor);			//疑似カラー
			}

			if (modOPTION.OptMultiFrameFlag) 
            {
				ImageProcMulti = AddImageProcItem(StringTable.IDS_MultiFrame);			//マルチフレーム
			}

			if (modOPTION.OptPictureRetouchFlag) 
            {
				ImageProcInfo = AddImageProcItem(StringTable.IDS_EditImageInfo);		//付帯情報修正
			}

			if (modOPTION.OptFilterFlag) 
            {
				ImageProcFilter = AddImageProcItem(StringTable.IDS_FilterProc);			//フィルタ処理
            }

#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //    If OptBoneDensityFlag Then
			//        ImageProcBone = AddImageProcItem(IDS_BoneAnalysis)       '骨塩定量解析
			//    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

            //フォーマット変換
			if (modOPTION.OptFormatTransferFlag) 
            {
				ImageProcFormat = AddImageProcItem(StringTable.IDS_FormatConvert);		//フォーマット変換

				//フォーマット変換については必ず一番上に配置
				//.Move .Left + 2 * (.width + 120), cmdImageProc(1).Top
				//フレーム内右の空きスペースの中央上に配置   'v15.0変更 byやまおか 2009/07/30
                cmdImageProc[ImageProcFormat].Location = new Point(cmdImageProc[cmdImageProc.Count-1].Left + (fraScanCondition[5].Width - cmdImageProc[cmdImageProc.Count-1].Left + 6) / 2, cmdImageProc[0].Top);
            }

			ImageProcRoiAngle = AddImageProcItem(StringTable.IDS_AngleMeasurement);		//角度測定(画像から測定用)   'v16.00/v17.00追加 byやまおか 2010/02/24
			cmdImageProc[ImageProcRoiAngle].Visible = false;			                //(非表示)                   'v16.01/v17.00追加 byやまおか 2010/02/24
		}


        //*************************************************************************************************
        //機　　能： 「画像処理」タブ内コマンドボタンの生成
        //
        //           変数名          [I/O] 型        内容
        //引　　数： ResID           [I/ ] Long      ボタンに表示する文字列のリソース番号
        //           HintResID       [I/ ] Long      ToolTipTextにセットする文字列のリソース番号
        //戻 り 値：                 [ /O] Integer
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  新規作成
        //*************************************************************************************************
		private int AddImageProcItem(int ResID, int HintResID = 0)
		{
			int Index = 0;
            Index = cmdImageProc.Count - 1;

            if (!string.IsNullOrEmpty(cmdImageProc[Index].Text) || Index == 0 )
            {
                Index = Index + 1;

                Button b = new Button();
				cmdImageProc.Add(b);

				fraScanCondition[5].SuspendLayout();

				b.Font = cmdImageProc1.Font;
				b.Name = string.Format("cmdImageProc{0}", Index);
				b.Size = cmdImageProc1.Size;
				b.UseVisualStyleBackColor = cmdImageProc1.UseVisualStyleBackColor;
				b.Click += cmdImageProc_Click; 

				fraScanCondition[5].Controls.Add(b);
				fraScanCondition[5].ResumeLayout(false);

                if (Index == 1)
                {
                    cmdImageProc[Index].Left = cmdImageProc[0].Left;
                    cmdImageProc[Index].Top = cmdImageProc[0].Top;
                    cmdImageProc[0].Visible = false;
                }
                else
                {
                    cmdImageProc[Index].Left = cmdImageProc[Index - 1].Left;
                    cmdImageProc[Index].Top = cmdImageProc[Index - 1].Top + cmdImageProc[Index].Height + 6;

                }
                //フレームをはみ出したら次の列に配置
                if ((cmdImageProc[Index].Top + cmdImageProc[Index].Height) > fraScanCondition[5].Height)
                {
                    cmdImageProc[Index].Left = cmdImageProc[Index].Left + cmdImageProc[Index].Width + 8;
                    //cmdImageProc[Index].Top = (cmdImageProc[cmdImageProc.GetLowerBound(0)].Top);
                    cmdImageProc[Index].Top = (cmdImageProc[0].Top);
                }

                cmdImageProc[Index].Visible = true;
                cmdImageProc[Index].TabIndex = cmdImageProc[Index - 1].TabIndex + 1;    //追加 by 間々田 2009/08/05
            }

			//キャプションのセット
            cmdImageProc[Index].Text = CTResources.LoadResString(ResID);

			//ToolTipTextのセット
            if (HintResID > 0) ToolTip1.SetToolTip(cmdImageProc[Index], CTResources.LoadResString(HintResID));

			//戻り値セット
			return Index;
		}


        //*************************************************************************************************
        //機　　能： 画像処理タブ内コマンドボタン：クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [I/ ] Integer   クリックしたボタンのインデクス
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  新規作成
        //*************************************************************************************************
        //Private Sub cmdImageProc_Click(Index As Integer)
		public void cmdImageProc_Click(object sender, EventArgs e)      //v16.01/v17.00 Public化 2010/02/24 byやまおか
		{
			if (sender as Button == null) return;

            //int Index = Array.IndexOf(cmdImageProc, sender);
            Button bt = (Button)sender;
            int Index = cmdImageProc.IndexOf(bt);

			if (Index < 0) return;

			string FileName = null;

			//選択したボタンを緑にする
			cmdImageProc[Index].BackColor = Color.Lime;

			//ズーミングの場合は再々構成タブ内のボタンも緑にする
			if (Index == ImageProcZooming) cmdZooming.BackColor = Color.Lime;

			//画像が表示されていない場合，中止する処理のチェック
            if (Index == ImageProcEnlarge ||
                Index == ImageProcZooming ||
                Index == ImageProcRoi ||
                Index == ImageProcProfile ||
                Index == ImageProcDist ||
                Index == ImageProcPD ||
                Index == ImageProcHist ||
                Index == ImageProcCTDump ||
                Index == ImageProcColor ||
                Index == ImageProcFilter ||
                Index == ImageProcBone)
            {
                //画像が表示されていない場合
                if (string.IsNullOrEmpty(frmScanImage.Instance.Target))
                {
                    //'実行するには、画像を表示してください  'v15.10削除 byやまおか 2009/11/30
                    //MsgBox LoadResString(12839), vbExclamation
                    //GoTo CancelHandler

                    //画像ファイル選択ダイアログ表示     'v15.10追加 byやまおか 2009/11/30
                    FileName = modFileIO.GetFileName(StringTable.IDS_Open, CTResources.LoadResString(StringTable.IDS_CTImage), ".img");
                    if (!string.IsNullOrEmpty(FileName))
                    {
                        frmScanImage.Instance.Target = FileName;
                    }
                    else
                    {
                        goto CancelHandler;
                    }
                }
            }

			//v15.10追加(ここから) byやまおか 2009/11/30
			//断面にROIを描く必要があるときはROIメッセージを表示する
            if (Index == ImageProcEnlarge ||
                Index == ImageProcZooming ||
                Index == ImageProcRoi ||
                Index == ImageProcProfile ||
                Index == ImageProcDist ||
                Index == ImageProcPD ||
                Index == ImageProcHist ||
                Index == ImageProcRoiAngle)     //v16.00/v17.00追加 byやまおか
            {

                //ROIメッセージ表示
                if (!modLibrary.IsExistForm("frmRoiMessage"))	//追加2015/01/30hata
                {
                    frmRoiMessage.Instance.Show(frmCTMenu.Instance);
                }
                else
                {
                    frmRoiMessage.Instance.WindowState = FormWindowState.Normal;
                    frmRoiMessage.Instance.Visible = true;
                }

                //ROIメッセージのキャプションをセット
                frmRoiMessage.Instance.Text = cmdImageProc[Index].Text;
                
                Application.DoEvents();     //追加2014/08/23(検S1)hata
            }
			//v15.10追加(ここまで) byやまおか 2009/11/30


            //和画像ボタンをクリック：和画像フォームを表示
            if(Index == ImageProcAdd)
            {
                if (!modLibrary.IsExistForm("frmAddition"))	//追加2015/01/30hata_if文追加
                {
                    frmAddition.Instance.Show(frmCTMenu.Instance);
                }
                else
                {
                    frmAddition.Instance.WindowState = FormWindowState.Normal;
                    frmAddition.Instance.Visible = true;
                }

            }
			//差画像ボタンをクリック：差画像フォームを表示
            else if(Index == ImageProcSub)
            {
                if (!modLibrary.IsExistForm("frmSubtraction"))	//追加2015/01/30hata_if文追加
                {
                    frmSubtraction.Instance.Show(frmCTMenu.Instance);
                }
                else
                {
                    frmSubtraction.Instance.WindowState = FormWindowState.Normal;
                    frmSubtraction.Instance.Visible = true;
                }

            }
			//単純拡大ボタンをクリック：単純拡大処理モードにする
			else if(Index == ImageProcEnlarge)
            {
				frmScanImage.Instance.ImageProc = frmScanImage.ImageProcType.roiEnlarge;
			}
            //ズーミングボタンをクリック：ズーミングフォームを表示
			else if(Index == ImageProcZooming)
            {
                //ダブルオブリークで画像用にメモリが読み込まれたままの場合は中止　'v13.0追加 by 間々田 2007/04/24
				if (CTSettings.scaninh.Data.double_oblique == 0) 
                {
					if (modDoubleOblique.IsDoubleObliqueBusy(CTResources.LoadResString(StringTable.IDS_Zooming))) 
                    {
						goto CancelHandler;
					}
				}

				//ズーミング処理モードにする
				frmScanImage.Instance.ImageProc = frmScanImage.ImageProcType.roiZooming;
            }
			//ROI処理ボタンをクリック：ROI処理モードにする
			else if(Index == ImageProcRoi)
            {
                frmScanImage.Instance.ImageProc = frmScanImage.ImageProcType.roiProcessing;
            }
            //プロフィールボタンをクリック：プロフィール処理モードにする
			else if(Index == ImageProcProfile)
            {
                frmScanImage.Instance.ImageProc = frmScanImage.ImageProcType.roiProfile;
			}
            //寸法測定ボタンをクリック：寸法測定処理モードにする
			else if(Index == ImageProcDist)
            {
                frmScanImage.Instance.ImageProc = frmScanImage.ImageProcType.roiDistance;
            }
            //プロフィールディスタンスボタンをクリック：プロフィールディスタンス処理モードにする
            else if(Index == ImageProcPD)
            {
                frmScanImage.Instance.ImageProc = frmScanImage.ImageProcType.RoiProfileDistance;
            }
            //ヒストグラムボタンをクリック：ヒストグラム処理モードにする
			else if(Index == ImageProcHist)
            {
                frmScanImage.Instance.ImageProc = frmScanImage.ImageProcType.roiHistgram;
            }            
            //CT値表示ボタンをクリック：ＣＴ値表示フォームを表示
			else if(Index == ImageProcCTDump)
            {
                if (!modLibrary.IsExistForm("frmCtDump"))	//追加2015/01/30hata_if文追加
                {
                    frmCtDump.Instance.Show(frmCTMenu.Instance);
                }
                else
                {
                    frmCtDump.Instance.WindowState = FormWindowState.Normal;
                    frmCtDump.Instance.Visible = true;
                }
            }
            //疑似カラーボタンをクリック：カラー処理フォームを表示
			else if(Index == ImageProcColor)
            {
                if (!modLibrary.IsExistForm("frmColor"))	//追加2015/01/30hata
                {
                    frmColor.Instance.Show(frmCTMenu.Instance);
                }
                else
                {
                    frmColor.Instance.WindowState = FormWindowState.Normal;
                    frmColor.Instance.Visible = true;
                }
            }
            //マルチフレームボタンをクリック：マルチフレームフォームを表示
			else if(Index == ImageProcMulti)
            {
                if (!modLibrary.IsExistForm("frmMulti"))	//追加2015/01/30hata_if文追加
                {
                    frmMulti.Instance.Show(frmCTMenu.Instance);
                }
                else
                {
                    frmMulti.Instance.WindowState = FormWindowState.Normal;
                    frmMulti.Instance.Visible = true;
                }
            }
            //付帯情報修正ボタンをクリック
			else if(Index == ImageProcInfo)
            {
                //ファイル選択ダイアログ表示
				FileName = modFileIO.GetFileName(Description: CTResources.LoadResString(StringTable.IDS_InfoFile),
                                                Extension: ".inf",
                                                InitFileName: frmImageInfo.Instance.Target + ".inf");

				if (string.IsNullOrEmpty(FileName)) goto CancelHandler;

				//選択したファイル名を付帯情報フォームに渡し、付帯情報を表示
				frmPictureRetouch.Instance.Target = modLibrary.RemoveExtension(FileName, ".inf");
			}
            //フィルタ処理ボタンをクリック：フィルタ処理フォームを表示
            else if(Index == ImageProcFilter)
            {
                if (!modLibrary.IsExistForm("frmFilter"))	//追加2015/01/30hata_if文追加
                {
                    frmFilter.Instance.Show(frmCTMenu.Instance);
                }
                else
                {
                    frmFilter.Instance.WindowState = FormWindowState.Normal;
                    frmFilter.Instance.Visible = true;
                }
            }

#region			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
				//骨塩定量測定ボタンをクリック：骨塩定量測定フォームを表示
				//        Case ImageProcBone: frmBoneDensity.Show , frmCTMenu
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
#endregion
                
			//画像フォーマット変換ボタンをクリック：画像フォーマット変換フォームを表示
			else if(Index == ImageProcFormat)
            {
                if (!modLibrary.IsExistForm("frmFormatTransfer"))	//追加2015/01/30hata_if文追加
                {
                    frmFormatTransfer.Instance.Show(frmCTMenu.Instance);
                }
                else
                {
                    frmFormatTransfer.Instance.WindowState = FormWindowState.Normal;
                    frmFormatTransfer.Instance.Visible = true;
                }

            }            
            //角度測定ボタン(非表示)をクリック：角度測定処理モードにする 'v16.01/v17.00追加 byやまおか 2010/02/24
			else if(Index == ImageProcRoiAngle)
            {
                frmScanImage.Instance.ImageProc = frmScanImage.ImageProcType.RoiAngle;
			}

			//通常はここで抜ける
			return;


//キャンセル時処理
CancelHandler:

			//ボタンの色を元に戻す
			cmdImageProc[Index].BackColor = SystemColors.Control;

            // Add Start 2018/10/29 M.Oyama V26.40 Windows10対応
            cmdImageProc[Index].UseVisualStyleBackColor = true;
            // Add End 2018/10/29

			//ズーミングの場合は再々構成タブ内のボタンも元に戻す
			//v19.00 ダブルオブリークの画像削除をキャンセル→ズーミング実行すると、ROIクラス生成されていない状態で実行になりエラーとなるため変更 by長野 2012/05/02
			//    If Index = ImageProcZooming Then cmdZooming.BackColor = vbButtonFace
			if (Index == ImageProcZooming) 
            {
				cmdZooming.BackColor = SystemColors.Control;

                // Add Start 2018/10/29 M.Oyama V26.40 Windows10対応
                cmdZooming.UseVisualStyleBackColor = true;
                // Add End 2018/10/29

				//ズーミング処理モードにする
                //追加2015/01/21(検S1)hata_if文追加
				if(!string.IsNullOrEmpty(frmScanImage.Instance.Target))
                    frmScanImage.Instance.ImageProc = frmScanImage.ImageProcType.roiZooming;
			}
		}


        //*************************************************************************************************
        //機　　能： 「スキャンスタート」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        //Private Sub cmdScanStart_Click()
		private void ctbtnScanStart_Click(object Sender, EventArgs e)
		{
			//    '２重呼び出し防止
			//    Static BUSYNOW  As Boolean  '状態(True:実行中,False:停止中)
			//    If BUSYNOW Then Exit Sub
			//    BUSYNOW = True

			bool Cancel = false;
			Cancel = false;

            //Rev23.30 / Rev23.13 by長野 2016/03/10
            if (modScanCondition.ExScanStartAbortFlg == true)
            {
                modScanCondition.ExScanStartAbortFlg = false;
            }

            //ボタンを使用不可にする
			//    cmdScanStart.Enabled = False
            
            //追加2014/10/07hata_v19.51反映
            //メカが動ける(パネルがOFF)かチェック     'v18.00追加 byやまおか 2011/07/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev23.10 ここで止まるとスタートボタンのenabledがfalseになったままになるので、確実に復帰させる by長野 2015/10/18
            if ((!modMechaControl.IsOkMechaMove()))
            {
                Cancel = true;
                if (Cancel) ctbtnScanStart.Enabled = true;   
                return;
            }


			//すでにスキャンスタートしている場合
			if ((modCTBusy.CTBusy & modCTBusy.CTScanStart) != 0) 
            {
				//'スキャンストップ指令
				//UserStopSet
				//
				//'連続回転コーンビーム＋高速再構成の時は、RAMディスクのscanstopを使う v17.40 追加 by 長野
				//If smooth_rot_cone_flg = True Then
				//
				//    UserStopSet_rmdsk
				//
				//End If

                //スキャンを停止する
                CTSettings.transImageControl.CaptureScanStop();


				//実行中の処理に対して停止要求をする     'v17.50上記の処理を関数化 by 間々田 2011/02/17
				modCT30K.CallUserStopSet();
			} 
            else 
            {
                //Rev20.00 追加 by長野 2015/02/20_2
                ctbtnScanStart.Visible = false;
                ctbtnScanStop.Visible = true;
                ctbtnScanStop.Enabled = false;
                Application.DoEvents();
            
                //CPUステータスが完了でないときは抜ける  'v17.61追加 byやまおか 2011/07/30
                //変更2014/10/07hata_v19.51反映
                //if ((frmStatus.Instance.stsCPU.Status != StringTable.GC_STS_STANDBY_OK)) 
                //スキャンスタートボタンを有効にしない条件     'v18.00追加 byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
				if ((frmStatus.Instance.stsCPU.Status != StringTable.GC_STS_STANDBY_OK) | (frmStatus.Instance.stsMecha.Status == StringTable.GC_STS_BUSY)) 
                {
					Cancel = true;					
				} 
                //それ以外はスキャンスタート処理をする   'v17.61追加 byやまおか 2011/07/30
                else 
                {

                    //Rev20.00 追加 スキャンストップファイルクリア 2014/08/27 by長野 
                    modCT30K.CallUserStopClear();     
                    //Rev20.00 スキャン前にメモリコンパクション 2014/08/27 by長野
                    GC.Collect();
                  
					//スキャン条件タブ内の値を保存してしてからスキャンスタートする
					if (!SaveScanCondition()) 
                    {
						Cancel = true;                    
					} 
                    //スキャン（またはコーンビーム）スタート
                    else if (!frmStatus.Instance.ExScanStart()) 
                    {
						Cancel = true;
					}
				}
                //Rev20.00 追加 by長野 2015/02/20_2
                ctbtnScanStop.Enabled = false;
                ctbtnScanStop.Visible = false;
                ctbtnScanStart.Visible = true;
                Application.DoEvents();
			}

			//処理がキャンセルされた場合：ボタンを使用可にする
			//If Cancel Then cmdScanStart.Enabled = True
			if (Cancel) ctbtnScanStart.Enabled = true;

			//    '元の状態に戻す
			//    BUSYNOW = False
		}
        //*************************************************************************************************
        //機　　能： 「スキャンストップ」ボタンクリック時処理(ExScanStart内部でストップさせるため追加)
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V20.00  15/02/21   (検S1)長野      新規作成
        //*************************************************************************************************
        //Private Sub cmdScanStart_Click()
        private void ctbtnScanStop_Click(object Sender, EventArgs e)
        {
            //すでにスキャンスタートしている場合
            if ((modCTBusy.CTBusy & modCTBusy.CTScanStart) != 0)
            {
                //'スキャンストップ指令
                //UserStopSet
                //
                //'連続回転コーンビーム＋高速再構成の時は、RAMディスクのscanstopを使う v17.40 追加 by 長野
                //If smooth_rot_cone_flg = True Then
                //
                //    UserStopSet_rmdsk
                //
                //End If

                //スキャンを停止する
                modScanCondition.ExScanStartAbortFlg = true;


                //実行中の処理に対して停止要求をする     'v17.50上記の処理を関数化 by 間々田 2011/02/17
                //modCT30K.CallUserStopSet();
            }
        }
        
        //*************************************************************************************************
        //機　　能： タブ切り替え処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
		private void SSTab1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //追加2014/09/11_dNet対応_hata
            if (!bySetup) return;

            int sindex = SSTab1.SelectedIndex;

            //Rev26.10 ガイドモード無しの場合、fraScanCondition[1]を表示しない by井上 2017/12/12
            if (CTSettings.scaninh.Data.guide_mode == 1)
            {
                if (sindex > 0) sindex = sindex + 1;
            }

            //表示していないタブ内のコントロールにフォーカスさせないようにするための措置
            int i = 0;
            if (CTSettings.scaninh.Data.mechacontrol == 0)
            {
                for (i = fraScanCondition.GetLowerBound(0); i <= fraScanCondition.GetUpperBound(0); i++)
                {
                    fraScanCondition[i].Enabled = (i == sindex);

                    //追加2014/09/11_dNet対応_hata
                    //選択されていないTabのVisibleがFalse になるための処置
                    if (sindex == i)
                    {
                        fraScanCondition[i].Parent = SSTab1.TabPages[SSTab1.SelectedIndex];
                        fraScanCondition[i].Location = new Point(1, 2);
                    }
                    else
                    {
                        fraScanCondition[i].Parent = this;
                        fraScanCondition[i].Top = SSTab1.Top + SSTab1.ItemSize.Height;
                        fraScanCondition[i].Left = SSTab1.Left;
                    }
                }
            }
            else
            {
                if (SSTab1.SelectedIndex == 0)
                {
                    fraScanCondition[4].Parent = SSTab1.TabPages[0];
                    fraScanCondition[4].Location = new Point(1, 2);

                    fraScanCondition[5].Parent = this;
                    fraScanCondition[5].Top = SSTab1.Top + SSTab1.ItemSize.Height;
                    fraScanCondition[5].Left = SSTab1.Left;

                    //Rev26.01/Rev25.101 add by chouno 2017/11/16
                    fraScanCondition[4].Enabled = true;
                    fraScanCondition[5].Enabled = false;

                }
                else if (SSTab1.SelectedIndex == 1)
                {
                    fraScanCondition[5].Parent = SSTab1.TabPages[1];
                    fraScanCondition[5].Location = new Point(1, 2);

                    fraScanCondition[4].Parent = this;
                    fraScanCondition[4].Top = SSTab1.Top + SSTab1.ItemSize.Height;
                    fraScanCondition[4].Left = SSTab1.Left;

                    //Rev26.01/Rev25.101 add by chouno 2017/11/16
                    fraScanCondition[4].Enabled = false;
                    fraScanCondition[5].Enabled = true;
                }
            }

            ////表示していないタブ内のコントロールにフォーカスさせないようにするための措置
            //int i = 0;
            //if (CTSettings.scaninh.Data.mechacontrol == 0)
            //{
            //    for (i = fraScanCondition.GetLowerBound(0); i <= fraScanCondition.GetUpperBound(0); i++)
            //    {
            //        fraScanCondition[i].Enabled = (i == SSTab1.SelectedIndex);

            //        //追加2014/09/11_dNet対応_hata
            //        //選択されていないTabのVisibleがFalse になるための処置
            //        if (SSTab1.SelectedIndex == i)
            //        {
            //            fraScanCondition[i].Parent = SSTab1.TabPages[i];
            //            fraScanCondition[i].Location = new Point(1, 2);
            //        }
            //        else
            //        {
            //            fraScanCondition[i].Parent = this;
            //            fraScanCondition[i].Top = SSTab1.Top + SSTab1.ItemSize.Height;
            //            fraScanCondition[i].Left = SSTab1.Left;
            //        }
            //    }
            //}
            //else
            //{
            //    if (SSTab1.SelectedIndex == 0)
            //    {
            //        fraScanCondition[4].Parent = SSTab1.TabPages[0];
            //        fraScanCondition[4].Location = new Point(1, 2);

            //        fraScanCondition[5].Parent = this;
            //        fraScanCondition[5].Top = SSTab1.Top + SSTab1.ItemSize.Height;
            //        fraScanCondition[5].Left = SSTab1.Left;

            //        //Rev26.01/Rev25.101 add by chouno 2017/11/16
            //        fraScanCondition[3].Enabled = true;
            //        fraScanCondition[4].Enabled = false;

            //    }
            //    else if (SSTab1.SelectedIndex == 1)
            //    {
            //        fraScanCondition[5].Parent = SSTab1.TabPages[1];
            //        fraScanCondition[5].Location = new Point(1, 2);

            //        fraScanCondition[4].Parent = this;
            //        fraScanCondition[4].Top = SSTab1.Top + SSTab1.ItemSize.Height;
            //        fraScanCondition[4].Left = SSTab1.Left;

            //        //Rev26.01/Rev25.101 add by chouno 2017/11/16
            //        fraScanCondition[3].Enabled = false;
            //        fraScanCondition[4].Enabled = true;
            //    }
            //}      
        }


        //*************************************************************************************************
        //機　　能： mecainf変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
		private void myMechaControl_MecainfChanged(object sender, EventArgs e)
		{
			//チェックボックスの更新
            byEvent = false;
            chkInhibit1.CheckState = (CTSettings.mecainf.Data.scanpos_cor_inh == 0 ? CheckState.Checked : CheckState.Unchecked);		//ｽｷｬﾝ位置校正ｽﾃｰﾀｽｲﾝﾋﾋﾞｯﾄ
            chkInhibit5.CheckState = (CTSettings.mecainf.Data.distance_cor_inh == 0 ? CheckState.Checked : CheckState.Unchecked);		//寸法校正ｽﾃｰﾀｽｲﾝﾋﾋﾞｯﾄ
			byEvent = true;


			//校正ステータス画面の更新
            frmCorrectionStatus.Instance.MyUpdate();
		}

        //*************************************************************************************************
        //機　　能： シーケンサ軸変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v26.00  2016/12/28 (検S1)長野  リニューアル
        //*************************************************************************************************
        private void myMechaControl_SeqAxisChanged(object sender, EventArgs e)
        {
            //[ガイド]タブで設定した内容は、設定時のFCD,FDD,Y軸位置でのみ有効なため、変更されたらフラグOFF
            if (scanAreaSetCmpFlg == true || scanCondSetCmpFlg == true)
            {
                scanAreaSetCmpFlg = false;
                scanCondSetCmpFlg = false;
            }

            //Rev26.00 画像からの条件再生クリア by chouno 2017/08/31
            if (scanCondImgSetCmpFlg == true)
            {
                scanCondImgSetCmpFlg = false;
            }
        }

        //*******************************************************************************
        //機　　能： 管電圧欄：値変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V15.10  09/11/19    やまおか    新規作成
        //*******************************************************************************
		private void cwneKV_ValueChanged(object sender, EventArgs e)
		{
			//スキャン条件の設定内容をコモンファイルへ書き込む
            //modScansel.scanselType theScansel = new modScansel.scanselType();
            //CTstr.SCANSEL theScansel = new CTstr.SCANSEL();
            ScanSel theScansel = new ScanSel();
            theScansel.Data.Initialize();

			//この画面で設定しているスキャン条件を取得
			//modScansel.GetScansel(ref theScansel);
            theScansel.Load();

			//管電圧を変更
            theScansel.Data.scan_kv = (float)cwneKV.Value;

			//scansel書き込み
			//modScansel.PutScansel(ref theScansel);
            CTSettings.scansel.Put(theScansel.Data);

			//MyScanselを更新
			//GetMyScansel
		}


        //*******************************************************************************
        //機　　能： 管電流欄：値変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V15.10  09/11/19    やまおか    新規作成
        //*******************************************************************************
		private void cwneMA_ValueChanged(object sender, EventArgs e)
		{
			//スキャン条件の設定内容をコモンファイルへ書き込む
            //modScansel.scanselType theScansel = new modScansel.scanselType();
            ScanSel theScansel = new ScanSel();
            theScansel.Data.Initialize();

			//この画面で設定しているスキャン条件を取得
			//modScansel.GetScansel(ref theScansel);
            theScansel.Load();

			//管電流を変更
            theScansel.Data.scan_ma = (float)cwneMA.Value;

			//scansel書き込み
			//modScansel.PutScansel(ref theScansel);
            CTSettings.scansel.Put(theScansel.Data);

			//MyScanselを更新
			//GetMyScansel
		}


        //*******************************************************************************
        //機　　能： 英語版のレイアウト調整
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V17.60  11/05/25   (検S１) 長野    新規作成
        //*******************************************************************************
		private void EnglishLayout()
		{
			int margin = 0;
            int num = 0;
            int i = 0;

			margin = 7;

			//各タブ
            SSTab1.Font = new Font(SSTab1.Font.Name, 10);

			//校正タブ
			//**********************************************************************************
            chkInhibit1.Width = 200;
            chkInhibit3.Width = 200;

			lblItem0.Left = (margin * 5);
			chkInhibit1.Left = (margin * 2);
			lblItem2.Left = (margin * 5);
			lblItem4.Left = (margin * 5);
			chkInhibit3.Left = (margin * 2);
			chkInhibit5.Left = (margin * 2);

            fraCorrectStatus.Left = 367;
            fraCorrectStatus.Width = 387;

			num = lblStatus.Length;
			for (i = 0; i <= num - 1; i++) 
            {
				lblColon[i].Left = lblItem2.Left + lblItem2.Width + margin;
				lblStatus[i].Left = lblColon[i].Left + margin;
			}

			//スキャン条件タブ
			//**********************************************************************************
			//Rev20.01 プリセットフレーム 追加 by長野 2015/05/19
            //chkImageCondition.Left = chkImageCondition.Left + 60;
            
            //透視画像処理フレーム
			//chkInfSave.Height = 355
            //cmdLive.Height = (int)(cmdLive.Height * 1.2);
            //cmdInteg.Height = (int)(cmdInteg.Height * 1.2);
            //cmdFZoom.Height = (int)(cmdFZoom.Height * 1.2);
            //cmdInverse.Height = (int)(cmdInverse.Height * 1.2);
            //cmdUndo.Height = (int)(cmdUndo.Height * 1.2);
            //cmdSaveMovie.Height = (int)(cmdSaveMovie.Height * 1.2);
            //cmdEdge.Height = (int)(cmdEdge.Height * 1.2);
            //cmdDiff.Height = (int)(cmdDiff.Height * 1.2);
            //cmdFImageOpen.Height = (int)(cmdFImageOpen.Height * 1.2);
            //cmdFImageSave.Height = (int)(cmdFImageSave.Height * 1.2);
            //cmdFImageTrans.Height = (int)(cmdFImageTrans.Height * 1.2);
            
            //Rev20.01 変更 by長野 2015/05/19
            //cmdLive.Height = Convert.ToInt32(cmdLive.Height * 1.2);
            //cmdInteg.Height = Convert.ToInt32(cmdInteg.Height * 1.2);
            //cmdFZoom.Height = Convert.ToInt32(cmdFZoom.Height * 1.2);
            //cmdInverse.Height = Convert.ToInt32(cmdInverse.Height * 1.2);
            //cmdUndo.Height = Convert.ToInt32(cmdUndo.Height * 1.2);
            //cmdSaveMovie.Height = Convert.ToInt32(cmdSaveMovie.Height * 1.2);
            //cmdEdge.Height = Convert.ToInt32(cmdEdge.Height * 1.2);
            //cmdDiff.Height = Convert.ToInt32(cmdDiff.Height * 1.2);
            //cmdFImageOpen.Height = Convert.ToInt32(cmdFImageOpen.Height * 1.2);
            //cmdFImageSave.Height = Convert.ToInt32(cmdFImageSave.Height * 1.2);
            //cmdFImageTrans.Height = Convert.ToInt32(cmdFImageTrans.Height * 1.2);
            cmdLive.Height = Convert.ToInt32(cmdLive.Height * 1.1);
            cmdInteg.Height = Convert.ToInt32(cmdInteg.Height * 1.1);
            cmdFZoom.Height = Convert.ToInt32(cmdFZoom.Height * 1.1);
            cmdInverse.Height = Convert.ToInt32(cmdInverse.Height * 1.1);
            cmdUndo.Height = Convert.ToInt32(cmdUndo.Height * 1.1);
            cmdSaveMovie.Height = Convert.ToInt32(cmdSaveMovie.Height * 1.1);
            cmdEdge.Height = Convert.ToInt32(cmdEdge.Height * 1.1);
            cmdDiff.Height = Convert.ToInt32(cmdDiff.Height * 1.1);
            cmdFImageOpen.Height = Convert.ToInt32(cmdFImageOpen.Height * 1.1);
            cmdFImageSave.Height = Convert.ToInt32(cmdFImageSave.Height * 1.1);
            cmdFImageTrans.Height = Convert.ToInt32(cmdFImageTrans.Height * 1.1);

            //Rev20.01 追加 by長野 2015/05/19
            chkInfSave.Font = new Font(chkInfSave.Font.Name, (float)8.5, chkInfSave.Font.Style, chkInfSave.Font.Unit);
            shpGreen.Left = shpGreen.Left + 10;
            shpCyan.Left = shpCyan.Left + 10;
            shpYellow.Left = shpYellow.Left + 10;
            Shape1.Left = Shape1.Left + 10;

			//アベレージングフレーム
			lblAverage.Text = "5" + CTResources.LoadResString(StringTable.IDS_Frame);

			//フラットパネル設定フレーム
			//Rev20.01 コメントアウト by長野 2015/05/19
            ////    lblFpdGain.Left = margin * 2
            ////2014/11/07hata キャストの修正
            ////lblFpdInteg.Width = (int)(lblFpdInteg.Width * 1.1);
            //lblFpdInteg.Width = Convert.ToInt32(lblFpdInteg.Width * 1.1);
            //lblFpdInteg.Top = (lblFpdInteg.Top - 7);
            ////2014/11/07hata キャストの修正
            ////lblFpdInteg.Height = (int)(lblFpdInteg.Height * 1.5);
            ////Rev20.01 コメントアウト by長野 2015/05/19
            //lblFpdInteg.Height = Convert.ToInt32(lblFpdInteg.Height * 1.5);
        }


//v19.00 ->(電S2)永井

        //*************************************************************************************************
        //機　　能： 「BHC」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v19.00  12/02/21    H.Nagai     新規作成
        //*************************************************************************************************
		private void cmdBHC_Click(object sender, EventArgs e)
		{
			//ボタンの色を緑にする
			cmdBHC.BackColor = Color.Lime;
                        
            frmBeamHardeningCorrection.Instance.ShowDialog(frmCTMenu.Instance);
            frmBeamHardeningCorrection.Instance.Dispose();

			cmdBHC.BackColor = SystemColors.Control;

            // Add Start 2018/10/29 M.Oyama V26.40 Windows10対応
            cmdBHC.UseVisualStyleBackColor = true;
            // Add End 2018/10/29
		}
//<- v19.00

        //*************************************************************************************************
        //機　　能： 「自動」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v23.12  12/12/121    (検S1)長野 新規作成
        //*************************************************************************************************
        private void cmdWLWWAuto_Click(object sender, EventArgs e)
        {
            //Rev20.00 追加 by長野 2015/02/09
            ResetWLWWProcessingFlg = true;

            //frmTransImageに表示中の画像が、保存した画像(bmp)の場合は処理を抜ける
            if (frmTransImage.Instance.Target != "")
            {
                return;
            }

            //自動実行時は16倍に戻す
            //ウィンドウレベル・ウィンドウ幅の最大値の設定
            //SetOption optScale, scancondpar.fimage_bit
            //SetOption optScale, IIf(DetType = DetTypePke, 0, scancondpar.fimage_bit)   'v17.00変更　山本 2009-10-09  Pke起動時は1倍とする
            modLibrary.SetOption(optScale, (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke ? 2 : CTSettings.scancondpar.Data.fimage_bit));    //v17.10変更 byやまおか 2010-08-24  Pke起動時は16倍とする
            cwneWL.Maximum = cwsldLevel.Maximum;			//v17.10追加 byやまおか 2010-08-24
            cwneWL.Minimum = cwsldLevel.Minimum;			//v17.10追加 byやまおか 2010-08-24
            cwneWW.Maximum = cwsldWidth.Maximum;			//v17.10追加 byやまおか 2010-08-24
            cwneWW.Minimum = cwsldWidth.Minimum;			//v17.10追加 byやまおか 2010-08-24

            //現在表示中の透視画像の元データを取得
            //キャプチャスレッドのデータをメインスレッド側の配列に入れる
            frmTransImage.Instance.TransImageCtrl.AutoContrastOn = true;
            frmTransImage.Instance.TransImageCtrl.Update(false);
            //Integer画像を取得（配列に格納）
            ushort[] WordImage;
            int h_size = frmTransImage.Instance.TransImageCtrl.ImageSize.Width;
            int v_size = frmTransImage.Instance.TransImageCtrl.ImageSize.Height;
            WordImage = new ushort[h_size * v_size];
            //frmTransImage.Instance..ctlTransImage.GetImage(WordImage);
            WordImage = frmTransImage.Instance.TransImageCtrl.GetImage();

            //最小値・最大値の探索範囲を決める
            //PkeFPDの場合の額縁幅を計算する 'v17.53追加 byやまおか 2011/05/13
            int modX = 0;
            int modY = 0;
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) && (!CTSettings.detectorParam.Use_FpdAllpix))
            {
                modX = Convert.ToInt32((frmTransImage.Instance.ctlTransImage.SizeX % 100) / 2F);
                modY = Convert.ToInt32((frmTransImage.Instance.ctlTransImage.SizeY % 100) / 2F);
            }
            else
            {
                modX = 0;
                modY = 0;
            }

            int StartXIndex = 0;
            int EndXIndex = 0;
            int StartYIndex = 0;
            int EndYIndex = 0;
            StartXIndex = modX;
            EndXIndex = h_size - modX;
            StartYIndex = modY;
            EndYIndex = v_size - modY;

            //最大値・最小値を求める
            int minVal = 0;
            int maxVal = 0;
            float div = 0.0f;
            float mean = 0.0f;
            ScanCorrect.GetStatisticalInfo(ref WordImage[0], h_size, v_size, StartXIndex, EndXIndex, StartYIndex, EndYIndex, ref minVal, ref maxVal, ref div, ref mean);

            //ウィンドウレベル／ウィンドウ幅を最小値・最大値から求める
            //cwsldLevel.Value = (maxVal - minVal) / 2;	
            //cwsldWidth.Value = maxVal - minVal;
            //cwsldLevel.Value = (int)mean;
            //cwsldWidth.Value = (int)(div * 6.0f * 2.0f);
            cwsldLevel.Value = modLibrary.CorrectInRange((int)mean, cwsldLevel.Minimum, cwsldLevel.Maximum);
            cwsldWidth.Value = modLibrary.CorrectInRange((int)(div * 6.0f * 2.0f), cwsldWidth.Minimum, cwsldWidth.Maximum);

            //プロフィールグラフが表示されている場合
            //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            if (modLibrary.IsExistForm(myProfGraph)) modLibrary.SetOption(frmProfGrph.Instance.optScale, modCT30K.FimageBitIndex);
            //if (modLibrary.IsExistForm("frmProfGrph")) modLibrary.SetOption(frmProfGrph.Instance.optScale, modCT30K.FimageBitIndex);

            //Rev20.00 追加 by長野 2015/02/09
            ResetWLWWProcessingFlg = false;
        }

        //追加2014/05hata
        private void frmScanControl_Activated(object sender, EventArgs e)
        {
            ////描画を強制する
            //if (this.Visible && this.Enabled) this.Refresh();
        }

        //追加2014/12/15hata
        private void txtWLWW_KeyPress(object sender, KeyPressEventArgs e)
        {
            presskye = e.KeyChar;
            switch (e.KeyChar)
            {
                //数字キーと削除キー
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
                    break;
                case (char)Keys.Return:
                    txtWLWW_TextChanged(sender, EventArgs.Empty);
                    break;
                default:
                    e.KeyChar = (char)0;
                    //変更2015/01/08hata_dNet
                    e.Handled = true;
                    //if (sender.Equals(txtWW))
                    //{
                    //    e.KeyChar = (char)1;
                    //}
                    break;
            }

        }

        //追加2014/12/15hata
        private void txtWLWW_TextChanged(object sender, EventArgs e)
        {
            int dval = 0;
            //Keys.Returnのときだけ反映する
            if (sender.Equals(txtWL))
            {
                dval = 0;
                if (! int.TryParse(txtWL.Text, out dval))
                {
                    presskye = (char)0;
                    return;
                }
                if ((int)cwneWL.Maximum < Convert.ToInt32(txtWL.Text))
                {
                    presskye = (char)0;
                    txtWL.Text = cwneWL.Maximum.ToString();
                    return;
                }
                if ((int)cwneWL.Minimum > Convert.ToInt32(txtWL.Text))
                {
                    presskye = (char)0;
                    txtWL.Text = cwneWL.Minimum.ToString();
                    return;
                }
                if (presskye == (char)Keys.Return)
                {
                    cwneWL.Value = dval;
                }
            }
            else if (sender.Equals(txtWW))
            {
                dval = 1;
                if (! int.TryParse(txtWW.Text, out dval))
                {
                    presskye = (char)0;
                    return;
                }
                if ((int)cwneWW.Maximum < Convert.ToInt32(txtWW.Text))
                {
                    presskye = (char)0;
                    txtWW.Text = cwneWW.Maximum.ToString();
                    return;
                }
                if ((int)cwneWW.Minimum > Convert.ToInt32(txtWW.Text))
                {
                    presskye = (char)0;
                    txtWW.Text = cwneWW.Minimum.ToString();
                    return;
                }
                if (presskye == (char)Keys.Return)
                {
                    cwneWW.Value = dval;
                }
            }
            presskye = (char)0;

        }

        private void txtWLWW_Leave(object sender, EventArgs e)
        {
            if (sender.Equals(txtWL))
            {
                //変更2015/01/08hata
                //if (txtWL.Text == "") txtWL.Text = cwneWL.Minimum.ToString();
                if (txtWL.Text == "") txtWL.Text = cwneWL.Value.ToString();

            }
            else if (sender.Equals(txtWW))
            {
                //変更2015/01/08hata
                //if (txtWW.Text == "") txtWW.Text = cwneWW.Minimum.ToString();
                if (txtWW.Text == "") txtWW.Text = cwneWW.Value.ToString();
            }
            //追加2015/01/08hata
            presskye = (char)Keys.Return;
            txtWLWW_TextChanged(sender, EventArgs.Empty);
        }

        //追加2015/01/08hata
        private void txtWLWW_KeyDown(object sender, KeyEventArgs e)
        {
            presskye = (char)e.KeyCode;
            switch (e.KeyCode)
            {
                //UpDownキー
                case Keys.Up:
                    if (sender.Equals(txtWL))
                    {
                        cwneWL.UpButton();
                    }
                    else if (sender.Equals(txtWW))
                    {
                        cwneWW.UpButton();
                    }
                    break;
                case Keys.Down:
                    if (sender.Equals(txtWL))
                    {
                        cwneWL.DownButton();
                    }
                    else if (sender.Equals(txtWW))
                    {
                        cwneWW.DownButton();
                    }
                    break;
            }
        }


        //追加2015/01/27hata
        public void InitTabEnable(int Index, bool value)
        {
            if (fraScanCondition[Index] != null)
            {
                if (fraScanCondition.GetLowerBound(0) <= Index & (fraScanCondition.GetUpperBound(0) >= Index) )
                    fraScanCondition[Index].Enabled = value;
            }
        }

        //*************************************************************************************************
        //機　　能： [エリア]タブ スキャンエリア選択時
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v26.00  12/27    (検S1)長野 新規作成
        //*************************************************************************************************
        private void optScanArea_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton r = sender as RadioButton;

            bool IsOK = false;

            if (r == null) return;

            //unCheckのときは何もしない
            if (!r.Checked) return;

            int Index = Array.IndexOf(optScanArea, r);

            if (Index < 0) return;

            scanAreaChangedIgnore = true;
            scanCondChangeIgnore = true;

            //[ガイド]タブのスキャン条件設定完了済みの場合は、移動成功時に、ボタンの内容を再セット
            bool setScanCondFlg = false;
            if (scanCondSetCmpFlg == true)
            {
                setScanCondFlg = true;
            }

            try
            {

                //機構部が動作可能かチェック
                if (!modMechaControl.IsOkMechaMove())
                {
                    throw new Exception();
                }

                //Rev26.00 add by chouno 2017/03/13
                if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
                {
                    throw new Exception();
                }

                //Rev23.10 X線切替有の場合は念のためテーブルを再読込 by長野 2015/11/24
                if (CTSettings.scaninh.Data.multi_tube == 0)
                {
                    if (CTSettings.scansel.Data.multi_tube == 0 || CTSettings.scansel.Data.multi_tube == 1)
                    {
                        ComLib.change_xray_com(CTSettings.scansel.Data.multi_tube);
                    }
                }


                if (optScanArea[Index].Checked == true)
                {
                    //対応するスキャンエリア半径(mm)取得
                    float ScanR = modScanCondition.getScanAreaForGuide(Index);

                    if (ScanR <= 0)
                    {
                        MessageBox.Show(CTResources.LoadResString(26001), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        throw new Exception();
                    }

                    //対応するスキャンモードとWスキャン有無を取得
                    int ScanModeIndex = 0;
                    int WScanFlg = 0;

                    modScanCondition.getScanModeForScanArea(Index, ref ScanModeIndex, ref WScanFlg);
                    if (ScanModeIndex == -1 || WScanFlg == -1)
                    {
                        MessageBox.Show(CTResources.LoadResString(26013), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        throw new Exception();
                    }

                    //条件セット
                    optScanMode[ScanModeIndex + 1].Checked = true;
                    chkW_Scan.Checked = (WScanFlg == 1);

                    //対応するFDD、テーブルZ軸位置を取得
                    float fdd = 0.0f;
                    float tableZ = 0.0f;
                    if (!modScanCondition.getMechaPosForScanArea(Index, ref fdd, ref tableZ))
                    {
                        MessageBox.Show(CTResources.LoadResString(26018), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new Exception();
                    }

                    //外観カメラ描画領域のサイズ
                    int Width = frmExObsCam.Instance.ClientSize.Width;
                    int Height = frmExObsCam.Instance.ClientSize.Height;
                    //2ndモニタ上に外観画像用画面がある場合の1画素サイズを取得(昇降位置max時)
                    float pixsize = frmExObsCam.Instance.pixsize * (1024.0f / Width);
                    //スキャンエリア(画素)
                    int ScanRPix = Convert.ToInt32(ScanR / pixsize);

                    //自動移動中は、外観制御enableをfalse
                    frmCTMenu.Instance.tsbtnExObsCam.Enabled = false;

                    //現在の昇降位置での撮影領域を計算
                    //float currentSizeByOpticalCamera = CTSettings.mechapara.Data.sampleTableSize[0] * ((CTSettings.scancondpar.Data.opticalPathForCamera + CTSettings.mecainf.Data.udab_pos) / CTSettings.scancondpar.Data.opticalPathForCamera);
                    //Rev99.99
                    float currentSizeByOpticalCamera = CTSettings.mechapara.Data.sampleTableSize[0] * ((CTSettings.scancondpar.Data.opticalPathForCamera - (CTSettings.t20kinf.Data.upper_limit / 100 - CTSettings.mecainf.Data.udab_pos)) / CTSettings.scancondpar.Data.opticalPathForCamera);

                    //現在の昇降位置での外観カメラ画像１画素サイズ計算
                    float pixsizeByCurrentUDPos = currentSizeByOpticalCamera / Width;
                    //現在の昇降位置で、scanRを満たす画素数を計算
                    //int ScanRPixByCurrentUDPos = Convert.ToInt32(ScanR / pixsizeByCurrentUDPos);
                    //Rev99.99
                    int ScanRPixByCurrentUDPos = Convert.ToInt32(ScanR / pixsizeByCurrentUDPos * 0.95f);

                    //サイズに対応したROIを描画、描画フラグON
                    frmExObsCam.Instance.ScanAreaROISet(Width, Height, (float)Width / 2.0f, ScanRPixByCurrentUDPos, RoiData.RoiShape.ROI_CIRC);
                    frmExObsCam.Instance.ScanAreaROIOn = true;

                    //現在のii視野、FCD、FDD、Y軸をコモンにコピー
                    CTSettings.mecainf.Load();

                    CTSettings.mecainf.Data.iifield = modSeqComm.GetIINo();                  //I.I.視野
                    CTSettings.mecainf.Data.table_x_pos = (float)frmMechaControl.Instance.ntbTableXPos.Value;   //試料テーブル（光軸と垂直方向)座標[mm] （従来のＸ軸座標）も書き込む

                    //modMecainf.PutMecainf(ref modMecainf.mecainf);
                    CTSettings.mecainf.Write();

                    //FID/FCDをコモンに書き込む  'v15.0追加 by 間々田 2009/06/16
                    CTSettings.scansel.Data.fid = frmMechaControl.Instance.FIDWithOffset;    //FID
                    CTSettings.scansel.Data.fcd = frmMechaControl.Instance.FCDWithOffset;    //FCD

                    //modScansel.PutScansel(ref modScansel.scansel);
                    CTSettings.scansel.Write();

                    //Rev26.00 3月製品承認版では、昇降軸まで考慮した移動は行わないのでiniファイルから取得した昇降位置は、現在のｚ軸に書き換え
                    //考慮した移動が完成したら、iniファイルの値をそのまま使用すること by chouno 2017/01/18
                    tableZ = frmMechaControl.Instance.Udab_Pos;
                    IsOK = frmMechaMove.Instance.MechaMoveForAutoScanPos_ScanAreaSetForGuide(Convert.ToInt32((float)Width / 2.0f), Convert.ToInt32((float)Height / 2.0f), ScanRPix, Width, pixsize, fdd, tableZ);

                    //自動移動完了後、外観制御enableをtrue
                    frmCTMenu.Instance.tsbtnExObsCam.Enabled = true;

                    //ROI描画フラグOFF
                    frmExObsCam.Instance.ScanAreaROIOn = false;

                }
                else
                {
                    //ROI描画フラグOFF
                    frmExObsCam.Instance.ScanAreaROIOn = false;
                }
            }
            catch
            {
 
            }
            finally
            {
                scanAreaChangedIgnore = false;
                scanCondChangeIgnore = false;

                if (IsOK == true)
                {
                    scanAreaSetCmpFlg = true;
                    //[ガイド]タブのスキャン条件設定が完了状態なら、ここで条件再セット
                    if (setScanCondFlg == true)
                    {
                        int selectedScanCondIndex = modLibrary.GetOption(optScanCond);
                        if (selectedScanCondIndex >= 0)
                        {
                            frmMechaControl.Instance.MyUpdate();

                            setScanCond(selectedScanCondIndex);
                        }
                    }
                }
                else
                {
                    scanAreaSetCmpFlg = false;
                }
            }
            this.ActiveControl = this.SSTab1; //フォーカスが残ったままタブを切り替えるとイベントが実行されるので外す
        }

        //*************************************************************************************************
        //機　　能： [エリア]タブ スキャン条件選択時
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v26.00  12/27    (検S1)長野 新規作成
        //*************************************************************************************************
        private void optScanCond_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton r = sender as RadioButton;

            if (r == null) return;

            //unCheckのときは何もしない
            if (!r.Checked) return;

            int selectedIndexScanCond = Array.IndexOf(optScanCond, r);

            if (selectedIndexScanCond < 0) return;

            if (selectedIndexScanCond == 4 && cmbPreset.Text == String.Empty)
            {
                optScanCond4.Checked = false;
                return;
            }

            //if (selectedIndexScanCond == 4 && optScanMat[optScanMat.Length - 1].Checked == true)
            //{
            //    return;//画質タブ2で設定済み
            //}

            switch (selectedIndexScanCond)
            {
                case 0:
                    selectedIndexScanCond = CTSettings.iniValue.guideImgQualityIndex[0];
                    break;
                case 1:
                    selectedIndexScanCond = CTSettings.iniValue.guideImgQualityIndex[1];
                    break;
                case 2:
                    selectedIndexScanCond = CTSettings.iniValue.guideImgQualityIndex[2];
                    break;
                case 3:
                    selectedIndexScanCond = CTSettings.iniValue.guideImgQualityIndex[3];
                    break;
                case 4:
                    selectedIndexScanCond = 16;
                    break;
                default:
                    return;
            }

            setScanCond(selectedIndexScanCond);

            foreach (RadioButton rdb in optScanMat)
            {
                rdb.Checked = false;
            }

            this.ActiveControl = this.SSTab1; //フォーカスが残ったままタブを切り替えるとイベントが実行されるので外す
        }
        //*************************************************************************************************
        //機　　能： [エリア]タブ スキャン条件選択時
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v26.00  12/27    (検S1)長野 新規作成
        //*************************************************************************************************
        private void optScanMat_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton r = sender as RadioButton;

            if (r == null) return;

            //unCheckのときは何もしない
            if (!r.Checked) return;

            int selectedIndexScanMat = Array.IndexOf(optScanMat, r);

            if (selectedIndexScanMat < 0) return;

            //if (selectedIndexScanMat == optScanMat.Length - 1 && cmbPreset.Text == String.Empty)
            //{
            //    optScanMat[optScanMat.Length - 1].Checked = false;
            //    return;
            //}

            //if (selectedIndexScanMat == optScanMat.Length - 1 && optScanCond4.Checked == true)
            //{
            //    return; //画質タブ1のプリセットタブで設定済み
            //}

            setScanCond(selectedIndexScanMat);

            foreach (RadioButton rdb in optScanCond)
            {
                rdb.Checked = false;
            }

            //if (selectedIndexScanMat == optScanMat.Length - 1)
            //{
            //    optScanCond4.Checked = true;
            //}

            this.ActiveControl = this.SSTab1; //フォーカスが残ったままタブを切り替えるとイベントが実行されるので外す
        }
        //*************************************************************************************************
        //機　　能： [エリア]タブ スキャン条件セット
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： indexの条件をセット
        //
        //履　　歴： v26.00  01/16    (検S1)長野 新規作成
        //*************************************************************************************************
        private void setScanCond(int ScanCondIndex)
        {
            bool IsOK = false;

            //追加2014/10/07hata_v19.51反映
            int oldScanmode = 0;            //v18.00追加 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            int nowScanmode = 0;            //v18.00追加 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev25.00 Wスキャン対応 by長野 2016/07/11
            CheckState old_W_ScanCheckState = (CTSettings.scansel.Data.w_scan == 1 ? CheckState.Checked : CheckState.Unchecked);
            CheckState now_W_ScanCheckState = CheckState.Unchecked;

            try
            {
                string scanCondFileName = "";

                scanCondFileName = modScanCondition.getScanCondFileName(ScanCondIndex);

                if (scanCondFileName == "")
                {
                    MessageBox.Show(CTResources.LoadResString(26002), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Rev26.30/Rev26.15 修正 by chouno 2018/10/15
                if (ScanCondIndex == optScanMat.Length)
                {
                    if (!modScanCondition.LoadSCFile(scanCondFileName))
                    {
                        //失敗した場合メッセージを表示
                        //   指定されたスキャン条件ファイルからスキャン条件を設定できませんでした。
                        MessageBox.Show(StringTable.BuildResStr(9951, StringTable.IDS_CondFile), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    //スキャン条件ファイルからスキャン条件を設定
                    if (!modScanCondition.LoadScanCondFile(scanCondFileName))
                    {
                        //失敗した場合メッセージを表示
                        //   指定されたスキャン条件ファイルからスキャン条件を設定できませんでした。
                        MessageBox.Show(StringTable.BuildResStr(9951, StringTable.IDS_CondFile), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            
                //Rev26.30/Rev26.15 del by chouno 2018/10/15
                //else
                //{
                    UpdateFpdGainInteg();

                    //スキャン条件の内容を画面に反映
                    LoadScanCondition();

                    //追加2014/10/07hata_v19.51反映
                    //今のスキャンモード     'v18.00追加 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    nowScanmode = modLibrary.GetOption(optScanMode);
                    //今のWスキャン //Rev25.00 追加 by長野 2016/07/11
                    now_W_ScanCheckState = chkW_Scan.CheckState;

                    //シフトスキャン⇔非シフトスキャン間の変更をしたとき 'v18.00追加 byやまおか 2011/07/23
                    //Wスキャンを条件に追加 Rev25.00 by長野 2016/07/11
                    if ((((oldScanmode == (int)ScanSel.ScanModeConstants.ScanModeShift) && (nowScanmode != (int)ScanSel.ScanModeConstants.ScanModeShift)) ||
                        ((oldScanmode != (int)ScanSel.ScanModeConstants.ScanModeShift) && (nowScanmode == (int)ScanSel.ScanModeConstants.ScanModeShift))) ||
                        (old_W_ScanCheckState != now_W_ScanCheckState))
                    {

                        //オートセンタリングをありにする
                        chkInhibit[3].CheckState = System.Windows.Forms.CheckState.Unchecked;
                        //★★★フラットパネルの幾何歪   'v18.00追加 byやまおか 2011/07/09
                        if (CTSettings.detectorParam.Use_FlatPanel)
                            ScanCorrect.FPD_DistorsionCorrect();

                    }

                    IsOK = true;
                //}
            }
            finally
            {
                if (IsOK == true)
                {
                    scanCondSetCmpFlg = true;
                }
                else
                {
                    scanCondSetCmpFlg = false;
                }
            }
        }
        //*************************************************************************************************
        //機　　能： [エリア]タブ スキャン条件、スキャンエリア設定完了状態の変更受付フラグのセット
        //
        //           変数名          [I/O] 型        内容
        //引　　数： true:受付不可,false:受付可
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v26.00  01/16    (検S1)長野 新規作成
        //*************************************************************************************************
        public void setScanAreaAndCondIgnoreFlg(bool ignoreFlg)
        {
            scanAreaChangedIgnore = ignoreFlg;
            scanCondChangeIgnore = ignoreFlg;
        }
        //*************************************************************************************************
        //機　　能： [エリア]タブ スキャン条件、スキャンエリア設定完了状態の変更
        //
        //           変数名          [I/O] 型        内容
        //引　　数： true:完了状態,false:未完了状態
        //戻 り 値： なし
        //
        //補　　足： ignoreフラグを無視して設定する
        //
        //履　　歴： v26.00  01/16    (検S1)長野 新規作成
        //*************************************************************************************************
        public void setScanAreaAndCmpFlg(bool cmpFlg)
        {
            setScanAreaAndCondIgnoreFlg(false);

            scanAreaChangedIgnore = cmpFlg;
            scanCondChangeIgnore = cmpFlg;
        }

        //*************************************************************************************************
        //機　　能： [校正]タブ スキャン開始前、自動校正を実行するかどうかのフラグ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 
        //
        //履　　歴： v26.00  01/16    (検S1)長野 新規作成
        //*************************************************************************************************
        private void chkAutoCorrectBeforeScan_CheckedChanged(object sender, EventArgs e)
        {
            autoCorBeforeScanFlg = chkAutoCorrectBeforeScan.Checked;
        }

        //追加2017/01/19hata
        //自動X線VI設定
        private void cmdXrayAutoVI_Click(object sender, MouseEventArgs e)
        {
            int vimode = -1;
            if (sender.Equals(cmdXrayAutoVIHigh))
            {
                vimode = 1;
            }
            else if (sender.Equals(cmdXrayAutoVIMid))
            {
                vimode = 2;
            }
            else if (sender.Equals(cmdXrayAutoVILow))
            {
                vimode = 3;
            }

            if (vimode == 1 || vimode == 2 || vimode == 3)
            {

                if (bXrayAuto)
                {
                    //AutoViCancel
                    modXrayAutoVI.XrayAutoVICancel();
                    return;
                }

                if (vimode == 1)
                {
                    //High
                    cmdXrayAutoVIHigh.FlatStyle = FlatStyle.Standard;
                    cmdXrayAutoVIHigh.BlinkInterval = CWSpeeds.cwSpeedFastest;
                    cmdXrayAutoVIMid.Enabled = false;
                    cmdXrayAutoVILow.Enabled = false;
                    //cmdXrayAutoVIHigh.BackColor = Color.Lime;
                }
                else if (vimode == 2)
                {
                    //Mid
                    cmdXrayAutoVIMid.FlatStyle = FlatStyle.Standard;
                    cmdXrayAutoVIMid.BlinkInterval = CWSpeeds.cwSpeedFastest;
                    cmdXrayAutoVIHigh.Enabled = false;
                    cmdXrayAutoVILow.Enabled = false;
                    //cmdXrayAutoVIMid.BackColor = Color.Lime;
                }
                else if (vimode == 3)
                {
                    //Low
                    cmdXrayAutoVILow.FlatStyle = FlatStyle.Standard;
                    cmdXrayAutoVILow.BlinkInterval = CWSpeeds.cwSpeedFastest;
                    cmdXrayAutoVIHigh.Enabled = false;
                    cmdXrayAutoVIMid.Enabled = false;
                    //cmdXrayAutoVILow.BackColor = Color.Lime;
                }

                bXrayAuto = true;
                modXrayAutoVI.XrayAutoVIModeProc(vimode);

                cmdXrayAutoVIHigh.BackColor = SystemColors.Control;
                cmdXrayAutoVIMid.BackColor = SystemColors.Control;
                cmdXrayAutoVILow.BackColor = SystemColors.Control;
                cmdXrayAutoVIHigh.Enabled = true;
                cmdXrayAutoVIMid.Enabled = true;
                cmdXrayAutoVILow.Enabled = true;
                cmdXrayAutoVIHigh.Value = false;
                cmdXrayAutoVIHigh.FlatStyle = FlatStyle.Standard;
                cmdXrayAutoVIMid.Value = false;
                cmdXrayAutoVIMid.FlatStyle = FlatStyle.Standard;
                cmdXrayAutoVILow.Value = false;
                cmdXrayAutoVILow.FlatStyle = FlatStyle.Standard;

                bXrayAuto = false;
            }
        }

        //*************************************************************************************************
        //機　　能： [ガイド]タブ 画質自動設定ボタン描画
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 
        //
        //履　　歴： v26.00  08/31    (CT開)長野 新規作成
        //*************************************************************************************************
        private void optScanCond0_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.DrawImage(scanCondImg0, 5, 5, 50, 50);
        }

        private void optScanCond1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.DrawImage(scanCondImg1, 5, 5, 50, 50);
        }

        private void optScanCond2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.DrawImage(scanCondImg2, 5, 5, 50, 50);
        }

        private void optScanCond3_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.DrawImage(scanCondImg3, 5, 5, 50, 50);
        }

        private void optScanCond4_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.DrawImage(scanCondImg4, 5, 5, 50, 50);
        }
        private void optScanPreset_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.DrawImage(scanCondImg4, 2, 2, 36, 36);
        }
        //*************************************************************************************************
        //機　　能： [ガイド]タブ プリセット用コントロールリスト更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： プリセット名のリスト
        //戻 り 値： なし
        //
        //補　　足： 
        //
        //履　　歴： v26.00  08/31    (CT開)長野 新規作成
        //*************************************************************************************************
        private void setCmbPresetList(List<string> PresetName)
        {
            cmbPreset.Items.Clear();
            
            for (int cnt = 0; cnt < PresetName.Count; cnt++)
            {
                cmbPreset.Items.Add(PresetName[cnt]);
            }
        }
        //*************************************************************************************************
        //機　　能： [ガイド]タブ プリセット用 [設定]ボタンクリック処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 
        //
        //履　　歴： v26.00  08/31    (CT開)長野 新規作成
        //*************************************************************************************************
        private void btnPreset_Click(object sender, EventArgs e)
        {
            if (modLibrary.IsExistForm("frmScanCondition")) frmScanCondition.Instance.Close();
            Application.DoEvents();

            frmScanCondition.Instance.Setup(true,presetSetting:true);
            
 
        }
        //*************************************************************************************************
        //機　　能： [ガイド]タブ プリセット用 [設定]ボタンクリック処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： 1:追加,2:削除
        //戻 り 値： なし
        //
        //補　　足： 
        //
        //履　　歴： v26.00  08/31    (CT開)長野 新規作成
        //*************************************************************************************************
        public void UpdatePresetList(int Mode)
        {
            //cmbPresetList更新
            setCmbPresetList(modScanCondition.PresetName);

            //プリセットのファイル更新
            modScanCondition.writePresetList(modScanCondition.PresetName, modScanCondition.PresetPath);

            if(Mode == 1)//追加の場合は、最後のインデックスにする
            {
                cmbPreset.SelectedIndex = modScanCondition.PresetName.Count - 1;
                cmbPreset.SelectedIndex = modScanCondition.PresetName.Count - 1;
            }
            else if (Mode == 2) //削除した場合はプリセット解除
            {
                optScanCond[4].Checked = false;
                txtPresetComment.Text = "";
                modScanCondition.PresetSelectedIndex = -1;

                toolTip2.SetToolTip(optScanCond[4],"");

            }
        }
        //*************************************************************************************************
        //機　　能： [ガイド]タブ プリセット用 コンボボックスからプリセット設定処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 
        //
        //履　　歴： v26.00  08/31    (CT開)長野 新規作成
        //*************************************************************************************************
        private void cmbPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbox = sender as ComboBox;
            modScanCondition.PresetSelectedIndex = cmbox.SelectedIndex;

            string FileName = Path.Combine(AppValue.PathScanCondPresetFile,cmbPreset.Text) + "-SC.csv";

            if (cmbPreset.Text != String.Empty)
            {
                if(File.Exists(FileName))
                {
                    setPresetToolTip(FileName);

                    if (optScanCond[4].Checked == false)
                    {
                        optScanCond4.Checked = true;
                    }
                    //else if(optScanMat[optScanMat.Length - 1].Checked == false)
                    //{
                    //    optScanMat[optScanMat.Length - 1].Checked = true;
                    //}
                    else
                    {
                        setScanCond(optScanMat.Length);
                    }

                    
                }
                else
                {
                    //エラー処理
                }
            }
        }
        //*************************************************************************************************
        //機　　能： [ガイド]タブ プリセット用 コンボボックスからプリセット設定処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 
        //
        //履　　歴： v26.00  08/31    (CT開)長野 新規作成
        //*************************************************************************************************
        private void setPresetToolTip(string FileName)
        {
            if (File.Exists(FileName))
            {
                string comment = "";
                modScanCondition.getPresetFileItem(FileName, "comment", ref comment);
                txtPresetComment.Text = comment;

                string MatSize = "";
                string SliceNum = "";
                string ViewNum = "";
                string IntegNum = "";
                string IntegTimeIndex = "";
                string IntegTime = "";
                string Space1 = "        ";
                string Space2 = "    ";
                string Space3 = "  ";
                string Colon = "：";

                modScanCondition.getPresetFileItem(FileName, "martix_size", ref MatSize);
                modScanCondition.getPresetFileItem(FileName, "scan_view", ref ViewNum);
                modScanCondition.getPresetFileItem(FileName, "scan_integ_number", ref IntegNum);
                modScanCondition.getPresetFileItem(FileName, "k", ref SliceNum);
                modScanCondition.getPresetFileItem(FileName, "fpd_integ", ref IntegTimeIndex);
                IntegTime = Math.Floor((modCT30K.fpd_integlist[Convert.ToInt32(IntegTimeIndex)])).ToString("0");

                toolTip2.SetToolTip(optScanCond[4], CTResources.LoadResString(12814) + Colon + MatSize + "\n" +
                                                        CTResources.LoadResString(9059) + Space2 + Colon + SliceNum + "\n" +
                                                        CTResources.LoadResString(12808) + Space1 + Colon + ViewNum + "\n" +
                                                        CTResources.LoadResString(12809) + Space1 + Colon + IntegNum + "\n" +
                                                        CTResources.LoadResString(20164) + "(msec)" + Space3 + Colon + IntegTime);
                //toolTip2.SetToolTip(optScanMat[16], CTResources.LoadResString(12814) + Colon + MatSize + "\n" +
                //                         CTResources.LoadResString(9059) + Space2 + Colon + SliceNum + "\n" +
                //                         CTResources.LoadResString(12808) + Space1 + Colon + ViewNum + "\n" +
                //                         CTResources.LoadResString(12809) + Space1 + Colon + IntegNum + "\n" +
                //                         CTResources.LoadResString(20164) + "(msec)" + Space3 + Colon + IntegTime);
            }
        }
        
        //*************************************************************************************************
        //機　　能： [ガイド]タブ 画質ボタン用 ツールチップ描画
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 
        //
        //履　　歴： v26.00  08/31    (CT開)長野 新規作成
        //*************************************************************************************************
        private void toolTip2_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();

            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Near;
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                sf.FormatFlags = StringFormatFlags.NoWrap;
                using (Font f = new Font("MS Gothic", 10))
                {
                    e.Graphics.DrawString(e.ToolTipText, f,
                    SystemBrushes.ControlText, e.Bounds, sf);
                }
            }
        }
        //*************************************************************************************************
        //機　　能： [ガイド]タブ 画質ボタン用 ツールチップ描画
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 
        //
        //履　　歴： v26.00  08/31    (CT開)長野 新規作成
        //*************************************************************************************************
        private void toolTip2_Popup(object sender, PopupEventArgs e)
        {
            using (Font f = new Font("MS Gothic", 10))
            {
                e.ToolTipSize = TextRenderer.MeasureText(toolTip2.GetToolTip(e.AssociatedControl), f);
            }
        }

        private void tabPage8_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(scanMatAxisPen, 120, 220, 325, 220);//画質マトリクス 解像度軸 Rev26.00 add by chouno 2016/12/28
            e.Graphics.DrawLine(scanMatAxisPen, 120, 222, 120, 15);  //画質マトリクス S/N軸    Rev26.00 add by chouno 2016/12/28
        }
        //*************************************************************************************************
        //機　　能： [ガイド]タブ 画質タブ切り替え処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 
        //
        //履　　歴： v26.00  2017/09/06    (CT開)長野 新規作成
        //*************************************************************************************************
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                tabControl1.Size = new Size(443, 269);
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                tabControl1.Size = new Size(443, 325);
            }
        }

        private void cmdXrayAutoVILow_ValueChanged(object sender, EventArgs e)
        {
            if (cmdXrayAutoVILow.Value == true)
            {
                cmdXrayAutoVILow.BlinkInterval = CWSpeeds.cwSpeedFastest;
            }
            else
            {
                cmdXrayAutoVILow.BlinkInterval = CWSpeeds.cwSpeedOff;
                if (bXrayAuto == true)
                {
                    modXrayAutoVI.XrayAutoVICancel();
                    modXrayControl.XrayOff();
                }
            }
        }

        private void cmdXrayAutoVIMid_ValueChanged(object sender, EventArgs e)
        {
            if (cmdXrayAutoVIMid.Value == true)
            {
                cmdXrayAutoVIMid.BlinkInterval = CWSpeeds.cwSpeedFastest;
            }
            else
            {
                cmdXrayAutoVIMid.BlinkInterval = CWSpeeds.cwSpeedOff;
                if (bXrayAuto == true)
                {
                    modXrayAutoVI.XrayAutoVICancel();
                    modXrayControl.XrayOff();
                }
            }
        }

        private void cmdXrayAutoVIHigh_ValueChanged(object sender, EventArgs e)
        {
            if (cmdXrayAutoVIHigh.Value == true)
            {
                cmdXrayAutoVIHigh.BlinkInterval = CWSpeeds.cwSpeedFastest;
            }
            else
            {
                cmdXrayAutoVIHigh.BlinkInterval = CWSpeeds.cwSpeedOff;
                if (bXrayAuto == true)
                {
                    modXrayAutoVI.XrayAutoVICancel();
                    modXrayControl.XrayOff();
                }
            }
        }
    }
}
