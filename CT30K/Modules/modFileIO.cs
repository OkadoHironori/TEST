using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;

//using Microsoft.VisualBasic;
//using VB = Microsoft.VisualBasic;

using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Text.RegularExpressions;
//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;
//using CT30K.Controls;
//using CT30K.Forms;

namespace CT30K
{
    public static class modFileIO
    {
        //#region 各種パス
        ///// <summary>
        ///// CTのパス（通常は C:\CT）
        ///// </summary>
        //public static string CTPATH;

        ///// <summary>
        ///// 実行ファイルのパス
        ///// </summary>
        //public static string pathCommand = @"C:\CT\COMMAND";

        ///// <summary>
        ///// SETFILEのパス
        ///// </summary>
        //public static string pathSetFile;

        ///// <summary>
        ///// 2nd検出器用SETFILEのパス    v17.20追加 by 長野 2010/09/20
        ///// </summary>
        //public static string SecondDetPathSetFile;

        ///// <summary>
        ///// ＣＴテンプディレクトリ     C:\CT\TEMP
        ///// </summary>
        //public static string CTTEMP;

        ///// <summary>
        ///// CTUSERのパス               C:\CTUSER
        ///// </summary>
        //public static string CTUSER;
        //#endregion

        //#region 実行ファイル（*.exe）のパス
        ///// <summary>
        ///// スキャン                   C:\CT\COMMAND\scanav.exe
        ///// </summary>
        //public static string SCANAV;
        
        ///// <summary>
        ///// 再構成リトライ                  C:\CT\COMMAND\reconmst.exe
        ///// </summary>
        //public static string RECONMST;

        ///// <summary>
        ///// コーンビームスキャン            C:\CT\COMMAND\conebeam.exe
        ///// </summary>
        //public static string CONEBEAM;

        ///// <summary>
        ///// コーンビーム再々構成            C:\CT\COMMAND\conerecon.exe
        ///// </summary>
        //public static string CONERECON;
        
        ///// <summary>
        ///// コモン初期化                    C:\CT\COMMAND\comset.exe
        ///// </summary>
        //public static string COMSET;

        ///// <summary>
        ///// コーンビームスキャン            C:\CT\COMMAND\conebeam.exe
        ///// </summary>
        //public static string CONEBEAM_NORMAL;
        
        ///// <summary>
        ///// コーンビーム再々構成            C:\CT\COMMAND\conerecon.exe
        ///// </summary>
        //public static string CONERECON_NORMAL;

        ///// <summary>
        ///// 高速再構成コーンビームスキャン  C:\CT\COMMAND\conebeamgpgpu.exe
        ///// </summary>
        //public static string CONEBEAM_GPGPU;

        ///// <summary>
        ///// 高速再構成コーンビーム再々構成 C:\CT\COMMAND\conerecongpgpu.exe
        ///// </summary>
        //public static string CONERECON_GPGPU;
        //#endregion

        //#region 画像処理時の作業ファイル
        ///// <summary>
        ///// 画像表示用ビットマップ     C:\CT\TEMP\IMAGEDISPLAY.BMP
        ///// </summary>
        //public static string DISPBMP;
        
        ///// <summary>
        ///// 画像処理時の結果画像       C:\CT\TEMP\IMAGERESULT.IMG
        ///// </summary>
        //public static string OTEMPIMG;
        
        ///// <summary>
        ///// 「元に戻す」用画像         C:\CT\TEMP\IMAGELAST.IMG
        ///// </summary>
        //public static string LASTIMG;

        ///// <summary>
        ///// 作業用ズーミングテーブル   C:\CT\TEMP\temp_zom.csv
        ///// </summary>
        //public static string ZOOMTMPCSV;
        //#endregion

        //#region SETFILE
        ///// <summary>
        ///// スキャン位置の傾き・切片   C:\CT\SETFILE\scan_posi.csv
        ///// </summary>
        //public static string SCANPOSI_CSV;

        ///// <summary>
        ///// FID/FCDオフセット          C:\CT\SETFILE\offset.csv
        ///// </summary>
        //public static string OFFSET_CSV;

        ///// <summary>
        ///// FID/FCDオフセット（検出器切替用）   C:\CT\SETFILE\offset_2.csv
        ///// </summary>
        //public static string OFFSET_2_CSV;
        //#endregion

        //#region その他
        ///// <summary>
        ///// オプション設定ファイル     C:\CT\OPTION\ctoption
        ///// </summary>
        //public static string CTOPTION;

        ///// <summary>
        ///// iniファイル                C:\CT\ini\ct30k.ini
        ///// </summary>
        //public static string IniFileName;
        //#endregion

        ///// <summary>
        ///// 保存先のドライブ名
        ///// </summary>
        //public static string MyDrive = "C";

        //public static string STEPWUP_INI;		//段階ウォームアップ設定ファイル C:\CT\INI\StepWUP.ini   'v17.72/v19.02追加 byやまおか 2012/05/16

        ////Pke用iniファイル   'v17.50追加 by 間々田 2011/03/04
        //public const string XisIniFileName = @"C:\XIS\Program\xis.ini";

        ////v19.00 追加(電S2)永井
        //public static string InitDir_ImageOpen;		//BHC画像ディレクトリパス名
        //public static string InitDir_BHCTable;		//BHCテーブルパス

        /// <summary>
        /// VBが使用するレジストリ キー
        /// "Software\VB and VBA Program Settings"
        /// </summary>
        //private static string RegistryKeyName = @"Software\VB and VBA Program Settings";
        private static string RegistryKeyName = @"software\ToshibaITC";

        #region フィールド
        private static int myDlgFilterIndex = 1;
        #endregion

        #region プロパティ
        public static int DlgFilterIndex
        {
            get
            {
                return (myDlgFilterIndex);
            }
            set
            {
                myDlgFilterIndex = value;
            }
        }
        #endregion

        #region 静的コンストラクタ
        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static modFileIO()
        {
            //#region CT30Kで使用するパスの初期化
//#if !DEBUG
//            // 実行ファイルのパス
//            pathCommand = Application.StartupPath;
//#endif
//            // CTのパス（通常は C:\CT）
//            CTPATH = Path.GetDirectoryName(pathCommand);

//            // CT下のパス
//            CTTEMP = Path.Combine(CTPATH, "Temp");

//            // ＣＴテンプディレクトリ C:\CT\Temp
//            pathSetFile = Path.Combine(CTPATH, "SETFILE");
            
//            // SETFILEのパス
//            SecondDetPathSetFile = Path.Combine(pathSetFile, "2ndDetSetFile");

//            // CTUSERのパス
//            CTUSER = Path.Combine(Path.GetDirectoryName(CTPATH), "CTUSER");

//            // スキャン
//            SCANAV = Path.Combine(pathCommand, "scanav.exe");

//            // 再構成リトライ
//            RECONMST = Path.Combine(pathCommand, "reconmst.exe");
            
//            // コーンビームスキャン
//            CONEBEAM = Path.Combine(pathCommand, "conebeam.exe");
            
//            // コーンビーム再々構成
//            CONERECON = Path.Combine(pathCommand, "conerecon.exe");

//            //コモン初期化
//            COMSET = Path.Combine(pathCommand, "comset.exe");

//            // 通常のコーンビームスキャン
//            CONEBEAM_NORMAL = Path.Combine(pathCommand, "conebeam.exe");
            
//            // 再構成高速化機能有のコーンビームスキャン
//            CONEBEAM_GPGPU = Path.Combine(pathCommand, "conebeamgpgpu.exe");

//            // 通常のコーンビーム再々構成
//            CONERECON_NORMAL = Path.Combine(pathCommand, "conerecon.exe");

//            // 再構成高速化機能有のコーンビーム再々構成
//            CONERECON_GPGPU = Path.Combine(pathCommand, "conerecongpgpu.exe");
            
//            // 画像処理時の作業ファイル
//            DISPBMP = Path.Combine(CTTEMP, "IMAGEDISPLAY.BMP");
//            OTEMPIMG = Path.Combine(CTTEMP, "IMAGERESULT.IMG");
//            LASTIMG = Path.Combine(CTTEMP, "IMAGELAST.IMG");
//            ZOOMTMPCSV = Path.Combine(CTTEMP, "temp_zom.csv");

//            // SETFILE
//            SCANPOSI_CSV = Path.Combine(pathSetFile, "scan_posi.csv");

//            // スキャン位置の傾き・切片
//            OFFSET_CSV = Path.Combine(pathSetFile, "offset.csv");
//            OFFSET_2_CSV = Path.Combine(SecondDetPathSetFile, "offset_2.csv"); //検出器切替用のSETFILE

//            // オプション設定ファイル
//            CTOPTION = Path.Combine(CTPATH, "OPTION\\ctoption");

//            // iniファイル
//            IniFileName = Path.Combine(CTPATH, "ini\\ct30k.ini");
//            #endregion

//            //var iniFile = new IniFile(IniFileName);
//            //string path = iniFile.GetIniString("DoubleOblique", "ExeFileName");

//            //Console.WriteLine(path);
        }
        #endregion

        #region プロパティ

        #endregion

        //*************************************************************************************************
        //機　　能： CT30Kで使用するパスの初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.0  2008/10/03  (SS1)間々田  新規作成
        //*************************************************************************************************
        public static void InitPath()
        {
            
            //#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
            ///*
            //'実行ファイルのパス
            //If InStr(LCase$(command()), "-debug") > 0 Then
            //*/
            //#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            //// コマンドライン引数取得　GetCommandLineArgs()の先頭要素(index = 0)は実行したプログラムファイル名なので無視する
            //string command = string.Join(" ", Environment.GetCommandLineArgs(), 1, Environment.GetCommandLineArgs().Length - 1);

            ////実行ファイルのパス
            //if (command.ToLower().IndexOf("-debug") > -1)
            //{
            //    pathCommand = @"C:\CT\COMMAND";
            //}
            //else
            //{
            //    pathCommand = Application.StartupPath;
            //}

            ////CTのパス（通常は C:\CT）
            //CTPATH = Path.GetDirectoryName(pathCommand);

            ////CT下のパス
            //CTTEMP = Path.Combine(CTPATH, "Temp");				//ＣＴテンプディレクトリ C:\CT\Temp
            //pathSetFile = Path.Combine(CTPATH, "SETFILE");		//SETFILEのパス
            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            ////    SecondDetPathSetFile = FSO.BuildPath(pathSetFile, "2ndDetSetFile")
            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            ////CTUSERのパス
            //CTUSER = Path.Combine(Path.GetDirectoryName(CTPATH), "CTUSER");

            ////実行ファイル（*.exe）のパス
            //SCANAV = Path.Combine(pathCommand, "scanav.exe");			//スキャン
            //RECONMST = Path.Combine(pathCommand, "reconmst.exe");		//再構成リトライ
            //CONEBEAM = Path.Combine(pathCommand, "conebeam.exe");		//コーンビームスキャン
            //CONERECON = Path.Combine(pathCommand, "conerecon.exe");	//コーンビーム再々構成
            //COMSET = Path.Combine(pathCommand, "comset.exe");			//コモン初期化

            ////v16.2 追加 by 山影 10-04-02
            //CONEBEAM_NORMAL = Path.Combine(pathCommand, "conebeam.exe");			//通常のコーンビームスキャン
            //CONEBEAM_GPGPU = Path.Combine(pathCommand, "conebeamgpgpu.exe");		//再構成高速化機能有のコーンビームスキャン
            //CONERECON_NORMAL = Path.Combine(pathCommand, "conerecon.exe");			//通常のコーンビーム再々構成
            //CONERECON_GPGPU = Path.Combine(pathCommand, "conerecongpgpu.exe");		//再構成高速化機能有のコーンビーム再々構成

            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            ////透視画像サーバー（64ビット） v17.5追加 2010/12/17 by 間々田
            ////    TransImageServer = FSO.BuildPath(pathCommand, "TransImageServer.exe") 'C:\CT\COMMAND\TransImageServer.exe
            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            ////画像処理時の作業ファイル
            //DISPBMP = Path.Combine(CTTEMP, "IMAGEDISPLAY.BMP");
            //OTEMPIMG = Path.Combine(CTTEMP, "IMAGERESULT.IMG");
            //LASTIMG = Path.Combine(CTTEMP, "IMAGELAST.IMG");
            //ZOOMTMPCSV = Path.Combine(CTTEMP, "temp_zom.csv");

            ////SETFILE
            //SCANPOSI_CSV = Path.Combine(pathSetFile, "scan_posi.csv");		//スキャン位置の傾き・切片
            //OFFSET_CSV = Path.Combine(pathSetFile, "offset.csv");			//FID/FCDオフセット

            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            ////v17.20 検出器切替用のSETFILE 追加 by 長野 2010-08-31
            ////v17.30 修正　scaninhibitを取得する前なので常にパスが取れなかった　by　長野　2010-09-28
            ////If SecondDetOn Then
            ////    OFFSET_2_CSV = FSO.BuildPath(SecondDetPathSetFile, "offset_2.csv")       'FID/FCDオフセット
            ////End If
            ////v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''

            ////オプション設定ファイル
            //CTOPTION = Path.Combine(CTPATH, "OPTION\\ctoption");

            ////iniファイル
            //IniFileName = Path.Combine(CTPATH, "ini\\ct30k.ini");

            ////段階ウォームアップ設定ファイル     'v17.72/v19.02追加 byやまおか 2012/05/16
            //STEPWUP_INI = Path.Combine(CTPATH, "ini\\StepWUP.ini");

            ////v19.00 BHC対応 削除(電S2)永井
            ////DEF_IMGSAVEDIR = CTPATH & "CTUSER\画像\"
            ////DEF_BHCTBLDIR = CTPATH & "CTUSER\BHCﾃｰﾌﾞﾙ"
            //InitDir_ImageOpen = Path.Combine(CTUSER, CTResources.LoadResString(StringTable.IDS_BHCImageDir));
            //InitDir_BHCTable = Path.Combine(CTUSER, CTResources.LoadResString(StringTable.IDS_BHCTableDir));
        
        }

        #region フォルダ参照ダイアログを使用してフォルダ名を取得する
        /// <summary>
        /// フォルダ参照ダイアログを使用してフォルダ名を取得する
        /// </summary>
        /// <param name="SelectedPath"></param>
        /// <returns></returns>
        public static string GetFolderName(string SelectedPath)
        {
            return GetFolderName(SelectedPath, "", true);
        }

        /// <summary>
        /// フォルダ参照ダイアログを使用してフォルダ名を取得する
        /// </summary>
        /// <param name="SelectedPath"></param>
        /// <param name="ShowNewFolderButton"></param>
        /// <returns></returns>
        public static string GetFolderName(string SelectedPath, bool ShowNewFolderButton)
        {
            return GetFolderName(SelectedPath, "", ShowNewFolderButton);
        }

        /// <summary>
        /// フォルダ参照ダイアログを使用してフォルダ名を取得する
        /// </summary>
        /// <param name="SelectedPath"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
	    public static string GetFolderName(string SelectedPath, string Description)
	    {
		    return GetFolderName(SelectedPath, Description, true);
	    }

        /// <summary>
        /// フォルダ参照ダイアログを使用してフォルダ名を取得する
        /// </summary>
        /// <param name="SelectedPath"></param>
        /// <param name="Description"></param>
        /// <param name="ShowNewFolderButton"></param>
        /// <returns></returns>
	    public static string GetFolderName(string SelectedPath, string Description, bool ShowNewFolderButton)
	    {
            // 戻り値用変数
		    string result = string.Empty;

		    // フォルダ選択ダイアログ生成
            using (var dialog = new FolderBrowserDialog())
            {
                // デフォルトのフォルダ
                dialog.SelectedPath = SelectedPath;

                // このダイアログに対する説明を表示する場合：～を選択してください。
                dialog.Description = (string.IsNullOrEmpty(Description) ? "" : StringTable.GetResString(9957, Description));

                //「新しいフォルダの作成」ボタンを表示させるか
                dialog.ShowNewFolderButton = ShowNewFolderButton;

                //　フォルダ選択ダイアログ表示：選択したパスを取得
                if (dialog.ShowDialog(frmCTMenu.Instance) == DialogResult.OK)
                    result = dialog.SelectedPath;
            }

            return result;
	    }
        #endregion

        #region コモンダイアログを利用してファイル名を取得する
        //public static string GetFileName(int Operation, string Description, string Extension)
        //{
        //    return GetFileName(Operation, Description, Extension, "", "", "");
        //}
        //*************************************************************************************************
        //機　　能： コモンダイアログを利用してファイル名を取得する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3 2006/01/31 (SI3)間々田    新規作成
        //*************************************************************************************************
        //public static string GetFileName(int Operation = StringTable.IDS_Open, string Description = "", string Extension = ".csv", string SubExtension = "", string InitFileName = "", string Purpose = "")
        public static string GetFileName(int Operation = StringTable.IDS_Open, string Description = "", string Extension = ".csv", string SubExtension = "", string InitFileName = "", string Purpose = "")
        {
            //戻り値初期化
            string functionReturnValue = "";

            //引数の補正
            switch (Operation)
            {
                case StringTable.IDS_Open:
                case StringTable.IDS_Save:
                case StringTable.IDS_Select:
                case StringTable.IDS_FileSpecify:
                    break;
                default:
                    Operation = StringTable.IDS_Open;
                    break;
            }

            string dialogTitle = "";
            string dialogFileName = "";
            string dialogFilter = "";
            int dialogFilterIndex = 0;
            int selectedDlgFilterInex = 1; //Rev22.00 追加 by長野 2015/07/02
            bool showReadOnly = false;
            bool overwritePrompt = false;
            bool checkFileExists = false;
            bool checkPathExists = false;
            string dialogInitialDirectory = "";

            #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
            /*
			'キャンセルボタン選択時の設定
			On Error Resume Next
*/
            #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            try
            {
                //ダイアログ設定

                //ダイアログタイトル
                if (string.IsNullOrEmpty(Description))
                {
                    dialogTitle = "";
                }
                else if (Operation == StringTable.IDS_FileSpecify)
                {
                    dialogTitle = StringTable.GetResString(Operation, Purpose);
                }
                else if (string.IsNullOrEmpty(Purpose))
                {
                    dialogTitle = StringTable.GetResString(Operation, Description);
                }
                else if (Description == CTResources.LoadResString(12597)) //Rev22.00 追加 by長野 2015/07/02
                {
                    dialogTitle = Description;
                }
                else
                {
                    dialogTitle = StringTable.GetResString(Operation, StringTable.GetResString(10121, Purpose, Description));
                }

                //デフォルトファイル名
                if (Operation == StringTable.IDS_Save)
                {
                    dialogFileName = modLibrary.AddExtension(InitFileName, SubExtension + Extension);
                }
                else
                {
                    dialogFileName = "";
                }

                //追加2015/01/22hata
                //フルパスで表示されるため、ここでファイル名だけにする
                if (dialogFileName != "") dialogFileName =  Path.GetFileName(dialogFileName);


                //フィルタ
                dialogFilter = CTResources.LoadResString(10300);			//すべてのファイル(*.*)|*.*

                //透視画像の場合
                if (Description == CTResources.LoadResString(StringTable.IDS_TransImage))
                {
                    //.filter = LoadResString(IDS_FilerFIMG) & "|" & LoadResString(10300)         'BMP(*.BMP)|*.BMP|JPG(*.JPG)|*.JPG|TIF(*.TIF)|*.TIF|すべてのファイル(*.*)|*.*
                    if ((Operation == StringTable.IDS_Open))
                    {
                        //.filter = LoadResString(IDS_TransImage) & "(*.BMP/*.JPG/*.TIF)|*.BMP;*.JPG;*.TIF|すべてのファイル(*.*)|*.*"  '種類を切り替えなくても表示されるようにした
                        //v17.60 ストリングテーブル化 & すべてのファイル削除 by長野 2011/05/30
                        //dialogFilter = CTResources.LoadResString(StringTable.IDS_TransImage) + CTResources.LoadResString(10309);
                        //Rev20.00 変更 by長野 2015/02/06
                        dialogFilter = CTResources.LoadResString(10309);

                    }
                    else
                    {
                        //.filter = LoadResString(IDS_FilerFIMG) & "|" & LoadResString(10300)     'BMP(*.BMP)|*.BMP|JPG(*.JPG)|*.JPG|TIF(*.TIF)|*.TIF|すべてのファイル(*.*)|*.*

                        //-->v17.50現地修正 by 間々田 2011/03/16

                        #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                        /*
						'デフォルトの保存拡張子をレジストリから取得
						Select Case UCase$(GetSetting(App.Title, Description, "DefaultExtension"))
*/
                        #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                        //デフォルトの保存拡張子をレジストリから取得
                        //string subKey = Path.Combine(RegistryKeyName, Application.ProductName, Description);
                        //using (RegistryKey key = Registry.CurrentUser.CreateSubKey(subKey))
                        //{
                        //switch (Convert.ToString(key.GetValue("DefaultExtension")).ToUpper())	//TODO レジストリから設定読み出し
                        // レジストリから読み込む
                        string data = GetRegistry(Path.Combine(RegistryKeyName, Application.ProductName), Description, "DefaultExtension");
                        switch (data.ToUpper())
                        {
                            //
                            case "JPG":
                                dialogFilter = MakeFilterString("JPG", "BMP", "TIF");
                                break;
                            case "TIF":
                                dialogFilter = MakeFilterString("TIF", "BMP", "JPG");
                                break;
                            default:
                                dialogFilter = MakeFilterString("BMP", "JPG", "TIF");
                                break;
                        }
                        //}

                        //v17.60 ファイル保存のとき「すべてのファイル」を選択して、保存するとソフトが落ちるため、
                        //保存時は、表示しない。 by 長野 2011/06/14
                        //「すべてのファイル(*.*)|*.*」を付加
                        //.filter = .filter & "|" & LoadResString(10300)

                        //<--v17.50現地修正 by 間々田 2011/03/16
                    }
                }
                else if (Description == CTResources.LoadResString(12597)) //Rev22.00 動画保存の場合を追加
                {
                    dialogFilter = CTResources.LoadResString(20066) + "|*@".Replace("@", SubExtension + Extension) + "|" + "MJPG動画ファイル" + "|*@".Replace("@", SubExtension + Extension);		//～(*aaa.bbb)|*aaa.bbb
                }
                else if (!string.IsNullOrEmpty(Description))    //条件式復活2015/01/20hata　
                {
                    //Rev20.00 条件式追加 -○○○.csvなどファイル名を含むフィルタにする場合、かつ、「常に拡張子を表示する」にチェックがある場合、(*-○○○.csv)が2重に出てしまうため by長野 2014/12/15
                    //
                    if (Description == CTResources.LoadResString(StringTable.IDS_SlicePlanTable))
                    {
                        dialogFilter = Description + "|*@".Replace("@", SubExtension + Extension);		//～(*aaa.bbb)|*aaa.bbb
                    }
                    else
                    {
                        dialogFilter = Description + "(*@)|*@".Replace("@", SubExtension + Extension);		//～(*aaa.bbb)|*aaa.bbb
                        //追加2015/01/22hata
                        if (SubExtension != "")
                        {
                            dialogFilter = Description + "|*@".Replace("@", SubExtension + Extension);		//～(*aaa.bbb)|*aaa.bbb
                        }
                    }
                }
    
                //スライスプランテーブルの場合で、コーンビーム・スライスプランの組み合わせが可の場合
                if (Description == CTResources.LoadResString(StringTable.IDS_SlicePlanTable) && (CTSettings.scaninh.Data.cone_multiscan_mode[2] == 0))
                {
                    dialogFilter = dialogFilter.ToLower();
                    if (Operation == StringTable.IDS_Open)
                    {
                        if (SubExtension.ToLower() == "-spl")
                        {
                            //.filter = "スキャン用" & .filter & "|" & "コーンビーム用" & Replace$(.filter, "-spl", "-csp")
                            //v17.60 ストリングテーブル化 by長野 2010/05/30
                            dialogFilter = CTResources.LoadResString(20110) + dialogFilter + "|" + CTResources.LoadResString(12390) + dialogFilter.Replace("-spl", "-csp");
                        }
                        else
                        {
                            //.filter = "コーンビーム用" & .filter & "|" & "スキャン用" & Replace$(.filter, "-csp", "-spl")
                            dialogFilter = CTResources.LoadResString(12390) + dialogFilter + "|" + CTResources.LoadResString(20110) + dialogFilter.Replace("-csp", "-spl");
                        }
                    }
                    else
                    {
                        if (SubExtension.ToLower() == "-spl")
                        {
                            dialogFilter = CTResources.LoadResString(20110) + dialogFilter;
                        }
                        else
                        {
                            dialogFilter = CTResources.LoadResString(12390) + dialogFilter;
                        }
                    }
                }
                //生データファイルが指定されていて，コーンビーム可の場合
                else if ((Description == CTResources.LoadResString(StringTable.IDS_RawFile)) && (CTSettings.scaninh.Data.data_mode[2] == 0))
                {
                    dialogFilter = dialogFilter + "|" +CTResources.LoadResString(StringTable.IDS_ConeRawFile) + "(*@)|*@".Replace("@", SubExtension + ".cob");
                }

                //フィルタのインデクス
                dialogFilterIndex = 1;

                //フラグ
                showReadOnly = false;

                if (Operation == StringTable.IDS_Save)
                {
                    overwritePrompt = true;		//既に存在する場合はメッセージボックスを表示
                }
                else if (Operation != StringTable.IDS_FileSpecify)
                {
                    #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                    /*
					.Flags = .Flags Or cdlOFNFileMustExist      '既存のファイルだけを入力できるようにする
*/
                    #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                    //既存のファイルだけを入力できるようにする
                    checkFileExists = true;
                    checkPathExists = true;
                }

                //デフォルトのパス
                if (!string.IsNullOrEmpty(InitFileName))
                {
                    dialogInitialDirectory = Path.GetDirectoryName(InitFileName);
                }
                else
                {
                    dialogInitialDirectory = GetDefaultFolder(Description);
                }

                //ダイアログ表示
                if ((Operation == StringTable.IDS_Save) || (Operation == StringTable.IDS_FileSpecify))
                {
                    #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                    /*
					.ShowSave
*/
                    #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                    SaveFileDialog dlgGetFileName = new SaveFileDialog();

                    dlgGetFileName.Title = dialogTitle;
                    dlgGetFileName.FileName = dialogFileName;
                    dlgGetFileName.Filter = dialogFilter;
                    dlgGetFileName.FilterIndex = dialogFilterIndex;
                    dlgGetFileName.OverwritePrompt = overwritePrompt;
                    dlgGetFileName.InitialDirectory = dialogInitialDirectory;
                                        
                    if (dlgGetFileName.ShowDialog() == DialogResult.Cancel) return functionReturnValue;
                    dialogFileName = dlgGetFileName.FileName;
                    selectedDlgFilterInex = dlgGetFileName.FilterIndex; //Rev22.00 追加 by長野 2015

                }
                else
                {
                    #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                    /*
					.ShowOpen
*/
                    #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                    OpenFileDialog dlgGetFileName = new OpenFileDialog();

                    dlgGetFileName.Title = dialogTitle;
                    dlgGetFileName.FileName = dialogFileName;
                    dlgGetFileName.Filter = dialogFilter;
                    dlgGetFileName.FilterIndex = dialogFilterIndex;
                    dlgGetFileName.ShowReadOnly = showReadOnly;
                    dlgGetFileName.CheckFileExists = checkFileExists;
                    dlgGetFileName.CheckPathExists = checkPathExists;
                    dlgGetFileName.InitialDirectory = dialogInitialDirectory;

                    if (dlgGetFileName.ShowDialog() == DialogResult.Cancel) return functionReturnValue;
                    dialogFileName = dlgGetFileName.FileName;
                    selectedDlgFilterInex = dlgGetFileName.FilterIndex; //Rev22.00 追加 by長野 2015
                   
               }
            }
            catch (Exception ex)
            {
                //エラーの場合、抜ける
                //キャンセルボタン選択時以外のエラーの場合、エラーメッセージを表示
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return functionReturnValue;
            }

            try
            {
                //ファイル単純指定モード時はそのままファイル名を返す
                if (Operation == StringTable.IDS_FileSpecify)
                {
                    //戻り値セット
                    functionReturnValue = dialogFileName;
                }
                //透視画像の場合はそのままファイル名を返す
                else if (Description == CTResources.LoadResString(StringTable.IDS_TransImage))
                {
                    functionReturnValue = dialogFileName;
                }
                else if (Description == CTResources.LoadResString(12597)) //Rev22.00 動画保存の場合を追加 by長野 2015/07/02
                {
                    functionReturnValue = dialogFileName;
                    myDlgFilterIndex = selectedDlgFilterInex;
                }
                //保存の場合はファイル名を細工する
                else if (Operation == StringTable.IDS_Save)
                {
                    //戻り値セット
                    functionReturnValue = Path.Combine(Path.GetDirectoryName(dialogFileName),
                                                        modLibrary.AddExtension(Path.GetFileNameWithoutExtension(dialogFileName), SubExtension) + Extension);
                }
                //生データファイルが指定されている場合
                else if (Description == CTResources.LoadResString(StringTable.IDS_RawFile))
                {
                    if (!(Regex.IsMatch(dialogFileName.ToUpper(), ("*" + Extension.ToUpper()).Replace(".", "[.]").Replace("*", ".+") + '$') ||
                          Regex.IsMatch(dialogFileName.ToUpper(), ("*.cob").ToUpper().Replace(".", "[.]").Replace("*", ".+") + '$') && (CTSettings.scaninh.Data.data_mode[2] == 0)))
                    {
                        //メッセージ表示：%1以外が選択されました。
                        MessageBox.Show(StringTable.GetResString(9964, Description), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return functionReturnValue;
                    }
                    functionReturnValue = dialogFileName;
                }
                //スライスプランテーブルの場合で、コーンビーム・スライスプランの組み合わせが可の場合
                else if ((Description == CTResources.LoadResString(StringTable.IDS_SlicePlanTable)) && (CTSettings.scaninh.Data.cone_multiscan_mode[2] == 0))
                {
                    if (Operation == StringTable.IDS_Open)
                    {
                        //指定外の拡張子が選ばれた場合
                        if (!(Regex.IsMatch(dialogFileName.ToUpper(), ("*-spl.csv").ToUpper().Replace(".", "[.]").Replace("*", ".+") + '$') ||
                              Regex.IsMatch(dialogFileName.ToUpper(), ("*-csp.csv").ToUpper().Replace(".", "[.]").Replace("*", ".+") + '$')))
                        {
                            //メッセージ表示：%1以外が選択されました。
                            MessageBox.Show(StringTable.GetResString(9964, Description), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return functionReturnValue;
                        }
                    }
                    else
                    {
                        //指定外の拡張子が選ばれた場合
                        if (!(Regex.IsMatch(dialogFileName.ToUpper(), ("*" + (SubExtension + Extension).ToUpper()).Replace(".", "[.]").Replace("*", ".+") + '$')))
                        {
                            //メッセージ表示：%1以外が選択されました。
                            MessageBox.Show(StringTable.GetResString(9964, modLibrary.GetFirstItem(dialogFilter, "(")),
                                            Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return functionReturnValue;
                        }
                    }
                    functionReturnValue = dialogFileName;
                }
                //上記以外
                else
                {
                    //指定外の拡張子が選ばれた場合
                    if (!(Regex.IsMatch(dialogFileName.ToUpper(), ("*" + (SubExtension + Extension).ToUpper()).Replace(".", "[.]").Replace("*", ".+") + '$')))
                    {
                        if (string.IsNullOrEmpty(Description))
                        {
                            //メッセージ表示：
                            //MsgBox "ファイルの拡張子が指定外です。", vbCritical
                            //v17.60 ストリングテーブル化 by長野 2011/05/25
                            MessageBox.Show(CTResources.LoadResString(20111), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            //メッセージ表示：%1以外が選択されました。
                            MessageBox.Show(StringTable.GetResString(9964, Description), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return functionReturnValue;
                    }
                    //画像ファイルの場合
                    else if (Extension.ToUpper() == ".IMG")
                    {
                        //付帯情報ファイルは存在するか？
                        if (!File.Exists(ChangeExtension(dialogFileName, ".inf")))
                        {
                            //メッセージ表示：付帯情報ファイルが見つかりません。
                            MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_NotFound, StringTable.IDS_InfoFile),
                                            Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return functionReturnValue;
                        }
                    }
                    //戻り値セット
                    functionReturnValue = dialogFileName;
                }
            }
            catch
            {
                //Nothing
            }

            try
            {
                //選択したパスをデフォルトパスとして記憶
                if (!string.IsNullOrEmpty(Description)) SaveDefaultFolder(Description, Path.GetDirectoryName(dialogFileName));

                //透視画像保存の際、拡張子をレジストリに記憶     'v17.50現地追加 by 間々田 2011/03/16
                if ((Description == CTResources.LoadResString(StringTable.IDS_TransImage)) && (Operation == StringTable.IDS_Save))
                {
                    #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                    /*
				SaveSetting App.Title, Description, "DefaultExtension", FSO.GetExtensionName(.FileName)
*/
                    #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                    //string subKey = Path.Combine(RegistryKeyName, Application.ProductName, Description);
                    //using (RegistryKey key = Registry.CurrentUser.CreateSubKey(subKey))
                    //{
                    //    key.SetValue("DefaultExtension", Path.GetExtension(dialogFileName).TrimStart('.'));	//TODO レジストリへの設定保存
                    //}
                    string title = Path.Combine(RegistryKeyName, Application.ProductName);
                    SaveRegistry(title, Description,"DefaultExtension",Path.GetExtension(dialogFileName).TrimStart('.'));


                }
            }
            catch
            {
                //Nothing
            }

            return functionReturnValue;
        }


        //public static string GetFileName(int Operation = StringTable.IDS_Open, string Description = "", string Extension = ".csv", string SubExtension = "", string InitFileName = "", string Purpose = "")
        //{
        //    string functionReturnValue = null;

        //    //戻り値初期化
        //    functionReturnValue = "";

        //    //引数の補正
        //    switch (Operation)
        //    {
        //        case StringTable.IDS_Open:
        //        case StringTable.IDS_Select:
        //            break;

        //        case StringTable.IDS_Save:
        //        case StringTable.IDS_FileSpecify:
        //            break;

        //        default:
        //            Operation = StringTable.IDS_Open;
        //            break;
        //    }

        //    // ファイル選択ダイアログ生成
        //    using (var dialog = new OpenFileDialog())
        //    {
        //        try
        //        {

        //            // ダイアログタイトル
        //            if (string.IsNullOrEmpty(Description))
        //            {
        //                dialog.Title = "";
        //            }
        //            //else if (Operation == StringTable.IDS_FileSpecify)
        //            //{
        //            //    dialog.Title = StringTable.GetResString(Operation, Purpose);
        //            //}

        //            else if (string.IsNullOrEmpty(Purpose))
        //            {
        //                dialog.Title = StringTable.GetResString(Operation, Description);
        //            }
        //            else
        //            {
        //                dialog.Title = StringTable.GetResString(Operation, StringTable.GetResString(10121, Purpose, Description));
        //            }

        //            // デフォルトファイル名
        //            //if (Operation == StringTable.IDS_Save)
        //            //{
        //            //    dialog.FileName = modLibrary.AddExtension(InitFileName, SubExtension + Extension);
        //            //}
        //            //else
        //            //{
        //            dialog.FileName = "";
        //            //}

        //            // フィルタ
        //            //すべてのファイル(*.*)|*.*
        //            dialog.Filter = Resources._10300; //すべてのファイル(*.*)|*.*

        //            //透視画像の場合
        //            if (Description == Resources.ResourceManager.GetString("str_" + Convert.ToString(StringTable.IDS_TransImage)))
        //            {
        //                //.filter = LoadResString(IDS_FilerFIMG) & "|" & LoadResString(10300)         'BMP(*.BMP)|*.BMP|JPG(*.JPG)|*.JPG|TIF(*.TIF)|*.TIF|すべてのファイル(*.*)|*.*
        //                if ((Operation == StringTable.IDS_Open))
        //                {
        //                    //.filter = LoadResString(IDS_TransImage) & "(*.BMP/*.JPG/*.TIF)|*.BMP;*.JPG;*.TIF|すべてのファイル(*.*)|*.*"  '種類を切り替えなくても表示されるようにした
        //                    //v17.60 ストリングテーブル化 & すべてのファイル削除 by長野 2011/05/30
        //                    dialog.Filter = Resources.ResourceManager.GetString("str_" + Convert.ToString(StringTable.IDS_TransImage)) + Resources._10309;
        //                }
        //                else
        //                {
        //                    //.filter = LoadResString(IDS_FilerFIMG) & "|" & LoadResString(10300)     'BMP(*.BMP)|*.BMP|JPG(*.JPG)|*.JPG|TIF(*.TIF)|*.TIF|すべてのファイル(*.*)|*.*
        //                    //-->v17.50現地修正 by 間々田 2011/03/16
        //                    dialog.Filter = Resources.ResourceManager.GetString("str_" + Convert.ToString(StringTable.IDS_FilerFIMG)) + "|" + Resources._10300;


        //                    //デフォルトの保存拡張子をレジストリから取得
        //                    //System.Reflection.AssemblyTitleAttribute asmtitle = (System.Reflection.AssemblyTitleAttribute)Attribute.GetCustomAttribute(System.Reflection.Assembly.GetExecutingAssembly(), typeof(System.Reflection.AssemblyTitleAttribute));

        //                    // レジストリから読み込む
        //                    //string data = GetRegistry(asmtitle.Title, Description, "DefaultExtension");
        //                    string data = GetRegistry(Application.ProductName, Description, "DefaultExtension");

        //                    //switch (Strings.UCase(Interaction.GetSetting(CT30K.My.MyProject.Application.Info.Title, Description, "DefaultExtension")))
        //                    switch (data.ToUpper())
        //                    {
        //                        //
        //                        case "JPG":
        //                            dialog.Filter = MakeFilterString(new string[] { "JPG", "BMP", "TIF" });
        //                            break;
        //                        case "TIF":
        //                            dialog.Filter = MakeFilterString(new string[] { "TIF", "BMP", "JPG" });
        //                            break;
        //                        default:
        //                            dialog.Filter = MakeFilterString(new string[] { "BMP", "JPG", "TIF" });
        //                            break;
        //                    }

        //                }
        //            }
        //            else if (!string.IsNullOrEmpty(Description))
        //            {
        //                //～(*aaa.bbb)|*aaa.bbb
        //                //dialog.Filter = Description + sStrings.Replace("(*@)|*@", "@", SubExtension + Extension);
        //                dialog.Filter = Description + "(*@)|*@".Replace("@", SubExtension + Extension);
        //            }

        //            //スライスプランテーブルの場合で、コーンビーム・スライスプランの組み合わせが可の場合
        //            if (Description == Resources.ResourceManager.GetString("STR_" + Convert.ToString(StringTable.IDS_SlicePlanTable)) & (CTSettings.scaninh.Data.cone_multiscan_mode[2] == 0))
        //            {

        //                //dialog.Filter = Strings.LCase(_with2.Filter);
        //                dialog.Filter = dialog.Filter.ToLower();

        //                if (Operation == StringTable.IDS_Open)
        //                {
        //                    //if (Strings.LCase(SubExtension) == "-spl")
        //                    if (SubExtension.ToLower() == "-spl")
        //                    {
        //                        dialog.Filter = "スキャン用" + dialog.Filter + "|" + "コーンビーム用" + dialog.Filter.Replace("-spl", "-csp");
        //                    }
        //                    else
        //                    {
        //                        dialog.Filter = "コーンビーム用" + dialog.Filter + "|" + "スキャン用" + dialog.Filter.Replace("-csp", "-spl");
        //                    }
        //                }
        //                else
        //                {
        //                    //if (Strings.LCase(SubExtension) == "-spl")
        //                    if (SubExtension.ToLower() == "-spl")
        //                    {
        //                        dialog.Filter = "スキャン用" + dialog.Filter;
        //                    }
        //                    else
        //                    {
        //                        dialog.Filter = "コーンビーム用" + dialog.Filter;
        //                    }
        //                }
        //                //生データファイルが指定されていて，コーンビーム可の場合
        //            }
        //            else if ((Description == Resources.ResourceManager.GetString("str_" + Convert.ToString(StringTable.IDS_RawFile))) & (CTSettings.scaninh.Data.data_mode[2] == 0))
        //            {
        //                dialog.Filter = dialog.Filter + "|" + Resources.ResourceManager.GetString("str_" + Convert.ToString(StringTable.IDS_ConeRawFile)) + "(*@)|*@".Replace("@", SubExtension + ".cob");
        //            }

        //            // フィルタのインデクス
        //            dialog.FilterIndex = 1;

        //            //フラグ
        //            dialog.ShowReadOnly = false;
        //            if (Operation == StringTable.IDS_Save)
        //            {
        //                //保存ダイアログ用のため設定できない
        //                //既に存在する場合はメッセージボックスを表示　(元の定数：cdlOFNOverwritePrompt)
        //                //dialog.OverwritePrompt = true;
        //            }
        //            else if (Operation != StringTable.IDS_FileSpecify)
        //            {
        //                // 既存のファイルだけを入力できるようにする (元の定数：cdlOFNFileMustExist)
        //                dialog.CheckFileExists = true;
        //                dialog.CheckPathExists = true;
        //            }

        //            //デフォルトのパス
        //            if (!string.IsNullOrEmpty(InitFileName))
        //            {
        //                //dialog.InitialDirectory = FSO.GetParentFolderName(InitFileName);
        //                dialog.InitialDirectory = Path.GetDirectoryName(InitFileName);
        //            }
        //            else
        //            {
        //                dialog.InitialDirectory = GetDefaultFolder(Description);
        //            }


        //            //ダイアログ表示
        //            if ((Operation == StringTable.IDS_Save) | (Operation == StringTable.IDS_FileSpecify))
        //            {
        //                //dialog.SaveDialog();
        //            }
        //            else
        //            {
        //                dialog.ShowDialog();
        //            }

        //            // ダイアログ表示
        //            if (dialog.ShowDialog() != DialogResult.OK)
        //            {
        //                // ＯＫでない場合抜ける
        //                return functionReturnValue;
        //            }


        //            ////エラーの場合、抜ける
        //            //if (Information.Err().Number != 0)
        //            //{
        //            //    //キャンセルボタン選択時以外のエラーの場合、エラーメッセージを表示
        //            //    if (Information.Err().Number != DialogResult.Cancel)
        //            //        MessageBox.Show(Information.Err().Description, MsgBoxStyle.Critical);

        //            //    MessageBox.Show(StringTable.GetResString(9964, Description),
        //            //                    Application.ProductName,
        //            //                    MessageBoxButtons.OK,
        //            //                    MessageBoxIcon.Error);



        //            //    return functionReturnValue;


        //            //}

        //            //ファイル単純指定モード時はそのままファイル名を返す
        //            if (Operation == StringTable.IDS_FileSpecify)
        //            {

        //                //戻り値セット
        //                functionReturnValue = dialog.FileName;

        //                //透視画像の場合はそのままファイル名を返す
        //            }
        //            else if (Description == Resources.ResourceManager.GetString("str_" + Convert.ToString(StringTable.IDS_TransImage)))
        //            {

        //                functionReturnValue = dialog.FileName;

        //                //保存の場合はファイル名を細工する
        //            }
        //            else if (Operation == StringTable.IDS_Save)
        //            {

        //                //戻り値セット
        //                functionReturnValue = Path.Combine(Path.GetDirectoryName(dialog.FileName), modLibrary.AddExtension(Path.GetFileNameWithoutExtension(dialog.FileName), SubExtension) + Extension);

        //                //生データファイルが指定されている場合
        //            }
        //            else if (Description == Resources.ResourceManager.GetString("str_" + Convert.ToString(StringTable.IDS_RawFile)))
        //            {
        //                // 指定外の拡張子が選ばれた場合
        //                string ext = Path.GetExtension(dialog.FileName).ToLower();
        //                if (!((ext == Extension.ToLower()) || ((ext == ".cob") && (CTSettings.scaninh.Data.data_mode[2] == 0))))
        //                {
        //                    // メッセージ表示：%1以外が選択されました。
        //                    MessageBox.Show(StringTable.GetResString(9964, Description),
        //                                    Application.ProductName,
        //                                    MessageBoxButtons.OK,
        //                                    MessageBoxIcon.Error);
        //                    return functionReturnValue;
        //                }

        //                functionReturnValue = dialog.FileName;

        //                //スライスプランテーブルの場合で、コーンビーム・スライスプランの組み合わせが可の場合
        //            }
        //            else if ((Description == Resources.ResourceManager.GetString("str_" + Convert.ToString(StringTable.IDS_SlicePlanTable))) & (CTSettings.scaninh.Data.cone_multiscan_mode[2] == 0))
        //            {

        //                if (Operation == StringTable.IDS_Open)
        //                {
        //                    // 指定外の拡張子が選ばれた場合
        //                    if (!(Regex.IsMatch(dialog.FileName.ToLower(), "-spl.csv$") || Regex.IsMatch(dialog.FileName.ToLower(), "-csp.csv$")))
        //                    {
        //                        // メッセージ表示：%1以外が選択されました。
        //                        MessageBox.Show(StringTable.GetResString(9964, Description),
        //                                        Application.ProductName,
        //                                        MessageBoxButtons.OK,
        //                                        MessageBoxIcon.Error);
        //                        return functionReturnValue;
        //                    }

        //                }
        //                else
        //                {
        //                    //指定外の拡張子が選ばれた場合
        //                    if (!Regex.IsMatch(dialog.FileName.ToLower(), SubExtension.ToLower() + Extension.ToLower() + "$"))
        //                    {
        //                        //メッセージ表示：%1以外が選択されました。
        //                        MessageBox.Show(StringTable.GetResString(9964, modLibrary.GetFirstItem(dialog.Filter, "(")),
        //                                        Application.ProductName,
        //                                        MessageBoxButtons.OK,
        //                                        MessageBoxIcon.Error);

        //                        return functionReturnValue;
        //                    }
        //                }

        //                functionReturnValue = dialog.FileName;

        //                //上記以外
        //            }
        //            else
        //            {

        //                //指定外の拡張子が選ばれた場合
        //                //if (!(Strings.UCase(dialog.FileName) ))
        //                if (!Regex.IsMatch(dialog.FileName.ToLower(), SubExtension.ToLower() + Extension.ToLower() + "$"))
        //                {

        //                    if (string.IsNullOrEmpty(Description))
        //                    {
        //                        // メッセージ表示：
        //                        MessageBox.Show("ファイルの拡張子が指定外です。",
        //                                        Application.ProductName,
        //                                        MessageBoxButtons.OK,
        //                                        MessageBoxIcon.Error);

        //                    }
        //                    else
        //                    {
        //                        //メッセージ表示：%1以外が選択されました。
        //                        MessageBox.Show(StringTable.GetResString(9964, Description),
        //                                        Application.ProductName,
        //                                        MessageBoxButtons.OK,
        //                                        MessageBoxIcon.Error);

        //                    }
        //                    return functionReturnValue;

        //                    //画像ファイルの場合
        //                }
        //                else if (Extension.ToLower() == ".IMG")
        //                {

        //                    //付帯情報ファイルは存在するか？
        //                    if (!File.Exists(ChangeExtension(dialog.FileName, ".inf")))
        //                    {
        //                        //メッセージ表示：付帯情報ファイルが見つかりません。
        //                        MessageBox.Show(StringTable.BuildResStr((int)StringTable.IDS_NotFound, (int)StringTable.IDS_InfoFile),
        //                        Application.ProductName,
        //                        MessageBoxButtons.OK,
        //                        MessageBoxIcon.Error);


        //                        return functionReturnValue;
        //                    }

        //                }

        //                //戻り値セット
        //                functionReturnValue = dialog.FileName;

        //            }

        //            //選択したパスをデフォルトパスとして記憶
        //            if (!string.IsNullOrEmpty(Description))
        //                //SaveDefaultFolder(Description, FSO.GetParentFolderName(dialog.FileName));
        //                SaveDefaultFolder(Description, Path.GetDirectoryName(dialog.FileName));


        //        }
        //        catch (System.Exception ex)
        //        {
        //            // ダイアログ表示
        //            if ((dialog.ShowDialog() != DialogResult.Cancel) && (dialog.ShowDialog() != DialogResult.Abort))
        //            {
        //                //メッセージ表示
        //                MessageBox.Show(ex.Message,
        //                Application.ProductName,
        //                MessageBoxButtons.OK,
        //                MessageBoxIcon.Error);
        //            }
        //            throw;
        //        }
        //    }

        //    return functionReturnValue;
        //}
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="Operation"></param>
        ///// <param name="Description"></param>
        ///// <param name="Extension"></param>
        ///// <param name="SubExtension"></param>
        ///// <param name="InitFileName"></param>
        ///// <param name="Purpose"></param>
        ///// <returns></returns>
        //public static string GetFileName(string Description, string Extension, string SubExtension, string InitFileName)
        //{
        //    // デフォルト値
        //    // Operation : IDS_Open 
        //    // Description : ""
        //    // Extension : ".csv"
        //    // SubExtension : ""
        //    // InitFileName : ""
        //    // Purpose : ""

        //    string functionReturnValue = null;

        //    //戻り値初期化
        //    functionReturnValue = "";

        //    // ファイル選択ダイアログ生成
        //    using (var dialog = new OpenFileDialog())
        //    {
        //        // ダイアログタイトル
        //        if (string.IsNullOrEmpty(Description))
        //        {
        //            dialog.Title = "";
        //        } 
        //        else
        //        {
        //            dialog.Title = StringTable.GetResString((int)StringTable.IDS_Open, Description);
        //        } 

        //        // デフォルトファイル名
        //        dialog.FileName = "";

        //        // デフォルトのフィルタ：すべてのファイル(*.*)|*.*
        //        dialog.Filter = Resources._10300;

        //        // 透視画像の場合
        //        if (Description == Resources.ResourceManager.GetString("str_" + Convert.ToString((int)StringTable.IDS_TransImage)))
        //        {
        //            // 種類を切り替えなくても表示されるようにした
        //            dialog.Filter = Resources.ResourceManager.GetString("str_" + Convert.ToString((int)StringTable.IDS_TransImage)) + "(*.BMP/*.JPG/*.TIF)|*.BMP;*.JPG;*.TIF|すべてのファイル(*.*)|*.*";
        //        }
        //        else if (!string.IsNullOrEmpty(Description))
        //        {
        //            dialog.Filter = Description + "(*@)|*@".Replace("@", SubExtension + Extension);//～(*aaa.bbb)|*aaa.bbb
        //        }

        //        //スライスプランテーブルの場合で、コーンビーム・スライスプランの組み合わせが可の場合
        //        if (Description == Resources.ResourceManager.GetString("str_" + Convert.ToString((int)StringTable.IDS_SlicePlanTable)) & (CTSettings.scaninh.Data.cone_multiscan_mode[2] == 0))
        //        {
        //            dialog.Filter = dialog.Filter.ToLower();
        //            if (SubExtension.ToLower() == "-spl")
        //            {
        //                dialog.Filter = "スキャン用" + dialog.Filter + "|" + "コーンビーム用" + dialog.Filter.Replace("-spl", "-csp");
        //            }
        //            else
        //            {
        //                dialog.Filter = "コーンビーム用" + dialog.Filter + "|" + "スキャン用" + dialog.Filter.Replace("-csp", "-spl");
        //            }
        //        }
        //        else if ((Description == Resources.ResourceManager.GetString("str_" + Convert.ToString((int)StringTable.IDS_RawFile))) & (CTSettings.scaninh.Data.data_mode[2] == 0))
        //        {
        //            dialog.Filter = dialog.Filter + "|" + Resources.ResourceManager.GetString("str_" + Convert.ToString((int)StringTable.IDS_ConeRawFile)) + "(*@)|*@".Replace("@", SubExtension + ".cob");
        //        }

        //        // フィルタのインデクス
        //        dialog.FilterIndex = 1;

        //        // フラグ
        //        dialog.ShowReadOnly = false;

        //        // 既存のファイルだけを入力できるようにする
        //        dialog.CheckFileExists = true;
        //        dialog.CheckPathExists = true;

        //        //デフォルトのパス
        //        if (!string.IsNullOrEmpty(InitFileName))
        //        {
        //            dialog.InitialDirectory = Path.GetDirectoryName(InitFileName);
        //        }
        //        else
        //        {
        //            dialog.InitialDirectory = GetDefaultFolder(Description);
        //        }

        //        // ダイアログ表示
        //        if (dialog.ShowDialog() != DialogResult.OK)
        //        {
        //            // ＯＫでない場合抜ける
        //            return functionReturnValue;
        //        }

        //        // 透視画像の場合はそのままファイル名を返す
        //        if (Description == Resources.ResourceManager.GetString("str_" + Convert.ToString((int)StringTable.IDS_TransImage)))
        //        {
        //            functionReturnValue = dialog.FileName;
        //        }
        //        // 生データファイルが指定されている場合 
        //        else if (Description == Resources.ResourceManager.GetString("str_" + Convert.ToString((int)StringTable.IDS_RawFile)))
        //        {
        //            // 指定外の拡張子が選ばれた場合
        //            string ext = Path.GetExtension(dialog.FileName).ToLower();
        //            if (!((ext == Extension.ToLower()) || ((ext == ".cob") && (CTSettings.scaninh.Data.data_mode[2] == 0))))
        //            {
        //                // メッセージ表示：%1以外が選択されました。
        //                MessageBox.Show(StringTable.GetResString(9964, Description),
        //                                Application.ProductName,
        //                                MessageBoxButtons.OK,
        //                                MessageBoxIcon.Error);
        //                return functionReturnValue;
        //            }

        //            functionReturnValue = dialog.FileName;
        //        }
        //        //スライスプランテーブルの場合で、コーンビーム・スライスプランの組み合わせが可の場合
        //        else if ((Description == Resources.ResourceManager.GetString("str_" + Convert.ToString((int)StringTable.IDS_SlicePlanTable))) & (CTSettings.scaninh.Data.cone_multiscan_mode[2] == 0))
        //        {
        //            // 指定外の拡張子が選ばれた場合
        //            if (!(Regex.IsMatch(dialog.FileName.ToLower(), "-spl.csv$") || Regex.IsMatch(dialog.FileName.ToLower(), "-csp.csv$")))
        //            {
        //                // メッセージ表示：%1以外が選択されました。
        //                MessageBox.Show(StringTable.GetResString(9964, Description),
        //                                Application.ProductName,
        //                                MessageBoxButtons.OK,
        //                                MessageBoxIcon.Error);
        //                return functionReturnValue;
        //            }

        //            functionReturnValue = dialog.FileName;
        //        }
        //        // 上記以外
        //        else
        //        {
        //            // 指定外の拡張子が選ばれた場合
        //            if (!Regex.IsMatch(dialog.FileName.ToLower(), SubExtension.ToLower() + Extension.ToLower() + "$"))
        //            {
        //                if (string.IsNullOrEmpty(Description))
        //                {
        //                    // メッセージ表示：
        //                    MessageBox.Show("ファイルの拡張子が指定外です。",
        //                                    Application.ProductName,
        //                                    MessageBoxButtons.OK,
        //                                    MessageBoxIcon.Error);
        //                }
        //                else
        //                {
        //                    //メッセージ表示：%1以外が選択されました。
        //                    MessageBox.Show(StringTable.GetResString(9964, Description),
        //                                    Application.ProductName,
        //                                    MessageBoxButtons.OK,
        //                                    MessageBoxIcon.Error);
        //                }
        //                return functionReturnValue;

        //            }
        //            // 画像ファイルの場合
        //            else if (Extension.ToUpper() == ".IMG")
        //            {
        //                //付帯情報ファイルは存在するか？
        //                if (!File.Exists(ChangeExtension(dialog.FileName, ".inf")))
        //                {
        //                    //メッセージ表示：付帯情報ファイルが見つかりません。
        //                    //MessageBox.Show(StringTable.BuildResString((int)StringTable.StringTable.IDS_NotFound, (int)StringTable.StringTable.IDS_InfoFile),
        //                    //                Application.ProductName,
        //                    //                MessageBoxButtons.OK,
        //                    //                MessageBoxIcon.Error);
        //                    //MessageBox.Show(StringTable.GetResString(StringTable.IDS_NotFound, StringTable.IDS_InfoFile),
        //                    MessageBox.Show(StringTable.BuildResStr((int)StringTable.IDS_NotFound, (int)StringTable.IDS_InfoFile),
        //                                    Application.ProductName,
        //                                    MessageBoxButtons.OK,
        //                                    MessageBoxIcon.Error);
        //                    return functionReturnValue;
        //                }
        //            }

        //            // 戻り値セット
        //            functionReturnValue = dialog.FileName;
        //        }

        //        //選択したパスをデフォルトパスとして記憶
        //        if (!string.IsNullOrEmpty(Description))
        //            SaveDefaultFolder(Description, Path.GetDirectoryName(dialog.FileName));
        //    }

        //    return functionReturnValue;
        //}

        #endregion


        #region パス文字列の拡張子を変更します
        /// <summary>
        /// パス文字列の拡張子を変更します
        /// </summary>
        /// <param name="path">変更するパス情報</param>
        /// <param name="extension">新しい拡張子 (先行ピリオド付き)</param>
        /// <returns>変更されたパス情報を含む文字列</returns>
        public static string ChangeExtension(string path, string extension)
        {
            return Path.ChangeExtension(path, extension);
        }
        #endregion

        #region ファイル名から拡張子・ズーミング連番・スキャノ連番・スライス連番コーンビーム連番を取り除いた文字列を返す 
        /// <summary>
        /// ファイル名から拡張子・ズーミング連番・スキャノ連番・スライス連番コーンビーム連番を取り除いた文字列を返す
        /// </summary>
        /// <param name="fileName">処理対象ファイル名</param>
        /// <returns>処理結果スライスベース名</returns>
        public static string GetSliceBaseName(string fileName)
        {
            // ベース名取得
            string result = Path.GetFileNameWithoutExtension(fileName);

            // ズーミング連番・スキャノ連番を取り除く
            //if (Regex.IsMatch(result.ToLower(), @".+-s\d{3}$") || (Regex.IsMatch(result.ToLower(), @".+-z\d{3}$")))
            if (Regex.IsMatch(result, @"-[sS]\d{3}$") || Regex.IsMatch(result, @"-[zZ]\d{3}$"))
            {
                result = result.Remove(result.Length - 5);
            }

            // スライス連番４桁タイプ
            //if (Regex.IsMatch(result.ToLower(), @".+-\d{4}$"))
            if (Regex.IsMatch(result, @"-\d{4}$"))
            {
                result = result.Remove(result.Length - 5);
            }

            // スライス連番・コーンビーム連番３桁タイプ
            //if (Regex.IsMatch(result.ToLower(), @".+-\d{3}$"))
            if (Regex.IsMatch(result, @"-\d{3}$"))
            {
                result = result.Remove(result.Length - 4);
            }

            // 戻り値セット
            return result;
        }
        #endregion

        #region フォルダ選択ダイアログを利用してファイル名を取得する
        /// <summary>
        /// フォルダ選択ダイアログを利用してファイル名を取得する
        /// </summary>
        /// <param name="description"></param>
        /// <param name="extension"></param>
        /// <param name="defaultPath"></param>
        /// <returns></returns>
        public static string[] GetFileListFromFolderDialog(string description, string extension, string defaultPath)
        {
	        // フォルダ参照ダイアログ表示
	        if (string.IsNullOrEmpty(description))
		        //description = Resources.STR_CTImage;
                description = Convert.ToString((int)StringTable.IDS_CTImage);

            string folderName = GetFolderName((string.IsNullOrEmpty(defaultPath) ? GetDefaultFolder(description) : defaultPath), false);

	        // キャンセル時
	        if (string.IsNullOrEmpty(folderName))
		        return null;

	        // デフォルトパスを記憶
	        SaveDefaultFolder(description, folderName);

            // ファイル名リスト取得
            string[] files = Directory.GetFiles(folderName, "*" + extension);

            return files;
        }        
        #endregion

        #region ファイル選択ダイアログを利用してファイル名を取得する
        /// <summary>
        /// ファイル選択ダイアログを利用してファイル名を取得する
        /// </summary>
        /// <param name="description"></param>
        /// <param name="extension"></param>
        /// <param name="defaultPath"></param>
        /// <returns></returns>
        public static string[] GetFileListFromFileDialog(string description = "", string extension= ".img", string defaultPath= "")
	    {
		    // 戻り値用変数
		    string[] result = null;

            // ファイル選択ダイアログ生成
            using (var dialog = new OpenFileDialog())
            {
                //変更2014/11/18hata_MessageBox確認　try,catchを追加
                try
                {
                    dialog.Title = (string.IsNullOrEmpty(description) ? "" : StringTable.GetResString((int)StringTable.IDS_Select, description));
                    dialog.FilterIndex = 1;
                    dialog.FileName = "";

                    // フィルタ
                    dialog.Filter = CTResources.LoadResString(10300);     //すべてのファイル(*.*)|*.*
                    if (!string.IsNullOrEmpty(description))
                    {
                        // ～(*aaa.bbb)|*aaa.bbb
                        dialog.Filter = description + "(*@)|*@".Replace("@", extension) + "|" + dialog.Filter;
                    }

                    // 読み取り専用チェックボックスは表示しない
                    dialog.ShowReadOnly = false;

                    // 既存のファイルだけを入力できるようにする
                    dialog.CheckFileExists = true;
                    dialog.CheckPathExists = true;

                    // 複数選択可能
                    dialog.Multiselect = true;

                    // デフォルトのパス
                    if (string.IsNullOrEmpty(description))
                        //description = Resources.STR_CTImage;
                        description = Convert.ToString((int)StringTable.IDS_CTImage);

                    dialog.InitialDirectory = (string.IsNullOrEmpty(defaultPath) ? GetDefaultFolder(description) : defaultPath);

                    // ダイアログ表示
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        // 選択ファイルを取得
                        result = dialog.FileNames;

                        // ソート
                        Array.Sort(result);

                        // デフォルトパスを記憶
                        SaveDefaultFolder(description, Path.GetDirectoryName(dialog.FileName));
                    }
                }
                catch (Exception ex)
                {
                    //エラーの場合、抜ける
                    //キャンセルボタン選択時以外のエラーの場合、エラーメッセージを表示
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
		    // 戻り値セット
            return result;
	    }
        #endregion

        #region 特定の作業のデフォルトフォルダを保存する
        /// <summary>
        /// 特定の作業のデフォルトフォルダを保存する
        /// </summary>
        /// <param name="section"></param>
        /// <param name="folder"></param>
        public static void SaveDefaultFolder(string section, string folder)
        {
            // 指定したフォルダが存在しない場合は、何もしない
            if (!Directory.Exists(folder))
                return;

            // 生データファイル・コーンビームＣＴ用生データファイルは画像ファイルのパスと同じとする
            //if (section == Resources.ResourceManager.GetString("str_" + Convert.ToString((int)StringTable.IDS_RawFile)) ||
            //    section == Resources.ResourceManager.GetString("str_" + Convert.ToString((int)StringTable.IDS_ConeRawFile)))
            //Rev21.00 修正 by長野 2015/03/13
            // 生データファイル・コーンビームＣＴ用生データファイルは画像ファイルのパスと同じとする
            if (section == Resources.ResourceManager.GetString("_" + Convert.ToString((int)StringTable.IDS_RawFile)) ||
                section == Resources.ResourceManager.GetString("_" + Convert.ToString((int)StringTable.IDS_ConeRawFile)))

                //section = Resources.STR_CTImage;
                //Rev21.00 修正 by長野 2015/03/13
                //section = Convert.ToString((int)StringTable.IDS_CTImage);
                section = Resources.ResourceManager.GetString("_" + Convert.ToString((int)StringTable.IDS_CTImage));

            // スキャン条件ファイルもとりあえずは画像ファイルのパスと同じとする
            //else if (section == Resources.ResourceManager.GetString("str_" + Convert.ToString((int)StringTable.IDS_CondFile)))
            //Rev21.00 修正 by長野 2015/03/13
            else if (section == Resources.ResourceManager.GetString("_" + Convert.ToString((int)StringTable.IDS_CondFile)))
                //Rev21.00 修正 by長野 2015/03/13
                //section = Convert.ToString((int)StringTable.IDS_CTImage);
                section = Resources.ResourceManager.GetString("_" + Convert.ToString((int)StringTable.IDS_CTImage));

            ////Interaction.SaveSetting(Application.ProductName, section, "DefaultFolder", folder);
            //SaveRegistry(Application.ProductName, section, "DefaultFolder", folder);
            SaveRegistry(Path.Combine(RegistryKeyName, Application.ProductName), section, "DefaultFolder", folder);

        }
        #endregion

        #region 特定の作業のデフォルトフォルダを読み込む
        /// <summary>
        /// 特定の作業のデフォルトフォルダを読み込む
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public static string GetDefaultFolder(string section)
        {
            // 生データファイル・コーンビームＣＴ用生データファイルは画像ファイルのパスと同じとする
            //if (section == Resources.ResourceManager.GetString("str_" + Convert.ToString((int)StringTable.IDS_RawFile)) ||
            //    section == Resources.ResourceManager.GetString("str_" + Convert.ToString((int)StringTable.IDS_ConeRawFile)))
            //Rev21.00 修正 by長野 2015/03/13
            if (section == Resources.ResourceManager.GetString("_" + Convert.ToString((int)StringTable.IDS_RawFile)) ||
                section == Resources.ResourceManager.GetString("_" + Convert.ToString((int)StringTable.IDS_ConeRawFile)))

                //section = Resources.STR_CTImage;
                //Rev21.00 修正 by長野 2015/03/13
                //section = Convert.ToString((int)StringTable.IDS_CTImage);
                section = Resources.ResourceManager.GetString("_" + Convert.ToString((int)StringTable.IDS_CTImage));

            // スキャン条件ファイルもとりあえずは画像ファイルのパスと同じとする
            //else if (section == Resources.ResourceManager.GetString("str_" + Convert.ToString((int)StringTable.IDS_CondFile)))
            //Rev21.00 修正 by長野 2015/03/13
            else if (section == Resources.ResourceManager.GetString("_" + Convert.ToString((int)StringTable.IDS_CondFile)))
                //section = Resources.STR_CTImage;
                //Rev21.00 修正 by長野 2015/03/13
                //section = Convert.ToString((int)StringTable.IDS_CTImage);
                section = Resources.ResourceManager.GetString("_" + Convert.ToString((int)StringTable.IDS_CTImage));


            // レジストリから読む
            //string folder = Interaction.GetSetting(Application.ProductName, section, "DefaultFolder", "");
            string folder = GetRegistry(Path.Combine(RegistryKeyName, Application.ProductName), section, "DefaultFolder");


            // デフォルトのフォルダ名を得られなかった場合、もしくは存在しない場合
            // アプリケーションが定めたフォルダ名に設定する
            if (!Directory.Exists(folder))
            {
                // ズーミングテーブル
                if (section == Resources.ResourceManager.GetString("str_" + Convert.ToString((int)StringTable.IDS_ZoomingTable)))

                    // C:\CTUSER\ｽﾞｰﾐﾝｸﾞﾃｰﾌﾞﾙ
                    folder = Path.Combine(AppValue.CTUSER, CTResources.LoadResString(10202));

                //スライスプランテーブル
                else if (section == Resources.ResourceManager.GetString("str_" + Convert.ToString((int)StringTable.IDS_SlicePlanTable)))

                    // C:\CTUSER\ｽﾗｲｽﾌﾟﾗﾝﾃｰﾌﾞﾙ
                    folder = Path.Combine(AppValue.CTUSER, CTResources.LoadResString(10203));

                // プロフィールディスタンステーブル
                else if (section == Resources.ResourceManager.GetString("str_" + Convert.ToString((int)StringTable.IDS_PDTable)))

                    // C:\CTUSER\ﾌﾟﾛﾌｨｰﾙﾃﾞｨｽﾀﾝｽﾃｰﾌﾞﾙ
                    folder = Path.Combine(AppValue.CTUSER, CTResources.LoadResString(10204));

                // 上記以外
                else

                    // C:\CTUSER\画像
                    folder = Path.Combine(AppValue.CTUSER, CTResources.LoadResString(10201));
            }

            return folder;
        }
        #endregion


        //*************************************************************************************************
        //機　　能： ファイル名選択時に使用するフィルタ文字列を作成する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.50  2011/03/16 (豊川)間々田 新規作成
        //*************************************************************************************************
        public static string MakeFilterString(params string[] p)
        {
            const string BaseString = "@(*.@)|*.@";

            string[] Cell = null;
            Cell = new string[p.GetUpperBound(0) + 1];

            for (int i = 0; i <= Cell.GetUpperBound(0); i++)
            {
                Cell[i] = BaseString.Replace("@", p[i]);
            }

            return string.Join("|", Cell);
        }

        //*************************************************************************************************
        //v19.00 追加 ->(電S2)永井
        //
        //   フルパスからパス名とファイル名を取得する                新規作成 by 間々田 2003/11/14
        //
        //   注）
        //       パス名    ：末尾に\が付くタイプ
        //       ファイル名：拡張子を取り除いたタイプ
        //
        //*************************************************************************************************
        public static void GetPathAndFileName(string FullFileName, ref string PathName, ref string FileName)
        {

            int thePos = 0;

            //thePos = Strings.InStrRev(FullFileName, "\\");
            thePos = FullFileName.LastIndexOf("\\");

            //パス名
            //PathName = Strings.Left(FullFileName, thePos);
            PathName = FullFileName.Substring(0, thePos);

            //ファイル名（拡張子が付いているかもしれない）
            //FileName = Strings.Mid(FullFileName, thePos + 1);
            FileName = FullFileName.Substring(thePos + 1);

            //拡張子があれば取り除く
            //thePos = Strings.InStrRev(FileName, ".");
            thePos = FileName.LastIndexOf(".");
            
            
            if (thePos > 0)
                //FileName = Strings.Left(FileName, thePos - 1);
                //FileName = FileName.Substring(0, thePos - 1);
                //Rev20.00 -1はいらない by長野 2014/12/15
                FileName = FileName.Substring(0, thePos);
        }

        //*************************************************************************************************
        //ファイルパスから拡張子を取り除いて返す
        //*************************************************************************************************
        public static string RemoveExtensionEx(string FullFileName)
        {

            //return FSO.BuildPath(FSO.GetParentFolderName(FullFileName), FSO.GetBaseName(FullFileName));
            return Path.Combine(Path.GetDirectoryName(FullFileName), Path.GetFileNameWithoutExtension(FullFileName));

        }

        //<- v19.00


        // レジストリから読み込む
        // 読込み先：Titleでパスを指定する
        public static string GetRegistry(string Title, string Section, string Param, string defultStr = "")
        {
            // レジストリから読み込む
            RegistryKey key = Registry.CurrentUser;
            string path = Path.Combine(Title, Section);
            key = key.OpenSubKey(path);
            string data = defultStr;
            if (key != null)
            {
                data = (string)key.GetValue(Param);
                key.Close();
            }
            return data;
        }

        // レジストリへ書き込む
        // 書き込み先：Titleでパスを指定する　
        public static void SaveRegistry(string Title, string Section, string Param, string Val)
        {
            RegistryKey key = Registry.CurrentUser;
            string path = Path.Combine(Title, Section);
            key = key.CreateSubKey(path);

            if (key == null) return;
            key.SetValue(Param, Val);
            key.Close();

        }


    }
}