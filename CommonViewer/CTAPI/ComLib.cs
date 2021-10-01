using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CTAPI
{
    /// <summary>
    /// comlib.dll
    /// </summary>
    public class ComLib
    {
        //-----------------------------------------------------------------------------
        // API関数
        //-----------------------------------------------------------------------------
        #region API
        /// <summary>
        /// コモンのscaninh構造体を参照する
        /// </summary>
        /// <param name="ScaninhRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int GetScaninh(out CTstr.SCANINH ScaninhRec);

        /// <summary>
        /// コモンのinfdef構造体を参照する
        /// </summary>
        /// <param name="InfdefRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int GetInfdef(out CTstr.INFDEF InfdefRec);

        /// <summary>
        /// コモンのctinfdef構造体を参照する
        /// </summary>
        /// <param name="CtinfdefRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int GetCtinfdef(out CTstr.CTINFDEF CtinfdefRec);

        /// <summary>
        /// コモンのt20kinf構造体を参照する
        /// </summary>
        /// <param name="T20kinfRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int GetT20kinf(out CTstr.T20KINF T20kinfRec);

        /// <summary>
        /// MACｱﾄﾞﾚｽを取得して、ｺﾓﾝに登録する
        /// </summary>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int getMACaddress();

        /// <summary>
        /// コモンのscancondpar構造体を参照する
        /// </summary>
        /// <param name="ScancondparRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int GetScancondpar(ref CTstr.SCANCONDPAR ScancondparRec);

        /// <summary>
        /// コモンのscancondpar構造体にデータを書き込む
        /// </summary>
        /// <param name="ScancondparRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int PutScancondpar(ref CTstr.SCANCONDPAR ScancondparRec);

        /// <summary>
        /// コモンのworkshopinf構造体を参照する
        /// </summary>
        /// <param name="WorkshopinfRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int GetWorkshopinf(out CTstr.WORKSHOPINF WorkshopinfRec);

        /// <summary>
        /// コモンのworkshopinf構造体にデータを書き込む
        /// </summary>
        /// <param name="WorkshopinfRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int PutWorkshopinf(ref CTstr.WORKSHOPINF WorkshopinfRec);

        /// <summary>
        /// コモンのmechapara構造体を参照する
        /// </summary>
        /// <param name="MechaparaRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int GetMechapara(out CTstr.MECHAPARA MechaparaRec);

        /// <summary>
        /// コモンのmechapara構造体にデータを書き込む
        /// </summary>
        /// <param name="MechaparaRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int PutMechapara(ref CTstr.MECHAPARA MechaparaRec);

        /// <summary>
        /// コモンのxtable構造体を参照する
        /// </summary>
        /// <param name="XtableRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int GetXtable(out CTstr.X_TABLE XtableRec);

        /// <summary>
        /// コモンのhscpara構造体を参照する
        /// </summary>
        /// <param name="HscparaRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int GetHscpara(out CTstr.HSC_PARA HscparaRec);
        
        /// <summary>
        /// コモンのscansel構造体を参照する
        /// </summary>
        /// <param name="ScanselRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int GetScansel(out CTstr.SCANSEL ScanselRec);

        /// <summary>
        /// コモンのscansel構造体にデータを書き込む
        /// </summary>
        /// <param name="ScanselRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int PutScansel(ref CTstr.SCANSEL ScanselRec);

        // 追加2013/04/11<dNet化>_hata
        /// <summary>
        /// コモンのメカ情報mecainf構造体を参照する
        /// </summary>
        /// <param name="MecainfRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int GetMecainf(out CTstr.MECAINF MecainfRec);

        // 追加2013/04/11<dNet化>_hata
        /// <summary>
        /// コモンのmecainf(メカ情報)構造体にデータを書き込む
        /// </summary>
        /// <param name="MecainfRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int PutMecainf(ref CTstr.MECAINF MecainfRec);

        // 追加2013/05/09<dNet化>_hata
        /// <summary>
        /// コモンのdispinf(画像表示情報)構造体にデータを参照する
        /// </summary>
        /// <param name="DispinfRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int GetDispinf(out CTstr.DISPINF DispinfRec);

        // 追加2013/05/09<dNet化>_hata
        /// <summary>
        /// コモンのdispinf(画像表示情報)構造体にデータを書き込む
        /// </summary>
        /// <param name="DispinfRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int PutDispinf(ref CTstr.DISPINF DispinfRec);

        // 追加2013/05/13<dNet化>_hata
        /// <summary>
        /// コモンのReconinf(再々構成・ズーミング画像生成情報)構造体にデータを参照する
        /// </summary>
        /// <param name="DReconinfRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int GetReconinf(out CTstr.RECONINF data);

        // 追加2013/05/13<dNet化>_hata
        /// <summary>
        /// コモンのReconinf(再々構成・ズーミング画像生成情報)構造体にデータを書き込む
        /// </summary>
        /// <param name="ReconinfRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int PutReconinf(ref CTstr.RECONINF data);

        // 追加2013/05/13<dNet化>_hata
        /// <summary>
        /// コモン????（modCommonにあったもの）
        /// /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ctcominit(int Mode);

        // コモン????（modCommonにあったもの）
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int putcommon_long(string com_name, string Name, int Data);

        // コモン????（modCommonにあったもの）
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int UserStopSet();

        // コモン????（modCommonにあったもの）
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int UserStopClear();

        // コモン????（modCommonにあったもの） //追加2014/10/07hata
        //Private Declare Function getcommon_long Lib "comlib.dll" (ByVal com_name As String, ByVal Name As String, Data As Long) As Long
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int getcommon_long(string com_name,string Name, int Data);

        // コモン????（modCommonにあったもの） //追加2014/10/07hata
        //Public Declare Function get_det_com Lib "comlib.dll" (ByVal det_no As Long) As Long
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int get_det_com(int det_no);

        #region v29.99 今のところ不要 by長野 2013/04/08

        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        // コモン????（modCommonにあったもの）RAMDISK用
        //Public Declare Function ramdiskclear Lib "comlib.dll" () As Long 'v17.40 追加 by 長野
        //Public Declare Function scanstop_set_rmdsk Lib "comlib.dll" () As Long 'v17.40 追加 by 長野
        //Public Declare Function UserStopSet_rmdsk Lib "comlib.dll" () As Long 'v17.40 追加 by 長野
        //Public Declare Function UserStopClear_rmdsk Lib "comlib.dll" () As Long 'v17.40 追加 by 長野
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        #endregion

        // コモン????（modCommonにあったもの） //追加2014/10/07hata 
        //v19.00 産業用CTからコピー (電S2)永井
        //Private Declare Function getcommon_float Lib "comlib.dll" (ByVal com_name As String, ByVal Name As String, Data As Single) As Long
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int getcommon_float(string com_name, string Name, float Data);


        //roikey（コモン）取得関数
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetRoikey(out CTstr.ROIKEY theRoikey);
 
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int PutRoikey(ref CTstr.ROIKEY theRoikey);


        //コモン用共有メモリの作成
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr CreateSharedCTCommon();

        //コモン用共有メモリの破棄
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int DestroySharedCTCommon(IntPtr hMap);

        //コモンファイルを共有メモリにセット
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetSharedCTCommon();

        //共有メモリ上のコモンファイルをファイルに保存
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SaveSharedCTCommon();

        /// <summary>
        /// 切り替えたX線の種類に従い、SETFILEをコモンにセットする。
        /// </summary>
        /// <param name="ReconinfRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int change_xray_com(int xray_no);

        /// <summary>
        /// コモンのzoomtbl構造体を参照する
        /// </summary>
        /// <param name="HscparaRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int GetZoomtbl(out CTstr.ZOOMTBL zoomtbl);

        /// <summary>
        /// コモンのsp2inf構造体を参照する
        /// </summary>
        /// <param name="HscparaRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int GetSp2inf(out CTstr.SP2INF sp2inf);

        /// <summary>
        /// コモンのpdplan構造体を参照する
        /// </summary>
        /// <param name="HscparaRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int GetPdplan(out CTstr.PDPLAN pdplan);

        /// <summary>
        /// コモンのdischarge_protect構造体を参照する
        /// </summary>
        /// <param name="HscparaRec"></param>
        /// <returns></returns>
        [DllImport("comlib.dll")]
        public static extern int GetDischargeProtect(out CTstr.DISCHARGE_PROTECT discharge_protect);

        #endregion API
    }
}
