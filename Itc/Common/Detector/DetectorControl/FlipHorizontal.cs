using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectorControl
{
    /// <summary>
    /// 左右反転
    /// </summary>
    public class FlipHorizontal: IFlipHorizontal
    {
        /// <summary>
        /// 左右反転完了
        /// </summary>
        public event EventHandler EndFlipHorizontal;
        /// <summary>
        /// 左右反転?
        /// </summary>
        public bool IsFlipHorizontal { get; private set; }
        /// <summary>
        /// 横
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// 高さ
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// 変換後のデータ
        /// </summary>
        public ushort[] Targets { get; private set; }
        /// <summary>
        /// 設定I/F
        /// </summary>
        private readonly IDetConf _DetConf;
        /// <summary>
        /// 左右反転
        /// </summary>
        public FlipHorizontal(IDetConf detconf)
        {
            _DetConf = detconf;
            _DetConf.EndLoadParam += (s, e) =>
            {
                var dc = s as DetConf;
                IsFlipHorizontal = dc.FlipHorizontal;
            };
        }
        /// <summary>
        /// 左右反転実行
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void DoFlipHorizontal(ushort[] data, int width, int height)
        {
            Width = width;
            Height = height;

            Targets = new ushort[width * height];
            foreach (var idx in Enumerable.Range(0, width * height))
            {
                int xx = (int)idx % width;
                int yy = (int)idx / width;
                //Debug.WriteLine($"前x={xx} y={yy}");
                //yy = width - 1 - xx;
                xx = width - 1 - xx;
                //Debug.WriteLine($"後x={xx} y={yy}");
                Targets[xx + (yy * width)] = data[idx];
            }
            EndFlipHorizontal?.Invoke(this, new EventArgs());
        }
    }
    /// <summary>
    /// 左右反転I/F
    /// </summary>
    public interface IFlipHorizontal
    {
        /// <summary>
        /// 左右反転
        /// </summary>
        void DoFlipHorizontal(ushort[] data, int width, int height);
        /// <summary>
        ///左右反転完了
        /// </summary>
        event EventHandler EndFlipHorizontal;
    }
}
