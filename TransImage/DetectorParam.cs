using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransImage
{
    /// <summary>
    /// X線検出器の種類
    /// </summary>
    public enum DetectorConstants : int
    {
        DetTypeII = 0,      // I.I.
        DetTypeHama = 1,    // 浜ホトFPD
        DetTypePke = 2,     // パーキンエルマーFPD
    }

     /// <summary>
    /// 検出器パラメータ
    /// </summary>
    public class DetectorParam
    {
        private DetectorConstants _DetType = DetectorConstants.DetTypeII;
        private float _frameRate = 15;
    
        /// <summary>
        /// X線検出器の種類
        /// </summary>
        public DetectorConstants DetType 
        {
            get { return _DetType; }
            set
            {
                _DetType = value;
                if (_DetType == DetectorConstants.DetTypeII)
                {
                    Use_FlatPanel = false;
                }
                else
                {
                    Use_FlatPanel = true;
                }
            }
        }

 
        /// <summary>
        /// 検出器フラットパネル
        /// </summary>
        //public bool Use_FlatPanel { get; private set; }   //ｸﾞﾛｰﾊﾞﾙに変更2014/10/31hata
        public bool Use_FlatPanel { get; set; }

        //public double hm { get; set; }

        //public double vm { get; set; }

        //public double phm { get; set; }

        //public double pvm { get; set; }

        //public double fphm { get; set; }

        //public double fpvm { get; set; }

        //public readonly double[] FR = new double[2];

        public float hm { get; set; }

        public float vm { get; set; }

        public float phm { get; set; }

        public float pvm { get; set; }

        public float fphm { get; set; }

        public float fpvm { get; set; }

        public readonly float[] FR = new float[2];

        public readonly string[] dcf = new string[2];

        //Pkeの校正ファイル    //追加2014/10/07hata_v19.51反映
        private string _Gain_Correct_File = null;
        //private string _Gain_Correct_Sft_File = null;
        //Rev23.20 左シフトと右シフトで区別する by長野 2015/11/19
        private string _Gain_Correct_Sft_L_File = null;
        private string _Gain_Correct_Sft_R_File = null;
        private string _Gain_Correct_L_File = null;
        //private string _Gain_Correct_L_Sft_File = null;
        //Rev23.20 左シフトと右シフトで区別する by長野 2015/11/19
        private string _Gain_Correct_L_Sft_L_File = null;
        private string _Gain_Correct_L_Sft_R_File = null;
        private string _Gain_Correct_Aire_File = null;
        private string _Off_Correct_File = null;
        private string _Def_Correct_File = null;

        /// <summary>
        /// 透視画像サイズ横
        /// </summary>
        public int h_size { get; set; }

        /// <summary>
        /// 透視画像サイズ縦
        /// </summary>
        public int v_size { get; set; }

        /// <summary>
        /// フレームグラバー種類(0:None 1:FG 2:FGX 3:FGE)
        /// </summary>
        public int v_capture_type { get; set; }

        /// <summary>
        /// 透視画像左右反転フラグ(FPDの場合だけ有効)
        /// </summary>
        public bool IsLRInverse { get; set; }

        /// <summary>
        /// FPDの全画素データを使う (True:使う, False:使わない)  
        /// </summary>
        public bool Use_FpdAllpix { get; set; }
 

        ///// <summary>
        ///// 電源チェック
        ///// </summary>
        //public bool PowerSupplyOK { get; set; }

        /// <summary>
        /// 検出器フレームレートの設定（デフォルト：１５）
        /// </summary>
        public float FrameRate 
        { 
            get {return  _frameRate; }

            set { _frameRate = value; } 
        }

        /// <summary>
        /// 検出器フレームレートの設定（スキャンに使用するフレームレート）
        /// </summary>
        public float FrameRateForScan { get; set; }


        //追加2014/10/07hata_v19.51反映
        /// <summary>
        /// Pkeのゲイン校正ファイル
        /// </summary>
        public string Gain_Correct_File
        {
            get { return _Gain_Correct_File; }
            set { _Gain_Correct_File = value; }
        }

        ///// <summary>
        ///// Pkeのゲイン(シフト用)校正ファイル
        ///// </summary>
        //public string Gain_Correct_Sft_File
        //{
        //    get { return _Gain_Correct_Sft_File; }
        //    set { _Gain_Correct_Sft_File = value; }
        //}

        //Rev23.20 左シフトと右シフト用にファイルを区別する by長野 2015/11/19
        /// <summary>
        /// Pkeのゲイン(左シフト用)校正ファイル
        /// </summary>
        public string Gain_Correct_Sft_L_File
        {
            get { return _Gain_Correct_Sft_L_File; }
            set { _Gain_Correct_Sft_L_File = value; }
        }

        /// <summary>
        /// Pkeのゲイン(右シフト用)校正ファイル
        /// </summary>
        public string Gain_Correct_Sft_R_File
        {
            get { return _Gain_Correct_Sft_R_File; }
            set { _Gain_Correct_Sft_R_File = value; }
        }

        /// <summary>
        /// Pkeのゲイン(Long型用)校正ファイル
        /// </summary>
        public string Gain_Correct_L_File
        {
            get { return _Gain_Correct_L_File; }
            set { _Gain_Correct_L_File = value; }
        }

        ///// <summary>
        ///// Pkeのゲイン(Long型用)(シフト用)校正ファイル
        ///// </summary>
        //public string Gain_Correct_L_Sft_File
        //{
        //    get { return _Gain_Correct_L_Sft_File; }
        //    set { _Gain_Correct_L_Sft_File = value; }
        //}

        //Rev23.20 右シフトと左シフト用でファイルを区別する by長野 2015/11/19
        /// <summary>
        /// Pkeのゲイン(Long型用)(左シフト用)校正ファイル
        /// </summary>
        public string Gain_Correct_L_Sft_L_File
        {
            get { return _Gain_Correct_L_Sft_L_File; }
            set { _Gain_Correct_L_Sft_L_File = value; }
        }

        /// <summary>
        /// Pkeのゲイン(Long型用)(右シフト用)校正ファイル
        /// </summary>
        public string Gain_Correct_L_Sft_R_File
        {
            get { return _Gain_Correct_L_Sft_R_File; }
            set { _Gain_Correct_L_Sft_R_File = value; }
        }

        /// <summary>
        /// Pkeのゲイン校正後に撮影したエアーの画像ファイル 
        /// </summary>
        public string Gain_Correct_Aire_File
        {
            get { return _Gain_Correct_Aire_File; }
            set { _Gain_Correct_Aire_File = value; }
        }

        /// <summary>
        /// Pkeのオフセット画像ファイル
        /// </summary>
        public string Off_Correct_File
        {
            get { return _Off_Correct_File; }
            set { _Off_Correct_File = value; }
        }

        /// <summary>
        /// Pkeの欠陥画像ファイル
        /// </summary>
        public string Def_Correct_File
        {
            get { return _Def_Correct_File; }
            set { _Def_Correct_File = value; }
        }


    }
}
