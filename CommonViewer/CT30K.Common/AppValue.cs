using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using CTAPI;

namespace CT30K.Common
{
    public sealed class AppValue
    {
        /// <summary>
        /// デバッグモード
        /// </summary>
        public static readonly bool IsDebug;

        /// <summary>
        /// テストモード
        /// </summary>
        public static readonly bool IsTestMode;


        #region 各種パス
        /// <summary>
        /// CTパス
        /// </summary>
        public static readonly string CTPATH;

        /// <summary>
        /// CTテンポラリパス
        /// </summary>
        public static readonly string CTTEMP;

        /// <summary>
        /// SETFILEのパス
        /// </summary>
        public static readonly string PathSetFile;

        /// <summary>
        /// 2nd SETFILEのパス
        /// </summary>
        public static readonly string SecondDetPathSetFile;

        /// <summary>
        /// CTユーザパス
        /// </summary>
        public static readonly string CTUSER;
        #endregion

        #region 各種実行ファイル
        /// <summary>
        /// スキャン実行ファイル
        /// </summary>
        public static readonly string SCANAV;

        /// <summary>
        /// スキャノ実行ファイル //Rev21.00 追加 by長野 2015/03/08
        /// </summary>
        public static readonly string SCANOAV;

        /// <summary>
        /// 再構成リトライ実行ファイル
        /// </summary>
        public static readonly string RECONMST;


        /// <summary>
        /// コモン初期化実行ファイル
        /// </summary>
        public static readonly string COMSET;

        /// <summary>
        /// 通常のコーンビームスキャン実行ファイル
        /// </summary>
        public static readonly string CONEBEAM_NORMAL;

        /// <summary>
        /// 再構成高速化機能有のコーンビームスキャン実行ファイル
        /// </summary>
        public static readonly string CONEBEAM_GPGPU;

        /// <summary>
        /// 通常のコーンビーム再々構成実行ファイル
        /// </summary>
        public static readonly string CONERECON_NORMAL;

        /// <summary>
        /// 再構成高速化機能有のコーンビーム再々構成実行ファイル
        /// </summary>
        public static readonly string CONERECON_GPGPU;
        #endregion

        /// <summary>
        /// 透視画像サーバー（64ビット）
        /// </summary>
        //public static readonly string TransImageServer;

        /// <summary>
        /// 画像処理時の作業ファイル IMAGEDISPLAY.BMP
        /// </summary>
        public static readonly string DISPBMP;

        /// <summary>
        /// 画像処理時の作業ファイル MAGERESULT.IMG
        /// </summary>
        public static readonly string OTEMPIMG;

        /// <summary>
        /// 画像処理時の作業ファイル MAGELAST.IMG
        /// </summary>
        public static readonly string LASTIMG;

        /// <summary>
        /// 画像処理時の作業ファイル temp_zom.csv
        /// </summary>
        public static readonly string ZOOMTMPCSV;

        /// <summary>
        /// SETFILE スキャン位置の傾き・切片
        /// </summary>
        public static readonly string SCANPOSI_CSV;

        /// <summary>
        /// SETFILE FID/FCDオフセット
        /// </summary>
        public static readonly string OFFSET_CSV;

        /// <summary>
        /// SETFILE FID/FCDオフセット
        /// </summary>
        public static readonly string OFFSET_2_CSV;
        
        /// <summary>
        /// オプション設定ファイル
        /// </summary>
        public static readonly string CTOPTION;

        /// <summary>
        /// iniファイル
        /// </summary>
        public static readonly string IniFileName;

        /// <summary>
        /// 段階ウォームアップ設定ファイル
        /// </summary>
        public static readonly string STEPWUP_INI;

        /// <summary>
        /// BHC対応 画像パス
        /// </summary>
        //public static readonly string InitDir_ImageOpen;
        public static string InitDir_ImageOpen;

        /// <summary>
        /// BHC対応 BHCテーブルパス
        /// </summary>
        //public static readonly string InitDir_BHCTable;
        public static string InitDir_BHCTable;

        /// <summary>
        /// PKE用iniファイル
        /// </summary>
        public static readonly string XisIniFileName;

        /// <summary>
        /// 外観カメラ用iniファイル //Rev23.30 追加 by長野 2016/02/06
        /// </summary>
        public static readonly string ExObsCamIniFileName;

        /// <summary>
        /// 保存先のドライブ名
        /// </summary>
        public static string MyDrive = "C";

        /// <summary>
        /// コーンビームスキャン実行ファイル
        /// </summary>
        public static string CONEBEAM
        {
            get
            {
                if (GPGPU)
                {
                    return CONEBEAM_GPGPU;
                }
                else
                {
                    return CONEBEAM_NORMAL;
                }
            }
        }

        /// <summary>
        /// コーンビーム再々構成実行ファイル
        /// </summary>
        public static string CONERECON
        {
            get
            {
                if (GPGPU)
                {
                    return CONERECON_GPGPU;
                }
                else
                {
                    return CONERECON_NORMAL;
                }
            }
        }

        /// <summary>
        /// GPGPUあり
        /// </summary>
        public static bool GPGPU { get; set; }

        /// <summary>
        /// スタティックコンストラクタ
        /// </summary>
        static AppValue()
        {

            // コマンドライン取得
            string[] cmds = Environment.GetCommandLineArgs();

            // モード
            IsDebug = false;
            IsTestMode = false;
            foreach (string arg in cmds)
            {
                if (arg == "-debug") IsDebug = true;
                if (arg == "-test") IsTestMode = true;
            }

            // アプリケーションディレクトリ取得
            string pathCommand = Path.GetDirectoryName(cmds[0]);

//#if DEBUG
////#if !DEBUG
//            // 実行ファイルのパス
//            pathCommand = Application.StartupPath;
//#endif

            //if (IsDebug) pathCommand = @"C:\CT\COMMAND";
            pathCommand = @"C:\CT\COMMAND";

            DirectoryInfo ctPathInfo = new DirectoryInfo(pathCommand);
            string rootDir = ctPathInfo.Root.FullName;
            

            // CTパス
            CTPATH = ctPathInfo.Parent.FullName;
            //CTPATH = ctPathInfo.FullName;

            // CT下のパス
            CTTEMP = Path.Combine(CTPATH, @"Temp");
            PathSetFile = Path.Combine(CTPATH, @"SETFILE");
            SecondDetPathSetFile = Path.Combine(PathSetFile, @"2ndDetSetFile");

            // CTUSERパス
            CTUSER = Path.Combine(rootDir, @"CTUSER");

            // 実行ファイル(*.exe)パス
            SCANAV = Path.Combine(pathCommand, @"scanav.exe");
            RECONMST = Path.Combine(pathCommand, @"reconmst.exe");
            COMSET = Path.Combine(pathCommand, @"comset.exe");
            //Rev21.00 追加 by長野 2015/03/08
            SCANOAV = Path.Combine(pathCommand, @"mScano.exe");

            CONEBEAM_NORMAL = Path.Combine(pathCommand, @"conebeam.exe");
            CONEBEAM_GPGPU = Path.Combine(pathCommand, @"conebeamgpgpu.exe");
            CONERECON_NORMAL = Path.Combine(pathCommand, @"conerecon.exe");
            CONERECON_GPGPU = Path.Combine(pathCommand, @"conerecongpgpu.exe");

            // 透視画像サーバー(64bit)
            //TransImageServer = Path.Combine(pathCommand, @"TransImageServer.exe");

            // 画像処理時の作業ファイル
            DISPBMP = Path.Combine(CTTEMP, @"IMAGEDISPLAY.BMP");
            OTEMPIMG = Path.Combine(CTTEMP, @"IMAGERESULT.IMG");
            LASTIMG = Path.Combine(CTTEMP, @"IMAGELAST.IMG");
            ZOOMTMPCSV = Path.Combine(CTTEMP, @"temp_zom.csv");

            // SETFILE
            SCANPOSI_CSV = Path.Combine(PathSetFile, @"scan_posi.csv");
            OFFSET_CSV = Path.Combine(PathSetFile, @"offset.csv");
            OFFSET_2_CSV = Path.Combine(SecondDetPathSetFile, @"offset_2.csv");

            // オプション設定ファイル
            CTOPTION = Path.Combine(CTPATH, @"OPTION\ctoption");

            // iniファイル
            IniFileName = Path.Combine(CTPATH, @"ini\ct30k.ini");

            // 段階ウォームアップ設定ファイル
            STEPWUP_INI = Path.Combine(CTPATH, @"ini\StepWUP.ini");

            // BHC対応
            InitDir_ImageOpen = Path.Combine(CTUSER, @"IMAGE");
            InitDir_BHCTable = Path.Combine(CTUSER, @"BHCTABLE");
            
            //Rev23.30 追加 by長野 2016/02/06
            ExObsCamIniFileName = Path.Combine(CTPATH, @"ini\ExObsCam.ini");

            // PKE用
            XisIniFileName = @"C:\XIS\Program\xis.ini";
        }
    }
}
