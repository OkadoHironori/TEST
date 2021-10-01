using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO; //Rev21.00 追加 by長野 2015/03/12

// Add Start 2018/08/31 M.Oyama
using System.Threading;
// Add End 2018/08/31
using SeqComm;
//
using CT30K.Common;
using CTAPI;
using TransImage;
//using CT30K.Controls;
//using CT30K.Modules;
using CT30K.Properties;


namespace CT30K
{
	//public partial class frmMechaControl: Form
    public partial class frmMechaControl : FixedForm
	{
        //v16.01 デザイン変更:フレームで項目分け by 山影 10-02-09
        //           fraMechaPos        :機構部位置
        //           fraUpDown          :昇降操作
        //           fraMechaControl    :機構部操作
        //           fraHighSpeedCamera :CT/高速切替
        //           fraCollimator      :コリメータ
        //           fraXrayRotate      :X線管操作
        //           fraAutoSacnPos     :自動スキャン位置指定
        //           fraIris            :I.I.絞り

        //スピード設定定数
        public enum SpeedConstants
        {
	        SpeedSlow,
	        SpeedMiddle,
	        SpeedFast,
	        speedmanual
        }

        //Const MECHA_HEIGHT = 5115 + 375
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
        Const MECHA_HEIGHT = 5115 + 375 - 90 '変更 by 間々田 2009/07/21
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
        public const int MECHA_HEIGHT = 341 + 25 - 6;      //変更 by 間々田 2009/07/21

        //速度設定フォーム
        frmSpeed mySpeedForm;

        //シーケンサ動作指令文字列
        private string LastCommand;

        private bool CommCheckOn;           //v11.3追加 by 間々田 2006/02/20
        
//v11.3追加ここから by 間々田 2006/02/20
#if DebugOn
        private frmVirtualSeqComm UC_SeqComm;   //デバッグ時は仮想シーケンサとする
#else	        
        private Seq UC_SeqComm;                 //シーケンサ通信
#endif
//v11.3追加ここまで by 間々田 2006/02/20  
      

        //イベント宣言
        public event EventHandler FIDChanged;

        public event EventHandler FCDChanged;

        public event EventHandler UDPosChanged;

        public event EventHandler FXChanged;

        public event EventHandler FYChanged;

        public event EventHandler YChanged;

        public event EventHandler IIMovingStopped;       //I.I.の動作が停止した時に生じるイベント '追加 2009/06/15

        public event EventHandler MecainfChanged;         //Mecainfが変更された                    '追加 2009/08/06

        public event EventHandler SeqCommChanged;         //SeqCommが変更された                    '追加 2009/08/06


        //コントロール更新用のデリゲート
        delegate void MechErrorDelegate();
        int MechErrorNo = 0;


        //FCD（含オフセット）
        private float myFCDWithOffset = 0;

        //FID（含オフセット）
        private float myFIDWithOffset = 0;

        //Rev23.10 追加 by長野 2015/10/27/////////////////////////////
        //FCD（含オフセット）メカ指令値
        private float myFCDWithOffsetMecha = 0;

        //FID（含オフセット）メカ指令値
        private float myFIDWithOffsetMecha = 0;

        //FCD（含オフセット）リニアスケール
        private float myFCDWithOffsetLinear = 0;

        //FID（含オフセット）リニアスケール
        private float myFIDWithOffsetLinear = 0;
        //////////////////////////////////////////////////////////

        private struct BooleanStatus        //追加 by 間々田 2009/08/04
		{
            public bool Value;
            public bool lastValue;
            public string Caption;
        }

        private enum SeqIdxConstants
        {
            IdxColliLOLimit,	//左開限
            IdxColliLCLimit,	//左閉限
            IdxColliROLimit,	//右開限
            IdxColliRCLimit,	//右閉限
            IdxColliUOLimit,	//上開限
            IdxColliUCLimit,	//上閉限
            IdxColliDOLimit,	//下開限
            IdxColliDCLimit,	//下閉限
            IdxXLLimit,		    //テーブルＸ左限
            IdxXRLimit,		    //テーブルＸ右限
            IdxYFLimit,		    //テーブルＹ前進限（拡大限）
            IdxYBLimit,		    //テーブルＹ後退限（縮小限）
            IdxXrayXLLimit,		//Ｘ線管Ｘ軸左限
            IdxXrayXRLimit,		//Ｘ線管Ｘ軸右限
            IdxXrayYFLimit,		//Ｘ線管Ｙ軸前進限
            IdxXrayYBLimit,		//Ｘ線管Ｙ軸後退限

            IdxEmergency,		//非常停止
            IdxXray225Trip,		//X線225KVトリップ
            IdxXray160Trip,		//X線160KVトリップ
            IdxFilterTouch,		//フィルタユニット接触
            IdxXray225Touch,	//X線管225KV接触
            IdxXray160Touch,	//X線管160KV接触
            IdxRotTouch,		//回転テーブル接触
            IdxTiltTouch,		//チルトテーブル接触
            IdxXDriverHeat,		//テーブルX軸オーバーヒート
            IdxYDriverHeat,		//テーブルY軸オーバーヒート
            IdxXrayDriverHeat,	//X線管切替オーバーヒート
            IdxSeqCpuErr,		//シーケンサCPUエラー
            IdxSeqBatteryErr,	//シーケンサバッテリーエラー
            IdxSeqKzCommErr,	//シーケンサKL通信エラー(KZ)
            IdxSeqKvCommErr,	//シーケンサKL通信エラー(KV)
            IdxFilterTimeout,	//フィルタタイムアウト
            IdxTiltTimeout,		//チルト原点復帰タイムアウト
            IdxXLTouch,		    //テーブルX左移動中接触
            IdxXRTouch,		    //テーブルX右移動中接触
            IdxYFTouch,		    //テーブルY前進中接触
            IdxYBTouch,		    //テーブルY後退中接触
            IdxXTimeout,		//X軸原点復帰タイムアウト
            IdxIIDriverHeat,	//II軸オーバーヒート
            IdxXDriveErr,		//Ｘ軸制御エラー
            IdxXrayCameraUDError,//X線・高速度カメラ軸エラー

            IdxRotOrigin,		//回転原点復帰
            IdxUdOrigin,		//昇降原点復帰
            IdxXStgOrigin,		//微調X軸原点復帰
            IdxYStgOrigin,		//微調Y軸原点復帰
            IdxTiltAndRot_Tilt_Origin,//回転傾斜テーブル原点復帰(傾斜) //Rev22.00 追加 by長野 2015/08/20
            IdxTiltAndRot_Rot_Origin, //回転傾斜テーブル原点復帰(回転)   //Rev22.00 追加 by長野 2015/08/20

            IdxIIBackward,		//I.I.後退中
            IdxIIForward,		//I.I.前進中

            IdxDoorLock,		//扉インターロック
            IdxRunReadySW,		//運転準備

            IdxTableMovePermit,	//Ｘ線管干渉制限解除

            //v16.01 追加 by 山影 10-02-02
            IdxCTIIDrive,		//CT用I.I.に切替中       'v17.20 検出器切替機能有の場合，検出器１に切替中を示す。 by 長野 2010-09-01
            IdxTVIIDrive,		//高速用I.I.に切替中     'v17.20 検出器切替機能有の場合，検出器２に切替中を示す。 by 長野 2010-09-01
            IdxCTIIPos,		    //I.I.CT位置             'v17.20 検出器切替機能有の場合，検出器１が撮影位置にいることを示す。 by 長野 2010-09-01
            IdxTVIIPos,		    //I.I.高速位置           'v17.20 検出器切替機能有の場合，検出器２が撮影位置にいることを示す。 by 長野 2010-09-01
            IdxUnknownIIPos,	//I.I.は撮影位置にあるか 'v17.20 検出器切替機能有の場合，検出器位置が不定であることを示す。 by 長野 2010-09-01
            IdxOKIIMove,		//I.I.切替可能か         'v17.20 検出器切替機能有の場合，検出器切替可能であることを示す。 by 長野 2010-09-01

            IdxMechaRstBusy,	//メカリセット中   'v17.20  by 長野 2010-09-13
            IdxMechaRstOK,		//メカリセット完了 'v17.20  by 長野 2010-09-13
            IdxXOrgReq,		    //テーブルY軸原点復帰要求 'v17.20 by 長野 2010-09-10
            IdxYOrgReq,		    //FCD原点復帰要求         'v17.20 by 長野 2010-09-10
            IdxIIOrgReq,		//FDD原点復帰要求         'v17.20 by 長野 2010-09-10
            IdxIIChgOrgReq,		//検出器切替軸原点復帰要求'v17.20 by 長野 2010-09-09

            //Rev23.10 X線切替用追加 ここから by長野 2015/09/20
            //切替＋シフトスキャンのため、従来のシフトと重複するステータスもある
            IdxMicroFpdPos,         //X線1位置
            IdxMicroFpdShiftPos,    //X線1位置(シフト)
            IdxNanoFpdPos,          //X線2位置
            IdxNanoFpdShiftPos,     //X線2位置(シフト)
            IdxMicroFpdBusy,        //X線1へ切り替え中
            IdxMicroFpdShiftBusy,   //X線1(シフト)へ切り替え中
            IdxNanoFpdBusy,         //X線2へ切り替え中
            IdxNanoFpdShiftBusy,    //X線2(シフト)へ切り替え中
            IdxUnknownFpdPos,       //位置不定状態
            IdxOkXrayFpdMove,     //FPD移動可能?
            //Rev23.10 X線切替用追加 ここまで by長野 2015/09/20

            //Rev23.20 検出器左右シフト＋2世代・3世代兼用CT用に追加 ここから by長野 2015/12/17
            IdxFPDLShiftPos,        //検出器左シフト位置
            IdxFPDLShiftBusy,       //検出器左シフト移動中
            IdxFDSystemPos,        //検出器FPD選択状態
            IdxFDSystemBusy,       //検出器FPD状態へ移動中
            //Rev23.20 検出器左右シフト＋2世代・3世代兼用CT用に追加 ここまで by長野 2015/12/17

            IdxWarmUpNow,		//WarmUp中

            IdxLargeTable,      //試料テーブル(大)用アタッチメント装着 //Rev26.00 add by chouno 2017/03/13

            IdxUpper            //ダミー 
        }

        private BooleanStatus[] mySeqStatus = new BooleanStatus[(int)SeqIdxConstants.IdxUpper + 1];     //追加 by 間々田 2009/08/04

        private enum SeqIdxValueConstants
        {
            IdxXPosition,       //従来のテーブルＸ（×100）
            IdxFCD,             //ＦＣＤ（×10）
            IdxFID,             //I.I.（×10）
            IdxXrayRotPos,      //Ｘ線管回転位置（×10000）
            IdxXrayFCD,         //従来のＸ線管Ｙ軸（×100）
            IdxXrayXPos,        //従来のＸ線管Ｘ軸（×100）
            IdxIINo,            //I.I.番号
            IdxValueUpper,      //ダミー
        }

        private int[] mySeqValue = new int[(int)SeqIdxValueConstants.IdxValueUpper + 1];                //追加 by 間々田 2009/08/04

        Color tmpFCDTextBoxColor;           //'v17.20 追加 by 長野　2010/09/10
        Color tmpYTextBoxColor;             //'v17.20 追加 by 長野　2010/09/10
        Color tmpIITextBoxColor;            //'v17.20 追加 by 長野　2010/09/10
        Color tmpIIChgTextBoxColor;         //'v17.20 追加 by 長野　2010/09/10


        private const int MINLONG = unchecked((int)0x80000000);

        //
        // cwbtnRotate_ValueChanged イベントで使用する static フィールド
        //
        //２重起動をさせないためのフラグ
        private static bool cwbtnRotate_ValueChanged_IsBusy = false;

        //
        // cwbtnUpDown_ValueChanged イベントで使用する static フィールド
        //
        //２重起動をさせないためのフラグ
        private static bool cwbtnUpDown_ValueChanged_IsBusy = false;

        //
        // cwbtnFineTable_ValueChanged イベントで使用する static フィールド
        //
        //２重起動をさせないためのフラグ
        private static bool cwbtnFineTable_ValueChanged_IsBusy = false;

        //Rev22.00 追加 by長野 2015/08/20
        //
        // cwbtnTiltAndRot_Rot_ValueChanged イベントで使用する static フィールド
        //
        //２重起動をさせないためのフラグ
        private static bool cwbtnTiltAndRot_Rot_ValueChanged_IsBusy;

        //Rev22.00 追加 by長野 2015/08/20
        //
        // cwbtnTiltAndRot_Tilt_ValueChanged イベントで使用する static フィールド
        //
        //２重起動をさせないためのフラグ
        private static bool cwbtnTiltAndRot_Tilt_ValueChanged_IsBusy;

        //
        // tmrMecainf_Timer イベントで使用する static フィールド
        //
        //ｲﾍﾞﾝﾄﾌﾟﾛｼｰｼﾞｬの2重呼び出し防止
        private static bool tmrMecainf_Timer_BUSYNOW = false;    //状態(True:実行中,False:停止中)
        
        //
        // UC_SeqComm_OnCommEnd イベントで使用する static フィールド
        //
        //ｲﾍﾞﾝﾄﾌﾟﾛｼｰｼﾞｬの2重呼び出し防止
        private static bool UC_SeqComm_OnCommEnd_BUSYNOW = false;    //状態(True:実行中,False:停止中)

        //
        // tmrSeqComm_Timer イベントで使用する static フィールド
        //
        private static bool tmrSeqComm_Timer_CommCheckByTimer = false;

        //Rev26.40 by chouno 2019/02/12 private->internal 
		//private CTCheckBox[] ctchkIIMove = null;
		//private Label[] lblIIMove = null;
		//private Button[] cmdCollimator = null;
		//private Button[] cmdIris = null;
        internal CTCheckBox[] ctchkIIMove = null;
        internal Label[] lblIIMove = null;
        internal Button[] cmdCollimator = null;
        internal Button[] cmdIris = null;

        public CWButton[] cwbtnChangeMode = null;
        public CWButton[] cwbtnChangeDet = null;
        public CWButton[] cwbtnChangeXray = null; //Rev23.10 追加 by長野 2015/09/18
        public ComboBox[] cboSpeed = null;

		//private CWButton[] cwbtnRotate = null;
		//private CWButton[] cwbtnUpDown = null;
		//private CWButton[] cwbtnRotateXray = null; 
		//private CWButton[] cwbtnMove = null;
		//private CWButton[] cwbtnFineTable = null;
        //Rev26.40 by chouno 2019/02/12 private->internal
        internal CWButton[] cwbtnRotate = null;
        internal CWButton[] cwbtnUpDown = null;
        internal CWButton[] cwbtnRotateXray = null;
        internal CWButton[] cwbtnMove = null;
        internal CWButton[] cwbtnFineTable = null;

		private Label[] lblMechaStatus = null;
		internal CTCheckBox[] ctchkRotate = null;

        private CWButton[] cwbtnTiltAndRot_Rot = null;  //Rev22.00 追加 回転傾斜テーブル用 by長野 2015/08/20
        private CWButton[] cwbtnTiltAndRot_Tilt = null; //Rev22.00 追加 回転傾斜テーブル用 by長野 2015/08/20

        //変更2015/02/12hata_スライダー新規          
        //private CTSliderTrack[] cwsldUpDown = null;
        private CTSliderVScroll[] cwsldUpDown = null;

		private static frmMechaControl _Instance = null;

        //追加2014/12/22hata_ｽﾗｲﾀﾞを1つにする
        private decimal stsUpDownPos = 0;     //UpDownｽﾗｲﾀﾞｰの現在位置ステータス
        private char presskye = (char)0;  //Keypressの値          
        private bool mousecapture = false;
        private string PreUpDownValText = "";   //UpDownTextを変更した前回の値
        //追加2015/02/10hata
        private static DateTime? SeqComm_Error_Time;	    // エラーを受け取った時間取得用
        private const int SeqComm_Error_WaitTime = 5;	// エラー表示の待ち時間(秒)
        private int SeqComm_Error_BeforNo = -1;	            // 前回受け取ったエラーNo

        //Rev20.00 フラグ追加 タッチパネルからのdll2重呼び出しを防止 by長野 2015/04/09
        private bool RotMoveByPanel = false;
        private bool fxTableMoveByPanel = false;
        private bool fyTableMoveByPanel = false;
        private bool UdMoveByPanel = false;
        private bool TiltMoveByPanel = false;    //Rev22.00 回転傾斜テーブル 傾斜 by長野 2015/08/20
        private bool TiltRotMoveByPanel = false; //Rev22.00 回転傾斜テーブル 回転 by長野 2015/08/20

        //Rev23.10 他クラスから参照・使用するための昇降位置 by長野 2015/10/16
        private float myUdab_pos = 0.0f;
        public float Udab_Pos
        {
            get
            {
                return myUdab_pos;
            }
            set
            {
                myUdab_pos = value;
            }
        }

        //Rev26.00 現在の機構部位置から推測したスキャンエリア by chouno 2017/01/18
        private float myGuessScanAreaByMechaPos = 0.0f;
        public float guessScanAreaByMechaPos
        {
            get
            {
                UpdateScanarea();
                return myGuessScanAreaByMechaPos;
            }
            set
            {
                myGuessScanAreaByMechaPos = value;
            }
        }

        //Rev26.00 add by chouno 2017/03/02
        private float m_flblScanArea = 0.0f;
        public float flblScanArea
        {
            get
            {
                return m_flblScanArea;
            }
            set
            {
                m_flblScanArea = value;
            }
        }
        private float m_flblPixSize = 0.0f;
        public float flblPixSize
        {
            get
            {
                return m_flblPixSize;
            }
            set
            {
                m_flblPixSize = value;
            }
        }

        /// <summary>
		/// 
		/// </summary>
		public frmMechaControl()
		{
			InitializeComponent();

			// VB6 でのコントロール配列の代替え
			//ctchkIIMove = new CTCheckBox[] {ctchkIIMove0, ctchkIIMove1, ctchkIIMove2, ctchkIIMove3};
            ctchkIIMove = new CTCheckBox[] { ctchkIIMove0, ctchkIIMove1, ctchkIIMove2, ctchkIIMove3, ctchkIIMove4 }; //Rev26.00 add by chouno 2017/02/13
            //lblIIMove = new Label[] {lblIIMove0, lblIIMove1, lblIIMove2, lblIIMove3};
            lblIIMove = new Label[] { lblIIMove0, lblIIMove1, lblIIMove2, lblIIMove3, lblIIMove4 };
            cmdCollimator = new Button[] {cmdCollimator0, cmdCollimator1, cmdCollimator2, cmdCollimator3, cmdCollimator4, cmdCollimator5, cmdCollimator6, cmdCollimator7};
			cmdIris = new Button[] {cmdIris0, cmdIris1, cmdIris2, cmdIris3, cmdIris4, cmdIris5, cmdIris6, cmdIris7};
            cwbtnChangeMode = new CWButton[] { cwbtnChangeMode0, cwbtnChangeMode1};
            cwbtnChangeDet = new CWButton[] { cwbtnChangeDet0, cwbtnChangeDet1 };
            cwbtnChangeXray = new CWButton[] { cwbtnChangeXray0, cwbtnChangeXray1 }; //Rev23.10 追加 by長野 2015/09/18
            cboSpeed = new ComboBox[] { cboSpeed0, cboSpeed1, cboSpeed2, cboSpeed3, cboSpeed4, null, cboSpeed6, cboSpeed7, cboSpeed8, cboSpeed9, cboSpeed10, cboSpeed11 };
			cwbtnRotate = new CWButton[] {cwbtnRotate0, cwbtnRotate1};
			cwbtnUpDown = new CWButton[] {cwbtnUpDown0, cwbtnUpDown1};
			cwbtnRotateXray = new CWButton[] {cwbtnRotateXray0, cwbtnRotateXray1}; 
			cwbtnMove = new CWButton[] {cwbtnMove0, cwbtnMove1, cwbtnMove2, cwbtnMove3, null, null, cwbtnMove6, cwbtnMove7, cwbtnMove8, cwbtnMove9};
			cwbtnFineTable = new CWButton[] {cwbtnFineTable0, cwbtnFineTable1};
			//lblMechaStatus = new Label[] {lblMechaStatus0, lblMechaStatus1, lblMechaStatus2, lblMechaStatus3, lblMechaStatus4, lblMechaStatus5};
            lblMechaStatus = new Label[] { lblMechaStatus0, lblMechaStatus1, lblMechaStatus2, lblMechaStatus3, lblMechaStatus4, lblMechaStatus5, lblMechaStatus6, lblMechaStatus7};
            ctchkRotate = new CTCheckBox[] {ctchkRotate0, ctchkRotate1, ctchkRotate2, ctchkRotate3};
            cwbtnTiltAndRot_Rot = new CWButton[] { cwbtnTiltAndRot_Rot0, cwbtnTiltAndRot_Rot1 };    //Rev22.00 回転傾斜テーブル 追加 by長野 2015/08/20
            cwbtnTiltAndRot_Tilt = new CWButton[] { cwbtnTiltAndRot_Tilt0, cwbtnTiltAndRot_Tilt1 }; //Rev22.00 回転傾斜テーブル 追加 by長野 2015/08/20

            //変更2015/02/12hata_スライダー新規
            //変更2014/12/22hata_ｽﾗｲﾀﾞを1つにする
            ////cwsldUpDown = new CTSliderTrack[] { cwsldUpDown0, cwsldUpDown1 };
            //cwsldUpDown = new CTSliderTrack[] { cwsldUpDown0, null };
            cwsldUpDown = new CTSliderVScroll[] { ctSliderVScroll1, null };

            //Rev23.10 追加 by長野 2015/09/18
            //シーケンサから送られてきた各軸の値をmmに変換するための係数を、コモンに従い設定する
            if (CTSettings.scaninh.Data.cm_mode == 1)
            {
                modMechaControl.GVal_FCD_SeqMagnify = 10;
                modMechaControl.GVal_FDD_SeqMagnify = 10;
                modMechaControl.GVal_TableX_SeqMagnify = 100;
                modMechaControl.GVal_Rot_SeqMagnify = 100;
                modMechaControl.GVal_Ud_SeqMagnify = 100;
            }
            else if (CTSettings.scaninh.Data.cm_mode == 0)
            {
                modMechaControl.GVal_FCD_SeqMagnify = 1000;
                modMechaControl.GVal_FDD_SeqMagnify = 1000;
                modMechaControl.GVal_TableX_SeqMagnify = 1000;
                modMechaControl.GVal_Rot_SeqMagnify = 1000;
                modMechaControl.GVal_Ud_SeqMagnify = 1000;
            }
            else
            {
                modMechaControl.GVal_FCD_SeqMagnify = 10;
                modMechaControl.GVal_FDD_SeqMagnify = 10;
                modMechaControl.GVal_TableX_SeqMagnify = 100;
                modMechaControl.GVal_Rot_SeqMagnify = 100;
                modMechaControl.GVal_Ud_SeqMagnify = 100;
            }

            // イベント定義
            InitializeEventHandler();

        }

		/// <summary>
		/// 
		/// </summary>
		public static frmMechaControl Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmMechaControl();
				}

				return _Instance;
			}
		}


        // イベント定義
        public void InitializeEventHandler()
        {

            //昇降スライダーのイベント
            cwsldUpDown[0].MouseCaptureChanged += new System.EventHandler(cwsldUpDown_MouseCaptureChanged);
            //cwsldUpDown[0].Scroll += new System.EventHandler(cwsldUpDown_Scroll);
            cwsldUpDown[0].ValueChanged += new System.EventHandler(cwsldUpDown_ValueChanged);

            //削除2014/12/22hata_ｽﾗｲﾀﾞを1つにする
            //cwsldUpDown[1].MouseCaptureChanged += new System.EventHandler(cwsldUpDown_MouseCaptureChanged);
            ////cwsldUpDown[1].Scroll += new System.EventHandler(cwsldUpDown_Scroll);
            //cwsldUpDown[1].ValueChanged += new System.EventHandler(cwsldUpDown_ValueChanged);


        }


        //*************************************************************************************************
        //機　　能： FCD（含オフセット）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        public float FCDWithOffset
        {
            get{ return myFCDWithOffset; }
        }
        //*************************************************************************************************
        //機　　能： FID（含オフセット）メカ値
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        public float FIDWithOffset
        {
            get{ return myFIDWithOffset; }
        }

        //*************************************************************************************************
        //機　　能： FCD（含オフセット）メカ値
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.10  15/10/28  (検S1)長野    新規作成
        //*************************************************************************************************
        public float FCDWithOffsetMecha
        {
            get { return myFCDWithOffsetMecha; }
        }

        //*************************************************************************************************
        //機　　能： FID（含オフセット）メカ値
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.10  15/10/28  (検S1)長野    新規作成
        //*************************************************************************************************
        public float FIDWithOffsetMecha
        {
            get { return myFIDWithOffsetMecha; }
        }
        //*************************************************************************************************
        //機　　能： FCD（含オフセット）リニアスケール値
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.10  15/10/28  (検S1)長野    新規作成
        //*************************************************************************************************
        public float FCDWithOffsetLinear
        {
            get { return myFCDWithOffsetLinear; }
        }

        //*************************************************************************************************
        //機　　能： FID（含オフセット）リニアスケール値
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.10  15/10/28  (検S1)長野    新規作成
        //*************************************************************************************************
        public float FIDWithOffsetLinear
        {
            get { return myFIDWithOffsetLinear; }
        }


        //*************************************************************************************************
        //機　　能： FCD/FID比
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        public float FCDFIDRate
        {
            get{ return myFCDWithOffset / myFIDWithOffset; }
        }

        //*************************************************************************************************
        //機　　能： Y軸座標
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        public float TableYPos
        {
            get{ return (float)ntbTableXPos.Value; }
        }


        //*************************************************************************************************
        //機　　能： 自動スキャン位置指定内「断面」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void cmdFromSlice_Click(object sender, EventArgs e)
        {
            string FileName = null;     //v15.10追加 byやまおか 2009/11/30

            //追加2014/10/07hata_v19.51反映
            //機構部が動作可能かチェック
            if (!modMechaControl.IsOkMechaMove())   //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                return;

            if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
            {
                return;
            }

            //透視ボタンクリック時に必要な校正が完了しているかチェック
            //幾何歪校正ステータスが準備完了でない場合
            //If Not frmScanControl.IsOkVertical() Then
            if ((!frmScanControl.Instance.IsOkVertical) && (!CTSettings.detectorParam.Use_FlatPanel))    //v17.00変更 byやまおか 2010/02/24
            {
                //メッセージ表示：
                //   幾何歪校正が準備完了でないため、処理を中止します。
                //   事前に幾何歪校正を実施してください。
                MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_CorNotReady, StringTable.IDS_CorDistortion), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Rev23.10 X線切替有の場合は念のためテーブルを再読込 by長野 2015/11/24
            if (CTSettings.scaninh.Data.multi_tube == 0)
            {
                if (CTSettings.scansel.Data.multi_tube == 0 || CTSettings.scansel.Data.multi_tube == 1)
                {
                    ComLib.change_xray_com(CTSettings.scansel.Data.multi_tube);
                }
            }

            //画像が表示されていない場合
            if (string.IsNullOrEmpty(frmScanImage.Instance.Target))
            {
                ////実行するには、画像を表示してください      'v15.10削除 byやまおか 2009/11/30
                //MsgBox LoadResString(12839), vbExclamation
                //Exit Sub

                //画像ファイル選択ダイアログ表示     'v15.10追加 byやまおか 2009/11/30
                FileName = modFileIO.GetFileName(StringTable.IDS_Open, CTResources.LoadResString(StringTable.IDS_CTImage), ".img");

                if (!string.IsNullOrEmpty(FileName))
                {
                    frmScanImage.Instance.Target = FileName;
                }
                else
                {
                    return;
                }
            }

            //'自動スキャン位置指定用のROI制御スタート
            //frmScanImage.ImageProc = RoiAutoPos

            //Rev23.30 CT画像上での位置指定モードでfrmAutoScanPosを表示させる by長野 2016/02/06
            frmAutoScanPos.Instance.AutoScanPosMode = 0;

            //自動スキャン位置指定フォーム表示           '別途フォームを用意した 2009/05/15
            if (!modLibrary.IsExistForm("frmAutoScanPos"))	//追加2015/01/30hata_if文追加
            {
                frmAutoScanPos.Instance.Show(frmCTMenu.Instance);
            }
            else
            {
                frmAutoScanPos.Instance.WindowState = FormWindowState.Normal;
                frmAutoScanPos.Instance.Visible = true;
            }

            //ボタンを緑色にする
            cmdFromSlice.BackColor = Color.Lime;
        }


        //*************************************************************************************************
        //機　　能： 自動スキャン位置指定内「透視」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void cmdFromTrans_Click(object sender, EventArgs e)
        {
            //追加2014/10/07hata_v19.51反映
            //機構部が動作可能かチェック
            if (!modMechaControl.IsOkMechaMove())   //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                return;

            //Rev26.00 add by chouno 2017/03/13
            if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
            {
                return;
            }

            //RAMディスクが構築されているかどうか  'v17.40追加 byやまおか 2010/10/26
            //If UseRamDisk And (Not RamDiskIsReady) Then Exit Sub
            #region     //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //    If UseRamDisk Then      'v17.42修正 byやまおか 2010/11/04
            //        If (Not RamDiskIsReady) Then Exit Sub
            //    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            #endregion

			frmScanControl frmScanControl = frmScanControl.Instance;

            //透視ボタンクリック時に必要な校正が完了しているかチェック 09-06-10 by YAMAKAGE
            //幾何歪校正ステータスが準備完了でない場合
            //If Not frmScanControl.IsOkVertical() Then
            if (!frmScanControl.IsOkVertical && !CTSettings.detectorParam.Use_FlatPanel)
            {
                MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_CorNotReady, StringTable.IDS_CorDistortion),
                                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //スキャン位置校正ステータスをチェック
            if (!frmScanControl.IsOkScanPosition)
            {
                //メッセージ表示：
                //   スキャン位置校正が準備完了でないため、処理を中止します。
                //   事前にスキャン位置校正を実施してください。
                MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_CorNotReady, StringTable.IDS_CorScanPos),
                                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //オフセット校正ステータスをチェック
            if (!frmScanControl.IsOkOffset)
            {
                //メッセージ表示：
                //   オフセット校正が準備完了でないため、処理を中止します。
                //   事前にオフセット校正を実施してください。
                MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_CorNotReady, StringTable.IDS_CorOffset),
                                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Rev23.10 X線切替有の場合は念のためテーブルを再読込 by長野 2015/11/24
            if (CTSettings.scaninh.Data.multi_tube == 0)
            {
                if (CTSettings.scansel.Data.multi_tube == 0 || CTSettings.scansel.Data.multi_tube == 1)
                {
                    ComLib.change_xray_com(CTSettings.scansel.Data.multi_tube);
                }
            }

#region     //v17.02削除 byやまおか 2010/07/12
            //If (Not frmTransImage.tmrLive.Enabled) And (Use_FlatPanel) Then 'v17.00追加 byやまおか 2010/03/11
            //    'メッセージ表示：
            //    '   ライブ画像処理を開始してから位置指定を行ってください。
            //    MsgBox "ライブ画像処理を開始してから位置指定を行ってください。", vbCritical
            //    Exit Sub
            //End If
#endregion

            //自動スキャン位置指定用のROI制御スタート
            frmTransImage.Instance.TransImageProc = frmTransImage.TransImageProcType.TransRoiAutoPos;

            cmdFromTrans.BackColor = Color.Lime;
        }

        //*************************************************************************************************
        //機　　能： 自動スキャン位置指定内「外観」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.30  16/02/05   (検S1)長野   新規作成
        //*************************************************************************************************
        private void cmdFromExObsCam_Click(object sender, EventArgs e)
        {

            //追加2014/10/07hata_v19.51反映
            //機構部が動作可能かチェック
            if (!modMechaControl.IsOkMechaMove())   //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                return;

            //Rev26.00 add by chouno 2017/03/13
            if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
            {
                return;
            }

            //Rev23.10 X線切替有の場合は念のためテーブルを再読込 by長野 2015/11/24
            if (CTSettings.scaninh.Data.multi_tube == 0)
            {
                if (CTSettings.scansel.Data.multi_tube == 0 || CTSettings.scansel.Data.multi_tube == 1)
                {
                    ComLib.change_xray_com(CTSettings.scansel.Data.multi_tube);
                }
            }

            //Rev26.20 外観カメラホームポジション初期化 by chouno 2019/02/12
            modMechaControl.InitMoveByCam();
            //Rev26.20 現在位置バックアップ by chouno 2019/02/12
            modMechaControl.BackUpCurrentPosition();

            //Rev26.20 外観カメラ固定状態により、処理を分岐
            if(!MoveCamHomePosition(modMechaControl.FixedCamType))
            {
                return;
            }

            //frmStatus.Instance.Check_RotateZero();
            //Rev26.40 change by chouno 2019/02/12
            if (!frmStatus.Instance.Check_RotateZero())
            {
                return;
            }

            //'自動スキャン位置指定用のROI制御スタート
            //frmScanImage.ImageProc = RoiAutoPos

            //Rev25.00 付帯情報表示は消す by長野 2016/08/22
            //del Rev26.40 by chouno 2019/03/12
            //frmImageInfo.Instance.Close();

            //Rev23.30 外観カメラ画像上での位置指定モードでfrmAutoScanPosを表示させる by長野 2016/02/06
            frmAutoScanPos.Instance.AutoScanPosMode = 1;
            
            //Rev23.30 追加 外観カメラ起動 by長野 2016/02/06
            if (frmExObsCam.Instance.CamHandle != IntPtr.Zero)
            {
                frmExObsCam.Instance.CaptureStop();

                Application.DoEvents();

                frmExObsCam.Instance.CloseCamera();

                Application.DoEvents();

                frmExObsCam.Instance.Close();
                frmExObsCam.Instance.Dispose();
            }

            frmExObsCam.Instance.ExObsCam.ExObsCamProcessStart(DispFlg:1);

            //自動スキャン位置指定フォーム表示           '別途フォームを用意した 2009/05/15
            if (!modLibrary.IsExistForm("frmAutoScanPos"))	//追加2015/01/30hata_if文追加
            {
                frmAutoScanPos.Instance.Show(frmCTMenu.Instance);
            }
            else
            {
                frmAutoScanPos.Instance.WindowState = FormWindowState.Normal;
                frmAutoScanPos.Instance.Visible = true;
            }

            //ボタンを緑色にする
            cmdFromExObsCam.BackColor = Color.Lime;
        }

        //*************************************************************************************************
        //機　　能： 外観カメラ位置指定機能実行時のホームポジション移動
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V17.20  10/09/06  (検S1) 長野      新規作成
        //*************************************************************************************************
        private bool MoveCamHomePosition(int CamType)
        {
            bool bret = false;

            int YSpeedBack = 0;

            //ＦＣＤ速度のバックアップ
            YSpeedBack = modSeqComm.MySeq.stsYSpeed;

            try
            {
                frmCTMenu.Instance.Enabled = false;
                if (CamType == 0) //試料テーブル一体型
                {
                    //テーブルを下限付近まで下げる
                    //確認メッセージ表示：（コモンによってメッセージを切り替える）
                    //   リソース24013:スキャン位置指定の前に、試料テーブルが下限付近まで下降し、テーブル回転角度が0度になります。
                    //   リソース9905:よろしければＯＫをクリックしてください。
                    DialogResult result = MessageBox.Show(CTResources.LoadResString(24013) + "\r" +
                                                            CTResources.LoadResString(StringTable.IDS_ClickOK),
                                                        Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    if (result == DialogResult.Cancel)
                    {
                        bret = false;

                        return bret;
                    }

                    if (modMechaControl.MechaUdIndex(CTSettings.GValUpperLimit) != 0)
                    {
                        //Rev21.00 追加 by長野 2015/03/08
                        if (CTSettings.t20kinf.Data.ud_type == 0)
                        {
                            //メッセージ表示：テーブル下降に失敗しています。
                            MessageBox.Show(CTResources.LoadResString(9428), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            //メッセージ表示：X線管・検出器昇降に失敗しています。
                            MessageBox.Show(CTResources.LoadResString(22006), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        throw new Exception();
                    }

                    bret = true;
                }
                else //検査室固定
                {
                    //ホームポジションへ移動
                    DialogResult result = MessageBox.Show(CTResources.LoadResString(24016) + "\r" +
                                                            CTResources.LoadResString(StringTable.IDS_ClickOK),
                                                        Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    if (result == DialogResult.Cancel)
                    {
                        bret = false;

                        return bret;
                    }

                    //ＦＣＤ速度を一時的に変更する
                    if (!modSeqComm.SeqWordWrite("YSpeed", (CTSettings.mechapara.Data.fcd_speed[(int)frmMechaControl.SpeedConstants.SpeedFast] * 10).ToString("0"), false))
                    {
                        bret = false;
                        throw new Exception();
                    }
                    Application.DoEvents();

                    //指定FCD位置が現在FDDを超えている場合はFDDをぶつからない位置へ動かす
                    if (modMechaControl.HomeFCD + CTSettings.Gval_BetMaxFcdAndFdd >= ScanCorrect.GVal_Fid)
                    {
                        if (!modSeqComm.MoveFID((int)((modMechaControl.HomeFCD + CTSettings.Gval_BetMaxFcdAndFdd) * modMechaControl.GVal_FDD_SeqMagnify)))
                        {
                            //MsgBox "指定された位置にI.I.を移動できませんでした。", vbExclamation
                            MessageBox.Show(StringTable.GetResString(20035, CTSettings.GStrIIOrFPD), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //v17.60修正 byやまおか 2011/06/07
                            throw new Exception();
                        }
                    }

                    //FCDを移動
                    if (!modSeqComm.MoveFCD((int)(modMechaControl.HomeFCD * modMechaControl.GVal_FCD_SeqMagnify)))
                    {
                        //MsgBox 指定されたFCD位置まで試料テーブルを移動させることができませんでした。
                        MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_MoveErr, StringTable.IDS_FCD, StringTable.IDS_SampleTable), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //v17.60修正 byやまおか 2011/06/07
                        throw new Exception();
                    }

                    //Yを移動
                    if (!modSeqComm.MoveXpos((int)(modMechaControl.HomeTableY * modMechaControl.GVal_TableX_SeqMagnify)))
                    {
                        //MsgBox 指定されたY軸位置まで試料テーブルを移動させることができませんでした。
                        MessageBox.Show(StringTable.GetResString(StringTable.IDS_MoveErr, CTSettings.AxisName[1], CTResources.LoadResString(StringTable.IDS_SampleTable)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //v17.60修正 byやまおか 2011/06/07
                        throw new Exception();
                    }

                    //Zを移動
                    if (modMechaControl.MechaUdIndex(modMechaControl.HomeTableZ) != 0)
                    {
                        //Rev21.00 追加 by長野 2015/03/08
                        if (CTSettings.t20kinf.Data.ud_type == 0)
                        {
                            //メッセージ表示：テーブル下降に失敗しています。
                            MessageBox.Show(CTResources.LoadResString(9428), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            throw new Exception();
                        }
                        else
                        {
                            //メッセージ表示：X線管・検出器昇降に失敗しています。
                            MessageBox.Show(CTResources.LoadResString(22006), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            throw new Exception();
                        }
                    }

                    bret = true;
                }
            }
            catch
            {
                bret = false;
            }
            finally
            {
                frmCTMenu.Instance.Enabled = true;
            }

            //ＦＣＤ速度を元に戻す
            modSeqComm.SeqWordWrite("YSpeed", Convert.ToString(YSpeedBack), false);
            Application.DoEvents();

            //Rev26.40 add by chouno 2019/02/12
            if (bret == false)
            {
                MessageBox.Show(CTResources.LoadResString(24017), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return bret;
        }
        //*************************************************************************************************
        //機　　能： 「メカリセット」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V17.20  10/09/06  (検S1) 長野      新規作成
        //*************************************************************************************************
        private void cmdMechaAllReset_Click(object sender, EventArgs e)
        {
            //すでにメカリセット中であれば無効
            if (modSeqComm.MySeq.stsMechaRstBusy)
            {
                return;
            }

            //削除2014/10/07hata_v19.51反映8'''''ここから'''''
            ////運転準備ボタンが押されていなければ無効
            //if (!modSeqComm.MySeq.stsRunReadySW)
            //{
            //    //MsgBox "運転準備が未完のためメカリセットできません。" & vbCrLf & "運転準備スイッチを押して運転準備完了にしてください。", vbCritical
            //    MessageBox.Show(CTResources.LoadResString(20031) + "\r\n" + CTResources.LoadResString(20032),
            //                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);                 //ストリングテーブル化 'v17.60 by 長野 2011/05/22
            //    return;
            //}

            //frmCTMenu frmCTMenu = frmCTMenu.Instance;

            ////v17.40 メンテナンスのときは検査室扉が閉まっていることをチェックしないように変更 by 長野 2010/10/21
            //if (!modSeqComm.MySeq.stsDoorPermit)
            //{
            //    if (frmCTMenu.DoorStatus == frmCTMenu.DoorStatusConstants.DoorOpened)   //インターロック用
            //    {
            //        //MsgBox "Ｘ線検査室の扉が開いているためメカリセットできません。" & vbCrLf & "X線検査室の扉を閉めてから､再度操作を行なって下さい｡", vbCritical
            //        MessageBox.Show(CTResources.LoadResString(20033) + "\r\n" + CTResources.LoadResString(20034),
            //                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);             // ストリングテーブル化 'v17.60 by 長野 2011/5/22
            //        return;
            //    }
            //}
            //削除2014/10/07hata_v19.51反映'''''ここまで'''''

            //追加2014/10/07hata_v19.51反映
            //機構部が動作可能かチェック（上記を関数化）
            if (!modMechaControl.IsOkMechaMove())   //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                return;


            //CTBusy状態なら無効
            if (modCTBusy.CTBusy != 0) return;

#region     //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //切替中なら無効
            //    If IsSwtichingDet() Then Exit Sub
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

            //X線ON中は不可
            if (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus) return;

#region     //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //他のダイアログが開いているときは無効
            //    If Not IsAllCloseFrm Then Exit Sub
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

            //    'すでにリセットが完了していたら無効
            //    If MySeq.stsMechaRstOK Then
            //        MsgBox ("リセットは完了しています。")
            //    Exit Sub
            //    End If

            if (!modLibrary.IsExistForm("frmMechaAllResetMove"))	//追加2015/01/30hata_if文追加
            {
                frmMechaAllResetMove.Instance.Show(frmCTMenu.Instance);
            }
            else
            {
                frmMechaAllResetMove.Instance.WindowState = FormWindowState.Normal;
                frmMechaAllResetMove.Instance.Visible = true;
            }

        }


        //*************************************************************************************************
        //機　　能： I.I.移動指定（400,600,800,1000mm指定）チェックボックスチェック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2009/07/21 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void ctchkIIMove_CheckedByClick(object sender, EventArgs e)
        {
			if (sender as CTCheckBox == null) return;

			int Index = Array.IndexOf(ctchkIIMove, sender);

			if (Index < 0) return;

            //削除2014/10/07hata_v19.51反映8'''''ここから'''''
            ////v17.20 検査室の扉が閉じていなければ無効 by 長野 2010/09/20
            ////If Not (frmCTMenu.DoorStatus = Doorclosed) Then 'インターロック用
            ////    If Not (frmCTMenu.DoorStatus = DoorLocked) Then '電磁ロック用
            ////
            ////チェックをはずす                               'v15.01追加 by 間々田 2009/09/02
            ////ctchkIIMove(Index).Checked = False
            ////            MsgBox LoadResString(17503), vbCritical

            ////            Exit Sub
            ////    End If
            ////

            //frmCTMenu frmCTMenu = frmCTMenu.Instance;

            ////v17.61 検査室扉が開いている場合の処理を追加 by長野 2011/09/12
            //if (!modSeqComm.MySeq.stsDoorPermit)
            //{
            //    //v17.20 検査室の扉が閉じていなければ無効 by 長野 2010/09/20
            //    if (frmCTMenu.DoorStatus == frmCTMenu.DoorStatusConstants.DoorOpened) //インターロック用
            //    {
            //        //MsgBox "Ｘ線検査室の扉が開いているため検出器を移動することができません。" & vbCrLf & "X線検査室の扉を閉めてから､再度操作を行なって下さい｡", vbCritical
            //        //MsgBox LoadResString(20056) & vbCrLf & LoadResString(8022), vbCritical 'ストリングテーブル化 'v17.60 by長野 2011/05/22
            //        MessageBox.Show(CTResources.LoadResString(20058) + "\r\n" + CTResources.LoadResString(8022),
            //                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

            //        ctchkIIMove[Index].Checked = false;

            //        return;
            //    }
            //}
            //削除2014/10/07hata_v19.51反映'''''ここまで'''''

            //追加2014/10/07hata_v19.51反映
            //機構部が動作可能かチェック（上記を関数化）
            if (!modMechaControl.IsOkMechaMove())   //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            {
                //チェックをはずす                               'v15.01追加 by 間々田 2009/09/02                   
                ctchkIIMove[Index].Checked = false;
                return;
            }

            //Rev26.00 add by chouno 2017/03/13
            if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
            {
                //チェックをはずす                               'v15.01追加 by 間々田 2009/09/02                   
                ctchkIIMove[Index].Checked = false;
                return;
            }

            //メカ準備画面を使用不可にする
            this.Enabled = false;

            //マウスポインタを砂時計にする
            //Me.MousePointer = vbHourglass
            Cursor.Current = Cursors.WaitCursor;    //v16.30変更 byやまおか 2010/05/20

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
            '他のチェックボックスのチェックをオフにする
            Dim i As Integer
            For i = ctchkIIMove.LBound To ctchkIIMove.UBound
                If i <> Index Then
                    ctchkIIMove(i).Checked = False
                End If
            Next
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            //他のチェックボックスのチェックをオフにする
            for (int i=0; i<ctchkIIMove.Length; i++)
            {
                if (i != Index)
                {
                    ctchkIIMove[i].Checked = false;;
                }
            }

            //移動
            //MoveFID Val(lblIIMove(Index).Caption) * 10

            //移動：失敗した場合はメッセージを出す           'v15.01変更 by 間々田 2009/09/02
            int value = 0;
            int.TryParse(lblIIMove[Index].Text, out value);
            //if (!modSeqComm.MoveFID(value * 10))
            if (!modSeqComm.MoveFID(value * modMechaControl.GVal_FDD_SeqMagnify))//Rev23.10 変更 by長野 2015/09/18
            {
                //MsgBox "指定された位置にI.I.を移動できませんでした。", vbExclamation
                //MsgBox LoadResString(20035), vbExclamation 'ストリングテーブル化 '17.60 by長野 2011/05/22
                MessageBox.Show(StringTable.GetResString(20035, CTSettings.GStrIIOrFPD), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //v17.60修正 byやまおか 2011/06/07
            }

            //チェックをはずす                               'v15.01追加 by 間々田 2009/09/02                   
            ctchkIIMove[Index].Checked = false;

            //表示更新
            MyUpdate();

            //マウスポインタを元に戻す
            //Me.MousePointer = vbDefault
            Cursor.Current = Cursors.Default;       //v16.30変更 byやまおか 2010/05/20

            //メカ準備画面を使用可にする
            this.Enabled = true;
        }


        //*************************************************************************************************
        //機　　能： フォームロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        private void frmMechaControl_Load(object sender, EventArgs e)
        {
            //変数初期化
            CommCheckOn = false;

            for (int i = mySeqValue.GetLowerBound(0); i <= mySeqValue.GetUpperBound(0); i++)
            {
                mySeqValue[i] = MINLONG;
            }

            //フォームの表示位置
            this.SetBounds(0, 0, modCT30K.FmControlWidth, MECHA_HEIGHT);

            //v19.50 この処理のログは取らないのでコメントアウトする by長野 2013/11/05
//#if UserLog     //v17.48/v17.53追加 byやまおか 2011/03/24
//            modCT30K.UserLogDel(524288);        //512kB(0.5MB)
//            modCT30K.UserLogOut(";" + DateTime.Now.ToString() + ",");
//#endif

            //キャプションのセット
            SetCaption();

            //v19.50 この処理のログは取らないのでコメントアウトする by長野 2013/11/05
//#if UserLog     //v17.48/v17.53追加 byやまおか 2011/03/24
//            modCT30K.UserLogOut("1,;");
//#endif

            //コントロールの初期設定
            InitControls();

		    //v19.50 この処理のログは取らないのでコメントアウトする by長野 2013/11/05
//#if UserLog     //v17.48/v17.53追加 byやまおか 2011/03/24
//            modCT30K.UserLogOut("2,;");
//#endif

            //追加2014/10/07hata_v19.51反映
            //コントロール配置の調整 v16.01 追加 by 山影 10-02-25
            AdjustLayout();

            //v19.50 この処理のログは取らないのでコメントアウトする by長野 2013/11/05
//#if UserLog     //v17.48/v17.53追加 byやまおか 2011/03/24
//            modCT30K.UserLogOut("3,;");
//#endif

            //英語用レイアウト調整 'v17.60 by長野　2011/06/09
            if (modCT30K.IsEnglish)
            {
                EnglishAdjustLayout();
            }

            //v19.50 この処理のログは取らないのでコメントアウトする by長野 2013/11/05
//#if UserLog     //v17.48/v17.53追加 byやまおか 2011/03/24
//            modCT30K.UserLogOut("4,;");
//#endif
            //InitMechacontrolを先に行わないと、データが初期化される   //2014.04/15(検S1)hata
            //mechacontrol.dll関連の初期化
            InitMechacontrol();

            //v19.50 この処理のログは取らないのでコメントアウトする by長野 2013/11/05
//#if UserLog     //v17.48/v17.53追加 byやまおか 2011/03/24
//            modCT30K.UserLogOut("5,;");
//#endif

            //v17.00下へ移動 byやまおか 2010/02/08
            //'Ｘ線検出器がフラットパネルの場合
            //If Use_FlatPanel Then
            //    Get_Vertical_Parameter_Ex 0
            //    Set_Vertical_Parameter
            //End If

            //シーケンサオブジェクト参照
            UC_SeqComm = modSeqComm.MySeq;
			UC_SeqComm.OnCommEnd += UC_SeqComm_OnCommEnd;

            //シーケンサに必要な情報を送る   'v11.3追加 by 間々田 2006/02/20
            InitSeqComm();

            //v19.50 この処理のログは取らないのでコメントアウトする by長野 2013/11/05
//#if UserLog     //v17.48/v17.53追加 byやまおか 2011/03/24
//            modCT30K.UserLogOut("6,;");
//#endif

            //変更2014/10/07hata_v19.51反映
            //v19.50 タイマーの統合 by長野 2013/12/17
            //tmrSeqComm_Timer(this, EventArgs.Empty);
            //tmrSeqComm.Enabled = true;      //v11.4追加 by 間々田 2006/03/06
            //Rev23.40 by長野 2016/04/05 //Rev23.21 by長野 2016/02/23
            //modMechaControl.Flg_SeqCommUpdate = true;
            tmrMecainfSeqComm.Enabled = true;
            //Rev25.00 Rev23.23の修正反映 by長野 2016/08/08 
            //tmrMecainfSeqComm_Timer(tmrMecainfSeqComm, new System.EventArgs());
            if (modSeqComm.MySeq.StatusRead() == 0)
            {
                //更新処理
                MyUpdate(false);
            }

            //v19.50 この処理のログは取らないのでコメントアウトする by長野 2013/11/05
//#if UserLog     //v17.48/v17.53追加 byやまおか 2011/03/24
//            modCT30K.UserLogOut("7,;");
//#endif
            
            //タイマー起動
            ////tmrMecainf_Timer
            //変更2014/10/07hata_v19.51反映
            //tmrMecainf.Enabled = true;
            //v19.50 タイマーの統合 by長野 2013/12/17
            //Rev23.40 by長野 2016/04/05 //Rev23.21 by長野 2016/02/23
            //modMechaControl.Flg_MechaControlUpdate = true;

            //追加2014/10/07hata_v19.51反映
            //検出器シフトありの場合は   'v18.00追加 byやまおか 2011/08/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //if ((CTSettings.DetShiftOn))
            if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
            {
                //表示を更新する
                modDetShift.DetShift = modDetShift.IsDetShiftPos;
            }

            //v17.20 検出器切替改造のため関数化　by 長野 2010/09/09
            //変更2014/10/07hata_v19.51反映
            //FPD_DistorsionCorrect();
            ScanCorrect.FPD_DistorsionCorrect();
            
            //    'Ｘ線検出器がフラットパネルの場合   'v17.00追加(ここから) byやまおか 2010/02/08
            //    If Use_FlatPanel Then

            //        Get_Vertical_Parameter_Ex 0
            //        Set_Vertical_Parameter
            //
            //        If DetType = DetTypePke Then
            //            '２次元幾何歪補正   '追加 2009-10-01 山本
            //            ReDim POSITION_IMAGE(h_size * v_size - 1)
            //            Call DistortionCorrect(POSITION_IMAGE)
            //        End If
            //
            //    End If                              'v17.00追加(ここまで) byやまおか 2010/02/08

        }


        //削除2014/10/07hata_v19.51反映
        //v18.00削除 ScanCorrect.basへ移動 byやまおか 2011/07/09
        ////*************************************************************************************************
        ////機　　能： FPDの場合，フォームロード時にパラメータ計算のため２次元幾何歪補正を行う
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： v17.20  2010/09/08 (検S1)長野  リニューアル
        ////*************************************************************************************************
        //public void FPD_DistorsionCorrect()
        //{
        //    if (CTSettings.detectorParam.Use_FlatPanel)
        //    {
        //        ScanCorrect.Get_Vertical_Parameter_Ex(0);
        //        ScanCorrect.Set_Vertical_Parameter();

        //        if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
        //        {
        //            //２次元幾何歪補正   '追加 2009-10-01 山本
        //            ScanCorrect.POSITION_IMAGE = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
        //            modScanCorrectNew.DistortionCorrect(ref ScanCorrect.POSITION_IMAGE);
        //        }
        //    }       //v17.00追加(ここまで) byやまおか 2010/02/08
        //}


        //*************************************************************************************************
        //機　　能： フォームアンロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            int error_sts = 0;

            //Timerは停止させる   //追加2014/04/11(検S1)hata
            tmrPIOCheck.Enabled = false;
            tmrMecainf.Enabled = false;
            tmrSeqComm.Enabled = false;
            tmrTryReloadForms.Enabled = false;

            Application.DoEvents();

            //高速度透撮影終了時処理 v16.01 追加 by 山影 10-02-12
            //v17.20 条件式追加 by 長野 2010/09/06
            if (CTSettings.HscOn)
            {
                modHighSpeedCamera.HSC_ShutdownProcess();
            }

            //制御スイッチクローズ
            error_sts = modMechaControl.SwOpeEnd();
    
            //監視時間の設定
            error_sts = modMechaControl.PioChkEnd();
    
            //メカクローズ
            modMechaControl.MechaClose();


            //シーケンサ イベントを解放
            if (UC_SeqComm != null)
            {
                 UC_SeqComm.OnCommEnd -= UC_SeqComm_OnCommEnd;
            }
            
            //シーケンサオブジェクト参照を破棄
            //UC_SeqComm.Dispose();

            //if (UC_SeqComm != null)
            //{
            //    UC_SeqComm.OnCommEnd -= UC_SeqComm_OnCommEnd;
            //    UC_SeqComm = null;
            //}

            UC_SeqComm = null;
        }

   
        //*************************************************************************************************
        //機　　能： 各コントロールのキャプションに文字列をセットする
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        public void SetCaption()
        {
            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする    '追加 by 間々田 2009/08/05
            StringTable.LoadResStrings(this);

            //各軸の位置表示用コントロール
            //ntbRotate.Caption = StringTable.GetResString(StringTable.IDS_Rotate, "");              //回転
            //英語版対応 'v17.60 by長野 2011/05/24
            if (modCT30K.IsEnglish == true)
            {
                ntbRotate.Caption = CTResources.LoadResString(20161);                
            }
            else
            {
                // Mod Start 2018/08/31 M.Oyama
                //ntbRotate.Caption = StringTable.GetResString(StringTable.IDS_Rotate, "");          //回転
                if (Thread.CurrentThread.CurrentCulture.Name.StartsWith("zh-CN") == true)
                {
                    ntbRotate.Caption = CTResources.LoadResString(20161);
                }
                else
                {
                    ntbRotate.Caption = StringTable.GetResString(StringTable.IDS_Rotate, ""); 
                }
                // Mod End
               
            }
            ntbRotate.Unit = CTResources.LoadResString(10816);                                    //度
            ntbUpDown.Caption = StringTable.GetResString(StringTable.IDS_Height, "");              //高さ
            //v17.20 検出器のかかわらずFDDで統一する by 長野 2010/09/20
            //ntbFID.Caption = IIf(scancondpar.detector = 0, "FID", "FDD")       'FID/FDD
            ntbFID.Caption = "FDD";
            ntbTableXPos.Caption = CTSettings.AxisName[1];                                          //Y軸
            ntbFTablePosX.Caption = StringTable.GetResString(12131, CTSettings.AxisName[0]);        //微調X軸
            ntbFTablePosY.Caption = StringTable.GetResString(12131, CTSettings.AxisName[1]);        //微調Y軸
            ntbXrayRotPos.Caption = StringTable.BuildResStr(StringTable.IDS_Rotate, StringTable.IDS_XrayTube);     //Ｘ線管回転
            ntbXrayRotPos.Unit = CTResources.LoadResString(10816);                                //度
            ntbXrayPosX.Caption = CTResources.LoadResString(StringTable.IDS_XrayTube) + CTSettings.AxisName[0];       //Ｘ線管X軸
            ntbXrayPosY.Caption = CTResources.LoadResString(StringTable.IDS_XrayTube) + CTSettings.AxisName[1];       //Ｘ線管Y軸

            ntbTilt.Caption = CTResources.LoadResString(22025);           //Rev22.00 追加 by長野 2015/08/20
            ntbTiltRot.Caption = CTResources.LoadResString(22019);        //Rev22.00 追加 by長野 2015/08/20

            if (CTSettings.detectorParam.Use_FlatPanel) lblXrayII.Text = CTResources.LoadResString(12343);       //X線I.I.→X線FPD    'v17.00追加 byやまおか 2010/01/19

            //コリメータフレーム
            //v16.01 変更 by 山影 10-02-17
            //    cmdCollimator(4).Caption = LoadResString(12134)         '開
            //    cmdCollimator(5).Caption = LoadResString(12135)         '閉
            //    cmdCollimator(0).Caption = LoadResString(12134)         '開
            //    cmdCollimator(1).Caption = LoadResString(12135)         '閉
            //    cmdCollimator(3).Caption = LoadResString(12135)         '閉
            //    cmdCollimator(2).Caption = LoadResString(12134)         '開
            //    cmdCollimator(7).Caption = LoadResString(12135)         '閉
            //    cmdCollimator(6).Caption = LoadResString(12134)         '開
            cmdCollimator[4].Text = CTResources.LoadResString(StringTable.IDS_OpenOnly);  //開
            cmdCollimator[5].Text = CTResources.LoadResString(StringTable.IDS_CloseOnly); //閉
            cmdCollimator[0].Text = CTResources.LoadResString(StringTable.IDS_OpenOnly);  //開
            cmdCollimator[1].Text = CTResources.LoadResString(StringTable.IDS_CloseOnly); //閉
            cmdCollimator[3].Text = CTResources.LoadResString(StringTable.IDS_CloseOnly); //閉
            cmdCollimator[2].Text = CTResources.LoadResString(StringTable.IDS_OpenOnly);  //開
            cmdCollimator[7].Text = CTResources.LoadResString(StringTable.IDS_CloseOnly); //閉
            cmdCollimator[6].Text = CTResources.LoadResString(StringTable.IDS_OpenOnly);  //開

            //v16.01 追加 by 山影 10-02-03
            //Ｘ線I.I.絞りフレーム
            fraIris.Text = CTResources.LoadResString(StringTable.IDS_Iris);               //X線I.I.絞り
            cmdIris[4].Text = CTResources.LoadResString(StringTable.IDS_OpenOnly);        //開
            cmdIris[5].Text = CTResources.LoadResString(StringTable.IDS_CloseOnly);       //閉
            cmdIris[0].Text = CTResources.LoadResString(StringTable.IDS_OpenOnly);        //開
            cmdIris[1].Text = CTResources.LoadResString(StringTable.IDS_CloseOnly);       //閉
            cmdIris[3].Text = CTResources.LoadResString(StringTable.IDS_CloseOnly);       //閉
            cmdIris[2].Text = CTResources.LoadResString(StringTable.IDS_OpenOnly);        //開
            cmdIris[7].Text = CTResources.LoadResString(StringTable.IDS_CloseOnly);       //閉
            cmdIris[6].Text = CTResources.LoadResString(StringTable.IDS_OpenOnly);        //開

            //CT/高速切り替えボタン
            for (int i = cwbtnChangeMode.GetLowerBound(0); i <= cwbtnChangeMode.GetUpperBound(0); i++)
            {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				With cwbtnChangeMode(i)
					.OnText = LoadResString(Choose(i + 1, IDS_CTmode, IDS_HSCmode))
					.OffText = .OnText
				End With
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                //cwbtnChangeMode[i].Text = CTResources.LoadResString(i + 1 == 1 ? StringTable.IDS_CTmode : (i + 1 == 2 ? StringTable.IDS_HSCmode : 0));
                cwbtnChangeMode[i].Caption = CTResources.LoadResString(i + 1 == 1 ? StringTable.IDS_CTmode : (i + 1 == 2 ? StringTable.IDS_HSCmode : 0)); //Rev23.10 変更 by長野 2015/09/18
            }

            //v17.20 検出器切替ボタンのキャプションを設定 by 長野 2010-08-31
            for (int j = cwbtnChangeDet.GetLowerBound(0); j <= cwbtnChangeDet.GetUpperBound(0); j++)
            {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				With cwbtnChangeDet(j)
					.OnText = infdef.detector_name(j)
					.OffText = .OnText
				End With
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
                //cwbtnChangeDet[j].Text = CTSettings.infdef.Data.detector_name[j].GetString();
                cwbtnChangeDet[j].Caption = CTSettings.infdef.Data.detector_name[j].GetString(); //Rev23.10 変更 by長野 2015/09/18
            }

            //Rev23.10 X線切替ボタン by長野 2015/09/18
            for (int k = cwbtnChangeXray.GetLowerBound(0); k <= cwbtnChangeXray.GetUpperBound(0); k++)
            {
                cwbtnChangeXray[k].Caption = CTSettings.infdef.Data.multi_tube[k].GetString();
            }

            //追加2014/10/07hata_v19.51反映
            //検出器シフトボタンのキャプションを設定 'v18.00追加 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //cwbtnDetShift.OnText = Project1.My.Resources.ResourceManager.GetString("str" + IDS_Shift);
            //cwbtnDetShift.OffText = Project1.My.Resources.ResourceManager.GetString("str" + IDS_Shift);
            //cwbtnDetShift.Text = CTResources.LoadResString(StringTable.IDS_Shift);
            //cwbtnDetShift.Caption = CTResources.LoadResString(StringTable.IDS_Shift); //Rev23.10 変更 by長野 2015/09/20
            //Rev25.00 文字列ではなく画像にする by長野 2016/08/08
            cwbtnDetShift.Caption = "←　　→";

            //追加2014/10/07hata_v19.51反映
            //スキャンエリアフレームの設定   'v18.00追加 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            fraScanArea.Text = CTResources.LoadResString(StringTable.IDS_ScanArea);
            ntbFullHalf.Caption = CTResources.LoadResString(StringTable.IDS_Half) + ", " + CTResources.LoadResString(StringTable.IDS_Full);
            ntbOffset.Caption = CTResources.LoadResString(StringTable.IDS_Offset);
            //Rev26.10 シフトスキャンの名称がWスキャンの場合は表示を切り替え by chouno 2018/01/16
            if (CTSettings.infdef.Data.scan_mode[3].GetString() == CTResources.LoadResString(25009))
            {
                ntbShift.Caption = CTResources.LoadResString(25001);
            }
            else
            {
                ntbShift.Caption = CTResources.LoadResString(StringTable.IDS_Shift);
            }
            //Rev23.30 メカ制御画面に表示するスキャン条件追加 by長野 2016/02/05
            lblScanMode.Text = CTResources.LoadResString(12817);
            lblScanArea.Text = CTResources.LoadResString(12029);
            lblPixSize.Text = CTResources.LoadResString(StringTable.IDS_PixelSize).Substring(0, CTResources.LoadResString(StringTable.IDS_PixelSize).LastIndexOf("("));//(mm)を除く

            //cboSpeed(MechaTableRotate).ToolTipText = GetResString(IDS_SpeedOf, BuildResStr(IDS_Rotate, IDS_Table))         'テーブル回転速度
            //cboSpeed(MechaTableUpDown).ToolTipText = GetResString(IDS_SpeedOf, BuildResStr(IDS_UpDown, IDS_Table))         'テーブル昇降速度
            //cboSpeed(MechaTableX).ToolTipText = GetResString(IDS_MoveSpeed, GetResString(IDS_TableAxis, AxisName(0))) 'テーブルX軸移動速度
            //cboSpeed(MechaII).ToolTipText = GetResString(IDS_SpeedOf, GetResString(IDS_Move, "I.I."))             'I.I.移動速度
            //cboSpeed(MechaTableY).ToolTipText = GetResString(IDS_MoveSpeed, GetResString(IDS_TableAxis, AxisName(1))) 'テーブルY軸移動速度
            //cboSpeed(MechaFTableX).ToolTipText = GetResString(IDS_SpeedOf, BuildResStr(IDS_Move, IDS_FTable))          '微調テーブル移動速度
            //変更2014/11/26hata_v19.51dnet
            //toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaXrayRotate], StringTable.GetResString(StringTable.IDS_SpeedOf, ntbXrayRotPos.Text));       //Ｘ線管回転速度
            //toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaXrayX], StringTable.GetResString(StringTable.IDS_MoveSpeed, ntbXrayPosX.Text));            //Ｘ線管X軸移動速度
            //toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaXrayY], StringTable.GetResString(StringTable.IDS_MoveSpeed, ntbXrayPosY.Text));            //Ｘ線管Y軸移動速度
            toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaXrayRotate], StringTable.GetResString(StringTable.IDS_SpeedOf, ntbXrayRotPos.Caption));       //Ｘ線管回転速度
            toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaXrayX], StringTable.GetResString(StringTable.IDS_MoveSpeed, ntbXrayPosX.Caption));            //Ｘ線管X軸移動速度
            toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaXrayY], StringTable.GetResString(StringTable.IDS_MoveSpeed, ntbXrayPosY.Caption));            //Ｘ線管Y軸移動速度

            //ToolTipTextを画面表示と合わせた    //v15.10変更 byやまおか 2009/11/09
            //変更2014/11/26hata_v19.51dnet
            //toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaTableRotate], StringTable.GetResString(StringTable.IDS_SpeedOf, ntbRotate.Text + " "));    //回転速度(テーブル)
            //toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaTableUpDown], StringTable.GetResString(StringTable.IDS_MoveSpeed, ntbUpDown.Text + " "));  //高さ移動速度(テーブル)
            //toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaTableX], StringTable.GetResString(StringTable.IDS_MoveSpeed, ntbFCD.Text + " "));          //FCD移動速度(テーブル)
            //toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaII], StringTable.GetResString(StringTable.IDS_MoveSpeed, ntbFID.Text + " "));              //FID/FDD移動速度(検出器)
            //toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaTableY], StringTable.GetResString(StringTable.IDS_MoveSpeed, ntbTableXPos.Text + " "));    //Y軸移動速度(テーブル)
            toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaTableRotate], StringTable.GetResString(StringTable.IDS_SpeedOf, ntbRotate.Caption + " "));    //回転速度(テーブル)
            toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaTableUpDown], StringTable.GetResString(StringTable.IDS_MoveSpeed, ntbUpDown.Caption + " "));  //高さ移動速度(テーブル)
            toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaTableX], StringTable.GetResString(StringTable.IDS_MoveSpeed, ntbFCD.Caption + " "));          //FCD移動速度(テーブル)
            toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaII], StringTable.GetResString(StringTable.IDS_MoveSpeed, ntbFID.Caption + " "));              //FID/FDD移動速度(検出器)
            toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaTableY], StringTable.GetResString(StringTable.IDS_MoveSpeed, ntbTableXPos.Caption + " "));    //Y軸移動速度(テーブル)

            toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaFTableX], StringTable.GetResString(StringTable.IDS_MoveSpeed, CTResources.LoadResString(12130) + "XY "));  //微調XY移動速度(微調テーブル)

            toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaTiltAndRot_Tilt], StringTable.GetResString(StringTable.IDS_SpeedOf ,ntbTilt.Caption + " ")); //Rev22.00 追加 回転傾斜テーブル by長野 2015/08/20
            toolTip.SetToolTip(cboSpeed[(int)modMechaControl.MechaConstants.MechaTiltAndRot_Rot], StringTable.GetResString(StringTable.IDS_SpeedOf, ntbTiltRot.Caption + " "));  //Rev22.00 追加 回転傾斜テーブル by長野 2015/08/20

            toolTip.SetToolTip(cwbtnRotate[0], StringTable.BuildResStr(StringTable.IDS_RotateCCW, StringTable.IDS_Table));          //テーブル左回転
            toolTip.SetToolTip(cwbtnRotate[1], StringTable.BuildResStr(StringTable.IDS_RotateCW, StringTable.IDS_Table));           //テーブル右回転

            //v19.51　昇降タイプで場合分けする by長野 2014/02/27
            //変更2014/10/07hata_v19.51反映
            //toolTip.SetToolTip(cwbtnUpDown[0], StringTable.BuildResStr(StringTable.IDS_Down, StringTable.IDS_Table));               //テーブル下降
            //toolTip.SetToolTip(cwbtnUpDown[1], StringTable.BuildResStr(StringTable.IDS_Up, StringTable.IDS_Table));                 //テーブル上昇
            if ((CTSettings.t20kinf.Data.ud_type == 0))
            {
                toolTip.SetToolTip(cwbtnUpDown[0], StringTable.BuildResStr(StringTable.IDS_Down, StringTable.IDS_Table));           //テーブル下降
                toolTip.SetToolTip(cwbtnUpDown[1], StringTable.BuildResStr(StringTable.IDS_Up, StringTable.IDS_Table));             //テーブル上昇
            }
            else
            {
                toolTip.SetToolTip(cwbtnUpDown[0], StringTable.BuildResStr(StringTable.IDS_Down, StringTable.IDS_XrayDetector));    //X線検出器下降
                toolTip.SetToolTip(cwbtnUpDown[1], StringTable.BuildResStr(StringTable.IDS_Up, StringTable.IDS_XrayDetector));      //X線検出器上昇
            }            
                      
            toolTip.SetToolTip(cwbtnRotateXray[0], StringTable.BuildResStr(StringTable.IDS_RotateCW, StringTable.IDS_XrayTube));    //Ｘ線管右回転
            toolTip.SetToolTip(cwbtnRotateXray[1], StringTable.BuildResStr(StringTable.IDS_RotateCCW, StringTable.IDS_XrayTube));   //Ｘ線管左回転

            toolTip.SetToolTip(cwbtnMove[0], StringTable.BuildResStr(StringTable.IDS_MoveL, StringTable.IDS_Table));                //テーブル左移動
            toolTip.SetToolTip(cwbtnMove[1], StringTable.BuildResStr(StringTable.IDS_MoveR, StringTable.IDS_Table));                //テーブル右移動
            toolTip.SetToolTip(cwbtnMove[2], StringTable.BuildResStr(StringTable.IDS_FWDEnlarge, StringTable.IDS_Table));           //テーブル前進（拡大）
            toolTip.SetToolTip(cwbtnMove[3], StringTable.BuildResStr(StringTable.IDS_BCKReduction, StringTable.IDS_Table));         //テーブル後退（縮小）
            //cwbtnMove(4).ToolTipText = GetResString(IDS_Forward, "I.I.")                'I.I.前進  '削除 by 間々田 2009/07/21 メカ詳細画面に移動
            //cwbtnMove(5).ToolTipText = GetResString(IDS_Back, "I.I.")                   'I.I.後退  '削除 by 間々田 2009/07/21 メカ詳細画面に移動
            toolTip.SetToolTip(cwbtnMove[6], StringTable.BuildResStr(StringTable.IDS_MoveL, StringTable.IDS_XrayTube));             //Ｘ線管左移動
            toolTip.SetToolTip(cwbtnMove[7], StringTable.BuildResStr(StringTable.IDS_MoveR, StringTable.IDS_XrayTube));             //Ｘ線管右移動
            toolTip.SetToolTip(cwbtnMove[8], StringTable.BuildResStr(StringTable.IDS_FWDEnlarge, StringTable.IDS_XrayTube));        //Ｘ線管前進（拡大）
            toolTip.SetToolTip(cwbtnMove[9], StringTable.BuildResStr(StringTable.IDS_BCKReduction, StringTable.IDS_XrayTube));      //Ｘ線管後退（縮小）

            toolTip.SetToolTip(cwbtnFineTable[0], StringTable.BuildResStr(StringTable.IDS_MoveL, StringTable.IDS_FTable));          //微調テーブル左移動
            toolTip.SetToolTip(cwbtnFineTable[1], StringTable.BuildResStr(StringTable.IDS_MoveR, StringTable.IDS_FTable));          //微調テーブル右移動


            //v14.24以下に変更 by 間々田 2009/03/10 メカ軸の名称はコモンを参照する
            //   さらにリソースの中身も変更
            //       リソース9208 "X軸ドライバーエラー"      → "%1軸ドライバーエラー"
            //       リソース9209 "Y軸オーバーヒート"        → "%1軸オーバーヒート"
            //       リソース9217 "テーブルX左移動中接触"    → "テーブル%1左移動中接触"
            //       リソース9218 "テーブルX右移動中接触"    → "テーブル%1右移動中接触"
            //       リソース9219 "テーブルY前進中接触"      → "テーブル%1前進中接触"
            //       リソース9220 "テーブルY後退中接触"      → "テーブル%1後退中接触"
            //       リソース9221 "X軸原点復帰タイムアウト"  → "%1軸原点復帰タイムアウト"
            //       リソース9223 "Ｘ軸制御エラー"           → "%1軸制御エラー"
            mySeqStatus[(int)SeqIdxConstants.IdxEmergency].Caption = CTResources.LoadResString(9200);         //非常停止
            mySeqStatus[(int)SeqIdxConstants.IdxXray225Trip].Caption = CTResources.LoadResString(9201);       //X線225KVトリップ
            mySeqStatus[(int)SeqIdxConstants.IdxXray160Trip].Caption = CTResources.LoadResString(9202);       //X線160KVトリップ
            mySeqStatus[(int)SeqIdxConstants.IdxFilterTouch].Caption = CTResources.LoadResString(9203);       //フィルタユニット接触
            mySeqStatus[(int)SeqIdxConstants.IdxXray225Touch].Caption = CTResources.LoadResString(9204);      //X線管225KV接触
            mySeqStatus[(int)SeqIdxConstants.IdxXray160Touch].Caption = CTResources.LoadResString(9205);      //X線管160KV接触
            mySeqStatus[(int)SeqIdxConstants.IdxRotTouch].Caption = CTResources.LoadResString(9206);          //回転テーブル接触
            mySeqStatus[(int)SeqIdxConstants.IdxTiltTouch].Caption = CTResources.LoadResString(9207);         //チルトテーブル接触
            mySeqStatus[(int)SeqIdxConstants.IdxXDriverHeat].Caption = StringTable.GetResString(9208, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[1].GetString()));  //%1軸ドライバーエラー
            mySeqStatus[(int)SeqIdxConstants.IdxYDriverHeat].Caption = StringTable.GetResString(9209, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[0].GetString()));  //%1軸オーバーヒート
            mySeqStatus[(int)SeqIdxConstants.IdxXrayDriverHeat].Caption = CTResources.LoadResString(9210);    //X線管切替オーバーヒート
            mySeqStatus[(int)SeqIdxConstants.IdxSeqCpuErr].Caption = CTResources.LoadResString(9211);         //シーケンサCPUエラー
            mySeqStatus[(int)SeqIdxConstants.IdxSeqBatteryErr].Caption = CTResources.LoadResString(9212);     //シーケンサバッテリーエラー
            mySeqStatus[(int)SeqIdxConstants.IdxSeqKzCommErr].Caption = CTResources.LoadResString(9213);      //シーケンサKL通信エラー(KZ)
            mySeqStatus[(int)SeqIdxConstants.IdxSeqKvCommErr].Caption = CTResources.LoadResString(9214);      //シーケンサKL通信エラー(KV)
            mySeqStatus[(int)SeqIdxConstants.IdxFilterTimeout].Caption = CTResources.LoadResString(9215);     //フィルタタイムアウト
            mySeqStatus[(int)SeqIdxConstants.IdxTiltTimeout].Caption = CTResources.LoadResString(9216);       //チルト原点復帰タイムアウト
            mySeqStatus[(int)SeqIdxConstants.IdxXLTouch].Caption = StringTable.GetResString(9217, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[1].GetString()));      //テーブル%1左移動中接触
            mySeqStatus[(int)SeqIdxConstants.IdxXRTouch].Caption = StringTable.GetResString(9218, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[1].GetString()));      //テーブル%1右移動中接触
            mySeqStatus[(int)SeqIdxConstants.IdxYFTouch].Caption = StringTable.GetResString(9219, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[0].GetString()));      //テーブル%1前進中接触
            mySeqStatus[(int)SeqIdxConstants.IdxYBTouch].Caption = StringTable.GetResString(9220, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[0].GetString()));      //テーブル%1後退中接触
            mySeqStatus[(int)SeqIdxConstants.IdxXTimeout].Caption = StringTable.GetResString(9221, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[1].GetString()));     //%1軸原点復帰タイムアウト
            mySeqStatus[(int)SeqIdxConstants.IdxIIDriverHeat].Caption = CTResources.LoadResString(9222);      //II軸オーバーヒート
            mySeqStatus[(int)SeqIdxConstants.IdxXDriveErr].Caption = StringTable.GetResString(9223, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[1].GetString()));    //%1軸制御エラー
            mySeqStatus[(int)SeqIdxConstants.IdxXrayCameraUDError].Caption = StringTable.GetResString(9223,modLibrary.RemoveNull(CTResources.LoadResString(27002)));//%1軸制御エラー Rev26.40 add by chouno 2019/02/24

            //v16.01 追加 by 山影 10-02-02
            mySeqStatus[(int)SeqIdxConstants.IdxCTIIDrive].Caption = CTResources.LoadResString(StringTable.IDS_CTIIDrive);        //CT用I.I.に切替中       'v17.20 検出器切替機能有の場合，検出器１に切替中を示す。 by 長野 2010-09-01
            mySeqStatus[(int)SeqIdxConstants.IdxTVIIDrive].Caption = CTResources.LoadResString(StringTable.IDS_TVIIDrive);        //高速用I.I.に切替中     'v17.20 検出器切替機能有の場合，検出器２に切替中を示す。 by 長野 2010-09-01
            mySeqStatus[(int)SeqIdxConstants.IdxCTIIPos].Caption = CTResources.LoadResString(StringTable.IDS_CTIIPos);            //CTI.I.位置             'v17.20 検出器切替機能有の場合，検出器１が撮影位置にいることを示す。 by 長野 2010-09-01
            mySeqStatus[(int)SeqIdxConstants.IdxTVIIPos].Caption = CTResources.LoadResString(StringTable.IDS_TVIIPos);            //高速I.I.位置           'v17.20 検出器切替機能有の場合，検出器２が撮影位置にいることを示す。 by 長野 2010-09-01
            mySeqStatus[(int)SeqIdxConstants.IdxUnknownIIPos].Caption = CTResources.LoadResString(StringTable.IDS_UnknownIIPos);  //I.I.位置不定           'v17.20 検出器切替機能有の場合，検出器位置が不定であることを示す。 by 長野 2010-09-01
            mySeqStatus[(int)SeqIdxConstants.IdxOKIIMove].Caption = CTResources.LoadResString(StringTable.IDS_IIOkMove);          //I.I.切替可否           'v17.20 検出器切替機能有の場合，検出器切替可能であることを示す。 by 長野 2010-09-01
            mySeqStatus[(int)SeqIdxConstants.IdxWarmUpNow].Caption = CTResources.LoadResString(StringTable.IDS_WarmUpNow);        //ｳｫｰﾑｱｯﾌﾟ中

            //v23.10 追加 ここから by 長野 15-09-20
            mySeqStatus[(int)SeqIdxConstants.IdxMicroFpdPos].Caption = CTResources.LoadResString(StringTable.IDS_MicroFpdPos);                  //FPDX線1位置
            mySeqStatus[(int)SeqIdxConstants.IdxMicroFpdShiftPos].Caption = CTResources.LoadResString(StringTable.IDS_MicroFpdShiftPos);        //FPDX線1(シフト)位置
            mySeqStatus[(int)SeqIdxConstants.IdxNanoFpdPos].Caption = CTResources.LoadResString(StringTable.IDS_NanoFpdPos);                    //FPDX線2位置
            mySeqStatus[(int)SeqIdxConstants.IdxNanoFpdShiftPos].Caption = CTResources.LoadResString(StringTable.IDS_NanoFpdShiftPos);          //FPDX線2(シフト)位置
            mySeqStatus[(int)SeqIdxConstants.IdxMicroFpdBusy].Caption = CTResources.LoadResString(StringTable.IDS_MicroFpdBusy);                //FPDX線1切り替え中
            mySeqStatus[(int)SeqIdxConstants.IdxMicroFpdShiftBusy].Caption = CTResources.LoadResString(StringTable.IDS_MicroFpdShiftBusy);      //FPDX線1切り替え中(シフト)
            mySeqStatus[(int)SeqIdxConstants.IdxNanoFpdBusy].Caption = CTResources.LoadResString(StringTable.IDS_NanoFpdBusy);                  //FPDX線2切り替え中
            mySeqStatus[(int)SeqIdxConstants.IdxNanoFpdShiftBusy].Caption = CTResources.LoadResString(StringTable.IDS_NanoFpdShiftBusy);        //FPDX線2切り替え中(シフト)
            mySeqStatus[(int)SeqIdxConstants.IdxUnknownFpdPos].Caption = CTResources.LoadResString(StringTable.IDS_UnknownFpdPos);              //FPDX線位置(シフト含む)状態
            //v23.10 追加 ここまで by 長野 15-09-20
         
            //v17.51追加 FPDの場合は 0,-90,-180,-270とする
            lblRot0.Text = (CTSettings.detectorParam.Use_FlatPanel ? "0" : "0");
            lblRot1.Text = (CTSettings.detectorParam.Use_FlatPanel ? "-270" : "90");
            lblRot2.Text = (CTSettings.detectorParam.Use_FlatPanel ? "-180" : "180");
            lblRot3.Text = (CTSettings.detectorParam.Use_FlatPanel ? "-90" : "270");

            //追加2014/12/22hata
            if (lblRot1.Text == "-270")
            {
                //変更2015/01/19hata
                //lblRot1.Location = new Point(lblRot1.Left - 7, lblRot1.Top - lblRot1.Height / 2);
                //lblRot3.Location = new Point(lblRot3.Left - 3, lblRot3.Top - lblRot3.Height / 2);
                lblRot1.Left = 159;
            }
        
        }


        //*************************************************************************************************
        //機　　能： 各コントロールの初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        //Private Sub InitControls()
        public void InitControls() //v17.20 public化 by 長野
        { 
            //昇降
            if (CTSettings.scaninh.Data.ud_mecha_pres != 0)
            {
                ntbUpDown.Visible = false;
                cboSpeed[(int)modMechaControl.MechaConstants.MechaTableUpDown].Visible = false;
                cwsldUpDown[0].Visible = false;
                //cwsldUpDown[1].Visible = false;   //削除2014/12/22hata_ｽﾗｲﾀﾞを1つにする
                cwbtnUpDown[0].Visible = false;
                cwbtnUpDown[1].Visible = false;
                lblUP.Visible = false;
                lblDOWN.Visible = false;
                cmdPosExec.Visible = false;
                cwnePos.Visible = false;
                Label22.Visible = false;
                txtUpDownPos.Visible = false;   //追加2015/01/08hata
            }
        
            //微調Ｘ
            ntbFTablePosX.Visible = (CTSettings.scaninh.Data.fine_table == 0) && (CTSettings.scaninh.Data.fine_table_x == 0);
        
            //微調Ｙ
            ntbFTablePosY.Visible = (CTSettings.scaninh.Data.fine_table == 0) && (CTSettings.scaninh.Data.fine_table_y == 0);
        
            //微調移動速度
            cboSpeed[(int)modMechaControl.MechaConstants.MechaFTableX].Visible = (CTSettings.scaninh.Data.fine_table == 0);

            //回転傾斜テーブル Rev22.00 追加 by長野 2015/08/20
            ntbTilt.Visible = (CTSettings.scaninh.Data.tilt_and_rot == 0);
            ntbTiltRot.Visible = (CTSettings.scaninh.Data.tilt_and_rot == 0);
            cboSpeed[(int)modMechaControl.MechaConstants.MechaTiltAndRot_Rot].Visible = (CTSettings.scaninh.Data.tilt_and_rot == 0);
            cboSpeed[(int)modMechaControl.MechaConstants.MechaTiltAndRot_Tilt].Visible = (CTSettings.scaninh.Data.tilt_and_rot == 0);
            if (CTSettings.scaninh.Data.tilt_and_rot == 0)
            {
                fraTiltAndRot.Visible = true;
            }
            else
            {
                fraTiltAndRot.Visible = false;
            }

            //追加2014/10/07hata_v19.51反映
            //微調のラベル       'v18.00追加 byやまおか 2011/01/20 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            lblFTable.Visible = (CTSettings.scaninh.Data.fine_table == 0);
            //追加2015/01/16hata
            lblFTable.Left = ShpFTable.Left + ShpFTable.Width / 2 -lblFTable.Width / 2;

            //II/FPD移動がない場合、移動用・速度設定用コントロールは非表示
            cboSpeed[(int)modMechaControl.MechaConstants.MechaII].Visible = (CTSettings.scaninh.Data.ii_move == 0);
            //cwbtnMove(4).Visible = (.ii_move = 0)      '削除 by 間々田 2009/07/21 メカ準備画面に移動
            //cwbtnMove(5).Visible = (.ii_move = 0)      '削除 by 間々田 2009/07/21 メカ準備画面に移動
            fraIIMove.Visible = (CTSettings.scaninh.Data.ii_move == 0);          //追加 by 間々田 2009/07/21
            //古い装置はFDD500mmまでしかいかないため400mmを非表示にする  'Rev17.44現地応急対応 byやまおか 2011/02/17
            if (Convert.ToInt32( CTSettings.t20kinf.Data.system_type.GetString()) == 0 )
            {
                ctchkIIMove[0].Visible = false;
                lblIIMove[0].Visible = false;
            }

            //Rev26.10 FD-Z, FD-ZⅡによってFDD最大値を変更する by井上 2017/12/12
            if (CTSettings.scaninh.Data.max_fdd1200 == 1)
            {
                ctchkIIMove4.Visible = false;
                lblIIMove4.Visible = false;
            }

            //追加2014/10/07hata_v19.51反映
            //検出器シフトの点線     'v18.00追加 byやまおか 2011/01/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //pnlDetShift.Visible = CTSettings.DetShiftOn;
            pnlDetShift.Visible = (CTSettings.DetShiftOn || CTSettings.W_ScanOn);
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //cwbtnDetShift.Visible = CTSettings.DetShiftOn;
            cwbtnDetShift.Visible = (CTSettings.DetShiftOn || CTSettings.W_ScanOn);

            //Rev23.20 追加 by長野 2015/12/17
            //if (CTSettings.scaninh.Data.lr_sft == 0)
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
            if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
            {
                pnlDetShift.Left = ShpCabinet.Left + ShpCabinet.Width / 2 - pnlDetShift.Width / 2;
            }

            //Rev23.20 外観カメラ無しの場合はfraMechaAutoPosをリサイズする
            if (CTSettings.scaninh.Data.ExObsCamera == 1)
            {
                cmdFromExObsCam.Visible = false;
                fraAutoScanPos.Width = (int)((cmdFromSlice.Width + cmdFromTrans.Width) * 1.2);
                int margin = (fraAutoScanPos.Width - cmdFromSlice.Width - cmdFromTrans.Width) / 3;
                cmdFromTrans.Left = (int)(margin);
                cmdFromSlice.Left = (int)(cmdFromTrans.Left + cmdFromTrans.Width + margin);
            }

            //追加2014/10/07hata_v19.51反映
            //スキャンエリア表示(微調がある場合は表示しない) 'v18.00追加 byやまおか 2011/02/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //v19.50 条件変更 by長野 2013/11/20
            //fraScanArea.Visible = DetShiftOn And CBool(scaninh.fine_table <> 0)
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //fraScanArea.Visible = CTSettings.DetShiftOn & Convert.ToBoolean(CTSettings.scaninh.Data.fine_table != 0) & Convert.ToBoolean(CTSettings.scaninh.Data.avmode == 0);
            fraScanArea.Visible = (CTSettings.W_ScanOn || CTSettings.DetShiftOn) & Convert.ToBoolean(CTSettings.scaninh.Data.fine_table != 0) & Convert.ToBoolean(CTSettings.scaninh.Data.avmode == 0);
            if (CTSettings.W_ScanOn == true)
            {
                fraScanArea.Height = 76;
            }

            //fraScanArea.Visible = True
            fraScanArea.Top = ntbFTablePosX.Top + 14;            //微調のステータス表示位置
         
            //Rev23.30 1画素サイズ表示 by長野 2016/02/05
            //fraMechaScanCondiion.Visible = CTSettings.scaninh.Data.ExObsCamera == 0 && fraScanArea.Visible == false;//スキャンエリア表示優先 
            //Rev23.31 条件変更 X線または検出器切り替えがない場合は表示
            fraMechaScanCondiion.Visible = ((CTSettings.scaninh.Data.high_speed_camera == 1 && CTSettings.scaninh.Data.multi_tube == 1 && CTSettings.scaninh.Data.second_detector == 1) && fraScanArea.Visible == false);//スキャンエリア表示優先 

            //fraScanArea.Visible = True
            fraMechaScanCondiion.Top = ntbFTablePosX.Top + 50;       //微調のステータス表示位置
            //fraAutoScanPos.Left = fraMechaScanCondiion.Left;         //スキャン位置指定フレームの微調整
            //Rev25.01 微調整 by長野 2016/12/05 
            fraAutoScanPos.Left = fraMechaScanCondiion.Left + 10;         //スキャン位置指定フレームの微調整
            
            fraMechaPos.Left = fraAutoScanPos.Left;            //自動スキャン位置指定フレームと同じ位置

            //試料テーブルX軸(光軸方向)(旧Y軸)ボタン //Rec23.20 変更 by長野 2015/12/17
            if (CTSettings.scaninh.Data.table_y == 1)
            {
                cwbtnMove[2].Visible = false;
                cwbtnMove[3].Visible = false;
                cboSpeed[2].Visible = false;
            }
            else
            {
                cwbtnMove[2].Visible = true;
                cwbtnMove[3].Visible = true;
                cboSpeed[2].Visible = true;
            }
            
            //微調ボタン
            cwbtnFineTable[0].Visible = (CTSettings.scaninh.Data.fine_table == 0) && (CTSettings.scaninh.Data.fine_table_x == 0) && (CTSettings.scaninh.Data.fine_table_y == 0);
            cwbtnFineTable[1].Visible = (CTSettings.scaninh.Data.fine_table == 0) && (CTSettings.scaninh.Data.fine_table_x == 0) && (CTSettings.scaninh.Data.fine_table_y == 0);
        
            //コリメータフレーム
            fraCollimator.Visible = (CTSettings.scaninh.Data.collimator == 0);
            cmdCollimator[0].Visible = (CTSettings.scaninh.Data.collimator_rl == 0);
            cmdCollimator[1].Visible = (CTSettings.scaninh.Data.collimator_rl == 0);
            cmdCollimator[2].Visible = (CTSettings.scaninh.Data.collimator_rl == 0);
            cmdCollimator[3].Visible = (CTSettings.scaninh.Data.collimator_rl == 0);
            cmdCollimator[4].Visible = (CTSettings.scaninh.Data.collimator_ud == 0);
            cmdCollimator[5].Visible = (CTSettings.scaninh.Data.collimator_ud == 0);
            cmdCollimator[6].Visible = (CTSettings.scaninh.Data.collimator_ud == 0);
            cmdCollimator[7].Visible = (CTSettings.scaninh.Data.collimator_ud == 0);

            //Ｘ線管干渉フレーム
            //fraTableRestriction.Visible = (CTSettings.scaninh.Data.table_restriction == 0) && (CTSettings.scaninh.Data.seqcomm == 0);
            //Rev23.20 条件追加 
            fraTableRestriction.Visible = (CTSettings.scaninh.Data.table_restriction == 0) && (CTSettings.scaninh.Data.seqcomm == 0) && (CTSettings.scaninh.Data.ct_gene2and3 != 0);
        
            //Ｘ線管操作フレーム
            fraXrayRotate.Visible = (CTSettings.scaninh.Data.xray_rotate == 0);
        
            //Ｘ線管回転ありの場合のみ表示する項目
            ntbXrayRotPos.Visible = (CTSettings.scaninh.Data.xray_rotate == 0);
            cboSpeed[(int)modMechaControl.MechaConstants.MechaXrayRotate].Visible = (CTSettings.scaninh.Data.xray_rotate == 0);
        
            ntbXrayPosX.Visible = (CTSettings.scaninh.Data.xray_rotate == 0);
            cboSpeed[(int)modMechaControl.MechaConstants.MechaXrayX].Visible = (CTSettings.scaninh.Data.xray_rotate == 0);
   
            ntbXrayPosY.Visible = (CTSettings.scaninh.Data.xray_rotate == 0);
            cboSpeed[(int)modMechaControl.MechaConstants.MechaXrayY].Visible = (CTSettings.scaninh.Data.xray_rotate == 0);

            //追加2014/10/07hata_v19.51反映 --- ここから　---
            //v16.01 追加 by 山影 10-02-02
            //Ｘ線I.I.絞り
            //fraIris.Visible = CTSettings.HscOn & modHighSpeedCamera.IsHSCmode;
            //change by chouno 2019/02/12 Rev26.20
            fraIris.Visible = CTSettings.HscOn & (modHighSpeedCamera.IsHSCmode || modHighSpeedCamera.IsDropTestmode);

            //CT/高速切替ボタン
            fraHighSpeedCamera.Visible = CTSettings.HscOn;

            if (CTSettings.HscOn)
            {
                cwbtnChangeMode[0].Value = modSeqComm.MySeq.stsCTIIPos;
                //cwbtnChangeMode[1].Value = modSeqComm.MySeq.stsTVIIPos;
                //Rev26.40 change by chouno 2019/02/17
                cwbtnChangeMode[1].Value = modSeqComm.MySeq.stsTVIIPos || modSeqComm.MySeq.stsFPDLShiftPos;
                
                //Rev26.40 セットする方法がタッチパネルの場合はenable = false(ステータス表示のみに使用する) by chouno 2019/02/12
                if (CTSettings.iniValue.HSCSettingType == 1)
                {
                    cwbtnChangeMode[0].FlatStyle = FlatStyle.Flat;
                    cwbtnChangeMode[1].FlatStyle = FlatStyle.Flat;
                    cwbtnChangeMode[0].Enabled = false;
                    cwbtnChangeMode[1].Enabled = false;
                }
            }

            //v17.20 検出器切替ボタン 追加 by 長野 10-08-31
            if (CTSettings.SecondDetOn)
            {
                fraChangeDetector.Visible = true;
                cwbtnChangeDet[0].Value = modSeqComm.MySeq.stsCTIIPos;
                cwbtnChangeDet[1].Value = modSeqComm.MySeq.stsTVIIPos;
            }
            //追加2014/10/07hata_v19.51反映 --- ここまで　---

            //Rev23.10 X線切替ボタン 追加 by長野 2015/09/18
            if (CTSettings.ChangeXrayOn)
            {
                fraChangeXray.Visible = true;
                cwbtnChangeXray[0].Value = (modSeqComm.MySeq.stsMicroFPDPos || modSeqComm.MySeq.stsNanoFPDShiftPos);
                cwbtnChangeXray[1].Value = (modSeqComm.MySeq.stsNanoFPDPos || modSeqComm.MySeq.stsNanoFPDShiftPos);
            }

            //v17.20 メカリセットボタン      追加 by 長野 10-09-22
            if (CTSettings.SecondDetOn)
            {
                cmdMechaAllReset.Visible = true;
            }
            else
            {
                cmdMechaAllReset.Visible = false;
            }

            //ソフト起動後に回転リセットする必要がある場合   'v18.00追加 byやまおか 2011/03/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //v19.51 X線・検出器昇降の場合の条件も追加する
            //Rev23.10 計測CTモードを条件に追加 by長野 2015/11/14
            if ((CTSettings.scaninh.Data.avmode == 0) | (CTSettings.t20kinf.Data.ud_type == 1) | (CTSettings.scaninh.Data.cm_mode == 0))
            {
                modMechaControl.Flg_StartupRotReset = false;                //False(未完了)で初期化
            }
            else
            {
                modMechaControl.Flg_StartupRotReset = true;                //True(完了)で初期化
            }

            //ソフト起動後に昇降リセットする必要がある場合    'v19.51 追加 by長野 2014/02/27
            //if (CTSettings.t20kinf.Data.ud_type == 1)
            //if (CTSettings.t20kinf.Data.ud_type == 1 || CTSettings.scaninh.Data.lr_sft == 0) //Rev23.20 条件追加 by長野 2016/01/23
            if (CTSettings.t20kinf.Data.ud_type == 1 || CTSettings.scaninh.Data.ct_gene2and3 == 0) //Rev25.00 修正 by長野 2016/07/07
            {
                modMechaControl.Flg_StartupUpDownReset = false;                //未完了で初期化
            }
            else
            {
                modMechaControl.Flg_StartupUpDownReset = true;                //完了で初期化
            }

            //v19.50 産業用CTの場合は、自動スキャン位置指定は非表示にする by長野 2014/01/15
            if ((CTSettings.scaninh.Data.avmode == 0))
            {
                fraAutoScanPos.Visible = false;
            }
            else
            {
                fraAutoScanPos.Visible = true;
            }


            //v19.50 タイマーの統合 シーケンサと位置決めボードの軸に関するUIの更新フラグは、初期値false by長野 2013/12/17
            modMechaControl.Flg_SeqCommUpdate = false;
            modMechaControl.Flg_MechaControlUpdate = false;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*	GroupBox には、FormBorderStyleなし

            //フレームの消去 v16.01 追加 by 山影 10-02-02
            fraHighSpeedCamera.FormBorderStyle = FormBorderStyle.None;
            fraMechaPos.FormBorderStyle = FormBorderStyle.None;
            fraUpDown.FormBorderStyle = FormBorderStyle.None;
            fraMechaControl.FormBorderStyle = FormBorderStyle.None;
            //v17.20 検出器切り替え用フレームの消去を追加 by 長野 10-08-31
            fraChangeDetector.FormBorderStyle = FormBorderStyle.None;
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版


            //変更2014/10/07hata_v19.51反映
            //v19.50 コンポーネントワークスのスライダーを反転 by長野 2014/02/04
            ////位置指定スライダーの設定：コモンt20kinf.ud_type=1の時、上側をupper_limit、下側をlower_limitとする）
            if (CTSettings.t20kinf.Data.ud_type == 1)
            {
                //cwsldUpDown[0].Minimum = CTSettings.t20kinf.Data.upper_limit / 100;
                //cwsldUpDown[0].Maximum = CTSettings.t20kinf.Data.lower_limit / 100;
                cwsldUpDown[0].Reverse = false;
            }
            else
            {
                //cwsldUpDown[0].Minimum = CTSettings.t20kinf.Data.lower_limit / 100;
                //cwsldUpDown[0].Maximum = CTSettings.t20kinf.Data.upper_limit / 100;
                cwsldUpDown[0].Reverse = true;
            }

            //削除2014/12/22hata_ｽﾗｲﾀﾞを1つにする
            //cwsldUpDown[1].Reverse = cwsldUpDown[0].Reverse;
            
            //2014/11/07hata キャストの修正
            //cwsldUpDown[0].Maximum = CTSettings.t20kinf.Data.upper_limit / 100;
            //cwsldUpDown[0].Minimum = CTSettings.t20kinf.Data.lower_limit / 100;
            //cwsldUpDown[0].TickFrequency = (int)((cwsldUpDown[0].Maximum - cwsldUpDown[0].Minimum) / 2);
            //cwsldUpDown[1].TickFrequency = (int)((cwsldUpDown[0].Maximum - cwsldUpDown[0].Minimum) / 2);
            cwsldUpDown[0].Maximum = Convert.ToInt32(CTSettings.t20kinf.Data.upper_limit / 100F);
            cwsldUpDown[0].Minimum = Convert.ToInt32(CTSettings.t20kinf.Data.lower_limit / 100F);
       
            //削除2015/02/12hata_スライダー新規
            //cwsldUpDown[0].TickFrequency = Convert.ToInt32((cwsldUpDown[0].Maximum - cwsldUpDown[0].Minimum) / 2F);
            
            //削除2014/12/22hata_ｽﾗｲﾀﾞを1つにする
            //cwsldUpDown[1].TickFrequency = Convert.ToInt32((cwsldUpDown[0].Maximum - cwsldUpDown[0].Minimum) / 2F);
            //cwsldUpDown[1].Maximum = cwsldUpDown[0].Maximum;
            //cwsldUpDown[1].Minimum = cwsldUpDown[0].Minimum;
			
            //Rev20.00 //昇降タイプによる場合わけを追加 by長野 2015/02/16
            if (CTSettings.t20kinf.Data.ud_type == 0)
            {
                //昇降スケールラベルの設定
                lblMin.Text = Convert.ToString(cwsldUpDown[0].Minimum);
                lblMax.Text = Convert.ToString(cwsldUpDown[0].Maximum);
            }
            else
            {
                //昇降スケールラベルの設定
                lblMin.Text = Convert.ToString(cwsldUpDown[0].Maximum);
                lblMax.Text = Convert.ToString(cwsldUpDown[0].Minimum);
            }
            //2014/11/07hata キャストの修正
            //lblMiddle.Text = Convert.ToString((int)((cwsldUpDown[0].Maximum - cwsldUpDown[0].Minimum) / 2));
            lblMiddle.Text = Convert.ToString(Convert.ToInt32((cwsldUpDown[0].Maximum - cwsldUpDown[0].Minimum) / 2F));
            
            //チェック
            if (CTSettings.GValLowerLimit > CTSettings.GValUpperLimit)
            {        
                //エラー表示：昇降下限値が昇降上限値を越えています。
                DialogResult result = MessageBox.Show(CTResources.LoadResString(9520), 
                                                      Application.ProductName, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Abort) Environment.Exit(0);      //[中止]なら終了（Form_Load中に実行する為、Unloadは不可）
            }
    
            //指定高さ位置移動欄：最小値・最大値の設定
            cwnePos.Minimum = Convert.ToDecimal(CTSettings.GValLowerLimit);
            cwnePos.Maximum = Convert.ToDecimal(CTSettings.GValUpperLimit);

            //Rev21.00 cwnePosのインクリメント設定を追加 by長野 2015/03/12
            //ファイルオープン

            StreamReader file = null;
            string buf = null;
            string[] strCell = null;
            double UdIncrement = 0.0;
            try
            {
                //変更2015/01/22hata
                //file = new StreamReader(FileName);
                file = new StreamReader(@"c:\ct\mechadata\boardpara.csv", Encoding.GetEncoding("shift-jis"));

                //while (!FileSystem.EOF(fileNo))
                while ((buf = file.ReadLine()) != null)
                {
                    //１行読み込む
                    if (!string.IsNullOrEmpty(buf))
                    {
                        //カンマで区切って配列に格納
                        strCell = buf.Split(',');

                        //コメントか？
                        if (strCell[0].Trim() == "UdStep")
                        {
                            //先頭列の文字が数字なら情報を取り出す
                            double IsNumeric = 0;
                            if (double.TryParse(strCell[1], out IsNumeric))
                            {
                                UdIncrement = IsNumeric;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                //ファイルクローズ
                if (file != null)
                {
                    file.Close();
                    file = null;
                }
            }
            cwnePos.Increment = (decimal)UdIncrement;

            //追加2015/01/08hata
            if (txtUpDownPos.Visible) txtUpDownPos.BringToFront();
            txtUpDownPos.Text = cwnePos.Value.ToString("0.000");
            cwsldUpDown[0].Value = cwnePos.Value;   //追加2015/01/08hata
            
            //追加2015/02/12hata_スライダー新規
            cwsldUpDown[0].ArrowValue = cwnePos.Value;

            DispUpDownPointer(cwsldUpDown[0].Value);


            //速度の初期設定
            cboSpeed[(int)modMechaControl.MechaConstants.MechaTableRotate].SelectedIndex = (int)SpeedConstants.SpeedFast;     //回転：最速とする
            cboSpeed[(int)modMechaControl.MechaConstants.MechaTableUpDown].SelectedIndex = (int)SpeedConstants.SpeedMiddle;   //
            cboSpeed[(int)modMechaControl.MechaConstants.MechaTableX].SelectedIndex = (int)SpeedConstants.SpeedMiddle;        //
            cboSpeed[(int)modMechaControl.MechaConstants.MechaTableY].SelectedIndex = (int)SpeedConstants.SpeedMiddle;        //
            cboSpeed[(int)modMechaControl.MechaConstants.MechaII].SelectedIndex = (int)SpeedConstants.SpeedMiddle;            //
            cboSpeed[(int)modMechaControl.MechaConstants.MechaFTableX].SelectedIndex = (int)SpeedConstants.SpeedMiddle;       //
            cboSpeed[(int)modMechaControl.MechaConstants.MechaTiltAndRot_Rot].SelectedIndex = (int)SpeedConstants.SpeedMiddle; //  //Rev22.00 回転傾斜テーブル(回転) by長野 2015/08/20
            cboSpeed[(int)modMechaControl.MechaConstants.MechaTiltAndRot_Tilt].SelectedIndex = (int)SpeedConstants.SpeedMiddle; // //Rev22.00 回転傾斜テーブル(チルト) by長野 2015/08/20

            //追加2014/10/07hata_v19.51反映
            //v17.20 テーブルX,Y軸、FDD,検出器切替の4軸リセットボタン by 長野 2010/09/05
            if (CTSettings.SecondDetOn)
            {
                cmdMechaAllReset.Visible = true;
            }

            //Rev23.10 計測CTモードの場合は表示桁数変更 by長野 2015/10/16
            if (CTSettings.scaninh.Data.cm_mode == 0)
            {
                ChangeDispUnit();
            }
        
        }
        //*************************************************************************************************
        //機　　能： 更新処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        public void ChangeDispUnit()
        {
            ////回転
            //ntbRotate.DiscreteInterval = (float)0.001;
            //ntbRotate.Max = (decimal)999.999;
            //ntbRotate.Min = (decimal)(-999.999);
            ////高さ
            //ntbUpDown.DiscreteInterval = (float)0.001;
            //ntbUpDown.Max = (decimal)999.999;
            //ntbUpDown.Min = (decimal)(-999.999);
            ////FCD
            //ntbFCD.DiscreteInterval = (float)0.001;
            //ntbFCD.Max = (decimal)999.999;
            //ntbFCD.Min = (decimal)(-999.999);
            ////FDD
            //ntbFID.DiscreteInterval = (float)0.001;
            //ntbFID.Max = (decimal)9999.999;
            //ntbFID.Min = (decimal)(-9999.999);
            ////Y軸
            //ntbTableXPos.DiscreteInterval = (float)0.001;
            //ntbTableXPos.Max = (decimal)999.999;
            //ntbTableXPos.Min = (decimal)(-999.999);

            //Rev26.14 change by chouno 2018/09/05
            //回転
            ntbRotate.DiscreteInterval = (float)0.001;
            ntbRotate.Max = (decimal)999.999;
            ntbRotate.Min = (decimal)(-999.999);
            //高さ
            ntbUpDown.DiscreteInterval = (float)0.001;
            ntbUpDown.Max = (decimal)999.999;
            ntbUpDown.Min = (decimal)(-999.999);
            //FCD
            ntbFCD.DiscreteInterval = (float)0.01;
            ntbFCD.Max = (decimal)999.99;
            ntbFCD.Min = (decimal)(-999.99);
            //FDD
            ntbFID.DiscreteInterval = (float)0.01;
            ntbFID.Max = (decimal)9999.99;
            ntbFID.Min = (decimal)(-9999.99);
            //Y軸
            ntbTableXPos.DiscreteInterval = (float)0.01;
            ntbTableXPos.Max = (decimal)999.99;
            ntbTableXPos.Min = (decimal)(-999.99);

        }

        //*************************************************************************************************
        //機　　能： 更新処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        //Private Sub Update() 'v19.12 明示的に呼べるようにするため public化 by長野 2013/03/08
        //public void MyUpdate()
        public void MyUpdate(bool ChangeSts = true) //Rev25.00 Rev23.23の修正反映 by長野 2016/08/08
        {

#if (!DebugOn)
            Seq MySeq = modSeqComm.MySeq;
#else

            frmVirtualSeqComm MySeq = modSeqComm.MySeq;
#endif

            //Rev23.10 計測CTモードの場合は、リニアスケール値に変更 by長野 2015/10/15
            if (CTSettings.scaninh.Data.cm_mode != 0)
            {
                //テーブルＸ
                //ntbTableXPos.Value = .stsXPosition / 100                'X軸位置    小数点以下2桁まで対応
                SetSeqValue(SeqIdxValueConstants.IdxXPosition, MySeq.stsXPosition);

                //テーブルＹ
                //ntbFCD.Value = .stsFCD / 10                             'ＦＣＤ
                SetSeqValue(SeqIdxValueConstants.IdxFCD, MySeq.stsFCD);

                //I.I.移動
                //ntbFID.Value = .stsFID / 10                             'ＦＩＤ
                SetSeqValue(SeqIdxValueConstants.IdxFID, MySeq.stsFID);
            }
            else if(CTSettings.scaninh.Data.cm_mode == 0)
            {
                //テーブルＸ
                //ntbTableXPos.Value = .stsXPosition / 100                'X軸位置    小数点以下2桁まで対応
                SetSeqValue(SeqIdxValueConstants.IdxXPosition, MySeq.stsLinearTableY);

                //テーブルＹ
                //ntbFCD.Value = .stsFCD / 10                             'ＦＣＤ
                SetSeqValue(SeqIdxValueConstants.IdxFCD, MySeq.stsLinearFCD);

                //I.I.移動
                //ntbFID.Value = .stsFID / 10                             'ＦＩＤ
                SetSeqValue(SeqIdxValueConstants.IdxFID, MySeq.stsLinearFDD);
 
            }
    
            //I.I.移動（移動終了時監視用）   '追加 by 間々田 2009/06/15
            mySeqStatus[(int)SeqIdxConstants.IdxIIBackward].Value = MySeq.stsIIBackward;   //I.I.後退中
            mySeqStatus[(int)SeqIdxConstants.IdxIIForward].Value = MySeq.stsIIForward;     //I.I.前進中
        
            //Ｘ線管干渉
            if (CTSettings.scaninh.Data.table_restriction == 0)
            {
                mySeqStatus[(int)SeqIdxConstants.IdxTableMovePermit].Value = MySeq.stsTableMovePermit;
            }

            //変更2014/10/07hata_v19.51反映
            //v17.20 透視画像とテーブルの矢印をI.I.とFPDで揃えるため、FPDの場合はステータスを反転させる。検出器位置不定の場合はI.I.とする。 by 長野 2010/09/17
            //if (CTSettings.detectorParam.Use_FlatPanel)    //v29.99 if文の条件を変更 by長野 2013/04/08'''''変更'''''
            if (CTSettings.SecondDetOn & CTSettings.detectorParam.Use_FlatPanel & mod2ndDetctor.IsOKDetPos) 
            {
                mySeqStatus[(int)SeqIdxConstants.IdxXLLimit].Value = MySeq.stsXRLimit; //テーブルＸ左限
                mySeqStatus[(int)SeqIdxConstants.IdxXRLimit].Value = MySeq.stsXLLimit; //テーブルＸ右限
            }
            else
            {
                mySeqStatus[(int)SeqIdxConstants.IdxXLLimit].Value = MySeq.stsXLLimit; //テーブルＸ左限
                mySeqStatus[(int)SeqIdxConstants.IdxXRLimit].Value = MySeq.stsXRLimit; //テーブルＸ右限
            }

            mySeqStatus[(int)SeqIdxConstants.IdxYFLimit].Value = MySeq.stsYFLimit; //テーブルＹ前進限（拡大限）
            mySeqStatus[(int)SeqIdxConstants.IdxYBLimit].Value = MySeq.stsYBLimit; //テーブルＹ後退限（縮小限）
            //SetLimitStatus cwbtnMove(4), "Up", .stsIIFLimit     'I.I.移動前進限 '削除 by 間々田 2009/07/21 メカ準備画面に移動
            //SetLimitStatus cwbtnMove(5), "Down", .stsIIBLimit   'I.I.移動後退限 '削除 by 間々田 2009/07/21 メカ準備画面に移動
         
            //コリメータ
            if (CTSettings.scaninh.Data.collimator == 0)
            {
                mySeqStatus[(int)SeqIdxConstants.IdxColliLOLimit].Value = MySeq.stsColliLOLimit;   //左開限
                mySeqStatus[(int)SeqIdxConstants.IdxColliLCLimit].Value = MySeq.stsColliLCLimit;   //左閉限
                mySeqStatus[(int)SeqIdxConstants.IdxColliROLimit].Value = MySeq.stsColliROLimit;   //右開限
                mySeqStatus[(int)SeqIdxConstants.IdxColliRCLimit].Value = MySeq.stsColliRCLimit;   //右閉限
                mySeqStatus[(int)SeqIdxConstants.IdxColliUOLimit].Value = MySeq.stsColliUOLimit;   //上開限
                mySeqStatus[(int)SeqIdxConstants.IdxColliUCLimit].Value = MySeq.stsColliUCLimit;   //上閉限
                mySeqStatus[(int)SeqIdxConstants.IdxColliDOLimit].Value = MySeq.stsColliDOLimit;   //下開限
                mySeqStatus[(int)SeqIdxConstants.IdxColliDCLimit].Value = MySeq.stsColliDCLimit;   //下閉限
            }
    
            //Ｘ線回転ありの場合
            if (CTSettings.scaninh.Data.xray_rotate == 0)
            {            
                //回転位置
                //ntbXrayRotPos.Value = .stsXrayRotPos / 10000
                SetSeqValue(SeqIdxValueConstants.IdxXrayRotPos, MySeq.stsXrayRotPos);
            
                //Ｘ軸（従来のＹ軸）
                //ntbXrayPosX.Value = .stsXrayFCD / 100
                SetSeqValue(SeqIdxValueConstants.IdxXrayFCD, MySeq.stsXrayFCD);
            
                //Ｙ軸（従来のＸ軸）
                //ntbXrayPosY.Value = .stsXrayXPos / 100
                SetSeqValue(SeqIdxValueConstants.IdxXrayXPos, MySeq.stsXrayXPos);
                    
                mySeqStatus[(int)SeqIdxConstants.IdxXrayXLLimit].Value = MySeq.stsXrayXLLimit; //Ｘ線管Ｘ軸左限
                mySeqStatus[(int)SeqIdxConstants.IdxXrayXRLimit].Value = MySeq.stsXrayXRLimit; //Ｘ線管Ｘ軸右限
                mySeqStatus[(int)SeqIdxConstants.IdxXrayYFLimit].Value = MySeq.stsXrayYFLimit; //Ｘ線管Ｙ軸前進限
                mySeqStatus[(int)SeqIdxConstants.IdxXrayYBLimit].Value = MySeq.stsXrayYBLimit; //Ｘ線管Ｙ軸後退限        
            }
        
            //I.I.視野
            if (CTSettings.scaninh.Data.iifield == 0)
            {
                SetSeqValue(SeqIdxValueConstants.IdxIINo, modSeqComm.GetIINo());
            }
                
            //扉電磁ロック
            if (CTSettings.scaninh.Data.door_lock == 0)
            {
                if (MySeq.stsDoorLock)
                {
                    frmCTMenu.Instance.DoorStatus = frmCTMenu.DoorStatusConstants.DoorLocked;
                }
                else if (MySeq.stsDoorKey)
                {
                    frmCTMenu.Instance.DoorStatus = frmCTMenu.DoorStatusConstants.DoorClosed;
                }
                else
                {
                    frmCTMenu.Instance.DoorStatus = frmCTMenu.DoorStatusConstants.DoorOpened;
                }
		    } 
            //追加2014/10/07hata_v19.51反映
            //電磁ロックがない場合   'v18.00追加 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            else
            {
			    //何もしない
		    }
        
            //扉インターロック
            if (CTSettings.scaninh.Data.door_lock == 0)
            {
                mySeqStatus[(int)SeqIdxConstants.IdxDoorLock].Value = MySeq.stsDoorLock;
            }
            else
            {
                mySeqStatus[(int)SeqIdxConstants.IdxDoorLock].Value = !(MySeq.stsDoorInterlock);
            }
        
            //v16.01 追加 by 山影 10-02-02
            //I.I.切替関連
            //v17.20 検出器切替の場合も同じステータスを使用するので条件を追加 by 長野 10-08-31
            //If HscOn Or SecondDetOn Then
            //v18.00変更 byやまおか 2011/02/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //if (CTSettings.HscOn | CTSettings.SecondDetOn | CTSettings.DetShiftOn)
            //Rev23.10 条件変更 従来のシフトと、X線切替＋シフトは共存できない by長野 2015/09/20 
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //if (CTSettings.HscOn | CTSettings.SecondDetOn | (CTSettings.DetShiftOn && !CTSettings.ChangeXrayOn))
            if (CTSettings.HscOn | CTSettings.SecondDetOn | ((CTSettings.DetShiftOn || CTSettings.W_ScanOn) && !CTSettings.ChangeXrayOn)) 
            {
                mySeqStatus[(int)SeqIdxConstants.IdxCTIIDrive].Value = MySeq.stsCTIIDrive;
                mySeqStatus[(int)SeqIdxConstants.IdxTVIIDrive].Value = MySeq.stsTVIIDrive;
                mySeqStatus[(int)SeqIdxConstants.IdxCTIIPos].Value = MySeq.stsCTIIPos;
                mySeqStatus[(int)SeqIdxConstants.IdxTVIIPos].Value = MySeq.stsTVIIPos;
                //Rev23.20 追加 by長野 2015/12/18
                mySeqStatus[(int)SeqIdxConstants.IdxFPDLShiftPos].Value = MySeq.stsFPDLShiftPos;
                //Rev26.40 add by chouno 2019/02/20
                mySeqStatus[(int)SeqIdxConstants.IdxFPDLShiftBusy].Value = MySeq.stsFPDLShiftBusy;
          
                //mySeqStatus[(int)SeqIdxConstants.IdxUnknownIIPos].Value = (MySeq.stsCTIIDrive | MySeq.stsCTIIPos | MySeq.stsTVIIDrive | MySeq.stsTVIIPos);
                //Rev23.20 変更 by長野 2015/12/17
                //mySeqStatus[(int)SeqIdxConstants.IdxUnknownIIPos].Value = (MySeq.stsCTIIDrive | MySeq.stsCTIIPos | MySeq.stsTVIIDrive | MySeq.stsTVIIPos | MySeq.stsFPDLShiftPos);
                //Rev26.40 add by chouno 2019/02/20
                mySeqStatus[(int)SeqIdxConstants.IdxUnknownIIPos].Value = (MySeq.stsCTIIDrive | MySeq.stsCTIIPos | MySeq.stsTVIIDrive | MySeq.stsTVIIPos | MySeq.stsFPDLShiftPos | MySeq.stsFPDLShiftBusy);
              
                if (CTSettings.HscOn)
                {
                    mySeqStatus[(int)SeqIdxConstants.IdxOKIIMove].Value = modHighSpeedCamera.IsOKIIMove();
                }
                if (CTSettings.SecondDetOn)
                {
                    mySeqStatus[(int)SeqIdxConstants.IdxOKIIMove].Value = mod2ndDetctor.IsSwitchDet() & mod2ndDetctor.IsAllCloseFrm;
                    mySeqStatus[(int)SeqIdxConstants.IdxMechaRstBusy].Value = MySeq.stsMechaRstBusy;
                    mySeqStatus[(int)SeqIdxConstants.IdxMechaRstOK].Value = MySeq.stsMechaRstOK;
                    mySeqStatus[(int)SeqIdxConstants.IdxXOrgReq].Value = MySeq.stsXOrgReq;
                    mySeqStatus[(int)SeqIdxConstants.IdxYOrgReq].Value = MySeq.stsYOrgReq;
                    mySeqStatus[(int)SeqIdxConstants.IdxIIOrgReq].Value = MySeq.stsIIOrgReq;
                    mySeqStatus[(int)SeqIdxConstants.IdxIIChgOrgReq].Value = MySeq.stsIIChgOrgReq;
                }
                //v18.00追加 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                //Rev25.00 Wスキャン追加 by長野 2016/06/19
                //if (CTSettings.DetShiftOn)
                if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                {
                    mySeqStatus[(int)SeqIdxConstants.IdxOKIIMove].Value = mod2ndDetctor.IsSwitchDet();
                }
            }

            //Rev23.20 追加 by長野 2015/12/21
            if (CTSettings.scaninh.Data.ct_gene2and3 == 0)
            {
                mySeqStatus[(int)SeqIdxConstants.IdxFDSystemPos].Value = MySeq.stsFDSystemPos;
                mySeqStatus[(int)SeqIdxConstants.IdxFDSystemBusy].Value = MySeq.stsFDSystemBusy;
            }

            //Rev23.10 追加 by長野 2015/09/20
            //Rev23.40 Wスキャン追加 by長野 2016/06/19
            //if (CTSettings.DetShiftOn && CTSettings.ChangeXrayOn)
            if ((CTSettings.DetShiftOn || CTSettings.W_ScanOn) && CTSettings.ChangeXrayOn)
            {
                mySeqStatus[(int)SeqIdxConstants.IdxOKIIMove].Value = mod2ndXray.IsChangeXray() & mod2ndXray.IsAllCloseFrm;
                mySeqStatus[(int)SeqIdxConstants.IdxMicroFpdPos].Value = MySeq.stsMicroFPDPos;
                mySeqStatus[(int)SeqIdxConstants.IdxMicroFpdShiftPos].Value = MySeq.stsMicroFPDShiftPos;
                mySeqStatus[(int)SeqIdxConstants.IdxNanoFpdPos].Value = MySeq.stsNanoFPDPos;
                mySeqStatus[(int)SeqIdxConstants.IdxNanoFpdShiftPos].Value = MySeq.stsNanoFPDShiftPos;
                mySeqStatus[(int)SeqIdxConstants.IdxMicroFpdBusy].Value = MySeq.stsMicroFPDBusy;
                mySeqStatus[(int)SeqIdxConstants.IdxMicroFpdShiftBusy].Value = MySeq.stsMicroFPDShiftBusy;
                mySeqStatus[(int)SeqIdxConstants.IdxNanoFpdBusy].Value = MySeq.stsNanoFPDBusy;
                mySeqStatus[(int)SeqIdxConstants.IdxNanoFpdShiftBusy].Value = MySeq.stsNanoFPDShiftBusy;
                mySeqStatus[(int)SeqIdxConstants.IdxUnknownFpdPos].Value = (MySeq.stsNanoFPDShiftBusy || MySeq.stsNanoFPDBusy || MySeq.stsMicroFPDShiftBusy || MySeq.stsMicroFPDBusy || MySeq.stsMicroFPDPos || MySeq.stsNanoFPDPos || MySeq.stsMicroFPDShiftPos || MySeq.stsNanoFPDShiftPos);              //FPDX線位置(シフト含む)状態
            }
            //Rev23.10 追加 by長野 2015/09/20
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //if (!CTSettings.DetShiftOn && CTSettings.ChangeXrayOn)
            if (!(CTSettings.DetShiftOn || CTSettings.W_ScanOn) && CTSettings.ChangeXrayOn)
            {
                mySeqStatus[(int)SeqIdxConstants.IdxOKIIMove].Value = mod2ndXray.IsChangeXray() & mod2ndXray.IsAllCloseFrm;
                mySeqStatus[(int)SeqIdxConstants.IdxMicroFpdPos].Value = MySeq.stsMicroFPDPos;
                mySeqStatus[(int)SeqIdxConstants.IdxNanoFpdPos].Value = MySeq.stsNanoFPDPos;
                mySeqStatus[(int)SeqIdxConstants.IdxMicroFpdBusy].Value = MySeq.stsMicroFPDBusy;
                mySeqStatus[(int)SeqIdxConstants.IdxNanoFpdBusy].Value = MySeq.stsNanoFPDBusy;
                mySeqStatus[(int)SeqIdxConstants.IdxUnknownFpdPos].Value = (MySeq.stsNanoFPDBusy || MySeq.stsMicroFPDBusy || MySeq.stsMicroFPDPos || MySeq.stsNanoFPDPos);
            }

            //WU中:シーケンサ通達用
            mySeqStatus[(int)SeqIdxConstants.IdxWarmUpNow].Value =modHighSpeedCamera.IsXrayWarmUpNow();
            

            //運転準備
            mySeqStatus[(int)SeqIdxConstants.IdxRunReadySW].Value = MySeq.stsRunReadySW;
        
            //試料テーブル(大)用アタッチメント Rev26.00 by chouno 2017/03/13
            mySeqStatus[(int)SeqIdxConstants.IdxLargeTable].Value = MySeq.stsRotLargeTable;

            //modCTBusyにまとめる    'v17.60/v18.00移動 byやまおか 2011/03/21
            //'v17.20 原点復帰が完了していることが自動スキャン位置を使用できる条件とする。 by 長野 2010/09/20
            //If Not .stsXOrgReq And Not .stsYOrgReq And Not .stsIIOrgReq And Not .stsIIChgOrgReq Then
            //       'cmdFromTrans.Enabled = True
            //       '透視画像サイズが標準のときだけ有効にする    'v17.43変更 byやまおか 2011/01/27
            //       cmdFromTrans.Enabled = (frmTransImage.ZoomScale = 1)
            //       cmdFromSlice.Enabled = True
            //Else
            //       cmdFromTrans.Enabled = False
            //       cmdFromSlice.Enabled = False
            //End If
    
            //Rev25.00 Rev23.23の修正反映 by長野 2016/08/08
            if (ChangeSts == true)
            {
                //ステータス変更監視
                for (int i = mySeqStatus.GetLowerBound(0); i <= mySeqStatus.GetUpperBound(0); i++)
                {
                    if (mySeqStatus[i].Value != mySeqStatus[i].lastValue)
                    {
                        ChangeSeqStatus((SeqIdxConstants)i, mySeqStatus[i].Value);
                        mySeqStatus[i].lastValue = mySeqStatus[i].Value;
                    }
                }
            }
        }


        //*************************************************************************************************
        //機　　能： メカ関連更新処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        //Private Sub UpdateMecha() 'v19.12 明示的に呼べるようにするためpublicに変更 by長野 2013/03/08
        public void UpdateMecha()
        {
            float rotAng = 0;   //角度(0≦rotAng＜360)   'v15.10追加 byやまおか 2009/12/03

			CTstr.MECAINF mecainf = CTSettings.mecainf.Data;

            Application.DoEvents();

            //Rev23.10 計測CT対応 表示に使用する値を切り替える by長野 2015/10/16
            float dispUdab_pos = 0.0f;
            if (CTSettings.scaninh.Data.cm_mode == 0)
            {
                dispUdab_pos = mecainf.ud_linear_pos;
            }
            else
            {
                dispUdab_pos = mecainf.udab_pos;
            }
            myUdab_pos = dispUdab_pos;

            //テーブル回転位置
            //2014/11/07hata キャストの修正
            //ntbRotate.Value = Convert.ToDecimal(mecainf.rot_pos / 100);
            //ntbRotate.Value = Convert.ToDecimal(mecainf.rot_pos / 100F);
            ntbRotate.Value = Convert.ToDecimal(mecainf.rot_pos / (float)modMechaControl.GVal_Rot_SeqMagnify); //Rev23.10 変更 by長野 2015/09/18
 
            //テーブル昇降位置
            //if (0 > Convert.ToDecimal(mecainf.udab_pos))
            //Rev23.10 計測CT対応 by長野 2015/10/16
            if (0 > Convert.ToDecimal(dispUdab_pos))
            {
                Debug.Print("ud_ga_minus\n");
            }

            //Rev21.00 cwnePosと同様、3桁に丸める by長野 2015/03/11
            //ntbUpDown.Value = Convert.ToDecimal(mecainf.udab_pos);
            //ntbUpDown.Value = Convert.ToDecimal(Math.Round(mecainf.udab_pos, 3, MidpointRounding.AwayFromZero));
            //Rev23.10 計測CT対応 by長野 2015/10/16
            ntbUpDown.Value = Convert.ToDecimal(Math.Round(dispUdab_pos, 3, MidpointRounding.AwayFromZero));
        
            //テーブル昇降リミットならボタン背景色を変える
            //v17.20　上下限のリミットを追加したのでコメントアウト by 長野 2010/09/21
            //SetLimitStatus cwbtnUpDown(0), "Down", (.udab_pos > GValUpperLimit) And (.ud_limit = 1) 'v15.10追加 byやまおか 2009/11/02
            //SetLimitStatus cwbtnUpDown(1), "Up", (.udab_pos < GValLowerLimit) And (.ud_limit = 1)   'v15.10追加 byやまおか 2009/11/02

            //変更2014/10/07hata_v19.51反映
            //v19.51 X線と検出器が昇降する場合は、逆にする by長野 2014/02/24
            //SetLimitStatus(cwbtnUpDown[0], "Down", (mecainf.udab_pos > CTSettings.GValUpperLimit) && (mecainf.ud_l_limit == 1)); //v17.20 ud_limiti⇒ud_l_limitに変更 by長野 2010/09/21
            ////SetLimitStatus(cwbtnUpDown[1], "Up", (mecainf.udab_pos < CTSettings.GValLowerLimit) && (mecainf.ud_u_limit == 1));   //v17.20 ud_limiti⇒ud_u_limitに変更 by長野 2010/09/21
            //if ((CTSettings.t20kinf.Data.ud_type == 0))
            //{
            //    SetLimitStatus(cwbtnUpDown[0], "Down", (mecainf.udab_pos > CTSettings.GValUpperLimit) & (mecainf.ud_l_limit == 1));			//v17.20 ud_limiti⇒ud_l_limitに変更 by長野 2010/09/21
            //    SetLimitStatus(cwbtnUpDown[1], "Up", (mecainf.udab_pos < CTSettings.GValLowerLimit) & (mecainf.ud_u_limit == 1));			//v17.20 ud_limiti⇒ud_u_limitに変更 by長野 2010/09/21
            //}
            //else
            //{
            //    SetLimitStatus(cwbtnUpDown[0], "Down", (mecainf.udab_pos < CTSettings.GValLowerLimit) & (mecainf.ud_l_limit == 1));			//v17.20 ud_limiti⇒ud_l_limitに変更 by長野 2010/09/21
            //    SetLimitStatus(cwbtnUpDown[1], "Up", (mecainf.udab_pos > CTSettings.GValUpperLimit) & (mecainf.ud_u_limit == 1));			//v17.20 ud_limiti⇒ud_u_limitに変更 by長野 2010/09/21
            //}
            //Rev23.10 計測CT対応 by長野 2015/10/16
            //Rev23.40 by長野 2016/04/05 //Rev23.21 昇降上下限値は見ないようにする by稲葉 2016/03/02
            //if ((CTSettings.t20kinf.Data.ud_type == 0))
            //{
            //    SetLimitStatus(cwbtnUpDown[0], "Down", (dispUdab_pos > CTSettings.GValUpperLimit) & (mecainf.ud_l_limit == 1));			//v17.20 ud_limiti⇒ud_l_limitに変更 by長野 2010/09/21
            //    SetLimitStatus(cwbtnUpDown[1], "Up", (dispUdab_pos < CTSettings.GValLowerLimit) & (mecainf.ud_u_limit == 1));			//v17.20 ud_limiti⇒ud_u_limitに変更 by長野 2010/09/21
            //}
            //else
            //{
            //    SetLimitStatus(cwbtnUpDown[0], "Down", (dispUdab_pos < CTSettings.GValLowerLimit) & (mecainf.ud_l_limit == 1));			//v17.20 ud_limiti⇒ud_l_limitに変更 by長野 2010/09/21
            //    SetLimitStatus(cwbtnUpDown[1], "Up", (dispUdab_pos > CTSettings.GValUpperLimit) & (mecainf.ud_u_limit == 1));			//v17.20 ud_limiti⇒ud_u_limitに変更 by長野 2010/09/21
            //}
            SetLimitStatus(cwbtnUpDown[0], "Down", mecainf.ud_l_limit == 1);
            SetLimitStatus(cwbtnUpDown[1], "Up", (!modSeqComm.MySeq.stsMechaPermit) | (mecainf.ud_u_limit == 1));

            //昇降位置指定用コントロールにも値を反映させる。ただしアクティブの時は更新しない。
            //変更2014/12/22hata_ｽﾗｲﾀﾞを1つにする
            //if (!(cwnePos == this.ActiveControl) &&
            //    !(cwsldUpDown[0] == this.ActiveControl || cwsldUpDown[1] == this.ActiveControl) &&
            //    !(cmdPosExec == this.ActiveControl) &&
            //    !(cmdPosExec.BackColor == Color.Lime))
            if (!(cwnePos == this.ActiveControl) &&
                !(cwsldUpDown[0] == this.ActiveControl) &&
                !(cmdPosExec == this.ActiveControl) &&
                !(cmdPosExec.BackColor == Color.Lime) && !(txtUpDownPos == this.ActiveControl))
            {
 
                //Min/Maxを超えないようにする     //追加2014/04/01(検S1)hata
                //if ((float)cwnePos.Maximum <= mecainf.udab_pos)
                //Rev23.10 計測CT対応 by長野 2015/10/16               
                if ((float)cwnePos.Maximum <= dispUdab_pos)
                {
                    cwnePos.Value = cwnePos.Maximum;
                }
                //Rev23.10 計測CT対応 by長野 2015/10/16
                //else if (((float)cwnePos.Minimum >= mecainf.udab_pos))
                else if (((float)cwnePos.Minimum >= dispUdab_pos))
                {
                    cwnePos.Value = cwnePos.Minimum;
                }
                else
                {
                     //変更2015/01/08hata_小数点4桁以下は四捨五入
                    //cwnePos.Value = Convert.ToDecimal(mecainf.udab_pos);       //元に戻す   2011/03/25
                    //cwnePos.Value = Convert.ToDecimal(Math.Round(mecainf.udab_pos, 3, MidpointRounding.AwayFromZero));       //元に戻す   2011/03/25
                    //Rev23.10 計測CT対応 by長野 2015/10/16                    
                    cwnePos.Value = Convert.ToDecimal(Math.Round(dispUdab_pos, 3, MidpointRounding.AwayFromZero));       //元に戻す   2011/03/25
                }

                //値が変更した時だけ設定する         'v17.51条件追加 by 間々田 2011/03/24 Windows7におけるちらつき防止対策
                //If cwnePos.Value <> .udab_pos Then
                //cwnePos.Value = Convert.ToDecimal(mecainf.udab_pos);       //元に戻す   2011/03/25
                //End If
 
            }
 
            //微調テーブル位置
            ntbFTablePosX.Value = Convert.ToDecimal(mecainf.ystg_pos);   //微調テーブルX位置(mm)
            ntbFTablePosY.Value = Convert.ToDecimal(mecainf.xstg_pos);   //微調テーブルY位置(mm)

            //Rev22.00 回転傾斜テーブル 回転位置 by長野 2015/08/20
            ntbTilt.Value = Convert.ToDecimal(mecainf.tilt_pos);
            //Rev22.00 回転傾斜テーブル 傾斜位置 by長野 2015/08/20
            ntbTiltRot.Value = Convert.ToDecimal(mecainf.tiltrot_pos);

            //v15.10追加(ここから) byやまおか 2009/12/03
            //rotAng = (Convert.ToSingle(mecainf.rot_pos) / 100) % 360;        //-360＜rotAng＜360
            rotAng = (Convert.ToSingle(mecainf.rot_pos) / (float)modMechaControl.GVal_Rot_SeqMagnify) % 360;//-360＜rotAng＜360 //Rev23.10 変更 by長野 2015/09/18
            rotAng = (rotAng >= 0) ? rotAng : 360 + rotAng;                             //0≦rotAng＜360
        
            //ソフトで微調の上限下限リミットを作る
            bool xstg_Ulimit = false;
            bool xstg_Llimit = false;
            bool ystg_Ulimit = false;
            bool ystg_Llimit = false;
        
            //xstg_Ulimit = (.xstg_limit = 1) And (.xstg_pos >= 0)
            //xstg_Llimit = (.xstg_limit = 1) And (.xstg_pos < 0)
            //ystg_Ulimit = (.ystg_limit = 1) And (.ystg_pos >= 0)
            //ystg_Llimit = (.ystg_limit = 1) And (.ystg_pos < 0)
            //
            //'v17.20 透視画面と試料テーブルで左右をそろえるため、FPDの場合はステータスを反転させる。検出器位置不定の場合はI.I.とする。 by 長野 2010/09/20
            //If SecondDetOn And Use_FlatPanel And IsOKDetPos() Then
            //    xstg_Llimit = (.xstg_limit = 1) And (.xstg_pos >= 0)
            //    xstg_Ulimit = (.xstg_limit = 1) And (.xstg_pos < 0)
            //    ystg_Llimit = (.ystg_limit = 1) And (.ystg_pos >= 0)
            //    ystg_Ulimit = (.ystg_limit = 1) And (.ystg_pos < 0)
            //Else
            //    xstg_Ulimit = (.xstg_limit = 1) And (.xstg_pos >= 0)
            //    xstg_Llimit = (.xstg_limit = 1) And (.xstg_pos < 0)
            //    ystg_Ulimit = (.ystg_limit = 1) And (.ystg_pos >= 0)
            //    ystg_Llimit = (.ystg_limit = 1) And (.ystg_pos < 0)
            //End If
        
            //v17.47/v17.53変更 上下限動作限専用のコモン追加による 2011/03/09 by 間々田
            xstg_Ulimit = (mecainf.xstg_u_limit == 1);
            xstg_Llimit = (mecainf.xstg_l_limit == 1);
            ystg_Ulimit = (mecainf.ystg_u_limit == 1);
            ystg_Llimit = (mecainf.ystg_l_limit == 1);
        
            //Rev26.00 ラージテーブルの着脱有無を優先 by chouno 2017/10/16
            //if (modSeqComm.MySeq.stsRotLargeTable == false)
            //Rev26.20 微調テーブルタイプも見る by chouno 2019/02/06
            if (modSeqComm.MySeq.stsRotLargeTable == false || CTSettings.t20kinf.Data.ftable_type == 1)
            {
                //微調リミットならボタン背景色を変える
                if ((rotAng >= 0) && (rotAng < 90))
                {
                    SetLimitStatus(cwbtnFineTable[1], "Right", (xstg_Ulimit || ystg_Ulimit));      //画面右ボタン
                    SetLimitStatus(cwbtnFineTable[0], "Left", (xstg_Llimit || ystg_Llimit));       //画面左ボタン
                }
                else if ((rotAng >= 90) && (rotAng < 180))
                {
                    SetLimitStatus(cwbtnFineTable[1], "Right", (xstg_Ulimit || ystg_Llimit));      //画面右ボタン
                    SetLimitStatus(cwbtnFineTable[0], "Left", (xstg_Llimit || ystg_Ulimit));       //画面左ボタン
                }
                else if ((rotAng >= 180) && (rotAng < 270))
                {
                    SetLimitStatus(cwbtnFineTable[1], "Right", (xstg_Llimit || ystg_Llimit));      //画面右ボタン
                    SetLimitStatus(cwbtnFineTable[0], "Left", (xstg_Ulimit || ystg_Ulimit));       //画面左ボタン
                }
                else if ((rotAng >= 270) && (rotAng < 360))
                {
                    SetLimitStatus(cwbtnFineTable[1], "Right", (xstg_Llimit || ystg_Ulimit));      //画面右ボタン
                    SetLimitStatus(cwbtnFineTable[0], "Left", (xstg_Ulimit || ystg_Llimit));       //画面左ボタン
                }
            }
        
//'        rotAng = (CSng(.rot_pos) / 100) Mod 360             '-360＜rotAng＜360
//'        rotAng = IIf(rotAng >= 0, rotAng, 360 + rotAng)     '0≦rotAng＜360
//'
//'        '315≦rotAng＜45の範囲では、xstgに着目
//'        If ((rotAng >= 0) And (rotAng < 45)) Or ((rotAng >= 315) And (rotAng < 360)) Then
//'            SetLimitStatus cwbtnFineTable(1), "Right", ((.xstg_pos > 0) And (.xstg_limit = 1)) Or ((.ystg_pos > 0) And (.ystg_limit = 1))  '画面右ボタン
//'            SetLimitStatus cwbtnFineTable(0), "Left", (.xstg_pos < 0) And (.xstg_limit = 1)     '画面左ボタン
//'
//'        '45≦rotAng＜135の範囲では、ystgに着目
//'        ElseIf ((rotAng >= 45) And (rotAng < 135)) Then
//'            SetLimitStatus cwbtnFineTable(1), "Right", (.ystg_pos > 0) And (.ystg_limit = 1)    '画面右ボタン
//'            SetLimitStatus cwbtnFineTable(0), "Left", (.ystg_pos < 0) And (.ystg_limit = 1)     '画面左ボタン
//'
//'        '135≦rotAng＜225の範囲では、xstgに着目
//'        ElseIf ((rotAng >= 135) And (rotAng < 225)) Then
//'            SetLimitStatus cwbtnFineTable(1), "Right", (.xstg_pos < 0) And (.xstg_limit = 1)    '画面右ボタン
//'            SetLimitStatus cwbtnFineTable(0), "Left", (.xstg_pos > 0) And (.xstg_limit = 1)     '画面左ボタン
//'
//'        '225≦rotAng＜315の範囲では、ystgに着目
//'        ElseIf ((rotAng >= 225) And (rotAng < 315)) Then
//'            SetLimitStatus cwbtnFineTable(1), "Right", (.ystg_pos < 0) And (.ystg_limit = 1)    '画面右ボタン
//'            SetLimitStatus cwbtnFineTable(0), "Left", (.ystg_pos > 0) And (.ystg_limit = 1)     '画面左ボタン
//'        End If

            //v15.10追加(ここまで) byやまおか 2009/12/03
            
            //Rev22.00 回転傾斜用に追加 by長野 2015/09/04
            if (mecainf.tilt_l_limit == 1)
            {
                cwbtnTiltAndRot_Tilt[0].BackColor = Color.Cyan;
            }
            else
            {
                cwbtnTiltAndRot_Tilt[0].BackColor = System.Drawing.SystemColors.Control;
            }
            if (mecainf.tilt_u_limit == 1)
            {
                cwbtnTiltAndRot_Tilt[1].BackColor = Color.Cyan;
            }
            else
            {
                cwbtnTiltAndRot_Tilt[1].BackColor = System.Drawing.SystemColors.Control;
            }


            //シーケンサにステータスを送るための処理
            lblMechaStatus[0].Text = mecainf.phm_onoff.ToString();       //ファントム有無
            lblMechaStatus[1].Text = mecainf.rot_error.ToString();       //テーブル回転エラー
            lblMechaStatus[2].Text = mecainf.ud_error.ToString();        //テーブル昇降エラー
            lblMechaStatus[3].Text = mecainf.phm_error.ToString();       //ファントムエラー
            lblMechaStatus[4].Text = mecainf.xstg_error.ToString();      //微調Ｘ軸エラー
            lblMechaStatus[5].Text = mecainf.ystg_error.ToString();      //微調Ｙ軸エラー    
            lblMechaStatus[6].Text = mecainf.tilt_error.ToString();      //Rev22.00 回転傾斜テーブル by長野 2015/08/20
            lblMechaStatus[7].Text = mecainf.tiltrot_error.ToString();   //Rev22.00 回転傾斜テーブル by長野 2015/08/20
            //追加2014/10/07hata_v19.51反映
            //スキャンエリアの更新   'v18.00追加 byやまおか 2011/07/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //if ((CTSettings.scaninh.Data.avmode == 0))
            //if ((CTSettings.scaninh.Data.avmode == 0) || (CTSettings.scaninh.Data.ExObsCamera == 0)) //Rev23.30 外観カメラと1画素サイズ表示のフラグはセットで扱う by長野 2016/02/05
            //Rev23.31 条件変更 by長野 2016/03/30
            if (((CTSettings.scaninh.Data.high_speed_camera == 1) && (CTSettings.scaninh.Data.multi_tube == 1) && (CTSettings.scaninh.Data.second_detector == 1)) || (CTSettings.scaninh.Data.avmode == 0) || (CTSettings.scaninh.Data.ExObsCamera == 0)) //Rev23.30 外観カメラと1画素サイズ表示のフラグはセットで扱う by長野 2016/02/05
            {
                UpdateScanarea();
            }
       
        }

        //*************************************************************************************************
        //機　　能： テーブル回転位置変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3 2006/02/20  (SI3)間々田   新規作成
        //*************************************************************************************************
        private void ntbRotate_ValueChanged(object sender, NumTextBox.ValueChangedEventArgs e)
        {    
			//modMecainf.mecainfType mecainf = CTSettings.mecainf.Data;
            CTstr.MECAINF mecainf = CTSettings.mecainf.Data;


            //シーケンサ（操作パネル）にも通知
            modSeqComm.SeqWordWrite("stsRotPosition", (mecainf.rot_pos).ToString(), false);

            //ctchkRotate(0).Value = (ntbRotate.Value = 0)
            //ctchkRotate(1).Value = (ntbRotate.Value = 90)
            //ctchkRotate(2).Value = (ntbRotate.Value = 180)
            //ctchkRotate(3).Value = (ntbRotate.Value = 270)
    
            //v17.51変更 by 間々田 2011/03/23 FPDの場合を考慮・mecainf.rot_posを処理対象とする
            //ctchkRotate[0].Value = (mecainf.rot_pos == 0 * 100) || (mecainf.rot_pos == 360 * 100) || (mecainf.rot_pos == -360 * 100);
            //ctchkRotate[1].Value = (mecainf.rot_pos == 90 * 100) || (mecainf.rot_pos == -270 * 100);
            //ctchkRotate[2].Value = (mecainf.rot_pos == 180 * 100) || (mecainf.rot_pos == -180 * 100);
            //ctchkRotate[3].Value = (mecainf.rot_pos == 270 * 100) || (mecainf.rot_pos == -90 * 100);
            //rev23.10 変更 by長野 2015/09/18 
            ctchkRotate[0].Value = (mecainf.rot_pos == (int)(0.0 * (float)modMechaControl.GVal_Rot_SeqMagnify)) || (mecainf.rot_pos == (int)(360.0 * (float)modMechaControl.GVal_Rot_SeqMagnify)) || (mecainf.rot_pos == (int)(-360.0 * (float)modMechaControl.GVal_Rot_SeqMagnify));
            ctchkRotate[1].Value = (mecainf.rot_pos == (int)(90.0 * (float)modMechaControl.GVal_Rot_SeqMagnify)) || (mecainf.rot_pos == (int)(-270.0 * (float)modMechaControl.GVal_Rot_SeqMagnify));
            ctchkRotate[2].Value = (mecainf.rot_pos == (int)(180.0 * (float)modMechaControl.GVal_Rot_SeqMagnify)) || (mecainf.rot_pos == (int)(-180.0 * (float)modMechaControl.GVal_Rot_SeqMagnify));
            ctchkRotate[3].Value = (mecainf.rot_pos == (int)(270.0 * (float)modMechaControl.GVal_Rot_SeqMagnify)) || (mecainf.rot_pos == (int)(-90.0 * (float)modMechaControl.GVal_Rot_SeqMagnify));

        }


        //*************************************************************************************************
        //機　　能： 昇降位置変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3 2006/02/20  (SI3)間々田   新規作成
        //*************************************************************************************************
        private void ntbUpDown_ValueChanged(object sender, NumTextBox.ValueChangedEventArgs e)
        {
            //シーケンサ（操作パネル）にも通知
            //modSeqComm.SeqWordWrite("stsUDPosition", Math.Floor(ntbUpDown.Value * 100).ToString(), false);

            modSeqComm.SeqWordWrite("stsUDPosition", Math.Floor(ntbUpDown.Value * (decimal)modMechaControl.GVal_Ud_SeqMagnify).ToString(), false); //Rev23.10 変更 by長野 2015/09/18 
            
            //Rev23.10 計測CTモードの場合は、リニアスケール値(実際は逆)
            if (CTSettings.scaninh.Data.cm_mode == 0)
            {
                modSeqComm.SeqWordWrite("stsLinearUDPosition", Math.Floor((decimal)CTSettings.mecainf.Data.ud_linear_pos * (decimal)modMechaControl.GVal_Ud_SeqMagnify).ToString(), false); //Rev23.10 変更 by長野 2015/09/18 
            }
            //スライダコントロールに反映
            //2014/11/07hata キャストの修正
            //cwsldUpDown[1].Value = (int)ntbUpDown.Value;
            //削除2014/12/22hata_ｽﾗｲﾀﾞを1つにする
            //cwsldUpDown[1].Value = Convert.ToInt32(ntbUpDown.Value);

            //追加2014/12/22hata_ｽﾗｲﾀﾞを1つにする
            //昇降高さ位置移動中でなければポインタ１側にも反映
            if (stsUpDownPos != ntbUpDown.Value)
            {
                stsUpDownPos = Convert.ToDecimal(ntbUpDown.Value);
                if (cmdPosExec.BackColor != Color.Lime)
                {
                    if (cwsldUpDown[0].Value != stsUpDownPos)
                        cwsldUpDown[0].Value = stsUpDownPos;
                }
            }

            //移動中のPointer位置を表示
            //変更2014/12/22hata_ｽﾗｲﾀﾞを1つにする
            DispUpDownPointer();
            ////float MoveSize = ((lineShape1.Parent.Bottom - lineShape1.Parent.Margin.Bottom) - (lineShape1.Parent.Top - lineShape1.Parent.Margin.Top));
            //int offset = 10;
            //float MoveSize = lineShape1.Parent.Height - (lineShape1.BorderWidth - 1) - 3 - offset * 2;
            ////2014/11/07hata キャストの修正
            ////int ypos = (int)(((float)ntbUpDown.Value / (cwsldUpDown[0].Maximum - cwsldUpDown[0].Minimum)) * MoveSize);
            //int ypos = Convert.ToInt32(((float)ntbUpDown.Value / (cwsldUpDown[0].Maximum - cwsldUpDown[0].Minimum)) * MoveSize);
            //int ypos1;
            //if (cwsldUpDown[0].Reverse)
            //{
            //    //TopがMinのとき    
            //    //2014/11/07hata キャストの修正
            //    //ypos1 = ypos + (lineShape1.BorderWidth - 1) / 2 + 1 + offset;
            //    ypos1 = ypos + Convert.ToInt32((lineShape1.BorderWidth - 1) / 2F) + 1 + offset;
            //    //ypos1 = 2 + ypos;
            //}
            //else
            //{
            //    //TopがMaxのとき    
            //    //2014/11/07hata キャストの修正
            //    //ypos1 = lineShape1.Parent.Height - ypos - (lineShape1.BorderWidth - 1) / 2 - 2 - offset;
            //    ypos1 = lineShape1.Parent.Height - ypos - Convert.ToInt32((lineShape1.BorderWidth - 1) / 2F) - 2 - offset;
            //    //ypos1 = 176 - ypos;
            //}
            //lineShape1.Y1 = ypos1;
            //lineShape1.Y2 = ypos1;
                
            
            //イベント生成
			if (UDPosChanged != null)
			{
				UDPosChanged(this, EventArgs.Empty);
			}
        }


        //*************************************************************************************************
        //機　　能： FCD変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        //'Private Sub ntbFCD_ValueChanged(ByVal PreviousValue As Variant)
        public void ntbFCD_ValueChanged(object sender, NumTextBox.ValueChangedEventArgs e)     //v17.60変更 Public化 byやまおか 2011/06/08
        {
            //グローバル変数にセット
            ScanCorrect.GVal_Fcd = (float)ntbFCD.Value;

            //オフセット込みのFCDを求める
            myFCDWithOffset = ScanCorrect.GVal_Fcd + CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()];
            //Rev23.10 追加 by長野 2015/10/27
            myFCDWithOffsetMecha = modSeqComm.MySeq.stsFCD / modMechaControl.GVal_FCD_SeqMagnify + CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()];
            myFCDWithOffsetLinear = modSeqComm.MySeq.stsLinearFCD / modMechaControl.GVal_FCD_SeqMagnify + CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()];
 
            //イベント発生
			if (FCDChanged != null)
			{
				FCDChanged(this, EventArgs.Empty);
			}
        }

        
        //*************************************************************************************************
        //機　　能： FID変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        //Private Sub ntbFID_ValueChanged(ByVal PreviousValue As Variant)
        public void ntbFID_ValueChanged(object sender, NumTextBox.ValueChangedEventArgs e)     //v17.60変更 Public化 byやまおか 2011/06/08
        {
            //変更2014/10/07hata_v19.51反映
            ////'グローバル変数にセット
            //ScanCorrect.GVal_Fid = (float)ntbFID.Value;

            ////'オフセット込みのFIDを求める
            //myFIDWithOffset = ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube];

            ////'追加 by 間々田 2009/07/21
            //decimal value = 0;
            //decimal.TryParse(lblIIMove[0].Text, out value);
            //ctchkIIMove[0].Value = (ntbFID.Value == value);

            //decimal.TryParse(lblIIMove[1].Text, out value);
            //ctchkIIMove[1].Value = (ntbFID.Value == value);

            //decimal.TryParse(lblIIMove[2].Text, out value);
            //ctchkIIMove[2].Value = (ntbFID.Value == value);

            //decimal.TryParse(lblIIMove[3].Text, out value);
            //ctchkIIMove[3].Value = (ntbFID.Value == value);

            //v18.00 関数化 byやまおか 2011/03/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            FidValueChange();
            
            //'イベント発生
			if (FIDChanged != null)
			{
				FIDChanged(this, EventArgs.Empty);
			}
        }


        //追加2014/10/07hata_v19.51反映
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        public void FidValueChange()
        {
            //グローバル変数にセット
            ScanCorrect.GVal_Fid = (float)ntbFID.Value;

            //オフセット込みのFIDを求める
            myFIDWithOffset = ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube];
            //Rev23.10 追加 by長野 2015/2015/10/27
            myFIDWithOffsetMecha = modSeqComm.MySeq.stsFID / modMechaControl.GVal_FDD_SeqMagnify + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube];
            myFIDWithOffsetLinear = modSeqComm.MySeq.stsLinearFDD / modMechaControl.GVal_FDD_SeqMagnify + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube];


            //追加 by 間々田 2009/07/21
            ////Rev23.10 計測CTの場合はstsFIDとの比較に変更する by長野 2015/10/21
            //if (CTSettings.scaninh.Data.cm_mode == 1)
            //{
            //    ctchkIIMove[0].Value = (ntbFID.Value == Convert.ToDecimal(lblIIMove[0].Text));
            //    ctchkIIMove[1].Value = (ntbFID.Value == Convert.ToDecimal(lblIIMove[1].Text));
            //    ctchkIIMove[2].Value = (ntbFID.Value == Convert.ToDecimal(lblIIMove[2].Text));
            //    ctchkIIMove[3].Value = (ntbFID.Value == Convert.ToDecimal(lblIIMove[3].Text));
            //}
            //else if (CTSettings.scaninh.Data.cm_mode == 0)
            //{
            //    ctchkIIMove[0].Value = (modSeqComm.MySeq.stsFID / modMechaControl.GVal_FDD_SeqMagnify == Convert.ToDecimal(lblIIMove[0].Text));
            //    ctchkIIMove[1].Value = (modSeqComm.MySeq.stsFID / modMechaControl.GVal_FDD_SeqMagnify == Convert.ToDecimal(lblIIMove[1].Text));
            //    ctchkIIMove[2].Value = (modSeqComm.MySeq.stsFID / modMechaControl.GVal_FDD_SeqMagnify == Convert.ToDecimal(lblIIMove[2].Text));
            //    ctchkIIMove[3].Value = (modSeqComm.MySeq.stsFID / modMechaControl.GVal_FDD_SeqMagnify == Convert.ToDecimal(lblIIMove[3].Text));
            //}
            //Rev23.10 元に戻す by長野 2015/11/03
            ctchkIIMove[0].Value = (ntbFID.Value == Convert.ToDecimal(lblIIMove[0].Text));
            ctchkIIMove[1].Value = (ntbFID.Value == Convert.ToDecimal(lblIIMove[1].Text));
            ctchkIIMove[2].Value = (ntbFID.Value == Convert.ToDecimal(lblIIMove[2].Text));
            ctchkIIMove[3].Value = (ntbFID.Value == Convert.ToDecimal(lblIIMove[3].Text));
            ctchkIIMove[4].Value = (ntbFID.Value == Convert.ToDecimal(lblIIMove[4].Text)); //Rev26.00 add by chouno 2017/03/03

        }

        //*************************************************************************************************
        //機　　能： テーブルＹ軸位置（従来のＸ軸）変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3 2006/02/20  (SI3)間々田   新規作成
        //*************************************************************************************************
        public void ntbTableXPos_ValueChanged(object sender, NumTextBox.ValueChangedEventArgs e) //Rev26.00 change private->public by chouno 2016/12/28
        {
            //イベント生成
			if (YChanged != null)
			{
				YChanged(this, EventArgs.Empty);
			}
        }


        //*************************************************************************************************
        //機　　能： 微調Ｘ軸位置変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        private void ntbFTablePosX_ValueChanged(object sender, NumTextBox.ValueChangedEventArgs e)
        {
            //微調Ｘ軸位置をシーケンサ（操作パネル）に通知
            modSeqComm.SeqWordWrite("stsXStgPosition", (ntbFTablePosX.Value * 100).ToString("0"), false);

            //イベント生成
			if (FXChanged != null)
			{
				FXChanged(this, EventArgs.Empty);
			}
        }


        //*************************************************************************************************
        //機　　能： 微調Ｙ軸位置変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        private void ntbFTablePosY_ValueChanged(object sender, NumTextBox.ValueChangedEventArgs e)
        {
            //微調Ｙ軸位置をシーケンサ（操作パネル）に通知
            modSeqComm.SeqWordWrite("stsYStgPosition", (ntbFTablePosY.Value * 100).ToString("0"), false);

            //イベント生成
			if (FYChanged != null)
			{
				FYChanged(this, EventArgs.Empty);
			}
        }

        //*************************************************************************************************
        //機　　能： 回転傾斜テーブル 傾斜変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v22.00  2015/08/20 (検S1)長野  新規作成
        //*************************************************************************************************
        private void ntbTilt_ValueChanged(object sender, NumTextBox.ValueChangedEventArgs e)
        {
            //傾斜位置をシーケンサ（操作パネル）に通知
            modSeqComm.SeqWordWrite("stsTiltPosition", (ntbTilt.Value * 100).ToString("0"), false);

        }
        //*************************************************************************************************
        //機　　能： 回転傾斜テーブル 回転変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v22.00  2015/08/20 (検S1)長野  新規作成
        //*************************************************************************************************
        private void ntbTiltRot_ValueChanged(object sender, NumTextBox.ValueChangedEventArgs e)
        {
            //回転位置をシーケンサ（操作パネル）に通知
            modSeqComm.SeqWordWrite("stsTiltRotPosition", (ntbTiltRot.Value * 100).ToString("0"), false);
        }

        //*************************************************************************************************
        //機　　能： 速度変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Value           [I/ ] Variant   変更後の速度
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //           v15.10  2009/11/09 やまおか     Public化
        //*************************************************************************************************
        public void mySpeedForm_ValueChanged(modMechaControl.MechaConstants theMecha, float theSpeed)
        {
            switch (theMecha)
            {
                case modMechaControl.MechaConstants.MechaTableRotate:
                    modMechaControl.GVal_TableRotateSpeed = theSpeed;                           //回転
                    break; 
                case modMechaControl.MechaConstants.MechaTableUpDown:  
                    modMechaControl.GVal_TableUpDownSpeed = theSpeed;                           //昇降                
                    break; 
                case modMechaControl.MechaConstants.MechaTableX:
                    modSeqComm.SeqWordWrite("YSpeed", (theSpeed * 10).ToString("0"));            //FCD（従来のＹ軸）                
                    break; 
                case modMechaControl.MechaConstants.MechaII:
                    modSeqComm.SeqWordWrite("IISpeed", (theSpeed * 10).ToString("0"));           //FDD                
                    break; 
                case modMechaControl.MechaConstants.MechaTableY:
                    modSeqComm.SeqWordWrite("XSpeed", (theSpeed * 10).ToString("0"));            //Ｙ軸（従来のＸ軸）
                    break; 
                case modMechaControl.MechaConstants.MechaFTableX:      
                    modMechaControl.GVal_FineTableSpeed = theSpeed;                             //微調テーブル                
                    break; 
                case modMechaControl.MechaConstants.MechaFTableY:      
                    modMechaControl.GVal_FineTableSpeed = theSpeed;                             //微調テーブル
                    break; 
                case modMechaControl.MechaConstants.MechaXrayRotate:
                    modSeqComm.SeqWordWrite("XrayRotSpeed", (theSpeed * 10000).ToString("0"));   //Ｘ線管回転速度
                    break; 
                case modMechaControl.MechaConstants.MechaXrayX:
                    modSeqComm.SeqWordWrite("XrayXSpeed", (theSpeed * 100).ToString("0"));       //Ｘ線管Ｘ軸
                    break; 
                case modMechaControl.MechaConstants.MechaXrayY:
                    modSeqComm.SeqWordWrite("XrayYSpeed", (theSpeed * 100).ToString("0"));       //Ｘ線管Ｙ軸
                    break;
                case modMechaControl.MechaConstants.MechaTiltAndRot_Rot:
                    modMechaControl.GVal_TiltAndRot_RotSpeed = theSpeed;                         //回転傾斜テーブル 回転    //Rev22.00 追加 by長野 2015/08/20
                    break;
                case modMechaControl.MechaConstants.MechaTiltAndRot_Tilt:
                    modMechaControl.GVal_TiltAdnRot_TiltSpeed = (theSpeed * (float)(1.0 /6.0));                        //回転傾斜テーブル 傾斜    //Rev22.00 追加 by長野 2015/08/20      
                    break; 
            }
        }

        
        //*************************************************************************************************
        //機　　能： 速度指定コンボボックス選択時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //'*************************************************************************************************
        public void cboSpeed_SelectedIndexChanged(object sender, EventArgs e)   //TODO cboSpeed配列作成 GetToolTip
        {
            if (sender as ComboBox == null) return;

            int Index = Array.IndexOf(cboSpeed, sender);

			if (Index < 0) return;

            float theSpeed = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
            Form theForm = null;    //v15.10追加 byやまおか 2009/11/09
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//modMechapara.MechaparaType mechapara = CTSettings.mechapara.Data;
            CTstr.MECHAPARA mechapara = CTSettings.mechapara.Data;

            //コモンに設定されている速度にする
            switch (Index)
            {
                case (int)modMechaControl.MechaConstants.MechaTableRotate:  
                    theSpeed = mechapara.rot_speed[cboSpeed[Index].SelectedIndex];         //回転
                    break; 
                case (int)modMechaControl.MechaConstants.MechaTableUpDown:  
                    theSpeed = mechapara.ud_speed[cboSpeed[Index].SelectedIndex];          //昇降
                    break; 
                case (int)modMechaControl.MechaConstants.MechaTableX:       
                    theSpeed = mechapara.fcd_speed[cboSpeed[Index].SelectedIndex];         //FCD（従来のＹ軸）
                    break; 
                case (int)modMechaControl.MechaConstants.MechaII:           
                    theSpeed = mechapara.fdd_speed[cboSpeed[Index].SelectedIndex];         //FDD
                    break; 
                case (int)modMechaControl.MechaConstants.MechaTableY:       
                    theSpeed = mechapara.tbl_y_speed[cboSpeed[Index].SelectedIndex];       //Ｙ軸（従来のＸ軸）
                    break; 
                case (int)modMechaControl.MechaConstants.MechaFTableX:      
                    theSpeed = mechapara.fine_tbl_speed[cboSpeed[Index].SelectedIndex];    //微調テーブル
                    break; 
                case (int)modMechaControl.MechaConstants.MechaXrayRotate:   
                    theSpeed = mechapara.xray_rot_speed[cboSpeed[Index].SelectedIndex];    //Ｘ線管回転速度
                    break; 
                case (int)modMechaControl.MechaConstants.MechaXrayX:        
                    theSpeed = mechapara.xray_x_speed[cboSpeed[Index].SelectedIndex];      //Ｘ線管Ｘ軸
                    break; 
                case (int)modMechaControl.MechaConstants.MechaXrayY:        
                    theSpeed = mechapara.xray_y_speed[cboSpeed[Index].SelectedIndex];      //Ｘ線管Ｙ軸
                    break;
                case (int)modMechaControl.MechaConstants.MechaTiltAndRot_Rot: //Rev22.00 回転傾斜テーブル 追加 by長野 2015/08/21
                    theSpeed = mechapara.tilt_and_rot_rot_speed[cboSpeed[Index].SelectedIndex];      //チルト回転テーブル(回転)
                    break;
                case (int)modMechaControl.MechaConstants.MechaTiltAndRot_Tilt: //Rev22.00 回転傾斜テーブル 追加 by長野 2015/08/21
                    theSpeed = mechapara.tilt_and_rot_tilt_speed[cboSpeed[Index].SelectedIndex] * (float)6.0;      //チルト回転テーブル(チルト)
                    break;
            }
        
            mySpeedForm_ValueChanged((modMechaControl.MechaConstants)Index, theSpeed);

			string toolTipText = toolTip.GetToolTip(cboSpeed[Index]);
        
            //frmSpeedダイアログが既に表示されていたら   'v15.10追加 byやまおか 2009/11/09
            foreach (Form theForm in Application.OpenForms)
            {
                if((theForm.Text == toolTipText) && (theForm.Visible))
                {
                    //そのダイアログをフォーカス
                    theForm.Focus();
                    //Manual以外が選択されたときはそのダイアログを閉じる
                    if(cboSpeed[Index].SelectedIndex != (int)SpeedConstants.speedmanual) theForm.Close();
                    //ここで抜ける
                    return;
                }                
            }
        
            //Manualが選択されたら，手動設定のウィンドウを表示
            if ((cboSpeed[Index].SelectedIndex == (int)SpeedConstants.speedmanual) && (this.ActiveControl == cboSpeed[Index]))
            {
                if (mySpeedForm != null)
                {
                    mySpeedForm = null;
                }
            
                mySpeedForm = new frmSpeed();

				//modScancondpar.scancondparType scancondpar = CTSettings.scancondpar.Data;
                CTstr.SCANCONDPAR scancondpar = CTSettings.scancondpar.Data;

#if (!DebugOn)
                Seq MySeq = modSeqComm.MySeq;
#else
                frmVirtualSeqComm MySeq = modSeqComm.MySeq;
#endif  

                switch (Index)
                {                
                    //回転
                    case (int)modMechaControl.MechaConstants.MechaTableRotate:
                        mySpeedForm.Dialog((modMechaControl.MechaConstants)Index, toolTipText, theSpeed, 0.1, scancondpar.rot_max_speed, "rpm");
                        break; 
                    //昇降
                    case (int)modMechaControl.MechaConstants.MechaTableUpDown:
                        mySpeedForm.Dialog((modMechaControl.MechaConstants)Index, toolTipText, theSpeed, 0.1, scancondpar.ud_max_speed);
                        break; 
                    //FCD（従来のＹ軸）
                    case (int)modMechaControl.MechaConstants.MechaTableX:
                        //2014/11/07hata キャストの修正
                        //mySpeedForm.Dialog((modMechaControl.MechaConstants)Index, toolTipText, theSpeed, MySeq.stsYMinSpeed / 10, MySeq.stsYMaxSpeed / 10);
                        mySpeedForm.Dialog((modMechaControl.MechaConstants)Index, toolTipText, theSpeed, MySeq.stsYMinSpeed / 10D, MySeq.stsYMaxSpeed / 10D);
                        
                        break; 
                    //FDD
                    case (int)modMechaControl.MechaConstants.MechaII:
                        //2014/11/07hata キャストの修正
                        //mySpeedForm.Dialog((modMechaControl.MechaConstants)Index, toolTipText, theSpeed, MySeq.stsIIMinSpeed / 10, MySeq.stsIIMaxSpeed / 10);
                        mySpeedForm.Dialog((modMechaControl.MechaConstants)Index, toolTipText, theSpeed, MySeq.stsIIMinSpeed / 10D, MySeq.stsIIMaxSpeed / 10D);
                        
                        break; 
                    //Ｙ軸（従来のＸ軸）
                    case (int)modMechaControl.MechaConstants.MechaTableY:
                        //2014/11/07hata キャストの修正
                        //mySpeedForm.Dialog((modMechaControl.MechaConstants)Index, toolTipText, theSpeed, MySeq.stsXMinSpeed / 10, MySeq.stsXMaxSpeed / 10);
                        mySpeedForm.Dialog((modMechaControl.MechaConstants)Index, toolTipText, theSpeed, MySeq.stsXMinSpeed / 10D, MySeq.stsXMaxSpeed / 10D);
                        
                        break;
                    //微調テーブル
                    case (int)modMechaControl.MechaConstants.MechaFTableX:
                        mySpeedForm.Dialog((modMechaControl.MechaConstants)Index, toolTipText, theSpeed, 0.1, scancondpar.ftable_max_speed);
                        
                        break;
                    //Ｘ線管回転
                    case (int)modMechaControl.MechaConstants.MechaXrayRotate:
                        //2014/11/07hata キャストの修正
                        //mySpeedForm.Dialog((modMechaControl.MechaConstants)Index, toolTipText, theSpeed, 0.1, MySeq.stsXrayRotMaxSp / 10000, "rpm");
                        mySpeedForm.Dialog((modMechaControl.MechaConstants)Index, toolTipText, theSpeed, 0.1, MySeq.stsXrayRotMaxSp / 10000D, "rpm");
                        
                        break; 
                    //Ｘ線管Ｘ軸
                    case (int)modMechaControl.MechaConstants.MechaXrayX:
                        //2014/11/07hata キャストの修正
                        //mySpeedForm.Dialog((modMechaControl.MechaConstants)Index, toolTipText, theSpeed, MySeq.stsXrayXMinSp / 100, MySeq.stsXrayXMaxSp / 100);
                        mySpeedForm.Dialog((modMechaControl.MechaConstants)Index, toolTipText, theSpeed, MySeq.stsXrayXMinSp / 100D, MySeq.stsXrayXMaxSp / 100D);
                        
                        break; 
                    //Ｘ線管Ｙ軸
                    case (int)modMechaControl.MechaConstants.MechaXrayY:
                        //2014/11/07hata キャストの修正
                        //mySpeedForm.Dialog((modMechaControl.MechaConstants)Index, toolTipText, theSpeed, MySeq.stsXrayYMinSp / 100, MySeq.stsXrayYMaxSp / 100);
                        mySpeedForm.Dialog((modMechaControl.MechaConstants)Index, toolTipText, theSpeed, MySeq.stsXrayYMinSp / 100D, MySeq.stsXrayYMaxSp / 100D);
                        
                        break;

                    //回転傾斜テーブル(回転) //Rev22.00 追加 by長野 2015/08/21
                    case (int)modMechaControl.MechaConstants.MechaTiltAndRot_Rot:
                        mySpeedForm.Dialog((modMechaControl.MechaConstants)Index, toolTipText, theSpeed, 0.1, scancondpar.tilt_and_rot_r_max_speed, "rpm");
                        break;

                    //回転傾斜テーブル(傾斜) //Rev22.00 追加 by長野 2015/08/21
                    case (int)modMechaControl.MechaConstants.MechaTiltAndRot_Tilt:
                        mySpeedForm.Dialog((modMechaControl.MechaConstants)Index, toolTipText, theSpeed, 0.01*6.0, scancondpar.tilt_and_rot_t_max_speed*6.0, "deg/s");
                        break; 

                }            
            }
            else
            {
            }
        }


        //*************************************************************************************************
        //機　　能： 昇降位置指定スライダ変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        //private void cwsldUpDown_ValueChanged(object sender, EventArgs e)
        //{
        //    if (sender as TrackBar == null) return;

        //    int Pointer = Array.IndexOf(cwsldUpDown, sender);

        //    if (Pointer < 0) return;

        //    //指定高さ位置欄に反映
        //    switch (Pointer)
        //    {
        //        case 0:
        //            if (this.ActiveControl == cwsldUpDown[0])
        //            {
        //                ////cwsldUpDown.ActivePointer.Color = vbGreen                     
        //                //追加2014/05hata
        //                cwnePos.Value = (decimal)cwsldUpDown[0].Value;
        //                //cwnePos.Value = (decimal)(cwsldUpDown[0].Maximum - cwsldUpDown[0].Value);
        //            }
        //            break;
        //        case 1:
        //            //昇降高さ位置移動中でなければポインタ１側にも反映
        //            if (cmdPosExec.BackColor != Color.Lime)
        //            {
        //                //追加2014/05hata
        //                cwsldUpDown[0].Value = cwsldUpDown[1].Value;
                        
        //                //ctSliderV1.Value = cwsldUpDown[1].Value;
                    
        //            }
        //            break;
        //    }
        //}
        private void cwsldUpDown_ValueChanged(object sender, EventArgs e)
        {
            //変更2015/02/12hata_スライダー新規
            //if (sender as CTSliderTrack == null) return;
            if (sender as CTSliderVScroll == null) return;

            int Pointer = Array.IndexOf(cwsldUpDown, sender);

            if (Pointer < 0) return;

            //指定高さ位置欄に反映
            switch (Pointer)
            {
                case 0:
                    if (this.ActiveControl == cwsldUpDown[0])
                    {
                        ////cwsldUpDown.ActivePointer.Color = vbGreen
                        //変更2015/02/02hata_Max/Min範囲のチェック
                        //cwnePos.Value = (decimal)cwsldUpDown[0].Value;
                        cwnePos.Value = modLibrary.CorrectInRange((decimal)cwsldUpDown[0].Value, cwnePos.Minimum, cwnePos.Maximum);
                    }
                    break;

                //削除2014/12/22hata_ｽﾗｲﾀﾞを1つにする
                //case 1:
                //    //昇降高さ位置移動中でなければポインタ１側にも反映
                //    if (cmdPosExec.BackColor != Color.Lime)
                //    {
                //        cwsldUpDown[0].Value = cwsldUpDown[1].Value;
                //    }
                //    break;
            }
        }

        //'*************************************************************************************************
        //'機　　能： 昇降位置指定スライダ変更終了時処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*************************************************************************************************
        //Private Sub cwsldUpDown_PointerValueCommitted(ByVal Pointer As Long, Value As Variant)
        //
        //    '指定高さ位置欄に反映
        //    If Pointer = 1 Then
        //        If Me.ActiveControl Is cwsldUpDown Then
        //            cmdPosExec_Click
        //        End If
        //    End If
        //
        //End Sub
        
        //*************************************************************************************************
        //機　　能： 昇降位置指定スライダ変更終了時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： PointerValueCommittedイベントが生じないことがあるので
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        //private void cwsldUpDown_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if(cwsldUpDown[0].Value != cwsldUpDown[1].Value)     
        //    {
        //        cmdPosExec_Click(cmdPosExec, EventArgs.Empty);
        //    }
        //}
        private void cwsldUpDown_MouseCaptureChanged(object sender, EventArgs e)
        {
            //変更2014/12/22hata_ｽﾗｲﾀﾞを1つにする
            //if (cwsldUpDown[0].Value != cwsldUpDown[1].Value)
            if (cwsldUpDown[0].Value != ntbUpDown.Value)
            {
                if (!cwsldUpDown[0].Capture)    //追加2015/02/12hata_if文追加　スライダー新規
                    cmdPosExec_Click(cmdPosExec, EventArgs.Empty);
            }
        }
        //*************************************************************************************************
        //機　　能： 指定高さ位置欄変更終了時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        //private void cwnePos_ValueChanged(object sender, EventArgs e)   //TODO 対応するコントロールを確認　Indexを 0-base に変更
        //Rev20.00 publicへ変更 by長野 2015/03/06
        public void cwnePos_ValueChanged(object sender, EventArgs e)   //TODO 対応するコントロールを確認　Indexを 0-base に変更
        {
            //2014/11/07hata キャストの修正
            //cwsldUpDown[0].Value = (int)cwnePos.Value;

            //変更2014/12/22hata_ｽﾗｲﾀﾞを1つにする           
            //cwsldUpDown[0].Value = Convert.ToInt32(cwnePos.Value);
            //インクリメント値で増減させる
            decimal rem = cwnePos.Value % cwnePos.Increment;
            
            //Rev26.10 udstepが0.001未満の場合は調整せずに、そのままcwnePosへ代入する by chouno 2018/01/13
            if (cwnePos.Increment < (decimal)0.001)
            {
                cwnePos.Value = modLibrary.CorrectInRange(cwnePos.Value, cwnePos.Minimum, cwnePos.Maximum);
            }
            else
            {
                if (rem > 0)
                {
                    //変更2015/02/02hata_Max/Min範囲のチェック
                    //cwnePos.Value = cwnePos.Value - rem + cwnePos.Increment;
                    cwnePos.Value = modLibrary.CorrectInRange(cwnePos.Value - rem + cwnePos.Increment, cwnePos.Minimum, cwnePos.Maximum);
                    //Rev21.00 incrementが半端な数値の場合も対応する by長野 2015/03/13
                    //return;
                }
            }
          
            if ((modCT30K.CT30KSetup) & (CTSettings.mecainf.Data.ud_busy != 1) & (cmdPosExec.BackColor != Color.Lime))
            {   
                //変更2015/01/30hata
                //if (ntbUpDown.Value != cwsldUpDown[0].Value)
                if (ntbUpDown.Value != cwnePos.Value)
                {
                    //変更2015/02/12hata_スライダー新規
                    //picPointer.Visible = true;
                    cwsldUpDown[0].ArrowVisible = true;

                }
                else
                {
                    //変更2015/02/12hata_スライダー新規
                    //picPointer.Visible = false;
                    cwsldUpDown[0].ArrowVisible = false;
                }
            }

            txtUpDownPos.Text = cwnePos.Value.ToString("0.000");

            cwsldUpDown[0].Value = cwnePos.Value;

        }

        //*************************************************************************************************
        //機　　能： 位置指定タブ：「実行」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void cmdPosExec_Click(object sender, EventArgs e)
        {
            
            
            int error_sts = 0;

             //追加2014/10/07hata_v19.51反映
            if (!modMechaControl.IsOkMechaMove())
                goto ExitHandler;            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //エラー時の扱い
            try 
	        {	        
                //高さ動作チェック
                if (IsOkUpDown((float)cwnePos.Value))
                {
                    //「実行」ボタンを使用不可にする
                    cmdPosExec.BackColor = Color.Lime;
                    cmdPosExec.Enabled = false;
                    this.Enabled = false;
                    //ctSliderV1.ControlLock = true;

                    //マウスを砂時計にする
                    Cursor.Current = Cursors.WaitCursor;

                    //メカ動作
                    error_sts = modMechaControl.MechaUdIndex((float)cwnePos.Value);
                    modCT30K.PauseForDoEvents(1);

                }
	        }
	        catch
	        {    
                //そのまま抜ける
	        }

        ExitHandler:    //追加2014/10/07hata_v19.51反映

            //状態を元に戻す
            this.Enabled = true;
            cmdPosExec.Enabled = true;
            cmdPosExec.BackColor = this.BackColor;
            // Add Start 2018/10/29 M.Oyama V26.40 Windows10対応
            cmdPosExec.UseVisualStyleBackColor = true;
            // Add End 2018/10/29
            Cursor.Current = Cursors.Default;

            //結果がどうあれ、現在の昇降位置にする
            //cwnePos.Value = ntbUpDown.Value
            
            //変更2015/02/02hata
            ////変更2015/01/08hata_小数点4桁以下は四捨五入
            ////cwnePos.Value = Convert.ToDecimal(CTSettings.mecainf.Data.udab_pos);    //v17.02変更 byやまおか 2010/07/23
            //cwnePos.Value = Convert.ToDecimal(Math.Round(CTSettings.mecainf.Data.udab_pos, 3, MidpointRounding.AwayFromZero));       //元に戻す   2011/03/25
            //decimal _val = Convert.ToDecimal(Math.Round(CTSettings.mecainf.Data.udab_pos, 3, MidpointRounding.AwayFromZero));
            //Rev23.10 計測CT対応 by長野 2015/10/16           
            decimal _val = Convert.ToDecimal(Math.Round(myUdab_pos, 3, MidpointRounding.AwayFromZero));
            cwnePos.Value = modLibrary.CorrectInRange(_val, cwnePos.Minimum, cwnePos.Maximum);

            //変更2015/02/12hata_スライダー新規
            //picPointer.Visible = false;   //追加2015/02/02hata
            cwsldUpDown[0].ArrowVisible = false;

            //エラーメッセージ
            if (error_sts != 0) modCT30K.ErrMessage(error_sts, Icon: MessageBoxIcon.Error);		
       
        }

        //*************************************************************************************************
        //機　　能： 指定高さ位置移動時のチェック
        //
        //           変数名          [I/O] 型        内容
        //引　　数： TargetPos       [I/ ] Single    目標昇降位置
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private bool IsOkUpDown(float targetPos)
        {
            //戻り値初期化
            bool functionReturnValue = false;
    
            
            //追加2015/01/27hata→//変更2015/02/04hata
            //現在位置と同一位置の場合は動作させない
            if (ntbUpDown.Value == cwnePos.Value)
            {
                return functionReturnValue;
            }

            //Rev20.00 先頭で行うように変更 by長野 2015/02/06
            //FCD のチェック  'v9.7追加 by added 間々田 2004/12/10
            if (!modSeqComm.CheckFCD(ScanCorrect.GVal_Fcd)) return functionReturnValue;

            //If (CurrentPosi < Val_udab_pos) And ((CurrentPosi - Val_udab_pos) < -0.0001) Then   '現在位置と指定位置が同じ場合はメッセージを出さない対策をした
            if((targetPos + 0.0001) < (double)ntbUpDown.Value)      //v11.3変更 by 間々田 2006/02/27
            {
                //確認メッセージ表示：（コモンによってメッセージを切り替える）
                //   Rev21.00 番号変更9472→9473
                //   リソース9473:X線管・検出器が下降します。サンプル・コリメータ／フィルタ等に衝突しないか確認して下さい。
                //   リソース9510:試料テーブルが上昇しても、X線管にぶつからない位置にいることを確認して下さい。
                //   リソース9905:よろしければＯＫをクリックしてください。
                DialogResult result = MessageBox.Show(CTResources.LoadResString(CTSettings.t20kinf.Data.ud_type == 1 ? 9472 : 9510) + "\r" + "\r" + CTResources.LoadResString(StringTable.IDS_ClickOK),
													  cmdPosExec.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if(result == DialogResult.Cancel)
                {
                    return functionReturnValue;
                }
            }

            //Rev20.00 FCDのチェックを先頭で行う by長野 2015/02/06
            ////FCD のチェック  'v9.7追加 by added 間々田 2004/12/10
            //if(!modSeqComm.CheckFCD(ScanCorrect.GVal_Fcd)) return functionReturnValue;
                
            //動作が禁止されている場合 v9.6 追加 by 間々田 2004/10/12
            if ((CTSettings.scaninh.Data.seqcomm == 0) && (CTSettings.scaninh.Data.table_restriction == 0))
            {
                if (!modSeqComm.MySeq.stsMechaPermit) return functionReturnValue;
            }
    
            //戻り値セット
            functionReturnValue = true;

            return functionReturnValue;
        }


        //*************************************************************************************************
        //機　　能： テーブル回転ボタンの処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [I/ ] Integer   0:左回転，1:右回転
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void cwbtnRotate_ValueChanged(object sender, EventArgs e)       //TODO Index cwbtnRotate配列作成
        {
            if (sender as CWButton == null) return;

            int Index = Array.IndexOf(cwbtnRotate, sender);

			if (Index < 0) return;
            
			int rc = 0;

            //追加2014/10/07hata_v19.51反映
            //機構部が動作可能かチェック
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (cwbtnRotate[Index].Value == true)
            {
                if (!modMechaControl.IsOkMechaMove()) 
                    return;

                //Rev26.00 add by chouno 2017/03/13
                if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
                {
                    return;
                }
            }
            
            if (cwbtnRotate_ValueChanged_IsBusy) return;
            cwbtnRotate_ValueChanged_IsBusy = true;

            //エラー時の扱い
            try 
	        {
                //Valueで分岐
                if (cwbtnRotate[Index].Value)
                {
                    //動作が禁止されている場合は何もしない
                    if ((CTSettings.scaninh.Data.seqcomm == 0) && (CTSettings.scaninh.Data.table_restriction == 0))
                    {
                        //If Not MySeq.stsMechaPermit Then GoTo ExitHandler
//						v17.60 解除ボタンを押すようメッセージを出す by長野 2011/06/02
                        if (!modSeqComm.MySeq.stsMechaPermit)
                        {
                            //
                            MessageBox.Show(CTResources.LoadResString(20038), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            throw new Exception();
                        }
                    }

                    //回転開始
                    rc = modMechaControl.RotateManual(modDeclare.hDevID1, Index, (float)modMechaControl.GVal_TableRotateSpeed, 0);

                    if ((rc == -1) || (rc == -2)) rc = 0;   //-1もしくは-2が正常終了
                }
                else
                {
                    //回転終了
                    //rc = RotateSlowStop(hDevID1)
                    rc = modMechaControl.MechaRotateStop();         //v15.0変更 by 間々田 2009/06/18
                }
	        }
	        catch
	        {
			}

            //マウスカーソルを切り替える
//				Screen.MousePointer = IIf(Value And (rc = 0), vbArrowHourglass, vbDefault)
            //v17.60 カーソルを切り替える条件を変更
            Cursor.Current = (cwbtnRotate[Index].Value && rc == 0 && modSeqComm.MySeq.stsMechaPermit) ? Cursors.AppStarting : Cursors.Default;

            //エラーが発生している場合：メッセージ表示
            if (rc != 0) modCT30K.ErrMessage(rc, Icon: MessageBoxIcon.Error);
    
            //フラグリセット
            cwbtnRotate_ValueChanged_IsBusy = false;
        }

        //*************************************************************************************************
        //機　　能： テーブル回転ボタンマウスアップ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [I/ ] Integer   0:左回転，1:右回転
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void cwbtnRotate_MouseUp(object sender, MouseEventArgs e)
        {
			CWButton button = sender as CWButton;

			if (button == null) return;			

            //ボタンのValueプロパティを確実にオフする
            //（これがないとボタンを連打した場合，ボタンのValueプロパティがTrueのままになってしまうので）
			button.Value = false;

        }

        //*************************************************************************************************
        //機　　能： テーブル昇降ボタンの処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [I/ ] Integer   0:下降，1:上昇
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void cwbtnUpDown_ValueChanged(object sender, EventArgs e)   //TODO Index
        {
            if (sender as CWButton == null) return;

            int Index = Array.IndexOf(cwbtnUpDown, sender);

			if (Index < 0) return;

            int rc = 0;
            bool DirectionOK = false;   //動作方向の許可 //追加2015/02/26hata
                    

            //削除2015/01/02hata
            ////追加2014/12/22hata_ｽﾗｲﾀﾞを1つにする
            //if ((modCT30K.CT30KSetup) & (CTSettings.mecainf.Data.ud_busy != 1) & (cmdPosExec.BackColor != Color.Lime) & cwbtnUpDown[Index].Capture)
            //{
            //     picPointer.Visible = true;
            //}

            //追加2014/10/07hata_v19.51反映
            //機構部が動作可能かチェック
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (cwbtnUpDown[Index].Value == true)
            {
                if (!modMechaControl.IsOkMechaMove())
                    return;

                //Rev26.00 add by chouno 2017/03/13
                if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
                {
                    return;
                }
            }

            if (cwbtnUpDown_ValueChanged_IsBusy) return;
            cwbtnUpDown_ValueChanged_IsBusy = true;

            //エラー時の扱い
            try 
	        {
                //Valueで分岐
                if (cwbtnUpDown[Index].Value)
                {
                    //追加2015/02/26hata
                    //昇降タイプでUpDown動作方向を分けする
                    //X線とテーブルが離れる移動は許可する
                    if ((CTSettings.t20kinf.Data.ud_type == 0))
                    {
                        //テーブルDown移動
                        if (Index == 0) DirectionOK = true;
                    }
                    else
                    {
                        //検出器Up移動
                        if (Index == 1) DirectionOK = true;
                    }

                    //動作が禁止されている場合は何もしない
                    //変更2015/02/26hata　X線とテーブルが離れる移動は許可する
                    //if ((CTSettings.scaninh.Data.seqcomm == 0) && (CTSettings.scaninh.Data.table_restriction == 0))
                    if ((CTSettings.scaninh.Data.seqcomm == 0) && (CTSettings.scaninh.Data.table_restriction == 0) && !DirectionOK)
                    {    //If Not MySeq.stsMechaPermit Then GoTo ExitHandler
                        //v17.60 解除ボタンを押すよメッセージを出す by 長野 2010/05/31
                        if (!modSeqComm.MySeq.stsMechaPermit)
                        {
                            MessageBox.Show(CTResources.LoadResString(20038), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            throw new Exception();
                        }
                    }
                    //昇降開始
                    rc = modMechaControl.UdManual(modDeclare.hDevID1, Index, (float)modMechaControl.GVal_TableUpDownSpeed);
                }
                else
                {
                    //昇降終了
                    //rc = UdSlowStop(hDevID1)
                    rc = modMechaControl.MechaUdStop();     //v15.0変更 by 間々田 2009/06/18
                }		
	        }
	        catch
	        {
			}

            //マウスカーソルを切り替える
//			Screen.MousePointer = IIf(Value And (rc = 0), vbArrowHourglass, vbDefault)
            //v17.60 カーソルを切り替える条件を変更
            this.Cursor = (cwbtnUpDown[Index].Value && (rc == 0) && modSeqComm.MySeq.stsMechaPermit) ? Cursors.AppStarting : Cursors.Default;

            //エラーが発生している場合：メッセージ表示
            if (rc != 0)
            {
                //Rev20.01 以上が発生した場合は停止してからメッセージを出す by長野 2015/06/03
                //昇降終了
                //rc = UdSlowStop(hDevID1)
                int rc2 = 0;
                rc2 = modMechaControl.MechaUdStop();     //v15.0変更 by 間々田 2009/06/18

                modCT30K.ErrMessage(rc, Icon: MessageBoxIcon.Error);
            }
            //フラグリセット
            cwbtnUpDown_ValueChanged_IsBusy = false;
        }

        //*************************************************************************************************
        //機　　能： テーブル昇降ボタンマウスアップ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [I/ ] Integer   0:下降，1:上昇
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void cwbtnUpDown_MouseUp(object sender, MouseEventArgs e)
        {
            //Button button = sender as Button;
            CWButton button = (CWButton)sender;

            if (button == null) return;

            //ボタンのValueプロパティを確実にオフする
            //（これがないとボタンを連打した場合，ボタンのValueプロパティがTrueのままになってしまうので）
            button.Value = false;

        }

        //追加2015/02/10hata
        private void cwbtnUpDown_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (sender as CWButton == null) return;
            int Index = Array.IndexOf(cwbtnUpDown, sender);
            if (Index < 0) return;

            if (!cwbtnUpDown[Index].Capture)
            {
                cwbtnUpDown[Index].Value = false;
            }
        }

        //*************************************************************************************************
        //機　　能： 移動ボタンの処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [I/ ] Integer
        //戻 り 値： なし
        //
        //補　　足： 各コマンドボタンのタグに動作指令があらかじめ埋め込まれている
        //           cwbtnMove(0):XLeft          テーブルＸ軸左
        //           cwbtnMove(1):XRight         テーブルＸ軸右
        //           cwbtnMove(2):YForward       テーブルＹ軸前進
        //           cwbtnMove(3):YBackward      テーブルＹ軸後退
        //           cwbtnMove(4):IIForward      II前進              '削除 2009/07/21
        //           cwbtnMove(5):IIBackward     II後退              '削除 2009/07/21
        //           cwbtnMove(6):XrayXLeft      Ｘ線管左
        //           cwbtnMove(7):XrayXRight     Ｘ線管右
        //           cwbtnMove(8):XrayYForward   Ｘ線管縮小
        //           cwbtnMove(9):XrayYBackward  Ｘ線管拡大
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void cwbtnMove_ValueChanged(object sender, EventArgs e) //TODO cwbtnMove配列作成 .OnImage.CWImage
        {
            if (sender as CWButton == null) return;

            CWButton btn = (CWButton)sender;
            int Index = Array.IndexOf(cwbtnMove, sender);

			if (Index < 0) return;

            //エラー時の扱い
            try 
            {
                //削除2014/10/07hata_v19.51反映
                ////運転準備ボタンが押されていなければ無効
                //if (!modSeqComm.MySeq.stsRunReadySW && (btn.Enabled == true))
                //{
                //    //MsgBox "運転準備が未完のため試料テーブルが移動できません。" & vbCrLf & "運転準備スイッチを押して運転準備完了にしてください。", vbCritical
                //    MessageBox.Show(CTResources.LoadResString(20036) + "\r\n" + CTResources.LoadResString(20032),
                //                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);        //ストリングテーブル化 'v17.60 by 長野 2011/05/22
                //    return;
                //}
                //
                ////v17.40 メンテナンスのときは検査室扉が閉まっていることをチェックしないように変更 by 長野 2010/10/21
                ////稲葉さんの改造待ち
                //if (!modSeqComm.MySeq.stsDoorPermit)
                //{
                ////    v17.20 検査室の扉が閉じていなければ無効 by 長野 2010/09/20
                //    if ((frmCTMenu.Instance.DoorStatus == frmCTMenu.DoorStatusConstants.DoorOpened) && (cwbtnMove[Index].Value == true)) //インターロック用
                //    {
                //        //MsgBox "X線検査室の扉が開いているため試料テーブルを移動することができません｡" & vbCrLf & "X線検査室の扉を閉めてから､再度操作を行なって下さい｡", vbCritical
                //        MessageBox.Show(CTResources.LoadResString(20037) + "\r\n" + CTResources.LoadResString(20034),
                //                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);        //ストリングテーブル化 'v17.60 by 長野 2011/05/22
                //        return;
                //    }
                //}

                //追加2014/10/07hata_v19.51反映
                //機構部が動作可能かチェック（上記を関数化）
                //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                if (cwbtnMove[Index].Value == true)
                {
                    if (!modMechaControl.IsOkMechaMove())
                        return;
                    
                    //Rev26.00 add by chouno 2017/03/13
                    if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
                    {
                        return;
                    }
                }

                //ボタンを押した時，点滅するか？
                bool isblink = false;
                //isblink = (cwbtnMove[Index].OnImage.CWImage.BlinkInterval != CWSpeeds.cwSpeedOff);
                isblink = (cwbtnMove[Index].BlinkInterval != CWSpeeds.cwSpeedOff);

                //Valueで分岐
                if (cwbtnMove[Index].Value)
                {
                    if (isblink)
                    {
                        //Rev26.00 オン指令出す前に動作中チェック
                        if (Index == 0 || Index == 1)
                        {
                            if (modSeqComm.MySeq.stsXLeft || modSeqComm.MySeq.stsXRight)
                            {
                                modCT30K.ErrMessage(6001, Icon: MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else if (Index == 2 || Index == 3)
                        {
                            if (modSeqComm.MySeq.stsYForward || modSeqComm.MySeq.stsYBackward)
                            {
                                modCT30K.ErrMessage(6002, Icon: MessageBoxIcon.Error);
                                return;
                            }
                        }

                        //シーケンサに動作オン指令を送る
                        SendOnToSeq(Convert.ToString(cwbtnMove[Index].Tag));
                    }            
                    //テーブル前進の場合
                    else if (Index == 2)
                    {
                        //変更2014/10/07hata_v19.51反映
                        //if (cmdTableMovePermit.BackColor != Color.Lime)     //v17.10 if追加 byやまおか 2010/08/26
                        //v18.00変更 byやまおか 2011/03/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                        if ((cmdTableMovePermit.BackColor != Color.Lime) & (CTSettings.scaninh.Data.table_restriction == 0))
                        {
                            //MsgBox "解除ボタンをクリックしてください", vbExclamation
                            MessageBox.Show(CTResources.LoadResString(20038), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //ストリングテーブル化 'v17.60 by長野 2011/05/25
                        }

                    }
                    //テーブル左右移動の場合
                    else if (Index == 0 || Index == 1)
                    {
                        //if (cwbtnMove[2].OnImage.CWImage.BlinkInterval == CWSpeeds.cwSpeedOff)
                        if (cwbtnMove[2].BlinkInterval == CWSpeeds.cwSpeedOff)
                        {
                            //変更2014/10/07hata_v19.51反映
                            //if (cmdTableMovePermit.BackColor != Color.Lime)     //v17.10 if追加 byやまおか 2010/08/26
                            //v18.00変更 byやまおか 2011/03/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                            if ((cmdTableMovePermit.BackColor != Color.Lime) & (CTSettings.scaninh.Data.table_restriction == 0))
                            {
                                //MsgBox "解除ボタンをクリックしてください", vbExclamation
                                MessageBox.Show(CTResources.LoadResString(20038), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //ストリングテーブル化 'v17.60 by長野 2011/05/25
                            }
                        }        
                    }
                }
                else
                {
                    //シーケンサに送信した動作オン指令を解除する
                    SendOffToSeq();
                }
                
                //マウスカーソルの制御
                this.Cursor = (cwbtnMove[Index].Value && isblink) ? Cursors.AppStarting : Cursors.Default;

                return;		
            }
            catch (Exception ex)
            {
                //マウスカーソルを元に戻す
                this.Cursor = Cursors.Default;

                //エラーメッセージ
                //modCT30K.ErrMessage(ex.Message, Icon: MessageBoxIcon.Error);    //TODO ErrMessage Err.Description, vbCritical
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //*************************************************************************************************
        //機　　能： 微調テーブル（Ｘ軸Ｙ軸）移動ボタンの処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [I/ ] Integer   0:画面左方向，1:画面右方向
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void cwbtnFineTable_ValueChanged(object sender, EventArgs e)
        {
            int rc = 0;
            float AngOffset = 0;    //試料テーブル上での微調テーブルの回転角（度）
            float Angle = 0;        //微調テーブルの回転角（度）
            float XSpeed = 0;       //X軸移動速度
            float YSpeed = 0;       //Y軸移動速度
            double Theata = 0;

            if (sender as CWButton == null) return;

            int Index = Array.IndexOf(cwbtnFineTable, sender);

			if (Index < 0) return;

            //Rev26.00 add by chouno 2017/10/16 
            //Rev26.40 del by chouno 2019/02/20
            //tmrMecainfSeqCommEx();
                
            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック 'v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (cwbtnFineTable[Index].Value == true)
            {
                if (!modMechaControl.IsOkMechaMove())
                    return;

                //if (modSeqComm.GetLargeRotTableSts() == 1)
                //Rev26.20 微調テーブルタイプも見る change by chouno 2019/02/11 
                if (modSeqComm.GetLargeRotTableSts() == 1 && CTSettings.t20kinf.Data.ftable_type == 0)
                {
                    MessageBox.Show(CTResources.LoadResString(21365), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //２重起動をさせないため
            if (cwbtnFineTable_ValueChanged_IsBusy) return;
            cwbtnFineTable_ValueChanged_IsBusy = true;

            //'v17.20 透視画像とテーブルの左右をI.I.とFPDで揃える処理 by 長野 2010/09/19
            //If SecondDetOn And Use_FlatPanel And IsOKDetPos() Then
            //
            //    If Index = 0 Then
            //        Index = 1
            //    Else
            //        Index = 0
            //    End If
            //
            //End If
            //透視表示の左右反転に対応したため微調(XY)の左右を入れ替えることは行わない。 'v17.50追加 byやまおか 2011/02/01
    
            //エラー時の扱い
            try 
	        {	        
                //ボタンがリミット(水色)のときは押せなくする 'やっぱりしない     'v15.10追加 byやまおか 2009/12/03
                //If cwbtnFineTable(Index).OnImage.CWImage.Picture = Me.ImageList1.ListImages(Index + 3).Picture Then GoTo ExitHandler
    
                //Valueで分岐
                if (cwbtnFineTable[Index].Value)
                {
                    //Rev26.00 add by chouno 2017/03/02
                    //if (CTSettings.mecainf.Data.ystg_busy == 1 || CTSettings.mecainf.Data.xstg_busy == 1)
                    //{
                    //    modCT30K.ErrMessage(6006, Icon: MessageBoxIcon.Error);
                    //}

                    //動作が禁止されている場合 v9.6 追加 by 間々田 2004/10/12
                    if ((CTSettings.scaninh.Data.seqcomm == 0) && (CTSettings.scaninh.Data.table_restriction == 0))
                    {
                        //If Not MySeq.stsMechaPermit Then GoTo ExitHandler
                        //v17.60 解除ボタンを押すようメッセージを出す by長野 2011/06/01
                        if (!modSeqComm.MySeq.stsMechaPermit)
                        {                
                            MessageBox.Show(CTResources.LoadResString(20038), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //ストリングテーブル化 'v17.60 by長野 2011/05/25
        
                            throw new Exception();                
                        }
                    }
        
                    //微調テーブルのオフセット角の設定（度）　将来は微調テーブルを付け直すので、そうしたら０にする
                    //AngOffset = CSng(t20kinf.v_capture_type)
                    AngOffset = Convert.ToSingle(CTSettings.infdef.Data.ftable_off_ang);  //変数がおかしい 'v17.10変更 byやまおか 2010/08/25
        
                    //Angle = (Val_rot_pos / 100) + AngOffset
                    Angle = (float)((ntbRotate.Value + Convert.ToDecimal(AngOffset)) % 360); //changed by 山本　2004-9-27　バグ修正
        
                    Theata = Angle * ScanCorrect.Pai / 180;
                    XSpeed = (float)(modMechaControl.GVal_FineTableSpeed * Math.Abs(Math.Sin(Theata)));
                    YSpeed = (float)(modMechaControl.GVal_FineTableSpeed * Math.Abs(Math.Cos(Theata)));
        
                    //X軸
                    if (XSpeed > 0.05)
                    {
                        if (Math.Sin(Theata) > 0)
                        {
                            rc = modMechaControl.XStgManual(modDeclare.hDevID1, Index, XSpeed);
                        }
                        else if (Math.Sin(Theata) < 0)
                        {
                            rc = modMechaControl.XStgManual(modDeclare.hDevID1, 1 - Index, XSpeed);
                        }
                    }
    
                    //Y軸
                    if (YSpeed > 0.05)
                    {
                        if (Math.Cos(Theata) > 0)
                        {
                            rc = modMechaControl.YStgManual(modDeclare.hDevID1, Index, YSpeed);
                        }
                        else if (Math.Cos(Theata) < 0)
                        {
                            rc = modMechaControl.YStgManual(modDeclare.hDevID1, 1 - Index, YSpeed);
                        }
                    }
                }
                else
                {
                    //微調XY軸停止
                    //rc = XStgSlowStop(hDevID1)
                    rc = modMechaControl.MechaXStgStop();           //v15.0変更 by 間々田 2009/06/18
                    //rc = YStgSlowStop(hDevID1)
                    rc = modMechaControl.MechaYStgStop();           //v15.0変更 by 間々田 2009/06/18
                }
	        }
	        catch
	        {
			}

            //マウスカーソルを切り替える
//			Screen.MousePointer = IIf(Value And (rc = 0), vbArrowHourglass, vbDefault)
            //v17.60 カーソルを切り替える条件を変更　by　長野 2011/05/31
            this.Cursor = (cwbtnFineTable[Index].Value  && (rc == 0) && modSeqComm.MySeq.stsMechaPermit) ? Cursors.AppStarting : Cursors.Default;

            //エラーが発生している場合：メッセージ表示
            if (rc != 0) modCT30K.ErrMessage(rc, Icon: MessageBoxIcon.Error);
    
            //フラグリセット
            cwbtnFineTable_ValueChanged_IsBusy = false;
        }
        
        //*************************************************************************************************
        //機　　能： 微調テーブル（Ｘ軸Ｙ軸）移動ボタンマウスアップ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [I/ ] Integer   0:画面左方向，1:画面右方向
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void cwbtnFineTable_MouseUp(object sender, MouseEventArgs e)
        {    
			CWButton button = sender as CWButton;

			if (button == null) return;			

            //ボタンのValueプロパティを確実にオフする
            //（これがないとボタンを連打した場合，ボタンのValueプロパティがTrueのままになってしまうので）
            button.Value = false;
        }

        //*************************************************************************************************
        //機　　能： 回転傾斜テーブル（回転）ボタンの処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [I/ ] Integer   0:右へ，1:左へ
        //戻 り 値： なし
        //
        //補　　足： 
        //
        //履　　歴： v22.00 2015/08/20 (検S1)長野   新規作成
        //*************************************************************************************************
        private void cwbtnTiltAndRot_Rot_ValueChanged(object sender, EventArgs e)
        {
            //変更2014/11/20hata
            //if (sender as Button == null) return;
            if (sender as CWButton == null) return;
            int Index = Array.IndexOf(cwbtnTiltAndRot_Rot, sender);
            if (Index < 0) return;

            int rc = 0;

            //回転傾斜テーブル未装着の場合は、そのまま抜ける by長野 2015/08/20
            CTSettings.mecainf.Load();
            if (CTSettings.mecainf.Data.tiltrot_table == 0)
            {
                return;
            }

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック(上記を関数化)
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (cwbtnTiltAndRot_Rot[Index].Value == true)
                if (!modMechaControl.IsOkMechaMove())
                    return;


            if (cwbtnTiltAndRot_Rot_ValueChanged_IsBusy) return;
            cwbtnTiltAndRot_Rot_ValueChanged_IsBusy = true;

            //エラー時の扱い
            try
            {
                //Valueで分岐
                if (cwbtnTiltAndRot_Rot[Index].Value)
                {
                    //動作が禁止されている場合 v9.6 追加 by 間々田 2004/10/12
                    if ((CTSettings.scaninh.Data.seqcomm == 0) && (CTSettings.scaninh.Data.table_restriction == 0))
                    {
                        //if (!modSeqComm.MySeq.stsMechaPermit) throw new Exception();
                        //Rev20.00 追加 メッセージを出すようにした by長野 2015/02/06
                        if (!modSeqComm.MySeq.stsMechaPermit)
                        {
                            MessageBox.Show(CTResources.LoadResString(20038), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //ストリングテーブル化 'v17.60 by長野 2011/05/25

                            throw new Exception();
                        }
                    }

                    //移動
                    rc = modMechaControl.TiltRotManual(modDeclare.hDevID1, Index, modMechaControl.GVal_TiltAndRot_RotSpeed);
                }
                else
                {
                    //停止
                    //rc = XStgSlowStop(hDevID1)
                    rc = modMechaControl.TiltRotSlowStop(modDeclare.hDevID1, null);
                }
            }
            catch
            {
            }
            finally
            {
                //マウスカーソルを切り替える
                Cursor.Current = (cwbtnTiltAndRot_Rot[Index].Value && (rc == 0) ? Cursors.AppStarting : Cursors.Default);

                //エラーが発生している場合：メッセージ表示
                if (rc != 0) modCT30K.ErrMessage(rc, Icon: MessageBoxIcon.Error);

                //フラグリセット
                cwbtnTiltAndRot_Rot_ValueChanged_IsBusy = false;
            }
        }


        //*************************************************************************************************
        //機　　能： 回転傾斜テーブル（回転）移動ボタンマウスアップ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [I/ ] Integer   0:右へ，1:左へ
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v22.00 2015/08/20 (検S1)長野   新規作成
        //*************************************************************************************************
        private void cwbtnTiltAndRot_Rot_MouseUp(object sender, MouseEventArgs e)
        {
            //変更2014/11/20hata
            //if (sender as Button == null) return;
            if (sender as CWButton == null) return;
            int Index = Array.IndexOf(cwbtnTiltAndRot_Rot, sender);
            if (Index < 0) return;

            //ボタンのValueプロパティを確実にオフする
            //（これがないとボタンを連打した場合，ボタンのValueプロパティがTrueのままになってしまうので）
            cwbtnTiltAndRot_Rot[Index].Value = false;
        }


        //*************************************************************************************************
        //機　　能： 回転傾斜テーブル（傾斜）移動ボタンの処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [I/ ] Integer   0:縮小方向へ，1:拡大方向へ
        //戻 り 値： なし
        //
        //補　　足： 
        //
        //履　　歴： v22.00 2015/08/20 (検S1)長野   リニューアル
        //*************************************************************************************************
        private void cwbtnTiltAndRot_Tilt_ValueChanged(object sender, EventArgs e)
        {
            //変更2014/11/20hata
            //if (sender as Button == null) return;
            if (sender as CWButton == null) return;
            int Index = Array.IndexOf(cwbtnTiltAndRot_Tilt, sender);
            if (Index < 0) return;

            int rc = 0;

            //回転傾斜テーブル未装着の場合は、そのまま抜ける by長野 2015/08/20
            CTSettings.mecainf.Load();
            if (CTSettings.mecainf.Data.tiltrot_table == 0)
            {
                return;
            }

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック(上記を関数化)
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (cwbtnTiltAndRot_Tilt[Index].Value == true)
                if (!modMechaControl.IsOkMechaMove())
                    return;

            if (cwbtnTiltAndRot_Tilt_ValueChanged_IsBusy) return;
            cwbtnTiltAndRot_Tilt_ValueChanged_IsBusy = true;

            //エラー時の扱い
            try
            {
                //Valueで分岐
                if (cwbtnTiltAndRot_Tilt[Index].Value)
                {
                    //動作が禁止されている場合 v9.6 追加 by 間々田 2004/10/12
                    if ((CTSettings.scaninh.Data.seqcomm == 0) && (CTSettings.scaninh.Data.table_restriction == 0))
                    {
                        //if (!modSeqComm.MySeq.stsMechaPermit)
                        // return
                        //Rev20.00 追加 メッセージを出すようにした by長野 2015/02/06
                        if (!modSeqComm.MySeq.stsMechaPermit)
                        {
                            MessageBox.Show(CTResources.LoadResString(20038), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //ストリングテーブル化 'v17.60 by長野 2011/05/25

                            return;
                        }

                    }

                    rc = modMechaControl.TiltManual(modDeclare.hDevID1, Index, modMechaControl.GVal_TiltAdnRot_TiltSpeed);
                }
                else
                {
                    //停止
                    //rc = YStgSlowStop(hDevID1)
                    rc = modMechaControl.TiltSlowStop(modDeclare.hDevID1, null);
                }
            }
            finally
            {
                //マウスカーソルを切り替える
                Cursor.Current = (cwbtnTiltAndRot_Tilt[Index].Value && (rc == 0) ? Cursors.AppStarting : Cursors.Default);

                //エラーが発生している場合：メッセージ表示
                if (rc != 0) modCT30K.ErrMessage(rc, Icon: MessageBoxIcon.Error);

                //フラグリセット
                cwbtnTiltAndRot_Tilt_ValueChanged_IsBusy = false;
            }
        }


        //*************************************************************************************************
        //機　　能： 回転傾斜テーブル（傾斜）移動ボタンマウスアップ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [I/ ] Integer   0:縮小方向へ，1:拡大方向へ
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v22.00 2015/08/20 (検S1)長野   新規作成
        //*************************************************************************************************
        private void cwbtnTiltAndRot_Tilt_MouseUp(object sender, MouseEventArgs e)
        {
            //変更2014/11/20hata
            //if (sender as Button == null) return;
            if (sender as CWButton == null) return;
            int Index = Array.IndexOf(cwbtnTiltAndRot_Tilt, sender);
            if (Index < 0) return;

            //ボタンのValueプロパティを確実にオフする
            //（これがないとボタンを連打した場合，ボタンのValueプロパティがTrueのままになってしまうので）
            cwbtnTiltAndRot_Tilt[Index].Value = false;
        }

        //*************************************************************************************************
        //機　　能： Ｘ線管回転動作ボタン
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [i/ ] Integer   0:Ｘ線管正転（右向きボタン）
        //                                           1:Ｘ線管逆転（左向きボタン）
        //戻 り 値： なし
        //
        //補　　足： 各ボタンのタグに動作指令があらかじめ埋め込まれている
        //           cwbtnRotateXray(0):XRAYROTCW     Ｘ線管正転
        //           cwbtnRotateXray(1):XRAYROTCCW    Ｘ線管逆転
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*************************************************************************************************
        private void cwbtnRotateXray_ValueChanged(object sender, EventArgs e)   //TODO cwbtnRotateXray配列作成
        {
            int rc = 0;

            if (sender as CWButton == null) return;

            int Index = Array.IndexOf(cwbtnFineTable, sender);

			if (Index < 0) return;

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック 'v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (cwbtnRotateXray[Index].Value == true)
            {
                if (!modMechaControl.IsOkMechaMove())
                    return;
            }

            //エラー時の扱い
            try 
	        {	        
                //動作が禁止されている場合
                if ((CTSettings.scaninh.Data.seqcomm == 0) && (CTSettings.scaninh.Data.table_restriction == 0))
                {
                    //If Not MySeq.stsMechaPermit Then GoTo ExitHandler
                    //v17.60 解除ボタンを押すようメッセージを出す by長野 2011/06/01
                    if (!modSeqComm.MySeq.stsMechaPermit)
                    {
                        MessageBox.Show(CTResources.LoadResString(20038), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //ストリングテーブル化 'v17.60 by長野 2011/05/25
                        
                        throw new Exception();
                    }                     
                }
                    
    
                //v17.60 テーブル昇降にも、運転準備ボタンと検査室扉の開閉チェックを追加 by長野
                //運転準備ボタンが押されていなければ無効
                if (!modSeqComm.MySeq.stsRunReadySW)
                {
                    //MsgBox "運転準備が未完のため試料テーブルが移動できません。" & vbCrLf & "運転準備スイッチを押して運転準備完了にしてください。", vbCritical
                    MessageBox.Show(CTResources.LoadResString(20036) + "\r\n" + CTResources.LoadResString(20032),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);  //ストリングテーブル化 'v17.60 by長野 2011/05/22
                }
    
                //v17.40 メンテナンスのときは検査室扉が閉まっていることをチェックしないように変更 by 長野 2010/10/21
                //稲葉さんの改造待ち
                if (!modSeqComm.MySeq.stsDoorPermit)
                {
                //    v17.20 検査室の扉が閉じていなければ無効 by 長野 2010/09/20
                    if (frmCTMenu.Instance.DoorStatus == frmCTMenu.DoorStatusConstants.DoorOpened)    //インターロック用
                    {
                        //MsgBox "X線検査室の扉が開いているため試料テーブルを移動することができません｡" & vbCrLf & "X線検査室の扉を閉めてから､再度操作を行なって下さい｡", vbCritical
                        MessageBox.Show(CTResources.LoadResString(20037) + "\r\n" + CTResources.LoadResString(20034),
										Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);  //ストリングテーブル化 'v17.60 by長野 2011/05/22
                    }    
                }
        
                //Ｘ線管回転開始・終了
                rc = modMechaControl.PioOutBit(Convert.ToString(cwbtnRotateXray[Index].Tag), (cwbtnRotateXray[Index].Value ? 1 : 0));
                
                //マウスカーソルを切り替える
                this.Cursor = cwbtnRotateXray[Index].Value? Cursors.AppStarting : Cursors.Default;
        
                //エラーがなければここで抜ける
                if (rc == 0) return;
	        }
	        catch
	        {
			}

            //マウスカーソルを元に戻す
            this.Cursor = Cursors.Default;
    
            //エラーメッセージ
            modCT30K.ErrMessage(rc, Icon: MessageBoxIcon.Error);       
        }


        //*************************************************************************************************
        //機　　能： 回転指定（0,90,180,270度指定）チェックボックスチェック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void ctchkRotate_CheckedByClick(object sender, EventArgs e)
        {
			if (sender as CTCheckBox == null) return;

            int Index = Array.IndexOf(ctchkRotate, sender);

			if (Index < 0) return;

            int error_sts = 0;

            //Rev26.00 add by chouno 2017/10/16 
            tmrMecainfSeqCommEx();

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック 'v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
　          if (!modMechaControl.IsOkMechaMove())
            {
                ctchkRotate[Index].Value = false;        //チェックをOFFにする   
                return;
            }

            //追加2014/10/07hata_v19.51反映
            //v19.51 回転大テーブルが装着されている場合は、確認のメッセージを出す by長野 2014/03/03
            //if (modSeqComm.GetLargeRotTableSts() == 1)
            //Rev26.20 微調テーブルタイプも見る by chouno 2019/02/11
            if (modSeqComm.GetLargeRotTableSts() == 1 && CTSettings.t20kinf.Data.ftable_type == 0)
            {

                //Rev26.00 X線・検出器昇降タイプと標準で分ける by chouno 2017/03/13
                if (CTSettings.t20kinf.Data.ud_type == 1)
                {
                    if (MessageBox.Show(CTResources.LoadResString(21360), Application.ProductName, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        ctchkRotate[Index].Checked = false;  //チェックをOFFにする
                        return;
                    }
                }
                else
                {
                    //if (CTSettings.mecainf.Data.xstg_pos != 0.00f || CTSettings.mecainf.Data.ystg_pos != 0.00f)
                    //Rev26.14 計測CTは微調内蔵ではない by chouno 2018/09/10
                    //if ((CTSettings.mecainf.Data.xstg_pos != 0.00f || CTSettings.mecainf.Data.ystg_pos != 0.00f) && CTSettings.scaninh.Data.cm_mode == 1)
                    //Rev26.20 元に戻す by chouno 2019/02/11
                    if (CTSettings.mecainf.Data.xstg_pos != 0.00f || CTSettings.mecainf.Data.ystg_pos != 0.00f)
                    {
                        MessageBox.Show(CTResources.LoadResString(21363), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ctchkRotate[Index].Checked = false; //チェックをOFFにする
                        return;
                    } 
                }
            }

            //ｖ17.60
            //動作が禁止されている場合は何もしない
            if ((CTSettings.scaninh.Data.seqcomm == 0) && (CTSettings.scaninh.Data.table_restriction == 0))
            {
                //If Not MySeq.stsMechaPermit Then GoTo ExitHandler
                //v17.60 解除ボタンを押すようメッセージを出す by長野 2011/06/02
                if (!modSeqComm.MySeq.stsMechaPermit)
                {
                    MessageBox.Show(CTResources.LoadResString(20038), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                   
                    return;
                }
            }
  
            //メカ画面を使用不可にする
            this.Enabled = false;
    
            //マウスポインタを砂時計にする
            this.Cursor = Cursors.WaitCursor;

			//他のチェックボックスのチェックをオフにする
			int i = 0;
			for (i = ctchkRotate.GetLowerBound(0); i<=ctchkRotate.GetUpperBound(0); i++)
			{
				if (i != Index)
				{
					ctchkRotate[i].Checked = false;
				}
			}

            switch (Index)
            {
                case 0:
                    //error_sts = MecaRotateIndex(0)
                    //検出器によって回転方向を変える 'v17.02変更 byやまおか 2010/07/23
                    //FPDの場合はCWで0に戻る
                    if (CTSettings.detectorParam.Use_FlatPanel)
                    {
                        error_sts = modMechaControl.RotateInit(modDeclare.hDevID1);     //回転軸初期化
                        error_sts = modMechaControl.MecaRotateOrigin(true);             //回転軸原点復帰
                    }
                    //I.I.の場合はCCWで0に戻る
                    else
                    {
                        error_sts = modMechaControl.MecaRotateIndex(0);
                    }
                    break;
        
                case 1:
                    //error_sts = MecaRotateIndex(90)
                    //FPDで回転角制限がある場合は、CCWで90度へ行く   'v17.43変更 byやまおか 2011/01/21
                    error_sts = modMechaControl.MecaRotateIndex(CTSettings.detectorParam.Use_FlatPanel ? -270 : 90);
                    break;

                case 2:
                    //error_sts = MecaRotateIndex(180)
                    //FPDで回転角制限がある場合は、CCWで180度へ行く  'v17.43変更 byやまおか 2011/01/21
                    error_sts = modMechaControl.MecaRotateIndex(CTSettings.detectorParam.Use_FlatPanel ? -180 : 180);
                    break;

                case 3:
                    //error_sts = MecaRotateIndex(270)
                    //FPDで回転角制限がある場合は、CCWで270度へ行く  'v17.43変更 byやまおか 2011/01/21
                    error_sts = modMechaControl.MecaRotateIndex(CTSettings.detectorParam.Use_FlatPanel ? -90 : 270);
                    break;
            }

            //表示更新
            UpdateMecha();

            //マウスポインタを元に戻す
            this.Cursor = Cursors.Default;

            //メカ画面を使用可にする
            this.Enabled = true;            
        }

        //*************************************************************************************************
        //機　　能：  Ｘ線管干渉「制限」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void cmdTableMoveRestrict_Click(object sender, EventArgs e)
        {
            //シーケンサに「制限」を設定
            modSeqComm.SeqBitWrite("TableMoveRestrict", true);
        }

        //*************************************************************************************************
        //機　　能：  Ｘ線管干渉「解除」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void cmdTableMovePermit_Click(object sender, EventArgs e)
        {
            //シーケンサに「制限」を設定
            modSeqComm.SeqBitWrite("TableMovePermit", true);
        }

        //*************************************************************************************************
        //機　　能： シーケンサに動作オン指令を送る
        //
        //           変数名          [I/O] 型        内容
        //引　　数： theCommand      [I/ ] String    シーケンサに送信する動作指令文字列
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v14.14  2008/02/20 (WEB)間々田  新規作成
        //*************************************************************************************************
        public void SendOnToSeq(string theCommand)
        {
            //シーケンサに動作オン指令を送る
            modSeqComm.SeqBitWrite(theCommand, true);
    
            //動作指令文字列を記憶
            LastCommand = theCommand;
    
            //通信チェックタイマー開始
            CommCheckOn = true;
        }

        //*************************************************************************************************
        //機　　能： シーケンサに送信した動作オン指令を解除する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v14.14  2008/02/20 (WEB)間々田  新規作成
        //*************************************************************************************************
        public void SendOffToSeq()
        {
            //シーケンサに送信した動作オン指令を解除する
            if (!string.IsNullOrEmpty(LastCommand))
            {
                modSeqComm.SeqBitWrite(LastCommand, false);
                LastCommand = string.Empty;
            }
    
            //Rev20.00 追加 by長野 2014/12/04
            cwbtnMove[0].Value = false;
            cwbtnMove[1].Value = false;
            cwbtnMove[2].Value = false;
            cwbtnMove[3].Value = false;

            //通信チェックタイマー終了
            CommCheckOn = false;
        }

        //*************************************************************************************************
        //機　　能： メカに送信した動作オン指令を解除する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v14.14  2008/02/20 (WEB)間々田  新規作成
        //*************************************************************************************************
        public void SendOffToMecha()
        {  
            //テーブル回転動作・昇降動作・Ｘ線管回転動作を解除
            //cwbtnRotate[0].Enabled = false;
            //cwbtnRotate[1].Enabled = false;
            //cwbtnUpDown[0].Enabled = false;
            //cwbtnUpDown[1].Enabled = false;
            //cwbtnRotateXray[0].Enabled = false;
            //cwbtnRotateXray[1].Enabled = false;
            //Rev20.00 修正 by長野 2014/12/04
            cwbtnRotate[0].Value = false;
            cwbtnRotate[1].Value = false;
            cwbtnUpDown[0].Value = false;
            cwbtnUpDown[1].Value = false;
            cwbtnRotateXray[0].Value = false;
            cwbtnRotateXray[1].Value = false;
    
            //微調テーブル動作解除
            //cwbtnFineTable[0].Enabled = false;
            //cwbtnFineTable[1].Enabled = false;
            //Rev20.00 修正 by長野 2014/12/04
            cwbtnFineTable[0].Value = false;
            cwbtnFineTable[1].Value = false;

            //Rev22.00 回転傾斜テーブル追加 by長野 2015/08/20
            cwbtnTiltAndRot_Rot[0].Value = false;
            cwbtnTiltAndRot_Rot[1].Value = false;
            cwbtnTiltAndRot_Tilt[0].Value = false;
            cwbtnTiltAndRot_Tilt[1].Value = false;

            //if (modLibrary.IsExistForm(frmMechaReset.Instance))
            if (modLibrary.IsExistForm("frmMechaReset"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            {
                frmMechaReset.Instance.SendOffToMecha();
            }
        }

        //*************************************************************************************************
        //機　　能： コリメータ開閉ボタンマウスダウン処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 各コマンドボタンのタグに動作指令があらかじめ埋め込まれている
        //           cmdCollimator(0):ColliLOpen    左開
        //           cmdCollimator(1):ColliLClose   左閉
        //           cmdCollimator(2):ColliROpen    右開
        //           cmdCollimator(3):ColliRClose   右閉
        //           cmdCollimator(4):ColliUOpen    上開
        //           cmdCollimator(5):ColliUClose   上閉
        //           cmdCollimator(6):ColliDOpen    下開
        //           cmdCollimator(7):ColliDClose   下閉
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void cmdCollimator_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender as Button == null) return;

			int Index = Array.IndexOf(cmdCollimator, sender);

            //追加2014/10/07hata_v19.51反映
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (!modMechaControl.IsOkMechaMove())
                return;

            if (Index == 4 || Index == 5)
            {
                if (modSeqComm.MySeq.stsColliUClose || modSeqComm.MySeq.stsColliUOpen)
                {
                    modCT30K.ErrMessage(6004, Icon: MessageBoxIcon.Error);
                    SendOffToSeq();
                    return;
                }
            }
            else if (Index == 6 || Index == 7)
            {  
                if(modSeqComm.MySeq.stsColliDClose || modSeqComm.MySeq.stsColliDOpen)
                {
                    modCT30K.ErrMessage(6005, Icon: MessageBoxIcon.Error);
                    SendOffToSeq();
                    return;
                }
           }

            //シーケンサに動作オン指令を送る
            SendOnToSeq(Convert.ToString(cmdCollimator[Index].Tag));
        }

        //*************************************************************************************************
        //機　　能： コリメータ開閉ボタンマウスアップ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 各コマンドボタンのタグに動作指令があらかじめ埋め込まれている
        //           cmdCollimator(0):ColliLOpen    左開
        //           cmdCollimator(1):ColliLClose   左閉
        //           cmdCollimator(2):ColliROpen    右開
        //           cmdCollimator(3):ColliRClose   右閉
        //           cmdCollimator(4):ColliUOpen    上開
        //           cmdCollimator(5):ColliUClose   上閉
        //           cmdCollimator(6):ColliDOpen    下開
        //           cmdCollimator(7):ColliDClose   下閉
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void cmdCollimator_MouseUp(object sender, MouseEventArgs e)
        {
            //シーケンサに送信した動作オン指令を解除する
            SendOffToSeq();
        }

        //*************************************************************************************************
        //機　　能： 「詳細...」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        private void cmdDetails_Click(object sender, EventArgs e)
        {
            //メカ準備－詳細画面を表示
            //frmMechaReset.Show , frmCTMenu
            //frmMechaReset.Show vbModal 'モーダル表示とする

            //追加2015/01/27hata
            if (modLibrary.IsExistForm("frmMechaReset"))
            {
                frmMechaReset.Instance.WindowState = FormWindowState.Normal;
                frmMechaReset.Instance.Visible = true;
                return;
            }
            frmMechaReset.Instance.Show(frmCTMenu.Instance);
            
        }


        //*************************************************************************************************
        //機　　能： mechacontrol.dll関連の初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        //Private Sub InitMechacontrol()
        public void InitMechacontrol() //public化　v17.20 by 長野　2010/09/20
        {
            int error_sts = 0;  //戻り値
    
            //メカオープン
            modMechaControl.MechaOpen();

            //回転軸の初期化                     'コメントから復活 2001-4-10 by 山本
            error_sts = modMechaControl.RotateInit(modDeclare.hDevID1);
            if (error_sts != 0) goto ErrorProc;

            //昇降軸の初期化                     'コメントから復活 2001-4-10 by 山本
            error_sts = modMechaControl.UdInit(modDeclare.hDevID1);
            if (error_sts != 0) goto ErrorProc;

            //ファントム機構の初期化
            //'error_sts = PhmInit(hDevID1)      'V5.0 deleted by 山本 2001/07/31
            error_sts = modMechaControl.MecaPhmOff();           //V5.0 append by 山本 2001/07/31
            if (error_sts != 0) goto ErrorProc;

            //監視時間の設定
            //error_sts = PioChkStart(ChkTim)
            //if error_sts <> 0 Then GoTo ErrProc

            //制御スイッチの初期化
            error_sts = modMechaControl.SwOpeStart();
            if (error_sts != 0) goto ErrorProc;

            if (CTSettings.scaninh.Data.fine_table == 0) 
            {    
                //微調X軸初期化
                error_sts = modMechaControl.XStgInit(modDeclare.hDevID1);
                if (error_sts != 0) goto ErrorProc;
        
                //微調Y軸初期化
                error_sts = modMechaControl.YStgInit(modDeclare.hDevID1);
                if (error_sts != 0) goto ErrorProc;
            }

            //Rev22.00 回転傾斜テーブルの初期化 追加 by長野 2015/08/20
            if (CTSettings.scaninh.Data.tilt_and_rot == 0)
            {
                //チルト初期化
                error_sts = modMechaControl.TiltInit(modDeclare.hDevID1);

                //回転初期化
                error_sts = modMechaControl.TiltRotInit(modDeclare.hDevID1);
            }

            return;

ErrorProc:
            //エラーメッセージ
            modCT30K.ErrMessage(error_sts, Icon: MessageBoxIcon.Error);		
        }

        ////*************************************************************************************************
        ////機　　能： mecainf（1秒おき）の更新
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴：
        ////*************************************************************************************************
        //v19.50 統合のため廃止 by長野 2013/12/17
        //private void tmrMecainf_Timer(object sender, EventArgs e)
        //{
        //    if (tmrMecainf_Timer_BUSYNOW) return;    //処理中なら実行しない
        //    tmrMecainf_Timer_BUSYNOW = true;

        //    //mecainfの取得
        //    //modMecainf.GetMecainf(ref CTSettings.mecainf.Data);
        //    CTSettings.mecainf.Load();

        //    //mecainfの内容を表示
        //    UpdateMecha();
    
        //    //Mecainfが変更されたことをイベント通知
        //    if (MecainfChanged != null)
        //    {
        //        MecainfChanged(this, EventArgs.Empty);
        //    }
            
        //    //元の状態に戻す
        //    tmrMecainf_Timer_BUSYNOW = false;
        //}


        //*************************************************************************************************
        //機　　能： 機構部のタイマー(tmrSeqCommとtmrMecainfの統合（0.1秒おき）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.40  16/06/19   (検S1)長野   新規作成
        //*************************************************************************************************
        internal void tmrMecainfSeqCommEx()
        {
            tmrMecainfSeqComm_Timer(tmrMecainfSeqComm, new System.EventArgs());
        }

        //*************************************************************************************************
        //機　　能： 機構部のタイマー(tmrSeqCommとtmrMecainfの統合（0.1秒おき）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴：
        //*************************************************************************************************
        private void tmrMecainfSeqComm_Timer(object sender, EventArgs e)
        {
            if (modMechaControl.Flg_SeqCommUpdate == true)
            {
                if (modSeqComm.MySeq == null)
                    return;

                //PCフリーズ対策時、CommCheckを書き込む。added by 山本2004-8-3 PCフリーズ時にX線OFFするため
                if (CTSettings.scaninh.Data.pc_freeze == 0)
                    tmrSeqComm_Timer_CommCheckByTimer = !tmrSeqComm_Timer_CommCheckByTimer;

                //通信チェックプロパティを更新
                if (CommCheckOn | tmrSeqComm_Timer_CommCheckByTimer)
                    modSeqComm.MySeq.BitWrite("CommCheck", true);

                //StatusReadメソッドを呼び出す
                if (modSeqComm.MySeq.StatusRead() == 0)
                {
                    //メカエラーの表示
                    UpdateSeqError();

                    //タッチパネルからの動作
                    Check_SeqOpe();

                    //更新処理
                    MyUpdate();

                    //シーケンサステータスが変更されたことをイベント通知
                    if (SeqCommChanged != null)
                    {
                        //SeqCommChanged();
                        SeqCommChanged(sender, e);
                    }

                    if (CTSettings.SecondDetOn)
                    {
                        //テキストボックスの変更
                        BlinkTextBox();
                    }
                }
            }

            //状態(True:実行中,False:停止中)
            if (modMechaControl.Flg_MechaControlUpdate == true)
            {
                //ここからtmrMecainf
                if (tmrMecainf_Timer_BUSYNOW) return;    //処理中なら実行しない
                tmrMecainf_Timer_BUSYNOW = true;

                //mecainfの取得
                //modMecainf.GetMecainf(ref CTSettings.mecainf.Data);
                CTSettings.mecainf.Load();

                //mecainfの内容を表示
                UpdateMecha();

                //Mecainfが変更されたことをイベント通知
                if (MecainfChanged != null)
                {
                    MecainfChanged(this, EventArgs.Empty);
                }

                //元の状態に戻す
                tmrMecainf_Timer_BUSYNOW = false;
            }

        }

        //*************************************************************************************************
        //機　　能： PIOCheck（0.1秒おき）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴：
        //*************************************************************************************************
        private void tmrPIOCheck_Timer(object sender, EventArgs e)
        {
            modMechaControl.PioChkStart(20);
        }

        //*************************************************************************************************
        //機　　能： シーケンサイベント
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/20  (SI3)間々田  新規作成
        //           v11.5  2006/07/12  (WEB)間々田　0(正常)の場合、SeqComm.exeからイベントを返さないようにした
        //*************************************************************************************************
        private void UC_SeqComm_OnCommEnd(int CommEndAns)
        {
            //スレッド間で操作できないため
            MechErrorNo = CommEndAns;
            try
            {
                SeqCommError();
            }
            catch
            {
            }
            finally
            {
            }                  
            
            
//            //メッセージ出力用
//            string strMsg = null;   //表示ﾒｯｾｰｼﾞ
//            strMsg = "";
    
//            if (UC_SeqComm_OnCommEnd_BUSYNOW) return;   //処理中なら実行しない
//            UC_SeqComm_OnCommEnd_BUSYNOW = true;
            
//#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
///*
//            On Error Resume Next
//*/
//#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

//            try 
//            {	        
//                //ActiveX制御ｴﾗｰの内容を表示
//                switch (CommEndAns)
//                {
//                    //異常
//                    case 700:
//                    case 701:
//                    case 702:
//                    case 703:
//                    case 704:
//                    case 710:
//                    case 715:
//                    case 716:
//                    case 720:
//                    case 721:
//                        //エラーメッセージ
//                        //   700:コミュニケーションエラーが発生しました。
//                        //   701:通信準備接続エラーが発生しました。
//                        //   702:VB通信エラーが発生しました。
//                        //   703:通信タイムアウトエラーが発生しました。
//                        //   704:シーケンサコマンド処理エラーが発生しました。
//                        //   710:読出しエラーが発生しました。
//                        //   715:ビット書込みエラーが発生しました。
//                        //   716:ビット書込みディバイスネームエラーが発生しました。
//                        //   720:ワード書込みエラーが発生しました。
//                        //   721:ワード書込みディバイスネームエラーが発生しました。
                        
//                        string dd = CTResources.LoadResString(CommEndAns);
//                        strMsg = modLibrary.GetFirstItem(dd , "@"); //取得したリソース文字列中の@マーク以降は無視する
//                        //strMsg = modLibrary.GetFirstItem(CTResources.LoadResString(CommEndAns), "@"); //取得したリソース文字列中の@マーク以降は無視する
//                        break;

//                    //未定義のエラー
//                    default:
//                        strMsg = CTResources.LoadResString(StringTable.IDS_UnkownError);      //予想外のエラーが発生しました。
//                        break;
//                }		
//            }
//            catch
//            {
//                // Nothing
//            }

//            try 
//            {	        
//                if (!string.IsNullOrEmpty(strMsg))
//                {
//                    //ｴﾗｰ表示
//                    DialogResult result = MessageBox.Show(CTResources.LoadResString(StringTable.IDS_ErrorNum) + CommEndAns.ToString() + "\r\n" + "\r\n"
//                                                        + strMsg + "\r\n" 
//                                                        + CTResources.LoadResString(9902),
//                                                        Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);  //リソース対応 2003/07/31 by 間々田

//                    if (result == DialogResult.Yes)
//                    {
//                        //End
//                        Environment.Exit(0);
                        
//                        ////下記に変更 by 間々田 2005/01/18 メインフォームをアンロードする（CT30kが起動させたSeqComm.exeなども終了させるため）
//                        //frmCTMenu.Instance.Close();
//                    }
//                }		
//            }
//            catch
//            {
//                // Nothing
//                UC_SeqComm_OnCommEnd_BUSYNOW = false;
                
//            }
            
//            //元の状態に戻す
//            UC_SeqComm_OnCommEnd_BUSYNOW = false;
        }

        //*************************************************************************************************
        //機　　能： SeqComm関連の初期化：シーケンサに必要な情報を送信する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.3  2006/02/20  (SI3)間々田  新規作成
        //*************************************************************************************************
        //Private Sub InitSeqComm()
        public void InitSeqComm()   //v17.20 public化 by 長野 2010/09/20
        {
            //scancondpar.fcd_limitをシーケンサに送信
            if (CTSettings.scaninh.Data.table_restriction == 0)
            {
                modSeqComm.SeqWordWrite("FCDLimit", CTSettings.GVal_FcdLimit.ToString());
                //Rev99.99 seqcomm修正まで保留 by chouno 2017/01/05
                ////Rev25.13 連続して送信するとエラーするので少し待つ by chouno 2017/11/16
                modCT30K.PauseForDoEvents(0.5f);
                modSeqComm.SeqWordWrite("LargeTableRingWidth", (CTSettings.GVal_LargeTableRingWidth * 10).ToString()); //Rev25.10 add by chouno 2017/09/12
                modCT30K.PauseForDoEvents(0.5f);
                modSeqComm.SeqWordWrite("FCDFineLimit", CTSettings.GVal_FcdLimit.ToString());
            }
    
            //Ｘ線外部制御の有無を送信
            modSeqComm.SeqBitWrite("XrayInhibit", (CTSettings.scaninh.Data.xray_remote == 0), false);

            //微調テーブル X/Y 軸の有無をシーケンサに送信
            modSeqComm.SeqBitWrite((CTSettings.scaninh.Data.fine_table_x == 0) ?  "FXTableOn" : "FXTableOff", true, false);
            modSeqComm.SeqBitWrite((CTSettings.scaninh.Data.fine_table_y == 0) ? "FYTableOn" : "FYTableOff", true, false);

            //Ｘ線検出器の種類をシーケンサに送信
            modSeqComm.SeqBitWrite("II", !CTSettings.detectorParam.Use_FlatPanel, false);
            modSeqComm.SeqBitWrite("FPD", CTSettings.detectorParam.Use_FlatPanel, false);

            //テーブルとＸ線管のどちらがＸ軸として移動するかをシーケンサに送信
            modSeqComm.SeqBitWrite("XTableMove", (CTSettings.scaninh.Data.table_x == 0), false);
            modSeqComm.SeqBitWrite("TubeMove", !(CTSettings.scaninh.Data.table_x == 0), false);
        
            //タッチパネル操作を許可
            modSeqComm.SeqBitWrite("PanelInhibit", false, false);
        }   

        //*************************************************************************************************
        //機　　能： シーケンサ通信関連タイマー（0.5秒おき）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： スキャン時は２秒おき
        //
        //履　　歴： v11.3  06/02/23  (SI3)間々田    frmStatusから移動してきた
        //*************************************************************************************************
        //v19.50 統合のため廃止 by長野 2013/12/17
        //        private void tmrSeqComm_Timer(object sender, EventArgs e)
//        {
//            if (modSeqComm.MySeq == null) return;
              
//            //PCフリーズ対策時、CommCheckを書き込む。added by 山本2004-8-3 PCフリーズ時にX線OFFするため
//            if (CTSettings.scaninh.Data.pc_freeze == 0) tmrSeqComm_Timer_CommCheckByTimer = !tmrSeqComm_Timer_CommCheckByTimer;

//            //通信チェックプロパティを更新
//            if (CommCheckOn || tmrSeqComm_Timer_CommCheckByTimer) modSeqComm.MySeq.BitWrite("CommCheck", true);
    
//            //StatusReadメソッドを呼び出す
//            if (modSeqComm.MySeq.StatusRead() == 0)
//            {
//                //メカエラーの表示
//                UpdateSeqError();

//                //タッチパネルからの動作
//                Check_SeqOpe();

//                //更新処理
//                MyUpdate();

//                //シーケンサステータスが変更されたことをイベント通知
//                if (SeqCommChanged != null)
//                {
//                    //SeqCommChanged();
//                    SeqCommChanged(sender, e);
//                }

//#region         //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
//                //        If SecondDetOn Then
//                //            'テキストボックスの変更
//                //            BlinkTextBox
//                //        End If
//                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
//#endregion
//            }           
//        }

        //*************************************************************************************************
        //機　　能： 原点復帰要求のあるパラメータをシーケンサの監視用タイマーを使ってBlinkで表示
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.20  2010/09/15 (検S1)長野  新規
        //*************************************************************************************************
#region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''

        private void BlinkTextBox()
        {
            //運転準備未完はオレンジとする
            //imcompleteColor = RGB(220, 100, 80);
            Color imcompleteColor = Color.FromArgb(220, 100, 80);

            var _with9 =modSeqComm.MySeq;

            //FCD軸
            if (_with9.stsYOrgReq)
            {

                if (tmpFCDTextBoxColor != imcompleteColor)
                {
                    ntbFCD.BackColor = imcompleteColor;
                    //ntbFCD.CaptionAlignment = vbLeftJustify;
                    ntbFCD.CaptionAlignment = ContentAlignment.MiddleLeft;

                    //ntbFCD.Caption = "原点復帰未完"
                    ntbFCD.Text = CTResources.LoadResString(20039);                   //ストリングテーブル化 '17.60 by 長野 2011/05/22

                }
                else
                {
                    ntbFCD.BackColor = System.Drawing.Color.White;
                    ntbFCD.Text = "FCD";
                }
                tmpFCDTextBoxColor = ntbFCD.BackColor;

                //リフレッシュ                                   'v17.51追加 by 間々田 2011/03/25
                ntbFCD.Refresh();

                //Else
                //v17.51変更 by 間々田 2011/03/25
            }
            else if ((ntbFCD.Text != "FCD") | (ntbFCD.BackColor != System.Drawing.Color.White))
            {

                ntbFCD.BackColor = System.Drawing.Color.White;
                ntbFCD.Text = "FCD";

                //リフレッシュ                                   'v17.51追加 by 間々田 2011/03/25
                ntbFCD.Refresh();

            }


            //FDD準備未完
            if (_with9.stsIIOrgReq)
            {

                if (tmpIITextBoxColor != imcompleteColor)
                {
                    ntbFID.BackColor = imcompleteColor;
                    //ntbFID.Caption = "原点復帰未完"
                    ntbFID.Text = CTResources.LoadResString(20039);
                    //ストリングテーブル化 'v17.60 by 長野 2011/05/22
                }
                else
                {
                    ntbFID.BackColor = System.Drawing.Color.White;
                    ntbFID.Text = CTSettings.gStrFidOrFdd;
                }
                tmpIITextBoxColor = ntbFID.BackColor;

                //リフレッシュ                                   'v17.51追加 by 間々田 2011/03/25
                ntbFID.Refresh();

                //Else
                //v17.51変更 by 間々田 2011/03/25
            }
            else if ((ntbFID.Text != CTSettings.gStrFidOrFdd) | (ntbFID.BackColor != System.Drawing.Color.White))
            {

                ntbFID.BackColor =Color.White;
                ntbFID.Text = CTSettings.gStrFidOrFdd;

                //リフレッシュ                                   'v17.51追加 by 間々田 2011/03/25
                ntbFID.Refresh();
            }


            //検出器切替軸準備未完
            if (_with9.stsIIChgOrgReq)
            {

                if (tmpIIChgTextBoxColor != imcompleteColor)
                {
                    lblXrayII.BackColor = imcompleteColor;
                }
                else
                {
                    lblXrayII.BackColor = Color.White;
                }
                tmpIIChgTextBoxColor = lblXrayII.BackColor;

                //リフレッシュ                                   'v17.51追加 by 間々田 2011/03/25
                lblXrayII.Refresh();

                //Else
                //v17.51変更 by 間々田 2011/03/25
            }
            else if (System.Drawing.ColorTranslator.ToOle(lblXrayII.BackColor) != System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White))
            {

                lblXrayII.BackColor = System.Drawing.Color.White;

                //リフレッシュ                                   'v17.51追加 by 間々田 2011/03/25
                lblXrayII.Refresh();

            }

            //Y軸
            if (_with9.stsXOrgReq)
            {

                if (tmpYTextBoxColor != imcompleteColor)
                {
                    ntbTableXPos.BackColor = imcompleteColor;
                    //ntbTableXPos.Caption = "原点復帰未完"
                    ntbTableXPos.Text = CTResources.LoadResString(20039);                    //ストリングテーブル化 'v17.60 by 長野 2011/05/22
                }
                else
                {
                    ntbTableXPos.BackColor = System.Drawing.Color.White;
                    //ntbTableXPos.Caption = "Y軸"
                    //変更2015/01/28hata
                    //ntbTableXPos.Text = StringTable.GetResString(12160, CTSettings.AxisName[1]);                    //ストリングテーブル化 'v17.60 by 長野 2011/05/22
                    ntbTableXPos.Text = CTSettings.AxisName[1];
                }
                tmpYTextBoxColor = ntbTableXPos.BackColor;

                //リフレッシュ                                   'v17.51追加 by 間々田 2011/03/25
                ntbTableXPos.Refresh();

                //Else
                //ElseIf ntbTableXPos.Caption <> "Y軸" Or ntbTableXPos.BackColor <> vbWhite Then           'v17.51変更 by 間々田 2011/03/25
                //ストリングテーブル化 'v17.60 by 長野 2011/05/22
            }
            //else if ((ntbTableXPos.Text != StringTable.GetResString(12160, CTSettings.AxisName[1])) | (ntbTableXPos.BackColor != Color.White))     //変更2015/01/28hata
            else if ((ntbTableXPos.Text != CTSettings.AxisName[1]) | (ntbTableXPos.BackColor != Color.White))
            {
                ntbTableXPos.BackColor = System.Drawing.Color.White;
                //ntbTableXPos.Caption = "Y軸"
                //変更2015/01/28hata
                //ntbTableXPos.Text = StringTable.GetResString(12160, CTSettings.AxisName[1]);                //ストリングテーブル化 'v17.60 by 長野 2011/05/22
                ntbTableXPos.Text = CTSettings.AxisName[1];

                //リフレッシュ                                   'v17.51追加 by 間々田 2011/03/25
                ntbTableXPos.Refresh();

            }
 

            //frmMechaControl.Refresh    'v17.51削除 by 間々田 2011/03/25 これをするとWindows7だと昇降入力欄がちかちかするので

        }
#endregion

        //*************************************************************************************************
        //機　　能： メカステータス変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*************************************************************************************************
        private void lblMechaStatus_TextChanged(object sender, EventArgs e)
        {
			if (sender as Label == null) return;

			int Index = Array.IndexOf(lblMechaStatus, sender);

			if (Index < 0) return;

			Label lbl = (Label)sender;

            //変化があればシーケンサに通知
            modSeqComm.SeqBitWrite(Convert.ToString(lbl.Tag), lbl.Text == "1", false);
        }

        //*************************************************************************************************
        //機　　能： エラーステータスの更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void UpdateSeqError()
        {
            //ここでエラーが発生しても無視

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
            On Error Resume Next
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            try
            {
                //メカエラーの読み込み    
                mySeqStatus[(int)SeqIdxConstants.IdxEmergency].Value = modSeqComm.MySeq.stsEmergency;           //非常停止
                mySeqStatus[(int)SeqIdxConstants.IdxXray225Trip].Value = modSeqComm.MySeq.stsXray225Trip;       //X線225KVトリップ
                mySeqStatus[(int)SeqIdxConstants.IdxXray160Trip].Value = modSeqComm.MySeq.stsXray160Trip;       //X線160KVトリップ
                mySeqStatus[(int)SeqIdxConstants.IdxFilterTouch].Value = modSeqComm.MySeq.stsFilterTouch;       //フィルタユニット接触
                mySeqStatus[(int)SeqIdxConstants.IdxXray225Touch].Value = modSeqComm.MySeq.stsXray225Touch;     //X線管225KV接触
                mySeqStatus[(int)SeqIdxConstants.IdxXray160Touch].Value = modSeqComm.MySeq.stsXray160Touch;     //X線管160KV接触
                mySeqStatus[(int)SeqIdxConstants.IdxRotTouch].Value = modSeqComm.MySeq.stsRotTouch;             //回転テーブル接触
                mySeqStatus[(int)SeqIdxConstants.IdxTiltTouch].Value = modSeqComm.MySeq.stsTiltTouch;           //チルトテーブル接触
                mySeqStatus[(int)SeqIdxConstants.IdxXDriverHeat].Value = modSeqComm.MySeq.stsXDriverHeat;       //テーブルX軸オーバーヒート
                mySeqStatus[(int)SeqIdxConstants.IdxYDriverHeat].Value = modSeqComm.MySeq.stsYDriverHeat;       //テーブルY軸オーバーヒート
                mySeqStatus[(int)SeqIdxConstants.IdxXrayDriverHeat].Value = modSeqComm.MySeq.stsXrayDriverHeat; //X線管切替オーバーヒート
                mySeqStatus[(int)SeqIdxConstants.IdxSeqCpuErr].Value = modSeqComm.MySeq.stsSeqCpuErr;           //シーケンサCPUエラー
                mySeqStatus[(int)SeqIdxConstants.IdxSeqBatteryErr].Value = modSeqComm.MySeq.stsSeqBatteryErr;   //シーケンサバッテリーエラー
                mySeqStatus[(int)SeqIdxConstants.IdxSeqKzCommErr].Value = modSeqComm.MySeq.stsSeqKzCommErr;     //シーケンサKL通信エラー[(int)SeqIdxConstants.KZ]
                mySeqStatus[(int)SeqIdxConstants.IdxSeqKvCommErr].Value = modSeqComm.MySeq.stsSeqKvCommErr;     //シーケンサKL通信エラー[(int)SeqIdxConstants.KV]
                mySeqStatus[(int)SeqIdxConstants.IdxFilterTimeout].Value = modSeqComm.MySeq.stsFilterTimeout;   //フィルタタイムアウト
                mySeqStatus[(int)SeqIdxConstants.IdxTiltTimeout].Value = modSeqComm.MySeq.stsTiltTimeout;       //チルト原点復帰タイムアウト
                mySeqStatus[(int)SeqIdxConstants.IdxXLTouch].Value = modSeqComm.MySeq.stsXLTouch;               //テーブルX左移動中接触
                mySeqStatus[(int)SeqIdxConstants.IdxXRTouch].Value = modSeqComm.MySeq.stsXRTouch;               //テーブルX右移動中接触
                mySeqStatus[(int)SeqIdxConstants.IdxYFTouch].Value = modSeqComm.MySeq.stsYFTouch;               //テーブルY前進中接触
                mySeqStatus[(int)SeqIdxConstants.IdxYBTouch].Value = modSeqComm.MySeq.stsYBTouch;               //テーブルY後退中接触
                mySeqStatus[(int)SeqIdxConstants.IdxXTimeout].Value = modSeqComm.MySeq.stsXTimeout;             //X軸原点復帰タイムアウト    'V5.0 append by 山本 2001/07/31
                mySeqStatus[(int)SeqIdxConstants.IdxIIDriverHeat].Value = modSeqComm.MySeq.stsIIDriverHeat;     //II軸オーバーヒート         'V5.0 append by 山本 2001/07/31
                mySeqStatus[(int)SeqIdxConstants.IdxXDriveErr].Value = modSeqComm.MySeq.stsXDriveErr;           //Ｘ軸制御エラー             'V7.0 append by 間々田 2003/10/24
                mySeqStatus[(int)SeqIdxConstants.IdxXrayCameraUDError].Value = modSeqComm.MySeq.stsXrayCameraUDError;   //Ｘ線・高速度カメラ軸制御エラー //Rev26.40 add by chouno 2019/02/24
            }
            catch
            {
                //Nothing
            }

            try
            {
                //Ｘ線が東芝EXM2-150の場合         'v11.3追加 by 間々田 2006/02/13
                if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150)
                {
                    cwneEXMErrCode.Value = (decimal)modSeqComm.MySeq.stsEXMErrCode;
                }   
            }
            catch
            {
                //Nothing
            }  
        }

        //*************************************************************************************************
        //機　　能： 東芝EXM2-150専用シーケンサエラーステータス変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*************************************************************************************************
        private void cwneEXMErrCode_ValueChanged(object sender, EventArgs e)
        {
            switch((int)cwneEXMErrCode.Value)
            {
                case 0:
                    return;
                case 1:
                case 2:
                case 3:
                case 4:
                case 7:
                case 8:
                case 11:
                case 12:
                case 13:
                case 14:                    
                case 15:
                case 16:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 31:
                case 32:
                case 33:
                case 34:
                    break;
                default:
                    cwneEXMErrCode.Value = 99;    //予想外のエラー
                    break;
            }
        
            //メッセージ表示：～が発生しました。
            MessageBox.Show(StringTable.GetResString(9307, CTResources.LoadResString(9800 + Convert.ToInt32(cwneEXMErrCode.Value))),
                                                     Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    
            //異常リセットコマンドの実行
            modSeqComm.SeqBitWrite("EXMReset", true);
        }

        //'*************************************************************************************************
        //'機　　能： タッチパネルの状態を読み取り、指示された動作を行う。（0.3秒おき）
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V4.0   01/02/08  (SI1)鈴山       新規作成
        //'           V5.0   01/07/31  (CATS)山本      メカ制御変更
        //'           v11.3  06/02/23  (SI3)間々田    frmStatusから移動してきた
        //'*************************************************************************************************
        private void Check_SeqOpe()
        {    
            int error_sts = 0;
    
            try 
	        {     
                //Ｘ線ＯＮ（Ｘ線外部制御可能な場合）
                if (CTSettings.scaninh.Data.xray_remote == 0)
                {
                    if (modSeqComm.MySeq.XrayOff)
                    {
                        //If IsXrayOn Then XrayOff    'Ｘ線ＯＦＦ処理
                        if (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus) modXrayControl.XrayOff();  //Ｘ線ＯＦＦ処理   'v11.5変更 by 間々田 2006/06/21
                    }
                    else if (modSeqComm.MySeq.XrayOn)
                    {
                        //If Not IsXrayOn Then XrayOn 'Ｘ線ＯＮ処理
                        if (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OffStatus) modXrayControl.XrayOn();  //Ｘ線ＯＮ処理       'v11.5変更 by 間々田 2006/06/21
                    }
                }
                
                //デバイスエラーリセット
                if (modSeqComm.MySeq.DeviceErrReset)
                {
                    //メカエラーリセット
                    error_sts = modMechaControl.Mechaerror_reset(modDeclare.hDevID1);
                    if (error_sts != 0) throw new Exception();
                }
        
                //動作が禁止されている場合、以降の動作をさせない         'v9.6 追加 by 間々田 2004/10/12
                if (CTSettings.scaninh.Data.table_restriction == 0)
                {
                    if (!modSeqComm.MySeq.stsMechaPermit) return;
                }
                
                //昇降原点復帰
                mySeqStatus[(int)SeqIdxConstants.IdxUdOrigin].Value = modSeqComm.MySeq.UdOrigin;
        
                //回転原点復帰
                mySeqStatus[(int)SeqIdxConstants.IdxRotOrigin].Value = modSeqComm.MySeq.RotOrigin;

                //微調テーブル
                if (CTSettings.scaninh.Data.fine_table == 0)
                {
                    mySeqStatus[(int)SeqIdxConstants.IdxXStgOrigin].Value = modSeqComm.MySeq.XStgOrigin;    //微調X軸原点復帰
                    mySeqStatus[(int)SeqIdxConstants.IdxYStgOrigin].Value = modSeqComm.MySeq.YStgOrigin;    //微調Y軸原点復帰
                }
                
                //Rev22.00 回転傾斜テーブル 追加 by長野 2015/08/20
                if (CTSettings.scaninh.Data.tilt_and_rot == 0)
                {
                    mySeqStatus[(int)SeqIdxConstants.IdxTiltAndRot_Rot_Origin].Value = modSeqComm.MySeq.TiltRotOrigin;
                    mySeqStatus[(int)SeqIdxConstants.IdxTiltAndRot_Tilt_Origin].Value = modSeqComm.MySeq.TiltOrigin;
                }

                return;                 
	        }
	        catch
	        {
                modCT30K.ErrMessage(error_sts, Icon: MessageBoxIcon.Error);		
	        }
        }

        //*************************************************************************************************
        //機　　能： シーケンサステータス変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.0  09/08/02  (SI1)間々田
        //*************************************************************************************************
        private void ChangeSeqStatus(SeqIdxConstants Index, bool Value) //TODO Private Sub ChangeSeqStatus(ByVal Index As SeqIdxConstants, ByVal Value As Boolean)　cmdCollimator配列
        {
            int error_sts = 0;

            switch (Index)
            {
                //コリメータ限
                case SeqIdxConstants.IdxColliLOLimit:
                case SeqIdxConstants.IdxColliLCLimit:
                case SeqIdxConstants.IdxColliROLimit:
                case SeqIdxConstants.IdxColliRCLimit:
                case SeqIdxConstants.IdxColliUOLimit:
                case SeqIdxConstants.IdxColliUCLimit:
                case SeqIdxConstants.IdxColliDOLimit:
                case SeqIdxConstants.IdxColliDCLimit:             
                    //ボタンの背景色を更新する
                    cmdCollimator[(int)Index].BackColor = (Value ? Color.Lime : SystemColors.Control);
                    break;

    
                //テーブル移動限/Ｘ線管移動限
                case SeqIdxConstants.IdxXLLimit:    
                    SetLimitStatus(cwbtnMove[0], "Left", Value);    //テーブルＸ左限   
                    break;
                case SeqIdxConstants.IdxXRLimit:    
                    SetLimitStatus(cwbtnMove[1], "Right", Value);   //テーブルＸ右限   
                    break;
                case SeqIdxConstants.IdxYFLimit:    
                    SetLimitStatus(cwbtnMove[2], "Up", Value);      //テーブルＹ前進限（拡大限）   
                    break;
                case SeqIdxConstants.IdxYBLimit:    
                    SetLimitStatus(cwbtnMove[3], "Down", Value);    //テーブルＹ後退限（縮小限）   
                    break;
                case SeqIdxConstants.IdxXrayXLLimit: 
                    SetLimitStatus(cwbtnMove[6], "Down", Value);   //Ｘ線管Ｘ軸左限
                    break;
                case SeqIdxConstants.IdxXrayXRLimit: 
                    SetLimitStatus(cwbtnMove[7], "Up", Value);     //Ｘ線管Ｘ軸右限
                    break;
                case SeqIdxConstants.IdxXrayYFLimit: 
                    SetLimitStatus(cwbtnMove[8], "Left", Value);   //Ｘ線管Ｙ軸前進限
                    break;
                case SeqIdxConstants.IdxXrayYBLimit: 
                    SetLimitStatus(cwbtnMove[9], "Right", Value);  //Ｘ線管Ｙ軸後退限
                    break;   
     
        
                //非常停止
                case SeqIdxConstants.IdxEmergency:
        
                    //メインフォームのステータスバー上のアイコンを更新
                    frmCTMenu.Instance.emergency = Value;

                    //追加2014/10/07hata_v19.51反映
                    //'v17.20 検出器切替有の場合のみの処理 非常停止のメッセージボックスでＯＫを押されたかどうかのフラグをfalse by 長野 2010/09/10
                    if (Value & CTSettings.SecondDetOn)
                    {
                        mod2ndDetctor.MsgBoxOK = false;
                    }
                    
                    //エラーが発生した時
                    if (Value)
                    {
                        //Rev26.00 スキャン中でなければX線OFF&ライブOFF add by chouno 2017/03/13
                        if (!(Convert.ToBoolean(modCTBusy.CTBusy & modCTBusy.CTScanStart)))
                        {
                            modXrayControl.XrayOff();
                            if (frmTransImage.Instance.CaptureOn)
                                frmTransImage.Instance.CaptureOn = false;
                        }

                        //メッセージ表示（非同期表示）：～が発生しました。
                        modCT30K.MsgBoxAsync(StringTable.GetResString(9307, mySeqStatus[(int)Index].Caption));
                
                        //実行中の処理に対して停止要求をする     'v17.50下記の処理を関数化して移動。校正時も停止要求する by 間々田 2011/02/17
                        if (Convert.ToBoolean(modCTBusy.CTBusy & modCTBusy.CTScanStart) || Convert.ToBoolean(modCTBusy.CTBusy & modCTBusy.CTScanCorrect))
                        {
                            modCT30K.CallUserStopSet();
                        }
                    }
            
                    //非常停止によるスキャンストップをuserstopを使ってVB側から実行するように変更 by 長野 2010/10/21
                    if ((modSeqComm.MySeq.stsEmergency == true) && ((modCTBusy.CTBusy & modCTBusy.CTScanStart) != 0))
                    {
                            //UserStopSet                        'v17.50上記に移動ここから by 間々田 2011/02/17
                            //
                            //If smooth_rot_cone_flg = True Then
                            //
                            //UserStopSet_rmdsk
                            //
                            //End If                             'v17.50上記に移動ここまで by 間々田 2011/02/17
                                        
                            //ここで非常停止ボタンが押されたことを示すフラグを立てる。
                            modCT30K.emergencyButton_Flg = true;
                    }
                    break;
                    
            
                //エラー                
                case SeqIdxConstants.IdxXray225Trip:
                case SeqIdxConstants.IdxXray160Trip:
                case SeqIdxConstants.IdxFilterTouch:
                case SeqIdxConstants.IdxXray225Touch:
                case SeqIdxConstants.IdxXray160Touch:
                case SeqIdxConstants.IdxRotTouch:
                case SeqIdxConstants.IdxTiltTouch:
                case SeqIdxConstants.IdxXDriverHeat:
                case SeqIdxConstants.IdxYDriverHeat:
                case SeqIdxConstants.IdxXrayDriverHeat:
                case SeqIdxConstants.IdxSeqCpuErr:
                case SeqIdxConstants.IdxSeqBatteryErr:
                case SeqIdxConstants.IdxSeqKzCommErr:
                case SeqIdxConstants.IdxSeqKvCommErr:
                case SeqIdxConstants.IdxFilterTimeout:
                case SeqIdxConstants.IdxTiltTimeout:
                case SeqIdxConstants.IdxXLTouch:
                case SeqIdxConstants.IdxXRTouch:
                case SeqIdxConstants.IdxYFTouch:
                case SeqIdxConstants.IdxYBTouch:
                case SeqIdxConstants.IdxXTimeout:
                case SeqIdxConstants.IdxIIDriverHeat:
                case SeqIdxConstants.IdxXDriveErr:
                case SeqIdxConstants.IdxXrayCameraUDError: //add by chouno 2019/02/24
    
                    //エラーが発生した時
                    if (Value)
                    {
                        //メッセージ表示（非同期表示）：～が発生しました。
                        modCT30K.MsgBoxAsync(StringTable.GetResString(9307, mySeqStatus[(int)Index].Caption));
                    }
                    break;

    
                //回転
                case SeqIdxConstants.IdxRotOrigin:
                    //If Value Then error_sts = MecaRotateOrigin(True)
                    //「パソコン停止で運転準備OFF」機能が有効なとき
                    //リセット途中でOFFしてしまう対策    'v17.46変更 byやまおか 2011/02/27
                    if (Value) 
                    {
                        //Rev20.00 追加 by長野 2015/04/09
                        if (RotMoveByPanel == false)
                        {
                            RotMoveByPanel = true;

                            //原点復帰の前に原点センサを抜ける
                            if (modLibrary.InRange(CTSettings.mecainf.Data.rot_pos, 1, 20 * 100 - 1))
                            {
                                error_sts = modMechaControl.MecaRotateIndex(20, 1);     //20度相対回転させる
                            }

                            //回転原点復帰
                            error_sts = modMechaControl.RotateOrigin(modDeclare.hDevID1, modMechaControl.MyCallbackSeq);
                            //追加2014/10/07hata_v19.51反映
                            modMechaControl.Flg_StartupRotReset = true;     //v18.00追加 byやまおか 2011/07/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

                            RotMoveByPanel = false;
                        }
                    }
                    break;


                //昇降
                case SeqIdxConstants.IdxUdOrigin:
                    //If Value Then error_sts = MecaUdOrigin()
                    //「パソコン停止で運転準備OFF」機能が有効なとき
                    //リセット途中でOFFしてしまう対策    'v17.46変更 byやまおか 2011/02/27
                    if (Value)
                    {

                        //昇降軸初期化
                        //error_sts = modMechaControl.UdInit(modDeclare.hDevID1);

                        //MyCallbackDelegate myCallback = new MyCallbackDelegate(MyCallback);

                        //Rev20.00 追加 by長野 2015/04/09
                        if (UdMoveByPanel == false)
                        {
                            UdMoveByPanel = true;
                            //昇降軸原点復帰
                            error_sts = modMechaControl.UdOrigin(modDeclare.hDevID1, modMechaControl.MyCallbackSeq);
                            UdMoveByPanel = false;
                        
                            //追加2014/10/07hata_v19.51反映
                            //v19.51 追加 by長野 2014/02/27
                            modMechaControl.Flg_StartupUpDownReset = true;
                        }
                    }
                    break;


                case SeqIdxConstants.IdxXStgOrigin:

                    //Rev20.00 追加 by長野 2015/04/09
                    if (fxTableMoveByPanel == false)
                    {
                        fxTableMoveByPanel = true;
                        if (Value) error_sts = modMechaControl.MecaXStgOrigin();
                        fxTableMoveByPanel = false;
                    }
                    break;


                case SeqIdxConstants.IdxYStgOrigin:
                    
                    //Rev20.00 追加 by長野 2015/04/09
                    if (fyTableMoveByPanel == false)
                    {
                        fyTableMoveByPanel = true;
                        if (Value) error_sts = modMechaControl.MecaYStgOrigin();
                        fyTableMoveByPanel = false;
                    }
                    break;
                
                //Rev22.00 追加 回転傾斜テーブル 回転原点復帰 by長野 2015/08/20
                case SeqIdxConstants.IdxTiltAndRot_Rot_Origin:

                    //Rev20.00 追加 by長野 2015/04/09
                    if (TiltRotMoveByPanel == false)
                    {
                        TiltRotMoveByPanel = true;
                        if (Value) error_sts = modMechaControl.MecaTilt_RotOrigin();
                        TiltRotMoveByPanel = false;
                    }
                    break;

                //Rev22.00 追加 回転傾斜テーブル チルト原点復帰 by長野 2015/08/20
                case SeqIdxConstants.IdxTiltAndRot_Tilt_Origin:

                    //Rev20.00 追加 by長野 2015/04/09
                    if (TiltMoveByPanel == false)
                    {
                        TiltMoveByPanel = true;
                        if (Value) error_sts = modMechaControl.MecaTilt_TiltOrigin();
                        TiltMoveByPanel = false;
                    }
                    break;
            
                //I.I.移動
                case SeqIdxConstants.IdxIIForward:
                case SeqIdxConstants.IdxIIBackward:
                    //I.I.の動作が停止した時にイベントを発生させる
                    if (!Value)
					{
						if (IIMovingStopped != null)
						{
							IIMovingStopped(this, EventArgs.Empty);
						}
					}
                    break;

            
                //扉インターロック
                case SeqIdxConstants.IdxDoorLock:
            
                    //メインフォームのステータスバー上のアイコンを更新
                    frmCTMenu.Instance.DoorInterlock = Value;
                    break;

            
                //運転準備
                case SeqIdxConstants.IdxRunReadySW:
        
                    //メインフォームのステータスバー上のアイコンを更新
                    frmCTMenu.Instance.RunReady = Value;
                    break;

          
                //Ｘ線管干渉制限解除
                case SeqIdxConstants.IdxTableMovePermit:

                    cmdTableMovePermit.BackColor = (Value ? Color.Lime : modCT30K.DarkGreen);       //解除
                    cmdTableMoveRestrict.BackColor = (Value ? modCT30K.DarkGreen : Color.Lime);     //制限
                    break;

        
                //v16.01 追加 by 山影 10-02-17
                case SeqIdxConstants.IdxCTIIDrive:
                case SeqIdxConstants.IdxTVIIDrive:
                case SeqIdxConstants.IdxFPDLShiftBusy: //Rev23.20 追加 by長野 2015/12/17

                    //Screen.MousePointer = IIf(Value, vbHourglass, vbDefault)    '動作中マウスを砂時計に    'v17.50削除 ここでマウスポインタを制御しない by 間々田 2011/03/18
           
                    int i = 0;
                    int j = 0;  //ループ用変数
                    int k = 0;  //Rev26.40 add by chouno 2019/02/12

                    i = (Index == SeqIdxConstants.IdxCTIIDrive ? 0 : 1);
                    j = (Index == SeqIdxConstants.IdxTVIIDrive ? 0 : 1);
                    k = (Index == SeqIdxConstants.IdxFPDLShiftBusy ? 0 : 1);//Rev26.40 add by chouno 2019/02/12
            
                    if (CTSettings.HscOn)
                    {
                        //cwbtnChangeMode[i].Value = true;
                        //cwbtnChangeMode[i].BlinkInterval = (Value ? CWSpeeds.cwSpeedFast : CWSpeeds.cwSpeedOff);     //動作中は点滅
                        //cwbtnChangeMode[j].Value = false;
                        //Rev23.40/23.21 by長野 2016/04/05
                        for (int cnt = cwbtnChangeMode.GetLowerBound(0); cnt <= cwbtnChangeMode.GetUpperBound(0); cnt++)
                        {
                            cwbtnChangeMode[cnt].Value = false;
                        }
                        if (i == 0)
                        {
                            cwbtnChangeMode[1].BackColor = Color.Green;
                            cwbtnChangeMode[0].BlinkInterval = (Value ? CWSpeeds.cwSpeedFast : CWSpeeds.cwSpeedOff);
                            cwbtnChangeMode[0].Value = Value;
                        }
                        else if (j == 0)
                        {
                            cwbtnChangeMode[0].BackColor = Color.Green;
                            //Rev26.40 add by chouno 2019/02/19
                            if (Value == true && cwbtnChangeMode[1].BlinkInterval == CWSpeeds.cwSpeedOff) cwbtnChangeMode[1].BackColor = Color.Green;
                            cwbtnChangeMode[1].BlinkInterval = (Value ? CWSpeeds.cwSpeedFast : CWSpeeds.cwSpeedOff);
                            cwbtnChangeMode[1].Value = Value;
                            cwbtnChangeMode[1].Caption = CTResources.LoadResString(17498);
                        }
                        else if (k == 0)
                        {
                            cwbtnChangeMode[0].BackColor = Color.Green;
                            //Rev26.40 add by chouno 2019/02/19
                            if(Value == true && cwbtnChangeMode[1].BlinkInterval == CWSpeeds.cwSpeedOff) cwbtnChangeMode[1].BackColor = Color.Green;
                            cwbtnChangeMode[1].BlinkInterval = (Value ? CWSpeeds.cwSpeedFast : CWSpeeds.cwSpeedOff);
                            cwbtnChangeMode[1].Value = Value;
                            cwbtnChangeMode[1].Caption = CTResources.LoadResString(26031);
                        }
                    }

                    //v17.20 検出器切替ボタンの処理を追加 by 長野 10-08-31
                    if (CTSettings.SecondDetOn & mod2ndDetctor.ReloadFlag)
                    {
                        cwbtnChangeDet[i].Value = true;
                        cwbtnChangeDet[i].BlinkInterval = (Value ? CWSpeeds.cwSpeedFast : CWSpeeds.cwSpeedOff);     //動作中は点滅
                        cwbtnChangeDet[j].Value = false;
                    }

                    //Screen.MousePointer = IIf(Value, vbHourglass, vbDefault)    '動作中マウスを砂時計に    'v17.50削除 ここでマウスポインタを制御しない by 間々田 2011/03/18

                    //検出器シフト中の処理を追加     'v18.00追加 byやまおか 2011/01/31
                    //if (CTSettings.DetShiftOn)
                    //Rev25.00 Wスキャン追加 by長野 2016/06/19
                    if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                    {
                        //Rev23.20 検出器左右シフトの場合の分岐を追加 by長野 2015/12/21
                        //if (CTSettings.scaninh.Data.lr_sft == 0)
                        //Rev25.00 Wスキャンを追加 by長野 2016/07/07
                        if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
                        {
                            if (Index == SeqIdxConstants.IdxCTIIDrive)
                            {
                                modDetShift.DetShift = modDetShift.DetShiftConstants.DetShift_origin;
                            }
                            else if (Index == SeqIdxConstants.IdxTVIIDrive)
                            {
                                modDetShift.DetShift = modDetShift.DetShiftConstants.DetShift_forward;
                            }
                            else if (Index == SeqIdxConstants.IdxFPDLShiftBusy)
                            {
                                modDetShift.DetShift = modDetShift.DetShiftConstants.DetShift_backward;
                            }
                        }
                        else
                        {
                            modDetShift.DetShift = (Index == SeqIdxConstants.IdxCTIIDrive ? modDetShift.DetShiftConstants.DetShift_origin : modDetShift.DetShiftConstants.DetShift_forward);
                        }
                        cwbtnDetShift.Value = true;
                        cwbtnDetShift.BlinkInterval = (Value ? CWSpeeds.cwSpeedFast : CWSpeeds.cwSpeedOff);     //動作中は点滅
                    }

                    //Screen.MousePointer = IIf(Value, vbHourglass, vbDefault)    '動作中マウスを砂時計に    'v17.50削除 ここでマウスポインタを制御しない by 間々田 2011/03/18
            
                    frmCTMenu.Instance.Enabled = !Value;           //CTmenuに全体に制限
                    break;

          
                case SeqIdxConstants.IdxCTIIPos:

                    //変更2014/10/07hata_v19.51反映
                    if (Value) {
					    //                IIMode = IIMode_CT
					    //            End If
					    //v17.20　条件式を追加 by 長野 10-08-31
					    if (CTSettings.HscOn) 
                        {
                            //Rev23.40/23.21 by長野 2016/04/05
                            for (i = cwbtnChangeMode.GetLowerBound(0); i <= cwbtnChangeMode.GetUpperBound(0); i++)
                            {
                                cwbtnChangeMode[i].Value = false;
                                cwbtnChangeMode[i].BlinkInterval = CWSpeeds.cwSpeedOff;
                            }
                            modHighSpeedCamera.IIMode = modHighSpeedCamera.IIModeConstants.IIMode_CT;
                        } 
                        else if (CTSettings.SecondDetOn) 
                        {
						    mod2ndDetctor.DetMode = mod2ndDetctor.DetModeConstants.DetMode_Det1;       //v18.00追加 byやまおか 2011/01/31 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
					    
                        }
                        //Rev25.00 Wスキャン追加 by長野 2016/06/19
                        //else if (CTSettings.DetShiftOn) 
                        else if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                        {
						    modDetShift.DetShift = modDetShift.DetShiftConstants.DetShift_origin;       //基準位置					    
                        }

				    }
				    break;
                
                case SeqIdxConstants.IdxTVIIPos:

                    //変更2014/10/07hata_v19.51反映
				    if (Value) {
					    //            IIMode = IIMode_HSC
					    //            End If
					    if (CTSettings.HscOn) 
                        {
                            //Rev23.40/23.21 by長野 2016/04/05
                            for (i = cwbtnChangeMode.GetLowerBound(0); i <= cwbtnChangeMode.GetUpperBound(0); i++)
                            {
                                cwbtnChangeMode[i].Value = false;
                                cwbtnChangeMode[i].BlinkInterval = CWSpeeds.cwSpeedOff;
                            }
                            modHighSpeedCamera.IIMode = modHighSpeedCamera.IIModeConstants.IIMode_HSC;
					    } 
                        else if (CTSettings.SecondDetOn) 
                        {
						    mod2ndDetctor.DetMode = mod2ndDetctor.DetModeConstants.DetMode_Det2;            //v18.00追加 byやまおか 2011/01/31 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
					    }
                        //Rev25.00 Wスキャン追加 by長野 2016/06/19
                        //else if (CTSettings.DetShiftOn)
                        else if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                        {
						    modDetShift.DetShift = modDetShift.DetShiftConstants.DetShift_forward;          //シフト位置
					    }
				    }
				    break;
    
                case SeqIdxConstants.IdxFPDLShiftPos:　//Rev23.20 追加 by長野 2015/12/17
 
                    if (Value)
                    {
                        //            IIMode = IIMode_HSC
                        //            End If
                        if (CTSettings.HscOn)
                        {
                            //Rev23.40/23.21 by長野 2016/04/05
                            for (i = cwbtnChangeMode.GetLowerBound(0); i <= cwbtnChangeMode.GetUpperBound(0); i++)
                            {
                                cwbtnChangeMode[i].Value = false;
                                cwbtnChangeMode[i].BlinkInterval = CWSpeeds.cwSpeedOff;
                            }
                            //Rev26.40 落下試験機構有りとの場合分け
                            if (CTSettings.iniValue.HSCModeType == 1)
                            {
                                modHighSpeedCamera.IIMode = modHighSpeedCamera.IIModeConstants.IIMode_DROP_TEST;
                            }
                            else
                            {
                                modHighSpeedCamera.IIMode = modHighSpeedCamera.IIModeConstants.IIMode_HSC;
                            }
                        }
                        else if (CTSettings.SecondDetOn)
                        {
                            mod2ndDetctor.DetMode = mod2ndDetctor.DetModeConstants.DetMode_Det2;            //v18.00追加 byやまおか 2011/01/31 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                        }
                        //Rev25.00 Wスキャン追加 by長野 2016/06/19
                        //else if (CTSettings.DetShiftOn)
                        else if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                        {
                            modDetShift.DetShift = modDetShift.DetShiftConstants.DetShift_backward;          //左シフト位置
                        }
                    }

                    break;

                case SeqIdxConstants.IdxFDSystemPos: //Rev23.20 追加 by長野 2015/12/21

                    modSeqComm.ct_gene2and3LimitControl(Value);

                    //Rev23.20 切り替え時にも受け取りなおす by長野 2016/01/26
                    if (Value == true)
                    {
                        string tmpString1;
                        int XOffHTimeY = 0;
                        int XOffHTimeMON = 0;
                        int XOffHTimeD = 0;
                        int XOffHTimeH = 0;
                        int XOffHTimeMIN = 0;

                        int XOffMTimeY = 0;
                        int XOffMTimeMON = 0;
                        int XOffMTimeD = 0;
                        int XOffMTimeH = 0;
                        int XOffMTimeMIN = 0;

                        int ret = 0;

                        //H
                        //年
                        XOffHTimeY = Convert.ToInt32(modSeqComm.MySeq.stsXrayHOffTimeY.ToString("x4"));

                        //月
                        tmpString1 = modSeqComm.MySeq.stsXrayHOffTimeMD.ToString("x4");
                        XOffHTimeMON = Convert.ToInt32(tmpString1.Substring(0, 2));
                        //日                           
                        XOffHTimeD = Convert.ToInt32(tmpString1.Substring(2, 2));

                        //時間
                        tmpString1 = modSeqComm.MySeq.stsXrayHOffTimeHM.ToString("x4");
                        XOffHTimeH = Convert.ToInt32(tmpString1.Substring(0, 2));
                        //分
                        XOffHTimeMIN = Convert.ToInt32(tmpString1.Substring(2, 2));

                        //M
                        //年
                        XOffMTimeY = Convert.ToInt32(modSeqComm.MySeq.stsXrayMOffTimeY.ToString("x4"));
                        //月
                        tmpString1 = modSeqComm.MySeq.stsXrayMOffTimeMD.ToString("x4");
                        XOffHTimeMON = Convert.ToInt32(tmpString1.Substring(0, 2));
                        //日                           
                        XOffHTimeD = Convert.ToInt32(tmpString1.Substring(2, 2));

                        //時間
                        tmpString1 = modSeqComm.MySeq.stsXrayMOffTimeHM.ToString("x4");
                        XOffHTimeH = Convert.ToInt32(tmpString1.Substring(0, 2));
                        //分
                        XOffHTimeMIN = Convert.ToInt32(tmpString1.Substring(2, 2));

                        //この内容をX線OFF時間管理ファイルに書き込む
                        ret = modTitan.Ti_SetXOffTime(430, XOffHTimeY, XOffHTimeMON, XOffHTimeD, XOffHTimeH, XOffHTimeMIN);
                        
                        modCT30K.PauseForDoEvents(0.1f);

                        ret = modTitan.Ti_SetXOffTime(400, XOffMTimeY, XOffMTimeMON, XOffMTimeD, XOffMTimeH, XOffMTimeMIN);

                        modCT30K.PauseForDoEvents(0.1f);

                    }

                    break;

                case SeqIdxConstants.IdxFDSystemBusy: //Rev23.20

                    frmCTMenu.Instance.Enabled = !Value;           //CTmenuに全体に制限
                    break;

				//i.i.位置が未定かつ切替中でもない場合
                case SeqIdxConstants.IdxUnknownIIPos:

                    #region     //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                    //if (!Value) 
                    //{
                    //    IIMode = IIMode_None;					//I.I.切替ボタンは全てOFF
                    //    for (i = cwbtnChangeMode.GetLowerBound(0); i <= cwbtnChangeMode.GetUpperBound(0); i++) {
                    //        cwbtnChangeMode[i].Value = false;
                    //    }
                    //}
                    //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
                    #endregion

                    //Rev23.40 by長野 2016/04/05 Rev23.21 高速度のソース復活 by長野 2016/03/02
                    if (CTSettings.HscOn)
                    {
                        if (!Value)
                        {
                            modHighSpeedCamera.IIMode = modHighSpeedCamera.IIModeConstants.IIMode_None;					//I.I.切替ボタンは全てOFF
                            for (i = cwbtnChangeMode.GetLowerBound(0); i <= cwbtnChangeMode.GetUpperBound(0); i++)
                            {
                                cwbtnChangeMode[i].Value = false;
                            }
                        }
                    }
                    //変更2014/10/07hata_v19.51反映
                    //v17.20 検出器切替ボタン用の処理を追加 by 長野 2010/09/06
				    if (CTSettings.SecondDetOn) 
                    {
					    if (!Value) 
                        {
                            mod2ndDetctor.DetMode = mod2ndDetctor.DetModeConstants.DetMode_None;
						    for (i = cwbtnChangeDet.GetLowerBound(0); i <= cwbtnChangeDet.GetUpperBound(0); i++) {
							    cwbtnChangeDet[i].Value = false;
						    }
					    }
				    }
                    //変更2014/10/07hata_v19.51反映
                    //検出器シフトの処理を追加     'v18.00追加 byやまおか 2011/02/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    //Rev25.00 Wスキャン追加 by長野 2016/06/19
                    //if (CTSettings.DetShiftOn) 
                    if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                    {
					    if (!Value) 
                        {
                            modDetShift.DetShift = modDetShift.DetShiftConstants.DetShift_none;						//シフト位置にいない
						    cwbtnDetShift.OnColor = System.Drawing.Color.Yellow;
						    //cwbtnDetShift.OffColor = System.Drawing.Color.Yellow;
                            cwbtnDetShift.BackColor = System.Drawing.Color.Yellow;					    
                        }
				    }
				    break;

                //変更2014/10/07hata_v19.51反映
                //I.I.切替可否
                case SeqIdxConstants.IdxOKIIMove:
                    modSeqComm.SeqBitWrite("CTChangeNO", !Value);
                    modSeqComm.SeqBitWrite("TVChangeNO", !Value);
                    break;

                //Rev23.10 追加 X線切替用 ここから by長野 2015/09/20
                case SeqIdxConstants.IdxMicroFpdShiftBusy://Rev23.10 追加 by長野 2015/09/20
                case SeqIdxConstants.IdxNanoFpdShiftBusy://Rev23.10 追加 by長野 2015/09/20

                     cwbtnDetShift.BlinkInterval = (Value ? CWSpeeds.cwSpeedFast : CWSpeeds.cwSpeedOff);     //動作中は点滅

    　               cwbtnDetShift.Value = Value;

                     frmCTMenu.Instance.Enabled = !Value;           //CTmenuに全体に制限

                    break;

                case SeqIdxConstants.IdxUnknownFpdPos:

                    if (CTSettings.ChangeXrayOn)
                    {
                        if (!Value)
                        {
                            mod2ndXray.XrayMode = mod2ndXray.XrayModeConstants.XrayMode_None;
                            for (i = cwbtnChangeXray.GetLowerBound(0); i <= cwbtnChangeXray.GetUpperBound(0); i++)
                            {
                                cwbtnChangeXray[i].Value = false;
                            }
                        }
                    }
                    //Rev25.00 Wスキャン追加 by長野 2016/06/19
                    //if (CTSettings.DetShiftOn)
                    if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                    {
                        if (!Value)
                        {
                            modDetShift.DetShift = modDetShift.DetShiftConstants.DetShift_none;						//シフト位置にいない
                            cwbtnDetShift.OnColor = System.Drawing.Color.Yellow;
                            //cwbtnDetShift.OffColor = System.Drawing.Color.Yellow;
                            cwbtnDetShift.BackColor = System.Drawing.Color.Yellow;
                        }
                    }
                    break;
                
 
                case SeqIdxConstants.IdxMicroFpdPos:

                    if (Value)
                    {
                        for (i = cwbtnChangeXray.GetLowerBound(0); i <= cwbtnChangeXray.GetUpperBound(0); i++)
                        {
                            cwbtnChangeXray[i].Value = false;
                        }
                        //modCT30K.PauseForDoEvents(0.5f);
                        //cwbtnChangeXray[0].BackColor = Color.Lime;
                        if (CTSettings.ChangeXrayOn)
                        {
                            mod2ndXray.XrayMode = mod2ndXray.XrayModeConstants.XrayMode_Xray1;
                        }
                        //Rev25.00 Wスキャン追加 by長野 2016/06/19
                        //if (CTSettings.DetShiftOn)
                        if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                        {
                            modDetShift.DetShift = modDetShift.DetShiftConstants.DetShift_origin;
                        }
                    }
                    //else
                    //{
                    //    cwbtnChangeXray[0].BackColor = Color.Green;
                    //}
                    break;

                case SeqIdxConstants.IdxMicroFpdShiftPos:

                    if (Value)
                    {
                        for (i = cwbtnChangeXray.GetLowerBound(0); i <= cwbtnChangeXray.GetUpperBound(0); i++)
                        {
                            cwbtnChangeXray[i].Value = false;
                        }
                        //cwbtnChangeXray[0].BackColor = Color.Lime;
                        if(CTSettings.ChangeXrayOn)
                        {
                            mod2ndXray.XrayMode = mod2ndXray.XrayModeConstants.XrayMode_Xray1;
                        }
                        //Rev25.00 Wスキャン追加 by長野 2016/06/19
                        //if (CTSettings.DetShiftOn)
                        if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                        {
                            modDetShift.DetShift = modDetShift.DetShiftConstants.DetShift_forward;
                        }
                    }
                    //else
                    //{
                    //    cwbtnChangeXray[0].BackColor = Color.Green;
                    //}
                    break;

                case SeqIdxConstants.IdxNanoFpdPos:

                    if (Value)
                    {
                        for (i = cwbtnChangeXray.GetLowerBound(0); i <= cwbtnChangeXray.GetUpperBound(0); i++)
                        {
                            cwbtnChangeXray[i].Value = false;
                        }
                        //modCT30K.PauseForDoEvents(0.5f);
                        //cwbtnChangeXray[1].BackColor = Color.Lime;

                        if (CTSettings.ChangeXrayOn)
                        {
                            mod2ndXray.XrayMode = mod2ndXray.XrayModeConstants.XrayMode_Xray2;
                        }
                        //Rev25.00 Wスキャン追加 by長野 2016/06/19
                        //if (CTSettings.DetShiftOn)
                        if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                        {
                            modDetShift.DetShift = modDetShift.DetShiftConstants.DetShift_origin;
                        }
                    }
                    //else
                    //{
                    //    cwbtnChangeXray[1].BackColor = Color.Green;
                    //}
                    break;

                case SeqIdxConstants.IdxNanoFpdShiftPos:

                    if (Value)
                    {
                        for (i = cwbtnChangeXray.GetLowerBound(0); i <= cwbtnChangeXray.GetUpperBound(0); i++)
                        {
                            cwbtnChangeXray[i].Value = false;
                        }
                        //cwbtnChangeXray[1].BackColor = Color.Lime;

                        if (CTSettings.ChangeXrayOn)
                        {
                            mod2ndXray.XrayMode = mod2ndXray.XrayModeConstants.XrayMode_Xray2;
                        }
                        //Rev25.00 Wスキャン追加 by長野 2016/06/19
                        //if (CTSettings.DetShiftOn)
                        if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
                        {
                            modDetShift.DetShift = modDetShift.DetShiftConstants.DetShift_forward;
                        }
                    }
                    //else
                    //{
                    //    cwbtnChangeXray[1].BackColor = Color.Green; 
                    //}
                    break;

                case SeqIdxConstants.IdxMicroFpdBusy:

                    for (i = cwbtnChangeXray.GetLowerBound(0); i <= cwbtnChangeXray.GetUpperBound(0); i++)
                    {
                        cwbtnChangeXray[i].Value = false;
                    }
                    cwbtnChangeXray[1].BackColor = Color.Green;
                    cwbtnChangeXray[0].BlinkInterval = (Value ? CWSpeeds.cwSpeedFast : CWSpeeds.cwSpeedOff);
                    cwbtnChangeXray[0].Value = Value;
             
                    break;
              
                case SeqIdxConstants.IdxNanoFpdBusy:

                    for (i = cwbtnChangeXray.GetLowerBound(0); i <= cwbtnChangeXray.GetUpperBound(0); i++)
                    {
                        cwbtnChangeXray[i].Value = false;
                    }
                    cwbtnChangeXray[0].BackColor = Color.Green;
                    cwbtnChangeXray[1].BlinkInterval = (Value ? CWSpeeds.cwSpeedFast : CWSpeeds.cwSpeedOff);
                    cwbtnChangeXray[1].Value = Value;
             
                    break;

                //X線切替可否
                case SeqIdxConstants.IdxOkXrayFpdMove:
                    modSeqComm.SeqBitWrite("FPDMoveProhibit", !Value);
                    break;

                //Rev23.10 追加 X線切替用 ここまで by長野 2015/09/20

                //X線WU中
                case SeqIdxConstants.IdxWarmUpNow:
                    modSeqComm.SeqBitWrite("stsWarmUpOn", Value);
                    break;

                //試料テーブル(大)用アタッチメント Rev26.00 add by chouno 2017/03/13
                case SeqIdxConstants.IdxLargeTable:
                    if (Value)
                    {
                        //Re26.14 計測CTの場合、微調テーブル内蔵ではない by chouno 2018/09/10
                        //if (CTSettings.mecainf.Data.xstg_pos != 0.00f || CTSettings.mecainf.Data.ystg_pos != 0.00f)
                        //if ((CTSettings.mecainf.Data.xstg_pos != 0.00f || CTSettings.mecainf.Data.ystg_pos != 0.00f) && CTSettings.scaninh.Data.cm_mode == 1)
                        //微調テーブルタイプをコモンに追加したのでそちらを見るようにした by chouno 2019/02/06
                        if ((CTSettings.mecainf.Data.xstg_pos != 0.00f || CTSettings.mecainf.Data.ystg_pos != 0.00f) && CTSettings.t20kinf.Data.ftable_type == 0)

                        {
                            modCT30K.MsgBoxAsync(CTResources.LoadResString(21364));
                        }
                    }
                    //Rev26.20 微調テーブルタイプで見る by chouno 2019/02/06
                    //if (CTSettings.scaninh.Data.cm_mode == 1) //Rev26.14 計測CTの場合、微調テーブル内蔵ではない by chouno 2018/09/10
                    if (CTSettings.t20kinf.Data.ftable_type == 0)
                    {
                        SetLimitStatus(cwbtnFineTable[1], "Right", Value);      //画面右ボタン
                        SetLimitStatus(cwbtnFineTable[0], "Left", Value);       //画面左ボタン
                    }

                    break;

#region     //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
//'				'v17.20 メカリセット中 追加 by 長野 2010/09/20
//				Case IdxMechaRstBusy
//					frmCTMenu.Enabled = Not Value
//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion
//					frmMechaAllResetMove.cwbtnMechaAllResetMove.Value = Value
//					If Value Then
//						frmMechaAllResetMove.cwbtnMechaAllResetMove.OnText = "停止"
//						frmMechaAllResetMove.cwbtnMechaAllResetMove.OffText = "停止"
//						frmMechaAllResetMove.cmdClose.Enabled = False
//					Else
//						frmMechaAllResetMove.cwbtnMechaAllResetMove.OnText = "OK"
//						frmMechaAllResetMove.cwbtnMechaAllResetMove.OffText = "OK"
//						frmMechaAllResetMove.cmdClose.Enabled = True
//					End If
            }
        }

        //*************************************************************************************************
        //機　　能： シーケンサ値変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.0  09/08/02  (SI1)間々田
        //*************************************************************************************************
        //private void SetSeqValue(SeqIdxValueConstants Index, int Value)     //TODO .OnImage.CWImage
        private void SetSeqValue(SeqIdxValueConstants Index, int Value)
        {
            //前回の値と同じなら抜ける
            
            if (mySeqValue[(int)Index] == Value) return;
            mySeqValue[(int)Index] = Value;
            
            switch (Index)
            {       
                //テーブルＸ
                case SeqIdxValueConstants.IdxXPosition:
                    //ntbTableXPos.Value = (decimal)Value / 100;       //X軸位置    小数点以下2桁まで対応
                    ntbTableXPos.Value = (decimal)Value / modMechaControl.GVal_TableX_SeqMagnify;//Rev23.10 変更 by長野 2015/09/18
                    break;
                
                //テーブルＹ
                case SeqIdxValueConstants.IdxFCD:
                    //ntbFCD.Value = (decimal)Value / 10;              //ＦＣＤ
                    ntbFCD.Value = (decimal)Value / modMechaControl.GVal_FCD_SeqMagnify;//Rev23.10 変更 by長野 2015/09/18
                    break;
        
                //I.I.移動
                case SeqIdxValueConstants.IdxFID:
                    //ntbFID.Value = (decimal)Value / 10;              //ＦＩＤ
                    ntbFID.Value = (decimal)Value / modMechaControl.GVal_FDD_SeqMagnify;//Rev23.10 変更 by長野 2015/09/18
                    break;

                //追加2014/10/07hata_v19.51反映　---　ここから
                //'Ｘ線管回転位置
				case SeqIdxValueConstants.IdxXrayRotPos:
					ntbXrayRotPos.Value = (decimal)Value / 10000;
                    break;
                //'従来のＸ線管Ｙ軸
				case SeqIdxValueConstants.IdxXrayFCD:
					ntbXrayPosX.Value = (decimal)Value / 100;
                    break;
                //'従来のＸ線管Ｘ軸
				case SeqIdxValueConstants.IdxXrayXPos:
                    ntbXrayPosY.Value = (decimal)Value / 100;
                    break;
                //2014/10/07hata_v19.51反映　---　ここまで

                //I.I.視野
                case SeqIdxValueConstants.IdxIINo:
                    
                    //メインフォームのツールバー上のアイコンを更新
                    frmCTMenu.Instance.iifield = Value;
                    break;
            }
        }

        public void SetLimitStatus(CWButton button, string key, bool limitOn)
        {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			With Button
    
				'限界値に至った
				If limitOn Then
            
					If Not (.OnImage.CWImage.BlinkInterval = cwSpeedOff) Then
						.Value = False
						.OnImage.CWImage.BlinkInterval = cwSpeedOff
						Set .OnImage.CWImage.Picture = ImageList1.ListImages(key & "Limit").Picture
						Set .OffImage.CWImage.Picture = ImageList1.ListImages(key & "Limit").Picture
					End If
       
				ElseIf .OnImage.CWImage.BlinkInterval = cwSpeedOff Then
            
					.OnImage.CWImage.BlinkInterval = cwSpeedFastest
					Set .OnImage.CWImage.Picture = ImageList1.ListImages(key).Picture
					Set .OffImage.CWImage.Picture = ImageList1.ListImages(key).Picture
        
				End If

			End With
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            //限界値に至った
            if (limitOn)
            {
                //if (!(button.BlinkInterval == CWSpeeds.cwSpeedOff))
                //{
                    //button.Enabled = false;
                    button.BlinkInterval = CWSpeeds.cwSpeedOff;
                    //追加2015/01/19hata
                    button.Value = false;
                   
                    switch (key)
                    {
                        case "Down":
                            button.OnImage = Resources.ARW01DNLM.ToBitmap();
                            break;
                        case "Up":
                            button.OnImage = Resources.ARW01UPLM.ToBitmap();
                            break;
                        case "Left":
                            button.OnImage = Resources.ARW01LTLM.ToBitmap();
                            break;
                        case "Right":
                            button.OnImage = Resources.ARW01RTLM.ToBitmap();
                            break;
                    //}
                    //button.BlinkInterval = CWSpeeds.cwSpeedOff;
                    
                    //button.OnImage = ImageList1.Images[key + "Limit"];
                    //button.OffImage = ImageList1.Images[key + "Limit"];
                }

                //追加2015/01/19hata
//                button.Value = false;
               
            }
            else if (button.BlinkInterval == CWSpeeds.cwSpeedOff)
            {
                //button.OnImage.CWImage.BlinkInterval = CWSpeeds.cwSpeedFastest;
                //button.OnImage.CWImage.Picture = ImageList1.Images[key];
                //button.OffImage.CWImage.Picture = ImageList1.Images[key];
                button.BlinkInterval = CWSpeeds.cwSpeedFastest;

                //button.OnImage = ImageList1.Images[key];
                //button.OffImage = ImageList1.Images[key];
                switch (key)
                {
                    case "Down":
                        button.OnImage = Resources.ARW01DN.ToBitmap();
                        break;
                    case "Up":
                        button.OnImage = Resources.ARW01UP.ToBitmap();
                        break;
                    case "Left":
                        button.OnImage = Resources.ARW01LT.ToBitmap();
                        break;
                    case "Right":
                        button.OnImage = Resources.ARW01RT.ToBitmap();
                        break;
                }
                //button.OffImage = null;
                //button.Enabled = true;
            }
        }

        //追加2014/10/07hata_v19.51反映
        //*************************************************************************************************
        //機　　能： 高速度透視撮影あり時のコントロール配置
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 各項目をフレームで纏めた
        //           fraMechaPos        :機構部位置
        //           fraUpDown          :昇降操作
        //           fraMechaControl    :機構部操作
        //           fraHighSpeedCamera :CT/高速切替
        //           fraCollimator      :コリメータ
        //           fraXrayRotate      :X線管操作
        //           fraAutoSacnPos     :自動スキャン位置指定
        //           fraIris            :I.I.絞り
        //           cmdDetails         :詳細ボタン
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //*************************************************************************************************
        public void AdjustLayout()
        {
            const int margin_Renamed = 8;   //余白   // 120-->120/15;

            int xbase = 0;          //左基点
            int ybase = 0;          //上基点
            int tmppos = 0;         //計算用変数

            xbase = 8;              //デフォルト値// 120-->120/15;
            ybase = 0;              //デフォルト値

            //CT/高速撮影切替ボタン
            if (CTSettings.HscOn)
                fraHighSpeedCamera.SetBounds(xbase, ybase, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

            //v17.20 検出器切替用ボタン 追加 by 長野 2010-08-31
            if (CTSettings.SecondDetOn)
                fraChangeDetector.SetBounds(xbase, ybase, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

            //v23.10 X線切替(外部制御有) 追加 by 長野 2015-09-18
            if (CTSettings.ChangeXrayOn)
                fraChangeXray.SetBounds(xbase, ybase, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
               
            //メカ位置表示＆Speed調整コントロール
            if (CTSettings.HscOn)
            {
                fraMechaPos.SetBounds(xbase, (fraHighSpeedCamera.Top + fraHighSpeedCamera.Height + margin_Renamed), 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
                //機構部位置フレームの縦サイズ調整
                fraMechaPos.Height = fraMechaPos.Height - (ntbXrayPosY.Top + ntbXrayPosY.Height - ntbXrayRotPos.Top);                //v17.20 検出器切替フレームの表示位置を追加 by 長野 10-08-31
            }
            else if (CTSettings.SecondDetOn)
            {
                fraMechaPos.SetBounds(xbase, (fraChangeDetector.Top + fraChangeDetector.Height + margin_Renamed), 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
                //機構部位置フレームの縦サイズ調整
                fraMechaPos.Height = fraMechaPos.Height - (ntbXrayPosY.Top + ntbXrayPosY.Height - ntbXrayRotPos.Top);
            }
            else if (CTSettings.ChangeXrayOn) //Rev23.10 追加 by長野 2015/09/18
            {
                fraMechaPos.SetBounds(xbase, (fraChangeXray.Top + fraChangeXray.Height + margin_Renamed), 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
                //機構部位置フレームの縦サイズ調整
                fraMechaPos.Height = fraMechaPos.Height - (ntbXrayPosY.Top + ntbXrayPosY.Height - ntbXrayRotPos.Top);
            }
            else
            {
                fraMechaPos.SetBounds(xbase, ybase, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            }

            //自動スキャン位置指定
            //2014/11/07hata キャストの修正
            //tmppos = (fraMechaPos.Width - fraAutoScanPos.Width) / 2;
            tmppos = Convert.ToInt32((fraMechaPos.Width - fraAutoScanPos.Width) / 2F);

            ////Rev22.00 回転傾斜テーブル機能有の場合、傾斜の表示値の下にもってくる(X線管操作とは共存しないという考え) by長野 2015/08/21
            //if (CTSettings.scaninh.Data.tilt_and_rot == 1)
            //{
            //    fraAutoScanPos.SetBounds(tmppos, (fraMechaPos.Top + fraMechaPos.Height + margin_Renamed - ntbXrayPosY.Height), 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            //}
            //else
            //{
            //    fraAutoScanPos.SetBounds(tmppos, (fraMechaPos.Top + fraMechaPos.Height), 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            //}

            //Rev26.00 change by chouno 2017/02/14
            //Rev22.00 回転傾斜テーブル機能有の場合、傾斜の表示値の下にもってくる(X線管操作とは共存しないという考え) by長野 2015/08/21
            if (CTSettings.scaninh.Data.tilt_and_rot == 1)
            {
                fraAutoScanPos.SetBounds(tmppos + 6, (fraMechaPos.Top + fraMechaPos.Height + margin_Renamed - ntbXrayPosY.Height), 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            }
            else
            {
                fraAutoScanPos.SetBounds(tmppos + 6, (fraMechaPos.Top + fraMechaPos.Height), 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            }

            //昇降操作コントロール
            fraUpDown.SetBounds((fraMechaPos.Left + fraMechaPos.Width + margin_Renamed), ybase, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

            //機構部操作コントロール
            fraMechaControl.SetBounds((fraUpDown.Left + fraUpDown.Width + margin_Renamed), ybase, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

            //コリメータ
            fraCollimator.SetBounds((fraMechaControl.Left + fraMechaControl.Width), ybase, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

            //X線管操作
            fraXrayRotate.SetBounds(fraCollimator.Left, (fraCollimator.Top + fraCollimator.Height), 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            fraXrayRotate.Visible = (CTSettings.scaninh.Data.xray_rotate == 0);

            //回転傾斜テーブル Rev22.00 追加 by長野 2015/08/20
            fraTiltAndRot.SetBounds(fraCollimator.Left, (fraCollimator.Top + fraCollimator.Height), 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

            //短残光I.I.絞り 'X線管操作と同じ位置
            fraIris.SetBounds(fraXrayRotate.Left, fraXrayRotate.Top, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            fraIris.Visible = CTSettings.HscOn;

            //v17.20 メカオールリセットボタン 2010/09/09 by 長野
            //2014/11/07hata キャストの修正
            //tmppos = ((fraIris.Width) - (cmdMechaAllReset.Width)) / 2;
            tmppos = Convert.ToInt32((fraIris.Width - cmdMechaAllReset.Width) / 2F);
            cmdMechaAllReset.SetBounds(((fraIris.Left) + tmppos), (fraIris.Top + fraIris.Height + margin_Renamed), 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

            //v17.20 詳細ボタンの位置変更 by 長野 2010/09/06

            //    '詳細ボタン
            //    tmpPos = (fraIris.width - cmdDetails.width) / 2
            //    cmdDetails.Move fraIris.Left + tmpPos, fraIris.Top + fraIris.Height + margin

            //詳細ボタン
            //2014/11/07hata キャストの修正
            //tmppos = (fraIris.Width - cmdDetails.Width) / 2;
            tmppos = Convert.ToInt32((fraIris.Width - cmdDetails.Width) / 2F);
            cmdDetails.SetBounds((fraIris.Left + tmppos), (cmdMechaAllReset.Top + cmdMechaAllReset.Height + margin_Renamed), 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
           
        }
   
 
        //*************************************************************************************************
        //機　　能： 短残光I.I.開閉ボタンマウスダウン処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 各コマンドボタンのタグに動作指令があらかじめ埋め込まれている
        //           cmdIris(0):IrisLOpen    左開
        //           cmdIris(1):IrisLClose   左閉
        //           cmdIris(2):IrisROpen    右開
        //           cmdIris(3):IrisRClose   右閉
        //           cmdIris(4):IrisUOpen    上開
        //           cmdIris(5):IrisUClose   上閉
        //           cmdIris(6):IrisDOpen    下開
        //           cmdIris(7):IrisDClose   下閉
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影    新規作成
        //*************************************************************************************************
        //Rev23.40/Rev23.21 by長野 2016/04/05
        private void cmdIris_MouseDown(object sender, MouseEventArgs e)
        {

            if (sender as Button == null) return;
            int Index = Array.IndexOf(cmdIris, sender);
            if (Index < 0) return;

            //シーケンサに動作オン指令を送る
            SendOnToSeq(Convert.ToString(cmdIris[Index].Tag));
        }
        //*************************************************************************************************
        //機　　能： 短残光I.I.開閉ボタンマウスアップ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 各コマンドボタンのタグに動作指令があらかじめ埋め込まれている
        //           cmdIris(0):IrisLOpen    左開
        //           cmdIris(1):IrisLClose   左閉
        //           cmdIris(2):IrisROpen    右開
        //           cmdIris(3):IrisRClose   右閉
        //           cmdIris(4):IrisUOpen    上開
        //           cmdIris(5):IrisUClose   上閉
        //           cmdIris(6):IrisDOpen    下開
        //           cmdIris(7):IrisDClose   下閉
        //
        //履　　歴： v16.01 2010/02/02 (検SS)山影    新規作成
        //*************************************************************************************************
        //Rev23.40/Rev23.21 by長野 2016/04/05       
        private void cmdIris_MouseUp(object sender, MouseEventArgs e)
        {
            //シーケンサに送信した動作オン指令を解除する
            SendOffToSeq();
        }     

        //'*************************************************************************************************
        //'機　　能：  CT/高速切替ボタンクリック時処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： Index           [I/ ] Integer
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v16.01 2010/02/02 (検SS)山影   新規作成
        //'*************************************************************************************************
        //Private Sub cwbtnChangeMode_Click(Index As Integer)
        //
        //    Dim err_sts As Long
        //    Dim i As Integer
        //    Dim theMode As IIModeConstants
        //
        //    '既にボタンが有効になっている場合
        //    If cwbtnChangeMode(Index).Value = True Then Exit Sub
        //
        //    '切り替え可否チェック
        //    If Not IsOKIIMove() Then Exit Sub
        //
        //    Select Case Index
        //        Case 0: theMode = IIMode_CT
        //        Case 1: theMode = IIMode_HSC
        //    End Select
        //
        //    'CT/高速撮影状態へ
        //    IIMode = theMode
        //
        //
        //    'I.I.移動開始
        //    If Not SwitchII(theMode) Then GoTo ExitHandler
        //
        //    Exit Sub
        //
        //ExitHandler:
        //
        //End Sub
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
        //Rev23.40/Rev23.21 by長野 2016/04/05
        private void cwbtnChangeMode_Click(Object sender, MouseEventArgs e)
        {
            if (sender as CWButton == null) return;

            int Index = Array.IndexOf(cwbtnChangeMode, sender);

            if (Index < 0) return;

            modHighSpeedCamera.IIModeConstants theMode = default(modHighSpeedCamera.IIModeConstants);

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック（上記を関数化）
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (!modMechaControl.IsOkMechaMove())
                return;

            //既にボタンが有効になっており(検出器が撮影位置にある)，
            //コモンも選択した検出器用になっている場合，何もせずに抜ける。
            if (Index == 0)
            {
                if (modSeqComm.MySeq.stsCTIIPos == true)
                {
                    return;
                }
            }
            if (Index == 1)
            {
                if (modSeqComm.MySeq.stsTVIIPos == true)
                {
                    return;
                }
            }


            //if (cwbtnChangeMode[Index].Value == true)
            //{
            //    return;
            //}

            //切替OKか判定
            if (!modHighSpeedCamera.IsOKIIMove())
            {
                return;
            }

            switch (Index)
            {
                case 0:
                    theMode = modHighSpeedCamera.IIModeConstants.IIMode_CT;
                    break;
                case 1:
                    theMode = modHighSpeedCamera.IIModeConstants.IIMode_HSC;
                    break;
            }

            //
            //モード切替（制御が変わる)
            //modHighSpeedCamera.IIMode = theMode;

            //モード切替（メカ位置が変わる）
            modHighSpeedCamera.SwitchII(theMode);

            return;
        }
        
        //*************************************************************************************************
        //機　　能： 検出器切替ボタンクリック時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.20  2010/08/31 (検S1)長野  リニューアル
        //*************************************************************************************************
        private void cwbtnChangeDet_Click(Object sender, EventArgs e)
        {
            if (sender as CWButton == null) return;

            int Index = Array.IndexOf(cwbtnChangeDet, sender);

            if (Index < 0) return;

             mod2ndDetctor.DetModeConstants theMode = default(mod2ndDetctor.DetModeConstants);

            //削除2014/10/07hata_v19.51反映
            //    '運転準備ボタンが押されていなければ無効
            //    If Not MySeq.stsRunReadySW Then
            //        'MsgBox "運転準備が未完のため検出器切替ができません。" & vbCrLf & "運転準備スイッチを押して運転準備完了にしてください。", vbCritical
            //        MsgBox LoadResString(20031) & vbCrLf & LoadResString(20032), vbCritical 'ストリングテーブル化 'v17.60 by 長野 2011/05/22
            //        Exit Sub
            //    End If
            //
            //    'v17.40 メンテナンスのときは検査室扉が閉まっていることをチェックしないように変更 by 長野 2010/10/21
            //    'v17.40 稲葉さんの改造待ち
            //    If Not MySeq.stsDoorPermit Then
            //
            //        'v17.20 検査室の扉が閉じていなければ無効 by 長野 2010/09/20
            //       If (frmCTMenu.DoorStatus = DoorOpened) Then
            //
            //        'MsgBox "Ｘ線検査室の扉が開いているため検出器切替ができません。" & vbCrLf & "Ｘ線検査室の扉を閉めてから、再度操作を行なって下さい。", vbCritical
            //        MsgBox LoadResString(20033) & vbCrLf & LoadResString(20034), vbCritical 'ストリングテーブル化 'v17.60 by 長野 2011/05/22
            //
            //        End If
            //
            //    End If

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック（上記を関数化）
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (!modMechaControl.IsOkMechaMove())
                return;

            //既にボタンが有効になっており(検出器が撮影位置にある)，
            //コモンも選択した検出器用になっている場合，何もせずに抜ける。
            if (cwbtnChangeDet[Index].Value == true & CTSettings.scansel.Data.com_detector_no == Index)
            {
                return;
            }

            //切り替え可否チェック
            if (!mod2ndDetctor.IsSwitchDet())
            {
                //MsgBox LoadResString(IDS_AlignmentNow), vbInformation
                //Interaction.MsgBox(Project1.My.Resources.str17502, MsgBoxStyle.OkOnly);
                MessageBox.Show(CTResources.LoadResString(17502), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.None);
                
                return;
            }

            //他のダイアログが開いていたら無効
            if (!mod2ndDetctor.IsAllCloseFrm)
            {
                //Interaction.MsgBox(Project1.My.Resources.str17502, MsgBoxStyle.OkOnly);
                MessageBox.Show(CTResources.LoadResString(17502), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }

            switch (Index)
            {
                case 0:
                    theMode = mod2ndDetctor.DetModeConstants.DetMode_Det1;
                    break;
                case 1:
                    theMode = mod2ndDetctor.DetModeConstants.DetMode_Det2;
                    break;
            }

            //DetMode = theMode

            //I.I.移動開始
            //If Not SwitchDet(theMode) Then Exit Sub

            //-->v17.50変更 by 間々田 2011/03/18 マウスポインタの制御はChangeSeqStatusではなくこちらで実行するための変更
            bool IsOK = false;

            //マウスポインタを砂時計にする前にバックアップを取る
            System.Windows.Forms.Cursor MousePointerBak = null;
            MousePointerBak = System.Windows.Forms.Cursor.Current;

            //検出器スイッチ
            //IsOK = SwitchDet(theMode)
            //IsOK = SwitchDet(theMode, UNSET_GAIN) 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //v19.50 元に戻す by長野 2013/12/17
            IsOK = modSeqComm.SwitchDet(theMode);

            //バックアップしたマウスポインタに戻す
            System.Windows.Forms.Cursor.Current = MousePointerBak;

            //失敗したらここで抜ける
            if (!IsOK)
                return;
            //<--v17.50変更 by 間々田 2011/03/18 マウスポインタの制御はChangeSeqStatusではなくこちらで実行するための変更

            frmCTMenu.Instance.Toolbar1.Items["I.I.Field"].Enabled = (CTSettings.scaninh.Data.mechacontrol == 0) & (CTSettings.scaninh.Data.iifield == 0) & (!CTSettings.detectorParam.Use_FlatPanel);            //I.I.視野：検出器がFPDの場合、非表示
            frmCTMenu.Instance.Toolbar1.Items["I.I.Field"].Visible = (CTSettings.scaninh.Data.mechacontrol == 0) & (CTSettings.scaninh.Data.iifield == 0) & (!CTSettings.detectorParam.Use_FlatPanel);            //I.I.視野：検出器がFPDの場合、非表示
        }

        //追加2014/10/07hata_v19.51反映
        //*************************************************************************************************
        //機　　能： 検出器切替時のフォームのリロードを再度トライするためのタイマー処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.51  2011/03/25 (豊川)間々田  新規作成
        //*************************************************************************************************
        private void tmrTryReloadForms_Tick(object sender, EventArgs e)
        {
            //このタイマーをオフ
            tmrTryReloadForms.Enabled = false;
            //イベントを取る
            modCT30K.PauseForDoEvents(1);
            
            //'再度フォームのリロードに行く
            mod2ndDetctor.ReloadForms(false);
        }
        //

        //*************************************************************************************************
        //機　　能： X線切替ボタン値変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v23.10  2015/09/28 (検S1)長野  新規作成
        //*************************************************************************************************
        private void cwbtnChangeXray_ValueChanged(object sender, EventArgs e)		// 【C#コントロールで代用】
        {
            int Index = Array.IndexOf(cwbtnChangeXray, sender);

            //if (cwbtnMoveValue)
            //if (cwbtnChangeXray[Index].Visible && cwbtnChangeXray[Index].Value)
            //{
            //    cwbtnChangeXray[Index].BackColor = Color.Lime;

            //}
            //else
            //{
            //    cwbtnChangeXray[Index].BackColor = Color.Green;
            //}
        }

        //
        //追加2014/10/07hata_v19.51反映
        //*************************************************************************************************
        //機　　能： スキャンエリアフレームを更新する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v18.00  2011/07/16  やまおか    新規作成
        //*************************************************************************************************
        //Public Sub UpdateScanarea(ByVal scanmode As Integer) 
        //private void UpdateScanarea()       //v18.00変更 byやまおか 2011/07/03
        public void UpdateScanarea()       //v18.00変更 byやまおか 2011/07/03 //Rev26.00 change by chouno 2017/03/03
        {

            float L_det = 0;                //検出器長(mm)
            float L_shift = 0;              //シフト量(mm)
            float L_shiftL = 0;             //シフト量左(mm) Rev23.20 左右シフト対応 by長野 2016/01/09
            float L_shiftR = 0;             //シフト量右(mm) Rec23.20 左右シフト対応 by長野 2016/01/09
            float L_det_shift = 0;          //シフト検出器長(mm)(検出器長＋シフト量)
            float L_inv = 0;                //検出器周辺の無効領域の片側長さ(mm)
            float L_margin = 0;             //回転中心位置の余裕(mm)(端に寄せすぎないように)
            float R_fullhalf = 0;           //直径(ハーフフル)
            float R_offset = 0;             //直径(オフセット)
            float R_shift = 0;              //直径(シフト)(判断してから使う)
            float R_U_shift = 0;            //直径(シフト)＋
            float R_L_shift = 0;            //直径(シフト)－
            float Y_fullhalf = 0;           //Y軸現在位置(ハーフフル)
            float Y_offset = 0;             //Y軸現在位置(オフセット)
            float Y_shift = 0;              //Y軸現在位置(シフト)
            float Ymax_fullhalf = 0;        //Y軸最大値(ハーフフル)
            float Ymax_offset = 0;          //Y軸最大値(オフセット)
            float Ymax_shift = 0;           //Y軸最大値(シフト)(判断してから使う)
            float Ymax_shift_U = 0;         //Y軸最大値(シフト位置)＋
            float Ymax_shift_L = 0;         //Y軸最大値(シフト位置)－
            float Ymax_unshift_U = 0;       //Y軸最大値(基準位置)＋
            float Ymax_unshift_L = 0;       //Y軸最大値(基準位置)－

            float theTableYPos = 0;
            float theFCDFIDRate = 0;
            float theFIDWithOffset = 0;
            float theYPosRate = 0;

            theTableYPos = TableYPos;
            theFCDFIDRate = FCDFIDRate;
            theFIDWithOffset = FIDWithOffset;

            //v19.50 検出器長を出すのにIlsとIleを使用する by長野 2014/01/28
            int Ile = 0;
            int Ils = 0;
            //v19.50 係数は独自にもつ by長野 2014/01/28
            float K = 0;
            //K = LimitFanAngle
            //K = 1;
            //Rev23.20 KがLimitFanangleに相当するのでスキャンソフト側とあわせる by長野 2016/01/23
            K = 0.99f;

            //scancondpar.csvから取得
            var _with14 = CTSettings.scancondpar.Data;
            
            //L_det = CSng(.mainch(0)) * .fpd_pitch   '検出器長
            //v19.50 IlsとIleを使用するように変更
            //スキャン校正で使うファン角を調整する   'v17.22変更 byやまおか 2010/10/19
            //PkeFPDの場合
            //PkeFPDの場合
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))
            {

                //iedは、sft量が加算されているのでh_sizeを使う。istはそのまま使える
                if (CTSettings.detectorParam.h_size == 1024)
                {
                    Ils = _with14.ist + 12;
                    Ile = CTSettings.detectorParam.h_size - 1 - 12;
                }
                else if (CTSettings.detectorParam.h_size == 2048)
                {
                    Ils = _with14.ist + 24;
                    Ile = CTSettings.detectorParam.h_size - 1 - 24;
                }
                //Rev25.14 変更 by chouno 2017/11/22
                L_det = (float)(Ile - Ils + 1) * _with14.fpd_pitch;
            }
            else
            {
                Ils = _with14.ist + 2;
                Ile = CTSettings.detectorParam.h_size - 1 - 2;
                //Rev25.14/26.01 I.I.の場合、検出器ピッチをI.I.用に変更 by chouno 2017/11/22
                float mdtpitch = 0;			                //データピッチ
                //幾何歪校正ステータスが準備完了の場合コモンのチャンネルピッチを使う
                if ((frmScanControl.Instance.lblStatus[2].Text == StringTable.GC_STS_STANDBY_OK) & (!CTSettings.detectorParam.Use_FlatPanel))
                {
                    mdtpitch = CTSettings.scancondpar.Data.mdtpitch[2];
                }
                //幾何歪み校正ステータスが準備未完了の場合、フラットパネルの場合
                else
                {
                    int iino = modSeqComm.GetIINo();
                    switch (iino)
                    {
                        case 0:
                        case 1:
                        case 2:
                            //II視野ごとの固定値を使う（コモンから取得する）
                            //mdtpitch = CTSettings.scancondpar.Data.detector_pitch[iino];
                            //Rev23.10 変更 by長野 2015/11/04
                            mdtpitch = CTSettings.scancondpar.Data.detector_pitch[ScanCorrect.GFlg_MultiTube + iino * 3];
                            break;
                        default:
                            mdtpitch = 1;
                            break;
                    }
                }
                L_det = (float)(Ile - Ils + 1) * mdtpitch;
            }

            //L_det = (Ile - Ils + 1) * _with14.fpd_pitch; //Rev25.14/26.01 FPDとI.I.で検出器ピッチを切り替える by chouno 2017/11/22
            L_shift = _with14.det_sft_length;                                   //シフト量
            L_shiftL = _with14.det_sft_length_l;                                //シフト量左(mm) //Rev23.20 by長野 2016/01/09
            L_shiftR = _with14.det_sft_length_r;                                //シフト量右(mm) //Rev23.20 by長野 2016/01/09
            L_det_shift = L_det + L_shift;                                      //シフト検出器長
            //Rev25.00 WスキャンONの場合、全モードのスキャンエリアはシフト込みのch数で計算 by長野 2016/08/03
            if (CTSettings.scansel.Data.w_scan == 1)
            {
                L_det = L_det_shift;
            }
            
            L_inv = _with14.fpd_pitch * Convert.ToSingle(_with14.inv_pix);      //無効領域
            //L_margin = (float)(_with14.fpd_pitch * 50.0);                     //ゆとり(mm)
            //Rev23.20 L_marginは0とする。
            //式から推測すると、誤差にしかならないため。
            L_margin = 0;

            //ファン角の端までの長さを１としたときの
            //テーブルY軸位置を割合で表した値
            //Rev23.20 左右シフトの場合わけ by長野 2016/01/09
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
            //Rev25.00 FCD=0.0のときのオーバーフロー防止 by長野 2016/10/31
            if (theFCDFIDRate <= 0.0)
            {
                theFCDFIDRate = -1.0f;
            }
            if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
            {
                theYPosRate = (float)(Math.Abs(theTableYPos) / (L_det_shift / 2.0) / theFCDFIDRate);
            }
            else
            {
                theYPosRate = (float)(Math.Abs(theTableYPos) / (L_det_shift - L_det / 2.0) / theFCDFIDRate);
            }

            //検出器ファン角に収まるY軸の限界値(符号なし)
            //Ymax_fullhalf = (float)((L_det / 2.0 - L_inv) * theFCDFIDRate);     //ハーフ、フルの場合
            //Ymax_offset = (float)((L_det / 2.0 - L_inv) * theFCDFIDRate);       //オフセットの場合
            //Rev23.20 修正 by長野 2016/01/23
            Ymax_fullhalf = (float)((L_det / 2.0) * theFCDFIDRate);     //ハーフ、フルの場合
            Ymax_offset = (float)((L_det / 2.0) * theFCDFIDRate);       //オフセットの場合


            //検出器ファン角に収まるY軸の限界値(符号なし)(シフトの場合)
            //Ymax_unshift_L = (float)((L_det / 2.0 - L_inv) * theFCDFIDRate);    //手前側(－)
            //Ymax_unshift_U = (float)((L_det / 2.0 - L_inv) * theFCDFIDRate);    //奥側(＋)
            //Rev23.20 修正 by長野 2016/01/23
            Ymax_unshift_L = (float)((L_det / 2.0) * theFCDFIDRate);    //手前側(－)
            Ymax_unshift_U = (float)((L_det / 2.0) * theFCDFIDRate);    //奥側(＋)
            
            //Ymax_shift_L = Ymax_unshift_L - (L_shift) * theFCDFIDRate;          //手前側(－)
            //Rev23.20 修正&左右シフトの分岐を追加 by長野 2016/01/23
            //if (CTSettings.scaninh.Data.lr_sft == 0)
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
            if(CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
            {
                Ymax_shift_L = Ymax_unshift_L + (L_shiftL) * theFCDFIDRate;          //手前側(－)
                Ymax_shift_U = Ymax_unshift_U + (L_shiftR) * theFCDFIDRate;          //奥側(＋)
            }
            else
            {
                Ymax_shift_L = Ymax_unshift_L;                                      //手前側(－)
                Ymax_shift_U = Ymax_unshift_U + (L_shift) * theFCDFIDRate;          //奥側(＋)
            }

            //Y軸現在位置を限界値に収める(符号あり)
            //Y_fullhalf = modLibrary.CorrectInRangeFloat(theTableYPos, -Ymax_fullhalf, Ymax_fullhalf);
            //Y_offset = CorrectInRangeFloat(theTableYPos, -Ymax_offset, Ymax_offset);
            Y_fullhalf = modLibrary.CorrectInRange(theTableYPos, -Ymax_fullhalf, Ymax_fullhalf);
            Y_offset = modLibrary.CorrectInRange(theTableYPos, -Ymax_offset, Ymax_offset);

            //Y軸現在位置を限界値に収める(符号あり)(シフトの場合)
            //回転中心はシフト位置のときに映っている必要がある
            //Y_shift = CorrectInRangeFloat(theTableYPos, -Ymax_shift_L, Ymax_shift_U);
            Y_shift = modLibrary.CorrectInRange(theTableYPos, -Ymax_shift_L, Ymax_shift_U);

            //スキャンエリアの計算
            //R_fullhalf = (Ymax_fullhalf - Abs(Y_fullhalf)) * Cos(arctan(L_det / 2# / theFIDWithOffset)) * 2# * LimitFanAngle
            //R_offset = (Ymax_offset + Abs(Y_offset) - L_margin * theYPosRate) * Cos(arctan(L_det / 2# / theFIDWithOffset)) * 2# * LimitFanAngle
            //v19.50 係数変更 by長野 2014/01/28
            R_fullhalf = (float)((Ymax_fullhalf - System.Math.Abs(Y_fullhalf)) * System.Math.Cos(ScanCorrect.arctan(L_det / 2.0 / theFIDWithOffset)) * 2.0 * K);
            R_offset = (float)((Ymax_offset + System.Math.Abs(Y_offset) - L_margin * theYPosRate) * System.Math.Cos(ScanCorrect.arctan(L_det / 2.0 / theFIDWithOffset)) * 2.0 * K);


            //スキャンエリアの計算(シフトの場合)
            //右オフセットと左オフセットに相当する判断が必要だが、判断基準はY軸0mm(画像の中央)ではない。
            //シフトを考慮した検出器長の中央が基準となる。
            //手前側(－)に移動したとき
            //Rev23.20 左右シフト用に対応
            float jdgLength = 0;
            float L_shift_L_distance = CTSettings.scancondpar.Data.det_sft_length_l * FCDFIDRate;
            float L_shift_R_distance = CTSettings.scancondpar.Data.det_sft_length_r * FCDFIDRate;

            //jdgLength = (CTSettings.scaninh.Data.lr_sft == 0 ? 0.0f : (L_shift / 2.0f) * FCDFIDRate);
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07 
            jdgLength = ((CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn) ? 0.0f : (L_shift / 2.0f) * FCDFIDRate);

            //if ((theTableYPos < (L_shift / 2.0) * FCDFIDRate)) 
            //Rev23.20 条件追加 by長野 2016/01/06
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07 
            //if (CTSettings.scaninh.Data.lr_sft == 0)
            if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
            {
                if ((theTableYPos < jdgLength))
                {
                    //奥側の限界値を使う
                    //R_L_shift = (Ymax_shift_U + Abs(Y_shift) - L_margin * theYPosRate) * Cos(arctan((L_det / 2# + L_shift) / theFIDWithOffset)) * 2# * LimitFanAngle
                    //v19.50 係数変更 by長野 2014/01/28
                    //R_L_shift = (float)((Ymax_shift_U + System.Math.Abs(Y_shift) - L_margin * theYPosRate) * System.Math.Cos(ScanCorrect.arctan((L_det / 2.0 + L_shift) / theFIDWithOffset)) * 2.0 * K);
                    //Rev23.20 左右シフト対応 by長野 2016/01/09
                    R_L_shift = (float)((Ymax_shift_U + System.Math.Abs(Y_shift) - L_margin * theYPosRate) * System.Math.Cos(ScanCorrect.arctan((L_det / 2.0 + L_shiftR) / theFIDWithOffset)) * 2.0 * K);
                    R_shift = R_L_shift;
                    Ymax_shift = Ymax_shift_L;                //奥側(＋)に移動したとき
                }
                else
                {
                    //手前側の限界値を使う
                    //R_U_shift = (Ymax_unshift_L + Abs(Y_shift) - L_margin * theYPosRate) * Cos(arctan(L_det / 2# / theFIDWithOffset)) * 2# * LimitFanAngle
                    //v19.50 係数変更 by長野 2014/01/28
                    //R_U_shift = (float)((Ymax_unshift_L - addLRshift + System.Math.Abs(Y_shift) - L_margin * theYPosRate) * System.Math.Cos(ScanCorrect.arctan(L_det / 2.0 / theFIDWithOffset)) * 2.0 * K);
                    //Rev23.20 左右シフト対応 by長野 2016/01/09 
                    R_U_shift = (float)((Ymax_shift_L + System.Math.Abs(Y_shift) - L_margin * theYPosRate) * System.Math.Cos(ScanCorrect.arctan((L_det / 2.0 + L_shiftL) / theFIDWithOffset)) * 2.0 * K);
                    R_shift = R_U_shift;
                    Ymax_shift = Ymax_shift_U;
                }
            }
            else
            {
                if ((theTableYPos < jdgLength))
                {
                    //奥側の限界値を使う
                    //R_L_shift = (Ymax_shift_U + Abs(Y_shift) - L_margin * theYPosRate) * Cos(arctan((L_det / 2# + L_shift) / theFIDWithOffset)) * 2# * LimitFanAngle
                    //v19.50 係数変更 by長野 2014/01/28
                    R_L_shift = (float)((Ymax_shift_U + System.Math.Abs(Y_shift) - L_margin * theYPosRate) * System.Math.Cos(ScanCorrect.arctan((L_det / 2.0 + L_shift) / theFIDWithOffset)) * 2.0 * K);
                    R_shift = R_L_shift;
                    Ymax_shift = Ymax_shift_L;                //奥側(＋)に移動したとき
                }
                else
                {
                    //手前側の限界値を使う
                    //R_U_shift = (Ymax_unshift_L + Abs(Y_shift) - L_margin * theYPosRate) * Cos(arctan(L_det / 2# / theFIDWithOffset)) * 2# * LimitFanAngle
                    //v19.50 係数変更 by長野 2014/01/28
                    R_U_shift = (float)((Ymax_unshift_L + System.Math.Abs(Y_shift) - L_margin * theYPosRate) * System.Math.Cos(ScanCorrect.arctan(L_det / 2.0 / theFIDWithOffset)) * 2.0 * K);
                    R_shift = R_U_shift;
                    Ymax_shift = Ymax_shift_U;
                }
            }

            //スキャンエリアを表示
            ntbFullHalf.Value = (R_fullhalf < 0 ? 0 : (decimal)R_fullhalf);
            ntbOffset.Value = (R_offset < 0 ? 0 : (decimal)R_offset);
            ntbShift.Value = (R_shift < 0 ? 0 : (decimal)R_shift);

            //Rev23.30 各スキャンエリアが確定後、メカ制御画面のスキャン条件画面に選択中のスキャンモード・スキャンエリア・1画素サイズを表示 by長野 2016/02/05
            float matpix = (float)(256 * Math.Pow(2,CTSettings.scansel.Data.matrix_size - 1));

            //float flblScanArea = 0.0f;
            //float flblPixSize = 0.0f;
            flblScanArea = 0.0f;
            flblPixSize = 0.0f;
 
            string strScanMode = "";

            
            //Rev25.00 Wスキャンで場合わけ追加 by長野 2016/08/03
            if (CTSettings.scansel.Data.w_scan == 1)
            {
                switch (CTSettings.scansel.Data.scan_mode)
                {
                    case (int)ScanSel.ScanModeConstants.ScanModeHalf:

                        strScanMode = CTResources.LoadResString(25003);
                        flblScanArea = (R_fullhalf < 0 ? 0 : R_fullhalf);
                        flblPixSize = flblScanArea / matpix;

                        break;
                    case (int)ScanSel.ScanModeConstants.ScanModeFull:

                        strScanMode = CTResources.LoadResString(25004);
                        flblScanArea = (R_fullhalf < 0 ? 0 : R_fullhalf);
                        flblPixSize = flblScanArea / matpix;

                        break;
                    case (int)ScanSel.ScanModeConstants.ScanModeOffset:

                        strScanMode = CTResources.LoadResString(25005);
                        flblScanArea = (R_offset < 0 ? 0 : R_offset);
                        flblPixSize = flblScanArea / matpix;

                        break;
                    case (int)ScanSel.ScanModeConstants.ScanModeShift:

                        strScanMode = CTResources.LoadResString(17518);
                        flblScanArea = (R_shift < 0 ? 0 : R_shift);
                        flblPixSize = flblScanArea / matpix;
                        break;
                    default:

                        strScanMode = CTResources.LoadResString(12315);
                        flblScanArea = (R_fullhalf < 0 ? 0 : R_fullhalf);
                        flblPixSize = flblScanArea / matpix;

                        break;
                }
            }
            else
            {
                switch (CTSettings.scansel.Data.scan_mode)
                {
                    case (int)ScanSel.ScanModeConstants.ScanModeHalf:

                        strScanMode = CTResources.LoadResString(12315);
                        flblScanArea = (R_fullhalf < 0 ? 0 : R_fullhalf);
                        flblPixSize = flblScanArea / matpix;

                        break;
                    case (int)ScanSel.ScanModeConstants.ScanModeFull:

                        strScanMode = CTResources.LoadResString(12316);
                        flblScanArea = (R_fullhalf < 0 ? 0 : R_fullhalf);
                        flblPixSize = flblScanArea / matpix;

                        break;
                    case (int)ScanSel.ScanModeConstants.ScanModeOffset:

                        strScanMode = CTResources.LoadResString(12313);
                        flblScanArea = (R_offset < 0 ? 0 : R_offset);
                        flblPixSize = flblScanArea / matpix;

                        break;
                    case (int)ScanSel.ScanModeConstants.ScanModeShift:

                        strScanMode = CTResources.LoadResString(17518);
                        flblScanArea = (R_shift < 0 ? 0 : R_shift);
                        flblPixSize = flblScanArea / matpix;
                        break;
                    default:

                        strScanMode = CTResources.LoadResString(12315);
                        flblScanArea = (R_fullhalf < 0 ? 0 : R_fullhalf);
                        flblPixSize = flblScanArea / matpix;

                        break;
                }
            }

            lblSelectedScanMode.Text = strScanMode;
            lblScanAreaNum.Text = (flblScanArea > 0? flblScanArea.ToString("##0.####"):"-");
            lblPixSizeNum.Text = (flblPixSize > 0 ? flblPixSize.ToString("##0.#####") : "-");

            //lblScanAreaNum.Text = String.Format("{0:f7}",flblScanArea);
            //lblPixSizeNum.Text = String.Format("{0:f7}", flblPixSize);
            
            
            //状態によって背景色を変える
            //Y軸が限界に近い時は背景色を黄色で表示する
            //Y軸が限界を超えた時は背景色を暗色で表示する
            switch (CTSettings.scansel.Data.scan_mode)
            {
                case 1:
                case 2:
                    //ハーフ、フル
                    //ntbFullHalf.BackColor = vbWhite    'v18.00変更 byやまおか 2011/07/04
                    ntbFullHalf.BackColor = SetColorFromPos(System.Math.Abs(theTableYPos), Ymax_fullhalf, 0.9F);
                    ntbFullHalf.TextBackColor = SetColorFromPos(System.Math.Abs(theTableYPos), Ymax_fullhalf, 0.9F);//Rev23.20 追加 by長野 2015/11/20
                    
                    ntbOffset.BackColor = System.Drawing.SystemColors.Control;
                    ntbOffset.TextBackColor = System.Drawing.SystemColors.Control;//Rev23.20 追加 by長野 2015/11/20
                    
                    ntbShift.BackColor = System.Drawing.SystemColors.Control;
                    ntbShift.TextBackColor = System.Drawing.SystemColors.Control;//Rev23.20 追加 by長野 2015/11/20
                    
                    break;
                case 3:
                    //オフセット
                    ntbFullHalf.BackColor = System.Drawing.SystemColors.Control;
                    ntbFullHalf.TextBackColor = System.Drawing.SystemColors.Control;//Rev23.20 追加 by長野 2015/11/20

                    //ntbOffset.BackColor = vbWhite      'v18.00変更 byやまおか 2011/07/04
                    ntbOffset.BackColor = SetColorFromPos(System.Math.Abs(theTableYPos), Ymax_offset, 0.8f); //Rev23.20 0.9->0.8変更 by長野 2016/01/24
                    ntbOffset.TextBackColor = SetColorFromPos(System.Math.Abs(theTableYPos), Ymax_offset, 0.8f);//Rev23.20 追加 by長野 2015/11/20 //Rev23.20 0.9->0.8変更 by長野 2016/01/24
                   
                    ntbShift.BackColor = System.Drawing.SystemColors.Control;
                    ntbShift.TextBackColor = System.Drawing.SystemColors.Control;//Rev23.20 追加 by長野 2015/11/20
                    
                    break;
                case 4:
                    //シフト
                    ntbFullHalf.BackColor = System.Drawing.SystemColors.Control;
                    ntbFullHalf.TextBackColor = System.Drawing.SystemColors.Control;//Rev23.20 追加 by長野 2015/11/20

                    ntbOffset.BackColor = System.Drawing.SystemColors.Control;
                    ntbOffset.TextBackColor = System.Drawing.SystemColors.Control;//Rev23.20 追加 by長野 2015/11/20

                    //ntbShift.BackColor = vbWhite       'v18.00変更 byやまおか 2011/07/04
                    ntbShift.BackColor = SetColorFromPos(System.Math.Abs(theTableYPos), Ymax_shift, 0.8F); //Rev23.20 0.9->0.8変更 by長野 2016/01/24
                    ntbShift.TextBackColor = SetColorFromPos(System.Math.Abs(theTableYPos), Ymax_shift, 0.8F);//Rev23.20 追加 by長野 2015/11/20 //Rev23.20 0.9->0.8変更 by長野 2016/01/24

                    break;
                default:
                    ntbFullHalf.BackColor = System.Drawing.SystemColors.Control;
                    ntbFullHalf.TextBackColor = System.Drawing.SystemColors.Control;

                    ntbOffset.BackColor = System.Drawing.SystemColors.Control;
                    ntbOffset.TextBackColor = System.Drawing.SystemColors.Control;

                    ntbShift.BackColor = System.Drawing.SystemColors.Control;
                    ntbShift.TextBackColor = System.Drawing.SystemColors.Control;

                    break;
            }

        }

        //追加2014/10/07hata_v19.51反映
        //*************************************************************************************************
        //機　　能： 位置と限界値によって色を返す関数
        //
        //           変数名          [I/O] 型        内容
        //引　　数： thePos          Single          位置
        //           theLimit        Single          限界値
        //           theRate         Single          限界の手前
        //
        //戻 り 値： SetColorFromPos ColorConstants  色
        //
        //補　　足： なし
        //
        //履　　歴： v18.00  2011/07/04  やまおか    新規作成
        //*************************************************************************************************
        private Color SetColorFromPos(float thePos, float theLimit, float theRate = 0.95F)
        {
            Color functionReturnValue = default(System.Drawing.Color);
            
            //制限値より少し手前
            if ((thePos > theLimit * theRate) & (thePos <= theLimit))
            {
                //黄色
                functionReturnValue = Color.Yellow;
           
            //制限値を超えた
            }
            else if ((thePos > theLimit))
            {
                //暗灰色
                functionReturnValue = modCT30K.DarkGray;

            //それ以外
            }
            else
            {
                //白色
                functionReturnValue = Color.White;
            }
            return functionReturnValue;

        }
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここまで

        
        //*************************************************************************************************
        //機　　能： 英語版のコントロール配置
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 各項目をフレームで纏めた
        //           fraMechaPos        :機構部位置
        //           fraUpDown          :昇降操作
        //           fraMechaControl    :機構部操作
        //           fraHighSpeedCamera :CT/高速切替
        //           fraCollimator      :コリメータ
        //           fraXrayRotate      :X線管操作
        //           fraAutoSacnPos     :自動スキャン位置指定
        //           fraIris            :I.I.絞り
        //           cmdDetails         :詳細ボタン
        //
        //履　　歴： v17.60 2011/05/24 (検S1)長野   新規作成
        //*************************************************************************************************
        public void EnglishAdjustLayout()
        {
            //const int margin = 2;      //余白
            int num = 0;
            int i = 0; 
    
            //int tmppos = 0; 
            //int xbase = 0; 
            //int ybase = 0; 
            int mechaposWidth = 0;      //fraMechaPos内のnumtextboxのwidth
            int mechaposCapWidth = 0;   //fraMechaPos内のnumtextboxのcapwidth
            mechaposWidth = 187;
            mechaposCapWidth = 87;
            
            //使用していない？
            //xbase = 8;
            //ybase = 0;

            //Rev20.01 追加 by長野 2015/05/19
            //自動スキャン位置指定フレーム調整
            //*************************************************************************************************
            cmdFromSlice.Left = cmdFromSlice.Left ;
            cmdFromSlice.Width = cmdFromSlice.Width + 7;
            cmdFromTrans.Left = cmdFromTrans.Left - 2;
            cmdFromTrans.Width = cmdFromTrans.Width + 7;
          
            //*************************************************************************************************

    
            //機構部位置フレーム調整
            //*************************************************************************************************
            num = cboSpeed.Length;
            for (i = 0; i < num; i++)
            {
                if (i != 5)
                {
                    cboSpeed[i].Left = fraMechaPos.Width - cboSpeed[i].Width;
                }
            }

            ntbRotate.Width = mechaposWidth;
            ntbRotate.CaptionWidth = mechaposCapWidth;
            ntbUpDown.Width = mechaposWidth;
            ntbUpDown.CaptionWidth = mechaposCapWidth;
            ntbFID.Width = mechaposWidth;
            ntbFID.CaptionWidth = mechaposCapWidth;
            ntbFCD.Width = mechaposWidth;
            ntbFCD.CaptionWidth = mechaposCapWidth;
            ntbTableXPos.Width = mechaposWidth;
            ntbTableXPos.CaptionWidth = mechaposCapWidth;
            ntbXrayRotPos.Width = mechaposWidth;
            ntbXrayRotPos.CaptionWidth = mechaposCapWidth;
            ntbXrayPosX.Width = mechaposWidth;
            ntbXrayPosX.CaptionWidth = mechaposCapWidth;
            ntbXrayPosY.Width = mechaposWidth;
            ntbXrayPosY.CaptionWidth = mechaposCapWidth;
            ntbFTablePosX.Width = mechaposWidth;
            ntbFTablePosX.CaptionWidth = mechaposCapWidth;
            ntbFTablePosY.Width = mechaposWidth;
            ntbFTablePosY.CaptionWidth = mechaposCapWidth;

            ntbTilt.CaptionWidth = mechaposCapWidth;    //Rev22.00 回転傾斜テーブル 追加 by長野 2015/08/20
            ntbTilt.Width = mechaposWidth;              //Rev22.00 回転傾斜テーブル 追加 by長野 2015/08/20
            ntbTiltRot.CaptionWidth = mechaposCapWidth; //Rev22.00 回転傾斜テーブル 追加 by長野 2015/08/20
            ntbTiltRot.Width = mechaposWidth;           //Rev22.00 回転傾斜テーブル 追加 by長野 2015/08/20
    
            //昇降操作コントロールフレーム調整
            //*************************************************************************************************
            fraUpDown.Left = fraMechaPos.Left + fraMechaPos.Width;
    
            //機構部操作フレーム調整
            //*************************************************************************************************
            fraMechaControl.Left = fraUpDown.Left + fraUpDown.Width;
            //Rev20.01 追加 by長野 2015/05/19
            cmdPosExec.Height = cmdPosExec.Height + 7;
            cmdPosExec.Top = cmdPosExec.Top -3;
    
            //X線管操作フレーム調整
            //*************************************************************************************************
            //2014/11/07hata キャストの修正
            //fraXrayRotate.Left = fraCollimator.Left + (fraCollimator.Width - fraXrayRotate.Width) / 2;
            fraXrayRotate.Left = fraCollimator.Left + Convert.ToInt32((fraCollimator.Width - fraXrayRotate.Width) / 2F);
    
            //コリメータフレーム調整
            //*************************************************************************************************
            fraCollimator.Left = fraMechaControl.Left + fraMechaControl.Width;
    
            num = cmdCollimator.Length;
            //2014/11/07hata キャストの修正
            //fraCollimator.Width = frmMechaControl..ScaleWidth - fraMechaControl.Left - fraMechaControl.Width;
            fraCollimator.Width = frmMechaControl.Instance.ClientSize.Width - fraMechaControl.Left - fraMechaControl.Width;
            for (i = 0; i <= num - 1; i++)
            {

                cmdCollimator[i].Width = 38;
                //cmdCollimator[i].fontSize = 8;
                Font a = cmdCollimator[i].Font;
                //Rev20.01 変更 by長野 2015/05/19
                //cmdCollimator[i].Font = new Font(a.Name,8);
                cmdCollimator[i].Font = new Font(a.Name, (float)6.5);

                if (i <= 3)
                {
                    //2014/11/07hata キャストの修正
                    //cmdCollimator[i].Left = (i + 1) * ((fraCollimator.Width - cmdCollimator[i].Width * 4) / 5) + i * cmdCollimator[i].Width;
                    cmdCollimator[i].Left = Convert.ToInt32((i + 1) * ((fraCollimator.Width - cmdCollimator[i].Width * 4) / 5F) + i * cmdCollimator[i].Width);
                }
                else
                {
                    //2014/11/07hata キャストの修正
                    //cmdCollimator[i].Left = (fraCollimator.Width - cmdCollimator[i].Width) / 2;
                    cmdCollimator[i].Left = Convert.ToInt32((fraCollimator.Width - cmdCollimator[i].Width) / 2F);
                }
            }
    
            //短残光I.I.絞り 'X線管操作と同じ位置
            //*************************************************************************************************
            fraIris.Left = fraCollimator.Left;
            fraIris.Width = fraCollimator.Width;
    
            num = cmdCollimator.Length;
            for (i = 0; i <= num - 1; i++)
            {
                cmdIris[i].Width = 37;
                //cmdIris[i].fontSize = 8;
                Font a = cmdIris[i].Font;
                cmdIris[i].Font = new Font(a.Name, 8);

                if (i <= 3)
                {
                    //2014/11/07hata キャストの修正
                    //cmdIris[i].Left = (i + 1) * ((fraCollimator.Width - cmdIris[i].Width * 4) / 5) + i * cmdIris[i].Width;
                    cmdIris[i].Left = Convert.ToInt32((i + 1) * ((fraCollimator.Width - cmdIris[i].Width * 4) / 5F) + i * cmdIris[i].Width);
                }
                else
                {
                    //2014/11/07hata キャストの修正
                    //cmdIris[i].Left = (fraCollimator.Width - cmdIris[i].Width) / 2;
                    cmdIris[i].Left = Convert.ToInt32((fraCollimator.Width - cmdIris[i].Width) / 2F);
                }
            }
        }


        //X線MechData更新
        private void SeqCommError()
        {
            if (InvokeRequired)
            {
                //Invoke(new MechErrorDelegate(SeqCommError));
                BeginInvoke(new MechErrorDelegate(SeqCommError));
                return;
            }

            //メッセージ出力用
            string strMsg = null;   //表示ﾒｯｾｰｼﾞ
            strMsg = "";

            if (UC_SeqComm_OnCommEnd_BUSYNOW) return;   //処理中なら実行しない
            UC_SeqComm_OnCommEnd_BUSYNOW = true;

            try
            {
                //追加2015/02/10hata　---ここから---
                //Debug.WriteLine("MechErrorNo = " + MechErrorNo.ToString());

                ////メカ動作中なら止める
                //if (CTSettings.mecainf.Data.ud_busy == 1)
                //{
                //    modMechaControl.MechaUdStop();      //昇降終了
                //}
                //if (CTSettings.mecainf.Data.rot_busy == 1)
                //{
                //    modMechaControl.MechaRotateStop();  //回転終了
                //}
                //if (CTSettings.mecainf.Data.xstg_busy == 1)
                //{
                //    modMechaControl.MechaXStgStop();    //微調X終了
                //}
                //if (CTSettings.mecainf.Data.ystg_busy == 1)
                //{
                //    modMechaControl.MechaYStgStop();    //微調Y終了
                //}

                //同じエラーNoの場合は、5秒間空ける
                if ((SeqComm_Error_BeforNo != MechErrorNo) | (SeqComm_Error_Time == null))
                {
                    //すぐに表示
                    SeqComm_Error_BeforNo = MechErrorNo;
                    SeqComm_Error_Time = DateTime.Now;	//受け取った時間を格納
                }
                else
                {
                    if ((DateTime.Now - SeqComm_Error_Time.Value).TotalSeconds < SeqComm_Error_WaitTime)
                    {
                        //時間待ちする
                        UC_SeqComm_OnCommEnd_BUSYNOW = false;
                        return;
                    }
                    SeqComm_Error_Time = DateTime.Now;	//受け取った時間を格納
                
                }
                //追加2015/02/10hata　---ここまで---
                
                
                //ActiveX制御ｴﾗｰの内容を表示
                switch (MechErrorNo)
                {
                    //異常
                    case 700:
                    case 701:
                    case 702:
                    case 703:
                    case 704:
                    case 710:
                    case 715:
                    case 716:
                    case 720:
                    case 721:
                        //エラーメッセージ
                        //   700:コミュニケーションエラーが発生しました。
                        //   701:通信準備接続エラーが発生しました。
                        //   702:VB通信エラーが発生しました。
                        //   703:通信タイムアウトエラーが発生しました。
                        //   704:シーケンサコマンド処理エラーが発生しました。
                        //   710:読出しエラーが発生しました。
                        //   715:ビット書込みエラーが発生しました。
                        //   716:ビット書込みディバイスネームエラーが発生しました。
                        //   720:ワード書込みエラーが発生しました。
  
                      //   721:ワード書込みディバイスネームエラーが発生しました。


//#if !(DEBUG || DebugOn)   //変更2015/02/09hata
#if (!DEBUG || (DEBUG && !SeqCommErrorDispOff))

                        string dd = CTResources.LoadResString(MechErrorNo);
                        strMsg = modLibrary.GetFirstItem(dd, "@"); //取得したリソース文字列中の@マーク以降は無視する
                        //strMsg = modLibrary.GetFirstItem(CTResources.LoadResString(CommEndAns), "@"); //取得したリソース文字列中の@マーク以降は無視する
#endif
                        break;

                    //未定義のエラー
                    default:
                        strMsg = CTResources.LoadResString(StringTable.IDS_UnkownError);      //予想外のエラーが発生しました。
                        break;
                }

                //追加2015/02/10hata
                this.Cursor = Cursors.Default;

            }
            catch
            {
                // Nothing
            }

            try
            {
                if (!string.IsNullOrEmpty(strMsg))
                {
                    //追加2015/03/11hata
                    //スキャン中はエラーメッセージを表示しない
                    if (!Convert.ToBoolean(modCTBusy.CTBusy & modCTBusy.CTScanStart))
                    {
                        //ｴﾗｰ表示
                        //変更2015/03/11hata_オーナーForm指定を追加
                        DialogResult result = MessageBox.Show(frmCTMenu.Instance, CTResources.LoadResString(StringTable.IDS_ErrorNum) + MechErrorNo.ToString() + "\r\n" + "\r\n"
                                                            + strMsg + "\r\n"
                                                            + CTResources.LoadResString(9902),
                                                            Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);  //リソース対応 2003/07/31 by 間々田

                        if (result == DialogResult.Yes)
                        {
                            //End
                            Environment.Exit(0);

                            //下記に変更 by 間々田 2005/01/18 メインフォームをアンロードする（CT30kが起動させたSeqComm.exeなども終了させるため）
                            //frmCTMenu.Instance.Close();
                        }

                        //追加2015/02/10hata
                        SeqComm_Error_Time = DateTime.Now;	//受け取った時間を格納
                    }
                }
            }
            catch
            {
                // Nothing
                UC_SeqComm_OnCommEnd_BUSYNOW = false;

            }

            //元の状態に戻す
            UC_SeqComm_OnCommEnd_BUSYNOW = false;
        }


        //追加2014/05hata
        private void frmMechaControl_Activated(object sender, EventArgs e)
        {
            ////描画を強制する
            //if (this.Visible && this.Enabled) this.Refresh();
        }

        //追加2014/12/22hata_三角のポインターを描画
        //*************************************************************************************************
        //機　　能： 昇降スライダの現在位置ポインターの表示処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        //全面変更2015/02/12hata_スライダー新規
        //private void DispUpDownPointer(decimal? pos = null)
        //{
        //   //pos現在位置
        //    if (pos == null) pos = stsUpDownPos;

        //    //移動中のPointer位置を表示
        //    int offset = 13;
        //    float MoveSize = picPointer.Parent.Height - offset * 2;
        //    //変更2015/02/02hata
        //    //int ypos = Convert.ToInt32(((float)ntbUpDown.Value / (cwsldUpDown[0].Maximum - cwsldUpDown[0].Minimum)) * MoveSize);
        //    //decimal pos0 = modLibrary.CorrectInRange(ntbUpDown.Value, cwsldUpDown[0].Minimum, cwsldUpDown[0].Maximum);
        //    decimal pos0 = ntbUpDown.Value;
        //    int ypos = Convert.ToInt32(((float)pos0 / (cwsldUpDown[0].Maximum - cwsldUpDown[0].Minimum)) * MoveSize);
        //    int ypos1;

        //    if (cwsldUpDown[0].Reverse)
        //    {
        //        //TopがMinのとき    
        //        ypos1 = ypos - Convert.ToInt32((picPointer.Height) / 2F) + offset;
        //    }
        //    else
        //    {
        //        //TopがMaxのとき    
        //        ypos1 = picPointer.Parent.Height - ypos - Convert.ToInt32((picPointer.Height) / 2F) - offset;
        //    }
        //    picPointer.Top = ypos1;
 
        //    picPointer.BackColor = Color.Transparent;
        //    if (!modCT30K.CT30KSetup)
        //    {
        //        picPointer.Visible = false;
        //        return;
        //    }
 
        //    //ｽﾗｲﾀﾞが現在位置と違うとき/移動中のとき/cmdPosExecがLimeのときにポインターを表示
        //    //変更2015/02/02Hata
        //    //if ((cwsldUpDown[0].Value != pos) & (cwsldUpDown[0].Maximum >= cwsldUpDown[0].Value) & (cwsldUpDown[0].Minimum <= cwsldUpDown[0].Value) & (CTSettings.mecainf.Data.ud_busy != 1) & mousecapture)
        //    if ((cwsldUpDown[0].Value != pos) & (CTSettings.mecainf.Data.ud_busy != 1) & mousecapture)
        //    {
        //        //設定中　現在位置がMaxかMinの外にあるとき
        //        picPointer.Visible = true;
        //    }
        //    else if(cmdPosExec.BackColor == Color.Lime)
        //    {
        //        picPointer.Visible = true;
        //    }            
        //    else
        //    {
        //        //設定も移動も無し
        //        picPointer.Visible = false;
        //        mousecapture = picPointer.Visible;
        //    }

        //    ////追加2015/01/30hata
        //    //if (ntbUpDown.Value == cwsldUpDown[0].Value)
        //    //{
        //    //    //指定位置に達した
        //    //    picPointer.Visible = false;
        //    //    mousecapture = picPointer.Visible;
        //    //}

        //}        
        private void DispUpDownPointer(decimal? pos = null)
        {
            //pos現在位置
            if (pos == null) pos = stsUpDownPos;
            cwsldUpDown[0].ArrowValue = (decimal)pos;

            if (!modCT30K.CT30KSetup)
            {
                cwsldUpDown[0].ArrowVisible = false;
                return;
            }

            //ｽﾗｲﾀﾞが現在位置と違うとき/移動中のとき/cmdPosExecがLimeのときにポインターを表示
            if ((cwsldUpDown[0].Value != pos) & (CTSettings.mecainf.Data.ud_busy != 1) & mousecapture)
            {
                //設定中　現在位置がMaxかMinの外にあるとき
                cwsldUpDown[0].ArrowVisible = true;
            }
            else if (cmdPosExec.BackColor == Color.Lime)
            {
                cwsldUpDown[0].ArrowVisible = true;
            }
            else
            {
                //設定も移動も無し
                cwsldUpDown[0].ArrowVisible = false;
                mousecapture = cwsldUpDown[0].ArrowVisible;
            }
        }        


        //追加2014/12/22hata_三角のポインターを描画
        private void picPointer_Paint(object sender, PaintEventArgs e)
        {
            //変更2015/02/12hata_スライダー新規
            ////三角のポインターを描画
            //Point[] points = new Point[3];
            //int midH = Convert.ToInt32(picPointer.Height / 2F) - 1;
            //points[0] = new Point(0, 0);
            //points[1] = new Point(picPointer.Width - 1, midH);
            //points[2] = new Point(0, picPointer.Height - 1);

            //e.Graphics.FillPolygon(Brushes.Yellow, points);
            //e.Graphics.DrawPolygon(Pens.Goldenrod, points);
        }

        //追加2014/12/22hata
        private void txtUpDownPos_TextChanged(object sender, EventArgs e)
        {
            decimal dval = 0;

            //変更2015/01/20hata
            //if (!decimal.TryParse(txtUpDownPos.Text, out dval))
            //{
            //    txtUpDownPos.Text = cwnePos.Value.ToString("0.000");
            //    presskye = (char)0;
            //    return;
            //}
            //if (cwnePos.Maximum < dval)
            //{
            //    dval = cwnePos.Maximum;
            //}
            //if (cwnePos.Minimum > dval)
            //{
            //    dval = cwnePos.Minimum;
            //}

            //if (presskye != (char)Keys.Return)
            //{
            //    txtUpDownPos.Text = dval.ToString();
            //}
            //else
            //{
            //    if (cwnePos.Value == dval) txtUpDownPos.Text = dval.ToString("0.000");
            //    cwnePos.Value = dval;
            //}
            if (presskye == (char)Keys.Return)
            {
                if (!decimal.TryParse(txtUpDownPos.Text, out dval))
                {
                    if (string.IsNullOrEmpty(PreUpDownValText)) PreUpDownValText = cwnePos.Value.ToString("0.000");
                    txtUpDownPos.Text = PreUpDownValText;
                    presskye = (char)0;
                    return;
                }

                if (cwnePos.Maximum < dval)
                {
                    dval = cwnePos.Maximum;
                }
                if (cwnePos.Minimum > dval)
                {
                    dval = cwnePos.Minimum;
                }

                if (cwnePos.Value == dval) txtUpDownPos.Text = dval.ToString("0.000");
                cwnePos.Value = dval;

            }
            else
            {
                if (!decimal.TryParse(txtUpDownPos.Text, out dval))
                {
                    if (string.IsNullOrEmpty(PreUpDownValText)) PreUpDownValText = cwnePos.Value.ToString("0.000");
                    if (txtUpDownPos.Text != "") txtUpDownPos.Text = PreUpDownValText;
                    presskye = (char)0;
                    return;
                }
            }
            PreUpDownValText = dval.ToString("0.000"); ;
           
        }

        //追加2014/12/22hata
        private void txtUpDownPos_KeyPress(object sender, KeyPressEventArgs e)
        {
            presskye = e.KeyChar;
            switch (e.KeyChar)
            {
                //数字キーと削除キー
                case (char)Keys.D0:
                case (char)Keys.D1:
                case (char)Keys.D2:
                case (char)Keys.D3:
                case (char)Keys.D4:
                case (char)Keys.D5:
                case (char)Keys.D6:
                case (char)Keys.D7:
                case (char)Keys.D8:
                case (char)Keys.D9:
                case (char)Keys.Back:
                case (char)46:          //追加2015/01/20hata
                    break;
                case (char)Keys.Return:
                    txtUpDownPos_TextChanged(sender, EventArgs.Empty);
                    break;
                default:
                    e.KeyChar = (char)0;
                    e.Handled = true;
                    break;
            }
        }

        //追加2014/12/22hata
        //UpDownキーで動作させる
        private void txtUpDownPos_KeyDown(object sender, KeyEventArgs e)
        {
            presskye = (char)e.KeyCode;
            switch (e.KeyCode)
            {
                //UpDownキー
                case Keys.Up:
                    cwnePos.UpButton();

                    break;
                case Keys.Down:
                    cwnePos.DownButton();

                    break;
            }
        }

        //追加2014/12/22hata
        private void txtUpDownPos_Leave(object sender, EventArgs e)
        {
            if (txtUpDownPos.Text == "") txtUpDownPos.Text = cwnePos.Value.ToString();
            presskye = (char)Keys.Return;
            txtUpDownPos_TextChanged(sender, EventArgs.Empty);
        }

        //追加2015/01/08hata
        private void cwsldUpDown_MouseDown(object sender, MouseEventArgs e)
        {
            mousecapture = true;
        }
        //追加2015/01/08hata
        private void cwsldUpDown_MouseUp(object sender, MouseEventArgs e)
        {
            mousecapture = false;
        }

        //追加2015/01/19hata
        private void cwbtnMove_MouseUp(object sender, MouseEventArgs e)
        {
            //
            //MySeq.stsXRLimit; //テーブルＸ左限
            //MySeq.stsXLLimit; //テーブルＸ右限
            //MySeq.stsYFLimit; //テーブルＹ前進限（拡大限）
            //MySeq.stsYBLimit; //テーブルＹ後退限（縮小限）
            //

            if (sender as CWButton == null) return;

            int Index = Array.IndexOf(cwbtnMove, sender);

            if (Index < 0) return;

            if (Index == 0)
                SetLimitStatus(cwbtnMove[0], "Left", modSeqComm.MySeq.stsXLLimit);
            else if (Index == 1)
                SetLimitStatus(cwbtnMove[1], "Right", modSeqComm.MySeq.stsXRLimit);
            else if (Index == 2)
                SetLimitStatus(cwbtnMove[2], "Up", modSeqComm.MySeq.stsYFLimit);
            else if (Index == 3)
                SetLimitStatus(cwbtnMove[3], "Down", modSeqComm.MySeq.stsYBLimit);

        }

        //追加2015/02/10hata
        private void cwbtnMove_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (sender as CWButton == null) return;
            int Index = Array.IndexOf(cwbtnMove, sender);
            if (Index < 0) return;

            if (!cwbtnMove[Index].Capture)
            {
                cwbtnMove[Index].Value = false;
            }
        }

        //追加2015/02/10hata
        private void cwbtnRotate_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (sender as CWButton == null) return;
            int Index = Array.IndexOf(cwbtnRotate, sender);
            if (Index < 0) return;

            if (!cwbtnRotate[Index].Capture)
            {
                cwbtnRotate[Index].Value = false;
            }
        }

        //追加2015/02/10hata
        private void cwbtnFineTable_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (sender as CWButton == null) return;
            int Index = Array.IndexOf(cwbtnFineTable, sender);
            if (Index < 0) return;

            if (!cwbtnFineTable[Index].Capture)
            {
                cwbtnFineTable[Index].Value = false;
            }
        }

        //追加2015/02/10hata
        private void cwbtnRotateXray_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (sender as CWButton == null) return;
            int Index = Array.IndexOf(cwbtnRotateXray, sender);
            if (Index < 0) return;

            if (!cwbtnRotateXray[Index].Capture)
            {
                cwbtnRotateXray[Index].Value = false;
            }
        }

        //Rev22.00 回転傾斜テーブル用 追加 by長野 2015/08/20
        private void cwbtnTiltAndRot_Tilt_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (sender as CWButton == null) return;
            int Index = Array.IndexOf(cwbtnTiltAndRot_Tilt, sender);
            if (Index < 0) return;

            if (!cwbtnTiltAndRot_Tilt[Index].Capture)
            {
                cwbtnTiltAndRot_Tilt[Index].Value = false;
            }
        }

        //Rev22.00 回転傾斜テーブル用 追加 by長野 2015/08/20
        private void cwbtnTiltAndRot_Rot_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (sender as CWButton == null) return;
            int Index = Array.IndexOf(cwbtnTiltAndRot_Rot, sender);
            if (Index < 0) return;

            if (!cwbtnTiltAndRot_Rot[Index].Capture)
            {
                cwbtnTiltAndRot_Rot[Index].Value = false;
            }
        }

        //*************************************************************************************************
        //機　　能： X線切替ボタンクリック時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v23.10  2015/09/18 (検S1)長野  新規作成
        //*************************************************************************************************
        private void cwbtnChangeXray_Click(object sender, MouseEventArgs e)
        {
            if (sender as CWButton == null) return;

            int Index = Array.IndexOf(cwbtnChangeXray, sender);

            if (Index < 0) return;

            if (!modSeqComm.CheckFCD2(ScanCorrect.GVal_Fcd))
            {
                return;
            }

            mod2ndXray.XrayModeConstants theMode = default(mod2ndXray.XrayModeConstants);

            //追加2014/10/07hata_v19.51反映
            //機構部動作が可能かチェック（上記を関数化）
            //v18.00追加 byやまおか 2011/02/19 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (!modMechaControl.IsOkMechaMove())
                return;

            //既にボタンが有効になっており(X線が正常な位置にある)，
            //コモンも選択したX線用になっている場合，何もせずに抜ける。
            if (cwbtnChangeXray[Index].Value == true & CTSettings.scansel.Data.multi_tube == Index)
            {
                return;
            }

            //切り替え可否チェック
            if (!mod2ndXray.IsChangeXray())
            {
                //MsgBox LoadResString(IDS_AlignmentNow), vbInformation
                //Interaction.MsgBox(Project1.My.Resources.str17502, MsgBoxStyle.OkOnly);
                MessageBox.Show(CTResources.LoadResString(17502), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.None);

                return;
            }

            //他のダイアログが開いていたら無効
            if (!mod2ndXray.IsAllCloseFrm)
            {
                //Interaction.MsgBox(Project1.My.Resources.str17502, MsgBoxStyle.OkOnly);
                MessageBox.Show(CTResources.LoadResString(17502), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }

            switch (Index)
            {
                case 0:
                    theMode = mod2ndXray.XrayModeConstants.XrayMode_Xray1;
                    break;
                case 1:
                    theMode = mod2ndXray.XrayModeConstants.XrayMode_Xray2;
                    break;
            }

            //DetMode = theMode

            //I.I.移動開始
            //If Not SwitchDet(theMode) Then Exit Sub

            //-->v17.50変更 by 間々田 2011/03/18 マウスポインタの制御はChangeSeqStatusではなくこちらで実行するための変更
            bool IsOK = false;

            //マウスポインタを砂時計にする前にバックアップを取る
            System.Windows.Forms.Cursor MousePointerBak = null;
            MousePointerBak = System.Windows.Forms.Cursor.Current;

            //検出器スイッチ
            //IsOK = SwitchDet(theMode)
            //IsOK = SwitchDet(theMode, UNSET_GAIN) 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //v19.50 元に戻す by長野 2013/12/17
            IsOK = modSeqComm.ChangeXray(theMode);

            //バックアップしたマウスポインタに戻す
            System.Windows.Forms.Cursor.Current = MousePointerBak;

            //失敗したらここで抜ける
            if (!IsOK)
                return;


            //<--v17.50変更 by 間々田 2011/03/18 マウスポインタの制御はChangeSeqStatusではなくこちらで実行するための変更
        }

        private void cwbtnDetShift_Click(object sender, MouseEventArgs e)
        {
            modDetShift.DetShiftConstants theMode = default(modDetShift.DetShiftConstants);

            //機構部動作が可能かチェック
            if (!modMechaControl.IsOkMechaMove())
                return;            //v18.00変更 byやまおか 2011/02/19

            //Rev26.00 add by chouno 2017/03/13
            if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
            {
                return;
            }

            //切り替え可否チェック
            if (!mod2ndDetctor.IsSwitchDet())
            {
                //他の処理が動作中です。処理が停止してから再度操作を行なって下さい。
                //Interaction.MsgBox(Project1.My.Resources.str17502, MsgBoxStyle.OkOnly);
                MessageBox.Show(CTResources.LoadResString(17502), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }

            //if (CTSettings.scaninh.Data.lr_sft == 0)
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
            if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
            {
                //左右シフトONの場合は、各位置をループして動かす。
                //検出器の状態によって移動方向を変える
                switch (modDetShift.IsDetShiftPos)
                {

                    //基準位置にいるなら右シフト
                    case modDetShift.DetShiftConstants.DetShift_origin:
                        theMode = modDetShift.DetShiftConstants.DetShift_forward;
                        break;

                    //右シフト位置なら左シフト
                    case modDetShift.DetShiftConstants.DetShift_forward:
                        theMode = modDetShift.DetShiftConstants.DetShift_backward;
                        break;
                    case modDetShift.DetShiftConstants.DetShift_backward:
                        theMode = modDetShift.DetShiftConstants.DetShift_origin;
                        break;
                    default:
                        theMode = modDetShift.DetShiftConstants.DetShift_none;
                        break;
                }
            }
            else
            {
                //検出器の状態によって移動方向を変える
                switch (modDetShift.IsDetShiftPos)
                {

                    //基準位置にいるならset_sft_pixによって移動方向を決める
                    case modDetShift.DetShiftConstants.DetShift_origin:
                        if ((CTSettings.scancondpar.Data.det_sft_pix < 0))
                        {
                            theMode = modDetShift.DetShiftConstants.DetShift_backward;
                        }
                        else if ((CTSettings.scancondpar.Data.det_sft_pix > 0))
                        {
                            theMode = modDetShift.DetShiftConstants.DetShift_forward;
                        }
                        else
                        {
                            theMode = modDetShift.DetShiftConstants.DetShift_origin;
                        }
                        break;

                    //基準位置にいない場合は基準位置に戻る
                    case modDetShift.DetShiftConstants.DetShift_backward:
                    case modDetShift.DetShiftConstants.DetShift_forward:
                        theMode = modDetShift.DetShiftConstants.DetShift_origin;
                        break;

                    default:
                        theMode = modDetShift.DetShiftConstants.DetShift_none;
                        break;
                }     
            }


            //検出器シフト開始
            //If Not ShiftDet(theMode) Then
            if (!modDetShift.ShiftDet(theMode, modDetShift.SET_GAIN))   //ゲインをセットする 'v18.00変更 byやまおか 2011/07/04
            {
                theMode = modDetShift.DetShiftConstants.DetShift_none;
                //I.I.切替強制停止
                //Rev23.10 条件追加 by長野 2015/09/20
                if (!CTSettings.ChangeXrayOn)
                {
                    modSeqComm.SeqBitWrite("IIChangeStop", true, false);
                }
                else
                {
                    modSeqComm.SeqBitWrite("FPDMoveStop", true, false);
                }
                return;
            }

        }

	}
}
