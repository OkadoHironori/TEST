using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

using CT30K.Common;
using CTAPI;

using System.Windows.Forms;

namespace TransImage
{
 
    /// <summary>
    /// キャプチャ用エラー
    /// </summary>
    public enum CaptureResult
    {
        OK,
        OpenErr,        // オープンエラー
        OffsetErr,      // オフセット校正エラー
        GainErr,        // ゲイン構成エラー
        PixelErr,       // 欠陥マップエラー
    }

    /// <summary>
    /// シフトスキャン用構造体 //Rev23.10 追加 by長野 2015/10/06
    /// </summary>
    public struct SFTPARA
    {
        public int mainch;//合成生データのch数（画素）無効画素を含む
        public int wt_st1;//ｳｴｲﾄ掛け開始画素1（0ｵﾘｼﾞﾝ）
        public int wt_end1;//ｳｴｲﾄ掛け終了画素1（0ｵﾘｼﾞﾝ）
        public int wt_st2;//ｳｴｲﾄ掛け開始画素2（0ｵﾘｼﾞﾝ）
        public int wt_end2;//ｳｴｲﾄ掛け終了画素2（0ｵﾘｼﾞﾝ）
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	    public double[] wt			;//ｳｴｲﾄ掛け係数

        public void Initialize()
        {
            wt = new double[2];
        }
    }
    
    /// <summary>
    /// 透視画像処理
    /// </summary>
    public class TransImageControl : TransPicture
    {
        /// <summary>
        /// 透視画像更新イベント
        ///// </summary>
        //public event EventHandler TransImageChanged;

        // オフセット校正
        private double[] offsetImage;

        // ゲイン校正32bit
        private uint[] gainImageL;
 
        // 透視画像
        private ushort[] transImage;

        //積分用
        private int[] WorkImageLong;
        private ushort[] workImage;


        // ゲイン校正
        private ushort[] gainImage;
 
        // フラットパネル欠陥（浜ホト製フラットパネル用）
        private ushort[] defImage;

        // かさあげ量
        private int adv;

        // 検出器パラメータ
        private DetectorParam detector = null;

        // スキャンパラメータ
        private ScanParam scanParam = new ScanParam();

        // キャプチャスレッド
        private Thread captureThread = null;
        private bool live = false;

        // Scanスレッド
        private Thread captureScanThread = null;
        private bool bScan = false;
        private int ScanError = 0;

        //マルチ、スライスプラン
        private bool bWaitScan = false;     //マルチ、スライスプランの待機フラグ
        private int ScanLoopNum = 0;        //マルチ、スライスプランのループ回数

        
        private float frameRate = 15f;
        private int IntegCnt = 0;
        private bool withsetImageOn = false;

        //セマフォハンドル
        private IntPtr hSemaphore = IntPtr.Zero;
        //Rev20.00 追加 by長野 2014/08/02
        private IntPtr hCapSemaphore = IntPtr.Zero;
 
        //Scan用変数
        private const int LIMIT_VIEW_NUM = 9;                   //透視画像の更新を停止する残りﾋﾞｭｰ数の値        
        private const int SCBNUM = 30;                          //配列数
        private int parm_cob_num_vol = 0;                       //生データ分割数
        private int parm_shared_cob_num_vol = 0;                //共有メモリ分割数 追加 by長野 20140/8/02
        private int parm_cone_acq_view = 0;                     //データ取り込みを行うビュー数
        private int parm_cob_view = 0;                          //生データ１個当たりの基本ビュー数
        private int parm_Itgnum = 0;                            //積算枚数
        private int parm_View = 0;					            //ビュー数（360°あたり）
        private int parm_Acq_view = 0;                          //実際に取り込むビュー数 
        private int parm_hDevID = 0;				            //メカ制御ボードハンドル
        private int parm_mrc = 0;			                    //メカ制御ボードエラーステータス            
        private int parm_table_rotation = 0;                    //テーブル回転モード 0:ステップ 1:連続
        private int parm_thinning_frame_num = 0;                //透視画像表示時の間引きフレーム数
        private StringBuilder parm_shared_cob_mutexNames;       //有メモリのMutex名（';'でつなげて列挙）
        private StringBuilder parm_shared_cob_objNames;         //有メモリのオブジェクト名（';'でつなげて列挙）
        private int[] parm_shared_cob_chg = new int[SCBNUM];    //分割した各生ﾃﾞｰﾀの先頭ﾋﾞｭｰ数を入れるテーブル
        private int[] parm_shared_cob_hdd_chg = new int[SCBNUM];//共有メモリからあふれる場合に、共有メモリ→HDDへデータを移すビュー数のテーブル //Rev20.00 追加 by長野 2014/08/01
        private int parm_viewsize = 0;				            //１ビューのサイズ
        private int parm_pur_viewsize = 0;				        //１ビューのサイズ //Rev20.00 追加 by長野 2014/07/16
        //private int parm_sharedMemOffset = 0;		            //１ビュー取得するのに必要なオフセットサイズ
        private int parm_shared_viewsize = 0;		            //１ビュー取得するのに必要なオフセットサイズ Rev20.00 変数名変更 by長野 2014/09/11
        private int parm_Js = 0;				                // 
        private int parm_Je = 0;		                        //
        private int parm_saveFluoroImage = 0;                   //透視画像を保存するかどうか 追加 by長野 2015/02/09

        //追加 シングルスキャン用 by長野 2014/08/27
        private StringBuilder parm_shared_raw_mutexNames;       //有メモリのMutex名（';'でつなげて列挙）
        private StringBuilder parm_shared_raw_objNames;         //有メモリのオブジェクト名（';'でつなげて列挙）
        private int parm_rawdatasize = 0;			            //生データサイズ
        private int parm_shared_rawdatasize = 0;                //オフセット込みの生データサイズ
        private int parm_shared_view_size = 0;                  //オフセット込みの１ビューサイズ
        private float[] parm_Delta_Ysw;                         //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
        private float[] parm_Delta_Ysw_dash;                    //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
        private float[] parm_Delta_Ysw_2dash;                   //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
        private float[] parm_SPA;                               //スキャン位置校正用の変数
        private float[] parm_SPB;                               //スキャン位置校正用の変数
        private float[] parm_SPB_sft;                           //スキャン位置校正用の変数(シフト用) Rev23.10 追加 by長野 2015/10/06
        private int parm_vs;                                    //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
        private int parm_ve;                                    //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
        private int parm_vs_sft;                                //ﾗｲﾝﾃﾞｰﾀ化に必要な変数(シフト用) Rev23.10 追加 by長野 2015/10/06
        private int parm_ve_sft;                                //ﾗｲﾝﾃﾞｰﾀ化に必要な変数(シフト用) Rev23.10 追加 by長野 2015/10/06
        private int parm_multi_slice_num;                       //マルチスライス数
        private int parm_line_size;                             //検出器横方向画素数(FFT方式であれば、FFTサイズ)
        private int parm_ud_direction;                          //マルチスキャン時のテーブル移動方向
        private int parm_vsize;                                 //透視画像縦サイズ(Scancondpar.csvに記録されている値そのもの)
        private int parm_hsize;                                 //透視画像横サイズ(Scancondpar.csvに記録されている値そのもの)
        private int parm_HalfNoAutoCenteringFlg;                //オートセンタリング用フラグ by長野 2015/01/24

        //Rev21.00 スキャノ用追加 by長野 2015/02/19
        private int parm_mscanopt;                                 //スキャノ撮影回数
        private int parm_real_mscanopt;                            //スキャノ撮影回数(最小スライスピッチ時)
        private int parm_mscano_integ_number;                      //スキャノ積分枚数 
        private float parm_mscano_width;                             //スキャノ厚
        private float parm_mscano_min_width;                         //最小スキャノ厚
        private int parm_mscano_widthPix;                          //スキャノ厚(画素)
        private float parm_mscano_udpitch;                           //スキャノ昇降ピッチ(スキャノピッチ)
        private int parm_ud_type;                                  //昇降タイプ

        //Rev23.10 シフト用mainch 追加 by長野 2015/10/06
        private int parm_sft_mainch;                              //mainch(シフト分込み)
        private int parm_mainch;
        private int parm_det_sft_pix;                             //検出器移動量(画素)
        //Rev23.10 スキャンモード追加 by長野 2015/10/06
        private int parm_scan_mode;

        //Rev25.00 シフト用 左右の画像の輝度値調整用係数 by長野 2016/09/24
        private float parm_ShiftFImageMagVal;
        private float parm_ShiftFImageMagValL;
        private float parm_ShiftFImageMagValR;

        //Rev23.12 純生データ用パラメータ追加 by長野 2015/12/11
        private int parm_savePurDataFlg;
        private string parm_purDataBaseFileName;

        //Rev25.00 Wスキャン追加 by長野 2016/07/07
        private int parm_w_scan;

        //Scan独自のデリゲート宣言
        public delegate void ScanEndHandler(int Error);
        //Scan独自のイベント宣言
        public event ScanEndHandler CaptureScanEnd;
        //Rev20.00 publicへ変更
        public IntPtr[] hSharedCTDataMap = new IntPtr[SCBNUM];			    //共有メモリのハンドル配列

        //Rev20.01 積算1ビュー用として追加 by長野 2015/05/21
        public IntPtr hSharedCTConeViewLongDataMap;
        public IntPtr hSumImgSemaphore;

        public static bool DetSftCompleteFlg = false; //Rev23.10 シフト用に追加 by長野 2015/10/06

        public static SFTPARA sft_par = new SFTPARA();

        //Rev25.00 ラインプロファイル 追加 by長野 2016/08/09 --->
        //データ
        private ushort[] LProfileV;
        private ushort[] LProfileH;
        //位置
        private int myLProfileVPos = 0;
        public int LProfileVPos
        {
            get
            {
                return myLProfileVPos;
            }
            set
            {
                myLProfileVPos = value;
            }
        }
        private int myLProfileHPos = 0;
        public int LProfileHPos
        {
            get
            {
                return myLProfileHPos;
            }
            set
            {
                myLProfileHPos = value;
            }
        }
        //<---

        //-----------------------------------------------------------------------------
        // API関数
        //-----------------------------------------------------------------------------
        #region WinAPI
        //SECURITY_ATTRIBUTES
        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public int bInheritHandle;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateSemaphore(
            ref SECURITY_ATTRIBUTES securityAttributes, 
            int initialCount, 
            int maximumCount, 
            string name);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateSemaphore(
            IntPtr lpSemaphoreAttributes,
            int lInitialCount,
            int lMaximumCount,
            string lpName);

        [DllImport("kernel32.dll")]
        public static extern bool ReleaseSemaphore(
            IntPtr hSemaphore, 
            int lReleaseCount,
            ref int lpPreviousCount);

        //Rev20.00 追加 by長野 2014/08/02
        [DllImport("kernel32.dll")]
        public static extern int WaitForSingleObject(
            IntPtr hSemaphore,
            int dwMilliSeconds);

        ////Rev20.00 追加 by長野 2014/09/11
        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(
            IntPtr hSemaphore
            );

        //Rev25.00 by長野 2016/08/18 
        // Win32APIの呼び出し宣言
        [DllImport("KERNEL32.DLL")]
        private static extern uint
        GetPrivateProfileString(string lpAppName,
        string lpKeyName, string lpDefault,
        StringBuilder lpReturnedString, uint nSize,
        string lpFileName);

        #endregion WinAPI

        #region 共有メモリアクセスSharedCTData　API

        [DllImport("SharedCTData.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ReconCreateSharedCTConeDataMem(
            IntPtr[] hSharedCTDataMap,             //handle
            int shared_cob_num,				    // 作成可能な共有メモリの個数
            int cob_num,						// 生データの分割個数
            int sharedMemOffset,				// １ビュー取得するのに必要なオフセットサイズ
            int cob_view,						// 生データ１個当たりの基本ビュー数
            [MarshalAs(UnmanagedType.LPArray)]
            byte[] shared_cob_objName,         // 共有メモリのオブジェクト名配列
            [MarshalAs(UnmanagedType.LPArray)]
            byte[] shared_cob_mutexName        // 共有メモリのオブジェクト名配列
            );

        [DllImport("SharedCTData.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ReconCreateSharedCTConeDataMemForCT30K(
            IntPtr[] hSharedCTDataMap,
            int shared_cob_num,				//(I)作成可能な共有メモリの個数
            int cob_num,						//(I)生データの分割個数
            int sharedMemOffset,				//(I)１ビュー取得するのに必要なオフセットサイズ
            int cob_view,						//(I)生データ１個当たりの基本ビュー数
            StringBuilder shared_cob_objNames,			//(I)共有メモリのオブジェクト名列挙
            StringBuilder shared_cob_mutexNames			//(I)共有メモリのミューテックス名列挙
        );

        //Rev20.00 追加 by長野 2014/08/27
        [DllImport("SharedCTData.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ReconCreateSharedCTSingleDataMemForCT30k(
            IntPtr[] hSharedCTDataMap,
            int rawdatasize,				    //(I)生データサイズ
            int view,
            int line_size,
            int multi_slice_num,				//(I)マルチスライス数
            ref int shared_view_size,
            StringBuilder shared_raw_objNames,			//(I)共有メモリのオブジェクト名列挙
            StringBuilder shared_raw_mutexNames			//(I)共有メモリのミューテックス名列挙
        );
        //Rev20.00 廃止 by長野 2014/08/18
        //[DllImport("SharedCTData.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int ReconCreateSharedCTSingleDataMem(
        //    int multi_slice_num,				//(I)マルチスライス数
        //    int raw_size,						//(I)生データのサイズ
        //    ////[MarshalAs(UnmanagedType.LPStr, SizeConst = 256)]
        //    //[MarshalAs(UnmanagedType.LPStr)]
        //    //string[] shared_raw_objName         //(I)生データのオブジェクト名配列
        //    StringBuilder[] shared_raw_objName
        //);		

        [DllImport("SharedCTData.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetSharedCTConeViewData(
            ushort[] cone_raw,		            // 生データ１ビューの配列
            int viewsize,				        // １ビューのサイズ
            int sharedMemOffset,			    // １ビュー取得するのに必要なオフセットサイズ
            int view_num,				        // セットするビュー数番号
            int cob_num,				        // 生データ分割番号
            int cob_baseview,			        // 生データ１個当たりの基本ビュー数
            ////[MarshalAs(UnmanagedType.LPStr, SizeConst = 256)]
            //[MarshalAs(UnmanagedType.LPStr)]
            //string[] shared_cob_mutexName,	    // 共有メモリのミューテックス名配列
            //[MarshalAs(UnmanagedType.LPStr)]
            //string[] shared_cob_objName         // 共有メモリのオブジェクト名配列
            StringBuilder[] shared_cob_mutexName,
            StringBuilder[] shared_cob_objName
            );		

        [DllImport("SharedCTData.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetSharedCTConeViewDataForCT30K(
            ushort[] cone_raw,		            // 生データ１ビューの配列
            int viewsize,				        // １ビューのサイズ
            int sharedMemOffset,			    // １ビュー取得するのに必要なオフセットサイズ
            int view_num,				        // セットするビュー数番号
            int cob_num,				        // 生データ分割番号
            int cob_baseview,			        // 生データ１個当たりの基本ビュー数
            string shared_cob_mutexName,	    // 共有メモリのミューテックス名
            string shared_cob_objName           // 共有メモリのオブジェクト名
            );

        [DllImport("SharedCTData.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetSharedCTConeViewDataForCT30K(
            ushort[] cone_raw,		            // 生データ１ビューの配列
            int viewsize,				        // １ビューのサイズ
            int sharedMemOffset,			    // １ビュー取得するのに必要なオフセットサイズ
            int view_num,				        // セットするビュー数番号
            int cob_num,				        // 生データ分割番号
            int cob_baseview,			        // 生データ１個当たりの基本ビュー数
            string shared_cob_mutexName,	    // 共有メモリのミューテックス名
            string shared_cob_objName           // 共有メモリのオブジェクト名
            );

        //Rev20.00 追加 by長野 2014/08/27
        [DllImport("SharedCTData.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int DestroySharedCTConeDataMap(
            int cob_num,                        //仮の値でOK
            IntPtr hSharedCTDataMap		        //生データの共有メモリハンドル
            );

        //Rev20.00 追加 by長野 2014/08/27
        [DllImport("SharedCTData.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int DestroySharedCTSingleDataMap(
            IntPtr hSharedCTDataMap		        //生データの共有メモリハンドル
            );		

        //共有メモリに全データが入るかジャッジする	
        [DllImport("SharedCTData.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int jdg_SharedCTConeData(
            int acq_view,					    //(I)撮影ビュー数
            int view_size,					    //(I)１ビューのサイズ
            ref int sharedMemOffset,			//(O)１ビュー取得するのに必要な共有メモリのオフセットサイズ
            ref long sharedOneCobSize,	        //(O)共有メモリ上での生データ１個分のサイズ
            long memsize,				        //(I)使用可能なメモリサイズ
            int cob_view					    //(I)生データ１個当たりの基本ビュー数
            );

        //分割した各共有メモリの先頭ﾋﾞｭｰ数を設定
        [DllImport("Conereconlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int shared_cob_chg_set(
            int cob_view,	                    //(I)分割生ﾃﾞｰﾀ1ﾌｧｲﾙ当りの基本ﾋﾞｭｰ数
            int cob_num,	                    //(I)分割生ﾃﾞｰﾀ数
            int[] shared_cob_chg	            //(O)分割した各生ﾃﾞｰﾀの先頭ﾋﾞｭｰ数を入れるテーブル（最大100個） ﾋﾞｭｰ数は0オリジン
            );

        // Conerecon時の生データ分割数を計算	
        [DllImport("Conereconlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int recon_shared_cob_num_set(
            long memsize,			            //(I)使用可能メモリサイズ
            long sharedOneCobSize,	            //(I)共有メモリ上での生データ１個のサイズ
            int	cob_num				            //(I)生データ分割個数
            );

        //  Conerecon時の分割共有メモリの作成可能個数と
        //　共有メモリとHDD間の一括保存・読込テーブルを作成
        [DllImport("Conereconlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void shared_cob_hdd_chg_set(
            int cob_view,	                    //(I)分割生ﾃﾞｰﾀ1ﾌｧｲﾙ当りの基本ﾋﾞｭｰ数
            int cob_num,	                    //(I)分割生ﾃﾞｰﾀ数
            int shared_cob_num,                 //(O)共有メモリ作成可能個数
            int[] cob_hdd_chg,                  //(O)共有メモリ・HDD間の一括保存・読込テーブル
            ref int cob_hdd_chg_num,            //共有メモリとHDD間の一括保存・読込回数
            int shared_ctdata_flg               //共有メモリに全部入るかどうかのフラグ
            );
        
        //Rev20.00 追加 by長野 2014/08/27
        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void MakeLineData_short(
            ushort[] IMAGE,
            double[] linedat,
            int h_size,
            int v_size,
            float[] SPA,
            float[] SPB,
            int vs,
            float[] Delta_Ysw,
            float[] Delta_Ysw_dash,
            float[] Delta_Ysw_2dash,
            int msc,
            int multi_slice_num
            );

        //Rev20.00 追加 by長野 2014/08/27
        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void MakeLineData_long(
            int[] IMAGE,
            double[] linedat,
            int h_size,
            int v_size,
            float[] SPA,
            float[] SPB,
            int vs,
            float[] Delta_Ysw,
            float[] Delta_Ysw_dash,
            float[] Delta_Ysw_2dash,
            int msc,
            int multi_slice_num
            );

        //Rev20.00 追加 by長野 2014/08/27
        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void LineDataCopy(
            double[] linedat,
            double[] PIXLINE0,
            double[] PIXLINE1,
            double[] PIXLINE2,
            double[] PIXLINE3,
            double[] PIXLINE4,
            int view,
            int h_size,
            int line_size,
            int multi_slice_num,
            int ud_direction,
            int msc
            );

        //Rev20.00 追加 by長野 2014/08/27
        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void MakeLineData_shortToshort(
            ushort[] IMAGE,
            ushort[] linedat,
            int h_size,
            int v_size,
            float[] SPA,
            float[] SPB,
            int vs,
            float[] Delta_Ysw,
            float[] Delta_Ysw_dash,
            float[] Delta_Ysw_2dash,
            int msc,
            int multi_slice_num
            );

        //Rev20.00 追加 by長野 2014/08/27
        [DllImport("SharedCTData.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetSharedCTSingleDataForCT30k(
            double[] PIXLINE0,		            // 生データ１ビューの配列
            double[] PIXLINE1,		            // 生データ１ビューの配列
            double[] PIXLINE2,		            // 生データ１ビューの配列
            double[] PIXLINE3,		            // 生データ１ビューの配列
            double[] PIXLINE4,		            // 生データ１ビューの配列
            int shared_view_size,				        // １ビューのサイズ
            int cntView,			    // １ビュー取得するのに必要なオフセットサイズ
            string shared_cob_mutexName,	    // 共有メモリのミューテックス名
            string shared_cob_objName,           // 共有メモリのオブジェクト名
            int multi_slice_num,
            int view_size,			        // 生データ１個当たりの基本ビュー数
            int shared_rawdatasize
            );


        //Rev20.00 追加 by長野 2014/08/27
        [DllImport("Pulsar.dll")]
        public static extern void CopyWordToLong(
            ushort[] IMAGE,					//画像		
            int[] SUM_IMAGE,				//積算画像
            int SIZE						//画像サイズ
        );

        //Rev20.00 追加 by長野 2014/08/27
        [DllImport("Pulsar.dll")]
        public static extern void CopyWordToWord(
            ushort[] INMAGE,				//画像		
            ushort[] OUT_IMAGE,				//積算画像
            int offset,                     //オフセット位置
            int SIZE						//サイズ
        );

        //Rev20.01 追加 by長野 2015/05/20
        [DllImport("SharedCTData.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr CreateSharedCTConeViewLongDataMap(
            int viewsize						            // 生データ1ビュー(積算済)当たりの基本ビュー数
        );

        //Rev20.01 追加 by長野 2015/05/20
        [DllImport("SharedCTData.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int DestroySharedCTConeViewLonfDatamap(
            IntPtr hSharedCTConeViewLongDataMap		        // 生データ1ビュー(積算済)当たりの共有メモリハンドル
        );

        //Rev20.01 追加 by長野 2015/05/20
        [DllImport("SharedCTData.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetSharedCTConeViewLongData(
            int[] sum_image,		            // 生データ１ビューの配列
            int viewsize				        // １ビューのサイズ
            );

        //Rev20.01 追加 by長野 2015/05/20
        [DllImport("SharedCTData.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetSharedCTConeViewLongData(
            int[] sum_image,		            // 生データ１ビューの配列
            int viewsize				        // １ビューのサイズ
            );

        //Rev20.01 追加 by長野 2015/05/20
        [DllImport("SendUDP.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SendMessToCT30K(
            string message		            // メッセージ
            );

        //Rev23.10 追加 by長野 2015/05/20
        [DllImport("Conebeamlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int sft_cone_makeForCT30K(
                ushort[] fst_cone_pur,		//入力ﾊﾞｯﾌｧ I:通常位置で収集した純生ﾃﾞｰﾀ先頭アドレス（ﾊﾞｯﾌｧのｻｲｽﾞはfimage_hsize*(je-js+1)）
                ushort[] sft_cone_pur,		//ｼﾌﾄ位置で収集したﾃﾞｰﾀ先頭ｱﾄﾞﾚｽ（ﾊﾞｯﾌｧのｻｲｽﾞはfimage_hsize*fimage_vsize）
                ushort[] cone_pur,			//Oﾊﾞｯﾌｧ（ﾊﾞｯﾌｧの横ｻｲｽﾞはscanpar.mainch） I:通常位置でのﾃﾞｰﾀ　O：合成ﾃﾞｰﾀ
                ushort[] cone_pur2,
                int	 js,				//フラットパネル用純生データの縦開始位置	Rev18.01 追加 11-06-20 by IWASAWA
                int	 je,				//フラットパネル用純生データの縦終了位置	Rev18.01 追加 11-06-20 by IWASAWA
                int  mainch,            //mainch(シフト込み)
                int  real_mainch,       //mainch
                int  det_sft_pix,       //検出器移動量(画素)
                int  wt_st1,
                int  wt_st2,
                int  wt_end1,
                int  wt_end2,
                double wt1,
                double wt2
            );
        
        #endregion 共有メモリアクセスSharedCTData　API



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TransImageControl(DetectorParam param)
        {
            ChangeDetector(param);
            IntegOn = false;
        }

        /// <summary>
        /// advクリア
        /// </summary>
        public void Clear_adv()
        {
            adv = 0;
        }

        /// <summary>
        /// 検出器パラメータ
        /// </summary>
        public DetectorParam Detector 
        { 
            get { return detector; }
            set { ChangeDetector(value); }
        }

        /// <summary>
        /// スキャンパラメータ設定
        /// </summary>
        public void SetScanParam(ScanParam param) 
        {
            scanParam = param;
        }

        /// <summary>
        /// Live中に画像セットを同時に行うフラグ
        /// </summary>
        public bool WithSetImageOn
        {
            get { return withsetImageOn ; }
            set { withsetImageOn = value; } 
        }
        
        /// <summary>
        /// 積算中フラグ
        /// </summary>
        public bool IntegOn { get; set; }

        ///<summary>
        /// オートコントラストフラグ Rev23.12 追加 by長野 2015/12/11
        /// </summary>
        private bool myAutoContrastOn = false;
        public bool AutoContrastOn
        {
            get
            {
                return myAutoContrastOn;
            }
            set
            {
                myAutoContrastOn = value;
            }
        }

        /// <summary>
        /// 共有メモリ破棄 //Rev20.00 追加 by長野 2014/09/11
        /// </summary>
        public void DestroyCTScanObj()
        {
            //Rev20.01 追加 by長野 2015/05/21
            if(hSharedCTConeViewLongDataMap != IntPtr.Zero)
            {
                CloseHandle(hSharedCTConeViewLongDataMap);
                hSharedCTConeViewLongDataMap = IntPtr.Zero;
            }
            if (hSumImgSemaphore != IntPtr.Zero)
            {
                CloseHandle(hSumImgSemaphore);
                hSumImgSemaphore = IntPtr.Zero;
            }
            
            if (hSemaphore != IntPtr.Zero)
            {
                CloseHandle(hSemaphore);
                hSemaphore = IntPtr.Zero;
            }
            //Rev20.00 ここで、確実に破棄 by長野 2014/08/27
            for (int cnt = 0; cnt < 30; cnt++)
            {
                if (hSharedCTDataMap[cnt] != IntPtr.Zero)
                {

                    TransImageControl.DestroySharedCTConeDataMap(0, hSharedCTDataMap[cnt]);
                    hSharedCTDataMap[cnt] = IntPtr.Zero;
                }
            }

            if(hCapSemaphore != IntPtr.Zero)
            {
                CloseHandle(hCapSemaphore);
                hCapSemaphore = IntPtr.Zero;
            }
        }

        //ここでは使用しない
        ///// <summary>
        ///// ズームスケール
        ///// </summary>
        //public int ZoomScale { get; set; }

        //ここでは使用しない
        ///// <summary>
        ///// 画像ビットインデックス
        ///// 0:8bit(256) 1:10bit(1024) 2:12bit(4096)
        ///// </summary>
        //public int FimageBitIndex { get; set; }

        /// <summary>
        /// 検出器パラメータ変更
        /// </summary>
        /// <param name="param"></param>
        private void ChangeDetector(DetectorParam param)
        {
            detector = param;
            ImageSize = new Size(detector.h_size, detector.v_size);
            //ViewSize = ImageSize;

            if (detector.DetType == DetectorConstants.DetTypePke)
            {
                SetLTSize(LTSize.LT16Bit);
            }
            else
            {
                SetLTSize(LTSize.LT12Bit);
            }
            
            int size = detector.h_size * detector.v_size;
            offsetImage = new double[size];
            gainImageL = new uint[size];
            transImage = new ushort[size];
            gainImage = new ushort[size];
            defImage = new ushort[size];
            workImage = new ushort[size];

            //Rev25.00 ラインプロファイルデータ 追加 by長野 2016/08/08;
            LProfileH = new ushort[detector.h_size];
            LProfileV = new ushort[detector.v_size];

            MirrorOn = detector.IsLRInverse;

            GC.Collect();
        }

        /// <summary>
        /// ラインプロファイルデータ取得 //Rev25.00 追加 by長野 2016/08/08
        /// </summary>
        public void GetLProfile(ref ushort[] lpH, ref ushort[] lpV)
        {
            if (lpH != null)
            {
                if (LProfileH.Length != lpH.Length)
                {
                    lpH = new ushort[LProfileH.Length];
                }
                LProfileH.CopyTo(lpH, 0);
                if (detector.DetType == DetectorConstants.DetTypePke || detector.DetType == DetectorConstants.DetTypeHama)
                {
                    Array.Reverse(lpH);
                }
            }
            if (lpV != null)
            {
                if (LProfileV.Length != lpV.Length)
                {
                    lpV = new ushort[LProfileV.Length];
                }
                LProfileV.CopyTo(lpV, 0);
            }
        }

        /// <summary>
        /// 欠陥とオフセットデータの設定
        /// </summary>
        public void SetDefGain(ushort[] defimage, ushort[] gainimage,int mAdv)
        {
            //条件追加2014/11/13hata
            //nullならコピーしない　
            if (defimage != null)
            {
                if (defImage.Length != defimage.Length)
                {
                    defImage = new ushort[defimage.Length];
                }
                defimage.CopyTo(defImage, 0);
            }

            //条件追加2014/11/13hata
            //nullならコピーしない　
            if (gainimage != null)
            {
                if (gainImage.Length != gainimage.Length)
                {
                    gainImage = new ushort[gainimage.Length];
                }
                gainimage.CopyTo(gainImage, 0);
            }

            adv = mAdv;
   
        }

        /// <summary>
        /// Pke用の校正データの設定
        /// </summary>
        public void SetPkeOffsetGain(double[] offsetimage, uint[] gainimageL)
        {
            //条件追加2014/11/13hata
            //nullならコピーしない　
            if (offsetimage != null)
            {
                if (offsetimage.Length != offsetimage.Length)
                {
                    offsetImage = new double[offsetimage.Length];
                }
                offsetimage.CopyTo(offsetImage, 0);
            }
            //条件追加2014/11/13hata
            //nullならコピーしない　
            if (gainimageL != null)
            {
                if (gainImageL.Length != gainimageL.Length)
                {
                    gainImageL = new uint[gainimageL.Length];
                }
                gainimageL.CopyTo(gainImageL, 0);
            }
        }

        /// <summary>
        /// 撮影画像データにセットする
        /// </summary>
        /// <param name="image"></param>
        public void SetTransImage(ushort[] image)
        {
            //条件追加2014/11/13hata
            //nullならコピーしない　
            if (image != null)
            {
                if (image.Length == transImage.Length)
                {
                    Array.Copy(image, transImage, transImage.Length);
                    //MakePicture(transImage);
                }
            }
        }

        ///// <summary>
        ///// 透視画像更新イベント発行
        ///// </summary>
        //private void OnTransImageChanged()
        //{
        //    if (TransImageChanged != null)
        //    {
        //        TransImageChanged(this, EventArgs.Empty);
        //    }
        //}

        #region キャプチャ

        //追加2014/10/07hata_v19.51反映
        //DetctorShiftを追加
        /// <summary>
        /// キャプチャ用ドライバオープン（Pke用のオフセットとゲイン画像を格納する）
        /// </summary>
        /// <param name="offsetimage"></param>
        /// <param name="gainimageL"></param>
        /// <param name="DetctorShift"></param>  
        /// <returns></returns>
        //public CaptureResult CaptureOpen(ref double[] offsetimage, ref uint[] gainimageL, bool DetctorShift = false)
        public CaptureResult CaptureOpen(ref double[] offsetimage, ref uint[] gainimageL, int DetctorShift = 0)//Rev23.20 引数変更 左右シフト対応 by長野 2015/11/19
        {
            CaptureResult res = CaptureResult.OK;

            res = CaptureOpen(DetctorShift);
            if (res == CaptureResult.OK)
            {
                gainimageL.CopyTo(gainImageL, 0);
                offsetimage.CopyTo(offsetImage, 0);
            }

            return res;
        }

        /// <summary>
        /// キャプチャ用ドライバオープン
        /// </summary>
        /// <param name="DetctorShift"></param>  
        /// <returns></returns>
        //public CaptureResult CaptureOpen(bool DetctorShift = false)
        public CaptureResult CaptureOpen(int DetctorShift = 0) //Rev23.20 引数変更(左右シフト対応)
        {
            CaptureResult res = CaptureResult.OK;

            //撮り込み停止用の共有メモリを作成
            if ((PulsarHelper.hStopMap == IntPtr.Zero) || (PulsarHelper.hStopMap.ToInt32() == 0))
            {
                PulsarHelper.hStopMap = PulsarHelper.CreateUserStopMap();
            }
            
            switch (detector.DetType)
            {
                case DetectorConstants.DetTypeII:
                case DetectorConstants.DetTypeHama:
                    if (Pulsar.hMil == IntPtr.Zero)
                    {
                        Pulsar.hPke = IntPtr.Zero;
                        //frameRate = 25;

                        // 同期でオープン
                        Pulsar.hMil = Pulsar.MilOpen(detector.dcf[0], Pulsar.IMAGE_BIT16, Pulsar.M_SYNCHRONOUS);
                        if (Pulsar.hMil == IntPtr.Zero) res = CaptureResult.OpenErr;
                    }
                    break;

                case DetectorConstants.DetTypePke:
                    if (Pulsar.hPke == IntPtr.Zero)
                    {
                        Pulsar.hMil = IntPtr.Zero;

                        // オープン
                        Pulsar.hPke = Pulsar.PkeOpen(scanParam.fpd_gain, scanParam.fpd_integ, detector.v_capture_type);
                        if (Pulsar.hPke == IntPtr.Zero)
                        {
                            res = CaptureResult.OpenErr;
                        }
                        else
                        {
                            // オフセット校正セット（ファイルからリードし、メモリにセット）
                            int ret = Pulsar.PkeSetOffsetData(Pulsar.hPke, 0, offsetImage, 0);
                            if (ret == 1) res = CaptureResult.OffsetErr;
                            
                            // ゲイン校正セット（ファイルからリードし、メモリにセット）
                            //変更2014/10/07hata_v19.51反映
                            //ret = Pulsar.PkeSetGainData(Pulsar.hPke, 0, gainImageL, 0);
                            
                            //string file = detector.Gain_Correct_L_File;
                            //if (DetctorShift)
                            //{
                            //    //Rev23.20 検出器のシフト位置で読み込みファイルを変える by長野 2015/11/19
                                
                            //    file = detector.Gain_Correct_L_Sft_File;
                            //}

                            //Rev23.20 左右シフト対応 by長野 2015/11/19
                            string file = "";
                            switch(DetctorShift)
                            {
                                case 0:
                                    file = detector.Gain_Correct_L_File;
                                    break;
                                case 1:
                                    file = detector.Gain_Correct_L_Sft_R_File;
                                    break;
                                case 2:
                                    file = detector.Gain_Correct_L_Sft_L_File;
                                    break;
                                default:
                                    file = detector.Gain_Correct_L_File;
                                    break;
                            }

                            ret = Pulsar.PkeSetGainData(Pulsar.hPke, gainImageL, 0, file);

                            if (ret == 1) res = CaptureResult.GainErr;

                            // 欠陥マップセット（オフセット、ゲイン、欠陥データをPkeにプリセットする）
                            ret = Pulsar.PkeSetPixelMap(Pulsar.hPke, 1);
                            if (ret == 1) res = CaptureResult.PixelErr;
                       
                        }

                    }
                    break;
                default:
                    break;
            }

            return res;
        }

        /// <summary>
        /// キャプチャ用ドライバクローズ
        /// </summary>
        public void CaptureClose()
        {
  
            switch (detector.DetType)
            {
                case DetectorConstants.DetTypeII:
                case DetectorConstants.DetTypeHama:
                    if (Pulsar.hMil != IntPtr.Zero)
                    {
                        Pulsar.MilClose(Pulsar.hMil);
                        Pulsar.hMil = IntPtr.Zero;
                    }
                    break;
                case DetectorConstants.DetTypePke:
                    if (Pulsar.hPke != IntPtr.Zero)
                    {
                        Pulsar.PkeClose(Pulsar.hPke);
                        Pulsar.hPke = IntPtr.Zero;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// キャプチャON状態
        /// </summary>
        public bool CaptureOn { get { return live; } }

        /// <summary>
        /// キャプチャ開始
        /// </summary>
        public void CaptureStart()
        {
            //// ・キャプチャオープン
            //CaptureOpen();

            //積分画像を初期化
            int size = detector.h_size * detector.v_size;
            WorkImageLong = new int[size];
            IntegCnt = 0;

            transImage = new ushort[size];
            workImage = new ushort[size];


            // 本関数を抜けたあとに
            // ・付帯情報をクリアする

            // キャプチャ準備
            Pulsar.CaptureSetup(Pulsar.hMil, Pulsar.hPke);

            // キャプチャ開始
            Pulsar.CaptureSeqStart(Pulsar.hMil, Pulsar.hPke);

            // 階調変換による画像自動更新をオフにする
            AutoUpdate = false;
           
            // キャプチャ開始
            //if (captureThread != null)　//2013/03/08hata
            if (captureThread == null)
            {
                live = true;
                captureThread = new Thread(new ThreadStart(CaptureLive));
                captureThread.IsBackground = true;
                captureThread.Start();
            }

        }


        /// <summary>
        /// キャプチャ停止
        /// </summary>
        public void CaptureStop()
        {
            // キャプチャ停止
            try
            {
                if (captureThread != null)
                {
                    live = false;
                     
                    //スレッド停止
                    //captureThread.Abort();
                   
                    //captureThread.Join();                 
                    captureThread.Join(2000);
                    Debug.Print("Liveスレッド終了しました。");　
                }
            }
            catch
            {
            }
            finally
            {
                captureThread = null;
            }

            // 階調変換による画像自動更新
            AutoUpdate = true;

#if NoCamera
#else
            // 取り込みが完全に終了するのを待つ
            if ((detector.DetType == DetectorConstants.DetTypeII) ||
                (detector.DetType == DetectorConstants.DetTypeHama))
            {
                Pulsar.MilGrabWait(Pulsar.hMil);
            }
            else
            {
                Pulsar.CaptureSeqStop(Pulsar.hMil, Pulsar.hPke);
            }
#endif
            
            if (!IntegOn)
            {
                // 透視画像データを登録
                SetImage(transImage);
            }
            else
            {
                // 積分画像データを登録
                UpdateIntegImage();
            }

            // 本関数を抜けたあとに
            // ・付帯情報に表示
            // ・積算フラグをオフ
            //Rev20.00 frmTransImageへ移動 by長野 2015/02/25
            //IntegOn = false;
        }

        /// <summary>
        /// キャプチャLIVEスレッド
        /// </summary>
        private void CaptureLive()
        {
            Stopwatch sw = new Stopwatch();
            double TargetInterval = 1000.0 / 15;

            if (detector.FrameRate == 0)
            {
                detector.FrameRate = 15;
            }    
            frameRate = detector.FrameRate;
            
            //フレームレートの時間で回るようにする
            if (frameRate > 0)
            {
                TargetInterval = 1000.0 / frameRate;
            }

            //int waitTime = (int)TargetInterval;
            int defwaitTime = 10;
            int waitTime = defwaitTime;
            int dummy = 5;
            int Interval = 0;


#if NoCamera  //v17.00追加(ここから) byやまおか 2010/01/19


            int tmr_Tick_Count = 0;
#endif
            Pulsar.Mil9AddCommentInit(CommentSize, CommentColor); //Rev22.00 透視画像コメント用 by長野 2015/08/20

            while (live)
            {
                sw.Restart();

                // 待ち
                Thread.Sleep(waitTime);
               
                ////sw.Start();
                //sw.Restart();

#if NoCamera  //v17.00追加(ここから) byやまおか 2010/01/19

                string FileName = null;
                if (tmr_Tick_Count < 300)
                {
                    tmr_Tick_Count = tmr_Tick_Count + 1;
                }
                else
                {
                    tmr_Tick_Count = 1;
                }
                FileName = Path.Combine("e:\\デバッグ用透視画像1392×520", "Trans" + tmr_Tick_Count.ToString("000") + ".dat");
                //ImageOpen TransImage(0), FileName, h_size, v_size
                IICorrect.ImageOpen(ref transImage[0], FileName, detector.h_size, detector.v_size);

#else

                // 透視画像取得
                Pulsar.GetCaptureImage(Pulsar.hMil, Pulsar.hPke, transImage);

#endif
                // 表示更新
                Update();
                
                sw.Stop();
                // 待ち時間計算
                //waitTime = (int)(interval - sw.Elapsed.TotalMilliseconds);
                //if (waitTime < 0)
                //{
                //    waitTime = (int)interval;
                //}
                Interval = (int)(sw.Elapsed.TotalMilliseconds + dummy);
                if (TargetInterval > Interval)
                {
                    waitTime = (int)(TargetInterval - Interval);
                }
                else
                {
                    waitTime = 1;
                    dummy = 0;
                }


            }

        }


        //これはLiveスレッドでやっているので不要
        ////フレームレートの時間で回る必要がある
        //int Interval_LastTime = 0;
        //private void AdjustInterval(int TargetInterval)
        //{
        //    int dummy = 5;
        //    int CurrentTime = 0;
        //    int Interval = 0;

        //    CurrentTime = Winapi.GetTickCount();

        //    if (Interval_LastTime != 0)
        //    {
        //        Interval = CurrentTime - Interval_LastTime + dummy;
        //        if (TargetInterval > Interval)
        //        {
        //            //Sleep TargetInterval - Interval
        //            //PauseForDoEvents (TargetInterval - Interval) / 1000 'v17.02変更 byやまおか 2010/07/22
        //            //v17.61変更 byやまおか 2011/06/24
        //            //Intervalが大きいときはPauseForDoEvents()を使う
        //            if ((Interval > (1000 / frameRate)))
        //            {
        //                //PauseForDoEvents((TargetInterval - Interval) / 1000);
        //                //Intervalが小さいときはSleep()を使う

        //                //開始時間を設定
        //                int startTime = Winapi.GetTickCount();

        //                //一定時間待つ
        //                while ((Winapi.GetTickCount() < startTime + (TargetInterval - Interval)))
        //                {
        //                    Thread.Sleep(1);
        //                }

        //            }
        //            else
        //            {
        //                Thread.Sleep((int)(TargetInterval - Interval));
        //            }
        //        }
        //        CurrentTime = Winapi.GetTickCount();
        //    }
        //    Interval_LastTime = (int)CurrentTime;
        //}


        /// <summary>
        /// 透視画像更新処理
        /// </summary>
        /// <param name="captured"></param>
        public void Update(bool captured = true, int rasterop = 0)
        {
            if (captured)
            {
                // Ｘ線検出器がフラットパネルの場合
                if (detector.Use_FlatPanel)
                {
                    // ゲイン補正
                    if (scanParam.FPGainOn)
                    {
                        IICorrect.FpdGainCorrect(transImage, gainImage, detector.h_size, detector.v_size, adv);
                    }

                    // 欠陥補正
                    if (detector.DetType == DetectorConstants.DetTypeHama)
                    {
                        IICorrect.FpdDefCorrect_short(transImage, defImage, detector.h_size, detector.v_size, 0, detector.v_size - 1);
                    }
                }

                // 積分処理時はアベレージング処理やシャープ処理は実施しない
                if (!IntegOn)
                {
                    //ushort[] workImage = new ushort[transImage.Length];
                    if (scanParam.AverageOn)
                    {
                        int dataSize = detector.h_size * detector.v_size;
                        ImgProc.Averaging(transImage, workImage, dataSize, scanParam.AverageNum);
                        if (scanParam.SharpOn)
                        {
                            ImgProc.Sharpen(workImage, transImage, detector.h_size, detector.v_size, LTMax);
                        }
                        else
                        {
                            Array.Copy(workImage, transImage, transImage.Length);
                        }
                    }
                    else if (scanParam.SharpOn)
                    {
                        ImgProc.Sharpen(transImage, workImage, detector.h_size, detector.v_size, LTMax);
                        Array.Copy(workImage, transImage, transImage.Length);
                    }
                }
                else
                {
                    //積分
                    if (IntegCnt < scanParam.FIntegNum)
                    {
                        ImgProc.AddImageWord(transImage, WorkImageLong, transImage.Length);
                        IntegCnt = IntegCnt + 1;
                    }
                }

                //Rev25.00 ラインプロファイルONの場合 by長野 2016/08/08
                if (scanParam.LineProfOn)
                {
                    UpdateProfile();
                }
            }

            //描画オペレーションの設定
            RasterOp = rasterop;

            // 変換対象となる画像の配列を登録
            if (live && !withsetImageOn)
            {
                if (myAutoContrastOn == true)//オートコントラスト実行時はSetImageを行い、キャプチャスレッドからデータが引っ張ってこれるようにする by長野 2015/12/11
                {
                    SetImage(transImage);
                    myAutoContrastOn = false;
                }
                else
                {
                    MakePicture(transImage);
                }
            }
            else
            {
                SetImage(transImage);
            }

            if (IntegOn)
            {
                // Integeraイベント発行
                int integcnt = IntegCnt;
                int integnum = scanParam.FIntegNum;
                OnIntegralCountUp(integcnt, integnum);
            }

            // イベント発行
            OnTransImageChanged();

        }

        //ラインプロファイル更新
        public void UpdateProfile()
        {
            IICorrect.GetLProfileVH(transImage, LProfileV, LProfileH, detector.h_size, detector.v_size, LProfileVPos, LProfileHPos);
        }

        //積分画像の表示
        public void UpdateIntegImage()
        {
            if (WorkImageLong != null)
            {
                IICorrect.DivImage(ref WorkImageLong[0], ref transImage[0], detector.h_size, detector.v_size, scanParam.FIntegNum);
                SetImage(transImage);

                //Rev25.00 ラインプロファイルONの場合 by長野 2016/08/08
                if (scanParam.LineProfOn)
                {
                    IICorrect.GetLProfileVH(transImage, LProfileV, LProfileH, detector.h_size, detector.v_size, LProfileVPos, LProfileHPos);
                }

                // イベント発行
                OnTransImageChanged();
            }

        }
        #endregion  キャプチャ

        #region ここからスキャノ
        /// <summary>
        /// キャプチャScanoスレッド
        /// </summary>
        public int CaptureScanoStart(int ScanLoop = 0)
        {
            // スキャンスレッド起動
            if (captureScanThread == null)
            {
                // 起動
                ScanLoopNum = ScanLoop;
                bWaitScan = false;
                ScanError = 0;
                bScan = true;
                captureScanThread = new Thread(new ThreadStart(CaptureScano));
                captureScanThread.IsBackground = true;
                captureScanThread.Start();
            }
            else
            {
                return 1;
            }

            return 0;
        }
        /// <summary>
        /// キャプチャScanoスレッド
        /// </summary>
        public int CaptureScanoStart()
        {
            //通常スキャン
            int sts = CaptureScanoStart(0);
            return sts;
        }

        /// <summary>
        /// キャプチャScan停止
        /// </summary>
        public void CaptureScanoStop()
        {
            // キャプチャ停止
            try
            {
                if (captureScanThread != null)
                {
                    bScan = false;
                    bWaitScan = false;
                    ScanLoopNum = 0;

                    //スレッド停止
                    captureScanThread.Join(2000);
                    Debug.Print("CaptureScanoスレッド終了しました。");
                }
            }
            catch
            {
            }
            finally
            {
                captureScanThread = null;
            }

            // 階調変換による画像自動更新
            AutoUpdate = true;
            IntegOn = false;
        }

        /// <summary>
        /// キャプチャScanスレッド
        /// </summary>
        private void CaptureScano()
        {
            int sts = 0;
            int cnt = 1;

            while (bScan)
            {
                sts = CaptureScanoEx(
                    parm_cob_num_vol,               //生データ分割数
                    parm_shared_cob_num_vol,        //共有メモリ分割数 追加 by長野 2014/08/02
                    parm_mscanopt,                  //撮影回数
                    parm_real_mscanopt,             //撮影回数(最小ピッチ時)
                    parm_mscano_width,              //スキャノ厚
                    parm_mscano_min_width,          //最小スキャノ厚
                    parm_mscano_widthPix,           //スキャノ厚(画素)
                    parm_mscano_integ_number,       //積算枚数
                    parm_mscano_udpitch,			//スキャノ昇降ピッチ(スキャノピッチ)
                    parm_hsize,                     //透視画像横サイズ
                    parm_vsize,                     //透視画像縦サイズ
                    parm_hDevID,				    //メカ制御ボードハンドル
                    parm_mrc,			            //メカ制御ボードエラーステータス            
                    parm_shared_cob_mutexNames,     //共有メモリミューテックス名
                    parm_shared_cob_objNames,       //共有メモリオブジェクト名
                    parm_shared_cob_chg,            //共有メモリ切替テーブル
                    parm_pur_viewsize,				//変更 １ビューのサイズ(純生) 2014/07/17
                    parm_viewsize,		            //１ビューサイズ(純生)
                    parm_shared_viewsize,		    //１ビュー取得するのに必要なオフセットサイズ //Rev20.00 変数名変更 by長野 2014/09/11
                    parm_ud_type,                   //昇降タイプ
                    parm_Delta_Ysw,                 //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
                    parm_Delta_Ysw_dash,            //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
                    parm_Delta_Ysw_2dash,           //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
                    parm_SPA,                       //ｽｷｬﾝ位置校正結果
                    parm_SPB,                       //ｽｷｬﾝ位置校正結果
                    parm_vs,                        //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
                    parm_ve,                         //ﾗｲﾝﾃﾞｰﾀ化に必要な変数              
                    parm_savePurDataFlg,            //Rev23.12 追加 by長野 2015/12/11
                    parm_purDataBaseFileName        //Rev23.12 追加 by長野 2015/12/11                    
                    );

                if (sts != 0) break;
                if (ScanLoopNum > cnt)
                {
                    bWaitScan = true;
                    while (bWaitScan && bScan)
                    {
                        //撮影待ち
                        Thread.Sleep(1);
                    }
                    cnt = cnt + 1;
                }
                else
                {
                    break;
                }
            }
            bScan = false;
            bWaitScan = false;
            ScanLoopNum = 0;
            captureScanThread = null;
        }


        /// <summary>
        /// キャプチャScan処理
        /// </summary>
        private int CaptureScanoEx(
            int cob_num_vol,             //生データ分割数
            int shared_cob_num_vol,      //Rev20.00 共有メモリ分割数 by長野 201/08/02
            int mscanopt,                //データ取り込みを行うビュー数
            int real_mscanopt,           //スキャノポイント(最小スライスピッチ時)
            float mscano_width,          //生データ１個当たりの基本ビュー数
            float mscano_min_width,      //最小スライス厚
            int mscano_widthPix,         //積算枚数
            int mscano_integ_number,	 //ビュー数（360°あたり）
            float mscano_udpitch,        //スキャノ昇降ピッチ(スキャノピッチ)
            int h_size,                  //透視画像横サイズ
            int v_size,                  //透視画像縦サイズ
            int hDevID,				     //メカ制御ボードハンドル
            int mrc,			         //メカ制御ボードエラーステータス            
            StringBuilder shared_cob_mutexNames,
            StringBuilder shared_cob_objNames,
            int[] shared_cob_chg,
            int pur_viewsize,			 //(I)１ビューのサイズ
            int viewsize,			     //(I)１ビューのサイズ
            int sharedMemOffset,         //(I)１ビュー取得するのに必要なオフセットサイズ 
            int ud_type,                 //昇降タイプ 
            float[] Delta_Ysw,
            float[] Delta_Ysw_dash,
            float[] Delta_Ysw_2dash,
            float[] SPA,
            float[] SPB,
            int vs,
            int ve,
            int savePurDataFlg,                     //Rev23.12 純生データ保存フラグ 追加 by長野 2015/12/11
            string purDataBaseFileName              //Rev23.12 純生データ ベースファイル名 by長野 2015/12/11
            )
        {
            //    //**********************************************
            //    //何の情報が必要か
            //    //long  table_rotation,			//テーブル回転モード 0:ステップ 1:連続			Rev2.00追加
            //    //×---long detector,		    	//検出器種類 0:I.I. 1:浜FPD 2:PkeFPD			Rev2.00追加
            //    //long  Itgnum,					//積算枚数
            //    //	long  View,					//ビュー数（360°あたり）
            //    //DWORD hDevID,					//メカ制御ボードハンドル
            //    //long  Acq_View,					//データ取り込みを行うビュー数
            //    //**********************************************

            // 戻り値用変数
            int Error = 0;

            int cob_chg_cnt = 0;		//Rev16.30 分割先頭ﾋﾞｭｰ数ﾃｰﾌﾞﾙｶｳﾝﾀ(0の時はﾌｧｲﾙﾁｪﾝｼﾞしない) 10-05-17 by IWASAWA

            const int SIGKILL = 10;        //指定回数毎に、終了チェックを行う

            int cnt_capture = 0;		//キャプチャした回数

            string objName = "";
            string mutexName = "";
            char[] cmpstr = new char[] { ';' };


            //Rev23.12 追加 by長野 2015/12/11
            string purExt = ".pur";
            string purSaveFileName = "";
            bool purDataSaveExFlg = false;
            //scanselのフラグがONでも、ファイル名が空白の場合はフラグOFF
            if (savePurDataFlg == 1 && purDataBaseFileName != "")
            {
                purDataSaveExFlg = true;
            }

            //Rev20.00 追加 ストップウォッチ by長野 2014/10/06
            //long startTime = 0;
            //long endTime = 0;
            long[] elapsedTime = new long[6000];
            Stopwatch sw = new Stopwatch();

            float FrameRateForScan = Detector.FrameRateForScan; //Rev20.00 追加 by長野

            //Rev21.00 空振り用のテーブルを作る。
            //
            string buf = null;
            string[] strCell = null;
            double UdStep = (double)0.0;

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
                                UdStep = IsNumeric;
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

            long UdPPS = 0;
            float UdFrameRate = 0.0f;
            float diffHalfFrame = 0.0f;
            long cnt_table = 0;
            long tableNum = 0;
            long baseCapNum = 0;
            long[] pTable = null;
            long adjust_flg = 0;
            long cnt_adjustTable = 0;	//キャプチャとテーブル回転間のズレ補正用テーブルのカウンタ

            //昇降のパルスレート計算
            UdPPS = (long)(((double)FrameRateForScan / ((double)mscanopt * (double)mscano_integ_number)) * (((double)real_mscanopt * (double)mscano_min_width) / (double)UdStep));
            //パルスレートをフレームレートだとして換算
            UdFrameRate = (float)((float)UdPPS * (float)(mscanopt * mscano_integ_number) / (((double)real_mscanopt * (double)mscano_min_width) / (double)UdStep));
            //キャプチャのフレームレートとパルスレートをフレームレートへ換算した、その差が何回キャプチャしたら0.5フレームになるか
            diffHalfFrame = (float)((float)0.5 / (FrameRateForScan - UdFrameRate) * FrameRateForScan);
            //空振りを含めない総キャプチャ数
            baseCapNum = mscanopt * mscano_integ_number;

            if ((baseCapNum - diffHalfFrame) > 0)
            {
                adjust_flg = 1;
                //補正用テーブルは計算値＋1番目まで参照されるため、また万が一のエラーでもソフトが落ちないようにするため、tableNumは計算値+2とし、計算値+2番目には-1を入れておく。
                tableNum = (long)(((float)baseCapNum + diffHalfFrame - (float)0.5) / ((float)2.0 * diffHalfFrame - (float)1.0)) + 2;
                //printf("tableNum = %d\n",tableNum);//デバッグ用
                //補正用テーブルの領域を確保
                pTable = new long[tableNum];

                //printf("テーブル作成開始");
                for (cnt_table = 0; cnt_table < tableNum; cnt_table++)
                {
                    if (cnt_table == (tableNum - 1))
                    {
                        pTable[cnt_table] = -1;
                    }
                    else
                    {
                        pTable[cnt_table] = (long)(diffHalfFrame + (float)2.0 * diffHalfFrame * (float)cnt_table + (float)0.5);
                    }
                }
                //printf("first = %d\n",*(pTable));//デバッグ用
                //printf("last = %d\n",*(pTable + (tableNum-1)));//デバッグ用
            }

            ///////////////////////////////////

            try
            {

                if ((Error = Pulsar.CaptureSetup(Pulsar.hMil, Pulsar.hPke)) != 0)
                {
                    return Error;
                }

                // 透視画像のサイズ
                int size = detector.h_size * detector.v_size;

                // 積算用配列
                int[] SUM_IMAGE = new int[size];
                transImage = new ushort[size];

                // 加算平均画像用配列：テーブル連続回転時のみ使用
                ushort[] AVE_IMAGE10 = new ushort[size];

                //Rev20.00 追加 by長野 2014/12/04
                int[] tempSUM_IMAGE = new int[size];
                ushort[] tempAVE_IMAGE10 = new ushort[size];


                //保存用配列
                //int typesize = Marshal.SizeOf(AVE_IMAGE10.GetType().GetElementType());
                int len = detector.h_size;
                ushort[] saveImage = new ushort[len];
                //ラインデータ１ビュー分
                //ushort[] linedat = new ushort[len];

                ////束ね用パラメータ
                //int upPix = 0;
                //int lowPix = 0;
                //upPix = detector.v_size / 2 + (int)(parm_mscano_widthPix / 2);
                //lowPix = detector.h_size / 2 - (int)(parm_mscano_widthPix / 2);

                int cntsem = 0;
                //セマフォの作成
                hSemaphore = CreateSemaphore(IntPtr.Zero, 0, mscanopt, "TEST_SEMAPHORE");

                // キャプチャ開始	
                Pulsar.CaptureSeqStart(Pulsar.hMil, Pulsar.hPke);

                // PkeFPDの場合
                if (Pulsar.hPke != IntPtr.Zero)
                {
                    //	// 空振りキャプチャ
                    //	// キャプチャ＆画像取得
                    Pulsar.PkeCaptureOnly(Pulsar.hPke, transImage);
                }
                // MILキャプチャの場合
                if (Pulsar.hPke == IntPtr.Zero)
                {
                    //	// 空振りキャプチャ
                    //	// キャプチャ＆画像取得
                    Pulsar.MilGrabAndGet(Pulsar.hMil, transImage);
                }

                //Rev20.00 変更 by長野 2014/09/11
                float tmpL = 0;
                float tmpSec = 0;
                tmpSec = mscanopt * mscano_integ_number / FrameRateForScan;
                //tmpL = mscanopt * mscano_width;
                tmpL = (real_mscanopt - 1) * mscano_min_width;
                float UdSpeed = (float)(tmpL / tmpSec);
              
                //Rev21.00 debug test by長野 2015/02/23
                // テーブル連続昇降開始
                //Error = MechaControl.UdManual(hDevID, ud_type, UdSpeed);
                //Rev23.20 float→doubleへ変更 by長野 2015/12/21
                Error = MechaControl.UdManual(hDevID, ud_type, (float)UdSpeed);

                // エラーが発生している？
                if (Error != 0)
                {
                    // 何もしない
                }
                else
                {

                    // キャプチャ開始	
                    Pulsar.CaptureSeqStart(Pulsar.hMil, Pulsar.hPke);

                    // 階調変換による画像自動更新をオフにする
                    AutoUpdate = false;

                    cob_chg_cnt = 1;

                    // ビュー数分収集ループ
                    for (int cntView = 0; cntView < mscanopt; cntView++)
                    {
                        Array.Clear(SUM_IMAGE, 0, size);

                        // 積算枚数分の処理
                        for (long icnt = 0; icnt < mscano_integ_number; icnt++)
                        {
                            // PkeFPDの場合
                            if (Pulsar.hPke != IntPtr.Zero)
                            {
                                //Rev19.10 テーブル使用せずに連続回転スキャンするように変更 by長野 2012/07/30
                                ////Rev17.69 FPDにも追加 テーブルがキャプチャより0.5フレーム遅れたら、１フレーム空振りするキャプチャ処理を入れて、ズレを±0.5内にするよう調整する。 by 長野 2012-03-03
                                if ((pTable != null) && (cnt_capture == pTable[cnt_adjustTable]))
                                {
                                    cnt_adjustTable++;

                                    //	// 空振りキャプチャ
                                    //	// キャプチャ＆画像取得
                                    Pulsar.PkeCaptureOnly(Pulsar.hPke, transImage);
                                    cnt_capture++;	// 総キャプチャ数カウントアップ
                                }

                                // キャプチャ＆画像取得
                                Pulsar.PkeCaptureOnly(Pulsar.hPke, transImage);
                                cnt_capture++;		// 総キャプチャ数カウントアップ
                            }

                            //Rev20.00 追加 by長野 2014/10/06
                            //sw.Restart();

                            // MILキャプチャの場合
                            if (Pulsar.hPke == IntPtr.Zero)
                            {
                                // 画像キャプチャ＆コピー
                                Pulsar.MilGrabAndGet(Pulsar.hMil, transImage);
                                cnt_capture++;		// 総キャプチャ数カウントアップ
                            }

                            //thinning_frame_num = ct_com.scancondpar.thinning_frame_num;
                            // 指定された回数ごとにCT30Kに透視画像を表示させる
                            //キャプチャの残りﾋﾞｭｰ数がLIMIT_VIWE_NUM以下になったら透視画像の更新をしないように条件文を変更 v16.00 by 長野 10/01/15
                            if (((cntView * mscano_integ_number + icnt) % 50 == 0) && ((mscanopt - cntView) >= LIMIT_VIEW_NUM))
                            {
                                //表示
                                Update();
                            }

                            // 画像積算
                            if (mscano_integ_number > 1)
                            {
                                ImgProc.AddImageWord(transImage, SUM_IMAGE, size);

                            }
                            else
                            {
                                //TransImageControl.CopyWordToLong(transImage, SUM_IMAGE, size);
                                //Rev20.00 Cに置き換え by長野 2014/08/27
                                //transImage.CopyTo(AVE_IMAGE10, 0);
                                transImage.CopyTo(SUM_IMAGE, 0);
                            }

                        }

                        // 純生データの平均化を行う
                        Pulsar.DivImage_short_Scan(SUM_IMAGE, AVE_IMAGE10, mscano_integ_number, size);

                        //Rev23.12 純生データ保存有の場合は、ここで保存を行う by長野 2015/12/05
                        if (purDataSaveExFlg == true)
                        {
                            //ファイル名決定
                            purSaveFileName = purDataBaseFileName + (mscanopt + 1).ToString() + purExt;
                            //ファイルに落とす
                            IICorrect.ImageSave(ref AVE_IMAGE10[0], purSaveFileName, detector.h_size, detector.v_size);
                        }

                        //Array.Copyを使う
                        //Rev20.00 Cに変更 by長野 2014/08/27
                        //Array.Copy(AVE_IMAGE10, detector.h_size, saveImage, 0, len);
                        //Array.Copy(AVE_IMAGE10,512 * detector.h_size,saveImage,0,len);
                        //TransImageControl.CopyWordToWord(AVE_IMAGE10, saveImage, js * detector.h_size,len);

                        ////Rev21.00 by長野 2015/02/23
                        //MakeLineData_shortToshort(AVE_IMAGE10, linedat, h_size, v_size, SPA, SPB, vs, Delta_Ysw, Delta_Ysw_dash, Delta_Ysw_2dash, (int)0, 0);
                        MakeLineData_shortToshort(AVE_IMAGE10, saveImage, h_size, v_size, SPA, SPB, vs, Delta_Ysw, Delta_Ysw_dash, Delta_Ysw_2dash, (int)0, 0);

                        //Array.Copy(linedat, 0, saveImage, 0, len);

                        //Rev21.00 debug test by長野 2015/02/23
                        // Array.Copy(AVE_IMAGE10, 512 * detector.h_size, saveImage, 0, len);

                        if (Error == -1) break;

                        //共有メモリへコピー
                        mutexName = GetStringBuilderToStr(shared_cob_mutexNames, cmpstr, cob_chg_cnt - 1);
                        objName = GetStringBuilderToStr(shared_cob_objNames, cmpstr, cob_chg_cnt - 1);

                        if ((mutexName == "") || (objName == ""))
                        {
                            Error = -1;
                            break;
                        }

                        //共有メモリへコピー
                        Error = SetSharedCTConeViewDataForCT30K(
                            saveImage,		        //(I)生データ１ビューの配列
                            viewsize,				//(I)１ビューのサイズ
                            sharedMemOffset,		//(I)１ビュー取得するのに必要なオフセットサイズ
                            cntView,				//(I)セットするビュー数番号
                            cob_chg_cnt,			//(I)生データ分割番号
                            mscanopt,			    //(I)生データ１個当たりの基本ビュー数
                            mutexName,	            //(I)共有メモリのミューテックス名(分割番号に相当する）
                            objName		            //(I)共有メモリのオブジェクト名(分割番号に相当する）
                        );
                        //Rev20.00 test 追加 by長野 2014/09/11
                        if (Error != 0)
                        {
                            Debug.Print(cntView.ToString());
                        }

                        //引数
                        //cone_raw                      →　透視データの配列
                        //viewsize                      →　配列の数
                        //sharedMemOffset               →　shared_viewsize 
                        //view_num                      →　撮影したview cntView
                        //cob_num                       →　cob_chg_cnt
                        //cob_baseview                  →　Ct_com.scaninf.scan_par.cob_view
                        //shared_cob_objNames[cob_num]  →　mutexName
                        //shared_cob_objNames[cob_num]  →　objName

                        //2014/07/31 ソフト側の取り出しを実際の透視画像取得に対して1ビュー遅れにする。書き込みの遅延で画像がおかしくならないようにするための予防 by長野 2014/07/31
                        // セマフォを１解放する
                        if (cntView > 0)
                        //if (cntView > testTable[shared_cob_hdd_chg_cnt])
                        {
                            //if (((cntView >= shared_cob_hdd_chg[shared_cob_hdd_chg_cnt]) || (cntView <= shared_cob_hdd_chg[shared_cob_hdd_chg_cnt] + 2)))
                            //{
                            ReleaseSemaphore(hSemaphore, 1, ref cntsem);
                            //Rev20.00 使用しない by長野 2014/10/23
                            //if (cntView + 1 == shared_cob_hdd_chg[shared_cob_hdd_chg_cnt + 1])
                            //{
                            //    shared_cob_hdd_chg_cnt = shared_cob_hdd_chg_cnt + 1;
                            //    ReleaseSemaphore(hSemaphore, 10, ref cntsem);
                            //}
                            //}
                        }

                        // SIGKILL回に1回、終了チェック 
                        if ((cntView % SIGKILL) == 0)
                        {
                            //if (rstop_flg == 0)	//Rev17.40 10-10-20 by IWASAWA
                            //{
                            //    if ((Error=CTLib.sig_chk())!=0) break;				
                            //}
                            //else
                            //{
                            //    if ((Error=CTLib.sig_chk_rmdsk())!=0)	break;//Rev17.40 10-10-20 by IWASAWA
                            //}
                            //if ((Error = CTLib.sig_chk()) != 0) break;
                            //Rev20.00 CheckCancelに変更 by長野 2014/09/18
                            if ((CTAPI.PulsarHelper.CheckCancel()) == true)
                            {
                                //Rev20.00 スキャンソフト側もスキャンストップファイルで止めれるようにセマフォは全部開放する
                                ReleaseSemaphore(hSemaphore, mscanopt - cntView, ref cntsem);
                                Error = 1902;
                                break;
                            }

                            /*
                            // 放電プロテクト処理（連続回転ならば何もしない）  added by 山本　2004-6-30	
                            if ((Error=discharge_protect_process(discharge_protect, table_rotation, cont_time, idle_time, Tstart)) != 0) 
                            {
                                break;
                            }
                            */
                        }

                        //Thread.Sleep(0);
                    }
                    //2014/07/31 最後に全部解放する by長野 2014/07/31
                    ReleaseSemaphore(hSemaphore, 1, ref cntsem);

                }
            }
            finally
            {
                MechaControl.UdSlowStop(hDevID, null);

                //元の位置もどす?

                // キャプチャ停止時処理
                Pulsar.CaptureSeqStop(Pulsar.hMil, Pulsar.hPke);
                ScanError = Error;
                OnCaptureScanEnd();

            }

            return Error;

        }

        #endregion


        #region スキャンスレッド

        /// <summary>
        /// Scan前処理（Scanの共有メモリを設定）
        /// </summary>
        public int CaptureScanMemset(
            int mainch,                 // チャンネル
            int sft_mainch,             // Rev23.10 シフトスキャン対応 by長野 2015/10/06
            int multiscan_mode,         // Rev26.00/Rev23.32/25.02 マルチスキャンモード add by chouno 2017/02/13
            int scan_mode,              // Rev23.10 スキャンモード追加 by長野 2015/10/06
            int w_scan,                 // Rev25.00 Wスキャン追加 by長野 2016/07/07
            int md,                     // 　
            int js,                     //
            int je,                     //
            int cob_num,                // 分割数
            int cone_acq_view,          // データ取り込みを行うビュー数
            int cob_view,               // ビュー数（360°あたり）
            int Itgnum,                 // 積算枚数
            int View,					// ビュー数（360°あたり）
            int hDevID,					// メカ制御ボードハンドル
	        int mrc,			        // メカ制御ボードエラーステータス            
            int table_rotation,         // テーブル回転モード 0:ステップ 1:連続
            int thinning_frame_num,     // 透視画像表示時の間引きフレーム数
            int SharedMemSize,          // 必要な共有メモリサイズ Rev20.00 追加 by長野 2014/09/11
            int saveFluoroImage,        // 透視画像を保存するかどうか 追加 by長野 2015/02/09
            int det_sft_pix,            // 移動量(画素) by長野 2015/10/06
            int savePurFlg,             //純生データ保存するかどうか 追加 by長野 2015/12/11 Rev23.12
            string purDataBaseFileName, //純生データ ベースファイル名 追加 by長野 2015/12/11 Rev23.12
            float shiftFImageMagVal,     // 左右のシフト画像の輝度値調整用係数 by長野 2016/09/24 Rev25.00
            float shiftFImageMagValL,
            float shiftFImageMagValR
            )
        {

            // スキャンスレッド動作中は抜ける
            //Rev20.00 スキャンスレッド中に、メモリ量計算→確保の手順を行う(マルチスキャン・スライスプランのため)のでコメントアウト by長野 2014/09/11
            //if (captureScanThread != null) return -1;　

            // 戻り値用変数
            int Error = 0;

            int SCBNUM = 30;
            //Rev20.00 解放はct30kでやる by長野 2014/08/27
            //IntPtr[] hSharedCTDataMap = new IntPtr[SCBNUM];			    //共有メモリのハンドル配列
            int[] shared_cob_chg = new int[SCBNUM];                 //共有メモリ分割生ﾃﾞｰﾀ先頭ﾋﾞｭｰ数 ﾋﾞｭｰ数は0オリジン
            int[] shared_cob_hdd_chg = new int[SCBNUM]; 	        //共有メモリとHDD間の一括保存・読込先頭ﾋﾞｭｰ数 ﾋﾞｭｰ数は0オリジン
            
            StringBuilder shared_cob_objNames = new StringBuilder(SCBNUM * 256);       // 共有メモリオブジェクト名
            StringBuilder shared_cob_mutexNames = new StringBuilder(SCBNUM * 256);     //共有メモリミューテックス名
            //string objName = "";
            //string mutexName = "";
            //char[] cmpstr = new char[] {';'};

            int viewsize;
	        int	pur_viewsize;		// 純生ﾃﾞｰﾀ1view分のﾃﾞｰﾀ数
            int tmp_pur_viewsize;   // Rev23.10 追加 by長野 2015/10/29


            int cal_viewsize;       //Rev23.10 計算に使用するデータサイズ by長野 2015/10/06

            //int shared_hsize = 0;
            //int shared_vsize = 0;
            int shared_viewsize = 0;
            int shared_ctdata_flg;
            //int sharedMemOffset = 0;
            long sharedOneCobSize = 0;

            //const int SIGKILL = 30;        //指定回数毎に、終了チェックを行う

            //Rev20.00 共有メモリ分割個数、HDD⇔共有メモリ間の一括保存・読取ビュー数の計算 ここから
		    //全データが共有メモリに入るかどうかジャッジする
		    //全メモリは、本番では、C#側で確保後にコモン経由で取得できるように変更すること
		    //2014/07/31 by長野 
            //long memsize = 17179869184;//16GBをBに直した値
            //Rev20.00 2014/08/27 by長野 2014/08/28
            //Rev20.00 ct30k.iniから取得した値を使う by長野 2014/09/11
            long memsize = SharedMemSize;

            //long memsize = 6442450944;//6GBをBに直した値
		    //long memsize = 5368709120;//5GBをBに直した値

            //Rev23.10 スキャンモードによってデータサイズの計算を変更する by長野 2015/10/06
            //Rev25.00 条件を変更する by長野 2016/08/07
            //if (scan_mode != 4)
            if (scan_mode != 4 && w_scan != 1)
            {
                viewsize = sizeof(ushort) * mainch * md;
                //pur_viewsize = mainch * (je - js + 1);
                //Rev20.00 変更 by長野 2014/07/16
                pur_viewsize = mainch * (je - js + 1) * sizeof(ushort);
                tmp_pur_viewsize = mainch * (je - js + 1) * sizeof(ushort);
            }
            else
            {
                viewsize = sizeof(ushort) * sft_mainch * md;
                //pur_viewsize = mainch * (je - js + 1);
                //Rev20.00 変更 by長野 2014/07/16
                tmp_pur_viewsize = sft_mainch * (je - js + 1) * sizeof(ushort);
                pur_viewsize = mainch * (je - js + 1) * sizeof(ushort);
            }

            if (tmp_pur_viewsize <= viewsize)
            {
                cal_viewsize = viewsize;
            }
            else
            {
                cal_viewsize = tmp_pur_viewsize;
            }


            //切替回数(=分割生データ個数)
            int shared_cob_chg_num = cob_num;

            //Rev20.00 shared_viewsizeはcで計算するためコメントアウト by長野 2014/09/11
            //            //共有メモリへのオフセットは64KB単位で固定なため、64KBの倍数になる大きさで確保する。
            //            //縦・横どちらかの大きさが512の倍数であればよい。横を延ばすようにする。

            //            if (mainch % 256 != 0)
            //            {
            //                shared_hsize = (int)(mainch / 256 + 1) * 256;
            //            }
            //            else
            //            {
            //                shared_hsize = (int)mainch;
            //            }
            ////           if (md % 256 != 0)
            //            //Rev20.00 サイズはje、jsで定義する 2014/07/16
            //            if ((je - js) % 256 != 0)
            //            {
            //                //shared_vsize = (int)(md / 256 + 1) * 256;
            //                //Rev20.00 サイズはje、jsで定義する 2014/07/16
            //                shared_vsize = (int)((je - js)/256 + 1) * 256;
            //            }
            //            else
            //            {
            //                //Rev20.00 サイズはje、jsで定義する 2014/07/16
            //                //shared_vsize = (int)md;
            //                shared_vsize = (int)(je - js);
            //            }

            //            shared_viewsize = sizeof(ushort) * shared_hsize * shared_vsize;

            //shared_ctdata_flg = jdg_SharedCTConeData(
            //    cone_acq_view,viewsize, 
            //    ref sharedMemOffset,
            //    ref sharedOneCobSize,
            //   memsize,
            //    cone_acq_view);

            //Rev20.00 純生に変更 by長野 2014/07/16
            //            shared_ctdata_flg = jdg_SharedCTConeData(
            //               cone_acq_view, pur_viewsize,
            //                ref sharedMemOffset,
            //                ref sharedOneCobSize,
            //                memsize,
            //                cone_acq_v

            shared_ctdata_flg = jdg_SharedCTConeData(
                //cone_acq_view, pur_viewsize,
                cone_acq_view, cal_viewsize,//Rev23.10 引数変更 by長野 2015/10/06 
                //ref sharedMemOffset,
                ref shared_viewsize,//Rev20.00 変更 by長野 2014/09/11
                ref sharedOneCobSize,
                memsize,
                cob_view);


		    //共有メモリ切替テーブル作成
            Error = shared_cob_chg_set(cob_view, cob_num, shared_cob_chg);
		    if((Error != 0))
		    {
           	    //まだ開始前なのでこのまま抜ける
                return Error; 
		    }

            int shared_cob_num;
            int shared_cob_hdd_chg_num; //共有メモリとHDD間の意一括保存・読込を行うためのテーブル要素数
            
            //共有メモリ・HDD間の一括保存・読込テーブル作成
            shared_cob_num = 0;
            shared_cob_hdd_chg_num = 0;
            shared_cob_num = recon_shared_cob_num_set(memsize, sharedOneCobSize, cob_num);
            shared_cob_hdd_chg_set(
                cob_view, 
                cob_num, 
                shared_cob_num, 
                shared_cob_hdd_chg, 
                ref shared_cob_hdd_chg_num, 
                shared_ctdata_flg);	//Rev16.30 scan_view ではなくcone_acq_view by Yamakage 10-03-03


            ////Rev20.00 ここで、確実に破棄 by長野 2014/08/27
            //for (int cnt = 0; cnt < SCBNUM; cnt++)
            //{
            //    if (hSharedCTDataMap[cnt] != IntPtr.Zero)
            //    {
            //        DestroySharedCTConeDataMap(0, hSharedCTDataMap[cnt]);
            //    }
            //}

            //Rev26.00/Rev25.02/Rev23.32 スライスプランの場合はメモリ可変になるため、あらかじめ最大で作成 by長野 2016/09/03
            if (multiscan_mode == (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan)
            {
                //Rev20.01 積算1ビュー用のメモリとして追加 by長野 2015/05/20
                hSharedCTConeViewLongDataMap = CreateSharedCTConeViewLongDataMap(mainch * mainch * sizeof(int));
                if (hSharedCTConeViewLongDataMap == IntPtr.Zero)
                {
                    Error = 1071;
                }
            }
            else
            {
                //Rev20.01 積算1ビュー用のメモリとして追加 by長野 2015/05/20
                hSharedCTConeViewLongDataMap = CreateSharedCTConeViewLongDataMap(mainch * (je - js + 1) * sizeof(int));
                if (hSharedCTConeViewLongDataMap == IntPtr.Zero)
                {
                    Error = 1071;
                }

            }

            //Rev26.00/Rev23.32 スライスプランの場合はメモリ可変になるため、あらかじめ最大で作成 by長野 2016/09/03
            if (multiscan_mode == (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan)
            {
                //共有メモリ作成
                Error = ReconCreateSharedCTConeDataMemForCT30K(
                    hSharedCTDataMap,
                    12,
                    12,
                    //sharedMemOffset, 
                    347777,//6000*347777で2GBギリギリ
                    6000,
                    shared_cob_objNames,
                    shared_cob_mutexNames);
            }
            else
            {
                //共有メモリ作成
                Error = ReconCreateSharedCTConeDataMemForCT30K(
                    hSharedCTDataMap,
                    shared_cob_num,
                    cob_num,
                    //sharedMemOffset, 
                    shared_viewsize,//Rev20.00 変更 by長野 2014/09/11
                    cob_view,
                    shared_cob_objNames,
                    shared_cob_mutexNames);
            }

            // スキャンスレッドが起動していなかったらパラメターをセット
            //Rev20.00 スキャンスレッド中に、メモリ量計算→確保の手順を行う(マルチスキャン・スライスプランのため)のでコメントアウト by長野 2014/09/11
            //if (captureScanThread == null)
            //{
                //パラメータをグロ－バルデータにセット
                parm_cob_num_vol =cob_num;
                parm_shared_cob_num_vol = shared_cob_num;//Rev20.00 追加 by長野 2014/08/02
                parm_cone_acq_view =cone_acq_view;
                parm_cob_view = cob_view;
                parm_Itgnum = Itgnum;
                parm_View = View;
                parm_hDevID = hDevID;
                parm_mrc = mrc;
                parm_table_rotation =table_rotation;
                parm_thinning_frame_num = thinning_frame_num;
                parm_shared_cob_mutexNames =shared_cob_mutexNames;
                parm_shared_cob_objNames = shared_cob_objNames;
                parm_shared_cob_chg =shared_cob_chg;
                parm_shared_cob_hdd_chg = shared_cob_hdd_chg;//Rev20.00 追加 by長野 2014/08/01
                //Rev20.00 サイズは純生を使用する 2014/07/16
                //Rev20.00 parm_viewsizeに値は入れる。2014/07/17
                parm_viewsize = viewsize;
                parm_pur_viewsize = pur_viewsize;
                //parm_sharedMemOffset = sharedMemOffset;
                parm_shared_viewsize = shared_viewsize;//Rev20.00 変更 by長野 2014/09/11
                parm_Js = js;
                parm_Je = je;
                parm_saveFluoroImage = saveFluoroImage; //Rev20.00 追加 by長野 2015/02/09
                parm_sft_mainch = sft_mainch;//Rev23.10 シフトスキャン対応 by長野 2015/10/06
                parm_scan_mode = scan_mode;  //Rev23.10 スキャンモード追加 by長野 2015/10/06
                parm_mainch = mainch;         //Rev23.10 追加 by長野 2015/10/06
                parm_det_sft_pix = det_sft_pix; //Rev23.10 追加 by長野 2015/10/06
                parm_savePurDataFlg = savePurFlg; //Rev23.12 追加 by長野 2015/12/11
                parm_purDataBaseFileName = purDataBaseFileName; //Rev23.12 追加 by長野 2015/12/11 
                parm_w_scan = w_scan;        //Rev25.00 Wスキャン追加 by長野 2016/07/07
                parm_ShiftFImageMagVal = shiftFImageMagVal; //左右のシフト画像の輝度値調整用係数 by長野 2016/09/24
                parm_ShiftFImageMagValL = shiftFImageMagValL;
                parm_ShiftFImageMagValR = shiftFImageMagValR;
            //}

            #region　//別に移動
            ////########################################################################################################
            ////ここからスキャンスレッド　

            //int cob_chg_cnt = 0;		//Rev16.30 分割先頭ﾋﾞｭｰ数ﾃｰﾌﾞﾙｶｳﾝﾀ(0の時はﾌｧｲﾙﾁｪﾝｼﾞしない) 10-05-17 by IWASAWA
            ////int cob_chg_flg = 0;		//Rev19.10 どちらのファイルポインタを使うかを示すフラグ by長野 2012/09/04
            ////int rstop_flg = 0;		//0:スキャンストップはCドライブ　1:スキャンストップはRAMディスク

            ////テーブル回転方向
            //int RotDirect = (detector.DetType == DetectorConstants.DetTypeHama || detector.DetType == DetectorConstants.DetTypePke) ? 1 : 0;
            //int cnt_capture = 0;		//キャプチャした回数
            
            ////使っていない
            ////int cnt_adjustTable = 0;	//キャプチャとテーブル回転間のズレ補正用テーブルのカウンタ

            //// キャプチャ準備
            //Pulsar.CaptureSetup(Pulsar.hMil, Pulsar.hPke);

            //if ((Error = Pulsar.CaptureSetup(Pulsar.hMil, Pulsar.hPke)) != 0)
            //{
            //    return Error;
            //}

            //// 透視画像のサイズ
            //int size = detector.h_size * detector.v_size;

            //// 積算用配列
            //int[] SUM_IMAGE = new int[size];
            //transImage = new ushort[size];

            //// 加算平均画像用配列：テーブル連続回転時のみ使用
            //ushort[] AVE_IMAGE10 = new ushort[size];

            //int cntsem = 0;
            ////セマフォの作成
            //hSemaphore = CreateSemaphore(IntPtr.Zero, 0, cone_acq_view, "TEST_SEMAPHORE");


            ////回転ﾓｰﾄﾞが連続回転であれば連続回転開始		//Rev2.00追加
            //if (table_rotation == 1)
            //{		// 回転速度
            //    float RotSpeed = (float)(60.0 * frameRate / View / Itgnum);

            //    // テーブル連続回転開始		
            //    if ((Error = MechaControl.RotateManual(hDevID, (1 - RotDirect), RotSpeed, 1)) < 1)
            //    {
            //        // 正常に回転開始した場合
            //        Thread.Sleep(700);		//Rev17.40 回転が一定速度になる前にRotateManualを抜けてしまうため追加 10-10-26 by IWASAWA
            //        Error = 0;
            //    }
            //}

            //// エラーが発生している？
            //if (Error != 0)
            //{
            //    // 何もしない
            //}
            //else
            //{

            //    // キャプチャ開始	
            //    Pulsar.CaptureSeqStart(Pulsar.hMil, Pulsar.hPke);


            //    // 階調変換による画像自動更新をオフにする
            //    AutoUpdate = false;

            //    // ビュー数分収集ループ
            //    for (int cntView = 0; cntView < cone_acq_view; cntView++)
            //    {
            //        if (cntView == shared_cob_chg[cob_chg_cnt])
            //        {
            //            cob_chg_cnt++;
            //        }

            //        // 積算画像用メモリの０クリア
            //        //if (Itgnum > 1)
            //            SUM_IMAGE = new int[size];

            //        // 積算枚数分の処理
            //        for (long icnt = 0; icnt < Itgnum; icnt++)
            //        {
            //            // PkeFPDの場合
            //            if (Pulsar.hPke != IntPtr.Zero)
            //            {
            //                // キャプチャ＆画像取得
            //                Pulsar.PkeCaptureOnly(Pulsar.hPke, transImage);
            //            }


            //            // MILキャプチャの場合
            //            if (Pulsar.hPke == IntPtr.Zero)
            //            {
            //                // 画像キャプチャ＆コピー
            //                Pulsar.MilGrabAndGet(Pulsar.hMil, transImage);
            //                cnt_capture++;		// 総キャプチャ数カウントアップ
            //            }

            //            // 最後のループ時にメカを動かす
            //            if (icnt == Itgnum - 1)
            //            {

            //                // テーブル回転
            //                MechaControl.RotateTable(cntView, View, hDevID, mrc, table_rotation, RotDirect);	// Rev17.00 PkeFPD対応 byやまおか 2010-02-24
            //                //テーブル回転エラー処理	//Rev17.452/17.61追加 byやまおか 2011/07/29
            //                if (mrc != 0)
            //                {
            //                    Error = mrc;
            //                    break;
            //                }

            //            }
            //            //thinning_frame_num = ct_com.scancondpar.thinning_frame_num;
            //            // 指定された回数ごとにCT30Kに透視画像を表示させる
            //            //キャプチャの残りﾋﾞｭｰ数がLIMIT_VIWE_NUM以下になったら透視画像の更新をしないように条件文を変更 v16.00 by 長野 10/01/15
            //            if (((cntView * Itgnum + icnt) % thinning_frame_num == 0) && ((cone_acq_view - cntView) >= LIMIT_VIEW_NUM))
            //            {
            //                //表示


            //            }

            //            // 画像積算
            //            if (Itgnum > 1)
            //            {
            //                ImgProc.AddImageWord(transImage, SUM_IMAGE, size);

            //            }
            //            else
            //            {
            //                //CopyWordToLong(TRANS_IMAGE, SUM_IMAGE, size);
            //                transImage.CopyTo(AVE_IMAGE10, 0);
            //            }
                        
            //        }

            //        if (table_rotation == 0) ////Rev16.2 変更 by YAMAKAGE 10-01-19
            //        {

            //        }
            //        // テーブル連続回転時 
            //        else
            //        { 
            //            // 純生データの平均化を行う
            //            Pulsar.DivImage_short(SUM_IMAGE, AVE_IMAGE10, Itgnum, detector.h_size, detector.v_size);

            //        }
            //        if (Itgnum > 1)
            //        {
            //            // 純生データの平均化を行う
            //            Pulsar.DivImage_short(SUM_IMAGE, AVE_IMAGE10, Itgnum, detector.h_size, detector.v_size);
            //        }



            //        if (Error == -1) break;
            
                    
            //        //共有メモリへコピー
            //        mutexName = GetStringBuilderToStr(shared_cob_mutexNames, cmpstr, cob_chg_cnt);
            //        objName = GetStringBuilderToStr(shared_cob_objNames, cmpstr, cob_chg_cnt);


            //        //共有メモリへコピー
            //        Error　= SetSharedCTConeViewDataForCT30K(
            //            AVE_IMAGE10,		    //(I)生データ１ビューの配列
            //            viewsize,				//(I)１ビューのサイズ
            //            sharedMemOffset,		//(I)１ビュー取得するのに必要なオフセットサイズ
            //            cntView,				//(I)セットするビュー数番号
            //            cob_chg_cnt,			//(I)生データ分割番号
            //            cob_view,			    //(I)生データ１個当たりの基本ビュー数
            //            mutexName,	            //(I)共有メモリのミューテックス名(分割番号に相当する）
            //            objName		            //(I)共有メモリのオブジェクト名(分割番号に相当する）
            //        );
            //             //引数
            //           //cone_raw                      →　透視データの配列
            //              //viewsize                      →　配列の数
            //           //sharedMemOffset               →　shared_viewsize 
            //           //view_num                      →　撮影したview cntView
            //           //cob_num                       →　cob_chg_cnt
            //           //cob_baseview                  →　Ct_com.scaninf.scan_par.cob_view
            //           //shared_cob_objNames[cob_num]  →　mutexName
            //              //shared_cob_objNames[cob_num]  →　objName


            //        // セマフォを１解放する
            //        ReleaseSemaphore(hSemaphore, 1, ref cntsem);


            ////        // SIGKILL回に1回、終了チェック 
            ////        if ((m % SIGKILL) == 0)
            ////        {
            ////            //if (rstop_flg == 0)	//Rev17.40 10-10-20 by IWASAWA
            ////            //{
            ////            //    if ((Error=CTLib.sig_chk())!=0) break;				
            ////            //}
            ////            //else
            ////            //{
            ////            //    if ((Error=CTLib.sig_chk_rmdsk())!=0)	break;//Rev17.40 10-10-20 by IWASAWA
            ////            //}
            ////            if ((Error = CTLib.sig_chk())!=0) break;

            ////            /*
            ////            // 放電プロテクト処理（連続回転ならば何もしない）  added by 山本　2004-6-30	
            ////            if ((Error=discharge_protect_process(discharge_protect, table_rotation, cont_time, idle_time, Tstart)) != 0) 
            ////            {
            ////                break;
            ////            }
            ////            */
            ////        }
  

            //    }

            //    // キャプチャ停止時処理
            //    Pulsar.CaptureSeqStop(Pulsar.hMil, Pulsar.hPke);

            //}

            ////ここまでスキャンスレッド　
            ////########################################################################################################
            #endregion

            return Error;

        }
        /// <summary>
        /// シングルScan前処理（Scanの共有メモリを設定）2014/08/27 追加 by長野
        /// </summary>
        public int CaptureSingleScanMemset(
            long view,                  //ビュー数
            long Acq_view,              //実際に取り込むビュー数
            long line_size,             //FFTサイズ
            long h_size,                //画像横サイズ
            long v_size,                //画像縦サイズ
            long multi_slice_num,       //マルチスライス数
            int mainch,                 //チャンネル
            int sft_mainch,             //シフト用mainch Rev23.10 追加 by長野 2015/10/06
            int scan_mode,              //スキャンモード Rev23.10 追加 by長野 2015/10/06
            int multiscan_mode,         //マルチスキャンモード Rev25.03 add by chouno 2017/02/16
            int w_scan,                 //Wスキャン Rev25.00 追加 by長野 2016/07/07
            int Itgnum,                 //積算枚数
            int hDevID,					//メカ制御ボードハンドル
            int mrc,			        //メカ制御ボードエラーステータス            
            int table_rotation,         //テーブル回転モード 0:ステップ 1:連続
            int thinning_frame_num,     //透視画像表示時の間引きフレーム数
            float[] Delta_Ysw,          //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            float[] Delta_Ysw_dash,     //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            float[] Delta_Ysw_2dash,    //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            float[] SPA,                //スキャン位置校正用の変数
            float[] SPB,                //スキャン位置校正用の変数
            float[] SPB_sft,            //スキャン位置校正用の変数(シフト) by長野 2015/10/06
            int vs,                     //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            int ve,                     //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            int vs_sft,                 //ﾗｲﾝﾃﾞｰﾀ化に必要な変数(シフト) by長野 2015/10/06
            int ve_sft,                 //ﾗｲﾝﾃﾞｰﾀ化に必要な変数(シフト) by長野 2015/10/06
            int ud_direction,           //マルチスキャン時のテーブル移動方向
            int HalfNoAutoCenteringFlg, //オートセンタリングフラグ by長野 2015/01/24
            int saveFluoroImage,         //透視画像を保存するかどうか 追加 by長野 2015/02/09
            int savePurFlg,             //純生データ保存するかどうか 追加 by長野 2015/12/11 Rev23.12
            string purDataBaseFileName, //純生データ ベースファイル名 追加 by長野 2015/12/11 Rev23.12
            float shiftFImageMagVal,     //左右のシフト画像の輝度値調整用係数 by長野 2016/09/24 Rev25.00
            float shiftFImageMagValL,
            float shiftFImageMagValR
            )
        {

            // スキャンスレッド動作中は抜ける
            //Rev20.00 スキャンスレッド中に、メモリ量計算→確保の手順を行う(マルチスキャン・スライスプランのため)のでコメントアウト by長野 2014/09/11
            //if (captureThread != null) return -1;

            // 戻り値用変数
            int Error = 0;
            
            StringBuilder shared_raw_objNames = new StringBuilder(256);     // 共有メモリオブジェクト名
            StringBuilder shared_raw_mutexNames = new StringBuilder(256);   //共有メモリミューテックス名

            long rawdatasize = sizeof(double) * view * line_size;
            int shared_rawdatasize = 0;
            int shared_view_size = 0;

            //Rev25.03/Rev23.32 スライスプランの場合はメモリが可変になるため、あらかじめ最大で作成 by長野 2016/09/03
            if (multiscan_mode == (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan)
            {
                //共有メモリ作成
                Error = ReconCreateSharedCTSingleDataMemForCT30k(
                    hSharedCTDataMap,
                    (int)(sizeof(double) * 6000 * 8192),
                    (int)view,
                    (int)line_size,
                    (int)multi_slice_num,
                    ref shared_rawdatasize,
                    shared_raw_objNames,
                    shared_raw_mutexNames
                    );
            }
            else
            {
                //共有メモリ作成
                Error = ReconCreateSharedCTSingleDataMemForCT30k(
                    hSharedCTDataMap,
                    (int)rawdatasize,
                    (int)view,
                    (int)line_size,
                    (int)multi_slice_num,
                    ref shared_rawdatasize,
                    shared_raw_objNames,
                    shared_raw_mutexNames
                    );
            }

            //シングルの場合、共有メモリから1ビューの取り出しは行わないのでshared_rawdatasize = view_sizeでよい
            shared_view_size = (int)rawdatasize;

            //Rev20.00 スキャンスレッド中に、メモリ量計算→確保の手順を行う(マルチスキャン・スライスプランのため)のでコメントアウト by長野 2014/09/11
            // スキャンスレッドしていなかったらパラメタセット
            //if (captureThread == null)
            //{
                //パラメータをグロ－バルデータにセット
                parm_Itgnum = Itgnum;
                parm_View = (int)view;
                parm_Acq_view = (int)Acq_view;
                parm_hDevID = hDevID;
                parm_mrc = mrc;
                parm_table_rotation = table_rotation;
                parm_thinning_frame_num = thinning_frame_num;
                parm_shared_raw_mutexNames = shared_raw_mutexNames;
                parm_shared_raw_objNames = shared_raw_objNames;
                parm_rawdatasize = (int)rawdatasize;
                parm_shared_rawdatasize = (int)shared_rawdatasize;
                parm_shared_view_size = (int)shared_view_size;
                parm_Delta_Ysw = Delta_Ysw;
                parm_Delta_Ysw_dash = Delta_Ysw_dash;
                parm_Delta_Ysw_2dash = Delta_Ysw_2dash;
                parm_SPA = SPA;
                parm_SPB = SPB;
                parm_vs = vs;
                parm_ve = ve;
                parm_multi_slice_num = (int)multi_slice_num;
                parm_line_size = (int)line_size;
                parm_ud_direction = ud_direction;
                parm_hsize = (int)h_size;
                parm_vsize = (int)v_size;
                parm_saveFluoroImage = saveFluoroImage; //Rev20.00 追加 by長野 2015/02/09
                parm_HalfNoAutoCenteringFlg = HalfNoAutoCenteringFlg; //Rev20.00 追加 by長野 2015/02/10
                parm_sft_mainch = sft_mainch; //Rev23.10 スキャンモード追加 by長野 2015/10/06
                parm_scan_mode = scan_mode;   //Rev23.10 スキャンモード追加 by長野 2015/10/06
                parm_mainch = mainch;         //Rev23.10 追加 by長野 2015/10/06
                parm_SPB_sft = SPB_sft;       //Rev23.10 追加 by長野 2015/10/06
                parm_vs_sft = vs_sft;         //Rev23.10 追加 by長野 2015/10/06
                parm_ve_sft = ve_sft;         //Rev23.10 追加 by長野 2015/10/06
                parm_savePurDataFlg = savePurFlg; //Rev23.11 追加 by長野 2015/12/11
                parm_purDataBaseFileName = purDataBaseFileName; //Rev23.12 追加 by長野 2015/12/11
                parm_w_scan = w_scan;        //Rev25.00 Wスキャン追加 by長野 2016/07/07
                parm_ShiftFImageMagVal = shiftFImageMagVal; //左右のシフト画像の輝度値調整用係数 by長野 2016/09/24
                parm_ShiftFImageMagValL = shiftFImageMagValL;
                parm_ShiftFImageMagValR = shiftFImageMagValR;
                //}


            #region　//別に移動
            ////########################################################################################################
            ////ここからスキャンスレッド　

            //int cob_chg_cnt = 0;		//Rev16.30 分割先頭ﾋﾞｭｰ数ﾃｰﾌﾞﾙｶｳﾝﾀ(0の時はﾌｧｲﾙﾁｪﾝｼﾞしない) 10-05-17 by IWASAWA
            ////int cob_chg_flg = 0;		//Rev19.10 どちらのファイルポインタを使うかを示すフラグ by長野 2012/09/04
            ////int rstop_flg = 0;		//0:スキャンストップはCドライブ　1:スキャンストップはRAMディスク

            ////テーブル回転方向
            //int RotDirect = (detector.DetType == DetectorConstants.DetTypeHama || detector.DetType == DetectorConstants.DetTypePke) ? 1 : 0;
            //int cnt_capture = 0;		//キャプチャした回数

            ////使っていない
            ////int cnt_adjustTable = 0;	//キャプチャとテーブル回転間のズレ補正用テーブルのカウンタ

            //// キャプチャ準備
            //Pulsar.CaptureSetup(Pulsar.hMil, Pulsar.hPke);

            //if ((Error = Pulsar.CaptureSetup(Pulsar.hMil, Pulsar.hPke)) != 0)
            //{
            //    return Error;
            //}

            //// 透視画像のサイズ
            //int size = detector.h_size * detector.v_size;

            //// 積算用配列
            //int[] SUM_IMAGE = new int[size];
            //transImage = new ushort[size];

            //// 加算平均画像用配列：テーブル連続回転時のみ使用
            //ushort[] AVE_IMAGE10 = new ushort[size];

            //int cntsem = 0;
            ////セマフォの作成
            //hSemaphore = CreateSemaphore(IntPtr.Zero, 0, cone_acq_view, "TEST_SEMAPHORE");


            ////回転ﾓｰﾄﾞが連続回転であれば連続回転開始		//Rev2.00追加
            //if (table_rotation == 1)
            //{		// 回転速度
            //    float RotSpeed = (float)(60.0 * frameRate / View / Itgnum);

            //    // テーブル連続回転開始		
            //    if ((Error = MechaControl.RotateManual(hDevID, (1 - RotDirect), RotSpeed, 1)) < 1)
            //    {
            //        // 正常に回転開始した場合
            //        Thread.Sleep(700);		//Rev17.40 回転が一定速度になる前にRotateManualを抜けてしまうため追加 10-10-26 by IWASAWA
            //        Error = 0;
            //    }
            //}

            //// エラーが発生している？
            //if (Error != 0)
            //{
            //    // 何もしない
            //}
            //else
            //{

            //    // キャプチャ開始	
            //    Pulsar.CaptureSeqStart(Pulsar.hMil, Pulsar.hPke);


            //    // 階調変換による画像自動更新をオフにする
            //    AutoUpdate = false;

            //    // ビュー数分収集ループ
            //    for (int cntView = 0; cntView < cone_acq_view; cntView++)
            //    {
            //        if (cntView == shared_cob_chg[cob_chg_cnt])
            //        {
            //            cob_chg_cnt++;
            //        }

            //        // 積算画像用メモリの０クリア
            //        //if (Itgnum > 1)
            //            SUM_IMAGE = new int[size];

            //        // 積算枚数分の処理
            //        for (long icnt = 0; icnt < Itgnum; icnt++)
            //        {
            //            // PkeFPDの場合
            //            if (Pulsar.hPke != IntPtr.Zero)
            //            {
            //                // キャプチャ＆画像取得
            //                Pulsar.PkeCaptureOnly(Pulsar.hPke, transImage);
            //            }


            //            // MILキャプチャの場合
            //            if (Pulsar.hPke == IntPtr.Zero)
            //            {
            //                // 画像キャプチャ＆コピー
            //                Pulsar.MilGrabAndGet(Pulsar.hMil, transImage);
            //                cnt_capture++;		// 総キャプチャ数カウントアップ
            //            }

            //            // 最後のループ時にメカを動かす
            //            if (icnt == Itgnum - 1)
            //            {

            //                // テーブル回転
            //                MechaControl.RotateTable(cntView, View, hDevID, mrc, table_rotation, RotDirect);	// Rev17.00 PkeFPD対応 byやまおか 2010-02-24
            //                //テーブル回転エラー処理	//Rev17.452/17.61追加 byやまおか 2011/07/29
            //                if (mrc != 0)
            //                {
            //                    Error = mrc;
            //                    break;
            //                }

            //            }
            //            //thinning_frame_num = ct_com.scancondpar.thinning_frame_num;
            //            // 指定された回数ごとにCT30Kに透視画像を表示させる
            //            //キャプチャの残りﾋﾞｭｰ数がLIMIT_VIWE_NUM以下になったら透視画像の更新をしないように条件文を変更 v16.00 by 長野 10/01/15
            //            if (((cntView * Itgnum + icnt) % thinning_frame_num == 0) && ((cone_acq_view - cntView) >= LIMIT_VIEW_NUM))
            //            {
            //                //表示


            //            }

            //            // 画像積算
            //            if (Itgnum > 1)
            //            {
            //                ImgProc.AddImageWord(transImage, SUM_IMAGE, size);

            //            }
            //            else
            //            {
            //                //CopyWordToLong(TRANS_IMAGE, SUM_IMAGE, size);
            //                transImage.CopyTo(AVE_IMAGE10, 0);
            //            }

            //        }

            //        if (table_rotation == 0) ////Rev16.2 変更 by YAMAKAGE 10-01-19
            //        {

            //        }
            //        // テーブル連続回転時 
            //        else
            //        { 
            //            // 純生データの平均化を行う
            //            Pulsar.DivImage_short(SUM_IMAGE, AVE_IMAGE10, Itgnum, detector.h_size, detector.v_size);

            //        }
            //        if (Itgnum > 1)
            //        {
            //            // 純生データの平均化を行う
            //            Pulsar.DivImage_short(SUM_IMAGE, AVE_IMAGE10, Itgnum, detector.h_size, detector.v_size);
            //        }



            //        if (Error == -1) break;


            //        //共有メモリへコピー
            //        mutexName = GetStringBuilderToStr(shared_cob_mutexNames, cmpstr, cob_chg_cnt);
            //        objName = GetStringBuilderToStr(shared_cob_objNames, cmpstr, cob_chg_cnt);


            //        //共有メモリへコピー
            //        Error　= SetSharedCTConeViewDataForCT30K(
            //            AVE_IMAGE10,		    //(I)生データ１ビューの配列
            //            viewsize,				//(I)１ビューのサイズ
            //            sharedMemOffset,		//(I)１ビュー取得するのに必要なオフセットサイズ
            //            cntView,				//(I)セットするビュー数番号
            //            cob_chg_cnt,			//(I)生データ分割番号
            //            cob_view,			    //(I)生データ１個当たりの基本ビュー数
            //            mutexName,	            //(I)共有メモリのミューテックス名(分割番号に相当する）
            //            objName		            //(I)共有メモリのオブジェクト名(分割番号に相当する）
            //        );
            //             //引数
            //           //cone_raw                      →　透視データの配列
            //              //viewsize                      →　配列の数
            //           //sharedMemOffset               →　shared_viewsize 
            //           //view_num                      →　撮影したview cntView
            //           //cob_num                       →　cob_chg_cnt
            //           //cob_baseview                  →　Ct_com.scaninf.scan_par.cob_view
            //           //shared_cob_objNames[cob_num]  →　mutexName
            //              //shared_cob_objNames[cob_num]  →　objName


            //        // セマフォを１解放する
            //        ReleaseSemaphore(hSemaphore, 1, ref cntsem);


            ////        // SIGKILL回に1回、終了チェック 
            ////        if ((m % SIGKILL) == 0)
            ////        {
            ////            //if (rstop_flg == 0)	//Rev17.40 10-10-20 by IWASAWA
            ////            //{
            ////            //    if ((Error=CTLib.sig_chk())!=0) break;				
            ////            //}
            ////            //else
            ////            //{
            ////            //    if ((Error=CTLib.sig_chk_rmdsk())!=0)	break;//Rev17.40 10-10-20 by IWASAWA
            ////            //}
            ////            if ((Error = CTLib.sig_chk())!=0) break;

            ////            /*
            ////            // 放電プロテクト処理（連続回転ならば何もしない）  added by 山本　2004-6-30	
            ////            if ((Error=discharge_protect_process(discharge_protect, table_rotation, cont_time, idle_time, Tstart)) != 0) 
            ////            {
            ////                break;
            ////            }
            ////            */
            ////        }


            //    }

            //    // キャプチャ停止時処理
            //    Pulsar.CaptureSeqStop(Pulsar.hMil, Pulsar.hPke);

            //}

            ////ここまでスキャンスレッド　
            ////########################################################################################################
            #endregion

            return Error;

        }

        /// <summary>
        /// Scano前処理（Scanoの共有メモリを設定）//Rev21.00 追加 by長野 2015/03/08
        /// </summary>
        public int CaptureScanoMemset(
            int mscanopt,           // スキャノポイント
            int real_mscanopt,      // スキャノポイント(最小スライスピッチ時)
            int mscano_integ_number,// スキャノ積分枚数　
            float mscano_udpitch,   // スキャノ昇降ピッチ(スキャノピッチ)
            float mscano_width,     // スキャノ厚
            float mscano_min_width, // 最小スキャノ厚
            int mscano_widthPix,    // スキャノ厚(画素)
            int h_size,             // 透視画像縦サイズ
            int v_size,             // 透視画像横サイズ
            int hDevID,				// メカ制御ボードハンドル
            int mrc,		        // メカ制御ボードエラーステータス            
            int SharedMemSize,      // 共有メモリサイズ
            int ud_type,            // 昇降タイプ
            float[] Delta_Ysw,        // ラインデータ化に必要な変数
            float[] Delta_Ysw_dash,   // ラインデータ化に必要な変数
            float[] Delta_Ysw_2dash,  // ラインデータ化に必要な変数
            float[] SPA,              // ｽｷｬﾝ位置校正結果
            float[] SPB,              // ｽｷｬﾝ位置校正結果
            int vs,                // ラインデータ化に必要な変数
            int ve,                 // ラインデータ化に必要な変数
            int savePurFlg,             //純生データ保存するかどうか 追加 by長野 2015/12/11 Rev23.12
            string purDataBaseFileName  //純生データ ベースファイル名 追加 by長野 2015/12/11 Rev23.12
            )
        {
            // 戻り値用変数
            int Error = 0;

            int SCBNUM = 30;
            int[] shared_cob_chg = new int[SCBNUM];                 //共有メモリ分割生ﾃﾞｰﾀ先頭ﾋﾞｭｰ数 ﾋﾞｭｰ数は0オリジン
            int[] shared_cob_hdd_chg = new int[SCBNUM]; 	        //共有メモリとHDD間の一括保存・読込先頭ﾋﾞｭｰ数 ﾋﾞｭｰ数は0オリジン

            StringBuilder shared_cob_objNames = new StringBuilder(SCBNUM * 256);       // 共有メモリオブジェクト名
            StringBuilder shared_cob_mutexNames = new StringBuilder(SCBNUM * 256);     //共有メモリミューテックス名

            int viewsize;
            int pur_viewsize;		// 純生ﾃﾞｰﾀ1view分のﾃﾞｰﾀ数
            int shared_viewsize = 0;
            int shared_ctdata_flg;
            long sharedOneCobSize = 0;

            //const int SIGKILL = 30;        //指定回数毎に、終了チェックを行う

            //Rev20.00 共有メモリ分割個数、HDD⇔共有メモリ間の一括保存・読取ビュー数の計算 ここから
            //全データが共有メモリに入るかどうかジャッジする
            //全メモリは、本番では、C#側で確保後にコモン経由で取得できるように変更すること
            //2014/07/31 by長野 
            //long memsize = 17179869184;//16GBをBに直した値
            //Rev20.00 2014/08/27 by長野 2014/08/28
            //Rev20.00 ct30k.iniから取得した値を使う by長野 2014/09/11
            long memsize = SharedMemSize;


            viewsize = sizeof(ushort) * h_size;
            //Rev20.00 変更 by長野 2014/07/16
            pur_viewsize = viewsize;

            //切替回数(=分割生データ個数)
            int shared_cob_chg_num = 1;
            int dummy = 0;

            int cob_view = 0;
            cob_view = mscanopt;

            shared_ctdata_flg = jdg_SharedCTConeData(
                cob_view, pur_viewsize,
                //ref sharedMemOffset,
                ref shared_viewsize,//Rev20.00 変更 by長野 2014/09/11
                ref sharedOneCobSize,
                memsize,
                dummy);


            //共有メモリ切替テーブル作成
            Error = shared_cob_chg_set(cob_view, shared_cob_chg_num, shared_cob_chg);
            if ((Error != 0))
            {
                //まだ開始前なのでこのまま抜ける
                return Error;
            }

            int shared_cob_num = 1;

            //int shared_cob_hdd_chg_num; //共有メモリとHDD間の意一括保存・読込を行うためのテーブル要素数

            ////共有メモリ・HDD間の一括保存・読込テーブル作成
            //shared_cob_num = 0;
            //shared_cob_hdd_chg_num = 0;
            //shared_cob_num = recon_shared_cob_num_set(memsize, sharedOneCobSize, 1);
            //shared_cob_hdd_chg_set(
            //    cob_view,
            //    1,
            //    shared_cob_num,
            //    shared_cob_hdd_chg,
            //    ref shared_cob_hdd_chg_num,
            //    shared_ctdata_flg);	//Rev16.30 scan_view ではなくcone_acq_view by Yamakage 10-03-03


            //共有メモリ作成
            Error = ReconCreateSharedCTConeDataMemForCT30K(
                hSharedCTDataMap,
                shared_cob_num,
                1,
                //sharedMemOffset, 
                shared_viewsize,//Rev20.00 変更 by長野 2014/09/11
                cob_view,
                shared_cob_objNames,
                shared_cob_mutexNames);

            // スキャンスレッドが起動していなかったらパラメターをセット
            //Rev20.00 スキャンスレッド中に、メモリ量計算→確保の手順を行う(マルチスキャン・スライスプランのため)のでコメントアウト by長野 2014/09/11
            //if (captureScanThread == null)
            //{
            //パラメータをグロ－バルデータにセット
            parm_cob_num_vol = 1;
            parm_shared_cob_num_vol = 1;//Rev20.00 追加 by長野 2014/08/02
            parm_mscanopt = mscanopt;
            parm_real_mscanopt = real_mscanopt;
            parm_mscano_integ_number = mscano_integ_number;
            parm_mscano_udpitch = mscano_udpitch;
            parm_mscano_width = mscano_width;
            parm_mscano_min_width = mscano_min_width;
            parm_mscano_widthPix = mscano_widthPix;
            parm_hsize = (int)h_size;
            parm_vsize = (int)v_size;
            parm_hDevID = hDevID;
            parm_mrc = mrc;
            parm_shared_cob_mutexNames = shared_cob_mutexNames;
            parm_shared_cob_objNames = shared_cob_objNames;
            parm_shared_cob_chg = shared_cob_chg;
            parm_shared_cob_hdd_chg = shared_cob_hdd_chg;//Rev20.00 追加 by長野 2014/08/01
            parm_viewsize = viewsize;
            parm_pur_viewsize = pur_viewsize;
            parm_shared_viewsize = shared_viewsize;//Rev20.00 変更 by長野 2014/09/11
            parm_ud_type = ud_type;
            parm_vs = vs;
            parm_ve = ve;
            parm_Delta_Ysw = Delta_Ysw;
            parm_Delta_Ysw_dash = Delta_Ysw_dash;
            parm_Delta_Ysw_2dash = Delta_Ysw_2dash;
            parm_SPA = SPA;
            parm_SPB = SPB;
            parm_savePurDataFlg = savePurFlg; //Rev23.11 追加 by長野 2015/12/11
            parm_purDataBaseFileName = purDataBaseFileName; //Rev23.12 追加 by長野 2015/12/11
            //}

            #region　//別に移動
            ////########################################################################################################
            ////ここからスキャンスレッド　

            //int cob_chg_cnt = 0;		//Rev16.30 分割先頭ﾋﾞｭｰ数ﾃｰﾌﾞﾙｶｳﾝﾀ(0の時はﾌｧｲﾙﾁｪﾝｼﾞしない) 10-05-17 by IWASAWA
            ////int cob_chg_flg = 0;		//Rev19.10 どちらのファイルポインタを使うかを示すフラグ by長野 2012/09/04
            ////int rstop_flg = 0;		//0:スキャンストップはCドライブ　1:スキャンストップはRAMディスク

            ////テーブル回転方向
            //int RotDirect = (detector.DetType == DetectorConstants.DetTypeHama || detector.DetType == DetectorConstants.DetTypePke) ? 1 : 0;
            //int cnt_capture = 0;		//キャプチャした回数

            ////使っていない
            ////int cnt_adjustTable = 0;	//キャプチャとテーブル回転間のズレ補正用テーブルのカウンタ

            //// キャプチャ準備
            //Pulsar.CaptureSetup(Pulsar.hMil, Pulsar.hPke);

            //if ((Error = Pulsar.CaptureSetup(Pulsar.hMil, Pulsar.hPke)) != 0)
            //{
            //    return Error;
            //}

            //// 透視画像のサイズ
            //int size = detector.h_size * detector.v_size;

            //// 積算用配列
            //int[] SUM_IMAGE = new int[size];
            //transImage = new ushort[size];

            //// 加算平均画像用配列：テーブル連続回転時のみ使用
            //ushort[] AVE_IMAGE10 = new ushort[size];

            //int cntsem = 0;
            ////セマフォの作成
            //hSemaphore = CreateSemaphore(IntPtr.Zero, 0, cone_acq_view, "TEST_SEMAPHORE");


            ////回転ﾓｰﾄﾞが連続回転であれば連続回転開始		//Rev2.00追加
            //if (table_rotation == 1)
            //{		// 回転速度
            //    float RotSpeed = (float)(60.0 * frameRate / View / Itgnum);

            //    // テーブル連続回転開始		
            //    if ((Error = MechaControl.RotateManual(hDevID, (1 - RotDirect), RotSpeed, 1)) < 1)
            //    {
            //        // 正常に回転開始した場合
            //        Thread.Sleep(700);		//Rev17.40 回転が一定速度になる前にRotateManualを抜けてしまうため追加 10-10-26 by IWASAWA
            //        Error = 0;
            //    }
            //}

            //// エラーが発生している？
            //if (Error != 0)
            //{
            //    // 何もしない
            //}
            //else
            //{

            //    // キャプチャ開始	
            //    Pulsar.CaptureSeqStart(Pulsar.hMil, Pulsar.hPke);


            //    // 階調変換による画像自動更新をオフにする
            //    AutoUpdate = false;

            //    // ビュー数分収集ループ
            //    for (int cntView = 0; cntView < cone_acq_view; cntView++)
            //    {
            //        if (cntView == shared_cob_chg[cob_chg_cnt])
            //        {
            //            cob_chg_cnt++;
            //        }

            //        // 積算画像用メモリの０クリア
            //        //if (Itgnum > 1)
            //            SUM_IMAGE = new int[size];

            //        // 積算枚数分の処理
            //        for (long icnt = 0; icnt < Itgnum; icnt++)
            //        {
            //            // PkeFPDの場合
            //            if (Pulsar.hPke != IntPtr.Zero)
            //            {
            //                // キャプチャ＆画像取得
            //                Pulsar.PkeCaptureOnly(Pulsar.hPke, transImage);
            //            }


            //            // MILキャプチャの場合
            //            if (Pulsar.hPke == IntPtr.Zero)
            //            {
            //                // 画像キャプチャ＆コピー
            //                Pulsar.MilGrabAndGet(Pulsar.hMil, transImage);
            //                cnt_capture++;		// 総キャプチャ数カウントアップ
            //            }

            //            // 最後のループ時にメカを動かす
            //            if (icnt == Itgnum - 1)
            //            {

            //                // テーブル回転
            //                MechaControl.RotateTable(cntView, View, hDevID, mrc, table_rotation, RotDirect);	// Rev17.00 PkeFPD対応 byやまおか 2010-02-24
            //                //テーブル回転エラー処理	//Rev17.452/17.61追加 byやまおか 2011/07/29
            //                if (mrc != 0)
            //                {
            //                    Error = mrc;
            //                    break;
            //                }

            //            }
            //            //thinning_frame_num = ct_com.scancondpar.thinning_frame_num;
            //            // 指定された回数ごとにCT30Kに透視画像を表示させる
            //            //キャプチャの残りﾋﾞｭｰ数がLIMIT_VIWE_NUM以下になったら透視画像の更新をしないように条件文を変更 v16.00 by 長野 10/01/15
            //            if (((cntView * Itgnum + icnt) % thinning_frame_num == 0) && ((cone_acq_view - cntView) >= LIMIT_VIEW_NUM))
            //            {
            //                //表示


            //            }

            //            // 画像積算
            //            if (Itgnum > 1)
            //            {
            //                ImgProc.AddImageWord(transImage, SUM_IMAGE, size);

            //            }
            //            else
            //            {
            //                //CopyWordToLong(TRANS_IMAGE, SUM_IMAGE, size);
            //                transImage.CopyTo(AVE_IMAGE10, 0);
            //            }

            //        }

            //        if (table_rotation == 0) ////Rev16.2 変更 by YAMAKAGE 10-01-19
            //        {

            //        }
            //        // テーブル連続回転時 
            //        else
            //        { 
            //            // 純生データの平均化を行う
            //            Pulsar.DivImage_short(SUM_IMAGE, AVE_IMAGE10, Itgnum, detector.h_size, detector.v_size);

            //        }
            //        if (Itgnum > 1)
            //        {
            //            // 純生データの平均化を行う
            //            Pulsar.DivImage_short(SUM_IMAGE, AVE_IMAGE10, Itgnum, detector.h_size, detector.v_size);
            //        }



            //        if (Error == -1) break;


            //        //共有メモリへコピー
            //        mutexName = GetStringBuilderToStr(shared_cob_mutexNames, cmpstr, cob_chg_cnt);
            //        objName = GetStringBuilderToStr(shared_cob_objNames, cmpstr, cob_chg_cnt);


            //        //共有メモリへコピー
            //        Error　= SetSharedCTConeViewDataForCT30K(
            //            AVE_IMAGE10,		    //(I)生データ１ビューの配列
            //            viewsize,				//(I)１ビューのサイズ
            //            sharedMemOffset,		//(I)１ビュー取得するのに必要なオフセットサイズ
            //            cntView,				//(I)セットするビュー数番号
            //            cob_chg_cnt,			//(I)生データ分割番号
            //            cob_view,			    //(I)生データ１個当たりの基本ビュー数
            //            mutexName,	            //(I)共有メモリのミューテックス名(分割番号に相当する）
            //            objName		            //(I)共有メモリのオブジェクト名(分割番号に相当する）
            //        );
            //             //引数
            //           //cone_raw                      →　透視データの配列
            //              //viewsize                      →　配列の数
            //           //sharedMemOffset               →　shared_viewsize 
            //           //view_num                      →　撮影したview cntView
            //           //cob_num                       →　cob_chg_cnt
            //           //cob_baseview                  →　Ct_com.scaninf.scan_par.cob_view
            //           //shared_cob_objNames[cob_num]  →　mutexName
            //              //shared_cob_objNames[cob_num]  →　objName


            //        // セマフォを１解放する
            //        ReleaseSemaphore(hSemaphore, 1, ref cntsem);


            ////        // SIGKILL回に1回、終了チェック 
            ////        if ((m % SIGKILL) == 0)
            ////        {
            ////            //if (rstop_flg == 0)	//Rev17.40 10-10-20 by IWASAWA
            ////            //{
            ////            //    if ((Error=CTLib.sig_chk())!=0) break;				
            ////            //}
            ////            //else
            ////            //{
            ////            //    if ((Error=CTLib.sig_chk_rmdsk())!=0)	break;//Rev17.40 10-10-20 by IWASAWA
            ////            //}
            ////            if ((Error = CTLib.sig_chk())!=0) break;

            ////            /*
            ////            // 放電プロテクト処理（連続回転ならば何もしない）  added by 山本　2004-6-30	
            ////            if ((Error=discharge_protect_process(discharge_protect, table_rotation, cont_time, idle_time, Tstart)) != 0) 
            ////            {
            ////                break;
            ////            }
            ////            */
            ////        }


            //    }

            //    // キャプチャ停止時処理
            //    Pulsar.CaptureSeqStop(Pulsar.hMil, Pulsar.hPke);

            //}

            ////ここまでスキャンスレッド　
            ////########################################################################################################
            #endregion

            return Error;
        }

        //StringBuilder文字から"指定文字"で分割したIndexの文字取得する
        private string GetStringBuilderToStr(StringBuilder SB, char[] cmpchar, int index = 0)
        {
            string result = "";
            string[] sel = new string[] {};

            string cmpstr = new string(cmpchar);
            string SBstr = SB.ToString();
            string SBstrtmp ="";

            if (SBstr != "" || cmpstr != "")
            {
                SBstrtmp = SBstr;
                if (SBstrtmp.EndsWith(cmpstr.ToString()))
                {
                    SBstr = SBstrtmp.Remove(SBstr.Length - 1);
                }

                sel = SBstr.Split(cmpchar);
                if (sel.Length - 1 >= index)
                {
                    result = sel[index];

                }
            }
            return result;
        }


        /// <summary>
        /// キャプチャScanスレッド
        /// </summary>
        public int CaptureScanStart(int ScanLoop = 0)
        {
            // スキャンスレッド起動
            if (captureScanThread == null)
            {
                // 起動
                ScanLoopNum = ScanLoop;
                bWaitScan = false;
                ScanError = 0;
                bScan = true;
                captureScanThread = new Thread(new ThreadStart(CaptureScan));
                captureScanThread.IsBackground = true;
                captureScanThread.Start();
            }
            else
            {
                return 1;
            }

            return 0;
        }
        public int CaptureScanStart()
        {
            //通常スキャン
            int sts = CaptureScanStart(0);
            return sts;
        }

        /// <summary>
        /// キャプチャScan停止
        /// </summary>
        public void CaptureScanStop()
        {
            // キャプチャ停止
            try
            {
                if (captureScanThread != null)
                {
                    bScan = false;
                    bWaitScan = false;
                    ScanLoopNum = 0;

                    //スレッド停止
                    captureScanThread.Join(2000);
                    Debug.Print("CaptureScanスレッド終了しました。");
                }
            }
            catch
            {
            }
            finally
            {
                captureScanThread = null;
            }

            // 階調変換による画像自動更新
            AutoUpdate = true;
            IntegOn = false;
        }

        /// </summary>
        /// マルチやスライスプランのときのScan再スタート
        /// </summary>
        public int ResstartScan()
        {
            ////Rev20.00 追加 by長野 2014/10/27
            ////再構成ソフトの処理を待つ
            ////開始時間を設定
            //int StartTime = Winapi.GetTickCount();
            //int PauseTime = 10;
            ////一定時間待つ
            //while ((Winapi.GetTickCount() < StartTime + PauseTime * 1000))
            //{
            //    Application.DoEvents();
            //    //System.Threading.Thread.Sleep(1);		//秒
            //}

            if (!bScan) return -1;              //Restart無効
            if (!bWaitScan) return -1;          //Restart無効
            //if (ScanLoopNum > 1) return -1;     //Restart無効
            //Rev20.00 不等号を逆にする by長野 2014/09/02
            if (ScanLoopNum < 1) return -1;     //Restart無効

            bWaitScan = false;

            return 0;
        }

        /// <summary>
        /// キャプチャScanスレッド
        /// </summary>
        private void CaptureScan()
        {
            int sts = 0;
            int cnt = 1;

            while (bScan)
            {
                sts = CaptureScanEx(
                    parm_cob_num_vol,               //生データ分割数
                    parm_shared_cob_num_vol,        //共有メモリ分割数 追加 by長野 2014/08/02
                    parm_cone_acq_view,             //データ取り込みを行うビュー数
                    parm_cob_view,                  //生データ１個当たりの基本ビュー数
                    parm_Itgnum,                    //積算枚数
                    parm_View,					    //ビュー数（360°あたり）
                    parm_hDevID,				    //メカ制御ボードハンドル
                    parm_mrc,			            //メカ制御ボードエラーステータス            
                    parm_table_rotation,            //テーブル回転モード 0:ステップ 1:連続
                    parm_thinning_frame_num,        //透視画像表示時の間引きフレーム数
                    parm_shared_cob_mutexNames,     //
                    parm_shared_cob_objNames,       //
                    parm_shared_cob_chg,            //
                    parm_shared_cob_hdd_chg,        //Rev20.00 共有メモリからあふれる場合の、共有メモリからHDDへデータを移すビュー数 by長野 2001/08/01
                    //parm_viewsize,				    //１ビューのサイズ
                    parm_pur_viewsize,				//変更 １ビューのサイズ(純生) 2014/07/17
                    //parm_sharedMemOffset,		    //１ビュー取得するのに必要なオフセットサイズ
                    parm_shared_viewsize,		    //１ビュー取得するのに必要なオフセットサイズ //Rev20.00 変数名変更 by長野 2014/09/11
                    parm_Js,
                    parm_Je,
                    parm_saveFluoroImage,           //Rev20.00 追加 by長野 2015/02/09
                    parm_scan_mode,                 //Rev23.10 スキャンモード追加 by長野 2015/10/06 
                    parm_w_scan,                    //Rev25.00 Wスキャン追加 by長野 2016/07/07
                    parm_sft_mainch,                //Rev23.10 シフトスキャン対応 by長野 2015/10/06
                    parm_mainch,                    //Rev23.10 シフトスキャン対応 by長野 2015/10/06 
                    parm_det_sft_pix,                //Rev23.10 シフト移動量(画素) by長野 2015/10/08
                    parm_savePurDataFlg,            //Rev23.12 追加 by長野 2015/12/11
                    parm_purDataBaseFileName,       //Rev23.12 追加 by長野 2015/12/11
                    parm_ShiftFImageMagVal,          //Rev25.00 追加 by長野 2016/09/24
                    parm_ShiftFImageMagValL,
                    parm_ShiftFImageMagValR
                    );

                if (sts != 0) break;
                if (ScanLoopNum > cnt)
                {
                    bWaitScan = true;
                    while (bWaitScan && bScan)
                    {
                        //撮影待ち
                        Thread.Sleep(1);
                    }
                    MechaControl.RotateIndex(parm_hDevID, 0, 0, null);
                    cnt = cnt + 1;
                }
                else
                {
                    break;
                }
            }
            bScan = false;
            bWaitScan = false;
            ScanLoopNum = 0;
            captureScanThread = null;
        }
        

        /// <summary>
        /// キャプチャScan処理
        /// </summary>
        private int CaptureScanEx(
            int cob_num_vol,          //生データ分割数
            int shared_cob_num_vol,   //Rev20.00 共有メモリ分割数 by長野 201/08/02
            int cone_acq_view,        //データ取り込みを行うビュー数
            int cob_view,             //生データ１個当たりの基本ビュー数
            int Itgnum,               //積算枚数
            int View,					//ビュー数（360°あたり）
            int hDevID,				//メカ制御ボードハンドル
	        int mrc,			        //メカ制御ボードエラーステータス            
            int table_rotation,       //テーブル回転モード 0:ステップ 1:連続
            int thinning_frame_num,    //透視画像表示時の間引きフレーム数
            StringBuilder shared_cob_mutexNames,
            StringBuilder　shared_cob_objNames,
            int[] shared_cob_chg,
            int[] shared_cob_hdd_chg,   //共有メモリからあふれる場合の、共有メモリ→HDDへデータを移すビュー数のテーブル Rev20.00 追加 by長野 2014/08/01
            int viewsize,				//(I)１ビューのサイズ(純生(シフト分はなし))
            //int pur_viewsize,           //(I)１ビューのサイズ
            int sharedMemOffset,        //(I)１ビュー取得するのに必要なオフセットサイズ 
            int js,
            int je,
            int saveFluoroImage,        //Rev20.00 追加 by長野 2015/02/09
            int scan_mode,              //Rev23.10 スキャンモード 追加 by長野 2015/10/06
            int w_scan,                 //Rev25.00 Wスキャン      追加 by長野 2016/07/07
            int sft_mainch,             //Rev23.10 シフトスキャン対応 by長野 2015/10/06
            int mainch,                 //Rev23.10 シフトスキャン対応 by長野 2015/10/06
            int det_sft_pix,            //Rev23.10 シフト移動量(画素) by長野 2015/10/06
            int savePurDataFlg,         //Rev23.12 純生データ保存フラグ 追加 by長野 2015/12/11
            string purDataBaseFileName, //Rev23.12 純生データ ベースファイル名 by長野 2015/12/11
            float ShiftFImagMagVal,      //Rev25.00 左右のシフト画像の輝度値調整用係数 by長野 2016/09/24
            float ShiftFImageMagValL,
            float ShiftFImageMagValR
            )		                          
        {
            //    //**********************************************
            //    //何の情報が必要か
            //    //long  table_rotation,			//テーブル回転モード 0:ステップ 1:連続			Rev2.00追加
            //    //×---long detector,		    	//検出器種類 0:I.I. 1:浜FPD 2:PkeFPD			Rev2.00追加
            //    //long  Itgnum,					//積算枚数
            //    //	long  View,					//ビュー数（360°あたり）
            //    //DWORD hDevID,					//メカ制御ボードハンドル
            //    //long  Acq_View,					//データ取り込みを行うビュー数
            //    //**********************************************
            
            // 戻り値用変数
            int Error = 0;

            int cob_chg_cnt = 0;		//Rev16.30 分割先頭ﾋﾞｭｰ数ﾃｰﾌﾞﾙｶｳﾝﾀ(0の時はﾌｧｲﾙﾁｪﾝｼﾞしない) 10-05-17 by IWASAWA
            //int shared_cob_hdd_chg_cnt = 1;//Rev20.00 追加 by長野 2014/08/02
            //int cob_chg_flg = 0;		//Rev19.10 どちらのファイルポインタを使うかを示すフラグ by長野 2012/09/04
            //int rstop_flg = 0;		//0:スキャンストップはCドライブ　1:スキャンストップはRAMディスク

            const int SIGKILL = 10;        //指定回数毎に、終了チェックを行う
            
            //テーブル回転方向
            int RotDirect = (detector.DetType == DetectorConstants.DetTypeHama || detector.DetType == DetectorConstants.DetTypePke) ? 1 : 0;
            int cnt_capture = 0;		//キャプチャした回数
            //使っていない
            //int cnt_adjustTable = 0;	//キャプチャとテーブル回転間のズレ補正用テーブルのカウンタ

            string objName = "";
            string mutexName = "";
            char[] cmpstr = new char[] { ';' };

            //Rev23.12 追加 by長野 2015/12/11
            string purExt = ".pur";
            string purSaveFileName = "";
            bool purDataSaveExFlg = false;
            //scanselのフラグがONでも、ファイル名が空白の場合はフラグOFF
            if (savePurDataFlg == 1 && purDataBaseFileName != "")
            {
                purDataSaveExFlg = true;
            }

            int wt_st1;
            int wt_end1;
            int wt_st2;
            int wt_end2;
            double[] wt = new double[2];

            //Rev23.10 シフト用追加 by長野 2015/10/06
            //シフトスキャンの場合は、２回目のデータ収集を行うようにする。
            int sft_mode = 0;
            int sft_cnt_max = 0;
            //if (scan_mode == 4)
            //Rev25.00 Wスキャン追加 by長野 2016/07/07
            if (scan_mode == 4 || w_scan == 1)
            {
                sft_cnt_max = 2;
                wt_st1 = sft_par.wt_st1;
                wt_st2 = sft_par.wt_st2;
                wt_end1 = sft_par.wt_end1;
                wt_end2 = sft_par.wt_end2;
                wt[0] = sft_par.wt[0];
                wt[1] = sft_par.wt[1];
            }
            else
            {
                sft_cnt_max = 1;
            }

            //Rev20.00 追加 by長野 2014/9/23
            //Rev20.01 廃止 by長野 2015/05/20
            //int[] testTable = new int[30];

            //Rev20.00 追加 ストップウォッチ by長野 2014/10/06
            //long startTime = 0;
            //long endTime = 0;
            long[] elapsedTime = new long[6000];
            Stopwatch sw = new Stopwatch();

            float FrameRateForScan = Detector.FrameRateForScan; //Rev20.00 追加 by長野

            //Rev25.00 追加 by長野 2016/09/24 --->
            float MagVal = 0.0f;
            MagVal = ShiftFImagMagVal;
            //<---
            float MagValL = 0.0f;
            MagValL = ShiftFImageMagValL;
            float MagValR = 0.0f;
            MagValR = ShiftFImageMagValR;
            //<---

            try
            {
                //Rev20.01 廃止 by長野 2015/05/20
                ////Rev20.00 testTable初期値設定
                //for (int cnt = 0; cnt < 30; cnt++)
                //{
                //    testTable[cnt] = shared_cob_hdd_chg[cnt] + 9;
                //}

                // キャプチャ準備
                //Rev25.00 不要 by長野 2016/08/08
                //Pulsar.CaptureSetup(Pulsar.hMil, Pulsar.hPke);

                if ((Error = Pulsar.CaptureSetup(Pulsar.hMil, Pulsar.hPke)) != 0)
                {
                    return Error;
                }

                // 透視画像のサイズ
                int size = detector.h_size * detector.v_size;

                // 積算用配列
                int[] SUM_IMAGE = new int[size];
                transImage = new ushort[size];

                // 加算平均画像用配列：テーブル連続回転時のみ使用
                ushort[] AVE_IMAGE10 = new ushort[size];

                //Rev20.00 追加 by長野 2014/12/04
                int[] tempSUM_IMAGE = new int[size];
                ushort[] tempAVE_IMAGE10 = new ushort[size];

                //保存用配列
                //int typesize = Marshal.SizeOf(AVE_IMAGE10.GetType().GetElementType());
                int len = (je - js + 1) * detector.h_size;
                ushort[] saveImage = new ushort[len];

                //Rev20.01 追加 積算1ビュー保存用 by長野 2015/05/20
                int sum_viewsize = sizeof(int) * (je - js + 1) * detector.h_size;
                int[] sum_saveImage = new int[len];

                //Rev23.10 シフト生データ用 追加 by長野 2015/10/06
                ushort[] sft_pur1st = new ushort[sft_mainch * detector.v_size];
                ushort[] sft_pur2nd = new ushort[sft_mainch * (je - js + 1)];
                int sft_pur_size = sizeof(ushort) * sft_mainch * (je - js + 1);
                //Rev23.10 シフト生データ用(基準位置) 追加 by長野 2015/10/06
                len = (je - js + 1) * detector.h_size;
                ushort[] tmp_sft_pur = new ushort[len];

                //Rev23.10 追加 by長野 2015/10/06
                bool sft_flg = false;
                //if (scan_mode == 4)
                //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                if (scan_mode == 4 || w_scan == 1)
                {
                    sft_flg = true;
                }

                int cntsem = 0;
                //セマフォの作成
                if (hSemaphore != IntPtr.Zero)
                {
                    CloseHandle(hSemaphore);
                }
                hSemaphore = CreateSemaphore(IntPtr.Zero, 0, cone_acq_view, "TEST_SEMAPHORE");

                //Rev20.01 積算1ビュー保存用に追加 by長野
                if (hSumImgSemaphore != IntPtr.Zero)
                {
                    CloseHandle(hSemaphore);
                }
                hSumImgSemaphore = CreateSemaphore(IntPtr.Zero, 0, cone_acq_view, "TEST_SUMIMAGE_SEMAPHORE");

                long sumimg_timeout = 0;
                if(((Itgnum + 2) * (int)frameRate) > 10000)
                {
                    sumimg_timeout = (Itgnum + 2) * (int)frameRate;
                }
                else
                {
                    sumimg_timeout = 10000;
                }
                //

                //Rev20.00 使用しない 1ビューずつ保存するようにしたので、キャプチャが未保存のデータを上書きする可能性はほぼ0であるため
                ////Rev20.00 ステップの同期用セマフォ作成(共有メモリに生データが全部入らない場合に使用する) by長野 2014/08/02
                //int cntcapsem = 0;
                //if (table_rotation == 0)
                //{
                //    cntcapsem = 0;
                //    int iniReleaseNum = 0;
                //    iniReleaseNum = shared_cob_hdd_chg[2];
                //    hCapSemaphore = CreateSemaphore(IntPtr.Zero, 0, cone_acq_view, "CAP_SEMAPHORE");
                //    if (iniReleaseNum <= 0)//全生データが入る場合
                //    {
                //        ReleaseSemaphore(hCapSemaphore, cone_acq_view, ref cntcapsem);
                //    }
                //    else//全生データが入らない場合は、切替ビュー数まで解放。次回分はスキャンソフト側が解放する by長野 2014/08/02
                //    {
                //        ReleaseSemaphore(hCapSemaphore, iniReleaseNum, ref cntcapsem);
                //        //ReleaseSemaphore(hCapSemaphore, 10, ref cntcapsem);
                //    }
                //}


                // キャプチャ開始	
                Pulsar.CaptureSeqStart(Pulsar.hMil, Pulsar.hPke);

                if (saveFluoroImage == 1)
                {
                    //Rev20.00 透視画像保存追加 by長野 2015/01/15
                    // 積算枚数分の処理
                    for (long icnt = 0; icnt < Itgnum; icnt++)
                    {
                        // PkeFPDの場合
                        if (Pulsar.hPke != IntPtr.Zero)
                        {
                            // キャプチャ＆画像取得
                            Pulsar.PkeCaptureOnly(Pulsar.hPke, transImage);
                        }


                        // MILキャプチャの場合
                        if (Pulsar.hPke == IntPtr.Zero)
                        {
                            // 画像キャプチャ＆コピー
                            Pulsar.MilGrabAndGet(Pulsar.hMil, transImage);
                            cnt_capture++;		// 総キャプチャ数カウントアップ
                        }
                        // 画像積算
                        if (Itgnum > 1)
                        {
                            ImgProc.AddImageWord(transImage, SUM_IMAGE, size);

                        }
                        else
                        {
                            //CopyWordToLong(TRANS_IMAGE, SUM_IMAGE, size);
                            transImage.CopyTo(AVE_IMAGE10, 0);
                        }

                    }

                    if (Itgnum > 1)
                    {
                        Pulsar.DivImage_short(SUM_IMAGE, AVE_IMAGE10, Itgnum, detector.h_size, detector.v_size);
                        //Array.Copy(SUM_IMAGE, vs * detector.h_size, tempSUM_IMAGE, 0, len);
                    }
                    else
                    {
                        //Array.Copy(AVE_IMAGE10, 0, tempAVE_IMAGE10, 0, size);
                    }
                    ImgProc.ConvertMirror(ref AVE_IMAGE10[0], detector.h_size, detector.v_size);
                    //Rev20.00 拡張子が抜けていたので追加 by長野 2015/01/28
                    //IICorrect.ImageSave(ref AVE_IMAGE10[0], "C:\\CT\\TEMP\\SaveFluoroTemp.", detector.h_size, detector.v_size);
                    IICorrect.ImageSave(ref AVE_IMAGE10[0], "C:\\CT\\TEMP\\SaveFluoroTemp.img", detector.h_size, detector.v_size);
                }
    
                //Rev25.00 test by長野 2016/08/22
                //回転ﾓｰﾄﾞが連続回転であれば連続回転開始		//Rev2.00追加
                //if (table_rotation == 1)
                if (table_rotation == 1 && !(scan_mode == 4 || w_scan == 1))
                {		// 回転速度

                    //Rev20.00 変更 by長野 2014/09/11
                    float RotSpeed = (float)(60.0 * FrameRateForScan / View / Itgnum);
                    //Rev20.00 応急処置 by長野 2014/09/08
                    //float RotSpeed = (float)(60.0 * 29.973 / View / Itgnum);

                    // テーブル連続回転開始		
                    if ((Error = MechaControl.RotateManual(hDevID, (1 - RotDirect), RotSpeed, 1)) < 1)
                    {
                        // 正常に回転開始した場合
                        Thread.Sleep(700);		//Rev17.40 回転が一定速度になる前にRotateManualを抜けてしまうため追加 10-10-26 by IWASAWA
                        Error = 0;
                    }
                }

                // エラーが発生している？
                if (Error != 0)
                {
                    // 何もしない
                }
                else
                {

                    //Rev20.00 移動 by長野 2015/02/06
                    // キャプチャ開始	
                    //Pulsar.CaptureSeqStart(Pulsar.hMil, Pulsar.hPke);


                    // 階調変換による画像自動更新をオフにする
                    AutoUpdate = false;

                    // シフト回数分ループ
                    for (int sft_cnt = 0; sft_cnt < sft_cnt_max; sft_cnt++)
                    {
                        //if (scan_mode == 4)
                        //Rev25.00 Wスキャンを追加 by長野 2016/07/07
                        if (scan_mode == 4 || w_scan == 1)
                        {
                            if (sft_mode == 1)
                            {
                                //シフト位置への移動命令
                                //string message = "DetShift";
                                //Rev23.20 メッセージ変更 by長野 2015/11/19 
                                string message = "DetShiftScan";
                                
                                SendMessToCT30K(message);

                                if (hSemaphore != IntPtr.Zero)
                                {
                                    CloseHandle(hSemaphore);
                                    hSemaphore = IntPtr.Zero;
                                }

                                if (hSumImgSemaphore != IntPtr.Zero)
                                {
                                    CloseHandle(hSumImgSemaphore);
                                    hSumImgSemaphore = IntPtr.Zero;
                                }

                                if (hSemaphore != IntPtr.Zero)
                                {
                                    CloseHandle(hSemaphore);
                                }
                                //セマフォの作成
                                hSemaphore = CreateSemaphore(IntPtr.Zero, 0, View, "TEST_SEMAPHORE");

                                //Rev20.01 積算1ビュー保存用に追加 by長野
                                if (hSumImgSemaphore != IntPtr.Zero)
                                {
                                    CloseHandle(IntPtr.Zero);
                                }
                                hSumImgSemaphore = CreateSemaphore(IntPtr.Zero, 0, cone_acq_view, "TEST_SUMIMAGE_SEMAPHORE");
                             
                                //DetSftCompleteFlgをCT30kがONにするまで待つ。
                                while (DetSftCompleteFlg == false)
                                {
                                    Thread.Sleep(1000);
                                }
                                DetSftCompleteFlg = false;

                                Thread.Sleep(10000);

                                //Rev25.00 test by長野 2016/08/22
                                if (table_rotation != 1)
                                {
                                    Error = MechaControl.RotateSlowStop(hDevID, null);

                                    Error = MechaControl.RotateIndex(hDevID, 0, 0, null);
                                }
                                else
                                {
                                    //Error = MechaControl.RotateSlowStop(hDevID, null);

                                    Error = MechaControl.RotateIndex(hDevID, 0, 0, null);

                                    //Rev20.00 変更 by長野 2014/09/11
                                    float RotSpeed = (float)(60.0 * FrameRateForScan / View / Itgnum);
                                    //Rev20.00 応急処置 by長野 2014/09/08
                                    //float RotSpeed = (float)(60.0 * 29.973 / View / Itgnum);

                                    // テーブル連続回転開始		
                                    if ((Error = MechaControl.RotateManual(hDevID, (1 - RotDirect), RotSpeed, 1)) < 1)
                                    {
                                        // 正常に回転開始した場合
                                        Thread.Sleep(700);		//Rev17.40 回転が一定速度になる前にRotateManualを抜けてしまうため追加 10-10-26 by IWASAWA
                                        Error = 0;
                                    }
                                }

                            }
                            else if (sft_mode == 0)
                            {
                                //シフト位置への移動命令
                                //string message = "DetShiftOrg";
                                //Rev23.20 メッセージ変更 by長野 2015/11/19
                                string message = "DetShiftOrgScan";
                                SendMessToCT30K(message);

                                //Rev25.00 追加 by長野 2016/08/22
                                if (table_rotation != 1)
                                {
                                    Error = MechaControl.RotateSlowStop(hDevID, null);

                                    Error = MechaControl.RotateIndex(hDevID, 0, 0, null);
                                }

                                if (hSemaphore != IntPtr.Zero)
                                {
                                    CloseHandle(hSemaphore);
                                    hSemaphore = IntPtr.Zero;
                                }

                                if (hSumImgSemaphore != IntPtr.Zero)
                                {
                                    CloseHandle(hSumImgSemaphore);
                                    hSumImgSemaphore = IntPtr.Zero;
                                }

                                //セマフォの作成
                                hSemaphore = CreateSemaphore(IntPtr.Zero, 0, View, "TEST_SEMAPHORE");

                                //Rev20.01 積算1ビュー保存用に追加 by長野
                                hSumImgSemaphore = CreateSemaphore(IntPtr.Zero, 0, cone_acq_view, "TEST_SUMIMAGE_SEMAPHORE");
                          
                                //DetSftCompleteFlgをCT30kがONにするまで待つ。
                                while (DetSftCompleteFlg == false)
                                {
                                    Thread.Sleep(1000);
                                }
                                DetSftCompleteFlg = false;

                                Thread.Sleep(10000);

                                if (table_rotation == 1)
                                {
                                    //Rev20.00 変更 by長野 2014/09/11
                                    float RotSpeed = (float)(60.0 * FrameRateForScan / View / Itgnum);
                                    //Rev20.00 応急処置 by長野 2014/09/08
                                    //float RotSpeed = (float)(60.0 * 29.973 / View / Itgnum);

                                    // テーブル連続回転開始		
                                    if ((Error = MechaControl.RotateManual(hDevID, (1 - RotDirect), RotSpeed, 1)) < 1)
                                    {
                                        // 正常に回転開始した場合
                                        Thread.Sleep(700);		//Rev17.40 回転が一定速度になる前にRotateManualを抜けてしまうため追加 10-10-26 by IWASAWA
                                        Error = 0;
                                    }
                                }
                            }

                            //Rev25.00 データが残らないように空読みする by長野 2016/08/10
                            // PkeFPDの場合
                            if (Pulsar.hPke != IntPtr.Zero)
                            {
                                // キャプチャ＆画像取得
                                Pulsar.PkeCaptureOnly(Pulsar.hPke, transImage);
                            }
                            // MILキャプチャの場合
                            if (Pulsar.hPke == IntPtr.Zero)
                            {
                                // 画像キャプチャ＆コピー
                                Pulsar.MilGrabAndGet(Pulsar.hMil, transImage);
                            }
                        }

                        cob_chg_cnt = 0;

                        // ビュー数分収集ループ
                        for (int cntView = 0; cntView < cone_acq_view; cntView++)
                        {
                            if (cntView == shared_cob_chg[cob_chg_cnt])
                            {
                                cob_chg_cnt++;
                            }

                            //Rev20.00 追加 by長野 2014/08/02 
                            //セマフォが獲得できるまで、待つ
                            //Rev20.00 使用しない 1ビューずつ保存するようにしたので、キャプチャが未保存のデータを上書きする可能性はほぼ0であるため by長野 2014/10/23
                            //WaitForSingleObject(hCapSemaphore, 0xfffffff);

                            // 積算画像用メモリの０クリア
                            //if (Itgnum > 1)
                            //Rev20.00 変更 by長野 2015/02/10
                            //SUM_IMAGE = new int[size];
                            Array.Clear(SUM_IMAGE, 0, size);

                            // 積算枚数分の処理
                            for (long icnt = 0; icnt < Itgnum; icnt++)
                            {
                                // PkeFPDの場合
                                if (Pulsar.hPke != IntPtr.Zero)
                                {
                                    try
                                    {
                                        // キャプチャ＆画像取得
                                        Pulsar.PkeCaptureOnly(Pulsar.hPke, transImage);
                                        //Rev25.00 by長野 2016/08/18
                                        //if (sft_mode == 0 && (scan_mode == 4 || w_scan == 1))
                                        //{
                                        //    for (int cnt = 0; cnt < transImage.Length; cnt++)
                                        //    {
                                        //        transImage[cnt] = (ushort)((float)transImage[cnt] * MagVal);
                                        //    }
                                        //}
                                        //Rev25.00 by長野 2016/08/18
                                        //if (sft_mode == 0 && w_scan == 1)
                                        //Rev26.10 変更 by chouno 2018/01/13
                                        if (sft_mode == 0 && (w_scan == 1 || scan_mode == 4))
                                        {
                                            for (int cnt = 0; cnt < transImage.Length; cnt++)
                                            {
                                                transImage[cnt] = (ushort)((float)transImage[cnt] * MagValL);
                                            }
                                        }
                                        //else if (sft_mode == 1 && w_scan == 1)
                                        //Rev26.10 変更 by chouno 2018/01/13
                                        else if (sft_mode == 1 && (w_scan == 1 || scan_mode == 4))
                                        {
                                            for (int cnt = 0; cnt < transImage.Length; cnt++)
                                            {
                                                transImage[cnt] = (ushort)((float)transImage[cnt] * MagValR);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.Print(ex.ToString());
                                    }
                                }

                                //Rev20.00 追加 by長野 2014/10/06
                                //sw.Restart();

                                // MILキャプチャの場合
                                if (Pulsar.hPke == IntPtr.Zero)
                                {
                                    // 画像キャプチャ＆コピー
                                    Pulsar.MilGrabAndGet(Pulsar.hMil, transImage);
                                    cnt_capture++;		// 総キャプチャ数カウントアップ
                                }

                                //Rev20.00 追加 by長野 2014/10/06
                                //elapsedTime[cntView] = sw.ElapsedMilliseconds;

                                // 最後のループ時にメカを動かす
                                if (icnt == Itgnum - 1)
                                {

                                    // テーブル回転
                                    //MechaControl.RotateTable(cntView, View, hDevID, mrc, table_rotation, RotDirect);	// Rev17.00 PkeFPD対応 byやまおか 2010-02-24
                                    //Rev23.40 変更 by長野 2016/06/19
                                    MechaControl.RotateTable(cntView, View, hDevID, ref mrc, table_rotation, RotDirect);	// Rev17.00 PkeFPD対応 byやまおか 2010-02-24

                                    //Rev20.00 test by長野 2014/08/27 
                                    //Thread.Sleep(66);
                                    //テーブル回転エラー処理	//Rev17.452/17.61追加 byやまおか 2011/07/29
                                    if (mrc != 0)
                                    {
                                        Error = mrc;
                                        break;
                                    }

                                }
                                //thinning_frame_num = ct_com.scancondpar.thinning_frame_num;
                                // 指定された回数ごとにCT30Kに透視画像を表示させる
                                //キャプチャの残りﾋﾞｭｰ数がLIMIT_VIWE_NUM以下になったら透視画像の更新をしないように条件文を変更 v16.00 by 長野 10/01/15
                                if (((cntView * Itgnum + icnt) % thinning_frame_num == 0) && ((cone_acq_view - cntView) >= LIMIT_VIEW_NUM))
                                {
                                    //表示
                                    Update();
                                }

                                // 画像積算
                                if (Itgnum > 1)
                                {
                                    ImgProc.AddImageWord(transImage, SUM_IMAGE, size);

                                }
                                else
                                {
                                    //TransImageControl.CopyWordToLong(transImage, SUM_IMAGE, size);
                                    //Rev20.00 Cに置き換え by長野 2014/08/27
                                    //transImage.CopyTo(AVE_IMAGE10, 0);
                                    transImage.CopyTo(SUM_IMAGE, 0);
                                }

                            }

                            //Rev23.12 純生データ保存有の場合は、ここで保存を行う by長野 2015/12/05
                            if (purDataSaveExFlg == true)
                            {
                                //平均化
                                Pulsar.DivImage_short_Scan(SUM_IMAGE, AVE_IMAGE10, Itgnum, size);
                                //ファイル名決定
                                purSaveFileName = purDataBaseFileName + ((sft_cnt * cone_acq_view) + cntView + 1).ToString() + purExt;
                                //ファイルに落とす
                                IICorrect.ImageSave(ref AVE_IMAGE10[0], purSaveFileName, detector.h_size, detector.v_size);
                            }

                            //Rev20.01 //積算2枚以上の場合分け by長野 2015/05/20
                            //if (Itgnum > 1 && table_rotation == 0)
                            if (Itgnum > 1 && table_rotation == 0 && sft_flg == false)//Rev23.10 条件追加 by長野 2015/10/06 
                            {
                                Array.Copy(SUM_IMAGE, js * detector.h_size, sum_saveImage, 0, len);
                                SetSharedCTConeViewLongData(sum_saveImage, sum_viewsize);
                            }
                            else
                            {

                                //if (table_rotation == 0) ////Rev16.2 変更 by YAMAKAGE 10-01-19
                                //{
                                //
                                //}
                                // テーブル連続回転時 
                                //else
                                //{
                                //    // 純生データの平均化を行う
                                //    Pulsar.DivImage_short(SUM_IMAGE, AVE_IMAGE10, Itgnum, detector.h_size, detector.v_size);
                                //
                                //}

                                if (table_rotation == 0)
                                {
                                    // 純生データの平均化を行う
                                    //Pulsar.DivImage_short(SUM_IMAGE, AVE_IMAGE10, 1, detector.h_size, detector.v_size);
                                    //rev20.00 修正 by長野 2015/01/24
                                    Pulsar.DivImage_short(SUM_IMAGE, AVE_IMAGE10, Itgnum, detector.h_size, detector.v_size);
                                }
                                else
                                {
                                    //Rev20.00 関数変更 by長野 2014/10/06
                                    //Pulsar.DivImage_short(SUM_IMAGE, AVE_IMAGE10, 1, detector.h_size, detector.v_size);
                                    // 純生データの平均化を行う
                                    Pulsar.DivImage_short_Scan(SUM_IMAGE, AVE_IMAGE10, Itgnum, size);
                                }

                                //共有メモリへコピー
                                mutexName = GetStringBuilderToStr(shared_cob_mutexNames, cmpstr, cob_chg_cnt - 1);
                                objName = GetStringBuilderToStr(shared_cob_objNames, cmpstr, cob_chg_cnt - 1);

                                if ((mutexName == "") || (objName == ""))
                                {
                                    Error = -1;
                                    break;
                                }

                                //Rev23.10 シフトスキャン、かつ、２回目の収集時の場合、ここでシフト用生データを作成する by長野 2015/10/06
                                //if (sft_mode == 1 && scan_mode == 4)
                                //Rev25.00 Wスキャンを追加 by長野 2016/07/07
                                if (sft_mode == 1 && (scan_mode == 4 || w_scan == 1))
                                {
                                    try
                                    {
                                        //基準位置での同じビューのデータを取り出す
                                        //共有メモリへコピー
                                        Error = GetSharedCTConeViewDataForCT30K(
                                            tmp_sft_pur,		    //(O)生データ１ビューの配列
                                            viewsize,				//(I)１ビューのサイズ
                                            sharedMemOffset,		//(I)１ビュー取得するのに必要なオフセットサイズ
                                            cntView,				//(I)セットするビュー数番号
                                            cob_chg_cnt,			//(I)生データ分割番号
                                            cob_view,			    //(I)生データ１個当たりの基本ビュー数
                                            mutexName,	            //(I)共有メモリのミューテックス名(分割番号に相当する）
                                            objName		            //(I)共有メモリのオブジェクト名(分割番号に相当する）
                                        );
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.Print(ex.ToString());
                                    }

                                    try
                                    {
                                        sft_cone_makeForCT30K(tmp_sft_pur, AVE_IMAGE10, sft_pur1st, sft_pur2nd, js, je, sft_mainch, mainch, det_sft_pix, sft_par.wt_st1, sft_par.wt_st2, sft_par.wt_end1, sft_par.wt_end2, sft_par.wt[0], sft_par.wt[1]);
                                    }
                                    catch(Exception ex)
                                    {
                                        Debug.Print(ex.ToString());
                                    }
                                    try
                                    {
                                        //共有メモリへコピー
                                        Error = SetSharedCTConeViewDataForCT30K(
                                            sft_pur2nd,	    	    //(I)生データ１ビューの配列
                                            sft_pur_size,	    	//(I)１ビューのサイズ
                                            sharedMemOffset,		//(I)１ビュー取得するのに必要なオフセットサイズ
                                            cntView,				//(I)セットするビュー数番号
                                            cob_chg_cnt,			//(I)生データ分割番号
                                            cob_view,			    //(I)生データ１個当たりの基本ビュー数
                                            mutexName,	            //(I)共有メモリのミューテックス名(分割番号に相当する）
                                            objName		            //(I)共有メモリのオブジェクト名(分割番号に相当する）
                                        );
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.Print(ex.ToString());
                                    }
                                }
                                else
                                {
                                    // 指定範囲のみ保存用配列にコピーする
                                    //AVE_IMAGE10をjs-jeで切り取ってSetSharedCTConeViewDataForCT30Kに入れる
                                    /*
                                    WORD *p = savedat;
                                    for (long j = js; j <= je; j++)
                                    {
                                        memcpy(p, dsedat + j * nd, sizeof(WORD) * nd);
                                        p += nd;
                                    }
                                    */
                                    //Array.Copyを使う
                                    //Rev20.00 Cに変更 by長野 2014/08/27
                                    Array.Copy(AVE_IMAGE10, js * detector.h_size, saveImage, 0, len);
                                    //TransImageControl.CopyWordToWord(AVE_IMAGE10, saveImage, js * detector.h_size,len);

                                    //Buffer.BlockCopyを使う 
                                    //Buffer.BlockCopy(AVE_IMAGE10, js * typesize, saveImage, 0, saveImage.Length * typesize);

                                    if (Error == -1) break;


                                    ////共有メモリへコピー
                                    //mutexName = GetStringBuilderToStr(shared_cob_mutexNames, cmpstr, cob_chg_cnt - 1);
                                    //objName = GetStringBuilderToStr(shared_cob_objNames, cmpstr, cob_chg_cnt - 1);

                                    //if ((mutexName == "") || (objName == ""))
                                    //{
                                    //    Error = -1;
                                    //    break;
                                    //}

                                    //共有メモリへコピー
                                    Error = SetSharedCTConeViewDataForCT30K(
                                        saveImage,		        //(I)生データ１ビューの配列
                                        viewsize,				//(I)１ビューのサイズ
                                        sharedMemOffset,		//(I)１ビュー取得するのに必要なオフセットサイズ
                                        cntView,				//(I)セットするビュー数番号
                                        cob_chg_cnt,			//(I)生データ分割番号
                                        cob_view,			    //(I)生データ１個当たりの基本ビュー数
                                        mutexName,	            //(I)共有メモリのミューテックス名(分割番号に相当する）
                                        objName		            //(I)共有メモリのオブジェクト名(分割番号に相当する）
                                    );
                                }
                                //Rev20.00 test 追加 by長野 2014/09/11
                                if (Error != 0)
                                {
                                    Debug.Print(cntView.ToString());
                                    //MessageBox.Show("失敗");
                                }
                                //引数
                                //cone_raw                      →　透視データの配列
                                //viewsize                      →　配列の数
                                //sharedMemOffset               →　shared_viewsize 
                                //view_num                      →　撮影したview cntView
                                //cob_num                       →　cob_chg_cnt
                                //cob_baseview                  →　Ct_com.scaninf.scan_par.cob_view
                                //shared_cob_objNames[cob_num]  →　mutexName
                                //shared_cob_objNames[cob_num]  →　objName
                            }

                            //2014/07/31 ソフト側の取り出しを実際の透視画像取得に対して10ビュー遅れにする。書き込みの遅延で画像がおかしくならないようにするための予防 by長野 2014/07/31
                            // セマフォを１解放する
                            //Rev20.01 ファイルやり取りではないので廃止 by長野 2015/05/20 
                            //if (cntView > 2)
                            //if (cntView > testTable[shared_cob_hdd_chg_cnt])
                            //{
                            //if (((cntView >= shared_cob_hdd_chg[shared_cob_hdd_chg_cnt]) || (cntView <= shared_cob_hdd_chg[shared_cob_hdd_chg_cnt] + 2)))
                            //{
                            ReleaseSemaphore(hSemaphore, 1, ref cntsem);
                            //Rev20.00 使用しない by長野 2014/10/23
                            //if (cntView + 1 == shared_cob_hdd_chg[shared_cob_hdd_chg_cnt + 1])
                            //{
                            //    shared_cob_hdd_chg_cnt = shared_cob_hdd_chg_cnt + 1;
                            //    ReleaseSemaphore(hSemaphore, 10, ref cntsem);
                            //}
                            //}
                            //}
                            //if (Itgnum > 1 && table_rotation == 0)
                            //rev23.10 条件変更 by長野 2015/11/08
                            if (Itgnum > 1 && table_rotation == 0 && sft_flg == false)
                            {
                                //Rev20.01 テーブル回転がステップ回転で、積算2枚以上の場合はスキャンソフト側が1ビュー処理したことを確認する。
                                if (WaitForSingleObject(hSumImgSemaphore, (int)sumimg_timeout) == 0x102)
                                {
                                    Error = 1913;
                                    break;
                                }
                            }

                            // SIGKILL回に1回、終了チェック 
                            if ((cntView % SIGKILL) == 0)
                            {
                                //if (rstop_flg == 0)	//Rev17.40 10-10-20 by IWASAWA
                                //{
                                //    if ((Error=CTLib.sig_chk())!=0) break;				
                                //}
                                //else
                                //{
                                //    if ((Error=CTLib.sig_chk_rmdsk())!=0)	break;//Rev17.40 10-10-20 by IWASAWA
                                //}
                                //if ((Error = CTLib.sig_chk()) != 0) break;
                                //Rev20.00 CheckCancelに変更 by長野 2014/09/18
                                if ((CTAPI.PulsarHelper.CheckCancel()) == true)
                                {
                                    //Rev20.00 スキャンソフト側もスキャンストップファイルで止めれるようにセマフォは全部開放する
                                    ReleaseSemaphore(hSemaphore, cone_acq_view - cntView, ref cntsem);
                                    Error = 1902;
                                    return Error; //Rev26.10 change by chouno 2018/01/10
                                    //break;
                                }

                                /*
                                // 放電プロテクト処理（連続回転ならば何もしない）  added by 山本　2004-6-30	
                                if ((Error=discharge_protect_process(discharge_protect, table_rotation, cont_time, idle_time, Tstart)) != 0) 
                                {
                                    break;
                                }
                                */
                            }

                            //Thread.Sleep(0);
                        }
                        //2014/07/31 最後に全部解放する by長野 2014/07/31
                        //Rev20.01 廃止 by長野 2015/05/20
                        // ReleaseSemaphore(hSemaphore, 3, ref cntsem);
                        //Rev23.10 シフトスキャンの場合は、sft_modeを切り替える
                        //if (sft_mode == 0 && scan_mode == 4)
                        //Rev25.00 Wスキャン追加 by長野 2016/07/07
                        if (sft_mode == 0 && (scan_mode == 4 || w_scan == 1))
                        {
                            if (table_rotation == 1)
                            {
                                Error = MechaControl.RotateSlowStop(hDevID, null);
                            }

                            sft_mode = 1;
                        }
                    }
                
                }
            }
            finally
            {
                //連続回転停止（連続回転モード時のみ）
                if (table_rotation == 1)
                {
                    MechaControl.RotateSlowStop(hDevID, null);
                    //Error = RotateSlowStop(hDevID);
                    //if (Result == 0) Result = Error; //v10.0追加 by 間々田 2005-02-09
                }

                //Rev20.00 EndProcessScanへ移動 by長野 2015/02/21
                ////追加2015/01/29hata_if文追加
                ////エラー発生の場合は回転を戻さない
                //if (Error == 0) 
                //    //Rev20.00 追加 by長野 2014/08/27
                //    MechaControl.RotateIndex(hDevID, 0, 0, null);
                

                // キャプチャ停止時処理
                Pulsar.CaptureSeqStop(Pulsar.hMil, Pulsar.hPke);
                ScanError = Error;
                OnCaptureScanEnd();

            }

            return Error;

        }
       
        //Rev20.00 ここから シングルスキャン用の処理 by長野 2014/08/27
        /// <summary>
        /// キャプチャScanスレッド
        /// </summary>
        //public int CaptureSingleScanStart()
        //{
        //    // スキャンスレッド起動
        //    if (captureThread == null)
        //    {
        //        // 起動
        //        ScanError = 0;
        //        bScan = true;
        //        captureScanThread = new Thread(new ThreadStart(CaptureSingleScan));
        //        captureScanThread.IsBackground = true;
        //        captureScanThread.Start();
        //    }
        //    else
        //    {
        //        return 1;
        //    }

        //    return 0;
        //}
        public int CaptureSingleScanStart(int ScanLoop = 0)
        {
            // スキャンスレッド起動
            if (captureScanThread == null)
            {
                // 起動
                ScanLoopNum = ScanLoop;
                bWaitScan = false;
                ScanError = 0;
                bScan = true;
                captureScanThread = new Thread(new ThreadStart(CaptureSingleScan));
                captureScanThread.IsBackground = true;
                captureScanThread.Start();
            }
            else
            {
                return 1;
            }

            return 0;
        }
        public int CaptureSingleScanStart()
        {
            //通常スキャン
            int sts = CaptureSingleScanStart(0);
            return sts;
        }

        /// <summary>
        /// キャプチャScanスレッド //Rev20.00 マルチスキャン・スライスプランに対応 by長野 2014/09/11
        /// </summary>
        private void CaptureSingleScan()
        {
            //int sts = CaptureSingleScanEx(
            //    parm_Itgnum,                    //積算枚数
            //    parm_View,					    //ビュー数（360°あたり）
            //    parm_hDevID,				    //メカ制御ボードハンドル
            //    parm_mrc,			            //メカ制御ボードエラーステータス            
            //    parm_table_rotation,            //テーブル回転モード 0:ステップ 1:連続
            //    parm_thinning_frame_num,        //透視画像表示時の間引きフレーム数
            //    parm_shared_raw_mutexNames,     //シングルスキャン共有メモリミューテックス名
            //    parm_shared_raw_objNames,       //シングルスキャン共有メモリオブジェクト名
            //    parm_rawdatasize,			    //１ビューのサイズ
            //    parm_shared_rawdatasize,        //オフセット込みの生データサイズ
            //    parm_shared_view_size,          //オフセット込みの１ビューサイズ
            //    parm_line_size,                 //生データ横方向サイズ(FFT方式の場合、FFTサイズ)
            //    parm_multi_slice_num,           //マルチスライス数
            //    parm_Delta_Ysw,                 //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            //    parm_Delta_Ysw_dash,            //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            //    parm_Delta_Ysw_2dash,           //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            //    parm_SPA,                       //スキャン位置校正用の変数
            //    parm_SPB,                       //スキャン位置校正用の変数
            //    parm_vs,                        //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            //    parm_ve,                        //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            //    parm_ud_direction,              //マルチスキャン時のテーブル移動方向
            //    parm_vsize,                     //透視画像縦サイズ
            //    parm_hsize                      //透視画像横サイズ
            //);

            //bScan = false;
            //captureScanThread = null;
            int sts = 0;
            int cnt = 1;

            while (bScan)
            {
                sts = CaptureSingleScanEx(
                    parm_Itgnum,                    //積算枚数
                    parm_View,					    //ビュー数（360°あたり）
                    parm_Acq_view,                  //実際に取り込むビュー数
                    parm_hDevID,				    //メカ制御ボードハンドル
                    parm_mrc,			            //メカ制御ボードエラーステータス            
                    parm_table_rotation,            //テーブル回転モード 0:ステップ 1:連続
                    parm_thinning_frame_num,        //透視画像表示時の間引きフレーム数
                    parm_shared_raw_mutexNames,     //シングルスキャン共有メモリミューテックス名
                    parm_shared_raw_objNames,       //シングルスキャン共有メモリオブジェクト名
                    parm_rawdatasize,			    //１ビューのサイズ
                    parm_shared_rawdatasize,        //オフセット込みの生データサイズ
                    parm_shared_view_size,          //オフセット込みの１ビューサイズ
                    parm_line_size,                 //生データ横方向サイズ(FFT方式の場合、FFTサイズ)
                    parm_multi_slice_num,           //マルチスライス数
                    parm_Delta_Ysw,                 //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
                    parm_Delta_Ysw_dash,            //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
                    parm_Delta_Ysw_2dash,           //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
                    parm_SPA,                       //スキャン位置校正用の変数
                    parm_SPB,                       //スキャン位置校正用の変数
                    parm_vs,                        //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
                    parm_ve,                        //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
                    parm_ud_direction,              //マルチスキャン時のテーブル移動方向
                    parm_vsize,                     //透視画像縦サイズ
                    parm_hsize,                     //透視画像横サイズ
                    parm_HalfNoAutoCenteringFlg,    //Rev20.00 オートセンタリング用フラグ by長野 2015/01/24
                    parm_saveFluoroImage,           //Rev20.00 追加 by長野 2015/02/09
                    parm_scan_mode,                 //Rev23.10 スキャンモード追加 by長野 2015/10/06
                    parm_w_scan,                    //Rev25.00 Wスキャン追加 by長野 2016/07/07
                    parm_sft_mainch,                //Rev23.10 シフトスキャン対応 by長野 2015/10/06
                    parm_mainch,                    //Rev23.10 シフトスキャン対応 by長野 2015/10/06 
                    parm_vs_sft,                    //Rev23.10 シフトスキャン対応 by長野 2015/10/06 
                    parm_ve_sft,                    //Rev23.10 シフトスキャン対応 by長野 2015/10/06 
                    parm_SPB_sft,                    //Rev23.10 シフトスキャン対応 by長野 2015/10/06 
                    parm_savePurDataFlg,            //Rev23.12 追加 by長野 2015/12/11
                    parm_purDataBaseFileName,       //Rev23.12 追加 by長野 2015/12/11
                    parm_ShiftFImageMagVal,          //Rev25.00 追加 by長野 2016/09/24
                    parm_ShiftFImageMagValL,
                    parm_ShiftFImageMagValR
                    );

                if (sts != 0) break;
                if (ScanLoopNum > cnt)
                {
                    bWaitScan = true;
                    while (bWaitScan && bScan)
                    {
                        //撮影待ち
                        Thread.Sleep(1);
                    }
                    MechaControl.RotateIndex(parm_hDevID, 0, 0, null);
                    cnt = cnt + 1;
                }
                else
                {
                    break;
                }
            }
            bScan = false;
            bWaitScan = false;
            ScanLoopNum = 0;
            captureScanThread = null;
        }


        /// <summary>
        /// キャプチャScan処理
        /// </summary>
        public int CaptureSingleScanEx(
            int Itgnum,                             //積算枚数
            int View,					            //ビュー数（360°あたり）
            int Acq_view,                           //実際に取り込むビュー数
            int hDevID,				                //メカ制御ボードハンドル
            int mrc,			                    //メカ制御ボードエラーステータス            
            int table_rotation,                     //テーブル回転モード 0:ステップ 1:連続
            int thinning_frame_num,                 //透視画像表示時の間引きフレーム数
            StringBuilder shared_raw_mutexNames,    //シングルスキャン共有メモリミューテックス名
            StringBuilder shared_raw_objNames,      //シングルスキャン共有メモリオブジェクト名
            int rawdatasize,                        //１ビューのサイズ
            int shared_rawdatasize,                 //オフセット込みの生データサイズ
            int shared_view_size,                   //オフセット込みの１ビューサイズ
            int line_size,                          //生データ横方向サイズ(FFT方式の場合、FFTサイズ)
            int multi_slice_num,                    //マルチスライス数
            float[] Delta_Ysw,                      //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            float[] Delta_Ysw_dash,                 //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            float[] Delta_Ysw_2dash,                //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            float[] SPA,                            //スキャン位置校正用の変数
            float[] SPB,                            //スキャン位置校正用の変数
            int vs,                                 //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            int ve,                                 //ﾗｲﾝﾃﾞｰﾀ化に必要な変数
            int ud_direction,                       //マルチスキャン時のテーブル移動方向
            int v_size,                             //透視画像縦サイズ
            int h_size,                             //透視画像横サイズ
            int HalfNoAutoCenteringFlg,             //Rev20.00 追加 by長野 2015/01/24
            int saveFluoroImage,                    //Rev20.00 追加 by長野 2015/02/09 
            int scan_mode,                          //Rev23.10 スキャンモード 追加 by長野 2015/10/06
            int w_scan,                             //Rev25.00 Wスキャン 追加 by長野 2016/07/07
            int sft_mainch,                         //Rev23.10 シフトスキャン対応 by長野 2015/10/06
            int mainch,                             //Rev23.10 シフトスキャン対応 by長野 2015/10/06
            int vs_sft,                             //Rev23.10 シフトスキャン対応 by長野 2015/10/06
            int ve_sft,                             //Rev23.10 シフトスキャン対応 by長野 2015/10/06
            float[] SPB_sft,                        //Rev23.10 シフトスキャン対応 by長野 2015/10/06
            int savePurDataFlg,                     //Rev23.12 純生データ保存フラグ 追加 by長野 2015/12/11
            string purDataBaseFileName,             //Rev23.12 純生データ ベースファイル名 by長野 2015/12/11
            float ShiftFImageMagVal,                 //Rev25.00 左右のシフト画像の輝度値調整用係数 by長野 2016/09/24
            float ShiftFImageMagValL,
            float ShiftFImageMagValR
            )
        {
            //    //**********************************************
            //    //何の情報が必要か
            //    //long  table_rotation,			//テーブル回転モード 0:ステップ 1:連続			Rev2.00追加
            //    //×---long detector,			//検出器種類 0:I.I. 1:浜FPD 2:PkeFPD			Rev2.00追加
            //    //long  Itgnum,					//積算枚数
            //    //	long  View,					//ビュー数（360°あたり）
            //    //DWORD hDevID,					//メカ制御ボードハンドル
            //    //long  Acq_View,					//データ取り込みを行うビュー数
            //    //**********************************************

            // 戻り値用変数
            int Error = 0;

            const int SIGKILL = 10;        //指定回数毎に、終了チェックを行う

            //テーブル回転方向
            int RotDirect = (detector.DetType == DetectorConstants.DetTypeHama || detector.DetType == DetectorConstants.DetTypePke) ? 1 : 0;
            int cnt_capture = 0;		//キャプチャした回数

            //使っていない
            //int cnt_adjustTable = 0;	//キャプチャとテーブル回転間のズレ補正用テーブルのカウンタ

            string objName = "";
            string mutexName = "";
            char[] cmpstr = new char[] { ';' };

            float FrameRateForScan = Detector.FrameRateForScan; //Rev20.00 追加 by長野 2014/09/11

            //Rev23.12 追加 by長野 2015/12/11
            string purExt = ".pur";
            string purSaveFileName = "";
            bool purDataSaveExFlg = false;
            //scanselのフラグがONでも、ファイル名が空白の場合はフラグOFF
            if (savePurDataFlg == 1 && purDataBaseFileName != "")
            {
                purDataSaveExFlg = true;
            }

            //Rev23.10 シフト用追加 by長野 2015/10/06
            //シフトスキャンの場合は、２回目のデータ収集を行うようにする。
            int sft_mode = 0;
            int sft_cnt_max = 0;
            int cal_vs = 0;
            int cal_ve = 0;
            float[] cal_SPB = new float[5];
            //if (scan_mode == 4)
            //Rev25.00 Wスキャンを追加 by長野 2016/07/07
            if (scan_mode == 4 || w_scan == 1)
            {
                sft_cnt_max = 2;
            }
            else
            {
                sft_cnt_max = 1;
            }
            
            //Rev25.00 追加 by長野 2016/08/18 --->
            float MagVal = 0.0f;
            MagVal = ShiftFImageMagVal;
            //<---
            float MagValL = 0.0f;
            MagValL = ShiftFImageMagValL;
            float MagValR = 0.0f;
            MagValR = ShiftFImageMagValR;
            //<---

            //Rev20.00 追加 by長野 2104/10/06
            long[] elapsedTime = new long[6000];
            Stopwatch sw = new Stopwatch();

            try
            {
                // キャプチャ準備
                Pulsar.CaptureSetup(Pulsar.hMil, Pulsar.hPke);

                if ((Error = Pulsar.CaptureSetup(Pulsar.hMil, Pulsar.hPke)) != 0)
                {
                    return Error;
                }

                // 透視画像のサイズ
                int size = detector.h_size * detector.v_size;

                // 積算用配列
                int[] SUM_IMAGE = new int[size];
                transImage = new ushort[size];

                // 加算平均画像用配列：テーブル連続回転時のみ使用
                ushort[] AVE_IMAGE10 = new ushort[size];

                //ラインデータ１ビュー分
                double[] linedat = new double[line_size];

                //１ビューのサイズ
                int view_size = sizeof(double) * line_size;

                //ラインデータ
                //double[] PIXLINE0 = new double[line_size * View];
                //double[] PIXLINE1 = new double[line_size * View];
                //double[] PIXLINE2 = new double[line_size * View];
                //double[] PIXLINE3 = new double[line_size * View];
                //double[] PIXLINE4 = new double[line_size * View];

                //Rev20.00 追加 by長野 2014/12/04
                int len = (ve - vs + 1) * detector.h_size;
                int[] tempSUM_IMAGE = new int[len];
                ushort[] tempAVE_IMAGE10 = new ushort[len];

                double[] PIXLINE0;
                double[] PIXLINE1;
                double[] PIXLINE2;
                double[] PIXLINE3;
                double[] PIXLINE4;
                PIXLINE0 = new double[line_size * View];
                PIXLINE1 = new double[line_size * View];
                PIXLINE2 = new double[line_size * View];
                PIXLINE3 = new double[line_size * View];
                PIXLINE4 = new double[line_size * View];
                //PIXLINEの領域を確保
                switch (multi_slice_num)
                {
                    case 0:
                        PIXLINE0 = new double[line_size * View];
                        PIXLINE1 = new double[1];
                        PIXLINE2 = new double[1];
                        PIXLINE3 = new double[1];
                        PIXLINE4 = new double[1];

                        break;
                    case 1:
                        PIXLINE0 = new double[line_size * View];
                        PIXLINE1 = new double[line_size * View];
                        PIXLINE2 = new double[line_size * View];
                        PIXLINE3 = new double[1];
                        PIXLINE4 = new double[1];

                        break;
                    case 2:
                        PIXLINE0 = new double[line_size * View];
                        PIXLINE1 = new double[line_size * View];
                        PIXLINE2 = new double[line_size * View];
                        PIXLINE3 = new double[line_size * View];
                        PIXLINE4 = new double[line_size * View];
                        break;
                }

                int cntsem = 0;
                //セマフォの作成
                if (hSemaphore != IntPtr.Zero)
                {
                    CloseHandle(hSemaphore);
                }
                hSemaphore = CreateSemaphore(IntPtr.Zero, 0, View, "TEST_SEMAPHORE");

                //// キャプチャ開始	
                Pulsar.CaptureSeqStart(Pulsar.hMil, Pulsar.hPke);

                //Rev20.00 追加 by長野 2015/02/09
                if (saveFluoroImage == 1)
                {
                    //Rev20.00 透視画像保存追加 by長野 2015/01/15
                    // 積算枚数分の処理
                    for (long icnt = 0; icnt < Itgnum; icnt++)
                    {
                        // PkeFPDの場合
                        if (Pulsar.hPke != IntPtr.Zero)
                        {
                            // キャプチャ＆画像取得
                            Pulsar.PkeCaptureOnly(Pulsar.hPke, transImage);
                        }


                        // MILキャプチャの場合
                        if (Pulsar.hPke == IntPtr.Zero)
                        {
                            // 画像キャプチャ＆コピー
                            Pulsar.MilGrabAndGet(Pulsar.hMil, transImage);
                            cnt_capture++;		// 総キャプチャ数カウントアップ
                        }
                        // 画像積算
                        if (Itgnum > 1)
                        {
                            ImgProc.AddImageWord(transImage, SUM_IMAGE, size);

                        }
                        else
                        {
                            //CopyWordToLong(TRANS_IMAGE, SUM_IMAGE, size);
                            transImage.CopyTo(AVE_IMAGE10, 0);
                        }

                    }

                    if (Itgnum > 1)
                    {
                        Pulsar.DivImage_short(SUM_IMAGE, AVE_IMAGE10, Itgnum, detector.h_size, detector.v_size);
                        //Array.Copy(SUM_IMAGE, vs * detector.h_size, tempSUM_IMAGE, 0, len);
                    }
                    else
                    {
                        //Array.Copy(AVE_IMAGE10, 0, tempAVE_IMAGE10, 0, len);
                    }

                    ImgProc.ConvertMirror(ref AVE_IMAGE10[0], detector.h_size, detector.v_size);
                    //Rev20.00 拡張子が抜けていたので追加 by長野 2015/01/28
                    //IICorrect.ImageSave(ref AVE_IMAGE10[0], "C:\\CT\\TEMP\\SaveFluoroTemp.", detector.h_size, detector.v_size);
                    IICorrect.ImageSave(ref AVE_IMAGE10[0], "C:\\CT\\TEMP\\SaveFluoroTemp.img", detector.h_size, detector.v_size);
                }

                //回転ﾓｰﾄﾞが連続回転であれば連続回転開始		//Rev2.00追加
                if (table_rotation == 1)
                {
                    // 回転速度
                    //Rev20.00 変更 by長野 2014/09/11
                    float RotSpeed = (float)(60.0 * FrameRateForScan / View / Itgnum);
                    //Rev20.00 応急処置 by長野 2014/09/08
                    //float RotSpeed = (float)(60.0 * 29.973 / View / Itgnum);

                    // テーブル連続回転開始		
                    if ((Error = MechaControl.RotateManual(hDevID, (1 - RotDirect), (float)RotSpeed, 1)) < 1)
                    {
                        // 正常に回転開始した場合
                        Thread.Sleep(700);		//Rev17.40 回転が一定速度になる前にRotateManualを抜けてしまうため追加 10-10-26 by IWASAWA
                        Error = 0;
                    }
                }

                // エラーが発生している？
                if (Error != 0)
                {
                    // 何もしない
                }
                else
                {

                    //rev20.00 移動 by長野 2015/01/09 
                    //// キャプチャ開始	
                    //Pulsar.CaptureSeqStart(Pulsar.hMil, Pulsar.hPke);

                    // 階調変換による画像自動更新をオフにする
                    AutoUpdate = false;


                    //シフト回数分ループ
                    //for (int cntView = 0; cntView < View; cntView++)
                    //Rev20.00 修正 by長野 2015/01/15
                    for (int sft_cnt = 0; sft_cnt < sft_cnt_max; sft_cnt++)
                    {
                        //if (scan_mode == 4)
                        //Rev25.00 追加 by長野 2016/07/07
                        if (scan_mode == 4 || w_scan == 1)
                        {
                            if (sft_mode == 1)
                            {
                                //シフト位置への移動命令
                                //string message = "DetShift";
                                //Rev23.20 メッセージ変更 by長野 2015/11/19
                                string message = "DetShiftScan";
                                SendMessToCT30K(message);


                                Error = MechaControl.RotateSlowStop(hDevID, null);

                                Error = MechaControl.RotateIndex(hDevID, 0, 0, null);

                                //データ収集２回目の準備
                                //Rev20.00 追加 by長野 2014/12/04
                                cal_vs = vs_sft;
                                cal_ve = ve_sft;
                                cal_SPB = SPB_sft;
                                len = (cal_ve - cal_vs + 1) * detector.h_size;
                                tempSUM_IMAGE = new int[len];
                                tempAVE_IMAGE10 = new ushort[len];


                                if (hSemaphore != IntPtr.Zero)
                                {
                                    CloseHandle(hSemaphore);
                                    hSemaphore = IntPtr.Zero;
                                }

                                //セマフォの作成
                                hSemaphore = CreateSemaphore(IntPtr.Zero, 0, View, "TEST_SEMAPHORE");

                                //DetSftCompleteFlgをCT30kがONにするまで待つ。
                                while (DetSftCompleteFlg == false)
                                {
                                    Thread.Sleep(1000);
                                }
                                DetSftCompleteFlg = false;

                                Thread.Sleep(10000);
                            }
                            else if (sft_mode == 0)
                            {
                                //シフト位置への移動命令
                                //string message = "DetShiftOrg";
                                //Rev23.20 メッセージ変更 by長野 2015/11/19 
                                string message = "DetShiftOrgScan";
                                SendMessToCT30K(message);

                                Error = MechaControl.RotateSlowStop(hDevID, null);

                                Error = MechaControl.RotateIndex(hDevID, 0, 0, null);

                                //データ収集２回目の準備
                                //Rev20.00 追加 by長野 2014/12/04
                                cal_vs = vs;
                                cal_ve = ve;
                                cal_SPB = SPB;
                                len = (cal_ve - cal_vs + 1) * detector.h_size;
                                tempSUM_IMAGE = new int[len];
                                tempAVE_IMAGE10 = new ushort[len];


                                if (hSemaphore != IntPtr.Zero)
                                {
                                    CloseHandle(hSemaphore);
                                    hSemaphore = IntPtr.Zero;
                                }

                                //セマフォの作成
                                //hSemaphore = CreateSemaphore(IntPtr.Zero, 0, View, "TEST_SEMAPHORE");
                                //Rev25.00 ハーフスキャンも考慮し、1回目のスキャンのセマフォはacq_viewにする
                                hSemaphore = CreateSemaphore(IntPtr.Zero, 0, Acq_view, "TEST_SEMAPHORE");

                                //DetSftCompleteFlgをCT30kがONにするまで待つ。
                                while (DetSftCompleteFlg == false)
                                {
                                    Thread.Sleep(1000);
                                }
                                DetSftCompleteFlg = false;

                                Thread.Sleep(10000);
                            }

                            //Rev25.00 データが残らないように空読みする by長野 2016/08/10
                            // PkeFPDの場合
                            if (Pulsar.hPke != IntPtr.Zero)
                            {
                                // キャプチャ＆画像取得
                                Pulsar.PkeCaptureOnly(Pulsar.hPke, transImage);
                            }
                            // MILキャプチャの場合
                            if (Pulsar.hPke == IntPtr.Zero)
                            {
                                // 画像キャプチャ＆コピー
                                Pulsar.MilGrabAndGet(Pulsar.hMil, transImage);
                            }
                        }
                        else
                        {
                            cal_vs = vs;
                            cal_ve = ve;
                            cal_SPB = SPB;
                        }
                        //ビュー数分収集ループ
                        for (int cntView = 0; cntView < Acq_view; cntView++)
                        {

                            // 積算画像用メモリの０クリア
                            //if (Itgnum > 1)
                            //Rev20.00 変更 by長野 2015/02/10
                            //SUM_IMAGE = new int[size];
                            Array.Clear(SUM_IMAGE, 0, size);

                            // 積算枚数分の処理
                            for (long icnt = 0; icnt < Itgnum; icnt++)
                            {
                                // PkeFPDの場合
                                if (Pulsar.hPke != IntPtr.Zero)
                                {
                                    // キャプチャ＆画像取得
                                    Pulsar.PkeCaptureOnly(Pulsar.hPke, transImage);
                                    //Rev25.00 by長野 2016/08/18
                                    //if (sft_mode == 0 && w_scan == 1)
                                    if (sft_mode == 0 && (w_scan == 1 || scan_mode == 4))
                                    {
                                        for (int cnt = 0; cnt < transImage.Length; cnt++)
                                        {
                                            transImage[cnt] = (ushort)((float)transImage[cnt] * MagValL);
                                        }
                                    }
                                    //Rev26.10 変更 by chouno 2018/01/13
                                    else if (sft_mode == 1 && (w_scan == 1 || scan_mode == 4))
                                    {
                                        for (int cnt = 0; cnt < transImage.Length; cnt++)
                                        {
                                            transImage[cnt] = (ushort)((float)transImage[cnt] * MagValR);
                                        }
                                    }
                                    //if (sft_mode == 0 && w_scan == 1)
                                    //{
                                    //    for (int cnt = 0; cnt < transImage.Length; cnt++)
                                    //    {
                                    //        transImage[cnt] = (ushort)((float)transImage[cnt] * MagVal);
                                    //    }
                                    //}
                                }


                                // MILキャプチャの場合
                                if (Pulsar.hPke == IntPtr.Zero)
                                {
                                    // 画像キャプチャ＆コピー
                                    Pulsar.MilGrabAndGet(Pulsar.hMil, transImage);
                                    cnt_capture++;		// 総キャプチャ数カウントアップ
                                }

                                //Rev20.00 追加 by長野 2014/10/06
                                //sw.Restart();

                                // 最後のループ時にメカを動かす
                                //Rev20.00 変更 by長野 2014/10/06
                                if (table_rotation == 0 && icnt == Itgnum - 1)
                                {

                                    // テーブル回転
                                    //MechaControl.RotateTable(cntView, View, hDevID, mrc, table_rotation, RotDirect);	// Rev17.00 PkeFPD対応 byやまおか 2010-02-24
                                    //Rev23.40 変更 by長野 2016/06/19
                                    MechaControl.RotateTable(cntView, View, hDevID, ref mrc, table_rotation, RotDirect);	// Rev17.00 PkeFPD対応 byやまおか 2010-02-24
                                    //テーブル回転エラー処理	//Rev17.452/17.61追加 byやまおか 2011/07/29
                                    if (mrc != 0)
                                    {
                                        Error = mrc;
                                        break;
                                    }

                                }
                                //thinning_frame_num = ct_com.scancondpar.thinning_frame_num;
                                // 指定された回数ごとにCT30Kに透視画像を表示させる
                                //キャプチャの残りﾋﾞｭｰ数がLIMIT_VIWE_NUM以下になったら透視画像の更新をしないように条件文を変更 v16.00 by 長野 10/01/15
                                if (((cntView * Itgnum + icnt) % thinning_frame_num == 0) && ((View - cntView) >= LIMIT_VIEW_NUM))
                                {
                                    //表示
                                    Update();

                                }

                                // 画像積算
                                if (Itgnum > 1)
                                {
                                    ImgProc.AddImageWord(transImage, SUM_IMAGE, size);

                                }
                                else
                                {
                                    //CopyWordToLong(TRANS_IMAGE, SUM_IMAGE, size);
                                    transImage.CopyTo(AVE_IMAGE10, 0);
                                }

                            }

                            //Rev23.12 純生データ保存有の場合は、ここで保存を行う by長野 2015/12/05
                            if (purDataSaveExFlg == true)
                            {
                                //ファイル名決定
                                purSaveFileName = purDataBaseFileName + ((sft_cnt * Acq_view) + cntView + 1).ToString() + purExt;

                                // 画像積算
                                if (Itgnum > 1)
                                {
                                    //平均化
                                    Pulsar.DivImage_short_Scan(SUM_IMAGE, AVE_IMAGE10, Itgnum, size);
                                }
                                //ファイルに落とす
                                IICorrect.ImageSave(ref AVE_IMAGE10[0], purSaveFileName, detector.h_size, detector.v_size);
                            }

                            if (Itgnum > 1)
                            {
                                //Array.Copy(SUM_IMAGE, vs * detector.h_size, tempSUM_IMAGE, 0, len);
                                //Rev23.10 変更 by長野 2015/11/14
                                Array.Copy(SUM_IMAGE, cal_vs * detector.h_size, tempSUM_IMAGE, 0, len);
                            }
                            else
                            {
                                //Rev23.10 変更 by長野 2015/11/14
                                //Array.Copy(AVE_IMAGE10, vs * detector.h_size, tempAVE_IMAGE10, 0, len);
                                Array.Copy(AVE_IMAGE10, cal_vs * detector.h_size, tempAVE_IMAGE10, 0, len);
                            }

                            #region 不要
                            ////if (table_rotation == 0) ////Rev16.2 変更 by YAMAKAGE 10-01-19
                            ////{

                            ////}
                            ////// テーブル連続回転時 
                            ////else
                            ////{
                            ////    // 純生データの平均化を行う
                            ////    Pulsar.DivImage_short(SUM_IMAGE, AVE_IMAGE10, Itgnum, detector.h_size, detector.v_size);

                            ////}
                            ////if (Itgnum > 1)
                            ////{
                            ////    // 純生データの平均化を行う
                            ////    Pulsar.DivImage_short(SUM_IMAGE, AVE_IMAGE10, Itgnum, detector.h_size, detector.v_size);
                            ////}
                            //if (table_rotation == 0) ////Rev16.2 変更 by YAMAKAGE 10-01-19
                            //{

                            //}
                            //// テーブル連続回転時 
                            //else
                            //{
                            //    // 純生データの平均化を行う
                            //    Pulsar.DivImage_short(SUM_IMAGE, AVE_IMAGE10, Itgnum, detector.h_size, detector.v_size);

                            //}
                            //if (Itgnum > 1)
                            //{
                            //    // 純生データの平均化を行う
                            //    Pulsar.DivImage_short(SUM_IMAGE, AVE_IMAGE10, Itgnum, detector.h_size, detector.v_size);
                            //}
                            #endregion 不要

                            // 画像入力1ビュー終了時の処理			
                            for (long j = 0; j < 2 * multi_slice_num + 1; j++)
                            {
                                // 透視画像データー>ラインデータ化
                                //if (SUM_IMAGE != null)
                                if (Itgnum > 1)
                                {
                                    //if (Mode == 1 && SUM_IMAGE != NULL)		//Rev17.50変更 積算配列を参照するのはスキャン時のみ by 間々田 2011/01/20 回転中心結果画像不具合対応
                                    //MakeLineData_long(SUM_IMAGE,ref linedat,detector.h_size,detector.v_size,SPA,SPB,vs,Delta_Ysw,Delta_Ysw_dash,Delta_Ysw_2dash,j,multi_slice_num);
                                    //MakeLineData_long(tempSUM_IMAGE, linedat, h_size, v_size, SPA, SPB, vs, Delta_Ysw, Delta_Ysw_dash, Delta_Ysw_2dash, (int)j, multi_slice_num);
                                    //Rev20.01 修正 by長野 2015/06/01 透視画像ではなく検出器の縦横サイズを使用する
                                    //MakeLineData_long(tempSUM_IMAGE, linedat, detector.h_size, detector.v_size, SPA, SPB, vs, Delta_Ysw, Delta_Ysw_dash, Delta_Ysw_2dash, (int)j, multi_slice_num);
                                    //Rev23.10 変更 by長野 2015/11/14
                                    MakeLineData_long(tempSUM_IMAGE, linedat, detector.h_size, detector.v_size, SPA, cal_SPB, cal_vs, Delta_Ysw, Delta_Ysw_dash, Delta_Ysw_2dash, (int)j, multi_slice_num);
                                    //MakeLineData_long(SUM_IMAGE, linedat, h_size, v_size, SPA, SPB, vs, Delta_Ysw, Delta_Ysw_dash, Delta_Ysw_2dash, (int)j, multi_slice_num);
                                }
                                else
                                {
                                    //後で戻す /////////////////////////////
                                    //MakeLineData_short(AVE_IMAGE10, linedat, h_size, v_size, SPA, SPB, vs, Delta_Ysw, Delta_Ysw_dash, Delta_Ysw_2dash, (int)j, multi_slice_num);
                                    //MakeLineData_short(tempAVE_IMAGE10, linedat, h_size, v_size, SPA, SPB, vs, Delta_Ysw, Delta_Ysw_dash, Delta_Ysw_2dash, (int)j, multi_slice_num);
                                    //Rev20.01 修正 by長野 2015/06/01 透視画像ではなく検出器の縦横サイズを使用する
                                    //MakeLineData_short(tempAVE_IMAGE10, linedat, detector.h_size, detector.v_size, SPA, SPB, vs, Delta_Ysw, Delta_Ysw_dash, Delta_Ysw_2dash, (int)j, multi_slice_num);
                                    //Rev23.10 変更 by長野 2015/11/14
                                    MakeLineData_short(tempAVE_IMAGE10, linedat, detector.h_size, detector.v_size, SPA, cal_SPB, cal_vs, Delta_Ysw, Delta_Ysw_dash, Delta_Ysw_2dash, (int)j, multi_slice_num);
                                    #region 不要
                                    //後                               double fI_Temp, Sum_Temp ;       //テンポラリ用
                                    //long   ii, jj, cc;
                                    //long   j_st, j_ed;			    //スライス厚開始・終了画素
                                    //float  Ysc;                      //スキャン位置上の座標
                                    //long   spav;						//スキャン位置用加算値
                                    //double fI_Temp, Sum_Temp ;       //テンポラリ用

                                    ////スキャン位置用ポインタ加算値の計算
                                    //spav = j + 2 - multi_slice_num;

                                    ////◆ スライス補完を行う ◆
                                    //for (ii = 0;  ii <  detector.h_size ; ii++)
                                    //{
                                    //    Sum_Temp = 0 ;
                                    //    Ysc = SPA[spav] * (ii - (detector.h_size / 2)) + SPB[spav] + (detector.v_size / 2) - vs;  //スライス位置センター

                                    //    {
                                    //        j_st = (long)(Ysc - Delta_Ysw_dash[spav]) ;     //加算開始位置
                                    //        j_ed = (long)(Ysc + Delta_Ysw_2dash[spav]);     //加算終了位置
                                    //    }

                                    //    if (j_st < 0)
                                    //    {
                                    //        j_st = 0 ;
                                    //    }
                                    //    if (j_ed > (detector.v_size - 1))
                                    //    {
                                    //        j_ed = detector.v_size - 1 ;
                                    //    }

                                    //    for( jj = j_st;  jj <= j_ed; jj++)
                                    //    {
                                    //        cc = jj * detector.h_size + ii ;
                                    //        if (cc == 718296)
                                    //        {
                                    //            Debug.Print("???");
                                    //        }
                                    //        //後で戻す
                                    //        //fI_Temp = (double)fI_Slice_Linear(jj - Ysc, Delta_Ysw[spav], Delta_Ysw_dash[spav], Delta_Ysw_2dash[spav]) ;
                                    //        //float ret;
                                    //        if ((jj >= (-1) * Delta_Ysw_dash[spav]) && (jj <= Delta_Ysw_dash[spav]))
                                    //        {
                                    //            fI_Temp = 1 / Delta_Ysw[spav];
                                    //        }
                                    //        else if ((jj > (-1) * Delta_Ysw_2dash[spav]) && (jj < (-1) * Delta_Ysw_dash[spav]))
                                    //        {
                                    //            fI_Temp = (jj + Delta_Ysw_2dash[spav]) / Delta_Ysw[spav];
                                    //        }
                                    //        else if ((jj > Delta_Ysw_dash[spav]) && (jj < Delta_Ysw_2dash[spav]))
                                    //        {
                                    //            fI_Temp = (Delta_Ysw_2dash[spav] - jj) / Delta_Ysw[spav];
                                    //        }
                                    //        else
                                    //        {
                                    //            fI_Temp = 0;
                                    //        }
                                    //        Sum_Temp = Sum_Temp + fI_Temp *  (double)(AVE_IMAGE10[cc]) ;
                                    //    }
                                    //    linedat[ii] = Sum_Temp   ;  //ラインデータ
                                    //}
                                    #endregion
                                }
                                //後で戻す /////////////////////////////

                                // ラインデータのコピー
                                LineDataCopy(linedat, PIXLINE0, PIXLINE1, PIXLINE2, PIXLINE3, PIXLINE4, cntView, detector.h_size, line_size, multi_slice_num, ud_direction, (int)j);
                            }

                            if (Error == -1) break;

                            //共有メモリへコピー
                            //mutexName = GetStringBuilderToStr(shared_raw_mutexNames, cmpstr, 0);
                            //objName = GetStringBuilderToStr(shared_raw_objNames, cmpstr, 0);

                            mutexName = "SHARED_SINGLEDATA_MUTEX";
                            objName = "SHARED_SINGLEDATA";

                            if ((mutexName == "") || (objName == ""))
                            {
                                Error = -1;
                                break;
                            }

                            //共有メモリへコピー
                            Error = SetSharedCTSingleDataForCT30k(
                                PIXLINE0,    		    //(I)生データ１ビューの配列
                                PIXLINE1,    		    //(I)生データ１ビューの配列
                                PIXLINE2,    		    //(I)生データ１ビューの配列
                                PIXLINE3,    		    //(I)生データ１ビューの配列
                                PIXLINE4,    		    //(I)生データ１ビューの配列
                                rawdatasize,		    //(I)１ビューのサイズ
                                cntView,				//(I)セットするビュー数番号
                                mutexName,	            //(I)共有メモリのミューテックス名(分割番号に相当する）
                                objName,		        //(I)共有メモリのオブジェクト名(分割番号に相当する）
                                multi_slice_num,
                                view_size,
                                shared_rawdatasize
                            );
                            if (Error != 0)
                            {
                                Debug.Print("Error");
                            }
                            //引数
                            //cone_raw                      →　透視データの配列
                            //viewsize                      →　配列の数
                            //sharedMemOffset               →　shared_viewsize 
                            //view_num                      →　撮影したview cntView
                            //cob_num                       →　cob_chg_cnt
                            //cob_baseview                  →　Ct_com.scaninf.scan_par.cob_view
                            //shared_cob_objNames[cob_num]  →　mutexName
                            //shared_cob_objNames[cob_num]  →　objName


                            // セマフォを１解放する
                            //ReleaseSemaphore(hSemaphore, 1, ref cntsem);
                            //2014/07/31 スキャンソフト側を2ビュー遅れにするために、2ビュー目までは解放しない by長野 2014/07/31
                            // セマフォを１解放する
                            if (cntView > 2)
                            {
                                ReleaseSemaphore(hSemaphore, 1, ref cntsem);
                            }


                            // SIGKILL回に1回、終了チェック 
                            if ((cntView % SIGKILL) == 0)
                            {
                                //if (rstop_flg == 0)	//Rev17.40 10-10-20 by IWASAWA
                                //{
                                //    if ((Error=CTLib.sig_chk())!=0) break;				
                                //}
                                //else
                                //{
                                //    if ((Error=CTLib.sig_chk_rmdsk())!=0)	break;//Rev17.40 10-10-20 by IWASAWA
                                //}
                                //if ((Error = CTLib.sig_chk()) != 0) break;
                                //Rev20.00 CheckCancelに変更 by長野 2014/09/18
                                if ((CTAPI.PulsarHelper.CheckCancel()) == true)
                                {
                                    //Rev20.00 スキャンソフト側もスキャンストップファイルで止めれるようにセマフォは全部開放する
                                    ReleaseSemaphore(hSemaphore, View - cntView, ref cntsem);
                                    Error = 1902;
                                    return Error; //Rev26.10 change by chouno 2018/01/10
                                    //break;
                                }

                                /*
                                // 放電プロテクト処理（連続回転ならば何もしない）  added by 山本　2004-6-30	
                                if ((Error=discharge_protect_process(discharge_protect, table_rotation, cont_time, idle_time, Tstart)) != 0) 
                                {
                                    break;
                                }
                                */
                            }

                            //Thread.Sleep(0);
                            //Rev20.00 追加 by長野 2014/10/06
                            //elapsedTime[cntView] = sw.ElapsedMilliseconds;
                        }

                        ReleaseSemaphore(hSemaphore, 3, ref cntsem);
                        //Rev20.00 追加 by長野 2014/10/14
                        //Rev20.00 条件追加（シングルハーフはセマフォの個数がオートセンタリング有無で変わるため) by長野 2015/01/24
                        //if (((View - Acq_view > 0)) && HalfNoAutoCenteringFlg == 1)
                        //Rev25.00 シフトの場合は2回目の収集後に行う by長野 2016/08/16
                        //if (((View - Acq_view > 0)) && HalfNoAutoCenteringFlg == 1 && w_scan == 1 && sft_mode == 1)
                        if (((View - Acq_view > 0)) && HalfNoAutoCenteringFlg == 1)
                        {
                            if (w_scan == 1)
                            {
                                if (sft_mode == 1)
                                {
                                    ReleaseSemaphore(hSemaphore, View - Acq_view, ref cntsem);
                                }
                            }
                            else
                            {
                                ReleaseSemaphore(hSemaphore, View - Acq_view, ref cntsem);
                            }
                        }

                        //Rev23.10 シフトスキャンの場合は、sft_modeを切り替える
                        //if (sft_mode == 0 && scan_mode == 4)
                        //Rev25.00 Wスキャン追加 by長野 2016/07/07
                        if (sft_mode == 0 && (scan_mode == 4 || w_scan == 1))
                        {
                            sft_mode = 1;
                        }

                    }
                }
            }
            catch (Exception ex) //Rev25.00 暫定処置 by長野 2016/08/04
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                //Rev20.00 追加 by長野 2014/08/27
                //連続回転停止（連続回転モード時のみ）
                if (table_rotation == 1)
                {
                    MechaControl.RotateSlowStop(hDevID, null);
                    //Error = RotateSlowStop(hDevID);
                    //if (Result == 0) Result = Error; //v10.0追加 by 間々田 2005-02-09
                }

                //Rev20.00 EndProcessScanへ移動 by長野 2015/02/21
                ////追加2015/01/29hata_if文追加
                ////エラー発生の場合は回転を戻さない
                //if (Error == 0) 
                //    //Rev20.00 追加 by長野 2014/08/27
                //    MechaControl.RotateIndex(hDevID, 0, 0, null);


                // キャプチャ停止時処理
                Pulsar.CaptureSeqStop(Pulsar.hMil, Pulsar.hPke);
                ScanError = Error;
                OnCaptureScanEnd();
            }

            return Error;

        }

        //Rev20.00 ここまで シングルスキャン用の処理 by長野 2014/08/18
        #endregion

        //Scan終了のイベント
        protected void OnCaptureScanEnd()
        {
            if (CaptureScanEnd != null)
            {
                CaptureScanEnd(ScanError);
            }
        }

   }



}
