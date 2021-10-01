using Itc.Common.Extensions;
using Itc.Common.WinApiImport;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Itc.Common.Utility
{
    public static partial class Tools
    {
        /// <summary>
        /// 空のフォルダかチェック
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsEmptyDirectory(string path)
        {
            //そもそも存在しない
            if (!Directory.Exists(path))
            {
                return false;
            }

            try
            {
                //中身のファイル数をチェック
                string[] entries = Directory.GetFileSystemEntries(path);

                if (null == entries)
                {
                    return false;
                }

                //０の時、空
                return (entries.Length == 0);
            }
            //GetFileSystemEntrie失敗時
            catch (Exception ex) 
            when (ex is UnauthorizedAccessException 
            || ex is ArgumentException 
            || ex is ArgumentNullException 
            || ex is PathTooLongException
            || ex is IOException
            || ex is DirectoryNotFoundException
            ) 
            {
                System.Diagnostics.Debug.WriteLine(ex);

                return false;
            }
        }

        /// <summary>
        /// 空のフォルダを削除
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="sub"></param>
        public static void DeleteEmptyDirectory(DirectoryInfo dir, bool sub)
        {
            if (null == dir)
            {
                return;
            }

            //サブディレクトリにも適用
            if (sub)
            {
                foreach (DirectoryInfo d in dir.GetDirectories())
                {
                    DeleteEmptyDirectory(d, sub);
                }
            }

            if (IsEmptyDirectory(dir.FullName))
            {
                dir.Delete();
            }
        }

        /// <summary>
        /// 外部プロセスを呼び出す。起動済みの場合は前面表示する（最小化は解除）
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="bootCurrentDirectory">指定ファイルの置かれたディレクトリをカレントディレクトリとして起動する</param>
        public static void CallProcess(System.IO.FileInfo fi, bool bootCurrentDirectory = false)
        {
            if (!fi.Exists) return;

            string name = System.IO.Path.GetFileNameWithoutExtension(fi.Name);

            Process[] process = Process.GetProcessesByName(name);

            if (null != process && process.Length > 0)
            {
                //起動済みなら
                Process p = process.First();

                User32.SetForegroundWindow(p.MainWindowHandle);

                //最小化していたら通常に戻す
                if (User32.IsIconic(p.MainWindowHandle))
                {
                    User32.ShowWindow(p.MainWindowHandle);
                }
            }
            else
            {
                if (bootCurrentDirectory)
                {
                    var startInfo = new ProcessStartInfo()
                    {
                        FileName = fi.FullName,
                        WorkingDirectory = fi.DirectoryName,
                    };

                    Process.Start(startInfo);
                }
                else
                {
                    Process.Start(fi.FullName);
                }
            }
        }

        /// <summary>
        /// 絶対パス → 相対パス変換
        /// </summary>
        /// <returns></returns>
        public static string ToRelativePath(string srcPath, string dstPath) 
            => new Uri(dstPath).MakeRelativeUri(new Uri(srcPath)).ToString().Replace("/", @"\");
    }
}
