using Itc.Common.Event;
using Itc.Common.TXEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MechaMaintCnt.FCD
{
    using NumUpdateEventHandler = Action<object, NumUpdateEventArgs>;
    using MessageEventHandler = Action<object, MessageEventArgs>;
    /// <summary>
    /// FCDのI/F
    /// </summary>
    public interface IFCD
    {
        /// <summary>
        /// コマンド変更
        /// </summary>
        event EventHandler CmdChanged;
        /// <summary>
        /// 変更通知
        /// </summary>
        event EventHandler FCDChanged;
        /// <summary>
        /// 原点復帰
        /// </summary>
        bool Origin(Action<string> mes, ComProgress progress);
        /// <summary>
        /// 手動動作
        /// </summary>
        bool Manual(MDirMode mode, Action<string> mes);
        /// <summary>
        /// 速度更新(選択 )
        /// </summary>
        void UpdateSpeed(string selspddisp, Action<string> mes);
        /// <summary>
        /// 速度更新(直入力　float)
        /// </summary>
        bool UpdateSpeed(float selspddisp, Action<string> mes);
        /// <summary>
        /// インデックス動作
        /// </summary>
        /// <param name="posi"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        bool Index(float posi, Action<string> mes, ComProgress progress);
        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        bool Stop(Action<string> mes);
        /// <summary>
        /// 原点位置の更新
        /// </summary>
        void UpdateOrigin(float origin, Action<string> errormes);
        /// <summary>
        /// ホーム位置の更新
        /// </summary>
        void UpdateHome(float home, Action<string> errormes);
        /// <summary>
        /// X線干渉位置 1の更新
        /// </summary>
        void UpdateHamamatu1(float hama1, Action<string> errormes);
        /// <summary>
        /// X線干渉位置 2の更新
        /// </summary>
        void UpdateHamamatu2(float hama2, Action<string> errormes);
        /// <summary>
        /// 更新要求
        /// </summary>
        void UpdateAll();
        /// <summary>
        /// 現在のFCD値を要求
        /// </summary>
        float GetFCD();
        /// <summary>
        /// 有効桁数の取得
        /// </summary>
        /// <returns></returns>
        int GetFCDDeip();
        /// <summary>
        /// FCD Home値を要求
        /// </summary>
        /// <returns></returns>
        float GetFCDHome();
        /// <summary>
        /// FCD Homeの有効桁数の取得
        /// </summary>
        /// <returns></returns>
        int GetFCDHomeDeip();
        /// <summary>
        /// FCD 原点位置の取得
        /// </summary>
        /// <returns></returns>
        float GetFCDOrigin();
        /// <summary>
        /// FCD 原点位置の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        int GetFCDOriginDeip();
        /// <summary>
        /// FCD 浜松干渉位置1の取得
        /// </summary>
        /// <returns></returns>
        float GetFCDHama1();
        /// <summary>
        /// FCD 浜松干渉位置１の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        int GetFCDHama1Deip();
        /// <summary>
        /// FCD 浜松干渉位置2の取得
        /// </summary>
        /// <returns></returns>
        float GetFCDHama2();
        /// <summary>
        /// FCD 浜松干渉位置２の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        int GetFCDHama2Deip();
        /// <summary>
        /// 原点位置か？
        /// </summary>
        /// <param name="isbesy"></param>
        void SetOrg(bool isorg);
        /// <summary>
        /// ビジー状態か
        /// </summary>
        /// <param name="isbesy"></param>
        void SetBesy(bool isbesy);
    }
}
