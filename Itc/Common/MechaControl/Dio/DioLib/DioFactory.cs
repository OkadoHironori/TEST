using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DioLib
{
    //抽象クラス //Abstract Factory Pattern
    public abstract class DioFactory
    {
        //デバッグモード
        public bool Debug { get; protected set; }

        /// <summary>
        /// Factoryインスタンスを取得
        /// </summary>
        /// <param name="boardtype"></param>
        /// <param name="isDebug"></param>
        /// <returns></returns>
        public static DioFactory GetFactory(DioBoardType boardtype, bool isDebug = false)
        {
            switch (boardtype)
            {
                case DioBoardType.GPC2000:
                    return new GpcDioFactory(isDebug);

                default:
                    //未実装
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// DIOインスタンス作成 
        /// </summary>
        /// <param name="no"></param>
        /// <param name="bitsnum"></param>
        /// <returns></returns>
        public Dio CreateDio(int no, int bitsnum)
        {
            DioMappedMemoryAccessor dimem = CreateAccessor(bitsnum, GetName(no) + DioLib.Properties.Resources.DI_NAME_SUFFIX);

            DioMappedMemoryAccessor domem = CreateAccessor(bitsnum, GetName(no) + DioLib.Properties.Resources.DO_NAME_SUFFIX);

            DioController dioctrl = CreateController(no, bitsnum);

            return new Dio(dioctrl, dimem, domem);
        }

        /// <summary>
        /// 文字列作成
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        protected abstract string GetName(int no);

        /// <summary>
        /// 共有メモリ作成
        /// </summary>
        /// <param name="num"></param>
        /// <param name="mappedname"></param>
        /// <returns></returns>
        protected virtual DioMappedMemoryAccessor CreateAccessor(int num, string mappedname)
        {
            return new DioMappedMemoryAccessor(num, mappedname);
        }

        //コントローラー作成
        protected abstract DioController CreateController(int no, int num);
    }

}