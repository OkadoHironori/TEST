using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectorControl
{
    /// <summary>
    /// CW90回転クラス
    /// </summary>
    public class RotCW90 : IRotCW90
    {
        /// <summary>
        /// CCW90回転
        /// </summary>
        public event EventHandler EndRotCW90;
        /// <summary>
        /// 設定I/F
        /// </summary>
        private readonly IDetConf _DetConf;
        /// <summary>
        /// CCW90回転
        /// </summary>
        public bool CW90 { get; private set; }
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
        /// CW90回転クラス
        /// </summary>
        public RotCW90(IDetConf conf)
        {
            _DetConf = conf;
            _DetConf.EndLoadParam += (s, e) =>
            {
                var dc = s as DetConf;
                CW90 = dc.CW90;
            };
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

                yy = width -(width - 1 - xx)-1;
                xx = height -(int)(idx / width)-1;

                Targets[xx + (yy * height)] = data[idx];
            }

            EndRotCW90?.Invoke(this, new EventArgs());
        }
    }
    /// <summary>
    /// 画像CCW90回転クラス I/F
    /// </summary>
    public interface IRotCW90
    {
        /// <summary>
        /// CCW90deg回転
        /// </summary>
        void DoRotCW90(ushort[] data, int width, int height);
        /// <summary>
        /// CCW90deg回転完了
        /// </summary>
        event EventHandler EndRotCW90;
    }

}
