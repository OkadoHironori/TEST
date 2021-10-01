using System;
using System.Threading;

namespace CT30K
{
	/// <summary>
	/// スレッドを作成および制御し、そのスレッドの優先順位の設定およびステータスの取得を行う。
	/// </summary>
	public class ThreadEx
	{
		#region Fields

		private Thread _Thread = null;
		private volatile bool _Stoped = false;
		private ManualResetEvent _Event = new ManualResetEvent(false);

		#endregion Fields

		#region Constructors

		/// <summary>
		/// ThreadEx クラスの新しいインスタンスを初期化する。 
		/// </summary>
		/// <param name="start"></param>
		public ThreadEx(ThreadStart start)
		{
			_Thread = new Thread(start);
		}

		/// <summary>
		/// スレッドの開始時にオブジェクトをスレッドに渡すことを許可するデリゲートを指定して、新しいインスタンスを初期化する。 
		/// </summary>
		/// <param name="start"></param>
		public ThreadEx(ParameterizedThreadStart start)
		{
			_Thread = new Thread(start);
		}

		#endregion Constractors

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			get { return _Thread.Name; }
			set { _Thread.Name = value; }
		}

		/// <summary>
		/// スレッドのスケジューリング優先順位を示す値を取得または設定する。 
		/// </summary>
		public ThreadPriority Priority
		{
			get { return _Thread.Priority; }
			set { _Thread.Priority = value; }
		}

		/// <summary>
		/// スレッドを停止するかどうかを示す値を取得または設定する。
		/// </summary>
		public bool Stoped
		{
			get { return _Stoped; }
			private set { _Stoped = value; }
		}

		/// <summary>
		/// 現在のスレッドの状態を示す値を取得する。 
		/// </summary>
		public ThreadState ThreadState
		{
			get { return _Thread.ThreadState; }
		}

		/// <summary>
		/// 現在のスレッドの実行ステータスを示す値を取得する。
		/// </summary>
		public bool IsAlive
		{
			get { return _Thread.IsAlive; }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// オペレーティング システムによって、現在のインスタンスの状態を ThreadState.Running に変更する。 
		/// </summary>
		public void Start()
		{
			_Thread.Start();
		}

		/// <summary>
		/// オペレーティング システムによって現在のインスタンスの状態が ThreadState.Running に変更され、オプションでスレッドが実行するメソッドで使用するデータを格納するオブジェクトが提供される。
		/// </summary>
		public void Start(object parameter)
		{
			_Thread.Start(parameter);
		}
		
		/// <summary>
		/// このメソッドが呼び出された対象のスレッドを終了する。 
		/// </summary>
		public void Stop()
		{
			_Event.Set();

			this.Stoped = true;

			_Thread.Join(2000);
		}

		/// <summary>
		/// このメソッドが呼び出された対象のスレッドで、そのスレッドの終了プロセスを開始する ThreadAbortException を発生させる。
		/// </summary>
		public void Abort()
		{
			try
			{
				_Thread.Abort();
			}
			catch
			{
			}
		}

		/// <summary>
		/// 指定した時間だけ現在のスレッドを中断するが、スレッドが停止される場合は中断から即座に復旧し、停止されるかどうかを示す値を返す。
		/// </summary>
		/// <param name="millisecondsTimeout">スレッドがブロックされるミリ秒数。</param>
		/// <returns>スレッドが停止される場合は true。それ以外は false。</returns>
		public bool Sleep(int millisecondsTimeout)
		{
			return _Event.WaitOne(millisecondsTimeout);
		}

		/// <summary>
		/// 指定した時間だけ現在のスレッドを中断するが、スレッドが停止される場合は中断から即座に復旧し、停止されるかどうかを示す値を返す。
		/// </summary>
		/// <param name="millisecondsTimeout">スレッドがブロックされる時間に設定される TimeSpan。</param>
		/// <returns>スレッドが停止される場合は true。それ以外は false。</returns>
		public bool Sleep(TimeSpan timeout)
		{
			return _Event.WaitOne(timeout);
		}

		#endregion Methods
	}
}
