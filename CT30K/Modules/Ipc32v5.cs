//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
//
using CTAPI;
using CT30K.Common;

namespace CT30K
{
	internal static class Ipc32v5
	{
        #region 64bitでは使用しない
        /*

        //
		// Built-in Auto-Pro functions
		// Definitions for built-in Auto-Pro functions. These functions are
		// exported by IPC32.DLL.
		//

		//-----------------------------------------------------------------
		// windows defininition
		//-----------------------------------------------------------------
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINTAPI
		{
			public int X;
			public int Y;
		}

		//ImageProテキスト描画オブジェクト   'v15.0追加 by 間々田 2009/01/19
		public static clsImagePro IPOBJ = new clsImagePro();

		//-----------------------------------------------------------------
		// Pre-defined variables
		//-----------------------------------------------------------------
		// Note: For use in Visual Basic, change the following line to
		// Global Const vbNullChar = vbNull
		//Global Const vbNullChar = chr$(0)  'v9.5 削除 by 間々田 2004/09/14

#if !ImageProV3									//v9.5 追加 by 間々田 2004/09/15

		public const int IPC_SIZECLASSIFIERS = 3;
		public const int IPC_SIZEICAL = 256;

		public static RECT ipRect;
		public static POINTAPI[] Pts = new POINTAPI[2049];
		public static short[] Lut = new short[257];
		public static float[] ipBins = new float[17];
		public static short[] ipClassifiers = new short[IPC_SIZECLASSIFIERS + 1];
		public static float[] ipICal = new float[IPC_SIZEICAL + 1];
		public static short[] ipHalfTypes = new short[8];
		public static short[] ipHalfScreens = new short[8];
		public static int ret;
		public static int IPNULL;
		public static short[] ipArray = new short[11];
		public static int[] ipLArray = new int[11];
		public static int ipLVal;
		public static string ipStrFixed;		//TODO 固定長文字列
		public static StringBuilder ipStrFixedSB = new StringBuilder(255);


		//-----------------------------------------------------------------
		// HIL definitions
		//-----------------------------------------------------------------

		public const int IMC_GRAY = 1;
		public const int IMC_PALETTE = 2;
		public const int IMC_RGB = 3;
		public const int IMC_GRAY12 = 4;
		public const int IMC_FLOAT = 5;
		public const int IMC_GRAY16 = 6;
		public const int IMC_RGB36 = 8;
		public const int IMC_RGB48 = 9;

		public const int IMC_C_SCALE = 0x800;
		public const int IMC_C_DIRECT = 0x400;

		public const int OR_LEFTRIGHT = 1;
		public const int OR_UPDOWN = 2;
		public const int OR_TRANSPOSE = 3;
		public const int OR_ROTATE90 = 4;
		public const int OR_ROTATE270 = 5;
		public const int OR_ROTATE180 = 6;

		public const int RA_CENTER = 5;

		public const int EQ_BESTFIT = 0;
		public const int EQ_LINEAR = 1;
		public const int EQ_BELL = 2;
		public const int EQ_LOGARITHMIC = 3;
		public const int EQ_EXPONENTIAL = 4;
		public const int EQ_WHITEBAL = 5;

		public const int IFFCOMP_NONE = 0;
		public const int IFFCOMP_DEFAULT = 1;
		public const int IFFCOMP_RLE = 2;
		public const int IFFCOMP_CCITT1D = 3;
		public const int IFFCOMP_CCITTG3 = 4;
		public const int IFFCOMP_CCITTG4 = 5;
		public const int IFFCOMP_LZW = 6;
		public const int IFFCOMP_LZWHPRED = 7;
		public const int IFFCOMP_JPEG = 8;

		public const int IFFCL_BILEVEL = 0;
		public const int IFFCL_GRAY = 1;
		public const int IFFCL_PALETTE = 2;
		public const int IFFCL_RGB = 3;
		public const int IFFCL_RGBPLANAR = 4;
		public const int IFFCL_RGBA = 5;
		public const int IFFCL_RGBAPLANAR = 6;

		// for IpDocOpen
		public const int IMA_RD = 1;
		public const int IMA_RDWR = 2;


		//-----------------------------------------------------------------
		// HAIL definitions
		//-----------------------------------------------------------------

		public const int CM_RGB = 0;
		public const int CM_HSI = 1;
		public const int CM_HSV = 2;
		public const int CM_YIQ = 3;

		// count/size (blob) measurements
		public const int BLBM_ALL = -1;
		public const int BLBM_AREA = 0;
		public const int BLBM_ASPECT = 1;
		public const int BLBM_BOX_AREA = 2;
		public const int BLBM_BOX_XY = 3;
		public const int BLBM_CENTRX = 4;
		public const int BLBM_CENTRY = 5;
		public const int BLBM_DENSITY = 6;
		public const int BLBM_DIRECTION = 7;
		public const int BLBM_HOLEAREA = 8;
		public const int BLBM_HOLEAREARATIO = 9;
		public const int BLBM_MAJORAX = 10;
		public const int BLBM_MINORAX = 11;
		public const int BLBM_MAXFERRET = 12;
		public const int BLBM_MINFERRET = 13;
		public const int BLBM_MEANFERRET = 14;
		public const int BLBM_MAXRADIUS = 15;
		public const int BLBM_MINRADIUS = 16;
		public const int BLBM_NUMHOLES = 17;
		public const int BLBM_PERIMETER = 18;
		public const int BLBM_RADIUSRATIO = 19;
		public const int BLBM_ROUNDNESS = 20;
		public const int BLBM_CLUSTER = 21;
		public const int BLBM_RED = 22;
		public const int BLBM_GREEN = 23;
		public const int BLBM_BLUE = 24;
		public const int BLBM_PERAREA = 25;
		public const int BLBM_CLASS = 26;
		public const int BLBM_LENGTH = 27;
		public const int BLBM_WIDTH = 28;
		public const int BLBM_PERIMETER2 = 29;
		public const int BLBM_IOD = 30;
		public const int BLBM_PCONVEX = 31;
		public const int BLBM_PELLIPSE = 32;
		public const int BLBM_PRATIO = 33;
		public const int BLBM_AREAPOLY = 34;
		public const int BLBM_FRACTDIM = 35;
		public const int BLBM_CMASSX = 36;
		public const int BLBM_CMASSY = 37;
		public const int BLBM_SIZECOUNT = 38;
		public const int BLBM_BOXX = 39;
		public const int BLBM_BOXY = 40;
		public const int BLBM_MINCALIP = 41;
		public const int BLBM_MAXCALIP = 42;
		public const int BLBM_MEANCALIP = 43;
		public const int BLBM_DENSMIN = 44;
		public const int BLBM_DENSMAX = 45;
		public const int BLBM_DENSDEV = 46;
		public const int BLBM_BRANCHLEN = 47;
		public const int BLBM_DENDRITES = 48;
		public const int BLBM_ENDPOINTS = 49;
		public const int BLBM_MARGINATION = 50;
		public const int BLBM_HETEROGENEITY = 51;
		public const int BLBM_CLUMPINESS = 52;
		public const int BLBM_DENSSUM = 53;
		public const int BLBM_SRANGE = 54;

		// count/size extended measurements
		public const int BLEX_RADIUS = 1000;
		public const int BLEX_DIAMETER = 1001;
		public const int BLEX_CALIPER = 1002;
		public const int BLEX_BRANCHLEN = 1003;

		// population density measurements
		public const int BPOP_OBJECTS = 2000;
		public const int BPOP_AREA = 2001;
		public const int BPOP_DENSITY = 2002;
		public const int BPOP_CORRDENSITY = 2003;

		// operations
		public const int OPA_ADD = 0;
		public const int OPA_SUB = 1;
		public const int OPA_DIFF = 2;
		public const int OPA_MULT = 3;
		public const int OPA_DIV = 4;
		public const int OPA_AVG = 5;
		public const int OPA_MAX = 6;
		public const int OPA_MIN = 7;
		// 8 is reserved for internal use
		public const int OPA_ACC = 9;
		public const int OPA_INV = 10;
		public const int OPA_X2 = 11;
		public const int OPA_SQR = 12;
		public const int OPA_LOG = 13;
		public const int OPA_EXP = 14;
		public const int OPA_X2Y = 15;
		public const int OPA_SET = 16;

		public const int OPL_AND = 0;
		public const int OPL_OR = 1;
		public const int OPL_XOR = 2;
		public const int OPL_NAND = 3;
		public const int OPL_NOR = 4;
		public const int OPL_NOT = 5;
		public const int OPL_COPY = 6;

		public const int MORPHO_2x2SQUARE = 0;
		public const int MORPHO_3x1ROW = 1;
		public const int MORPHO_1x3COLUMN = 2;
		public const int MORPHO_3x3CROSS = 3;
		public const int MORPHO_5x5OCTAGON = 4;
		public const int MORPHO_CUSTOM = 5;

		public const int FFT_NOTCH = 0x0;
		public const int FFT_PHASE = 0x1;
		public const int FFT_SPECTRUM = 0x2;
		public const int FFT_HANNING = 0x1000;


		// for IpFftForward. Uses FFT_PHASE and FFT_SPECTRUM above also
		public const int FFT_PHASE32 = 0x3;
		public const int FFT_SPECTRUM32 = 0x4;
		public const int FFT_SPECPHAS32 = 0x5;

		public const int MORPHO_7x7OCTAGON = 6;
		public const int MORPHO_11x11OCTAGON = 7;

		public const int FFT_SOURCE = -1;
		public const int FFT_NEWIMAGE = -2;
		public const int FFT_NEWFLOAT = -3;

		//-----------------------------------------------------------------
		// IPWIN definitions
		//-----------------------------------------------------------------
		public const int COLORMODEL = 100;
		public const int AUTOUPDATE = 102;
		public const int SCAL = 103;
		public const int ICAL = 104;
		public const int ACCUMULATE = 105;
		public const int LINETYPE = 106;
		public const int BIN = 107;
		public const int BRIGHTNESS = 108;
		public const int CONTRAST = 109;
		public const int GAMMA = 110;
		public const int STATISTICS = 111;
		public const int ORIGIN = 112;
		public const int GRID = 113;
		public const int REFERENCE = 114;
		public const int FREEZE = 115;
		public const int CURVE = 116;
		public const int UNIT = 117;
		public const int Channel = 118;
		public const int CHANNEL1 = 120;
		public const int CHANNEL2 = 121;
		public const int CHANNEL3 = 122;
		public const int ADVANCED = 123;
		public const int SETCURSEL = 124;
		public const int SEGMETHOD = 125;
		public const int SETTABS = 126;
		public const int LINEGEOMETRY = 127;
		public const int INVERT = 128;
		public const int SEGCLR_RED = 129;
		public const int SEGCLR_GREEN = 130;
		public const int SEGCLR_BLUE = 131;
		public const int CURSORSIZE = 132;
		public const int DEGREE = 133;
		public const int Threshold = 134;

		public const int PROFTYPE_LINE = 1016;
		public const int PROFTYPE_CIRCLE = 1017;
		public const int PROFTYPE_FREEFORM = 1018;

		public const int BLOB_OUTLINEMODE = 0;
		public const int BLOB_OUTLINECOLOR = 1;
		public const int BLOB_LABELMODE = 2;
		public const int BLOB_LABELCOLOR = 3;
		public const int BLOB_SMOOTHING = 4;
		public const int BLOB_MINAREA = 5;
		public const int BLOB_FILLHOLES = 6;
		public const int BLOB_BRIGHTOBJ = 8;
		public const int BLOB_AUTORANGE = 9;
		public const int BLOB_MEASUREOBJECTS = 10;
		public const int BLOB_FILTEROBJECTS = 11;
		public const int BLOB_ADDCOUNT = 12;
		public const int BLOB_CLEANBORDER = 13;
		public const int BLOB_CONVEX = 14;
		public const int BLOB_8CONNECT = 15;

		// for the IpSegPreview
		public const int PREVIEW_NONE = 0;
		public const int CURRENT_C_T = 1;
		public const int ALL_C_T = 2;
		public const int ALL_T_W = 3;
		public const int CURRENT_W_B = 4;
		public const int CURRENT_B_W = 5;
		public const int CURRENT_C_B = 6;
		public const int CURRENT_C_W = 7;
		public const int CURRENT_B_T = 8;
		public const int CURRENT_T_B = 9;
		public const int CURRENT_T_W = 10;
		public const int ALL_W_B = 11;
		public const int ALL_B_W = 12;
		public const int ALL_C_B = 13;
		public const int ALL_C_W = 14;
		public const int ALL_B_T = 15;
		public const int ALL_T_B = 16;

		public const int CM = 0;
		public const int INCHES = 1;
		public const int PIXELS = 2;

		public const int SEG_SELNEW = 0;
		public const int SEG_SELADD = 1;
		public const int SEG_SELSUBTRACT = 2;

		public const int SEG_HISTOGRAM = 0;
		public const int SEG_COLORCUBE = 1;

		public const int MASK_BILEVELNEW = 4;
		public const int MASK_BILEVELINPLACE = 5;
		public const int MASK_COLORNEW = 6;

		public const int FILLCOLOR = 0;
		public const int FILLHUE = 1;
		public const int FILLTINT = 2;
		public const int FILLPATTERN = 3;
		public const int FILLTEXTURE = 4;

		public const int CLRFORE = 0;
		public const int CLRBACK = 1;
		public const int CLRWHITE = 2;
		public const int CLRBLACK = 3;

		public const int FRAME_RESET = -1;
		public const int FRAME_NONE = 0;
		public const int FRAME_RECTANGLE = 1;
		public const int FRAME_ELLIPSE = 2;
		public const int FRAME_IRREGULAR = 3;
		public const int FRAME_INVIEW = 10;

		public const int THICKNORMAL = 0;
		public const int THICKHORZ = 1;
		public const int THICKVERT = 2;
		public const int THICKAVG = 3;
		public const int THICKSTDDEV = 4;

		public const int LUT_BRIGHTNESS = 0;
		public const int LUT_CONTRAST = 1;
		public const int LUT_GAMMA = 2;
		public const int LUT_HISHAD = 3;
		public const int LUT_4TONES = 4;
		public const int LUT_8TONES = 5;
		public const int LUT_FREEFORM = 6;
		public const int LUT_ALL = 7;

		// for IpLutData
		public const int LUT_GET_LENGTH = 0;
		public const int LUT_GET_DATA = 1;
		public const int LUT_SET_DATA = 2;

		// measurement attributes
		public const int MEAS_STATS = 0;
		public const int MEAS_DISPCOLOR = 1;
		public const int MEAS_LABELCOLOR = 2;
		public const int MEAS_THICKMODE = 3;
		public const int MEAS_REPEAT = 9;
		public const int MEAS_ANGLE180 = 10;
		public const int MEAS_CLICK = 11;
		public const int MEAS_MEASCOLOR = 40;
		public const int MEAS_UPDATE = 41;
		public const int MEAS_PROMPTS = 42;
		public const int MEAS_SHOWLAYOUT = 43;
		public const int MEAS_DISPBFPTS = 44;
		public const int MEAS_MAXLINEPTS = 45;
		public const int MEAS_MAXCIRCLEPTS = 46;
		public const int MEAS_MAXARCPTS = 47;
		public const int MEAS_PASSFAILTYPE = 48;
		public const int MEAS_DISPCOUNTOPTS = 49;
		public const int MEAS_TAG = -2;
		public const int MEAS_ALL = -1;

		// measurement tools
		public const int MEAS_LENGTH = 4;
		public const int MEAS_AREA = 5;
		public const int MEAS_ANGLE = 6;
		public const int MEAS_TRACE = 7;
		public const int MEAS_THICK = 8;
		public const int MEAS_POINT = 20;
		public const int MEAS_RECT = 21;
		public const int MEAS_CIRCLE = 22;
		public const int MEAS_BFLINE = 23;
		public const int MEAS_BFCIRCLE = 24;
		public const int MEAS_BFARC = 25;
		public const int MEAS_DIST = 26;
		public const int MEAS_NEWANGLE = 27;
		public const int MEAS_HTHICK = 28;
		public const int MEAS_VTHICK = 29;
		public const int MEAS_CTHICK = 30;
		public const int MEAS_PERPDIST = 31;
		public const int MEAS_COUNT = 32;
		public const int MEAS_DATA_TO_IMAGE = 33;
		public const int MEAS_SELECT = 100;

		// measurement data types
		public const int MDATA_POS = 0;
		public const int MDATA_POSY = -1;
		public const int MDATA_AREA = -2;
		public const int MDATA_LEN = -3;
		public const int MDATA_RADIUS = -4;
		public const int MDATA_START = -5;
		public const int MDATA_STARTY = -6;
		public const int MDATA_END = -7;
		public const int MDATA_ENDY = -8;
		public const int MDATA_ANGLE = -9;
		public const int MDATA_AVGDIST = -10;
		public const int MDATA_MINDIST = -11;
		public const int MDATA_MAXDIST = -12;
		public const int MDATA_CTRDIST = -13;
		public const int MDATA_PERPDIST = -14;
		public const int MDATA_COUNT = -15;

		// special GET for IpMeasGet
		public const int GETNAME = 200;
		public const int GETFEATVALUES = 201;
		public const int GETNUMMEAS = 202;
		public const int GETMEASVALUES = 203;

		// measurement load options
		public const int MLOAD_INTERACTIVE = 1;

		// measurement IpMeasShow options
		public const int MEAS_HIDE = 0;
		public const int MEAS_SHOW = 1;
		public const int MEAS_SHOWADVANCED = 2;
		public const int MEAS_SHOWBASIC = 3;

		// measurement pass/fail tolerance options
		public const int MPF_NONE = 0;
		public const int MPF_TOLERANCES = 1;
		public const int MPF_MINMAX = 2;

		public const int TAG_VIEW_COUNTS = 0;
		public const int TAG_VIEW_POINTS = 1;
		public const int TAG_VIEW_CLASSSTATS = 2;
		public const int TAG_VIEW_AREA = 3;
		public const int TAG_VIEW_LABEL = 4;
		public const int TAG_VIEW_MARKER = 5;

		public const int TAG_MEAS_XPOS = 6;
		public const int TAG_MEAS_YPOS = 7;
		public const int TAG_MEAS_INTENSITY = 8;
		public const int TAG_MEAS_CLASS = 9;
		public const int TAG_MEAS_RED = 10;
		public const int TAG_MEAS_GREEN = 11;
		public const int TAG_MEAS_BLUE = 12;
		public const int TAG_MEAS_AREA = 13;
		public const int TAG_MEAS_RADIUS = 14;
		public const int TAG_ACTIVECLASS = 15;

		public const int AOIDELETE = 0;
		public const int AOIADD = 1;
		public const int AOISET = 2;
		public const int AOISHOWDLG = 3;
		public const int AOIHIDEDLG = 4;
		public const int AOILOAD = 5;
		public const int AOISAVE = 6;

		public const int DOCSEL_NEXTID = -1;
		public const int DOCSEL_PREVID = -2;
		public const int DOCSEL_ACTIVE = -3;
		public const int DOCSEL_ALL = -4;
		public const int DOCSEL_NONE = -5;

		public const int GET_VALUE = 0;
		public const int SET_VALUE = 1;

		// for IpAppArrange()
		public const int DOCS_CASCADE = 0;
		public const int DOCS_TILE = 1;
		public const int DOCS_OVERLAP = 2;

		// for IpAcqSnap()
		public const int ACQ_NEW = -1;
		public const int ACQ_CURRENT = -2;
		public const int ACQ_FILE = -3;
		public const int ACQ_SEQUENCE = -4;
		public const int ACQ_SEQUENCE_APPEND = -5;

		// for IpAppRun()
		public const int RUN_NORMAL = 5;
		public const int RUN_MINIMIZED = 2;
		public const int RUN_MAXIMIZED = 3;
		public const int RUN_AUTOCLOSE = 1;
		public const int RUN_MODAL = 2;

		// IpXxxxSave SaveMode flags
		// Data to send
		public const int S_DATA1 = 0x4;
		public const int S_DATA2 = 0x8;
		public const int S_STATS = 0x2;

		// Output format
		public const int S_TABLE = 0x40;
		public const int S_GRAPH = 0x80;

		// Destination
		public const int S_FILE = 0x0;
		public const int S_CLIPBOARD = 0x10;
		public const int S_OUTPUT = 0x20;
		public const int S_DDE = 0x1000;
		public const int S_DATABASE = 0x2000;
		public const int S_PRINTER = 0x4000;

		// Append or replace
		public const int S_APPEND = 0x1;
		public const int S_NEW = 0x1;

		// Adornments
		public const int S_HEADER = 0x100;
		public const int S_LEGEND = 0x200;
		public const int S_X_AXIS = 0x400;
		public const int S_Y_AXIS = 0x800;

		// Obsolete or old style name
		public const int S_RECORD = 0x2000;
		public const int S_DATA = 0x4;
		public const int S_RANGE = 0x8;

		// for IpAppMenuSelect()
		public const int MENU_ID = 1;
		public const int MENU_NAME = 2;
		public const int MENU_COORD = 4;
		public const int MENU_DLL = 8;
		public const int DLG_MENU_ID = 16;
		public const int DLG_MENU_NAME = 32;
		public const int DLG_MENU_COORD = 64;

		// for IpAppWindow()
		public const int APW_GETNAME = 0;
		public const int APW_GETID = 1;
		public const int APW_GETHWND = 2;
		public const int APW_ACTIVATENAME = 3;
		public const int APW_ACTIVATEID = 4;
		public const int APW_ACTIVATEHWND = 5;

		// for IpAppCtl()
		public const int APC_GETHWND = 1;
		public const int APC_CLICK = 2;
		public const int APC_GETFOCUSID = 3;
		public const int APC_SETFOCUSID = 4;
		public const int APC_GETCHECK = 5;
		public const int APC_SETCHECK = 6;
		public const int APC_GETSCROLL = 7;
		public const int APC_SETSCROLL = 8;
		public const int APC_GETCURSEL = 9;
		public const int APC_SETCURSEL = 10;
		public const int APC_SETPOSX = 11;
		public const int APC_SETPOSY = 12;

		// for IpAppWndState()
		public const int WST_ENABLED = 0x1;
		public const int WST_VISIBLE = 0x2;
		public const int WST_NORMAL = 0x4;
		public const int WST_MINIMIZED = 0x8;
		public const int WST_MAXIMIZED = 0x10;

		// for IpWsLoadRes()
		public const int LOAD_PROMPT = -1;
		public const int LOAD_SMALLEST = -3;

		// for IpPrtSize()
		public const int PRT_ACTUAL = 1;
		public const int PRT_FIT = 2;
		public const int PRT_DISTORT = 3;

		// for IpAppGet, IpDocGet() and IpWsChangeDescription()
		public const int GETACTDOC = 1;
		public const int GETDOCWND = 2;
		public const int GETDOCVRI = 3;
		public const int GETAPPWND = 4;
		public const int GETNUMDOC = 5;
		public const int GETDOCLST = 6;
		public const int GETDOCINFO = 7;
		public const int GETINSTINFO = 8;
		public const int GETPLUGSN = 9;
		public const int GETAPPDIR = 10;
		public const int GETAPPNAME = 11;
		public const int GETAPPVERSION = 12;
		public const int GETOSVERSION = 13;

		public const int INF_SUBJECT = 19;
		public const int INF_TITLE = 20;
		public const int INF_ARTIST = 21;
		public const int INF_DATE = 22;
		public const int INF_DESCRIPTION = 23;
		public const int INF_DPIX = 24;
		public const int INF_DPIY = 25;
		public const int INF_RANGE = 26;
		public const int INF_NAME = 27;
		public const int INF_MAXRANGE = 28;
		public const int INF_FILENAME = 29;
		public const int INF_XPOSITION = 30;
		public const int INF_YPOSITION = 31;
		public const int INF_ZPOSITION = 32;
		public const int INF_XSCROLL = 33;
		public const int INF_YSCROLL = 34;
		public const int INF_ZOOMFACTOR = 35;

		// for IpAppGet and IpAppSet
		public const int PST_BLEND_PREVIEW = 50;
		public const int PST_BLEND_APPLY = 51;
		public const int PST_APPLY_TYPE = 52;
		public const int PST_BLEND_SOURCE = 53;

		// paste apply types for use with PST_APPLY_TYPE
		public const int PST_APPLY_ALL = 0;
		public const int PST_APPLY_LIGHTER = 1;
		public const int PST_APPLY_DARKER = 2;

		// for IpBlbGet()
		public const int GETNUMOBJ = 1;
		public const int GETHBLOB = 2;
		public const int GETTHRESH = 3;
		public const int GETNUMRANGES = 4;
		public const int GETNUMSITES = 5;
		public const int GETSITESTATS = 6;
		public const int GETNUMOBJEX = 7;

		// flags for IpBlbGet() param2
		public const int BLB_ALLOBJECTS = 0x0;
		public const int BLB_INRANGE = 0x1;
		public const int BLB_ACTIVERANGE = 0x2;

		// for IpWsCreateFromVri()
		public const int VRI_SHARE = 0;
		public const int VRI_COPY = 1;
		public const int VRI_NODELETE = 2;

		// for IpMacroStop()
		public const int MS_MODAL = 0x1;
		public const int MS_YESNO = 0x10;
		public const int MS_OKCAN = 0x20;
		public const int MS_YESNOCAN = 0x40;
		public const int MS_STOP = 0x100;
		public const int MS_EXCLAM = 0x200;
		public const int MS_QUEST = 0x400;
		public const int MS_DEF2 = 0x800;
		public const int MS_DEF3 = 0x1000;

		// for IpBitAttr()
		public const int BIT_SAMPLE = 1;
		public const int BIT_SAVEALL = 2;
		public const int BIT_CALIB = 3;

		// for IpSortAttr()
		public const int SORT_ROTATE = 1;
		public const int SORT_MEAS = 2;
		public const int SORT_LABELS = 3;
		public const int SORT_COLOR = 4;
		public const int SORT_INDEX = 5;
		public const int SORT_AUTO = 6;

		// for IpRegister()
		public const int AFF_CLIP = 0x1;
		public const int AFF_AOI = 0x2;
		public const int AFF_NOBILINEAR = 0x4;
		public const int AFF_FLOAT = 0x8;
		public const int AFF_NOTILT = 0x10;
		public const int AFF_NOSCALE = 0x20;

		// for IpAoiGet()
		public const int AOI_BOX = 1;
		public const int AOI_ELLIPSE = 3;
		public const int AOI_POLYGON = 5;

		// used by various query functions
		public const int GETNUMPTS = 100;
		public const int GETBOUNDS = 101;
		public const int GETPOINTS = 102;
		public const int GETSTATS = 103;
		public const int GETVALUES = 104;
		public const int GETRANGE = 105;
		public const int GETTYPE = 106;
		public const int GETSTATUS = 107;
		public const int GETLABEL = 108;
		public const int GETINDEX = 109;
		public const int GETHIT = 110;
		public const int GETNUMCLASS = 111;
		public const int GETNUMSAMPLES = 112;
		public const int GETLNUMPTS = 113;
		public const int GETCHANNELS = 114;
		public const int GETRANGESTATS = 115;

		// for IpTrackBar()
		public const int TBOPEN = 1;
		public const int TBUPDATE = 2;
		public const int TBCLOSE = 3;

		// for IpDocGetLine(), IpDocGetArea()
		public const int CPROG = 0x2000;
		public const int USEAOI = 0x1000;

		// for IpDocGet()
		[StructLayout(LayoutKind.Sequential)]
		public struct IPDOCINFO
		{
			public short width;
			public short Height;
			public short Class;
			public short BPP;
			public RECT Extent;
		}

		// for IpAcqShow()
		public const int ACQ_SNAP = 1;
		public const int ACQ_AVG = 2;
		public const int ACQ_TIMED = 3;
		public const int ACQ_MULTI = 4;
		public const int ACQ_LIVE = 5;
		public const int ACQ_ISLIVE = 6;
		public const int ACQ_ISSHOWN = 7;
		public const int ACQ_SETUP = 8;
		public const int ACQ_SETTINGS = 9;
		public const int ACQ_MACROS = 10;
		public const int ACQ_ISINITIALIZED = 11;
		public const int ACQ_SHOWLAST = 12;

		// for IpAcqSettings()
		public const int ACQ_LOAD = 0;
		public const int ACQ_SAVE = 1;
		public const int ACQ_GETCURRENT = 2;

		// for IpIniFile()
		public const int GETINT = 1;
		public const int GETFLOAT = 2;
		public const int GETSTRING = 3;
		public const int SETINT = 5;
		public const int SETFLOAT = 6;
		public const int SETSTRING = 7;

		// for IpCalSaveEx()
		public const int NONAME = 1;
		public const int NOSYSTEM = 2;

		// for IpSCalShow() and IpSCalShowEx()
		public const int SCAL_HIDE = 0;
		public const int SCAL_DLG_MAIN = 1;
		public const int SCAL_DLG_SELECT = 2;
		public const int SCAL_ADD_MARKER = 3;
		public const int SCAL_MINIMIZE = 4;
		public const int SCAL_DLG_WIZARD = 5;
		public const int SCAL_DLG_SYSTEM = 6;
		public const int SCAL_SHOW = 7;
		public const int SCAL_HIDEALL = 8;

		// constants for IpSCal functions
		public const int SCAL_CURRENT_CAL = -1;
		public const int SCAL_SYSTEM_CAL = -2;
		public const int SCAL_ALL = -3;
		public const int SCAL_ALL_REF = -4;

		// for IpSCalGetLong() and IpSCalSetLong()
		public const int SCAL_NUM_ALL = 0;
		public const int SCAL_NUM_REF = 1;
		public const int SCAL_GET_ALL = 2;
		public const int SCAL_GET_REF = 3;
		public const int SCAL_ONIMAGE_COLOR = 10;
		public const int SCAL_CURRENT = 11;
		public const int SCAL_SYSTEM = 12;
		public const int SCAL_MARKER_STYLE = 13;
		public const int SCAL_SHOW_REF_ONLY = 14;
		public const int SCAL_UNIT_CONVERT = 15;
		public const int SCAL_ADD_TO_REF = 30;
		public const int SCAL_REMOVE_FROM_REF = 31;
		public const int SCAL_APPLY = 32;

		// for IpSCalGetSng() and IpSCalSetSng()
		public const int SCAL_ASPECT = 50;
		public const int SCAL_CONVERSION_TO_MM = 51;
		public const int SCAL_SCALE_X = 60;
		public const int SCAL_SCALE_Y = 61;
		public const int SCAL_ORIGIN_X = 62;
		public const int SCAL_ORIGIN_Y = 63;
		public const int SCAL_ANGLE = 64;
		public const int SCAL_SYSTEM_MODIFIER = 65;
		// changer or anything else that affects the overall magnification of the optics
		public const int SCAL_MARKER_WIDTH = 66;

		// for IpSCalGetStr() and IpSCalSetStr()
		public const int SCAL_NAME = 100;
		public const int SCAL_UNITS = 101;
		public const int SCAL_FIND_BY_NAME = 102;
		// Note: the Calibration parameter must be SCAL_ALL or SCAL_ALL_REF

		// marker styles for use with IpSCalGetLong and IpSCalSetLong SCAL_MARKER_STYLE command
		public const int SCAL_MARKER_BONW = 0;
		public const int SCAL_MARKER_BONWB = 1;
		public const int SCAL_MARKER_WONB = 2;
		public const int SCAL_MARKER_WONBB = 3;
		public const int SCAL_MARKER_ND_X = 4;
		public const int SCAL_MARKER_ND_XY = 5;
		public const int SCAL_MARKER_ND_Y = 6;

		// for IpICalShow()
		public const int ICAL_HIDE = 0;
		public const int ICAL_SHOW = 1;
		public const int ICAL_MINIMIZE = 4;

		// constants for IpICal functions
		public const int ICAL_CURRENT_CAL = -1;
		// Note: A constant cannot be provided to represent the current system calibration because there are multiple system intensity
		// calibrations, one per image class. Use IpICalGetSystem to get the system calibration for a particular image class.
		public const int ICAL_ALL = -2;
		public const int ICAL_ALL_REF = -3;

		// for IpICalGetLong() and IpICalSetLong()
		public const int ICAL_NUM_ALL = 0;
		public const int ICAL_NUM_REF = 1;
		public const int ICAL_GET_ALL = 2;
		public const int ICAL_GET_REF = 3;
		public const int ICAL_NUM_SAMPLES = 5;
		public const int ICAL_CURRENT = 10;
		public const int ICAL_ADD_TO_REF = 20;
		public const int ICAL_REMOVE_FROM_REF = 21;
		public const int ICAL_APPLY = 22;

		// for IpICalGetSng() and IpICalSetSng()
		public const int ICAL_OD_BLACK = 10;
		public const int ICAL_OD_INCIDENT = 11;

		// for IpICalGetStr() and IpICalSetStr()
		public const int ICAL_NAME = 30;
		public const int ICAL_UNITS = 31;

		// for IpAnotAttr()
		public const int DRAW_FILLCOLOR = 0;
		public const int DRAW_LINECOLOR = 1;
		public const int DRAW_LINEWIDTH = 2;
		public const int DRAW_THINLINE = 0;
		public const int DRAW_THICKLINE = 1;

		// for IpFltLocHistEq()
		public const int LOCEQ_LINEAR = 1;
		public const int LOCEQ_BELL = 2;
		public const int LOCEQ_LOG = 3;
		public const int LOCEQ_EXP = 4;
		public const int LOCEQ_BESTFIT = 5;
		public const int LOCEQ_STDDEV = 6;

		// for IpFltDistance()
		public const int DISTANCE_SQUARE = 0;
		public const int DISTANCE_DIAGONAL = 1;
		public const int DISTANCE_EUCLIDEAN = 2;

		// for IpFltReduce()
		public const int REDUCE_4NEIGHBOR = 2;
		public const int REDUCE_8NEIGHBOR = 0;
		public const int REDUCE_16NEIGHBOR = 4;

		// for IpFltBranchEnd()
		public const int BR_END = 32;
		public const int BR_SKEL = 16;
		public const int BR_BRANCH3 = 64;
		public const int BR_BRANCHN = 128;

		// for IpAnotLine()
		public const int DRAW_PLAINLINE = 0;
		public const int DRAW_SMALLARROWRIGHT = 1;
		public const int DRAW_SMALLARROWBOTH = 2;
		public const int DRAW_SMALLARROWLEFT = 3;
		public const int DRAW_LARGEARROWRIGHT = 4;
		public const int DRAW_LARGEARROWLEFT = 5;
		public const int DRAW_LARGEARROWBOTH = 6;
		public const int DRAW_CIRCLEARROW = 7;
		public const int DRAW_ARROWCIRCLE = 8;
		public const int DRAW_DIAMONDBOTH = 9;
		public const int DRAW_CIRCLEBOTH = 10;

		// Plot and overlay definitions
		public const int PDT_INT16 = 0;
		public const int PDT_WORD16 = 1;
		public const int PDT_INT32 = 2;
		public const int PDT_WORD32 = 3;
		public const int PDT_FLOAT = 4;
		public const int PDT_DFLOAT = 5;

		//#define ATT_FIXED 0x8000
		public const int ATT_FIXED = -32768;
		public const int ATT_NOCOPY = 0x4000;
		public const int ATT_FIXEDX = 0x2000;
		public const int ATT_FIXEDY = 0x1000;
		public const int ATT_CONTROLS = 0x800;
		public const int ATT_CALIPER = 0x400;

		public const int SETHWNDMESSAGE = 1;
		public const int GETEDITPOINT = 1;
		public const int GETCURPOS = 2;
		public const int RECTANGLE = -10;

		// constants for IpPlot... functions

		public const int XAXIS = 0;
		public const int YAXIS = 1;

		public const int GETGRAPH = 1;
		public const int GETHWND = 2;
		public const int SETPARENT = 3;
		public const int SETNOTIFY = 4;

		public const int RGE_FIXED = 0;
		public const int RGE_AUTO = 1;
		public const int RGE_FIXEDMIN = 2;
		public const int RGE_FIXEDMAX = 3;


		// for IpWsConvertImage() - conversion methods
		public const int CONV_SCALE = 0;
		public const int CONV_SHIFT = 1;
		public const int CONV_DIRECT = 2;
		public const int CONV_USER = 3;
		public const int CONV_MCOLOR = 4;
		public const int CONV_MEDIAN = 5;
		public const int CONV_PSEUDOCOLOR = 6;

		// for IpDocGetPosition()
		[StructLayout(LayoutKind.Sequential)]
		public struct IPDOCPOS
		{
			public short IsKnown;
			public float Position;
		}

		// constants for frame selection for the IpDocGetProp and IpDocSetProp routines
		public const int DOC_ACTIVEFRAME = -1;
		public const int DOC_ACTIVEPORTION = -2;
		public const int DOC_ENTIREIMAGE = -3;
		// for IpDocGetPropDbl() and IpDocSetPropDbl()
		public const int DOCPROP_XPOSITION = 0;
		public const int DOCPROP_YPOSITION = 1;
		public const int DOCPROP_ZPOSITION = 2;
		public const int DOCPROP_EMWAVELENGTH = 3;
		public const int DOCPROP_EXWAVELENGTH = 4;
		public const int DOCPROP_REFINDEX = 5;
		public const int DOCPROP_NUMAPERTURE = 6;

		// for IpDocGetPropStr() and IpDocSetPropStr()
		public const int DOCPROP_CHANNELNAME = 10;
		public const int DOCPROP_SITELABEL = 11;

		// for IpDocGetPropDate() and IpDocSetPropDate()
		public const int DOCPROP_TIME = 20;
		public const int DOCPROP_TIMEPOINT = 21;

		// for IpPcTint()
		public const int TINT_REMOVE = 0;
		public const int TINT_RED = 1;
		public const int TINT_GREEN = 2;
		public const int TINT_BLUE = 3;

		// for IpToolbarGetStr
		public const int IPTB_TOOLBAR = 1;

		// constants used for several Apply functions
		public const int APPLYTO_FRAME = 1;
		public const int APPLYTO_IMAGE = 2;
		public const int APPLYTO_PORTION = 3;
		public const int APPLYTO_SET = 4;
		public const int APPLYTO_ZSTACK = 5;
		public const int APPLYTO_CHANNEL = 6;

		//-----------------------------------------------------------------
		// Error codes returned by Auto-Pro functions
		//
		// All error codes are negative values.
		// IPCERR_NONE (0) indicates no error.
		// Positive values may be used by certain functions to return results.
		//-----------------------------------------------------------------
		public const int IPCERR_NONE = 0;

		public const int IPCERR_APPINACTIVE = -1;
		public const int IPCERR_NOTFOUND = -2;
		public const int IPCERR_DLLNOTFOUND = -3;
		public const int IPCERR_FUNCNOTFOUND = -4;
		public const int IPCERR_INVCOMMAND = -5;
		public const int IPCERR_NODOC = -6;
		public const int IPCERR_INVARG = -7;
		public const int IPCERR_MEMORY = -8;
		public const int IPCERR_BUSY = -9;
		public const int IPCERR_EMPTY = -10;
		public const int IPCERR_LIMIT = -11;
		public const int IPCERR_CANCELLED = -12;
		public const int IPCERR_FUNC = -1000;


		//-----------------------------------------------------------------
		// Video acquisition
		//-----------------------------------------------------------------
		[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAcqAverage(short Frames, short Divider);
		[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAcqSnap(short dest);
		[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAcqTimed(string szDir, string szPrefix, short StartNumber, short Frames, int Interval);
		[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAcqShow(short wDialog, short bShow);
		[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAcqMultiSnap(short startframe, short numframes, short dest);
		//[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//public static extern int IpAcqControl(short Cmd, short Param, ref Any lpParam);			//TODO As Any
        
        
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAcqSettings(string lpszFile, short bSave);
		[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAcqSelectDriver(string lpszFile, short bGetDriv);


		//-----------------------------------------------------------------
		// Aoi manager functions
		//-----------------------------------------------------------------
		[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAoiCreateBox(ref RECT ipRect);
		[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAoiCreateEllipse(ref RECT ipRect);
		[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAoiCreateIrregular(ref POINTAPI ipAoiPoint, short numpoints);
		[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAoiShow(short FrameType);
		[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAoiManager(short FuncID, string Name);
		[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAoiChangeName(string oldName, string newName);
		[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAoiMove(short deltax, short deltay);
		[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAoiValidate();
		//[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//public static extern int IpAoiGet(short command, short wParam, ref Any lpParam);		//TODO As Any
		[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAoiMultAppend(short append);
		[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAoiMultShow(short yesNo);


		//-----------------------------------------------------------------
		// Application functions
		//-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppArrange(short Mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppCloseAll();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppExit();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppMaximize();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppMinimize();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppMove(short X, short Y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppRestore();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppSelectDoc(short docid);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppSize(short Width, short Height);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppUpdateDoc(short docid);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppHide(short hide);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppRun(string CommandLine, short Mode, short AutoClose);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int IpAppGet(short command, short wParam, ref Any lpParam);		//TODO As Any
        public static extern int IpAppGet(short command, short wParam, ref int lpParam);		//TODO As Any
        
        
        
        [DllImport("IPC32", EntryPoint = "IpAppGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppGet2(short sCmd, short sParam, string lpParam);
        // Use for GETAPPNAME and GETAPPDIR
        [DllImport("IPC32", EntryPoint = "IpAppGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppGetStr(short sCmd, short sParam, string lpParam);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppMenuSelect(short Id1, short Id2, string itemName, short Mode);
        
        //使用していない
        //[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//public static extern int IpAppWindow(string WindowName, ref Any WindowParam, short Mode);		//TODO As Any
        //[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//public static extern int IpAppCtl(string CtlName, short ParamCmd, ref Any lpParamValue);		//TODO As Any
        
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppWndPos(string WindowName, ref RECT lpRect, short Mode);

        //使用していない
        //[DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//public static extern int IpAppWndState(string WindowName, ref Any theState, short Mode);		//TODO As Any
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppCtlText(string ControlName, string Caption, short Mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppCloseTools();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppSet(short command, short Attr);

        //-----------------------------------------------------------------
        // Blob manager functions
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbCount();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbDelete();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbEnableMeas(short MeasurementType, short bEnable);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbLoadOutline(string OutlineFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbLoadSetting(string SettingFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSaveData(string DataFile, short append);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSaveOutline(string OutlineFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSaveSetting(string SettingFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSetAttr(short Attrib, short Value);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSetRange(short iStart, short iEnd);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowAutoClass(ref short ipClassifiers, short NumMeas, short NumClasses, short bIterate, short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowCluster(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowData(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowHistogram(short Measure, short Bins, short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowObjectWindow(short bShow);
        [DllImport("IPC32", EntryPoint = "IpBlbShowScattergram", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowScatterGram(short Measure1, short Measure2, short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowSingleClass(short NumMeasurements, ref float ipBins, short NumClasses, short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowStatistics(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSplitObjects(short bWatershed);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbUpdate(short bRedrawImage);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbRemoveHoles();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSmoothObjects(short smoothing);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSavePopDensities(string DataFile, short append);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSaveClasses(string DataFile, short append);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowPopDens(string OutlineFile, short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbMeasure();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbFilter();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSetFilterRange(short MeasurementType, float Min, float Max);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbData(short Measure, short fromObj, short toObj, ref float values);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbGet(short command, short tag, short Param2, ref Any lpParam);		//TODO As Any
        // Use for GETLABEL
        [DllImport("IPC32", EntryPoint = "IpBlbGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbGetStr(short sCmd, short sTag, short sParam, string lpParam);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbCreateMask();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbMultiRanges(ref float intRanges, short numranges);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbRange(short rangeid);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbHideObject(short objnum, short rangeid, short hide);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbFromAoi(short sMode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSetRangeEx(short sRange, float fStart, float fEnd);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbHitTest(short X, short Y);

        //-----------------------------------------------------------------
        // calibration functions
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCalLoad(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCalSave(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCalSaveAll(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCalSaveEx(string FileName, short docid, short Mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCalGet(string szInput, string szOutput);

        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalCreate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalDestroy();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalMove(short X, short Y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalReset();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSelect(string szICal);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSetName(string szICal);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSetOptDens(float BlackLevel, float IncidentLevel);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSetPoints(ref float ipICalPoints, short numpoints, short fitmode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSetSamples(short NumSamples);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSetUnitName(string UnitName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalShowFormat(short bOptDens);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalLinearize(short bNewImage, short bInverse, short bScale);


        // additional intensity calibration functions
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalLoad(string FileName, short Ref);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSave(int Calibration, string FileName);
        // functions that work with specific calibrations
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalDestroyEx(int Calibration);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSetPointsEx(int Calibration, ref float ipICalPoints, short numpoints, short fitmode);
        // functions that get or set calibration information
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalGetLong(int Calibration, short sAttribute, ref int CurrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalGetSng(int Calibration, short sAttribute, ref float CurrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalGetStr(int Calibration, short sAttribute, string CurrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSetLong(int Calibration, short sAttribute, int newValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSetSng(int Calibration, short sAttribute, float newValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSetStr(int Calibration, short sAttribute, string newValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalGetSystem(short wClass);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSetSystem(int Calibration, short wClass);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSetSystemByName(string CalName, short wClass);

        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalCreate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalDestroy();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalMove(short X, short Y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalReset();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSelect(string szSCal);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSetAngle(float Angle);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSetAspect(float AspectRatio);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSetName(string szSCal);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSetOrigin(float X, float Y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSetUnit(float X, float Y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSetUnitName(string UnitName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalShow(short Show);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalShowEx(short Dialog, short Show);


        // additional spatial calibration functions
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalLoad(string FileName, short Ref);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSave(int Calibration, string FileName);
        // functions that work with specific calibrations
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalDestroyEx(int Calibration);
        // functions that get or set calibration information
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalGetLong(int Calibration, short sAttribute, ref int CurrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalGetSng(int Calibration, short sAttribute, ref float CurrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalGetStr(int Calibration, short sAttribute, string CurrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSetLong(int Calibration, short sAttribute, int newValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSetSng(int Calibration, short sAttribute, float newValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSetStr(int Calibration, short sAttribute, string newValue);


        //-----------------------------------------------------------------
        // Screen capture
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCapArea(ref RECT ipRect, short bCursor);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCapFile(string FileFormat, string Directory, string Prefix, short Number);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCapHotKey(string KeyName, short bShift, short bCtrl, short bAlt);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCapWindow(string Title, short bClientOnly, short bCursor);

        //-----------------------------------------------------------------
        // Color Transformation Functions
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCmChannelExtract(short cmColor, short cmComp, short Channel);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCmChannelMerge(short WsId, short cmColor, short Channel);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCmTransform(short cmOut, short cmIn, short bNewImage);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCmChannelMerge3(short WsIdDest, short WsIdChannel1, short WsIdChannel2, short WsIdChannel3, short cmColor, short bNewImage);


        //----------------------------------------------------------------
        // Document window functions
        //----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocClose();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocMaximize();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocMinimize();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocMove(short X, short Y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocRestore();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocSize(short Width, short Height);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int IpDocGet(short command, short wParam, ref Any lpParam);					//TODO As Any
        public static extern int IpDocGet(short Command, short wParam, ref int lpParam);

        // Use for INF_NAME, INF_TITLE, INF_ARTIST, INF_DATE and INF_DESCRIPTION
        [DllImport("IPC32", EntryPoint = "IpDocGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocGetStr(short sCmd, short sParam, string lpParam);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocOpenVri(short docid, short oMode, ref RECT oExtent);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocOpenAoi(short docid, short oMode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocCloseVri(int iiInst);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpDocGetLine(int iiInst, short LineNum, ref Any LineBuf);					//TODO As Any
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpDocPutLine(int iiInst, short LineNum, ref Any LineBuf, short bAOI);		//TODO As Any
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpDocGetArea(short docid, ref RECT rArea, ref Any lpBuf, short gMode);		//TODO As Any
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpDocPutArea(short docid, ref RECT rArea, ref Any lpBuf, short pMode);		//TODO As Any
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocClick(string Message, ref POINTAPI CurPos);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocFind(string WorkspaceName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocCloseEx(short docid);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpDocGetAreaSize(short docid, ref RECT rArea, short gMode, ref Any lplSize);			//TODO As Any
        // The DocPosition parameter should be an IPDOCPOS structure
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpDocGetPosition(short docid, short PositionId, int frame, ref Any DocPosition);		//TODO As Any
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocSetPosition(short docid, short PositionId, int frame, double DocPosition);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocGetPropDbl(short docid, short PositionId, int frame, ref double DocProperty);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocSetPropDbl(short docid, short PositionId, int frame, double DocProperty);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocGetPropStr(short docid, short PositionId, int frame, string DocProperty);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocSetPropStr(short docid, short PositionId, int frame, string DocProperty);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocGetPropDate(short docid, short PositionId, int frame, ref DateTime DocProperty);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocSetPropDate(short docid, short PositionId, int frame, DateTime DocProperty);


        //-----------------------------------------------------------------
        // FFTDLG.DLL - Fast Fourier Transform functions
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftForward(short DisplayType, short bFullFft);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftHiPass(short iType, short Transition, short PreserveNil);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftInverse(short WsId, short PreserveData);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftLoad(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftLoPass(short iType, short Transition);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftSave(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftSpikeCut(short iType, short Transition, short Symmetrical);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftSpikeBoost(short iType, short Transition, short Symmetrical);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftTag(short docid, short iType, short sourceClass);


        //-----------------------------------------------------------------
        // FILTRDLG.DLL - Spatial filtering
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltClose(short shape, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltConvolveKernel(string KernelName, short Strength, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltDilate(short shape, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltErode(short shape, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltExtractBkgnd(short BrightOnDark, short ObjectSize);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltFlatten(short BrightOnDark, short ObjectSize);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltHiPass(short sSize, short Strength, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltLaplacian(short sSize, short Strength, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltLoPass(short sSize, short Strength, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltMedian(short sSize, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltRank(short sSize, short Threshold, short Rank, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltOpen(short shape, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltPhase();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltRoberts();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltSharpen(short sSize, short Strength, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltSobel();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltThin(short Threshold);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltThinEx(short Threshold, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltWatershed(short Threshold);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltWatershedEx(short Threshold, short Erosions);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltVariance(short SizeX, short SizeY);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltPrune(short Threshold, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltUserErode(string KernelName, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltUserDilate(string KernelName, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltLocHistEq(short WinSize, short Step, short EqType, float StdDev);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltDistance(short Threshold, short Mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltReduce(short Threshold, short Mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltGauss(short sSize, short Strength, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltBranchEnd(short Threshold, short Classify);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltDespeckle(short sSize, short Strength, short Passes);


        //-----------------------------------------------------------------
        // Gallery
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalAdd(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalChangeDescription(short DescriptionType, string Description);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalDelete(string GalleryName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalNew(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalOpen(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalOpenPhotoCD(string DriveLetter, string GalleryName, short Resolution);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalRemove(short bFromDisk);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalSetActive(short GalId);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalSort(short bByName, short bAscending);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalTag(short SlotNumber, short bTag);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalUpdate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalImageOpen(short imageId);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalClose();


        //-----------------------------------------------------------------
        // Histogram functions
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstEqualize(short Method);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstCreate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstDestroy();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstMaximize();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstMinimize();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstMove(short X, short Y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstRestore();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstSave(string FileName, short bAppend);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstScale(short bVert, short bAuto, float From, float iEnd);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstSelect(short HstId);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstSetAttr(short AttrType, short AttrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstSize(short CX, short CY);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstUpdate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstGet(short command, short wParam, ref Any lpParam);		//TODO As Any


        //-----------------------------------------------------------------
        // Contrast Enhancement (Lookup Table)
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLutApply();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpLutBinarize(short MinRange, short MaxRange, short WHITEONBLACK);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLutLoad(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLutReset(short Channel, short iType);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLutSave(string FileName, string Description);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLutSetAttr(short AttrType, short AttrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLutSetControl(short ControlType, ref short ipLutControls, short Count);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLutShow(short Show);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpLutData(short sAttrType, ref Any pData);						//TODO As Any


        //-----------------------------------------------------------------
        // Measurements
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasAttr(short bAttr, short Value);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasDelete(short bMeasurement);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasLoadOutline(string OutlineFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasMove(short X, short Y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasRestore();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasSaveData(string DataFile, short saveMode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasSaveOutline(string OutlineFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasShow(short Show);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasSize(short CX, short CY);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasTool(short tool);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasTag(short Id, short OnOff);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasGet(short command, short wParam, ref Any lpParam);		//TODO As Any
        // Use for GETNAME
        [DllImport("IPC32", EntryPoint = "IpMeasGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasGetStr(short sCmd, short sParam, string lpParam);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasLoad(string MeasFile, short loadMode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasSave(string MeasFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasAddMeasure(short Feature, short Measure, float TargetVal, float MinTol, float MaxTol);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasDelMeasure(short Index);


        //-----------------------------------------------------------------
        // Manual Tagging
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagAttr(short bAttr, short Value);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagDelete(short Index);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagLoadPoints(string PointsFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagSaveData(string DataFile, short saveMode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagSavePoints(string PointsFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagSaveEnv(string PointsFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagLoadEnv(string PointsFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagUpdate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagGet(short command, short wParam, ref Any lpParam);		//TODO As Any
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagPt(short xPos, short yPos, short PointClass);


        //-----------------------------------------------------------------
        // Arithmetics and logical operations
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOpBkgndCorrect(short WsBackId, short BlackLevel, short bNewImage);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOpBkgndSubtract(short WsBackId, short bNewImage);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOpImageArithmetics(short WsId, float Number, short OpaCode, short bNewImage);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOpImageLogic(short WsId, short OplCode, short bNewImage);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOpNumberArithmetics(float Number, short OpaCode, short bNewImage);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOpNumberLogic(short Number, short OplCode, short bNewImage);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOpShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOpNumberRgb(ref float Number, short OpaCode, short bNewImage);


        //-----------------------------------------------------------------
        // Restricted Dilation
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltRstrDilate(short WsId, short nThresh, short nDilateType, short nIterations);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltRstrDilateShow(short bShow);


        //-----------------------------------------------------------------
        // Palette tool
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPalShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPalSetPaletteColor(short PaletteIndex, short Red, short Green, short Blue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPalSetGrayBrush(short bForeGround, short GrayIndex);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPalSetPaletteBrush(short bForeGround, short PaletteIndex);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPalSetRGBBrush(short bForeGround, short Red, short Green, short Blue);


        //-----------------------------------------------------------------
        // Line profile
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfCreate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfDestroy();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfLineMove(short x1, short y1, short x2, short y2);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfMaximize();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfMinimize();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfMove(short X, short Y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfRestore();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfSave(string FileName, short bAppend);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfSelect(short ProfId);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfSetAttr(short AttrType, short AttrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfSize(short CX, short CY);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfUpdate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfGet(short command, short wParam, ref Any lpParam);		//TODO As Any


        //-----------------------------------------------------------------
        // IPPRINT.DLL - Printing
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPrtHalftone(short bUsePrtHalftone, short bUsePrtScaling, short HalftoneType, short HaltoneOption);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPrtPage(short PageNo, short bUsePrtComp, short Copies);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPrtSize(short bActual, short bCentered, float Top, float Left, float Width, float Height, short bSmooth);	//TODO As Any
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPrtScreen(short PageNo, short bUsePrtComp, short Copies);


        //-----------------------------------------------------------------
        // Pseudo color
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcDefineColorSpread(short ColorSpread, int ClrFrom, int ClrTo, short Method);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcLoad(string PseudoColorFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcSave(string PseudoColorFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcSaveData(string FileName, short flag);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcSetColor(short DivNo, short Red, short Green, short Blue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcSetColorSpread(short ColorSpread);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcSetDivisions(short Divisions);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcSetRange(short DivNo, short FromVal, short ToVal);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcTint(short Tint);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcApplyDyeTint(string DyeFile);


        //-----------------------------------------------------------------
        // scan functions
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpScanShow();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpScanSelect();


        //-----------------------------------------------------------------
        // color segmentation functions
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegCreateMask(short MaskType, short MaskMethod, short MaskClass);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegLoad(string ColorRangesFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegMerge(string ColorRangesFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegReset();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegSave(string ColorRangesFile, short bHSI);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegPreview(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegSetAttr(short AttrType, short AttrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegNew(string ClassName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegDelete(string ClassName, short Index);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegRename(short Index, string ClassName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegSetRange(short nChannel, float FromVal, float ToVal);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegSelect(short SelectionType, short Sensitivity);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegSelectArea(short SelectionType, short Sensitivity, short xPos, short yPos, short nSize);


        //-----------------------------------------------------------------
        // Workspace functions
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsChangeDescription(short DescriptionType, string Description);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsChangeInfo(short InfoType, short Info);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertFile(string DstFile, string DstFormat, string SrcFile, string SrcFormat, short Compr, short imClass, short HalfType, short HalfOpt, short Dpi);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToBilevel(short HalftoneType, short Screen, short OutputDpi);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToFloat();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToGray();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToGray12();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToGray16();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToPaletteMColor();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToPaletteMedian(short StartIndex, short NumColors);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToRGB();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertImage(short iType, short Method, int InStart, int InEnd, int OutStart, int OutEnd);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsCopy();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsCreate(short Width, short Height, short Dpi, short Class);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsCreateFromClipboard();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsDuplicate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsFill(short FillType, short ColorType, short Transparency);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsFillPattern(string PatternFile);
        // IPCFUNC IpWsGetId(LPSTR WsName, LPSHORT Id);
        // IPCFUNC IpWsGetName(short Id, LPSTR WsName, short WsNameBufSize);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsLoad(string FileName, string FileFormat);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsLoadNumber(short Number);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsLoadPreview(string FileName, string FileFormat, short Left, short Top, short Right, short Bottom);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsMove(short X, short Y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsOrient(short OrientType);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsOverlay(string SrcImageName, short Transparency, short DoFloatingOverlay);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsOverlayEx(short SrcImgDocID, short X, short Y, short Transparency, short ApplyType);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsPan(short X, short Y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsPaste(short X, short Y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsPasteEx(string Prompt, string UndoFunction);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsRedo(short Number);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsReload();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsRotate(float Angle, short Anchor);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsRulerShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsRulerType(short RulerType);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsSave();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsSaveAs(string FileName, string FileFormat);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsSaveEx(string FileName, string FileFormat, short Compression, short BitsPerPlane);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsScale(short Width, short Height, short bSmooth);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsTestStrips(short HorzPage, short VertPage, short iType, short MinValue, short MaxValue, short Reduction, short bRed, short bGreen, short bBlue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsTestStrips2(short HorzPage, short VertPage, short Type1, short MinValue1, short MaxValue1, short Type2, short MinValue2, short MaxValue2, short Reduction, short bRed, short bGreen, short bBlue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsTestStripsHalftone(short AllTypes, short Color, ref short ipHalfTypes, ref short ipHalfScreens, short OutputDpi, short Reduction);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsUndo(short Number);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsZoom(short PercentZoom);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsLoadSetRes(short Number);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsCreateFromVri(short Vri, string Name, short Mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsGray12To8(short start12, short end12, short start8, short end8);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsGray16To8(short start16, short end16, short start8, short end8);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsStretchLut(short Mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsCreateEx(short Width, short Height, short Dpi, short Class, int lNumFrames);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToRGB36();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToRGB48();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToGrayEx(int start16, int end16, short start8, short end8);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToRGBEx(int start16, int end16, short start8, short end8);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsCopyFrames(int lStart, int lNumber);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsCutFrames(int lStart, int lNumber);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsDeleteFrames(int lStart, int lNumber);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsPasteFrames(int lPosition);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsSelectFrames(int lStart, int lNumber);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsSubSampleFrames(int lOffset, int lInterval);


        //-----------------------------------------------------------------
        // Template Mode functions
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTemplateMode(short OnOff);


        //-----------------------------------------------------------------
        // Macro Management functions
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMacroRun(string MacroName, string ScriptFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMacroLoad(string ScriptFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMacroStop(string Message, short exclusive);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMacroWait(short delay);


        //-----------------------------------------------------------------
        // Miscellaneous
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpStAutoName(string Format, short Number, string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpStSearchDir(string Directory, string filter, short Number, string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpStGetName(string Title, string Default, string filter, string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpStGetString(string Prompt, string istring, short maxlen);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpStGetInt(string Prompt, ref short Value, short initval, short Min, short Max);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpListPts(ref POINTAPI Points, string istring);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMorePts(string istring);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpStGetFloat(string Prompt, ref float Value, float initval, float Min, float Max, float inc);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTrackBar(short Cmd, short tValue, string Caption);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpIniFile(short Cmd, string ValName, ref Any lpVal);		//TODO As Any
        [DllImport("IPC32", EntryPoint = "IpIniFile", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for GETSTRING and SETSTRING
        public static extern int IpIniFileStr(short sCmd, string lpName, string lpValue);


        //-----------------------------------------------------------------
        // Registration
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpRegShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpRegister(ref POINTAPI FromPoints, ref POINTAPI ToPoints, short numpoints, short AffCode);


        //-----------------------------------------------------------------
        // Macro Output functions
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOutput(string Message);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOutputShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOutputClear();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOutputSave(string FileName, short Mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOutputSet(short sCmd, short sParam, ref Any lpParam);		//TODO As Any


        //-----------------------------------------------------------------
        // Bitmap analysis functions
        //-----------------------------------------------------------------

        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBitShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBitAttr(short bAttr, short Value);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBitSaveData(string DataFile, short saveMode);


        //-----------------------------------------------------------------
        // Object Sort
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSortAttr(short Attr, short Value);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSortShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSortObjects();

        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDde(short Cmd, string szStr1, string szStr2);


        //-----------------------------------------------------------------
        // Draw functions
        //-----------------------------------------------------------------

        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnotAttr(short Attr, int Value);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnotLine(ref POINTAPI lpPoints, short numpoints, short LineEndType, short bFilled);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnotBox(ref RECT lpBoxRect, short bFilled);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnotEllipse(ref POINTAPI lpCenter, short radx, short rady, short bFilled);


        //-----------------------------------------------------------------
        // Overlay (and plot) functions
        //-----------------------------------------------------------------

        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPlotCreate(string Title);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPlotData(short plotid, short Axis, short valueType, ref Any values, short Count);		//TODO As Any
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpPlotRange(short plotid, short Axis, short valueType, short rangeType, ref Any values);	//TODO As Any
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPlotSet(short plotid, string commandString);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPlotShow(short plotid, short sMode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPlotUpdate(short plotid);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPlotDestroy(short plotid);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDraw(ref POINTAPI Points, short numpoints, short Attrib);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDrawText(string Text, ref POINTAPI pos, short Attrib);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDrawClear(short objid);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGetLine(string Message, ref POINTAPI LinePts, ref short numpoints, short maxpoints, short Attrib);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDrawClearDoc(short docid);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpDrawGet(short command, short objid, ref Any lpParam);	//TODO As Any
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpDrawSet(short command, short objid, ref Any lpParam);	//TODO As Any


        public const int DDE_OPEN = 1;
        public const int DDE_CLOSE = 2;
        public const int DDE_PUT = 3;
        public const int DDE_GET = 4;
        public const int DDE_EXEC = 5;
        public const int DDE_SET = 6;

        // for IpBlbSetFilterRange
        public const int CALIB_UNIT = 0x4000;

        // print functions
        public const int P_GRAPH = 1;
        public const int P_TABLE = 2;
        public const int P_IMAGE = 3;

        public const int TXT_BOLD = 1;
        public const int TXT_UNDERLINE = 2;
        public const int TXT_ITALIC = 3;
        public const int TXT_STRIKEOUT = 4;
        public const int TXT_DROPSHADOW = 5;
        public const int TXT_ENCLOSED = 6;
        public const int TXT_SPACING = 7;

        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTextShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTextBurn(string textStr, ref POINTAPI textPos);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTextSetAttr(short Attr, short Value);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTextFont(string fontName, short fontSize);

        //-----------------------------------------------------------------
        // Annotation Functions and Constants
        //-----------------------------------------------------------------
        // Graph Object Types
        public const int GO_OBJ_LINE = 1;
        public const int GO_OBJ_RECT = 2;
        public const int GO_OBJ_ROUNDRECT = 3;
        public const int GO_OBJ_ELLIPSE = 4;
        public const int GO_OBJ_TEXT = 5;
        public const int GO_OBJ_POLY = 6;

        // Graph Object Attributes
        public const int GO_ATTR_PENCOLOR = 1;
        public const int GO_ATTR_BRUSHCOLOR = 2;
        public const int GO_ATTR_TEXTCOLOR = 3;
        public const int GO_ATTR_PENWIDTH = 4;
        public const int GO_ATTR_PENSTYLE = 5;
        public const int GO_ATTR_RECTSTYLE = 6;
        public const int GO_ATTR_LINESTART = 7;
        public const int GO_ATTR_LINEEND = 8;
        public const int GO_ATTR_ZOOM = 9;
        public const int GO_ATTR_CONNECT = 10;
        public const int GO_ATTR_TEXTWORDWRAP = 11;
        public const int GO_ATTR_TEXTCENTERED = 12;
        public const int GO_ATTR_TEXTAUTOSIZE = 13;
        public const int GO_ATTR_USEASDEFAULT = 14;
        public const int GO_ATTR_FONTSIZE = 15;
        public const int GO_ATTR_FONTBOLD = 16;
        public const int GO_ATTR_FONTITALIC = 17;
        public const int GO_ATTR_FONTUNDERLINE = 18;
        public const int GO_ATTR_TEXTLENGTH = 19;
        public const int GO_ATTR_TEXT = 20;
        public const int GO_ATTR_NUMPOINTS = 21;
        public const int GO_ATTR_POINTS = 22;

        public const int GO_OBJ_NUMBER = 100;
        public const int GO_OBJ_INDEX = 101;
        public const int GO_SEL_NUMBER = 102;
        public const int GO_SEL_INDEX = 103;

        // Pen Styles, values from wingdi.h for PS_xxx
        public const int GO_PENSTYLE_SOLID = 0;
        public const int GO_PENSTYLE_DASH = 1;
        public const int GO_PENSTYLE_DOT = 2;
        public const int GO_PENSTYLE_DASHDOT = 3;
        public const int GO_PENSTYLE_DASHDOTDOT = 4;

        // Rect Styles, index in CmGraphOverlay
        public const int GO_RECTSTYLE_BORDER_NOFILL = 0;
        public const int GO_RECTSTYLE_BORDER_FILL = 1;
        public const int GO_RECTSTYLE_NOBORDER_FILL = 2;

        // Line Endings, index in CmGraphOverlay
        public const int GO_LINEEND_NOTHING = 0;
        public const int GO_LINEEND_SMALLARROW = 1;
        public const int GO_LINEEND_LARGEARROW = 2;
        public const int GO_LINEEND_SMALLDIAMOND = 3;
        public const int GO_LINEEND_LARGEDIAMOND = 4;
        public const int GO_LINEEND_CIRCLE = 5;
        public const int GO_LINEEND_SMALLTICKMARK = 6;
        public const int GO_LINEEND_LARGETICKMARK = 7;

        // Annotation Functions - recorded
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnCreateObj(short nObjType);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnDeleteObj();
        
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnMove(short nHandle, short X, short Y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnMove(short nHandle, int X, int Y);

        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnSet(short nAttr, int nValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnGet(short nAttr, ref Any lpValue);								//TODO As Any
        [DllImport("IPC32", EntryPoint = "IpAnGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnGetStr(short nAttr, string lpValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnText(string szText);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnAddText(string szText);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnSetFontName(string szFontName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnGetFontName(string szFontName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnPolyAddPtArray(ref POINTAPI Points, short nCount);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnBurn();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnShowAnnot(short bShow);


        // Annotation Functions - not recorded
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnActivateObjID(int nObjID);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnActivateObjXY(short X, short Y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnActivateDefaultObj(short nObjType);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnPolyAddPtString(string szPoints);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnDeleteAll();


        //-----------------------------------------------------------------
        // Workflow toolbar Functions and Constants
        //-----------------------------------------------------------------
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpToolbarShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpToolbarSelect(string szToolbar);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpToolbarGetStr(short nAttr, string CurrValue);


        //
        // Alignment and Tiling functions and constants
        //
        // Copyright (c) 2003 Media Cybernetics, Inc.
        //



        // if this include file is included by task.h, only one of these set of macros should
        // be defined.
        // Commands

        // Integer arguments (Get/Set). sParam is ignored unless specified.
        public const int ALGN_ALGORITHM = 100;
        public const int ALGN_ANGLE_NUM = 110;
        public const int ALGN_SCALE_NUM = 120;
        public const int ALGN_OPTIONS = 130;
        public const int ALGN_CAL_ORDER = 140;
        public const int ALGN_REF_FRAME = 150;
        public const int ALGN_ALG_OPTION = 160;
        public const int ALGN_GETNUMFRAMES = 170;
        public const int ALGN_GETFRAMELST = 180;
        public const int ALGN_TRIMBORDERS = 190;
        // GETNUMDOC (get) number of images in list
        // GETDOCLST (get) list of doc ID's, max count sParam
        public const int ALGN_UPDATEUI = 200;
        public const int ALGN_ITERATE = 210;

        public const int ALGN_HAVECALIB = 220;
        public const int ALGN_HAVEPOSITION = 230;
        public const int ALGN_CALCSTATE = 240;

        // Floating point arguments (Get/Set)
        public const int ALGN_X_PERIMAGE = 1010;
        public const int ALGN_Y_PERIMAGE = 1020;
        public const int ALGN_X_CAL_ANGLE = 1030;
        public const int ALGN_Y_CAL_ANGLE = 1040;

        // Return value is the number sent back.
        // These are valid only after IpAlignCalculate( ) is called or
        // these values are set by a macro call.

        // Note that these array fetch routines require:
        // Second parameter is the index (see ALGN_GETNUMFRAMES)

        // Get only, for each frame, expressing how it is manipulated compared to the
        // previous frame
        public const int ALGN_OFFSET_COUNT = 2000;
        public const int ALGN_ANGLE_COUNT = 2010;
        public const int ALGN_SCALE_COUNT = 2020;

        // Second parameter is the index (see ALGN_GETNUMFRAMES)
        public const int ALGN_OFFSET_VAL = 2030;
        public const int ALGN_ANGLE_VAL = 2040;
        public const int ALGN_SCALE_VAL = 2050;

        // Second parameter is the INDEX (see ALGN_GETNUMFRAMES)
        public const int ALGN_OFFSET_RANK = 2060;
        public const int ALGN_ANGLE_RANK = 2070;
        public const int ALGN_SCALE_RANK = 2080;

        // List of the best alignment values. Second parameter is the index of the
        // frames, 0 to n-2. DOCSEL_ALL gets/sets the entire list of ALGN_GETNUMFRAMES
        // values.
        public const int ALGN_BEST_OFFSET = 2090;
        public const int ALGN_BEST_ANGLE = 2100;
        public const int ALGN_BEST_SCALE = 2110;

        // ALGN_OPTIONS arguments
        public const int ALGN_ROTATE = 2200;
        public const int ALGN_SCALE = 2210;
        public const int ALGN_TRANSLATE = 2220;

        // ALGN_ALGORITHM arguments. Additional methods can be
        // added here, with ALGN_ALG_OPTION arguments for algorithm
        // specific settings.
        public const int ALGN_FFT = 1;
        public const int ALGN_USER = 2;

        // ALGN_ALG_OPTION constants for ALGN_FFT, specific to that algorithm
        public const int ALGN_METHOD = 1;
        public const int ALGN_FFT_NANGLES = 2;
        public const int ALGN_FFT_NSCALES = 3;
        public const int ALGN_FFT_APODIZE = 4;

        // ALGN_ALG_OPTION, ALGN_METHOD constants for ALGN_FFT, specific to that algorithm
        public const int ALGN_FFTFULL = 1;
        public const int ALGN_FFTPHASE = 2;

        // ALGN_ALG_OPTION calls for ALGN_USER, specific to that algorithm
        public const int ALGN_USER_X = 1;
        public const int ALGN_USER_Y = 2;
        public const int ALGN_USER_XANGLE = 3;
        public const int ALGN_USER_YANGLE = 4;
        public const int ALGN_USER_XDIST = 5;
        public const int ALGN_USER_YDIST = 6;
        public const int ALGN_USER_ZDIST = 7;

        // IpAlignShow arguments
        public const int ALGN_IMAGETAB = 1;
        public const int ALGN_OPTIONTAB = 2;
        public const int ALGN_ADJUST = 3;


		//-----------------------------------------------------------------
		// Align Functions
		//-----------------------------------------------------------------

		// Show/hide (1/0) the specified dialog
		[DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAlignShow(short nDialog, short bShow);
		// Generic Set routine
		[DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAlignSetEx(short sAttribute, short sParam, ref Any lpData);		//TODO As Any
		[DllImport("IPALGN32", EntryPoint = "IpAlignSetEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAlignSetStr(short sCmd, short sParam, string lpParam);
		// Set an integer value; variable pointers are not required
		[DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAlignSetInt(short sAttribute, short sParam, short sData);
		// Set a float value; variable pointers are not required
		[DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAlignSetSingle(short sAttribute, short sParam, float fData);
		// Get function for module data
		[DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAlignGet(short sAttribute, short sParam, ref Any lpData);		//TODO As Any
		[DllImport("IPALGN32", EntryPoint = "IpAlignGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAlignGetStr(short sCmd, short sParam, string lpParam);
		// Add a frame from a workspace to the selected set; returns IPCERR_INVCOMMAND if
		// the image does not meet the current size and class requirements
		[DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAlignAdd(short docid, short frame);
		// Remove a frame from the selected set
		[DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAlignRemove(short docid, short frame);
		// Calculate offsets for the images
		[DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAlignCalculate();
		// Apply the offsets, creating a new workspace and returning the document ID
		[DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAlignApply();
		// Save the current offset values
		[DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAlignSave(string FileName);
		// Load offset values. Fails if the number of offsets do not match the
		// current number of selected frames/images
		[DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpAlignOpen(string FileName);


        // Underlying affine transform function for shifting images. This
        // rotates, scales and then transforms the active workspace.
        //Declare Function IpAffine Lib "IPALGN32" (ByVal rotate!, ByVal scale!, ByVal dX%, ByVal dY%) As Long           
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]                 
        public static extern int IpAffine(float rotate, float scale1, short dX, short dY);			//v9.5 変更 by 間々田 2004/09/14 scale は使用不可

        //-----------------------------------------------------------------
        // Tiling constants
        //-----------------------------------------------------------------

        // Integer arguments (Get/Set). sParam is ignored unless specified.
        public const int TILE_ALGORITHM = 500;
        public const int TILE_CAL_ORDER = 510;
        public const int TILE_ALG_OPTION = 520;
        public const int TILE_GETNUMFRAMES = 530;
        public const int TILE_GETFRAMELST = 540;
        // GETNUMDOC (get) number of images in list
        // GETDOCLST (get) list of doc ID's, max count sParam
        public const int TILE_UPDATEUI = 550;
        public const int TILE_HAVECALIB = 560;
        public const int TILE_HAVEPOSITION = 570;
        public const int TILE_CALCSTATE = 580;
        public const int TILE_X_REVERSE = 590;
        public const int TILE_Y_REVERSE = 600;
        public const int TILE_ZIGZAG_ORDER = 610;
        public const int TILE_VERTICAL_ORDER = 620;
        public const int TILE_FULL_LIST = 630;
        public const int TILE_ISPREVIEWING = 640;
        public const int TILE_BLEND = 650;
        public const int TILE_SETFROMPOS = 660;
        public const int TILE_MOVEUP = 670;
        public const int TILE_MOVEDOWN = 680;

        // Note that these array fetch routines require:
        // Second parameter is the index (see TILE_GETNUMFRAMES)

        // Get only, for each frame, expressing how it is manipulated compared to the
        // previous frame
        public const int TILE_OFFSET_COUNT = 2000;

        // Second parameter is the index (see TILE_GETNUMFRAMES)
        public const int TILE_OFFSET_VAL = 2030;

        // Second parameter is the INDEX (see TILE_GETNUMFRAMES)
        public const int TILE_OFFSET_RANK = 2060;

        // TILE_ALGORITHM arguments. Additional methods can be
        // added here, with TILE_ALG_OPTION arguments for algorithm
        // specific settings.
        public const int TILE_FFT = 1;
        public const int TILE_USER = 2;

        // TILE_ALG_OPTION constants for TILE_FFT, specific to that algorithm
        public const int TILE_METHOD = 1;

        // TILE_ALG_OPTION, TILE_METHOD constants for TILE_FFT, specific to that algorithm
        public const int TILE_FFTFULL = 1;
        public const int TILE_FFTPHASE = 2;

        public const int DOCSEL_BLANK = -100;

        // List argument (get)
        public const int TILE_INDEXES = 3000;
        public const int TILE_XY_LAYOUT = 3010;
        // this index shows how the inputs are ordered under
        // current params
        public const int TILE_POSITIONS = 3020;
        // sParam indicates which tile, while DOCSEL_ALL requests the
        // complete list. lpData must be an appropriately sized
        // list of POINT structures.

        // Floating point arguments
        public const int TILE_X_PERC_OVERLAP = 3500;
        public const int TILE_Y_PERC_OVERLAP = 3510;
        public const int TILE_X_PIXEL_OVERLAP = 3520;
        public const int TILE_Y_PIXEL_OVERLAP = 3530;
        public const int TILE_X_CAL_OVERLAP = 3540;
        public const int TILE_Y_CAL_OVERLAP = 3550;

        // TILE_BLEND arguments
        public const int TILE_BLEND_OVERLAY = 1;
        public const int TILE_BLEND_GRADIENT = 2;
        public const int TILE_BLEND_CLIP = 3;

        // IpTileShow arguments
        public const int TILE_IMAGETAB = 1;
        public const int TILE_OPTIONTAB = 2;
        public const int TILE_LAYOUT = 3;
        public const int TILE_ADJUST = 4;

        // TILE_ALG_OPTION calls for TILE_USER, specific to that algorithm
        public const int TILE_USER_X_HORIZ = 1;
        public const int TILE_USER_Y_HORIZ = 2;
        public const int TILE_USER_X_VERT = 3;
        public const int TILE_USER_Y_VERT = 4;

        public const int TILE_USER_X_CALHORIZ = 5;
        public const int TILE_USER_Y_CALHORIZ = 6;
        public const int TILE_USER_X_CALVERT = 7;
        public const int TILE_USER_Y_CALVERT = 8;


        //-----------------------------------------------------------------
        // Tiling Functions
        //-----------------------------------------------------------------

        // Show/hide (1/0) the specified dialog
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTileShow(short nDialog, short bShow);
        // Generic Set routine
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTileSetEx(short sAttribute, short sParam, ref Any lpData);		//TODO As Any
        // Set an integer value; variable pointers are not required
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTileSetInt(short sAttribute, short sParam, short sData);
        // Set a float value; variable pointers are not required
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTileSetSingle(short sAttribute, short sParam, float fData);
        [DllImport("IpTile32", EntryPoint = "IpTileSetEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTileSetStr(short sCmd, short sParam, string lpParam);
        // Get function for module data
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTileGet(short sAttribute, short sParam, ref Any lpData);			//TODO As Any
        [DllImport("IpTile32", EntryPoint = "IpTileGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTileGetStr(short sCmd, short sParam, string lpParam);
        // Add a frame from a workspace to the selected set; returns IPCERR_INVCOMMAND if
        // the image does not meet the current size and class requirements
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTileAdd(short docid, short frame);
        // Remove a frame from the selected set
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTileRemove(short docid, short frame);
        // Calculate offsets for the images
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTileCalculate();
        // Apply the offsets, creating a new workspace and returning the document ID
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTileApply();
        // Save the current offset values
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTileSave(string FileName);
        // Load offset values. Fails if the number of offsets do not match the
        // current number of selected frames/images, or if tile layouts are different.
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTileOpen(string FileName);


        //
        // Definitions for Caliper Auto-Pro Functions
        //
        // Copyright (c) 1998 - , Media Cybernetics, Inc.
        //



        // Clpr Sampler types
        public const int CLPR_LINE = 0;
        public const int CLPR_CWCIRCLE = 1;
        public const int CLPR_CCWCIRCLE = 2;
        public const int CLPR_POLYLINE = 3;

        // Clpr Edge Detector types
        public const int CLPR_DERIVATIVE = 0;
        public const int CLPR_PATTERN_MATCH = 1;

        // Clpr Attribute types
        public const int CLPR_AUTOREFRESH = 200;

        public const int CLPR_CIRCLE_ORIGIN = 201;

        public const int CLPRE_NAME = 301;
        public const int CLPRE_LABEL = 302;
        public const int CLPRE_COLOR = 303;
        public const int CLPRE_OFFSET = 304;
        public const int CLPRE_STYLE = 305;
        public const int CLPRE_THRESHOLD = 306;
        public const int CLPRE_SENS = 307;

        public const int CLPRO_SMOOTHING = 310;
        public const int CLPRO_THICKNESS = 311;
        public const int CLPRO_APPLY_ICAL = 312;
        public const int CLPRO_APPLY_SCAL = 313;
        public const int CLPRO_AUTO_SCALE = 314;
        public const int CLPRO_SHOW_LABEL = 315;
        public const int CLPRO_SHOW_NUMBER = 316;
        public const int CLPRO_PRECISION = 317;

        // Clpr Edge Detector Attribute values
        public const int CLPR_PEAK = 0;
        public const int CLPR_VALLEY = 1;
        public const int CLPR_RISING = 2;
        public const int CLPR_FALLING = 3;

        // Measurement types
        public const int CLPR_MEAS_POSX = 0;
        public const int CLPR_MEAS_POSY = 1;
        public const int CLPR_MEAS_DIST = 2;
        public const int CLPR_MEAS_DIST1 = 3;
        public const int CLPR_MEAS_DIST2 = 4;

        // Clipboard commands
        public const int CLPR_CUT = 0;
        public const int CLPR_COPY = 1;
        public const int CLPR_PASTE = 2;

        // IpClprGetData commands
        public const int CLPD_STAT = 0x800;
        public const int CLPD_GETROWCOUNT = 1;
        public const int CLPD_GETCOLCOUNT = 2;
        public const int CLPD_GETCELL = 3;

        public const int CLPR_MAX_PATTERN_SIZE = 50;
        public static float[] ipPattern = new float[CLPR_MAX_PATTERN_SIZE + 1];


        // Clpr Functions
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprShow(short nShow);


        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprCreateSampler(short nType, string szName, ref POINTAPI pt, short nNumPoints);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprEditSampler(short nHandle, short X, short Y);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprSelectSampler(short nID);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprDeleteSampler();
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprClipboard(short nCommand);

        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprCreateDerivativeEdge(string szName, string szLabel, int lColor, short nOffset, short nStyle);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprCreatePatternMatchEdge(string szName, string szLabel, int lColor, short nOffset, short nThreshold, ref float ptPattern, short nNumPoints);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprSelectEdge(string szName);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprDeleteEdge();

        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprCreateMeas(short nType, string szFromName, string szToName);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprDeleteMeas(short nType, string szFromName, string szToName);

        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprToggleMarker(short X, short Y);

        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprSet(short sAttribute, float fData);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprGet(short sAttribute, ref float lpfData);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprSetStr(short sAttribute, string lpStr);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprGetStr(short sAttribute, string lpStr);

        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprSettings(string szFileName, short bSave);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprSave(string szFileName, short nSaveMode);


        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprGetData(short command, short nParam1, short nParam2, string szRetVal);
        //
        // Color Composite functions and constants
        //
        // Copyright (c) 1998 - 2001 , Media Cybernetics, Inc.
        //





        public const int SHIFT_X = 7;
        public const int SHIFT_Y = 8;
        public const int COMP_HUE = 9;
        public const int COMP_TINT = 10;

        public const int COMP_BACKGROUND = 10;

        public const int COMP_BESTFIT = 11;
        public const int COMP_RESET = 12;

        public const int COMP_UPDATE = 13;

        public const int COMP_FRAME = 14;
        public const int COMP_NUMFRAMES = 15;

        public const int COMP_MAKESEQUENCE = 16;

        public const int COMP_DISPLAY = 17;

        public const int COMP_SHOW = 1;
        public const int COMP_HIDE = 0;

        public const int HUE_INTERACTIVE = -3;
        public const int HUE_DEFAULT = -2;
        public const int HUE_QUERY = -1;
        public const int HUE_RED = 0;
        public const int HUE_GREEN = 120;
        public const int HUE_BLUE = 240;
        public const int HUE_YELLOW = 60;
        public const int HUE_CYAN = 180;
        public const int HUE_MAGENTA = 300;
        public const int HUE_WHITE = 361;


        // All settings for the color composite
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCmpSet(short command, short Param, int Value);

        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]       
        public static extern int IpCmpGet(short command, short Param, ref Any lpParam);		//TODO As Any

        // Start a new composite. Composite size will be set to the size of the doc.
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCmpNew(short docid, short hue);

        // Add a document to the composite.
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCmpAdd(short docid, short hue);

        // Add a document to the composite with x/y shift
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCmpAddEx(short docid, short hue, short dX, short dY);

        // Delete a document from the composite.
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCmpDel(short docid);

        // Show or hide the color composite dialog
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCmpShow(short flag);

        // Color composite channels can also be defined with a specific RGB tint
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCmpNewTint(short docid, int Tint);
        // Add a document to the composite with a specific tint.
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCmpAddTint(short docid, int Tint);
        // Same with x/y shift
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCmpAddTintPos(short docid, int Tint, short dX, short dY);

        //
        // Definitions for L*a*b color/Color correction module
        //
        // Copyright (c) 1998 - , Media Cybernetics, Inc.
        //




        //L*a*b color/Color correction module
        // Color modes
        public const int COLM_LAB = 0;
        public const int COLM_XYZ = 1;
        public const int COLM_RGB = 2;
        public const int COLM_YIQ = 3;
        public const int COLM_CMY = 4;

        public const int COLM_CH1 = 1;
        public const int COLM_CH2 = 2;
        public const int COLM_CH3 = 4;

        public const int SET_CAL_POINT = 0;
        public const int SET_CAL_INFO = 1;
        public const int SET_CAL_MATRIX = 2;

        public const int GET_CAL_POINT = 0;
        public const int GET_CAL_INFO = 1;
        public const int GET_CAL_MATRIX = 2;


        public const int COLM_MAXPOINTS = 20;


        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColExtract(int MASK, short ColMode, short IsFloat);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColShow(short Show);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColCalNew(short InpMode, short ColModel);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColCalGetRGB(short X, short Y, short SIZE, ref Any lpParam);		//TODO As Any
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColCalAdd(ref float fRGB, ref float fLAB);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColCalCreate();
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColCalShow(short Show);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColCalSave(string Name);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColCalLoad(string Name);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColCalCorrect(string InName, string OutName);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGetConvertColor(ref float rgb, ref float Out, int ColMode, int Class, int Norm);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColConvert(short ColMod);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColCalGet(short command, short N, ref float Out);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColCalSet(short command, short N, ref float Out);

        //
        // Co-localization functions and constants
        //
        // Copyright (c) 1999 - 2001, Media Cybernetics, Inc.
        //



        public const int CP_RED_GREEN = 0;
        public const int CP_BLUE_RED = 1;
        public const int CP_GREEN_BLUE = 2;
        public const int CP_GREEN_RED = 3;
        public const int CP_RED_BLUE = 4;
        public const int CP_BLUE_GREEN = 5;

        public const int CLOC_FWDMASK = 0;
        public const int CLOC_FWDCOLOR = 1;
        public const int CLOC_FWD3D = 2;
        public const int CLOC_FWDPARAMS = 3;

        public const int CLOC_INVMASK = 0;
        public const int CLOC_INVPARAMS = 1;

        public const int CLDOC_COLORCOMPOSITE = 0;
        public const int CLDOC_SCATTERPLOT = 1;
        public const int CLDOC_3DMASK = 2;


        //-----------------------------------------------------------------
        // Co-Localization Functions
        //-----------------------------------------------------------------

        //Shows/hides co-localization dialog
        [DllImport("IpCoLoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCoLocShow(short bShow);

        //Calculates co-localization parameters of active color or active gray and second gray image
        //
        // SecondImageId - image ID of second gray image if the active image is gray
        // if active image is color this parameter is ignored (should be -1)
        //ColorPair - color pair, must be one of the following:
        // CP_RED_GREEN, CP_BLUE_RED, CP_GREEN_BLUE,
        // CP_GREEN_RED, CP_RED_BLUE, CP_BLUE_GREEN
        // where first color is channel number 1 (active image in
        // case of gray images) - horizontal axis of co-localization plot,
        // second color is channel number 2 (2nd image in case of gray images)
        // - vertical axis of co-localization plot
        //FType - type of the function:
        // CLOC_FWDMASK - creates gray co-localization, 16-bit image of co-localization plot
        // where brightness (gray level) represents the frequency (number of occurrences)
        // of color pair on the original image, sequence or AOI
        // CLOC_FWDCOLOR - creates color co-localization, RGB image of co-localization plot
        // that have non-zero pixels where this coloc pair present on the original image
        // CLOC_FWD3D - opens 3D view on co-localization plot, to create a 3D image of co-localization
        // plot user has to click "New image" button in Output tab of Surface plot dialog
        // CLOC_FWDPARAMS - calculates first set of co-localization parameters
        [DllImport("IpCoLoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCoLocForward(short SecondImageId, short ColorPair, short iType);

        //Calculates co-localization parameters the image on the base of AOI on co-localization plot
        //
        //IType - Type of operation
        // CLOC_INVMASK - creates a mask of co-localizing pixels on the base of an AOI on
        // the image of co-localization plot
        // CLOC_INVPARAMS - calculates second set of co-localization parameters
        [DllImport("IpCoLoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCoLocInverse(short iType);

        //Gets co-localization overlap parameters of original image
        // where Stats is array of 10 single
        //Dim Stats[10] as single
        // return values
        // Stats[0] - Pearson's correlation Rr
        // Stats[1] - Overlap coefficient R
        // Stats[2] - Overlap coefficient k1
        // Stats[3] - Overlap coefficient k2
        // Stats[4..9] - reserved
        [DllImport("IpCoLoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCoLocGetForward(short SecondImageId, short ColorPair, ref float CLData);



        //Gets co-localization parameters on the base of AOI on
        //the active of co-localization plot
        // where Stats is array of 10 single
        //Dim Stats[10] as single
        // return values
        // Stats[0] - Co-localization coefficient M1
        // Stats[1] - Co-localization coefficient M2
        // Stats[2..9] - reserved
        [DllImport("IpCoLoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCoLocGetInverse(ref float CLData);


        // Original Solutions-Zone function names
        [DllImport("IpCoLoc32", EntryPoint = "IpCoLocShow", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColcShow(short bShow);
        [DllImport("IpCoLoc32", EntryPoint = "IpCoLocForward", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColcForw(short SecondImageId, short ColorPair, short iType);
        [DllImport("IpCoLoc32", EntryPoint = "IpCoLocInverse", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColcInv(short iType);
        [DllImport("IpCoLoc32", EntryPoint = "IpCoLocGetForward", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColcGetForw(short SecondImageId, short ColorPair, ref float CLData);
        [DllImport("IpCoLoc32", EntryPoint = "IpCoLocGetInverse", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColcGetInv(ref float CLData);


        //Gets document IDs for images created by co-localization (IpCoLocForward)
        [DllImport("IpCoLoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCoLocGetDocument(short DocType, ref short docid);

        //
        // Definitions for Data Collector Auto-Pro Functions
        //
        // Copyright (c) 1998 - 2001, Media Cybernetics, Inc.
        //



        public const int DC_FETCH = 0;
        public const int DC_RESET = 1;
        public const int DC_RESETLAST = 2;

        public const int DC_AUTO = 10;
        public const int DC_AUTOMODE = 11;
        public const int DC_BREAK = 12;
        public const int DC_TOPLINE = 13;
        public const int DC_LEFTCOL = 14;
        public const int DC_COLWIDTH = 15;
        public const int DC_SIGNIF = 16;

        public const int DC_COL = 50;
        public const int DC_ROW = 51;

        public const int DC_NUMROW = 100;
        public const int DC_NUMCOL = 101;
        public const int DC_NUMBLOCK = 102;
        public const int DC_NUMVAL = 103;
        public const int DC_TYPE = 104;
        public const int DC_STATS = 105;
        public const int DC_BLOCKROW1 = 106;
        public const int DC_DATA = 107;

        public const int DC_CELL = 200;

        public const int DC_MAXSTRINGLEN = 128;

        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcShow(short bShow);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcSet(short sAttribute, int lData);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcSelect(string szModuleName, string szItemName, short sParam);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcUnSelect(string szModuleName, string szItemName, short sParam);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcUpdate(short bShow);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcSaveData(string szFileName, short sFlags);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcGet(short sCmd, short sParam, ref Any lpParam);					//TODO As Any
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcGetStr(short sCmd, short sParam, string szString);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcAddCol(string szColumnName);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcDeleteCol(int lColID);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcAddSng(int lColID, short sNewBlock, short sNumRows, ref float lpfData);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcAddStr(int lColID, short sNewBlock, short sRow, string szDataStr);
        //
        // Image and File Signature functions
        //
        // Copyright (c) 2001 - , Media Cybernetics, Inc.
        //




        // Image Signature Commands
        public const int IS_SIGNATURE = 1;
        public const int IS_SIGNATURE_STR = 2;
        public const int IS_COMPARE = 3;
        public const int IS_COMPARE_STR = 4;


        //-----------------------------------------------------------------
        // Image Signature Functions
        //-----------------------------------------------------------------
        [DllImport("IpDigSign32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpIsShow(short bShow);
        [DllImport("IpDigSign32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpIsGet(short sAttribute, ref Any lpData);		//TODO As Any
        [DllImport("IpDigSign32", EntryPoint = "IpIsGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpIsGetStr(short sAttribute, string lpString);


        // Original Solutions-Zone functions
        [DllImport("IpDigSign32", EntryPoint = "IpIsShow", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDsShow(short bShow);
        [DllImport("IpDigSign32", EntryPoint = "IpIsGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDsGet(short sAttribute, ref Any Data);		//TODO As Any
        [DllImport("IpDigSign32", EntryPoint = "IpIsGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDsGetStr(short sAttribute, string lpString);



        // File Signature Commands
        public const int FS_SIGNATURE = 1;
        public const int FS_SIGNATURE_STR = 2;
        public const int FS_COMPARE = 3;
        public const int FS_COMPARE_STR = 4;


        //-----------------------------------------------------------------
        // Image Signature Functions
        //-----------------------------------------------------------------
        [DllImport("IpDigSign32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFsGet(string lpFile, short sAttribute, ref Any lpData);		//TODO As Any
        [DllImport("IpDigSign32", EntryPoint = "IpFsGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFsGetStr(string lpFile, short sAttribute, string lpString);


        //
        // Dye management functions
        //
        // Copyright (c) 1999-2003 - , Media Cybernetics, Inc.
        //




        // commands for IpDyeGet
        public const int DYE_WAVELENGTH = 1;
        public const int DYE_RGB_TINT = 2;
        public const int DYE_NUMDYES = 3;
        public const int DYE_EXWAVELENGTH = 4;

        // commands for IpDyeGetStr
        public const int DYE_PATH = 10;
        public const int DYE_LIST = 11;


        //-----------------------------------------------------------------
        // Dye Functions
        //-----------------------------------------------------------------
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDyeSelect(string Dye);
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDyeAdd(string Dye, int WL, int ExWL);
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDyeGet(string Dye, short command, ref int Value);
        // Use for DYE_PATH and DYE_LIST
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDyeGetStr(string Dye, short command, short Index, string Value);
        // Use for DYE_PATH
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDyeSetStr(string Dye, short command, string Value);
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDyeAddTint(string Dye, int WL, int ExWL, int Tint);
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDyeApply(string Dye, short ApplyTo, short ApplyTint);
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDyeEdit(string Dye, string newDye);

        //
        // Definitions for Extended Depth of Field (EDF) Auto-Pro Functions and constants
        //
        // Copyright (c) 2001 - , Media Cybernetics, Inc.
        //



        // Multi-Plane Focus output types
        public const int EDF_COMPOSITE = 1;
        public const int EDF_BEST_FOCUS = 2;
        public const int EDF_ANALYZE_ONLY = 0x10;

        // Get/Set Commands
        public const int EDF_NORMALIZE = 1;
        public const int EDF_CRITERIA = 2;
        public const int EDF_TOPO_MAP = 3;
        public const int EDF_TOPO_CALIBRATED = 4;
        public const int EDF_ORDER = 5;
        public const int EDF_DEFAULT_FRAME = 6;
        public const int EDF_BEST_PLANE = 7;
        public const int EDF_NUM_PLANES = 8;
        public const int EDF_TS_MAP = 9;
        public const int EDF_TS_GALLERY = 10;
        public const int EDF_TOPO_SURFACE_PLOT = 11;

        // Analysis criteria
        public const int EDF_MAX_LOCALCONTRAST = 0;
        public const int EDF_MAX_INTENSITY = 1;
        public const int EDF_MIN_INTENSITY = 2;
        public const int EDF_MAX_DEPTHCONTRAST = 3;

        // image order
        public const int EDF_TOPDOWN = 0;
        public const int EDF_BOTTOMUP = 1;


        //-----------------------------------------------------------------
        // Multi-Plane Focus Functions
        //-----------------------------------------------------------------
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpEDFShow(short Show);
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpEDFNew(short docid);
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpEDFAdd(short docid);
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpEDFRemove(short docid);
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpEDFCreate(short iType);
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpEDFTopoMap();
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpEDFGet(short sAttribute, ref short CurrValue, ref int CurrFrame);
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpEDFSet(short sAttribute, short newValue, int NewFrame);
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpEDFGetConf(ref float ConfidenceArray);
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpEDFTestStrips();


        //***********************************************************************
        // FTP functions
        //***********************************************************************


        public const int FTP_DUMMY = 0;

        [DllImport("IPFTP32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFTPOpen(string server, string remotefile);
        [DllImport("IPFTP32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFTPSave(string server, string remotefile);

        //-----------------------------------------------------------------
        // Image Database/Gallery functions
        //-----------------------------------------------------------------




        // IpDbSearch

        public const int OP_EQUAL = 0;
        public const int OP_LT = 1;
        public const int OP_LE = 2;
        public const int OP_GT = 3;
        public const int OP_GE = 4;
        public const int OP_LIKE = 5;
        public const int OP_NOTLIKE = 6;

        public const int DB_INT = 0;
        public const int DB_LONG = 1;
        public const int DB_STRING = 2;
        public const int DB_BINARY = 3;
        public const int DB_MEMO = 4;
        public const int DB_FILE = 5;

        public const int DB_CAPTION = 21;
        public const int DB_COPYCUSTOM = 22;

        // IpDbGoto
        public const int DB_FIRST = -1;
        public const int DB_LAST = -2;
        public const int DB_NEXT = -3;
        public const int DB_PREV = -4;



        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbWrite(string szFieldName, short FieldType, ref Any szFieldValue, int DataLength);		//TODO As Any
        // Use for DB_STRING writes
        [DllImport("IPGALI32", EntryPoint = "IpDbWrite", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbWriteStr(string szFieldName, short FieldType, string FieldValue, int DataLength);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpDbRead(string szFieldName, short FieldType, ref Any szFieldValue, int DataLength);		//TODO As Any
        // Use for DB_STRING reads
		[DllImport("IPGALI32", EntryPoint = "IpDbRead", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbReadStr(string szFieldName, short FieldType, string FieldValue, int DataLength);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbGoto(short RecordNum);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpDbSearch(string szFieldName, short FieldType, short Operator, ref Any FieldValue);		//TODO As Any
        // Use for DB_STRING searches
        [DllImport("IPGALI32", EntryPoint = "IpDbSearch", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbSearchStr(string szFieldName, short FieldType, short Operator, string FieldValue);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpDbFind(string szFieldName, short FieldType, short Operator, ref Any FieldValue);			//TODO As Any
        // Use for DB_STRING searches
        [DllImport("IPGALI32", EntryPoint = "IpDbFind", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbFindStr(string szFieldName, short FieldType, short Operator, string FieldValue);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbViewFolder(string szFolderName);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbViewAll();
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbLoadView(string szViewName);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbNewFolder(string szFolderName, string szDescription);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbOpenFolder(string szFolderName);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbPrint(short sLayout);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbAddField(string szFieldName, short FieldType, short FieldLength);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbSetAttr(short Attrib, short nValue, string strValue);

        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbWriteNum(string szFieldName, short FieldType, ref Any szFieldValue, int DataLength);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbReadNum(string szFieldName, short FieldType, ref Any szFieldValue, int DataLength);



        //
        // Definitions for Grid Auto-Pro Functions
        //
        // Copyright (c) 1998 - 2001, Media Cybernetics, Inc.
        //



        //-----------------------------------------------------------------
        // Grid mask functions
        //-----------------------------------------------------------------

        public const int GRID_CALIBFLAG_PIXEL = 1;
        public const int GRID_CALIBFLAG_IMAGE = 2;

        public const int GRID_ATTR_OBJECT = 1;
        public const int GRID_ATTR_LAYOUT = 2;
        public const int GRID_ATTR_DISPLAYAS = 3;
        public const int GRID_ATTR_HSPACE = 4;
        public const int GRID_ATTR_VSPACE = 5;
        public const int GRID_ATTR_RSPACE = 6;
        public const int GRID_ATTR_CHECKERED = 7;
        public const int GRID_ATTR_FLAGRANDSEED = 8;
        public const int GRID_ATTR_VALRANDSEED = 9;
        public const int GRID_ATTR_COUNT = 10;
        public const int GRID_ATTR_LENGTH = 11;
        public const int GRID_ATTR_HLENGTH = 12;
        public const int GRID_ATTR_VLENGTH = 13;
        public const int GRID_ATTR_LMARGIN = 14;
        public const int GRID_ATTR_TMARGIN = 15;
        public const int GRID_ATTR_RMARGIN = 16;
        public const int GRID_ATTR_BMARGIN = 17;
        public const int GRID_ATTR_FULLSIZE = 18;
        public const int GRID_ATTR_COLOR = 19;
        public const int GRID_ATTR_PENWIDTH = 20;

        public const int GRID_OBJECT_POINT = 1;
        public const int GRID_OBJECT_LINE = 2;
        public const int GRID_OBJECT_LINESGM = 3;
        public const int GRID_OBJECT_CIRCLE = 4;
        public const int GRID_OBJECT_CYCLOID = 5;

        public const int GRID_LAYOUT_ORTHOGONAL = 1;
        public const int GRID_LAYOUT_CONCENTRIC = 2;
        public const int GRID_LAYOUT_RANDOM = 3;

        public const int GRID_POINT_CIRCLE_LRG = 1;
        public const int GRID_POINT_CIRCLE_SML = 2;
        public const int GRID_POINT_CROSS_LRG90 = 3;
        public const int GRID_POINT_CROSS_LRG45 = 4;
        public const int GRID_POINT_CROSS_SML90 = 5;
        public const int GRID_POINT_CROSS_SML45 = 6;
        public const int GRID_POINT_MED = 7;
        public const int GRID_POINT_RECT_LRG = 8;
        public const int GRID_POINT_RECT_SML = 9;
        public const int GRID_POINT_DIAMOND_LRG = 10;
        public const int GRID_POINT_DIAMOND_SML = 11;
        public const int GRID_POINT_STAR8 = 12;
        public const int GRID_POINT_THREEUP = 13;
        public const int GRID_POINT_THREEDOWN = 14;

        [DllImport("IPGRID32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGridShow(short nValue);
        [DllImport("IPGRID32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGridSelect(string szGridFile);
        [DllImport("IPGRID32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGridApply(short nValue);
        [DllImport("IPGRID32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGridCreateMask();

        //
        // Lens management functions
        //
        // Copyright (c) 1999-2003 - , Media Cybernetics, Inc.
        //




        // commands for IpLensGetSng
        public const int LENS_MAGNIFICATION = 1;
        public const int LENS_NA = 2;
        public const int LENS_RI = 3;

        // commands for IpLensGetLong
        public const int LENS_NUMLENSES = 10;

        // commands for IpLensGetStr
        public const int LENS_PATH = 20;
        public const int LENS_LIST = 21;

        //-----------------------------------------------------------------
        //  Dye Functions
        //-----------------------------------------------------------------
        [DllImport("IPLENS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLensSelect(string Lens);
        [DllImport("IPLENS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLensAdd(string Lens, float Magnification, float NA, float RI);
        // Use for LENS_NUMLENSES
        [DllImport("IPLENS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLensGetLong(short command, ref int lpParam);
        // Use for LENS_MAGNIFICATION, LENS_NA and LENS_RI
        [DllImport("IPLENS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLensGetSng(string Lens, short command, ref float lpParam);
        // Use for LENS_PATH and LENS_LIST
        [DllImport("IPLENS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLensGetStr(short command, short Index, string Value);
        // Use for LENS_PATH
        [DllImport("IPLENS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLensSetStr(short command, string Value);
        // Use to apply lens characteristics to the active image
        [DllImport("IPLENS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLensApply(string Lens, short ApplyTo);

        //
        // Local Zoom functions and constants
        //
        // Copyright (c) 1999-2003 Media Cybernetics, Inc.
        //




        // Commands for IpLocZoomGet/Set
        public const int IP_LZ_ZOOM = 100;
        public const int IP_LZ_CROSS = 101;
        public const int IP_LZ_ISSHOWN = 102;


        //-----------------------------------------------------------------
        // Local Zoom Functions
        //-----------------------------------------------------------------
        [DllImport("IPLOCZOOM32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLocZoomShow(short bShow);
        [DllImport("IPLOCZOOM32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLocZoomMove(short xPos, short yPos);
        [DllImport("IPLOCZOOM32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLocZoomSize(short xSize, short ySize);
        [DllImport("IPLOCZOOM32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLocZoomSetPos(short xPos, short yPos);
        [DllImport("IPLOCZOOM32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLocZoomSet(short sCommand, short sVal);
        [DllImport("IPLOCZOOM32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLocZoomGet(short sCommand, ref short sVal);


        //
        // Definitions for Large Spectral Filters Auto-Pro Functions
        //
        // Copyright (c) 1998 - 2001 , Media Cybernetics, Inc.
        //




        public const int ATTRIBUTE_1 = 10000;

        public const int LF_LOPASS = 0;
        public const int LF_HIPASS = 1;
        public const int LF_EDGEPL = 2;
        public const int LF_EDGEMN = 3;
        public const int LF_BANDPASS = 4;


        [DllImport("IpLrg32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLSFltShow(short bShow);
        [DllImport("IpLrg32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLSFltApply(short sType, short Width, short Height, short Passes, short Strength);

        // Original Solutions-Zone function names
        [DllImport("IpLrg32", EntryPoint = "IpLSFltShow", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLFltShow(short bShow);
        [DllImport("IpLrg32", EntryPoint = "IpLSFltApply", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLFltApply(short sType, short Width, short Height, short Passes, short Strength);
        //
        // Mail functions
        //



        [DllImport("IPMAIL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMail(string mailTo, string mailCC, string mailSubj, string mailMsg, string mailAttach);

        //
        // Definitions for Mosaic Auto-Pro Constants and Functions
        //
        // Copyright (c) 1998 - 2001, Media Cybernetics, Inc.
        //





        // Attributes
        public const int MA_IMAGESIZE = 1;
        public const int MA_IMAGEWIDTH = 2;
        public const int MA_IMAGEHEIGHT = 3;
        public const int MA_IMAGECLASS = 4;
        public const int MA_ROWS = 5;
        public const int MA_COLUMNS = 6;
        public const int MA_SPACING = 7;
        public const int MA_TITLE = 8;
        public const int MA_FOOTER = 9;
        public const int MA_CAPTION = 10;
        public const int MA_AUTOGRID = 11;
        public const int MA_PAGENUMBERS = 12;
        public const int MA_FONT = 13;
        public const int MA_FONTSIZE = 14;

        // Image Size types for MA_IMAGESIZE
        public const int MIS_PRINTER = 0;
        public const int MIS_PRINTERQTRSIZE = 1;
        public const int MIS_USER = 2;

        // Caption types for MA_CAPTION
        public const int MAC_NONE = 0;
        public const int MAC_IMAGENAME = 1;
        public const int MAC_FILENAME = 2;
        public const int MAC_DATETIME = 3;
        public const int MAC_DESCRIPTION = 4;
        public const int MAC_FRAMENUMBER = 5;


        //-----------------------------------------------------------------
        // Functions
        //-----------------------------------------------------------------
        [DllImport("IPMOS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMosaicShow(short Show);
        [DllImport("IPMOS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMosaicCreate(string lpszImages, short sNumImages);
        [DllImport("IPMOS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMosaicGet(short sAttr, ref Any lpParam, string lpszParam);		//TODO As Any
        [DllImport("IPMOS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMosaicSet(short sAttr, short sValue, string lpszParam);

        //
        // Definitions for third-party plug-in Auto-Pro Functions
        //
        // Copyright (c) 1998 - 1999, Media Cybernetics, Inc.
        //




        // Commands

        //-----------------------------------------------------------------
        // Psho Functions
        //-----------------------------------------------------------------
        [DllImport("IPPLUG32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPlShow(short nPlugType, short bShow);
        [DllImport("IPPLUG32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPlImport(string szImportName, short bShow);
        [DllImport("IPPLUG32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPlFilter(string szCategory, string szFilterName, short bShow);

        //
        // Display Range Auto-Pro functions and structures definitions.
        //
        // Copyright (c) 1999-2003 Media Cybernetics, Inc.
        //


        public const int DR_RANGE = 1;
        public const int DR_GAMMA = 2;
        public const int DR_INV = 3;
        public const int DR_BEST = 4;
        public const int DR_FRANGE = 5;
        public const int DR_RANGE_RESET = 6;


        [DllImport("IPRNGE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDrShow(short bShow);
        [DllImport("IPRNGE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDrGet(short Id, short sParam, ref Any lpParam);					//TODO As Any
        [DllImport("IPRNGE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpDrSet(short Id, short sParam, ref Any lpParam);					//TODO As Any
        //
        // Report Generator Auto-Pro functions and structures definitions.
        //
        // Copyright (c) 1999-2003 Media Cybernetics, Inc.
        //


        public const int RPT_DUMMY = 0;

        [DllImport("IPRPT32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpRptShow();
        [DllImport("IPRPT32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpRptNew(string templatefile);
        [DllImport("IPRPT32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpRptOpen(string reportfile);
        [DllImport("IPRPT32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpRptClose();
        [DllImport("IPRPT32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpRptSave(string reportfile);
        [DllImport("IPRPT32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpRptPrint();

        //
        // Auto-Pro functions and structures definitions for sequencer
        //



        // Sequence Play commands for use with IpSeqPlay
        public const int SEQ_STOP = -1;
        public const int SEQ_FOR = -2;
        public const int SEQ_REV = -3;
        public const int SEQ_FFOR = -4;
        public const int SEQ_FREV = -5;
        public const int SEQ_FFRA = -6;
        public const int SEQ_LFRA = -7;
        public const int SEQ_PREV = -8;
        public const int SEQ_NEXT = -9;

        // Sequence attributes for use with IpSeqGet and IpSeqSet
        public const int SEQ_NUMFRAMES = 1;
        public const int SEQ_ACTIVEFRAME = 2;
        public const int SEQ_FRAMETIME = 3;
        public const int SEQ_SKIP = 4;
        public const int SEQ_START = 5;
        public const int SEQ_END = 6;
        public const int SEQ_PLAYTYPE = 7;
        public const int SEQ_PLAYUPDATE = 8;
        public const int SEQ_APPLY = 9;
        public const int SEQ_SYNCALL = 10;
        public const int SEQ_PLAYING = 11;

        // Sequence play types
        public const int SEQ_PLAYWRAP = 1;
        public const int SEQ_PLAYTOEND = 2;
        public const int SEQ_PLAYAUTOREV = 3;


        //-----------------------------------------------------------------
        // Motion Analysis functions
        //-----------------------------------------------------------------
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqShow(short sShow);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpSeqGet(short sAttr, ref Any lpValue);								//TODO As Any
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqSet(short sAttr, int lNewAttr);

        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqMerge(string FileName, string Library, int StartNumber, int numframes);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqOpen(string FileName, string Library, int StartNumber, int numframes);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqPlay(int PlayCmd);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqSave(string FileName, string Library, int StartNumber, int numframes);

        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqDifference(int lStart, int lNumber);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqAverage(int lStart, int lNumber);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqRunningAvg(int lStart, int lNumber, int lAvgWindow, short bDropPartial);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqExtractFrames(int lStart, int lNumber);

        [DllImport("IPSM32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqOpenEx(string FileName, string Library, int StartNumber, int numframes, int Interval);
        //
        // Definitions for Sequence Gallery Auto-Pro Functions
        //
        // Copyright (c) 2001 - , Media Cybernetics, Inc.
        //




        //-----------------------------------------------------------------
        // Sequence Gallery Constants & commands
        //-----------------------------------------------------------------
        public const int SEQG_TRACKENABLE = 0;
        public const int SEQG_ISTRACKED = 1;
        public const int SEQG_ISGALLERY = 2;


        //-----------------------------------------------------------------
        // Sequence Gallery Functions
        //-----------------------------------------------------------------
        [DllImport("IPTHMB32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqGShow(short bShow);
        [DllImport("IPTHMB32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqGCreate();
        [DllImport("IPTHMB32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqGUpdate(short docid);
        [DllImport("IPTHMB32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqGSet(short sAttribute, short sParam);
        [DllImport("IPTHMB32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqGGet(short sAttribute, short sParam, ref short lpsData);

        //
        // IpSnap function
        //
        // Copyright (c) 1999-2003, Media Cybernetics, Inc.
        //




        //-----------------------------------------------------------------
        // Snap Functions
        //-----------------------------------------------------------------
        [DllImport("IPSNAP32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSnap();


        //
        // Definitions for Surface Plot Auto-Pro Functions
        //
        // Copyright (c) 1998 - 2001, Media Cybernetics, Inc.
        //




        //-----------------------------------------------------------------
        // Functions
        //-----------------------------------------------------------------
        public const int SP_DEFAULT = 100;
        public const int SP_VIEW_ELEVATION = 101;
        public const int SP_VIEW_ROTATION = 102;
        public const int SP_LIGHT_ELEVATION = 103;
        public const int SP_LIGHT_ROTATION = 104;
        public const int SP_LIGHT_COLOR = 105;
        public const int SP_STYLE_TYPE = 106;
        public const int SP_STYLE_WIREFRAME_SPAN = 107;
        public const int SP_STYLE_DRAWEDGES = 108;
        public const int SP_STYLE_DRAWAXES = 109;
        public const int SP_STYLE_ZSCALE = 110;
        public const int SP_AMBIENT_REFLECTANCE = 111;
        public const int SP_DIFFUSE_REFLECTANCE = 112;
        public const int SP_SPECULAR_REFLECTANCE = 113;
        public const int SP_GLOSS = 114;
        public const int SP_COLORIZED_FROM = 115;
        public const int SP_COLORIZED_TO = 116;
        public const int SP_COLORIZED_FROM_COLOR = 117;
        public const int SP_COLORIZED_TO_COLOR = 118;
        public const int SP_SURFACE_COLOR_SPIN = 119;
        public const int SP_SURFACE_COLOR_SPREAD = 120;

        public const int SP_STYLE_TEXTURED = 121;
        public const int SP_TEXTURE_ID = 122;
        public const int SP_SHADOW_DEPTH = 123;

        public const int SPO_NEW = 1;
        public const int SPO_NEW_WITH_ISCALE = 2;
        public const int SPO_PRINTER = 3;
        public const int SPO_CLIPBOARD = 4;

        public const int SPS_WIREFRAME = 0;
        public const int SPS_UNSHADED = 1;
        public const int SPS_SHADED = 2;

        [DllImport("IPSURF32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSurfShow(short bShow);
        [DllImport("IPSURF32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSurfSet(short sAttribute, int lData);
        [DllImport("IPSURF32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int IpSurfGet(short sAttribute, ref Any lpData);						//TODO As Any
        [DllImport("IPSURF32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSurfOutput(short OutputType);
        [DllImport("IPSURF32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSurfAutoRefresh(short bAutoRefresh);
        //
        // Definitions for Trace Auto-Pro Functions
        //
        // Copyright (c) 1998 - 2001, Media Cybernetics, Inc.
        //

        // Trace Module
        // IpTraceAttr()
        public const int TR_MODE = 1;
        public const int TR_PEN = 2;
        public const int TR_ERASER = 3;
        public const int TR_SHOW = 4;

        // IpTraceDo()
        public const int TR_DELETE = 0;
        public const int TR_AUTO = 1;
        public const int TR_IMAGE = 2;

        [DllImport("IPTRCE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTraceShow(short bShow);
        [DllImport("IPTRCE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTraceAttr(short sAttrib, int lValue);
        [DllImport("IPTRCE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTraceDo(short sCmd);
        //
        // Definitions for Object Tracking Functions
        //
        // Copyright (c) 1999-2003 - , Media Cybernetics, Inc.
        //



        // if this include file is included by task.h, only one of these set of macros should
        // be defined.
        // Commands

        //show/hide
        public const int TRACK_HIDE = 0;
        public const int TRACK_SHOW = 1;

        //tracking windows
        public const int TRACK_TABLE = 0;
        public const int TRACK_GRAPH = 1;

        // constants for IpTrackSaveData
        // sources of data
        public const int TR_MM_DATA = 1;
        public const int TR_MM_STATS = 2;
        public const int TR_MM_ACTIVE = 3;
        public const int TR_GRAPH = 7;

        // types of data
        public const int TRF_TABLE = 0x100;
        public const int TRF_GRAPH = 0x200;

        // destinations
        public const int TRDF_FILE = 1;
        public const int TRDF_CLIPBOARD = 2;
        public const int TRDF_DDE = 4;
        public const int TRDF_MASK = 0xff;

        public const int TRDF_CSV = 0x200;
        public const int TRDF_HTML = 0x400;
        public const int TRDF_LAST = 0x800;
        // TRDF_CSV and TRDF_HTML cannot be combined. In absence of either, will be tab-delimited


        //measurement labels
        public const int trLabelsShowName = 0;
        public const int trLabelsShowMeasurement = 1;
        public const int trLabelsShowNone = 2;

        //time units
        public const int trtuSecond = 0;
        public const int trtuMinute = 1;
        public const int trtuHour = 2;

        //graph X axis label type
        public const int trxlFrameNumber = 0;
        public const int trxlRelTime = 1;
        public const int trxlAbsTime = 2;

        //track measurements
        public const int TRM_DIST = 0;
        public const int TRM_X_COORD = 1;
        public const int TRM_Y_COORD = 2;
        public const int TRM_OR_DIST = 3;
        public const int TRM_ANGLE = 4;
        public const int TRM_SPEED = 5;
        public const int TRM_ACCELERATION = 6;
        public const int TRM_ACC_DIST = 7;

        //statistics
        public const int TRSTMean = 0;
        public const int TRSTStDev = 1;
        public const int TRSTMin = 2;
        public const int TRSTMax = 3;
        public const int TRSTRange = 4;
        public const int TRSTSum = 5;
        public const int TRSTIndMin = 6;
        public const int TRSTIndMax = 7;
        public const int TRSTNObj = 8;
        public const int TRSTNShown = 9;

        public const int TRSTEnd = 10;

        public const int TR_VALUE = 100;

        public const int TR_ALL = -1;


        //tracking measurements options
        public const int TM_TRACK_COLOR = 0;
        public const int TM_SEL_COLOR = 1;
        public const int TM_EL_SIZE = 2;
        public const int TM_TEXT_COLOR = 3;
        public const int TM_FONT_SIZE = 4;
        public const int TM_LABEL_TYPE = 5;
        public const int TM_TRACK_PREF_SET = 6;
        public const int TM_TRACK_PREF_GET = 7;
        public const int TM_RESET_MEAS = 8;
        public const int TM_ADD_MEAS = 9;
        public const int TM_SHOW_STATS = 11;
        public const int TM_STATS_GET = 12;
        public const int TM_NUM_TRACKS_GET = 13;
        public const int TM_NUM_POINTS_GET = 14;
        public const int TM_POINTS_GET = 15;
        public const int TM_NUM_MEAS_GET = 16;
        public const int TM_MEAS_LIST_GET = 17;
        public const int TM_MEAS_GET = 18;
        public const int TM_SEL_GET = 19;
        public const int TM_SEL_SET = 20;
        public const int TM_SHOW_GET = 21;
        public const int TM_SHOW_SET = 22;
        public const int TM_NAME_GET = 23;
        public const int TM_NAME_SET = 24;
        public const int TM_UPDATE = 25;
        public const int TM_TYPE_GET = 26;
        public const int TM_ACTION = 27;
        public const int TM_CREATE_MEAS = 28;
        public const int TM_SHOW_ALL = 29;
        public const int TM_SHOW_SELECTED = 30;
        public const int TM_DELETE_ALL = 31;
        public const int TM_DELETE_SELECTED = 32;
        public const int TM_COLORING = 33;
        public const int TM_ADD_TRACK = 34;
        public const int TM_COLOR_GET = 35;
        public const int TM_COLOR_SET = 36;
        public const int TM_NUM_DEC = 37;
        public const int TM_TIME_UNITS = 38;
        public const int TM_ADD_AUTO_TRACK = 39;
        public const int TM_SEARCH_RADIUS = 40;
        public const int TM_INIT_AUTO_TRACKING = 41;
        public const int TM_UPDATE_SET = 42;
        public const int TM_ADD_AUTO_ALL_TRACKS = 43;
        public const int TM_ACCEL_LIMIT = 44;
        public const int TM_AUTO_ACCEL_LIMIT = 45;
        public const int TM_PARTIAL_TRACKS = 46;
        public const int TM_MIN_TRACK_LEN = 47;
        public const int TM_SHOW_OUTLINES = 48;
        public const int TM_AUTO_SPLIT = 49;
        public const int TM_WATERSHED_SPLIT = 70;
        public const int TM_SHARED_OBJECTS = 71;
        public const int TM_MOTION_TYPE = 72;
        public const int TM_MIN_TRACK_LENGTH = 73;
        public const int TM_SWAP_RC = 74;
        public const int TM_MERGE_SELECTED = 75;
        //graph variables
        public const int TM_GRAPH_MEAS = 50;
        public const int TM_GRAPH_RANGE_AUTO = 51;
        public const int TM_GRAPH_RANGE_MIN = 52;
        public const int TM_GRAPH_RANGE_MAX = 53;
        public const int TM_GRAPH_X_LABELS = 54;


        public const int TRAC_ATTR_SHORT = 10000;
        public const int TRAC_ATTR_LONG = 10010;
        public const int TRAC_ATTR_FLOAT = 10020;
        // there is no double for Auto-Pro function argument.
        public const int TRAC_ATTR_STRING = 10030;


        //-----------------------------------------------------------------
        // Trac Functions
        //-----------------------------------------------------------------
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTrackShow(short sDialog, short bShow);
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTrackMove(short sDialog, short xPos, short yPos);
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTrackSize(short sDialog, short xSize, short ySize);

        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTrackOptionsFile(string szFileName, short bSave);
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTrackFile(string szFileName, short bSave);

        //tracking measurements
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTrackMeas(short sCommand, int lOpt1, ref Any lParam);				//TODO As Any
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTrackMeasSet(short sCommand, int lOpt1, double lParam);
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTrackMeasSetStr(short sCommand, int lOpt1, string lpszDest);
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTrackMeasGetStr(short sCommand, int lOpt1, string lpszDest);
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTrackSaveData(short sSrcFlags, short sDstFlags, string lpszDest);

#endif
       

        // 
        // Built-in Auto-Pro functions 
        // Definitions for built-in Auto-Pro functions. These functions are 
        // exported by IPC32.DLL. 
        // 




        //----------------------------------------------------------------- 
        // windows defininition 
        //----------------------------------------------------------------- 
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct POINTAPI
        {
            public int x;
            public int y;
            public POINTAPI(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        };

        //----------------------------------------------------------------- 
        // Pre-defined variables 
        //----------------------------------------------------------------- 

        public const short IPC_SIZECLASSIFIERS = 3;
        public const short IPC_SIZEICAL = 256;

        public static RECT ipRect;
        public static POINTAPI[] Pts = new POINTAPI[2049];
        public static short[] Lut = new short[257];
        public static float[] ipBins = new float[17];
        public static short[] ipClassifiers = new short[IPC_SIZECLASSIFIERS + 1];
        public static float[] ipICal = new float[IPC_SIZEICAL + 1];
        public static short[] ipHalfTypes = new short[8];
        public static short[] ipHalfScreens = new short[8];
        public static int ret;
        public static int IPNULL;
        public static short[] ipArray = new short[11];
        public static int[] ipLArray = new int[11];
        public static int ipLVal;
        public static float ipSVal;
        public static double lpDVal;
        public static double ipDVal;

        //警告がでるのでコメントにする//
        //public static Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString ipStrFixed = new Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString(255);
        

        //----------------------------------------------------------------- 
        // HIL definitions 
        //----------------------------------------------------------------- 

        public const short IMC_GRAY = 1;
        public const short IMC_PALETTE = 2;
        public const short IMC_RGB = 3;
        public const short IMC_GRAY12 = 4;
        public const short IMC_FLOAT = 5;
        public const short IMC_GRAY16 = 6;
        public const short IMC_RGB36 = 8;
        public const short IMC_RGB48 = 9;

        public const int IMC_C_SCALE = 0x800;
        public const int IMC_C_DIRECT = 0x400;

        public const short OR_LEFTRIGHT = 1;
        public const short OR_UPDOWN = 2;
        public const short OR_TRANSPOSE = 3;
        public const short OR_ROTATE90 = 4;
        public const short OR_ROTATE270 = 5;
        public const short OR_ROTATE180 = 6;

        public const short RA_CENTER = 5;

        public const short EQ_BESTFIT = 0;
        public const short EQ_LINEAR = 1;
        public const short EQ_BELL = 2;
        public const short EQ_LOGARITHMIC = 3;
        public const short EQ_EXPONENTIAL = 4;
        public const short EQ_WHITEBAL = 5;

        public const short IFFCOMP_NONE = 0;
        public const short IFFCOMP_DEFAULT = 1;
        public const short IFFCOMP_RLE = 2;
        public const short IFFCOMP_CCITT1D = 3;
        public const short IFFCOMP_CCITTG3 = 4;
        public const short IFFCOMP_CCITTG4 = 5;
        public const short IFFCOMP_LZW = 6;
        public const short IFFCOMP_LZWHPRED = 7;
        public const short IFFCOMP_JPEG = 8;

        // retained for compatibility - no longer supported by IpWsConvertFile 
        public const short IFFCL_BILEVEL = 0;
        public const short IFFCL_GRAY = 1;
        public const short IFFCL_PALETTE = 2;
        public const short IFFCL_RGB = 3;
        // retained for compatibility - not supported by IpWsConvertFile 
        public const short IFFCL_RGBPLANAR = 4;
        // retained for compatibility - not supported by IpWsConvertFile 
        public const short IFFCL_RGBA = 5;
        // retained for compatibility - not supported by IpWsConvertFile 
        public const short IFFCL_RGBAPLANAR = 6;

        // for IpDocOpen 
        public const short IMA_RD = 1;
        public const short IMA_RDWR = 2;


        //----------------------------------------------------------------- 
        // HAIL definitions 
        //----------------------------------------------------------------- 

        public const short CM_RGB = 0;
        public const short CM_HSI = 1;
        public const short CM_HSV = 2;
        public const short CM_YIQ = 3;

        // count/size (blob) measurements 
        public const short BLBM_ALL = -1;
        public const short BLBM_AREA = 0;
        public const short BLBM_ASPECT = 1;
        public const short BLBM_BOX_AREA = 2;
        public const short BLBM_BOX_XY = 3;
        public const short BLBM_CENTRX = 4;
        public const short BLBM_CENTRY = 5;
        public const short BLBM_DENSITY = 6;
        public const short BLBM_DIRECTION = 7;
        public const short BLBM_HOLEAREA = 8;
        public const short BLBM_HOLEAREARATIO = 9;
        public const short BLBM_MAJORAX = 10;
        public const short BLBM_MINORAX = 11;
        public const short BLBM_MAXFERRET = 12;
        public const short BLBM_MINFERRET = 13;
        public const short BLBM_MEANFERRET = 14;
        public const short BLBM_MAXRADIUS = 15;
        public const short BLBM_MINRADIUS = 16;
        public const short BLBM_NUMHOLES = 17;
        public const short BLBM_PERIMETER = 18;
        public const short BLBM_RADIUSRATIO = 19;
        public const short BLBM_ROUNDNESS = 20;
        public const short BLBM_CLUSTER = 21;
        public const short BLBM_RED = 22;
        public const short BLBM_GREEN = 23;
        public const short BLBM_BLUE = 24;
        public const short BLBM_PERAREA = 25;
        public const short BLBM_CLASS = 26;
        public const short BLBM_LENGTH = 27;
        public const short BLBM_WIDTH = 28;
        public const short BLBM_PERIMETER2 = 29;
        public const short BLBM_IOD = 30;
        public const short BLBM_PCONVEX = 31;
        public const short BLBM_PELLIPSE = 32;
        public const short BLBM_PRATIO = 33;
        public const short BLBM_AREAPOLY = 34;
        public const short BLBM_FRACTDIM = 35;
        public const short BLBM_CMASSX = 36;
        public const short BLBM_CMASSY = 37;
        public const short BLBM_SIZECOUNT = 38;
        public const short BLBM_BOXX = 39;
        public const short BLBM_BOXY = 40;
        public const short BLBM_MINCALIP = 41;
        public const short BLBM_MAXCALIP = 42;
        public const short BLBM_MEANCALIP = 43;
        public const short BLBM_DENSMIN = 44;
        public const short BLBM_DENSMAX = 45;
        public const short BLBM_DENSDEV = 46;
        public const short BLBM_BRANCHLEN = 47;
        public const short BLBM_DENDRITES = 48;
        public const short BLBM_ENDPOINTS = 49;
        public const short BLBM_MARGINATION = 50;
        public const short BLBM_HETEROGENEITY = 51;
        public const short BLBM_CLUMPINESS = 52;
        public const short BLBM_DENSSUM = 53;
        public const short BLBM_SRANGE = 54;
        public const short BLBM_PERIMETER3 = 55;
        public const short BLBM_PERIMETERLEN = 56;
        public const short BLBM_NUM_MEAS = 57;

        // count/size extended measurements 
        public const short BLEX_RADIUS = 1000;
        public const short BLEX_DIAMETER = 1001;
        public const short BLEX_CALIPER = 1002;
        public const short BLEX_BRANCHLEN = 1003;

        // population density measurements 
        public const short BPOP_OBJECTS = 2000;
        public const short BPOP_AREA = 2001;
        public const short BPOP_DENSITY = 2002;
        public const short BPOP_CORRDENSITY = 2003;

        // operations 
        public const short OPA_ADD = 0;
        public const short OPA_SUB = 1;
        public const short OPA_DIFF = 2;
        public const short OPA_MULT = 3;
        public const short OPA_DIV = 4;
        public const short OPA_AVG = 5;
        public const short OPA_MAX = 6;
        public const short OPA_MIN = 7;
        // 8 is reserved for internal use 
        public const short OPA_ACC = 9;
        public const short OPA_INV = 10;
        public const short OPA_X2 = 11;
        public const short OPA_SQR = 12;
        public const short OPA_LOG = 13;
        public const short OPA_EXP = 14;
        public const short OPA_X2Y = 15;
        public const short OPA_SET = 16;

        public const short OPL_AND = 0;
        public const short OPL_OR = 1;
        public const short OPL_XOR = 2;
        public const short OPL_NAND = 3;
        public const short OPL_NOR = 4;
        public const short OPL_NOT = 5;
        public const short OPL_COPY = 6;

        public const short MORPHO_2x2SQUARE = 0;
        public const short MORPHO_3x1ROW = 1;
        public const short MORPHO_1x3COLUMN = 2;
        public const short MORPHO_3x3CROSS = 3;
        public const short MORPHO_5x5OCTAGON = 4;
        public const short MORPHO_CUSTOM = 5;

        public const int FFT_NOTCH = 0x0;
        public const int FFT_PHASE = 0x1;
        public const int FFT_SPECTRUM = 0x2;
        public const int FFT_HANNING = 0x1000;


        // for IpFftForward. Uses FFT_PHASE and FFT_SPECTRUM above also 
        public const int FFT_PHASE32 = 0x3;
        public const int FFT_SPECTRUM32 = 0x4;
        public const int FFT_SPECPHAS32 = 0x5;

        public const short MORPHO_7x7OCTAGON = 6;
        public const short MORPHO_11x11OCTAGON = 7;

        public const short FFT_SOURCE = -1;
        public const short FFT_NEWIMAGE = -2;
        public const short FFT_NEWFLOAT = -3;

        //----------------------------------------------------------------- 
        // IPWIN definitions 
        //----------------------------------------------------------------- 
        public const short COLORMODEL = 100;
        public const short AUTOUPDATE = 102;
        public const short SCAL = 103;
        public const short ICAL = 104;
        public const short ACCUMULATE = 105;
        public const short LINETYPE = 106;
        public const short BIN = 107;
        public const short BRIGHTNESS = 108;
        public const short CONTRAST = 109;
        public const short GAMMA = 110;
        public const short STATISTICS = 111;
        public const short ORIGIN = 112;
        public const short GRID = 113;
        public const short REFERENCE = 114;
        public const short FREEZE = 115;
        public const short CURVE = 116;
        public const short UNIT = 117;
        public const short Channel = 118;
        public const short CHANNEL1 = 120;
        public const short CHANNEL2 = 121;
        public const short CHANNEL3 = 122;
        public const short ADVANCED = 123;
        public const short SETCURSEL = 124;
        public const short SEGMETHOD = 125;
        public const short SETTABS = 126;
        public const short LINEGEOMETRY = 127;
        public const short INVERT = 128;
        public const short SEGCLR_RED = 129;
        public const short SEGCLR_GREEN = 130;
        public const short SEGCLR_BLUE = 131;
        public const short CURSORSIZE = 132;
        public const short DEGREE = 133;
        public const short Threshold = 134;
        public const short SEG_SENSITIVITY = 135;

        public const short PROFTYPE_LINE = 1016;
        public const short PROFTYPE_CIRCLE = 1017;
        public const short PROFTYPE_FREEFORM = 1018;

        public const short BLOB_OUTLINEMODE = 0;
        public const short BLOB_OUTLINECOLOR = 1;
        public const short BLOB_LABELMODE = 2;
        public const short BLOB_LABELCOLOR = 3;
        public const short BLOB_SMOOTHING = 4;
        public const short BLOB_MINAREA = 5;
        public const short BLOB_FILLHOLES = 6;
        public const short BLOB_BRIGHTOBJ = 8;
        public const short BLOB_AUTORANGE = 9;
        public const short BLOB_MEASUREOBJECTS = 10;
        public const short BLOB_FILTEROBJECTS = 11;
        public const short BLOB_ADDCOUNT = 12;
        public const short BLOB_CLEANBORDER = 13;
        public const short BLOB_CONVEX = 14;
        public const short BLOB_8CONNECT = 15;
        public const short BLOB_DISPLAY = 16;
        // Note: ignored unless BLOB_LABELMODE is set to 3 (measurement labels) - specified measurement must be selected 
        public const short BLOB_LABELMEAS = 17;
        // columns displayed in Classification grid 
        public const short BLOB_CLASS_DISPLAY = 18;
        // columns displayed in Range statistics grid 
        public const short BLOB_RANGE_DISPLAY = 19;
        // Used when the Count/Size operation is used by other features, and data from the count should not be collected by Data Collector 
        public const short BLOB_SUPPRESS_COLLECT = 20;

        // for the IpSegPreview 
        public const short PREVIEW_NONE = 0;
        public const short CURRENT_C_T = 1;
        public const short ALL_C_T = 2;
        public const short ALL_T_W = 3;
        public const short CURRENT_W_B = 4;
        public const short CURRENT_B_W = 5;
        public const short CURRENT_C_B = 6;
        public const short CURRENT_C_W = 7;
        public const short CURRENT_B_T = 8;
        public const short CURRENT_T_B = 9;
        public const short CURRENT_T_W = 10;
        public const short ALL_W_B = 11;
        public const short ALL_B_W = 12;
        public const short ALL_C_B = 13;
        public const short ALL_C_W = 14;
        public const short ALL_B_T = 15;
        public const short ALL_T_B = 16;

        public const short CM = 0;
        public const short INCHES = 1;
        public const short PIXELS = 2;

        public const short SEG_SELNEW = 0;
        public const short SEG_SELADD = 1;
        public const short SEG_SELSUBTRACT = 2;

        public const short SEG_HISTOGRAM = 0;
        public const short SEG_COLORCUBE = 1;

        public const short MASK_BILEVELNEW = 4;
        public const short MASK_BILEVELINPLACE = 5;
        public const short MASK_COLORNEW = 6;

        public const short FillColor = 0;
        public const short FILLHUE = 1;
        public const short FILLTINT = 2;
        public const short FILLPATTERN = 3;
        public const short FILLTEXTURE = 4;

        public const short CLRFORE = 0;
        public const short CLRBACK = 1;
        public const short CLRWHITE = 2;
        public const short CLRBLACK = 3;

        public const short FRAME_RESET = -1;
        public const short FRAME_NONE = 0;
        public const short FRAME_RECTANGLE = 1;
        public const short FRAME_ELLIPSE = 2;
        public const short FRAME_IRREGULAR = 3;
        public const short FRAME_DONUT = 4;
        public const short FRAME_INVIEW = 10;

        public const short THICKNORMAL = 0;
        public const short THICKHORZ = 1;
        public const short THICKVERT = 2;
        public const short THICKAVG = 3;
        public const short THICKSTDDEV = 4;

        public const short LUT_BRIGHTNESS = 0;
        public const short LUT_CONTRAST = 1;
        public const short LUT_GAMMA = 2;
        public const short LUT_HISHAD = 3;
        public const short LUT_4TONES = 4;
        public const short LUT_8TONES = 5;
        public const short LUT_FREEFORM = 6;
        public const short LUT_ALL = 7;

        // for IpLutData 
        public const short LUT_GET_LENGTH = 0;
        public const short LUT_GET_DATA = 1;
        public const short LUT_SET_DATA = 2;
        public const short LUT_GET_BRIGHTNESS = 3;
        public const short LUT_GET_CONTRAST = 4;
        public const short LUT_GET_GAMMA = 5;

        // measurement attributes 
        public const short MEAS_STATS = 0;
        public const short MEAS_DISPCOLOR = 1;
        public const short MEAS_LABELCOLOR = 2;
        public const short MEAS_THICKMODE = 3;
        public const short MEAS_REPEAT = 9;
        public const short MEAS_ANGLE180 = 10;
        public const short MEAS_CLICK = 11;
        public const short MEAS_MEASCOLOR = 40;
        public const short MEAS_UPDATE = 41;
        public const short MEAS_PROMPTS = 42;
        public const short MEAS_SHOWLAYOUT = 43;
        public const short MEAS_DISPBFPTS = 44;
        public const short MEAS_MAXLINEPTS = 45;
        public const short MEAS_MAXCIRCLEPTS = 46;
        public const short MEAS_MAXARCPTS = 47;
        public const short MEAS_PASSFAILTYPE = 48;
        public const short MEAS_DISPCOUNTOPTS = 49;
        public const short MEAS_DISPLAYFEATURES = 50;
        public const short MEAS_SIGNIFICANTDIGITS = 51;
        public const short MEAS_DISPLAYTYPE = 52;
        public const short MEAS_DISPLAYLINEWIDTH = 53;
        public const short MEAS_DISPLAYFONTSIZE = 54;
        public const short MEAS_DISPLAYFONTWEIGHT = 55;
        public const short MEAS_TAG = -2;
        public const short MEAS_ALL = -1;

        // special SETs for IpMeasAttrStr 
        public const short MEAS_SETNAME = 300;
        public const short MEAS_DISPLAYFONTFACE = 301;

        // measurement tools 
        // no active tool 
        public const short MEAS_NONE = -1;
        // line 
        public const short MEAS_LENGTH = 4;
        // polygon 
        public const short MEAS_AREA = 5;
        public const short MEAS_ANGLE = 6;
        public const short MEAS_TRACE = 7;
        public const short MEAS_THICK = 8;
        public const short MEAS_POINT = 20;
        public const short MEAS_RECT = 21;
        public const short MEAS_CIRCLE = 22;
        public const short MEAS_BFLINE = 23;
        public const short MEAS_BFCIRCLE = 24;
        public const short MEAS_BFARC = 25;
        public const short MEAS_DIST = 26;
        public const short MEAS_NEWANGLE = 27;
        public const short MEAS_HTHICK = 28;
        public const short MEAS_VTHICK = 29;
        public const short MEAS_CTHICK = 30;
        public const short MEAS_PERPDIST = 31;
        public const short MEAS_COUNT = 32;
        public const short MEAS_DATA_TO_IMAGE = 33;
        public const short MEAS_SELECT = 100;

        // measurement data types 
        public const short MDATA_POS = 0;
        public const short MDATA_POSY = -1;
        public const short MDATA_AREA = -2;
        public const short MDATA_LEN = -3;
        public const short MDATA_RADIUS = -4;
        public const short MDATA_START = -5;
        public const short MDATA_STARTY = -6;
        public const short MDATA_END = -7;
        public const short MDATA_ENDY = -8;
        public const short MDATA_ANGLE = -9;
        public const short MDATA_AVGDIST = -10;
        public const short MDATA_MINDIST = -11;
        public const short MDATA_MAXDIST = -12;
        public const short MDATA_CTRDIST = -13;
        public const short MDATA_PERPDIST = -14;
        public const short MDATA_COUNT = -15;

        // special GET for IpMeasGet 
        public const short GETNAME = 200;
        public const short GETFEATVALUES = 201;
        public const short GETNUMMEAS = 202;
        public const short GETMEASVALUES = 203;
        public const short GETNUMSELFEATURES = 204;
        public const short GETSELFEATURES = 205;

        // measurement load options 
        // load measurement template and prompt for user to make specified measurements 
        public const short MLOAD_INTERACTIVE = 1;
        // re-load previously measured features 
        public const short MLOAD_RELOAD = 2;

        // measurement IpMeasShow options 
        public const short MEAS_HIDE = 0;
        // show last used page 
        public const short MEAS_SHOW = 1;
        public const short MEAS_SHOWADVANCED = 2;
        public const short MEAS_SHOWBASIC = 4;
        // show Features page (can combine with MEAS_SHOWADVANCED or MEAS_SHOWBASIC) 
        public const short MEAS_SHOWFEATURES = 8;
        // show Measurements page (must be MEAS_SHOWADVANCED) 
        public const short MEAS_SHOWMEASUREMENTS = 18;
        // show Input/Output page (can combine with MEAS_SHOWADVANCED or MEAS_SHOWBASIC) 
        public const short MEAS_SHOWINPUTOUTPUT = 32;
        // show Options page (can combine with MEAS_SHOWADVANCED or MEAS_SHOWBASIC) 
        public const short MEAS_SHOWOPTIONS = 64;
        // show Advanced Options page (must be MEAS_SHOWADVANCED) 
        public const short MEAS_SHOWADVOPTIONS = 130;

        // measurement pass/fail tolerance options 
        public const short MPF_NONE = 0;
        public const short MPF_TOLERANCES = 1;
        public const short MPF_MINMAX = 2;

        // measurement on-image feature display options (for MEAS_DISPLAYTYPE) 
        // short-cut to none selected 
        public const short MDISP_NONE = 0;
        // the default 
        public const short MDISP_NAME = 1;
        // these can all be 
        public const short MDISP_VALUE = 2;
        // combined, e.g. a value of 7 shows all 
        public const short MDISP_UNITS = 4;

        public const short TAG_VIEW_COUNTS = 0;
        public const short TAG_VIEW_POINTS = 1;
        public const short TAG_VIEW_CLASSSTATS = 2;
        public const short TAG_VIEW_AREA = 3;
        public const short TAG_VIEW_LABEL = 4;
        public const short TAG_VIEW_MARKER = 5;

        public const short TAG_MEAS_XPOS = 6;
        public const short TAG_MEAS_YPOS = 7;
        public const short TAG_MEAS_INTENSITY = 8;
        public const short TAG_MEAS_CLASS = 9;
        public const short TAG_MEAS_RED = 10;
        public const short TAG_MEAS_GREEN = 11;
        public const short TAG_MEAS_BLUE = 12;
        public const short TAG_MEAS_AREA = 13;
        public const short TAG_MEAS_RADIUS = 14;
        public const short TAG_ACTIVECLASS = 15;

        public const short AOIDELETE = 0;
        public const short AOIADD = 1;
        public const short AOISET = 2;
        public const short AOISHOWDLG = 3;
        public const short AOIHIDEDLG = 4;
        public const short AOILOAD = 5;
        public const short AOISAVE = 6;

        public const short DOCSEL_NEXTID = -1;
        public const short DOCSEL_PREVID = -2;
        public const short DOCSEL_ACTIVE = -3;
        public const short DOCSEL_ALL = -4;
        public const short DOCSEL_NONE = -5;

        public const short GET_VALUE = 0;
        public const short SET_VALUE = 1;

        // for IpAppArrange() 
        public const short DOCS_CASCADE = 0;
        public const short DOCS_TILE = 1;
        public const short DOCS_OVERLAP = 2;

        // for IpAcqSnap() 
        public const short ACQ_NEW = -1;
        public const short ACQ_CURRENT = -2;
        public const short ACQ_FILE = -3;
        public const short ACQ_SEQUENCE = -4;
        public const short ACQ_SEQUENCE_APPEND = -5;
        // New commands for IpAcqSnap which do not change the destination. 
        public const short ACQ_NEWEX = 1;
        public const short ACQ_CURRENTEX = 2;
        public const short ACQ_FILEEX = 3;
        public const short ACQ_SEQUENCEEX = 4;
        public const short ACQ_SEQUENCE_APPENDEX = 5;
        public const short ACQ_2VRIONLY = 6;

        // for IpAppRun() 
        // SW_SHOW 
        public const short RUN_NORMAL = 5;
        // SW_SHOWMINIMIZED 
        public const short RUN_MINIMIZED = 2;
        // SW_SHOWMAXIMIZED 
        public const short RUN_MAXIMIZED = 3;
        public const short RUN_AUTOCLOSE = 1;
        public const short RUN_MODAL = 2;

        // IpXxxxSave SaveMode flags 
        // Data to send 
        // Main data 
        public const int S_DATA1 = 0x4;
        // Secondary data 
        public const int S_DATA2 = 0x8;
        // Statistics of S_DATA1 or S_DATA2 
        public const int S_STATS = 0x2;

        // Output format 
        // Text table 
        public const int S_TABLE = 0x40;
        // Plot or picture 
        public const int S_GRAPH = 0x80;

        // Destination 
        // File 
        public const int S_FILE = 0x0;
        // Clipboard 
        public const int S_CLIPBOARD = 0x10;
        // Output window 
        public const int S_OUTPUT = 0x20;
        // DDE to Excel 
        public const int S_DDE = 0x1000;
        // Database 
        public const int S_DATABASE = 0x2000;
        // Printer 
        public const int S_PRINTER = 0x4000;

        // Append or replace 
        // Append to existing data 
        public const int S_APPEND = 0x1;
        // Used with S_DB only 
        public const int S_NEW = 0x1;

        // Adornments 
        // Title 
        public const int S_HEADER = 0x100;
        public const int S_LEGEND = 0x200;
        public const int S_X_AXIS = 0x400;
        public const int S_Y_AXIS = 0x800;
        //flag for Profile, export pixel coordinates (same as S_Y_AXIS, which is not used with Profile) 
        public const int S_COORDS = 0x800;

        // Obsolete or old style name 
        // Old name of S_DB 
        public const int S_RECORD = 0x2000;
        // Old name of S_DATA1 
        public const int S_DATA = 0x4;
        // Old name of S_DATA2 
        public const int S_RANGE = 0x8;

        // for IpAppMenuSelect() 
        public const short MENU_ID = 1;
        public const short MENU_NAME = 2;
        public const short MENU_COORD = 4;
        public const short MENU_DLL = 8;
        public const short DLG_MENU_ID = 16;
        public const short DLG_MENU_NAME = 32;
        public const short DLG_MENU_COORD = 64;

        // for IpAppWindow() 
        public const short APW_GETNAME = 0;
        public const short APW_GETID = 1;
        public const short APW_GETHWND = 2;
        public const short APW_ACTIVATENAME = 3;
        public const short APW_ACTIVATEID = 4;
        public const short APW_ACTIVATEHWND = 5;

        // for IpAppCtl() 
        public const short APC_GETHWND = 1;
        public const short APC_CLICK = 2;
        public const short APC_GETFOCUSID = 3;
        public const short APC_SETFOCUSID = 4;
        public const short APC_GETCHECK = 5;
        public const short APC_SETCHECK = 6;
        public const short APC_GETSCROLL = 7;
        public const short APC_SETSCROLL = 8;
        public const short APC_GETCURSEL = 9;
        public const short APC_SETCURSEL = 10;
        public const short APC_SETPOSX = 11;
        public const short APC_SETPOSY = 12;

        // for IpAppWndState() 
        public const int WST_ENABLED = 0x1;
        public const int WST_VISIBLE = 0x2;
        public const int WST_NORMAL = 0x4;
        public const int WST_MINIMIZED = 0x8;
        public const int WST_MAXIMIZED = 0x10;

        // for IpWsLoadRes() 
        public const short LOAD_PROMPT = -1;
        public const short LOAD_SMALLEST = -3;
        public const short LOAD_LARGEST = -4;

        // for IpPrtSize() 
        public const short PRT_ACTUAL = 1;
        public const short PRT_FIT = 2;
        public const short PRT_DISTORT = 3;

        // for IpAppGet, IpDocGet() and IpWsChangeDescription() 
        public const short GETACTDOC = 1;
        public const short GETDOCWND = 2;
        public const short GETDOCVRI = 3;
        public const short GETAPPWND = 4;
        public const short GETNUMDOC = 5;
        public const short GETDOCLST = 6;
        public const short GETDOCINFO = 7;
        public const short GETINSTINFO = 8;
        public const short GETPLUGSN = 9;
        public const short GETAPPDIR = 10;
        public const short GETAPPNAME = 11;
        public const short GETAPPVERSION = 12;
        public const short GETOSVERSION = 13;
        public const short GETAPPSETTINGSDIR = 14;

        public const short INF_SUBJECT = 19;
        public const short INF_TITLE = 20;
        public const short INF_ARTIST = 21;
        public const short INF_DATE = 22;
        public const short INF_DESCRIPTION = 23;
        public const short INF_DPIX = 24;
        public const short INF_DPIY = 25;
        public const short INF_RANGE = 26;
        public const short INF_NAME = 27;
        public const short INF_MAXRANGE = 28;
        public const short INF_FILENAME = 29;
        public const short INF_XPOSITION = 30;
        public const short INF_YPOSITION = 31;
        public const short INF_ZPOSITION = 32;
        public const short INF_XSCROLL = 33;
        public const short INF_YSCROLL = 34;
        public const short INF_ZOOMFACTOR = 35;
        public const short INF_COPYRIGHT = 36;
        public const short INF_IS_MODIFIED = 37;

        public const short DOC_POS_X = 40;
        public const short DOC_POS_Y = 41;

        // for IpAppGet and IpAppSet 
        public const short PST_BLEND_PREVIEW = 50;
        public const short PST_BLEND_APPLY = 51;
        public const short PST_APPLY_TYPE = 52;
        public const short PST_BLEND_SOURCE = 53;
        // if non-zero wait for button press, else pause for delay and resume 
        public const short MACRO_PAUSE_TYPE = 54;
        // set behavior of the Window, Tile command 
        public const short WINDOW_TILING_TYPE = 55;
        // control sync of pan/scroll/zoom of all workspaces 
        public const short SYNC_PAN_SCROLL_ZOOM = 56;
        // control dark lab mode 
        public const short DARK_LAB_MODE = 57;
        // the "open as sequence" option for File, Open 
        public const short FILE_OPEN_AS_SEQ = 58;
        // the "save as series" option for File, Save As and IpWsSaveAs 
        public const short FILE_SAVE_AS_SERIES = 59;
        // the "Prompt before closing modified images" option from the Application page of Edit, Preferences 
        public const short PROMPT_WHEN_MODIFIED = 60;
        // the "Prompt for menu selection on startup" option from the Application page of Edit, Preferences 
        public const short PROMPT_FOR_MENU_SELECT = 61;
        // the "Prompt for subsample of multi-image/sequence files" option from the Document page of Edit, Preferences 
        public const short PROMPT_FOR_SEQUENCES = 62;
        // get/set whether to apply dye info and (optionally) tint (see dye apply types) 
        public const short CAPT_APPLY_DYE = 63;
        // get/set whether to apply lens info 
        public const short CAPT_APPLY_LENS = 64;

        // paste apply types for use with IpAppGet/Set PST_APPLY_TYPE 
        public const short PST_APPLY_ALL = 0;
        public const short PST_APPLY_LIGHTER = 1;
        public const short PST_APPLY_DARKER = 2;

        // window tiling types for use with IpAppGet/Set WINDOW_TILING_TYPE 
        // behavior of all previous releases 
        public const short TILE_NORMAL = 0;
        // select zoom factor to try to make as much of the images visible as possible 
        public const short TILE_ZOOM_TO_FIT = 1;
        // reorder based on order of open/capture/creation 
        public const short TILE_REORDER = 2;
        // resize after tiling so all workspaces are the same size 
        public const short TILE_SAMESIZE = 4;
        // compact so workspaces are directly adjacent to each other - requires TILE_RESIZE 
        public const short TILE_COMPACT = 8;

        // dye apply types for use with IpAppGet/Set CAPT_APPLY_DYE 
        public const short CAPT_DYE_NONE = 0;
        public const short CAPT_DYE_INFO_ONLY = 1;
        public const short CAPT_DYE_INFO_AND_TINT = 2;

        // for IpBlbGet() 
        // tag == 0 or class number; see below for Param2 flags 
        public const short GETNUMOBJ = 1;
        public const short GETHBLOB = 2;
        public const short GETTHRESH = 3;
        public const short GETNUMRANGES = 4;
        // number of population density sites 
        public const short GETNUMSITES = 5;
        public const short GETSITESTATS = 6;
        // like GETNUMOBJ, but returns the number of objects to a Long variable 
        public const short GETNUMOBJEX = 7;
        public const short GETMEASENABLED = 8;
        // add to BLOB_ constants in IpBlbGet or MEAS_ constants in IpMeasGet to inquire current setting values 
        public const short GETIPPSETTING = 1000;

        // statistics for IpBlbData() Measure parameter 
        // returns an array of 4 Singles (mean, sum, background and total, in that order) 
        public const short BPOP_OBJECTS_STATS = 3000;
        // returns an array of 4 Singles (mean, sum, background and total, in that order) 
        public const short BPOP_AREA_STATS = 3001;
        // returns an array of 4 Singles (mean, sum, background and total, in that order) 
        public const short BPOP_DENSITY_STATS = 3002;
        // returns an array of 4 Singles (mean, sum, background and total, in that order) 
        public const short BPOP_CORRDENSITY_STATS = 3003;
        // returns an array of 6 singles (original count, cluster count, single object count, objects in clusters, total objects, and typical object area, in that order) 
        public const short BCLUSTER_STATS = 3005;

        // flags for IpBlbGet() param2 
        public const int BLB_ALLOBJECTS = 0x0;
        // if set, GETNUMOBJ returns only in-range objects, if not set returns all objects 
        public const int BLB_INRANGE = 0x1;
        // if set, GETNUMOBJ returns only the number in the current range (see IpBlbRange) 
        public const int BLB_ACTIVERANGE = 0x2;

        // for IpWsCreateFromVri() 
        public const short VRI_SHARE = 0;
        public const short VRI_COPY = 1;
        public const short VRI_NODELETE = 2;

        // for IpMacroStop() and IpMacroPause() 
        public const int MS_MODAL = 0x1;
        public const int MS_YESNO = 0x10;
        public const int MS_OKCAN = 0x20;
        public const int MS_YESNOCAN = 0x40;
        public const int MS_STOP = 0x100;
        public const int MS_EXCLAM = 0x200;
        public const int MS_QUEST = 0x400;
        public const int MS_DEF2 = 0x800;
        public const int MS_DEF3 = 0x1000;

        // only for use with IpMacroPause() - do not combine 
        public const int MP_WAITFORRESPONSE = 0x2000;
        // respect current setting (see IpAppGet/Set MACRO_PAUSE_TYPE) 
        public const int MP_RESPECTSETTING = 0x4000;
        // the default action if not specified 
        public const int MP_PAUSEANDCONTINUE = 0x0;

        // for IpBitAttr() 
        public const short BIT_SAMPLE = 1;
        public const short BIT_SAVEALL = 2;
        public const short BIT_CALIB = 3;

        // for IpSortAttr() 
        public const short SORT_ROTATE = 1;
        public const short SORT_MEAS = 2;
        public const short SORT_LABELS = 3;
        public const short SORT_COLOR = 4;
        public const short SORT_INDEX = 5;
        public const short SORT_AUTO = 6;

        // for IpRegister() 
        public const int AFF_CLIP = 0x1;
        public const int AFF_AOI = 0x2;
        public const int AFF_NOBILINEAR = 0x4;
        public const int AFF_FLOAT = 0x8;
        public const int AFF_NOTILT = 0x10;
        public const int AFF_NOSCALE = 0x20;

        // for IpAoiGet() 
        public const short AOI_BOX = 1;
        public const short AOI_ELLIPSE = 3;
        public const short AOI_POLYGON = 5;
        public const short AOIMGR_GET_NUM = 201;
        public const short AOIMGR_GET_NAME = 202;

        // used by various query functions 
        public const short GETNUMPTS = 100;
        public const short GETBOUNDS = 101;
        public const short GETPOINTS = 102;
        public const short GETSTATS = 103;
        public const short GETVALUES = 104;
        public const short GETRANGE = 105;
        public const short IPGETTTYPE = 106;
        public const short GETSTATUS = 107;
        public const short GETLABEL = 108;
        public const short GETINDEX = 109;
        public const short GETHIT = 110;
        public const short GETNUMCLASS = 111;
        public const short GETNUMSAMPLES = 112;
        public const short GETLNUMPTS = 113;
        public const short GETCHANNELS = 114;
        public const short GETRANGESTATS = 115;

        // for IpTrackBar() 
        public const short TBOPEN = 1;
        public const short TBUPDATE = 2;
        public const short TBCLOSE = 3;

        // for IpDocGetLine(), IpDocGetArea() 
        public const int CPROG = 0x2000;
        public const int USEAOI = 0x1000;

        // for IpDocGet() 
        public struct IPDOCINFO
        {
            public short Width;
            public short Height;
            public short iClass;
            public short Bpp;
            public RECT Extent;
        }

        // for IpAcqShow() 
        public const short ACQ_SNAP = 1;
        public const short ACQ_AVG = 2;
        public const short ACQ_TIMED = 3;
        public const short ACQ_MULTI = 4;
        public const short ACQ_LIVE = 5;
        public const short ACQ_ISLIVE = 6;
        public const short ACQ_ISSHOWN = 7;
        public const short ACQ_SETUP = 8;
        public const short ACQ_SETTINGS = 9;
        public const short ACQ_MACROS = 10;
        public const short ACQ_ISINITIALIZED = 11;
        public const short ACQ_BASIC = 12;
        public const short ACQ_BASICEX = 13;

        // for IpAcqSettings() 
        public const short ACQ_LOAD = 0;
        public const short ACQ_SAVE = 1;
        public const short ACQ_GETCURRENT = 2;

        // for IpIniFile() 
        public const short GETINT = 1;
        public const short GETFLOAT = 2;
        public const short GETSTRING = 3;
        public const short SETINT = 5;
        public const short SETFLOAT = 6;
        public const short SETSTRING = 7;

        // for IpCalSaveEx() 
        public const short NONAME = 1;
        public const short NOSYSTEM = 2;

        // for IpSCalShow() and IpSCalShowEx() 
        // hide the first spatial calibration dialog that is found to be open 
        public const short SCAL_HIDE = 0;
        // show the main spatial calibration dialog 
        public const short SCAL_DLG_MAIN = 1;
        // show the spatial calibration selection dialog (only valid if an image is open) 
        public const short SCAL_DLG_SELECT = 2;
        // add a marker 
        public const short SCAL_ADD_MARKER = 3;
        // minimize the main spatial calibration dialog 
        public const short SCAL_MINIMIZE = 4;
        // show the spatial calibration wizard 
        public const short SCAL_DLG_WIZARD = 5;
        // show the system calibration toolbar 
        public const short SCAL_DLG_SYSTEM = 6;
        // for use with IpSCalShowEx only 
        public const short SCAL_SHOW = 7;
        // for use with IpSCalShowEx only 
        public const short SCAL_HIDEALL = 8;

        // constants for IpSCal functions 
        // use as Calibration to get/set/save the current calibration's attributes 
        public const short SCAL_CURRENT_CAL = -1;
        // use as Calibration to get/set/save the current system calibration's attributes 
        public const short SCAL_SYSTEM_CAL = -2;
        // use as Calibration to save all in list 
        public const short SCAL_ALL = -3;
        // use as Calibration to save all in reference list or remove all from reference list 
        public const short SCAL_ALL_REF = -4;

        // for IpSCalGetLong() and IpSCalSetLong() 
        // read-only (not supported by IpSCalSetLong) 
        public const short SCAL_NUM_ALL = 0;
        // read-only 
        public const short SCAL_NUM_REF = 1;
        // read-only 
        public const short SCAL_GET_ALL = 2;
        // read-only 
        public const short SCAL_GET_REF = 3;
        // read-only 
        public const short SCAL_IS_REFERENCE = 4;
        // read-only 
        public const short SCAL_IS_SYSTEM = 5;

        public const short SCAL_ONIMAGE_COLOR = 10;
        public const short SCAL_CURRENT = 11;
        public const short SCAL_SYSTEM = 12;
        public const short SCAL_MARKER_STYLE = 13;
        // affects the list displayed in the main calibration dialog 
        public const short SCAL_SHOW_REF_ONLY = 14;
        // corresponding to the "convert when change units" control, determines whether to convert SCALE_X and SCALE_Y when change UNITS (won't convert if not absolute units) 
        public const short SCAL_UNIT_CONVERT = 15;
        public const short SCAL_MARKER_FONT_SIZE = 16;
        public const short SCAL_MARKER_FONT_WEIGHT = 17;
        public const short SCAL_MARKER_LINE_WIDTH = 18;
        public const short SCAL_MARKER_LINE_ENDS = 19;

        // write-only (not supported by IpSCalGetLong) 
        public const short SCAL_ADD_TO_REF = 30;
        // write-only 
        public const short SCAL_REMOVE_FROM_REF = 31;
        // write-only, applies the specified calibration to the active image 
        public const short SCAL_APPLY = 32;
        // write-only, creates a calibration from the image resolution and applies it to the active image 
        public const short SCAL_APPLY_RESOLUTION = 33;

        // for IpSCalGetSng() and IpSCalSetSng() 
        // read-only - set by ratio of SCALE_X / SCALE_Y 
        public const short SCAL_ASPECT = 50;
        // read-only - conversion factor to millimeeters 
        public const short SCAL_CONVERSION_TO_MM = 51;
        public const short SCAL_SCALE_X = 60;
        public const short SCAL_SCALE_Y = 61;
        public const short SCAL_ORIGIN_X = 62;
        public const short SCAL_ORIGIN_Y = 63;
        public const short SCAL_ANGLE = 64;
        // use this to adjust the system calibration for the effects of a magnification 
        public const short SCAL_SYSTEM_MODIFIER = 65;
        // changer or anything else that affects the overall magnification of the optics 
        public const short SCAL_MARKER_WIDTH = 66;

        // for IpSCalGetStr() and IpSCalSetStr() 
        public const short SCAL_NAME = 100;
        public const short SCAL_UNITS = 101;
        // read-only - returns calibration ID if one exists with the specified name 
        public const short SCAL_FIND_BY_NAME = 102;
        // Note: the Calibration parameter must be SCAL_ALL or SCAL_ALL_REF 
        public const short SCAL_MARKER_FONT_FACE = 103;

        // marker styles for use with IpSCalGetLong and IpSCalSetLong SCAL_MARKER_STYLE command 
        // black text on white background, no border 
        public const short SCAL_MARKER_BONW = 0;
        // black text on white background, with border 
        public const short SCAL_MARKER_BONWB = 1;
        // white text on black background, no border 
        public const short SCAL_MARKER_WONB = 2;
        // white text on black background, with border 
        public const short SCAL_MARKER_WONBB = 3;
        // non-destructive X axis only 
        public const short SCAL_MARKER_ND_X = 4;
        // non-destructive both axes 
        public const short SCAL_MARKER_ND_XY = 5;
        // non-destructive Y axis only 
        public const short SCAL_MARKER_ND_Y = 6;

        // for IpICalShow() 
        public const short ICAL_HIDE = 0;
        public const short ICAL_SHOW = 1;
        public const short ICAL_MINIMIZE = 4;

        // constants for IpICal functions 
        // use as Calibration to get/set/save the current calibration's attributes 
        public const short ICAL_CURRENT_CAL = -1;
        // Note: A constant cannot be provided to represent the current system calibration because there are multiple system intensity 
        // calibrations, one per image class. Use IpICalGetSystem to get the system calibration for a particular image class. 
        // use as Calibration to save all in list 
        public const short ICAL_ALL = -2;
        // use as Calibration to save all in reference list 
        public const short ICAL_ALL_REF = -3;

        // for IpICalGetLong() and IpICalSetLong() 
        // read-only (not supported by IpICalSetLong) 
        public const short ICAL_NUM_ALL = 0;
        // read-only 
        public const short ICAL_NUM_REF = 1;
        // read-only 
        public const short ICAL_GET_ALL = 2;
        // read-only 
        public const short ICAL_GET_REF = 3;
        // read-only - set samples using calibration type and/or IpICalSetPoints/IpICalSetPointsEx 
        public const short ICAL_NUM_SAMPLES = 5;
        // read-only 
        public const short ICAL_IS_REFERENCE = 6;
        public const short ICAL_CURRENT = 10;
        // affects the list displayed in the main calibration dialog 
        public const short ICAL_SHOW_REF_ONLY = 11;
        // write-only (not supported by IpICalGetLong) 
        public const short ICAL_ADD_TO_REF = 20;
        // write-only 
        public const short ICAL_REMOVE_FROM_REF = 21;
        // write-only, applies the specified calibration to the active image 
        public const short ICAL_APPLY = 22;

        // for IpICalGetSng() and IpICalSetSng() 
        // black level of optical density calibration 
        public const short ICAL_OD_BLACK = 10;
        // incident (white) level of optical density calibration 
        public const short ICAL_OD_INCIDENT = 11;

        // for IpICalGetStr() and IpICalSetStr() 
        public const short ICAL_NAME = 30;
        public const short ICAL_UNITS = 31;

        // for IpAnotAttr() 
        public const short DRAW_FILLCOLOR = 0;
        public const short DRAW_LINECOLOR = 1;
        public const short DRAW_LINEWIDTH = 2;
        public const short DRAW_THINLINE = 0;
        public const short DRAW_THICKLINE = 1;

        // for IpFltLocHistEq() 
        public const short LOCEQ_LINEAR = 1;
        public const short LOCEQ_BELL = 2;
        public const short LOCEQ_LOG = 3;
        public const short LOCEQ_EXP = 4;
        public const short LOCEQ_BESTFIT = 5;
        public const short LOCEQ_STDDEV = 6;

        // for IpFltDistance() 
        public const short DISTANCE_SQUARE = 0;
        public const short DISTANCE_DIAGONAL = 1;
        public const short DISTANCE_EUCLIDEAN = 2;

        // for IpFltReduce() 
        public const short REDUCE_4NEIGHBOR = 2;
        public const short REDUCE_8NEIGHBOR = 0;
        public const short REDUCE_16NEIGHBOR = 4;

        // for IpFltBranchEnd() 
        public const short BR_END = 32;
        public const short BR_SKEL = 16;
        public const short BR_BRANCH3 = 64;
        public const short BR_BRANCHN = 128;

        // for IpAnotLine() 
        public const short DRAW_PLAINLINE = 0;
        public const short DRAW_SMALLARROWRIGHT = 1;
        public const short DRAW_SMALLARROWBOTH = 2;
        public const short DRAW_SMALLARROWLEFT = 3;
        public const short DRAW_LARGEARROWRIGHT = 4;
        public const short DRAW_LARGEARROWLEFT = 5;
        public const short DRAW_LARGEARROWBOTH = 6;
        public const short DRAW_CIRCLEARROW = 7;
        public const short DRAW_ARROWCIRCLE = 8;
        public const short DRAW_DIAMONDBOTH = 9;
        public const short DRAW_CIRCLEBOTH = 10;

        // Plot and overlay definitions 
        public const short PDT_INT16 = 0;
        public const short PDT_WORD16 = 1;
        public const short PDT_INT32 = 2;
        public const short PDT_WORD32 = 3;
        public const short PDT_FLOAT = 4;
        public const short PDT_DFLOAT = 5;

        //#define ATT_FIXED 0x8000 
        public const short ATT_FIXED = -32768;
        public const int ATT_NOCOPY = 0x4000;
        public const int ATT_FIXEDX = 0x2000;
        public const int ATT_FIXEDY = 0x1000;
        public const int ATT_CONTROLS = 0x800;
        public const int ATT_CALIPER = 0x400;

        public const short SETHWNDMESSAGE = 1;
        public const short GETEDITPOINT = 1;
        public const short GETCURPOS = 2;
        public const short RECTANGLE = -10;

        // constants for IpPlot... functions 

        public const short XAXIS = 0;
        public const short YAXIS = 1;

        public const short GETGRAPH = 1;
        public const short GETHWND = 2;
        public const short SETPARENT = 3;
        public const short SETNOTIFY = 4;

        public const short RGE_FIXED = 0;
        public const short RGE_AUTO = 1;
        public const short RGE_FIXEDMIN = 2;
        public const short RGE_FIXEDMAX = 3;


        // for IpWsConvertImage() - conversion methods 
        public const short CONV_SCALE = 0;
        public const short CONV_SHIFT = 1;
        public const short CONV_DIRECT = 2;
        public const short CONV_USER = 3;
        public const short CONV_MCOLOR = 4;
        public const short CONV_MEDIAN = 5;
        public const short CONV_PSEUDOCOLOR = 6;

        // for IpDocGetPosition() 
        public struct IPDOCPOS
        {
            public short IsKnown;
            public float Position;
        }

        // constants for frame selection for the IpDocGetProp and IpDocSetProp routines 
        // can be used with IpDocGetProp and IpDocSetProp functions, as well as many other frame-related functions 
        public const short DOC_ACTIVEFRAME = -1;
        // for use only with IpDocSetProp functions 
        public const short DOC_ACTIVEPORTION = -2;
        // for use only with IpDocSetProp functions 
        public const short DOC_ENTIREIMAGE = -3;
        // for IpDocGetPropDbl() and IpDocSetPropDbl() 
        public const short DOCPROP_XPOSITION = 0;
        public const short DOCPROP_YPOSITION = 1;
        public const short DOCPROP_ZPOSITION = 2;
        public const short DOCPROP_EMWAVELENGTH = 3;
        public const short DOCPROP_EXWAVELENGTH = 4;
        public const short DOCPROP_REFINDEX = 5;
        public const short DOCPROP_NUMAPERTURE = 6;
        public const short DOCPROP_MAGNIFICATION = 7;
        public const short DOCPROP_EXPOSURE = 8;
        public const short DOCPROP_GAIN = 9;
        public const short DOCPROP_OFFSET = 50;
        public const short DOCPROP_GAMMA = 51;
        public const short DOCPROP_WHITEBAL_R = 52;
        public const short DOCPROP_WHITEBAL_G = 53;
        public const short DOCPROP_WHITEBAL_B = 54;
        public const short DOCPROP_SUGG_WHITEBAL_R = 55;
        public const short DOCPROP_SUGG_WHITEBAL_G = 56;
        public const short DOCPROP_SUGG_WHITEBAL_B = 57;

        // for IpDocGetPropStr() and IpDocSetPropStr() 
        public const short DOCPROP_CHANNELNAME = 10;
        public const short DOCPROP_SITELABEL = 11;
        public const short DOCPROP_CAPTDRIVERNAME = 12;
        public const short DOCPROP_CAPTCAMERANAME = 13;
        public const short DOCPROP_CAPTCAMERAID = 14;
        // Note: This string may be up to 512 characters long, if present 
        public const short DOCPROP_CAPTDRIVERFEATURES = 15;
        public const short DOCPROP_CAPTDRIVERVERSION = 16;
        public const short DOCPROP_TIMEPHASELABEL = 17;

        // for IpDocGetPropDate() and IpDocSetPropDate() 
        // These can be also used with IpDocGetPropDbl and IpDocGetPropDbl for more accuracy, and also for calculating deltas 
        public const short DOCPROP_TIME = 20;
        public const short DOCPROP_TIMEPOINT = 21;

        // for IpDocGetPropLong() and IpDocSetPropLong() 
        public const short DOCPROP_BIN_X = 30;
        public const short DOCPROP_BIN_Y = 31;
        public const short DOCPROP_CAPTRECT_L = 32;
        public const short DOCPROP_CAPTRECT_R = 33;
        public const short DOCPROP_CAPTRECT_T = 34;
        public const short DOCPROP_CAPTRECT_B = 35;
        public const short DOCPROP_CHIPCOORD_L = 36;
        public const short DOCPROP_CHIPCOORD_R = 37;
        public const short DOCPROP_CHIPCOORD_T = 38;
        public const short DOCPROP_CHIPCOORD_B = 39;
        public const short DOCPROP_NATIVE_BITDEPTH = 40;
        // Typically, an image displays with the current tint or pseudo-coloring. This property allows you to shut off tint/pseudocolor display, or turn it back on, without disturbing the tint and pseudocoloring that's been set up. 
        public const short DOCPROP_DISPLAY_TINT = 41;

        // for IpPcTint() 
        public const short TINT_REMOVE = 0;
        public const short TINT_RED = 1;
        public const short TINT_GREEN = 2;
        public const short TINT_BLUE = 3;

        // for IpToolbarGetStr 
        // returns the full path name of the current toolbar 
        public const short IPTB_TOOLBAR = 1;

        // constants used for several Apply functions 
        public const short APPLYTO_FRAME = 1;
        public const short APPLYTO_IMAGE = 2;
        public const short APPLYTO_PORTION = 3;
        public const short APPLYTO_SET = 4;
        public const short APPLYTO_ZSTACK = 5;
        public const short APPLYTO_CHANNEL = 6;
        public const short APPLYTO_TIMEPT = 7;
        public const short APPLYTO_ALLTIMEPTS = 7;

        // for IpTagGetStr 
        public const short TAG_CLASSNAME = 200;

        // color definitions for IpMeasAttr (MEAS_DISPCOLOR, MEAS_LABELCOLOR, and MEAS_MEASCOLOR) 
        // and for IpSmSet (SM_TIMESTAMPCOLOR) 
        public const short DISPCOLOR_RED = 0;
        public const short DISPCOLOR_GREEN = 1;
        public const short DISPCOLOR_BLUE = 2;
        public const short DISPCOLOR_YELLOW = 3;
        public const short DISPCOLOR_CYAN = 4;
        public const short DISPCOLOR_MAGENTA = 5;
        public const short DISPCOLOR_WHITE = 6;
        public const short DISPCOLOR_BLACK = 7;

        // defines for use with IpWsSelectFrames 
        // use in Start parameter to select active frame 
        public const short FRAMESEL_ACTIVE = -1;
        // use with zero or start frame number to indicate "to end of sequence" 
        public const short FRAMESEL_ALL = -1;
        // use with Start parameter set to FRAMESEL_ACTIVE to select the portion of sequence containing the current Z stack 
        public const short FRAMESEL_ZSTACK = -2;

        //----------------------------------------------------------------- 
        // Error codes returned by Auto-Pro functions 
        // 
        // All error codes are negative values. 
        // IPCERR_NONE (0) indicates no error. 
        // Positive values may be used by certain functions to return results. 
        //----------------------------------------------------------------- 
        // no error calling the function 
        public const short IPCERR_NONE = 0;

        // Image-Pro is not running 
        public const short IPCERR_APPINACTIVE = -1;
        public const short IPCERR_NOTFOUND = -2;
        // Can't find DLL that implements the function 
        public const short IPCERR_DLLNOTFOUND = -3;
        // Can't find the function in the DLL 
        public const short IPCERR_FUNCNOTFOUND = -4;
        // Not applicable to the current image/situation 
        public const short IPCERR_INVCOMMAND = -5;
        // There is no active workspace 
        public const short IPCERR_NODOC = -6;
        // Invalid command arguments 
        public const short IPCERR_INVARG = -7;
        // Insufficient memory 
        public const short IPCERR_MEMORY = -8;
        // Image-Pro is busy executing another function 
        public const short IPCERR_BUSY = -9;
        // The requested information is not present 
        public const short IPCERR_EMPTY = -10;
        // An argument was out of range, may have been executed within the valid limits 
        public const short IPCERR_LIMIT = -11;
        // Operation cancelled by user 
        public const short IPCERR_CANCELLED = -12;
        // Not really an error, but the file cannot be opened as a set, and has been opened as a single image workspace instead 
        public const short IPCERR_NOTASET = -13;
        // Scope-Pro hardware init problem 
        public const short IPCERR_SCP_HW_NOT_INITIALIZED = -14;
        // Stage-Pro hardware init problem 
        public const short IPCERR_STG_HW_NOT_INITIALIZED = -15;
        // Stage-Pro hardware init problem 
        public const short IPCERR_STG_WELL_ORIGIN_NOT_INITIALIZED = -16;

        public const short IPCERR_FUNC = -1000;

        // common enumerations 
        public enum IPCOLORMODELS
        {
            IPCM_RGB = Ipc32v5.CM_RGB,
            IPCM_HSI = Ipc32v5.CM_HSI,
            IPCM_HSV = Ipc32v5.CM_HSV,
            IPCM_YIQ = Ipc32v5.CM_YIQ
        }

        // can be used with IpWsConvertFile to convert to the closest possible image class 
        public const short PRESERVE_IMAGE_CLASS = -1;
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Video acquisition 
        //----------------------------------------------------------------- 
        public static extern int IpAcqAverage(short Frames, short Divider);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAcqSnap(short dest);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAcqTimed(string szDir, string szPrefix, short StartNumber, short Frames, int Interval);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAcqShow(short wDialog, short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAcqMultiSnap(short startframe, short NumFrames, short dest);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpAcqControl(short Cmd, short param, ref int lpParam);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAcqSettings(string lpszFile, short bSave);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAcqSelectDriver(string lpszFile, short bGetDriv);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Aoi manager functions 
        //----------------------------------------------------------------- 
        //UPGRADE_WARNING: 構造体 RECT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAoiCreateBox(ref Winapi.RECT ipRect);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 RECT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAoiCreateEllipse(ref Winapi.RECT ipRect);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 POINTAPI に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAoiCreateIrregular(ref POINTAPI ipAoiPoint, short numpoints);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 RECT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAoiCreateDonut(ref Winapi.RECT ipRect, short Thickness);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAoiShow(short FrameType);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAoiManager(short FuncID, string Name);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAoiChangeName(string oldName, string newName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAoiMove(short deltax, short deltay);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAoiValidate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpAoiGet(short Command_Renamed, short wParam, ref int lpParam);
        [DllImport("IPC32", EntryPoint = "IpAoiGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpAoiGetStr(short Command_Renamed, short wParam, string lpParam);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAoiMultAppend(short append);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAoiMultShow(short yesNo);

        //----------------------------------------------------------------- 
        // Application functions 
        //----------------------------------------------------------------- 

        // commands for IpAppArrange 
        public enum APPARR_ATTRIBUTES
        {
            APPARR_DOCS_CASCADE = Ipc32v5.DOCS_CASCADE,
            APPARR_DOCS_TILE = Ipc32v5.DOCS_TILE,
            APPARR_DOCS_OVERLAP = Ipc32v5.DOCS_OVERLAP
        }

        // commands for IpAppGet 
        public enum APPGET_ATTRIBUTES
        {
            APPGET_GETAPPWND = Ipc32v5.GETAPPWND,
            APPGET_GETPLUGSN = Ipc32v5.GETPLUGSN,
            APPGET_BLEND_PREVIEW = Ipc32v5.PST_BLEND_PREVIEW,
            APPGET_BLEND_APPLY = Ipc32v5.PST_BLEND_APPLY,
            APPGET_APPLY_TYPE = Ipc32v5.PST_APPLY_TYPE,
            APPGET_BLEND_SOURCE = Ipc32v5.PST_BLEND_SOURCE,
            APPGET_MACRO_PAUSE_TYPE = Ipc32v5.MACRO_PAUSE_TYPE,
            APPGET_WINDOW_TILING_TYPE = Ipc32v5.WINDOW_TILING_TYPE,
            APPGET_OPEN_AS_SEQ = Ipc32v5.FILE_OPEN_AS_SEQ,
            APPGET_SAVE_AS_SERIES = Ipc32v5.FILE_SAVE_AS_SERIES,
            APPGET_PROMPT_WHEN_MODIFIED = Ipc32v5.PROMPT_WHEN_MODIFIED,
            APPGET_PROMPT_FOR_MENU_SELECT = Ipc32v5.PROMPT_FOR_MENU_SELECT,
            APPGET_PROMPT_FOR_SEQUENCES = Ipc32v5.PROMPT_FOR_SEQUENCES
        }

        // commands for IpAppGetStr 
        public enum APPGETSTR_ATTRIBUTES
        {
            APPGETSTR_GETAPPDIR = Ipc32v5.GETAPPDIR,
            APPGETSTR_GETAPPNAME = Ipc32v5.GETAPPNAME,
            APPGETSTR_GETAPPVERSION = Ipc32v5.GETAPPVERSION,
            APPGETSTR_GETOSVERSION = Ipc32v5.GETOSVERSION,
            APPGETSTR_GETAPPSETTINGSDIR = Ipc32v5.GETAPPSETTINGSDIR
        }

        // commands for IpAppSet 
        public enum APPSET_ATTRIBUTES
        {
            APPSET_BLEND_PREVIEW = Ipc32v5.PST_BLEND_PREVIEW,
            APPSET_BLEND_APPLY = Ipc32v5.PST_BLEND_APPLY,
            APPSET_APPLY_TYPE = Ipc32v5.PST_APPLY_TYPE,
            APPSET_BLEND_SOURCE = Ipc32v5.PST_BLEND_SOURCE,
            APPSET_MACRO_PAUSE_TYPE = Ipc32v5.MACRO_PAUSE_TYPE,
            APPSET_WINDOW_TILING_TYPE = Ipc32v5.WINDOW_TILING_TYPE,
            APPSET_OPEN_AS_SEQ = Ipc32v5.FILE_OPEN_AS_SEQ,
            APPSET_SAVE_AS_SERIES = Ipc32v5.FILE_SAVE_AS_SERIES,
            APPSET_PROMPT_WHEN_MODIFIED = Ipc32v5.PROMPT_WHEN_MODIFIED,
            APPSET_PROMPT_FOR_MENU_SELECT = Ipc32v5.PROMPT_FOR_MENU_SELECT,
            APPSET_PROMPT_FOR_SEQUENCES = Ipc32v5.PROMPT_FOR_SEQUENCES
        }
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //UPGRADE_WARNING: 構造体 APPARR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAppArrange(APPARR_ATTRIBUTES mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppCloseAll();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppExit();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppMaximize();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppMinimize();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppMove(short x, short y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppRestore();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppSelectDoc(short docId);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppSize(short Width, short Height);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppUpdateDoc(short docId);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppHide(short hide);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppRun(string CommandLine, short mode, short AutoClose);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 APPGET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAppGet(APPGET_ATTRIBUTES Command_Renamed, short wParam, ref int lpParam);
        [DllImport("IPC32", EntryPoint = "IpAppGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 APPGETSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAppGet2(APPGETSTR_ATTRIBUTES sCmd, short sParam, string lpParam);
        [DllImport("IPC32", EntryPoint = "IpAppGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for GETAPPNAME and GETAPPDIR 
        //UPGRADE_WARNING: 構造体 APPGETSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAppGetStr(APPGETSTR_ATTRIBUTES sCmd, short sParam, string lpParam);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppMenuSelect(short Id1, short Id2, string ItemName, short mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpAppWindow(string WindowName, ref int WindowParam, short mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpAppCtl(string CtlName, short ParamCmd, ref int lpParamValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 RECT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAppWndPos(string WindowName, ref Winapi.RECT lpRect, short mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpAppWndState(string WindowName, ref int theState, short mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppCtlText(string ControlName, string caption, short mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAppCloseTools();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 APPSET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAppSet(APPSET_ATTRIBUTES Command_Renamed, short Attr);

        //----------------------------------------------------------------- 
        // Count/Size functions 
        //----------------------------------------------------------------- 

        public const short BLBDISP_CLASS_OBJECTS = 1;
        public const short BLBDISP_CLASS_PERCENTOBJECTS = 2;
        public const short BLBDISP_CLASS_MEAN = 4;
        public const short BLBDISP_CLASS_RANGEMIN = 8;
        public const short BLBDISP_CLASS_RANGEMAX = 16;
        public const short BLBDISP_CLASS_STDDEV = 32;
        public const short BLBDISP_CLASS_SUM = 64;
        public const short BLBDISP_CLASS_PERCENT = 128;
        public const short BLBDISP_CLASS_BIN = 256;

        // attributes for IpBlbSetAttr 
        public enum BLBSET_ATTRIBUTES
        {
            BLBSET_OUTLINEMODE = Ipc32v5.BLOB_OUTLINEMODE,
            BLBSET_OUTLINECOLOR = Ipc32v5.BLOB_OUTLINECOLOR,
            BLBSET_LABELMODE = Ipc32v5.BLOB_LABELMODE,
            BLBSET_LABELCOLOR = Ipc32v5.BLOB_LABELCOLOR,
            BLBSET_SMOOTHING = Ipc32v5.BLOB_SMOOTHING,
            BLBSET_MINAREA = Ipc32v5.BLOB_MINAREA,
            BLBSET_FILLHOLES = Ipc32v5.BLOB_FILLHOLES,
            BLBSET_BRIGHTOBJ = Ipc32v5.BLOB_BRIGHTOBJ,
            BLBSET_AUTORANGE = Ipc32v5.BLOB_AUTORANGE,
            BLBSET_MEASUREOBJECTS = Ipc32v5.BLOB_MEASUREOBJECTS,
            BLBSET_FILTEROBJECTS = Ipc32v5.BLOB_FILTEROBJECTS,
            BLBSET_ADDCOUNT = Ipc32v5.BLOB_ADDCOUNT,
            BLBSET_CLEANBORDER = Ipc32v5.BLOB_CLEANBORDER,
            BLBSET_CONVEX = Ipc32v5.BLOB_CONVEX,
            BLBSET_8CONNECT = Ipc32v5.BLOB_8CONNECT,
            BLBSET_DISPLAY = Ipc32v5.BLOB_DISPLAY,
            BLBSET_LABELMEAS = Ipc32v5.BLOB_LABELMEAS,
            BLBSET_CLASS_DISPLAY = Ipc32v5.BLOB_CLASS_DISPLAY,
            BLBSET_RANGE_DISPLAY = Ipc32v5.BLOB_RANGE_DISPLAY,
            BLBSET_SUPPRESS_COLLECT = Ipc32v5.BLOB_SUPPRESS_COLLECT
        }

        // commands for IpBlbGet 
        public enum BLBGET_ATTRIBUTES
        {
            BLBGET_GETNUMOBJ = Ipc32v5.GETNUMOBJ,
            BLBGET_GETHBLOB = Ipc32v5.GETHBLOB,
            BLBGET_GETTHRESH = Ipc32v5.GETTHRESH,
            BLBGET_GETNUMRANGES = Ipc32v5.GETNUMRANGES,
            BLBGET_GETNUMSITES = Ipc32v5.GETNUMSITES,
            BLBGET_GETSITESTATS = Ipc32v5.GETSITESTATS,
            BLBGET_GETNUMOBJEX = Ipc32v5.GETNUMOBJEX,
            BLBGET_GETMEASENABLED = Ipc32v5.GETMEASENABLED,
            BLBGET_GETNUMPTS = Ipc32v5.GETNUMPTS,
            BLBGET_GETBOUNDS = Ipc32v5.GETBOUNDS,
            BLBGET_GETPOINTS = Ipc32v5.GETPOINTS,
            BLBGET_GETSTATS = Ipc32v5.GETSTATS,
            BLBGET_GETRANGE = Ipc32v5.GETRANGE,
            BLBGET_GETSTATUS = Ipc32v5.GETSTATUS,
            BLBGET_GETHIT = Ipc32v5.GETHIT,
            BLBGET_GETNUMSAMPLES = Ipc32v5.GETNUMSAMPLES,
            BLBGET_GETRANGESTATS = Ipc32v5.GETRANGESTATS,
            BLBGET_OUTLINEMODE = Ipc32v5.BLOB_OUTLINEMODE + Ipc32v5.GETIPPSETTING,
            BLBGET_OUTLINECOLOR = Ipc32v5.BLOB_OUTLINECOLOR + Ipc32v5.GETIPPSETTING,
            BLBGET_LABELMODE = Ipc32v5.BLOB_LABELMODE + Ipc32v5.GETIPPSETTING,
            BLBGET_LABELCOLOR = Ipc32v5.BLOB_LABELCOLOR + Ipc32v5.GETIPPSETTING,
            BLBGET_SMOOTHING = Ipc32v5.BLOB_SMOOTHING + Ipc32v5.GETIPPSETTING,
            BLBGET_MINAREA = Ipc32v5.BLOB_MINAREA + Ipc32v5.GETIPPSETTING,
            BLBGET_FILLHOLES = Ipc32v5.BLOB_FILLHOLES + Ipc32v5.GETIPPSETTING,
            BLBGET_BRIGHTOBJ = Ipc32v5.BLOB_BRIGHTOBJ + Ipc32v5.GETIPPSETTING,
            BLBGET_AUTORANGE = Ipc32v5.BLOB_AUTORANGE + Ipc32v5.GETIPPSETTING,
            BLBGET_MEASUREOBJECTS = Ipc32v5.BLOB_MEASUREOBJECTS + Ipc32v5.GETIPPSETTING,
            BLBGET_FILTEROBJECTS = Ipc32v5.BLOB_FILTEROBJECTS + Ipc32v5.GETIPPSETTING,
            BLBGET_ADDCOUNT = Ipc32v5.BLOB_ADDCOUNT + Ipc32v5.GETIPPSETTING,
            BLBGET_CLEANBORDER = Ipc32v5.BLOB_CLEANBORDER + Ipc32v5.GETIPPSETTING,
            BLBGET_CONVEX = Ipc32v5.BLOB_CONVEX + Ipc32v5.GETIPPSETTING,
            BLBGET_8CONNECT = Ipc32v5.BLOB_8CONNECT + Ipc32v5.GETIPPSETTING,
            BLBGET_DISPLAY = Ipc32v5.BLOB_DISPLAY + Ipc32v5.GETIPPSETTING,
            BLBGET_LABELMEAS = Ipc32v5.BLOB_LABELMEAS + Ipc32v5.GETIPPSETTING,
            BLBGET_CLASS_DISPLAY = Ipc32v5.BLOB_CLASS_DISPLAY + Ipc32v5.GETIPPSETTING,
            BLBGET_RANGE_DISPLAY = Ipc32v5.BLOB_RANGE_DISPLAY + Ipc32v5.GETIPPSETTING,
            BLBGET_SUPPRESS_COLLECT = Ipc32v5.BLOB_SUPPRESS_COLLECT + Ipc32v5.GETIPPSETTING
        }

        // commands for IpBlbGetStr 
        public enum BLBGETSTR_ATTRIBUTES
        {
            BLBGET_GETLABEL = Ipc32v5.GETLABEL
        }

        // measurements types, for IpBlbData, IpBlbEnableMeas and IpBlbSetFilterRange 
        public enum BLB_MEASUREMENTS
        {
            BLBMEAS_ALL = -1,
            BLBMEAS_AREA = Ipc32v5.BLBM_AREA,
            BLBMEAS_ASPECT = Ipc32v5.BLBM_ASPECT,
            BLBMEAS_BOX_AREA = Ipc32v5.BLBM_BOX_AREA,
            BLBMEAS_BOX_XY = Ipc32v5.BLBM_BOX_XY,
            BLBMEAS_CENTRX = Ipc32v5.BLBM_CENTRX,
            BLBMEAS_CENTRY = Ipc32v5.BLBM_CENTRY,
            BLBMEAS_DENSITY = Ipc32v5.BLBM_DENSITY,
            BLBMEAS_DIRECTION = Ipc32v5.BLBM_DIRECTION,
            BLBMEAS_HOLEAREA = Ipc32v5.BLBM_HOLEAREA,
            BLBMEAS_HOLEAREARATIO = Ipc32v5.BLBM_HOLEAREARATIO,
            BLBMEAS_MAJORAX = Ipc32v5.BLBM_MAJORAX,
            BLBMEAS_MINORAX = Ipc32v5.BLBM_MINORAX,
            BLBMEAS_MAXFERRET = Ipc32v5.BLBM_MAXFERRET,
            BLBMEAS_MINFERRET = Ipc32v5.BLBM_MINFERRET,
            BLBMEAS_MEANFERRET = Ipc32v5.BLBM_MEANFERRET,
            BLBMEAS_MAXRADIUS = Ipc32v5.BLBM_MAXRADIUS,
            BLBMEAS_MINRADIUS = Ipc32v5.BLBM_MINRADIUS,
            BLBMEAS_NUMHOLES = Ipc32v5.BLBM_NUMHOLES,
            BLBMEAS_PERIMETER = Ipc32v5.BLBM_PERIMETER,
            BLBMEAS_RADIUSRATIO = Ipc32v5.BLBM_RADIUSRATIO,
            BLBMEAS_ROUNDNESS = Ipc32v5.BLBM_ROUNDNESS,
            BLBMEAS_CLUSTER = Ipc32v5.BLBM_CLUSTER,
            BLBMEAS_RED = Ipc32v5.BLBM_RED,
            BLBMEAS_GREEN = Ipc32v5.BLBM_GREEN,
            BLBMEAS_BLUE = Ipc32v5.BLBM_BLUE,
            BLBMEAS_PERAREA = Ipc32v5.BLBM_PERAREA,
            BLBMEAS_CLASS = Ipc32v5.BLBM_CLASS,
            BLBMEAS_LENGTH = Ipc32v5.BLBM_LENGTH,
            BLBMEAS_WIDTH = Ipc32v5.BLBM_WIDTH,
            BLBMEAS_PERIMETER2 = Ipc32v5.BLBM_PERIMETER2,
            BLBMEAS_IOD = Ipc32v5.BLBM_IOD,
            BLBMEAS_PCONVEX = Ipc32v5.BLBM_PCONVEX,
            BLBMEAS_PELLIPSE = Ipc32v5.BLBM_PELLIPSE,
            BLBMEAS_PRATIO = Ipc32v5.BLBM_PRATIO,
            BLBMEAS_AREAPOLY = Ipc32v5.BLBM_AREAPOLY,
            BLBMEAS_FRACTDIM = Ipc32v5.BLBM_FRACTDIM,
            BLBMEAS_CMASSX = Ipc32v5.BLBM_CMASSX,
            BLBMEAS_CMASSY = Ipc32v5.BLBM_CMASSY,
            BLBMEAS_SIZECOUNT = Ipc32v5.BLBM_SIZECOUNT,
            BLBMEAS_BOXX = Ipc32v5.BLBM_BOXX,
            BLBMEAS_BOXY = Ipc32v5.BLBM_BOXY,
            BLBMEAS_MINCALIP = Ipc32v5.BLBM_MINCALIP,
            BLBMEAS_MAXCALIP = Ipc32v5.BLBM_MAXCALIP,
            BLBMEAS_MEANCALIP = Ipc32v5.BLBM_MEANCALIP,
            BLBMEAS_DENSMIN = Ipc32v5.BLBM_DENSMIN,
            BLBMEAS_DENSMAX = Ipc32v5.BLBM_DENSMAX,
            BLBMEAS_DENSDEV = Ipc32v5.BLBM_DENSDEV,
            BLBMEAS_BRANCHLEN = Ipc32v5.BLBM_BRANCHLEN,
            BLBMEAS_DENDRITES = Ipc32v5.BLBM_DENDRITES,
            BLBMEAS_ENDPOINTS = Ipc32v5.BLBM_ENDPOINTS,
            BLBMEAS_MARGINATION = Ipc32v5.BLBM_MARGINATION,
            BLBMEAS_HETEROGENEITY = Ipc32v5.BLBM_HETEROGENEITY,
            BLBMEAS_CLUMPINESS = Ipc32v5.BLBM_CLUMPINESS,
            BLBMEAS_DENSSUM = Ipc32v5.BLBM_DENSSUM,
            BLBMEAS_SRANGE = Ipc32v5.BLBM_SRANGE,
            BLBMEAS_PERIMETER3 = Ipc32v5.BLBM_PERIMETER3,
            BLBMEASEX_RADIUS = Ipc32v5.BLEX_RADIUS,
            BLBMEASEX_DIAMETER = Ipc32v5.BLEX_DIAMETER,
            BLBMEASEX_CALIPER = Ipc32v5.BLEX_CALIPER,
            BLBMEASEX_BRANCHLEN = Ipc32v5.BLEX_BRANCHLEN,
            BLBMEAS_BPOP_OBJECTS_STATS = Ipc32v5.BPOP_OBJECTS_STATS,
            BLBMEAS_BPOP_AREA_STATS = Ipc32v5.BPOP_AREA_STATS,
            BLBMEAS_BPOP_DENSITY_STATS = Ipc32v5.BPOP_DENSITY_STATS,
            BLBMEAS_BPOP_CORRDENSITY_STATS = Ipc32v5.BPOP_CORRDENSITY_STATS,
            BLBMEAS_BCLUSTER_STATS = Ipc32v5.BCLUSTER_STATS
        }
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpBlbCount();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbDelete();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 BLB_MEASUREMENTS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpBlbEnableMeas(BLB_MEASUREMENTS MeasurementType, short bEnable);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbLoadOutline(string OutlineFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbLoadSetting(string SettingFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSaveData(string DataFile, short append);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSaveOutline(string OutlineFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSaveSetting(string SettingFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 BLBSET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpBlbSetAttr(BLBSET_ATTRIBUTES Attrib, short Value);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSetRange(short iStart, short iEnd);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowAutoClass(ref short ipClassifiers, short NumMeas, short NumClasses, short bIterate, short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowCluster(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowData(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowHistogram(short Measure, short Bins, short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowObjectWindow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowScattergram(short Measure1, short Measure2, short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowSingleClass(short NumMeasurements, ref float ipBins, short NumClasses, short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowStatistics(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSplitObjects(short bWatershed);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbUpdate(short bRedrawImage);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbRemoveHoles();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSmoothObjects(short smoothing);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSavePopDensities(string DataFile, short append);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSaveClasses(string DataFile, short append);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbShowPopDens(string OutlineFile, short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbMeasure();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbFilter();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 BLB_MEASUREMENTS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpBlbSetFilterRange(BLB_MEASUREMENTS MeasurementType, float min, float max);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 BLB_MEASUREMENTS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpBlbData(BLB_MEASUREMENTS MeasurementType, int fromObj, int toObj, ref float values);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 BLBGET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpBlbGet(BLBGET_ATTRIBUTES Command_Renamed, int tag, short Param2, ref int lpParam);
        [DllImport("IPC32", EntryPoint = "IpBlbGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for GETLABEL 
        public static extern int IpBlbGetStr(short sCmd, short sTag, short sParam, string lpParam);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbCreateMask();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbMultiRanges(ref float intRanges, short numranges);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbRange(short rangeid);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbHideObject(int objnum, short rangeid, short hide);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbFromAoi(short sMode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbSetRangeEx(short sRange, float fStart, float fEnd);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBlbHitTest(short x, short y);

        //----------------------------------------------------------------- 
        // calibration functions 
        //----------------------------------------------------------------- 

        // for IpICalShow() 
        public enum ICALSHOW_COMMANDS
        {
            ICALSHOW_HIDE = Ipc32v5.ICAL_HIDE,
            ICALSHOW_SHOW = Ipc32v5.ICAL_SHOW,
            ICALSHOW_MINIMIZE = Ipc32v5.ICAL_MINIMIZE
        }

        // for IpICalGetLong() 
        public enum ICALGETLNG_ATTRIBUTES
        {
            ICALGET_NUM_ALL = Ipc32v5.ICAL_NUM_ALL,
            ICALGET_NUM_REF = Ipc32v5.ICAL_NUM_REF,
            ICALGET_GET_ALL = Ipc32v5.ICAL_GET_ALL,
            ICALGET_GET_REF = Ipc32v5.ICAL_GET_REF,
            ICALGET_NUM_SAMPLES = Ipc32v5.ICAL_NUM_SAMPLES,
            ICALGET_IS_REFERENCE = Ipc32v5.ICAL_IS_REFERENCE,
            ICALGET_CURRENT = Ipc32v5.ICAL_CURRENT,
            ICALGET_SHOW_REF_ONLY = Ipc32v5.ICAL_SHOW_REF_ONLY
        }

        // for IpICalGetLong() and IpICalSetLong() 
        public enum ICALSETLNG_ATTRIBUTES
        {
            ICALSET_NUM_ALL = Ipc32v5.ICAL_NUM_ALL,
            ICALSET_NUM_REF = Ipc32v5.ICAL_NUM_REF,
            ICALSET_GET_ALL = Ipc32v5.ICAL_GET_ALL,
            ICALSET_GET_REF = Ipc32v5.ICAL_GET_REF,
            ICALSET_NUM_SAMPLES = Ipc32v5.ICAL_NUM_SAMPLES,
            ICALSET_IS_REFERENCE = Ipc32v5.ICAL_IS_REFERENCE,
            ICALSET_CURRENT = Ipc32v5.ICAL_CURRENT,
            ICALSET_SHOW_REF_ONLY = Ipc32v5.ICAL_SHOW_REF_ONLY,
            ICALSET_ADD_TO_REF = Ipc32v5.ICAL_ADD_TO_REF,
            ICALSET_REMOVE_FROM_REF = Ipc32v5.ICAL_REMOVE_FROM_REF,
            ICALSET_APPLY = Ipc32v5.ICAL_APPLY
        }

        // for IpICalGetSng() and IpICalSetSng() 
        public enum ICALSNG_ATTRIBUTES
        {
            ICALDBL_OD_BLACK = Ipc32v5.ICAL_OD_BLACK,
            ICALDBL_OD_INCIDENT = Ipc32v5.ICAL_OD_INCIDENT
        }

        // for IpICalGetStr() and IpICalSetStr() 
        public enum ICALSTR_ATTRIBUTES
        {
            ICALSTR_NAME = Ipc32v5.ICAL_NAME,
            ICALSTR_UNITS = Ipc32v5.ICAL_UNITS
        }

        // for IpICalSetPoints, IpICalSetPointsEx 
        public enum ICALSETFIT_METHOD
        {
            ICALSETFIT_POLYNOMIAL1 = 1,
            // First order polynomial (linear) 
            ICALSETFIT_POLYNOMIAL2 = 2,
            // Second order polynomial 
            ICALSETFIT_POLYNOMIAL3 = 3,
            // Third order polynomial 
            ICALSETFIT_LAGRANGE1 = 4,
            // First order Lagrange (linear) 
            ICALSETFIT_LAGRANGE2 = 5,
            // Second order Lagrange 
            ICALSETFIT_LAGRANGE3 = 6
            // Third order Lagrange 
        }
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpCalLoad(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCalSave(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCalSaveAll(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCalSaveEx(string FileName, short docId, short mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCalGet(string szInput, string szOutput);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpICalCreate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalDestroy();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalMove(short x, short y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalReset();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSelect(string szICal);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSetName(string szICal);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSetOptDens(float BlackLevel, float IncidentLevel);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 ICALSETFIT_METHOD に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpICalSetPoints(ref float ipICalPoints, short numpoints, ICALSETFIT_METHOD fitmode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSetSamples(short NumSamples);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSetUnitName(string UnitName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 ICALSHOW_COMMANDS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpICalShow(ICALSHOW_COMMANDS bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalShowFormat(short bOptDens);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalLinearize(short bNewImage, short bInverse, short bScale);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // additional intensity calibration functions 
        public static extern int IpICalLoad(string FileName, short Ref);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSave(int Calibration, string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // functions that work with specific calibrations 
        public static extern int IpICalDestroyEx(int Calibration);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 ICALSETFIT_METHOD に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpICalSetPointsEx(int Calibration, ref float ipICalPoints, short numpoints, ICALSETFIT_METHOD fitmode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // functions that get or set calibration information 
        //UPGRADE_WARNING: 構造体 ICALGETLNG_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpICalGetLong(int Calibration, ICALGETLNG_ATTRIBUTES sAttribute, ref int CurrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 ICALSNG_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpICalGetSng(int Calibration, ICALSNG_ATTRIBUTES sAttribute, ref float CurrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 ICALSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpICalGetStr(int Calibration, ICALSTR_ATTRIBUTES sAttribute, string CurrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 ICALSETLNG_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpICalSetLong(int Calibration, ICALSETLNG_ATTRIBUTES sAttribute, int NewValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 ICALSNG_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpICalSetSng(int Calibration, ICALSNG_ATTRIBUTES sAttribute, float NewValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 ICALSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpICalSetStr(int Calibration, ICALSTR_ATTRIBUTES sAttribute, string NewValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalGetSystem(short wClass);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSetSystem(int Calibration, short wClass);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalSetSystemByName(string CalName, short wClass);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpICalCalibValues(int Calibration, short numPixels, ref double PixelList, ref double ValueList);

        // for IpSCalShow() 
        public enum SCALSHOW_COMMANDS
        {
            SCALSHOW_HIDE = Ipc32v5.SCAL_HIDE,
            SCALSHOW_MAIN = Ipc32v5.SCAL_DLG_MAIN,
            SCALSHOW_SELECT = Ipc32v5.SCAL_DLG_SELECT,
            SCALSHOW_ADD_MARKER = Ipc32v5.SCAL_ADD_MARKER,
            SCALSHOW_MINIMIZE = Ipc32v5.SCAL_MINIMIZE,
            SCALSHOW_WIZARD = Ipc32v5.SCAL_DLG_WIZARD,
            SCALSHOW_SYSTEM = Ipc32v5.SCAL_DLG_SYSTEM
        }

        // for IpSCalShowEx() 
        public enum SCALSHOWEX_COMMANDS
        {
            SCALSHOWEX_HIDE = Ipc32v5.SCAL_HIDE,
            SCALSHOWEX_MINIMIZE = Ipc32v5.SCAL_MINIMIZE,
            SCALSHOWEX_SHOW = Ipc32v5.SCAL_SHOW,
            SCALSHOWEX_HIDEALL = Ipc32v5.SCAL_HIDEALL
        }

        public enum SCALSHOWEX_DIALOGS
        {
            SCALDLG_MAIN = Ipc32v5.SCAL_DLG_MAIN,
            SCALDLG_SELECT = Ipc32v5.SCAL_DLG_SELECT,
            SCALDLG_WIZARD = Ipc32v5.SCAL_DLG_WIZARD,
            SCALDLG_SYSTEM = Ipc32v5.SCAL_DLG_SYSTEM
        }

        // for IpSCalGetLong() 
        public enum SCALGETLNG_ATTRIBUTES
        {
            SCALGET_NUM_ALL = Ipc32v5.SCAL_NUM_ALL,
            SCALGET_NUM_REF = Ipc32v5.SCAL_NUM_REF,
            SCALGET_GET_ALL = Ipc32v5.SCAL_GET_ALL,
            SCALGET_GET_REF = Ipc32v5.SCAL_GET_REF,
            SCALGET_IS_REFERENCE = Ipc32v5.SCAL_IS_REFERENCE,
            SCALGET_IS_SYSTEM = Ipc32v5.SCAL_IS_SYSTEM,
            SCALGET_ONIMAGE_COLOR = Ipc32v5.SCAL_ONIMAGE_COLOR,
            SCALGET_CURRENT = Ipc32v5.SCAL_CURRENT,
            SCALGET_SYSTEM = Ipc32v5.SCAL_SYSTEM,
            SCALGET_MARKER_STYLE = Ipc32v5.SCAL_MARKER_STYLE,
            SCALGET_SHOW_REF_ONLY = Ipc32v5.SCAL_SHOW_REF_ONLY,
            SCALGET_UNIT_CONVERT = Ipc32v5.SCAL_UNIT_CONVERT,
            SCALGET_MARKER_FONT_SIZE = Ipc32v5.SCAL_MARKER_FONT_SIZE,
            SCALGET_MARKER_FONT_WEIGHT = Ipc32v5.SCAL_MARKER_FONT_WEIGHT,
            SCALGET_MARKER_LINE_WIDTH = Ipc32v5.SCAL_MARKER_LINE_WIDTH,
            SCALGET_MARKER_LINE_ENDS = Ipc32v5.SCAL_MARKER_LINE_ENDS
        }

        // for IpSCalSetLong() 
        public enum SCALSETLNG_ATTRIBUTES
        {
            SCALSET_ONIMAGE_COLOR = Ipc32v5.SCAL_ONIMAGE_COLOR,
            SCALSET_CURRENT = Ipc32v5.SCAL_CURRENT,
            SCALSET_SYSTEM = Ipc32v5.SCAL_SYSTEM,
            SCALSET_MARKER_STYLE = Ipc32v5.SCAL_MARKER_STYLE,
            SCALSET_SHOW_REF_ONLY = Ipc32v5.SCAL_SHOW_REF_ONLY,
            SCALSET_UNIT_CONVERT = Ipc32v5.SCAL_UNIT_CONVERT,
            SCALSET_ADD_TO_REF = Ipc32v5.SCAL_ADD_TO_REF,
            SCALSET_REMOVE_FROM_REF = Ipc32v5.SCAL_REMOVE_FROM_REF,
            SCALSET_APPLY = Ipc32v5.SCAL_APPLY,
            SCALSET_APPLY_RESOLUTION = Ipc32v5.SCAL_APPLY_RESOLUTION,
            SCALSET_MARKER_FONT_SIZE = Ipc32v5.SCAL_MARKER_FONT_SIZE,
            SCALSET_MARKER_FONT_WEIGHT = Ipc32v5.SCAL_MARKER_FONT_WEIGHT,
            SCALSET_MARKER_LINE_WIDTH = Ipc32v5.SCAL_MARKER_LINE_WIDTH,
            SCALSET_MARKER_LINE_ENDS = Ipc32v5.SCAL_MARKER_LINE_ENDS
        }

        // for IpSCalGetSng() and IpSCalSetSng() 
        public enum SCALGETSNG_ATTRIBUTES
        {
            SCALGETSNG_ASPECT = Ipc32v5.SCAL_ASPECT,
            SCALGETSNG_CONVERSION_TO_MM = Ipc32v5.SCAL_CONVERSION_TO_MM,
            SCALGETSNG_SCALE_X = Ipc32v5.SCAL_SCALE_X,
            SCALGETSNG_SCALE_Y = Ipc32v5.SCAL_SCALE_Y,
            SCALGETSNG_ORIGIN_X = Ipc32v5.SCAL_ORIGIN_X,
            SCALGETSNG_ORIGIN_Y = Ipc32v5.SCAL_ORIGIN_Y,
            SCALGETSNG_ANGLE = Ipc32v5.SCAL_ANGLE,
            SCALGETSNG_SYSTEM_MODIFIER = Ipc32v5.SCAL_SYSTEM_MODIFIER,
            SCALGETSNG_MARKER_WIDTH = Ipc32v5.SCAL_MARKER_WIDTH
        }

        // for IpSCalSetSng() 
        public enum SCALSETSNG_ATTRIBUTES
        {
            SCALSETSNG_SCALE_X = Ipc32v5.SCAL_SCALE_X,
            SCALSETSNG_SCALE_Y = Ipc32v5.SCAL_SCALE_Y,
            SCALSETSNG_ORIGIN_X = Ipc32v5.SCAL_ORIGIN_X,
            SCALSETSNG_ORIGIN_Y = Ipc32v5.SCAL_ORIGIN_Y,
            SCALSETSNG_ANGLE = Ipc32v5.SCAL_ANGLE,
            SCALSETSNG_SYSTEM_MODIFIER = Ipc32v5.SCAL_SYSTEM_MODIFIER,
            SCALSETSNG_MARKER_WIDTH = Ipc32v5.SCAL_MARKER_WIDTH
        }

        // for IpSCalGetStr() and IpSCalSetStr() 
        public enum SCALGETSTR_ATTRIBUTES
        {
            SCALGETSTR_NAME = Ipc32v5.SCAL_NAME,
            SCALGETSTR_UNITS = Ipc32v5.SCAL_UNITS,
            SCALGETSTR_FIND_BY_NAME = Ipc32v5.SCAL_FIND_BY_NAME,
            SCALGETSTR_MARKER_FONT_FACE = Ipc32v5.SCAL_MARKER_FONT_FACE
        }

        // for IpSCalGetStr() and IpSCalSetStr() 
        public enum SCALSETSTR_ATTRIBUTES
        {
            SCALSETSTR_NAME = Ipc32v5.SCAL_NAME,
            SCALSETSTR_UNITS = Ipc32v5.SCAL_UNITS,
            SCALSETSTR_MARKER_FONT_FACE = Ipc32v5.SCAL_MARKER_FONT_FACE
        }
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpSCalCreate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalDestroy();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalMove(short x, short y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalReset();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSelect(string szSCal);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSetAngle(float Angle);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSetAspect(float AspectRatio);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSetName(string szSCal);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSetOrigin(float x, float y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSetUnit(float x, float y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSetUnitName(string UnitName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 SCALSHOW_COMMANDS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpSCalShow(SCALSHOW_COMMANDS Show);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 SCALSHOWEX_COMMANDS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 SCALSHOWEX_DIALOGS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpSCalShowEx(SCALSHOWEX_DIALOGS Dialog, SCALSHOWEX_COMMANDS Show);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // additional spatial calibration functions 
        public static extern int IpSCalLoad(string FileName, short Ref);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSCalSave(int Calibration, string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // functions that work with specific calibrations 
        public static extern int IpSCalDestroyEx(int Calibration);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // functions that get or set calibration information 
        //UPGRADE_WARNING: 構造体 SCALGETLNG_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpSCalGetLong(int Calibration, SCALGETLNG_ATTRIBUTES sAttribute, ref int CurrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 SCALGETSNG_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpSCalGetSng(int Calibration, SCALGETSNG_ATTRIBUTES sAttribute, ref float CurrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 SCALGETSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpSCalGetStr(int Calibration, SCALGETSTR_ATTRIBUTES sAttribute, string CurrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 SCALSETLNG_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpSCalSetLong(int Calibration, SCALSETLNG_ATTRIBUTES sAttribute, int NewValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 SCALSETSNG_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpSCalSetSng(int Calibration, SCALSETSNG_ATTRIBUTES sAttribute, float NewValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 SCALSETSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpSCalSetStr(int Calibration, SCALSETSTR_ATTRIBUTES sAttribute, string NewValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 POINTAPI に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpSCalCalibValues(int Calibration, short sNumPoints, ref POINTAPI PointList, ref double ValueList);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Screen capture 
        //----------------------------------------------------------------- 
        //UPGRADE_WARNING: 構造体 RECT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpCapArea(ref Winapi.RECT ipRect, short bCursor);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCapFile(string FileFormat, string Directory, string Prefix, short Number);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCapHotKey(string KeyName, short bShift, short bCtrl, short bAlt);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpCapWindow(string title, short bClientOnly, short bCursor);

        //----------------------------------------------------------------- 
        // Color Transformation Functions 
        //----------------------------------------------------------------- 
        public enum IPCOLORCHANNELS
        {
            CM_CLR_RED = 0,
            // for use with CM_RGB 
            CM_CLR_GREEN = 1,
            CM_CLR_BLUE = 2,
            CM_CLR_HUE = 0,
            // for use with CM_HSI and CM_HSV 
            CM_CLR_SATURATION = 1,
            CM_CLR_INTENSITY = 2,
            // for use with CM_HSI 
            CM_CLR_VALUE = 2,
            // for use with CM_HSV 
            CM_CLR_LUMINANCE = 0,
            // for use with CM_YIQ 
            CM_CLR_INPHASE = 1,
            CM_CLR_QUADRATURE = 2
        }
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //UPGRADE_WARNING: 構造体 IPCOLORCHANNELS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 IPCOLORMODELS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 IPCOLORMODELS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpCmChannelExtract(IPCOLORMODELS cmColor, IPCOLORMODELS cmComp, IPCOLORCHANNELS Channel);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 IPCOLORCHANNELS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 IPCOLORMODELS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpCmChannelMerge(short WsId, IPCOLORMODELS cmColor, IPCOLORCHANNELS Channel);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 IPCOLORMODELS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 IPCOLORMODELS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpCmTransform(IPCOLORMODELS cmOut, IPCOLORMODELS cmIn, short bNewImage);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 IPCOLORMODELS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpCmChannelMerge3(short WsIdDest, short WsIdChannel1, short WsIdChannel2, short WsIdChannel3, IPCOLORMODELS cmColor, short bNewImage);

        //---------------------------------------------------------------- 
        // Document window functions 
        //---------------------------------------------------------------- 

        // commands to IpDocGet 
        public enum DOCGET_ATTRIBUTES
        {
            DOCGET_GETACTDOC = Ipc32v5.GETACTDOC,
            DOCGET_GETDOCWND = Ipc32v5.GETDOCWND,
            DOCGET_GETDOCVRI = Ipc32v5.GETDOCVRI,
            DOCGET_GETNUMDOC = Ipc32v5.GETNUMDOC,
            DOCGET_GETDOCLST = Ipc32v5.GETDOCLST,
            DOCGET_GETDOCINFO = Ipc32v5.GETDOCINFO,
            DOCGET_GETINSTINFO = Ipc32v5.GETINSTINFO,
            DOCGET_INF_DPIX = Ipc32v5.INF_DPIX,
            DOCGET_INF_DPIY = Ipc32v5.INF_DPIY,
            DOCGET_INF_RANGE = Ipc32v5.INF_RANGE,
            DOCGET_INF_MAXRANGE = Ipc32v5.INF_MAXRANGE,
            DOCGET_INF_XSCROLL = Ipc32v5.INF_XSCROLL,
            DOCGET_INF_YSCROLL = Ipc32v5.INF_YSCROLL,
            DOCGET_INF_ZOOMFACTOR = Ipc32v5.INF_ZOOMFACTOR,
            DOCGET_DOC_POS_X = Ipc32v5.DOC_POS_X,
            DOCGET_DOC_POS_Y = Ipc32v5.DOC_POS_Y,
            DOCGET_INF_IS_MODIFIED = Ipc32v5.INF_IS_MODIFIED
        }

        // commands to IpDocGetStr 
        public enum DOCGETSTR_ATTRIBUTES
        {
            DOCGETSTR_INF_TITLE = Ipc32v5.INF_TITLE,
            DOCGETSTR_INF_ARTIST = Ipc32v5.INF_ARTIST,
            DOCGETSTR_INF_DATE = Ipc32v5.INF_DATE,
            DOCGETSTR_INF_DESCRIPTION = Ipc32v5.INF_DESCRIPTION,
            DOCGETSTR_INF_NAME = Ipc32v5.INF_NAME,
            DOCGETSTR_INF_FILENAME = Ipc32v5.INF_FILENAME,
            DOCGETSTR_INF_COPYRIGHT = Ipc32v5.INF_COPYRIGHT
        }

        // commands to IpDocGetPosition and IpDocSetPosition 
        public enum DOCPROPPOS_ATTRIBUTES
        {
            DOCPROPPOS_XPOSITION = Ipc32v5.INF_XPOSITION,
            DOCPROPPOS_YPOSITION = Ipc32v5.INF_YPOSITION,
            DOCPROPPOS_ZPOSITION = Ipc32v5.INF_ZPOSITION
        }

        // properties for IpDocGetPropDbl() and IpDocSetPropDbl() 
        public enum DOCPROPDBL_ATTRIBUTES
        {
            DOCPROPDBL_XPOSITION = Ipc32v5.DOCPROP_XPOSITION,
            DOCPROPDBL_YPOSITION = Ipc32v5.DOCPROP_YPOSITION,
            DOCPROPDBL_ZPOSITION = Ipc32v5.DOCPROP_ZPOSITION,
            DOCPROPDBL_EMWAVELENGTH = Ipc32v5.DOCPROP_EMWAVELENGTH,
            DOCPROPDBL_EXWAVELENGTH = Ipc32v5.DOCPROP_EXWAVELENGTH,
            DOCPROPDBL_REFINDEX = Ipc32v5.DOCPROP_REFINDEX,
            DOCPROPDBL_NUMAPERTURE = Ipc32v5.DOCPROP_NUMAPERTURE,
            DOCPROPDBL_MAGNIFICATION = Ipc32v5.DOCPROP_MAGNIFICATION,
            DOCPROPDBL_EXPOSURE = Ipc32v5.DOCPROP_EXPOSURE,
            DOCPROPDBL_GAIN = Ipc32v5.DOCPROP_GAIN,
            DOCPROPDBL_OFFSET = Ipc32v5.DOCPROP_OFFSET,
            DOCPROPDBL_GAMMA = Ipc32v5.DOCPROP_GAMMA,
            DOCPROPDBL_WHITEBAL_R = Ipc32v5.DOCPROP_WHITEBAL_R,
            DOCPROPDBL_WHITEBAL_G = Ipc32v5.DOCPROP_WHITEBAL_G,
            DOCPROPDBL_WHITEBAL_B = Ipc32v5.DOCPROP_WHITEBAL_B,
            DOCPROPDBL_SUGG_WHITEBAL_R = Ipc32v5.DOCPROP_SUGG_WHITEBAL_R,
            DOCPROPDBL_SUGG_WHITEBAL_G = Ipc32v5.DOCPROP_SUGG_WHITEBAL_G,
            DOCPROPDBL_SUGG_WHITEBAL_B = Ipc32v5.DOCPROP_SUGG_WHITEBAL_B
        }

        // properties for IpDocGetPropStr() and IpDocSetPropStr() 
        public enum DOCPROPSTR_ATTRIBUTES
        {
            DOCPROPSTR_CHANNELNAME = Ipc32v5.DOCPROP_CHANNELNAME,
            DOCPROPSTR_SITELABEL = Ipc32v5.DOCPROP_SITELABEL,
            DOCPROPSTR_CAPTDRIVERNAME = Ipc32v5.DOCPROP_CAPTDRIVERNAME,
            DOCPROPSTR_CAPTCAMERANAME = Ipc32v5.DOCPROP_CAPTCAMERANAME,
            DOCPROPSTR_CAPTCAMERAID = Ipc32v5.DOCPROP_CAPTCAMERAID,
            DOCPROPSTR_CAPTDRIVERFEATURES = Ipc32v5.DOCPROP_CAPTDRIVERFEATURES,
            DOCPROPSTR_CAPTDRIVERVERSION = Ipc32v5.DOCPROP_CAPTDRIVERVERSION,
            DOCPROPSTR_TIMEPHASELABEL = Ipc32v5.DOCPROP_TIMEPHASELABEL
        }

        // properties IpDocGetPropDate() and IpDocSetPropDate() 
        // These can be also used with IpDocGetPropDbl and IpDocGetPropDbl for more accuracy, and also for calculating deltas 
        public enum DOCPROPDATE_ATTRIBUTES
        {
            DOCPROPDATE_TIME = Ipc32v5.DOCPROP_TIME,
            DOCPROPDATE_TIMEPOINT = Ipc32v5.DOCPROP_TIMEPOINT
        }

        // properties for IpDocGetPropLong() and IpDocSetPropLong() 
        public enum DOCPROPLNG_ATTRIBUTES
        {
            DOCPROPLNG_BIN_X = Ipc32v5.DOCPROP_BIN_X,
            DOCPROPLNG_BIN_Y = Ipc32v5.DOCPROP_BIN_Y,
            DOCPROPLNG_CAPTRECT_L = Ipc32v5.DOCPROP_CAPTRECT_L,
            DOCPROPLNG_CAPTRECT_R = Ipc32v5.DOCPROP_CAPTRECT_R,
            DOCPROPLNG_CAPTRECT_T = Ipc32v5.DOCPROP_CAPTRECT_T,
            DOCPROPLNG_CAPTRECT_B = Ipc32v5.DOCPROP_CAPTRECT_B,
            DOCPROPLNG_CHIPCOORD_L = Ipc32v5.DOCPROP_CHIPCOORD_L,
            DOCPROPLNG_CHIPCOORD_R = Ipc32v5.DOCPROP_CHIPCOORD_R,
            DOCPROPLNG_CHIPCOORD_T = Ipc32v5.DOCPROP_CHIPCOORD_T,
            DOCPROPLNG_CHIPCOORD_B = Ipc32v5.DOCPROP_CHIPCOORD_B,
            DOCPROPLNG_NATIVE_BITDEPTH = Ipc32v5.DOCPROP_NATIVE_BITDEPTH,
            DOCPROPLNG_DISPLAY_TINT = Ipc32v5.DOCPROP_DISPLAY_TINT
        }
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpDocClose();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocMaximize();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocMinimize();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocMove(short x, short y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocRestore();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocSize(short Width, short Height);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 DOCGET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDocGet(DOCGET_ATTRIBUTES Command_Renamed, short wParam, ref int lpParam);
        [DllImport("IPC32", EntryPoint = "IpDocGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for INF_NAME, INF_TITLE, INF_ARTIST, INF_DATE, INF_DESCRIPTION and INF_COPYRIGHT 
        //UPGRADE_WARNING: 構造体 DOCGETSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDocGetStr(DOCGETSTR_ATTRIBUTES sCmd, short sParam, string lpParam);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 RECT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDocOpenVri(short docId, short oMode, ref Winapi.RECT oExtent);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocOpenAoi(short docId, short oMode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocCloseVri(int iiInst);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpDocGetLine(int iiInst, short LineNum, ref int LineBuf);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpDocPutLine(int iiInst, short LineNum, ref int LineBuf, short bAOI);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 RECT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDocGetArea(short docId, ref Winapi.RECT rArea, ref int lpBuf, short gMode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 RECT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDocPutArea(short docId, ref Winapi.RECT rArea, ref int lpBuf, short pMode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 POINTAPI に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDocClick(string Message, ref POINTAPI CurPos);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocFind(string WorkspaceName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDocCloseEx(short docId);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 RECT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDocGetAreaSize(short docId, ref Winapi.RECT rArea, short gMode, ref int lplSize);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // The DocPosition parameter should be an IPDOCPOS structure 
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 DOCPROPPOS_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDocGetPosition(short docId, DOCPROPPOS_ATTRIBUTES Attr, int frame, ref int DocPosition);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DOCPROPPOS_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDocSetPosition(short docId, DOCPROPPOS_ATTRIBUTES Attr, int frame, double DocPosition);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DOCPROPDBL_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDocGetPropDbl(short docId, DOCPROPDBL_ATTRIBUTES Attr, int frame, ref double DocProperty);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DOCPROPDBL_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDocSetPropDbl(short docId, DOCPROPDBL_ATTRIBUTES Attr, int frame, double DocProperty);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DOCPROPSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDocGetPropStr(short docId, DOCPROPSTR_ATTRIBUTES Attr, int frame, string DocProperty);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DOCPROPSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDocSetPropStr(short docId, DOCPROPSTR_ATTRIBUTES Attr, int frame, string DocProperty);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for DOCPROP_TIME and DOCPROP_TIMEPOINT 
        //UPGRADE_WARNING: 構造体 DOCPROPDATE_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDocGetPropDate(short docId, DOCPROPDATE_ATTRIBUTES Attr, int frame, ref System.DateTime DocProperty);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DOCPROPDATE_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDocSetPropDate(short docId, DOCPROPDATE_ATTRIBUTES Attr, int frame, System.DateTime DocProperty);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for DOCPROP_BIN, DOCPROP_CAPTRECT, DOCPROP_CHIPCOORD, DOCPROP_NATIVE_BITDEPTH, and DOCPROP_DISPLAY_TINT properties 
        //UPGRADE_WARNING: 構造体 DOCPROPLNG_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDocGetPropLong(short docId, DOCPROPLNG_ATTRIBUTES Attr, int frame, ref int DocProperty);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DOCPROPLNG_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDocSetPropLong(short docId, DOCPROPLNG_ATTRIBUTES Attr, int frame, int DocProperty);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // FFT - Fast Fourier Transform functions 
        //----------------------------------------------------------------- 
        public static extern int IpFftForward(short DisplayType, short bFullFft);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftHiPass(short iType, short Transition, short PreserveNil);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftInverse(short WsId, short PreserveData);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftLoad(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftLoPass(short iType, short Transition);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftSave(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftSpikeCut(short iType, short Transition, short Symmetrical);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftSpikeBoost(short iType, short Transition, short Symmetrical);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFftTag(short docId, short iType, short sourceClass);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // FILTRDLG.DLL - Spatial filtering 
        //----------------------------------------------------------------- 
        public static extern int IpFltClose(short Shape, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltConvolveKernel(string KernelName, short Strength, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltDilate(short Shape, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltErode(short Shape, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltExtractBkgnd(short BrightOnDark, short ObjectSize);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltFlatten(short BrightOnDark, short ObjectSize);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltHiPass(short sSize, short Strength, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltLaplacian(short sSize, short Strength, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltLoPass(short sSize, short Strength, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltMedian(short sSize, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltRank(short sSize, short Threshold, short Rank, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltOpen(short Shape, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltPhase();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltRoberts();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltSharpen(short sSize, short Strength, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltSobel();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltThin(short Threshold);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltThinEx(short Threshold, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltWatershed(short Threshold);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltWatershedEx(short Threshold, short Erosions);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltVariance(short sizex, short sizey);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltPrune(short Threshold, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltUserErode(string KernelName, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltUserDilate(string KernelName, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_NOTE: Step は Step_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpFltLocHistEq(short WinSize, short Step_Renamed, short EqType, float StdDev);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltDistance(short Threshold, short mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltReduce(short Threshold, short mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltGauss(short sSize, short Strength, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltBranchEnd(short Threshold, short Classify);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltDespeckle(short sSize, short Strength, short Passes);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Gallery 
        //----------------------------------------------------------------- 
        public static extern int IpGalAdd(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalChangeDescription(short DescriptionType, string Description);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalDelete(string GalleryName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalNew(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalOpen(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalOpenPhotoCD(string DriveLetter, string GalleryName, short Resolution);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalRemove(short bFromDisk);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalSetActive(short GalId);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalSort(short bByName, short bAscending);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalTag(short SlotNumber, short bTag);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalUpdate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalImageOpen(short imageId);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGalClose();

        //----------------------------------------------------------------- 
        // Histogram functions 
        //----------------------------------------------------------------- 

        // commands for IpHstGet 
        public enum HSTGET_ATTRIBUTES
        {
            HSTGET_GETNUMPTS = Ipc32v5.GETNUMPTS,
            HSTGET_GETSTATS = Ipc32v5.GETSTATS,
            HSTGET_GETVALUES = Ipc32v5.GETVALUES,
            HSTGET_GETRANGE = Ipc32v5.GETRANGE,
            HSTGET_GETINDEX = Ipc32v5.GETINDEX,
            HSTGET_GETLNUMPTS = Ipc32v5.GETLNUMPTS,
            HSTGET_GETCHANNELS = Ipc32v5.GETCHANNELS
        }

        // attributes for IpHstSetAttr 
        public enum HSTSET_ATTRIBUTES
        {
            HSTSET_COLORMODEL = Ipc32v5.COLORMODEL,
            HSTSET_AUTOUPDATE = Ipc32v5.AUTOUPDATE,
            HSTSET_SCAL = Ipc32v5.SCAL,
            HSTSET_ICAL = Ipc32v5.ICAL,
            HSTSET_ACCUMULATE = Ipc32v5.ACCUMULATE,
            HSTSET_LINETYPE = Ipc32v5.LINETYPE,
            HSTSET_BIN = Ipc32v5.BIN,
            HSTSET_STATISTICS = Ipc32v5.STATISTICS,
            HSTSET_GRID = Ipc32v5.GRID,
            HSTSET_CHANNEL1 = Ipc32v5.CHANNEL1,
            HSTSET_CHANNEL2 = Ipc32v5.CHANNEL2,
            HSTSET_CHANNEL3 = Ipc32v5.CHANNEL3
        }
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpHstEqualize(short Method);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstCreate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstDestroy();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstMaximize();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstMinimize();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstMove(short x, short y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstRestore();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstSave(string FileName, short bAppend);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstScale(short bVert, short bAuto, float From, float iEnd);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstSelect(short HstId);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 HSTSET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpHstSetAttr(HSTSET_ATTRIBUTES AttrType, short AttrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstSize(short cx, short cy);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpHstUpdate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 HSTGET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpHstGet(HSTGET_ATTRIBUTES Command_Renamed, short wParam, ref int lpParam);

        //----------------------------------------------------------------- 
        // Contrast Enhancement (Lookup Table) 
        //----------------------------------------------------------------- 

        public enum LUTSET_ATTRIBUTES
        {
            LUTSET_BRIGHTNESS = Ipc32v5.LUT_BRIGHTNESS,
            LUTSET_CONTRAST = Ipc32v5.LUT_CONTRAST,
            LUTSET_GAMMA = Ipc32v5.LUT_GAMMA,
            LUTSET_CHANNEL = Ipc32v5.Channel,
            LUTSET_CURVE = Ipc32v5.CURVE,
            LUTSET_GRID = Ipc32v5.GRID
        }
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpLutApply();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLutBinarize(short MinRange, short MaxRange, short WhiteOnBlack);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLutLoad(string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLutReset(short Channel, short iType);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLutSave(string FileName, string Description);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 LUTSET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpLutSetAttr(LUTSET_ATTRIBUTES AttrType, short AttrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLutSetControl(short ControlType, ref short ipLutControls, short Count);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLutShow(short Show);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpLutData(short sAttrType, ref int pData);

        //----------------------------------------------------------------- 
        // Measurements 
        //----------------------------------------------------------------- 

        // attributes for IpMeasAttr 
        public enum MEASSET_ATTRIBUTES
        {
            MEASSET_STATS = Ipc32v5.MEAS_STATS,
            MEASSET_DISPCOLOR = Ipc32v5.MEAS_DISPCOLOR,
            MEASSET_LABELCOLOR = Ipc32v5.MEAS_LABELCOLOR,
            MEASSET_THICKMODE = Ipc32v5.MEAS_THICKMODE,
            MEASSET_ANGLE180 = Ipc32v5.MEAS_ANGLE180,
            MEASSET_MEASCOLOR = Ipc32v5.MEAS_MEASCOLOR,
            MEASSET_UPDATE = Ipc32v5.MEAS_UPDATE,
            MEASSET_PROMPTS = Ipc32v5.MEAS_PROMPTS,
            MEASSET_SHOWLAYOUT = Ipc32v5.MEAS_SHOWLAYOUT,
            MEASSET_DISPBFPTS = Ipc32v5.MEAS_DISPBFPTS,
            MEASSET_MAXLINEPTS = Ipc32v5.MEAS_MAXLINEPTS,
            MEASSET_MAXCIRCLEPTS = Ipc32v5.MEAS_MAXCIRCLEPTS,
            MEASSET_MAXARCPTS = Ipc32v5.MEAS_MAXARCPTS,
            MEASSET_PASSFAILTYPE = Ipc32v5.MEAS_PASSFAILTYPE,
            MEASSET_DISPCOUNTOPTS = Ipc32v5.MEAS_DISPCOUNTOPTS,
            MEASSET_DISPLAYFEATURES = Ipc32v5.MEAS_DISPLAYFEATURES,
            MEASSET_SIGNIFICANTDIGITS = Ipc32v5.MEAS_SIGNIFICANTDIGITS,
            MEASSET_DISPLAYTYPE = Ipc32v5.MEAS_DISPLAYTYPE,
            MEASSET_DISPLAYLINEWIDTH = Ipc32v5.MEAS_DISPLAYLINEWIDTH,
            MEASSET_DISPLAYFONTSIZE = Ipc32v5.MEAS_DISPLAYFONTSIZE,
            MEASSET_DISPLAYFONTWEIGHT = Ipc32v5.MEAS_DISPLAYFONTWEIGHT
        }

        // commands for IpMeasAttrStr 
        public enum MEASSETSTR_ATTRIBUTES
        {
            MEASSETSTR_SETNAME = Ipc32v5.MEAS_SETNAME,
            MEASSETSTR_DISPLAYFONTFACE = Ipc32v5.MEAS_DISPLAYFONTFACE
        }

        // commands for IpMeasGet 
        public enum MEASGET_ATTRIBUTES
        {
            MEASGET_GETNUMOBJ = Ipc32v5.GETNUMOBJ,
            MEASGET_GETNUMPTS = Ipc32v5.GETNUMPTS,
            MEASGET_GETBOUNDS = Ipc32v5.GETBOUNDS,
            MEASGET_GETPOINTS = Ipc32v5.GETPOINTS,
            MEASGET_GETSTATS = Ipc32v5.GETSTATS,
            MEASGET_GETVALUES = Ipc32v5.GETVALUES,
            MEASGET_GETTYPE = Ipc32v5.IPGETTTYPE,
            MEASGET_GETLABEL = Ipc32v5.GETLABEL,
            MEASGET_GETINDEX = Ipc32v5.GETINDEX,
            MEASGET_GETNUMMEAS = Ipc32v5.GETNUMMEAS,
            MEASGET_GETFEATVALUES = Ipc32v5.GETFEATVALUES,
            MEASGET_GETMEASVALUES = Ipc32v5.GETMEASVALUES,
            MEASGET_GETNUMSELFEATURES = Ipc32v5.GETNUMSELFEATURES,
            MEASGET_GETSELFEATURES = Ipc32v5.GETSELFEATURES,
            MEASGET_STATS = Ipc32v5.MEAS_STATS + Ipc32v5.GETIPPSETTING,
            MEASGET_DISPCOLOR = Ipc32v5.MEAS_DISPCOLOR + Ipc32v5.GETIPPSETTING,
            MEASGET_LABELCOLOR = Ipc32v5.MEAS_LABELCOLOR + Ipc32v5.GETIPPSETTING,
            MEASGET_THICKMODE = Ipc32v5.MEAS_THICKMODE + Ipc32v5.GETIPPSETTING,
            MEASGET_ANGLE180 = Ipc32v5.MEAS_ANGLE180 + Ipc32v5.GETIPPSETTING,
            MEASGET_MEASCOLOR = Ipc32v5.MEAS_MEASCOLOR + Ipc32v5.GETIPPSETTING,
            MEASGET_UPDATE = Ipc32v5.MEAS_UPDATE + Ipc32v5.GETIPPSETTING,
            MEASGET_PROMPTS = Ipc32v5.MEAS_PROMPTS + Ipc32v5.GETIPPSETTING,
            MEASGET_SHOWLAYOUT = Ipc32v5.MEAS_SHOWLAYOUT + Ipc32v5.GETIPPSETTING,
            MEASGET_DISPBFPTS = Ipc32v5.MEAS_DISPBFPTS + Ipc32v5.GETIPPSETTING,
            MEASGET_MAXLINEPTS = Ipc32v5.MEAS_MAXLINEPTS + Ipc32v5.GETIPPSETTING,
            MEASGET_MAXCIRCLEPTS = Ipc32v5.MEAS_MAXCIRCLEPTS + Ipc32v5.GETIPPSETTING,
            MEASGET_MAXARCPTS = Ipc32v5.MEAS_MAXARCPTS + Ipc32v5.GETIPPSETTING,
            MEASGET_PASSFAILTYPE = Ipc32v5.MEAS_PASSFAILTYPE + Ipc32v5.GETIPPSETTING,
            MEASGET_DISPCOUNTOPTS = Ipc32v5.MEAS_DISPCOUNTOPTS + Ipc32v5.GETIPPSETTING,
            MEASGET_DISPLAYFEATURES = Ipc32v5.MEAS_DISPLAYFEATURES + Ipc32v5.GETIPPSETTING,
            MEASGET_SIGNIFICANTDIGITS = Ipc32v5.MEAS_SIGNIFICANTDIGITS + Ipc32v5.GETIPPSETTING,
            MEASGET_DISPLAYTYPE = Ipc32v5.MEAS_DISPLAYTYPE + Ipc32v5.GETIPPSETTING,
            MEASGET_DISPLAYLINEWIDTH = Ipc32v5.MEAS_DISPLAYLINEWIDTH + Ipc32v5.GETIPPSETTING,
            MEASGET_DISPLAYFONTSIZE = Ipc32v5.MEAS_DISPLAYFONTSIZE + Ipc32v5.GETIPPSETTING,
            MEASGET_DISPLAYFONTWEIGHT = Ipc32v5.MEAS_DISPLAYFONTWEIGHT + Ipc32v5.GETIPPSETTING
        }


        // commands for IpMeasGetStr 
        public enum MEASGETSTR_ATTRIBUTES
        {
            MEASGETSTR_GETNAME = GETNAME,
            MEASGETSTR_DISPLAYFONTFACE = Ipc32v5.MEAS_DISPLAYFONTFACE
        }

        public enum MEAS_TOOLS
        {
            MEASTOOL_NONE = Ipc32v5.MEAS_NONE,
            MEASTOOL_LENGTH = Ipc32v5.MEAS_LENGTH,
            MEASTOOL_AREA = Ipc32v5.MEAS_AREA,
            MEASTOOL_ANGLE = Ipc32v5.MEAS_ANGLE,
            MEASTOOL_TRACE = Ipc32v5.MEAS_TRACE,
            MEASTOOL_THICK = Ipc32v5.MEAS_THICK,
            MEASTOOL_POINT = Ipc32v5.MEAS_POINT,
            MEASTOOL_RECT = Ipc32v5.MEAS_RECT,
            MEASTOOL_CIRCLE = Ipc32v5.MEAS_CIRCLE,
            MEASTOOL_BFLINE = Ipc32v5.MEAS_BFLINE,
            MEASTOOL_BFCIRCLE = Ipc32v5.MEAS_BFCIRCLE,
            MEASTOOL_BFARC = Ipc32v5.MEAS_BFARC,
            MEASTOOL_DIST = Ipc32v5.MEAS_DIST,
            MEASTOOL_NEWANGLE = Ipc32v5.MEAS_NEWANGLE,
            MEASTOOL_HTHICK = Ipc32v5.MEAS_HTHICK,
            MEASTOOL_VTHICK = Ipc32v5.MEAS_VTHICK,
            MEASTOOL_CTHICK = Ipc32v5.MEAS_CTHICK,
            MEASTOOL_PERPDIST = Ipc32v5.MEAS_PERPDIST,
            MEASTOOL_COUNT = Ipc32v5.MEAS_COUNT,
            MEASTOOL_DATA_TO_IMAGE = Ipc32v5.MEAS_DATA_TO_IMAGE,
            MEASTOOL_SELECT = Ipc32v5.MEAS_SELECT
        }

        // commands for IpMeasShow 
        public enum MEASSHOW_COMMANDS
        {
            MEASSHOW_HIDE = Ipc32v5.MEAS_HIDE,
            MEASSHOW_SHOW = Ipc32v5.MEAS_SHOW,
            MEASSHOW_SHOWADVANCED = Ipc32v5.MEAS_SHOWADVANCED,
            MEASSHOW_SHOWBASIC = Ipc32v5.MEAS_SHOWBASIC,
            MEASSHOW_SHOWFEATURES = Ipc32v5.MEAS_SHOWFEATURES,
            MEASSHOW_SHOWMEASUREMENTS = Ipc32v5.MEAS_SHOWMEASUREMENTS,
            MEASSHOW_SHOWINPUTOUTPUT = Ipc32v5.MEAS_SHOWINPUTOUTPUT,
            MEASSHOW_SHOWOPTIONS = Ipc32v5.MEAS_SHOWOPTIONS,
            MEASSHOW_SHOWADVOPTIONS = Ipc32v5.MEAS_SHOWADVOPTIONS
        }

        public enum MEASLOAD_COMMANDS
        {
            MEASLOAD_INTERACTIVE = Ipc32v5.MLOAD_INTERACTIVE,
            MEASLOAD_RELOAD = Ipc32v5.MLOAD_RELOAD
        }
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //UPGRADE_WARNING: 構造体 MEASSET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpMeasAttr(MEASSET_ATTRIBUTES sAttr, short Value);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasDelete(short bMeasurement);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasLoadOutline(string OutlineFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasMove(short x, short y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasRestore();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasSaveData(string DataFile, short saveMode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasSaveOutline(string OutlineFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 MEASSHOW_COMMANDS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpMeasShow(MEASSHOW_COMMANDS Show);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasSize(short cx, short cy);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 MEAS_TOOLS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpMeasTool(MEAS_TOOLS tool);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasTag(short Id, short OnOff);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 MEASGET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpMeasGet(MEASGET_ATTRIBUTES Command_Renamed, short wParam, ref int lpParam);
        [DllImport("IPC32", EntryPoint = "IpMeasGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for GETNAME 
        //UPGRADE_WARNING: 構造体 MEASGETSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpMeasGetStr(MEASGETSTR_ATTRIBUTES sCmd, short sParam, string lpParam);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 MEASLOAD_COMMANDS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpMeasLoad(string MeasFile, MEASLOAD_COMMANDS loadMode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasSave(string MeasFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasAddMeasure(short Feature, short Measure, float TargetVal, float MinTol, float MaxTol);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasDelMeasure(short index);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 POINTAPI に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 MEAS_TOOLS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpMeasAdd(MEAS_TOOLS tool, short numpoints, ref POINTAPI lpFeaturePoints);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMeasUpdate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for SETNAME 
        //UPGRADE_WARNING: 構造体 MEASSETSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpMeasAttrStr(MEASSETSTR_ATTRIBUTES Attr, short index, string Value);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 MEAS_TOOLS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpMeasGetHit(MEAS_TOOLS FeatureType, string PromptString);

        //----------------------------------------------------------------- 
        // Manual Tagging 
        //----------------------------------------------------------------- 

        // commands for IpTagGet 
        public enum TAGGET_ATTRIBUTES
        {
            TAGGET_GETNUMPTS = Ipc32v5.GETNUMPTS,
            TAGGET_GETPOINTS = Ipc32v5.GETPOINTS,
            TAGGET_GETNUMCLASS = Ipc32v5.GETNUMCLASS,
            TAGGET_GETSTATS = Ipc32v5.GETSTATS
        }

        // commands for IpTagGetStr 
        public enum TAGGETSTR_ATTRIBUTES
        {
            TAGGETSTR_CLASSNAME = Ipc32v5.TAG_CLASSNAME
        }

        // attributes for IpTagAttr 
        public enum TAGSET_ATTRIBUTES
        {
            TAGSET_VIEW_COUNTS = Ipc32v5.TAG_VIEW_COUNTS,
            TAGSET_VIEW_POINTS = Ipc32v5.TAG_VIEW_POINTS,
            TAGSET_VIEW_CLASSSTATS = Ipc32v5.TAG_VIEW_CLASSSTATS,
            TAGSET_VIEW_AREA = Ipc32v5.TAG_VIEW_AREA,
            TAGSET_VIEW_LABEL = Ipc32v5.TAG_VIEW_LABEL,
            TAGSET_VIEW_MARKER = Ipc32v5.TAG_VIEW_MARKER,
            TAGSET_MEAS_XPOS = Ipc32v5.TAG_MEAS_XPOS,
            TAGSET_MEAS_YPOS = Ipc32v5.TAG_MEAS_YPOS,
            TAGSET_MEAS_INTENSITY = Ipc32v5.TAG_MEAS_INTENSITY,
            TAGSET_MEAS_CLASS = Ipc32v5.TAG_MEAS_CLASS,
            TAGSET_MEAS_RED = Ipc32v5.TAG_MEAS_RED,
            TAGSET_MEAS_GREEN = Ipc32v5.TAG_MEAS_GREEN,
            TAGSET_MEAS_BLUE = Ipc32v5.TAG_MEAS_BLUE,
            TAGSET_MEAS_AREA = Ipc32v5.TAG_MEAS_AREA,
            TAGSET_MEAS_RADIUS = Ipc32v5.TAG_MEAS_RADIUS,
            TAGSET_ACTIVECLASS = Ipc32v5.TAG_ACTIVECLASS
        }
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //UPGRADE_WARNING: 構造体 TAGSET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpTagAttr(TAGSET_ATTRIBUTES bAttr, short Value);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagDelete(short index);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagLoadPoints(string PointsFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagSaveData(string DataFile, short saveMode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagSavePoints(string PointsFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagSaveEnv(string PointsFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagLoadEnv(string PointsFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagUpdate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 TAGGET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpTagGet(TAGGET_ATTRIBUTES Command_Renamed, short wParam, ref int lpParam);
        [DllImport("IPC32", EntryPoint = "IpTagGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 TAGGETSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpTagGetStr(TAGGETSTR_ATTRIBUTES Command_Renamed, short wParam, string lpParam);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagPt(short xPos, short yPos, short PointClass);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagDeleteClass(short PointClass);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTagAddClass(string ClassName);

        //----------------------------------------------------------------- 
        // Arithmetics and logical operations 
        //----------------------------------------------------------------- 

        // commands for IpOpImageArithmetics and IpOpNumberArithmetics 
        public enum OPARITH_COMMANDS
        {
            OPACMD_ADD = Ipc32v5.OPA_ADD,
            OPACMD_SUB = Ipc32v5.OPA_SUB,
            OPACMD_DIFF = Ipc32v5.OPA_DIFF,
            OPACMD_MULT = Ipc32v5.OPA_MULT,
            OPACMD_DIV = Ipc32v5.OPA_DIV,
            OPACMD_AVG = Ipc32v5.OPA_AVG,
            OPACMD_MAX = Ipc32v5.OPA_MAX,
            OPACMD_MIN = Ipc32v5.OPA_MIN,
            OPACMD_ACC = Ipc32v5.OPA_ACC,
            OPACMD_INV = Ipc32v5.OPA_INV,
            OPACMD_X2 = Ipc32v5.OPA_X2,
            OPACMD_SQR = Ipc32v5.OPA_SQR,
            OPACMD_LOG = Ipc32v5.OPA_LOG,
            OPACMD_EXP = Ipc32v5.OPA_EXP,
            OPACMD_X2Y = Ipc32v5.OPA_X2Y,
            OPACMD_SET = Ipc32v5.OPA_SET
        }

        // commands for IpOpImageLogic and IpOpNumberLogic 
        public enum OPLOGIC_COMMANDS
        {
            OPLCMD_AND = Ipc32v5.OPL_AND,
            OPLCMD_OR = Ipc32v5.OPL_OR,
            OPLCMD_XOR = Ipc32v5.OPL_XOR,
            OPLCMD_NAND = Ipc32v5.OPL_NAND,
            OPLCMD_NOR = Ipc32v5.OPL_NOR,
            OPLCMD_NOT = Ipc32v5.OPL_NOT,
            OPLCMD_COPY = Ipc32v5.OPL_COPY
        }
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpOpBkgndCorrect(short WsBackId, short BlackLevel, short bNewImage);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOpBkgndSubtract(short WsBackId, short bNewImage);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 OPARITH_COMMANDS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpOpImageArithmetics(short WsId, float Number, OPARITH_COMMANDS OpaCode, short bNewImage);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 OPLOGIC_COMMANDS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpOpImageLogic(short WsId, OPLOGIC_COMMANDS OplCode, short bNewImage);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 OPARITH_COMMANDS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpOpNumberArithmetics(float Number, OPARITH_COMMANDS OpaCode, short bNewImage);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 OPLOGIC_COMMANDS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpOpNumberLogic(short Number, OPLOGIC_COMMANDS OplCode, short bNewImage);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOpShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 OPARITH_COMMANDS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpOpNumberRgb(ref float Number, OPARITH_COMMANDS OpaCode, short bNewImage);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Restricted Dilation 
        //----------------------------------------------------------------- 
        public static extern int IpFltRstrDilate(short WsId, short nThresh, short nDilateType, short nIterations);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFltRstrDilateShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Palette tool 
        //----------------------------------------------------------------- 
        public static extern int IpPalShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPalSetPaletteColor(short PaletteIndex, short Red, short Green, short Blue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPalSetGrayBrush(short bForeGround, short GrayIndex);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPalSetPaletteBrush(short bForeGround, short PaletteIndex);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPalSetRGBBrush(short bForeGround, short Red, short Green, short Blue);

        //----------------------------------------------------------------- 
        // Line profile 
        //----------------------------------------------------------------- 

        // commands for IpProfGet 
        public enum PROFGET_ATTRIBUTES
        {
            PROFGET_GETNUMPTS = Ipc32v5.GETNUMPTS,
            PROFGET_GETPOINTS = Ipc32v5.GETPOINTS,
            PROFGET_GETSTATS = Ipc32v5.GETSTATS,
            PROFGET_GETVALUES = Ipc32v5.GETVALUES,
            PROFGET_GETRANGE = Ipc32v5.GETRANGE,
            PROFGET_GETINDEX = Ipc32v5.GETINDEX,
            PROFGET_GETCHANNELS = Ipc32v5.GETCHANNELS
        }

        // attributes for IpProfSetAttr 
        public enum PROFSET_ATTRIBUTES
        {
            PROFSET_COLORMODEL = Ipc32v5.COLORMODEL,
            PROFSET_AUTOUPDATE = Ipc32v5.AUTOUPDATE,
            PROFSET_SCAL = Ipc32v5.SCAL,
            PROFSET_ICAL = Ipc32v5.ICAL,
            PROFSET_LINETYPE = Ipc32v5.LINETYPE,
            PROFSET_STATISTICS = Ipc32v5.STATISTICS,
            PROFSET_ORIGIN = Ipc32v5.ORIGIN,
            PROFSET_GRID = Ipc32v5.GRID,
            PROFSET_REFERENCE = Ipc32v5.REFERENCE,
            PROFSET_FREEZE = Ipc32v5.FREEZE,
            PROFSET_CHANNEL1 = Ipc32v5.CHANNEL1,
            PROFSET_CHANNEL2 = Ipc32v5.CHANNEL2,
            PROFSET_CHANNEL3 = Ipc32v5.CHANNEL3,
            PROFSET_LINEGEOMETRY = Ipc32v5.LINEGEOMETRY
        }
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpProfCreate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfDestroy();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfLineMove(short x1, short y1, short x2, short y2);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfMaximize();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfMinimize();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfMove(short x, short y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfRestore();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfSave(string FileName, short bAppend);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfSelect(short ProfId);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 PROFSET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpProfSetAttr(PROFSET_ATTRIBUTES AttrType, short AttrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfSize(short cx, short cy);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpProfUpdate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 PROFGET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpProfGet(PROFGET_ATTRIBUTES Command_Renamed, short wParam, ref int lpParam);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 POINTAPI に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpProfSetFreeForm(short numpoints, ref POINTAPI Points);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Printing 
        //----------------------------------------------------------------- 
        public static extern int IpPrtHalftone(short bUsePrtHalftone, short bUsePrtScaling, short HalftoneType, short HaltoneOption);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPrtPage(short PageNo, short bUsePrtComp, short Copies);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_NOTE: Left は Left_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpPrtSize(short bActual, short bCentered, float Top, float Left_Renamed, float Width, float Height, short bSmooth);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPrtScreen(short PageNo, short bUsePrtComp, short Copies);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Pseudo color 
        //----------------------------------------------------------------- 
        public static extern int IpPcDefineColorSpread(short ColorSpread, int ClrFrom, int ClrTo, short Method);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcLoad(string PseudoColorFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcSave(string PseudoColorFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcSaveData(string FileName, short flag);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcSetColor(short DivNo, short Red, short Green, short Blue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcSetColorSpread(short ColorSpread);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcSetDivisions(short Divisions);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcSetRange(short DivNo, short FromVal, short ToVal);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcTint(short Tint);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPcApplyDyeTint(string DyeFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // scan functions 
        //----------------------------------------------------------------- 
        public static extern int IpScanShow();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpScanSelect();

        //----------------------------------------------------------------- 
        // color segmentation functions 
        //----------------------------------------------------------------- 

        // attributes for IpSegSetAttr 
        public enum SEGSET_ATTRIBUTES
        {
            SEGSET_COLORMODEL = Ipc32v5.COLORMODEL,
            SEGSET_CHANNEL = Ipc32v5.Channel,
            SEGSET_CLR_RED = Ipc32v5.SEGCLR_RED,
            SEGSET_CLR_GREEN = Ipc32v5.SEGCLR_GREEN,
            SEGSET_CLR_BLUE = Ipc32v5.SEGCLR_BLUE,
            SEGSET_SETCURSEL = Ipc32v5.SETCURSEL,
            SEGSET_INVERTY = Ipc32v5.INVERT,
            SEGSET_SEGMETHOD = Ipc32v5.SEGMETHOD,
            SEGSET_CURSORSIZE = Ipc32v5.CURSORSIZE,
            SEGSET_DEGREE = Ipc32v5.DEGREE,
            SEGSET_THRESHOLD = Ipc32v5.Threshold,
            SEGSET_SENSITIVITY = Ipc32v5.SEG_SENSITIVITY
        }
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpSegCreateMask(short MaskType, short MaskMethod, short MaskClass);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegLoad(string ColorRangesFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegMerge(string ColorRangesFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegReset();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegSave(string ColorRangesFile, short bHSI);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegPreview(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 SEGSET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpSegSetAttr(SEGSET_ATTRIBUTES AttrType, short AttrValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegNew(string ClassName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegDelete(string ClassName, short index);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegRename(short index, string ClassName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegSetRange(short nChannel, float FromVal, float ToVal);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegSelect(short SelectionType, short sensitivity);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegSelectArea(short SelectionType, short sensitivity, short xPos, short yPos, short nSize);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSegGetRange(short nChannel, ref float FromVal, ref float ToVal);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Workspace functions 
        //----------------------------------------------------------------- 
        public static extern int IpWsChangeDescription(short DescriptionType, string Description);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsChangeInfo(short InfoType, short Info);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertFile(string DstFile, string DstFormat, string SrcFile, string SrcFormat, short Compr, short imClass, short HalfType, short HalfOpt, short Dpi);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToBilevel(short HalftoneType, short Screen, short OutputDpi);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToFloat();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToGray();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToGray12();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToGray16();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToPaletteMColor();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToPaletteMedian(short StartIndex, short NumColors);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToRGB();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertImage(short iType, short Method, int InStart, int InEnd, int OutStart, int OutEnd);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsCopy();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsCreate(short Width, short Height, short Dpi, short iClass);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsCreateFromClipboard();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsDuplicate();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsFill(short FillType, short ColorType, short Transparency);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsFillPattern(string PatternFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // IPCFUNC IpWsGetId(LPSTR WsName, LPSHORT Id); 
        // IPCFUNC IpWsGetName(short Id, LPSTR WsName, short WsNameBufSize); 
        public static extern int IpWsLoad(string FileName, string FileFormat);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsLoadNumber(short Number);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_NOTE: Right は Right_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        //UPGRADE_NOTE: Left は Left_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpWsLoadPreview(string FileName, string FileFormat, short Left_Renamed, short Top, short Right_Renamed, short Bottom);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsMove(short x, short y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsOrient(short OrientType);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsOverlay(string SrcImageName, short Transparency, short DoFloatingOverlay);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsOverlayEx(short SrcImgDocID, short x, short y, short Transparency, short ApplyType);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsPan(short x, short y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsPaste(short x, short y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsPasteEx(string prompt, string UndoFunction);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsRedo(short Number);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsReload();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsRotate(float Angle, short Anchor);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsRulerShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsRulerType(short RulerType);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsSave();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsSaveAs(string FileName, string FileFormat);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsSaveEx(string FileName, string FileFormat, short Compression, short BitsPerPlane);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsScale(short Width, short Height, short bSmooth);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsTestStrips(short HorzPage, short VertPage, short iType, short MinValue, short MaxValue, short Reduction, short bRed, short bGreen, short bBlue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsTestStrips2(short HorzPage, short VertPage, short Type1, short MinValue1, short MaxValue1, short Type2, short MinValue2, short MaxValue2, short Reduction, short bRed,
        short bGreen, short bBlue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsTestStripsHalftone(short AllTypes, short Color, ref short ipHalfTypes, ref short ipHalfScreens, short OutputDpi, short Reduction);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsUndo(short Number);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsZoom(short PercentZoom);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsLoadSetRes(short Number);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsCreateFromVri(short Vri, string Name, short mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsGray12To8(short start12, short end12, short start8, short end8);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsGray16To8(short start16, short end16, short start8, short end8);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsStretchLut(short mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsCreateEx(short Width, short Height, short Dpi, short iClass, int lNumFrames);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToRGB36();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToRGB48();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToGrayEx(int start16, int end16, short start8, short end8);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsConvertToRGBEx(int start16, int end16, short start8, short end8);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsCopyFrames(int lStart, int lNumber);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsCutFrames(int lStart, int lNumber);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsDeleteFrames(int lStart, int lNumber);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsPasteFrames(int lPosition);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsSelectFrames(int lStart, int lNumber);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpWsSubSampleFrames(int lOffset, int lInterval);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Template Mode functions 
        //----------------------------------------------------------------- 
        public static extern int IpTemplateMode(short OnOff);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Macro Management functions 
        //----------------------------------------------------------------- 
        public static extern int IpMacroRun(string macroName, string scriptFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMacroLoad(string scriptFile);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMacroStop(string Message, short sType);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMacroWait(short delay);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMacroPause(string Message, short sType, int delay);

        //----------------------------------------------------------------- 
        // Miscellaneous 
        //----------------------------------------------------------------- 

        // commands for IpIniFile() 
        public enum INI_COMMANDS
        {
            INICMD_GETINT = Ipc32v5.GETINT,
            INICMD_GETFLOAT = Ipc32v5.GETFLOAT,
            INICMD_SETINT = Ipc32v5.SETINT,
            INICMD_SETFLOAT = Ipc32v5.SETFLOAT
        }

        // commands for IpIniFileStr() 
        public enum INISTR_COMMANDS
        {
            INICMDSTR_GETSTRING = Ipc32v5.GETSTRING,
            INICMDSTR_SETSTRING = Ipc32v5.SETSTRING
        }
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpStAutoName(object Format, short Number, string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_NOTE: Filter は Filter_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpStSearchDir(string Directory, string Filter_Renamed, short Number, string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_NOTE: Filter は Filter_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        //UPGRADE_NOTE: Default は Default_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpStGetName(string title, string Default_Renamed, string Filter_Renamed, string FileName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpStGetString(string prompt, string istring, short maxlen);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpStGetInt(string prompt, ref short Value, short initval, short min, short max);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 POINTAPI に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpListPts(ref POINTAPI Points, string istring);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMorePts(string istring);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpStGetFloat(string prompt, ref float Value, float initval, float min, float max, float inc);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTrackBar(short Cmd, short tValue, string caption);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 INI_COMMANDS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpIniFile(INI_COMMANDS Cmd, string ValName, ref int lpVal);
        [DllImport("IPC32", EntryPoint = "IpIniFile", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for GETSTRING and SETSTRING 
        //UPGRADE_WARNING: 構造体 INISTR_COMMANDS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpIniFileStr(INISTR_COMMANDS sCmd, string lpName, string lpValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpStGetLong(string prompt, ref int Value, int initval, int min, int max);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpStGetDouble(string prompt, ref double Value, double initval, double min, double max, double inc);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Registration 
        //----------------------------------------------------------------- 
        public static extern int IpRegShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 POINTAPI に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 POINTAPI に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpRegister(ref POINTAPI FromPoints, ref POINTAPI ToPoints, short numpoints, short AffCode);

        //----------------------------------------------------------------- 
        // Macro Output functions 
        //----------------------------------------------------------------- 

        // commands for IpIniFile() 
        public enum OUTSET_ATTRIBUTES
        {
            OUTSET_SETTABS = Ipc32v5.SETTABS
        }
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpOutput(string Message);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOutputShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOutputClear();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpOutputSave(string FileName, short mode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 OUTSET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpOutputSet(OUTSET_ATTRIBUTES sCmd, short sParam, ref int lpParam);

        //----------------------------------------------------------------- 
        // Bitmap analysis functions 
        //----------------------------------------------------------------- 

        // for IpBitAttr() 
        public enum BITSET_ATTRIBUTES
        {
            BITSET_SAMPLE = Ipc32v5.BIT_SAMPLE,
            BITSET_SAVEALL = Ipc32v5.BIT_SAVEALL,
            BITSET_CALIB = Ipc32v5.BIT_CALIB
        }
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpBitShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 BITSET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpBitAttr(BITSET_ATTRIBUTES bAttr, short Value);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBitSaveData(string DataFile, short saveMode);

        //----------------------------------------------------------------- 
        // Object Sort 
        //----------------------------------------------------------------- 

        // for IpSortAttr() 
        public enum SORTSET_ATTRIBUTES
        {
            SORTSET_ROTATE = Ipc32v5.SORT_ROTATE,
            SORTSET_MEAS = Ipc32v5.SORT_MEAS,
            SORTSET_LABELS = Ipc32v5.SORT_LABELS,
            SORTSET_COLOR = Ipc32v5.SORT_COLOR,
            SORTSEt_INDEX = Ipc32v5.SORT_INDEX,
            SORTSET_AUTO = Ipc32v5.SORT_AUTO
        }
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //UPGRADE_WARNING: 構造体 SORTSET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpSortAttr(SORTSET_ATTRIBUTES Attr, short Value);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSortShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSortObjects();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpDde(short Cmd, string szStr1, string szStr2);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Draw functions 
        //----------------------------------------------------------------- 

        public static extern int IpAnotAttr(short Attr, int Value);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 POINTAPI に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAnotLine(ref POINTAPI lpPoints, short numpoints, short lineEndType, short bFilled);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 RECT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAnotBox(ref Winapi.RECT lpBoxRect, short bFilled);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 POINTAPI に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAnotEllipse(ref POINTAPI lpCenter, short radx, short rady, short bFilled);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Overlay (and plot) functions 
        //----------------------------------------------------------------- 

        public static extern int IpPlotCreate(string title);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpPlotData(short plotid, short axis, short valueType, ref int values, short Count);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpPlotRange(short plotid, short axis, short valueType, short rangeType, ref int values);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPlotSet(short plotid, string commandString);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPlotShow(short plotid, short sMode);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPlotUpdate(short plotid);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPlotDestroy(short plotid);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 POINTAPI に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDraw(ref POINTAPI Points, short numpoints, short Attrib);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 POINTAPI に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDrawText(string text, ref POINTAPI pos, short Attrib);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDrawClear(short objid);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 POINTAPI に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpGetLine(string Message, ref POINTAPI LinePts, ref short numpoints, short maxpoints, short Attrib);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDrawClearDoc(short docId);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpDrawGet(short Command_Renamed, short objid, ref int lpParam);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpDrawSet(short Command_Renamed, short objid, ref int lpParam);


        public const short DDE_OPEN = 1;
        public const short DDE_CLOSE = 2;
        public const short DDE_PUT = 3;
        public const short DDE_GET = 4;
        public const short DDE_EXEC = 5;
        public const short DDE_SET = 6;

        // for IpBlbSetFilterRange 
        public const int CALIB_UNIT = 0x4000;

        // print functions 
        public const short P_GRAPH = 1;
        public const short P_TABLE = 2;
        public const short P_IMAGE = 3;

        public const short TXT_BOLD = 1;
        public const short TXT_UNDERLINE = 2;
        public const short TXT_ITALIC = 3;
        public const short TXT_STRIKEOUT = 4;
        public const short TXT_DROPSHADOW = 5;
        public const short TXT_ENCLOSED = 6;
        public const short TXT_SPACING = 7;
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpTextShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 POINTAPI に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpTextBurn(string textStr, ref POINTAPI textPos);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTextSetAttr(short Attr, short Value);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTextFont(string fontName, short fontSize);

        //----------------------------------------------------------------- 
        // Annotation Functions and Constants 
        //----------------------------------------------------------------- 

        // Graph Object Types 
        public const short GO_OBJ_LINE = 1;
        public const short GO_OBJ_RECT = 2;
        public const short GO_OBJ_ROUNDRECT = 3;
        public const short GO_OBJ_ELLIPSE = 4;
        public const short GO_OBJ_TEXT = 5;
        public const short GO_OBJ_POLY = 6;

        public enum GOOBJ_TYPES
        {
            GOOBJTYPE_LINE = Ipc32v5.GO_OBJ_LINE,
            GOOBJTYPE_RECT = Ipc32v5.GO_OBJ_RECT,
            GOOBJTYPE_ROUNDRECT = Ipc32v5.GO_OBJ_ROUNDRECT,
            GOOBJTYPE_ELLIPSE = Ipc32v5.GO_OBJ_ELLIPSE,
            GOOBJTYPE_TEXT = Ipc32v5.GO_OBJ_TEXT,
            GOOBJTYPE_POLY = Ipc32v5.GO_OBJ_POLY
        }

        // Graph Object Attributes 
        public const short GO_ATTR_PENCOLOR = 1;
        public const short GO_ATTR_BRUSHCOLOR = 2;
        public const short GO_ATTR_TEXTCOLOR = 3;
        public const short GO_ATTR_PENWIDTH = 4;
        public const short GO_ATTR_PENSTYLE = 5;
        public const short GO_ATTR_RECTSTYLE = 6;
        public const short GO_ATTR_LINESTART = 7;
        public const short GO_ATTR_LINEEND = 8;
        public const short GO_ATTR_ZOOM = 9;
        public const short GO_ATTR_CONNECT = 10;
        public const short GO_ATTR_TEXTWORDWRAP = 11;
        public const short GO_ATTR_TEXTCENTERED = 12;
        public const short GO_ATTR_TEXTAUTOSIZE = 13;
        public const short GO_ATTR_USEASDEFAULT = 14;
        public const short GO_ATTR_FONTSIZE = 15;
        public const short GO_ATTR_FONTBOLD = 16;
        public const short GO_ATTR_FONTITALIC = 17;
        public const short GO_ATTR_FONTUNDERLINE = 18;
        public const short GO_ATTR_TEXTLENGTH = 19;
        public const short GO_ATTR_TEXT = 20;
        public const short GO_ATTR_NUMPOINTS = 21;
        public const short GO_ATTR_POINTS = 22;

        public enum GOOBJSET_ATTRIBUTES
        {
            GOOBJSET_PENCOLOR = Ipc32v5.GO_ATTR_PENCOLOR,
            GOOBJSET_BRUSHCOLOR = Ipc32v5.GO_ATTR_BRUSHCOLOR,
            GOOBJSET_TEXTCOLOR = Ipc32v5.GO_ATTR_TEXTCOLOR,
            GOOBJSET_PENWIDTH = Ipc32v5.GO_ATTR_PENWIDTH,
            GOOBJSET_PENSTYLE = Ipc32v5.GO_ATTR_PENSTYLE,
            GOOBJSET_RECTSTYLE = Ipc32v5.GO_ATTR_RECTSTYLE,
            GOOBJSET_LINESTART = Ipc32v5.GO_ATTR_LINESTART,
            GOOBJSET_LINEEND = Ipc32v5.GO_ATTR_LINEEND,
            GOOBJSET_ZOOM = Ipc32v5.GO_ATTR_ZOOM,
            GOOBJSET_CONNECT = Ipc32v5.GO_ATTR_CONNECT,
            GOOBJSET_TEXTWORDWRAP = Ipc32v5.GO_ATTR_TEXTWORDWRAP,
            GOOBJSET_TEXTCENTERED = Ipc32v5.GO_ATTR_TEXTCENTERED,
            GOOBJSET_TEXTAUTOSIZE = Ipc32v5.GO_ATTR_TEXTAUTOSIZE,
            GOOBJSET_USEASDEFAULT = Ipc32v5.GO_ATTR_USEASDEFAULT,
            GOOBJSET_FONTSIZE = Ipc32v5.GO_ATTR_FONTSIZE,
            GOOBJSET_FONTBOLD = Ipc32v5.GO_ATTR_FONTBOLD,
            GOOBJSET_FONTITALIC = Ipc32v5.GO_ATTR_FONTITALIC,
            GOOBJSET_FONTUNDERLINE = Ipc32v5.GO_ATTR_FONTUNDERLINE
        }

        public const short GO_OBJ_NUMBER = 100;
        public const short GO_OBJ_INDEX = 101;
        public const short GO_SEL_NUMBER = 102;
        public const short GO_SEL_INDEX = 103;
        public const short GO_OBJ_TYPE = 104;

        public enum GOOBJGET_ATTRIBUTES
        {
            GOOBJGET_PENCOLOR = Ipc32v5.GO_ATTR_PENCOLOR,
            GOOBJGET_BRUSHCOLOR = Ipc32v5.GO_ATTR_BRUSHCOLOR,
            GOOBJGET_TEXTCOLOR = Ipc32v5.GO_ATTR_TEXTCOLOR,
            GOOBJGET_PENWIDTH = Ipc32v5.GO_ATTR_PENWIDTH,
            GOOBJGET_PENSTYLE = Ipc32v5.GO_ATTR_PENSTYLE,
            GOOBJGET_RECTSTYLE = Ipc32v5.GO_ATTR_RECTSTYLE,
            GOOBJGET_LINESTART = Ipc32v5.GO_ATTR_LINESTART,
            GOOBJGET_LINEEND = Ipc32v5.GO_ATTR_LINEEND,
            GOOBJGET_ZOOM = Ipc32v5.GO_ATTR_ZOOM,
            GOOBJGET_CONNECT = Ipc32v5.GO_ATTR_CONNECT,
            GOOBJGET_TEXTWORDWRAP = Ipc32v5.GO_ATTR_TEXTWORDWRAP,
            GOOBJGET_TEXTCENTERED = Ipc32v5.GO_ATTR_TEXTCENTERED,
            GOOBJGET_TEXTAUTOSIZE = Ipc32v5.GO_ATTR_TEXTAUTOSIZE,
            GOOBJGET_USEASDEFAULT = Ipc32v5.GO_ATTR_USEASDEFAULT,
            GOOBJGET_FONTSIZE = Ipc32v5.GO_ATTR_FONTSIZE,
            GOOBJGET_FONTBOLD = Ipc32v5.GO_ATTR_FONTBOLD,
            GOOBJGET_FONTITALIC = Ipc32v5.GO_ATTR_FONTITALIC,
            GOOBJGET_FONTUNDERLINE = Ipc32v5.GO_ATTR_FONTUNDERLINE,
            GOOBJGET_TEXTLENGTH = Ipc32v5.GO_ATTR_TEXTLENGTH,
            GOOBJGET_NUMPOINTS = Ipc32v5.GO_ATTR_NUMPOINTS,
            GOOBJGET_POINTS = Ipc32v5.GO_ATTR_POINTS,
            GOOBJGET_OBJ_NUMBER = Ipc32v5.GO_OBJ_NUMBER,
            GOOBJGET_OBJ_INDEX = Ipc32v5.GO_OBJ_INDEX,
            GOOBJGET_SEL_NUMBER = Ipc32v5.GO_SEL_NUMBER,
            GOOBJGET_SEL_INDEX = Ipc32v5.GO_SEL_INDEX,
            GOOBJGET_OBJ_TYPE = Ipc32v5.GO_OBJ_TYPE
        }

        public enum GOOBJGETSTR_ATTRIBUTES
        {
            GOOBJATTR_TEXT = Ipc32v5.GO_ATTR_TEXT
        }

        // Pen Styles, used as the value with the GOOBJSET_PENSTYLE attribute 
        // ------- 
        public const short GO_PENSTYLE_SOLID = 0;
        // ------- 
        public const short GO_PENSTYLE_DASH = 1;
        // ....... 
        public const short GO_PENSTYLE_DOT = 2;
        // _._._._ 
        public const short GO_PENSTYLE_DASHDOT = 3;
        // _.._.._ 
        public const short GO_PENSTYLE_DASHDOTDOT = 4;

        // Rect Styles, used as the value with the GOOBJSET_RECTSTYLE attribute 
        public const short GO_RECTSTYLE_BORDER_NOFILL = 0;
        public const short GO_RECTSTYLE_BORDER_FILL = 1;
        public const short GO_RECTSTYLE_NOBORDER_FILL = 2;
        public const short GO_RECTSTYLE_NOBORDER_NOFILL = 3;

        // Line Endings, used as the value with the GOOBJSET_LINESTART and GOOBJSET_LINEEND attributes 
        public const short GO_LINEEND_NOTHING = 0;
        public const short GO_LINEEND_SMALLARROW = 1;
        public const short GO_LINEEND_LARGEARROW = 2;
        public const short GO_LINEEND_SMALLDIAMOND = 3;
        public const short GO_LINEEND_LARGEDIAMOND = 4;
        public const short GO_LINEEND_CIRCLE = 5;
        public const short GO_LINEEND_SMALLTICKMARK = 6;
        public const short GO_LINEEND_LARGETICKMARK = 7;
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Annotation Functions - recorded 
        public static extern int IpAnShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 GOOBJ_TYPES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAnCreateObj(GOOBJ_TYPES nObjType);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnDeleteObj();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnMove(short nHandle, int x, int y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 GOOBJSET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAnSet(GOOBJSET_ATTRIBUTES nAttr, int nValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 GOOBJGET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAnGet(GOOBJGET_ATTRIBUTES nAttr, ref int lpValue);
        [DllImport("IPC32", EntryPoint = "IpAnGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 GOOBJGETSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAnGetStr(GOOBJGETSTR_ATTRIBUTES nAttr, string lpValue);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnText(string szText);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnAddText(string szText);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnSetFontName(string szFontName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnGetFontName(string szFontName);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 POINTAPI に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAnPolyAddPtArray(ref POINTAPI Points, short nCount);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnBurn();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnShowAnnot(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnActivateAll();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Annotation Functions - not recorded 
        public static extern int IpAnActivateObjID(int nObjID);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnActivateObjXY(short x, short y);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 GOOBJ_TYPES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAnActivateDefaultObj(GOOBJ_TYPES nObjType);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnPolyAddPtString(string szPoints);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAnDeleteAll();
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Workflow toolbar Functions and Constants 
        //----------------------------------------------------------------- 
        public static extern int IpToolbarShow(short bShow);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpToolbarSelect(string szToolbar);
        [DllImport("IPC32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpToolbarGetStr(short nAttr, string CurrValue);


        // 
        // Alignment and Tiling functions and constants 
        // 
        // Copyright (c) 2003 Media Cybernetics, Inc. 
        // 



        // if this include file is included by task.h, only one of these set of macros should 
        // be defined. 
        // Commands 

        // Integer arguments (Get/Set). sParam is ignored unless specified. 
        // Set the method for alignment calculations 
        public const short ALGN_ALGORITHM = 100;
        // Set the number of angles - must be a power of two 
        public const short ALGN_ANGLE_NUM = 110;
        // Set the number of scales - must be a power of two 
        public const short ALGN_SCALE_NUM = 120;
        // Set options; scale/rotate/translate 
        public const short ALGN_OPTIONS = 130;
        // Order the images as per calibrated positions 
        public const short ALGN_CAL_ORDER = 140;
        // Which frame in the list to use as the fixed anchor 
        public const short ALGN_REF_FRAME = 150;
        // Algorithm specific option 
        public const short ALGN_ALG_OPTION = 160;
        // (get) number of frames in list 
        public const short ALGN_GETNUMFRAMES = 170;
        // (get) list of frames 
        public const short ALGN_GETFRAMELST = 180;
        // Trim image borders down to fully overlapping frames 
        public const short ALGN_TRIMBORDERS = 190;
        // GETNUMDOC (get) number of images in list 
        // GETDOCLST (get) list of doc ID's, max count sParam 
        // Determine whether or not UI is updated 
        public const short ALGN_UPDATEUI = 200;
        // Iterate, setting the results to be the next input 
        public const short ALGN_ITERATE = 210;

        // (get) Bool; All images have consistent spatial calibrations 
        public const short ALGN_HAVECALIB = 220;
        // (get) Bool; All images have required XYZ position information 
        public const short ALGN_HAVEPOSITION = 230;
        // (get) Bool; A solution has been calculated 
        public const short ALGN_CALCSTATE = 240;
        // Bool; always recalculate computed alignment 
        public const short ALGN_ALWAYSRECALC = 250;

        // Floating point arguments (Get/Set) 
        // X pixel shift per image (stacks) 
        public const short ALGN_X_PERIMAGE = 1010;
        // Y pixel shift per image (stacks) 
        public const short ALGN_Y_PERIMAGE = 1020;
        // Calibrated X angle shift (stacks) 
        public const short ALGN_X_CAL_ANGLE = 1030;
        // Calibrated Y angle shift (stacks) 
        public const short ALGN_Y_CAL_ANGLE = 1040;

        // Return value is the number sent back. 
        // These are valid only after IpAlignCalculate( ) is called or 
        // these values are set by a macro call. 

        // Note that these array fetch routines require: 
        // Second parameter is the index (see ALGN_GETNUMFRAMES) 

        // Get only, for each frame, expressing how it is manipulated compared to the 
        // previous frame 
        // Number of matching offsets (short) 
        public const short ALGN_OFFSET_COUNT = 2000;
        // Number of matching angles (short) 
        public const short ALGN_ANGLE_COUNT = 2010;
        // Number of matching scales (short) 
        public const short ALGN_SCALE_COUNT = 2020;

        // Second parameter is the index (see ALGN_GETNUMFRAMES) 
        // List of POINTAPI offsets 
        public const short ALGN_OFFSET_VAL = 2030;
        // List of float matching angles 
        public const short ALGN_ANGLE_VAL = 2040;
        // List of float matching scales 
        public const short ALGN_SCALE_VAL = 2050;

        // Second parameter is the INDEX (see ALGN_GETNUMFRAMES) 
        // List of float relative match values 
        public const short ALGN_OFFSET_RANK = 2060;
        // List of float relative match values 
        public const short ALGN_ANGLE_RANK = 2070;
        // List of float relative match values 
        public const short ALGN_SCALE_RANK = 2080;

        // List of the best alignment values. Second parameter is the index of the 
        // frames, 0 to n-2. DOCSEL_ALL gets/sets the entire list of ALGN_GETNUMFRAMES 
        // values. 
        // List of ALGN_GETNUMFRAMES POINTAPI offsets 
        public const short ALGN_BEST_OFFSET = 2090;
        // List of ALGN_GETNUMFRAMES float matching angles 
        public const short ALGN_BEST_ANGLE = 2100;
        // List of ALGN_GETNUMFRAMES float matching scales 
        public const short ALGN_BEST_SCALE = 2110;

        // ALGN_OPTIONS arguments 
        // Calculate rotation 
        public const short ALGN_ROTATE = 2200;
        // Calculate scaling 
        public const short ALGN_SCALE = 2210;
        // Calculate translation 
        public const short ALGN_TRANSLATE = 2220;

        // ALGN_ALGORITHM arguments. Additional methods can be 
        // added here, with ALGN_ALG_OPTION arguments for algorithm 
        // specific settings. 
        // FFT correlation 
        public const short ALGN_FFT = 1;
        // User specified offsets 
        public const short ALGN_USER = 2;

        // ALGN_ALG_OPTION constants for ALGN_FFT, specific to that algorithm 
        // Internal algorithm 
        public const short ALGN_METHOD = 1;
        // Number of angles of rotation (power of 2) 
        public const short ALGN_FFT_NANGLES = 2;
        // Number of scales (power of 2) 
        public const short ALGN_FFT_NSCALES = 3;
        // Short, percentage blurring distance from edge of image 
        public const short ALGN_FFT_APODIZE = 4;

        // ALGN_ALG_OPTION, ALGN_METHOD constants for ALGN_FFT, specific to that algorithm 
        // Set to FFT full correlation 
        public const short ALGN_FFTFULL = 1;
        // Set to FFT phase correlation 
        public const short ALGN_FFTPHASE = 2;

        // ALGN_ALG_OPTION calls for ALGN_USER, specific to that algorithm 
        // X shift per plane (float) 
        public const short ALGN_USER_X = 1;
        // Y shift per plane (float) 
        public const short ALGN_USER_Y = 2;
        // X shift angle (float, degrees) 
        public const short ALGN_USER_XANGLE = 3;
        // Y shift angle (float, degrees) 
        public const short ALGN_USER_YANGLE = 4;
        // X spacing 
        public const short ALGN_USER_XDIST = 5;
        // Y spacing 
        public const short ALGN_USER_YDIST = 6;
        // Z spacing 
        public const short ALGN_USER_ZDIST = 7;

        // IpAlignShow arguments 
        public const short ALGN_IMAGETAB = 1;
        public const short ALGN_OPTIONTAB = 2;
        public const short ALGN_ADJUST = 3;

        //constants for OutParam in IpAlignFindPattern 
        // X coordinate of found object 
        public const short ALGN_PM_OUT_X = 0;
        // Y coordinate of found object 
        public const short ALGN_PM_OUT_Y = 1;
        // angle of the best match in radians 
        public const short ALGN_PM_OUT_ANGLE = 2;
        // scale of the best match 
        public const short ALGN_PM_OUT_SCALE = 3;
        // rank value of the pattern match 
        public const short ALGN_PM_OUT_RANK = 4;
        // size of ALGN_PM_OUT array per point 
        public const short ALGN_PM_OUT_SIZE = 5;

        // Alignment enumerations 
        // attributes for IpAlignSetInt 
        public enum ALIGNSET_SET_INT
        {
            ALIGNSET_ALGORITHM = Ipc32v5.ALGN_ALGORITHM,
            // Set the method for alignment calculations 
            ALIGNSET_ANGLE_NUM = Ipc32v5.ALGN_ANGLE_NUM,
            // Set the number of angles - must be a power of two 
            ALIGNSET_SCALE_NUM = Ipc32v5.ALGN_SCALE_NUM,
            // Set the number of scales - must be a power of two 
            ALIGNSET_OPTIONS = Ipc32v5.ALGN_OPTIONS,
            // Set options; scale/rotate/translate 
            ALIGNSET_CAL_ORDER = Ipc32v5.ALGN_CAL_ORDER,
            // Order the images as per calibrated positions 
            ALIGNSET_REF_FRAME = Ipc32v5.ALGN_REF_FRAME,
            // Which frame in the list to use as the fixed anchor 
            ALIGNSET_ALG_OPTION = Ipc32v5.ALGN_ALG_OPTION,
            // Algorithm specific option 
            ALIGNSET_TRIMBORDERS = Ipc32v5.ALGN_TRIMBORDERS,
            // Trim image borders down to fully overlapping frames 
            ALIGNSET_UPDATEUI = Ipc32v5.ALGN_UPDATEUI,
            // Determine whether or not UI is updated 
            ALIGNSET_ITERATE = Ipc32v5.ALGN_ITERATE,
            // Iterate, setting the results to be the next input 
            ALIGNSET_ALWAYSRECALC = Ipc32v5.ALGN_ALWAYSRECALC
            // Bool; always recalculate computed alignment 
        }

        // attributes for IpAlignSetSingle 
        public enum ALIGNSET_SET_SINGLE
        {
            ALIGNSET_X_PERIMAGE = Ipc32v5.ALGN_X_PERIMAGE,
            // X pixel shift per image (stacks) 
            ALIGNSET_Y_PERIMAGE = Ipc32v5.ALGN_Y_PERIMAGE,
            // Y pixel shift per image (stacks) 
            ALIGNSET_X_CAL_ANGLE = Ipc32v5.ALGN_X_CAL_ANGLE,
            // Calibrated X angle shift (stacks) 
            ALIGNSET_Y_CAL_ANGLE = Ipc32v5.ALGN_Y_CAL_ANGLE
            // Calibrated Y angle shift (stacks) 
        }

        // attributes for IpAlignSetEx 
        // Second parameter is the index (see ALGN_GETNUMFRAMES) 
        // Second parameter is the INDEX (see ALGN_GETNUMFRAMES) 
        // List of the best alignment values. Second parameter is the index of the 
        // frames, 0 to n-2. DOCSEL_ALL gets/sets the entire list of ALGN_GETNUMFRAMES 
        // values. 
        public enum ALIGNSET_SET_EX
        {
            ALIGNSET_OFFSET_VAL = Ipc32v5.ALGN_OFFSET_VAL,
            // List of POINTAPI offsets 
            ALIGNSET_ANGLE_VAL = Ipc32v5.ALGN_ANGLE_VAL,
            // List of float matching angles 
            ALIGNSET_SCALE_VAL = Ipc32v5.ALGN_SCALE_VAL,
            // List of float matching scales 
            ALIGNSET_OFFSET_RANK = Ipc32v5.ALGN_OFFSET_RANK,
            // List of float relative match values 
            ALIGNSET_ANGLE_RANK = Ipc32v5.ALGN_ANGLE_RANK,
            // List of float relative match values 
            ALIGNSET_SCALE_RANK = Ipc32v5.ALGN_SCALE_RANK,
            // List of float relative match values 
            ALIGNSET_BEST_OFFSET = Ipc32v5.ALGN_BEST_OFFSET,
            // List of ALGN_GETNUMFRAMES POINTAPI offsets 
            ALIGNSET_BEST_ANGLE = Ipc32v5.ALGN_BEST_ANGLE,
            // List of ALGN_GETNUMFRAMES float matching angles 
            ALIGNSET_BEST_SCALE = Ipc32v5.ALGN_BEST_SCALE
            // List of ALGN_GETNUMFRAMES float matching scales 
        }

        // attributes for IpAlignGet 
        // Floating point arguments (Get/Set) 
        // Get only, for each frame, expressing how it is manipulated compared to the 
        // previous frame 
        // Second parameter is the index (see ALIGNGET_GETNUMFRAMES) 
        // Second parameter is the INDEX (see ALIGNGET_GETNUMFRAMES) 
        // List of the best alignment values. Second parameter is the index of the 
        // frames, 0 to n-2. DOCSEL_ALL gets/sets the entire list of ALIGNGET_GETNUMFRAMES 
        // values. 
        public enum ALIGNSET_GET
        {
            ALIGNGET_ALGORITHM = Ipc32v5.ALGN_ALGORITHM,
            // Set the method for alignment calculations 
            ALIGNGET_ANGLE_NUM = Ipc32v5.ALGN_ANGLE_NUM,
            // Set the number of angles - must be a power of two 
            ALIGNGET_SCALE_NUM = Ipc32v5.ALGN_SCALE_NUM,
            // Set the number of scales - must be a power of two 
            ALIGNGET_OPTIONS = Ipc32v5.ALGN_OPTIONS,
            // Set options; scale/rotate/translate 
            ALIGNGET_CAL_ORDER = Ipc32v5.ALGN_CAL_ORDER,
            // Order the images as per calibrated positions 
            ALIGNGET_REF_FRAME = Ipc32v5.ALGN_REF_FRAME,
            // Which frame in the list to use as the fixed anchor 
            ALIGNGET_ALG_OPTION = Ipc32v5.ALGN_ALG_OPTION,
            // Algorithm specific option 
            ALIGNGET_GETNUMFRAMES = Ipc32v5.ALGN_GETNUMFRAMES,
            // (get) number of frames in list 
            ALIGNGET_GETFRAMELST = Ipc32v5.ALGN_GETFRAMELST,
            // (get) list of frames 
            ALIGNGET_TRIMBORDERS = Ipc32v5.ALGN_TRIMBORDERS,
            // Trim image borders down to fully overlapping frames 
            ALIGNGET_GETNUMDOC = Ipc32v5.GETNUMDOC,
            // (get) number of images in list 
            ALIGNGET_GETDOCLST = Ipc32v5.GETDOCLST,
            // (get) list of doc ID's, max count sParam 
            ALIGNGET_UPDATEUI = Ipc32v5.ALGN_UPDATEUI,
            // Determine whether or not UI is updated 
            ALIGNGET_ITERATE = Ipc32v5.ALGN_ITERATE,
            // Iterate, setting the results to be the next input 
            ALIGNGET_HAVECALIB = Ipc32v5.ALGN_HAVECALIB,
            // (get) Bool; All images have consistent spatial calibrations 
            ALIGNGET_HAVEPOSITION = Ipc32v5.ALGN_HAVEPOSITION,
            // (get) Bool; All images have required XYZ position information 
            ALIGNGET_CALCSTATE = Ipc32v5.ALGN_CALCSTATE,
            // (get) Bool; A solution has been calculated 
            ALIGNGET_ALWAYSRECALC = Ipc32v5.ALGN_ALWAYSRECALC,
            // Bool; always recalculate computed alignment 
            ALIGNGET_X_PERIMAGE = Ipc32v5.ALGN_X_PERIMAGE,
            // X pixel shift per image (stacks) 
            ALIGNGET_Y_PERIMAGE = Ipc32v5.ALGN_Y_PERIMAGE,
            // Y pixel shift per image (stacks) 
            ALIGNGET_X_CAL_ANGLE = Ipc32v5.ALGN_X_CAL_ANGLE,
            // Calibrated X angle shift (stacks) 
            ALIGNGET_Y_CAL_ANGLE = Ipc32v5.ALGN_Y_CAL_ANGLE,
            // Calibrated Y angle shift (stacks) 
            ALIGNGET_OFFSET_COUNT = Ipc32v5.ALGN_OFFSET_COUNT,
            // Number of matching offsets (short) 
            ALIGNGET_ANGLE_COUNT = Ipc32v5.ALGN_ANGLE_COUNT,
            // Number of matching angles (short) 
            ALIGNGET_SCALE_COUNT = Ipc32v5.ALGN_SCALE_COUNT,
            // Number of matching scales (short) 
            ALIGNGET_OFFSET_VAL = Ipc32v5.ALGN_OFFSET_VAL,
            // List of POINTAPI offsets 
            ALIGNGET_ANGLE_VAL = Ipc32v5.ALGN_ANGLE_VAL,
            // List of float matching angles 
            ALIGNGET_SCALE_VAL = Ipc32v5.ALGN_SCALE_VAL,
            // List of float matching scales 
            ALIGNGET_OFFSET_RANK = Ipc32v5.ALGN_OFFSET_RANK,
            // List of float relative match values 
            ALIGNGET_ANGLE_RANK = Ipc32v5.ALGN_ANGLE_RANK,
            // List of float relative match values 
            ALIGNGET_SCALE_RANK = Ipc32v5.ALGN_SCALE_RANK,
            // List of float relative match values 
            ALIGNGET_BEST_OFFSET = Ipc32v5.ALGN_BEST_OFFSET,
            // List of ALIGNGET_GETNUMFRAMES POINTAPI offsets 
            ALIGNGET_BEST_ANGLE = Ipc32v5.ALGN_BEST_ANGLE,
            // List of ALIGNGET_GETNUMFRAMES float matching angles 
            ALIGNGET_BEST_SCALE = Ipc32v5.ALGN_BEST_SCALE
            // List of ALIGNGET_GETNUMFRAMES float matching scales 
        }

        // attributes for IpAlignShow 
        public enum ALIGNSHOW_COMMANDS
        {
            ALIGNSHOW_IMAGETAB = Ipc32v5.ALGN_IMAGETAB,
            ALIGNSHOW_OPTIONTAB = Ipc32v5.ALGN_OPTIONTAB,
            ALIGNSHOW_ADJUST = Ipc32v5.ALGN_ADJUST
        }
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Align Functions 
        //----------------------------------------------------------------- 

        // Show/hide (1/0) the specified dialog 
        //UPGRADE_WARNING: 構造体 ALIGNSHOW_COMMANDS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAlignShow(ALIGNSHOW_COMMANDS nDialog, short bShow);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Generic Set routine 
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 ALIGNSET_SET_EX に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAlignSetEx(ALIGNSET_SET_EX sAttribute, short sParam, ref int lpData);
        [DllImport("IPALGN32", EntryPoint = "IpAlignSetEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAlignSetStr(short sCmd, short sParam, string lpParam);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Set an integer value; variable pointers are not required 
        //UPGRADE_WARNING: 構造体 ALIGNSET_SET_INT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAlignSetInt(ALIGNSET_SET_INT sAttribute, short sParam, short sData);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Set a float value; variable pointers are not required 
        //UPGRADE_WARNING: 構造体 ALIGNSET_SET_SINGLE に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAlignSetSingle(ALIGNSET_SET_SINGLE sAttribute, short sParam, float fData);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Get function for module data 
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 ALIGNSET_GET に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAlignGet(ALIGNSET_GET sAttribute, short sParam, ref int lpData);
        [DllImport("IPALGN32", EntryPoint = "IpAlignGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpAlignGetStr(short sCmd, short sParam, string lpParam);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Add a frame from a workspace to the selected set; returns IPCERR_INVCOMMAND if 
        // the image does not meet the current size and class requirements 
        public static extern int IpAlignAdd(short docId, short frame);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Remove a frame from the selected set 
        public static extern int IpAlignRemove(short docId, short frame);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Calculate offsets for the images 
        public static extern int IpAlignCalculate();
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Apply the offsets, creating a new workspace and returning the document ID 
        public static extern int IpAlignApply();
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Save the current offset values 
        public static extern int IpAlignSave(string FileName);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Load offset values. Fails if the number of offsets do not match the 
        // current number of selected frames/images 
        public static extern int IpAlignOpen(string FileName);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Underlying affine transform function for shifting images. This 
        // rotates, scales and then transforms the active workspace. 
        public static extern int IpAffine(float fRotate, float fScale, short dx, short dy);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // The function performs cross-correlation between RefImage and TargetImage 
        // (images are defined by Vri, RECT and FrameNumber) 
        // and returns translation, rotation and scale of the transform, which moves the defined area on RefImage to TargetImage 
        // in the array of doubles defined by lpOutParam 
        // OutParam[0] - translation X 
        // OutParam[1] - translation Y 
        // OutParam[2] - angle 
        // OutParam[3] - scale 
        // OutParam[4] - rank value that shows the degree of cross-correlation 
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 RECT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 RECT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAlignGetCrossCorrelValues(short sRefImageVri, short sRefFrame, ref Winapi.RECT RefRect, short sTargetImageVri, short sTargetFrame, ref RECT TargetRect, short bDoRotate, short bDoScale, short bDoTranslate, short bPhaseCorr,
        ref int lpOutParam);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //The function sets the search pattern by ImageHandle, Frame and Rect 
        //this function must be followed by IpAlignFindPattern, which will return the position 
        //of the pattern on the target image 
        //UPGRADE_WARNING: 構造体 RECT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAlignSetSearchPattern(short sRefImageVri, short sRefFrame, ref Winapi.RECT RefRect);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Finds pattern on the target image. The pattern is set by IpAlignSetSearchPattern. 
        // 
        // sTargetImageVri - VRI of the target image 
        // sTargetFrame - frame number 
        // TargetRect - rectangle, within which the search will be performed 
        // 
        // bDoRotate - defines whether rotation will be analyzed finding the pattern (1- On, 0- Off) 
        // bDoScale - defines whether scaling will be analyzed finding the pattern (1- On, 0- Off) 
        // bDoTranslate - defines whether translation will be analyzed finding the pattern, should be On in most of cases (1- On, 0- Off) 
        // bPhaseCorr - defines the type of cross-correlation (0 - Full correlation, 1 - phase correlation only) 
        // in the array of doubles defined by lpOutParam 
        // OutParam[0] - X position on the target image 
        // OutParam[1] - Y position on the target image 
        // OutParam[2] - angle 
        // OutParam[3] - scale 
        // OutParam[4] - rank value that shows the degree of cross-correlation 
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 RECT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpAlignFindPattern(short sTargetImageVri, short sTargetFrame, ref Winapi.RECT TargetRect, short bDoRotate, short bDoScale, short bDoTranslate, short bPhase, ref int lpOutParam, int sNumExpectedPoints);

        //----------------------------------------------------------------- 
        // Tiling constants 
        //----------------------------------------------------------------- 

        // Integer arguments (Get/Set). sParam is ignored unless specified. 
        // Set the method for alignment calculations 
        public const short TILE_ALGORITHM = 500;
        // Order the images as per calibrated positions 
        public const short TILE_CAL_ORDER = 510;
        // Algorithm specific option 
        public const short TILE_ALG_OPTION = 520;
        // (get) number of frames in list 
        public const short TILE_GETNUMFRAMES = 530;
        // (get) list of frames 
        public const short TILE_GETFRAMELST = 540;
        // GETNUMDOC (get) number of images in list 
        // GETDOCLST (get) list of doc ID's, max count sParam 
        // Determine whether or not UI is updated 
        public const short TILE_UPDATEUI = 550;
        // (get) Bool; All images have consistent spatial calibrations 
        public const short TILE_HAVECALIB = 560;
        // (get) Bool; All images have required XYZ position information 
        public const short TILE_HAVEPOSITION = 570;
        // (get) Bool; A solution has been calculated 
        public const short TILE_CALCSTATE = 580;
        // Normal or reverse X ordering (calibrated or as supplied) 
        public const short TILE_X_REVERSE = 590;
        // Normal or reverse Y ordering (calibrated or as supplied) 
        public const short TILE_Y_REVERSE = 600;
        // Zig-zag arrangement, alternate rows 
        public const short TILE_ZIGZAG_ORDER = 610;
        // Vertical arrangement, as opposed to horizontal 
        public const short TILE_VERTICAL_ORDER = 620;
        // Full list of selections, as opposed to grouped 
        public const short TILE_FULL_LIST = 630;
        // (get) Is the preview active? 
        public const short TILE_ISPREVIEWING = 640;
        // Specify how tiles are combined at edges, see arguments below 
        public const short TILE_BLEND = 650;
        // (set) Arrange tiles as per their positions 
        public const short TILE_SETFROMPOS = 660;
        // (set) Move specified frame up in the list 
        public const short TILE_MOVEUP = 670;
        // (set) Move specified frame down in the list 
        public const short TILE_MOVEDOWN = 680;
        // Bool; always recalculate computed alignment 
        public const short TILE_ALWAYSRECALC = 690;

        // Note that these array fetch routines require: 
        // Second parameter is the index (see TILE_GETNUMFRAMES) 

        // Get only, for each frame, expressing how it is manipulated compared to the 
        // previous frame 
        // Number of matching offsets (short) 
        public const short TILE_OFFSET_COUNT = 2000;

        // Second parameter is the index (see TILE_GETNUMFRAMES) 
        // List of POINTAPI offsets 
        public const short TILE_OFFSET_VAL = 2030;

        // Second parameter is the INDEX (see TILE_GETNUMFRAMES) 
        // List of float relative match values 
        public const short TILE_OFFSET_RANK = 2060;

        // TILE_ALGORITHM arguments. Additional methods can be 
        // added here, with TILE_ALG_OPTION arguments for algorithm 
        // specific settings. 
        // FFT correlation 
        public const short TILE_FFT = 1;
        // User specified offsets 
        public const short TILE_USER = 2;

        // Constant indicating a blank frame 
        public const short DOCSEL_BLANK = -100;

        // List argument (get) 
        // NumFrames list of shorts, sParam with max count. 
        public const short TILE_INDEXES = 3000;
        // 2 element short array of X and Y tile layout size, 
        public const short TILE_XY_LAYOUT = 3010;
        // this index shows how the inputs are ordered under 
        // current params 
        // List of point offsets for final tile positions. 
        public const short TILE_POSITIONS = 3020;
        // sParam indicates which tile, while DOCSEL_ALL requests the 
        // complete list. lpData must be an appropriately sized 
        // list of POINT structures. 

        // Floating point arguments 
        // Percent of image to overlap in X (tile) 
        public const short TILE_X_PERC_OVERLAP = 3500;
        // Percent of image to overlap in Y (tile) 
        public const short TILE_Y_PERC_OVERLAP = 3510;
        // Pixels of image to overlap in X (tile) 
        public const short TILE_X_PIXEL_OVERLAP = 3520;
        // Pixels of image to overlap in Y (tile) 
        public const short TILE_Y_PIXEL_OVERLAP = 3530;
        // Calibrated distance to overlap in X (tile) 
        public const short TILE_X_CAL_OVERLAP = 3540;
        // Calibrated distance to overlap in Y (tile) 
        public const short TILE_Y_CAL_OVERLAP = 3550;

        // TILE_BLEND arguments 
        // No blending, overlay images in tile order 
        public const short TILE_BLEND_OVERLAY = 1;
        // Gradient blend at edges 
        public const short TILE_BLEND_GRADIENT = 2;
        // Trim images to fit halfway between overlaps 
        public const short TILE_BLEND_CLIP = 3;

        // IpTileShow arguments 
        public const short TILE_IMAGETAB = 1;
        public const short TILE_OPTIONTAB = 2;
        public const short TILE_LAYOUT = 3;
        public const short TILE_ADJUST = 4;

        // TILE_ALG_OPTION constants for TILE_FFT, specific to that algorithm 
        // Internal algorithm 
        public const short TILE_METHOD = 1;

        // TILE_ALG_OPTION, TILE_METHOD constants for TILE_FFT, specific to that algorithm 
        // Set to FFT full correlation 
        public const short TILE_FFTFULL = 1;
        // Set to FFT phase correlation 
        public const short TILE_FFTPHASE = 2;

        // TILE_ALG_OPTION calls for TILE_USER, specific to that algorithm 
        // Horizontal X shift per plane (float) 
        public const short TILE_USER_X_HORIZ = 101;
        // Horizontal Y shift per plane (float) 
        public const short TILE_USER_Y_HORIZ = 102;
        // Vertical X shift per plane (float) 
        public const short TILE_USER_X_VERT = 103;
        // Vertical Y shift per plane (float) 
        public const short TILE_USER_Y_VERT = 104;

        // Calibrated Horizontal X shift per plane (float) 
        public const short TILE_USER_X_CALHORIZ = 105;
        // Calibrated Horizontal Y shift per plane (float) 
        public const short TILE_USER_Y_CALHORIZ = 106;
        // Calibrated Vertical X shift per plane (float) 
        public const short TILE_USER_X_CALVERT = 107;
        // Calibrated Vertical Y shift per plane (float) 
        public const short TILE_USER_Y_CALVERT = 108;

        // Alignment enumerations 
        // attributes for IpTileSetInt 
        public enum TILE_SET_INT
        {
            TILESET_ALGORITHM = Ipc32v5.TILE_ALGORITHM,
            // Set the method for alignment calculations 
            TILESET_CAL_ORDER = Ipc32v5.TILE_CAL_ORDER,
            // Order the images as per calibrated positions 
            TILESET_ALG_OPTION = Ipc32v5.TILE_ALG_OPTION,
            // Algorithm specific option 
            TILESET_UPDATEUI = Ipc32v5.TILE_UPDATEUI,
            // Determine whether or not UI is updated 
            TILESET_X_REVERSE = Ipc32v5.TILE_X_REVERSE,
            // Normal or reverse X ordering (calibrated or as supplied) 
            TILESET_Y_REVERSE = Ipc32v5.TILE_Y_REVERSE,
            // Normal or reverse Y ordering (calibrated or as supplied) 
            TILESET_ZIGZAG_ORDER = Ipc32v5.TILE_ZIGZAG_ORDER,
            // Zig-zag arrangement, alternate rows 
            TILESET_VERTICAL_ORDER = Ipc32v5.TILE_VERTICAL_ORDER,
            // Vertical arrangement, as opposed to horizontal 
            TILESET_FULL_LIST = Ipc32v5.TILE_FULL_LIST,
            // Full list of selections, as opposed to grouped 
            TILESET_BLEND = Ipc32v5.TILE_BLEND,
            // Specify how tiles are combined at edges, see arguments below 
            TILESET_MOVEUP = Ipc32v5.TILE_MOVEUP,
            // (set) Move specified frame up in the list 
            TILESET_MOVEDOWN = Ipc32v5.TILE_MOVEDOWN,
            // (set) Move specified frame down in the list 
            TILESET_ALWAYSRECALC = Ipc32v5.TILE_ALWAYSRECALC
            // Bool; always recalculate computed alignment 
        }

        // attributes for IpTileSetSingle 
        public enum TILE_SET_SINGLE
        {
            TILESET_X_PERC_OVERLAP = Ipc32v5.TILE_X_PERC_OVERLAP,
            // Percent of image to overlap in X (tile) 
            TILESET_Y_PERC_OVERLAP = Ipc32v5.TILE_Y_PERC_OVERLAP,
            // Percent of image to overlap in Y (tile) 
            TILESET_X_PIXEL_OVERLAP = Ipc32v5.TILE_X_PIXEL_OVERLAP,
            // Pixels of image to overlap in X (tile) 
            TILESET_Y_PIXEL_OVERLAP = Ipc32v5.TILE_Y_PIXEL_OVERLAP,
            // Pixels of image to overlap in Y (tile) 
            TILESET_X_CAL_OVERLAP = Ipc32v5.TILE_X_CAL_OVERLAP,
            // Calibrated distance to overlap in X (tile) 
            TILESET_Y_CAL_OVERLAP = Ipc32v5.TILE_Y_CAL_OVERLAP
            // Calibrated distance to overlap in Y (tile) 
        }

        // attributes for IpTileSetEx 
        public enum TILE_SET_EX
        {
            TILESET_OFFSET_VAL = Ipc32v5.TILE_OFFSET_VAL,
            // List of POINTAPI offsets 
            TILESET_OFFSET_RANK = Ipc32v5.TILE_OFFSET_RANK
            // List of float relative match values 
        }

        // attributes for IpTileGet 
        // this index shows how the inputs are ordered under 
        // current params 
        // sParam indicates which tile, while DOCSEL_ALL requests the 
        // complete list. lpData must be an appropriately sized 
        // list of POINT structures. 
        public enum TILE_GET
        {
            TILEGET_ALGORITHM = Ipc32v5.TILE_ALGORITHM,
            // Set the method for alignment calculations 
            TILEGET_ALG_OPTION = Ipc32v5.TILE_ALG_OPTION,
            // Algorithm specific option 
            TILEGET_GETNUMFRAMES = Ipc32v5.TILE_GETNUMFRAMES,
            // (get) number of frames in list 
            TILEGET_GETFRAMELST = Ipc32v5.TILE_GETFRAMELST,
            // (get) list of frames 
            TILEGET_GETNUMDOC = Ipc32v5.GETNUMDOC,
            // (get) number of images in list 
            TILEGET_GETDOCLST = Ipc32v5.GETDOCLST,
            // (get) list of doc ID's, max count sParam 
            TILEGET_UPDATEUI = Ipc32v5.TILE_UPDATEUI,
            // Determine whether or not UI is updated 
            TILEGET_HAVECALIB = Ipc32v5.TILE_HAVECALIB,
            // (get) Bool; All images have consistent spatial calibrations 
            TILEGET_HAVEPOSITION = Ipc32v5.TILE_HAVEPOSITION,
            // (get) Bool; All images have required XYZ position information 
            TILEGET_CALCSTATE = Ipc32v5.TILE_CALCSTATE,
            // (get) Bool; A solution has been calculated 
            TILEGET_X_REVERSE = Ipc32v5.TILE_X_REVERSE,
            // Normal or reverse X ordering (calibrated or as supplied) 
            TILEGET_Y_REVERSE = Ipc32v5.TILE_Y_REVERSE,
            // Normal or reverse Y ordering (calibrated or as supplied) 
            TILEGET_ZIGZAG_ORDER = Ipc32v5.TILE_ZIGZAG_ORDER,
            // Zig-zag arrangement, alternate rows 
            TILEGET_VERTICAL_ORDER = Ipc32v5.TILE_VERTICAL_ORDER,
            // Vertical arrangement, as opposed to horizontal 
            TILEGET_FULL_LIST = Ipc32v5.TILE_FULL_LIST,
            // Full list of selections, as opposed to grouped 
            TILEGET_ISPREVIEWING = Ipc32v5.TILE_ISPREVIEWING,
            // (get) Is the preview active? 
            TILEGET_BLEND = Ipc32v5.TILE_BLEND,
            // Specify how tiles are combined at edges, see arguments below 
            TILEGET_ALWAYSRECALC = Ipc32v5.TILE_ALWAYSRECALC,
            // Bool; always recalculate computed alignment 
            TILEGET_OFFSET_COUNT = Ipc32v5.TILE_OFFSET_COUNT,
            // Number of matching offsets (short) 
            TILEGET_OFFSET_VAL = Ipc32v5.TILE_OFFSET_VAL,
            // List of POINTAPI offsets 
            TILEGET_OFFSET_RANK = Ipc32v5.TILE_OFFSET_RANK,
            // List of float relative match values 
            TILEGET_INDEXES = Ipc32v5.TILE_INDEXES,
            // NumFrames list of shorts, sParam with max count. 
            TILEGET_XY_LAYOUT = Ipc32v5.TILE_XY_LAYOUT,
            // 2 element short array of X and Y tile layout size, 
            TILEGET_POSITIONS = Ipc32v5.TILE_POSITIONS,
            // List of point offsets for final tile positions. 
            TILEGET_X_PERC_OVERLAP = Ipc32v5.TILE_X_PERC_OVERLAP,
            // Percent of image to overlap in X (tile) 
            TILEGET_Y_PERC_OVERLAP = Ipc32v5.TILE_Y_PERC_OVERLAP,
            // Percent of image to overlap in Y (tile) 
            TILEGET_X_PIXEL_OVERLAP = Ipc32v5.TILE_X_PIXEL_OVERLAP,
            // Pixels of image to overlap in X (tile) 
            TILEGET_Y_PIXEL_OVERLAP = Ipc32v5.TILE_Y_PIXEL_OVERLAP,
            // Pixels of image to overlap in Y (tile) 
            TILEGET_X_CAL_OVERLAP = Ipc32v5.TILE_X_CAL_OVERLAP,
            // Calibrated distance to overlap in X (tile) 
            TILEGET_Y_CAL_OVERLAP = Ipc32v5.TILE_Y_CAL_OVERLAP
            // Calibrated distance to overlap in Y (tile) 
        }

        // attributes for IpTileShow 
        public enum TILE_SHOW
        {
            TILESHOW_IMAGETAB = Ipc32v5.TILE_IMAGETAB,
            TILESHOW_OPTIONTAB = Ipc32v5.TILE_OPTIONTAB,
            TILESHOW_LAYOUT = Ipc32v5.TILE_LAYOUT,
            TILESHOW_ADJUST = Ipc32v5.TILE_ADJUST
        }
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Tiling Functions 
        //----------------------------------------------------------------- 

        // Show/hide (1/0) the specified dialog 
        //UPGRADE_WARNING: 構造体 TILE_SHOW に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpTileShow(TILE_SHOW nDialog, short bShow);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Generic Set routine 
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 TILE_SET_EX に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpTileSetEx(TILE_SET_EX sAttribute, short sParam, ref int lpData);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Set an integer value; variable pointers are not required 
        //UPGRADE_WARNING: 構造体 TILE_SET_INT に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpTileSetInt(TILE_SET_INT sAttribute, short sParam, short sData);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Set a float value; variable pointers are not required 
        //UPGRADE_WARNING: 構造体 TILE_SET_SINGLE に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpTileSetSingle(TILE_SET_SINGLE sAttribute, short sParam, float fData);
        [DllImport("IpTile32", EntryPoint = "IpTileSetEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTileSetStr(short sCmd, short sParam, string lpParam);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Get function for module data 
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 TILE_GET に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpTileGet(TILE_GET sAttribute, short sParam, ref int lpData);
        [DllImport("IpTile32", EntryPoint = "IpTileGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTileGetStr(short sCmd, short sParam, string lpParam);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Add a frame from a workspace to the selected set; returns IPCERR_INVCOMMAND if 
        // the image does not meet the current size and class requirements 
        public static extern int IpTileAdd(short docId, short frame);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Remove a frame from the selected set 
        public static extern int IpTileRemove(short docId, short frame);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Calculate offsets for the images 
        public static extern int IpTileCalculate();
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Apply the offsets, creating a new workspace and returning the document ID 
        public static extern int IpTileApply();
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Save the current offset values 
        public static extern int IpTileSave(string FileName);
        [DllImport("IPALGN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Load offset values. Fails if the number of offsets do not match the 
        // current number of selected frames/images, or if tile layouts are different. 
        public static extern int IpTileOpen(string FileName);
        [DllImport("IPAOIEX32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]


        // 
        // Definitions for Extended AOI functions 
        // 
        // Copyright (c) 1999 - 2004, Media Cybernetics, Inc. 
        // 




        //----------------------------------------------------------------- 
        // AoiEx Functions 
        //----------------------------------------------------------------- 
        public static extern int IpAoiExShow();


        // 
        // Definitions for Auto-Pro Functions of Bayer interpolation 
        // 
        // Copyright (c) 1999 - 2009, Media Cybernetics, Inc. 
        // 




        // Bayer attributes 
        public const short BAYER_INTERPOLATION_MODE = 0;
        public const short BAYER_PIXEL_FORMAT = 1;
        public const short BAYER_PIXEL_OFFSET = 2;
        public const short BAYER_GREEN_PLANE = 3;
        public const short BAYER_OUTPUT = 4;

        // Bayer interpolation modes 
        public const short BAYER_NO_INTERPOLATION = 0;
        public const short BAYER_BILINEAR = 1;
        public const short BAYER_BICUBIC = 2;

        // Bayer pixel formats 
        // pixels arranged R Gr, with Gb B on second line 
        public const short BAYER_FMT_R_GR_GB_B = 0;
        // pixels arranged Gr R, with B Gb on second line 
        public const short BAYER_FMT_GR_R_B_GB = 1;
        // pixels arranged Gb B, with R Gr on second line 
        public const short BAYER_FMT_GB_B_R_GR = 2;
        // pixels arranged B Gb, with Gr R on second line 
        public const short BAYER_FMT_B_GB_GR_R = 3;

        // Bayer pixels offsets 
        // 
        public const short BAYER_NO_OFFSET = 0;
        // Offset by one pixel horizontally 
        public const short BAYER_HORIZONTAL_OFFSET = 1;
        // Offset by one pixel vertically 
        public const short BAYER_VERTICAL_OFFSET = 2;
        // Offset by one pixel horizontally and vertically 
        public const short BAYER_BOTH_OFFSET = 3;

        // Green plane options 
        // Combine green planes 
        public const short BAYER_COMBINE_GREEN = 0;
        // Use only the Gr plane 
        public const short BAYER_USE_GR = 1;
        // Use only the Gb plane 
        public const short BAYER_USE_GB = 2;

        // Output options 
        public const short BAYER_OUTPUT_RGB = 0;
        // when set, outputs separate R, G and B images 
        public const short BAYER_OUTPUT_PLANES = 1;
        [DllImport("IPBAYER", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Bayer interpolation Functions 
        //----------------------------------------------------------------- 
        public static extern int IpBayerShow(short bShow);
        [DllImport("IPBAYER", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBayerSetInt(short sAttribute, short sValue);
        [DllImport("IPBAYER", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBayerGetInt(short sAttribute, ref short sValue);
        [DllImport("IPBAYER", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpBayerInterpolate();


        // 
        // Definitions for Caliper Auto-Pro Functions 
        // 
        // Copyright (c) 1998 - , Media Cybernetics, Inc. 
        // 



        // Clpr Sampler types 
        public const short CLPR_LINE = 0;
        public const short CLPR_CWCIRCLE = 1;
        public const short CLPR_CCWCIRCLE = 2;
        public const short CLPR_POLYLINE = 3;

        // Clpr Edge Detector types 
        public const short CLPR_DERIVATIVE = 0;
        public const short CLPR_PATTERN_MATCH = 1;

        // Clpr Attribute types for use with IpClprGet/Set 
        public const short CLPR_AUTOREFRESH = 200;
        public const short CLPR_CIRCLE_ORIGIN = 201;

        public const short CLPRE_NAME = 301;
        public const short CLPRE_LABEL = 302;
        public const short CLPRE_COLOR = 303;
        public const short CLPRE_OFFSET = 304;
        public const short CLPRE_STYLE = 305;
        public const short CLPRE_THRESHOLD = 306;
        // sensitivity threshold in percents 
        public const short CLPRE_SENS = 307;

        public const short CLPRO_SMOOTHING = 310;
        public const short CLPRO_THICKNESS = 311;
        public const short CLPRO_APPLY_ICAL = 312;
        public const short CLPRO_APPLY_SCAL = 313;
        public const short CLPRO_AUTO_SCALE = 314;
        public const short CLPRO_SHOW_LABEL = 315;
        public const short CLPRO_SHOW_NUMBER = 316;
        public const short CLPRO_PRECISION = 317;
        public const short CLPRO_LOAD_AS_TEMPLATE = 318;

        // new attribute types for use with IpClprGetIntEx 
        // get the number of samplers, Index not used 
        public const short CLPR_NUM_SAMPLERS = 400;
        // get the ID of the sampler specified by Index (use the ID with IpClprSelectSampler) 
        public const short CLPR_SAMPLER_ID = 401;
        // Get the number of intensity values returned by CLPR_PROFILE or point locations returned by IpClprGetPoints, Index not used 
        public const short CLPR_NUM_PROFILE_POINTS = 402;
        // Get the number of measurements 
        public const short CLPR_NUM_MEASUREMENTS = 403;
        // Get the number of measurement values for a particular measurement 
        public const short CLPR_NUM_MEAS_VALUES = 404;
        // Get the number of sampler definition points returned by IpClprGetPoints 
        public const short CLPR_NUM_SAMPLER_POINTS = 405;

        // new attribute types for use with IpClprGetIntEx and IpClprSetIntEx 
        // Get the INDEX of the active sampler, or set the active sampler by index 
        public const short CLPR_ACTIVE_SAMPLER = 450;
        // Get the index of the active detector, or set the active detector by index 
        public const short CLPR_ACTIVE_DETECTOR = 451;

        // new attribute types for use with IpClprGetStrEx 
        // Get the name of the sampler specified by Index 
        public const short CLPR_SAMPLER_NAME = 500;
        // Get the name of the detector specified by Index on the active sampler 
        public const short CLPR_DETECTOR_NAME = 501;

        // new attribute types for use with IpClprGetSngEx 
        // get luminosity profile from the sampler specified by Index 
        public const short CLPR_PROFILE = 600;

        // new attribute types for use with IpClprDetGetInt 
        public const short CLPR_GET_NUM_DETECTORS = 700;
        public const short CLPR_GET_DETECTOR_TYPE = 701;
        public const short CLPR_GET_DET_NUM_MARKERS = 702;

        // new attribute types for use with IpClprDetGetSng 
        public const short CLPR_GET_DET_MARKER_X = 800;
        public const short CLPR_GET_DET_MARKER_Y = 801;

        // Clpr Edge Detector Attribute values 
        public const short CLPR_PEAK = 0;
        public const short CLPR_VALLEY = 1;
        public const short CLPR_RISING = 2;
        public const short CLPR_FALLING = 3;

        // Measurement types 
        // X-position of markers in image coordinate 
        public const short CLPR_MEAS_POSX = 0;
        // Y-position of markers in image coordinate 
        public const short CLPR_MEAS_POSY = 1;
        // distance along the sampler's outline 
        public const short CLPR_MEAS_DIST = 2;
        // distances between consecutive markers in a detector 
        public const short CLPR_MEAS_DIST1 = 3;
        // distances between corresponding markers of two detectors 
        public const short CLPR_MEAS_DIST2 = 4;

        // Clipboard commands 
        public const short CLPR_CUT = 0;
        public const short CLPR_COPY = 1;
        public const short CLPR_PASTE = 2;

        // IpClprGetData commands 
        public const int CLPD_STAT = 0x800;
        public const short CLPD_GETROWCOUNT = 1;
        public const short CLPD_GETCOLCOUNT = 2;
        public const short CLPD_GETCELL = 3;

        public const short CLPR_MAX_PATTERN_SIZE = 50;
        public static float[] ipPattern = new float[CLPR_MAX_PATTERN_SIZE + 1];

        // Caliper enumerations 

        // for IpClprSet 
        public enum CLPRSET_ATTRIBUTES
        {
            CLPRSET_AUTOREFRESH = Ipc32v5.CLPR_AUTOREFRESH,
            CLPRSET_CIRCLE_ORIGIN = Ipc32v5.CLPR_CIRCLE_ORIGIN,
            CLPRSET_COLOR = Ipc32v5.CLPRE_COLOR,
            CLPRSET_OFFSET = Ipc32v5.CLPRE_OFFSET,
            CLPRSET_STYLE = Ipc32v5.CLPRE_STYLE,
            CLPRSET_THRESHOLD = Ipc32v5.CLPRE_THRESHOLD,
            CLPRSET_SENS = Ipc32v5.CLPRE_SENS,
            CLPRSET_SMOOTHING = Ipc32v5.CLPRO_SMOOTHING,
            CLPRSET_THICKNESS = Ipc32v5.CLPRO_THICKNESS,
            CLPRSET_APPLY_ICAL = Ipc32v5.CLPRO_APPLY_ICAL,
            CLPRSET_APPLY_SCAL = Ipc32v5.CLPRO_APPLY_SCAL,
            CLPRSET_AUTO_SCALE = Ipc32v5.CLPRO_AUTO_SCALE,
            CLPRSET_SHOW_LABEL = Ipc32v5.CLPRO_SHOW_LABEL,
            CLPRSET_SHOW_NUMBER = Ipc32v5.CLPRO_SHOW_NUMBER,
            CLPRSET_PRECISION = Ipc32v5.CLPRO_PRECISION,
            CLPRSET_LOAD_AS_TEMPLATE = Ipc32v5.CLPRO_LOAD_AS_TEMPLATE
        }

        // for IpClprSet 
        public enum CLPRGET_ATTRIBUTES
        {
            CLPRGET_AUTOREFRESH = Ipc32v5.CLPR_AUTOREFRESH,
            CLPRGET_CIRCLE_ORIGIN = Ipc32v5.CLPR_CIRCLE_ORIGIN,
            CLPRGET_COLOR = Ipc32v5.CLPRE_COLOR,
            CLPRGET_OFFSET = Ipc32v5.CLPRE_OFFSET,
            CLPRGET_STYLE = Ipc32v5.CLPRE_STYLE,
            CLPRGET_THRESHOLD = Ipc32v5.CLPRE_THRESHOLD,
            CLPRGET_SENS = Ipc32v5.CLPRE_SENS,
            CLPRGET_SMOOTHING = Ipc32v5.CLPRO_SMOOTHING,
            CLPRGET_THICKNESS = Ipc32v5.CLPRO_THICKNESS,
            CLPRGET_APPLY_ICAL = Ipc32v5.CLPRO_APPLY_ICAL,
            CLPRGET_APPLY_SCAL = Ipc32v5.CLPRO_APPLY_SCAL,
            CLPRGET_AUTO_SCALE = Ipc32v5.CLPRO_AUTO_SCALE,
            CLPRGET_SHOW_LABEL = Ipc32v5.CLPRO_SHOW_LABEL,
            CLPRGET_SHOW_NUMBER = Ipc32v5.CLPRO_SHOW_NUMBER,
            CLPRGET_PRECISION = Ipc32v5.CLPRO_PRECISION,
            CLPRGET_LOAD_AS_TEMPLATE = Ipc32v5.CLPRO_LOAD_AS_TEMPLATE
        }

        // for IpClprSetStr 
        public enum CLPRSETSTR_ATTRIBUTES
        {
            CLPRSETSTR_NAME = Ipc32v5.CLPRE_NAME,
            CLPRSETSTR_LABEL = Ipc32v5.CLPRE_LABEL
        }

        // for IpClprGetStr 
        public enum CLPRGETSTR_ATTRIBUTES
        {
            CLPRGETSTR_NAME = Ipc32v5.CLPRE_NAME,
            CLPRGETSTR_LABEL = Ipc32v5.CLPRE_LABEL
        }

        // for IpClprGetIntEx 
        public enum CLPRGETINTEX_ATTRIBUTES
        {
            CLPRGETINTEX_NUM_SAMPLERS = Ipc32v5.CLPR_NUM_SAMPLERS,
            CLPRGETINTEX_SAMPLER_ID = Ipc32v5.CLPR_SAMPLER_ID,
            CLPRGETINTEX_NUM_PROFILE_POINTS = Ipc32v5.CLPR_NUM_PROFILE_POINTS,
            CLPRGETINTEX_NUM_MEASUREMENTS = Ipc32v5.CLPR_NUM_MEASUREMENTS,
            CLPRGETINTEX_NUM_MEAS_VALUES = Ipc32v5.CLPR_NUM_MEAS_VALUES,
            CLPRGETINTEX_CLPR_ACTIVE_SAMPLER = Ipc32v5.CLPR_ACTIVE_SAMPLER,
            CLPRGETINTEX_CLPR_ACTIVE_DETECTOR = Ipc32v5.CLPR_ACTIVE_DETECTOR,
            CLPRGETINTEX_CLPR_NUM_SAMPLER_POINTS = Ipc32v5.CLPR_NUM_SAMPLER_POINTS
        }

        // for IpClprGetStrEx 
        public enum CLPRGETSTREX_ATTRIBUTES
        {
            CLPRGETSTREX_SAMPLER_NAME = Ipc32v5.CLPR_SAMPLER_NAME,
            CLPRGETSTREX_DETECTOR_NAME = Ipc32v5.CLPR_DETECTOR_NAME
        }

        // for IpClprGetSngEx 
        public enum CLPRGETSNGEX_ATTRIBUTES
        {
            CLPRGETSNGEX_CLPR_PROFILE = Ipc32v5.CLPR_PROFILE
        }

        // for IpClprSetIntEx 
        public enum CLPRSETINTEX_ATTRIBUTES
        {
            CLPRSETINTEX_CLPR_ACTIVE_SAMPLER = Ipc32v5.CLPR_ACTIVE_SAMPLER,
            CLPRSETINTEX_CLPR_ACTIVE_DETECTOR = Ipc32v5.CLPR_ACTIVE_DETECTOR
        }

        // for IpClprDetGetInt 
        public enum CLPRDETGETINT_ATTRIBUTES
        {
            CLPRDETGETINT_NUM_DETECTORS = Ipc32v5.CLPR_GET_NUM_DETECTORS,
            CLPRDETGETINT_DETECTOR_TYPE = Ipc32v5.CLPR_GET_DETECTOR_TYPE,
            CLPRDETGETINT_DET_NUM_MARKERS = Ipc32v5.CLPR_GET_DET_NUM_MARKERS
        }

        // for IpClprDetGetSng 
        public enum CLPRDETGETSNG_ATTRIBUTES
        {
            CLPRDETGETSNG_DET_MARKER_X = Ipc32v5.CLPR_GET_DET_MARKER_X,
            CLPRDETGETSNG_DET_MARKER_Y = Ipc32v5.CLPR_GET_DET_MARKER_Y
        }

        // for IpClprTool 
        public enum CLPRTOOL_TYPES
        {
            CLPRTOOL_NONE = 0,
            CLPRTOOL_SELECT = 1,
            CLPRTOOL_LINE = 2,
            CLPRTOOL_CWCIRCLE = 3,
            CLPRTOOL_CCWCIRCLE = 4,
            CLPRTOOL_POLYLINE = 5,
            CLPRTOOL_MARKER = 6
        }


        // for IpClprGetPoints 
        public enum CLPRPOINT_TYPES
        {
            CLPRPTS_SAMPLER = 0,
            // return the points defining the sampler 
            CLPRPTS_PROFILE = 1
            // return all the points sampled in the sampler profile, in image coordinates 
        }
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Caliper Functions 
        public static extern int IpClprShow(short nShow);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //UPGRADE_WARNING: 構造体 POINTAPI に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpClprCreateSampler(short nType, string szName, ref POINTAPI pt, short nNumPoints);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprEditSampler(short nHandle, short x, short y);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprSelectSampler(short nID);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprDeleteSampler();
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprClipboard(short nCommand);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpClprCreateDerivativeEdge(string szName, string szLabel, int lColor, short nOffset, short nStyle);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprCreatePatternMatchEdge(string szName, string szLabel, int lColor, short nOffset, short nThreshold, ref float ptPattern, short nNumPoints);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprSelectEdge(string szName);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprDeleteEdge();
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpClprCreateMeas(short nType, string strFromName, string strToName);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprDeleteMeas(short nType, string strFromName, string strToName);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprToggleMarker(short x, short y);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 CLPRSET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpClprSet(CLPRSET_ATTRIBUTES sAttribute, float fData);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 CLPRGET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpClprGet(CLPRGET_ATTRIBUTES sAttribute, ref float fData);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 CLPRSETSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpClprSetStr(CLPRSETSTR_ATTRIBUTES sAttribute, string strValue);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 CLPRGETSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpClprGetStr(CLPRGETSTR_ATTRIBUTES sAttribute, string strValue);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprSettings(string szFileName, short bSave);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpClprSave(string szFileName, short nSaveMode);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpClprGetData(short Command_Renamed, short nParam1, short nParam2, string strRetVal);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // sampler data functions 
        //UPGRADE_WARNING: 構造体 CLPRGETINTEX_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpClprGetIntEx(CLPRGETINTEX_ATTRIBUTES sAttribute, short sIndex, ref short sValue);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 CLPRGETSTREX_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpClprGetStrEx(CLPRGETSTREX_ATTRIBUTES sAttribute, short sIndex, string strValue);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 CLPRGETSNGEX_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpClprGetSngEx(CLPRGETSNGEX_ATTRIBUTES sAttribute, short sIndex, ref float fValue);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // set function 
        //UPGRADE_WARNING: 構造体 CLPRSETINTEX_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpClprSetIntEx(CLPRSETINTEX_ATTRIBUTES sAttribute, short sValue);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // detector data functions 
        //UPGRADE_WARNING: 構造体 CLPRDETGETINT_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpClprDetGetInt(CLPRDETGETINT_ATTRIBUTES sAttribute, short sSampler, short sDetector, ref short sValue);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 CLPRDETGETSNG_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpClprDetGetSng(CLPRDETGETSNG_ATTRIBUTES sAttribute, short sSampler, short sDetector, short sIndex, ref float fValue);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // data function 
        public static extern int IpClprGetDataEx(short sMeasureIndex, short sNumber, ref float fValues);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // caliper tool function 
        //UPGRADE_WARNING: 構造体 CLPRTOOL_TYPES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpClprTool(CLPRTOOL_TYPES sCaliperTool);
        [DllImport("IPCLPR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //UPGRADE_WARNING: 構造体 POINTAPI に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 CLPRPOINT_TYPES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpClprGetPoints(short sSampler, CLPRPOINT_TYPES sPointType, short sNumber, ref POINTAPI Points);
        // 
        // Color Composite functions and constants 
        // 
        // Copyright (c) 1998 - 2001 , Media Cybernetics, Inc. 
        // 





        public const short SHIFT_X = 7;
        public const short SHIFT_Y = 8;
        public const short COMP_HUE = 9;

        public const short COMP_BACKGROUND = 10;

        public const short COMP_BESTFIT = 11;
        public const short COMP_RESET = 12;

        public const short COMP_UPDATE = 13;

        public const short COMP_FRAME = 14;
        public const short COMP_NUMFRAMES = 15;

        public const short COMP_MAKESEQUENCE = 16;

        public const short COMP_DISPLAY = 17;

        public const short COMP_AUTO_COMPOSITE = 18;

        public const short COMP_TINT = 19;

        public const short COMP_SHOW = 1;
        public const short COMP_HIDE = 0;

        public const short HUE_INTERACTIVE = -3;
        public const short HUE_DEFAULT = -2;
        public const short HUE_QUERY = -1;
        public const short HUE_RED = 0;
        public const short HUE_GREEN = 120;
        public const short HUE_BLUE = 240;
        public const short HUE_YELLOW = 60;
        public const short HUE_CYAN = 180;
        public const short HUE_MAGENTA = 300;
        public const short HUE_WHITE = 361;
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // All settings for the color composite 
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpCmpSet(short Command_Renamed, short param, int Value);
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpCmpGet(short Command_Renamed, short param, ref int lpParam);
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Start a new composite. Composite size will be set to the size of the doc. 
        public static extern int IpCmpNew(short docId, short hue);
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Add a document to the composite. 
        public static extern int IpCmpAdd(short docId, short hue);
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Add a document to the composite with x/y shift 
        public static extern int IpCmpAddEx(short docId, short hue, short dx, short dy);
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Delete a document from the composite. 
        public static extern int IpCmpDel(short docId);
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Show or hide the color composite dialog 
        public static extern int IpCmpShow(short flag);
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Color composite channels can also be defined with a specific RGB tint 
        public static extern int IpCmpNewTint(short docId, int Tint);
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Add a document to the composite with a specific tint. 
        public static extern int IpCmpAddTint(short docId, int Tint);
        [DllImport("IpCCmp32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Same with x/y shift 
        public static extern int IpCmpAddTintPos(short docId, int Tint, short dx, short dy);

        // 
        // Definitions for L*a*b color/Color correction module 
        // 
        // Copyright (c) 1998 - , Media Cybernetics, Inc. 
        // 




        //L*a*b color/Color correction module 
        // Color modes 
        public const short COLM_LAB = 0;
        public const short COLM_XYZ = 1;
        public const short COLM_RGB = 2;
        public const short COLM_YIQ = 3;
        public const short COLM_CMY = 4;

        public const short COLM_CH1 = 1;
        public const short COLM_CH2 = 2;
        public const short COLM_CH3 = 4;

        public const short SET_CAL_POINT = 0;
        public const short SET_CAL_INFO = 1;
        public const short SET_CAL_MATRIX = 2;
        public const short SET_CAL_ICC = 3;

        public const short GET_CAL_POINT = 0;
        public const short GET_CAL_INFO = 1;
        public const short GET_CAL_MATRIX = 2;
        public const short GET_CAL_ICC = 3;


        public const short COLM_MAXPOINTS = 20;
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]


        public static extern int IpColExtract(int MASK, short ColMode, short IsFloat);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColShow(short Show);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColCalNew(short InpMode, short ColModel);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpColCalGetRGB(short x, short y, short Size, ref int lpParam);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColCalAdd(ref float fRGB, ref float fLAB);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColCalCreate();
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColCalShow(short Show);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColCalSave(string Name);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColCalLoad(string Name);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColCalCorrect(string InName, string OutName);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_NOTE: rgb は rgb_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpGetConvertColor(ref float rgb_Renamed, ref float Out, int ColMode, int iClass, int Norm);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColConvert(short ColMod);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpColCalGet(short Command_Renamed, short N, ref float Out);
        [DllImport("IPCOL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpColCalSet(short Command_Renamed, short N, ref float Out);

        // 
        // Co-localization functions and constants 
        // 
        // Copyright (c) 1999 - 2001, Media Cybernetics, Inc. 
        // 



        public const short CP_RED_GREEN = 0;
        public const short CP_BLUE_RED = 1;
        public const short CP_GREEN_BLUE = 2;
        public const short CP_GREEN_RED = 3;
        public const short CP_RED_BLUE = 4;
        public const short CP_BLUE_GREEN = 5;

        public const short CLOC_FWDMASK = 0;
        public const short CLOC_FWDCOLOR = 1;
        public const short CLOC_FWD3D = 2;
        public const short CLOC_FWDPARAMS = 3;

        public const short CLOC_INVMASK = 0;
        public const short CLOC_INVPARAMS = 1;

        public const short CLDOC_COLORCOMPOSITE = 0;
        public const short CLDOC_SCATTERPLOT = 1;
        public const short CLDOC_3DMASK = 2;

        //CLDOC enumerations 

        public enum CL_COLOR_PAIRS
        {
            CLOCCP_RED_GREEN = Ipc32v5.CP_RED_GREEN,
            CLOCCP_BLUE_RED = Ipc32v5.CP_BLUE_RED,
            CLOCCP_GREEN_BLUE = Ipc32v5.CP_GREEN_BLUE,
            CLOCCP_GREEN_RED = Ipc32v5.CP_GREEN_RED,
            CLOCCP_RED_BLUE = Ipc32v5.CP_RED_BLUE,
            CLOCCP_BLUE_GREEN = Ipc32v5.CP_BLUE_GREEN
        }

        public enum CLOC_FTYPES
        {
            CLOCFT_FWDMASK = Ipc32v5.CLOC_FWDMASK,
            CLOCFT_FWDCOLOR = Ipc32v5.CLOC_FWDCOLOR,
            CLOCFT_FWD3D = Ipc32v5.CLOC_FWD3D,
            CLOCFT_FWDPARAMS = Ipc32v5.CLOC_FWDPARAMS
        }
        public enum CLOC_ITYPES
        {
            CLOCIT_INVMASK = Ipc32v5.CLOC_INVMASK,
            CLOCIT_INVPARAMS = Ipc32v5.CLOC_INVPARAMS
        }

        public enum CLOCDOC_TYPES
        {
            CLDOCT_COLORCOMPOSITE = Ipc32v5.CLDOC_COLORCOMPOSITE,
            CLDOCT_SCATTERPLOT = Ipc32v5.CLDOC_SCATTERPLOT,
            CLDOCT_3DMASK = Ipc32v5.CLDOC_3DMASK
        }
        [DllImport("IpCoLoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]



        //----------------------------------------------------------------- 
        // Co-Localization Functions 
        //----------------------------------------------------------------- 

        //Shows/hides co-localization dialog 
        public static extern int IpCoLocShow(short bShow);
        [DllImport("IpCoLoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //Calculates co-localization parameters of active color or active gray and second gray image 
        // 
        // SecondImageId - image ID of second gray image if the active image is gray 
        // if active image is color this parameter is ignored (should be -1) 
        //ColorPair - color pair, must be one of the following: 
        // CP_RED_GREEN, CP_BLUE_RED, CP_GREEN_BLUE, 
        // CP_GREEN_RED, CP_RED_BLUE, CP_BLUE_GREEN 
        // where first color is channel number 1 (active image in 
        // case of gray images) - horizontal axis of co-localization plot, 
        // second color is channel number 2 (2nd image in case of gray images) 
        // - vertical axis of co-localization plot 
        //FType - type of the function: 
        // CLOC_FWDMASK - creates gray co-localization, 16-bit image of co-localization plot 
        // where brightness (gray level) represents the frequency (number of occurrences) 
        // of color pair on the original image, sequence or AOI 
        // CLOC_FWDCOLOR - creates color co-localization, RGB image of co-localization plot 
        // that have non-zero pixels where this coloc pair present on the original image 
        // CLOC_FWD3D - opens 3D view on co-localization plot, to create a 3D image of co-localization 
        // plot user has to click "New image" button in Output tab of Surface plot dialog 
        // CLOC_FWDPARAMS - calculates first set of co-localization parameters 
        //UPGRADE_WARNING: 構造体 CLOC_FTYPES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 CL_COLOR_PAIRS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpCoLocForward(short SecondImageId, CL_COLOR_PAIRS ColorPair, CLOC_FTYPES iType);
        [DllImport("IpCoLoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //Calculates co-localization parameters the image on the base of AOI on co-localization plot 
        // 
        //IType - Type of operation 
        // CLOC_INVMASK - creates a mask of co-localizing pixels on the base of an AOI on 
        // the image of co-localization plot 
        // CLOC_INVPARAMS - calculates second set of co-localization parameters 
        //UPGRADE_WARNING: 構造体 CLOC_ITYPES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpCoLocInverse(CLOC_ITYPES iType);
        [DllImport("IpCoLoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //Gets co-localization overlap parameters of original image 
        // where Stats is array of 10 single 
        //Dim Stats[10] as single 
        // return values 
        // Stats[0] - Pearson's correlation Rr 
        // Stats[1] - Overlap coefficient R 
        // Stats[2] - Overlap coefficient k1 
        // Stats[3] - Overlap coefficient k2 
        // Stats[4..9] - reserved 
        //UPGRADE_WARNING: 構造体 CL_COLOR_PAIRS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpCoLocGetForward(short SecondImageId, CL_COLOR_PAIRS ColorPair, ref float CLData);
        [DllImport("IpCoLoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]


        //Gets co-localization parameters on the base of AOI on 
        //the active of co-localization plot 
        // where Stats is array of 10 single 
        //Dim Stats[10] as single 
        // return values 
        // Stats[0] - Co-localization coefficient M1 
        // Stats[1] - Co-localization coefficient M2 
        // Stats[2..9] - reserved 
        public static extern int IpCoLocGetInverse(ref float CLData);
        [DllImport("IpCoLoc32", EntryPoint = "IpCoLocShow", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Original Solutions-Zone function names 
        public static extern int IpColcShow(short bShow);
        [DllImport("IpCoLoc32", EntryPoint = "IpCoLocForward", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColcForw(short SecondImageId, short ColorPair, short iType);
        [DllImport("IpCoLoc32", EntryPoint = "IpCoLocInverse", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColcInv(short iType);
        [DllImport("IpCoLoc32", EntryPoint = "IpCoLocGetForward", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColcGetForw(short SecondImageId, short ColorPair, ref float CLData);
        [DllImport("IpCoLoc32", EntryPoint = "IpCoLocGetInverse", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpColcGetInv(ref float CLData);
        [DllImport("IpCoLoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //Gets document IDs for images created by co-localization (IpCoLocForward) 
        //UPGRADE_WARNING: 構造体 CLOCDOC_TYPES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpCoLocGetDocument(CLOCDOC_TYPES DocType, ref short docId);

        // 
        // Definitions for Data Collector Auto-Pro Functions 
        // 
        // Copyright (c) 1998 - 2001, Media Cybernetics, Inc. 
        // 



        public const short DC_FETCH = 0;
        public const short DC_RESET = 1;
        public const short DC_RESETLAST = 2;

        public const short DC_AUTO = 10;
        public const short DC_AUTOMODE = 11;
        public const short DC_BREAK = 12;
        public const short DC_TOPLINE = 13;
        public const short DC_LEFTCOL = 14;
        public const short DC_COLWIDTH = 15;
        public const short DC_SIGNIF = 16;
        public const short DC_AUTO_SAVE = 17;
        public const short DC_LOG_NAME = 18;
        public const short DC_RETAIN_LAST = 19;
        public const short DC_BUFFER_LENGTH = 20;

        public const short DC_COL = 50;
        public const short DC_ROW = 51;

        public const short DC_NUMROW = 100;
        public const short DC_NUMCOL = 101;
        public const short DC_NUMBLOCK = 102;
        public const short DC_NUMVAL = 103;
        public const short DC_TYPE = 104;
        public const short DC_STATS = 105;
        public const short DC_BLOCKROW1 = 106;
        public const short DC_DATA = 107;
        public const short DC_NUMCUSTCOL = 108;
        public const short DC_CUSTCOLID = 109;

        public const short DC_CELL = 200;

        public const short DC_LOAD = 0;
        public const short DC_SAVE = 1;

        // maximum string item field length 
        public const short DC_MAXSTRINGLEN = 128;

        //data collector enumerations 

        // IpDcUpdate 
        public enum DCUPDT_ATTR
        {
            DCU_FETCH = Ipc32v5.DC_FETCH,
            DCU_RESET = Ipc32v5.DC_RESET,
            DCU_RESETLAST = Ipc32v5.DC_RESETLAST
        }

        // IpDcSet 
        public enum DCSET_ATTR
        {
            DCSET_AUTO = Ipc32v5.DC_AUTO,
            DCSET_AUTOMODE = Ipc32v5.DC_AUTOMODE,
            DCSET_BREAK = Ipc32v5.DC_BREAK,
            DCSET_TOPLINE = Ipc32v5.DC_TOPLINE,
            DCSET_LEFTCOL = Ipc32v5.DC_LEFTCOL,
            DCSET_COLWIDTH = Ipc32v5.DC_COLWIDTH,
            DCSET_SIGNIF = Ipc32v5.DC_SIGNIF,
            DCSET_AUTO_SAVE = Ipc32v5.DC_AUTO_SAVE,
            DCSET_RETAIN_LAST = Ipc32v5.DC_RETAIN_LAST,
            DCSET_BUFFER_LENGTH = Ipc32v5.DC_BUFFER_LENGTH,
            DCSET_COL = Ipc32v5.DC_COL,
            DCSET_ROW = Ipc32v5.DC_ROW
        }
        // IpDcGet 
        public enum DCGET_ATTR
        {
            DCGET_COL = Ipc32v5.DC_COL,
            DCGET_ROW = Ipc32v5.DC_ROW,
            DCGET_NUMROW = Ipc32v5.DC_NUMROW,
            DCGET_NUMCOL = Ipc32v5.DC_NUMCOL,
            DCGET_NUMBLOCK = Ipc32v5.DC_NUMBLOCK,
            DCGET_NUMVAL = Ipc32v5.DC_NUMVAL,
            DCGET_TYPE = Ipc32v5.DC_TYPE,
            DCGET_STATS = Ipc32v5.DC_STATS,
            DCGET_BLOCKROW1 = Ipc32v5.DC_BLOCKROW1,
            DCGET_DATA = Ipc32v5.DC_DATA,
            DCGET_NUMCUSTCOL = Ipc32v5.DC_NUMCUSTCOL,
            DCGET_CUSTCOLID = Ipc32v5.DC_CUSTCOLID
        }

        // IpDcGetStr 
        public enum DCGETSTR_ATTR
        {
            DCGETSTR_CELL = Ipc32v5.DC_CELL,
            DCGETSTR_COL = Ipc32v5.DC_COL
        }
        // IpDcSetStr 
        public enum DCSETSTR_ATTR
        {
            DCSETSTR_LOG_NAME = Ipc32v5.DC_LOG_NAME
        }

        //IpDcMeasList 
        public enum DCMLIST_COMMAND
        {
            DCML_LOAD = Ipc32v5.DC_LOAD,
            DCML_SAVE = Ipc32v5.DC_SAVE
        }

        //constants for IpDcCreateChart 
        public enum DCCHRT_TYPE
        {
            DCCHRT_GRAPH = 0,
            DCCHRT_HIST = 1,
            DCCHRT_SCAT = 2
        }
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]


        public static extern int IpDcShow(short bShow);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DCSET_ATTR に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDcSet(DCSET_ATTR sAttribute, int lData);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DCSETSTR_ATTR に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDcSetStr(DCSETSTR_ATTR sAttribute, string szName);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcSelect(string szModuleName, string szItemName, short sParam);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcUnSelect(string szModuleName, string szItemName, short sParam);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcSetVarName(string szModuleName, string szItemName, short sParam, string szVarName);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DCUPDT_ATTR に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDcUpdate(DCUPDT_ATTR bShow);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcSaveData(string szFileName, short sFlags);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 DCGET_ATTR に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDcGet(DCGET_ATTR sCmd, short sParam, ref int lpParam);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DCGETSTR_ATTR に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDcGetStr(DCGETSTR_ATTR sCmd, short sParam, string szString);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcAddCol(string szColumnName);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcDeleteCol(int lColID);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcAddSng(int lColID, short sNewBlock, short sNumRows, ref float lpfData);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDcAddStr(int lColID, short sNewBlock, short sRow, string szDataStr);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DCMLIST_COMMAND に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDcMeasList(DCMLIST_COMMAND sCommand, string szFileName);
        [DllImport("IPDATA32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DCCHRT_TYPE に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDcCreateChart(DCCHRT_TYPE sChartType);

        // ipcDemo.h 
        // 
        // Definitions for Macro Player Auto-Pro Functions 
        // 
        // Copyright (c) 2003 - 2004, Media Cybernetics, Inc. 
        // 



        // Macro Player Commands for the IpDemoSetStr and IpDemoGetStr functions 
        // obsolete - do not use 
        public const short DEMO_ATTR_LISTFILE = 1;
        // Important note: DEMO_ATTR_LISTFILE is obsolete and replaced by DEMO_ATTR_LISTPATH, as the Macro Player 
        // now lists the macros from all of the .MPL files in the current macro path. 
        // Set or get the folder to search for list files 
        public const short DEMO_ATTR_LISTPATH = 2;
        [DllImport("IPDEMO32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Macro Player Functions 
        //----------------------------------------------------------------- 
        public static extern int IpDemoShow(short bShow);
        [DllImport("IPDEMO32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpDemoSet(short sAttribute, short sParam, ref int lpData);
        [DllImport("IPDEMO32", EntryPoint = "IpDemoSet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDemoSetStr(short sCmd, short sParam, string lpParam);
        [DllImport("IPDEMO32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpDemoGet(short sAttribute, short sParam, ref int lpData);
        [DllImport("IPDEMO32", EntryPoint = "IpDemoGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDemoGetStr(short sCmd, short sParam, string lpParam);


        // 
        // Distance tool functions 
        // 
        // Copyright (c) 1999-2003, Media Cybernetics, Inc. 
        // 



        // commands for IpDistGetLong and IpDistSetLong 
        // read-only - do not use with IpDistSetLong 
        public const short DIST_NUM = 0;
        public const short DIST_COLOR = 1;
        public const short DIST_LINE_ENDS = 2;
        public const short DIST_DISP_NAME = 3;
        public const short DIST_DISP_UNITS = 4;
        public const short DIST_FONT_SIZE = 5;
        public const short DIST_FONT_WEIGHT = 6;
        public const short DIST_SIGN_DIGITS = 7;
        public const short DIST_LINE_WIDTH = 8;

        // commands for IpDistGetStr and IpDistSetStr 
        // sFeature ignored 
        public const short DIST_FONT_FACE = 10;
        // sFeature indicates feature of interest 
        public const short DIST_NAME = 11;

        // commands for IpDistGetSng 
        // return the distance value from the specified distance feature 
        public const short DIST_VALUE = 20;
        // return the statistics for all distance features as an array of 9 singles 
        public const short DIST_STATS = 21;


        //----------------------------------------------------------------- 
        // Distance Functions 
        //----------------------------------------------------------------- 

        // for IpDistShow 
        public enum DIST_TOOLS
        {
            DISTTOOL_NONE = Ipc32v5.MEAS_NONE,
            DISTTOOL_SELECT = Ipc32v5.MEAS_SELECT,
            DISTTOOL_DIST = Ipc32v5.MEAS_DIST
        }

        // for IpDistGetLong and IpDistSetLong 
        public enum DISTGETLNG_ATTRIBUTES
        {
            DISTGETLNG_NUM = Ipc32v5.DIST_NUM,
            DISTGETLNG_COLOR = Ipc32v5.DIST_COLOR,
            DISTGETLNG_LINE_ENDS = Ipc32v5.DIST_LINE_ENDS,
            DISTGETLNG_DISP_NAME = Ipc32v5.DIST_DISP_NAME,
            DISTGETLNG_DISP_UNITS = Ipc32v5.DIST_DISP_UNITS,
            DISTGETLNG_FONT_SIZE = Ipc32v5.DIST_FONT_SIZE,
            DISTGETLNG_FONT_WEIGHT = Ipc32v5.DIST_FONT_WEIGHT,
            DISTGETLNG_SIGN_DIGITS = Ipc32v5.DIST_SIGN_DIGITS,
            DISTGETLNG_LINE_WIDTH = Ipc32v5.DIST_LINE_WIDTH
        }

        // for IpDistGetLong and IpDistSetLong 
        public enum DISTSETLNG_ATTRIBUTES
        {
            DISTSETLNG_COLOR = Ipc32v5.DIST_COLOR,
            DISTSETLNG_LINE_ENDS = Ipc32v5.DIST_LINE_ENDS,
            DISTSETLNG_DISP_NAME = Ipc32v5.DIST_DISP_NAME,
            DISTSETLNG_DISP_UNITS = Ipc32v5.DIST_DISP_UNITS,
            DISTSETLNG_FONT_SIZE = Ipc32v5.DIST_FONT_SIZE,
            DISTSETLNG_FONT_WEIGHT = Ipc32v5.DIST_FONT_WEIGHT,
            DISTSETLNG_SIGN_DIGITS = Ipc32v5.DIST_SIGN_DIGITS,
            DISTSETLNG_LINE_WIDTH = Ipc32v5.DIST_LINE_WIDTH
        }

        // for IpDistGetStr and IpDistSetStr 
        public enum DISTSTR_ATTRIBUTES
        {
            DISTSTR_FONT_FACE = Ipc32v5.DIST_FONT_FACE,
            DISTSTR_NAME = Ipc32v5.DIST_NAME
        }

        // for IpDistGetSng 
        public enum DISTSNG_ATTRIBUTES
        {
            DISTSNG_VALUE = Ipc32v5.DIST_VALUE,
            DISTSNG_STATS = Ipc32v5.DIST_STATS
        }
        [DllImport("IPMSUR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpDistShow(short sShow);
        [DllImport("IPMSUR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DIST_TOOLS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDistTool(DIST_TOOLS tool);
        [DllImport("IPMSUR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDistTag(short sFeature, short OnOff);
        [DllImport("IPMSUR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDistDelete(short sFeature);
        [DllImport("IPMSUR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DISTGETLNG_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDistGetLong(DISTGETLNG_ATTRIBUTES sAttribute, ref int lplParam);
        [DllImport("IPMSUR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DISTSETLNG_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDistSetLong(DISTSETLNG_ATTRIBUTES sAttribute, int lValue);
        [DllImport("IPMSUR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DISTSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDistGetStr(DISTSTR_ATTRIBUTES sAttribute, short sFeature, string lpszParam);
        [DllImport("IPMSUR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DISTSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDistSetStr(DISTSTR_ATTRIBUTES sAttribute, short sFeature, string lpszParam);
        [DllImport("IPMSUR32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DISTSNG_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDistGetSng(DISTSNG_ATTRIBUTES sAttribute, short sParam, ref float lpfParam);


        // 
        // Image and File Signature functions 
        // 
        // Copyright (c) 2001 - , Media Cybernetics, Inc. 
        // 




        // Image Signature Commands 
        public const short IS_SIGNATURE = 1;
        public const short IS_SIGNATURE_STR = 2;
        public const short IS_COMPARE = 3;
        public const short IS_COMPARE_STR = 4;

        //----------------------------------------------------------------- 
        // Image Signature Functions 
        //----------------------------------------------------------------- 

        // Image Signature Commands 
        public enum ISGET_ATTRIBUTES
        {
            ISGET_SIGNATURE = Ipc32v5.IS_SIGNATURE,
            ISGET_COMPARE = Ipc32v5.IS_COMPARE
        }

        public enum ISGETSTR_ATTRIBUTES
        {
            ISGETSTR_SIGNATURE = Ipc32v5.IS_SIGNATURE_STR,
            ISGETSTR_COMPARE = Ipc32v5.IS_COMPARE_STR
        }
        [DllImport("IpDigSign32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpIsShow(short bShow);
        [DllImport("IpDigSign32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 ISGET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpIsGet(ISGET_ATTRIBUTES sAttribute, ref int lpData);
        [DllImport("IpDigSign32", EntryPoint = "IpIsGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 ISGETSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpIsGetStr(ISGETSTR_ATTRIBUTES sAttribute, string lpString);
        [DllImport("IpDigSign32", EntryPoint = "IpIsShow", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Original Solutions-Zone functions 
        public static extern int IpDsShow(short bShow);
        [DllImport("IpDigSign32", EntryPoint = "IpIsGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpDsGet(short sAttribute, ref int Data);
        [DllImport("IpDigSign32", EntryPoint = "IpIsGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDsGetStr(short sAttribute, string lpString);



        // File Signature Commands 
        public const short FS_SIGNATURE = 1;
        public const short FS_SIGNATURE_STR = 2;
        public const short FS_COMPARE = 3;
        public const short FS_COMPARE_STR = 4;
        [DllImport("IpDigSign32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Image Signature Functions 
        //----------------------------------------------------------------- 
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpFsGet(string lpFile, short sAttribute, ref int lpData);
        [DllImport("IpDigSign32", EntryPoint = "IpFsGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFsGetStr(string lpFile, short sAttribute, string lpString);


        // 
        // Dye management functions 
        // 
        // Copyright (c) 1999-2003 - , Media Cybernetics, Inc. 
        // 




        // commands for IpDyeGet 
        public const short DYE_WAVELENGTH = 1;
        public const short DYE_RGB_TINT = 2;
        public const short DYE_NUMDYES = 3;
        public const short DYE_EXWAVELENGTH = 4;

        // commands for IpDyeGetStr 
        // current dye folder, also used with IpDyeSetStr 
        public const short DYE_PATH = 10;
        public const short DYE_LIST = 11;
        public const short DYE_ACTIVEDYE = 12;

        //----------------------------------------------------------------- 
        // Dye Functions 
        //----------------------------------------------------------------- 

        // Dye function enumerations 

        // for IpDyeGet 
        public enum DYEGET_ATTRIBUTES
        {
            DYEGET_WAVELENGTH = Ipc32v5.DYE_WAVELENGTH,
            DYEGET_RGB_TINT = Ipc32v5.DYE_RGB_TINT,
            DYEGET_NUMDYES = Ipc32v5.DYE_NUMDYES,
            DYEGET_EXWAVELENGTH = Ipc32v5.DYE_EXWAVELENGTH
        }

        // for IpDyeGetStr 
        public enum DYEGETSTR_ATTRIBUTES
        {
            DYEGETSTR_PATH = Ipc32v5.DYE_PATH,
            DYEGETSTR_LIST = Ipc32v5.DYE_LIST,
            DYEGETSTR_ACTIVEDYE = Ipc32v5.DYE_ACTIVEDYE
        }

        // for IpDyeSetStr 
        public enum DYESETSTR_ATTRIBUTES
        {
            DYESETSTR_PATH = Ipc32v5.DYE_PATH,
            DYESETSTR_ACTIVEDYE = Ipc32v5.DYE_ACTIVEDYE
        }
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpDyeSelect(string Dye);
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDyeAdd(string Dye, int WL, int ExWL);
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 DYEGET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDyeGet(string Dye, DYEGET_ATTRIBUTES sAttribute, ref int Value);
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for DYE_PATH and DYE_LIST 
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpDyeGetStr(string Dye, short Command_Renamed, short index, string Value);
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for DYE_PATH 
        //UPGRADE_WARNING: 構造体 DYESETSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpDyeSetStr(string Dye, DYESETSTR_ATTRIBUTES sAttribute, string Value);
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDyeAddTint(string Dye, int WL, int ExWL, int Tint);
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDyeApply(string Dye, short ApplyTo, short ApplyTint);
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDyeEdit(string Dye, string newDye);
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDyeDelete(string Dye);
        [DllImport("IPDYE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // NOTE: This function will not work from external programs, and is intended primarily for internal use. 
        public static extern int IpDyeApplyToImage(string Dye, int Image, short ApplyTo, short ApplyTint);

        // 
        // Definitions for Extended Depth of Field (EDF) Auto-Pro Functions and constants 
        // 
        // Copyright (c) 2001 - , Media Cybernetics, Inc. 
        // 



        // Multi-Plane Focus output types 
        public const short EDF_COMPOSITE = 1;
        public const short EDF_BEST_FOCUS = 2;
        // combine with one of the others to request analysis but no output 
        public const int EDF_ANALYZE_ONLY = 0x10;

        // Get/Set Commands 
        public const short EDF_NORMALIZE = 1;
        public const short EDF_CRITERIA = 2;
        public const short EDF_TOPO_MAP = 3;
        public const short EDF_TOPO_CALIBRATED = 4;
        public const short EDF_ORDER = 5;
        public const short EDF_DEFAULT_FRAME = 6;
        // Reserved for future use 
        public const short EDF_BEST_PLANE = 7;
        // Note: For use with IpEDFGet only 
        public const short EDF_NUM_PLANES = 8;
        public const short EDF_TS_MAP = 9;
        public const short EDF_TS_GALLERY = 10;
        public const short EDF_TOPO_SURFACE_PLOT = 11;

        // Analysis criteria 
        // Note: these first three options 
        public const short EDF_MAX_LOCALCONTRAST = 0;
        // use the same numerical constants as 
        public const short EDF_MAX_INTENSITY = 1;
        // Scope-Pro 
        public const short EDF_MIN_INTENSITY = 2;
        public const short EDF_MAX_DEPTHCONTRAST = 3;
        public const short EDF_HFE_SMALL = 4;
        public const short EDF_HFE_MEDIUM = 5;
        public const short EDF_HFE_LARGE = 6;

        // image order 
        public const short EDF_TOPDOWN = 0;
        public const short EDF_BOTTOMUP = 1;

        //----------------------------------------------------------------- 
        // Extended Depth of Field Functions 
        //----------------------------------------------------------------- 

        // Extended Depth of Field function enumerations 

        // for IpEDFCreate 
        public enum EDFCREATE_TYPES
        {
            EDFTYPE_COMPOSITE = Ipc32v5.EDF_COMPOSITE,
            EDFTYPE_BEST_FOCUS = Ipc32v5.EDF_BEST_FOCUS,
            EDFTYPE_ANALYZE_COMPOSITE = Ipc32v5.EDF_COMPOSITE + Ipc32v5.EDF_ANALYZE_ONLY,
            EDFTYPE_ANALYZE_BEST_FOCUS = Ipc32v5.EDF_BEST_FOCUS + Ipc32v5.EDF_ANALYZE_ONLY
        }

        // for IpEDFGet 
        public enum EDFGET_ATTRIBUTES
        {
            EDFGET_NORMALIZE = Ipc32v5.EDF_NORMALIZE,
            EDFGET_CRITERIA = Ipc32v5.EDF_CRITERIA,
            EDFGET_TOPO_MAP = Ipc32v5.EDF_TOPO_MAP,
            EDFGET_TOPO_CALIBRATED = Ipc32v5.EDF_TOPO_CALIBRATED,
            EDFGET_ORDER = Ipc32v5.EDF_ORDER,
            EDFGET_DEFAULT_FRAME = Ipc32v5.EDF_DEFAULT_FRAME,
            EDFGET_BEST_PLANE = Ipc32v5.EDF_BEST_PLANE,
            EDFGET_NUM_PLANES = Ipc32v5.EDF_NUM_PLANES,
            EDFGET_TS_MAP = Ipc32v5.EDF_TS_MAP,
            EDFGET_TS_GALLERY = Ipc32v5.EDF_TS_GALLERY,
            EDFGET_TOPO_SURFACE_PLOT = Ipc32v5.EDF_TOPO_SURFACE_PLOT
        }

        // for IpEDFGet 
        public enum EDFSET_ATTRIBUTES
        {
            EDFSET_NORMALIZE = Ipc32v5.EDF_NORMALIZE,
            EDFSET_CRITERIA = Ipc32v5.EDF_CRITERIA,
            EDFSET_TOPO_MAP = Ipc32v5.EDF_TOPO_MAP,
            EDFSET_TOPO_CALIBRATED = Ipc32v5.EDF_TOPO_CALIBRATED,
            EDFSET_ORDER = Ipc32v5.EDF_ORDER,
            EDFSET_DEFAULT_FRAME = Ipc32v5.EDF_DEFAULT_FRAME,
            EDFSET_BEST_PLANE = Ipc32v5.EDF_BEST_PLANE,
            EDFSET_TS_MAP = Ipc32v5.EDF_TS_MAP,
            EDFSET_TS_GALLERY = Ipc32v5.EDF_TS_GALLERY,
            EDFSET_TOPO_SURFACE_PLOT = Ipc32v5.EDF_TOPO_SURFACE_PLOT
        }
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpEDFShow(short Show);
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpEDFNew(short docId);
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpEDFAdd(short docId);
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpEDFRemove(short docId);
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 EDFCREATE_TYPES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpEDFCreate(EDFCREATE_TYPES iType);
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpEDFTopoMap();
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 EDFGET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpEDFGet(EDFGET_ATTRIBUTES sAttribute, ref short CurrValue, ref int CurrFrame);
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 EDFSET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpEDFSet(EDFSET_ATTRIBUTES sAttribute, short NewValue, int NewFrame);
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpEDFGetConf(ref float ConfidenceArray);
        [DllImport("IpFoc32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpEDFTestStrips();


        //*********************************************************************** 
        // FTP functions 
        //*********************************************************************** 


        public const short FTP_DUMMY = 0;
        [DllImport("IPFTP32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpFTPOpen(string server, string remotefile);
        [DllImport("IPFTP32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpFTPSave(string server, string remotefile);

        //----------------------------------------------------------------- 
        // Image Database/Gallery functions 
        //----------------------------------------------------------------- 




        // IpDbSearch 

        public const short OP_EQUAL = 0;
        public const short OP_LT = 1;
        public const short OP_LE = 2;
        public const short OP_GT = 3;
        public const short OP_GE = 4;
        public const short OP_LIKE = 5;
        public const short OP_NOTLIKE = 6;

        public const short DB_INT = 0;
        public const short DB_LONG = 1;
        public const short DB_STRING = 2;
        public const short DB_BINARY = 3;
        public const short DB_MEMO = 4;
        public const short DB_FILE = 5;

        public const short DB_CAPTION = 21;
        public const short DB_COPYCUSTOM = 22;

        // IpDbGoto 
        public const short DB_FIRST = -1;
        public const short DB_LAST = -2;
        public const short DB_NEXT = -3;
        public const short DB_PREV = -4;
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]


        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpDbWrite(string szFieldName, short FieldType, ref int szFieldValue, int DataLength);
        [DllImport("IPGALI32", EntryPoint = "IpDbWrite", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for DB_STRING writes 
        public static extern int IpDbWriteStr(string szFieldName, short FieldType, string FieldValue, int DataLength);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpDbRead(string szFieldName, short FieldType, ref int szFieldValue, int DataLength);
        [DllImport("IPGALI32", EntryPoint = "IpDbRead", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for DB_STRING reads 
        public static extern int IpDbReadStr(string szFieldName, short FieldType, string FieldValue, int DataLength);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbGoto(short RecordNum);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_NOTE: Operator は Operator_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpDbSearch(string szFieldName, short FieldType, short Operator_Renamed, ref int FieldValue);
        [DllImport("IPGALI32", EntryPoint = "IpDbSearch", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for DB_STRING searches 
        //UPGRADE_NOTE: Operator は Operator_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpDbSearchStr(string szFieldName, short FieldType, short Operator_Renamed, string FieldValue);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_NOTE: Operator は Operator_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpDbFind(string szFieldName, short FieldType, short Operator_Renamed, ref int FieldValue);
        [DllImport("IPGALI32", EntryPoint = "IpDbFind", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for DB_STRING searches 
        //UPGRADE_NOTE: Operator は Operator_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpDbFindStr(string szFieldName, short FieldType, short Operator_Renamed, string FieldValue);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbViewFolder(string szFolderName);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbViewAll();
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbLoadView(string szViewName);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbNewFolder(string szFolderName, string szDescription);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbOpenFolder(string szFolderName);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbPrint(short sLayout);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbAddField(string szFieldName, short FieldType, short FieldLength);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpDbSetAttr(short Attrib, short nValue, string strValue);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpDbWriteNum(string szFieldName, short FieldType, ref int szFieldValue, int DataLength);
        [DllImport("IPGALI32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpDbReadNum(string szFieldName, short FieldType, ref int szFieldValue, int DataLength);



        // 
        // Definitions for Grid Auto-Pro Functions 
        // 
        // Copyright (c) 1998 - 2001, Media Cybernetics, Inc. 
        // 



        //----------------------------------------------------------------- 
        // Grid mask functions 
        //----------------------------------------------------------------- 

        public const short GRID_CALIBFLAG_PIXEL = 1;
        public const short GRID_CALIBFLAG_IMAGE = 2;

        public const short GRID_ATTR_OBJECT = 1;
        public const short GRID_ATTR_LAYOUT = 2;
        public const short GRID_ATTR_DISPLAYAS = 3;
        public const short GRID_ATTR_HSPACE = 4;
        public const short GRID_ATTR_VSPACE = 5;
        public const short GRID_ATTR_RSPACE = 6;
        public const short GRID_ATTR_CHECKERED = 7;
        public const short GRID_ATTR_FLAGRANDSEED = 8;
        public const short GRID_ATTR_VALRANDSEED = 9;
        public const short GRID_ATTR_COUNT = 10;
        public const short GRID_ATTR_LENGTH = 11;
        public const short GRID_ATTR_HLENGTH = 12;
        public const short GRID_ATTR_VLENGTH = 13;
        public const short GRID_ATTR_LMARGIN = 14;
        public const short GRID_ATTR_TMARGIN = 15;
        public const short GRID_ATTR_RMARGIN = 16;
        public const short GRID_ATTR_BMARGIN = 17;
        public const short GRID_ATTR_FULLSIZE = 18;
        public const short GRID_ATTR_COLOR = 19;
        public const short GRID_ATTR_PENWIDTH = 20;

        public const short GRID_OBJECT_POINT = 1;
        public const short GRID_OBJECT_LINE = 2;
        public const short GRID_OBJECT_LINESGM = 3;
        public const short GRID_OBJECT_CIRCLE = 4;
        public const short GRID_OBJECT_CYCLOID = 5;

        public const short GRID_LAYOUT_ORTHOGONAL = 1;
        public const short GRID_LAYOUT_CONCENTRIC = 2;
        public const short GRID_LAYOUT_RANDOM = 3;

        public const short GRID_POINT_CIRCLE_LRG = 1;
        public const short GRID_POINT_CIRCLE_SML = 2;
        public const short GRID_POINT_CROSS_LRG90 = 3;
        public const short GRID_POINT_CROSS_LRG45 = 4;
        public const short GRID_POINT_CROSS_SML90 = 5;
        public const short GRID_POINT_CROSS_SML45 = 6;
        public const short GRID_POINT_MED = 7;
        public const short GRID_POINT_RECT_LRG = 8;
        public const short GRID_POINT_RECT_SML = 9;
        public const short GRID_POINT_DIAMOND_LRG = 10;
        public const short GRID_POINT_DIAMOND_SML = 11;
        public const short GRID_POINT_STAR8 = 12;
        public const short GRID_POINT_THREEUP = 13;
        public const short GRID_POINT_THREEDOWN = 14;
        [DllImport("IPGRID32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpGridShow(short nValue);
        [DllImport("IPGRID32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGridSelect(string szGridFile);
        [DllImport("IPGRID32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGridApply(short nValue);
        [DllImport("IPGRID32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpGridCreateMask();

        // 
        // Definitions for Image Data Overlay Auto-Pro Functions 
        // 
        // Copyright (c) 1999-2006, Media Cybernetics, Inc. 
        // 



        // Commands 
        public const short IOVR_CURRENTBCG = 10000;
        public const short IOVR_APPLIEDBCG = 10010;
        public const short IOVR_CAPTUREDEV = 10020;
        public const short IOVR_EXPOSURE = 10030;
        public const short IOVR_ACCUMULATED = 10040;
        public const short IOVR_DATE = 10050;
        public const short IOVR_TIME = 10060;
        public const short IOVR_FILENAME = 10070;
        public const short IOVR_IMAGESIGN = 10080;
        public const short IOVR_OVRLIMAGE = 10090;
        public const short IOVR_OVRLPRINT = 10110;
        public const short IOVR_LOCATION = 10120;
        public const short IOVR_SETINFO = 10130;
        public const short IOVR_POSITION = 10140;
        public const short IOVR_CHANNEL = 10150;

        // these are defined for FONTINFO 
        public const short IOVR_FONT_FACE = 10200;
        public const short IOVR_FONT_STYLE = 10210;
        public const short IOVR_FONT_SIZE = 10220;
        public const short IOVR_FONT_AFFECTS = 10230;
        public const short IOVR_FONT_COLOR = 10240;

        // these are for FONT_STYLE 
        public const short IOVR_FONT_NORMAL = 0;
        public const short IOVR_FONT_ITALIC = 1;
        public const short IOVR_FONT_BOLD = 2;

        // these are for FONT_AFFECTS 
        public const short IOVR_FONT_NOEFFECTS = 0;
        public const short IOVR_FONT_STRIKEOUT = 1;
        public const short IOVR_FONT_UNDERLINE = 2;

        // these are for IOVR_LOCATION 
        public const short IOVR_LOC_UPPERLEFT = 0;
        public const short IOVR_LOC_LOWERLEFT = 1;
        public const short IOVR_LOC_UPPERRIGHT = 2;
        public const short IOVR_LOC_LOWERRIGHT = 3;

        // these are for Apply to new image command. 
        public const short IOVR_LOC_HEADER = 0;
        public const short IOVR_LOC_FOOTER = 1;

        public const short IOVR_COLOR_WHITE = 0;
        public const short IOVR_COLOR_GRAY = 1;
        public const short IOVR_COLOR_BLACK = 2;

        public enum IMAGEOVERLAY_ATTRIBUTES
        {
            IMAGEOVERLAY_CURRENTBCG = Ipc32v5.IOVR_CURRENTBCG,
            IMAGEOVERLAY_APPLIEDBCG = Ipc32v5.IOVR_APPLIEDBCG,
            IMAGEOVERLAY_EXPOSURE = Ipc32v5.IOVR_EXPOSURE,
            IMAGEOVERLAY_ACCUMULATED = Ipc32v5.IOVR_ACCUMULATED,
            IMAGEOVERLAY_DATE = Ipc32v5.IOVR_DATE,
            IMAGEOVERLAY_TIME = Ipc32v5.IOVR_TIME,
            IMAGEOVERLAY_FILENAME = Ipc32v5.IOVR_FILENAME,
            IMAGEOVERLAY_IMAGESIGN = Ipc32v5.IOVR_IMAGESIGN,
            IMAGEOVERLAY_OVRLIMAGE = Ipc32v5.IOVR_OVRLIMAGE,
            IMAGEOVERLAY_OVRLPRINT = Ipc32v5.IOVR_OVRLPRINT,
            IMAGEOVERLAY_LOCATION = Ipc32v5.IOVR_LOCATION,
            IMAGEOVERLAY_SETINFO = Ipc32v5.IOVR_SETINFO,
            IMAGEOVERLAY_POSITION = Ipc32v5.IOVR_POSITION,
            IMAGEOVERLAY_CHANNEL = Ipc32v5.IOVR_CHANNEL,
            IMAGEOVERLAY_FONT_STYLE = Ipc32v5.IOVR_FONT_STYLE,
            IMAGEOVERLAY_FONT_SIZE = Ipc32v5.IOVR_FONT_SIZE,
            IMAGEOVERLAY_FONT_AFFECTS = Ipc32v5.IOVR_FONT_AFFECTS,
            IMAGEOVERLAY_FONT_COLOR = Ipc32v5.IOVR_FONT_COLOR
        }

        public enum IMAGEOVERLAY_STR_ATTRIBUTES
        {
            IMAGEOVERLAY_FONT_FACE = Ipc32v5.IOVR_FONT_FACE
        }

        public enum IMAGEOVERLAY_POSITIONS
        {
            IMAGEOVERLAY_POS_HEADER = Ipc32v5.IOVR_LOC_HEADER,
            IMAGEOVERLAY_POS_FOOTER = Ipc32v5.IOVR_LOC_FOOTER
        }

        public enum IMAGEOVERLAY_FILLCOLORS
        {
            IMAGEOVERLAY_COLOR_WHITE = Ipc32v5.IOVR_LOC_HEADER,
            IMAGEOVERLAY_COLOR_GRAY = Ipc32v5.IOVR_COLOR_GRAY,
            IMAGEOVERLAY_COLOR_BLACK = Ipc32v5.IOVR_COLOR_BLACK
        }
        [DllImport("IPIIOVRL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Image data overlay functions 
        //----------------------------------------------------------------- 
        public static extern int IpIOvrShow(short bShow);
        [DllImport("IPIIOVRL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 IMAGEOVERLAY_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpIOvrSet(IMAGEOVERLAY_ATTRIBUTES sAttribute, short sParam, ref int lpData);
        [DllImport("IPIIOVRL32", EntryPoint = "IpIOvrSet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 IMAGEOVERLAY_STR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpIOvrSetStr(IMAGEOVERLAY_STR_ATTRIBUTES sCmd, short sParam, string lpParam);
        [DllImport("IPIIOVRL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 IMAGEOVERLAY_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpIOvrGet(IMAGEOVERLAY_ATTRIBUTES sAttribute, short sParam, ref int lpData);
        [DllImport("IPIIOVRL32", EntryPoint = "IpIOvrGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 IMAGEOVERLAY_STR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpIOvrGetStr(IMAGEOVERLAY_STR_ATTRIBUTES sCmd, short sParam, string lpParam);
        [DllImport("IPIIOVRL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 IMAGEOVERLAY_FILLCOLORS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 IMAGEOVERLAY_POSITIONS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpIOvrApply(IMAGEOVERLAY_POSITIONS Position, IMAGEOVERLAY_FILLCOLORS FillColor, short bApplyDataOL);

        // 
        // Lens management functions 
        // 
        // Copyright (c) 1999-2003 - , Media Cybernetics, Inc. 
        // 




        // commands for IpLensGetSng 
        public const short LENS_MAGNIFICATION = 1;
        // numeric aperture 
        public const short LENS_NA = 2;
        // reflective index 
        public const short LENS_RI = 3;

        // commands for IpLensGetLong 
        // number of lens in current lens folder 
        public const short LENS_NUMLENSES = 10;

        // commands for IpLensGetStr 
        // current lens folder, also used with IpLensSetStr 
        public const short LENS_PATH = 20;
        // return a lens from the list 
        public const short LENS_LIST = 21;
        // get or set the active lens (Note: changes the system calibration!) 
        public const short LENS_ACTIVELENS = 22;

        //----------------------------------------------------------------- 
        // Lens Functions 
        //----------------------------------------------------------------- 

        // Lens function enumerations 

        // for IpLensGetLong 
        public enum LENSGETLNG_ATTRIBUTES
        {
            LENSGETLNG_NUMLENSES = Ipc32v5.LENS_NUMLENSES
        }

        // for IpLensGetSng 
        public enum LENSGETSNG_ATTRIBUTES
        {
            LENSGETSNG_MAGNIFICATION = Ipc32v5.LENS_MAGNIFICATION,
            LENSGETSNG_NA = Ipc32v5.LENS_NA,
            LENSGETSNG_RI = Ipc32v5.LENS_RI
        }

        // for IpLensGetStr 
        public enum LENSGETSTR_ATTRIBUTES
        {
            LENSGETSTR_PATH = Ipc32v5.LENS_PATH,
            LENSGETSTR_LIST = Ipc32v5.LENS_LIST,
            LENSGETSTR_ACTIVELENS = Ipc32v5.LENS_ACTIVELENS
        }

        // for IpLensSetStr 
        public enum LENSSETSTR_ATTRIBUTES
        {
            LENSSETSTR_PATH = Ipc32v5.LENS_PATH,
            LENSSETSTR_ACTIVELENS = Ipc32v5.LENS_ACTIVELENS
        }
        [DllImport("IPLENS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpLensSelect(string Lens);
        [DllImport("IPLENS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLensAdd(string Lens, float Magnification, float NA, float RI);
        [DllImport("IPLENS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for LENS_NUMLENSES 
        //UPGRADE_WARNING: 構造体 LENSGETLNG_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpLensGetLong(LENSGETLNG_ATTRIBUTES sAttribute, ref int lpParam);
        [DllImport("IPLENS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for LENS_MAGNIFICATION, LENS_NA and LENS_RI 
        //UPGRADE_WARNING: 構造体 LENSGETSNG_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpLensGetSng(string Lens, LENSGETSNG_ATTRIBUTES sAttribute, ref float lpParam);
        [DllImport("IPLENS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for LENS_PATH and LENS_LIST 
        //UPGRADE_WARNING: 構造体 LENSGETSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpLensGetStr(LENSGETSTR_ATTRIBUTES sAttribute, short index, string Value);
        [DllImport("IPLENS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use for LENS_PATH 
        //UPGRADE_WARNING: 構造体 LENSSETSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpLensSetStr(LENSSETSTR_ATTRIBUTES sAttribute, string Value);
        [DllImport("IPLENS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // Use to apply lens characteristics to the active image 
        public static extern int IpLensApply(string Lens, short ApplyTo);
        [DllImport("IPLENS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLensEdit(string Lens, string newLens);
        [DllImport("IPLENS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLensDelete(string Lens);
        [DllImport("IPLENS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // NOTE: This function will not work from external programs, and is intended primarily for internal use. 
        public static extern int IpLensApplyToImage(string Lens, int Image, short ApplyTo);

        // 
        // Definitions for Auto-Pro Functions of Live EDF 
        // 
        // Copyright (c) 1999 - 2008, Media Cybernetics, Inc. 
        // 




        //live composition modes used with LIVEEDF_COMP_MODE 
        public const short LIVECOMP_LOCAL_CONTRAST = 0;
        public const short LIVECOMP_MAX = 1;
        public const short LIVECOMP_MIN = 2;
        public const short LIVECOMP_DIFF = 3;
        public const short LIVECOMP_ABS_DIFF = 4;

        //live composition modes used with 
        //normal view of EDF 
        public const short DUALVIEW_NONE = 0;
        //horizontal views side by side 
        public const short DUALVIEW_HORIZONTAL = 1;
        //normal view of the live image 
        public const short DUALVIEW_LIVE = 2;
        //picture in picture EDF in the corner 
        public const short DUALVIEW_PIP_EDF = 3;
        //picture in picture Live in the corner 
        public const short DUALVIEW_PIP_LIVE = 4;

        //Wavelet filter 
        //Daubechies 7-9 biorthogonal filters 
        public const short LIVEEDF_WVF_D79 = 0;
        //Haar filters 
        public const short LIVEEDF_WVF_HAAR = 1;
        //Daubechies 8 tap orthogonal 
        public const short LIVEEDF_WVF_D8 = 2;
        //Antonini 7-9 biorthogonal filters 
        public const short LIVEEDF_WVF_A79 = 3;

        //EDF algorithm 
        //variance algorithm 
        public const short LIVEEDF_ALG_VAR = 0;
        //wavelet algorithm 
        public const short LIVEEDF_ALG_WAVELET = 1;

        // LiveEDF Commands for the IpLiveEDFSetInt 
        //set base image to EDF 
        public const short LIVEEDF_LOWER_IMAGE = 1;
        //activate stereo mode (auto-alignment) 
        public const short LIVEEDF_STEREO_MODE = 2;
        //do EDF of the current image with the base image (not adding it to the base, see LIVEEDF_ADD_TO_EDF) 
        public const short LIVEEDF_DO_EDF = 3;
        //variance filter size 
        public const short LIVEEDF_FILTER_SIZE = 4;
        //auto-alignment pattern size in horizontal direction 
        public const short LIVEEDF_SEARCH_SIZE_H = 5;
        //activate live EDF (image is update on ImageChange event, fired by workspace preview 
        public const short LIVEEDF_ACTIVATE = 6;
        //in live mode use accumulated EDF 
        public const short LIVEEDF_MULTIFRAME = 7;
        //sets Dual View mode (see DUALVIEW_ options) 
        public const short LIVEEDF_DUAL_VIEW = 8;
        //composition modes (see LIVECOMP_ options) 
        public const short LIVEEDF_COMP_MODE = 9;
        //do EDF, adding current image to the Base image 
        public const short LIVEEDF_ADD_TO_EDF = 10;
        //create image of forward wavelet transform 
        public const short LIVEEDF_FORWARD_WT_IMAGE = 11;
        //create image of inverse wavelet transform 
        public const short LIVEEDF_INVERSE_WT_IMAGE = 12;
        //set wavelet filter type (see LIVEEDF_WVF_ options) 
        public const short LIVEEDF_WV_FILTER = 13;
        //set EDF algorithm, used only when LIVEEDF_COMP_MODE is LIVECOMP_LOCAL_CONTRAST (see LIVEEDF_ALG_ options) 
        public const short LIVEEDF_ALGORITHM = 14;
        //using FULL FFT for alignment in stereo mode, if 0 - phase only alignment is used 
        public const short LIVEEDF_FULL_FFT = 15;
        //align image using the previous result (if 0 the first image will be used as search pattern) 
        public const short LIVEEDF_ALIGN_BY_PREV = 16;
        //size of the blending area along edges of zones, if 0 (default) no blending is used 
        public const short LIVEEDF_BLENDING_RADIUS = 17;
        //create output image, sParam defines the image type (0 - lower image, 1-var lower image, 2-wavelet lower image, 3 - upper image, 4 - var upper image) 
        public const short LIVEEDF_CREATE_IMAGE = 18;
        //auto-alignment pattern size in vertical direction 
        public const short LIVEEDF_SEARCH_SIZE_V = 19;
        //first capture step 
        public const short LIVEEDF_CAPTURE = 20;
        //second capture step 
        public const short LIVEEDF_CAPTURE2 = 21;

        //activate live tiling 
        public const short LIVETILING_ACTIVATE = 50;
        //set search image 
        public const short LIVETILING_SEARCH_IMAGE = 51;
        //add tile 
        public const short LIVETILING_ADD_TILE = 52;
        //set background image 
        public const short LIVETILING_BACK_IMAGE = 53;
        //set search image and add output 
        public const short LIVETILING_SEARCH_IMAGE_ADD = 54;
        //color of overlay rectangle 
        public const short LIVETILING_OVL_COLOR = 55;
        //color of error overlay rectangle 
        public const short LIVETILING_OVL_COLOR_ERROR = 56;
        //overlay line width 
        public const short LIVETILING_OVL_WIDTH = 57;
        //number of Undo steps 
        public const short LIVETILING_NUM_UNDOS = 58;

        //get 
        public const short LIVEEDF_FPS = 201;
        [DllImport("IPLIVEEDF", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // LiveEDF Functions 
        //----------------------------------------------------------------- 
        public static extern int IpLiveEDFShow(short bShow);
        [DllImport("IPLIVEEDF", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpLiveEDFSet(short sAttribute, short sParam, ref int lpData);
        [DllImport("IPLIVEEDF", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLiveEDFSetInt(short sAttribute, short sParam, int lData);
        [DllImport("IPLIVEEDF", EntryPoint = "IpLiveEDFSet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLiveEDFSetStr(short sCmd, short sParam, string lpParam);
        [DllImport("IPLIVEEDF", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpLiveEDFGet(short sAttribute, short sParam, ref int lpData);
        [DllImport("IPLIVEEDF", EntryPoint = "IpLiveEDF1Get", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLiveEDFGetStr(short sCmd, short sParam, string lpParam);
        [DllImport("IPLIVEEDF", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLiveTileSetInt(short sAttribute, short sParam, int lData);


        // 
        // Local Zoom functions and constants 
        // 
        // Copyright (c) 1999-2003 Media Cybernetics, Inc. 
        // 




        // Commands for IpLocZoomGet/Set 
        // set zoom 
        public const short IP_LZ_ZOOM = 100;
        // show/hide cross 
        public const short IP_LZ_CROSS = 101;
        // is local zoom shown? 
        public const short IP_LZ_ISSHOWN = 102;

        //LZ enumerations 

        // Commands for IpLocZoomGet/Set 
        public enum LZSET_COMMAND
        {
            LZSET_ZOOM = Ipc32v5.IP_LZ_ZOOM,
            // set zoom 
            LZSET_CROSS = Ipc32v5.IP_LZ_CROSS
            // show/hide cross 
        }
        public enum LZGET_COMMAND
        {
            LZGET_ZOOM = Ipc32v5.IP_LZ_ZOOM,
            // set zoom 
            LZGET_CROSS = Ipc32v5.IP_LZ_CROSS,
            // show/hide cross 
            LZGET_ISSHOWN = Ipc32v5.IP_LZ_ISSHOWN
            // is local zoom shown? 
        }
        [DllImport("IPLOCZOOM32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]


        //----------------------------------------------------------------- 
        // Local Zoom Functions 
        //----------------------------------------------------------------- 
        public static extern int IpLocZoomShow(short bShow);
        [DllImport("IPLOCZOOM32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLocZoomMove(short xPos, short yPos);
        [DllImport("IPLOCZOOM32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLocZoomSize(short xSize, short ySize);
        [DllImport("IPLOCZOOM32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpLocZoomSetPos(short xPos, short yPos);
        [DllImport("IPLOCZOOM32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 LZSET_COMMAND に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpLocZoomSet(LZSET_COMMAND sCommand, short sVal);
        [DllImport("IPLOCZOOM32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 LZGET_COMMAND に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpLocZoomGet(LZGET_COMMAND sCommand, ref short sVal);


        // 
        // Definitions for Large Spectral Filters Auto-Pro Functions 
        // 
        // Copyright (c) 1998 - 2001 , Media Cybernetics, Inc. 
        // 




        public const short LF_LOPASS = 0;
        public const short LF_HIPASS = 1;
        public const short LF_EDGEPL = 2;
        public const short LF_EDGEMN = 3;
        public const short LF_BANDPASS = 4;

        //LF enumerations 

        public enum LSF_TYPES
        {
            LSFT_LOPASS = Ipc32v5.LF_LOPASS,
            LSFT_HIPASS = Ipc32v5.LF_HIPASS,
            LSFT_EDGEPL = Ipc32v5.LF_EDGEPL,
            LSFT_EDGEMN = Ipc32v5.LF_EDGEMN,
            LSFT_BANDPASS = Ipc32v5.LF_BANDPASS
        }
        [DllImport("IpLrg32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]


        public static extern int IpLSFltShow(short bShow);
        [DllImport("IpLrg32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 LSF_TYPES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpLSFltApply(LSF_TYPES sType, short Width, short Height, short Passes, short Strength);
        [DllImport("IpLrg32", EntryPoint = "IpLSFltShow", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // Original Solutions-Zone function names 
        public static extern int IpLFltShow(short bShow);
        [DllImport("IpLrg32", EntryPoint = "IpLSFltApply", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 LSF_TYPES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpLFltApply(LSF_TYPES sType, short Width, short Height, short Passes, short Strength);
        [DllImport("IPMAIL32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // 
        // Mail functions 
        // 




        public static extern int IpMail(string mailTo, string mailCC, string mailSubj, string mailMsg, string mailAttach);

        // 
        // Definitions for Memory Monitor Auto-Pro Functions 
        // 
        // Copyright (c) 2004, Media Cybernetics, Inc. 
        // 



        // if this include file is included by task.h, only one of these set of macros should 
        // be defined. 

        // IpMmonShow commands 
        public const short MMON_HIDE = 0;
        public const short MMON_SHOW = 1;
        public const short MMON_MAXIMIZE = 2;
        public const short MMON_MINIMIZE = 3;

        // Get/Set Enable/disable 
        public const short MMON_VMENABLE = 1;
        [DllImport("IPMMON32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Mmon Functions 
        //----------------------------------------------------------------- 
        public static extern int IpMmonShow(short bShow);
        [DllImport("IPMMON32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMmonSetInt(short sAttribute, short sParam, short sValue);
        [DllImport("IPMMON32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpMmonSet(short sAttribute, short sParam, ref int lpData);
        [DllImport("IPMMON32", EntryPoint = "IpMmonSet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMmonSetStr(short sCmd, short sParam, string lpParam);
        [DllImport("IPMMON32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpMmonGet(short sAttribute, short sParam, ref int lpData);
        [DllImport("IPMMON32", EntryPoint = "IpMmonGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMmonGetStr(short sCmd, short sParam, string lpParam);


        // 
        // Definitions for Mosaic Auto-Pro Constants and Functions 
        // 
        // Copyright (c) 1998 - 2001, Media Cybernetics, Inc. 
        // 





        // Attributes 
        public const short MA_IMAGESIZE = 1;
        public const short MA_IMAGEWIDTH = 2;
        public const short MA_IMAGEHEIGHT = 3;
        public const short MA_IMAGECLASS = 4;
        public const short MA_ROWS = 5;
        public const short MA_COLUMNS = 6;
        public const short MA_SPACING = 7;
        public const short MA_TITLE = 8;
        public const short MA_FOOTER = 9;
        public const short MA_CAPTION = 10;
        public const short MA_AUTOGRID = 11;
        public const short MA_PAGENUMBERS = 12;
        public const short MA_FONT = 13;
        public const short MA_FONTSIZE = 14;

        // Image Size types for MA_IMAGESIZE 
        public const short MIS_PRINTER = 0;
        public const short MIS_PRINTERQTRSIZE = 1;
        public const short MIS_USER = 2;

        // Caption types for MA_CAPTION 
        public const short MAC_NONE = 0;
        public const short MAC_IMAGENAME = 1;
        public const short MAC_FILENAME = 2;
        public const short MAC_DATETIME = 3;
        public const short MAC_DESCRIPTION = 4;
        public const short MAC_FRAMENUMBER = 5;
        [DllImport("IPMOS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Functions 
        //----------------------------------------------------------------- 
        public static extern int IpMosaicShow(short Show);
        [DllImport("IPMOS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMosaicCreate(string lpszImages, short sNumImages);
        [DllImport("IPMOS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpMosaicGet(short sAttr, ref int lpParam, string lpszParam);
        [DllImport("IPMOS32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpMosaicSet(short sAttr, short sValue, string lpszParam);

        // ipcOrigin.h 
        // 
        // Definitions for Auto-Pro Functions of IpOrigin.DLL 
        // 
        // Copyright (c) 1999 - 2005, Media Cybernetics, Inc. 
        // 




        // Origin Commands for the IpOriginSet and IpOriginGet functions 
        // checks whether Origin is installd on the computer (returns TRUE if installed, FALSE if not) 
        public const short ORIGIN_IS_INSTALLED = 1;
        // checks whether Origin is running 
        public const short ORIGIN_IS_RUNNING = 2;
        // returns number of opened worksheets in Origin 
        public const short ORIGIN_NUM_WRKS = 3;
        // set row 
        public const short ORIGIN_ROW = 4;
        // set col 
        public const short ORIGIN_COL = 5;
        // worksheet name 
        public const short ORIGIN_WRKS = 6;
        // send clipboard to Origin 
        public const short ORIGIN_CLIPBOARD = 7;
        // get worspace name by index 
        public const short ORIGIN_WRKS_NAME = 8;

        // origin enumerations 
        // IpOriginSet 
        public enum ORIGINSET_ATTR
        {
            ORGNSET_ROW = Ipc32v5.ORIGIN_ROW,
            // set row 
            ORGNSET_COL = Ipc32v5.ORIGIN_COL,
            // set col 
            ORGNSET_CLIPBOARD = Ipc32v5.ORIGIN_CLIPBOARD
            // send clipboard to Origin 
        }

        // IpOriginSetStr 
        public enum ORIGINSETSTR_ATTR
        {
            ORGNSETSTR_WRKS = Ipc32v5.ORIGIN_WRKS,
            // worksheet name 
            ORGNSETSTR_CLIPBOARD = Ipc32v5.ORIGIN_CLIPBOARD,
            // send clipboard to Origin 
            ORGNSETSTR_WRKS_NAME = Ipc32v5.ORIGIN_WRKS_NAME
            // get worspace name by index 
        }

        // IpOriginGet 
        public enum ORIGINGET_ATTR
        {
            ORGNGET_IS_INSTALLED = Ipc32v5.ORIGIN_IS_INSTALLED,
            // checks whether Origin is installd on the computer (returns TRUE if installed, FALSE if not) 
            ORGNGET_IS_RUNNING = Ipc32v5.ORIGIN_IS_RUNNING,
            // checks whether Origin is running 
            ORGNGET_NUM_WRKS = Ipc32v5.ORIGIN_NUM_WRKS
            // returns number of opened worksheets in Origin 
        }

        // IpOriginSetStr 
        public enum ORIGINGETSTR_ATTR
        {
            ORGNGETSTR_WRKS_NAME = Ipc32v5.ORIGIN_WRKS_NAME
            // get worspace name by index 
        }
        [DllImport("IPORIGIN", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Origin Functions 
        //----------------------------------------------------------------- 
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 ORIGINSET_ATTR に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpOriginSet(ORIGINSET_ATTR sAttribute, short sParam, ref int lpData);
        [DllImport("IPORIGIN", EntryPoint = "IpOriginSet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 ORIGINSETSTR_ATTR に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpOriginSetStr(ORIGINSETSTR_ATTR sCmd, short sParam, string lpParam);
        [DllImport("IPORIGIN", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 ORIGINGET_ATTR に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpOriginGet(ORIGINGET_ATTR sAttribute, short sParam, ref int lpData);
        [DllImport("IPORIGIN", EntryPoint = "IpOriginGet", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 ORIGINGETSTR_ATTR に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpOriginGetStr(ORIGINGETSTR_ATTR sCmd, short sParam, string lpParam);
        [DllImport("IPPLUG32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]


        // 
        // Definitions for third-party plug-in Auto-Pro Functions 
        // 
        // Copyright (c) 1998 - 1999, Media Cybernetics, Inc. 
        // 




        // Commands 

        //----------------------------------------------------------------- 
        // Psho Functions 
        //----------------------------------------------------------------- 
        public static extern int IpPlShow(short nPlugType, short bShow);
        [DllImport("IPPLUG32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPlImport(string szImportName, short bShow);
        [DllImport("IPPLUG32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPlFilter(string szCategory, string szFilterName, short bShow);

        // 
        // Definitions for Port I/O Auto-Pro Functions 
        // 
        // Copyright (c) 1999 - 2005, Media Cybernetics, Inc. 
        // 




        // Commands for the IpPortIOSet and IpPortIOGet functions 
        // read-only - get the number of parallel ports 
        public const short PORTIO_NUM_BOARDS = 1;
        // read-only - get whether the specified serial port is present on the system 
        public const short PORTIO_SERIAL_IS_PRESENT = 2;

        // read-only (set by configuration) - get the number of 8-bit digital inputs 
        public const short PORTIO_NUM_D_INPUTS = 10;
        // read-only (set by configuration) - get the number of 8-bit digital outputs 
        public const short PORTIO_NUM_D_OUTPUTS = 11;
        // read-only (set by configuration) - get the number of digital input pins 
        public const short PORTIO_NUM_D_INPUT_PINS = 12;
        // read-only (set by configuration) - get the number of digital output pins 
        public const short PORTIO_NUM_D_OUTPUT_PINS = 13;

        // read-only (set by configuration) - get the board containing the specified analog port 
        public const short PORTIO_D_INPUT_BRD = 21;
        // read-only (set by configuration) - get the board containing the specified analog port 
        public const short PORTIO_D_OUTPUT_BRD = 22;
        // read-only (set by configuration) - get the index of the specified digital input pin within the digital port 
        public const short PORTIO_D_INPUT_PIN_INDEX = 23;
        // read-only (set by configuration) - get the board containing the specified analog port 
        public const short PORTIO_D_INPUT_PIN_BRD = 24;
        // read-only (set by configuration) - get the index of the specified digital input pin within the digital port 
        public const short PORTIO_D_OUTPUT_PIN_INDEX = 25;
        // read-only (set by configuration) - get the board containing the specified analog port 
        public const short PORTIO_D_OUTPUT_PIN_BRD = 26;

        // read-only - get the 8-bit digital input value 
        public const short PORTIO_D_INPUT_VALUE = 30;
        // read-only - get the digital input pin value 
        public const short PORTIO_D_INPUT_PIN_VALUE = 31;

        // Gets or sets whether the specified board is disabled (non-zero if disabled, zero if enabled for use) 
        public const short PORTIO_BOARD_DISABLED = 50;
        // Gets or sets the port configuration 
        public const short PORTIO_DIGITAL_CONFIGURATION = 51;
        // get/set whether updating outputs immediately or only after setting PORTIO_BLOCK_UPDATE back to FALSE 
        public const short PORTIO_BLOCK_UPDATE = 52;
        // get/set whether to open the last saved configuration 
        public const short PORTIO_OPEN_LAST_CONFIG = 53;
        // get/set the current serial port's baud rate (use PORTIO_BAUDRATES enumeration values NOT the baud rate) 
        public const short PORTIO_SERIAL_BAUD = 54;
        // get/set the current serial port's parity 
        public const short PORTIO_SERIAL_PARITY = 55;
        // get/set the current serial port's flow control 
        public const short PORTIO_SERIAL_FLOW = 56;
        // get/set the current serial port's stop bits 
        public const short PORTIO_SERIAL_STOPBITS = 57;
        // get/set the current serial port's data byte size (5 - 8) 
        public const short PORTIO_SERIAL_DATASIZE = 58;

        // get/set the 8-bit digital output value 
        public const short PORTIO_D_OUTPUT_VALUE = 60;
        // get/set the digital output pin value (Note: any non-zero value sets an output pin active/high) 
        public const short PORTIO_D_OUTPUT_PIN_VALUE = 61;

        // This command will configure the ports for use based on their current configuration. 
        public const short PORTIO_CONFIGURE = 70;

        // The only inquiries that are valid prior to using the PORTIO_CONFIGURE command are: 
        // PORTIO_NUM_BOARDS and PORTIO_DIGITAL_CONFIGURATION. 
        // The ports are also configured by calling IpPortIOOpenConfig (if a valid configuration can be opened) and IpPortIOSaveConfig. 

        // argument for PORTIO_NUM_D_INPUTS and PORTIO_NUM_D_OUTPUTS 
        public const short PORTIO_ALL = -1;

        // types of digital port configuration 
        public const short PORTIO_D_8BIT_INPUT = 1;
        public const short PORTIO_D_8BIT_OUTPUT = 2;
        public const short PORTIO_D_8_INPUT_PINS = 3;
        public const short PORTIO_D_8_OUTPUT_PINS = 4;

        //----------------------------------------------------------------- 
        // Port I/O Functions 
        //----------------------------------------------------------------- 

        // Port I/O enumerations 

        // for IpPortIOGet function 
        public enum PORTIOGET_ATTRIBUTES
        {
            PORTIOGET_NUM_BOARDS = Ipc32v5.PORTIO_NUM_BOARDS,
            PORTIOGET_NUM_D_INPUTS = Ipc32v5.PORTIO_NUM_D_INPUTS,
            PORTIOGET_NUM_D_OUTPUTS = Ipc32v5.PORTIO_NUM_D_OUTPUTS,
            PORTIOGET_NUM_D_INPUT_PINS = Ipc32v5.PORTIO_NUM_D_INPUT_PINS,
            PORTIOGET_NUM_D_OUTPUT_PINS = Ipc32v5.PORTIO_NUM_D_OUTPUT_PINS,
            PORTIOGET_D_INPUT_BRD = Ipc32v5.PORTIO_D_INPUT_BRD,
            PORTIOGET_D_OUTPUT_BRD = Ipc32v5.PORTIO_D_OUTPUT_BRD,
            PORTIOGET_D_INPUT_PIN_INDEX = Ipc32v5.PORTIO_D_INPUT_PIN_INDEX,
            PORTIOGET_D_INPUT_PIN_BRD = Ipc32v5.PORTIO_D_INPUT_PIN_BRD,
            PORTIOGET_D_OUTPUT_PIN_INDEX = Ipc32v5.PORTIO_D_OUTPUT_PIN_INDEX,
            PORTIOGET_D_OUTPUT_PIN_BRD = Ipc32v5.PORTIO_D_OUTPUT_PIN_BRD,
            PORTIOGET_D_INPUT_VALUE = Ipc32v5.PORTIO_D_INPUT_VALUE,
            PORTIOGET_D_INPUT_PIN_VALUE = Ipc32v5.PORTIO_D_INPUT_PIN_VALUE,
            PORTIOGET_BOARD_DISABLED = Ipc32v5.PORTIO_BOARD_DISABLED,
            PORTIOGET_DIGITAL_CONFIGURATION = Ipc32v5.PORTIO_DIGITAL_CONFIGURATION,
            PORTIOGET_BLOCK_UPDATE = Ipc32v5.PORTIO_BLOCK_UPDATE,
            PORTIOGET_OPEN_LAST_CONFIG = Ipc32v5.PORTIO_OPEN_LAST_CONFIG,
            PORTIOGET_SERIAL_BAUD = Ipc32v5.PORTIO_SERIAL_BAUD,
            PORTIOGET_SERIAL_PARITY = Ipc32v5.PORTIO_SERIAL_PARITY,
            PORTIOGET_SERIAL_FLOW = Ipc32v5.PORTIO_SERIAL_FLOW,
            PORTIOGET_SERIAL_STOPBITS = Ipc32v5.PORTIO_SERIAL_STOPBITS,
            PORTIOGET_SERIAL_DATASIZE = Ipc32v5.PORTIO_SERIAL_DATASIZE,
            PORTIOGET_D_OUTPUT_VALUE = Ipc32v5.PORTIO_D_OUTPUT_VALUE,
            PORTIOGET_D_OUTPUT_PIN_VALUE = Ipc32v5.PORTIO_D_OUTPUT_PIN_VALUE
        }

        // for IpPortIOSet function 
        public enum PORTIOSET_ATTRIBUTES
        {
            PORTIOSET_BOARD_DISABLED = Ipc32v5.PORTIO_BOARD_DISABLED,
            PORTIOSET_DIGITAL_CONFIGURATION = Ipc32v5.PORTIO_DIGITAL_CONFIGURATION,
            PORTIOSET_BLOCK_UPDATE = Ipc32v5.PORTIO_BLOCK_UPDATE,
            PORTIOSET_OPEN_LAST_CONFIG = Ipc32v5.PORTIO_OPEN_LAST_CONFIG,
            PORTIOSET_SERIAL_BAUD = Ipc32v5.PORTIO_SERIAL_BAUD,
            PORTIOSET_SERIAL_PARITY = Ipc32v5.PORTIO_SERIAL_PARITY,
            PORTIOSET_SERIAL_FLOW = Ipc32v5.PORTIO_SERIAL_FLOW,
            PORTIOSET_SERIAL_STOPBITS = Ipc32v5.PORTIO_SERIAL_STOPBITS,
            PORTIOSET_SERIAL_DATASIZE = Ipc32v5.PORTIO_SERIAL_DATASIZE,
            PORTIOSET_D_OUTPUT_VALUE = Ipc32v5.PORTIO_D_OUTPUT_VALUE,
            PORTIOSET_D_OUTPUT_PIN_VALUE = Ipc32v5.PORTIO_D_OUTPUT_PIN_VALUE,
            PORTIOSET_CONFIGURE = Ipc32v5.PORTIO_CONFIGURE
        }

        public enum PORTIO_BAUDRATES
        {
            PORTIO_BAUD_300 = 0,
            PORTIO_BAUD_1200 = 1,
            PORTIO_BAUD_2400 = 2,
            PORTIO_BAUD_9600 = 3,
            PORTIO_BAUD_14400 = 4,
            PORTIO_BAUD_19200 = 5,
            PORTIO_BAUD_38400 = 6,
            PORTIO_BAUD_56000 = 7,
            PORTIO_BAUD_57600 = 8,
            PORTIO_BAUD_115200 = 9,
            PORTIO_BAUD_128000 = 10,
            PORTIO_BAUD_256000 = 11
        }

        public enum PORTIO_PARITYTYPES
        {
            PORTIO_PARITY_NONE = 0,
            PORTIO_PARITY_EVEN = 1,
            PORTIO_PARITY_ODD = 2
        }

        public enum PORTIO_STOPBITS
        {
            PORTIO_STOP_ONE = 0,
            PORTIO_STOP_ONE_PT_FIVE = 1,
            PORTIO_STOP_TWO = 2
        }

        public enum PORTIO_FLOWTYPES
        {
            PORTIO_FLOW_NONE = 0,
            PORTIO_FLOW_XONXOFF = 1,
            PORTIO_FLOW_HARDWARE = 2
        }

        public enum PORTIO_DATASIZE
        {
            PORTIO_DATASIZE_FIVE = 5,
            PORTIO_DATASIZE_SIX = 6,
            PORTIO_DATASIZE_SEVEN = 7,
            PORTIO_DATASIZE_EIGHT = 8
        }

        public const short PORTIO_MAX_SERIAL = 256;

        public enum PORTIO_COMMANDS
        {
            PORTIO_INIT = 0,
            PORTIO_CLOSE = 1,
            PORTIO_UPDATE = 2,
            PORTIO_CLEAR = 3
        }
        [DllImport("IpPortIO32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]


        public static extern int IpPortIOShowConfig();
        [DllImport("IpPortIO32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPortIOOpenConfig(string strConfigFile);
        [DllImport("IpPortIO32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPortIOSaveConfig(string strConfigFile);
        [DllImport("IpPortIO32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 PORTIOSET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpPortIOSetInt(PORTIOSET_ATTRIBUTES sAttribute, short sParam, short sValue);
        [DllImport("IpPortIO32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 PORTIOGET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpPortIOGetInt(PORTIOGET_ATTRIBUTES sAttribute, short sParam, ref short sValue);
        [DllImport("IpPortIO32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        // for serial ports only 
        //UPGRADE_WARNING: 構造体 PORTIO_COMMANDS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpPortIOControl(short sPort, PORTIO_COMMANDS sCmd);
        [DllImport("IpPortIO32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_NOTE: Command は Command_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        public static extern int IpPortIOWrite(short sPort, string Command_Renamed, short bTerminated, short Count);
        [DllImport("IpPortIO32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpPortIORead(short sPort, string Response, short bTerminated, short Count, int TimeOut);


        // 
        // Display Range Auto-Pro functions and structures definitions. 
        // 
        // Copyright (c) 1999-2003 Media Cybernetics, Inc. 
        // 


        public const short DR_RANGE = 1;
        public const short DR_GAMMA = 2;
        public const short DR_INV = 3;
        public const short DR_BEST = 4;
        public const short DR_FRANGE = 5;
        public const short DR_RANGE_RESET = 6;
        // an option defining the set of frames to analyze 
        public const short DR_FRAMES_OPTION = 7;
        // the sub-sampling rate if DR_FRAMES_OPTION is set to DRFRAMES_SUBSAMPLE 
        public const short DR_FRAMES_SUBSAMPLE = 8;

        // possible values for the DR_FRAMES_OPTION 
        // the behavior of previous Image-Pro versions 
        public const short DRFRAMES_ALL = 0;
        // analyze only the frames that have already been loaded into memory 
        public const short DRFRAMES_LOADED = 1;
        // sub-sample every Nth frame, where N is set by the DR_FRAMES_SUBSAMPLE command, for instance 5 to sample every 5th frame 
        public const short DRFRAMES_SUBSAMPLE = 2;
        // analyze only the first, last and middle frames 
        public const short DRFRAMES_FIRST_LAST_MIDDLE = 3;
        [DllImport("IPRNGE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpDrShow(short bShow);
        [DllImport("IPRNGE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpDrGet(short Id, short sParam, ref int lpParam);
        [DllImport("IPRNGE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        public static extern int IpDrSet(short Id, short sParam, ref int lpParam);
        // 
        // Report Generator Auto-Pro functions and structures definitions. 
        // 
        // Copyright (c) 1999-2003 Media Cybernetics, Inc. 
        // 


        public const short RPT_DUMMY = 0;
        [DllImport("IPRPT32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpRptShow();
        [DllImport("IPRPT32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpRptNew(string templatefile);
        [DllImport("IPRPT32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpRptOpen(string reportfile);
        [DllImport("IPRPT32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpRptClose();
        [DllImport("IPRPT32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpRptSave(string reportfile);
        [DllImport("IPRPT32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpRptPrint();

        // 
        // Auto-Pro functions and structures definitions for sequencer 
        // 



        // Sequence Play commands for use with IpSeqPlay 
        public const short SEQ_STOP = -1;
        public const short SEQ_FOR = -2;
        public const short SEQ_REV = -3;
        public const short SEQ_FFOR = -4;
        public const short SEQ_FREV = -5;
        public const short SEQ_FFRA = -6;
        public const short SEQ_LFRA = -7;
        public const short SEQ_PREV = -8;
        public const short SEQ_NEXT = -9;

        // Sequence attributes for use with IpSeqGet and IpSeqSet 
        public const short SEQ_NUMFRAMES = 1;
        public const short SEQ_ACTIVEFRAME = 2;
        // determines sequence play rate 
        public const short SEQ_FRAMETIME = 3;
        public const short SEQ_SKIP = 4;
        public const short SEQ_START = 5;
        public const short SEQ_END = 6;
        public const short SEQ_PLAYTYPE = 7;
        public const short SEQ_PLAYUPDATE = 8;
        public const short SEQ_APPLY = 9;
        // synchronize play of all active sequences 
        public const short SEQ_SYNCALL = 10;
        // read-only - returns the current play command 
        public const short SEQ_PLAYING = 11;
        // whether to dynamically adjust play rate 
        public const short SEQ_ADJUST_RATE = 12;
        public const short SEQ_EDIT_PROMPT = 13;
        // whether to prompt for running average settings 
        public const short SEQ_AVG_PROMPT = 14;
        // running average: number of frames to average 
        public const short SEQ_AVG_FRAMES = 15;
        // running average: drop incomplete frames? 
        public const short SEQ_AVG_DROP_INCOMPLETE = 16;
        // whether to prompt for sequence different settings 
        public const short SEQ_DIFF_PROMPT = 17;
        // difference: method of handling incomplete frames 
        public const short SEQ_DIFF_TYPE = 18;
        // statistics from 
        public const short SEQ_FRAMES_DISPLAYED = 19;
        // last sequence play 
        public const short SEQ_FRAMES_DROPPED = 20;
        // current frame time (e.g. after adjustment) - does not mark image as modified 
        public const short SEQ_CURRENT_FRAMETIME = 21;

        // Sequence play types 
        public const short SEQ_PLAYWRAP = 1;
        public const short SEQ_PLAYTOEND = 2;
        public const short SEQ_PLAYAUTOREV = 3;

        // Sequence difference types for IpSeqDifferenceEx 
        // last frame is difference between last frame and first frame (previous behavior) 
        public const short SEQDIFF_WRAP = 1;
        // sequence is one frame shorter than original, returning only difference frames 
        public const short SEQDIFF_DIFFONLY = 2;
        // first frame of result is zero difference frame 
        public const short SEQDIFF_PADFIRST = 3;
        // last frame of result is zero difference frame 
        public const short SEQDIFF_PADLAST = 4;

        // Reslice types for IpSeqReslice 
        public const short SEQSLICE_XZ = 1;
        public const short SEQSLICE_YZ = 2;
        // dfResample is ignored 
        public const short SEQSLICE_REVERSEZ = 3;

        //----------------------------------------------------------------- 
        // Motion Analysis functions 
        //----------------------------------------------------------------- 


        // for use with IpSeqPlay (also can use a frame index) 
        public enum SEQPLAY_COMMANDS
        {
            SEQPLAY_STOP = Ipc32v5.SEQ_STOP,
            SEQPLAY_FORWARD = Ipc32v5.SEQ_FOR,
            SEQPLAY_REVERVSE = Ipc32v5.SEQ_REV,
            SEQPLAY_FASTFORWARD = Ipc32v5.SEQ_FFOR,
            SEQPLAY_FASTREVERSE = Ipc32v5.SEQ_FREV,
            SEQPLAY_FIRSTFRAME = Ipc32v5.SEQ_FFRA,
            SEQPLAY_LASTFRAME = Ipc32v5.SEQ_LFRA,
            SEQPLAY_PREVIOUSFRAME = Ipc32v5.SEQ_PREV,
            SEQPLAY_NEXTFRAME = Ipc32v5.SEQ_NEXT
        }

        // for IpSeqGet and IpSeqSet 
        public enum SEQGET_ATTRIBUTES
        {
            SEQGET_NUMFRAMES = Ipc32v5.SEQ_NUMFRAMES,
            SEQGET_ACTIVEFRAME = Ipc32v5.SEQ_ACTIVEFRAME,
            SEQGET_FRAMETIME = Ipc32v5.SEQ_FRAMETIME,
            SEQGET_SKIP = Ipc32v5.SEQ_SKIP,
            SEQGET_START = Ipc32v5.SEQ_START,
            SEQGET_END = Ipc32v5.SEQ_END,
            SEQGET_PLAYTYPE = Ipc32v5.SEQ_PLAYTYPE,
            SEQGET_PLAYUPDATE = Ipc32v5.SEQ_PLAYUPDATE,
            SEQGET_APPLY = Ipc32v5.SEQ_APPLY,
            SEQGET_SYNCALL = Ipc32v5.SEQ_SYNCALL,
            SEQGET_PLAYING = Ipc32v5.SEQ_PLAYING,
            SEQGET_ADJUST_RATE = Ipc32v5.SEQ_ADJUST_RATE,
            SEQGET_EDIT_PROMPT = Ipc32v5.SEQ_EDIT_PROMPT,
            SEQGET_AVG_PROMPT = Ipc32v5.SEQ_AVG_PROMPT,
            SEQGET_AVG_FRAMES = Ipc32v5.SEQ_AVG_FRAMES,
            SEQGET_AVG_DROP_INCOMPLETE = Ipc32v5.SEQ_AVG_DROP_INCOMPLETE,
            SEQGET_DIFF_PROMPT = Ipc32v5.SEQ_DIFF_PROMPT,
            SEQGET_DIFF_TYPE = Ipc32v5.SEQ_DIFF_TYPE,
            SEQGET_FRAMES_DISPLAYED = Ipc32v5.SEQ_FRAMES_DISPLAYED,
            SEQGET_FRAMES_DROPPED = Ipc32v5.SEQ_FRAMES_DROPPED,
            SEQGET_CURRENT_FRAMETIME = Ipc32v5.SEQ_CURRENT_FRAMETIME
        }

        // for IpSeqGet and IpSeqSet 
        public enum SEQSET_ATTRIBUTES
        {
            SEQSET_ACTIVEFRAME = Ipc32v5.SEQ_ACTIVEFRAME,
            SEQSET_FRAMETIME = Ipc32v5.SEQ_FRAMETIME,
            SEQSET_SKIP = Ipc32v5.SEQ_SKIP,
            SEQSET_START = Ipc32v5.SEQ_START,
            SEQSET_END = Ipc32v5.SEQ_END,
            SEQSET_PLAYTYPE = Ipc32v5.SEQ_PLAYTYPE,
            SEQSET_PLAYUPDATE = Ipc32v5.SEQ_PLAYUPDATE,
            SEQSET_APPLY = Ipc32v5.SEQ_APPLY,
            SEQSET_SYNCALL = Ipc32v5.SEQ_SYNCALL,
            SEQSET_ADJUST_RATE = Ipc32v5.SEQ_ADJUST_RATE,
            SEQSET_EDIT_PROMPT = Ipc32v5.SEQ_EDIT_PROMPT,
            SEQSET_AVG_PROMPT = Ipc32v5.SEQ_AVG_PROMPT,
            SEQSET_AVG_FRAMES = Ipc32v5.SEQ_AVG_FRAMES,
            SEQSET_AVG_DROP_INCOMPLETE = Ipc32v5.SEQ_AVG_DROP_INCOMPLETE,
            SEQSET_DIFF_PROMPT = Ipc32v5.SEQ_DIFF_PROMPT,
            SEQSET_DIFF_TYPE = Ipc32v5.SEQ_DIFF_TYPE,
            SEQSET_CURRENT_FRAMETIME = Ipc32v5.SEQ_CURRENT_FRAMETIME
        }
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpSeqShow(short sShow);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 SEQGET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpSeqGet(SEQGET_ATTRIBUTES sAttribute, ref int lpValue);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 SEQSET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpSeqSet(SEQSET_ATTRIBUTES sAttribute, int lNewAttr);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpSeqMerge(string FileName, string Library, int StartNumber, int NumFrames);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqOpen(string FileName, string Library, int StartNumber, int NumFrames);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 SEQPLAY_COMMANDS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpSeqPlay(SEQPLAY_COMMANDS PlayCmd);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqSave(string FileName, string Library, int StartNumber, int NumFrames);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpSeqDifference(int lStart, int lNumber);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqAverage(int lStart, int lNumber);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqRunningAvg(int lStart, int lNumber, int lAvgWindow, short bDropPartial);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqExtractFrames(int lStart, int lNumber);
        [DllImport("IPMOTN32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpSeqDifferenceEx(int lStart, int lNumber, short sDiffType);
        [DllImport("IPSM32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpSeqOpenEx(string FileName, string Library, int StartNumber, int NumFrames, int Interval);
        [DllImport("IPSM32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpSeqOptions();
        [DllImport("IPSM32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpSeqReslice(int lStart, int lNumber, short sSliceType, double dfResample);
        [DllImport("IPSM32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpSeqMergeDoc(int docId, int DocFrame, int DestFrame);
        // 
        // Definitions for Sequence Gallery Auto-Pro Functions 
        // 
        // Copyright (c) 2001 - , Media Cybernetics, Inc. 
        // 




        //----------------------------------------------------------------- 
        // Sequence Gallery Constants & commands 
        //----------------------------------------------------------------- 
        public const short SEQG_TRACKENABLE = 0;
        public const short SEQG_ISTRACKED = 1;
        public const short SEQG_ISGALLERY = 2;
        public const short SEQG_DISPLAY_TINT = 3;
        [DllImport("IPTHMB32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //----------------------------------------------------------------- 
        // Sequence Gallery Functions 
        //----------------------------------------------------------------- 
        public static extern int IpSeqGShow(short bShow);
        [DllImport("IPTHMB32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqGCreate();
        [DllImport("IPTHMB32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqGUpdate(short docId);
        [DllImport("IPTHMB32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqGSet(short sAttribute, short sParam);
        [DllImport("IPTHMB32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSeqGGet(short sAttribute, short sParam, ref short lpsData);
        [DllImport("IPSNAP32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]


        // 
        // IpSnap function 
        // 
        // Copyright (c) 1999-2003, Media Cybernetics, Inc. 
        // 




        //----------------------------------------------------------------- 
        // Snap Functions 
        //----------------------------------------------------------------- 
        public static extern int IpSnap();


        // 
        // Definitions for Surface Plot Auto-Pro Functions 
        // 
        // Copyright (c) 1998 - 2001, Media Cybernetics, Inc. 
        // 




        //----------------------------------------------------------------- 
        // Functions 
        //----------------------------------------------------------------- 
        // not a real attr. Sets all attr to default 
        public const short SP_DEFAULT = 100;
        // View point elevation (degree) 
        public const short SP_VIEW_ELEVATION = 101;
        // View point rotation (degree) 
        public const short SP_VIEW_ROTATION = 102;
        // Light source elevation (degree) 
        public const short SP_LIGHT_ELEVATION = 103;
        // Light source rotation (degree) 
        public const short SP_LIGHT_ROTATION = 104;
        // Light source color (COLORREF) 
        public const short SP_LIGHT_COLOR = 105;
        // Draw style see SPS_xxx 
        public const short SP_STYLE_TYPE = 106;
        // Wire frame span (pixels between lines) 
        public const short SP_STYLE_WIREFRAME_SPAN = 107;
        // Draw edge (skirt/curtain) 
        public const short SP_STYLE_DRAWEDGES = 108;
        // Draw axis 
        public const short SP_STYLE_DRAWAXES = 109;
        // Height (Z) amplification (0..400) 
        public const short SP_STYLE_ZSCALE = 110;
        // Ambient light reflectance (0..100) 
        public const short SP_AMBIENT_REFLECTANCE = 111;
        // Diffuse light reflectance (0..100) 
        public const short SP_DIFFUSE_REFLECTANCE = 112;
        // Specular light reflectance (0..100) 
        public const short SP_SPECULAR_REFLECTANCE = 113;
        // Material glossiness (1..100) 
        public const short SP_GLOSS = 114;
        // Start colorizing surface at gray value (0..255) 
        public const short SP_COLORIZED_FROM = 115;
        // Stop colorizing surface at gray value (0..255) 
        public const short SP_COLORIZED_TO = 116;
        // Color of From (COLORREF) 
        public const short SP_COLORIZED_FROM_COLOR = 117;
        // Color of To (COLORREF) 
        public const short SP_COLORIZED_TO_COLOR = 118;
        // Number of spin in HSV color space from From color to To color 
        public const short SP_SURFACE_COLOR_SPIN = 119;
        // Spread From and To colors on the uncolorized gray values. 
        public const short SP_SURFACE_COLOR_SPREAD = 120;

        // Texture Map? 
        public const short SP_STYLE_TEXTURED = 121;
        // Texture Image ID 
        public const short SP_TEXTURE_ID = 122;
        // Shadow depth 
        public const short SP_SHADOW_DEPTH = 123;

        // Output to new image ws 
        public const short SPO_NEW = 1;
        // Output to new image ws and intensity scale bar ws. 
        public const short SPO_NEW_WITH_ISCALE = 2;
        // Output to printer 
        public const short SPO_PRINTER = 3;
        // Output to clipboard 
        public const short SPO_CLIPBOARD = 4;

        // Draw as wireframe 
        public const short SPS_WIREFRAME = 0;
        // Draw as solid surface with no lighting 
        public const short SPS_UNSHADED = 1;
        // Draw as solid surface with lighting (shading) 
        public const short SPS_SHADED = 2;

        //surface plot enumerations 

        public enum SPSET_ATTR
        {
            SPSET_DEFAULT = Ipc32v5.SP_DEFAULT,
            // not a real attr. Sets all attr to default 
            SPSET_VIEW_ELEVATION = Ipc32v5.SP_VIEW_ELEVATION,
            // View point elevation (degree) 
            SPSET_VIEW_ROTATION = Ipc32v5.SP_VIEW_ROTATION,
            // View point rotation (degree) 
            SPSET_LIGHT_ELEVATION = Ipc32v5.SP_LIGHT_ELEVATION,
            // Light source elevation (degree) 
            SPSET_LIGHT_ROTATION = Ipc32v5.SP_LIGHT_ROTATION,
            // Light source rotation (degree) 
            SPSET_LIGHT_COLOR = Ipc32v5.SP_LIGHT_COLOR,
            // Light source color (COLORREF) 
            SPSET_STYLE_TYPE = Ipc32v5.SP_STYLE_TYPE,
            // Draw style see SPS_xxx 
            SPSET_STYLE_WIREFRAME_SPAN = Ipc32v5.SP_STYLE_WIREFRAME_SPAN,
            // Wire frame span (pixels between lines) 
            SPSET_STYLE_DRAWEDGES = Ipc32v5.SP_STYLE_DRAWEDGES,
            // Draw edge (skirt/curtain) 
            SPSET_STYLE_DRAWAXES = Ipc32v5.SP_STYLE_DRAWAXES,
            // Draw axis 
            SPSET_STYLE_ZSCALE = Ipc32v5.SP_STYLE_ZSCALE,
            // Height (Z) amplification (0..400) 
            SPSET_AMBIENT_REFLECTANCE = Ipc32v5.SP_AMBIENT_REFLECTANCE,
            // Ambient light reflectance (0..100) 
            SPSET_DIFFUSE_REFLECTANCE = Ipc32v5.SP_DIFFUSE_REFLECTANCE,
            // Diffuse light reflectance (0..100) 
            SPSET_SPECULAR_REFLECTANCE = Ipc32v5.SP_SPECULAR_REFLECTANCE,
            // Specular light reflectance (0..100) 
            SPSET_GLOSS = Ipc32v5.SP_GLOSS,
            // Material glossiness (1..100) 
            SPSET_COLORIZED_FROM = Ipc32v5.SP_COLORIZED_FROM,
            // Start colorizing surface at gray value (0..255) 
            SPSET_COLORIZED_TO = Ipc32v5.SP_COLORIZED_TO,
            // Stop colorizing surface at gray value (0..255) 
            SPSET_COLORIZED_FROM_COLOR = Ipc32v5.SP_COLORIZED_FROM_COLOR,
            // Color of From (COLORREF) 
            SPSET_COLORIZED_TO_COLOR = Ipc32v5.SP_COLORIZED_TO_COLOR,
            // Color of To (COLORREF) 
            SPSET_SURFACE_COLOR_SPIN = Ipc32v5.SP_SURFACE_COLOR_SPIN,
            // Number of spin in HSV color space from From color to To color 
            SPSET_SURFACE_COLOR_SPREAD = Ipc32v5.SP_SURFACE_COLOR_SPREAD,
            // Spread From and To colors on the uncolorized gray values. 
            SPSET_STYLE_TEXTURED = Ipc32v5.SP_STYLE_TEXTURED,
            // Texture Map? 
            SPSET_TEXTURE_ID = Ipc32v5.SP_TEXTURE_ID,
            // Texture Image ID 
            SPSET_SHADOW_DEPTH = Ipc32v5.SP_SHADOW_DEPTH
            // Shadow depth 
        }
        public enum SPGET_ATTR
        {
            SPGET_VIEW_ELEVATION = Ipc32v5.SP_VIEW_ELEVATION,
            // View point elevation (degree) 
            SPGET_VIEW_ROTATION = Ipc32v5.SP_VIEW_ROTATION,
            // View point rotation (degree) 
            SPGET_LIGHT_ELEVATION = Ipc32v5.SP_LIGHT_ELEVATION,
            // Light source elevation (degree) 
            SPGET_LIGHT_ROTATION = Ipc32v5.SP_LIGHT_ROTATION,
            // Light source rotation (degree) 
            SPGET_LIGHT_COLOR = Ipc32v5.SP_LIGHT_COLOR,
            // Light source color (COLORREF) 
            SPGET_STYLE_TYPE = Ipc32v5.SP_STYLE_TYPE,
            // Draw style see SPS_xxx 
            SPGET_STYLE_WIREFRAME_SPAN = Ipc32v5.SP_STYLE_WIREFRAME_SPAN,
            // Wire frame span (pixels between lines) 
            SPGET_STYLE_DRAWEDGES = Ipc32v5.SP_STYLE_DRAWEDGES,
            // Draw edge (skirt/curtain) 
            SPGET_STYLE_DRAWAXES = Ipc32v5.SP_STYLE_DRAWAXES,
            // Draw axis 
            SPGET_STYLE_ZSCALE = Ipc32v5.SP_STYLE_ZSCALE,
            // Height (Z) amplification (0..400) 
            SPGET_AMBIENT_REFLECTANCE = Ipc32v5.SP_AMBIENT_REFLECTANCE,
            // Ambient light reflectance (0..100) 
            SPGET_DIFFUSE_REFLECTANCE = Ipc32v5.SP_DIFFUSE_REFLECTANCE,
            // Diffuse light reflectance (0..100) 
            SPGET_SPECULAR_REFLECTANCE = Ipc32v5.SP_SPECULAR_REFLECTANCE,
            // Specular light reflectance (0..100) 
            SPGET_GLOSS = Ipc32v5.SP_GLOSS,
            // Material glossiness (1..100) 
            SPGET_COLORIZED_FROM = Ipc32v5.SP_COLORIZED_FROM,
            // Start colorizing surface at gray value (0..255) 
            SPGET_COLORIZED_TO = Ipc32v5.SP_COLORIZED_TO,
            // Stop colorizing surface at gray value (0..255) 
            SPGET_COLORIZED_FROM_COLOR = Ipc32v5.SP_COLORIZED_FROM_COLOR,
            // Color of From (COLORREF) 
            SPGET_COLORIZED_TO_COLOR = Ipc32v5.SP_COLORIZED_TO_COLOR,
            // Color of To (COLORREF) 
            SPGET_SURFACE_COLOR_SPIN = Ipc32v5.SP_SURFACE_COLOR_SPIN,
            // Number of spin in HSV color space from From color to To color 
            SPGET_SURFACE_COLOR_SPREAD = Ipc32v5.SP_SURFACE_COLOR_SPREAD,
            // Spread From and To colors on the uncolorized gray values. 
            SPGET_STYLE_TEXTURED = Ipc32v5.SP_STYLE_TEXTURED,
            // Texture Map? 
            SPGET_TEXTURE_ID = Ipc32v5.SP_TEXTURE_ID,
            // Texture Image ID 
            SPGET_SHADOW_DEPTH = Ipc32v5.SP_SHADOW_DEPTH
            // Shadow depth 
        }

        public enum SPO_TYPE
        {
            SPOTYPE_NEW = Ipc32v5.SPO_NEW,
            // Output to new image ws 
            SPOTYPE_NEW_WITH_ISCALE = Ipc32v5.SPO_NEW_WITH_ISCALE,
            // Output to new image ws and intensity scale bar ws. 
            SPOTYPE_PRINTER = Ipc32v5.SPO_PRINTER,
            // Output to printer 
            SPOTYPE_CLIPBOARD = Ipc32v5.SPO_CLIPBOARD
            // Output to clipboard 
        }
        [DllImport("IPSURF32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //surface plot functions 
        public static extern int IpSurfShow(short bShow);
        [DllImport("IPSURF32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 SPSET_ATTR に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpSurfSet(SPSET_ATTR sAttribute, int lData);
        [DllImport("IPSURF32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 SPGET_ATTR に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpSurfGet(SPGET_ATTR sAttribute, ref int lpData);
        [DllImport("IPSURF32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 SPO_TYPE に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpSurfOutput(SPO_TYPE OutputType);
        [DllImport("IPSURF32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpSurfAutoRefresh(short bAutoRefresh);
        // 
        // Definitions for Trace Auto-Pro Functions 
        // 
        // Copyright (c) 1998 - 2001, Media Cybernetics, Inc. 
        // 

        // Trace Module 
        // IpTraceAttr() 
        public const short TR_MODE = 1;
        public const short TR_PEN = 2;
        public const short TR_ERASER = 3;
        public const short TR_SHOW = 4;

        // IpTraceDo() 
        public const short TR_DELETE = 0;
        public const short TR_AUTO = 1;
        public const short TR_IMAGE = 2;
        [DllImport("IPTRCE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpTraceShow(short bShow);
        [DllImport("IPTRCE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTraceAttr(short sAttrib, int lValue);
        [DllImport("IPTRCE32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTraceDo(short sCmd);
        // 
        // Definitions for Object Tracking Functions 
        // 
        // Copyright (c) 1999-2003 - , Media Cybernetics, Inc. 
        // 



        // if this include file is included by task.h, only one of these set of macros should 
        // be defined. 
        // Commands 

        // show/hide 
        public const short TRACK_HIDE = 0;
        public const short TRACK_SHOW = 1;

        // tracking windows 
        public const short TRACK_TABLE = 0;
        public const short TRACK_GRAPH = 1;

        // constants for IpTrackSaveData 
        // sources of data 
        // measurements table 
        public const short TR_MM_DATA = 1;
        // 
        public const short TR_MM_STATS = 2;
        // 
        public const short TR_MM_ACTIVE = 3;
        // 
        public const short TR_GRAPH = 7;

        // types of data 
        // raw data in tab-delimited form 
        public const int TRF_TABLE = 0x100;
        // metafile graph format 
        public const int TRF_GRAPH = 0x200;

        // destinations 
        // File (can be used with APPEND, CSV, or HTML, does not support GRAPH) 
        public const short TRDF_FILE = 1;
        // Clipboard 
        public const short TRDF_CLIPBOARD = 2;
        // DDE to Excel 
        public const short TRDF_DDE = 4;
        // destination mask 
        public const int TRDF_MASK = 0xff;

        // comma-separated values 
        public const int TRDF_CSV = 0x200;
        // HTML table 
        public const int TRDF_HTML = 0x400;
        // last type 
        public const int TRDF_LAST = 0x800;
        // TRDF_CSV and TRDF_HTML cannot be combined. In absence of either, will be tab-delimited 


        // measurement labels 
        public const short trLabelsShowName = 0;
        public const short trLabelsShowMeasurement = 1;
        public const short trLabelsShowNone = 2;

        // time units 
        public const short trtuSecond = 0;
        public const short trtuMinute = 1;
        public const short trtuHour = 2;

        // graph X axis label type 
        public const short trxlFrameNumber = 0;
        public const short trxlRelTime = 1;
        public const short trxlAbsTime = 2;
        public const short trxlRelTimeEq = 3;

        // track measurements 
        public const short TRM_DIST = 0;
        public const short TRM_X_COORD = 1;
        public const short TRM_Y_COORD = 2;
        public const short TRM_OR_DIST = 3;
        public const short TRM_ANGLE = 4;
        public const short TRM_SPEED = 5;
        public const short TRM_ACCELERATION = 6;
        public const short TRM_ACC_DIST = 7;
        public const short TRM_REL_TIME = 8;
        public const short TRM_COUNT_SIZE = 9;

        // statistics 
        public const short TRSTMean = 0;
        public const short TRSTStDev = 1;
        public const short TRSTMin = 2;
        public const short TRSTMax = 3;
        public const short TRSTRange = 4;
        public const short TRSTSum = 5;
        public const short TRSTIndMin = 6;
        public const short TRSTIndMax = 7;
        public const short TRSTNObj = 8;
        public const short TRSTNShown = 9;

        public const short TRSTEnd = 10;

        public const short TR_VALUE = 100;

        public const short TR_ALL = -1;

        // tracking measurements options 
        // line color 
        public const short TM_TRACK_COLOR = 0;
        // selection color 
        public const short TM_SEL_COLOR = 1;
        // arrow size 
        public const short TM_EL_SIZE = 2;
        // text color 
        public const short TM_TEXT_COLOR = 3;
        // font size 
        public const short TM_FONT_SIZE = 4;
        // label type (name,first measurement,none) 
        public const short TM_LABEL_TYPE = 5;
        // sets track prefix 
        public const short TM_TRACK_PREF_SET = 6;
        // gets track prefix 
        public const short TM_TRACK_PREF_GET = 7;
        // resets track list 
        public const short TM_RESET_MEAS = 8;
        // adds measurement to the list 
        public const short TM_ADD_MEAS = 9;
        // shows statistics 
        public const short TM_SHOW_STATS = 11;
        // gets statistics 
        public const short TM_STATS_GET = 12;
        // gets number of tracks 
        public const short TM_NUM_TRACKS_GET = 13;
        // gets number of points in track 
        public const short TM_NUM_POINTS_GET = 14;
        // gets coordinates of points of tracks 
        public const short TM_POINTS_GET = 15;
        // gets number of active measurements 
        public const short TM_NUM_MEAS_GET = 16;
        // gets list of active measurements 
        public const short TM_MEAS_LIST_GET = 17;
        // gets measurement from manual or volume objects 
        public const short TM_MEAS_GET = 18;
        // gets selection flag from manual or volume objects 
        public const short TM_SEL_GET = 19;
        // sets selection to from manual or volume objects 
        public const short TM_SEL_SET = 20;
        // gets hide flag from manual or volume objects 
        public const short TM_SHOW_GET = 21;
        // sets hide flag to manual or volume objects 
        public const short TM_SHOW_SET = 22;
        // gets name of manual or volume objects 
        public const short TM_NAME_GET = 23;
        // sets name of manual or volume objects 
        public const short TM_NAME_SET = 24;
        // updates manual measurements data table and objects applying new settings 
        public const short TM_UPDATE = 25;
        // gets type of manual measurement element (IP_REND_MM_POINT,IP_REND_MM_LINE,IP_REND_MM_POLY_LINE,IP_REND_MM_ANGLE) 
        public const short TM_TYPE_GET = 26;
        // sets measurements action (mmActionSelect,mmActionAddPoint,mmActionAddLine,...) 
        public const short TM_ACTION = 27;
        // creates new measuremetn based on the selected objects 
        public const short TM_CREATE_MEAS = 28;
        // shows all objects 
        public const short TM_SHOW_ALL = 29;
        // shows/hides selected objects 
        public const short TM_SHOW_SELECTED = 30;
        // deletes all objects 
        public const short TM_DELETE_ALL = 31;
        // deletes selected objects 
        public const short TM_DELETE_SELECTED = 32;
        // coloring 
        public const short TM_COLORING = 33;
        // add new track 
        public const short TM_ADD_TRACK = 34;
        // set color 
        public const short TM_COLOR_GET = 35;
        // get color 
        public const short TM_COLOR_SET = 36;
        // number of digits after decimal 
        public const short TM_NUM_DEC = 37;
        // time units 
        public const short TM_TIME_UNITS = 38;
        // add new track automatically 
        public const short TM_ADD_AUTO_TRACK = 39;
        // search radius for auto-tracking 
        public const short TM_SEARCH_RADIUS = 40;
        // init count/size 
        public const short TM_INIT_AUTO_TRACKING = 41;
        // enable/disable update 
        public const short TM_UPDATE_SET = 42;
        // find all tracks automatically 
        public const short TM_ADD_AUTO_ALL_TRACKS = 43;
        // acceleration limit 
        public const short TM_ACCEL_LIMIT = 44;
        // auto acceleration limit 
        public const short TM_AUTO_ACCEL_LIMIT = 45;
        // partial tracks 
        public const short TM_PARTIAL_TRACKS = 46;
        // minimum track length 
        public const short TM_MIN_TRACK_LEN = 47;
        // show outlines 
        public const short TM_SHOW_OUTLINES = 48;
        // auto-split 
        public const short TM_AUTO_SPLIT = 49;
        // watershed split 
        public const short TM_WATERSHED_SPLIT = 70;
        // allow shared objects 
        public const short TM_SHARED_OBJECTS = 71;
        // motion type 
        public const short TM_MOTION_TYPE = 72;
        // minimum track length in calibrated units 
        public const short TM_MIN_TRACK_LENGTH = 73;
        // swap rows/columns of data table exporting data to Excel 
        public const short TM_SWAP_RC = 74;
        // merge selected tracks 
        public const short TM_MERGE_SELECTED = 75;
        // get number of selected measurements 
        public const short TM_NUM_SEL_MEAS_GET = 76;
        // 1- overlay shows complete track, 0 - shows partial track relatively to active frame 
        public const short TM_SHOW_COMPLETE_TRACK = 77;
        // defines track head length when TM_SHOW_COMPLETE_TRACK=0 
        public const short TM_TRACK_HEAD_LENGTH = 78;
        // defines track tail length when TM_SHOW_COMPLETE_TRACK=0 
        public const short TM_TRACK_TAIL_LENGTH = 79;
        // track only one object 
        public const short TM_TRACK_ONE_OBJ = 80;
        // split track 
        public const short TM_SPLIT_TRACK = 81;
        // add intensity track 
        public const short TM_ADD_INT_TRACK = 82;
        // show outlines of the active frame 
        public const short TM_TRACK_SHOW_OUTLINES = 83;
        // apply coherence filtering 
        public const short TM_TRACK_COHER_FLTR = 84;
        // angle range (in st.deviations) for coherence filtering 
        public const short TM_TRACK_ANGLE_DEV = 85;
        // coherence filter size as percent of image size 
        public const short TM_TRACK_COHR_FLT_SIZE = 86;
        // tracking prediction depth 
        public const short TM_TRACK_PREDICTION = 87;
        // add correlation track 
        public const short TM_ADD_CORREL_TRACK = 88;
        // correlation options, use Previous frame/First 
        public const short TM_TRACK_CORR_REF_PREV = 89;
        // correlation options, use scaling 
        public const short TM_TRACK_CORR_SCALE = 90;
        // correlation options, use rotation 
        public const short TM_TRACK_CORR_ROT = 91;
        // correlation options, phase correlation/full correlation 
        public const short TM_TRACK_CORR_PHASE = 92;
        // type of measurement index in data table: frame index, relative time or absolute time 
        public const short TM_TRACK_DATA_INDEX = 93;
        // use custom frame interval, if false use frame capture time 
        public const short TM_TRACK_USE_CUSTOM_INTERVAL = 94;
        // custom frame interval in seconds 
        public const short TM_TRACK_CUSTOM_INTERVAL = 95;
        // smoothing of track coordinates using Running average filter 
        public const short TM_TRACK_SMOOTHING = 96;
        // correlation threshold 
        public const short TM_TRACK_CORREL_THRESH = 97;
        // set reference track 
        public const short TM_REF_TRACK_SET = 98;
        // set normalization track 
        public const short TM_NORM_TRACK_SET = 99;
        // use center of mass instead of geometrical object center 
        public const short TM_TRACK_USE_CENTEROFMASS = 100;
        // remeasure selected tracks 
        public const short TM_REMEASURE_SELECTED = 101;
        // export measurements index in separate row/column 
        public const short TM_EXPORT_INDEX_ROW = 102;
        // normalize count/size measurements by maximum value in track 
        public const short TM_TRACK_NORM_BY_MAX = 103;
        // set active frame of tracking sequence 
        public const short TM_ACTIVE_FRAME = 104;
        // gets start frame of the track 
        public const short TM_START_GET = 105;

        // graph variables 
        // graph measurement 
        public const short TM_GRAPH_MEAS = 50;
        // graph range auto 
        public const short TM_GRAPH_RANGE_AUTO = 51;
        // graph range min 
        public const short TM_GRAPH_RANGE_MIN = 52;
        // graph range max 
        public const short TM_GRAPH_RANGE_MAX = 53;
        // graph X label type 
        public const short TM_GRAPH_X_LABELS = 54;

        // tracking enumerations 
        // attributes for IpTrackMeas 
        public enum TMEAS_ATTRIBUTES
        {
            TMEAS_TRACK_COLOR = Ipc32v5.TM_TRACK_COLOR,
            // line color 
            TMEAS_SEL_COLOR = Ipc32v5.TM_SEL_COLOR,
            // selection color 
            TMEAS_EL_SIZE = Ipc32v5.TM_EL_SIZE,
            // arrow size 
            TMEAS_TEXT_COLOR = Ipc32v5.TM_TEXT_COLOR,
            // text color 
            TMEAS_FONT_SIZE = Ipc32v5.TM_FONT_SIZE,
            // font size 
            TMEAS_LABEL_TYPE = Ipc32v5.TM_LABEL_TYPE,
            // label type (name,first measurement,none) 
            TMEAS_RESET_MEAS = Ipc32v5.TM_RESET_MEAS,
            // resets track list 
            TMEAS_ADD_MEAS = Ipc32v5.TM_ADD_MEAS,
            // adds measurement to the list 
            TMEAS_SHOW_STATS = Ipc32v5.TM_SHOW_STATS,
            // shows statistics 
            TMEAS_STATS_GET = Ipc32v5.TM_STATS_GET,
            // gets statistics 
            TMEAS_NUM_TRACKS_GET = Ipc32v5.TM_NUM_TRACKS_GET,
            // gets number of tracks 
            TMEAS_NUM_POINTS_GET = Ipc32v5.TM_NUM_POINTS_GET,
            // gets number of points in track 
            TMEAS_POINTS_GET = Ipc32v5.TM_POINTS_GET,
            // gets coordinates of points of tracks 
            TMEAS_NUM_MEAS_GET = Ipc32v5.TM_NUM_MEAS_GET,
            // gets number of active measurements 
            TMEAS_MEAS_LIST_GET = Ipc32v5.TM_MEAS_LIST_GET,
            // gets list of active measurements 
            TMEAS_MEAS_GET = Ipc32v5.TM_MEAS_GET,
            // gets measurement from manual or volume objects 
            TMEAS_SEL_GET = Ipc32v5.TM_SEL_GET,
            // gets selection flag from manual or volume objects 
            TMEAS_SEL_SET = Ipc32v5.TM_SEL_SET,
            // sets selection to from manual or volume objects 
            TMEAS_SHOW_GET = Ipc32v5.TM_SHOW_GET,
            // gets hide flag from manual or volume objects 
            TMEAS_SHOW_SET = Ipc32v5.TM_SHOW_SET,
            // sets hide flag to manual or volume objects 
            TMEAS_NAME_GET = Ipc32v5.TM_NAME_GET,
            // gets name of manual or volume objects 
            TMEAS_NAME_SET = Ipc32v5.TM_NAME_SET,
            // sets name of manual or volume objects 
            TMEAS_UPDATE = Ipc32v5.TM_UPDATE,
            // updates manual measurements data table and objects applying new settings 
            TMEAS_TYPE_GET = Ipc32v5.TM_TYPE_GET,
            // gets type of manual measurement element (IP_REND_MM_POINT,IP_REND_MM_LINE,IP_REND_MM_POLY_LINE,IP_REND_MM_ANGLE) 
            TMEAS_ACTION = Ipc32v5.TM_ACTION,
            // sets measurements action (mmActionSelect,mmActionAddPoint,mmActionAddLine,...) 
            TMEAS_CREATE_MEAS = Ipc32v5.TM_CREATE_MEAS,
            // creates new measuremetn based on the selected objects 
            TMEAS_SHOW_ALL = Ipc32v5.TM_SHOW_ALL,
            // shows all objects 
            TMEAS_SHOW_SELECTED = Ipc32v5.TM_SHOW_SELECTED,
            // shows/hides selected objects 
            TMEAS_DELETE_ALL = Ipc32v5.TM_DELETE_ALL,
            // deletes all objects 
            TMEAS_DELETE_SELECTED = Ipc32v5.TM_DELETE_SELECTED,
            // deletes selected objects 
            TMEAS_COLORING = Ipc32v5.TM_COLORING,
            // coloring 
            TMEAS_ADD_TRACK = Ipc32v5.TM_ADD_TRACK,
            // add new track 
            TMEAS_COLOR_GET = Ipc32v5.TM_COLOR_GET,
            // set color 
            TMEAS_COLOR_SET = Ipc32v5.TM_COLOR_SET,
            // get color 
            TMEAS_NUM_DEC = Ipc32v5.TM_NUM_DEC,
            // number of digits after decimal 
            TMEAS_TIME_UNITS = Ipc32v5.TM_TIME_UNITS,
            // time units 
            TMEAS_ADD_AUTO_TRACK = Ipc32v5.TM_ADD_AUTO_TRACK,
            // add new track automatically 
            TMEAS_SEARCH_RADIUS = Ipc32v5.TM_SEARCH_RADIUS,
            // search radius for auto-tracking 
            TMEAS_INIT_AUTO_TRACKING = Ipc32v5.TM_INIT_AUTO_TRACKING,
            // init count/size 
            TMEAS_UPDATE_SET = Ipc32v5.TM_UPDATE_SET,
            // enable/disable update 
            TMEAS_ADD_AUTO_ALL_TRACKS = Ipc32v5.TM_ADD_AUTO_ALL_TRACKS,
            // find all tracks automatically 
            TMEAS_ACCEL_LIMIT = Ipc32v5.TM_ACCEL_LIMIT,
            // acceleration limit 
            TMEAS_AUTO_ACCEL_LIMIT = Ipc32v5.TM_AUTO_ACCEL_LIMIT,
            // auto acceleration limit 
            TMEAS_PARTIAL_TRACKS = Ipc32v5.TM_PARTIAL_TRACKS,
            // partial tracks 
            TMEAS_MIN_TRACK_LEN = Ipc32v5.TM_MIN_TRACK_LEN,
            // minimum track length 
            TMEAS_SHOW_OUTLINES = Ipc32v5.TM_SHOW_OUTLINES,
            // show outlines 
            TMEAS_AUTO_SPLIT = Ipc32v5.TM_AUTO_SPLIT,
            // auto-split 
            TMEAS_WATERSHED_SPLIT = Ipc32v5.TM_WATERSHED_SPLIT,
            // watershed split 
            TMEAS_SHARED_OBJECTS = Ipc32v5.TM_SHARED_OBJECTS,
            // allow shared objects 
            TMEAS_MOTION_TYPE = Ipc32v5.TM_MOTION_TYPE,
            // motion type 
            TMEAS_MIN_TRACK_LENGTH = Ipc32v5.TM_MIN_TRACK_LENGTH,
            // minimum track length in calibrated units 
            TMEAS_SWAP_RC = Ipc32v5.TM_SWAP_RC,
            // swap rows/columns of data table exporting data to Excel 
            TMEAS_MERGE_SELECTED = Ipc32v5.TM_MERGE_SELECTED,
            // merge selected tracks 
            TMEAS_NUM_SEL_MEAS_GET = Ipc32v5.TM_NUM_SEL_MEAS_GET,
            // get number of selected measurements 
            TMEAS_SHOW_COMPLETE_TRACK = Ipc32v5.TM_SHOW_COMPLETE_TRACK,
            // 1- overlay shows complete track, 0 - shows partial track relatively to active frame 
            TMEAS_TRACK_HEAD_LENGTH = Ipc32v5.TM_TRACK_HEAD_LENGTH,
            // defines track head length when TM_SHOW_COMPLETE_TRACK=0 
            TMEAS_TRACK_TAIL_LENGTH = Ipc32v5.TM_TRACK_TAIL_LENGTH,
            // defines track tail length when TM_SHOW_COMPLETE_TRACK=0 
            TMEAS_TRACK_ONE_OBJ = Ipc32v5.TM_TRACK_ONE_OBJ,
            // track only one object 
            TMEAS_SPLIT_TRACK = Ipc32v5.TM_SPLIT_TRACK,
            // split track 
            TMEAS_ADD_INT_TRACK = Ipc32v5.TM_ADD_INT_TRACK,
            // add intensity track 
            TMEAS_TRACK_SHOW_OUTLINES = Ipc32v5.TM_TRACK_SHOW_OUTLINES,
            // show outlines of the active frame 
            TMEAS_TRACK_COHER_FLTR = Ipc32v5.TM_TRACK_COHER_FLTR,
            // apply coherence filtering 
            TMEAS_TRACK_ANGLE_DEV = Ipc32v5.TM_TRACK_ANGLE_DEV,
            // angle range (in st.deviations) for coherence filtering 
            TMEAS_TRACK_COHR_FLT_SIZE = Ipc32v5.TM_TRACK_COHR_FLT_SIZE,
            // coherence filter size as a percentage of image size 
            TMEAS_TRACK_PREDICTION = Ipc32v5.TM_TRACK_PREDICTION,
            // tracking prediction depth 
            TMEAS_ADD_CORREL_TRACK = Ipc32v5.TM_ADD_CORREL_TRACK,
            // add correlation track 
            TMEAS_TRACK_CORR_REF_PREV = Ipc32v5.TM_TRACK_CORR_REF_PREV,
            // correlation options, use Previous frame/First 
            TMEAS_TRACK_CORR_SCALE = Ipc32v5.TM_TRACK_CORR_SCALE,
            // correlation options, use scaling 
            TMEAS_TRACK_CORR_ROT = Ipc32v5.TM_TRACK_CORR_ROT,
            // correlation options, use rotation 
            TMEAS_TRACK_CORR_PHASE = Ipc32v5.TM_TRACK_CORR_PHASE,
            // correlation options, phase correlation/full correlation 
            TMEAS_TRACK_DATA_INDEX = Ipc32v5.TM_TRACK_DATA_INDEX,
            // type of measurement index in data table: frame index, relative time or absolute time 
            TMEAS_TRACK_USE_CUSTOM_INTERVAL = Ipc32v5.TM_TRACK_USE_CUSTOM_INTERVAL,
            // use custom frame interval, if false use frame capture time 
            TMEAS_TRACK_CUSTOM_INTERVAL = Ipc32v5.TM_TRACK_CUSTOM_INTERVAL,
            // custom frame interval in seconds 
            TMEAS_TRACK_SMOOTHING = Ipc32v5.TM_TRACK_SMOOTHING,
            // smoothing of track coordinates using Running average filter 
            TMEAS_TRACK_CORREL_THRESH = Ipc32v5.TM_TRACK_CORREL_THRESH,
            // correlation threshold 
            TMEAS_REF_TRACK_SET = Ipc32v5.TM_REF_TRACK_SET,
            // set reference track 
            TMEAS_NORM_TRACK_SET = Ipc32v5.TM_NORM_TRACK_SET,
            // set normalization track 
            TMEAS_USE_CENTEROFMASS = Ipc32v5.TM_TRACK_USE_CENTEROFMASS,
            // use center of mass instead of geometrical object center 
            TMEAS_EXPORT_INDEX_ROW = Ipc32v5.TM_EXPORT_INDEX_ROW,
            // export measurements index in separate row/column 
            TMEAS_NORM_BY_MAX = Ipc32v5.TM_TRACK_NORM_BY_MAX,
            // normalize count/size measurements by maximum value in track 
            TMEAS_START_GET = Ipc32v5.TM_START_GET
            // gets start frame of the track 
        }

        // attributes for IpTrackMeasSet 
        // graph variables 
        public enum TMSET_ATTRIBUTES
        {
            TMSET_TRACK_COLOR = Ipc32v5.TM_TRACK_COLOR,
            // line color 
            TMSET_SEL_COLOR = Ipc32v5.TM_SEL_COLOR,
            // selection color 
            TMSET_EL_SIZE = Ipc32v5.TM_EL_SIZE,
            // arrow size 
            TMSET_TEXT_COLOR = Ipc32v5.TM_TEXT_COLOR,
            // text color 
            TMSET_FONT_SIZE = Ipc32v5.TM_FONT_SIZE,
            // font size 
            TMSET_LABEL_TYPE = Ipc32v5.TM_LABEL_TYPE,
            // label type (name,first measurement,none) 
            TMSET_RESET_MEAS = Ipc32v5.TM_RESET_MEAS,
            // resets track list 
            TMSET_ADD_MEAS = Ipc32v5.TM_ADD_MEAS,
            // adds measurement to the list 
            TMSET_SHOW_STATS = Ipc32v5.TM_SHOW_STATS,
            // shows statistics 
            TMSET_SEL_SET = Ipc32v5.TM_SEL_SET,
            // sets selection to from manual or volume objects 
            TMSET_SHOW_SET = Ipc32v5.TM_SHOW_SET,
            // sets hide flag to manual or volume objects 
            TMSET_UPDATE = Ipc32v5.TM_UPDATE,
            // updates manual measurements data table and objects applying new settings 
            TMSET_ACTION = Ipc32v5.TM_ACTION,
            // sets measurements action (mmActionSelect,mmActionAddPoint,mmActionAddLine,...) 
            TMSET_CREATE_MEAS = Ipc32v5.TM_CREATE_MEAS,
            // creates new measuremetn based on the selected objects 
            TMSET_SHOW_ALL = Ipc32v5.TM_SHOW_ALL,
            // shows all objects 
            TMSET_SHOW_SELECTED = Ipc32v5.TM_SHOW_SELECTED,
            // shows/hides selected objects 
            TMSET_DELETE_ALL = Ipc32v5.TM_DELETE_ALL,
            // deletes all objects 
            TMSET_DELETE_SELECTED = Ipc32v5.TM_DELETE_SELECTED,
            // deletes selected objects 
            TMSET_COLORING = Ipc32v5.TM_COLORING,
            // coloring 
            TMSET_ADD_TRACK = Ipc32v5.TM_ADD_TRACK,
            // add new track 
            TMSET_COLOR_SET = Ipc32v5.TM_COLOR_SET,
            // set color 
            TMSET_NUM_DEC = Ipc32v5.TM_NUM_DEC,
            // number of digits after decimal 
            TMSET_TIME_UNITS = Ipc32v5.TM_TIME_UNITS,
            // time units 
            TMSET_ADD_AUTO_TRACK = Ipc32v5.TM_ADD_AUTO_TRACK,
            // add new track automatically 
            TMSET_SEARCH_RADIUS = Ipc32v5.TM_SEARCH_RADIUS,
            // search radius for auto-tracking 
            TMSET_INIT_AUTO_TRACKING = Ipc32v5.TM_INIT_AUTO_TRACKING,
            // init count/size 
            TMSET_UPDATE_SET = Ipc32v5.TM_UPDATE_SET,
            // enable/disable update 
            TMSET_ADD_AUTO_ALL_TRACKS = Ipc32v5.TM_ADD_AUTO_ALL_TRACKS,
            // find all tracks automatically 
            TMSET_ACCEL_LIMIT = Ipc32v5.TM_ACCEL_LIMIT,
            // acceleration limit 
            TMSET_AUTO_ACCEL_LIMIT = Ipc32v5.TM_AUTO_ACCEL_LIMIT,
            // auto acceleration limit 
            TMSET_PARTIAL_TRACKS = Ipc32v5.TM_PARTIAL_TRACKS,
            // partial tracks 
            TMSET_MIN_TRACK_LEN = Ipc32v5.TM_MIN_TRACK_LEN,
            // minimum track length 
            TMSET_SHOW_OUTLINES = Ipc32v5.TM_SHOW_OUTLINES,
            // show outlines 
            TMSET_AUTO_SPLIT = Ipc32v5.TM_AUTO_SPLIT,
            // auto-split 
            TMSET_WATERSHED_SPLIT = Ipc32v5.TM_WATERSHED_SPLIT,
            // watershed split 
            TMSET_SHARED_OBJECTS = Ipc32v5.TM_SHARED_OBJECTS,
            // allow shared objects 
            TMSET_MOTION_TYPE = Ipc32v5.TM_MOTION_TYPE,
            // motion type 
            TMSET_MIN_TRACK_LENGTH = Ipc32v5.TM_MIN_TRACK_LENGTH,
            // minimum track length in calibrated units 
            TMSET_SWAP_RC = Ipc32v5.TM_SWAP_RC,
            // swap rows/columns of data table exporting data to Excel 
            TMSET_MERGE_SELECTED = Ipc32v5.TM_MERGE_SELECTED,
            // merge selected tracks 
            TMSET_NUM_SEL_MEAS_GET = Ipc32v5.TM_NUM_SEL_MEAS_GET,
            // get number of selected measurements 
            TMSET_SHOW_COMPLETE_TRACK = Ipc32v5.TM_SHOW_COMPLETE_TRACK,
            // 1- overlay shows complete track, 0 - shows partial track relatively to active frame 
            TMSET_TRACK_HEAD_LENGTH = Ipc32v5.TM_TRACK_HEAD_LENGTH,
            // defines track head length when TM_SHOW_COMPLETE_TRACK=0 
            TMSET_TRACK_TAIL_LENGTH = Ipc32v5.TM_TRACK_TAIL_LENGTH,
            // defines track tail length when TM_SHOW_COMPLETE_TRACK=0 
            TMSET_TRACK_ONE_OBJ = Ipc32v5.TM_TRACK_ONE_OBJ,
            // track only one object 
            TMSET_SPLIT_TRACK = Ipc32v5.TM_SPLIT_TRACK,
            // split track 
            TMSET_ADD_INT_TRACK = Ipc32v5.TM_ADD_INT_TRACK,
            // add intensity track 
            TMSET_TRACK_SHOW_OUTLINES = Ipc32v5.TM_TRACK_SHOW_OUTLINES,
            // show outlines of the active frame 
            TMSET_TRACK_COHER_FLTR = Ipc32v5.TM_TRACK_COHER_FLTR,
            // apply coherence filtering 
            TMSET_TRACK_ANGLE_DEV = Ipc32v5.TM_TRACK_ANGLE_DEV,
            // angle range (in st.deviations) for coherence filtering 
            TMSET_TRACK_COHR_FLT_SIZE = Ipc32v5.TM_TRACK_COHR_FLT_SIZE,
            // coherence filter size as a percentage of image size 
            TMSET_TRACK_PREDICTION = Ipc32v5.TM_TRACK_PREDICTION,
            // tracking prediction depth 
            TMSET_ADD_CORREL_TRACK = Ipc32v5.TM_ADD_CORREL_TRACK,
            // add correlation track 
            TMSET_TRACK_CORR_REF_PREV = Ipc32v5.TM_TRACK_CORR_REF_PREV,
            // correlation options, use Previous frame/First 
            TMSET_TRACK_CORR_SCALE = Ipc32v5.TM_TRACK_CORR_SCALE,
            // correlation options, use scaling 
            TMSET_TRACK_CORR_ROT = Ipc32v5.TM_TRACK_CORR_ROT,
            // correlation options, use rotation 
            TMSET_TRACK_CORR_PHASE = Ipc32v5.TM_TRACK_CORR_PHASE,
            // correlation options, phase correlation/full correlation 
            TMSET_TRACK_DATA_INDEX = Ipc32v5.TM_TRACK_DATA_INDEX,
            // type of measurement index in data table: frame index, relative time or absolute time 
            TMSET_TRACK_USE_CUSTOM_INTERVAL = Ipc32v5.TM_TRACK_USE_CUSTOM_INTERVAL,
            // use custom frame interval, if false use frame capture time 
            TMSET_TRACK_CUSTOM_INTERVAL = Ipc32v5.TM_TRACK_CUSTOM_INTERVAL,
            // custom frame interval in seconds 
            TMSET_TRACK_SMOOTHING = Ipc32v5.TM_TRACK_SMOOTHING,
            // smoothing of track coordinates using Running average filter 
            TMSET_TRACK_CORREL_THRESH = Ipc32v5.TM_TRACK_CORREL_THRESH,
            // correlation threshold 
            TMSET_REF_TRACK_SET = Ipc32v5.TM_REF_TRACK_SET,
            // set reference track 
            TMSET_NORM_TRACK_SET = Ipc32v5.TM_NORM_TRACK_SET,
            // set normalization track 
            TMSET_USE_CENTEROFMASS = Ipc32v5.TM_TRACK_USE_CENTEROFMASS,
            // use center of mass instead of geometrical object center 
            TMSET_EXPORT_INDEX_ROW = Ipc32v5.TM_EXPORT_INDEX_ROW,
            // export measurements index in separate row/column 
            TMSET_NORM_BY_MAX = Ipc32v5.TM_TRACK_NORM_BY_MAX,
            // normalize count/size measurements by maximum value in track 
            TMSET_GRAPH_MEAS = Ipc32v5.TM_GRAPH_MEAS,
            // graph measurement 
            TMSET_GRAPH_RANGE_AUTO = Ipc32v5.TM_GRAPH_RANGE_AUTO,
            // graph range auto 
            TMSET_GRAPH_RANGE_MIN = Ipc32v5.TM_GRAPH_RANGE_MIN,
            // graph range min 
            TMSET_GRAPH_RANGE_MAX = Ipc32v5.TM_GRAPH_RANGE_MAX,
            // graph range max 
            TMSET_GRAPH_X_LABELS = Ipc32v5.TM_GRAPH_X_LABELS,
            // graph X label type 
            TMSET_REMEASURE_SELECTED = Ipc32v5.TM_REMEASURE_SELECTED,
            // remeasure selected tracks 
            TMSET_ACTIVE_FRAME = Ipc32v5.TM_ACTIVE_FRAME
            // set active frame of tracking sequence 
        }

        // attributes for IpTrackMeasSetStr 
        public enum TMSETSTR_ATTRIBUTES
        {
            TMSETSTR_TRACK_PREF_SET = Ipc32v5.TM_TRACK_PREF_SET,
            // sets track prefix 
            TMSETSTR_NAME_SET = Ipc32v5.TM_NAME_SET
            // sets name of manual or volume objects 
        }

        // attributes for IpTrackMeasGetStr 
        public enum TMGETSTR_ATTRIBUTES
        {
            TMGETSTR_TRACK_PREF_GET = Ipc32v5.TM_TRACK_PREF_GET,
            // gets track prefix 
            TMGETSTR_NAME_GET = Ipc32v5.TM_NAME_GET
            // gets name of manual or volume objects 
        }

        // show/hide 
        public enum TMSHOW_COMMANDS
        {
            TMSHOW_HIDE = Ipc32v5.TRACK_HIDE,
            TMSHOW_SHOW = Ipc32v5.TRACK_SHOW
        }

        // tracking windows 
        public enum TM_DIALOGS
        {
            TMDLG_TABLE = Ipc32v5.TRACK_TABLE,
            TMDLG_GRAPH = Ipc32v5.TRACK_GRAPH
        }

        // constants for IpTrackSaveData 
        // sources of data 
        public enum TM_SRC_TYPE
        {
            TMSRC_DATA = Ipc32v5.TR_MM_DATA,
            // measurements table 
            TMSRC_STATS = Ipc32v5.TR_MM_STATS,
            // 
            TMSRC_ACTIVE = Ipc32v5.TR_MM_ACTIVE,
            // 
            TMSRC_GRAPH = Ipc32v5.TR_GRAPH
            // 
        }

        // types of data 
        // TRDF_CSV and TRDF_HTML cannot be combined. In absence of either, will be tab-delimited 
        public enum TM_DST_TYPE
        {
            TMDST_TABLE = Ipc32v5.TRF_TABLE,
            // raw data in tab-delimited form 
            TMDST_GRAPH = Ipc32v5.TRF_GRAPH,
            // metafile graph format 
            TMDSTF_FILE = Ipc32v5.TRDF_FILE,
            // File (can be used with APPEND, CSV, or HTML, does not support GRAPH) 
            TMDSTF_CLIPBOARD = Ipc32v5.TRDF_CLIPBOARD,
            // Clipboard 
            TMDSTF_DDE = Ipc32v5.TRDF_DDE,
            // DDE to Excel 
            TMDSTF_MASK = Ipc32v5.TRDF_MASK,
            // destination mask 
            TMDSTF_CSV = Ipc32v5.TRDF_CSV,
            // comma-separated values 
            TMDSTF_HTML = Ipc32v5.TRDF_HTML,
            // HTML table 
            TMDSTF_LAST = Ipc32v5.TRDF_LAST
            // last type 
        }
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]


        //----------------------------------------------------------------- 
        // Trac Functions 
        //----------------------------------------------------------------- 
        //UPGRADE_WARNING: 構造体 TMSHOW_COMMANDS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 TM_DIALOGS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpTrackShow(TM_DIALOGS sDialog, TMSHOW_COMMANDS eShow);
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 TM_DIALOGS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpTrackMove(TM_DIALOGS sDialog, short xPos, short yPos);
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 TM_DIALOGS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpTrackSize(TM_DIALOGS sDialog, short xSize, short ySize);
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int IpTrackOptionsFile(string szFileName, short bSave);
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpTrackFile(string szFileName, short bSave);
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        //tracking measurements 
        //UPGRADE_ISSUE: パラメータ 'As Any' の宣言はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 TMEAS_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpTrackMeas(TMEAS_ATTRIBUTES sCommand, int lOpt1, ref int lParam);
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 TMSET_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpTrackMeasSet(TMSET_ATTRIBUTES sCommand, int lOpt1, double lParam);
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 TMSETSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpTrackMeasSetStr(TMSETSTR_ATTRIBUTES sCommand, int lOpt1, string lpszDest);
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 TMGETSTR_ATTRIBUTES に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpTrackMeasGetStr(TMGETSTR_ATTRIBUTES sCommand, int lOpt1, string lpszDest);
        [DllImport("IPTRACK32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 TM_DST_TYPE に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        //UPGRADE_WARNING: 構造体 TM_SRC_TYPE に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpTrackSaveData(TM_SRC_TYPE sSrcFlags, TM_DST_TYPE sDstFlags, string lpszDest);

        //typedef short TMSET_ATTRIBUTES; 


        // ipcView3D.h 
        // 
        // Definitions for Auto-Pro Functions of IpView3D.DLL 
        // 
        // Copyright (c) 1999 - 2005, Media Cybernetics, Inc. 
        // 




        // constants for IpView3DSet 
        // image channel to load 
        public const short V3D_CHANNEL = 1;
        // 
        public const short V3D_VOXELSIZE = 2;
        // 
        public const short V3D_SUBSAMPLING = 3;
        // 
        public const short V3D_ACTIVE_PORTION = 4;
        // 
        public const short V3D_BACK_COLOR = 5;
        // 
        public const short V3D_AUTO_RELOAD_MODIFY = 6;
        // 
        public const short V3D_AUTO_RELOAD_TIME = 7;
        // 
        public const short V3D_AUTO_RELOAD_LUT = 8;
        // 
        public const short V3D_TRANSP = 9;
        // 
        public const short V3D_SLICES = 10;
        // 
        public const short V3D_SHOW_AXES = 11;
        // 
        public const short V3D_ANIM_ANGLE = 12;
        // 
        public const short V3D_ANIM_FRAMES = 13;
        // 
        public const short V3D_ORTHO = 14;
        // 
        public const short V3D_MIP = 15;
        // camera zoom 
        public const short V3D_ZOOM = 16;

        // V3D enumerations 

        public enum V3DSET_ATTR
        {
            V3DSET_CHANNEL = Ipc32v5.V3D_CHANNEL,
            // image channel to load 
            V3DSET_VOXELSIZE = Ipc32v5.V3D_VOXELSIZE,
            // 
            V3DSET_SUBSAMPLING = Ipc32v5.V3D_SUBSAMPLING,
            // 
            V3DSET_ACTIVE_PORTION = Ipc32v5.V3D_ACTIVE_PORTION,
            // 
            V3DSET_BACK_COLOR = Ipc32v5.V3D_BACK_COLOR,
            // 
            V3DSET_AUTO_RELOAD_MODIFY = Ipc32v5.V3D_AUTO_RELOAD_MODIFY,
            // 
            V3DSET_AUTO_RELOAD_TIME = Ipc32v5.V3D_AUTO_RELOAD_TIME,
            // 
            V3DSET_AUTO_RELOAD_LUT = Ipc32v5.V3D_AUTO_RELOAD_LUT,
            // 
            V3DSET_TRANSP = Ipc32v5.V3D_TRANSP,
            // 
            V3DSET_SLICES = Ipc32v5.V3D_SLICES,
            // 
            V3DSET_SHOW_AXES = Ipc32v5.V3D_SHOW_AXES,
            // 
            V3DSET_ANIM_ANGLE = Ipc32v5.V3D_ANIM_ANGLE,
            // 
            V3DSET_ANIM_FRAMES = Ipc32v5.V3D_ANIM_FRAMES,
            // 
            V3DSET_ORTHO = Ipc32v5.V3D_ORTHO,
            // 
            V3DSET_MIP = Ipc32v5.V3D_MIP,
            // 
            V3DSET_ZOOM = Ipc32v5.V3D_ZOOM
            // camera zoom 
        }

        public enum V3DSHOW_FLAGS
        {
            V3D_SHOW = 1,
            V3D_HIDE = 0
        }
        [DllImport("IPVIEW3D", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]


        //----------------------------------------------------------------- 
        // View3D Functions 
        //----------------------------------------------------------------- 
        //UPGRADE_WARNING: 構造体 V3DSHOW_FLAGS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpView3DCreate(V3DSHOW_FLAGS bShow);
        [DllImport("IPVIEW3D", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 V3DSHOW_FLAGS に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpView3DShow(int WinID, V3DSHOW_FLAGS bShow);
        [DllImport("IPVIEW3D", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpView3DMove(int WinID, int x, int y);
        [DllImport("IPVIEW3D", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpView3DSize(int WinID, int x, int y);
        [DllImport("IPVIEW3D", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //UPGRADE_WARNING: 構造体 V3DSET_ATTR に、この Declare ステートメントの引数としてマーシャリング属性を渡す必要があります。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"' をクリックしてください。
        public static extern int IpView3DSet(int WinID, V3DSET_ATTR sAttribute, float dParam1, float dParam2, float dParam3);
        [DllImport("IPVIEW3D", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpView3DSetCamera(int WinID, float x, float y, float z, float w);
        [DllImport("IPVIEW3D", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpView3DLoad(int WinID);
        [DllImport("IPVIEW3D", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpView3DReLoad(int WinID);
        [DllImport("IPVIEW3D", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpView3DCopy(int WinID);
        [DllImport("IPVIEW3D", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IpView3DCreateAnimation(int WinID);


         */
        #endregion 

    }
}

