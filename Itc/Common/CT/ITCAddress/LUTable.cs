using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTAddress
{

    /// <summary>
    /// ルックアップテーブル
    /// </summary>
    public class LUTable : ILUTable
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
        public int MaxCTValue { get; private set; } = 8191;
        /// <summary>
        /// 最小CT値 8191
        /// </summary>
        public int MinCTValue { get; private set; } = -8192;
        /// <summary>
        /// データ
        /// </summary>
        private readonly ICTDattum _CTDatas;
        /// <summary>
        /// ルックアップテーブル　コンストラクタ
        /// </summary>
        /// <param name="dataInfo"></param>
        public LUTable(ICTDattum datas)
        {
            _CTDatas = datas;
            _CTDatas.EndLoadData += (s, e) =>
            {
                if (s is CTDattum di)
                {
                    Gamma = di.Gamma;
                    WW = di.WW;
                    WL = di.WL;
                    MaxCTValue = di.MaxVale;
                    MinCTValue = di.MinVale;
                    MakeLookupTable(WL, WW, Gamma);

                    UpdateLUT?.Invoke(this, new EventArgs());
                }
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
            int Low = wl - (int)(ww / 2.0 + 0.5) - (int)MinCTValue;
            int High = wl + (int)(ww / 2.0 + 0.5) - (int)MinCTValue;

            //ビットマップ生成用ルックアップテーブル
            Lookuptbl = new byte[MaxCTValue - MinCTValue + 1];
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
        }
    }
    /// <summary>
    /// ルックアップテーブル I/F
    /// </summary>
    public interface ILUTable
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
        /// 階調設定
        /// </summary>
        /// <param name="ww"></param>
        /// <param name="wl"></param>
        void SetWWWL(int ww, int wl);

    }

}
