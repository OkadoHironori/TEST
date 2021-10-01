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
using System.Reflection;

namespace CT30K.Common.Library
{
    /// <summary>
    /// ログレベル
    /// </summary>
    public enum LogLevel
    {
        // 優先度高い順から
        /// <summary>
        /// エラー
        /// </summary>
        Error,

        /// <summary>
        /// 警告
        /// </summary>
        Warning,

        /// <summary>
        /// 情報
        /// </summary>
        Information,

        /// <summary>
        /// トレース
        /// </summary>
        Trace
    }

    /// <summary>
    /// エラーログクラス
    /// </summary>
    public class ErrLogger
    {
        private const string LOG_FOLDER = @"Log\Err";		// エラーログフォルダ
        private const string LOG_PREFIX = @"Err_";		// エラーログフォルダ

        private static LogLevel logLevel = LogLevel.Trace;  // 出力ログレベル
        private static Logger logging = new Logger();       // ログクラス

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private ErrLogger()
        {
        }

        /// <summary>
        /// ログレベル取得、設定
        /// </summary>
        public static LogLevel Level
        {
            get { return logLevel; }
            set
            {
                if (value >= LogLevel.Warning)
                {
                    logLevel = value;
                }
                else
                {
                    logLevel = LogLevel.Warning;
                }
            }
        }

        /// <summary>
        /// ログ書込み
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="logs">出力文字列（複数指定可能）</param>
        public static void Write(LogLevel level, int stack, params string[] logs)
        {
            // 出力優先度より低い(値が大きい)場合は何もしない
            if (logLevel < level)
            {
                return;
            }

            string levelLog = "";
            switch (level)
            {
                case LogLevel.Error:
                    levelLog = "[エラー]";
                    break;
                case LogLevel.Warning:
                    levelLog = "[警告]";
                    break;
                case LogLevel.Information:
                    levelLog = "[情報]";
                    break;
                case LogLevel.Trace:
                    levelLog = "[トレース]";
                    break;
            }

            string folder = Path.Combine(logging.AppDir, LOG_FOLDER);

            // 現在の日付
            DateTime now = DateTime.Now;
            string nowDate = now.ToString(Logger.DATE_FILENAME);
            string filename = LOG_PREFIX + nowDate + Logger.LOG_EXT;

            // エラー元メソッド名取得
            StackTrace trace = new StackTrace(0);
            if (stack >= trace.FrameCount)
            {
                stack = trace.FrameCount - 1;
            }
            if (stack < 1)
            {
                stack = 1;
            }
            StackFrame frame = trace.GetFrame(stack);
            MethodBase method = frame.GetMethod();
            StringBuilder baseName = new StringBuilder(method.ReflectedType.Name);
            baseName.AppendFormat(".{0}()", method.Name);

            // ログ文字列作成
            string[] logArray = new string[logs.Length + 2];
            logArray[0] = levelLog;
            logArray[1] = baseName.ToString();
            logs.CopyTo(logArray, 2);

            // ログ追加
            logging.AddLog(folder, filename, logArray);

        }

        /// <summary>
        /// ログファイル削除
        /// </summary>
        /// <param name="keepCount">ログファイルの保持数</param>
        public static void CleanUp(int keepCount)
        {
            //ログ削除
            logging.DeleteLog(Path.Combine(logging.AppDir, LOG_FOLDER), keepCount);
        }
    }
}
