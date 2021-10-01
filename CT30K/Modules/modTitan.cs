
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CT30K
{
	static class modTitan
	{

		//ウォームアップ中   'v18.00追加 byやまおか 2011/03/07
		public static bool Flg_TiWarmingUp;

        //Public Type XRAYTITAINFO
        //    MaxkV As Long
        //    MaxuA As Long
        //    MinkV As Long
        //    MinuA As Long
        //    MaxPW As Long
        //    SetpointV As Long
        //    SetpointuA As Long
        //    ActualV As Long
        //    ActualuA As Long
        //    Actualfocus As Long
        //    Wutime As Long
        //    Status01 As Long
        //    Status06 As Long
        //    Status30 As Long
        //    Status12 As Long
        //    RTCDD As Long
        //    RTCMM As Long
        //    RTCYY As Long
        //    RTChh As Long
        //    RTCff As Long
        //    RTCss As Long
        //    CommStatus As Boolean
        //    CommErr As Long
        //    SendData(511) As Byte
        //    RecData(1023) As Byte
        //End Type

		public enum WarmupConstants
		{
			WU_OFF_OVER,
			WU_OFF_2WEEK,
			WU_OFF_2DAY,
			WU_OFF_1DAY,
			WU_OFF_6HOUR,
			WU_OFF_4HOUR,
			WU_OFF_2HOUR,
			WU_READY
		}

        //Declare Function TitanOpen Lib "Titan" (ByRef lpInfo As XRAYTITAINFO) As Long
        //'Declare Function TitanOpen Lib "Titan" (ByVal CallBackAddress As Long, ByRef lpInfo As XRAYTITAINFO) As Long
        //Declare Function TitanClose Lib "Titan" () As Long
        //Declare Function SetTitanOn Lib "Titan" (ByVal OnOff As Long) As Long
        //Declare Function SetTitanReset Lib "Titan" () As Long
        //Declare Function SetTitanWUp0 Lib "Titan" () As Long
        //Declare Function SetTitanWUpX Lib "Titan" (ByVal wumode As Long, ByVal WUkV As Long) As Long
        //Declare Function SetTitanClErr49 Lib "Titan" () As Long
        //Declare Function SetTitanV Lib "Titan" (ByVal VValue As Long) As Long
        //Declare Function SetTitanuA Lib "Titan" (ByVal uAValue As Long) As Long
        //Declare Function SetTitanFocus Lib "Titan" (ByVal FocusSize As Long) As Long
        //Declare Function SetTitanIMode Lib "Titan" () As Long
        //Declare Function SetTitanPMode Lib "Titan" () As Long
        //Declare Function SetTitanManual Lib "Titan" () As Long
        //Declare Function SetTitanKBLock Lib "Titan" (ByVal KBLock As Long) As Long
        //Declare Function SetTitanRTC Lib "Titan" (ByVal SetDD As Long, _
        //'                                          ByVal SetMM As Long, _
        //'                                          ByVal SetYY As Long, _
        //'                                          ByVal Sethh As Long, _
        //'                                          ByVal Setff As Long, _
        //'                                          ByVal Setss As Long) As Long


#if (!DebugOn)

        //VBで使用するTitan関数  'v18.00変更 byやまおか 2011/03/01
		[DllImport("Titan.dll", EntryPoint = "XrayOn", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_XrayOn();
		
        [DllImport("Titan.dll", EntryPoint = "XrayOff", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_XrayOff();
		
        [DllImport("Titan.dll", EntryPoint = "CheckWarmUpStatus", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//public static extern WarmupConstants Ti_CheckWarmUpStatus(long kv);
        public static extern WarmupConstants Ti_CheckWarmUpStatus(int kv);
		
        [DllImport("Titan.dll", EntryPoint = "SetXrayVoltage", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int Ti_SetXrayVoltage(long kv);
        public static extern int Ti_SetXrayVoltage(int kv);
       
        //Declare Function Ti_StartWarmUp Lib "Titan.dll" Alias "StartWarmUp" (ByVal wumode As WarmupConstants, ByVal MyCallback As Long) As Long
        [DllImport("Titan.dll", EntryPoint = "StartWarmUp", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int Ti_StartWarmUp(long wukv, WarmupConstants wumode, long MyCallback);//v18.00 wukv追加 byやまおか 2011/03/05
        //public static extern int Ti_StartWarmUp(int wukv, WarmupConstants wumode, int MyCallback);//v18.00 wukv追加 byやまおか 2011/03/05
		public static extern int Ti_StartWarmUp(int wukv, WarmupConstants wumode, TitanWuCallbackDelegate  MyCallback);//v18.00 wukv追加 byやまおか 2011/03/05

        [DllImport("Titan.dll", EntryPoint = "UpdateXoffcsv", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int Ti_UpdateXoffcsv();
		[DllImport("Titan.dll", EntryPoint = "GetXrayOffTime", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//public static extern int Ti_GetXrayOffTime(long kv);
        public static extern int Ti_GetXrayOffTime(int kv);
		
        [DllImport("Titan.dll", EntryPoint = "SetXrayVoltage_UnChk", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int Ti_SetXrayVoltage_UnChk(long kv);
        public static extern int Ti_SetXrayVoltage_UnChk(int kv);
		
        [DllImport("Titan.dll", EntryPoint = "SetXrayCurrent", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_SetXrayCurrent(int kv);
		[DllImport("Titan.dll", EntryPoint = "Open", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int Ti_Open();
		
        [DllImport("Titan.dll", EntryPoint = "Close", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_Close();
		[DllImport("Titan.dll", EntryPoint = "ComClose", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_ComClose();
		
        //Declare Function Ti_GetSetpointVoltage Lib "Titan.dll" Alias "GetSetpointVoltage" () As Long
        //Declare Function Ti_GetSetpointCurrent Lib "Titan.dll" Alias "GetSetpointCurrent" () As Long
        //Declare Function Ti_GetAcutualVoltage Lib "Titan.dll" Alias "GetAcutualVoltage" () As Long
        //Declare Function Ti_GetAcutualCurrent Lib "Titan.dll" Alias "GetAcutualCurrent" () As Long

        [DllImport("Titan.dll", EntryPoint = "GetTargetVoltage", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern double Ti_GetTargetVoltage();
		[DllImport("Titan.dll", EntryPoint = "GetTargetCurrent", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_GetTargetCurrent();
		[DllImport("Titan.dll", EntryPoint = "GetAcutualVoltage", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern double Ti_GetAcutualVoltage();
		[DllImport("Titan.dll", EntryPoint = "GetAcutualCurrent", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_GetAcutualCurrent();
		[DllImport("Titan.dll", EntryPoint = "GetMaxMinVoltage", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int Ti_GetMaxMinVoltage(ref long Max, ref long Min);
        public static extern int Ti_GetMaxMinVoltage(ref int Max, ref int Min);

		[DllImport("Titan.dll", EntryPoint = "GetMaxMinCurrent", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int Ti_GetMaxMinCurrent(ref long Max, ref long Min);
        public static extern int Ti_GetMaxMinCurrent(ref int Max, ref int Min);

		[DllImport("Titan.dll", EntryPoint = "GetMaxOpVoltage", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int Ti_GetMaxOpVoltage(ref long Max);		//v19.50 追加 by長野 2013/12/19
        public static extern int Ti_GetMaxOpVoltage(ref int Max);		//v19.50 追加 by長野 2013/12/19
		
        [DllImport("Titan.dll", EntryPoint = "GetRtcTime", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int Ti_GetRtcTime();
		[DllImport("Titan.dll", EntryPoint = "GetRtcYear", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_GetRtcYear();
		[DllImport("Titan.dll", EntryPoint = "GetRtcMonth", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_GetRtcMonth();
		[DllImport("Titan.dll", EntryPoint = "GetRtcDay", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_GetRtcDay();
		[DllImport("Titan.dll", EntryPoint = "GetRtcHour", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_GetRtcHour();
		[DllImport("Titan.dll", EntryPoint = "GetRtcMinute", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_GetRtcMinute();
		[DllImport("Titan.dll", EntryPoint = "GetRtcSecond", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_GetRtcSecond();

        [DllImport("Titan.dll", EntryPoint = "CheckAvailabled", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_CheckAvailabled();
		[DllImport("Titan.dll", EntryPoint = "CheckXrayON", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_CheckXrayON();
		[DllImport("Titan.dll", EntryPoint = "GetFocusSize", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_GetFocusSize();
		[DllImport("Titan.dll", EntryPoint = "SetFocusSize", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int Ti_SetFocusSize(long Focus);
        public static extern int Ti_SetFocusSize(int Focus);
		
        [DllImport("Titan.dll", EntryPoint = "CheckXrayReady", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_CheckXrayReady();
		[DllImport("Titan.dll", EntryPoint = "SetTitanKBLock", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int Ti_SetTitanKBLock(long KbUnLock);
        public static extern int Ti_SetTitanKBLock(int KbUnLock);

		[DllImport("Titan.dll", EntryPoint = "SetTitanManual", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_SetTitanManual();
		[DllImport("Titan.dll", EntryPoint = "UpdateWarmupRestSec", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_UpdateWarmupRestSec();
		[DllImport("Titan.dll", EntryPoint = "SetTitanReset", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_SetTitanReset();
		[DllImport("Titan.dll", EntryPoint = "GetErrorCode", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Ti_GetErrorCode();

        //Rev23.20 追加 by長野 2016/01/11
        [DllImport("Titan.dll", EntryPoint = "GetCommErrorCode", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int Ti_GetCommErrorCode();
        
        [DllImport("Titan.dll", EntryPoint = "UpdateWarmupMode", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int Ti_UpdateWarmupMode(WarmupConstants Mode, long kv);
        public static extern int Ti_UpdateWarmupMode(WarmupConstants Mode, int kv);

		[DllImport("Titan.dll", EntryPoint = "GetWarmupRestSec", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int Ti_GetWarmupRestSec(long kv);
        public static extern int Ti_GetWarmupRestSec(int kv);

		[DllImport("Titan.dll", EntryPoint = "SetWarmingupFlag", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int Ti_SetWarmingupFlag(long flag);		//v18.00追加 byやまおか 2011/09/05
        public static extern int Ti_SetWarmingupFlag(int flag);		//v18.00追加 byやまおか 2011/09/05

        //Rev23.20 任意の管電圧とOFF時間で、X線OFF管理ファイルを上書きする。 追加 by長野 2015/01/19
        [DllImport("Titan.dll", EntryPoint = "SetXOffTime", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int Ti_SetXOffTime(long kv,int OffTimeY,int OffTimeMON,int OffTimeD,int OffTimeH,int OffTimeMIN);

#else

        //デバグ用ダミー関数     'v18.00追加 byやまおか 2011/04/27
		public static int Ti_XrayOn()
		{
            return 0;
		}

		public static int Ti_XrayOff()
		{
            return 0;
        }

		public static WarmupConstants Ti_CheckWarmUpStatus(int kv)
		{
            return 0;
        }

		public static int Ti_SetXrayVoltage(int kv)
		{
            return 0;
        }

		//public static int Ti_StartWarmUp(int wukv, WarmupConstants wumode, int MyCallback)
        public static int Ti_StartWarmUp(int wukv, WarmupConstants wumode, TitanWuCallbackDelegate MyCallback)
        {
            return 0;
        }

		public static int Ti_UpdateXoffcsv()
		{
            return 0;
        }

		public static int Ti_GetXrayOffTime(int kv)
		{
            return 0;
        }

		public static int Ti_SetXrayVoltage_UnChk(int kv)
		{
            return 0;
        }

		public static int Ti_SetXrayCurrent(short kv)
		{
            return 0;
        }

		public static int Ti_Open()
		{
            return 0;
        }

		public static int Ti_Close()
		{
            return 0;
        }

		public static int Ti_ComClose()
		{
            //v18.00追加 byやまおか 2011/06/29
            return 0;
		}

		public static double Ti_GetTargetVoltage()
		{
            return 0;
        }

		public static int Ti_GetTargetCurrent()
		{
            return 0;
        }

		public static double Ti_GetAcutualVoltage()
		{
            return 0;
        }

		public static int Ti_GetAcutualCurrent()
		{
            return 0;
        }

		public static int Ti_GetMaxMinVoltage(ref int Max, ref int Min)
		{
            return 0;
        }

		public static int Ti_GetMaxOpVoltage(ref int Max)
		{
			//v19.50 追加 by長野 2013/12/19
            return 0;
        }

		public static int Ti_GetMaxMinCurrent(ref int Max, ref int Min)
		{
            return 0;
        }

		public static int Ti_GetRtcTime()
		{
            return 0;
        }

		public static int Ti_GetRtcYear()
		{
            return 0;
        }

		public static int Ti_GetRtcMonth()
		{
            return 0;
        }

		public static int Ti_GetRtcDay()
		{
            return 0;
        }

		public static int Ti_GetRtcHour()
		{
            return 0;
        }

		public static int Ti_GetRtcMinute()
		{
            return 0;
        }

		public static int Ti_GetRtcSecond()
		{
            return 0;
        }

		public static int Ti_CheckAvailabled()
		{
            return 0;
        }

		public static int Ti_CheckXrayON()
		{
            return 0;
        }

		public static int Ti_GetFocusSize()
		{
            return 0;
        }

		public static int Ti_SetFocusSize(int Focus)
		{
            return 0;
        }

		public static int Ti_CheckXrayReady()
		{
            return 0;
        }

		public static int Ti_SetTitanKBLock(int KbUnLock)
		{
            return 0;
        }

		public static int Ti_SetTitanManual()
		{
            return 0;
        }

		public static int Ti_UpdateWarmupRestSec()
		{
            return 0;
        }

		public static int Ti_SetTitanReset()
		{
            return 0;
        }

		public static int Ti_GetErrorCode()
		{
            return 0;
        }

        public static int Ti_GetCommErrorCode() //Rev23.20 追加 by長野 2016/01/12
        {
            return 0;
        }

		public static int Ti_UpdateWarmupMode(WarmupConstants Mode, int kv)
		{
            return 0;
        }

		public static int Ti_GetWarmupRestSec(int kv)
		{
            return 0;
        }

		public static int Ti_SetWarmingupFlag(int flag)
		{
			//v18.00追加 byやまおか 2011/09/05
            return 0;
        }

        //Rev23.20 追加 by長野 2016/01/19
        public static int Ti_SetXOffTime(long kv,int OffTimeY,int OffTimeMON,int OffTimeD,int OffTimeH,int OffTimeMIN)
        {
            return 0;
        }
#endif

        //
        // TitanWuCallback コールバック関数のデリゲート
        //
        public delegate void TitanWuCallbackDelegate(int parm);


        //コールバック関数   'v18.00変更 byやまおか 2011/03/02
		static int static_WuCallback_CBcount;
		
        
        
        public static void WuCallback(int Param)
		{
			int SkipCount = 0;

			try
            {

                //間引き間隔
                SkipCount = Convert.ToInt32(1);

                //Rev23.40 変更 by長野 2016/06/19
                //System.Windows.Forms.Application.DoEvents();

                //SkipCount回に1回だけUpdateする
                if ((static_WuCallback_CBcount % SkipCount) == 0)
                {
                    //frmXrayControl.Instance.MyUpdate();
                    //Rev24.00 変更 by長野 2016/05/18
                    modCT30K.PauseForDoEvents(0.1f);
                    static_WuCallback_CBcount = 0;
                }
    			static_WuCallback_CBcount = static_WuCallback_CBcount + 1;			//カウント インクリメント
            
            }
            catch
            { 
            }

		}

        //焦点サイズを取得し、焦点ボタンとmecainfを更新する関数  'v18.00追加 byやまおか 2011/07/09
		//public static int Get_Ti_GetFocusSize()
        public static void Get_Ti_GetFocusSize()
		{

			int focussize = 0;

			//焦点サイズを取得
			focussize = Ti_GetFocusSize();

			//mecainf（コモン）取得
			//modMecainf.GetMecainf(ref modMecainf.mecainf);
            CTSettings.mecainf.Load();

            //mecainfの焦点を変更
			//modMecainf.mecainf.xfocus = focussize;
            //modMecainf.mecainf.xfocus = focussize;
            //Rev23.40 異なるときだけ書き込む by長野 2016/06/19
            if (CTSettings.mecainf.Data.xfocus != focussize)
            {
                CTSettings.mecainf.Data.xfocus = focussize;

                //mecainf（コモン）更新
                //modMecainf.PutMecainf(ref modMecainf.mecainf);
                CTSettings.mecainf.Write();
            }

            //mecainf（コモン）更新
			//modMecainf.PutMecainf(ref modMecainf.mecainf);
            CTSettings.mecainf.Write();

			//焦点ボタンを更新
			//modLibrary.SetCmdButton(frmXrayControl.Instance.cmdFocus, focussize + 1, , , true);
			//modLibrary.SetCmdButton(frmXrayControl.Instance.cmdFocus, focussize + 1, true);
            //Rev23.20 変更 by長野 2015/12/23
            modLibrary.SetCmdButton(frmXrayControl.Instance.cmdFocus, focussize, true);
		}

        //Rev23.20 追加 XOFF時に、シーケンサに400以上or430以上のX線をだしたかどうか送信する(ON中は周期的に送信)
        public static void SendSeq_Ti_XoffVolt()
        {
            long Volt = (long)modXrayControl.XrayVoltSet();
            if (Volt >= 430)
            {
                modSeqComm.MySeq.BitWrite("stsXrayHOff",true);
                modSeqComm.MySeq.BitWrite("stsXrayMOff", true);
            }
            else if (Volt >= 400 && Volt < 430)
            {
                modSeqComm.MySeq.BitWrite("stsXrayHOff", false);
                modSeqComm.MySeq.BitWrite("stsXrayMOff", true);
            }
            else
            {
                modSeqComm.MySeq.BitWrite("stsXrayHOff", false);
                modSeqComm.MySeq.BitWrite("stsXrayMOff", false);
            }
        }
        //Rev23.20 追加 XOFF時に、シーケンサに400以上or430以上のX線をだしたかどうか送信する(ON中は周期的に送信)
        public static void SendSeq_Ti_XoffVolt_ByValue(int Volt)
        {
            if (Volt >= 430)
            {
                modSeqComm.MySeq.BitWrite("stsXrayHOff", true);
                modSeqComm.MySeq.BitWrite("stsXrayMOff", true);
            }
            else if (Volt >= 400 && Volt < 430)
            {
                modSeqComm.MySeq.BitWrite("stsXrayHOff", false);
                modSeqComm.MySeq.BitWrite("stsXrayMOff", true);
            }
            else
            {
                modSeqComm.MySeq.BitWrite("stsXrayHOff", false);
                modSeqComm.MySeq.BitWrite("stsXrayMOff", false);
            }
        }
    }
}
