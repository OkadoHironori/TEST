using System;
using System.IO;

namespace XrayCtrl
{
	internal static class modIniFiles
	{
		//******************************************************************************
		//
		//   INIファイル制御
		//
		//   各INIファイルの読み込みを行う
		//
		//   Date        Version     Designed/Changed
		//   2003/05/28  1.00        (NSD)Shibui
		//
		//******************************************************************************

		//------------------------------------------------------------------------------
		//   定数宣言
		//------------------------------------------------------------------------------

		//----- 設定ファイルパス名 -----
		//Public Const PATH_INI_FILES                         As String = "C:\WinNT\System32\TMDATA\TMSA\"    //INIファイル格納フォルダ
		public const string PATH_INI_FILES = @"C:\CT\Ini\";				//INIファイル格納フォルダ
		public const string FNAME_TSB_001_L9181 = "Hama_L9181.ini";		//浜松ホトニクス製 密閉型 130kVパラメータ
		public const string FNAME_TSB_001_L9191 = "Hama_L9191.ini";		//浜松ホトニクス製 開放型 160kVパラメータ
		public const string FNAME_TSB_001_L9421 = "Hama_L9421.ini";		//浜松ホトニクス製 密閉型 90kVパラメータ
		//追加2009/08/19(KSS)hata_L10801対応 ---------->
		public const string FNAME_TSB_001_L10801 = "Hama_L10801.ini";	//浜松ホトニクス製 開放型 230kVパラメータ
		//追加2009/08/19(KSS)hata_L10801対応 ----------<
		//追加2010/02/19(KSS)hata_L8601対応 ---------->
		public const string FNAME_TSB_001_L8601 = "Hama_L8601.ini";		//浜松ホトニクス製 密閉分離型 90kVパラメータ
		//追加2010/02/19(KSS)hata_L8601対応 ----------<
		//追加2012/03/20(KS1)hata_L88121-02対応 ------>
		public const string FNAME_TSB_001_L8121_02 = "Hama_L8121.ini";	//浜松ホトニクス製 密閉分離型 90kVパラメータ
		//追加2012/03/20(KS1)hata_L88121-02対応 ------<

        public const string FNAME_TSB_001_L12721 = "Hama_L12721.ini";	//浜松ホトニクス製 開放型 300kVパラメータ
        public const string FNAME_TSB_001_L10711 = "Hama_L10711.ini";	//浜松ホトニクス製 密閉分離型 160kVパラメータ
        

		//----- パラメータデータ用 -----
		public const long MIN_SPD				= 0;			// 0.001 -> 0;
		public const long MAX_SPD				= 999;			// 999.999 -> 999;
		public const long MIN_ORG_NO			= 1;
		public const long MAX_ORG_NO			= 15;
		public const long MIN_RATE				= 1;
		public const long MAX_RATE				= 99;
		public const long MIN_PPS				= -9999;		// -9999.9999 -> -9999;
		public const long MAX_PPS				= 9999;			// 9999.9999 -> 9999;
		public const long MIN_SPINIT			= 0x0;
		public const long MAX_SPINIT			= 0xEF;
		public const float MIN_ZOOM				= 0;
		public const float MAX_ZOOM				= 999.9f;
		public const float MIN_FIELD			= 0;
		public const float MAX_FIELD			= 99.999f;
		public const float MIN_PICTURE			= 0;
		public const float MAX_PICTURE			= 9999;
		public const float MIN_INTERFERENCE		= -999.999f;
		public const float MAX_INTERFERENCE		= 999.999f;
		public const float MIN_STEP				= -999.999f;
		public const float MAX_STEP				= 999.999f;
		public const float MIN_POSITION			= -999.999f;
		public const float MAX_POSITION			= 999.999f;
		public const float MIN_KV				= 0;
		public const float MAX_KV				= 999;
		public const float MIN_UA				= 0;
		public const float MAX_UA				= 999;
		public const float MAX_UA_4				= 9999;        //追加2009/09/02hata L10801対応
		public const float MIN_SEC				= 0;
		public const float MAX_SEC				= 999;

		//----- X線管タイプ -----
		//Public Const XRAY_TYPE_90KV         As String = "L9421"     //90kV
		//Public Const XRAY_TYPE_130KV        As String = "L9181"     //130kV
		//Public Const XRAY_TYPE_160KV        As String = "L9191"     //160kV
        
        //追加2014/10/07hata
        public const int XRAY_TYPE_NO_90KV = 0;     //90kV
        public const int XRAY_TYPE_NO_130KV = 1;    //130kV
        public const int XRAY_TYPE_NO_160KV = 2;    //160kV
        public const int XRAY_TYPE_NO_150KV = 3;    //150kV
        public const int XRAY_TYPE_NO_230KV = 4;    //230kV
        public const int XRAY_TYPE_NO_300KV = 5;    //300kV //Rev23.10 追加 by長野 2015/10/03
        public const int XRAY_TYPE_NO_160KV_2 = 6;    //160kV(nano) //Rev23.10 追加 by長野 2015/10/03

		//----- センサタイプ -----
		//Public Const SENS_TYPE_T40          As String = "T40"       //東芝製I.I.40万画素タイプ
		//Public Const SENS_TYPE_T100         As String = "T100"      //東芝製I.I.100万画素タイプ
		//Public Const SENS_TYPE_C7921        As String = "C7921"     //浜ホト製FPD
		//Public Const SENS_TYPE_C7942        As String = "C7942"     //浜ホト製FPD
		//Public Const SENS_TYPE_C7943        As String = "C7943"     //浜ホト製FPD

		//----- 戻り値 -----
		public const int FILE_OK		= 0;  //正常終了
		public const int FILE_ERR_OPEN	= 1;  //オープン異常（ファイルが見つからない）
		public const int FILE_ERR_READ	= 2;  //ファイル読み込み異常

		//==============================================================================
		//
		//   浜松ホトニクス製 密閉型 １３０ｋＶパラメータ読み込み
		//
		//==============================================================================
		public static int ReadTSB_001_L9181()
		{
			string strFileName;
			string strSect;
		    
			try
			{
				strFileName = PATH_INI_FILES + FNAME_TSB_001_L9181;
				strSect = "Xray";
			    
				if (!File.Exists(strFileName))	//指定ファイル検索
				{
					return FILE_ERR_OPEN;
				}
		        
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				With gudtXraySystemValue
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                    if (modTools.CIntChk(out modData.portNo, modTools.INIRead(strSect, "portNo", "0", strFileName), (int)0, (int)10) != 0) return FILE_ERR_READ; //Rev23.10 ポート番号追加 by長野 2015/09/29
                	if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMaxkV, modTools.INIRead(strSect, "XrayScaleMaxkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMaxuA, modTools.INIRead(strSect, "XrayScaleMaxuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMinkV, modTools.INIRead(strSect, "XrayScaleMinkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMinuA, modTools.INIRead(strSect, "XrayScaleMinuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMaxkV, modTools.INIRead(strSect, "XraySetMaxkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMaxuA, modTools.INIRead(strSect, "XraySetMaxuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMinkV, modTools.INIRead(strSect, "XraySetMinkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMinuA, modTools.INIRead(strSect, "XraySetMinuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;;
			        
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF1MaxPower, modTools.INIRead(strSect, "XrayF1MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF2MaxPower, modTools.INIRead(strSect, "XrayF2MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF3MaxPower, modTools.INIRead(strSect, "XrayF3MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF4MaxPower, modTools.INIRead(strSect, "XrayF4MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayFocusNumber, modTools.INIRead(strSect, "XrayFocusNumber", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;;
			        
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailkV, modTools.INIRead(strSect, "XrayAvailkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailuA, modTools.INIRead(strSect, "XrayAvailuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailTimeInside, modTools.INIRead(strSect, "XrayAvailTimeInside", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailTimeOn, modTools.INIRead(strSect, "XrayAvailTimeOn", "0", strFileName), (int)MIN_SEC,(int) MAX_SEC) != 0) return FILE_ERR_READ;;
			        
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmupTime, modTools.INIRead(strSect, "XrayWarmupTime", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup1Time, modTools.INIRead(strSect, "XrayWarmup1Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup2Time, modTools.INIRead(strSect, "XrayWarmup2Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup3Time, modTools.INIRead(strSect, "XrayWarmup3Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;;
			        
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayVacuumInf, modTools.INIRead(strSect, "XrayVacuumSts", "0", strFileName), 0, 1) != 0) return FILE_ERR_READ;; //追加2010/05/20(KS1)hata_L9421-02対応

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				End With
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				return FILE_OK;
			}
			catch
			{
				//ファイル読み込み異常
				return FILE_ERR_READ;
			}
		}

		//==============================================================================
		//
		//   浜松ホトニクス製 開放型 １６０ｋＶパラメータ読み込み
		//
		//==============================================================================
		public static int ReadTSB_001_L9191()
		{
			string strFileName;
			string strSect;
		    
			try
			{
		   		strFileName = PATH_INI_FILES + FNAME_TSB_001_L9191;
				strSect = "Xray";
			    
				if (!File.Exists(strFileName))	//指定ファイル検索
				{
					return FILE_ERR_OPEN;
				}
			        
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				With gudtXraySystemValue
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                    if (modTools.CIntChk(out modData.portNo, modTools.INIRead(strSect, "portNo", "0", strFileName), (int)0, (int)10) != 0) return FILE_ERR_READ; //Rev23.10 ポート番号追加 by長野 2015/09/29
                	if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMaxkV, modTools.INIRead(strSect, "XrayScaleMaxkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMaxuA, modTools.INIRead(strSect, "XrayScaleMaxuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMinkV, modTools.INIRead(strSect, "XrayScaleMinkV", "0", strFileName), (int)MIN_KV,(int) MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMinuA, modTools.INIRead(strSect, "XrayScaleMinuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMaxkV, modTools.INIRead(strSect, "XraySetMaxkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMaxuA, modTools.INIRead(strSect, "XraySetMaxuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMinkV, modTools.INIRead(strSect, "XraySetMinkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMinuA, modTools.INIRead(strSect, "XraySetMinuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayMaxPower, modTools.INIRead(strSect, "XrayMaxPower", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF1MaxPower, modTools.INIRead(strSect, "XrayF1MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF2MaxPower, modTools.INIRead(strSect, "XrayF2MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF3MaxPower, modTools.INIRead(strSect, "XrayF3MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF4MaxPower, modTools.INIRead(strSect, "XrayF4MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayFocusNumber, modTools.INIRead(strSect, "XrayFocusNumber", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailkV, modTools.INIRead(strSect, "XrayAvailkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailuA, modTools.INIRead(strSect, "XrayAvailuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailTimeInside, modTools.INIRead(strSect, "XrayAvailTimeInside", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailTimeOn, modTools.INIRead(strSect, "XrayAvailTimeOn", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
			        
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmupTime, modTools.INIRead(strSect, "XrayWarmupTime", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup1Time, modTools.INIRead(strSect, "XrayWarmup1Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup2Time, modTools.INIRead(strSect, "XrayWarmup2Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup3Time, modTools.INIRead(strSect, "XrayWarmup3Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
			        
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayTargetInfSTG, modTools.INIRead(strSect, "XrayMaxPower", "0", strFileName), 0, 1) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayVacuumInf, modTools.INIRead(strSect, "XrayVacuumSts", "0", strFileName), 0, 1) != 0) return FILE_ERR_READ;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				End With
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				return FILE_OK;
			}
			catch
			{    
				//ファイル読み込み異常
				return FILE_ERR_READ;
			}		    
		}

		//==============================================================================
		//
		//   浜松ホトニクス製 開放型 ９０ｋＶパラメータ読み込み
		//
		//==============================================================================
		public static int ReadTSB_001_L9421()
		{
			string strFileName;
			string strSect;

			try
			{
				strFileName = PATH_INI_FILES + FNAME_TSB_001_L9421;
				strSect = "Xray";
			    
				if (!File.Exists(strFileName))		//指定ファイル検索
				{
					return FILE_ERR_OPEN;
				}
			        
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				With gudtXraySystemValue
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                    string ret = modTools.INIRead(strSect, "XrayScaleMaxkV", "0", strFileName);

                    if (modTools.CIntChk(out modData.portNo, modTools.INIRead(strSect, "portNo", "0", strFileName), (int)0, (int)10) != 0) return FILE_ERR_READ; //Rev23.10 ポート番号追加 by長野 2015/09/29
                    if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMaxkV, modTools.INIRead(strSect, "XrayScaleMaxkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMaxuA, modTools.INIRead(strSect, "XrayScaleMaxuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMinkV, modTools.INIRead(strSect, "XrayScaleMinkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMinuA, modTools.INIRead(strSect, "XrayScaleMinuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMaxkV, modTools.INIRead(strSect, "XraySetMaxkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMaxuA, modTools.INIRead(strSect, "XraySetMaxuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMinkV, modTools.INIRead(strSect, "XraySetMinkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMinuA, modTools.INIRead(strSect, "XraySetMinuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF1MaxPower, modTools.INIRead(strSect, "XrayF1MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF2MaxPower, modTools.INIRead(strSect, "XrayF2MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF3MaxPower, modTools.INIRead(strSect, "XrayF3MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF4MaxPower, modTools.INIRead(strSect, "XrayF4MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayFocusNumber, modTools.INIRead(strSect, "XrayFocusNumber", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailkV, modTools.INIRead(strSect, "XrayAvailkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailuA, modTools.INIRead(strSect, "XrayAvailuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailTimeInside, modTools.INIRead(strSect, "XrayAvailTimeInside", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailTimeOn, modTools.INIRead(strSect, "XrayAvailTimeOn", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
			        
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmupTime, modTools.INIRead(strSect, "XrayWarmupTime", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup1Time, modTools.INIRead(strSect, "XrayWarmup1Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup2Time, modTools.INIRead(strSect, "XrayWarmup2Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup3Time, modTools.INIRead(strSect, "XrayWarmup3Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
			        
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayVacuumInf, modTools.INIRead(strSect, "XrayVacuumSts", "0", strFileName), 0, 1) != 0) return FILE_ERR_READ; //追加2010/05/20(KS1)hata_L9421-02対応

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				End With
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				return FILE_OK;
			}
			catch
			{
				//ファイル読み込み異常
				return FILE_ERR_READ;
		    }
		}


		//==============================================================================
		//
		//   浜松ホトニクス製 開放型 ２３０ｋＶパラメータ読み込み
		//
		//   追加2009/08/19（KSS)hata_L10801
		//==============================================================================
		public static int ReadTSB_001_L10801()
		{
			string strFileName;
			string strSect;
		    
			try
			{
				strFileName = PATH_INI_FILES + FNAME_TSB_001_L10801;
                //Rev23.10 by長野 2015/09/29
                //strFileName = modIniFiles.iniFile;

                strSect = "Xray";
			    
				if (!File.Exists(strFileName))		//指定ファイル検索
				{
					return FILE_ERR_OPEN;
				}

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				With gudtXraySystemValue
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                    if (modTools.CIntChk(out modData.portNo, modTools.INIRead(strSect, "portNo", "0", strFileName), (int)0, (int)10) != 0) return FILE_ERR_READ; //Rev23.10 ポート番号追加 by長野 2015/09/29
                	if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMaxkV, modTools.INIRead(strSect, "XrayScaleMaxkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMaxuA, modTools.INIRead(strSect, "XrayScaleMaxuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMinkV, modTools.INIRead(strSect, "XrayScaleMinkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMinuA, modTools.INIRead(strSect, "XrayScaleMinuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMaxkV, modTools.INIRead(strSect, "XraySetMaxkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMaxuA, modTools.INIRead(strSect, "XraySetMaxuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMinkV, modTools.INIRead(strSect, "XraySetMinkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMinuA, modTools.INIRead(strSect, "XraySetMinuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayMaxPower, modTools.INIRead(strSect, "XrayMaxPower", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF1MaxPower, modTools.INIRead(strSect, "XrayF1MaxPower", "0", strFileName), MIN_UA, MAX_UA_4) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF2MaxPower, modTools.INIRead(strSect, "XrayF2MaxPower", "0", strFileName), MIN_UA, MAX_UA_4) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF3MaxPower, modTools.INIRead(strSect, "XrayF3MaxPower", "0", strFileName), MIN_UA, MAX_UA_4) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF4MaxPower, modTools.INIRead(strSect, "XrayF4MaxPower", "0", strFileName), MIN_UA, MAX_UA_4) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayFocusNumber, modTools.INIRead(strSect, "XrayFocusNumber", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailkV, modTools.INIRead(strSect, "XrayAvailkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailuA, modTools.INIRead(strSect, "XrayAvailuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailTimeInside, modTools.INIRead(strSect, "XrayAvailTimeInside", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailTimeOn, modTools.INIRead(strSect, "XrayAvailTimeOn", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
			        
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmupTime, modTools.INIRead(strSect, "XrayWarmupTime", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup1Time, modTools.INIRead(strSect, "XrayWarmup1Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup2Time, modTools.INIRead(strSect, "XrayWarmup2Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup3Time, modTools.INIRead(strSect, "XrayWarmup3Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
			        
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayTargetInfSTG, modTools.INIRead(strSect, "XrayMaxPower", "0", strFileName), 0, 1) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayVacuumInf, modTools.INIRead(strSect, "XrayVacuumSts", "0", strFileName), 0, 1) != 0) return FILE_ERR_READ;
			         
					 //追加2011/09/15(KS1)hata_X線電力制限に対応
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWattageLimit, modTools.INIRead(strSect, "XrayWattageLimit", "0", strFileName), 0, 1) != 0) return FILE_ERR_READ;
			    
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				End With
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				return FILE_OK;
			}
			catch
			{		    
				//ファイル読み込み異常
				return FILE_ERR_READ;
		    }
		}

		//==============================================================================
		//
		//   浜松ホトニクス製 密封　分離型 ９０ｋＶパラメータ読み込み
		//
		//   追加2010/02/19(KSS)hata_L8601対応
		//==============================================================================
		public static int ReadTSB_001_L8601()
		{
			string strFileName;
			string strSect;
		    
			try
			{
				strFileName = PATH_INI_FILES + FNAME_TSB_001_L8601;
				strSect = "Xray";
			    
				if (!File.Exists(strFileName))		//指定ファイル検索
				{
					return FILE_ERR_OPEN;
				}

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				With gudtXraySystemValue
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                    if (modTools.CIntChk(out modData.portNo, modTools.INIRead(strSect, "portNo", "0", strFileName), (int)0, (int)10) != 0) return FILE_ERR_READ; //Rev23.10 ポート番号追加 by長野 2015/09/29
                	if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMaxkV, modTools.INIRead(strSect, "XrayScaleMaxkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMaxuA, modTools.INIRead(strSect, "XrayScaleMaxuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMinkV, modTools.INIRead(strSect, "XrayScaleMinkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMinuA, modTools.INIRead(strSect, "XrayScaleMinuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMaxkV, modTools.INIRead(strSect, "XraySetMaxkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMaxuA, modTools.INIRead(strSect, "XraySetMaxuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMinkV, modTools.INIRead(strSect, "XraySetMinkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMinuA, modTools.INIRead(strSect, "XraySetMinuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF1MaxPower, modTools.INIRead(strSect, "XrayF1MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF2MaxPower, modTools.INIRead(strSect, "XrayF2MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF3MaxPower, modTools.INIRead(strSect, "XrayF3MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF4MaxPower, modTools.INIRead(strSect, "XrayF4MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayFocusNumber, modTools.INIRead(strSect, "XrayFocusNumber", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailkV, modTools.INIRead(strSect, "XrayAvailkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailuA, modTools.INIRead(strSect, "XrayAvailuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailTimeInside, modTools.INIRead(strSect, "XrayAvailTimeInside", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailTimeOn, modTools.INIRead(strSect, "XrayAvailTimeOn", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
			        
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmupTime, modTools.INIRead(strSect, "XrayWarmupTime", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup1Time, modTools.INIRead(strSect, "XrayWarmup1Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup2Time, modTools.INIRead(strSect, "XrayWarmup2Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup3Time, modTools.INIRead(strSect, "XrayWarmup3Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
			        
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayVacuumInf, modTools.INIRead(strSect, "XrayVacuumSts", "0", strFileName), 0, 1) != 0) return FILE_ERR_READ; //追加2010/05/20(KS1)hata_L9421-02対応

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				End With
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				return FILE_OK;
			}
		    catch
			{
				//ファイル読み込み異常
				return FILE_ERR_READ;
		    }
		}


		//==============================================================================
		//
		//   浜松ホトニクス製 密封　分離型 ９０ｋＶパラメータ読み込み
		//
		//   追加2010/02/19(KSS)hata_L8601対応
		//==============================================================================
		public static int ReadTSB_001_L8121_02()
		{
			string strFileName;
			string strSect;
		    
			try
			{
				strFileName = PATH_INI_FILES + FNAME_TSB_001_L8121_02;
				strSect = "Xray";
			    
				if (!File.Exists(strFileName))		//指定ファイル検索
				{
					return FILE_ERR_OPEN;
				}
			        
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				With gudtXraySystemValue
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                    if (modTools.CIntChk(out modData.portNo, modTools.INIRead(strSect, "portNo", "0", strFileName), (int)0, (int)10) != 0) return FILE_ERR_READ; //Rev23.10 ポート番号追加 by長野 2015/09/29
            		if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMaxkV, modTools.INIRead(strSect, "XrayScaleMaxkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMaxuA, modTools.INIRead(strSect, "XrayScaleMaxuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMinkV, modTools.INIRead(strSect, "XrayScaleMinkV", "0", strFileName), (int)MIN_KV,(int) MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMinuA, modTools.INIRead(strSect, "XrayScaleMinuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMaxkV, modTools.INIRead(strSect, "XraySetMaxkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMaxuA, modTools.INIRead(strSect, "XraySetMaxuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMinkV, modTools.INIRead(strSect, "XraySetMinkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMinuA, modTools.INIRead(strSect, "XraySetMinuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
			        
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayMaxPower, modTools.INIRead(strSect, "XrayMaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF1MaxPower, modTools.INIRead(strSect, "XrayF1MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF2MaxPower, modTools.INIRead(strSect, "XrayF2MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF3MaxPower, modTools.INIRead(strSect, "XrayF3MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF4MaxPower, modTools.INIRead(strSect, "XrayF4MaxPower", "0", strFileName), MIN_UA, MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayFocusNumber, modTools.INIRead(strSect, "XrayFocusNumber", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailkV, modTools.INIRead(strSect, "XrayAvailkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailuA, modTools.INIRead(strSect, "XrayAvailuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailTimeInside, modTools.INIRead(strSect, "XrayAvailTimeInside", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailTimeOn, modTools.INIRead(strSect, "XrayAvailTimeOn", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
			        
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmupTime, modTools.INIRead(strSect, "XrayWarmupTime", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup1Time, modTools.INIRead(strSect, "XrayWarmup1Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup2Time, modTools.INIRead(strSect, "XrayWarmup2Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup3Time, modTools.INIRead(strSect, "XrayWarmup3Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
			        
					if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayVacuumInf, modTools.INIRead(strSect, "XrayVacuumSts", "0", strFileName), 0, 1) != 0) return FILE_ERR_READ; //追加2010/05/20(KS1)hata_L9421-02対応

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				End With
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				return FILE_OK;
			}
		    catch
			{
				//ファイル読み込み異常
				return FILE_ERR_READ;
			}
		}

        ////
        //==============================================================================
        //
        //   浜松ホトニクス製 開放型 ３００ｋＶパラメータ読み込み
        //
        //   追加2015/10/01（検S1)長野 Rev23.10
        //==============================================================================
        public static int ReadTSB_001_L12721()
        {
            string strFileName;
            string strSect;

            try
            {
                strFileName = PATH_INI_FILES + FNAME_TSB_001_L12721;
                //Rev23.10 by長野 2015/09/29
                //strFileName = modIniFiles.iniFile;

                strSect = "Xray";

                if (!File.Exists(strFileName))		//指定ファイル検索
                {
                    return FILE_ERR_OPEN;
                }

                #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                //				With gudtXraySystemValue
                #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                if (modTools.CIntChk(out modData.portNo, modTools.INIRead(strSect, "portNo", "0", strFileName), (int)0, (int)10) != 0) return FILE_ERR_READ; //Rev23.10 ポート番号追加 by長野 2015/09/29
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMaxkV, modTools.INIRead(strSect, "XrayScaleMaxkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMaxuA, modTools.INIRead(strSect, "XrayScaleMaxuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMinkV, modTools.INIRead(strSect, "XrayScaleMinkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMinuA, modTools.INIRead(strSect, "XrayScaleMinuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMaxkV, modTools.INIRead(strSect, "XraySetMaxkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMaxuA, modTools.INIRead(strSect, "XraySetMaxuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMinkV, modTools.INIRead(strSect, "XraySetMinkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMinuA, modTools.INIRead(strSect, "XraySetMinuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayMaxPower, modTools.INIRead(strSect, "XrayMaxPower", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF1MaxPower, modTools.INIRead(strSect, "XrayF1MaxPower", "0", strFileName), MIN_UA, MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF2MaxPower, modTools.INIRead(strSect, "XrayF2MaxPower", "0", strFileName), MIN_UA, MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF3MaxPower, modTools.INIRead(strSect, "XrayF3MaxPower", "0", strFileName), MIN_UA, MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF4MaxPower, modTools.INIRead(strSect, "XrayF4MaxPower", "0", strFileName), MIN_UA, MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayFocusNumber, modTools.INIRead(strSect, "XrayFocusNumber", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailkV, modTools.INIRead(strSect, "XrayAvailkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailuA, modTools.INIRead(strSect, "XrayAvailuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailTimeInside, modTools.INIRead(strSect, "XrayAvailTimeInside", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailTimeOn, modTools.INIRead(strSect, "XrayAvailTimeOn", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;

                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmupTime, modTools.INIRead(strSect, "XrayWarmupTime", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup1Time, modTools.INIRead(strSect, "XrayWarmup1Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup2Time, modTools.INIRead(strSect, "XrayWarmup2Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup3Time, modTools.INIRead(strSect, "XrayWarmup3Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;

                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayTargetInfSTG, modTools.INIRead(strSect, "XrayMaxPower", "0", strFileName), 0, 1) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayVacuumInf, modTools.INIRead(strSect, "XrayVacuumSts", "0", strFileName), 0, 1) != 0) return FILE_ERR_READ;

                //追加2011/09/15(KS1)hata_X線電力制限に対応
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWattageLimit, modTools.INIRead(strSect, "XrayWattageLimit", "0", strFileName), 0, 1) != 0) return FILE_ERR_READ;

                #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                //				End With
                #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                return FILE_OK;
            }
            catch
            {
                //ファイル読み込み異常
                return FILE_ERR_READ;
            }
        }

        ////
        //==============================================================================
        //
        //   浜松ホトニクス製 開放型 １６０ｋＶパラメータ読み込み
        //
        //   追加2015/10/01（検S1)長野 L10711
        //==============================================================================
        public static int ReadTSB_001_L10711()
        {
            string strFileName;
            string strSect;

            try
            {
                strFileName = PATH_INI_FILES + FNAME_TSB_001_L10711;
                //Rev23.10 by長野 2015/09/29
                //strFileName = modIniFiles.iniFile;

                strSect = "Xray";

                if (!File.Exists(strFileName))		//指定ファイル検索
                {
                    return FILE_ERR_OPEN;
                }

                #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                //				With gudtXraySystemValue
                #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                if (modTools.CIntChk(out modData.portNo, modTools.INIRead(strSect, "portNo", "0", strFileName), (int)0, (int)10) != 0) return FILE_ERR_READ; //Rev23.10 ポート番号追加 by長野 2015/09/29
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMaxkV, modTools.INIRead(strSect, "XrayScaleMaxkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMaxuA, modTools.INIRead(strSect, "XrayScaleMaxuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMinkV, modTools.INIRead(strSect, "XrayScaleMinkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayScaleMinuA, modTools.INIRead(strSect, "XrayScaleMinuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMaxkV, modTools.INIRead(strSect, "XraySetMaxkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMaxuA, modTools.INIRead(strSect, "XraySetMaxuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMinkV, modTools.INIRead(strSect, "XraySetMinkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XraySetMinuA, modTools.INIRead(strSect, "XraySetMinuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayMaxPower, modTools.INIRead(strSect, "XrayMaxPower", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF1MaxPower, modTools.INIRead(strSect, "XrayF1MaxPower", "0", strFileName), MIN_UA, MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF2MaxPower, modTools.INIRead(strSect, "XrayF2MaxPower", "0", strFileName), MIN_UA, MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF3MaxPower, modTools.INIRead(strSect, "XrayF3MaxPower", "0", strFileName), MIN_UA, MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CSngChk(out modData.gudtXraySystemValue.E_XrayF4MaxPower, modTools.INIRead(strSect, "XrayF4MaxPower", "0", strFileName), MIN_UA, MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayFocusNumber, modTools.INIRead(strSect, "XrayFocusNumber", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailkV, modTools.INIRead(strSect, "XrayAvailkV", "0", strFileName), (int)MIN_KV, (int)MAX_KV) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailuA, modTools.INIRead(strSect, "XrayAvailuA", "0", strFileName), (int)MIN_UA, (int)MAX_UA_4) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailTimeInside, modTools.INIRead(strSect, "XrayAvailTimeInside", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayAvailTimeOn, modTools.INIRead(strSect, "XrayAvailTimeOn", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;

                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmupTime, modTools.INIRead(strSect, "XrayWarmupTime", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup1Time, modTools.INIRead(strSect, "XrayWarmup1Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup2Time, modTools.INIRead(strSect, "XrayWarmup2Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWarmup3Time, modTools.INIRead(strSect, "XrayWarmup3Time", "0", strFileName), (int)MIN_SEC, (int)MAX_SEC) != 0) return FILE_ERR_READ;

                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayTargetInfSTG, modTools.INIRead(strSect, "XrayMaxPower", "0", strFileName), 0, 1) != 0) return FILE_ERR_READ;
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayVacuumInf, modTools.INIRead(strSect, "XrayVacuumSts", "0", strFileName), 0, 1) != 0) return FILE_ERR_READ;

                //追加2011/09/15(KS1)hata_X線電力制限に対応
                if (modTools.CIntChk(out modData.gudtXraySystemValue.E_XrayWattageLimit, modTools.INIRead(strSect, "XrayWattageLimit", "0", strFileName), 0, 1) != 0) return FILE_ERR_READ;

                #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                //				End With
                #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                return FILE_OK;
            }
            catch
            {
                //ファイル読み込み異常
                return FILE_ERR_READ;
            }
        }

        ////



	}
}
