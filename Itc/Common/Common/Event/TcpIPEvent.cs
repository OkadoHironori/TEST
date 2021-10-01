namespace Itc.Common.Event
{
    using System;
    using System.Diagnostics;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    //using TcpClientCommandReceivedEventArgsHandler = Action<object, TcpClientCommandReceivedEventArgs>;
    /// <summary>
    /// TCPクライアント指令文字列受信イベント引数クラス
    /// </summary>
    public class TcpClientCommandReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// TCPクライアント指令文字列
        /// </summary>
        private string m_TcpClientCommand = string.Empty;

        /// <summary>
        /// TCPサーバー応答文字列
        /// </summary>
        private string m_TcpServerResponse = string.Empty;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="command">TCPクライアント指令文字列</param>
        public TcpClientCommandReceivedEventArgs(string command)
            : base()
        {
            m_TcpClientCommand = command;
        }


        /// <summary>
        /// TCPクライアント指令文字列
        /// </summary>
        public string TcpClientCommand
        {
            get
            {
                return m_TcpClientCommand;
            }
        }

        /// <summary>
        /// TCPサーバー応答文字列
        /// </summary>
        public string TcpServerResponse
        {
            get
            {
                return m_TcpServerResponse;
            }
            set
            {
                m_TcpServerResponse = value;
            }
        }
    }
    /// <summary>
    /// TCPクライアント指令文字列受信イベント・ハンドラ・デリゲート
    /// </summary>
    /// <param name="sender">イベント発生元オブジェクト</param>
    /// <param name="e">イベント引数</param>
    public delegate void TcpClientCommandReceivedEventHandler(object sender, TcpClientCommandReceivedEventArgs e);
    /// <summary>
    /// TCPサーバー・ソケット通信スレッド・クラス
    /// </summary>
    /// <remarks>TCPクライアントからの指令を受信すると、イベントを発生します。</remarks>
    public class TcpServerSocketThread : BaseThreadEx
    {
        /// <summary>
        /// TCPサーバー応答エラー文字列
        /// </summary>
        protected const string RESPONSING_ERROR = "ERR";


        /// <summary>
        /// TCPサーバー・ソケット・オブジェクト
        /// </summary>
        protected TcpServerSocket m_TcpServerSocket = null;

        /// <summary>
        /// ホスト名
        /// </summary>
        protected string m_HostName = string.Empty;

        /// <summary>
        /// ポート番号
        /// </summary>
        private int m_PortNumber = 0;

        /// <summary>
        /// 送信タイムアウト[msec]
        /// </summary>
        private int m_SendTimeout = 3000;

        /// <summary>
        /// 送信バッファ・サイズ
        /// </summary>
        private int m_SendBufferSize = 8192;

        /// <summary>
        /// 受信タイムアウト[msec]
        /// </summary>
        private int m_ReceiveTimeout = 3000;

        /// <summary>
        /// 受信バッファ・サイズ
        /// </summary>
        private int m_ReceiveBufferSize = 8192;

        /// <summary>
        /// 終端文字
        /// </summary>
        private string m_NewLine = "\r";

        /// <summary>
        /// エンコーディング指定
        /// </summary>
        private Encoding m_Encoding = Encoding.ASCII;

        /// <summary>
        /// 並行処理オブジェクト
        /// </summary>
        protected SimpleThread m_Concurrent = new SimpleThread();

        /// <summary>
        /// TCPクライアント指令文字列受信イベント・ハンドラ
        /// </summary>
        private TcpClientCommandReceivedEventHandler m_TcpClientCommandReceived = null;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TcpServerSocketThread()
            : base()
        {
        }


        /// <summary>
        /// TCPサーバー・ソケット・オブジェクト
        /// </summary>
        public TcpServerSocket TcpServerSocket
        {
            get
            {
                return m_TcpServerSocket;
            }
        }

        /// <summary>
        /// ホスト名
        /// </summary>
        public string HostName
        {
            get
            {
                return m_HostName;
            }
            set
            {
                if (!IsAlive)
                {
                    m_HostName = value;
                }
            }
        }

        /// <summary>
        /// ポート番号
        /// </summary>
        public int PortNumber
        {
            get
            {
                return m_PortNumber;
            }
            set
            {
                if (!IsAlive)
                {
                    m_PortNumber = value;
                }
            }
        }

        /// <summary>
        /// 送信タイムアウト(msec)
        /// </summary>
        public int SendTimeout
        {
            get
            {
                return m_SendTimeout;
            }
            set
            {
                if (!IsAlive)
                {
                    m_SendTimeout = (value >= 0) ? value : 0;
                }
            }
        }

        /// <summary>
        /// 送信バッファ・サイズ
        /// </summary>
        public int SendBufferSize
        {
            get
            {
                return m_SendBufferSize;
            }
            set
            {
                if (!IsAlive)
                {
                    m_SendBufferSize = value;
                }
            }
        }

        /// <summary>
        /// 受信タイムアウト(msec)
        /// </summary>
        public int ReceiveTimeout
        {
            get
            {
                return m_ReceiveTimeout;
            }
            set
            {
                if (!IsAlive)
                {
                    m_ReceiveTimeout = (value >= 0) ? value : 0;
                }
            }
        }

        /// <summary>
        /// 受信バッファ・サイズ
        /// </summary>
        public int ReceiveBufferSize
        {
            get
            {
                return m_ReceiveBufferSize;
            }
            set
            {
                if (!IsAlive)
                {
                    m_ReceiveBufferSize = value;
                }
            }
        }

        /// <summary>
        /// 終端文字
        /// </summary>
        public string NewLine
        {
            get
            {
                return m_NewLine;
            }
            set
            {
                if (!IsAlive)
                {
                    m_NewLine = value;
                }
            }
        }

        /// <summary>
        /// エンコーディング指定
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                return m_Encoding;
            }
            set
            {
                if (!IsAlive)
                {
                    m_Encoding = value;
                }
            }
        }

        /// <summary>
        /// TCPクライアント指令文字列受信イベント
        /// </summary>
        public event TcpClientCommandReceivedEventHandler TcpClientCommandReceived
        {
            add
            {
                m_TcpClientCommandReceived += value;
            }
            remove
            {
                m_TcpClientCommandReceived -= value;
            }
        }


        /// <summary>
        /// TCPクライアント指令文字列受信イベント発生
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected void RaiseTcpClientCommandReceivedEvent(object sender, TcpClientCommandReceivedEventArgs e)
        {
            if (m_TcpClientCommandReceived != null)
            {
                m_TcpClientCommandReceived(this, e);
            }
        }


        /// <summary>
        /// スレッド起動
        /// </summary>
        /// <param name="host_name">ホスト名</param>
        /// <param name="socket_port">ソケット・ポート番号</param>
        /// <param name="encoding">エンコーディング指定(既定:US-ASCII)</param>
        /// <param name="arg">ThreadMainメソッドへ渡す引数</param>
        public virtual void Start(string host_name, int socket_port, Encoding encoding = null, object arg = null)
        {
            // ソケット通信設定
            if (host_name != null)
            {
                HostName = host_name;
            }
            if (socket_port > 0)
            {
                PortNumber = socket_port;
            }
            if (encoding != null)
            {
                Encoding = encoding;
            }

            //スレッド起動
            base.Start(arg);
        }

        /// <summary>
        /// 並行処理監視タイマー時間経過コールバック
        /// </summary>
        /// <param name="state">補正制御動作内容</param>
        private void TimerCallback(object state)
        {
            // 並行処理終了確認
            if (!m_Concurrent.IsAlive)
            {
                // 並行処理タイマー停止
                m_Concurrent.StopTimer();

                // 並行処理終了
                m_Concurrent.Terminate();
            }
        }


        /// <summary>
        /// スレッド・メイン関数
        /// </summary>
        /// <param name="arg">引数(未使用)</param>
        protected override void ThreadMain(object arg)
        {
            try
            {
                // スレッド・ループ
                bool exit_loop = false;
                while (!exit_loop)
                {
                    // スレッド終了要求確認
                    if (ExitRequest)
                    {
                        exit_loop = true;
                        continue;
                    }

                    // スレッド・キャンセル監視
                    CancelMonitor();

                    Thread.Sleep(SleepTime);

                    try
                    {
                        // TCPサーバー・ソケット・オブジェクト生成
                        m_TcpServerSocket = new TcpServerSocket(m_HostName, m_PortNumber);
                        m_TcpServerSocket.SendTimeout = m_SendTimeout;
                        m_TcpServerSocket.SendBufferSize = m_SendBufferSize;
                        m_TcpServerSocket.ReceiveTimeout = m_ReceiveTimeout;
                        m_TcpServerSocket.ReceiveBufferSize = m_ReceiveBufferSize;
                        m_TcpServerSocket.NewLine = m_NewLine;
                        m_TcpServerSocket.Encoding = m_Encoding;

                        // ソケット通信接続待ち
                        m_TcpServerSocket.Listen();

                        // ソケット通信接続要求存在確認
                        while (!m_TcpServerSocket.Pending)
                        {
                            // スレッド終了要求確認
                            if (ExitRequest)
                            {
                                exit_loop = true;
                                break;
                            }

                            // スレッド・キャンセル監視
                            CancelMonitor();

                            Thread.Sleep(SleepTime);
                        }
                        if (exit_loop)
                        {
                            continue;
                        }

                        // ソケット通信受け入れ
                        m_TcpServerSocket.Accept();

                        // ソケット通信接続中
                        while (m_TcpServerSocket.Connected)
                        {
                            // スレッド終了要求確認
                            if (ExitRequest)
                            {
                                exit_loop = true;
                                break;
                            }

                            // スレッド・キャンセル監視
                            CancelMonitor();

                            Thread.Sleep(10);

                            // TCPクライアント指令文字列受信待ち
                            if (m_TcpServerSocket.Available <= 0)
                            {
                                continue;
                            }

                            // TCPクライアント指令文字列受信
                            string command = m_TcpServerSocket.ReadLine();
                            if (string.IsNullOrEmpty(command))
                            {
                                continue;
                            }

                            // タイマー開始
                            Stopwatch timer = new Stopwatch();
                            timer.Start();

                            // TCPサーバー応答文字列取得
                            string response = RESPONSING_ERROR;
                            bool available = false;
                            if (m_TcpClientCommandReceived != null)
                            {
                                // 並行処理開始
                                TcpClientCommandReceivedEventArgs e = new TcpClientCommandReceivedEventArgs(command);
                                m_Concurrent.Start(
                                    () => {
                                        // TCPクライアント指令文字列受信イベント発生
                                        RaiseTcpClientCommandReceivedEvent(this, e);
                                    }
                                );

                                // 並行処理監視タイマー開始
                                m_Concurrent.StartTimer(TimerCallback, null, 10, 10);

                                // 並行処理終了待ち
                                available = true;
                                while (m_Concurrent.IsAlive)
                                {
                                    m_Concurrent.Callback();
                                    if (ExitRequest)
                                    {
                                        available = false;
                                        exit_loop = true;
                                        break;
                                    }
                                    if (!m_TcpServerSocket.Connected)
                                    {
                                        available = false;
                                        break;
                                    }
                                    if (m_SendTimeout > 0 && timer.ElapsedMilliseconds > (long)m_SendTimeout)
                                    {
                                        available = false;
                                        break;
                                    }
                                    CancelMonitor();
                                    Thread.Sleep(10);
                                }
                                if (string.IsNullOrEmpty(e.TcpServerResponse))
                                {
                                    available = false;
                                }
                                if (available)
                                {
                                    // TCPサーバー応答文字列取得成功
                                    response = e.TcpServerResponse;
                                }
                                else
                                {
                                    // 並行処理監視タイマー停止
                                    m_Concurrent.StopTimer();

                                    // 並行処理強制終了
                                    m_Concurrent.Terminate(0);

                                    // TCPサーバー応答文字列取得失敗
                                    response = RESPONSING_ERROR;
                                }
                            }

                            // タイマー開始
                            timer.Stop();

                            // TCPサーバー応答文字列送信
                            m_TcpServerSocket.WriteLine(response);
                        }
                        if (exit_loop)
                        {
                            continue;
                        }
                    }
                    //catch (SocketException ex)
                    //{
                    //    if (ex.Message != "ソケット通信接続要求失敗")
                    //    {
                    //        //modApplication.TraceException(ex);
                    //    }
                    //}
                    catch (Exception ex)
                    {
                        throw new Exception($"ソケット通信接続要求失敗{Environment.NewLine}{ex.Message}");
                        //modApplication.TraceException(ex);
                    }
                    finally
                    {
                        // TCPサーバー・ソケット・オブジェクト破棄
                        if (m_TcpServerSocket != null)
                        {
                            m_TcpServerSocket.Dispose();
                            m_TcpServerSocket = null;
                        }
                    }
                }
            }
            catch (ThreadAbortException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception($"ThreadMain is failed{Environment.NewLine}{ex.Message}");
            }
        }
    }


    /// <summary>
    /// TCPクライアント・ソケット通信スレッド・クラス
    /// </summary>
    /// <remarks>TCPサーバーへ指令を送信して、応答を受信します。</remarks>
    public class TcpClientSocketThread : BaseThreadEx
    {
        /// <summary>
        /// TCPサーバー応答エラー文字列
        /// </summary>
        protected const string RESPONSING_ERROR = "ERR";


        /// <summary>
        /// TCPクライアント・ソケット・オブジェクト
        /// </summary>
        protected TcpClientSocket m_TcpClientSocket = null;

        /// <summary>
        /// ホスト名
        /// </summary>
        private string m_HostName = string.Empty;

        /// <summary>
        /// ポート番号
        /// </summary>
        private int m_PortNumber = 0;

        /// <summary>
        /// 送信タイムアウト[msec]
        /// </summary>
        private int m_SendTimeout = 3000;

        /// <summary>
        /// 送信バッファ・サイズ
        /// </summary>
        private int m_SendBufferSize = 8192;

        /// <summary>
        /// 受信タイムアウト[msec]
        /// </summary>
        private int m_ReceiveTimeout = 3000;

        /// <summary>
        /// 受信バッファ・サイズ
        /// </summary>
        private int m_ReceiveBufferSize = 8192;

        /// <summary>
        /// 終端文字
        /// </summary>
        private string m_NewLine = "\r";

        /// <summary>
        /// エンコーディング指定
        /// </summary>
        private Encoding m_Encoding = Encoding.ASCII;

        /// <summary>
        /// TCPクライアント指令文字列送信イベント・オブジェクト
        /// </summary>
        protected AutoResetEvent m_TcpClientCommandSending = new AutoResetEvent(false);

        /// <summary>
        /// TCPサーバー応答文字列送信イベント・オブジェクト
        /// </summary>
        protected AutoResetEvent m_TcpServerResponseReceived = new AutoResetEvent(false);

        /// <summary>
        /// TCPクライアント指令文字列
        /// </summary>
        protected string m_TcpClientCommand = string.Empty;

        /// <summary>
        /// TCPサーバー応答文字列
        /// </summary>
        protected string m_TcpServerResponse = string.Empty;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TcpClientSocketThread()
            : base()
        {
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~TcpClientSocketThread()
        {
            if (m_TcpClientCommandSending != null)
            {
                m_TcpClientCommandSending.Dispose();
                m_TcpClientCommandSending = null;
            }
            if (m_TcpServerResponseReceived != null)
            {
                m_TcpServerResponseReceived.Dispose();
                m_TcpServerResponseReceived = null;
            }
        }


        /// <summary>
        /// TCPクライアント・ソケット・オブジェクト
        /// </summary>
        public TcpClientSocket TcpClientSocket
        {
            get
            {
                return m_TcpClientSocket;
            }
        }

        /// <summary>
        /// ホスト名
        /// </summary>
        public string HostName
        {
            get
            {
                return m_HostName;
            }
            set
            {
                if (!IsAlive)
                {
                    m_HostName = value;
                }
            }
        }

        /// <summary>
        /// ポート番号
        /// </summary>
        public int PortNumber
        {
            get
            {
                return m_PortNumber;
            }
            set
            {
                if (!IsAlive)
                {
                    m_PortNumber = value;
                }
            }
        }

        /// <summary>
        /// 送信タイムアウト(msec)
        /// </summary>
        public int SendTimeout
        {
            get
            {
                return m_SendTimeout;
            }
            set
            {
                m_SendTimeout = (value >= 0) ? value : 0;
            }
        }

        /// <summary>
        /// 送信バッファ・サイズ
        /// </summary>
        public int SendBufferSize
        {
            get
            {
                return m_SendBufferSize;
            }
            set
            {
                m_SendBufferSize = value;
            }
        }

        /// <summary>
        /// 受信タイムアウト(msec)
        /// </summary>
        public int ReceiveTimeout
        {
            get
            {
                return m_ReceiveTimeout;
            }
            set
            {
                m_ReceiveTimeout = (value >= 0) ? value : 0;
            }
        }

        /// <summary>
        /// 受信バッファ・サイズ
        /// </summary>
        public int ReceiveBufferSize
        {
            get
            {
                return m_ReceiveBufferSize;
            }
            set
            {
                m_ReceiveBufferSize = value;
            }
        }

        /// <summary>
        /// 終端文字
        /// </summary>
        public string NewLine
        {
            get
            {
                return m_NewLine;
            }
            set
            {
                if (!IsAlive)
                {
                    m_NewLine = value;
                }
            }
        }

        /// <summary>
        /// エンコーディング指定
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                return m_Encoding;
            }
            set
            {
                if (!IsAlive)
                {
                    m_Encoding = value;
                }
            }
        }


        /// <summary>
        /// スレッド起動
        /// </summary>
        /// <param name="host_name">ホスト名</param>
        /// <param name="socket_port">ソケット・ポート番号</param>
        /// <param name="encoding">エンコーディング指定(既定:US-ASCII)</param>
        /// <param name="arg">ThreadMainメソッドへ渡す引数</param>
        public virtual void Start(string host_name, int socket_port, Encoding encoding = null, object arg = null)
        {
            // ソケット通信設定
            if (host_name != null)
            {
                HostName = host_name;
            }
            if (socket_port > 0)
            {
                PortNumber = socket_port;
            }
            if (encoding != null)
            {
                Encoding = encoding;
            }

            // スレッド起動
            base.Start(arg);
        }

        /// <summary>
        /// TCPクライアント指令文字列送信
        /// </summary>
        /// <param name="command">TCPクライアント指令文字列</param>
        public void Send(string command)
        {
            try
            {
                lock (this)
                {
                    // 初期化
                    m_TcpClientCommandSending.Reset();

                    // TCPクライアント指令文字列送信
                    m_TcpClientCommand = command;
                    m_TcpClientCommandSending.Set();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"TcpClientSocketThread is failed{Environment.NewLine}{ex.Message}");
                //modApplication.TraceException(ex);
            }
        }

        /// <summary>
        /// TCPサーバー応答文字列受信
        /// </summary>
        /// <returns>TCPサーバー応答文字列</returns>
        public string Receive()
        {
            string response = RESPONSING_ERROR;

            try
            {
                // TCPサーバー応答文字列受信待ち
                bool received = false;
                if (m_ReceiveTimeout > 0)
                {
                    received = m_TcpServerResponseReceived.WaitOne();
                }
                else
                {
                    received = m_TcpServerResponseReceived.WaitOne(m_ReceiveTimeout + 50);
                }
                if (received)
                {
                    // TCPサーバー応答文字列受信
                    lock (this)
                    {
                        response = m_TcpServerResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                //modApplication.TraceException(ex);
                throw new Exception($"TcpClientSocketThread is failed{Environment.NewLine}{ex.Message}");
                //response = RESPONSING_ERROR;
            }

            return response;
        }


        /// <summary>
        /// スレッド・メイン関数
        /// </summary>
        /// <param name="arg">引数(未使用)</param>
        protected override void ThreadMain(object arg)
        {
            try
            {
                // スレッド・ループ
                bool exit_loop = false;
                while (!exit_loop)
                {
                    // スレッド終了要求確認
                    if (ExitRequest)
                    {
                        exit_loop = true;
                        continue;
                    }

                    // スレッド・キャンセル監視
                    CancelMonitor();

                    Thread.Sleep(SleepTime);

                    try
                    {
                        // ホストがリモート・コンピュータである場合、コンピュータ応答確認
                        if (!string.IsNullOrEmpty(m_HostName))
                        {
                            // PING送信
                            Ping ping = new Ping(m_HostName);
                            if (!ping.CheckAlive())
                            {
                                continue;
                            }
                        }

                        // TCPクライアント・ソケット・オブジェクト生成
                        m_TcpClientSocket = new TcpClientSocket(m_HostName, m_PortNumber);
                        m_TcpClientSocket.SendTimeout = m_SendTimeout;
                        m_TcpClientSocket.SendBufferSize = m_SendBufferSize;
                        m_TcpClientSocket.ReceiveTimeout = m_ReceiveTimeout;
                        m_TcpClientSocket.ReceiveBufferSize = m_ReceiveBufferSize;
                        m_TcpClientSocket.NewLine = m_NewLine;
                        m_TcpClientSocket.Encoding = m_Encoding;

                        // ソケット通信接続中
                        while (m_TcpClientSocket.Connected)
                        {
                            // スレッド終了要求確認
                            if (ExitRequest)
                            {
                                exit_loop = true;
                                break;
                            }

                            // スレッド・キャンセル監視
                            CancelMonitor();

                            // TCPクライアント指令文字列送信イベント待ち
                            if (!m_TcpClientCommandSending.WaitOne(SleepTime))
                            {
                                continue;
                            }

                            lock (this)
                            {
                                // 初期化
                                string response = RESPONSING_ERROR;
                                m_TcpServerResponse = response;
                                m_TcpServerResponseReceived.Reset();

                                // TCPクライアント指令文字列取得
                                string command = m_TcpClientCommand;
                                if (string.IsNullOrEmpty(command))
                                {
                                    continue;
                                }

                                // TCPクライアント指令文字列送信
                                m_TcpClientSocket.WriteLine(command);

                                // タイマー開始
                                Stopwatch timer = new Stopwatch();
                                timer.Start();

                                // TCPサーバー応答文字列受信待ち
                                bool available = true;
                                while (m_TcpClientSocket.Available <= 0)
                                {
                                    if (ExitRequest)
                                    {
                                        available = false;
                                        exit_loop = true;
                                        break;
                                    }
                                    if (!m_TcpClientSocket.Connected)
                                    {
                                        available = false;
                                        break;
                                    }
                                    if (m_ReceiveTimeout > 0 && timer.ElapsedMilliseconds > (long)m_ReceiveTimeout)
                                    {
                                        available = false;
                                        break;
                                    }
                                    CancelMonitor();
                                    Thread.Sleep(10);
                                }

                                // タイマー停止
                                timer.Stop();

                                if (available)
                                {
                                    // TCPサーバー応答文字列受信
                                    response = m_TcpClientSocket.ReadLine();
                                    if (string.IsNullOrEmpty(command))
                                    {
                                        continue;
                                    }

                                    // TCPサーバー応答文字列取得
                                    m_TcpServerResponse = response;
                                }

                                // TCPサーバー応答文字列受信イベント
                                m_TcpServerResponseReceived.Set();
                            }
                        }
                        if (exit_loop)
                        {
                            continue;
                        }
                    }
                    catch (SocketException ex)
                    {
                        if (ex.Message != "ソケット通信接続要求失敗")
                        {
                            //modApplication.TraceException(ex);
                            throw new Exception($"TcpClientSocketThread is failed{Environment.NewLine}{ex.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        //modApplication.TraceException(ex);
                        throw new Exception($"TcpClientSocketThread is failed{Environment.NewLine}{ex.Message}");
                    }
                    finally
                    {
                        // TCPクライアント・ソケット・オブジェクト破棄
                        if (m_TcpClientSocket != null)
                        {
                            m_TcpClientSocket.Shutdown(SocketShutdown.Both);
                            m_TcpClientSocket.Dispose();
                            m_TcpClientSocket = null;
                        }
                    }
                }
            }
            catch (ThreadAbortException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                //modApplication.TraceException(ex);
                throw new Exception($"TcpClientSocketThread is failed{Environment.NewLine}{ex.Message}");
            }
        }
    }
}
