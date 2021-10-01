//-----------------------------------------------------------------------------
// @(s)
// ITCライブラリ [ファイル]
//
// @(h) ログファイル書き込み ( '05/07/25 (ITC)(SI3)K.Yahagi )
//
//  (c) Copyright 2005 by Toshiba IT & Control Systems Corporation
//-----------------------------------------------------------------------------
// @(h) 内容変更履歴
// (修正内容 日付 変更者名 <修正タグ>)
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace CT30K.Common.Library
{
    /// <summary>
    /// ログデータクラス
    /// </summary>
    public class LogData
    {
        /// <summary>
        /// 区切り文字
        /// </summary>
        public const string LOG_SPLIT = ",";
        /// <summary>
        /// 日付フォーマット
        /// </summary>
        public const string DATE_FORMAT = "yyyy/MM/dd";
        /// <summary>
        /// 時間フォーマット
        /// </summary>
        public const string TIME_FORMAT = "HH:mm:ss";

        private string folderName;
        private string fileName;
        private int threadId;
        private string[] logArray;
        private string dateLog;

        /// <summary>
        /// ログデータなし
        /// </summary>
        public static readonly LogData None = new LogData("", "", 0, new string[0]);

        /// <summary>
        /// コンストラクタ
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
        /// フォルダ名取得
        /// </summary>
        public string FolderName
        {
            get { return folderName; }
        }

        /// <summary>
        /// ファイル名取得
        /// </summary>
        public string FileName
        {
            get { return fileName; }
        }

        /// <summary>
        /// スレッドID
        /// </summary>
        public int ThreadId
        {
            get { return threadId; }
        }

        /// <summary>
        /// 日時ログ取得
        /// </summary>
        public string DateLog
        {
            get { return dateLog; }
        }

        /// <summary>
        /// ログ配列取得
        /// </summary>
        /// <returns></returns>
        public string[] GetLogArray()
        {
            return logArray;
        }
    }
}
