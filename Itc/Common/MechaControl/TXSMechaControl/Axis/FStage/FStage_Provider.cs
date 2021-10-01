using Board.BoardControl;
using Dio.DioController;
using Itc.Common.Extensions;
using Itc.Common.TXEnum;
using PLCController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TXSMechaControl.FStage
{
    public class FStage_Provider : BindableBase, IDisposable
    {
        /// <summary>
        /// 微調X軸 微調Y軸 運転速度
        /// </summary>
        private float _stsSpeed;
        /// 微調X軸 微調Y軸 運転速度
        /// </summary>
        public float StsSpeed
        {
            get { return this._stsSpeed; }
            set { SetProperty(ref _stsSpeed, value); }
        }
        /// <summary>
        /// 微調X位置
        /// </summary>
        private float _StsFXPosi;
        /// <summary>
        /// 微調X位置
        /// </summary>
        public float StsFXPosi
        {
            get { return this._StsFXPosi; }
            set { SetProperty(ref _StsFXPosi, value); }
        }
        /// <summary>
        /// 微調Y位置
        /// </summary>
        private float _StsFYPosi;
        /// <summary>
        /// 微調Y位置
        /// </summary>
        public float StsFYPosi
        {
            get { return this._StsFYPosi; }
            set { SetProperty(ref _StsFYPosi, value); }
        }
        /// <summary>
        /// ボードの微調X軸制御
        /// </summary>
        private readonly IBoardXStage _BoardFX;
        /// <summary>
        /// ボードの微調X軸制御
        /// </summary>
        private readonly IBoardYStage _BoardFY;
        /// <summary>
        /// PLC制御
        /// </summary>
        private readonly IPLCControl _PLC;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pLC"></param>
        /// <param name="diocnt"></param>
        /// <param name="boardud"></param>
        public FStage_Provider(IPLCControl plc, IBoardXStage xs, IBoardYStage ys)
        {
            _PLC = plc;
            _BoardFX = xs;
            _BoardFY = ys;

            //PLC変更
            _PLC.StatusChanged += (s, e) =>
            {
                var stsname = (s as PLCmodel).Element;

                switch (stsname)
                {
                    case ("stsXStgSpeed"):
                        StsSpeed = (s as PLCmodel).FloatStatus;
                        break;
                    case ("stsYStgSpeed"):
                        StsSpeed = (s as PLCmodel).FloatStatus;
                        break;
                }
            };

            //ボード変更
            _BoardFX.NotifyCount += (s, e) =>
            {
                StsFXPosi = e.NumValue;
            };

            //ボード変更
            _BoardFY.NotifyCount += (s, e) =>
            {
                StsFYPosi = e.NumValue;
            };
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {

        }
    }
}
