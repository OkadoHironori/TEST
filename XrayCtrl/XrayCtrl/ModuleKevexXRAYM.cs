using System;
using System.IO;

namespace XrayCtrl
{
	internal static class ModuleKevexXRAYM
	{
		//cCmdクラスのプロパティの実体
		//Public fXraymForcus          As Integer  '焦点
		//Public fXraymVolt            As Integer  '管電圧
		//Public fXraymAmp             As Integer  '管電流
		//Public fXraymOn              As Integer  'X線ON
		public static int fXraymOff = 0;			//X線OFF(X線ON不可能)
		//Public fXraymOffDateRst      As Integer  'X線OFF日時強制更新
		//Public fXraymInterlockErrRst As Integer  'インターロック異常表示解除
		//Public fXraymOnErrRst        As Integer  'X線ON異常表示解除
		//Public fXraymOffErrRst       As Integer  'X線OFF異常表示解除
		//Public fXraymEmergency       As Integer  '異常停止
		//
		//'cStsクラスのプロパティの実体
		//Public gXraymWarmup          As Integer  'ウォームアップ不要
		//Public gXraymWarmup2D        As Integer  'ウォームアップ2D
		//Public gXraymWarmup2W        As Integer  'ウォームアップ2W
		//Public gXraymWarmup3W        As Integer  'ウォームアップ3W
		//Public gXraymWarmupOn        As Integer  'ウォームアップ中
		//Public gXraymWarmupEnd       As Integer  'ウォームアップ完
		//Public gXraymWarmupTimer     As Integer  'ウォームアップ残時間
		//Public gXraymOffDateRst      As Integer  'X線OFF日時強制更新
		//Public gXraymOffDate         As Date     'X線OFF日時
		//Public gXraymForcus          As Integer  '焦点
		//Public gXraymVolt            As Single  '管電圧
		//Public gXraymAmp             As Single  '管電流
		//Public gXraymWatt            As Single   '電力
		//'Public gxraymFine            As Integer  'ファイン表示
		//Public gXraymOn              As Integer  'X線ON
		//Public gXraymTVCL            As Integer  'TVCL制御中
		//Public gXraymInterlockErr    As Integer  'インターロック異常
		//Public gXraymInterlockErrDsp As Integer  'インターロック異常表示
		//Public gXraymOnErr           As Integer  'X線ON異常
		//Public gXraymOnErrDsp        As Integer  'X線ON異常表示
		//Public gXraymOffErr          As Integer  'X線OFF異常
		//Public gXraymOffErrDsp       As Integer  'X線OFF異常表示
		//Public gXraymErr             As Integer  '異常発生中
		//Public gXraymPermitWarmup    As Integer  'ﾌﾟﾘﾋｰﾄ完了
		//Public gXAvail    As Integer  'アベイラブル
		//
		//Public ifXrayonoff_Set  As Integer

		//内部
		private static DateTime D200 = DateTime.MinValue;
		private static DateTime D201 = DateTime.MinValue;

		private static DateTime D202 = DateTime.MinValue;
		private static double sD202 = 0;
		private static double sD2021 = 0;
		private static double sD2022 = 0;
		private static double sD2023 = 0;
		private static int D203 = 0;
		private static int D300 = 0;
		private static int D301 = 0;
		private static int D304 = 0;
		//Private D305 As Integer
		//Private D306 As Integer
		//Private D307 As Integer
		private static int D308 = 0;
		private static int D309 = 0;
		private static int D310 = 0;
		private static int D400 = 0;
		private static int D401 = 0;
		private static int D404 = 0;
		//Private D405 As Integer
		//Private D406 As Integer
		//Private D407 As Integer
		private static int D408 = 0;
		private static int D409 = 0;
		private static int D410 = 0;
		private static int D500 = 0;
		private static int D501 = 0;
		private static int D504 = 0;
		//Private D505 As Integer
		//Private D506 As Integer
		//Private D507 As Integer
		private static int D508 = 0;
		private static int D509 = 0;
		private static int D510 = 0;
		private static int D600 = 0;
		private static int D601 = 0;
		private static int D602 = 0;
		private static int D603 = 0;
		private static int D604 = 0;
		private static int D607 = 0;

		private static float D614 = 0;
		private static float D615 = 0;
		private static float D616 = 0;
		private static float D701 = 0;
		private static float D702 = 0;
		private static float D704 = 0;
		private static float D705 = 0;

		//Private D706 As Integer
		private static int D707 = 0;
		private static int D708 = 0;
		private static int D709 = 0;
		private static int D710 = 0;
		//Private D711 As Integer
		//Private D712 As Integer
		private static int D713 = 0;
		private static int D714 = 0;
		private static int D715 = 0;
		//Private D716 As Integer
		private static int D717 = 0;
		private static int D718 = 0;
		//Private D719 As Integer
		private static int D720 = 0;
		private static int D721 = 0;
		//Private D801 As Integer
		//Private D802 As Integer

		private static int D803 = 0;
		private static int D804 = 0;

		//Private i102 As Integer
		//Private i103 As Integer
		//Private i907 As Integer
		//Private i904 As Integer
		//Private i1101 As Integer
		//Private i1008 As Integer
		//Private i1010 As Integer

		private static int M100 = 0;
		private static int M101 = 0;
		private static int M102 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M103 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M104 = 0;
		private static int M105 = 0;
		private static int M106 = 0;
		private static int M107 = 0;
		private static int M108 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M109 = 0;
//		private static int M110 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M111 = 0;
		private static int M112 = 0;
		private static int M113 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M114 = 0;
//		private static int M115 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M116 = 0;
		private static int M117 = 0;
		private static int M118 = 0;
		private static int M119 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M120 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M121 = 0;
		private static int M122 = 0;
		private static int M123 = 0;
		private static int M124 = 0;
		private static int M125 = 0;
		private static int M126 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M127 = 0;
//		private static int M128 = 0;
//		private static int M129 = 0;			//X線ON指令
//		private static int M130 = 0;			//謎･･･
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M131 = 0;
		private static int M132 = 0;
		private static int M133 = 0;
		private static int M134 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M200 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M201 = 0;
		private static int M202 = 0;
		private static int M203 = 0;
		private static int M204 = 0;
		private static int M205 = 0;
		private static int M206 = 0;
		private static int M207 = 0;
		private static int M208 = 0;
		private static int M209 = 0;
		private static int M210 = 0;
		private static int M211 = 0;
		private static int M212 = 0;
		//Private M213 As Integer

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M214 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M215 = 0;
		private static int M216 = 0;

		//1999-12-07 T.Shibui
		private static int M217 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M300 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M301 = 0;
		private static int M302 = 0;
		private static int M303 = 0;
		private static int M304 = 0;
		private static int M305 = 0;
		private static int M306 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M307 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M308 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M309 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M310 = 0;
		private static int M311 = 0;
		private static int M312 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M400 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M401 = 0;
		private static int M402 = 0;
		private static int M403 = 0;
		private static int M404 = 0;
		private static int M405 = 0;
		private static int M406 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M407 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M408 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M409 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M410 = 0;
		private static int M411 = 0;
		private static int M412 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M500 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M501 = 0;
		private static int M502 = 0;
		private static int M503 = 0;
		private static int M504 = 0;
		private static int M505 = 0;
		private static int M506 = 0;
		//Private M507 As Integer
		private static int M508 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M509 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M510 = 0;
		private static int M511 = 0;
		private static int M512 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M600 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M601 = 0;
		private static int M602 = 0;
		private static int M603 = 0;
		private static int M604 = 0;
		private static int M605 = 0;
		private static int M606 = 0;
		//Private M607 As Integer
		//Private M608 As Integer
		//Private M609 As Integer
		//Private M610 As Integer
		//Private M611 As Integer
		//Private M612 As Integer
		//Private M613 As Integer
		private static int M614 = 0;
		private static int M615 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M616 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//Private M617 As Integer
		//Private M618 As Integer
		//Private M619 As Integer
		private static int M700 = 0;
		private static int M701 = 0;
		private static int M702 = 0;
		private static int M703 = 0;
		private static int M704 = 0;
		private static int M705 = 0;
		private static int M706 = 0;
		private static int M707 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M708 = 0;
//		private static int M709 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M710 = 0;
		private static int M711 = 0;
		private static int M712 = 0;
		private static int M713 = 0;
		private static int M714 = 0;
		private static int M715 = 0;
		private static int M716 = 0;
		private static int M717 = 0;
		private static int M718 = 0;
		private static int M719 = 0;
		private static int M720 = 0;
		private static int M721 = 0;
		//Private M722 As Integer
		private static int M723 = 0;
		private static int M724 = 0;
		private static int M725 = 0;
		private static int M726 = 0;
		private static int M727 = 0;
		private static int M728 = 0;
		private static int M729 = 0;
		private static int M730 = 0;
		private static int M731 = 0;
		//Private M732 As Integer
		//Private M733 As Integer
		//Private M734 As Integer
		//Private M735 As Integer
		//Private M736 As Integer
		//Private M737 As Integer
		//Private M738 As Integer
		//Private M739 As Integer
		private static int M740 = 0;
		private static int M741 = 0;
		private static int M742 = 0;
		private static int M743 = 0;
		//Private M744 As Integer
		private static int M745 = 0;
		private static int M746 = 0;
		private static int M747 = 0;
		private static int M748 = 0;
		private static int M749 = 0;
		private static int M750 = 0;
		private static int M751 = 0;
		//Private M752 As Integer
		private static int M753 = 0;
		private static int M754 = 0;
		private static int M755 = 0;
		private static int M756 = 0;
		private static int M757 = 0;
		private static int M758 = 0;
		//Private M801 As Integer
		//Private M802 As Integer
		//Private M803 As Integer
		//Private M804 As Integer
		//Private M805 As Integer
		//Private M806 As Integer
		private static int M807 = 0;
		//Private M808 As Integer
		//Private M809 As Integer
		//Private M810 As Integer
		//Private M811 As Integer
		//Private M812 As Integer
		//Private M813 As Integer
		//Private M814 As Integer
		//Private M815 As Integer
		//Private M816 As Integer
		//Private M817 As Integer
		//Private M818 As Integer
		private static int M819 = 0;
		private static int M820 = 0;
		private static int M821 = 0;

		//Private M900 As Integer
		private static int M901 = 0;
		private static int M902 = 0;
		private static int M903 = 0;
		//Private M906 As Integer
		//Private M907 As Integer
		private static int M908 = 0;
		private static int M909 = 0;
		private static int M910 = 0;
		private static int M911 = 0;
		private static int M912 = 0;
		private static int M913 = 0;
		//Private M914 As Integer
		private static int M915 = 0;
		//Private M916 As Integer
		//Private M917 As Integer
		//Private M918 As Integer
		//Private M919 As Integer
		//Private M920 As Integer
		private static int M921 = 0;
		//Private M922 As Integer
		private static int M923 = 0;
		//Private M924 As Integer
		//Private M925 As Integer
		//Private M926 As Integer

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M927 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//Private M928 As Integer
		//Private M929 As Integer
		//Private M930 As Integer
		//Private M931 As Integer
		private static int M932 = 0;
		private static int M933 = 0;
		//Private M938 As Integer
		private static int M939 = 0;
		private static int M940 = 0;
		private static int M941 = 0;
		private static int M942 = 0;
		private static int M943 = 0;

		//Private M1000 As Integer
		//Private M1001 As Integer
		//Private M1002 As Integer
		//Private M1003 As Integer
		//Private M1004 As Integer

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M1005 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//Private M1006 As Integer
		private static int M1007 = 0;
		private static int M1008 = 0;
		private static int M1009 = 0;
		private static int M1010 = 0;
		//Private M1011 As Integer
		private static int M1012 = 0;
		private static int M1013 = 0;
		//Private M1014 As Integer
		//Private M1015 As Integer
		//Private M1016 As Integer
		//Private M1017 As Integer
		//Private M1018 As Integer
		//Private M1019 As Integer
		//Private M1020 As Integer
		private static int M1021 = 0;
		//Private M1022 As Integer
		//Private M1023 As Integer
		//Private M1024 As Integer
		//Private M1025 As Integer
		//Private M1026 As Integer
		//Private M1027 As Integer
		//Private M1028 As Integer
		private static int M1029 = 0;
		//Private M1100 As Integer
		//Private M1101 As Integer
		//Private M1102 As Integer
		//Private M1103 As Integer
		//Private M1104 As Integer
		//Private M1105 As Integer
		//Private M1106 As Integer
		//Private M1107 As Integer
		//Private M1108 As Integer
		private static int M1109 = 0;
		//Private M1110 As Integer
		//Private M1111 As Integer
		//Private M1112 As Integer
		//Private M1113 As Integer
		//Private M1114 As Integer
		//Private M1115 As Integer
		private static int M1116 = 0;
		//Private M1117 As Integer
		//Private M1118 As Integer
		//Private M1119 As Integer
		//Private M1120 As Integer
		//Private M1121 As Integer
		//Private M1122 As Integer
		private static int M1123 = 0;
		//Private M1124 As Integer
		private static int M1125 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		private static int M1126 = 0;
		private static int M1201 = 0;
		private static int M1202 = 0;
		private static int M1203 = 0;
		private static int M1204 = 0;
		private static int M1205 = 0;
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//Private M1206 As Integer
		private static int M1207 = 0;
		//Private M1208 As Integer
		private static int M1209 = 0;

		private static DateTime XrayOffTimeReadTime = DateTime.MinValue;

		//１ショットタイマー番号割付
		private static mLogic.PulseType P101 = new mLogic.PulseType();
		private static mLogic.PulseType P102 = new mLogic.PulseType();

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static mLogic.PulseType P103 = new mLogic.PulseType();
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//ＯＮディレータイマー番号割付
		//Private T101 As DelayType
		//Private T102 As DelayType
		private static mLogic.DelayType T103 = new mLogic.DelayType();

		//Private T301 As DelayType
		//Private T302 As DelayType
		//Private T401 As DelayType
		//Private T402 As DelayType
		//Private T501 As DelayType
		//Private T502 As DelayType
		private static mLogic.DelayType T701 = new mLogic.DelayType();
		private static mLogic.DelayType T702 = new mLogic.DelayType();
		//Private T801 As DelayType
		//Private T802 As DelayType
		//Private T803 As DelayType
		//Private T804 As DelayType
		//Private T805 As DelayType
		private static mLogic.DelayType T806 = new mLogic.DelayType();
		private static mLogic.DelayType T807 = new mLogic.DelayType();
		private static mLogic.DelayType T901 = new mLogic.DelayType();
		//Private T902 As DelayType
		//Private T903 As DelayType
		//Private T904 As DelayType
		//Private T905 As DelayType

		//1999-11-19 T.Shibui    未使用
		//Private T906 As DelayType

		//Private T1001 As DelayType
		//Private T1002 As DelayType
		//Private T1003 As DelayType
		//Private T1101 As DelayType
		//Private T1102 As DelayType
		//Private T1103 As DelayType
		//Private T1104 As DelayType
		//Private T1201 As DelayType
		//Private T1202 As DelayType

		private const int rDaV = 1;
		private const double rDaA90 = 0.09766;
		private const int rDaA130 = 1;

		//
		// Warmup2D_130KV で使用する static フィールド
		//
		private static int Warmup2D_130KV_iSec = 0;
		private static DateTime Warmup2D_130KV_st = DateTime.MinValue;
		private static int Warmup2D_130KV_stf = 0;

		//
		// Warmup2W_130KV で使用する static フィールド
		//
		static float Warmup2W_130KV_iSec = 0;
		static DateTime Warmup2W_130KV_st = DateTime.MinValue;
		static int Warmup2W_130KV_stf = 0;

		//
		// Warmup3W_130KV メソッドで使用する static フィールド
		//
		static float Warmup3W_130KV_iSec = 0;
		static DateTime Warmup3W_130KV_st = DateTime.MinValue;
		static int Warmup3W_130KV_stf = 0;

		/// <summary>
		/// 
		/// </summary>
		public static void KevexXraymLogic()
		{
			//=========================================================
			//入力

			//ｲﾝﾀｰﾛｯｸ
			M101 = ModuleXRAYM.fXraymEmergency;		//CTRLMより
			M102 = ModuleIOM.gDioDi14;			//DIO
//		    If i102 > 0 Then
//				M102 = 0
//			Else
//				M102 = 1
//			End If

			M915 = ModuleIOM.gDioDi30;			//X線ON中
			if (ModuleIOM.gDioDi30 == 1)
			{
				M915 = 0;
			}
			else
			{
				M915 = 1;
			}

			M104 = M933;				//異常発生中
			M123 = ModuleXRAYM.fXraymOffDateRst;

			//X線発生器ﾀｲﾌﾟ 130kv固定
			M1008 = 0;
			M1010 = 1;

			M119 = ModuleXRAYM.fXraymForcus;			//焦点
			D600 = ModuleXRAYM.fXraymVolt;				//管電圧
			D614 = ModuleXRAYM.fXraymVolt;				//管電圧
			D603 = ModuleXRAYM.fXraymAmp;				//管電流
			D615 = ModuleXRAYM.fXraymAmp;				//管電流
			M100 = ModuleXRAYM.fXraymOn;				//X線ON
			M134 = fXraymOff;							//X線OFF
			M911 = ModuleXRAYM.fXraymInterlockErrRst;	//インターロック異常表示解除
			M921 = ModuleXRAYM.fXraymOnErrRst;			//X線ON異常表示解除
			M1207 = ModuleXRAYM.fXraymOffErrRst;		//X線OFF異常表示解除

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			M1005 = 1;									//強制解除
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			D200 = DateTime.Now;
			if (M215 == 0)
			{
				XrayOffTimeRead();
				D201 = XrayOffTimeReadTime;
				M215 = 1;
			}

			//=========================================================
			//X線ON
			if (M101 > 0)
			{
				M121 = 0;
			}
			else
			{
				M121 = 1;
			}

			if (M104 > 0)
			{
				M105 = 0;
			}
			else
			{
				M105 = 1;
			}

//			If M121 * M102 * M103 * M105 > 0 Then
			if (M121 * M102 * M105 > 0)
			{
				M106 = 1;
			}
			else
			{
				M106 = 0;
			}

			//X線ON指令
			if (M100 * M106 > 0)
			{
				M107 = 1;
			}
			else
			{
				M107 = 0;
			}

			//X線OFF指令
			if (M107 > 0)
			{
				M108 = 0;
			}
			else
			{
				M108 = 1;
			}

			if (M306 + M406 + M506 > 0)
			{
				M118 = 1;
				M132 = 1;
			}
			else
			{
				M118 = 0;
			}

			mLogic.iOnDelay(M132, ref T103, 1000, ref M133);
			if (M133 != 0)
			{
				M132 = 0;
			}

			if (M113 > 0)
			{
				M122 = 0;
			}
			else if (M107 > 0)
			{
				M122 = 1;
			}

			if (M108 * M122 * M207 > 0)
			{
				M111 = 1;
			}
			else
			{
				M111 = 0;
			}

			if (M118 + M111 > 0)
			{
				M112 = 1;
			}
			else
			{
				M112 = 0;
			}

			mLogic.iPulse(M112, ref P101, ref M113);			//X線OFF書き込み
			mLogic.iPulse(M123, ref P102, ref M124);			//X線OFF書き込み
			mLogic.iPulse(M217, ref P102, ref M131);			//X線OFF書き込み

			if (M113 + M124 + M131 > 0)
			{
				M125 = 1;
			}
			else
			{
				M125 = 0;
			}

			if (M125 > 0)
			{
				D201 = DateTime.Now;
				XrayOffTimeWrite(D201);
			}

			if (M119 + M211 > 0)
			{
				M116 = 1;
			}
			else
			{
				M116 = 0;
			}

			//焦点選択
			if (M107 + M939 != 0)
			{
				M126 = 1;
			}
			else
			{
				M126 = 0;
			}
			if (M126 > 0)
			{
			} else {
				if (M116 > 0)
				{
					M117 = 1;
				}
				else
				{
					M117 = 0;
				}
			}

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			if (M117 != 0)
			{
				M130 = 0;
			}
			else
			{
				M130 = 1;
			}
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//=========================================================
			//ウォームアップ判定
			D202 = DateTime.FromOADate(D200.ToOADate() - D201.ToOADate());
			sD202 = D202.ToOADate();
			sD2021 = new DateTime(1899, 12, 30 + 0, 3, 0, 0).ToOADate();
			sD2022 = new DateTime(1899, 12, 30 + 2, 0, 0, 0).ToOADate();
			sD2023 = new DateTime(1899, 12, 30 + 14, 0, 0, 0).ToOADate();

			if (sD202 < sD2021)
			{
				M201 = 1;
			}
			else
			{
				M201 = 0;
			}

			if (M201 > 0)
			{
				M202 = 0;
			}
			else
			{
				M202 = 1;
			}

			if (sD202 < sD2022)
			{
				M203 = 1;
			}
			else
			{
				M203 = 0;
			}

			if (M203 > 0)
			{
				M204 = 0;
			}
			else
			{
				M204 = 1;
			}

			if (sD202 < sD2023)
			{
				M205 = 1;
			}
			else
			{
				M205 = 0;
			}

			if (M205 > 0)
			{
				M206 = 0;
			}
			else
			{
				M206 = 1;
			}

			//ウォームアップ不要
			if (M201 * M203 * M205 > 0)
			{
				M207 = 1;
			}
			else
			{
				M207 = 0;
			}

			if (M212 != 0)
			{
				M943 = 0;
			}
			else
			{
				M943 = 1;
			}

			//1999-11-25 T.Shibui X線ON中で、ｳｫｰﾑｱｯﾌﾟ中でない時はｳｫｰﾑｱｯﾌﾟ不要
			if (M908 * M943 != 0)
			{
				M942 = 0;
			}
			else
			{
				M942 = 1;
			}

			//ウォームアップ２D
			if (M203 * M202 * M205 * M942 > 0)
			{
				M208 = 1;
			}
			else
			{
				M208 = 0;
			}

			//ウォームアップ２W
			if (M205 * M202 * M204 * M942 > 0)
			{
				M209 = 1;
			}
			else
			{
				M209 = 0;
			}

			//ウォームアップ３W
			if (M202 * M204 * M206 * M942 > 0)
			{
				M210 = 1;
			}
			else
			{
				M210 = 0;
			}

			//ウォームアップ必要
			if (M208 + M209 + M210 > 0)
			{
				M211 = 1;
			}
			else
			{
				M211 = 0;
			}

			//ウォームアップ中
			if (M305 + M405 + M505 > 0)
			{
				M212 = 1;
			}
			else
			{
				M212 = 0;
			}

			//ウォームアップ残時間
			if (M305 > 0)
			{
				D203 = D304;
			}

			//ウォームアップ残時間
			if (M405 > 0)
			{
				D203 = D404;
			}

			//ウォームアップ残時間
			if (M505 > 0)
			{
				D203 = D504;
			}
			if (M212 != 0)
			{
				M216 = 0;
			}
			else
			{
				M216 = 1;
			}
			if (M216 != 0)
			{
				D203 = 0;
			}

			if (M207 * M908 != 0)
			{
				M217 = 1;
			}
			else
			{
				M217 = 0;
			}

			//=========================================================
			//X線ウォームアップ（２Ｄ）
			if (M405 > 0)
			{
				M301 = 0;
			}
			else
			{
				M301 = 1;
			}

			if (M505 > 0)
			{
				M302 = 0;
			}
			else
			{
				M302 = 1;
			}

			if (M301 * M302 > 0)
			{
				M303 = 1;
			}
			else
			{
				M303 = 0;
			}

			if (M107 * M303 * M208 > 0)
			{
				M304 = 1;
			}
			else
			{
				M304 = 0;
			}

			if (M108 > 0)
			{
				M305 = 0;
			}
			else if (M304 > 0)
			{
				M305 = 1;
			}

			if (M1013 * M305 > 0)
			{
				M308 = 1;
			}
			else
			{
				M308 = 0;
			}

			Warmup2D_130KV(ref M308, ref D308, ref D309, ref D310, ref M311);

			if (M1013 > 0)
			{
				D300 = D308;
			}

			if (M1013 > 0)
			{
				D301 = D309;
			}

			if (M1013 > 0)
			{
				D304 = D310;
			}

			if (M1013 * M311 > 0)
			{
				M312 = 1;
			}
			else
			{
				M312 = 0;
			}

			if (M310 + M312 > 0)
			{
				M306 = 1;
			}
			else
			{
				M306 = 0;
			}

			//=========================================================
			//X線ウォームアップ（２Ｗ）
			if (M305 > 0)
			{
				M401 = 0;
			}
			else
			{
				M401 = 1;
			}

			if (M505 > 0)
			{
				M402 = 0;
			}
			else
			{
				M402 = 1;
			}

			if (M401 * M402 > 0)
			{
				M403 = 1;
			}
			else
			{
				M403 = 0;
			}

			if (M107 * M403 * M209 > 0)
			{
				M404 = 1;
			}
			else
			{
				M404 = 0;
			}

			if (M108 > 0)
			{
				M405 = 0;
			}
			else if (M404 > 0)
			{
				M405 = 1;
			}

			if (M1013 * M405 > 0)
			{
				M408 = 1;
			}
			else
			{
				M408 = 0;
			}

			Warmup2W_130KV(ref M408, ref D408, ref D409, ref D410, ref M411);

			if (M1013 > 0)
			{
				D400 = D408;
			}

			if (M1013 > 0)
			{
				D401 = D409;
			}

			if (M1013 > 0)
			{
				D404 = D410;
			}

			if (M1013 * M411 > 0)
			{
				M412 = 1;
			}
			else
			{
				M412 = 0;
			}

			if (M410 + M412 > 0)
			{
				M406 = 1;
			}
			else
			{
				M406 = 0;
			}

			//=========================================================
			//X線ウォームアップ（３Ｗ）
			if (M305 > 0)
			{
				M501 = 0;
			}
			else
			{
				M501 = 1;
			}

			if (M405 > 0)
			{
				M502 = 0;
			}
			else
			{
				M502 = 1;
			}

			if (M501 * M502 > 0)
			{
				M503 = 1;
			}
			else
			{
				M503 = 0;
			}

			if (M107 * M503 * M210 > 0)
			{
				M504 = 1;
			}
			else
			{
				M504 = 0;
			}

			if (M108 > 0)
			{
				M505 = 0;
			}
			else if (M504 > 0)
			{
				M505 = 1;
			}

			if (M1013 * M505 > 0)
			{
				M508 = 1;
			}
			else
			{
				M508 = 0;
			}

			Warmup3W_130KV(ref M508, ref D508, ref D509, ref D510, ref M511);

			if (M1013 > 0)
			{
				D500 = D508;
			}

			if (M1013 > 0)
			{
				D501 = D509;
			}

			if (M1013 > 0)
			{
				D504 = D510;
			}

			if (M1013 * M511 > 0)
			{
				M512 = 1;
			}
			else
			{
				M512 = 0;
			}

			if (M510 + M512 > 0)
			{
				M506 = 1;
			}
			else
			{
				M506 = 0;
			}

			//=========================================================
			//X線管電圧／管電流選択
			if (M305 > 0)
			{
				M601 = 0;
			}
			else
			{
				M601 = 1;
			}

			if (M405 > 0)
			{
				M602 = 0;
			}
			else
			{
				M602 = 1;
			}

			if (M505 > 0)
			{
				M603 = 0;
			}
			else
			{
				M603 = 1;
			}

			//ウォームアップ中でない
			if (M601 * M602 * M603 > 0)
			{
				M604 = 1;
			}
			else
			{
				M604 = 0;
			}

			if (M305 > 0)
			{
				D601 = D300;
			}

			if (M405 > 0)
			{
				D601 = D400;
			}

			if (M505 > 0)
			{
				D601 = D500;
			}

			if (D601 < 20)
			{
				M605 = 1;
			}
			else
			{
				M605 = 0;
			}

			if (M605 > 0)
			{
				M606 = 0;
			}
			else
			{
				M606 = 1;
			}

			if (M604 * M606 > 0)
			{
				M615 = 1;
			}
			else
			{
				M615 = 0;
			}

			if (M615 > 0)
			{
				D601 = D600;
			}

			if (M604 * M605 > 0)
			{
				M614 = 1;
			}
			else
			{
				M614 = 0;
			}

			if (M614 > 0)
			{
				D601 = 20;
			}

			D602 = D601;

			if (M305 > 0)
			{
				D604 = D301;
			}

			if (M405 > 0)
			{
				D604 = D401;
			}

			if (M505 > 0)
			{
				D604 = D501;
			}

			if (M604 > 0)
			{
				D604 = D603;
			}

			D607 = D604;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			if (M939 != 0)
			{
				M616 = 0;
			}
			else
			{
				M616 = 1;
			}
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//=========================================================
			//Ｘ線ＴＶＣＬ制御
			D704 = D705;
			D701 = D602;

			if (D701 < D602)
			{
				M700 = 1;
			}
			else
			{
				M700 = 0;
			}

			if (D602 < D701)
			{
				M701 = 1;
			}
			else
			{
				M701 = 0;
			}

			if (M700 + M701 > 0)
			{
				M702 = 1;
			}
			else
			{
				M702 = 0;
			}

			if (M705 > 0)
			{
				M703 = 0;
			}
			else
			{
				M703 = 1;
			}

			if (M702 * M703 > 0)
			{
				M704 = 1;
			}
			else
			{
				M704 = 0;
			}

			mLogic.iOnDelay(M704, ref T701, 0, ref M705);

			if (M700 * M705 > 0)
			{
				M706 = 1;
			}
			else
			{
				M706 = 0;
			}

			if (M705 * M701 > 0)
			{
				M707 = 1;
			}
			else
			{
				M707 = 0;
			}

			if (M108 > 0)
			{
				D702 = 0;
			}
			else if (M706 > 0 && M707 <= 0)
			{
				D702 = D701 + 1;
			}
			else if (M707 > 0 && M706 <= 0)
			{
				D702 = D701 - 1;
			}
			else
			{
				D702 = D701;
			}

//			XrayTVCL_90KV D702, D704, D706, M722
//
//			If M1012 > 0 Then
//				D709 = D706
//			End If

//			If M1012 * M722 > 0 Then
//				M723 = 1
//			Else
//				M723 = 0
//			End If

			XrayTVCL_130KV_Large(D702, D704, ref D707, ref M724);

			if (M728 > 0)
			{
				D709 = D707;
			}

			if (M1013 * M117 > 0)
			{
				M728 = 1;
			}
			else
			{
				M728 = 0;
			}

			if (M728 * M724 > 0)
			{
				M725 = 1;
			}
			else
			{
				M725 = 0;
			}

			XrayTVCL_130KV_Small(D702, D704, ref D708, ref M726);

			if (M729 > 0)
			{
				D709 = D708;
			}

			if (M117 > 0)
			{
				M730 = 0;
			}
			else
			{
				M730 = 1;
			}

			if (M1013 * M730 > 0)
			{
				M729 = 1;
			}
			else
			{
				M729 = 0;
			}

			if (M729 * M726 > 0)
			{
				M727 = 1;
			}
			else
			{
				M727 = 0;
			}

			if (M723 + M725 + M727 > 0)
			{
				M741 = 1;
			}
			else
			{
				M741 = 0;
			}

			if (M706 + M707 > 0)
			{
				M740 = 1;
			}
			else
			{
				M740 = 0;
			}

			if (M741 > 0)
			{
				M742 = 0;
			}
			else
			{
				M742 = 1;
			}

			if (M742 * M740 > 0)
			{
				M743 = 1;
			}
			else
			{
				M743 = 0;
			}

			if (M743 > 0)
			{
				M710 = 0;
			}
			else if (M741 > 0)
			{
				M710 = 1;
			}

			if (M710 > 0)
			{
				M731 = 0;
			}
			else
			{
				M731 = 1;
			}

			if (M731 > 0)
			{
				D701 = D709;
			}

			D710 = (int)D701;
			D714 = (int)(D701 / rDaV);
			D704 = D607;

			if (D704 < D607)
			{
				M712 = 1;
			}
			else
			{
				M712 = 0;
			}

			if (M710 > 0)
			{
				M711 = 0;
			}
			else
			{
				M711 = 1;
			}

			if (M711 * M712 > 0)
			{
				M713 = 1;
			}
			else
			{
				M713 = 0;
			}

			if (D607 < D704)
			{
				M714 = 1;
			}
			else
			{
				M714 = 0;
			}

			if (M714 + M710 > 0)
			{
				M715 = 1;
			}
			else
			{
				M715 = 0;
			}

			if (M713 + M715 > 0)
			{
				M716 = 1;
			}
			else
			{
				M716 = 0;
			}

			if (M719 > 0)
			{
				M717 = 0;
			}
			else
			{
				M717 = 1;
			}

			if (M716 * M717 > 0)
			{
				M718 = 1;
			}
			else
			{
				M718 = 0;
			}

			mLogic.iOnDelay(M718, ref T702, 0, ref M719);
//			XrayTVCL_90KV D702, D704 + 1, D716, M744
//
//			If M1012 * M744 > 0 Then
//				M745 = 1
//			Else
//				M745 = 0
//			End If

			XrayTVCL_130KV_Large(D702, D704 + 1, ref D717, ref M746);

			if (M728 * M746 > 0)
			{
				M747 = 1;
			}
			else
			{
				M747 = 0;
			}

			XrayTVCL_130KV_Small(D702, D704 + 1, ref D718, ref M748);

			if (M729 * M748 > 0)
			{
				M749 = 1;
			}
			else
			{
				M749 = 0;
			}

			if (M745 + M747 + M749 > 0)
			{
				M750 = 1;
			}
			else
			{
				M750 = 0;
			}

			if (M750 > 0)
			{
				M751 = 0;
			}
			else
			{
				M751 = 1;
			}

			if (M713 * M719 * M751 > 0)
			{
				M720 = 1;
			}
			else
			{
				M720 = 0;
			}

			if (M719 * M715 > 0)
			{
				M721 = 1;
			}
			else
			{
				M721 = 0;
			}

			if (M108 > 0)
			{
				D705 = 0;
			}
			else if (M720 > 0 && M721 <= 0)
			{
				D705 = D704 + 1;
			}
			else if (M721 > 0 && M720 <= 0)
			{
				D705 = D704 - 1;
			}
			else
			{
				D705 = D704;
			}

			D713 = (int)D705;
			if (M1012 > 0)
			{
//				D715 = D705 / rDaA90
			}
			else if (M1013 > 0)
			{
				D715 = (int)(D705 / rDaA130);
			}
			else
			{
				D715 = 0;
			}
//			XrayTVCL_90KV D602, D607, D719, M752
//
//			If M1012 * M752 > 0 Then
//				M753 = 1
//			Else
//				M753 = 0
//			End If

			XrayTVCL_130KV_Large(D602, D607, ref D720, ref M754);

			if (M728 * M754 > 0)
			{
				M755 = 1;
			}
			else
			{
				M755 = 0;
			}

			XrayTVCL_130KV_Small(D602, D607, ref D721, ref M756);

			if (M729 * M756 > 0)
			{
				M757 = 1;
			}
			else
			{
				M757 = 0;
			}

			if (M753 + M755 + M757 > 0)
			{
				M758 = 1;
			}
			else
			{
				M758 = 0;
			}

			//=========================================================
			//Ｘ線管電圧／管電流確認
			//2002-08-21 Shibui
			if (D803 != D600 || D804 != D603)
			{
				D803 = D600;
				D804 = D603;
				M819 = 0;
			}
			else
			{
				M819 = 1;
			}

//			iOnDelay M107, T806, ifAvailTimXOn, M820        'X線ON後10秒
//			iOnDelay M819, T807, ifAvailTimkVmA, M821       '管電圧・管電流変更後

			//アベイラブル用関数を新たに作成した　'changed by 山本　2002-9-6
			mLogic.iAveDelay(M107, ref T806, ModuleCTRLM.ifAvailTimXOn, ref M820);			//X線ON後10秒
			mLogic.iAveDelay(M819, ref T807, ModuleCTRLM.ifAvailTimkVmA, ref M821);			//管電圧・管電流変更後

			if (M821 * M820 * M908 * M207 != 0)
			{
				M807 = 1;
			}
			else
			{
				M807 = 0;
			}

			//=========================================================
			//異常検出
			if (M102 > 0)
			{
				M901 = 0;
			}
			else
			{
				M901 = 1;
			}

			mLogic.iOnDelay(M901, ref T901, 100, ref M902);

			if (M902 > 0)
			{
				M903 = 0;
			}
			else
			{
				M903 = 1;
			}

			if (M100 + M903 > 0)
			{
				M909 = 1;
			}
			else
			{
				M909 = 0;
			}

			if (M901 * M909 > 0)
			{
				M910 = 1;
			}
			else
			{
				M910 = 0;
			}
			if (M100 + M939 != 0)
			{
				M940 = 1;
			}
			else
			{
				M940 = 0;
			}
			if (M910 * M940 != 0)
			{
				M941 = 1;
			}
			else
			{
				M941 = 0;
			}
			if (M911 > 0)
			{
				M912 = 0;
			}
			else if (M941 > 0)
			{
				M912 = 1;
			}
			//インターロック異常
			if (M901 + M912 > 0)
			{
				M913 = 1;
			}
			else
			{
				M913 = 0;
			}

			if (M100 != 0)
			{
				M908 = 1;
			}
			else
			{
				M908 = 0;
			}
			if (M908 != 0)
			{
//			If M908 + M938 Then
				M939 = 1;
			}
			else
			{
				M939 = 0;
			}

//2002-08-30 Shibui
//		    iOnDelay M107, T902, 5000, M914
//
//			If M914 * M915 > 0 Then
//				M916 = 1
//			Else
//				 M916 = 0
//			End If
//
//			iOnDelay M916, T903, 100, M917
//
//			If M917 > 0 Then
//				M918 = 0
//			Else
//				M918 = 1
//			End If
//
//			If M918 + M100 > 0 Then
//				M919 = 1
//			Else
//				M919 = 0
//			End If
//
//			If M916 * M919 > 0 Then
//				M920 = 1
//			Else
//				M920 = 0
//			End If
//
//			If M921 > 0 Then
//				M922 = 0
//			ElseIf M920 > 0 Then
//				M922 = 1
//			End If
//
//			'Ｘ線ＯＮ異常
//			If M916 + M922 > 0 Then
//				M923 = 1
//			Else
//				M923 = 0
//			End If
//
//			If M906 > 0 Then
//				M924 = 0
//			Else
//				M924 = 1
//			End If
//
//			iOnDelay M924, T904, 1000, M925
//			iOnDelay M925, T905, 100, M926
//
//			If M926 > 0 Then
//				M927 = 0
//			Else
//				M927 = 1
//			End If
//
//			If M927 + M100 > 0 Then
//				M928 = 1
//			Else
//				M928 = 0
//			End If
//
//			If M925 * M928 > 0 Then
//				M929 = 1
//			Else
//				 M929 = 0
//			End If
//
//			If M930 > 0 Then
//				M931 = 0
//			ElseIf M929 > 0 Then
//				M931 = 1
//			End If

			//１３０ＫＶ
			if (M1008 > 0)
			{
				M1009 = 0;
			}
			else
			{
				M1009 = 1;
			}
			if (M1009 * M1010 > 0)
			{
				M1013 = 1;
			}
			else
			{
				M1013 = 0;
			}

//2002-08-30 shibui
//			If M1009 * M1011 > 0 Then
//				M1014 = 1
//			Else
//				M1014 = 0
//			End If
//
//			iOnDelay M1014, T1002, 100, M1015
//
//			If M1015 > 0 Then
//				M1016 = 0
//			Else
//				M1016 = 1
//			End If
//
//			If M1016 + M100 > 0 Then
//				M1017 = 1
//			Else
//				M1017 = 0
//			End If
//
//			If M1014 * M1017 > 0 Then
//				M1018 = 1
//			Else
//				M1018 = 0
//			End If
//
//			If M1019 > 0 Then
//				M1020 = 0
//			ElseIf M1018 > 0 Then
//				M1020 = 1
//			End If
//
//			iOnDelay M108, T1201, 5000, M1201
//
//			If M1201 * M908 > 0 Then
//				M1202 = 1
//			Else
//				M1202 = 0
//			End If
//
//			iOnDelay M1202, T1202, 100, M1203
//
//			If M1203 > 0 Then
//				M1204 = 0
//			Else
//				M1204 = 1
//			End If
//
//			If M1204 + M100 > 0 Then
//				M1205 = 1
//			Else
//				M1205 = 0
//			End If
//
//			If M1202 * M1207 > 0 Then
//				M1206 = 1
//			Else
//				M1206 = 0
//			End If
//
//			If M1207 > 0 Then
//				M1208 = 0
//			ElseIf M1206 > 0 Then
//				M1208 = 1
//			End If
//
//			'X線OFF異常
//			If M1202 + M1208 > 0 Then
//				M1209 = 1
//			Else
//				M1209 = 0
//			End If

			//異常発生中
			if (M913 + M923 + M932 + M1007 + M1125 + M1109 + M1021 + M1029 + M1116 + M1123 + M1209 > 0)
			{
				M933 = 1;
			}
			else
			{
				M933 = 0;
			}

			//=========================================================
			//出力

//2002-08-20 Shibui
//'2000-09-18 T.Shbiui
//			If M101 = 0 And M933 = 0 And gConmXrayOff = 1 And D600 > 0 And D603 > 0 And M134 = 0 Then
			//ﾘﾓｰﾄX線ON可能
			if (M101 == 0 && M933 == 0 && D600 > 0 && D603 > 0 && M134 == 0)
			{
				ModuleIOM.gDioDo03 = 1;
				ModuleIOM.gDioDo04 = 1;
			}
			else
			{
				ModuleIOM.gDioDo03 = 0;
				ModuleIOM.gDioDo04 = 0;
			}


//2002-08-20 Shibui DIOで行う
//			gAioDo00 = M107				'Ｘ線ＯＮ指令
			ModuleIOM.gDioDo10 = M107;	//Ｘ線ＯＮ指令

			if (M134 == 1)
			{
				ModuleIOM.gDioDo11 = 0;		//Ｘ線ＯＦＦ指令 OFF
			}
			else
			{
				ModuleIOM.gDioDo11 = 1;		//Ｘ線ＯＦＦ指令 ON
			}

//2000-09-01 T.Shbiui
// 1999-11-19 T.Shibui
//			gDioDo03 = M129     'Ｘ線ＯＮ指令
//			gDioDo04 = M107     'Ｘ線ＯＮ指令

//2002-08-20 Shibui DIOで行う
//			gAioDo01 = M117     '焦点切り替え（ON:大焦点/OFF:小焦点）
			ModuleIOM.gDioDo05 = M117;

// 1999-11-19 T.Shibui
//			gAioDo02 = M1126    '発生器異常リセット
//			gAioAo00 = D714     '管電圧

// 1999-11-19 T.Shibui ->
			ModuleIOM.gDioDo27 = 1;		//kV ENABLE

			//管電圧設定ﾃﾞｰﾀ
			int v = 0;
			v = D714;
			v = v * 100;

			if (v >= 6600)
			{
				ModuleIOM.gDioDo26 = 1;
				v = v - 6600;
			}
			else
			{
				ModuleIOM.gDioDo26 = 0;
			}
			if (v >= 3300)
			{
				ModuleIOM.gDioDo25 = 1;
				v = v - 3300;
			}
			else
			{
				ModuleIOM.gDioDo25 = 0;
			}
			if (v >= 1650)
			{
				ModuleIOM.gDioDo24 = 1;
				v = v - 1650;
			}
			else
			{
				ModuleIOM.gDioDo24 = 0;
			}
			if (v >= 825)
			{
				ModuleIOM.gDioDo23 = 1;
				v = v - 825;
			}
			else
			{
				ModuleIOM.gDioDo23 = 0;
			}
			if (v >= 410)
			{
				ModuleIOM.gDioDo22 = 1;
				v = v - 410;
			}
			else
			{
				ModuleIOM.gDioDo22 = 0;
			}
			if (v >= 200)
			{
				ModuleIOM.gDioDo21 = 1;
				v = v - 200;
			}
			else
			{
				ModuleIOM.gDioDo21 = 0;
			}
			if (v >= 100)
			{
				ModuleIOM.gDioDo20 = 1;
			}
			else
			{
				ModuleIOM.gDioDo20 = 0;
			}
//			gAioAo01 = D715     '管電流
			ModuleIOM.gDioDo37 = 1;
			//管電流設定ﾃﾞｰﾀ
			int c = 0;
			if (D714 == 0)
			{
				c = 0;
			}
			else if (M730 != 0)
			{
				c = D715 / (8000 / D714) * 512;
			}
			else if (D714 <= 32)
			{
				c = D715 / 500 * 512;
			}
			else
			{
				c = D715 / (16000 / D714) * 512;
			}
			if (c >= 256)
			{
				ModuleIOM.gDioDo36 = 1;
				c = c - 256;
			}
			else
			{
				ModuleIOM.gDioDo36 = 0;
			}
			if (c >= 128)
			{
				ModuleIOM.gDioDo35 = 1;
				c = c - 128;
			}
			else
			{
				ModuleIOM.gDioDo35 = 0;
			}
			if (c >= 64)
			{
				ModuleIOM.gDioDo34 = 1;
				c = c - 64;
			}
			else
			{
				ModuleIOM.gDioDo34 = 0;
			}
			if (c >= 32)
			{
				ModuleIOM.gDioDo33 = 1;
				c = c - 32;
			}
			else
			{
				ModuleIOM.gDioDo33 = 0;
			}
			if (c >= 16)
			{
				ModuleIOM.gDioDo32 = 1;
				c = c - 16;
			}
			else
			{
				ModuleIOM.gDioDo32 = 0;
			}
			if (c >= 8)
			{
				ModuleIOM.gDioDo31 = 1;
				c = c - 8;
			}
			else
			{
				ModuleIOM.gDioDo31 = 0;
			}
			if (c >= 4)
			{
				ModuleIOM.gDioDo30 = 1;
			}
			else
			{
				ModuleIOM.gDioDo30 = 0;
			}
// <- 1999-11-19 T.Shibui

//2002-08-20 Shibui
//'1999-10-21 T.Shibui
//			If M120 = 1 Then
//				gDioDo00 = 0     'I.I.視野切り替え
//			Else
//				gDioDo00 = 1     'I.I.視野切り替え
//			End If

//2002-08-20 Shibui
//'1999-12-07 T.Shibui
//			ipPow = M214
//			gDioDo00 = M120     'I.I.視野切り替え
//			gDioDo01 = M214     'I.I.電源ON
//			gDioDo02 = M908     'X線時間計ON

// 1999-11-19 T.Shibui
//			gDioDo04 = 1        'X線ON許可
			ModuleIOM.gDioDo05 = M117;				//X線焦点切り替え

//			gXraymFldSize = M120        'I.I.視野切り替え
			ModuleXRAYM.gXraymOffDateRst = M123;
//			gXraymType90 = M1012        'Ｘ線管９０ＫＶ
//			gXraymType130 = M1013       'Ｘ線管１３０ＫＶ
			ModuleXRAYM.gXraymWarmup = M207;		//ウォームアップ不要
			ModuleXRAYM.gXraymWarmup2D = M208;		//ウォームアップ2D
			ModuleXRAYM.gXraymWarmup2W = M209;		//ウォームアップ2W
			ModuleXRAYM.gXraymWarmup3W = M210;		//ウォームアップ3W
			ModuleXRAYM.gXraymWarmupOn = M212;		//ウォームアップ中
			ModuleXRAYM.gXraymWarmupEnd = M118;
			ModuleXRAYM.gXraymWarmupTimer = D203;	//ウォームアップ残時間
			ModuleXRAYM.gXraymOffDate = D201;		//X線OFF日時
			ModuleXRAYM.gXraymForcus = M117;		//焦点
			ModuleXRAYM.gXraymVolt = D614;			//管電圧
			ModuleXRAYM.gXraymAmp = D615;			//管電流
			ModuleXRAYM.gXraymWatt = D616;			//電力

//2002-08-21 Shibui
//' 1999-11-19 T.Shibui
//'			gxraymFine = M619           'FINE
//			 gxraymFine = M617           'FINE

			ModuleXRAYM.gXraymOn = M107;				//X線ON
			ModuleXRAYM.gXraymTVCL = M758;				//TVCL制御中
			ModuleXRAYM.gXraymInterlockErr = M913;		//インターロック異常
			ModuleXRAYM.gXraymInterlockErrDsp = M912;	//インターロック異常表示

//2002-08-30 Shibui
//'2001-11-06 (NSD)Shibui ｺﾒﾝﾄ解除
//			gXraymOnErr = M923           'X線ON異常
//			gXraymOnErrDsp = M922        'X線ON異常表示
//
//			gXraymOffErr = M1209         'X線OFF異常
//			gXraymOffErrDsp = M1208      'X線OFF異常表示

//2002-08-20 Shibui
//			gXraymOnRyErr = M932         'X線ONリレー異常
//			gXraymOnRyErrDsp = M931      'X線ONリレー異常表示
//			gXraymStandbyErr = M1007     'スタンバイ異常
//			gXraymStandbyErrDsp = M1006  'スタンバイ異常表示
//			gXraymNoSelectErr = M1021    '未選択異常
//			gXraymNoSelectErrDsp = M1020 '未選択異常表示
//			gXraymOverlapErr = M1029     '重複選択異常
//			gXraymOverlapErrDsp = M1028  '重複選択異常表示
//			gXraymUnitErr = M1109        'X線発生器異常
//			gXraymUnitErrDsp = M1108     'X線発生器異常表示
//			gXraymVoltErr = M1116        '管電圧異常
//			gXraymVoltErrDsp = M1115     '管電圧異常表示
//			gXraymAmpErr = M1123         '管電流異常
//			gXraymAmpErrDsp = M1122      '管電流異常表示
//			gXraymPermitWarmup = M1124   'エージング許可
			ModuleXRAYM.gXraymPermitWarmup = 1;		//エージング許可
			ModuleXRAYM.gXraymErr = M933;			//異常発生中

			//1999-09-30 T.Shibui
			ModuleXRAYM.gXAvail = M807;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vNowTime"></param>
		public static void XrayOffTimeWrite(DateTime vNowTime)
		{
			string sNowTime = null;
			sNowTime = vNowTime.ToString("yyyy/mm/dd hh:mm");

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			On Error Resume Next

			Open "C:\CT\INI\xraym.off" For Output As #1
			Print #1, sNowTime
			Close #1
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			StreamWriter sw = null;

			try
			{
				sw = new StreamWriter(@"C:\CT\\INI\xraym.off");
				sw.WriteLine(sNowTime);
			}
			catch
			{
				if (sw != null) sw.Close();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void XrayOffTimeRead()
		{
			string sNowTime;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			string sYear;
			string sMonth;
			string sDay;
			string sHour;
			string sMin;
			int iYear;
			int iMonth;
			int iDay;
			int iHour;
			int iMin;
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			// VB6 の DateSerial(0, 0, 0) は、1999/11/30
			DateTime vNowTime = new DateTime(1999, 11, 30, 0, 0, 0);

			StreamReader sr = null;

			try
			{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				FileSystem.FileOpen(1, "C:\\CT\\INI\\xraym.off", OpenMode.Input);
				sNowTime = FileSystem.LineInput(1);
				FileSystem.FileClose(1);
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				sr = new StreamReader(@"C:\CT\\INI\xraym.off");
				sNowTime = sr.ReadLine();

				//保存フォーマット
				//1234567890123456
				//yyyy/mm/dd hh:mm
				if (sNowTime.Length == 16)
				{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
					sYear = Mid$(sNowTime, 1, 4)
					iYear = val(sYear)
					sMonth = Mid$(sNowTime, 6, 2)
					iMonth = val(sMonth)
					sDay = Mid$(sNowTime, 9, 2)
					iDay = val(sDay)
					sHour = Mid$(sNowTime, 12, 2)
					iHour = val(sHour)
					sMin = Mid$(sNowTime, 15, 2)
					iMin = val(sMin)
					vNowTime = System.Date.FromOADate(DateAndTime.DateSerial(iYear, iMonth, iDay).ToOADate() + DateAndTime.TimeSerial(iHour, iMin, 0).ToOADate());
				} else {
					vNowTime = System.Date.FromOADate(DateAndTime.DateSerial(0, 0, 0).ToOADate() + DateAndTime.TimeSerial(0, 0, 0).ToOADate());
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

					DateTime.TryParse(sNowTime, out vNowTime);
				}
			}
			catch
			{
				if (sr != null) sr.Close();
			}

			XrayOffTimeReadTime = vNowTime;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="f"></param>
		/// <param name="v"></param>
		/// <param name="i"></param>
		/// <param name="t"></param>
		/// <param name="e"></param>
		public static void Warmup2D_130KV(ref int f, ref int v, ref int i, ref int t, ref int e)
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Static iSec As Integer
			Static st As Date
			Static stf As Integer
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			if (f != 0)
			{
				if (Warmup2D_130KV_stf == 0)
				{
					Warmup2D_130KV_st = DateTime.Now;
					Warmup2D_130KV_stf = 1;
				}
				Warmup2D_130KV_iSec = (int)((DateTime.Now - Warmup2D_130KV_st).TotalSeconds);
			}
			else
			{
				Warmup2D_130KV_stf = 0;
				Warmup2D_130KV_iSec = 0;
			}

			if (f > 0)
			{
				if (Warmup2D_130KV_iSec < 1 * 60)			//1
				{
					v = 27;
					i = 0;
				}
				else if (Warmup2D_130KV_iSec < 2 * 60)		//2
				{
					v = 56;
					i = 20;
				}
				else if (Warmup2D_130KV_iSec < 3 * 60)		//3
				{
					v = 74;
					i = 40;
				}
				else if (Warmup2D_130KV_iSec < 4 * 60)		//4
				{
					v = 86;
					i = 57;
				}
				else if (Warmup2D_130KV_iSec < 5 * 60)		//5
				{
					v = 94;
					i = 73;
				}
				else if (Warmup2D_130KV_iSec < 6 * 60)		//6
				{
					v = 102;
					i = 80;
				}
				else if (Warmup2D_130KV_iSec < 7 * 60)		//7
				{
					v = 110;
					i = 93;
				}
				else if (Warmup2D_130KV_iSec < 8 * 60)		//8
				{
					v = 116;
					i = 101;
				}
				else if (Warmup2D_130KV_iSec < 9 * 60)		//9
				{
					v = 120;
					i = 106;
				}
				else if (Warmup2D_130KV_iSec < 10 * 60)	//10
				{
					v = 122;
					i = 111;
				}
				else if (Warmup2D_130KV_iSec < 11 * 60)	//11
				{
					v = 124;
					i = 115;
				}
				else if (Warmup2D_130KV_iSec < 12 * 60)	//12
				{
					v = 126;
					i = 118;
				}
				else if (Warmup2D_130KV_iSec < 13 * 60)	//13
				{
					v = 128;
					i = 120;
				}
				else if (Warmup2D_130KV_iSec < 14 * 60)	//14
				{
					v = 130;
					i = 122;
				}
				else if (Warmup2D_130KV_iSec < 15 * 60)	//15
				{
					v = 130;
					i = 123;
				}
				else						//16
				{
					v = 130;
					i = 123;
				}
				if (Warmup2D_130KV_iSec > 16 * 60)
				{
					e = 1;
				}
				else
				{
					e = 0;
				}
				t = 16 * 60 - Warmup2D_130KV_iSec;
				if (t <= 0)
				{
					t = 1;
				}
			}
			else
			{
				v = 0;
				i = 0;
				t = 0;
				e = 0;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="f"></param>
		/// <param name="v"></param>
		/// <param name="i"></param>
		/// <param name="t"></param>
		/// <param name="e"></param>
		public static void Warmup2W_130KV(ref int f, ref int v, ref int i, ref int t, ref int e)
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Static iSec As Single
			Static st As Date
			Static stf As Integer
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			const double bs = 127.5;
			if (f != 0)
			{
				if (Warmup2W_130KV_stf == 0)
				{
					Warmup2W_130KV_st = DateTime.Now;
					Warmup2W_130KV_stf = 1;
				}
				Warmup2W_130KV_iSec = (float)(DateTime.Now - Warmup2W_130KV_st).TotalSeconds;
			}
			else
			{
				Warmup2W_130KV_stf = 0;
				Warmup2W_130KV_iSec = 0;
			}

			if (f > 0)
			{

				if (Warmup2W_130KV_iSec < 1 * bs)				//1
				{
					v = 27;
					i = 0;
				}
				else if (Warmup2W_130KV_iSec < 2 * bs)			//2
				{
					v = 56;
					i = 20;
				}
				else if (Warmup2W_130KV_iSec < 3 * bs)			//3
				{
					v = 74;
					i = 40;
				}
				else if (Warmup2W_130KV_iSec < 4 * bs)			//4
				{
					v = 86;
					i = 57;
				}
				else if (Warmup2W_130KV_iSec < 5 * bs)			//5
				{
					v = 94;
					i = 73;
				}
				else if (Warmup2W_130KV_iSec < 6 * bs)			//6
				{
					v = 102;
					i = 80;
				}
				else if (Warmup2W_130KV_iSec < 7 * bs)			//7
				{
					v = 110;
					i = 93;
				}
				else if (Warmup2W_130KV_iSec < 8 * bs)			//8
				{
					v = 116;
					i = 101;
				}
				else if (Warmup2W_130KV_iSec < 9 * bs)			//9
				{
					v = 120;
					i = 106;
				}
				else if (Warmup2W_130KV_iSec < 10 * bs)			//10
				{
					v = 122;
					i = 111;
				}
				else if (Warmup2W_130KV_iSec < 11 * bs)			//11
				{
					v = 124;
					i = 115;
				}
				else if (Warmup2W_130KV_iSec < 12 * bs)			//12
				{
					v = 126;
					i = 118;
				}
				else if (Warmup2W_130KV_iSec < 13 * bs)			//13
				{
					v = 128;
					i = 120;
				}
				else if (Warmup2W_130KV_iSec < 14 * bs)			//14
				{
					v = 130;
					i = 122;
				}
				else if (Warmup2W_130KV_iSec < 15 * bs)			//15
				{
					v = 130;
					i = 123;
				}
				else													//16
				{
					v = 130;
					i = 123;
				}
				if (Warmup2W_130KV_iSec > 16 * bs)
				{
					e = 1;
				}
				else
				{
					e = 0;
				}
				t = (int)(16 * bs - Warmup2W_130KV_iSec);
				if (t <= 0)
				{
					t = 1;
				}
			}
			else
			{
				v = 0;
				i = 0;
				t = 0;
				e = 0;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="f"></param>
		/// <param name="v"></param>
		/// <param name="i"></param>
		/// <param name="t"></param>
		/// <param name="e"></param>
		public static void Warmup3W_130KV(ref int f, ref int v, ref int i, ref int t, ref int e)
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Static iSec As Single
			Static st As Date
			Static stf As Integer
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			const double bs = 262.5;
			if (f != 0)
			{
				if (Warmup3W_130KV_stf == 0)
				{
					Warmup3W_130KV_st = DateTime.Now;
					Warmup3W_130KV_stf = 1;
				}
				Warmup3W_130KV_iSec = (float)(DateTime.Now - Warmup3W_130KV_st).TotalSeconds;
			}
			else
			{
				Warmup3W_130KV_stf = 0;
				Warmup3W_130KV_iSec = 0;
			}

			if (f > 0)
			{
				if (Warmup3W_130KV_iSec < 1 * bs)				//1
				{
					v = 27;
					i = 0;
				}
				else if (Warmup3W_130KV_iSec < 2 * bs)			//2
				{
					v = 56;
					i = 20;
				}
				else if (Warmup3W_130KV_iSec < 3 * bs)			//3
				{
					v = 74;
					i = 40;
				}
				else if (Warmup3W_130KV_iSec < 4 * bs)			//4
				{
					v = 86;
					i = 57;
				}
				else if (Warmup3W_130KV_iSec < 5 * bs)			//5
				{
					v = 94;
					i = 73;
				}
				else if (Warmup3W_130KV_iSec < 6 * bs)			//6
				{
					v = 102;
					i = 80;
				}
				else if (Warmup3W_130KV_iSec < 7 * bs)			//7
				{
					v = 110;
					i = 93;

				}
				else if (Warmup3W_130KV_iSec < 8 * bs)			//8
				{
					v = 116;
					i = 101;
				}
				else if (Warmup3W_130KV_iSec < 9 * bs)			//9
				{
					v = 120;
					i = 106;
				}
				else if (Warmup3W_130KV_iSec < 10 * bs)			//10
				{
					v = 122;
					i = 111;
				}
				else if (Warmup3W_130KV_iSec < 11 * bs)			//11
				{
					v = 124;
					i = 115;
				}
				else if (Warmup3W_130KV_iSec < 12 * bs)			//12
				{
					v = 126;
					i = 118;
				}
				else if (Warmup3W_130KV_iSec < 13 * bs)			//13
				{
					v = 128;
					i = 120;
				}
				else if (Warmup3W_130KV_iSec < 14 * bs)			//14
				{
					v = 130;
					i = 122;
				}
				else if (Warmup3W_130KV_iSec < 15 * bs)			//15
				{
					v = 130;
					i = 123;
				}
				else													//16
				{
					v = 130;
					i = 123;
				}
				if (Warmup3W_130KV_iSec > 16 * bs)
				{
					e = 1;
				}
				else
				{
					e = 0;
				}
				t = (int)(16 * bs - Warmup3W_130KV_iSec);
				if (t <= 0)
				{
					t = 1;
				}
			}
			else
			{
				v = 0;
				i = 0;
				t = 0;
				e = 0;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iv"></param>
		/// <param name="ii"></param>
		/// <param name="ov"></param>
		/// <param name="er"></param>
		public static void XrayTVCL_130KV_Large(float iv, float ii, ref int ov, ref int er)
		{
			float cv = 0;
			float ci = 0;
			int il = 0;

			cv = iv;
			ci = ii;
			if (cv <= 25)
			{
				ov = (int)iv;
				er = 0;
			}
			else if (cv <= 32)
			{
				il = 500;

				if (ci <= il)
				{
					ov = (int)iv;
					er = 0;
				}
				else
				{
					er = 1;
				}
			}
			else
			{
				il = (int)(16000 / cv);

				if (ci <= il)
				{
					ov = (int)iv;
					er = 0;
				}
				else
				{
					er = 1;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iv"></param>
		/// <param name="ii"></param>
		public static void XrayTVCL_130KV_Large_i(float iv, ref float ii)
		{
			float cv = 0;
			float ci = 0;
			int il = 0;

			cv = iv;
			ci = ii;

			if (cv < 25)
			{
			}
			else if (cv <= 32)
			{
				il = 500;

				if (ci <= il)
				{
				}
				else
				{
					ii = il;
				}
			}
			else
			{
				il = (int)(16000 / cv);

				if (ci <= il)
				{
				}
				else
				{
					ii = il;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iv"></param>
		/// <param name="ii"></param>
		/// <param name="ov"></param>
		/// <param name="er"></param>
		public static void XrayTVCL_130KV_Small(float iv, float ii, ref int ov, ref int er)
		{
			float cv = 0;
			float ci = 0;
			int il = 0;

			cv = iv;
			ci = ii;

			if (cv <= 25)
			{
				ov = (int)iv;
				er = 0;
			}
			else
			{
				il = (int)(8000 / cv);

				if (ci <= il)
				{
					ov = (int)iv;
					er = 0;
				}
				else
				{
					er = 1;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iv"></param>
		/// <param name="ii"></param>
		public static void XrayTVCL_130KV_Small_i(float iv, ref float ii)
		{
			float cv = 0;
			float ci = 0;
			int il = 0;

			cv = iv;
			ci = ii;

			if (cv < 25)
			{
			}
			else
			{
				il = (int)(8000 / cv);

				if (ci <= il)
				{
				}
				else
				{
					ii = il;
				}
			}
		}
	}
}
