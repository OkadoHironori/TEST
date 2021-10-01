using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRayControler
{
    /// <summary>
    /// 収集した画像の制御
    /// </summary>
    public class IRayImageCtrl: IIRayImageCtrl
    {
        /// <summary>
        /// データ収集クラスI/F
        /// </summary>
        private readonly IIRaySeqAcquisition _SeqAcquisition;
        /// <summary>
        /// 画像90degCCW回転
        /// </summary>
        public bool RotCCW_90 { get; private set; } = IRayParam.Default.RotCCW_90;
        /// <summary>
        /// 画像90degCW回転
        /// </summary>
        public bool RotCW_90 { get; private set; } = IRayParam.Default.RotCW_90;

        public IRayImageCtrl(IIRaySeqAcquisition acq)
        {
            _SeqAcquisition = acq;
            _SeqAcquisition.EndAcqData += (s, e) => 
            {


            };
        }
        /// <summary>
        /// 画像90deg_CCW_回転(CW有効の場合は無効になる)
        /// </summary>
        public void ChangeRotCCW_90()
        {
            if (RotCW_90)
            {
                RotCW_90 = !RotCW_90;
                IRayParam.Default.RotCW_90 = RotCW_90;
                IRayParam.Default.Save();
            }

            RotCCW_90 = !RotCCW_90;
            IRayParam.Default.RotCCW_90 = RotCCW_90;
            IRayParam.Default.Save();
        }
        /// <summary>
        /// 画像90deg_CW_回転(CCW有効の場合は無効になる)
        /// </summary>
        public void ChangeRotCW_90()
        {
            if (RotCCW_90)
            {
                RotCCW_90 = !RotCCW_90;
                IRayParam.Default.RotCCW_90 = RotCCW_90;
                IRayParam.Default.Save();
            }

            RotCW_90 = !RotCW_90;
            IRayParam.Default.RotCW_90 = RotCW_90;
            IRayParam.Default.Save();
        }


        public void SetPtrForUT(IntPtr intPtr)
        {

        }
    }
    public interface IIRayImageCtrl
    {
        /// <summary>
        /// UT用
        /// </summary>
        /// <param name="intPtr"></param>
        void SetPtrForUT(IntPtr intPtr);
        /// <summary>
        /// 画像_CCW方向に90deg反転
        /// </summary>
        void ChangeRotCCW_90();
        /// <summary>
        /// 画像_CW方向に90deg反転
        /// </summary>
        void ChangeRotCW_90();

    }


}
