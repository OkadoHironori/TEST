/*
  
	共通ライブラリ     ソケット通信クラス
 
 
 
	(c) Copyright  TOSHIBA IT & Control Systems Corporation 2017, All Rights Reserved

    History:
        Date        Version     Explanation                             Modifier        
        -------------------------------------------------------------------------------
        2018/05/28  0.0.0.0     コーディング完成                        (AIT)M.KOIKE
 
*/
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;

namespace Itc.Common.Event
{
    /// <summary>
    /// ソケット例外クラス
    /// </summary>
    public class SocketException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SocketException()
            : base()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public SocketException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="sender">例外発生元オブジェクト</param>
        public SocketException(string message, object sender)
            : base(message)
        {
            this.Data.Add("SENDER", sender);
        }

        /// <summary>
        /// 例外発生元オブジェクト
        /// </summary>
        public object Sender
        {
            get
            {
                return this.Data["SENDER"];
            }
        }
    }

    /// <summary>
    /// ソケット・ポート・クラス
    /// </summary>
    [Serializable]
    public abstract class SocketPort : IDisposable
    {
        /// <summary>
        /// ターミネータ
        /// </summary>
        private string m_NewLine = "\r";

        /// <summary>
        /// エンコーディング
        /// </summary>
        private Encoding m_Encoding = Encoding.ASCII;

        /// <summary>
        /// 最大リトライ回数
        /// </summary>
        private int m_MaximumRetryCount = 0;

        /// <summary>
        /// 通信中断
        /// </summary>
        private bool m_Abort = false;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected SocketPort()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="encoding">エンコーディング指定(既定:US-ASCII)</param>
        protected SocketPort(Encoding encoding)
        {
            if (encoding != null)
            {
                m_Encoding = encoding;
            }
        }

        #region "IDisposable Support"
        /// <summary>
        /// リソース解放済みフラグ
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// リソース解放
        /// </summary>
        /// <param name="disposing">マネージ・オブジェクト破棄指定</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // マネージ・オブジェクト破棄
                    this.Close();
                }

                // アンマネージ・オブジェクト解放
            }
            this.disposedValue = true;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~SocketPort()
        {
            Dispose(false);
        }

        /// <summary>
        /// リソース解放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// 終端文字列
        /// </summary>
        public string NewLine
        {
            get
            {
                return m_NewLine;
            }
            set
            {
                if (value != null)
                {
                    m_NewLine = value;
                }
            }
        }

        /// <summary>
        /// エンコーディング
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                return m_Encoding;
            }
            set
            {
                if (value != null)
                {
                    m_Encoding = value;
                }
            }
        }

        /// <summary>
        /// 最大リトライ回数
        /// </summary>
        public int MaximumRetryCount
        {
            get
            {
                return m_MaximumRetryCount;
            }
            set
            {
                m_MaximumRetryCount = (value >= 0) ? value : 0;
            }
        }

        /// <summary>
        /// 通信中断
        /// </summary>
        public bool Abort
        {
            get
            {
                return m_Abort;
            }
            set
            {
                m_Abort = value;
            }
        }

        /// <summary>
        /// ソケット通信データ送信
        /// </summary>
        /// <param name="data">送信データ</param>
        public abstract void Write(byte[] data);

        /// <summary>
        /// ソケット通信文字列送信
        /// </summary>
        /// <param name="message">送信文字列</param>
        public abstract void WriteLine(string message);

        /// <summary>
        /// ソケット通信データ受信
        /// </summary>
        /// <returns>受信データ</returns>
        public abstract byte[] Read();

        /// <summary>
        /// ソケット通信文字列受信
        /// </summary>
        /// <returns>受信文字列</returns>
        public abstract string ReadLine();

        /// <summary>
        /// ソケット通信切断
        /// </summary>
        public virtual void Close()
        {
            m_Abort = false;
        }
    }

    /// <summary>
    /// TCP/IPサーバー・ソケット・クラス
    /// </summary>
    [Serializable]
    public class TcpServerSocket : SocketPort, ICommunicationPort, IDisposable
    {
        /// <summary>
        /// TCPサーバー・オブジェクト
        /// </summary>
        protected TcpListener m_TcpServer = null;

        /// <summary>
        /// TCPクライアント・オブジェクト
        /// </summary>
        protected TcpClient m_TcpClient = null;

        /// <summary>
        /// ネットワークス・ストリーム・オブジェクト
        /// </summary>
        protected NetworkStream m_NetworkStream = null;

        /// <summary>
        /// ホスト名
        /// </summary>
        private string m_HostName = string.Empty;

        /// <summary>
        /// ポート番号
        /// </summary>
        private int m_PortNumber = 0;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TcpServerSocket()
            : base()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="encoding">エンコーディング指定(既定:US-ASCII)</param>
        public TcpServerSocket(Encoding encoding)
            : base(encoding)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host_name">ホスト名</param>
        /// <param name="socket_port">ソケット・ポート番号</param>
        /// <param name="encoding">エンコーディング指定(既定:US-ASCII)</param>
        public TcpServerSocket(string host_name, int socket_port, Encoding encoding = null)
            : base(encoding)
        {
            //排他処理
            lock (this)
            {
                try
                {
                    IPAddress ip_address = IPAddress.Any;

                    if (host_name == null)
                    {
                        host_name = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(host_name))
                    {
                        try
                        {
                            //IPアドレスが指定されている場合
                            ip_address = IPAddress.Parse(host_name);
                        }
                        catch
                        {
                            //ホスト名が指定されている場合
                            ip_address = Dns.GetHostEntry(host_name).AddressList[0];
                        }
                    }

                    //サーバー・ソケット・オブジェクト生成
                    m_TcpServer = new TcpListener(ip_address, socket_port);

                    m_HostName = host_name;
                    m_PortNumber = socket_port;
                }
#if DEBUG
                catch (Exception ex)
                {
                    //modApplication.TraceException(ex);
                    throw new SocketException($"ソケット通信接続要求失敗{Environment.NewLine}{ex.ToString()}", this);
                }
#else
                catch
                {
                    throw new SocketException($"ソケット通信接続要求失敗{Environment.NewLine}", this);
                }
#endif
            }
        }

        /// <summary>
        /// TCPサーバー・オブジェクト
        /// </summary>
        public TcpListener TcpServer
        {
            get
            {
                return m_TcpServer;
            }
        }

        /// <summary>
        /// TCPクライアント・オブジェクト
        /// </summary>
        public TcpClient TcpClient
        {
            get
            {
                return m_TcpClient;
            }
        }

        /// <summary>
        /// ネットワーク・ストリーム
        /// </summary>
        public NetworkStream NetworkStream
        {
            get
            {
                return m_NetworkStream;
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
                if (!Connected)
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
                if (!Connected)
                {
                    m_PortNumber = value;
                }
            }
        }

        /// <summary>
        /// ポート名
        /// </summary>
        public string PortName
        {
            get
            {
                return string.Format("{0}:{1}", Dns.GetHostAddresses(m_HostName)[0], m_PortNumber);
            }
        }

        /// <summary>
        /// 接続確認
        /// </summary>
        public bool Connected
        {
            get
            {
                bool value = false;
                if (m_TcpClient != null)
                {
                    value = (m_TcpClient.Connected && (!m_TcpClient.Client.Poll(1000, SelectMode.SelectRead) || m_TcpClient.Client.Available > 0));
                }
                return value;
            }
        }

        /// <summary>
        /// 読み取り可能データ量
        /// </summary>
        public int Available
        {
            get
            {
                int value = 0;
                if (m_TcpClient != null)
                {
                    value = m_TcpClient.Available;
                }
                return value;
            }
        }

        /// <summary>
        /// 接続要求存在確認
        /// </summary>
        public bool Pending
        {
            get
            {
                bool value = false;
                if (m_TcpServer != null)
                {
                    value = m_TcpServer.Pending();
                }
                return value;
            }
        }

        /// <summary>
        /// 送信タイムアウト(msec)
        /// </summary>
        public int SendTimeout
        {
            get
            {
                int value = 0;
                if (m_TcpClient != null)
                {
                    value = m_TcpClient.SendTimeout;
                }
                return value;
            }
            set
            {
                if (m_TcpClient != null)
                {
                    m_TcpClient.SendTimeout = (value >= 0) ? value : 0;
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
                int value = 0;
                if (m_TcpClient != null)
                {
                    value = m_TcpClient.SendBufferSize;
                }
                return value;
            }
            set
            {
                if (m_TcpClient != null)
                {
                    m_TcpClient.SendBufferSize = value;
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
                int value = 0;
                if (m_TcpClient != null)
                {
                    value = m_TcpClient.ReceiveTimeout;
                }
                return value;
            }
            set
            {
                if (m_TcpClient != null)
                {
                    m_TcpClient.ReceiveTimeout = (value >= 0) ? value : 0;
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
                int value = 0;
                if (m_TcpClient != null)
                {
                    value = m_TcpClient.ReceiveBufferSize;
                }
                return value;
            }
            set
            {
                if (m_TcpClient != null)
                {
                    m_TcpClient.ReceiveBufferSize = value;
                }
            }
        }

        /// <summary>
        /// ソケット通信接続ポート・オープン
        /// </summary>
        /// <param name="host_name">ホスト名</param>
        /// <param name="socket_port">ソケット・ポート番号</param>
        public virtual void Bind(string host_name, int socket_port)
        {
            //排他処理
            lock (this)
            {
                try
                {
                    IPAddress ip_address = IPAddress.Any;

                    if (host_name == null)
                    {
                        host_name = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(host_name))
                    {
                        try
                        {
                            //IPアドレスが指定されている場合
                            ip_address = IPAddress.Parse(host_name);
                        }
                        catch
                        {
                            //ホスト名が指定されている場合
                            ip_address = Dns.GetHostEntry(host_name).AddressList[0];
                        }
                    }

                    //サーバー・ソケット・オブジェクト生成
                    m_TcpServer = new TcpListener(ip_address, socket_port);

                    m_HostName = host_name;
                    m_PortNumber = socket_port;
                }
                catch (Exception ex)
                {
                    //modApplication.TraceException(ex);
                    throw new SocketException($"ソケット通信接続準備失敗{Environment.NewLine}{ex.ToString()}", this);
                }

                this.Abort = false;
            }
        }

        /// <summary>
        /// ソケット通信接続待ち
        /// </summary>
        public virtual void Listen()
        {
            //排他処理
            lock (this)
            {
                if (m_TcpServer != null)
                {
                    try
                    {
                        m_TcpServer.Start();
                    }
                    catch (Exception ex)
                    {
                        //modApplication.TraceException(ex);
                        throw new SocketException($"ソケット通信接続待ち失敗{Environment.NewLine}{ex.ToString()}", this);
                    }
                }
                else
                {
                    throw new SocketException("サーバー・ソケット通信オブジェクト未生成", this);
                }
            }
        }

        /// <summary>
        /// ソケット通信接続受付
        /// </summary>
        public virtual void Accept()
        {
            //排他処理
            lock (this)
            {
                if (m_TcpServer != null)
                {
                    try
                    {
                        m_TcpClient = m_TcpServer.AcceptTcpClient();
                        m_NetworkStream = m_TcpClient.GetStream();
                    }
                    catch (Exception ex)
                    {
                        //modApplication.TraceException(ex);
                        throw new SocketException($"ソケット通信接続受入失敗{Environment.NewLine}{ex.ToString()}", this);
                    }
                }
                else
                {
                    throw new SocketException("サーバー・ソケット通信オブジェクト未生成", this);
                }
            }
        }

        /// <summary>
        /// ソケット通信データ送信
        /// </summary>
        /// <param name="data">送信データ</param>
        public override void Write(byte[] data)
        {
            //排他処理
            lock (this)
            {
                if (m_TcpClient != null && m_NetworkStream != null)
                {
                    if (m_NetworkStream.CanWrite)
                    {
                        try
                        {
                            if (data != null && data.Length > 0)
                            {
                                m_NetworkStream.Write(data, 0, data.Length);
                            }
                        }
                        catch (Exception ex)
                        {
                            //modApplication.TraceException(ex);
                            throw new SocketException($"ソケット通信送信失敗{Environment.NewLine}{ex.ToString()}", this);
                        }
                    }
                    else
                    {
                        throw new SocketException("クライアント・ソケット通信未接続", this);
                    }
                }
                else
                {
                    throw new SocketException("クライアント・ソケット通信未接続", this);
                }
            }
        }

        /// <summary>
        /// ソケット通信文字列送信
        /// </summary>
        /// <param name="message">送信文字列</param>
        public override void WriteLine(string message)
        {
            this.Write(this.Encoding.GetBytes(message + this.NewLine));
        }

        /// <summary>
        /// ソケット通信データ受信
        /// </summary>
        /// <returns>受信データ</returns>
        public override byte[] Read()
        {
            byte[] data = new byte[0];

            //排他処理
            lock (this)
            {
                if (m_TcpClient != null && m_NetworkStream != null)
                {
                    if (m_NetworkStream.CanRead)
                    {
                        try
                        {
                            byte[] newline = this.Encoding.GetBytes(this.NewLine);

                            byte[] buffer = new byte[1024];
                            int length = 0;
                            int count = 0;
                            bool terminated = false;
                            int i = 0;
                            int j = 0;

                            for (int retry = 0; retry <= this.MaximumRetryCount; retry++)
                            {
                                if (this.Abort)
                                {
                                    terminated = true;
                                    break;
                                }

                                count = m_NetworkStream.Read(buffer, 0, buffer.Length);
                                if (count > 0)
                                {
                                    Array.Resize<byte>(ref data, length + count);
                                    Array.Copy(buffer, 0, data, length, count);
                                    length = data.Length;

                                    if (newline == null)
                                    {
                                        terminated = true;
                                        break;
                                    }

                                    if (newline.Length <= 0)
                                    {
                                        terminated = true;
                                        break;
                                    }

                                    if (newline[0] == 0)
                                    {
                                        terminated = true;
                                        break;
                                    }

                                    //デリミタ検索
                                    for (i = 0; i < data.Length; i++)
                                    {
                                        if (data[i] == newline[0])
                                        {
                                            for (j = 0; j < newline.Length; j++)
                                            {
                                                if (data[i + j] != newline[j])
                                                {
                                                    break;
                                                }
                                            }

                                            if (j >= newline.Length)
                                            {
                                                Array.Resize<byte>(ref data, i + newline.Length);
                                                terminated = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    break;
                                }

                                if (terminated)
                                {
                                    break;
                                }

                                Thread.Sleep(20);
                            }

                            if (count != 0)
                            {
                                if (!terminated)
                                {
                                    throw new Exception();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //modApplication.TraceException(ex);
                            throw new SocketException($"ソケット通信受信失敗{Environment.NewLine}{ex.ToString()}", this);
                        }
                    }
                    else
                    {
                        throw new SocketException("クライアント・ソケット通信未接続", this);
                    }
                }
                else
                {
                    throw new SocketException("クライアント・ソケット通信未接続", this);
                }
            }

            return data;
        }

        /// <summary>
        /// ソケット通信文字列受信
        /// </summary>
        /// <returns>受信文字列</returns>
        public override string ReadLine()
        {
            string message = "";

            byte[] data = this.Read();
            int length = data.Length - this.NewLine.Length;
            if (length > 0)
            {
                message = this.Encoding.GetString(data, 0, length);
            }

            return message;
        }

        /// <summary>
        /// ソケット通信切断
        /// </summary>
        public override void Close()
        {
            //排他処理
            lock (this)
            {
                if (m_NetworkStream != null)
                {
                    m_NetworkStream.Close();
                    m_NetworkStream = null;
                }

                if (m_TcpClient != null)
                {
                    m_TcpClient.Close();
                    m_TcpClient = null;
                }

                if (m_TcpServer != null)
                {
                    m_TcpServer.Stop();
                    m_TcpServer = null;
                }

                base.Close();
            }
        }
    }

    /// <summary>
    /// TCP/IPクライアント・ソケット・クラス
    /// </summary>
    [Serializable]
    public class TcpClientSocket : SocketPort, ICommunicationPort, IDisposable
    {
        /// <summary>
        /// TCPクライアント・オブジェクト
        /// </summary>
        protected TcpClient m_TcpClient = null;

        /// <summary>
        /// ネットワークス・ストリーム・オブジェクト
        /// </summary>
        protected NetworkStream m_NetworkStream = null;

        /// <summary>
        /// ホスト名
        /// </summary>
        private string m_HostName = string.Empty;

        /// <summary>
        /// ポート番号
        /// </summary>
        private int m_PortNumber = 0;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TcpClientSocket()
            : base()
        {
            //排他処理
            lock (this)
            {
                //クライアント・ソケット・オブジェクト生成
                m_TcpClient = new TcpClient(AddressFamily.InterNetwork);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="encoding">エンコーディング指定(既定:US-ASCII)</param>
        public TcpClientSocket(Encoding encoding)
            : base(encoding)
        {
            //排他処理
            lock (this)
            {
                //クライアント・ソケット・オブジェクト生成
                m_TcpClient = new TcpClient(AddressFamily.InterNetwork);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host_name">ホスト名</param>
        /// <param name="socket_port">ソケット・ポート番号</param>
        /// <param name="encoding">エンコーディング指定(既定:US-ASCII)</param>
        public TcpClientSocket(string host_name, int socket_port, Encoding encoding = null)
            : base(encoding)
        {
            //排他処理
            lock (this)
            {
                try
                {
                    //クライアント・ソケット・オブジェクト生成
                    if (host_name == null)
                    {
                        host_name = string.Empty;
                    }
                    m_TcpClient = new TcpClient(host_name, socket_port);
                    m_NetworkStream = m_TcpClient.GetStream();

                    m_HostName = host_name;
                    m_PortNumber = socket_port;
                }
#if DEBUG
                catch (Exception ex)
                {
                    //modApplication.TraceException(ex);
                    throw new SocketException($"ソケット通信接続要求失敗{Environment.NewLine}{ex.ToString()}", this);
                }
#else
                catch
                {
                    throw new SocketException($"ソケット通信接続要求失敗{Environment.NewLine}", this);
                }
#endif
            }
        }

        /// <summary>
        /// TCPクライアント・オブジェクト
        /// </summary>
        public TcpClient TcpClient
        {
            get
            {
                return m_TcpClient;
            }
        }

        /// <summary>
        /// ネットワーク・ストリーム
        /// </summary>
        public NetworkStream NetworkStream
        {
            get
            {
                return m_NetworkStream;
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
                if (!Connected)
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
                if (!Connected)
                {
                    m_PortNumber = value;
                }
            }
        }

        /// <summary>
        /// ポート名
        /// </summary>
        public string PortName
        {
            get
            {
                return string.Format("{0}:{1}", Dns.GetHostAddresses(m_HostName)[0], m_PortNumber);
            }
        }

        /// <summary>
        /// 接続確認
        /// </summary>
        public bool Connected
        {
            get
            {
                bool value = false;
                if (m_TcpClient != null)
                {
                    value = (m_TcpClient.Connected && (!m_TcpClient.Client.Poll(1000, SelectMode.SelectRead) || m_TcpClient.Client.Available > 0));
                }
                return value;
            }
        }

        /// <summary>
        /// 読み取り可能データ量
        /// </summary>
        public int Available
        {
            get
            {
                int value = 0;
                if (m_TcpClient != null)
                {
                    value = m_TcpClient.Available;
                }
                return value;
            }
        }

        /// <summary>
        /// 送信タイムアウト(msec)
        /// </summary>
        public int SendTimeout
        {
            get
            {
                int value = 0;
                if (m_TcpClient != null)
                {
                    value = m_TcpClient.SendTimeout;
                }
                return value;
            }
            set
            {
                if (m_TcpClient != null)
                {
                    m_TcpClient.SendTimeout = (value >= 0) ? value : 0;
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
                int value = 0;
                if (m_TcpClient != null)
                {
                    value = m_TcpClient.SendBufferSize;
                }
                return value;
            }
            set
            {
                if (m_TcpClient != null)
                {
                    m_TcpClient.SendBufferSize = value;
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
                int value = 0;
                if (m_TcpClient != null)
                {
                    value = m_TcpClient.ReceiveTimeout;
                }
                return value;
            }
            set
            {
                if (m_TcpClient != null)
                {
                    m_TcpClient.ReceiveTimeout = (value >= 0) ? value : 0;
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
                int value = 0;
                if (m_TcpClient != null)
                {
                    value = m_TcpClient.ReceiveBufferSize;
                }
                return value;
            }
            set
            {
                if (m_TcpClient != null)
                {
                    m_TcpClient.ReceiveBufferSize = value;
                }
            }
        }

        /// <summary>
        /// ソケット通信接続要求
        /// </summary>
        public virtual void Connect()
        {
            //排他処理
            lock (this)
            {
                if (m_TcpClient == null)
                {
                    //クライアント・ソケット・オブジェクト生成
                    m_TcpClient = new TcpClient(AddressFamily.InterNetwork);
                }

                if (m_TcpClient != null)
                {
                    try
                    {
                        if (!m_TcpClient.Connected)
                        {
                            //接続要求実行
                            m_TcpClient.Connect(m_HostName, m_PortNumber);
                            m_NetworkStream = m_TcpClient.GetStream();
                        }
                    }
                    catch (Exception ex)
                    {
                        //modApplication.TraceException(ex);
                        throw new SocketException($"ソケット通信接続要求失敗{Environment.NewLine}{ex.ToString()}", this);
                    }
                }
                else
                {
                    throw new SocketException("クライアント・ソケット通信オブジェクト未生成");
                }

                this.Abort = false;
            }
        }

        /// <summary>
        /// ソケット通信接続要求
        /// </summary>
        /// <param name="host_name">ホスト名</param>
        /// <param name="socket_port">ソケット・ポート番号</param>
        public virtual void Connect(string host_name, int socket_port)
        {
            //排他処理
            lock (this)
            {
                if (m_TcpClient == null)
                {
                    //クライアント・ソケット・オブジェクト生成
                    m_TcpClient = new TcpClient(AddressFamily.InterNetwork);
                }

                if (m_TcpClient != null)
                {
                    try
                    {
                        if (!m_TcpClient.Connected)
                        {
                            //接続要求実行
                            if (host_name == null)
                            {
                                host_name = string.Empty;
                            }
                            m_TcpClient.Connect(host_name, socket_port);
                            m_NetworkStream = m_TcpClient.GetStream();

                            m_HostName = host_name;
                            m_PortNumber = socket_port;
                        }
                    }
                    catch (Exception ex)
                    {
                        //modApplication.TraceException(ex);
                        throw new SocketException($"ソケット通信接続要求失敗{Environment.NewLine}{ex.ToString()}", this);
                    }
                }
                else
                {
                    throw new SocketException("クライアント・ソケット通信オブジェクト未生成");
                }

                this.Abort = false;
            }
        }

        /// <summary>
        /// ソケット通信データ送信
        /// </summary>
        /// <param name="data">送信データ</param>
        public override void Write(byte[] data)
        {
            //排他処理
            lock (this)
            {
                if (m_TcpClient != null && m_NetworkStream != null)
                {
                    if (m_NetworkStream.CanWrite)
                    {
                        try
                        {
                            if (data != null && data.Length > 0)
                            {
                                m_NetworkStream.Write(data, 0, data.Length);
                            }
                        }
                        catch (Exception ex)
                        {
                            //modApplication.TraceException(ex);
                            throw new SocketException($"ソケット通信送信失敗{Environment.NewLine}{ex.ToString()}", this);
                        }
                    }
                    else
                    {
                        throw new SocketException("クライアント・ソケット通信未接続", this);
                    }
                }
                else
                {
                    throw new SocketException("クライアント・ソケット通信未接続", this);
                }
            }
        }

        /// <summary>
        /// ソケット通信文字列送信
        /// </summary>
        /// <param name="message">送信文字列</param>
        public override void WriteLine(string message)
        {
            this.Write(this.Encoding.GetBytes(message + this.NewLine));
        }

        /// <summary>
        /// ソケット通信データ受信
        /// </summary>
        /// <returns>受信データ</returns>
        public override byte[] Read()
        {
            byte[] data = new byte[0];

            //排他処理
            lock (this)
            {
                if (m_TcpClient != null && m_NetworkStream != null)
                {
                    if (m_NetworkStream.CanRead)
                    {
                        try
                        {
                            byte[] newline = this.Encoding.GetBytes(this.NewLine);

                            byte[] buffer = new byte[1024];
                            int length = 0;
                            int count = 0;
                            bool terminated = false;
                            int i = 0;
                            int j = 0;

                            for (int retry = 0; retry <= this.MaximumRetryCount; retry++)
                            {
                                if (this.Abort)
                                {
                                    terminated = true;
                                    break;
                                }

                                count = m_NetworkStream.Read(buffer, 0, buffer.Length);

                                if (count > 0)
                                {
                                    Array.Resize<byte>(ref data, length + count);
                                    Array.Copy(buffer, 0, data, length, count);
                                    length = data.Length;

                                    if (newline == null)
                                    {
                                        terminated = true;
                                        break;
                                    }

                                    if (newline.Length <= 0)
                                    {
                                        terminated = true;
                                        break;
                                    }

                                    if (newline[0] == 0)
                                    {
                                        terminated = true;
                                        break;
                                    }

                                    //デリミタ検索
                                    for (i = 0; i < data.Length; i++)
                                    {
                                        if (data[i] == newline[0])
                                        {
                                            for (j = 0; j < newline.Length; j++)
                                            {
                                                if (data[i + j] != newline[j])
                                                {
                                                    break;
                                                }
                                            }

                                            if (j >= newline.Length)
                                            {
                                                Array.Resize<byte>(ref data, i + newline.Length);
                                                terminated = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    break;
                                }

                                if (terminated)
                                {
                                    break;
                                }

                                Thread.Sleep(20);
                            }

                            if (count != 0)
                            {
                                if (!terminated)
                                {
                                    throw new Exception();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //modApplication.TraceException(ex);
                            throw new SocketException($"ソケット通信受信失敗{Environment.NewLine}{ex.ToString()}", this);
                        }
                    }
                    else
                    {
                        throw new SocketException("クライアント・ソケット通信未接続", this);
                    }
                }
                else
                {
                    throw new SocketException("クライアント・ソケット通信未接続", this);
                }
            }

            return data;
        }

        /// <summary>
        /// ソケット通信文字列受信
        /// </summary>
        /// <returns>受信文字列</returns>
        /// <remarks></remarks>
        public override string ReadLine()
        {
            string message = "";

            byte[] data = this.Read();
            int length = data.Length - this.NewLine.Length;
            if (length > 0)
            {
                message = this.Encoding.GetString(data, 0, length);
            }

            return message;
        }

        /// <summary>
        /// ソケット通信切断
        /// </summary>
        public override void Close()
        {
            //排他処理
            lock (this)
            {
                if (m_NetworkStream != null)
                {
                    m_NetworkStream.Close();
                    m_NetworkStream = null;
                }

                if (m_TcpClient != null)
                {
                    m_TcpClient.Close();
                    m_TcpClient = null;
                }

                base.Close();
            }
        }

        /// <summary>
        /// シャットダウン
        /// </summary>
        /// <param name="how">ソケット・シャットダウン方法</param>
        public virtual void Shutdown(SocketShutdown how)
        {
            if (m_TcpClient != null)
            {
                if (m_TcpClient.Client != null)
                {
                    m_TcpClient.Client.Shutdown(how);
                }
            }
        }
    }

    /// <summary>
    /// UDPソケット・クラス
    /// </summary>
    [Serializable]
    public class UdpSocket : SocketPort, IDisposable
    {
        /// <summary>
        /// UDPソケット・オブジェクト
        /// </summary>
        protected UdpClient m_UdpClient = null;

        /// <summary>
        /// ホスト名
        /// </summary>
        private string m_HostName = string.Empty;

        /// <summary>
        /// ポート番号
        /// </summary>
        private int m_PortNumber = 0;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UdpSocket()
            : base()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="encoding">エンコーディング指定(既定:US-ASCII)</param>
        public UdpSocket(Encoding encoding)
            : base(encoding)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="local_port">ローカル・ポート番号</param>
        /// <param name="encoding">エンコーディング指定(既定:US-ASCII)</param>
        public UdpSocket(int local_port, Encoding encoding = null)
            : base(encoding)
        {
            //排他処理
            lock (this)
            {
                try
                {
                    //UDPソケット・オブジェクト生成
                    m_UdpClient = new UdpClient(local_port);

                    m_HostName = string.Empty;
                    m_PortNumber = local_port;
                }
#if DEBUG
                catch (Exception ex)
                {

                    //modApplication.TraceException(ex);
                    throw new SocketException($"ソケット通信接続要求失敗{Environment.NewLine}{ex.ToString()}", this);
                }
#else
                catch
                {
                    throw new SocketException($"ソケット通信接続要求失敗{Environment.NewLine}", this);
                }
#endif
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host_name">ホスト名</param>
        /// <param name="remote_port">リモート・ポート番号</param>
        /// <param name="encoding">エンコーディング指定(既定:US-ASCII)</param>
        public UdpSocket(string host_name, int remote_port, Encoding encoding = null)
            : base(encoding)
        {
            //排他処理
            lock (this)
            {
                try
                {
                    //UDPソケット・オブジェクト生成
                    m_UdpClient = new UdpClient();

                    //接続要求実行
                    if (host_name == null)
                    {
                        host_name = string.Empty;
                    }
                    m_UdpClient.Connect(host_name, remote_port);

                    m_HostName = host_name;
                    m_PortNumber = remote_port;
                }
#if DEBUG
                catch (Exception ex)
                {
                    //modApplication.TraceException(ex);
                    throw new SocketException($"ソケット通信接続要求失敗{Environment.NewLine}{ex.ToString()}", this);
                }
#else
                catch
                {
                    throw new SocketException($"ソケット通信接続要求失敗{Environment.NewLine}", this);
                }
#endif
            }
        }

        /// <summary>
        /// ソケット
        /// </summary>
        public UdpClient UdpClient
        {
            get
            {
                return m_UdpClient;
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
                if (!Connected)
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
                if (!Connected)
                {
                    m_PortNumber = value;
                }
            }
        }

        /// <summary>
        /// ポート名
        /// </summary>
        public string PortName
        {
            get
            {
                return string.Format("{0}:{1}", Dns.GetHostAddresses(m_HostName)[0], m_PortNumber);
            }
        }

        /// <summary>
        /// 接続確認
        /// </summary>
        public bool Connected
        {
            get
            {
                bool value = false;
                if (m_UdpClient != null)
                {
                    value = (!m_UdpClient.Client.Poll(1000, SelectMode.SelectRead) || m_UdpClient.Client.Available > 0);
                }
                return value;
            }
        }

        /// <summary>
        /// 読み取り可能データ量
        /// </summary>
        public int Available
        {
            get
            {
                int value = 0;
                if (m_UdpClient != null)
                {
                    value = m_UdpClient.Available;
                }
                return value;
            }
        }

        /// <summary>
        /// ソケット通信接続要求
        /// </summary>
        /// <param name="local_port">ローカル・ポート番号</param>
        public virtual void Connect(int local_port)
        {
            //排他処理
            lock (this)
            {
                try
                {
                    //UDPソケット・オブジェクト生成
                    m_UdpClient = new UdpClient(local_port);

                    m_HostName = string.Empty;
                    m_PortNumber = local_port;
                }
#if DEBUG
                catch (Exception ex)
                {
                    //modApplication.TraceException(ex);
                    throw new SocketException($"ソケット通信接続要求失敗{Environment.NewLine}{ex.ToString()}", this);
                }
#else
                catch
                {
                    throw new SocketException($"ソケット通信接続要求失敗{Environment.NewLine}", this);
                }
#endif

                this.Abort = false;
            }
        }

        /// <summary>
        /// ソケット通信接続要求
        /// </summary>
        /// <param name="host_name">ホスト名</param>
        /// <param name="remote_port">リモート・ポート番号</param>
        public virtual void Connect(string host_name, int remote_port)
        {
            //排他処理
            lock (this)
            {
                if (m_UdpClient == null)
                {
                    //UDPソケット・オブジェクト生成
                    m_UdpClient = new UdpClient();
                }

                if (m_UdpClient != null)
                {
                    try
                    {
                        //接続要求実行
                        if (host_name == null)
                        {
                            host_name = string.Empty;
                        }
                        m_UdpClient.Connect(host_name, remote_port);

                        m_HostName = host_name;
                        m_PortNumber = remote_port;
                    }
                    catch (Exception ex)
                    {
                        //modApplication.TraceException(ex);
                        throw new SocketException($"ソケット通信接続要求失敗{Environment.NewLine}{ex.ToString()}", this);
                    }
                }
                else
                {
                    throw new Exception("UDPソケット通信オブジェクト未生成");
                }

                this.Abort = false;
            }
        }

        /// <summary>
        /// ソケット通信データ送信
        /// </summary>
        /// <param name="data">送信データ</param>
        public override void Write(byte[] data)
        {
            //排他処理
            lock (this)
            {
                if (m_UdpClient != null)
                {
                    try
                    {
                        if (data != null && data.Length > 0)
                        {
                            m_UdpClient.Send(data, data.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        //modApplication.TraceException(ex);
                        throw new SocketException($"ソケット通信送信失敗{Environment.NewLine}{ex.ToString()}", this);
                    }
                }
                else
                {
                    throw new SocketException($"UDPソケット通信未接続{Environment.NewLine}", this);
                }
            }
        }

        /// <summary>
        /// ソケット通信文字列送信
        /// </summary>
        /// <param name="message">送信文字列</param>
        public override void WriteLine(string message)
        {
            this.Write(this.Encoding.GetBytes(message + this.NewLine));
        }

        /// <summary>
        /// ソケット通信データ受信
        /// </summary>
        /// <returns>受信データ</returns>
        public override byte[] Read()
        {
            byte[] data = new byte[0];

            //排他処理
            lock (this)
            {
                if (m_UdpClient != null)
                {
                    try
                    {
                        byte[] newline = this.Encoding.GetBytes(this.NewLine);

                        System.Net.IPEndPoint remote_ep = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0);
                        byte[] buffer = new byte[0];
                        int length = 0;
                        int count = 0;
                        bool terminated = false;
                        int i = 0;
                        int j = 0;

                        for (int retry = 0; retry <= this.MaximumRetryCount; retry++)
                        {
                            if (this.Abort)
                            {
                                terminated = true;
                                break;
                            }

                            buffer = m_UdpClient.Receive(ref remote_ep);
                            count = buffer.Length;

                            if (count > 0)
                            {
                                Array.Resize<byte>(ref data, length + count);
                                Array.Copy(buffer, 0, data, length, count);
                                length = data.Length;

                                if (newline == null)
                                {
                                    terminated = true;
                                    break;
                                }

                                if (newline.Length <= 0)
                                {
                                    terminated = true;
                                    break;
                                }

                                if (newline[0] == 0)
                                {
                                    terminated = true;
                                    break;
                                }

                                //デリミタ検索
                                for (i = 0; i < data.Length; i++)
                                {
                                    if (data[i] == newline[0])
                                    {
                                        for (j = 0; j < newline.Length; j++)
                                        {
                                            if (data[i + j] != newline[j])
                                            {
                                                break;
                                            }
                                        }

                                        if (j >= newline.Length)
                                        {
                                            Array.Resize<byte>(ref data, i + newline.Length);
                                            terminated = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }

                            if (terminated)
                            {
                                break;
                            }

                            Thread.Sleep(20);
                        }

                        if (count != 0)
                        {
                            if (!terminated)
                            {
                                throw new Exception();
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        //modApplication.TraceException(ex);
                        throw new Exception($"ソケット通信受信失敗.{Environment.NewLine}{ex.Message}");
                        //throw new SocketException("ソケット通信受信失敗", this);
                    }
                }
                else
                {
                    throw new Exception($"UDPソケット通信未接続.{Environment.NewLine}");
                    //throw new SocketException("UDPソケット通信未接続", this);
                }
            }

            return data;
        }

        /// <summary>
        /// ソケット通信文字列受信
        /// </summary>
        /// <returns>受信文字列</returns>
        public override string ReadLine()
        {
            string message = "";

            byte[] data = this.Read();
            int length = data.Length - this.NewLine.Length;
            if (length > 0)
            {
                message = this.Encoding.GetString(data, 0, length);
            }

            return message;
        }

        /// <summary>
        /// ソケット通信切断
        /// </summary>
        public override void Close()
        {
            //排他処理
            lock (this)
            {
                if (m_UdpClient != null)
                {
                    m_UdpClient.Close();
                    m_UdpClient = null;
                }

                base.Close();
            }
        }
    }
}
