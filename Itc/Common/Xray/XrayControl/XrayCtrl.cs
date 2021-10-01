using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XrayControl
{
    /// <summary>
    /// X線制御サービス
    /// </summary>
    public class XrayCtrlService : IXrayCtrlService
    {
        /// <summary>
        /// PCからX線源
        /// </summary>
        private readonly IPCToXray _PCToXray;
        /// <summary>
        /// X線源からPC
        /// </summary>
        private readonly IXrayToPC _XrayToPC;
        /// <summary>
        /// X線制御サービス
        /// </summary>
        public XrayCtrlService(IXrayToPC xraytopc, IPCToXray pctoxray)
        {
            _XrayToPC = xraytopc;
            _XrayToPC.PropertyChanged += (s, e) => 
            {

            };
            _PCToXray = pctoxray;
        }
        /// <summary>
        /// X線ON
        /// </summary>
        public void DoXrayON() => _PCToXray.DoXrayOn();
        /// <summary>
        /// X線ON
        /// </summary>
        public void DoXrayOFF() => _PCToXray.DoXrayOFF();
        /// <summary>
        /// X線セットでON
        /// </summary>
        public void DoSetVoltCurrentFocus(int volt, int current, string focus)
        {
            _PCToXray.DoSetFocus(focus);
            _PCToXray.DoSetVolate(volt);
            _PCToXray.DoSetCurrent(current);
            _PCToXray.DoXrayOn();
        }
    }
    /// <summary>
    /// X線制御サービス I/F
    /// </summary>
    public interface IXrayCtrlService
    {
        /// <summary>
        /// X線オン
        /// </summary>
        void DoXrayON();
        /// <summary>
        /// X線オフ
        /// </summary>
        void DoXrayOFF();
        /// <summary>
        /// X線セット
        /// </summary>
        void DoSetVoltCurrentFocus(int volt, int current, string focus);
    }
}
