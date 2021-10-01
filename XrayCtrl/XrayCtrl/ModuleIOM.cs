using System;

namespace XrayCtrl
{
	internal static class ModuleIOM
	{
		//cDio1クラスのプロパティの実体
		public static int gDioDi00 = 0;		//IO入力 接点00 (0 or 1)
		public static int gDioDi01 = 0;		//IO入力 接点01
		public static int gDioDi02 = 0;		//IO入力 接点02
		public static int gDioDi03 = 0;		//IO入力 接点03
		public static int gDioDi04 = 0;		//IO入力 接点04
		public static int gDioDi05 = 0;		//IO入力 接点05
		public static int gDioDi06 = 0;		//IO入力 接点06
		public static int gDioDi07 = 0;		//IO入力 接点07
		public static int gDioDi10 = 0;		//IO入力 接点10
		public static int gDioDi11 = 0;		//IO入力 接点11
		public static int gDioDi12 = 0;		//IO入力 接点12
		public static int gDioDi13 = 0;		//IO入力 接点13
		public static int gDioDi14 = 0;		//IO入力 接点14
		public static int gDioDi15 = 0;		//IO入力 接点15
		public static int gDioDi16 = 0;		//IO入力 接点16
		public static int gDioDi17 = 0;		//IO入力 接点17
		public static int gDioDi20 = 0;		//IO入力 接点20
		public static int gDioDi21 = 0;		//IO入力 接点21
		public static int gDioDi22 = 0;		//IO入力 接点22
		public static int gDioDi23 = 0;		//IO入力 接点23
		public static int gDioDi24 = 0;		//IO入力 接点24
		public static int gDioDi25 = 0;		//IO入力 接点25
		public static int gDioDi26 = 0;		//IO入力 接点26
		public static int gDioDi27 = 0;		//IO入力 接点27
		public static int gDioDi30 = 0;		//IO入力 接点30
		public static int gDioDi31 = 0;		//IO入力 接点31
		public static int gDioDi32 = 0;		//IO入力 接点32
		public static int gDioDi33 = 0;		//IO入力 接点33
		public static int gDioDi34 = 0;		//IO入力 接点34
		public static int gDioDi35 = 0;		//IO入力 接点35
		public static int gDioDi36 = 0;		//IO入力 接点36
		public static int gDioDi37 = 0;		//IO入力 接点37

		public static int gDioDo00 = 0;		//IO出力 接点00 (0 or 1)
		public static int gDioDo01 = 0;		//IO出力 接点01
		public static int gDioDo02 = 0;		//IO出力 接点02
		public static int gDioDo03 = 0;		//IO出力 接点03
		public static int gDioDo04 = 0;		//IO出力 接点04
		public static int gDioDo05 = 0;		//IO出力 接点05
		public static int gDioDo06 = 0;		//IO出力 接点06
		public static int gDioDo07 = 0;		//IO出力 接点07
		public static int gDioDo10 = 0;		//IO出力 接点10
		public static int gDioDo11 = 0;		//IO出力 接点11
		public static int gDioDo12 = 0;		//IO出力 接点12
		public static int gDioDo13 = 0;		//IO出力 接点13
		public static int gDioDo14 = 0;		//IO出力 接点14
		public static int gDioDo15 = 0;		//IO出力 接点15
		public static int gDioDo16 = 0;		//IO出力 接点16
		public static int gDioDo17 = 0;		//IO出力 接点17
		public static int gDioDo20 = 0;		//IO出力 接点20
		public static int gDioDo21 = 0;		//IO出力 接点21
		public static int gDioDo22 = 0;		//IO出力 接点22
		public static int gDioDo23 = 0;		//IO出力 接点23
		public static int gDioDo24 = 0;		//IO出力 接点24
		public static int gDioDo25 = 0;		//IO出力 接点25
		public static int gDioDo26 = 0;		//IO出力 接点26
		public static int gDioDo27 = 0;		//IO出力 接点27
		public static int gDioDo30 = 0;		//IO出力 接点30
		public static int gDioDo31 = 0;		//IO出力 接点31
		public static int gDioDo32 = 0;		//IO出力 接点32
		public static int gDioDo33 = 0;		//IO出力 接点33
		public static int gDioDo34 = 0;		//IO出力 接点34
		public static int gDioDo35 = 0;		//IO出力 接点35
		public static int gDioDo36 = 0;		//IO出力 接点36
		public static int gDioDo37 = 0;		//IO出力 接点37

		public static IntPtr hDeviceHandle = IntPtr.Zero;


		//設定プロパティ

		//'-----------------------------------------------------------------------
		//'
		//'   \WINNT\SYSTEM32\NSD_IOM.INI
		//'
		//Public Function IomIniFileRead() As Integer
		//    Dim Fname   As String
		//    Dim Fno     As Integer
		//    Dim Var1    As String
		//    Dim Var2    As Integer
		//
		//    Fno = FreeFile()
		//
		//    Fname = "C:\CT\INI\IOM.INI"
		//
		//    On Error GoTo EX
		//    Open Fname For Input As #Fno
		//    Input #Fno, Var1, Var2
		//
		//    Close #Fno
		//
		//EX:
		//End Function

		//-----------------------------------------------------------------------
		//   最新の状態に更新する(Dio)
		//
		//
		public static uint DiRefresh()
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			int Ans = 0;
			short[] value = new short[3];

			//32点入力
			Ans = DioInputPoint(hDeviceHandle, gDioDi00, 1, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi01, 2, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi02, 3, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi03, 4, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi04, 5, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi05, 6, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi06, 7, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi07, 8, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi10, 9, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi11, 10, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi12, 11, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi13, 12, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi14, 13, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi15, 14, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi16, 15, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi17, 16, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi20, 17, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi21, 18, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi22, 19, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi23, 20, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi24, 21, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi25, 22, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi26, 23, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi27, 24, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi30, 25, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi31, 26, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi32, 27, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi33, 28, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi34, 29, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi35, 30, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi36, 31, 1);
			Ans = DioInputPoint(hDeviceHandle, gDioDi37, 32, 1);

			return Ans;
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			int[] value = new int[32];

			uint Ans = gpc2000.DioInputPoint(hDeviceHandle, value, 0, 32);

			gDioDi00 = value[0];
			gDioDi01 = value[1];
			gDioDi02 = value[2];
			gDioDi03 = value[3];
			gDioDi04 = value[4];
			gDioDi05 = value[5];
			gDioDi06 = value[6];
			gDioDi07 = value[7];
			gDioDi10 = value[8];
			gDioDi11 = value[9];
			gDioDi12 = value[10];
			gDioDi13 = value[11];
			gDioDi14 = value[12];
			gDioDi15 = value[13];
			gDioDi16 = value[14];
			gDioDi17 = value[15];
			gDioDi20 = value[16];
			gDioDi21 = value[17];
			gDioDi22 = value[18];
			gDioDi23 = value[19];
			gDioDi24 = value[20];
			gDioDi25 = value[21];
			gDioDi26 = value[22];
			gDioDi27 = value[23];
			gDioDi30 = value[24];
			gDioDi31 = value[25];
			gDioDi32 = value[26];
			gDioDi33 = value[27];
			gDioDi34 = value[28];
			gDioDi35 = value[29];
			gDioDi36 = value[30];
			gDioDi37 = value[31];

			return Ans;
		}

		//-----------------------------------------------------------------------
		//   最新の状態に更新する(Dio)
		//
		//
		public static uint DoRefresh()
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			int Ans = 0;
			short[] value = new short[3];

			//32点出力
			Ans = DioOutputPoint(hDeviceHandle, gDioDo00, 1, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo01, 2, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo02, 3, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo03, 4, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo04, 5, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo05, 6, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo06, 7, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo07, 8, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo10, 9, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo11, 10, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo12, 11, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo13, 12, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo14, 13, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo15, 14, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo16, 15, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo17, 16, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo20, 17, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo21, 18, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo22, 19, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo23, 20, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo24, 21, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo25, 22, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo26, 23, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo27, 24, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo30, 25, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo31, 26, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo32, 27, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo33, 28, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo34, 29, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo35, 30, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo36, 31, 1);
			Ans = DioOutputPoint(hDeviceHandle, gDioDo37, 32, 1);

 			return Ans;
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			int[] value = new int[32];

			value[0] = gDioDo00;
			value[1] = gDioDo01;
			value[2] = gDioDo02;
			value[3] = gDioDo03;
			value[4] = gDioDo04;
			value[5] = gDioDo05;
			value[6] = gDioDo06;
			value[7] = gDioDo07;
			value[8] = gDioDo10;
			value[9] = gDioDo11;
			value[10] = gDioDo12;
			value[11] = gDioDo13;
			value[12] = gDioDo14;
			value[13] = gDioDo15;
			value[14] = gDioDo16;
			value[15] = gDioDo17;
			value[16] = gDioDo20;
			value[17] = gDioDo21;
			value[18] = gDioDo22;
			value[19] = gDioDo23;
			value[20] = gDioDo24;
			value[21] = gDioDo25;
			value[22] = gDioDo26;
			value[23] = gDioDo27;
			value[24] = gDioDo30;
			value[25] = gDioDo31;
			value[26] = gDioDo32;
			value[27] = gDioDo33;
			value[28] = gDioDo34;
			value[29] = gDioDo35;
			value[30] = gDioDo36;
			value[31] = gDioDo37;

			return gpc2000.DioOutputPoint(hDeviceHandle, value, 0, 32);
		}

		public static int IOM_Initialize()
		{
			//    IomIniFileRead
			DioInitialize();

			return 0;
		}

		public static int IOM_Terminate()
		{
			DioTerminate();

			return 0;
		}

		private static int DioInitialize()
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			int Ans = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			string lpszName = null;

			lpszName = "FBIDIO2";
			hDeviceHandle = gpc2000.DioOpen(lpszName, gpc2000.FBIDIO_FLAG_SHARE);

			return 0;
		}

		private static int DioTerminate()
		{
			uint Ans = 0;
			Ans = gpc2000.DioClose(hDeviceHandle);

			return 0;
		}
	}
}
