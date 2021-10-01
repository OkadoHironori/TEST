using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransImage
{
    public class ScanParam
    {
        /// <summary>
        /// アベレージ枚数
        /// </summary>
        public int AverageNum { get; set; }

        /// <summary>
        /// アベレージングあり
        /// </summary>
        public bool AverageOn { get; set; }

        /// <summary>
        /// シャープあり
        /// </summary>
        public bool SharpOn { get; set; }

        /// <summary>
        /// ラインプロファイルあり //Rev25.00 追加 by長野 2016/08/08
        /// </summary>
        public bool LineProfOn { get; set; }

        /// <summary>
        /// ゲイン補正あり
        /// </summary>
        public bool FPGainOn { get; set; }

        /// <summary>
        /// FPDゲイン
        /// </summary>
        public int fpd_gain { get; set; }

        /// <summary>
        /// FPD積分時間
        /// </summary>
        public int fpd_integ { get; set; }

        //使用していない
        /// <summary>
        /// データモード
        /// </summary>
        public int data_mode { get; set; }

        //-------------------------
        //画像処理フラグ
        //-------------------------
        /// <summary>
        /// 動画保存時間
        /// </summary>
        public int MovieSaveTime { get; set; }

        /// <summary>
        /// フレームレートのインデックス値 
        /// </summary>
        public int gFrameRateIndex { get; set; }


        //-------------------------
        //透視画像処理用設定値
        //-------------------------
        /// <summary>
        /// 積算枚数（積分枚数）
        /// </summary>
        public int FIntegNum { get; set; }

        /// <summary>
        /// エッジフィルタ番号保存用
        /// </summary>
        public int EdgeFilterNo { get; set; }

        /// <summary>
        /// 微分フィルタ番号保存用
        /// </summary>
        public int DiffFilterNo { get; set; }
        
        //追加2014/10/07hata_v19.51反映
        // by長野 v19.51 2014/03/19
        /// <summary>
        /// /何枚の積分枚数でキャプチャしたか記憶させるための変数
        /// </summary>
        public int bakIntegNum { get; set; }


    }
}
