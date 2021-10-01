
using Itc.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itc.Common.Basic
{
    /// <summary>
    /// NumUpDown系の表示用クラス
    /// </summary>
    public class NumBase
    {
        /// <summary>
        /// 最大
        /// </summary>
        public float Max { get; private set; }
        /// <summary>
        /// 最小
        /// </summary>
        public float Min { get; private set; }
        /// <summary>
        /// 現在地
        /// </summary>
        public float Cur { get; private set; }
        /// <summary>
        /// Decimal位置
        /// </summary>
        public int DeciPosi { get; private set; }
        /// <summary>
        /// インクリメント
        /// </summary>
        public decimal Increment { get; private set; }
        /// <summary>
        /// コンストラクタ(decimal) 本当はT型にして共通化を図るべき
        /// </summary>
        /// <param name="max">最大</param>
        /// <param name="min">最小</param>
        /// <param name="cur">数値</param>
        /// <param name="deci">有効桁数</param>
        public NumBase(decimal max, decimal min, float cur, int deci, decimal increment) 
        {
            Max = (float)max;
            Min = (float)min;
            Cur = cur.CorrectRange((float)min, (float)max);
            DeciPosi = deci;
            Increment = increment;
        }
        /// <summary>
        /// コンストラクタ(float) 本当はT型にして共通化を図るべき
        /// </summary>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="cur"></param>
        /// <param name="deci"></param>
        public NumBase(float max, float min, float cur, int deci, decimal increment)
        {
            Max = (float)max;
            Min = (float)min;
            Cur = cur.CorrectRange((float)min, (float)max);
            DeciPosi = deci;
            Increment = increment;
        }
    }

    /// <summary>
    /// NumUpDown系の表示用クラス
    /// </summary>
    public class ResBase
    {
        /// <summary>
        /// 結果 値
        /// </summary>
        public float ResValue { get; set; }
        /// <summary>
        /// 結果
        /// </summary>
        public bool Res { get; set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="b"></param>
        /// <param name="d"></param>
        public ResBase(bool b, float d)
        {
            Res = b;
            ResValue = d;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ResBase()
        {

        }
    }
}
