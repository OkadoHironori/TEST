
using Board.BoardControl;
using Itc.Common.TXEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TXSMechaControl.UpDown
{
    public interface IUdStage
    {
        /// <summary>
        /// 昇降に関する何かしらが変更された
        /// </summary>
        event EventHandler UdChanged;
        /// <summary>
        /// 初期化
        /// </summary>
        bool Init(Action<string> mes);
        /// <summary>
        /// 原点復帰
        /// </summary>
        /// <param name="errormes"></param>
        /// <param name="prog">進捗</param>
        /// <param name="ctoken">キャンセル</param>
        /// <returns></returns>
        bool Origin(Action<string> errormes, IProgress<CalProgress> prog, CancellationTokenSource ctoken);
        /// <summary>
        /// マニュアル動作
        /// </summary>
        bool Manual(RevMode mode, Action<string> errormes);
        /// <summary>
        /// 速度更新
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        void UpdateSpeed(string speadmes, Action<string> errormes);
        /// <summary>
        /// 速度更新
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        void UpdateSpeed(float speadmes, Action<string> errormes);
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="errormes"></param>
        /// <returns></returns>
        bool Stop(Action<string> errormes);
        /// <summary>
        /// インデックス移動
        /// </summary>
        /// <param name="posi"></param>
        /// <param name="errormes"></param>
        /// <param name="prog"></param>
        /// <param name="ctoken"></param>
        /// <returns></returns>
        bool Index(float posi, Action<string> errormes, ComProgress com);
        /// <summary>
        /// 更新要求
        /// </summary>
        void UpdateAll();
        /// <summary>
        /// アラームリセット
        /// </summary>
        void AlamReset();
        /// <summary>
        /// 昇降位置を要求
        /// </summary>
        /// <returns></returns>
        float GetUPPosi();
        /// <summary>
        /// 昇降テーブルの有効桁数の取得
        /// </summary>
        /// <returns></returns>
        int GetUDDeip();
        /// <summary>
        /// 昇降位置の最大値
        /// </summary>
        /// <returns></returns>
        float GetUDMax();
        /// <summary>
        /// 昇降位置の最小値
        /// </summary>
        /// <returns></returns>
        float GetUDMin();
        /// <summary>
        /// 昇降最高速度
        /// </summary>
        /// <returns></returns>
        float GetUDSpeedMax();
        /// <summary>
        /// 昇降最低速度
        /// </summary>
        /// <returns></returns>
        float GetUDSpeedMin();

    }
}
