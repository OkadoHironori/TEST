using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXSMechaControl.SpOp
{
    public class SpecialOption
    {
        /// <summary>
        /// スライスライト
        /// </summary>
        public bool StsSLight { get; set; }
        /// <summary>
        /// 振動／落下 試験時の判別
        /// 振動:true 落下:false
        /// </summary>
        public bool StsTestSelection { get; set; }
        /// <summary>
        ///  FDシステム位置移動中
        /// </summary>
        public bool StsFDSystemBusy { get; set; }
        /// <summary>
        /// FDシステム位置 追加 by 稲葉 15-12-14
        /// </summary>
        public bool StsFDSystemPos { get; set; }
        /// <summary>
        /// 空調機水ｵｰﾊﾞｰﾌﾛｰ
        /// </summary>
        public bool StsAirConOverFlow { get; set; }
        /// <summary>
        /// 冷蔵箱扉閉 追加 by 稲葉 15-03-12
        /// </summary>
        public bool StsColdBoxDoorClose { get; set; }
        /// <summary>
        /// 冷蔵箱正規位置 追加 by 稲葉 15-03-12
        /// </summary>
        public bool StsColdBoxPosOK { get; set; }
        /// <summary>
        /// 検査室入室安全ｽｲｯﾁ '追加 by 稲葉 14-03-05  /
        /// </summary>
        public bool StsRoomInSw { get; set; }
        /// <summary>
        /// GYTオンライン用
        /// </summary>
        public OnlineCTGYT OnlineCTGYT { get; set; }
    }

    /// <summary>
    /// ＧＹＴオンライン
    /// </summary>
    public class OnlineCTGYT
    {
        /// 電池検査用　異常ﾘｾｯﾄ要求　 追加 by 稲葉 16-04-14
        public bool ErrResetReq { get; set; }
        /// 電池検査用　時刻設定要求　 追加 by 稲葉 16-04-14
        public bool TimeSetReq { get; set; }
        /// 電池検査用　ｽｷｬﾝ(ﾃｨ-ﾁﾝｸﾞ)停止要求　 追加 by 稲葉 16-04-14
        public bool ScanStopReq { get; set; }
        /// 電池検査用　ｽｷｬﾝ(ﾃｨ-ﾁﾝｸﾞ)開始要求　 追加 by 稲葉 16-04-14
        public bool ScanStartReq { get; set; }
        /// 電池検査用　自動運転中　 追加 by 稲葉 16-04-14
        public bool stsTeachingOpeBusy { get; set; }
        /// 電池検査用
        public bool stsAutoOpeBusy { get; set; }
        /// 電池検査用　X線ｳｫｰﾑｱｯﾌﾟ要求　 追加 by 稲葉 16-04-14
        public bool XrayWarmupReq { get; set; }
        /// 電池検査用　ﾃｨｰﾁﾝｸﾞ運転ﾓｰﾄﾞ　 追加 by 稲葉 16-04-14
        public bool stsTeachingOpeMode { get; set; }
        /// 電池検査用　手動運転ﾓｰﾄﾞ　 追加 by 稲葉 16-04-14
        public bool stsManualOpeMode { get; set; }
        /// 電池検査用　ﾜｰｸ反転縦-90°位置　 追加 by 稲葉 16-04-14
        public bool stsWorkTurningNPos { get; set; }
        /// 電池検査用　ﾜｰｸ反転縦90°位置　 追加 by 稲葉 16-04-14
        public bool stsWorkTurningPPos { get; set; }
        /// 電池検査用　ﾜｰｸ反転水平0°位置　 追加 by 稲葉 16-04-14
        public bool stsWorkTurningHPos { get; set; }
        /// 電池検査用　ｺﾘﾒｰﾀ下原点復帰完了　 追加 by 稲葉 16-04-14
        public bool stsColiDOriginOK { get; set; }
        /// 電池検査用　ｺﾘﾒｰﾀ上原点復帰完了　 追加 by 稲葉 16-04-14
        public bool stsColiUOriginOK { get; set; }
        /// 電池検査用　自動運転ﾓｰﾄﾞ　 追加 by 稲葉 16-04-14
        public bool stsAutoOpeMode { get; set; }
        /// 電池検査用　ｺﾘﾒｰﾀ下異常　 追加 by 稲葉 16-04-14
        public bool stsColiDErr { get; set; }
        /// 電池検査用　ﾜｰｸ反転原点復帰完了　 追加 by 稲葉 16-05-12
        public bool stsWorkTurningOriginOK { get; set; }
        /// 電池検査用　試料扉ｴﾘｱｾﾝｻ状態　 追加 by 稲葉 16-06-24
        public bool stsAreaSensorDark { get; set; }
        /// 電池検査用　処理開始日時 分秒　 追加 by 稲葉 16-04-14
        public int stsStartNSTime { get; set; }
        /// 電池検査用　CT装置異常　 追加 by 稲葉 16-06-01
        public bool stsCTError { get; set; }
        /// 電池検査用　処理開始日時 日時　 追加 by 稲葉 16-04-14
        public int stsStartDHTime { get; set; }
        /// 電池検査用　PLC現在時刻 分秒　 追加 by 稲葉 16-04-14
        public int stsPlcNSTime { get; set; }
        /// 電池検査用　PLC現在時刻 日時　 追加 by 稲葉 16-04-14
        public int stsPlcDHTime { get; set; }
        /// 電池検査用　PLC現在時刻 年月　 追加 by 稲葉 16-04-14
        public int stsPlcYMTime { get; set; }
        public string stsWorkSerialNo { get; set; }
        /// 電池検査用　ｺﾘﾒｰﾀ下現在値　 追加 by 稲葉 16-04-14
        public int stsColiDPosition { get; set; }
        /// 電池検査用　ｺﾘﾒｰﾀ上現在値　 追加 by 稲葉 16-04-14
        public int stsColiUPosition { get; set; }
        /// 電池検査用　処理開始日時 年月　 追加 by 稲葉 16-04-14
        public int stsStartYMTime { get; set; }
        /// 電池検査用　ｺﾘﾒｰﾀ上異常　 追加 by 稲葉 16-04-14
        public bool stsColiUErr { get; set; }
    }

}
