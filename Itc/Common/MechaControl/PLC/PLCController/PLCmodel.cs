using Itc.Common.Extensions;
using Itc.Common.TXEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCController
{
    public class PLCmodel : BindableBase
    {
        /// <summary>
        /// PLC番号 //bitとwordで被る場合がある
        /// </summary>
        public int Idx { get; set; }
        /// <summary>
        /// 要素名
        /// </summary>
        public string Element { get; set; }
        /// <summary>
        /// 状態(bool)
        /// </summary>
        private bool _boolStatus;
        /// <summary>
        /// 状態(bool)
        /// </summary>
        public bool BoolStatus
        {
            get { return this._boolStatus; }
            set { SetProperty(ref _boolStatus, value); }
        }
        /// <summary>
        /// 状態(int)
        /// </summary>
        private int _intStatus;
        /// <summary>
        /// 状態(int)
        /// </summary>
        public int IntStatus
        {
            get { return this._intStatus; }
            set { SetProperty(ref _intStatus, value); }
        }
        /// <summary>
        /// 状態(float)
        /// </summary>
        private float _floatStatus;
        /// <summary>
        /// 状態(float)
        /// </summary>
        public float FloatStatus
        {
            get { return this._floatStatus; }
            set { SetProperty(ref _floatStatus, value); }
        }
        /// <summary>
        /// スケール
        /// </summary>
        public int Scale { get; set; }
        /// <summary>
        /// 型
        /// </summary>
        public PLCVType VarType { get; set; }
        /// <summary>
        /// PLCから送信される種類
        /// </summary>
        public PLCDataType DataType { get; set; }
    }
}
