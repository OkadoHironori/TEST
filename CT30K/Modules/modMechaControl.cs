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
	public static class modMechaControl
	{
		//ＰＩＯボードデバイスハンドル格納先ポインタ
		private static int hPioID1 = 0;
        
        //メカ動作種別定数
		public enum MechaConstants
		{
			MechaTableRotate,
			MechaTableUpDown,
            MechaTableX,
            MechaTableY,
            MechaFTableX,
            MechaFTableY,
            MechaII,
            MechaXrayRotate,
            MechaXrayX,
            MechaXrayY,
            MechaTiltAndRot_Rot, //Rev22.00 追加 by長野 2015/08/20
            MechaTiltAndRot_Tilt //Rev22.00 追加 by長野 2015/08/20
        }
        
        //外観カメラホームポジション Rev26.20 add by chouno 2019/02/12
        public static int FixedCamType { get; private set; }
        public static float HomeFCD { get; private set; }
        public static float HomeTableY { get; private set; }
        public static float HomeTableZ { get; private set; }
        //外観カメラ移動前の位置
        public static float BackUpFCD { get; private set; }
        public static float BackUpFDD { get; private set; }
        public static float BackUpTableY { get; private set; }
        public static float BackUpTableZ { get; private set; }
        public static float BackUpRot { get; private set; }

        //メカ準備の設定値保持用
        public static float GVal_TableRotateSpeed;	//テーブル回転速度
        public static float GVal_TableUpDownSpeed;	//テーブル昇降速度
        public static float GVal_FineTableSpeed;	//微調ﾃｰﾌﾞﾙ速度(mm/sec) 'V5.0 append by 山本 2001/07/31
        public static float GVal_TiltAndRot_RotSpeed;  //回転傾斜テーブル 回転速度 追加 by長野 2015/08/20
        public static float GVal_TiltAdnRot_TiltSpeed; //回転傾斜テーブル 傾斜速度 追加 by長野 2015/08/20

        //シーケンサから受け取った各軸の数値をmmに変換するときの係数 //Rev23.10 追加 by長野 2015/09/18
        public static int   GVal_FCD_SeqMagnify;    //FCD軸
        public static int   GVal_TableX_SeqMagnify; //Y軸
        public static int   GVal_FDD_SeqMagnify;    //FDD軸
        public static int   GVal_Rot_SeqMagnify;    //FDD軸
        public static int   GVal_Ud_SeqMagnify;     //FDD軸

        
        //追加2014/10/07hata_v19.51反映 
        //ソフト起動後に回転リセットしたかどうかを判定するフラグ
         public static bool Flg_StartupRotReset;    //True:した False:まだ   'v18.00追加 byやまおか 2011/02/20 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07      
        //ソフト起動時に昇降リセットしたかどうかを判定するフラグ
        public static bool Flg_StartupUpDownReset;  //True:した False:まだ   'v19.51 追加 by長野 2014/02/27      
        //v19.50 シーケンサ動作軸に関して更新するかどうかのフラグ by長野 2013/12/17
        public static bool Flg_SeqCommUpdate;
        //v19.50 メカコントロールボードで動作する軸に関して更新するかどうかのフラグ 2013/12/17
        public static bool Flg_MechaControlUpdate;

        //
		// MyCallbackCaptureメソッドで使用する static フィールド
		//
		private static int MyCallbackCapture_CBcount;

		//
		// MyCallback コールバック関数のデリゲート
		//
		public delegate void MyCallbackDelegate(int parm);
 		
        //
		// MyCallbackCapture コールバック関数のデリゲート
		//
        public delegate void MyCallbackCaptureDelegate(int parm);

 //
//   デバッグ用
//		
#if DebugOn
		public static int MechaDevOpen(ref int hDevID)
		{
            return 0;
		}

		public static int MechaDevClose(int hDevID)
		{
            return 0;
		}

		public static int Mechastatus_check(int hDevID)
		{
            return 0;
		}

		public static int Mechaerror_reset(int hDevID)
		{
            return 0;
		}

		public static int RotateInit(int hDevID)
		{
            return 0;
		}

		public static int RotateManual(int hDevID, int RotDir, float RotManuSpeed, int RotSpeedDet)
		{
            return 0;
		}

        private static int RotateIndex(int hDevID, int RotCoordi, float RotAngle, MyCallbackDelegate CallBackAddress)
		{
            return 0;
		}

        //Private Function RotateOrigin(ByVal hDevID As Long, ByVal CallBackAddress As Long) As Long
        public static int RotateOrigin(int hDevID, MyCallbackDelegate CallBackAddress)			//v17.46変更 byやまおか 2011/02/27
		{
            return 0;
		}

        private static int RotateSlowStop(int hDevID, MyCallbackDelegate CallBackAddress)
		{
            return 0;
		}

		public static int UdInit(int hDevID)
		{
            return 0;
		}

		public static int UdManual(int hDevID, int UdDir, float UdManuSpeed)
        {
            return 0;
		}

        //Private Function UdIndex(ByVal hDevID As Long, ByVal UdCoordi As Long, ByVal UdAngle As Single, ByVal CallBackAddress As Long) As Long
        private static int UdIndex(int hDevID, int UdCoordi, float UdAngle, MyCallbackCaptureDelegate CallBackCaptureAddress, MyCallbackDelegate CallBackAddress)	//v17.10変更 byやまおか 2010/07/28
		{
            return 0;		
		}

		public static int UdOrigin(int hDevID, MyCallbackDelegate CallBackAddress)
		{
            return 0;
		}

		private static int UdSlowStop(int hDevID, MyCallbackDelegate CallBackAddress)
		{
            return 0;
		}

		private static int UdFastStop(int hDevID, MyCallbackDelegate CallBackAddress)
		{
            return 0;
		}

		private static int PhmOn(MyCallbackDelegate CallBackAddress)
		{
            return 0;
		}

        private static int PhmOff(MyCallbackDelegate CallBackAddress)
		{
            return 0;
		}

		public static int PioDevOpen(ref int hDevPIO)
		{
            return 0;
		}

		public static int PioDevClose(int hDevPIO)
		{
            return 0;
		}

		public static int PioChkStart(int ChkTim)
		{
            return 0;
		}

		public static int PioChkEnd()
		{
            return 0;
		}

		public static int SwOpeStart()
		{
            return 0;
		}

		public static int SwOpeEnd()
		{
            return 0;
		}

		public static int XStgInit(int hDevID)
		{
            return 0;
		}

		private static int XStgOrigin(int hDevID, MyCallbackDelegate CallBackAddress)
		{
            return 0;
		}

		private static int XStgSlowStop(int hDevID, MyCallbackDelegate CallBackAddress)
		{
            return 0;
		}

		private static int XStgFastStop(int hDevID, MyCallbackDelegate CallBackAddress)
		{
            return 0;
		}

		public static int XStgManual(int hDevID, int XStgDir, float XStgManuSpeed)
		{
            return 0;
		}

		public static int YStgInit(int hDevID)
		{
            return 0;
		}

		private static int YStgOrigin(int hDevID, MyCallbackDelegate CallBackAddress)
		{
            return 0;
		}

		private static int YStgSlowStop(int hDevID, MyCallbackDelegate CallBackAddress)
		{
            return 0;
		}

		private static int YStgFastStop(int hDevID, MyCallbackDelegate CallBackAddress)
		{
            return 0;
		}

		public static int YStgManual(int hDevID, int YStgDir, float YStgManuSpeed)
		{
            return 0;
		}

		public static int GetRotAccel(ref float pRotAccelTm)
		{
            return 0;
		}

		public static int PioOutBit(string XrayCom, int OnOff)
		{
            return 0;
		}

		private static int XStgIndex(int hDevID, int XStgCoordi, float XStgIndexPos)
		{
            return 0;
		}

		private static int YStgIndex(int hDevID, int YStgCoordi, float YStgIndexPos)
		{
            return 0;
		}

        //v19.51 追加 X線・検出器昇降の場合もデバッグで起動させるため by長野 2014/03/03
        public static int XrayUdManual(int UdDir, float UdManuSpeed)
        {
            return 0;
        }

        public static int XrayUdOrigin(MyCallbackDelegate CallBackAddress)
        {
            return 0;
        }

        public static int DetUdManual(int UdDir, float UdManuSpeed)
        {
            return 0;
        }

        public static int DetUdOrigin(MyCallbackDelegate CallBackAddress)
        {
            return 0;
        }

        //Rev22.00 ここから追加 回転傾斜テーブル by長野 2015/08/21
        public static int TiltInit(int hDevID)
        {
            return 0;
        }
        public static int TiltManual(int hDevID, int TiltDir, float TiltManuSpeed)
        {
            return 0;
        }
        public static int TiltOrigin(int hDevID, MyCallbackDelegate CallBackAddress)
        {
            return 0;
        }
        public static int TiltSlowStop(int hDevID, MyCallbackDelegate CallBackAddress)
        {
            return 0;
        }
        public static int TiltFastStop(int hDevID, MyCallbackDelegate CallBackAddress)
        {
            return 0;
        }
        public static int TiltRotInit(int hDevID)
        {
            return 0;
        }
        public static int TiltRotManual(int hDevID, int TiltRotDir, float TiltRotManuSpeed)
        {
            return 0;
        }
        public static int TiltRotOrigin(int hDevID, MyCallbackDelegate CallBackAddress)
        {
            return 0;
        }
        public static int TiltRotSlowStop(int hDevID, MyCallbackDelegate CallBackAddress)
        {
            return 0;
        }
        public static int TiltRotFastStop(int hDevID, MyCallbackDelegate CallBackAddress)
        {
            return 0;
        }
        //Rev22.00 ここまで追加

#else
        //
        //   mechacontrol.dll（メカ制御ボード用ＤＬＬ）
        //
		[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int MechaDevOpen(ref int hDevID);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int MechaDevClose(int hDevID);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Mechastatus_check(int hDevID);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Mechaerror_reset(int hDevID);

		[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int RotateInit(int hDevID);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int RotateManual(int hDevID, int RotDir, float RotManuSpeed, int RotSpeedDet);
        
        //Declare Function RotateIndex Lib "mechacontrol.dll" (ByVal hDevID As Long, ByVal RotCoordi As Long, ByVal RotAngle As Single) As Long
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//private static extern int RotateIndex(int hDevID, int RotCoordi, float RotAngle, int CallBackAddress);
        public static extern int RotateIndex(int hDevID, int RotCoordi, float RotAngle, MyCallbackDelegate CallBackAddress);
 
        //Declare Function RotateOrigin Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long
        //Private Declare Function RotateOrigin Lib "mechacontrol.dll" (ByVal hDevID As Long, ByVal CallBackAddress As Long) As Long      'v10.0変更 by 間々田 2005/02/10
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]		
        //public static extern int RotateOrigin(int hDevID, int CallBackAddress);                                 //v17.46変更 byやまおか 2011/02/27
        public static extern int RotateOrigin(int hDevID, MyCallbackDelegate CallBackAddress);                                 //v17.46変更 byやまおか 2011/02/27
        
        //Declare Function RotateSlowStop Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//private static extern int RotateSlowStop(int hDevID, int CallBackAddress);                              //v15.0変更 by 間々田 2009/06/18
        public static extern int RotateSlowStop(int hDevID, MyCallbackDelegate CallBackAddress);                              //v15.0変更 by 間々田 2009/06/18

		[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int UdInit(int hDevID);
		
        //[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//public static extern int UdManual(int hDevID, int UdDir, float UdManuSpeed);

        //Rev23.20 変更 by長野 2015/12/21 float→doubleへ変更
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int UdManual(int hDevID, int UdDir, float UdManuSpeed);

        //Private Declare Function UdIndex Lib "mechacontrol.dll" (ByVal hDevID As Long, ByVal UdCoordi As Long, ByVal UdAngle As Single, ByVal CallBackAddress As Long) As Long
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//private static extern int UdIndex(int hDevID, int UdCoordi, float UdAngle, int CallBackAddress, int CallBackAddress);		//v17.02変更 byやまおか 2010/07/22
        public static extern int UdIndex(int hDevID, int UdCoordi, float UdAngle, MyCallbackCaptureDelegate CallBackCaptureAddress, MyCallbackDelegate CallBackAddress);		//v17.02変更 byやまおか 2010/07/22

        //Private Declare Function UdIndexWithCallBack Lib "mechacontrol.dll" (ByVal hDevID As Long, ByVal UdCoordi As Long, ByVal UdAngle As Single, ByVal CallBackAddress As Long) As Long 'v10.0追加 by 間々田 2005/02/10
        //Declare Function UdOrigin Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int UdOrigin(int hDevID, int CallBackAddress);		                                //v10.0変更 by 間々田 2005/02/10
        public static extern int UdOrigin(int hDevID, MyCallbackDelegate CallBackAddress);		                                //v10.0変更 by 間々田 2005/02/10

        //Declare Function UdSlowStop Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //private static extern int UdSlowStop(int hDevID, int CallBackAddress);		                            //v15.0変更 by 間々田 2009/06/18
        public static extern int UdSlowStop(int hDevID, MyCallbackDelegate CallBackAddress);		                            //v15.0変更 by 間々田 2009/06/18
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //private static extern int UdFastStop(int hDevID, int CallBackAddress);		                            //v15.0追加 by 間々田 2009/06/17
        public static extern int UdFastStop(int hDevID, MyCallbackDelegate CallBackAddress);		                            //v15.0追加 by 間々田 2009/06/17
        
		[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //private static extern int PhmOn(int CallBackAddress = 0);		                                        //V5.0 append by 山本 2001/07/31
        public static extern int PhmOn(MyCallbackDelegate CallBackAddress);		                                        //V5.0 append by 山本 2001/07/31
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //private static extern int PhmOff(int CallBackAddress = 0);		                                        //V5.0 append by 山本 2001/07/31
        public static extern int PhmOff(MyCallbackDelegate CallBackAddress);		                                        //V5.0 append by 山本 2001/07/31

		[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int PioDevOpen(ref int hDevPIO);
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int PioDevClose(int hDevPIO);
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int PioChkStart(int ChkTim);
		[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int PioChkEnd();
		[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int SwOpeStart();
		[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int SwOpeEnd();
		[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int XStgInit(int hDevID);		                                                    //V5.0 append by 山本 2001/07/31
        
        //Declare Function XStgOrigin Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long           'V5.0 append by 山本 2001/07/31
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//private static extern int XStgOrigin(int hDevID, int CallBackAddress);		                            //v11.5変更 by 間々田 2006/06/30
        public static extern int XStgOrigin(int hDevID, MyCallbackDelegate CallBackAddress);		                            //v11.5変更 by 間々田 2006/06/30
        
        //Declare Function XStgSlowStop Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long         'V5.0 append by 山本 2001/07/31
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //private static extern int XStgSlowStop(int hDevID, int CallBackAddress);		                        //v15.0変更 by 間々田 2009/06/18
        public static extern int XStgSlowStop(int hDevID, MyCallbackDelegate CallBackAddress);		                        //v15.0変更 by 間々田 2009/06/18
        
        //Declare Function XStgFastStop Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long         'V5.0 append by 山本 2001/07/31
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//private static extern int XStgFastStop(int hDevID, int CallBackAddress);		                        //v15.0変更 by 間々田 2009/06/18
        public static extern int XStgFastStop(int hDevID, MyCallbackDelegate CallBackAddress);		                        //v15.0変更 by 間々田 2009/06/18
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int XStgManual(int hDevID, int XStgDir, float XStgManuSpeed);		                //V5.0 append by 山本 2001/07/31

		[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int YStgInit(int hDevID);		                                                    //V5.0 append by 山本 2001/07/31
        
        //Declare Function YStgOrigin Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long           'V5.0 append by 山本 2001/07/31
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //private static extern int YStgOrigin(int hDevID, int CallBackAddress);		                        //v11.5変更 by 間々田 2006/06/30
        public static extern int YStgOrigin(int hDevID, MyCallbackDelegate CallBackAddress);		                        //v11.5変更 by 間々田 2006/06/30
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //Declare Function YStgSlowStop Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long         'V5.0 append by 山本 2001/07/31
		//private static extern int YStgSlowStop(int hDevID, int CallBackAddress);                    		//v15.0変更 by 間々田 2009/06/18
        public static extern int YStgSlowStop(int hDevID, MyCallbackDelegate CallBackAddress);                    		//v15.0変更 by 間々田 2009/06/18
        
        //Declare Function YStgFastStop Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long         'V5.0 append by 山本 2001/07/31
        
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //private static extern int YStgFastStop(int hDevID, int CallBackAddress);		                    //v15.0変更 by 間々田 2009/06/18
        public static extern int YStgFastStop(int hDevID, MyCallbackDelegate CallBackAddress);		                    //v15.0変更 by 間々田 2009/06/18
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int YStgManual(int hDevID, int YStgDir, float YStgManuSpeed);		            //V5.0 append by 山本 2001/07/31

		[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int GetRotAccel(ref float pRotAccelTm);		                                //V7.0 append by 間々田 2003/09/25

        //v9.0 added by 間々田 2004/02/09
		[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int PioOutBit(string XrayCom, int OnOff);

		[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		private static extern int XStgIndex(int hDevID, int XStgCoordi, float XStgIndexPos);
		[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		private static extern int YStgIndex(int hDevID, int YStgCoordi, float YStgIndexPos);

        //TEST added by 稲葉 2014/02/22_追加2014/10/07hata_v19.51反映
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int XrayUdManual(int UdDir, float UdManuSpeed);
		[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int XrayUdOrigin(MyCallbackDelegate  CallBackAddress);
		[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int DetUdManual(int UdDir, float UdManuSpeed);
		[DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int DetUdOrigin(MyCallbackDelegate  CallBackAddress);

        //Rev22.00 added by 長野 2015/08/20
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int TiltInit(int hDevID);
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int TiltManual(int hDevID,int TiltDir,float TiltManuSpeed);
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int TiltOrigin(int hDevID, MyCallbackDelegate CallBackAddress);
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int TiltSlowStop(int hDevID, MyCallbackDelegate CallBackAddress);
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int TiltFastStop(int hDevID, MyCallbackDelegate CallBackAddress);

        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int TiltRotInit(int hDevID);
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int TiltRotManual(int hDevID, int TiltRotDir, float TiltRotManuSpeed);
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int TiltRotOrigin(int hDevID, MyCallbackDelegate CallBackAddress);
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int TiltRotSlowStop(int hDevID, MyCallbackDelegate CallBackAddress);
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int TiltRotFastStop(int hDevID, MyCallbackDelegate CallBackAddress);


#endif
        //*******************************************************************************
        //機　　能： 外観カメラ移動ホームポジションセット
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V26.20  19/02/12  (検S1)長野      新規作成
        //*******************************************************************************
        public static void InitMoveByCam()
        {
            FixedCamType = CTSettings.iniValue.FixedCamPosition;
            if (FixedCamType == 1)//検査室固定
            {
                HomeFCD = CTSettings.iniValue.FixedCamFCD;
                HomeTableY = CTSettings.iniValue.FixedCamTableY;
                HomeTableZ = CTSettings.iniValue.FixedCamTableZ;
            }
            else//テーブル一体型
            {
                HomeFCD = (float)frmMechaControl.Instance.ntbFCD.Value;
                HomeTableY = (float)frmMechaControl.Instance.ntbTableXPos.Value;
                HomeTableZ = (float)CTSettings.t20kinf.Data.upper_limit / 100.0f;
            }
        }
        //*******************************************************************************
        //機　　能： 外観カメラ移動前バックアップ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V26.20  19/02/12  (検S1)長野      新規作成
        //*******************************************************************************
        public static void BackUpCurrentPosition()
        {
            BackUpFCD = (float)frmMechaControl.Instance.ntbFCD.Value;
            BackUpFDD = (float)frmMechaControl.Instance.ntbFID.Value;
            BackUpTableY = (float)frmMechaControl.Instance.ntbTableXPos.Value;
            if (CTSettings.scaninh.Data.cm_mode == 0)
            {
                BackUpTableZ = (float)CTSettings.mecainf.Data.ud_linear_pos;
            }
            else
            {
                BackUpTableZ = (float)CTSettings.mecainf.Data.udab_pos;
            }
            BackUpRot = (float)CTSettings.mecainf.Data.rot_pos;
        }

        //*******************************************************************************
		//機　　能：
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static void MechaOpen()
		{
            int hPio = 0;
			int error_sts = 0;

			//メカ制御ボードドライバオープン
			error_sts = MechaDevOpen(ref modDeclare.hDevID1);

			//メカエラーリセット
			error_sts = Mechaerror_reset(modDeclare.hDevID1);

			//ＰＩＯボードの初期化
            //error_sts = PioDevOpen(ref hPioID1);
            error_sts = PioDevOpen(ref hPio);
            hPioID1 = hPio;
        }

		public static void MechaClose()
		{
			int error_sts = 0;

			//ＰＩＯボードクローズ
            error_sts = PioDevClose(hPioID1);
            
			//メカ制御ボードドライバクローズ
			error_sts = MechaDevClose(modDeclare.hDevID1);
		}


		//*******************************************************************************
		//機　　能： UdIndex の呼び出し
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		//Public Function MechaUdIndex(ByVal UdIndexPos As Single) As Long
		public static int MechaUdIndex(float UdIndexPos, CTStatus StatusLabel = null)
		{
			if ((StatusLabel != null))
			{
				//「テーブル移動中」と表示
				StatusLabel.Status = StringTable.GC_STS_TABLE_MOVING;
			}

			modCT30K.PauseForDoEvents(0.5F);			//v17.44追加 byやまおか 2011/02/17

			MyCallbackDelegate myCallback = new MyCallbackDelegate(MyCallback);
			MyCallbackCaptureDelegate myCallbackCapture = new MyCallbackCaptureDelegate(MyCallbackCapture);

			//UdIndexを呼び出す
			//MechaUdIndex = UdIndex(hDevID1, 0, UdIndexPos, AddressOf MyCallback)
			return UdIndex(modDeclare.hDevID1, 0, UdIndexPos, myCallbackCapture, myCallback);		//v17.02変更 byやまおか 2010/07/22
		}


        //*******************************************************************************
        //機　　能： UdIndex の呼び出し（マルチスキャン中の相対移動）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        //Public Function MechaUdIndex(ByVal UdIndexPos As Single) As Long
        public static int MechaUdIndex_MultiScan(float UdIndexPos, CTStatus StatusLabel = null)
        {
            if ((StatusLabel != null))
            {
                //「テーブル移動中」と表示
                StatusLabel.Status = StringTable.GC_STS_TABLE_MOVING;
            }

            modCT30K.PauseForDoEvents(0.5F);			//v17.44追加 byやまおか 2011/02/17

            MyCallbackDelegate myCallback = new MyCallbackDelegate(MyCallback);
            MyCallbackCaptureDelegate myCallbackCapture = new MyCallbackCaptureDelegate(MyCallbackCapture);

            //UdIndexを呼び出す
            //MechaUdIndex = UdIndex(hDevID1, 0, UdIndexPos, AddressOf MyCallback)
            //Rev23.10 絶対位置移動に変更 by長野 2015/11/03
            //return UdIndex(modDeclare.hDevID1, 1, UdIndexPos, myCallbackCapture, myCallback);		//v17.02変更 byやまおか 2010/07/22
            return UdIndex(modDeclare.hDevID1, 0, UdIndexPos, myCallbackCapture, myCallback);		//v17.02変更 byやまおか 2010/07/22
        }

		//*******************************************************************************
		//機　　能： UdSlowStop の呼び出し
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static int MechaUdStop()
		{
			MyCallbackDelegate myCallback = new MyCallbackDelegate(MyCallback);

			//UdSlowStopを呼び出す
			return UdSlowStop(modDeclare.hDevID1, myCallback);
		}


		//*******************************************************************************
		//機　　能： RotateSlowStop の呼び出し
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static int MechaRotateStop()
		{
			MyCallbackDelegate myCallback = new MyCallbackDelegate(MyCallback);

			//RotateSlowStopを呼び出す
			return RotateSlowStop(modDeclare.hDevID1, myCallback);
		}


		//*******************************************************************************
		//機　　能： XStgSlowStop の呼び出し
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static int MechaXStgStop(bool Fast = false)
		{
			int functionReturnValue = 0;

			MyCallbackDelegate myCallback = new MyCallbackDelegate(MyCallback);

			if (Fast)
			{
				functionReturnValue = XStgFastStop(modDeclare.hDevID1, myCallback);
			}
			else
			{
				functionReturnValue = XStgSlowStop(modDeclare.hDevID1, myCallback);
			}

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： YStgSlowStop の呼び出し
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static int MechaYStgStop(bool Fast = false)
		{
			int functionReturnValue = 0;

			MyCallbackDelegate myCallback = new MyCallbackDelegate(MyCallback);

			if (Fast)
			{
				functionReturnValue = YStgFastStop(modDeclare.hDevID1, myCallback);
			}
			else
			{
				functionReturnValue = YStgSlowStop(modDeclare.hDevID1, myCallback);
			}

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： PhmOff の呼び出し
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static int MecaPhmOff()
		{
			MyCallbackDelegate myCallback = new MyCallbackDelegate(MyCallback);

			return PhmOff(myCallback);
		}


		//*******************************************************************************
		//機　　能： PhmOn の呼び出し
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static int MecaPhmOn()
		{
			MyCallbackDelegate myCallback = new MyCallbackDelegate(MyCallback);

			return PhmOn(myCallback);
		}


		//*******************************************************************************
		//機　　能： RotateOrigin の呼び出し
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static int MecaRotateOrigin(bool IsExitOrigin = false)
		{
			int error_sts = 0;

			modCT30K.PauseForDoEvents(0.5F);			//v17.44追加 byやまおか 2011/02/17

			if (IsExitOrigin)
			{
				//原点復帰の前に原点センサを抜ける
                if (modLibrary.InRange(CTSettings.mecainf.Data.rot_pos, 1, 20 * 100 - 1))
				{
					error_sts = MecaRotateIndex(20, 1);             //20度相対回転させる
				}
			}

            MyCallbackDelegate myCallback = new MyCallbackDelegate(MyCallback);
            
            //'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
			//return RotateOrigin(modDeclare.hDevID1, myCallback);
            //原点復帰に成功したらフラグON     'v18.00変更 byやまおか 2011/07/01
            error_sts = RotateOrigin(modDeclare.hDevID1, MyCallback);
            if ((error_sts == 0)) Flg_StartupRotReset = true;

            return error_sts;

        
        }


		//*******************************************************************************
		//機　　能： RotateIndex の呼び出し
		//
		//           変数名          [I/O] 型        内容
		//引　　数： RotAngle        [I/ ] Single    回転角度（度）
		//           RotCoordi       [I/ ] Long      0:絶対回転  1:相対回転
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static int MecaRotateIndex(float RotAngle, int RotCoordi = 0)
		{
			modCT30K.PauseForDoEvents(0.5F);			//v17.44追加 byやまおか 2011/02/17

			MyCallbackDelegate myCallback = new MyCallbackDelegate(MyCallback);

			return RotateIndex(modDeclare.hDevID1, RotCoordi, RotAngle, myCallback);
		}


		//*******************************************************************************
		//機　　能： UdOrigin の呼び出し
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static int MecaUdOrigin()
		{
			int error_sts = 0;

			//昇降軸初期化
			error_sts = UdInit(modDeclare.hDevID1);

			MyCallbackDelegate myCallback = new MyCallbackDelegate(MyCallback);

			//昇降軸原点復帰
            //v19.51 原点復帰に成功したらフラグON 'v19.51 追加 by長野 2014/02/27
            //return UdOrigin(modDeclare.hDevID1, myCallback);
            error_sts = UdOrigin(modDeclare.hDevID1, MyCallback);
            if ((error_sts == 0))　Flg_StartupUpDownReset = true;
            return error_sts;
        
        }


		//*******************************************************************************
		//機　　能： XStgOrigin の呼び出し
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static int MecaXStgOrigin()
		{
			int error_sts = 0;

			//微調X軸初期化
			error_sts = XStgInit(modDeclare.hDevID1);

			MyCallbackDelegate myCallback = new MyCallbackDelegate(MyCallback);

			//微調X軸原点復帰
			return XStgOrigin(modDeclare.hDevID1, myCallback);
		}


		//*******************************************************************************
		//機　　能： YStgOrigin の呼び出し
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static int MecaYStgOrigin()
		{
			int error_sts = 0;

			//微調Y軸初期化
			error_sts = YStgInit(modDeclare.hDevID1);

			MyCallbackDelegate myCallback = new MyCallbackDelegate(MyCallback);

			//微調Y軸原点復帰
			return YStgOrigin(modDeclare.hDevID1, myCallback);
		}

		//*******************************************************************************
		//機　　能： XStgIndex の呼び出し
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static int MecaXStgIndex(float pos)
		{
			//絶対位置指定で呼び出し
			return XStgIndex(modDeclare.hDevID1, 0, pos);
		}


		//*******************************************************************************
		//機　　能： YStgIndex の呼び出し
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static int MecaYStgIndex(float pos)
		{
			//絶対位置指定で呼び出し
			return YStgIndex(modDeclare.hDevID1, 0, pos);
		}

        //*******************************************************************************
        //機　　能： TiltOrigin の呼び出し
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V22.00  15/08/20  (検S1)長野    新規作成
        //*******************************************************************************
        public static int MecaTilt_TiltOrigin()
        {
            int error_sts = 0;

            //回転傾斜テーブル 傾斜初期化
            error_sts = TiltInit(modDeclare.hDevID1);

            MyCallbackDelegate myCallback = new MyCallbackDelegate(MyCallback);

            //回転傾斜テーブル チルト原点復帰
            return TiltOrigin(modDeclare.hDevID1, myCallback);
        }

        //*******************************************************************************
        //機　　能： MecaTilt_RotOrigin の呼び出し
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V22.00  15/08/20  (検S1)長野    新規作成
        //*******************************************************************************
        public static int MecaTilt_RotOrigin()
        {
            int error_sts = 0;

            //チルト回転テーブル 回転初期化
            error_sts = TiltRotInit(modDeclare.hDevID1);

            MyCallbackDelegate myCallback = new MyCallbackDelegate(MyCallback);

            //チルト回転テーブル 回転原点復帰
            return TiltRotOrigin(modDeclare.hDevID1, myCallback);
        }

		//*******************************************************************************
		//機　　能： メカ動作中のdll呼び出し中も、CT30Kがイベントを取れるようにする
		//           ためのコールバック関数
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static void MyCallback(int parm)
		{
			Application.DoEvents();

#region		//v17.02削除 tmrLiveでUpdateするから byやまおか 2010/07/20
			//'ライブ画像取り込み中の場合
			//If frmTransImage.CaptureOn Then
			//    'MilCapture frmTransImage.hMil, TransImage(0)
			//    'frmTransImage.Update
			//    Select Case DetType     'v17.00追加(ここから) byやまおか 2010/02/08
			//        Case DetTypeII, DetTypeHama
			//            MilCapture frmTransImage.hMil, TransImage(0)
			//            frmTransImage.Update
			//
			//        Case DetTypePke
			//            'PkeCapture frmTransImage.hPke, DestImage(0), TransImage(0)   'changed by 山本　2009-09-16
			//            'frmTransImage.Update    '削除 by 山本　2010-09-16
			//    End Select              'v17.00追加(ここまで) byやまおか 2010/02/08
			//End If
#endregion
		}


		//*******************************************************************************
		//機　　能： メカ動作中のdll呼び出し中も、CT30Kがイベントを取れるようにする
		//           ためのコールバック関数(シーケンサーにも通知する)
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V17.46  2011/02/27  やまおか    新規作成
		//*******************************************************************************
		public static void MyCallbackSeq(int parm)
		{
			//通信チェックプロパティを更新
			modSeqComm.MySeq.BitWrite("CommCheck", true);

			//CT30K側に返す
			Application.DoEvents();			//v17.50追加 by 間々田 2011/03/18
		}


		//*******************************************************************************
		//機　　能： メカ動作中のdll呼び出し中も、CT30Kがイベントを取れるようにする
		//           ためのコールバック関数（キャプチャ中は間引く）
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V17.02  10/07/22    やまおか    新規作成
		//*******************************************************************************
		public static void MyCallbackCapture(int parm)
		{
			int SkipCount = 0;
			float myCurrentFrameRate = 0;

			//ライブ画像取り込み中の場合
			if (frmTransImage.Instance.CaptureOn)
			{
				//フレームレート(計算値)を取得
				myCurrentFrameRate = frmTransImage.Instance.GetCurrentFR();

				//フレームレートが15fps以下のときは間引いてDoEventsする
				if (myCurrentFrameRate < 15)
				{
					//間引き間隔
					SkipCount = Convert.ToInt32(250 / myCurrentFrameRate);

					//SkipCount回に1回だけDoEventsする
					if ((MyCallbackCapture_CBcount % SkipCount) == 0)
					{
						Application.DoEvents();
						MyCallbackCapture_CBcount = 0;
					}
				}
				//それ以外は毎回DoEventsする
				else
				{
					Application.DoEvents();
				}
			}
			//ライブ画像停止中の場合は毎回DoEventsする
			else
			{
				Application.DoEvents();
			}
			MyCallbackCapture_CBcount = MyCallbackCapture_CBcount + 1;		//カウント インクリメント
		}


		//'*******************************************************************************
		//'機　　能： 自動スキャン位置指定時用移動関数
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： FCD             [I/ ] Variant   FCD(単位 mm、オフセット値含めない)
		//'           FID             [I/ ] Variant   FID(単位 mm、オフセット値含めない)
		//
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//'*******************************************************************************
		//Public Function MoveAutoScanPos(Optional ByVal FCD As Variant, _
		//'                                Optional ByVal Fid As Variant, _
		//'                                Optional ByVal y As Variant, _
		//'                                Optional ByVal fx As Variant, _
		//'                                Optional ByVal fy As Variant, _
		//'                                Optional ByVal UpDownPos As Variant) As Boolean
		//    Dim Msg As String
		//    Msg = ""
		//
		//    '戻り値初期化
		//    MoveAutoScanPos = False
		//
		//    If Not IsMissing(FCD) Then
		//        If FCD <> GVal_Fcd Then
		//            Msg = Msg & "FCD(mm):" & vbTab & vbTab & Format$(FCD, "####0.00") & vbCr
		//        End If
		//    End If
		//
		//    If Not IsMissing(Fid) Then
		//        If Fid <> GVal_Fid Then
		//            Msg = Msg & "FID(mm):" & vbTab & vbTab & Format$(Fid, "####0.00") & vbCr
		//        End If
		//    End If
		//
		//    If Not IsMissing(y) Then
		//        If y <> frmMechaControl.ntbTableXPos.Value Then
		//            Msg = Msg & AxisName(1) & "(mm):" & vbTab & vbTab & Format$(y, "####0.00") & vbCr
		//        End If
		//    End If
		//
		//    If Not IsMissing(fx) Then
		//        If fx <> frmMechaControl.ntbFTablePosX.Value Then
		//            Msg = Msg & GetResString(12131, AxisName(0)) & "(mm):" & vbTab & Format$(fx, "####0.00") & vbCr
		//        End If
		//    End If
		//
		//    If Not IsMissing(fy) Then
		//        If fy <> frmMechaControl.ntbFTablePosY.Value Then
		//            Msg = Msg & GetResString(12131, AxisName(1)) & "(mm):" & vbTab & Format$(fy, "####0.00") & vbCr
		//        End If
		//    End If
		//
		//    If Not IsMissing(UpDownPos) Then
		//        If UpDownPos <> frmMechaControl.ntbUpDown.Value Then
		//            Msg = Msg & "高さ(mm):" & vbTab & vbTab & Format$(UpDownPos, "####0.000") & vbCr
		//        End If
		//    End If
		//
		//    '移動不要の場合はここで抜ける
		//    If Msg = "" Then Exit Function
		//
		//    '確認メッセージ
		//    '   下記指定位置までテーブル等を移動します。
		//    '   ...
		//    '   よろしいですか？
		//    If MsgBox(LoadResString(12140) & vbCr & vbCr & _
		//'              Msg & vbCr & _
		//'             LoadResString(12141), vbInformation + vbOKCancel) = vbCancel Then Exit Function
		//
		//    '試料テーブルをＸ線管近くに移動する場合
		//    If Not IsMissing(FCD) Then
		//        If (FCD < GVal_Fcd) And (FCD < GVal_FcdLimit) Then
		//
		//            'Ｘ線干渉が制限されている場合
		//            If (scaninh.table_restriction = 0) And (Not MySeq.stsTableMovePermit) Then
		//                If MsgBox("指定されたFCD位置に試料テーブルを移動させるには、Ｘ線管干渉制限を解除する必要があります。" & vbCr & _
		//'                          "解除すると、試料テーブルはＸ線管近くに移動し、場合によってはワーク等がＸ線管に衝突する恐れがあります。" & vbCr & _
		//'                          "Ｘ線管干渉制限を解除しますか？", vbExclamation + vbYesNo) = vbNo Then Exit Function
		//                '制限解除
		//                SeqBitWrite "TableMovePermit", True
		//            Else
		//                If MsgBox("試料テーブルはＸ線管近くに移動し、場合によってはワーク等がＸ線管に衝突する恐れがあります。" & vbCr & _
		//'                          "よろしいですか？", vbExclamation + vbYesNo) = vbNo Then Exit Function
		//            End If
		//
		//        End If
		//    End If
		//
		//    'マウスポインタを砂時計にする
		//    Screen.MousePointer = vbHourglass
		//
		//    Dim buf As String
		//    buf = ""
		//
		//    If Not IsMissing(fx) Then
		//        If MecaYStgIndex(fx) <> 0 Then
		//             'エラーの場合：指定されたX軸位置まで微調テーブルを移動させることができませんでした。
		//            buf = buf & "* " & GetResString(IDS_MoveErr, AxisName(0), LoadResString(IDS_FTable)) & vbCr
		//        End If
		//    End If
		//
		//    If Not IsMissing(fy) Then
		//        If MecaXStgIndex(fy) <> 0 Then
		//             'エラーの場合：指定されたY軸位置まで微調テーブルを移動させることができませんでした。
		//            buf = buf & "* " & GetResString(IDS_MoveErr, AxisName(1), LoadResString(IDS_FTable)) & vbCr
		//        End If
		//    End If
		//
		//    If Not IsMissing(UpDownPos) Then
		//        If MechaUdIndex(UpDownPos) <> 0 Then
		//            'エラーの場合：指定された昇降位置まで試料テーブルを移動させることができませんでした。
		//            buf = buf & "* " & GetResString(IDS_MoveErr, "昇降", LoadResString(IDS_SampleTable)) & vbCr
		//        End If
		//    End If
		//
		//    'I.I.を後退させる場合、I.I.を移動させてから試料テーブルを移動させる
		//    If (Not IsMissing(Fid)) And (Fid > GVal_Fid) Then
		//
		//        If Not IsMissing(Fid) Then
		//            '指定FID位置までI.I.を移動させる
		//            If Not MoveFID(Fid * 10) Then
		//                'エラーの場合：指定されたFID位置までI.I.を移動させることができませんでした。
		//                buf = buf & "* " & BuildResStr(IDS_MoveErr, IDS_FID, IDS_II) & vbCr
		//            End If
		//        End If
		//
		//        If Not IsMissing(FCD) Then
		//            '指定FCD位置まで試料テーブルを移動させる
		//            If Not MoveFCD(FCD * 10) Then
		//                'エラーの場合：指定されたFCD位置まで試料テーブルを移動させることができませんでした。
		//                buf = buf & "* " & BuildResStr(IDS_MoveErr, IDS_FCD, IDS_SampleTable) & vbCr
		//            End If
		//        End If
		//
		//    Else
		//
		//        If Not IsMissing(FCD) Then
		//            '指定FCD位置まで試料テーブルを移動させる
		//            If Not MoveFCD(FCD * 10) Then
		//                'エラーの場合：指定されたFCD位置まで試料テーブルを移動させることができませんでした。
		//                buf = buf & "* " & BuildResStr(IDS_MoveErr, IDS_FCD, IDS_SampleTable) & vbCr
		//            End If
		//        End If
		//
		//        If Not IsMissing(Fid) Then
		//            '指定FID位置までI.I.を移動させる
		//            If Not MoveFID(Fid * 10) Then
		//                'エラーの場合：指定されたFID位置までI.I.を移動させることができませんでした。
		//                buf = buf & "* " & BuildResStr(IDS_MoveErr, IDS_FID, IDS_II) & vbCr
		//            End If
		//        End If
		//
		//    End If
		//
		//    'テーブルＹ軸移動：（従来の）Ｘ軸方向の移動
		//    If Not IsMissing(y) Then
		//        If Not MoveXpos(y * 100) Then
		//            'エラーの場合：指定されたY軸位置まで試料テーブルを移動させることができませんでした。
		//            buf = buf & "* " & GetResString(IDS_MoveErr, AxisName(1), LoadResString(IDS_SampleTable)) & vbCr
		//        End If
		//    End If
		//
		//    'マウスポインタを元に戻す
		//    Screen.MousePointer = vbDefault
		//
		//    '移動できなかった場合、メッセージを表示
		//    If buf <> "" Then
		//        MsgBox buf, vbExclamation
		//    End If
		//
		//    '戻り値セット
		//    MoveAutoScanPos = (buf = "")
		//
		//End Function


        //追加2014/10/07hata_v19.51反映 
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここから
        //*******************************************************************************
        //機　　能： 機構部動作が可能か判定する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： IsOkMechaMove   Boolean         True:動作OK False:動作NG
        //
        //補　　足： なし
        //
        //履　　歴： V18.00  2011/02/19  やまおか    新規作成
        //*******************************************************************************
        //public static bool IsOkMechaMove()
        //public static bool IsOkMechaMove(bool detSystemReset = false)//Rev23.20 引数追加 by長野 2015/12/21
        public static bool IsOkMechaMove(bool detSystemReset = false, bool doorLock = true)//Rev25.10 引数追加 by 長野 2017/09/11
        {
            bool functionReturnValue = false;

            //初期化
            functionReturnValue = false;

            //運転準備ボタンが押されていなければ無効
            if (!modSeqComm.MySeq.stsRunReadySW)
            {
                //運転準備が未完了のため動作しません。運転準備をＯＮにしてから操作を行ってください。
                //Interaction.MsgBox(CTResources.LoadResString(17510), MsgBoxStyle.OkOnly);
                MessageBox.Show(CTResources.LoadResString(17510), Application.ProductName, MessageBoxButtons.OK);
               
                return functionReturnValue;
            }

            if (modSeqComm.MySeq.stsRoomInSw == true)
            {

                //入室スイッチがONの場合は操作不可にする by長野 2014/03/04
                //Interaction.MsgBox(CTResources.LoadResString(21362), MsgBoxStyle.OkOnly);
                MessageBox.Show(CTResources.LoadResString(21362), Application.ProductName, MessageBoxButtons.OK);

                return functionReturnValue;

            }

            //Rev23.20 追加 by長野 2015/12/21
            //検出器が2世代もしくは不定状態であれば、処理を中止
            if (CTSettings.scaninh.Data.ct_gene2and3 == 0 && detSystemReset == false && !modSeqComm.MySeq.stsFDSystemPos)
            {
                //検出器システムが準備未完です。検出器システムのリセットを行ってください。
                MessageBox.Show(CTResources.LoadResString(23100), Application.ProductName, MessageBoxButtons.OK);
                return functionReturnValue;
            }


            //操作パネルがある場合
            //Rev23.10 操作パネルONの機能を分離 by長野 2015/10
            //if ((CTSettings.scaninh.Data.avmode == 0))
            if ((CTSettings.scaninh.Data.avmode == 0) || (CTSettings.scaninh.Data.op_panel == 0))
            {
                //操作パネルがONなら
                if (modSeqComm.MySeq.PcInhibit)
                {
                    //操作パネルがONのため動作しません。
                    //Interaction.MsgBox(CT30K.My.Resources.str17513, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation);
                    //変更2014/11/18hata_MessageBox確認
                    //MessageBox.Show(CTResources.LoadResString(17513), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(CTResources.LoadResString(17513), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return functionReturnValue;
                }

            //操作パネルがない場合
            }
            //Rev26.40 add by chouno 2019/02/17
            else if (CTSettings.scaninh.Data.high_speed_camera == 0 && CTSettings.iniValue.HSCSettingType == 1)
            {
                //メッセージ表示：
                //動作許可が不許可なら
                if (modSeqComm.MySeq.PcInhibit)
                {
                    //動作許可がOFFのため動作しません。
                    MessageBox.Show(CTResources.LoadResString(27000), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return functionReturnValue;
                }
            }
            else
            {
                ////メンテナンスのときは検査室扉が閉まっていることをチェックしない
                //if (!modSeqComm.MySeq.stsDoorPermit)
                //{
                //    if (CTSettings.scaninh.Data.door_lock == 1)
                //    {
                //        //試料扉が閉じていなければ無効
                //        //if ((My.MyProject.Forms.frmCTMenu.DoorStatus != frmCTMenu.DoorStatusConstants.DoorClosed) & (My.MyProject.Forms.frmCTMenu.DoorStatus != frmCTMenu.DoorStatusConstants.DoorLocked))
                //        if ((frmCTMenu.Instance.DoorStatus != frmCTMenu.DoorStatusConstants.DoorClosed) & (frmCTMenu.Instance.DoorStatus != frmCTMenu.DoorStatusConstants.DoorLocked))
                //        {
                //            //扉が開いているため動作しません。
                //            //Interaction.MsgBox(CT30K.My.Resources.str17511, MsgBoxStyle.OkOnly);
                //            MessageBox.Show(CTResources.LoadResString(17511), Application.ProductName, MessageBoxButtons.OK);
                //            return functionReturnValue;
                //        }
                //    }
                //    else
                //    {
                //        //試料扉が閉じていなければ無効
                //        //if ((My.MyProject.Forms.frmCTMenu.DoorStatus != frmCTMenu.DoorStatusConstants.DoorClosed) & (My.MyProject.Forms.frmCTMenu.DoorStatus != frmCTMenu.DoorStatusConstants.DoorLocked))
                //        if ((frmCTMenu.Instance.DoorStatus != frmCTMenu.DoorStatusConstants.DoorLocked))
                //        {
                //            //扉が開いているため動作しません。
                //            //Interaction.MsgBox(CT30K.My.Resources.str17511, MsgBoxStyle.OkOnly);
                //            MessageBox.Show(CTResources.LoadResString(17511), Application.ProductName, MessageBoxButtons.OK);
                //            return functionReturnValue;
                //        }
                //    }
                //}
                //メンテナンスのときは検査室扉が閉まっていることをチェックしない
                if (!modSeqComm.MySeq.stsDoorPermit)
                {
                    if (doorLock == true) //Rev25.10 add by chouno 2017/09/11
                    {
                        //Rev25.03 電磁ロック機能なしの場合は、扉閉だけを見る by chouno 2017/05/29
                        if (CTSettings.scaninh.Data.door_lock == 1)
                        {
                            //試料扉が閉じていなければ無効
                            if ((frmCTMenu.Instance.DoorStatus != frmCTMenu.DoorStatusConstants.DoorClosed) & (frmCTMenu.Instance.DoorStatus != frmCTMenu.DoorStatusConstants.DoorLocked))
                            {
                                //扉が開いているため動作しません。
                                //Interaction.MsgBox(CT30K.My.Resources.str17511, MsgBoxStyle.OkOnly);
                                //MessageBox.Show(CTResources.LoadResString(17511), Application.ProductName, MessageBoxButtons.OK);
                                //Rev25.03 change by chouno 2017/05/29
                                MessageBox.Show(CTResources.LoadResString(17511), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return functionReturnValue;
                            }
                        }
                        else
                        {
                            //Rev25.03 電磁ロック閉が条件とする by chouno 2017/05/29
                            //試料扉が閉じていなければ無効
                            //if ((My.MyProject.Forms.frmCTMenu.DoorStatus != frmCTMenu.DoorStatusConstants.DoorClosed) & (My.MyProject.Forms.frmCTMenu.DoorStatus != frmCTMenu.DoorStatusConstants.DoorLocked))
                            //if (frmCTMenu.Instance.DoorStatus != frmCTMenu.DoorStatusConstants.DoorLocked)
                            //Rev25.03 test
                            if (frmCTMenu.Instance.DoorStatus != frmCTMenu.DoorStatusConstants.DoorLocked)
                            {
                                //扉が開いているため動作しません。
                                //Interaction.MsgBox(CT30K.My.Resources.str17511, MsgBoxStyle.OkOnly);
                                //MessageBox.Show(CTResources.LoadResString(17511), Application.ProductName, MessageBoxButtons.OK);
                                //Rev25.03 change by chouno 2017/05/29
                                MessageBox.Show(CTResources.LoadResString(17511), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return functionReturnValue;
                            }
                        }
                    }
                }
            }

            //動作OK
            functionReturnValue = true;
            return functionReturnValue;

        }

        //*******************************************************************************
        //機　　能： 回転テーブル(大)用アタッチメントを装着していても機構部動作が可能か判定する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： IsOkMechaMoveWithLargeTable     Boolean         True:動作OK False:動作NG
        //
        //補　　足： なし
        //
        //履　　歴： V26.00  2017/03/13  長野    新規作成
        //*******************************************************************************
        public static bool IsOkMechaMoveWithLargeTable()
        {
            bool ret = false;

            //Rev26.00 add by chouno 2017/10/16 
            frmMechaControl.Instance.tmrMecainfSeqCommEx();

            try
            {
                //Rev26.14 計測の場合は微調内蔵ではないのでメッセージは出さない
                //if (CTSettings.scaninh.Data.cm_mode == 1)
                //Rev26.20 微調テーブルタイプで見る by chouno 2019/02/06
                if(CTSettings.t20kinf.Data.ftable_type == 0)
                {
                    //Rev26.00 add by chouno 2017/03/13
                    //v19.51 回転大テーブルが装着されている場合は、操作不可にする by長野 2014/03/03
                    if ((modSeqComm.GetLargeRotTableSts() == 1))
                    {
                        //Rev26.00 X線・検出器昇降タイプと標準で分ける by chouno 2017/03/13
                        if (CTSettings.t20kinf.Data.ud_type == 1)
                        {
                            //Interaction.MsgBox(CT30K.My.Resources.str21359, MsgBoxStyle.Critical);
                            MessageBox.Show(CTResources.LoadResString(21359), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            throw new Exception();

                        }
                        else
                        {
                            //if (CTSettings.mecainf.Data.xstg_pos != 0.00f || CTSettings.mecainf.Data.ystg_pos != 0.00f)
                            //Rev26.14 計測CTは微調内蔵ではない by chouno 2018/09/10
                            //if ((CTSettings.mecainf.Data.xstg_pos != 0.00f || CTSettings.mecainf.Data.ystg_pos != 0.00f) && CTSettings.scaninh.Data.cm_mode == 1)
                            //Rev26.20 元に戻す by chouno 2019/02/06
                            if (CTSettings.mecainf.Data.xstg_pos != 0.00f || CTSettings.mecainf.Data.ystg_pos != 0.00f)
                            {
                                MessageBox.Show(CTResources.LoadResString(21363), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                throw new Exception();
                            }
                        }
                    }
                }
                ret = true;
            }
            catch
            {
                ret = false;
            }

            return ret;
        }


        //追加2014/10/07hata_v19.51反映 
        //*******************************************************************************
        //機　　能： 電動フィルタを切り替える関数
        //
        //           変数名          [I/O] 型        内容
        //引　　数： condIndex       Integer         選択されているＸ線条件 0:L 1:M 2:H
        //           iiIndex         Integer         選択されているI.I.視野 0:9 1:6 3:4.5
        //
        //戻 り 値： ChangeXConditionFilter   Boolean         True:動作完了 False:動作せず
        //
        //補　　足： なし
        //
        //履　　歴： V18.00  2011/07/05  やまおか    新規作成
        //*******************************************************************************
        //public static bool ChangeXConditionFilter(short condIndex, short iiIndex)
        public static bool ChangeXConditionFilter(int condIndex, int iiIndex)
        {
            bool functionReturnValue = false;

            float theXFltThick = 0;     //v18.00追加 byやまおか 2011/03/06
            int theXFltNum = 0;         //v18.00追加 byやまおか 2011/03/06
            int cnt = 0;                //v18.00追加 byやまおか 2011/03/06

            functionReturnValue = false;

            theXFltNum = -1;            //変数初期化

            //フィルタ厚(mm)を取得する
            //theXFltThick = CTSettings.infdef.Data.filter[condIndex, iiIndex];
            theXFltThick = CTSettings.infdef.Data.filter[iiIndex * 3 + condIndex];
            
            //0mmはフィルタなし
            if ((theXFltThick == 0.0))
            {
                theXFltNum = 0;

                //0mmでなければ
            }
            else
            {
                //フィルタ厚(mm)からフィルタ番号を取得する(最後はシャッター)
                //for (cnt = 1; cnt <= Information.UBound(modInfdef.infdef.xfilter_c) - 1; cnt++)
                for (cnt = 1; cnt <= CTSettings.infdef.Data.xfilter_c.GetUpperBound(0) - 1; cnt++)
                    {
                        if ((theXFltThick == Convert.ToSingle(modLibrary.GetFirstItem(CTSettings.infdef.Data.xfilter_c[cnt].GetString(), "mm"))))
                    {
                        theXFltNum = cnt;
                    }
                }
            }

            //filtertable.csvのフィルタ厚(mm)と、infdef.xfilter_c()の値が一致したら
            if ((theXFltNum != -1))
            {
                //指定のフィルタに切り替える
                //modSeqComm.SeqBitWrite("Filter" + theXFltNum, true);
                modSeqComm.SeqBitWrite("Filter" + theXFltNum.ToString(), true);

                //一致するものがなければ抜ける
            }
            else
            {
                return functionReturnValue;

            }

            //成功
            functionReturnValue = true;
            return functionReturnValue;

        }

        //追加2014/10/07hata_v19.51反映 
        //*******************************************************************************
        //機　　能： 電動フィルタの番号をチェックする関数
        //
        //           変数名          [I/O] 型        内容
        //引　　数： condIndex       Integer         選択されているＸ線条件 0:L 1:M 2:H
        //           iiIndex         Integer         選択されているI.I.視野 0:9 1:6 3:4.5
        //
        //戻 り 値： GetXFilterIndex Long            X線フィルタ番号
        //
        //補　　足： なし
        //
        //履　　歴： V18.00  2011/08/09  やまおか    新規作成
        //*******************************************************************************
        //public static int GetXFilterIndex(short condIndex, short iiIndex)
        public static int GetXFilterIndex(int condIndex, int iiIndex)
        {
            int functionReturnValue = 0;

            float theXFltThick = 0;
            int cnt = 0;

            //変数初期化
            theXFltThick = -1;

            //フィルタ厚(mm)を取得する
            //theXFltThick = modInfdef.infdef.filter_Renamed[condIndex, iiIndex];
            theXFltThick = CTSettings.infdef.Data.filter[iiIndex * 3 + condIndex];


            //0mmはフィルタなし
            if ((theXFltThick == 0.0))
            {
                functionReturnValue = 0;

                //0mmでなければ
            }
            else
            {
                //フィルタ厚(mm)からフィルタ番号を取得する(最後はシャッター)
                //for (cnt = 1; cnt <= Information.UBound(modInfdef.infdef.xfilter_c) - 1; cnt++)
                for (cnt = 1; cnt <= CTSettings.infdef.Data.xfilter_c.GetUpperBound(0) - 1; cnt++)
                {
                    //if ((theXFltThick == Convert.ToSingle(modLibrary.GetFirstItem(modInfdef.infdef.xfilter_c[cnt], "mm"))))
                    if ((theXFltThick == Convert.ToSingle(modLibrary.GetFirstItem(CTSettings.infdef.Data.xfilter_c[cnt].GetString(), "mm"))))
                    {
                        functionReturnValue = cnt;
                    }
                }
            }
            return functionReturnValue;

        }
        //Rev23.40/23.21 追加 by長野 2016/04/05
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここまで
        //*******************************************************************************
        //機　　能： 移動先が、干渉エリア内、かつ、テーブル昇降が制限値を超えないかチェック
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //           
        //
        //戻 り 値： チェックに該当する場合　：false
        //           チェックに該当しない場合：true
        //補　　足： なし
        //
        //履　　歴： V23.21  2016/03/10  (検S1)長野  新規作成
        //*******************************************************************************
        //public static int GetXFilterIndex(short condIndex, short iiIndex)
        public static bool chkTablePosByAutoPos(float TagetTablePos, float TagetFcd)
        {
            bool ret = false;

            if (modSeqComm.MySeq.stsUpLimitPermit == true) //機能がない場合はtrueで返す by長野 2016/03/10
            {
                ret = true;
            }
            else
            {
                float LimitPos = 0.0f;//LimitPosにはシーケンサから与えられる昇降位置制限値を入れる。
                LimitPos = (float)modSeqComm.MySeq.stsUpLimitPos / 100.0f;
                //if ((TagetTablePos < LimitPos) && (CTSettings.GVal_FcdLimit > TagetFcd))
                //Rev25.10 add by chouno 2017/09/11
                if ((TagetTablePos < LimitPos) && (modSeqComm.GetFCDLimit() > TagetFcd))
                {
                    ret = false;
                }
                else
                {
                    ret = true;
                }
            }

            return ret;

        }

	}
}
