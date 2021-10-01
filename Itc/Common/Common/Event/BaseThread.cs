
namespace Itc.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows.Forms;
    /// <summary>
    /// 基底スレッド・オブジェクト・クラス
    /// </summary>
    public abstract class BaseThreadObject
    {
        /// <summary>
        /// スレッド間同期オブジェクト・リスト
        /// </summary>
        private static object[] m_SyncObjects = new object[0];

        /// <summary>
        /// スレッド間同期排他ロック状態スタック
        /// </summary>
        private static Stack<bool>[] m_SyncLocked = new Stack<bool>[0];

        /// <summary>
        /// スレッド終了要求イベント・オブジェクト
        /// </summary>
        private AutoResetEvent m_ExitRequest = new AutoResetEvent(false);

        /// <summary>
        /// スレッド一旦停止要求イベント・オブジェクト
        /// </summary>
        private AutoResetEvent m_SuspendRequest = new AutoResetEvent(false);

        /// <summary>
        /// スリープ時間[msec]
        /// </summary>
        private int m_SleepTime = 50;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">スレッド名</param>
        protected BaseThreadObject(string name = null)
        {
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~BaseThreadObject()
        {
            //if (m_ExitRequest != null)
            //{
            //    m_ExitRequest.Dispose();
            //    m_ExitRequest = null;
            //}

            //if (m_SuspendRequest != null)
            //{
            //    m_SuspendRequest.Dispose();
            //    m_SuspendRequest = null;
            //}

            if (m_SyncObjects != null)
            {
                for (int i = 0; i < m_SyncObjects.Length; i++)
                {
                    m_SyncObjects[i] = null;
                }
                m_SyncObjects = null;
            }

            if (m_SyncLocked != null)
            {
                for (int i = 0; i < m_SyncLocked.Length; i++)
                {
                    m_SyncLocked[i].Clear();
                    m_SyncLocked[i] = null;
                }
                m_SyncLocked = null;
            }
        }


        /// <summary>
        /// スレッド終了要求イベント・オブジェクト
        /// </summary>
        protected AutoResetEvent ExitRequestEvent
        {
            get
            {
                return m_ExitRequest;
            }
        }

        /// <summary>
        /// スレッド一旦停止要求イベント・オブジェクト
        /// </summary>
        protected AutoResetEvent SuspendRequestEvent
        {
            get
            {
                return m_SuspendRequest;
            }
        }

        /// <summary>
        /// スレッド終了要求
        /// </summary>
        protected bool ExitRequest
        {
            get
            {
                return m_ExitRequest.WaitOne(0);
            }
        }

        /// <summary>
        /// スリープ時間[msec]
        /// </summary>
        public int SleepTime
        {
            get
            {
                return m_SleepTime;
            }
            set
            {
                m_SleepTime = (value >= 10) ? value : 10;
            }
        }

        /// <summary>
        /// スレッド・メイン関数
        /// </summary>
        /// <param name="arg">引数</param>
        /// <remarks>このメソッドをオーバーライドして下さい。</remarks>
        protected abstract void ThreadMain(object arg);

        /// <summary>
        /// スレッド起動
        /// </summary>
        /// <param name="arg">ThreadMainメソッドへ渡す引数</param>
        public abstract void Start(object arg = null);

        /// <summary>
        /// スレッド停止
        /// </summary>
        /// <param name="timeout">タイムアウト時間(msec)</param>
        public abstract void Terminate(int timeout = System.Threading.Timeout.Infinite);

        /// <summary>
        /// スレッド一旦停止開始
        /// </summary>
        public virtual void Suspend()
        {
            AutoResetEvent.WaitAny(
                new AutoResetEvent[] {
                    m_ExitRequest,
                    m_SuspendRequest
                }
            );
        }

        /// <summary>
        /// スレッド一旦停止終了
        /// </summary>
        public virtual void Resume()
        {
            m_SuspendRequest.Set();
        }


        /// <summary>
        /// スレッド間同期オブジェクト確保
        /// </summary>
        public void AllocateLock(int count)
        {
            m_SyncObjects = new object[count];
            m_SyncLocked = new Stack<bool>[count];

            for (int i = 0; i < count; i++)
            {
                m_SyncObjects[i] = new object();
                m_SyncLocked[i] = new Stack<bool>();
            }
        }

        /// <summary>
        /// 排他ロック取得
        /// </summary>
        /// <param name="lock_number">排他ロック番号</param>
        /// <param name="timeout">タイムアウト時間[msec]</param>
        /// <returns>
        /// true :排他ロック成功
        /// false:排他ロック失敗
        /// </returns>
        public bool Lock(int lock_number, int timeout = Timeout.Infinite)
        {
            bool locked = false;

            if (timeout == 0)
            {
                locked = Monitor.TryEnter(m_SyncObjects[lock_number]);
            }
            else if (timeout > 0)
            {
                locked = Monitor.TryEnter(m_SyncObjects[lock_number], timeout);
            }
            else
            {
                Monitor.Enter(m_SyncObjects[lock_number]);
                locked = true;
            }

            m_SyncLocked[lock_number].Push(locked);

            return locked;
        }

        /// <summary>
        /// 排他ロック解放
        /// </summary>
        /// <param name="lock_number">排他ロック番号</param>
        public void Unlock(int lock_number)
        {
            if (m_SyncLocked[lock_number].Pop())
            {
                Monitor.Exit(m_SyncObjects[lock_number]);
            }
        }

        /// <summary>
        /// 全排他ロック取得
        /// </summary>
        /// <param name="timeout">タイムアウト時間[msec]</param>
        /// <returns>
        /// true :排他ロック成功
        /// false:排他ロック失敗
        /// </returns>
        public bool LockAll(int timeout = Timeout.Infinite)
        {
            bool total_locked = false;

            Stopwatch timer = new Stopwatch();

            try
            {
                if (m_SyncObjects.Length > 0)
                {
                    total_locked = true;
                    timer.Start();

                    for (int i = 0, timeout0 = timeout; i < m_SyncObjects.Length; i++)
                    {
                        bool locked = false;
                        if (timeout > 0)
                        {
                            timeout = timeout0 - (int)timer.ElapsedMilliseconds;
                            if (timeout < 0)
                            {
                                timeout = 0;
                            }
                        }
                        if (timeout == 0)
                        {
                            locked = Monitor.TryEnter(m_SyncObjects[i]);
                        }
                        else if (timeout > 0)
                        {
                            locked = Monitor.TryEnter(m_SyncObjects[i], timeout);
                        }
                        else
                        {
                            Monitor.Enter(m_SyncObjects[i]);
                            locked = true;
                        }
                        if (!locked)
                        {
                            total_locked = false;
                        }

                        m_SyncLocked[i].Push(locked);
                    }
                }
            }
            finally
            {
                timer.Stop();
            }

            return total_locked;
        }

        /// <summary>
        /// 全排他ロック解放
        /// </summary>
        public void UnlockAll()
        {
            for (int i = 0; i < m_SyncObjects.Length; i++)
            {
                if (m_SyncLocked[i].Pop())
                {
                    Monitor.Exit(m_SyncObjects[i]);
                }
            }
        }
    }
}
