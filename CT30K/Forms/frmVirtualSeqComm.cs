using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CT30K.Common;
using CTAPI;

namespace CT30K
{
	///* ************************************************************************** */
	///* システム　　： マイクロＣＴスキャナ TOSCANER-30000 Ver11.2                 */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： frmVirtualSeqComm                                           */
	///* 処理概要　　： 仮想シーケンサ通信                                          */
	///* 注意事項　　： デバッグ時のみ使用                                          */
	///* -------------------------------------------------------------------------- */
	///* 適用計算機　： DOS/V PC                                                    */
	///* ＯＳ　　　　： WindowsXP (SP2)                                             */
	///* コンパイラ　： VB 6.0 (SP5)                                                */
	///* -------------------------------------------------------------------------- */
	///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
	///*                                                                            */
	///* V11.2       05/10/07   (SI3)間々田          新規作成                       */
	///* v16.01      10/02/09   (検SS)山影           項目追加                       */
	///*                                                                            */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2005                 */
	///* ************************************************************************** */
	public partial class frmVirtualSeqComm : Form
	{

		private static frmVirtualSeqComm _Instance = null;

		public frmVirtualSeqComm()
		{
			InitializeComponent();

#if DebugOn
            //this.stsIIBackward.ValueChanged += new System.EventHandler(this.stsIIBackward_ValueChanged);
            //this.stsIIForward.ValueChanged += new System.EventHandler(this.stsIIForward_ValueChanged);
            //this.stsYBackward.ValueChanged += new System.EventHandler(this.stsYBackward_ValueChanged);
            //this.stsYForward.ValueChanged += new System.EventHandler(this.stsYForward_ValueChanged);
            //this.stsXRight.ValueChanged += new System.EventHandler(this.stsXRight_ValueChanged);
            //this.stsXLeft.ValueChanged += new System.EventHandler(this.stsXLeft_ValueChanged);
            //this.stsTVII4.ValueChanged += new System.EventHandler(this.stsTVII4_ValueChanged);
            //this.stsTVII6.ValueChanged += new System.EventHandler(this.stsTVII6_ValueChanged);
            //this.stsTVII9.ValueChanged += new System.EventHandler(this.stsTVII9_ValueChanged);
            //this.stsTVIIDrive.ValueChanged += new System.EventHandler(this.stsTVIIDrive_ValueChanged);
            //this.stsCTIIDrive.ValueChanged += new System.EventHandler(this.stsCTIIDrive_ValueChanged);
            //this.stsTVIIPos.ValueChanged += new System.EventHandler(this.stsTVIIPos_ValueChanged);
            //this.stsCTIIPos.ValueChanged += new System.EventHandler(this.stsCTIIPos_ValueChanged);

            this.Load += new System.EventHandler(this.frmVirtualSeqComm_Load);
            this.ctbIIBackward.ValueChanged += new System.EventHandler(this.ctbIIBackward_ValueChanged);
            this.ctbIIForward.ValueChanged += new System.EventHandler(this.ctbIIForward_ValueChanged);
            this.ctbYBackward.ValueChanged += new System.EventHandler(this.ctbYBackward_ValueChanged);
            this.ctbYForward.ValueChanged += new System.EventHandler(this.ctbYForward_ValueChanged);
            this.ctbXRight.ValueChanged += new System.EventHandler(this.ctbXRight_ValueChanged);
            this.ctbXLeft.ValueChanged += new System.EventHandler(this.ctbXLeft_ValueChanged);
            this.ctbTVII4.ValueChanged += new System.EventHandler(this.ctbTVII4_ValueChanged);
            this.ctbTVII6.ValueChanged += new System.EventHandler(this.ctbTVII6_ValueChanged);
            this.ctbTVII9.ValueChanged += new System.EventHandler(this.ctbTVII9_ValueChanged);
            this.ctbTVIIDrive.ValueChanged += new System.EventHandler(this.ctbTVIIDrive_ValueChanged);
            this.ctbTVIIPos.ValueChanged += new System.EventHandler(this.ctbTVIIPos_ValueChanged);
            this.ctbCTIIPos.ValueChanged += new System.EventHandler(this.ctbCTIIPos_ValueChanged);
            this.ctbRoomInSw.ValueChanged += new System.EventHandler(this.ctbRoomInSw_ValueChanged);
            this.ctbRotLargeTable.ValueChanged += new System.EventHandler(this.ctbRotLargeTable_ValueChanged);

            //Rev23.10 初期値設定 by長野 2015/09/29
            _stsMicroFPDPos = true;
            this.ctbMicroFPDSet.Click += new System.EventHandler(this.ctbMicroFPDSet_Click);
            this.ctbMicroFPDShiftSet.Click += new System.EventHandler(this.ctbMicroFPDShiftSet_Click);
            this.ctbNanoFPDSet.Click += new System.EventHandler(this.ctbNanoFPDSet_Click);
            this.ctbNanoFPDShiftSet.Click += new System.EventHandler(this.ctbNanoFPDShiftSet_Click);

            this.ctbMicroNanoReset.Click += new System.EventHandler(this.ctbMicroNanoReset_Click);
            this.ctbMicroXrayChangeBusy.Click += new System.EventHandler(this.ctbMicroXrayChangeBusy_Click);
            this.ctbNanoXrayChangeBusy.Click += new System.EventHandler(this.ctbNanoXrayChangeBusy_Click);
            this.ctbMicroFpdShiftBusy.Click += new System.EventHandler(this.ctbMicroFpdShiftBusy_Click);
            this.ctbNanoFpdShiftBusy.Click += new System.EventHandler(this.ctbNanoFpdShiftBusy_Click);
            this.ctbCTIIDrive.Click += new System.EventHandler(this.ctbCTIIDrive_Click);
            
#endif

        }

		public static frmVirtualSeqComm Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmVirtualSeqComm();
				}

				return _Instance;
			}
		}

#if DebugOn

		public event OnCommEndEventHandler OnCommEnd;
		public delegate void OnCommEndEventHandler(int CommEndAns);


		//以下のコメント化された変数は実際のシーケンサ（SeqComm.exe）では定義されているが，CT30Kでは未使用
		//'Public stsCommBusy       As Boolean  '通信中
		//'Public PcInhibit         As Boolean  'ﾊﾟｿｺﾝ操作
		//'Public stsSeqCounterErr  As Boolean  'ｼｰｹﾝｻｶｳﾝﾀﾕﾆｯﾄｴﾗｰ
		//'Public stsTiltCw         As Boolean  '傾斜CW中
		//'Public stsTiltCCw        As Boolean  '傾斜CCW中
		//'Public stsTiltOriginRun  As Boolean  '傾斜原点復帰中
		//'Public stsColliLOpen     As Boolean  'ｺﾘﾒｰﾀ左開中
		//'Public stsColliLClose    As Boolean  'ｺﾘﾒｰﾀ左閉中
		//'Public stsColliROpen     As Boolean  'ｺﾘﾒｰﾀ右開中
		//'Public stsColliRClose    As Boolean  'ｺﾘﾒｰﾀ右閉中
		//'Public stsColliUOpen     As Boolean  'ｺﾘﾒｰﾀ上開中
		//'Public stsColliUClose    As Boolean  'ｺﾘﾒｰﾀ上閉中
		//'Public stsColliDOpen     As Boolean  'ｺﾘﾒｰﾀ下開中
		//'Public stsColliDClose    As Boolean  'ｺﾘﾒｰﾀ下閉中
		//'Public stsFilter0Run     As Boolean  'ﾌｨﾙﾀ無し動作中
		//'Public stsFilter1Run     As Boolean  'ﾌｨﾙﾀ1動作中
		//'Public stsFilter2Run     As Boolean  'ﾌｨﾙﾀ2動作中
		//'Public stsFilter3Run     As Boolean  'ﾌｨﾙﾀ3動作中
		//'Public stsFilter4Run     As Boolean  'ﾌｨﾙﾀ4動作中
		//'Public stsFilter5Run     As Boolean  'ﾌｨﾙﾀ5動作中
		//'Public RotIndex          As Boolean  '回転位置決め要求
		//'Public UpDownIndex       As Boolean  '昇降位置決め要求
		//'Public XStageIndex       As Boolean  'ｽﾃｰｼﾞX位置決め要求
		//'Public YStageIndex       As Boolean  'ｽﾃｰｼﾞY位置決め要求
		//'Public stsXrayXErr       As Boolean  'X線管X軸ｴﾗｰ
		//'Public stsXrayYErr       As Boolean  'X線管Y軸ｴﾗｰ
		//'Public stsXrayXL         As Boolean  'X線管X軸左移動中
		//'Public stsXrayXR         As Boolean  'X線管X軸右移動中
		//'Public stsXrayYF         As Boolean  'X線管Y軸前進移動中
		//'Public stsXrayYB         As Boolean  'X線管Y軸後退移動中
		//'Public stsXrayRotLock    As Boolean  'X線管回転動作不可
		//'Public stsXStgSpeed      As Integer  '微調X手動運転速度
		//'Public stsXStgIndexPos   As Long     '微調X位置決め位置
		//'Public stsYStgSpeed      As Integer  '微調Y手動運転速度
		//'Public stsYStgIndexPos   As Long     '微調Y位置決め位置
		//'Public stsUDSpeed        As Long     '昇降手動運転速度
		//'Public stsRotSpeed       As Long     '昇降手動運転速度
		//'Public stsUDIndexPos     As Long     '昇降位置決め位置
		//'Public stsRotIndexPos    As Long     '回転位置決め位置
		//'Public stsYPosition      As Long     'ﾃｰﾌﾞﾙY軸現在位置
		//public int stsXrayFCD = 0;			//X線管FCD
		//public int stsXrayXPos = 0;			//X線管X軸現在位置
		//'Public stsXrayYPos      As Long     'X線管Y軸現在位置
		//'Public stsXrayRotMinSp  As Integer  'X線管回転最低速度
		//public int stsXrayRotMaxSp = 0;		//X線管回転最高速度
		//'Public stsXrayRotSpeed  As Integer  'X線管回転運転速度
		//Public stsXrayRotPos    As Long     'X線管回転現在位置
		//public int stsXrayRotAccel = 0;		//X線管回転加速度

        //Rev23.10 プロパティをstatic化 by長野 2015/09/29
        private static bool _stsCommBusy = false;          //'通信中
        //private bool _stsPcInhibit = false;         //'ﾊﾟｿｺﾝ操作
        private static bool _stsSeqCounterErr = false;     //'ｼｰｹﾝｻｶｳﾝﾀﾕﾆｯﾄｴﾗｰ
        private static bool _stsTiltCCw = false;           //'傾斜CCW中
        private static bool _RotIndex = false;             //'回転位置決め要求
        private static bool _UpDownIndex = false;          //'昇降位置決め要求
        private static bool _XStageIndex = false;          //'ｽﾃｰｼﾞX位置決め要求
        private static bool _YStageIndex = false;          //'ｽﾃｰｼﾞY位置決め要求

        //private bool _stsSPIIChange = false;          //'ｽｷｬﾝ位置校正I.I.移動有り
        private static bool _stsXrayXErr = false;          //'X線管X軸ｴﾗｰ
        private static bool _stsXrayYErr = false;          //'X線管Y軸ｴﾗｰ
        private static bool _stsXrayRotLock = false;       //'X線管回転動作不可
        private static bool _stsEXMNormal2 = false;        //'X線EXMﾃﾞｰﾀ書込正常
        private static bool _stsEXMRemote = false;         //X線EXMﾘﾓｰﾄ中
        private static bool _stsAutoRestrict = false;      //動作制限自動復帰設定状態
        private static bool _stsYIndexSlow = false;        //Y軸ｲﾝﾃﾞｯｸｽ減速設定状態

        //private bool _stsShutter = false;           //ｼｬｯﾀ位置
        private static bool _stsShutterBusy = false;       //ｼｬｯﾀ動作中

        private static int _stsUDSpeed = 0;               // 昇降手動運転速度  
        private static int _stsUDIndexPos = 0;            // 昇降位置決め位置  

        private static int _stsRotSpeed = 0;               // 回転手動運転速度  
        private static int _stsRotIndexPos = 0;            // 回転位置決め位置  

        private static int _stsXStgSpeed = 0;              //微調X手動運転速度  
        private static int _stsXStgIndexPos = 0;           //微調X位置決め位置  
        private static int _stsYStgSpeed = 0;              //微調Y手動運転速度  
        private static int _stsYStgIndexPos = 0;           //微調Y位置決め位置
        private static int _stsYPosition = 0;              //ﾃｰﾌﾞﾙY軸現在位置   
        private static int _stsXrayFCD = 0;                //X線管FCD
        private static int _stsXrayXPos = 0;               //X線管X軸現在位置      
        private static int _stsXrayYPos = 0;               //X線管Y軸現在位置      
        private static int _stsLinearY = 0;                //テーブルY軸 リニアスケール値 Rev23.10 追加 by長野 2015/09/18
        private static int _stsXrayRotMinSp = 0;           //X線管回転最低速度      
        private static int _stsXrayRotMaxSp = 0;           //X線管回転最高速度       
        private static int _stsXrayRotSpeed = 0;           //X線管回転運転速度       
        private static int _stsXrayRotAccel = 0;           //X線管回転加速度
        private static int _stsEXMMaxW = 0;                //X線EXM最大出力値

        private static int _stsEXMLimitTV = 0;             //X線EXM制限管電圧値
        private static int _stsEXMLimitTC = 0;             //X線EXM制限管電流値

        private static bool _IsReady = true;              //シーケンサがレディ状態になったか
        //private bool _stsRotLargeTable = false;     //回転大ﾃｰﾌﾞﾙ有無
        //private bool _stsRoomInSw = false;          //検査室入室安全ｽｲｯﾁ

        private static bool _stsColdBoxDoorClose;          //冷蔵箱ドア開閉 //Rev22.00 追加 by長野 2015/08/21
        private static bool _stsColdBoxPosOK;              //冷蔵箱位置確認 //Rev22.00 追加 by長野 2015/08/21

        private static int _stsTiltSpeed;                  //回転傾斜 傾斜速度 Rev23.10 追加 by長野 2015/09/18
        private static int _stsTiltRotSpeed;               //回転傾斜 回転速度 Rev23.10 追加 by長野 2015/09/18

        private static int _stsLinearFDD = 0;              //FDD リニアスケール値 Rev23.10 追加 by長野 2015/09/18
        private static int _stsLinearTableY = 0;           //Y軸 リニアスケール値 Rev23.10 追加 by長野 2015/09/18
        private static int _stsLinearFCD = 0;              //FCD リニアスケール値 Rev23.10 追加 by長野 2015/09/18

        private static bool _stsMicroFPDPos;           //ﾏｲｸﾛﾌｫｰｶｽX線検出器位置 追加 Rev23.10 by長野 2015/09/18
        private static bool _stsMicroFPDShiftPos;      // ﾏｲｸﾛﾌｫｰｶｽX線検出器ｼﾌﾄ位置 追加 Rev23.10 by長野 2015/09/18
        private static bool _stsNanoFPDShiftPos;       // ﾅﾉﾌｫｰｶｽX線検出器ｼﾌﾄ位置 追加 Rev23.10 by長野 2015/09/18
        private static bool _stsNanoFPDPos;       //ﾅﾉﾌｫｰｶｽX線検出器位置 追加 Rev23.10 by長野 2015/09/18
        private static bool _stsMicroFPDBusy;   // ﾏｲｸﾛﾌｫｰｶｽX線切替中 追加 Rev23.10 by長野 2015/09/18
        private static bool _stsNanoFPDBusy;    // ﾅﾉﾌｫｰｶｽX線切替中 追加 Rev23.10 by長野 2015/09/18
        private static bool _stsMicroFPDShiftBusy;     // ﾏｲｸﾛﾌｫｰｶｽX線検出器ｼﾌﾄ中 追加 Rev23.10 by長野 2015/09/18
        private static bool _stsNanoFPDShiftBusy;      // ﾅﾉﾌｫｰｶｽX線検出器ｼﾌﾄ中 追加 Rev23.10 by長野 2015/09/18
        private static int _stsFCDLimitAdj;            // FCD軸ｲﾝﾀｰﾛｯｸﾘﾐｯﾄ位置補正 追加 Rev23.10 by長野 2015/09/18

        private static bool _stsCTIIDrive;            // 追加 Rev23.10 by長野 2015/09/18 
        private static bool _stsTVIIDrive;            // 追加 Rev23.10 by長野 2015/09/18

        //Rev23.20 追加 by長野 2016/01/19 --->
        private static bool _stsFPDLShiftPos;
        private static bool _stsFPDLShiftBusy;
        private static bool _stsFDSystemBusy;
        private static bool _stsFDSystemPos;

        private static int  _stsXrayHOffTimeY;
        private static int  _stsXrayHOffTimeMD;
        private static int  _stsXrayHOffTimeHM;
        private static int  _stsXrayMOffTimeY;
        private static int  _stsXrayMOffTimeMD;
        private static int  _stsXrayMOffTimeHM;
        //<---

        //Rev23.40 追加 by長野 2016/06/19
        private static bool _stsUpLimitPermit;
        private static int  _stsUpLimitPos;

        //----------------------------------------------------------------
        //　プロパティ
        //----------------------------------------------------------------

        #region パブリックフィールド
        /// <summary>
        /// 通信中
        /// </summary>
        public bool stsCommBusy
        {
            get
            {
                return _stsCommBusy;
            }
            set
            {
                _stsCommBusy = value;
            }
        }

        /// <summary>
        /// ﾊﾟｿｺﾝ操作
        /// </summary>
        public bool PcInhibit
        {
            get
            {
                return ctbPcInhibit.Value;
            }
            set
            {
                ctbPcInhibit.Value = value;
            }
        }

        /// <summary>
        /// 運転準備ｽｲｯﾁ  '追加 by 稲葉 05-11-24
        /// </summary>
        public bool stsRunReadySW
        {
            get
            {
                return ctbRunReadySW.Value;
            }
            set
            {
                ctbRunReadySW.Value = value;
            }
        }

        /// <summary>
        /// 扉ｲﾝﾀｰﾛｯｸ
        /// </summary>
        public bool stsDoorInterlock
        {
            get
            {
                return ctbDoorInterlock.Value;
            }
            set
            {
                ctbDoorInterlock.Value = value;
            }
        }

        /// <summary>
        /// 非常停止
        /// </summary>
        public bool stsEmergency
        {
            get
            {
                return ctbEmergency.Value;
            }
            set
            {
                ctbEmergency.Value = value;
            }
        }

        /// <summary>
        /// X線225KVﾄﾘｯﾌﾟ
        /// </summary>
        public bool stsXray225Trip
        {
            get
            {
                return ctbXray225Trip.Value;
            }
            set
            {
                ctbXray225Trip.Value = value;
            }
        }

        /// <summary>
        /// X線160KVﾄﾘｯﾌﾟ
        /// </summary>
        public bool stsXray160Trip
        {
            get
            {
                return ctbXray160Trip.Value;
            }
            set
            {
                ctbXray160Trip.Value = value;
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀﾕﾆｯﾄ接触
        /// </summary>
        public bool stsFilterTouch
        {
            get
            {
                return ctbFilterTouch.Value;
            }
            set
            {
                ctbFilterTouch.Value = value;
            }
        }

        /// <summary>
        /// X線225KV接触
        /// </summary>
        public bool stsXray225Touch
        {
            get
            {
                return ctbXray225Touch.Value;
            }
            set
            {
                ctbXray225Touch.Value = value;
            }
        }

        /// <summary>
        /// X線160KV接触
        /// </summary>
        public bool stsXray160Touch
        {
            get
            {
                return ctbXray160Touch.Value;
            }
            set
            {
                ctbXray160Touch.Value = value;
            }
        }

        /// <summary>
        /// 回転ﾃｰﾌﾞﾙ接触
        /// </summary>
        public bool stsRotTouch
        {
            get
            {
                return ctbRotTouch.Value;
            }
            set
            {
                ctbRotTouch.Value = value;
            }
        }

        /// <summary>
        /// 傾斜ﾃｰﾌﾞﾙ接触
        /// </summary>
        public bool stsTiltTouch
        {
            get
            {
                return ctbTiltTouch.Value;
            }
            set
            {
                ctbTiltTouch.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙX軸ｵｰﾊﾞｰﾋｰﾄ
        /// </summary>
        public bool stsXDriverHeat
        {
            get
            {
                return ctbXDriverHeat.Value;
            }
            set
            {
                ctbXDriverHeat.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙY軸ｵｰﾊﾞｰﾋｰﾄ
        /// </summary>
        public bool stsYDriverHeat
        {
            get
            {
                return ctbYDriverHeat.Value;
            }
            set
            {
                ctbYDriverHeat.Value = value;
            }
        }

        /// <summary>
        /// X線管切替ｵｰﾊﾞｰﾋｰﾄ
        /// </summary>
        public bool stsXrayDriverHeat
        {
            get
            {
                return ctbXrayDriverHeat.Value;
            }
            set
            {
                ctbXrayDriverHeat.Value = value;
            }
        }

        /// <summary>
        /// ｼｰｹﾝｻCPUｴﾗｰ
        /// </summary>
        public bool stsSeqCpuErr
        {
            get
            {
                return ctbSeqCpuErr.Value;
            }
            set
            {
                ctbSeqCpuErr.Value = value;
            }
        }

        /// <summary>
        /// ｼｰｹﾝｻﾊﾞｯﾃﾘｰｴﾗｰ
        /// </summary>
        public bool stsSeqBatteryErr
        {
            get
            {
                return ctbSeqBatteryErr.Value;
            }
            set
            {
                ctbSeqBatteryErr.Value = value;
            }
        }

        /// <summary>
        /// ｼｰｹﾝｻKL通信ｴﾗｰ(KZ)
        /// </summary>
        public bool stsSeqKzCommErr
        {
            get
            {
                return ctbSeqKzCommErr.Value;
            }
            set
            {
                ctbSeqKzCommErr.Value = value;
            }
        }

        /// <summary>
        /// ｼｰｹﾝｻKL通信ｴﾗｰ(KV)
        /// </summary>
        public bool stsSeqKvCommErr
        {
            get
            {
                return ctbSeqKvCommErr.Value;
            }
            set
            {
                ctbSeqKvCommErr.Value = value;
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀｰﾀｲﾑｱｳﾄ
        /// </summary>
        public bool stsFilterTimeout
        {
            get
            {
                return ctbFilterTimeout.Value;
            }
            set
            {
                ctbFilterTimeout.Value = value;
            }
        }

        /// <summary>
        /// 傾斜ﾀｲﾑｱｳﾄ
        /// </summary>
        public bool stsTiltTimeout
        {
            get
            {
                return ctbTiltTimeout.Value;
            }
            set
            {
                ctbTiltTimeout.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙX軸ﾀｲﾑｱｳﾄ
        /// </summary>
        public bool stsXTimeout
        {
            get
            {
                return ctbXTimeout.Value;
            }
            set
            {
                ctbXTimeout.Value = value;
            }
        }

        /// <summary>
        /// I.I.軸ｵｰﾊﾞｰﾋｰﾄ
        /// </summary>
        public bool stsIIDriverHeat
        {
            get
            {
                return ctbIIDriverHeat.Value;
            }
            set
            {
                ctbIIDriverHeat.Value = value;
            }
        }

        /// <summary>
        /// ｼｰｹﾝｻｶｳﾝﾀﾕﾆｯﾄｴﾗｰ
        /// </summary>
        public bool stsSeqCounterErr
        {
            get
            {
                return _stsSeqCounterErr;
            }
            set
            {
                _stsSeqCounterErr = value;
            }
        }

        /// <summary>
        /// X軸動作ｴﾗｰ
        /// </summary>
        public bool stsXDriveErr
        {
            get
            {
                return ctbXDriveErr.Value;
            }
            set
            {
                ctbXDriveErr.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙX左限
        /// </summary>
        public bool stsXLLimit
        {
            get
            {
                return ctbXLLimit.Value;
            }
            set
            {
                ctbXLLimit.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙX右限
        /// </summary>
        public bool stsXRLimit
        {
            get
            {
                return ctbXRLimit.Value;
            }
            set
            {
                ctbXRLimit.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙX左移動中
        /// </summary>
        public bool stsXLeft
        {
            get
            {
                return ctbXLeft.Value;
            }
            set
            {
                ctbXLeft.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙX右移動中
        /// </summary>
        public bool stsXRight
        {
            get
            {
                return ctbXRight.Value;
            }
            set
            {
                ctbXRight.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙX左移動中接触
        /// </summary>
        public bool stsXLTouch
        {
            get
            {
                return ctbXLTouch.Value;
            }
            set
            {
                ctbXLTouch.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙX右移動中接触
        /// </summary>
        public bool stsXRTouch
        {
            get
            {
                return ctbXRTouch.Value;
            }
            set
            {
                ctbXRTouch.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙY前進限
        /// </summary>
        public bool stsYFLimit
        {
            get
            {
                return ctbYFLimit.Value;
            }
            set
            {
                ctbYFLimit.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙY後退限
        /// </summary>
        public bool stsYBLimit
        {
            get
            {
                return ctbYBLimit.Value;
            }
            set
            {
                ctbYBLimit.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙY前進中
        /// </summary>
        public bool stsYForward
        {
            get
            {
                return ctbYForward.Value;
            }
            set
            {
                ctbYForward.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙY後退中
        /// </summary>
        public bool stsYBackward
        {
            get
            {
                return ctbYBackward.Value;
            }
            set
            {
                ctbYBackward.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙY前進中接触
        /// </summary>
        public bool stsYFTouch
        {
            get
            {
                return ctbYFTouch.Value;
            }
            set
            {
                ctbYFTouch.Value  = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙY後退中接触
        /// </summary>
        public bool stsYBTouch
        {
            get
            {
                return ctbYBTouch.Value;
            }
            set
            {
                ctbYBTouch.Value = value;
            }
        }

        /// <summary>
        /// I.I.前進限
        /// </summary>
        public bool stsIIFLimit
        {
            get
            {
                return ctbIIFLimit.Value;
            }
            set
            {
                ctbIIFLimit.Value = value;
            }
        }

        /// <summary>
        /// I.I.後退限
        /// </summary>
        public bool stsIIBLimit
        {
            get
            {
                return ctbIIBLimit.Value;
            }
            set
            {
                ctbIIBLimit.Value = value;
            }
        }

        /// <summary>
        /// I.I.前進中
        /// </summary>
        public bool stsIIForward
        {
            get
            {
                return ctbIIForward.Value;
            }
            set
            {
                ctbIIForward.Value = value;
            }
        }

        /// <summary>
        /// I.I.後退中
        /// </summary>
        public bool stsIIBackward
        {
            get
            {
                return ctbIIBackward.Value;
            }
            set
            {
                ctbIIBackward.Value = value;
            }
        }

        /// <summary>
        /// 傾斜CW限
        /// </summary>
        public bool stsTiltCwLimit
        {
            get
            {
                return ctbTiltCwLimit.Value;
            }
            set
            {
                ctbTiltCwLimit.Value = value;
            }
        }

        /// <summary>
        /// 傾斜CCW限
        /// </summary>
        public bool stsTiltCCwLimit
        {
            get
            {
                return ctbTiltCCwLimit.Value;
            }
            set
            {
                ctbTiltCCwLimit.Value = value;
            }
        }

        /// <summary>
        /// 傾斜原点（水平）
        /// </summary>
        public bool stsTiltOrigin
        {
            get
            {
                return ctbTiltOrigin.Value;
            }
            set
            {
                ctbTiltOrigin.Value  = value;
            }
        }

        /// <summary>
        /// 傾斜CW中
        /// </summary>
        public bool stsTiltCw
        {
            get
            {
                return ctbTiltCw.Value;
            }
            set
            {
                ctbTiltCw.Value = value;
            }
        }

        /// <summary>
        /// 傾斜CCW中
        /// </summary>
        public bool stsTiltCCw
        {
            get
            {
                    return _stsTiltCCw;
            }
            set
            {
                    _stsTiltCCw = value;
            }
        }

        /// <summary>
        /// 傾斜原点復帰中
        /// </summary>
        public bool stsTiltOriginRun
        {
            get
            {
                return ctbTiltOriginRun.Value;
            }
            set
            {
                ctbTiltOriginRun.Value = value;
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ左開限
        /// </summary>
        public bool stsColliLOLimit
        {
            get
            {
                return ctbColliLOLimit.Value;
            }
            set
            {
                ctbColliLOLimit.Value = value;
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ左閉限
        /// </summary>
        public bool stsColliLCLimit
        {
            get
            {
                return ctbColliLCLimit.Value;
            }
            set
            {
                ctbColliLCLimit.Value = value;
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ右開限
        /// </summary>
        public bool stsColliROLimit
        {
            get
            {
                return ctbColliROLimit.Value;
            }
            set
            {
                ctbColliROLimit.Value = value;
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ右閉限
        /// </summary>
        public bool stsColliRCLimit
        {
            get
            {
                return ctbColliRCLimit.Value;
            }
            set
            {
                ctbColliRCLimit.Value = value;
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ上開限
        /// </summary>
        public bool stsColliUOLimit
        {
            get
            {
                return ctbColliUOLimit.Value;
            }
            set
            {
                ctbColliUOLimit.Value = value;
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ上閉限
        /// </summary>
        public bool stsColliUCLimit
        {
            get
            {
                return ctbColliUCLimit.Value;
            }
            set
            {
                ctbColliUCLimit.Value = value;
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ下開限
        /// </summary>
        public bool stsColliDOLimit
        {
            get
            {
                return ctbColliDOLimit.Value;
            }
            set
            {
                ctbColliDOLimit.Value = value;
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ下閉限
        /// </summary>
        public bool stsColliDCLimit
        {
            get
            {
                return ctbColliDCLimit.Value;
            }
            set
            {
                ctbColliDCLimit.Value = value;
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ左開中
        /// </summary>
        public bool stsColliLOpen
        {
            get
            {
                return ctbColliLOpen.Value;
            }
            set
            {
                ctbColliLOpen.Value = value;
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ左閉中
        /// </summary>
        public bool stsColliLClose
        {
            get
            {
                return ctbColliLClose.Value;
            }
            set
            {
                ctbColliLClose.Value = value;
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ右開中
        /// </summary>
        public bool stsColliROpen
        {
            get
            {
                return ctbColliROpen.Value;
            }
            set
            {
                ctbColliROpen.Value = value;
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ右閉中
        /// </summary>
        public bool stsColliRClose
        {
            get
            {
                return ctbColliRClose.Value;
            }
            set
            {
                ctbColliRClose.Value = value;
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ上開中
        /// </summary>
        public bool stsColliUOpen
        {
            get
            {
                return ctbColliUOpen.Value;
            }
            set
            {
                ctbColliUOpen.Value = value;
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ上閉中
        /// </summary>
        public bool stsColliUClose
        {
            get
            {
                return ctbColliUClose.Value;
            }
            set
            {
                ctbColliUClose.Value = value;
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ下開中
        /// </summary>
        public bool stsColliDOpen
        {
            get
            {
                return ctbColliDOpen.Value;
            }
            set
            {
                ctbColliDOpen.Value = value;
            }
        }

        /// <summary>
        /// ｺﾘﾒｰﾀ下閉中
        /// </summary>
        public bool stsColliDClose
        {
            get
            {
                return ctbColliDClose.Value;
            }
            set
            {
                ctbColliDClose.Value = value;
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ無し位置
        /// </summary>
        public bool stsFilter0
        {
            get
            {
                return ctbFilter0.Value;
            }
            set
            {
                ctbFilter0.Value = value;
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ1位置
        /// </summary>
        public bool stsFilter1
        {
            get
            {
                return ctbFilter1.Value;
            }
            set
            {
                ctbFilter1.Value = value;
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ2位置
        /// </summary>
        public bool stsFilter2
        {
            get
            {
                return ctbFilter2.Value;
            }
            set
            {
                ctbFilter2.Value = value;
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ3位置
        /// </summary>
        public bool stsFilter3
        {
            get
            {
                return ctbFilter3.Value;
            }
            set
            {
                ctbFilter3.Value = value;
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ4位置
        /// </summary>
        public bool stsFilter4
        {
            get
            {
                return ctbFilter4.Value;
            }
            set
            {
                ctbFilter4.Value = value;
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ5位置
        /// </summary>
        public bool stsFilter5
        {
            get
            {
                return ctbFilter5.Value;
            }
            set
            {
                ctbFilter5.Value = value;
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ無し動作中
        /// </summary>
        public bool stsFilter0Run
        {
            get
            {
                return ctbFilter0Run.Value;
            }
            set
            {
                ctbFilter0Run.Value = value;
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ1動作中
        /// </summary>
        public bool stsFilter1Run
        {
            get
            {
                return ctbFilter1Run.Value;
            }
            set
            {
                ctbFilter1Run.Value = value;
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ2動作中
        /// </summary>
        public bool stsFilter2Run
        {
            get
            {
                return ctbFilter2Run.Value;
            }
            set
            {
                ctbFilter2Run.Value = value;
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ3動作中
        /// </summary>
        public bool stsFilter3Run
        {
            get
            {
                return ctbFilter3Run.Value;
            }
            set
            {
                ctbFilter3Run.Value = value;
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ4動作中
        /// </summary>
        public bool stsFilter4Run
        {
            get
            {
                return ctbFilter4Run.Value;
            }
            set
            {
                ctbFilter4Run.Value = value;
            }
        }

        /// <summary>
        /// ﾌｨﾙﾀ5動作中
        /// </summary>
        public bool stsFilter5Run
        {
            get
            {
                return ctbFilter5Run.Value;
            }
            set
            {
                ctbFilter5Run.Value = value;
            }
        }

        /// <summary>
        /// X線ON
        /// </summary>
        public bool XrayOn
        {
            get
            {
                return ctbXrayOn.Value;
            }
            set
            {
                ctbXrayOn.Value = value;
            }
        }

        /// <summary>
        /// X線OFF
        /// </summary>
        public bool XrayOff
        {
            get
            {
                return ctbXrayOff.Value;
            }
            set
            {
                ctbXrayOff.Value = value;
            }
        }

        /// <summary>
        /// ｽﾃｰｼﾞX左移動
        /// </summary>
        public bool XStgLeft
        {
            get
            {
                return ctbXStgLeft.Value;
            }
            set
            {
                ctbXStgLeft.Value = value;
            }
        }

        /// <summary>
        /// ｽﾃｰｼﾞX右移動
        /// </summary>
        public bool XStgRight
        {
            get
            {
                return ctbXStgRight.Value;
            }
            set
            {
                ctbXStgRight.Value = value;
            }
        }

        /// <summary>
        /// ｽﾃｰｼﾞX原点復帰
        /// </summary>
        public bool XStgOrigin
        {
            get
            {
                return ctbXStgOrigin.Value;
            }
            set
            {
                ctbXStgOrigin.Value = value;
            }
        }

        /// <summary>
        /// ｽﾃｰｼﾞY前進
        /// </summary>
        public bool YStgForward
        {
            get
            {
                return ctbYStgForward.Value;
            }
            set
            {
                ctbYStgForward.Value = value;
            }
        }

        /// <summary>
        /// ｽﾃｰｼﾞY後退
        /// </summary>
        public bool YStgBackward
        {
            get
            {
                return ctbYStgBackward.Value;
            }
            set
            {
                ctbYStgBackward.Value = value;
            }
        }

        /// <summary>
        /// ｽﾃｰｼﾞY原点復帰
        /// </summary>
        public bool YStgOrigin
        {
            get
            {
                return ctbYStgOrigin.Value;
            }
            set
            {
                ctbYStgOrigin.Value = value;
            }
        }

        /// <summary>
        /// 回転位置決め要求
        /// </summary>
        public bool RotIndex
        {
            get
            {
                return RotIndex;
            }
            set
            {
                _RotIndex = value;
            }
        }

        /// <summary>
        /// I.I.視野9”
        /// </summary>
        public bool stsII9
        {
            get
            {
                return ctbII9.Value;
            }
            set
            {
                ctbII9.Value = value;
            }
        }

        /// <summary>
        /// I.I.視野6”
        /// </summary>
        public bool stsII6
        {
            get
            {
                return ctbII6.Value;
            }
            set
            {
                ctbII6.Value = value;
            }
        }

        /// <summary>
        /// I.I.視野4.5”
        /// </summary>
        public bool stsII4
        {
            get
            {
                return ctbII4.Value;
            }
            set
            {
                ctbII4.Value = value;
            }
        }

        /// <summary>
        /// I.I.電源
        /// </summary>
        public bool stsIIPower
        {
            get
            {
                return ctbIIPower.Value;
            }
            set
            {
                ctbIIPower.Value = value;
            }
        }

        /// <summary>
        /// ｽﾗｲｽﾗｲﾄ
        /// </summary>
        public bool stsSLight
        {
            get
            {
                return ctbSLight.Value;
            }
            set
            {
                ctbSLight.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙｲﾝﾀｰﾛｯｸ解除
        /// </summary>
        public bool stsTableMovePermit
        {
            get
            {
                return ctbTableMovePermit.Value;
            }
            set
            {
                ctbTableMovePermit.Value = value;
           }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙ回転、上昇、微調動作禁止
        /// </summary>
        public bool stsMechaPermit
        {
            get
            {
                return ctbMechaPermit.Value;
            }
            set
            {
                ctbMechaPermit.Value = value;
            }
        }

        /// <summary>
        /// 昇降位置決め要求
        /// </summary>
        public bool UpDownIndex
        {
            get
            {
                return _UpDownIndex;
            }
            set
            {
                _UpDownIndex = value;
            }
        }

        /// <summary>
        /// ｽﾃｰｼﾞX位置決め要求
        /// </summary>
        public bool XStageIndex
        {
            get
            {
                return _XStageIndex;
            }
            set
            {
                _XStageIndex = value;
            }
        }

        /// <summary>
        /// ｽﾃｰｼﾞY位置決め要求
        /// </summary>
        public bool YStageIndex
        {
            get
            {
                return _YStageIndex;
            }
            set
            {
                _YStageIndex = value;
            }
        }

        /// <summary>
        /// ﾒｶﾃﾞﾊﾞｲｽ　ｴﾗｰﾘｾｯﾄ
        /// </summary>
        public bool DeviceErrReset
        {
            get
            {
                return ctbDeviceErrReset.Value;
            }
            set
            {
                ctbDeviceErrReset.Value = value;
            }
        }

        /// <summary>
        /// 昇降原点復帰
        /// </summary>
        public bool UdOrigin
        {
            get
            {
                return ctbUdOrigin.Value;
            }
            set
            {
                ctbUdOrigin.Value = value;
            }
        }

        /// <summary>
        /// 回転原点復帰
        /// </summary>
        public bool RotOrigin
        {
            get
            {
                return ctbRotOrigin.Value;
            }
            set
            {
                ctbRotOrigin.Value = value;
            }
        }

        /// <summary>
        /// 回転中心校正ﾃｰﾌﾞﾙX移動有り
        /// </summary>
        public bool stsRotXChange
        {
            get
            {
                return ctbRotXChange.Value;
            }
            set
            {
                ctbRotXChange.Value = value;
            }
        }

        /// <summary>
        /// 寸法校正ﾃｰﾌﾞﾙX移動有り
        /// </summary>
        public bool stsDisXChange
        {
            get
            {
                return ctbDisXChange.Value;
            }
            set
            {
                ctbDisXChange.Value = value;
            }
        }

        /// <summary>
        /// 回転中心校正ﾃｰﾌﾞﾙY移動有り
        /// </summary>
        public bool stsRotYChange
        {
            get
            {
                return ctbRotYChange.Value;
            }
            set
            {
                ctbRotYChange.Value = value; 
            }
        }

        /// <summary>
        /// 寸法校正ﾃｰﾌﾞﾙY移動有り
        /// </summary>
        public bool stsDisYChange
        {
            get
            {
                return ctbDisYChange.Value;
            }
            set
            {
                ctbDisYChange.Value = value;
            }
        }

        /// <summary>
        /// 幾何歪校正I.I.移動有り
        /// </summary>
        public bool stsVerIIChange
        {
            get
            {
                return ctbVerIIChange.Value;
            }
            set
            {
                ctbVerIIChange.Value = value;
            }
        }

        /// <summary>
        /// 回転中心校正I.I.移動有り
        /// </summary>
        public bool stsRotIIChange
        {
            get
            {
                return ctbRotIIChange.Value;
            }
            set
            {
                ctbRotIIChange.Value = value;
            }
        }

        /// <summary>
        /// ｹﾞｲﾝ校正I.I.移動有り
        /// </summary>
        public bool stsGainIIChange
        {
            get
            {
                return ctbGainIIChange.Value;
            }
            set
            {
                ctbGainIIChange.Value = value;
            }
        }

        /// <summary>
        /// 寸法校正I.I.移動有り
        /// </summary>
        public bool stsDisIIChange
        {
            get
            {
                return ctbDisIIChange.Value;
            }
            set
            {
                ctbDisIIChange.Value = value;
            }
        }

        /// <summary>
        /// ｽｷｬﾝ位置校正I.I.移動有り '追加 by 稲葉 05-10-21
        /// </summary>
        public bool stsSPIIChange
        {
            get
            {
                return ctbSpIIChange.Value;
            }
            set
            {
                ctbSpIIChange.Value = value;
            }
        }

        /// <summary>
        /// X線管X軸ｴﾗｰ
        /// </summary>
        public bool stsXrayXErr
        {
            get
            {
                return _stsXrayXErr;
            }
            set
            {
                _stsXrayXErr = value;
            }
        }

        /// <summary>
        /// X線管Y軸ｴﾗｰ
        /// </summary>
        public bool stsXrayYErr
        {
            get
            {
                return _stsXrayYErr;
            }
            set
            {
                _stsXrayYErr = value;
            }
        }

        /// <summary>
        /// X線管回転ｴﾗｰ
        /// </summary>
        public bool stsXrayRotErr
        {
            get
            {
                return ctbXrayRotErr.Value;
            }
            set
            {
                ctbXrayRotErr.Value = value;
            }
        }

        /// <summary>
        /// X線管X軸左限
        /// </summary>
        public bool stsXrayXLLimit
        {
            get
            {
                return ctbXrayXLLimit.Value;
            }
            set
            {
                ctbXrayXLLimit.Value = value;
            }
        }

        /// <summary>
        /// X線管X軸右限
        /// </summary>
        public bool stsXrayXRLimit
        {
            get
            {
                return ctbXrayXRLimit.Value;
            }
            set
            {
                ctbXrayXRLimit.Value = value;
            }
        }

        /// <summary>
        /// X線管X軸左移動中
        /// </summary>
        public bool stsXrayXL
        {
            get
            {
                return ctbXrayXL.Value;
            }
            set
            {
                ctbXrayXL.Value = value;
            }
        }

        /// <summary>
        /// X線管X軸右移動中
        /// </summary>
        public bool stsXrayXR
        {
            get
            {
                return ctbXrayXR.Value;
            }
            set
            {
                ctbXrayXR.Value = value;
            }
        }

        /// <summary>
        /// 回転中心校正X線管X軸移動有り
        /// </summary>
        public bool stsRotXrayXCh
        {
            get
            {
                return ctbRotXrayXCh.Value;
            }
            set
            {
                ctbRotXrayXCh.Value = value;
            }
        }

        /// <summary>
        /// 寸法校正X線管X軸移動有り
        /// </summary>
        public bool stsDisXrayXCh
        {
            get
            {
                return ctbDisXrayXCh.Value;
            }
            set
            {
                ctbDisXrayXCh.Value = value;
            }
        }

        /// <summary>
        /// X線管Y軸前進限
        /// </summary>
        public bool stsXrayYFLimit
        {
            get
            {
                return ctbXrayYFLimit.Value;
            }
            set
            {
                ctbXrayYFLimit.Value = value;
            }
        }

        /// <summary>
        /// X線管Y軸後退限
        /// </summary>
        public bool stsXrayYBLimit
        {
            get
            {
                return ctbXrayYBLimit.Value;
            }
            set
            {
                ctbXrayYBLimit.Value = value;
            }
        }

        /// <summary>
        /// X線管Y軸前進移動中
        /// </summary>
        public bool stsXrayYF
        {
            get
            {
                return ctbXrayYF.Value;
            }
            set
            {
                ctbXrayYF.Value = value;
            }
        }

        /// <summary>
        /// X線管Y軸後退移動中
        /// </summary>
        public bool stsXrayYB
        {
            get
            {
                return ctbXrayYB.Value;
            }
            set
            {
                ctbXrayYB.Value = value;
            }
        }

        /// <summary>
        /// 回転中心校正X線管Y軸移動有り
        /// </summary>
        public bool stsRotXrayYCh
        {
            get
            {
                return ctbRotXrayYCh.Value;
            }
            set
            {
                ctbRotXrayYCh.Value = value;
            }
        }

        /// <summary>
        /// 寸法校正X線管Y軸移動有り
        /// </summary>
        public bool stsDisXrayYCh
        {
            get
            {
                return ctbDisXrayYCh.Value;
            }
            set
            {
                ctbDisXrayYCh.Value = value;
            }
        }

        /// <summary>
        /// X線管正転限
        /// </summary>
        public bool stsXrayCWLimit
        {
            get
            {
                return ctbXrayCWLimit.Value;
            }
            set
            {
                ctbXrayCWLimit.Value = value;
            }
        }

        /// <summary>
        /// X線管逆転限
        /// </summary>
        public bool stsXrayCCWLimit
        {
            get
            {
                return ctbXrayCCWLimit.Value;
            }
            set
            {
                ctbXrayCCWLimit.Value = value;
            }
        }

        /// <summary>
        /// X線管正転中
        /// </summary>
        public bool stsXrayCW
        {
            get
            {
                return ctbXrayCW.Value;
            }
            set
            {
                ctbXrayCW.Value = value;
            }
        }

        /// <summary>
        /// X線管逆転中
        /// </summary>
        public bool stsXrayCCW
        {
            get
            {
                return ctbXrayCCW.Value;
            }
            set
            {
                ctbXrayCCW.Value = value;
            }
        }

        /// <summary>
        /// X線管回転動作不可
        /// </summary>
        public bool stsXrayRotLock
        {
            get
            {
                return _stsXrayRotLock;
            }
            set
            {
                _stsXrayRotLock = value;
            }
        }

        /// <summary>
        /// 試料扉電磁ﾛｯｸｷｰ挿入完了
        /// </summary>
        public bool stsDoorKey
        {
            get
            {
                return ctbDoorKey.Value;
            }
            set
            {
                ctbDoorKey.Value = value;
            }
        }

        /// <summary>
        /// 試料扉電磁ﾛｯｸON中
        /// </summary>
        public bool stsDoorLock
        {
            get
            {
                return ctbDoorLock.Value;
            }
            set
            {
                ctbDoorLock.Value = value;
            }
        }

        /// <summary>
        /// X線EXM ON中
        /// </summary>
        public bool stsEXMOn
        {
            get
            {
                return ctbEXMOn.Value;
            }
            set
            {
                ctbEXMOn.Value = value;
            }
        }

        /// <summary>
        /// X線EXM ON可能
        /// </summary>
        public bool stsEXMReady
        {
            get
            {
                return ctbEXMReady.Value;
            }
            set
            {
                ctbEXMReady.Value = value;
            }
        }

        /// <summary>
        /// X線EXM発生装置正常
        /// </summary>
        public bool stsEXMNormal1
        {
            get
            {
                return ctbEXMNormal1.Value;
            }
            set
            {
                ctbEXMNormal1.Value = value;
            }
        }

        /// <summary>
        /// X線EXMﾃﾞｰﾀ書込正常
        /// </summary>
        public bool stsEXMNormal2
        {
            get
            {
                return _stsEXMNormal2;
            }
            set
            {
                _stsEXMNormal2 = value;
            }
        }

        /// <summary>
        /// X線EXMｳｫｰﾑｱｯﾌﾟ必要又は実行中
        /// </summary>
        public bool stsEXMWU
        {
            get
            {
                return ctbEXMWU.Value;
            }
            set
            {
                ctbEXMWU.Value = value;
            }
        }

        /// <summary>
        /// X線EXMﾘﾓｰﾄ中
        /// </summary>
        public bool stsEXMRemote
        {
            get
            {
                return _stsEXMRemote;
            }
            set
            {
                _stsEXMRemote = value;
            }
        }

        /// <summary>
        /// CT用I.I.撮影位置
        /// </summary>
        public bool stsCTIIPos
        {
            get
            {
                return ctbCTIIPos.Value;
            }
            set
            {
                ctbCTIIPos.Value = value;
            }
        }

        /// <summary>
        /// 高速用I.I.撮影位置
        /// </summary>
        public bool stsTVIIPos
        {
            get
            {
                return ctbTVIIPos.Value;
            }
            set
            {
                ctbTVIIPos.Value = value;
            }
        }

        /// <summary>
        /// CT用I.I.切替中
        /// </summary>
        public bool stsCTIIDrive
        {
            get
            {
                return _stsCTIIDrive;
            }
            set
            {
                _stsCTIIDrive = value;
            }
        }

        /// <summary>
        /// 高速用I.I.切替中
        /// </summary>
        public bool stsTVIIDrive
        {
            get
            {
                return ctbTVIIDrive.Value;
            }
            set
            {
                ctbTVIIDrive.Value = value;
            }
        }

        /// <summary>
        /// 高速用I.I.視野9”
        /// </summary>
        public bool stsTVII9
        {
            get
            {
                return ctbTVII9.Value;
            }
            set
            {
                ctbTVII9.Value = value;
            }
        }

        /// <summary>
        /// 高速用I.I.視野6”
        /// </summary>
        public bool stsTVII6
        {
            get
            {
                return ctbTVII6.Value;
            }
            set
            {
                ctbTVII6.Value = value;
            }
        }

        /// <summary>
        /// 高速用I.I.視野4.5”
        /// </summary>
        public bool stsTVII4
        {
            get
            {
                return ctbTVII4.Value;
            }
            set
            {
                ctbTVII4.Value = value;
            }    
        }

        /// <summary>
        /// 高速用I.I.電源
        /// </summary>
        public bool stsTVIIPower
        {
            get
            {
                return ctbTVIIPower.Value;
            }
            set
            {
                ctbTVIIPower.Value = value;
            }
        }

        /// <summary>
        /// 高速用I.I.電源
        /// </summary>
        public bool stsCameraPower
        {
            get
            {
                return ctbCameraPower.Value;
            }
            set
            {
                ctbCameraPower.Value = value;
            }
        }

        /// <summary>
        /// I.I.絞り左開中
        /// </summary>
        public bool stsIrisLOpen
        {
            get
            {
                return ctbIrisLOpen.Value;
            }
            set
            {
                ctbIrisLOpen.Value = value;
            }
        }

        /// <summary>
        /// I.I.絞り左閉中
        /// </summary>
        public bool stsIrisLClose
        {
            get
            {
                return ctbIrisLClose.Value;
            }
            set
            {
                ctbIrisLClose.Value = value;
            }
        }

        /// <summary>
        /// I.I.絞り右開中
        /// </summary>
        public bool stsIrisROpen
        {
            get
            {
                return ctbIrisROpen.Value;
            }
            set
            {
                ctbIrisROpen.Value = value;
            }
        }

        /// <summary>
        /// I.I.絞り右閉中
        /// </summary>
        public bool stsIrisRClose
        {
            get
            {
                return ctbIrisRClose.Value;
            }
            set
            {
                ctbIrisRClose.Value = value;
            }
        }

        /// <summary>
        /// I.I.絞り上開中
        /// </summary>
        public bool stsIrisUOpen
        {
            get
            {
                return ctbIrisUOpen.Value;
            }
            set
            {
                ctbIrisUOpen.Value = value;
            }
        }

        /// <summary>
        /// I.I.絞り上閉中
        /// </summary>
        public bool stsIrisUClose
        {
            get
            {
                return ctbIrisUClose.Value;
            }
            set
            {
                ctbIrisUClose.Value = value;
            }
        }

        /// <summary>
        /// I.I.絞り下開中
        /// </summary>
        public bool stsIrisDOpen
        {
            get
            {
                return ctbIrisDOpen.Value;
            }
            set
            {
                ctbIrisDOpen.Value = value;
            }
        }

        /// <summary>
        /// I.I.絞り下閉中
        /// </summary>
        public bool stsIrisDClose
        {
            get
            {
                return ctbIrisDClose.Value;
            }
            set
            {
                ctbIrisDClose.Value = value;
            }
        }

        /// <summary>
        /// 動作制限自動復帰設定状態   '追加 by 稲葉 10-10-19
        /// </summary>
        public bool stsAutoRestrict
        {
            get
            {
                return _stsAutoRestrict;
            }
            set
            {
                _stsAutoRestrict = value;
            }
        }

        /// <summary>
        /// Y軸ｲﾝﾃﾞｯｸｽ減速設定状態     '追加 by 稲葉 10-10-19
        /// </summary>
        public bool stsYIndexSlow
        {
            get
            {
                return _stsYIndexSlow;
            }
            set
            {
                _stsYIndexSlow = value;
            }
        }

        /// <summary>
        /// 動作用扉ｲﾝﾀｰﾛｯｸ設定状態    '追加 by 稲葉 10-10-19
        /// </summary>
        public bool stsDoorPermit
        {
            get
            {
                return ctbDoorPermit.Value;
            }
            set
            {
                ctbDoorPermit.Value = value;
            }
        }

        /// <summary>
        /// ｼｬｯﾀ位置    '追加 by 稲葉 10-11-22
        /// </summary>
        public bool stsShutter
        {
            get
            {
                return ctbShutter.Value;
            }
            set
            {
                ctbShutter.Value = value;
            }
        }

        /// <summary>
        /// ｼｬｯﾀ動作中  '追加 by 稲葉 10-11-22
        /// </summary>
        public bool stsShutterBusy
        {
            get
            {
                return _stsShutterBusy;
            }
            set
            {
                _stsShutterBusy = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙ左右原点復帰要求
        /// </summary>
        public bool stsXOrgReq
        {
            get
            {
                return ctbXOrgReq.Value;
            }
            set
            {
                ctbXOrgReq.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙ前後原点復帰要求
        /// </summary>
        public bool stsYOrgReq
        {
            get
            {
                return ctbYOrgReq.Value;
            }
            set
            {
                ctbYOrgReq.Value = value;
            }
        }

        /// <summary>
        /// 検出器前後原点復帰要求
        /// </summary>
        public bool stsIIOrgReq
        {
            get
            {
                return ctbIIOrgReq.Value;
            }
            set
            {
                ctbIIOrgReq.Value = value;
            }
        }

        /// <summary>
        /// 検出器切替原点復帰要求
        /// </summary>
        public bool stsIIChgOrgReq
        {
            get
            {
                return ctbIIChgOrgReq.Value;
            }
            set
            {
                ctbIIChgOrgReq.Value = value;
            }
        }

        /// <summary>
        /// ﾒｶﾘｾｯﾄ中                  '追加 by 稲葉 10-09-10
        /// </summary>
        public bool stsMechaRstBusy
        {
            get
            {
                return ctbMechaRstBusy.Value;
            }
            set
            {
                ctbMechaRstBusy.Value = value;
            }
        }

        /// <summary>
        /// ﾒｶﾘｾｯﾄ完了                '追加 by 稲葉 10-09-10
        /// </summary>
        public bool stsMechaRstOK
        {
            get
            {
                return ctbMechaRstOK.Value;
            }
            set
            {
                ctbMechaRstOK.Value = value;
            }
        }

        /// <summary>
        /// FID
        /// </summary>
        public int stsFID
        {
            get
            {
                return (int)ctbFID.Value;
            }
            set
            {
                ctbFID.Value = value;
            }
        }

        /// <summary>
        /// FCD
        /// </summary>
        public int stsFCD
        {
            get
            {
                return (int)ctbFCD.Value;
            }
            set
            {
                ctbFCD.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙX最低速度
        /// </summary>
        public int stsXMinSpeed
        {
            get
            {
                return (int)ctbXMinSpeed.Value;
            }
            set
            {
                ctbXMinSpeed.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙX最高速度
        /// </summary>
        public int stsXMaxSpeed
        {
            get
            {
                return (int)ctbXMaxSpeed.Value;
            }
            set
            {
                ctbXMaxSpeed.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙX運転速度
        /// </summary>
        public int stsXSpeed
        {
            get
            {
                return (int)ctbXSpeed.Value;
            }
            set
            {
                ctbXSpeed.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙY最低速度
        /// </summary>
        public int stsYMinSpeed
        {
            get
            {
                return (int)ctbYMinSpeed.Value;
            }
            set
            {
                ctbYMinSpeed.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙY最高速度
        /// </summary>
        public int stsYMaxSpeed
        {
            get
            {
                return (int)ctbYMaxSpeed.Value;
            }
            set
            {
                ctbYMaxSpeed.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙY運転速度
        /// </summary>
        public int stsYSpeed
        {
            get
            {
                return (int)ctbYSpeed.Value;
            }
            set
            {
                ctbYSpeed.Value = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙY運転速度
        /// </summary>
        public int stsFcdDecelerationSpeed = 10;        /// <summary>
        /// ﾃｰﾌﾞﾙX現在値
        /// </summary>
        public int stsXPosition
        {
            get
            {
                return (int)ctbXPosition.Value;
            }
            set
            {
                ctbXPosition.Value = value;
            }
        }

        /// <summary>
        /// I.I.最低速度
        /// </summary>
        public int stsIIMinSpeed
        {
            get
            {
                return (int)ctbIIMinSpeed.Value;
            }
            set
            {
                ctbIIMinSpeed.Value = value;
            }
        }

        /// <summary>
        /// I.I.最高速度
        /// </summary>
        public int stsIIMaxSpeed
        {
            get
            {
                return (int)ctbIIMaxSpeed.Value;
            }
            set
            {
                ctbIIMaxSpeed.Value = value;
            }
        }

        /// <summary>
        /// I.I.運転速度
        /// </summary>
        public int stsIISpeed
        {
            get
            {
                return (int)ctbIISpeed.Value;
            }
            set
            {
                ctbIISpeed.Value = value;
            }
        }

        /// <summary>
        /// 昇降手動運転速度
        /// </summary>
        public int stsUDSpeed
        {
            get
            {
                return _stsUDSpeed;
            }
            set
            {
                _stsUDSpeed = value;
            }
        }

        /// <summary>
        /// 昇降位置決め位置
        /// </summary>
        public int stsUDIndexPos
        {
            get
            {
                return _stsUDIndexPos;
            }
            set
            {
                _stsUDIndexPos = value;
            }
        }

        /// <summary>
        /// 回転手動運転速度
        /// </summary>
        public int stsRotSpeed
        {
            get
            {
                return _stsRotSpeed;
            }
            set
            {
                _stsRotSpeed = value;
            }
        }

        /// <summary>
        /// 回転位置決め位置
        /// </summary>
        public int stsRotIndexPos
        {
            get
            {
                return _stsRotIndexPos;
            }
            set
            {
                _stsRotIndexPos = value;
            }
        }

        /// <summary>
        /// 微調X手動運転速度
        /// </summary>
        public int stsXStgSpeed
        {
            get
            {
                return _stsXStgSpeed;
            }
            set
            {
                _stsXStgSpeed = value;
            }
        }

        /// <summary>
        /// 微調X位置決め位置
        /// </summary>
        public int stsXStgIndexPos
        {
            get
            {
                return _stsXStgIndexPos;
            }
            set
            {
                _stsXStgIndexPos = value;
            }
        }

        /// <summary>
        /// 微調Y手動運転速度
        /// </summary>
        public int stsYStgSpeed
        {
            get
            {
                return _stsYStgSpeed;
            }
            set
            {
                _stsYStgSpeed = value;
            }
        }

        /// <summary>
        /// 微調Y位置決め位置
        /// </summary>
        public int stsYStgIndexPos
        {
            get
            {
                return _stsYStgIndexPos;
            }
            set
            {
                _stsYStgIndexPos = value;
            }
        }

        /// <summary>
        /// ﾃｰﾌﾞﾙY軸現在位置
        /// </summary>
        public int stsYPosition
        {
            get
            {
                return _stsYPosition;
            }
            set
            {
                _stsYPosition = value;
            }
        }

        /// <summary>
        /// X線管FCD
        /// </summary>
        public int stsXrayFCD
        {
            get
            {
                return _stsXrayFCD;
            }
            set
            {
                _stsXrayFCD = value;
            }
        }

        /// <summary>
        /// X線管X軸最低速度
        /// </summary>
        public int stsXrayXMinSp
        {
            get
            {
                return (int)ctbXrayXMinSp.Value;
            }
            set
            {
                ctbXrayXMinSp.Value = value;
            }
        }

        /// <summary>
        /// X線管X軸最高速度
        /// </summary>
        public int stsXrayXMaxSp
        {
            get
            {
                return (int)ctbXrayXMaxSp.Value;
            }
            set
            {
                ctbXrayXMaxSp.Value = value;
            }
        }

        /// <summary>
        /// X線管X軸運転速度
        /// </summary>
        public int stsXrayXSpeed
        {
            get
            {
                return (int)ctbXrayXSpeed.Value;
            }
            set
            {
                ctbXrayXSpeed.Value = value;
            }
        }

        /// <summary>
        /// X線管X軸現在位置
        /// </summary>
        public int stsXrayXPos
        {
            get
            {
                return _stsXrayXPos;
            }
            set
            {
                _stsXrayXPos = value;
            }
        }

        /// <summary>
        /// X線管Y軸最低速度
        /// </summary>
        public int stsXrayYMinSp
        {
            get
            {
                return (int)ctbXrayYMinSp.Value;
            }
            set
            {
                ctbXrayYMinSp.Value = value;
            }
        }

        /// <summary>
        /// X線管Y軸最高速度
        /// </summary>
        public int stsXrayYMaxSp
        {
            get
            {
                return (int)ctbXrayYMaxSp.Value;
            }
            set
            {
                ctbXrayYMaxSp.Value = value;
            }
        }

        /// <summary>
        /// X線管Y軸運転速度
        /// </summary>
        public int stsXrayYSpeed
        {
            get
            {
                return (int)ctbXrayYSpeed.Value;
            }
            set
            {
                ctbXrayYSpeed.Value = value;
           }
        }

        /// <summary>
        /// X線管Y軸現在位置
        /// </summary>
        public int stsXrayYPos
        {
            get
            {
                return _stsXrayYPos;
            }
            set
            {
                _stsXrayYPos = value;
            }
        }

        /// <summary>
        /// X線管回転最低速度
        /// </summary>
        public int stsXrayRotMinSp
        {
            get
            {
                return _stsXrayRotMinSp;
            }
            set
            {
                _stsXrayRotMinSp = value;
            }
        }

        /// <summary>
        /// X線管回転最高速度
        /// </summary>
        public int stsXrayRotMaxSp
        {
            get
            {
                return _stsXrayRotMaxSp;
            }
            set
            {
                _stsXrayRotMaxSp = value;
            }
        }

        /// <summary>
        /// X線管回転運転速度
        /// </summary>
        public int stsXrayRotSpeed
        {
            get
            {
                return _stsXrayRotSpeed;
            }
            set
            {
                _stsXrayRotSpeed = value;
            }
        }

        /// <summary>
        /// X線管回転現在位置
        /// </summary>
        public int stsXrayRotPos
        {
            get
            {
                return (int)ctbXrayRotPos.Value;
            }
            set
            {
                ctbXrayRotPos.Value = value;
            }
        }

        /// <summary>
        /// X線管回転加速度
        /// </summary>
        public int stsXrayRotAccel
        {
            get
            {
                return _stsXrayRotAccel;
            }
            set
            {
                _stsXrayRotAccel = value;
            }
        }

        /// <summary>
        /// X線EXM最大出力値
        /// </summary>
        public int stsEXMMaxW
        {
            get
            {
                return _stsEXMMaxW;
            }
            set
            {
                _stsEXMMaxW = value;
            }
        }

        /// <summary>
        /// X線EXM最大管電圧値
        /// </summary>
        public int stsEXMMaxTV
        {
            get
            {
                return (int)ctbEXMMaxTV.Value;
            }
            set
            {
                ctbEXMMaxTV.Value = value;
            }
        }

        /// <summary>
        /// X線EXM最小管電圧値
        /// </summary>
        public int stsEXMMinTV
        {
            get
            {
                return (int)ctbEXMMinTV.Value;
            }
            set
            {
                ctbEXMMinTV.Value = value;
            }
        }

        /// <summary>
        /// X線EXM最大管電流値
        /// </summary>
        public int stsEXMMaxTC
        {
            get
            {
                return (int)ctbEXMMaxTC.Value;
            }
            set
            {
                ctbEXMMaxTC.Value = value;
            }
        }

        /// <summary>
        /// X線EXM最小管電流値
        /// </summary>
        public int stsEXMMinTC
        {
            get
            {
                return (int)ctbEXMMinTC.Value;
            }
            set
            {

                ctbEXMMinTC.Value = value;
            }
        }

        /// <summary>
        /// X線EXM制限管電圧値
        /// </summary>
        public int stsEXMLimitTV
        {
            get
            {
                return _stsEXMLimitTV;
            }
            set
            {
                _stsEXMLimitTV = value;
            }
        }

        /// <summary>
        /// X線EXM制限管電流値
        /// </summary>
        public int stsEXMLimitTC
        {
            get
            {
                return _stsEXMLimitTC;
            }
            set
            {
                _stsEXMLimitTC = value;
            }
        }

        /// <summary>
        /// X線EXM管電圧実測値
        /// </summary>
        public int stsEXMTV
        {
            get
            {
                return (int)ctbEXMTV.Value;
            }
            set
            {
                ctbEXMTV.Value = value;
            }
        }

        /// <summary>
        /// X線EXM管電流実測値
        /// </summary>
        public int stsEXMTC
        {
            get
            {
                return (int)ctbEXMTC.Value;
            }
            set
            {
                ctbEXMTC.Value = value;
            }
        }

        /// <summary>
        /// X線EXM管電圧設定値
        /// </summary>
        public int stsEXMTVSet
        {
            get
            {
                return (int)ctbEXMTVSet.Value;
            }
            set
            {
                ctbEXMTVSet.Value = value;
            }
        }

        /// <summary>
        /// X線EXM管電流設定値
        /// </summary>
        public int stsEXMTCSet
        {
            get
            {
                return (int)ctbEXMTCSet.Value;
            }
            set
            {
                ctbEXMTCSet.Value = value;
            }
        }

        /// <summary>
        /// X線EXMｴﾗｰｺｰﾄﾞ
        /// </summary>
        public int stsEXMErrCode
        {
            get
            {
                return (int)ctbEXMErrCode.Value;
            }
            set
            {
                ctbEXMErrCode.Value = value;
            }
        }

        /// <summary>
        /// シーケンサがレディ状態になったか　v11.5追加 by 間々田 2006/07/12
        /// </summary>
        public bool IsReady
        {
            get
            {
                return _IsReady;
            }
            set
            {
                _IsReady = value;
            }
        }


        /// <summary>
        /// 回転大ﾃｰﾌﾞﾙ有無    '追加 by 稲葉 14-02-26    //2014/07/19_v19.51反映
        /// </summary>
        public bool stsRotLargeTable
        {
            get
            {
                return ctbRotLargeTable.Value;
            }
            set
            {
                ctbRotLargeTable.Value = value;
            }
        }

        /// <summary>
        /// 検査室入室安全ｽｲｯﾁ '追加 by 稲葉 14-03-05  //2014/07/19_v19.51反映
        /// </summary>
        public bool stsRoomInSw
        {
            get
            {
                return ctbRoomInSw.Value;
            }
            set
            {
                ctbRoomInSw.Value = value;
            }
        }

        /// <summary>
        /// 冷蔵箱ドア開閉 '追加 Rev22.00 by 長野 15-08-21
        /// /// </summary>
        public bool stsColdBoxDoorClose
        {
            get
            {
                return _stsColdBoxDoorClose;
            }
            set
            {
                _stsColdBoxDoorClose = value;
            }
        }

        /// <summary>
        /// 冷蔵箱位置確認 '追加 Rev22.00 by 長野 15-08-21
        /// </summary>
        public bool stsColdBoxPosOK
        {
            get
            {
                return _stsColdBoxPosOK;
            }
            set
            {
                _stsColdBoxPosOK = value;
            }
        }

        #endregion

        /// <summary>
        /// 回転傾斜テーブル 傾斜リセット //Rev22.00 追加 by長野 2015/08/21
        /// </summary>
        public bool TiltOrigin
        {
            get
            {
                return true;
            }
            set
            {

            }
        }

        /// <summary>
        /// 回転傾斜テーブルテーブル 回転リセット //Rev22.00 追加 by長野 2015/08/21
        /// </summary>
        public bool TiltRotOrigin
        {
            get
            {
                return true;
            }
            set
            {

            }
        }

        /// <summary>
        /// FDD(リニアスケール) Rev23.10 追加 by長野 2015/09/18
        /// </summary>
        public int stsLinearFDD
        {
            get
            {
                return _stsLinearFDD;
            }
            set
            {
                _stsLinearFDD = value;
            }
        }

        /// <summary>
        /// Y軸(リニアスケール) Rev23.10 追加 by長野 2015/09/18
        /// </summary>
        public int stsLinearTableY
        {
            get
            {
                return _stsLinearTableY;
            }
            set
            {
                _stsLinearTableY = value;
            }
        }

        /// <summary>
        /// FCD軸(リニアスケール) Rev23.10 追加 by長野 2015/09/18
        /// </summary>
        public int stsLinearFCD
        {
            get
            {
                return _stsLinearFCD;
            }
            set
            {
                _stsLinearFCD = value;
            }
        }

        /// <summary>
        /// ﾏｲｸﾛﾌｫｰｶｽX線検出器位置 追加 Rev23.10 by長野 2015/09/18
        /// </summary>
        public bool stsMicroFPDPos
        {
            get
            {
                return _stsMicroFPDPos;
            }
            set
            {
                _stsMicroFPDPos = value;
            }

        }

        /// <summary>
        /// ﾅﾉﾌｫｰｶｽX線検出器位置 追加 Rev23.10 by長野 2015/09/18
        /// </summary>
        public bool stsNanoFPDPos
        {
            get
            {
                return _stsNanoFPDPos;
            }
            set
            {
                _stsNanoFPDPos = value;
            }

        }
   
        /// <summary>
        /// ﾏｲｸﾛﾌｫｰｶｽX線検出器ｼﾌﾄ位置 追加 Rev23.10 by長野 2015/09/18
        /// </summary>
        public bool stsMicroFPDShiftPos
        {
            get
            {
                return _stsMicroFPDShiftPos;
            }
            set
            {
                _stsMicroFPDShiftPos = value;
            }
        }

        /// <summary>
        /// ﾅﾉﾌｫｰｶｽX線検出器ｼﾌﾄ位置 追加 Rev23.10 by長野 2015/09/18
        /// </summary>
        public bool stsNanoFPDShiftPos
        {
            get
            {
                return _stsNanoFPDShiftPos;
            }
            set
            {
                _stsNanoFPDShiftPos = value;
            }
        } 

        /// <summary>
        /// ﾏｲｸﾛﾌｫｰｶｽX線切替中 追加 Rev23.10 by長野 2015/09/18
        /// </summary>
        public bool stsMicroFPDBusy
        {
            get
            {
                return _stsMicroFPDBusy;
            }
            set
            {
                _stsMicroFPDBusy = value;
            }
        }

        /// <summary>
        /// ﾅﾉﾌｫｰｶｽX線切替中 追加 Rev23.10 by長野 2015/09/18
        /// </summary>
        public bool stsNanoFPDBusy
        {
            get
            {
                return _stsNanoFPDBusy;
            }
            set
            {
                _stsNanoFPDBusy = value;
            }
        }

        /// <summary>
        /// ﾏｲｸﾛﾌｫｰｶｽX線検出器ｼﾌﾄ中 追加 Rev23.10 by長野 2015/09/18
        /// </summary>
        public bool stsMicroFPDShiftBusy
        {
            get
            {
                return _stsMicroFPDShiftBusy;
            }
            set
            {
                _stsMicroFPDShiftBusy = value;
            }
        }

        /// <summary>
        /// ﾅﾉﾌｫｰｶｽX線検出器ｼﾌﾄ中 追加 Rev23.10 by長野 2015/09/18
        /// </summary>
        public bool stsNanoFPDShiftBusy
        {
            get
            {
                return _stsNanoFPDShiftBusy;
            }
            set
            {
                _stsNanoFPDShiftBusy = value;
            }
        }

        /// <summary>
        /// FCD軸ｲﾝﾀｰﾛｯｸﾘﾐｯﾄ位置補正 追加 Rev23.10 by長野 2015/09/18
        /// </summary>
        public int stsFCDLimitAdj
        {
            get
            {
                return _stsFCDLimitAdj;
            }
            set
            {
                _stsFCDLimitAdj = value;
            }
        }

        /// <summary>
        /// 左シフト位置 追加 Rev23.20 by長野 2016/01/13
        /// </summary>
        public bool stsFPDLShiftPos
        {
            get
            {
                return _stsFPDLShiftPos;
            }
            set
            {
                _stsFPDLShiftPos = value;
            }
        }

        /// <summary>
        /// 左シフト移動中 追加 Rev23.20 by長野 2016/01/13
        /// </summary>
        public bool stsFPDLShiftBusy
        {
            get
            {
                return _stsFPDLShiftBusy;
            }
            set
            {
                _stsFPDLShiftBusy = value;
            }
        }

        //Rev23.20 追加 by長野 2016/01/19 --->
        /// <summary>
        /// 2・3世代モード 追加 Rev23.20 by長野 2016/01/13
        /// </summary>
        public bool stsFDSystemPos
        {
            get
            {
                return _stsFDSystemPos;
            }
            set
            {
                _stsFDSystemPos = value;
            }
        }

        /// <summary>
        /// 2・3世代モード 追加 Rev23.20 by長野 2016/01/13
        /// </summary>
        public bool stsFDSystemBusy
        {
            get
            {
                return _stsFDSystemBusy;
            }
            set
            {
                _stsFDSystemBusy = value;
            }
        }

        /// <summary>
        /// シーケンサに記録されているX線Off時間(年)を取得 (管電圧Hのとき)
        /// </summary>
        public int stsXrayHOffTimeY
        {
            get
            {
                return _stsXrayHOffTimeY;
            }
            set
            {
                _stsXrayHOffTimeY = value;
            }
        }

        /// <summary>
        /// シーケンサに記録されているX線Off時間(月日)を取得 (管電圧Hのとき)
        /// </summary>
        public int stsXrayHOffTimeMD
        {
            get
            {
                return _stsXrayHOffTimeMD;
            }
            set
            {
                _stsXrayHOffTimeMD = value;
            }
        }

        /// <summary>
        /// シーケンサに記録されているX線Off時間(時分)を取得 (管電圧Hのとき)
        /// </summary>
        public int stsXrayHOffTimeHM
        {
            get
            {
                return _stsXrayHOffTimeHM;
            }
            set
            {
                _stsXrayHOffTimeHM = value;
            }
        }

            /// <summary>
        /// シーケンサに記録されているX線Off時間(年)を取得 (管電圧Mのとき)
        /// </summary>
        public int stsXrayMOffTimeY
        {
            get
            {
                return _stsXrayMOffTimeY;
            }
            set
            {
                _stsXrayMOffTimeY = value;
            }
        }

        /// <summary>
        /// シーケンサに記録されているX線Off時間(月日)を取得 (管電圧Mのとき)
        /// </summary>
        public int stsXrayMOffTimeMD
        {
            get
            {
                return _stsXrayMOffTimeMD;
            }
            set
            {
                _stsXrayMOffTimeMD = value;
            }
        }

        /// <summary>
        /// シーケンサに記録されているX線Off時間(時分)を取得 (管電圧Mのとき)
        /// </summary>
        public int stsXrayMOffTimeHM
        {
            get
            {
                return _stsXrayMOffTimeHM;
            }
            set
            {
                _stsXrayMOffTimeHM = value;
            }
        }
        //<---

        //Rev23.40 追加 by長野 2016/06/19 --->
        public bool stsUpLimitPermit
        {
            get
            {
                return _stsUpLimitPermit;
            }
            set
            {
                _stsUpLimitPermit = value;
            }
        }

        public int stsUpLimitPos
        {
            get
            {
                return _stsUpLimitPos;
            }
            set
            {
                _stsUpLimitPos = value;
            }
        }
        //<---

        //v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここから
        //*******************************************************************************
        //機　　能： フォームロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v18.00  2011/03/12  やまおか    新規作成
        //*******************************************************************************
        private void frmVirtualSeqComm_Load(System.Object eventSender, System.EventArgs eventArgs)
        {

            //コントロールの初期設定
            InitControls();

        }
        //*******************************************************************************
        //機　　能： 各コントロールの初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v18.00  2011/03/12  やまおか    新規作成
        //*******************************************************************************
        private void InitControls()
        {

            if ((CTSettings.scaninh.Data.avmode == 0))
            {
                stsFCD = 7500;
                stsFID = 11000;
            }

        }

		//*******************************************************************************
		//機　　能： BitWriteメソッド
		//
		//           変数名          [I/O] 型        内容
		//引　　数： theCommand      [I/ ] String    メソッドコマンド
		//           theParameter    [I/ ] Boolean   パラメーター
		//戻 り 値： なし
		//
		//補　　足：
		//
		//履　　歴： v11.2  05/03/18  (SI3)間々田    新規作成
		//*******************************************************************************
		public int BitWrite(string theCommand, bool theParameter)
		{
			//受信ログを表示する
			lstInfo.Items.Add("BitWrite " + theCommand + " " + Convert.ToString(theParameter));

			if (theParameter)
			{
				switch (theCommand)
				{
					case "II9":
					case "II6":
					case "II4":
						stsII9 = false;
						stsII6 = false;
						stsII4 = false;
						break;
					case "Filter0":
					case "Filter1":
					case "Filter2":
					case "Filter3":
					case "Filter4":
					case "Filter5":
                    case "Shutter":     //v18.00 Shutter追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        stsFilter0 = false;
						stsFilter1 = false;
						stsFilter2 = false;
						stsFilter3 = false;
						stsFilter4 = false;
						stsFilter5 = false;
						stsShutter = false;   //v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
						break;
				}

				switch (theCommand)
				{
					case "GainIIChangeSet":
                        stsGainIIChange = true;
						break;
					case "GainIIChangeReset":
                        stsGainIIChange = false;
						break;

					case "SPIIChangeSet":
						stsSPIIChange = true;
						break;
					case "SPIIChangeReset":
						stsSPIIChange = false;
						break;

					case "VerIIChangeSet":
						stsVerIIChange = true;
						break;
					case "VerIIChangeReset":
						stsVerIIChange = false;
						break;

					case "RotXChangeSet":
						stsRotXChange = true;
						break;
					case "RotXChangeReset":
						stsRotXChange = false;
						break;

					case "RotYChangeSet":
						stsRotYChange = true;
						break;
					case "RotYChangeReset":
						stsRotYChange = false;
						break;

					case "RotIIChangeSet":
						stsRotIIChange = true;
						break;
					case "RotIIChangeReset":
						stsRotIIChange = false;
						break;

					case "DisXChangeSet":
						stsDisXChange = true;
						break;
					case "DisXChangeReset":
						stsDisXChange = false;
						break;

					case "DisYChangeSet":
						stsDisYChange = true;
						break;
					case "DisYChangeReset":
						stsDisYChange = false;
						break;

					case "DisIIChangeSet":
						stsDisIIChange = true;
						break;
					case "DisIIChangeReset":
						stsDisIIChange = false;
						break;

					case "IIPowerOn":
						stsIIPower = true;
						break;
					case "IIPowerOff":
						stsIIPower = false;
						break;

					case "SlightOn":
						stsSLight = true;
						break;
					case "SlightOff":
						stsSLight = false;
						break;

					case "II9":
						stsII9 = true;
						break;
					case "II6":
						stsII6 = true;
						break;
					case "II4":
						stsII4 = true;
						break;

					case "Filter0":
                        stsFilter0 = true;
						break;
					case "Filter1":
						stsFilter1 = true;
						break;
					case "Filter2":
                        stsFilter2 = true;
						break;
					case "Filter3":
						stsFilter3 = true;
						break;
					case "Filter4":
                        stsFilter4 = true;
						break;
					case "Filter5":
						stsFilter5 = true;
						break;
                    case "Shutter":
                        stsShutter = true;      //v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        break;

					case "EXMWUNone":
						stsEXMOn = false;
						break;
					case "EXMWUShort":
						stsEXMWU = false;
						break;
					case "EXMWULong":
						stsEXMWU = false;
						break;

					case "EXMOn":
						stsEXMOn = true;
						break;
					case "EXMOff":
						stsEXMOn = false;
						break;
				}
			}

			//v16.01 追加 by 山影 10-02-04
			switch (theCommand)
			{
				case "XLeft":
					stsXLeft = theParameter;
					break;
				case "XRight":
					stsXRight = theParameter;
					break;
				case "YForward":
					stsYForward = theParameter;
					break;
				case "YBackward":
					stsYBackward = theParameter;
					break;
				case "IIForward":
					stsIIForward = theParameter;
					break;
				case "IIBackward":
					stsIIBackward = theParameter;
					break;

				//高速度透視撮影関係
				case "CTIISet":
				case "IIChangeReset":
					stsCTIIDrive = true;
					break;
				case "TVIISet":
					stsTVIIDrive = true;
					break;

				case "IIChangeStop":
					stsCTIIDrive = false;
					stsTVIIDrive = false;
					break;

				case "TVII9":
					stsTVII9 = true;
					break;
				case "TVII6":
					stsTVII6 = true;
					break;
				case "TVII4":
					stsTVII4 = true;
					break;

				case "TVIIPowerOn":
					stsTVIIPower = true;
					break;
				case "TVIIPowerOff":
					stsTVIIPower = false;
					break;
				case "CameraPowerOn":
					stsCameraPower = true;
					break;
				case "CameraPowerOff":
					stsCameraPower = false;
					break;

				//I.I.絞り
				case "IrisLOpen":
					stsIrisLOpen = theParameter;
					break;
				case "IrisLClose":
					stsIrisLClose = theParameter;
					break;
				case "IrisROpen":
					stsIrisROpen = theParameter;
					break;
				case "IrisRClose":
					stsIrisRClose = theParameter;
					break;
				case "IrisUOpen":
					stsIrisUOpen = theParameter;
					break;
				case "IrisUClose":
					stsIrisUClose = theParameter;
					break;
				case "IrisDOpen":
					stsIrisDOpen = theParameter;
					break;
				case "IrisDClose":
					stsIrisDClose = theParameter;
					break;

				//コリメータ
				case "ColliLOpen":
					stsColliLOpen = theParameter;
					break;
				case "ColliLClose":
					stsColliLClose = theParameter;
					break;
				case "ColliROpen":
					stsColliROpen = theParameter;
					break;
				case "ColliRClose":
					stsColliRClose = theParameter;
					break;
				case "ColliUOpen":
					stsColliUOpen = theParameter;
					break;
				case "ColliUClose":
					stsColliUClose = theParameter;
					break;
				case "ColliDOpen":
					stsColliDOpen = theParameter;
					break;
				case "ColliDClose":
					stsColliDClose = theParameter;
					break;

				case "DoorLockOn":
					stsDoorLock = true;
					break;
				case "DoorLockOff":
					stsDoorLock = false;
					break;

				case "TableMoveRestrict":
					stsTableMovePermit = false;
					break;
				case "TableMovePermit":
					stsTableMovePermit = true;
					break;
			}

			//v17.20 追加 by 長野 10-09-06
			switch (theCommand)
			{
				case "MechaReset":
					stsMechaRstBusy = true;
					break;
				case "MechaResetStop":
					stsMechaRstBusy = false;
					break;
			}

			return 0;
		}


		//*******************************************************************************
		//機　　能： WordWriteメソッド
		//
		//           変数名          [I/O] 型        内容
		//引　　数： theCommand      [I/ ] String    メソッドコマンド
		//           theParameter    [I/ ] String    パラメーター
		//戻 り 値： なし
		//
		//補　　足：
		//
		//履　　歴： v11.2  05/03/18  (SI3)間々田    新規作成
		//*******************************************************************************
		public int WordWrite(string theCommand,ref  string theParameter)
		{
			//受信ログを表示する
			lstInfo.Items.Add("WordWrite " + theCommand + " " + theParameter);

			switch (theCommand)
			{
				case "EXMTVSet":
					decimal value = 0M;
					decimal.TryParse(theParameter, out value);
					ctbEXMTVSet.Value = value;
					break;
			}

			return 0;
		}


		//*******************************************************************************
		//機　　能： StatusReadメソッド
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//
		//戻 り 値： なし
		//
		//補　　足：
		//
		//履　　歴： v11.2  05/03/18  (SI3)間々田    新規作成
		//*******************************************************************************
		public int StatusRead()
		{
			//受信ログを表示する
			lstInfo.Items.Add("StatusRead");

			return 0;
		}


		//追加 by 山影
        //CT切替中は、CT位置、高速位置、高速切替中は解除

		private void ctbCTIIPos_ValueChanged(object sender, EventArgs e)
        {

        }

   
        private void ctbRoomInSw_ValueChanged(object sender, EventArgs e)
        {

            stsRoomInSw = ctbRoomInSw.Value;

        }

        //v19.51 追加 by長野 2014/03/03
        private void ctbRotLargeTable_ValueChanged(object sender, EventArgs e)
        {

            stsRotLargeTable = ctbRotLargeTable.Value;

        }


		private void ctbTVIIDrive_ValueChanged(object sender, EventArgs e)
		{
			if (ctbTVIIDrive.Value == true)
			{
				stsTVIIPos = false;
				stsCTIIPos = false;
				stsCTIIDrive = false;
			}
		}

		private void ctbTVIIPos_ValueChanged(object sender, EventArgs e)
        {

        }

		private void ctbTVII4_ValueChanged(object sender, EventArgs e)
        {

        }

		private void ctbTVII6_ValueChanged(object sender, EventArgs e)
        {

        }

		private void ctbTVII9_ValueChanged(object sender, EventArgs e)
        {

        }


		//テーブル、I.I.を移動させると、逆方向動作限は解除する
		private void ctbXLeft_ValueChanged(object sender, EventArgs e)
		{
			stsXRLimit = false;
		}

		private void ctbXRight_ValueChanged(object sender, EventArgs e)
		{
			stsXLLimit = false;
		}

		private void ctbYBackward_ValueChanged(object sender, EventArgs e)
		{
			stsYFLimit = false;
		}

		private void ctbYForward_ValueChanged(object sender, EventArgs e)
		{
			stsYBLimit = false;
		}
		private void ctbIIBackward_ValueChanged(object sender, EventArgs e)
		{
			stsIIFLimit = false;
		}

		private void ctbIIForward_ValueChanged(object sender, EventArgs e)
		{
			stsIIBLimit = false;
		}

        private void ctbCTIIPos_Click(object sender, EventArgs e)
        {
            if (ctbCTIIPos.Value == false)
            {
                ctbCTIIPos.Value = true;
                stsCTIIPos = true;
            }
            else
            {
                ctbCTIIPos.Value = false;
                stsCTIIPos = false;
            }
        }

        private void ctbCTIIDrive_Click(object sender, EventArgs e)
        {
            if (ctbCTIIDrive.Value == false)
            {
                ctbCTIIDrive.Value = true;
                stsCTIIDrive = true;
            }
            else
            {
                ctbCTIIDrive.Value = false;
                stsCTIIDrive = false;
            }
        }

        //Rev23.10 ここから追加 by長野 2015/09/28
        private void ctbMicroFPDSet_Click(object sender, EventArgs e)
        {
            if (ctbMicroFPDSet.Value == false)
            {
                ctbMicroFPDSet.Value = true;
                stsMicroFPDPos = true;
            }
            else
            {
                ctbMicroFPDSet.Value = false;
                stsMicroFPDPos = false;
            }
        }

        private void ctbMicroFPDShiftSet_Click(object sender, EventArgs e)
        {
            if (ctbMicroFPDShiftSet.Value == false)
            {
                ctbMicroFPDShiftSet.Value = true;
                stsMicroFPDShiftPos = true;
            }
            else
            {
                ctbMicroFPDShiftSet.Value = false;
                stsMicroFPDShiftPos = false;
            }
        }

        private void ctbNanoFPDSet_Click(object sender, EventArgs e)
        {
            if (ctbNanoFPDSet.Value == false)
            {
                ctbNanoFPDSet.Value = true;
                stsNanoFPDPos = true;
            }
            else
            {
                ctbNanoFPDSet.Value = false;
                stsNanoFPDPos = false;
            }
        }

        private void ctbNanoFPDShiftSet_Click(object sender, EventArgs e)
        {
            if (ctbNanoFPDShiftSet.Value == false)
            {
                ctbNanoFPDShiftSet.Value = true;
                stsNanoFPDShiftPos = true;
            }
            else
            {
                ctbNanoFPDShiftSet.Value = false;
                stsNanoFPDShiftPos = false;
            }
        }

        private void ctbMicroNanoReset_Click(object sender, EventArgs e)
        {
            if (ctbMicroFPDSet.Value == false)
            {
                ctbMicroFPDSet.Value = true;
                stsMicroFPDPos = true;
            }
            else
            {
                ctbMicroFPDSet.Value = false;
                stsMicroFPDPos = false;
            }
        }

        private void ctbMicroXrayChangeBusy_Click(object sender, EventArgs e)
        {
            if (ctbMicroXrayChangeBusy.Value == false)
            {
                ctbMicroXrayChangeBusy.Value = true;
                _stsMicroFPDBusy = true;
            }
            else
            {
                ctbMicroXrayChangeBusy.Value = false;
                _stsMicroFPDBusy = false;
            }
        }

        private void ctbNanoXrayChangeBusy_Click(object sender, EventArgs e)
        {
            if (ctbNanoXrayChangeBusy.Value == false)
            {
                ctbNanoXrayChangeBusy.Value = true;
                _stsNanoFPDBusy = true;
            }
            else
            {
                ctbNanoXrayChangeBusy.Value = false;
                _stsNanoFPDBusy = false;
            }
        }

        private void ctbMicroFpdShiftBusy_Click(object sender, EventArgs e)
        {
            if (ctbMicroFpdShiftBusy.Value == false)
            {
                ctbMicroFpdShiftBusy.Value = true;
                _stsMicroFPDShiftBusy = true;
            }
            else
            {
                ctbMicroFpdShiftBusy.Value = false;
                _stsMicroFPDShiftBusy = false;
            }
        }

        private void ctbNanoFpdShiftBusy_Click(object sender, EventArgs e)
        {
            if (ctbNanoFpdShiftBusy.Value == false)
            {
                ctbNanoFpdShiftBusy.Value = true;
                _stsNanoFPDShiftBusy = true;
            }
            else
            {
                ctbNanoFpdShiftBusy.Value = false;
                _stsNanoFPDShiftBusy = false;
            }
        }

		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//Private Sub Timer1_Timer()
		//
		//    'RaiseEvent OnCommEnd(0)
		//    nCondTime = nCondTime - 1
		//    pnFilamentTime = pnFilamentTime - 1
		//
		//End Sub
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

#endif



    }
}
