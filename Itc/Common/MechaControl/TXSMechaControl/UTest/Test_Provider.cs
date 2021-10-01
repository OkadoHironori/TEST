

namespace TXSMechaControl.UTest
{
    using Board.BoardControl;
    using Dio.DioController;
    using Itc.Common.Extensions;
    using Itc.Common.TXEnum;
    using PLCController;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TXSMechaControl.Common;

    /// <summary>
    /// PLCからの指令
    /// </summary>
    public class Somthing_Provider : BindableBase, IDisposable
    {
        /// <summary>
        /// 回転位置
        /// </summary>
        private float _StsRot;
        /// <summary>
        /// 回転位置
        /// </summary>
        public float StsRot
        {
            get { return this._StsRot; }
            set { SetProperty(ref _StsRot, value); }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Somthing_Provider()
        {

        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
