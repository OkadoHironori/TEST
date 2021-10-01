/*
  
	簡易スレッド・クラス
 
 
 
	(c) Copyright  TOSHIBA IT & Control Systems Corporation 2017, All Rights Reserved

    History:
        Date        Version     Explanation                             Modifier        
        -------------------------------------------------------------------------------
        2018/05/28  0.0.0.0     コーディング完成                        (AIT)M.KOIKE
 
*/
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Itc.Common.Event
{
    /// <summary>
    /// 簡易スレッド・クラス
    /// </summary>
    public class SimpleThread
    {
        /// <summary>
        /// スレッド・オブジェクト
        /// </summary>
        protected Thread m_Thread = null;

        /// <summary>
        /// タスク・オブジェクト
        /// </summary>
        protected Task m_Task = null;

        /// <summary>
        /// タスク・キャンセル・オブジェクト
        /// </summary>
        protected CancellationTokenSource m_Cancel = null;

        /// <summary>
        /// タスク終了要求イベント・オブジェクト
        /// </summary>
        protected AutoResetEvent m_ExitRequest = new AutoResetEvent(false);

        /// <summary>
        /// タスク一旦停止要求イベント・オブジェクト
        /// </summary>
        protected AutoResetEvent m_SuspendRequest = new AutoResetEvent(false);

        /// <summary>
        /// タスク終了通知イベント・オブジェクト
        /// </summary>
        protected AutoResetEvent m_ExitedAcknowledge = new AutoResetEvent(false);

        /// <summary>
        /// タスク動作指定
        /// </summary>
        protected Action m_Action = null;

        /// <summary>
        /// タスク・タイマー・オブジェクト
        /// </summary>
        protected Timer m_Timer = null;

        /// <summary>
        /// タスク異常コード
        /// </summary>
        protected int m_ErrorCode = 0;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SimpleThread()
        {
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~SimpleThread()
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

            Dispose();
        }

        /// <summary>
        /// オブジェクト破棄
        /// </summary>
        public virtual void Dispose()
        {
            try
            {
                if (IsAlive)
                {
                    Terminate();
                }

                if (m_Thread != null)
                {
                    m_Thread = null;
                }
                if (m_Task != null)
                {
                    m_Task.Dispose();
                    m_Task = null;
                }
                if(m_Cancel != null)
                {
                    m_Cancel.Dispose();
                    m_Cancel = null;
                }
                if (m_ExitedAcknowledge != null)
                {
                    m_ExitedAcknowledge.Dispose();
                    m_ExitedAcknowledge = null;
                }

                m_Action = null;
                m_ErrorCode = 0;
            }
            catch (Exception ex)
            {
                //modApplication.TraceException(ex);
                throw new Exception($"SimpleThread is failed.{Environment.NewLine}{ex.Message}");
            }

            // デストラクタを呼び出さない。
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// スレッド・オブジェクト
        /// </summary>
        public Thread ThreadObject
        {
            get
            {
                return m_Thread;
            }
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
        /// タスク・キャンセル・オブジェクト
        /// </summary>
        public CancellationTokenSource CancelObject
        {
            get
            {
                return m_Cancel;
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
        /// タスク終了通知イベント・オブジェクト
        /// </summary>
        protected AutoResetEvent ExitedAcknowledgeEvent
        {
            get
            {
                return m_ExitedAcknowledge;
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
        /// タスク終了通知
        /// </summary>
        public bool ExitedAcknowledge
        {
            get
            {
                return m_ExitedAcknowledge.WaitOne(0);
            }
            set
            {
                if(value)
                {
                    m_ExitedAcknowledge.Set();
                }
                else
                {
                    m_ExitedAcknowledge.Reset();
                }
            }
        }

        /// <summary>
        /// タスク動作指定
        /// </summary>
        public Action Action
        {
            get
            {
                return m_Action;
            }
        }

        /// <summary>
        /// タスク実行中
        /// </summary>
        public bool IsAlive
        {
            get
            {
                bool value = false;
                if(m_Thread != null)
                {
                    value = m_Thread.IsAlive;
                }
                else if(m_Task != null)
                {
                    value = (m_Task.Status == TaskStatus.Running
                            || m_Task.Status == TaskStatus.WaitingForChildrenToComplete
                            || m_Task.Status == TaskStatus.WaitingToRun);
                }
                else
                {
                    // DO NOTHING
                }
                return value;
            }
        }

        /// <summary>
        /// タスク実行結果
        /// </summary>
        public bool Result
        {
            get
            {
                return (m_ErrorCode == 0);
            }
        }

        /// <summary>
        /// タスク異常コード
        /// </summary>
        public int ErrorCode
        {
            get
            {
                return m_ErrorCode;
            }
            set
            {
                m_ErrorCode = value;
            }
        }


        /// <summary>
        /// タスク・タイマー開始
        /// </summary>
        /// <param name="callback">コールバック・メソッド</param>
        /// <param name="state">コールバック・メソッドに渡すパラメータ</param>
        /// <param name="delay_time">コールバック・メソッド呼出開始遅延時間[msec]</param>
        /// <param name="interval">コールバック・メソッド呼出周期[msec]</param>
        public virtual void StartTimer(TimerCallback callback, object state, int delay_time, int interval)
        {
            m_Timer = new Timer(callback, state, delay_time, interval);
        }

        /// <summary>
        /// タスク・タイマー停止
        /// </summary>
        public virtual void StopTimer()
        {
            if (m_Timer != null)
            {
                m_Timer.Dispose();
                m_Timer = null;
            }
        }


        /// <summary>
        /// スレッド開始
        /// </summary>
        /// <param name="action">動作指定</param>
        /// <param name="max_stack_size">最大スタック・サイズ[byte]</param>
        /// <returns>異常コード</returns>
        /// <remarks>動作指定は、ラムダ式で指定してください。</remarks>
        public virtual int Start(Action action, int max_stack_size = 0)
        {
            int result = 0;

            try
            {
                do
                {
                    // スレッド実行中確認
                    if (IsAlive)
                    {
                        result = -1;
                        break;
                    }

                    // スレッド終了イベント・リセット
                    m_ExitRequest.Reset();
                    m_SuspendRequest.Reset();
                    m_ExitedAcknowledge.Reset();
                    m_Action = null;
                    m_ErrorCode = 0;

                    // スレッド開始準備
                    m_Thread = new Thread(new ThreadStart(action), max_stack_size);
                    if (m_Thread == null)
                    {
                        result = -1;
                        break;
                    }

                    // スレッド開始
                    m_Thread.Start();
                    m_Action = action;
                } while (false);
            }
            catch (Exception ex)
            {
                throw new Exception($"SimpleThread is failed.{Environment.NewLine}{ex.Message}");
                //result = -1;
            }
            finally
            {
                if (result != 0)
                {
                    if (m_Thread != null)
                    {
                        m_Thread = null;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// タスク開始
        /// </summary>
        /// <param name="action">動作指定</param>
        /// <param name="options">タスク生成オプション</param>
        /// <param name="scheduler">タスク・スケジューラ</param>
        /// <returns>異常コード</returns>
        /// <remarks>動作指定は、ラムダ式で指定してください。</remarks>
        public virtual int StartEx(Action action, TaskCreationOptions? options = null, TaskScheduler scheduler = null)
        {
            int result = 0;

            try
            {
                do
                {
                    // タスク実行中確認
                    if (IsAlive)
                    {
                        result = -1;
                        break;
                    }

                    // タスク終了イベント・リセット
                    m_ExitRequest.Reset();
                    m_SuspendRequest.Reset();
                    m_ExitedAcknowledge.Reset();
                    m_Action = null;
                    m_ErrorCode = 0;

                    // タスク開始準備
                    m_Cancel = new CancellationTokenSource();
                    if (m_Cancel == null)
                    {
                        result = -1;
                        break;
                    }
                    if (options != null)
                    {
                        m_Task = new Task(action, m_Cancel.Token, (TaskCreationOptions)options);
                    }
                    else
                    {
                        m_Task = new Task(action, m_Cancel.Token);
                    }
                    if (m_Task == null)
                    {
                        result = -1;
                        break;
                    }

                    // タスク開始
                    if (scheduler != null)
                    {
                        m_Task.Start(scheduler);
                    }
                    else
                    {
                        m_Task.Start();
                    }
                    m_Action = action;
                } while (false);
            }
            catch (Exception ex)
            {
                //modApplication.TraceException(ex);
                throw new Exception($"SimpleThread is failed.{Environment.NewLine}{ex.Message}");
                //result = -1;
            }
            finally
            {
                if (result != 0)
                {
                    if(m_Task != null)
                    {
                        m_Task.Dispose();
                        m_Task = null;
                    }
                    if (m_Cancel != null)
                    {
                        m_Cancel.Dispose();
                        m_Cancel = null;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// タスク強制終了
        /// </summary>
        /// <param name="wait_time">タスク終了待ち時間@msec]</param>
        public virtual void Terminate(int wait_time = 10000)
        {
            try
            {
                if (m_Thread != null)
                {
                    if (m_Thread.IsAlive)
                    {
                        if (!m_Thread.Join(wait_time))
                        {
                            // スレッド強制終了
                            m_Thread.Abort();
                        }
                    }
                    m_Thread = null;

                    m_Action = null;
                    m_ErrorCode = 0;
                }
                else if (m_Task != null)
                {
                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    while (m_Task.Status == TaskStatus.WaitingToRun
                        || m_Task.Status == TaskStatus.WaitingForChildrenToComplete)
                    {
                        if (timer.ElapsedMilliseconds > wait_time)
                        {
                            break;
                        }
                        Thread.Sleep(100);
                    }
                    timer.Stop();

                    if (m_Task.Status == TaskStatus.Running)
                    {
                        if (!m_Task.Wait(wait_time))
                        {
                            // タスク強制終了
                            if (m_Cancel != null)
                            {
                                m_Cancel.Cancel();
                            }
                            else
                            {
                                throw new ApplicationException();
                            }
                            m_Task.Wait();
                        }
                    }
                    bool completed = m_Task.IsCompleted;
                    if (m_Task.IsCompleted || m_Task.IsFaulted || m_Task.IsCanceled)
                    {
                        m_Task.Dispose();
                    }
                    m_Task = null;

                    if (m_Cancel != null)
                    {
                        m_Cancel.Dispose();
                        m_Cancel = null;
                    }

                    m_Action = null;
                    m_ErrorCode = 0;
                }
                else
                {
                    // DO NOTHING
                }
            }
            catch (Exception ex)
            {
                //modApplication.TraceException(ex);
                throw new Exception($"SimpleThread is failed.{Environment.NewLine}{ex.Message}");
            }
        }

        /// <summary>
        /// タスク終了待ち
        /// </summary>
        /// <param name="timeout">タイムアウト時間[msec]</param>
        /// <returns>異常コード</returns>
        public virtual int Wait(int timeout = Timeout.Infinite)
        {
            int result = 0;

            try
            {
                bool signaled = false;

                if (m_Thread != null)
                {
                    if (timeout == Timeout.Infinite)
                    {
                        m_ExitedAcknowledge.WaitOne();
                        signaled = true;
                    }
                    else
                    {
                        signaled = m_ExitedAcknowledge.WaitOne(timeout);
                    }

                    if (signaled)
                    {
                        Thread.Sleep(100);

                        if (m_Thread.IsAlive)
                        {
                            if (!m_Thread.Join(10000))
                            {
                                // スレッド強制終了
                                m_Thread.Abort();
                            }
                        }
                        m_Thread = null;

                        result = m_ErrorCode;

                        m_Action = null;
                        m_ErrorCode = 0;
                    }
                    else
                    {
                        result = -1;
                    }
                }
                else if (m_Task != null)
                {
                    if (timeout == Timeout.Infinite)
                    {
                        m_ExitedAcknowledge.WaitOne();
                        signaled = true;
                    }
                    else
                    {
                        signaled = m_ExitedAcknowledge.WaitOne(timeout);
                    }

                    if (signaled)
                    {
                        Thread.Sleep(100);

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
                            if (!m_Task.Wait(10000))
                            {
                                // タスク強制終了
                                if (m_Cancel != null)
                                {
                                    m_Cancel.Cancel();
                                }
                                else
                                {
                                    throw new ApplicationException();
                                }
                                m_Task.Wait();
                            }
                        }
                        bool completed = m_Task.IsCompleted;
                        if (m_Task.IsCompleted || m_Task.IsFaulted || m_Task.IsCanceled)
                        {
                            m_Task.Dispose();
                        }
                        m_Task = null;

                        if (m_Cancel != null)
                        {
                            m_Cancel.Dispose();
                            m_Cancel = null;
                        }

                        if (completed)
                        {
                            result = m_ErrorCode;
                        }

                        m_Action = null;
                        m_ErrorCode = 0;
                    }
                    else
                    {
                        result = -1;
                    }
                }
                else
                {
                    // DO NOTHING
                }
            }
            catch (Exception ex)
            {
                //modApplication.TraceException(ex);
                result = -1;
                throw new Exception($"SimpleThread is failed.{Environment.NewLine}{ex.Message}");
            }

            return result;
        }

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
        /// コールバック
        /// </summary>
        public int Callback()
        {
            // タスク・キャンセル処理
            if (m_Cancel != null && m_Cancel.Token != null)
            {
                if (m_Cancel.Token.IsCancellationRequested)
                {
                    Debug.WriteLine("**** Task Cancellation Requested ****");
                    m_Cancel.Token.ThrowIfCancellationRequested();
                }
            }
            return 0;
        }
    }
}
