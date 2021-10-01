using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Test
{
    public static class Module1
	{
        //追加2015/03/14hata
        public static bool XstgFlg = false;
        public static bool YstgFlg = false; 

#if AllTest
        
        public static int hDevID = 0;
		public static int hDevPIO =0;
        //共有メモリのハンドル
        public static IntPtr hComMap = IntPtr.Zero;

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
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //    Declare Function RotateIndex Lib "mechacontrol.dll" (ByVal hDevID As Long, ByVal RotCoordi As Long, ByVal RotAngle As Single) As Long
		public static extern int RotateIndex(int hDevID, int RotCoordi, float RotAngle, int CallBackAddress);
        //public static extern int RotateIndex(int hDevID, int RotCoordi, float RotAngle, MyCallbackDelegate CallBackAddress);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //Declare Function RotateOrigin Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long
		public static extern int RotateOrigin(int hDevID, int CallBackAddress);
        //public static extern int RotateOrigin(int hDevID, MyCallbackDelegate CallBackAddress);                                 //v17.46変更 byやまおか 2011/02/27
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //    Declare Function RotateSlowStop Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long
        public static extern int RotateSlowStop(int hDevID, int CallBackAddress);
        //public static extern int RotateSlowStop(int hDevID, MyCallbackDelegate CallBackAddress);                              //v15.0変更 by 間々田 2009/06/18
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int RotateFastStop(int hDevID, int CallBackAddress);
        //public static extern int RotateFastStop(int hDevID, MyCallbackDelegate CallBackAddress);                              //v15.0変更 by 間々田 2009/06/18
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int UdInit(int hDevID);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int UdManual(int hDevID, int UdDir, float UdManuSpeed);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //変更2014/07/23hata_v1951反映
        ////Declare Function UdIndex Lib "mechacontrol.dll" (ByVal hDevID As Long, ByVal UdCoordi As Long, ByVal UdPos As Single) As Long
		//public static extern int UdIndex(int hDevID, int UdCoordi, float UdPos, int CallBackAddress);
        public static extern int UdIndex(int hDevID, int UdCoordi, float UdAngle, int CallBackCaptureAddress, int CallBackAddress);
        //public static extern int UdIndex(int hDevID, int UdCoordi, float UdAngle, MyCallbackCaptureDelegate CallBackCaptureAddress, MyCallbackDelegate CallBackAddress);		//v17.02変更 byやまおか 2010/07/22

        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //Declare Function UdOrigin Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long
		public static extern int UdOrigin(int hDevID, int CallBackAddress);
        //public static extern int UdOrigin(int hDevID, MyCallbackDelegate CallBackAddress);		                                //v10.0変更 by 間々田 2005/02/10
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //    Declare Function UdSlowStop Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long
        //    Declare Function UdFastStop Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long
		public static extern int UdSlowStop(int hDevID, int CallBackAddress);
        //public static extern int UdSlowStop(int hDevID, MyCallbackDelegate CallBackAddress);		                            //v15.0変更 by 間々田 2009/06/18
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int UdFastStop(int hDevID, int CallBackAddress);
        //public static extern int UdFastStop(int hDevID, MyCallbackDelegate CallBackAddress);		                            //v15.0追加 by 間々田 2009/06/17
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int XStgInit(int hDevID);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int XStgManual(int hDevID, int XStgDir, float XStgManuSpeed);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int XStgIndex(int hDevID, int XStgCoordi, float XStgPos);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //Declare Function XStgOrigin Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long
		public static extern int XStgOrigin(int hDevID, int CallBackAddress);
        //public static extern int XStgOrigin(int hDevID, MyCallbackDelegate CallBackAddress);		                            //v11.5変更 by 間々田 2006/06/30
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //Declare Function XStgSlowStop Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long
        //Declare Function XStgFastStop Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long
		public static extern int XStgSlowStop(int hDevID, int CallBackAddress);
        //public static extern int XStgSlowStop(int hDevID, MyCallbackDelegate CallBackAddress);		                        //v15.0変更 by 間々田 2009/06/18
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int XStgFastStop(int hDevID, int CallBackAddress);
        //public static extern int XStgFastStop(int hDevID, MyCallbackDelegate CallBackAddress);		                        //v15.0変更 by 間々田 2009/06/18
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int YStgInit(int hDevID);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int YStgManual(int hDevID, int YStgDir, float YStgManuSpeed);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int YStgIndex(int hDevID, int YStgCoordi, float YStgPos);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //Declare Function YStgOrigin Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long
		public static extern int YStgOrigin(int hDevID, int CallBackAddress);
        //public static extern int YStgOrigin(int hDevID, MyCallbackDelegate CallBackAddress);		                        //v11.5変更 by 間々田 2006/06/30
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //Declare Function YStgSlowStop Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long
        //Declare Function YStgFastStop Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long
		public static extern int YStgSlowStop(int hDevID, int CallBackAddress);
        //public static extern int YStgSlowStop(int hDevID, MyCallbackDelegate CallBackAddress);                    		//v15.0変更 by 間々田 2009/06/18
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int YStgFastStop(int hDevID, int CallBackAddress);
        //public static extern int YStgFastStop(int hDevID, MyCallbackDelegate CallBackAddress);		                    //v15.0変更 by 間々田 2009/06/18
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //Declare Function PhmOn Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long
        //Declare Function PhmOff Lib "mechacontrol.dll" (ByVal hDevID As Long) As Long
		public static extern int PhmOn(int CallBackAddress);
        //public static extern int PhmOn(MyCallbackDelegate CallBackAddress);		                                        //V5.0 append by 山本 2001/07/31
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//V5.0 append by 山本 2001/07/31
		public static extern int PhmOff(int CallBackAddress);
        //public static extern int PhmOff(MyCallbackDelegate CallBackAddress);		                                        //V5.0 append by 山本 2001/07/31
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//V5.0 append by 山本 2001/07/31
		public static extern int PhmStop(int hDevID);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int PioDevOpen(ref int hDevPIO);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int PioDevClose(int hDevPIO);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int PioChkStart(int ChkTim);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int PioChkEnd();
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int PioOutBit(string BitName, int data);
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SwOpeStart();
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int SwOpeEnd();
		
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int GetRotAccel(ref float RotAccelTime);
        //PublicDeclare Function getcommon_float Lib "comlib.dll" (ByVal com_name As String, ByVal name As String, data As Single) As Long
        //Public Declare Function getcommon_long Lib "comlib.dll" (ByVal com_name As String, ByVal name As String, data As Long) As Long
        //Public ErrForm As frmErrDialog

        //コモン用共有メモリの作成
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr CreateSharedCTCommon();

        //コモン用共有メモリの破棄
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int DestroySharedCTCommon(IntPtr hMap);

        //コモンファイルを共有メモリにセット
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetSharedCTCommon();

        //共有メモリ上のコモンファイルをファイルに保存
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SaveSharedCTCommon();


        #region 共有メモリの作成と破棄
        //追加2015/02/09hata
        /// <summary>
        /// 共有メモリの作成
        /// </summary>
        /// <returns></returns>
        public static int InitializeSharedCTCommon()
        {
            //共有メモリの作成
            hComMap = CreateSharedCTCommon();
            if (hComMap == IntPtr.Zero)
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("共有メモリ作成失敗",
                //                    Application.ProductName,
                //                    MessageBoxButtons.OK,
                //                    MessageBoxIcon.Error);
                return 1;

            }
            //共有メモリのセット
            if (SetSharedCTCommon() != 0)
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("共有メモリセット失敗",
                //                    Application.ProductName,
                //                    MessageBoxButtons.OK,
                //                    MessageBoxIcon.Error);

                return 2;
            }

            return 0;
        }

        //追加2015/02/09hata
        //コモンファイルを共有メモリ解放
        /// <summary>
        /// 共有メモリの解放
        /// </summary>
        public static void ExitSharedCTCommon()
        {
            //共有メモリを解放
            if (hComMap != IntPtr.Zero)
            {
                //ComLib.SaveSharedCTCommon();
                DestroySharedCTCommon(hComMap);
                hComMap = IntPtr.Zero;
            }

        }
        #endregion


        //追加2015/03/14hata
        public static void MyCallback(int parm)
        {
            Application.DoEvents();
        }


#endif

        //public static short[] SpeedData = new short[10];
        public static int[] SpeedData = new int[10];

        public static SeqComm.Seq MySeq;
        public static frmMechaStatus MyfMSts =null;


        public static void Init(frmMechaStatus form)
        {
            //MyfMSts = new frmMechaStatus();
            MyfMSts = form;

#if AllTest
            //追加2015/03/14hata
            MyCallbackDelegate myCallback = new MyCallbackDelegate(MyCallback);
#endif

        }


        //ｼｰｹﾝｻへのﾋﾞｯﾄ書込み
		public static int PlcBitWrite(ref string DeviceName, ref bool data)
		//public static int PlcBitWrite(string DeviceName, bool data)
		{
            int ReturnValue = 0;
            DialogResult Response;
            //bool data1 = data;

            if (MySeq == null) return ReturnValue;

            ReturnValue = MySeq.BitWrite(DeviceName, data);
            //ReturnValue = MySeq.BitWrite(DeviceName, data1);
            if (ReturnValue !=  0)
            {
                //        frmMechaStatus.Timer1.Enabled = False
                Console.Beep();
                Response = MessageBox.Show("エラーコード ： " + ReturnValue + "\n" + "シーケンサへの書込みを続行しますか", "通信エラー",
                                                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                if (Response == DialogResult.No)
                {
                    MyfMSts.Timer1.Enabled = false;
                    MyfMSts.cmdSeqWrite.BackColor = System.Drawing.SystemColors.Control;
                }
                else
                {
                    MyfMSts.Timer1.Enabled = true;
                    MyfMSts.cmdSeqWrite.BackColor = System.Drawing.Color.Lime;
                }
                MyfMSts.commTest.stsCommBusy = false;
            }
            return ReturnValue;

		}

        //ｼｰｹﾝｻへのﾜｰﾄﾞ書込み
		public static int PlcWordWrite(ref string DeviceName, ref string data)
		//public static int PlcWordWrite(string DeviceName, string data)
		{
            int ReturnValue = 0;
            DialogResult Response;
            
            if (MySeq == null) return ReturnValue;

            //ReturnValue = MySeq.WordWrite(DeviceName, ref data);
            ReturnValue = MySeq.WordWrite(DeviceName,ref  data);
            if (ReturnValue != 0)
            {
                Console.Beep();
                Response = MessageBox.Show("エラーコード ： " + ReturnValue + "\n" + "シーケンサへの書込みを続行しますか", "通信エラー",
                                                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                if (Response == DialogResult.No)
                {
                    if (MyfMSts != null)
                    {
                        MyfMSts.Timer1.Enabled = false;
                        MyfMSts.cmdSeqWrite.BackColor = System.Drawing.SystemColors.Control;
                    }
                }
                else
                {
                    if (MyfMSts != null)
                    {
                        MyfMSts.Timer1.Enabled = true;
                        MyfMSts.cmdSeqWrite.BackColor = System.Drawing.Color.Lime;
                    }
                }
                MySeq.stsCommBusy = false;
            }

            return ReturnValue;
		}

        //ｼｰｹﾝｻからﾒｶｽﾃｰﾀｽの読み出し
		public static int MechaStatusReasd()
		{
            int ReturnValue = 0;
            DialogResult Response;

            ReturnValue = MySeq.StatusRead();
            if (ReturnValue != 0)
            {
                Console.Beep();
                Response = MessageBox.Show("エラーコード ： " + ReturnValue + "\n" + "シーケンサへの書込みを続行しますか", "通信エラー",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                if (Response == DialogResult.No)
                {
                    if (MyfMSts != null)
                    {
                        MyfMSts.Timer2.Enabled = false;
                        MyfMSts.cmdSeqRead.BackColor = System.Drawing.SystemColors.Control;
                    }
                }
                else
                {
                    if (MyfMSts != null)
                    {
                        MyfMSts.Timer2.Enabled = true;
                        MyfMSts.cmdSeqRead.BackColor = System.Drawing.Color.Lime;
                    }
                }
                MySeq.stsCommBusy = false;
            }
            return ReturnValue;

		}



    
    
    }
}
