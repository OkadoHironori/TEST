using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DioLib
{
    /// <summary>
    /// GPC-2000 - Interface製 DIOボード制御用 Factoryクラス
    /// </summary>
    internal class GpcDioFactory : DioFactory
    {
        //ボード種別（固定）
        public static readonly DioBoardType Type = DioBoardType.GPC2000;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="debug">true：デバッグモード</param>
        public GpcDioFactory(bool debug)
        {
            Debug = debug;
        }

        //制御クラスのインスタンス作成
        protected override DioController CreateController(int no, int num)
        {
            //System.Diagnostics.Debug.Assert(Tools.InRange(no, byte.MinValue, byte.MaxValue));
            //System.Diagnostics.Debug.Assert(Tools.InRange(num, byte.MinValue, byte.MaxValue));

            if (Debug)
            {
                return new VirtualDioController(Convert.ToByte(no), Convert.ToByte(num), GetName(no));
            }
            else
            {
                return new GpcDioController(Convert.ToByte(no), Convert.ToByte(num));
            }
        }
        
        //名前作成
        protected override string GetName(int no)
        {
            //System.Diagnostics.Debug.Assert(Tools.InRange(no, byte.MinValue, byte.MaxValue));

            return GpcDioController.CreateName(Convert.ToByte(no));
        }
    }
}
