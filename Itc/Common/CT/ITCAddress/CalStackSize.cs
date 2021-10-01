
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTAddress
{
    /// <summary>
    /// 縦断面画素数計算サービス
    /// </summary>
    public class CalStackSize: ICalStackSize
    {
        /// <summary>
        /// 縦断面数の計算完了
        /// </summary>
        public event EventHandler EndCalStack;
        /// <summary>
        /// 縦断面画素数
        /// </summary>
        public int DispStackSize { get; private set;}
        /// <summary>
        /// FOV変換サービス
        /// </summary>
        private readonly IFOVConverter _FOVConv;
        /// <summary>
        /// コンストラクタ 
        /// </summary>
        /// <param name="service"></param>
        public CalStackSize(IFOVConverter service)
        {
            _FOVConv = service;
            _FOVConv.EndInputFov += (s, e) => 
            {
                if (s is FOVConverter fovcon)
                {
                    DispStackSize = CalCmpStackNum(fovcon.SlicePitch, fovcon.ImportDotSize, fovcon.ImportFiles.Count());
                    EndCalStack?.Invoke(this, new EventArgs());
                }
            };
        }
        /// <summary>
        /// 1画素サイズとスライスピッチとインポートするファイル数から縦方向の画像サイズを求める
        /// </summary>
        /// <param name="isSinglemode"></param>
        /// <param name="slicepitch"></param>
        /// <param name="dotsize"></param>
        /// <param name="filecnt"></param>
        private int CalCmpStackNum(float slicepitch, float dotsize, int filecnt)
        {
            if (filecnt == 1)
            {   //1枚読込の場合
                return 1;
            }
            else
            {
                float tempStackSize = slicepitch / dotsize * (filecnt-1);
                return (int)Math.Round(tempStackSize, 0, MidpointRounding.AwayFromZero);
            }
        }
    };

    /// <summary>
    /// 縦断面画素数計算サービス
    /// </summary>  
    public interface ICalStackSize
    {
        event EventHandler EndCalStack;
    }
}
