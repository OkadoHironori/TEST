using System;
using System.Threading;
using System.Diagnostics;

namespace XrayCtrl
{
    public class clsTActiveX : IDisposable
    {
        //Private frmTos As FormTos
        //Private WithEvents frmTosTimer As Timer
        private ThreadEx frmTosTimer;

        /// <summary>
        /// スレッドの周期
        /// </summary>
        private const int Interval = 100;

        /// <summary>
        /// 排他用オブジェクト
        /// </summary>
        internal static object gLock = new object();

        //追加2014/06/09_hata
        /// <summary>
        //スレッドの起動変数
        /// </summary>
        private bool TosTimerFlg = false;       // スレッドONフラグ
        //private bool TosTimerEndFlg = false;    // スレッド終了フラグ

        //Event ErrDisp(ErrDispNo As Integer)

        ////// --------------------------------------------------------
        //       構造体定義ファイル
        //   <概要>
        //       各メソッド・イベントで使用する構造体を定義する
        //
        //   Date        Version     Designed/Changed
        //   99/08/19    1.00        (TOSFEC)Masaki Okamura
        //   00/03/14    2.00        (NSD)Tsutomu Shibui ･･･ 8160FP対応
        //// --------------------------------------------------------
        ////                  ----- Methods -------
        // 2-1 X線情報設定メソッド
        public struct XrayUpDownSet
        {
            public int m_kVUp;     // 管電圧UP   (0:処理なし  1:+1)
            public int m_kVDown;   // 管電圧Down (0:処理なし  1:-1)
            public int m_mAUp;     // 管電流UP   (0:処理なし  1:+1)
            public int m_mADown;   // 管電流Down (0:処理なし  1:-1)
        }

        // 2-2 X線直接指定設定メソッド
        public struct XrayValueSet
        {
            public float m_kVSet;  // 管電圧設定値
            public float m_mASet;  // 管電流設定値
        }
        
        // 2-6 ユーザ設定メソッド
        public struct UserValueSet
        {
            public float m_XYStepSet;          // X/Y軸ｽﾃｯﾌﾟ移動時の移動量
            public float m_IISpeedSet;         // I.I.昇降速度
            public float m_XtubeSpeedSet;      // ﾃｰﾌﾞﾙZ軸移動速度 Rev4 ← X線管昇降速度
            public int m_IIXLockSet;           // II/Z軸機構ﾛｯｸ (1:ﾛｯｸ  0:ｱﾝﾛｯｸ) Rev4 ← II/X線機構ﾛｯｸ (1:ﾛｯｸ  0:ｱﾝﾛｯｸ)
            public int m_XrayModeSet;          // X線ﾀｲﾏ設定    (1:ﾀｲﾏﾓｰﾄﾞ  0:連続)
            public int m_XrayTimeSet;          // X線ﾀｲﾏ設定時間
            public int m_XrayFocusSet;         // X線焦点自動 (1:小  2:大  3:ｵｰﾄ) Rev4 ← X線焦点自動 (0:設定なし  1:小  2:大)
            public int m_XYTblSpeedSet;        // ｼﾞｮｲｽﾃｨｯｸ速度設定 (0:低  1:中  2:高)

        //2000-03-14 T.Shibui 8160FP対応 ->
            public float m_IIThetaSpeedSet;    //I.I.軸旋回速度
            public float m_ShiSpeedSet;        //回転速度
            public float m_CaXSpeedSet;        //補正X軸移動速度
            public float m_CaYSpeedSet;        //補正Y軸移動速度
            public float m_CauXYStepSet;       //補正X･Y軸ﾕｰｻﾞ送り移動時の移動量
            public float m_ShiModeSet;         //回転動作ﾓｰﾄﾞ（1:連続 0:不連続）
        //<- 2000-03-24 T.Shibui
        }

        ////             ----------- Events -------------
        // 4-1 管電圧･管電流更新通知イベント
        public struct XrayValue
        {
            public float m_kVSet;   // 管電圧設定値
            public float m_mASet;   // 管電流設定値
        }

        // 4-2 出力管電圧･出力管電流･メカ位置･拡大率更新通知イベント
        public struct MechData
        {
            public float m_Voltage;    // 出力管電圧
            public float m_Curent;     // 出力管電流
            public int m_XrayOnSet;    // X線ON/OFF (0:OFF  1:ON)
            public int m_XAvail;       // X線ｱﾍﾞｲﾗﾌﾞﾙ (0:範囲外  1:範囲内)
    
            //2004-09-09 Shibui
            //    m_MechMove          As Integer  // 指定位置移動 (0:移動完了  1:移動中)
            //    m_XTblPosi          As Single   // ﾃｰﾌﾞﾙ X座標位置
            //    m_YTblPosi          As Single   // ﾃｰﾌﾞﾙ Y座標位置
            //    m_IIPosi            As Single   // I.I.座標位置
            //    m_XtubePosi         As Single   // Z軸座標位置 Rev ← X線管座標位置
            //    m_ZoomRate          As Integer  // 拡大率
            //    m_IISize            As Integer  // I.I.ｻｲｽﾞ(1:大ｲﾝﾁ  2:小ｲﾝﾁ)
            //    m_WorkPosi          As Single   // ﾜｰｸ位置高さ
            //
            ////2000-03-14 T.Shibui 8160FP対応 ->
            //    m_IIThetaPosi       As Single   // I.I.旋回軸位置
            //    m_ShiPosi           As Single   // 回転軸位置
            //    m_CaXPosi           As Single   //補正X軸位置
            //    m_CaYPosi           As Single   //補正Y軸位置
            //    m_CaXuPosi          As Single   //補正X軸ﾕｰｻﾞ送り移動
            //    m_CaYuPosi          As Single   //補正Y軸ﾕｰｻﾞ送り移動
            ////<- 2000-03-14 T.Shibui

            //2004-09-09 Shibui
            public int m_XrayTargetInf;        //ターゲット電流ステータスの有無
            public float m_XrayTargetInfSTG;   //ターゲット電流
            public int m_XrayTargetLimit;      //ターゲット電流到達
            public int m_XrayVacuumInf;        //真空度情報の有無
            public string m_XrayVacuumSVC;     //真空度
        }

        // 4-4 装置状態通知イベント
        public struct StatusValue
        {
            public int m_XrayType;          // X線ﾀｲﾌﾟ       (0:90kV  1:130kV 2:160kV)
            public float m_XrayMax_kV;      // 最大管電圧値
            public float m_XrayMax_mA;      // 最大管電流値
            public float m_XrayMin_kV;      // 最小管電圧値
            public float m_XrayMin_mA;      // 最小管電流値
            public int m_XrayWarmupSWS;     // ｳｫｰﾑｱｯﾌﾟｽﾃｯﾌﾟ
            public int m_XrayStandby;       // X線ｽﾀﾝﾊﾞｲ     (0:NG  1:OK)
            public int m_XrayStatus;        // X線正常       (1:正常  0:異常)
            public int m_InterLock;         // ｲﾝﾀｰﾛｯｸ       (0:NG  1:OK)
            public int m_WarmUp;            // ｳｫｰﾐﾝｸﾞｱｯﾌﾟ   (0:未完  1:中  2:完)
            public int m_WarmMode;          // ｳｫｰﾑｱｯﾌﾟﾓｰﾄﾞ  (-1:不要  1:2D  2:2W  3:3W)
            public int m_XrayOffMode;       // X線ﾀｲﾏﾓｰﾄﾞ    (0:ﾀｲﾏ  1:連続)
            public string m_XrayOffTime;    // 最新X線OFF日時(YYYY-MM-DD HH:MM)
            //2004-09-09 Shibui
            //    m_IIPowerOn         As Integer  // II電源        (0:OFF  1:ON)
            //    m_Fine              As Integer  // ﾌｧｲﾝｾｯﾄ       (1:ﾌｧｲﾝ  0:ﾌｧｲﾝ外)
            //    m_MechOrigin        As Integer  // 原位置復帰    (0:未完  1:中  2:完了)
            //    m_MechOrgMove       As Integer  // 原位置移動    (0:移動なし  1:移動中)
            //    m_MechStatus        As Integer  // 機構正常      (1:正常  0:異常)
            public int m_PreHeat;           // ﾌﾟﾘﾋｰﾄ        (0:未完  1:完)
            ////2000-04-21 T.Shibui
            //    m_ShiMode           As Integer  // 回転動作ﾓｰﾄﾞ  (0:連続  1:不連続)

        //追加2010/02/22(KSS)hata_L8601対応 ----->
            public int m_XrayOperateSRL;    // ｵﾍﾟﾚｰﾄｽｲｯﾁ状態確認(0：OFF、1:REMOTE、2:LOCAL)
            public int m_XrayRemoteSRB;     // ﾘﾓｰﾄ動作状態確認(0:BUSY、1:READY)
        //追加2010/02/22(KSS)hata_L8601対応 -----<

        //追加2010/05/14(KS1)hata_L9421-02対応 ----->
            public int m_XrayBatterySBT;    // ﾊﾞｯﾃﾘｰﾄ状態確認(0:正常、1:Low)
        //追加2010/05/14(KS1)hata_L9421-02対応 -----<
        }

        // 4-6 ユーザ設定通知イベント
        public struct UserValue
        {
            //2004-09-09 Shibui
            //    m_XYStep            As Single   // X/Y軸ｽﾃｯﾌﾟ移動時の移動量
            //    m_IISpeed           As Single   // I.I.昇降速度
            //    m_XtubeSpeed        As Single   // Z軸昇降速度 Rev4 ← X線管昇降速度
            //    m_IIXLock           As Integer  // I.I./X線機構ﾛｯｸ(1:ﾛｯｸ  0:ｱﾝﾛｯｸ)
            public int m_XrayOffMode;      // X線ﾀｲﾏﾓｰﾄﾞ     (1:ﾀｲﾏﾓｰﾄﾞ  0:連続)
            public int m_XrayTime;         // X線ﾀｲﾏ設定時間
            public int m_XrayFocusSize;    // X線焦点自動(1:小  2:大  3:設定なし)
            //    m_XYTblSpeed        As Integer  // ｼﾞｮｲｽﾃｨｯｸ速度設定(0:低  1:中  2:高)
            //
            ////2000-03-14 T.Shibui 8160FP対応 ->
            //    m_CaXSpeed          As Single   // 補正X軸移動速度
            //    m_CaYSpeed          As Single   // 補正Y軸移動速度
            //    m_IIThetaSpeed      As Single   // I.I.旋回速度
            //    m_ShiSpeed          As Single   // 回転速度
            //    m_CauXYStep         As Single   // 補正X/Y軸ﾕｰｻﾞ送り移動時の移動量
            ////<- 2000-03-14 T.Shibui
        }

        // 4-7 ウォームアップ残時間イベント
        public struct WarmUpTime
        {
            public int m_WarmM;    // 分
            public int m_WarmS;    // 秒
        }

        // 4-8 ActiveX制御エラー発生イベント
        public struct ErrSet
        {
            public int m_ErrNO;    // ｴﾗｰNO
        }

        // 4-11 ActiveX制御エラー発生イベント
        public struct MechMove
        {
            public int m_MechMove;     // ﾒｲﾝからの指定位置移動動作要求
        }

        //2004-09-07 Shibui 9191対応------------------------------------------------------------->
        public struct udtXrayStatus3ValueDisp
        {
            public int m_XrayStatusSBX;        //X軸方向アライメント
            public int m_XrayStatusSBY;        //Y軸方向アライメント
            public int m_XrayStatusSAD;        //アライメントモニタ
            public float m_XrayStatusSOB;      //フォーカス値
            public float m_XrayStatusSVV;      //真空計値
            public long m_XrayStatusSTM;       //電源ON通電時間
            public long m_XrayStatusSXT;       //X線照射時間
            public int m_XrayStatusSHM;        //フィラメント入力確認
            public float m_XrayStatusSHS;      //フィラメント設定電圧確認

        //変更2009/08/31(KSS)hata L10801 ---------->
        //    m_XrayStatusSHT As Integer  //フィラメント通電時間
            public long m_XrayStatusSHT;       //フィラメント通電時間
        //変更2009/08/31(KSS)hata L10801 ----------<
    
            public int m_XrayStatusSAT;        //自動X線停止時間
            public int m_XrayStatusSOV;        //過負荷保護機構
            public int m_XrayStatusSER;        //制御基板異常
    
        //追加2009/10/08(KSS)hata L10801 ---------->
            public int m_XrayStatusSWE;        //ウォームアップ状態
            public string m_XrayStatusSWU;     //ウォームアップ管電圧上昇下降ﾊﾟﾗﾒｰﾀ確認
            public int m_XrayStatusSMV;        //使用上限管電圧読み出し //v15.10追加 byやまおか 2009/11/12
        //追加2009/10/08(KSS)hata L10801 ----------<

        //Rev23.10 L10711 対応 by長野 2015/10/05 ------------->
            public int m_XrayStatusCAX;        //X軸方向アライメント(コンデンサ)
            public int m_XrayStatusCAY;        //Y軸方向アライメント(コンデンサ)
            public int m_XrayStatusSMD;        //フィラメントモード
            public float m_XrayStatusCOB;      //フォーカス値(コンデンサ)
            public float m_XrayStatusSVS;      //フィラメント設定電圧確認(Sモード)
        //Rev23.10 L10711 対応 by長野 2015/10/05 -------------<

        }

        private udtXrayStatus3ValueDisp mudtXrayStatus3ValueDisp;

        ////9191用メソッド指示格納バッファ
        //public Type udtL9191M
        //    intOBJ      As Integer  //フォーカス値
        //    intSAV      As Integer  //フォーカス値を保存する
        //    intOST      As Integer  //フォーカス値を自動的に決定する
        //    intOBX      As Integer  //電子ビームのX方向位置を調整する
        //    intOBY      As Integer  //電子ビームのY方向位置を調整する
        //    intADJ      As Integer  //電子ビームのビームアライメントを調整する
        //    intADA      As Integer  //電子ビームのビームアライメント調整を一括で実施する
        //    intSTP      As Integer  //電子ビームのビームアライメント調整を中止する
        //    intRST      As Integer  //過負荷保護機能を解除する
        //    intWarmUp   As Integer  //ウォームアップ完了状態時にウォームアップを開始する
        //End Type
        //Private mudtXrayL9191M    As udtL9191M

        ////9191用プロパティ情報格納バッファ
        //public Type udtL9191P
        //    intTargetInf    As Integer  //ターゲット電流ステータスの有無
        //    sngTargetInfSTG As Single   //ターゲット電流
        //    intTargetLimit  As Integer  //ターゲット電流値到達
        //    intVacuumInf    As Integer  //真空度情報の有無
        //    strVacuumSVC    As String   //真空度
        //    intSBX          As Integer  //X軸方向アライメント確認
        //    intSBY          As Integer  //Y軸方向アライメント確認
        //    intSAD          As Integer  //アライメントモニタ
        //    sngSOB          As Single   //フォーカス値
        //    sngSVV          As Single   //真空計値
        //    lngSTM          As Long     //電源ON通電時間
        //    lngSXT          As Long     //X線照射時間
        //    intSHM          As Integer  //フィラメント入力確認
        //    sngSHS          As Integer  //フィラメント設定電圧確認
        //    intSHT          As Integer  //フィラメント通電時間
        //    intSAT          As Integer  //自動X線停止時間
        //    intSOV          As Integer  //過負荷保護機構
        //    intSER          As Integer  //制御基板異常
        //End Type
        //変更2009/08/31(KSS)hata L10801 ---------->
        //Private mudtXrayL9191P   As udtL9191P
        private cValue.udtXrayP mudtXrayP;
        //変更2009/08/31(KSS)hata L10801 ----------<

        //2004-09-07 Shibui 9191対応-------------------------------------------------------------<
        ////// --------------------------------------------------------
        //       イベント宣言
        //// --------------------------------------------------------
        public class XrayValueEventArgs : EventArgs             // 管電圧・管電流更新
        {
            public XrayValue XrayValue;
        }
        public delegate void XrayValueDispEventHandler(object sender, XrayValueEventArgs e);
        public event XrayValueDispEventHandler XrayValueDisp;

        public class MechDataEventArgs : EventArgs              // 出力管電圧・出力管電流・メカ位置・拡大率更新
        {
            public MechData MechData;
        }
        public delegate void MechDataDispEventHandler(object sender, MechDataEventArgs e);
        public event MechDataDispEventHandler MechDataDisp;

        public class StatusValueEventArgs : EventArgs           // 装置状態
        {
            public StatusValue StatusValue;
        }
        public delegate void StatusValueDispEventHandler(object sender, StatusValueEventArgs e);
        public event StatusValueDispEventHandler StatusValueDisp;

        public class UserValueEventArgs : EventArgs             // ユーザ設定
        {
            public UserValue UserValue;
        }
        public delegate void UserValueDispEventHandler(object sender, UserValueEventArgs e);
        public event UserValueDispEventHandler UserValueDisp;

        public class WarmUpTimeEventArgs : EventArgs            // ウォームアップ残時間
        {
            public WarmUpTime WarmUpTime;
        }
        public delegate void WarmUpTimeDispEventHandler(object sender, WarmUpTimeEventArgs e);
        public event WarmUpTimeDispEventHandler WarmUpTimeDisp;

        public class ErrSetEventArgs : EventArgs                // ActiveX 制御エラー発生
        {
            public ErrSet ErrSet;
        }
        public delegate void ErrSetDispEventHandler(object sender, ErrSetEventArgs e);
        public event ErrSetDispEventHandler ErrSetDisp;

        //2004-09-09 Shibui
        public class UdtXrayStatus3EventArgs : EventArgs        //L9191/L10801用
        {
            public udtXrayStatus3ValueDisp udtXrayStatus3ValueDisp;
        }
        public delegate void XrayStatus3ValueDispEventHandler(object sender, UdtXrayStatus3EventArgs e);
        public event XrayStatus3ValueDispEventHandler XrayStatus3ValueDisp;
        

        //==================================================
        //状態格納用
        //==================================================
        ////              ----------- Events -------------
        // 4-1 管電圧･管電流更新通知イベント
        internal float tmp_kVSet;     // 管電圧設定値
        internal float tmp_mASet;     // 管電流設定値

        // 4-2 出力管電圧･出力管電流･メカ位置･拡大率更新通知イベント
        internal float tmp_Voltage;       // 出力管電圧
        internal float tmp_Curent;        // 出力管電流
        internal int tmp_XrayOnSet;       // X線ON/OFF (0:OFF  1:ON)
        internal int tmp_XAvail;          // X線ｱﾍﾞｲﾗﾌﾞﾙ (0:範囲外  1:範囲内)

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
        internal int tmp_mechmove;        // 指定位置移動 (0:移動完了  1:移動中)
        internal float tmp_XTblPosi;      // ﾃｰﾌﾞﾙ X座標位置
        internal float tmp_YTblPosi;      // ﾃｰﾌﾞﾙ Y座標位置
        internal float tmp_IIPosi;        // I.I.座標位置
        internal float tmp_XtubePosi;     // Z軸座標位置 Rev4 ← X線管座標位置
        internal int tmp_ZoomRate;        // 拡大率
        internal int tmp_IISize;          // I.I.ｻｲｽﾞ(1:大ｲﾝﾁ  2:小ｲﾝﾁ)
        internal float tmp_WorkPosi;      // ﾜｰｸ高さ位置

        //2000-03-14 T.Shibui 8160FP対応 ->
        internal float tmp_IIthetaPosi;   // I.I.旋回軸位置
        internal float tmp_ShiPosi;       // 回転軸位置
        internal float tmp_CaXPosi;       //補正X軸位置
        internal float tmp_CaYPosi;       //補正Y軸位置
        internal float tmp_CaXuPosi;      //補正X軸ﾕｰｻﾞ送り移動
        internal float tmp_CaYuPosi;      //補正Y軸ﾕｰｻﾞ送り移動
        //<- 2000-03-14 T.Shibui

        //4-3 センサ情報イベント
        internal int tmp_TouchSens;           // 接触ｾﾝｻ     (0:OFF  1:ON)
        internal int tmp_XLeftSens;           // X左限       (0:OFF  1:ON)
        internal int tmp_XHomeSens;           // X原点       (0:OFF  1:ON)
        internal int tmp_XRightSens;          // X右限       (0:OFF  1:ON)
        internal int tmp_YFwdSens;            // Y前進限     (0:OFF  1:ON)
        internal int tmp_YHomeSens;           // Y原点       (0:OFF  1:ON)
        internal int tmp_YBwdSens;            // Y後退限     (0:OFF  1:ON)
        internal int tmp_IIUpperSens;         // II上昇限    (0:OFF  1:ON)
        internal int tmp_IIHomeSens;          // II原点      (0:OFF  1:ON)
        internal int tmp_IILowerSens;         // II下降限    (0:OFF  1:ON)
        internal int tmp_XtubeUpperSens;      // Z軸上昇限 Rev4 ← X線管上昇限 (0:OFF  1:ON)
        internal int tmp_XtubeHomeSens;       // Z軸原点 Rev4 ← X線管原点   (0:OFF  1:ON)
        internal int tmp_XtubeLowerSens;      // Z軸下降限 Rev4 ← X線管下降限 (0:OFF  1:ON)

        //2000-03-14 T.Shibui 8160FP対応 ->
        internal int tmp_ShiHomeSens;         //回転原点         (0:OFF  1:ON)
        internal int tmp_IITheta0Sens;        //I.I.旋回軸0度    (0:OFF  1:ON)
        internal int tmp_IIThetaHomeSens;     //I.I.旋回軸原点   (0:OFF  1:ON)
        internal int tmp_IITheta45Sens;       //I.I.旋回軸45度   (0:OFF  1:ON)
        internal int tmp_CaXRightSens;        //補正X軸 右限     (0:OFF  1:ON)
        internal int tmp_CaXHomeSens;         //補正X軸 原点     (0:OFF  1:ON)
        internal int tmp_CaXLeftSens;         //補正X軸 左限     (0:OFF  1:ON)
        internal int tmp_CaYFwdSens;          //補正Y軸 前進限   (0:OFF  1:ON)
        internal int tmp_CaYHomeSens;         //補正Y軸 原点     (0:OFF  1:ON)
        internal int tmp_CaYBwdSens;          //補正Y軸 後退限   (0:OFF  1:ON)
        //<- 2000-03-14 T.Shibui
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

        // 4-4 装置状態通知イベント
        internal int tmp_XrayType;            // X線ﾀｲﾌﾟ       (0:90kV  1:130kV 2:160kV)
        internal float tmp_XrayMax_kV;        // 最大管電圧値
        internal float tmp_XrayMax_mA;        // 最大管電流値
        internal float tmp_XrayMin_kV;        // 最小管電圧値
        internal float tmp_XrayMin_mA;        // 最小管電流値
        internal int tmp_XrayWarmupSWS;       // ｳｫｰﾑｱｯﾌﾟｽﾃｯﾌﾟ
        internal int tmp_XrayStandby;         // X線ｽﾀﾝﾊﾞｲ     (0:NG  1:OK)
        internal int tmp_XrayStatus;          // X線正常       (1:正常  0:異常)
        internal int tmp_InterLock;           // ｲﾝﾀｰﾛｯｸ       (0:NG  1:OK)
        internal int tmp_WarmUp;              // ｳｫｰﾐﾝｸﾞｱｯﾌﾟ   (0:未完  1:中  2:完)
        internal int tmp_WarmMode;            // ｳｫｰﾑｱｯﾌﾟﾓｰﾄﾞ  (-1:不要  1:2D  2:2W  3:3W)
        internal int tmp_XrayOffMode;         // X線ﾀｲﾏﾓｰﾄﾞ    (0:ﾀｲﾏ  1:連続)
        internal string tmp_XrayOffTime;      // 最新X線OFF日時(YYYY-MM-DD HH:MM)

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//        internal int tmp_IIPowerOn;           // II電源        (0:OFF  1:ON)
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

        internal int tmp_Fine;                // ﾌｧｲﾝｾｯﾄ       (1:ﾌｧｲﾝ  0:ﾌｧｲﾝ外)

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
        internal int tmp_MechOrigin;          // 原位置復帰    (0:未完  1:中  2:完了)
        internal int tmp_MechOrgMove;         // 原位置移動    (0:移動なし  1:移動中)
        internal int tmp_MechStatus;          // 機構正常      (1:正常  0:異常)
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

        internal int tmp_PreHeat;             // ﾌﾟﾘﾋｰﾄ       (0:未完  1:完)

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
        //2000-04-21 T.Shibui
        internal int tmp_ShiMode;             // 回転動作ﾓｰﾄﾞ  (0:連続  1:不連続)
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

        //追加2010/02/22(KSS)hata_L8601対応 ----->
        internal int tmp_XrayOperateSRL;      // ｵﾍﾟﾚｰﾄｽｲｯﾁ状態確認(0：OFF、1:REMOTE、2:LOCAL)
        internal int tmp_XrayRemoteSRB;       // ﾘﾓｰﾄ動作状態確認(0:BUSY、1:READY)
        //追加2010/02/22(KSS)hata_L8601対応 -----<
        //追加2010/05/14(KS1)hata_L9421-02対応 ----->
        internal int tmp_XrayBatterySBT;      // ﾊﾞｯﾃﾘｰﾄ状態確認(0:正常、1:Low)
        //追加2010/05/14(KS1)hata_L9421-02対応 -----<

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
        // 4-5 サービス設定通知イベント
        internal float tmp_XYPulse;           // X/Y軸ﾓｰﾀﾊﾟﾙｽ出力設定値
        internal float tmp_IIPulse;           // I.I.軸ﾓｰﾀﾊﾟﾙｽ出力設定値
        internal float tmp_XtubePulse;        // Z軸ﾓｰﾀﾊﾟﾙｽ出力設定値 Rev4 ← X線管軸ﾓｰﾀﾊﾟﾙｽ出力設定値
        internal int tmp_IIType;              // I.I.ﾀｲﾌﾟ設定 (1:2  2:4/2  3:7/4)
        internal float tmp_Monitor;           // 使用ﾓﾆﾀ縦有効画素ｻｲｽﾞ(mm)
        internal float tmp_IILUsing;          // 使用I.I.縦有効画素ｻｲｽﾞ(mm)
        internal float tmp_IISUsing;          // 使用I.I.縦有効画素ｻｲｽﾞ(mm)
        internal float tmp_ZoomD1;            // 拡大率ﾊﾟﾗﾒｰﾀ D1
        internal float tmp_ZoomD2;            // 拡大率ﾊﾟﾗﾒｰﾀ D2
        internal float tmp_ZoomD3;            // 拡大率ﾊﾟﾗﾒｰﾀ D3
        internal float tmp_ZoomD4;            // 拡大率ﾊﾟﾗﾒｰﾀ D4
        internal float tmp_ZoomD5;            // 拡大率ﾊﾟﾗﾒｰﾀ D5
        internal float tmp_ZoomD6;            // 拡大率ﾊﾟﾗﾒｰﾀ D6

        //2000-03-14 T.Shibui 8160FP対応 ->
        internal float tmp_IIthetaPulse;      // I.I.旋回軸ﾓｰﾀﾊﾟﾙｽ出力設定値
        internal float tmp_ShiPulse;          // 回転軸ﾓｰﾀﾊﾟﾙｽ出力設定値
        internal float tmp_CaXYPulse;         // 補正X/Y軸ﾓｰﾀﾊﾟﾙｽ設定値
        internal float tmp_DiSpan;            // I.I.上部最大半径
        internal float tmp_DjSpan;            // I.I.入力面最大半径
        internal float tmp_Ddu0Span;          // ﾃｰﾌﾞﾙ面からI.I.上限距離(0deg)
        internal float tmp_Ddd0Span;          // ﾃｰﾌﾞﾙ面からI.I.下限距離(0deg)
        internal float tmp_DxSpan;            // X線管半径
        internal float tmp_Ds0Span;           // ﾃｰﾌﾞﾙ下面からX線上限距離
        //<- 2000-03-14 T.Shibui
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

        // 4-6 ユーザ設定通知イベント

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
        internal float tmp_XYStep;            // X/Y軸ｽﾃｯﾌﾟ移動時の移動量
        internal float tmp_IISpeed;           // I.I.昇降速度
        internal float tmp_XtubeSpeed;        // Z軸昇降速度 Rev4 ← X線管昇降速度
        internal int tmp_IIXLock;             // I.I./X線機構ﾛｯｸ(1:ﾛｯｸ  0:ｱﾝﾛｯｸ)
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

        internal int tmp_XrayOffModeU;        // X線ﾀｲﾏﾓｰﾄﾞ     (1:ﾀｲﾏﾓｰﾄﾞ  0:連続)
        internal int tmp_XrayTime;            // X線ﾀｲﾏ設定時間
        internal int tmp_XrayFocusSize;       // X線焦点自動(1:小  2:大  3:設定なし  )

 #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
       internal int tmp_XYTblSpeed;          // ｼﾞｮｲｽﾃｨｯｸ速度設定(0:低  1:中  2:高)

        //2000-03-14 T.Shibui 8160FP対応 ->
        internal float tmp_CaXSpeed;          // 補正X軸移動速度
        internal float tmp_CaYSpeed;          // 補正Y軸移動速度
        internal float tmp_IIThetaSpeed;      // I.I.旋回速度
        internal float tmp_ShiSpeed;          // 回転速度
        internal float tmp_CauXYStep;         // 補正X/Y軸ﾕｰｻﾞ送り移動時の移動量
        //<- 2000-03-14 T.Shibui
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

        // 4-7 ウォームアップ残時間イベント
        internal int tmp_WarmM;       // 分
        internal int tmp_WarmS;       // 秒

        // 4-8 ActiveX制御エラー発生イベント
        internal int tmp_ErrNO;       // ｴﾗｰNO

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
        // 4-9 機構部のソフトリミット情報イベント 2000-03-14 T.Shibui 8160FP対応
        internal int tmp_XleftSoftLimit;          // X 左限               (0:OFF  1:ON)
        internal int tmp_XleftLoSpeed;            // X 左減速動作中
        internal int tmp_XrightSoftLimit;         // X 右限
        internal int tmp_XrightLoSpeed;           // X 右減速動作中
        internal int tmp_YfwdSoftLimit;           // Y 前進限
        internal int tmp_YfwdLoSpeed;             // Y 前進減速動作中
        internal int tmp_YBwdSoftLimit;           // Y 後退限
        internal int tmp_YBwdLoSpeed;             // Y 後退減速動作中
        internal int tmp_IIUpperSoftLimit;        // II 上昇限
        internal int tmp_IIUpperLoSpeed;          // II 上昇減速動作中
        internal int tmp_IILowerSoftLimit;        // II 下降限
        internal int tmp_IILowerLoSpeed;          // II 下降減速動作中
        internal int tmp_XtubeUpperSoftLimit;     // X線管上昇限
        internal int tmp_XtubeUpperLoSpeed;       // X線管上昇減速動作中
        internal int tmp_XtubeLowerSoftLimit;     // X線管下降限
        internal int tmp_XtubeLowerLoSpeed;       // X線管下降減速動作中
        internal int tmp_IItheta0SoftLimit;       // II 旋回垂直限
        internal int tmp_IItheta0LoSpeed;         // II 旋回垂直減速動作中
        internal int tmp_IItheta45SoftLimit;      // II 旋回傾斜限
        internal int tmp_IItheta45LoSpeed;        // II 旋回傾斜減速動作中
        internal int tmp_CaxRightSoftLimit;       // 補正 X 右限
        internal int tmp_CaxRightLoSpeed;         // 補正 X 右減速動作中
        internal int tmp_CaxLeftSoftLimit;        // 補正 X 左限
        internal int tmp_CaxLeftLoSpeed;          // 補正 X 左減速動作中
        internal int tmp_CayFwdSoftLimit;         // 補正 Y 前進限
        internal int tmp_CayFwdLoSpeed;           // 補正 Y 前進減速動作中
        internal int tmp_CayBwdSoftLimit;         // 補正 Y 後退限
        internal int tmp_CayBwdLoSpeed;           // 補正 Y 後退減速動作中
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

        //イベント通知用バッファ
        private XrayValue tmp_XrayValue;        //管電圧・管電流更新
        private MechData tmp_MechData;          //出力管電圧・出力管電流・拡大率更新
        private StatusValue tmp_StatusValue;    //装置状態
        private UserValue tmp_UserValue;        //ユーザ設定
        private WarmUpTime tmp_WarmUpTime;      //ウォームアップ残時間
        private ErrSet tmp_ErrSet;              //ActivX制御エラー発生

        //==================================================
        //プロパティ(Get)
        //==================================================

        //==================================================
        //プリヒート
        //1999-10-07 T.Shibui
        public int Up_PreHeat
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0;
                    return Module1.mcValue.pXPermitWarmup;
                }
            }
        }
      
        //==================================================
        //Z軸タイプ Rev4 ← X線管タイプ    要チェック
        public int Up_XR_type
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0;
                    return Module1.mcValue.pX_type;
                }
            }
        }

        //==================================================
        //設定管電圧
        public float Up_XR_VoltFeedback
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0.0F;
                    return Module1.mcValue.pX_Volt;
                }
            }
        }

        //==================================================
        //設定管電流
        public float Up_XR_CurrentFeedback
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0.0F;
                    return Module1.mcValue.pX_Amp;
                }
            }
        }

        //==================================================
        //ISOWAT制御時の電圧・電流
        public float Up_X_Watt
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0.0F;
                    return Module1.mcValue.pX_Watt;
                }
            }
        }

        //==================================================
        //ファインセット
        public int Up_XR_Fine
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0;
                    return (Module1.mcValue.pX_Fine == 0) ? 0 : 1;
                }
            }
        }

        //==================================================
        //X線ON中
        public int Up_X_On
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0;
                    return (Module1.mcValue.pX_On == 0) ? 0 : 1;
                }
            }
        }

        //==================================================
        //スタンバイ
        public int Up_Standby
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0;
                    return (Module1.mcValue.pStandby == 0) ? 0 : 1;
                }
            }
        }

        //==================================================
        //インターロック
        public int Up_InterLock
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0;
                    return (Module1.mcValue.pInterlock == 0) ? 0 : 1;
                }
            }
        }

        //==================================================
        //焦点
        public int Up_Focussize
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0;

                    switch (Module1.mcValue.pFocussize)
                    {
                        case 1:
                            return 1;
                        case 2:
                            return 2;
                        case 3:
                            return 3;
                        case 4:
                            return 4;
                        default:
                            return 0;
                    }
                }
            }
        }

        //==================================================
        //ウォームアップ中
        public int Up_Warmup
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0;
                    return Module1.mcValue.pWarmup;
                }
            }
        }

        //==================================================
        //X線正常
        public int Up_XR_Status
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0;
                //2004-03-10 Shibui
                //    If mcValue.pXStatus Then
                //        Up_XR_Status = 1
                //    Else
                //        Up_XR_Status = 0
                //    End If
                    return Module1.mcValue.pXStatus;
                }
            }
        }

        //==================================================
        //
        public int Up_Permit_Warmup
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0;
                    return (Module1.mcValue.pXPermitWarmup == 0) ? 0 : 1;
                }
            }
        }

        //==================================================
        //ウォームアップモード
        public int Up_Wrest_Mode
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0;
                    return Module1.mcValue.pWarmup_Mode;
                }
            }
        }

        //==================================================
        //ウォームアップ残時間
        public int Up_Wrest_TimeM
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0;
                    return Module1.mcValue.pWrest_timeM;
                }
            }
        }

        //==================================================
        //ウォームアップ残時間
        public int Up_Wrest_TimeS
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0;
                    return Module1.mcValue.pWrest_timeS;
                }
            }
        }

        //==================================================
        //X線OFFモード
        public int Up_Xcont_Mode
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0;
                    return (Module1.mcValue.pXcont_Mode == 0) ? 0 : 1;
                }
            }
        }

        //==================================================
        //X線OFF時間
        public int Up_Xtimer
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0;
                    return Module1.mcValue.pXtimer;
                }
            }
        }

        //==================================================
        //最新X線OFF日時
        public string Up_X_LastOff
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return null;
                    return Module1.mcValue.pLastOff;
                }
            }
        }

        //==================================================
        //異常発生コード
        public int Up_Err
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0;
                    return Module1.mcValue.pErrsts;
                }
            }
        }
        //==================================================
        //設定管電圧プロパティ
        //
        public float Up_XR_VoltSet
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0.0F;
                    return Module1.mcValue.pcnd_Volt;
                }
            }
        }

        //==================================================
        //設定管電流プロパティ
        //
        public float Up_XR_CurrentSet
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0.0F;
                    return Module1.mcValue.pcnd_Amp;
                }
            }
        }

        //==================================================
        //設定可能電圧
        //
        public float Up_XR_Max_kV
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0.0F;
                    return Module1.mcValue.pXRMaxkV;
                }
            }
        }

        //==================================================
        //設定可能電流
        //
        public float Up_XR_Max_mA
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0.0F;
                    return Module1.mcValue.pXRMaxmA;
                }
            }
        }

        //==================================================
        //X線アベイラブル
        //
        public int Up_X_Avail
        {
            get
            {
                lock (gLock)
                {
                    if (Module1.gTosCount <= 0) return 0;
                    return Module1.mcValue.pXAvail;
                }
            }
        }

        //2004-03-02 Shibui
        //ｳｫｰﾑｱｯﾌﾟｽﾃｯﾌﾟ
        public int Up_XrayWarmupSWS
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.pXrayWarmupSWS;
                }
            }
        }

        //最小管電圧
        public float Up_XR_Min_kV
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.pXRMinkV;
                }
            }
        }

        //最小管電流
        public float Up_XR_Min_mA
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.pXRMinmA;
                }
            }
        }


        //============================================================================
        //   設定可能最大管電圧(kV)
        //============================================================================
        public int Up_XrayScaleMaxkV
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.P_XrayScaleMaxkV;
                }
            }
        }

        //============================================================================
        //   設定可能最大管電流(μA)
        //============================================================================
        public int Up_XrayScaleMaxuA
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.P_XrayScaleMaxuA;
                }
            }
        }        

        //============================================================================
        //   設定可能最小管電圧(kV)
        //============================================================================
        public int Up_XrayScaleMinkV
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.P_XrayScaleMinkV;
                }
            }
        } 
        
        //============================================================================
        //   設定可能最小管電流(μA)
        //============================================================================
        public int Up_XrayScaleMinuA
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.P_XrayScaleMinuA;
                }
            }
        }

        //============================================================================
        //   最大POWER制限が最大（W)かタゲット電流の把握
        //============================================================================
        public int Up_XrayMaxPower
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.P_XrayMaxPower;
                }
            }
        }        

        //============================================================================
        //   フォーカスF1時の最大(W)又はターゲット電流
        //============================================================================
        public float Up_XrayF1MaxPower
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.P_XrayF1MaxPower;
                }
            }
        }

        //============================================================================
        //   フォーカスF2時の最大(W)又はターゲット電流
        //============================================================================
        public float Up_XrayF2MaxPower
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.P_XrayF2MaxPower;
                }
            }
        }

        //============================================================================
        //   フォーカスF3時の最大(W)又はターゲット電流
        //============================================================================
        public float Up_XrayF3MaxPower
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.P_XrayF3MaxPower;
                }
            }
        }

        //============================================================================
        //   フォーカスF4時の最大(W)又はターゲット電流
        //============================================================================
        public float Up_XrayF4MaxPower
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.P_XrayF4MaxPower;
                }
            }
        }

        //============================================================================
        //   ウォーミングアップパターンの時間表示有無
        //============================================================================
        public int Up_XrayWarmupTime
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.P_XrayWarmupTime;
                }
            }
        }        

        //============================================================================
        //   ウォーミングアップパターン１のウォームアップ時間（分）
        //============================================================================
        public int Up_XrayWarmup1Time
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.P_XrayWarmup1Time;
                }
            }
        }         

        //============================================================================
        //   ウォーミングアップパターン２のウォームアップ時間（分）
        //============================================================================
        public int Up_XrayWarmup2Time
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.P_XrayWarmup2Time;
                }
            }
        }

        //============================================================================
        //   ウォーミングアップパターン３のウォームアップ時間（分）
        //============================================================================
        public int Up_XrayWarmup3Time
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.P_XrayWarmup3Time;
                }
            }
        }
        
        //2004-09-09 Shibui
        //***********************************************************
        //   最大Power制限値が最大（W)かターゲット電流の把握
        //***********************************************************
        public int Up_XrayFocusNumber
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpXrayFocusNumber;
                }
            }
        }

        //***********************************************************
        //   アベイラブル管電圧範囲
        //***********************************************************
        public int Up_XrayAvailkV
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpXrayAvailkV;
                }
            }
        }
        
        //***********************************************************
        //   アベイラブル管電流範囲
        //***********************************************************
        public int Up_XrayAvailuA
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpXrayAvailuA;
                }
            }
        }        

        //***********************************************************
        //   X線ON中に設定値を変更した場合のアベイラブル時間
        //***********************************************************
        public int Up_XrayAvailTimeInside
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpXrayAvailTimeInside;
                }
            }
        }        

        //***********************************************************
        //   X線OFFからX線ON時のアベイラブル時間
        //***********************************************************
        public int Up_XrayAvailTimeOn
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpXrayAvailTimeOn;
                }
            }
        }

        //***********************************************************
        //   ターゲット電流ステータスの有無
        //***********************************************************
        public int Up_XrayTargetInf
        {
            get 
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.intTargetInf;
                }
            }
        }        

        //***********************************************************
        //   ターゲット電流
        //***********************************************************
        public float Up_XrayTargetInfSTG
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.sngTargetInfSTG;
                }
            }
        }        

        //***********************************************************
        //   真空度情報の有無
        //***********************************************************
        public int Up_XrayVacuumInf
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.intVacuumInf;
                }
            }
        }        

        //***********************************************************
        //   真空度
        //***********************************************************
        public string Up_XrayVacuumSVC
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.strVacuumSVC;
                }
            }
        }        

        //***********************************************************
        //   X軸方向アライメント確認
        //***********************************************************
        public int Up_XrayStatusSBX
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.intSBX;
                }
            }
        }        

        //***********************************************************
        //   Y軸方向アライメント確認
        //***********************************************************
        public int Up_XrayStatusSBY
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.intSBY;
                }
            }
        }
        //Rev23.10 L10711 対応 by長野 2015/10/05 ---------------->
        //***********************************************************
        //   X軸方向アライメント確認(コンデンサ)
        //***********************************************************
        public int Up_XrayStatusSCX
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.intSCX;
                }
            }
        }

        //***********************************************************
        //   Y軸方向アライメント確認(コンデンサ)
        //***********************************************************
        public int Up_XrayStatusSCY
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.intSCY;
                }
            }
        }

        //***********************************************************
        //   フィラメントモード
        //***********************************************************
        public int Up_XrayStatusSMD
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.intSMD;
                }
            }
        }

        //Rev23.10 L10711 対応 by長野 2015/10/05 ----------------<

        //***********************************************************
        //   アライメントモニタ
        //***********************************************************
        public int Up_XrayStatusSAD
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.intSAD;
                }
            }
        }        
        
        //***********************************************************
        //   フォーカス値
        //***********************************************************
        public float Up_XrayStatusSOB
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.sngSOB;
                }
            }
        }
        
        //Rev23.10 L10711 対応 by長野 2015/10/05
        //***********************************************************
        //   フォーカス値
        //***********************************************************
        public float Up_XrayStatusCOB
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.sngCOB;
                }
            }
        }     

        //***********************************************************
        //   真空計値
        //***********************************************************
        public float Up_XrayStatusSVV
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.sngSVV;
                }
            }
        }        

        //***********************************************************
        //   電源ON通電時間
        //***********************************************************
        public long Up_XrayStatusSTM
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.lngSTM;
                }
            }
        }        
        
        //***********************************************************
        //   X線照射時間
        //***********************************************************
        public long Up_XrayStatusSXT
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.lngSXT;
                }
            }
        }        
        
        //***********************************************************
        //   フィラメント入力確認
        //***********************************************************
        public int Up_XrayStatusSHM
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.intSHM;
                }
            }
        }        

        //***********************************************************
        //   フィラメント設定電圧確認
        //***********************************************************
        public float Up_XrayStatusSHS
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.sngSHS;
                }
            }
        }

        //Rev23.10 L10711 対応 by長野 2015/10/05 ----------------------->
        //***********************************************************
        //   フィラメント設定電圧確認
        //***********************************************************
        public float Up_XrayStatusSVS
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.sngSVS;
                }
            }
        }
        //Rev23.10 L10711 対応 by長野 2015/10/05 -----------------------<


        //***********************************************************
        //   フィラメント通電時間
        //***********************************************************
        //@_@変更L10801_2009/08/31hata ----->
        //public Property Get Up_XrayStatusSHT() As Integer
        //    Up_XrayStatusSHT = mcValue.gpudtXrayStatus.intSHT
        //End Property
        public long Up_XrayStatusSHT
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.lngSHT;
                }
            }
        }
        //@_@変更L10801_2009/08/31hata -----<

        //***********************************************************
        //   自動X線停止時間
        //***********************************************************
        public int Up_XrayStatusSAT
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.intSAT;
                }
            }
        }

        //***********************************************************
        //   過負荷保護機構
        //***********************************************************
        public int Up_XrayStatusSOV
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.intSOV;
                }
            }
        }

        //***********************************************************
        //   制御基板異常
        //***********************************************************
        public int Up_XrayStatusSER
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.intSER;
                }
            }
        }

        //***********************************************************
        //   ウォームアップモードステップ確認
        //***********************************************************
        public int Up_XrayStatusSWS
        {
            //    Up_XrayStatusSWS
            get { return 0; }
        }

        //追加2009/08/31(KSS)hata_L10801対応 ---------->
        //***********************************************************
        //   フィラメント状態確認
        //***********************************************************
        public int Up_XrayStatusFLM
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.intFLM;
                }
            }
        }

        //***********************************************************
        //   ターゲット温度
        //***********************************************************
        public int Up_XrayStatusZT1
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.intZT1;
                }
            }
        }

        //***********************************************************
        //   X線装置型名
        //***********************************************************
        public string Up_XrayStatusTYP
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.strTYP;
                }
            }
        }

        //***********************************************************
        //   ウォームアップ状態確認
        //***********************************************************
        public string Up_XrayStatusSWE
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.intSWE.ToString();
                }
            }
        }

        //***********************************************************
        //   ウォームアップ管電圧上昇下降パラメータ確認
        //***********************************************************
        public string Up_XrayStatusSWU
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.strSWU;
                }
            }
        }

        //***********************************************************
        //   使用上限管電圧読み出し      //v15.10追加 byやまおか 2009/11/12
        //***********************************************************
        public int Up_XrayStatusSMV
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.intSMV;
                }
            }
        }

        //***********************************************************
        //   ステータス自動送信確認
        //***********************************************************
        public string Up_XrayStatusSSA
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpudtXrayStatus.intSSA.ToString();
                }
            }
        }
        //追加2009/08/31(KSS)hata_L10801対応 ----------<


        //追加2010/02/22(KSS)hata_L8601対応
        //***********************************************************
        //   ｵﾍﾟﾚｰﾄｽｲｯﾁ状態確認(0：OFF、1:REMOTE、2:LOCAL)
        //***********************************************************
        public int Up_XrayOperateSRL
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.pXrayOperateSRL;
                }
            }
        }

        //追加2010/02/22(KSS)hata_L8601対応
        //***********************************************************
        //   ﾘﾓｰﾄ動作状態確認(0:BUSY、1:READY)
        //***********************************************************
        public int Up_XrayRemoteSRB
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.pXrayRemoteSRB;
                }
            }
        }

        //追加2010/05/14(KS1)hata_L9421-02対応
        //***********************************************************
        //バッテリー状態確認(0:正常、1:Low)
        //L9421-02/L9181-02用
        //***********************************************************
        public int Up_XrayBatterySBT
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.pXrayBatterySBT;
                }
            }
        }

        //追加2011/09/15(KS1)hata_L10811X線電力制限に対応
        //***********************************************************
        //   X線電力制限
        //***********************************************************
        public int Up_XrayWattageLimit
        {
            get
            {
                lock (gLock)
                {
                    return Module1.mcValue.gpXrayWattageLimit;
                }
            }
        }


        //==================================================
        //2000-03-14 T.Shibui 8160FP対応
        //================================================
        //プロパティ(Let)
        //メソッド
        //================================================

        //==================================================
        //X線ON/OFFメソッド
        //2000-07-11 T.Shibui
        public int Xrayonoff_Set(int minf)
        {
            lock (gLock)
            {
                int ans;
            //logOut "Xrayonoff_Set Start"
                ans = 0;
                if (Module1.gTosCount <= 0)
                {
            //logOut "Xrayonoff_Set gTosCount <= 0 Exit"
                    return 0;
                }

                Module1.mcValue.Xrayonoff_Set = minf;
                return ans;
            //logOut "Xrayonoff_Set End"
            }
        }


        //==================================================
        //Ｘ線情報設定メソッド
        public int XrayUpDown_Set(XrayUpDownSet Val1)
        {
            lock (gLock)
            {
                int ans = 0;
                if (Module1.gTosCount <= 0) return 0;

                if (Val1.m_kVUp == 1)
                {
                    Module1.mcValue.fX_Volt_Up = 1;     //管電圧UP
                }
                else
                {
                    Module1.mcValue.fX_Volt_Down = 1;   //管電圧Down
                }

                if (Val1.m_mAUp == 1)
                {
                    Module1.mcValue.fX_Amp_Up = 1;     //管電流UP
                }
                else
                {
                    Module1.mcValue.fX_Amp_Down = 1;   //管電流Down
                }

                return ans;
            }
        }


        //==================================================
        //X線直接指定設定メソッド
        public int XrayValue_Set(XrayValueSet Val1)
        {
            lock (gLock)
            {
                int ans = 0;
                if (Module1.gTosCount <= 0) return 0;

                Module1.mcValue.fX_Volt = (int)Val1.m_kVSet;      //管電圧
                Module1.mcValue.fX_Amp = (int)Val1.m_mASet;       //管電流

                //XrayValue_Flg = 1 //削除 by 間々田 2006/04/17　未使用

                return ans;
            }
        }


        //2004-09-07 Shibui L9191用-------------------------------------------------------------->
        //***********************************************************
        //   フォーカスOBJを設定する
        //   ウォーミングアップ完了で、F1モードのX線ON中時のみ出力する。
        //***********************************************************
        public int XrayOBJ_Set(float Val1)
        {
            lock (gLock)
            {
                int intAns = -1;
                //追加2009/08/31(KSS)hata L10801 ---------->
                float sngMax = 0;
                float sngMin = 0;

                if (Module1.gTosCount <= 0) return intAns;

                //変更2009/08/31(KSS)hata L10801 ---------->
                ////2004-09-27 Shibui
                ////    If Val1 > gcintMaxOBJ Or Val1 < gcintMinOBJ Then
                //    If Val1 > (gcintMaxOBJ * 0.1) Or Val1 < (gcintMinOBJ * 0.1) Then
                //
                //        intAns = -1
                //        GoTo ExitHandler
                //    End If
                switch (Module1.mcValue.gpudtXrayStatus.strTYP)
                {
                    case "L9191":
                        sngMax = Module1.gcintMaxOBJ * 0.1f;
                        sngMin = Module1.gcintMinOBJ * 0.1f;
                        break;
                    case "L10801":
                        sngMax = (float)Module1.gcintMaxOBJL10801;
                        sngMin = (float)Module1.gcintMinOBJL10801;
                        break;
                    case "L12721":
                        sngMax = (float)Module1.gcintMaxOBJL12721;
                        sngMin = (float)Module1.gcintMinOBJL12721;
                        break;
                    case "L10711":
                        sngMax = (float)Module1.gcintMaxOBJL10711;
                        sngMin = (float)Module1.gcintMinOBJL10711;
                        break;
                    default:
                        return intAns;
                }

                if (Val1 > sngMax || Val1 < sngMin) return intAns;
                //変更2009/08/31(KSS)hata L10801 ----------<

                //2004-09-27 Shibui
                //    mcValue.gmintOBJ = Val1
                Module1.mcValue.gmsngOBJ = Val1;
                intAns = 0;

                return intAns;
            }
        }

        //***********************************************************
        //   フォーカス値を保存する
        //***********************************************************
        public int XraySAV_Set(int Val1)
        {
            lock (gLock)
            {
                int intAns;

                if (Module1.gTosCount <= 0)
                {
                    intAns = -1;
                }
                else
                {
                    Module1.mcValue.gmintSAV = Val1;
                    intAns = 0;
                }

                return intAns;
            }
        }

        //***********************************************************
        //   フォーカス値を自動的に決定する
        //***********************************************************
        public int XrayOST_Set(int Val1)
        {
            lock (gLock)
            {
                int intAns;

                if (Module1.gTosCount <= 0)
                {
                    intAns = -1;
                }
                else
                {
                    Module1.mcValue.gmintOST = Val1;
                    intAns = 0;
                }

                return intAns;
            }
        }

        //***********************************************************
        //   電子ビームのＸ方向位置を調整する
        //***********************************************************
        public int XrayOBX_Set(int Val1)
        {
            lock (gLock)
            {
                int intAns;
                //追加2009/08/31(KSS)hata L10801 ---------->
                int intMax;
                int intMin;

                if (Module1.gTosCount <= 0)
                {
                    intAns = -1;
                    return intAns;
                }

                //変更2009/08/31(KSS)hata L10801 ---------->
                //    If Val1 > gcintMaxOBX Or Val1 < gcintMinOBX Then
                //        intAns = -1
                //        GoTo ExitHandler
                //    End If
                switch (Module1.mcValue.gpudtXrayStatus.strTYP)
                {
                    case "L9191":
                        intMax = Module1.gcintMaxOBX;
                        intMin = Module1.gcintMinOBX;
                        break;
                    case "L10801":
                        intMax = Module1.gcintMaxOBXL10801;
                        intMin = Module1.gcintMinOBXL10801;
                        break;
                    case "L12721":
                        intMax = Module1.gcintMaxOBXL12721;
                        intMin = Module1.gcintMinOBXL12721;
                        break;
                    case "L10711":
                        intMax = Module1.gcintMaxOBXL10711;
                        intMin = Module1.gcintMinOBXL10711;
                        break;
                    default:
                        intAns = -1;
                        return intAns;
                }

                if (Val1 > intMax || Val1 < intMin)
                {
                    intAns = -1;
                    return intAns;
                }
                //変更2009/08/31(KSS)hata L10801 ----------<

                Module1.mcValue.gmintOBX = Val1;
                intAns = 0;

                return intAns;
            }
        }

        //***********************************************************
        //   電子ビームのＹ方向位置を調整する
        //***********************************************************
        public int XrayOBY_Set(int Val1)
        {
            lock (gLock)
            {
                int intAns;
                //追加2009/08/31(KSS)hata L10801 ---------->
                int intMax;
                int intMin;

                if (Module1.gTosCount <= 0)
                {
                    intAns = -1;
                    return intAns;
                }

                //変更2009/08/31(KSS)hata L10801 ---------->
                //    If Val1 > gcintMaxOBY Or Val1 < gcintMinOBY Then
                //        intAns = -1
                //        GoTo ExitHandler
                //    End If
                switch (Module1.mcValue.gpudtXrayStatus.strTYP)
                {
                    case "L9191":
                        intMax = Module1.gcintMaxOBY;
                        intMin = Module1.gcintMinOBY;
                        break;
                    case "L10801":
                        intMax = Module1.gcintMaxOBYL10801;
                        intMin = Module1.gcintMinOBYL10801;
                        break;
                    case "L12721":
                        intMax = Module1.gcintMaxOBYL12721;
                        intMin = Module1.gcintMinOBYL12721;
                        break;
                    case "L10711":
                        intMax = Module1.gcintMaxOBYL10711;
                        intMin = Module1.gcintMinOBYL10711;
                        break;
                    default:
                        intAns = -1;
                        return intAns;
                }

                if (Val1 > intMax || Val1 < intMin)
                {
                    intAns = -1;
                    return intAns;
                }
                //変更2009/08/31(KSS)hata L10801 ----------<

                Module1.mcValue.gmintOBY = Val1;
                intAns = 0;

                return intAns;
            }
        }

        //***********************************************************
        //   電子ビームのＸ方向位置を調整する(コンデンサ)
        //***********************************************************
        public int XrayCAX_Set(int Val1)
        {
            lock (gLock)
            {
                int intAns;
                //追加2009/08/31(KSS)hata L10801 ---------->
                int intMax;
                int intMin;

                if (Module1.gTosCount <= 0)
                {
                    intAns = -1;
                    return intAns;
                }

                //変更2009/08/31(KSS)hata L10801 ---------->
                //    If Val1 > gcintMaxOBX Or Val1 < gcintMinOBX Then
                //        intAns = -1
                //        GoTo ExitHandler
                //    End If
                switch (Module1.mcValue.gpudtXrayStatus.strTYP)
                {
                    case "L10711":
                        intMax = Module1.gcintMaxCAXL10711;
                        intMin = Module1.gcintMinCAXL10711;
                        break;
                    default:
                        intAns = -1;
                        return intAns;
                }

                if (Val1 > intMax || Val1 < intMin)
                {
                    intAns = -1;
                    return intAns;
                }
                //変更2009/08/31(KSS)hata L10801 ----------<

                Module1.mcValue.gmintCAX = Val1;
                intAns = 0;

                return intAns;
            }
        }

        //***********************************************************
        //   電子ビームのＹ方向位置を調整する(コンデンサ)
        //***********************************************************
        public int XrayCAY_Set(int Val1)
        {
            lock (gLock)
            {
                int intAns;
                //追加2009/08/31(KSS)hata L10801 ---------->
                int intMax;
                int intMin;

                if (Module1.gTosCount <= 0)
                {
                    intAns = -1;
                    return intAns;
                }

                //変更2009/08/31(KSS)hata L10801 ---------->
                //    If Val1 > gcintMaxOBY Or Val1 < gcintMinOBY Then
                //        intAns = -1
                //        GoTo ExitHandler
                //    End If
                switch (Module1.mcValue.gpudtXrayStatus.strTYP)
                {
                    case "L10711":
                        intMax = Module1.gcintMaxCAYL10711;
                        intMin = Module1.gcintMinCAYL10711;
                        break;
                    default:
                        intAns = -1;
                        return intAns;
                }

                if (Val1 > intMax || Val1 < intMin)
                {
                    intAns = -1;
                    return intAns;
                }
                //変更2009/08/31(KSS)hata L10801 ----------<

                Module1.mcValue.gmintOBY = Val1;
                intAns = 0;

                return intAns;
            }
        }

        //Rev23.10 L10711対応 by長野 2015/10/05
        //***********************************************************
        //   フィラメントモードの設定
        //***********************************************************
        public int XrayMDE_Set(int Val1)
        {
            lock (gLock)
            {
                int intAns;

                if (Module1.gTosCount <= 0)
                {
                    intAns = -1;
                    return intAns;
                }

                Module1.mcValue.gmintMDE = Val1;
             
                intAns = 0;

                return intAns;
            }
        }

        //***********************************************************
        //   電子ビームのビームアライメントを調整する
        //***********************************************************
        public int XrayADJ_Set(int Val1)
        {
            lock (gLock)
            {
                int intAns;

                if (Module1.gTosCount <= 0)
                {
                    intAns = -1;
                }
                else if (Val1 != 2 && Val1 != 3)
                {
                    intAns = -1;
                }
                else
                {
                    Module1.mcValue.gmintADJ = Val1;
                    intAns = 0;
                }

                return intAns;
            }
        }

        //***********************************************************
        //   電子ビームのビームアライメント調整を一括で実施する
        //***********************************************************
        public int XrayADA_Set(int Val1)
        {
            lock (gLock)
            {
                int intAns;

                if (Module1.gTosCount <= 0)
                {
                    intAns = -1;
                }
                else
                {
                    Module1.mcValue.gmintADA = Val1;
                    intAns = 0;
                }

                return intAns;
            }
        }

        //***********************************************************
        //   電子ビームのビームアライメント調整を中止する
        //***********************************************************
        public int XraySTP_Set(int Val1)
        {
            lock (gLock)
            {
                int intAns;

                if (Module1.gTosCount <= 0)
                {
                    intAns = -1;
                }
                else
                {
                    Module1.mcValue.gmintSTP = Val1;
                    intAns = 0;
                }

                return intAns;
            }
        }

        //***********************************************************
        //   過負荷保護機能を解除する
        //***********************************************************
        public int XrayRST_Set(int Val1)
        {
            lock (gLock)
            {
                int intAns;

                if (Module1.gTosCount <= 0)
                {
                    intAns = -1;
                }
                else
                {
                    Module1.mcValue.gmintRST = Val1;
                    intAns = 0;
                }

                return intAns;
            }
        }

        //***********************************************************
        //   ウォームアップ完了状態時にウォームアップを開始する
        //***********************************************************
        public int XrayWarmUp_Set(int Val1)
        {
            lock (gLock)
            {
                int intAns;

                if (Module1.gTosCount <= 0)
                {
                    intAns = -1;
                }
                else
                {
                    Module1.mcValue.gmintWarmUp = Val1;
                    intAns = 0;
                }

                return intAns;
            }
        }

        //2004-09-07 Shibui L9191用--------------------------------------------------------------<

        //==================================================
        //ユーザ設定メソッド
        public int UserValue_Set(UserValueSet Val1)
        {
            lock (gLock)
            {
                int ans;

                if (Module1.gTosCount <= 0) return 0;

                ans = 0;

                Module1.mcValue.fXcont_Mode = Val1.m_XrayModeSet;
                Module1.mcValue.fXtimer = Val1.m_XrayTimeSet;
                Module1.mcValue.fFocussize = Val1.m_XrayFocusSet;

                return ans;
            }
        }

        //==================================================
        //コマンドボタン（ウォーミングアップキャンセル）メソッド
        public int WarmUpCancel_Set(int minf)
        {
            lock (gLock)
            {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//               int ans = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                if (Module1.gTosCount <= 0) return 0;

                if (minf == 0)
                {
                    // Nothing
                }
                else if (minf == 1)
                {
                    Module1.mcValue.fWarmupCancel(1);
                }

                return 0;
            }
        }

        //==================================================
        //コマンドボタン（ウォーミングアップ強制終了）メソッド
        public int WarmUpQuit_Set(int minf)
        {
            lock (gLock)
            {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//                int ans = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                if (Module1.gTosCount <= 0) return 0;

                if (minf == 0)
                {
                    // Nothing
                }
                else if (minf == 1)
                {
                    Module1.mcValue.fWarmupReset(1);
                }

                return 0;
            }
        }

        //***********************************************************
        //   アライメント値を保存する
        //
        //  追加L10801_2009/10/08(KSS)hata
        //***********************************************************
        public int XrayAlignmentSAV_Set(int Val1)
        {
            lock (gLock)
            {
                int intAns;

                if (Module1.gTosCount <= 0)
                {
                    intAns = -1;
                }
                else
                {
                    Module1.mcValue.gmintAlignmentSAV = Val1;
                    intAns = 0;
                }

                return intAns;
            }
        }

        //***********************************************************
        //   デフォルト値読み出し
        //
        //  追加L10801_2009/10/08(KSS)hata
        //***********************************************************
        public int XrayDDL_Set(int Val1)
        {
            lock (gLock)
            {
                int intAns;

                if (Module1.gTosCount <= 0)
                {
                    intAns = -1;
                }
                else
                {
                    Module1.mcValue.gmintDDL = Val1;
                    intAns = 0;
                }

                return intAns;
            }
        }

        //***********************************************************
        //   上限管電圧を制限する
        //
        //  追加L10801_2009/11/13 やまおか
        //***********************************************************
        public int XrayCMV_Set(int Val1)
        {
            lock (gLock)
            {
                int intAns = -1;
                int intMax = 0;
                int intMin = 0;

                if (Module1.gTosCount <= 0)
                {
                    return intAns;
                }

                switch (Module1.mcValue.gpudtXrayStatus.strTYP)
                {
                    case "L9191":
                    case "L10801":
                    case "L12721"://Rev23.10 追加 by長野 2015/11/08
                        intMax = (int)Module1.mcValue.pXRMaxkV;
                        intMin = (int)Module1.mcValue.pXRMinkV;
                        break;
                    default:
                        return intAns;
                }

                if (Val1 > intMax || Val1 < intMin)
                {
                    return intAns;
                }

                Module1.mcValue.gmintCMV = Val1;
                intAns = 0;

                return intAns;
            }
        }

        //==================================================
        //状態要求メソッド
        //
        public int X_AllEventRaise_Set(int minf)
        {
            lock (gLock)
            {
                int ans = 0;
                if (Module1.gTosCount <= 0) return 0;

                if (minf == 0)
                {
                    ans = 0;
                }
                else if (minf == 1)
                {
                    //全てのイベント通知を実行
                    //2004-09-15 Shibui                
                    TmpIni();
                    //      ans = 0
                    //      e_XrayValue
                    //      e_MechData
                    //      e_StatusValue
                    //      e_UserValue
                    //      e_WarmUpTime
                    //      e_ErrSet
                    ////2004-09-09 Shibui
                    //      e_XrayStatus3ValueDisp
                }
                else
                {
                    ans = 2;
                }

                return ans;
            }
        }

        //==================================================
        //メッセージ確認メソッド
        public int MessageOk_Set(int minf)
        {
            lock (gLock)
            {
                if (Module1.gTosCount <= 0) return 0;

                Module1.mcValue.fErrrst = minf;
                return 0;
            }
        }

        //==================================================
        //イベント処理動作メソッド
        public int EventValue_Set(int minf)
        {
            lock (gLock)
            {
                int ans = 0;

                switch (minf)
                {
                    case 0:
                        //処理無し
                        break;
                    case 1:
                        //FeinFoucus用ｲﾍﾞﾝﾄ開始
                        Module1.EventValue = 1;
                        Module1.mcValue.fEventvalue = minf;
                        break;
                    case 2:
                        //ｲﾍﾞﾝﾄ停止
                        if (Module1.EventValue != 2)
                        {
                            Module1.EventValue = 2;
                            Module1.mcValue.fEventvalue = minf;
                        }
                        break;
                    case 3:
                        //Kvex用ｲﾍﾞﾝﾄ開始
                        Module1.EventValue = 3;
                        Module1.mcValue.fEventvalue = minf;
                        break;
                    //2004-02-14 Shibui
                    case 4:
                        //浜ﾎﾄ（130kV L9421）のｲﾍﾞﾝﾄ開始
                        Module1.EventValue = 4;
                        Module1.mcValue.fEventvalue = minf;
                        break;
                    //2004-09-05 Shibui ------------------------>
                    case 5:
                        //浜ﾎﾄ（130kV L9181）のｲﾍﾞﾝﾄ開始
                        Module1.EventValue = 5;
                        Module1.mcValue.fEventvalue = minf;
                        break;
                    case 6:
                        //浜ﾎﾄ（130kV L9191）のｲﾍﾞﾝﾄ開始
                        Module1.EventValue = 6;
                        Module1.mcValue.fEventvalue = minf;
                        break;
                    //2004-09-05 Shibui ------------------------<

                    //追加2009/08/19(KSS)hata_L10801対応 ---------->
                    case 7:
                        //浜ﾎﾄ（230kV L10801）のｲﾍﾞﾝﾄ開始
                        Module1.EventValue = 7;
                        Module1.mcValue.fEventvalue = minf;
                        break;
                    //追加2009/08/19(KSS)hata_L10801対応 ----------<

                    //追加2010/02/22(KSS)hata_L8601対応 ---------->
                    case 8:
                        //浜ﾎﾄ（90kV L8601）のｲﾍﾞﾝﾄ開始
                        Module1.EventValue = 8;
                        Module1.mcValue.fEventvalue = minf;
                        break;
                    //追加2010/02/22(KSS)hata_L8601対応 ----------<

                    //追加2010/05/14(KS1)hata_L9421-02対応 ---------->
                    case 9:
                        //浜ﾎﾄ（90kV L9421-02）のｲﾍﾞﾝﾄ開始
                        Module1.EventValue = 9;
                        Module1.mcValue.fEventvalue = minf;
                        break;
                    //追加2010/05/14(KS1)hata_L9421-02対応 ----------<

                    //追加2012/03/20(KS1)hata_L8121-02対応 ---------->
                    case 10:
                        //450kV用に使用
                        Module1.EventValue = 10;
                        Module1.mcValue.fEventvalue = minf;
                        break;
                    case 11:
                        //浜ﾎﾄ（150kV L8121-02）のｲﾍﾞﾝﾄ開始
                        Module1.EventValue = 11;
                        Module1.mcValue.fEventvalue = minf;
                        break;
                    //追加2012/03/20(KS1)hata_L8121-02対応 ----------<

                    //追加2015/02/06(KS1)hata
                    case 12:
                        //浜ﾎﾄ（130kV L9181-02）のｲﾍﾞﾝﾄ開始
                        Module1.EventValue = 12;
                        Module1.mcValue.fEventvalue = minf;
                        break;

                    case 13:
                        //浜ﾎﾄ（300kV L12721）のｲﾍﾞﾝﾄ開始
                        Module1.EventValue = 13;
                        Module1.mcValue.fEventvalue = minf;
                        break;

                    case 14:
                        //浜ﾎﾄ（160kV L10711）のｲﾍﾞﾝﾄ開始
                        Module1.EventValue = 14;
                        Module1.mcValue.fEventvalue = minf;
                        break;

                }

                //2004-03-23 Shibui
                Thread.Sleep(100);

                return ans;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public clsTActiveX()
        {
            lock (gLock)
            {
                Class_Initialize();
            }
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Dispose()
        {
            //変更2014/06/09_hata
            //排他処理しない
            //lock (gLock)
            //{
            //    Class_Terminate();
            //}
            Class_Terminate();
        }

        //==================================================
        //クラス・イニシャライズ
        //
        private void Class_Initialize()
        {
            //    Select Case EventValue
            //    Case 1  //FineFocus
            if (Module1.mcCtrlm == null)
            {
                Module1.mcCtrlm = new XrayCtrl.cCtrlm();
                Module1.mcValue = Module1.mcCtrlm.cValue;
            }

            //If frmTos Is Nothing Then
            //    Set frmTos = New FormTos
            //    Load frmTos
            //    Set frmTosTimer = frmTos.Timer
            //    'mLogic.Init frmTosTimer.Interval   '削除 by 間々田 2006/04/17 無意味
            //    'LogSet = 1                         '削除 by 間々田 2006/04/17 無意味
            //End If
            frmTosTimer = new ThreadEx(new ThreadStart(frmTosTimer_Timer));
            frmTosTimer.Name = this.ToString();

            //追加2014/06/09_hata
            TosTimerFlg = true;

            frmTosTimer.Start();

            Module1.gTosCount = Module1.gTosCount + 1;
            TmpIni();
            //    Case 2  //動作なし
            //
            //    Case 3  //Kevex
            //        If mcKevexCtrlm Is Nothing Then
            //            Set mcKevexCtrlm = New cKevexCtrlm
            //            Set mcValue = mcKevexCtrlm.cValue
            //        End If
            //        If frmTos Is Nothing Then
            //            Set frmTos = New FormTos
            //            Load frmTos
            //            Set frmTosTimer = frmTos.Timer
            //            mLogic.Init frmTosTimer.Interval
            //            LogSet = 1
            //        End If
            //        gTosCount = gTosCount + 1
            //        TmpIni
            //    End Select
        }

        //==================================================
        //クラス・ターミネイト
        //
        private void Class_Terminate()
        {
            ////2004-09-21 Shibui
            //if (frmTosTimer != null)
            //{
            //    TosTimerFlg = false;
            //    if (frmTosTimer != null) frmTosTimer.Stop();
            //    //frmTosTimer.Stop();
            //}

             TosTimerFlg= false;

            //2004-03-23 Shibui
            Module1.gTosCount = Module1.gTosCount - 1;
            if(Module1.gTosCount == 0)
            {
                Module1.mcValue = null;

                //2004-03-23 Shibui
                if (Module1.mcCtrlm != null)
                {
                    Module1.mcCtrlm.Dispose();
                }
                Module1.mcCtrlm = null;
                
                //  Set mcKevexCtrlm = Nothing

            }

            //2004-09-21 Shibui
            if (frmTosTimer != null)
            {
                frmTosTimer.Stop();
            }
            
            Module1.mcValue = null;
            Module1.mcCtrlm = null;
            frmTosTimer = null;
            TosTimerFlg = false;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
                If frmTos Is Nothing Then
                Else
                    Set frmTosTimer = Nothing
                    Unload frmTos
                    Set frmTos = Nothing
            //2004-03-23 Shibui
            //        gTosCount = gTosCount - 1
                End If
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
        
        }


        //==================================================
        //メインルーチン（タイマー）
        //
        private void frmTosTimer_Timer()
        {            
            //Dim sbuf As Single
            //Dim ibuf As Integer

            //Logic  //削除 by 間々田 2006/04/17　無意味

            try
            {
 
                //while (!frmTosTimer.Sleep(Interval))
                while (TosTimerFlg)
                {

				    bool raise_mechdatadisp = true;
				    bool raise_statusvaluedisp = true;
				    bool raise_uservaluedisp = true;

				    XrayValueEventArgs xve = null;
	                MechDataEventArgs mde = null;
				    StatusValueEventArgs sve = null;
				    UserValueEventArgs uve = null;
				    UdtXrayStatus3EventArgs use = null;
				    WarmUpTimeEventArgs wute = null;
				    ErrSetEventArgs ese = null;



                    if (!TosTimerFlg) return;   //追加2014/06/09_hata
                    if (frmTosTimer == null) return;
                    if (frmTosTimer.Stoped) return;
                    if (!frmTosTimer.IsAlive) return;

                    lock (gLock)
                    {
                        if (Module1.EventValue == 0 || Module1.EventValue == 2)
                        {
                            continue;
                        }

                        //---------------------------------------------------------
                        //3.1 管電圧・管電流更新通知イベント
                        //1999-10-21 T.Shibui XrayValue_Flg追加
                        //2004-03-23 Shibui
                        //					If (tmp_kVSet <> mcValue.pcnd_volt) Or (tmp_mASet <> mcValue.pcnd_amp) Or (XrayValue_Flg = 1) Then

                        if (tmp_kVSet != Module1.mcValue.pcnd_Volt || tmp_mASet != Module1.mcValue.pcnd_Amp || Module1.mcValue.P_XrayValueDisp == 1)
                        {
                            //XrayValue_Flg = 0  //削除 by 間々田 2006/04/17　未使用
                            xve = e_XrayValue();
                        }
                    }

                    if (!TosTimerFlg) return;   //追加2014/06/09_hata

                    if (XrayValueDisp != null && xve != null)
                    {
                        XrayValueDisp(this, xve);
                        //Debug.Print("3.1 Event= XrayValue   Raise !!");
                    }

                    lock (gLock)
                    {
                        //---------------------------------------------------------
                        //3.2 出力管電圧・出力管電流等の更新通知イベント

                        raise_mechdatadisp = true;    //イベント発生フラグ初期化

                        //状態変化確認
                        if (tmp_Voltage != Module1.mcValue.pX_Volt) { }
                        //出力管電圧
                        else if (tmp_Curent != Module1.mcValue.pX_Amp) { }
                        //出力管電流
                        else if (tmp_XrayOnSet != Module1.mcValue.pX_On) { }
                        //X線ON/OFF
                        else if (tmp_XAvail != Module1.mcValue.pXAvail) { }
                        //アベイラブル

    //2004-09-09 Shibui
                        else if (mudtXrayP.intTargetInf != Module1.mcValue.gpudtXrayStatus.intTargetInf) { }	//ターゲット電流
                        else if (mudtXrayP.sngTargetInfSTG != Module1.mcValue.gpudtXrayStatus.sngTargetInfSTG) { }
                        else if (mudtXrayP.intTargetLimit != Module1.mcValue.gpudtXrayStatus.intTargetLimit) { }
                        else if (mudtXrayP.intVacuumInf != Module1.mcValue.gpudtXrayStatus.intVacuumInf) { }
                        else if (mudtXrayP.strVacuumSVC != Module1.mcValue.gpudtXrayStatus.strVacuumSVC) { }
                        else
                        {
                            //変化無し
                            raise_mechdatadisp = false;
                        }

                        if (raise_mechdatadisp == true)
                        {
                            mde = e_MechData();
                        }
                    }

                    if (!TosTimerFlg) return;   //追加2014/06/09_hata

                    if (MechDataDisp != null && mde != null)
                    {
                        //Debug.Print("3.2 Event= MechData   Raise !!");
                        MechDataDisp(this, mde);
                    }

                    lock (gLock)
                    {
                        //---------------------------------------------------------
                        //3.3 装置状態通知イベント
                        //
                        raise_statusvaluedisp = true;   //イベント発生フラグ初期化

                        if (tmp_XrayType != Module1.mcValue.pX_type) { }
                        else if (tmp_XrayMax_kV != Module1.mcValue.pXRMaxkV) { }
                        else if (tmp_XrayMax_mA != Module1.mcValue.pXRMaxmA) { }
                        else if (tmp_XrayMin_kV != Module1.mcValue.pXRMinkV) { }
                        else if (tmp_XrayMin_mA != Module1.mcValue.pXRMinmA) { }
                        else if (tmp_XrayWarmupSWS != Module1.mcValue.pXrayWarmupSWS) { }
                        else if (tmp_XrayStandby != Module1.mcValue.pStandby) { }
                        else if (tmp_XrayStatus != Module1.mcValue.pXStatus) { }
                        else if (tmp_InterLock != Module1.mcValue.pInterlock) { }
                        else if (tmp_WarmUp != Module1.mcValue.pWarmup) { }
                        else if (tmp_WarmMode != Module1.mcValue.pWarmup_Mode) { }
                        else if (tmp_XrayOffMode != Module1.mcValue.pXcont_Mode) { }
                        else if (tmp_XrayOffTime != Module1.mcValue.pLastOff) { }
                        //					ElseIf tmp_IIPowerOn <> mcValue.pPow Then
                        //2005-02-03 Shibui
                        //					ElseIf tmp_Fine <> mcValue.pX_Fine Then
                        //					ElseIf tmp_MechOrigin <> mcValue.pOrigin Then
                        //					ElseIf tmp_MechOrgMove <> mcValue.pMechOrgMove Then
                        //1999-10-07 T.Shibui
                        //					ElseIf tmp_MechStatus <> mcValue.pMech_Status Then
                        else if (tmp_PreHeat != Module1.mcValue.pXPermitWarmup) { }
                        //					ElseIf tmp_ShiMode <> mcValue.pShiMode Then

    //追加2010/02/22(KSS)hata_L8601対応 ----->
                        else if (tmp_XrayOperateSRL != Module1.mcValue.pXrayOperateSRL) { }
                        else if (tmp_XrayRemoteSRB != Module1.mcValue.pXrayRemoteSRB) { }
                        //追加2010/02/22(KSS)hata_L8601対応 -----<

    //追加2010/05/14(KS1)hata_L9421-02対応 ----->
                        else if (tmp_XrayBatterySBT != Module1.mcValue.pXrayBatterySBT) { }
                        //追加2010/05/14(KS1)hata_L9421-02対応 -----<

                        else
                        {
                            //状態変化無し
                            raise_statusvaluedisp = false;
                        }

                        if (raise_statusvaluedisp == true)
                        {
                            sve = e_StatusValue();
                        }
                    }

                    if (!TosTimerFlg) return;   //追加2014/06/09_hata

                    if (StatusValueDisp != null && sve != null)
                    {
                        //Debug.Print("3.3 Event= StatusValueDisp   Raise !!");
                        StatusValueDisp(this, sve);
                    }

                    lock (gLock)
                    {
                        //---------------------------------------------------------
                        //3.4 ユーザ設定通知イベント
                        //
                        raise_uservaluedisp = true;    //イベント発生フラグ初期化

                        //					If tmp_XYStep <> mcValue.pStepXY Then
                        //					ElseIf tmp_IISpeed <> mcValue.pII_Speed Then
                        //					ElseIf tmp_XtubeSpeed <> mcValue.pXR_Speed Then
                        //					ElseIf tmp_IIXLock <> mcValue.pMech_Lock Then
                        if (tmp_XrayOffModeU != Module1.mcValue.pXcont_Mode) { }
                        else if (tmp_XrayTime != Module1.mcValue.pXtimer) { }
                        else if (tmp_XrayFocusSize != Module1.mcValue.pFocussize) { }
                        //					ElseIf tmp_XYTblSpeed <> mcValue.pXYTblSpeed Then
                        //2000-03-14 T.Shibui 8160FP対応 ->
                        //					ElseIf tmp_CaXSpeed <> mcValue.pCaxSpeed Then
                        //						//補正X軸移動速度
                        //					ElseIf tmp_CaYSpeed <> mcValue.pCaySpeed Then
                        //						//補正Y軸移動速度
                        //					ElseIf tmp_IIThetaSpeed <> mcValue.pIIthetaSpeed Then
                        //						//I.I.旋回移動速度
                        //					ElseIf tmp_ShiSpeed <> mcValue.pShiSpeed Then
                        //						//回転速度
                        //					ElseIf tmp_CauXYStep <> mcValue.pCauXYStep Then
                        //						//補正X/Y軸ユーザ送り移動時の移動量
                        //<- 2000-03-14 T.Shibui
                        else
                        {
                            //状態変化無し
                            raise_uservaluedisp = false;
                        }

                        if (raise_uservaluedisp == true)
                        {
                            uve = e_UserValue();
                        }
                    }

                    if (!TosTimerFlg) return;   //追加2014/06/09_hata

                    if (UserValueDisp != null && uve != null)
                    {
                        //Debug.Print("3.4 Event= UserValueDisp   Raise !!");
                        //イベント発行
                        UserValueDisp(this, uve);
                    }

                    lock (gLock)
                    {
                        //---------------------------------------------------------
                        //3.5 開放管X線発生装置の更新通知イベント
                        //
                        //@_@変更L10801_2009/08/31hata -----<
                        ////2004-09-09 Shibui
                        //					//L9191用
                        //					With mudtXrayL9191P
                        //						If .intSBX <> mcValue.gpXrayL9191.intSBX _
                        //						Or .intSBY <> mcValue.gpXrayL9191.intSBY _
                        //						Or .intSAD <> mcValue.gpXrayL9191.intSAD _
                        //						Or .sngSOB <> mcValue.gpXrayL9191.sngSOB _
                        //						Or .sngSVV <> mcValue.gpXrayL9191.sngSVV _
                        //						Or .lngSTM <> mcValue.gpXrayL9191.lngSTM _
                        //						Or .lngSXT <> mcValue.gpXrayL9191.lngSXT _
                        //						Or .intSHM <> mcValue.gpXrayL9191.intSHM _
                        //						Or .sngSHS <> mcValue.gpXrayL9191.sngSHS _
                        //						Or .intSHT <> mcValue.gpXrayL9191.intSHT _
                        //						Or .intSAT <> mcValue.gpXrayL9191.intSAT _
                        //						Or .intSOV <> mcValue.gpXrayL9191.intSOV _
                        //						Or .intSER <> mcValue.gpXrayL9191.intSER Then
                        //
                        //							e_XrayStatus3ValueDisp
                        //						End If
                        //					End With
                        //					With mudtXrayP
                        //						If .intSBX <> mcValue.gpudtXrayStatus.intSBX _
                        //						Or .intSBY <> mcValue.gpudtXrayStatus.intSBY _
                        //						Or .intSAD <> mcValue.gpudtXrayStatus.intSAD _
                        //						Or .sngSOB <> mcValue.gpudtXrayStatus.sngSOB _
                        //						Or .sngSVV <> mcValue.gpudtXrayStatus.sngSVV _
                        //						Or .lngSTM <> mcValue.gpudtXrayStatus.lngSTM _
                        //						Or .lngSXT <> mcValue.gpudtXrayStatus.lngSXT _
                        //						Or .intSHM <> mcValue.gpudtXrayStatus.intSHM _
                        //						Or .sngSHS <> mcValue.gpudtXrayStatus.sngSHS _
                        //						Or .lngSHT <> mcValue.gpudtXrayStatus.lngSHT _
                        //						Or .intSAT <> mcValue.gpudtXrayStatus.intSAT _
                        //						Or .intSOV <> mcValue.gpudtXrayStatus.intSOV _
                        //						Or .intSER <> mcValue.gpudtXrayStatus.intSER _
                        //						Or .intSWE <> mcValue.gpudtXrayStatus.intSWE _
                        //						Or .strSWU <> mcValue.gpudtXrayStatus.strSWU Then
                        //v15.10変更 byやまおか 2009/11/12
                        if (mudtXrayP.intSBX != Module1.mcValue.gpudtXrayStatus.intSBX
                        || mudtXrayP.intSBY != Module1.mcValue.gpudtXrayStatus.intSBY
                        || mudtXrayP.intSAD != Module1.mcValue.gpudtXrayStatus.intSAD
                        || mudtXrayP.sngSOB != Module1.mcValue.gpudtXrayStatus.sngSOB
                        || mudtXrayP.sngSVV != Module1.mcValue.gpudtXrayStatus.sngSVV
                        || mudtXrayP.lngSTM != Module1.mcValue.gpudtXrayStatus.lngSTM
                        || mudtXrayP.lngSXT != Module1.mcValue.gpudtXrayStatus.lngSXT
                        || mudtXrayP.intSHM != Module1.mcValue.gpudtXrayStatus.intSHM
                        || mudtXrayP.sngSHS != Module1.mcValue.gpudtXrayStatus.sngSHS
                        || mudtXrayP.lngSHT != Module1.mcValue.gpudtXrayStatus.lngSHT
                        || mudtXrayP.intSAT != Module1.mcValue.gpudtXrayStatus.intSAT
                        || mudtXrayP.intSOV != Module1.mcValue.gpudtXrayStatus.intSOV
                        || mudtXrayP.intSER != Module1.mcValue.gpudtXrayStatus.intSER
                        || mudtXrayP.intSWE != Module1.mcValue.gpudtXrayStatus.intSWE
                        || mudtXrayP.strSWU != Module1.mcValue.gpudtXrayStatus.strSWU
                        || mudtXrayP.intSMV != Module1.mcValue.gpudtXrayStatus.intSMV
                        || mudtXrayP.intSCX != Module1.mcValue.gpudtXrayStatus.intSCX  //Rev23.10 L10711 対応 by長野 2015/10/05
                        || mudtXrayP.intSCY != Module1.mcValue.gpudtXrayStatus.intSCY  //Rev23.10 L10711 対応 by長野 2015/10/05
                        || mudtXrayP.intSMD != Module1.mcValue.gpudtXrayStatus.intSMD  //Rev23.10 L10711 対応 by長野 2015/10/05
                        || mudtXrayP.sngCOB != Module1.mcValue.gpudtXrayStatus.sngCOB  //Rev23.10 L10711 対応 by長野 2015/10/05
                        || mudtXrayP.sngSVS != Module1.mcValue.gpudtXrayStatus.sngSVS) //Rev23.10 L10711 対応 by長野 2015/10/05
                        {
                            use = e_XrayStatus3ValueDisp();
                        }
                        //@_@変更L10801_2009/08/31hata -----<

                    }

                    if (!TosTimerFlg) return;   //追加2014/06/09_hata

                    if (XrayStatus3ValueDisp != null && use != null)
                    {
                        //Debug.Print("3.5 Event= XrayStatus3ValueDisp   Raise !!");
                        XrayStatus3ValueDisp(this, use);
                    }

                    lock (gLock)
                    {
                        //---------------------------------------------------------
                        //ウォームアップ残時間イベント
                        //
                        if (tmp_WarmM != Module1.mcValue.pWrest_timeM || tmp_WarmS != Module1.mcValue.pWrest_timeS)
                        {
                            wute = e_WarmUpTime();
                        }
                    }

                    if (!TosTimerFlg) return;   //追加2014/06/09_hata

                    if (WarmUpTimeDisp != null && wute != null)
                    {
                        WarmUpTimeDisp(this, wute);
                    }

                    lock (gLock)
                    {
                        //---------------------------------------------------------
                        //ActiveXエラー発生イベント
                        //
                        if (tmp_ErrNO != Module1.mcValue.pErrsts)
                        {
                            ese = e_ErrSet();
                        }
                    }


                    if (!TosTimerFlg) return;   //追加2014/06/09_hata

                    if (ErrSetDisp != null && ese != null)
                    {
                        ErrSetDisp(this, ese);
                    }

                    if (!TosTimerFlg) return;   //追加2014/06/09_hata
                    if (frmTosTimer == null) return;
                    if (!frmTosTimer.IsAlive) return;
                    if (frmTosTimer.Stoped) return;

                    frmTosTimer.Sleep(Interval);//追加2014/06/09_hata
                    //Thread.Sleep(Interval);
                    if (!TosTimerFlg) return;   //追加2014/06/09_hata
                        
                }
                //追加2014/06/09_hata
            }
			//catch(Exception ex)
			catch
            {
                //----- エラー処理 -----
                //無し
                //Debug.Print("frmTosTimer TryError ");
                //Debug.Print(ex.Source.ToString());
            }

        }

        
        //=================================================================
        //管電圧・管電流更新通知イベント発行
        //
        private XrayValueEventArgs e_XrayValue()
        {
            tmp_kVSet = Module1.mcValue.pcnd_Volt;
            tmp_XrayValue.m_kVSet = tmp_kVSet;
            tmp_mASet = Module1.mcValue.pcnd_Amp;
            tmp_XrayValue.m_mASet = tmp_mASet;

			XrayValueEventArgs e = new XrayValueEventArgs();

			e.XrayValue = tmp_XrayValue;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			'イベント発生
//			RaiseEvent XrayValueDisp(tmp_XrayValue)
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			return e;
        }

        //=================================================================
        //出力管電圧・出力管電流・メカ位置・拡大率更新通知イベント発行
        //
        private MechDataEventArgs e_MechData()
        {
            //2004-09-09 Shibui

            //出力管電圧
            tmp_Voltage = Module1.mcValue.pX_Volt;
            tmp_MechData.m_Voltage = tmp_Voltage;

            //出力管電流
            tmp_Curent = Module1.mcValue.pX_Amp;
            tmp_MechData.m_Curent = tmp_Curent;

            //X線ON/OFF
            tmp_XrayOnSet = Module1.mcValue.pX_On;
            tmp_MechData.m_XrayOnSet = tmp_XrayOnSet;

            //アベイラブル
            tmp_XAvail = Module1.mcValue.pXAvail;
            tmp_MechData.m_XAvail = tmp_XAvail;

            //2004-09-09 Shibui
            mudtXrayP.intTargetInf = Module1.mcValue.gpudtXrayStatus.intTargetInf;
            tmp_MechData.m_XrayTargetInf = mudtXrayP.intTargetInf;

            mudtXrayP.sngTargetInfSTG = Module1.mcValue.gpudtXrayStatus.sngTargetInfSTG;
            tmp_MechData.m_XrayTargetInfSTG = mudtXrayP.sngTargetInfSTG;

            mudtXrayP.intTargetLimit = Module1.mcValue.gpudtXrayStatus.intTargetLimit;
            tmp_MechData.m_XrayTargetLimit = mudtXrayP.intTargetLimit;

            mudtXrayP.intVacuumInf = Module1.mcValue.gpudtXrayStatus.intVacuumInf;
            tmp_MechData.m_XrayVacuumInf = mudtXrayP.intVacuumInf;

            mudtXrayP.strVacuumSVC = Module1.mcValue.gpudtXrayStatus.strVacuumSVC;
            tmp_MechData.m_XrayVacuumSVC = mudtXrayP.strVacuumSVC;

            MechDataEventArgs e = new MechDataEventArgs();
			e.MechData = tmp_MechData;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			'イベント発生
//			RaiseEvent MechDataDisp(tmp_MechData)
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			return e;
        }

        //=================================================================
        //装置状態通知イベント発行
        //
        private StatusValueEventArgs e_StatusValue()
        {
            tmp_XrayType = Module1.mcValue.pX_type;
            tmp_StatusValue.m_XrayType = tmp_XrayType;

            tmp_XrayMax_kV = Module1.mcValue.pXRMaxkV;
            tmp_StatusValue.m_XrayMax_kV = tmp_XrayMax_kV;

            tmp_XrayMax_mA = Module1.mcValue.pXRMaxmA;
            tmp_StatusValue.m_XrayMax_mA = tmp_XrayMax_mA;

            tmp_XrayMin_kV = Module1.mcValue.pXRMinkV;
            tmp_StatusValue.m_XrayMin_kV = tmp_XrayMin_kV;

            tmp_XrayMin_mA = Module1.mcValue.pXRMinmA;
            tmp_StatusValue.m_XrayMin_mA = tmp_XrayMin_mA;

            tmp_XrayWarmupSWS = Module1.mcValue.pXrayWarmupSWS;
            tmp_StatusValue.m_XrayWarmupSWS = tmp_XrayWarmupSWS;

            tmp_XrayStandby = Module1.mcValue.pStandby;
            tmp_StatusValue.m_XrayStandby = tmp_XrayStandby;

            tmp_XrayStatus = Module1.mcValue.pXStatus;
            tmp_StatusValue.m_XrayStatus = tmp_XrayStatus;

            tmp_InterLock = Module1.mcValue.pInterlock;
            tmp_StatusValue.m_InterLock = tmp_InterLock;

            tmp_WarmUp = Module1.mcValue.pWarmup;
            tmp_StatusValue.m_WarmUp = tmp_WarmUp;

            tmp_WarmMode = Module1.mcValue.pWarmup_Mode;
            tmp_StatusValue.m_WarmMode = tmp_WarmMode;

            tmp_XrayOffMode = Module1.mcValue.pXcont_Mode;
            tmp_StatusValue.m_XrayOffMode = tmp_XrayOffMode;

            tmp_XrayOffTime = Module1.mcValue.pLastOff;
            tmp_StatusValue.m_XrayOffTime = tmp_XrayOffTime;

//			tmp_IIPowerOn = mcValue.pPow
//			tmp_StatusValue.m_IIPowerOn = tmp_IIPowerOn

//2004-09-09
//			tmp_Fine = mcValue.pX_Fine
//			tmp_StatusValue.m_Fine = tmp_Fine

//			tmp_MechOrigin = mcValue.pOrigin
//			tmp_StatusValue.m_MechOrigin = tmp_MechOrigin
//
//			tmp_MechOrgMove = mcValue.pMechOrgMove
//			tmp_StatusValue.m_MechOrgMove = tmp_MechOrgMove
//
//			tmp_MechStatus = mcValue.pMech_Status
//			tmp_StatusValue.m_MechStatus = tmp_MechStatus

            //1999-10-07 T.Shibui
            tmp_PreHeat = Module1.mcValue.pXPermitWarmup;
            tmp_StatusValue.m_PreHeat = tmp_PreHeat;

//			//2000-04-21 T.Shibui
//			tmp_ShiMode = mcValue.pShiMode
//			tmp_StatusValue.m_ShiMode = tmp_ShiMode
//
//追加2010/02/22(KSS)hata_L8601対応 ----->
            tmp_XrayRemoteSRB = Module1.mcValue.pXrayRemoteSRB;
            tmp_StatusValue.m_XrayRemoteSRB = tmp_XrayRemoteSRB;

            tmp_XrayOperateSRL = Module1.mcValue.pXrayOperateSRL;
            tmp_StatusValue.m_XrayOperateSRL = tmp_XrayOperateSRL;
//追加2010/02/22(KSS)hata_L8601対応 -----<

//追加2010/05/14(KS1)hata_L9421-02対応 ----->
            tmp_XrayBatterySBT = Module1.mcValue.pXrayBatterySBT;
            tmp_StatusValue.m_XrayBatterySBT = tmp_XrayBatterySBT;
//追加2010/05/14(KS1)hata_L9421-02対応 -----<

	        StatusValueEventArgs e = new StatusValueEventArgs();
			e.StatusValue = tmp_StatusValue;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			'イベント発行
//			RaiseEvent StatusValueDisp(tmp_StatusValue)
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			return e;
        }

        //=================================================================
        //ユーザ設定通知イベント発行
        //
        private UserValueEventArgs e_UserValue()
        {
//			tmp_XYStep = mcValue.pStepXY
//			tmp_UserValue.m_XYStep = tmp_XYStep
//
//			tmp_IISpeed = mcValue.pII_Speed
//			tmp_UserValue.m_IISpeed = tmp_IISpeed
//
//			tmp_XtubeSpeed = mcValue.pXR_Speed
//			tmp_UserValue.m_XtubeSpeed = tmp_XtubeSpeed
//
//			tmp_IIXLock = mcValue.pMech_Lock
//			tmp_UserValue.m_IIXLock = tmp_IIXLock
//
            tmp_XrayOffModeU = Module1.mcValue.pXcont_Mode;
            tmp_UserValue.m_XrayOffMode = tmp_XrayOffModeU;

            tmp_XrayTime = Module1.mcValue.pXtimer;
            tmp_UserValue.m_XrayTime = tmp_XrayTime;

            tmp_XrayFocusSize = Module1.mcValue.pFocussize;
            tmp_UserValue.m_XrayFocusSize = tmp_XrayFocusSize;

//			tmp_XYTblSpeed = mcValue.pXYTblSpeed
//			tmp_UserValue.m_XYTblSpeed = tmp_XYTblSpeed

////2000-03-14 T.Shibui 8160FP対応 ->
//			//補正X軸移動速度
//			tmp_CaXSpeed = mcValue.pCaxSpeed
//			tmp_UserValue.m_CaXSpeed = tmp_CaXSpeed
//
//			//補正Y軸移動速度
//			tmp_CaYSpeed = mcValue.pCaySpeed
//			tmp_UserValue.m_CaYSpeed = tmp_CaYSpeed
//
//			//I.I.旋回速度
//			tmp_IIThetaSpeed = mcValue.pIIthetaSpeed
//			tmp_UserValue.m_IIThetaSpeed = tmp_IIThetaSpeed
//
//			//回転速度
//			tmp_ShiSpeed = mcValue.pShiSpeed
//			tmp_UserValue.m_ShiSpeed = tmp_ShiSpeed
//
//			//補正X/Y軸ユーザ送り移動時の移動量
//			tmp_CauXYStep = mcValue.pCauXYStep
//			tmp_UserValue.m_CauXYStep = tmp_CauXYStep
////<- 2000-03-14 T.Shibui

            UserValueEventArgs e = new UserValueEventArgs();

			e.UserValue = tmp_UserValue;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			'イベント発行
//			RaiseEvent UserValueDisp(tmp_UserValue)
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			return e;
        }

        //=================================================================
        //ウォームアップ残時間イベント発行
        //
        private WarmUpTimeEventArgs e_WarmUpTime()
        {
			tmp_WarmM = Module1.mcValue.pWrest_timeM;
			tmp_WarmUpTime.m_WarmM = tmp_WarmM;

			tmp_WarmS = Module1.mcValue.pWrest_timeS;
			tmp_WarmUpTime.m_WarmS = tmp_WarmS;

            WarmUpTimeEventArgs e = new WarmUpTimeEventArgs();

			e.WarmUpTime = tmp_WarmUpTime;

			return e;
        }

        //=================================================================
        //ActiveX制御エラー発生イベント発行
        //
        private ErrSetEventArgs e_ErrSet()
        {
			tmp_ErrNO = Module1.mcValue.pErrsts;
			tmp_ErrSet.m_ErrNO = tmp_ErrNO;

            //Debug.Print(String.Format("mErr {0}", tmp_ErrSet.m_ErrNO));

            ErrSetEventArgs e = new ErrSetEventArgs();

			e.ErrSet = tmp_ErrSet;

			return e;
        }

        private void TmpIni()
        {
            // 4-1 管電圧･管電流更新通知イベント
            tmp_kVSet = -1;             // 管電圧設定値
            tmp_mASet = -1;             // 管電流設定値

            // 4-2 出力管電圧･出力管電流･メカ位置･拡大率更新通知イベント
            tmp_Voltage = -1;           // 出力管電圧
            tmp_Curent = -1;            // 出力管電流
            tmp_XrayOnSet = -1;         // X線ON/OFF (0:OFF  1:ON)
            tmp_XAvail = -1;            // X線ｱﾍﾞｲﾗﾌﾞﾙ (0:範囲外  1:範囲内)
            
            // 4-4 装置状態通知イベント
            tmp_XrayType = -1;          // X線ﾀｲﾌﾟ       (0:90kV  1:130kV 2:160kV)
            tmp_XrayMax_kV = -1;        // 最大管電圧値
            tmp_XrayMax_mA = -1;        // 最大管電流値
            tmp_XrayMin_kV = -1;        // 最小管電圧値
            tmp_XrayMin_mA = -1;        // 最小管電流値
            tmp_XrayWarmupSWS = -1;     // ｳｫｰﾑｱｯﾌﾟｽﾃｯﾌﾟ
            tmp_XrayStandby = -1;       // X線ｽﾀﾝﾊﾞｲ     (0:NG  1:OK)
            tmp_XrayStatus = -1;        // X線正常       (1:正常  0:異常)
            tmp_InterLock = -1;         // ｲﾝﾀｰﾛｯｸ       (0:NG  1:OK)
            tmp_WarmUp = -1;            // ｳｫｰﾐﾝｸﾞｱｯﾌﾟ   (0:未完  1:中  2:完)
            tmp_WarmMode = -1;          // ｳｫｰﾑｱｯﾌﾟﾓｰﾄﾞ  (-1:不要  1:2D  2:2W  3:3W)
            tmp_XrayOffMode = -1;       // X線ﾀｲﾏﾓｰﾄﾞ    (0:ﾀｲﾏ  1:連続)
            tmp_XrayOffTime = "-1";       // 最新X線OFF日時(YYYY-MM-DD HH:MM)        //TODO string変換
            tmp_Fine = -1;              // ﾌｧｲﾝｾｯﾄ       (1:ﾌｧｲﾝ  0:ﾌｧｲﾝ外)
            tmp_PreHeat = -1;           // ﾌﾟﾘﾋｰﾄ       (0:未完  1:完)

            tmp_XrayOffModeU = -1;      // X線ﾀｲﾏﾓｰﾄﾞ     (1:ﾀｲﾏﾓｰﾄﾞ  0:連続)
            tmp_XrayTime = -1;          // X線ﾀｲﾏ設定時間
            tmp_XrayFocusSize = -1;     // X線焦点自動(1:小  2:大  3:設定なし  )
            tmp_XrayOperateSRL = -1;    // ｵﾍﾟﾚｰﾄｽｲｯﾁ状態確認(0：OFF、1:REMOTE、2:LOCAL) //追加2009/08/31(KSS)hata
            tmp_XrayRemoteSRB = -1;     // ﾘﾓｰﾄ動作状態確認(0:BUSY、1:READY)  //追加2009/08/31(KSS)hata
            tmp_XrayBatterySBT = -1;    // ﾊﾞｯﾃﾘｰﾄ状態確認(0:正常、1:Low)  //追加2010/05/14(KS1)hata

            // 4-8 ActiveX制御エラー発生イベント
            tmp_ErrNO = -1;         // ｴﾗｰNO
    
            //2004-09-09 Shibui
            mudtXrayP.intSAD = -1;
            mudtXrayP.intSAT = -1;
            mudtXrayP.intSBX = -1;
            mudtXrayP.intSBY = -1;
            mudtXrayP.intSER = -1;
            mudtXrayP.intSHM = -1;
            mudtXrayP.sngSHS = -1;
            //変更2009/08/31(KSS)hata L10801 ---------->
            //    .intSHT = -1
            mudtXrayP.lngSHT = -1;
            //変更2009/08/31(KSS)hata L10801 ----------<
            mudtXrayP.intSOV = -1;
            mudtXrayP.intTargetInf = -1;
            mudtXrayP.intTargetLimit = -1;
            mudtXrayP.intVacuumInf = -1;
            //    .intVacuumInf = -1
            mudtXrayP.lngSTM = -1;
            mudtXrayP.lngSXT = -1;
            mudtXrayP.sngSOB = -1;
            mudtXrayP.sngSVV = -1;
            mudtXrayP.sngTargetInfSTG = -1;
            mudtXrayP.strVacuumSVC = "-1";
            //追加2009/08/31(KSS)hata L10801 ---------->
            mudtXrayP.intFLM = -1;
            mudtXrayP.intSSA = -1;
            mudtXrayP.strTYP = "";
            mudtXrayP.intZT1 = -1;
            mudtXrayP.intSWE = -1;
            mudtXrayP.strSWU = "";
            mudtXrayP.intSMV = -1;    //v15.10追加 byやまおか 2009/11/12
            //追加2009/08/31(KSS)hata L10801 ----------<

            //Rev23.10 L10711 対応 by長野 2015/10/05 ------------>
            mudtXrayP.intSMD = -1;
            mudtXrayP.intSCX = -1;
            mudtXrayP.intSCY = -1;
            mudtXrayP.sngCOB = -1;
            mudtXrayP.sngSVS = -1;
            //Rev23.10 L10711 対応 by長野 2015/10/05 ------------<

        }

        //2004-09-09 Shibui
        //***********************************************************
        //   L9191用イベント発行
        //***********************************************************
        private UdtXrayStatus3EventArgs e_XrayStatus3ValueDisp()
        {
            mudtXrayP = Module1.mcValue.gpudtXrayStatus;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			With mudtXrayP
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				mudtXrayStatus3ValueDisp.m_XrayStatusSBX = mudtXrayP.intSBX;
				mudtXrayStatus3ValueDisp.m_XrayStatusSBY = mudtXrayP.intSBY;
				mudtXrayStatus3ValueDisp.m_XrayStatusSAD = mudtXrayP.intSAD;
				mudtXrayStatus3ValueDisp.m_XrayStatusSOB = mudtXrayP.sngSOB;
				mudtXrayStatus3ValueDisp.m_XrayStatusSVV = mudtXrayP.sngSVV;
				mudtXrayStatus3ValueDisp.m_XrayStatusSTM = mudtXrayP.lngSTM;
				mudtXrayStatus3ValueDisp.m_XrayStatusSXT = mudtXrayP.lngSXT;
				mudtXrayStatus3ValueDisp.m_XrayStatusSHM = mudtXrayP.intSHM;
				mudtXrayStatus3ValueDisp.m_XrayStatusSHS = mudtXrayP.sngSHS;
//変更2009/08/31(KSS)hata L10801 ---------->
//				mudtXrayStatus3ValueDisp.m_XrayStatusSHT = .intSHT
	            mudtXrayStatus3ValueDisp.m_XrayStatusSHT = mudtXrayP.lngSHT;
//変更2009/08/31(KSS)hata L10801 ----------<
				mudtXrayStatus3ValueDisp.m_XrayStatusSAT = mudtXrayP.intSAT;
				mudtXrayStatus3ValueDisp.m_XrayStatusSOV = mudtXrayP.intSOV;
				mudtXrayStatus3ValueDisp.m_XrayStatusSER = mudtXrayP.intSER;
//追加2009/10/08(KSS)hata L10801 ---------->
				mudtXrayStatus3ValueDisp.m_XrayStatusSWE = mudtXrayP.intSWE;
				mudtXrayStatus3ValueDisp.m_XrayStatusSWU = mudtXrayP.strSWU;
				mudtXrayStatus3ValueDisp.m_XrayStatusSMV = mudtXrayP.intSMV;    //v15.10追加 byやまおか 2009/11/12
//追加2009/10/08(KSS)hata L10801 ----------<

                //Rev23.10 L10711 対応 by長野 2015/10/05 --------------------->
                mudtXrayStatus3ValueDisp.m_XrayStatusCAX = mudtXrayP.intSCX;
                mudtXrayStatus3ValueDisp.m_XrayStatusCAY = mudtXrayP.intSCY;
                mudtXrayStatus3ValueDisp.m_XrayStatusSMD = mudtXrayP.intSMD;
                mudtXrayStatus3ValueDisp.m_XrayStatusCOB = mudtXrayP.sngCOB;
                mudtXrayStatus3ValueDisp.m_XrayStatusSVS = mudtXrayP.sngSVS;
                //Rev23.10 L10711 対応 by長野 2015/10/05 ---------------------<


#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				End With

//				RaiseEvent XrayStatus3ValueDisp(mudtXrayStatus3ValueDisp)
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            //Debug.Print "SBX= "; mudtXrayStatus3ValueDisp.m_XrayStatusSBX
            //Debug.Print "SBY= "; mudtXrayStatus3ValueDisp.m_XrayStatusSBY
            //Debug.Print "SAD= "; mudtXrayStatus3ValueDisp.m_XrayStatusSAD
            //Debug.Print "SOB= "; mudtXrayStatus3ValueDisp.m_XrayStatusSOB

			UdtXrayStatus3EventArgs e = new UdtXrayStatus3EventArgs();

			e.udtXrayStatus3ValueDisp = mudtXrayStatus3ValueDisp;

			return e;
        }
    }
}
