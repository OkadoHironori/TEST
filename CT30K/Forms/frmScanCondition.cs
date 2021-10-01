using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

//
using CT30K.Common;
using CTAPI;
using TransImage;
using CT30K.Properties;

namespace CT30K
{
	///* ************************************************************************** */
	///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： frmScanCondition.frm                                        */
	///* 処理概要　　： スキャン条件                                                */
	///* 注意事項　　： なし                                                        */
	///* -------------------------------------------------------------------------- */
	///* 適用計算機　： DOS/V PC                                                    */
	///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
	///* コンパイラ　： VB 6.0                                                      */
	///* -------------------------------------------------------------------------- */
	///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
	///*                                                                            */
	///* V1.00       XX/XX/XX    (TOSFEC) ????????                                  */
	///* V2.0        00/02/08    (TOSFEC) 鈴山　修   V1.00を改造                    */
	///* V3.0        00/08/01    (TOSFEC) 鈴山　修   ｺｰﾝﾋﾞｰﾑCT対応                  */
	///* V8.0        03/12/15    (SI4)    松井       分散処理対応                   */
	///* V9.0        04/02/15    (SI4)    間々田     スキャノモード廃止             */
	///* v9.7        04/12/08    (SI4)    間々田     アーティファクト低減対応       */
	///* v19.00      12/02/21    H.Nagai             BHC対応                        */
	///*                                                                            */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
	///* ************************************************************************** */
	public partial class frmScanCondition : Form
	{

		//*********************************************************************************************************************
		//  共通データ宣言
		//*********************************************************************************************************************

		//初期値保存用
		//private float InitFid = 0;			//FID(オフセットを含まない)
		//private float InitFcd = 0;			//FCD(オフセットを含まない)

		//scansel（コモン）バックアップ用
		//private modScansel.scanselType ScanselOrg;   //v11.2追加 by 間々田 2006/01/13
        private CTstr.SCANSEL ScanselOrg;   //v11.2追加 by 間々田 2006/01/13


		//コーンビームスキャンか？
		private bool IsCone = false;

		//プリセット(画像)か？
		private bool IsPresetImage = false;		//v17.61追加 byやまおか 2011/06/28

		//生データ保存ありか？
		private bool IsRawDataSave = false;

		private float[] hizumi = null;		//幾何歪ﾃｰﾌﾞﾙ                    '配列化 V3.0 by 鈴山
		private int mc = 0;					//縦中心ﾁｬﾝﾈﾙ(ﾗｲﾝ数半幅)         'V3.0 change by 鈴山 2000/10/02 (Single → Integer)
        //変更2014/10/07hata_v19.51反映
        //private int mc_max = 0;				//最大縦中心ﾁｬﾝﾈﾙ                'V3.0 change by 鈴山 2000/10/02 (Single → Integer)
        private int[] mc_max = new int[2];   	//最大縦中心ﾁｬﾝﾈﾙ 0:非シフト 1:シフト    'v18.00変更 byやまおか 2011/07/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        
        private float ZW = 0;				//可能Z方向幅(mm)
		private double? ksw = null;			//ksw?
		//Private ZW0             As Single   'ビームZ幅(mm)
		private float zs = 0;				//再構成開始位置(mm)
		private float ze = 0;				//再構成終了位置(mm)
		private float delta_msw = 0;		//画面上のｽﾗｲｽ幅(mm)
		private float kzp = 0;				//1+alpha/pai
		private float mcl = 0;				//ビュー数の制約による最大mc   　　'added by 山本　2003-9-27

		private int? myContTime = null;

		private float FIDWithOffset = 0;	//FID（オフセットを含む）
		private float FCDWithOffset = 0;	//FCD（オフセットを含む）
		private float FCDFIDRate = 0;		//FcdWithOffset/FidWithOffset

		private float SWMin = 0;			//FcdFidRate * scancondpar.mdtpitch(2) * vm / hm

		private float SWMinCone = 0;		//FcdFidRate * scancondpar.dpm
		private float SWMaxPara = 0;		//2 * (mc_max - 1) * SWMinCone
		private float SWMaxParaV = 0;		//2 * (mcl - 1) * SWMinCone

		//コーンスライスピッチ（画素）
		private double? myConeSlicePitchPix = null;

		//昇降位置
		private float udPos;

		//メカ制御画面への参照
		private frmMechaControl myMechaControl = null;

		//Ｘ線制御画面への参照
		private frmXrayControl myXrayControl = null;

		//回転中心が左右に寄っていてもスキャンを続行するどうか判定するためのプロパティ 'v17.65 追加 by長野(検S1) 2011/11/26
		private bool myScanOptValueChk2ok = false;

        private bool myNoClose = false;


        //追加2014/06/23(検S1)hata
        //Load時のイベントロック用
        //private bool AreaEvntLock = false;
        //private bool DeltazEvntLock = false;
        //private bool ImgRotAnglEvntLock = false;
        //private bool IntegEvntLock = false;
        //private bool KEvntLock = false;
        //private bool MSPitchEvntLock = false;
        //private bool MSSliceEvntLock = false;
        //private bool SliceEvntLock = false;
        //private bool SlicePixEvntLock = false;
        //private bool ViewNumEvntLock = false;
        //private bool ZpEvntLock = false;
        private bool EvntLock = false;
        //private bool NoVisibleEvntLock = false;
        
		private RadioButton[] optMultiTube = null;
		private RadioButton[] optMultislice = null;
		private RadioButton[] optBinning = null;

        //private RadioButton[] optScanMode = null;//変更2014/10/07hata_v19.51反映
        public RadioButton[] optScanMode = null;
		
        private RadioButton[] optMultiScanMode = null;
		private RadioButton[] optMatrix = null;
		private RadioButton[] optFilter = null;
		private RadioButton[] optDirection = null;
		private RadioButton[] optFilterProcess = null;
		private RadioButton[] optRFC = null;
		private RadioButton[] optReconMask = null;
		private RadioButton[] optImageMode = null;
		private RadioButton[] optTableRotation = null;
		private RadioButton[] optAutoCentering = null;
		private RadioButton[] optScanAndView = null;
		private RadioButton[] optContrastFitting = null;
		private RadioButton[] optHelical = null;
		private RadioButton[] optRotateSelect = null;
		private Panel[] fraMenu = null;
        private RadioButton[] optOverScan = null;

        //追加2015/01/27hata
        private TabPageCtrl tabPage;

		private static frmScanCondition _Instance = null;

        private frmXrayControl.ChangedEventArgs changedEventArgs  = null;

        //追加2014/10/07hata_v19.51反映
        //v18.00追加(ここから) byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        //クリックする前のスキャンモード
        private int optScanMode_oldIndex;

        //Rev25.00 追加 by長野 2016/07/11
        //クリックする前のWスキャン
        private CheckState chkW_Scan_oldCheckState;

        //Rev26.00 プリセット設定時、スキャン条件の表示項目を切り替えるためのフラグ by chouno 2017/08/31
        private bool m_PresetSettingFlg = false;

		public frmScanCondition()
		{
			InitializeComponent();

			optMultiTube = new RadioButton[] { optMultiTube0, optMultiTube1 };
			optMultislice = new RadioButton[] { optMultislice0, optMultislice1, optMultislice2 };
			optBinning = new RadioButton[] { optBinning0, optBinning1, optBinning2 };
            optScanMode = new RadioButton[] { null, optScanMode1, optScanMode2, optScanMode3, optScanMode4 };
			optMultiScanMode = new RadioButton[] { null, optMultiScanMode1, null, optMultiScanMode3, null, optMultiScanMode5 };
			optMatrix = new RadioButton[] { null, optMatrix1, optMatrix2, optMatrix3, optMatrix4, optMatrix5 };
			optFilter = new RadioButton[] { null, optFilter1, optFilter2, optFilter3 };
			optDirection = new RadioButton[] { optDirection0, optDirection1 };
			optFilterProcess = new RadioButton[] { optFilterProcess0, optFilterProcess1 };
			optRFC = new RadioButton[] { optRFC0, optRFC1, optRFC2, optRFC3};
			optReconMask = new RadioButton[] { optReconMask0, optReconMask1 };
			optImageMode = new RadioButton[] { optImageMode0, optImageMode1, optImageMode2 };
			optTableRotation = new RadioButton[] { optTableRotation0, optTableRotation1 };
			optAutoCentering = new RadioButton[] { optAutoCentering0, optAutoCentering1 };
			optScanAndView = new RadioButton[] { optScanAndView0, optScanAndView1 };
			optContrastFitting = new RadioButton[] { optContrastFitting0, optContrastFitting1 };
			optHelical = new RadioButton[] { optHelical0, optHelical1 };
			optRotateSelect = new RadioButton[] { optRotateSelect0, optRotateSelect1 };
			fraMenu = new Panel[] { fraMenu0, fraMenu1, fraMenu2 };
            optOverScan = new RadioButton[] { optOverScan0, optOverScan1 };

            changedEventArgs = new frmXrayControl.ChangedEventArgs();

            //追加2015/01/27hata
            //TaPageの表示／非表示切り替えのため、TabPageCtrlオブジェクトを作成
            tabPage = new TabPageCtrl(this.sstMenu);


            //追加2015/03/18hata_Loadから移動
            //変数初期化
            ksw = null;
            myContTime = null;
            myConeSlicePitchPix = null;

        }

		public static frmScanCondition Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmScanCondition();
				}

				return _Instance;
			}
		}
		//*******************************************************************************
		//機　　能： スキャン条件を設定する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： IsConebeam      [I/ ] Boolean   コーンビーム？
		//           IsVisible       [I/ ] Boolean   表示する？
		//           K               [I/ ] Long      コーンビーム枚数
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2009/06/17 (SS1)間々田   リニューアル
		//*******************************************************************************
		//Public Sub Setup(Optional ByVal IsVisible As Boolean = False, Optional ByVal IsConeBeam As Variant, Optional ByVal K As Long = 0)
        //public void Setup(bool IsVisible = false, bool? IsConeBeam = null, int K = 0, double? swPix = null, double? delta_z_pix = null, bool? NoClose = null)		//変更 2009/08/20 by 間々田
        //public void Setup(bool IsVisible = false, bool? IsConeBeam = null, int K = 0, double? swPix = null, double? delta_z_pix = null, bool? NoClose = null,bool? scanMat = null,float? sw_magnify = null)
        public void Setup(bool IsVisible = false, bool? IsConeBeam = null, int K = 0, double? swPix = null, double? delta_z_pix = null, bool? NoClose = null,bool? scanCond = null,float? sw_magnify = null,bool? presetSetting = false) //Rev26.00 Change by chouno 2017/08/31
        {
           
            //windowStateを設定する
            //Load関数がないので次で対応(ShowInTaskbar = false / WindowState = FormWindowState.Minimized)
            //HideをするとVisivleがFalseになってしまうので注意。
            if (IsVisible)
            {
                //NoVisibleEvntLock = false;
                //EvntLock = true;

                //削除2014/12/22hata_dNet
                //frmScanCondition.Instance.TopMost = true;
                frmScanCondition.Instance.WindowState = FormWindowState.Normal;

            }
            else
            {
                //NoVisibleEvntLock = true;
                //EvntLock = true;
                frmScanCondition.Instance.TopMost = false;
                frmScanCondition.Instance.WindowState = FormWindowState.Minimized;
            }

			//コーンビーム？
			if (IsConeBeam == null)
			{
                IsCone = (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone);
			} 
			else
			{
				IsCone = (bool)IsConeBeam;
			}

			//プリセット(画像)？     'v17.61追加 byやまおか 2011/06/28
			//IsPresetImage = Convert.ToBoolean((frmScanControl.Instance.optQuality[5].Checked == true) && (frmScanControl.Instance.chkImageCondition.CheckState == CheckState.Checked));
            
            //Rev26.10 ガイドモードオフの場合はoptQualityとchkImgConditionを見る
            if (CTSettings.scaninh.Data.guide_mode == 0)
            {
                //Rev26.00 change by chouno 2017/08/31
                IsPresetImage = Convert.ToBoolean(frmScanControl.Instance.txtPresetImgFile.Text != String.Empty);
            }
            else
            {
                IsPresetImage = Convert.ToBoolean((frmScanControl.Instance.optQuality[5].Checked == true) && (frmScanControl.Instance.chkImageCondition.CheckState == CheckState.Checked));
            }

			//v19.10/v19.20の反映 FPDの場合、フォームのロードに入る前にscancondpar.cone_max_mfanangleを更新する(FDD依存の値のため） by長野 2012/11/28
			//プリセット、スキャン条件設定画面オープン・クローズ時、スキャンスタート時は必ずここを通過するので、ここでよい。
			//念のため、スキャン位置校正時にも追加している。
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
			{
				//Update_cone_max_mfanangle
				//v19.12 追加 シングルで使っているmax_mfanangleも同時に更新するので関数名変更 by長野 2013/03/12
				Update_scan_max_mfanangle();
			}

            //Rev20.00 シングルハーフでオートセンタリングの有無のフラグを設定 by長野 2015/01/24
            if ((CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScan) && (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeHalf))
            {
                //if (CTSettings.scansel.Data.auto_centering == 0 && CTSettings.scansel.Data.over_scan == 0 && CTSettings.scansel.Data.rfc == 0)
                //{
                //    if (CTSettings.scansel.Data.scan_and_view == 1 && CTSettings.scansel.Data.table_rotation == 0 && CTSettings.scansel.Data.matrix_size < 3)
                //    {
                //        modScanCondition.HalfNoAutoCenteringFlg = 1;
                //    }
                //    else
                //    {
                //        modScanCondition.HalfNoAutoCenteringFlg = 0;
                //    }
                //}
                //else
                //{
                //    modScanCondition.HalfNoAutoCenteringFlg = 0;
                //}
                //
                if (CTSettings.scansel.Data.auto_centering == 0 && CTSettings.scansel.Data.over_scan == 0 && CTSettings.scansel.Data.rfc == 0)
                {
                    modScanCondition.HalfNoAutoCenteringFlg = 1;
                }
                else
                {
                    modScanCondition.HalfNoAutoCenteringFlg = 0;
                }
            }
            else
            {
                modScanCondition.HalfNoAutoCenteringFlg = 0;
            }

            //'フォームのロード
            //Load Me

            //通常の設定か、プリセット用の設定画面を開くかどうかの分岐フラグセット //Rev26.00 add by chouno 2017/08/31
            m_PresetSettingFlg = (bool)presetSetting;

            //非表示モードの場合、ォームのロードを実行
            if (IsVisible)
            {
                //tes中!!_畑_2014/09/11
                //EvntLock = true;
                //フォームのロード
                //変更2015/01/17hata
                //this.Show();
                this.Show(frmCTMenu.Instance);
            }
            else
            {
                //tes中!!_畑_2014/09/11
                //EvntLock = false;
                //フォームロード処理
                frmScanCondition_Load(this, EventArgs.Empty);
            }


			//スライス厚もしくはピッチが指定されていて、かつスライス枚数が指定されている場合、いったん枚数を１にする '追加 2009/08/20 by 間々田
			if ((swPix != null || delta_z_pix != null) && (K > 0))
			{
                //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                //cwneK.Value = 1;
                if ((cwneK.Maximum >= 1) && (cwneK.Minimum <= 1))
                {   //代入時のイベントロック
                    //KEvntLock = true;
                    cwneK.Value = 1;
                    //KEvntLock = false;
                }
                //追加2015/01/29hata
                else if (cwneK.Maximum < 1)
                {
                    cwneK.Value = cwneK.Maximum;
                }
                else if (cwneK.Minimum > 1)
                {
                    cwneK.Value = cwneK.Minimum;
                }           
            }

			//スライス厚が指定されている '追加 2009/08/20 by 間々田
			if (swPix != null)
			{
                //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                //cwneSlicePix.Value = (decimal)swPix;
                if ((cwneSlicePix.Maximum >= (decimal)swPix) && (cwneSlicePix.Minimum <= (decimal)swPix))
                {
                    //代入時のイベントロック
                    //SlicePixEvntLock = true;    //代入時のイベントロック 追加2014/06/23(検S1)hata
                    //Rev20.00 追加 四捨五入 by長野 2015/01/26
                    //cwneSlicePix.Value = (decimal)swPix;
                    //cwneSlicePix.Value = Math.Round((decimal)swPix / cwneSlice.Increment, MidpointRounding.AwayFromZero) * cwneSlice.Increment;
                    //Rev26.30/Rev26.15 四捨五入はしない by chouno 2018/10/15
                    cwneSlicePix.Value = (decimal)swPix;
                    
                    //SlicePixEvntLock = false;   //代入時のイベントロック 追加2014/06/23(検S1)hata
                }
                //追加2015/01/29hata
                else if (cwneSlicePix.Maximum < (decimal)swPix)
                {
                    cwneSlicePix.Value = cwneSlicePix.Maximum;
                }
                else if (cwneSlicePix.Minimum > (decimal)swPix)
                {
                    cwneSlicePix.Value = cwneSlicePix.Minimum;
                }
            }

			//プリセット(画像)の場合はここでswPixではなくcone_scan_widthを入れなおす   'v17.61追加 byやまおか 2011/06/29
			if (IsPresetImage)
			{
				//スライスピッチ(mm)
                //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                //cwneSlice.Value = (decimal)CTSettings.scansel.Data.cone_scan_width;
                if ((cwneSlice.Maximum >= (decimal)CTSettings.scansel.Data.cone_scan_width) && (cwneSlice.Minimum <= (decimal)CTSettings.scansel.Data.cone_scan_width))
                {
                    //代入時のイベントロック
                    //SliceEvntLock = true;
                    cwneSlice.Value = (decimal)CTSettings.scansel.Data.cone_scan_width;
                    //SliceEvntLock = false;
                }
                //追加2015/01/29hata
                else if (cwneSlice.Maximum < (decimal)CTSettings.scansel.Data.cone_scan_width)
                {
                    cwneSlice.Value = cwneSlice.Maximum;
                }
                else if (cwneSlice.Minimum > (decimal)CTSettings.scansel.Data.cone_scan_width)
                {
                    cwneSlice.Value = cwneSlice.Minimum;
                }
            }

			//ピッチが指定されている     '追加 2009/08/20 by 間々田
			//If Not IsMissing(delta_z_pix) Then
			if ((delta_z_pix != null) && IsCone)			//変更 2009/08/21 by 間々田
			{
				//cwneDelta_z.Value = delta_z_pix * scansel.FCD / scansel.Fid * scancondpar.dpm
				//v17.62 プリセットの値から計算したピッチを小数点3桁で丸めるようにする（丸めないと保存したピッチに対して±0.001ズレる) by長野 2011/09/26

                //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                //decimal fdltaz0 = (decimal)(delta_z_pix * Math.Round(CTSettings.scansel.Data.fcd / CTSettings.scansel.Data.fid * CTSettings.scancondpar.Data.dpm, 3));		//v17.64追加 v17.63は反映漏れ byやまおか 2011/10/21
                //Rev26.30/Rev26.15 修正 by chouno 2018/10/15
                decimal fdltaz0 = Math.Round((decimal)delta_z_pix * cwneSlice.Minimum,3);

                if ((cwneDelta_z.Maximum >= fdltaz0) && (cwneDelta_z.Minimum <= fdltaz0))
                {   //代入時のイベントロック
                    //DeltazEvntLock = true;
                    cwneDelta_z.Value = fdltaz0;		//v17.64追加 v17.63は反映漏れ byやまおか 2011/10/21
                    //DeltazEvntLock = false;  
                }
                //追加2015/01/29hata
                else if (cwneDelta_z.Maximum < fdltaz0)
                {
                    cwneDelta_z.Value = cwneDelta_z.Maximum;
                }
                else if (cwneDelta_z.Minimum > fdltaz0)
                {
                    cwneDelta_z.Value = cwneDelta_z.Minimum;
                }
            }

			//プリセット(画像)の場合はここでdelta_z_pixではなくdelta_zを入れなおす   'v17.61追加 byやまおか 2011/06/29
			if (IsPresetImage)
			{
				//スライスピッチ(mm)
                //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                decimal fdltaz1 = (decimal)CTSettings.scansel.Data.delta_z;
                //Rev20.00 四捨五入を追加 by長野 2015/01/26
                fdltaz1 = Math.Round(fdltaz1 / cwneDelta_z.Increment,MidpointRounding.AwayFromZero) * cwneDelta_z.Increment;
                if ((cwneDelta_z.Maximum >= fdltaz1) && (cwneDelta_z.Minimum <= fdltaz1))
                {   //代入時のイベントロック
                    //DeltazEvntLock = true;
                    cwneDelta_z.Value = fdltaz1;
                    //DeltazEvntLock = false;
                }
                //追加2015/01/29hata
                else if (cwneDelta_z.Maximum < fdltaz1)
                {
                    cwneDelta_z.Value = cwneDelta_z.Maximum;
                }
                else if (cwneDelta_z.Minimum > fdltaz1)
                {
                    cwneDelta_z.Value = cwneDelta_z.Minimum;
                }
            }

			//コーンビーム枚数の設定
			if ((K > 0) && IsCone)
			{
                //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                //cwneK.Value = K;
                if ((cwneK.Maximum >= K) && (cwneK.Minimum <= K))
                {   //代入時のイベントロック
                    //KEvntLock = true;
                    cwneK.Value = K;
                    //KEvntLock = false;
                }
                //追加2015/01/29hata
                else if (cwneK.Maximum < K)
                {
                    cwneK.Value = cwneK.Maximum;
                }
                else if (cwneK.Minimum > K)
                {
                    cwneK.Value = cwneK.Minimum;
                }            
            }

            //画質マトリクスの場合 //Rev26.00 by chouno 2017/02/08
            if (scanCond == true)
            {
                //cwneDelta_z.Value = cwneDelta_z.Maximum;
                float sw = 0.0f;
                sw = (float)cwneDelta_z.Value * (float)sw_magnify;
                cwneSlice.Value = modLibrary.CorrectInRange((decimal)sw, cwneSlice.Minimum, cwneSlice.Maximum);
            }


			//非表示モードの場合、OKボタンクリック時処理を実行
			if (IsVisible)
			{
				//フォームのshow
                //this.Show(frmCTMenu.Instance);
                this.Visible = true;
                //tes中!!_畑_2014/09/11
                //EvntLock = false;
            }
			else
			{
                if (NoClose != null)
                {
                    myNoClose = true;
                }

                //変更2014/10/07hata_v19.51反映
                //cmdOK_Click(cmdOK, EventArgs.Empty);
                SetScanCondition();     //V19.20 ＯＫボタンクリック処理と別にする by Inaba 2012/11/01 'v19.50 統合 v19.41との統合 by長野 2013/11/17

                //frmScanConditionは閉じないようにする
                //if (modLibrary.IsExistForm("frmScanCondition")) this.Close();  //追加 2009/08/24
			}

			//コーンビームスキャンかつ連続回転モードの場合 'v16.2 連続回転コーンビーム時対応 by 山影 2010/01/19
			//UpdateIntegMin
            myNoClose = false;

        }


		//*******************************************************************************
		//機　　能： ＦＣＤ・ＦＩＤ値変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void UpdateFcdFid()
		{
			int Index = 0;
			Index = modLibrary.GetOption(optMultiTube);
			if (Index == -1) return;


			//FID/FCDオフセット
            FIDWithOffset = ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[Index];		//FID（オフセットを含む）
            FCDWithOffset = ScanCorrect.GVal_Fcd + CTSettings.scancondpar.Data.fcd_offset[Index];		//FCD（オフセットを含む）

			//Fcd/Fid比率
			FCDFIDRate = FCDWithOffset / FIDWithOffset;

            //シフトスキャン？   'v18.00追加 byやまおか 2011/07/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            int sft_mode = 0;
            //sft_mode = (ScanCorrect.IsShiftScan() ? 1 : 0);
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/08/10
            sft_mode = ((ScanCorrect.IsShiftScan() || ScanCorrect.IsW_Scan())  ? 1 : 0);

			float OldSWMin = 0;
			OldSWMin = (float)cwneSlice.Minimum;
			
            if (IsCone)
			{
				//スキャンエリア最大値の更新
				UpdateScanAreaMax();

				//上記FcdFidRateにscancondpar.dpmを掛けたもの
                SWMinCone = FCDFIDRate * CTSettings.scancondpar.Data.dpm;

				//上記SWMinConeに 2 * (mc_max - 1)を掛けたもの
				//SWMaxPara = 2 * (mc_max - 1) * SWMinCone;
                //変更2014/10/07hata_v19.51反映
                //SWMaxPara = 2 * (mc_max[0] - 1) * SWMinCone; //Rev20.00 2014/08/27 コモンの変更・追加の仮対応 by長野
                SWMaxPara = 2 * (mc_max[sft_mode] - 1) * SWMinCone; //v18.00シフト対応 byやまおか 2011/07/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

				//上記SWMinConeに 2 * (mcl - 1)を掛けたもの
				SWMaxParaV = 2 * (mcl - 1) * SWMinCone;

				//最大スライス厚の設定
				UpdateSWMax();

				//最大スライス枚数の設定
				UpdateKMax();

				//最大スライスピッチの設定
				UpdateDeltaZMax();

				//最大ヘリカルピッチの設定
				UpdateZpMax();

				//画素を固定し、スライスピッチ（mm）を更新
				if (myConeSlicePitchPix != null)
				{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
					With cwneDelta_z
						.Value = Val(Format$(myConeSlicePitchPix * SWMinCone, .FormatString))
					End With
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

					decimal value = 0;
					decimal.TryParse(((double)myConeSlicePitchPix * SWMinCone).ToString(string.Format("F{0}", cwneDelta_z.DecimalPlaces)), out value);
                    
                    //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                    //cwneDelta_z.Value = value;
                    if ((cwneDelta_z.Maximum >= value) && (cwneDelta_z.Minimum <= value))
                    {   
                        //代入時のイベントロック
                        //DeltazEvntLock = true;    
                        cwneDelta_z.Value = value;
                        //DeltazEvntLock = false;   
                    }
                    //追加2015/01/29hata
                    else if (cwneDelta_z.Maximum < value)
                    {
                        cwneDelta_z.Value = cwneDelta_z.Maximum;
                    }
                    else if (cwneDelta_z.Minimum > value)
                    {
                        cwneDelta_z.Value = cwneDelta_z.Minimum;
                    }
                }
			}
			else
			{
				//スライス厚最小
                SWMin = modLibrary.MaxVal(FCDFIDRate * CTSettings.scancondpar.Data.mdtpitch[2] * CTSettings.detectorParam.vm / CTSettings.detectorParam.hm, 0.001F);
                

				//最大・最小スライス厚の更新
				UpdateSliceMinMax();
			}

			//スライス厚更新
			if ((float)cwneSlice.Minimum != OldSWMin)
			{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				cwneSlicePix_ValueChanged cwneSlicePix.Value, cwneSlicePix.Value, False
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
				cwneSlicePix_ValueChanged(cwneSlicePix, EventArgs.Empty);
			}
		}


		//*******************************************************************************
		//機　　能： アーティファクト低減チェックボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void chkArtifactReduction_CheckStateChanged(object sender, EventArgs e)
		{
			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == chkArtifactReduction) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： 透視画像保存チェックボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void chkFluoroImageSave_CheckStateChanged(object sender, EventArgs e)
		{
			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == chkFluoroImageSave) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： メール送信チェックボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void chkSendMail_CheckStateChanged(object sender, EventArgs e)
		{
			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == chkSendMail) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： フラットパネル設定（ゲイン）コンボボックスクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v17.00  2001/02/17  やまおか    新規作成
		//*******************************************************************************
		private void cmbGain_SelectedIndexChanged(object sender, EventArgs e)
		{
			//ＯＫボタンを使用可にする '追加　山本 2010-01-27
			if (this.ActiveControl == cmbGain) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： フラットパネル設定（積分時間）コンボボックスクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v17.00  2001/02/17  やまおか    新規作成
		//*******************************************************************************
		private void cmbInteg_SelectedIndexChanged(object sender, EventArgs e)
		{
            //変更2014/10/07hata_v19.51反映
            //v19.17 修正 by長野 2013/09/13
            //v19.01 追加 by長野 2012/05/21
            //if (CTSettings.scaninh.Data.smooth_rot_cone == 0 && CTSettings.scansel.Data.table_rotation == 1)
			if (optTableRotation[1].Checked == true)
			{
				CreateViewListComboBox();
			}

			//ＯＫボタンを使用可にする '追加　山本 2010-01-27
			if (this.ActiveControl == cmbInteg) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： スキャンエリア変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cwneArea_ValueChanged(object sender, EventArgs e)
		{
            //if (AreaEvntLock) return;   //代入時のイベントロック 追加2014/06/23(検S1)hata
            if (EvntLock) return;   //Load時のイベントロック 追加2014/06/23(検S1)hata

            //変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == cwneArea) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： 画像回転角度変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cwneImageRotateAngle_ValueChanged(object sender, EventArgs e)
		{
            //if (ImgRotAnglEvntLock) return;   //代入時のイベントロック 追加2014/06/23(検S1)hata
            if (EvntLock) return;   //Load時のイベントロック 追加2014/06/23(検S1)hata
           
            //変更した場合，ＯＫボタンを使用可にする
			cmdOK.Enabled = true;
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
			//ＦＣＤ・ＦＩＤ値変更時処理
			UpdateFcdFid();
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
			//ＦＣＤ・ＦＩＤ値変更時処理
			UpdateFcdFid();
		}


		//*******************************************************************************
		//機　　能： メカ画面からの昇降位置変更時イベント処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void myMechaControl_UDPosChanged(object sender, EventArgs e)
		{
			udPos = (float)frmMechaControl.Instance.ntbUpDown.Value;

			//マルチスキャンのスライス数上下限値を設定
			SetSliceNumRange();

			//マルチスキャンのスキャンピッチ上下限値を設定
			SetScanPitchRange();

			if (IsCone)
			{
				//ヘリカル開始位置 (mm)
				//    構造体名：mecainf
				//    コモン名：udab_pos

                //変更2014/06/18(検S1)hata
                // 最大/最小を超える場合は最大/最小を合わせる
                //cwneZdas.Value = (decimal)udPos;
                if ((cwneZdas.Maximum >= (decimal)udPos) && (cwneZdas.Minimum <= (decimal)udPos))
                {
                    cwneZdas.Value = (decimal)udPos;
                }
                //追加2015/01/29hata
                else if(cwneZdas.Maximum < (decimal)udPos)
                {
                    cwneZdas.Value = cwneZdas.Maximum;
                }
                else if(cwneZdas.Minimum > (decimal)udPos)
                {
                    cwneZdas.Value = cwneZdas.Minimum;
                }
			}
		}


		//********************************************************************************
		//機    能  ：  最大・最小スライス厚の再計算と表示を行う
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：
		//
		//履    歴  ：  V3.0   00/08/09  (SI1)鈴山       新規作成
		//********************************************************************************
		private void UpdateSliceMinMax()
		{
			float theMax = 0;			//最大スライス厚(mm)
			float theMaxPix = 0;		//最大スライス厚(画素)
			float theMin = 0;			//最小スライス厚(mm)小数点第４位以下を必ず切り上げ 'v19.00 追加 by長野 2012-04-05

			//コーンの場合は別途 'v15.0追加 2009/04/03
			if (IsCone) return;

			//マルチスライスの枚数不定の場合、何もしない
			int MultiSliceIndex = 0;
			MultiSliceIndex = modLibrary.GetOption(optMultislice);
			if (MultiSliceIndex == -1) return;

			//ビニングが不定の場合、何もしない
			if (modLibrary.GetOption(optBinning) == -1) return;

            //最大スライス厚計算(mm)
            //2014/11/07hata キャストの修正
            //theMax = ScanCorrect.GetSWMax((short)ntbTransImageHeight.Value,
            //                              (short)ntbTransImageWidth.Value,
            //                              CTSettings.scancondpar.Data.scan_posi_a[2],
            //                              CTSettings.scancondpar.Data.scan_posi_b[2], 
            //                              SWMin,
            //                              (float)ntbMultislicePitch.Value,
            //                              (short)MultiSliceIndex);
            theMax = ScanCorrect.GetSWMax(Convert.ToInt16(ntbTransImageHeight.Value),
                                          Convert.ToInt16(ntbTransImageWidth.Value),
                                          CTSettings.scancondpar.Data.scan_posi_a[2],
                                          CTSettings.scancondpar.Data.scan_posi_b[2],
                                          SWMin,
                                          (float)ntbMultislicePitch.Value,
                                          Convert.ToInt16(MultiSliceIndex));
        
            //画素に変換：画素単位で100を越えた場合は、100で抑える
			theMaxPix = modLibrary.MinVal(theMax / SWMin, 100);

			//mmに変換して返す
			theMax = theMaxPix * SWMin;

			//表示
#region 【C#コントロールで代用】
/*
			With cwneSlice
				'最小スライス厚を小数点第４以下で四捨五入すると理論値以下を設定できるように
				'なるため、必ず切り上げるようにする by長野 2012-04-03 v19.00
				theMin = Val(Format$(MaxVal(SWMin, 0.001) + 0.0009999, .FormatString))

				'最小値・最大値の設定
				'SWMinではなく、小数点第４以下で切り上げたtheMinを使う by長野 2012-04-05
				'.SetMinMax Val(Format$(SWMin, .FormatString)), Val(Format$(theMax, .FormatString))
				.SetMinMax Val(Format$(theMin, .FormatString)), Val(Format$(theMax, .FormatString))

				'最小値・最大値の表示
				lblSliceMinMax.Caption = GetResString(IDS_RangeMM, _
													  Format$(.Minimum, .FormatString), _
													  Format$(.Maximum, .FormatString))
			End With
*/
#endregion

			string cwneSliceFormat = string.Format("F{0}", cwneSlice.DecimalPlaces);

            //最小スライス厚を小数点第４以下で四捨五入すると理論値以下を設定できるように
			//なるため、必ず切り上げるようにする by長野 2012-04-03 v19.00
            //変更2015/01/20hata_計算が合わなくなるので切り上げしない
            //float.TryParse((modLibrary.MaxVal(SWMin, 0.001) + 0.0009999).ToString(cwneSliceFormat), out theMin);
            //float.TryParse((modLibrary.MaxVal(SWMin, 0.001)).ToString(cwneSliceFormat), out theMin);
            
            //Rev23.00 変更 最小は切り上げないと1画素未満になってしまう by長野 2015/09/14
            theMin = (float)(modLibrary.MaxVal(SWMin, 0.001));
            float tmp_theMin = (float)Math.Ceiling(theMin * 1000.0);
            theMin = (float)(tmp_theMin / 1000.0);

			//最小値・最大値の設定
			//SWMinではなく、小数点第４以下で切り上げたtheMinを使う by長野 2012-04-05
			//.SetMinMax Val(Format$(SWMin, .FormatString)), Val(Format$(theMax, .FormatString))
			decimal theMinValue = 0;
			decimal theMaxValue = 0;
			decimal.TryParse(theMin.ToString(cwneSliceFormat), out theMinValue);
			decimal.TryParse(theMax.ToString(cwneSliceFormat), out theMaxValue);
			cwneSlice.Minimum = theMinValue;
			cwneSlice.Maximum = theMaxValue;

  			//最小値・最大値の表示
			lblSliceMinMax.Text = StringTable.GetResString(StringTable.IDS_RangeMM, 
														   cwneSlice.Minimum.ToString(cwneSliceFormat), 
														   cwneSlice.Maximum.ToString(cwneSliceFormat));
		}


		//********************************************************************************
		//機    能  ：  スキャン条件の設定内容をチェック（その１）
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V2.0   00/02/08  (SI1)鈴山       新規作成
		//********************************************************************************
		private bool OptValueChk1()
		{
			string FileName = null;
			//string driveName = null;
            int sft_mode = 0; //追加2014/10/07hata_v19.51反映
            int lr_sft_mode = 0; //Rev23.20 追加 by長野 2016/01/23

			//返り値の初期化
			bool functionReturnValue = false;

            //Rev23.10

			//v19.01 追加
			if (cmbViewNum.SelectedIndex < 0)
			{
				MessageBox.Show(CTResources.LoadResString(21311), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return functionReturnValue;
			}


			//スライスプラン時のチェック
			if (optMultiScanMode5.Checked)
			{
				FileName = Path.Combine(txtSlicePlanDir.Text, txtSlicePlanName.Text);

				//スライスプランテーブルが指定されていない場合
				if (string.IsNullOrEmpty(FileName))
				{
					//メッセージ表示：スライスプランテーブルが指定されていません。
					MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_NotSpecified, StringTable.IDS_SlicePlanTable), 
									Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return functionReturnValue;
				}
				//スライスプランテーブルが存在しない場合
				else if (!File.Exists(FileName))
				{
					//メッセージ表示：スライスプランテーブルが見つかりません。
					MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_NotFound, StringTable.IDS_SlicePlanTable), 
									Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return functionReturnValue;
				}
			}

			//オートズーミング時のチェック
			//If chkZooming.Value = vbChecked Then
			//v14.0変更 by 間々田 2007/08/20
			if ((chkZooming.CheckState == CheckState.Checked) && chkZooming.Visible)
			{
				//FileName = Path.Combine(txtZoomFileName.Text, txtZoomFileName.Text);
                //Rev20.00 修正 by長野 2014/12/04
                FileName = Path.Combine(txtZoomDirName.Text, txtZoomFileName.Text);

				//ズーミングテーブルが指定されていない場合
				if (string.IsNullOrEmpty(FileName))
				{
					//メッセージ表示：ズーミングテーブルが指定されていません。
					MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_NotSpecified, StringTable.IDS_ZoomingTable), 
									Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return functionReturnValue;
				}
				//ズーミングテーブルが存在しない場合
				else if (!File.Exists(FileName))
				{
					//メッセージ表示：ズーミングテーブルが見つかりません。
					MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_NotFound, StringTable.IDS_ZoomingTable), 
									Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return functionReturnValue;
				}
			}

			//メール送信が設定されている場合 v9.1 added by 間々田 2004/05/13
			if (fraSendMail.Visible && (chkSendMail.CheckState == CheckState.Checked))
			{
				//SMTPサーバ名・送信者・宛先のいずれかが未設定の場合
                if ((string.IsNullOrEmpty(modLibrary.RemoveNull(CTSettings.scansel.Data.smtp_server.GetString()))) ||
                    (string.IsNullOrEmpty(modLibrary.RemoveNull(CTSettings.scansel.Data.transmitting_person.GetString()))) ||
                    (string.IsNullOrEmpty(modLibrary.RemoveNull(CTSettings.scansel.Data.address.GetString()))))
				{
					//メッセージ表示：メール送信に必要な項目（SMTPサーバ名・送信者・宛先）を設定してください。
					MessageBox.Show(CTResources.LoadResString(13008), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return functionReturnValue;
				}
			}


			if (IsCone)
			{
				//double HelicalRawSize = 0;		//added by 山本　2002-12-21
				//double MaxFileS = 0;			//added by 山本　2002-12-21
				//int MaxViewS = 0;				//added by 山本　2002-12-21

				//MaxFileS = 2147483645;			//added by 山本　2002-12-21 'changed by 山本　2003-9-27　余裕をとるためマイナス２した


				//Δmsw計算
				//    delta_msw = sw * (FGD / FCDm) * (B1 / 10) * (Sqr(1 + a * a) / v_mag)
				delta_msw = (float)cwneSlice.Value / SWMinCone;								//v7.0 FPD対応 by 間々田

                //シフトスキャン？   'v18.00追加 byやまおか 2011/07/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                //sft_mode = (ScanCorrect.IsShiftScan() ? 1 : 0);
                //Rev25.00 Wスキャンを条件に追加 by長野 2016/08/10
                sft_mode = ((ScanCorrect.IsShiftScan() || ScanCorrect.IsW_Scan())  ? 1 : 0);

                //左右シフトスキャンかどうか Rev23.20 by長野 2016/01/23
                //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                if ((CTSettings.scaninh.Data.lr_sft == 0 && optScanMode[4].Checked == true) || (CTSettings.W_ScanOn && chkW_Scan.Checked == true))
                {
                    lr_sft_mode = 1;
                }
                else
                {
                    lr_sft_mode = 0;
                }
    
				//mc計算
				if (optHelical[0].Checked)
				{
                    //2014/11/07hata キャストの修正
                    mc = (int)(((float)cwneDelta_z.Value * ((float)cwneK.Value - 1) / SWMinCone / ksw + delta_msw) / 2) + 2;
                  
                    //変更2014/10/07hata_v19.51反映
                    //if (mc > mc_max) mc = mc_max;
                    //v18.00シフト対応 byやまおか 2011/07/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    if (mc > mc_max[sft_mode])
                    {
                        mc = mc_max[sft_mode];
                    }

                }
				else
				{
					//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''

					//            mc = Int((cwneZp.Value * (1 + scancondpar.Alpha / Pai) / SWMinCone / ksw + delta_msw) / 2) + 2
					//
					//            'ヘリカルスキャン時の生データ量を計算し、2GBを超える場合は警告する  added by 山本　2002-12-21
					//            HelicalRawSize = (CDbl(2) * CDbl(mc) + CDbl(1)) * CDbl(2) * CDbl(scancondpar.fimage_hsize) * Val(lblAcqView.Caption)
					//            If HelicalRawSize > MaxFileS Then
					//                MaxViewS = MaxFileS / (CDbl(2) * CDbl(mc) + CDbl(1)) / CDbl(2) / CDbl(scancondpar.fimage_hsize)
					//    '            MaxViewS = Int(MaxViewS / CDbl(100)) * CDbl(100)      'deleted by 山本　2003-9-27　データ収集ビュー数は100単位にする必要なし
					//
					//                'メッセージ表示：
					//                '   生データサイズが2GBを超えるため、データ収集ビュー数が MaxViewS 以下になるように
					//                '   コーンビームスキャン条件で設定してください。スキャン中にエラーする恐れがあります    '変更 by 間々田 2009/08/22 リソースから「コーンビームスキャン条件で」の部分を削除
					//                MsgBox GetResString(9593, CStr(MaxViewS)), vbCritical
					//                Exit Function        '生データサイズが2GBを越える場合は抜ける　'added by 山本　2003-9-27
					//            End If
					//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

					//        Debug.Print "ヘリカル生データサイズ = "; HelicalRawSize     'added by 山本　2003-9-27
				}

				//v7.0 append by 間々田 2003/09/09 Start

				//ﾃｰﾌﾞﾙ回転
				//    構造体名：scansel
				//    コモン名：table_rotation
				//    コモン値：0(ｽﾃｯﾌﾟ),1(連続)

				float the_ic = 0;
				float the_jc = 0;
				float the_theta_s = 0;
				float the_dpm = 0;
				int nstart = 0;
				int nend = 0;
				int mstart = 0;
				int mend = 0;
				float the_delta_theta = 0;
				float the_n0 = 0;
				float the_m0 = 0;
				float delta_im = 0;
				float delta_jm = 0;
				float ir0 = 0;
				float jr0 = 0;
				float kix = 0;
				float kjx = 0;
				int js = 0;
				int je = 0;
				float delta_ix = 0;
				float delta_jx = 0;

                //Rev23.10 追加 by長野 2015/10/06
                int tmp_h_size = 0;
                //Rev25.00 Wスキャンの場合を追加 by長野 2016/08/19
                //if (optScanMode[4].Checked == true)
                if (optScanMode[4].Checked == true || chkW_Scan.Checked == true)
                {
                    tmp_h_size = CTSettings.scancondpar.Data.fimage_hsize + CTSettings.scancondpar.Data.det_sft_pix;
                }
                else
                {
                    tmp_h_size = CTSettings.scancondpar.Data.fimage_hsize;
                }

				//ステップ回転の場合
				//If optTableRotation(0).value Then
                //If True Then                            '連続回転の場合の計算でエラーになるのでとりあえず、こうする by 間々田 2009/06/30
                #region　ステップ/連続　条件分けしない
                //条件分けしない
                //if (optTableRotation[0].Checked)			//v16.?? 戻す
                //{
                //    //１ビューあたりの生データサイズ
                //    ScanCorrect.ConeRawSize = CTSettings.scancondpar.Data.fimage_hsize * (2 * mc + 1) * 2 / 1024;
                //}
                ////連続回転の場合
                //else
                //{

					hizumi = new float[2];			//v16.2 cone_setup_iicrctでのアクセスエラー回避用 by 山影 2010/01/19

					//js, je の算出
					//Call cone_setup_iicrct(the_ic, the_jc, the_theta_s, the_dpm, nstart, nend, mstart, mend, the_delta_theta, the_n0, the_m0, _
					//'                       delta_im, delta_jm, ir0, jr0, kix, kjx, B1, a, b, h, v, FGD, h, mc, hizumi(0), js, je, delta_ix, delta_jx, hm, vm, 0)
                    
                    ////v11.2以下に変更 by 間々田 2005/10/11
                    ////CTAPI.CTLib.cone_setup_iicrct(ref the_ic, ref the_jc, ref the_theta_s, ref the_dpm, ref nstart, ref nend, ref mstart, ref mend, 
                    ////							 ref the_delta_theta, ref the_n0, ref the_m0, ref delta_im, ref delta_jm, ref ir0, ref jr0, ref kix, ref kjx,
                    ////                           CTSettings.scancondpar.Data.b[1], CTSettings.scancondpar.Data.scan_posi_a[2], CTSettings.scancondpar.Data.scan_posi_b[2],
                    ////                             CTSettings.scancondpar.Data.fimage_hsize, CTSettings.scancondpar.Data.fimage_vsize,
                    ////                             FIDWithOffset, CTSettings.scancondpar.Data.fimage_hsize, (2 * mc + 1), mc, ref hizumi[0], ref js, ref je,
                    ////                            ref delta_ix, ref delta_jx, CTSettings.detectorParam.hm, CTSettings.detectorParam.vm, 0,CTSettings.scaninh.Data.full_distortion,
                    ////                             CTSettings.scancondpar.Data.ist, CTSettings.scancondpar.Data.ied, CTSettings.scancondpar.Data.detector);			//v17.00 引数detector追加 byやまおか 2010/02/26
                    //// Rev20.00 scan_posi_a[2]とscan_posi_b[2]をそれぞれコーン用に変更 by長野 2014/07/17
                    //CTAPI.CTLib.cone_setup_iicrct(ref the_ic, ref the_jc, ref the_theta_s, ref the_dpm, ref nstart, ref nend, ref mstart, ref mend, 
                    //                             ref the_delta_theta, ref the_n0, ref the_m0, ref delta_im, ref delta_jm, ref ir0, ref jr0, ref kix, ref kjx,
                    //                             CTSettings.scancondpar.Data.b[1], CTSettings.scancondpar.Data.cone_scan_posi_a, CTSettings.scancondpar.Data.cone_scan_posi_b,
                    //                             CTSettings.scancondpar.Data.fimage_hsize, CTSettings.scancondpar.Data.fimage_vsize,
                    //                             FIDWithOffset, CTSettings.scancondpar.Data.fimage_hsize, (2 * mc + 1), mc, ref hizumi[0], ref js, ref je,
                    //                             ref delta_ix, ref delta_jx, CTSettings.detectorParam.hm, CTSettings.detectorParam.vm, 0,CTSettings.scaninh.Data.full_distortion,
                    //                             CTSettings.scancondpar.Data.ist, CTSettings.scancondpar.Data.ied, CTSettings.scancondpar.Data.detector);			//v17.00 引数detector追加 byやまおか 2010/02/26
                    
                    //CTAPI.CTLib.cone_setup_iicrct(ref the_ic, ref the_jc, ref the_theta_s, ref the_dpm, ref nstart, ref nend, ref mstart, ref mend,
                    //             ref the_delta_theta, ref the_n0, ref the_m0, ref delta_im, ref delta_jm, ref ir0, ref jr0, ref kix, ref kjx,
                    //             CTSettings.scancondpar.Data.b[1], CTSettings.scancondpar.Data.cone_scan_posi_a, CTSettings.scancondpar.Data.cone_scan_posi_b,
                    //             CTSettings.scancondpar.Data.fimage_hsize, CTSettings.scancondpar.Data.fimage_vsize,
                    //             //FIDWithOffset, CTSettings.scancondpar.Data.fimage_hsize, (2 * mc + 1), mc, ref hizumi[0], ref js, ref je,
                    //             FIDWithOffset, tmp_h_size, (2 * mc + 1), mc, ref hizumi[0], ref js, ref je,
                    //             ref delta_ix, ref delta_jx, CTSettings.detectorParam.hm, CTSettings.detectorParam.vm, 0, CTSettings.scaninh.Data.full_distortion,
                    //             CTSettings.scancondpar.Data.ist, CTSettings.scancondpar.Data.ied, CTSettings.scancondpar.Data.detector,
                    //             CTSettings.scansel.Data.scan_mode); //'v17.00 引数detector追加 byやまおか 2010/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/12

                    //CTAPI.CTLib.cone_setup_iicrct(ref the_ic, ref the_jc, ref the_theta_s, ref the_dpm, ref nstart, ref nend, ref mstart, ref mend,
                    //             ref the_delta_theta, ref the_n0, ref the_m0, ref delta_im, ref delta_jm, ref ir0, ref jr0, ref kix, ref kjx,
                    //             CTSettings.scancondpar.Data.b[1], CTSettings.scancondpar.Data.cone_scan_posi_a, modScanCondition.real_cone_scan_posi_b,//Rev23.20 引数変更 by長野 2015/11/20
                    //             CTSettings.scancondpar.Data.fimage_hsize, CTSettings.scancondpar.Data.fimage_vsize,
                    //                    //FIDWithOffset, CTSettings.scancondpar.Data.fimage_hsize, (2 * mc + 1), mc, ref hizumi[0], ref js, ref je,
                    //             FIDWithOffset, tmp_h_size, (2 * mc + 1), mc, ref hizumi[0], ref js, ref je,
                    //             ref delta_ix, ref delta_jx, CTSettings.detectorParam.hm, CTSettings.detectorParam.vm, 0, CTSettings.scaninh.Data.full_distortion,
                    //             CTSettings.scancondpar.Data.ist, CTSettings.scancondpar.Data.ied, CTSettings.scancondpar.Data.detector,
                    //             CTSettings.scansel.Data.scan_mode); //'v17.00 引数detector追加 byやまおか 2010/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/12

                    CTAPI.CTLib.cone_setup_iicrct(ref the_ic, ref the_jc, ref the_theta_s, ref the_dpm, ref nstart, ref nend, ref mstart, ref mend,
                                 ref the_delta_theta, ref the_n0, ref the_m0, ref delta_im, ref delta_jm, ref ir0, ref jr0, ref kix, ref kjx,
                                 CTSettings.scancondpar.Data.b[1], CTSettings.scancondpar.Data.cone_scan_posi_a, modScanCondition.real_cone_scan_posi_b,//Rev23.20 引数変更 by長野 2015/11/20
                                 CTSettings.scancondpar.Data.fimage_hsize, CTSettings.scancondpar.Data.fimage_vsize,
                                        //FIDWithOffset, CTSettings.scancondpar.Data.fimage_hsize, (2 * mc + 1), mc, ref hizumi[0], ref js, ref je,
                                 FIDWithOffset, tmp_h_size, (2 * mc + 1), mc, ref hizumi[0], ref js, ref je,
                                 ref delta_ix, ref delta_jx, CTSettings.detectorParam.hm, CTSettings.detectorParam.vm, 0, CTSettings.scaninh.Data.full_distortion,
                                 CTSettings.scancondpar.Data.ist, CTSettings.scancondpar.Data.ied, CTSettings.scancondpar.Data.detector,
                                 CTSettings.scansel.Data.scan_mode,lr_sft_mode,CTSettings.scancondpar.Data.det_sft_pix_r,CTSettings.scancondpar.Data.det_sft_pix_l); //'23.20 引数追加 by長野 2016/01/22

                
                //１ビューあたりの生データサイズ
                    //2014/11/07hata キャストの修正
                    //ScanCorrect.ConeRawSize = CTSettings.scancondpar.Data.fimage_hsize * (je - js + 1) * 2 / 1024;
                    ScanCorrect.ConeRawSize = CTSettings.scancondpar.Data.fimage_hsize * (je - js + 1) * 2 / 1024F;

                //追加2014/07/08(検S1)hata
                    modScanCondition.ScanJs = js;
                    modScanCondition.ScanJe = je;

                //}

                //ここは不要     変更2014/10/07hata_v19.51反映
                //Rev20.00 イキ スキャンスタート後に移す by長野 2014/12/15
                //Rev20.00 やはり不要 by長野 2014/12/15
                //v19.17 連続時の純生データサイズが容量オーバーの場合 2013/09/13 by長野
                //if ((IsCone) & (optTableRotation[1].Checked == true))
                //{
                //    long AllRawSize = 0;
                //    long AllMemSizeKB = 0;
                //    AllRawSize = (long)(ScanCorrect.ConeRawSize * (float)cwneViewNum.Value);
                //    AllMemSizeKB = (long)((float)CTSettings.iniValue.SharedMemSize * 1024.0f * 0.9f);
                //    //余裕をもつ
                //    if ((AllMemSizeKB < AllRawSize))
                //    {
                //        MessageBox.Show(CTResources.LoadResString(21313), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return functionReturnValue;
                //    }
                //}


                #endregion
            }

			functionReturnValue = true;
			return functionReturnValue;
		}


		//********************************************************************************
		//機    能  ：  スキャン条件の設定内容をチェック（その２）
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V4.0   01/02/13  (SI1)鈴山       新規作成
		//********************************************************************************
		private bool OptValueChk2()
		{
			//戻り値を初期化
			bool functionReturnValue = false;

			//v17.65 ScanOptValueChk2okをTrueで初期化 by(検S1)長野 2011/11/26
			ScanOptValueChk2ok = true;

			//回転中心画素
			//    構造体名：scancondpar
			//    コモン名：xlc[5]

			//透視画像サイズ(横)
			//    構造体名：scancondpar
			//    コモン名：fimage_hsize


			//オートセンタリングなしの場合
			if (optAutoCentering0.Checked)
			{
				float rXlc = 0;

                //Rev25.00 Wスキャンに対応させる by長野 2016/08/08
                if (CTSettings.scansel.Data.w_scan == 1 || chkW_Scan.Checked == true)
                {
                    rXlc = (IsCone ? CTSettings.scancondpar.Data.nc : CTSettings.scancondpar.Data.xlc[2]) / (CTSettings.detectorParam.h_size + CTSettings.scancondpar.Data.det_sft_pix);
                }
                else
                {
                    //rXlc = (IsCone ? CTSettings.scancondpar.Data.nc : CTSettings.scancondpar.Data.xlc[2]) / CTSettings.scancondpar.Data.fimage_hsize;
                    //Rev25.00 fimageじゃなくてdetectorParamで確認 by長野 2016/08/08 
                    rXlc = (IsCone ? CTSettings.scancondpar.Data.nc : CTSettings.scancondpar.Data.xlc[2]) / CTSettings.detectorParam.h_size;
                }

				if (rXlc == 0)
				{
					//エラーメッセージ表示：
					//   コモン初期化後回転中心校正を行っていません。
					//   回転中心校正を行ってからスキャンしてください。
					//MsgBox LoadResString(9400), vbExclamation
                    if (this.Visible && this.WindowState != FormWindowState.Minimized)
                    {
                        MessageBox.Show(CTResources.LoadResString(9400), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    return functionReturnValue;
				}
				//変更 by 間々田 2009/08/17 スキャン条件画面を表示している時だけ表示
				//else if ((optScanMode[(int)ScanSel.ScanModeConstants.ScanModeHalf].Checked || optScanMode[2].Checked) && (rXlc < 0.3 || rXlc > 0.7))
				//Rev25.00 ScanModeConstantsを使うように変更 by長野 2016/08/08
                else if ((optScanMode[(int)ScanSel.ScanModeConstants.ScanModeHalf].Checked || optScanMode[(int)ScanSel.ScanModeConstants.ScanModeFull].Checked) && (rXlc < 0.3 || rXlc > 0.7))
                {
					//起動時に回転中心が端に寄っているとエラーになるため仮対策 v16.03/v16.20 追加 by 山影 10-03-12
					if (modCT30K.CheckRC)
					{
						//エラーメッセージ表示：
						//   回転中心位置が左右の端に寄っているため、スキャンエリアが小さくなっていて、画像がぼけたように表示されます。
						//   ハーフスキャンまたはフルスキャンを行う場合は、回転中心位置が画面の中心になるように回転中心校正をやり直してください。
						//   回転中心画素値が 透視画像横サイズのほぼ1/2 となるように調整してください。
						MessageBox.Show(CTResources.LoadResString(9463), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						//v17.65 スキャンスタート時では、スキャンを続行するかどうかのメッセージを出す。by(検S1)長野 2011/11/26
						if ((frmScanControl.Instance.ctbtnScanStart.Enabled == false) && (frmXrayControl.Instance.XrayStatus != StringTable.GC_Xray_WarmUp))
						{
							DialogResult result = MessageBox.Show(CTResources.LoadResString(20053) + CTResources.LoadResString(20188), 
																  Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
							if (result == DialogResult.OK)
							{
								ScanOptValueChk2ok = true;
							}
							else
							{
								ScanOptValueChk2ok = false;
							}
						}
						return functionReturnValue;
					}
				}
			}
			functionReturnValue = true;
			return functionReturnValue;
		}


		//*************************************************************************************************
		//機　　能： scansel（コモン）に基づいて各コントロールに値をセットする
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  XX/XX/XX  (SI3)鈴山      新規作成
		//           v7.0   03/09/26  (SI4)間々田    ビニング対応
		//           v8.0   03/11/21  (SI4)間々田    コーン分散処理/コーンビームのスライスプラン対応
		//           v9.0   04/01/30  (SI4)間々田    オーバースキャン/往復スキャン/回転選択対応
		//           v9.1   04/05/13  (SI4)間々田    メール送信対応
		//           v9.3   04/06/30  (SI4)間々田    Ｘ線休止スキャン対応
		//           v9.7   04/12/08  (SI4)間々田    アーティファクト低減対応
		//*************************************************************************************************
		private void SetControls()
        {
            string FileName = null;
            string FileName2 = null; //Rev20.00 追加 by長野 2014/12/04

            //生データ保存ありか？
            IsRawDataSave = (CTSettings.scansel.Data.rawdata_save == 1);

            //マルチスキャンモード：1(シングル), 3(マルチ), 5(スライスプラン)
            modLibrary.SetOption(optMultiScanMode, CTSettings.scansel.Data.multiscan_mode, (int)ScanSel.MultiScanModeConstants.MultiScanModeSingle);

            //マルチスキャン
            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneMSPitch.Value = (decimal)CTSettings.scansel.Data.pitch;				//スキャンピッチ
            if ((cwneMSPitch.Maximum >= (decimal)CTSettings.scansel.Data.pitch) && (cwneMSPitch.Minimum <= (decimal)CTSettings.scansel.Data.pitch))
            {
                //代入時のイベントロック
                //MSPitchEvntLock = true;
                cwneMSPitch.Value = (decimal)CTSettings.scansel.Data.pitch;				//スキャンピッチ
                //MSPitchEvntLock = false;
            }
            //追加2015/01/29hata
            else if (cwneMSPitch.Maximum < (decimal)CTSettings.scansel.Data.pitch)
            {
                cwneMSPitch.Value = cwneMSPitch.Maximum;
            }
            else if (cwneMSPitch.Minimum > (decimal)CTSettings.scansel.Data.pitch)
            {
                cwneMSPitch.Value = cwneMSPitch.Minimum;
            }

            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneMSSlice.Value = (decimal)CTSettings.scansel.Data.multinum;			//スライス数
            if ((cwneMSSlice.Maximum >= (decimal)CTSettings.scansel.Data.multinum) && (cwneMSSlice.Minimum <= (decimal)CTSettings.scansel.Data.multinum))
            {
                //代入時のイベントロック
                //MSSliceEvntLock = true;
                cwneMSSlice.Value = (decimal)CTSettings.scansel.Data.multinum;			//スライス数
                //MSSliceEvntLock = false;
            }
            //追加2015/01/29hata
            else if (cwneMSSlice.Maximum < (decimal)CTSettings.scansel.Data.multinum)
            {
                cwneMSSlice.Value = cwneMSSlice.Maximum;
            }
            else if (cwneMSSlice.Minimum > (decimal)CTSettings.scansel.Data.multinum)
            {
                cwneMSSlice.Value = cwneMSSlice.Minimum;
            }

            //現在の昇降位置を取得する：
            //   マルチスキャンのスキャンピッチ・スライス数の最大値・最小値がここでセットされる
            myMechaControl_UDPosChanged(this, EventArgs.Empty);

            if (IsCone)
            {
                FileName = Path.Combine(CTSettings.scansel.Data.cone_sliceplan_dir.GetString(), CTSettings.scansel.Data.cone_slice_plan.GetString());
            }
            else
            {
                FileName = Path.Combine(CTSettings.scansel.Data.sliceplan_dir.GetString(), CTSettings.scansel.Data.slice_plan.GetString());
            }

            //拡張子を付加
            FileName = modLibrary.AddExtension(FileName, ".csv");

            //スライスプランテーブル
            txtSlicePlanDir.Text = string.IsNullOrEmpty(FileName) ? "" : Path.GetDirectoryName(FileName);
            txtSlicePlanName.Text = string.IsNullOrEmpty(FileName) ? "" : Path.GetFileName(FileName);

            //マトリクスサイズ：2(512x512), 3(1024x1024), 4(2048x2048)
            modLibrary.SetOption(optMatrix, CTSettings.scansel.Data.matrix_size, (int)ScanSel.MatrixSizeConstants.MatrixSize512);

            //ビュー数設定値
            cwneViewNum.Minimum = (decimal)CTSettings.GVal_ViewMin;
            cwneViewNum.Maximum = (decimal)CTSettings.GVal_ViewMax;
            //cwneViewNum.Maximum = GVal_ViewMax

            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneViewNum.Value = (decimal)CTSettings.scansel.Data.scan_view;
            if ((cwneViewNum.Maximum >= (decimal)CTSettings.scansel.Data.scan_view) && (cwneViewNum.Minimum <= (decimal)CTSettings.scansel.Data.scan_view))
            {
                //代入時のイベントロック
                //ViewNumEvntLock = true;
                cwneViewNum.Value = (decimal)CTSettings.scansel.Data.scan_view;
                //ViewNumEvntLock = false;
            }
            //追加2015/01/29hata
            else if (cwneViewNum.Maximum < (decimal)CTSettings.scansel.Data.scan_view)
            {
                cwneViewNum.Value = cwneViewNum.Maximum;
            }
            else if (cwneViewNum.Minimum > (decimal)CTSettings.scansel.Data.scan_view)
            {
                cwneViewNum.Value = cwneViewNum.Minimum;
            }

            //画像積算枚数   最小 infdef.min_integ_number 最大 infdef.max_integ_number
            cwneInteg.Minimum = (decimal)CTSettings.GValIntegNumMin;
            cwneInteg.Maximum = (decimal)CTSettings.GValIntegNumMax;
            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneInteg.Value = (decimal)CTSettings.scansel.Data.scan_integ_number;
            if ((cwneInteg.Maximum >= (decimal)CTSettings.scansel.Data.scan_integ_number) && (cwneInteg.Minimum <= (decimal)CTSettings.scansel.Data.scan_integ_number))
            {
                //代入時のイベントロック
                //IntegEvntLock = true;
                cwneInteg.Value = (decimal)CTSettings.scansel.Data.scan_integ_number;
                //IntegEvntLock = false;
            }
            //追加2015/01/29hata
            else if (cwneInteg.Maximum < (decimal)CTSettings.scansel.Data.scan_integ_number)
            {
                cwneInteg.Value = cwneInteg.Maximum;
            }
            else if (cwneInteg.Minimum > (decimal)CTSettings.scansel.Data.scan_integ_number)
            {
                cwneInteg.Value = cwneInteg.Minimum;
            }

            //スキャンモード：1(ハーフ), 2(フル), 3(オフセット)
            modLibrary.SetOption(optScanMode, CTSettings.scansel.Data.scan_mode);

            //Rev25.00 Wスキャン追加 by長野 2016/08/03
            chkW_Scan.CheckState = CTSettings.scansel.Data.w_scan == 1 ? CheckState.Checked : CheckState.Unchecked;

            //'スキャンエリア                                            'v15.02削除 下に移動 by 間々田 2009/09/14
            //cwneArea.Value = IIf(IsCone, .cone_scan_area, .mscan_area)

            //バイアス
            ntbBias.Value = (decimal)CTSettings.scansel.Data.mscan_bias;

            //スロープ
            ntbSlope.Value = (decimal)CTSettings.scansel.Data.mscan_slope;

            //フィルタ関数：1(FC1),2(FC2),3(FC3)
            modLibrary.SetOption(optFilter, CTSettings.scansel.Data.filter, 2);

            //画像方向
            modLibrary.SetOption(optDirection, CTSettings.scansel.Data.image_direction, 0);

            //フィルタ処理(FFT/Conv)の設定                   'v13.0追加 by Ohkado 2007/02/16
            modLibrary.SetOption(optFilterProcess, CTSettings.scansel.Data.filter_process, 0);

            //RFC：0(無),1(弱),2(中),3(強)                   'v14.00追加 byやまおか 2007/07/13
            modLibrary.SetOption(optRFC, CTSettings.scansel.Data.rfc, 0);

            //アーティファクト低減           v9.7追加 by 間々田 2004/12/08
            if (CTSettings.scaninh.Data.artifact_reduction == 0)
            {
                fraArtifactReduction.Visible = true;
                chkArtifactReduction.CheckState = (CTSettings.scansel.Data.artifact_reduction == 1 ? CheckState.Checked : CheckState.Unchecked);
            }

            //透視画像保存：0(保存しない),1(保存する)
            //chkFluoroImageSave.CheckState = ((CTSettings.scansel.Data.fluoro_image_save == 1 && CTSettings.scansel.Data.data_mode == 1) ? CheckState.Checked : CheckState.Unchecked);
            //Rev20.00 コーンでも保存できるようにする by長野 2015/02/06
            chkFluoroImageSave.CheckState = ((CTSettings.scansel.Data.fluoro_image_save == 1) ? CheckState.Checked : CheckState.Unchecked);

            //再構成形状：0(正方形),1(円)
            modLibrary.SetOption(optReconMask, CTSettings.scansel.Data.recon_mask, 0);

            //画像回転角度：-180～180
            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneImageRotateAngle.Value = (decimal)CTSettings.scansel.Data.image_rotate_angle;
            if ((cwneImageRotateAngle.Maximum >= (decimal)CTSettings.scansel.Data.image_rotate_angle) && (cwneImageRotateAngle.Minimum <= (decimal)CTSettings.scansel.Data.image_rotate_angle))
            {
                //代入時のイベントロック
                //ImgRotAnglEvntLock = true;
                cwneImageRotateAngle.Value = (decimal)CTSettings.scansel.Data.image_rotate_angle;
                //ImgRotAnglEvntLock = false;
            }
            //追加2015/01/29hata
            else if (cwneImageRotateAngle.Maximum < (decimal)CTSettings.scansel.Data.image_rotate_angle)
            {
                cwneImageRotateAngle.Value = cwneImageRotateAngle.Maximum;
            }
            else if (cwneImageRotateAngle.Minimum > (decimal)CTSettings.scansel.Data.image_rotate_angle)
            {
                cwneImageRotateAngle.Value = cwneImageRotateAngle.Minimum;
            }

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //メール送信                             v9.1追加 by 間々田 2004/05/13
            //        If scaninh.mail_send = 0 Then
            //            fraSendMail.Visible = True
            //            chkSendMail.Value = IIf(.mail_send = 1, vbChecked, vbUnchecked)
            //        End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //コーン画質：0(標準),1(精細),2(高速)        'コーンビーム用
            modLibrary.SetOption(optImageMode, CTSettings.scansel.Data.cone_image_mode, 0);

            //スライス枚数                               'コーンビーム用
            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneK.Value = CTSettings.scansel.Data.k;
            if ((cwneK.Maximum >= (decimal)CTSettings.scansel.Data.k) && (cwneK.Minimum <= (decimal)CTSettings.scansel.Data.k))
            {   //代入時のイベントロック
                //KEvntLock = true;
                cwneK.Value = CTSettings.scansel.Data.k;
                //KEvntLock = false;
            }
            //追加2015/01/29hata
            else if (cwneK.Maximum < (decimal)CTSettings.scansel.Data.k)
            {
                cwneK.Value = cwneK.Maximum;
            }
            else if (cwneK.Minimum > (decimal)CTSettings.scansel.Data.k)
            {
                cwneK.Value = cwneK.Minimum;
            }

            //ヘリカルピッチ(mm)                         'コーンビーム用
            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneZp.Value = (decimal)CTSettings.scansel.Data.zp;
            if ((cwneZp.Maximum >= (decimal)CTSettings.scansel.Data.zp) && (cwneZp.Minimum <= (decimal)CTSettings.scansel.Data.zp))
            {
                //代入時のイベントロック
                //ZpEvntLock = true;
                cwneZp.Value = (decimal)CTSettings.scansel.Data.zp;
                //ZpEvntLock = false;
            }
            //追加2015/01/29hata
            else if (cwneZp.Maximum < (decimal)CTSettings.scansel.Data.zp)
            {
                cwneZp.Value = cwneZp.Maximum;
            }
            else if (cwneZp.Minimum > (decimal)CTSettings.scansel.Data.zp)
            {
                cwneZp.Value = cwneZp.Minimum;
            }

            //最大ヘリカル開始位置(mm)=昇降下限値(mm)
            //    構造体名：t20kinf
            //    コモン名：lower_limit
            //    コモン値：×100の値を格納
            lblZdasmin.Text = CTSettings.GValLowerLimit.ToString("###0.000");

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //往復スキャン                   v9.0追加 by 間々田 2004/01/30
            //        SetOption optRoundTrip, .round_trip
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //テーブル回転：0(ｽﾃｯﾌﾟ),1(連続)
            modLibrary.SetOption(optTableRotation, CTSettings.scansel.Data.table_rotation, 0);

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //X線管
            if (CTSettings.scaninh.Data.multi_tube == 0)
            {
                //fraMultiTube.Visible = True;
                //SetOption optMultiTube, .multi_tube '0(130kV),1(225kV)
                fraMultiTube.Visible = true;
                modLibrary.SetOption(optMultiTube, CTSettings.scansel.Data.multi_tube, 0);

                //Rev23.10 条件追加 by長野 2015/09/29
                if (CTSettings.scaninh.Data.xray_remote == 0)
                {
                    fraMultiTube.Enabled = false;
                }
                else
                {
                    fraMultiTube.Enabled = true;
                }
            }
            //回転選択                       v9.0追加 by 間々田 2004/01/30
            else if (CTSettings.scaninh.Data.rotate_select == 0)
            {
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //            fraRotateSelect.Visible = True
                //            SetOption optRotateSelect, .rotate_select, 0
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            }
            else
            {
                modLibrary.SetOption(optMultiTube, 0);
            }

            //v29.99 optMultiTube_Clickの中のUpdateFcdFidをここで処理する。by長野 2013/04/08'''''ここから'''''
            UpdateFcdFid();
            //v29.99 optMultiTube_Clickの中のUpdateFcdFidをここで処理する。by長野 2013/04/08'''''ここまで'''''


            //スキャンエリア                                             'v15.02追加 上から移動 by 間々田 2009/09/14
            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneArea.Value = (decimal)(IsCone ? CTSettings.scansel.Data.cone_scan_area : CTSettings.scansel.Data.mscan_area);
            float fArea = (IsCone ? CTSettings.scansel.Data.cone_scan_area : CTSettings.scansel.Data.mscan_area);
            if ((cwneArea.Maximum >= (decimal)fArea) && (cwneArea.Minimum <= (decimal)fArea))
            {   //代入時のイベントロック
                //AreaEvntLock = true;
                cwneArea.Value = (decimal)fArea;
                //AreaEvntLock = false;
            }
            //追加2015/01/29hata
            else if (cwneArea.Maximum < (decimal)fArea)
            {
                cwneArea.Value = cwneArea.Maximum;
            }
            else if (cwneArea.Minimum > (decimal)fArea)
            {
                cwneArea.Value = cwneArea.Minimum;
            }
  
            //スライス厚（mm）
            if (IsCone)
            {
                //cwneSlice.SetMinMax .min_cone_slice_width, .max_cone_slice_width    '最小値・最大値を設定
                //cwneSlice.value = .cone_scan_width                                  'スライス厚(mm)
                //cwneSlicePix.value = .cone_scan_width / .min_cone_slice_width

                //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                //cwneK.Value = (decimal)CTSettings.scansel.Data.delta_msw;
                if ((cwneSlicePix.Maximum >= (decimal)CTSettings.scansel.Data.delta_msw) && (cwneSlicePix.Minimum <= (decimal)CTSettings.scansel.Data.delta_msw))
                {
                    //代入時のイベントロック
                    //SlicePixEvntLock = true;    //代入時のイベントロック 追加2014/06/23(検S1)hata
                    cwneSlicePix.Value = (decimal)CTSettings.scansel.Data.delta_msw;
                    //SlicePixEvntLock = false;   //代入時のイベントロック 追加2014/06/23(検S1)hata
                }
                //追加2015/01/29hata
                else if (cwneSlicePix.Maximum < (decimal)CTSettings.scansel.Data.delta_msw)
                {
                    cwneSlicePix.Value = cwneSlicePix.Maximum;
                }
                else if (cwneSlicePix.Minimum > (decimal)CTSettings.scansel.Data.delta_msw)
                {
                    cwneSlicePix.Value = cwneSlicePix.Minimum;
                }
            }
            else
            {
                //cwneSlice.SetMinMax .min_slice_wid, .max_slice_wid  '最小値・最大値を設定
                //cwneSlice.value = .mscan_width                      'スライス厚

                //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                //cwneSlicePix.Value = (decimal)(CTSettings.scansel.Data.mscan_width / CTSettings.scansel.Data.min_slice_wid);
                decimal fSlicePix = (decimal)(CTSettings.scansel.Data.mscan_width / CTSettings.scansel.Data.min_slice_wid);
                //Rev20.00 四捨五入を追加 by長野 2015/01/26
                fSlicePix = Math.Round(fSlicePix / cwneSlicePix.Increment,MidpointRounding.AwayFromZero) * cwneSlicePix.Increment;
                if ((cwneSlicePix.Maximum >= fSlicePix) && (cwneSlicePix.Minimum <= fSlicePix))
                {
                    //代入時のイベントロック
                    //SlicePixEvntLock = true;    //代入時のイベントロック 追加2014/06/23(検S1)hata
                    cwneSlicePix.Value = fSlicePix;
                    //SlicePixEvntLock = false;   //代入時のイベントロック 追加2014/06/23(検S1)hata
                }
                //追加2015/01/29hata
                else if (cwneSlicePix.Maximum < fSlicePix)
                {
                    cwneSlicePix.Value = cwneSlicePix.Maximum;
                }
                else if (cwneSlicePix.Minimum > fSlicePix)
                {
                    cwneSlicePix.Value = cwneSlicePix.Minimum;
                }
            }

            //画素欄にも値をセットするため以下の処理を行う
            //cwneSlice_ValueChanged cwneSlice.value, cwneSlice.value, False

            //FCD, FID値が前回と変更している場合、画素値固定としてスライス厚（mm）の再計算
            if (CTSettings.scansel.Data.fcd != frmMechaControl.Instance.FCDWithOffset || CTSettings.scansel.Data.fid != frmMechaControl.Instance.FIDWithOffset)
            {
                //cwneSlicePix_ValueChanged(cwneSlicePix, EventArgs.Empty);
                //Rev20.00 イベントだとLoad時はスルーするためイベントじゃないcwneSlicePixを実行
                //VBではLoad時にイベントが実行されないので上記のような形をとってあると思われるので、この修正を行った by長野 2015/02/05
                cwneSlicePix_ValueChangedNoEvent();
            }
            else
            {
                if (IsCone)
                {
                    //最小値・最大値を設定
                    //変更2015/01/29hata
                    //cwneSlice.Minimum = (decimal)CTSettings.scansel.Data.min_cone_slice_width;
                    //cwneSlice.Maximum = (decimal)CTSettings.scansel.Data.max_cone_slice_width;
                    cwneSlice.Minimum = Math.Round((decimal)CTSettings.scansel.Data.min_cone_slice_width / cwneSlice.Increment, MidpointRounding.AwayFromZero) * cwneSlice.Increment;
                    cwneSlice.Maximum = Math.Round((decimal)CTSettings.scansel.Data.max_cone_slice_width / cwneSlice.Increment, MidpointRounding.AwayFromZero) * cwneSlice.Increment;

                    //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                    //cwneSlice.Value = (decimal)CTSettings.scansel.Data.cone_scan_width;			//スライス厚(mm)
                    if ((cwneSlice.Maximum >= (decimal)CTSettings.scansel.Data.cone_scan_width) && (cwneSlice.Minimum <= (decimal)CTSettings.scansel.Data.cone_scan_width))
                    {
                        //代入時のイベントロック
                        //SliceEvntLock = true;
                        cwneSlice.Value = (decimal)CTSettings.scansel.Data.cone_scan_width;			//スライス厚(mm)
                        //SliceEvntLock = false;
                    }
                    //追加2015/01/29hata
                    else if (cwneSlice.Maximum < (decimal)CTSettings.scansel.Data.cone_scan_width)
                    {
                        cwneSlice.Value = cwneSlice.Maximum;
                    }
                    else if (cwneSlice.Minimum > (decimal)CTSettings.scansel.Data.cone_scan_width)
                    {
                        cwneSlice.Value = cwneSlice.Minimum;
                    }
                }
                else
                {
                    //最小値・最大値を設定
                    //変更2015/01/29hata
                    //cwneSlice.Minimum = (decimal)CTSettings.scansel.Data.min_slice_wid;
                    //cwneSlice.Maximum = (decimal)CTSettings.scansel.Data.max_slice_wid;
                    cwneSlice.Minimum = Math.Round((decimal)CTSettings.scansel.Data.min_slice_wid / cwneSlice.Increment, MidpointRounding.AwayFromZero) * cwneSlice.Increment;
                    cwneSlice.Maximum = Math.Round((decimal)CTSettings.scansel.Data.max_slice_wid / cwneSlice.Increment, MidpointRounding.AwayFromZero) * cwneSlice.Increment;

                    //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                    //cwneSlice.Value = (decimal)CTSettings.scansel.Data.mscan_width;				//スライス厚(mm)
                    if ((cwneSlice.Maximum >= (decimal)CTSettings.scansel.Data.mscan_width) && (cwneSlice.Minimum <= (decimal)CTSettings.scansel.Data.mscan_width))
                    {
                        //代入時のイベントロック
                        //SliceEvntLock = true;
                        cwneSlice.Value = (decimal)CTSettings.scansel.Data.mscan_width;				//スライス厚(mm)
                        //SliceEvntLock = false;
                    }
                    //追加2015/01/29hata
                    else if (cwneSlice.Maximum < (decimal)CTSettings.scansel.Data.mscan_width)
                    {
                        cwneSlice.Value = cwneSlice.Maximum;
                    }
                    else if (cwneSlice.Minimum > (decimal)CTSettings.scansel.Data.mscan_width)
                    {
                        cwneSlice.Value = cwneSlice.Minimum;
                    }
                }
            }

            //スライスピッチ(mm)=軸方向Boxelｻｲｽﾞ(mm)     'コーンビーム用
            //cwneDelta_z.value = .delta_z
            //cwneDelta_z.value = .delta_z / .min_cone_slice_width * SWMinCone
            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneDelta_z.Value = (decimal)(CTSettings.scansel.Data.delta_z * CTSettings.scansel.Data.delta_msw / CTSettings.scansel.Data.cone_scan_width * SWMinCone);
            float fDelta_z = (CTSettings.scansel.Data.delta_z * CTSettings.scansel.Data.delta_msw / CTSettings.scansel.Data.cone_scan_width * SWMinCone);
            //Rev20.00 四捨五入する by長野 2015/01/24
            decimal value = 0;
            decimal.TryParse(((double)fDelta_z).ToString(string.Format("F{0}", cwneDelta_z.DecimalPlaces)), out value);
            if ((cwneDelta_z.Maximum >= (decimal)fDelta_z) && (cwneDelta_z.Minimum <= (decimal)fDelta_z))
            {   //代入時のイベントロック
                //DeltazEvntLock = true;
                //cwneDelta_z.Value = (decimal)fDelta_z;
                //Rev20.00 四捨五入した値を入れる by長野 2015/01/20
                cwneDelta_z.Value = value;
                //DeltazEvntLock = false;
            }
            //追加2015/01/29hata
            else if (cwneDelta_z.Maximum < value)
            {
                cwneDelta_z.Value = cwneDelta_z.Maximum;
            }
            else if (cwneDelta_z.Minimum > value)
            {
                cwneDelta_z.Value = cwneDelta_z.Minimum;
            }

            //オートセンタリング：0(なし),1(あり)
            if (CTSettings.scaninh.Data.auto_centering == 0)
            {
                fraAutoCentering.Visible = true;
                modLibrary.SetOption(optAutoCentering, CTSettings.scansel.Data.auto_centering);
            }

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //オーバースキャン                       v9.0追加 by 間々田 2004/01/30
            //        If scaninh.over_scan = 0 Then
            //            fraOverScan.Visible = True
            //        End If
            //
            //        'オーバースキャン                       v9.0追加 by 間々田 2004/01/30
            //        SetOption optOverScan, .over_scan
            //
            //        'コーン分散処理                         v8.0追加 by 間々田 2003/11/21
            //        If (scaninh.cone_distribute = 0) Or (scaninh.cone_distribute2 = 0) Then
            //            SetOption optConeDistribute, .cone_distribute
            //        End If
            modLibrary.SetOption(optOverScan, CTSettings.scansel.Data.over_scan);

            //マルチスライス
            //If Not IsCone Then
            if (! IsCone) 
            {            
                //
                //            'マルチスライスのピッチ
                //            With ntbMultislicePitch
                //                '最大値
                //                .Max = scansel.max_multislice_pitch
                //                '最大値の表示：最大 ～mm
                //                lblPitchMinMax.Caption = GetResString(IDS_MaxMM, .Max)
                //                'ピッチ
                //                .Value = scansel.multislice_pitch
                //            End With
                //
                //            '同時スキャン枚数最大(0:1ｽﾗｲｽ,1:3ｽﾗｲｽ,2:5ｽﾗｲｽ)
                //            optMultislice(0).Enabled = (0 <= .max_multislice)
                //            optMultislice(1).Enabled = (1 <= .max_multislice)
                //            optMultislice(2).Enabled = (2 <= .max_multislice)
                //v29.99 今のところ不要だが処理を進めるため変更して生かす by長野 2013/04/08'''''ここから'''''
                //同時スキャン枚数(0:1ｽﾗｲｽ,1:3ｽﾗｲｽ,2:5ｽﾗｲｽ)
                //SetOption optMultislice, .multislice
                modLibrary.SetOption(optMultislice, 0);
                //
                //v29.99 今のところ不要だが処理を進めるため変更して生かす by長野 2013/04/08'''''ここまで'''''
            }

            //
            //        'Ｘ線休止スキャン                       v9.3追加 by 間々田 2004/06/30
            //        If scaninh.discharge_protect = 0 Then
            //            fraDischargeProtect.Visible = True
            //            chkDischargeProtect.Value = IIf(.discharge_protect = 1, vbChecked, vbUnchecked)
            //        End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //ビニング                               v7.0追加 by 間々田 2003/09/26
            modLibrary.SetOption(optBinning, CTSettings.scansel.Data.binning);

            //スキャン中再構成
            modLibrary.SetOption(optScanAndView, CTSettings.scansel.Data.scan_and_view);

            //オートズーミング有無：
            chkZooming.CheckState = (CTSettings.scansel.Data.auto_zoomflag == 1 ? CheckState.Checked : CheckState.Unchecked);

            //ズーミングテーブル
            FileName = modLibrary.AddExtension(Path.Combine(CTSettings.scansel.Data.autozoom_dir.GetString(), CTSettings.scansel.Data.auto_zoom.GetString()), ".csv");
            txtZoomDirName.Text = string.IsNullOrEmpty(FileName) ? "" : Path.GetDirectoryName(FileName);		//ディレクトリ
            txtZoomFileName.Text = string.IsNullOrEmpty(FileName) ? "" : Path.GetFileName(FileName);			//テーブル名

            //オートプリント
            chkPrint.CheckState = (CTSettings.scansel.Data.auto_print == 1 ? CheckState.Checked : CheckState.Unchecked);

            //画像階調最適化
            modLibrary.SetOption(optContrastFitting, CTSettings.scansel.Data.contrast_fitting);

            if (IsCone)
            {
                //最大スライス厚の計算
                //SWMaxChange((2 / 3) * SWMaxPara);
                //Rev20.00 小数点での計算が必要なため変更 by長野 2014/11/04
                SWMaxChange((2.0f / 3.0f) * (float)SWMaxPara);

                //最大ヘリカルピッチの設定
                ZpMaxChange((float)((decimal)CTSettings.GValUpperLimit - cwneZdas.Value - cwneDelta_z.Value * (cwneK.Value - 1)) / kzp);

                //ヘリカルモード
                modLibrary.SetOption(optHelical, CTSettings.scansel.Data.inh);
            }

            //v17.02削除 frmScanControlへ移動したため byやまおか 2010/07/16
            //'PkeFPDのゲイン/積分時間をセットする    'v17.00追加(ここから) byやまおか 2010/02/17
            //If DetType = DetTypePke Then
            //    cmbGain.ListIndex = .fpd_gain   'FPDのゲイン設定
            //    cmbInteg.ListIndex = .fpd_integ 'FPDの積分時間設定
            //End If                                  'v17.00追加(ここまで) byやまおか 2010/02/17

            //v19.00 ->(電S2)永井
            if (CTSettings.scaninh.Data.mbhc == 0)
            {
                //ﾋﾞｰﾑﾊﾄﾞﾆﾝｸﾞ補正　  0(行わない),1(行う)
                chkBHC.CheckState = (CTSettings.scansel.Data.mbhc_flag == 1 ? CheckState.Checked : CheckState.Unchecked);

                //Rev20.00 追加 by長野 2014/12/04
                //FileName2 = modLibrary.AddExtension(Path.Combine(CTSettings.scansel.Data.mbhc_dir.GetString(), CTSettings.scansel.Data.mbhc_name.GetString()), ".csv");

                //Rev20.00 変更 by長野 2015/02/15
                if (CTSettings.scansel.Data.mbhc_dir.GetString() == "" || CTSettings.scansel.Data.mbhc_name.GetString() == "")
                {
                    FileName2 = "";
                }
                else
                {
                    FileName2 = modLibrary.AddExtension(Path.Combine(CTSettings.scansel.Data.mbhc_dir.GetString(), CTSettings.scansel.Data.mbhc_name.GetString()), ".csv");
                }

                ////ＢＨＣディレクトリ名
                //txtBhcDirName.Text = CTSettings.scansel.Data.mbhc_dir.GetString();

                ////ＢＨＣテーブル名                             'v8.1 追加 : 拡張子(.csv)を表示する。by Ohkado 2007/02/15
                //txtBhcFileName.Text = modLibrary.AddExtension(modLibrary.RemoveNull(CTSettings.scansel.Data.mbhc_name.GetString()), ".csv");

                txtBhcDirName.Text = string.IsNullOrEmpty(FileName2) ? "" : Path.GetDirectoryName(FileName2);		//ディレクトリ
                txtBhcFileName.Text = string.IsNullOrEmpty(FileName2) ? "" : Path.GetFileName(FileName2);			//テーブル名

                txtBhcFileNameChange();
            }
            //<- v19.00

            //Rev26.00 追加 by井上 2017/01/18
            //コモンscansel.mbhc_phantomlessの値と等しいcmbBHCPhantomlessのインデックスを選択
            cmbBHCPhantomless.SelectedIndex = CTSettings.scansel.Data.mbhc_phantomless;

            //コンボボックスのリストを作成
            CreateViewListComboBox();

            //削除 2012-01-11

            //scanselに保存されているビュー数に、最も近いビュー数を選択されているindexとする。
            cmbViewNum.SelectedIndex = SetViewListIndex();

            //        If cmbViewNum.List(cmbViewNum.ListIndex) > GVal_ViewMax Then
            //
            //            cwneViewNum.SetMinMax GVal_ViewMin, cmbViewNum.List(cmbViewNum.ListIndex)
            //
            //        Else
            //
            //            cwneViewNum.SetMinMax GVal_ViewMin, GVal_ViewMax
            //
            //        End If
        }


		//*******************************************************************************
		//機　　能： X線管変更時に校正ステータスを初期化する関数
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  2002-10-3   山本      新規作成
		//*******************************************************************************
		private bool Init_Cor_Status()
		{
			//modMecainf.mecainfType theMecainf = default(modMecainf.mecainfType);
            MecaInf theMecainf = new MecaInf();
            theMecainf.Data.Initialize();

			//戻り値初期化
			bool functionReturnValue = false;

			//校正ステータスの初期化メッセージ：
			//   Ｘ線管を変更するとオフセットを除くすべての校正ステータスが初期化されます。
			//   よろしければＯＫをクリックして下さい。
			DialogResult result = MessageBox.Show(CTResources.LoadResString(9376) + "\r" + CTResources.LoadResString(StringTable.IDS_ClickOK),
												  CTResources.LoadResString(12244), MessageBoxButtons.OKCancel);
			if (result == DialogResult.Cancel) return functionReturnValue;

			//mecainf読み込み
			//modMecainf.GetMecainf(ref theMecainf);
            theMecainf.Load();

			theMecainf.Data.normal_rc_cor = 0;	//ノーマルスキャン用回転中心校正ステータス
            theMecainf.Data.cone_rc_cor = 0;		//コーンビーム用回転中心校正ステータス
            theMecainf.Data.ver_iifield = -1;	//幾何歪校正I.I.視野
            theMecainf.Data.ver_mt = -1;			//幾何歪校正Ｘ線管
            theMecainf.Data.rc_kv = 0;			//回転中心校正管電圧
            theMecainf.Data.rc_udab_pos = 0;		//回転中心校正昇降位置
            theMecainf.Data.rc_iifield = -1;		//回転中心校正I.I.視野
            theMecainf.Data.rc_mt = -1;			//回転中心校正Ｘ線管
            theMecainf.Data.gain_iifield = -1;	//ゲイン校正I.I.視野
            theMecainf.Data.gain_kv = 0;			//ゲイン校正管電圧
			//.gain_ma = 0        'ゲイン校正管電流      'v15.0削除 by 間々田 2009/03/18 未使用のため
            theMecainf.Data.gain_ma = 0;			//ゲイン校正管電流       'v19.00復活 by 長野   2012/05/10
            theMecainf.Data.gain_mt = -1;		//ゲイン校正Ｘ線管
            theMecainf.Data.gain_filter = 0;		//ゲイン校正フィルタ
            theMecainf.Data.dc_iifield = -1;		//寸法校正I.I.視野
            theMecainf.Data.dc_mt = -1;			//寸法校正Ｘ線管
            theMecainf.Data.sp_iifield = -1;		//スキャン位置校正I.I.視野
            theMecainf.Data.sp_mt = -1;			//スキャン位置校正Ｘ線管
            theMecainf.Data.ver_bin = -1;		//幾何歪校正実行時のビニングモード
            theMecainf.Data.rc_bin = -1;			//回転中心校正実行時のビニングモード
            theMecainf.Data.gain_bin = -1;		//ゲイン校正実行時のビニングモード
            theMecainf.Data.dc_bin = -1;			//寸法校正実行時のビニングモード
            theMecainf.Data.sp_bin = -1;			//スキャン位置校正実行時のビニングモード

			//mecainf書き換え
			//modMecainf.PutMecainf(ref theMecainf);
            theMecainf.Write();

			//戻り値セット
			functionReturnValue = true;
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： マルチスキャン時のスキャンピッチの最大値・最小値を設定する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 昇降位置・スライス枚数側が変更したら再設定すること
		//
		//履　　歴： v11.4 2006/03/13  (SI3) 間々田     新規作成
		//*******************************************************************************
		private void SetScanPitchRange()
		{
            //Rev26.00 add by chouno 2017/10/16 
            frmMechaControl.Instance.tmrMecainfSeqCommEx();

			//マルチスキャン時のスキャンピッチの設定

			//範囲の更新
            //変更2014/10/07hata_v19.51反映
            //cwneMSPitch.Minimum = Math.Truncate((decimal)(CTSettings.GValLowerLimit - udPos) / (cwneMSSlice.Value - 1) / cwneMSPitch.Increment) * cwneMSPitch.Increment;
            //cwneMSPitch.Maximum = Math.Truncate((decimal)(CTSettings.GValUpperLimit - udPos) / (cwneMSSlice.Value - 1) / cwneMSPitch.Increment) * cwneMSPitch.Increment;
            //v19.51 回転大テーブルが装着されている場合、X線、検出器に当たる方向のマルチは選択させない by長野 2014/02/27
            //X線・検出器昇降の場合は原点が試料扉の中央付近にくることを想定してコーディングしています by長野 2014/02/27
            //if ((modSeqComm.GetLargeRotTableSts() == 1))
            //Rev26.00 change by chouno 2017/03/13
            //if ((modSeqComm.GetLargeRotTableSts() == 1) && (CTSettings.t20kinf.Data.ud_type == 1))
            //Rev26.20 微調テーブルタイプを見るように変更 by chouno 2019/02/11
            if (((modSeqComm.GetLargeRotTableSts() == 1) && (CTSettings.t20kinf.Data.ftable_type == 0)) && (CTSettings.t20kinf.Data.ud_type == 1))
            {
                if ((CTSettings.t20kinf.Data.ud_type == 1))
                {
                    //範囲の更新
                    //_with10.SetMinMax(0.0, Conversion.Fix((modGlobal.GValUpperLimit - udPos) / (cwneMSSlice.Value - 1) / _with10.DiscreteInterval) * _with10.DiscreteInterval);
                    cwneMSPitch.Minimum = 0.0M;
                    cwneMSPitch.Maximum = Math.Truncate((decimal)(CTSettings.GValUpperLimit - udPos) / (cwneMSSlice.Value - 1) / cwneMSPitch.Increment) * cwneMSPitch.Increment;
                }
                else
                {
                    //範囲の更新
                    //_with10.SetMinMax(Conversion.Fix((modGlobal.GValLowerLimit - udPos) / (cwneMSSlice.Value - 1) / _with10.DiscreteInterval) * _with10.DiscreteInterval, 
                    //                  Conversion.Fix((modGlobal.GValUpperLimit - udPos) / (cwneMSSlice.Value - 1) / _with10.DiscreteInterval) * _with10.DiscreteInterval);
                    //cwneMSPitch.Minimum = Math.Truncate((decimal)(CTSettings.GValLowerLimit - udPos) / (cwneMSSlice.Value - 1) / cwneMSPitch.Increment) * cwneMSPitch.Increment;
                    //Rev23.10 変更 by長野 201511/08 
                    //cwneMSPitch.Minimum = Math.Truncate((decimal)(CTSettings.GValLowerLimit - udPos) / (cwneMSSlice.Value - 1) / cwneMSPitch.Increment) * cwneMSPitch.Increment;
                    //Rev23.10 変更 by長野 2015/11/14
                    cwneMSPitch.Minimum = (decimal)0.0M;
                    cwneMSPitch.Maximum = Math.Truncate((decimal)(CTSettings.GValUpperLimit - udPos) / (cwneMSSlice.Value - 1) / cwneMSPitch.Increment) * cwneMSPitch.Increment;
                }
            }
            else
            {
                //範囲の更新
                //_with10.SetMinMax(Conversion.Fix((modGlobal.GValLowerLimit - udPos) / (cwneMSSlice.Value - 1) / _with10.DiscreteInterval) * _with10.DiscreteInterval, 
                //                  Conversion.Fix((modGlobal.GValUpperLimit - udPos) / (cwneMSSlice.Value - 1) / _with10.DiscreteInterval) * _with10.DiscreteInterval);
                cwneMSPitch.Minimum = Math.Truncate((decimal)(CTSettings.GValLowerLimit - udPos) / (cwneMSSlice.Value - 1) / cwneMSPitch.Increment) * cwneMSPitch.Increment;
                cwneMSPitch.Maximum = Math.Truncate((decimal)(CTSettings.GValUpperLimit - udPos) / (cwneMSSlice.Value - 1) / cwneMSPitch.Increment) * cwneMSPitch.Increment;
            }


			//範囲の表示
			lblPitchBand.Text = StringTable.GetResString(StringTable.IDS_Range, cwneMSPitch.Minimum.ToString("0.000"),
																				cwneMSPitch.Maximum.ToString("0.000"));
		}


		//*******************************************************************************
		//機　　能： マルチスキャン時のスライス枚数の最大値を設定する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： スキャンピッチ側が変更したら再設定すること
		//
		//履　　歴： v11.4 2006/03/13  (SI3) 間々田     新規作成
		//*******************************************************************************
		private void SetSliceNumRange()
		{
			int MaxScanCount = 0;
			int SliceMax = 0;		//ｽﾗｲｽ数の最大値(9999以下)

			//容量的な意味でのスキャン可能回数を取得する
			//MaxScanCount = GetMaxScanCount()  あとで
			MaxScanCount = 9999;

			//スライス数の最大値
			if (cwneMSPitch.Value < 0)
			{
                //2014/11/07hata キャストの修正
                //SliceMax = (int)(1 + Math.Truncate((CTSettings.GValLowerLimit - udPos) / (float)cwneMSPitch.Value));
                SliceMax = Convert.ToInt32(1 + Math.Truncate((CTSettings.GValLowerLimit - udPos) / (float)cwneMSPitch.Value));
            }
			else if (cwneMSPitch.Value > 0)
			{
                //2014/11/07hata キャストの修正
                //SliceMax = (int)(1 + Math.Truncate((CTSettings.GValUpperLimit - udPos) / (float)cwneMSPitch.Value));
                SliceMax = Convert.ToInt32(1 + Math.Truncate((CTSettings.GValUpperLimit - udPos) / (float)cwneMSPitch.Value));
            }
			else
			{
				SliceMax = 9999;
			}

			//強制的にスライス枚数最大値を２以上にする（その場合、ピッチを０にする）
			if (SliceMax < 2)
			{
				SliceMax = 2;
                //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                //cwneMSPitch.Value = 0;
                if ((cwneMSPitch.Maximum >= 0) && (cwneMSPitch.Minimum <= 0))
                {
                    //代入時のイベントロック
                    //MSPitchEvntLock = true;
                    cwneMSPitch.Value = 0;
                    //MSPitchEvntLock = false;
                }
                //追加2015/01/29hata
                else if (cwneMSPitch.Maximum < 0)
                {
                    cwneMSPitch.Value = cwneMSPitch.Maximum;
                }
                else if (cwneMSPitch.Minimum > 0)
                {
                    cwneMSPitch.Value = cwneMSPitch.Minimum;
                }
            }

			//マルチスキャン時のスライス数の設定

			//.Minimum = 2           '最小値は２（固定）
			cwneMSSlice.Maximum = modLibrary.MinVal(SliceMax, MaxScanCount);

			//範囲の表示
			lblSliceBand.Text = StringTable.GetResString(StringTable.IDS_Range, cwneMSSlice.Minimum.ToString(), cwneMSSlice.Maximum.ToString());
		}


		//*******************************************************************************
		//機　　能： Ｘ線休止スキャンチェックボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v9.3 2004/07/02  (SI4) 間々田     新規作成
		//*******************************************************************************
		private void chkDischargeProtect_CheckStateChanged(object sender, EventArgs e)
		{
			//added by 山本　2004-7-5　X線ON時間の表示を追加
			lblContTime.Visible = (chkDischargeProtect.CheckState == CheckState.Checked);

			//最大最小ビュー数の更新
			UpdateViewMinMax();

			//最大積算枚数の更新
			UpdateIntegMax();

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == chkDischargeProtect) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： 「オートプリント」チェックボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.5  2006/09/01  (WEB)間々田  新規作成
		//*******************************************************************************
		private void chkPrint_CheckStateChanged(object sender, EventArgs e)
		{
			//スキャン中再構成を行わない場合
			if ((!IsCone) && optScanAndView0.Checked && (chkPrint.CheckState == CheckState.Checked))
			{
				//メッセージ表示：
				//   スキャン中再構成を行わない場合はオートプリントができません。
				//   オートプリントを行う場合は、先にスキャン中再構成を行うに設定してください。
				MessageBox.Show(StringTable.GetResString(9410, fraAutoPrint.Text), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

				//強制的にチェックを外す
				chkPrint.CheckState = CheckState.Unchecked;
				return;
			}

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == chkPrint) cmdOK.Enabled = true;
		}


		//'*******************************************************************************
		//'機　　能： 「生データをセーブする」チェックボタンクリック時処理
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//'*******************************************************************************
		//Private Sub ChkSave_Click()
		//
		//    'コーンビーム時
		//    If IsCone Then
		//
		//        'コーン分散処理有効時は、生データ保存ありを選択できない
		//        'If optConeDistribute(1).Value And (chkSave.Value = vbChecked) Then
		//        If optConeDistribute(1).value And (chkSave.value = vbChecked) And (scaninh.cone_distribute = 0) Then 'v10.0変更 by 間々田 2005/02/02 コーン分散処理２の場合、設定できるようにした
		//            'メッセージ表示：コーン分散処理有効時は、生データ保存ありを選択できません。強制的に「生データ保存なし」に設定します。
		//            MsgBox LoadResString(12919), vbExclamation
		//            chkSave.value = vbUnchecked
		//        End If
		//
		//    'コーンビーム時以外：スキャン中再構成を行わない場合は必ず生データを保存する
		//    ElseIf optScanAndView(0).value And (chkSave.value = vbUnchecked) Then
		//
		//        'メッセージ表示：
		//        '   スキャン中再構成を行わない場合は強制的に生データが保存されます。
		//        '   生データを保存しない場合は、先にスキャン中再構成を行うに設定してください。
		//        MsgBox LoadResString(9411), vbExclamation
		//
		//        '強制的にチェックにして抜ける
		//        chkSave.value = vbChecked
		//
		//    End If
		//
		//End Sub


		//*******************************************************************************
		//機　　能： オートズーミングを行う・チェックボックスクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void chkZooming_CheckStateChanged(object sender, EventArgs e)
		{
			//スキャン中再構成を行わない場合
			if ((!IsCone) && optScanAndView0.Checked && (chkZooming.CheckState == CheckState.Checked))
			{
				//メッセージ表示：
				//   スキャン中再構成を行わない場合はオートズームができません。
				//   オートズームを行う場合は、先にスキャン中再構成を行うに設定してください。
				//MsgBox LoadResString(9410), vbCritical
				MessageBox.Show(StringTable.GetResString(9410, fraZoomingPlan.Text), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);		//v11.5変更 by 間々田 2006/09/01

				//強制的にチェックを外す
				chkZooming.CheckState = CheckState.Unchecked;
				return;
			}

			//関係するコントロールの使用可・不可の設定
			modLibrary.SetEnabledInFrame(fraZoomingPlan, (chkZooming.CheckState == CheckState.Checked), "CheckBox");

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == chkZooming) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： キャンセルボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			//スキャン条件用フォームをアンロード
			this.Close();
            //CallClosing();
        }


		//*******************************************************************************
		//機　　能： スライスプランテーブル内変更ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdChangeSlicePlan_Click(object sender, EventArgs e)
		{
			string FileName = null;

			//ファイル選択ダイアログ表示
			FileName = modFileIO.GetFileName(StringTable.IDS_Select, 
											 CTResources.LoadResString(StringTable.IDS_SlicePlanTable),
											 SubExtension: (IsCone ? "-csp" : "-spl"), 
											 InitFileName: Path.Combine(txtSlicePlanDir.Text, txtSlicePlanName.Text));
			if (string.IsNullOrEmpty(FileName)) return;

			txtSlicePlanDir.Text = Path.GetDirectoryName(FileName);
			txtSlicePlanName.Text = Path.GetFileName(FileName);

			//変更した場合，ＯＫボタンを使用可にする
			cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： スライスプランテーブル内「テーブル編集...」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdEditSPTable_Click(object sender, EventArgs e)
        {
            //スライスプランフォームをモーダル表示
            //frmSliceplan.Instance.ShowDialog(frmCorrectionStatusModal.Instance);
            //Rev23.40 変更 by長野 2016/06/19
            frmSliceplan.Instance.ShowDialog(frmCTMenu.Instance);
            
            //追加2014/11/07hata
            frmSliceplan.Instance.Dispose();
        }


		//*******************************************************************************
		//機　　能： オートズームフレーム内変更ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdChangeZoomingTable_Click(object sender, EventArgs e)
		{
			string FileName = null;

			//ズーミングテーブル選択ダイアログ表示
			FileName = modFileIO.GetFileName(StringTable.IDS_Select, 
											 CTResources.LoadResString(StringTable.IDS_ZoomingTable),
											 SubExtension: "-zom", 
											 InitFileName: Path.Combine(txtZoomFileName.Text, txtZoomFileName.Text));
			if (string.IsNullOrEmpty(FileName)) return;

			//ファイル名をズーミングテーブル欄にセット
			//txtZoomFileName.Text = Path.GetDirectoryName(FileName);
            //Rev20.00 修正 by長野 2014/12/04
            txtZoomDirName.Text = Path.GetDirectoryName(FileName);	
            txtZoomFileName.Text = Path.GetFileName(FileName);

			//変更した場合，ＯＫボタンを使用可にする
			cmdOK.Enabled = true;
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
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
        //変更2014/10/07hata_v19.51反映
        private void cmdOK_Click(object sender, EventArgs e)
        {
            //V19.20 スライスプラン選択時のプリセット設定 by Inaba 2012/11/01
            if (optMultiScanMode[5].Checked)
                PresetSlicePlan();

            SetScanCondition();
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
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************        
        //変更2014/10/07hata_v19.51反映
        //V19.20 ＯＫボタンクリック処理と別にする by Inaba 2012/11/01
        //Private Sub cmdOK_Click()
        //private void cmdOK_Click(object sender, EventArgs e)
        private void SetScanCondition()
		{

            //追加2014/10/07hata_v19.51反映
            int oldScanmode = 0;            //v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            int newScanMode = 0;            //v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //追加2014/10/07hata_v19.51反映
            //前のスキャンモード
            oldScanmode = CTSettings.scansel.Data.scan_mode;    //v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //Rev25.00 Wスキャン対応 by長野 2016/07/11
            CheckState old_W_ScanCheckState = (CTSettings.scansel.Data.w_scan == 1 ? CheckState.Checked : CheckState.Unchecked);
            CheckState new_W_ScanCheckState = CheckState.Unchecked;

            //Ｘ線管を変更した場合はすべての校正ステータスを準備未完了にする  'added by 山本 2002-10-3
            //Rev23.10 外部制御有の場合は、切替時の未完にするので条件追加 by長野 2015/09/29 
            //if (CTSettings.scaninh.Data.multi_tube == 0)
            if (CTSettings.scaninh.Data.multi_tube == 0 && CTSettings.scaninh.Data.xray_remote == 1)
            {
                if ((modLibrary.GetOption(optMultiTube) != ScanselOrg.multi_tube))
                {
                    if (!Init_Cor_Status())
                    {
                        //Rev20.00 最後の処理を行うように変更 by長野 2014/12/15
                        goto ErrorHandler;
                        //return;
                    }
                }
            }
 
            Debug.Print("ksw " + ksw.ToString());

			//スキャン条件の設定内容をチェック（その１）
            if (!OptValueChk1())
            {
                //Rev20.00 最後の処理を行うように変更 by長野 2014/12/15
                goto ErrorHandler;
                //return;
            }

			//スキャン条件の設定内容をチェック（その２）     'V4.0 append by 鈴山 2001/02/13
			OptValueChk2();

			//スキャン条件の設定内容をチェック（その３）     'V19.00 (電S2)永井
            if (!OptValueChk3())
            {
                //Rev20.00 最後の処理を行うように変更 by長野 2014/12/15
                goto ErrorHandler;
                //return;
            }

			//マウスポインタを砂時計にする
			this.Cursor = Cursors.WaitCursor;

			//スキャン条件の設定内容をコモンファイルへ書き込む
			//modScansel.scanselType theScansel = default(modScansel.scanselType);
            CTstr.SCANSEL theScansel =new CTstr.SCANSEL();// = default(CTstr.SCANSEL);
            theScansel.Initialize();

			//この画面で設定しているスキャン条件を取得
			GetControls(ref theScansel);

			//scansel書き込み
			//modScansel.PutScansel(ref theScansel);
            CTSettings.scansel.Put(theScansel);

			//MyScanselを更新                                            '追加 by 間々田 2009/07/31 下から移動
			modCommon.GetMyScansel();

            //追加2014/10/07hata_v19.51反映
            //新しく選んだスキャンモードモード
            newScanMode = theScansel.scan_mode;            //v18.00追加 byやまおか 2011/07/09
            new_W_ScanCheckState = (theScansel.w_scan == 1 ? CheckState.Checked : CheckState.Unchecked);  //Rev25.00 Wスキャン対応 追加 by長野 2016/07/11

            //追加2014/10/07hata_v19.51反映
            //シフトスキャン⇔非シフトスキャン間の変更をしたとき 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Wスキャンを条件に追加 Rev25.00 by長野 2016/07/11
            if ((((oldScanmode == (int)ScanSel.ScanModeConstants.ScanModeShift) && (newScanMode != (int)ScanSel.ScanModeConstants.ScanModeShift)) ||
                ((oldScanmode != (int)ScanSel.ScanModeConstants.ScanModeShift) && (newScanMode == (int)ScanSel.ScanModeConstants.ScanModeShift))) ||
                (old_W_ScanCheckState != new_W_ScanCheckState))
            {
                //★★★フラットパネルの幾何歪
                if (CTSettings.detectorParam.Use_FlatPanel)
                    ScanCorrect.FPD_DistorsionCorrect();
            }

			//グローバル変数VIEW_N にも代入しておく                                       'added by 間々田 2003/11/28
			//（スキャン条件でビュー数を変えてもグローバル変数VIEW_Nが変らず、スキャンスタート時の外部トリガ発生時の計算に使用するビュー数が異なっていた対策）
			ScanCorrect.VIEW_N = Convert.ToInt32(cwneViewNum.Value);

			modCT30K.TableRotOn = optTableRotation1.Checked;				//v7.0 added by 間々田 2003/09/26

			//オートセンタリング：0(なし),1(あり)
			//If fraAutoCentering.Visible Then
			//変更 by 間々田 2009/08/27
			if (CTSettings.scaninh.Data.auto_centering == 0)
			{
                //Call putcommon_long("scansel", "auto_centering", GetOption(optAutoCentering))
				frmScanControl.Instance.chkInhibit[3].Checked = (1 - modLibrary.GetOption(optAutoCentering)) != 0;		//v11.2変更 by 間々田 2006/01/13
            }

			//v15.0追加 by 間々田 2009/03/03 frmConeScanCondtionと統合
			if (IsCone)
			{
                CTSettings.scancondpar.Data.m0 = mc - CTSettings.scancondpar.Data.scan_posi_b[2];

                //modScancondpar.CallPutScancondpar();
                CTSettings.scancondpar.Write();

            }

			//'MyScanselを更新    'v10.0追加 by 間々田 2005-01-26
			//'GetScansel
			//GetMyScansel                                               'v11.2追加 by 間々田 2006/01/13 '削除 by 間々田 2009/07/31 上に移動

            //追加2014/10/07hata_v19.51反映
            //ビニングが変更された場合のみ実行する
            //v11.5追加 by 間々田 2006/07/28
            if (CTSettings.scansel.Data.binning != ScanselOrg.binning)
            {
                modCT30K.GetBinning();
                //ビニングモード変更時の各種値取得
                //frmStatus.UpdateBinningMode                                     'v11.4追加 by 間々田 2006/03/13
                frmCTMenu.Instance.UpdateBinningMode();                //v15.0変更 by 間々田 2009/02/27

            }
            //v11.5追加 by 間々田 2006/07/28

			//マトリクスサイズ表示の更新
			//frmStatus.UpdateMatrixSize
			frmCTMenu.Instance.UpdateMatrixSize();			//v15.0変更 by 間々田 2009/02/27

			//ビニングの変化があった場合、wlevel/wwidthを自動的に変更 2003/10/22 by 間々田
            if (CTSettings.scansel.Data.binning != ScanselOrg.binning)
			{
				float theScale = 0;
                float[] choose = new float[] { 1, 2, 4 };
                theScale = choose[CTSettings.scansel.Data.binning + 1] / choose[ScanselOrg.binning + 1];

				//wLevel = wLevel * theScale
				//wWidth = wWidth * theScale
                //2014/11/07hata キャストの修正
                //frmScanControl.Instance.WindowLevel = (int)(frmScanControl.Instance.WindowLevel * theScale);
                //frmScanControl.Instance.WindowWidth = (int)(frmScanControl.Instance.WindowWidth * theScale);
                frmScanControl.Instance.WindowLevel = Convert.ToInt32(frmScanControl.Instance.WindowLevel * theScale);
                frmScanControl.Instance.WindowWidth = Convert.ToInt32(frmScanControl.Instance.WindowWidth * theScale);
            }

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//DICOM変換用データ生成のためのサブルーチン added V6.0 by 間々田 2002-08-08
			//    UpdateForDICOM
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//frmStatusの校正ステータスを更新する    'v11.2追加 by 間々田 2006/01/19
			frmStatus.Instance.UpdateCorrectStatus();

			//v17.02削除 frmScanConditionへ移動 byやまおか 2010/07/16
			//'PkeFPD用のゲインと積分時間をセットする 'v17.00追加(ここから) byやまおか 2010/02/23
			//If (DetType = DetTypePke) And (hPke <> NullAddress) Then
			//
			//    Dim LiveON_flg As Boolean  'v17.02追加 byやまおか 2010/07/06
			//    LiveON_flg = False         'v17.02追加 byやまおか 2010/07/06
			//
			//    If (frmScanControl.ntbFpdGain.Value <> cmbGain.Text) Or (frmScanControl.ntbFpdInteg.Value <> cmbInteg.Text) Then
			//
			//        'Ｘ線オフと連動して透視ライブをオフする
			//        'frmTransImage.CaptureOn = False     'キャプチャ中だとゲイン/積分時間を設定できないため
			//        'v17.02変更 byやまおか 2010/07/06
			//        If frmTransImage.CaptureOn = True Then
			//            'frmTransImage.CaptureOn = False     'ONならOFFする
			//            PkeCaptureStop (hPke)   'ライブではなくPkeを止める  'v17.02変更 byやまおか 2010/07/16
			//            LiveON_flg = True
			//        End If
			//
			//        'ゲインと積分時間をセットする
			//        ret = PkeSetGainFrameRate(hPke, scansel.fpd_gain, scansel.fpd_integ)
			//
			//        'frmScanControlのフラットパネル設定ステータスを更新する 'v17.02改良 byやまおか 2010/07/16
			//        If (ret = 0) Then
			//            frmScanControl.UpdateFpdGainInteg
			//        Else
			//            MsgBox "FPDゲインとFPD積分時間の設定に失敗しました。"
			//        End If
			//
			//        '設定するためにOFFした場合は再度ONする    'v17.02追加 byやまおか 2010/07/06
			//        If LiveON_flg = True Then
			//            frmTransImage.CaptureOn = True
			//            LiveON_flg = False
			//        End If
			//
			//    End If
			//End If                                  'v17.00追加(ここまで) byやまおか 2010/02/23

			//画像データサイズ＋生データサイズの計算を行う   V3.0 append by 鈴山 2000/10/24
			modCT30K.Cal_SaveImageSize();

			//V7.0 append by 間々田 2003/10/21 新しいビニングモードでの幾何歪パラメータのセット
            if (CTSettings.detectorParam.Use_FlatPanel && (ScanselOrg.binning != CTSettings.scansel.Data.binning))
			{
				ScanCorrect.Get_Vertical_Parameter_Ex(0);
				ScanCorrect.Set_Vertical_Parameter();
			}

			//画質オプションコントロールをどれも選択していない状態にする（マニュアル）
            if (this.Visible && this.WindowState != FormWindowState.Minimized )
			{
                //Rev26.10 ガイドモードオフの場合は従来の制御にする 2018/01/17 by chouno
                if (CTSettings.scaninh.Data.guide_mode == 0)
                {
                    //frmScanControl.Instance.optQuality[(int)frmScanControl.ScanQualityConstants.ScanQualityManual].Checked = true;
                    //Rev26.00 change by chouno 2017/09/01
                    //if (modScanCondition.PresetSelectedIndex > -1) frmScanControl.Instance.optScanCond[modScanCondition.PresetSelectedIndex].Checked = false;
                    //Rev99.99 修正 by chouno 2018/05/23
                    frmScanControl.Instance.optScanCond[4].Checked = false;
                    frmScanControl.Instance.scanCondSetCmpFlg = false; //Rev26.00 add by chouno 2016/12/28
                }
                else
                {
                    frmScanControl.Instance.optQuality[(int)frmScanControl.ScanQualityConstants.ScanQualityManual].Checked = true;
                }
			}

            //追加2014/10/07hata_v19.51反映
            //スキャンコントロール画面にも反映する   'v18.00追加 byやまおか 2011/07/05 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            frmScanControl.Instance.optScanMode[CTSettings.scansel.Data.scan_mode].Checked = true;

            //Rev25.00 Wスキャンにも対応させる by長野 2016/08/05
            frmScanControl.Instance.chkW_Scan.CheckState = CTSettings.scansel.Data.w_scan == 1 ? CheckState.Checked : CheckState.Unchecked;

            //Rev20.00 goto文追加 by長野 2014/12/15

ErrorHandler:

            //スキャン条件用フォームをアンロード
            if (!myNoClose)
            {
                this.Close();
            }
            else
            {
                CallClosing();
            }
		}


		//*******************************************************************************
		//機　　能： スキャンピッチの自動設定ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.04  2006/03/13 (SI3)間々田  新規作成
		//*******************************************************************************
		private void cmdSetScanPitch_Click(object sender, EventArgs e)
		{
			//double thePitch = 0;							//v16.00変更 Single→Double byちょうの
            decimal thePitch = 0;							//v20.00変更 Double→Decimal by長野 2015/01/26

			//スライスピッチ×スライス枚数の計算
			//thePitch = cwneDelta_z.Value * cwneK.Value
			//スライス枚数1枚のときはピッチ0にする   'v17.43修正 byやまおか 2011/02/01
			if (cwneK.Value == 1)
			{
				thePitch = 0;
			}
			else
			{
				thePitch = (decimal)(cwneDelta_z.Value * cwneK.Value);
                //Rev20.00 四捨五入 by長野 2015/01/26 
                thePitch = Math.Round(thePitch/cwneMSPitch.Increment,MidpointRounding.AwayFromZero) * cwneMSPitch.Increment;
			}

            cwneMSPitch_dummy.Value = (decimal)thePitch;	//v16.00追加 byちょうの
          
			//昇降範囲を考慮
            //if (!modLibrary.InRangeFloat((float)thePitch, (float)cwneMSPitch.Minimum, (float)cwneMSPitch.Maximum))
            if (!modLibrary.InRange((float)thePitch, (float)cwneMSPitch.Minimum, (float)cwneMSPitch.Maximum))
			{
				//メッセージ表示：
				//   昇降範囲の制約のため､スキャンピッチの自動設定はできません。
				//   スライスピッチ、スライス枚数、マルチスキャンのスライス数を再設定してください。
				MessageBox.Show(CTResources.LoadResString(9571), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//ボタンをクリックする前に入力した値と異なる場合､OKボタンを使用可にする｡ 'v16.00追加 byちょうの
			if (cwneMSPitch.Value != cwneMSPitch_dummy.Value)
			{
				cmdOK.Enabled = true;
			}

			//スライスピッチ×スライス枚数をマルチスキャンピッチの欄に入れる
            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneMSPitch.Value = (decimal)thePitch;
            if ((cwneMSPitch.Maximum >= (decimal)thePitch) && (cwneMSPitch.Minimum <= (decimal)thePitch))
            {
                //代入時のイベントロック
                //MSPitchEvntLock = true;
                cwneMSPitch.Value = (decimal)thePitch;
                //MSPitchEvntLock = false;
            }
            //追加2015/01/29hata
            else if (cwneMSPitch.Maximum < (decimal)thePitch)
            {
                cwneMSPitch.Value = cwneMSPitch.Maximum;
            }
            else if (cwneMSPitch.Minimum > (decimal)thePitch)
            {
                cwneMSPitch.Value = cwneMSPitch.Minimum;
            }

			//変更した場合，ＯＫボタンを使用可にする     'v15.10追加 byやまおか 2010/01/08
			if (this.ActiveControl == cmdSetScanPitch) cmdOK.Enabled = true;
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
		//履　　歴： V14.1  07/08/02   やまおか      新規作成
		//*******************************************************************************
		private void cmdMeasureFrImg_Click(object sender, EventArgs e)
		{
			//  Dim FileName()  As String   'ダイアログボックスのファイル名
			//  Dim KeyName     As String   'ファイル名検索用のキー
			//
			//  '開くﾁｪｯｸがあれば
			//  If chkImgOpen.Value = 1 Then
			//      '画像ファイル選択ダイアログ表示
			//      If Not SelectImageFile(FileName) Then Exit Sub
			//      KeyName = FileName(1)
			//      '画像付帯情報の表示(画像ファイル名をコモンに書き込み後、実行)
			//      frmInformation.LoadImageInfo RemoveExtension(RemoveNull(KeyName), ".img")
			//  '開くﾁｪｯｸがなければ
			//  Else
			//      '画像を表示していないときは、メッセージを表示する
			//      If ImageDisplayFlag = False Then
			//          '画像を表示してください
			//          If MsgBox(LoadResString(12837), vbInformation) = vbOK Then Exit Sub
			//      End If
			//  End If
			//
			//  '[画像から測定]フラグを立てる
			//  MeaFrImgFlg = 2
			//
			//  '再構成リトライを隠す
			//  Me.hide
			//
			//  '寸法測定フォームを表示
			//  frmDistance.Show

			//v15.0変更 by 間々田 2009/02/27
			string FileName = null;

			//開くチェックがあれば
			if (chkImgOpen.CheckState == CheckState.Checked)
			{
				//画像ファイル選択ダイアログ表示
				FileName = modFileIO.GetFileName(StringTable.IDS_Select, CTResources.LoadResString(StringTable.IDS_CTImage), ".img");
				if (string.IsNullOrEmpty(FileName)) return;

				//選択した画像を表示
				frmScanImage.Instance.Target = FileName;
			}
			//「開く」チェックがない場合で画像を表示していないとき
			else if (string.IsNullOrEmpty(frmScanImage.Instance.Target))
			{
				//'メッセージを表示：測定するには、画像を表示してください。  'v17.00削除 byやまおか 2010/02/24
				//MsgBox LoadResString(12837), vbInformation
				//Exit Sub

				//画像ファイル選択ダイアログ表示     'v17.00追加 byやまおか 2010/02/24
				FileName = modFileIO.GetFileName(StringTable.IDS_Open, CTResources.LoadResString(StringTable.IDS_CTImage), ".img");
				if (!string.IsNullOrEmpty(FileName))
				{
					frmScanImage.Instance.Target = FileName;
				}
				else
				{
					return;
				}
			}

			//「画像から測定」ボタンを使用不可にする
			//cmdMeasureFrImg.Enabled = False
            //Rev20.00 追加 by長野 2014/12/04
            this.TopMost = false;
            this.WindowState = FormWindowState.Minimized;
			this.Enabled = false;

			//ROI制御スタート：角度測定
			//frmScanImage.ImageProc = RoiAngle
			frmScanControl frmScanControl = frmScanControl.Instance;
			frmScanControl.cmdImageProc_Click(frmScanControl.cmdImageProc[frmScanControl.ImageProcRoiAngle], EventArgs.Empty);	//v17.00変更 byやまおか 2010/02/24
		}


		//*******************************************************************************
		//機　　能： 積算枚数変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cwneInteg_ValueChanged(object sender, EventArgs e)
		{
            //if (IntegEvntLock) return;   //代入時のイベントロック 追加2014/06/23(検S1)hata
            if (EvntLock) return;   //Load時のイベントロック 追加2014/06/23(検S1)hata
            
            //最大最小ビュー数の更新
			UpdateViewMinMax();

   		
            //v19.10 追加 by長野 2012/07/30
			//If scaninh.smooth_rot_cone = 0 And optTableRotation(1).Value = True Then
			//v19.15 シングル連続時にも通過するように修正 by長野 2013/06/15
			//v19.17 条件式変更 by長野　2013/09/17
			//If ((Not IsCone) Or scaninh.smooth_rot_cone = 0) And optTableRotation(1).Value = True Then
            //変更2014/10/07hata_v19.51反映
            //if (CTSettings.scaninh.Data.smooth_rot_cone == 0 && optTableRotation1.Checked == true)
            if (optTableRotation[1].Checked == true)
			{
				//v19.01 連続の場合 ビュー数リスト更新 by長野 2012/05/21
				CreateViewListComboBox();

				//cwneViewNumに保存されているビュー数に、最も近いビュー数を選択されているindexとする。
				cmbViewNum.SelectedIndex = SetViewListIndex();
			}

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == cwneInteg) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： マルチスキャン・スキャンピッチ変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cwneMSPitch_ValueChanged(object sender, EventArgs e)
		{
            //if (MSPitchEvntLock) return;   //代入時のイベントロック 追加2014/06/23(検S1)hata
            if (EvntLock) return;   //Load時のイベントロック 追加2014/06/23(検S1)hata
            
            //マルチスキャンのスライス数上下限値を設定
			SetSliceNumRange();

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == cwneMSPitch) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： マルチスキャン・スライス数変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cwneMSSlice_ValueChanged(object sender, EventArgs e)
		{
            //if (MSSliceEvntLock) return;   //代入時のイベントロック 追加2014/06/23(検S1)hata
            if (EvntLock) return;   //Load時のイベントロック 追加2014/06/23(検S1)hata
            
            //マルチスキャンのスキャンピッチ上下限値を設定
			SetScanPitchRange();

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == cwneMSSlice) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： バイアス変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void ntbBias_ValueChanged(object sender, EventArgs e)		// 【C#コントロールで代用】
		{
			//変更した場合，ＯＫボタンを使用可にする
			if (this.Visible) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： スロープ変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void ntbSlope_ValueChanged(object sender, EventArgs e)		// 【C#コントロールで代用】
		{
			//変更した場合，ＯＫボタンを使用可にする
			if (this.Visible) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： マルチスライス：スライスピッチフォーカス取得時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void ntbMultislicePitch_Enter(object sender, EventArgs e)
		{
			//メッセージを表示：スライスピッチの変更は、マルチスライス校正で行ってください。
			MessageBox.Show(CTResources.LoadResString(9413), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			//    cmdOK.SetFocus
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
		private void cwneSlice_ValueChanged(object sender, EventArgs e)
		{
            //if (SliceEvntLock) return;   //代入時のイベントロック 追加2014/06/23(検S1)hata
            if (EvntLock) return;   //Load時のイベントロック 追加2014/06/23(検S1)hata
            
            //コーンの場合
			if (IsCone)
			{
				//各パラメータの再計算
				SW_Change();
			}

			float r = 0;
			r = (IsCone ? SWMinCone : SWMin);

			//スライス厚（画素）側も更新
            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneSlicePix.Value = cwneSlice.Value / (decimal)r;
            decimal fSlicePix = cwneSlice.Value / (decimal)r;
            //Rev20.00 追加 四捨五入 by長野 2015/01/26
            //Rev26.30/Rev26.15 del by chouno 2018/10/15
            //fSlicePix = Math.Round(fSlicePix / cwneSlicePix.Increment,MidpointRounding.AwayFromZero) * cwneSlicePix.Increment;

            if ((cwneSlicePix.Maximum >= fSlicePix) && (cwneSlicePix.Minimum <= fSlicePix))
            {
                //代入時のイベントロック
                //SlicePixEvntLock = true;
                cwneSlicePix.Value = fSlicePix;
                //SlicePixEvntLock = false;
            }
            //追加2015/01/29hata
            else if (cwneSlicePix.Maximum < fSlicePix)
            {
                cwneSlicePix.Value = cwneSlicePix.Maximum;
            }
            else if (cwneSlicePix.Minimum > fSlicePix)
            {
                cwneSlicePix.Value = cwneSlicePix.Minimum;
            }

			//透視画像のラインを更新する
			UpdateLine();

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == cwneSlice) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： スライス厚（画素）変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cwneSlicePix_ValueChanged(object sender, EventArgs e)
		{
            //if (SlicePixEvntLock) return;   //代入時のイベントロック 追加2014/06/23(検S1)hata
            if (EvntLock) return;   //Load時のイベントロック 追加2014/06/23(検S1)hata
           
            float r = 0;
			r = (IsCone ? SWMinCone : SWMin);

			//スライス厚（mm）側も更新
#region 【C#コントロールで代用】
/*
			With cwneSlice
				.Value = Val(Format$(Value * r, .FormatString))
			End With
*/
#endregion

			string cwneSliceFormat = string.Format("F{0}", cwneSlice.DecimalPlaces);

			decimal value = 0;
			decimal.TryParse((cwneSlicePix.Value * (decimal)r).ToString(cwneSliceFormat), out value);
            
            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneSlice.Value = value;
            if ((cwneSlice.Maximum >= value) && (cwneSlice.Minimum <= value))
            {
                //Rev20.00 コーンの場合は、自動で変更しない by長野 2015/01/26
                //Rev20.00 元に戻す by長野 2015/02/05
                //if (!IsCone)
                //{
                    //代入時のイベントロック
                    //SliceEvntLock = true;
                    cwneSlice.Value = value;
                    //SliceEvntLock = false;
                //}
            }
            //追加2015/01/29hata
            else if (cwneSlice.Maximum < value)
            {
                cwneSlice.Value = cwneSlice.Maximum;
            }
            else if (cwneSlice.Minimum > value)
            {
                cwneSlice.Value = cwneSlice.Minimum;
            }

            //変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == cwneSlicePix) cmdOK.Enabled = true;
		}

        //*******************************************************************************
        //機　　能： スライス厚（画素）変更時処理（Load時にも動かせるようにするため）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v20.00  15/02/15   (検S1)長野      新規作成
        //*******************************************************************************
        private void cwneSlicePix_ValueChangedNoEvent()
        {
            //if (SlicePixEvntLock) return;   //代入時のイベントロック 追加2014/06/23(検S1)hata
            //if (EvntLock) return;   //Load時のイベントロック 追加2014/06/23(検S1)hata

            float r = 0;
            r = (IsCone ? SWMinCone : SWMin);

            //スライス厚（mm）側も更新
            #region 【C#コントロールで代用】
            /*
			With cwneSlice
				.Value = Val(Format$(Value * r, .FormatString))
			End With
*/
            #endregion

            string cwneSliceFormat = string.Format("F{0}", cwneSlice.DecimalPlaces);

            decimal value = 0;
            decimal.TryParse((cwneSlicePix.Value * (decimal)r).ToString(cwneSliceFormat), out value);

            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneSlice.Value = value;
            if ((cwneSlice.Maximum >= value) && (cwneSlice.Minimum <= value))
            {
                //Rev20.00 コーンの場合は、自動で変更しない by長野 2015/01/26
                //Rev20.00 元に戻す by長野 2015/02/05
                //if (!IsCone)
                //{
                //代入時のイベントロック
                //SliceEvntLock = true;
                cwneSlice.Value = value;
                //SliceEvntLock = false;
                //}
            }
            //追加2015/01/29hata
            else if (cwneSlice.Maximum < value)
            {
                cwneSlice.Value = cwneSlice.Maximum;
            }
            else if (cwneSlice.Minimum > value)
            {
                cwneSlice.Value = cwneSlice.Minimum;
            }

            //変更した場合，ＯＫボタンを使用可にする
            if (this.ActiveControl == cwneSlicePix) cmdOK.Enabled = true;
        }

		//*************************************************************************************************
		//機　　能： ビュー数変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void cwneViewNum_ValueChanged(object sender, EventArgs e)
		{
            decimal val1 = 0;
            if (optTableRotation0.Checked) //Rev20.00 テーブルステップ回転時のみ実行する by長野 2014/09/11
            {
                //数字のインクリメントを合わせる10,100,200･･･
                val1 = cwneViewNum.Value / (decimal)100.0;
                val1 = Math.Round(val1, 0, MidpointRounding.AwayFromZero) * 100;
                if (val1 < cwneViewNum.Minimum) val1 = cwneViewNum.Minimum;
                if (val1 > cwneViewNum.Maximum) val1 = cwneViewNum.Maximum;
                if (cwneViewNum.Value != val1)
                {
                    cwneViewNum.Value = val1;
                    return;
                }
            }
            
            //if (ViewNumEvntLock) return;	//代入時のイベントロック 追加2014/06/23(検S1)hata
            if (EvntLock) return;   //Load時のイベントロック 追加2014/06/23(検S1)hata
            
            //最大積算枚数の更新
			UpdateIntegMax();

			//コーン時：データ収集ビュー数の更新
			if (IsCone) UpdateAcqView();

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == cwneViewNum) cmdOK.Enabled = true;
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
		private void frmScanCondition_Load(object sender, EventArgs e)
		{
            //tes中!!_畑_2014/09/11
            EvntLock = true;
            
            //削除2015/03/18hata
            //------------------------------------------------------------------------------------
            //　Setup処理でLoad済み後にLoadを通るため、設定済み変数がnullに初期化されてしまう。
            //　変数初期化はコンストラクタに移す。
            //
            ////変数初期化
            ////Rev20.00 Loadのたびに初期化しない。C#ではソフト起動時にnullで初期化されている。
            ////Loadのたびにnullにすると途中で計算がおかしくなってしまう by長野 2014/12/04
            ////Rev20.00 元々nullで動いていたはずなので復活 by長野 2015/01/26
            //ksw = null;
            //myContTime = null;
            //myConeSlicePitchPix = null;
            //------------------------------------------------------------------------------------

            //追加2014/09/11_dNet対応_hata
            //選択されていないTabのVisibleがFalse になるための処置
            sstMenu.SelectedIndex = 0;
            for (int i = 1; i <= fraMenu.GetUpperBound(0); i++)
            {
                fraMenu[i].Parent = this;
                fraMenu[i].Top = sstMenu.Top + sstMenu.ItemSize.Height;
                fraMenu[i].Left = sstMenu.Left;
            }

			//フラグをセット
			modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTScanCondition;

			//v17.60 英語用レイアウト調整 by 長野　2011/05/25
			if (modCT30K.IsEnglish == true)
			{
				EnglishAdjustLayout();
			}

			//キャプションのセット
			SetCaption();

			//フォームの表示位置                             'v15.0変更 by 間々田 2009/02/27
			modCT30K.SetPosNormalForm(this);

			//scancondpar（コモン）の読み込み                'v11.2追加 by 間々田 2005/10/06
			//modScancondpar.CallGetScancondpar();
            CTSettings.scancondpar.Load(CTSettings.scaninh.Data.rotate_select);

			//コーンか？                                     'v15.0追加 by 間々田 2009/01/15 'v15.0削除 by 間々田 2009/06/17
			//IsCone = (scansel.data_mode = DataModeCone)

			//コーンの場合、必要なパラメータを取得
			if (IsCone) GetParaForCone();

			//コントロールの初期化
			InitControls();

			//スキャン条件の一時保存
            ScanselOrg = new CTstr.SCANSEL();
            ScanselOrg.Initialize();
            ScanselOrg = CTSettings.scansel.Data;

			//各コントロールに値をセットする
			SetControls();

			//メカ制御画面への参照
            if (myMechaControl == null) //追加2014/10/28hata
            {
                myMechaControl = frmMechaControl.Instance;
                myMechaControl.FCDChanged += new EventHandler(myMechaControl_FCDChanged);
                myMechaControl.FIDChanged += new EventHandler(myMechaControl_FIDChanged);
                myMechaControl.UDPosChanged += new EventHandler(myMechaControl_UDPosChanged);
            }
			//Ｘ線制御画面への参照
			//v15.10条件追加 byやまおか 2009/10/28
            if (CTSettings.scaninh.Data.xray_remote == 0)
            {
                if (myXrayControl == null) //追加2014/10/28hata
                {
                    myXrayControl = frmXrayControl.Instance;
                    myXrayControl.Changed += new EventHandler<frmXrayControl.ChangedEventArgs>(myXrayControl_Changed);
                }
				
                changedEventArgs.volt = frmXrayControl.Instance.ntbSetVolt.Value;
				changedEventArgs.current = frmXrayControl.Instance.ntbSetCurrent.Value;

				myXrayControl_Changed(this, changedEventArgs);
			}

            //tes中!!_畑_2014/09/11
            //2014/07/07(検S1)hata
            //NoVisibleEvntLock=falseなら EvntLockを解除
            //if (!NoVisibleEvntLock) EvntLock = false;
            EvntLock = false;
		
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
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void frmScanCondition_FormClosed(object sender, FormClosedEventArgs e)
		{

            //イベントロックを解除　//2014/07/07(検S1)hata
            //NoVisibleEvntLock = false;
            EvntLock = false;

			//メカ制御画面への参照破棄
            if (myMechaControl != null)
            {
                myMechaControl.FCDChanged -= myMechaControl_FCDChanged;
                myMechaControl.FIDChanged -= myMechaControl_FIDChanged;
                myMechaControl.UDPosChanged -= myMechaControl_UDPosChanged;
                myMechaControl = null;
            }
			//Ｘ線制御画面への参照破棄
            if (myXrayControl != null)
            {
                myXrayControl.Changed -= myXrayControl_Changed;
                myXrayControl = null;
            }
			//終了時はフラグをリセット
			modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTScanCondition);

			//透視画像上のラインを更新
			//if (modLibrary.IsExistForm(frmTransImage.Instance))
            if (modLibrary.IsExistForm("frmTransImage"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            {
				frmTransImage.Instance.SetLine();
			}
		}


        private void CallClosing()
        {
            //マウスポインタを砂時計にする
            this.Cursor = Cursors.Default;

            if (this.WindowState == FormWindowState.Minimized)
            {

                //最小化時はフラグをリセット
                modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTScanCondition);

                //透視画像上のラインを更新
                //if (modLibrary.IsExistForm(frmTransImage.Instance))
                if (modLibrary.IsExistForm("frmTransImage"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
                {
                    frmTransImage.Instance.SetLine();
                }
            }
            else
            {
                this.Close();
            }
        }

		//*************************************************************************************************
		//機　　能： 各コントロールの位置・サイズ等の初期化
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.2  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void InitControls()
		{
            //Rev26.00 プリセット表示用でなければ、通常の表示項目・位置に変更 by chouno 2017/08/31
            if (m_PresetSettingFlg == false)
            {
                this.Size = new Size(750, 581);
                sstMenu.Location = new Point(8,8);
                cmdSaveCondition.Location = new Point(12,512);
                fraPicSize.Location = new Point(120, 503);
                fraFpdGainInteg.Location = new Point(232,503);
                cmdOK.Location = new Point(520,508);
                CmdCancel.Location = new Point(632,508);

                lblPresetComment.Visible = false;
                lblPresetCommentColon.Visible = false;
                txtPresetComment.Visible = false;
                lblPresetName.Visible = false;
                lblPresetNameColon.Visible = false;
                txtPresetName.Visible = false;
                cmdSaveCondition.Visible = false;
                cmdDeleteCondition.Visible = false;
                cmdOK.Visible = true;
            }

			//スキャンモード
            //変更2014/10/07hata_v19.51反映
            //optScanMode1.Visible = (CTSettings.scaninh.Data.scan_mode[0] == 0);			//ハーフ
            //optScanMode2.Visible = (CTSettings.scaninh.Data.scan_mode[1] == 0);			//フル
            //optScanMode3.Visible = (CTSettings.scaninh.Data.scan_mode[2] == 0);			//オフセット
            optScanMode[1].Enabled = (CTSettings.scaninh.Data.scan_mode[0] == 0);       //'ハーフ             'v18.00変更 byやまおか 2011/02/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            optScanMode[2].Enabled = (CTSettings.scaninh.Data.scan_mode[1] == 0);       // 'フル               'v18.00変更 byやまおか 2011/02/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            optScanMode[3].Enabled = (CTSettings.scaninh.Data.scan_mode[2] == 0);       //'オフセット         'v18.00変更 byやまおか 2011/02/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //optScanMode[4].Enabled = (CTSettings.scaninh.Data.scan_mode[3] == 0);       //'シフトオフセット   'v18.00追加 byやまおか 2011/02/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev25.00 Wスキャン時はシフト可能だが隠す by長野 2016/06/1
            optScanMode[4].Enabled = (CTSettings.scaninh.Data.scan_mode[3] == 0 || CTSettings.W_ScanOn);

            //'スキャンモードオプションボタンの表示と位置調整
            modLibrary.RePosOption(optScanMode);     //'v18.00追加 byやまおか 2011/02/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //Rev25.00 WスキャンON時は、シフトオフセットを隠す　(シフトオフセットとの両立は無) by長野 2016/06/19
            if (CTSettings.scaninh.Data.scan_mode[3] == 1 && CTSettings.W_ScanOn)
            {
                optScanMode[4].Visible = false;
                optScanMode[4].Enabled = false;//Rev26.10 add by chouno 20178/01/05
                chkW_Scan.Visible = true;
                chkW_Scan.Location = new Point(optScanMode[1].Left, optScanMode[1].Top);
                for (int cnt = 1; cnt < 4; cnt++)
                {
                    optScanMode[cnt].Location = new Point(optScanMode[cnt + 1].Left, optScanMode[cnt + 1].Top);
                }

            }

#if NoHalfCone		//LG化学向け added by 間々田 2004/03/25
			//LG化学向けでは、コーンビームのハーフスキャンは設定できないようにする
            optScanMode[(int)ScanSel.ScanModeConstants.ScanModeHalf].Enabled = !IsCone;

			//スキャンモードオプションボタンの調整
			modLibrary.CorrectOption(optScanMode);
#endif

			//マルチスキャンモード
            optMultiScanMode1.Enabled = ((IsCone ? CTSettings.scaninh.Data.cone_multiscan_mode[0] : CTSettings.scaninh.Data.multiscan_mode[0]) == 0);
            optMultiScanMode3.Enabled = ((IsCone ? CTSettings.scaninh.Data.cone_multiscan_mode[1] : CTSettings.scaninh.Data.multiscan_mode[1]) == 0);
            optMultiScanMode5.Enabled = ((IsCone ? CTSettings.scaninh.Data.cone_multiscan_mode[2] : CTSettings.scaninh.Data.multiscan_mode[2]) == 0);

			optMultiScanMode1.Visible = optMultiScanMode1.Enabled;
			optMultiScanMode3.Visible = optMultiScanMode3.Enabled;
			optMultiScanMode5.Visible = optMultiScanMode5.Enabled;

			//マルチスキャンモードオプションボタンの調整 'v15.10追加 byやまおか 2009/11/26
			modLibrary.RePosOption(optMultiScanMode);

			//昇降位置(絶対座標)の値が昇降上下限値の範囲外の場合はマルチスキャンは使用不可
			//optMultiScanMode(3).Enabled = InRangeFloat(mecainf.udab_pos, GValLowerLimit, GValUpperLimit) あとで

			//マルチスキャンモードオプションボタンの調整
			//Call CorrectOption(optMultiScanMode)
			modLibrary.RePosOption(optMatrix);

            //マトリクスサイズ
            //Rev20.00 表示･非表示の制御を変更 by長野 2015/01/16
            //optMatrix1.Enabled = (CTSettings.scaninh.Data.scan_matrix[0] == 0) && IsCone;		// 256x 256
            //optMatrix2.Enabled = (CTSettings.scaninh.Data.scan_matrix[1] == 0);					// 512x 512
            //optMatrix3.Enabled = (CTSettings.scaninh.Data.scan_matrix[2] == 0);					//1024x1024
            ////optMatrix4.Enabled = (CTSettings.scaninh.Data.scan_matrix[3] == 0) && (!IsCone);		//2048x2048
            ////rev20.00 test コーン2048 by長野 2015/01/09
            //optMatrix4.Enabled = (CTSettings.scaninh.Data.scan_matrix[3] == 0);		//2048x2048
            ////optMatrix5.Enabled = (CTSettings.scaninh.Data.scan_matrix[4] == 0) && (!IsCone);		//4096x4096 '4096対応 v16.10 by 長野　10/01/29
            ////Rev20.00 test コーン4096 by長野 2015/01/09
            //optMatrix5.Enabled = (CTSettings.scaninh.Data.scan_matrix[4] == 0) && (!IsCone);		//4096x4096 '4096対応 v16.10 by 長野　10/01/29

            //Rev20.00 表示･非表示の制御を変更 by長野 2015/01/09
            if (IsCone)
            {
                optMatrix1.Enabled = (CTSettings.scaninh.Data.cone_matrix[0] == 0);		// 256x 256
                optMatrix2.Enabled = (CTSettings.scaninh.Data.cone_matrix[1] == 0);		// 512x 512
                optMatrix3.Enabled = (CTSettings.scaninh.Data.cone_matrix[2] == 0);		//1024x1024
                optMatrix4.Enabled = (CTSettings.scaninh.Data.cone_matrix[3] == 0);		//2048x2048
                optMatrix5.Enabled = (CTSettings.scaninh.Data.cone_matrix[4] == 0);		//4096x4096 '4096対応 v16.10 by 長野　10/01/29
            }
            else
            {
                optMatrix1.Enabled = (CTSettings.scaninh.Data.scan_matrix[0] == 0);		// 256x 256
                optMatrix2.Enabled = (CTSettings.scaninh.Data.scan_matrix[1] == 0);		// 512x 512
                optMatrix3.Enabled = (CTSettings.scaninh.Data.scan_matrix[2] == 0);		//1024x1024
                optMatrix4.Enabled = (CTSettings.scaninh.Data.scan_matrix[3] == 0);		//2048x2048
                optMatrix5.Enabled = (CTSettings.scaninh.Data.scan_matrix[4] == 0);		//4096x4096 '4096対応 v16.10 by 長野　10/01/29
            }

            //Rev20.00 全ラジオボタンを上に動かすため変更 by長野 2015/01/16
            //マトリクスサイズオプションボタンの位置の調整
            //modLibrary.RePosOption(optMatrix);

            //マトリクスサイズオプションボタンの位置の調整
            modLibrary.RePosOption2(optMatrix, 4);

			//ビニング
            fraBinning.Visible = (CTSettings.scaninh.Data.binning == 0);
            optBinning0.Visible = (CTSettings.scaninh.Data.bin_char[0] == 0);
            optBinning1.Visible = (CTSettings.scaninh.Data.bin_char[1] == 0);
			optBinning2.Visible = (CTSettings.scaninh.Data.bin_char[2] == 0);

			//往復スキャンフレーム
			fraRoundTrip.Visible = (CTSettings.scaninh.Data.round_trip == 0);

			//オートプリントフレーム
			fraAutoPrint.Visible = (CTSettings.scaninh.Data.auto_print == 0);

			//フィルタ処理
			fraFilterProcess.Visible = (CTSettings.scaninh.Data.filter_process[0] == 0) && (CTSettings.scaninh.Data.filter_process[1] == 0);

			//RFC    'v14.2 by YAMAKAGE 2008/04/08
			fraRFC.Visible = (CTSettings.scaninh.Data.rfc == 0);

			//ヘリカルモード
			//fraHelicalMode.Visible = (.helical = 0) And IsCone
			//ヘリカルは行わないため常に表示しない
			fraHelicalMode.Visible = false;						//v17.00変更 byやまおか 2010/02/19

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//コーン分散処理：コーンビーム時のみ表示
			//        fraConeDistribute.Visible = ((.cone_distribute = 0) Or (.cone_distribute2 = 0)) And IsCone
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//マルチスライス：ノーマルスキャン時のみ表示
			fraMultislice.Visible = (CTSettings.scaninh.Data.multislice == 0) && (!IsCone);

			//テーブル回転・連続オプション   'v7.0 added by 間々田 2003/09/26 コーンビーム時は外部トリガ可能な場合のみ表示
			//If False Then   'Rev16.2 コーンビーム連続回転ありの場合False,なしの場合True コモン追加までの仮対策 by 山影
			//        If True Then    'v16.20修正 出荷物件に連続回転コーンが不要なためとりあえず元に戻す byやまおか 2010/05/19
			//                optTableRotation(1).Visible = (Not IsCone) Or (.ext_trig = 0)
			//        End If
			//v17.40 条件変更　連続コーンはコモンを見るように変更 by長野 2010/10/21
            //optTableRotation[1].Visible = (!IsCone) || (CTSettings.scaninh.Data.ext_trig == 0) || (CTSettings.scaninh.Data.smooth_rot_cone == 0);//変更2014/10/07hata_v19.51反映
            //'シフトの場合も連続回転を無効にする     'v18.00変更 byやまおか 2011/03/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            optTableRotation[1].Visible = (!IsCone) || (CTSettings.scaninh.Data.ext_trig == 0) || ((optScanMode[(int)ScanSel.ScanModeConstants.ScanModeShift].Checked));
         
			//v19.00 (電S2)永井
			//タブ表示
            //変更2015/01/27hata
			//tabPage2.Visible = (CTSettings.scaninh.Data.mbhc == 0);

            //Rev26.00 ファントムレスBHCと従来BHCの有無により、画面の表示を切り替える by井上 2017/01/16
            tabPage.TabVisible(2, (CTSettings.scaninh.Data.mbhc == 0 || CTSettings.scaninh.Data.mbhc_phantomless == 0));
            if (CTSettings.scaninh.Data.mbhc == 0 || CTSettings.scaninh.Data.mbhc_phantomless == 0)
            {
                fraMenu2.Parent = sstMenu.TabPages[2];
                fraMenu2.Location = new Point(5, 2);
            }
            else
            {
                fraMenu2.Parent = this;
                fraMenu2.Top = sstMenu.Top + sstMenu.ItemSize.Height;
                fraMenu2.Left = sstMenu.Left;
            }
            
            fraBHC.Visible = (CTSettings.scaninh.Data.mbhc == 0);
            //Rev26.00 追加 by 井上 2017/02/08
            fraBHCPhantomless.Visible = (CTSettings.scaninh.Data.mbhc_phantomless == 0);
            if (CTSettings.scaninh.Data.mbhc_phantomless == 0)
            {
                fraBHCPhantomless.Location = new Point(fraBHC.Left, fraBHC.Top);

                //コンボボックスにファントムレスBHCの材質追加
                cmbBHCPhantomless.Items.Clear();
                int BHCmat_cnt = 0;
                while ( BHCmat_cnt < modBHC.BHCmatnum)
                {
                    cmbBHCPhantomless.Items.Add(modBHC.BHCMaterial[BHCmat_cnt]);
                    BHCmat_cnt++;
                }
                
            }

            //Rev26.11(特) 修正 ガイドモードの時のみON
            if (CTSettings.scaninh.Data.guide_mode == 1)
            {
                //登録ボタンを表示 by井上 2018/1/15
                cmdSaveCondition.Visible = true;
            }

			//透視画像保存：コーンビーム時は非表示
            //Rev20.00 コーンでも表示する by長野 2015/02/06
			//fraFluoroImageSave.Visible = (!IsCone);
            fraFluoroImageSave.Visible = true;

			//画像階調最適化：コーンビーム時は非表示
			fraContrastFitting.Visible = (!IsCone);

			//データ収集ビュー数
			fraAcqView.Visible = IsCone;

			//v19.10 追加 連続回転用のComboBox by長野 2012/07/30
			cmbViewNum.Width = cwneViewNum.Width;
			cmbViewNum.Left = cwneViewNum.Left;
			cmbViewNum.Top = cwneViewNum.Top;

			if (optTableRotation1.Checked == true)
			{
				cwneViewNum.Visible = false;
#region 【C#コントロールで代用】
/*
				cwneViewNum.DiscreteInterval = 1
*/
#endregion
				cwneViewNum.Increment = 1;

            	cmbViewNum.Visible = true;
			}
			else
			{
				cwneViewNum.Visible = true;
#region 【C#コントロールで代用】
/*
				cwneViewNum.DiscreteInterval = 100
				cwneViewNum.SetMinMax GVal_ViewMax, GVal_ViewMax
*/
#endregion
				cwneViewNum.Increment = 100;
                cwneViewNum.Minimum = CTSettings.GVal_ViewMin;
                cwneViewNum.Maximum = CTSettings.GVal_ViewMax;
				cmbViewNum.Visible = false;
			}

			//PkeFPDのときはフラットパネル設定を表示 'v17.00追加 byやまおか 2010/02/17
			//fraFpdGainInteg.Visible = (DetType = DetTypePke)
			fraFpdGainInteg.Visible = false;						//v17.02変更 frmScanControlへ移動したため byやまおか 2010/07/16

			//フレームの境界線を消す
			fraMenu0.BorderStyle = BorderStyle.None;
			fraMenu1.BorderStyle = BorderStyle.None;
			fraMenu2.BorderStyle = BorderStyle.None;					//v19.00 (電S2)永井
			fraSliceWidthMM.BorderStyle = BorderStyle.None;
			fraSliceWidthPix.BorderStyle = BorderStyle.None;
			fraConeBeamMulti.BorderStyle = BorderStyle.None;			//v11.4追加 by 間々田 2006/03/13


			//スライスプランテーブルフレームの位置調整
			fraSlicePlanName.Location = new Point(fraMulti.Left, fraMulti.Top);

			//マルチスライスフレームの位置調整
			fraMultislice.Location = new Point(fraSlicePitch.Left, fraSlicePitch.Top);

			//オートズーミングフレームの位置調整
			fraZoomingPlan.Location = new Point(fraReconRange.Left, fraReconRange.Top);

			//画質フレームの位置調整
			fraImageMode.Location = new Point(fraContrastFitting.Left, fraContrastFitting.Top);


			//コーンの場合スライス厚の画素欄は非表示とする
			if (IsCone)
			{
				fraSliceWidthPix.Visible = false;
				//位置の調整
                //2014/11/07hata キャストの修正
                //fraSliceWidthMM.Top = fraSlice.Height / 3;
                fraSliceWidthMM.Top = Convert.ToInt32(fraSlice.Height / 3F);
				lblSliceMinMax.Top = fraSliceWidthMM.Top + fraSliceWidthMM.Height;
			}

			//最初に「条件設定」のタブを表示
			sstMenu.SelectedIndex = 0;
		}


		//*************************************************************************************************
		//機　　能： オートセンタリングオプションボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   0:なし, 1:あり
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void optAutoCentering_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
			int Index = Array.IndexOf(optAutoCentering, sender);
			if (Index < 0) return;

			//オートセンタリングありの場合はスキャンエリアを「自動設定」と表示する  added by 山本　2003-2-18
			lblACcomment.Visible = (Index == 1);
			//lblFai.Visible = (Index = 0)       'v16.30削除 byやまおか 2010/05/25
			//cwneArea.Visible = (Index = 0)     'v16.30削除 byやまおか 2010/05/25
			//lblAreaUni.Visible = (Index = 0)   'v16.30削除 byやまおか 2010/05/25
			lblAreaMinMax.Visible = (Index == 0);

			//常に最大とするためスキャンエリア設定値のコントロールを非表示にする
			cwneArea.Enabled = false;				//v16.30追加 byやまおか 2010/05/25
			cwneArea.Visible = false;				//v16.30追加 byやまおか 2010/05/25
			lblFai.Visible = false;					//v16.30追加 byやまおか 2010/05/25
			lblAreaUni.Visible = false;				//v16.30追加 byやまおか 2010/05/25
			lblAreaMinMax.Top = lblACcomment.Top;	//v16.30追加 byやまおか 2010/05/25

			//コーン時：データ収集ビュー数の更新
			if (IsCone) UpdateAcqView();

			//KSWの更新
			if (IsCone) UpdateKSW();

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == optAutoCentering[Index]) cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： ビニングオプションボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void optBinning_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
			int Index = Array.IndexOf(optBinning, sender);
			if (Index < 0) return;

            //透視画像サイズの調整
            ntbTransImageWidth.Value = (decimal)(CTSettings.scancondpar.Data.h_size / CTSettings.scancondpar.Data.h_mag[Index]);
            ntbTransImageHeight.Value = (decimal)(CTSettings.scancondpar.Data.v_size / CTSettings.scancondpar.Data.v_mag[Index]);
            
            //最大最小ビュー数の更新
			UpdateViewMinMax();

			//最大積算枚数の更新
			UpdateIntegMax();

			//最大・最小スライス厚の更新
			UpdateSliceMinMax();

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == optBinning[Index]) cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： コーン分散処理オプションボタン選択時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//Private Sub optConeDistribute_Click(Index As Integer)
		//
		//    ''コーン分散処理有効時は、生データ保存ありを選択できない
		//    ''If optConeDistribute(1).Value And (chkSave.Value = vbChecked) Then
		//    'If optConeDistribute(1).value And (chkSave.value = vbChecked) And (scaninh.cone_distribute = 0) Then 'v10.0変更 by 間々田 2005/02/02 コーン分散処理２の場合、設定できるようにした
		//    '    'メッセージ表示：コーン分散処理有効時は、生データ保存ありを選択できません。強制的に「生データ保存なし」に設定します。
		//    '    MsgBox LoadResString(12919), vbExclamation
		//    '    chkSave.value = vbUnchecked
		//    'End If
		//
		//    'v15.0変更 by 間々田 2009/03/06
		//    If optConeDistribute(1).Value And IsRawDataSave And (scaninh.cone_distribute = 0) Then
		//        'メッセージ表示：
		//        '   生データ保存する場合は、コーン分散処理を「有効」に設定できません。元に戻します。
		//        MsgBox LoadResString(9930), vbExclamation
		//        optConeDistribute(0).Value = True
		//    End If
		//
		//    '変更した場合，ＯＫボタンを使用可にする
		//    If Me.ActiveControl Is optConeDistribute(Index) Then cmdOK.Enabled = True
		//
		//End Sub
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


		//*************************************************************************************************
		//機　　能： 「画像階調最適化」オプションボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   0:なし, 1:あり
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.5  2006/09/01  (WEB)間々田  新規作成
		//*************************************************************************************************
		private void optContrastFitting_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
			int Index = Array.IndexOf(optContrastFitting, sender);
			if (Index < 0) return;

			//スキャン中再構成を行わない場合
			if ((!IsCone) && optScanAndView0.Checked && (Index == 1))
			{
				//メッセージ表示：
				//   スキャン中再構成を行わない場合は画像階調最適化ができません。
				//   画像階調最適化を行う場合は、先にスキャン中再構成を行うに設定してください。
				MessageBox.Show(StringTable.GetResString(9410, fraContrastFitting.Text), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

				//強制的になしにする
				modLibrary.SetOption(optContrastFitting, 0);
			}

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == optContrastFitting[Index]) cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： オートセンタリングを選択可・不可を設定する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
        //変更2014/10/07hata_v19.51反映
        //private void AutoCenteringEnabled()
		public void AutoCenteringEnabled()
		{
			//オートセンタリングが不可の場合は、無視
			if (CTSettings.scaninh.Data.auto_centering != 0) return;

#if NoHalfAutoCentering
			//ヘリカルスキャンまたはハーフスキャンが選択されているときは、オートセンタリングを「なし」に固定
            optAutoCentering1.Enabled = !((IsCone && optHelical1.Checked) || optScanMode1.Checked);
#else
			//ヘリカルスキャンが選択されているときは、オートセンタリングを「なし」に固定
			optAutoCentering1.Enabled = !(IsCone && optHelical1.Checked);
#endif

			//オートセンタリングオプションボタンの調整
			modLibrary.CorrectOption(optAutoCentering);
		}


		//*************************************************************************************************
		//機　　能： 画像方向オプションボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   0:上から見た画像, 1:下から見た画像
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void optDirection_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
			int Index = Array.IndexOf(optDirection, sender);
			if (Index < 0) return;

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == optDirection[Index]) cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： フィルタ関数オプションボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   1:256, 2:512, 3:1024, 4:2048
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void optFilter_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
			int Index = Array.IndexOf(optFilter, sender);
			if (Index < 0) return;

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == optFilter[Index]) cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： フィルタ処理オプションボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   1:256, 2:512, 3:1024, 4:2048
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void optFilterProcess_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
			int Index = Array.IndexOf(optFilterProcess, sender);
			if (Index < 0) return;

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == optFilterProcess[Index]) cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： 画質オプションボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   0:標準, 1:精細, 2:高速
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void optImageMode_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
			int Index = Array.IndexOf(optImageMode, sender);
			if (Index < 0) return;

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == optImageMode[Index]) cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： マトリクスオプションボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   1:256, 2:512, 3:1024, 4:2048
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void optMatrix_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
			int Index = Array.IndexOf(optMatrix, sender);
			if (Index < 0) return;

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == optMatrix[Index]) cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： マルチスキャンモードオプションボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   1:シングル, 3:マルチ, 5:スライスプラン
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void optMultiScanMode_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
			int Index = Array.IndexOf(optMultiScanMode, sender);
			if (Index < 0) return;

			//マルチスキャン
			fraMulti.Visible = (Index == (int)ScanSel.MultiScanModeConstants.MultiScanModeMulti);

            //追加2014/10/07hata_v19.51反映
            //v19.51 常にオートセンタリングの機能有、かつ、マルチコーンの場合は強制的にオートセンタリング有りする by長野 2014/02/26
            if ((IsCone == true & Index == (int)ScanSel.MultiScanModeConstants.MultiScanModeMulti & CTSettings.scaninh.Data.alw_autocentering == 0))
            {
                optAutoCentering[0].Checked =false; 
                optAutoCentering[1].Checked = true;
                optAutoCentering[0].Enabled = false;
            }
            else
            {
                optAutoCentering[0].Enabled = true;
            }

			//スライスプランテーブル
			fraSlicePlanName.Visible = (Index == (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan);

			//ビュー数：スライスプラン時は非表示
			fraViewNum.Visible = (Index != (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan);
            
            //Rev20.00 追加 by長野 2015/01/29
            //ビュー数：スライスプラン時は非表示
            fraTableRotation.Visible = (Index != (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan);

			//画像積算枚数：スライスプラン時は非表示
			fraInteg.Visible = (Index != (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan);

			//マトリクスサイズ：スライスプラン時は非表示
			fraMatrix.Visible = (Index != (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan);

			//スキャンエリア：スライスプラン時は非表示
			fraArea.Visible = (Index != (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan);

			//スライス厚：スライスプラン時は非表示
			fraSlice.Visible = (Index != (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan);

			//オートズーミング：コーンビーム時やスライスプラン時は非表示
			fraZoomingPlan.Visible = (CTSettings.scaninh.Data.auto_zoom == 0) && (!IsCone) && (Index != (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan);

			//コーンビームかつマルチスキャン時のみ表示する項目   'v11.4追加 by 間々田 2006/03/13
			fraConeBeamMulti.Visible = IsCone && (Index == (int)ScanSel.MultiScanModeConstants.MultiScanModeMulti);

			//画質フレーム
			//fraImageMode.Visible = IsCone And (Index <> MultiScanModeSlicePlan)
			//'v16.00 再構成高速化機能有の場合，コーンビームの再構成画質選択のフレームを必ず表示させないように変更 by 長野　10/01/21
			//If (scaninh.gpgpu = 0) Then
			//    fraImageMode.Visible = False
			//Else
			//    fraImageMode.Visible = IsCone And (Index <> MultiScanModeSlicePlan)
			//End If

			//画質フレームは表示しない（VC側で常に精細。GPGPUオプションにより、高速と標準は無用と判断。）
			fraImageMode.Visible = false;				//v16.30変更 byやまおか 2010/05/25

			//スライスピッチ(スライス枚数1枚のときは非表示)
			//fraSlicePitch.Visible = IsCone And (Index <> MultiScanModeSlicePlan)
			fraSlicePitch.Visible = IsCone && (Index != (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan) && (cwneK.Value != 1);		//v17.43変更 byやまおか 2011/01/27

			//スライス枚数
			fraSliceNumber.Visible = IsCone && (Index != (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan);

			//コーンビームのときはスライス枚数をチェックする     'v17.43追加 byやまおか 2011/02/01
			//昇降ピッチが最小0.002mmのため、コーンビームのマルチでスライス枚数が偶数枚になるようにする。
			//(例)スライスピッチ0.001mm＆スライス枚数201だと、スキャンピッチが0.201mmなのに対し0.202mmになって
			//　　コーンの継ぎ目で不規則ピッチのデータとなってしまう対策。
			if (IsCone)
			{
				UpdateKMax();				//KMaxの更新
				UpdateKSW();				//KSWの更新
			}

			//再構成範囲
			fraReconRange.Visible = IsCone && (Index != (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan);

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == optMultiScanMode[Index]) cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： Ｘ線管変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		//v29.99 複数X線管は今のところ不要だが、UpdateFcdFidだけ必要なのでForm_Loadに移動 by長野 2013/04/08'''''ここから'''''
		//Private Sub optMultiTube_Click(Index As Integer)
		//
		//    'ＦＣＤ・ＦＩＤ値変更時処理
		//    UpdateFcdFid
		//
		//    '変更した場合，ＯＫボタンを使用可にする
		//    If Me.ActiveControl Is optMultiTube(Index) Then cmdOK.Enabled = True
		//
		//End Sub
		//v29.99 複数X線管は今のところ不要だが、UpdateFcdFidだけ必要なのでForm_Loadに移動  by長野 2013/04/08'''''ここまで'''''


		//*************************************************************************************************
		//機　　能： オーバースキャン有無変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //private void optOverScan_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
        //    int Index = Array.IndexOf(optOverScan, sender);
        //    if (Index < 0) return;

        //    //コーン時：データ収集ビュー数の更新
        //    if (IsCone) UpdateAcqView();

        //    //変更した場合，ＯＫボタンを使用可にする
        //    //If Me.ActiveControl Is optOverScan(Index) Then cmdOK.Enabled = True
        //    if (this.ActiveControl == optOverScan[Index]) cmdOK.Enabled = true;
		
        //}
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

		//*************************************************************************************************
		//機　　能： 再構成形状オプションボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   0:正方形 1:円
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void optReconMask_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
			int Index = Array.IndexOf(optReconMask, sender);
			if (Index < 0) return;

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == optReconMask[Index]) cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： ＲＦＣオプションボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   0:正方形 1:円
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void optRFC_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
			int Index = Array.IndexOf(optRFC, sender);
			if (Index < 0) return;

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == optRFC[Index]) cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： 回転選択オプションボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   0: テーブル 1:Ｘ線管
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void optRotateSelect_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
			int Index = Array.IndexOf(optRotateSelect, sender);
			if (Index < 0) return;

			//FID/FCDオフセットを表示するための処理
			modLibrary.SetOption(optMultiTube, Index);

			switch (Index)
			{
				//テーブルを選択
				case 0:

					//往復スキャンで「する」を選択できなくする
					if (optRoundTrip1.Checked)
					{
						//メッセージ表示：回転選択でテーブルを選択した場合、往復スキャンで「する」を選択できなくなります。
						MessageBox.Show(CTResources.LoadResString(12932), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						optRoundTrip0.Checked = true;
					}
					optRoundTrip1.Enabled = false;

					//テーブル回転でステップ回転は選択できるようにする
					optTableRotation0.Enabled = true;
					break;

				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
				//Ｘ線管を選択
				//        Case 1
				//
				//            'テーブル回転でステップ回転は選択できなくする
				//            If optTableRotation(0).Value Then
				//                'メッセージ表示：回転選択でＸ線管を選択した場合、テーブル回転で「ステップ回転」を選択できなくなります。
				//                MsgBox LoadResString(12933), vbExclamation
				//                optTableRotation(1).Value = True
				//            End If
				//            optTableRotation(0).Enabled = False
				//
				//            '往復スキャンで「する」を選択できるようにする
				//            optRoundTrip(1).Enabled = True
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			}

			//最大最小ビュー数の更新
			UpdateViewMinMax();

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == optRotateSelect[Index]) cmdOK.Enabled = true;
		}


		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//*******************************************************************************
		//機　　能： 往復スキャンオプションボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   0:しない, 1:する
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		//Private Sub optRoundTrip_Click(Index As Integer)
		//
		//    '変更した場合，ＯＫボタンを使用可にする
		//    If Me.ActiveControl Is optRoundTrip(Index) Then cmdOK.Enabled = True
		//
		//End Sub
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


		//*******************************************************************************
		//機　　能： スキャンモードオプションボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   1:ﾊｰﾌｽｷｬﾝ,2:ﾌﾙｽｷｬﾝ,3:ｵﾌｾｯﾄｽｷｬﾝ
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void optScanMode_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
			int Index = Array.IndexOf(optScanMode, sender);
			if (Index < 0) return;

            //追加2014/10/07hata_v19.51反映
            //シフトスキャン⇔非シフトスキャン間の変更をしたとき
			if (((optScanMode_oldIndex == (int)ScanSel.ScanModeConstants.ScanModeShift) & (Index != (int)ScanSel.ScanModeConstants.ScanModeShift)) | 
                ((optScanMode_oldIndex != (int)ScanSel.ScanModeConstants.ScanModeShift) & (Index == (int)ScanSel.ScanModeConstants.ScanModeShift))) {
				
                //オートセンタリングをありにする
				optAutoCentering[1].Checked = true;
			}

			//前のスキャンモードを記憶
			optScanMode_oldIndex = Index;

			//v18.00追加(ここまで) byやまおか 2011/07/09

			//シフトスキャンはステップ回転のみ   'v18.00追加 byやまおか 2011/03/26
			if ((Index == (int)ScanSel.ScanModeConstants.ScanModeShift) | ((IsCone) & (CTSettings.scaninh.Data.smooth_rot_cone != 0))) {

				//連続回転にチェックがあるときは
				if ((optTableRotation[1].Checked == true)) {
					//メッセージ表示：シフトスキャンの場合、強制的にステップ回転とします。
					//Interaction.MsgBox(CT30K.My.Resources.str9522, MsgBoxStyle.Exclamation);
                    MessageBox.Show(CTResources.LoadResString(9522), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					optTableRotation[0].Checked = true;
				}
				//連続回転を隠す
				optTableRotation[1].Visible = false;

				//シフトスキャンじゃないとき
			} else {
				optTableRotation[1].Visible = true;

			}

            //Wスキャンはステップ回転のみ v25.01 追加 by長野 2016/12/16 --->
            if (chkW_Scan.Checked == true | ((IsCone) & (CTSettings.scaninh.Data.smooth_rot_cone != 0)))
            {

                //連続回転にチェックがあるときは
                if ((optTableRotation[1].Checked == true))
                {
                    optTableRotation[0].Checked = true;
                }
                //連続回転を隠す
                optTableRotation[1].Visible = false;

                //Wスキャンじゃないとき
            }
            else
            {
                optTableRotation[1].Visible = true;
            }
            //<---

            //変更2014/10/07hata_v19.51反映
			////スキャンエリア最大値の更新
			//UpdateScanAreaMax();
            //
			////スキャンモードを変えたら必ず最大スキャンエリアとする by 山本　2002-12-14
            ////AreaEvntLock = true;    //代入時のイベントロック 追加2014/06/23(検S1)hata
            //cwneArea.Value = cwneArea.Maximum;
            ////AreaEvntLock = false;    //代入時のイベントロック 追加2014/06/23(検S1)hata
            //
			////ハーフのオートセンタリングができないバージョンの場合
            //#if NoHalfAutoCentering
            //
			////オートセンタリングを選択可・不可を設定する
			//AutoCenteringEnabled();
            //
            //#endif
            //
			////コーン時：データ収集ビュー数の更新
			//if (IsCone) UpdateAcqView();
            //
			////KSWの更新
			//if (IsCone) UpdateKSW();

            //上記を関数化   'v18.00変更 byやまおか 2011/02/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            UpdateCondition();


			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == optScanMode[Index]) cmdOK.Enabled = true;
		}

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

            //通常スキャン⇔Wスキャン間の変更をしたときは、オートセンタリングON
            if (((chkW_Scan_oldCheckState == (CTSettings.scansel.Data.w_scan == 1? CheckState.Checked:CheckState.Unchecked)) && (chkW_Scan.CheckState != (CTSettings.scansel.Data.w_scan == 1? CheckState.Checked:CheckState.Unchecked))) ||
                ((chkW_Scan_oldCheckState !=  (CTSettings.scansel.Data.w_scan == 1? CheckState.Checked:CheckState.Unchecked)) && (chkW_Scan.CheckState == (CTSettings.scansel.Data.w_scan == 1? CheckState.Checked:CheckState.Unchecked))))

            {
                //オートセンタリングをありにする
                optAutoCentering[1].Checked = true;

                //Rev26.00 [ガイド]タブのエリア指定完了フラグを落とす add by chouno 2017/01/16 
                frmScanControl.Instance.scanAreaSetCmpFlg = false;
            }

            //前のWスキャンモードを記憶
            chkW_Scan_oldCheckState = chkW_Scan.CheckState;

            //v18.00追加(ここまで) byやまおか 2011/07/09

            //Wスキャンはステップ回転のみ   
            if (chkW_Scan.CheckState == CheckState.Checked)
            {
                //Rev25.01 by長野 2016/08/22
                //連続回転にチェックがあるときは
                if ((optTableRotation[1].Checked == true))
                {
                    //メッセージ表示：Wスキャンの場合、強制的にステップ回転とします。
                    MessageBox.Show(CTResources.LoadResString(25008), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    optTableRotation[0].Checked = true;
                }
                //連続回転を隠す
                optTableRotation[1].Visible = false;

                //シフトスキャンじゃないとき
            }
            else
            {
                optTableRotation[1].Visible = true;

            }

            //コントロールの値をコモンにセーブする。
            CTSettings.scansel.Data.w_scan = (chkW_Scan.CheckState == CheckState.Checked) ? 1 : 0;

            UpdateCondition();

            //変更した場合，ＯＫボタンを使用可にする
            if (this.ActiveControl == chkW_Scan) cmdOK.Enabled = true;
        }

        //追加2014/10/07hata_v19.51反映
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        public void UpdateCondition()
		{

			//スキャンエリア最大値の更新
			UpdateScanAreaMax();

			//スキャンモードを変えたら必ず最大スキャンエリアとする by 山本　2002-12-14
			cwneArea.Value = cwneArea.Maximum;

			//ハーフのオートセンタリングができないバージョンの場合
#if NoHalfAutoCentering

			//オートセンタリングを選択可・不可を設定する
			AutoCenteringEnabled();

#endif

			//コーン時：データ収集ビュー数の更新
			if (IsCone)
				UpdateAcqView();

			//KSWの更新
			if (IsCone)
				UpdateKSW();

			//ゲイン校正ステータス（簡易表示）の更新     'v18.00追加 byやまおか 2011/02/11
			var _with17 = frmCorrectionStatus.Instance;
			//if ((CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift))
            //Rev25.00 Wスキャン追加 by長野 2016/07/07
            if ((CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift) || (CTSettings.scansel.Data.w_scan == 1)) 
            {
				//シフトスキャンならゲイン校正ステータス(シフト用)を考慮して更新
				_with17.UpdateStatus(_with17.lblItemGain,ref frmScanControl.Instance.lblStatus[0],"" , _with17.lblItemGainShift);
			} 
            else 
            {
				//シフトスキャン以外は通常のゲイン校正ステータスだけで更新
				_with17.UpdateStatus(_with17.lblItemGain,ref frmScanControl.Instance.lblStatus[0]);
			}

		}

		//*******************************************************************************
		//機　　能： スキャンエリア最大値を更新する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   0:1ｽﾗｲｽ,1:3ｽﾗｲｽ,2:5ｽﾗｲｽ
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		//private void UpdateScanAreaMax()
		public void UpdateScanAreaMax()
		{
			//スキャンモード取得
			int Index = 0;
			Index = modLibrary.GetOption(optScanMode);

			//スキャンモード不定ならば何もしない
			if (Index == -1) return;

            //変更2014/10/07hata_v19.51反映
            ////スキャンエリア最大値の更新
            //if (IsCone)
            //{
            //    float Mode = 0;
            //    Mode = (Index == (int)ScanSel.ScanModeConstants.ScanModeOffset ? 1 : 0);
            //    cwneArea.Maximum = (decimal)(2 * FCDWithOffset * Math.Sin(CTSettings.scancondpar.Data.theta0[(int)Mode] / 2) / Math.Cos(CTSettings.scancondpar.Data.thetaoff));
            //}
            //else
            //{
            //    cwneArea.Maximum = (decimal)CTSettings.scansel.Data.max_scan_area[Index - 1];
            //}
            //スキャンエリア最大値の更新(シフトスキャンに対応)   'v18.00変更 byやまおか 2011/07/16 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            int Mode = 0;
            var _with18 = CTSettings.scancondpar.Data;
            if (IsCone)
            {
                decimal tmpVal = 0;

                switch (Index)
                {
                    //rev20.00 追加 四捨五入 by長野 2015/01/26
                    case (int)ScanSel.ScanModeConstants.ScanModeShift:
                        Mode = 2;
                        tmpVal = (decimal)(2 * FCDWithOffset * System.Math.Sin(_with18.theta0[Mode] / 2) / System.Math.Cos(_with18.thetaoff));
                        //cwneArea.Maximum = (decimal)(2 * FCDWithOffset * System.Math.Sin(_with18.theta0[Mode] / 2) / System.Math.Cos(_with18.thetaoff));
                        cwneArea.Maximum = Math.Round(tmpVal / cwneArea.Increment,MidpointRounding.AwayFromZero) * cwneArea.Increment;
                        break;
                    case (int)ScanSel.ScanModeConstants.ScanModeOffset:
                        Mode = 1;
                        tmpVal = (decimal)(2 * FCDWithOffset * System.Math.Sin(_with18.theta0[Mode] / 2) / System.Math.Cos(_with18.thetaoff));
                        cwneArea.Maximum = Math.Round(tmpVal / cwneArea.Increment, MidpointRounding.AwayFromZero) * cwneArea.Increment;                       
                        //cwneArea.Maximum = (decimal)(2 * FCDWithOffset * System.Math.Sin(_with18.theta0[Mode] / 2) / System.Math.Cos(_with18.thetaoff));
                        break;
                    default:
                        Mode = 0;
                        tmpVal = (decimal)(2 * FCDWithOffset * System.Math.Sin(_with18.theta0[Mode] / 2) / System.Math.Cos(_with18.thetaoff));
                        cwneArea.Maximum = Math.Round(tmpVal / cwneArea.Increment, MidpointRounding.AwayFromZero) * cwneArea.Increment;                        
                        //cwneArea.Maximum = (decimal)(2 * FCDWithOffset * System.Math.Sin(_with18.theta0[Mode] / 2) / System.Math.Cos(_with18.thetaoff));
                        break;
                }
            }
            else
            {
                //変更2015/01/29hata
                //cwneArea.Maximum = (decimal)CTSettings.scansel.Data.max_scan_area[Index - 1];
                cwneArea.Maximum = Math.Round((decimal)CTSettings.scansel.Data.max_scan_area[Index - 1] / cwneArea.Increment, MidpointRounding.AwayFromZero) * cwneArea.Increment; 
            }


            //最大値の表示
            lblAreaMinMax.Text = StringTable.GetResString(StringTable.IDS_MaxMM, cwneArea.Maximum.ToString(string.Format("F{0}", cwneArea.DecimalPlaces)));

        
        }


		//*******************************************************************************
		//機　　能： マルチスライスモードオプションボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   0:1ｽﾗｲｽ,1:3ｽﾗｲｽ,2:5ｽﾗｲｽ
		//戻 り 値： なし
		//
		//補　　足： マルチスライスでも連続回転できるようにした
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void optMultislice_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
			int Index = Array.IndexOf(optMultislice, sender);
			if (Index < 0) return;
            
            //複数スライスが1枚の時はピッチを表示しない
			ntbMultislicePitch.Visible = (Index > 0);			//V3.0 append by 鈴山
            lblPitchMinMax.Visible = (Index > 0);

			//最大最小スライス厚の再計算
			UpdateSliceMinMax();

			UpdateLine();

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == optMultislice[Index]) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： スキャン中再構成オプションボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   1:行う,0:行わない
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void optScanAndView_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
			int Index = Array.IndexOf(optScanAndView, sender);
			if (Index < 0) return;

			//スキャン中再構成を行わない場合
			if (Index == 0)
			{
				//「生データ保存なし」の場合 'v15.0追加 by 間々田 2009/03/06
				if (!IsRawDataSave)
				{
					//
					//MsgBox "生データ保存なしの場合は、スキャン中再構成を「行わない」に設定することはできません。", vbExclamation
					MessageBox.Show(CTResources.LoadResString(20070), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);		//ストリングテーブル化 'v17.60 by長野 2011/05/22
					optScanAndView1.Checked = true;

					//「オートズームあり」もしくは「生データ保存なし」の場合
					//If (chkZooming.Value = vbChecked) Or (chkSave.Value = vbUnchecked) Then
					//If (chkZooming.value = vbChecked) Or (chkSave.value = vbUnchecked) Or _
					//'   (fraContrastFitting.Visible And optContrastFitting(1).value) Or _
					//'   (fraAutoPrint.Visible And (chkPrint.value = vbChecked)) Then                                 'v11.5追加 by 間々田 2006/09/01 条件項目追加
				}
				//v15.0変更 by 間々田 2009/03/06
				else if ((chkZooming.CheckState == CheckState.Checked) || 
						(fraContrastFitting.Visible && optContrastFitting1.Checked) || 
						(fraAutoPrint.Visible && (chkPrint.CheckState == CheckState.Checked)))					//v11.5追加 by 間々田 2006/09/01 条件項目追加
				{
					//メッセージ表示：
					//'   スキャン中再構成を行わない場合は自動的に生データ保存モードになります。
					//'   また、オートズームは行わないモードになります。
					//MsgBox LoadResString(9412), vbExclamation

					//v11.5メッセージの変更ここから　 by 間々田 2006/09/01
					//   スキャン中再構成を行わない場合は、自動的に以下の条件が設定されます。
					//       生データ保存　：あり
					//       オートズーム　：なし
					//       オートプリント：なし
					//       画像階調最適化：なし
					string Msg = null;
					Msg = CTResources.LoadResString(9412) + "\r";

					//'生データ保存無しの場合は強制的に生データ保存ありモードにする  'v15.0削除 by 間々田 2009/03/06
					//If chkSave.value = vbUnchecked Then
					//    Msg = Msg & vbCr & vbTab & fraOrgSave.Caption & vbTab & LoadResString(IDS_Colon) & LoadResString(IDS_ON)
					//End If

					//オートズームありの場合は強制的にオートズーム無しにする
					if (chkZooming.CheckState == CheckState.Checked)
					{
						Msg = Msg + "\r" + "\t" + fraZoomingPlan.Text + "\t" + "\t" + CTResources.LoadResString(StringTable.IDS_Colon) + CTResources.LoadResString(StringTable.IDS_OFF);
					}

					//画像階調最適化ありの場合は強制的に画像階調最適化無しにする
					if (fraContrastFitting.Visible & optContrastFitting[1].Checked)
					{
						Msg = Msg + "\r" + "\t" + fraContrastFitting.Text + "\t" + CTResources.LoadResString(StringTable.IDS_Colon) + CTResources.LoadResString(StringTable.IDS_OFF);
					}

					//オートプリントありの場合は強制的にオートプリント無しにする
					if (fraAutoPrint.Visible & (chkPrint.CheckState == System.Windows.Forms.CheckState.Checked))
					{
						Msg = Msg + "\r" + "\t" + fraAutoPrint.Text + "\t" + "\t" + CTResources.LoadResString(StringTable.IDS_Colon) + CTResources.LoadResString(StringTable.IDS_OFF);
					}

					MessageBox.Show(Msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					//v11.5メッセージの変更ここまで　 by 間々田 2006/09/01

					//'生データ保存無しの場合は強制的に生データ保存ありモードにする  'v15.0削除 by 間々田 2009/03/06
					//If chkSave.value = vbUnchecked Then chkSave.value = vbChecked

					//オートズームありの場合は強制的にオートズーム無しにする
					if (chkZooming.CheckState == CheckState.Checked) chkZooming.CheckState = CheckState.Unchecked;

					//画像階調最適化ありの場合は強制的に画像階調最適化無しにする                                 'v11.5追加 by 間々田 2006/09/01
					if (fraContrastFitting.Visible && optContrastFitting1.Checked) modLibrary.SetOption(optContrastFitting, 0);

					//オートプリントありの場合は強制的にオートプリント無しにする                                 'v11.5追加 by 間々田 2006/09/01
					if (fraAutoPrint.Visible && (chkPrint.CheckState == CheckState.Checked)) chkPrint.CheckState = CheckState.Unchecked;
				}
			}

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == optScanAndView[Index]) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： テーブル回転オプションボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Index           [I/ ] Integer   0:ｽﾃｯﾌﾟ,1:連続
		//戻 り 値： なし
		//
		//補　　足： マルチスライスでの連続回転不可の制限を廃止
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void optTableRotation_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
			int Index = Array.IndexOf(optTableRotation, sender);
			if (Index < 0) return;

            //Rev20.00 UpdateViewMinMaxの下に移動 by長野 2015/02/20
            ////変更2014/10/07hata_v19.51反映
            ////v19.01 追加 by長野 2012/05/21
            ////if (CTSettings.scaninh.Data.smooth_rot_cone == 0)
            ////v19.15 シングル連続時にも通過するように修正 by長野 2013/06/15
            //if (((!IsCone) | CTSettings.scaninh.Data.smooth_rot_cone == 0))

            //{
            //    ChangeViewList();
            //}

            ////変更2014/10/07hata_v19.51反映
            ////if (optTableRotation0.Checked == true)
            ////v19.19 修正
            //if (optTableRotation[0].Checked == true & IsCone)
            //{
            //    lblViewMinMax.Visible = true;
            //    //v19.17 追加 by長野 2013/09/17     //追加2014/10/07hata_v19.51反映
            //    fraAcqView.Visible = true;
            //}
            //else
            //{
            //    lblViewMinMax.Visible = false;
            //    //v19.17 追加 by長野 2013/09/17     //追加2014/10/07hata_v19.51反映
            //    fraAcqView.Visible = false;
            //}

            ////Rev20.00 テーブルステップ・連続切替時にもビュー数の整数化処理は実行する by長野 2014/09/18
            //decimal val1 = 0;
            //if (modLibrary.GetOption(optTableRotation) == 0) //Rev20.00 テーブルステップ回転時のみ実行する by長野 2014/09/11
            //{
            //    //数字のインクリメントを合わせる10,100,200･･･
            //    val1 = cwneViewNum.Value / (decimal)100.0;
            //    val1 = Math.Round(val1, 0, MidpointRounding.AwayFromZero) * 100;
            //    if (val1 < cwneViewNum.Minimum) val1 = cwneViewNum.Minimum;
            //    if (val1 > cwneViewNum.Maximum) val1 = cwneViewNum.Maximum;
            //    if (cwneViewNum.Value != val1)
            //    {
            //        cwneViewNum.Value = val1;
            //        //Rev20.00 returnで終わらせず、後の処理も実行する by長野 2015/01/24
            //        //return;
            //    }
            //}

			//最大最小ビュー数の更新
			UpdateViewMinMax();

            //変更2014/10/07hata_v19.51反映
            //v19.01 追加 by長野 2012/05/21
            //if (CTSettings.scaninh.Data.smooth_rot_cone == 0)
            //v19.15 シングル連続時にも通過するように修正 by長野 2013/06/15
            if (((!IsCone) | CTSettings.scaninh.Data.smooth_rot_cone == 0))
            {
                ChangeViewList();
            }

            //変更2014/10/07hata_v19.51反映
            //if (optTableRotation0.Checked == true)
            //v19.19 修正
            if (optTableRotation[0].Checked == true & IsCone)
            {
                lblViewMinMax.Visible = true;
                //v19.17 追加 by長野 2013/09/17     //追加2014/10/07hata_v19.51反映
                fraAcqView.Visible = true;
            }
            else
            {
                lblViewMinMax.Visible = false;
                //v19.17 追加 by長野 2013/09/17     //追加2014/10/07hata_v19.51反映
                fraAcqView.Visible = false;
            }

            //Rev20.00 テーブルステップ・連続切替時にもビュー数の整数化処理は実行する by長野 2014/09/18
            decimal val1 = 0;
            if (modLibrary.GetOption(optTableRotation) == 0) //Rev20.00 テーブルステップ回転時のみ実行する by長野 2014/09/11
            {
                //数字のインクリメントを合わせる10,100,200･･･
                val1 = cwneViewNum.Value / (decimal)100.0;
                val1 = Math.Round(val1, 0, MidpointRounding.AwayFromZero) * 100;
                if (val1 < cwneViewNum.Minimum) val1 = cwneViewNum.Minimum;
                if (val1 > cwneViewNum.Maximum) val1 = cwneViewNum.Maximum;
                if (cwneViewNum.Value != val1)
                {
                    cwneViewNum.Value = val1;
                    //Rev20.00 returnで終わらせず、後の処理も実行する by長野 2015/01/24
                    //return;
                }
            }

			//最大積算枚数の更新
			UpdateIntegMax();

			//最小積算枚数の更新 'v16.2 追加 by 山影 2010/01/19
			UpdateIntegMin();

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == optTableRotation[Index]) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： メール送信フレーム内・設定ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v9.1 2004/05/13 (SI4)間々田      新規作成
		//*******************************************************************************
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//Private Sub cmdSettingMail_Click()
		//
		//    'メール送信設定フォーム表示
		//    frmMailCondition.Show vbModal
		//
		//    '変更した場合，ＯＫボタンを使用可にする
		//    cmdOK.Enabled = True
		//
		//End Sub
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

		//*******************************************************************************
		//機　　能： DICOM関連の更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v6.0 2002/08/08 (SI4)間々田      新規作成
		//           v11.2 2006/01/19 (SI3)間々田    scancondparへのアクセス方法を変更
		//*******************************************************************************
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//Private Sub UpdateForDICOM()
		//
		//    With scancondpar
		//
		//        'スキャン済みの場合のみ実行
		//        If .scan_comp = 0 Then Exit Sub
		//
		//        'スキャン条件が変更されている場合
		//        If ChangeInitCondition() Then
		//
		//            'スキャン済みフラッグを未スキャンとする
		//            .scan_comp = 0
		//
		//            'シリーズ番号をカウントアップ
		//            .series_num = .series_num + 1
		//
		//            '収集番号を１にリセット
		//            .acq_num = 1
		//
		//        End If
		//
		//        'v15.0削除ここから by 間々田 2009/03/16
		//        '' ディレクトリ名・スライス名のいずれかが変更されている場合
		//        'If (UCase$(scansel.pro_code) <> UCase$(ScanselOrg.pro_code)) Or _
		//'        '   (UCase$(scansel.pro_name) <> UCase$(ScanselOrg.pro_name)) Then
		//        '
		//        '    '検査ＩＤをカウントアップ
		//        '    .study_id = .study_id + 1
		//        '
		//        '    'setfile 側もカウントアップ
		//        '    UpdateCsv DICOM_CSV, "study_id", CStr(.study_id)
		//        '
		//        '    'スキャン済みフラッグを未スキャンとする
		//        '    .scan_comp = 0
		//        '
		//        '    'シリーズ番号を１にリセット
		//        '    .series_num = 1
		//        '
		//        '    '収集番号を１にリセット
		//        '    .acq_num = 1
		//        '
		//        'End If
		//        'v15.0削除ここまで by 間々田 2009/03/16
		//
		//    End With
		//
		//    'scancondparの書き込み
		//    CallPutScancondpar
		//
		//End Sub
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


		// added V6.0 by 間々田 2002-08-08 START
		private bool ChangeInitCondition()
		{
			bool functionReturnValue = true;

			//        If scansel.scan_kv <> .scan_kv Then Exit Function                                   '1 管電圧
			//        If scansel.scan_ma <> .scan_ma Then Exit Function                                   '2 管電流
            if (CTSettings.scansel.Data.scan_view != ScanselOrg.scan_view) return functionReturnValue;								//3 ビュー数
            if (CTSettings.scansel.Data.scan_integ_number != ScanselOrg.scan_integ_number) return functionReturnValue;				//4 画像積算枚数
            if (CTSettings.scansel.Data.mscan_width != ScanselOrg.mscan_width) return functionReturnValue;							//5 スライス厚
            if (CTSettings.scansel.Data.matrix_size != ScanselOrg.matrix_size) return functionReturnValue;							//6 マトリクスサイズ
            if (CTSettings.scansel.Data.scan_mode != ScanselOrg.scan_mode) return functionReturnValue;								//7 スキャンモード
            if (CTSettings.scansel.Data.filter != ScanselOrg.filter) return functionReturnValue;										//8 フィルタ関数
            if (CTSettings.scansel.Data.mscan_bias != ScanselOrg.mscan_bias) return functionReturnValue;								//9 バイアス
            if (CTSettings.scansel.Data.mscan_slope != ScanselOrg.mscan_slope) return functionReturnValue;							//10 スロープ
            if (CTSettings.scansel.Data.data_mode != ScanselOrg.data_mode) return functionReturnValue;								//11 データモード
            if (CTSettings.scansel.Data.image_direction != ScanselOrg.image_direction) return functionReturnValue;					//12 画像方向
			//        If cwneFid.Value <> InitFid Then Exit Function                                      '13 FID(オフセットを含まない)
			//        If cwneFcd.Value <> InitFcd Then Exit Function                                      '14 FCD(オフセットを含まない)
            if (CTSettings.scansel.Data.mscan_area != ScanselOrg.mscan_area) return functionReturnValue;								//15 スキャンエリア
            if (CTSettings.scansel.Data.auto_zoomflag != ScanselOrg.auto_zoomflag) return functionReturnValue;						//16 オートズーミング：処理
            if (CTSettings.scansel.Data.autozoom_dir.GetString().ToUpper() != ScanselOrg.autozoom_dir.GetString().ToUpper()) return functionReturnValue;		//17 オートズーミング：ディレクトリ名    'v9.7変更 by 間々田 2004/11/26
            if (CTSettings.scansel.Data.auto_zoom.GetString().ToUpper() != ScanselOrg.auto_zoom.GetString().ToUpper()) return functionReturnValue;			//18 オートズーミング：テーブル名        'v9.7変更 by 間々田 2004/11/26
            if (CTSettings.scansel.Data.recon_mask != ScanselOrg.recon_mask) return functionReturnValue;								//19 再構成形状(0:正方形,1:円)
            if (CTSettings.scansel.Data.image_rotate_angle != ScanselOrg.image_rotate_angle) return functionReturnValue;				//20 画像回転角度
            if (CTSettings.scansel.Data.table_rotation != ScanselOrg.table_rotation) return functionReturnValue;						//21 テーブル回転
            if (CTSettings.scansel.Data.binning != ScanselOrg.binning) return functionReturnValue;									//22 透視画像サイズ(横)
            if (fraMultiTube.Visible && (CTSettings.scansel.Data.multi_tube != ScanselOrg.multi_tube)) return functionReturnValue;	//23 X線管

			//v19.00 ->(電S2)永井
            if (Convert.ToInt32(chkBHC.CheckState) != ScanselOrg.mbhc_flag) return functionReturnValue;																		//BHCテーブル→処理         v8.0追加 Ohkado 2007/01/24
            if (txtBhcDirName.Text.ToUpper() != modLibrary.RemoveNull(ScanselOrg.mbhc_dir.GetString()).ToUpper()) return functionReturnValue;											//BHCテーブル→ﾃﾞｨﾚｸﾄﾘ名    v8.0追加 Ohkado 2007/01/24
            if (txtBhcFileName.Text.ToUpper() != modLibrary.AddExtension(modLibrary.RemoveNull(ScanselOrg.mbhc_name.GetString()), ".csv").ToUpper()) return functionReturnValue;		//BHCテーブル→ﾃｰﾌﾞﾙ名      v8.0追加 Ohkado 2007/01/24
			//<- v19.00
			
			functionReturnValue = false;

			return functionReturnValue;
		}
		// added V6.0 by 間々田 2002-08-08 END


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
		//           v9.7   04/12/08  (SI4)間々田    アーティファクト低減対応
		//*******************************************************************************
		private void SetCaption()
		{
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

			this.Text = StringTable.BuildResStr(StringTable.IDS_Details, StringTable.IDS_ScanCondition);		//スキャン条件－詳細

			//テーブル回転フレーム
            // Mod Start 2018/08/24 M.Oyama 中国語対応
            //fraTableRotation.Text = StringTable.BuildResStr(StringTable.IDS_Rotate, StringTable.IDS_Table);		//テーブル回転
            fraTableRotation.Text = StringTable.BuildResStr(StringTable.IDS_Rotate, StringTable.IDS_SampleTable);		//テーブル回転
            // Mod End 2018/08/24

			//v17.60 英語版ではpixelが長いのでpix.とする。 by長野 2011/06/25
			//    'スライス厚フレーム
			//    lblPixDsp.Caption = LoadResString(IDS_Pixels) & ")"                  '画素)
			//
			if (modCT30K.IsEnglish)
			{
				lblPixDsp.Text = "pix.)";
			}
			else
			{
				//スライス厚フレーム
				lblPixDsp.Text = CTResources.LoadResString(StringTable.IDS_Pixels) + ")";		//画素)
			}

			//透視画像保存フレーム
			fraFluoroImageSave.Text = StringTable.BuildResStr(StringTable.IDS_Save, StringTable.IDS_TransImage);		//透視画像の保存
			chkFluoroImageSave.Text = StringTable.BuildResStr(StringTable.IDS_DoSave, StringTable.IDS_TransImage);		//透視画像を保存する

			//画像回転角度フレーム
			lblImageRotateAngleMinMax.Text = StringTable.GetResString(StringTable.IDS_Range, "-180", "180");			//(-180～180)

			//以下はコーンビームスキャン条件から移動してきた

			//スキャン開始可能範囲フレーム
			if (modCT30K.IsEnglish) fraScanStartRange.Font = new Font(fraScanStartRange.Font.Name, 9);

			//スライス枚数フレーム
			fraSliceNumber.Text = CTResources.LoadResString(StringTable.IDS_SliceNumber);							//スライス枚数
			lblSliceNumUni.Text = (modCT30K.IsEnglish ? "" : CTResources.LoadResString(StringTable.IDS_Frame));	//枚

			//v17.02削除 frmScanControlへ移動したため byやまおか 2010/07/16
			//'フラットパネル設定フレーム 'v17.00追加 byやまおか 2010/02/17
			//cmbGain.List(0) = GetFpdGainStr(0)
			//cmbGain.List(1) = GetFpdGainStr(1)
			//cmbGain.List(2) = GetFpdGainStr(2)
			//cmbGain.List(3) = GetFpdGainStr(3)
			//cmbGain.List(4) = GetFpdGainStr(4)
			//cmbGain.List(5) = GetFpdGainStr(5)
			//cmbInteg.List(0) = GetFpdIntegStr(0)
			//cmbInteg.List(1) = GetFpdIntegStr(1)
			//cmbInteg.List(2) = GetFpdIntegStr(2)
			//cmbInteg.List(3) = GetFpdIntegStr(3)
			//cmbInteg.List(4) = GetFpdIntegStr(4)
			//cmbInteg.List(5) = GetFpdIntegStr(5)
			//cmbInteg.List(6) = GetFpdIntegStr(6)
			//cmbInteg.List(7) = GetFpdIntegStr(7)

			//以下はコモンから取得

			//フレーム
            fraZoomingPlan.Text = CTSettings.infdef.Data.auto_zoom.GetString();					//オートズーミング
            fraAutoPrint.Text = CTSettings.infdef.Data.auto_print.GetString();					//ｵｰﾄﾌﾟﾘﾝﾄ

			//オプションボタン
            optMultiScanMode1.Text = CTSettings.infdef.Data.multiscan_mode[0].GetString();		//ｼﾝｸﾞﾙ
            optMultiScanMode3.Text = CTSettings.infdef.Data.multiscan_mode[1].GetString();		//ﾏﾙﾁ
            optMultiScanMode5.Text = CTSettings.infdef.Data.multiscan_mode[2].GetString();		//スライスプラン

            optScanMode1.Text = CTSettings.infdef.Data.scan_mode[0].GetString();				//ﾊｰﾌｽｷｬﾝ
            optScanMode2.Text = CTSettings.infdef.Data.scan_mode[1].GetString();				//ﾌﾙｽｷｬﾝ
            optScanMode3.Text = CTSettings.infdef.Data.scan_mode[2].GetString();				//ｵﾌｾｯﾄｽｷｬﾝ
            //追加2014/10/07hata_v19.51反映
            optScanMode4.Text = CTSettings.infdef.Data.scan_mode[3].GetString();                //ｼﾌﾄｽｷｬﾝ    'v18.00追加 byやまおか 2011/02/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //Rev25.00 追加 by長野 2016/06/19
            chkW_Scan.Text = CTResources.LoadResString(25001);                                 //Wスキャン

            optFilter1.Text = CTSettings.infdef.Data.fc[0].GetString();							//Laks
            optFilter2.Text = CTSettings.infdef.Data.fc[1].GetString();							//Shepp
            optFilter3.Text = CTSettings.infdef.Data.fc[2].GetString();							//HightReso

            optMatrix1.Text = CTSettings.infdef.Data.matrixsize[0].GetString();					//256×256
            optMatrix2.Text = CTSettings.infdef.Data.matrixsize[1].GetString();					//512×512
            optMatrix3.Text = CTSettings.infdef.Data.matrixsize[2].GetString();					//1024×1024
            optMatrix4.Text = CTSettings.infdef.Data.matrixsize[3].GetString();					//2048×2048
            optMatrix5.Text = CTSettings.infdef.Data.matrixsize[4].GetString();					//4096×4096 '4096対応 v16.10 by 長野 10/01/29

            optMultislice0.Text = CTSettings.infdef.Data.multislice[0].GetString();				//1ｽﾗｲｽ
            optMultislice1.Text = CTSettings.infdef.Data.multislice[1].GetString();				//3ｽﾗｲｽ
            optMultislice2.Text = CTSettings.infdef.Data.multislice[2].GetString();				//5ｽﾗｲｽ

            optMultiTube0.Text = CTSettings.infdef.Data.multi_tube[0].GetString();				//130kV
            optMultiTube1.Text = CTSettings.infdef.Data.multi_tube[1].GetString();				//225kV

            optFilterProcess0.Text = CTSettings.infdef.Data.filter_process[0].GetString();		//FFT            'v13.0追加? byやまおか 2007/??/??
            optFilterProcess1.Text = CTSettings.infdef.Data.filter_process[1].GetString();		//ｺﾝﾎﾞﾘｭｰｼｮﾝ     'v13.0追加? byやまおか 2007/??/??

            optRFC0.Text = CTSettings.infdef.Data.rfc_char[0].GetString();						//無     'v14.0追加 byやまおか 07/07/17
            optRFC1.Text = CTSettings.infdef.Data.rfc_char[1].GetString();						//弱     'v14.0追加 byやまおか 07/07/17
            optRFC2.Text = CTSettings.infdef.Data.rfc_char[2].GetString();						//強→中 'v14.0追加 byやまおか 07/07/17
            optRFC3.Text = CTSettings.infdef.Data.rfc_char[3].GetString();						//強     'v14.2追加 byYAMAKAGE 08/04/08

			//v19.00 ->(電S2)永井
            fraBHC.Text = CTSettings.infdef.Data.bhc.GetString();
			//<- v19.00


			//    lblWidthUni.Caption = LoadResString(IDS_Pixels)         '画素
			//    lblHeightUni.Caption = LoadResString(IDS_Pixels)        '画素

			//v19.00 ->(電S2)永井
			//ﾋﾞｰﾑﾊﾄﾞﾆﾝｸﾞ補正フレーム内
			chkBHC.Text = CTResources.LoadResString(21200);											//ＢＨＣ処理を行う
			lblBhcDirTitle.Text = StringTable.LoadResStringWithColon(StringTable.IDS_DirName);		//ディレクトリ名：
			lblBhcFileTitle.Text = StringTable.LoadResStringWithColon(StringTable.IDS_TableName);
			cmdChangeBHCTable.Text = CTResources.LoadResString(StringTable.IDS_btnChange);			//変更...    'v8.0 追加 by Ohkado 2007/01/24
			//<- v19.00
			//v19.11　リソース化 by長野 2013/02/20
			cmdChangeBHCTableDefault.Text = CTResources.LoadResString(21312);			//デフォルト

            //Rev26.00 プリセット名、コメント by chouno 2017/09/01
            if (modScanCondition.PresetSelectedIndex > -1)
            {
                txtPresetName.Text = modScanCondition.PresetName[modScanCondition.PresetSelectedIndex];
                string tmpText = "";
                modScanCondition.getPresetFileItem(Path.Combine(AppValue.PathScanCondPresetFile, txtPresetName.Text) + "-SC.csv", "comment", ref tmpText);
                txtPresetComment.Text = tmpText;
            }
		}

		//
		//   Fid/Fcd offset配列のIndex値を求める added by 間々田 2004/02/03
		//
		private int GetFcdIndexInScanCondition()
		{
			int functionReturnValue = 0;

            if (CTSettings.scaninh.Data.multi_tube == 0)
			{
				functionReturnValue = modLibrary.MaxVal(modLibrary.GetOption(optMultiTube), 0);
			}
            else if (CTSettings.scaninh.Data.rotate_select == 0)
			{
				functionReturnValue = modLibrary.MaxVal(modLibrary.GetOption(optRotateSelect), 0);
			}
			else
			{
				functionReturnValue = 0;
			}

			return functionReturnValue;
		}



		//以下はfrmConeScanConditionから移動してきた

		//********************************************************************************
		//機    能  ：  スキャン枚数Kを変えた場合
		//              変数名           [I/O] 型        内容
		//引    数  ：  flg_Bar          Boolean
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V3.0   XX/XX/XX  ??????????????  新規作成
		//              V3.1   00/10/11  (CATS)巻淵      変更
		//********************************************************************************
		private void K_Change()
		{
			//スライスピッチのフレーム：K>1の場合可視とし、K=1の場合不可視とする
            //変更2014/10/07hata_v19.51反映
            //fraSlicePitch.Visible = (cwneK.Value > 1);
            fraSlicePitch.Visible = IsCone & !optMultiScanMode[5].Checked & (cwneK.Value != 1);            //V19.20 変更 by Inaba 2012/11/01 'v19.50 v19.41との統合 by長野 2013/11/17

			//最大スライスピッチの設定
			UpdateDeltaZMax();

			//最大スライス厚の設定
			UpdateSWMax();

			//非ヘリカルの場合
			if (optHelical0.Checked)
			{
				//再構成範囲（開始位置）の更新
				UpdateZs();

				//再構成範囲（終了位置）の更新
				UpdateZe();
			}
			//ヘリカルの場合
			else
			{
				//再構成範囲（終了位置）の更新
				UpdateZe();

				//スキャン開始可能範囲（最大値）の更新
				UpdateZdasmax();

				//最大ヘリカルピッチの設定
				UpdateZpMax();
			}
		}


		//********************************************************************************
		//機    能  ：  スライス厚SWを変えた場合
		//              変数名           [I/O] 型        内容
		//引    数  ：  flg_Bar          Boolean
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V3.0   XX/XX/XX  ??????????????  新規作成
		//              V3.1   00/10/11  (CATS)巻淵      変更
		//********************************************************************************
		private void SW_Change()
		{
            //変更2014/06/18(検S1)hata
            //kswが不定の場合は、計算しない  
            ////ZWの計算
            //ZW = (SWMaxPara - (float)cwneSlice.Value) * (float)ksw;
            if (ksw != null)
            {
                //ZWの計算
                ZW = (SWMaxPara - (float)cwneSlice.Value) * (float)ksw;
            }

			//最大スライスピッチの設定
			UpdateDeltaZMax();

			//非ヘリカルの場合
			if (optHelical0.Checked)
			{
				//最大スライス枚数の設定
				UpdateKMax();
			}
			//ヘリカルの場合
			else
			{
				//最大ヘリカルピッチの設定
				UpdateZpMax();
			}
		}


		//********************************************************************************
		//機    能  ：  Zdasを変えた場合
		//              変数名           [I/O] 型        内容
		//引    数  ：  flg_Bar          Boolean
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V3.0   XX/XX/XX  ??????????????  新規作成
		//              V3.1   00/10/11  (CATS)巻淵      変更
		//********************************************************************************
		private void Zdas_Change()
		{
			//再構成範囲（開始位置）の更新
			UpdateZs();

			//最大ヘリカルピッチの設定
			UpdateZpMax();

			//再構成範囲（終了位置）の更新
			UpdateZe();
		}


		//********************************************************************************
		//機    能  ：  Zp（ヘリカルピッチ）を変えた場合
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V3.0   XX/XX/XX  ??????????????  新規作成
		//              V3.1   00/10/11  (CATS)巻淵      変更
		//********************************************************************************
		private void Zp_Change()
		{

            //変更2014/06/18(検S1)hata
            //kswが不定の場合は、計算しない
            ////スライス厚の最大値の設定
            //SWMaxChange(SWMaxPara - (float)cwneZp.Value * kzp / (float)ksw);
            if (ksw != null)
            {
                //スライス厚の最大値の設定
                SWMaxChange(SWMaxPara - (float)cwneZp.Value * kzp / (float)ksw);
            }

			//最大スライス枚数の設定
			UpdateKMax();

			//再構成範囲（開始位置）の更新
			UpdateZs();

			//ヘリカル終了位置の更新
			UpdateZdae();

			//スキャン開始可能範囲（最大値）の更新
			UpdateZdasmax();

			//最大スライスピッチの設定
			UpdateDeltaZMax();
		}


		//********************************************************************************
		//機    能  ：  スライスピッチΔZを変えた場合
		//              変数名           [I/O] 型        内容
		//引    数  ：  flg_Bar          Boolean
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V3.0   XX/XX/XX  ??????????????  新規作成
		//              V3.1   00/10/11  (CATS)巻淵      変更
		//********************************************************************************
		private void delta_z_Change()
		{

			//エラー時の扱い
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			On Error Resume Next                '追加 by 間々田 2009/08/21
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			try
			{
				//画素変換
				myConeSlicePitchPix = (double)cwneDelta_z.Value / SWMinCone;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				If Err.Number <> 0 Then Err.Clear   '追加 by 間々田 2009/08/21 ０割対応
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
			}
			catch
			{ }

			try
			{
				//最大スライス枚数の設定
				UpdateKMax();
			}
			catch
			{ }

			try
			{
				//最大スライス厚の設定
				UpdateSWMax();
			}
			catch
			{ }

			//非ヘリカルの場合
			if (optHelical0.Checked)
			{
				try
				{
					//再構成範囲（開始位置）の更新
					UpdateZs();
				}
				catch
				{ }

				try
				{
					//再構成範囲（終了位置）の更新
					UpdateZe();
				}
				catch
				{ }
			}
			//ヘリカルの場合
			else
			{
				try
				{
					//再構成範囲（終了位置）の更新
					UpdateZe();
				}
				catch
				{ }

				try
				{
					//スキャン開始可能範囲（最大値）の更新
					UpdateZdasmax();
				}
				catch
				{ }

				try
				{
					//最大ヘリカルピッチの設定
					UpdateZpMax();
				}
				catch
				{ }
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
            //if (DeltazEvntLock) return;   //代入時のイベントロック 追加2014/06/23(検S1)hata
            if (EvntLock) return;   //Load時のイベントロック 追加2014/06/23(検S1)hata
            
            //各パラメータの再計算
			delta_z_Change();

			//透視画像のラインを更新する
			UpdateLine();

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == cwneDelta_z) cmdOK.Enabled = true;
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
            //if (KEvntLock) return;   //代入時のイベントロック 追加2014/06/23(検S1)hata
            if (EvntLock) return;   //Load時のイベントロック 追加2014/06/23(検S1)hata

            //(コーンビームの)マルチスキャンでスライス枚数が奇数(1枚を除く)のときは1枚減らす 'v17.43追加 byやまおか 2011/01/27
			if ((optMultiScanMode3.Checked == true) && ((cwneK.Value % 2) == 1) && (cwneK.Value != 1))
			{
                //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                //cwneK.Value = cwneK.Value - 1;
                if ((cwneK.Maximum >= cwneK.Value - 1) && (cwneK.Minimum <= cwneK.Value - 1))
                {
                    //代入時のイベントロック
                    //KEvntLock = true;
                    cwneK.Value = cwneK.Value - 1;
                    //KEvntLock = false;
                }
                //追加2015/01/29hata
                else if (cwneK.Maximum < cwneK.Value - 1)
                {
                    cwneK.Value = cwneK.Maximum;
                }
                else if (cwneK.Minimum > cwneK.Value - 1)
                {
                    cwneK.Value = cwneK.Minimum;
                }
            }

			//各パラメータの再計算
			K_Change();

			//透視画像のラインを更新する
			UpdateLine();

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == cwneK) cmdOK.Enabled = true;
		}


#region 【C#コントロールで代用】
/*
		//*******************************************************************************
		//機　　能： スライス枚数アップダウンボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V17.43  11/01/27    やまおか    新規作成
		//*******************************************************************************
		Private Sub cwneK_IncDecButtonClicked(ByVal Value As Boolean)

			'(コーンビーム)マルチスキャンでインクリメントのときは
			If Value And (optMultiScanMode(3).Value = True) Then
					cwneK.Value = cwneK.Value + 2
			End If

		End Sub
*/
#endregion

		// 【C#コントロールで代用】
		// (コーンビーム)マルチスキャン時に cwneK コントロールからのスライス枚数増減を2枚ずつにするため
		// コントロールをアクティブにした時に, マルチスキャンモード設定によって Increment値 を変更する
		private void cwneK_Enter(object sender, EventArgs e)
		{
			if (optMultiScanMode3.Checked == true)
			{
				cwneK.Increment = 2;
			}
			else
			{
				cwneK.Increment = 1;
			}
		}


		//*******************************************************************************
		//機　　能： ヘリカルピッチ変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cwneZp_ValueChanged(object sender, EventArgs e)
		{
            //if (ZpEvntLock) return;	//代入時のイベントロック 追加2014/06/23(検S1)hata
            if (EvntLock) return;   //Load時のイベントロック 追加2014/06/23(検S1)hata
            
            //各パラメータの再計算
			Zp_Change();

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == cwneZp) cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： スキャン昇降範囲（開始位置）の更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void cwneZdas_ValueChanged(object sender, EventArgs e)
		{
			//各パラメータの再計算
			Zdas_Change();

			//再構成棒グラフの更新
			//UpdateBar
		}


		//*******************************************************************************
		//機　　能： kswの更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： コーンの場合、スキャンモードまたはオートセンタリング有無を変更した場合、この関数を呼び出す
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
        //変更2014/10/07hata_v19.51反映
        //private void UpdateKSW()
        public void UpdateKSW()     //v19.50 Public化 v19.41とv18.02の統合 by長野 2013/11/05
        {
			int kBack = 0;					//v15.01追加ここから by 間々田 2009/09/01
			bool IsKSWChange = false;
			IsKSWChange = false;			//v15.01追加ここまで by 間々田 2009/09/01

			//スキャンモード
			int ScanModeIndex = 0;
			ScanModeIndex = modLibrary.GetOption(optScanMode);

			//スキャンモードが不定の場合、何もしない
			if (ScanModeIndex == -1) return;

			//オートセンタリング有無が不定の場合、何もしない
			if (modLibrary.GetOption(optAutoCentering) == -1) return;

			//■スキャン昇降範囲

			//kswの計算
			//ksw = 1 - Sin(Theta_0(scan_mode) / 2) / Cos(theta_off)
			//ksw = 1 - Sin(scancondpar.theta0(scan_mode) / 2) / Cos(scancondpar.thetaoff) 'v11.2変更 by 間々田 2005/10/04
			//V11.2変更 by 山本 2005-12-19 オートセンタリングありの場合は、上下方向の再構成範囲を小さくしておく Conebeam.exe実行時にスライスピッチを再設定されるのを防ぐため
			if (optAutoCentering1.Checked)
			{
				//ksw = 1 - Sin(theta0MaxCone / 2)

                //変更2014/10/07hata_v19.51反映
                //changed by 山本　2006-11-2　オフセットスキャンでオートセンタリングありの場合の最大スライスピッチが間違っている対策
				//if (ScanModeIndex == (int)ScanSel.ScanModeConstants.ScanModeOffset)		//オフセットスキャンの場合
				//オフセットスキャン、シフトスキャンの場合   'v18.00変更 byやまおか 2011/02/03
				//v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                if (Convert.ToBoolean(ScanModeIndex == (int)ScanSel.ScanModeConstants.ScanModeOffset) |
                    Convert.ToBoolean(ScanModeIndex == (int)ScanSel.ScanModeConstants.ScanModeShift))
                {
                    ksw = 1 - Math.Sin(CTSettings.scancondpar.Data.cone_max_mfanangle) / Math.Cos(CTSettings.scancondpar.Data.cone_max_mfanangle / 2);
				}
				else																	//ハーフ、フルスキャンの場合
				{
                    ksw = 1 - Math.Sin(CTSettings.scancondpar.Data.cone_max_mfanangle / 2);
				}

				//v11.2追加 by 間々田 2005/01/12
				if (ksw < modCT30K.LastKsw)
				{
                    //変更2014/10/07hata_v19.51反映
                    //if (this.ActiveControl == optScanMode[(int)ScanSel.ScanModeConstants.ScanModeOffset])					//v15.01追加 原因別にメッセージの内容を変更 by 間々田 2009/09/02
				    //v18.00変更 byやまおか 2011/02/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    if ((this.ActiveControl == optScanMode[(int)ScanSel.ScanModeConstants.ScanModeOffset]) |
                        (this.ActiveControl == optScanMode[(int)ScanSel.ScanModeConstants.ScanModeShift]))
                    {
						//メッセージ表示：
						//                MsgBox "オフセットスキャンにすると、上下方向の再構成範囲が小さくなる可能性があるため、" & vbCr & _
						//'                       "スライスピッチまたはスライス枚数が自動的に再設定されます。", vbExclamation
						MessageBox.Show(CTResources.LoadResString(20071), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);		//ストリングテーブル化 'v17.60 by長野 2011/05/22
					}
					else if (this.ActiveControl == optAutoCentering[1])
					{
						//メッセージ表示：
						//MsgBox "オートセンタリングをありにすると、上下方向の再構成範囲が小さくなる可能性があるため、" & vbCr & _
						//'       "スライスピッチまたはスライス枚数が自動的に再設定されます。", vbExclamation
						MessageBox.Show(CTResources.LoadResString(20072), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);		//ストリングテーブル化 'v17.60 by長野 2011/05/22
					}

					IsKSWChange = true;				//v15.01追加 by 間々田 2009/09/01
				}
			}
			else
			{
				float Mode = 0;
                //変更2014/10/07hata_v19.51反映
                //Mode = (ScanModeIndex == (int)ScanSel.ScanModeConstants.ScanModeOffset ? 1 : 0);
                //v18.00変更 byやまおか 2011/07/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                if ((ScanModeIndex == (int)ScanSel.ScanModeConstants.ScanModeOffset))
                {
                    Mode = 1;
                }
                else if ((ScanModeIndex == (int)ScanSel.ScanModeConstants.ScanModeShift))
                {
                    Mode = 2;
                } 
                ksw = 1 - Math.Sin(CTSettings.scancondpar.Data.theta0[(int)Mode] / 2) / Math.Cos(CTSettings.scancondpar.Data.thetaoff);
			}

			modCT30K.LastKsw = (float)ksw;				//v11.2追加 by 間々田 2005/01/12

			//'最大スライス厚の設定              'v15.01削除ここから by 間々田 2009/09/01
			//UpdateSWMax
			//
			//'最大スライス枚数の設定
			//UpdateKMax
			//
			//'最大スライスピッチの設定
			//UpdateDeltaZMax
			//
			//'最大ヘリカルピッチの設定
			//UpdateZpMax                        'v15.01削除ここまで by 間々田 2009/09/01


			if (IsKSWChange)						//v15.01追加ここから by 間々田 2009/09/01
			{
				//スライス枚数のバックアップ
                //2014/11/07hata キャストの修正
                //kBack = (int)cwneK.Value;
                kBack = Convert.ToInt32(cwneK.Value);

				//一時的に１にする
                //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                //cwneK.Value = 1;
                if ((cwneK.Maximum >= 1)  && (cwneK.Minimum <= 1))
                {   //代入時のイベントロック
                    //KEvntLock = true;
                    cwneK.Value = 1;
                    //KEvntLock = false;
                }
                //追加2015/01/29hata
                else if (cwneK.Maximum < 1)
                {
                    cwneK.Value = cwneK.Maximum;
                }
                else if (cwneK.Minimum > 1)
                {
                    cwneK.Value = cwneK.Minimum;
                }
            }

			//ZWを更新するため以下の処理を行う
			SW_Change();

			//スライス枚数を元に戻す
			if (IsKSWChange)
			{
                //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                //cwneK.Value = kBack;
                //Rev20.00 変更 by長野 2015/01/29
                if(cwneK.Maximum <= kBack)
                {
                    cwneK.Value = cwneK.Maximum;
                }
                else if (cwneK.Minimum >= kBack)
                {
                    cwneK.Value = cwneK.Minimum;
                }
                else
                {
                    cwneK.Value = kBack;
                }
                //if ((cwneK.Maximum >= kBack) && (cwneK.Minimum <= kBack))
                //{   //代入時のイベントロック
                //    //KEvntLock = true;
                //    cwneK.Value = kBack;
                //    //KEvntLock = false;
                //}           
            }										//v15.01追加ここまで by 間々田 2009/09/01
		}


		//*******************************************************************************
		//機　　能： コーンビーム用に必要なパラメータを計算する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void GetParaForCone()
		{
			float ic = 0;			//透視画像中心座標
			float jc = 0;			//透視画像中心座標
            //2014/11/07hata キャストの修正
            //ic = (CTSettings.scancondpar.Data.fimage_hsize - 1) / 2;
            //jc = (CTSettings.scancondpar.Data.fimage_vsize - 1) / 2;
            //Rev23.20 修正 by長野 2016/01/22
            //ic = Convert.ToInt32((CTSettings.scancondpar.Data.fimage_hsize - 1) / 2F);
            //if (CTSettings.scaninh.Data.lr_sft == 0 && ScanCorrect.IsShiftScan())
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
            if ((CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn) && (ScanCorrect.IsShiftScan() || ScanCorrect.IsW_Scan()))
            {
                ic = Convert.ToInt32((CTSettings.scancondpar.Data.fimage_hsize - 1) / 2F) + CTSettings.scancondpar.Data.det_sft_pix_r;
            }
            //else if (CTSettings.scaninh.Data.lr_sft == 1 && ScanCorrect.IsShiftScan())
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
            else if ((CTSettings.scaninh.Data.lr_sft == 1 && !CTSettings.W_ScanOn) && (ScanCorrect.IsShiftScan() || ScanCorrect.IsW_Scan()))
            {
                ic = Convert.ToInt32((CTSettings.scancondpar.Data.fimage_hsize - 1) / 2F) + CTSettings.scancondpar.Data.det_sft_pix;
            }
            else
            {
                ic = Convert.ToInt32((CTSettings.scancondpar.Data.fimage_hsize - 1) / 2F);
            }
            
            jc = Convert.ToInt32((CTSettings.scancondpar.Data.fimage_vsize - 1) / 2F);

            

			//２次元幾何歪                           'v11.2追加ここから by 間々田 2005/10/04
			if (CTSettings.scaninh.Data.full_distortion == 0)
			{
				float S = 0;
				float e = 0;
                float S_sft = 0;
                float e_sft = 0;
                //Rev23.20 左右シフト対応 by長野 2015/11/19
                float sft = 0;
                float sft_val = 0;
                sft = CTSettings.scancondpar.Data.det_sft_pix;
                //sft_R = CTSettings.scancondpar.Data.det_sft_pix_r;
                //sft_L = CTSettings.scancondpar.Data.det_sft_pix_l;


                //変更2014/10/07hata_v19.51反映
                //S = CTSettings.scancondpar.Data.scan_posi_b[2] + jc + CTSettings.scancondpar.Data.scan_posi_a[2] * (CTSettings.scancondpar.Data.ist + 2 - ic);
                ////e = CTSettings.scancondpar.Data.scan_posi_b[2] + jc + CTSettings.scancondpar.Data.scan_posi_a[2] * (CTSettings.scancondpar.Data.ied - 2 - ic);
                //e = CTSettings.scancondpar.Data.scan_posi_b[2] + jc + CTSettings.scancondpar.Data.scan_posi_a[2] * ((CTSettings.scancondpar.Data.ied - sft_val) - 2 - ic);                //v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                //S_sft = CTSettings.scancondpar.Data.scan_posi_b[2] + jc + CTSettings.scancondpar.Data.scan_posi_a[2] * (CTSettings.scancondpar.Data.ist + 2 - (ic + sft));                //v18.00シフト対応 byやまおか 2011/07/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                //e_sft = CTSettings.scancondpar.Data.scan_posi_b[2] + jc + CTSettings.scancondpar.Data.scan_posi_a[2] * (CTSettings.scancondpar.Data.ied - 2 - (ic + sft));                //v18.00シフト対応 byやまおか 2011/07/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

                //Rev23.20 左右シフト対応 by長野 2015/11/20
                //変更2014/10/07hata_v19.51反映
                S = modScanCondition.real_scan_posi_b + jc + CTSettings.scancondpar.Data.scan_posi_a[2] * (CTSettings.scancondpar.Data.ist + 2 - ic);
                //e = CTSettings.scancondpar.Data.scan_posi_b[2] + jc + CTSettings.scancondpar.Data.scan_posi_a[2] * (CTSettings.scancondpar.Data.ied - 2 - ic);
                e = modScanCondition.real_scan_posi_b + jc + CTSettings.scancondpar.Data.scan_posi_a[2] * ((CTSettings.scancondpar.Data.ied - sft_val) - 2 - ic);                //v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                S_sft = modScanCondition.real_scan_posi_b + jc + CTSettings.scancondpar.Data.scan_posi_a[2] * (CTSettings.scancondpar.Data.ist + 2 - (ic + sft));                //v18.00シフト対応 byやまおか 2011/07/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                e_sft = modScanCondition.real_scan_posi_b + jc + CTSettings.scancondpar.Data.scan_posi_a[2] * (CTSettings.scancondpar.Data.ied - 2 - (ic + sft));

                //Rev20.00 コモンの変更
                //mc_max = (int)modLibrary.MinVal(S - CTSettings.scancondpar.Data.jst, CTSettings.scancondpar.Data.jed - S, e - CTSettings.scancondpar.Data.jst, CTSettings.scancondpar.Data.jed - e);
                //2014/11/07hata キャストの修正
                //mc_max[0] = (int)modLibrary.MinVal(S - CTSettings.scancondpar.Data.jst, CTSettings.scancondpar.Data.jed - S, e - CTSettings.scancondpar.Data.jst, CTSettings.scancondpar.Data.jed - e);                  //v18.00シフト対応 byやまおか 2011/07/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                //mc_max[1] = (int)modLibrary.MinVal(S_sft - CTSettings.scancondpar.Data.jst, CTSettings.scancondpar.Data.jed - S_sft, e_sft - CTSettings.scancondpar.Data.jst, CTSettings.scancondpar.Data.jed - e_sft);  //v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                mc_max[0] = Convert.ToInt32(modLibrary.MinVal(S - CTSettings.scancondpar.Data.jst, CTSettings.scancondpar.Data.jed - S, e - CTSettings.scancondpar.Data.jst, CTSettings.scancondpar.Data.jed - e));                  //v18.00シフト対応 byやまおか 2011/07/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                mc_max[1] = Convert.ToInt32(modLibrary.MinVal(S_sft - CTSettings.scancondpar.Data.jst, CTSettings.scancondpar.Data.jed - S_sft, e_sft - CTSettings.scancondpar.Data.jst, CTSettings.scancondpar.Data.jed - e_sft));  //v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            }
			//１次元幾何歪（従来の方法）
			else									//v11.2追加ここまで by 間々田 2005/10/04
			{
                //Rev20.00 1次元幾何歪補正はもうやらないため不要 by長野 2014/11/10 
                //int kv = 0;
                //float r = 0;
                //int mm = 0;
                //float delta_Jp = 0;
                //float temp = 0;
                
                ////幾何歪テーブル読み込み     'V3.0 append by 鈴山
                //if (!ScanCorrect.Read_HizumiTable(ref hizumi)) {
                //    //メッセージ表示：
                //    //   ～が見つかりません。
                //    //   先に幾何学歪校正を行ってください。
                //    //Interaction.MsgBox(StringTable.GetResString(StringTable.IDS_NotFound, ScanCorrect.HIZUMI_CSV) + Constants.vbCrLf + CT30K.My.Resources.str9596, MsgBoxStyle.Exclamation);
                //    MessageBox.Show(StringTable.GetResString(StringTable.IDS_NotFound, ScanCorrect.HIZUMI_CSV) + "\r\n" + CTResources.LoadResString(9596), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //}

                ////2014/11/07hata キャストの修正
                ////kv = (int)(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);
                //kv = Convert.ToInt32(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);

                ////r = Sqr((ic ^ 2) + (jc ^ 2))       'deleted by 山本　2002-6-18
                //r = (float)Math.Sqrt((Math.Pow(ic, 2)) + (Math.Pow((kv * jc), 2)));				//added by 山本　2002-6-18

                ////2014/11/07hata キャストの修正
                ////mm =(int)(2 * r);
                //mm = Convert.ToInt32(Math.Floor(2 * r));
                //if ((mm < hizumi.GetLowerBound(0)) | (mm > hizumi.GetUpperBound(0)))
                //{
                //    mm = hizumi.GetLowerBound(0);
                //}

                ////ΔJpの計算
                ////2014/11/07hata キャストの修正
                ////delta_Jp = (int)(jc * hizumi[mm] + 3);
                //delta_Jp = Convert.ToInt32(jc * hizumi[mm] + 3);
                //if (delta_Jp < 3)
                //    delta_Jp = 3;       //v7.0 change by 間々田 2003/09/09

                ////mc_maxを計算する
                //temp = modLibrary.MinVal(CTSettings.scancondpar.Data.scan_posi_b[2] + jc - CTSettings.scancondpar.Data.scan_posi_a[2] * ic,
                //                         jc - CTSettings.scancondpar.Data.scan_posi_b[2] + CTSettings.scancondpar.Data.scan_posi_a[2] * ic,
                //                          CTSettings.scancondpar.Data.scan_posi_b[2] + jc + CTSettings.scancondpar.Data.scan_posi_a[2] * ic,
                //                         jc - CTSettings.scancondpar.Data.scan_posi_b[2] - CTSettings.scancondpar.Data.scan_posi_a[2] * ic);

                
                
                ////mc_max = CInt(Temp - delta_Jp)      'V3.0 change by 鈴山 2000/10/02 (CIntでｷｬｽﾄ)
                ////'mc_max = CInt((Temp - delta_Jp) * 0.594)    'V3.0 change by 山本 2000/10/07  'V4.0 by 山本 2001-4-14 画面いっぱいにデータを取れるように変更
                ////mc_max = CInt((temp - delta_Jp) * 0.9)                                              'v10.0変更 by 間々田 2005/02/17　上下のボケ対策
                ////2014/11/07hata キャストの修正
                ////mc_max[1] = (int)((temp - delta_Jp) * 0.9);				//v18.00シフト対応 byやまおか 2011/07/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                //mc_max[1] = Convert.ToInt32((temp - delta_Jp) * 0.9);				//v18.00シフト対応 byやまおか 2011/07/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            
            }
			//v11.2追加 by 間々田 2005/10/04

			//v29.99 ヘリカルは今のところ不要だが、コーンにも必要なパラメータのため生かしておく by長野 2013/04/08'''''ここから'''''
			//ヘリカル用パラメータ計算
            kzp = (float)(1 + CTSettings.scancondpar.Data.alpha / ScanCorrect.Pai);
			//v29.99 ヘリカルは今のところ不要だが、コーンにも必要なパラメータのため生かしておく by長野 2013/04/08'''''ここまで'''''
		}


		//*******************************************************************************
		//機　　能： ヘリカルモードオプションボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void optHelical_CheckedChanged(object sender, EventArgs e)
		{
			if (sender as RadioButton == null || ((RadioButton)sender).Checked == false) return;
			int Index = Array.IndexOf(optHelical, sender);
			if (Index < 0) return;


			float LmtPos = 0;

			//Indexで分岐(0:非ﾍﾘｶﾙ,1:ﾍﾘｶﾙ)
			if (optHelical0.Checked)
			{
				//各パラメータの再計算
				SW_Change();
				delta_z_Change();
				K_Change();
			}
			else
			{
				//        'あとで
				//        ''Zpmaxの初期値が負の場合
				//        'If (cwneZp.Maximum < 0) Then
				//        '
				//        '    LmtPos = GValUpperLimit - cwneDelta_z.value * (cwneK.value - 1)
				//        '    'メッセージ表示：
				//        '    '   ヘリカルスキャンを行うためには、現在位置が低すぎます。
				//        '    '   メカ準備画面でテーブルを ～mm より上に上げてください。
				//        '    '   （高さ位置の数値を小さくしてください。)
				//        '    MsgBox GetResString(9592, Format$(LmtPos, "###0.000")), vbCritical
				//        '
				//        '    '非ヘリカル条件入力モードに切替
				//        '    optHelical(0).value = True
				//        '    Exit Sub
				//        '
				//        'End If


                //追加2014/10/07hata_v19.51反映
                //スキャン開始位置の判定
                LmtPos = CTSettings.GValUpperLimit - (float)cwneZp.Value * kzp / 2F;
                if ((float)cwneZdas.Value > LmtPos)
                {
                    //メッセージ表示：
                    //   ヘリカルスキャンを行うためには、現在位置が低すぎます。
                    //   テーブルを %1mm より上に上げてください。
                    //   （高さ位置の数値を小さくしてください。)
                    //Interaction.MsgBox(StringTable.GetResString(9592, Microsoft.VisualBasic.Compatibility.VB6.Support.Format(LmtPos, "###0.000")), MsgBoxStyle.Critical);
                    MessageBox.Show(StringTable.GetResString(9592, LmtPos.ToString("###0.000")), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    optHelical[0].Checked = true;
                    return;
                }

                //各パラメータの再計算
                K_Change();
                delta_z_Change();
                Zdas_Change();
                SW_Change();
                Zp_Change();

			}

			//ヘリカル用フレームの表示・非表示の設定
			fraUDRange.Visible = optHelical1.Checked;
			fraScanStartRange.Visible = optHelical1.Checked;
			fraHelicalPitch.Visible = optHelical1.Checked;

			//再構成範囲の棒グラフを表示する
			//UpdateBar

			//オートセンタリングを選択可・不可を設定する
			AutoCenteringEnabled();

			//変更した場合，ＯＫボタンを使用可にする
			if (this.ActiveControl == optHelical[Index]) cmdOK.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： スライス枚数最大値変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void KMaxChange(int NewMaxValue)
		{
            //NewMaxValueが0以下の場合がるため
            //cwneK.Maximum = modLibrary.MinVal(NewMaxValue, CTSettings.scancondpar.Data.klimit);
            if (NewMaxValue > 0)
            {
                cwneK.Maximum = (decimal)modLibrary.MinVal(NewMaxValue, CTSettings.scancondpar.Data.klimit);
            }

			//最大値表示
			lblKmax.Text = cwneK.Maximum.ToString(string.Format("F{0}", cwneK.DecimalPlaces));
		}


		//*******************************************************************************
		//機　　能： スライス厚最大値変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void SWMaxChange(float NewMaxValue)
		{
			float theMin = 0;
			float theMax = 0;

			var _with21 = cwneSlice;

			//        theMin = Int(MaxVal(SWMinCone, 0.001) / .DiscreteInterval) * .DiscreteInterval
			//        theMax = Int(MaxVal(NewMaxValue, theMin) / .DiscreteInterval) * .DiscreteInterval

			//v19.00
			//最小スライス厚の小数点第４位以下で四捨五入すると、理論値以下を設定できるようになるため、
			//必ず切り上げるようにする。
			//theMin = Val(Format$(MaxVal(SWMinCone, 0.001), .FormatString))
			//float.TryParse((modLibrary.MaxVal(SWMinCone, 0.001) + 0.0009999).ToString(string.Format("F{0}", cwneSlice.DecimalPlaces)), out theMin);
            //Rev20.00 戻す by長野 2015/01/26           
            //float.TryParse((modLibrary.MaxVal(SWMinCone, 0.001)).ToString(string.Format("F{0}", cwneSlice.DecimalPlaces)), out theMin);
            //Rev23.00 変更 最小は切り上げないと1画素未満になってしまう by長野 2015/09/14
            theMin = (float)(modLibrary.MaxVal(SWMinCone, 0.001));
            float tmp_theMin = (float)Math.Ceiling(theMin * 1000.0);
            theMin = (float)(tmp_theMin / 1000.0);


#region 【C#コントロールで代用】
//			theMax = Int(MaxVal(NewMaxValue, theMin) / .DiscreteInterval) * .DiscreteInterval
#endregion
            //2014/11/07hata キャストの修正
            //theMax = (int)(modLibrary.MaxVal(NewMaxValue, theMin) / (float)cwneSlice.Increment) * (float)cwneSlice.Increment;
            theMax = (float)(Math.Floor(modLibrary.MaxVal(NewMaxValue, theMin) / (double)cwneSlice.Increment) * (float)cwneSlice.Increment);

			//スライス厚の最大値を100に抑える
			if (theMax / theMin > 100) theMax = 100 * theMin;

            //Rev20.00 追加 四捨五入 by長野 2015/01/26
            //cwneSlice.Minimum = (decimal)theMin;
            cwneSlice.Minimum = Math.Round((decimal)theMin / cwneSlice.Increment, MidpointRounding.AwayFromZero) * cwneSlice.Increment;
			//cwneSlice.Maximum = (decimal)theMax;
            cwneSlice.Maximum = Math.Round((decimal)theMax / cwneSlice.Increment,MidpointRounding.AwayFromZero) * cwneSlice.Increment;

			//最大値の表示
			lblSliceMinMax.Text = StringTable.GetResString(StringTable.IDS_RangeMM, 
														   cwneSlice.Minimum.ToString(string.Format("F{0}", cwneSlice.DecimalPlaces)), 
														   cwneSlice.Maximum.ToString(string.Format("F{0}", cwneSlice.DecimalPlaces)));
		}


		//*******************************************************************************
		//機　　能： スライスピッチ最大値変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void DeltaZMaxChange(float NewMaxValue)
		{
			//最大値設定
            //cwneDelta_z.Maximum = (decimal)(modLibrary.MaxVal(NewMaxValue, (float)cwneDelta_z.Minimum) * 1000) / 1000;
            //Rev20.00 1000倍したらIntにキャスト、その後1000で割ってdecimalにキャストする by長野 2014/11/04
            float tmp = 0.0f;
            tmp = (modLibrary.MaxVal(NewMaxValue, (float)cwneDelta_z.Minimum));
            //2014/11/07hata キャストの修正
            //cwneDelta_z.Maximum = (decimal)((float)(Convert.ToInt32(tmp * 1000)) / 1000.0f);
            cwneDelta_z.Maximum = (decimal)((float)(Convert.ToInt32(Math.Floor(tmp * 1000))) / 1000.0f);

			//表示
			lbldelta_zmax.Text = cwneDelta_z.Maximum.ToString(string.Format("F{0}", cwneDelta_z.DecimalPlaces));
		}


		//*******************************************************************************
		//機　　能： ヘリカルピッチ最大値変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void ZpMaxChange(float NewMaxValue)
		{
			//最大値設定
            //変更2015/01/30hata
			//cwneZp.Maximum = (decimal)modLibrary.MaxVal(NewMaxValue, (float)cwneZp.Minimum);
            cwneZp.Maximum = Math.Round((decimal)modLibrary.MaxVal(NewMaxValue, (float)cwneZp.Minimum) / cwneZp.Increment, MidpointRounding.AwayFromZero) * cwneZp.Increment;

            //表示
			lblZpmax.Text = cwneZp.Maximum.ToString(string.Format("F{0}", cwneZp.DecimalPlaces));
		}


		//*******************************************************************************
		//機　　能： データ収集ビュー数の更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： オーバースキャン有無・スキャンモード・オートセンタリング・設定ビュー数を
		//           変更したらこの関数を呼ぶ必要がある
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*******************************************************************************
		private void UpdateAcqView()
		{
			float DeltaFaiM = 0;
			int AcqView = 0;

			try
			{
				//v29.99 今のところ不要 by長野 2013/04/11'''''ここから'''''
				//オーバースキャン有無が不定の場合、何もしない
				//    If GetOption(optOverScan) = -1 Then Exit Sub
				//v29.99 今のところ不要 by長野 2013/04/11'''''ここまで'''''

				//スキャンモードが不定の場合、何もしない
				if (modLibrary.GetOption(optScanMode) == -1) return;

				//オートセンタリング有無が不定の場合、何もしない
				if (modLibrary.GetOption(optAutoCentering) == -1) return;

				DeltaFaiM = (float)(2 * ScanCorrect.Pai / (double)cwneViewNum.Value);			//changed by 山本　2005-10-6     Pai / scan_view　->  2 * Pai / scan_view

				//オーバースキャンありの場合
				if ((CTSettings.scaninh.Data.over_scan == 0) && optOverScan1.Checked)
				{
					//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
					//フル／オフセット
					//        If Not optScanMode(ScanModeHalf).Value Then
					//
					//            AcqView = cwneViewNum.Value + 2 * (Int(scancondpar.alpha_h / DeltaFaiM) + 1)
					//
					//        'ハーフでオートセンタリングがあり
					//        ElseIf optAutoCentering(1).Value Then
					//
					//            AcqView = Int((Pai + scancondpar.cone_max_mfanangle + 2 * scancondpar.alpha_h) / DeltaFaiM) + 1
					//
					//        'ハーフでオートセンタリングがなし
					//        Else
					//
					//            AcqView = Int((Pai + scancondpar.theta0(0) + 2 * scancondpar.alpha_h) / DeltaFaiM) + 1
					//
					//        End If
					//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
				}
				//オーバースキャンなしの場合
				else
				{

					//フル／オフセット
					if (!optScanMode[(int)ScanSel.ScanModeConstants.ScanModeHalf].Checked)
					{
                        //2014/11/07hata キャストの修正
                        //AcqView = (int)cwneViewNum.Value;
                        AcqView = Convert.ToInt32(cwneViewNum.Value);
                    }
					//ハーフでオートセンタリングがあり
					else if (optAutoCentering1.Checked)
					{
                        //2014/11/07hata キャストの修正
                        //AcqView = (int)((float)cwneViewNum.Value * (180 + CTSettings.scancondpar.Data.cone_max_mfanangle * 180 / ScanCorrect.Pai) / 360) + 1;
                        AcqView = Convert.ToInt32(Math.Floor((float)cwneViewNum.Value * (180 + CTSettings.scancondpar.Data.cone_max_mfanangle * 180F / ScanCorrect.Pai) / 360F)) + 1;
                    }
					//ハーフでオートセンタリングがなし
					else
					{
                        //2014/11/07hata キャストの修正
                        //AcqView = (int)((float)cwneViewNum.Value * (180 + CTSettings.scancondpar.Data.theta0[0] * 180 / ScanCorrect.Pai) / 360) + 1;
                        AcqView = Convert.ToInt32(Math.Floor((float)cwneViewNum.Value * (180 + CTSettings.scancondpar.Data.theta0[0] * 180F / ScanCorrect.Pai) / 360F)) + 1;
                    }
				}

				//表示
				lblAcqView.Text = AcqView.ToString("###0");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\r" + "\r" + "At UpdateAcqView of " + this.Name, 
								Application.ProductName, MessageBoxButtons.OK);
			}
			return;
		}


		//*******************************************************************************
		//機　　能： データ収集ビュー数変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*******************************************************************************
		private void lblAcqView_TextChanged(object sender, EventArgs e)
		{
			//ビュー数による最大mcの計算(最後のマイナス２は余裕）　added by 山本　2003-9-27
			//mcl = (CLng(1024) * CLng(1024) * CLng(1024) / scan_view / h - 1) / 2 - 2
			//mcl = (CLng(1024) * CLng(1024) * CLng(1024) / Val(lblAcqView.Caption) / scancondpar.fimage_hsize - 1) / 2 - 2         'v9.6 変更 by 間々田 2004/10/22 オーバーフロー対策
            
            //縦中心チャンネル(ライン数半幅) 'v17.00変更 byやまおか 2010/02/25
            //一度に計算するとオーバーフローするため3段階に分ける
            mcl = Convert.ToInt32(1000) * Convert.ToInt32(1000);			//第1段階
            //mcl = mcl * Convert.ToInt32(16) * Convert.ToInt32(1000);		//第2段階

            //Rev20.00 ステップ・連続に関わらず iniの値にする。 by長野 2014/12/15
            ////Rev20.00 テーブル連続回転の場合は、ソース埋め込みの値ではなくCT30k.iniの値を使用する by長野 2014/09/18
            //if (optTableRotation0.Checked)
            //{
            //    mcl = mcl * Convert.ToInt32(16) * Convert.ToInt32(1000);		//第2段階
            //}
            //else
            //{
            //    //Rev20.00 式変更 by長野 2014/09/11
            //    //mcl = mcl * Convert.ToInt32(CTSettings.iniValue.SharedMemSize) * Convert.ToInt32(1000);		//第2段階
            //    //Rev20.00 修正 by長野 2014/12/15
            //    mcl = mcl * Convert.ToInt32(CTSettings.iniValue.SharedMemSize);		//第2段階
            //}

            mcl = mcl * Convert.ToInt32(CTSettings.iniValue.SharedMemSize);

            float lblAcqViewTextValue = 0;
            float.TryParse(lblAcqView.Text, out lblAcqViewTextValue);
            //2014/11/07hata キャストの修正
            //mcl = (mcl / lblAcqViewTextValue / CTSettings.scancondpar.Data.fimage_hsize - 1) / 2 - 2;		//第3段階
            //mcl = modLibrary.MinVal(mcl, (float)((CTSettings.scancondpar.Data.fimage_vsize - 1) / 2));		//縦サイズを超えないように
            //Rev20.00  修正 mclは半分の幅なのでさらに２で割る必要がある。by長野 2014/12/15
            //mcl = (mcl / lblAcqViewTextValue / (float)CTSettings.scancondpar.Data.fimage_hsize - 1) / 2F - 2;		//第3段階
            mcl = (mcl / lblAcqViewTextValue / (float)CTSettings.scancondpar.Data.fimage_hsize - 1) / 2F / 2F - 2;		//第3段階
            mcl = modLibrary.MinVal(mcl, (float)((CTSettings.scancondpar.Data.fimage_vsize - 1) / 2F));		//縦サイズを超えないように

            //追加2014/10/07hata_v19.51反映
            //v19.51 シフトの場合、mcが大きいとセンタリングデータがCUDAのテクスチャメモリを超えてしまうため0.7掛けする by長野 2014/03/04
            //シフト115mmを参考に係数を決めた。検討する必要あり
            //if (optScanMode[4].Checked == true)
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
            //if (optScanMode[4].Checked == true && CTSettings.scaninh.Data.lr_sft == 1)
            //Rev26.10 条件変更 by chouno 2018/01/18
            //if(CTSettings.W_ScanOn && chkW_Scan.Checked == true)
            //{
            //    //Rev26.10 add by chouno 2017/01/05
            //    if (CTSettings.detectorParam.h_size <= 1024)
            //    {
            //        mcl = mcl * 0.6F;
            //    }
            //    else
            //    {
            //        mcl = mcl * 0.5F;
            //    }
            //}
            ////else
            //else if(optScanMode[4].Checked == true) //Rev26.10 add by chouno 2018/01/13
            //{
            //    //Rev26.10 変更 by chouno 2018/01/13
            //    //if (optScanMode[4].Checked == true && (CTSettings.scaninh.Data.lr_sft == 1))
            //    //{
            //    //    mcl = mcl * 0.7F;
            //    //}
            //    //else if (optScanMode[4].Checked == true && CTSettings.scaninh.Data.lr_sft == 0)
            //    //{
            //    //    mcl = mcl * 0.5F;
            //    //}
            //    //Rev26.10 add by chouno 2017/01/05
            //    if (CTSettings.detectorParam.h_size <= 1024)
            //    {
            //        mcl = mcl * 0.6F;
            //    }
            //    else
            //    {
            //        mcl = mcl * 0.5F;
            //    }
            //}

            if ((CTSettings.W_ScanOn && chkW_Scan.Checked == true) || optScanMode[4].Checked == true)
            {
                mcl = mcl * CTSettings.iniValue.mclMagnify;
            }

            mcl = Convert.ToInt32(mcl);

            //mclを使用する箇所の再計算
			UpdateFcdFid();
		}


		//*******************************************************************************
		//機　　能： 透視画像のラインを更新する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void UpdateLine()
		{
			//if (modLibrary.IsExistForm(frmTransImage.Instance))
            if (modLibrary.IsExistForm("frmTransImage"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            {
				if (IsCone)
				{
                    frmTransImage.Instance.SetLine((double)cwneSlice.Value, (double)cwneK.Value, (double)cwneDelta_z.Value);
				}
				else
                {
                    if (modLibrary.GetOption(optMultislice) == -1) return;
                    frmTransImage.Instance.SetLine((double)cwneSlice.Value, (double)(modLibrary.GetOption(optMultislice)), (double)ntbMultislicePitch.Value);
                }
			}
		}


		//*************************************************************************************************
		//機　　能： 最大スライスピッチの計算
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void UpdateDeltaZMax()
		{
			//ヘリカル有無が不定の場合、何もしない
			if (modLibrary.GetOption(optHelical) == -1) return;

            //Rev20.00 追加 by長野 2014/12/04
            //kswが不定の場合、何もしない
            //Rev20.00 追加したが、元々必要なかったはずなので外す by長野 2015/01/26
            //if (ksw == null) return;

			//非ヘリカルの場合
			if (optHelical0.Checked)
			{
				//K>1の場合
				if (cwneK.Value > 1)
				{
					//最大スライスピッチの設定
					float value1 = 0;
					float value2 = 0;
					value1 = ZW / ((float)cwneK.Value - 1);
					value2 = (SWMaxParaV - (float)cwneSlice.Value) * (float)ksw / ((float)cwneK.Value - 1);
					DeltaZMaxChange(modLibrary.MinVal(value1, value2));
				}
				//K=1の場合
				else
				{
					//最大スライスピッチの設定
					DeltaZMaxChange(ZW);
				}
			}
			//ヘリカルの場合
			else
			{
				//最大スライスピッチの設定
                //2014/11/07hata キャストの修正
                //DeltaZMaxChange((CTSettings.GValUpperLimit - (float)cwneZp.Value * kzp / 2 - zs) / modLibrary.MaxVal((float)cwneK.Value - 1, 1));
                DeltaZMaxChange((CTSettings.GValUpperLimit - (float)cwneZp.Value * kzp / 2F - zs) / modLibrary.MaxVal((float)cwneK.Value - 1, 1F));
            }
		}


		//*************************************************************************************************
		//機　　能： 最大ヘリカルピッチの設定
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void UpdateZpMax()
		{
			//kswが不定の場合、何もしない
			if (ksw == null) return;

			float value1 = 0;
			float value2 = 0;

			value1 = (SWMaxPara - (float)cwneSlice.Value) * (float)ksw / kzp;
            value2 = (CTSettings.GValUpperLimit - (float)cwneDelta_z.Value * ((float)cwneK.Value - 1) - (float)cwneZdas.Value) / kzp;

			ZpMaxChange(modLibrary.MinVal(value1, value2));
		}


		//*************************************************************************************************
		//機　　能： 最大コーン枚数の設定
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void UpdateKMax()
		{
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//ヘリカル有無が不定の場合、何もしない
			//    If GetOption(optHelical) = -1 Then Exit Sub
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //Rev20.00 追加 by長野 2014/12/04
            //kswが不定の場合、何もしない
            if (ksw == null) return;

            //非ヘリカルの場合
			if (optHelical0.Checked)
			{
				int value1 = 0;
				int value2 = 0;
                //2014/11/07hata キャストの修正
                //value1 = (int)(ZW / (float)cwneDelta_z.Value) + 1;
                value1 = Convert.ToInt32(Math.Floor(ZW / (float)cwneDelta_z.Value)) + 1;
                
                //(コーンビームの)マルチスキャンでスライス枚数が奇数(1枚を除く)のときは1枚減らす 'v17.43追加 byやまおか 2011/02/01
				if ((optMultiScanMode3.Checked == true) && ((value1 % 2) == 1) && (value1 != 1))
				{
					value1 = value1 - 1;
				}
                //2014/11/07hata キャストの修正
                //value2 = (int)((SWMaxParaV - (float)cwneSlice.Value) * (float)ksw / (float)cwneDelta_z.Value) + 1;
                value2 = Convert.ToInt32(Math.Floor((SWMaxParaV - (float)cwneSlice.Value) * (float)ksw / (float)cwneDelta_z.Value)) + 1;
				
                KMaxChange(modLibrary.MinVal(value1, value2));
			}
			else
			{
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
				//        KMaxChange Int((GValUpperLimit - cwneZp.Value * kzp / 2 - zs) / cwneDelta_z.Value) + 1
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
			}
		}


		//*************************************************************************************************
		//機　　能： 最大スライス厚の設定
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足：
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void UpdateSWMax()
		{
			//kswが不定の場合、何もしない
			if (ksw == null) return;

			float value1 = 0;
			float value2 = 0;
			value1 = SWMaxPara - ((float)cwneK.Value - 1) * (float)cwneDelta_z.Value / (float)ksw;
			value2 = SWMaxParaV - ((float)cwneK.Value - 1) * (float)cwneDelta_z.Value / (float)ksw;

			SWMaxChange(modLibrary.MinVal(value1, value2));
		}


		//*************************************************************************************************
		//機　　能： スキャン昇降範囲（終了位置）の更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： ヘリカル用
		//
		//履　　歴： v15.0  09/01/21  (SS1)間々田    リニューアル
		//*************************************************************************************************
		private void UpdateZdae()
		{
			float Zdae = 0;
            //2014/11/07hata キャストの修正
            //Zdae = ze + (float)cwneZp.Value * kzp / 2;
            Zdae = ze + (float)cwneZp.Value * kzp / 2F;
            lblZdae.Text = Zdae.ToString("###0.000");
		}


		//*************************************************************************************************
		//機　　能： スキャン開始可能範囲（最大値）の更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： ヘリカル用
		//
		//履　　歴： v15.0  09/01/21  (SS1)間々田    リニューアル
		//*************************************************************************************************
		private void UpdateZdasmax()
		{
			float Zdasmax = 0;
            Zdasmax = CTSettings.GValUpperLimit - (float)cwneZp.Value * kzp - (float)cwneDelta_z.Value * ((float)cwneK.Value - 1);
			lblZdasmax.Text = Zdasmax.ToString("###0.000");
		}


		//*************************************************************************************************
		//機　　能： 再構成範囲開始位置更新処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.0  09/01/21  (SS1)間々田    リニューアル
		//*************************************************************************************************
		private void UpdateZs()
		{
			//ヘリカル有無が不定の場合、何もしない
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//    If GetOption(optHelical) = -1 Then Exit Sub
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//非ヘリカルの場合
			if (optHelical0.Checked)
			{
                //2014/11/07hata キャストの修正
                //zs = (float)(cwneZdas.Value - cwneDelta_z.Value * (cwneK.Value - 1) / 2);
                zs = (float)(cwneZdas.Value - cwneDelta_z.Value * (cwneK.Value - 1) / 2M);
            }
			else
			{
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
				//        zs = cwneZdas.Value + cwneZp.Value * kzp / 2
				//        UpdateDeltaZMax
				//        UpdateKMax
				//        UpdateZe
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
			}

			//表示：0.0001mmプラスして表示（単精度浮動小数点数型の問題に対処） 'v11.2変更 2006/01/25
			lblZs.Text = (zs + 0.0001F).ToString("###0.000");
		}


		//*************************************************************************************************
		//機　　能： 再構成範囲終了位置更新処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.0  09/01/21  (SS1)間々田    リニューアル
		//*************************************************************************************************
		private void UpdateZe()
		{
			//ヘリカル有無が不定の場合、何もしない
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//    If GetOption(optHelical) = -1 Then Exit Sub
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//非ヘリカルの場合
			if (optHelical0.Checked)
			{
                //2014/11/07hata キャストの修正
                //ze = (float)(cwneZdas.Value + cwneDelta_z.Value * (cwneK.Value - 1) / 2);
                ze = (float)(cwneZdas.Value + cwneDelta_z.Value * (cwneK.Value - 1) / 2M);
            }
			else
			{
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
				//        ze = zs + cwneDelta_z.Value * (cwneK.Value - 1)
				//        UpdateZdae
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
			}

			//表示：0.0001mmプラスして表示（単精度浮動小数点数型の問題に対処） 'v11.2変更 2006/01/25
			lblZe.Text = (ze + 0.0001F).ToString("###0.000");
		}


		//*******************************************************************************
		//機　　能： 「登録」ボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.0  09/01/21  (SS1)間々田    リニューアル
		//*******************************************************************************
		private void cmdSaveCondition_Click(object sender, EventArgs e)
		{
            //Rev26.11(特) 修正 ガイドモード有無で処理を分ける
            if (CTSettings.scaninh.Data.guide_mode == 0)
            {
                //リスト最大数のチェック
                if (modScanCondition.PresetPath.Count >= 20)
                {
                    //メッセージ表示：プリセットファイルの登録数が最大値を超えています。(最大20)
                    MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_NotSpecified, 26023), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                for (int cnt = 0; cnt < modScanCondition.PresetName.Count; cnt++)
                {
                    if (modScanCondition.PresetName[cnt] == txtPresetName.Text)
                    {
                        //メッセージ表示：指定されたプリセットファイル名は既に存在しているため、追加できません。
                        MessageBox.Show(CTResources.LoadResString(26027), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //スキャン条件の設定内容をチェック       '追加 by 間々田 2009/08/22
                if (!OptValueChk1()) return;

                //ファイル保存ダイアログ表示
                //string FileName = null;
                //FileName = modFileIO.GetFileName(StringTable.IDS_Save, CTResources.LoadResString(StringTable.IDS_CondFile), SubExtension: "-SC");			//スキャン条件ファイル
                //if (string.IsNullOrEmpty(FileName)) return;

                //プリセット名を指定していない
                if (string.IsNullOrEmpty(txtPresetName.Text.Trim()))
                {
                    //メッセージ表示：プリセット名が指定されていません。
                    MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_NotSpecified, 26020), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //スライス名に禁止文字が含まれている          
                else if (!modLibrary.FileNameProhibitionCheck(txtPresetName.Text))
                {
                    //メッセージ表示：
                    //   スライス名に以下の禁止文字を使用しないでください。
                    //   \ / : * ? < > | " Space
                    MessageBox.Show(CTResources.LoadResString(26021) + "\r" + "\r" + "\\ / : * ? < > | " + (char)34 + " Space", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string FileName = Path.Combine(AppValue.PathScanCondPresetFile, txtPresetName.Text) + "-SC.csv";

                //modScansel.scanselType theScansel = default(modScansel.scanselType);
                CTstr.SCANSEL theScansel = default(CTstr.SCANSEL);

                //この画面で設定しているスキャン条件を取得
                GetControls(ref theScansel);

                //プリセットファイルを登録する
                //if (modScanCondition.SaveSCFile(FileName, theScansel))
                if (modScanCondition.SaveSCFile(FileName, theScansel, txtPresetComment.Text)) //Rev26.00 change by chouno 2017/08/31
                {
                    //メッセージ表示：プリセットファイルを保存しました。
                    MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_Saved, 26022),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                //プリセットリストに追加
                modScanCondition.PresetName.Add(txtPresetName.Text);
                modScanCondition.PresetPath.Add(FileName);

                frmScanControl.Instance.UpdatePresetList(1);

                this.Close();
            }
            else
            {
                //プリセットファイルを任意に保存できるようにする by井上 2018/1/15
                ////リスト最大数のチェック
                //if (modScanCondition.PresetPath.Count >= 20)
                //{
                //    //メッセージ表示：プリセットファイルの登録数が最大値を超えています。(最大20)
                //    MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_NotSpecified, 26023), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                //for (int cnt = 0; cnt < modScanCondition.PresetName.Count; cnt++)
                //{
                //    if (modScanCondition.PresetName[cnt] == txtPresetName.Text)
                //    {
                //        //メッセージ表示：指定されたプリセットファイル名は既に存在しているため、追加できません。
                //        MessageBox.Show(CTResources.LoadResString(26027), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return;
                //    }
                //}

                //スキャン条件の設定内容をチェック       '追加 by 間々田 2009/08/22
                if (!OptValueChk1()) return;

                //ファイル保存ダイアログ表示
                string FileName = null;
                FileName = modFileIO.GetFileName(StringTable.IDS_Save, CTResources.LoadResString(StringTable.IDS_CondFile), SubExtension: "-SC");			//スキャン条件ファイル
                if (string.IsNullOrEmpty(FileName)) return;

                ////プリセット名を指定していない
                //if (string.IsNullOrEmpty(txtPresetName.Text.Trim()))
                //{
                //    //メッセージ表示：プリセット名が指定されていません。
                //    MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_NotSpecified, 26020), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                ////スライス名に禁止文字が含まれている          
                //else if (!modLibrary.FileNameProhibitionCheck(txtPresetName.Text))
                //{
                //    //メッセージ表示：
                //    //   スライス名に以下の禁止文字を使用しないでください。
                //    //   \ / : * ? < > | " Space
                //    MessageBox.Show(CTResources.LoadResString(26021) + "\r" + "\r" + "\\ / : * ? < > | " + (char)34 + " Space", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                //string FileName = Path.Combine(AppValue.PathScanCondPresetFile, txtPresetName.Text) + "-SC.csv";

                //modScansel.scanselType theScansel = default(modScansel.scanselType);
                CTstr.SCANSEL theScansel = default(CTstr.SCANSEL);

                //この画面で設定しているスキャン条件を取得
                GetControls(ref theScansel);

                //プリセットファイルを登録する
                //if (modScanCondition.SaveSCFile(FileName, theScansel))
                if (modScanCondition.SaveSCFile(FileName, theScansel, txtPresetComment.Text)) //Rev26.00 change by chouno 2017/08/31
                {
                    //メッセージ表示：プリセットファイルを保存しました。
                    MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_Saved, 26022),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            //プリセットリストに追加
            //modScanCondition.PresetName.Add(txtPresetName.Text);
            //modScanCondition.PresetPath.Add(FileName);

            //frmScanControl.Instance.UpdatePresetList(1);

            //this.Close();
		}


		//*******************************************************************************
		//機　　能： 「登録」ボタン・クリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.0  09/01/21  (SS1)間々田    リニューアル
		//*******************************************************************************
		//private void GetControls(ref modScansel.scanselType theScansel)
		private void GetControls(ref CTstr.SCANSEL theScansel)
		{
			//最新 scancel（コモン）取得
			//modScansel.GetScansel(ref theScansel);
            ScanSel scansel = new ScanSel();
            scansel.Data.Initialize();
            scansel.Load();
            theScansel = scansel.Data;
            
			//マルチスキャンモード：1(シングル), 3(マルチ), 5(スライスプラン)
			theScansel.multiscan_mode = modLibrary.GetOption(optMultiScanMode);

			//マルチスキャン
			if (fraMulti.Visible)
			{
				theScansel.pitch = (float)cwneMSPitch.Value;			    //スキャンピッチ
                //2014/11/07hata キャストの修正
                //theScansel.multinum = (int)cwneMSSlice.Value;			    //スライス数
                theScansel.multinum = Convert.ToInt32(cwneMSSlice.Value);	//スライス数
            }
			//スライスプラン
			else if (fraSlicePlanName.Visible)
			{
				if (IsCone)
				{
					//modLibrary.SetField(modLibrary.AddExtension(txtSlicePlanDir.Text, "\\"), ref theScansel.cone_sliceplan_dir);	//コーンビーム用ディレクトリ
					//modLibrary.SetField(Path.GetFileNameWithoutExtension(txtSlicePlanName.Text), ref theScansel.cone_slice_plan);	//コーンビーム用テーブル名
                    theScansel.cone_sliceplan_dir.SetString(modLibrary.AddExtension(txtSlicePlanDir.Text, "\\"));
                    theScansel.cone_slice_plan.SetString(Path.GetFileNameWithoutExtension(txtSlicePlanName.Text));
                }
				else
				{
					//modLibrary.SetField(modLibrary.AddExtension(txtSlicePlanDir.Text, "\\"), ref theScansel.sliceplan_dir);	//ディレクトリ
					//modLibrary.SetField(Path.GetFileNameWithoutExtension(txtSlicePlanName.Text), ref theScansel.slice_plan);	//テーブル名
                    theScansel.sliceplan_dir.SetString(modLibrary.AddExtension(txtSlicePlanDir.Text, "\\"));
                    theScansel.slice_plan.SetString(Path.GetFileNameWithoutExtension(txtSlicePlanName.Text));
				}
                //Rev20.00 ここでスライスプランテーブルから取得したスライス回数をコモンに入れておく by長野 2014/12/15
                string TableName = null;
                TableName = Path.Combine(txtSlicePlanDir.Text, txtSlicePlanName.Text);
                StreamReader sr = null;
                sr = new StreamReader(TableName,System.Text.Encoding.GetEncoding("shift-jis"));
                string Data = null;
                string[] strCell = null;
                char strChar = Convert.ToChar(",");
                int multi_cnt = 0;
                //ファイルから１行読み込む
                while ((Data = sr.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(Data))
                    {
                        //コンマ","で切り出し配列に格納
                        strCell = Data.Split(strChar);

                        //先頭列の文字が数字なら情報を取り出す
                        double IsNumeric = 0;
                        if (double.TryParse(strCell[0], out IsNumeric))
                        {
                            if (strCell.GetUpperBound(0) >= 3)
                            {
                                multi_cnt++;
                            }
                        }
                    }
                }
                //ファイルクローズ
                if (sr != null)
                {
                    sr.Close();
                    sr = null;
                }
                theScansel.multinum = multi_cnt;
			}

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//Ｘ線休止スキャンが可能な場合 v9.3追加 by 間々田 2004/06/30
			//        If scaninh.discharge_protect = 0 Then
			//            .discharge_protect = chkDischargeProtect.Value
			//        End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//スキャンモード：1(ハーフ), 2(フル), 3(オフセット)
			theScansel.scan_mode = modLibrary.GetOption(optScanMode);

            //Rev25.300 Wスキャン追加 by長野 2016/08/06
            theScansel.w_scan = (chkW_Scan.CheckState == CheckState.Checked? 1 : 0);

			//マトリクスサイズ：2(512x512),3(1024x1024)
			if (fraMatrix.Visible)
			{
				theScansel.matrix_size = modLibrary.GetOption(optMatrix);
			}

			//オートセンタリング：0(なし),1(あり)
			if (fraAutoCentering.Visible)
			{
				theScansel.auto_centering = (optAutoCentering1.Checked ? 1 : 0);
			}

			//ビュー数設定値
            //2014/11/07hata キャストの修正
            //theScansel.scan_view = (int)cwneViewNum.Value;
            theScansel.scan_view = Convert.ToInt32(cwneViewNum.Value);

			//画像積算枚数
			if (cwneInteg.Visible)
			{
                //2014/11/07hata キャストの修正
                //theScansel.scan_integ_number = (int)cwneInteg.Value;
                theScansel.scan_integ_number =Convert.ToInt32(cwneInteg.Value);
            }

			//スキャンエリア
			//If cwneArea.Visible Then                       'v15.02削除 by 間々田 2009/09/14

			if (IsCone)
			{
                //Rev20.00 応急処置 cwneArea.Maximumを使う by長野 2014/09/02
				//theScansel.cone_scan_area = (float)cwneArea.Value;
                theScansel.cone_scan_area = (float)cwneArea.Maximum;
                theScansel.cone_max_scan_area = (float)cwneArea.Maximum;		//最大スキャンエリア(ｺｰﾝﾋﾞｰﾑCT用)
			}
			else
			{
				theScansel.mscan_area = (float)cwneArea.Value;
			}

			//End If                                         'v15.02削除 by 間々田 2009/09/14

			//スライス厚
			//If cwneSlice.Visible Then

			if (IsCone)
			{
				theScansel.cone_scan_width = (float)cwneSlice.Value;
				theScansel.max_cone_slice_width = (float)cwneSlice.Maximum;				//最大スライス厚
				theScansel.min_cone_slice_width = (float)cwneSlice.Minimum;				//最小スライス厚
			}
			else
			{
				theScansel.mscan_width = (float)cwneSlice.Value;
				theScansel.max_slice_wid = Convert.ToSingle(cwneSlice.Maximum.ToString(string.Format("F{0}", cwneSlice.DecimalPlaces)));		//最大スライス厚
				theScansel.min_slice_wid = Convert.ToSingle(cwneSlice.Minimum.ToString(string.Format("F{0}", cwneSlice.DecimalPlaces)));		//最小スライス厚
			}

			//End If

			//バイアス
			theScansel.mscan_bias = (float)ntbBias.Value;

			//スロープ
			theScansel.mscan_slope = (float)ntbSlope.Value;

			//フィルタ関数：1(FC1),2(FC2),3(FC3)
			theScansel.filter = modLibrary.GetOption(optFilter);

			//スキャン中再構成
			theScansel.scan_and_view = modLibrary.GetOption(optScanAndView);

			//画像方向
			theScansel.image_direction = modLibrary.GetOption(optDirection);

			//マルチスライス：同時スキャン枚数 0(1ｽﾗｲｽ),1(3ｽﾗｲｽ),2(5ｽﾗｲｽ)
			if (fraMultislice.Visible)
			{
				theScansel.multislice = modLibrary.GetOption(optMultislice);
			}

			//オートズーミング：0(行わない),1(行う)
			if (fraZoomingPlan.Visible)
			{
				theScansel.auto_zoomflag = Convert.ToInt32(chkZooming.CheckState);
				//modLibrary.SetField(modLibrary.AddExtension(txtZoomFileName.Text, "\\"), ref theScansel.autozoom_dir);		//ディレクトリ名設定（末尾に\を付加）
				//modLibrary.SetField(Path.GetFileNameWithoutExtension(txtZoomFileName.Text), ref theScansel.auto_zoom);		//ファイル名の設定（拡張子は取り除く）
                //theScansel.autozoom_dir.SetString(modLibrary.AddExtension(txtZoomFileName.Text, "\\"));
                //Rev20.00 修正 by長野 2014/12/04
                theScansel.autozoom_dir.SetString(modLibrary.AddExtension(txtZoomDirName.Text, "\\"));
                theScansel.auto_zoom.SetString(Path.GetFileNameWithoutExtension(txtZoomFileName.Text));
            }

			//'生データ保存：0(行わない),1(行う)     'v15.0削除 by 間々田 2009/03/06
			//If fraOrgSave.Visible Then
			//    .rawdata_save = chkSave.value
			//End If

			//オートプリント：0(行わない),1(行う)
			if (chkPrint.Visible)
			{
				theScansel.auto_print = Convert.ToInt32(chkPrint.CheckState);
			}

			//透視画像保存：0(保存しない),1(保存する)
			if (chkFluoroImageSave.Visible)
			{
				theScansel.fluoro_image_save = Convert.ToInt32(chkFluoroImageSave.CheckState);
			}

			//アーティファクト低減           v9.7追加 by 間々田 2004/12/08
			if (chkArtifactReduction.Visible)
			{
				theScansel.artifact_reduction = Convert.ToInt32(chkArtifactReduction.CheckState);
			}

			//再構成形状：0(正方形),1(円)
			theScansel.recon_mask = modLibrary.GetOption(optReconMask);

			//画像階調最適化：0(なし),1(あり)
			if (fraContrastFitting.Visible)
			{
				theScansel.contrast_fitting = modLibrary.GetOption(optContrastFitting);
			}

			//画像回転角度：-180～180
			theScansel.image_rotate_angle = (float)cwneImageRotateAngle.Value;

            //回転中心ch調整(ch) この機能はリトライ(コーン)専用のため、念のためスキャン条件設定時は0.0とする。
            //VC側ではもscan_parの同名パラメータに0.0を入れている //追加 Rev23.00 by長野 2015/09/07
            theScansel.numOfAdjCenterCh = (float)0.0;

			//テーブル回転：0(ｽﾃｯﾌﾟ),1(連続)
			theScansel.table_rotation = modLibrary.GetOption(optTableRotation);

			//FID
			theScansel.fid = FIDWithOffset;

			//FCD
			theScansel.fcd = FCDWithOffset;

			//X線管：0(130kV),1(225kV)
			theScansel.multi_tube = modLibrary.GetOption(optMultiTube);

			//'管電圧/管電流                            '削除 by 間々田 2009/08/06 スキャン直前に変更
			//.scan_kv = frmXrayControl.cwneKV.Value    '管電圧
			//.scan_ma = frmXrayControl.cwneMA.Value    '管電流

			//ビニング：0(1x1),1(2x2),2(4x4)
			theScansel.binning = modLibrary.GetOption(optBinning);

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//コーン分散処理                         'v8.0追加 by 間々田 2003/11/21
			//        If fraConeDistribute.Visible Then
			//            .cone_distribute = GetOption(optConeDistribute)
			//        End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//回転選択                               'v9.0追加 by 間々田 2004/01/30
			if (fraRotateSelect.Visible) theScansel.rotate_select = modLibrary.GetOption(optRotateSelect);

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//往復スキャン                           'v9.0追加 by 間々田 2004/01/30
			//        If fraRoundTrip.Visible Then .round_trip = GetOption(optRoundTrip)
			//
			//        'オーバースキャン                       'v9.0追加 by 間々田 2004/01/30
			//        If fraOverScan.Visible Then .over_scan = GetOption(optOverScan)
			//
			//        'メール送信                             'v9.1追加 by 間々田 2004/05/13
			//        If fraSendMail.Visible Then .mail_send = IIf(chkSendMail.Value = vbChecked, 1, 0)
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//フィルタ処理                           'v13.0追加? byやまおか 2007/??/??
			if ((CTSettings.scaninh.Data.filter_process[0] == 0) && (CTSettings.scaninh.Data.filter_process[1] == 0))
			{
				theScansel.filter_process = modLibrary.GetOption(optFilterProcess);
			}
			else
			{
				theScansel.filter_process = (CTSettings.scaninh.Data.filter_process[1] == 0 ? 1 : 0);
			}

			//RFC                                    'v14.0追加 byやまおか 2007/07/06
			if (fraRFC.Visible) theScansel.rfc = modLibrary.GetOption(optRFC);

			//ﾏﾙﾁｺｰﾝの時の自動MPR表示の有無 v16.00 by 長野　2010/01/21
            //cwneMSPitch_dummy.Value = cwneDelta_z.Value * cwneK.Value;
			//Rev20.00 追加 四捨五入 by長野 2015/01/26
            cwneMSPitch_dummy.Value = Math.Round((cwneDelta_z.Value * cwneK.Value)/cwneMSPitch.Increment,MidpointRounding.AwayFromZero) * cwneMSPitch.Increment;

            if ((Math.Abs(cwneMSPitch.Value) == Math.Abs(cwneMSPitch_dummy.Value)) && (modLibrary.GetOption(optMultiScanMode) == 3))
			{
				theScansel.multi_dbq_disp = 1;
			}
			else
			{
				theScansel.multi_dbq_disp = 0;
			}

			//v17.02削除 frmScanControlへ移動したため byやまおか 2010/07/16
			//'PkeFPD用のゲインと積分時間をセットする 'v17.00追加(ここから) byやまおか 2010/02/17
			//If (DetType = DetTypePke) Then
			//    .fpd_gain = cmbGain.ListIndex
			//    .fpd_integ = cmbInteg.ListIndex
			//End If                                  'v17.00追加(ここまで) byやまおか 2010/02/17

			//コーンビーム選択時の場合
			if (IsCone)
			{
				//コーンビーム最大ビュー数               'v8.0 added by 間々田 2003/11/13
                //2014/11/07hata キャストの修正
                //theScansel.max_cone_view = (int)cwneViewNum.Maximum;
                theScansel.max_cone_view = Convert.ToInt32(cwneViewNum.Maximum);

				//画質：0(標準),1(精細),2(高速)
				theScansel.cone_image_mode = modLibrary.GetOption(optImageMode);

				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
				//            'ヘリカルモード：0(非ﾍﾘｶﾙ),1(ﾍﾘｶﾙ)
				//            .inh = GetOption(optHelical)
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

				//スライスピッチ(mm)=軸方向Boxelｻｲｽﾞ(mm)
				theScansel.delta_z = (float)cwneDelta_z.Value;

				//スライス枚数
                //2014/11/07hata キャストの修正
                //theScansel.k = (int)cwneK.Value;
                theScansel.k = Convert.ToInt32(cwneK.Value);

				//再構成範囲
				theScansel.zs = zs;		//再構成開始位置(mm)
				theScansel.ze = ze;		//再構成終了位置(mm)

				//ヘリカルピッチ(mm)
				theScansel.zp = (float)cwneZp.Value;

				//■その他

				//縦中心ﾁｬﾝﾈﾙ(ﾗｲﾝ数半幅)
				theScansel.mc = mc;

				//縦中心ﾁｬﾝﾈﾙの最大値(ﾗｲﾝ数半幅)   'added by 山本 2007-2-12
				//theScansel.mc_max = mc_max;
                //Rev20.00 コモンの変更 by長野 2014/08/27 
                theScansel.mc_max[0] = mc_max[0];//v18.00シフト対応 byやまおか 2011/07/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                theScansel.mc_max[1] = mc_max[1];//v18.00シフト対応 byやまおか 2011/07/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

				//昇降識別値(1:非ﾍﾘｶﾙ･ﾍﾘｶﾙ下降,-1:ﾍﾘｶﾙ上昇)
				theScansel.iud = 1;			//非ヘリカル、ヘリカル下降に固定

				//画面上のｽﾗｲｽ幅(mm)
				theScansel.delta_msw = delta_msw;

				//コーンビーム生データサイズ（KB）                                   'v7.0 append by 間々田 2003/09/09
				theScansel.cone_raw_size = Convert.ToInt32(ScanCorrect.ConeRawSize);
			}

			//v19.00 ->(電S2)永井
			//ﾋﾞｰﾑﾊﾄﾞﾆﾝｸﾞ補正:0(行わない),1(行う)
			if (CTSettings.scaninh.Data.mbhc == 0)
			{
				theScansel.mbhc_flag = Convert.ToInt32(chkBHC.CheckState);
				//theScansel.mbhc_dir = modLibrary.AddNull(txtBhcDirName.Text);											//BHCテーブルディレクトリ名
				//theScansel.mbhc_name = modLibrary.AddNull(modLibrary.RemoveExtension(txtBhcFileName.Text, ".csv"));		//BHCテーブル名
                //theScansel.mbhc_dir.SetString(modLibrary.AddNull(txtBhcDirName.Text + "\\"));
                //Rev20.00 変更 by長野 2014/12/04
                theScansel.mbhc_dir.SetString(modLibrary.AddExtension(txtBhcDirName.Text, "\\")); 
                theScansel.mbhc_name.SetString(modLibrary.AddNull(modLibrary.RemoveExtension(txtBhcFileName.Text, ".csv")));
            }
			else
			{
				//BHC無効のとき
				theScansel.mbhc_flag = 0;
				//theScansel.mbhc_dir = "";
				//theScansel.mbhc_name = "";
                theScansel.mbhc_dir.SetString("");
                theScansel.mbhc_name.SetString("");
			}
			//<- v19.00

            //Rev26.00 追加 by井上 2017/01/18
            //現在設定中のcmbBHCPhantomlessのインデックスをコモンscansel.mbhc_phantomlessにセット

            if (cmbBHCPhantomless.SelectedIndex == 0)
            {
                theScansel.mbhc_method = modBHC.BHCmethod[cmbBHCPhantomless.SelectedIndex];
                theScansel.mbhc_phantomless = cmbBHCPhantomless.SelectedIndex;
                theScansel.mbhc_phantomless_c.SetString(cmbBHCPhantomless.Items[cmbBHCPhantomless.SelectedIndex].ToString());
            }
            else if (cmbBHCPhantomless.SelectedIndex > 0)
            {
                string dirPath = @"C:\CT\ScanCorrect\phantomlessBHC";
                string[] fileList = Directory.GetFileSystemEntries(dirPath, @cmbBHCPhantomless.SelectedIndex + "_BHC_*.csv");
                string fullPath;
                fullPath = fileList[0];

                theScansel.mbhc_method = modBHC.BHCmethod[cmbBHCPhantomless.SelectedIndex];

                if (theScansel.mbhc_method == 1)//従来BHC
                {
                    theScansel.mbhc_dir.SetString("C:\\CT\\ScanCorrect\\phantomlessBHC\\");
                    theScansel.mbhc_name.SetString(cmbBHCPhantomless.SelectedIndex.ToString() + "_BHC_" + cmbBHCPhantomless.Items[cmbBHCPhantomless.SelectedIndex].ToString());
                    theScansel.mbhc_phantomless = cmbBHCPhantomless.SelectedIndex;
                    theScansel.mbhc_phantomless_c.SetString(cmbBHCPhantomless.Items[cmbBHCPhantomless.SelectedIndex].ToString());
                }
                else if (theScansel.mbhc_method == 0)//ファントムレスBHC
                {
                    theScansel.mbhc_phantomless = cmbBHCPhantomless.SelectedIndex;
                    theScansel.mbhc_phantomless_c.SetString(cmbBHCPhantomless.Items[cmbBHCPhantomless.SelectedIndex].ToString());
                }
            }
		}

        //*************************************************************************************************
		//機　　能： 設定管電圧・設定管電流　変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void myXrayControl_Changed(object sender, frmXrayControl.ChangedEventArgs e)
		{
			//東芝 EXM2-150以外は単位をmAに変換
			if (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150) e.current = e.current / 1000;

			if (e.volt == 0) return;
			if (e.current == 0) return;

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//ContTimeの計算
			//    GetDischargeProtect discharge_protect
			//    With discharge_protect
			//        myContTime = (.ct_para1 * .ct_para2 / volt / current) * Exp(.ct_para3 * volt) * 60
			//    End With
            //
            //float ContTime = 0;
            //ContTime = modLibrary.MinVal((int)myContTime, Convert.ToInt32(60) * Convert.ToInt32(60) * Convert.ToInt32(24));
            //lblContTime.Text = StringTable.GetResString(13102, Convert.ToInt32(ContTime / 60).ToString());					//%1分毎
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//最大最小ビュー数の更新
			UpdateViewMinMax();

			//最大積算枚数の更新
			UpdateIntegMax();
		}


		//*************************************************************************************************
		//機　　能： 最大最小ビュー数の更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： ビニングモード、テーブル回転モード、回転選択、Ｘ線休止有無、積算枚数、ContTime
		//           を変更したらコールする
		//
		//履　　歴： v9.3  04/07/02   (SI4)間々田   Ｘ線休止処理対応
		//*************************************************************************************************
		private void UpdateViewMinMax()
		{
			int theViewMin = 0;
			int theViewMax = 0;
			int theBinning = 0;
			float theFR = 0;

			//現在選択しているビニングモードの取得：未選択の場合は何もしない
			theBinning = modLibrary.GetOption(optBinning);
			if (theBinning == -1) return;

			//FRの取得
			//theFR = CTSettings.scancondpar.Data.frame_rate[theBinning, (IsCone ? 1 : 0)];
            int _mode = (IsCone ? 1 : 0);
            theFR = CTSettings.scancondpar.Data.frame_rate[theBinning * 2 + _mode];

			//デフォルトの最小ビュー数
            theViewMin = CTSettings.GVal_ViewMin;

			//changed by 山本 2002-9-14 連続回転による最小ビュー数の制約（最大回転数 3.3rpm)
			if (optTableRotation1.Checked)
			{
                //v19.10 PKEとその他の検出器でframerate取得方法を変える 2012/09/06 by長野
                if (CTSettings.scancondpar.Data.detector == 2)
                {
                    //StringBuilder dummy = new StringBuilder(256);
                    //double dummyValue = 0;
                    //double FrameRate = 0;
                    //if (modDeclare.GetPrivateProfileString("Timings", "Timing_" + Convert.ToString(frmScanControl.Instance.cmbInteg.SelectedIndex), "", dummy, (uint)dummy.Capacity, AppValue.XisIniFileName) > 0)
                    //{
                    //    double.TryParse(dummy.ToString(), out dummyValue);
                    //    FrameRate = 1000 / (dummyValue / 1000);
                    //}
                    //Rev20.00 変更 by長野 2015/02/06
                    double FrameRate = 0;
                    FrameRate = (double)1.0 / (modCT30K.fpd_integlist[frmScanControl.Instance.cmbInteg.SelectedIndex] / (double)1000.0);
                    theViewMin = Convert.ToInt32(Math.Floor((FrameRate * 60 / modSeqComm.GetRotMax(optRotateSelect1.Checked) - 1) / 100F + 1)) * 100;

                }
                else
                {
                    //2014/11/07hata キャストの修正
                    //theViewMin = (int)((theFR * 60 / modSeqComm.GetRotMax(optRotateSelect1.Checked) - 1) / 100 + 1) * 100;
                    theViewMin = Convert.ToInt32(Math.Floor((theFR * 60 / modSeqComm.GetRotMax(optRotateSelect1.Checked) - 1) / 100F + 1)) * 100;
                }
            }

			//デフォルトの最大ビュー数
            theViewMax = CTSettings.GVal_ViewMax;

			//added by 山本　2002-3-11　コーンビーム時は生データサイズ最大2GBの制約からビュー数を制限する
			//If IsCone Then
            if (IsCone && (CTSettings.scansel.Data.cone_raw_size > 0))
            {
                    //v19.17 分割生になってからは不要 by長野 2013/09/13
                    //float hm_a = 0;
                    //float hm_p = 0;

                    //hm_a = CTSettings.scancondpar.Data.h_mag[theBinning];						//v7.0 added by 間々田 2003/10/22
                    //hm_p = CTSettings.scancondpar.Data.h_mag[CTSettings.scansel.Data.binning];		//v7.0 added by 間々田 2003/10/22

                    ////Val_ViewMax = 1700
                    ////Val_ViewMax = MinVal(2 * CLng(1024) * CLng(1024) / theConeRawSize, ViewMax)                            'v7.0 change by 間々田 2003/09/26
                    ////theViewMax = MinVal((hm_a / hm_p) * 2 * CLng(1024) * CLng(1024) / scansel.cone_raw_size, theViewMax)    'v7.0 change by 間々田 2003/10/22
                    ////生データ分割するため最大値を16000000000B(!= 17179869184B(=16GB))とする 'v17.00変更 byやまおか 2010/02/19

                    ////Int32の最大数を超えている(16000000000)　//変更2014/06/24(検S1)hata
                    ////theViewMax = modLibrary.MinVal((int)((hm_a / hm_p) * Convert.ToInt32(16000) * Convert.ToInt32(1000000) / CTSettings.scansel.Data.cone_raw_size), theViewMax);
                    ////Rev20.00 テーブルステップ・連続回転で処理を分ける by長野 2014/9/18
                    //Int64 _viewnum = 0;
                    //if (optTableRotation0.Checked)
                    //{
                    //    _viewnum = Convert.ToInt64((hm_a / hm_p) * Convert.ToInt64(16000) * Convert.ToInt64(1000000) / CTSettings.scansel.Data.cone_raw_size);
                    //}
                    //else
                    //{
                    //    //Rev20.00 式変更 余裕を持って0.8掛けしておく by長野 2014/09/11
                    //    _viewnum = Convert.ToInt64((hm_a / hm_p) * Convert.ToInt64(CTSettings.iniValue.SharedMemSize) * Convert.ToInt64(1000000) * 0.8 / CTSettings.scansel.Data.cone_raw_size);
                    //}
                    //if (_viewnum > Int32.MaxValue) _viewnum = Int32.MaxValue;
                    //theViewMax = modLibrary.MinVal(Convert.ToInt32(_viewnum), theViewMax);

            }

			//追加 by 間々田 2004/07/02 Ｘ線休止処理対応
			//
			//Ｘ線休止スキャンが選択され、かつ連続回転スキャンモードが選択されている時、
			//最大ビュー数に制限を設ける
			if ((chkDischargeProtect.CheckState == CheckState.Checked) && 
				optTableRotation1.Checked && 
				(myContTime != null))
			{
				//０割りを防ぐ
				if (cwneInteg.Value > 0)
				{
					theViewMax = modLibrary.MinVal(Convert.ToInt32(myContTime * theFR / (float)cwneInteg.Value), theViewMax);
				}
			}

#region 【C#コントロールで代用】
/*
			.SetMinMax theViewMin, _
			           Int(theViewMax / .DiscreteInterval) * .DiscreteInterval  'データ収集ビュー数は100単位
*/
#endregion
			cwneViewNum.Minimum = theViewMin;
            //2014/11/07hata キャストの修正
            //cwneViewNum.Maximum = (int)(theViewMax / cwneViewNum.Increment) * cwneViewNum.Increment;			//データ収集ビュー数は100単位
            cwneViewNum.Maximum = Convert.ToInt32(Math.Floor(theViewMax / cwneViewNum.Increment)) * cwneViewNum.Increment;			//データ収集ビュー数は100単位
            
            lblViewMinMax.Text = StringTable.GetResString(StringTable.IDS_Range, cwneViewNum.Minimum.ToString(), cwneViewNum.Maximum.ToString());
		}


		//*************************************************************************************************
		//機　　能： 最小積算枚数の更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v16.2  2010/01/19 山影
		//*************************************************************************************************
		private void UpdateIntegMin()
		{
			int IntegMin = 0;

			//デフォルトの最大積算枚数
            IntegMin = CTSettings.GValIntegNumMin;

			//コーンビームスキャンが選択され、かつ連続回転スキャンモードが選択されている時、制限を設ける
			if (IsCone && optTableRotation1.Checked)
			{
				IntegMin = CTSettings.scancondpar.Data.smoothcone_min_integ;
			}

			//積算枚数

			//最小積算枚数
            //2014/11/07hata キャストの修正
            //cwneInteg.Minimum = modLibrary.MinVal(IntegMin, (int)cwneInteg.Maximum);
            cwneInteg.Minimum = modLibrary.MinVal(IntegMin, Convert.ToInt32(cwneInteg.Maximum));

			//積算枚数の最小・最大の表示
			lblIntegMinMax.Text = StringTable.GetResString(StringTable.IDS_Range, cwneInteg.Minimum.ToString(), cwneInteg.Maximum.ToString());
		}


		//*************************************************************************************************
		//機　　能： 最大積算枚数の更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void UpdateIntegMax()
		{
			float theFR = 0;
			int IntegMax = 0;

			//デフォルトの最大積算枚数
            IntegMax = CTSettings.GValIntegNumMax;

			//Ｘ線休止スキャンが選択され、かつ連続回転スキャンモードが選択されている時、制限を設ける
			if ((chkDischargeProtect.CheckState == CheckState.Checked) && 
				optTableRotation1.Checked && 
				(myContTime != null))
			{
				//frame_rateの取得
				//theFR = CTSettings.scancondpar.Data.frame_rate[modLibrary.GetOption(optBinning), (IsCone ? 1 : 0)];
                int _mode = (IsCone ? 1 : 0);
                int _binnning = modLibrary.GetOption(optBinning);
                theFR = CTSettings.scancondpar.Data.frame_rate[_binnning * 2 + _mode];

				if (cwneViewNum.Value > 0)
				{
					IntegMax = modLibrary.MinVal(Convert.ToInt32(myContTime * theFR / (float)cwneViewNum.Value), IntegMax);
				}
			}

			//積算枚数

			//最大積算枚数
            //2014/11/07hata キャストの修正
            //cwneInteg.Maximum = modLibrary.MaxVal(IntegMax, (int)cwneInteg.Minimum);
            cwneInteg.Maximum = modLibrary.MaxVal(IntegMax, Convert.ToInt32(cwneInteg.Minimum));

			//積算枚数の最小・最大の表示
			lblIntegMinMax.Text = StringTable.GetResString(StringTable.IDS_Range, cwneInteg.Minimum.ToString(), cwneInteg.Maximum.ToString());
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
		private void sstMenu_SelectedIndexChanged(object sender, EventArgs e)
		{
			//表示していないタブ内のコントロールにフォーカスさせないようにするための措置
			int i = 0;
			for (i = fraMenu.GetLowerBound(0); i <= fraMenu.GetUpperBound(0); i++)
			{
				fraMenu[i].Enabled = (i == sstMenu.SelectedIndex);

                //追加2014/09/11_dNet対応_hata
                //選択されていないTabのVisibleがFalse になるための処置
                if (sstMenu.SelectedIndex == i)
                {
                    fraMenu[i].Parent = sstMenu.TabPages[i];
                    fraMenu[i].Location = new Point(5, 2);
                }
                else
                {
                    fraMenu[i].Parent = this;
                    fraMenu[i].Top = sstMenu.Top + sstMenu.ItemSize.Height;
                    fraMenu[i].Left = sstMenu.Left;
                }
            }
            Application.DoEvents();

        }

		//*************************************************************************************************
		//機　　能： マルチスキャン・スキャンピッチキー入力時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void cwneMSPitch_KeyPress(object sender, KeyPressEventArgs e)
		{
			//ＯＫボタンを使用可にする
		    cmdOK.Enabled = true;
        }


		//*************************************************************************************************
		//機　　能： マルチスキャン・スライス数キー入力時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void cwneMSSlice_KeyPress(object sender, KeyPressEventArgs e)
		{
			//ＯＫボタンを使用可にする
			cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： スライスピッチキー入力時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void cwneDelta_z_KeyPress(object sender, KeyPressEventArgs e)
		{
			//ＯＫボタンを使用可にする
			cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： スライス枚数キー入力時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void cwneK_KeyPress(object sender, KeyPressEventArgs e)
		{
			//ＯＫボタンを使用可にする
			cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： スライスエリアキー入力時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void cwneArea_KeyPress(object sender, KeyPressEventArgs e)
		{
			//ＯＫボタンを使用可にする
			cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： スライス厚（mm）キー入力時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void cwneSlice_KeyPress(object sender, KeyPressEventArgs e)
		{
			//ＯＫボタンを使用可にする
			cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： スライス厚（画素数）キー入力時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void cwneSlicePix_KeyPress(object sender, KeyPressEventArgs e)
		{
			//ＯＫボタンを使用可にする
			cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： ビュー数キー入力時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void cwneViewNum_KeyPress(object sender, KeyPressEventArgs e)
		{
			//変更した場合，ＯＫボタンを使用可にする
			cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： 積算枚数キー入力時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void cwneInteg_KeyPress(object sender, KeyPressEventArgs e)
		{
			//ＯＫボタンを使用可にする
			cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： 積算枚数キー入力時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v19.10  2012/09/08 (検S1)長野  新規作成
		//*************************************************************************************************
		//Private Sub cwneInteg_IncDecButtonClicked(ByVal IncButton As Boolean)
		//'
		//'    'v19.01 連続の場合 ビュー数リスト更新 by長野 2012/05/21
		//'
		//'    If scaninh.smooth_rot_cone = 0 Then
		//'
		//'        ChangeViewList
		//'
		//'    End If
		//'
		//'    'ＯＫボタンを使用可にする
		//'    cmdOK.Enabled = True
		//
		//End Sub

		//*************************************************************************************************
		//機　　能： 画像回転角度キー入力時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void cwneImageRotateAngle_KeyPress(object sender, KeyPressEventArgs e)
		{
			//ＯＫボタンを使用可にする
			cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： ヘリカルピッチキー入力時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void cwneZp_KeyPress(object sender, KeyPressEventArgs e)
		{
			//ＯＫボタンを使用可にする
			cmdOK.Enabled = true;
		}


		//*************************************************************************************************
		//機　　能： 英語用レイアウト調整
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V17.60  11/05/25  (検S1)長野      新規作成
		//*************************************************************************************************
		private void EnglishAdjustLayout()
		{
			//int num = 0;
			//int i = 0;

			//条件設定１
			//*************************************************************************
			//透視画像保存フレーム
            //2014/11/07hata キャストの修正
            //fraFluoroImageSave.Width = (int)(fraFluoroImageSave.Width * 1.1);
            //fraFluoroImageSave.Width = Convert.ToInt32(fraFluoroImageSave.Width * 1.10);
            //Rev20.01 変更 by長野 2015/05/19
            fraFluoroImageSave.Width = Convert.ToInt32(fraFluoroImageSave.Width * 1.15);

			//オートプリンタフレーム
            //2014/11/07hata キャストの修正
            //fraAutoPrint.Width = (int)(fraAutoPrint.Width * 1.1);
            //fraAutoPrint.Width = Convert.ToInt32(fraAutoPrint.Width * 1.10);
            //Rev20.01 変更 by長野 2015/05/19
            fraAutoPrint.Width = Convert.ToInt32(fraAutoPrint.Width * 1.15);

			//マトリクスサイズフレーム
			fraMatrix.Left = fraMatrix.Left + 15;
            //2014/11/07hata キャストの修正
            //fraMatrix.Width = (int)(fraMatrix.Width * 0.9);
            fraMatrix.Width = Convert.ToInt32(fraMatrix.Width * 0.9);

			//条件設定２
			//*************************************************************************
			//画像階調最適化フレーム
			fraContrastFitting.Font = new Font(fraContrastFitting.Font.Name, 9);
		}


		//*************************************************************************************************
		//機　　能： スキャンスタート時、回転中心位置が左右に寄っていてもスキャンを続行するかどうか
		//           判定するプロパティ

		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V17.65  11/11/26  (検S1)長野      新規作成
		//*************************************************************************************************
		public bool ScanOptValueChk2ok
		{
			get { return myScanOptValueChk2ok; }
			set { myScanOptValueChk2ok = value;}
		}


		//v19.00 追加 ->
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
			//FileName = modFileIO.GetFileName(Description: CTResources.LoadResString(StringTable.IDS_BHCTable), SubExtension: "-bhc", InitFileName: txtBhcDirName.Text + txtBhcFileName.Text);
            //Rev20.00 修正 by長野 2015/03/23
            FileName = modFileIO.GetFileName(Description: CTResources.LoadResString(StringTable.IDS_BHCTable), SubExtension: "-bhc", InitFileName: txtBhcDirName.Text + "\\" + txtBhcFileName.Text);

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
		//履　　歴： V19.90   2012/03/25 長野    新規作成
		//*******************************************************************************
		private void cmdChangeBHCTableDefault_Click(object sender, EventArgs e)
		{
			//v8.2追加ここから by 間々田 2007/06/11 上記の方法だと存在しないＢＨＣテーブルを指定できてしまうので
			string FileName = null;
			string PathName = null;
			string TableName = null;

			//デフォルトのファイル名は、"C:\CTUSER\BHCﾃｰﾌﾞﾙ\BHC-DEFAULT_TABLE.csv"とする
			//v19.12 英語化対応 by長野 2013/02/20
			//FileName = AppValue.InitDir_BHCTable + "\\DEFAULT_TABLE-BHC.csv";
            //Rev20.00 変更 by長野 2015/02/11
            FileName = Path.Combine(AppValue.CTUSER,CTResources.LoadResString(21001)) + "\\DEFAULT_TABLE-BHC.csv";
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
			else				//v8.0追加 by Ohakdo 2007/01/24
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
			if (CTSettings.scaninh.Data.mbhc == 0)
			{
				if (chkBHC.CheckState == CheckState.Checked && fraBHC.Visible)
				{
					//ファイル名に".csv"がつくためピリオド判定なしバージョンを呼び出す
					if (!modLibrary.IsValidFileName2(txtBhcDirName.Text, txtBhcFileName.Text))
					{
						modCT30K.ErrMessage(1217, Icon: MessageBoxIcon.Error);			//リソース　BHCﾃｰﾌﾞﾙが指定されたﾃﾞｨﾚｸﾄﾘにありません。
						return functionReturnValue;
					}
				}
			}

            //Rev26.00 ファントムレスBHC追加 by井上 2018/01/18
            if (CTSettings.scaninh.Data.mbhc_phantomless == 0 && cmbBHCPhantomless.SelectedIndex > 0)
            {
                //Rev26.00 change by chouno 2017/04/17
                if(modScanCondition.ChkXraySpectrumDataExists(Convert.ToInt32(CTSettings.t20kinf.Data.system_type.GetString())) != 0)
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
                    modCT30K.ErrMessage(1218, Icon: MessageBoxIcon.Error);			//リソース　材質データがが指定されたﾃﾞｨﾚｸﾄﾘにありません。
                    return functionReturnValue;
                }
            }

			functionReturnValue = true;
			return functionReturnValue;
		}
        //********************************************************************************
        //機    能  ：  ファントムレスBHC用コンボボックス インデックス変更時処理
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履　　歴： V26.00  2017/01/18  井上            新規作成
        //********************************************************************************
        private void cmbBHCPhantomless_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ＯＫボタンを使用可にする
            cmdOK.Enabled = true;

        }
		//<- v19.00
        //追加2014/10/07hata_v19.51反映
        //********************************************************************************
        //機    能  ：  スライスプラン選択時のプリセット設定
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履　　歴： V19.20  2012/11/01   Inaba      新規作成
        //********************************************************************************
        private bool PresetSlicePlan()
        {
            bool functionReturnValue = false;

            string TableName = null;        //スライスプランのテーブル名
            string ImageName = null;        //スライスプランの画像ファイル名
            string Data = null;             //スライスプランテーブルから読み込んだレコード
            //short fileNo = 0;               //ファイル番号
            string[] strCell = null;
            
            //返り値の初期化
            functionReturnValue = false;

            StreamReader sr = null;
            char strChar = Convert.ToChar(",");
 
            //エラー時の設定
            // ERROR: Not supported in C#: OnErrorStatement
            try
            {
                //スライスプランテーブルのファイル名
                //TableName = modFileIO.FSO.BuildPath(txtSlicePlanDir.Text, txtSlicePlanName.Text);
                TableName = Path.Combine(txtSlicePlanDir.Text, txtSlicePlanName.Text);

                //ファイルオープン
                //sr = new StreamReader(TableName);
                //Rev20.00 shift-jisでエンコード by長野 2014/12/15
                sr = new StreamReader(TableName, System.Text.Encoding.GetEncoding("shift-jis"));
            
                //ファイルから１行読み込む
                while ((Data = sr.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(Data))
                    {
                        //コンマ","で切り出し配列に格納
                        strCell = Data.Split(strChar);

                        //先頭列の文字が数字なら情報を取り出す
                        double IsNumeric = 0;
                        if (double.TryParse(strCell[0], out IsNumeric))
                        {
                            if (strCell.GetUpperBound(0) >= 3)
                            {
                                ImageName = strCell[2] + strCell[3];
                                break; 
                            }
                        }
                    }
                }
                //ファイルクローズ
                if (sr != null)
                {
                    sr.Close();
                    sr = null;
                }

                //スライスプランのスキャン条件を設定
                if (!LoadSCSlicePlan(ImageName))
                {
                    //失敗した場合メッセージを表示：
                    //   指定されたスライスプランテーブルからスキャン条件を設定できませんでした。
                    //Interaction.MsgBox(StringTable.BuildResStr(9951, StringTable.IDS_SlicePlanTable), MsgBoxStyle.Critical);
                    MessageBox.Show(StringTable.BuildResStr(9951, StringTable.IDS_SlicePlanTable), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return functionReturnValue;
                }

                //スキャン条件の内容を画面に反映
                frmScanControl.Instance.LoadScanCondition();

                functionReturnValue = true;

            }
            catch(Exception e)
            {
                if (sr != null)
                {
                    sr.Close();
                    sr = null;
                }

                //エラーメッセージ表示
                //Interaction.MsgBox(Err().Description, MsgBoxStyle.Critical);
                MessageBox.Show(e.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
 
            return functionReturnValue;

        }

        //追加2014/10/07hata_v19.51反映
        //*******************************************************************************
        //機　　能： スライスプランテーブルからスキャン条件を設定する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V19.20  2012/11/01   Inaba      新規作成
        //*******************************************************************************
        private bool LoadSCSlicePlan(string FileName)
		{
			bool functionReturnValue = false;

			//ImageInfoStruct theInfoRec = default(ImageInfoStruct);
            CTAPI.CTstr.IMAGEINFO theInfoRec = default(CTAPI.CTstr.IMAGEINFO);

			//戻り値初期化
			functionReturnValue = false;


			//ファイル名チェック
			if (!Regex.IsMatch(FileName.ToLower(), "^.+[.]img$")) 
                return functionReturnValue;

			//付帯情報を取得
			if (!ImageInfo.ReadImageInfo(ref theInfoRec, modLibrary.RemoveExtension(FileName, ".img")))
				return functionReturnValue;

            //Rev23.40/Rev23.21 移動先の機構部位置が干渉エリア内＆昇降制限を超える場合は中止 by長野 2016/03/10
            float TargetUdPos = 0.0f;
            float TargetFCD = 0.0f;
            float.TryParse(theInfoRec.d_tablepos.GetString(), out TargetUdPos);
            TargetFCD = theInfoRec.fcd - theInfoRec.fcd_offset;
            if (!modMechaControl.chkTablePosByAutoPos(TargetUdPos, TargetFCD))
            {
                //メッセージの表示：移動後のテーブル昇降位置が、干渉エリア内での制限位置を越えるため、処理を中止します。
                //MsgBox LoadResString(IDS_CorReadyAlready), vbCritical
                MessageBox.Show(CTResources.LoadResString(24100), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                return functionReturnValue;
            }

			//マウスポインタを砂時計にする
			System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

			//最新 scancel（コモン）取得
			//modScansel.GetScansel(ref modScansel.scansel);
            CTSettings.scansel.Load();

			//スキャン条件のセット
			int ii = 0;
			var _with33 = theInfoRec;

			//管電圧
			float volt = 0;
			float.TryParse(theInfoRec.volt.GetString(), out volt);
			modXrayControl.SetVolt(volt);
            
            //kV/uAを連続設定すると無視される対策
			modCT30K.PauseForDoEvents(1);
			
            //管電流
			float anpere = 0;
			float.TryParse(theInfoRec.anpere.GetString(), out anpere);
			modXrayControl.SetCurrent(anpere);

            modCT30K.PauseForDoEvents(1);

            //Rev23.40 追加 by長野 2016/06/19 
            //X線制御コントロール更新
            frmXrayControl.Instance.UpdateGeCwneKVMA(volt, anpere);

            //Rev23.40 追加 by長野 2016/06/19 
            //焦点
            int Index = theInfoRec.xfocus;
            frmXrayControl.Instance.cmdFocus_ClickEx();

			//PkeFPDの場合
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
			{
                //FPDゲイン/FPD積分時間
                frmScanControl.Instance.SetFpdGainInteg(theInfoRec.fpd_gain, theInfoRec.fpd_integ);
                if (CTSettings.t20kinf.Data.pki_fpd_type == 0) //Rev25.03/Rev25.02 add by chouno 2017/02/14
                {
                    frmScanControl.Instance.cmbGain.SelectedIndex = theInfoRec.fpd_gain - 1;
                }
                else
                {
                    frmScanControl.Instance.cmbGain.SelectedIndex = theInfoRec.fpd_gain;
                }
                frmScanControl.Instance.cmbInteg.SelectedIndex = theInfoRec.fpd_integ;
			}

			//マトリクスサイズ
			//modScansel.scansel.matrix_size = modLibrary.GetIndexByStr(modLibrary.RemoveNull(_with33.matsiz), ref ref modCommon.MyCtinfdef.matsiz, 2);
            CTSettings.scansel.Data.matrix_size = modCommon.MyCtinfdef.matsiz.GetIndexByStr(modLibrary.RemoveNull(theInfoRec.matsiz.GetString()), 2);
            
            //Rev20.00 テーブル回転追加 by長野 2015/01/29
            if (_with33.table_rotation == 0)
            {
                CTSettings.scansel.Data.table_rotation = 0;
            }
            else
            {
                CTSettings.scansel.Data.table_rotation = 1;
            }

            //Rev23.40 スキャンモードも復元させる by長野 2016/06/19
            CTSettings.scansel.Data.scan_mode = modCommon.MyCtinfdef.full_mode.GetIndexByStr(modLibrary.RemoveNull(theInfoRec.full_mode.GetString()), 0) + 1;
            
			//ビュー数
			//modScansel.scansel.scan_view = Conversion.Val(_with33.scan_view);
			int.TryParse(theInfoRec.scan_view.GetString(), out CTSettings.scansel.Data.scan_view);


			//画像積算枚数
			//modScansel.scansel.scan_integ_number = Conversion.Val(_with33.integ_number);
            int.TryParse(theInfoRec.integ_number.GetString(), out CTSettings.scansel.Data.scan_integ_number);

            //Rev20.00 ここは不要 by長野 2014/12/15
            ////スキャンエリア
            //if (IsCone) {
            //    CTSettings.scansel.Data.cone_scan_area = _with33.mscan_area;
            //} else {
            //    CTSettings.scansel.Data.mscan_area = _with33.mscan_area;
            //}

			//スライス厚
			//CTSettings.scansel.Data.mscan_width = Conversion.Val(_with33.width);
            float.TryParse(theInfoRec.width.GetString(), out CTSettings.scansel.Data.mscan_width);

            //再構成画像回転角度 Rev23.40 追加 by長野 2016/06/19
            CTSettings.scansel.Data.image_rotate_angle = theInfoRec.recon_start_angle;

			//コーンビームフラグ=1の場合
			if (IsCone) {

				//スライス厚(mm)
				//CTSettings.scansel.Data.cone_scan_width = Convert.ToSingle(_with33.width);
                //Rev20.00 修正 by長野 2014/12/15
                CTSettings.scansel.Data.cone_scan_width = Convert.ToSingle(_with33.width.GetString());

				//スライスピッチ(mm)=軸方向Boxelｻｲｽﾞ(mm)
				CTSettings.scansel.Data.delta_z = _with33.delta_z;

				//スライス枚数
				CTSettings.scansel.Data.k = _with33.k;

				//再構成開始位置(mm)
				CTSettings.scansel.Data.zs = _with33.zs0;

				//再構成終了位置(mm)
				CTSettings.scansel.Data.ze = _with33.ze0;

				//画面上のｽﾗｲｽ幅(mm)
				CTSettings.scansel.Data.delta_msw = CTSettings.scansel.Data.cone_scan_width
                                            * (_with33.fid / _with33.fcd) 
                                            * (_with33.b1 / 10) 
                                            * (float)(Math.Sqrt(1 + _with33.scan_posi_a * _with33.scan_posi_a) / (_with33.kv == 0 ? 1 : _with33.kv));

			}

			//I.I.視野を設定
			//ii = modLibrary.GetIndexByStr(modLibrary.RemoveNull(_with33.iifield), modCommon.MyCtinfdef.iifield);
            ii = modCommon.MyCtinfdef.iifield.GetIndexByStr(modLibrary.RemoveNull(theInfoRec.iifield.GetString()),0);
            //modSeqComm.SeqBitWrite(new string[] { "II9", "II6", "II4" }[ii + 1], true);
            //Rev20.01 修正 by長野 2015/06/02
            modSeqComm.SeqBitWrite(new string[]{ "II9", "II6", "II4" }[ii], true);
 
			//マウスポインタを元に戻す
			System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

			//テーブル移動			
            float d_tablepos = 0;
			float.TryParse(theInfoRec.d_tablepos.GetString(), out d_tablepos);
			frmMechaMove.Instance.MechaMove((decimal)_with33.ftable_y_pos, 
                                            (decimal)_with33.ftable_x_pos, 
                                            _with33.fcd - _with33.fcd_offset, 
                                            _with33.fid - _with33.fid_offset, 
                                            (decimal)_with33.table_x_pos, 
                                            (decimal)(IsCone ? _with33.z0 : d_tablepos));


			//scancel（コモン）更新
			//modScansel.PutScansel(ref modScansel.scansel);
            CTSettings.scansel.Write();

            
            var _with34 = CTSettings.scansel.Data;

			//各コントロールに値をセットする

			//マトリクスサイズ：2(512x512), 3(1024x1024), 4(2048x2048)
			modLibrary.SetOption(optMatrix, _with34.matrix_size, (int)ScanSel.MatrixSizeConstants.MatrixSize512);

            //Rev20.00 テーブル回転追加 by長野 2015/01/29
            if (_with34.table_rotation == 0)
            {
                optTableRotation[0].Checked = true;
                optTableRotation[1].Checked = false;
            }
            else
            {
                optTableRotation[0].Checked = false;
                optTableRotation[1].Checked = true;
            }

			//ビュー数設定値
			cwneViewNum.Value = _with34.scan_view;

			//画像積算枚数
			cwneInteg.Value = _with34.scan_integ_number;
            
            //スライス枚数                               'コーンビーム用
			//cwneK.Value = _with34.k;
            //Rev20.00 スライス枚数が範囲外になる場合は、先にスライス厚とピッチを変更する。by長野 2014/12/15
            if (cwneK.Maximum < _with34.k || cwneK.Minimum > _with34.k)
            {
                //Rev20.00 ここは不要 by長野 2014/12/15
                //スキャンエリア
                //cwneArea.Value = (decimal)(IsCone ? _with34.cone_scan_area : _with34.mscan_area);

                if (IsCone)
                {				//最大スライス厚の計算
                    //最大スライス厚の計算
                    //SWMaxChange((2 / 3) * SWMaxPara);
                    //Rev20.00 小数点での計算が必要なため変更 by長野 2014/11/04
                    SWMaxChange((2.0f / 3.0f) * (float)SWMaxPara);
                }

                //スライス厚(mm)
                if (IsCone)
                {
                    cwneSlice.Value = (decimal)_with34.cone_scan_width;
                }
                else
                {
                    cwneSlice.Value = (decimal)_with34.mscan_width;
                }

                //スライスピッチ(mm)
                cwneDelta_z.Value = (decimal)_with34.delta_z;

                cwneK.Value = _with34.k;
            }
            else
            {
                cwneK.Value = _with34.k;

                //Rev20.00 ここは不要 by長野 2014/12/15
                //スキャンエリア
                //cwneArea.Value = (decimal)(IsCone ? _with34.cone_scan_area : _with34.mscan_area);

                if (IsCone)
                {				//最大スライス厚の計算
                    //最大スライス厚の計算
                    //SWMaxChange((2 / 3) * SWMaxPara);
                    //Rev20.00 小数点での計算が必要なため変更 by長野 2014/11/04
                    SWMaxChange((2.0f / 3.0f) * (float)SWMaxPara);
                }

                //スライス厚(mm)
                if (IsCone)
                {
                    cwneSlice.Value = (decimal)_with34.cone_scan_width;
                }
                else
                {
                    cwneSlice.Value = (decimal)_with34.mscan_width;
                }

                //スライスピッチ(mm)
                cwneDelta_z.Value = (decimal)_with34.delta_z;
            }

            //戻り値セット
			functionReturnValue = true;

            //マウスポインタを元に戻す
			System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
			return functionReturnValue;

		}

		//********************************************************************************
		//機    能  ：  連続の場合のビュー数をコンボボックスのリストに入れる
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V19.10   12/07/30    長野     新規作成
		//********************************************************************************
		private bool CreateViewListComboBox()
		{
			bool functionReturnValue = false;

			int Cnti = 0;					//カウンタ
			int Cntj = 0;					//カウンタ
            //int Cntk = 0;					//カウンタ  //追加2014/10/07hata_v19.51反映
			int CntView = 0;				//回転可能な速度になるビュー数の個数
			int TablePPS = 0;				//フレームレートから計算したPPS
			double FrameRate = 0;			//フレームレート
			double TableFrameRate = 0;		//TablePPSから計算したフレームレート
			double DiffHalfFrame = 0;		//0.5フレームずれるときのキャプチャ回数
			double DiffMin = 0;				//キャプチャボードのフレームレートと回転速度から計算したフレームレートの差の最小値
			int SearchMax = 0;				//サーチ範囲最大
			int SearchMin = 0;				//サーチ範囲最小
			//float Rpm = 0;					//回転速度
			//float RpmMin = 0;				//回転速度最小値
			//RpmMin = 0.01F;
			int AdjustViewNum = 0;			//連続用に補正したビュー数

			cmbViewNum.Items.Clear();
            
            //Rev20.00 条件式変更 by長野 2015/03/06
            //if ((CTSettings.GVal_ViewMax == 6000))
            if ((CTSettings.GVal_ViewMax >= 6000))
            {
				//modCT30K.OrgViewListBox = new int[21];
                //Rev20.00 変更 by長野 2015/02/06
                modCT30K.OrgViewListBox = new int[23];

				//リスト化したいビュー数
                //modCT30K.OrgViewListBox[0] = 600;
                //modCT30K.OrgViewListBox[1] = 1200;
                //modCT30K.OrgViewListBox[2] = 1800;
                //modCT30K.OrgViewListBox[3] = 2400;
                //modCT30K.OrgViewListBox[4] = 3000;
                //modCT30K.OrgViewListBox[5] = 3600;
                //modCT30K.OrgViewListBox[6] = 4800;
                //modCT30K.OrgViewListBox[7] = 5400;
                //modCT30K.OrgViewListBox[8] = 6000;

                //Rev20.00 変更 by長野 2015/02/06
                modCT30K.OrgViewListBox[0] = 100;
                modCT30K.OrgViewListBox[1] = 150;
                modCT30K.OrgViewListBox[2] = 300;
                modCT30K.OrgViewListBox[3] = 600;
                modCT30K.OrgViewListBox[4] = 900;
                modCT30K.OrgViewListBox[5] = 1200;
                modCT30K.OrgViewListBox[6] = 1500;
                modCT30K.OrgViewListBox[7] = 1800;
                modCT30K.OrgViewListBox[8] = 2100;
                modCT30K.OrgViewListBox[9] = 2400;
                modCT30K.OrgViewListBox[10] = 2700;
                modCT30K.OrgViewListBox[11] = 3000;
                modCT30K.OrgViewListBox[12] = 3300;
                modCT30K.OrgViewListBox[13] = 3600;
                modCT30K.OrgViewListBox[14] = 3900;
                modCT30K.OrgViewListBox[15] = 4200;
                modCT30K.OrgViewListBox[16] = 4500;
                modCT30K.OrgViewListBox[17] = 4800;
                modCT30K.OrgViewListBox[18] = 5100;
                modCT30K.OrgViewListBox[19] = 5400;
                modCT30K.OrgViewListBox[20] = 5700;
                modCT30K.OrgViewListBox[21] = 6000;

            }
			else
			{
				modCT30K.OrgViewListBox = new int[17];

				//リスト化したいビュー数
                //modCT30K.OrgViewListBox[0] = 600;
                //modCT30K.OrgViewListBox[1] = 1200;
                //modCT30K.OrgViewListBox[2] = 1800;
                //modCT30K.OrgViewListBox[3] = 2400;
                //modCT30K.OrgViewListBox[4] = 3000;
                //modCT30K.OrgViewListBox[5] = 3600;
                //modCT30K.OrgViewListBox[6] = 4800;

                //Rev20.00 変更 by長野 2015/02/06
                modCT30K.OrgViewListBox[0] = 100;
                modCT30K.OrgViewListBox[1] = 150;
                modCT30K.OrgViewListBox[2] = 300;
                modCT30K.OrgViewListBox[3] = 600;
                modCT30K.OrgViewListBox[4] = 900;
                modCT30K.OrgViewListBox[5] = 1200;
                modCT30K.OrgViewListBox[6] = 1500;
                modCT30K.OrgViewListBox[7] = 1800;
                modCT30K.OrgViewListBox[8] = 2100;
                modCT30K.OrgViewListBox[9] = 2400;
                modCT30K.OrgViewListBox[10] = 2700;
                modCT30K.OrgViewListBox[11] = 3000;
                modCT30K.OrgViewListBox[12] = 3300;
                modCT30K.OrgViewListBox[13] = 3600;
                modCT30K.OrgViewListBox[14] = 3900;
                modCT30K.OrgViewListBox[15] = 4200;
                modCT30K.OrgViewListBox[16] = 4500;
                modCT30K.OrgViewListBox[17] = 4800;
            }

			//Xis.iniから正確な積分時間を取得する
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				Dim dummy As String * 256
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//v19.10 PKEとその他の検出器でframerate取得方法を変える 2012/09/06 by長野
			if (CTSettings.scancondpar.Data.detector == 2)
			{
                //Rev20.00 変更 by長野 2015/02/06
                FrameRate = (double)1.0 / (modCT30K.fpd_integlist[frmScanControl.Instance.cmbInteg.SelectedIndex] / (double)1000.0);
                //if (modDeclare.GetPrivateProfileString("Timings", "Timing_" + Convert.ToString(frmScanControl.Instance.cmbInteg.SelectedIndex), "", dummy, (uint)dummy.Capacity, AppValue.XisIniFileName) > 0)
                //{
                //    double dummyValue = 0;
                //    double.TryParse(dummy.ToString(), out dummyValue);
                //    FrameRate = 1000 / (dummyValue / 1000);
                //}
			}
			else
			{
         		FrameRate = frmTransImage.Instance.GetCurrentFR();
			}

			//v19.10 tablePPSからの逆算では、最も整数値に近いビュー数は出せないのでfor文で±50ビューの範囲で探索する。 by長野 2012/07/31
			for (Cnti = 0; Cnti <= modCT30K.OrgViewListBox.GetUpperBound(0) - 1; Cnti++)
			{
                //変更2014/10/07hata_v19.51反映
                //if (modCT30K.OrgViewListBox[Cnti] == 6000)
                if (Cnti == (modCT30K.OrgViewListBox.GetUpperBound(0) - 1))
                {
					SearchMax = 0;
                    //変更2014/10/07hata_v19.51反映
                    //v19.16 探索範囲を広げる by長野 2013/08/03
                    //SearchMin = -100;
                    //Rev20.00 test by長野 2014/12/15
                    //SearchMin = -300;
                    SearchMin = -150;
                }
				else if (modCT30K.OrgViewListBox[Cnti] == 100)
				{
                    //変更2014/10/07hata_v19.51反映
                    //v19.16 探索範囲を広げる by長野 2013/08/03
                    //SearchMax = 100;
                    //Rev20.00 test by長野 2014/12/15
                    //SearchMax = 300;
                    //SearchMin = 0;
                    SearchMax = 49;
                    SearchMin = 0;
				}
                else if (modCT30K.OrgViewListBox[Cnti] == 150)
                {
                    //変更2014/10/07hata_v19.51反映
                    //v19.16 探索範囲を広げる by長野 2013/08/03
                    //SearchMax = 100;
                    //Rev20.00 test by長野 2014/12/15
                    //SearchMax = 300;
                    //SearchMin = 0;
                    SearchMax = 149;
                    SearchMin = 0;
                }
                else if (modCT30K.OrgViewListBox[Cnti] == 300)
                {
                    //変更2014/10/07hata_v19.51反映
                    //v19.16 探索範囲を広げる by長野 2013/08/03
                    //SearchMax = 100;
                    //Rev20.00 test by長野 2014/12/15
                    //SearchMax = 300;
                    //SearchMin = 0;
                    SearchMax = 149;
                    SearchMin = 0;
                }
				else
				{
                    //変更2014/10/07hata_v19.51反映
                    //v19.16 探索範囲を広げる(低い方よりに広げる) by長野 2013/08/03
                    //SearchMax = 50;
					//SearchMin = -50;
                    //重ならないように-1
                    //Rev20.00 test by長野 2014/12/15
                    //SearchMax = 299;
                    //SearchMin = -299;
                    SearchMax = 149;
                    SearchMin = -149;
				}

				//DiffMin初期化

				DiffMin = 0;
				CntView = 0;

				for (Cntj = SearchMin; Cntj <= SearchMax; Cntj++)
				{
					//orgViewListBoxからテーブル回転のPPSを出しておく
                    //2014/11/07hata キャストの修正
                    //TablePPS = (int)((FrameRate / ((modCT30K.OrgViewListBox[Cnti] + Cntj) * (double)cwneInteg.Value)) * (360.0 / 0.001));
                    TablePPS = Convert.ToInt32(Math.Floor((FrameRate / ((modCT30K.OrgViewListBox[Cnti] + Cntj) * (double)cwneInteg.Value)) * (360.0 / 0.001)));

					//TablePPSからテーブル回転（フレームレート単位）を計算
					TableFrameRate = TablePPS * (modCT30K.OrgViewListBox[Cnti] + Cntj) * (double)cwneInteg.Value / (360.0 / 0.001);

					//何回キャプチャしたらキャプチャ側に対し，0.5フレーム遅れるかを計算
					DiffHalfFrame = 0.5 / (FrameRate - TableFrameRate) * FrameRate;

					if (DiffMin < Math.Abs(DiffHalfFrame))
					{
						DiffMin = DiffHalfFrame;
						AdjustViewNum = modCT30K.OrgViewListBox[Cnti] + Cntj;
					}
				}

				//v19.01 探索したビュー数だと最低速より低い速度になる場合はリストから除外 2013-1-11 by長野
                //2014/11/07hata キャストの修正
                //if ((60.0 * FrameRate / AdjustViewNum / (double)cwneInteg.Value) < 0.01)
                if ((60.0 * FrameRate / (double)AdjustViewNum / (double)cwneInteg.Value) < 0.01)
				{
					//cmbViewNum.List(Cnti) = ""
				}
                //DiffMinがビュー数×積分枚数未満の場合はリストから除外 by長野 2014/12/15
                else if (DiffMin < AdjustViewNum * (int)cwneInteg.Value)
                {
                    //Debug.Print("no list!!");
                }
                //最高速より速くなる場合はリストから除外 by長野 2015/02/06
                else if (AdjustViewNum < cwneViewNum.Minimum)
                {
                    //
                }
                else
                {
                    cmbViewNum.Items.Add(AdjustViewNum);


                    //cmbViewNum.Items[Cnti] = AdjustViewNum;
                    //Debug.Print("Cnti= " + Cnti.ToString());

                    CntView = CntView + 1;
                }

				//       cmbViewNum.List(Cnti) = AdjustViewNum
			}

			//
			functionReturnValue = true;
			return functionReturnValue;
		}


		//********************************************************************************
		//機    能  ：  連続の場合のコンボボックスのリストのインデックスを決める
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V19.10   12/07/30    長野     新規作成
		//********************************************************************************
		private int SetViewListIndex()
		{
			int Cnti = 0;
			int DiffViewNum = 0;
			int TempDiffViewNum = 0;
			//int SetIndex = 0;

			//初期化
			DiffViewNum = 9999;
			TempDiffViewNum = 0;
			int functionReturnValue = -1;

            //変更2014/10/07hata_v19.51反映
            //for (Cnti = 0; Cnti <= modCT30K.OrgViewListBox.GetUpperBound(0) - 1; Cnti++)
            //{
            //    TempDiffViewNum = (int)Math.Abs(cwneViewNum.Value - (modCT30K.OrgViewListBox[Cnti]));

            //    if (DiffViewNum > TempDiffViewNum)
            //    {
            //        DiffViewNum = TempDiffViewNum;

            //        //このときのビュー数が表示リストに入っているかチェック
            //        double value = 0;
            //        if (double.TryParse(Convert.ToString(cmbViewNum.Items[Cnti]), out value))
            //        {
            //            functionReturnValue = Cnti;
            //        }
            //    }
            //}
            if (!(cmbViewNum.Items.Count == 0))
            {
                //Rev20.00 実際に作成されたリストの数のループにする by長野 2014/11/10
                //for (Cnti = 0; Cnti <= modCT30K.OrgViewListBox.GetUpperBound(0) - 1; Cnti++)
                for (Cnti = 0; Cnti <= cmbViewNum.Items.Count - 1; Cnti++)
                {
                    //v19.16 修正 by長野 2013/08/03
                    if (!(string.IsNullOrEmpty(cmbViewNum.Items[Cnti].ToString())))
                    {
                        int viennum =0;
                        int.TryParse(cmbViewNum.Items[Cnti].ToString(), out viennum);
                        //2014/11/07hata キャストの修正
                        //TempDiffViewNum = System.Math.Abs((int)cwneViewNum.Value - viennum);
                        TempDiffViewNum = System.Math.Abs(Convert.ToInt32(cwneViewNum.Value) - viennum);

                        if (DiffViewNum > TempDiffViewNum)
                        {
                            DiffViewNum = TempDiffViewNum;

                            //このときのビュー数が表示リストに入っているかチェック
                            double value = 0;
                            if (double.TryParse(Convert.ToString(cmbViewNum.Items[Cnti]), out value))
                            {
                                functionReturnValue = Cnti;
                            }
                        }
                    }
                }
            }            
            
            return functionReturnValue;
		}


		//'********************************************************************************
		//'機    能  ：  連続の場合のビュー数のコンボボックスクリック時の処理
		//'              変数名           [I/O] 型        内容
		//'引    数  ：  なし
		//'戻 り 値  ：  なし
		//'補    足  ：  なし
		//'
		//'履    歴  ：  V19.10   12/07/30    長野     新規作成
		//'********************************************************************************
		private void cmbViewNum_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (optTableRotation1.Checked == true)
			{

                if (cmbViewNum.SelectedIndex >= 0)
                {
                   //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                   //cwneViewNum.Value = Convert.ToDecimal(cmbViewNum.Items[cmbViewNum.SelectedIndex]);
                    decimal fViewnum = Convert.ToDecimal(cmbViewNum.Items[cmbViewNum.SelectedIndex]);
                    if ((cwneViewNum.Maximum >= fViewnum) && (cwneViewNum.Minimum <= fViewnum))
                    {
                        //代入時のイベントロック
                        //ViewNumEvntLock = true;
                        cwneViewNum.Value = fViewnum;
                        //ViewNumEvntLock = false;
                    }
                    //追加2015/01/29hata
                    else if (cwneViewNum.Maximum < fViewnum)
                    {
                        cwneViewNum.Value = cwneViewNum.Maximum;
                    }
                    else if (cwneViewNum.Minimum > fViewnum)
                    {
                        cwneViewNum.Value = cwneViewNum.Minimum;
                    }               
                }
                else
                {
                    //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                    //cwneViewNum.Value = 600M;
                    if ((cwneViewNum.Maximum >= 600M) && (cwneViewNum.Minimum <= 600M))
                    {
                        //代入時のイベントロック
                        //ViewNumEvntLock = true;
                        cwneViewNum.Value = 600M;
                        //ViewNumEvntLock = false;
                    }
                    //追加2015/01/29hata
                    else if (cwneViewNum.Maximum < 600M)
                    {
                        cwneViewNum.Value = cwneViewNum.Maximum;
                    }
                    else if (cwneViewNum.Minimum > 600M)
                    {
                        cwneViewNum.Value = cwneViewNum.Minimum;
                    }
                }
            
            }
		}


		//'********************************************************************************
		//'機    能  ：  連続とステップを切り替えた時のビュー数設定欄の変更
		//'              変数名           [I/O] 型        内容
		//'引    数  ：  なし
		//'戻 り 値  ：  なし
		//'補    足  ：  なし
		//'
		//'履    歴  ：  V19.01   12/05/21    長野     新規作成
		//'********************************************************************************
		private void ChangeViewList()
		{
			//v19.10 追加 by長野 2012/05/21
			if (optTableRotation1.Checked == true)
			{
				cwneViewNum.Visible = false;
#region 【C#コントロールで代用】
//				cwneViewNum.DiscreteInterval = 1
#endregion
				cwneViewNum.Increment = 1;

                cmbViewNum.Visible = true;

				//連続の場合は，テーブル回転とフレームレートが合わなくなる（同期無しのため）ため決まったビュー数でのみスキャンするようにした
				//ビュー数操作ボックスのリストを作成
                
                //変更2014/10/07hata_v19.51反映
                //v19.15 この条件式は不要なので削除 by長野 2013/06/15
                //if (CTSettings.scaninh.Data.smooth_rot_cone == 0)
				//{
					CreateViewListComboBox();

					//cwneViewNumに保存されているビュー数に、最も近いビュー数を選択されているindexとする。
					cmbViewNum.SelectedIndex = SetViewListIndex();

				//}
			}
			else
			{
				cwneViewNum.Visible = true;
#region 【C#コントロールで代用】
//				cwneViewNum.DiscreteInterval = 1
#endregion
				cwneViewNum.Increment = 100;
				cwneViewNum.Minimum = CTSettings.GVal_ViewMin;
                cwneViewNum.Maximum = CTSettings.GVal_ViewMax;
				cmbViewNum.Visible = false;
			}
		}


		//*******************************************************************************
		//機　　能： cone_max_mfanangleを更新する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v19.20 2012/11/28 (検S1)長野   リニューアル
		//*******************************************************************************
		//Public Sub Update_cone_max_mfanangle()
		public void Update_scan_max_mfanangle()			//v19.12 max_mfanangleも更新するので関数名変更 by長野 2013/03/14
		{
			//Scancondpar呼び出し
			//modScancondpar.CallGetScancondpar();
            CTSettings.scancondpar.Load(CTSettings.scaninh.Data.rotate_select);

			//v19.20 cone_max_mfanangleが更新されないとmcが正しく求まらないため、ここで更新 2012/11/28 by長野
			float FlatPanel_e_dash = 0;
			float FlatPanel_Ils = 0;
			float FlatPanel_Ile = 0;
			float FlatPanel_kv = 0;
			float FlatPanel_bb0 = 0;
			float FlatPanel_Dpi = 0;
			float FGD = 0;

			//v19.12 max_mfanagle用の変数追加 by長野 2013/03/14
			float FlatPanelDelta_Ip = 0;
			float h = 0;
			float v = 0;
			float ic = 0;
			float jc = 0;

            //Rev25.00 Wスキャン用に追加 by長野 2016/08/16
            int sft_val = 0;
            sft_val = ((ScanCorrect.IsShiftScan() || ScanCorrect.IsW_Scan()) ? CTSettings.scancondpar.Data.det_sft_pix : 0);

			//ここからcone_max_mfanangleの計算
			FGD = ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[modCT30K.GetFcdOffsetIndex()];
			FlatPanel_bb0 = ScanCorrect.GVal_ScanPosiA[2];
            FlatPanel_kv = CTSettings.detectorParam.vm / CTSettings.detectorParam.hm;
			FlatPanel_Dpi = (float)(10 / ScanCorrect.B1);
			FlatPanel_e_dash = (float)Math.Atan(FlatPanel_kv * FlatPanel_bb0);
            //変更2014/10/07hata_v19.51反映
            //v19.12 ±8に変更 by長野 2013/03/06
			//FlatPanel_Ils = CTSettings.scancondpar.Data.ist + 8;
			//FlatPanel_Ile = CTSettings.scancondpar.Data.ied - 8;
            //v19.50 修正 by長野 2014/01/28
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))
            {
                if (CTSettings.detectorParam.h_size == 1024)
                {
                    FlatPanel_Ils = CTSettings.scancondpar.Data.ist + 12;
                    FlatPanel_Ile = CTSettings.scancondpar.Data.ied - 12;
                }
                else if (CTSettings.detectorParam.h_size == 2048)
                {
                    FlatPanel_Ils = CTSettings.scancondpar.Data.ist + 24;
                    FlatPanel_Ile = CTSettings.scancondpar.Data.ied - 24;
                }
            }
            else
            {
                FlatPanel_Ils = CTSettings.scancondpar.Data.ist + 2;
                FlatPanel_Ile = CTSettings.scancondpar.Data.ied - 2;
            }                      
            
            //FlatPanel_Ils = .ist + 2
			//FlatPanel_Ile = .ied - 2
			
            //Rev25.00 LimitFanAngleつきに変える by長野
            //CTSettings.scancondpar.Data.cone_max_mfanangle = (float)(2 * Math.Atan((FlatPanel_Ile - FlatPanel_Ils) * FlatPanel_Dpi / Math.Cos(FlatPanel_e_dash) * 1.02 * 0.5 / FGD));
            CTSettings.scancondpar.Data.cone_max_mfanangle = (float)((2 * Math.Atan((FlatPanel_Ile - FlatPanel_Ils) * FlatPanel_Dpi / Math.Cos(FlatPanel_e_dash) * 1.02 * 0.5 / FGD)) * ScanCorrect.LimitFanAngle);

    		//v19.12 max_mfanangleの計算もここで行う
			//ここからmax_mfanangleの計算
            h = CTSettings.detectorParam.h_size;
            v = CTSettings.detectorParam.v_size;
			ic = (h - 1) / 2;
			jc = (v - 1) / 2;

			FlatPanel_Dpi = (float)(10 / ScanCorrect.A1[2]);

			//2014/11/07hata キャストの修正
            //FlatPanelDelta_Ip = (int)(2 + jc * FlatPanel_kv * FlatPanel_kv * Math.Abs(FlatPanel_bb0));
			FlatPanelDelta_Ip = Convert.ToInt32(Math.Floor(2 + jc * FlatPanel_kv * FlatPanel_kv * Math.Abs(FlatPanel_bb0)));

            FlatPanel_Ils = FlatPanelDelta_Ip;

			//FlatPanel_Ile = h - FlatPanelDelta_Ip - 1;
            //Rev25.00 Wスキャン対応のためIle変更 by長野 2016/08/16
            FlatPanel_Ile = h + sft_val - FlatPanelDelta_Ip - 1;   //v18.00変更 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

			CTSettings.scancondpar.Data.max_mfanangle[2] = (float)(2 * Math.Atan((FlatPanel_Ile - FlatPanel_Ils) * FlatPanel_Dpi / Math.Cos(FlatPanel_e_dash) * 1.02 * 0.5 / FGD));

			//modScancondpar.CallPutScancondpar();
            CTSettings.scancondpar.Write();
		}

        //追加2015/01/20hata
        //データを再表示する
        private void NumicValue_Leave(object sender, EventArgs e)
        {
            if (sender as NumericUpDown == null) return;

            NumericUpDown udbtn = (NumericUpDown)sender;

            string sval = udbtn.Value.ToString();
            //変更2015/01/28hata
            if(string.IsNullOrEmpty(sval))
            {
                udbtn.Value = udbtn.Minimum;
                return;
            }
            udbtn.Text = sval;
            //if (sval == "")
            //{
            //    udbtn.Value = udbtn.Minimum;
            //    return;
            //}

            ////Rev20.00 test by長野 2015/01/28
            //if (udbtn.Text == "")
            //{
            //    MessageBox.Show("空欄を入力することはできません。数値を入力してください。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    udbtn.Focus();
            //}

            /* Rev20.00 test by長野 2015/01/28
            //Rev20.00 畑さんに変更・コメント追加をしてもらった by長野 2015/01/28
            //同じ数字の場合に表示されないため、一旦数字を変える
            //if (udbtn.Value + 1 > udbtn.Maximum)
            //{
            //    udbtn.Value = udbtn.Value - (decimal)(1 / Math.Exp(udbtn.DecimalPlaces));
            //}
            //else if (udbtn.Value - 1 < udbtn.Minimum)
            //{
            //    udbtn.Value = udbtn.Value + (decimal)(1 / Math.Exp(udbtn.DecimalPlaces));
            //}
            decimal val = (decimal)(1 / Math.Pow(10, udbtn.DecimalPlaces));

            //Rev20.00 最小と最大が一致する場合を追加 by長野 2015/01/28
            //if (udbtn.Minimum == udbtn.Maximum)
            //{
            //    decimal oldMax = udbtn.Maximum;
            //    udbtn.Maximum = udbtn.Maximum + val;
            //    udbtn.Value = udbtn.Value + val;
            //    udbtn.Maximum = oldMax;
            //}
            //elseif (udbtn.Value + val > udbtn.Maximum)
            if (udbtn.Value + val > udbtn.Maximum)
            {
                udbtn.Value = udbtn.Value - (decimal)(1 / Math.Pow(10, udbtn.DecimalPlaces));
                udbtn.Value = udbtn.Value - val;
            }
            //else if (udbtn.Value - val < udbtn.Minimum)
            //{
            //    udbtn.Value = udbtn.Value + (decimal)(1 / Math.Pow(10, udbtn.DecimalPlaces));
            //}
            else //Rev20.00 追加 いずれでもない場合は+valして値を変える by長野 2015/01/28
            {
                udbtn.Value = udbtn.Value + val;
            }
            udbtn.Value = Convert.ToDecimal(sval);
            */
        }
        //*******************************************************************************
        //機　　能： プリセットリストから削除
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v26.00 2017/08/31 (CT開)長野   新規作成
        //*******************************************************************************
        private void cmdDeleteCondition_Click(object sender, EventArgs e)
        {
            string FileName = Path.Combine(AppValue.PathScanCondPresetFile + txtPresetName.Text) + "-SC.csv";

            if (!File.Exists(FileName))
            {
                //メッセージ表示：指定されたプリセットファイルは存在しません。
                MessageBox.Show(CTResources.LoadResString(26024), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //メッセージ表示
			//　指定されたプリセットファイルを削除します。
            //  よろしいですか?
			DialogResult result = MessageBox.Show(CTResources.LoadResString(26025),Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (result == DialogResult.Yes)
            {
                File.Delete(FileName);
                modScanCondition.deletePresetList(txtPresetName.Text);

                MessageBox.Show(CTResources.LoadResString(26026), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.None);
            }

            frmScanControl.Instance.UpdatePresetList(2);

            this.Close();
        }
   }
}
