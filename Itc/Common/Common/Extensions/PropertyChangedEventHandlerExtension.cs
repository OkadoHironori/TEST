using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Threading;
using System.Linq.Expressions;

namespace Itc.Common.Extensions
{
    /// <summary>
    /// INotifyPropertyChanged関係の拡張メソッド
    /// </summary>
    public static class PropertyChangedEventHandlerExtension
    {
        /// <summary>
        /// ［拡張メソッド］呼び出し元のプロパティ名を PropertyChangedEventArgs の PropertyName に設定する。
        /// </summary>
        /// <param name="eh"></param>
        /// <param name="sender"></param>
        /// <param name="context">同期コンテキスト</param>
        /// <param name="propertyName">プロパティ名（指定なければ呼び出し元のプロパティ名を設定する）</param>
        public static void Notice(this PropertyChangedEventHandler eh, object sender,
            SynchronizationContext context = null,
            [System.Runtime.CompilerServices.CallerMemberName]string propertyName = null)
        {
            if (null != eh)
            {
                if (null == context)
                {
                    eh(sender, new PropertyChangedEventArgs(propertyName));
                }
                else
                {
                    //UI以外のスレッドからバインディング処理させる場合、/必要な個所にSynchronizationContextを渡しておく
                    context.Send((d) =>
                    {
                        eh(sender, new PropertyChangedEventArgs(d as string));
                    }, propertyName);
                }
            }
        }
    }
}
