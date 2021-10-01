using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCController
{
    /// <summary>
    /// 生存確認用
    /// </summary>
    public class PLCComChecker : IPLCComChecker, IDisposable
    {
        /// <summary>
        /// ComCheckerコマンド
        /// </summary>
        public string CmdComChecker { get; private set; }
        /// <summary>
        /// 指定したインターバル毎にたたく
        /// </summary>
        public event EventHandler DoComCheck;
        /// <summary>
        /// _PLC書込情報の取得用
        /// </summary>
        private readonly IPLCAddress _PLCAddress;
        /// <summary>
        /// _PLCチェッカのインターバル
        /// </summary>
        private const int WaitTime = 500;
        /// <summary>
        /// タイマー
        /// </summary>
        private System.Timers.Timer _Timer = null;
        /// <summary>
        /// 生存確認用
        /// </summary>
        public PLCComChecker(IPLCAddress ad)
        {
            _PLCAddress = ad;
            _PLCAddress.EndLoadCSV += (s, e) =>
            {
                var pa = s as PLCAddress;
                PCtoPLCBitTbl dev = pa.PCtoPLCMap.PCtoPLCBitTbl.ToList().Find(p => p.DevName == "CommCheck");
                if (dev == null)
                {
                    throw new Exception("can't find CommCheck DevName");
                }

                string bitdata = true ? "1" : "0";
                CmdComChecker = dev.Command + bitdata + "\r\n";

            };
            if(_Timer!=null)
            {
                _PLCAddress.RequestParam();
            }

        }
        /// <summary>
        /// 生存確認
        /// </summary>
        public void DoComChecker()
        {
            if(_Timer!=null)
            {
                Stop();
            }

            _Timer = new System.Timers.Timer()
            {
                Interval = WaitTime,
                AutoReset = false,
            };

            _Timer.Elapsed += (s, e) =>
            {

                DoComCheck?.Invoke(this, new EventArgs());
                _Timer.Start();
            };

            _Timer.Start();
        }
        public void Stop()
        {
            if (_Timer != null)
            {
                _Timer.Stop();

                Task.WaitAll(Task.Delay(WaitTime));

                _Timer?.Dispose();

                _Timer = null;
            }
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            Stop();

            Debug.WriteLine($"{nameof(PLCComChecker)} is Disposed");
        }
    }
    public interface IPLCComChecker
    {
        event EventHandler DoComCheck;

        void DoComChecker();

        void Stop();
    }
}
