using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CT30K
{
	#region ReceiveCollBack Delegate

	/// <summary>
	/// 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	internal delegate void ReceiveCollBack(object sender, ReceivedEventArgs e);

	#endregion ReceiveCollBack Delegate

	#region ReceivedEventArgs Class

	/// <summary>
	/// 
	/// </summary>
	internal class ReceivedEventArgs : EventArgs
	{
		private byte[] _ReceiveData = null;

		public ReceivedEventArgs(byte[] data)
		{
			_ReceiveData = data;
		}

		public byte[] ReceiveData
		{
			get { return _ReceiveData?? new byte[0]; }
		}
	}

	#endregion ReceivedEventArgs Class

	#region UpdReceiver Class

	/// <summary>
	/// 
	/// </summary>
	internal class UpdReceiver
	{
		#region Fields

        /// <summary>
        /// 受信を行うソケット
        /// </summary>
        private UdpClient _Listener = null;

        /// <summary>
        /// 受信待ちをするローカルIPエンドポイント
        /// </summary>
        private IPEndPoint _LocalEndPoint = null;

		/// <summary>
		/// 切断用の同期オブジェクト
		/// </summary>
		private object _SyncSocketClose = new object();

		/// <summary>
		/// 
		/// </summary>
		private AsyncCallback _DataReceivedCallback = null;

		/// <summary>
		/// 受信コールバック
		/// </summary>
		private ReceiveCollBack _OnRecieved = null;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// 新しいインスタンスを初期化する。
		/// </summary>
		/// <param name="callback">受信したときに呼び出されるメソッドを表す ReceiveCollBack デリゲート。</param>
		public UpdReceiver(ReceiveCollBack callback)
		{
			_OnRecieved = callback;
		}

		#endregion Constructors

		#region Methods

		/// <summary>
		/// 指定した IPEndpoint で接続の待ち受けを開始する。
		/// </summary>
		/// <param name="endpoint"></param>
		public void Start(IPEndPoint endpoint)
		{
			_LocalEndPoint = endpoint;

			_Listener = new UdpClient(endpoint);

			_DataReceivedCallback = new AsyncCallback(DataReceived);

			_Listener.BeginReceive(_DataReceivedCallback, _Listener);
		}

		/// <summary>
		/// 受信接続待ちを停止し、接続を閉じる。
		/// </summary>
		public void Stop()
		{
			lock (_SyncSocketClose)
			{
				if (_Listener != null)
				{
					_Listener.Close();
					_Listener = null;
				}
			}
		}

		/// <summary>
		/// 要求の読み込み時に呼ばれるコールバックメソッド
		/// </summary>
		/// <param name="ar"></param>
		private void DataReceived(IAsyncResult ar)
		{
			try
			{
				byte[] data = null;

				lock (_SyncSocketClose)
				{
					UdpClient uc = (UdpClient)ar.AsyncState;

					if (uc.Client == null)
					{
						return;
					}

					IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);

					data = uc.EndReceive(ar, ref ep);

					uc.BeginReceive(_DataReceivedCallback, uc);
				}

				if (data != null)
				{
					//受信処理
					OnReceived(new ReceivedEventArgs(data));
				}
			}
			catch
			{
			}
			finally
			{ 
				//nothing
			}
		}

		/// <summary>
		/// OnReceived コールバックを発生させる。
		/// </summary>
		protected virtual void OnReceived(ReceivedEventArgs e)
		{
			if (_OnRecieved != null)
			{
				_OnRecieved(this, e);
			}
		}

		#endregion Methods
	}

	#endregion UpdReceiver Class
}
