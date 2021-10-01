using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectorControl
{
    /// <summary>
    /// 諧調変換I/F
    /// </summary>
    public class LUTableUS : ILUTableUS
    {
        /// <summary>
        /// LUTの更新
        /// </summary>
        public event EventHandler UpdateLUT;
        /// <summary>
        /// LUTTable本体
        /// </summary>
        public byte[] Lookuptbl { get; private set; }
        /// <summary>
        /// ウィンドウ幅
        /// </summary>
        public int WW { get; private set; }
        /// <summary>
        /// ウィンドウレベル
        /// </summary>
        public int WL { get; private set; }
        /// <summary>
        /// ガンマ値
        /// </summary>
        public float Gamma { get; private set; }
        /// <summary>
        /// 最大CT値 -8192 or 
        /// </summary>
        public int MaxValue { get; private set; } = ushort.MaxValue;
        /// <summary>
        /// 最小CT値 8191
        /// </summary>
        public int MinValue { get; private set; } = ushort.MinValue;
        /// <summary>
        /// ウィンドウ幅自動調整の係数
        /// </summary>
        public float Coeff { get; private set; } = 0.5F;
        /// <summary>
        /// 検出器データ
        /// </summary>
        private readonly IDetectorData _DetData;
        /// <summary>
        /// ルックアップテーブルクラス
        /// </summary>
        /// <param name="detdata"></param>
        public LUTableUS(IDetectorData detdata)
        {
            _DetData = detdata;
            _DetData.SendArrayData += (s, e) => 
            {
                var dd = s as DetectorData;

                var max = dd.ArrayData.Max();

                var min = dd.ArrayData.Min();

                WL = (max + min) / 2;

                WW = (int)((max - min) * Coeff);

                MakeLookupTable(WL, WW, 1.0F);

                UpdateLUT?.Invoke(this, new EventArgs());
            };
        }

        /// <summary>
        /// ルックアップテーブル作成
        /// </summary>
        /// <param name="wl"></param>
        /// <param name="ww"></param>
        /// <param name="gamma"></param>
        /// <returns></returns>
        private void MakeLookupTable(int wl, int ww, float gamma)
        {
            WL = wl;
            WW = ww;
            Gamma = gamma;

            float Rate = (float)byte.MaxValue;
            int Low = wl - (int)(ww / 2.0 + 0.5) - (int)MinValue;
            int High = wl + (int)(ww / 2.0 + 0.5) - (int)MinValue;

            //ビットマップ生成用ルックアップテーブル
            Lookuptbl = new byte[MaxValue - MinValue + 1];
            for (int i = 0; i < Lookuptbl.Length; i++)
            {
                if (i < Low)
                    Lookuptbl[i] = byte.MinValue;
                else if (i > High)
                    Lookuptbl[i] = byte.MaxValue;
                else
                {
                    Lookuptbl[i] = (byte)(Rate * Math.Pow(((double)(i - Low) / ww), (double)(1 / gamma)));
                }
            }
        }
        /// <summary>
        /// LUT要求
        /// </summary>
        public void RequestLUT()
        {
            UpdateLUT?.Invoke(this, new EventArgs());
        }
        public void SetWWWL(int ww, int wl)
        {
            WL = wl;
            WW = ww;
            Gamma = 1F;
            MakeLookupTable(WL, WW, Gamma);
            UpdateLUT?.Invoke(this, new EventArgs());
        }
    }
    /// <summary>
    /// 諧調変換I/F
    /// </summary>
    public interface ILUTableUS
    {
        /// <summary>
        /// LUTable変更通知
        /// </summary>
        event EventHandler UpdateLUT;
        /// <summary>
        /// LUTの要求
        /// </summary>
        void RequestLUT();
        /// <summary>
        /// LUTの変更
        /// </summary>
        void SetWWWL(int ww, int wl);
    }
}
