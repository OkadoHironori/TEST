using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRayControler
{
    /// <summary>
    /// IRAY社　データ収集クラス
    /// </summary>
    public class IRaySeqAcquisition: IIRaySeqAcquisition
    {
        /// <summary>
        /// データ収集完了イベント
        /// </summary>
        public event EventHandler EndAcqData;
        /// <summary>
        /// データポインタ
        /// </summary>
        public IntPtr DataPtr { get; private set; }
        /// <summary>
        /// 画像幅
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// 画像高さ
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// iRay制御I/F
        /// </summary>
        private readonly IIRayCtrl _IRayCtrl;
        /// <summary>
        /// 
        /// </summary>
        private readonly IIRaySelectMode _SelMode;
        /// <summary>
        /// IRAY社　データ収集クラス
        /// </summary>
        public IRaySeqAcquisition(IIRayCtrl irayctrl, IIRaySelectMode mode)
        {
            _IRayCtrl = irayctrl;
            _IRayCtrl.EndAcqData += (s, e) =>
            {
                IRayCtrl dd = s as IRayCtrl;
                GetAcqDataForUT(dd.AcqData, Width, Height);
            };

            _SelMode = mode;
            _SelMode.ChangeMode += (s, e) => 
            {
                IRaySelectMode md = s as IRaySelectMode;
                Width = md.Width;
                Height = md.Height;
            };
            _SelMode.RequestMode();
        }

        public void GetAcqDataForUT(IntPtr dataPtr, int width, int height)
        {
            Width = width;
            Height = height;
            DataPtr = dataPtr;
            EndAcqData?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// 校正条件の設定
        /// </summary>
        public void SetCorrection()
        {
            _IRayCtrl.SetCorrection();
        }
        /// <summary>
        /// データ収集開始
        /// </summary>
        public void DoAcqData()
        {
            _IRayCtrl.DoDataAcq();
        }
        /// <summary>
        /// データ収集停止
        /// </summary>
        public void DoStopAcqData()
        {
            _IRayCtrl.DoStopDataAcq();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IIRaySeqAcquisition
    {
        void GetAcqDataForUT(IntPtr intPtr, int width, int height);
        /// <summary>
        /// データ収集完了イベント
        /// </summary>
        event EventHandler EndAcqData;
        /// <summary>
        /// データ収集開始
        /// </summary>
        void DoAcqData();
        /// <summary>
        /// データ収集停止
        /// </summary>
        void DoStopAcqData();
        /// <summary>
        /// 校正の設定
        /// </summary>
        void SetCorrection();
    }
}
