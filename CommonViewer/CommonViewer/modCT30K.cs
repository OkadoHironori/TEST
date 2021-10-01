using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

using CTAPI;
using CT30K.Common;

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： CT30K.bas                                                   */
    ///* 処理概要　　： ＣＴ共通モジュール                                          */
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
    ///* V19.00      12/02/20    H.Nagai             BHC対応                        */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
    //internal static class modCT30K
    public class modCT30K
    {
        //********************************************************************************
        //  定数データ宣言
        //********************************************************************************

        //制御画面のフォーム幅   'v15.0追加 by 間々田 2009/01/09
        public const int FmControlWidth = 842;
        public const int FmControlWidth2nd = 522;	//v15.0追加 2ndPC対応 byやまおか 2009/07/29
        
        //定数の定義
        public const int HD_LIMIT = 50;     //ハードディスク最低空き容量(単位ＭＢ)
        public const int ChkTim = 20;	    //ＰＩＯ監視間隔（msec）

        //オンオフステータス定数
        public enum OnOffStatusConstants
        {
            UnknownStatus = -1,
            OffStatus = 0,
            OnStatus = 1
        }

        //色定数
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
        Public Const DarkGreen     As Long = &H8000&
        Public Const SunsetOrange  As Long = &H80C0FF
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
        public static Color DarkGreen = Color.Green;
        public static Color SunsetOrange = Color.FromArgb(0xFF, 0xC0, 0x80);
        //追加2014/10/07hata_v19.51反映
        public static Color DarkGray = Color.FromArgb(0x40, 0x40, 0x40);    //'v18.00追加 byやまおか 2011/07/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************

        //Public IsFineMovingFromPC As Boolean    '微調テーブル動作ステータス(PCから起動) False:停止　True:動作中 'v14.14追加 by 間々田 2008/02/20 今のところコメント

        //PKEのハンドル    'v17.00added by 山本 2009-09-12
        //public static int hPke;
        public const int FPD_INTEGLIST_MAX = 8;			                            //v17.10追加 byやまおか 2010/08/21
        public static double[] fpd_integlist = new double[FPD_INTEGLIST_MAX + 1];	//v17.10追加 byやまおか 2010/08/21

#region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //骨塩定量キャリブROI測定済みフラッグ     'added by 山本 2002-10-19
        //Public GFlg_MaesureCalibRoi   As Integer
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

        //英語モードフラグ             'v7.0 added by 間々田 2003/08/11
        public static bool IsEnglish;

        //v7.0 FPD対応 by 間々田 2003/09/24
        //Public binning      As Long    'v11.4削除 by 間々田 2006/03/13
        //public static float hm;
        //public static float vm;
        //public static float phm;
        //public static float pvm;
        //public static float fphm;
        //public static float fpvm;
        public static bool TableRotOn;
        //public static float[] FR = new float[2];
        //public static string[] dcf = new string[2];

        //public static bool FPGainOn;
        
        public static bool FPDefOn;
        public static int FimageBitIndex;	//0:8bit(256) 1:10bit(1024) 2:12bit(4096)    '追加 by 間々田 2005/01/07

        //追加2014/07/29hata_v19.51反映
        //'透視画像表示用スケールパラメータ   'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        public static float hScale;
        public static float vScale;
        public static float TransScale;   //'v18.00追加 byやまおか 2011/07/16

        public static int tmp_on;	//X線ONフラグ？  'v17.00追加　by　山本 2009-10-29

        //Public GValRawFileSize As Long     'change by 間々田 2003/12/20		
        public static long GValRawFileSize;	//change by　山本　2004-5-19

        //前回のksw                  'v11.2追加 by 間々田 2006/01/12
        public static float LastKsw;

        //強制ウォームアップ 0:短期 1:長期   'v11.3追加 by 間々田 2006/02/20
        public static int gForcedWarmup;

        //終了要求フラグ                     'v11.5追加 by 間々田 2006/06/23
        public static bool RequestExit;

        //public static string ImageProExe;       //イメージプロ実行ファイル名                             'v11.5追加 by 間々田 2006/09/01		
        //public static float ImageProPauseTime;	//イメージプロ起動待ち時間（秒）                         'v11.5追加 by 間々田 2006/09/01		
        //public static bool ImageProStartup;	    //CT30K起動時にイメージプロを起動するかどうかのフラグ    'v11.5追加 by 間々田 2006/09/01

        //[画像から測定]ﾌﾗｸﾞ(0:通常,1:再構成ﾘﾄﾗｲから起動,2:ｽｷｬﾝ条件から起動)     'v14.1追加 byやまおか 2007/08/01
        //Public MeaFrImgFlg  As Integer                                          'v14.1追加 byやまおか 2007/08/01

#region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //回転中心校正ステータスの変化の要因が自動テーブル移動によるものかどうかを判別するためのフラグ。追加 by (SI4)間々田 2002-09-12
        //Public byTableAutoMove          As Boolean  '(True:自動テーブル移動によるもの,False:それ以外)
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

        //回転中心位置のチェックフラグ v16.03/v16.20 追加 by 山影 10-03-12		
        public static bool CheckRC;	    //True:チェックあり、False:なし

        //CT30Kが起動中のフラグ(frmStart表示中)    'v17.20追加 byやまおか 2010/09/16		
        public static bool CT30kNowStartingFlg;	    //True:起動中、 False:起動完了

#region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //連続回転コーン実行可能フラグﾞ 'v17.40追加 by 長野 2010/10/21
        //Public smooth_rot_cone_flg  As Boolean
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

        //
        // IntegTimeメソッドで使用する static フィールド
        //
        private static double IntegTime_Total;

        ////********************************************************************************
        ////  外部関数宣言
        ////********************************************************************************
        ////タスク強制終了用の宣言 START  added by 山本　2004-8-5////////////////////////////////////
        ////Public Declare Function TerminateProcess Lib "kernel32" (ByVal hProcess As Long, ByVal uExitCode As Long) As Long
        //[DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int CloseHandle(int hObject);

        //Public Const SYNCHRO = 1048576         'v17.50削除 2011/02/28 by 間々田
        //Public Const PROCESS_TERMINATE = &H1   'v17.50削除 2011/02/28 by 間々田
        //タスク強制終了用の宣言 END added by 山本　2004-8-5////////////////////////////////////


        //取り込んだ画像ファイル 'v15.0追加
        public static List<string> ImportedCTImages;

        //試験モード？
        //public static bool IsTestMode;

        //前回の画像保存先
        public static string DestOld;

        //非常停止が押されたかどうかを示すフラグ 'v17.40 追加 by 長野 2010/10/21
        public static bool emergencyButton_Flg;

        #region	    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //透視画像サーバー（64ビットEXE）のプロセスID            'v17.50追加 2010/12/17 by 間々田
        //public static int TransImageServerID;
        #endregion	    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''

        //CT30Kメッセージ受信用ポート番号(iniファイルから取得)   'v17.50追加 2011/01/20 by 間々田
        //public static int CT30KPort;

        //関数GetWindowHandleByProcess/GetWindowHandleByProcessSubで使用されるハンドル   'v17.50追加 2011/02/28 by 間々田
        private static int TargetProcessId;
        private static int WindowHandleByProcessSub;

        //v19.10 連続回転用に追加 by長野 2012/07/30
        public static int[] OrgViewListBox;

        //起動完了フラグ   追加2014/10/28hata
        public static bool CT30KSetup = false; 

        //*******************************************************************************
        //機　　能： コモン構造体変数の取得
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        public static void GetCommon()
        {
            //scaninh（コモン）の取得
            //modScaninh.GetScaninh(ref CTSettings.scaninh.Data);
            CTSettings.scaninh = new ScanInh();
            if (!CTSettings.scaninh.Load())
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("ScanInh読み込み失敗",
                //                   Application.ProductName,
                //                   MessageBoxButtons.OK,
                //                   MessageBoxIcon.Error);
            }


            //infdef（コモン）の取得
            //modInfdef.GetInfdef(ref modInfdef.infdef);
            //vb6(setMyCtinfdefのInfdef部分)も同時に取得
            CTSettings.infdef = new InfDef();
            CTSettings.infdef.Data.Initialize();
            if (!CTSettings.infdef.Load())
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("InfDef読み込み失敗",
                //                   Application.ProductName,
                //                   MessageBoxButtons.OK,
                //                   MessageBoxIcon.Error);
            }
            //軸名称の取得
            //軸名称のセット Rev14.24追加 by 間々田 2009-03-10
            //CTSettings.AxisName[0] = StringTable.GetResString(StringTable.IDS_Axis, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[0].GetString()));		//Ｘ線光軸方向
            //CTSettings.AxisName[1] = StringTable.GetResString(StringTable.IDS_Axis, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[1].GetString()));		//Ｘ線光軸と垂直方向
 
            //ctinfdef（コモン）の取得
            //modCtinfdef.GetCtinfdef(ref modCtinfdef.ctinfdef);
            //vb6(setMyCtinfdefのCtinfdef部分)も同時に取得
            CTSettings.ctinfdef = new CTInfDef();
            CTSettings.ctinfdef.Data.Initialize();
            if (!CTSettings.ctinfdef.Load())
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("CTInfDef読み込み失敗",
                //                   Application.ProductName,
                //                   MessageBoxButtons.OK,
                //                   MessageBoxIcon.Error);
            }

            //MyCtinfdefへ設定する
            //modCommon.SetMyCtinfdef();			//v11.4追加 by 間々田 2006/03/14
            modCommon.SetMyCtinfdef();


            //t20kinf（コモン）の取得
            //modT20kinf.GetT20kinf(ref modT20kinf.t20kinf);
            //t20kinf(コモン)の取得
            CTSettings.t20kinf = new T20kInf();
            CTSettings.t20kinf.Data.Initialize();
            if (!CTSettings.t20kinf.Load())
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("T20kInf読み込み失敗",
                //                   Application.ProductName,
                //                   MessageBoxButtons.OK,
                //                   MessageBoxIcon.Error);
            }

            //scancondpar（コモン）の取得
            //modScancondpar.CallGetScancondpar();
            CTSettings.scancondpar = new ScanCondPar();
            CTSettings.scancondpar.Data.Initialize();
            if (!CTSettings.scancondpar.Load(CTSettings.scaninh.Data.rotate_select))
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("ScanCondPar読み込み失敗",
                //                   Application.ProductName,
                //                   MessageBoxButtons.OK,
                //                   MessageBoxIcon.Error);
            }

            //workshopinf（コモン）の取得
            //modWorkshopinf.GetWorkshopinf(ref modWorkshopinf.workshopinf);		//v11.5追加 by 間々田 2006/04/24
            CTSettings.workshopinf = new WorkShopInf();
            CTSettings.workshopinf.Data.Initialize();
            if (!CTSettings.workshopinf.Load())
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("WorkShopInf読み込み失敗",
                //                   Application.ProductName,
                //                   MessageBoxButtons.OK,
                //                   MessageBoxIcon.Error);
            }

            //mechapara（コモン）の取得
            //modMechapara.GetMechapara(ref modMechapara.mechapara);			//v15.0追加 by 間々田 2009/01/07
            CTSettings.mechapara = new MechaPara();
            CTSettings.mechapara.Data.Initialize();
            if (!CTSettings.mechapara.Load())
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("MechaPara読み込み失敗",
                //                   Application.ProductName,
                //                   MessageBoxButtons.OK,
                //                   MessageBoxIcon.Error);
            }

            //xtable（コモン）の取得
            //modXTable.GetXtable(ref modXTable.xtable);			//v15.0追加 by 間々田 2009/01/07
            CTSettings.xtable = new XTable();
            CTSettings.xtable.Data.Initialize();
            if (!CTSettings.xtable.Load())
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("XTable読み込み失敗",
                //                   Application.ProductName,
                //                   MessageBoxButtons.OK,
                //                   MessageBoxIcon.Error);
            }

#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            ////    'hscpara(コモン) の取得
            ////    GetHscpara hscpara              'v16.01 追加 by 山影 2010/02/03
            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            //CTSettings.hscpara = new HscPara();
            //if (!CTSettings.hscpara.Load())
            //    MessageBox.Show(" HscPara読み込み失敗",
            //                        Application.ProductName,
            //                        MessageBoxButtons.OK,
            //                        MessageBoxIcon.Error);
#endregion

            //dispinf(コモン)の取得
            CTSettings.dispinf = new DispInf();
            CTSettings.dispinf.Data.Initialize();
            if (!CTSettings.dispinf.Load())
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("DispInf読み込み失敗",
                //                   Application.ProductName,
                //                   MessageBoxButtons.OK,
                //                   MessageBoxIcon.Error);
            }
            //mecainf(コモン)の取得
            CTSettings.mecainf = new MecaInf();
            CTSettings.mecainf.Data.Initialize();
            if (!CTSettings.mecainf.Load())
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("MecaInf読み込み失敗",
                //                    Application.ProductName,
                //                    MessageBoxButtons.OK,
                //                    MessageBoxIcon.Error);
            }

            //scansel(コモン)の取得
            CTSettings.scansel = new ScanSel();
            CTSettings.scansel.Data.Initialize();
            if (!CTSettings.scansel.Load())
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("MecaInf読み込み失敗",
                //                    Application.ProductName,
                //                    MessageBoxButtons.OK,
                //                    MessageBoxIcon.Error);
            }

            //sp2inf(コモン)の取得
            CTSettings.sp2inf = new sp2inf();
            CTSettings.sp2inf.Data.Initialize();
            if (!CTSettings.sp2inf.Load())
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("MecaInf読み込み失敗",
                //                    Application.ProductName,
                //                    MessageBoxButtons.OK,
                //                    MessageBoxIcon.Error);
            }

            //pdplan(コモン)の取得
            CTSettings.pdplan = new pdplan();
            CTSettings.pdplan.Data.Initialize();
            if (!CTSettings.pdplan.Load())
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("MecaInf読み込み失敗",
                //                    Application.ProductName,
                //                    MessageBoxButtons.OK,
                //                    MessageBoxIcon.Error);
            }

            //discharge_disprotect(コモン)の取得
            CTSettings.discharge_protect = new discharge_protect();
            CTSettings.discharge_protect.Data.Initialize();
            if (!CTSettings.discharge_protect.Load())
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("MecaInf読み込み失敗",
                //                    Application.ProductName,
                //                    MessageBoxButtons.OK,
                //                    MessageBoxIcon.Error);
            }

            //zoomtbl(コモン)の取得
            CTSettings.zoomtbl = new zoomtbl();
            CTSettings.zoomtbl.Data.Initialize();
            if (!CTSettings.zoomtbl.Load())
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("MecaInf読み込み失敗",
                //                    Application.ProductName,
                //                    MessageBoxButtons.OK,
                //                    MessageBoxIcon.Error);
            }

            //
            //一応ここで読んでおく
            //
            //reconinf(コモン)の取得
            CTSettings.reconinf = new ReconInf();
            CTSettings.reconinf.Data.Initialize();

            //imageinfo(コモン)の取得
            CTSettings.gImageinfo = new ImageInfo();
            CTSettings.gImageinfo.Data.Initialize();

        }
        

        //********************************************************************************
        //機    能  ：  保存画像サイズを計算する
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  結果をintExpendSize()に代入する
        //
        //履    歴  ：  V1.00  2000/03/24  (CATS)山本       新規作成
        //              V3.0   2000/09/06  (SI1)鈴山        ｺｰﾝﾋﾞｰﾑ対応
        //              V4.0   2001/02/21  (SI1)鈴山        frmScanConditionから移した。
        //********************************************************************************
        public static void Cal_SaveImageSize()
        {
//            int Val_View = 0;		//ビュー数
//            int Val_Mc = 0;			//縦チャンネル数
//            string FileName = null;
//            int FftSize = 0;		//FFTサイズ
//            int nd = 0;			    //横チャンネル数                         'V3.0 append by 鈴山
//            long RawFile = 0;	    //生ﾃﾞｰﾀﾌｧｲﾙｻｲｽﾞ(ﾉｰﾏﾙCT/ｺｰﾝﾋﾞｰﾑCT兼用)   'V3.0 append by 鈴山    'V3.0 change by 鈴山 2000/10/02 (Long → Variant)
//            int InfFile = 0;		//付帯情報ﾌｧｲﾙｻｲｽﾞ                       'V3.0 append by 鈴山
//            int DataView = 0;		//ﾃﾞｰﾀ収集ﾋﾞｭｰ数                         'V3.0 append by 鈴山
//            int cnt_k = 0;			//ｺｰﾝﾋﾞｰﾑｽﾗｲｽ枚数(ｺｰﾝﾋﾞｰﾑCT専用)         'V3.0 append by 鈴山
//            int cnt_slice = 0;	    //ﾏﾙﾁｽｷｬﾝ・ｽﾗｲｽﾌﾟﾗﾝの枚数                'V3.0 append by 鈴山
//            int cnt_multi = 0;	    //オートズームの枚数                     'V3.0 append by 鈴山
//            int cnt_zoom = 0;		//オートズームの枚数                     'V3.0 append by 鈴山

//            //スライスプランテーブル用の変数
//            int SPView = 0;
//            int SPK = 0;
//            double ScanImageSize = 0;		//現在のスキャン条件でスキャンする場合に消費するディスク容量(MB)

//            //scancondpar（コモン）の読み込み                    'v11.2追加 by 間々田 2005/10/06
//            //CallGetScancondpar                                  'v11.2削除 by 間々田 2006/04/26

//            //On Error GoTo ErrorHandler

//            //初期値
//            cnt_slice = 1;
//            SPView = 0;
//            SPK = 0;
//            cnt_zoom = 0;			//オートズームの枚数
//            cnt_multi = 1;			//マルチスライスの枚数

//            switch (CTSettings.scansel.Data.multiscan_mode)
//            {
//                //マルチの場合
//                case (int)ScanSel.MultiScanModeConstants.MultiScanModeMulti:
//                    cnt_slice = (int)modLibrary.MaxVal(CTSettings.scansel.Data.multinum, 1);
//                    break;

//                //スライスプランの場合
//                case (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan:

//                    //スライスプランテーブルの取得
//                    FileName = GetSlicePlanTable(CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone);

//                    //スライス数の取得
//                    //cnt_slice = MaxVal(GetCsvCount(FileName, LoadResString(12035)), 1)  'LoadResString(12035):スライス数
//                    GetSlicePlanParameter(FileName, ref cnt_slice, ref SPView, ref SPK);		//change by 間々田 2003/12/20
//                    break;
//            }

//            //ビュー数設定値
//            Val_View = (SPView > 0 ? SPView : CTSettings.scansel.Data.scan_view);

//            //■ｺｰﾝﾋﾞｰﾑｽｷｬﾝ条件

//            //縦中心ﾁｬﾝﾈﾙ(ﾗｲﾝ数半幅)
//            Val_Mc = CTSettings.scansel.Data.mc;

//            //■保存画像サイズを計算する

//            //透視画像横サイズからFFTサイズ（生ﾃﾞｰﾀの横サイズ）を求める
//            if (CTSettings.scancondpar.Data.fimage_hsize < 1024)
//            {
//                FftSize = 1024;
//            }
//            else
//            {
//                FftSize = 2048;
//            }

//            //オフセットスキャンの場合はFFTサイズを2倍する
//            //変更2014/10/07hata_v19.51反映
//            //if (CTSettings.scansel.Data.scan_mode == 3) FftSize = FftSize * 2;
//            if ((CTSettings.scansel.Data.scan_mode == 3) || (CTSettings.scansel.Data.scan_mode == 3)) FftSize = FftSize * 2;    //'v18.00変更 byやまおか 2011/02/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

//            //横チャンネル数を求める          'V3.0 append by 鈴山
//            //'v18.00修正 byやまおか 2011/02/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
//            //nd = CTSettings.scancondpar.Data.mainch[CTSettings.scansel.Data.scan_mode - 1];
//            switch (CTSettings.scansel.Data.scan_mode)
//            {
//                //ハーフ、フル
//                case 1:
//                case 2:
//                    nd = CTSettings.scancondpar.Data.mainch[0];
//                    break;
                
//                //'オフセット
//                case 3:
//                    nd = CTSettings.scancondpar.Data.mainch[0] * 2;
//                    break;
//                //'シフトオフセット
//                case 4:
//                    nd = (CTSettings.scancondpar.Data.mainch[0] + CTSettings.scancondpar.Data.det_sft_pix) * 2;
//                    break;
//            }

//            //データ収集ビュー数を求める scancondpar.Alpha:ｵｰﾊﾞｰﾗｯﾌﾟ角度(radian)
//            //2014/11/13hata キャストの修正
//            //DataView = (int)(Val_View * (1 + CTSettings.scancondpar.Data.alpha / ScanCorrect.Pai));
//            //DataView = Convert.ToInt32(Val_View * (1 + CTSettings.scancondpar.Data.alpha / ScanCorrect.Pai));

//#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
//            //    'ヘリカルの場合
//            //    '   scansel.zs:再構成開始位置(mm)=1枚目ｽﾗｲｽのZ位置(mm)
//            //    '   scansel.ze:再構成終了位置(mm)=K枚目ｽﾗｲｽのZ位置(mm)
//            //    '   scansel.Zp:ヘリカルピッチ
//            //    If scansel.inh = 1 Then DataView = DataView + (scansel.ze - scansel.zs) / scansel.Zp
//            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
//#endregion

//            //透視画像横サイズ       'v11.5下から移動 by 間々田 2006/06/06
//            int h = 0;
//            h = CTSettings.scancondpar.Data.fimage_hsize;

//            //生データファイルサイズを求める     'V3.0 append by 鈴山
//            switch (CTSettings.scansel.Data.data_mode)
//            {
//                //スキャンの場合
//                case (int)ScanSel.DataModeConstants.DataModeScan:
//                    //RawFile = Val_View * FftSize * 2;
//                    RawFile = Val_View * nd * 2; //'v18.00修正 今は生を引き伸ばしていない byやまおか 2011/02/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

                    
//                    break;

//                //コーンビームスキャンの場合
//                case (int)ScanSel.DataModeConstants.DataModeCone:

//                    //外部トリガ取込みかつテーブル回転が連続回転の時             'V7.0 FPD対応 by 間々田 2003/09/10
//                    //If (scaninh.ext_trig = 0) And TableRotOn Then
//                    //If False Then                                   '連続回転の場合の計算でエラーになるのでとりあえず、こうする by 間々田 2009/06/30
//                    if (TableRotOn)     //v16.2 追加 by 山影 2010/01/19
//                    {
//                        float the_ic = 0;
//                        float the_jc = 0;
//                        float the_theta_s = 0;
//                        float the_dpm = 0;
//                        int nstart = 0;
//                        int nend = 0;
//                        int mstart = 0;
//                        int mend = 0;
//                        float the_delta_theta = 0;
//                        float the_n0 = 0;
//                        float the_m0 = 0;
//                        float delta_im = 0;
//                        float delta_jm = 0;
//                        float ir0 = 0;
//                        float jr0 = 0;
//                        float kix = 0;
//                        float kjx = 0;
//                        int js = 0;
//                        int je = 0;
//                        float delta_ix = 0;
//                        float delta_jx = 0;

//                        //'透視画像横サイズ
//                        //Dim h As Long
//                        //h = scancondpar.fimage_hsize

//                        //v16.2 ２次元幾何歪補正の場合、Read_HizumiTableを呼ばないように変更 by 山影 2010/01/19
//                        if (CTSettings.scaninh.Data.full_distortion == 0)
//                        {
//                            //ScanCorrect.hizumi = new float[2];			//cone_setup_iicrctでのアクセスエラー回避用
//                        }
//                        //Rev99.99 1次元幾何歪は行わないため不要 by長野 2014/11/10
//                        //else
//                        //{
//                        //    ScanCorrect.Read_HizumiTable(ref ScanCorrect.hizumi);
//                        //}

//                        //js, je の算出
//                        //Call cone_setup_iicrct(the_ic, the_jc, the_theta_s, the_dpm, nstart, nend, mstart, mend, the_delta_theta, the_n0, the_m0, _
//                        //'                       delta_im, delta_jm, ir0, jr0, kix, kjx, B1, a, b, h, v, FGD, h, Val_Mc, hizumi(0), js, je, delta_ix, delta_jx, hm, vm, 0)

//                        //変更2014/10/07hata_v19.51反映
//                        //v11.2以下に変更 by 間々田 2005/10/06
//                        //js, je の算出
//                        //CTAPI.CTLib.cone_setup_iicrct(ref the_ic, ref the_jc, ref the_theta_s, ref the_dpm, ref nstart, ref nend, ref mstart, ref mend, ref the_delta_theta,
//                        //                             ref the_n0, ref the_m0, ref delta_im, ref delta_jm, ref ir0, ref jr0, ref kix, ref kjx,
//                        //                             CTSettings.scancondpar.Data.b[1], CTSettings.scancondpar.Data.scan_posi_a[2], CTSettings.scancondpar.Data.scan_posi_b[2],
//                        //                             CTSettings.scancondpar.Data.fimage_hsize, CTSettings.scancondpar.Data.fimage_vsize,
//                        //                             CTSettings.scansel.Data.fid, CTSettings.scancondpar.Data.fimage_hsize, 2 * Val_Mc + 1, Val_Mc,
//                        //                             ref ScanCorrect.hizumi[0], ref js, ref je, ref delta_ix, ref delta_jx, CTSettings.detectorParam.hm, CTSettings.detectorParam.vm, 0, CTSettings.scaninh.Data.full_distortion,
//                        //                             CTSettings.scancondpar.Data.ist, CTSettings.scancondpar.Data.ied, CTSettings.scancondpar.Data.detector);	//v17.00 引数detector追加 byやまおか 2010/02/26
//                        //CTAPI.CTLib.cone_setup_iicrct(ref the_ic, ref the_jc, ref the_theta_s, ref the_dpm, ref nstart, ref nend, ref mstart, ref mend, ref the_delta_theta,
//                        //                             ref the_n0, ref the_m0, ref delta_im, ref delta_jm, ref ir0, ref jr0, ref kix, ref kjx,
//                        //                             CTSettings.scancondpar.Data.b[1], CTSettings.scancondpar.Data.scan_posi_a[2], CTSettings.scancondpar.Data.scan_posi_b[2],
//                        //                             CTSettings.scancondpar.Data.fimage_hsize, CTSettings.scancondpar.Data.fimage_vsize,
//                        //                             CTSettings.scansel.Data.fid, CTSettings.scancondpar.Data.fimage_hsize, 2 * Val_Mc + 1, Val_Mc,
//                        //                             ref ScanCorrect.hizumi[0], ref js, ref je, ref delta_ix, ref delta_jx, CTSettings.detectorParam.hm, CTSettings.detectorParam.vm, 0, CTSettings.scaninh.Data.full_distortion,
//                        //                             CTSettings.scancondpar.Data.ist, CTSettings.scancondpar.Data.ied, CTSettings.scancondpar.Data.detector,
//                        //                             CTSettings.scansel.Data.scan_mode);	//v17.00 引数detector追加 byやまおか 2010/02/26 v19.41とv18.02の統合 by長野 2013/11/12

//                        ////Rev99.99 modScanConditionもここで更新 by長野 2014/12/15
//                        //modScanCondition.ScanJe = je;
//                        //modScanCondition.ScanJs = js;

//                        //v9.6 追加ここから by 間々田 2004/10/22
//                        double theValue = 0;
//                        //theValue = Convert.ToDouble(je - js + 1) * Convert.ToDouble(nd) * Convert.ToDouble(DataView) * 2.0;
//                        theValue = Convert.ToDouble(je - js + 1) * Convert.ToDouble(CTSettings.scancondpar.Data.mainch[0]) * Convert.ToDouble(DataView) * 2.0;  //'v18.00修正 コーンの場合は常に引き伸ばさない byやまおか 2011/02/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                        
//                        //if (theValue >= Math.Pow(2, 31))
//                        //{
//                        //    //DataView = (Val_View - 100) * (1 + Val_Alpha / Pai) 'DataViewの再計算
//                        //    //2014/11/13hata キャストの修正
//                        //    //DataView =(int)((Val_View - 100) * (1 + CTSettings.scancondpar.Data.alpha / ScanCorrect.Pai));		//DataViewの再計算   'v11.2変更 by 間々田 2005/10/06
//                        //    DataView = Convert.ToInt32((Val_View - 100) * (1 + CTSettings.scancondpar.Data.alpha / ScanCorrect.Pai));		//DataViewの再計算   'v11.2変更 by 間々田 2005/10/06
//                        //}
//                        //v9.6 追加ここまで by 間々田 2004/10/22

//                        //                RawFile = CVar(2 * Val_Mc + 1) * nd * DataView * 2 + (je - js + 1) * nd * DataView * 2
//                        //                RawFile = CVar(2 * Val_Mc + 1) * h * DataView * 2 + (je - js + 1) * h * DataView * 2        'changed by 山本　2004-10-22　nd->h ビニング後のサイズにするため
//                        RawFile = 2 * Val_Mc + 1 * h * DataView * 2 + je - js + 1 * h * DataView * 2;					//v16.2 オーバーフロー対策 by山影 2010/01/19
//                    }
//                    else
//                    {
//                        //                RawFile = CVar(2 * Val_Mc + 1) * nd * DataView * 2
//                        RawFile = 2 * Val_Mc + 1 * h * DataView * 2;						//changed by 山本　2004-10-22　nd->h ビニング後のサイズにするため
//                    }
//                    break;
//            }


//            //付帯情報ファイルサイズを求める
//            //modImageInfo.ImageInfoStruct dummy = default(modImageInfo.ImageInfoStruct);
//            CTAPI.CTstr.IMAGEINFO dummy = default(CTAPI.CTstr.IMAGEINFO);
            
//            InfFile = Marshal.SizeOf(dummy);

//            //ｺｰﾝﾋﾞｰﾑｽﾗｲｽ枚数を求める

//            // V3.0 append by 鈴山 START
//            //コーンビームスキャンの場合
//            if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone)
//            {
//                //スライス枚数
//                cnt_k = (SPK > 0 ? SPK : (int)modLibrary.MaxVal(CTSettings.scansel.Data.k, 1));
//            }
//            //ノーマルスキャンの場合
//            else
//            {
//                cnt_k = 1;
//                switch (CTSettings.scansel.Data.multislice)  //ﾏﾙﾁｽﾗｲｽ：0(1ｽﾗｲｽ),1(3ｽﾗｲｽ),2(5ｽﾗｲｽ)
//                {
//                    case 1:
//                        cnt_multi = 3;
//                        break;
//                    case 2:
//                        cnt_multi = 5;
//                        break;
//                }

//                //オートズーミング時（ノーマルスキャンかつシングル／マルチの場合）
//                if ((CTSettings.scansel.Data.auto_zoomflag == 1) && (!(CTSettings.scansel.Data.multiscan_mode == 5)))
//                {
//                    //ズーミングテーブルの取得
//                    FileName = GetZoomingTable();

//                    //ズーミング個数を取得
//                    //cnt_zoom = MaxVal(GetCsvCount(FileName, LoadResString(12050)), 0)
//                    int zoom = 0;
//                    int.TryParse(modLibrary.GetCsvItem(FileName, CTResources.LoadResString(StringTable.IDS_ZoomingCount)), out zoom);
//                    cnt_zoom = (int)modLibrary.MaxVal(zoom, 0);			//v9.5修正 by 間々田 2004/10/18
//                }
//            }
//            // V3.0 append by 鈴山 END

//            GValRawFileSize = RawFile;			//グローバル変数に値を保持   added 2003/11/25 by 間々田

//            int theMatrix = 0;

//            //マトリックスサイズ(256x256の画像のファイルサイズは５１２)
//#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
///*
//            theMatrix = Choose(scansel.matrix_size, 512, 512, 1024, 2048, 4096)  'v16.10 4096を追加 by 長野　2010/02/23
//*/
//#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
//            switch (CTSettings.scansel.Data.matrix_size)
//            {
//                case 1:
//                    theMatrix = 512;
//                    break;
//                case 2:
//                    theMatrix = 512;
//                    break;
//                case 3:
//                    theMatrix = 1024;
//                    break;
//                case 4:
//                    theMatrix = 2048;
//                    break;
//                case 5:
//                    theMatrix = 4096;
//                    break;
//            }			//v16.10 4096を追加 by 長野　2010/02/23

//            double theImageMB = 0;
//            theImageMB = (double)(theMatrix * theMatrix * 2 + InfFile) / 1024 / 1024;		//MB単位

//            ScanImageSize = theImageMB * cnt_k * cnt_slice * cnt_multi * (cnt_zoom + 1);

//            //生データ保存時またはコーンビームの場合
//            //   ＊コーンビームの場合は生データ保存なしでも一時的にファイル保存するため、
//            //     空き容量計算においては生データ保存ありにしておく
//            if ((CTSettings.scansel.Data.rawdata_save == 1) ||
//                (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone))
//            {
//                if (CTSettings.scansel.Data.rawdata_save == 0)
//                {                
//                }                
//                else                                                                                            //v11.5追加 by 間々田 2006-06-28
//                {                                                                                               //v11.5追加 by 間々田 2006-06-28
//                    ScanImageSize = ScanImageSize + (double)(RawFile * cnt_slice * cnt_multi) / 1024 / 1024;	//MB単位
//                }                                                                                               //v11.5追加 by 間々田 2006-06-28				
//            }

//            //frmStatus.lblScanImageSize.Caption = CStr(ScanImageSize)
//            //frmCTMenu.Instance.ScanImageSize = ScanImageSize;

//            return;


////ここに入らないのでコメントとする
////ErrorHandler:
    
////            //ｴﾗｰﾒｯｾｰｼﾞ
////            modLibrary.ErrorDescription(StringTable.GetResString(9904, "modCT30K", "Cal_SaveImageSize"));
            return;
        }


        //********************************************************************************
        //機    能  ：  スタートアップモジュール
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V3.0   00/08/01  (SI1)鈴山       新規作成
        //             v13.0  2007/03/19 (WEB)間々田   ダブルオブリーク対応
        //********************************************************************************
        public static void CT30KMain()
        {
            //int error_sts = 0;

            //追加2014/10/28hata
            CT30KSetup = false; //起動開始中

            //英語モード？                                   'リソース対応 2003/07/26 by 間々田
            //IsEnglish = (CTResources.LoadResString(100) == "English");

            //試験モード？
            ////IsTestMode = (InStr(LCase$(command()), "-test") > 0)
            //string[] commands = Environment.GetCommandLineArgs(); 
            //for (int i = 1; i < commands.Length; i++)   // GetCommandLineArgs()の先頭要素は実行したプログラムファイル名なので無視する
            //{
            //    if (commands[i].ToLower().IndexOf("-test") != -1)
            //    {
            //        IsTestMode = true;
            //        break;
            //    }
            //}
            //→　AppValue.IsTestModeで取得する

            //program.csで実施に変更_2014/03/19hata　
            ////二重起動防止
            //string processName = Process.GetCurrentProcess().ProcessName;
            //if (Process.GetProcessesByName(processName).Length > 1)
            //{
            //    //メッセージ表示：
            //    //   ＣＴプログラムが既に実行されていますので、先に立上がっている方を使用してください。
            //    //   立上げを実行しますか？"
            //    DialogResult dialogResult = MessageBox.Show(CTResources.LoadResString(9349), Application.ProductName,
            //                                                 MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            //    if (dialogResult == DialogResult.No) Environment.Exit(0);
            //}

            ////パスの設定→GetCtIni内で行うため不要
            //modFileIO.InitPath();

            //iniファイルの読み込み
            if (!GetCtIni()) Environment.Exit(0);

            //共有メモリ作成
            if (CTSettings.InitializeSharedCTCommon() != 0)
            {
                Environment.Exit(0);
            }
            
            //イメージプロ起動   'frmCTMenu の MDIForm_Load から移動 by 間々田 2004/09/16 フリーズする対策
            //if (ImageProStartup) StartImagePro();
            if (CTSettings.iniValue.ImageProStartup)
            {
                StartImagePro();

                ////イメージプロServerの起動確認をする
                //Process[] ps = Process.GetProcessesByName("ImageProServer");
                //foreach (Process p in ps)
                //{
                //    //起動中の場合は終了させる
                //    p.Kill();
                //    p.WaitForExit(2000);
                //    p.Dispose();
                //}

                ////イメージプロServerの起動
                //Process pIp = null;
                //try
                //{
                //    pIp = new Process();
                //    pIp.StartInfo.FileName = @"C:\CT\COMMAND\ImageProServer.exe";
                //    pIp.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                //    pIp.Start();

                //}
                //finally
                //{
                //    if (pIp != null)
                //    {
                //        pIp.Dispose();
                //        pIp = null;
                //    }
                //}
                
            }

            ////    'コモンの初期化処理(ＶＢ用)
            ////    error_sts = ctcominit(0)
            ////    If error_sts <> 0 Then
            ////        'エラー表示：[中止]なら終了
            ////        If ErrMessage(error_sts) = vbAbort Then End
            ////    End If

            //コモン初期化→comset.exeを実行
            Process pcomset = null;
            try
            {
                pcomset = new Process();
                pcomset.StartInfo.FileName = AppValue.COMSET;
                pcomset.StartInfo.Arguments = "0";
                pcomset.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;

                pcomset.Start();
                pcomset.WaitForExit(2000);
            }
            finally
            {
                if (pcomset != null)
                {
                    pcomset.Dispose();
                    pcomset = null;
                }
            }
            //PauseForDoEvents(2);

            //コモン構造体変数の取得（SetFixedVariableもここで行う）
            //GetCommon();
            CTSettings.Load();


            //v16.2 変更 by 山影
            //    If scaninh.gpgpu = 0 Then                                             'v16.00 再構成高速化機能が有りの場合，コールするCONEBEAMとCONERECONのexeをGPGPUを使用する関数へ変更させる　by Chouno 2009-11-16
            //        CONEBEAM = FSO.BuildPath(pathCommand, "conebeamgpgpu.exe")        'v16.00 再構成高速化機能有のコーンビームスキャン
            //        CONERECON = FSO.BuildPath(pathCommand, "conerecongpgpu.exe")      'v16.00 再構成高速化機能有のコーンビーム再々構成
            //    End If
            //modFileIO.CONEBEAM = (CTSettings.scaninh.Data.gpgpu == 0 ? modFileIO.CONEBEAM_GPGPU : modFileIO.CONEBEAM_NORMAL);
            //modFileIO.CONERECON = (CTSettings.scaninh.Data.gpgpu == 0 ? modFileIO.CONERECON_GPGPU : modFileIO.CONERECON_NORMAL);
            //→　AppValue.CONEBEAMで値が取れるため設定不要
            //→　AppValue.CONERECONで値が取れるため設定不要


#if DebugOn
            //scaninh の設定を一時的に変更するためのダイアログ（デバッグ時のみ表示）
            frmConfig.Instance.ShowDialog();
#endif

            //マウスカーソルを砂時計にする   'v16.30/v17.00追加 byやまおか 2010/02/23
            Cursor.Current = Cursors.WaitCursor;

#region	    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //透視画像サーバーを使用する場合（64ビット対応） 追加 2010/12/17 by 間々田
            //#If TRANS_IMAGE_SERVER_USE Then
            //
            //    TransImageServerID = 0
            //    If scaninh.mechacontrol = 0 Then
            //
            //        '透視画像サーバー（64ビットEXE）の起動
            //        If FSO.FileExists(TransImageServer) Then
            //            TransImageServerID = Shell(TransImageServer)
            //        End If
            //
            //        If TransImageServerID = 0 Then
            //            'MsgBox "透視画像サーバーを起動できませんでした。", vbCritical
            //            'v17.60 ストリングテーブル化 by長野 2011/05/25
            //            MsgBox LoadResString(20180), vbCritical
            //            End
            //        End If
            //
            //    End If
            //
            //#End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //    '複数Ｘ線管と回転選択がともに有効の場合はエラーメッセージを表示し、scaninhibit.csvの再設定を促す    'V9.0 append by 間々田 2004/01/30
            //    If (scaninh.multi_tube = 0) And (scaninh.rotate_select = 0) Then
            //        'メッセージ表示：
            //        '   複数Ｘ線管と回転選択がともに有効なため、CT30Kを起動できません。
            //        '   どちらかを無効にするように設定ファイル scaninhibit.csv を修正した後、CT30Kを再起動してください。
            //        'MsgBox LoadResString(12914), vbCritical
            //        'v16.01 変更 by 山影 10-02-17
            //        MsgBox BuildResStr(IDS_ConflictScanInhibit, IDS_MultiTube, IDS_RotateSelect), vbCritical
            //        End
            //    End If
            if ((CTSettings.scaninh.Data.multi_tube == 0) && (CTSettings.scaninh.Data.rotate_select == 0))
            {
                //string MsgTitle = CTSettings.t20kinf.Data.system_name + " " + CTSettings.t20kinf.Data.version;
                //string msg = string.Format(StringTable.GetResString((int)StringTable.IDS_ConflictScanInhibit),
                //        StringTable.GetResString((int)StringTable.IDS_MultiTube), StringTable.GetResString((int)StringTable.IDS_RotateSelect));

                //MessageBox.Show(msg, MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Environment.Exit(0);
            }
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //    'v16.01 追加 by 山影 10-02-04
            //    'X線管回転と高速度透視撮影がともに有効の場合はエラーメッセージを表示し、scaninhibit.csvの再設定を促す
            //    If (scaninh.xray_rotate = 0) And (scaninh.high_speed_camera = 0) Then
            //        'メッセージ表示：
            //        '   X線管回転と高速度透視撮影がともに有効なため、CT30Kを起動できません。
            //        '   どちらかを無効にするように設定ファイル scaninhibit.csv を修正した後、CT30Kを再起動してください。
            //        MsgBox BuildResStr(IDS_ConflictScanInhibit, IDS_XrayRotate, IDS_HighSpeedCamera), vbCritical
            //        End
            //    End If
            if ((CTSettings.scaninh.Data.xray_rotate == 0) && (CTSettings.scaninh.Data.high_speed_camera == 0))
            {
                //// メッセージ表示：
                ////    X線管回転と高速度透視撮影がともに有効なため、CT30Kを起動できません。
                ////    どちらかを無効にするように設定ファイル scaninhibit.csv を修正した後、CT30Kを再起動してください。
                //string MsgTitle = CTSettings.t20kinf.Data.system_name + " " + CTSettings.t20kinf.Data.version;
                //string msg = string.Format((StringTable.GetResString(StringTable.IDS_ConflictScanInhibit)),
                //        (StringTable.GetResString(StringTable.IDS_XrayRotate)), StringTable.GetResString(StringTable.IDS_HighSpeedCamera));

                //MessageBox.Show(msg, MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Application.Exit();
                //System.Environment.Exit(0);
            }
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion


            //ＣＴ起動時に値が決定される変数（その後不変）の設定             'v10.0追加 by 間々田 2005/01/25
            //CTSettings.SetFixedVariable();

            ////PKEで xis.iniが存在しない場合処理をやめる        'v17.50追加 by 間々田 2011/03/05
            //if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) && (CTSettings.scaninh.Data.mechacontrol == 0))
            //{
            //     if (!File.Exists(AppValue.XisIniFileName))
            //    {
            //        //MsgBox "以下のファイルが見つかりません。ＣＴ３０Ｋを終了します。" & vbCr & vbCr & XisIniFileName, vbCritical
            //        //v17.60 ストリングテーブル化 by長野 2011/05/30

            //        string msg = string.Format("{0}\n\n{1}", CTResources.LoadResString(20181), AppValue.XisIniFileName);
            //        MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        Environment.Exit(0);
            //    }
            //}

            //コモンの Scansel でCT30kで頻繁に使用するものを変数に記憶       'v10.0追加 by 間々田 2005/01/25
            //GetScansel
            modCommon.GetMyScansel();			//v11.2追加 by 間々田 2006/01/13
            
            //グローバル変数の初期化                                         'v9.71追加 by 間々田 05/01/12
            InitGlobalVariables();
            
            ////ダブルオブリークの起動：起動画面表示の前に起動する             'v13.0追加 by 間々田 2007/03/19
            //if (CTSettings.scaninh.Data.double_oblique == 0) modDoubleOblique.StartDoubleOblique();

            ////既存の文字列定数をリソースから読み込む
            //StringTable.SetConstString();

            ////v15.0追加 by 間々田 2009/04/10
            //ScanCorrect.OptValueGet_Cor();

            ////ＣＴメインメニューを表示
            //var _frmCTMenu = frmCTMenu.Instance;
            //_frmCTMenu.Show();

            //追加2014/10/07hata_v19.51反映
            //'スキャンモードオプションボタンの表示と位置調整(InitControl内だと失敗するのでここで実行)
            //modLibrary.RePosVisibleOption((frmScanControl.Instance.optScanMode)); //'v18.00追加 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            
            //データが渡るまで少し待つ
            //PauseForDoEvents(2);

            //v15.0変更 2ndPC対応 byやまおか 09/07/29
            if (CTSettings.scaninh.Data.mechacontrol == 0)
            {
                //// 撮影コントロール　検出器設定
                //CTSettings.transImageControl.Detector = CTSettings.detectorParam;
                //CTSettings.transImageControl.SetScanParam(CTSettings.scanParam);

                ////透視画像処理フォームを表示
                //frmTransImage.Instance.Show(_frmCTMenu);

                ////透視画像付帯情報フォームを表示
                //frmTransImageInfo.Instance.Show(_frmCTMenu);
                
                ////透視画像コントロールフォームを表示
                //frmTransImageControl.Instance.Show(_frmCTMenu);

                ////起動時のCT/高速撮影の判定 v16.01 追加 by 山影 10-02-04
                //modHighSpeedCamera.HscStartUpProcess();

                if (CTSettings.SecondDetOn)
                {
                    
                    //'起動時の検出器の判定 v17.20 追加 by 長野 10-08-31
                    //'SecondDetctorStartUpProcess
                    
                    //// 'v17.51変更 by 間々田 2011/03/24
                    //if (mod2ndDetctor.SecondDetctorStartUpProcess())
                    //{
                    //    //'検出器切替ありで検出器不定ではないとき、ここでウォームアップを促す
                    //    frmXrayControl.Instance.QueryWarmup();
                    //}
                }
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            }

            ////追加2014/09/11hata
            ////ここで初期化
            ////起動フラグを初期化
            //modCTBusy.CTBusy = 0;

            ////CTメニューにフォーカスする
            //_frmCTMenu.Focus();			//v17.02追加 byやまおか 2010/07/22

            ////起動画面アンロード
            //frmStart.Instance.Close();

            //起動時に回転中心が端に寄っているとエラーになるため仮対策 v16.03/v16.20 追加 by 山影 10-03-12
            CheckRC = !CheckRC;

            //マウスカーソルを元に戻す   'v16.30/v17.00追加 byやまおか 2010/02/23
            Cursor.Current = Cursors.Default;

            //Rev99.99 この後に実行するfrmXrayControl.Instance.QueryWarmupを実行させるために、ここに移動 by長野 2014/11/10
            //追加2014/10/28hata
            CT30KSetup = true; //起動完了

            ////追加2014/10/07hata_v19.51反映
            ////'v19.19 Viscomのときは、フォームが全表示された後にウォームアップ画面を表示する by長野 2013/09/27
            ////'v19.30 条件追加 by長野 2013/10/02 'v19.50 条件修正 by長野 2013/11/12
            ////'If (XrayType = XrayTypeViscom) Or (XrayType = XrayTypeHamaL10801) Or (scaninh.xray_remote = 0) Then
            //if (((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom) || 
            //     (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801)) 
            //    && (CTSettings.scaninh.Data.xray_remote == 0))
            //{
            //    frmCTMenu.Instance.Enabled = false;
			
            //    PauseForDoEvents(3.5f);
			
            //    frmCTMenu.Instance.Enabled = true;

            //    frmXrayControl.Instance.QueryWarmup();
            //}

            //////追加2014/10/28hata
            ////CT30KSetup = true; //起動完了

            ////追加2014/11/07hata 
            //frmCTMenu.Instance.Enabled = true;  //有効にする

            ////追加2014/11/28hata_v19.51_dnet
            //frmCTMenu.Instance.Toolbar1.Focus();

        }


#region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //'
        //'   メール送信  追加 by 間々田 2004/05/13
        //'
        //Public Sub CallSendMail(ByVal ErrCode As Integer)
        //
        //    Dim strRet      As String
        //    Dim strServer   As String
        //    Dim strTo       As String
        //    Dim strFrom     As String
        //    Dim strSubject  As String
        //    Dim strBody     As String
        //    Dim strFile     As String
        //
        //    '件名と本文
        //    Select Case ErrCode
        //        Case 0, 1901
        //            strSubject = LoadResString(13011) 'スキャン正常終了
        //            strBody = LoadResString(13021)    'スキャンが正常終了しました。
        //        Case Else
        //            strSubject = LoadResString(13012) 'スキャン異常終了
        //            strBody = GetErrMessage(ErrCode)
        //    End Select
        //    strBody = strBody & vbCrLf & Format$(Now, "yyyy-mm-dd hh:nn:ss")    '時刻付加
        //
        //    With scansel
        //
        //        'SMTPサーバ名   'タブで区切ってポート番号を指定できます。
        //        strServer = RemoveNull(.smtp_server)
        //
        //        ' 宛先
        //        ' 複数の宛先に送付するときは、アドレスをタブで区切って
        //        ' いくらでも指定できます。
        //        strTo = Replace$(RemoveNull(.address), ";", vbTab)
        //
        //        'CC指定
        //        If RemoveNull(.carbon_copy) <> "" Then
        //            strTo = strTo & vbTab & "cc" & vbTab & Replace$(RemoveNull(.carbon_copy), ";", vbTab)
        //        End If
        //
        //        '送信元
        //        strFrom = RemoveNull(.transmitting_person)
        //
        //    End With
        //
        //    ' ファイルを添付するときは、ファイル名をフルパスで指定します。
        //    ' ファイルを複数指定するときは、タブで区切ってください。
        //    '    szFile = "c:\a1.gif" & vbTab & "c:\a2.jpeg" ' ファイル２個
        //    ' ファイルを添付しないときは次のようにします。
        //    strFile = ""   ' ファイル添付なし
        //
        //    '「メール送信中...」ダイアログフォーム表示          added by 間々田 2004/06/01
        //    frmNowSendMail.Show
        //
        //    strRet = SendMail(strServer, strTo, strFrom, strSubject, strBody, strFile)
        //
        //    ' 送信エラーのときは、戻り値にエラーメッセージが返ります。
        //    If Len(strRet) <> 0 Then MsgBox "Error !" & vbCr & strRet
        //
        //    '「メール送信中...」ダイアログフォーム・アンロード  added by 間々田 2004/06/01
        //    Unload frmNowSendMail
        //
        //End Sub
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

        //追加2014/10/07hata_v19.51反映
        //
        //   ビニングモードの文字列を返します    2003/08/12 by 間々田
        //
	    public static string GetBinningStr(int Mode)
	    {
		    string functionReturnValue = null;

            switch (Mode) {
			    //Case -1                                                'v11.2削除 by 間々田 2006/01/11
			    //    GetBinningStr = LoadResString(12747)    '未完了    'v11.2削除 by 間々田 2006/01/11
			    case 0:
				    functionReturnValue = "1 x 1";
				    break;
			    case 1:
				    functionReturnValue = "2 x 2";
				    break;
			    case 2:
				    functionReturnValue = "4 x 4";
				    break;
			    default:
				    functionReturnValue = " - ";
				    break;
		    }
		    return functionReturnValue;
	    }


        //
        //   I.I.視野の文字列を返します    2003/10/27 by 間々田
        //
        public static string GetIIStr(int Index)
        {
            string functionReturnValue = null;

            if (CTSettings.infdef.Data.iifield.GetLowerBound(0) <= Index && Index <= CTSettings.infdef.Data.iifield.GetUpperBound(0))
            {
                functionReturnValue = modLibrary.RemoveNull(CTSettings.infdef.Data.iifield[Index].GetString());
            }
            else
            {
                functionReturnValue = " - ";
            }

            return functionReturnValue;
        }


        //
        //   現在のフィルタを返します    2005/01/12 by 間々田
        //
        public static string GetFilterStr(int num)
        {
            string functionReturnValue = null;

            switch (num)
            {
                case 0:
                    //変更2014/10/07hata_v19.51反映
                    //'v18.00変更 byやまおか 2011/03/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    //functionReturnValue = CTResources.LoadResString(StringTable.IDS_None);	//なし
                    //break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    //変更2014/10/07hata_v19.51反映
                    //functionReturnValue = "No." + no.ToString();  //'v18.00変更 byやまおか 2011/03/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    functionReturnValue = modLibrary.RemoveNull(CTSettings.infdef.Data.xfilter_c[num].GetString());

                    break;
                default:
                    functionReturnValue = " - ";
                    break;
            }

            return functionReturnValue;
        }


        //追加2014/10/07hata_v19.51反映
        //'
        //'   filtertable.csvからフィルタの文字列を返します   'v18.00追加 byやまおか 2011/08/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        //'
        public static string GetFilterTableStr(int condIndex, int iiIndex)
        {
            float theFltTbl = 0;

            //'変数初期化
            string functionReturnValue = "-1";  // null;

            //'filtertable.csvからフィルタ厚(数値だけ)を取得
            theFltTbl = CTSettings.infdef.Data.filter[iiIndex * 3 + condIndex];

            //'"0"だったら"なし"にする
            if (theFltTbl == 0)
            {
                functionReturnValue = modLibrary.RemoveNull(CTSettings.infdef.Data.xfilter_c[0].GetString());
            }
            //'それ以外は"mm"をつける
            else
            {
                functionReturnValue = Convert.ToString(theFltTbl) + "mm";
            }

            return functionReturnValue;
        }


        //追加2014/10/07hata_v19.51反映
        //'
        //'   現在の焦点の文字を返します    2011/03/11 byやまおか 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        //'
        public static string GetFocusStr(int num)
        {
            //string functionReturnValue = " - ";

            //switch (modXrayControl.XrayType)
            //{
            //    case modXrayControl.XrayTypeConstants.XrayTypeKevex:
            //        switch (num)
            //        {
            //            case 0:
            //                functionReturnValue =CTResources.LoadResString(12138);  //'小
            //                break;
            //            case 1:
            //                functionReturnValue = CTResources.LoadResString(12137); //'大
            //                break;
            //            default:
            //                functionReturnValue = " - ";
            //                break;
            //        }
            //        break;
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02: //追加2014/11/05hata L9181-02に対応
            //        switch (num)
            //        {
            //            case 0:
            //                functionReturnValue =CTResources.LoadResString(12138);  //'小
            //                break;
            //            case 1:
            //                functionReturnValue = CTResources.LoadResString(12132); //'中
            //                break;
            //            case 2:
            //                functionReturnValue = CTResources.LoadResString(12137); //'大
            //                break;
            //            default:
            //                functionReturnValue = " - ";
            //                break;
            //        }
            //        break;
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
            //        switch (num)
            //        {
            //            case 0:
            //                functionReturnValue ="F1";
            //                break;
            //            case 1:
            //                functionReturnValue = "F2";
            //                break;
            //            case 2:
            //                functionReturnValue = "F3";
            //                break;
            //            case 3:
            //                functionReturnValue = "F4"; 
            //                break;
            //            default:
            //                functionReturnValue = " - ";
            //                break;
            //        }
            //        break;
                
            //    //'浜ホト90kV
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
            //    case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
            //        switch (num)
            //        {
            //            case 0:
            //                functionReturnValue =CTResources.LoadResString(12138);  //'小
            //                break;
            //            default:
            //                functionReturnValue = " - ";
            //                break;
            //        }
            //        break;

            //    //'Titan
            //    case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:
            //        switch (num)
            //        {
            //            case 0:
            //            case 1:
            //            case 2:
            //                functionReturnValue = modLibrary.RemoveNull(CTSettings.infdef.Data.focustype[num].GetString());  //'ctinfinit.csvから取得
            //                break;
            //            default:
            //                functionReturnValue = " - ";
            //                break;
            //        }
            //        break;

            //    default:
            //        functionReturnValue = " - ";
            //        break;
            //}
            //return functionReturnValue;
            return "-";
        }


        //変更2014/10/07hata_v19.51反映
        //
        //   現在のＸ線管を返します    2005/01/12 by 間々田
        //
        public static string GetXrayStr(int Index)
        {
            string functionReturnValue = null;

            if (CTSettings.infdef.Data.multi_tube.GetLowerBound(0) == Index)
            {
                functionReturnValue = modLibrary.RemoveNull(CTSettings.infdef.Data.multi_tube[Index].GetString());
            }
            else
            {
                functionReturnValue = " - ";
            }

            return functionReturnValue;

        }

        //変更2014/10/07hata_v19.51反映
        //
        //   回転選択の文字列を返します    2003/10/27 by 間々田
        //
        public static string GetRotateStr(int Mode)
        {
            string functionReturnValue = null;

            switch (Mode)
            {

                //case 0:
                //    functionReturnValue = CTResources.LoadResString(StringTable.IDS_Table);
                //    //テーブル
                //    break;
                //case 1:
                //    functionReturnValue = CTResources.LoadResString(StringTable.IDS_XrayTube);
                //    //Ｘ線管
                //    break;
                //default:
                //    functionReturnValue = " - ";
                //    break;

            }
            return functionReturnValue;

        }

        //
        //   FPDのゲインを返します      'v16.2/17.00追加 byやまおか 2010/02/17
        //
        public static string GetFpdGainStr(int Mode)
        {
            string functionReturnValue = null;

            switch (Mode)
            {
                case 0:
                    functionReturnValue = "0.25";
                    break;
                case 1:
                    functionReturnValue = "0.5";
                    break;
                case 2:
                    functionReturnValue = "1";
                    break;
                case 3:
                    functionReturnValue = "2";
                    break;
                case 4:
                    functionReturnValue = "4";
                    break;
                case 5:
                    functionReturnValue = "8";
                    break;
                default:
                    functionReturnValue = " - ";

#if NoCamera    //v17.48/v17.53デバグ用 byやまおか 'v19.50 条件修正 by長野 2013/12/16
//#if !NoCamera    //v17.48/v17.53デバグ用 byやまおか
                    functionReturnValue = "99";
#endif
                    break;
            }

            return functionReturnValue;
        }


        //
        //   FPDの積分時間を返します      'v16.2/17.00追加 byやまおか 2010/02/17
        //
        public static string GetFpdIntegStr(int Mode)
        {
            //Select Case Mode
            //
            //    Case 0
            //        GetFpdIntegStr = "66"
            //    Case 1
            //        GetFpdIntegStr = "82"
            //    Case 2
            //        GetFpdIntegStr = "99"
            //    Case 3
            //        GetFpdIntegStr = "124"
            //    Case 4
            //        GetFpdIntegStr = "166"
            //    Case 5
            //        GetFpdIntegStr = "249"
            //    Case 6
            //        GetFpdIntegStr = "499"
            //    Case 7
            //        GetFpdIntegStr = "999"
            //    Case Else
            //        GetFpdIntegStr = " - "
            //
            //End Select

            //積分時間はPkeFPDから取得する   'v17.10変更 byやまおか 2010/08/24


            //変更2014/10/07hata_v19.51反映
            ////引数のチェック(通常は0～7の8段階)
            //if ((Mode > FPD_INTEGLIST_MAX - 1)) Mode = FPD_INTEGLIST_MAX - 1;
            //if ((Mode < 0)) Mode = 0;

            //return ((int)(fpd_integlist[Mode] - 0.5)).ToString("0");		//少数以下切り捨て

            string functionReturnValue = null;

            //'コモン初期化後の表示を改善するため修正     'v18.00変更 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (Mode > FPD_INTEGLIST_MAX - 1)
            {
                //Rev99.99 -0.5は不要 by長野 2014/11/10
                //functionReturnValue = ((int)(fpd_integlist[FPD_INTEGLIST_MAX - 1] - 0.5)).ToString("0"); //'少数以下切り捨て
                functionReturnValue = ((int)(fpd_integlist[FPD_INTEGLIST_MAX - 1])).ToString("0"); //'少数以下切り捨て
            }
            else if (Mode < 0)
            {
                functionReturnValue = " - ";
            }
            else
            {
                //Rev99.99 -0.5は不要 by長野 2014/11/10
                //functionReturnValue = ((int)(fpd_integlist[Mode] - 0.5)).ToString("0");		//少数以下切り捨て
                functionReturnValue = ((int)(fpd_integlist[Mode])).ToString("0");		//少数以下切り捨て
            }
            return functionReturnValue;
        }


        //
        //   FPDの積分時間を返します      'v17.10追加 byやまおか 2010/08/24
        //
        public static double GetFpdIntegDouble(int Mode)
        {
            //引数のチェック(通常は0～7の8段階)
            if ((Mode > FPD_INTEGLIST_MAX - 1)) Mode = FPD_INTEGLIST_MAX - 1;
            if ((Mode < 0)) Mode = 0;

            return fpd_integlist[Mode];
        }


        //
        //   ビニングモード変更時処理            'v7.0 added by 間々田 2003/09/26
        //
        public static void GetBinning()
        {
//            //ビニングモードを求める
//            //binning = GetCommonLong("scansel", "binning")              'v11.4削除 by 間々田 2006/03/13

//            //hm の取得
//            //hm = GetCommonFloat("scancondpar", "h_mag[" & CStr(binning) & "]")
//            CTSettings.detectorParam.hm = CTSettings.scancondpar.Data.h_mag[CTSettings.scansel.Data.binning];			    //v11.4変更 by 間々田 2006/03/13

//            //vm の取得
//            //vm = GetCommonFloat("scancondpar", "v_mag[" & CStr(binning) & "]")
//            CTSettings.detectorParam.vm = CTSettings.scancondpar.Data.v_mag[CTSettings.scansel.Data.binning];			    //v11.4変更 by 間々田 2006/03/13

//            //phm の取得
//            //phm = GetCommonFloat("scancondpar", "pulsar_h_mag[" & CStr(binning) & "]")
//            CTSettings.detectorParam.phm = CTSettings.scancondpar.Data.pulsar_h_mag[CTSettings.scansel.Data.binning];		//v11.4変更 by 間々田 2006/03/13

//            //pvm の取得
//            //pvm = GetCommonFloat("scancondpar", "pulsar_v_mag[" & CStr(binning) & "]")
//            CTSettings.detectorParam.pvm = CTSettings.scancondpar.Data.pulsar_v_mag[CTSettings.scansel.Data.binning];		//v11.4変更 by 間々田 2006/03/13

//#if DebugOn         
//            //v17.02追加 byやまおか 2010/07/08
//            if ((CTSettings.scancondpar.Data.v_size / CTSettings.detectorParam.pvm >= 1024))
//            {
//                CTSettings.detectorParam.phm = CTSettings.detectorParam.phm * 2;
//                CTSettings.detectorParam.pvm = CTSettings.detectorParam.pvm * 2;
//            }
//#endif

//            //fphm の取得
//            //fphm = GetCommonFloat("scancondpar", "fpulsar_h_mag[" & CStr(binning) & "]")
//            CTSettings.detectorParam.fphm = CTSettings.scancondpar.Data.fpulsar_h_mag[CTSettings.scansel.Data.binning];			//v11.4変更 by 間々田 2006/03/13

//            //fpvm の取得
//            //fpvm = GetCommonFloat("scancondpar", "fpulsar_v_mag[" & CStr(binning) & "]")
//            CTSettings.detectorParam.fpvm = CTSettings.scancondpar.Data.fpulsar_v_mag[CTSettings.scansel.Data.binning];			//v11.4変更 by 間々田 2006/03/13

//            //FR(0)の取得
//            //FR(0) = GetCommonFloat("scancondpar", "frame_rate[0][" & CStr(binning) & "]")
//            //FR[0] = CTSettings.scancondpar.Data.frame_rate[CTSettings.scansel.Data.binning, 0];			//v11.4変更 by 間々田 2006/03/13
//            CTSettings.detectorParam.FR[0] = CTSettings.scancondpar.Data.frame_rate[CTSettings.scansel.Data.binning + 3 * 0];			//v11.4変更 by 間々田 2006/03/13

//            //FR(1)の取得
//            //FR(1) = GetCommonFloat("scancondpar", "frame_rate[1][" & CStr(binning) & "]")
//            //FR[1] = CTSettings.scancondpar.Data.frame_rate[CTSettings.scansel.Data.binning, 1];			//v11.4変更 by 間々田 2006/03/13
//            CTSettings.detectorParam.FR[1] = CTSettings.scancondpar.Data.frame_rate[CTSettings.scansel.Data.binning + 3 * 1];			//v11.4変更 by 間々田 2006/03/13

//            //DCF(0)の取得
//            //dcf(0) = GetCommonStr("scancondpar", "dcf[0][" & CStr(binning) & "]")
//            //dcf[0] = modLibrary.RemoveNull(CTSettings.scancondpar.Data.dcf[CTSettings.scansel.Data.binning, 0]);		//v11.4変更 by 間々田 2006/03/13
//            CTSettings.detectorParam.dcf[0] = modLibrary.RemoveNull(CTSettings.scancondpar.Data.dcf[CTSettings.scansel.Data.binning + 3 * 0].GetString());		//v11.4変更 by 間々田 2006/03/13

//            //DCF(1)の取得
//            //dcf(1) = GetCommonStr("scancondpar", "dcf[1][" & CStr(binning) & "]")
//            //dcf[1] = modLibrary.RemoveNull(CTSettings.scancondpar.Data.dcf[CTSettings.scansel.Data.binning, 1]);		//v11.4変更 by 間々田 2006/03/13
//            CTSettings.detectorParam.dcf[1] = modLibrary.RemoveNull(CTSettings.scancondpar.Data.dcf[CTSettings.scansel.Data.binning * 3 + 1].GetString());		//v11.4変更 by 間々田 2006/03/13

//            //H_SIZE の取得
//            //h_size = GetCommonLong("scancondpar", "h_size") / hm
//            //2014/11/13hata キャストの修正
//            //CTSettings.detectorParam.h_size = (int)(CTSettings.scancondpar.Data.h_size / CTSettings.detectorParam.hm);			//v11.4変更 by 間々田 2006/03/13
//            CTSettings.detectorParam.h_size = Convert.ToInt32(CTSettings.scancondpar.Data.h_size / CTSettings.detectorParam.hm);			//v11.4変更 by 間々田 2006/03/13
//            //Call putcommon_long("scancondpar", "fimage_hsize", h_size) 'v11.4削除 by 間々田 2006/03/13

//            //V_SIZE の取得
//            //v_size = GetCommonLong("scancondpar", "v_size") / vm
//            //2014/11/13hata キャストの修正
//            //CTSettings.detectorParam.v_size = (int)(CTSettings.scancondpar.Data.v_size / CTSettings.detectorParam.vm);			//v11.4変更 by 間々田 2006/03/13
//            CTSettings.detectorParam.v_size = Convert.ToInt32(CTSettings.scancondpar.Data.v_size / CTSettings.detectorParam.vm);			//v11.4変更 by 間々田 2006/03/13
//            //Call putcommon_long("scancondpar", "fimage_vsize", v_size) 'v11.4削除 by 間々田 2006/03/13

//            //v11.4追加ここから by 間々田 2006/03/13
//            //ScanCorrect.hsize = CTSettings.detectorParam.h_size;
//            //ScanCorrect.vsize = CTSettings.detectorParam.v_size;
//            CTSettings.scancondpar.Data.fimage_hsize = CTSettings.detectorParam.h_size;
//            CTSettings.scancondpar.Data.fimage_vsize = CTSettings.detectorParam.v_size;

//            //modScancondpar.CallPutScancondpar();//v11.4追加ここまで by 間々田 2006/03/13
//            CTSettings.scancondpar.Write();

//            //ビニングモード別校正画像ファイル
//            ScanCorrect.GAIN_CORRECT = ScanCorrect.CORRECT_PATH + "Gain" + CTSettings.scansel.Data.binning.ToString() + ".cor";		//ゲイン
//            ScanCorrect.GAIN_CORRECT_SFT = ScanCorrect.CORRECT_PATH + "GainS" + CTSettings.scansel.Data.binning.ToString() + ".cor";  //'ゲイン(シフト用)  'v18.00追加 byやまおか 2011/02/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            
//            //ScanCorrect.GAIN_CORRECT_L = ScanCorrect.CORRECT_PATH + "Gain_L.cor";			//ゲイン(LONG型用    'v17.00added by 山本 2009-10-19
//            ScanCorrect.GAIN_CORRECT_L = ScanCorrect.CORRECT_PATH + "GainL" + CTSettings.scansel.Data.binning.ToString() + ".cor";          //'ゲイン(LONG型用)               'v18.00変更 byやまおか 2011/02/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
//            ScanCorrect.GAIN_CORRECT_L_SFT = ScanCorrect.CORRECT_PATH + "GainLS" + CTSettings.scansel.Data.binning.ToString() + ".cor";     //'ゲイン(LONG型用)(シフト用) 'v18.00追加 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

//            ScanCorrect.GAIN_CORRECT_AIR = ScanCorrect.CORRECT_PATH + "Gain_air.cor";		//追加 ゲイン校正後に撮影したエアーの画像 'v19.00 by 長野 2012-05-10

//            ScanCorrect.OFF_CORRECT = ScanCorrect.CORRECT_PATH + "Offset" + CTSettings.scansel.Data.binning.ToString() + ".cor";		//オフセット

//            ScanCorrect.DEF_CORRECT = ScanCorrect.CORRECT_PATH + "Def_IMAGE" + CTSettings.scansel.Data.binning.ToString() + ".cor";  //欠陥画像ファイル

//            //追加2014/10/07hata_v19.51反映    
//            //'v19.50 v19.41とv18.02の統合
//            CTSettings.detectorParam.Gain_Correct_File = ScanCorrect.GAIN_CORRECT;
//            CTSettings.detectorParam.Gain_Correct_Sft_File = ScanCorrect.GAIN_CORRECT_SFT;
//            CTSettings.detectorParam.Gain_Correct_L_File = ScanCorrect.GAIN_CORRECT_L;
//            CTSettings.detectorParam.Gain_Correct_L_Sft_File = ScanCorrect.GAIN_CORRECT_L_SFT;
//            CTSettings.detectorParam.Gain_Correct_Aire_File = ScanCorrect.GAIN_CORRECT_AIR;
//            CTSettings.detectorParam.Off_Correct_File = ScanCorrect.OFF_CORRECT;
//            CTSettings.detectorParam.Def_Correct_File = ScanCorrect.DEF_CORRECT; 

//            //校正処理の初期値設定       'added by 間々田 2003/10/29
//            //繰り上げる //2014/07/07(検S1)hata
//            //modScanCorrect.IntegNumAtVer = (int)(CTSettings.detectorParam.FR[0] * 4);
//            //ScanCorrect.IntegNumAtOff = (int)(CTSettings.detectorParam.FR[0] * 4);
//            //ScanCorrect.IntegNumAtPos = (int)(CTSettings.detectorParam.FR[0] * 4);
//            modScanCorrect.IntegNumAtVer = (int)(CTSettings.detectorParam.FR[0] + 0.5) * 4;
//            ScanCorrect.IntegNumAtOff = (int)(CTSettings.detectorParam.FR[0] + 0.5) * 4;
//            ScanCorrect.IntegNumAtPos = (int)(CTSettings.detectorParam.FR[0] + 0.5) * 4;
 
//            //    ViewNumAtRot = Int((FR(0) * 40 + 99) * 0.01) / 0.01 '10の位で切り上げをする
//            //2014/11/13hata キャストの修正
//            //ScanCorrect.ViewNumAtRot = (int)((int)((CTSettings.detectorParam.FR[0] * 20 + 99) * 0.01) / 0.01);			//10の位で切り上げをする    '修正 2003/10/31 by 間々田
//            ScanCorrect.ViewNumAtRot = Convert.ToInt32(Math.Floor((CTSettings.detectorParam.FR[0] * 20 + 99) * 0.01) / 0.01);			//10の位で切り上げをする    '修正 2003/10/31 by 間々田

//            //v10.0追加 by 間々田 2005/01/24
//            //繰り上げる //2014/07/07(検S1)hata
//            //modScanCorrect.ViewNumAtGain = (int)(CTSettings.detectorParam.FR[0] * 20);		//ゲイン校正時のビュー数
//            modScanCorrect.ViewNumAtGain = (int)(CTSettings.detectorParam.FR[0] + 0.5) * 20;		//ゲイン校正時のビュー数

//            modScanCorrect.IntegNumAtGain = 1;			            //ゲイン校正時の積算枚数


//            //'v19.50 FPDの場合、オフセットのデフォルト枚数をビュー数と同じ。
//            //'スキャン位置構成の積算枚数を半分にする by長野 2014/01/28
//            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
//            {           
//                //2014/11/13hata キャストの修正
//                //ScanCorrect.IntegNumAtPos = ScanCorrect.IntegNumAtPos / 2;
//                //ScanCorrect.IntegNumAtOff = (int)(Math.Round(modScanCorrect.ViewNumAtGain / 100d, 0, MidpointRounding.AwayFromZero) * 100);
//                ScanCorrect.IntegNumAtPos = Convert.ToInt32(ScanCorrect.IntegNumAtPos / 2F);
//                ScanCorrect.IntegNumAtOff = Convert.ToInt32(Math.Round(modScanCorrect.ViewNumAtGain / 100D) * 100);
                
//                //'IntegNumAtOffが0以下になった場合を追加 by長野 2014/02/24
//                if (ScanCorrect.IntegNumAtOff <= 0)
//                {
//                    ScanCorrect.IntegNumAtOff = 10;
//                }
//            }
            


        }


        //
        //   テキストボックスバイト数チェック（全角文字対応版）              '新規作成 2003/11/19 by 間々田
        //
        public static void ChangeTextBox(TextBox theTextBox)
        {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
            Dim ansiStr     As String
    
            With theTextBox
    
                'イベントの連鎖防止
                If Not .Enabled Then Exit Sub
    
                'バイト数チェックのためansiコードに変換する
                ansiStr = StrConv(.Text, vbFromUnicode)
    
                'バイト数がMaxLengthを超えたら
                If LenB(ansiStr) > .MaxLength Then
        
                    'メッセージボックスの表示：%1文字を越えています。
                    MsgBox GetResString(9318, CStr(.MaxLength)), vbExclamation
            
                    '強制的にMaxLengthバイトにする。unicodeに戻す
                    .Enabled = False
                    .Text = StrConv(LeftB(ansiStr, .MaxLength), vbUnicode)
                    .Enabled = True
                
                End If
        
            End With
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            //イベントの連鎖防止
            if (!theTextBox.Enabled) return;

            string tbText = theTextBox.Text;

            //バイト数がMaxLengthを超えたら
            if (Encoding.Default.GetByteCount(tbText) > theTextBox.MaxLength)
            {
                ////メッセージボックスの表示：%1文字を越えています。
                //MessageBox.Show(StringTable.GetResString(9318, theTextBox.MaxLength.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                ////強制的にMaxLengthバイトにする。
                //string cutText = Encoding.Default.GetString(Encoding.Default.GetBytes(tbText), 0, theTextBox.MaxLength);

                //if (cutText[cutText.Length - 1] != tbText[cutText.Length - 1])
                //{
                //    cutText = cutText.Substring(0, cutText.Length - 1);
                //}

                //theTextBox.Enabled = false;
                //theTextBox.Text = cutText;
                //theTextBox.Enabled = true;
            }
		}


        //*****************************************************************************
        //   Function名    ErrMessage(errcode As Long, buttons As Variant)
        //   機能
        //       ｴﾗｰﾒｯｾｰｼﾞ表示
        //
        //       errcode         ｴﾗｰｺｰﾄﾞ番号
        //       buttons         ｴﾗｰﾒｯｾｰｼﾞといっしょに表示するﾎﾞﾀﾝの種類（MsgBox関数と同様）
        //
        //   変更履歴
        //       J.IWASAWA       '97.3.10     初版
        //       J.IWASAWA       '97.10.01    ｴﾗｰｺｰﾄﾞも表示
        //       J.IWASAWA       '97.11.04    標準MsgBoxを使用するように変更
        //       間々田          2002.10.21   BEEPを実行する。
        //                                    ボタンのデフォルト値を設定。
        //                                    配列およびsplit関数を使用。
        //       間々田          2003/07/11   リソース対応
        //*****************************************************************************
        public static int ErrMessage(int ErrCode)
        {
            return ErrMessage(ErrCode, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
        }

        public static int ErrMessage(int ErrCode, MessageBoxButtons Buttons = MessageBoxButtons.OK, MessageBoxIcon Icon = MessageBoxIcon.None, MessageBoxDefaultButton DefaultButton = MessageBoxDefaultButton.Button1)
        {
            int functionReturnValue = 0;

            int i = 0;
            string strErrorCode = null;
            string strMsg = null;
            string[] strCell = null;			//@で区切られた要素を格納する文字列型配列

            //リソース取得に失敗した場合の表示文字
            strErrorCode = "Error Code : ";
            strMsg = "Unknown Error";

            //エラーハンドラの指定
            try
            {
                ////ｴﾗｰｺｰﾄﾞ：/未定義のエラーです。
                //strErrorCode = CTResources.LoadResString(StringTable.IDS_ErrorNum) + " ";	//ｴﾗｰｺｰﾄﾞ：
                //strMsg = CTResources.LoadResString(StringTable.IDS_UnkownError);			//予想外のエラーが発生しました。

                ////ErrMessageがサポートするErrCodeは4桁までの数字である
                //if (ErrCode >= 10000) throw new Exception();

                ////指定したエラーコードに対応する文字列をリソースから取得→@で配列に分ける
                //strCell = CTResources.LoadResString(ErrCode).Split('@');

                //strMsg = strCell[0];
                //for (i = 1; i < strCell.Length; i++)
                //{
                //    strMsg = strMsg + "\r\n"
                //           + CTResources.LoadResString(Convert.ToInt32(strCell[i]));
                //}

                //v14.24追加 メカ軸名称を含むエラーメッセージの場合、メカ軸名称をコモンを参考にして表示する by 間々田 2009/03/10
                //   この変更に伴い、下記リソース番号（および8120～8125）の文字列を以下のように変更した
                //       "Ｘテーブル" → "%1テーブル"
                //       "Ｙテーブル" → "%1テーブル"
                switch (ErrCode)
                {
                    case 642:
                    case 647:
                    case 653:
                    case 665:
                    case 666:
                    case 667:
                    case 668:
                    case 669:
                    case 670:
                    case 671:
                    case 672:
                    case 673:
                    case 674:
                    case 675:
                    case 693:
                    case 696:
                        strMsg = strMsg.Replace("%1", modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[1].GetString()));		//光軸と垂直方向
                        break;
                    case 643:
                    case 648:
                    case 654:
                    case 676:
                    case 677:
                    case 678:
                    case 679:
                    case 680:
                    case 681:
                    case 682:
                    case 683:
                    case 684:
                    case 685:
                    case 686:
                    case 694:
                    case 697:
                        strMsg = strMsg.Replace("%1", modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[0].GetString()));      //光軸方向                
                        break;
                }

                //メッセージボックス表示
                DialogResult dialogResult = MessageBox.Show(strErrorCode + ErrCode.ToString() + "\r\n" + "\r\n" + strMsg, Application.ProductName, Buttons, Icon, DefaultButton);
                switch (dialogResult)
                {
                    case DialogResult.OK:
                        functionReturnValue = 1;
                        break;
                    case DialogResult.Cancel:
                        functionReturnValue = 2;
                        break;
                    case DialogResult.Abort:
                        functionReturnValue = 3;
                        break;
                    case DialogResult.Retry:
                        functionReturnValue = 4;
                        break;
                    case DialogResult.Ignore:
                        functionReturnValue = 5;
                        break;
                    case DialogResult.Yes:
                        functionReturnValue = 6;
                        break;
                    case DialogResult.No:
                        functionReturnValue = 7;
                        break;
                    default:
                        functionReturnValue = 0;
                        break;
                }
            }
            //リソース取得時エラー
            catch
            {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//              Err().Clear();
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                ////    ErrMessage = MsgBox(strErrorCode & CStr(ErrCode) & vbCrLf & vbCrLf & strMsg, Buttons)
                ////changed by 山本　2004-8-17　無限にエラーが発生する場合は強制終了させる

                //DialogResult dialogResult = MessageBox.Show(strErrorCode + ErrCode.ToString() + "\r\n" + "\r\n"
                //                                            + strMsg + CTResources.LoadResString(9902),
                //                                            Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                //if (dialogResult == DialogResult.Yes) Environment.Exit(0);
            }

            // append by 間々田 2003/07/11 End

            return functionReturnValue;
        }


        //
        //   エラーメッセージの取得 by 間々田 2004/05/13 (メール送信機能関連)
        //
        public static string GetErrMessage(int ErrCode)
        {
            string functionReturnValue = null;

            int i = 0;
            string strErrorCode = null;
            string strMsg = null;
            string[] strCell = null;			//@で区切られた要素を格納する文字列型配列

            //リソース取得に失敗した場合の表示文字
            strErrorCode = "Error Code : ";
            strMsg = "Unknown Error";

            //エラーハンドラの指定
            try
            {
                ////ｴﾗｰｺｰﾄﾞ：/未定義のエラーです。
                //strErrorCode = CTResources.LoadResString(StringTable.IDS_ErrorNum) + " ";	//ｴﾗｰｺｰﾄﾞ：
                //strMsg = CTResources.LoadResString(StringTable.IDS_UnkownError);			//予想外のエラーが発生しました。

                ////ErrMessageがサポートするErrCodeは4桁までの数字である
                //if (ErrCode >= 10000) throw new Exception();

                ////指定したエラーコードに対応する文字列をリソースから取得→@で配列に分ける
                //strCell = CTResources.LoadResString(ErrCode).Split('@');

                //strMsg = strCell[0];
                //for (i = 1; i <= strCell.GetUpperBound(0); i++)
                //{
                //    strMsg = strMsg + "\r\n" + CTResources.LoadResString(Convert.ToInt32(strCell[i]));
                //}

                //メッセージボックス表示
                functionReturnValue = strErrorCode + ErrCode.ToString() + "\r\n" + "\r\n" + strMsg;
            }
            //リソース取得時エラー
            catch
            {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//              Err().Clear();
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                functionReturnValue = strErrorCode + ErrCode.ToString() + "\r\n" + "\r\n" + strMsg;
            }

            // append by 間々田 2003/07/11 End

            return functionReturnValue;
        }


        //
        //   一定時間待つ    新規作成 by 間々田 2003/11/20
        //
        public static void PauseForDoEvents(float PauseTime)
        {
            int StartTime = 0;

            //開始時間を設定
            StartTime = Winapi.GetTickCount();

            //一定時間待つ
            while ((Winapi.GetTickCount() < StartTime + PauseTime * 1000))
            {
            
                Application.DoEvents();
                //System.Threading.Thread.Sleep(1);		//秒
            }

        }


        //
        //   一定時間待つ(負荷軽減のためSleepを入れる)
        //
        //   'v17.72/v19.02追加 byやまおか 2012/05/14
        //
        public static void PauseForDoEventsSleep(float PauseTime, float SleepInterval = 0.005F)
        {
            int StartTime = 0;

            //開始時間を設定
            StartTime = Winapi.GetTickCount();

            //一定時間待つ
            while ((Winapi.GetTickCount() < StartTime + PauseTime * 1000))
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep((int)(SleepInterval * 1000));		//秒
            }

        }


        private static void GetSlicePlanParameter(string FileName, ref int theSlice, ref int theView, ref int theK)
        {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//          Dim fileNo      As Integer
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            string strREC = null;			//レコード
            string[] strCell = null;		//,で区切られた要素を格納する文字列型配列
            bool GetParam = false;

            GetParam = false;

            StreamReader sr = null;

            //エラー時の扱い
            try
            {

                //ファイルオープン
                //変更2015/01/22hata
                //sr = new StreamReader(FileName);
                sr = new StreamReader(FileName, Encoding.GetEncoding("shift-jis"));

                while ((strREC = sr.ReadLine()) != null)    //１行読み込み
                {
                    if (!string.IsNullOrEmpty(strREC))
                    {
                        //文字型配列に分ける
                        strCell = strREC.Split(',');

                        //最初のパラメータを採用
                        double parseValue = 0;
                        if (double.TryParse(strCell[0], out parseValue) && (!GetParam))
                        {
                            int.TryParse(strCell[7], out theView);				//ビュー数
                            int.TryParse(strCell[17], out theK);				//コーンスライス枚数
                            GetParam = true;
                        }
                        ////レコード先頭列が"スライス数"なら
                        //else if (strCell[0].Trim() == CTResources.LoadResString(StringTable.IDS_SliceNum))	//スライス数
                        //{
                        //    int.TryParse(strCell[1], out theSlice);
                        //    break;
                        //}
                    }
                }
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr = null;
                }
            }
        }


        //
        //   Fcd offset配列のIndex値を求める added by 間々田 2004/02/03
        //
        public static int GetFcdOffsetIndex()
        {
            int functionReturnValue = 0;

            if (CTSettings.scaninh.Data.multi_tube == 0)
            {
                functionReturnValue = CTSettings.scansel.Data.multi_tube;
            }
            else if (CTSettings.scaninh.Data.rotate_select == 0)
            {
                functionReturnValue = CTSettings.scansel.Data.rotate_select;
            }
            else
            {
                functionReturnValue = 0;
            }

            return functionReturnValue;
        }


        //
        //   iniファイルの読み込み   'v11.5追加 by 間々田 2006/08/25
        //
        public static bool GetCtIni()
        {
            //string dummy = null;
            //StringBuilder dummy;

            //戻り値初期化
            bool functionReturnValue = false;

            //パスの設定
            //iniファイルの読み込み
            //ここでVB(IniPath相当)をコンストラクタで実行される
            if (!CTSettings.iniValue.Load(AppValue.IniFileName))
            {
                ////メッセージ表示：～が見つかりません。
                //MessageBox.Show(StringTable.GetResString(StringTable.IDS_NotFound, AppValue.IniFileName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //return functionReturnValue;
            }


            //以下はCTSettings.iniValue.Loadで行う
            ////iniファイル
            ////if (!modFileIO.FSO.FileExists(modFileIO.IniFileName))
            //if (!File.Exists(AppValue.IniFileName))
            //{
            //    //メッセージ表示：～が見つかりません。
            //    MessageBox.Show(StringTable.GetResString(StringTable.IDS_NotFound, AppValue.IniFileName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return functionReturnValue;
            //}

            ////イメージプロ実行ファイル名取得
            ////dummy = new string((char)0, 255);			            //ヌルクリア
            //dummy = new StringBuilder(255);

            //modDeclare.GetPrivateProfileString("Image-Pro Plus", "ExeFileName", "", dummy, (uint)dummy.Length, AppValue.IniFileName);	//Iniファイル読み込み用APIの呼び出し
            //ImageProExe = modLibrary.RemoveNull(dummy.ToString());			    //ヌル以降削除

            ////イメージプロ起動待ち時間取得
            ////dummy = new string((char)0, 255);			            //ヌルクリア
            //dummy = new StringBuilder(255);
            //modDeclare.GetPrivateProfileString("Image-Pro Plus", "PauseTime", "1", dummy, (uint)dummy.Length, AppValue.IniFileName);		//Iniファイル読み込み用APIの呼び出し
            //float.TryParse(dummy.ToString(), out ImageProPauseTime);			//数値に変換
            //if (!(ImageProPauseTime > 0)) ImageProPauseTime = 1;    //値の調整

            ////CT30K起動時にイメージプロを起動するか
            ////dummy = new string((char)0, 255);			            //ヌルクリア
            //dummy = new StringBuilder(255);
            //modDeclare.GetPrivateProfileString("Image-Pro Plus", "Startup", "1", dummy, (uint)dummy.Length, AppValue.IniFileName);		//Iniファイル読み込み用APIの呼び出し
            //double parseValue = 0;
            //double.TryParse(dummy.ToString(), out parseValue);
            //ImageProStartup = (parseValue != 0);

            //#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            ////    'Ｘ線ドライバ（Viscom）のバージョン     'v12.01追加 by 間々田 2006/12/13
            ////    dummy = String$(255, 0)                                                                             'ヌルクリア
            ////    Call GetPrivateProfileString("Viscom Driver", "DllVersion", "", dummy, Len(dummy), IniFileName)      'Iniファイル読み込み用APIの呼び出し
            ////    DLLVERSION = Val(dummy)
            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            //#endregion

            ////v13.0追加ここから ダブルオブリーク実行ファイルのパスもここで取得する 2007/03/28 by 間々田
            ////modDoubleOblique.DoubleObliquePath = new string((char)0, 255);
            ////modDeclare.GetPrivateProfileString("DoubleOblique", "ExeFileName", "", modDoubleOblique.DoubleObliquePath, modDoubleOblique.DoubleObliquePath.Length, AppValue.IniFileName);	//追加 by 間々田 2005/05/14
            ////modDoubleOblique.DoubleObliquePath = modLibrary.RemoveNull(modDoubleOblique.DoubleObliquePath);
            //dummy = new StringBuilder(255);
            //modDeclare.GetPrivateProfileString("DoubleOblique", "ExeFileName", "", dummy, (uint)dummy.Length, AppValue.IniFileName);	//追加 by 間々田 2005/05/14
            //modDoubleOblique.DoubleObliquePath = modLibrary.RemoveNull(dummy.ToString());
            ////v13.0追加ここまで

            //#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            ////    'X線警告音WAVファイルパスを取得 v16.01 追加 by 山影 10-02-23
            ////    WavWarningPath = String$(255, 0)
            ////    Call GetPrivateProfileString("WavWarning", "WavFileName", "", WavWarningPath, Len(WavWarningPath), IniFileName) 'Iniファイル読み込み用APIの呼び出し
            ////    WavWarningPath = RemoveNull(WavWarningPath)                                                                     'ヌル以降削除
            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            //#endregion

            ////CT30Kメッセージ受信用ポート番号：デフォルトは7010                              'v17.50追加 2011/01/20 by 間々田
            //CT30KPort = modDeclare.GetPrivateProfileInt("SocketPort", "CT30KPort", 7010, AppValue.IniFileName);

            ////段階ウォームアップ設定パラメータを取得     'v17.72/v19.02追加 byやまおか 2012/05/16
            ////ステップ数
            ////dummy = new string((char)0, 255);			                                    //ヌルクリア
            //dummy = new StringBuilder(255);
            //modDeclare.GetPrivateProfileString("StepWarmUP", "StepNUM", "1", dummy, (uint)dummy.Length, AppValue.STEPWUP_INI);	//Iniファイル読み込み用APIの呼び出し
            //int.TryParse(dummy.ToString(), out CTSettings.iniValue.STEPWU_NUM);			                    //数値に変換
            //if ((modXrayControl.STEPWU_NUM < 1)) modXrayControl.STEPWU_NUM = 1;			    //値の調整
            //if ((modXrayControl.STEPWU_NUM > 5)) modXrayControl.STEPWU_NUM = 5;			    //値の調整

            ////第1段階
            ////dummy = new string((char)0, 255);			                                    //ヌルクリア
            //dummy = new StringBuilder(255);
            //modDeclare.GetPrivateProfileString("StepWarmUP", "StepKV1", "1", dummy, (uint)dummy.Length, AppValue.STEPWUP_INI);	//Iniファイル読み込み用APIの呼び出し
            //int.TryParse(dummy.ToString(), out modXrayControl.STEPWU_KV[1]);			                //数値に変換
            //if ((modXrayControl.STEPWU_KV[1] < 1)) modXrayControl.STEPWU_KV[1] = 1;			//値の調整
            //if ((modXrayControl.STEPWU_KV[1] > 1000)) modXrayControl.STEPWU_KV[1] = 1000;	//値の調整

            ////第2段階
            ////dummy = new string((char)0, 255);			                                    //ヌルクリア
            //dummy = new StringBuilder(255);
            //modDeclare.GetPrivateProfileString("StepWarmUP", "StepKV2", "1", dummy, (uint)dummy.Length, AppValue.STEPWUP_INI);	//Iniファイル読み込み用APIの呼び出し
            //int.TryParse(dummy.ToString(), out modXrayControl.STEPWU_KV[2]);			                //数値に変換
            //if ((modXrayControl.STEPWU_KV[2] < 1)) modXrayControl.STEPWU_KV[2] = 1;			//値の調整
            //if ((modXrayControl.STEPWU_KV[2] > 1000)) modXrayControl.STEPWU_KV[2] = 1000;	//値の調整

            ////第3段階
            ////dummy = new string((char)0, 255);			                                    //ヌルクリア
            //dummy = new StringBuilder(255);
            //modDeclare.GetPrivateProfileString("StepWarmUP", "StepKV3", "1", dummy, (uint)dummy.Length, AppValue.STEPWUP_INI);	//Iniファイル読み込み用APIの呼び出し
            //int.TryParse(dummy.ToString(), out modXrayControl.STEPWU_KV[3]);			                //数値に変換
            //if ((modXrayControl.STEPWU_KV[3] < 1)) modXrayControl.STEPWU_KV[3] = 1;			//値の調整
            //if ((modXrayControl.STEPWU_KV[3] > 1000)) modXrayControl.STEPWU_KV[3] = 1000;	//値の調整

            ////第4段階
            ////dummy = new string((char)0, 255);			                                    //ヌルクリア
            //dummy = new StringBuilder(255);
            //modDeclare.GetPrivateProfileString("StepWarmUP", "StepKV4", "1", dummy, (uint)dummy.Length, AppValue.STEPWUP_INI);	//Iniファイル読み込み用APIの呼び出し
            //int.TryParse(dummy.ToString(), out modXrayControl.STEPWU_KV[4]);			                //数値に変換
            //if ((modXrayControl.STEPWU_KV[4] < 1)) modXrayControl.STEPWU_KV[4] = 1;			//値の調整
            //if ((modXrayControl.STEPWU_KV[4] > 1000)) modXrayControl.STEPWU_KV[4] = 1000;	//値の調整

            ////第5段階
            ////dummy = new string((char)0, 255);			                                    //ヌルクリア
            //dummy = new StringBuilder(255);
            //modDeclare.GetPrivateProfileString("StepWarmUP", "StepKV5", "1", dummy, (uint)dummy.Length, AppValue.STEPWUP_INI);	//Iniファイル読み込み用APIの呼び出し
            //int.TryParse(dummy.ToString(), out modXrayControl.STEPWU_KV[5]);			                //数値に変換
            //if ((modXrayControl.STEPWU_KV[5] < 1)) modXrayControl.STEPWU_KV[5] = 1;			//値の調整
            //if ((modXrayControl.STEPWU_KV[5] > 1000)) modXrayControl.STEPWU_KV[5] = 1000;	//値の調整

            //戻り値セット
            functionReturnValue = true;
            return functionReturnValue;
        }


        //
        //   ImagePro の起動     V9.5 追加 by 間々田 2004/09/14
        //                       v11.5変更 by 間々田 2006/08/25 iniファイル読み込み部分は 別関数 GetCtIni に分けた
        //
        public static bool StartImagePro()
        {
            //const int SW_SHOWMINIMIZED = 2;

            //int theHandle = 0;			// Image-Proのハンドル ～Image-Pro動作中の確認   'v11.5追加 by 間々田 2006/04/17
            //int ret = 0;

            //戻り値初期化
            bool functionReturnValue = false;

            string IPExe = CTSettings.iniValue.ImageProExe;
            
            //ファイル有無を確認
            bool bval = File.Exists(IPExe);
            
            //実行ファイル名が指定されているか
            if (string.IsNullOrEmpty(IPExe) || !bval)
            {
                ////メッセージ表示：
                ////   イメージプロを起動できませんでした。
                ////   ～ でイメージプロの実行ファイル名が正しく定義されていない可能性があります。
                ////MessageBox.Show(StringTable.GetResString(15201, modFileIO.IniFileName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MessageBox.Show(StringTable.GetResString(15201,AppValue.IniFileName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return functionReturnValue;
            }

            //エラーを無視           'v15.0追加 by 間々田 2009/01/09
            
            try
            {
                #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                /*
                //Image-Proのハンドル取得（Image-Pro動作中の確認）                           'v11.5追加 by 間々田 2006/04/17
                ret = Ipc32v5.IpAppGet(Ipc32v5.GETAPPWND, 0, ref theHandle);
                */
                //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                //CTSettings.IPOBJ.StartImageProServer();    
                #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

            }
            catch
            {
                // Nothing
            }
            //すでにImage-Pro が起動していない場合のみ実行
            //'If FindWindow(vbNullString, "Image-Pro Plus") = 0 Then
            //If Not IsAlive(ImageProExe) Then          'v9.7変更 by 間々田 2004/12/07
            
            //int ProcessID = 0;    //削除2015/01/17hata
            
            //変更2014/03/20hata
            //if (theHandle == 0)			//v11.5変更 by 間々田 2006/04/17
            //変更2015/01/17hata
            bool bAlive = false;
            string ipf = Path.GetFileNameWithoutExtension(IPExe);
            //イメージプロServerの起動確認をする
            Process[] ps = Process.GetProcessesByName(ipf);
            foreach (Process p in ps)
            {
                //起動中
                bAlive = true;
            }
            if (!bAlive) 
            //if (!IsAlive(IPExe))
            {
                try
                {
                    PauseForDoEvents(CTSettings.iniValue.ImageProPauseTime);    //added by 山本 2005-2-22   イメージプロのプロテクトキーが見つからない問題の対策
                }
                catch
                {
                    // Nothing
                }

                //変更2015/01/17hata
                //サーバーと共にリスタートさせる
                //CTSettings.IPOBJ.ReStartImageProServer();    
                //Process p = null;
                //try
                //{
                //    //イメージプロの起動
                //    //rc = WinExec(ImageProExe, SW_SHOWMINIMIZED)    'ウィンドウを最小化する
                //    //        rc = IpAppMinimize()                                                   'v10.0削除 by 間々田 2005/02/17　不要
                //    //        rc = IpAppMove(0, 0)                                                   'v10.0削除 by 間々田 2005/02/17　不要
                //    p = new Process();
                //    p.StartInfo.FileName = IPExe;
                //    //'最小化して起動    'v18.00変更 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                //    p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized; //'ウィンドウを最小化する         

                //    p.Start();
                //    ProcessID = p.Id;
                //}
                //finally
                //{
                //    if (p != null)
                //    {
                //        p.Dispose();
                //    }
                //    p = null;
                //}

                //if (ProcessID == 0)
                //{
                //    try
                //    {
                //        //メッセージ表示：
                //        //   イメージプロを起動できませんでした。
                //        //   ～ でイメージプロの実行ファイル名が正しく定義されていない可能性があります。
                //        MessageBox.Show(StringTable.GetResString(15201, AppValue.IniFileName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    }
                //    catch
                //    {
                //        // Nothing
                //    }
                //    return functionReturnValue;
                //}


                //CT30Kにフォーカスを戻す            'v9.7削除 by 間々田 2004/12/07
                //        If IsExistForm(frmCTMenu) Then
                //            If frmCTMenu.Visible Then frmCTMenu.SetFocus
                //        End If

                //        'CT30Kをアクティブにする            'v9.7追加 by 間々田 2004/12/07     'v10.0削除 by 間々田 2005/02/17　かえってアクティブにならないので
                //        AppActivate App.title
                //        DoEvents
            }

            //戻り値セット
            functionReturnValue = true;
            return functionReturnValue;
        }


#region //v15.0削除ここから by 間々田 2006/03/16
        //'
        //'   IpTextBurn を呼び出す   V9.5 新規作成 by 間々田 2004/09/17  Image-Pro Ver.5 にも対応
        //'
        //Public Function DoIpTextBurn(ByVal theText1 As String, ByVal theText2 As String, p As POINTAPI, Optional ByVal vPit As Integer = 16) As Integer
        //
        //    Dim workStr As String
        //    Dim rc      As Long
        //
        //    workStr = StrConv(LeftB$(StrConv(theText1 & Space$(17), vbFromUnicode), 17), vbUnicode) & ":" & theText2
        //    workStr = StrConv(LeftB$(StrConv(workStr, vbFromUnicode), 255), vbUnicode)  '最大255文字まで
        //
        //#If ImageProV3 Then                                         'v9.5 コンパイルオプションによる分岐を追加 by 間々田 2004/09/17
        //    DoIpTextBurn = IpTextBurn(workStr, p)
        //#Else
        //    rc = IpAnCreateObj(GO_OBJ_TEXT)                                 '注釈オブジェクトの作成
        //    rc = IpAnMove(0, p.X, p.y)                                      'オブジェクトの移動
        //    rc = IpAnText(workStr)                                          '注釈オブジェクトにテキストの書込み
        //    rc = IpAnSet(GO_ATTR_FONTSIZE, 12)                              'フォントサイズの設定：12ポイント
        //    rc = IpAnSetFontName(LoadResString(IDS_FImageInfo0))            'フォント名の設定：ＭＳ ゴシック
        //    rc = IpAnSet(GO_ATTR_TEXTCOLOR, vbWhite)                        'テキストの色指定
        //    rc = IpAnMove(5, p.X + lstrlen(workStr) * 12 / 2 - 1, p.y + 11) '注釈ｵﾌﾞｼﾞｪｸﾄのｻｲｽﾞ指定（右下端座標）
        //    rc = IpAnBurn()                                                 'テキストを画像に書込む
        //#End If
        //
        //    p.y = p.y + vPit
        //
        //End Function
        //v15.0削除ここまで by 間々田 2006/03/16
#endregion

		//
		//   指定したモジュール名のプロセスが生きているか判定する    v9.7追加 by 間々田 2004/12/09
		//
		public static bool IsAlive(string TargetModule)
		{
			int lngApiRet = 0;
			int cc = 0;
			int lnghProcess = 0;
			int lnghModule = 0;
			string strModuleName = null;	//TODO 固定長文字列
			StringBuilder sb = new StringBuilder(256);
            //FixedString256 ModuleName = new FixedString256();       	

            int lngcbNeeded = 0;
			int lngStrLen = 0;
			int i = 0;
			int[] lngProcessID = new int[257];

			//戻り値初期化
			bool functionReturnValue = false;

			lngApiRet = Winapi.EnumProcesses(ref lngProcessID[0], lngProcessID.GetUpperBound(0), ref cc);

			for (i = 1; i <= cc / 4 - 1; i++)
			{
                lnghProcess = Winapi.OpenProcess(Winapi.PROCESS_QUERY_INFORMATION | Winapi.PROCESS_VM_READ, 0, lngProcessID[i]);
				if (lnghProcess != 0)
				{
                    if (Winapi.EnumProcessModules(lnghProcess, ref lnghModule, 0, ref lngcbNeeded) != 0)
					{
                        lngStrLen = Winapi.GetModuleFileNameEx(lnghProcess, lnghModule, sb, sb.Capacity);
                        strModuleName = sb.ToString();
                        //strModuleName = ModuleName.GetString();

					}
				}

                lngApiRet = Winapi.CloseHandle(lnghProcess);

                if (!string.IsNullOrEmpty(strModuleName))
                {
                    //if (strModuleName.Substring(0, lngStrLen).ToUpper() == TargetModule.ToUpper())
                    if (strModuleName.ToUpper() == TargetModule.ToUpper())
                    {
					    functionReturnValue = true;
					    break;
				    }
                }
			}
			return functionReturnValue;
		}


        //
        //   指定したモジュール名のプロセスがなくなるまでウエイトする    v11.5追加 by 間々田 2006/07/10
        //
        public static void WaitWhileAlive(string Process, int PauseTime = 1000)
        {
            int StartTime = 0;

            StartTime = Winapi.GetTickCount();

            //プロセスがなくなるまでウエイトする

            while (IsAlive(Process))    //（10秒）
            {
                PauseForDoEvents(1);
                if (Winapi.GetTickCount() - StartTime > PauseTime) break;
            }
        }


        //*******************************************************************************
        //機　　能： グローバル変数の初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V9.7  05/01/12 (SI4)間々田     新規作成
        //*******************************************************************************
        //Private Sub InitGlobalVariables()
        public static void InitGlobalVariables()        //v17.20 検出器切替改造のためpublic化 by 長野 2010-09-01
        {
            ////透視画像のビット数（scancondpar.fimage_bit）によって透視画像表示用のウィンドウレベル／ウィンドウ幅のデフォルト値を設定
            //modCT30K.FimageBitIndex = CTSettings.scancondpar.Data.fimage_bit;

            ////スキャン位置校正時のデフォルトのモード（True:自動, False:手動）'v11.2追加 by 間々田 2006/01/10
            //ScanCorrect.IsAutoAtSpCorrect = (CTSettings.scaninh.Data.full_distortion == 0);		//フル２次元：自動   １次元：手動

            ////前回のksw                  'v11.2追加 by 間々田 2006/01/12
            //LastKsw = 0;

            ////自動スキャン位置校正用I.I.移動フラグリセット   v11.21追加 by 間々田 2006/02/10
            //modScanCorrectNew.IIMovedAtAutoSpCorrect = false;

            ////v11.3追加ここから by 間々田 2006/02/20 frmCTMenu.MDIForm_Load から移動

            ////校正処理の初期値設定
            //ScanCorrect.IntegNumAtRot = 1;
            //ScanCorrect.GVal_SlWid_Rot = 0.0F;			//added V2.0 by 鈴山
            //ScanCorrect.GVal_SlPix_Rot = 20.0F;			//added V2.0 by 鈴山
            ////GVal_DistVal = 10#
            //ScanCorrect.GVal_DistVal[1] = 10.0F;		//v16.2/v17.0 配列に変更 2010/02/23 by Iwasawa
            //ScanCorrect.GVal_DistVal[2] = 10.0F;		//v16.2/v17.0 配列に変更 2010/02/23 by Iwasawa
            //ScanCorrect.GFlg_Shading_Rot = false;		//V4.0 append by 鈴山 2001/03/29

            ////メカ準備の初期値設定                   'V4.0 append by 鈴山 2001/02/06

            ////微調テーブル移動速度
            //modMechaControl.GVal_FineTableSpeed = CTSettings.scancondpar.Data.ftable_max_speed * 0.5f;	//コモン値から最大値*0.5を設定　'V7.0 change by 間々田 2003/11/06

            ////画像処理関連の初期化処理
            //modImgProc.InitImgProc();

            ////コーンビーム調整フラッグの初期値設定　 added by 山本　2002-8-31
            //Maint.ConeAdjustFlg = 0;

            //#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            ////Viscom調整メンテナンスフラッグの初期値設定  added by 山本 2006-12-27
            ////    ViscomMaintFlg = 0
            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            //#endregion


            ////'v19.19 GetBinningの下にもってくる by長野 2013/09/26    //変更2014/10/07hata_v19.51反映
            //////校正関連の初期化
            ////modScanCorrect.InitScanCorrect();

            ////v7.0 FPD対応 by 間々田 2003/09/24
            //GetBinning();

            //TableRotOn = (CTSettings.scansel.Data.table_rotation == 1);
            ////    FPGainOn = Use_FlatPanel   'v15.0削除 by 間々田 2009/01/13
            ////    FPDefOn = Use_FlatPanel    'v15.0削除 by 間々田 2009/01/13

            ////'v19.19 InitScanCorrectをGetBinnginの下にもってくる    //変更2014/10/07hata_v19.51反映
            ////校正関連の初期化
            //modScanCorrect.InitScanCorrect();

            ////v11.3追加ここまで by 間々田 2006/02/20 frmCTMenu.MDIForm_Load から移動

            ////強制ウォームアップ（東芝 EXM2-150用）                  'v11.4追加 by 間々田 2006/02/17
            //gForcedWarmup = 0;			//0:短期 1:長期

            ////Ｘ線ツール画面で使用するタイマーカウント               'v11.4追加 by 間々田 2006/03/10
            //modXrayControl.XrayToolTimerCount = 20 * 60;

            ////バックアップ用設定管電圧                               'v11.4追加 by 間々田 2006/03/10
            //modXrayControl.BackXrayVoltSet = -1;

            ////ウォームアップ後フィラメント調整を開始する場合のフラグ(Viscom専用)　'v11.5追加 by 間々田 2006/05/08
            //modXrayControl.FilamentAdjustAfterWarmup = false;

            ////Ｘ線ウォームアップ中断フラグ(Viscom専用)　'v11.5追加 by 間々田 2006/05/08
            //modXrayControl.XrayWarmUpCanceled = false;

            ////ウォームアップ中の最大出力管電圧をクリア   'v11.5追加 by 間々田 2006/05/16
            //modXrayControl.XrayMaxFeedBackVolt = 0;

            ////Ｘ線手動センタリングフラグ(Viscom専用)　'v11.5追加 by 間々田 2006/05/10
            //modXrayControl.XrayCenteringManual = false;

            ////透視画像処理関連の変数初期化   'v15.0追加 by 間々田 2009/01/09
            ////FluoroIP.InitFluoroIP();
            //CTSettings.InitFluoroIP();

            ////終了要求フラグクリア                    'v11.5追加 by 間々田 2006/06/23
            //RequestExit = false;

            ////前回の画像保存先を求めておく           'v15.0追加 by 間々田 2009/06/16
            ////DestOld = modFileIO.FSO.BuildPath(CTSettings.scansel.Data.pro_code, CTSettings.scansel.Data.pro_name);
            //DestOld = Path.Combine(CTSettings.scansel.Data.pro_code.GetString(), CTSettings.scansel.Data.pro_name.GetString());

            ////回転中心位置のチェックフラグ v16.03/v16.20 追加 by 山影 10-03-12
            //CheckRC = false;

            ////非常停止ボタンが押されたかどうかのチェックフラグ 'v17.40 by 長野
            //emergencyButton_Flg = false;

            //#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            ////高速再構成可、かつ、連続コーン可の時に連続回転コーン実行可能フラグを追加(Falseで初期化) 'v17.40 追加 by 長野 2010/10/21
            ////If (scaninh.gpgpu = 0) And (scaninh.smooth_rot_cone = 0) Then
            ////        smooth_rot_cone_flg = False     'v17.42変更 常に初期化する byやまおか 2010/11/04
            ////End If
            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            //#endregion

            ////v19.11 ROI処理、ヒストグラム、骨塩定量解析のROI形状を初期化 2013/02/20
            //DrawRoi.RoiCalRoiNo = 2;
            //DrawRoi.HistRoiNo = 2;
            //DrawRoi.BoneDensityRoiNo = 2;
        }


        public static void logOut(string theString)
        {

#if DebugOn

            StreamWriter sw = null;

            try
            {
                //sw = new StreamWriter(modFileIO.FSO.BuildPath(modFileIO.CTTEMP, "CT30kDebugTmp.txt"), true);
                //変更2015/01/22hata
                //sw = new StreamWriter(Path.Combine(AppValue.CTTEMP, "CT30kDebugTmp.txt"), true);
                sw = new StreamWriter(Path.Combine(AppValue.CTTEMP, "CT30kDebugTmp.txt"), true, Encoding.GetEncoding("shift-jis"));

                sw.WriteLine(DateTime.Now.ToString() + " " + theString);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw = null;
                }
            }
#endif
        }


        //CTUSERの下にログファイルを残す 'v17.48/v17.53追加 byやまおか 2011/03/21
        public static void UserLogOut(string theString)
        {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
            Dim fileNo As Integer
    
            fileNo = FreeFile()
            Open FSO.BuildPath(CTUSER, "CT30KSCAN.log") For Append As fileNo
    
            Print #fileNo, Tab(0); theString;

            Close fileNo
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            StreamWriter sw = null;

            try
            {
                //変更2015/01/22hata
                ////sw = new StreamWriter(modFileIO.FSO.BuildPath(AppValue.CTUSER, "CT30KSCAN.log"), true);
                //sw = new StreamWriter(Path.Combine(AppValue.CTUSER, "CT30KSCAN.log"), true);
                sw = new StreamWriter(Path.Combine(AppValue.CTUSER, "CT30KSCAN.log"), true, Encoding.GetEncoding("shift-jis"));
                
                sw.Write(theString);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw = null;
                }
            }
        }


        //ログファイルのサイズをチェックして容量オーバーならクリアする   'v17.48/v17.53追加 byやまおか 2011/03/21
        public static void UserLogDel(int theSize)
        {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
            Dim fileNo As Integer
            Dim filePathName As String
    
            fileNo = FreeFile()
            filePathName = FSO.BuildPath(CTUSER, "CT30KSCAN.log")
    
            Open filePathName For Append As fileNo
            Close fileNo
    
            'ファイルサイズチェック
            If FileLen(filePathName) > theSize Then
    
                'ログデーファイルを削除する
                If Dir(filePathName, vbNormal) <> "" Then Kill filePathName
        
            End If
*/
#endregion

            string filePathName = null;

            //filePathName = modFileIO.FSO.BuildPath(AppValue.CTUSER, "CT30KSCAN.log");
            filePathName = Path.Combine(AppValue.CTUSER, "CT30KSCAN.log");
            
            FileInfo file = new FileInfo(filePathName);

            //ファイルサイズチェック
            if (file.Length > theSize)
            {
                //ログデーファイルを削除する
                if (file.Exists &&
                   (file.Attributes & (FileAttributes.System | FileAttributes.Hidden | FileAttributes.ReadOnly)) == 0)
                {
                    file.Delete();
                }
            }
        }





        //v19.50必ず消す
        //CTUSERの下にログファイルを残す 'v17.48/v17.53追加 byやまおか 2011/03/21
        public static void UserTiLogOut(string theString)
        {
            StreamWriter sw = null;

            try
            {
                //変更2015/01/22hata
                //sw = new StreamWriter(Path.Combine(AppValue.CTUSER, "CT30KTi.log"), true);
                sw = new StreamWriter(Path.Combine(AppValue.CTUSER, "CT30KTi.log"), true, Encoding.GetEncoding("shift-jis"));

                sw.Write(theString);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw = null;
                }
            }
        }


        //v19.50必ず消す
        //ログファイルのサイズをチェックして容量オーバーならクリアする   'v17.48/v17.53追加 byやまおか 2011/03/21
        public static void UserTiLogDel(int theSize)
        {
            string filePathName = null;

            filePathName = Path.Combine(AppValue.CTUSER, "CT30KTi.log");

            FileInfo file = new FileInfo(filePathName);

            //ファイルサイズチェック
            if (file.Length > theSize)
            {
                //ログデーファイルを削除する
                if (file.Exists &&
                   (file.Attributes & (FileAttributes.System | FileAttributes.Hidden | FileAttributes.ReadOnly)) == 0)
                {
                    file.Delete();
                }
            }
        }



        //*******************************************************************************
        //機　　能： CSVファイルの所定のキーワードの項目を更新する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.2  05/09/12  (SI3)間々田    新規作成
        //*******************************************************************************
        public static void UpdateCsv(string FileName, string KeyWord, string SetData, int Column = 2)
        {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
            Dim fileIn  As Integer
            Dim fileOut As Integer
            Dim buf     As String
            Dim Cell()  As String

            'エラー時の扱い
            On Error GoTo ErrorHandle
    
            'ファイルオープン（入力用）
            fileIn = FreeFile()
            Open FileName For Input As #fileIn
    
            'ファイルオープン(出力用)
            fileOut = FreeFile()
            Open FileName & ".tmp" For Output As #fileOut
    
            Do While Not EOF(fileIn)
        
                '入力ファイルから１行読み込む
                Line Input #fileIn, buf
        
                If buf <> "" Then
                    Cell() = Split(buf, ",")
                    If UBound(Cell) >= 1 Then
                        If Trim$(Cell(1)) = KeyWord Then
                            If UBound(Cell) < Column Then ReDim Preserve Cell(Column)
                            Cell(Column) = SetData
                            buf = Join(Cell, ",")
                        End If
                    End If
                End If
        
                '出力ファイルに書き込む
                Print #fileOut, buf
        
            Loop
    
            'ファイルクローズ
            Close #fileIn
            Close #fileOut
    
            '古いＣＳＶを削除。その後作成したＣＳＶをリネーム
            Kill FileName
            Name FileName & ".tmp" As FileName
    
            Exit Sub
*/
#endregion

            string buf = null;
            string[] Cell = null;

            StreamReader sr = null;
            StreamWriter sw = null;

            //エラー時の扱い
            try
            {
                try
                {
                    //ファイルオープン（入力用）
                    //変更2015/01/22hata
                    //sr = new StreamReader(FileName);
                    sr = new StreamReader(FileName, Encoding.GetEncoding("shift-jis"));
                    
                    //ファイルオープン(出力用)
                    //変更2015/01/22hata
                    //sw = new StreamWriter(FileName + ".tmp");
                    sw = new StreamWriter(FileName + ".tmp", false, Encoding.GetEncoding("shift-jis"));

                    //入力ファイルから１行読み込む
                    while ((buf = sr.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(buf))
                        {
                            Cell = buf.Split(',');
                            if (Cell.GetUpperBound(0) >= 1)
                            {
                                if (Cell[1].Trim() == KeyWord)
                                {
                                    if (Cell.GetUpperBound(0) < Column) Array.Resize(ref Cell, Column + 1);
                                    Cell[Column] = SetData;
                                    buf = string.Join(",", Cell);
                                }
                            }
                        }
                        //出力ファイルに書き込む
                        sw.WriteLine(buf);
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    //ファイルクローズ
                    if (sr != null)
                    {
                        sr.Close();
                        sr = null;
                    }
                    if (sw != null)
                    {
                        sw.Close();
                        sw = null;
                    }
                }

                //古いＣＳＶを削除。その後作成したＣＳＶをリネーム
                File.Delete(FileName);
                File.Move(FileName + ".tmp", FileName);
            }
            catch (Exception ex)
            {
                ////エラーメッセージの表示：以下のファイルの書き込み時にエラーが発生しました。
                //MessageBox.Show(CTResources.LoadResString(9968) + FileName + "\r" + "\r"
                //                + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        //*******************************************************************************
        //機　　能： 選択してあるズーミングテーブルをコモンから取得
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [ /O] String    ズーミングテーブル名（拡張子つき）
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  97/12/07  (CATE)山本     新規作成
        //*******************************************************************************
        public static string GetZoomingTable()
        {
            //return modLibrary.AddExtension(modFileIO.FSO.BuildPath(CTSettings.scansel.Data.autozoom_dir, CTSettings.scansel.Data.auto_zoom), ".csv");
            return modLibrary.AddExtension(Path.Combine(CTSettings.scansel.Data.autozoom_dir.GetString(), CTSettings.scansel.Data.auto_zoom.GetString()), ".csv");
        }


        //*******************************************************************************
        //機　　能： 選択しているスライスプランテーブルをコモンから取得
        //
        //           変数名          [I/O] 型        内容
        //引　　数： IsConeBeam      [I/O] Boolean   コーンビームの場合、True
        //戻 り 値：                 [ /O] String    スライスプランテーブル名（拡張子つき）
        //
        //補　　足： なし
        //
        //履　　歴： V1.00   XX/XX/XX    ?????????   新規作成
        //*******************************************************************************
        public static string GetSlicePlanTable(bool IsConeBeam = false)
        {
            string FileName = null;

            if (IsConeBeam)
            {
                //FileName = modFileIO.FSO.BuildPath(CTSettings.scansel.Data.cone_sliceplan_dir, CTSettings.scansel.Data.cone_slice_plan);  //コーンビーム用
                FileName = Path.Combine(CTSettings.scansel.Data.cone_sliceplan_dir.GetString(), CTSettings.scansel.Data.cone_slice_plan.GetString());  //コーンビーム用
            }
            else
            {
                //FileName = modFileIO.FSO.BuildPath(CTSettings.scansel.Data.sliceplan_dir, CTSettings.scansel.Data.slice_plan);            //ノーマルスキャン用
                FileName = Path.Combine(CTSettings.scansel.Data.sliceplan_dir.GetString(), CTSettings.scansel.Data.slice_plan.GetString());            //ノーマルスキャン用
            }

            //拡張子（.csv）を付加する
            return modLibrary.AddExtension(FileName, ".csv");
        }


        //
        //   画像情報ファイルをサーチする  追加 by 間々田 2004/04/06
        //
        public static bool SearchImageInfo(string FileName, bool SearchZoomingInf = false)
        {
            int i = 0;
            string workStr = null;

            //戻り値初期化
            bool functionReturnValue = false;

            //if (modFileIO.FSO.FileExists(FileName + ".inf"))
            if (File.Exists(FileName + ".inf"))
            {
                functionReturnValue = true;
            }
            else if (SearchZoomingInf)
            {
                //ズーミングした画像の付帯情報ファイルが有るか？
                for (i = 1; i <= 999; i++)
                {
                    workStr = FileName + "-Z" + i.ToString("000");
                    //if (modFileIO.FSO.FileExists(workStr + ".inf"))
                    if (File.Exists(workStr + ".inf"))
                    {
                        //付帯情報ファイルをズーミングなしの付帯情報ファイル名としてコピーする
                        File.Copy(workStr + ".inf", FileName + ".inf", true);
                        functionReturnValue = true;
                        break;
                    }
                }
            }

            return functionReturnValue;
        }


        //
        //   子プロセスの起動    'v11.5追加 by 間々田 2006/06/22
        //
        public static int StartProcess(string strCommand)
        {
//            int ProcessID = 0;            //ShellコマンドのプロセスID格納用

//#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
//            //分散処理ステータスフォームを下げる
//            //    If IsExistForm(frmDistributeStatus) Then
//            //        frmDistributeStatus.Top = 0 + 5910 + 1050
//            //    End If
//            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
//#endregion

//            //フラグをセット
//            if (strCommand == AppValue.SCANAV || strCommand == AppValue.CONEBEAM)
//            {
//                modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTScanStart;
//            }
//            else
//            {
//                modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTReconStart;
//            }

//            //PIOチェックタイマーを一時的に止める
//            //if (modLibrary.IsExistForm(frmMechaControl.Instance))
//            if (modLibrary.IsExistForm("frmMechaControl"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
//            {
//                frmMechaControl.Instance.tmrPIOCheck.Enabled = false;
//            }

//            //ＣＴ画像コレクションをクリア
//            if (!(ImportedCTImages == null)) ImportedCTImages = null;

//            //処理開始
//            Process p = null;
//            try
//            {
//                p = new Process();
//                p.StartInfo.FileName = strCommand;
//                p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
//                p.Start();
//                ProcessID = p.Id;
//            }
//            finally
//            {
//                if (p != null)
//                {
//                    p.Dispose();
//                }
//                p = null;
//            }

//            //    '子プロセスからの送信を受け付ける
//            //    frmCTMenu.socCT30K.Bind 7010

//            //マウスカーソルを矢印＆砂時計にする
//            Cursor.Current = Cursors.AppStarting;

//            //戻り値
//             return ProcessID;
            return 0;
        }


        //
        //   子プロセスの終了処理    'v11.5追加 by 間々田 2006/06/22
        //
        public static void EndProcess(int ReturnCode = 0)
        {
#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //進捗ステータス非表示
            //    Unload frmScanStatus
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

//            //子プロセスからの送信を拒否
//            //    frmCTMenu.socCT30K.Close

//            //reconmst.exe実行中のフラッグをfalseに戻す
//            modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTReconStart);

//            //PIOチェックタイマーを元に戻す
//            //if (modLibrary.IsExistForm(frmMechaControl.Instance))
//            if (modLibrary.IsExistForm("frmMechaControl"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
//            {
//                frmMechaControl.Instance.tmrPIOCheck.Enabled = true;
//            }

//            //スキャン条件画面の設定値を元に戻す
//            modCommon.RestoreScansel();

//#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
//            //分散処理ステータスフォームを元の位置に戻す
//            //    If IsExistForm(frmDistributeStatus) Then
//            //        frmDistributeStatus.Top = 0 + 5910
//            //        frmDistributeStatus.Refresh
//            //    End If
//            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
//#endregion

//            //マウスカーソルを元に戻す
//            Cursor.Current = Cursors.Default;

//            //リターンコードの扱い
//            switch (ReturnCode)
//            {
//                //正常終了時またはスキャンエラーコードが０の時
//                case 0:
//                case 1901:
//                    break;
//                //異常終了時はエラーメッセージを表示
//                default:
//                    ErrMessage(ReturnCode, Icon: MessageBoxIcon.Error);
//                    break;
//            }

//            //ズーミング時・再構成リトライ時
//            //if (modLibrary.IsExistForm(frmRetryCondition.Instance))
//            if (modLibrary.IsExistForm("frmRetryCondition"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
//            {
//                //再構成フォームのアンロード
//                //frmRetryCondition.Instance.Close();
//                //Rev99.99 showDialogで開いているためdispose by長野 2014/12/15
//                frmRetryCondition.Instance.Dispose();
//            }
//            //コーン後再構成時
//            //else if (modLibrary.IsExistForm(frmPostConeReconstruction.Instance))
//            if (modLibrary.IsExistForm("frmPostConeReconstruction"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
//            {
//                //コーン後再構成フォームのアンロード
//                frmPostConeReconstruction.Instance.Close();
//            }
        }


        //*******************************************************************************
        //機　　能： 画像ファイルごとに画像階調最適化を行なう
        //
        //           変数名          [I/O] 型        内容
        //引　　数： FileName         [I/ ] String   画像ファイル名
        //戻 り 値： なし
        //
        //補　　足： 画像の最大・最小値を求めたら、ウィンドウ幅・レベルを計算し、
        //           結果を付帯情報ファイルに書込みます。
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        public static void ContrastFitting(string FileName)
        {
            int theMatrix = 0;			//マトリクスサイズ（512,1024）
            int theWidth = 0;			//ウィンドウ幅（1～16384）
            int theLevel = 0;			//ウィンドウレベル（-8192～8191）
            int theMax = 0;			    //画像の最大値
            int theMin = 0;			    //画像の最小値
            ushort[] theImage = null;	//画像読込みバッファ

            //modImageInfo.ImageInfoStruct theInfo = default(modImageInfo.ImageInfoStruct);			//付帯情報構造体変数
            CTAPI.CTstr.IMAGEINFO theInfo = default(CTAPI.CTstr.IMAGEINFO);	//付帯情報構造体変数
            theInfo.Initialize();

            //マトリクスサイズ
            FileInfo fi = new FileInfo(FileName);
            //2014/11/13hata キャストの修正
            //theMatrix = (int)Math.Sqrt(fi.Length / 2);
            theMatrix = (int)Math.Sqrt(fi.Length / 2D);

            //配列の確保
            theImage = new ushort[theMatrix * theMatrix];

            ////画像を配列に取り込む
            //if (ScanCorrect.ImageOpen(ref theImage[0], FileName, theMatrix, theMatrix) != 0) return;

            ////画像の最大値・最小値を求める
            //ScanCorrect.GetMaxMin(ref theImage[0], theMatrix, theMatrix, ref theMin, ref theMax);

            //最適なウィンドウ幅・ウィンドウレベルを求める
            theWidth = theMax - theMin;
            //2014/11/13hata キャストの修正
            //theLevel = theMin + theWidth / 2;
            theLevel = theMin + Convert.ToInt32(theWidth / 2F);

            //付帯情報を取得
            //if (!modImageInfo.ReadImageInfo(ref theInfo, modLibrary.RemoveExtension(FileName, ".img"))) return;
            if (!ImageInfo.ReadImageInfo(ref theInfo, modLibrary.RemoveExtension(FileName, ".img"))) return;

            //ウィンドウ幅・ウィンドウレベルを付帯情報ファイルに書込む
            //modLibrary.SetField(theWidth.ToString(), ref theInfo.ww);
            //modLibrary.SetField(theLevel.ToString(), ref theInfo.WL);
            theInfo.ww.SetString(theWidth.ToString());
            theInfo.wl.SetString(theLevel.ToString());

            ImageInfo.WriteImageInfo(ref theInfo, modLibrary.RemoveExtension(FileName, ".img"));
        }


        //*******************************************************************************
        //機　　能： フォームの位置を設定する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： theForm         [I/ ] Form      位置設定するフォーム
        //戻 り 値： なし
        //
        //補　　足： 画面左側の中央に位置設定する
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        public static void SetPosNormalForm(Form theForm)
        {
            int pos_left = 0;			//v15.0追加 byやまおか 2009/07/30
            int toolbar_right = 0;		//v15.0追加 byやまおか 2009/07/30

            //.Move frmScanControl.width + frmCTMenu.Toolbar1.width + 30 - .width, Screen.Height / 2 - .Height / 2
            //画面をはみ出す場合は、ツールバー位置にそろえる(CT画像にかぶる)     'v15.0追加 byやまおか 2009/07/30
            //var _frmCTMenu = frmCTMenu.Instance;
            //pos_left = frmScanControl.Instance.Width + _frmCTMenu.Toolbar1.Width + 2 - theForm.Width;
            //toolbar_right = _frmCTMenu.Toolbar1.Left + _frmCTMenu.Toolbar1.Width + 2;

            //2014/11/13hata キャストの修正
            //theForm.Location = new Point((pos_left >= toolbar_right ? pos_left : toolbar_right), Screen.GetBounds(theForm).Height / 2 - theForm.Height / 2);
            theForm.Location = new Point((pos_left >= toolbar_right ? pos_left : toolbar_right), Convert.ToInt32(Screen.GetBounds(theForm).Height / 2F - theForm.Height / 2F));
        }


        //*******************************************************************************
        //機　　能： フォームの位置を設定する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： theForm         [I/ ] Form      位置設定するフォーム
        //戻 り 値： なし
        //
        //補　　足： 画面右上に位置設定する
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        public static void SetPosToolForm(Form theForm)
        {
            theForm.Location = new Point(Screen.GetBounds(theForm).Width - theForm.Width, 21);
        }


        //*******************************************************************************
        //機　　能： 非同期でメッセージボックスを表示する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Prompt          [I/ ] String    表示させるメッセージ
        //戻 り 値： なし
        //
        //補　　足：
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        public static void MsgBoxAsync(string Prompt)
        {
            
            //frmCTMenu.Instance.tmrMessage.Tag = Prompt;
            //frmCTMenu.Instance.tmrMessage.Enabled = true;
        }


        //*******************************************************************************
        //機　　能： 非同期でメッセージを表示する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Prompt          [I/ ] String    表示させるメッセージ
        //戻 り 値： なし
        //
        //補　　足：
        //
        //履　　歴： v15.00 2009/08/24 (SS1)間々田   リニューアル
        //*******************************************************************************
        public static void ShowMessage(string Prompt)
        {
            //frmCTMenu.tmrMessageNoButton.tag = Prompt
            //frmCTMenu.tmrMessageNoButton.Enabled = True

            ////表示するメッセージ
            //frmMessage _frmMessage = frmMessage.Instance;
            //_frmMessage.lblMessage.Text = Prompt;
            //_frmMessage.lblMessage.Refresh();
            
            //try
            //{
            //    if (_frmMessage.Visible)
            //    {
            //        _frmMessage.Visible = true;
            //    }
            //    else
            //    {
            //        _frmMessage.Show(frmCTMenu.Instance);
            //    }

            //}
            ////catch(Exception  ex)
            //catch
            //{
            //    //エラーは無視
            //    //Debug.Print(ex.Message);
            //}
        }


        //*******************************************************************************
        //機　　能： HideMessage
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足：
        //
        //履　　歴： v15.00 2009/08/24 (SS1)間々田   リニューアル
        //*******************************************************************************
        public static void HideMessage()
        {
//            //frmCTMenu.tmrMessageNoButton.Enabled = False
//#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
////          Unload frmMessage
//#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

//            frmMessage.Instance.Close();
        }


        //時間計測時		
        public static double IntegTime(int theTime)
        {
            //2014/11/13hata キャストの修正
            //IntegTime_Total = IntegTime_Total * 9 / 10 + theTime / 10;
            IntegTime_Total = IntegTime_Total * 9 / (double)10 + theTime / (double)10;

            return IntegTime_Total;
        }


        //*******************************************************************************
        //機　　能： RamDiskIsReady
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足：
        //
        //履　　歴： v17.40 2010/10/26   やまおか   新規作成
        //*******************************************************************************
#region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //Public Function RamDiskIsReady() As Boolean
        //
        //    RamDiskIsReady = False
        //
        //    'RAMディスクが構築されているかどうか  'v17.40追加 byやまおか 2010/10/26
        //    If (smooth_rot_cone_flg = False) Then
        //        'メッセージ表示
        //        'キャプチャ環境を構築中です。構築にはシステムを起動してから10分程度の時間がかかります。
        //        'しばらくしてから、処理を実行してください。
        //        MsgBox LoadResString(17506) & vbCr & LoadResString(17507), vbCritical
        //        Exit Function
        //    End If
        //
        //    RamDiskIsReady = True
        //
        //End Function
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

        //*******************************************************************************
        //機　　能： 停止要求フラグをクリアする
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足：
        //
        //履　　歴： v17.50 2011/02/17 (電S1)間々田  新規作成
        //*******************************************************************************
        public static void CallUserStopClear()
        {
            //シーケンサ通信確認ファイルの値を0クリアする
            //modCommon.UserStopClear();
            ComLib.UserStopClear();

#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //    '連続回転コーンビーム＋高速再構成の時は、RAMディスクのscanstopを使う
            //    If smooth_rot_cone_flg Then
            //
            //        UserStopClear_rmdsk
            //
            //    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

            ////共有メモリの停止要求領域もクリアする
            //ScanCorrect.SetCancel(0);
        }


        //*******************************************************************************
        //機　　能： 実行中の処理に対して停止要求をする
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足：
        //
        //履　　歴： v17.50 2011/02/17 (電S1)間々田  新規作成
        //*******************************************************************************
        public static void CallUserStopSet()
        {
            //停止要求フラグセット
            CTAPI.ComLib.UserStopSet();

#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //連続回転コーンビーム＋高速再構成の時は、RAMディスクのscanstopを使う
            //    If smooth_rot_cone_flg Then
            //
            //        UserStopSet_rmdsk
            //
            //    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

            //共有メモリの停止要求領域にもフラグセット
            //ScanCorrect.SetCancel(1);
        }


        //*************************************************************************************************
        //機　　能： GetWindowHandleByProcess関数のサブ関数
        //
        //           変数名          [I/O] 型            内容
        //引　　数：
        //戻 り 値：
        //
        //補　　足： 見つかったプロセスのハンドルを記憶
        //
        //履　　歴： v17.5 2011/02/28 (電S1)間々田       新規作成
        //*************************************************************************************************
        public static bool GetWindowHandleByProcessSub(int hWnd, ref int lParam)
        {
            int lngTrd = 0;			//スレッド
            int lngPrs = 0;			//プロセス

            //戻り値初期化：Trueの間は、Windowsに存在するハンドルを最後まで取得しようとする
            bool functionReturnValue = true;

            //親ウィンドウだけを処理する
            if (Winapi.GetParent(hWnd) == 0)
            {
                //スレッドとプロセスを取得する
                lngTrd = Winapi.GetWindowThreadProcessId(hWnd, ref lngPrs);

                //同じプロセスだとしたら
                if (lngPrs == TargetProcessId)
                {
                    //取得してきたハンドルを記憶
                    WindowHandleByProcessSub = hWnd;

                    //これ以上のハンドルは取得しないでもいいので、Falseをセット
                    functionReturnValue = false;
                }
            }
            return functionReturnValue;
        }


        //*************************************************************************************************
        //機　　能： 指定したプロセスに対するウィンドウハンドルを取得する
        //
        //           変数名          [I/O] 型            内容
        //引　　数：
        //戻 り 値：
        //
        //補　　足： なし
        //
        //履　　歴： v17.5 2011/02/28 (電S1)間々田       新規作成
        //*************************************************************************************************
#region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //Public Function GetWindowHandleByProcess(ByVal ProcessID As Long) As Long
        //
        //    '戻り値初期化
        //    GetWindowHandleByProcess = 0
        //
        //    '指定したプロセスＩＤが有効ならば
        //    If ProcessID <> 0 Then
        //
        //        '処理対象プロセスＩＤの登録
        //        TargetProcessId = ProcessID
        //
        //        'GetWindowHandleByProcessSubによって取得されるウィンドウハンドルを初期化
        //        WindowHandleByProcessSub = 0
        //
        //        'Windowsに存在する全部のハンドルから、電卓のプロセスの一緒のハンドルを探す
        //        Call EnumWindows(AddressOf GetWindowHandleByProcessSub, 0&)
        //
        //        '取得してきたウィンドウハンドルを返す
        //        GetWindowHandleByProcess = WindowHandleByProcessSub
        //
        //    End If
        //
        //End Function
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

        //v19.00 追加 ->(電S2)永井
        //*****************************************************************************************************
        //機　　能： 指定されたフォーム上のフォント設定を一括して行なう
        //
        //           変数名          [I/O] 型        内容
        //引　　数： theForm         [I/ ] Form      フルファイル名
        //           fontName        [I/ ] String    フォント名
        //           fontSize        [I/ ] Integer   フォントサイズ
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v8.2       2007/05/09  (WEB)間々田  新規作成
        //           v8.5/9.04　2008/07/29  (SS1)間々田  関数名をSetLabelFont→SetFontOnFormに変更。
        //                                               Label以外のコントロールにも対応
        //*****************************************************************************************************
        public static void SetFontOnForm(Control theForm, string fontName = "Arial", int fontSize = 10)
        {
            foreach (Control theControl in theForm.Controls)
            {
                switch (theControl.GetType().Name) //TODO Formのタイプ
                {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
                    Case "Label", "OptionButton", "ComboBox", "CheckBox", "Frame", "TextBox", _
                         "CommandButton", "SSTab", "CWNumEdit"
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                    case "Label":
                    case "RadioButton":
                    case "ComboBox":
                    case "CheckBox":
                    case "GroupBox":   
                    case "TextBox":
                    case "Button":
                    case "TabControl":
                    case "NumericUpDown":
                        theControl.Font = new Font(fontName, fontSize);
                        break;
                    default:
                        SetFontOnForm(theControl);
                        break;
                }
            }
        }


        //*******************************************************************************
        //機　　能： 指定した画像ファイル（付帯情報ファイル）に対する生データファイル名を返す
        //
        //           変数名          [I/O] 型        内容
        //引　　数： FileName        [I/ ] String    画像ファイル（付帯情報ファイル）
        //戻 り 値：                 [ /O] String    生データファイル名
        //
        //補　　足： なし
        //
        //履　　歴： v8.5  2008/07/30 (SS1)間々田   新規作成
        //*******************************************************************************
        public static string GetRawName(string FileName)
        {
            string BaseName = null;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版]
/*
            Const ZoomExt As String = "-z###"
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            const string ZoomExt = @"\-z\d\d\d";

            //ベース名取得
            //BaseName = modFileIO.FSO.GetBaseName(FileName);
            BaseName = Path.GetFileNameWithoutExtension(FileName);

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版]
/*
            'ズーミング連番があれば取り除く
            If LCase$(BaseName) Like "*" & ZoomExt Then
                BaseName = Left$(BaseName, Len(BaseName) - Len(ZoomExt))
            End If
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            //ズーミング連番があれば取り除く
            if (System.Text.RegularExpressions.Regex.IsMatch(BaseName.ToLower(), '^' + ".*" + ZoomExt + '$'))
            {
                BaseName = BaseName.Substring(0, BaseName.Length - 5);  // 5 : "-z###" の 5 文字
            }

            //戻り値セット
            //return modFileIO.FSO.BuildPath(modFileIO.FSO.GetParentFolderName(FileName), BaseName + ".raw");
            return Path.Combine(Path.GetDirectoryName(FileName), BaseName + ".raw");

         }
        //<- v19.00

        //追加2015/01/17hata
        //Formを非表示にするときに一時的にActhiveFormを移す
        public static void FormHide(Form form)
        {
            ////FormTransparence.Instance.Activate();
            //if (frmCTMenu.Instance.Visible & frmCTMenu.Instance.Enabled)
            //    frmCTMenu.Instance.Activate();

            ////フォームを非表示にする
            //form.Hide();
        }

    }
}
