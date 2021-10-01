using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectorControl
{
    /// <summary>
    /// 画像CCW90回転クラス
    /// </summary>
    public class RotCCW90 : IRotCCW90
    {
        /// <summary>
        /// CCW90回転
        /// </summary>
        public event EventHandler EndRotCCW90;
        /// <summary>
        /// 設定I/F
        /// </summary>
        private readonly IDetConf _DetConf;
        /// <summary>
        /// CCW90回転
        /// </summary>
        public bool CCW90 { get; private set; }
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
        /// 画像CCW90回転クラス
        /// </summary>
        public RotCCW90(IDetConf conf)
        {
            _DetConf = conf;
            _DetConf.EndLoadParam += (s, e) =>
            {
                var dc = s as DetConf;
                CCW90 = dc.CCW90;
            };
        }
        /// <summary>
        /// 画像CCW90回転
        /// </summary>
        /// <param name="dataPtr"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void DoRotCCW90(ushort[] data, int width, int height)
        {

            Width = height;
            Height = width;

            Targets = new ushort[width * height];
            foreach (var idx in Enumerable.Range(0, width * height))
            {
                int xx = (int)idx % width;
                int yy = (int)idx / width;
                //Debug.WriteLine($"前x={xx} y={yy}");
                yy = width - 1 - xx;
                xx = idx / width;
                //Debug.WriteLine($"後x={xx} y={yy}");
                Targets[xx + (yy * height)] = data[idx];
            }
            EndRotCCW90?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// 画像CCW90回転
        /// </summary>
        /// <param name="dataPtr"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void DoRotCW90(ushort[] data, int width, int height)
        {
            Width = height;
            Height = width;

            Targets = new ushort[width * height];
            foreach (var idx in Enumerable.Range(0, width * height))
            {
                int xx = (int)idx % width;
                int yy = (int)idx / width;
                Targets[xx + (yy * height)] = data[idx];
            }

            EndRotCCW90?.Invoke(this, new EventArgs());

        }
    }
    /// <summary>
    /// 画像CCW90回転クラス I/F
    /// </summary>
    public interface IRotCCW90
    {
        /// <summary>
        /// CCW90deg回転
        /// </summary>
        void DoRotCCW90(ushort[] data, int width, int height);
        /// <summary>
        /// CCW90deg回転完了
        /// </summary>
        event EventHandler EndRotCCW90;
    }
}
