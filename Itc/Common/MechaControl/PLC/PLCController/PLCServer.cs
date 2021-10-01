using Itc.Common.Event;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PLCController
{
    /// <summary>
    /// PLCサーバーサービスクラス
    /// TODO もっときれいになると思う
    /// </summary>
    public class PLCServer : IPLCServer, IDisposable
    {
        /// <summary>
        /// 読み取りロック用
        /// </summary>
        private readonly object balanceLock = new object();
        /// <summary>
        /// シリアルポート
        /// </summary>
        public SerialPort PLCSerical { get; private set; }
        /// <summary>
        /// シリアル開通
        /// </summary>
        public bool IsSerialConnect { get; private set; }
        /// <summary>
        /// 応答
        /// </summary>
        public byte[] Respons { get; private set; }
        /// <summary>
        /// コマンドタイプ
        /// </summary>
        public string CmdType { get; private set;}
        /// <summary>
        /// PLCの読込マップ
        /// </summary>
        public IEnumerable<ReadPLCMap> ReadPLCMap { get; private set; }
        /// <summary>
        /// PCからPLCへの書き込みマップ
        /// </summary>
        public PCtoPLCMap PCtoPLCMap { get; private set; }
        /// <summary>
        /// PLC_Bitデータ読込イベント
        /// </summary>
        public event EventHandler PLCBitMessage;
        /// <summary>
        /// PLC_Wordデータ読込イベント
        /// </summary>
        public event EventHandler PLCWordMessage;
        /// <summary>
        /// PLC開始イベント
        /// </summary>
        public event EventHandler PLCStart;
        /// <summary>
        /// PLCのコマンドの順番を整理するコンカレントキュー
        /// </summary>
        private static readonly ConcurrentQueue<PLCCommand> OrderCommand = new ConcurrentQueue<PLCCommand>();
        /// <summary>
        /// Dioの定期監視用
        /// </summary>
        private static System.Timers.Timer _Timer = null;
        /// <summary>
        /// PLCの値チェッカ
        /// </summary>
        private readonly IPLCChecker _PLCChecker;
        /// <summary>
        /// PLCのアドレスマップ
        /// </summary>
        private readonly IPLCAddress _Address;
        /// <summary>
        /// 生存確認用
        /// </summary>
        private readonly IPLCComChecker _ComChecker;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="address"></param>
        public PLCServer(IPLCAddress address, IPLCChecker checker, IPLCComChecker comcheck)
        {
            _Address = address;
            _Address.EndLoadCSV += (s, e) =>
            {
                var pa = s as PLCAddress;
                ReadPLCMap = pa.ReadPLCMap;
                PCtoPLCMap = pa.PCtoPLCMap;
            };
            if (ReadPLCMap == null)
            {
                _Address.RequestParam();
            }

            if(Init())
            {
                _PLCChecker = checker;
                _PLCChecker.DoCheck += (s, e) =>
                {
                    var plad = s as PLCChecker;
                    PLCDataType type = plad.CurrentType;
                    ReadPLCData(type);
                };
                _PLCChecker.Start();

                _ComChecker = comcheck;
                _ComChecker.DoComCheck += (s, e) =>
                {
                    var plcc = s as PLCComChecker;
                    WriteBitPLCData(plcc.CmdComChecker);
                };
                _ComChecker.DoComChecker();
            }
        }
        /// <summary>
        /// PLCポート初期化
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            PLCSerical = new SerialPort()
            {
                PortName = "COM3",
                BaudRate = 115200,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.RequestToSend,
                DtrEnable = true,
                RtsEnable = true,
                ReadTimeout = 1000,
            };

            try
            {
                PLCSerical.Open();
            }
            catch
            {
                throw new Exception($"{PLCSerical.PortName} seem to be use!.{Environment.NewLine}");
            }

            foreach(var map in ReadPLCMap)
            {
                ClearCmd();
                Task.WaitAll(Task.Delay(50));
                PLCSerical.Write(map.PLCContext.ReadCommand);
                Task.WaitAll(Task.Delay(50));//respons waiting..
                if(PLCSerical.BytesToRead==0)
                {
                    throw new Exception($"{PLCSerical.PortName} can't find!.{Environment.NewLine}");
                }
                _Address.UpdateExpectByteNum(map.PLCDataType, PLCSerical.BytesToRead);
            }

            IsSerialConnect = true;

            return (PLCSerical.IsOpen);
        }
        /// <summary>
        /// PLCの読込・書込開始
        /// </summary>
        public void Start()
        {
            const int interval = 10;

            _Timer = new System.Timers.Timer()
            {
                Interval = interval,
                AutoReset = false,//再突入防止策
            };

            _Timer.Elapsed += delegate
            {
                while (OrderCommand.Count != 0)
                {
                    if (OrderCommand.TryDequeue(out PLCCommand command))
                    {
                        ClearCmd();

                        Task.WaitAll(Task.Delay(interval));

                        DoSerialWrite(command);
                    }
                    Task.WaitAll(Task.Delay(interval));
                }

                _Timer?.Start();
            };

            _Timer.Start();

            PLCStart?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// SerialWrite
        /// </summary>
        private void DoSerialWrite(PLCCommand cmd)
        {
            if (cmd.Dtype == PLCDataType.Non)
            {
                Task.WaitAll(Task.Run(() => ReadEventDataFromPLC(cmd.Command, 0, cmd.Dtype)));
            }
            else
            {
                var quer = ReadPLCMap.ToList().Find(p => p.PLCDataType == cmd.Dtype).PLCContext;
                if (quer != null)
                {
                    Task.WaitAll(Task.Run(() => ReadEventDataFromPLC(quer.ReadCommand, quer.ExpectByteNum, cmd.Dtype)));
                }
            }

        }
        /// <summary>
        /// データ送受信(Bit用)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="expextcnt"></param>
        /// <param name="bit">True:bit False:Word</param>
        /// <returns></returns>
        private async Task ReadEventDataFromPLC(string command, int expextcnt, PLCDataType type)
        {
            TimeSpan timeout = TimeSpan.FromMilliseconds(1000);

            var sendTask = Task.Run(() => SendMessage(command, timeout, expextcnt));
            try
            {
                await Task.WhenAny(sendTask, Task.Delay(timeout));
            }
            catch (TaskCanceledException)
            {
                throw new TimeoutException();
            }
            Respons = await sendTask;

            //失敗した場合
            if (Respons != null)
            {
                switch (type)
                {
                    case (PLCDataType.ParamBit):
                    case (PLCDataType.ParamBitFunc):
                        //CmdType = nameof(type);
                        CmdType = type.ToString();

                        PLCBitMessage?.Invoke(this, new EventArgs());
                        break;
                    case (PLCDataType.ParamFCDWord):
                    case (PLCDataType.ParamFDDWord):
                    case (PLCDataType.ParamTBLYWord):
                    case (PLCDataType.ParamWord):
                    case (PLCDataType.ParamXrayColiWord):

                        CmdType = type.ToString();

                        PLCWordMessage?.Invoke(this, new EventArgs());

                        break;
                    default:
                        throw new Exception($"{nameof(PLCServer)}Unknow CMD Type");
                }
            }
        }
        /// <summary>
        /// PLC情報の読取メッセージの送信
        /// </summary>
        /// <param name="message"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private byte[] SendMessage(string message, TimeSpan timeout, int expectnum)
        {
            // Use stopwatch to update SerialPort.ReadTimeout and SerialPort.WriteTimeout 
            // as we go.
            var stopwatch = Stopwatch.StartNew();

            // Organize critical section for logical operations using some standard .NET tool.
            lock (balanceLock)
            {
                var originalWriteTimeout = PLCSerical.WriteTimeout;
                var originalReadTimeout = PLCSerical.ReadTimeout;
                try
                {
                    // Start logical request.
                    PLCSerical.WriteTimeout = (int)Math.Max((timeout - stopwatch.Elapsed).TotalMilliseconds, 0);
                    PLCSerical.Write(message);

                    if(expectnum==0)
                    {
                        return null;
                    }

                    byte[] buffer = new byte[expectnum];
                    // Expected response length. Look for the constant value from 
                    // the device communication protocol specification or extract 
                    // from the response header (first response bytes) if there is 
                    // any specified in the protocol.
                    int count = expectnum;
                    int offset = 0;
                    // Loop until we recieve a full response.
                    while (count > 0)
                    {
                        PLCSerical.ReadTimeout = (int)Math.Max((timeout - stopwatch.Elapsed).TotalMilliseconds, 0);
                        var readCount = PLCSerical.Read(buffer, offset, count);
                        offset += readCount;
                        count -= readCount;
                    }
                    return buffer;
                }
                catch (Exception e)
                {
                    ClearCmd();
                    Debug.WriteLine($"{nameof(PLCServer)}{Environment.NewLine}{expectnum.ToString()}{Environment.NewLine}{e.ToString()}");
                    return null;
                }
                finally
                {
                    // Restore SerialPort state.
                    PLCSerical.ReadTimeout = originalReadTimeout;
                    PLCSerical.WriteTimeout = originalWriteTimeout;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        public void ReadPLCData(PLCDataType cmdtype)
        {
            ReadPLCMap quer = ReadPLCMap.ToList().Find(p => p.PLCDataType == cmdtype);

            if(quer!=null)
            {
                var tmpcmdbit = new PLCCommand()
                {
                    Command = quer.PLCContext.ReadCommand,
                    Dtype = cmdtype,
                    ExpectNum = quer.PLCContext.ExpectByteNum,
                };
                OrderCommand.Enqueue(tmpcmdbit);
            }
        }
        /// <summary>
        /// 生存確認用
        /// </summary>
        /// <param name="name"></param>
        public void WriteBitPLCData(string cmd)
        {
            var tmpcmd = new PLCCommand()
            {
                Command = cmd,
                Dtype = PLCDataType.Non,
            };

            OrderCommand.Enqueue(tmpcmd);
        }
        /// <summary>
        /// Word情報の書込
        /// </summary>
        /// <param name="name"></param>
        public void WriteWordPLCData(string cmd)
        {
            var tmpcmd = new PLCCommand()
            {
                Command = cmd,
                Dtype = PLCDataType.Non,
            };
            OrderCommand.Enqueue(tmpcmd);
        }
        /// <summary>
        /// Command Clear 
        /// </summary>
        private void ClearCmd()
        {
            //送受信クリア 
            PLCSerical?.DiscardInBuffer();
            PLCSerical?.DiscardOutBuffer();
        }
        /// <summary>
        /// PLCサービス停止
        /// </summary>
        public void Stop()
        {
            if (_Timer != null)
            {
                _PLCChecker.Stop();
                _ComChecker.Stop();

                while (OrderCommand.Count != 0)
                {
                    Task.WaitAll(Task.Delay(1000));
                }

                _Timer.Stop();
                _Timer.Enabled = false;
                _Timer.Dispose();
                _Timer = null;
            }

            if (PLCSerical.IsOpen)
            {
                PLCSerical.Close();
                Task.WaitAll(Task.Delay(500));
                PLCSerical.Dispose();
            }
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            Stop();
            //if (_Timer!=null)
            //{
            //    _Timer.Stop();
            //    _Timer.Enabled = false;
            //    _Timer.Dispose();
            //    _Timer = null;
            //    while (OrderCommand.Count != 0)
            //    {
            //        Task.WaitAll(Task.Delay(1000));
            //    }
            //}
            //if(PLCSerical!=null)
            //{                
            //    PLCSerical.Close();
            //    Task.WaitAll(Task.Delay(500));
            //    PLCSerical.Dispose();
            //}
        }
        /// <summary>
        /// PLCのコマンド
        /// </summary>
        internal class PLCCommand
        {
            /// <summary>
            /// コマンド
            /// </summary>
            internal string Command { get; set; }
            /// <summary>
            /// DataType
            /// </summary>
            internal PLCDataType Dtype { get; set; }
            /// <summary>
            /// 送信後に取得できるbyte[]配列の量
            /// </summary>
            internal int ExpectNum { get; set; }
        }
    }

    /// <summary>
    /// PLCサーバーインターフェイス
    /// </summary>
    public interface IPLCServer
    {
        /// <summary>
        /// PLCポート初期化
        /// </summary>
        /// <returns></returns>
        //bool Init();
        /// <summary>
        /// PLCサービス開始
        /// </summary>
        void Start();
        /// <summary>
        /// PLCサービス停止
        /// </summary>
        void Stop();
        /// <summary>
        /// PLCのデータ取得(全)
        /// </summary>
        void ReadPLCData(PLCDataType cmdtype);
        /// <summary>
        /// Bit情報の書込
        /// </summary>
        /// <param name="name"></param>
        void WriteBitPLCData(string cmd);
        /// <summary>
        /// Word情報の書込
        /// </summary>
        /// <param name="name"></param>
        void WriteWordPLCData(string cmd);
        /// <summary>
        /// PLC_Bitデータ読込イベント
        /// </summary>
        event EventHandler PLCBitMessage;
        /// <summary>
        /// PLC_Wordデータ読込イベント
        /// </summary>
        event EventHandler PLCWordMessage;
        /// <summary>
        /// PLC開始イベント
        /// </summary>
        event EventHandler PLCStart;
    }
}
