
using Board.BoardControl;
using Itc.Common.TXEnum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TXSMechaControl.FStage
{
    public interface IFStage
    {
        /// <summary>
        /// 微調テーブルに関する何かしらが変更された
        /// </summary>
        event EventHandler FStageChanged;
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
        /// 停止
        /// </summary>
        /// <param name="errormes"></param>
        /// <returns></returns>
        bool Stop(Action<string> errormes);
        /// <summary>
        /// インデックスX移動
        /// </summary>
        /// <param name="posi"></param>
        /// <param name="errormes"></param>
        /// <param name="prog"></param>
        /// <param name="ctoken"></param>
        /// <returns></returns>
        bool Index_FX(float posi, Action<string> errormes, IProgress<CalProgress> prog, CancellationTokenSource ctoken);
        /// <summary>
        /// インデックスY移動
        /// </summary>
        /// <param name="posi"></param>
        /// <param name="errormes"></param>
        /// <param name="prog"></param>
        /// <param name="ctoken"></param>
        /// <returns></returns>
        bool Index_FY(float posi, Action<string> errormes, IProgress<CalProgress> prog, CancellationTokenSource ctoken);
        /// <summary>
        /// 速度更新
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        void UpdateSpeed(string selspddisp, Action<string> errormes);
        /// <summary>
        /// 微調X軸のマニュアル動作
        /// </summary>
        bool Manual_FX(RevMode mode, Action<string> errormes);
        /// <summary>
        /// 微調Y軸のマニュアル動作
        /// </summary>
        bool Manual_FY(RevMode mode, Action<string> errormes);
        /// <summary>
        /// 更新要求
        /// </summary>
        void UpdateAll();
        /// <summary>
        /// 微調テーブルX軸の位置を要求
        /// </summary>
        /// <returns></returns>
        float GetFXPosi();
        /// <summary>
        /// 微調テーブルY軸の位置を要求
        /// </summary>
        /// <returns></returns>
        float GetFYPosi();
        /// <summary>
        /// 微調テーブルX軸の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        int GetFXDeip();
        /// <summary>
        /// 微調テーブルY軸の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        int GetFYDeip();
        /// <summary>
        /// 微調テーブルX軸の最大値
        /// </summary>
        /// <returns></returns>
        float GetFXMax();
        /// <summary>
        /// 微調テーブルY軸の最大値
        /// </summary>
        /// <returns></returns>
        float GetFYMax();
        /// <summary>
        /// 微調テーブルY軸の最大値
        /// </summary>
        /// <returns></returns>
        float GetFYMin();

    }
}
