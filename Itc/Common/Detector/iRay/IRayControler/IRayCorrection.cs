using iDetector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRayControler
{
    /// <summary>
    /// IRay社の校正情報取得
    /// </summary>
    public class IRayCorrection: IIRayCorrection
    {
        /// <summary>
        /// モード名
        /// </summary>
        public string CurrentMode { get; private set; }
        /// <summary>
        /// オフセット有効状態
        /// </summary>
        public int OffsetValidityState { get; private set; }
        /// <summary>
        /// ゲイン有効状態
        /// </summary>
        public int GainValidityState { get; private set; }
        /// <summary>
        /// 欠陥校正有効状態
        /// </summary>
        public int DefectValidityState { get; private set; }
        /// <summary>
        /// 校正状態の有効・無効
        /// </summary>
        public string CurrentCorrection { get; private set; }
        /// <summary>
        /// オフセット校正有効か?
        /// </summary>
        public bool OffsetEnable { get; private set; }
        /// <summary>
        /// 校正完了
        /// </summary>
        public event EventHandler EndCorrection;
        /// <summary>
        /// 
        /// </summary>
        private readonly IIRaySelectMode _IRayMode;

        private readonly IIRayCtrl _IRayCtrl;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IRayCorrection(IIRayCtrl irayctrl, IIRaySelectMode mode)
        {
            _IRayCtrl = irayctrl;

            _IRayMode = mode;
            _IRayMode.ChangeMode += (s, e) =>
            {
                var irsm = s as IRaySelectMode;
                OffsetValidityState = irsm.OffsetValidityState;
                GainValidityState = irsm.GainValidityState;
                DefectValidityState = irsm.DefectValidityState;
                CurrentMode = irsm.SelectMode;
                CurrentCorrection = irsm.CurrentCorrectMode;
                string[] offsetA = CurrentCorrection.ToString().ToCharArray().Select(c => c.ToString()).ToArray();

                int offsetvalue = (int)Enm_CorrectOption.Enm_CorrectOp_SW_PreOffset;
                var offsetvalues = Convert.ToString(offsetvalue, 2);
                string[] offset = offsetvalues.ToString().ToCharArray().Select(c => c.ToString()).ToArray();

                //if (offsetA[offsetA.Length - offset.Length] == "1")
                //{
                //    OffsetEnable = true;
                //}
                //else
                //{
                //    OffsetEnable = false;
                //}


            };
            _IRayMode.GetModeInf();

            _IRayMode.RequestMode();
        }
        /// <summary>
        /// 校正初期化
        /// </summary>
        public void DoInitCorrection()
        {
            _IRayMode.GetModeInf();
            _IRayMode.RequestMode();

            _IRayCtrl.DoInitCorrection();
            _IRayCtrl.DoAcqOffset();

            _IRayMode.GetModeInf();
            _IRayMode.RequestMode();

            EndCorrection?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// 状態取得
        /// </summary>
        public void RequestState()
        {
            EndCorrection?.Invoke(this, new EventArgs());
        }
    }
    public interface IIRayCorrection
    {
        /// <summary>
        /// 校正実行
        /// </summary>
        void DoInitCorrection();
        /// <summary>
        /// 状態取得
        /// </summary>
        void RequestState();
        /// <summary>
        /// 校正完了
        /// </summary>
        event EventHandler EndCorrection;
    }
}
