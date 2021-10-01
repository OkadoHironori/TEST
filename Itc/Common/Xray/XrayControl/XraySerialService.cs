using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XrayControl
{
    public class XraySerialService : IXraySerialService, IDisposable
    {
        /// <summary>
        /// X線制御命令キューの定期監視用
        /// </summary>
        private static System.Timers.Timer _Timer = null;
        /// <summary>
        /// シリアル命令周期
        /// </summary>
        private const int Interval = 20;// スレッド周期
        /// <summary>
        /// シリアルデータ受信
        /// </summary>
        public event EventHandler GetSerialParam;
        /// <summary>
        /// PLCのコマンドの順番を整理するコンカレントキュー
        /// </summary>
        private static readonly ConcurrentQueue<XrayCommand> OrderCommand = new ConcurrentQueue<XrayCommand>();
        /// <summary>
        /// シリアルポート
        /// </summary>
        public SerialPort PLCSerical { get; private set; }
        /// <summary>
        /// 応答
        /// </summary>
        public byte[] Respons { get; private set; }
        /// <summary>
        /// デバックモードから
        /// </summary>
        public bool IsDebugMode { get; set; }
        /// <summary>
        /// X線からの情報受信checker たまに
        /// </summary>
        /// <param name="xrayChecker"></param>
        private readonly IXrayCheckerFrequency _XrayCheckerFrequency;
        /// <summary>
        /// X線からの情報受信checker 頻繁に
        /// </summary>
        /// <param name="xrayChecker"></param>
        private readonly IXrayCheckerIrregularity _XrayCheckerIrregular;
        /// <summary>
        /// X線からの情報受信checker
        /// </summary>
        /// <param name="xrayChecker"></param>
        private readonly IXrayAddres_L9181_02LT _XrayAddress;
        /// <summary>
        /// X線のシリアルサービス
        /// </summary>
        /// <param name="xrayChecker"></param>
        public XraySerialService(IXrayCheckerFrequency xrayCheckerFrq, IXrayCheckerIrregularity xrayCheckerIrg, IXrayAddres_L9181_02LT Xrayaddress)
        {
            _XrayAddress = Xrayaddress;
            _XrayCheckerFrequency = xrayCheckerFrq;
            _XrayCheckerFrequency.DoInit += (s, e) =>
            {//初期化
                XrayCheckerFrequency xce = s as XrayCheckerFrequency;
                foreach(var cmd in xce.XraytoPCCmd)
                {
                    if (!IsDebugMode)
                    {
                        ClearCmd();
                        PLCSerical.Write($"{cmd.Cmd}{Environment.NewLine}");
                        Task.WaitAll(Task.Delay(100));//respons waiting..
                        if (PLCSerical.BytesToRead == 0)
                        {
                            throw new Exception($"{PLCSerical.PortName} can't find!.{Environment.NewLine}");
                        }
                        cmd.ExpByteNum = PLCSerical.BytesToRead;
                    }
                    else
                    {
                        cmd.ExpByteNum = 6;
                    }
                }
            };
            _XrayCheckerFrequency.DoCheck += (s, e) => 
            {//ステータス定期監視
                if (!IsDebugMode)
                {
                    var cmds = s as XrayCheckerFrequency;
                    var tmpcmdbit = new XrayCommand()
                    {
                        Command = $"{cmds.Cmd_XrayToPC}{Environment.NewLine}",
                        ExpectNum = cmds.ExpByteNum,
                    };
                    OrderCommand.Enqueue(tmpcmdbit);
                }
            };

            _XrayCheckerIrregular = xrayCheckerIrg;
            _XrayCheckerIrregular.DoInit += (s, e) =>
            {//初期化
                XrayCheckerIrregularity xce = s as XrayCheckerIrregularity;
                foreach (var cmd in xce.XraytoPCCmd)
                {
                    if (!IsDebugMode)
                    {
                        ClearCmd();
                        PLCSerical.Write($"{cmd.Cmd}{Environment.NewLine}");
                        Task.WaitAll(Task.Delay(100));//respons waiting..
                        if (PLCSerical.BytesToRead == 0)
                        {
                            throw new Exception($"{PLCSerical.PortName} can't find!.{Environment.NewLine}");
                        }
                        cmd.ExpByteNum = PLCSerical.BytesToRead;
                    }
                    else
                    {
                        cmd.ExpByteNum = 6;
                    }
                }
            };
            _XrayCheckerIrregular.DoCheck += (s, e) =>
            {//ステータス定期監視
                if (!IsDebugMode)
                {
                    var cmds = s as XrayCheckerIrregularity;
                    var tmpcmdbit = new XrayCommand()
                    {
                        Command = $"{cmds.Cmd_XrayToPC}{Environment.NewLine}",
                        ExpectNum = cmds.ExpByteNum,
                    };
                    OrderCommand.Enqueue(tmpcmdbit);
                }
            };

        }
        /// <summary>
        /// シリアルポートオープン
        /// </summary>
        public void OpenSerialPort()
        {
            if (!IsDebugMode)
            {
                PLCSerical = new SerialPort()
                {
                    PortName = "COM1",
                    BaudRate = 38400,
                    Parity = Parity.None,
                    DataBits = 8,
                    StopBits = StopBits.One,
                    WriteBufferSize = 512,
                    ReadBufferSize = 1024,
                    DtrEnable = true,
                    RtsEnable = true,
                };
                try
                {
                    PLCSerical.Open();
                }
                catch
                {
                    throw new Exception($"{PLCSerical.PortName} seem to be use!.{Environment.NewLine}");
                }

                ClearCmd();
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

                if(!IsDebugMode)
                {
                    ClearCmd();
                    PLCSerical?.Close();
                    PLCSerical?.Dispose();
                    PLCSerical = null;
                }
            }
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            this._XrayCheckerIrregular?.Stop();
            this._XrayCheckerFrequency?.Stop();

            if (_Timer != null)
            {
                _Timer.Stop();
                _Timer.Enabled = false;
                _Timer.Dispose();
                _Timer = null;

                if (!IsDebugMode)
                {
                    ClearCmd();
                    PLCSerical.Close();
                    PLCSerical.Dispose();
                    PLCSerical = null;
                }
            }
        }
        /// <summary>
        /// PLCの読込・書込開始
        /// </summary>
        public void Start()
        {
            OpenSerialPort();

            _XrayCheckerFrequency.Start();

            _XrayCheckerIrregular.Start();

            _Timer = new System.Timers.Timer()
            {
                Interval = Interval,
                AutoReset = false,//再突入防止策
            };

            _Timer.Elapsed += delegate
            {
                while (OrderCommand.Count != 0)
                {
                    if (OrderCommand.TryDequeue(out XrayCommand command))
                    {
                        ClearCmd();

                        Task.WaitAll(Task.Delay(Interval));

                        DoSerialWrite(command);
                    }
                    Task.WaitAll(Task.Delay(Interval));
                }
                _Timer?.Start();
            };
            _Timer?.Start();
        }
        /// <summary>
        /// 指示
        /// </summary>
        /// <returns></returns>
        public void DoCommand(string cmd)
        {
            var tmpcmdbit = new XrayCommand()
            {
                Command = $"{cmd}{Environment.NewLine}",
                ExpectNum = 0,
            };
            OrderCommand.Enqueue(tmpcmdbit);
        }
        /// <summary>
        /// データ送受信(Bit用)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="expextcnt"></param>
        /// <param name="bit">True:bit False:Word</param>
        /// <returns></returns>
        private async Task ReadEventDataFromPLC(string command, int expextcnt)
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
            var originalWriteTimeout = PLCSerical.WriteTimeout;
            var originalReadTimeout = PLCSerical.ReadTimeout;


            //if (expectnum != 0)
            //{
            //    Task.WaitAll(Task.Delay(TimeSpan.FromMilliseconds(50)));//respons waiting..
            //    if (PLCSerical.BytesToRead == 0)
            //    {
            //        throw new Exception($"{nameof(XraySerialService)} has error");
            //    }
            //    else
            //    {
            //        expectnum = PLCSerical.BytesToRead;
            //    }
            //}

            try
            {
                // Start logical request.
                PLCSerical.WriteTimeout = (int)Math.Max((timeout - stopwatch.Elapsed).TotalMilliseconds, 0);
                PLCSerical.Write(message);

                if (expectnum == 0)
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
                Debug.WriteLine($"{nameof(XraySerialService)}{Environment.NewLine}{expectnum.ToString()}{Environment.NewLine}{e.ToString()}");
                return null;
            }
            finally
            {
                // Restore SerialPort state.
                PLCSerical.ReadTimeout = originalReadTimeout;
                PLCSerical.WriteTimeout = originalWriteTimeout;
            }

        }
        /// <summary>
        /// SerialWrite
        /// </summary>
        private void DoSerialWrite(XrayCommand cmd)
        {
            if (!IsDebugMode)
            {
                if (cmd.ExpectNum != 0)
                {
                    Task.WaitAll(Task.Run(() => ReadEventDataFromPLC(cmd.Command, cmd.ExpectNum)));
                    GetSerialParam?.Invoke(this, new EventArgs());
                }
                else
                {
                    Task.WaitAll(Task.Run(() => ReadEventDataFromPLC(cmd.Command, 0)));
                }
            }
            else
            {
                //if (cmd.ExpectNum != 0)
                //{
                    //var res = cmd.Command.Substring(0, 3);
                    //string resp = $"{res} 0{Environment.NewLine}";
                    Respons = System.Text.Encoding.GetEncoding("shift_jis").GetBytes(cmd.Command);
                    GetSerialParam?.Invoke(this, new EventArgs());
                //}
                //else
                //{
                //    var res = cmd.Command.Substring(0, 3);
                //    Debug.WriteLine($"{res}");
                //}
            }
        }
        /// <summary>
        /// Command Clear 
        /// </summary>
        private void ClearCmd()
        {
            if (!IsDebugMode)
            {
                //送受信クリア 
                PLCSerical?.DiscardInBuffer();
                PLCSerical?.DiscardOutBuffer();
            }
            else
            {
                //Debug.WriteLine($"クリアコマンドでエラー発生");
            }
        }

        /// <summary>
        /// PLCのコマンド
        /// </summary>
        internal class XrayCommand
        {
            /// <summary>
            /// コマンド
            /// </summary>
            internal string Command { get; set; }
            /// <summary>
            /// 送信後に取得できるbyte[]配列の量
            /// </summary>
            internal int ExpectNum { get; set; }
        }
    }

    public interface IXraySerialService
    {
        /// <summary>
        /// シリアルデータ受信
        /// </summary>
        event EventHandler GetSerialParam;
        /// <summary>
        /// 開始
        /// </summary>
        /// <returns></returns>
        void Start();
        /// <summary>
        /// 指示
        /// </summary>
        /// <returns></returns>
        void DoCommand(string cmd);
        /// <summary>
        /// 停止
        /// </summary>
        void Stop();
    }
}
