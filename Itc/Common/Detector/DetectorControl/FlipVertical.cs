using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectorControl
{
    /// <summary>
    /// 上下反転
    /// </summary>
    public class FlipVertical : IFlipVertical
    {
        /// <summary>
        /// 上下反転完了
        /// </summary>
        public event EventHandler EndFlipVertical;
        /// <summary>
        /// 上下反転?
        /// </summary>
        public bool IsFlipVertical {get; private set; }
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
        /// 上下反転
        /// </summary>
        public FlipVertical(IDetConf detconf)
        {
            _DetConf = detconf;
            _DetConf.EndLoadParam += (s, e) =>
            {
                var dc = s as DetConf;
                IsFlipVertical = dc.FlipVertical;
            };
        }
        /// <summary>
        /// 上下反転実行
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void DoFlipVertical(ushort[] data, int width, int height)
        {
            Width = width;
            Height = height;

            Targets = new ushort[width * height];
            foreach (var idx in Enumerable.Range(0, width * height))
            {
                int xx = (int)idx % width;
                int yy = (int)idx / width;
                //Debug.WriteLine($"前x={xx} y={yy}");
                yy = height - 1 - yy;
                //Debug.WriteLine($"後x={xx} y={yy}");
                Targets[xx + (yy * width)] = data[idx];
            }
            EndFlipVertical?.Invoke(this, new EventArgs());
        }
    }
    /// <summary>
    /// 上下反転 I/F
    /// </summary>
    public interface IFlipVertical
    {
        /// <summary>
        /// 上下反転
        /// </summary>
        void DoFlipVertical(ushort[] data, int width, int height);
        /// <summary>
        ///上下反転完了
        /// </summary>
        event EventHandler EndFlipVertical;
    }
}
