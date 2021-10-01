using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using Common.IO;

using System.Resources;
//using Microsoft.VisualBasic;
using System.Collections;
using System.Diagnostics;
using System.IO;

using CT30K.Common;
using CTAPI;
//using TransImage;

//using CT30K.Forms;
//using CT30K.Properties;
//using CT30K.Controls;
using System.Runtime.InteropServices;


namespace CT30K
{
    public static class CTSettings
    {
        #region コモン
        /// <summary>
        /// コモン
        /// </summary>
        public static ScanInh scaninh;
        public static InfDef infdef;
        public static CTInfDef ctinfdef;
        public static T20kInf t20kinf;
        public static ScanCondPar scancondpar;
        public static WorkShopInf workshopinf;
        public static MechaPara mechapara;
        public static XTable xtable;
        public static HscPara hscpara;
        public static ScanSel scansel;
        public static DispInf dispinf;
        public static MecaInf mecainf;
        public static ReconInf reconinf;
        public static ImageInfo gImageinfo;
        public static zoomtbl zoomtbl;
        public static sp2inf sp2inf;
        public static pdplan pdplan;
        public static discharge_protect discharge_protect;  
        
        //追加2014/10/07hata_v19.51反映
        public static int bakIntegNum;    //'何枚の積分枚数でキャプチャしたか記憶させるための変数 by長野 v19.51 2014/03/19

        #endregion

        // 透視検出器パラメータ
        //public static DetectorParam detectorParam;
        // スキャンパラメータ
        //public static ScanParam scanParam;

        //ini読込み            
        public static IniValue iniValue;

        //撮影コントロール            
        //public static TransImageControl transImageControl;


        //ImageProテキスト描画オブジェクト   'v15.0追加 by 間々田 2009/01/19
        //public static clsImagePro IPOBJ = new clsImagePro();



        #region ＣＴ起動時に値が決定される変数（その後不変）
        /// <summary>
        /// ﾋﾞｭｰ数最小
        /// </summary>
        public static int GVal_ViewMin;

        /// <summary>
        /// ﾋﾞｭｰ数最大
        /// </summary>
        public static int GVal_ViewMax;

        /// <summary>
        /// 画像積算枚数最小
        /// </summary>
        public static int GValIntegNumMin;

        /// <summary>
        /// 画像積算枚数最大
        /// </summary>
        public static int GValIntegNumMax;

        /// <summary>
        /// I.I もしくは FPD
        /// </summary>
        public static string GStrIIOrFPD;

        /// <summary>
        /// テーブル移動 もしくは Ｘ線管移動
        /// </summary>
        public static string GStrTableOrXray;

        /// <summary>
        /// ﾃｰﾌﾞﾙがX線管と干渉する限界FCD ※自動校正用
        /// </summary>
        public static float GVal_FcdLimit;

        /// <summary>
        /// FDD側の限界値(FDD-FCD
        /// </summary>
        public static float Gval_FcdFddLimit;   //v19.51 追加 by長野 FDD側の限界値(FDD-FCD) by長野 2014/03/05


        /// <summary>
        /// 昇降上限値(mm)
        /// </summary>
        public static float GValUpperLimit;

        /// <summary>
        /// 昇降下限値(mm)
        /// </summary>
        public static float GValLowerLimit;

        ///// <summary>
        ///// Ｘ線検出器がフラットパネル場合:True            'v7.0追加 by 間々田 2003/09/08
        ///// </summary>
        //public static bool Use_FlatPanel;

        /// <summary>
        /// FCD/FDD の文字列                               'V9.6 append by 間々田 2004/10/13
        /// </summary>
        public static string gStrFidOrFdd;

        #region //v29.99 分散処理 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        /*
        /// <summary>
        /// コーン分散処理用ＰＣ２（True:可, False:不可    'v10.0追加 by 間々田 2005/01/24
        /// </summary>
        public static bool UsePCWS2;

        /// <summary>
        /// コーン分散処理用ＰＣ３（True:可, False:不可）  'v10.0追加 by 間々田 2005/01/24
        /// </summary>
        public static bool UsePCWS3;

        /// <summary>
        /// コーン分散処理用ＰＣ４（True:可, False:不可）  'v10.0追加 by 間々田 2005/01/24
        /// </summary>
        public static bool UsePCWS4;
*/
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
        #endregion

        /// <summary>
        /// 高速度透視撮影 (True:有，False:無)             'v16.01 追加 by 山影 10-02-18
        /// </summary>
        public static bool HscOn;
         
        /// <summary>
        /// 検出器切替機能 (True:有, False:無)             'v17.20 追加 by 長野 10-08-31
        /// </summary>
        public static bool SecondDetOn;

        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //public static bool  UseRamDisk           As Boolean  //RAMディスクの有無（True:有，False:無）         'v17.40追加 byやまおか 2010/10/26
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


        ////FPDの全画素データを使う (True:使う, False:使わない)    'v17.22追加 byやまおか 2010/10/19
        //public static bool Use_FpdAllpix;

        ////透視画像左右反転フラグ(FPDの場合だけ有効)      'v17.50 modAutoPosから移動 byやまおか 2011/20/20
        //public static bool IsLRInverse;

        /// <summary>
        /// 軸名称
        /// </summary>
        public static string[] AxisName = new string[2];

        ////0:8bit(256) 1:10bit(1024) 2:12bit(4096)    '追加 by 間々田 2005/01/07
        //public static int FimageBitIndex;

        //共有メモリのハンドル
        public static IntPtr hComMap = IntPtr.Zero;

        //検出器シフトスキャン（True:有，False:無）      'v18.00追加 byやまおか 2011/01/31 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        public static bool DetShiftOn;
        
        #endregion

        #region 静的コンストラクタ
        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static CTSettings()
        {

            //iniファイル初期化
            iniValue = new IniValue();

            // 透視検出器パラメータ初期化
            //detectorParam = new DetectorParam();

            // スキャンパラメータ初期化
            //scanParam = new ScanParam();

            // 撮影コントロール初期化
            //transImageControl = new TransImageControl(detectorParam);
            //transImageControl.SetScanParam(scanParam);

        }
        #endregion


        /// <summary>
        /// データ読込み
        /// </summary>
        /// <returns></returns>
        public static bool Load()
        {

            // コモン取得
            modCT30K.GetCommon();


            //#if DEBUG
            //            // scaninh の設定を一時的に変更するためのダイアログ（デバッグ時のみ表示）
            //            frmConfig.Instance.ShowDialog();
            //#endif

            // ＣＴ起動時に値が決定される変数（その後不変）をコモンから取得
            SetFixedVariable();

            //コモンからScansel情報を取得し、グローバル変数に記憶させる
            //GetMyScansel();

            ////グローバル変数の初期化
            //InitGlobalVariables();


            return true;
        }

        //#region コモンを取得
        ///// <summary>
        ///// コモンを取得
        ///// </summary>
        //private static void GetCommon()
        //{


        //    //scaninh(コモン)の取得
        //    scaninh = new ScanInh();
        //    if (!scaninh.Load())
        //        MessageBox.Show("ScanInh読み込み失敗",
        //                            Application.ProductName,
        //                            MessageBoxButtons.OK,
        //                            MessageBoxIcon.Error);

        //    //infdef(コモン)の取得
        //    //vb6(setMyCtinfdef)も同時に取得
        //    //軸名称の取得
        //    infdef = new InfDef();
        //    infdef.Data.Initialize();
        //    if (!infdef.Load())
        //        MessageBox.Show("InfDef読み込み失敗",
        //                            Application.ProductName,
        //                            MessageBoxButtons.OK,
        //                            MessageBoxIcon.Error);

        //    //ctinfdef(コモン)の取得
        //    //vb6(setMyCtinfdef)も同時に取得
        //    ctinfdef = new CTInfDef();
        //    ctinfdef.Data.Initialize();
        //    if (!ctinfdef.Load())
        //        MessageBox.Show("CTInfDef読み込み失敗",
        //                            Application.ProductName,
        //                            MessageBoxButtons.OK,
        //                            MessageBoxIcon.Error);

        //    //t20kinf(コモン)の取得
        //    t20kinf = new T20kInf();
        //    t20kinf.Data.Initialize();
        //    if (!t20kinf.Load())
        //        MessageBox.Show("T20kInf読み込み失敗",
        //                            Application.ProductName,
        //                            MessageBoxButtons.OK,
        //                            MessageBoxIcon.Error);

        //    //scancondpar(コモン)の取得
        //    scancondpar = new ScanCondPar();
        //    scancondpar.Data.Initialize();
        //    if (!scancondpar.Load(scaninh.Data.rotate_select))
        //        MessageBox.Show("ScanCondPar読み込み失敗",
        //                            Application.ProductName,
        //                            MessageBoxButtons.OK,
        //                            MessageBoxIcon.Error);

        //    //workshopinf(コモン)の取得
        //    workshopinf = new WorkShopInf();
        //    workshopinf.Data.Initialize();
        //    if (!workshopinf.Load())
        //        MessageBox.Show("WorkShopInf読み込み失敗",
        //                            Application.ProductName,
        //                            MessageBoxButtons.OK,
        //                            MessageBoxIcon.Error);

        //    //mechapara(コモン)の取得
        //    mechapara = new MechaPara();
        //    mechapara.Data.Initialize();
        //    if (!mechapara.Load())
        //        MessageBox.Show("MechaPara読み込み失敗",
        //                            Application.ProductName,
        //                            MessageBoxButtons.OK,
        //                            MessageBoxIcon.Error);

        //    //xtable(コモン)の取得
        //    xtable = new XTable();
        //    xtable.Data.Initialize();
        //    if (!xtable.Load())
        //        MessageBox.Show("XTable読み込み失敗",
        //                            Application.ProductName,
        //                            MessageBoxButtons.OK,
        //                            MessageBoxIcon.Error);

        //    //hscpara(コモン)の取得
        //    hscpara = new HscPara();
        //    if (!hscpara.Load())
        //        MessageBox.Show(" HscPara読み込み失敗",
        //                            Application.ProductName,
        //                            MessageBoxButtons.OK,
        //                            MessageBoxIcon.Error);




        //    //dispinf(コモン)の取得
        //    dispinf = new DispInf();
        //    dispinf.Data.Initialize();
        //    if (!dispinf.Load())
        //        MessageBox.Show("DispInf読み込み失敗",
        //                            Application.ProductName,
        //                            MessageBoxButtons.OK,
        //                            MessageBoxIcon.Error);

        //    //mecainf(コモン)の取得
        //    mecainf = new MecaInf();
        //    mecainf.Data.Initialize();
        //    if (!mecainf.Load())
        //        MessageBox.Show("MecaInf読み込み失敗",
        //                            Application.ProductName,
        //                            MessageBoxButtons.OK,
        //                            MessageBoxIcon.Error);


        //    //
        //    //一応ここで読んでおく
        //    //
        //    //reconinf(コモン)の取得
        //    reconinf = new ReconInf();
        //    reconinf.Data.Initialize();

        //    //imageinfo(コモン)の取得
        //    gImageinfo = new ImageInfo();
        //    gImageinfo.Data.Initialize();


        //}


        ////コモンからScansel情報を取得し、グローバル変数に記憶させる
        //public static void GetMyScansel()
        //{

        //    //scansel(コモン)の取得
        //    if (scansel == null)
        //    {
        //        scansel = new ScanSel();
        //        scansel.Data.Initialize();
        //    }
        //    if (!scansel.Load(CTSettings.scaninh.Data))
        //        MessageBox.Show("ScanSel読み込み失敗",
        //                            Application.ProductName,
        //                            MessageBoxButtons.OK,
        //                            MessageBoxIcon.Error);

        //}

        //#endregion


        #region 共有メモリの作成と破棄
        /// <summary>
        /// 共有メモリの作成
        /// </summary>
        /// <returns></returns>
        public static int InitializeSharedCTCommon()
        {
            //共有メモリの作成
            hComMap = ComLib.CreateSharedCTCommon();
            if (hComMap == IntPtr.Zero)
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("共有メモリ作成失敗",
                //                    Application.ProductName,
                //                    MessageBoxButtons.OK,
                //                    MessageBoxIcon.Error);
                return 1;

            }
            //共有メモリのセット
            if (ComLib.SetSharedCTCommon() != 0)
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("共有メモリセット失敗",
                //                    Application.ProductName,
                //                    MessageBoxButtons.OK,
                //                    MessageBoxIcon.Error);

                return 2;
            }

            return 0;
        }

        //コモンファイルを共有メモリ解放
        /// <summary>
        /// 共有メモリの解放
        /// </summary>
        public static void ExitSharedCTCommon()
        {
            //共有メモリを解放
            if (CTSettings.hComMap != IntPtr.Zero)
            {
                ComLib.SaveSharedCTCommon();
                ComLib.DestroySharedCTCommon(CTSettings.hComMap);
                CTSettings.hComMap = IntPtr.Zero;
            }

        }
        #endregion


        #region ＣＴ起動時に値が決定される変数（その後不変）をコモンから取得
        /// <summary>
        /// ＣＴ起動時に値が決定される変数（その後不変）をコモンから取得
        /// </summary>
        public static void SetFixedVariable()
        {

            //#region　v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            ///*
            //// コーン分散処理用ＰＣ２
            //UsePCWS2 = (Scaninh.pcws2 == 0);

            //// コーン分散処理用ＰＣ３
            //UsePCWS3 = (Scaninh.pcws3 == 0);

            //// コーン分散処理用ＰＣ４
            //UsePCWS4 = (Scaninh.pcws4 == 0);
            //*/
            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            //#endregion

            //// 高速度透視撮影機能     'v16.01 追加 by 山影 10-02-18
            //HscOn = (scaninh.Data.high_speed_camera == 0);

            //// 検出器切替機能(高速透視優先) v17.20 by 長野 10-08-31
            //SecondDetOn = (scaninh.Data.second_detector == 0) & !HscOn;

            ////FPDの全画素データを使う    'v17.22追加 byやまおか 2010/10/19
            ////CTSettings.detectorParam.Use_FpdAllpix = (scaninh.Data.fpd_allpix == 0);

            //#region　v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            ////UseRamDisk = (scaninh.Data..ramdisk == 0)     'v17.40追加 byやまおか 2010/10/26
            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            //#endregion


            //DetShiftOn = (CTSettings.scaninh.Data.scan_mode[3] == 0);            //シフト(オフセット)スキャン 'v18.00追加 byやまおか 2011/01/31 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

            //#region　DetectorParamで設定している。ここでは不要
            ////→DetectorParamで設定している。ここでは不要
            ///*
            //// Ｘ線検出器の種類：　０：Ｘ線I.I.　１：フラットパネル
            //Use_FlatPanel = (scancondpar.Data.detector == 1) || (scancondpar.Data.detector == 2);
            //*/
            //#endregion

            //// 0:I.I.  1:浜ホトFPD  2:PkeFPD  'v16.20/v17.00追加 byやまおか 2010/01/19
            ////detectorParam.DetType = (DetectorConstants)scancondpar.Data.detector;

            //// v17.20 検出器の種類にかかわらず、FDDで統一する by 長野 2010/09/20
            //gStrFidOrFdd = "FDD";

            //// ﾃｰﾌﾞﾙがX線管と干渉する限界FCD
            //GVal_FcdLimit = scancondpar.Data.fcd_limit;

            //Gval_FcdFddLimit = 510; //追加2014/10/07hata_v19.51反映 

            ////FPD(X線からみた画像)でscaninhibitの設定が有効な場合に透視を反転させる    'v17.50変更 byやまおか 2011/02/02
            ////このフラグが有効な場合はI.I.と同じように検出器側から見た透視になる
            ////detectorParam.IsLRInverse = detectorParam.Use_FlatPanel & Convert.ToBoolean(scaninh.Data.transdisp_lr_inv == 0);
			
            //#region　サイズの設定　ここではやらない
            ////ここではやらない
            ////detectorParam.h_size = scancondpar.Data.h_size;
            ////detectorParam.v_size = scancondpar.Data.v_size;
            //#endregion

            //// キャプチャタイプ(0:None 1:FG 2:FGX 3:FGE)
            ////CTSettings.detectorParam.v_capture_type = t20kinf.Data.v_capture_type;

            //// ビュー数最小
            //GVal_ViewMin = Convert.ToInt32(infdef.Data.min_view.GetString());

            //// ビュー数最大
            //GVal_ViewMax = Math.Max(Convert.ToInt32(infdef.Data.max_view.GetString()), GVal_ViewMin);

            //// 画像積算枚数最小
            //GValIntegNumMin = Math.Max(Convert.ToInt32(infdef.Data.min_integ_number.GetString()), 1);

            ////画像積算枚数最大
            //GValIntegNumMax = Math.Max(Convert.ToInt32(infdef.Data.max_integ_number.GetString()), GValIntegNumMin);

            ////I.I もしくは FPD の文字列
            ////GStrIIOrFPD = Convert.ToString(infdef.Data.detector[(detectorParam.Use_FlatPanel ? 1 : 0)].GetString());

            ////テーブル移動 もしくは Ｘ線管移動
            ////GStrTableOrXray = Convert.ToString(infdef.Data.table_y[(scaninh.Data.table_y == 1 ? 1 : 0)]);
            //////GStrTableOrXray = RemoveNull(.table_y(IIf(scaninh.table_y = 1, 1, 0)))
            ////v17.60 ストリングテーブル化 by長野 2011/05/25
            ////v29.99 今のところX線管移動は不要のため変更 by長野 2013/04/08'''''ここから'''''
            ////GStrTableOrXray = IIf(scaninh.table_y = 1, LoadResString(20174), LoadResString(20126))
            ////GStrTableOrXray = infdef.Data.table_y[(scaninh.Data.table_y == 1 ? 1 : 0)].GetString();
            //GStrTableOrXray = CTResources.LoadResString(20126);
            ////v29.99 今のところX線管移動は不要のため変更 by長野 2013/04/08'''''ここまで'''''


            ////昇降上限値(mm)：×100の値を格納（実際には mechapara.csv の d8_1b の値）
            ////2014/11/13hata キャストの修正
            ////GValUpperLimit = t20kinf.Data.upper_limit / 100;
            //GValUpperLimit = Convert.ToInt32(t20kinf.Data.upper_limit / 100F);

            ////昇降下限値(mm)：×100の値を格納（実際には mechapara.csv の d8_1c の値）
            ////2014/11/13hata キャストの修正
            ////GValLowerLimit = t20kinf.Data.lower_limit / 100;
            //GValLowerLimit = Convert.ToInt32(t20kinf.Data.lower_limit / 100F);

            ////Ｘ線制御器のタイプ取得：0(FEINFOCUS),1(KEVEX),2(浜ホト)
            ////modXrayControl.XrayType = (modXrayControl.XrayTypeConstants)Convert.ToInt32(t20kinf.Data.system_type.GetString());

        }
        #endregion


        //modCT30Kに戻す
        ////*******************************************************************************
        ////機　　能： グローバル変数の初期化
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： V9.7  05/01/12 (SI4)間々田     新規作成
        ////*******************************************************************************
        //public static void InitGlobalVariables()
        //{

        //    //透視画像のビット数（scancondpar.fimage_bit）によって透視画像表示用のウィンドウレベル／ウィンドウ幅のデフォルト値を設定
        //    modCT30K.FimageBitIndex = scancondpar.Data.fimage_bit;

        //    //スキャン位置校正時のデフォルトのモード（True:自動, False:手動）'v11.2追加 by 間々田 2006/01/10
        //    ScanCorrect.IsAutoAtSpCorrect = (scaninh.Data.full_distortion == 0);
        //    //フル２次元：自動   １次元：手動

        //    //前回のksw                  'v11.2追加 by 間々田 2006/01/12
        //    modCT30K.LastKsw = 0;

        //    //自動スキャン位置校正用I.I.移動フラグリセット   v11.21追加 by 間々田 2006/02/10
        //    modScanCorrectNew.IIMovedAtAutoSpCorrect = false;

        //    //v11.3追加ここから by 間々田 2006/02/20 frmCTMenu.MDIForm_Load から移動

        //    //校正処理の初期値設定
        //    ScanCorrect.IntegNumAtRot = 1;
        //    ScanCorrect.GVal_SlWid_Rot = 0.0f;            //added V2.0 by 鈴山
        //    ScanCorrect.GVal_SlPix_Rot = 20.0f;            //added V2.0 by 鈴山
        //    //GVal_DistVal = 10#
        //    ScanCorrect.GVal_DistVal[1] = 10.0f;            //v16.2/v17.0 配列に変更 2010/02/23 by Iwasawa
        //    ScanCorrect.GVal_DistVal[2] = 10.0f;            //v16.2/v17.0 配列に変更 2010/02/23 by Iwasawa
        //    ScanCorrect.GFlg_Shading_Rot = false;            //V4.0 append by 鈴山 2001/03/29

        //    //メカ準備の初期値設定                   'V4.0 append by 鈴山 2001/02/06

        //    //微調テーブル移動速度
        //    modMechaControl.GVal_FineTableSpeed = scancondpar.Data.ftable_max_speed * 0.5f;            //コモン値から最大値*0.5を設定　'V7.0 change by 間々田 2003/11/06

        //    //画像処理関連の初期化処理
        //    modImgProc.InitImgProc();

        //    //コーンビーム調整フラッグの初期値設定　 added by 山本　2002-8-31
        //    Maint.ConeAdjustFlg = 0;

        //    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //    //Viscom調整メンテナンスフラッグの初期値設定  added by 山本 2006-12-27
        //    //    ViscomMaintFlg = 0
        //    //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

        //    //校正関連の初期化
        //    modScanCorrect.InitScanCorrect();

        //    //v7.0 FPD対応 by 間々田 2003/09/24
        //    GetBinning();

        //    modCT30K.TableRotOn = (scansel.Data.table_rotation == 1);

        //    //強制ウォームアップ（東芝 EXM2-150用）                  'v11.4追加 by 間々田 2006/02/17
        //    modCT30K.gForcedWarmup = 0;            //0:短期 1:長期

        //    //Ｘ線ツール画面で使用するタイマーカウント               'v11.4追加 by 間々田 2006/03/10
        //    modXrayControl.XrayToolTimerCount = 20 * 60;

        //    //バックアップ用設定管電圧                               'v11.4追加 by 間々田 2006/03/10
        //    modXrayControl.BackXrayVoltSet = -1;

        //    //ウォームアップ後フィラメント調整を開始する場合のフラグ(Viscom専用)　'v11.5追加 by 間々田 2006/05/08
        //    modXrayControl.FilamentAdjustAfterWarmup = false;

        //    //Ｘ線ウォームアップ中断フラグ(Viscom専用)　'v11.5追加 by 間々田 2006/05/08
        //    modXrayControl.XrayWarmUpCanceled = false;

        //    //ウォームアップ中の最大出力管電圧をクリア   'v11.5追加 by 間々田 2006/05/16
        //    modXrayControl.XrayMaxFeedBackVolt = 0;

        //    //Ｘ線手動センタリングフラグ(Viscom専用)　'v11.5追加 by 間々田 2006/05/10
        //    modXrayControl.XrayCenteringManual = false;


        //    //透視画像処理関連の変数初期化   'v15.0追加 by 間々田 2009/01/09
        //    InitFluoroIP();

        //    //終了要求フラグクリア                    'v11.5追加 by 間々田 2006/06/23
        //    modCT30K.RequestExit = false;

        //    //前回の画像保存先を求めておく           'v15.0追加 by 間々田 2009/06/16
        //    modCT30K.DestOld = Path.Combine(scansel.Data.pro_code.GetString(), scansel.Data.pro_name.GetString());

        //    //回転中心位置のチェックフラグ v16.03/v16.20 追加 by 山影 10-03-12
        //    modCT30K.CheckRC = false;

        //    //非常停止ボタンが押されたかどうかのチェックフラグ 'v17.40 by 長野
        //    modCT30K.emergencyButton_Flg = false;

        //    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //    //高速再構成可、かつ、連続コーン可の時に連続回転コーン実行可能フラグを追加(Falseで初期化) 'v17.40 追加 by 長野 2010/10/21
        //    //if ((scaninh.Data.gpgpu == 0) && (scaninh.Data.smooth_rot_cone == 0))
        //    //{
        //    //    modCT30K.smooth_rot_cone_flg = false;     //'v17.42変更 常に初期化する byやまおか 2010/11/04
        //    //}
        //    //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

        //    //v19.11 ROI処理、ヒストグラム、骨塩定量解析のROI形状を初期化 2013/02/20
        //    DrawRoi.RoiCalRoiNo = 2;
        //    DrawRoi.HistRoiNo = 2;
        //    DrawRoi.BoneDensityRoiNo = 2;

        //}



        //modCT30Kに戻す
        //        //
        //        //   ビニングモード変更時処理            'v7.0 added by 間々田 2003/09/26
        //        //
        //        public static void GetBinning()
        //        {
        //            //ビニングモードを求める
        //            //binning = GetCommonLong("scansel", "binning")              'v11.4削除 by 間々田 2006/03/13

        //            //hm の取得
        //            //detectorParam.hm = _with2.h_mag(scansel.binning);     //v11.4変更 by 間々田 2006/03/13
        //            detectorParam.hm = scancondpar.Data.h_mag[scansel.Data.binning];

        //            //vm の取得
        //            //vm = _with2.v_mag(scansel.binning);       //v11.4変更 by 間々田 2006/03/13
        //            detectorParam.vm = scancondpar.Data.v_mag[scansel.Data.binning];

        //            //phm の取得
        //            //phm = _with2.pulsar_h_mag(scansel.binning);			//v11.4変更 by 間々田 2006/03/13
        //            detectorParam.phm = scancondpar.Data.pulsar_h_mag[scansel.Data.binning];


        //            //pvm の取得
        //            //pvm = _with2.pulsar_v_mag(scansel.binning);			//v11.4変更 by 間々田 2006/03/13
        //            detectorParam.pvm = scancondpar.Data.pulsar_v_mag[scansel.Data.binning];


        //#if (DebugOn)  //'v17.02追加 byやまおか 2010/07/08

        //            if ((scancondpar.Data.v_size / detectorParam.pvm >= 1024))
        //            {
        //                detectorParam.phm = detectorParam.phm * 2;
        //                detectorParam.pvm = detectorParam.pvm * 2;
        //            }
        //#endif

        //            //fphm の取得
        //            //fphm = _with2.fpulsar_h_mag(scansel.binning);			//v11.4変更 by 間々田 2006/03/13
        //            detectorParam.fphm = scancondpar.Data.fpulsar_h_mag[scansel.Data.binning];			//v11.4変更 by 間々田 2006/03/13

        //            //fpvm の取得
        //            //fpvm = _with2.fpulsar_v_mag(scansel.binning);			//v11.4変更 by 間々田 2006/03/13
        //            detectorParam.fpvm = scancondpar.Data.fpulsar_v_mag[scansel.Data.binning];			//v11.4変更 by 間々田 2006/03/13

        //            //FR(0)の取得
        //            //FR[0] = _with2.frame_rate(scansel.binning, 0);			//v11.4変更 by 間々田 2006/03/13
        //            detectorParam.FR[0] = scancondpar.Data.frame_rate[scansel.Data.binning + 3 * 0];			//v11.4変更 by 間々田 2006/03/13

        //            //FR(1)の取得
        //            //FR[1] = _with2.frame_rate(scansel.binning, 1);			//v11.4変更 by 間々田 2006/03/13
        //            detectorParam.FR[1] = scancondpar.Data.frame_rate[scansel.Data.binning + 3 * 1];			//v11.4変更 by 間々田 2006/03/13

        //            //DCF(0)の取得
        //            //dcf[0] = RemoveNull(_with2.dcf(scansel.binning, 0));	//v11.4変更 by 間々田 2006/03/13
        //            detectorParam.dcf[0] = modLibrary.RemoveNull(scancondpar.Data.dcf[scansel.Data.binning + 3 * 0].GetString());	//v11.4変更 by 間々田 2006/03/13

        //            //DCF(1)の取得
        //            //dcf[1] = RemoveNull(_with2.dcf(scansel.binning, 1));    //v11.4変更 by 間々田 2006/03/13
        //            detectorParam.dcf[1] = modLibrary.RemoveNull(scancondpar.Data.dcf[scansel.Data.binning * 3 + 1].GetString());    //v11.4変更 by 間々田 2006/03/13

        //            //H_SIZE の取得
        //            //h_size = _with2.h_size / hm;			//v11.4変更 by 間々田 2006/03/13
        //            detectorParam.h_size = (int)(scancondpar.Data.h_size / detectorParam.hm);			//v11.4変更 by 間々田 2006/03/13

        //            //V_SIZE の取得
        //            //v_size = _with2.v_size / vm;		//v11.4変更 by 間々田 2006/03/13
        //            detectorParam.v_size = (int)(scancondpar.Data.v_size / detectorParam.vm);		//v11.4変更 by 間々田 2006/03/13

        //            //_with2.fimage_hsize = h_size;
        //            //_with2.fimage_vsize = v_size;
        //            //CallPutScancondpar();
        //            scancondpar.Data.fimage_hsize = detectorParam.h_size;
        //            scancondpar.Data.fimage_vsize = detectorParam.v_size;
        //            scancondpar.Write();

        //            //ビニングモード別校正画像ファイル
        //            //ゲイン
        //            //GAIN_CORRECT = CORRECT_PATH + "Gain" + Convert.ToString(scansel.binning) + ".cor";
        //            ScanCorrect.GAIN_CORRECT = ScanCorrect.CORRECT_PATH + "Gain" + scansel.Data.binning.ToString() + ".cor";

        //            //ゲイン(LONG型用    'v17.00added by 山本 2009-10-19
        //            //GAIN_CORRECT_L = CORRECT_PATH + "Gain_L.cor";
        //            ScanCorrect.GAIN_CORRECT_L = ScanCorrect.CORRECT_PATH + "Gain_L.cor";

        //            //追加 ゲイン校正後に撮影したエアーの画像 'v19.00 by 長野 2012-05-10
        //            //GAIN_CORRECT_AIR = CORRECT_PATH + "Gain_air.cor";
        //            ScanCorrect.GAIN_CORRECT_AIR = ScanCorrect.CORRECT_PATH + "Gain_air.cor";

        //            //オフセット
        //            //OFF_CORRECT = CORRECT_PATH + "Offset" + Convert.ToString(scansel.binning) + ".cor";
        //            ScanCorrect.OFF_CORRECT = ScanCorrect.CORRECT_PATH + "Offset" + scansel.Data.binning.ToString() + ".cor";

        //            //欠陥画像ファイル
        //            //DEF_CORRECT = CORRECT_PATH + "Def_IMAGE" + Convert.ToString(scansel.binning) + ".cor";
        //            ScanCorrect.DEF_CORRECT = ScanCorrect.CORRECT_PATH + "Def_IMAGE" + scansel.Data.binning.ToString() + ".cor";

        //            //校正処理の初期値設定       'added by 間々田 2003/10/29
        //            //IntegNumAtVer = FR[0] * 4;
        //            //IntegNumAtOff = FR[0] * 4;
        //            //IntegNumAtPos = FR[0] * 4;
        //            modScanCorrect.IntegNumAtVer = (int)(detectorParam.FR[0] * 4);
        //            ScanCorrect.IntegNumAtOff = (int)(detectorParam.FR[0] * 4);
        //            ScanCorrect.IntegNumAtPos = (int)(detectorParam.FR[0] * 4);

        //            //10の位で切り上げをする    '修正 2003/10/31 by 間々田
        //            //ViewNumAtRot = Conversion.Int((FR[0] * 20 + 99) * 0.01) / 0.01;
        //            ScanCorrect.ViewNumAtRot = Convert.ToInt32(((detectorParam.FR[0] * 20 + 99) * 0.01) / 0.01);

        //            //v10.0追加 by 間々田 2005/01/24
        //            //ゲイン校正時のビュー数
        //            //ViewNumAtGain = FR[0] * 20;
        //            modScanCorrect.ViewNumAtGain = (int)(detectorParam.FR[0] * 20);

        //            //ゲイン校正時の積算枚数
        //            //IntegNumAtGain = 1;
        //            modScanCorrect.IntegNumAtGain = 1;

        //        }

        //FlluoroIPから移行
        public static void InitFluoroIP()
        {

            //scanParam.EdgeFilterNo = 3;
            //エッジフィルタ番号デフォルト値
            //scanParam.DiffFilterNo = 3;
            //微分フィルタ番号デフォルト値
            //scanParam.FIntegNum = 50;
            //積分枚数のデフォルト値

            //動画保存時間：初期値は10秒
            //scanParam.MovieSaveTime = 10;

            //ライブ画像処理：アベレージング枚数のデフォルト値
            //scanParam.AverageNum = 5;

            //ライブ画像処理：透視画像動画保存時のフレームレートのインデックス値
            //scanParam.gFrameRateIndex = 2;

        }


        public static void CallMakeLT(ref byte[] lt, int wl, int ww)
        {
            ImgProc.MakeLT(lt, lt.Length, wl, ww);
        }
    }
}