using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itc.Common.TXEnum
{
    /// <summary>
    /// 停止モード
    /// </summary>
    public enum StopMode : int
    {
        /// <summary>
        /// 減速停止
        /// </summary>
        Slow = 0,
        /// <summary>
        /// 即時停止
        /// </summary>
        Fast = 1,
    }
    /// <summary>
    /// 監視モード
    /// </summary>
    public enum WatchMode : int
    {
        /// <summary>
        /// 停止待ち
        /// </summary>
        Ready = 0,
        /// <summary>
        /// 起動待ち
        /// </summary>
        Busy = 1,
        /// <summary>
        /// 定速待ち
        /// </summary>
        Const = 2,
    }
    /// <summary>
    /// 回転機構の回転方向 CW 0 CCW 1 SCAN_CW 2 SCAN_CCW 3
    /// </summary>
    public enum RevMode : int
    {
        /// <summary>
        /// 未定義
        /// </summary>
        Non = -1,
        /// <summary>
        /// 時計回り
        /// </summary>
        CW = 0,
        /// <summary>
        /// 反時計回り
        /// </summary>
        CCW = 1,
        /// <summary>
        /// CWスキャン
        /// </summary>
        SCAN_CW = 2,
        /// <summary>
        /// CCWスキャン(通常)
        /// </summary>
        SCAN_CCW = 3,
    }
    /// <summary>
    /// Winformsの状態
    /// </summary>
    public enum FrmSts
    {
        /// <summary>
        /// 初期状態
        /// </summary>
        Init,
        /// <summary>
        /// ビジー状態
        /// </summary>
        Busy,
        /// <summary>
        /// 準備完了状態
        /// </summary>
        Ready,
    }
    /// <summary>
    /// Winformsのインターロック状態
    /// </summary>
    public enum FrmInterlockSts
    {
        /// <summary>
        /// 電磁ロックON(拘束状態)
        /// </summary>
        Lock,
        /// <summary>
        /// 電磁ロック(拘束解除)
        /// </summary>
        Unlock,
    }

    /// <summary>
    /// 位置指定モード
    /// 絶対:0  相対:1
    /// </summary>
    public enum PosiMode : int
    {
        /// <summary>
        /// 絶対座標
        /// </summary>
        Abso = 0,
        /// <summary>
        /// 相対座標
        /// </summary>
        Rela = 1,
    }
    /// <summary>
    /// 移動方向 Forward 1 Backward -1
    /// </summary>
    public enum MDirMode : int
    {
        Non = -99,
        /// <summary>
        /// 停止
        /// </summary>
        Stop = 0,
        /// <summary>
        /// インクリメント方向
        /// </summary>
        Forward = 1,
        /// <summary>
        /// デクリメント方向
        /// </summary>
        Backward = -1,
    }

    /// <summary>
    /// 命令対象の軸
    /// </summary>
    public enum OptMechaAxis : int
    {
        /// <summary>
        /// 非選択
        /// </summary>
        Non = -1,
        /// <summary>
        /// FCD
        /// </summary>
        FCD = 1,
        /// <summary>
        /// FDD
        /// </summary>
        FDD = 2,
        /// <summary>
        /// 回転
        /// </summary>
        Rot = 3,
        /// <summary>
        /// その他
        /// </summary>
        AUX = 4,
        /// <summary>
        /// 昇降
        /// </summary>
        UPDOWN = 5,
        /// <summary>
        /// 微調テーブル
        /// </summary>
        FSTAGE = 6,
        /// <summary>
        /// テーブルY
        /// </summary>
        TBLY = 7,               //Add2020/12/17hata

    }
    /// <summary>
    /// 移動方向 Forward 1 Backward -1
    /// </summary>
    public enum LimitMode
    {
        /// <summary>
        /// 制限中
        /// </summary>
        OnLimit,
        /// <summary>
        /// 制限されていない
        /// </summary>
        limitless,
    }
}
