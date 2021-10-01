using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTAddress
{
    /// <summary>
    /// 入力ファイル
    /// </summary>
    public class ImportFile
    {
        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// コーン番号
        /// </summary>
        public int ConeNo { get; set; }
        /// <summary>
        /// スライス番号
        /// </summary>
        public int SliceNo { get; set; }
        /// <summary>
        /// スキャン位置
        /// </summary>
        public float ScanPosi { get; set; }
        /// <summary>
        /// システム名
        /// </summary>
        public string SystemName { get; set; }
    }
}
