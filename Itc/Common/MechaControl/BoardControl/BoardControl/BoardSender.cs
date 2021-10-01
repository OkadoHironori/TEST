using Itc.Common.Event;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Board.BoardControl
{
    using CountChangeEventHandler = Action<object, NumUpdateEventArgs>;
    /// <summary>
    /// ボードの表示値を決めた周期で送信する
    /// </summary>
    public class BoardSender : IBoardSender, IDisposable
    {
        public string _ID { get; set; }
        /// <summary>
        /// 位置を通知
        /// </summary>
        public event CountChangeEventHandler NotifyRot;
        /// <summary>
        /// 位置を通知
        /// </summary>
        public event CountChangeEventHandler NotifyUd;
        /// <summary>
        /// 位置を通知
        /// </summary>
        public event CountChangeEventHandler NotifyFX;
        /// <summary>
        /// 位置を通知
        /// </summary>
        public event CountChangeEventHandler NotifyFY;

        /// <summary>
        /// ボードの定期監視用
        /// </summary>
        public System.Timers.Timer _Timer = null;
        /// <summary>
        /// ボードの表示値の順番を整理するコンカレントスタック
        /// </summary>
        private static readonly ConcurrentStack<ValueAxis> OrderValue = new ConcurrentStack<ValueAxis>();
        /// <summary>
        /// タイマー起動
        /// </summary>
        public void SetTimer(int interval)
        {
            if (_Timer == null)
            {
                _Timer = new System.Timers.Timer()
                {
                    Interval = interval,
                    AutoReset = false,
                };

                //タイマー
                _Timer.Elapsed += (s, e) =>
                {
                    while (OrderValue.Count != 0)
                    {
                        if (OrderValue.TryPeek(out ValueAxis latest))//スタックの上のデータを抜き取る
                        {
                            switch (latest.Axis)
                            {
                                case ("ROT_JIKU"):
                                    NotifyRot.Invoke(this, new NumUpdateEventArgs(latest.BoardValue));
                                    break;
                                case ("UD_JIKU"):
                                    NotifyUd.Invoke(this, new NumUpdateEventArgs(latest.BoardValue));
                                    break;
                                case ("XSTG_JIKU"):
                                    NotifyFX.Invoke(this, new NumUpdateEventArgs(latest.BoardValue));
                                    break;
                                case ("YSTG_JIKU"):
                                    NotifyFY.Invoke(this, new NumUpdateEventArgs(latest.BoardValue));
                                    break;
                            }
                            OrderValue.Clear();//スタックをクリア
                        }
                    }

                    _Timer.Start();
                };

                _Timer.Start();
            }
        }
        /// <summary>
        /// ボードからの値をプッシュ
        /// </summary>
        /// <param name="boaddispvale"></param>
        public void PushData(float boarddvale, string BoardAxis)
        {
            OrderValue.Push(new ValueAxis() { Axis = BoardAxis, BoardValue = boarddvale });
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            if (_Timer != null)
            {
                _Timer.Stop();
                _Timer.Enabled = false;//タイマー停止
                Task.WaitAll(Task.Delay(100));
                _Timer.Dispose();
                _Timer = null;

            }
            OrderValue.Clear();
            //気休め
            NotifyRot = null;
            NotifyUd = null;
            NotifyFX = null;
            NotifyFY = null;
        }

        internal class ValueAxis
        {
            public string Axis { get; set; }
            public float BoardValue { get; set; }
        }
    }

    public interface IBoardSender
    {
        /// <summary>
        /// 位置を通知
        /// </summary>
        event CountChangeEventHandler NotifyRot;
        /// <summary>
        /// 位置を通知
        /// </summary>
        event CountChangeEventHandler NotifyUd;
        /// <summary>
        /// 位置を通知
        /// </summary>
        event CountChangeEventHandler NotifyFX;
        /// <summary>
        /// 位置を通知
        /// </summary>
        event CountChangeEventHandler NotifyFY;
        /// <summary>
        /// データをセット
        /// </summary>
        /// <param name="boaddispvale"></param>
        void PushData(float boaddispvale, string axis);
        /// <summary>
        /// タイマーのセット
        /// </summary>
        /// <param name="interval"></param>
        void SetTimer(int interval);

    }
}
