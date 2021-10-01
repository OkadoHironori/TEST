using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace Itc.Common.Utility
{
    /*
     * オブジェクトの複製ストックを、一定数キープしておく：未完成
     */
    public class CloneCreator<T> : IDisposable
        where T : class, ICloneable
    {
        T org;
        int _init, _minstock, _addcount;
        Queue<T> queue;

        //監視用タイマー
        Timer timer;

        //クローン作製
        private T _Clone()
        {
            //DeepCopy
            return CloneHelper.DeepClone<T>(org);
        }

        //コンストラクタ
        public CloneCreator(T target, int init, int minstock, int addcount)
        {
            //Clone作成
            org = target.Clone() as T;
            _init = init;
            _minstock = minstock;
            _addcount = addcount;

            //ストック作成
            queue = new Queue<T>();
            for (int i = 0; i < _init; ++i)
            {
                queue.Enqueue(_Clone());
            }

            //監視用
            timer = new Timer(new TimerCallback(StockCounter), null, 10, 1000);
            //var thread = new System.Threading.Thread(new System.Threading.ThreadStart(_WatchStockCount));
            //thread.Start();
        }

        //タイマーコールバック    //排他処理必要かも？
        private void StockCounter(object state)
        {
            if (queue.Count < _minstock)
            {
                //追加
                for (int i = 0; i < _addcount; ++i)
                {
                    queue.Enqueue(_Clone());
                }
            }
        }

        //インスタンス取得
        public T GetCloneInstance()
        {
            if (queue.Count == 0)
                throw new Exception();

            return queue.Dequeue();
        }

        //ストック数
        public int Count
        {
            get { return queue.Count; }
        }

        public void Dispose()
        {
            //タイマー停止＆破棄
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            timer.Dispose();

            //キューの中身を破棄
            while (queue.Count > 0)
            {
                var obj = queue.Dequeue();
                if (obj is IDisposable)
                {
                    var d = obj as IDisposable;
                    d.Dispose();
                }
            }


        }
    }

    /*
     * オブジェクトのクローン作製（DeepCopy）
     */
    public class CloneHelper
    {
        //
        /// <summary>
        /// srcのDeepCopyを作成する。
        /// ※このメソッドを使用する場合は、対象クラスがSerializable属性であることが前提となります
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src">コピー対象インスタンス</param>
        /// <returns>コピーされたインスタンス</returns>
        public static T DeepClone<T>(T src)
            where T : class
        {
            object dst = null;

            using (var stream = new MemoryStream())
            {
                var bformatter = new BinaryFormatter();

                bformatter.Serialize(stream, src);

                stream.Position = 0;

                dst = bformatter.Deserialize(stream);
            }

            return (T)dst;
        }
    }

}
