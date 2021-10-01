using System.Collections.Generic;

namespace Itc.Common
{
    /// <summary>
    /// Open/Saveダイアログ用フィルター生成
    /// </summary>
    public class DialogFilterGenerator
    {
        /// <summary>
        /// ダイアログ用フィルター生成
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string Create(string name, string ext) 
            => string.Format("{0} (*{1})|*{1}", name, ext);

        /// <summary>
        /// 複数フィルターの組み合わせ
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string MultiFilter(IEnumerable<string> filter)
            => string.Join("|", filter);
    }
}
