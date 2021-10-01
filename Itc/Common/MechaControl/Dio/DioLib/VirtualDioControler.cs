using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Threading;

namespace DioLib
{
    /// <summary>
    /// 仮想DIOボード制御クラス（デバッグ用）
    /// </summary>
    public class VirtualDioController : DioController
    {
        private DioMappedMemoryAccessor diAccessor;
        private DioMappedMemoryAccessor doAccessor;

        //コンストラクタ
        internal VirtualDioController(byte boardno, byte bitsnum, string mappedname)
        {
            this.No = boardno;
            this.BitsNum = bitsnum;

            diAccessor = new DioMappedMemoryAccessor(bitsnum, mappedname + DioLib.Properties.Resources.DI_NAME_SUFFIX);
            doAccessor = new DioMappedMemoryAccessor(bitsnum, mappedname + DioLib.Properties.Resources.DO_NAME_SUFFIX);
        }

        public override void GetDi(bool[] bits)
        {
            Debug.Assert(null != diAccessor);

            base.GetDi(bits);

            diAccessor.GetAllBits(bits);
        }
        public override bool GetDi(uint index)
        {
            Debug.Assert(null != diAccessor);

            base.GetDi(index);
            
            return diAccessor.GetBit(index);
        }
        public override void SetDo(bool[] bits)
        {
            Debug.Assert(null != doAccessor);

            base.SetDo(bits);
            
            doAccessor.SetAllBits(bits);
        }
        public override void SetDo(uint index, bool value)
        {
            Debug.Assert(null != doAccessor);

            base.SetDo(index, value);
            
            doAccessor.SetBit(value, index);
        }

        #region IDisposable
        protected override void FreeManagedResource()
        {
            if (null != diAccessor)
            {
                diAccessor.Dispose();
                diAccessor = null;
            }

            if (null != doAccessor)
            {
                doAccessor.Dispose();
                doAccessor = null;
            }
        }
        #endregion
    }
}
