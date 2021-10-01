//-----------------------------------------------------------------------------
// @(s)
// ITCライブラリ [ファイル]
//
// @(h) iniファイル読み書き ( '05/07/12 (ITC)(SI3)K.Yahagi )
//
//  (c) Copyright 2005 by Toshiba IT & Control Systems Corporation
//-----------------------------------------------------------------------------
// @(h) 内容変更履歴
// (修正内容 日付 変更者名 <修正タグ>)
//-----------------------------------------------------------------------------
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace CT30K.Common.Library
{
	/// <summary>
	/// iniファイルの読み書き
	/// </summary>
	public class IniFile
	{
		// Win32APIの呼び出し宣言
		[DllImport("KERNEL32.DLL")]
		private static extern uint 
			GetPrivateProfileString(string lpAppName, 
			string lpKeyName, string lpDefault, 
			StringBuilder lpReturnedString, uint nSize, 
			string lpFileName);

		[DllImport("KERNEL32.DLL",EntryPoint="GetPrivateProfileStringA")]
		private static extern uint 
			GetPrivateProfileStringByByteArray(string lpAppName, 
			string lpKeyName, string lpDefault, 
			byte [] lpReturnedString, uint nSize, 
			string lpFileName);

		[DllImport("KERNEL32.DLL")]
		private static extern uint 
			GetPrivateProfileInt( string lpAppName, 
			string lpKeyName, int nDefault, string lpFileName );

		[DllImport("KERNEL32.DLL")]
		private static extern uint WritePrivateProfileString(
			string lpAppName,
			string lpKeyName,
			string lpString,
			string lpFileName);

		// 定数
		private const int BUFFER_LENGTH = 1024;

		// 変数宣言
		private string iniFilePath;

		/// <summary>
		/// iniファイルクラスのコンストラクタ
		/// </summary>
		public IniFile()
		{
			string fileName;
			
			// 実行ファイル名取得
			try
			{
				string [] appArgs = Environment.GetCommandLineArgs();
				fileName = Path.GetFileNameWithoutExtension(appArgs[0]);
			}
			catch 
			{
				fileName = "NotApplication.ini";
			}
			
			// iniファイルパス作成
			StringBuilder path = new StringBuilder(Environment.CurrentDirectory);
			path.Append("\\");
			path.Append(fileName);
			path.Append(".ini");

			iniFilePath =  path.ToString();	
		}
		

		/// <summary>
		/// iniファイルクラスのコンストラクタ
		/// </summary>
		/// <param name="path">ファイルパス名</param>
		public IniFile(string path)
		{
			iniFilePath = path;
		}

        public bool Exists
        {
            get { return File.Exists(iniFilePath); }
        }

		/// <summary>
		/// iniファイル文字列読み込み
		/// </summary>
		/// <param name="section">セクション名</param>
		/// <param name="key">キー項目</param>
		/// <param name="defaultVal">デフォルト値</param>
		/// <returns>読み込み文字列</returns>
		public string GetIniString(string section, string key, string defaultVal) 
		{
			StringBuilder sb = new StringBuilder(BUFFER_LENGTH);
			
			// ini文字列読み込み
			GetPrivateProfileString(section, key, defaultVal, sb, (uint)sb.Capacity, iniFilePath);

			return sb.ToString();
		}

        //追加2014/11/28hata_v19.51_dnet
        /// <summary>
        /// ファイルを指定してiniファイル文字列読み込み
        /// </summary>
        /// <param name="section">セクション名</param>
        /// <param name="key">キー項目</param>
        /// <param name="defaultVal">デフォルト値</param>
        /// <param name="FilePath">ファイルパス</param>
        /// <returns>読み込み文字列</returns>
        public string GetFileIniString(string section, string key, string defaultVal, string FilePath)
        {
            StringBuilder sb = new StringBuilder(BUFFER_LENGTH);

            // ini文字列読み込み
            GetPrivateProfileString(section, key, defaultVal, sb, (uint)sb.Capacity, FilePath);

            return sb.ToString();
        }

		/// <summary>
		/// iniファイル整数読み込み
		/// </summary>
		/// <param name="section">セクション名</param>
		/// <param name="key">キー項目</param>
		/// <param name="nDefault">デフォルト値</param>
		/// <returns>読み込み整数値</returns>
		public int GetIniInt(string section, string key, int nDefault)
		{
			// ini整数読み込み
			return (int)GetPrivateProfileInt(section, key, nDefault, iniFilePath);
		}

		/// <summary>
		/// iniファイル書き込み
		/// </summary>
		/// <param name="section">セクション名</param>
		/// <param name="key">キー項目</param>
		/// <param name="val">値</param>
		/// <returns>true:成功 / false:失敗</returns>
		public bool WriteIniString(string section, string key, string val) 
		{
			// ini書き込み
			uint ret = WritePrivateProfileString(section, key, val, iniFilePath);

			if (ret == 0) 
			{
				// 失敗
				return false;
			} 
			else 
			{
				// 成功
				return true;
			}

		}
	}
}
