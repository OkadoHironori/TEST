using Itc.Common.TXEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TXSMechaControl.TblY
{
    public interface ITableY
    {
        /// <summary>
        /// テーブルYに関する何かしらが変更された
        /// </summary>
        event EventHandler TblYChanged;
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
        /// 有効桁数
        /// </summary>
        /// <returns></returns>
        int GetTblYDeip();
        /// <summary>
        /// 速度更新
        /// </summary>
        /// <param name="selspddisp"></param>
        /// <param name="mes"></param>
        void UpdateSpeed(string selspddisp, Action<string> mes);
        /// <summary>
        /// 速度更新
        /// </summary>
        /// <param name="selspddisp"></param>
        /// <param name="mes"></param>
        bool UpdateSpeed(float selspddisp, Action<string> mes);
        /// <summary>
        /// 更新要求
        /// </summary>
        void UpdateAll();
        /// <summary>
        /// <summary>
        /// テーブルY値の取得
        /// </summary>
        /// <returns></returns>
        float GetTblY();
        /// <summary>
        /// テーブルY 原点位置の取得
        /// </summary>
        /// <returns></returns>
        float GetTblYOrigin();
        /// <summary>
        /// テーブルY 原点位置の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        int GetTblYOriginDeip();
        /// <summary>
        /// X線干渉位置 1の更新
        /// </summary>
        void UpdateHamamatu1(float hama1, Action<string> errormes);
        /// <summary>
        /// X線干渉位置 2の更新
        /// </summary>
        void UpdateHamamatu2(float hama2, Action<string> errormes);
        /// <summary>
        /// FCD 浜松干渉位置1の取得
        /// </summary>
        /// <returns></returns>
        float GetTblYHama1();
        /// <summary>
        /// FCD 浜松干渉位置１の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        int GetTblYHama1Deip();
        /// <summary>
        /// FCD 浜松干渉位置2の取得
        /// </summary>
        /// <returns></returns>
        float GetTblYHama2();
        /// <summary>
        /// FCD 浜松干渉位置２の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        int GetTblYHama2Deip();
    }
}
