using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itc.Common.Extensions
{
    /// <summary>
    /// Task関係の拡張メソッド
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// タイムアウト処理
        /// </summary>
        /// <param name="task">実行するTask</param>
        /// <param name="timeout">タイムアウトする時間</param>
        /// <returns></returns>
        public static async Task Timeout(this Task task, TimeSpan timeout)
        {
            Task delay = Task.Delay(timeout);

            if (await Task.WhenAny(task, delay) == delay)
            {
                //先にdelayTaskが終わったのでタイムアウト
                throw new TimeoutException();
            }
        }
    }
}
