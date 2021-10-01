using System.IO;

namespace Itc.Common.Extensions
{
    public static class IOExtensions
    {
        /// <summary>
        /// 指定した名前のサブディレクトリ情報を取得する
        /// </summary>
        /// <remarks>CreateSubdirectoryだと確認だけすることができないので</remarks>
        /// <param name="dir"></param>
        /// <param name="subname"></param>
        /// <returns></returns>
        public static DirectoryInfo GetSubDirectory(this DirectoryInfo dir, string subname) => new DirectoryInfo(Path.Combine(dir.FullName, subname));

        /// <summary>
        /// 指定した名前のファイル情報を取得する
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static FileInfo GetFileInfo(this DirectoryInfo dir, string filename) => new FileInfo(Path.Combine(dir.FullName, filename));

        /// <summary>
        /// 上層ディレクトリを取得する
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static DirectoryInfo GetParentDirectoryInfo(this DirectoryInfo dir)
        {
            return dir.GetSubDirectory("../");
        }
    }
}
