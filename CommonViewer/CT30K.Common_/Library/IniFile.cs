//-----------------------------------------------------------------------------
// @(s)
// ITC���C�u���� [�t�@�C��]
//
// @(h) ini�t�@�C���ǂݏ��� ( '05/07/12 (ITC)(SI3)K.Yahagi )
//
//  (c) Copyright 2005 by Toshiba IT & Control Systems Corporation
//-----------------------------------------------------------------------------
// @(h) ���e�ύX����
// (�C�����e ���t �ύX�Җ� <�C���^�O>)
//-----------------------------------------------------------------------------
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace CT30K.Common.Library
{
	/// <summary>
	/// ini�t�@�C���̓ǂݏ���
	/// </summary>
	public class IniFile
	{
		// Win32API�̌Ăяo���錾
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

		// �萔
		private const int BUFFER_LENGTH = 1024;

		// �ϐ��錾
		private string iniFilePath;

		/// <summary>
		/// ini�t�@�C���N���X�̃R���X�g���N�^
		/// </summary>
		public IniFile()
		{
			string fileName;
			
			// ���s�t�@�C�����擾
			try
			{
				string [] appArgs = Environment.GetCommandLineArgs();
				fileName = Path.GetFileNameWithoutExtension(appArgs[0]);
			}
			catch 
			{
				fileName = "NotApplication.ini";
			}
			
			// ini�t�@�C���p�X�쐬
			StringBuilder path = new StringBuilder(Environment.CurrentDirectory);
			path.Append("\\");
			path.Append(fileName);
			path.Append(".ini");

			iniFilePath =  path.ToString();	
		}
		

		/// <summary>
		/// ini�t�@�C���N���X�̃R���X�g���N�^
		/// </summary>
		/// <param name="path">�t�@�C���p�X��</param>
		public IniFile(string path)
		{
			iniFilePath = path;
		}

        public bool Exists
        {
            get { return File.Exists(iniFilePath); }
        }

		/// <summary>
		/// ini�t�@�C��������ǂݍ���
		/// </summary>
		/// <param name="section">�Z�N�V������</param>
		/// <param name="key">�L�[����</param>
		/// <param name="defaultVal">�f�t�H���g�l</param>
		/// <returns>�ǂݍ��ݕ�����</returns>
		public string GetIniString(string section, string key, string defaultVal) 
		{
			StringBuilder sb = new StringBuilder(BUFFER_LENGTH);
			
			// ini������ǂݍ���
			GetPrivateProfileString(section, key, defaultVal, sb, (uint)sb.Capacity, iniFilePath);

			return sb.ToString();
		}

        //�ǉ�2014/11/28hata_v19.51_dnet
        /// <summary>
        /// �t�@�C�����w�肵��ini�t�@�C��������ǂݍ���
        /// </summary>
        /// <param name="section">�Z�N�V������</param>
        /// <param name="key">�L�[����</param>
        /// <param name="defaultVal">�f�t�H���g�l</param>
        /// <param name="FilePath">�t�@�C���p�X</param>
        /// <returns>�ǂݍ��ݕ�����</returns>
        public string GetFileIniString(string section, string key, string defaultVal, string FilePath)
        {
            StringBuilder sb = new StringBuilder(BUFFER_LENGTH);

            // ini������ǂݍ���
            GetPrivateProfileString(section, key, defaultVal, sb, (uint)sb.Capacity, FilePath);

            return sb.ToString();
        }

		/// <summary>
		/// ini�t�@�C�������ǂݍ���
		/// </summary>
		/// <param name="section">�Z�N�V������</param>
		/// <param name="key">�L�[����</param>
		/// <param name="nDefault">�f�t�H���g�l</param>
		/// <returns>�ǂݍ��ݐ����l</returns>
		public int GetIniInt(string section, string key, int nDefault)
		{
			// ini�����ǂݍ���
			return (int)GetPrivateProfileInt(section, key, nDefault, iniFilePath);
		}

		/// <summary>
		/// ini�t�@�C����������
		/// </summary>
		/// <param name="section">�Z�N�V������</param>
		/// <param name="key">�L�[����</param>
		/// <param name="val">�l</param>
		/// <returns>true:���� / false:���s</returns>
		public bool WriteIniString(string section, string key, string val) 
		{
			// ini��������
			uint ret = WritePrivateProfileString(section, key, val, iniFilePath);

			if (ret == 0) 
			{
				// ���s
				return false;
			} 
			else 
			{
				// ����
				return true;
			}

		}
	}
}
