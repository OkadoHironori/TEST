using System;

namespace XrayCtrl
{
	internal static class ModuleXRAYM
	{

//2000-03-06 T.Shibui
		//通信用変数
		public static float ComkV = 0;
		public static float CommA = 0;
		public static string ComS = string.Empty;
		public static int ComXon = 0;
		public static int ComXoff = 0;
		public static string ComI = string.Empty;
		public static int ComInterLock = 0;
		public static int ComStandby = 0;
		public static int ComReady = 0;
		public static int ComErr = 0;

		public static int ComOutXOn = 0;
		public static string ComOutkV = string.Empty;
		public static string ComOutmA = string.Empty;

		//cCmdクラスのプロパティの実体
		public static int fXraymForcus = 0;				//焦点
		public static int fXraymVolt = 0;				//管電圧
		public static int fXraymAmp = 0;				//管電流
		public static int fXraymOn = 0;					//X線ON
		public static int fXraymOffDateRst = 0;			//X線OFF日時強制更新
		public static int fXraymInterlockErrRst = 0;	//インターロック異常表示解除
		public static int fXraymOnErrRst = 0;			//X線ON異常表示解除
		public static int fXraymOffErrRst = 0;			//X線OFF異常表示解除
		//Public fXraymOnRyErrRst      As Integer  'X線ONリレー異常表示解除
		//Public fXraymStandbyErrRst   As Integer  'スタンバイ異常表示解除
		//Public fXraymNoSelectErrRst  As Integer  '未選択異常表示解除
		//Public fXraymOverlapErrRst   As Integer  '重複選択異常表示解除
		public static int fXraymUnitErrRst = 0;			//X線発生器異常表示解除
		public static int fXraymVoltErrRst = 0;			//管電圧異常表示解除
		public static int fXraymAmpErrRst = 0;			//管電流異常表示解除
		//Public fXraymFldSize         As Integer  'I.I.視野切り替え
		//Public fXraymFldPower        As Integer  'I.I.電源
		public static int fXraymEmergency = 0;			//異常停止

		//cStsクラスのプロパティの実体
		//Public gXraymFldSize         As Integer  'I.I.視野切り替え
		//Public gXraymType90          As Integer  'Ｘ線管９０ＫＶ
		//Public gXraymType130         As Integer  'Ｘ線管１３０ＫＶ
		public static int gXraymWarmup = 0;				//ウォームアップ不要
		public static int gXraymWarmup2D = 0;			//ウォームアップ2D
		public static int gXraymWarmup2W = 0;			//ウォームアップ2W
		public static int gXraymWarmup3W = 0;			//ウォームアップ3W
		public static int gXraymWarmupOn = 0;			//ウォームアップ中
		public static int gXraymWarmupEnd = 0;			//ウォームアップ完
		public static int gXraymWarmupTimer = 0;		//ウォームアップ残時間
		public static int gXraymOffDateRst = 0;			//X線OFF日時強制更新
		public static DateTime gXraymOffDate;			//X線OFF日時
		public static int gXraymForcus = 0;				//焦点

//1999-11-10 T.Shibui
		public static float gXraymVolt = 0;				//管電圧
		public static float gXraymAmp = 0;				//管電流
		public static float gXraymWatt = 0;				//電力

		public static int gxraymFine = 0;				//ファイン表示
		public static int gXraymOn = 0;					//X線ON
		public static int gXraymTVCL = 0;				//TVCL制御中
		public static int gXraymInterlockErr = 0;		//インターロック異常
		public static int gXraymInterlockErrDsp = 0;	//インターロック異常表示
		public static int gXraymOnErr = 0;				//X線ON異常
		public static int gXraymOnErrDsp = 0;			//X線ON異常表示
		public static int gXraymOffErr = 0;				//X線OFF異常
		public static int gXraymOffErrDsp = 0;			//X線OFF異常表示
		//Public gXraymOnRyErr         As Integer  'X線ONリレー異常
		//Public gXraymOnRyErrDsp      As Integer  'X線ONリレー異常表示
		//Public gXraymStandbyErr      As Integer  'スタンバイ異常
		//Public gXraymStandbyErrDsp   As Integer  'スタンバイ異常表示
		//Public gXraymNoSelectErr     As Integer  '未選択異常
		//Public gXraymNoSelectErrDsp  As Integer  '未選択異常表示
		//Public gXraymOverlapErr      As Integer  '重複選択異常
		//Public gXraymOverlapErrDsp   As Integer  '重複選択異常表示
		public static int gXraymUnitErr = 0;			//X線発生器異常
		public static int gXraymUnitErrDsp = 0;			//X線発生器異常表示
		//Public gXraymVoltErr         As Integer  '管電圧異常
		//Public gXraymVoltErrDsp      As Integer  '管電圧異常表示
		//Public gXraymAmpErr          As Integer  '管電流異常
		//Public gXraymAmpErrDsp       As Integer  '管電流異常表示
		public static int gXraymErr = 0;				//異常発生中
		public static int gXraymPermitWarmup = 0;		//ﾌﾟﾘﾋｰﾄ完了

//1999-09-30 T.Shibui
		public static int gXAvail = 0;					//アベイラブル

		//内部
		//Private D200 As Date       'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D201 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D202 As Date       'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private sD202 As Single    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private sD2021 As Single   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private sD2022 As Single   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private sD2023 As Single   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D203 As Long       'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D300 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D301 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D304 As Long       'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D305 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D306 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D307 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D308 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D309 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D310 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D400 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D401 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D404 As Long       'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D405 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D406 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D407 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D408 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D409 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D410 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D500 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D501 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D504 As Long       'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D505 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D506 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D507 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D508 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D509 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D510 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		private static float D600 = 0;
		private static float D601 = 0;
		//Private D602 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		private static float D603 = 0;
		//Private D604 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D605 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D606 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D607 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01

//1999-11-10 T.Shibui
		private static float D608 = 0;
		private static float D609 = 0;
		private static float D610 = 0;
		private static float D611 = 0;

		//Private D612 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
//1999-11-10 T.Shibui
		private static float D613 = 0;
		private static float D614 = 0;
		private static float D615 = 0;
		private static float D616 = 0;
		//Private D701 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D702 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D704 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D705 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01

		//Private D706 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D707 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D708 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D709 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D710 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D711 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D712 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D713 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		private static float D714 = 0;
		private static float D715 = 0;
		//Private D716 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D717 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D718 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D719 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D720 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D721 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01

//2000-02-28 T.Shibui
		private static float D722 = 0;
		private static float D723 = 0;

		//Private D801 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private D802 As Single     'v11.5未使用なので削除 by 間々田 2006/08/01

//2000-02-28 T.Shibui
		private static float D803 = 0;
		private static float D804 = 0;

		//Private i102 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private i103 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private i907 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private i904 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private i1101 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private i1008 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private i1010 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01

		private static int M100 = 0;
		private static int M101 = 0;
		private static int M102 = 0;
		//Private M103 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M104 = 0;
		private static int M105 = 0;
		private static int M106 = 0;
		private static int M107 = 0;
		private static int M108 = 0;
		//Private M109 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M110 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M111 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M112 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M113 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M114 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M115 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M116 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M117 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M118 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M119 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M120 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M121 = 0;
		//Private M122 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M123 = 0;
		//Private M124 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M125 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M126 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01

//1999-12-07 T.Shibui
		//Private M127 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M128 = 0;
		//Private M129 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M130 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01

		//Private M200 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M201 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M202 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M203 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M204 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M205 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M206 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M207 = 0;
		//Private M208 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M209 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M210 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M211 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M212 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M213 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M214 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M215 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M216 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01

//1999-12-07 T.Shibui
		//Private M217 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01

		//Private M300 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M301 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M302 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M303 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M304 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M305 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M306 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M307 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M308 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M309 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M310 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M311 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M312 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M400 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M401 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M402 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M403 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M404 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M405 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M406 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M407 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M408 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M409 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M410 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M411 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M412 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M500 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M501 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M502 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M503 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M504 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M505 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M506 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M507 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M508 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M509 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M510 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M511 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M512 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M600 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M601 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M602 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M603 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M604 = 0;
		private static int M605 = 0;
		private static int M606 = 0;
		//Private M607 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M608 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M609 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M610 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M611 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M612 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M613 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M614 = 0;
		private static int M615 = 0;
		private static int M616 = 0;
		//Private M617 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M618 = 0;
		private static int M619 = 0;
		//Private M700 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M701 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M702 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M703 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M704 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M705 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M706 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M707 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M708 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M709 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M710 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M711 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M712 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M713 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M714 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M715 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M716 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M717 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M718 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M719 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M720 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M721 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M722 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M723 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M724 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M725 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M726 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M727 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M728 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M729 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M730 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M731 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M732 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M733 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M734 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M735 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M736 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M737 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M738 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M739 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M740 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M741 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M742 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M743 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M744 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M745 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M746 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M747 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M748 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M749 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M750 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M751 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M752 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M753 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M754 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M755 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M756 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M757 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M758 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M801 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M802 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M803 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M804 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M805 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M806 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M807 = 0;
		//Private M808 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M809 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M810 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M811 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M812 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M813 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M814 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M815 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M816 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01

//1999-11-10 T.Shibui
		//Private M817 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M818 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01

//2000-02-28 T.Shibui
		private static int M819 = 0;
		private static int M820 = 0;
		private static int M821 = 0;

		//Private M900 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M901 = 0;
		private static int M902 = 0;
		private static int M903 = 0;
		//Private M904 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M905 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M906 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M907 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M908 = 0;
		private static int M909 = 0;
		private static int M910 = 0;
		private static int M911 = 0;
		private static int M912 = 0;
		private static int M913 = 0;
		//Private M914 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M915 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M916 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M917 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M918 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M919 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M920 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M921 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M922 = 0;
		private static int M923 = 0;
		//Private M924 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M925 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M926 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M927 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M928 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M929 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M930 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M931 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M932 = 0;
		private static int M933 = 0;
		//Private M934 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M935 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M936 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M937 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M938 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M939 = 0;
		private static int M940 = 0;
		private static int M941 = 0;

//1999-11-25 T.Shibui
		//Private M942 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M943 As Integer    'v11.5未使用なので削除 by 間々田 2006/08/01

		//Private M1000 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1001 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1002 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1003 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1004 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1005 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1006 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M1007 = 0;
		//Private M1008 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1009 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1010 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1011 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1012 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1013 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1014 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1015 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1016 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1017 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1018 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1019 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1020 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1021 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1022 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1023 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1024 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1025 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1026 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1027 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1028 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1029 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1100 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1101 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M1102 = 0;
		private static int M1103 = 0;
		private static int M1104 = 0;
		private static int M1105 = 0;
		private static int M1106 = 0;
		private static int M1107 = 0;
		private static int M1108 = 0;
		private static int M1109 = 0;
		//Private M1110 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1111 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1112 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1113 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1114 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1115 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M1116 = 0;
		//Private M1117 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1118 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1119 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1120 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1121 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private M1122 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M1123 = 0;
		//Private M1124 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01
		private static int M1125 = 0;
		//Private M1126 As Integer   'v11.5未使用なので削除 by 間々田 2006/08/01

		private static int M1201 = 0;
		private static int M1202 = 0;
		private static int M1203 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M1204 = 0;
//		private static int M1205 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M1206 = 0;
		private static int M1207 = 0;
		private static int M1208 = 0;
		private static int M1209 = 0;
		//Private XrayOffTimeReadTime As Date    'v11.5未使用なので削除 by 間々田 2006/08/01

		//１ショットタイマー番号割付
		//Private P101 As PulseType  'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private P102 As PulseType  'v11.5未使用なので削除 by 間々田 2006/08/01

//1999-12-07 T.Shibui
		//Private P103 As PulseType  'v11.5未使用なので削除 by 間々田 2006/08/01

//2000-03-07 T.Shibui
		//Private P104 As PulseType  'v11.5未使用なので削除 by 間々田 2006/08/01

		//ＯＮディレータイマー番号割付
		//Private T301 As DelayType  'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T302 As DelayType  'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T401 As DelayType  'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T402 As DelayType  'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T501 As DelayType  'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T502 As DelayType  'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T701 As DelayType  'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T702 As DelayType  'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T801 As DelayType  'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T802 As DelayType  'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T803 As DelayType  'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T804 As DelayType  'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T805 As DelayType  'v11.5未使用なので削除 by 間々田 2006/08/01

//2000-02-28 T.Shibui
		private static mLogic.DelayType T806;
		private static mLogic.DelayType T807;

		private static mLogic.DelayType T901;
		//Private T902 As DelayType  'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T903 As DelayType  'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T904 As DelayType  'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T905 As DelayType  'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T1001 As DelayType 'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T1002 As DelayType 'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T1003 As DelayType 'v11.5未使用なので削除 by 間々田 2006/08/01
		private static mLogic.DelayType T1101;
		//Private T1102 As DelayType 'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T1103 As DelayType 'v11.5未使用なので削除 by 間々田 2006/08/01
		//Private T1104 As DelayType 'v11.5未使用なので削除 by 間々田 2006/08/01
		private static mLogic.DelayType T1201;
		private static mLogic.DelayType T1202;

		public static void XraymLogic()
		{
			//=========================================================
			//入力
			D608 = ComkV;
			if (D608 < 0)
			{
				D608 = 0;
			}
			D610 = CommA;
			if (D610 < 0)
			{
				D610 = 0;
			}

			M101 = fXraymEmergency;			//
			M102 = ComInterLock;			//X線ｲﾝﾀｰﾛｯｸ
//			M103 = ComStandby    'ｽﾀﾝﾊﾞｲ
			M104 = M933;
			M123 = fXraymOffDateRst;

			if (ComXon != 0)
			{
				M908 = 1;
			}
			else
			{
				M908 = 0;
			}
			M1102 = ComErr;

			D600 = fXraymVolt;
			//管電圧
			D603 = fXraymAmp;
			//管電流
			M100 = fXraymOn;
			//X線ON
			M911 = fXraymInterlockErrRst;	//インターロック異常表示解除
			//M921 = fXraymOnErrRst        'X線ON異常表示解除    'v11.5未使用なので削除 by 間々田 2006/08/01
			M1207 = fXraymOffErrRst;		//X線OFF異常表示解除
			//    M1005 = fXraymStandbyErrRst  'スタンバイ異常表示解除
			//M1005 = 1                    '強制解除             'v11.5未使用なので削除 by 間々田 2006/08/01
			M1107 = fXraymUnitErrRst;		//X線発生器異常表示解除
			//M1114 = fXraymVoltErrRst     '管電圧異常表示解除   'v11.5未使用なので削除 by 間々田 2006/08/01
			//M1121 = fXraymAmpErrRst      '管電流異常表示解除   'v11.5未使用なので削除 by 間々田 2006/08/01

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

//2000-03-07 T.Shibui
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

//2000-03-08 T.Shibui ｳｫｰﾑｱｯﾌﾟ完は、完了時にX線をOFFする為にある。したがって、ｳｫｰﾑｱｯﾌﾟを行わない6160は、0固定とする。
//2000-02-28 T.Shibui ｳｫｰﾑｱｯﾌﾟ完は、ｳｫｰﾑｱｯﾌﾟ強制終了で管理。
			//M118 = 0   'v11.5未使用なので削除 by 間々田 2006/08/01


			//=========================================================
			//ウォームアップ判定
			//ウォームアップ不要
//2000-02-28 T.Shibui ｳｫｰﾑｱｯﾌﾟ強制終了で判断
			if (M123 != 0)
			{
				M207 = 1;
			}
			else
			{
				M207 = 0;
			}

			//ＩＩ電源
//2000-03-08 T.Shibui ｳｫｰﾑｱｯﾌﾟ強制終了を条件に追加
			//If M213 > 0 Then   'v11.5無意味なので削除 by 間々田 2006/08/01
			//    M214 = 1       'v11.5無意味なので削除 by 間々田 2006/08/01
			//Else               'v11.5無意味なので削除 by 間々田 2006/08/01
			//    M214 = 0       'v11.5無意味なので削除 by 間々田 2006/08/01
			//End If             'v11.5無意味なので削除 by 間々田 2006/08/01

			//=========================================================
			//X線管電圧／管電流選択
			//ウォームアップ中でない
			if (M123 != 0)
			{
				M604 = 1;
			}
			else
			{
				M604 = 0;
			}

			if (D601 < 0)
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
				D601 = 0;
			}

			//D602 = D601        'v11.5未使用なので削除 by 間々田 2006/08/01

			//If M604 > 0 Then
			//    D604 = D603    'v11.5未使用なので削除 by 間々田 2006/08/01
			//End If

			//D607 = D604        'v11.5未使用なので削除 by 間々田 2006/08/01

			D609 = D608;

			if (M939 != 0)
			{
				M616 = 0;
			}
			else
			{
				M616 = 1;
			}
			if (M939 != 0)
			{
				D614 = D609;
			}
			if (M616 != 0)
			{
				D614 = 0;
			}
			D611 = D610;
			//D612 = D610    'v11.5未使用なので削除 by 間々田 2006/08/01

			//管電流（フィードバック）
			D613 = D611;

			if (M939 != 0)
			{
				D615 = D613;
			}
			if (M616 != 0)
			{
				D615 = 0;
			}
			D616 = D614 / 1000000.0f * D615 * 1000.0f;

            //Debug.Print("Val= " + D616);

			//FINE判定
			if (D616 <= 4)
			{
				M618 = 1;
			}
			else
			{
				M618 = 0;
			}
			if (M618 != 0)
			{
				M619 = 1;
			}
			else
			{
				M619 = 0;
			}

			//=========================================================
			//Ｘ線ＴＶＣＬ制御

			D714 = D600;
			//D710 = D714    'v11.5未使用なので削除 by 間々田 2006/08/01
			D715 = D603;
			//D713 = D715    'v11.5未使用なので削除 by 間々田 2006/08/01

			//=========================================================
			//Ｘ線管電圧／管電流確認
			if (D803 != D614 || D804 != D615)
			{
				D803 = D614;
				D804 = D615;
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

			//If D609 > 2 Then   'v11.5無意味なので削除 by 間々田 2006/08/01
			//    M938 = 1       'v11.5無意味なので削除 by 間々田 2006/08/01
			//Else               'v11.5無意味なので削除 by 間々田 2006/08/01
			//    M938 = 0       'v11.5無意味なので削除 by 間々田 2006/08/01
			//End If             'v11.5無意味なので削除 by 間々田 2006/08/01
			if (M908 != 0)
			{
				M939 = 1;
			}
			else
			{
				M939 = 0;
			}
			mLogic.iOnDelay(M1102, ref T1101, 100, ref M1103);

			if (M1103 > 0)
			{
				M1104 = 0;
			}
			else
			{
				M1104 = 1;
			}

			if (M1104 + M100 > 0)
			{
				M1105 = 1;
			}
			else
			{
				M1105 = 0;
			}

			if (M1102 * M1105 > 0)
			{
				M1106 = 1;
			}
			else
			{
				M1106 = 0;
			}

			if (M1107 > 0)
			{
				M1108 = 0;
			}
			else if (M1106 > 0)
			{
				M1108 = 1;
			}

			//If M1107 Then  'v11.5無意味なので削除 by 間々田 2006/08/01
			//    M1126 = 0  'v11.5無意味なので削除 by 間々田 2006/08/01
			//Else           'v11.5無意味なので削除 by 間々田 2006/08/01
			//    M1126 = 1  'v11.5無意味なので削除 by 間々田 2006/08/01
			//End If         'v11.5無意味なので削除 by 間々田 2006/08/01

			//Ｘ線発生器異常
			if (M1102 + M1108 > 0)
			{
				M1109 = 1;
			}
			else
			{
				M1109 = 0;
			}

			mLogic.iOnDelay(M108, ref T1201, 5000, ref M1201);

			if (M1201 * M908 > 0)
			{
				M1202 = 1;
			}
			else
			{
				M1202 = 0;
			}

			mLogic.iOnDelay(M1202, ref T1202, 100, ref M1203);

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			if (M1203 > 0)
			{
				M1204 = 0;
			}
			else
			{
				M1204 = 1;
			}

			if (M1204 + M100 > 0)
			{
				M1205 = 1;
			}
			else
			{
				M1205 = 0;
			}
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			if (M1202 * M1207 > 0)
			{
				M1206 = 1;
			}
			else
			{
				M1206 = 0;
			}

			if (M1207 > 0)
			{
				M1208 = 0;
			}
			else if (M1206 > 0)
			{
				M1208 = 1;
			}

			//X線OFF異常
			if (M1202 + M1208 > 0)
			{
				M1209 = 1;
			}
			else
			{
				M1209 = 0;

			}

			//異常発生中
			if (M913 + M932 + M1007 + M1125 + M1109 + M1116 + M1123 + M1209 > 0)
			{
				M933 = 1;
			}
			else
			{
				M933 = 0;
			}

			//=========================================================
			//出力

			if (M107 != M128)
			{
				if (M107 != 0)
				{
					ComOutXOn = 1;
				}
				else
				{
					ComOutXOn = 0;
				}
				M128 = M107;
			}

			if (D722 != D714)
			{
				ComOutkV = "K" + Convert.ToString(D714);	//管電圧指定
				D722 = D714;
			}
			if (D723 != D715)
			{
				ComOutmA = "A" + Convert.ToString(D715);	//管電流指定
				D723 = D715;
			}

			gXraymWarmup = M207;			//ウォームアップ不要
			gXraymVolt = D614;				//管電圧
			gXraymAmp = D615;				//管電流
			gXraymWatt = D616;				//電力
			gxraymFine = M619;				//FINE
			gXraymOn = M107;				//X線ON
			gXraymInterlockErr = M913;		//インターロック異常
			gXraymInterlockErrDsp = M912;	//インターロック異常表示
			gXraymOnErr = M923;				//X線ON異常
			gXraymOnErrDsp = M922;			//X線ON異常表示
			gXraymOffErr = M1209;			//X線OFF異常
			gXraymOffErrDsp = M1208;		//X線OFF異常表示
//			gXraymStandbyErr = M1007     'スタンバイ異常
//			gXraymStandbyErrDsp = M1006  'スタンバイ異常表示
			gXraymUnitErr = M1109;			//X線発生器異常
			gXraymUnitErrDsp = M1108;		//X線発生器異常表示
//			gXraymVoltErr = M1116        '管電圧異常
//			gXraymVoltErrDsp = M1115     '管電圧異常表示
//			gXraymAmpErr = M1123         '管電流異常
//			gXraymAmpErrDsp = M1122      '管電流異常表示
			gXraymPermitWarmup = 1;			//エージング許可

			gXraymErr = M933;				//異常発生中

			gXAvail = M807;
		}
	}
}
