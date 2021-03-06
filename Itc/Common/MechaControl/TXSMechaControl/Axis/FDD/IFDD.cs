using Itc.Common.Event;
using Itc.Common.TXEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TXSMechaControl.FDD
{
    public interface IFDD
    {
        /// <summary>
        /// FDDに関する何かしらが変更された
        /// </summary>
        event EventHandler FDDChanged;
        /// <summary>
        /// <summary>
        /// 初期化
        /// </summary>
        bool Init(Action<string> mes);
        /// <summary>
        /// 原点復帰
        /// </summary>
        bool Origin(Action<string> errormes, IProgress<CalProgress> prog, CancellationToken ctoken);
        /// <summary>
        /// 手動動作
        /// </summary>
        bool Manual(MDirMode mode, Action<string> mes);
        /// <summary>
        /// インデックス動作
        /// </summary>
        /// <param name="posi"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        bool Index(float posi, Action<string> errormes, IProgress<CalProgress> prog, CancellationToken ctoken);
        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        bool Stop(Action<string> mes);
        /// <summary>
        /// 速度更新
        /// </summary>
        /// <param name="selspddisp"></param>
        /// <param name="mes"></param>
        void UpdateSpeed(string selspddisp, Action<string> mes);
        /// <summary>
        /// 更新要求
        /// </summary>
        void UpdateAll();
        /// <summary>
        /// 有効桁数
        /// </summary>
        /// <returns></returns>
        int GetFDDDeip();
        /// <summary>
        /// FDD値
        /// </summary>
        /// <returns></returns>
        float GetFDD();



        /// <summary>
        /// 速度更新
        /// </summary>
        /// <param name="selspddisp"></param>
        /// <param name="mes"></param>
        bool UpdateSpeed(float selspddisp, Action<string> mes);
        /// <summary>
        /// テーブルY 原点位置の取得
        /// </summary>
        /// <returns></returns>
        float GetFDDOrigin();
        /// <summary>
        /// テーブルY 原点位置の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        int GetFDDOriginDeip();
    }
}
