using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCController
{
    /// <summary>
    /// PLC生存確認クラス
    /// </summary>
    public class PLCChecker : IPLCChecker, IDisposable
    {
        /// <summary>
        /// タイプ
        /// </summary>
        public PLCDataType CurrentType { get; private set; }
        /// <summary>
        /// PLCに接続しているか？
        /// </summary>
        //public bool IsConnectPLC { get; private set; }
        /// <summary>
        /// PLCの読込マップ
        /// </summary>
        public IEnumerable<ReadPLCMap> ReadPLCMap { get; private set; }
        /// <summary>
        /// PCからPLCへの書き込みマップ
        /// </summary>
        public PCtoPLCMap PCtoPLCMap { get; private set; }
        /// <summary>
        /// PLCの初期化
        /// </summary>
        public ReadPLCMap InitPLCMap { get; private set; }
        /// <summary>
        /// チェックイベント
        /// </summary>
        public event EventHandler DoCheck;
        /// <summary>
        /// 初期調整
        /// </summary>
        //public event EventHandler DoInit;
        /// <summary>
        /// _PLC書込情報の取得用
        /// </summary>
        private readonly IPLCAddress _PLCAddress;
        /// <summary>
        /// _PLCサーバー
        /// </summary>
        //private readonly IPLCServer _PLCServer;
        /// <summary>
        /// _PLCチェッカのインターバル
        /// </summary>
        private const int WaitTime = 100;
        /// <summary>
        /// タイマー
        /// </summary>
        private System.Timers.Timer _Timer = null;
        /// <summary>
        /// PLCの値を取得する順番
        /// </summary>
        private IEnumerable<string> CycleMode;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PLCChecker(IPLCAddress address)
        {
            //_PLCServer = server;

            _PLCAddress = address;
            _PLCAddress.EndLoadCSV += (s, e) =>
            {
                var plad = s as PLCAddress;

                ReadPLCMap = plad.ReadPLCMap;
                PCtoPLCMap = plad.PCtoPLCMap;

                List<string> tmpMode = new List<string>();

                foreach (var mode in ReadPLCMap)
                {
                    if (mode.PLCDataType == PLCDataType.Non) { continue; }
                    tmpMode.Add((mode.PLCDataType.ToString()));
                }
                CycleMode = tmpMode;
            };
            if(CycleMode==null)_PLCAddress.RequestParam();

        }
        //public void Init()
        //{
        //    foreach(var mode in CycleMode)
        //    {

        //        DoInit?.Invoke(this, new EventArgs());
        //    }
        //}
        /// <summary>
        /// 生存確認の開始
        /// </summary>
        public void Start()
        {
            // _PLCServer.Init();
            //_PLCServer.Start();
            
            //PCtoPLCBitTbl dev = PCtoPLCMap.PCtoPLCBitTbl.ToList().Find(p => p.DevName == "CommCheck");
            //if (dev == null)
            //{
            //    throw new Exception("can't find CommCheck DevName");
            //}

            //string bitdata = true ? "1" : "0";
            //string senddata = dev.Command + bitdata + "\r\n";

            _Timer = new System.Timers.Timer()
            {
                Interval = WaitTime,
                AutoReset = false,
            };

            int Idx = 0;
            //同期しない（1回1回が重たいので）
            _Timer.Elapsed += (s, e) =>
            {
                if (Enum.TryParse(CycleMode.ElementAt(Idx), out PLCDataType type))
                {
                    //Debug.WriteLine($"時間{DateTime.Now.TimeOfDay},Mode{type.ToString()},Idx {Idx.ToString()}");

                    CurrentType = type;

                    DoCheck?.Invoke(this, new EventArgs());
                }
                else
                {
                    throw new Exception($"{nameof(PLCChecker)} has error");
                }

                Idx %= CycleMode.Count();

                Idx++;

                if(Idx == CycleMode.Count()) {
                    Idx = 0;

                }



                _Timer?.Start();
            };

            _Timer?.Start();


            //IsConnectPLC = true;


        }
        /// <summary>
        /// PLCチェッカーの停止
        /// </summary>
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
            if (_Timer != null)
            {
                _Timer.Stop();

                Task.WaitAll(Task.Delay(WaitTime));
                _Timer?.Dispose();

                _Timer = null;
            }
        }
    }
    /// <summary>
    /// PLC生存確認クラス IF
    /// </summary>
    public interface IPLCChecker
    {
        /// <summary>
        /// PLC生存確認開始
        /// </summary>
        void Start();
        /// <summary>
        /// PLC生存確認停止
        /// </summary>
        /// <param name="waittime"></param>
        void Stop();
        /// <summary>
        /// チェックイベント
        /// </summary>
        event EventHandler DoCheck;
        ///// <summary>
        ///// 初期調整
        ///// </summary>
        //event EventHandler DoInit;
    }
}
