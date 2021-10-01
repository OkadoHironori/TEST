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
//
//using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{

	///* ************************************************************************** */
	///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： frmRetryCondition.frm                                       */
	///* 処理概要　　： 再構成リトライ条件の設定                                    */
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
	///* V4.0        01/03/01    (ITC)    鈴山　修   ﾓｰﾀﾞﾙﾌｫｰﾑ→MDI子ﾌｫｰﾑに変更     */
	///* v8.0        07/02/19    (CATS)   Ohkado     FFT/ｺﾝﾎﾞﾘｭｰｼｮﾝ切り替え         */
	///* v14.2       08/04/08    (検SS)   YAMAKAGE   コーンRFC対応・強度中追加      */
	///* v15.0       09/01/21    (SI1)    間々田     リニューアル                   */
	///* v19.00      12/02/20    H.Nagai             BHC対応                        */
	///*                                                                            */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
	///* ************************************************************************** */
	public partial class frmRetryCondition : Form
	{
		//********************************************************************************
		//  共通データ宣言
		//********************************************************************************

		//再構成（ズーミング）する生データファイル名（拡張子なし形式）
		private string myRawName = null;

		//付帯情報パラメータ
		private float m_Z0 = 0;					//ｽｷｬﾝ位置(非ﾍﾘｶﾙ)
		private float m_SW0 = 0;				//スライス厚初期値(mm)
		private float m_Zs0 = 0;				//再構成開始位置初期値(mm)
		private float m_Ze0 = 0;        		//再構成終了位置初期値(mm)
		private bool IsHelical = false;			//ヘリカルモード

		//スキャン条件設定値
		private float m_ZW = 0;			//可能Z方向幅(mm)
		private float m_Ksw = 0;		//1 - Sin(m_Theta0 / 2) / Cos(m_ThetaOff)
		private float m_ZW0 = 0;		//ビームZ幅(mm)
		private float m_Ze = 0;			//再構成終了位置(mm)
		private float m_Zp = 0;			//ヘリカルピッチ(mm)
		private float m_Zdas = 0;		//ヘリカル開始位置(mm)       '追加 2000/10/10 巻淵
		private float m_Zdae = 0;		//ヘリカル終了位置(mm)       '追加 2000/10/10 巻淵

		//イベント宣言           'v15.0追加 by 間々田 2009/01/21
		//public event ClickedEventHandler Clicked;
		//public delegate void ClickedEventHandler(modImgProc.CTButtonConstants theButton);

		//コーンか？
		private bool IsCone = false;

		//ズーミングか？
		private bool IsZooming = false;

		//処理実行中？
		private bool myBusy = false;

        //FormLoad完了フラグ
        private bool myLoad = false;

        //Rev20.00 ROI処理中かどうか
        private bool isExRoi = false;

		private RadioButton[] optMatrix = null;
		private RadioButton[] optFilter = null;
		private RadioButton[] optDirection = null;
		private RadioButton[] optReconMask = null;
		private RadioButton[] optFilterProcess = null;
		private RadioButton[] optRFC = null;
		private RadioButton[] optImageMode = null;
		private PictureBox[] PicBar1 = null;
		private PictureBox[] PicBar2 = null;
		private Panel[] fraRetry = null;

		private static frmRetryCondition _Instance = null;

		public frmRetryCondition()
		{
			InitializeComponent();

			optMatrix = new RadioButton[]{null, optMatrix1, optMatrix2, optMatrix3, optMatrix4, optMatrix5};
			optFilter = new RadioButton[]{null,optFilter1, optFilter2, optFilter3};
			optDirection = new RadioButton[] { optDirection0, optDirection1 };
			optReconMask = new RadioButton[] { optReconMask0, optReconMask1 };
			optFilterProcess = new RadioButton[] { optFilterProcess0, optFilterProcess1 };
			optRFC = new RadioButton[] { optRFC0, optRFC1, optRFC2, optRFC3 };
			optImageMode = new RadioButton[] { optImageMode0, optImageMode1, optImageMode2 };
			PicBar1 = new PictureBox[] { PicBar1_0, PicBar1_1, PicBar1_2, PicBar1_3, PicBar1_4 };
			PicBar2 = new PictureBox[] { PicBar2_0, PicBar2_1, PicBar2_2, PicBar2_3, PicBar2_4 };
			fraRetry = new Panel[] { fraRetry0, fraRetry1 };
		}

		public static frmRetryCondition Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmRetryCondition();
				}

				return _Instance;
			}
		}
        //*************************************************************************************************
        //機　　能： IsExRoiプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v20.00 2014/12/04 (検S1)長野    新規作成
        //*************************************************************************************************
        /// <summary>
        /// 
        /// </summary>
        public bool IsExRoi
        {
            get { return isExRoi; }
            set { isExRoi = value; }
        }

		//*************************************************************************************************
		//機　　能： IsBusyプロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v10.2 2005/08/22 (SI3)間々田    新規作成
		//*************************************************************************************************
		private bool IsBusy
		{
			get { return myBusy; }
			set
			{
				//設定値を保存
				myBusy = value;

				//「ＯＫ」ボタンと「停止」ボタンの切り替え
				cmdOK.Text = CTResources.LoadResString(myBusy ? StringTable.IDS_btnStop : StringTable.IDS_btnOK);

				//各コントロールのEnabledプロパティを制御
				sstRetry.Enabled = !myBusy;
				cmdCancel.Enabled = !myBusy;

				//マウスポインタの制御
				this.Cursor = (myBusy ? Cursors.AppStarting : Cursors.Default);
			}
		}


		//*******************************************************************************
		//機　　能： ＯＫボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		private void cmdOK_Click(object sender, EventArgs e)
		{
			//v19.00 BHCテーブルチェック
			if (!OptValueChk3()) return;

			//処理実行中の場合
			if (IsBusy)
			{
				//UserStopSet
				//
				//連続回転コーンビーム＋高速再構成の時は、RAMディスクのscanstopを使う v17.40 追加 by 長野
				//If smooth_rot_cone_flg = True Then
				//
				//    UserStopSet_rmdsk
				//
				//End If

				//実行中の処理に対して停止要求をする     'v17.50上記の処理を関数化 by 間々田 2011/02/17
				modCT30K.CallUserStopSet();

				return;
			}

			//ビジーフラグセット
			IsBusy = true;

			//各コントロールから値を取得する
			GetControls();

			//Clickedイベント通知
			//RaiseEvent Clicked(btnCTOK)
			//
			//ズーミング時
			//If IsZooming Then
			//
			//    'アンロード
			//    Unload Me
			//
			//    'コモン設定
			//    SetCommonForRetry myRawName, True
			//
			//    '再構成リトライ実行
			//    StartProcess IIf(frmImageInfo.IsConeBeam, CONERECON, RECONMST)
			//
			//End If

			//コモン設定
			modReconst.SetCommonForRetry(myRawName, IsZooming);

            //Rev20.00 撮影前にこの時点でのコモンをファイルに落とす by長野 2014/09/11
            if(ComLib.SaveSharedCTCommon() != 0)
            {
                return;
            }

            //Rev20.00 直前でガベージコレクションを実行しておく by長野 2014/11/04
            GC.Collect();

			//再構成リトライ実行
            modCT30K.StartProcess(IsCone ? AppValue.CONERECON : AppValue.RECONMST);
		}


		//*******************************************************************************
		//機　　能： キャンセルボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		private void cmdCancel_Click(object sender, EventArgs e)
		{
            this.Close();   //復活2015/01/26hata　Dispose前にCloseする
            //Rev20.00 showDialogなので、Disposeで破棄する by長野 2014/12/15
            this.Dispose();
		}


		//*******************************************************************************
		//機　　能： [画像から測定]ボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V14.1  07/08/28   やまおか      新規作成
		//           v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		private void cmdMeasureFrImg_Click(object sender, EventArgs e)
		{
			//指定した生データに対応する画像が存在する場合：表示画像と比較して相違があれば指定した生データに対応する画像を表示
			if (File.Exists(myRawName + ".img"))
			{
				//末尾の比較対象外となる文字数
				int IgnorDigits = 0;
                IgnorDigits = ((CTSettings.gImageinfo.Data.bhc == 1) ? 4 : 0);		//コーン再構成のときは末尾４桁を比較しない

				//表示画像と再構成画像のKeyNameが一致していなければ、imgを開く
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				If Not (UCase$(frmImageInfo.Target) Like UCase$(Left$(myRawName, Len(myRawName) - IgnorDigits) & String$(IgnorDigits, "#"))) Then
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
				
                //Rev20.00 Stringのメソッドで判定するように変更 by長野 2014/12/04
                //StringBuilder pattern = new StringBuilder(myRawName.Substring(0, myRawName.Length - IgnorDigits).ToUpper());
                String pattern = myRawName.Substring(0, myRawName.Length - IgnorDigits);

                //pattern.Insert(pattern.Length, @"\d", IgnorDigits);
                //if (!Regex.IsMatch(frmImageInfo.Instance.Target.ToUpper(), pattern.ToString()))
                if((frmImageInfo.Instance.Target.ToString().IndexOf(pattern)) < 0)
                {
                    //画像の表示
                    frmScanImage.Instance.Target = myRawName + ".img";
                }
     
			}
			//指定した生データに対応する画像が存在しない場合：画像を表示していないとき
			else if (string.IsNullOrEmpty(frmScanImage.Instance.Target))
			{
				//メッセージを表示：対応する画像ファイルがありません。
				MessageBox.Show(CTResources.LoadResString(12836), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			//指定した生データに対応する画像が存在しない場合：画像を表示しているとき
			else
			{
				//ダイアログ表示：対応する画像ファイルがありません。表示している画像から測定しますか？
				DialogResult result = MessageBox.Show(CTResources.LoadResString(12836) + CTResources.LoadResString(12838), 
													  Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				if (result == DialogResult.No) return;
			}

			//「画像から測定」ボタンを使用不可にする
			//cmdMeasureFrImg.Enabled = False
			//Me.Enabled = False
            //Rev20.00 追加 by長野 2014/12/04
            IsExRoi = true;
            //変更2015/1/17hata_非表示のときにちらつくため
            //this.Hide();			//v16.01/v17.00変更 byやまおか 2010/02/24
            modCT30K.FormHide(this);

            //ROI制御スタート：角度測定
			//frmScanImage.ImageProc = RoiAngle
			frmScanControl frmScanControl = frmScanControl.Instance;
			frmScanControl.cmdImageProc_Click(frmScanControl.cmdImageProc[frmScanControl.ImageProcRoiAngle], EventArgs.Empty);		//v16.01/v17.00変更 byやまおか 2010/02/24
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
		private void frmRetryCondition_Load(object sender, EventArgs e)
		{

            //追加2014/09/11_dNet対応_hata
            //選択されていないTabのVisibleがFalse になるための処置
            sstRetry.SelectedIndex = 0;
            for (int i = 1; i <= fraRetry.GetUpperBound(0); i++)
            {
                fraRetry[i].Parent = this;
                fraRetry[i].Top = sstRetry.Top + sstRetry.ItemSize.Height;
                fraRetry[i].Left = sstRetry.Left;
            }
            
            //実行時はフラグをセット
			modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTReconstruct;

			//位置決め
			modCT30K.SetPosNormalForm(this);

			//ストリングテーブルから各コントロールのCaptionにセット
			SetCaption();

			//コントロールの初期化
			InitControls();

			//各コントロールに値をセットする
			SetControls();

			//各コントロールに値をセットする
			if (IsCone)
			{
				SetControlsForCone();
			}

			//ビジーフラグ初期化
			IsBusy = false;

            //Load完了
            myLoad = true;

		}


		//*************************************************************************************************
		//機　　能： フォームアンロード時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void frmRetryCondition_FormClosed(object sender, FormClosedEventArgs e)
		{
			//終了時はフラグをリセット
			modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTReconstruct);

			//ズーミング時：画像表示フォームを使用可にする
			//if (IsZooming && modLibrary.IsExistForm(frmScanImage.Instance)) frmScanImage.Instance.Enabled = true;
            if (modLibrary.IsExistForm("frmScanImage")) frmScanImage.Instance.Enabled = true;  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata

		}


		//*******************************************************************************
		//機　　能： ストリングテーブルから各コントロールのCaptionにセット
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V7.00  03/08/25 (SI4)間々田     新規作成
		//           v14.1  2007/08/28 やまおか      角度測定に対応
		//*******************************************************************************
		private void SetCaption()
		{
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

			//フォームのタイトル：「ズーミング条件」もしくは「再構成リトライ条件」
			this.Text = StringTable.BuildResStr(StringTable.IDS_Conditions, ((modCTBusy.CTBusy & modCTBusy.CTZooming) != 0 ? StringTable.IDS_Zooming : StringTable.IDS_Reconst));

			lblImageRotateAngleMinMax.Text = StringTable.GetResString(StringTable.IDS_Range, "-180", "180");		//'(-180～180)

			//v14.0追加(↓↓↓ここから↓↓↓) byやまおか 07/07/03
			//以下はコモンから取得
			optMatrix[1].Text = CTSettings.infdef.Data.matrixsize[0].GetString();				//256×256
            optMatrix[2].Text = CTSettings.infdef.Data.matrixsize[1].GetString();				//512×512
            optMatrix[3].Text = CTSettings.infdef.Data.matrixsize[2].GetString();				//1024×1024
            optMatrix[4].Text = CTSettings.infdef.Data.matrixsize[3].GetString();				//2048×2048
            optMatrix[5].Text = CTSettings.infdef.Data.matrixsize[4].GetString();				//4096×4096 v16.10 4096対応 by 長野
            optFilter[1].Text = CTSettings.infdef.Data.fc[0].GetString();						//Laks
            optFilter[2].Text = CTSettings.infdef.Data.fc[1].GetString();						//Shepp
            optFilter[3].Text = CTSettings.infdef.Data.fc[2].GetString();						//HightReso
            optFilterProcess[0].Text = CTSettings.infdef.Data.filter_process[0].GetString();	//FFT
            optFilterProcess[1].Text = CTSettings.infdef.Data.filter_process[1].GetString();	//ｺﾝﾎﾞﾘｭｰｼｮﾝ
            optRFC[0].Text = CTSettings.infdef.Data.rfc_char[0].GetString();					//無     'v14.0追加 byやまおか 07/07/17
            optRFC[1].Text = CTSettings.infdef.Data.rfc_char[1].GetString();					//弱     'v14.0追加 byやまおか 07/07/17
            optRFC[2].Text = CTSettings.infdef.Data.rfc_char[2].GetString();					//強→中 'v14.0追加 byやまおか 07/07/17
            optRFC[3].Text = CTSettings.infdef.Data.rfc_char[3].GetString();					//強     'v14.2追加 by YAMAKAGE 08/04/08

			//v19.00 ->(電S2)永井
            fraBHC.Text = CTSettings.infdef.Data.bhc.GetString();
			//<- v19.00

			//v14.0追加(↑↑↑ここまで↑↑↑) byやまおか 07/07/03


			//v15.0追加 以下コーンビーム関連 by 間々田 2009/02/19
			if (modCT30K.IsEnglish) fraZe.Font = new Font(fraZe.Font.Name, 9);

			fraSliceNumber.Text = CTResources.LoadResString(StringTable.IDS_SliceNumber);			//スライス枚数

			Label3.Text = CTResources.LoadResString(12067);			//スキャン  リトライ
			Label11.Text = fraZs.Text;								//再構成範囲
			Label10.Text = fraSW.Text;								//スライス厚

			if (modCT30K.IsEnglish) Label11.Top = Label11.Top - 5;

			if (!modCT30K.IsEnglish)
			{
				fraAcqView.Text = CTResources.LoadResString(StringTable.IDS_DataCollectViews);	//データ収集ビュー数
			}
			//英語の場合、"The number of data collection views"となるが、これでは収まりきれないので
			//"The number of views"（ビュー数）とし、注釈として"The number of data collection views"と表示する。
			else
			{
				fraAcqView.Text = CTResources.LoadResString(StringTable.IDS_ViewNum) + "*";		//ビュー数
				lblEnglishComment.Visible = true;
				fraZe.Text = CTResources.LoadResString(12068) + "*";
			}

            if (!modCT30K.IsEnglish)
            {
                Label8.Visible = false;
            }

			//v19.00 ->(電S2)永井
			//ﾋﾞｰﾑﾊﾄﾞﾆﾝｸﾞ補正フレーム内
            chkBHC.Text = CTResources.LoadResString(21200);											//ＢＨＣ処理を行う
			lblBhcDirTitle.Text = StringTable.LoadResStringWithColon(StringTable.IDS_DirName);		//ディレクトリ名：
			lblBhcFileTitle.Text = StringTable.LoadResStringWithColon(StringTable.IDS_TableName);
            cmdChangeBHCTable.Text = CTResources.LoadResString(StringTable.IDS_btnChange);			//変更...    'v8.0 追加 by Ohkado 2007/01/24
			//<- v19.00
			//v19.12 英語リソース化 by長野 2013/03/06
            cmdChangeBHCTableDefault.Text = CTResources.LoadResString(21312);
		}


		//*******************************************************************************
		//機　　能： 各コントロールの位置・サイズ等の初期化
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		private void InitControls()
		{
			bool IsOkFilterProcess = false;
			bool IsOkRFC = false;

			//コーンか？
            IsCone = (CTSettings.gImageinfo.Data.bhc == 1);

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			sstRetry.TabVisible(1) = IsCone
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
			if (!IsCone)
            {
                if(sstRetry.TabPages.Contains(tabPage1))
 				    sstRetry.TabPages.Remove(tabPage1);
            }

            //追加2014/10/07hata_v19.51反映
            //'ヘリカルモードが有効なときだけ表示 'v18.00追加 byやまおか 2011/07/24
            fraHelicalMode.Visible = (CTSettings.scaninh.Data.helical == 0);

			//フィルタ処理(FFT/Conv)の表示・非表示の設定
            IsOkFilterProcess = ((CTSettings.scaninh.Data.filter_process[0] == 0) && (CTSettings.scaninh.Data.filter_process[1] == 0));
			fraFilterProcess.Visible = IsOkFilterProcess;

			//RFCの表示・非表示の設定
            IsOkRFC = (CTSettings.scaninh.Data.rfc == 0);
			fraRFC.Visible = IsOkRFC;

			//フィルタ処理・RFCが共に非表示のとき
			if (!(IsOkFilterProcess || IsOkRFC))
			{
				//高さ調整
				//Me.Height = Me.Height - fraFilterProcess.Height 'サイズ調整しない
			}

            //Rev23.00 リトライ時の回転中心ch調整機能追加 by長野 2015/09/05
            //コーンのみの機能とする
            if (CTSettings.scaninh.Data.adj_center_ch == 0 && IsCone)
            {
                fraAdjCenterCh.Visible = true;
                if (fraHelicalMode.Visible == false)
                {
                    fraAdjCenterCh.Top = fraHelicalMode.Top;
                    fraAdjCenterCh.Left = fraHelicalMode.Left;
                    fraAdjCenterCh.Height = fraHelicalMode.Height;
                    fraAdjCenterCh.Width = fraHelicalMode.Width;
                }
                cwneAdjCenterCh.Minimum = 1;//初期最小値
                cwneAdjCenterCh.Minimum = CTSettings.scancondpar.Data.mainch[0];//初期最大値
            }
            else
            {
                fraAdjCenterCh.Visible = false;
            }

            //Rev20.00 表示・非表示を全てinhを使った制御に変更 by長野 2015/01/16
            //コーンビーム再構成を行う場合
            //if (IsCone)
            //{
            //    optMatrix[1].Enabled = true;
            //    //Rev20.00  2048コーン by長野 2015/01/09
            //    //optMatrix[4].Enabled = true;
            //    //optMatrix[4].Enabled = false;
            //    optMatrix[4].Enabled = (CTSettings.scaninh.Data.cone_matrix[3] == 0);
            //    //v16.10 4096を追加 by 長野 2010/02/03
            //    //optMatrix[5].Enabled = false;
            //    //Rev20.00 4096コーン by長野 2015/01/09
            //    //optMatrix[5].Enabled = true;
            //    optMatrix[5].Enabled = (CTSettings.scaninh.Data.cone_matrix[4] == 0);
            //}
            ////通常の再構成を行う場合
            //else
            //{
            //    optMatrix[1].Enabled = false;
            //    optMatrix[4].Enabled = (CTSettings.scaninh.Data.scan_matrix[3] == 0);
            //    //v16.10 4096を追加 by 長野 2010/02/03
            //    optMatrix[5].Enabled = (CTSettings.scaninh.Data.scan_matrix[4] == 0);
            //}

            if (IsCone)
            {
                optMatrix[1].Enabled = (CTSettings.scaninh.Data.cone_matrix[0] == 0);
                optMatrix[2].Enabled = (CTSettings.scaninh.Data.cone_matrix[1] == 0);
                optMatrix[3].Enabled = (CTSettings.scaninh.Data.cone_matrix[2] == 0);
                optMatrix[4].Enabled = (CTSettings.scaninh.Data.cone_matrix[3] == 0);
                optMatrix[5].Enabled = (CTSettings.scaninh.Data.cone_matrix[4] == 0);
            }
            //通常の再構成を行う場合
            else
            {
                optMatrix[1].Enabled = (CTSettings.scaninh.Data.scan_matrix[0] == 0);
                optMatrix[2].Enabled = (CTSettings.scaninh.Data.scan_matrix[1] == 0);
                optMatrix[4].Enabled = (CTSettings.scaninh.Data.scan_matrix[2] == 0);
                optMatrix[4].Enabled = (CTSettings.scaninh.Data.scan_matrix[3] == 0);
                optMatrix[5].Enabled = (CTSettings.scaninh.Data.scan_matrix[4] == 0);
            }

			//v19.01 FPDかつv19.00未満は再構成リトライ時のBHCは不可とする 追加 by長野 2012/06/27

			//if ((modImageInfo.GetImageInfoVerNumber(modImageInfo.gImageInfo.version) >= 19) || 
			//	!(modImageInfo.gImageInfo.detector == (int)DetectorConstants.DetTypePke))
            if ((modImageInfo.GetImageInfoVerNumber(CTSettings.gImageinfo.Data.version.GetString()) >= 19) ||
               !(CTSettings.gImageinfo.Data.detector == (int)DetectorConstants.DetTypePke))
            {
                //v19.00(電S2)永井
                //fraBHC.Visible = (CTSettings.scaninh.Data.mbhc == 0);

                //Rev26.00 ファントムレスBHC画面追加 by井上 2017/01/18
                if (CTSettings.scaninh.Data.mbhc == 0)
                {
                    fraBHC.Visible = true;
                    fraBHCPhantomless.Visible = false;

                }
                else if (CTSettings.scaninh.Data.mbhc_phantomless == 0)
                {
                    fraBHC.Visible = false;
                    fraBHCPhantomless.Visible = true;
                    fraBHCPhantomless.Location = new Point(fraBHC.Left, fraBHC.Top);

                    //コンボボックスにファントムレスBHCの材質追加
                    cmbBHCPhantomless.Items.Clear();
                    int BHCmat_cnt = 0;
                    while (BHCmat_cnt < modBHC.BHCmatnum)
                    {
                        cmbBHCPhantomless.Items.Add(modBHC.BHCMaterial[BHCmat_cnt]);
                        BHCmat_cnt++;
                    }
                }
			}
			else
			{
				fraBHC.Visible = false;
                fraBHCPhantomless.Visible = false;//Rev26.00 ファントムレスBHC by 井上 2017/02/08
			}

			//マトリクスサイズオプションボタンの位置の調整
			modLibrary.RePosOption(optMatrix);

			//ズーミング時は、再構成形状・画像回転角度・画像方向を非表示にする
			if ((modCTBusy.CTBusy & modCTBusy.CTZooming) != 0)
			{
				fraReconMask.Visible = false;
				fraImageRotateAngle.Visible = false;
				fraDirection.Visible = false;

				//フォームの幅を調整
				//Me.width = fraReconMask.Left                       'サイズ調整しない
			}

            
			//フレームの境界線を消す
			for (int i = fraRetry.GetLowerBound(0); i <= fraRetry.GetUpperBound(0); i++)
			{
				fraRetry[i].BorderStyle = BorderStyle.None;
			}

            //追加2014/10/07hata_v19.51反映
            //'NumEditの上下ボタン長押し加速のタイミング変更  'v18.00追加 byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (cwneSW.Accelerations.Count > 0) cwneSW.Accelerations.Clear();
            if (cwneDelta_Z.Accelerations.Count > 0) cwneDelta_Z.Accelerations.Clear();
            if (cwneK.Accelerations.Count > 0) cwneK.Accelerations.Clear();
            if (cwneZs.Accelerations.Count > 0) cwneZs.Accelerations.Clear();
            cwneSW.Accelerations.Add(new NumericUpDownAcceleration(5, (decimal)0.5));
            cwneDelta_Z.Accelerations.Add(new NumericUpDownAcceleration(5, (decimal)0.02));
            cwneK.Accelerations.Add(new NumericUpDownAcceleration(5, 20));
            cwneZs.Accelerations.Add(new NumericUpDownAcceleration(5, 5));       
		    //'NumEditの上下ボタンのインクリメント値を変更    'v18.00追加 byやまおか 2011/07/30 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            cwneZs.Increment = (decimal)0.1;

        }


		//*******************************************************************************
		//機　　能： ダイアログ処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： RawBaseName     [I/ ] String    再構成（ズーミング）する生データファイル名
		//                                           （拡張子なし形式）
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//           v14.1  2007/08/28 やまおか      角度測定に対応
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		//public void ShowDialog(string theRawName, bool theZooming = false)
		public void Dialog(string theRawName, bool theZooming = false)
		{
			IsZooming = theZooming;

			myRawName = theRawName;

			//表示
			//Me.Show , frmCTMenu
			this.ShowDialog(frmCTMenu.Instance);	//v15.10変更 byやまおか 2009/12/01
        }


		//*******************************************************************************
		//機　　能： 各コントロールに値をセットする
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.5  06/05/31 (WEB)間々田     新規作成
		//*******************************************************************************
		private void SetControls()
		{
            IsCone = (CTSettings.gImageinfo.Data.bhc == 1);
            ntbBias.Value = (decimal)CTSettings.gImageinfo.Data.image_bias;							//バイアス
            ntbSlope.Value = (decimal)CTSettings.gImageinfo.Data.image_slope;						//スロープ

            modLibrary.SetOption(optMatrix, modCommon.MyCtinfdef.matsiz.GetIndexByStr(modLibrary.RemoveNull(CTSettings.gImageinfo.Data.matsiz.GetString()), 2));
            modLibrary.SetOption(optFilter,modCommon.MyCtinfdef.fc.GetIndexByStr(modLibrary.RemoveNull(CTSettings.gImageinfo.Data.fc.GetString()), 0) + 1);
            
            modLibrary.SetOption(optDirection, CTSettings.gImageinfo.Data.image_direction);			//画像方向
            modLibrary.SetOption(optReconMask, CTSettings.gImageinfo.Data.recon_mask);				//再構成形状
            //変更2015/02/02hata_Max/Min範囲のチェック
            //cwneImageRotateAngle.Value = (decimal)CTSettings.gImageinfo.Data.recon_start_angle;		//画像回転角度
            cwneImageRotateAngle.Value = modLibrary.CorrectInRange((decimal)CTSettings.gImageinfo.Data.recon_start_angle, cwneImageRotateAngle.Minimum, cwneImageRotateAngle.Maximum);		//画像回転角度
            
            //Rev23.00 追加 by長野 2015/09/07
            if (CTSettings.scaninh.Data.adj_center_ch == 0　&& IsCone)
            {
                lblOrgCh.Text = CTSettings.gImageinfo.Data.nc.ToString("#.0");
                float tmp1 = (float)CTSettings.gImageinfo.Data.nc;
                float tmp2 = (float)CTSettings.scancondpar.Data.mainch[0] - tmp1;
                if (tmp1 >= tmp2)
                {
                    cwneAdjCenterCh.Maximum = (decimal)(tmp1 - 2);//0オリジンかつ、1ch内側
                    cwneAdjCenterCh.Minimum = (decimal)(-tmp2 + 1);//1ch内側
                    
                }
                else if (tmp1 < tmp2)
                {
                    cwneAdjCenterCh.Maximum = (decimal)(tmp2 - 2);
                    cwneAdjCenterCh.Minimum = (decimal)(-tmp1 + 1);
                }
                lblAdjMin.Text = cwneAdjCenterCh.Minimum.ToString();
                lblAdjMax.Text = cwneAdjCenterCh.Maximum.ToString();
                //cwneAdjCenterCh.Value = modLibrary.CorrectInRange((decimal)CTSettings.gImageinfo.Data.numOfAdjCenterCh, cwneAdjCenterCh.Minimum, cwneAdjCenterCh.Maximum);	//回転中心ch調整量
                cwneAdjCenterCh.Value = (decimal)0.0;
            }

            modLibrary.SetOption(optFilterProcess, CTSettings.gImageinfo.Data.filter_proc, 0);		//フィルタ処理　'v13.0変更 by Ohkado 2007/02/19
            modLibrary.SetOption(optRFC, CTSettings.gImageinfo.Data.rfc, 0);						//RFC           'v14.0追加 byやまおか 2007/07/05

            //v19.00 ->(電S2)永井
			//ﾋﾞｰﾑﾊﾄﾞﾆﾝｸﾞ補正　  0(行わない),1(行う)
            chkBHC.CheckState = ((CTSettings.gImageinfo.Data.mbhc_flag == 1) ? CheckState.Checked : CheckState.Unchecked);

            //Rev20.00 追加 by長野 2014/12/04
            string FileName = null;
            //FileName.Remove(0);

            //FileName = modLibrary.AddExtension(Path.Combine(CTSettings.scansel.Data.mbhc_dir.GetString(), CTSettings.scansel.Data.mbhc_name.GetString()), ".csv");

            //txtBhcDirName.Text = string.IsNullOrEmpty(FileName) ? "" : Path.GetDirectoryName(FileName);		//ディレクトリ
            //txtBhcFileName.Text = string.IsNullOrEmpty(FileName) ? "" : Path.GetFileName(FileName);			//テーブル名

            //Rev20.00 変更 by長野 2015/02/15
            //if (CTSettings.scansel.Data.mbhc_dir.GetString() == "" || CTSettings.scansel.Data.mbhc_name.GetString() == "")
            if (CTSettings.gImageinfo.Data.mbhc_dir.GetString() == "" || CTSettings.gImageinfo.Data.mbhc_name.GetString() == "")
            {
                FileName = "";
            }
            else
            {
                FileName = modLibrary.AddExtension(Path.Combine(CTSettings.gImageinfo.Data.mbhc_dir.GetString(), CTSettings.gImageinfo.Data.mbhc_name.GetString()), ".csv");
            }

            txtBhcDirName.Text = string.IsNullOrEmpty(FileName) ? "" : Path.GetDirectoryName(FileName);		//ディレクトリ
            txtBhcFileName.Text = string.IsNullOrEmpty(FileName) ? "" : Path.GetFileName(FileName);			//テーブル名

            txtBhcFileNameChange();

			//ＢＨＣディレクトリ名
            //txtBhcDirName.Text = CTSettings.gImageinfo.Data.mbhc_dir.GetString();

			//ＢＨＣテーブル名                             'v8.1 追加 : 拡張子(.csv)を表示する。by Ohkado 2007/02/15
            //txtBhcFileName.Text = modLibrary.AddExtension(modLibrary.RemoveNull(CTSettings.gImageinfo.Data.mbhc_name.GetString()), ".csv");
			//txtBhcFileNameChange();
			//<- v19.00

            //Rev26.00 追加 by井上 2017/01/19
            cmbBHCPhantomless.SelectedIndex = CTSettings.gImageinfo.Data.mbhc_phantomless;
		}


		//*******************************************************************************
		//機　　能： 各コントロールから値を取得する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.5  06/05/31 (WEB)間々田     新規作成
		//*******************************************************************************
		private void GetControls()
		{
            //2014/11/07hata キャストの修正
            //CTSettings.gImageinfo.Data.image_bias = (int)ntbBias.Value;									//バイアス
            CTSettings.gImageinfo.Data.image_bias = Convert.ToInt32(ntbBias.Value);									//バイアス
            CTSettings.gImageinfo.Data.image_slope = (float)ntbSlope.Value;								//スロープ

            //.matsiz = GetFirstItem(optMatrix(GetOption(optMatrix)).Caption, "x")    'マトリクスサイズ
            //modLibrary.SetField(Convert.ToString(value), ref modImageInfo.gImageInfo.matsiz);			//マトリクスサイズ   'v15.01変更 マトリクス256を選んでも512が選ばれてしまうバグ修正 2009/09/01
            string str1 = optMatrix[modLibrary.GetOption(optMatrix)].Text;  //マトリクスサイズ                                  
            string pat = @"\D\d{1,}";
            string str2 = Regex.Replace(str1, pat, "");
            //double value = 0;
            //double.TryParse(str2, out value);
            //CTSettings.gImageinfo.Data.matsiz.SetString(Convert.ToString(value));
            CTSettings.gImageinfo.Data.matsiz.SetString(str2);

            //CTSettings.gImageinfo.Data.fc = optFilter[modLibrary.GetOption(optFilter)].Text;				//フィルタ関数
            CTSettings.gImageinfo.Data.fc.SetString(optFilter[modLibrary.GetOption(optFilter)].Text);

            CTSettings.gImageinfo.Data.image_direction = modLibrary.GetOption(optDirection);				//画像方向
            CTSettings.gImageinfo.Data.recon_mask = modLibrary.GetOption(optReconMask);					//再構成形状
            CTSettings.gImageinfo.Data.recon_start_angle = (float)cwneImageRotateAngle.Value;				//画像回転角度

            //Rev23.00 追加 回転中心ch調整 by長野 2015/09/07
            CTSettings.gImageinfo.Data.numOfAdjCenterCh = (float)cwneAdjCenterCh.Value;
            
            if ((CTSettings.scaninh.Data.filter_process[0] == 0) && (CTSettings.scaninh.Data.filter_process[1] == 0))			//ﾌｨﾙﾀ処理　'v13.00追加 by Ohkado 2007/04/16
			{
				//modImageInfo.gImageInfo.filter_process = optFilterProcess[modLibrary.GetOption(optFilterProcess)].Text;
                CTSettings.gImageinfo.Data.filter_process.SetString(optFilterProcess[modLibrary.GetOption(optFilterProcess)].Text);
            
            }
			else
			{
				//.filter_process = optFilterProcess(IIf(gImageInfo.filter_proc = 1, 1, 0)).Caption   'ﾌｨﾙﾀ処理 0:FFT　1:ｺﾝﾎﾞﾘｭｰｼｮﾝ  'v14.00削除 byやまおか 2007/07/23
				//v14.00追加 FFT/ｺﾝﾎﾞ切替機能がないときはinhibitで判断(0:FFT,1:ｺﾝﾎﾞﾘｭｰｼｮﾝ) byやまおか 2007/07/23
                //CTSettings.gImageinfo.Data.filter_process = optFilterProcess[((CTSettings.scaninh.Data.filter_process[1] == 0) ? 1 : 0)].Text;
                CTSettings.gImageinfo.Data.filter_process.SetString(optFilterProcess[((CTSettings.scaninh.Data.filter_process[1] == 0) ? 1 : 0)].Text);
            }

            CTSettings.gImageinfo.Data.rfc = ((CTSettings.scaninh.Data.rfc == 0) ? modLibrary.GetOption(optRFC) : 0);		//RFC機能があるときはﾎﾞﾀﾝの値を取得、機能がないときは"0:なし"    'v14.00追加 byやまおか 2007/07/23

			if (IsCone)
			{
                CTSettings.gImageinfo.Data.cone_image_mode = modLibrary.GetOption(optImageMode);		//画質
				//.width = CStr(cwneSW.Value)
				
                //modLibrary.SetField(cwneSW.Text, ref modImageInfo.gImageInfo.Width);				//v14.23変更 by 間々田 2008/12/15
                CTSettings.gImageinfo.Data.width.SetString(cwneSW.Text);

                CTSettings.gImageinfo.Data.delta_z = (float)cwneDelta_Z.Value;
                //2014/11/07hata キャストの修正
                //CTSettings.gImageinfo.Data.k = (int)cwneK.Value;
                CTSettings.gImageinfo.Data.k = Convert.ToInt32(cwneK.Value);
                CTSettings.gImageinfo.Data.zp = m_Zp;
                CTSettings.gImageinfo.Data.zs0 = (float)cwneZs.Value;
                CTSettings.gImageinfo.Data.ze0 = m_Ze;
			}

			//v19.00 ->(電S2)永井
			//ﾋﾞｰﾑﾊﾄﾞﾆﾝｸﾞ補正:0(行わない),1(行う)                                            'v8.0追加 by Ohkado 2007/02/19
			if (chkBHC.Visible)
			{
                CTSettings.gImageinfo.Data.mbhc_flag = Convert.ToInt32(chkBHC.Checked);
                
                //CTSettings.gImageinfo.Data.mbhc_dir = modLibrary.AddNull(txtBhcDirName.Text);											//BHCテーブルディレクトリ名
                //CTSettings.gImageinfo.Data.mbhc_dir.SetString(modLibrary.AddNull(txtBhcDirName.Text));
                //Rev20.00 frmScanCondition.cs側と処理を合わせる(最後に"\\"をつける)
                CTSettings.gImageinfo.Data.mbhc_dir.SetString(modLibrary.AddNull(txtBhcDirName.Text + "\\"));
                
                //CTSettings.gImageinfo.Data.mbhc_name = modLibrary.AddNull(modLibrary.RemoveExtension(txtBhcFileName.Text, ".csv"));	//BHCテーブル名
                CTSettings.gImageinfo.Data.mbhc_name.SetString(modLibrary.AddNull(modLibrary.RemoveExtension(txtBhcFileName.Text, ".csv")));
            
            }
			else
			{
                CTSettings.gImageinfo.Data.mbhc_flag = 0;
                CTSettings.gImageinfo.Data.mbhc_dir.SetString("");
                CTSettings.gImageinfo.Data.mbhc_name.SetString("");
			}
			//<- v19.00

            //Rev26.00 追加 by井上 2017/01/19
            if (fraBHCPhantomless.Visible)
            {
                if (cmbBHCPhantomless.SelectedIndex == 0)
                {
                    CTSettings.gImageinfo.Data.mbhc_phantomless = cmbBHCPhantomless.SelectedIndex;
                    CTSettings.gImageinfo.Data.mbhc_phantomless_c.SetString(cmbBHCPhantomless.Items[cmbBHCPhantomless.SelectedIndex].ToString());
                    CTSettings.gImageinfo.Data.mbhc_method = modBHC.BHCmethod[cmbBHCPhantomless.SelectedIndex];//BHC無しであれば、0にする。1では従来BHCが実行されるから。Rev26.00 add 2017/04/19
                }
                else if (cmbBHCPhantomless.SelectedIndex > 0)
                {
                    CTSettings.gImageinfo.Data.mbhc_method = modBHC.BHCmethod[cmbBHCPhantomless.SelectedIndex];

                    if (CTSettings.gImageinfo.Data.mbhc_method == 1)//従来BHC
                    {
                        CTSettings.gImageinfo.Data.mbhc_dir.SetString("C:\\CT\\ScanCorrect\\phantomlessBHC\\");
                        CTSettings.gImageinfo.Data.mbhc_name.SetString(cmbBHCPhantomless.SelectedIndex.ToString() + "_BHC_" + cmbBHCPhantomless.Items[cmbBHCPhantomless.SelectedIndex].ToString());
                        CTSettings.gImageinfo.Data.mbhc_phantomless = cmbBHCPhantomless.SelectedIndex;
                        CTSettings.gImageinfo.Data.mbhc_phantomless_c.SetString(cmbBHCPhantomless.Items[cmbBHCPhantomless.SelectedIndex].ToString());
                    }
                    else if (CTSettings.gImageinfo.Data.mbhc_method == 0)//ファントムレスBHC
                    {
                        CTSettings.gImageinfo.Data.mbhc_phantomless = cmbBHCPhantomless.SelectedIndex;
                        CTSettings.gImageinfo.Data.mbhc_phantomless_c.SetString(cmbBHCPhantomless.Items[cmbBHCPhantomless.SelectedIndex].ToString());
                    }
                }
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
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void frmRetryCondition_Resize(object sender, EventArgs e)
		{
			//ＯＫボタンの位置
			cmdOK.Location = new Point(this.Width / 2 - cmdOK.Width - 8, this.ClientSize.Height - cmdOK.Height - 11);

			//キャンセルボタンの位置
			cmdCancel.Location = new Point(this.Width / 2 + 8, this.ClientSize.Height - cmdCancel.Height - 11);
		}


		//********************************************************************************
		//機    能  ：  スライスピッチΔZを変えた場合
		//              変数名           [I/O] 型        内容
		//引    数  ：  flg_bar          Boolean
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V3.0   XX/XX/XX  ??????????????  新規作成
		//              V3.1   00/10/11  (CATS)巻淵      変更
		//********************************************************************************
		private void delta_z_Change()
		{
			//エラー時の扱い
			try
			{
				//最大スライス枚数を更新
				UpdateKMax();

				//非ヘリカルの場合
				if (!IsHelical)
				{
					//最大スライス厚の更新
					UpdateSWMax();
				}

				//再構成範囲開始位置最大値・最小値の更新
				UpdateZsminmax();

				//再構成範囲終了位置の更新
				UpdateZe();

				return;
			}
			catch
			{
				//ｴﾗｰﾒｯｾｰｼﾞ
				modLibrary.ErrorDescription(StringTable.GetResString(9904, this.Name, "delta_z_Change"));
			}
		}


		//********************************************************************************
		//機    能  ：  スキャン枚数Kを変えた場合
		//              変数名           [I/O] 型        内容
		//引    数  ：  flg_bar          Boolean
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V3.0   XX/XX/XX  ??????????????  新規作成
		//              V3.1   00/10/11  (CATS)巻淵      変更
		//********************************************************************************
		private void K_Change()
		{
			//エラー時の扱い
			try
			{
				//スライスピッチのフレーム
				fraSlicePitch.Visible = (cwneK.Value > 1);

				//最大スライスピッチの更新
				UpdateDeltaZMax();

				//非ヘリカルの場合
				if (!IsHelical)
				{
					//最大スライス厚の更新
					UpdateSWMax();
				}

				//再構成範囲開始位置最大値・最小値の更新
				UpdateZsminmax();

				//再構成範囲終了位置の更新
				UpdateZe();

				return;
			}
			catch
			{
				//ｴﾗｰﾒｯｾｰｼﾞ
				modLibrary.ErrorDescription(StringTable.GetResString(9904, this.Name, "K_Change"));
			}
		}


		//********************************************************************************
		//機    能  ：  スライス厚SWを変えた場合
		//              変数名           [I/O] 型        内容
		//引    数  ：  flag_bar         Boolean
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V3.0   XX/XX/XX  ??????????????  新規作成
		//              V3.1   00/10/11  (CATS)巻淵      変更
		//********************************************************************************
		private void SW_Change()
		{
			//エラー時の扱い
			try
			{
				//ZWの計算
				m_ZW = m_ZW0 - (float)cwneSW.Value * m_Ksw;

				//最大スライスピッチの更新
				UpdateDeltaZMax();

				//非ヘリカルの場合
				if (!IsHelical)
				{
					//最大スライス枚数を更新
					UpdateKMax();

					//再構成範囲開始位置最大値・最小値の更新
					UpdateZsminmax();
				}

				return;
			}
			catch
			{
				//ｴﾗｰﾒｯｾｰｼﾞ
				modLibrary.ErrorDescription(StringTable.GetResString(9904, this.Name, "SW_Change"));
			}
		}


		//********************************************************************************
		//機    能  ：  再構成開始位置Zsを変えた場合
		//              変数名           [I/O] 型        内容
		//引    数  ：  flg_Bar          Boolean
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V3.0   XX/XX/XX  ??????????????  新規作成
		//              V3.1   00/10/11  (CATS)巻淵      変更
		//********************************************************************************
		private void Zs_Change()
		{
			//エラー時の扱い
			try
			{
				//最大スライスピッチの更新
				UpdateDeltaZMax();

				//非ヘリカルの場合
				if (!IsHelical)
				{
					//最大スライス厚の更新
					UpdateSWMax();
				}

				//最大スライス枚数を更新
				UpdateKMax();

				//再構成範囲終了位置の更新
				UpdateZe();

				return;
			}
			catch
			{
				//ｴﾗｰﾒｯｾｰｼﾞ
				modLibrary.ErrorDescription(StringTable.GetResString(9904, this.Name, "Zs_Change"));
			}
		}


		//*******************************************************************************
		//機　　能： スライスピッチ変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cwneDelta_Z_ValueChanged(object sender, EventArgs e)
		{
            //Load完了前はスキップ
            if (!myLoad) return;
            
            //スライスピッチ変更に伴う各パラメータの再計算
			delta_z_Change();

			//棒グラフ表示
			indicate_Bar();
		}


		//*******************************************************************************
		//機　　能： スライス枚数変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cwneK_ValueChanged(object sender, EventArgs e)
		{
            //Load完了前はスキップ
            if (!myLoad) return;

			//スライス枚数変更に伴う各パラメータの再計算
			K_Change();

			//棒グラフ表示
			indicate_Bar();
		}


		//*******************************************************************************
		//機　　能： スライス厚変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cwneSW_ValueChanged(object sender, EventArgs e)
		{
            //Load完了前はスキップ
            if (!myLoad) return;

            //スライス厚変更に伴う各パラメータの再計算
			SW_Change();

			//棒グラフ表示
			indicate_Bar();
		}


		//*******************************************************************************
		//機　　能： 再構成範囲（開始位置）変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cwneZs_ValueChanged(object sender, EventArgs e)
		{
            //Load完了前はスキップ
            if (!myLoad) return;
            
            //再構成範囲（開始位置）変更に伴う各パラメータの再計算
			Zs_Change();

			//棒グラフ表示
			indicate_Bar();
		}


		//********************************************************************************
		//機    能  :  棒グラフを表示する
		//              変数名           [I/O] 型        内容
		//引    数  :  tHeilcalFlag     integer         0(非ヘリカル)、1(ヘリカル)
		//          :   m_SW              single          スライス厚sw(mm)
		//          :   m_Zdas            single          ヘリカル開始位置(mm)
		//          :   m_Zdae            single          ヘリカル終了位置(mm)
		//          :   m_Z0              single          昇降現在位置Z0(mm)
		//          :   m_Ksw             single
		//          :   m_ZW0             single          ビーム幅(mm)
		//          :   m_Zs              single          一枚目のスライス位置(mm)
		//          :   m_Ze              single          ｋ枚目のスライス位置(mm)
		//          :   m_Zs0             single          初期一枚目のスライス位置(mm)
		//          :   m_Ze0             single          初期ｋ枚目のスライス位置(mm)
		//          :   m_SW0             single          初期スライス厚SW（mm）
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V3.1   2000/10/10    巻淵    新規作成
		//********************************************************************************
		private void indicate_Bar()
		{
			//変数の宣言
			const int numTop = 24;				//棒グラフのTop（pixel）
			const int numHeight = 300;			//棒グラフのHeight（pixel）
			const int numWidth = 28;			//棒グラフのWidth（pixel）
			float[] tBorder1 = new float[6];	//棒グラフの境界線（mm）
			float[] tBorder2 = new float[6];	//棒グラフの境界線（mm）
			float[] numBorder1 = new float[6];	//棒グラフの境界線を求めるのに必要なパラメータ
			float[] numBorder2 = new float[6];	//棒グラフの境界線を求めるのに必要なパラメータ
			double ta1 = 0;
			double ta2 = 0;
			int i = 0;							//ループ用
			int tempnumBorder = 0;

			//ヘリカルモード
			if (IsHelical)
			{
				Label10.Visible = false;
				PicBar1[1].BackColor = Color.Lime;
				PicBar1[2].BackColor = Color.Silver;
				PicBar2[1].BackColor = Color.Lime;
				PicBar2[2].BackColor = Color.Silver;
				PicBar1[3].Visible = false;
				PicBar1[4].Visible = false;
				PicBar2[3].Visible = false;
				PicBar2[4].Visible = false;

				tBorder1[0] = m_Zdae;
				tBorder1[1] = m_Ze0;
				tBorder1[2] = m_Zs0;
				tBorder1[3] = m_Zdas;
				tBorder2[0] = m_Zdae;
				tBorder2[1] = m_Ze;
				tBorder2[2] = (float)cwneZs.Value;
				tBorder2[3] = m_Zdas;

				ta1 = Convert.ToDouble(tBorder1[0] - tBorder1[3]) / Convert.ToDouble(numHeight);
				ta2 = Convert.ToDouble(tBorder2[0] - tBorder2[3]) / Convert.ToDouble(numHeight);
				for (i = 0; i <= 3; i++)
				{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
					tempnumBorder = CInt(CDbl(tBorder1(0) - tBorder1(i)) / ta1)
					numBorder1(i) = tempnumBorder - (tempnumBorder Mod 15) + numTop
					tempnumBorder = CInt(CDbl(tBorder2(0) - tBorder2(i)) / ta2)
					numBorder2(i) = tempnumBorder - (tempnumBorder Mod 15) + numTop
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

					tempnumBorder = Convert.ToInt32(Convert.ToDouble(tBorder1[0] - tBorder1[i]) / ta1);
					numBorder1[i] = tempnumBorder + numTop;
					tempnumBorder = Convert.ToInt32(Convert.ToDouble(tBorder2[0] - tBorder2[i]) / ta2);
					numBorder2[i] = tempnumBorder + numTop;
				}

				for (i = 0; i <= 2; i++)
				{
					PicBar1[i].Top = Convert.ToInt32(numBorder1[i]);
					PicBar1[i].Height = Convert.ToInt32(numBorder1[i + 1] - numBorder1[i]) + 1;
					PicBar2[i].Top = Convert.ToInt32(numBorder2[i]);
					PicBar2[i].Height = Convert.ToInt32(numBorder2[i + 1] - numBorder2[i]) + 1;
				}

				lbl_underlimit.Text = tBorder1[0].ToString("###.###");
				lbl_upperlimit.Text = tBorder1[3].ToString("###.###");
			}
			//非ヘリカルモード
			else
			{
				float Z0border = 0;							//'Z0を示すｙ座表（pixel）

				Label10.Visible = true;
				PicBar1[1].BackColor = Color.Yellow;
				PicBar1[2].BackColor = Color.Lime;
				PicBar2[1].BackColor = Color.Yellow;
				PicBar2[2].BackColor = Color.Lime;

				PicBar1[3].Visible = true;
				PicBar1[4].Visible = true;
				PicBar2[3].Visible = true;
				PicBar2[4].Visible = true;

				tBorder1[0] = m_Z0 + m_ZW0 / Convert.ToSingle(2.0);
				tBorder1[1] = m_Ze0 + m_SW0 * m_Ksw / Convert.ToSingle(2.0);
				tBorder1[2] = m_Ze0;
				Z0border = m_Z0;										//V4.0 change by 鈴山 2001/03/16 (m_Z0 → m_Z0)
				tBorder1[3] = m_Zs0;
				tBorder1[4] = m_Zs0 - m_SW0 * m_Ksw / Convert.ToSingle(2.0);
                tBorder1[5] = m_Z0 - m_ZW0 / Convert.ToSingle(2.0);
                tBorder2[0] = tBorder1[0];
				tBorder2[1] = m_Ze + (float)cwneSW.Value * m_Ksw / Convert.ToSingle(2.0);
				tBorder2[2] = m_Ze;
				tBorder2[3] = (float)cwneZs.Value;
				tBorder2[4] = (float)cwneZs.Value - (float)cwneSW.Value * m_Ksw / Convert.ToSingle(2.0);
				tBorder2[5] = tBorder1[5];

				ta1 = Convert.ToDouble(tBorder1[0] - tBorder1[5]) / Convert.ToDouble(numHeight);
				ta2 = Convert.ToDouble(tBorder2[0] - tBorder2[5]) / Convert.ToDouble(numHeight);
				for (i = 0; i <= 5; i++)
				{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
					tempnumBorder = CInt(CDbl(tBorder1(0) - tBorder1(i)) / ta1)
					numBorder1(i) = tempnumBorder - (tempnumBorder Mod 15) + numTop
					tempnumBorder = CInt(CDbl(tBorder2(0) - tBorder2(i)) / ta2)
					numBorder2(i) = tempnumBorder - (tempnumBorder Mod 15) + numTop
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

					tempnumBorder = Convert.ToInt32(Convert.ToDouble(tBorder1[0] - tBorder1[i]) / ta1);
					numBorder1[i] = tempnumBorder + numTop;
					tempnumBorder = Convert.ToInt32(Convert.ToDouble(tBorder2[0] - tBorder2[i]) / ta2);
					numBorder2[i] = tempnumBorder + numTop;
				}

				for (i = 0; i <= 4; i++)
				{
					PicBar1[i].Top = Convert.ToInt32(numBorder1[i]);
					PicBar1[i].Height = Convert.ToInt32(numBorder1[i + 1] - numBorder1[i]) + 1;
					PicBar2[i].Top = Convert.ToInt32(numBorder2[i]);
					PicBar2[i].Height = Convert.ToInt32(numBorder2[i + 1] - numBorder2[i]) + 1;
				}

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				PicBar1(2).Line (0, CInt(PicBar1(2).Height / 2# - 15))-(numWidth, CInt(PicBar1(2).Height / 2#) - 15), vbBlack
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				PicBar1[2].Image = new Bitmap(PicBar1[2].Width, PicBar1[2].Height);
				Graphics g = Graphics.FromImage(PicBar1[2].Image);
                int y = Convert.ToInt32(PicBar1[2].Height / 2.0 - 1);
				g.DrawLine(new Pen(Color.Black), 0, y, numWidth, y);
				g.Dispose();

				lbl_underlimit.Text = tBorder1[0].ToString("###.###");
				lbl_upperlimit.Text = tBorder1[5].ToString("###.###");
			}

			//    indicate_Bar2
		}


		//Private Sub indicate_Bar2()
		//
		//    Dim UnderLimit  As Single
		//    UnderLimit = m_Z0 + m_ZW0 / 2
		//
		//    Dim r           As Double
		//    r = m_ZW0 / shpScanAll.Height
		//
		//    shpScanSW.Top = (UnderLimit - (m_Ze0 + m_SW0 * m_Ksw / 2)) / r + shpScanAll.Top
		//    shpScanSW.Height = MaxVal((m_Ze0 - m_Zs0 + m_SW0 * m_Ksw) / r, 15)
		//    shpScanRange.Top = (UnderLimit - m_Ze0) / r + shpScanAll.Top
		//    shpScanRange.Height = MaxVal((m_Ze0 - m_Zs0) / r, 15)
		//    Line1.y1 = shpScanRange.Top + shpScanRange.Height / 2
		//    Line1.y2 = Line1.y1
		//
		//    shpRetrySW.Top = (UnderLimit - (m_Ze + cwneSW.Value * m_Ksw / 2)) / r + shpRetryAll.Top
		//    shpRetrySW.Height = MaxVal((m_Ze - cwneZs.Value + cwneSW.Value * m_Ksw) / r, 15)
		//    shpRetryRange.Top = (UnderLimit - m_Ze) / r + shpRetryAll.Top
		//    shpRetryRange.Height = MaxVal((m_Ze - cwneZs.Value) / r, 15)
		//
		//    lbl_underlimit.Caption = Format$(m_Z0 + m_ZW0 / 2, "###.###")
		//    lbl_upperlimit.Caption = Format$(m_Z0 - m_ZW0 / 2, "###.###")
		//
		//End Sub


		//*******************************************************************************
		//機　　能： 各コントロールに値をセットする
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.5  06/05/31 (WEB)間々田     新規作成
		//*******************************************************************************
		private void SetControlsForCone()
		{
			float kzp = 0;		//1+Alpha/pai

			//共通パラメータ計算
			try
			{
				//固定値を設定する
                m_Z0 = CTSettings.gImageinfo.Data.z0;

				//kswの計算
                m_Ksw = (float)(1 - Math.Sin(CTSettings.gImageinfo.Data.theta0 / 2) / Math.Cos(CTSettings.gImageinfo.Data.thetaoff));

				//ヘリカル用パラメータ計算
                kzp = (float)(1 + CTSettings.gImageinfo.Data.alpha / ScanCorrect.Pai);

				//ヘリカルモード
                IsHelical = (CTSettings.gImageinfo.Data.inh != 0);

				//ヘリカルモードの表示
				lblHelicalMode.Text = CTResources.LoadResString(IsHelical ? StringTable.IDS_Helical : StringTable.IDS_NonHelical);

				//画質フレーム
				//'v16.00 再構成高速化機能有の場合，コーンビームの再構成画質選択のフレームを必ず表示させないように変更 by 長野　10/01/21
				//If (scaninh.gpgpu = 0) Then
				//    fraImageMode.Visible = False
				//End If

				//画質フレームは表示しない（VC側で常に精細。GPGPUオプションにより、高速と標準は無用と判断。）
				fraImageMode.Visible = false;			//v16.30変更 byやまおか 2010/05/25

				//画質
                modLibrary.SetOption(optImageMode, CTSettings.gImageinfo.Data.cone_image_mode, 0);

				//スライス厚最小値
                decimal SWval = 0;
                //cwneSW.Minimum = (decimal)(CTSettings.gImageinfo.Data.fcd / CTSettings.gImageinfo.Data.fid * CTSettings.gImageinfo.Data.dpm);
                SWval = (decimal)(CTSettings.gImageinfo.Data.fcd / CTSettings.gImageinfo.Data.fid * CTSettings.gImageinfo.Data.dpm);
                //Rev20.00 追加 四捨五入 by長野 2015/01/26
                SWval = Math.Round(SWval/cwneSW.Increment,MidpointRounding.AwayFromZero) * cwneSW.Increment;
                if (cwneSW.Value < SWval)
                {
                    cwneSW.Value = SWval;
                }
                cwneSW.Minimum = SWval;
                lblSWmin.Text = cwneSW.Minimum.ToString("###0.000");

				//スライス厚最大値
                SWval = 0;
				if (IsHelical)
				{
                    decimal.TryParse(CTSettings.gImageinfo.Data.width.GetString(), out SWval);
				}
				else
				{
                    SWval = (decimal)(2d / 3d * CTSettings.gImageinfo.Data.fcd / CTSettings.gImageinfo.Data.fid * CTSettings.gImageinfo.Data.dpm * (2 * CTSettings.gImageinfo.Data.mc - 2));
				}

                //Rev20.00 追加 四捨五入 by長野 2015/01/26
                SWval = (Math.Round(SWval / cwneSW.Increment, MidpointRounding.AwayFromZero) * cwneSW.Increment);
                cwneSW.Maximum = SWval;

				//スライス厚
				decimal widthValue = 0;
                decimal.TryParse(CTSettings.gImageinfo.Data.width.GetString(), out widthValue);
                //変更2015/02/02hata_Max/Min範囲のチェック
				//cwneSW.Value = widthValue;
                cwneSW.Value = modLibrary.CorrectInRange(widthValue, cwneSW.Minimum, cwneSW.Maximum);

				//スライス厚初期値
				m_SW0 = (float)cwneSW.Value;

				//スライスピッチ
                //cwneDelta_Z.Value = (decimal)CTSettings.gImageinfo.Data.delta_z;
                decimal DeltaZval = (decimal)CTSettings.gImageinfo.Data.delta_z;

                if (DeltaZval < cwneDelta_Z.Minimum)
                {
                    cwneDelta_Z.Minimum = DeltaZval;
                }
                //変更2015/02/02hata_Max/Min範囲のチェック
				//cwneDelta_Z.Value = DeltaZval;
                cwneDelta_Z.Value = modLibrary.CorrectInRange(DeltaZval, cwneDelta_Z.Minimum, cwneDelta_Z.Maximum);
               
				//スライス枚数
                cwneK.Maximum = (decimal)CTSettings.scancondpar.Data.klimit;
                //変更2015/02/02hata_Max/Min範囲のチェック
                //cwneK.Value = (decimal)CTSettings.gImageinfo.Data.k;
                cwneK.Value = modLibrary.CorrectInRange((decimal)CTSettings.gImageinfo.Data.k, cwneK.Minimum, cwneK.Maximum);

				m_Zp = CTSettings.gImageinfo.Data.zp;

				//再構成範囲開始位置
				m_Zs0 = CTSettings.gImageinfo.Data.zs0;
                //変更2015/02/02hata_Max/Min範囲のチェック
                //cwneZs.Value = (decimal)CTSettings.gImageinfo.Data.zs0;
                cwneZs.Value = modLibrary.CorrectInRange((decimal)CTSettings.gImageinfo.Data.zs0, cwneZs.Minimum, cwneZs.Maximum);

				m_Ze0 = CTSettings.gImageinfo.Data.ze0;
				m_Ze = CTSettings.gImageinfo.Data.ze0;

				//データ収集ビュー数
				lblAcqView.Text = CTSettings.gImageinfo.Data.acq_view.ToString("###0");

				//昇降範囲
				m_Zdae = m_Ze0 + m_Zp * kzp / 2;
				m_Zdas = m_Zs0 - m_Zp * kzp / 2;

				//ZW0の計算
				m_ZW0 = (2 * CTSettings.gImageinfo.Data.mc - 2) * CTSettings.gImageinfo.Data.dpm * (CTSettings.gImageinfo.Data.fcd * m_Ksw / CTSettings.gImageinfo.Data.fid);


				//各最大値の計算
				SW_Change();
				delta_z_Change();
				K_Change();
				if (IsHelical) Zs_Change();

				//棒グラフ表示
				indicate_Bar();

				return;
			}
			catch
			{
				//ｴﾗｰﾒｯｾｰｼﾞ
				modLibrary.ErrorDescription(StringTable.GetResString(9904, this.Name, "Zs_Change"));
			}
		}


		//*******************************************************************************
		//機　　能： 最大スライス厚を更新する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*******************************************************************************
		private void UpdateSWMax()
		{
			float a = 0;
			float b = 0;

			a = 2 * (m_Z0 + m_ZW0 / 2 - (float)cwneZs.Value - (float)cwneDelta_Z.Value * ((float)cwneK.Value - 1)) / m_Ksw;
			b = 2 * ((float)cwneZs.Value - m_Z0 + m_ZW0 / 2) / m_Ksw;

			float MaxValue = 0;
			MaxValue = modLibrary.MinVal(a, b);
            //2014/11/07hata キャストの修正
            //MaxValue = (int)(MaxValue * 1000) / 1000f;
            //MaxValue = Convert.ToInt32(Math.Floor(MaxValue * 1000) / 1000F);
            //Rev20.00 変更 by長野 2015/02/28
            MaxValue = (float)(Math.Floor(MaxValue * 1000) / 1000F);

			cwneSW.Maximum = (decimal)modLibrary.MaxVal(MaxValue, (float)cwneSW.Minimum);
			lblSWmax.Text = cwneSW.Maximum.ToString("###0.000");
		}


		//*******************************************************************************
		//機　　能： 最大スライスピッチを更新する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*******************************************************************************
		private void UpdateDeltaZMax()
		{
			float EndPos = 0;
			EndPos = (IsHelical ? m_Ze0 : m_Z0 + m_ZW / 2);

			float MaxValue = 0;
			MaxValue = (EndPos - (float)cwneZs.Value) / modLibrary.MaxVal((float)cwneK.Value - 1, 1);
			//MaxValue = (int)(MaxValue * 1000) / 1000;
            //Rev20.00 計算順序を変更 2014/08/27
            //2014/11/07hata キャストの修正
            //MaxValue = (float)((int)(MaxValue * 1000)) / (float)1000.0;
            MaxValue = (float)(Math.Floor(MaxValue * 1000) / 1000F);
    
			cwneDelta_Z.Maximum = (decimal)modLibrary.MaxVal(MaxValue, (float)cwneDelta_Z.Minimum);
			lbldelta_zmax.Text = cwneDelta_Z.Maximum.ToString("###0.000");
		}


		//*******************************************************************************
		//機　　能： 最大スライス枚数を更新する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*******************************************************************************
		private void UpdateKMax()
		{
			float EndPos = 0;
			EndPos = (IsHelical ? m_Ze0 : m_Z0 + m_ZW / 2);

            //2014/11/07hata キャストの修正
            //cwneK.Maximum = (decimal)modLibrary.MinVal((int)((EndPos - (float)cwneZs.Value) / (float)cwneDelta_Z.Value) + 1, CTSettings.scancondpar.Data.klimit);
            cwneK.Maximum = (decimal)modLibrary.MinVal(Math.Floor((EndPos - (float)cwneZs.Value) / (float)cwneDelta_Z.Value) + 1, CTSettings.scancondpar.Data.klimit);
            
            lblKmax.Text = cwneK.Maximum.ToString("###");
		}


		//'*******************************************************************************
		//'機　　能： 再構成範囲開始位置の最大・最小を更新する
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//'*******************************************************************************
		private void UpdateZsminmax()
		{
			float EndPos = 0;
			EndPos = (IsHelical ? m_Ze0 : m_Z0 + m_ZW / 2);

            //Rev20.00 追加 by長野 2015/01/26
            decimal tmpVal = 0;

			cwneZs.Maximum = (decimal)EndPos - cwneDelta_Z.Value * (cwneK.Value - 1);
            
            //Rev20.00 追加 by長野 2015/01/26
            //tmpVal = Math.Round(cwneZs.Maximum / cwneZs.Increment, MidpointRounding.AwayFromZero) * cwneZs.Increment;
            //Rev20.00 修正 by長野 2015/02/28
            //tmpVal = Math.Round(cwneZs.Maximum / (decimal)0.001, MidpointRounding.AwayFromZero) * cwneZs.Increment;
            //Rev20.00 修正 by長野 2015/04/21
            tmpVal = Math.Round(cwneZs.Maximum / (decimal)0.001, MidpointRounding.AwayFromZero) * (decimal)0.001;  

            cwneZs.Maximum = tmpVal;
			
            cwneZs.Minimum = (decimal)(IsHelical ? m_Zs0 : m_Z0 - m_ZW / 2);

            //Rev20.00 追加 by長野 2015/01/26
            //tmpVal = Math.Round(cwneZs.Minimum / cwneZs.Increment, MidpointRounding.AwayFromZero) * cwneZs.Increment;
            //Rev20.01 修正 by長野 2015/05/19
            tmpVal = Math.Round(cwneZs.Minimum / (decimal)0.001, MidpointRounding.AwayFromZero) * (decimal)0.001;  

            cwneZs.Minimum = tmpVal;

			lblZsmin.Text = cwneZs.Minimum.ToString("###0.000");
			lblZsmax.Text = cwneZs.Maximum.ToString("###0.000");
		}


		//*******************************************************************************
		//機　　能： 再構成範囲終了位置を更新する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*******************************************************************************
		private void UpdateZe()
		{
			//Zeの計算
			m_Ze = (float)(cwneZs.Value + cwneDelta_Z.Value * (cwneK.Value - 1));

			//表示の更新
			lblZe.Text = m_Ze.ToString("###0.000");
		}


		//v19.00 追加 ->(電S2)永井

		//*******************************************************************************
		//機　　能： 「ＢＨＣ処理を行う」チェックボックスクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void chkBHC_CheckStateChanged(object sender, EventArgs e)
		{
			bool theStatus = false;
			//string FileName = null;

			theStatus = (chkBHC.CheckState == CheckState.Checked);

			//条件によりオブジェクトを薄文字で表示
			lblBhcDirTitle.Enabled = theStatus;
			lblBhcFileTitle.Enabled = theStatus;
			txtBhcDirName.Enabled = theStatus;
			txtBhcFileName.Enabled = theStatus;

			//v19.00 OKボタンを有効にする
			cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： ビームハードニング補正(BHC)フレーム内「変更..」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.0   2006/12/22 Ohkado    新規作成
		//*******************************************************************************
		private void cmdChangeBHCTable_Click(object sender, EventArgs e)
		{
			//v8.2削除ここから by 間々田 2007/06/11
			//With CommonDialog1
			//
			//    .DialogTitle = ""
			//    .FileName = ""
			//    .Flags = cdlOFNCreatePrompt Or cdlOFNHideReadOnly
			//    .Filter = MakeCommonDialogFilter(LoadResString(IDS_BHCTable), "-bhc.csv")
			//    .InitDir = IIf(txtBhcDirName.text = "", LoadResString(IDS_BHCTableDir), txtBhcDirName.text)
			//    'エラー時の扱い
			//    On Error Resume Next
			//
			//    'ダイアログ表示
			//    .ShowOpen
			//
			//    'ＯＫボタン選択以外の時
			//    If Err.Number <> 0 Then
			//        'キャンセルボタンを選択時以外はエラーメッセージを表示
			//        If Err.Number <> cdlCancel Then MsgBox Err.Description, vbCritical
			//        Exit Sub
			//    End If
			//
			//    '以下、ＯＫボタン選択時の処理
			//    txtBhcDirName.text = RemoveExtension(.FileName, .FileTitle)
			//    txtBhcFileName.text = .FileTitle                                 '変更 by 村田 2007/01/22
			//    txtBhcFileNameChange
			//    'BHC可否のチェックをONにする
			//    chkBHC.Value = 1
			//End With
			//v8.2削除ここまで by 間々田 2007/06/11

			//v8.2追加ここから by 間々田 2007/06/11 上記の方法だと存在しないＢＨＣテーブルを指定できてしまうので
			string FileName = null;
			string PathName = null;
			string TableName = null;

			//ダイアログによるファイル選択
            //FileName = modFileIO.GetFileName(Description: CTResources.LoadResString(StringTable.IDS_BHCTable), 
            //                                 SubExtension: "-bhc", 
            //                                 InitFileName: txtBhcDirName.Text + txtBhcFileName.Text);
            //Rev20.00 修正 by長野 2015/03/23
            FileName = modFileIO.GetFileName(Description: CTResources.LoadResString(StringTable.IDS_BHCTable),
                                             SubExtension: "-bhc",
                                             InitFileName: txtBhcDirName.Text + "\\" + txtBhcFileName.Text);

            if (string.IsNullOrEmpty(FileName)) return;

			//パス名とテーブル名に分ける
			modLibrary.SeparateFileName(FileName, ref PathName, ref TableName);

			//テキストボックスに表示
			txtBhcDirName.Text = PathName;
			txtBhcFileName.Text = TableName;
			txtBhcFileNameChange();

			//ＢＨＣ可否のチェックをONにする
			chkBHC.CheckState = CheckState.Checked;
			//v8.2追加ここまで by 間々田 2007/06/11

			//v19.00 OKボタンを有効にする
			cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： ビームハードニング補正(BHC)フレーム内「デフォルト」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.0   2006/12/22 Ohkado    新規作成
		//*******************************************************************************
		private void cmdChangeBHCTableDefault_Click(object sender, EventArgs e)
		{
			//v8.2追加ここから by 間々田 2007/06/11 上記の方法だと存在しないＢＨＣテーブルを指定できてしまうので
			string FileName = null;
			string PathName = null;
			string TableName = null;

			//デフォルトのファイル名は、"C:\CTUSER\BHCﾃｰﾌﾞﾙ\BHC-DEFAULT_TABLE.csv"とする
			//FileName = "C:\CTUSER\BHCﾃｰﾌﾞﾙ\DEFAULT_TABLE-BHC.csv"
			//v19.12 英語化対応 by長野 2013/02/20
			//FileName = AppValue.InitDir_BHCTable + @"\DEFAULT_TABLE-BHC.csv";
            //Rev20.00 変更 by長野 2015/02/11
            FileName = Path.Combine(AppValue.CTUSER, CTResources.LoadResString(21001)) + "\\DEFAULT_TABLE-BHC.csv";
			
            if (string.IsNullOrEmpty(FileName)) return;

			//パス名とテーブル名に分ける
			modLibrary.SeparateFileName(FileName, ref PathName, ref TableName);

			//テキストボックスに表示
            txtBhcDirName.Text = PathName;
            txtBhcFileName.Text = TableName;
			txtBhcFileNameChange();

			//ＢＨＣ可否のチェックをONにする
			chkBHC.CheckState = CheckState.Checked;
			//v8.2追加ここまで by 間々田 2007/06/11

			//v19.00 OKボタンを有効にする
			cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： ビームハドニング補正フレーム内テーブル名変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴：V1.00  99/XX/XX   ????????      新規作成
		//          V8.00  2007/01/24   Ohkado      Else以下を追加
		//*******************************************************************************
		private void txtBhcFileNameChange()
		{
			if (string.IsNullOrEmpty(txtBhcDirName.Text) || string.IsNullOrEmpty(txtBhcFileName.Text))
			{
				chkBHC.CheckState = CheckState.Unchecked;
				chkBHC.Enabled = false;
			}
			else									//v8.0追加 by Ohakdo 2007/01/24
			{
				chkBHC.Enabled = true;
			}
		}


		//********************************************************************************
		//機    能  ：  スキャン条件の設定内容をチェック（その３）
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V19.00   12/02/21    H.Nagai     新規作成
		//********************************************************************************
		private bool OptValueChk3()
		{
			bool functionReturnValue = false;

			//BHC
			if ((chkBHC.CheckState == CheckState.Checked) && fraBHC.Visible)
			{
				//ファイル名に".csv"がつくためピリオド判定なしバージョンを呼び出す
				if (!modLibrary.IsValidFileName2(txtBhcDirName.Text, txtBhcFileName.Text))
				{
					modCT30K.ErrMessage(1217, Icon: MessageBoxIcon.Error);				//リソース　BHCﾃｰﾌﾞﾙが指定されたﾃﾞｨﾚｸﾄﾘにありません。
					return functionReturnValue;
				}
			}

            //Rev26.00 ファントムレスBHC追加 by長野 2017/04/17
            if (CTSettings.scaninh.Data.mbhc_phantomless == 0 && cmbBHCPhantomless.SelectedIndex > 0)
            {
                //Rev26.00 change by chouno 2017/04/17
                //if (modScanCondition.ChkXraySpectrumDataExists(Convert.ToInt32(CTSettings.t20kinf.Data.system_type.GetString())) != 0)
                if (modScanCondition.ChkXraySpectrumDataExists(Convert.ToInt32(CTSettings.t20kinf.Data.system_type.GetString())) != 0)
                {
                    modCT30K.ErrMessage(1219, Icon: MessageBoxIcon.Error);			//リソース　該当するX線源のデータがありません。
                    return functionReturnValue;
                }

                //材質の種類を判定
                string dirPath = @"C:\CT\ScanCorrect\phantomlessBHC";
                string[] fileList = Directory.GetFileSystemEntries(dirPath, @cmbBHCPhantomless.SelectedIndex + "_BHC_*.csv");
                string fullPath;
                fullPath = fileList[0];

                if (!modLibrary.IsValidFileName2(dirPath, System.IO.Path.GetFileName(fullPath)))
                {
                    modCT30K.ErrMessage(1218, Icon: MessageBoxIcon.Error);			//リソース　材質データがが指定されたﾃﾞｨﾚｸﾄﾘにりません。
                    return functionReturnValue;
                }
            }

			functionReturnValue = true;
			return functionReturnValue;
		}
        //<- v19.00

        private void sstRetry_SelectedIndexChanged(object sender, EventArgs e)
        {
            //表示していないタブ内のコントロールにフォーカスさせないようにするための措置
            int i = 0;
            for (i = fraRetry.GetLowerBound(0); i <= fraRetry.GetUpperBound(0); i++)
            {
                //追加2014/09/11_dNet対応_hata
                //選択されていないTabのVisibleがFalse になるための処置
                if (sstRetry.SelectedIndex == i)
                {
                    fraRetry[i].Parent = sstRetry.TabPages[i];
                    fraRetry[i].Location = new Point(1, 1);
                }
                else
                {
                    fraRetry[i].Parent = this;
                    fraRetry[i].Top = sstRetry.Top + sstRetry.ItemSize.Height;
                    fraRetry[i].Left = sstRetry.Left;
                }

            }
        }

        //追加2015/01/20hata
        //データを再表示する
        private void NumicValue_Leave(object sender, EventArgs e)
        {
            if (sender as NumericUpDown == null) return;

            NumericUpDown udbtn = (NumericUpDown)sender;

            string sval = udbtn.Value.ToString();
            if (string.IsNullOrEmpty(sval))
            {
                udbtn.Value = udbtn.Minimum;
                return;
            }
            udbtn.Text = sval;
        }


	}
}
