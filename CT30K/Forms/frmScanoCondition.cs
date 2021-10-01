using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

using CT30K.Common;
using CTAPI;
using TransImage;
using CT30K.Properties;

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： frmScanoCondition.cs                                        */
    ///* 処理概要　　： スキャノ条件                                                */
    ///* 注意事項　　： なし                                                        */
    ///* -------------------------------------------------------------------------- */
    ///* 適用計算機　： DOS/V PC                                                    */
    ///* ＯＳ　　　　： Windows 7  (SP1)                                            */
    ///* コンパイラ　： VS2010                                                      */
    ///* -------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
    ///*                                                                            */
    ///* v20.00      15/02/19    M.Chouno            新規作成                       */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2015                 */
    ///* ************************************************************************** */
    ///
    public partial class frmScanoCondition : Form
    {

        private RadioButton[] optScanoMatrix = null;
        private Panel[] fraMenu = null;

        //追加2015/01/27hata
        private static frmScanoCondition _Instance = null;
        private frmXrayControl.ChangedEventArgs changedEventArgs = null;

        private bool EvntLock = false;

        //scansel（コモン）バックアップ用
        //private modScansel.scanselType ScanselOrg;   //v11.2追加 by 間々田 2006/01/13
        private CTstr.SCANSEL ScanselOrg;   //v11.2追加 by 間々田 2006/01/13

        private float FIDWithOffset = 0;	//FID（オフセットを含む）
        private float FCDWithOffset = 0;	//FCD（オフセットを含む）
        private float FCDFIDRate = 0;		//FcdWithOffset/FidWithOffset

        private float SWMin = 0;			//FcdFidRate * scancondpar.mdtpitch(2) * vm / hm

        //昇降位置
        private float udPos;

        private char presskey = (char)0;  //Keypressの値          


        //メカ制御画面への参照
        private frmMechaControl myMechaControl = null;

        //Ｘ線制御画面への参照
        private frmXrayControl myXrayControl = null;

        private bool myNoClose = false;

        private long MAXMEM = 1900;

        //Rev22.00 Rev21.01の反映 by長野 2015/07/24
        private double myUdMaxSpeed = 0;
        public double UdMaxSpeed
        {
            get
            {
                return myUdMaxSpeed;
            }
        }
        private double myUdMinSpeed = 0;
        public double UdMinSpeed
        {
            get
            {
                return myUdMaxSpeed;
            }
        }
        private double myUdStep = 0;
        public double UdStep
        {
            get
            {
                return myUdStep;
            }
        }

        public frmScanoCondition()
        {
            InitializeComponent();

            optScanoMatrix = new RadioButton[] { null, optScanoMatrix1, optScanoMatrix2, optScanoMatrix3, optScanoMatrix4, optScanoMatrix5 };
            fraMenu = new Panel[] { fraMenu0, null, null };
        
            changedEventArgs = new frmXrayControl.ChangedEventArgs();

        }

        public static frmScanoCondition Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmScanoCondition();
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
        //履　　歴： V21.00  15/02/19  (検S1)長野      新規作成
        //*************************************************************************************************
        private void frmScanoCondition_Load(object sender, EventArgs e)
        {
            //tes中!!_畑_2014/09/11
            EvntLock = true;

            //フラグをセット
            modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTScanoCondition;

            //キャプションのセット
            SetCaption();

            //フォームの表示位置                             'v15.0変更 by 間々田 2009/02/27
            modCT30K.SetPosNormalForm(this);

            //scancondpar（コモン）の読み込み                'v11.2追加 by 間々田 2005/10/06
            //modScancondpar.CallGetScancondpar();
            CTSettings.scancondpar.Load(CTSettings.scaninh.Data.rotate_select);

            //コントロールの初期化
            InitControls();

            //スキャン条件の一時保存
            ScanselOrg = new CTstr.SCANSEL();
            ScanselOrg.Initialize();
            ScanselOrg = CTSettings.scansel.Data;

            //Rev22.00 Rev21.01の反映 by長野 2015/07/31
            LoadUdMaxMinSpeed();

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

            //Rev22.00 Rev21.01の反映 by長野 2015/07/31
            changeIntegByUdSpeed();
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
        //履　　歴： V21.00 15/02/19  (検S1)長野      新規作成
        //           
        //*******************************************************************************
        private void SetCaption()
        {
            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            this.Text = StringTable.BuildResStr(StringTable.IDS_Details, StringTable.IDS_ScanoCondition);		//スキャン条件－詳細

            if (modCT30K.IsEnglish)
            {
                lblPixDsp.Text = "pix.)";
                lblPixDsp2.Text = "pix.)";
            }
            else
            {
                //スライス厚フレーム
                lblPixDsp.Text = CTResources.LoadResString(StringTable.IDS_Pixels) + ")";		//画素)
                //スライス厚フレーム
                lblPixDsp2.Text = CTResources.LoadResString(StringTable.IDS_Pixels) + ")";		//画素)
            }

            //フレーム
            optScanoMatrix1.Text = CTSettings.infdef.Data.matrixsize[0].GetString();					//256×256
            optScanoMatrix2.Text = CTSettings.infdef.Data.matrixsize[1].GetString();					//512×512
            optScanoMatrix3.Text = CTSettings.infdef.Data.matrixsize[2].GetString();					//1024×1024
            optScanoMatrix4.Text = CTSettings.infdef.Data.matrixsize[3].GetString();					//2048×2048
            optScanoMatrix5.Text = CTSettings.infdef.Data.matrixsize[4].GetString();					//4096×4096 '4096対応 v16.10 by 長野 10/01/29

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
        //履　　歴： v21.00  15/02/19   (検S1)長野      新規作成
        //*************************************************************************************************
        private void InitControls()
        {
            //マトリクスサイズ
            //1024以外は使用不可
            optScanoMatrix1.Enabled = false;		// 256x 256
            optScanoMatrix2.Enabled = false;		// 512x 512
            optScanoMatrix3.Enabled = true; 		//1024x1024
            optScanoMatrix4.Enabled = false;		//2048x2048
            optScanoMatrix5.Enabled = false;		//4096x4096 '4096対応 v16.10 by 長野　10/01/29

            //マトリクスサイズオプションボタンの位置の調整
            modLibrary.RePosOption2(optScanoMatrix, 4);

            //フレームの境界線を消す
            fraMenu0.BorderStyle = BorderStyle.None;

            //透視画像サイズの調整 
            //Rev21.00 開発時はビニング考慮せず。
            ntbTransImageWidth.Value = (decimal)(CTSettings.scancondpar.Data.h_size);
            ntbTransImageHeight.Value = (decimal)(CTSettings.scancondpar.Data.v_size);

            //最初に「条件設定」のタブを表示
            sstMenu.SelectedIndex = 0;
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
        //履　　歴： V21.00  15/02/19  (検S1)長野      新規作成
        //*************************************************************************************************
        private void SetControls()
        {

            //マトリクスサイズ：2(512x512), 3(1024x1024), 4(2048x2048)
            //Rev21.00 スキャノは3だけ。
            modLibrary.SetOption(optScanoMatrix, 3, (int)ScanSel.MatrixSizeConstants.MatrixSize1024);

            //画像積算枚数   最小 infdef.min_integ_number 最大 infdef.max_integ_number
            cwneScanoInteg.Minimum = (decimal)CTSettings.GValIntegNumMin;
            cwneScanoInteg.Maximum = (decimal)CTSettings.GValIntegNumMax;
            
            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneInteg.Value = (decimal)CTSettings.scansel.Data.scan_integ_number;
            if ((cwneScanoInteg.Maximum >= (decimal)CTSettings.scansel.Data.mscano_integ_number) && (cwneScanoInteg.Minimum <= (decimal)CTSettings.scansel.Data.mscano_integ_number))
            {
                //代入時のイベントロック
                //IntegEvntLock = true;
                cwneScanoInteg.Value = (decimal)CTSettings.scansel.Data.mscano_integ_number;
                //IntegEvntLock = false;
            }
            //追加2015/01/29hata
            else if (cwneScanoInteg.Maximum < (decimal)CTSettings.scansel.Data.mscano_integ_number)
            {
                cwneScanoInteg.Value = cwneScanoInteg.Maximum;
            }
            else if (cwneScanoInteg.Minimum > (decimal)CTSettings.scansel.Data.mscano_integ_number)
            {
                cwneScanoInteg.Value = cwneScanoInteg.Minimum;
            }

            //バイアス
            ntbScanoBias.Value = (decimal)CTSettings.scansel.Data.mscano_bias;

            //スロープ
            ntbScanoSlope.Value = (decimal)CTSettings.scansel.Data.mscano_slope;

            UpdateFcdFid();
            
            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneSlicePix.Value = (decimal)(CTSettings.scansel.Data.mscan_width / CTSettings.scansel.Data.min_slice_wid);
            decimal fScanoWidthPix = (decimal)(CTSettings.scansel.Data.mscano_width / CTSettings.scansel.Data.min_slice_wid);
            
            //Rev20.00 四捨五入を追加 by長野 2015/01/26
            fScanoWidthPix = Math.Round(fScanoWidthPix / cwneScanoWidthPix.Increment, MidpointRounding.AwayFromZero) * cwneScanoWidthPix.Increment;
            if ((cwneScanoWidthPix.Maximum >= fScanoWidthPix) && (cwneScanoWidthPix.Minimum <= fScanoWidthPix))
            {
                //代入時のイベントロック
                cwneScanoWidthPix.Value = fScanoWidthPix;
            }
            else if (cwneScanoWidthPix.Maximum < fScanoWidthPix)
            {
                cwneScanoWidthPix.Value = cwneScanoWidthPix.Maximum;
            }
            else if (cwneScanoWidthPix.Minimum > fScanoWidthPix)
            {
                cwneScanoWidthPix.Value = cwneScanoWidthPix.Minimum;
            }
           
            //FCD, FID値が前回と変更している場合、画素値固定としてスライス厚（mm）の再計算
            if (CTSettings.scansel.Data.fcd != frmMechaControl.Instance.FCDWithOffset || CTSettings.scansel.Data.fid != frmMechaControl.Instance.FIDWithOffset)
            {
                //cwneSlicePix_ValueChanged(cwneSlicePix, EventArgs.Empty);
                //Rev20.00 イベントだとLoad時はスルーするためイベントじゃないcwneSlicePixを実行
                //VBではLoad時にイベントが実行されないので上記のような形をとってあると思われるので、この修正を行った by長野 2015/02/05
                cwneScanoWidthPix_ValueChangedNoEvent();
            }
            else
            {
                //最小値・最大値を設定
                cwneScanoWidth.Minimum = Math.Round((decimal)CTSettings.scansel.Data.min_slice_wid / cwneScanoWidth.Increment, MidpointRounding.AwayFromZero) * cwneScanoWidth.Increment;
                cwneScanoWidth.Maximum = Math.Round((decimal)CTSettings.scansel.Data.max_slice_wid / cwneScanoWidth.Increment, MidpointRounding.AwayFromZero) * cwneScanoWidth.Increment;

                //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                //cwneSlice.Value = (decimal)CTSettings.scansel.Data.mscan_width;				//スライス厚(mm)
                if ((cwneScanoWidth.Maximum >= (decimal)CTSettings.scansel.Data.mscano_width) && (cwneScanoWidth.Minimum <= (decimal)CTSettings.scansel.Data.mscano_width))
                {
                    //代入時のイベントロック
                    //SliceEvntLock = true;
                    cwneScanoWidth.Value = (decimal)CTSettings.scansel.Data.mscano_width;				//スライス厚(mm)
                    //SliceEvntLock = false;
                }
                //追加2015/01/29hata
                else if (cwneScanoWidth.Maximum < (decimal)CTSettings.scansel.Data.mscano_width)
                {
                    cwneScanoWidth.Value = cwneScanoWidth.Maximum;
                }
                else if (cwneScanoWidth.Minimum > (decimal)CTSettings.scansel.Data.mscano_width)
                {
                    cwneScanoWidth.Value = cwneScanoWidth.Minimum;
                }
            }

            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneSlicePix.Value = (decimal)(CTSettings.scansel.Data.mscan_width / CTSettings.scansel.Data.min_slice_wid);
            decimal fScanoSlicePitchPix = (decimal)(CTSettings.scansel.Data.mscano_udpitch / CTSettings.scansel.Data.min_slice_wid);
            //Rev20.00 四捨五入を追加 by長野 2015/01/26
            fScanoSlicePitchPix = Math.Round(fScanoSlicePitchPix / cwneScanoSlicePitchPix.Increment, MidpointRounding.AwayFromZero) * cwneScanoSlicePitchPix.Increment;

            if ((cwneScanoSlicePitchPix.Maximum >= fScanoSlicePitchPix) && (cwneScanoSlicePitchPix.Minimum <= fScanoSlicePitchPix))
            {
                //代入時のイベントロック
                cwneScanoSlicePitchPix.Value = fScanoSlicePitchPix;
            }
            else if (cwneScanoSlicePitchPix.Maximum < fScanoSlicePitchPix)
            {
                cwneScanoSlicePitchPix.Value = cwneScanoSlicePitchPix.Maximum;
            }
            else if (cwneScanoSlicePitchPix.Minimum > fScanoSlicePitchPix)
            {
                cwneScanoSlicePitchPix.Value = cwneScanoSlicePitchPix.Minimum;
            }

            //FCD, FID値が前回と変更している場合、画素値固定としてスライス厚（mm）の再計算
            if (CTSettings.scansel.Data.fcd != frmMechaControl.Instance.FCDWithOffset || CTSettings.scansel.Data.fid != frmMechaControl.Instance.FIDWithOffset)
            {
                //cwneSlicePix_ValueChanged(cwneSlicePix, EventArgs.Empty);
                //Rev20.00 イベントだとLoad時はスルーするためイベントじゃないcwneSlicePixを実行
                //VBではLoad時にイベントが実行されないので上記のような形をとってあると思われるので、この修正を行った by長野 2015/02/05
                cwneScanoSlicePitchPix_ValueChangedNoEvent();
            }
            else
            {
                //最小値・最大値を設定
                cwneScanoSlicePitch.Minimum = Math.Round((decimal)CTSettings.scansel.Data.min_slice_wid / cwneScanoSlicePitch.Increment, MidpointRounding.AwayFromZero) * cwneScanoSlicePitch.Increment;
                //cwneScanoSlicePitch.Maximum = Math.Round((decimal)CTSettings.scansel.Data.max_slice_wid / cwneScanoSlicePitch.Increment, MidpointRounding.AwayFromZero) * cwneScanoSlicePitch.Increment;
                //Rev21.00 修正 by長野 2015/03/16
                cwneScanoSlicePitch.Maximum = Math.Round((decimal)CTSettings.scansel.Data.max_slice_wid / cwneScanoSlicePitch.Increment, MidpointRounding.AwayFromZero) * (decimal)0.01;

                //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
                //cwneSlice.Value = (decimal)CTSettings.scansel.Data.mscan_width;				//スライス厚(mm)
                if ((cwneScanoSlicePitch.Maximum >= (decimal)CTSettings.scansel.Data.mscano_udpitch) && (cwneScanoSlicePitch.Minimum <= (decimal)CTSettings.scansel.Data.mscano_udpitch))
                {
                    //代入時のイベントロック
                    //SliceEvntLock = true;
                    cwneScanoSlicePitch.Value = (decimal)CTSettings.scansel.Data.mscano_udpitch;				//スライス厚(mm)
                    //SliceEvntLock = false;
                }
                //追加2015/01/29hata
                else if (cwneScanoSlicePitch.Maximum < (decimal)CTSettings.scansel.Data.mscano_udpitch)
                {
                    cwneScanoWidth.Value = cwneScanoWidth.Maximum;
                }
                else if (cwneScanoSlicePitch.Minimum > (decimal)CTSettings.scansel.Data.mscano_udpitch)
                {
                    cwneScanoSlicePitch.Value = cwneScanoSlicePitch.Minimum;
                }
            }

            //現在の昇降位置を取得する：
            //スキャノデータ収集範囲を決める。
            myMechaControl_UDPosChanged(this, EventArgs.Empty);

            //収集終了位置をセット
            if (cwneScanoDataRange.Maximum <= (decimal)CTSettings.scansel.Data.mscano_data_endpos)
            {
                cwneScanoDataRange.Value = cwneScanoDataRange.Maximum;
            }
            else if (cwneScanoDataRange.Minimum >= (decimal)CTSettings.scansel.Data.mscano_data_endpos)
            {
                cwneScanoDataRange.Value = cwneScanoDataRange.Minimum;
            }
            else
            {
                cwneScanoDataRange.Value = (decimal)CTSettings.scansel.Data.mscano_data_endpos;
            }

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
        //履　　歴： V21.00  15/02/19   (検S1)長野        新規作成
        //*******************************************************************************
        private void myMechaControl_UDPosChanged(object sender, EventArgs e)
        {
            udPos = (float)frmMechaControl.Instance.ntbUpDown.Value;

            //データ収集範囲を計算。
            cwneScanoDataRange.Minimum = (decimal)udPos;
            if (cwneScanoDataRange.Minimum >= cwneScanoDataRange.Maximum)
            {
                cwneScanoDataRange.Minimum = cwneScanoDataRange.Maximum;
            }
            long    tmpScanopt = 0;
            long    datasize = 0;
            decimal tmpVal1 = 0;
            decimal tmpVal2 = 0;
            tmpVal2 = (decimal)((float)CTSettings.t20kinf.Data.upper_limit / 100.0 - udPos);
            tmpScanopt = (long)(tmpVal2 / (decimal)(SWMin)) + 1;
            datasize = tmpScanopt * CTSettings.scancondpar.Data.h_size * 2;
            if(datasize > (MAXMEM * 1024 * 1024))
            {
                tmpVal1 = (MAXMEM * 1024 * 1024) / (CTSettings.scancondpar.Data.h_size * 2);
                cwneScanoDataRange.Maximum = tmpVal1 * (decimal)SWMin + (decimal)udPos;      
            }
            else
            {
                cwneScanoDataRange.Maximum = (decimal)CTSettings.t20kinf.Data.upper_limit / (decimal)100.0;
            }
            lblScanoDataRange_max.Text = cwneScanoDataRange.Maximum.ToString();
            lblScanoDataRange_min.Text = cwneScanoDataRange.Minimum.ToString();
            lblScanoDataRange_min2.Text = cwneScanoDataRange.Minimum.ToString();

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
        //履　　歴： V21.00  15/02/19   (検S1)長野      新規作成
        //*******************************************************************************
        private void UpdateFcdFid()
        {
            float OldSWMin = 0;
            OldSWMin = (float)cwneScanoWidth.Minimum;

            //FID/FCDオフセット
            //Rev21.00 開発時は、1通りのFCD/FDDを使用
            FIDWithOffset = ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[0];		//FID（オフセットを含む）
            FCDWithOffset = ScanCorrect.GVal_Fcd + CTSettings.scancondpar.Data.fcd_offset[0];		//FCD（オフセットを含む）

            //Fcd/Fid比率
            FCDFIDRate = FCDWithOffset / FIDWithOffset;

            //スライス厚最小
            SWMin = modLibrary.MaxVal(FCDFIDRate * CTSettings.scancondpar.Data.mdtpitch[2] * CTSettings.detectorParam.vm / CTSettings.detectorParam.hm, 0.001F);

            //最大・最小スキャノ厚・スキャノピッチの更新
            UpdateSliceMinMax();

            //スライス厚更新
            if ((float)cwneScanoWidth.Minimum != OldSWMin)
            {
                #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                /*
				cwneSlicePix_ValueChanged cwneSlicePix.Value, cwneSlicePix.Value, False
*/
                #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
                cwneScanoWidthPix_ValueChanged(cwneScanoWidthPix, EventArgs.Empty);
            }
        }
        //*******************************************************************************
        //機　　能： スキャノ厚（画素）変更時処理（Load時にも動かせるようにするため）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //*******************************************************************************
        private void cwneScanoWidthPix_ValueChangedNoEvent()
        {

            string cwneScanoWitdhFormat = string.Format("F{0}", cwneScanoWidth.DecimalPlaces);

            float r = 0;
            r = SWMin;

            decimal value = 0;
            decimal.TryParse((cwneScanoWidthPix.Value * (decimal)r).ToString(cwneScanoWitdhFormat), out value);

            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneSlice.Value = value;
            if ((cwneScanoWidth.Maximum >= value) && (cwneScanoWidth.Minimum <= value))
            {
                //Rev20.00 コーンの場合は、自動で変更しない by長野 2015/01/26
                //Rev20.00 元に戻す by長野 2015/02/05
                //if (!IsCone)
                //{
                //代入時のイベントロック
                //SliceEvntLock = true;
                cwneScanoWidth.Value = value;
                //SliceEvntLock = false;
                //}
            }
            //追加2015/01/29hata
            else if (cwneScanoWidth.Maximum < value)
            {
                cwneScanoWidth.Value = cwneScanoWidth.Maximum;
            }
            else if (cwneScanoWidth.Minimum > value)
            {
                cwneScanoWidth.Value = cwneScanoWidth.Minimum;
            }

            //変更した場合，ＯＫボタンを使用可にする
            if (this.ActiveControl == cwneScanoWidth) cmdOK.Enabled = true;
        }
        //*******************************************************************************
        //機　　能： スキャノ厚（画素）変更時処理（Load時にも動かせるようにするため）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //*******************************************************************************
        private void cwneScanoWidthPix_ValueChanged(object sender, EventArgs e)
        {
            //if (IntegEvntLock) return;   //代入時のイベントロック 追加2014/06/23(検S1)hata
            if (EvntLock) return;   //Load時のイベントロック 追加2014/06/23(検S1)hata

            string cwneScanoWitdhFormat = string.Format("F{0}", cwneScanoWidth.DecimalPlaces);

            float r = 0;
            r = SWMin;

            decimal value = 0;
            decimal.TryParse((cwneScanoWidthPix.Value * (decimal)r).ToString(cwneScanoWitdhFormat), out value);

            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneSlice.Value = value;
            if ((cwneScanoWidth.Maximum >= value) && (cwneScanoWidth.Minimum <= value))
            {
                //Rev20.00 コーンの場合は、自動で変更しない by長野 2015/01/26
                //Rev20.00 元に戻す by長野 2015/02/05
                //if (!IsCone)
                //{
                //代入時のイベントロック
                //SliceEvntLock = true;
                cwneScanoWidth.Value = value;
                //SliceEvntLock = false;
                //}
            }
            //追加2015/01/29hata
            else if (cwneScanoWidth.Maximum < value)
            {
                cwneScanoWidth.Value = cwneScanoWidth.Maximum;
            }
            else if (cwneScanoWidth.Minimum > value)
            {
                cwneScanoWidth.Value = cwneScanoWidth.Minimum;
            }

            //Rev22.00 Rev21.01の反映 by長野 2015/07/31
            changeIntegByUdSpeed();

            //透視画像のラインを更新する
            UpdateLine();

            //変更した場合，ＯＫボタンを使用可にする
            if (this.ActiveControl == cwneScanoWidth) cmdOK.Enabled = true;
        }
        //*******************************************************************************
        //機　　能： スキャノピッチ（画素）変更時処理（Load時にも動かせるようにするため）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //*******************************************************************************
        private void cwneScanoSlicePitchPix_ValueChangedNoEvent()
        {
            //if (IntegEvntLock) return;   //代入時のイベントロック 追加2014/06/23(検S1)hata
            //Rev26.40 修正 by chouno 2019/03/25
            //if (EvntLock) return;   //Load時のイベントロック 追加2014/06/23(検S1)hata

            string cwneScanoSliceFormat = string.Format("F{0}", cwneScanoSlicePitch.DecimalPlaces);

            float r = 0;
            r = SWMin;

            decimal value = 0;
            //decimal.TryParse((cwneScanoSlicePitch.Value * (decimal)r).ToString(cwneScanoSliceFormat), out value);
            //Rev26.40 修正 by chouno 2019/03/25
            decimal.TryParse((cwneScanoSlicePitchPix.Value * (decimal)r).ToString(cwneScanoSliceFormat), out value);

            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneSlice.Value = value;
            if ((cwneScanoSlicePitch.Maximum >= value) && (cwneScanoSlicePitch.Minimum <= value))
            {
                //Rev20.00 コーンの場合は、自動で変更しない by長野 2015/01/26
                //Rev20.00 元に戻す by長野 2015/02/05
                //if (!IsCone)
                //{
                //代入時のイベントロック
                //SliceEvntLock = true;
                cwneScanoSlicePitch.Value = value;
                //SliceEvntLock = false;
                //}
            }
            //追加2015/01/29hata
            else if (cwneScanoSlicePitch.Maximum < value)
            {
                cwneScanoSlicePitch.Value = cwneScanoSlicePitch.Maximum;
            }
            else if (cwneScanoSlicePitch.Minimum > value)
            {
                cwneScanoSlicePitch.Value = cwneScanoSlicePitch.Minimum;
            }

            //変更した場合，ＯＫボタンを使用可にする
            if (this.ActiveControl == cwneScanoSlicePitch) cmdOK.Enabled = true;
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
                frmTransImage.Instance.SetLine((double)cwneScanoWidth.Value, 1, 0);
            }
        }
        //********************************************************************************
        //機    能  ：  最大・最小スキャノ厚の再計算と表示を行う
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：
        //
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //********************************************************************************
        private void UpdateSliceMinMax()
        {
            float theMax = 0;			//最大スライス厚(mm)
            float theMaxPix = 0;		//最大スライス厚(画素)
            float theMin = 0;			//最小スライス厚(mm)小数点第４位以下を必ず切り上げ 'v19.00 追加 by長野 2012-04-05

            float thePitchMax = 0;			//最大スライス厚(mm)
            float thePitchMaxPix = 0;		//最大スライス厚(画素)
            float thePitchMin = 0;			//最小スライス厚(mm)小数点第４位以下を必ず切り上げ 'v19.00 追加 by長野 2012-04-05

            //最大スライス厚計算(mm)
            theMax = ScanCorrect.GetSWMax(Convert.ToInt16(ntbTransImageHeight.Value),
                                          Convert.ToInt16(ntbTransImageWidth.Value),
                                          CTSettings.scancondpar.Data.scan_posi_a[2],
                                          CTSettings.scancondpar.Data.scan_posi_b[2],
                                          SWMin,
                                          0,
                                          0);
       
            //画素に変換：画素単位で10を越えた場合は、10で抑える
            theMaxPix = modLibrary.MinVal(theMax / SWMin, 10);

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

            string cwneScanoWidthFormat = string.Format("F{0}", cwneScanoWidth.DecimalPlaces);

            //最小スライス厚を小数点第４以下で四捨五入すると理論値以下を設定できるように
            //なるため、必ず切り上げるようにする by長野 2012-04-03 v19.00
            //変更2015/01/20hata_計算が合わなくなるので切り上げしない
            //float.TryParse((modLibrary.MaxVal(SWMin, 0.001) + 0.0009999).ToString(cwneSliceFormat), out theMin);
            float.TryParse((modLibrary.MaxVal(SWMin, 0.001)).ToString(cwneScanoWidthFormat), out theMin);

            //最小値・最大値の設定
            //SWMinではなく、小数点第４以下で切り上げたtheMinを使う by長野 2012-04-05
            //.SetMinMax Val(Format$(SWMin, .FormatString)), Val(Format$(theMax, .FormatString))
            decimal theMinValue = 0;
            decimal theMaxValue = 0;
            decimal.TryParse(theMin.ToString(cwneScanoWidthFormat), out theMinValue);
            decimal.TryParse(theMax.ToString(cwneScanoWidthFormat), out theMaxValue);
            cwneScanoWidth.Minimum = theMinValue;
            cwneScanoWidth.Maximum = theMaxValue;

            //最小値・最大値の表示
            lblScanoWidthMinMax.Text = StringTable.GetResString(StringTable.IDS_RangeMM,
                                                           cwneScanoWidth.Minimum.ToString(cwneScanoWidthFormat),
                                                           cwneScanoWidth.Maximum.ToString(cwneScanoWidthFormat));

            //スキャノピッチ更新
            //最大スライス厚計算(mm)
            theMax = ScanCorrect.GetSWMax(Convert.ToInt16(ntbTransImageHeight.Value),
                                          Convert.ToInt16(ntbTransImageWidth.Value),
                                          CTSettings.scancondpar.Data.scan_posi_a[2],
                                          CTSettings.scancondpar.Data.scan_posi_b[2],
                                          SWMin,
                                          0,
                                          0);

            //画素に変換：画素単位で100を越えた場合は、100で抑える
            thePitchMaxPix = modLibrary.MinVal(theMax / SWMin, 100);

            //mmに変換して返す
            thePitchMax = thePitchMaxPix * SWMin;

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

            string cwneScanoSlicePitchFormat = string.Format("F{0}", cwneScanoSlicePitch.DecimalPlaces);

            //最小スライス厚を小数点第４以下で四捨五入すると理論値以下を設定できるように
            //なるため、必ず切り上げるようにする by長野 2012-04-03 v19.00
            //変更2015/01/20hata_計算が合わなくなるので切り上げしない
            //float.TryParse((modLibrary.MaxVal(SWMin, 0.001) + 0.0009999).ToString(cwneSliceFormat), out theMin);
            float.TryParse((modLibrary.MaxVal(SWMin, 0.001)).ToString(cwneScanoSlicePitchFormat), out theMin);

            //最小値・最大値の設定
            //SWMinではなく、小数点第４以下で切り上げたtheMinを使う by長野 2012-04-05
            //.SetMinMax Val(Format$(SWMin, .FormatString)), Val(Format$(theMax, .FormatString))
            decimal thePitchMinValue = 0;
            decimal thePitchMaxValue = 0;
            decimal.TryParse(theMin.ToString(cwneScanoSlicePitchFormat), out thePitchMinValue);
            //decimal.TryParse(theMax.ToString(cwneScanoSlicePitchFormat), out thePitchMaxValue);
            //Rev21.00 修正 by長野 2015/03/16
            decimal.TryParse(thePitchMax.ToString(cwneScanoSlicePitchFormat), out thePitchMaxValue);

            cwneScanoSlicePitch.Minimum = thePitchMinValue;
            cwneScanoSlicePitch.Maximum = thePitchMaxValue;

            //最小値・最大値の表示
            lblScanoSliceMinMax.Text = StringTable.GetResString(StringTable.IDS_RangeMM,
                                                           cwneScanoSlicePitch.Minimum.ToString(cwneScanoWidthFormat),
                                                           cwneScanoSlicePitch.Maximum.ToString(cwneScanoWidthFormat));
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
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
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
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //*******************************************************************************
        private void myMechaControl_FIDChanged(object sender, EventArgs e)
        {
            //ＦＣＤ・ＦＩＤ値変更時処理
            UpdateFcdFid();
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
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //*************************************************************************************************
        private void myXrayControl_Changed(object sender, frmXrayControl.ChangedEventArgs e)
        {
            //東芝 EXM2-150以外は単位をmAに変換
            if (modXrayControl.XrayType != modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150) e.current = e.current / 1000;

            if (e.volt == 0) return;
            if (e.current == 0) return;
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
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //*******************************************************************************
        private void cmdOK_Click(object sender, EventArgs e)
        {
            SetScanoCondition();
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
        //機　　能： ＯＫボタン・クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //*******************************************************************************        
        private void SetScanoCondition()
        {

            //スキャノ条件の設定内容をチェック（その１）
            if (!OptValueScanoChk1())
            {
                //Rev20.00 最後の処理を行うように変更 by長野 2014/12/15
                goto ErrorHandler;
                //return;
            }

            //マウスポインタを砂時計にする
            this.Cursor = Cursors.WaitCursor;

            //スキャン条件の設定内容をコモンファイルへ書き込む
            //modScansel.scanselType theScansel = default(modScansel.scanselType);
            CTstr.SCANSEL theScansel = new CTstr.SCANSEL();// = default(CTstr.SCANSEL);
            theScansel.Initialize();

            //この画面で設定しているスキャン条件を取得
            GetControls(ref theScansel);

            //scansel書き込み
            //modScansel.PutScansel(ref theScansel);
            CTSettings.scansel.Put(theScansel);

            //MyScanselを更新                                            '追加 by 間々田 2009/07/31 下から移動
            modCommon.GetMyScansel();

            //マトリクスサイズ表示の更新
            //frmStatus.UpdateMatrixSize
            frmCTMenu.Instance.UpdateMatrixSize();			//v15.0変更 by 間々田 2009/02/27

            //frmStatusの校正ステータスを更新する    'v11.2追加 by 間々田 2006/01/19
            frmStatus.Instance.UpdateCorrectStatus();


            //追加2014/10/07hata_v19.51反映
            //スキャンコントロール画面にも反映する   'v18.00追加 byやまおか 2011/07/05 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            frmScanControl.Instance.optScanMode[CTSettings.scansel.Data.scan_mode].Checked = true;

            ErrorHandler:

            //スキャノ条件用フォームをアンロード
            
            if (!myNoClose)
            {
                this.Close();
            }
            else
            {
                CallClosing();
            }
        }
        //********************************************************************************
        //機    能  ：  スキャノ条件の設定内容をチェック（その１）
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //********************************************************************************
        private bool OptValueScanoChk1()
        {
            //返り値の初期化
            bool functionReturnValue = false;

            //Rev21.00
            //未定

            functionReturnValue = true;
            return functionReturnValue;
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
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //*******************************************************************************
        private void GetControls(ref CTstr.SCANSEL theScansel)
        {
            //最新 scancel（コモン）取得
            //modScansel.GetScansel(ref theScansel);
            ScanSel scansel = new ScanSel();
            scansel.Data.Initialize();
            scansel.Load();
            theScansel = scansel.Data;

            //マトリクスサイズ：2(512x512),3(1024x1024)
            if (fraScanoMatrix.Visible)
            {
                theScansel.matrix_size = modLibrary.GetOption(optScanoMatrix);
            }

            //画像積算枚数
            if (cwneScanoInteg.Visible)
            {
                //2014/11/07hata キャストの修正
                //theScansel.scan_integ_number = (int)cwneInteg.Value;
                theScansel.mscano_integ_number = Convert.ToInt32(cwneScanoInteg.Value);
            }

            //スライス厚
            theScansel.mscano_width = (float)cwneScanoWidth.Value;
            theScansel.max_slice_wid = Convert.ToSingle(cwneScanoWidth.Maximum.ToString(string.Format("F{0}", cwneScanoWidth.DecimalPlaces)));		//最大スライス厚
            theScansel.min_slice_wid = Convert.ToSingle(cwneScanoWidth.Minimum.ToString(string.Format("F{0}", cwneScanoWidth.DecimalPlaces)));		//最小スライス厚

            //バイアス
            theScansel.mscano_bias = (float)ntbScanoBias.Value;

            //スロープ
            theScansel.mscano_slope = (float)ntbScanoSlope.Value;

            //スキャノポイント
            long tmp = 0;
            float udPos = 0;
            udPos = (float)frmMechaControl.Instance.ntbUpDown.Value;
            //加速して現在位置も含めないので+1でOK
            tmp = (long)((cwneScanoDataRange.Value - (decimal)udPos) / cwneScanoSlicePitch.Value) + 1;
            theScansel.mscanopt = (int)tmp;
            tmp = (long)((cwneScanoDataRange.Value - (decimal)udPos) / (decimal)theScansel.min_slice_wid) + 1;
            theScansel.mscano_real_mscanopt = (int)tmp;

            //Rev22.00 mscano_area計算の前に移動 by長野 2015/08/28
            //スキャノデータピッチ
            //theScansel.mscano_mdtpitch = CTSettings.scancondpar.Data.detector;
            //Rev22.00 変更 by長野 2015/08/28
            //theScansel.mscano_mdtpitch = CTSettings.scancondpar.Data.detector_pitch[0];
            //Rev23.10 変更 by長野 2015/11/04
            int iino = modSeqComm.GetIINo();
            switch (iino)
            {
                case 0:
                case 1:
                case 2:
                    //II視野ごとの固定値を使う（コモンから取得する）
                    //mdtpitch = CTSettings.scancondpar.Data.detector_pitch[iino];
                    //Rev23.10 変更 by長野 2015/11/04
                    theScansel.mscano_mdtpitch = CTSettings.scancondpar.Data.detector_pitch[ScanCorrect.GFlg_MultiTube + iino * 3];
                    break;
                default:
                    theScansel.mscano_mdtpitch = 1;
                    break;
            }

            //Rev21.00 追加 by長野 2015/02/25 2015/02/26
            theScansel.mscano_area = CTSettings.scansel.Data.mscano_mdtpitch * CTSettings.scancondpar.Data.fimage_hsize * FCDFIDRate;

            ////Rev22.00 mscano_area計算の前に移動 by長野 2015/08/28
            ////スキャノデータピッチ
            //theScansel.mscano_mdtpitch = CTSettings.scancondpar.Data.mdtpitch[2];

            //スキャノピッチ
            theScansel.mscano_udpitch = (float)cwneScanoSlicePitch.Value;

            //FID
            theScansel.fid = FIDWithOffset;

            //FCD
            theScansel.fcd = FCDWithOffset;

            //X線管：0(130kV),1(225kV)
            theScansel.multi_tube = 0;

            //ビニング：0(1x1),1(2x2),2(4x4)
            theScansel.binning = 0;

            //スキャノ終了昇降位置
            theScansel.mscano_data_endpos = (float)cwneScanoDataRange.Value;

        }
        //*******************************************************************************
        //機　　能： スキャノ厚（画素）変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //*******************************************************************************
        private void cwneSlicePix_ValueChanged(object sender, EventArgs e)
        {
            if (EvntLock) return;   //Load時のイベントロック 追加2014/06/23(検S1)hata

            float r = 0;
            r = SWMin;

            string cwneScanoWidthFormat = string.Format("F{0}", cwneScanoWidth.DecimalPlaces);

            decimal value = 0;
            decimal.TryParse((cwneScanoWidthPix.Value * (decimal)r).ToString(cwneScanoWidthFormat), out value);

            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneSlice.Value = value;
            if ((cwneScanoWidth.Maximum >= value) && (cwneScanoWidth.Minimum <= value))
            {
                //Rev20.00 コーンの場合は、自動で変更しない by長野 2015/01/26
                //Rev20.00 元に戻す by長野 2015/02/05
                //if (!IsCone)
                //{
                //代入時のイベントロック
                //SliceEvntLock = true;
                cwneScanoWidth.Value = value;
                //SliceEvntLock = false;
                //}
            }
            //追加2015/01/29hata
            else if (cwneScanoWidth.Maximum < value)
            {
                cwneScanoWidth.Value = cwneScanoWidth.Maximum;
            }
            else if (cwneScanoWidth.Minimum > value)
            {
                cwneScanoWidth.Value = cwneScanoWidth.Minimum;
            }

            //変更した場合，ＯＫボタンを使用可にする
            if (this.ActiveControl == cwneScanoWidth) cmdOK.Enabled = true;
        }
        //*******************************************************************************
        //機　　能： スキャノピッチ（画素）変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //*******************************************************************************
        private void cwneScanoSlicePitchPix_ValueChanged(object sender, EventArgs e)
        {
            if (EvntLock) return;   //Load時のイベントロック 追加2014/06/23(検S1)hata

            float r = 0;
            r = SWMin;

            string cwneScanoSlicePitchFormat = string.Format("F{0}", cwneScanoSlicePitch.DecimalPlaces);

            decimal value = 0;
            decimal.TryParse((cwneScanoSlicePitchPix.Value * (decimal)r).ToString(cwneScanoSlicePitchFormat), out value);

            //Min/Maxの範囲チェックと代入時のイベントロック追加 追加2014/06/23(検S1)hata
            //cwneSlice.Value = value;
            //if ((cwneScanoWidth.Maximum >= value) && (cwneScanoWidth.Minimum <= value))
            //Rev21.00 修正 by長野 2015/03/16
            if ((cwneScanoSlicePitch.Maximum >= value) && (cwneScanoSlicePitch.Minimum <= value))
            {
                //Rev20.00 コーンの場合は、自動で変更しない by長野 2015/01/26
                //Rev20.00 元に戻す by長野 2015/02/05
                //if (!IsCone)
                //{
                //代入時のイベントロック
                //SliceEvntLock = true;
                cwneScanoSlicePitch.Value = value;
                //SliceEvntLock = false;
                //}
            }
            //追加2015/01/29hata
            else if (cwneScanoSlicePitch.Maximum < value)
            {
                cwneScanoSlicePitch.Value = cwneScanoSlicePitch.Maximum;
            }
            else if (cwneScanoWidth.Minimum > value)
            {
                cwneScanoSlicePitch.Value = cwneScanoSlicePitch.Minimum;
            }

            //Rev22.00 Rev21.01の反映 by長野 2015/07/31
            changeIntegByUdSpeed();

            //変更した場合，ＯＫボタンを使用可にする
            if (this.ActiveControl == cwneScanoSlicePitch) cmdOK.Enabled = true;
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
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //*************************************************************************************************
        private void frmScanoCondition_FormClosed(object sender, FormClosedEventArgs e)
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
            modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTScanoCondition);

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
                modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTScanoCondition);

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
        //*******************************************************************************
        //機　　能： バイアス変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //*******************************************************************************
        private void ntbScanoBias_ValueChanged(object sender, EventArgs e)		// 【C#コントロールで代用】
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
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //*******************************************************************************
        private void ntbScanoSlope_ValueChanged(object sender, EventArgs e)		// 【C#コントロールで代用】
        {
            //変更した場合，ＯＫボタンを使用可にする
            if (this.Visible) cmdOK.Enabled = true;
        }
        //*******************************************************************************
        //機　　能： デー収集範囲変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //*******************************************************************************
        private void cwneScanoDataRange_ValueChanged(object sender, EventArgs e)
        {
            //Rev26.40 修正 コントロールのincrementを0.002に変更したのでコメントアウト by chouno 2019/03/25 --->
            //long tmp = 0;
            //decimal tmp2 = 0;
            //tmp2 = cwneScanoDataRange.Value;
            ////四捨五入してIncrementの単位にする
            //tmp2 = Math.Round(tmp2 / (decimal)0.001, MidpointRounding.AwayFromZero) * (decimal)0.001;

            //tmp = (long)(tmp2 * (decimal)1000);

            //if (tmp % 2 != 0)
            //{
            //    if (tmp2 + (decimal)0.001 <= cwneScanoDataRange.Maximum)
            //    {
            //        cwneScanoDataRange.Value += (decimal)0.001;
            //    }
            //    else if (tmp2 - (decimal)0.001 >= cwneScanoDataRange.Minimum)
            //    {
            //        cwneScanoDataRange.Value -= (decimal)0.001;
            //    }
            //}
            //<---

            //Rev22.00 Rev21.01の反映 by長野 2015/07/31
            changeIntegByUdSpeed();
           
            //変更した場合，ＯＫボタンを使用可にする
            if (this.Visible) cmdOK.Enabled = true;
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
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //*******************************************************************************
        private void cwneScanoInteg_ValueChanged(object sender, EventArgs e)
        {   
            //Rev22.00 Rev21.01の反映 by長野 2015/07/31
            changeIntegByUdSpeed();

            //変更した場合，ＯＫボタンを使用可にする
            if (this.ActiveControl == cwneScanoInteg) cmdOK.Enabled = true;
        }
        //*******************************************************************************
        //機　　能： スキャノ条件を設定する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： IsConebeam      [I/ ] Boolean   コーンビーム？
        //           IsVisible       [I/ ] Boolean   表示する？
        //           K               [I/ ] Long      コーンビーム枚数
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V21.00  15/02/15   (検S1)長野      新規作成
        //*******************************************************************************
        public void Setup(bool IsVisible = false, bool? NoClose = null)
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
                //frmScanCondition.Instance.WindowState = FormWindowState.Normal;
                //Rev22.00 修正 by長野 2015/07/31
                frmScanoCondition.Instance.WindowState = FormWindowState.Normal;


            }
            else
            {
                //NoVisibleEvntLock = true;
                //EvntLock = true;
                //frmScanCondition.Instance.TopMost = false;
                //frmScanCondition.Instance.WindowState = FormWindowState.Minimized;
                //Rev22.00 修正 by長野 2015/07/31
                frmScanoCondition.Instance.TopMost = false;
                frmScanoCondition.Instance.WindowState = FormWindowState.Minimized;
            }

            //'フォームのロード
            //Load Me
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
                frmScanoCondition_Load(this, EventArgs.Empty);
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
                SetScanoCondition();     //V19.20 ＯＫボタンクリック処理と別にする by Inaba 2012/11/01 'v19.50 統合 v19.41との統合 by長野 2013/11/17

                //frmScanConditionは閉じないようにする
                //if (modLibrary.IsExistForm("frmScanCondition")) this.Close();  //追加 2009/08/24
            }

            //コーンビームスキャンかつ連続回転モードの場合 'v16.2 連続回転コーンビーム時対応 by 山影 2010/01/19
            //UpdateIntegMin
            myNoClose = false;

        }
        //追加2015/01/20hata
        //データを再表示する
        private void NumicValue_Leave(object sender, EventArgs e)
        {
            if (sender as NumericUpDown == null) return;

            NumericUpDown udbtn = (NumericUpDown)sender;

            string sval = udbtn.Value.ToString();
            //変更2015/01/28hata
            if (string.IsNullOrEmpty(sval))
            {
                udbtn.Value = udbtn.Minimum;
                return;
            }
            udbtn.Text = sval;
        }
        //********************************************************************************
        //機    能  ：  最大・最小昇降速度の読み出し
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：                   [ /O] Boolean   結果 True  : ｽｷｬﾝを続行
        //                                                    False : ｽｷｬﾝを中止
        //補    足  ：  校正ステータスが準備未完了なら、原因となる校正名を表示する。
        //
        //履    歴  ：  V21.01   15/07/31  (検S1)長野       新規作成
        //********************************************************************************
        private void LoadUdMaxMinSpeed() //Rev21.00 追加 by長野 2015/03/06
        {
            //速度のチェック
            //Rev21.00 空振り用のテーブルを作る。

            string buf = null;
            string[] strCell = null;

            //ファイルオープン
            StreamReader file = null;
            try
            {
                //変更2015/01/22hata
                //file = new StreamReader(FileName);
                file = new StreamReader(@"c:\ct\mechadata\boardpara.csv", Encoding.GetEncoding("shift-jis"));

                //while (!FileSystem.EOF(fileNo))
                while ((buf = file.ReadLine()) != null)
                {
                    //１行読み込む
                    if (!string.IsNullOrEmpty(buf))
                    {
                        //カンマで区切って配列に格納
                        strCell = buf.Split(',');

                        //コメントか？
                        if (strCell[0].Trim() == "UdStep")
                        {
                            //先頭列の文字が数字なら情報を取り出す
                            double IsNumeric = 0;
                            if (double.TryParse(strCell[1], out IsNumeric))
                            {
                                myUdStep = IsNumeric;
                            }
                        }
                        //コメントか？
                        if (strCell[0].Trim() == "UdMmsFL")
                        {
                            //先頭列の文字が数字なら情報を取り出す
                            double IsNumeric = 0;
                            if (double.TryParse(strCell[1], out IsNumeric))
                            {
                                myUdMinSpeed = IsNumeric;
                            }
                        }
                        //コメントか？
                        if (strCell[0].Trim() == "UdManualLimitMms")
                        {
                            //先頭列の文字が数字なら情報を取り出す
                            double IsNumeric = 0;
                            if (double.TryParse(strCell[1], out IsNumeric))
                           {
                                myUdMaxSpeed = IsNumeric;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                //ファイルクローズ
                if (file != null)
                {
                    file.Close();
                    file = null;
                }
            }
        }
        //********************************************************************************
        //機    能  ：  昇降速度が設定範囲を超えたら積算枚数を調整して合わせる
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：                   [ /O] なし
        //                                                   
        //補    足  ：  
        //
        //履    歴  ：  V21.01   15/07/31  (検S1)長野       新規作成
        //********************************************************************************
        public void changeIntegByUdSpeed() //Rev21.00 追加 by長野 2015/03/06
        {
            if (EvntLock == false)
            {
                //調整後の積算枚数
                double tmpAdjIntegNum = 0;
                int AdjIntegNum = 0;

                //現在の画面に対するscanselを取得
                //modScansel.scanselType theScansel = default(modScansel.scanselType);
                CTstr.SCANSEL theScansel = new CTstr.SCANSEL();// = default(CTstr.SCANSEL);
                theScansel.Initialize();

                //この画面で設定しているスキャン条件を取得
                GetControls(ref theScansel);

                frmScanControl frmScanControl = frmScanControl.Instance;

                //速度のチェック
                //Rev21.00 空振り用のテーブルを作る。

                double FrameRate = 0.0;
                //v19.10 PKEとその他の検出器でframerate取得方法を変える 2012/09/06 by長野
                if (CTSettings.scancondpar.Data.detector == 2)
                {
                    //Rev20.00 変更 by長野 2015/02/06
                    FrameRate = (double)1.0 / (modCT30K.fpd_integlist[frmScanControl.cmbInteg.SelectedIndex] / (double)1000.0);
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
                float tmpL = 0;
                float tmpSec = 0;
                tmpSec = theScansel.mscanopt * theScansel.mscano_integ_number / (float)FrameRate;
                //tmpL = mscanopt * mscano_width;
                tmpL = (theScansel.mscano_real_mscanopt - 1) * theScansel.min_slice_wid;

                if (tmpSec > 0 && tmpL > 0)
                {
                    float UdSpeed = (float)(tmpL / tmpSec);
                    if (UdSpeed > myUdMaxSpeed)
                    {
                        tmpAdjIntegNum = (((double)(theScansel.mscano_real_mscanopt - 1) * theScansel.min_slice_wid * FrameRate) / (myUdMaxSpeed * (double)theScansel.mscanopt));
                        AdjIntegNum = (int)Math.Round(tmpAdjIntegNum + 0.5, MidpointRounding.AwayFromZero);
                        if (cwneScanoInteg.Maximum >= AdjIntegNum && cwneScanoInteg.Minimum <= AdjIntegNum)
                        {
                            cwneScanoInteg.Value = (decimal)AdjIntegNum;
                        }
                        else
                        {
                            cwneScanoInteg.Value = 1;
                        }
                    }
                    else if (UdSpeed < myUdMinSpeed)
                    {
                        tmpAdjIntegNum = (((double)(theScansel.mscano_real_mscanopt - 1) * theScansel.min_slice_wid * FrameRate) / (myUdMinSpeed * (double)theScansel.mscanopt));
                        AdjIntegNum = (int)tmpAdjIntegNum;
                        if (cwneScanoInteg.Maximum >= AdjIntegNum && cwneScanoInteg.Minimum <= AdjIntegNum)
                        {
                            cwneScanoInteg.Value = (decimal)AdjIntegNum;
                        }
                        else
                        {
                            cwneScanoInteg.Value = 1;
                        }
                    }
                }
            }
        }
    }
}
