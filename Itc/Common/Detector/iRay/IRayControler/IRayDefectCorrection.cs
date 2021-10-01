using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRayControler
{
    /// <summary>
    /// IRay欠陥校正クラス
    /// </summary>
    public class IRayDefectCorrection : IIRayDefectCorrection
    {
        /// <summary>
        /// 欠陥校正完了
        /// </summary>
        public event EventHandler EndDefectCorrection;
        /// <summary>
        /// IRay欠陥校正クラス
        /// </summary>
        public IRayDefectCorrection()
        {
              
        }
        /// <summary>
        /// 
        /// </summary>
        public void DoDefectCorrection()
        {
            EndDefectCorrection?.Invoke(this, new EventArgs());
        }
    }
    /// <summary>
    /// IRay欠陥校正クラス I/F
    /// </summary>
    public interface IIRayDefectCorrection
    {
        /// <summary>
        /// 欠陥校正完了
        /// </summary>
        event EventHandler EndDefectCorrection;
        /// <summary>
        /// 欠陥校正実行
        /// </summary>
        void DoDefectCorrection();

    }
}
