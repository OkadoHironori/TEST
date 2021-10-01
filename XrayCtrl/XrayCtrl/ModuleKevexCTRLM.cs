using System;

namespace XrayCtrl
{
	internal static class ModuleKevexCTRLM
	{
		public static int itMechSetPosition;

		private static mLogic.DelayType T101 = new mLogic.DelayType();
		private static mLogic.DelayType T102 = new mLogic.DelayType();
		private static mLogic.DelayType T103 = new mLogic.DelayType();
		private static mLogic.DelayType T104 = new mLogic.DelayType();
		private static mLogic.DelayType T105 = new mLogic.DelayType();

//2002-08-30 Shibui
		private static mLogic.DelayType T106 = new mLogic.DelayType();

		private static mLogic.DelayType T108 = new mLogic.DelayType();
		private static mLogic.DelayType T201 = new mLogic.DelayType();
		private static mLogic.DelayType T202 = new mLogic.DelayType();
		private static mLogic.DelayType T203 = new mLogic.DelayType();
		private static mLogic.DelayType T204 = new mLogic.DelayType();
		private static mLogic.DelayType T205 = new mLogic.DelayType();
		private static mLogic.DelayType T206 = new mLogic.DelayType();

		private static mLogic.PulseType P201 = new mLogic.PulseType();

		private static int M101 = 0;
		private static int M102 = 0;
		private static int M105 = 0;
		private static int M106 = 0;
		private static int M107 = 0;
		private static int M108 = 0;
		private static int M109 = 0;
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
		//Private M140 As Integer
		//Private M141 As Integer
		//Private M142 As Integer
		//Private M143 As Integer
		//Private M144 As Integer
		//Private M145 As Integer
		//Private M146 As Integer
		//Private M147 As Integer
		private static int M148 = 0;
		private static int M149 = 0;
		private static int M150 = 0;
		private static int M151 = 0;
		private static int M152 = 0;
		private static int M153 = 0;
		private static int M154 = 0;
		private static int M155 = 0;
		private static int M156 = 0;
		private static int M157 = 0;
		//Private M158 As Integer

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M159 = 0;
//		private static int M160 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

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
//1999-10-15 T.Shibui
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M174 = 0;
//		private static int M175 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M176 = 0;
		private static int M177 = 0;
		private static int M178 = 0;

//2002-08-30 Shibui
		private static int M180 = 0;
		private static int M181 = 0;

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

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		private static int M211 = 0;
		private static int M215 = 0;
		private static int M216 = 0;
		private static int M217 = 0;
		private static int M218 = 0;
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M219 = 0;
		private static int M220 = 0;
		private static int M221 = 0;
		private static int M222 = 0;
		private static int M223 = 0;
		private static int M224 = 0;
		private static int M225 = 0;
		private static int M226 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M227 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M228 = 0;
		private static int M229 = 0;
		private static int M230 = 0;
		private static int M231 = 0;
		private static int M232 = 0;
		private static int M233 = 0;
		private static int M234 = 0;
		private static int M235 = 0;
		private static int M236 = 0;
		private static int M237 = 0;
		private static int M238 = 0;
		private static int M239 = 0;
		private static int M240 = 0;
		private static int M241 = 0;
		private static int M242 = 0;
		private static int M243 = 0;
		private static int M501 = 0;
		private static int M502 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M503 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M504 = 0;
		private static int M505 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M506 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M507 = 0;
		private static int M508 = 0;
		private static int M509 = 0;
		private static int M510 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static int M511 = 0;
//		private static int M512 = 0;
//		private static int M513 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static int M514 = 0;
		private static int M515 = 0;
		private static int M516 = 0;
		private static int M517 = 0;
		private static int M518 = 0;
		private static int M519 = 0;

		private static float D101 = 0;
		private static float D102 = 0;
		private static int D103 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		private static float D104 = 0;
//		private static float D105 = 0;
//		private static float D106 = 0;
//		private static float D107 = 0;
//		private static float D108 = 0;
//		private static float D109 = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private static float D110 = 0;
		private static float D111 = 0;
		private static float D112 = 0;
		private static float D113 = 0;
		private static float D114 = 0;
		private static float D201 = 0;
		private static float D202 = 0;
		private static float D203 = 0;
		private static float D501 = 0;
		private static float D502 = 0;
		private static int D503 = 0;

		public static void KevexCtrlmLogic()
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			float fid = 0;
//			float fod = 0;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//==========================================================
			//入力

			if (ModuleCTRLM.ifXrayonoff_Set == 1)
			{
				M105 = 1;				//X線ON
			}
			else if (ModuleCTRLM.ifXrayonoff_Set == 2)
			{
				M108 = 1;				//X線OFF
			}
			else
			{
				M105 = 0;
				M108 = 0;
			}
			ModuleCTRLM.ifXrayonoff_Set = 0;
			//    M120 = gDioDi30             'X線ON中
			//    M154 = gConmEmergencyErr
			M153 = ModuleIOM.gDioDi14;


			if (itMechSetPosition != 0)			//ﾒｶ移動開始
			{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
// 				M159 = 1;
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
				itMechSetPosition = 0;
			}
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			else
			{
				M159 = 0;
			}
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			if (M159 != 0)
			{
				M160 = 1;
			}
			else
			{
				M160 = 0;
			}
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			M102 = ModuleCTRLM.ifFocussize;			//←→ 焦点サイズ 0:小 1:大

			M106 = ModuleXRAYM.gXraymErr;
			M109 = ModuleCTRLM.ifXcont_Mode;

			if (ModuleCTRLM.ifXtimer >= 0)
			{
				D103 = ModuleCTRLM.ifXtimer;
				ModuleCTRLM.ifXtimer = -1;
			}

			D101 = ModuleCTRLM.ifX_Volt;
			D102 = ModuleCTRLM.ifX_Amp;

			D110 = ModuleCTRLM.ifXRMinkV;
			D111 = ModuleCTRLM.ifXRMinmA;
			D112 = ModuleCTRLM.ifXRMaxkV;
			D113 = ModuleCTRLM.ifXRMaxmA;

			M137 = ModuleCTRLM.ifWarmup_Reset;
			ModuleCTRLM.ifWarmup_Reset = 0;
			M138 = ModuleCTRLM.ifWarmup_Cancel;
			ModuleCTRLM.ifWarmup_Cancel = 0;
			M156 = ModuleXRAYM.gXraymWarmupEnd;

			//XRAY STS ---------------------------------------------------------------
			D501 = ModuleXRAYM.gXraymVolt;
			D502 = ModuleXRAYM.gXraymAmp;
			M501 = ModuleXRAYM.gXraymForcus;
			M502 = ModuleXRAYM.gXraymOn;
			M504 = ModuleXRAYM.gXraymInterlockErr;
			M505 = ModuleXRAYM.gXraymWarmupOn;
			D503 = ModuleXRAYM.gXraymWarmupTimer;
			M507 = ModuleXRAYM.gXraymWarmup2D;
			M508 = ModuleXRAYM.gXraymWarmup2W;
			M509 = ModuleXRAYM.gXraymWarmup3W;

			M219 = 0;
			M223 = 0;
			M243 = 0;
			M231 = 0;
			M235 = 0;
			M239 = 0;

			//==========================================================
			//Ｘ線
			if (M153 != 0)
			{
				M155 = 0;
			}
			else
			{
				M155 = 1;
			}
			if (M154 + M155 != 0)
			{
				M101 = 1;
			}
			else
			{
				M101 = 0;
			}
			M121 = ModuleCTRLM.ifX_Volt_Up;
			ModuleCTRLM.ifX_Volt_Up = 0;
			if (M124 != 0)
			{
				M122 = 0;
			}
			else
			{
				M122 = 1;
			}
			mLogic.iOnDelay(M122, ref T102, ModuleCTRLM.UpDownRate, ref M123);
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
				if (D101 < D112)
				{
					D101 = D101 + 1;
				}
				else
				{
					D101 = D112;
				}
			}
			if (D101 > D112)
			{
				D101 = D112;
			}

			M125 = ModuleCTRLM.ifX_Volt_Down;
			ModuleCTRLM.ifX_Volt_Down = 0;
			if (M128 != 0)
			{
				M126 = 0;
			}
			else
			{
				M126 = 1;
			}
			mLogic.iOnDelay(M126, ref T103, ModuleCTRLM.UpDownRate, ref M127);
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
//1999-09-30 T.Shibui 最小管電圧
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

			M129 = ModuleCTRLM.ifX_Amp_Up;
			ModuleCTRLM.ifX_Amp_Up = 0;
			if (M132 != 0)
			{
				M130 = 0;
			}
			else
			{
				M130 = 1;
			}
			mLogic.iOnDelay(M130, ref T104, ModuleCTRLM.UpDownRate, ref M131);
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
				if (D102 < D113)
				{
					D102 = D102 + 1;
				} else {
					D102 = D113;
				}
			}
			if (D102 > D113)
			{
				D102 = D113;
			}
			if (M102 != 0)
			{
				ModuleKevexXRAYM.XrayTVCL_130KV_Large_i(D101, ref D102);
			}
			else
			{
				ModuleKevexXRAYM.XrayTVCL_130KV_Small_i(D101, ref D102);
			}
			M133 = ModuleCTRLM.ifX_Amp_Down;
			ModuleCTRLM.ifX_Amp_Down = 0;
			if (M136 != 0)
			{
				M134 = 0;
			}
			else
			{
				M134 = 1;
			}
			mLogic.iOnDelay(M134, ref T105, ModuleCTRLM.UpDownRate, ref M135);
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
			if (M101 + M106 != 0)
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
			if (M157 + M138 + M156 != 0)
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

			if (M120 != 0)
			{
				M148 = 1;
			}
			else
			{
				M148 = 0;
			}
			mLogic.iOnDelay(M148, ref T101, D103 * 1000, ref M117);
			if (M510 != 0)
			{
				M149 = 0;
			}
			else
			{
				M149 = 1;
			}

			if (M116 * M148 * M149 != 0)
			{
//2002-08-26 Shibui 装置が動かなかったら異常を出すを削除。
//				M176 = 1
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
				M178 = 1;
			}

//2002-08-30 Shibui
//			If M107 + M139 + M151 + M178 Then
			if (M107 + M139 + M151 + M178 + M180 != 0)
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
			else if (M105 != 0)
			{
				M120 = 1;
			}

//2002-08-30 Shibui X線ON中にX線ON中IOがOFFした場合（制御器でOFFした場合）X線OFFとする。
			mLogic.iOnDelay(M120, ref T106, 100, ref M181);
			if (M181 == 1 && ModuleIOM.gDioDi30 == 0)
			{
				M180 = 1;
			}
			else
			{
				M180 = 0;
			}

			if (M101 + M151 + M178 != 0)
			{
				M152 = 1;
			}
			else
			{
				M152 = 0;
			}

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			if (M503 != 0)
			{
				M513 = 0;
			}
			else
			{
				M513 = 1;
			}
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			if (M504 != 0)
			{
				M514 = 0;
			}
			else
			{
				M514 = 1;
			}
			if (M518 != 0)
			{
				M519 = 0;
			}
			else
			{
				M519 = 1;
			}
			if (M507 + M508 + M509 != 0)
			{
				M510 = 1;				//ｳｫｰﾑｱｯﾌﾟ不要
			}
			else
			{
				M510 = 0;
			}
			if (M510 > 0 && M505 == 0)
			{
				M515 = 1;
			}
			else
			{
				M515 = 0;
			}
			if (M505 != 0)
			{
				M516 = 1;
			}
			else
			{
				M516 = 0;
			}
			if (M510 == 0 && M505 == 0)
			{
				M517 = 1;
			}
			else
			{
				M517 = 0;
			}
//
//			'==========================================================
//			'ＳＴＳ
			if (M201 != 0)
			{
				M202 = 0;
			}
			else
			{
				M202 = 1;
			}
			if (M203 != 0)
			{
				M204 = 0;
			}
			else
			{
				M204 = 1;
			}
			if (M205 != 0)
			{
				M206 = 0;
			}
			else
			{
				M206 = 1;
			}
			if (M207 != 0)
			{
				M208 = 0;
			}
			else
			{
				M208 = 1;
			}
			if (M202 + M204 + M206 + M208 != 0)
			{
				M209 = 1;
			}
			else
			{
				M209 = 0;
			}
			mLogic.iPulse(M209, ref P201, ref M210);
			if (M222 != 0)
			{
				M220 = 0;
			}
			else
			{
				M220 = 1;
			}
			mLogic.iOnDelay(M220, ref T201, ModuleCTRLM.UpDownRate, ref M221);
			if (M219 * M221 != 0)
			{
				M222 = 1;
			}
			else
			{
				M222 = 0;
			}
			if (M226 != 0)
			{
				M224 = 0;
			}
			else
			{
				M224 = 1;
			}
			mLogic.iOnDelay(M224, ref T202, ModuleCTRLM.UpDownRate, ref M225);
			if (M223 * M225 != 0)
			{
				M226 = 1;
			}
			else
			{
				M226 = 0;
			}
			if (M222 == 1)
			{
				if (D201 < 100)
				{
					D201 = D201 + 1;
				}
				else
				{
					D201 = 100;
				}
			}
			if (M226 == 1)
			{
				if (D201 > 0)
				{
					D201 = D201 - 1;
				}
				else
				{
					D201 = 0;
				}
			}
			if (D201 > 100)
			{
				D201 = 100;
			}
			if (D201 < 0)
			{
				D201 = 0;
			}
			if (M230 != 0)
			{
				M228 = 0;
			}
			else
			{
				M228 = 1;
			}
			mLogic.iOnDelay(M228, ref T203, ModuleCTRLM.UpDownRate, ref M229);
			if (M243 * M229 != 0)
			{
				M230 = 1;
			}
			else
			{
				M230 = 0;
			}
			if (M234 != 0)
			{
				M232 = 0;
			}
			else
			{
				M232 = 1;
			}
			mLogic.iOnDelay(M232, ref T204, ModuleCTRLM.UpDownRate, ref M233);
			if (M231 * M233 != 0)
			{
				M234 = 1;
			}
			else
			{
				M234 = 0;
			}
			if (M230 != 0)
			{
				if (D202 < 300)
				{
					D202 = D202 + 0.1f;
				}
				else
				{
					D202 = 300;
				}
			}
			if (M234 != 0)
			{
				if (D202 > 0)
				{
					D202 = D202 - 0.1f;
				}
				else
				{
					D202 = 0;
				}
			}
			if (D202 > 300)
			{
				D202 = 300;
			}
			if (D202 < 0)
			{
				D202 = 0;
			}
			if (M238 != 0)
			{
				M236 = 0;
			}
			else
			{
				M236 = 1;
			}
			mLogic.iOnDelay(M236, ref T205, ModuleCTRLM.UpDownRate, ref M237);
			if (M235 * M237 != 0)
			{
				M238 = 1;
			}
			else
			{
				M238 = 0;
			}
			if (M242 != 0)
			{
				M240 = 0;
			}
			else
			{
				M240 = 1;
			}
			mLogic.iOnDelay(M240, ref T206, ModuleCTRLM.UpDownRate, ref M241);
			if (M239 * M241 != 0)
			{
				M242 = 1;
			}
			else
			{
				M242 = 0;
			}
			if (M238 != 0)
			{
				if (D203 < 10)
				{
					D203 = D203 + 0.1f;
				}
				else
				{
					D203 = 0;
				}
			}
			if (M242 != 0)
			{
				if (D203 > 0)
				{
					D203 = D203 - 0.1f;
				}
				else
				{
					D203 = 0;
				}
			}
			if (D203 > 10)
			{
				D203 = 10;
			}
			if (D203 < 0)
			{
				D203 = 0;
			}
			//
			//==========================================================
			//出力
			ModuleXRAYM.fXraymEmergency = M152;
			//---------------------------------------------------------
			ModuleCTRLM.ipXcont_Mode = M109;
			ModuleCTRLM.ipXtimer = D103;
			ModuleXRAYM.fXraymVolt = (int)D101;
			ModuleCTRLM.ifX_Volt = D101;
			ModuleXRAYM.fXraymAmp = (int)D102;
			ModuleCTRLM.ifX_Amp = D102;
			ModuleXRAYM.fXraymForcus = M102;
			ModuleXRAYM.fXraymOn = M120;
			ModuleKevexXRAYM.fXraymOff = M119;

			ModuleXRAYM.fXraymOffDateRst = M137;
			ModuleCTRLM.ipcndVolt = D101;
			ModuleCTRLM.ipcndAmp = D102;
			ModuleCTRLM.ipX_Volt = D501;
			ModuleCTRLM.ipX_Amp = D502;
//			ipX_Fine = gxraymFine
			ModuleCTRLM.ipX_Watt = ModuleXRAYM.gXraymWatt;
			ModuleCTRLM.ipFocussize = M501;
			ModuleCTRLM.ipX_On = M502;
			ModuleCTRLM.ipInterlock = M514;
			if (M515 > 0)
			{
				ModuleCTRLM.ipWarmup = 0;
			}
			if (M516 > 0)
			{
				ModuleCTRLM.ipWarmup = 1;
			}
			if (M517 > 0)
			{
				ModuleCTRLM.ipWarmup = 2;
			}
			ModuleCTRLM.ipXStatus = M519;
			ModuleCTRLM.ipXPermitWarmup = ModuleXRAYM.gXraymPermitWarmup;

			if (M507 != 0)
			{
				ModuleCTRLM.ipWarmup_Mode = 1;
			}
			else if (M508 != 0)
			{
				ModuleCTRLM.ipWarmup_Mode = 2;
			}
			else if (M509 != 0)
			{
				ModuleCTRLM.ipWarmup_Mode = 3;
			}
			else
			{
				ModuleCTRLM.ipWarmup_Mode = -1;
			}
			ModuleCTRLM.ipWrest_timeM = (int)(D503 / 60);
			ModuleCTRLM.ipWrest_timeS = D503 % 60;

            //見直し書き方を変更_2014/10/07hata
            //ModuleCTRLM.ipX_type = 1;
            ModuleCTRLM.ipX_type = modIniFiles.XRAY_TYPE_NO_130KV;
            
            ModuleCTRLM.ipXAvail = ModuleXRAYM.gXAvail;
			ModuleCTRLM.ipXRMaxkV = ModuleCTRLM.ifXRMaxkV;
			ModuleCTRLM.ipXRMaxmA = ModuleCTRLM.ifXRMaxmA;
			ModuleCTRLM.ipXRMinkV = ModuleCTRLM.ifXRMinkV;
			ModuleCTRLM.ipXRMinmA = ModuleCTRLM.ifXRMinmA;

			//=======================================================
			//Err Check
			ModuleCTRLM.eRst = ModuleCTRLM.ifErrrst;
			ModuleCTRLM.eSts = ModuleCTRLM.ipErrsts;

//			EmergencyErrDsp = gConmEmergencyErrDsp

			ModuleCTRLM.InterLockErrDsp = ModuleXRAYM.gXraymInterlockErrDsp;
//2002-08-29 Shibui
//			OffErrDsp = gXraymOffErrDsp
//			OnErrDsp = gXraymOnErrDsp

			if (ModuleCTRLM.eRst != 0)
			{
				if (ModuleCTRLM.eSts == 101)
				{
					ModuleCTRLM.EmergencyErrRst = 1;
				}

//				If eSts = 6 Then
//					AmpErrRst = 1
//				End If
				if (ModuleCTRLM.eSts == 4)
				{
					ModuleCTRLM.InterLockErrRst = 1;
				}

//2002-08-29 Shibui
//				If eSts = 12 Then
//					OffErrRst = 1
//				End If
//				If eSts = 11 Then
//					OnErrRst = 1
//				End If

				ModuleCTRLM.eSts = 0;
				ModuleCTRLM.eRst = 0;
			}
			if (ModuleCTRLM.eSts == 0)
			{
				if (ModuleCTRLM.EmergencyErrDsp > 0 && ModuleCTRLM.EmergencyErrRst == 0)
				{
					ModuleCTRLM.eSts = 101;
				}

				if (ModuleCTRLM.InterLockErrDsp > 0 && ModuleCTRLM.InterLockErrRst == 0)
				{
					ModuleCTRLM.eSts = 4;
				}
//2002-08-29 Shibui
//				If OffErrDsp > 0 And OffErrRst = 0 Then
//					eSts = 12
//				End If
//				If OnErrDsp > 0 And OnErrRst = 0 Then
//					eSts = 11
//				End If

			}
			if (ModuleCTRLM.EmergencyErrDsp == 0)
			{
				ModuleCTRLM.EmergencyErrRst = 0;
			}

//			If AmpErrDsp = 0 Then
//				AmpErrRst = 0
//			End If
			if (ModuleCTRLM.InterLockErrDsp == 0)
			{
				ModuleCTRLM.InterLockErrRst = 0;
			}

//2002-08-29 Shibui
//			If OffErrDsp = 0 Then
//				OffErrRst = 0
//			End If
//			If OnErrDsp = 0 Then
//				OnErrRst = 0
//			End If

//			fConmEmergencyErrRst = EmergencyErrRst

//			fXraymAmpErrRst = AmpErrRst
			ModuleXRAYM.fXraymInterlockErrRst = ModuleCTRLM.InterLockErrRst;

//2002-08-29 Shibui
//			fXraymOffErrRst = OffErrRst
//			fXraymOnErrRst = OnErrRst

			ModuleCTRLM.ipErrsts = ModuleCTRLM.eSts;
			ModuleCTRLM.ifErrrst = ModuleCTRLM.eRst;
		}
	}
}
