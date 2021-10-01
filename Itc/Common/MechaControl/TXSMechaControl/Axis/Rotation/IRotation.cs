using Board.BoardControl;
using Itc.Common.TXEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TXSMechaControl.Rotation
{
    /// <summary>
    /// TXSの試料テーブル回転インターフェイス
    /// </summary>
    public interface IRotation
    {
        /// <summary>
        /// Rotationに関する何かしらが変更された
        /// </summary>
        event EventHandler RotChanged;
        /// <summary>
        /// PLCから回転要求があった
        /// </summary>
        event EventHandler PLCRotRequest;
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
        bool Index(float posi, Action<string> errormes, ComProgress progress);
        /// <summary>
        /// 更新要求
        /// </summary>
        void UpdateAll();
        /// <summary>
        /// 回転角度を要求
        /// </summary>
        /// <returns></returns>
        float GetRot();
        /// <summary>
        /// 回転角度の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        int GetRotDeip();
        /// <summary>
        /// 最大回転角度
        /// </summary>
        /// <returns></returns>
        float GetRotMax();
        /// <summary>
        /// 最小回転角度
        /// </summary>
        /// <returns></returns>
        float GetRotMin();
        /// <summary>
        /// 回転の最大角度
        /// </summary>
        /// <returns></returns>
        float GetRotSpeedMax();
        /// <summary>
        /// 回転の最小角度
        /// </summary>
        /// <returns></returns>
        float GetRotSpeedMin();
    }
}
