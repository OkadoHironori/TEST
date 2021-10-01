using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Itc.Common.TXEnum
{
    public class SelectSPD
    {
        /// <summary>
        /// 速度
        /// </summary>
        public float SPD { get; set;}
        /// <summary>
        /// 名前
        /// </summary>
        public string DispName { get; set; }
    }

    /// <summary>
    /// 進捗
    /// </summary>
    public class CalProgress
    {
        public int Percent { get; set; }
        public string Status { get; set; }
    }

    /// <summary>
    /// 進捗
    /// </summary>
    public class ComProgress
    {
        /// <summary>
        /// 進捗通知
        /// </summary>
        public IProgress<CalProgress> prog { get; set; }
        /// <summary>
        /// キャンセル
        /// </summary>
        public CancellationTokenSource ctoken { get; set; }
        /// <summary>
        /// トータル工程
        /// </summary>       
        public float Total { get; set; }
        /// <summary>
        /// 目標工程
        /// </summary>       
        public float Target { get; set; }
        /// <summary>
        /// 表示メッセージ
        /// </summary>
        public string Message { get; set; }
    }
}
