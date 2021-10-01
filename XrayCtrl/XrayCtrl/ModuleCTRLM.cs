using System;
using System.IO;
using System.Text;

namespace XrayCtrl
{
	internal static class ModuleCTRLM
	{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		Option Explicit
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		public static int gTosCount = 0;
		public static cValue gValue;

		//Etc
		public static int ipEmergency = 0;
		//Public ipTouch_Sens     As Integer
		public static int ipErrsts = 0;
		public static int ifErrrst = 0;

		public static int ifEventValue = 0;
		public static int ifOldEventValue = 0;

		//XRAY
        //　/// ipX_type /////////////////////
        //  0:90kV      L9421,Kevex90
        //  1:130kV     L9181,Kevex130
        //  2:160kV     L9191,FINFOCUS
        //  3:230kV     L10801
        //  4:90kV      L8601
        //  5:90kV      L9421-02
        //  6:
        //  7:
        //  8:
        //  9:
        //  10:150kV    L8121-02
        //  11:
        //  12:
        //  13:
        //　/////////////////////////////////
        public static int ipX_type = 0;     
        
        public static float ipX_Volt = 0;
		public static float ifX_Volt = 0;
		public static int ifX_Volt_Up = 0;
		public static int ifX_Volt_Down = 0;
		public static float ipX_Amp = 0;
		public static float ifX_Amp = 0;
		public static int ifX_Amp_Up = 0;
		public static int ifX_Amp_Down = 0;
		public static float ipX_Watt = 0;
		public static int ipX_Fine = 0;
		public static int ipX_On = 0;
		public static int ifX_On = 0;
		public static int ipStandby = 0;
		public static int ipInterlock = 0;
		public static int ipFocussize = 0;
		public static int ifFocussize = 0;
		public static int ipWarmup = 0;
		public static int ipXStatus = 0;
		public static int ipXPermitWarmup = 0;
		public static int ipWarmup_Mode = 0;
		public static int ifWarmup_Cancel = 0;
		public static int ifWarmup_Reset = 0;
		public static int ipWrest_timeM = 0;
		public static int ipWrest_timeS = 0;
		public static int ipXcont_Mode = 0;
		public static int ifXcont_Mode = 0;
		public static int ipXtimer = 0;
		public static int ifXtimer = 0;

		//Public ipZoomrate       As Single
		//Public ipPow            As Integer
		//Public ifPow            As Integer
		//Public ipWkPos          As Single
		//Public ifWkPos          As Single
		//Public ipII_Scope       As Integer
		//Public ifII_Scope       As Integer
		//Public ifII_Scope2      As Integer
		//Public ifII_Scope4      As Integer
		//Public ifII_Scope7      As Integer

		public static float ipcndVolt = 0;
		public static float ipcndAmp = 0;

        public static int ipcndVoltSetErrFlg = 0; //Rev23.10 by長野 2015/09/29
        public static int ipcndAmpSetErrFlg = 0;  //Rev23.10 by長野 2015/09/29
         
        public const int UpDownRate = 150;

		public static int ipXAvail = 0;			//X線アベイラブル
		public static float ifXRMaxkV = 0;		//管電圧最大値
		public static float ifXRMaxmA = 0;		//管電流最大値
		public static float ipXRMaxkV = 0;		//管電圧最大値
		public static float ipXRMaxmA = 0;		//管電流最大値
		public static float ifXRMinkV = 0;		//管電圧最小値
		public static float ifXRMinmA = 0;		//管電流最小値
		public static float ipXRMinkV = 0;		//管電圧最小値
		public static float ipXRMinmA = 0;		//管電流最小値

		public static int ifAvailkV = 0;
		public static int ifAvailmA = 0;
		public static long ifAvailTimekV = 0;
		public static long ifAvailTimemA = 0;
		public static long ifAvailTimXOn = 0;
		public static long ifAvailTimkVmA = 0;
		public static int if_X_NoMoveTimer = 0;
		public static int ifXrayonoff_Set = 0;

		public static long ifAvTm40kV30mA = 0;     //Kevex用アベイラブル時間 added by 山本　2002-8-31
		public static long ifAvTm40kV40mA = 0;     //Kevex用アベイラブル時間 added by 山本　2002-8-31
		public static long ifAvTm60kV30mA = 0;     //Kevex用アベイラブル時間 added by 山本　2002-8-31
		public static long ifAvTm60kV40mA = 0;     //Kevex用アベイラブル時間 added by 山本　2002-8-31
		public static long ifAvTm80kV30mA = 0;     //Kevex用アベイラブル時間 added by 山本　2002-8-31
		public static long ifAvTm80kV40mA = 0;     //Kevex用アベイラブル時間 added by 山本　2002-8-31
		public static long ifAvTm100kV30mA = 0;    //Kevex用アベイラブル時間 added by 山本　2002-8-31
		public static long ifAvTm100kV40mA = 0;    //Kevex用アベイラブル時間 added by 山本　2002-8-31
		public static long ifAvTm120kV30mA = 0;    //Kevex用アベイラブル時間 added by 山本　2002-8-31
		public static long ifAvTm120kV40mA = 0;    //Kevex用アベイラブル時間 added by 山本　2002-8-31
		public static long ifAvTm130kV30mA = 0;    //Kevex用アベイラブル時間 added by 山本　2002-8-31
		public static long ifAvTm130kV40mA = 0;    //Kevex用アベイラブル時間 added by 山本　2002-8-31

		//Err
		public static int eRst = 0;
		public static int eSts = 0;

		public static int EmergencyErrDsp = 0;
		public static int EmergencyErrRst = 0;

		public static int AmpErrDsp = 0;
		public static int AmpErrRst = 0;
		public static int InterLockErrDsp = 0;
		public static int InterLockErrRst = 0;
		public static int NoSelectErrDsp = 0;
		public static int NoSelectErrRst = 0;
		public static int OffErrDsp = 0;
		public static int OffErrRst = 0;
		public static int OnErrDsp = 0;
		public static int OnErrRst = 0;
		public static int OnRyErrDsp = 0;
		public static int OnRyErrRst = 0;
		public static int OverlapErrDsp = 0;
		public static int OverlapErrRst = 0;
		public static int StandbyErrDsp = 0;
		public static int StandbyErrRst = 0;
		public static int UnitErrDsp = 0;
		public static int UnitErrRst = 0;
		public static int VoltErrDsp = 0;
		public static int VoltErrRst = 0;
		public static int TimOffErrDsp = 0;
		public static int TimOffErrRst = 0;

		private static mLogic.DelayType T101;
		private static mLogic.DelayType T102;
		private static mLogic.DelayType T103;
		private static mLogic.DelayType T104;
		private static mLogic.DelayType T105;
		private static mLogic.DelayType T108;

		//Private M101 As Integer
		//Private M102 As Integer
		//Private M103 As Integer
		//Private M104 As Integer
		private static int M105 = 0;
		private static int M106 = 0;
		private static int M107 = 0;
		private static int M108 = 0;
		private static int M109 = 0;
		//Private M110 As Integer
		//Private M111 As Integer
		//Private M113 As Integer
		//Private M114 As Integer
		private static int M115 = 0;
		private static int M116 = 0;
		private static int M117 = 0;
		private static int M118 = 0;
		private static int M119 = 0;
		private static int M120 = 0;
		private static int M121 = 0;
		private static int M122 = 0;
		private static int M123 = 0;
		private static int M124 = 0;
		private static int M125 = 0;
		private static int M126 = 0;
		private static int M127 = 0;
		private static int M128 = 0;
		private static int M129 = 0;
		private static int M130 = 0;
		private static int M131 = 0;
		private static int M132 = 0;
		private static int M133 = 0;
		private static int M134 = 0;
		private static int M135 = 0;
		private static int M136 = 0;
		private static int M137 = 0;
		private static int M138 = 0;
		private static int M139 = 0;
		private static int M148 = 0;
		private static int M149 = 0;
		private static int M150 = 0;
		private static int M151 = 0;
		private static int M152 = 0;
		//Private M153 As Integer
		//Private M154 As Integer
		//Private M155 As Integer
		//Private M156 As Integer
		private static int M157 = 0;
		//Private M158 As Integer
		//Private M159 As Integer
		//Private M160 As Integer
		//Private M161 As Integer
		//Private M162 As Integer
		//Private M163 As Integer
		//Private M164 As Integer
		//Private M165 As Integer
		//Private M166 As Integer
		//Private M167 As Integer
		//Private M168 As Integer
		//Private M169 As Integer
		//Private M170 As Integer
		//Private M171 As Integer
		//Private M172 As Integer
		//Private M173 As Integer
		//Private M174 As Integer
		//Private M175 As Integer
		private static int M176 = 0;
		private static int M177 = 0;
		private static int M178 = 0;
		private static int M179 = 0;
		private static int M180 = 0;
		//Private M181 As Integer
		//Private M182 As Integer
		//Private M183 As Integer
		//Private M184 As Integer
		//Private M201 As Integer
		//Private M202 As Integer
		//Private M203 As Integer
		//Private M204 As Integer
		//Private M205 As Integer
		//Private M206 As Integer
		//Private M207 As Integer
		//Private M208 As Integer
		//Private M209 As Integer
		//Private M210 As Integer
		//Private M211 As Integer
		//Private M214 As Integer
		//Private M215 As Integer
		//Private M216 As Integer
		//Private M217 As Integer
		//Private M218 As Integer
		//Private M219 As Integer
		//Private M220 As Integer
		//Private M221 As Integer
		//Private M222 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M223 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M224 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M225 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M226 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M227 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M228 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M229 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M230 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M231 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M232 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M233 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M234 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M235 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M236 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M237 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M238 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M239 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M240 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M241 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M242 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M243 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M244 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M245 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M246 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M247 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M248 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M249 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M250 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M251 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M252 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M253 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M254 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M255 As Integer    //v11.5未使用なので削除 by 間々田 2006/08/01

		private static int M502 = 0;
		private static int M504 = 0;
		private static int M514 = 0;
		private static int M518 = 0;
		private static int M519 = 0;

		private static float D101 = 0;
		private static float D102 = 0;
		private static float D103 = 0;
		//Private D104 As Single     //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D105 As Single     //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D106 As Single     //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D107 As Single     //v11.5未使用なので削除 by 間々田 2006/08/01
		private static float D108 = 0;
		private static float D109 = 0;
		private static float D110 = 0;
		private static float D111 = 0;
		//Private D112 As Single     //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D113 As Single     //v11.5未使用なので削除 by 間々田 2006/08/01
		private static float D114 = 0;

		//Private D201 As Single     //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D202 As Single     //v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D203 As Single     //v11.5未使用なので削除 by 間々田 2006/08/01

		private static float D501 = 0;
		private static float D502 = 0;

		public static void CtrlmLogic()
		{
			//==========================================================
			//入力
		    
//2000-03-08 T.Shibui ｳｫｰﾑｱｯﾌﾟ強制終了してないとX線ONできない
			if (ifXrayonoff_Set == 1)
			{
				if (M137 != 0)        //ｳｫｰﾑｱｯﾌﾟ強制終了
				{
					M105 = 1;        //X線ON
				}
			}
			else if (ifXrayonoff_Set == 2)
			{
				M108 = 1;            //X線OFF
			}
			else
			{
				M105 = 0;
				M108 = 0;
			}
			ifXrayonoff_Set = 0;
		    
			M106 = ModuleXRAYM.gXraymErr;	//X線異常
			M109 = ifXcont_Mode;
			if (ifXtimer >= 0)
			{
				D103 = ifXtimer;     //X線ﾀｲﾏｰ
				ifXtimer = -1;
			}
		    
			M179 = ModuleXRAYM.ComXon;		//X線ON中
			M180 = ModuleXRAYM.ComXoff;		//X線OFF中
			ModuleXRAYM.ComXoff = 0;
			D108 = ifXRMaxkV;        //最大管電圧値
			D109 = ifXRMaxmA;        //最大管電流値
			D110 = ifXRMinkV;        //最小管電圧値
			D111 = ifXRMinmA;        //最小管電流値
			M137 = ifWarmup_Reset;   //ｳｫｰﾑｱｯﾌﾟ強制終了
		//    M156 = gXraymWarmupEnd
			M138 = ifWarmup_Cancel;  //ｳｫｰﾑｱｯﾌﾟｷｬﾝｾﾙ
			ifWarmup_Cancel = 0;
			D114 = if_X_NoMoveTimer;
			D101 = ifX_Volt;         //管電圧値
			D102 = ifX_Amp;          //管電流値
		    
			//XRAY STS ---------------------------------------------------------------
			D501 = ModuleXRAYM.gXraymVolt;           //管電圧ﾌｨｰﾄﾞﾊﾞｯｸ
			D502 = ModuleXRAYM.gXraymAmp;            //管電流ﾌｨｰﾄﾞﾊﾞｯｸ
			M502 = ModuleXRAYM.gXraymOn;             //X線ON
			M504 = ModuleXRAYM.gXraymInterlockErr;   //ｲﾝﾀｰﾛｯｸｴﾗｰ
			M518 = ModuleXRAYM.gXraymUnitErr;        //X線制御機ｴﾗｰ
		    
			//==========================================================
			//Ｘ線
		    
			//管電圧Up
			M121 = ifX_Volt_Up;
			ifX_Volt_Up = 0;
			if (M124 != 0)
			{
				M122 = 0;
			}
			else
			{
				M122 = 1;
			}
			mLogic.iOnDelay(M122, ref T102, UpDownRate, ref M123);
			if (M121 * M123 != 0)
			{
				M124 = 1;
			}
			else
			{
				M124 = 0;
			}
			if (M124 != 0)
			{
				if (D101 < D108)
				{
					D101 = D101 + 1;
				}
				else
				{
					D101 = D108;
				}
			}
			if (D101 > D108)
			{
				D101 = D108;
			}

			//管電圧Down
			M125 = ifX_Volt_Down;
			ifX_Volt_Down = 0;
			if (M128 != 0)
			{
				M126 = 0;
			}
			else
			{
				M126 = 1;
			}
			mLogic.iOnDelay(M126, ref T103, UpDownRate, ref M127);
			if (M125 * M127 != 0)
			{
				M128 = 1;
			}
			else
			{
				M128 = 0;
			}
			if (M128 != 0)
			{
				if (D101 > D110)
				{
					D101 = D101 - 1;
				}
				else
				{
					D101 = D110;
				}
			}
			if (D101 < D110)
			{
				D101 = D110;
			}
		    
			//管電流Up
			M129 = ifX_Amp_Up;
			ifX_Amp_Up = 0;
			if (M132 != 0)
			{
				M130 = 0;
			}
			else
			{
				M130 = 1;
			}
			mLogic.iOnDelay(M130, ref T104, UpDownRate, ref M131);
			if (M129 * M131 != 0)
			{
				M132 = 1;
			}
			else
			{
				M132 = 0;
			}
			if (M132 != 0)
			{
				if (D102 < D109)
				{
					D102 = D102 + 1;
				}
				else
				{
					D102 = D109;
				}
			}
			if (D102 > D109)
			{
				D102 = D109;
			}
		    
			//管電流Down
			M133 = ifX_Amp_Down;
			ifX_Amp_Down = 0;
			if (M136 != 0)
			{
				M134 = 0;
			}
			else
			{
				M134 = 1;
			}
			mLogic.iOnDelay(M134, ref T105, UpDownRate, ref M135);
			if (M133 * M135 != 0)
			{
				M136 = 1;
			}
			else
			{
				M136 = 0;
			}
			if (M136 != 0)
			{
				if (D102 > D111)
				{
					D102 = D102 - 1;
				}
				else
				{
					D102 = D111;
				}
			}
			if (D102 < D111)
			{
				D102 = D111;
			}
		    
			if (M106 != 0)
			{
				M107 = 1;
			}
			else
			{
				M107 = 0;
			}
			if (M108 != 0)
			{
				M157 = 1;
			}
			else
			{
				M157 = 0;
			}
		    
			if (M157 + M138 + M180 != 0)
			{
				M139 = 1;
			}
			else
			{
				M139 = 0;
			}
			if (M121 + M125 + M129 + M133 != 0)
			{
				M115 = 1;
			}
			else
			{
				M115 = 0;
			}
			if (M115 != 0)
			{
				M116 = 0;
			}
			else
			{
				M116 = 1;
			}
			if (D103 > 600)
			{
				D103 = 600;
			}
			if (D103 < 0)
			{
				D103 = 0;
			}
		    
			if (M120 != 0)
			{
				M148 = 1;
			}
			else
			{
				M148 = 0;
			}
			mLogic.iOnDelay(M148, ref T101, (long)(D103 * 1000), ref M117);
			if (M137 != 0)
			{
				M149 = 1;
			}
			else
			{
				M149 = 0;
			}
		    
			if (M116 * M148 * M149 != 0)
			{
				M176 = 1;
			}
			else
			{
				M176 = 0;
			}
			mLogic.iOnDelay(M176, ref T108, (long)(D114 * 1000), ref M177);
		    
			if (M109 * M149 * M117 != 0)
			{
				M118 = 1;
			}
			else
			{
				M118 = 0;
			}
		    
			if (M118 != 0)
			{
				M151 = 1;
			}
			else
			{
				M151 = 0;
			}
			if (M150 != 0)
			{
				M178 = 0;
			}
			else if (M177 != 0)
			{
//2000-08-30 T.Shibui
//				M178 = 1
			}

			if (M107 + M139 + M151 + M178 != 0)
			{
				M119 = 1;
			}
			else
			{
				M119 = 0;
			}

			if (M119 != 0)
			{
				M120 = 0;
			}
			else if (M105 + M179 != 0)
			{
				M120 = 1;
			}
			if (M151 + M178 != 0)
			{
				M152 = 1;
			}
			else
			{
				M152 = 0;
			}
		    
			//ｽﾀﾝﾊﾞｲ異常
//			If M503 Then
//				M513 = 0
//			Else
//				M513 = 1
//			End If
		    
			//ｲﾝﾀﾛｯｸ異常
			if (M504 != 0)
			{
				M514 = 0;
			}
			else
			{
				M514 = 1;
			}
		    
			//ﾕﾆｯﾄ異常
			if (M518 != 0)
			{
				M519 = 0;
			}
			else
			{
				M519 = 1;
			}
		    
			//==========================================================
			//出力
			ModuleXRAYM.fXraymEmergency = M152;
			//---------------------------------------------------------

			ipXcont_Mode = M109;
			ipXtimer = (int)D103;
			ModuleXRAYM.fXraymVolt = (int)D101;
			ifX_Volt = D101;
			ModuleXRAYM.fXraymAmp = (int)D102;
			ifX_Amp = D102;
		    
			ModuleXRAYM.fXraymOn = M120;
		    
			ModuleXRAYM.fXraymOffDateRst = M137;
			ipcndVolt = ModuleXRAYM.ComkV;
			ipcndAmp = ModuleXRAYM.CommA;
		    
			ipX_Volt = D501;
			ipX_Amp = D502;
			ipX_Fine = ModuleXRAYM.gxraymFine;
			ipX_Watt = ModuleXRAYM.gXraymWatt;
			ipX_On = M502;
			ipInterlock = M514;
		    
//2000-02-25 T.Shibui
//ｳｫｰﾑｱｯﾌﾟ強制終了の時、ｳｫｰﾑｱｯﾌﾟ完了とする。
			if (M137 > 0)
			{
				ipWarmup = 2;
			}
		    
			ipXStatus = M519;
			ipXPermitWarmup = ModuleXRAYM.gXraymPermitWarmup;
		    
//2000-02-28 T.Shibui X線管ﾀｲﾌﾟは2:160KVで固定
			//ipX_type = 2; //書き方を変更_2014/10/07hata
			ipX_type = modIniFiles.XRAY_TYPE_NO_160KV;
			ipXAvail = ModuleXRAYM.gXAvail;
		    
			ipXRMaxkV = ifXRMaxkV;
			ipXRMaxmA = ifXRMaxmA;
			ipXRMinkV = ifXRMinkV;
			ipXRMinmA = ifXRMinmA;
		        
			//=======================================================
			//Err Check
			eRst = ifErrrst;
			eSts = ipErrsts;
		    
//			EmergencyErrDsp = gConmEmergencyErrDsp
		    
//			AmpErrDsp = gXraymAmpErrDsp
			InterLockErrDsp = ModuleXRAYM.gXraymInterlockErrDsp;
//			NoSelectErrDsp = gXraymNoSelectErrDsp
			OffErrDsp = ModuleXRAYM.gXraymOffErrDsp;
			OnErrDsp = ModuleXRAYM.gXraymOnErrDsp;
//			OnRyErrDsp = gXraymOnRyErrDsp
//			OverlapErrDsp = gXraymOverlapErrDsp
//			StandbyErrDsp = gXraymStandbyErrDsp
			UnitErrDsp = ModuleXRAYM.gXraymUnitErrDsp;
//			VoltErrDsp = gXraymVoltErrDsp
			TimOffErrDsp = M178;
		    
			if (eRst != 0)
			{
				if (eSts == 101)
				{
					EmergencyErrRst = 1;
				}
		        
				if (eSts == 6)
				{
					AmpErrRst = 1;
				}
				if (eSts == 4)
				{
					InterLockErrRst = 1;
				}
				if (eSts == 15)
				{
					NoSelectErrRst = 1;
				}
				if (eSts == 12)
				{
					OffErrRst = 1;
				}
				if (eSts == 11)
				{
					OnErrRst = 1;
				}
				if (eSts == 13)
				{
					OnRyErrRst = 1;
				}
				if (eSts == 16)
				{
					OverlapErrRst = 1;
				}
				if (eSts == 14)
				{
					StandbyErrRst = 1;
				}
				if (eSts == 20)
				{
					UnitErrRst = 1;
					ModuleXRAYM.ComErr = 0;
				}
				if (eSts == 5)
				{
					VoltErrRst = 1;
				}
				if (eSts == 7)
				{
					TimOffErrRst = 1;
				}
		        
				eSts = 0;
				eRst = 0;
			}
			if (eSts == 0)
			{
				if (EmergencyErrDsp > 0 && EmergencyErrRst == 0)
				{
					eSts = 101;
				}
		        
				if (AmpErrDsp > 0 && AmpErrRst == 0)
				{
					eSts = 6;
				}
				if (InterLockErrDsp > 0 && InterLockErrRst == 0)
				{
					eSts = 4;
				}
				if (NoSelectErrDsp > 0 && NoSelectErrRst == 0)
				{
					eSts = 15;
				}
				if (OffErrDsp > 0 && OffErrRst == 0)
				{
					eSts = 12;
				}
				if (OnErrDsp > 0 && OnErrRst == 0)
				{
					eSts = 11;
				}
				if (OnRyErrDsp > 0 && OnRyErrRst == 0)
				{
					eSts = 13;
				}
				if (OverlapErrDsp > 0 && OverlapErrRst == 0)
				{
					eSts = 16;
				}
				if (StandbyErrDsp > 0 && StandbyErrRst == 0)
				{
					eSts = 14;
				}
				if (UnitErrDsp > 0 && UnitErrRst == 0)
				{
					eSts = 20;
				}
				if (VoltErrDsp > 0 && VoltErrRst == 0)
				{
					eSts = 5;
				}
				if (TimOffErrDsp > 0 && TimOffErrRst == 0)
				{
					eSts = 7;
				}
			}
			if (EmergencyErrDsp == 0)
			{
				EmergencyErrRst = 0;
			}
			if (AmpErrDsp == 0)
			{
				AmpErrRst = 0;
			}
			if (InterLockErrDsp == 0)
			{
				InterLockErrRst = 0;
			}
			if (NoSelectErrDsp == 0)
			{
				NoSelectErrRst = 0;
			}
			if (OffErrDsp == 0)
			{
				OffErrRst = 0;
			}
			if (OnErrDsp == 0)
			{
				OnErrRst = 0;
			}
			if (OnRyErrDsp == 0)
			{
				OnRyErrRst = 0;
			}
			if (OverlapErrDsp == 0)
			{
				OverlapErrRst = 0;
			}
			if (StandbyErrDsp == 0)
			{
				StandbyErrRst = 0;
			}
			if (UnitErrDsp == 0)
			{
				UnitErrRst = 0;
			}
			if (VoltErrDsp == 0)
			{
				VoltErrRst = 0;
			}
			if (TimOffErrDsp == 0)
			{
				TimOffErrRst = 0;
			}
		    
//			fConmEmergencyErrRst = EmergencyErrRst
		    
			ModuleXRAYM.fXraymAmpErrRst = AmpErrRst;
			ModuleXRAYM.fXraymInterlockErrRst = InterLockErrRst;
//		    fXraymNoSelectErrRst = NoSelectErrRst
			ModuleXRAYM.fXraymOffErrRst = OffErrRst;
			ModuleXRAYM.fXraymOnErrRst = OnErrRst;
//			fXraymOnRyErrRst = OnRyErrRst
//			fXraymOverlapErrRst = OverlapErrRst
//			fXraymStandbyErrRst = StandbyErrRst
			ModuleXRAYM.fXraymUnitErrRst = UnitErrRst;
			ModuleXRAYM.fXraymVoltErrRst = VoltErrRst;
			M150 = TimOffErrRst;
		    
			ipErrsts = eSts;
			ifErrrst = eRst;
		}

		public static void CtrlInitialRead()
		{
			string Fname;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			Dim Fno     As Integer
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			int iVar;
			float sVar;
			long lVar;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Dim sVar1   As String
			Dim sVar2   As String
			Dim sCmnt   As String
			Dim dVar    As Double
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			Fname = @"C:\CT\INI\FEINFOCUS.INI";

			const char delimiter = ',';

			string[] s = null;
			StreamReader sr = null;

			try
			{
				sr = new StreamReader(Fname);

				s = sr.ReadLine().Split(delimiter);
				float.TryParse(s[1], out sVar);
				ifX_Volt = sVar;

				s = sr.ReadLine().Split(delimiter);
				float.TryParse(s[1], out sVar);
				ifX_Amp = sVar;

				s = sr.ReadLine().Split(delimiter);
				int.TryParse(s[1], out iVar);
				ifFocussize = iVar;
		    
				s = sr.ReadLine().Split(delimiter);
				int.TryParse(s[1], out iVar);
				ifXtimer = iVar;
		    
				s = sr.ReadLine().Split(delimiter);
				int.TryParse(s[1], out iVar);
				ifXcont_Mode = iVar;
		    
				s = sr.ReadLine().Split(delimiter);
				float.TryParse(s[1], out sVar);
				ifXRMaxkV = sVar;
		    
				s = sr.ReadLine().Split(delimiter);
				float.TryParse(s[1], out sVar);
				ifXRMaxmA = sVar;
		    
				s = sr.ReadLine().Split(delimiter);
				float.TryParse(s[1], out sVar);
				ifXRMinkV = sVar;
		    
				s = sr.ReadLine().Split(delimiter);
				float.TryParse(s[1], out sVar);
				ifXRMinmA = sVar;
		    
				s = sr.ReadLine().Split(delimiter);
				long.TryParse(s[1], out lVar);
				ifAvailTimXOn = lVar;
		    
				s = sr.ReadLine().Split(delimiter);
				long.TryParse(s[1], out lVar);
				ifAvailTimkVmA = lVar;
		    
//Rem Kevex用アベイラブル時間　START added by 山本　2002-8-31
		    
				s = sr.ReadLine().Split(delimiter);
				long.TryParse(s[1], out lVar);
				ifAvTm40kV30mA = lVar;
		    
				s = sr.ReadLine().Split(delimiter);
				long.TryParse(s[1], out lVar);
				ifAvTm40kV40mA = lVar;
		        
				s = sr.ReadLine().Split(delimiter);
				long.TryParse(s[1], out lVar);
				ifAvTm60kV30mA = lVar;
		    
				s = sr.ReadLine().Split(delimiter);
				long.TryParse(s[1], out lVar);
				ifAvTm60kV40mA = lVar;
		        
				s = sr.ReadLine().Split(delimiter);
				long.TryParse(s[1], out lVar);
				ifAvTm80kV30mA = lVar;
		    
				s = sr.ReadLine().Split(delimiter);
				long.TryParse(s[1], out lVar);
				ifAvTm80kV40mA = lVar;
		        
				s = sr.ReadLine().Split(delimiter);
				long.TryParse(s[1], out lVar);
				ifAvTm100kV30mA = lVar;
		    
				s = sr.ReadLine().Split(delimiter);
				long.TryParse(s[1], out lVar);
				ifAvTm100kV40mA = lVar;
		        
				s = sr.ReadLine().Split(delimiter);
				long.TryParse(s[1], out lVar);
				ifAvTm120kV30mA = lVar;
		    
				s = sr.ReadLine().Split(delimiter);
				long.TryParse(s[1], out lVar);
				ifAvTm120kV40mA = lVar;
		        
				s = sr.ReadLine().Split(delimiter);
				long.TryParse(s[1], out lVar);
				ifAvTm130kV30mA = lVar;
		    
				s = sr.ReadLine().Split(delimiter);
				long.TryParse(s[1], out lVar);
				ifAvTm130kV40mA = lVar;
		        
//Rem Kevex用アベイラブル時間　END added by 山本　2002-8-31
			}
			catch
			{
			}
			finally
			{
				if (sr != null)
				{
					sr.Close();
					sr = null;
				}
			}
		    
            //v17.65 FEINFOCUS.INIの書き込みに失敗した場合、FEINFOCUS.INIのバックアップから読み込むように変更 by 長野
            //v19.16 コメント追記by長野 2013/07/22 ⇒ 失敗したと判定する条件は、管電圧・管電流の設定値が0になってしまっている場合（通常、起こりえない）
			if ((ifX_Volt == 0) && (ifX_Amp == 0))
			{
				sr = null;

				try
				{
					Fname = @"C:\CT\INI\FEINFOCUS_Back.INI";

					sr = new StreamReader(Fname);

					s = sr.ReadLine().Split(delimiter);
					float.TryParse(s[1], out sVar);
					ifX_Volt = sVar;
				    
					s = sr.ReadLine().Split(delimiter);
					float.TryParse(s[1], out sVar);
					ifX_Amp = sVar;
				    
					s = sr.ReadLine().Split(delimiter);
					int.TryParse(s[1], out iVar);
					ifFocussize = iVar;
				    
					s = sr.ReadLine().Split(delimiter);
					int.TryParse(s[1], out iVar);
					ifXtimer = iVar;
				    
					s = sr.ReadLine().Split(delimiter);
					int.TryParse(s[1], out iVar);
					ifXcont_Mode = iVar;
				    
					s = sr.ReadLine().Split(delimiter);
					float.TryParse(s[1], out sVar);
					ifXRMaxkV = sVar;
				    
					s = sr.ReadLine().Split(delimiter);
					float.TryParse(s[1], out sVar);
					ifXRMaxmA = sVar;
				    
					s = sr.ReadLine().Split(delimiter);
					float.TryParse(s[1], out sVar);
					ifXRMinkV = sVar;
				    
					s = sr.ReadLine().Split(delimiter);
					float.TryParse(s[1], out sVar);
					ifXRMinmA = sVar;
				    
					s = sr.ReadLine().Split(delimiter);
					long.TryParse(s[1], out lVar);
					ifAvailTimXOn = lVar;
				    
					s = sr.ReadLine().Split(delimiter);
					long.TryParse(s[1], out lVar);
					ifAvailTimkVmA = lVar;
				    
					s = sr.ReadLine().Split(delimiter);
					long.TryParse(s[1], out lVar);
					ifAvTm40kV30mA = lVar;
				    
					s = sr.ReadLine().Split(delimiter);
					long.TryParse(s[1], out lVar);
					ifAvTm40kV40mA = lVar;
				        
					s = sr.ReadLine().Split(delimiter);
					long.TryParse(s[1], out lVar);
					ifAvTm60kV30mA = lVar;
				    
					s = sr.ReadLine().Split(delimiter);
					long.TryParse(s[1], out lVar);
					ifAvTm60kV40mA = lVar;
				        
					s = sr.ReadLine().Split(delimiter);
					long.TryParse(s[1], out lVar);
					ifAvTm80kV30mA = lVar;
				    
					s = sr.ReadLine().Split(delimiter);
					long.TryParse(s[1], out lVar);
					ifAvTm80kV40mA = lVar;
				        
					s = sr.ReadLine().Split(delimiter);
					long.TryParse(s[1], out lVar);
					ifAvTm100kV30mA = lVar;
				    
					s = sr.ReadLine().Split(delimiter);
					long.TryParse(s[1], out lVar);
					ifAvTm100kV40mA = lVar;
				        
					s = sr.ReadLine().Split(delimiter);
					long.TryParse(s[1], out lVar);
					ifAvTm120kV30mA = lVar;
				    
					s = sr.ReadLine().Split(delimiter);
					long.TryParse(s[1], out lVar);
					ifAvTm120kV40mA = lVar;
				        
					s = sr.ReadLine().Split(delimiter);
					long.TryParse(s[1], out lVar);
					ifAvTm130kV30mA = lVar;
				    
					s = sr.ReadLine().Split(delimiter);
					long.TryParse(s[1], out lVar);
					ifAvTm130kV40mA = lVar;
				    

                    //v19.16 FeinFocus.iniが破損した場合、上書きができないため、
                    //FeinFocus.iniを削除し、ソフト終了時に新規作成とする by長野 2013/07/22

					FileInfo cFileInfo = new FileInfo(@"C:\CT\INI\FEINFOCUS.INI");

					if (cFileInfo.Exists)
					{
						// 読み取り専用属性がある場合は、読み取り専用属性を解除する
						if ((cFileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
						{
							cFileInfo.Attributes = FileAttributes.Normal;
						}

						cFileInfo.Delete();
					}
				}
				finally
				{
					if (sr != null) sr.Close();
				}
			}
		}

		public static void CtrlInitialWrite()
		{
			string Fname;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Dim Fno     As Integer
			Dim iVar    As Integer
			Dim lVar    As Long
			Dim sVar1   As String
			Dim sVar2   As String
			Dim sCmnt   As String
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			Fname = @"C:\CT\INI\FEINFOCUS.INI";

			StreamWriter sw = null;

			try
			{
				StringBuilder sb = new StringBuilder();

				sb.AppendLine(string.Format("\"{0}\",{1}", "Volt", ifX_Volt));
				sb.AppendLine(string.Format("\"{0}\",{1}", "Amp", ifX_Amp));
				sb.AppendLine(string.Format("\"{0}\",{1}", "Focus 0:S 1:L", ipFocussize));
				sb.AppendLine(string.Format("\"{0}\",{1}", "Xtimer", ipXtimer));
				sb.AppendLine(string.Format("\"{0}\",{1}", "XcontMode", ipXcont_Mode));
				sb.AppendLine(string.Format("\"{0}\",{1}", "XRMaxkV", ifXRMaxkV));
				sb.AppendLine(string.Format("\"{0}\",{1}", "XRMaxmA", ifXRMaxmA));
				sb.AppendLine(string.Format("\"{0}\",{1}", "XRMinkV", ipXRMinkV));
				sb.AppendLine(string.Format("\"{0}\",{1}", "XRMinmA", ipXRMinmA));
				sb.AppendLine(string.Format("\"{0}\",{1}", "AvailTimXon", ifAvailTimXOn));
				sb.AppendLine(string.Format("\"{0}\",{1}", "AvailTimkVmA", ifAvailTimkVmA));

//Rem Kevex用アベイラブル時間　START added by 山本　2002-8-31

				sb.AppendLine(string.Format("\"{0}\",{1}", "AvTm40kV30mA", ifAvTm40kV30mA));
				sb.AppendLine(string.Format("\"{0}\",{1}", "AvTm40kV40mA", ifAvTm40kV40mA));
				sb.AppendLine(string.Format("\"{0}\",{1}", "AvTm60kV30mA", ifAvTm60kV30mA));
				sb.AppendLine(string.Format("\"{0}\",{1}", "AvTm60kV40mA", ifAvTm60kV40mA));
				sb.AppendLine(string.Format("\"{0}\",{1}", "AvTm80kV30mA", ifAvTm80kV30mA));
				sb.AppendLine(string.Format("\"{0}\",{1}", "AvTm80kV40mA", ifAvTm80kV40mA));
				sb.AppendLine(string.Format("\"{0}\",{1}", "AvTm100kV30mA", ifAvTm100kV40mA));
				sb.AppendLine(string.Format("\"{0}\",{1}", "AvTm100kV40mA", ifAvTm100kV40mA));
				sb.AppendLine(string.Format("\"{0}\",{1}", "AvTm120kV30mA", ifAvTm120kV30mA));
				sb.AppendLine(string.Format("\"{0}\",{1}", "AvTm120kV40mA", ifAvTm120kV40mA));
				sb.AppendLine(string.Format("\"{0}\",{1}", "AvTm130kV30mA", ifAvTm130kV30mA));
				sb.AppendLine(string.Format("\"{0}\",{1}", "AvTm130kV40mA", ifAvTm130kV40mA));

//Rem Kevex用アベイラブル時間　END added by 山本　2002-8-31

				sw = new StreamWriter(Fname);
				sw.Write(sb);
			}
			finally
			{
				if (sw != null)
				{
					sw.Close();
				}
			}
		}
	}
}
