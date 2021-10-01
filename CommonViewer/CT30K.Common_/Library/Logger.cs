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
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace CT30K.Common.Library
{    
    /// <summary>
    /// ログクラス
    /// </summary>
    internal class Logger : IDisposable
    {
        public const string LOG_EXT = ".csv";				// 拡張子
        public const string DATE_FILENAME = "yyyyMMdd";		// 日付フォーマット
        private const int LOG_QUEUE_MAX = 500;              // ログキュー最大値
        private const string LOG_MAX_MESSAGE = "ログキューが最大値を超えました。ログ書込み異常です。";

        private Queue<LogData> queueData = new Queue<LogData>();  // データキュー
        private readonly object lockData = new object();	// 排他ロック用オブジェクト

        private Thread loggingThread = null;
        private AutoResetEvent logDataEvent = new AutoResetEvent(false);

        // shift-jisエンコーディング
        private Encoding sjis = Encoding.GetEncoding(932);

        // 同期オブジェクト
        private readonly object sync = new object();

        // アプリケーションフォルダ
        private string appDir;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Logger()
        {
            string[] cmds = Environment.GetCommandLineArgs();
            appDir = Path.GetDirectoryName(cmds[0]);

            // スレッド開始
            if (loggingThread == null)
            {
                loggingThread = new Thread(new ThreadStart(LoggingProc));
                loggingThread.IsBackground = true;
                loggingThread.Start();
            }
        }

        /// <summary>
        /// アプリケーションフォルダ取得
        /// </summary>
        public string AppDir
        {
            get { return appDir; }
        }

        /// <summary>
        /// ログ追加
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <param name="logs"></param>
        public void AddLog(string folder, string fileName, params string[] logs)
        {
            LogData data;

            lock (lockData)
            {
                if (queueData.Count > LOG_QUEUE_MAX)
                {
                    string[] err = new string[1];
                    err[0] = LOG_MAX_MESSAGE;
                    data = new LogData(folder, fileName, Thread.CurrentThread.ManagedThreadId, err);
                    queueData.Clear();
                }
                else
                {
                    data = new LogData(folder, fileName, Thread.CurrentThread.ManagedThreadId, logs);
                }

                queueData.Enqueue(data);
            }
            logDataEvent.Set();
        }

        /// <summary>
        /// ログ取り出し
        /// </summary>
        /// <returns></returns>
        private LogData GetLog()
        {
            LogData data;

            if (queueData.Count > 0)
            {
                lock (lockData)
                {
                    data = queueData.Dequeue();
                }
            }
            else
            {
                data = LogData.None;
            }

            // まだキューがあったらイベント発行しておく
            if (queueData.Count > 0)
            {
                logDataEvent.Set();
            }

            return data;
        }
        
        /// <summary>
        /// ロギングスレッド処理
        /// </summary>
        private void LoggingProc()
        {
            while (loggingThread != null)
            {
                logDataEvent.WaitOne();

                LogData data = GetLog();
                if (data != LogData.None)
                {
                    WriteLog(data);
                }
            }
        }

        /// <summary>
        /// ログ書込み
        /// </summary>
        /// <param name="log"></param>
        private void WriteLog(LogData log)
        {
            lock (sync)
            {
                try
                {
                    // 出力フォルダなければ作成
                    if (!Directory.Exists(log.FolderName))
                    {
                        // フォルダ作成
                        Directory.CreateDirectory(log.FolderName);
                    }

                    using (StreamWriter writer = new StreamWriter(Path.Combine(log.FolderName, log.FileName), true, sjis))
                    {

                        // 出力文字列作成
                        StringBuilder data = new StringBuilder(log.DateLog);

                        //スレッドID出力
                        data.Append(LogData.LOG_SPLIT);
                        data.Append(log.ThreadId.ToString());

                        // ログ文字列取得
                        string[] logs = log.GetLogArray();
                        for (int i = 0; i < logs.Length; i++)
                        {
                            data.Append(LogData.LOG_SPLIT);
                            data.Append(logs[i]);
                        }

                        // 書込み
                        writer.WriteLine(data.ToString());

                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(this.ToString() + " Error\n" + e.Message);
                }
                finally
                {
                }
            }
        }

        /// <summary>
        /// ログ削除処理
        /// </summary>
        /// <param name="logFolder"></param>
        /// <param name="keepCount"></param>
        public void DeleteLog(string logFolder, int keepCount)
        {
            try
            {
                // 出力フォルダなければ作成
                if (!Directory.Exists(logFolder))
                {
                    // フォルダ作成
                    Directory.CreateDirectory(logFolder);
                    return;
                }

                if (keepCount < 0)
                {
                    return;
                }

                //ログファイル名の取得
                string[] logFiles = Directory.GetFiles(logFolder, "*" + LOG_EXT);
                int logCount = logFiles.Length;

                if (logCount > keepCount)
                {
                    //ファイル名を昇順に並べ替え
                    Array.Sort(logFiles);

                    //ログファイルを削除
                    for (int i = 0; i < logCount - keepCount; i++)
                    {
                        File.Delete(logFiles[i]);
                    }
                }
            }
            catch
            {
            }

        }

        #region IDisposable メンバ

        public void Dispose()
        {
        }

        #endregion

        #region IDisposable メンバ

        void IDisposable.Dispose()
        {
        }

        #endregion
    }
}
