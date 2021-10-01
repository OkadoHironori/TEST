using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXSMechaControl.BusyCtrl
{
    public interface IBusyCtrl
    {
        bool IsBusy { get; }
        /// <summary>
        /// 
        /// </summary>
        void DoMove();
       
    }
}
