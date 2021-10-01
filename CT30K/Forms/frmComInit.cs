using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{
	public partial class frmComInit : Form
	{
		//mecainf のバックアップ用 v11.2追加 by 間々田 2006/01/12
		//private modMecainf.mecainfType mecainfBak;
        private MecaInf mecainfBak;

		//scancondpar のバックアップ用 v15.01追加 by 間々田 2009/08/28
		//private modScancondpar.scancondparType scancondparBak;
        private ScanCondPar scancondparBak;

		private static frmComInit _Instance = null;

		public frmComInit()
		{
			InitializeComponent();
		}

		public static frmComInit Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmComInit();
				}

				return _Instance;
			}
		}


		//*******************************************************************************
		//機　　能： 「いいえ」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*******************************************************************************
		private void cmdNo_Click(object sender, EventArgs e)
		{
			this.Close();
		}


		//*******************************************************************************
		//機　　能： 「はい」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*******************************************************************************
		private void cmdYes_Click(object sender, EventArgs e)
		{
			//マウスカーソルを矢印＆砂時計にする
			Cursor.Current = Cursors.AppStarting;

			lblComInit.Visible = true;
			chkComInit.Enabled = false;
			cmdYes.Enabled = false;
			cmdNo.Enabled = false;

			//タイマーの停止
			//    frmStatus.tmrStatus.Enabled = False
			frmMechaControl.Instance.tmrPIOCheck.Enabled = false;
            
            //v19.50 タイマー統合 by長野 2013/12/17     //変更2014/10/07hata_v19.51反映
            //frmMechaControl.Instance.tmrSeqComm.Enabled = false;			//追加 by 間々田 2009/07/31
			//frmMechaControl.Instance.tmrMecainf.Enabled = false;			//追加 by 間々田 2009/07/31
            modMechaControl.Flg_MechaControlUpdate = false;
            modMechaControl.Flg_SeqCommUpdate = false;
            frmMechaControl.Instance.tmrMecainfSeqComm.Enabled = false;


			//「校正結果も初期化する」チェックボックスがチェックされていない場合、校正ステータスを待避
			if (chkComInit.CheckState == CheckState.Unchecked)
			{
				//mecainf のバックアップ v11.2追加 by 間々田 2006/01/12
				//mecainfBak = default(modMecainf.mecainfType);
				//modMecainf.GetMecainf(ref mecainfBak);
                mecainfBak = new MecaInf();
                mecainfBak.Data.Initialize();
                mecainfBak.Load();

				//Scancondpar のバックアップ v15.01追加 by 間々田 2009/08/28
				//scancondparBak = modScancondpar.scancondparType.Initialize();
				//modScancondpar.GetScancondpar(ref scancondparBak);
                scancondparBak = new ScanCondPar();
                scancondparBak.Data.Initialize();
                scancondparBak.Load();
            }

			//FCD_OFFSET値も初期化する   'v11.4変更 by 間々田 2006/03/07 上記コメント化した処理と同じ処理
			modCT30K.UpdateCsv(AppValue.OFFSET_CSV, "fcd_offset", "0.0000", 2);
            modCT30K.UpdateCsv(AppValue.OFFSET_CSV, "fcd_offset", (CTSettings.scancondpar.Data.fid_offset[1] - CTSettings.scancondpar.Data.fid_offset[0]).ToString("0.0000"), 3);
            modCT30K.UpdateCsv(AppValue.OFFSET_CSV, "fcd_offset", "0.0000", 4);

            //変更2014/10/07hata_v19.51反映
            if (CTSettings.SecondDetOn)
            {
                //FCD_OFFSET_2値も初期化する 'v17.20 追加 by 長野
                modCT30K.UpdateCsv(AppValue.OFFSET_2_CSV, "fcd_offset", "0.0000", 2);
                modCT30K.UpdateCsv(AppValue.OFFSET_2_CSV, "fcd_offset", (CTSettings.scancondpar.Data.fid_offset[1] - CTSettings.scancondpar.Data.fid_offset[0]).ToString("0.0000"), 3);
                modCT30K.UpdateCsv(AppValue.OFFSET_2_CSV, "fcd_offset", "0.0000", 4);
            }

            //Rev20.00 コモン初期化処理の前はファイルに落としておく。 by長野 2015/01/24
            ComLib.SaveSharedCTCommon();

			//コモン初期化→comset.exeを実行
			int ProcessID = 0;
			Process p = null;
			try
			{
				p = new Process();
				p.StartInfo.FileName = AppValue.COMSET;
				p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;

				p.Start();
				ProcessID = p.Id;
			}
			finally
			{
				if (p != null)
				{
					p.Dispose();
					p = null;
				}
			}

			//待つ
			//PauseForDoEvents 2
			modCT30K.PauseForDoEvents(4);

			//コモン初期化後の処理
			EndProcess();

			//フォームのアンロード
			this.Close();
		}


		//*******************************************************************************
		//機　　能： comset.exeを実行後のコールバック関数
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V4.0   01/02/13  (SI1)鈴山       新規作成
		//*******************************************************************************
		private void EndProcess()
		{
			int error_sts = 0;			//戻り値

			frmScanControl frmScanControl = frmScanControl.Instance;
			frmCorrectionStatus frmCorrectionStatus = frmCorrectionStatus.Instance;
			frmMechaControl frmMechaControl = frmMechaControl.Instance;

			//コモン構造体変数の取得
			modCT30K.GetCommon();

			//mecainf（コモン）の取得
			//modMecainf.GetMecainf(ref CTSettings.mecainf.Data);
            CTSettings.mecainf.Load();

			//scansel（コモン）の取得
			modCommon.GetMyScansel();
			//    frmStatus.UpdateDataMode                                        'v11.4追加 by 間々田 2006/03/13

			//v7.0 FPD対応 by 間々田 2003/10/20
			modCT30K.GetBinning();

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//     frmCTMenu.UpdateBinningMode                                     'v11.4追加 by 間々田 2006/03/13
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//マトリクスサイズ表示の更新
			frmCTMenu.Instance.UpdateMatrixSize();

			//スキャン条件画面の更新
			frmScanControl.LoadScanCondition();

            //Rev26.14 修正 by chouno 2018/09/11
            if (CTSettings.scaninh.Data.guide_mode == 1)
            {
                //コモン初期化後は画質を Quick にする                        'v15.0追加 by 間々田 2009/06/17
                frmScanControl.optQuality[(int)frmScanControl.ScanQualityConstants.ScanQualityManual].Checked = true;	//いったんマニュアルにする   '追加 by 間々田 2009/08/27
                frmScanControl.optQuality[(int)frmScanControl.ScanQualityConstants.ScanQualityQuick].Checked = true;
            }

			//プリセット欄もクリア                                       'v15.0追加 by 間々田 2009/06/17
			//frmScanControl.chkUserCondition.CheckState = CheckState.Unchecked;
			//frmScanControl.chkImageCondition.CheckState = CheckState.Unchecked;
			frmScanControl.txtPresetImgFile.Text = "";

			//スキャン条件の内部設定（コーンビームモードで設定）         'v15.0追加 by 間々田 2009/06/17
            frmScanCondition.Instance.Setup(IsConeBeam: true);
            //frmScanCondition.Instance.Setup(IsConeBeam: 1);

			//「校正結果も初期化する」チェックボックスがチェックされていない場合、校正ステータスを復活
			if (chkComInit.CheckState == CheckState.Unchecked)
			{
				//'管電圧/管電流を復活       'v15.0削除 by 間々田 2009/04/23
				//With scansel
				//    .scan_kv = frmXrayControl.cwneKV.value 'v11.4変更 by 間々田 2006/03/02
				//    .scan_ma = frmXrayControl.cwneMA.value 'v11.4変更 by 間々田 2006/03/02
				//End With
				//PutScansel scansel

				//校正パラメータを復活
                CTSettings.mecainf.Data.normal_rc_cor = mecainfBak.Data.normal_rc_cor;		//回転中心校正ステータス（ノーマル）
                CTSettings.mecainf.Data.cone_rc_cor = mecainfBak.Data.cone_rc_cor;			//回転中心校正ステータス（コーン）
                CTSettings.mecainf.Data.gain_iifield = mecainfBak.Data.gain_iifield;			//ゲイン校正I.I.視野
                CTSettings.mecainf.Data.gain_kv = mecainfBak.Data.gain_kv;					//ゲイン校正管電圧
				//.gain_ma = mecainfBak.gain_ma               'ゲイン校正管電流      'v15.0削除 by 間々田 2009/03/18 未使用のため
                CTSettings.mecainf.Data.gain_ma = mecainfBak.Data.gain_ma;					//ゲイン校正管電流       'v19.0復活 by 長野 2012/05/10
                CTSettings.mecainf.Data.gain_date = mecainfBak.Data.gain_date;				//ゲイン校正年月日       'v15.01追加 by 間々田 2009/08/28
                CTSettings.mecainf.Data.gain_mt = mecainfBak.Data.gain_mt;					//ゲイン校正Ｘ線管
                CTSettings.mecainf.Data.gain_filter = mecainfBak.Data.gain_filter;			//ゲイン校正フィルタ
                CTSettings.mecainf.Data.gain_bin = mecainfBak.Data.gain_bin;					//ゲイン校正実行時のビニングモード
                CTSettings.mecainf.Data.sp_iifield = mecainfBak.Data.sp_iifield;				//スキャン位置校正I.I.視野
                CTSettings.mecainf.Data.sp_mt = mecainfBak.Data.sp_mt;						//スキャン位置校正Ｘ線管
                CTSettings.mecainf.Data.sp_bin = mecainfBak.Data.sp_bin;						//スキャン位置校正実行時のビニングモード
                CTSettings.mecainf.Data.ver_iifield = mecainfBak.Data.sp_iifield;				//幾何歪校正I.I.視野
                CTSettings.mecainf.Data.ver_mt = mecainfBak.Data.sp_mt;						//幾何歪校正Ｘ線管
                CTSettings.mecainf.Data.ver_bin = mecainfBak.Data.sp_bin;						//幾何歪校正実行時のビニングモード
                CTSettings.mecainf.Data.off_date = mecainfBak.Data.off_date;					//オフセット校正年月日
                CTSettings.mecainf.Data.off_bin = mecainfBak.Data.off_bin;					//オフセット校正実行時のビニングモード
                CTSettings.mecainf.Data.dc_iifield = mecainfBak.Data.dc_iifield;				//寸法校正I.I.視野
                CTSettings.mecainf.Data.dc_mt = mecainfBak.Data.dc_mt;						//寸法校正Ｘ線管
                CTSettings.mecainf.Data.dc_bin = mecainfBak.Data.dc_bin;						//寸法校正実行時のビニングモード
                CTSettings.mecainf.Data.dc_rs = mecainfBak.Data.dc_rs;						//寸法校正回転選択ステータス

				//modMecainf.PutMecainf(ref CTSettings.mecainf.Data);
                CTSettings.mecainf.Write();

				//幾何歪校正関連のパラメータも元に戻す           'v15.01追加 by 間々田 2009/08/28
				int i = 0;
				//int j = 0;

				//検出器ピッチ (mm/画素)
				for (i = CTSettings.scancondpar.Data.mdtpitch.GetLowerBound(0); i <= CTSettings.scancondpar.Data.mdtpitch.GetUpperBound(0); i++)
				{
                    CTSettings.scancondpar.Data.mdtpitch[i] = scancondparBak.Data.mdtpitch[i];
				}

				//幾何歪校正係数(A0～A5)
				for (i = CTSettings.scancondpar.Data.a.GetLowerBound(0); i <= CTSettings.scancondpar.Data.a.GetUpperBound(0); i++)
				{
                    //for (j = CTSettings.scancondpar.Data.a.GetLowerBound(0); j <= CTSettings.scancondpar.Data.a.GetUpperBound(0); j++)
                    //{
                    //    //CTSettings.scancondpar.Data.a[i, j] = scancondparBak.a[i, j];
                    //    CTSettings.scancondpar.Data.a[i * 6 + j] = scancondparBak.Data.a[i * 6 + j];
                    //}
                    CTSettings.scancondpar.Data.a[i] = scancondparBak.Data.a[i];
                }

				//有効データ開始画素
				for (i = CTSettings.scancondpar.Data.xls.GetLowerBound(0); i <= CTSettings.scancondpar.Data.xls.GetUpperBound(0); i++)
				{
                    CTSettings.scancondpar.Data.xls[i] = scancondparBak.Data.xls[i];
				}

				//有効データ終了画素
				for (i = CTSettings.scancondpar.Data.xle.GetLowerBound(0); i <= CTSettings.scancondpar.Data.xle.GetUpperBound(0); i++)
				{
                    CTSettings.scancondpar.Data.xle[i] = scancondparBak.Data.xle[i];
				}

				//最大ファン角
				for (i = CTSettings.scancondpar.Data.max_mfanangle.GetLowerBound(0); i <= CTSettings.scancondpar.Data.max_mfanangle.GetUpperBound(0); i++)
				{
                    CTSettings.scancondpar.Data.max_mfanangle[i] = scancondparBak.Data.max_mfanangle[i];
				}

				//傾き
				for (i = CTSettings.scancondpar.Data.scan_posi_a.GetLowerBound(0); i <= CTSettings.scancondpar.Data.scan_posi_a.GetUpperBound(0); i++)
				{
                    CTSettings.scancondpar.Data.scan_posi_a[i] = scancondparBak.Data.scan_posi_a[i];
				}

				//切片
				for (i = CTSettings.scancondpar.Data.scan_posi_b.GetLowerBound(0); i <= CTSettings.scancondpar.Data.scan_posi_b.GetUpperBound(0); i++)
				{
                    CTSettings.scancondpar.Data.scan_posi_b[i] = scancondparBak.Data.scan_posi_b[i];
				}

				//コーンビーム用幾何歪校正係数(B0～B5)
				for (i = CTSettings.scancondpar.Data.b.GetLowerBound(0); i <= CTSettings.scancondpar.Data.b.GetUpperBound(0); i++)
				{
					CTSettings.scancondpar.Data.b[i] = scancondparBak.Data.b[i];
				}

				//m方向ﾃﾞｰﾀﾋﾟｯﾁ(mm)
                CTSettings.scancondpar.Data.dpm = scancondparBak.Data.dpm;

				//最大ファン角（コーンビーム用）
                CTSettings.scancondpar.Data.cone_max_mfanangle = scancondparBak.Data.cone_max_mfanangle;

				//Scancondpar（コモン）の書き込み
				//modScancondpar.CallPutScancondpar();
                CTSettings.scancondpar.Write();

			}
			else
			{
				//v11.2追加ここから by 間々田 2006/01/10 校正ステータスの ***移動をすべて移動ありにするための処理
				modSeqComm.SeqBitWrite("GainIIChangeSet", true);
				modSeqComm.SeqBitWrite("VerIIChangeSet", true);
				modSeqComm.SeqBitWrite("RotXChangeSet", true);
				modSeqComm.SeqBitWrite("RotYChangeSet", true);
				modSeqComm.SeqBitWrite("RotIIChangeSet", true);
				modSeqComm.SeqBitWrite("DisIIChangeSet", true);
				modSeqComm.SeqBitWrite("DisXChangeSet", true);
				modSeqComm.SeqBitWrite("DisYChangeSet", true);
				modSeqComm.SeqBitWrite("SPIIChangeSet", true);
				//v11.2追加ここまで by 間々田 2006/01/10 校正ステータスの ***移動をすべて移動ありにするための処理
			}

			//Ｘ線検出器がフラットパネルの場合 v7.0 added by 間々田 2003/10/20  'v10.0上から移動 by 間々田 2005/02/09
            if (CTSettings.detectorParam.Use_FlatPanel)
			{
				error_sts = ScanCorrect.Get_Vertical_Parameter_Ex(0);
				ScanCorrect.Set_Vertical_Parameter();
			}

			//スキャン校正画面の更新
			frmCorrectionStatus.MyUpdate();
			frmCorrectionStatus.UpdateRCStatus();

			//表示画像をクリア           'v10.2追加 by 間々田 2005/08/23
			frmScanImage.Instance.Target = "";

			//タイマーの再開
			//    frmStatus.tmrStatus.Enabled = True
			frmMechaControl.Instance.tmrPIOCheck.Enabled = true;
            //v19.50 タイマーの統合 by長野 2013/12/17        //変更2014/10/07hata_v19.51反映
            modMechaControl.Flg_SeqCommUpdate = true;
            modMechaControl.Flg_MechaControlUpdate = true;
            frmMechaControl.tmrMecainfSeqComm.Enabled = true;
            //frmMechaControl.tmrSeqComm.Enabled = true;			//追加 by 間々田 2009/07/31
            //frmMechaControl.tmrMecainf.Enabled = true;			//追加 by 間々田 2009/07/31

			//メカ動作速度の再設定
			frmMechaControl.cboSpeed_SelectedIndexChanged(frmMechaControl.cboSpeed[0], EventArgs.Empty);
			frmMechaControl.cboSpeed_SelectedIndexChanged(frmMechaControl.cboSpeed[1], EventArgs.Empty);
			frmMechaControl.cboSpeed_SelectedIndexChanged(frmMechaControl.cboSpeed[2], EventArgs.Empty);
			frmMechaControl.cboSpeed_SelectedIndexChanged(frmMechaControl.cboSpeed[3], EventArgs.Empty);
			frmMechaControl.cboSpeed_SelectedIndexChanged(frmMechaControl.cboSpeed[6], EventArgs.Empty);
			frmMechaControl.cboSpeed_SelectedIndexChanged(frmMechaControl.cboSpeed[4], EventArgs.Empty);

			//マウスカーソルを元に戻す   V4.0 append by 鈴山 2001/03/02
			Cursor.Current = Cursors.Default;

			//CT30Kの再起動を促す
			//ストリングテーブル化　'v17.60 by長野
			//MsgBox "コモン初期化が完了しました。" + vbCrLf + "CTソフトを再起動してください。", vbOKOnly
			MessageBox.Show(CTResources.LoadResString(20007) + "\r\n" + CTResources.LoadResString(20008), Application.ProductName, MessageBoxButtons.OK);

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
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*******************************************************************************
		private void frmComInit_Load(object sender, EventArgs e)
		{
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

            //追加2014/10/07hata_v19.51反映
            //各コントロールの初期化 'v18.00追加 byやまおか 2011/07/24 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            InitControls();

		}

        //追加2014/10/07hata_v19.51反映
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから
        //*************************************************************************************************
        //機　　能： フォームロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V18.00  2011/07/24  やまおか    新規作成
        //*************************************************************************************************
		private void InitControls()
		{

			//起動時は、校正結果も初期化するにチェックを入れておく
			chkComInit.CheckState = System.Windows.Forms.CheckState.Checked;

		}
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここまで
	}
}
