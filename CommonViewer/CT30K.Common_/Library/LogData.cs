//-----------------------------------------------------------------------------
// @(s)
// ITC���C�u���� [�t�@�C��]
//
// @(h) ���O�t�@�C���������� ( '05/07/25 (ITC)(SI3)K.Yahagi )
//
//  (c) Copyright 2005 by Toshiba IT & Control Systems Corporation
//-----------------------------------------------------------------------------
// @(h) ���e�ύX����
// (�C�����e ���t �ύX�Җ� <�C���^�O>)
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace CT30K.Common.Library
{
    /// <summary>
    /// ���O�f�[�^�N���X
    /// </summary>
    public class LogData
    {
        /// <summary>
        /// ��؂蕶��
        /// </summary>
        public const string LOG_SPLIT = ",";
        /// <summary>
        /// ���t�t�H�[�}�b�g
        /// </summary>
        public const string DATE_FORMAT = "yyyy/MM/dd";
        /// <summary>
        /// ���ԃt�H�[�}�b�g
        /// </summary>
        public const string TIME_FORMAT = "HH:mm:ss";

        private string folderName;
        private string fileName;
        private int threadId;
        private string[] logArray;
        private string dateLog;

        /// <summary>
        /// ���O�f�[�^�Ȃ�
        /// </summary>
        public static readonly LogData None = new LogData("", "", 0, new string[0]);

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="file"></param>
        /// <param name="logs"></param>
        public LogData(string folder, string file, int thread, string[] logs)
        {
            DateTime now = DateTime.Now;
            this.dateLog = now.ToString(DATE_FORMAT + LOG_SPLIT + TIME_FORMAT);
            this.folderName = folder;
            this.fileName = file;
            this.threadId = thread;
            this.logArray = (string[])logs.Clone();
        }

        /// <summary>
        /// �t�H���_���擾
        /// </summary>
        public string FolderName
        {
            get { return folderName; }
        }

        /// <summary>
        /// �t�@�C�����擾
        /// </summary>
        public string FileName
        {
            get { return fileName; }
        }

        /// <summary>
        /// �X���b�hID
        /// </summary>
        public int ThreadId
        {
            get { return threadId; }
        }

        /// <summary>
        /// �������O�擾
        /// </summary>
        public string DateLog
        {
            get { return dateLog; }
        }

        /// <summary>
        /// ���O�z��擾
        /// </summary>
        /// <returns></returns>
        public string[] GetLogArray()
        {
            return logArray;
        }
    }
}
