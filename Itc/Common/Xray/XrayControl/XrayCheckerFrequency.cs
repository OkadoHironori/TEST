using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XrayControl
{
    /// <summary>
    /// X線定期監視クラス
    /// </summary>
    public class XrayCheckerFrequency : IXrayCheckerFrequency, IDisposable
    {
        /// <summary>
        /// L9181_02LT 130kV浜松ホト
        /// </summary>
        private readonly IXrayAddres_L9181_02LT _XrayAddress;
        /// <summary>
        /// 定期チェック
        /// </summary>
        public event EventHandler DoCheck;
        /// <summary>
        /// シリアル通信の初期化
        /// </summary>
        public event EventHandler DoInit;
        /// <summary>
        /// インターバル
        /// </summary>
        private readonly int CheckerInterval = 100;
        /// <summary>
        /// タイマー
        /// </summary>
        private System.Timers.Timer _Timer = null;
        /// <summary>
        /// X線源へのコマンド
        /// </summary>
        public string Cmd_XrayToPC { get; private set; }
        /// <summary>
        /// バイト数
        /// </summary>
        public int ExpByteNum { get; private set; }
        /// <summary>
        /// XrayからPCのコマンド
        /// </summary>
        public IEnumerable<XraytoPC> XraytoPCCmd { get; private set; }
        /// <summary>
        /// X線定期監視クラス
        /// </summary>
        public XrayCheckerFrequency(IXrayAddres_L9181_02LT xrayaddress)
        {

            _XrayAddress = xrayaddress;
            _XrayAddress.EndLoadFiles += (s, e) => 
            {
                var xa = s as XrayAddres_L9181_02LT;
                XraytoPCCmd = xa.XraytoPCCmdFreq;
            };
            if(XraytoPCCmd==null)
            {
                _XrayAddress.RequestParam();
            }
        }
        /// <summary>
        /// 開始
        /// </summary>
        public void Start()
        {
            DoInit?.Invoke(this, new EventArgs());

            _Timer = new System.Timers.Timer()
            {
                Interval = CheckerInterval,
                AutoReset = false,
            };

            int Idx = 0;
            //同期しない（1回1回が重たいので）
            _Timer.Elapsed += (s, e) =>
            {

                var cmd = XraytoPCCmd.ElementAt(Idx);

                Cmd_XrayToPC = cmd.Cmd;
                ExpByteNum = cmd.ExpByteNum;
                DoCheck?.Invoke(this, new EventArgs());

                Idx++;

                if(XraytoPCCmd.Count()==Idx)
                {
                    Idx = 0;
                }

                _Timer.Start();
            };

            _Timer.Start();
        }

        /// <summary>
        /// PLCチェッカーの停止
        /// </summary>
        public void Stop()
        {
            if (_Timer != null)
            {
                _Timer.Enabled = false;

                _Timer.Stop();

                Task.WaitAll(Task.Delay(CheckerInterval * 2));

                _Timer?.Dispose();

                _Timer = null;
            }
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            if (_Timer != null)
            {
                this.Stop();
            }
        }
    }
    /// <summary>
    /// X線定期監視クラス
    /// </summary>
    public interface IXrayCheckerFrequency
    {
        /// <summary>
        /// 停止
        /// </summary>
        void Stop();
        /// <summary>
        /// スタート
        /// </summary>
        void Start();
        /// <summary>
        /// チェック
        /// </summary>
        event EventHandler DoCheck;
        /// <summary>
        /// シリアル通信の初期化
        /// </summary>
        event EventHandler DoInit;


    }

}
