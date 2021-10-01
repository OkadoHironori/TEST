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
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： frmRotationCenter.frm                                       */
    ///* 処理概要　　： 回転中心校正                                                */
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
    ///* V4.0        01/01/30    (ITC)    鈴山　修   ﾓｰﾀﾞﾙﾌｫｰﾑ→MDI子ﾌｫｰﾑに変更     */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
    public partial class frmRotationCenter : Form
    {
        #region インスタンスを返すプロパティ

        // frmRotationCenterのインスタンス
        private static frmRotationCenter _Instance = null;

        /// <summary>
        /// frmRotationCenterのインスタンスを返す
        /// </summary>
        public static frmRotationCenter Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmRotationCenter();
                }

                return _Instance;
            }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmRotationCenter()
        {
            InitializeComponent();
        }

        #endregion

        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************
        private bool OK;                //入力結果
        private float Val_ScnSlWidMax;  //回転中心校正用最大スライス厚
        private float Val_ScnSlWidMin;  //回転中心校正用最小スライス厚
        private int MyMultiSlice;       //同時スキャン枚数(0:1ｽﾗｲｽ,1:3ｽﾗｲｽ,2:5ｽﾗｲｽ)   'v10.0追加 by 間々田 2005/02/14

        //メカコントロールフォーム参照用
        private frmMechaControl _myMechaControl;
        public frmMechaControl myMechaControl
        {
            get { return _myMechaControl; }
            set
            {
                if (_myMechaControl != null)
                {
                    _myMechaControl.FCDChanged -= myMechaControl_FCDChanged;
                    _myMechaControl.FIDChanged -= myMechaControl_FIDChanged;
                }

                _myMechaControl = value;
                if (_myMechaControl != null)
                {
                    _myMechaControl.FCDChanged += new EventHandler(myMechaControl_FCDChanged);
                    _myMechaControl.FIDChanged += new EventHandler(myMechaControl_FIDChanged);
                }
			}
		}

        private bool myBusy;

        //*******************************************************************************
        //機　　能： IsBusyプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v10.2 2005/08/22 (SI3)間々田    新規作成
        //*******************************************************************************
		public bool IsBusy
        {
			get { return myBusy; }
			set
            {
				//設定値を保存
				myBusy = value;

				//「ＯＫ」ボタンと「停止」ボタンの切り替
				cmdOK.Text = (myBusy ? CTResources.LoadResString(StringTable.IDS_btnStop) : CTResources.LoadResString(StringTable.IDS_btnOK));

				//各コントロールのEnabledプロパティを制御
				cmdEnd.Enabled = !myBusy;
				cwneScanView.Enabled = !myBusy;
				cwneSum.Enabled = !myBusy;
				cwneSlicePix.Enabled = (!myBusy) & (!ScanCorrect.MultiSliceMode);
				cboMultiTube.Enabled = !myBusy;
				chkShading.Enabled = !myBusy;

				//マウスポインタを元に戻す
				Cursor.Current = (myBusy ? Cursors.AppStarting : Cursors.Default);

				//CTBusyフラグを更新
				if (myBusy)
                {
					modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTMechaBusy;
				}
                else
                {
					modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTMechaBusy);
				}
			}
		}

        //*******************************************************************************
        //機　　能： FCDWithOffsetプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private float FCDWithOffset
        {
            //FCD（オフセットを含む）
			get
            {
                return ScanCorrect.GVal_Fcd + CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()];
            }
		}

        //*******************************************************************************
        //機　　能： FIDWithOffsetプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private float FIDWithOffset
        {   
            //FID（オフセットを含む）
			get
            {
                return ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube];
            }
		}

        //*******************************************************************************
        //機　　能： メカ画面からのＦＣＤ値変更時イベント処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void myMechaControl_FCDChanged(object sender, EventArgs e)
        {
			//スライス厚の最大値・最小値の再計算
			//UpdateSliceWidthRange();
		}

        //*******************************************************************************
        //機　　能： メカ画面からのＦＩＤ値変更時イベント処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void myMechaControl_FIDChanged(object sender, EventArgs e)
		{
			//スライス厚の最大値・最小値の再計算
			//UpdateSliceWidthRange();
		}

        //********************************************************************************
        //機    能  ：  回転中心校正の設定値を表示する
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  初期値は下記のように指定する。
        //
        //                ﾋﾞｭｰ数         = Public変数
        //                積算枚数       = Public変数
        //                スライス厚         = Public変数
        //                FID            = コモン
        //                FCD            = コモン
        //                ﾋﾞｭｰ数の範囲   = コモン
        //                積算枚数の範囲 = コモン
        //                スライス厚の範囲   = コモン
        //
        //履    歴  ：  V2.0   00/02/08  (SI1)鈴山       新規作成
        //********************************************************************************
		private void Disp_RotationCenter_Condition()
		{
			//ビュー数
			//最大値・最小値の設定
            //cwneScanView.SetMinMax(modSeqComm.GetViewMin(), modGlobal.GVal_ViewMax);
            cwneScanView.Minimum = modSeqComm.GetViewMin();
            cwneScanView.Maximum = CTSettings.GVal_ViewMax;


			//最大値・最小値の表示
            lblViewMinMax.Text = StringTable.GetResString(StringTable.IDS_Range, cwneScanView.Minimum.ToString(), cwneScanView.Maximum.ToString());

			//値の設定
            //Rev20.00 範囲チェック追加 by長野 2015/01/29
            //cwneScanView.Value = ScanCorrect.ViewNumAtRot;
            if (cwneScanView.Maximum <= ScanCorrect.ViewNumAtRot)
            {
                cwneScanView.Value = cwneScanView.Maximum;
            }
            else if (cwneScanView.Minimum >= ScanCorrect.ViewNumAtRot)
            {
                cwneScanView.Value = cwneScanView.Minimum;
            }
            else
            {
                cwneScanView.Value = ScanCorrect.ViewNumAtRot;
            }

            //積算枚数
			//最小値・最大値の設定
			//   最小:infdef.min_integ_number
			//   最大:infdef.max_integ_number
            //cwneSum.SetMinMax(modGlobal.GValIntegNumMin, modGlobal.GValIntegNumMax);
            cwneSum.Minimum = CTSettings.GValIntegNumMin;
            cwneSum.Maximum = CTSettings.GValIntegNumMax;

			//最大値・最小値の表示
            lblIntegMinMax.Text = StringTable.GetResString(StringTable.IDS_Range, cwneSum.Minimum.ToString(), cwneSum.Maximum.ToString());

			//値の設定
            //変更2014/11/28hata_v19.51_dnet
            //数字の初期値は10単位にする
            //cwneSum.Value = ScanCorrect.IntegNumAtRot;
            cwneSum.Value = ScanCorrect.RoundControlVale(ScanCorrect.IntegNumAtRot, cwneSum.Maximum, cwneSum.Minimum, 10F);

			//シェーディング補正                         'V4.0 append by 鈴山 2001/03/29
			chkShading.CheckState = (ScanCorrect.GFlg_Shading_Rot ? CheckState.Checked : CheckState.Unchecked);

			//スライス厚の初期値を画素で設定
            //変更2015/02/02hata_Max/Min範囲のチェック
			//cwneSlicePix.Value = Convert.ToInt32(ScanCorrect.GVal_SlPix_Rot);
            cwneSlicePix.Value = modLibrary.CorrectInRange(Convert.ToInt32(ScanCorrect.GVal_SlPix_Rot), cwneSlicePix.Minimum, cwneSlicePix.Maximum);
 			
            //スライス厚の最大値・最小値の再計算
			UpdateSliceWidthRange();

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//X線管
			//cboMultiTube.ListIndex = scansel.multi_tube
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            //Rev23.10 復活 by長野　2015/10/05
            cboMultiTube.SelectedIndex = CTSettings.scansel.Data.multi_tube;
 
		}

        //*************************************************************************************************
        //機    能  ：  最大・最小スライス厚の再計算と表示を行う
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：
        //
        //履    歴  ：  V3.0   00/08/19  (SI1)鈴山       新規作成
        //*************************************************************************************************
		private void UpdateSliceWidthRange()
		{
			//スライス厚最小(mm)
			float SWMin = 0;
            SWMin = (float)modLibrary.MaxVal((FCDWithOffset / FIDWithOffset * ScanCorrect.GVal_mdtpitch[2] * CTSettings.detectorParam.vm / CTSettings.detectorParam.hm), 0.001);

			//スライス厚最大(mm)
			float SWMax = 0;

			//最大スライス厚計算(mm)
            SWMax = ScanCorrect.GetSWMax((short)ScanCorrect.GVal_ImgVsize,
                                         (short)ScanCorrect.GVal_ImgHsize, 
                                         ScanCorrect.GVal_ScanPosiA[2], 
                                         ScanCorrect.GVal_ScanPosiB[2], SWMin, 
                                         CTSettings.scansel.Data.multislice_pitch,
                                         (short)(CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone ? 1 : MyMultiSlice));

			//計算処理   v10.0引数変更 by 間々田 2005/02/14 GFlg_MltSlice→MyMultiSlice　GVal_MltPitch→MyScansel.MultiSlicePitch
			Val_ScnSlWidMax = SWMax;
			Val_ScnSlWidMin = SWMin;

			//スライス厚（画素）
			//最大値最小値設定
            //cwneSlicePix.SetMinMax(1, Val_ScnSlWidMax / Val_ScnSlWidMin);
            cwneSlicePix.Minimum = 1;
            //Rev20.00 追加 四捨五入 by長野 2015/01/26
            //cwneSlicePix.Maximum = Convert.ToDecimal(Val_ScnSlWidMax / Val_ScnSlWidMin);
            decimal tmpVal = 0;
            tmpVal = Convert.ToDecimal(Val_ScnSlWidMax / Val_ScnSlWidMin);
            cwneSlicePix.Maximum = Math.Round(tmpVal / cwneSlicePix.Increment, MidpointRounding.AwayFromZero) * cwneSlicePix.Increment;           

			//最大値最小値表示
			lblSlicePixMinMax.Text = StringTable.GetResString(StringTable.IDS_Range, cwneSlicePix.Minimum.ToString("0"), cwneSlicePix.Maximum.ToString("0"));
		}

        //********************************************************************************
        //機    能  ：  回転中心校正の設定値をコモンに書込む
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V2.0   00/02/08  (SI1)鈴山       新規作成
        //********************************************************************************
		private void Set_RotationCenter_Condition()
		{
			//再計算用
			float fFID = 0;     //FID(FID+FIDｵﾌｾｯﾄ)
			float fFCD = 0;     //FCD(FCD+FCDｵﾌｾｯﾄ)
			float fMDT = 0;     //検出器ﾋﾟｯﾁ(mm/画素)
			int iVMG = 0;       //縦倍率

			//現在値読み込み
			fFID = FIDWithOffset;
			fFCD = FCDWithOffset;
			fMDT = ScanCorrect.GVal_mdtpitch[2];
            //2014/11/07hata キャストの修正
            //iVMG = (int)(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);
            iVMG = Convert.ToInt32(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);

			//積算枚数   added V2.0 by 鈴山
            //2014/11/07hata キャストの修正
            //ScanCorrect.IntegNumAtRot = (int)cwneSum.Value;
            ScanCorrect.IntegNumAtRot = Convert.ToInt32(cwneSum.Value);

			//ビュー数
            //2014/11/07hata キャストの修正
            //ScanCorrect.ViewNumAtRot = (int)cwneScanView.Value;
            ScanCorrect.ViewNumAtRot = Convert.ToInt32(cwneScanView.Value);
			ScanCorrect.VIEW_N = ScanCorrect.ViewNumAtRot;  //added V2.0 by 鈴山

			//FID,FCDのコモンへの書き込みは回転中心校正結果フォームで保存するところに変更した by 山本 2000-4-14  → 復活 V3.0 by 鈴山
			//FID
			CTSettings.scansel.Data.fid = FIDWithOffset;

			//FCD
			//v29.99 複数X線管は今のところ不要のため変更 by長野 2013/04/08'''''ここから'''''
			//scansel.FCD = GVal_Fcd + scancondpar.fcd_offset(cboMultiTube.ListIndex) 'v9.0 change by 間々田 2004/03/10
			//CTSettings.scansel.Data.fcd = ScanCorrect.GVal_Fcd + CTSettings.scancondpar.Data.fcd_offset[0];
			//v29.99 複数X線管は今のところ不要のため変更 by長野 2013/04/08'''''ここまで'''''
            //Rev23.10 復活 by長野 2015/10/05
            CTSettings.scansel.Data.fcd = ScanCorrect.GVal_Fcd + CTSettings.scancondpar.Data.fcd_offset[cboMultiTube.SelectedIndex];

			//スライス厚(画素)   added V2.0 by 鈴山
			ScanCorrect.GVal_SlPix_Rot = Convert.ToInt32(cwneSlicePix.Value);

			//スライス厚(mm)   added V2.0 by 鈴山
			ScanCorrect.GVal_SlWid_Rot = ScanCorrect.Trans_PixToWid(ScanCorrect.GVal_SlPix_Rot, fFID, fFCD, fMDT, iVMG);    //v9.7変更 by 間々田 2004/12/02

			//１スライスのとき、最大同時スキャン枚数はコモンに０と書込む
			if (!ScanCorrect.MultiSliceMode)
            {
				//同時スキャン枚数：0(1ｽﾗｲｽ),1(3ｽﾗｲｽ),2(5ｽﾗｲｽ)
				CTSettings.scansel.Data.max_multislice = 0;  //v10.0追加 by 間々田 2005/02/14
				CTSettings.scansel.Data.multislice = 0;      //v10.0追加 by 間々田 2005/02/14
				
				//１スライスのとき、最大・最小スライス厚をコモンに書込む

				//スライス厚
				CTSettings.scansel.Data.max_slice_wid = Val_ScnSlWidMax; //最大スライス厚(mm)
				CTSettings.scansel.Data.min_slice_wid = Val_ScnSlWidMin; //最小スライス厚(mm)
			}

			//scansel書き込み
			//modScansel.PutScansel(ref modScansel.scansel);
            CTSettings.scansel.Write();

			//シェーディング補正の有無を記憶             'V4.0 append by 鈴山 2001/01/24
			ScanCorrect.GFlg_Shading_Rot = (chkShading.CheckState == CheckState.Checked);
		}

        //*******************************************************************************
        //機　　能： Ｘ線管（回転選択）コンボボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        //v29.99 今のところ不要 by長野 2013/04/15'''''ここから'''''
        //Private Sub cboMultiTube_Click()
        //
        //    GFlg_MultiTube_R = cboMultiTube.ListIndex
        //
        //End Sub
        //v29.99 今のところ不要 by長野 2013/04/15'''''ここまで'''''


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
		private void cmdEnd_Click(object sender, EventArgs e)
		{
			//フォームを消去
			//変更2015/1/17hata_非表示のときにちらつくため
            //Hide();
            modCT30K.FormHide(this);

			if ((!ScanCorrect.MultiSliceMode))
            {
				this.Close();
			}
		}

        //*******************************************************************************
        //機　　能： ＯＫボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void cmdOK_Click(object sender, EventArgs e)
		{
			int rc = 0;

			//RAMディスクが構築されているかどうか  'v17.40変更 byやまおか 2010/10/26
			//If UseRamDisk And (Not RamDiskIsReady) Then Exit Sub
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//    If UseRamDisk Then      'v17.42修正 byやまおか 2010/11/04
			//        If (Not RamDiskIsReady) Then Exit Sub
			//    End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //追加2014/10/07hata_v19.51反映
            //メカが動ける(パネルがOFF)かチェック     'v18.00追加 byやまおか 2011/07/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if ((!modMechaControl.IsOkMechaMove()))
                return;

            //マルチスライス以外の場合           追加 by 間々田 2004/12/28
			if (!ScanCorrect.MultiSliceMode)
            {
				//終了ボタンが使用不可？→校正実行中にクリックされたとみなし、dllに対して停止要求を行なう
				if (!cmdEnd.Enabled)
                {
					//'シーケンサ通信確認ファイルの書き込み（停止要求）
					//UserStopSet
					//
					//'連続回転コーンビーム＋高速再構成の時は、RAMディスクのscanstopを使う v17.40 追加 by 長野
					//If smooth_rot_cone_flg = True Then
					//
					//UserStopSet_rmdsk
					//
					//End If

					//実行中の処理に対して停止要求をする     'v17.50上記の処理を関数化 by 間々田 2011/02/17
					modCT30K.CallUserStopSet();

					return;
				}

				//フル２次元幾何歪補正時はスキャン位置校正ステータスをチェック
				if (CTSettings.scaninh.Data.full_distortion == 0)
                {
					if (!frmScanControl.Instance.IsOkScanPosition)
                    {
						//メッセージ表示：
						//   スキャン位置校正が準備完了でないため、処理を中止します。
						//   事前にスキャン位置校正を実施してください。
                        MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_CorNotReady, StringTable.IDS_CorScanPos), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}

				//'シーケンサ通信確認ファイルの値を0クリアする
				//'ClearCommCheck
				//UserStopClear      'v11.5変更 by 間々田 2006/04/14
				//
				//'連続回転コーンビーム＋高速再構成の時は、RAMディスクのscanstopを使う v17.40 追加 by 長野
				//If smooth_rot_cone_flg = True Then
				//
				//    UserStopClear_rmdsk
				//
				//End If

				//停止要求フラグをクリアする             'v17.50上記の処理を関数化 by 間々田 2011/02/17
				modCT30K.CallUserStopClear();

				//I.I.（またはFPD）電源のチェック
                if (!modSeqComm.PowerSupplyOK())
                {
                    return;
                }
			}

			//X線検出器がフラットパネルでなくかつ
			//幾何歪校正ステータスが準備未完了の場合は
			//確認メッセージを表示する
			//If Not frmScanControl.IsOkVertical() Then
			//v17.00変更 byやまおか 2010/02/24
            if ((!frmScanControl.Instance.IsOkVertical) && (!CTSettings.detectorParam.Use_FlatPanel))
            {
				//確認メッセージ：
				//   幾何歪校正が準備完了でないため、
				//   回転中心校正がエラーする可能性がありますが、
				//   実行しますか？
                if (MessageBox.Show(CTResources.LoadResString(9329) + "\r\n" + "\r\n", 
                                    Application.ProductName, 
                                    MessageBoxButtons.YesNo, 
                                    MessageBoxIcon.Exclamation, 
                                    MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    goto ExitHandler;
                }
			}

			//    'StatusReadメソッドを呼び出す（FID/FCD通信が可能な場合。タイマー停止中にて必要） by 間々田 2004/04/26
			//    If SeqStatusRead() Then
			//
			//        'Ｘ線管回転選択時
			//        If (scansel.rotate_select = 1) And (scaninh.rotate_select = 0) Then                            'v10.0変更 by 間々田 2005/02/08
			//#If v9Seq Then
			//            GVal_Fcd = CSng(Format$(MySeq.stsXrayFCD / 100, "0.0"))     'change 10→100 by 山本 2004-6-2
			//#Else
			//            GVal_Fcd = CSng(Format$(MySeq.stsFCD / 10, "0.0"))
			//#End If
			//        Else
			//            GVal_Fcd = CSng(Format$(MySeq.stsFCD / 10, "0.0"))
			//        End If

			//FCD+FCDオフセットが0以下になっていた場合、処理を中止する
			if (FCDWithOffset <= 0)
            {
				//メッセージ表示：
				//   FCD値が負の数になっているので、試料テーブルＹ軸のFCD値の誤差が大きい可能性があります。
				//   Ｙ軸の原点復帰とコモン初期化を行ってから再度実行してください。
				//MsgBox LoadResString(9330), vbCritical

				//v14.24変更 by 間々田 2009/03/10 メカ軸名称はコモンを使用する
				//   リソース9330の中身も以下に変更：
				//   FCD値が負の数になっているので、試料テーブル%1軸のFCD値の誤差が大きい可能性があります。
				//   %1軸の原点復帰とコモン初期化を行ってから再度実行してください。
                MessageBox.Show(StringTable.GetResString(9330, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[0].GetString())), 
                                Application.ProductName, 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
				return;
			}
			//v9.7追加ここまで by 間々田 2004/11/16

			//FCD のチェック  'v9.7追加 by added 間々田 2004/12/10
            if (!modSeqComm.CheckFCD(ScanCorrect.GVal_Fcd))
            {
                return;
            }
			//    End If

			//不要と思われる
			//    'FID/FCDを変えた場合の処理
			//    FIDFCD_Change
			//
			//    '最大スライス厚を計算してみる   v10.0引数変更 by 間々田 2005/02/14 GFlg_MltSlice→MyMultiSlice　GVal_MltPitch→MyScansel.MultiSlicePitch
			//    If GetSliceWidMax(GVal_ImgVsize, _
			//'                      GVal_ImgHsize, _
			//'                      GVal_ScanPosiA(2), _
			//'                      GVal_ScanPosiB(2), _
			//'                      FidWithOffset, _
			//'                      FcdWithOffset, _
			//'                      GVal_mdtpitch(2), _
			//'                      vm / hm, _
			//'                      scansel.multislice_pitch, _
			//'                      IIf(scansel.data_mode = DataModeCone, 1, MyMultiSlice), _
			//'                      True) = 0 Then
			//
			//        'メッセージ表示：
			//        '   最大スライス厚の計算に失敗しました。 _
			//'        '   事前に必要な校正を正しく行っていない可能性があります。
			//        '   幾何歪校正を実行後、回転中心校正を再度行ってください。
			//        MsgBox LoadResString(9502), vbExclamation
			//
			//        '終了手続き
			//        GoTo ErrorHandler
			//
			//    '最小スライス厚を計算してみる
			//    'ElseIf Cal_MinSliceWid_Ex(1) = 0 Then
			//    ElseIf GetSliceWidMin(FidWithOffset, _
			//'                          FcdWithOffset, _
			//'                          GVal_mdtpitch(2), _
			//'                          vm / hm, _
			//'                          True) = 0 Then
			//        'メッセージ表示：
			//        '   最小スライス厚の計算に失敗しました。
			//        '   事前に必要な校正を正しく行っていない可能性があります。
			//        '   幾何歪校正を実行後、回転中心校正を再度行ってください。
			//        MsgBox LoadResString(9498), vbExclamation
			//
			//        '終了手続き
			//        GoTo ErrorHandler
			//
			//    End If

			//回転中心校正の設定値をコモンに書込む
			Set_RotationCenter_Condition();

			//マルチスライスでの処理はここで終了
			if (ScanCorrect.MultiSliceMode)
            {
				OK = true;

				//終了手続き
				goto ExitHandler;
			}

			//ビジーフラグセット
			IsBusy = true;

            //シフトスキャン収集が選ばれているときは   'v18.00追加 byやまおか 2011/02/28 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //if (CTSettings.scansel.Data.scan_mode == 4)
            //Rev25.00 Wスキャンを追加 by長野 2016/07/07
            if (CTSettings.scansel.Data.scan_mode == 4)
            {
                //'テーブル位置を確認    'v18.00削除 byやまおか 2011/07/04
                //If (frmMechaControl.ntbTableXPos.Value < 0) Then
                //    MsgBox "テーブル" + AxisName(1) + "を正方向へ移動してください。", vbCritical
                //    GoTo ErrorHandler
                //End If
                modScanCorrect.Flg_RCShiftScan = true;  //フラグON

            //シフトスキャン収集が選ばれていないときは
            }
            else
            {
                modScanCorrect.Flg_RCShiftScan = false; //フラグOFF
            }


			//回転を0度に戻す    'v17.61/v18.00追加 byやまおか 2011/07/30
			if ((!frmStatus.Instance.Check_RotateZero()))
            {
				IsBusy = false;
				goto Err_Process;
			}

			//回転中心校正画像を配列に読み込む
			//    If Not Get_RotationCenter_Image(0) Then
			//If Not GetImageRotationCenterCorrect(0) Then GoTo ErrorHandler
            if (!modScanCorrectNew.GetImageRotationCenterCorrect(0, 
                                                                 ScanCorrect.ViewNumAtRot, 
                                                                 ScanCorrect.IntegNumAtRot, 
                                                                 ScanCorrect.GVal_SlWid_Rot, 
                                                                 stsRotCor, 
                                                                 pgbRotationCenter))
            {
                goto ErrorHandler;
            }   //v10.0変更 by 間々田 2005/02/04

			//回転中心パラメータ計算
			//rc = Get_RotationCenter_Parameter_Ex(0)
			rc = ScanCorrect.Get_RotationCenter_Parameter_Ex(0, ScanCorrect.ViewNumAtRot);
			//v10.0変更 by 間々田 2005/02/04

			//エラー判定
			if (rc == 0)
            {
				//フォームを非表示にする
				//変更2015/1/17hata_非表示のときにちらつくため
                //Hide();
                modCT30K.FormHide(this);

				Application.DoEvents(); //added by 山本　2004-8-18

				//回転中心校正結果フォームを表示する
				frmRotationCenterResult.Instance.Dialog();

				//メカ制御ボードの初期化
                rc = modMechaControl.RotateInit(modDeclare.hDevID1);

                //Rev25.01 原点復帰をz相で確認するタイプだと初期化で準備完了が落ちるため、回転軸原点復帰 2016/12/17 by長野
                rc = modMechaControl.MecaRotateOrigin(true);
			}
            else if (rc == 1)   //V4.0 append by 鈴山 2001/04/03
            {
			}
            else
            {
				//メカ制御ボードの初期化
				rc = modMechaControl.RotateInit(modDeclare.hDevID1);

                //Rev25.01 原点復帰をz相で確認するタイプだと初期化で準備完了が落ちるため、回転軸原点復帰 2016/12/17 by長野
                rc = modMechaControl.MecaRotateOrigin(true);

				//終了手続き
				goto ErrorHandler;
			}

ExitHandler:

			//フォームを消去
			//hide
            if (this.Visible)
            {
                //変更2015/1/17hata_非表示のときにちらつくため
                //Hide();
                modCT30K.FormHide(this);
            }
			//v16.30/v17.00変更 byやまおか 2010/03/02

			if ((!ScanCorrect.MultiSliceMode))
            {
				this.Close();
			}

			return;

//例外処理
Err_Process:

			//メッセージ表示：
			//   校正処理に失敗しました。
			//   回転中心校正を再度行ってください。
            MessageBox.Show(StringTable.GetResString(9319, this.Text), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

ErrorHandler:

			//「データ収集異常終了」と表示
			stsRotCor.Status = StringTable.GC_STS_CAPT_NG;

			//ビジーオフ
			IsBusy = false;

			if ((!ScanCorrect.MultiSliceMode) && (!this.Visible))
            {
				this.Close();
			}
		}

        //*******************************************************************************
        //機　　能： 各コントロールの初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void InitControls()
		{
			//複数Ｘ線管に対応
			//if (CTSettings.scaninh.Data.multi_tube == 0)
            //Rev23.10 条件追加 by長野 2015/10/05
            if(CTSettings.scaninh.Data.multi_tube == 0 && CTSettings.scaninh.Data.xray_remote == 1) 
            {
                lblMultiTube.Text = CTResources.LoadResString(StringTable.IDS_XrayTube);          //Ｘ線管
                cboMultiTube.Items.Add(modLibrary.RemoveNull(CTSettings.infdef.Data.multi_tube[0].GetString()));  //130kV
                cboMultiTube.Items.Add(modLibrary.RemoveNull(CTSettings.infdef.Data.multi_tube[1].GetString()));  //225kV
			}
            //回転選択対応   v9.0 added by 間々田 2004/02/13
            else if (CTSettings.scaninh.Data.rotate_select == 0)
            {
				lblMultiTube.Text = CTResources.LoadResString(12331);                             //回転選択
				cboMultiTube.Items.Add(CTResources.LoadResString(StringTable.IDS_Table));         //テーブル
				cboMultiTube.Items.Add(CTResources.LoadResString(StringTable.IDS_XrayTube));      //Ｘ線管
			}
            //どちらにも対応していない場合、複数Ｘ線管／回転選択を非表示とし、上に詰める
            else
            {
                cboMultiTube.Items.Add(modLibrary.RemoveNull(CTSettings.infdef.Data.multi_tube[0].GetString()));  //130kV
                cboMultiTube.Items.Add(modLibrary.RemoveNull(CTSettings.infdef.Data.multi_tube[1].GetString()));  //225kV
				
				lblMultiTube.Visible = false;
				cboMultiTube.Visible = false;
                //2014/11/07hata キャストの修正
                //chkShading.Top = chkShading.Top - (600 / 15);       //シェーディング補正
                //cmdOK.Top = cmdOK.Top - (600 / 15);                 //ＯＫ
                //cmdEnd.Top = cmdEnd.Top - (600 / 15);               //終 了
                //this.Height = this.Height - (600 / 15);             //フォームの高さ
                chkShading.Top = chkShading.Top - 40;       //シェーディング補正
                cmdOK.Top = cmdOK.Top - 40;                 //ＯＫ
                cmdEnd.Top = cmdEnd.Top - 40;               //終 了
                this.Height = this.Height - 40;             //フォームの高さ
            }

            //2014/11/07hata キャストの修正
            //this.Width = lblMessage.Width + (900 / 15);
            this.Width = lblMessage.Width + 60;
        
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
		private void frmRotationCenter_Load(object sender, EventArgs e)
		{
			//実行時はフラグをセット
			modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTScanCorrect;

			//メカコントロールフォームの参照を取得
			_myMechaControl = frmMechaControl.Instance;
            _myMechaControl.FCDChanged += new EventHandler(myMechaControl_FCDChanged);
            _myMechaControl.FIDChanged += new EventHandler(myMechaControl_FIDChanged);

			//キャプションのセット
			SetCaption();

            //各コントロールの初期化
			InitControls();

            //現在のコモン内容を取り出す
			//V4.0 append by 鈴山 2001/01/29
			if (!ScanCorrect.MultiSliceMode)
            {
				ScanCorrect.OptValueGet_Cor();

				//GFlg_MltSlice = 0       '１スライスのとき、同時スキャン枚数を変更（ここではコモンには書込まない）
				//v10.0以下に変更 by 間々田 2005/02/14
				MyMultiSlice = 0;
			}
            else
            {
				MyMultiSlice = CTSettings.scansel.Data.multislice;
			}

			//複数スライスのとき、一部の入力を制限
			if (ScanCorrect.MultiSliceMode)
            {
				cwneSlicePix.Enabled = false;
			}

			//表示
			Disp_RotationCenter_Condition();

			//停止中
			stsRotCor.Status = StringTable.GC_STS_STOP;
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
		private void frmRotationCenter_FormClosed(object sender, FormClosedEventArgs e)
		{
			//メカコントロールフォームの参照を破棄
            if (_myMechaControl != null)
            {
                //_myMechaControl.Dispose();
                _myMechaControl.FCDChanged -= myMechaControl_FCDChanged;
                _myMechaControl.FIDChanged -= myMechaControl_FIDChanged;
                
                _myMechaControl = null;
                
            }

			//ビジーオフ
			IsBusy = false;

			//元の画面に戻る
			if (!ScanCorrect.MultiSliceMode)
            {
				//終了時はフラグをリセット
				modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTScanCorrect);
			}

            //Rev20.01 追加 by長野 2015/06/03
            frmXrayControl.Instance.UpdateWarmUp();
        
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
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void SetCaption()
		{
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

			//メッセージ
			if (CTSettings.scaninh.Data.xray_remote == 0)
            {
				//回転中心校正ファントムを試料テーブルに取り付けてください。
				//準備ができたらＯＫをクリックしてください。
                lblMessage.Text = CTResources.LoadResString(9467) + "\r" + lblMessage.Text;
			}
            else
            {
				//回転中心校正ファントムを試料テーブルに取り付け、Ｘ線をオンしてください。
				//準備ができたらＯＫをクリックしてください。
                lblMessage.Text = CTResources.LoadResString(9466) + "\r" + lblMessage.Text;
			}

			//v17.60 フォントがバラつき不自然なため削除 by 長野 2011/06/11
			//英語環境の場合、ラベルコントロールに使用するフォントをArialにする
			//If IsEnglish Then SetLabelFont Me
		}

        //*******************************************************************************
        //機　　能： ダイアログ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [ /O] Boolean   True:「はい」・False:「いいえ」
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		public bool Dialog()
		{
			bool functionReturnValue = false;

			//戻り値の初期化
			OK = false;

			//フォームを表示
			//Me.Show , frmCTMenu
            this.ShowDialog(frmCTMenu.Instance);    //v16.30/v17.00 Modal化 byやまおか 2010/02/10

            while (this.Visible)
            {
				Application.DoEvents();
			}

			//戻り値のセット
			functionReturnValue = OK;

			//フォームをアンロード
			this.Close();
			return functionReturnValue;
		}

        //追加2014/11/28hata_v19.51_dnet
        //*************************************************************************************************
        //機　　能： ビュー数変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V19.51dnet  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void cwneScanView_ValueChanged(object sender, EventArgs e)
        {
            decimal val1 = 0;

            //数字のインクリメントを合わせる10,100,200･･･
            val1 = ScanCorrect.RoundControlVale(cwneScanView.Value, cwneScanView.Maximum, cwneScanView.Minimum, 100F);
            if (cwneScanView.Value != val1)
            {
                cwneScanView.Value = val1;
            }
        }
  
    }
}
