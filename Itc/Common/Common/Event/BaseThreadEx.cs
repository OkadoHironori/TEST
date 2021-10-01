using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itc.Common.Event
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    /// <summary>
    /// 拡張基底スレッド・クラス
    /// </summary>
    /// <remarks>System.Threading.Tasks.Taskを利用して、スレッドを管理します。</remarks>
    public abstract class BaseThreadEx : BaseThreadObject
    {
        /// <summary>
        /// タスク・オブジェクト
        /// </summary>
        private Task m_Task = null;

        /// <summary>
        /// タスク名
        /// </summary>
        private string m_TaskName = string.Empty;

        /// <summary>
        /// タスク生成オプション
        /// </summary>
        private TaskCreationOptions? m_TaskOption = null;

        /// <summary>
        /// タスク・キャンセル・トークン・ソース
        /// </summary>
        private CancellationTokenSource m_TaskCancel = null;

        /// <summary>
        /// タスク・パラメータ
        /// </summary>
        private object m_Arguments = null;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">スレッド名</param>
        /// <param name="option">タスク生成オプション</param>
        protected BaseThreadEx(string name = null, TaskCreationOptions? option = null)
            : base()
        {
            if (name != null)
            {
                m_TaskName = name;
            }
            m_TaskOption = option;
        }

        /// <summary>
        /// タスク・オブジェクト
        /// </summary>
        public Task TaskObject
        {
            get
            {
                return m_Task;
            }
        }

        /// <summary>
        /// スレッド名
        /// </summary>
        public string Name
        {
            get
            {
                return m_TaskName;
            }
            set
            {
                if (value != null)
                {
                    m_TaskName = value;
                }
                else
                {
                    m_TaskName = string.Empty;
                }
            }
        }

        /// <summary>
        /// スレッド実行中確認
        /// </summary>
        public bool IsAlive
        {
            get
            {
                bool value = false;
                if (m_Task != null)
                {
                    value = (m_Task.Status == TaskStatus.Running
                            || m_Task.Status == TaskStatus.WaitingForChildrenToComplete
                            || m_Task.Status == TaskStatus.WaitingToRun);
                }
                return value;
            }
        }

        /// <summary>
        /// スレッド正常終了確認
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                bool value = false;
                if (m_Task != null)
                {
                    value = m_Task.IsCompleted;
                }
                return value;
            }
        }

        /// <summary>
        /// スレッド異常終了確認
        /// </summary>
        public bool IsFaulted
        {
            get
            {
                bool value = false;
                if (m_Task != null)
                {
                    value = m_Task.IsFaulted;
                }
                return value;
            }
        }

        /// <summary>
        /// スレッド・キャンセル終了確認
        /// </summary>
        public bool IsCanceled
        {
            get
            {
                bool value = false;
                if (m_Task != null)
                {
                    value = m_Task.IsCanceled;
                }
                return value;
            }
        }

        /// <summary>
        /// スレッド・ステータス
        /// </summary>
        public TaskStatus? Status
        {
            get
            {
                TaskStatus? value = null;
                if (m_Task != null)
                {
                    value = m_Task.Status;
                }
                return value;
            }
        }

        /// <summary>
        /// スレッド起動
        /// </summary>
        /// <param name="arg">ThreadMainメソッドへ渡す引数</param>
        public override void Start(object arg = null)
        {
            this.Start(arg, null);
        }

        /// <summary>
        /// スレッド起動
        /// </summary>
        /// <param name="arg">ThreadMainメソッドへ渡す引数</param>
        /// <param name="scheduler">タスク・スケジューラ</param>
        public virtual void Start(object arg = null, TaskScheduler scheduler = null)
        {
            if (m_Task != null)
            {
                // スレッド停止
                this.Terminate(1000);
            }

            // スレッド生成
            // ※ラムダ式で記述 ({}内がスレッド処理部分)
            m_TaskCancel = new CancellationTokenSource();
            if (m_TaskOption != null)
            {
                m_Task = new Task(() => { ThreadMain(m_Arguments); }, m_TaskCancel.Token, (TaskCreationOptions)m_TaskOption);
            }
            else
            {
                m_Task = new Task(() => { ThreadMain(m_Arguments); }, m_TaskCancel.Token);
            }

            //スレッド停止要求リセット
            this.ExitRequestEvent.Reset();

            //スレッド起動
            m_Arguments = arg;
            if (scheduler != null)
            {
                m_Task.Start(scheduler);
            }
            else
            {
                m_Task.Start();
            }
        }

        /// <summary>
        /// スレッド停止
        /// </summary>
        /// <param name="timeout">タイムアウト時間(msec)</param>
        public override void Terminate(int timeout = System.Threading.Timeout.Infinite)
        {
            if (m_Task != null)
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                while (m_Task.Status == TaskStatus.WaitingToRun
                    || m_Task.Status == TaskStatus.WaitingForChildrenToComplete)
                {
                    if (timer.ElapsedMilliseconds > 10000)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
                timer.Stop();

                if (m_Task.Status == TaskStatus.Running)
                {
                    //スレッド停止
                    this.ExitRequestEvent.Set();

                    //スレッド停止待ち
                    if (m_TaskCancel != null && timeout > System.Threading.Timeout.Infinite)
                    {
                        if (!m_Task.Wait(timeout))
                        {
                            //スレッド強制終了
                            m_TaskCancel.Cancel();
                            m_Task.Wait();
                        }
                    }
                    else
                    {
                        m_Task.Wait();
                    }
                }

                // スレッド破棄
                if (m_Task.IsCompleted || m_Task.IsFaulted || m_Task.IsCanceled)
                {
                    m_Task.Dispose();
                }
                if (m_TaskCancel != null)
                {
                    m_TaskCancel.Dispose();
                    m_TaskCancel = null;
                }
            }

            m_Task = null;
        }

        /// <summary>
        /// キャンセル監視
        /// </summary>
        protected virtual void CancelMonitor()
        {
            if (m_TaskCancel.Token.IsCancellationRequested)
            {
                m_TaskCancel.Token.ThrowIfCancellationRequested();
            }
        }
    }
}
