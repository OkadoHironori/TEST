using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
 // ERROR: Not supported in C#: OptionDeclaration
using VB = Microsoft.VisualBasic;
namespace CT30K
{
	static class modViscom
	{

//v12.01追加 by 間々田 2006-12-13
//   GetDrvStateにてViscomドライバがbusyかどうかチェックできるが
//   実際にはSendCodeを送ってもすぐにはbusyにならない場合があるので
//   以下のフラグを用意した
		public static bool ViscomBusy;

//Global Const DLLVERSION = 1003
//Global Const DLLVERSION = 1004      'changed by 山本 2006-12-12
			//v12.01変更 by 間々田 2006-12-13 ct30k.iniから読み込む
		public static int DLLVERSION;

//****
//*
//*  KOMMANDOS FUER XRAY_DRV
//*
//****
			// NIL
		public const short CODE_XRAY_NIL = -1;
			// SET CURRENT AND HIGH TENSION VALUES
		public const short CODE_XRAY_SET = 0;
			// SWITCH ON/OFF XRAY
		public const short CODE_XRAY_ON = 1;
			// GET ACTUAL VALUES FOR HIGH TENSION AND CURRENT
		public const short CODE_XRAY_GET = 2;
			// GET ADDRESS OF SI-PROGRAM
		public const short CODE_XRAY_UTIFAT = 3;
			// CYCLE WAKE UP OF DRIVER
		public const short CODE_XRAY_AWAKE = 4;
			// INIT DRIVER AND XRAY CONTROLLER
		public const short CODE_XRAY_INI = 5;
			// PREPARE BEAM ON SHADOW TARGET
		public const short CODE_XRAY_PBEAM = 6;
			// SHUTTER ON/OFF THE BEAM
		public const short CODE_XRAY_BEAM = 7;
			// WAIT CONTROLLER IS READY
		public const short CODE_XRAY_WAIT = 8;
			// ADD AN OFFSET ON CURRENT AND SEND TO CONTROLLER
		public const short CODE_XRAY_SETOFF = 9;
			// WRITE AN OFFSET TABLE TO A FILE
		public const short CODE_XRAY_WTAB = 10;
			// INITS AN OFFSET TABLE
		public const short CODE_XRAY_ITAB = 11;
			// SET FILAMENT VALUE
		public const short CODE_XRAY_SFIL = 12;
			// TURN VACUUM ON/OFF
		public const short CODE_XRAY_VACON = 13;
			// SET CENTERING VALUES (ONLY OFFSET)
		public const short CODE_XRAY_SCENT = 14;
			// SET FOCUS VALUES (ONLY OFFSET)
		public const short CODE_XRAY_SFOC = 15;
			// SET CONDENSOR VALUES (ONLY OFFSET)
		public const short CODE_XRAY_SCON = 16;
			// WARMUP COND. TUBE
		public const short CODE_XRAY_WCON = 17;
			// WARMUP FILAMENT ADJUST
		public const short CODE_XRAY_WFIL = 18;
			// WARMUP CENTERING
		public const short CODE_XRAY_WCEN = 19;
			// CONTROLLER INIT
		public const short CODE_XRAY_CINI = 20;
			// CONTROLLER RESTART
		public const short CODE_XRAY_CRES = 21;
			// CONTROLLER STOP
		public const short CODE_XRAY_CSTP = 22;
			// MANUAL CENTERING ON/OFF
		public const short CODE_XRAY_WCEN_MAN = 23;
			// WOBBLE RANGE
		public const short CODE_XRAY_WOBBLE = 24;
			// DEFOCUS TUBE
		public const short CODE_XRAY_DEFOCUS = 25;
			// NEW FILAMENT
		public const short CODE_XRAY_NEWFIL = 26;
			// TIMER
		public const short CODE_XRAY_TIMER = 27;
			// COMMAND PERFORMED
		public const short CODE_XRAY_DONE = 28;
			// CHECK INTERLOCK
		public const short CODE_XRAY_INT = 29;
			// DELETE CENTERING
		public const short CODE_XRAY_DELCNT = 30;
			// SET CENTERING VALUES (ABSOLUT VALUES)
		public const short CODE_XRAY_SCENTA = 31;
			// RESET TIMER
		public const short CODE_XRAY_TRES = 32;
			// DELETE DEFOCUS TABLE
		public const short CODE_XRAY_DELFCS = 33;
			// CENTER ON SPECIFIED SAMPLE POINT
		public const short CODE_XRAY_WCENS = 34;
			// SWITCH OVER TO ANOTHER IMAGE ITENSIFIER
		public const short CODE_XRAY_II = 35;
			// WARMUP WITH OPTIONAL ARGUMENTS
		public const short CODE_XRAY_WUP = 36;
			// RADIATION OFF, SI_XRAY_DRV KILLS ITSELF
		public const short CODE_XRAY_END = 37;
			// WARMUP COND. TUBE (SPECIAL WITH MAX KV in %)
		public const short CODE_XRAY_WCONS = 38;
			// BURN IN TEST
		public const short CODE_XRAY_BURNIN = 39;
			// GET WARMUP TIME
		public const short CODE_XRAY_GWTIME = 40;
			// SET SPOT SIZE
		public const short CODE_XRAY_SET_SPS = 41;

			// LAST ENTRY ID
		public const short CODE_XRAY_ENTRY = 42;

			// Warmup: CONDITIONING OF THE TUBE
		public const short WUP_CON = 1;
			// Warmup: SET FILAMENT
		public const short WUP_FIL = 2;
			// Warmup: CENTER THE TUBE
		public const short WUP_CEN = 4;
			// Warmup: BURN-IN TEST
		public const short WUP_BI = 8;
//////////////////////////////////////////////////////////////////////////////
// driver state
		public const short XST1_DRV_DOWN = 0x1;
		public const short XST1_DRV_READY = 0x2;
		public const short XST1_DRV_BUSY = 0x4;
		public const short XST1_DRV_CHKCONF = 0x8;
// state of warmup:
			// unknown
		public const short XST1_WUP_INIT = 0x10;
			// warmup in progress
		public const short XST1_WUP_RUNNING = 0x20;
			// warmup done
		public const short XST1_WUP_READY = 0x40;
			// warmup failed
		public const short XST1_WUP_FAILED = 0x80;
// state of filemant adjust
			// unknown
		public const short XST1_FLM_INIT = 0x100;
			// filament adjust in progress
		public const short XST1_FLM_RUNNING = 0x200;
			// filament adjust done
		public const short XST1_FLM_READY = 0x400;
			// filament adjust failed
		public const short XST1_FLM_FAILED = 0x800;
// Status der Zentrierung
			// unknown
		public const short XST1_CNT_INIT = 0x1000;
			// centering in progess
		public const short XST1_CNT_RUNNING = 0x2000;
			// centering done
		public const short XST1_CNT_READY = 0x4000;
//Global Const XST1_CNT_CHECK = &H8000            ' check centering
			// check centering       '修正 by 間々田 2006/04/10 末尾に&を付けないとマイナス値となってしまう
		public const int XST1_CNT_CHECK = 0x8000;
			// centering failed
		public const int XST1_CNT_FAILED = 0x10000;

			// target burn in progess
		public const int XST1_BURNIN_RUNNING = 0x20000;
			// target burn in test done
		public const int XST1_BURNIN_READY = 0x40000;
			// target burn in was detectet
		public const int XST1_BURNIN_FAILED = 0x80000;


			// vacuum ok for xray generation
		public const int XST1_VAC_OK = 0x100000;
			// vacuum pumps are running
		public const int XST1_PUMP_OK = 0x200000;
			// current in coil shutter is ok
		public const int XST1_CSHT_OK = 0x400000;
			// current in coil focus is ok
		public const int XST1_CFC_OK = 0x800000;

			// feedback value of uA is OK
		public const int XST1_FUA_OK = 0x1000000;
			// feedback value of kV is OK
		public const int XST1_FKV_OK = 0x2000000;

			// not over uA  from generator
		public const int XST1_GUA_OK = 0x4000000;
			// not over kV  from generator
		public const int XST1_GKV_OK = 0x8000000;
			// board of generator: U is OK
		public const int XST1_GIU_OK = 0x10000000;
			// board of generator: I is OK
		public const int XST1_GII_OK = 0x20000000;

			// power on cap lower than max power
		public const int XST1_CAP_OK = 0x40000000;
			// power on cap2 lower than max power
		public const int XST1_CAP2_OK = 0x80000000;

///////////////////////////////////////////////////////////////////////////////
//STATE 2
			// warning lamp is on
		public const short XST2_WRN_LMP_ON = 0x1;
			// warning lamp is ok
		public const short XST2_WRN_LMP_OK = 0x2;
			// warning lamp is disabled
		public const short XST2_WRN_LMP_DIS = 0x4;

			// prewarning lamp is on
		public const short XST2_PREWRN_LMP_ON = 0x8;
			// prewarning lamp is ok
		public const short XST2_PREWRN_LMP_OK = 0x10;
			// prewarning lamp is disabled
		public const short XST2_PREWRN_LMP_DIS = 0x20;

			// knife switch closed
		public const short XST2_INT_KNF = 0x40;
			// interlock closed
		public const short XST2_INT_SCH = 0x80;
			// key is on, FALSE: stand by mode
		public const short XST2_INT_KEY = 0x100;

			// shutter is open
		public const short XST2_SHT_OPEN = 0x200;
			// system ready for xray on command
		public const short XST2_RDY = 0x400;
			// xray is on
		public const short XST2_XON = 0x800;

			// status of cpu0 - working
		public const short XST2_CPU0_WRK = 0x1000;
			// status of cpu1 - working
		public const short XST2_CPU1_WRK = 0x2000;
			// status of cpu2 - working
		public const short XST2_CPU2_WRK = 0x4000;
//Global Const XST2_CPU3_WRK = &H8000             ' status of cpu3 - working
			// status of cpu3 - working  '修正 by 間々田 2006/05/08 末尾に&を付けないとマイナス値となってしまう
		public const int XST2_CPU3_WRK = 0x8000;
			// status of cpu4 - working
		public const int XST2_CPU4_WRK = 0x10000;
			// status of cpu5 - working
		public const int XST2_CPU5_WRK = 0x20000;
			// status of cpu6 - working
		public const int XST2_CPU6_WRK = 0x40000;
			// status of cpu7 - working
		public const int XST2_CPU7_WRK = 0x80000;
			// status of cpu8 - working
		public const int XST2_CPU8_WRK = 0x100000;
			// status of cpu9 - working
		public const int XST2_CPU9_WRK = 0x200000;
////////////////////////////////////////////////////////////////////////////////
//****
//*
//*  ERRORS FUER XRAY_DRV
//*
//****
			// driver down
		public const short XER_DRV_DOWN = 0x1;
			// warmup failed
		public const short XER_WUP_FAILED = 0x2;
			// filament adjust failed
		public const short XER_FLM_FAILED = 0x4;
			// centering failed
		public const short XER_CNT_FAILED = 0x8;

			// target burn in was detectet
		public const short XER_TARGET = 0x10;
			// insufficient vacuum
		public const short XER_VAC = 0x20;
			// vacuum pumps error
		public const short XER_PUMP = 0x40;
			// current error in shutter coil
		public const short XER_CSHT = 0x80;
			// current error in focus coil
		public const short XER_CFC = 0x100;

			// feedback value of uA
		public const short XER_FUA = 0x200;
			// feedback value of kV
		public const short XER_FKV = 0x400;


			// over uA  from generator
		public const short XER_GUA = 0x800;
			// over kV  from generator
		public const short XER_GKV = 0x1000;
			// inv.board of generator: U is failed
		public const short XER_GIU = 0x2000;
			// inv.board of generator: I is failed
		public const short XER_GII = 0x4000;

			// power on cap greater than max power,XRAY is turned OFF
		public const short XER_CAP = 0x8000;
			// power on cap2 greater than max power,XRAY is turned OFF
		public const int XER_CAP2 = 0x10000;

			// warning lamp is broken
		public const int XER_WRN_LMP = 0x20000;
			// prewarning lamp is broken
		public const int XER_PREWRN_LMP = 0x40000;
			// knife switch is open
		public const int XER_INT_KNF = 0x80000;
			// interlock is open
		public const int XER_INT_SCH = 0x100000;
			// stand by mode
		public const int XER_INT_KEY = 0x200000;
			// cpu not working
		public const int XER_CPU = 0x400000;
			// spot size definition error
		public const int XER_SPSDEF = 0x800000;
			// driver not life
		public const int XER_DRV_LIFE = 0x1000000;

//****
//*
//*  ERRORS TEXT
//*
//****
		public const string XER_TEXT_DRV_DOWN = "driver down;= ";
		public const string XER_TEXT_WUP_FAILED = "warmup failed;";
		public const string XER_TEXT_FLM_FAILED = "filament adjust failed;= ";
		public const string XER_TEXT_CNT_FAILED = "centering failed;= ";

		public const string XER_TEXT_TARGET = "target burn in was detected;= ";
		public const string XER_TEXT_VAC = "insufficient vacuum;= ";
		public const string XER_TEXT_PUMP = "vacuum pumps error;= ";
		public const string XER_TEXT_CSHT = "current error in shutter coil;= ";
		public const string XER_TEXT_CFC = "current error in focus coil;= ";

		public const string XER_TEXT_FUA = "feedback value of uA;= ";
		public const string XER_TEXT_FKV = "feedback value of kV;= ";


		public const string XER_TEXT_GUA = "over uA  from generator;= ";
		public const string XER_TEXT_GKV = "over kV  from generator;= ";
		public const string XER_TEXT_GIU = "inv.board of generator: U is failed;= ";
		public const string XER_TEXT_GII = "inv.board of generator: I is failed;= ";

		public const string XER_TEXT_CAP = "power on cap greater than max power,XRAY is turned OFF;= ";
		public const string XER_TEXT_CAP2 = "power on cap2 greater than max power,XRAY is turned OFF;= ";

		public const string XER_TEXT_WRN_LMP = "warning lamp is broken;= ";
		public const string XER_TEXT_PREWRN_LMP = "prewarning lamp is broken;= ";
		public const string XER_TEXT_INT_KNF = "knife switch is open;= ";
		public const string XER_TEXT_INT_SCH = "interlock is open;= ";
		public const string XER_TEXT_INT_KEY = "stand by mode;= ";
		public const string XER_TEXT_CPU = "cpu not working;= ";
		public const string XER_TEXT_SPSDEF = "spot size definition error";
		public const string XER_TEXT_DRV_LIFE = "driver not life";

//****
//*
//*  ACTUAL VALUES FUER XRAY_DRV
//*
//****
			// target vacuum
		public const short XACT_TVAC = 0x1;
			// target kV
		public const short XACT_TKV = 0x2;
			// target uA
		public const short XACT_TUA = 0x4;
			// filament demand value
		public const short XACT_TFIL = 0x8;

			// feedback vacuum
		public const short XACT_FVAC = 0x10;
			// feedback kV
		public const short XACT_FKV = 0x20;
			// feedback uA
		public const short XACT_FUA = 0x40;
			// filament resitance
		public const short XACT_FFIL = 0x80;


			// centering1 value
		public const short XACT_CENT1 = 0x100;
			// centering2 value
		public const short XACT_CENT2 = 0x200;
			// centering1 aperture
		public const short XACT_MB1 = 0x400;
			// centering2 aperture
		public const short XACT_MB2 = 0x800;

			// focus value
		public const short XACT_FOC = 0x1000;
			// condensor value
		public const short XACT_CON = 0x2000;
			// focus regulation
		public const short XACT_FCS = 0x4000;
			// spot size value
		public const short XACT_SPOT = 0x8000;
			// dose
		public const int XACT_DOSE = 0x10000;
			// time of operation
		public const int XACT_TOP = 0x20000;
			// progress information for centering
		public const int XACT_CENPROG = 0x40000;

			// target spot size definition
		public const int XACT_TSPS = 0x100000;
			// feedback spot size definition
		public const int XACT_FSPS = 0x200000;

//
// version control struct
//
		public struct XRAY_VERSION
		{
				// software version of the xray_drv
			public int version;
				// software version of the dll
			public int dll_version;
				// vacuum control
			public int ver_vac;
				// centering
			public int ver_cnt;
				// focus
			public int ver_fcs;
				// generator
			public int ver_gnr;
				// centering aperture
			public int ver_mv;
				// tube version/type
			public int ver_tb;
		}

		public const short WM_USER = 0x400;
		public const int WM_XDD_ERROR = WM_USER + 1;

//
// Some global constants
//
		private const string ArgProz = "-proz_file=";
		private const short DEFAULT_TIMER_ELAPSE = 100;

			// The timer ID if not using calbacks
		private static int TimerId;

		public static int ViscomError;
		public static int ViscomState1;
		public static int ViscomState2;

		public static int gViscomFocus;

//Public gCondTime     As Long    'v12.01追加 by 間々田 2006/12/12 次のウォームアップまでの時間
//Public gFilamentTime As Long    'v12.01追加 by 間々田 2006/12/12 次のフィラメント調整までの時間
			//v12.01追加 by 間々田 2006/12/13 次のウォームアップまでの時間が経過した
		public static bool IsElapsedWUP;
			//v12.01追加 by 間々田 2006/12/13 次のフィラメント調整までの時間経過した
		public static bool IsElapsedFLM;
		[DllImport("kernel32", EntryPoint = "SetCurrentDirectoryA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//
//-----------------------------WIN32 API IMPORTS GO HERE-----------------------------
//

//'sets Timer as normal SetTimer does
//Public Declare Function SetTimer Lib "user32" _
//'                    (ByVal hWnd As Long, _
//'                     ByVal nIDEvent As Long, _
//'                     ByVal uElapse As Long, _
//'                     ByVal lpTimerFunc As Long) As Long
//
//'kills timer as normal KillTimer does
//Public Declare Function KillTimer Lib "user32" _
//'                    (ByVal hWnd As Long, _
//'                     ByVal nIDEvent As Long) As Long

//sets current working directory
		public static extern int SetCurrentDirectory(string x_lpPathName);

//
//------------------------------------XRAY DLL FUNCTIONS BEGIN HERE--------------------
//
		#if DebugOn

		public static int nCondTime;
		public static int pnFilamentTime;

		public static int CreateDrv(string x_strSiProzFileName, int x_nAddr_OnState, int x_nAddr_OnValue, int x_nAddr_OnError)
		{
			int functionReturnValue = 0;
			My.MyProject.Forms.frmVirtualXrayControl.lstViscom.Items.Add("CreateDrv");
			functionReturnValue = 1;

			nCondTime = 2;
			pnFilamentTime = 2;
			return functionReturnValue;
		}

		public static void DeleteDrv()
		{
			My.MyProject.Forms.frmVirtualXrayControl.lstViscom.Items.Add("DeleteDrv");
		}

		public static int GetVersion(ref XRAY_VERSION x_nOptionalParam)
		{
			x_nOptionalParam.dll_version = DLLVERSION;
		}

		public static int GetFocusI()
		{
			//UPGRADE_WARNING: オブジェクト frmVirtualXrayControl.Focus._Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			return My.MyProject.Forms.frmVirtualXrayControl.Focus_Renamed._Value;
		}

		public static void SetXrayStop()
		{
			var _with1 = frmVirtualXrayControl;
			_with1.lstViscom.Items.Add("SetXrayStop");
			_with1.Up_X_On.Value = 0;
			//Ｘ線オフ
			_with1.cwneViscom(1).Value = false;
			//ウォームアップ中オフ
			_with1.cwneViscom(7).Value = false;
			//センタリング調整中オフ
		}

		public static string GetLastErrorText()
		{
			return My.MyProject.Forms.frmVirtualXrayControl.txtError.Text;
		}

		public static int GetFeedbackU()
		{
			//UPGRADE_WARNING: オブジェクト frmVirtualXrayControl.Up_XR_VoltFeedback._Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			return My.MyProject.Forms.frmVirtualXrayControl.Up_XR_VoltFeedback._Value;
		}

		public static int GetFeedbackI()
		{
			//UPGRADE_WARNING: オブジェクト frmVirtualXrayControl.Up_XR_CurrentFeedback._Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			return My.MyProject.Forms.frmVirtualXrayControl.Up_XR_CurrentFeedback._Value;
		}

		public static int GetFeedbackVac()
		{
			return 349;
		}

		public static int GetFeedbackIon()
		{
			return 7;
		}

		public static int GetTargetU()
		{
			//UPGRADE_WARNING: オブジェクト frmVirtualXrayControl.Up_XR_VoltSet._Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			return My.MyProject.Forms.frmVirtualXrayControl.Up_XR_VoltSet._Value;
		}

		public static int GetTargetI()
		{
			//UPGRADE_WARNING: オブジェクト frmVirtualXrayControl.Up_XR_CurrentSet._Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			return My.MyProject.Forms.frmVirtualXrayControl.Up_XR_CurrentSet._Value;
		}

		public static int SetTargetU(int x_nKv)
		{
			My.MyProject.Forms.frmVirtualXrayControl.Up_XR_VoltSet._Value = x_nKv;
		}

		public static int SetTargetI(int x_nUa)
		{
			My.MyProject.Forms.frmVirtualXrayControl.Up_XR_CurrentSet._Value = x_nUa;
		}

		public static int GetCenteringProgress()
		{
			return 100;
		}

		public static int GetCenteringAperture(int x_nMod = 0)
		{
			//UPGRADE_WARNING: オブジェクト frmVirtualXrayControl.CenteringAperture._Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			return My.MyProject.Forms.frmVirtualXrayControl.CenteringAperture._Value;
		}

		public static void GetCenteringXY(ref int x_pnXc, ref int x_pnYc, int x_nMod = 0)
		{
			//UPGRADE_WARNING: オブジェクト frmVirtualXrayControl.CenteringX._Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			x_pnXc = My.MyProject.Forms.frmVirtualXrayControl.CenteringX._Value;
			//UPGRADE_WARNING: オブジェクト frmVirtualXrayControl.CenteringY._Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			x_pnYc = My.MyProject.Forms.frmVirtualXrayControl.CenteringY._Value;
		}

		public static void GetMaxMinKv(ref int x_pnMaxKv, ref int x_pnMinKv)
		{
			//UPGRADE_WARNING: オブジェクト frmVirtualXrayControl.Up_XR_Max_kV._Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			x_pnMaxKv = My.MyProject.Forms.frmVirtualXrayControl.Up_XR_Max_kV._Value;
			//UPGRADE_WARNING: オブジェクト frmVirtualXrayControl.Up_XR_Min_kV._Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			x_pnMinKv = My.MyProject.Forms.frmVirtualXrayControl.Up_XR_Min_kV._Value;
		}

		public static void GetMaxMinUa(ref int x_pnMaxUa, ref int x_pnMinUa)
		{
			//UPGRADE_WARNING: オブジェクト frmVirtualXrayControl.Up_XR_Max_mA._Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			x_pnMaxUa = My.MyProject.Forms.frmVirtualXrayControl.Up_XR_Max_mA._Value;
			//UPGRADE_WARNING: オブジェクト frmVirtualXrayControl.Up_XR_Min_mA._Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			x_pnMinUa = My.MyProject.Forms.frmVirtualXrayControl.Up_XR_Min_mA._Value;
		}

		public static void GetDrvState(ref int x_nState1, ref int x_nState2)
		{

			x_nState1 = 0;
			x_nState2 = 0;

			var _with2 = frmVirtualXrayControl;

			if (_with2.cwneViscom(19)._Value)
				x_nState1 = x_nState1 | XST1_DRV_READY;
			//Ｘ線ドライバレディ
			if (_with2.cwneViscom(0)._Value)
				x_nState1 = x_nState1 | XST1_DRV_BUSY;
			//Ｘ線ドライバビジー
			if (_with2.cwneViscom(1)._Value)
				x_nState1 = x_nState1 | XST1_WUP_RUNNING;
			//ウォームアップ中
			if (_with2.cwneViscom(2)._Value)
				x_nState1 = x_nState1 | XST1_WUP_READY;
			//ウォームアップ完了
			if (_with2.cwneViscom(3)._Value)
				x_nState1 = x_nState1 | XST1_WUP_FAILED;
			//ウォームアップ失敗
			if (_with2.cwneViscom(4)._Value)
				x_nState1 = x_nState1 | XST1_FLM_RUNNING;
			//フィラメント調整中
			if (_with2.cwneViscom(5)._Value)
				x_nState1 = x_nState1 | XST1_FLM_READY;
			//フィラメント準備完了
			if (_with2.cwneViscom(6)._Value)
				x_nState1 = x_nState1 | XST1_FLM_FAILED;
			//フィラメント調整失敗
			if (_with2.cwneViscom(7)._Value)
				x_nState1 = x_nState1 | XST1_CNT_RUNNING;
			//センタリング調整中
			if (_with2.cwneViscom(8)._Value)
				x_nState1 = x_nState1 | XST1_CNT_READY;
			//センタリング準備完了
			if (_with2.cwneViscom(9)._Value)
				x_nState1 = x_nState1 | XST1_CNT_CHECK;
			//センタリングチェック
			if (_with2.cwneViscom(10)._Value)
				x_nState1 = x_nState1 | XST1_CNT_FAILED;
			//センタリング調整失敗
			if (_with2.cwneViscom(11)._Value)
				x_nState1 = x_nState1 | XST1_VAC_OK;
			//真空ポンプ準備完了
			if (_with2.cwneViscom(12)._Value)
				x_nState2 = x_nState2 | XST2_CPU0_WRK;
			//
			if (_with2.cwneViscom(13)._Value)
				x_nState2 = x_nState2 | XST2_CPU1_WRK;
			//
			if (_with2.cwneViscom(14)._Value)
				x_nState2 = x_nState2 | XST2_CPU2_WRK;
			//
			if (_with2.cwneViscom(15)._Value)
				x_nState2 = x_nState2 | XST2_CPU3_WRK;
			//
			if (_with2.cwneViscom(16)._Value)
				x_nState2 = x_nState2 | XST2_CPU4_WRK;
			//
			if (_with2.cwneViscom(17)._Value)
				x_nState2 = x_nState2 | XST2_CPU5_WRK;
			//
			if (_with2.cwneViscom(18)._Value)
				x_nState2 = x_nState2 | XST2_CPU6_WRK;
			//

			if (_with2.Up_X_On.Value == 1)
				x_nState2 = x_nState2 | XST2_XON;
			//Ｘ線オン
			if (_with2.Up_Standby.Value == 1)
				x_nState2 = x_nState2 | XST2_INT_KEY;
			//スタンバイ


		}

		public static void GetDrvErrors(ref int x_nError)
		{
			//UPGRADE_WARNING: オブジェクト frmVirtualXrayControl.cwneError.Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			x_nError = My.MyProject.Forms.frmVirtualXrayControl.cwneError.Value;
		}

		public static int SendCode(int x_nCommand, int x_nP1 = 0, int x_nP2 = 0, int x_nP3 = 0)
		{

			string strCommand = null;

			var _with3 = frmVirtualXrayControl;

			switch (x_nCommand) {
				case CODE_XRAY_ON:
					strCommand = "CODE_XRAY_ON, " + Convert.ToString(x_nP1);
					_with3.Up_X_On.Value = x_nP1;
					//v17.72/v19.02追加 byやまおか 2012/05/14
					if ((x_nP1 == 0)) {
						_with3.cwneViscom(1).Value = false;
						_with3.Up_X_On.Value = 0;
					}
					break;
				case CODE_XRAY_WCON:
					strCommand = "CODE_XRAY_WCON";
					_with3.cwneViscom(1).Value = true;
					_with3.Up_X_On.Value = 1;
					break;
				case CODE_XRAY_WFIL:
					strCommand = "CODE_XRAY_WFIL";
					break;
				case CODE_XRAY_NEWFIL:
					strCommand = "CODE_XRAY_NEWFIL";
					break;
				case CODE_XRAY_WCENS:
					strCommand = "CODE_XRAY_WCENS, " + Convert.ToString(x_nP1);
					_with3.cwneViscom(7).Value = true;
					break;
				case CODE_XRAY_WCEN:
					strCommand = "CODE_XRAY_WCEN, " + Convert.ToString(x_nP1);
					_with3.cwneViscom(7).Value = true;
					break;
				case CODE_XRAY_WCEN_MAN:
					strCommand = "CODE_XRAY_WCEN_MAN, " + Convert.ToString(x_nP1);
					break;
				case CODE_XRAY_WOBBLE:
					strCommand = "CODE_XRAY_WOBBLE, " + Convert.ToString(x_nP1);
					break;
				case CODE_XRAY_WTAB:
					strCommand = "CODE_XRAY_WTAB, " + Convert.ToString(x_nP1);
					break;
				case CODE_XRAY_DELCNT:
					strCommand = "CODE_XRAY_DELCNT";
					break;
				case CODE_XRAY_SCENT:
					strCommand = "CODE_XRAY_SCENT, " + Convert.ToString(x_nP1) + ", " + Convert.ToString(x_nP2);
					break;
				case CODE_XRAY_DEFOCUS:
					strCommand = "CODE_XRAY_DEFOCUS, " + Convert.ToString(x_nP1);
					_with3.Focus_Renamed._Value = x_nP1;
					break;
				case CODE_XRAY_DELFCS:
					strCommand = "CODE_XRAY_DELFCS";
					break;
				case CODE_XRAY_CRES:
					strCommand = "CODE_XRAY_CRES";
					break;
				case CODE_XRAY_CSTP:
					strCommand = "CODE_XRAY_CSTP";
					break;
			}

			_with3.lstViscom.Items.Add("SendCode " + strCommand);


		}

		public static int GetNextWarmupTime(ref int x_nCondTime, ref int x_pnFilamentTime)
		{
			x_nCondTime = nCondTime;
			x_pnFilamentTime = pnFilamentTime;
		}
		[DllImport("XrayDrvDll.dll", EntryPoint = "_CreateDrv", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

		#elif
//UPGRADE_NOTE: 式 Else が True に評価されなかったか、またはまったく評価されなかったため、#If #EndIf ブロックはアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="27EE2C3C-05AF-4C04-B2AF-657B4FB6B5FC"' をクリックしてください。

//creates Driver Instance and can start it
		public static extern long CreateDrv(string x_strSiProzFileName, long x_nAddr_OnState, long x_nAddr_OnValue, long x_nAddr_OnError);
		[DllImport("XrayDrvDll.dll", EntryPoint = "_StopDrv", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void StopDrv();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_DeleteDrv", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void DeleteDrv();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetVersion", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//stops  driver

//shuts down driver and cleans up memory

//provides some version information for XrayDllDrv, see XRAY_VERSION in the XrayDllCtrl_Header.bas
		public static extern long GetVersion(ref XRAY_VERSION x_nOptionalParam);
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetFocusI", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//Returns focus current of the tube
		public static extern long GetFocusI();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_SetXrayStop", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void SetXrayStop();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetLastErrorText", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//stops xray anyway

//returns last error's description in ascii form
		public static extern string GetLastErrorText();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetFeedbackU", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns actual (feedback) value for the high tension
		public static extern long GetFeedbackU();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetFeedbackI", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns actual (feedback) value for the current
		public static extern long GetFeedbackI();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetFeedbackVac", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns actual (feedback) value for the vacuum
		public static extern long GetFeedbackVac();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetTargetU", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns target value for the high tension
		public static extern long GetTargetU();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetTargetI", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns target value for the current
		public static extern long GetTargetI();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetTargetVac", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns target value for the vacuum
		public static extern long GetTargetVac();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_SetTargetU", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//sets target value for the high tension
		public static extern long SetTargetU(long x_nKv);
		[DllImport("XrayDrvDll.dll", EntryPoint = "_SetTargetI", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//sets target value for the current
		public static extern long SetTargetI(long x_nUa);
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetTargetSpotSizeDef", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns target spot size
		public static extern long GetTargetSpotSizeDef();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_SetTargetSpotSizeDef", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//sets target spot size
		public static extern long SetTargetSpotSizeDef(long x_nSpotSize);
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetFeedbackSpotSizeDef", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns actual (feedback) spot size
		public static extern long GetFeedbackSpotSizeDef();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetFilOperationTime", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns filament life time (in seconds) since the filament was built-in in the system
		public static extern long GetFilamentOperationTime();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetFilAverageTime", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns filament life time (in seconds) since the filament manufactured
		public static extern long GetFilamentAverageTime();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetDose", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns actual xray radiation dose in uSv/sec
		public static extern long GetDose();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetSpotSize", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns spot size of focal spot in uM
		public static extern long GetSpotSize();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetCenteringProgress", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns progress of centering in %
		public static extern long GetCenteringProgress();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetCenteringAperture", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns centering aperture (0 = in the tube; 1 = in the rod anode, if any)
		public static extern long GetCenteringAperture(long x_nMod = 0);
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetCenteringXY", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns beam offsets for X and Y directions
		public static extern void GetCenteringXY(ref long x_pnXc, ref long x_pnYc, long x_nMod = 0);
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetMaxMinKv", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns minimum and maximum values for high tension possible
		public static extern void GetMaxMinKv(ref long x_pnMaxKv, ref long x_pnMinKv);
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetMaxMinUa", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns minimum and maximum values for current possible
		public static extern void GetMaxMinUa(ref long x_pnMaxUa, ref long x_pnMinUa);
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetMaxPower", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns max. power for xray, focus, centering 1 and centering 2
		public static extern void GetMaxPower(ref long x_pnXrayPow, ref long x_pnFocusPow = 0, ref long x_pnCent1_Pow = 0, long x_pnCent2_Pow = 0);
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetState", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns actual Xray-driver state bit-coded in two variables
		public static extern void GetDrvState(ref long x_nState1, ref long x_nState2);
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetError", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns driver bit-coded double word for errors
		public static extern void GetDrvErrors(ref long x_nError);
		[DllImport("XrayDrvDll.dll", EntryPoint = "_SendWCode", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//sends a command to driver and waits until it's executed
		public static extern long SendWCode(long x_nCommand, long x_nP1 = 0, long x_nP2 = 0, long x_nP3 = 0);
		[DllImport("XrayDrvDll.dll", EntryPoint = "_SendCode", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//sends a command to driver without waiting for its execution end
		public static extern long SendCode(long x_nCommand, long x_nP1 = 0, long x_nP2 = 0, long x_nP3 = 0);
		[DllImport("XrayDrvDll.dll", EntryPoint = "_EnableMonValue", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//enables calling OnValue callback function out of Xray driver
		public static extern void EnableMonValue(long x_nState);
		[DllImport("XrayDrvDll.dll", EntryPoint = "_IsErrInterlock", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//Check if interlock chain is open
		public static extern long IsErrInterlock();
		[DllImport("XrayDrvDll.dll", EntryPoint = "_GetNextWarmupTime", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

//returns time interval in seconds to the next warmup
		public static extern long GetNextWarmupTime(ref long x_nCondTime, ref long x_pnFilamentTime);

		#endif

//
// ----------------------------THE MAIN FUNCTION----------------------------------------
//
		public static void StartViscom()
		{
			 // ERROR: Not supported in C#: OnErrorStatement


			string strCommandLine = null;
			string strProzFile = null;
			int StartPosProz = 0;
			int EndPosProz = 0;
			int ArgProzLen = 0;
			string strExePath = null;
			string strPathEnironment = null;
			int nPosLastBackSlash = 0;


			ViscomError = 0;
			ViscomState1 = 0;
			ViscomState2 = 0;
			gViscomFocus = 0;

			// Get command line and make lower case
			strCommandLine = Strings.LCase(Interaction.Command());

			// Check if have to set default xrc bin directory
			//If InStr(strCommandLine, "-debug") Then
			//
			// Used only while developing. Use yours
			// if differs from mine
			//
			strExePath = "c:\\viscom\\project\\xrc\\bin\\";
			//Else
			//    strExePath = App.Path
			//End If

			SetCurrentDirectory(strExePath);
			//
			// Determine proz file to start with
			//
			StartPosProz = Strings.InStr(strCommandLine, ArgProz);
			if (StartPosProz > 0) {

				//
				// Is given by command line
				//
				ArgProzLen = Strings.Len(ArgProz);

				EndPosProz = Strings.InStr(StartPosProz + ArgProzLen, strCommandLine, " ");
				if (EndPosProz > 0) {
					strProzFile = Strings.Mid(strCommandLine, StartPosProz + ArgProzLen, EndPosProz - (StartPosProz + ArgProzLen) + 1);
				} else {
					strProzFile = Strings.Right(strCommandLine, Strings.Len(strCommandLine) - (StartPosProz + ArgProzLen) + 1);
				}
			} else {
				//
				// Not given by command line, take default
				//
				strProzFile = "si.proz";
			}

			//
			int nStartRevSearchPos = 0;
			if (Strings.InStr(strProzFile, "\\") == 0 & Strings.InStr(strProzFile, "/") == 0) {


				if (Strings.Right(strExePath, 1) == "\\") {
					nStartRevSearchPos = Strings.Len(strExePath) - 1;
				} else {
					nStartRevSearchPos = -1;
				}

				nPosLastBackSlash = Strings.InStrRev(strExePath, "\\", nStartRevSearchPos);
				strProzFile = Strings.Left(strExePath, nPosLastBackSlash) + "data\\" + strProzFile;
			}

			//
			// Start driver, set callback pointers
			//
			if (!(CreateDrv(strProzFile, 0, 0, 0) > 0)) {
				Interaction.MsgBox("Unable to start Xray driver... The reasons could be:" + Constants.vbCrLf + "1. no/wrong prozess start file found in data directory" + Constants.vbCrLf + "2. data directory not present" + Constants.vbCrLf + "3. no/wrong si_xray_drv.exe present found bin directory");
				return;
			}

			XRAY_VERSION sctVersion = default(XRAY_VERSION);

			//Get Version
			GetVersion(ref sctVersion);

			var _with4 = sctVersion;

			//Check Version Compatibility
			if (_with4.dll_version != DLLVERSION) {

				//Versions of dll and the current program are incompatible
				Interaction.MsgBox("xray DLL version conflict! - Must be: " + Convert.ToString(_with4.dll_version) + " Is: " + Convert.ToString(DLLVERSION));

			}


		}

//
//   Viscomステータスを取り込む  'v12.01追加 by 間々田 2006/12/13
//

		static bool static_GetViscom_IsLastWarmup;
		//True : 前回ウォームアップ中だった
		static bool static_GetViscom_IsLastFilament;
		//True : 前回フィラメント調整中だった
		//True : 今回ウォームアップ中だった
		//True : 今回フィラメント調整中だった
		static short static_GetViscom_Count;
		public static void GetViscom()
		{
			bool IsNowWarmup = false;
			bool IsNowFilament = false;
			int CondTime = 0;
			int FilamentTime = 0;

			//エラーステータス取得
			GetDrvErrors(ref ViscomError);

			//その他ステータス取得
			GetDrvState(ref ViscomState1, ref ViscomState2);

			//「ウォームアップ開始」「フィラメント調整開始」時は強制的に XST1_DRV_BUSY ビットをオンにする
			if (ViscomBusy) {
				if (ViscomState1 & XST1_DRV_BUSY) {
					ViscomBusy = false;
				} else {
					ViscomState1 = ViscomState1 | XST1_DRV_BUSY;
				}
			}

			IsNowWarmup = ViscomState1 & XST1_WUP_RUNNING;
			IsNowFilament = ViscomState1 & XST1_FLM_RUNNING;

			if (static_GetViscom_IsLastWarmup != IsNowWarmup)
				IsElapsedWUP = false;
			if (static_GetViscom_IsLastFilament != IsNowFilament)
				IsElapsedFLM = false;

			//GetDrvStateで「ウォームアップ完了」「フィラメント調整完了」でも
			//Ｘ線をオンしてみると実際にはそうでない場合があるので、
			//GetNextWarmupTimeをコールして必要に応じてステータスを調整する
			//なお、GetNextWarmupTimeは負荷が高いので連続してコールしないこと（２秒に１回の割合で呼び出す）
			if ((!Convert.ToBoolean(ViscomState1 & XST1_DRV_BUSY)) & (!Convert.ToBoolean(ViscomState2 & XST2_XON)) & (Convert.ToBoolean(ViscomState1 & XST1_FLM_READY) | Convert.ToBoolean(ViscomState1 & XST1_WUP_READY))) {

				static_GetViscom_Count = (static_GetViscom_Count < 3 ? static_GetViscom_Count + 1 : 0);

				if (((!IsElapsedWUP) | (!IsElapsedFLM)) & (static_GetViscom_Count == 0)) {
					//次のウォームアップ/フィラメント調整までの時間を求める
					GetNextWarmupTime(ref CondTime, ref FilamentTime);
					IsElapsedWUP = (CondTime <= 1);
					IsElapsedFLM = (FilamentTime <= 1);

				}

			}

			//次のウォームアップまでの時間が経過している場合、XST1_WUP_READY ビットをオフにする
			if (IsElapsedWUP)
				ViscomState1 = ViscomState1 & (!XST1_WUP_READY);

			//次のフィラメント調整までの時間が経過している場合、XST1_FLM_READY ビットをオフにする
			if (IsElapsedFLM)
				ViscomState1 = ViscomState1 & (!XST1_FLM_READY);

			static_GetViscom_IsLastWarmup = IsNowWarmup;
			static_GetViscom_IsLastFilament = IsNowFilament;

		}
	}
}
