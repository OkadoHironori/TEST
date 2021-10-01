using IRayControler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectorControl
{
    /// <summary>
    /// 
    /// </summary>
    public class DetectorCtrl: IDetectorCtrl
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler StateChange;
        /// <summary>
        /// 
        /// </summary>
        private readonly IIRayCorrection _IRayCorrection;
        /// <summary>
        /// 
        /// </summary>
        public DetectorCtrl(IIRayCorrection correction)
        {
            _IRayCorrection = correction;
            _IRayCorrection.EndCorrection += (s, e) =>
            {
                StateChange?.Invoke(this, new EventArgs());
            };
            _IRayCorrection.RequestState();
        }

        public void QueryState()
        {
            _IRayCorrection.RequestState();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IDetectorCtrl
    {
        event EventHandler StateChange;

        void QueryState();
    }
}
