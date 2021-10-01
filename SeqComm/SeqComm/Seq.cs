using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Timers;
using System.Threading;

namespace SeqComm
{
    [System.Runtime.InteropServices.ProgId("Seq_NET.Seq")]
    public class Seq : IDisposable
    {
        #region 公開しているイベント
        public event OnCommEndEventHandler OnCommEnd;
        public delegate void OnCommEndEventHandler(int CommEndAns);
        #endregion

        #region 内部で使用するコンポーネント
        /// <summary>
        /// シリアルポート
        /// </summary>
        private SerialPort mySerialPort;

        /// <summary>
        /// タイマー
        /// </summary>
        private System.Timers.Timer myTimer;

        //送信コマンド（Log用）        
        //private string strSendCommamd = null; //変更2015/03/04_hata 
        private string strSendCommamd = "";

        //追加2014/06/09_hata
        private bool CommEndFlg = false;	        // 通信停止要求用
        private ManualResetEvent SerialRead = new ManualResetEvent(false);        //通信read時の待ち認識用

        //追加2015/02/10hata
        private bool CommLineError = false;	    // 通信ラインのエラー有無
        private bool CommDataError = false;	    // 通信データのエラー有無
        //private bool CommTimeOutError = false;	// 通信のタイムアウトエラー有無       

        //追加2015/03/10hata
        private DateTime RecvOnTime;
        private int RecvPoint = 0;  //追加2015/03/14hata
        private bool bDatAll = false;    //追加2015/03/14hata
        #endregion

        #region 定数
        //DebuTest3/16hata
        /// <summary>
        /// 通信ﾀｲﾑｱｳﾄ時間（秒）'変更 by 稲葉 05-02-08
        /// </summary>
        //private const float CommTimeOut = 5;  //変更2015/03/13hata 5→10
        private const float CommTimeOut = 10;


#if conKvMode1 ||  conKvMode2       //ｼｰｹﾝｻKV
        /// <summary>
        /// 読み出しﾋﾞｯﾄ数 -1
        /// </summary>
	    private const int BitReadNo = 191;
        
        /// <summary>
        /// 読み出しﾜｰﾄﾞ数 -1
        /// </summary>
        private const int WordReadNo = 63;
#else                               //ｼｰｹﾝｻKZ
        /// <summary>
        /// 読み出しﾋﾞｯﾄ数 -1
        /// </summary>
        private const int BitReadNo = 127;

        /// <summary>
        /// 読み出しﾜｰﾄﾞ数 -1
        /// </summary>
        private const int WordReadNo = 31;
#endif

        /// <summary>
        /// バッファサイズ
        /// </summary>
        //private const int BufferSize = 100;
        private const int BufferSize = 1024;

        /// <summary>
        /// タイマーの周期
        /// </summary>
        private const int Interval = 500;
        private const int SendInterval = 1;
        
        #endregion

        #region フィールド
        /// <summary>
        /// 排他用オブジェクト
        /// </summary>
        internal static object gLock = new object();

        /// <summary>
        /// タイマー排他用オブジェクト
        /// </summary>
        internal static object tLock = new object();

        /// <summary>
        /// 受信ﾃﾞｰﾀﾊﾞｯﾌｧ
        /// </summary>
	    string rData;
	    
        /// <summary>
        /// 通信ﾀｲﾑｱｳﾄ時間             '変更 2011/03/02 by 間々田
        /// </summary>
        DateTime Duration;
	    
        /// <summary>
        /// ﾋﾞｯﾄ/ﾜｰﾄﾞｽﾃｰﾀｽ読込ﾌﾗｸﾞ
        /// 1:ﾋﾞｯﾄ読み出し 2:ﾜｰﾄﾞ読み出し
        /// </summary>
        int ReadFlag;
        		
	    /// <summary>
        /// 読込ﾒｿｯﾄﾞNo
	    /// </summary>
        int StatusReadNo;
		
	    /// <summary>
        /// ﾋﾞｯﾄ書込みﾒｿｯﾄﾞNo
	    /// </summary>
        int BitWriteNo;
		
	    /// <summary>
        /// ﾜｰﾄﾞ書込みﾒｿｯﾄﾞNo
	    /// </summary>
        int WordWriteNo;
	    
        /// <summary>
        /// ﾋﾞｯﾄﾜｰﾄﾞ書込みﾒｿｯﾄﾞNo 追加 by 稲葉 02-09-19
        /// </summary>
        int BitWordWriteNo;
	    
        /// <summary>
        /// ﾋﾞｯﾄ書込みﾃﾞﾊﾞｲｽﾈｰﾑ
        /// </summary>
        string[] BitWriteName;
	    
        /// <summary>
        /// ﾋﾞｯﾄ書込みﾃﾞｰﾀ
        /// </summary>
        bool[] BitWriteData;
	    
        /// <summary>
        /// ﾜｰﾄﾞ書込みﾃﾞﾊﾞｲｽﾈｰﾑ
        /// </summary>
        string[] WordWriteName;

        /// <summary>
        /// ﾜｰﾄﾞ書込みﾃﾞｰﾀ
        /// </summary>
	    string[] WordWriteData;
		
	    /// <summary>
        /// 書込みﾀｲﾌﾟ(ﾋﾞｯﾄ/ﾜｰﾄﾞ) 追加 by 稲葉 02-09-19
	    /// </summary>
        string[] BitWordType;
	        	    
        /// <summary>
        /// 昇降手動運転速度現在値
        /// </summary>
        int OldUDSpeed;
	    
        /// <summary>
        /// 回転手動運転速度現在値
        /// </summary>
        int OldRotSpeed;
	    
        /// <summary>
        /// 微調X手動運転速度現在値
        /// </summary>
        int OldXStgSpeed;
	    
        /// <summary>
        /// 微調Y手動運転速度現在値
        /// </summary>
        int OldYStgSpeed;

        /// <summary>
        /// 傾斜X手動運転速度現在値    '追加 by 稲葉 15-07-24
        /// </summary>
        int OldTiltSpeed;

        /// <summary>
        /// 傾斜回転Y手動運転速度現在値    '追加 by 稲葉 15-07-24
        /// </summary>
        int OldTiltRotSpeed;

        /// <summary>
        /// OnCommEndｲﾍﾞﾝﾄ
        /// </summary>
	    int SeqCommEvent;

        /// <summary>
        /// 通信中
        /// </summary>
        private bool _stsCommBusy;

        /// <summary>
        /// ﾊﾟｿｺﾝ操作
        /// </summary>
        private bool _PcInhibit;

        /// <summary>
        /// 運転準備ｽｲｯﾁ  '追加 by 稲葉 05-11-24
        /// </summary>
        private bool _stsRunReadySW;

        /// <summary>
        /// 扉ｲﾝﾀｰﾛｯｸ
        /// </summary>
        private bool _stsDoorInterlock;

        /// <summary>
        /// 非常停止
        /// </summary>
        private bool _stsEmergency;

        /// <summary>
        /// X線225KVﾄﾘｯﾌﾟ
        /// </summary>
        private bool _stsXray225Trip;

        /// <summary>
        /// X線160KVﾄﾘｯﾌﾟ
        /// </summary>
        private bool _stsXray160Trip;

        /// <summary>
        /// ﾌｨﾙﾀﾕﾆｯﾄ接触
        /// </summary>
        private bool _stsFilterTouch;

        /// <summary>
        /// X線225KV接触
        /// </summary>
        private bool _stsXray225Touch;

        /// <summary>
        /// X線160KV接触
        /// </summary>
        private bool _stsXray160Touch;

        /// <summary>
        /// 回転ﾃｰﾌﾞﾙ接触
        /// </summary>
        private bool _stsRotTouch;

        /// <summary>
        /// 傾斜ﾃｰﾌﾞﾙ接触
        /// </summary>
        private bool _stsTiltTouch;

        /// <summary>
        /// ﾃｰﾌﾞﾙX軸ｵｰﾊﾞｰﾋｰﾄ
        /// </summary>
        private bool _stsXDriverHeat;

        /// <summary>
        /// ﾃｰﾌﾞﾙY軸ｵｰﾊﾞｰﾋｰﾄ
        /// </summary>
        private bool _stsYDriverHeat;

        /// <summary>
        /// X線管切替ｵｰﾊﾞｰﾋｰﾄ
        /// </summary>
        private bool _stsXrayDriverHeat;

        /// <summary>
        /// ｼｰｹﾝｻCPUｴﾗｰ
        /// </summary>
        private bool _stsSeqCpuErr;

        /// <summary>
        /// ｼｰｹﾝｻﾊﾞｯﾃﾘｰｴﾗｰ
        /// </summary>
        private bool _stsSeqBatteryErr;

        /// <summary>
        /// ｼｰｹﾝｻKL通信ｴﾗｰ(KZ)
        /// </summary>
        private bool _stsSeqKzCommErr;

        /// <summary>
        /// ｼｰｹﾝｻKL通信ｴﾗｰ(KV)
        /// </summary>
        private bool _stsSeqKvCommErr;

        /// <summary>
        /// ﾌｨﾙﾀｰﾀｲﾑｱｳﾄ
        /// </summary>
        private bool _stsFilterTimeout;

        /// <summary>
        /// 傾斜ﾀｲﾑｱｳﾄ
        /// </summary>
        private bool _stsTiltTimeout;

        /// <summary>
        /// ﾃｰﾌﾞﾙX軸ﾀｲﾑｱｳﾄ
        /// </summary>
        private bool _stsXTimeout;

        /// <summary>
        /// I.I.軸ｵｰﾊﾞｰﾋｰﾄ
        /// </summary>
        private bool _stsIIDriverHeat;

        /// <summary>
        /// ｼｰｹﾝｻｶｳﾝﾀﾕﾆｯﾄｴﾗｰ
        /// </summary>
        private bool _stsSeqCounterErr;

        /// <summary>
        /// X軸動作ｴﾗｰ
        /// </summary>
        private bool _stsXDriveErr;

        /// <summary>
        /// ﾃｰﾌﾞﾙX左限
        /// </summary>
        private bool _stsXLLimit;

        /// <summary>
        /// ﾃｰﾌﾞﾙX右限
        /// </summary>
        private bool _stsXRLimit;

        /// <summary>
        /// ﾃｰﾌﾞﾙX左移動中
        /// </summary>
        private bool _stsXLeft;

        /// <summary>
        /// ﾃｰﾌﾞﾙX右移動中
        /// </summary>
        private bool _stsXRight;

        /// <summary>
        /// ﾃｰﾌﾞﾙX左移動中接触
        /// </summary>
        private bool _stsXLTouch;

        /// <summary>
        /// ﾃｰﾌﾞﾙX右移動中接触
        /// </summary>
        private bool _stsXRTouch;

        /// <summary>
        /// ﾃｰﾌﾞﾙY前進限
        /// </summary>
        private bool _stsYFLimit;

        /// <summary>
        /// ﾃｰﾌﾞﾙY後退限
        /// </summary>
        private bool _stsYBLimit;

        /// <summary>
        /// ﾃｰﾌﾞﾙY前進中
        /// </summary>
        private bool _stsYForward;

        /// <summary>
        /// ﾃｰﾌﾞﾙY後退中
        /// </summary>
        private bool _stsYBackward;

        /// <summary>
        /// ﾃｰﾌﾞﾙY前進中接触
        /// </summary>
        private bool _stsYFTouch;

        /// <summary>
        /// ﾃｰﾌﾞﾙY後退中接触
        /// </summary>
        private bool _stsYBTouch;

        /// <summary>
        /// I.I.前進限
        /// </summary>
        private bool _stsIIFLimit;

        /// <summary>
        /// I.I.後退限
        /// </summary>
        private bool _stsIIBLimit;

        /// <summary>
        /// I.I.前進中
        /// </summary>
        private bool _stsIIForward;

        /// <summary>
        /// I.I.後退中
        /// </summary>
        private bool _stsIIBackward;

        /// <summary>
        /// 傾斜CW限
        /// </summary>
        private bool _stsTiltCwLimit;

        /// <summary>
        /// 傾斜CCW限
        /// </summary>
        private bool _stsTiltCCwLimit;

        /// <summary>
        /// 傾斜原点（水平）
        /// </summary>
        private bool _stsTiltOrigin;

        /// <summary>
        /// 傾斜CW中
        /// </summary>
        private bool _stsTiltCw;

        /// <summary>
        /// 傾斜CCW中
        /// </summary>
        private bool _stsTiltCCw;

        /// <summary>
        /// 傾斜原点復帰中
        /// </summary>
        private bool _stsTiltOriginRun;

        /// <summary>
        /// ｺﾘﾒｰﾀ左開限
        /// </summary>
        private bool _stsColliLOLimit;

        /// <summary>
        /// ｺﾘﾒｰﾀ左閉限
        /// </summary>
        private bool _stsColliLCLimit;

        /// <summary>
        /// ｺﾘﾒｰﾀ右開限
        /// </summary>
        private bool _stsColliROLimit;

        /// <summary>
        /// ｺﾘﾒｰﾀ右閉限
        /// </summary>
        private bool _stsColliRCLimit;

        /// <summary>
        /// ｺﾘﾒｰﾀ上開限
        /// </summary>
        private bool _stsColliUOLimit;

        /// <summary>
        /// ｺﾘﾒｰﾀ上閉限
        /// </summary>
        private bool _stsColliUCLimit;

        /// <summary>
        /// ｺﾘﾒｰﾀ下開限
        /// </summary>
        private bool _stsColliDOLimit;

        /// <summary>
        /// ｺﾘﾒｰﾀ下閉限
        /// </summary>
        private bool _stsColliDCLimit;

        /// <summary>
        /// ｺﾘﾒｰﾀ左開中
        /// </summary>
        private bool _stsColliLOpen;

        /// <summary>
        /// ｺﾘﾒｰﾀ左閉中
        /// </summary>
        private bool _stsColliLClose;

        /// <summary>
        /// ｺﾘﾒｰﾀ右開中
        /// </summary>
        private bool _stsColliROpen;

        /// <summary>
        /// ｺﾘﾒｰﾀ右閉中
        /// </summary>
        private bool _stsColliRClose;

        /// <summary>
        /// ｺﾘﾒｰﾀ上開中
        /// </summary>
        private bool _stsColliUOpen;

        /// <summary>
        /// ｺﾘﾒｰﾀ上閉中
        /// </summary>
        private bool _stsColliUClose;

        /// <summary>
        /// ｺﾘﾒｰﾀ下開中
        /// </summary>
        private bool _stsColliDOpen;

        /// <summary>
        /// ｺﾘﾒｰﾀ下閉中
        /// </summary>
        private bool _stsColliDClose;

        /// <summary>
        /// ﾌｨﾙﾀ無し位置
        /// </summary>
        private bool _stsFilter0;

        /// <summary>
        /// ﾌｨﾙﾀ1位置
        /// </summary>
        private bool _stsFilter1;

        /// <summary>
        /// ﾌｨﾙﾀ2位置
        /// </summary>
        private bool _stsFilter2;

        /// <summary>
        /// ﾌｨﾙﾀ3位置
        /// </summary>
        private bool _stsFilter3;

        /// <summary>
        /// ﾌｨﾙﾀ4位置
        /// </summary>
        private bool _stsFilter4;

        /// <summary>
        /// ﾌｨﾙﾀ5位置
        /// </summary>
        private bool _stsFilter5;

        /// <summary>
        /// ﾌｨﾙﾀ無し動作中
        /// </summary>
        private bool _stsFilter0Run;

        /// <summary>
        /// ﾌｨﾙﾀ1動作中
        /// </summary>
        private bool _stsFilter1Run;

        /// <summary>
        /// ﾌｨﾙﾀ2動作中
        /// </summary>
        private bool _stsFilter2Run;

        /// <summary>
        /// ﾌｨﾙﾀ3動作中
        /// </summary>
        private bool _stsFilter3Run;

        /// <summary>
        /// ﾌｨﾙﾀ4動作中
        /// </summary>
        private bool _stsFilter4Run;

        /// <summary>
        /// ﾌｨﾙﾀ5動作中
        /// </summary>
        private bool _stsFilter5Run;

        /// <summary>
        /// X線ON
        /// </summary>
        private bool _XrayOn;

        /// <summary>
        /// X線OFF
        /// </summary>
        private bool _XrayOff;

        /// <summary>
        /// ｽﾃｰｼﾞX左移動
        /// </summary>
        private bool _XStgLeft;

        /// <summary>
        /// ｽﾃｰｼﾞX右移動
        /// </summary>
        private bool _XStgRight;

        /// <summary>
        /// ｽﾃｰｼﾞX原点復帰
        /// </summary>
        private bool _XStgOrigin;

        /// <summary>
        /// ｽﾃｰｼﾞY前進
        /// </summary>
        private bool _YStgForward;

        /// <summary>
        /// ｽﾃｰｼﾞY後退
        /// </summary>
        private bool _YStgBackward;

        /// <summary>
        /// ｽﾃｰｼﾞY原点復帰
        /// </summary>
        private bool _YStgOrigin;

        /// <summary>
        /// 回転位置決め要求
        /// </summary>
        private bool _RotIndex;

        /// <summary>
        /// I.I.視野9”
        /// </summary>
        private bool _stsII9;

        /// <summary>
        /// I.I.視野6”
        /// </summary>
        private bool _stsII6;

        /// <summary>
        /// I.I.視野4.5”
        /// </summary>
        private bool _stsII4;

        /// <summary>
        /// I.I.電源
        /// </summary>
        private bool _stsIIPower;

        /// <summary>
        /// ｽﾗｲｽﾗｲﾄ
        /// </summary>
        private bool _stsSLight;

        /// <summary>
        /// ﾃｰﾌﾞﾙｲﾝﾀｰﾛｯｸ解除
        /// </summary>
        private bool _stsTableMovePermit;

        /// <summary>
        /// ﾃｰﾌﾞﾙ回転、上昇、微調動作禁止
        /// </summary>
        private bool _stsMechaPermit;

        /// <summary>
        /// 昇降位置決め要求
        /// </summary>
        private bool _UpDownIndex;

        /// <summary>
        /// ｽﾃｰｼﾞX位置決め要求
        /// </summary>
        private bool _XStageIndex;

        /// <summary>
        /// ｽﾃｰｼﾞY位置決め要求
        /// </summary>
        private bool _YStageIndex;

        /// <summary>
        /// ﾒｶﾃﾞﾊﾞｲｽ　ｴﾗｰﾘｾｯﾄ
        /// </summary>
        private bool _DeviceErrReset;

        /// <summary>
        /// 昇降原点復帰
        /// </summary>
        private bool _UdOrigin;

        /// <summary>
        /// 回転原点復帰
        /// </summary>
        private bool _RotOrigin;

        /// <summary>
        /// 回転中心校正ﾃｰﾌﾞﾙX移動有り
        /// </summary>
        private bool _stsRotXChange;

        /// <summary>
        /// 寸法校正ﾃｰﾌﾞﾙX移動有り
        /// </summary>
        private bool _stsDisXChange;

        /// <summary>
        /// 回転中心校正ﾃｰﾌﾞﾙY移動有り
        /// </summary>
        private bool _stsRotYChange;

        /// <summary>
        /// 寸法校正ﾃｰﾌﾞﾙY移動有り
        /// </summary>
        private bool _stsDisYChange;

        /// <summary>
        /// 幾何歪校正I.I.移動有り
        /// </summary>
        private bool _stsVerIIChange;

        /// <summary>
        /// 回転中心校正I.I.移動有り
        /// </summary>
        private bool _stsRotIIChange;

        /// <summary>
        /// ｹﾞｲﾝ校正I.I.移動有り
        /// </summary>
        private bool _stsGainIIChange;

        /// <summary>
        /// 寸法校正I.I.移動有り
        /// </summary>
        private bool _stsDisIIChange;

        /// <summary>
        /// ｽｷｬﾝ位置校正I.I.移動有り '追加 by 稲葉 05-10-21
        /// </summary>
        private bool _stsSPIIChange;

        /// <summary>
        /// X線管X軸ｴﾗｰ
        /// </summary>
        private bool _stsXrayXErr;

        /// <summary>
        /// X線管Y軸ｴﾗｰ
        /// </summary>
        private bool _stsXrayYErr;

        /// <summary>
        /// X線管回転ｴﾗｰ
        /// </summary>
        private bool _stsXrayRotErr;

        /// <summary>
        /// X線管X軸左限
        /// </summary>
        private bool _stsXrayXLLimit;

        /// <summary>
        /// X線管X軸右限
        /// </summary>
        private bool _stsXrayXRLimit;

        /// <summary>
        /// X線管X軸左移動中
        /// </summary>
        private bool _stsXrayXL;

        /// <summary>
        /// X線管X軸右移動中
        /// </summary>
        private bool _stsXrayXR;

        /// <summary>
        /// 回転中心校正X線管X軸移動有り
        /// </summary>
        private bool _stsRotXrayXCh;

        /// <summary>
        /// 寸法校正X線管X軸移動有り
        /// </summary>
        private bool _stsDisXrayXCh;

        /// <summary>
        /// X線管Y軸前進限
        /// </summary>
        private bool _stsXrayYFLimit;

        /// <summary>
        /// X線管Y軸後退限
        /// </summary>
        private bool _stsXrayYBLimit;

        /// <summary>
        /// X線管Y軸前進移動中
        /// </summary>
        private bool _stsXrayYF;

        /// <summary>
        /// X線管Y軸後退移動中
        /// </summary>
        private bool _stsXrayYB;

        /// <summary>
        /// 回転中心校正X線管Y軸移動有り
        /// </summary>
        private bool _stsRotXrayYCh;

        /// <summary>
        /// 寸法校正X線管Y軸移動有り
        /// </summary>
        private bool _stsDisXrayYCh;

        /// <summary>
        /// X線管正転限
        /// </summary>
        private bool _stsXrayCWLimit;

        /// <summary>
        /// X線管逆転限
        /// </summary>
        private bool _stsXrayCCWLimit;

        /// <summary>
        /// X線管正転中
        /// </summary>
        private bool _stsXrayCW;

        /// <summary>
        /// X線管逆転中
        /// </summary>
        private bool _stsXrayCCW;

        /// <summary>
        /// X線管回転動作不可
        /// </summary>
        private bool _stsXrayRotLock;

        /// <summary>
        /// 試料扉電磁ﾛｯｸｷｰ挿入完了
        /// </summary>
        private bool _stsDoorKey;

        /// <summary>
        /// 試料扉電磁ﾛｯｸON中
        /// </summary>
        private bool _stsDoorLock;

        /// <summary>
        /// X線EXM ON中
        /// </summary>
        private bool _stsEXMOn;

        /// <summary>
        /// X線EXM ON可能
        /// </summary>
        private bool _stsEXMReady;

        /// <summary>
        /// X線EXM発生装置正常
        /// </summary>
        private bool _stsEXMNormal1;

        /// <summary>
        /// X線EXMﾃﾞｰﾀ書込正常
        /// </summary>
        private bool _stsEXMNormal2;

        /// <summary>
        /// X線EXMｳｫｰﾑｱｯﾌﾟ必要又は実行中
        /// </summary>
        private bool _stsEXMWU;

        /// <summary>
        /// X線EXMﾘﾓｰﾄ中
        /// </summary>
        private bool _stsEXMRemote;

        /// <summary>
        /// CT用I.I.撮影位置
        /// </summary>
        private bool _stsCTIIPos;

        /// <summary>
        /// 高速用I.I.撮影位置
        /// </summary>
        private bool _stsTVIIPos;

        /// <summary>
        /// CT用I.I.切替中
        /// </summary>
        private bool _stsCTIIDrive;

        /// <summary>
        /// 高速用I.I.切替中
        /// </summary>
        private bool _stsTVIIDrive;

        /// <summary>
        /// 高速用I.I.視野9”
        /// </summary>
        private bool _stsTVII9;

        /// <summary>
        /// 高速用I.I.視野6”
        /// </summary>
        private bool _stsTVII6;

        /// <summary>
        /// 高速用I.I.視野4.5”
        /// </summary>
        private bool _stsTVII4;

        /// <summary>
        /// 高速用I.I.電源
        /// </summary>
        private bool _stsTVIIPower;

        /// <summary>
        /// 高速用I.I.電源
        /// </summary>
        private bool _stsCameraPower;

        /// <summary>
        /// I.I.絞り左開中
        /// </summary>
        private bool _stsIrisLOpen;

        /// <summary>
        /// I.I.絞り左閉中
        /// </summary>
        private bool _stsIrisLClose;

        /// <summary>
        /// I.I.絞り右開中
        /// </summary>
        private bool _stsIrisROpen;

        /// <summary>
        /// I.I.絞り右閉中
        /// </summary>
        private bool _stsIrisRClose;

        /// <summary>
        /// I.I.絞り上開中
        /// </summary>
        private bool _stsIrisUOpen;

        /// <summary>
        /// I.I.絞り上閉中
        /// </summary>
        private bool _stsIrisUClose;

        /// <summary>
        /// I.I.絞り下開中
        /// </summary>
        private bool _stsIrisDOpen;

        /// <summary>
        /// I.I.絞り下閉中
        /// </summary>
        private bool _stsIrisDClose;

        /// <summary>
        /// 動作制限自動復帰設定状態   '追加 by 稲葉 10-10-19
        /// </summary>
        private bool _stsAutoRestrict;

        /// <summary>
        /// Y軸ｲﾝﾃﾞｯｸｽ減速設定状態     '追加 by 稲葉 10-10-19
        /// </summary>
        private bool _stsYIndexSlow;

        /// <summary>
        /// 動作用扉ｲﾝﾀｰﾛｯｸ設定状態    '追加 by 稲葉 10-10-19
        /// </summary>
        private bool _stsDoorPermit;

        /// <summary>
        /// ｼｬｯﾀ位置    '追加 by 稲葉 10-11-22
        /// </summary>
        private bool _stsShutter;

        /// <summary>
        /// ｼｬｯﾀ動作中  '追加 by 稲葉 10-11-22
        /// </summary>
        private bool _stsShutterBusy;

        /// <summary>
        /// ﾃｰﾌﾞﾙ左右原点復帰要求
        /// </summary>
        private bool _stsXOrgReq;

        /// <summary>
        /// ﾃｰﾌﾞﾙ前後原点復帰要求
        /// </summary>
        private bool _stsYOrgReq;

        /// <summary>
        /// 検出器前後原点復帰要求
        /// </summary>
        private bool _stsIIOrgReq;

        /// <summary>
        /// 検出器切替原点復帰要求
        /// </summary>
        private bool _stsIIChgOrgReq;

        /// <summary>
        /// ﾒｶﾘｾｯﾄ中                  '追加 by 稲葉 10-09-10
        /// </summary>
        private bool _stsMechaRstBusy;

        /// <summary>
        /// ﾒｶﾘｾｯﾄ完了                '追加 by 稲葉 10-09-10
        /// </summary>
        private bool _stsMechaRstOK;

        /// <summary>
        /// FID
        /// </summary>
        private int _stsFID;

        /// <summary>
        /// FCD
        /// </summary>
        private int _stsFCD;

        /// <summary>
        /// ﾃｰﾌﾞﾙX最低速度
        /// </summary>
        private int _stsXMinSpeed;

        /// <summary>
        /// ﾃｰﾌﾞﾙX最高速度
        /// </summary>
        private int _stsXMaxSpeed;

        /// <summary>
        /// ﾃｰﾌﾞﾙX運転速度
        /// </summary>
        private int _stsXSpeed;

        /// <summary>
        /// ﾃｰﾌﾞﾙY最低速度
        /// </summary>
        private int _stsYMinSpeed;

        /// <summary>
        /// ﾃｰﾌﾞﾙY最高速度
        /// </summary>
        private int _stsYMaxSpeed;

        /// <summary>
        /// ﾃｰﾌﾞﾙY運転速度
        /// </summary>
        private int _stsYSpeed;

        /// <summary>
        /// FCD減速速度 追加 by 稲葉 18-01-15
        /// </summary>
        private int _stsFcdDecelerationSpeed;

        /// <summary>
        /// ﾃｰﾌﾞﾙX現在値
        /// </summary>
        private int _stsXPosition;

        /// <summary>
        /// I.I.最低速度
        /// </summary>
        private int _stsIIMinSpeed;

        /// <summary>
        /// I.I.最高速度
        /// </summary>
        private int _stsIIMaxSpeed;

        /// <summary>
        /// I.I.運転速度
        /// </summary>
        private int _stsIISpeed;

        /// <summary>
        /// 昇降手動運転速度
        /// </summary>
        private int _stsUDSpeed;

        /// <summary>
        /// 昇降位置決め位置
        /// </summary>
        private int _stsUDIndexPos;

        /// <summary>
        /// 回転手動運転速度
        /// </summary>
        private int _stsRotSpeed;

        /// <summary>
        /// 回転位置決め位置
        /// </summary>
        private int _stsRotIndexPos;

        /// <summary>
        /// 微調X手動運転速度
        /// </summary>
        private int _stsXStgSpeed;

        /// <summary>
        /// 微調X位置決め位置
        /// </summary>
        private int _stsXStgIndexPos;

        /// <summary>
        /// 微調Y手動運転速度
        /// </summary>
        private int _stsYStgSpeed;

        /// <summary>
        /// 微調Y位置決め位置
        /// </summary>
        private int _stsYStgIndexPos;

        /// <summary>
        /// ﾃｰﾌﾞﾙY軸現在位置
        /// </summary>
        private int _stsYPosition;

        /// <summary>
        /// X線管FCD
        /// </summary>
        private int _stsXrayFCD;

        /// <summary>
        /// X線管X軸最低速度
        /// </summary>
        private int _stsXrayXMinSp;

        /// <summary>
        /// X線管X軸最高速度
        /// </summary>
        private int _stsXrayXMaxSp;

        /// <summary>
        /// X線管X軸運転速度
        /// </summary>
        private int _stsXrayXSpeed;

        /// <summary>
        /// X線管X軸現在位置
        /// </summary>
        private int _stsXrayXPos;

        /// <summary>
        /// X線管Y軸最低速度
        /// </summary>
        private int _stsXrayYMinSp;

        /// <summary>
        /// X線管Y軸最高速度
        /// </summary>
        private int _stsXrayYMaxSp;

        /// <summary>
        /// X線管Y軸運転速度
        /// </summary>
        private int _stsXrayYSpeed;

        /// <summary>
        /// X線管Y軸現在位置
        /// </summary>
        private int _stsXrayYPos;

        /// <summary>
        /// X線管回転最低速度
        /// </summary>
        private int _stsXrayRotMinSp;

        /// <summary>
        /// X線管回転最高速度
        /// </summary>
        private int _stsXrayRotMaxSp;

        /// <summary>
        /// X線管回転運転速度
        /// </summary>
        private int _stsXrayRotSpeed;

        /// <summary>
        /// X線管回転現在位置
        /// </summary>
        private int _stsXrayRotPos;

        /// <summary>
        /// X線管回転加速度
        /// </summary>
        private int _stsXrayRotAccel;

        /// <summary>
        /// X線EXM最大出力値
        /// </summary>
        private int _stsEXMMaxW;

        /// <summary>
        /// X線EXM最大管電圧値
        /// </summary>
        private int _stsEXMMaxTV;

        /// <summary>
        /// X線EXM最小管電圧値
        /// </summary>
        private int _stsEXMMinTV;

        /// <summary>
        /// X線EXM最大管電流値
        /// </summary>
        private int _stsEXMMaxTC;

        /// <summary>
        /// X線EXM最小管電流値
        /// </summary>
        private int _stsEXMMinTC;

        /// <summary>
        /// X線EXM制限管電圧値
        /// </summary>
        private int _stsEXMLimitTV;

        /// <summary>
        /// X線EXM制限管電流値
        /// </summary>
        private int _stsEXMLimitTC;

        /// <summary>
        /// X線EXM管電圧実測値
        /// </summary>
        private int _stsEXMTV;

        /// <summary>
        /// X線EXM管電流実測値
        /// </summary>
        private int _stsEXMTC;

        /// <summary>
        /// X線EXM管電圧設定値
        /// </summary>
        private int _stsEXMTVSet;

        /// <summary>
        /// X線EXM管電流設定値
        /// </summary>
        private int _stsEXMTCSet;

        /// <summary>
        /// X線EXMｴﾗｰｺｰﾄﾞ
        /// </summary>
        private int _stsEXMErrCode;

        /// <summary>
        /// シーケンサがレディ状態になったか　v11.5追加 by 間々田 2006/07/12
        /// </summary>
        private bool _IsReady;

        /// <summary>
        /// 回転大ﾃｰﾌﾞﾙ有無    '追加 by 稲葉 14-02-26    //2014/10/06_v1951反映
        /// </summary>
        private bool _stsRotLargeTable;

        /// <summary>
        /// 検査室入室安全ｽｲｯﾁ '追加 by 稲葉 14-03-05  //2014/10/06_v1951反映
        /// </summary>
        private bool _stsRoomInSw;

        /// <summary>
        /// 冷蔵箱正規位置 追加 by 稲葉 15-03-12
        /// </summary>
        private bool _stsColdBoxDoorClose;

        /// <summary>
        /// 冷蔵箱扉閉 追加 by 稲葉 15-03-12
        /// </summary>
        private bool _stsColdBoxPosOK;

        /// <summary>
        /// ﾁﾙﾄﾃｰﾌﾞﾙ　ﾁﾙﾄ原点復帰 追加 by 稲葉 15-07-24
        /// </summary>
        private bool _TiltOrigin;

        /// <summary>
        /// ﾁﾙﾄﾃｰﾌﾞﾙ　回転原点復帰 追加 by 稲葉 15-07-24
        /// </summary>
        private bool _TiltRotOrigin;

        /// <summary>
        /// ﾏｲｸﾛﾌｫｰｶｽX線検出器位置 追加 by 稲葉 15-07-24
        /// </summary>
        private bool _stsMicroFPDPos;

        /// <summary>
        /// ﾅﾉﾌｫｰｶｽX線検出器位置 追加 by 稲葉 15-07-24
        /// </summary>
        private bool _stsNanoFPDPos;

        /// <summary>
        /// ﾏｲｸﾛﾌｫｰｶｽX線検出器ｼﾌﾄ位置 追加 by 稲葉 15-07-24
        /// </summary>
        private bool _stsMicroFPDShiftPos;

        /// <summary>
        /// ﾅﾉﾌｫｰｶｽX線検出器ｼﾌﾄ位置 追加 by 稲葉 15-07-24
        /// </summary>
        private bool _stsNanoFPDShiftPos;

        /// <summary>
        /// ﾏｲｸﾛﾌｫｰｶｽX線切替中 追加 by 稲葉 15-07-24
        /// </summary>
        private bool _stsMicroFPDBusy;

        /// <summary>
        /// ﾅﾉﾌｫｰｶｽX線切替中 追加 by 稲葉 15-07-24
        /// </summary>
        private bool _stsNanoFPDBusy;

        /// <summary>
        /// ﾏｲｸﾛﾌｫｰｶｽX線検出器ｼﾌﾄ中 追加 by 稲葉 15-07-24
        /// </summary>
        private bool _stsMicroFPDShiftBusy;

        /// <summary>
        /// ﾅﾉﾌｫｰｶｽX線検出器ｼﾌﾄ中 追加 by 稲葉 15-07-24
        /// </summary>
        private bool _stsNanoFPDShiftBusy;

        /// <summary>
        /// FCD軸ｲﾝﾀｰﾛｯｸﾘﾐｯﾄ位置補正 追加 by 稲葉 15-07-24
        /// </summary>
        private int _stsFCDLimitAdj;

        /// <summary>
        /// FDDﾘﾆｱｽｹｰﾙ値 追加 by 稲葉 15-07-24
        /// </summary>
        private int _stsLinearFDD;

        /// <summary>
        /// ﾃｰﾌﾞﾙY軸ﾘﾆｱｽｹｰﾙ値 追加 by 稲葉 15-07-24
        /// </summary>
        private int _stsLinearTableY;

        /// <summary>
        /// FCDﾘﾆｱｽｹｰﾙ値 追加 by 稲葉 15-07-24
        /// </summary>
        private int _stsLinearFCD;

        /// <summary>
        /// ﾁﾙﾄﾃｰﾌﾞﾙ　ﾁﾙﾄ手動速度 追加 by 稲葉 15-07-24
        /// </summary>
        private int _stsTiltSpeed;

        /// <summary>
        /// ﾁﾙﾄﾃｰﾌﾞﾙ　回転手動速度 追加 by 稲葉 15-07-24
        /// </summary>
        private int _stsTiltRotSpeed;

        /// <summary>
        /// 空調機水ｵｰﾊﾞｰﾌﾛｰ 追加 by 稲葉 15-12-14
        /// </summary>
        private bool _stsAirConOverFlow;

        /// <summary>
        /// FPD左ｼﾌﾄｼﾌﾄ位置 追加 by 稲葉 15-12-14
        /// </summary>
        private bool _stsFPDLShiftPos;

        /// <summary>
        /// FPD左ｼﾌﾄｼﾌﾄ位置移動中 追加 by 稲葉 15-12-14
        /// </summary>
        private bool _stsFPDLShiftBusy;

        /// <summary>
        /// FDｼｽﾃﾑ位置 追加 by 稲葉 15-12-14
        /// </summary>
        private bool _stsFDSystemPos;

        /// <summary>
        /// FDｼｽﾃﾑ位置移動中 追加 by 稲葉 15-12-14
        /// </summary>
        private bool _stsFDSystemBusy;

        /// <summary>
        /// AVｼｽﾃﾑ管電圧H X線OFF時刻 年 追加 by 稲葉 16-01-13
        /// </summary>
        private int _stsXrayHOffTimeY;

        /// <summary>
        /// AVｼｽﾃﾑ管電圧H X線OFF時刻 月日 追加 by 稲葉 16-01-13
        /// </summary>
        private int _stsXrayHOffTimeMD;

        /// <summary>
        /// AVｼｽﾃﾑ管電圧H X線OFF時刻 時分 追加 by 稲葉 16-01-13
        /// </summary>
        private int _stsXrayHOffTimeHM;

        /// <summary>
        /// AVｼｽﾃﾑ管電圧M X線OFF時刻 年 追加 by 稲葉 16-01-13
        /// </summary>
        private int _stsXrayMOffTimeY;

        /// <summary>
        /// AVｼｽﾃﾑ管電圧M X線OFF時刻 月日 追加 by 稲葉 16-01-13
        /// </summary>
        private int _stsXrayMOffTimeMD;

        /// <summary>
        /// AVｼｽﾃﾑ管電圧M X線OFF時刻 時分 追加 by 稲葉 16-01-13
        /// </summary>
        private int _stsXrayMOffTimeHM;

        #endregion

        /// <summary>
        /// ﾃｰﾌﾞﾙ上昇制限設定値 追加 by 稲葉 16-03-10
        /// </summary>
        private int _stsUpLimitPos;

        /// <summary>
        /// ﾃｰﾌﾞﾙ上昇制限解除 追加 by 稲葉 16-03-10
        /// </summary>
        private bool _stsUpLimitPermit;

        /// <summary>
        /// 電池検査用　ｺﾘﾒｰﾀ上異常　 追加 by 稲葉 16-04-14
        /// </summary>
        private bool _stsColiUErr;

        /// <summary>
        /// 電池検査用　ｺﾘﾒｰﾀ下異常　 追加 by 稲葉 16-04-14
        /// </summary>
        private bool _stsColiDErr;

        /// <summary>
        /// 電池検査用　ｺﾘﾒｰﾀ上原点復帰完了　 追加 by 稲葉 16-04-14
        /// </summary>
        private bool _stsColiUOriginOK;

        /// <summary>
        /// 電池検査用　ｺﾘﾒｰﾀ下原点復帰完了　 追加 by 稲葉 16-04-14
        /// </summary>
        private bool _stsColiDOriginOK;

        /// <summary>
        /// 電池検査用　ﾜｰｸ反転水平0°位置　 追加 by 稲葉 16-04-14
        /// </summary>
        private bool _stsWorkTurningHPos;

        /// <summary>
        /// 電池検査用　ﾜｰｸ反転縦90°位置　 追加 by 稲葉 16-04-14
        /// </summary>
        private bool _stsWorkTurningPPos;

        /// <summary>
        /// 電池検査用　ﾜｰｸ反転縦-90°位置　 追加 by 稲葉 16-04-14
        /// </summary>
        private bool _stsWorkTurningNPos;

        /// <summary>
        /// 電池検査用　手動運転ﾓｰﾄﾞ　 追加 by 稲葉 16-04-14
        /// </summary>
        private bool _stsManualOpeMode;

        /// <summary>
        /// 電池検査用　自動運転ﾓｰﾄﾞ　 追加 by 稲葉 16-04-14
        /// </summary>
        private bool _stsAutoOpeMode;

        /// <summary>
        /// 電池検査用　ﾃｨｰﾁﾝｸﾞ運転ﾓｰﾄﾞ　 追加 by 稲葉 16-04-14
        /// </summary>
        private bool _stsTeachingOpeMode;

        /// <summary>
        /// 電池検査用　自動運転中　 追加 by 稲葉 16-04-14
        /// </summary>
        private bool _stsAutoOpeBusy;

        /// <summary>
        /// 電池検査用　自動運転中　 追加 by 稲葉 16-04-14
        /// </summary>
        private bool _stsTeachingOpeBusy;

        /// <summary>
        /// 電池検査用　ｽｷｬﾝ(ﾃｨ-ﾁﾝｸﾞ)開始要求　 追加 by 稲葉 16-04-14
        /// </summary>
        private bool _ScanStartReq;

        /// <summary>
        /// 電池検査用　ｽｷｬﾝ(ﾃｨ-ﾁﾝｸﾞ)停止要求　 追加 by 稲葉 16-04-14
        /// </summary>
        private bool _ScanStopReq;

        /// <summary>
        /// 電池検査用　時刻設定要求　 追加 by 稲葉 16-04-14
        /// </summary>
        private bool _TimeSetReq;

        /// <summary>
        /// 電池検査用　異常ﾘｾｯﾄ要求　 追加 by 稲葉 16-04-14
        /// </summary>
        private bool _ErrResetReq;

        /// <summary>
        /// 電池検査用　X線ｳｫｰﾑｱｯﾌﾟ要求　 追加 by 稲葉 16-04-14
        /// </summary>
        private bool _XrayWarmupReq;

        /// <summary>
        /// 電池検査用　ﾜｰｸ反転原点復帰完了　 追加 by 稲葉 16-05-12
        /// </summary>
        private bool _stsWorkTurningOriginOK;

        /// <summary>
        /// 電池検査用　CT装置異常　 追加 by 稲葉 16-06-01
        /// </summary>
        private bool _stsCTError;

        /// <summary>
        /// 電池検査用　試料扉ｴﾘｱｾﾝｻ状態　 追加 by 稲葉 16-06-24
        /// </summary>
        private bool _stsAreaSensorDark;

        /// <summary>
        /// 電池検査用　ｺﾘﾒｰﾀ上現在値　 追加 by 稲葉 16-04-14
        /// </summary>
        private int _stsColiUPosition;

        /// <summary>
        /// 電池検査用　ｺﾘﾒｰﾀ下現在値　 追加 by 稲葉 16-04-14
        /// </summary>
        private int _stsColiDPosition;

        /// <summary>
        /// 電池検査用　電池ｼﾘｱﾙNo.　 追加 by 稲葉 16-04-14
        /// </summary>
        private string _stsWorkSerialNo;

        /// <summary>
        /// 電池検査用　PLC現在時刻 年月　 追加 by 稲葉 16-04-14
        /// </summary>
        private int _stsPlcYMTime;

        /// <summary>
        /// 電池検査用　PLC現在時刻 日時　 追加 by 稲葉 16-04-14
        /// </summary>
        private int _stsPlcDHTime;

        /// <summary>
        /// 電池検査用　PLC現在時刻 分秒　 追加 by 稲葉 16-04-14
        /// </summary>
        private int _stsPlcNSTime;

        /// <summary>
        /// 電池検査用　処理開始日時 年月　 追加 by 稲葉 16-04-14
        /// </summary>
        private int _stsStartYMTime;

        /// <summary>
        /// 電池検査用　処理開始日時 日時　 追加 by 稲葉 16-04-14
        /// </summary>
        private int _stsStartDHTime;

        /// <summary>
        /// 電池検査用　処理開始日時 分秒　 追加 by 稲葉 16-04-14
        /// </summary>
        private int _stsStartNSTime;

        /// <summary>
        /// FDZ2+高速度カメラ用　X線/ｶﾒﾗ昇降異常　追加 by 稲葉 19-03-04
        /// </summary>
        private bool _stsXrayCameraUDError;

        /// <summary>
        /// FDZ2+高速度カメラ用　X線/ｶﾒﾗ昇降動作中　追加 by 稲葉 19-03-04
        /// </summary>
        private bool _stsXrayCameraUDBusy;

        /// <summary>
        /// FDZ2+高速度カメラ用　X線/ｶﾒﾗ上昇限　追加 by 稲葉 19-03-04
        /// </summary>
        private bool _stsXrayCameraUpperLimit;

        /// <summary>
        /// FDZ2+高速度カメラ用　X線/ｶﾒﾗ下降限　追加 by 稲葉 19-03-04
        /// </summary>
        private bool _stsXrayCameraLowerLimit;

        /// <summary>
        /// FDZ2+高速度カメラ用　X線/ｶﾒﾗ昇降現在位置読出し　追加 by 稲葉 19-03-04
        /// </summary>
        private int _stsXrayCameraUDPosition;

        #region パブリックフィールド
        /// <summary>
        /// 通信中
        /// </summary>
        public bool stsCommBusy
        {
            get
            {
                lock (gLock)
                {
                    return _stsCommBusy;
                }
            }
            set 
            {
                lock (gLock)
                {
                    _stsCommBusy = value;
                } 
            }
        }
      
        /// <summary>
        /// ﾊﾟｿｺﾝ操作
        /// </summary>
        public bool PcInhibit
        {
            get
            {
                lock (gLock)
                {
                    return _PcInhibit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _PcInhibit = value;
                }
            }
        }
		    
        /// <summary>
        /// 運転準備ｽｲｯﾁ  '追加 by 稲葉 05-11-24
        /// </summary>
        public bool stsRunReadySW
        {
            get
            {
                lock (gLock)
                {
                    return _stsRunReadySW;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsRunReadySW = value;
                }
            }
        }
		    
        /// <summary>
        /// 扉ｲﾝﾀｰﾛｯｸ
        /// </summary>
        public bool stsDoorInterlock
        {
            get
            {
                lock (gLock)
                {
                    return _stsDoorInterlock;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsDoorInterlock = value;
                }
            }
        }
		    
        /// <summary>
        /// 非常停止
        /// </summary>
        public bool stsEmergency
        {
            get
            {
                lock (gLock)
                {
                    return _stsEmergency;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEmergency = value;
                }
            }
        }
		    
        /// <summary>
        /// X線225KVﾄﾘｯﾌﾟ
        /// </summary>
        public bool stsXray225Trip
        {
            get
            {
                lock (gLock)
                {
                    return _stsXray225Trip;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXray225Trip = value;
                }
            }
        }
		    
        /// <summary>
        /// X線160KVﾄﾘｯﾌﾟ
        /// </summary>
        public bool stsXray160Trip
        {
            get
            {
                lock (gLock)
                {
                    return _stsXray160Trip;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXray160Trip = value;
                }
            }
        }
		   
        /// <summary>
        /// ﾌｨﾙﾀﾕﾆｯﾄ接触
        /// </summary>
        public bool stsFilterTouch
        {
            get
            {
                lock (gLock)
                {
                    return _stsFilterTouch;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFilterTouch = value;
                }
            }
        }
		    
        /// <summary>
        /// X線225KV接触
        /// </summary>
        public bool stsXray225Touch
        {
            get
            {
                lock (gLock)
                {
                    return _stsXray225Touch;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXray225Touch = value;
                }
            }
        }
		    
        /// <summary>
        /// X線160KV接触
        /// </summary>
        public bool stsXray160Touch
        {
            get
            {
                lock (gLock)
                {
                    return _stsXray160Touch;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXray160Touch = value;
                }
            }
        }
		    
        /// <summary>
        /// 回転ﾃｰﾌﾞﾙ接触
        /// </summary>
        public bool stsRotTouch
        {
            get
            {
                lock (gLock)
                {
                    return _stsRotTouch;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsRotTouch = value;
                }
            }
        }
		
        /// <summary>
        /// 傾斜ﾃｰﾌﾞﾙ接触
        /// </summary>
        public bool stsTiltTouch
        {
            get
            {
                lock (gLock)
                {
                    return _stsTiltTouch;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTiltTouch = value;
                }
            }
        }
		
        /// <summary>
        /// ﾃｰﾌﾞﾙX軸ｵｰﾊﾞｰﾋｰﾄ
        /// </summary>
        public bool stsXDriverHeat
        {
            get
            {
                lock (gLock)
                {
                    return _stsXDriverHeat;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXDriverHeat = value;
                }
            }
        }
		
        /// <summary>
        /// ﾃｰﾌﾞﾙY軸ｵｰﾊﾞｰﾋｰﾄ
        /// </summary>
        public bool stsYDriverHeat
        {
            get
            {
                lock (gLock)
                {
                    return _stsYDriverHeat;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsYDriverHeat = value;
                }
            }
        }
		
        /// <summary>
        /// X線管切替ｵｰﾊﾞｰﾋｰﾄ
        /// </summary>
        public bool stsXrayDriverHeat
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayDriverHeat;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayDriverHeat = value;
                }
            }
        }
		
        /// <summary>
        /// ｼｰｹﾝｻCPUｴﾗｰ
        /// </summary>
        public bool stsSeqCpuErr
        {
            get
            {
                lock (gLock)
                {
                    return _stsSeqCpuErr;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsSeqCpuErr = value;
                }
            }
        }
		
        /// <summary>
        /// ｼｰｹﾝｻﾊﾞｯﾃﾘｰｴﾗｰ
        /// </summary>
        public bool stsSeqBatteryErr
        {
            get
            {
                lock (gLock)
                {
                    return _stsSeqBatteryErr;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsSeqBatteryErr = value;
                }
            }
        }
        
        /// <summary>
        /// ｼｰｹﾝｻKL通信ｴﾗｰ(KZ)
        /// </summary>
        public bool stsSeqKzCommErr
        {
            get
            {
                lock (gLock)
                {
                    return _stsSeqKzCommErr;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsSeqKzCommErr = value;
                }
            }
        }
		
        /// <summary>
        /// ｼｰｹﾝｻKL通信ｴﾗｰ(KV)
        /// </summary>
        public bool stsSeqKvCommErr
        {
            get
            {
                lock (gLock)
                {
                    return _stsSeqKvCommErr;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsSeqKvCommErr = value;
                }
            }
        }
		
        /// <summary>
        /// ﾌｨﾙﾀｰﾀｲﾑｱｳﾄ
        /// </summary>
        public bool stsFilterTimeout
        {
            get
            {
                lock (gLock)
                {
                    return _stsFilterTimeout;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFilterTimeout = value;
                }
            }
        }
		
        /// <summary>
        /// 傾斜ﾀｲﾑｱｳﾄ
        /// </summary>
        public bool stsTiltTimeout
        {
            get
            {
                lock (gLock)
                {
                    return _stsTiltTimeout;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTiltTimeout = value;
                }
            }
        }
		
        /// <summary>
        /// ﾃｰﾌﾞﾙX軸ﾀｲﾑｱｳﾄ
        /// </summary>
        public bool stsXTimeout
        {
            get
            {
                lock (gLock)
                {
                    return _stsXTimeout;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXTimeout = value;
                }
            }
        }
		
        /// <summary>
        /// I.I.軸ｵｰﾊﾞｰﾋｰﾄ
        /// </summary>
        public bool stsIIDriverHeat
        {
            get
            {
                lock (gLock)
                {
                    return _stsIIDriverHeat;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIIDriverHeat = value;
                }
            }
        }

        /// <summary>
        /// ｼｰｹﾝｻｶｳﾝﾀﾕﾆｯﾄｴﾗｰ
        /// </summary>
        public bool stsSeqCounterErr
        {
            get
            {
                lock (gLock)
                {
                    return _stsSeqCounterErr;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsSeqCounterErr = value;
                }
            }
        }

        /// <summary>
        /// X軸動作ｴﾗｰ
	    /// </summary>
        public bool stsXDriveErr
        {
            get
            {
                lock (gLock)
                {
                    return _stsXDriveErr;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXDriveErr = value;
                }
            }
        }

	    /// <summary>
        /// ﾃｰﾌﾞﾙX左限
	    /// </summary>
        public bool stsXLLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsXLLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXLLimit = value;
                }
            }
        }
		    
	    /// <summary>
        /// ﾃｰﾌﾞﾙX右限
	    /// </summary>
        public bool stsXRLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsXRLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXRLimit = value;
                }
            }
        }
		
	    /// <summary>
        /// ﾃｰﾌﾞﾙX左移動中
	    /// </summary>
        public bool stsXLeft
        {
            get
            {
                lock (gLock)
                {
                    return _stsXLeft;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXLeft = value;
                }
            }
        }
		
	    /// <summary>
        /// ﾃｰﾌﾞﾙX右移動中
	    /// </summary>
        public bool stsXRight
        {
            get
            {
                lock (gLock)
                {
                    return _stsXRight;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXRight = value;
                }
            }
        }
		
	    /// <summary>
        /// ﾃｰﾌﾞﾙX左移動中接触
	    /// </summary>
        public bool stsXLTouch
        {
            get
            {
                lock (gLock)
                {
                    return _stsXLTouch;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXLTouch = value;
                }
            }
        }
		
	    /// <summary>
        /// ﾃｰﾌﾞﾙX右移動中接触
	    /// </summary>
        public bool stsXRTouch
        {
            get
            {
                lock (gLock)
                {
                    return _stsXRTouch;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXRTouch = value;
                }
            }
        }
		
	    /// <summary>
        /// ﾃｰﾌﾞﾙY前進限
	    /// </summary>
        public bool stsYFLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsYFLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsYFLimit = value;
                }
            }
        }
		
	    /// <summary>
        /// ﾃｰﾌﾞﾙY後退限
	    /// </summary>
        public bool stsYBLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsYBLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsYBLimit = value;
                }
            }
        }
		
	    /// <summary>
        /// ﾃｰﾌﾞﾙY前進中
	    /// </summary>
        public bool stsYForward
        {
            get
            {
                lock (gLock)
                {
                    return _stsYForward;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsYForward = value;
                }
            }
        }
		
	    /// <summary>
        /// ﾃｰﾌﾞﾙY後退中
	    /// </summary>
        public bool stsYBackward
        {
            get
            {
                lock (gLock)
                {
                    return _stsYBackward;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsYBackward = value;
                }
            }
        }
		
	    /// <summary>
        /// ﾃｰﾌﾞﾙY前進中接触
	    /// </summary>
        public bool stsYFTouch
        {
            get
            {
                lock (gLock)
                {
                    return _stsYFTouch;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsYFTouch = value;
                }
            }
        }
		
	    /// <summary>
        /// ﾃｰﾌﾞﾙY後退中接触
	    /// </summary>
        public bool stsYBTouch
        {
            get
            {
                lock (gLock)
                {
                    return _stsYBTouch;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsYBTouch = value;
                }
            }
        }
		
	    /// <summary>
        /// I.I.前進限
	    /// </summary>
        public bool stsIIFLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsIIFLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIIFLimit = value;
                }
            }
        }
		
	    /// <summary>
        /// I.I.後退限
	    /// </summary>
        public bool stsIIBLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsIIBLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIIBLimit = value;
                }
            }
        }
		
	    /// <summary>
        /// I.I.前進中
	    /// </summary>
        public bool stsIIForward
        {
            get
            {
                lock (gLock)
                {
                    return _stsIIForward;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIIForward = value;
                }
            }
        }
		
	    /// <summary>
        /// I.I.後退中
	    /// </summary>
        public bool stsIIBackward
        {
            get
            {
                lock (gLock)
                {
                    return _stsIIBackward;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIIBackward = value;
                }
            }
        }

        /// <summary>
        /// 傾斜CW限
        /// </summary>
        public bool stsTiltCwLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsTiltCwLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTiltCwLimit = value;
                }
            }
        }
		    
	    /// <summary>
        /// 傾斜CCW限
	    /// </summary>
        public bool stsTiltCCwLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsTiltCCwLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTiltCCwLimit = value;
                }
            }
        }
		
	    /// <summary>
        /// 傾斜原点（水平）
	    /// </summary>
        public bool stsTiltOrigin
        {
            get
            {
                lock (gLock)
                {
                    return _stsTiltOrigin;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTiltOrigin = value;
                }
            }
        }
		
	    /// <summary>
        /// 傾斜CW中
	    /// </summary>
        public bool stsTiltCw
        {
            get
            {
                lock (gLock)
                {
                    return _stsTiltCw;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTiltCw = value;
                }
            }
        }
		
	    /// <summary>
        /// 傾斜CCW中
	    /// </summary>
        public bool stsTiltCCw
        {
            get
            {
                lock (gLock)
                {
                    return _stsTiltCCw;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTiltCCw = value;
                }
            }
        }
		
	    /// <summary>
        /// 傾斜原点復帰中
	    /// </summary>
        public bool stsTiltOriginRun
        {
            get
            {
                lock (gLock)
                {
                    return _stsTiltOriginRun;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTiltOriginRun = value;
                }
            }
        }
		
	    /// <summary>
        /// ｺﾘﾒｰﾀ左開限
	    /// </summary>
        public bool stsColliLOLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsColliLOLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColliLOLimit = value;
                }
            }
        }
		
	    /// <summary>
        /// ｺﾘﾒｰﾀ左閉限
	    /// </summary>
        public bool stsColliLCLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsColliLCLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColliLCLimit = value;
                }
            }
        }
		
	    /// <summary>
        /// ｺﾘﾒｰﾀ右開限
	    /// </summary>
        public bool stsColliROLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsColliROLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColliROLimit = value;
                }
            }
        }
		
	    /// <summary>
        /// ｺﾘﾒｰﾀ右閉限
	    /// </summary>
        public bool stsColliRCLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsColliRCLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColliRCLimit = value;
                }
            }
        }
		
	    /// <summary>
        /// ｺﾘﾒｰﾀ上開限
	    /// </summary>
        public bool stsColliUOLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsColliUOLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColliUOLimit = value;
                }
            }
        }
		
        /// <summary>
        /// ｺﾘﾒｰﾀ上閉限
        /// </summary>
        public bool stsColliUCLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsColliUCLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColliUCLimit = value;
                }
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ下開限
        /// </summary>
        public bool stsColliDOLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsColliDOLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColliDOLimit = value;
                }
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ下閉限
        /// </summary>
        public bool stsColliDCLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsColliDCLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColliDCLimit = value;
                }
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ左開中
        /// </summary>
        public bool stsColliLOpen
        {
            get
            {
                lock (gLock)
                {
                    return _stsColliLOpen;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColliLOpen = value;
                }
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ左閉中
        /// </summary>
        public bool stsColliLClose
        {
            get
            {
                lock (gLock)
                {
                    return _stsColliLClose;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColliLClose = value;
                }
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ右開中
        /// </summary>
        public bool stsColliROpen
        {
            get
            {
                lock (gLock)
                {
                    return _stsColliROpen;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColliROpen = value;
                }
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ右閉中
        /// </summary>
        public bool stsColliRClose
        {
            get
            {
                lock (gLock)
                {
                    return _stsColliRClose;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColliRClose = value;
                }
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ上開中
        /// </summary>
        public bool stsColliUOpen
        {
            get
            {
                lock (gLock)
                {
                    return _stsColliUOpen;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColliUOpen = value;
                }
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ上閉中
        /// </summary>
        public bool stsColliUClose
        {
            get
            {
                lock (gLock)
                {
                    return _stsColliUClose;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColliUClose = value;
                }
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ下開中
        /// </summary>
        public bool stsColliDOpen
        {
            get
            {
                lock (gLock)
                {
                    return _stsColliDOpen;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColliDOpen = value;
                }
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ下閉中
        /// </summary>
        public bool stsColliDClose
        {
            get
            {
                lock (gLock)
                {
                    return _stsColliDClose;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColliDClose = value;
                }
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ無し位置
        /// </summary>
        public bool stsFilter0
        {
            get
            {
                lock (gLock)
                {
                    return _stsFilter0;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFilter0 = value;
                }
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ1位置
        /// </summary>
        public bool stsFilter1
        {
            get
            {
                lock (gLock)
                {
                    return _stsFilter1;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFilter1 = value;
                }
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ2位置
        /// </summary>
        public bool stsFilter2
        {
            get
            {
                lock (gLock)
                {
                    return _stsFilter2;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFilter2 = value;
                }
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ3位置
        /// </summary>
        public bool stsFilter3
        {
            get
            {
                lock (gLock)
                {
                    return _stsFilter3;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFilter3 = value;
                }
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ4位置
        /// </summary>
        public bool stsFilter4
        {
            get
            {
                lock (gLock)
                {
                    return _stsFilter4;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFilter4 = value;
                }
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ5位置
        /// </summary>
        public bool stsFilter5
        {
            get
            {
                lock (gLock)
                {
                    return _stsFilter5;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFilter5 = value;
                }
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ無し動作中
        /// </summary>
        public bool stsFilter0Run
        {
            get
            {
                lock (gLock)
                {
                    return _stsFilter0Run;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFilter0Run = value;
                }
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ1動作中
        /// </summary>
        public bool stsFilter1Run
        {
            get
            {
                lock (gLock)
                {
                    return _stsFilter1Run;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFilter1Run = value;
                }
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ2動作中
        /// </summary>
        public bool stsFilter2Run
        {
            get
            {
                lock (gLock)
                {
                    return _stsFilter2Run;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFilter2Run = value;
                }
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ3動作中
        /// </summary>
        public bool stsFilter3Run
        {
            get
            {
                lock (gLock)
                {
                    return _stsFilter3Run;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFilter3Run = value;
                }
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ4動作中
        /// </summary>
        public bool stsFilter4Run
        {
            get
            {
                lock (gLock)
                {
                    return _stsFilter4Run;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFilter4Run = value;
                }
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ5動作中
        /// </summary>
        public bool stsFilter5Run
        {
            get
            {
                lock (gLock)
                {
                    return _stsFilter5Run;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFilter5Run = value;
                }
            }
        }

        /// <summary>
        /// X線ON
        /// </summary>
        public bool XrayOn
        {
            get
            {
                lock (gLock)
                {
                    return _XrayOn;
                }
            }
            set
            {
                lock (gLock)
                {
                    _XrayOn = value;
                }
            }
        }

        /// <summary>
        /// X線OFF
        /// </summary>
        public bool XrayOff
        {
            get
            {
                lock (gLock)
                {
                    return _XrayOff;
                }
            }
            set
            {
                lock (gLock)
                {
                    _XrayOff = value;
                }
            }
        }

        /// <summary>
        /// ｽﾃｰｼﾞX左移動
        /// </summary>
        public bool XStgLeft
        {
            get
            {
                lock (gLock)
                {
                    return _XStgLeft;
                }
            }
            set
            {
                lock (gLock)
                {
                    _XStgLeft = value;
                }
            }
        }

        /// <summary>
        /// ｽﾃｰｼﾞX右移動
        /// </summary>
        public bool XStgRight
        {
            get
            {
                lock (gLock)
                {
                    return _XStgRight;
                }
            }
            set
            {
                lock (gLock)
                {
                    _XStgRight = value;
                }
            }
        }

        /// <summary>
        /// ｽﾃｰｼﾞX原点復帰
        /// </summary>
        public bool XStgOrigin
        {
            get
            {
                lock (gLock)
                {
                    return _XStgOrigin;
                }
            }
            set
            {
                lock (gLock)
                {
                    _XStgOrigin = value;
                }
            }
        }

        /// <summary>
        /// ｽﾃｰｼﾞY前進
        /// </summary>
        public bool YStgForward
        {
            get
            {
                lock (gLock)
                {
                    return _YStgForward;
                }
            }
            set
            {
                lock (gLock)
                {
                    _YStgForward = value;
                }
            }
        }

        /// <summary>
        /// ｽﾃｰｼﾞY後退
        /// </summary>
        public bool YStgBackward
        {
            get
            {
                lock (gLock)
                {
                    return _YStgBackward;
                }
            }
            set
            {
                lock (gLock)
                {
                    _YStgBackward = value;
                }
            }
        }

        /// <summary>
        /// ｽﾃｰｼﾞY原点復帰
        /// </summary>
        public bool YStgOrigin
        {
            get
            {
                lock (gLock)
                {
                    return _YStgOrigin;
                }
            }
            set
            {
                lock (gLock)
                {
                    _YStgOrigin = value;
                }
            }
        }

        /// <summary>
        /// 回転位置決め要求
        /// </summary>
        public bool RotIndex
        {
            get
            {
                lock (gLock)
                {
                    return _RotIndex;
                }
            }
            set
            {
                lock (gLock)
                {
                    _RotIndex = value;
                }
            }
        }

        /// <summary>
        /// I.I.視野9”
        /// </summary>
        public bool stsII9
        {
            get
            {
                lock (gLock)
                {
                    return _stsII9;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsII9 = value;
                }
            }
        }

        /// <summary>
        /// I.I.視野6”
        /// </summary>
        public bool stsII6
        {
            get
            {
                lock (gLock)
                {
                    return _stsII6;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsII6 = value;
                }
            }
        }

        /// <summary>
        /// I.I.視野4.5”
        /// </summary>
        public bool stsII4
        {
            get
            {
                lock (gLock)
                {
                    return _stsII4;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsII4 = value;
                }
            }
        }

        /// <summary>
        /// I.I.電源
        /// </summary>
        public bool stsIIPower
        {
            get
            {
                lock (gLock)
                {
                    return _stsIIPower;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIIPower = value;
                }
            }
        }

        /// <summary>
        /// ｽﾗｲｽﾗｲﾄ
        /// </summary>
        public bool stsSLight
        {
            get
            {
                lock (gLock)
                {
                    return _stsSLight;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsSLight = value;
                }
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙｲﾝﾀｰﾛｯｸ解除
        /// </summary>
        public bool stsTableMovePermit
        {
            get
            {
                lock (gLock)
                {
                    return _stsTableMovePermit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTableMovePermit = value;
                }
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙ回転、上昇、微調動作禁止
        /// </summary>
        public bool stsMechaPermit
        {
            get
            {
                lock (gLock)
                {
                    return _stsMechaPermit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsMechaPermit = value;
                }
            }
        }

        /// <summary>
        /// 昇降位置決め要求
        /// </summary>
        public bool UpDownIndex
        {
            get
            {
                lock (gLock)
                {
                    return _UpDownIndex;
                }
            }
            set
            {
                lock (gLock)
                {
                    _UpDownIndex = value;
                }
            }
        }

        /// <summary>
        /// ｽﾃｰｼﾞX位置決め要求
        /// </summary>
        public bool XStageIndex
        {
            get
            {
                lock (gLock)
                {
                    return _XStageIndex;
                }
            }
            set
            {
                lock (gLock)
                {
                    _XStageIndex = value;
                }
            }
        }

        /// <summary>
        /// ｽﾃｰｼﾞY位置決め要求
        /// </summary>
        public bool YStageIndex
        {
            get
            {
                lock (gLock)
                {
                    return _YStageIndex;
                }
            }
            set
            {
                lock (gLock)
                {
                    _YStageIndex = value;
                }
            }
        }

        /// <summary>
        /// ﾒｶﾃﾞﾊﾞｲｽ　ｴﾗｰﾘｾｯﾄ
        /// </summary>
        public bool DeviceErrReset
        {
            get
            {
                lock (gLock)
                {
                    return _DeviceErrReset;
                }
            }
            set
            {
                lock (gLock)
                {
                    _DeviceErrReset = value;
                }
            }
        }

        /// <summary>
        /// 昇降原点復帰
        /// </summary>
        public bool UdOrigin
        {
            get
            {
                lock (gLock)
                {
                    return _UdOrigin;
                }
            }
            set
            {
                lock (gLock)
                {
                    _UdOrigin = value;
                }
            }
        }

        /// <summary>
        /// 回転原点復帰
        /// </summary>
        public bool RotOrigin
        {
            get
            {
                lock (gLock)
                {
                    return _RotOrigin;
                }
            }
            set
            {
                lock (gLock)
                {
                    _RotOrigin = value;
                }
            }
        }

        /// <summary>
        /// 回転中心校正ﾃｰﾌﾞﾙX移動有り
        /// </summary>
        public bool stsRotXChange
        {
            get
            {
                lock (gLock)
                {
                    return _stsRotXChange;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsRotXChange = value;
                }
            }
        }

        /// <summary>
        /// 寸法校正ﾃｰﾌﾞﾙX移動有り
        /// </summary>
        public bool stsDisXChange
        {
            get
            {
                lock (gLock)
                {
                    return _stsDisXChange;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsDisXChange = value;
                }
            }
        }

        /// <summary>
        /// 回転中心校正ﾃｰﾌﾞﾙY移動有り
        /// </summary>
        public bool stsRotYChange
        {
            get
            {
                lock (gLock)
                {
                    return _stsRotYChange;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsRotYChange = value;
                }
            }
        }

        /// <summary>
        /// 寸法校正ﾃｰﾌﾞﾙY移動有り
        /// </summary>
        public bool stsDisYChange
        {
            get
            {
                lock (gLock)
                {
                    return _stsDisYChange;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsDisYChange = value;
                }
            }
        }

        /// <summary>
        /// 幾何歪校正I.I.移動有り
        /// </summary>
        public bool stsVerIIChange
        {
            get
            {
                lock (gLock)
                {
                    return _stsVerIIChange;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsVerIIChange = value;
                }
            }
        }

        /// <summary>
        /// 回転中心校正I.I.移動有り
        /// </summary>
        public bool stsRotIIChange
        {
            get
            {
                lock (gLock)
                {
                    return _stsRotIIChange;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsRotIIChange = value;
                }
            }
        }

        /// <summary>
        /// ｹﾞｲﾝ校正I.I.移動有り
        /// </summary>
        public bool stsGainIIChange
        {
            get
            {
                lock (gLock)
                {
                    return _stsGainIIChange;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsGainIIChange = value;
                }
            }
        }

        /// <summary>
        /// 寸法校正I.I.移動有り
        /// </summary>
        public bool stsDisIIChange
        {
            get
            {
                lock (gLock)
                {
                    return _stsDisIIChange;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsDisIIChange = value;
                }
            }
        }

        /// <summary>
        /// ｽｷｬﾝ位置校正I.I.移動有り '追加 by 稲葉 05-10-21
        /// </summary>
        public bool stsSPIIChange
        {
            get
            {
                lock (gLock)
                {
                    return _stsSPIIChange;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsSPIIChange = value;
                }
            }
        }

        /// <summary>
        /// X線管X軸ｴﾗｰ
        /// </summary>
        public bool stsXrayXErr
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayXErr;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayXErr = value;
                }
            }
        }

        /// <summary>
        /// X線管Y軸ｴﾗｰ
        /// </summary>
        public bool stsXrayYErr
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayYErr;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayYErr = value;
                }
            }
        }

        /// <summary>
        /// X線管回転ｴﾗｰ
        /// </summary>
        public bool stsXrayRotErr
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayRotErr;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayRotErr = value;
                }
            }
        }

        /// <summary>
        /// X線管X軸左限
        /// </summary>
        public bool stsXrayXLLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayXLLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayXLLimit = value;
                }
            }
        }

        /// <summary>
        /// X線管X軸右限
        /// </summary>
        public bool stsXrayXRLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayXRLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayXRLimit = value;
                }
            }
        }

        /// <summary>
        /// X線管X軸左移動中
        /// </summary>
        public bool stsXrayXL
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayXL;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayXL = value;
                }
            }
        }

        /// <summary>
        /// X線管X軸右移動中
        /// </summary>
        public bool stsXrayXR
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayXR;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayXR = value;
                }
            }
        }

        /// <summary>
        /// 回転中心校正X線管X軸移動有り
        /// </summary>
        public bool stsRotXrayXCh
        {
            get
            {
                lock (gLock)
                {
                    return _stsRotXrayXCh;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsRotXrayXCh = value;
                }
            }
        }

        /// <summary>
        /// 寸法校正X線管X軸移動有り
        /// </summary>
        public bool stsDisXrayXCh
        {
            get
            {
                lock (gLock)
                {
                    return _stsDisXrayXCh;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsDisXrayXCh = value;
                }
            }
        }

        /// <summary>
        /// X線管Y軸前進限
        /// </summary>
        public bool stsXrayYFLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayYFLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayYFLimit = value;
                }
            }
        }

        /// <summary>
        /// X線管Y軸後退限
        /// </summary>
        public bool stsXrayYBLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayYBLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayYBLimit = value;
                }
            }
        }

        /// <summary>
        /// X線管Y軸前進移動中
        /// </summary>
        public bool stsXrayYF
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayYF;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayYF = value;
                }
            }
        }

        /// <summary>
        /// X線管Y軸後退移動中
        /// </summary>
        public bool stsXrayYB
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayYB;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayYB = value;
                }
            }
        }

        /// <summary>
        /// 回転中心校正X線管Y軸移動有り
        /// </summary>
        public bool stsRotXrayYCh
        {
            get
            {
                lock (gLock)
                {
                    return _stsRotXrayYCh;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsRotXrayYCh = value;
                }
            }
        }

        /// <summary>
        /// 寸法校正X線管Y軸移動有り
        /// </summary>
        public bool stsDisXrayYCh
        {
            get
            {
                lock (gLock)
                {
                    return _stsDisXrayYCh;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsDisXrayYCh = value;
                }
            }
        }

        /// <summary>
        /// X線管正転限
        /// </summary>
        public bool stsXrayCWLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayCWLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayCWLimit = value;
                }
            }
        }

        /// <summary>
        /// X線管逆転限
        /// </summary>
        public bool stsXrayCCWLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayCCWLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayCCWLimit = value;
                }
            }
        }

        /// <summary>
        /// X線管正転中
        /// </summary>
        public bool stsXrayCW
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayCW;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayCW = value;
                }
            }
        }

        /// <summary>
        /// X線管逆転中
        /// </summary>
        public bool stsXrayCCW
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayCCW;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayCCW = value;
                }
            }
        }

        /// <summary>
        /// X線管回転動作不可
        /// </summary>
        public bool stsXrayRotLock
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayRotLock;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayRotLock = value;
                }
            }
        }

        /// <summary>
        /// 試料扉電磁ﾛｯｸｷｰ挿入完了
        /// </summary>
        public bool stsDoorKey
        {
            get
            {
                lock (gLock)
                {
                    return _stsDoorKey;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsDoorKey = value;
                }
            }
        }

        /// <summary>
        /// 試料扉電磁ﾛｯｸON中
        /// </summary>
        public bool stsDoorLock
        {
            get
            {
                lock (gLock)
                {
                    return _stsDoorLock;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsDoorLock = value;
                }
            }
        }

	    /// <summary>
        /// X線EXM ON中
	    /// </summary>
        public bool stsEXMOn
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMOn;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMOn = value;
                }
            }
        }

	    /// <summary>
        /// X線EXM ON可能
	    /// </summary>
        public bool stsEXMReady
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMReady;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMReady = value;
                }
            }
        }

	    /// <summary>
        /// X線EXM発生装置正常
	    /// </summary>
        public bool stsEXMNormal1
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMNormal1;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMNormal1 = value;
                }
            }
        }

	    /// <summary>
        /// X線EXMﾃﾞｰﾀ書込正常
	    /// </summary>
        public bool stsEXMNormal2
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMNormal2;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMNormal2 = value;
                }
            }
        }

	    /// <summary>
        /// X線EXMｳｫｰﾑｱｯﾌﾟ必要又は実行中
	    /// </summary>
        public bool stsEXMWU
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMWU;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMWU = value;
                }
            }
        }

	    /// <summary>
        /// X線EXMﾘﾓｰﾄ中
	    /// </summary>
        public bool stsEXMRemote
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMRemote;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMRemote = value;
                }
            }
        }
		    
	    /// <summary>
        /// CT用I.I.撮影位置
	    /// </summary>
        public bool stsCTIIPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsCTIIPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsCTIIPos = value;
                }
            }
        }
		
	    /// <summary>
        /// 高速用I.I.撮影位置
	    /// </summary>
        public bool stsTVIIPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsTVIIPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTVIIPos = value;
                }
            }
        }
		
	    /// <summary>
        /// CT用I.I.切替中
	    /// </summary>
        public bool stsCTIIDrive
        {
            get
            {
                lock (gLock)
                {
                    return _stsCTIIDrive;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsCTIIDrive = value;
                }
            }
        }
		
	    /// <summary>
        /// 高速用I.I.切替中
	    /// </summary>
        public bool stsTVIIDrive
        {
            get
            {
                lock (gLock)
                {
                    return _stsTVIIDrive;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTVIIDrive = value;
                }
            }
        }
		
	    /// <summary>
        /// 高速用I.I.視野9”
	    /// </summary>
        public bool stsTVII9
        {
            get
            {
                lock (gLock)
                {
                    return _stsTVII9;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTVII9 = value;
                }
            }
        }
	    
        /// <summary>
        /// 高速用I.I.視野6”
        /// </summary>
        public bool stsTVII6
        {
            get
            {
                lock (gLock)
                {
                    return _stsTVII6;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTVII6 = value;
                }
            }
        }
		
	    /// <summary>
        /// 高速用I.I.視野4.5”
	    /// </summary>
        public bool stsTVII4
        {
            get
            {
                lock (gLock)
                {
                    return _stsTVII4;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTVII4 = value;
                }
            }
        }
		
	    /// <summary>
        /// 高速用I.I.電源
	    /// </summary>
        public bool stsTVIIPower
        {
            get
            {
                lock (gLock)
                {
                    return _stsTVIIPower;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTVIIPower = value;
                }
            }
        }
		
	    /// <summary>
        /// 高速用I.I.電源
	    /// </summary>
        public bool stsCameraPower
        {
            get
            {
                lock (gLock)
                {
                    return _stsCameraPower;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsCameraPower = value;
                }
            }
        }
		
	    /// <summary>
        /// I.I.絞り左開中
	    /// </summary>
        public bool stsIrisLOpen
        {
            get
            {
                lock (gLock)
                {
                    return _stsIrisLOpen;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIrisLOpen = value;
                }
            }
        }
		
	    /// <summary>
        /// I.I.絞り左閉中
	    /// </summary>
        public bool stsIrisLClose
        {
            get
            {
                lock (gLock)
                {
                    return _stsIrisLClose;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIrisLClose = value;
                }
            }
        }
		
	    /// <summary>
        /// I.I.絞り右開中
	    /// </summary>
        public bool stsIrisROpen
        {
            get
            {
                lock (gLock)
                {
                    return _stsIrisROpen;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIrisROpen = value;
                }
            }
        }
		
	    /// <summary>
        /// I.I.絞り右閉中
	    /// </summary>
        public bool stsIrisRClose
        {
            get
            {
                lock (gLock)
                {
                    return _stsIrisRClose;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIrisRClose = value;
                }
            }
        }
		
	    /// <summary>
        /// I.I.絞り上開中
	    /// </summary>
        public bool stsIrisUOpen
        {
            get
            {
                lock (gLock)
                {
                    return _stsIrisUOpen;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIrisUOpen = value;
                }
            }
        }
		
	    /// <summary>
        /// I.I.絞り上閉中
	    /// </summary>
        public bool stsIrisUClose
        {
            get
            {
                lock (gLock)
                {
                    return _stsIrisUClose;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIrisUClose = value;
                }
            }
        }
		
	    /// <summary>
        /// I.I.絞り下開中
	    /// </summary>
        public bool stsIrisDOpen
        {
            get
            {
                lock (gLock)
                {
                    return _stsIrisDOpen;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIrisDOpen = value;
                }
            }
        }
		
	    /// <summary>
        /// I.I.絞り下閉中
	    /// </summary>
        public bool stsIrisDClose
        {
            get
            {
                lock (gLock)
                {
                    return _stsIrisDClose;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIrisDClose = value;
                }
            }
        }
		
	    /// <summary>
        /// 動作制限自動復帰設定状態   '追加 by 稲葉 10-10-19
	    /// </summary>
        public bool stsAutoRestrict
        {
            get
            {
                lock (gLock)
                {
                    return _stsAutoRestrict;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsAutoRestrict = value;
                }
            }
        }
		
	    /// <summary>
        /// Y軸ｲﾝﾃﾞｯｸｽ減速設定状態     '追加 by 稲葉 10-10-19
	    /// </summary>
        public bool stsYIndexSlow
        {
            get
            {
                lock (gLock)
                {
                    return _stsYIndexSlow;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsYIndexSlow = value;
                }
            }
        }
		
	    /// <summary>
        /// 動作用扉ｲﾝﾀｰﾛｯｸ設定状態    '追加 by 稲葉 10-10-19
	    /// </summary>
        public bool stsDoorPermit
        {
            get
            {
                lock (gLock)
                {
                    return _stsDoorPermit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsDoorPermit = value;
                }
            }
        }

        /// <summary>
        /// ｼｬｯﾀ位置    '追加 by 稲葉 10-11-22
        /// </summary>
        public bool stsShutter
        {
            get
            {
                lock (gLock)
                {
                    return _stsShutter;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsShutter = value;
                }
            }
        }

        /// <summary>
        /// ｼｬｯﾀ動作中  '追加 by 稲葉 10-11-22
        /// </summary>
        public bool stsShutterBusy
        {
            get
            {
                lock (gLock)
                {
                    return _stsShutterBusy;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsShutterBusy = value;
                }
            }
        }
	    
        /// <summary>
        /// ﾃｰﾌﾞﾙ左右原点復帰要求
        /// </summary>
        public bool stsXOrgReq
        {
            get
            {
                lock (gLock)
                {
                    return _stsXOrgReq;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXOrgReq = value;
                }
            }
        }
	    
        /// <summary>
        /// ﾃｰﾌﾞﾙ前後原点復帰要求
        /// </summary>
        public bool stsYOrgReq
        {
            get
            {
                lock (gLock)
                {
                    return _stsYOrgReq;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsYOrgReq = value;
                }
            }
        }
	    
        /// <summary>
        /// 検出器前後原点復帰要求
        /// </summary>
        public bool stsIIOrgReq
        {
            get
            {
                lock (gLock)
                {
                    return _stsIIOrgReq;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIIOrgReq = value;
                }
            }
        }
	    
        /// <summary>
        /// 検出器切替原点復帰要求
        /// </summary>
        public bool stsIIChgOrgReq
        {
            get
            {
                lock (gLock)
                {
                    return _stsIIChgOrgReq;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIIChgOrgReq = value;
                }
            }
        }
	    
        /// <summary>
        /// ﾒｶﾘｾｯﾄ中                  '追加 by 稲葉 10-09-10
        /// </summary>
        public bool stsMechaRstBusy
        {
            get
            {
                lock (gLock)
                {
                    return _stsMechaRstBusy;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsMechaRstBusy = value;
                }
            }
        }
		
	    /// <summary>
        /// ﾒｶﾘｾｯﾄ完了                '追加 by 稲葉 10-09-10
	    /// </summary>
        public bool stsMechaRstOK
        {
            get
            {
                lock (gLock)
                {
                    return _stsMechaRstOK;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsMechaRstOK = value;
                }
            }
        }
	    
        /// <summary>
        /// FID
        /// </summary>
        public int stsFID
        {
            get
            {
                lock (gLock)
                {
                    return _stsFID;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFID = value;
                }
            }
        }
	    
        /// <summary>
        /// FCD
        /// </summary>
        public int stsFCD
        {
            get
            {
                lock (gLock)
                {
                    return _stsFCD;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFCD = value;
                }
            }
        }
	    
        /// <summary>
        /// ﾃｰﾌﾞﾙX最低速度
        /// </summary>
        public int stsXMinSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsXMinSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXMinSpeed = value;
                }
            }
        }
	    
        /// <summary>
        /// ﾃｰﾌﾞﾙX最高速度
        /// </summary>
        public int stsXMaxSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsXMaxSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXMaxSpeed = value;
                }
            }
        }
	    
        /// <summary>
        /// ﾃｰﾌﾞﾙX運転速度
        /// </summary>
        public int stsXSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsXSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXSpeed = value;
                }
            }
        }
		
	    /// <summary>
        /// ﾃｰﾌﾞﾙY最低速度
	    /// </summary>
        public int stsYMinSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsYMinSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsYMinSpeed = value;
                }
            }
        }
	    
        /// <summary>
        /// ﾃｰﾌﾞﾙY最高速度
        /// </summary>
        public int stsYMaxSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsYMaxSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsYMaxSpeed = value;
                }
            }
        }
	    
        /// <summary>
        /// ﾃｰﾌﾞﾙY運転速度
        /// </summary>
        public int stsYSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsYSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsYSpeed = value;
                }
            }
        }

        /// <summary>
        /// FCD減速速度 追加 by 稲葉 18-01-15
        /// </summary>
        public int stsFcdDecelerationSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsFcdDecelerationSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFcdDecelerationSpeed = value;
                }
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙX現在値
        /// </summary>
        public int stsXPosition
        {
            get
            {
                lock (gLock)
                {
                    return _stsXPosition;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXPosition = value;
                }
            }
        }
	    
        /// <summary>
        /// I.I.最低速度
        /// </summary>
        public int stsIIMinSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsIIMinSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIIMinSpeed = value;
                }
            }
        }
	    
        /// <summary>
        /// I.I.最高速度
        /// </summary>
        public int stsIIMaxSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsIIMaxSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIIMaxSpeed = value;
                }
            }
        }
		
        /// <summary>
        /// I.I.運転速度
        /// </summary>
        public int stsIISpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsIISpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsIISpeed = value;
                }
            }
        }

        /// <summary>
        /// 昇降手動運転速度
        /// </summary>
        public int stsUDSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsUDSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsUDSpeed = value;
                }
            }
        }
		
	    /// <summary>
        /// 昇降位置決め位置
	    /// </summary>
        public int stsUDIndexPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsUDIndexPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsUDIndexPos = value;
                }
            }
        }
	    
        /// <summary>
        /// 回転手動運転速度
        /// </summary>
        public int stsRotSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsRotSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsRotSpeed = value;
                }
            }
        }
	    
        /// <summary>
        /// 回転位置決め位置
        /// </summary>
        public int stsRotIndexPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsRotIndexPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsRotIndexPos = value;
                }
            }
        }
	    
        /// <summary>
        /// 微調X手動運転速度
        /// </summary>
        public int stsXStgSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsXStgSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXStgSpeed = value;
                }
            }
        }
	    
        /// <summary>
        /// 微調X位置決め位置
        /// </summary>
        public int stsXStgIndexPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsXStgIndexPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXStgIndexPos = value;
                }
            }
        }
	    
        /// <summary>
        /// 微調Y手動運転速度
        /// </summary>
        public int stsYStgSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsYStgSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsYStgSpeed = value;
                }
            }
        }
	    
        /// <summary>
        /// 微調Y位置決め位置
        /// </summary>
        public int stsYStgIndexPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsYStgIndexPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsYStgIndexPos = value;
                }
            }
        }
	    
        /// <summary>
        /// ﾃｰﾌﾞﾙY軸現在位置
        /// </summary>
        public int stsYPosition
        {
            get
            {
                lock (gLock)
                {
                    return _stsYPosition;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsYPosition = value;
                }
            }
        }
	    
        /// <summary>
        /// X線管FCD
        /// </summary>
        public int stsXrayFCD
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayFCD;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayFCD = value;
                }
            }
        }
	    
        /// <summary>
        /// X線管X軸最低速度
        /// </summary>
        public int stsXrayXMinSp
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayXMinSp;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayXMinSp = value;
                }
            }
        }
	    
        /// <summary>
        /// X線管X軸最高速度
        /// </summary>
        public int stsXrayXMaxSp
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayXMaxSp;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayXMaxSp = value;
                }
            }
        }
	    
        /// <summary>
        /// X線管X軸運転速度
        /// </summary>
        public int stsXrayXSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayXSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayXSpeed = value;
                }
            }
        }
	    
        /// <summary>
        /// X線管X軸現在位置
        /// </summary>
        public int stsXrayXPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayXPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayXPos = value;
                }
            }
        }
	    
        /// <summary>
        /// X線管Y軸最低速度
        /// </summary>
        public int stsXrayYMinSp
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayYMinSp;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayYMinSp = value;
                }
            }
        }
	    
        /// <summary>
        /// X線管Y軸最高速度
        /// </summary>
        public int stsXrayYMaxSp
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayYMaxSp;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayYMaxSp = value;
                }
            }
        }
	    
        /// <summary>
        /// X線管Y軸運転速度
        /// </summary>
        public int stsXrayYSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayYSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayYSpeed = value;
                }
            }
        }
	    
        /// <summary>
        /// X線管Y軸現在位置
        /// </summary>
        public int stsXrayYPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayYPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayYPos = value;
                }
            }
        }
	    
        /// <summary>
        /// X線管回転最低速度
        /// </summary>
        public int stsXrayRotMinSp
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayRotMinSp;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayRotMinSp = value;
                }
            }
        }
	    
        /// <summary>
        /// X線管回転最高速度
        /// </summary>
        public int stsXrayRotMaxSp
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayRotMaxSp;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayRotMaxSp = value;
                }
            }
        }
	    
        /// <summary>
        /// X線管回転運転速度
        /// </summary>
        public int stsXrayRotSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayRotSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayRotSpeed = value;
                }
            }
        }
	    
        /// <summary>
        /// X線管回転現在位置
        /// </summary>
        public int stsXrayRotPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayRotPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayRotPos = value;
                }
            }
        }
	    
        /// <summary>
        /// X線管回転加速度
        /// </summary>
        public int stsXrayRotAccel
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayRotAccel;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayRotAccel = value;
                }
            }
        }
	    
        /// <summary>
        /// X線EXM最大出力値
        /// </summary>
        public int stsEXMMaxW
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMMaxW;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMMaxW = value;
                }
            }
        }
	    
        /// <summary>
        /// X線EXM最大管電圧値
        /// </summary>
        public int stsEXMMaxTV
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMMaxTV;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMMaxTV = value;
                }
            }
        }
	    
        /// <summary>
        /// X線EXM最小管電圧値
        /// </summary>
        public int stsEXMMinTV
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMMinTV;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMMinTV = value;
                }
            }
        }
	    
        /// <summary>
        /// X線EXM最大管電流値
        /// </summary>
        public int stsEXMMaxTC
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMMaxTC;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMMaxTC = value;
                }
            }
        }
		
	    /// <summary>
        /// X線EXM最小管電流値
	    /// </summary>
        public int stsEXMMinTC
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMMinTC;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMMinTC = value;
                }
            }
        }
	    
        /// <summary>
        /// X線EXM制限管電圧値
        /// </summary>
        public int stsEXMLimitTV
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMLimitTV;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMLimitTV = value;
                }
            }
        }
	    
        /// <summary>
        /// X線EXM制限管電流値
        /// </summary>
        public int stsEXMLimitTC
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMLimitTC;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMLimitTC = value;
                }
            }
        }
	    
        /// <summary>
        /// X線EXM管電圧実測値
        /// </summary>
        public int stsEXMTV
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMTV;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMTV = value;
                }
            }
        }
	    
        /// <summary>
        /// X線EXM管電流実測値
        /// </summary>
        public int stsEXMTC
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMTC;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMTC = value;
                }
            }
        }
		
	    /// <summary>
        /// X線EXM管電圧設定値
	    /// </summary>
        public int stsEXMTVSet
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMTVSet;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMTVSet = value;
                }
            }
        }
		
	    /// <summary>
        /// X線EXM管電流設定値
	    /// </summary>
        public int stsEXMTCSet
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMTCSet;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMTCSet = value;
                }
            }
        }
		
        /// <summary>
        /// X線EXMｴﾗｰｺｰﾄﾞ
        /// </summary>
        public int stsEXMErrCode
        {
            get
            {
                lock (gLock)
                {
                    return _stsEXMErrCode;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsEXMErrCode = value;
                }
            }
        }

	    /// <summary>
        /// シーケンサがレディ状態になったか　v11.5追加 by 間々田 2006/07/12
	    /// </summary>
        public bool IsReady
        {
            get
            {
                lock (gLock)
                {
                    return _IsReady;
                }
            }
            set
            {
                lock (gLock)
                {
                    _IsReady = value;
                }
            }
        }

        /// <summary>
        /// 回転大ﾃｰﾌﾞﾙ有無    '追加 by 稲葉 14-02-26    //2014/10/06_v1951反映
        /// </summary>
        public bool stsRotLargeTable
        {
            get
            {
                lock (gLock)
                {
                    return _stsRotLargeTable;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsRotLargeTable = value;
                }
            }
        }

        /// <summary>
        /// 検査室入室安全ｽｲｯﾁ '追加 by 稲葉 14-03-05  //2014/10/06_v1951反映
        /// </summary>
        public bool stsRoomInSw
        {
            get
            {
                lock (gLock)
                {
                    return _stsRoomInSw;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsRoomInSw = value;
                }
            }
        }

        /// <summary>
        /// 冷蔵箱正規位置 追加 by 稲葉 15-03-12
        /// </summary>
        public bool stsColdBoxPosOK
        {
            get
            {
                lock (gLock)
                {
                    return _stsColdBoxPosOK;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColdBoxPosOK = value;
                }
            }
        }

        /// <summary>
        /// 冷蔵箱扉閉 追加 by 稲葉 15-03-12
        /// </summary>
        public bool stsColdBoxDoorClose
        {
            get
            {
                lock (gLock)
                {
                    return _stsColdBoxDoorClose;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColdBoxDoorClose = value;
                }
            }
        }

        /// <summary>
        /// ﾁﾙﾄﾃｰﾌﾞﾙ　ﾁﾙﾄ原点復帰 追加 by 稲葉 15-07-24
        /// </summary>
        public bool TiltOrigin
        {
            get
            {
                lock (gLock)
                {
                    return _TiltOrigin;
                }
            }
            set
            {
                lock (gLock)
                {
                    _TiltOrigin = value;
                }
            }
        }

        /// <summary>
        /// ﾁﾙﾄﾃｰﾌﾞﾙ　回転原点復帰 追加 by 稲葉 15-07-24
        /// </summary>
        public bool TiltRotOrigin
        {
            get
            {
                lock (gLock)
                {
                    return _TiltRotOrigin;
                }
            }
            set
            {
                lock (gLock)
                {
                    _TiltRotOrigin = value;
                }
            }
        }

        /// <summary>
        /// ﾏｲｸﾛﾌｫｰｶｽX線検出器位置 追加 by 稲葉 15-07-24
        /// </summary>
        public bool stsMicroFPDPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsMicroFPDPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsMicroFPDPos = value;
                }
            }
        }

        /// <summary>
        /// ﾅﾉﾌｫｰｶｽX線検出器位置 追加 by 稲葉 15-07-24
        /// </summary>
        public bool stsNanoFPDPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsNanoFPDPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsNanoFPDPos = value;
                }
            }
        }

        /// <summary>
        /// ﾏｲｸﾛﾌｫｰｶｽX線検出器ｼﾌﾄ位置 追加 by 稲葉 15-07-24
        /// </summary>
        public bool stsMicroFPDShiftPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsMicroFPDShiftPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsMicroFPDShiftPos = value;
                }
            }
        }

        /// <summary>
        /// ﾅﾉﾌｫｰｶｽX線検出器ｼﾌﾄ位置 追加 by 稲葉 15-07-24
        /// </summary>
        public bool stsNanoFPDShiftPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsNanoFPDShiftPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsNanoFPDShiftPos = value;
                }
            }
        }

        /// <summary>
        /// ﾏｲｸﾛﾌｫｰｶｽX線切替中 追加 by 稲葉 15-07-24
        /// </summary>
        public bool stsMicroFPDBusy
        {
            get
            {
                lock (gLock)
                {
                    return _stsMicroFPDBusy;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsMicroFPDBusy = value;
                }
            }
        }

        /// <summary>
        /// ﾅﾉﾌｫｰｶｽX線切替中 追加 by 稲葉 15-07-24
        /// </summary>
        public bool stsNanoFPDBusy
        {
            get
            {
                lock (gLock)
                {
                    return _stsNanoFPDBusy;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsNanoFPDBusy = value;
                }
            }
        }

        /// <summary>
        /// ﾏｲｸﾛﾌｫｰｶｽX線検出器ｼﾌﾄ中 追加 by 稲葉 15-07-24
        /// </summary>
        public bool stsMicroFPDShiftBusy
        {
            get
            {
                lock (gLock)
                {
                    return _stsMicroFPDShiftBusy;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsMicroFPDShiftBusy = value;
                }
            }
        }

        /// <summary>
        /// ﾅﾉﾌｫｰｶｽX線検出器ｼﾌﾄ中 追加 by 稲葉 15-07-24
        /// </summary>
        public bool stsNanoFPDShiftBusy
        {
            get
            {
                lock (gLock)
                {
                    return _stsNanoFPDShiftBusy;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsNanoFPDShiftBusy = value;
                }
            }
        }

        /// <summary>
        /// FCD軸ｲﾝﾀｰﾛｯｸﾘﾐｯﾄ位置補正 追加 by 稲葉 15-07-24
        /// </summary>
        public int stsFCDLimitAdj
        {
            get
            {
                lock (gLock)
                {
                    return _stsFCDLimitAdj;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFCDLimitAdj = value;
                }
            }
        }

        /// <summary>
        /// FDDﾘﾆｱｽｹｰﾙ値 追加 by 稲葉 15-07-24
        /// </summary>
        public int stsLinearFDD
        {
            get
            {
                lock (gLock)
                {
                    return _stsLinearFDD;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsLinearFDD = value;
                }
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙY軸ﾘﾆｱｽｹｰﾙ値 追加 by 稲葉 15-07-24
        /// </summary>
        public int stsLinearTableY
        {
            get
            {
                lock (gLock)
                {
                    return _stsLinearTableY;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsLinearTableY = value;
                }
            }
        }

        /// <summary>
        /// FCDﾘﾆｱｽｹｰﾙ値 追加 by 稲葉 15-07-24
        /// </summary>
        public int stsLinearFCD
        {
            get
            {
                lock (gLock)
                {
                    return _stsLinearFCD;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsLinearFCD = value;
                }
            }
        }

        /// <summary>
        /// ﾁﾙﾄﾃｰﾌﾞﾙ　ﾁﾙﾄ手動速度 追加 by 稲葉 15-07-24
        /// </summary>
        public int stsTiltSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsTiltSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTiltSpeed = value;
                }
            }
        }

        /// <summary>
        /// ﾁﾙﾄﾃｰﾌﾞﾙ　回転手動速度 追加 by 稲葉 15-07-24
        /// </summary>
        public int stsTiltRotSpeed
        {
            get
            {
                lock (gLock)
                {
                    return _stsTiltRotSpeed;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTiltRotSpeed = value;
                }
            }
        }

        /// <summary>
        /// 空調機水ｵｰﾊﾞｰﾌﾛｰ 追加 by 稲葉 15-12-14
        /// </summary>
        public bool stsAirConOverFlow
        {
            get
            {
                lock (gLock)
                {
                    return _stsAirConOverFlow;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsAirConOverFlow = value;
                }
            }
        }

        /// <summary>
        /// FPD左ｼﾌﾄ位置 追加 by 稲葉 15-12-14
        /// </summary>
        public bool stsFPDLShiftPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsFPDLShiftPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFPDLShiftPos = value;
                }
            }
        }

        /// <summary>
        /// FPD左ｼﾌﾄ位置移動中 追加 by 稲葉 15-12-14
        /// </summary>
        public bool stsFPDLShiftBusy
        {
            get
            {
                lock (gLock)
                {
                    return _stsFPDLShiftBusy;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFPDLShiftBusy = value;
                }
            }
        }

        /// <summary>
        /// FDシステム位置 追加 by 稲葉 15-12-14
        /// </summary>
        public bool stsFDSystemPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsFDSystemPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFDSystemPos = value;
                }
            }
        }

        /// <summary>
        /// FDシステム位置移動中 追加 by 稲葉 15-12-14
        /// </summary>
        public bool stsFDSystemBusy
        {
            get
            {
                lock (gLock)
                {
                    return _stsFDSystemBusy;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsFDSystemBusy = value;
                }
            }
        }

        /// <summary>
        /// AVｼｽﾃﾑ管電圧H X線OFF時刻 年 追加 by 稲葉 16-01-13
        /// </summary>
        public int stsXrayHOffTimeY
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayHOffTimeY;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayHOffTimeY = value;
                }
            }
        }

        /// <summary>
        /// AVｼｽﾃﾑ管電圧H X線OFF時刻 月日 追加 by 稲葉 16-01-13
        /// </summary>
        public int stsXrayHOffTimeMD
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayHOffTimeMD;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayHOffTimeMD = value;
                }
            }
        }

        /// <summary>
        /// AVｼｽﾃﾑ管電圧H X線OFF時刻 時分 追加 by 稲葉 16-01-13
        /// </summary>
        public int stsXrayHOffTimeHM
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayHOffTimeHM;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayHOffTimeHM = value;
                }
            }
        }

        /// <summary>
        /// AVｼｽﾃﾑ管電圧M X線OFF時刻 年 追加 by 稲葉 16-01-13
        /// </summary>
        public int stsXrayMOffTimeY
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayMOffTimeY;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayMOffTimeY = value;
                }
            }
        }

        /// <summary>
        /// AVｼｽﾃﾑ管電圧M X線OFF時刻 月日 追加 by 稲葉 16-01-13
        /// </summary>
        public int stsXrayMOffTimeMD
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayMOffTimeMD;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayMOffTimeMD = value;
                }
            }
        }

        /// <summary>
        /// AVｼｽﾃﾑ管電圧M X線OFF時刻 時分 追加 by 稲葉 16-01-13
        /// </summary>
        public int stsXrayMOffTimeHM
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayMOffTimeHM;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayMOffTimeHM = value;
                }
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙ上昇制限設定値 追加 by 稲葉 16-03-10
        /// </summary>
        public int stsUpLimitPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsUpLimitPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsUpLimitPos = value;
                }
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙ上昇制限解除 追加 by 稲葉 16-03-10
        /// </summary>
        public bool stsUpLimitPermit
        {
            get
            {
                lock (gLock)
                {
                    return _stsUpLimitPermit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsUpLimitPermit = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　ｺﾘﾒｰﾀ上異常　 追加 by 稲葉 16-04-14
        /// </summary>
        public bool stsColiUErr
        {
            get
            {
                lock (gLock)
                {
                    return _stsColiUErr;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColiUErr = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　ｺﾘﾒｰﾀ下異常　 追加 by 稲葉 16-04-14
        /// </summary>
        public bool stsColiDErr
        {
            get
            {
                lock (gLock)
                {
                    return _stsColiDErr;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColiDErr = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　ｺﾘﾒｰﾀ上原点復帰完了　 追加 by 稲葉 16-04-14
        /// </summary>
        public bool stsColiUOriginOK
        {
            get
            {
                lock (gLock)
                {
                    return _stsColiUOriginOK;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColiUOriginOK = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　ｺﾘﾒｰﾀ下原点復帰完了　 追加 by 稲葉 16-04-14
        /// </summary>
        public bool stsColiDOriginOK
        {
            get
            {
                lock (gLock)
                {
                    return _stsColiDOriginOK;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColiDOriginOK = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　ﾜｰｸ反転水平0°位置　 追加 by 稲葉 16-04-14
        /// </summary>
        public bool stsWorkTurningHPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsWorkTurningHPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsWorkTurningHPos = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　ﾜｰｸ反転縦90°位置　 追加 by 稲葉 16-04-14
        /// </summary>
        public bool stsWorkTurningPPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsWorkTurningPPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsWorkTurningPPos = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　ﾜｰｸ反転縦-90°位置　 追加 by 稲葉 16-04-14
        /// </summary>
        public bool stsWorkTurningNPos
        {
            get
            {
                lock (gLock)
                {
                    return _stsWorkTurningNPos;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsWorkTurningNPos = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　手動運転ﾓｰﾄﾞ　 追加 by 稲葉 16-04-14
        /// </summary>
        public bool stsManualOpeMode
        {
            get
            {
                lock (gLock)
                {
                    return _stsManualOpeMode;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsManualOpeMode = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　自動運転ﾓｰﾄﾞ　 追加 by 稲葉 16-04-14
        /// </summary>
        public bool stsAutoOpeMode
        {
            get
            {
                lock (gLock)
                {
                    return _stsAutoOpeMode;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsAutoOpeMode = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　ﾃｨｰﾁﾝｸﾞ運転ﾓｰﾄﾞ　 追加 by 稲葉 16-04-14
        /// </summary>
        public bool stsTeachingOpeMode
        {
            get
            {
                lock (gLock)
                {
                    return _stsTeachingOpeMode;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTeachingOpeMode = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　自動運転中　 追加 by 稲葉 16-04-14
        /// </summary>
        public bool stsAutoOpeBusy
        {
            get
            {
                lock (gLock)
                {
                    return _stsAutoOpeBusy;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsAutoOpeBusy = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　自動運転中　 追加 by 稲葉 16-04-14
        /// </summary>
        public bool stsTeachingOpeBusy
        {
            get
            {
                lock (gLock)
                {
                    return _stsTeachingOpeBusy;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsTeachingOpeBusy = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　ｽｷｬﾝ(ﾃｨ-ﾁﾝｸﾞ)開始要求　 追加 by 稲葉 16-04-14
        /// </summary>
        public bool ScanStartReq
        {
            get
            {
                lock (gLock)
                {
                    return _ScanStartReq;
                }
            }
            set
            {
                lock (gLock)
                {
                    _ScanStartReq = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　ｽｷｬﾝ(ﾃｨ-ﾁﾝｸﾞ)停止要求　 追加 by 稲葉 16-04-14
        /// </summary>
        public bool ScanStopReq
        {
            get
            {
                lock (gLock)
                {
                    return _ScanStopReq;
                }
            }
            set
            {
                lock (gLock)
                {
                    _ScanStopReq = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　時刻設定要求　 追加 by 稲葉 16-04-14
        /// </summary>
        public bool TimeSetReq
        {
            get
            {
                lock (gLock)
                {
                    return _TimeSetReq;
                }
            }
            set
            {
                lock (gLock)
                {
                    _TimeSetReq = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　異常ﾘｾｯﾄ要求　 追加 by 稲葉 16-04-14
        /// </summary>
        public bool ErrResetReq
        {
            get
            {
                lock (gLock)
                {
                    return _ErrResetReq;
                }
            }
            set
            {
                lock (gLock)
                {
                    _ErrResetReq = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　X線ｳｫｰﾑｱｯﾌﾟ要求　 追加 by 稲葉 16-04-14
        /// </summary>
        public bool XrayWarmupReq
        {
            get
            {
                lock (gLock)
                {
                    return _XrayWarmupReq;
                }
            }
            set
            {
                lock (gLock)
                {
                    _XrayWarmupReq = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　ﾜｰｸ反転原点復帰完了　 追加 by 稲葉 16-05-12
        /// </summary>
        public bool stsWorkTurningOriginOK
        {
            get
            {
                lock (gLock)
                {
                    return _stsWorkTurningOriginOK;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsWorkTurningOriginOK = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　CT装置異常　 追加 by 稲葉 16-06-01
        /// </summary>
        public bool stsCTError
        {
            get
            {
                lock (gLock)
                {
                    return _stsCTError;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsCTError = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　試料扉ｴﾘｱｾﾝｻ状態　 追加 by 稲葉 16-06-24
        /// </summary>
        public bool stsAreaSensorDark
        {
            get
            {
                lock (gLock)
                {
                    return _stsAreaSensorDark;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsAreaSensorDark = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　ｺﾘﾒｰﾀ上現在値　 追加 by 稲葉 16-04-14
        /// </summary>
        public int stsColiUPosition
        {
            get
            {
                lock (gLock)
                {
                    return _stsColiUPosition;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColiUPosition = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　ｺﾘﾒｰﾀ下現在値　 追加 by 稲葉 16-04-14
        /// </summary>
        public int stsColiDPosition
        {
            get
            {
                lock (gLock)
                {
                    return _stsColiDPosition;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsColiDPosition = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　電池ｼﾘｱﾙNo.　 追加 by 稲葉 16-04-14
        /// </summary>
        public string stsWorkSerialNo
        {
            get
            {
                lock (gLock)
                {
                    return _stsWorkSerialNo;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsWorkSerialNo = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　PLC現在時刻 年月　 追加 by 稲葉 16-04-14
        /// </summary>
        public int stsPlcYMTime
        {
            get
            {
                lock (gLock)
                {
                    return _stsPlcYMTime;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsPlcYMTime = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　PLC現在時刻 日時　 追加 by 稲葉 16-04-14
        /// </summary>
        public int stsPlcDHTime
        {
            get
            {
                lock (gLock)
                {
                    return _stsPlcDHTime;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsPlcDHTime = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　PLC現在時刻 分秒　 追加 by 稲葉 16-04-14
        /// </summary>
        public int stsPlcNSTime
        {
            get
            {
                lock (gLock)
                {
                    return _stsPlcNSTime;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsPlcNSTime = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　処理開始日時 年月　 追加 by 稲葉 16-04-14
        /// </summary>
        public int stsStartYMTime
        {
            get
            {
                lock (gLock)
                {
                    return _stsStartYMTime;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsStartYMTime = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　処理開始日時 日時　 追加 by 稲葉 16-04-14
        /// </summary>
        public int stsStartDHTime
        {
            get
            {
                lock (gLock)
                {
                    return _stsStartDHTime;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsStartDHTime = value;
                }
            }
        }

        /// <summary>
        /// 電池検査用　処理開始日時 分秒　 追加 by 稲葉 16-04-14
        /// </summary>
        public int stsStartNSTime
        {
            get
            {
                lock (gLock)
                {
                    return _stsStartNSTime;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsStartNSTime = value;
                }
            }
        }

        /// <summary>
        /// FDZ2+高速度カメラ用　X線/ｶﾒﾗ昇降異常　追加 by 稲葉 19-03-04
        /// </summary>
        public bool stsXrayCameraUDError
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayCameraUDError;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayCameraUDError = value;
                }
            }
        }

        /// <summary>
        /// FDZ2+高速度カメラ用　X線/ｶﾒﾗ昇降動作中　追加 by 稲葉 19-03-04
        /// </summary>
        public bool stsXrayCameraUDBusy
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayCameraUDBusy;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayCameraUDBusy = value;
                }
            }
        }

        /// <summary>
        /// FDZ2+高速度カメラ用　X線/ｶﾒﾗ上昇限　追加 by 稲葉 19-03-04
        /// </summary>
        public bool stsXrayCameraUpperLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayCameraUpperLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayCameraUpperLimit = value;
                }
            }
        }

        /// <summary>
        /// FDZ2+高速度カメラ用　X線/ｶﾒﾗ下降限　追加 by 稲葉 19-03-04
        /// </summary>
        public bool stsXrayCameraLowerLimit
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayCameraLowerLimit;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayCameraLowerLimit = value;
                }
            }
        }

        /// <summary>
        /// FDZ2+高速度カメラ用　X線/ｶﾒﾗ昇降現在位置読出し　追加 by 稲葉 19-03-04
        /// </summary>
        public int stsXrayCameraUDPosition
        {
            get
            {
                lock (gLock)
                {
                    return _stsXrayCameraUDPosition;
                }
            }
            set
            {
                lock (gLock)
                {
                    _stsXrayCameraUDPosition = value;
                }
            }
        }

        #endregion

        #region StatusRead
        /// <summary>
        /// ｼｰｹﾝｻからのﾃﾞｰﾀの読み出し
        /// </summary>
        /// <returns></returns>
	    public int StatusRead()
        {
            int functionReturnValue = 0;
            bool _CommBusyFlg = false;
            
            try
            {
                lock (gLock)
                {
                    //Debug.Print("stsCommBusy1 " + stsCommBusy.ToString());

                    if (stsCommBusy || (StatusReadNo != 0))
                    {
                        //通信待ち
                        StatusReadNo = 1;
                    }
                    else
                    {
                        stsCommBusy = true; //通信中 追加 by 稲葉 02-03-05
                        _CommBusyFlg = true;
                    }
                }
                if (_CommBusyFlg == true) functionReturnValue = StatusReadExe();
            }
            catch
            {
                lock (gLock)
                {
                    //通信ﾎﾟｰﾄｴﾗｰ
                    functionReturnValue = 710;
                    stsCommBusy = false;
                }
            }           

            return functionReturnValue;
        }
        #endregion

        #region StatusReadExe
        /// <summary>
        /// ｼｰｹﾝｻからのﾃﾞｰﾀの読み出し（ﾌﾟﾛﾄｺﾙﾓｰﾄﾞ4）
        /// </summary>
        /// <returns></returns>
	    public int StatusReadExe()
        {
            int functionReturnValue = 0;
            string ReadCommand = null;  //読み出しｺﾏﾝﾄﾞ

            try
            {
                functionReturnValue = CommInit();

                lock (gLock)
                {
                    if (functionReturnValue == 0)
                    {

                        //Debug.Print("StatusReadExe");

                        //全ﾃﾞｰﾀ受信で通信ｲﾍﾞﾝﾄを発生させる。   追加 by 稲葉 02-02-26
                        //Thread.Sleep(0);

                        //エラーするのでThresholdサイズを変えない　//2014/04/23(検S1)hata
                        //mySerialPort.ReceivedBytesThreshold = BitReadNo + 1 + 8;

                        //ﾋﾞｯﾄ読み出しｺﾏﾝﾄﾞを送信します。
                        ReadFlag = 1;

                        // ﾜｰﾄﾞ数(Hex) 
                        string _WordNo = Convert.ToString(BitReadNo + 1, 16).PadLeft(2, '0');

                        //読み出しｺﾏﾝﾄﾞ = ENQ + 局番 + PC番号 + ｺﾏﾝﾄﾞ + ｳｴｲﾄ + 先頭ﾃﾞﾊﾞｲｽ + ﾜｰﾄﾞ数(Hex) + CR + LF
#if conKvMode1
                        ReadCommand = Convert.ToChar(0x5) + "00" + "FF" + "JR" + "0" + "X001F40" + _WordNo + "\r\n";
#elif conKvMode2 
			            ReadCommand = Convert.ToChar(0x5) + "00" + "FF" + "JR" + "0" + "M007000" + _WordNo + "\r\n";
#else
                        ReadCommand = Convert.ToChar(0x5) + "00" + "FF" + "JR" + "0" + "B000000" + _WordNo + "\r\n";
#endif
                        //Log用　2015/03/02hata
                        //strSendCommamd = ReadCommand; //変更2015/03/04_hata 
                        strSendCommamd = "StatusReadExe";

                        //追加2015/03/16hata
                        // CTSﾗｲﾝｴﾗｰﾁｪｯｸ
                        DateTime ntim = DateTime.Now;
                        while (!mySerialPort.CtsHolding)
                        {
                            if (((DateTime.Now - ntim).TotalSeconds > 2000))
                            {
                                //Logを取る
                                //LogWrite("CtsHolding=false");
                                break;
                            }
                            Thread.Sleep(1);
                        }

                        mySerialPort.Write(ReadCommand);

                        // 通信ﾀｲﾑｱｳﾄ時間を設定します。
                        TimeOutSet();
                        
                    }
                }
            }
            catch
            { 
                //通信ﾎﾟｰﾄｴﾗｰ
                int _CommEndAns;
                bool _RaiseEvent;

                lock (gLock)
                {
                    SeqCommEvent = 710;
                    functionReturnValue = SeqCommEvent;
                    _CommEndAns = SeqCommEvent;
                    _RaiseEvent = EventProcess(SeqCommEvent);
                }
                if (_RaiseEvent == true && OnCommEnd != null) OnCommEnd(_CommEndAns);
            }

            return functionReturnValue;
        }
        #endregion

        #region BitWrite
        /// <summary>
        /// ｼｰｹﾝｻへのﾋﾞｯﾄﾃﾞｰﾀの書込み
        /// </summary>
        /// <param name="DeviceName"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
	    public int BitWrite(string DeviceName, bool Data)
	    {
		    int functionReturnValue = 0;
            bool _CommBusyFlg = false;

            try
            {
                lock (gLock)
                {
                    if (stsCommBusy || (BitWriteNo != 0))
                    {
                        if (BitWriteNo < BufferSize)
                        {
                            //追加 by 稲葉 03-11-01
                            if ((BitWriteName[BitWriteNo] != DeviceName || BitWriteData[BitWriteNo] != Data) || BitWriteNo == 0)
                            {
                                //通信待ち
                                BitWriteNo++;
                                BitWriteName[BitWriteNo] = DeviceName;
                                BitWriteData[BitWriteNo] = Data;
                                BitWordWriteNo++;   //追加 by 稲葉 02-09-19
                                BitWordType[BitWordWriteNo] = "Bit";    //追加 by 稲葉 02-09-19

                                //Debug.Print("ﾋﾞｯﾄﾃﾞｰﾀ書込時BitWriteNo = " + BitWriteNo.ToString());    //追加 by 稲葉 02-03-05
                                //Debug.Print("ﾋﾞｯﾄﾃﾞｰﾀ書込時BitWriteName = " + DeviceName);     //追加 by 稲葉 02-03-05
                                //Debug.Print("ﾋﾞｯﾄﾃﾞｰﾀ書込時BitWriteData = " + Data.ToString());        //追加 by 稲葉 02-03-05
                                //Debug.Print("ﾋﾞｯﾄﾃﾞｰﾀ書込時BitWordWriteNo = " + BitWordWriteNo);       //追加 by 稲葉 02-09-19
                                //Debug.Print("ﾋﾞｯﾄﾃﾞｰﾀ書込時BitWordType(BitWordWriteNo) = " + BitWordType[BitWordWriteNo]); //追加 by 稲葉 02-09-19
                            }
                        }
                        else
                        {
                            functionReturnValue = 730;  //追加 by 稲葉 03-11-01　ﾋﾞｯﾄﾃﾞｰﾀ書込みのﾊﾞｯﾌｧｵｰﾊﾞｰﾌﾛｰ
                        }
                    }
                    else
                    {
                        stsCommBusy = true;     //通信中 追加 by 稲葉 02-03-05
                        _CommBusyFlg = true;
                    }
                }
                if (_CommBusyFlg == true) functionReturnValue = BitWriteExe(DeviceName, Data);
            }
            catch 
            {
		        //通信ﾎﾟｰﾄｴﾗｰ
		        functionReturnValue = 715;
            }

            return functionReturnValue;
        }
        #endregion

        #region WordWrite
        /// <summary>
        /// ｼｰｹﾝｻへのﾜｰﾄﾞﾃﾞｰﾀの書込み
        /// </summary>
        /// <param name="DeviceName"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
	    public int WordWrite(string DeviceName, ref string Data)
	    {
		    int functionReturnValue = 0;
            bool _CommBusyFlg = false;

            try
            {
                lock (gLock)
                {
                    if (stsCommBusy || (WordWriteNo != 0))
                    {
                        if (WordWriteNo < BufferSize)
                        {
                            //追加 by 稲葉 03-11-01
                            if ((WordWriteName[WordWriteNo] != DeviceName || WordWriteData[WordWriteNo] != Data) || WordWriteNo == 0)
                            {
                                //通信待ち
                                WordWriteNo++;
                                WordWriteName[WordWriteNo] = DeviceName;
                                WordWriteData[WordWriteNo] = Data;
                                BitWordWriteNo++;   //追加 by 稲葉 02-09-19
                                BitWordType[BitWordWriteNo] = "Word";   //追加 by 稲葉 02-09-19
                                
                                //Debug.Print("ﾜｰﾄﾞﾃﾞｰﾀ書込時BitWordWriteNo = " + BitWordWriteNo);   //追加 by 稲葉 02-09-19
                                //Debug.Print("ﾜｰﾄﾞﾃﾞｰﾀ書込時BitWordType(BitWordWriteNo) = " + BitWordType[BitWordWriteNo]); //追加 by 稲葉 02-09-19
                            }
                        }
                        else
                        {
                            functionReturnValue = 730;  //追加 by 稲葉 03-11-01　ﾜｰﾄﾞﾃﾞｰﾀ書込みのﾊﾞｯﾌｧｵｰﾊﾞｰﾌﾛｰ
                        }
                    }
                    else
                    {
                        stsCommBusy = true;     //通信中 追加 by 稲葉 02-03-05
                        _CommBusyFlg = true;
                    }
                }
                if (_CommBusyFlg == true) functionReturnValue = WordWriteExe(DeviceName, ref Data);
            }
            catch 
            {
                //通信ﾎﾟｰﾄｴﾗｰ
                functionReturnValue = 720;
            }

            return functionReturnValue;
	    }
        #endregion

        #region BitWriteExe
        /// <summary>
        /// ｼｰｹﾝｻへのﾋﾞｯﾄﾃﾞｰﾀの書込み（ﾌﾟﾛﾄｺﾙﾓｰﾄﾞ4）
        /// </summary>
        /// <param name="DeviceName"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
	    private int BitWriteExe(string DeviceName, bool Data)
	    {
		    int functionReturnValue = 0;
		    string BitWriteCommand = null;  //ﾋﾞｯﾄ書込みｺﾏﾝﾄﾞ
		    string DeviceNo = null; //ﾃﾞﾊﾞｲｽ番号
		    string BitData = null;  //ﾋﾞｯﾄﾃﾞｰﾀ

            bool _DefaultDevice = false;

            try
            {
                functionReturnValue = CommInit();

                if (functionReturnValue != 0)
                {
                    return functionReturnValue;
                }
                else
                {
                    lock (gLock)
                    {
                        switch (DeviceName)
                        {
#if conKvMode1                                          //ｷｰｴﾝｽｼｰｹﾝｻKV用
                            case "CommCheck":
                                DeviceNo = "X002080";
                                break;
                            case "WordWrite":
                                DeviceNo = "X002081";
                                break;
                            case "PanelInhibit":
                                DeviceNo = "X002082";
                                break;
                            case "stsScanBusy":        //追加  by 稲葉 10-11-22
                                DeviceNo = "X002083";
                                break;
                            case "ScanPCErr":  //AVFD用　追加  by 稲葉 16-01-13
                                DeviceNo = "X002084";
                                break;
                            case "ErrOff":
                                DeviceNo = "X002085";
                                break;
                            case "RotXChangeReset":
                                DeviceNo = "X002086";
                                break;
                            case "DisXChangeReset":
                                DeviceNo = "X002087";
                                break;
                            case "RotYChangeReset":
                                DeviceNo = "X002088";
                                break;
                            case "DisYChangeReset":
                                DeviceNo = "X002089";
                                break;
                            case "VerIIChangeReset":
                                DeviceNo = "X00208A";
                                break;
                            case "RotIIChangeReset":
                                DeviceNo = "X00208B";
                                break;
                            case "GainIIChangeReset":
                                DeviceNo = "X00208C";
                                break;
                            case "DisIIChangeReset":
                                DeviceNo = "X00208D";
                                break;
                            case "SPIIChangeReset":     //追加  by 稲葉 05-10-21
                                DeviceNo = "X00208E";
                                break;
                            case "XLeft":
                                DeviceNo = "X002090";
                                break;
                            case "XRight":
                                DeviceNo = "X002091";
                                break;
                            case "XIndex":
                                DeviceNo = "X002092";
                                break;
                            case "XIndexStop":
                                DeviceNo = "X002093";
                                break;
                            case "XOrigin":
                                DeviceNo = "X002094";
                                break;
                            case "YForward":
                                DeviceNo = "X002095";
                                break;
                            case "YBackward":
                                DeviceNo = "X002096";
                                break;
                            case "YIndex":
                                DeviceNo = "X002097";
                                break;
                            case "YIndexStop":
                                DeviceNo = "X002098";
                                break;
                            case "YOrigin":
                                DeviceNo = "X002099";
                                break;
                            case "IIForward":
                                DeviceNo = "X00209A";
                                break;
                            case "IIBackward":
                                DeviceNo = "X00209B";
                                break;
                            case "IIindex":
                                DeviceNo = "X00209C";
                                break;
                            case "IIindexStop":
                                DeviceNo = "X00209D";
                                break;
                            case "XYIndex":     //追加  by 稲葉 09-06-24
                                DeviceNo = "X00209E";
                                break;
                            case "IIOrigin":    //追加  by 稲葉 10-08-24
                                DeviceNo = "X00209F";
                                break;
                            case "TiltCw":
                                DeviceNo = "X0020A0";
                                break;
                            case "TiltCcw":
                                DeviceNo = "X0020A1";
                                break;
                            case "TiltOrigin":
                                DeviceNo = "X0020A2";
                                break;
                            case "ColliLOpen":
                                DeviceNo = "X0020A5";
                                break;
                            case "ColliLClose":
                                DeviceNo = "X0020A6";
                                break;
                            case "ColliROpen":
                                DeviceNo = "X0020A7";
                                break;
                            case "ColliRClose":
                                DeviceNo = "X0020A8";
                                break;
                            case "ColliUOpen":
                                DeviceNo = "X0020A9";
                                break;
                            case "ColliUClose":
                                DeviceNo = "X0020AA";
                                break;
                            case "ColliDOpen":
                                DeviceNo = "X0020AB";
                                break;
                            case "ColliDClose":
                                DeviceNo = "X0020AC";
                                break;
                            case "Shutter":      //'追加  by 稲葉 10-11-22
                                DeviceNo = "X0020AF";
                                break;
                            case "Filter0":
                                DeviceNo = "X0020B0";
                                break;
                            case "Filter1":
                                DeviceNo = "X0020B1";
                                break;
                            case "Filter2":
                                DeviceNo = "X0020B2";
                                break;
                            case "Filter3":
                                DeviceNo = "X0020B3";
                                break;
                            case "Filter4":
                                DeviceNo = "X0020B4";
                                break;
                            case "Filter5":
                                DeviceNo = "X0020B5";
                                break;
                            case "stsWarmUpOn":     //追加  by 稲葉 10-02-03
                                DeviceNo = "X0020B7";
                                break;
                            case "stsXrayOn":
                                DeviceNo = "X0020B8";
                                break;
                            case "XrayInhibit":
                                DeviceNo = "X0020B9";
                                break;
                            case "Xray225":
                                DeviceNo = "X0020BA";
                                break;
                            case "Xray160":
                                DeviceNo = "X0020BB";
                                break;
                            case "II":
                                DeviceNo = "X0020BC";
                                break;
                            case "FPD":
                                DeviceNo = "X0020BD";
                                break;
                            case "XTableMove":
                                DeviceNo = "X0020BE";
                                break;
                            case "TubeMove":
                                DeviceNo = "X0020BF";
                                break;
                            case "II9":
                                DeviceNo = "X0020C0";
                                break;
                            case "II6":
                                DeviceNo = "X0020C1";
                                break;
                            case "II4":
                                DeviceNo = "X0020C2";
                                break;
                            case "IIPowerOn":
                                DeviceNo = "X0020C3";
                                break;
                            case "IIPowerOff":
                                DeviceNo = "X0020C4";
                                break;
                            case "SLightOn":
                                DeviceNo = "X0020C5";
                                break;
                            case "SLightOff":
                                DeviceNo = "X0020C6";
                                break;
                            case "TableMovePermit":
                                DeviceNo = "X0020C7";
                                break;
                            case "TableMoveRestrict":
                                DeviceNo = "X0020C8";
                                break;
                            case "stsUDErr":
                                DeviceNo = "X0020CA";
                                break;
                            case "stsUDLimit":
                                DeviceNo = "X0020CB";
                                break;
                            case "stsUDBusy":
                                DeviceNo = "X0020CC";
                                break;
                            case "sts1stPcErr":          //追加  by 稲葉 13-04-12
                                DeviceNo = "X0020CE";
                                break;
                            case "stsXrayErr":           //追加  by 稲葉 13-04-12
                                DeviceNo = "X0020CF";
                                break;
                            case "stsRotErr":
                                DeviceNo = "X0020D0";
                                break;
                            case "stsRotBusy":
                                DeviceNo = "X0020D1";
                                break;
                            case "stsXStgErr":
                                DeviceNo = "X0020D2";
                                break;
                            case "stsTiltErr":          //追加  by 稲葉 15-07-24
                                DeviceNo = "X0020D3";
                                break;
                            case "stsYStgErr":
                                DeviceNo = "X0020D4";
                                break;
                            case "stsTiltRotErr":       //追加  by 稲葉 15-07-24
                                DeviceNo = "X0020D5";
                                break;
                            case "stsPhmErr":
                                DeviceNo = "X0020D6";
                                break;
                            case "stsPhmLimit":
                                DeviceNo = "X0020D7";
                                break;
                            case "stsPhmBusy":
                                DeviceNo = "X0020D8";
                                break;
                            case "stsPhmOnOff":
                                DeviceNo = "X0020D9";
                                break;
                            case "RotOriginOK":
                                DeviceNo = "X0020DA";
                                break;
                            case "TrgReq":
                                DeviceNo = "X0020DB";
                                break;
                            case "FXTableOn":
                                DeviceNo = "X0020DC";
                                break;
                            case "FXTableOff":
                                DeviceNo = "X0020DD";
                                break;
                            case "FYTableOn":
                                DeviceNo = "X0020DE";
                                break;
                            case "FYTableOff":
                                DeviceNo = "X0020DF";
                                break;
                            case "XrayXLeft":
                                DeviceNo = "X0020E0";
                                break;
                            case "XrayXRight":
                                DeviceNo = "X0020E1";
                                break;
                            case "XrayXOrg":
                                DeviceNo = "X0020E2";
                                break;
                            case "XrayXIndex":
                                DeviceNo = "X0020E3";
                                break;
                            case "XrayXStop":
                                DeviceNo = "X0020E4";
                                break;
                            case "RotXrayXChReset":
                                DeviceNo = "X0020E5";
                                break;
                            case "DisXrayXChReset":
                                DeviceNo = "X0020E6";
                                break;
                            case "XrayYForward":
                                DeviceNo = "X0020EB";
                                break;
                            case "XrayYBackward":
                                DeviceNo = "X0020EC";
                                break;
                            case "XrayYOrg":
                                DeviceNo = "X0020ED";
                                break;
                            case "XrayYIndex":
                                DeviceNo = "X0020EE";
                                break;
                            case "XrayYStop":
                                DeviceNo = "X0020EF";
                                break;
                            case "RotXrayYChReset":
                                DeviceNo = "X0020F0";
                                break;
                            case "DisXrayYChReset":
                                DeviceNo = "X0020F1";
                                break;
                            case "DoorLockOn":
                                DeviceNo = "X0020F3";
                                break;
                            case "DoorLockOff":
                                DeviceNo = "X0020F4";
                                break;
                            case "EXMRemote":
                                DeviceNo = "X0020F5";
                                break;
                            case "EXMLocal":
                                DeviceNo = "X0020F6";
                                break;
                            case "EXMOn":
                                DeviceNo = "X0020F7";
                                break;
                            case "EXMOff":
                                DeviceNo = "X0020F8";
                                break;
                            case "EXMReset":
                                DeviceNo = "X0020F9";
                                break;
                            case "EXMWUShort":
                                DeviceNo = "X0020FA";
                                break;
                            case "EXMWULong":
                                DeviceNo = "X0020FB";
                                break;
                            case "EXMWUNone":
                                DeviceNo = "X0020FC";
                                break;
                            case "MechaReset":    //追加  by 稲葉 10-09-10
                                DeviceNo = "X0020FE";
                                break;
                            case "MechaResetStop":  //追加  by 稲葉 10-09-10
                                DeviceNo = "X0020FF";
                                break;
                            case "RotXChangeSet":
                                DeviceNo = "X002100";
                                break;
                            case "DisXChangeSet":
                                DeviceNo = "X002101";
                                break;
                            case "RotYChangeSet":
                                DeviceNo = "X002102";
                                break;
                            case "DisYChangeSet":
                                DeviceNo = "X002103";
                                break;
                            case "VerIIChangeSet":
                                DeviceNo = "X002104";
                                break;
                            case "RotIIChangeSet":
                                DeviceNo = "X002105";
                                break;
                            case "GainIIChangeSet":
                                DeviceNo = "X002106";
                                break;
                            case "DisIIChangeSet":
                                DeviceNo = "X002107";
                                break;
                            case "SPIIChangeSet":
                                DeviceNo = "X002108";
                                break;
                            //追加 by 稲葉 10-02-03
                            case "CTIISet":
                                DeviceNo = "X00210A";
                                break;
                            case "TVIISet":
                                DeviceNo = "X00210B";
                                break;
                            case "IIChangeReset":
                                DeviceNo = "X00210C";
                                break;
                            case "IIChangeStop":
                                DeviceNo = "X00210D";
                                break;
                            case "TVII9":
                                DeviceNo = "X00210E";
                                break;
                            case "TVII6":
                                DeviceNo = "X00210F";
                                break;
                            case "TVII4":
                                DeviceNo = "X002110";
                                break;
                            case "TVIIPowerOn":
                                DeviceNo = "X002111";
                                break;
                            case "TVIIPowerOff":
                                DeviceNo = "X002112";
                                break;
                            case "CameraPowerOn":
                                DeviceNo = "X002113";
                                break;
                            case "CameraPowerOff":
                                DeviceNo = "X002114";
                                break;
                            case "IrisLOpen":
                                DeviceNo = "X002115";
                                break;
                            case "IrisLClose":
                                DeviceNo = "X002116";
                                break;
                            case "IrisROpen":
                                DeviceNo = "X002117";
                                break;
                            case "IrisRClose":
                                DeviceNo = "X002118";
                                break;
                            case "IrisUOpen":
                                DeviceNo = "X002119";
                                break;
                            case "IrisUClose":
                                DeviceNo = "X00211A";
                                break;
                            case "IrisDOpen":
                                DeviceNo = "X00211B";
                                break;
                            case "IrisDClose":
                                DeviceNo = "X00211C";
                                break;
                            case "CTChangeNO":
                                DeviceNo = "X00211D";
                                break;
                            case "TVChangeNO":
                                DeviceNo = "X00211E";
                                break;
                            case "FPDLShiftSet":   //追加  by 稲葉 15-12-14
                                DeviceNo = "X00211F";
                                break;
                            case "AutoRestrictOn":  //追加  by 稲葉 10-10-19
                                DeviceNo = "X002120";
                                break;
                            case "AutoRestrictOff": //追加  by 稲葉 10-10-19
                                DeviceNo = "X002121";
                                break;
                            case "YIndexSlowOn":    //追加  by 稲葉 10-10-19
                                DeviceNo = "X002122";
                                break;
                            case "YIndexSlowOff":   //追加  by 稲葉 10-10-19
                                DeviceNo = "X002123";
                                break;

                            // 計測CT　//追加  by 稲葉 15-07-24
                            case "MicroFPDSet":          
                                DeviceNo = "X002124";
                                break;
                            case "NanoFPDSet":          
                                DeviceNo = "X002125";
                                break;
                            case "MicroTableYSet":  //追加  by 稲葉 15-10-20
                                DeviceNo = "X002126";
                                break;
                            case "NanoTableYSet":   //追加  by 稲葉 15-10-20
                                DeviceNo = "X002127";
                                break;
                            case "MicroFPDShiftSet":          
                                DeviceNo = "X002128";
                                break;
                            case "NanoFPDShiftSet":          
                                DeviceNo = "X002129";
                                break;
                            case "FPDMoveStop":          
                                DeviceNo = "X00212A";
                                break;
                            case "FPDMoveProhibit":          
                                DeviceNo = "X00212B";
                                break;

                            // AVFD用
                            case "SystemMoveProhibit":  //追加  by 稲葉 15-12-14
                                DeviceNo = "X00212D";
                                break;
                            case "FDSystemSet":         //追加  by 稲葉 15-12-14
                                DeviceNo = "X00212E";
                                break;
                            case "FDSystemStop":        //追加  by 稲葉 15-12-14
                                DeviceNo = "X00212F";
                                break;
                            case "stsXrayHOff":         //追加  by 稲葉 16-01-13
                                DeviceNo = "X002130";
                                break;
                            case "stsXrayMOff":         //追加  by 稲葉 16-01-13
                                DeviceNo = "X002131";
                                break;

                            // 電池検査用    //追加  by 稲葉 16-04-14
                            case "DoorOpen":  
                                DeviceNo = "X002140";
                                break;
                            case "DoorClose":
                                DeviceNo = "X002141";
                                break;
                            case "DoorStop":
                                DeviceNo = "X002142";
                                break;
                            case "ColiOrigin":
                                DeviceNo = "X002143";
                                break;
                            case "ColiIndex":
                                DeviceNo = "X002144";
                                break;
                            case "ColiStop":
                                DeviceNo = "X002145";
                                break;
                            case "WorkTurningHSet":
                                DeviceNo = "X002146";
                                break;
                            case "WorkTurningPSet":
                                DeviceNo = "X002147";
                                break;
                            case "WorkTurningNSet":
                                DeviceNo = "X002148";
                                break;
                            case "WorkTurningStop":
                                DeviceNo = "X002149";
                                break;
                            case "stsScanPCReady":
                                DeviceNo = "X00214A";
                                break;
                            case "stsXrayReady":
                                DeviceNo = "X00214B";
                                break;
                            case "stsXrayWUOK":
                                DeviceNo = "X00214C";
                                break;
                            case "ScanOK":
                                DeviceNo = "X00214D";
                                break;
                            case "TimeSetAck":
                                DeviceNo = "X00214E";
                                break;
                            case "ErrResetAck":
                                DeviceNo = "X00214F";
                                break;
                            case "UdOriginAck":
                                DeviceNo = "X002150";
                                break;
                            case "RotOriginAck":
                                DeviceNo = "X002151";
                                break;
                            case "UdIndexAck":
                                DeviceNo = "X002152";
                                break;
                            case "RotIndexAck":
                                DeviceNo = "X002153";
                                break;
                            case "WorkTurningOrigin":   //追加  by 稲葉 16-05-09
                                DeviceNo = "X002154";
                                break;

                            // ｱｲｼﾝ精機向けFDZ2＋高速度ｶﾒﾗ用 //追加  by 稲葉 19-03-04
                            case "XrayPrewarning":
                                DeviceNo = "X002180";
                                break;
                            case "XrayCameraUDIndex":
                                DeviceNo = "X002181";
                                break;

#elif  (conKvMode2)     //三菱ｼｰｹﾝｻ用追加  by 稲葉 10-05-11
				            case "CommCheck":
					            DeviceNo = "M007400";
					            break;
				            case "WordWrite":
					            DeviceNo = "M007401";
					            break;
				            case "PanelInhibit":
					            DeviceNo = "M007402";
					            break;
				            case "ErrOff":
					            DeviceNo = "M007405";
					            break;
				            case "RotXChangeReset":
					            DeviceNo = "M007406";
					            break;
				            case "DisXChangeReset":
					            DeviceNo = "M007407";
					            break;
				            case "RotYChangeReset":
					            DeviceNo = "M007408";
					            break;
				            case "DisYChangeReset":
					            DeviceNo = "M007409";
					            break;
				            case "VerIIChangeReset":
					            DeviceNo = "M007410";
					            break;
				            case "RotIIChangeReset":
					            DeviceNo = "M007411";
					            break;
				            case "GainIIChangeReset":
					            DeviceNo = "M007412";
					            break;
				            case "DisIIChangeReset":
					            DeviceNo = "M007413";
					            break;
				            case "SPIIChangeReset":
					            DeviceNo = "M007414";
					            break;
				            case "XLeft":
					            DeviceNo = "M007416";
					            break;
				            case "XRight":
					            DeviceNo = "M007417";
					            break;
				            case "XIndex":
					            DeviceNo = "M007418";
					            break;
				            case "XIndexStop":
					            DeviceNo = "M007419";
					            break;
				            case "XOrigin":
					            DeviceNo = "M007420";
					            break;
				            case "YForward":
					            DeviceNo = "M007421";
					            break;
				            case "YBackward":
					            DeviceNo = "M007422";
					            break;
				            case "YIndex":
					            DeviceNo = "M007423";
					            break;
				            case "YIndexStop":
					            DeviceNo = "M007424";
					            break;
				            case "YOrigin":
					            DeviceNo = "M007425";
					            break;
				            case "IIForward":
					            DeviceNo = "M007426";
					            break;
				            case "IIBackward":
					            DeviceNo = "M007427";
					            break;
				            case "IIindex":
					            DeviceNo = "M007428";
					            break;
				            case "IIindexStop":
					            DeviceNo = "M007429";
					            break;
				            case "XYIndex":
					            DeviceNo = "M007430";
					            break;
				            case "IIOrigin":
					            DeviceNo = "M007431";
					            break;
				            case "TiltCw":
					            DeviceNo = "M007432";
					            break;
				            case "TiltCcw":
					            DeviceNo = "M007433";
					            break;
				            case "TiltOrigin":
					            DeviceNo = "M007434";
					            break;
				            case "ColliLOpen":
					            DeviceNo = "M007437";
					            break;
				            case "ColliLClose":
					            DeviceNo = "M007438";
					            break;
				            case "ColliROpen":
					            DeviceNo = "M007439";
					            break;
				            case "ColliRClose":
					            DeviceNo = "M007440";
					            break;
				            case "ColliUOpen":
					            DeviceNo = "M007441";
					            break;
				            case "ColliUClose":
					            DeviceNo = "M007442";
					            break;
				            case "ColliDOpen":
					            DeviceNo = "M007443";
					            break;
				            case "ColliDClose":
					            DeviceNo = "M007444";
					            break;
				            case "Filter0":
					            DeviceNo = "M007448";
					            break;
				            case "Filter1":
					            DeviceNo = "M007449";
					            break;
				            case "Filter2":
					            DeviceNo = "M007450";
					            break;
				            case "Filter3":
					            DeviceNo = "M007451";
					            break;
				            case "Filter4":
					            DeviceNo = "M007452";
					            break;
				            case "Filter5":
					            DeviceNo = "M007453";
					            break;
				            case "stsWarmUpOn":
					            DeviceNo = "M007455";
					            break;
				            case "stsXrayOn":
					            DeviceNo = "M007456";
					            break;
				            case "XrayInhibit":
					            DeviceNo = "M007457";
					            break;
				            case "Xray225":
					            DeviceNo = "M007458";
					            break;
				            case "Xray160":
					            DeviceNo = "M007459";
					            break;
				            case "II":
					            DeviceNo = "M007460";
					            break;
				            case "FPD":
					            DeviceNo = "M007461";
					            break;
				            case "XTableMove":
					            DeviceNo = "M007462";
					            break;
				            case "TubeMove":
					            DeviceNo = "M007463";
					            break;
				            case "II9":
					            DeviceNo = "M007464";
					            break;
				            case "II6":
					            DeviceNo = "M007465";
					            break;
				            case "II4":
					            DeviceNo = "M007466";
					            break;
				            case "IIPowerOn":
					            DeviceNo = "M007467";
					            break;
				            case "IIPowerOff":
					            DeviceNo = "M007468";
					            break;
				            case "SLightOn":
					            DeviceNo = "M007469";
					            break;
				            case "SLightOff":
					            DeviceNo = "M007470";
					            break;
				            case "TableMovePermit":
					            DeviceNo = "M007471";
					            break;
				            case "TableMoveRestrict":
					            DeviceNo = "M007472";
					            break;
				            case "stsUDErr":
					            DeviceNo = "M007474";
					            break;
				            case "stsUDLimit":
					            DeviceNo = "M007475";
					            break;
				            case "stsUDBusy":
					            DeviceNo = "M007476";
					            break;
				            case "stsRotErr":
					            DeviceNo = "M007480";
					            break;
				            case "stsRotBusy":
					            DeviceNo = "M007481";
					            break;
				            case "stsXStgErr":
					            DeviceNo = "M007482";
					            break;
				            case "stsYStgErr":
					            DeviceNo = "M007484";
					            break;
				            case "stsPhmErr":
					            DeviceNo = "M007486";
					            break;
				            case "stsPhmLimit":
					            DeviceNo = "M007487";
					            break;
				            case "stsPhmBusy":
					            DeviceNo = "M007488";
					            break;
				            case "stsPhmOnOff":
					            DeviceNo = "M007489";
					            break;
				            case "RotOriginOK":
					            DeviceNo = "M007490";
					            break;
				            case "TrgReq":
					            DeviceNo = "M007491";
					            break;
				            case "FXTableOn":
					            DeviceNo = "M007492";
					            break;
				            case "FXTableOff":
					            DeviceNo = "M007493";
					            break;
				            case "FYTableOn":
					            DeviceNo = "M007494";
					            break;
				            case "FYTableOff":
					            DeviceNo = "M007495";
					            break;
				            case "XrayXLeft":
					            DeviceNo = "M007496";
					            break;
				            case "XrayXRight":
					            DeviceNo = "M007497";
					            break;
				            case "XrayXOrg":
					            DeviceNo = "M007498";
					            break;
				            case "XrayXIndex":
					            DeviceNo = "M007499";
					            break;
				            case "XrayXStop":
					            DeviceNo = "M007500";
					            break;
				            case "RotXrayXChReset":
					            DeviceNo = "M007501";
					            break;
				            case "DisXrayXChReset":
					            DeviceNo = "M007502";
					            break;
				            case "XrayYForward":
					            DeviceNo = "M007507";
					            break;
				            case "XrayYBackward":
					            DeviceNo = "M007508";
					            break;
				            case "XrayYOrg":
					            DeviceNo = "M007509";
					            break;
				            case "XrayYIndex":
					            DeviceNo = "M007510";
					            break;
				            case "XrayYStop":
					            DeviceNo = "M007511";
					            break;
				            case "RotXrayYChReset":
					            DeviceNo = "M007512";
					            break;
				            case "DisXrayYChReset":
					            DeviceNo = "M007513";
					            break;
				            case "DoorLockOn":
					            DeviceNo = "M007515";
					            break;
				            case "DoorLockOff":
					            DeviceNo = "M007516";
					            break;
				            case "EXMRemote":
					            DeviceNo = "M007517";
					            break;
				            case "EXMLocal":
					            DeviceNo = "M007518";
					            break;
				            case "EXMOn":
					            DeviceNo = "M007519";
					            break;
				            case "EXMOff":
					            DeviceNo = "M007520";
					            break;
				            case "EXMReset":
					            DeviceNo = "M007521";
					            break;
				            case "EXMWUShort":
					            DeviceNo = "M007522";
					            break;
				            case "EXMWULong":
					            DeviceNo = "M007523";
					            break;
				            case "EXMWUNone":
					            DeviceNo = "M007524";
					            break;
				            case "MechaReset":  //追加  by 稲葉 10-09-10
					            DeviceNo = "M007526";
					            break;
				            case "MechaResetStop":  //追加  by 稲葉 10-09-10
					            DeviceNo = "M007527";
					            break;
				            case "RotXChangeSet":
					            DeviceNo = "M007528";
					            break;
				            case "DisXChangeSet":
					            DeviceNo = "M007529";
					            break;
				            case "RotYChangeSet":
					            DeviceNo = "M007530";
					            break;
				            case "DisYChangeSet":
					            DeviceNo = "M007531";
					            break;
				            case "VerIIChangeSet":
					            DeviceNo = "M007532";
					            break;
				            case "RotIIChangeSet":
					            DeviceNo = "M007533";
					            break;
				            case "GainIIChangeSet":
					            DeviceNo = "M007534";
					            break;
				            case "DisIIChangeSet":
					            DeviceNo = "M007535";
					            break;
				            case "SPIIChangeSet":
					            DeviceNo = "M007536";
					            break;
				            case "CTIISet":
					            DeviceNo = "M007538";
					            break;
				            case "TVIISet":
					            DeviceNo = "M007539";
					            break;
				            case "IIChangeReset":
					            DeviceNo = "M007540";
					            break;
				            case "IIChangeStop":
					            DeviceNo = "M007541";
					            break;
				            case "TVII9":
					            DeviceNo = "M007542";
					            break;
				            case "TVII6":
					            DeviceNo = "M007543";
					            break;
				            case "TVII4":
					            DeviceNo = "M007544";
					            break;
				            case "TVIIPowerOn":
					            DeviceNo = "M007545";
					            break;
				            case "TVIIPowerOff":
					            DeviceNo = "M007546";
					            break;
				            case "CameraPowerOn":
					            DeviceNo = "M007547";
					            break;
				            case "CameraPowerOff":
					            DeviceNo = "M007548";
					            break;
				            case "IrisLOpen":
					            DeviceNo = "M007549";
					            break;
				            case "IrisLClose":
					            DeviceNo = "M007550";
					            break;
				            case "IrisROpen":
					            DeviceNo = "M007551";
					            break;
				            case "IrisRClose":
					            DeviceNo = "M007552";
					            break;
				            case "IrisUOpen":
					            DeviceNo = "M007553";
					            break;
				            case "IrisUClose":
					            DeviceNo = "M007554";
					            break;
				            case "IrisDOpen":
					            DeviceNo = "M007555";
					            break;
				            case "IrisDClose":
					            DeviceNo = "M007556";
					            break;
				            case "CTChangeNO":
					            DeviceNo = "M007557";
					            break;
				            case "TVChangeNO":
					            DeviceNo = "M007558";
					            break;
				            case "AutoRestrictOn":  //追加  by 稲葉 10-10-19
					            DeviceNo = "M007560";
					            break;
				            case "AutoRestrictOff": //追加  by 稲葉 10-10-19
					            DeviceNo = "M007561";
					            break;
				            case "YIndexSlowOn":    //追加  by 稲葉 10-10-19
					            DeviceNo = "M007562";
					            break;
				            case "YIndexSlowOff":   //追加  by 稲葉 10-10-19
					            DeviceNo = "M007563";
					        break;
#else
                            case "CommCheck":
                                DeviceNo = "B000200";
                                break;
                            case "WordWrite":
                                DeviceNo = "B000201";
                                break;
                            case "PanelInhibit":
                                DeviceNo = "B000202";
                                break;
                            case "ErrOff":
                                DeviceNo = "B000205";
                                break;
                            case "RotXChangeReset":
                                DeviceNo = "B000206";
                                break;
                            case "DisXChangeReset":
                                DeviceNo = "B000207";
                                break;
                            case "RotYChangeReset":
                                DeviceNo = "B000208";
                                break;
                            case "DisYChangeReset":
                                DeviceNo = "B000209";
                                break;
                            case "VerIIChangeReset":
                                DeviceNo = "B00020A";
                                break;
                            case "RotIIChangeReset":
                                DeviceNo = "B00020B";
                                break;
                            case "GainIIChangeReset":
                                DeviceNo = "B00020C";
                                break;
                            case "DisIIChangeReset":
                                DeviceNo = "B00020D";
                                break;
                            case "SPIIChangeReset":     //追加  by 稲葉 05-10-21
                                DeviceNo = "B00020E";
                                break;
                            case "XLeft":
                                DeviceNo = "B000210";
                                break;
                            case "XRight":
                                DeviceNo = "B000211";
                                break;
                            case "XIndex":
                                DeviceNo = "B000212";
                                break;
                            case "XIndexStop":
                                DeviceNo = "B000213";
                                break;
                            //追加 by 稲葉 01-07-10
                            case "XOrigin":
                                DeviceNo = "B000214";
                                break;

                            case "YForward":
                                DeviceNo = "B000215";
                                Debug.Print("Y軸前進 = " + Data);      //追加 by 稲葉 02-03-05
                                break;
                            case "YBackward":
                                DeviceNo = "B000216";
                                Debug.Print("Y軸後退 = " + Data);      //追加 by 稲葉 02-03-05
                                break;
                            case "YIndex":
                                DeviceNo = "B000217";
                                break;
                            case "YIndexStop":
                                DeviceNo = "B000218";
                                break;
                            //追加 by 稲葉 04-09-01
                            case "YOrigin":
                                DeviceNo = "B000219";
                                break;

                            case "IIForward":
                                DeviceNo = "B00021A";
                                break;
                            case "IIBackward":
                                DeviceNo = "B00021B";
                                break;
                            case "IIindex":
                                DeviceNo = "B00021C";
                                break;
                            case "IIindexStop":
                                DeviceNo = "B00021D";
                                break;

                            case "XYindex":     //追加  by 稲葉 09-06-24
                                DeviceNo = "B00021E";
                                break;
                            case "TiltCw":
                                DeviceNo = "B000220";
                                break;
                            case "TiltCcw":
                                DeviceNo = "B000221";
                                break;
                            case "TiltOrigin":
                                DeviceNo = "B000222";
                                break;
                            case "ColliLOpen":
                                DeviceNo = "B000225";
                                break;
                            case "ColliLClose":
                                DeviceNo = "B000226";
                                break;
                            case "ColliROpen":
                                DeviceNo = "B000227";
                                break;
                            case "ColliRClose":
                                DeviceNo = "B000228";
                                break;
                            case "ColliUOpen":
                                DeviceNo = "B000229";
                                break;
                            case "ColliUClose":
                                DeviceNo = "B00022A";
                                break;
                            case "ColliDOpen":
                                DeviceNo = "B00022B";
                                break;
                            case "ColliDClose":
                                DeviceNo = "B00022C";
                                break;
                            case "Filter0":
                                DeviceNo = "B000230";
                                break;
                            case "Filter1":
                                DeviceNo = "B000231";
                                break;
                            case "Filter2":
                                DeviceNo = "B000232";
                                break;
                            case "Filter3":
                                DeviceNo = "B000233";
                                break;
                            case "Filter4":
                                DeviceNo = "B000234";
                                break;
                            case "Filter5":
                                DeviceNo = "B000235";
                                break;
                            case "stsXrayOn":
                                DeviceNo = "B000238";
                                break;
                            case "XrayInhibit":
                                DeviceNo = "B000239";
                                break;
                            case "Xray225":
                                DeviceNo = "B00023A";
                                break;
                            case "Xray160":
                                DeviceNo = "B00023B";
                                break;
                            //追加 by 稲葉 03-08-01
                            case "II":
                                DeviceNo = "B00023C";
                                break;
                            case "FPD":
                                DeviceNo = "B00023D";
                                break;
                            case "XTableMove":
                                DeviceNo = "B00023E";
                                break;
                            case "TubeMove":
                                DeviceNo = "B00023F";
                                break;

                            case "II9":
                                DeviceNo = "B000240";
                                break;
                            case "II6":
                                DeviceNo = "B000241";
                                break;
                            case "II4":
                                DeviceNo = "B000242";
                                break;
                            case "IIPowerOn":
                                DeviceNo = "B000243";
                                break;
                            case "IIPowerOff":
                                DeviceNo = "B000244";
                                break;
                            //追加 by 稲葉 01-07-10
                            case "SLightOn":
                                DeviceNo = "B000245";
                                break;
                            case "SLightOff":
                                DeviceNo = "B000246";
                                break;
                            case "TableMovePermit":
                                DeviceNo = "B000247";
                                break;
                            case "TableMoveRestrict":
                                DeviceNo = "B000248";
                                break;

                            case "stsUDErr":
                                DeviceNo = "B00024A";
                                break;
                            case "stsUDLimit":
                                DeviceNo = "B00024B";
                                break;
                            case "stsUDBusy":
                                DeviceNo = "B00024C";
                                break;
                            case "stsRotErr":
                                DeviceNo = "B000250";
                                break;
                            case "stsRotBusy":
                                DeviceNo = "B000251";
                                break;
                            //追加 by 稲葉 01-07-10
                            case "stsXStgErr":
                                DeviceNo = "B000252";
                                break;
                            case "stsYStgErr":
                                DeviceNo = "B000254";
                                break;

                            case "stsPhmErr":
                                DeviceNo = "B000256";
                                break;
                            case "stsPhmLimit":
                                DeviceNo = "B000257";
                                break;
                            case "stsPhmBusy":
                                DeviceNo = "B000258";
                                break;
                            case "stsPhmOnOff":
                                DeviceNo = "B000259";
                                break;
                            //追加 by 稲葉 03-06-16
                            case "RotOriginOK":
                                DeviceNo = "B00025A";
                                break;
                            case "TrgReq":
                                DeviceNo = "B00025B";
                                break;
                            //追加 by 稲葉 03-10-14
                            case "FXTableOn":
                                DeviceNo = "B00025C";
                                break;
                            case "FXTableOff":
                                DeviceNo = "B00025D";
                                break;
                            case "FYTableOn":
                                DeviceNo = "B00025E";
                                break;
                            case "FYTableOff":
                                DeviceNo = "B00025F";
                                break;
                            //追加 by 稲葉 04-09-01
                            case "XrayXLeft":
                                DeviceNo = "B000260";
                                break;
                            case "XrayXRight":
                                DeviceNo = "B000261";
                                break;
                            case "XrayXOrg":
                                DeviceNo = "B000262";
                                break;
                            case "XrayXIndex":
                                DeviceNo = "B000263";
                                break;
                            case "XrayXStop":
                                DeviceNo = "B000264";
                                break;
                            case "RotXrayXChReset":
                                DeviceNo = "B000265";
                                break;
                            case "DisXrayXChReset":
                                DeviceNo = "B000266";
                                break;
                            case "XrayYForward":
                                DeviceNo = "B00026B";
                                break;
                            case "XrayYBackward":
                                DeviceNo = "B00026C";
                                break;
                            case "XrayYOrg":
                                DeviceNo = "B00026D";
                                break;
                            case "XrayYIndex":
                                DeviceNo = "B00026E";
                                break;
                            case "XrayYStop":
                                DeviceNo = "B00026F";
                                break;
                            case "RotXrayYChReset":
                                DeviceNo = "B000270";
                                break;
                            case "DisXrayYChReset":
                                DeviceNo = "B000271";
                                break;
                            //追加 by 稲葉 05-12-06
                            case "RotXChangeSet":
                                DeviceNo = "B000280";
                                break;
                            case "DisXChangeSet":
                                DeviceNo = "B000281";
                                break;
                            case "RotYChangeSet":
                                DeviceNo = "B000282";
                                break;
                            case "DisYChangeSet":
                                DeviceNo = "B000283";
                                break;
                            case "VerIIChangeSet":
                                DeviceNo = "B000284";
                                break;
                            case "RotIIChangeSet":
                                DeviceNo = "B000285";
                                break;
                            case "GainIIChangeSet":
                                DeviceNo = "B000286";
                                break;
                            case "DisIIChangeSet":
                                DeviceNo = "B000287";
                                break;
                            case "SPIIChangeSet":
                                DeviceNo = "B000288";
                                break;
#endif
                            default:
                                _DefaultDevice = true;
                                break;
                        }

                        if (_DefaultDevice == false)
                        {
                            //書込みﾋﾞｯﾄﾃﾞｰﾀ
                            BitData = Data ? "1" : "0";

                            //1文字受信で通信ｲﾍﾞﾝﾄを発生させる。   追加 by 稲葉 02-02-26
                            //エラーするのでThresholdサイズを変えない　//2014/04/23(検S1)hata
                            //mySerialPort.ReceivedBytesThreshold = 1;
                            
                            //ﾋﾞｯﾄﾃﾞﾊﾞｲｽ書込みｺﾏﾝﾄﾞを送信します。
                            //書込みｺﾏﾝﾄﾞ = ENQ + 局番 + PC番号 + ｺﾏﾝﾄﾞ + ｳｴｲﾄ + 先頭ﾃﾞﾊﾞｲｽ + 書込みﾋﾞｯﾄ数 + ﾃﾞｰﾀ + CR + LF
                            BitWriteCommand = Convert.ToChar(0x5) + "00" + "FF" + "JW" + "0" + DeviceNo + "01" + BitData + "\r\n";   //ﾋﾞｯﾄﾃﾞﾊﾞｲｽ書込ｺﾏﾝﾄﾞ（1点単位）

                            //Log用　2015/03/02hata
                            //strSendCommamd = BitWriteCommand; //変更2015/03/04_hata
                            strSendCommamd = DeviceName;

                            //追加2015/03/16hata
                            // CTSﾗｲﾝｴﾗｰﾁｪｯｸ
                            DateTime ntim = DateTime.Now;
                            while (!mySerialPort.CtsHolding)
                            {
                                if (((DateTime.Now - ntim).TotalSeconds > 2000))
                                {
                                    //Logを取る
                                    //LogWrite("CtsHolding=false");
                                    break;
                                }
                                Thread.Sleep(1);
                            }

                            mySerialPort.Write(BitWriteCommand);

                            // 通信ﾀｲﾑｱｳﾄ時間を設定します。
                            TimeOutSet();
                            
                        }
                    }

                    if (_DefaultDevice == true)
                    {
                        int _CommEndAns;
                        bool _RaiseEvent;

                        lock (gLock)
                        {
                            SeqCommEvent = 716;
                            functionReturnValue = SeqCommEvent;
                            _CommEndAns = SeqCommEvent;
                            _RaiseEvent = EventProcess(SeqCommEvent);   //ｲﾍﾞﾝﾄ処理
                        }
                        if (_RaiseEvent == true && OnCommEnd != null) OnCommEnd(_CommEndAns);
                    }
                }
            }
            catch 
            {
                //通信ﾎﾟｰﾄｴﾗｰ
                int _CommEndAns;
                bool _RaiseEvent;

                lock (gLock)
                {
                    SeqCommEvent = 715;
                    functionReturnValue = SeqCommEvent;
                    _CommEndAns = SeqCommEvent;
                    _RaiseEvent = EventProcess(SeqCommEvent);
                }
                if (_RaiseEvent == true && OnCommEnd != null) OnCommEnd(_CommEndAns);
            }

            return functionReturnValue;
        }
        #endregion

        #region WordWriteExe
        /// <summary>
        /// ｼｰｹﾝｻへのﾜｰﾄﾞﾃﾞｰﾀの書込み（ﾌﾟﾛﾄｺﾙﾓｰﾄﾞ4）
        /// </summary>
        /// <param name="DeviceName"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
	    private int WordWriteExe(string DeviceName, ref string Data)
	    {
		    int functionReturnValue = 0;
		    string WordWriteCommand = null; //ﾜｰﾄﾞ書込みｺﾏﾝﾄﾞ
		    string DeviceNo = null;     //ﾃﾞﾊﾞｲｽ番号
		    string WordNo = null;       //書込みﾜｰﾄﾞ数
		    string WordData = null;     //ﾜｰﾄﾞﾃﾞｰﾀ
		    string WordData2 = null;    //2ﾜｰﾄﾞﾃﾞｰﾀ
            //int tmpval = 0;

		    //初期化         '追加 2011/03/02 by 間々田 初期化を明示的に実行（VB.NET化を考慮）
		    WordData = "";

            bool _DefaultDevice = false;

            try
            {
                functionReturnValue = CommInit();

                if (functionReturnValue != 0)
                {
                    return functionReturnValue;
                }
                else
                {
                    lock (gLock)
                    {
                        switch (DeviceName)
                        {
#if conKvMode1
                            case "XSpeed":
                                DeviceNo = "D019100";
                                WordNo = "01";
                                break;
                            case "YSpeed":
                                DeviceNo = "D019102";
                                WordNo = "01";
                                break;
                            case "YIndexPosition":
                                DeviceNo = "D019103";
                                WordNo = "02";
                                break;
                            case "XIndexPosition":
                                DeviceNo = "D019105";
                                WordNo = "02";
                                break;
                            case "stsUDPosition":
                                DeviceNo = "D019108";
                                WordNo = "02";
                                break;
                            //追加 by 稲葉 05-12-06
                            case "EXMTVSet":
                                DeviceNo = "D019110";
                                WordNo = "01";
                                break;
                            case "EXMTCSet":
                                DeviceNo = "D019111";
                                WordNo = "01";
                                break;

                            case "stsRotPosition":
                                DeviceNo = "D019113";
                                WordNo = "02";
                                break;
                            case "stsXStgPosition":
                                DeviceNo = "D019116";
                                WordNo = "01";
                                break;
                            case "stsYStgPosition":
                                DeviceNo = "D019118";
                                WordNo = "01";
                                break;
                            case "IISpeed":
                                DeviceNo = "D019120";
                                WordNo = "01";
                                break;
                            case "IIindexPosition":
                                DeviceNo = "D019121";
                                WordNo = "02";
                                break;
                            case "ScanStartPos":
                                DeviceNo = "D019123";
                                WordNo = "01";
                                break;
                            case "TrgTime":
                                DeviceNo = "D019124";
                                WordNo = "02";
                                break;
                            case "TrgCycleTime":    //追加 by 稲葉 04-06-10
                                DeviceNo = "D019126";
                                WordNo = "02";
                                break;
                            case "XrayXSpeed":
                                DeviceNo = "D019128";
                                WordNo = "01";
                                break;
                            case "XrayXPos":
                                DeviceNo = "D019129";
                                WordNo = "01";
                                break;
                            case "XrayYSpeed":
                                DeviceNo = "D019132";
                                WordNo = "01";
                                break;
                            case "XrayYPos":
                                DeviceNo = "D019133";
                                WordNo = "01";
                                break;
                            case "XrayRotSpeed":
                                DeviceNo = "D019136";
                                WordNo = "01";
                                break;
                            case "XrayRotPos":
                                DeviceNo = "D019137";
                                WordNo = "02";
                                break;
                            case "FCDLimit":    //追加 by 稲葉 04-12-13
                                DeviceNo = "D019139";
                                WordNo = "01";
                                break;
                            case "FCDFineLimit":    //追加 by 稲葉 04-12-13
                                DeviceNo = "D019140";
                                WordNo = "01";
                                break;
                            case "FIDFineLimit":    //追加 by 稲葉 04-12-13
                                DeviceNo = "D019141";
                                WordNo = "01";
                                break;
                            case "stsTiltPosition":     //追加 by 稲葉 15-07-24
                                DeviceNo = "D019142";
                                WordNo = "02";
                                break;
                            case "stsTiltRotPosition":  //追加 by 稲葉 15-07-24
                                DeviceNo = "D019144";
                                WordNo = "02";
                                break;
                            case "stsLinearUDPosition":  //追加 by 稲葉 15-07-24
                                DeviceNo = "D019146";
                                WordNo = "02";
                                break;
                            case "LargeTableRingWidth":  //追加 by 稲葉 17-09-11
                                DeviceNo = "D019148";
                                WordNo = "01";
                                break;

                            // 電池検査用    //追加  by 稲葉 16-04-14
                            case "ColiUIndexPosition":
                                DeviceNo = "D019200";
                                WordNo = "01";
                                break;
                            case "ColiDIndexPosition":
                                DeviceNo = "D019201";
                                WordNo = "01";
                                break;

                            // ｱｲｼﾝ精機向けFDZ2＋高速度ｶﾒﾗ用 //追加  by 稲葉 19-03-04
                            case "XrayCameraUDIndexPosition":
                                DeviceNo = "D019400";
                                WordNo = "02";
                                break;

#elif conKvMode2        //追加 by 稲葉 10-05-11
				            case "XSpeed":
					            DeviceNo = "D007100";
					            WordNo = "01";
					            break;
				            case "YSpeed":
					            DeviceNo = "D007102";
					            WordNo = "01";
					            break;
				            case "YIndexPosition":
					            DeviceNo = "D007103";
					            WordNo = "02";
					            break;
				            case "XIndexPosition":
					            DeviceNo = "D007105";
					            WordNo = "02";
					            break;
				            case "stsUDPosition":
					            DeviceNo = "D007108";
					            WordNo = "02";
					            break;
				            case "EXMTVSet":
					            DeviceNo = "D007110";
					            WordNo = "01";
					            break;
				            case "EXMTCSet":
					            DeviceNo = "D007111";
					            WordNo = "01";
					            break;
				            case "stsRotPosition":
					            DeviceNo = "D007113";
					            WordNo = "02";
					            break;
				            case "stsXStgPosition":
					            DeviceNo = "D007116";
					            WordNo = "01";
					            break;
				            case "stsYStgPosition":
					            DeviceNo = "D007118";
					            WordNo = "01";
					            break;
				            case "IISpeed":
					            DeviceNo = "D007120";
					            WordNo = "01";
					            break;
				            case "IIindexPosition":
					            DeviceNo = "D007121";
					            WordNo = "02";
					            break;
				            case "ScanStartPos":
					            DeviceNo = "D007123";
					            WordNo = "01";
					            break;
				            case "TrgTime":
					            DeviceNo = "D007124";
					            WordNo = "02";
					            break;
				            case "TrgCycleTime":
					            DeviceNo = "D007126";
					            WordNo = "02";
					            break;
				            case "XrayXSpeed":
					            DeviceNo = "D007128";
					            WordNo = "01";
					            break;
				            case "XrayXPos":
					            DeviceNo = "D007129";
					            WordNo = "01";
					            break;
				            case "XrayYSpeed":
					            DeviceNo = "D007132";
					            WordNo = "01";
					            break;
				            case "XrayYPos":
					            DeviceNo = "D007133";
					            WordNo = "01";
					            break;
				            case "XrayRotSpeed":
					            DeviceNo = "D007136";
					            WordNo = "01";
					            break;
				            case "XrayRotPos":
					            DeviceNo = "D007137";
					            WordNo = "02";
					            break;
				            case "FCDLimit":
					            DeviceNo = "D007139";
					            WordNo = "01";
					            break;
				            case "FCDFineLimit":
					            DeviceNo = "D007140";
					            WordNo = "01";
					            break;
				            case "FIDFineLimit":
					            DeviceNo = "D007141";
					            WordNo = "01";
					            break;
#else
                            case "XSpeed":
                                DeviceNo = "W000100";
                                WordNo = "01";
                                break;
                            case "YSpeed":
                                DeviceNo = "W000102";
                                WordNo = "01";
                                break;
                            case "YIndexPosition":
                                DeviceNo = "W000103";
                                WordNo = "02";
                                break;
                            case "XIndexPosition":
                                DeviceNo = "W000105";
                                WordNo = "02";
                                break;
                            case "stsUDPosition":
                                DeviceNo = "W000108";
                                WordNo = "02";
                                break;
                            case "stsRotPosition":
                                DeviceNo = "W00010D";
                                WordNo = "02";
                                break;
                            //追加 by 稲葉 01-07-10
                            case "stsXStgPosition":
                                DeviceNo = "W000110";
                                WordNo = "01";
                                break;
                            case "stsYStgPosition":
                                DeviceNo = "W000112";
                                WordNo = "01";
                                break;
                            case "IISpeed":
                                DeviceNo = "W000114";
                                WordNo = "01";
                                break;
                            case "IIindexPosition":
                                DeviceNo = "W000115";
                                WordNo = "02";
                                break;
                            //追加 by 稲葉 03-06-16
                            case "ScanStartPos":
                                DeviceNo = "W000117";
                                WordNo = "01";
                                break;
                            case "Trgtime":
                                DeviceNo = "W000118";
                                WordNo = "02";
                                break;
                            //追加 by 稲葉 04-09-01
                            case "TrgCycleTime":
                                DeviceNo = "W000120";
                                WordNo = "02";
                                break;
                            case "XrayXSpeed":
                                DeviceNo = "W000123";
                                WordNo = "01";
                                break;
                            case "XrayXPos":
                                DeviceNo = "W000124";
                                WordNo = "01";
                                break;
                            case "XrayYSpeed":
                                DeviceNo = "W000127";
                                WordNo = "01";
                                break;
                            case "XrayYPos":
                                DeviceNo = "W000128";
                                WordNo = "01";
                                break;
                            case "XrayRotSpeed":
                                DeviceNo = "W000131";
                                WordNo = "01";
                                break;
                            case "XrayRotPos":
                                DeviceNo = "W000132";
                                WordNo = "02";
                                break;
                            case "FCDLimit":    //追加 by 稲葉 04-12-13
                                DeviceNo = "W000134";
                                WordNo = "01";
                                break;
                            case "FCDFineLimit":    //追加 by 稲葉 04-12-13
                                DeviceNo = "W000135";
                                WordNo = "01";
                                break;
                            case "FIDFineLimit":    //追加 by 稲葉 04-12-13
                                DeviceNo = "W000136";
                                WordNo = "01";
                                break;
#endif
                            default:
                                _DefaultDevice = true;
                                break;
                        }

                        if (_DefaultDevice == false)
                        {
                            //書込みﾃﾞｰﾀ
                            if (WordNo == "01")
                            {
                                //追加 by 稲葉 03-10-28
                                if (Convert.ToDouble(Data) > 32767)
                                {
                                    Data = "32767";
                                }
                                else if (Convert.ToDouble(Data) < -32768)
                                {
                                    Data = "-32768";
                                }

                                //変更 by 稲葉 04-03-30
                                //            WordData = Format(Hex(CInt(Data)), "@@@@")
                                WordData = Convert.ToString(Convert.ToInt16(Data), 16).PadLeft(4, '0');

                            }
                            else if (WordNo == "02")
                            {
                                //変更 by 稲葉 04-03-30
                                //            WordData2 = Format(Hex(CLng(Data)), "@@@@@@@@")
                                WordData2 = Convert.ToString(Convert.ToInt32(Data), 16).PadLeft(8, '0');
                                WordData = WordData2.Substring(4, 4) + WordData2.Substring(0, 4);   //2ﾜｰﾄﾞﾃﾞｰﾀの上位下位の入替
                            }

                            //1文字受信で通信ｲﾍﾞﾝﾄを発生させる。   追加 by 稲葉 02-02-26
                            //エラーするのでThresholdサイズを変えない　//2014/04/23(検S1)hata
                            //mySerialPort.ReceivedBytesThreshold = 1;
                            
                            //ﾜｰﾄﾞﾃﾞﾊﾞｲｽ書込みｺﾏﾝﾄﾞを送信します。
                            //書込みｺﾏﾝﾄﾞ = ENQ + 局番 + PC番号 + ｺﾏﾝﾄﾞ + ｳｴｲﾄ + 先頭ﾃﾞﾊﾞｲｽ + 書込みﾜｰﾄﾞ数 + ﾃﾞｰﾀ + CR + LF
                            WordWriteCommand = Convert.ToChar(0x5) + "00" + "FF" + "QW" + "0" + DeviceNo + WordNo + WordData + "\r\n";   //ﾜｰﾄﾞﾃﾞﾊﾞｲｽ書込ｺﾏﾝﾄﾞ

                            //Log用　2015/03/02hata
                            //strSendCommamd = WordWriteCommand;    //変更2015/03/04_hata
                            strSendCommamd = DeviceName;

                            //追加2015/03/16hata
                            // CTSﾗｲﾝｴﾗｰﾁｪｯｸ
                            DateTime ntim = DateTime.Now;
                            while (!mySerialPort.CtsHolding)
                            {
                                if (((DateTime.Now - ntim).TotalSeconds > 2000))
                                {
                                    //Logを取る
                                    //LogWrite("CtsHolding=false");
                                    break;
                                }
                                Thread.Sleep(1);
                            }

                            mySerialPort.Write(WordWriteCommand);

                            // 通信ﾀｲﾑｱｳﾄ時間を設定します。
                            TimeOutSet();
                        }
                    }

                    if (_DefaultDevice == true)
                    {
                        int _CommEndAns;
                        bool _RaiseEvent;

                        lock (gLock)
                        {
                            SeqCommEvent = 721;
                            functionReturnValue = SeqCommEvent;
                            _CommEndAns = SeqCommEvent;
                            _RaiseEvent = EventProcess(SeqCommEvent);     //ｲﾍﾞﾝﾄ処理
                        }
                        if (_RaiseEvent == true && OnCommEnd != null) OnCommEnd(_CommEndAns);
                    }
                }
            }
            catch 
            {
		        //通信ﾎﾟｰﾄｴﾗｰ
                int _CommEndAns;
                bool _RaiseEvent;

                lock (gLock)
                {
                    SeqCommEvent = 720;
                    functionReturnValue = SeqCommEvent;
                    _CommEndAns = SeqCommEvent;
                    _RaiseEvent = EventProcess(SeqCommEvent);
                }
                if (_RaiseEvent == true && OnCommEnd != null) OnCommEnd(_CommEndAns);
            }

            return functionReturnValue;
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
	    public Seq()
	    {
            lock (gLock)
            {
                //シーケンサがレディ状態ではない　v11.5追加 by 間々田 2006/07/12
                IsReady = false;

                //追加 by 稲葉 04-03-01
                if (mySerialPort == null)
                {
                    mySerialPort = new SerialPort();

                    CommEndFlg = false;  //追加2014/06/09_hata

                    // 通信ﾎﾟｰﾄを開きます。
                    mySerialPort.PortName = "COM2";

#if conKvMode1 || conKvMode2

                    mySerialPort.BaudRate = 115200;
                    mySerialPort.DataBits = 8;
                    mySerialPort.Parity = Parity.None;
                    mySerialPort.StopBits = StopBits.One;
#else
                    mySerialPort.BaudRate = 38400;
                    mySerialPort.DataBits = 8;
                    mySerialPort.Parity = Parity.None;
                    mySerialPort.StopBits = StopBits.One;
#endif
                    mySerialPort.Handshake = Handshake.RequestToSend;
                    
                    mySerialPort.DtrEnable = true;  //追加2015/03/13hata
                    mySerialPort.RtsEnable = true;

                    //条件追加 by 稲葉 05-01-18
                    if (!mySerialPort.IsOpen)
                    {
                        mySerialPort.Open();
                    }

                    mySerialPort.ReceivedBytesThreshold = 1;    //1文字受信で通信ｲﾍﾞﾝﾄを発生させる
                    mySerialPort.NewLine = "\r\n";
                    mySerialPort.DiscardOutBuffer();    // 送信ﾊﾞｯﾌｧをｸﾘｱします                             
                    mySerialPort.DiscardInBuffer();     // 受信ﾊﾞｯﾌｧをｸﾘｱします                

                    //Debug.WriteLine("CtsHolding = " + mySerialPort.CtsHolding.ToString());
                    //Debug.WriteLine("DsrHolding = " + mySerialPort.DsrHolding.ToString());
                    //Debug.WriteLine("IsOpen = " + mySerialPort.IsOpen.ToString());
                    //Debug.WriteLine("Parity = " + mySerialPort.Parity.ToString());
                    
                    // ポートがエラーするときは書き込まない //変更2015/02/10hata
                    //if (!CommLineError & !CommDataError & mySerialPort.CtsHolding & (!mySerialPort.DtrEnable | (mySerialPort.DtrEnable & mySerialPort.DsrHolding)))
                    if (!CommLineError & !CommDataError & mySerialPort.CtsHolding)
                        mySerialPort.Write(Convert.ToChar(0x4) + "\r\n");  //ｼｰｹﾝｻの送受信ｼｰｹﾝｽをｸﾘｱします｡
                }

                lock (tLock)
                {
                    if (myTimer == null)
                    {
                        myTimer = new System.Timers.Timer(Interval);
                        myTimer.Enabled = false;
                    }
                }

                //test畑---4/23
                //mySendTimer = new System.Timers.Timer(SendInterval);
                //mySendTimer.Enabled = false;

                //追加 by 稲葉 03-08-29
                BitWriteName = new string[BufferSize + 1];
                BitWriteData = new bool[BufferSize + 1];
                WordWriteName = new string[BufferSize + 1];
                WordWriteData = new string[BufferSize + 1];
                BitWordType = new string[BufferSize * 2 + 1];

                StatusReadNo = 0;
                BitWriteNo = 0;
                WordWriteNo = 0;
                BitWordWriteNo = 0;

                // イベントハンドラの定義
                InitializeHandler();
            }
        }
        #endregion

        #region 終了処理
        /// <summary>
        /// リソースの解放
        /// </summary>
        public void Dispose()
        {
            //変更2014/06/09_hata
            //排他処理しない
            //lock (gLock)
            //{
 
                PortClose();                
                TimerDispose();
                PortDispose();
            //}
        }

        /// <summary>
        /// シリアルポートを閉じる
        /// </summary>
        private void PortClose()
        {
            //int CntEnd = 2000;  //削除2015/03/16hata
            try
            {
              
                //通信終了
                CommEndFlg = true;  //追加2014/06/09_hata
 
 
                // ﾎﾟｰﾄを閉じます。
                if (mySerialPort != null && mySerialPort.IsOpen)
                {
                    //シリアルポートがReadから離れるまで待つ
                    //SerialRead.WaitOne(CntEnd);     //削除2015/03/16hata
                    mySerialPort.Close();
                }
            }
            catch
            {
                // '    Beep
                // '    MsgBox "通信ポート（COM2）はクローズできませんでした。"
            }
        }

        /// <summary>
        /// シリアルポートのリソースを解放する
        /// </summary>
        private void PortDispose()
        {
            try
            {
                if (mySerialPort != null)
                {
                    mySerialPort.Dispose();
                    mySerialPort = null;
                }
            }
            catch
            {
                // Nothing
            }
        }

        /// <summary>
        /// タイマーのリソースを解放する
        /// </summary>
        private void TimerDispose()
        {
            try
            {
                lock (tLock)
                {
                    if (myTimer != null)
                    {
                        myTimer.Enabled = false;
                        myTimer.Dispose();
                        myTimer = null;
                    }

                    ////test畑---4/23
                    //if (mySendTimer != null)
                    //{
                    //    mySendTimer.Enabled = false;
                    //    mySendTimer.Dispose();
                    //    mySendTimer = null;
                    //}
 
                }
            }
            catch
            {
                // Nothing              
            }
        }
        #endregion

        #region イベントハンドラの定義
        /// <summary>
        /// イベントハンドラの定義
        /// </summary>
        private void InitializeHandler()
        {
            mySerialPort.DataReceived += delegate(object sender, SerialDataReceivedEventArgs e) 
            { 
			    // 受信ﾃﾞｰﾀがあれば読み出します。
				ReceiveProcess();
            };

            mySerialPort.ErrorReceived += delegate(object sender, SerialErrorReceivedEventArgs e)
            {
		        switch (e.EventType)
                {
                    //変更2015/02/10hata
                    case SerialError.Frame:     //フレーム エラーです。
                        //break;
                    //case SerialError.Overrun:   //ポート オーバーランです。
                        //break;
                    case SerialError.RXOver:    //受信バッファ オーバーフローです。
                        //break;
                    case SerialError.RXParity:  //パリティ エラーです。
                        //break;
                    case SerialError.TXFull:    //送信バッファがいっぱいです。
                        CommDataError = true;   
                        break;
                    default:
                        break;
                }

            
            };

            //追加2015/02/10hata
            mySerialPort.PinChanged += delegate(object sender, SerialPinChangedEventArgs e)
            {
                //string data = "";
                bool eCTSFlg = false;
                bool eDSRFlg = false;

                //ピンチェンジの判別
                switch (e.EventType)
                {
                    case SerialPinChange.CtsChanged:
                        //CTSのPinが変更された
                        if (mySerialPort.CtsHolding)
                        {    //CTS On
                            //data = "CTS On";
                        }
                        else
                        {    //CTS Off
                            //data = "CTS Off";
                            eCTSFlg = true;

                        }
                        break;

                    //戻す2015/03/13hata
                    case SerialPinChange.DsrChanged:
                        //DSRのPinが変更された
                        if (mySerialPort.DsrHolding)
                        {
                            //DSR On
                            //data = "DSR On";
                        }
                        else
                        {
                            //DSR off
                            //data = "DSR Off";
                            eDSRFlg = true;
                        }
                        break;

                    case SerialPinChange.CDChanged:
                        //CD (Carrier Detect) シグナルの状態が変化しました。
                        //このシグナルは、モデムが動作中の電話回線に接続され、
                        //データ キャリア シグナルが検出されるかどうかを示すために使用されます。

                        //CDのPinが変更された
                        if (mySerialPort.CDHolding)
                        {
                            //CD On
                            //data = "CD On";
                        }
                        else
                        {
                            //CD off
                            //ata = "CD Off";
                        }
                        break;

                    case SerialPinChange.Break:
                        //入力時にブレークが検出されました。

                        //Breakが検出された
                        //data = "Break On";
                        break;

                    case SerialPinChange.Ring:
                        //リング インジケーターが検出されました。モデム動作

                        //Ringが検出された
                        //data = "Ring On";
                        break;
                }

                //追加2015/02/10hata
                if (eCTSFlg | eDSRFlg)
                    CommLineError = true;
                else
                    CommLineError = false;

                 
            };

            myTimer.Elapsed += new ElapsedEventHandler(mTimer_Timer);

        }
        #endregion

        #region CommInit

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int CommInit()
        {
            int functionReturnValue = 0;
            
            int _CommEndAns = 0;
            bool _RaiseEvent = false;

            lock (gLock)
            {
                // CTSﾗｲﾝｴﾗｰﾁｪｯｸ
                //DebuTest3/16hata
                //if (!mySerialPort.CtsHolding)
                if (!mySerialPort.DsrHolding)
                {
                    SeqCommEvent = 701;
                    functionReturnValue = SeqCommEvent;
                    _CommEndAns = SeqCommEvent;
                    _RaiseEvent = EventProcess(SeqCommEvent);    //ｲﾍﾞﾝﾄ処理                    
                }
                else
                {                    
                    mySerialPort.DiscardOutBuffer();    // 送信ﾊﾞｯﾌｧをｸﾘｱします                    
                    mySerialPort.DiscardInBuffer();     // 受信ﾊﾞｯﾌｧをｸﾘｱします
                    rData = "";

                    return functionReturnValue;
                }
            }
            if (_RaiseEvent == true && OnCommEnd != null) OnCommEnd(_CommEndAns);

            return functionReturnValue;
        }

        #endregion

        #region mTimer_Timer
        private void mTimer_Timer(object sender, EventArgs e)
        {
            lock (tLock)
            {
                if (myTimer == null || myTimer.Enabled == false) return;

                myTimer.Enabled = false;

                //通信ﾀｲﾑｱｳﾄﾁｪｯｸ
                //If Timer > Duration Then		    
                if (!((DateTime.Now - Duration).TotalSeconds > 0))     //v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20
                {
                    myTimer.Enabled = true;
                    return;
                }                
            }

            // タイムアウト発生時、タイマーを再開せずOnCommEndイベントを発生させる
            int _CommEndAns;
            lock (gLock)
            {
                SeqCommEvent = 703;
                _CommEndAns = SeqCommEvent;
                EventProcess(SeqCommEvent);
            }
            //Log書き込み　2015/03/02hata
            LogWrite();

            if (OnCommEnd != null) OnCommEnd(_CommEndAns);
        }
        #endregion

        #region TimeOutSet
        private void TimeOutSet()
        {
            // 通信ﾀｲﾑｱｳﾄ時間を設定します。
            //Duration = Timer + CommTimeOut
            Duration = DateTime.Now.AddSeconds(CommTimeOut);    //v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20

            lock (tLock)
            {
                if (myTimer != null) myTimer.Enabled = true;
            }
        }
        #endregion

        #region Log書き込み
        //変更2015/03/17hata
        //Log書き込み　2015/03/02hata
        //private void LogWrite()
        private void LogWrite(string pulsMsg = "")
        {
            //追加2015/03/04hata
            string file = @"C:\CTUSER\SeqCommERR";
            string ext = ".log";
            bool app = true;
            string Msg = "";
            string DataAll ="NoAll";

            try
            {
                System.IO.FileInfo finfo = new System.IO.FileInfo(file + ext);
                if (finfo.Exists)
                {
                    if (finfo.Length > 10000000)
                    {
                        //10Mを超える場合は名前の後ろに日付を付けて保管
                        string st = DateTime.Now.ToString("yyyyMMdd'-'HH'.'mm'.'ss'.'fffffff");
                        finfo.MoveTo(file + "_" + st + ext);
                        app = false;
                    }
                }
            }
            catch
            {
            }

            //追加2015/03/18hata
            if (pulsMsg != "") Msg = "; " + pulsMsg;
            if (bDatAll) DataAll = "All";

            //変更2015/03/04hata
            //System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\CTUSER\SeqCommERR.log", true);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(file + ext, app);
            try
            {
                //sw.WriteLine(DateTime.Now.ToString() + " " + strSendCommamd);
                //変更2015/03/18hata
                //sw.WriteLine(DateTime.Now.ToString() + " " + "SeqCommEvent=" + SeqCommEvent.ToString() + " " + strSendCommamd );
                sw.WriteLine(DateTime.Now.ToString() + "; " +
                             "SeqCommEvent=" + SeqCommEvent.ToString() + "; " +
                             strSendCommamd + "; " +
                             "ReciveTime=" + RecvOnTime.ToString() + " - " + 
                             "Data=" + DataAll + " - " +
                             "RcvPt=" + RecvPoint.ToString()
                             + Msg);

            }
            catch
            {
                //Debug.WriteLine("PinChanged Err");
            }
            finally
            {
                sw.Close();
                sw.Dispose();
            }
        }
        #endregion






        ////test畑---4/23
        //#region mSendTimer_Timer
        //bool mSendf = false;
        //private void mSendTimer_Timer(object sender, EventArgs e)
        //{
        //    //if (mySendTimer != null)
        //    //{
        //    //    mySendTimer.Enabled = false;
        //    //    if (!mSendf) return;
        //    //    mSendf = true; 
            
        //    //    //MethodProcessを実行する
        //    //    MethodProcess();    //通信待ち時のｼｰｹﾝｻへの読込書込み処理                        
            
        //    //}
        //    //mSendf = false;
        //}
        //#endregion


        #region EventProcess
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CommEndAns"></param>
        /// <returns>OnCommEndイベントを発生する場合に true を返す</returns>
        private bool EventProcess(int CommEndAns)
	    {
            byte[] buf = {0x4, 0xd, 0xa};
            bool functionReturnValue = false;

            try
            {
                //ｲﾍﾞﾝﾄ処理
                lock (tLock)
                {
                    if (myTimer != null) myTimer.Enabled = false;
                }

                //v11.5追加 by 間々田 2006/07/12 '0の場合（正常の場合）イベントを返すのをやめた
                if (CommEndAns == 0)
                {
                    IsReady = true; //シーケンサがレディ状態 'v11.5追加 by 間々田 2006/07/12
                }
                else
                {
                    StatusReadNo = 0;
                    BitWriteNo = 0;
                    WordWriteNo = 0;
                    BitWordWriteNo = 0;     //追加 by 稲葉 02-09-19
                    stsCommBusy = false;
                                        
                    mySerialPort.DiscardInBuffer();         // 受信ﾊﾞｯﾌｧをｸﾘｱします                   
                    mySerialPort.DiscardOutBuffer();        // 送信ﾊﾞｯﾌｧをｸﾘｱします                    

                    // ポートがエラーするときは書き込まない //変更2015/02/10hata
                    //if (!CommLineError & !CommDataError & mySerialPort.CtsHolding & (!mySerialPort.DtrEnable | (mySerialPort.DtrEnable & mySerialPort.DsrHolding)))
                    if (!CommLineError & !CommDataError & mySerialPort.CtsHolding )
                        mySerialPort.Write(buf, 0, buf.Length); // ｼｰｹﾝｻの送受信ｼｰｹﾝｽをｸﾘｱします

                    functionReturnValue = true;                   
                }
            }
            catch
            {
                // '    Beep
                // '    MsgBox "ｴﾗｰNo：" & Err.Number, , "通信ｴﾗｰ"Nothing
            }

            return functionReturnValue;
	    }
        #endregion

        #region MethodProcess

        /// <summary>
        /// 
        /// </summary>
        private void MethodProcess()
        {
            int i = 0;

            try
            {
                bool _BitWriteFlg = false;
                bool _WordWriteFlg = false;
                bool _StatusReadFlg = false;
                string _BitWriteName = "";
                bool _BitWriteData = false;
                string _WordWriteName = "";
                string _WordWriteData = "";

                lock (gLock)
                {
                    if (WordWriteNo == 0 && BitWriteNo == 0 && StatusReadNo == 0)
                    {
                        stsCommBusy = false;
                    }

                    //ﾋﾞｯﾄﾜｰﾄﾞ書込み処理最優先に変更 by 稲葉 02-09-19
                    if (BitWordType[1] == "Bit" && BitWriteNo != 0)
                    {
                        //削除2014/04/05(検S1)hata
                        ////通信待ち時のｼｰｹﾝｻへのﾋﾞｯﾄ書込み処理
                        //for (i = 0; i < BitWriteNo; i++)
                        //{
                        //    BitWriteName[i] = BitWriteName[i + 1];
                        //    BitWriteData[i] = BitWriteData[i + 1];
                        //}
                        //for (i = 0; i < BitWordWriteNo; i++)
                        //{
                        //    BitWordType[i] = BitWordType[i + 1];
                        //}
                        _BitWriteName=  BitWriteName[1];
                        _BitWriteData = BitWriteData[1];

                        _BitWriteFlg = true;
                    }
                    else if (BitWordType[1] == "Word" && WordWriteNo != 0)
                    {
                        //削除2014/04/05(検S1)hata
                        //通信待ち時のｼｰｹﾝｻへのﾜｰﾄﾞ書込み処理
                        //for (i = 0; i < WordWriteNo; i++)
                        //{
                        //    WordWriteName[i] = WordWriteName[i + 1];
                        //    WordWriteData[i] = WordWriteData[i + 1];
                        //}
                        //for (i = 0; i < BitWordWriteNo; i++)
                        //{
                        //    BitWordType[i] = BitWordType[i + 1];
                        //}
                        _WordWriteName = WordWriteName[1];
                        _WordWriteData = WordWriteData[1];

                        _WordWriteFlg = true;
                    }
                    else if (StatusReadNo != 0)
                    {
                        _StatusReadFlg = true;
                    }

                } 

                if (_BitWriteFlg == true)
                {
                    //変更2014/04/05(検S1)hata
                    //BitWriteExe(BitWriteName[0], BitWriteData[0]);
                    BitWriteExe(_BitWriteName, _BitWriteData);
                    lock (gLock)  
                    {
                        //追加2014/04/05(検S1)hata
                        //通信待ち時のｼｰｹﾝｻへのﾋﾞｯﾄ書込み処理
                        for (i = 0; i < BitWriteNo; i++)
                        {
                            BitWriteName[i] = BitWriteName[i + 1];
                            BitWriteData[i] = BitWriteData[i + 1];
                        }
                        for (i = 0; i < BitWordWriteNo; i++)
                        {
                            BitWordType[i] = BitWordType[i + 1];
                        }
                            
                        BitWriteNo--;   //位置移動 by 稲葉 02-02-28
                        BitWordWriteNo--;

                        //Debug.Print("ﾋﾞｯﾄﾃﾞｰﾀ書込後BitWriteNo = " + BitWriteNo.ToString());    //追加 by 稲葉 02-03-05
                        //Debug.Print("ﾋﾞｯﾄﾃﾞｰﾀ書込後BitWriteName = " + BitWriteName[0]);    //追加 by 稲葉 02-03-05
                        //Debug.Print("ﾋﾞｯﾄﾃﾞｰﾀ書込後BitWriteData = " + BitWriteData[0].ToString()); //追加 by 稲葉 02-03-05

                    }
                }
                else if (_WordWriteFlg == true)
                {
                    //変更2014/04/05(検S1)hata
                    //WordWriteExe(WordWriteName[0], ref WordWriteData[0]);
                    WordWriteExe(_WordWriteName, ref _WordWriteData);

                    lock (gLock)  
                    {
                        //追加2014/04/05(検S1)hata
                        //通信待ち時のｼｰｹﾝｻへのﾜｰﾄﾞ書込み処理
                        for (i = 0; i < WordWriteNo; i++)
                        {
                            WordWriteName[i] = WordWriteName[i + 1];
                            WordWriteData[i] = WordWriteData[i + 1];
                        }
                        for (i = 0; i < BitWordWriteNo; i++)
                        {
                            BitWordType[i] = BitWordType[i + 1];
                        }

                        WordWriteNo--;  //位置移動 by 稲葉 02-02-28
                        BitWordWriteNo--;

                    }
                }
                else if (_StatusReadFlg == true)
                {
                    //通信待ち時のｼｰｹﾝｻへの読込処理
                    //        StatusReadNo = StatusReadNo - 1
                    //        Debug.Print "stsCommBusy2"; stsCommBusy
                    StatusReadExe();
                    lock (gLock)
                    {
                        StatusReadNo--; //位置移動 by 稲葉 02-02-28
                    }
                }
            }
            catch
            {
                // 追加 by 稲葉 05-02-16
                int _CommEndAns;
                bool _RaiseEvent;

                lock (gLock)
                {
                    SeqCommEvent = 700;
                    _CommEndAns = SeqCommEvent;
                    _RaiseEvent = EventProcess(SeqCommEvent);   //ｲﾍﾞﾝﾄ処理
                }
                if (_RaiseEvent == true && OnCommEnd != null) OnCommEnd(_CommEndAns);
            }
        }

        #endregion

        #region ReceiveProcess
        /// <summary>
        /// 受信ﾃﾞｰﾀ処理
        /// </summary>
        private void ReceiveProcess()
        {
            string ReadCommand = null;  //読み出しｺﾏﾝﾄﾞ
            string ResponseData = null; //ﾚｽﾎﾟﾝｽﾃﾞｰﾀ
            string[] BitResponse = null;    //ﾚｽﾎﾟﾝｽﾋﾞｯﾄﾃﾞｰﾀ
            string[] WordResponse = null;   //ﾚｽﾎﾟﾝｽﾜｰﾄﾞﾃﾞｰﾀ

            int rbyte = mySerialPort.BytesToRead;
            char[] buf = new char[rbyte];
            int EndPos;
            int StartPos;

            //Debug.Print("Receive !! " + BitWriteNo.ToString() + " " + WordWriteNo.ToString() + " " + BitWordWriteNo.ToString());

             //追加2014/06/09_hata
            if (CommEndFlg) return;
 
            try
            {
                bool _WordReadFlag = false;

                //追加2015/03/16hata
                //レシーブ時間を取得
                RecvPoint = 0;
                RecvOnTime = DateTime.Now;
                bDatAll = true;

                //変更2015/03/13hata_デットロックのため外す
                //lock (gLock)
                //{

                    //削除y2015/03/16hata
                    ////追加2014/06/09_hata
                    //SerialRead.Reset();
                    
                    //rData += mMSComm.Input;
                    mySerialPort.Read(buf, 0, buf.Length);
                    rData += new string(buf);

                    //Debug.Print("rData " + rData);

                    // 最後まで受信したかをﾁｪｯｸします。
                    EndPos = rData.IndexOf("\n");

                    //削除y2015/03/16hata
                    ////追加2014/06/09_hata
                    //SerialRead.Set();


                    if (EndPos != -1)
                    {
                        //読込ﾚｽﾎﾟﾝｽﾃﾞｰﾀの取得
                        StartPos = rData.IndexOf(Convert.ToChar(0x2));
                        switch (ReadFlag)
                        {
                            case 1:
                                if (StartPos != -1)
                                {
                                    RecvPoint = 10;//追加2015/03/14hata

                                    //変更2015/03/16hata
                                    if (rData.Length >= StartPos + 5 + EndPos + 1 - 8)
                                    //if ((rData.Length >= StartPos + 5) & (rData.Length >= EndPos + 1 - 8))
                                    {
                                        bDatAll = true; //追加2015/03/16hata
                                        ResponseData = rData.Substring((StartPos + 5), (EndPos + 1 - 8));

                                        RecvPoint = 11; //追加2015/03/16hata
    
                                        BitResponse = new string[BitReadNo + 1];
                                        for (int i = 0; i < BitReadNo + 1; i++)
                                        {
                                            BitResponse[i] = ResponseData.Substring(i, 1);
                                        }

                                        RecvPoint = 12; //追加2015/03/16hata

                                        // 各フィールドにセット
                                        PcInhibit = Convert.ToBoolean(Convert.ToInt32(BitResponse[0]));          //ﾘﾓｰﾄ
                                        stsRunReadySW = Convert.ToBoolean(Convert.ToInt32(BitResponse[1]));      //運転準備ｽｲｯﾁ  '追加 by 稲葉 05-11-24
                                        stsDoorInterlock = Convert.ToBoolean(Convert.ToInt32(BitResponse[5]));   //扉ｲﾝﾀｰﾛｯｸ
                                        stsEmergency = Convert.ToBoolean(Convert.ToInt32(BitResponse[6]));       //非常停止
                                        stsXray225Trip = Convert.ToBoolean(Convert.ToInt32(BitResponse[7]));     //X線225KVﾄﾘｯﾌﾟ
                                        stsXray160Trip = Convert.ToBoolean(Convert.ToInt32(BitResponse[8]));     //X線160KVﾄﾘｯﾌﾟ
                                        stsFilterTouch = Convert.ToBoolean(Convert.ToInt32(BitResponse[9]));     //ﾌｨﾙﾀﾕﾆｯﾄ接触
                                        stsXray225Touch = Convert.ToBoolean(Convert.ToInt32(BitResponse[10]));   //X線225KV接触
                                        stsXray160Touch = Convert.ToBoolean(Convert.ToInt32(BitResponse[11]));   //X線160KV接触
                                        stsRotTouch = Convert.ToBoolean(Convert.ToInt32(BitResponse[12]));       //回転ﾃｰﾌﾞﾙ接触
                                        stsTiltTouch = Convert.ToBoolean(Convert.ToInt32(BitResponse[13]));      //傾斜ﾃｰﾌﾞﾙ接触
                                        stsXDriverHeat = Convert.ToBoolean(Convert.ToInt32(BitResponse[14]));    //ﾃｰﾌﾞﾙX軸ｵｰﾊﾞｰﾋｰﾄ
                                        stsYDriverHeat = Convert.ToBoolean(Convert.ToInt32(BitResponse[15]));    //ﾃｰﾌﾞﾙY軸ｵｰﾊﾞｰﾋｰﾄ
                                        stsXrayDriverHeat = Convert.ToBoolean(Convert.ToInt32(BitResponse[16])); //X線管切替ｵｰﾊﾞｰﾋｰﾄ
                                        stsSeqCpuErr = Convert.ToBoolean(Convert.ToInt32(BitResponse[17]));      //ｼｰｹﾝｻCPUｴﾗｰ
                                        stsSeqBatteryErr = Convert.ToBoolean(Convert.ToInt32(BitResponse[18]));  //ｼｰｹﾝｻﾊﾞｯﾃﾘｰｴﾗｰ
                                        stsSeqKzCommErr = Convert.ToBoolean(Convert.ToInt32(BitResponse[19]));   //ｼｰｹﾝｻKL通信ｴﾗｰ(KZ)
                                        stsSeqKvCommErr = Convert.ToBoolean(Convert.ToInt32(BitResponse[20]));   //ｼｰｹﾝｻKL通信ｴﾗｰ(KV)
                                        stsFilterTimeout = Convert.ToBoolean(Convert.ToInt32(BitResponse[21]));  //ﾌｨﾙﾀｰﾀｲﾑｱｳﾄ
                                        stsTiltTimeout = Convert.ToBoolean(Convert.ToInt32(BitResponse[22]));    //傾斜ﾀｲﾑｱｳﾄ//追加 by 稲葉 01-07-10
                                        stsXTimeout = Convert.ToBoolean(Convert.ToInt32(BitResponse[23]));       //ﾃｰﾌﾞﾙX軸ﾀｲﾑｱｳﾄ
                                        stsIIDriverHeat = Convert.ToBoolean(Convert.ToInt32(BitResponse[24]));   //I.I.軸ｵｰﾊﾞｰﾋｰﾄ
                                        stsSeqCounterErr = Convert.ToBoolean(Convert.ToInt32(BitResponse[25]));  //ｼｰｹﾝｻｶｳﾝﾀﾕﾆｯﾄｴﾗｰ
                                        stsXDriveErr = Convert.ToBoolean(Convert.ToInt32(BitResponse[26]));      //X軸動作ｴﾗｰ

                                        stsXLLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[32]));        //ﾃｰﾌﾞﾙX左限
                                        stsXRLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[33]));        //ﾃｰﾌﾞﾙX右限
                                        stsXLeft = Convert.ToBoolean(Convert.ToInt32(BitResponse[34]));          //ﾃｰﾌﾞﾙX左移動中
                                        stsXRight = Convert.ToBoolean(Convert.ToInt32(BitResponse[35]));         //ﾃｰﾌﾞﾙX右移動中
                                        stsXLTouch = Convert.ToBoolean(Convert.ToInt32(BitResponse[36]));        //ﾃｰﾌﾞﾙX左移動中接触
                                        stsXRTouch = Convert.ToBoolean(Convert.ToInt32(BitResponse[37]));        //ﾃｰﾌﾞﾙX右移動中接触
                                        stsYFLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[40]));        //ﾃｰﾌﾞﾙY前進限
                                        stsYBLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[41]));        //ﾃｰﾌﾞﾙY後退限
                                        stsYForward = Convert.ToBoolean(Convert.ToInt32(BitResponse[42]));       //ﾃｰﾌﾞﾙY前進中
                                        stsYBackward = Convert.ToBoolean(Convert.ToInt32(BitResponse[43]));      //ﾃｰﾌﾞﾙY後退中
                                        stsYFTouch = Convert.ToBoolean(Convert.ToInt32(BitResponse[44]));        //ﾃｰﾌﾞﾙY前進中接触
                                        stsYBTouch = Convert.ToBoolean(Convert.ToInt32(BitResponse[45]));        //ﾃｰﾌﾞﾙY後退中接触
                                        stsIIFLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[48]));       //I.I.前進限
                                        stsIIBLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[49]));       //I.I.後退限
                                        stsIIForward = Convert.ToBoolean(Convert.ToInt32(BitResponse[50]));      //I.I.前進中
                                        stsIIBackward = Convert.ToBoolean(Convert.ToInt32(BitResponse[51]));     //I.I.後退中
                                        stsTableMovePermit = Convert.ToBoolean(Convert.ToInt32(BitResponse[53]));//ﾃｰﾌﾞﾙｲﾝﾀｰﾛｯｸ解除
                                        stsMechaPermit = Convert.ToBoolean(Convert.ToInt32(BitResponse[54]));    //ﾃｰﾌﾞﾙ回転、上昇、微調動作禁止
                                        stsUpLimitPermit = Convert.ToBoolean(Convert.ToInt32(BitResponse[55]));  //ﾃｰﾌﾞﾙ上昇制限解除  '追加 by 稲葉 16-03-10
                                        stsTiltCwLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[56]));    //傾斜CW限
                                        stsTiltCCwLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[57]));   //傾斜CCW限
                                        stsTiltOrigin = Convert.ToBoolean(Convert.ToInt32(BitResponse[58]));     //傾斜原点（水平）
                                        stsTiltCw = Convert.ToBoolean(Convert.ToInt32(BitResponse[59]));         //傾斜CW中
                                        stsTiltCCw = Convert.ToBoolean(Convert.ToInt32(BitResponse[60]));        //傾斜CCW中
                                        stsTiltOriginRun = Convert.ToBoolean(Convert.ToInt32(BitResponse[61]));  //傾斜原点復帰中

                                        stsColliLOLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[64]));   //ｺﾘﾒｰﾀ左開限
                                        stsColliLCLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[65]));   //ｺﾘﾒｰﾀ左閉限
                                        stsColliROLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[66]));   //ｺﾘﾒｰﾀ右開限
                                        stsColliRCLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[67]));   //ｺﾘﾒｰﾀ右閉限
                                        stsColliUOLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[68]));   //ｺﾘﾒｰﾀ上開限
                                        stsColliUCLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[69]));   //ｺﾘﾒｰﾀ上閉限
                                        stsColliDOLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[70]));   //ｺﾘﾒｰﾀ下開限
                                        stsColliDCLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[71]));   //ｺﾘﾒｰﾀ下閉限
                                        stsColliLOpen = Convert.ToBoolean(Convert.ToInt32(BitResponse[72]));     //ｺﾘﾒｰﾀ左開中
                                        stsColliLClose = Convert.ToBoolean(Convert.ToInt32(BitResponse[73]));    //ｺﾘﾒｰﾀ左閉中
                                        stsColliROpen = Convert.ToBoolean(Convert.ToInt32(BitResponse[74]));     //ｺﾘﾒｰﾀ右開中
                                        stsColliRClose = Convert.ToBoolean(Convert.ToInt32(BitResponse[75]));    //ｺﾘﾒｰﾀ右閉中
                                        stsColliUOpen = Convert.ToBoolean(Convert.ToInt32(BitResponse[76]));     //ｺﾘﾒｰﾀ上開中
                                        stsColliUClose = Convert.ToBoolean(Convert.ToInt32(BitResponse[77]));    //ｺﾘﾒｰﾀ上閉中
                                        stsColliDOpen = Convert.ToBoolean(Convert.ToInt32(BitResponse[78]));     //ｺﾘﾒｰﾀ下開中
                                        stsColliDClose = Convert.ToBoolean(Convert.ToInt32(BitResponse[79]));    //ｺﾘﾒｰﾀ下閉中
                                        stsFilter0 = Convert.ToBoolean(Convert.ToInt32(BitResponse[80]));        //ﾌｨﾙﾀ無し位置
                                        stsFilter1 = Convert.ToBoolean(Convert.ToInt32(BitResponse[81]));        //ﾌｨﾙﾀ1位置
                                        stsFilter2 = Convert.ToBoolean(Convert.ToInt32(BitResponse[82]));        //ﾌｨﾙﾀ2位置
                                        stsFilter3 = Convert.ToBoolean(Convert.ToInt32(BitResponse[83]));        //ﾌｨﾙﾀ3位置
                                        stsFilter4 = Convert.ToBoolean(Convert.ToInt32(BitResponse[84]));        //ﾌｨﾙﾀ4位置
                                        stsFilter5 = Convert.ToBoolean(Convert.ToInt32(BitResponse[85]));        //ﾌｨﾙﾀ5位置
                                        stsFilter0Run = Convert.ToBoolean(Convert.ToInt32(BitResponse[86]));     //ﾌｨﾙﾀ無し動作中
                                        stsFilter1Run = Convert.ToBoolean(Convert.ToInt32(BitResponse[87]));     //ﾌｨﾙﾀ1動作中
                                        stsFilter2Run = Convert.ToBoolean(Convert.ToInt32(BitResponse[88]));     //ﾌｨﾙﾀ2動作中
                                        stsFilter3Run = Convert.ToBoolean(Convert.ToInt32(BitResponse[89]));     //ﾌｨﾙﾀ3動作中
                                        stsFilter4Run = Convert.ToBoolean(Convert.ToInt32(BitResponse[90]));     //ﾌｨﾙﾀ4動作中
                                        stsFilter5Run = Convert.ToBoolean(Convert.ToInt32(BitResponse[91]));     //ﾌｨﾙﾀ5動作中
                                        XrayOn = Convert.ToBoolean(Convert.ToInt32(BitResponse[94]));            //X線ON
                                        XrayOff = Convert.ToBoolean(Convert.ToInt32(BitResponse[95]));           //X線OFF
                                        XStgLeft = Convert.ToBoolean(Convert.ToInt32(BitResponse[96]));          //ｽﾃｰｼﾞX左移動
                                        XStgRight = Convert.ToBoolean(Convert.ToInt32(BitResponse[97]));         //ｽﾃｰｼﾞX右移動
                                        XStgOrigin = Convert.ToBoolean(Convert.ToInt32(BitResponse[98]));        //ｽﾃｰｼﾞX原点復帰
                                        YStgForward = Convert.ToBoolean(Convert.ToInt32(BitResponse[99]));       //ｽﾃｰｼﾞY前進
                                        YStgBackward = Convert.ToBoolean(Convert.ToInt32(BitResponse[100]));     //ｽﾃｰｼﾞY後退
                                        YStgOrigin = Convert.ToBoolean(Convert.ToInt32(BitResponse[101]));       //ｽﾃｰｼﾞY原点復帰
                                        RotIndex = Convert.ToBoolean(Convert.ToInt32(BitResponse[103]));         //回転位置決め要求
                                        stsII9 = Convert.ToBoolean(Convert.ToInt32(BitResponse[104]));           //I.I.視野9”
                                        stsII6 = Convert.ToBoolean(Convert.ToInt32(BitResponse[105]));           //I.I.視野6”
                                        stsII4 = Convert.ToBoolean(Convert.ToInt32(BitResponse[106]));           //I.I.視野4.5”
                                        stsIIPower = Convert.ToBoolean(Convert.ToInt32(BitResponse[107]));       //I.I.電源
                                        stsSLight = Convert.ToBoolean(Convert.ToInt32(BitResponse[108]));        //ｽﾗｲｽﾗｲﾄ
                                        UpDownIndex = Convert.ToBoolean(Convert.ToInt32(BitResponse[109]));      //昇降位置決め要求
                                        XStageIndex = Convert.ToBoolean(Convert.ToInt32(BitResponse[110]));      //微調X位置決め要求
                                        YStageIndex = Convert.ToBoolean(Convert.ToInt32(BitResponse[111]));      //微調Y位置決め要求
                                        DeviceErrReset = Convert.ToBoolean(Convert.ToInt32(BitResponse[112]));   //ﾒｶﾃﾞﾊﾞｲｽｴﾗｰﾘｾｯﾄ
                                        UdOrigin = Convert.ToBoolean(Convert.ToInt32(BitResponse[113]));         //昇降原点復帰
                                        RotOrigin = Convert.ToBoolean(Convert.ToInt32(BitResponse[114]));        //回転原点復帰
                                        stsRotXChange = Convert.ToBoolean(Convert.ToInt32(BitResponse[117]));    //回転中心校正ﾃｰﾌﾞﾙX移動有り
                                        stsDisXChange = Convert.ToBoolean(Convert.ToInt32(BitResponse[118]));    //寸法校正ﾃｰﾌﾞﾙX移動有り
                                        stsRotYChange = Convert.ToBoolean(Convert.ToInt32(BitResponse[119]));    //回転中心校正ﾃｰﾌﾞﾙY移動有り
                                        stsDisYChange = Convert.ToBoolean(Convert.ToInt32(BitResponse[120]));    //寸法校正ﾃｰﾌﾞﾙY移動有り
                                        stsVerIIChange = Convert.ToBoolean(Convert.ToInt32(BitResponse[121]));   //幾何歪校正I.I.移動有り
                                        stsRotIIChange = Convert.ToBoolean(Convert.ToInt32(BitResponse[122]));   //回転中心校正I.I.移動有り
                                        stsGainIIChange = Convert.ToBoolean(Convert.ToInt32(BitResponse[123]));  //ｹﾞｲﾝ校正I.I.移動有り
                                        stsDisIIChange = Convert.ToBoolean(Convert.ToInt32(BitResponse[124]));   //寸法校正I.I.移動有り
                                        stsSPIIChange = Convert.ToBoolean(Convert.ToInt32(BitResponse[125]));    //ｽｷｬﾝ位置校正I.I.移動有り '追加 by 稲葉 05-10-21
    #if conKvMode1 || conKvMode2
                                        stsXrayXErr = Convert.ToBoolean(Convert.ToInt32(BitResponse[27]));       //Ｘ線管X軸動作ｴﾗｰ
                                        stsXrayYErr = Convert.ToBoolean(Convert.ToInt32(BitResponse[28]));       //Ｘ線管Y軸動作ｴﾗｰ
                                        stsXrayRotErr = Convert.ToBoolean(Convert.ToInt32(BitResponse[29]));     //Ｘ線管回転動作ｴﾗｰ
                                        stsAirConOverFlow = Convert.ToBoolean(Convert.ToInt32(BitResponse[30])); //計測CT用空調機水ｵｰﾊﾞｰﾌﾛｰ 追加 by 稲葉 15-12-14
                                        TiltOrigin = Convert.ToBoolean(Convert.ToInt32(BitResponse[115]));       //ﾁﾙﾄﾃｰﾌﾞﾙ　ﾁﾙﾄ原点復帰 追加 by 稲葉 15-07-24
                                        TiltRotOrigin = Convert.ToBoolean(Convert.ToInt32(BitResponse[116]));    //ﾁﾙﾄﾃｰﾌﾞﾙ　回転原点復帰 追加 by 稲葉 15-07-24
                                        stsMechaRstBusy = Convert.ToBoolean(Convert.ToInt32(BitResponse[126]));  //ﾒｶﾘｾｯﾄ中          '追加 by 稲葉 10-09-10
                                        stsMechaRstOK = Convert.ToBoolean(Convert.ToInt32(BitResponse[127]));    //ﾒｶﾘｾｯﾄ完了        '追加 by 稲葉 10-09-10
                                        stsXrayXLLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[128]));   //X線管X軸左限
                                        stsXrayXRLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[129]));   //X線管X軸右限
                                        stsXrayXL = Convert.ToBoolean(Convert.ToInt32(BitResponse[130]));        //X線管X軸左移動中
                                        stsXrayXR = Convert.ToBoolean(Convert.ToInt32(BitResponse[131]));        //X線管X軸右移動中
                                        stsRotXrayXCh = Convert.ToBoolean(Convert.ToInt32(BitResponse[132]));    //回転中心校正X線管X軸移動有り
                                        stsDisXrayXCh = Convert.ToBoolean(Convert.ToInt32(BitResponse[133]));    //寸法校正X線管X軸移動有り
                                        stsXrayYFLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[138]));   //X線管Y軸前進限
                                        stsXrayYBLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[139]));   //X線管Y軸後退限
                                        stsXrayYF = Convert.ToBoolean(Convert.ToInt32(BitResponse[140]));        //X線管Y軸前進移動中
                                        stsXrayYB = Convert.ToBoolean(Convert.ToInt32(BitResponse[141]));        //X線管Y軸後退移動中
                                        stsRotXrayYCh = Convert.ToBoolean(Convert.ToInt32(BitResponse[142]));    //回転中心校正X線管Y軸移動有り
                                        stsDisXrayYCh = Convert.ToBoolean(Convert.ToInt32(BitResponse[143]));    //寸法校正X線管Y軸移動有り
                                        stsXrayCWLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[148]));   //X線管正転限
                                        stsXrayCCWLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[149]));  //X線管逆転限
                                        stsXrayCW = Convert.ToBoolean(Convert.ToInt32(BitResponse[150]));        //X線管正転中
                                        stsXrayCCW = Convert.ToBoolean(Convert.ToInt32(BitResponse[151]));       //X線管逆転中
                                        stsXrayRotLock = Convert.ToBoolean(Convert.ToInt32(BitResponse[152]));   //X線管回転動作不可
                                        stsDoorKey = Convert.ToBoolean(Convert.ToInt32(BitResponse[92]));        //試料扉電磁ﾛｯｸｷｰ挿入完了
                                        stsDoorLock = Convert.ToBoolean(Convert.ToInt32(BitResponse[93]));       //試料扉電磁ﾛｯｸON中
                                        stsEXMOn = Convert.ToBoolean(Convert.ToInt32(BitResponse[153]));         //X線EXM ON中
                                        stsEXMReady = Convert.ToBoolean(Convert.ToInt32(BitResponse[154]));      //X線EXM ON可能
                                        stsEXMNormal1 = Convert.ToBoolean(Convert.ToInt32(BitResponse[155]));    //X線EXM発生装置正常
                                        stsEXMNormal2 = Convert.ToBoolean(Convert.ToInt32(BitResponse[156]));    //X線EXMﾃﾞｰﾀ書込正常
                                        stsEXMWU = Convert.ToBoolean(Convert.ToInt32(BitResponse[157]));         //X線EXMｳｫｰﾑｱｯﾌﾟ必要又は実行中
                                        stsEXMRemote = Convert.ToBoolean(Convert.ToInt32(BitResponse[158]));     //X線EXMﾘﾓｰﾄ中//追加 by 稲葉 10-02-03

                                        // ﾏｲｸﾛCT用
                                        stsCTIIPos = Convert.ToBoolean(Convert.ToInt32(BitResponse[160]));       //CT用I.I.撮影位置
                                        stsTVIIPos = Convert.ToBoolean(Convert.ToInt32(BitResponse[161]));       //高速用I.I.撮影位置
                                        stsCTIIDrive = Convert.ToBoolean(Convert.ToInt32(BitResponse[162]));     //CT用I.I.切替中
                                        stsTVIIDrive = Convert.ToBoolean(Convert.ToInt32(BitResponse[163]));     //高速用I.I.切替中
                                        stsTVII9 = Convert.ToBoolean(Convert.ToInt32(BitResponse[164]));         //高速用I.I.視野9"
                                        stsTVII6 = Convert.ToBoolean(Convert.ToInt32(BitResponse[165]));         //高速用I.I.視野6"
                                        stsTVII4 = Convert.ToBoolean(Convert.ToInt32(BitResponse[166]));         //高速用I.I.視野4.5"
                                        stsTVIIPower = Convert.ToBoolean(Convert.ToInt32(BitResponse[167]));     //高速用I.I.電源
                                        // 計測CT用　 追加 by 稲葉 15-07-24
                                        stsMicroFPDPos = Convert.ToBoolean(Convert.ToInt32(BitResponse[160]));          //ﾏｲｸﾛﾌｫｰｶｽX線検出器位置
                                        stsNanoFPDPos = Convert.ToBoolean(Convert.ToInt32(BitResponse[161]));           //ﾅﾉﾌｫｰｶｽX線検出器位置
                                        stsMicroFPDShiftPos = Convert.ToBoolean(Convert.ToInt32(BitResponse[162]));     //ﾏｲｸﾛﾌｫｰｶｽX線検出器ｼﾌﾄ位置
                                        stsNanoFPDShiftPos = Convert.ToBoolean(Convert.ToInt32(BitResponse[163]));      //ﾅﾉﾌｫｰｶｽX線検出器ｼﾌﾄ位置
                                        stsMicroFPDBusy = Convert.ToBoolean(Convert.ToInt32(BitResponse[164]));  //ﾏｲｸﾛﾌｫｰｶｽX線切替中
                                        stsNanoFPDBusy = Convert.ToBoolean(Convert.ToInt32(BitResponse[165]));   //ﾅﾉﾌｫｰｶｽX線切替中
                                        stsMicroFPDShiftBusy = Convert.ToBoolean(Convert.ToInt32(BitResponse[166]));    //ﾏｲｸﾛﾌｫｰｶｽX線検出器ｼﾌﾄ中
                                        stsNanoFPDShiftBusy = Convert.ToBoolean(Convert.ToInt32(BitResponse[167]));     //ﾅﾉﾌｫｰｶｽX線検出器ｼﾌﾄ中

                                        stsCameraPower = Convert.ToBoolean(Convert.ToInt32(BitResponse[168]));   //高速ｶﾒﾗ電源
                                        // AVFD用　 追加 by 稲葉 15-12-14
                                        stsFPDLShiftPos = Convert.ToBoolean(Convert.ToInt32(BitResponse[169]));  //FPD左ｼﾌﾄ位置
                                        stsFPDLShiftBusy = Convert.ToBoolean(Convert.ToInt32(BitResponse[170])); //FPD左ｼﾌﾄ位置移動中
                                        stsFDSystemPos = Convert.ToBoolean(Convert.ToInt32(BitResponse[171]));   //FDｼｽﾃﾑ位置移動中
                                        stsFDSystemBusy = Convert.ToBoolean(Convert.ToInt32(BitResponse[172]));  //FDｼｽﾃﾑ位置

                                        stsColdBoxPosOK = Convert.ToBoolean(Convert.ToInt32(BitResponse[174]));  //冷蔵箱正規位置 追加 by 稲葉 15-03-12
                                        stsColdBoxDoorClose = Convert.ToBoolean(Convert.ToInt32(BitResponse[175])); //冷蔵箱扉閉 追加 by 稲葉 15-03-12
                                        stsIrisLOpen = Convert.ToBoolean(Convert.ToInt32(BitResponse[176]));     //I.I.絞り左開中
                                        stsIrisLClose = Convert.ToBoolean(Convert.ToInt32(BitResponse[177]));    //I.I.絞り左閉中
                                        stsIrisROpen = Convert.ToBoolean(Convert.ToInt32(BitResponse[178]));     //I.I.絞り右開中
                                        stsIrisRClose = Convert.ToBoolean(Convert.ToInt32(BitResponse[179]));    //I.I.絞り右閉中
                                        stsIrisUOpen = Convert.ToBoolean(Convert.ToInt32(BitResponse[180]));     //I.I.絞り上開中
                                        stsIrisUClose = Convert.ToBoolean(Convert.ToInt32(BitResponse[181]));    //I.I.絞り上閉中
                                        stsIrisDOpen = Convert.ToBoolean(Convert.ToInt32(BitResponse[182]));     //I.I.絞り下開中
                                        stsIrisDClose = Convert.ToBoolean(Convert.ToInt32(BitResponse[183]));    //I.I.絞り下閉中
                                        stsAutoRestrict = Convert.ToBoolean(Convert.ToInt32(BitResponse[184]));  //動作制限自動復帰設定状態 '追加 by 稲葉 10-10-19
                                        stsYIndexSlow = Convert.ToBoolean(Convert.ToInt32(BitResponse[185]));    //Y軸ｲﾝﾃﾞｯｸｽ減速設定状態     '追加 by 稲葉 10-10-19
                                        stsDoorPermit = Convert.ToBoolean(Convert.ToInt32(BitResponse[186]));    //動作用扉ｲﾝﾀｰﾛｯｸ設定状態    '追加 by 稲葉 10-10-19
                                        stsShutter = Convert.ToBoolean(Convert.ToInt32(BitResponse[187]));       //ｼｬｯﾀ位置       '追加 by 稲葉 10-11-22
                                        stsShutterBusy = Convert.ToBoolean(Convert.ToInt32(BitResponse[188]));   //ｼｬｯﾀ動作中    '追加 by 稲葉 10-11-22
                                        stsRotLargeTable = Convert.ToBoolean(Convert.ToInt32(BitResponse[189])); //回転大ﾃｰﾌﾞﾙ有無    '追加 by 稲葉 14-02-26 //2014/10/06_v1951反映
                                        stsRoomInSw = Convert.ToBoolean(Convert.ToInt32(BitResponse[190]));      //検査室入室安全ｽｲｯﾁ '追加 by 稲葉 14-03-05 //2014/10/06_v1951反映
                                        // 電池検査用　 追加 by 稲葉 16-04-14
                                        stsColiUErr = Convert.ToBoolean(Convert.ToInt32(BitResponse[128]));         //ｺﾘﾒｰﾀ上異常
                                        stsColiDErr = Convert.ToBoolean(Convert.ToInt32(BitResponse[129]));         //ｺﾘﾒｰﾀ下異常
                                        stsColiUOriginOK = Convert.ToBoolean(Convert.ToInt32(BitResponse[130]));    //ｺﾘﾒｰﾀ上原点復帰完了
                                        stsColiDOriginOK = Convert.ToBoolean(Convert.ToInt32(BitResponse[131]));    //ｺﾘﾒｰﾀ下原点復帰完了
                                        stsWorkTurningHPos = Convert.ToBoolean(Convert.ToInt32(BitResponse[132]));  //ﾜｰｸ反転水平0°位置
                                        stsWorkTurningPPos = Convert.ToBoolean(Convert.ToInt32(BitResponse[133]));  //ﾜｰｸ反転水平90°位置
                                        stsWorkTurningNPos = Convert.ToBoolean(Convert.ToInt32(BitResponse[134]));  //ﾜｰｸ反転水平-90°位置
                                        stsManualOpeMode = Convert.ToBoolean(Convert.ToInt32(BitResponse[135]));    //手動運転ﾓｰﾄﾞ
                                        stsAutoOpeMode = Convert.ToBoolean(Convert.ToInt32(BitResponse[136]));      //自動運転ﾓｰﾄﾞ
                                        stsTeachingOpeMode = Convert.ToBoolean(Convert.ToInt32(BitResponse[137]));  //ﾃｨｰﾁﾝｸﾞ運転ﾓｰﾄﾞ
                                        stsAutoOpeBusy = Convert.ToBoolean(Convert.ToInt32(BitResponse[138]));      //自動運転中
                                        stsTeachingOpeBusy = Convert.ToBoolean(Convert.ToInt32(BitResponse[139]));  //ﾃｨｰﾁﾝｸﾞ運転中
                                        ScanStartReq = Convert.ToBoolean(Convert.ToInt32(BitResponse[140]));        //ｽｷｬﾝ(ﾃｨｰﾁﾝｸﾞ）開始要求
                                        ScanStopReq = Convert.ToBoolean(Convert.ToInt32(BitResponse[141]));         //ｽｷｬﾝ(ﾃｨｰﾁﾝｸﾞ）停止要求
                                        TimeSetReq = Convert.ToBoolean(Convert.ToInt32(BitResponse[142]));          //時刻設定要求
                                        ErrResetReq = Convert.ToBoolean(Convert.ToInt32(BitResponse[143]));         //異常ﾘｾｯﾄ要求
                                        XrayWarmupReq = Convert.ToBoolean(Convert.ToInt32(BitResponse[144]));       //X線ｳｫｰﾑｱｯﾌﾟ要求
                                        stsWorkTurningOriginOK = Convert.ToBoolean(Convert.ToInt32(BitResponse[145]));  //ﾜｰｸ反転原点復帰完了
                                        stsCTError = Convert.ToBoolean(Convert.ToInt32(BitResponse[146]));          //CT装置異常
                                        stsAreaSensorDark = Convert.ToBoolean(Convert.ToInt32(BitResponse[147]));   //試料扉ｴﾘｱｾﾝｻ状態
                                        // FDZ2+高速度カメラ用　 追加 by 稲葉 19-03-04
                                        stsXrayCameraUDError = Convert.ToBoolean(Convert.ToInt32(BitResponse[148]));    //X線/ｶﾒﾗ昇降異常
                                        stsXrayCameraUDBusy = Convert.ToBoolean(Convert.ToInt32(BitResponse[149]));     //X線/ｶﾒﾗ昇降動作中
                                        stsXrayCameraUpperLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[150])); //X線/ｶﾒﾗ上昇限
                                        stsXrayCameraLowerLimit = Convert.ToBoolean(Convert.ToInt32(BitResponse[151])); //X線/ｶﾒﾗ下降限
#endif

#if conKvMode2
						                stsXOrgReq = Convert.ToBoolean(Convert.ToInt32(BitResponse[134]));       //ﾃｰﾌﾞﾙ左右原点復帰要求
						                stsYOrgReq = Convert.ToBoolean(Convert.ToInt32(BitResponse[135]));       //ﾃｰﾌﾞﾙ前後原点復帰要求
						                stsIIOrgReq = Convert.ToBoolean(Convert.ToInt32(BitResponse[136]));      //検出器前後原点復帰要求
						                stsIIChgOrgReq = Convert.ToBoolean(Convert.ToInt32(BitResponse[137]));   //検出器切替原点復帰要求
#endif

                                        //追加2015/03/16hata
                                        RecvPoint = 13;

                                    }
                                    else
                                    {
                                        
                                        //Dataが足りない
                                        RecvPoint = 14;
                                        //LogWrite("NoAll");

                                    }

                                    //ﾋﾞｯﾄﾃﾞﾊﾞｲｽﾃﾞｰﾀの読み出し
                                    // 受信ﾊﾞｯﾌｧをｸﾘｱします
                                    mySerialPort.DiscardInBuffer();

                                    // 送信ﾊﾞｯﾌｧをｸﾘｱします
                                    mySerialPort.DiscardOutBuffer();

                                    rData = "";
                                    StartPos = -1;

                                    //ﾜｰﾄﾞ読み出しｺﾏﾝﾄﾞを送信します。
                                    ReadFlag = 2;

                                    //エラーするのでThresholdサイズを変えない　//2014/04/23(検S1)hata
                                    //mySerialPort.ReceivedBytesThreshold = (WordReadNo + 1) * 4 + 8;

                                    // ﾜｰﾄﾞ数(Hex)
                                    string _WordNo = Convert.ToString(WordReadNo + 1, 16).PadLeft(2, '0');

                                    //読み出しｺﾏﾝﾄﾞ = ENQ + 局番 + PC番号 + ｺﾏﾝﾄﾞ + ｳｴｲﾄ + 先頭ﾃﾞﾊﾞｲｽ + ﾜｰﾄﾞ数(Hex) + CR + LF
#if conKvMode1
                                    ReadCommand = Convert.ToChar(0x5) + "00" + "FF" + "QR" + "0" + "D019000" + _WordNo + "\r\n";
#elif conKvMode2
						            ReadCommand = Convert.ToChar(0x5) + "00" + "FF" + "QR" + "0" + "D007000" + _WordNo + "\r\n";
#else
						            ReadCommand = Convert.ToChar(0x5) + "00" + "FF" + "QR" + "0" + "W000000" + _WordNo + "\r\n";
#endif
                                    //追加2015/03/16hata
                                    // CTSﾗｲﾝｴﾗｰﾁｪｯｸ
                                    DateTime ntim = DateTime.Now;
                                    while (!mySerialPort.CtsHolding)
                                    {
                                        if (((DateTime.Now - ntim).TotalSeconds > 2000))
                                        {
                                            //Logを取る
                                            //LogWrite("CtsHolding=false");
                                            break;
                                        }
                                        Thread.Sleep(1);
                                    }

                                    //追加2015/03/16hata
                                    RecvPoint = 15;

                                    mySerialPort.Write(ReadCommand);

                                    //追加2015/03/16hata
                                    // 通信ﾀｲﾑｱｳﾄ時間を設定します。
                                    TimeOutSet();

                                    //追加2015/03/16hata
                                    RecvPoint = 16;
                                   
                                }
                                break;

                            case 2:

                                //_WordReadFlag = true;   //変更2015/03/16hata

                                if (StartPos != -1)
                                {
                                    RecvPoint = 20;//追加2015/03/16hata

                                    _WordReadFlag = true;   //追加2015/03/16hata

                                    if (rData.Length >= StartPos + 5 + EndPos + 1 - 8)
                                    {
                                        bDatAll = true; //追加2015/03/16hata
                                        ResponseData = rData.Substring((StartPos + 5), (EndPos + 1 - 8));

                                        RecvPoint = 21;//追加2015/03/14hata

                                        WordResponse = new string[WordReadNo + 1];
                                        for (int i = 0; i < WordReadNo + 1; i++)
                                        {
                                            WordResponse[i] = ResponseData.Substring(i * 4, 4);
                                        }
                                       
                                        RecvPoint = 22;     //追加2015/03/13hata
 
                                        stsFID = Convert.ToInt32("0x" + WordResponse[1] + WordResponse[0], 16);             //FID
                                        stsFCD = Convert.ToInt32("0x" + WordResponse[3] + WordResponse[2], 16);             //FCD
                                        stsXMinSpeed = Convert.ToInt32("0x" + WordResponse[5], 16);                         //ﾃｰﾌﾞﾙX最低速度
                                        stsXMaxSpeed = Convert.ToInt32("0x" + WordResponse[6], 16);                         //ﾃｰﾌﾞﾙX最高速度
                                        stsXSpeed = Convert.ToInt32("0x" + WordResponse[7], 16);                            //ﾃｰﾌﾞﾙX運転速度
                                        stsYMinSpeed = Convert.ToInt32("0x" + WordResponse[9], 16);                         //ﾃｰﾌﾞﾙY最低速度
                                        stsYMaxSpeed = Convert.ToInt32("0x" + WordResponse[10], 16);                        //ﾃｰﾌﾞﾙY最高速度
                                        stsYSpeed = Convert.ToInt32("0x" + WordResponse[11], 16);                           //ﾃｰﾌﾞﾙY運転速度
                                        stsFcdDecelerationSpeed = Convert.ToInt32("0x" + WordResponse[12], 16);             //FCD減速速度   追加 by 稲葉 18-01-15
                                        stsXPosition = Convert.ToInt32("0x" + WordResponse[14] + WordResponse[13], 16);     //ﾃｰﾌﾞﾙX現在値
                                        stsIIMinSpeed = Convert.ToInt32("0x" + WordResponse[16], 16);                       //I.I.最低速度
                                        stsIIMaxSpeed = Convert.ToInt32("0x" + WordResponse[17], 16);                       //I.I.最高速度
                                        stsIISpeed = Convert.ToInt32("0x" + WordResponse[18], 16);                          //I.I.運転速度
                                        stsUDSpeed = Convert.ToInt32("0x" + WordResponse[19], 16);                          //昇降手動運転速度
                                        stsUDIndexPos = Convert.ToInt32("0x" + WordResponse[21] + WordResponse[20], 16);    //昇降位置決め位置
                                        stsRotSpeed = Convert.ToInt32("0x" + WordResponse[22], 16);                         //回転手動運転速度
                                        stsRotIndexPos = Convert.ToInt32("0x" + WordResponse[24] + WordResponse[23], 16);   //回転位置決め位置
                                        stsXStgSpeed = Convert.ToInt32("0x" + WordResponse[25], 16);                        //微調X手動運転速度
                                        stsXStgIndexPos = Convert.ToInt32("0x" + WordResponse[26], 16);                     //微調X位置決め位置
                                        stsYStgSpeed = Convert.ToInt32("0x" + WordResponse[27], 16);                        //微調Y手動運転速度
                                        stsYStgIndexPos = Convert.ToInt32("0x" + WordResponse[28], 16);                     //微調Y位置決め位置
#if conKvMode1 || conKvMode2
                                        stsFCDLimitAdj = Convert.ToInt32("0x" + WordResponse[4], 16);                       //FCD軸ｲﾝﾀｰﾛｯｸﾘﾐｯﾄ位置補正 追加 by 稲葉 15-07-24
                                        stsYPosition = Convert.ToInt32("0x" + WordResponse[30] + WordResponse[29], 16);     //ﾃｰﾌﾞﾙY現在値

                                        stsXrayFCD = Convert.ToInt32("0x" + WordResponse[33] + WordResponse[32], 16);       //X線管FCD
                                        stsLinearFDD = Convert.ToInt32("0x" + WordResponse[33] + WordResponse[32], 16);     //FDDﾘﾆｱｽｹｰﾙ値 追加 by 稲葉 15-07-24

                                        stsXrayXMinSp = Convert.ToInt32("0x" + WordResponse[34], 16);                       //X線管X軸最低速度
                                        stsXrayXMaxSp = Convert.ToInt32("0x" + WordResponse[35], 16);                       //X線管X軸最高速度
                                        stsXrayXSpeed = Convert.ToInt32("0x" + WordResponse[36], 16);                       //X線管X軸運転速度

                                        stsXrayXPos = Convert.ToInt32("0x" + WordResponse[38] + WordResponse[37], 16);      //X線管X軸現在位置
                                        stsLinearTableY = Convert.ToInt32("0x" + WordResponse[38] + WordResponse[37], 16);  //ﾃｰﾌﾞﾙYﾘﾆｱｽｹｰﾙ値 追加 by 稲葉 15-07-24

                                        stsEXMLimitTV = Convert.ToInt32("0x" + WordResponse[41], 16);                       //X線EXM制限管電圧値
                                        stsEXMLimitTC = Convert.ToInt32("0x" + WordResponse[42], 16);                       //X線EXM制限管電流値

                                        stsXrayYMinSp = Convert.ToInt32("0x" + WordResponse[43], 16);                       //X線管Y軸最低速度
                                        stsXrayYMaxSp = Convert.ToInt32("0x" + WordResponse[44], 16);                       //X線管Y軸最高速度
                                        stsXrayYSpeed = Convert.ToInt32("0x" + WordResponse[45], 16);                       //X線管Y軸運転速度

                                        stsXrayYPos = Convert.ToInt32("0x" + WordResponse[47] + WordResponse[46], 16);      //X線管Y軸現在位置
                                        stsLinearFCD = Convert.ToInt32("0x" + WordResponse[47] + WordResponse[46], 16);     //FCDﾘﾆｱｽｹｰﾙ値 追加 by 稲葉 15-07-24

                                        stsEXMTVSet = Convert.ToInt32("0x" + WordResponse[48], 16);                         //X線EXM管電圧設定値
                                        stsXrayHOffTimeY = Convert.ToInt32("0x" + WordResponse[48], 16);                    //AVｼｽﾃﾑ管電圧H X線OFF時刻 年 追加 by 稲葉 16-01-13

                                        stsEXMTCSet = Convert.ToInt32("0x" + WordResponse[49], 16);                         //X線EXM管電流設定値
                                        stsXrayHOffTimeMD = Convert.ToInt32("0x" + WordResponse[49], 16);                   //AVｼｽﾃﾑ管電圧H X線OFF時刻 月日 追加 by 稲葉 16-01-13

                                        stsEXMTV = Convert.ToInt32("0x" + WordResponse[50], 16);                            //X線EXM管電圧実測値
                                        stsXrayHOffTimeHM = Convert.ToInt32("0x" + WordResponse[50], 16);                   //AVｼｽﾃﾑ管電圧H X線OFF時刻 時分 追加 by 稲葉 16-01-13

                                        stsEXMTC = Convert.ToInt32("0x" + WordResponse[51], 16);                            //X線EXM管電流実測値
                                        stsXrayMOffTimeY = Convert.ToInt32("0x" + WordResponse[51], 16);                    //AVｼｽﾃﾑ管電圧M X線OFF時刻 年 追加 by 稲葉 16-01-13

                                        stsXrayRotMinSp = Convert.ToInt32("0x" + WordResponse[52], 16);                     //X線管回転最低速度
                                        stsTiltSpeed = Convert.ToInt32("0x" + WordResponse[52], 16);                        //ﾁﾙﾄﾃｰﾌﾞﾙ　ﾁﾙﾄ手動速度 追加 by 稲葉 15-07-24
                                        stsXrayMOffTimeMD = Convert.ToInt32("0x" + WordResponse[52], 16);                   //AVｼｽﾃﾑ管電圧M X線OFF時刻 月日 追加 by 稲葉 16-01-13

                                        stsXrayRotMaxSp = Convert.ToInt32("0x" + WordResponse[53], 16);                     //X線管回転最高速度
                                        stsTiltRotSpeed = Convert.ToInt32("0x" + WordResponse[53], 16);                     //ﾁﾙﾄﾃｰﾌﾞﾙ　回転手動速度 追加 by 稲葉 15-07-24
                                        stsXrayMOffTimeHM = Convert.ToInt32("0x" + WordResponse[53], 16);                   //AVｼｽﾃﾑ管電圧M X線OFF時刻 時分 追加 by 稲葉 16-01-13

                                        stsXrayRotSpeed = Convert.ToInt32("0x" + WordResponse[54], 16);                     //X線管回転運転速度
                                        stsXrayRotPos = Convert.ToInt32("0x" + WordResponse[56] + WordResponse[55], 16);    //X線管回転現在位置
                                        stsUpLimitPos = Convert.ToInt32("0x" + WordResponse[56] + WordResponse[55], 16);    //ﾃｰﾌﾞﾙ上昇制限設定値 追加 by 稲葉 16-03-10

                                        stsXrayRotAccel = Convert.ToInt32("0x" + WordResponse[57], 16);                     //X線管回転加速度
                                        stsEXMMaxW = Convert.ToInt32("0x" + WordResponse[58], 16);                          //X線EXM最大出力値
                                        stsEXMMaxTV = Convert.ToInt32("0x" + WordResponse[59], 16);                         //X線EXM最大管電圧値
                                        stsEXMMinTV = Convert.ToInt32("0x" + WordResponse[60], 16);                         //X線EXM最小管電圧値
                                        stsEXMMaxTC = Convert.ToInt32("0x" + WordResponse[61], 16);                         //X線EXM最大管電流値
                                        stsEXMMinTC = Convert.ToInt32("0x" + WordResponse[62], 16);                         //X線EXM最小管電流値
                                        stsEXMErrCode = Convert.ToInt32("0x" + WordResponse[63], 16);                       //X線EXMｴﾗｰｺｰﾄﾞ

                                        // 電池検査用　 追加 by 稲葉 16-04-14
                                        stsColiUPosition = Convert.ToInt32("0x" + WordResponse[32], 16);     //ｺﾘﾒｰﾀ上現在値
                                        stsColiDPosition = Convert.ToInt32("0x" + WordResponse[33], 16);     //ｺﾘﾒｰﾀ下現在値
                                        stsWorkSerialNo = "";   // 追加 by 稲葉 16-05-04
                                        //for (int i = 34; i < 42; i++)
                                        for (int i = 34; i < 44; i++)   // 変更 by 稲葉 16-06-21
                                            {
                                            stsWorkSerialNo += ((char)Convert.ToInt16("0x" + WordResponse[i].Substring(2, 2), 16)).ToString() + //電池ｼﾘｱﾙNo. 
                                                              ((char)Convert.ToInt16("0x" + WordResponse[i].Substring(0, 2), 16)).ToString();
                                        }
                                        stsPlcYMTime = Convert.ToInt32("0x" + WordResponse[44], 16);     //PLC現在時刻 年月
                                        stsPlcDHTime = Convert.ToInt32("0x" + WordResponse[45], 16);     //PLC現在時刻 日時
                                        stsPlcNSTime = Convert.ToInt32("0x" + WordResponse[46], 16);     //PLC現在時刻 分秒
                                        stsStartYMTime = Convert.ToInt32("0x" + WordResponse[47], 16);     //処理開始日時 年月
                                        stsStartDHTime = Convert.ToInt32("0x" + WordResponse[48], 16);     //処理開始日時 日時
                                        stsStartNSTime = Convert.ToInt32("0x" + WordResponse[49], 16);     //処理開始日時 分秒

                                        // FDZ2+高速度カメラ用　 追加 by 稲葉 19-03-04
                                        stsXrayCameraUDPosition = Convert.ToInt32("0x" + WordResponse[59] + WordResponse[58], 16);     //X線/ｶﾒﾗ昇降現在位置読出し

#endif
                                        //追加2015/03/16hata
                                        RecvPoint = 23;
                                    }
                                    else
                                    {
                                        //追加2015/03/16hata
                                        //Dataが足りない
                                        RecvPoint = 24;
                                        //LogWrite("NoAll");
                                    }
                                    
                                }
                            break;
                        }
                    }
                    else
                    {
                        return;
                    }

                //}     //変更2015/03/13hata_デットロックのため外す

                if (_WordReadFlag == true)
                {
                    //読込完了
                    int _CommEndAns;
                    bool _RaiseEvent;

                    lock (gLock)
                    {
                        SeqCommEvent = 0;
                        _CommEndAns = SeqCommEvent;
                        _RaiseEvent = EventProcess(SeqCommEvent);     //ｲﾍﾞﾝﾄ処理
                    }
                    if (_RaiseEvent == true && OnCommEnd != null) OnCommEnd(_CommEndAns);

                    MethodProcess();        //通信待ち時のｼｰｹﾝｻへの読込書込み処理

                    lock (gLock)
                    {
                        //追加 by 稲葉 03-12-08
                        //昇降手動速度設定
                        if (stsUDSpeed != OldUDSpeed)
                        {
                            //変更2014/11/28hata_v19.51_dnet
                            //MechaControl.SpeedWriteTP(0, stsUDSpeed);
                            
                            short tmpUDSpeed = Convert.ToInt16(stsUDSpeed);
                            MechaControl.SpeedWriteTP(0, tmpUDSpeed);
                            OldUDSpeed = stsUDSpeed;
                        }
                        //回転手動速度設定
                        if (stsRotSpeed != OldRotSpeed)
                        {
                            //変更2014/11/28hata_v19.51_dnet
                            //MechaControl.SpeedWriteTP(1, stsRotSpeed);
                            short tmpRotSpeed = Convert.ToInt16(stsRotSpeed);

                            //変更2015/01/19hata
                            //MechaControl.SpeedWriteTP(0, tmpRotSpeed);
                            MechaControl.SpeedWriteTP(1, tmpRotSpeed);
                            
                            OldRotSpeed = stsRotSpeed;
                        }
                        //微調X手動速度設定
                        if (stsXStgSpeed != OldXStgSpeed)
                        {
                            //変更2014/11/28hata_v19.51_dnet
                            //MechaControl.SpeedWriteTP(2, stsXStgSpeed);
                            short tmpXStgSpeed = Convert.ToInt16(stsXStgSpeed);

                            //変更2015/01/19hata
                            //MechaControl.SpeedWriteTP(0, tmpXStgSpeed);
                            MechaControl.SpeedWriteTP(2, tmpXStgSpeed);
                            
                            OldXStgSpeed = stsXStgSpeed;
                        }
                        //微調Y手動速度設定
                        if (stsYStgSpeed != OldYStgSpeed)
                        {
                            //変更2014/11/28hata_v19.51_dnet
                            //MechaControl.SpeedWriteTP(3, stsYStgSpeed);
                            short tmpYStgSpeed = Convert.ToInt16(stsYStgSpeed);

                            //変更2015/01/19hata
                            //MechaControl.SpeedWriteTP(0, tmpYStgSpeed);
                            MechaControl.SpeedWriteTP(3, tmpYStgSpeed);
                            
                            OldYStgSpeed = stsYStgSpeed;
                        }

                        //追加 by 稲葉 15-07-24
                        //傾斜X手動速度設定
                        if (stsTiltSpeed != OldTiltSpeed)
                        {
                            short tmpTiltSpeed = Convert.ToInt16(stsTiltSpeed);
                            MechaControl.SpeedWriteTP(4, tmpTiltSpeed);
                            OldTiltSpeed = stsTiltSpeed;
                        }
                        //傾斜回転Y手動速度設定
                        if (stsTiltRotSpeed != OldTiltRotSpeed)
                        {
                            short tmpTiltRotSpeed = Convert.ToInt16(stsTiltRotSpeed);
                            MechaControl.SpeedWriteTP(5, tmpTiltRotSpeed);
                            OldTiltRotSpeed = stsTiltRotSpeed;
                        }

                        //昇降位置決め
                        if (UpDownIndex)
                        {
                            //ActiveX DLLからEXE用に変更 by 稲葉 05-01-08
                            //                        UdIndexTP 0, stsUDIndexPos / 100, 1
                            //変更2014/11/28hata_v19.51_dnet
                            //MechaControl.UdIndexTPexe(0, stsUDIndexPos / 100, 1);
                            BitWrite("UdIndexAck", true);   // 追加 by 稲葉 16-04-14
#if MicroUnit   //追加 by 稲葉 15-10-14
                            MechaControl.UdIndexTPexe(0, Convert.ToSingle(stsUDIndexPos / 1000F), 1);
#else
                            MechaControl.UdIndexTPexe(0, Convert.ToSingle(stsUDIndexPos / 100F), 1);
#endif
                        }
                        //回転位置決め
                        if (RotIndex)
                        {
                            //ActiveX DLLからEXE用に変更 by 稲葉 05-01-08
                            //                        RotIndexTP 0, stsRotIndexPos / 100, 0
                            //変更2014/11/28hata_v19.51_dnet
                            //MechaControl.RotIndexTPexe(0, stsRotIndexPos / 100, 0);
                            BitWrite("RotIndexAck", true);   // 追加 by 稲葉 16-04-14
#if MicroUnit   //追加 by 稲葉 15-10-14
                            MechaControl.RotIndexTPexe(0, Convert.ToSingle(stsRotIndexPos / 1000F), 0);
#else
                            MechaControl.RotIndexTPexe(0, Convert.ToSingle(stsRotIndexPos / 100F), 0);
#endif
                        }
                        //微調X位置決め
                        if (XStageIndex)
                        {
                            //ActiveX DLLからEXE用に変更 by 稲葉 05-01-08
                            //                        XStgIndexTP 0, stsXStgIndexPos / 100, 0
                            //変更2014/11/28hata_v19.51_dnet
                            //MechaControl.XStgIndexTPexe(0, stsXStgIndexPos / 100, 0);
                            MechaControl.XStgIndexTPexe(0, Convert.ToSingle(stsXStgIndexPos / 100F), 0);
                        }
                        //微調Y位置決め
                        if (YStageIndex)
                        {
                            //ActiveX DLLからEXE用に変更 by 稲葉 05-01-08
                            //                        YStgIndexTP 0, stsYStgIndexPos / 100, 0
                            //変更2014/11/28hata_v19.51_dnet
                            //MechaControl.YStgIndexTPexe(0, stsYStgIndexPos / 100F, 0);
                            MechaControl.YStgIndexTPexe(0, Convert.ToSingle(stsYStgIndexPos / 100F), 0);
                        }
                    }

                    return;     //追加 by 稲葉 02-02-28 
                }

                //書込みﾚｽﾎﾟﾝｽﾃﾞｰﾀの取得
                StartPos = rData.IndexOf(Convert.ToChar(0x6));

                //書込み完了
                if (StartPos != -1)
                {
                    bDatAll = true;

                    lock (tLock)
                    {
                        if (myTimer != null) myTimer.Enabled = false;             
                    }

                    MethodProcess();    //通信待ち時のｼｰｹﾝｻへの読込書込み処理                        
                    return;     //追加 by 稲葉 02-02-28                        
                }

                //読込書込ｴﾗｰﾚｽﾎﾟﾝｽﾃﾞｰﾀの取得
                StartPos = rData.IndexOf(Convert.ToChar(0x15));
                if (StartPos != -1)
                {
                    int _CommEndAns;
                    bool _RaiseEvent;

                    lock (gLock)
                    {
                        SeqCommEvent = 704;
                        _CommEndAns = SeqCommEvent;
                        _RaiseEvent = EventProcess(SeqCommEvent);   //ｲﾍﾞﾝﾄ処理
                    }
                    if (_RaiseEvent == true && OnCommEnd != null) OnCommEnd(_CommEndAns);
                }
            }
            catch
            {
                //'削除 by 稲葉 05-02-08
                //'    If OverRunFlag = 0 Then '追加 by 稲葉 02-03-14
                //    '    Beep
                //'        MsgBox "ｴﾗｰNo：" & Err.Number, , "通信ﾎﾟｰﾄｴﾗｰ"
                //'通信エラーを表示する　変更 by 稲葉 04-02-23
                //'        SeqCommEvent = 700                                  '通信エラーを表示しない deleted by 山本 2002-9-11 削除 2007-3-14 山本
                //'        EventProcess SeqCommEvent    'ｲﾍﾞﾝﾄ処理             '通信エラーを表示しない deleted by 山本 2002-9-11 削除 2007-3-14 山本
                //'    Else '追加 by 稲葉 02-03-14
                //'        mTimer.Enabled = False
                //'        TimeOutFlag = 0
                //'        stsCommBusy = False
                //'    End If

                RecvPoint = RecvPoint + 100;    //追加2015/03/14hata

            }
            finally
            {
                // Nothing
            }
        }
        #endregion

        #region RestProcess
        /// <summary>
        ///シリアルポートのリセット
        /// </summary>
        public void RestProcess()
        {
            byte[] buf = { 0x4, 0xd, 0xa };

            lock (gLock)
            {
                StatusReadNo = 0;
                BitWriteNo = 0;
                WordWriteNo = 0;
                BitWordWriteNo = 0;
                stsCommBusy = false;

                mySerialPort.DiscardInBuffer();         // 受信ﾊﾞｯﾌｧをｸﾘｱします                   
                mySerialPort.DiscardOutBuffer();        // 送信ﾊﾞｯﾌｧをｸﾘｱします                    

                // ポートがエラーするときは書き込まない //変更2015/02/10hata
                //if (!CommLineError & !CommDataError & mySerialPort.CtsHolding & (!mySerialPort.DtrEnable | (mySerialPort.DtrEnable & mySerialPort.DsrHolding)))
                if (!CommLineError & !CommDataError & mySerialPort.CtsHolding)
                    mySerialPort.Write(buf, 0, buf.Length); // ｼｰｹﾝｻの送受信ｼｰｹﾝｽをｸﾘｱします
            }
        }
        #endregion

    }
}
