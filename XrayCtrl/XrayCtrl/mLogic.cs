using System;

namespace XrayCtrl
{
	internal static class mLogic
	{
		public struct DelayType
		{
			public int Flag;
			public long Data;
			public DateTime stime;
		}

		public struct PulseType
		{
			public int Flag;
		}

		static long BaseTime;
//		Public BaseSecCount As Single      'v11.5削除 by 間々田 2006/08/01 未使用

		public static void Init(long Delay)
		{
			BaseTime = Delay;
		}

		//v11.5削除ここから by 間々田 2006/08/01 未使用
		//Sub iOr(Data As Integer, Out As Integer)
		//    If Data > 0 Then
		//        Out = 1
		//    Else
		//        Out = 0
		//    End If
		//End Sub
		//
		//Sub iAnd(Data As Integer, Out As Integer)
		//    If Data > 0 Then
		//        Out = 1
		//    Else
		//        Out = 0
		//    End If
		//End Sub
		//
		//Sub iNot(Data As Integer, Out As Integer)
		//    If Data > 0 Then
		//        Out = 0
		//    Else
		//        Out = 1
		//    End If
		//End Sub
		//
		//Sub iOut(Data As Integer, Out As Integer)
		//    If Data > 0 Then
		//        Out = 1
		//    Else
		//        Out = 0
		//    End If
		//End Sub
		//
		//Sub iSr(iSet As Integer, iRst As Integer, Out As Integer)
		//    If iRst > 0 Then
		//        Out = 0
		//    ElseIf iSet > 0 Then
		//        Out = 1
		//    End If
		//End Sub
		//
		//Sub iDl(iData As Integer, iLach As Integer, Out As Integer)
		//    If iLach > 0 Then
		//    Else
		//        If iData > 0 Then
		//            Out = 1
		//        Else
		//            Out = 0
		//        End If
		//    End If
		//End Sub
		//v11.5削除ここまで by 間々田 2006/08/01 未使用

		//***********************************************************************************
		//2002-08-27 Shibui
		//
		//   管電圧=ipX_Volt
		//   管電流=ipX_Amp
		//   Ｘ線オン=ipX_On 0:off 1:on
		//
		//***********************************************************************************
		public static void iOnDelay(int iData, ref DelayType dd, long lDelay, ref int Out)
		{

			if (iData > 0)
			{
				//停止->起動 ﾀｲﾏｰ値ｾｯﾄ
				if (dd.Flag == 0)
				{
					dd.Flag = 1;
					dd.Data = lDelay;
					if (dd.Data >= 5000)
					{
						dd.stime = DateTime.Now;
					}
					Out = 0;
				}
				else						//起動中 ﾀｲﾏｰ値更新
				{
					if (dd.Data > 0)
					{
						if (dd.Data >= 5000)
						{
							if (dd.Data <= (DateTime.Now - dd.stime).TotalMilliseconds)
							{
								dd.Data = 0;
							}
						}
						else
						{
							dd.Data = dd.Data - BaseTime;
						}
					}
					if (dd.Data <= 0)
					{
						Out = 1;
					}
					else
					{
						Out = 0;
					}
				}
			}
			else
			{
				dd.Flag = 0;
				Out = 0;
			}
		}

		//***********************************************************************************
		//2002-09-06 山本
		// Kevex用アベイラブル時間　added by 山本　2002-8-31 END
		//
		//   管電圧=ipX_Volt
		//   管電流=ipX_Amp
		//   Ｘ線オン=ipX_On 0:off 1:on
		//
		//***********************************************************************************
		public static void iAveDelay(int iData, ref DelayType dd, long lDelay, ref int Out)
		{
			long DelayTime = 0;

			DelayTime = lDelay;

			//Kevex用の時だけX線条件によりアベイラブル時間を変える
			if (ModuleCTRLM.ifEventValue == 3)
			{
				if (ModuleCTRLM.ipX_Volt <= 40 && ModuleCTRLM.ipX_Amp <= 40)
				{
					DelayTime = ModuleCTRLM.ifAvTm40kV30mA;
				}
				else if (ModuleCTRLM.ipX_Volt <= 40 && ModuleCTRLM.ipX_Amp > 40)
				{
					DelayTime = ModuleCTRLM.ifAvTm40kV40mA;
				}
				else if (ModuleCTRLM.ipX_Volt > 40 && ModuleCTRLM.ipX_Volt <= 60 && ModuleCTRLM.ipX_Amp <= 40)
				{
					DelayTime = ModuleCTRLM.ifAvTm60kV30mA;
				}
				else if (ModuleCTRLM.ipX_Volt > 40 && ModuleCTRLM.ipX_Volt <= 60 && ModuleCTRLM.ipX_Amp > 40)
				{
					DelayTime = ModuleCTRLM.ifAvTm60kV40mA;
				}
				else if (ModuleCTRLM.ipX_Volt > 60 && ModuleCTRLM.ipX_Volt <= 80 && ModuleCTRLM.ipX_Amp <= 40)
				{
					DelayTime = ModuleCTRLM.ifAvTm80kV30mA;
				}
				else if (ModuleCTRLM.ipX_Volt > 60 && ModuleCTRLM.ipX_Volt <= 80 && ModuleCTRLM.ipX_Amp > 40)
				{
					DelayTime = ModuleCTRLM.ifAvTm80kV40mA;
				}
				else if (ModuleCTRLM.ipX_Volt > 80 && ModuleCTRLM.ipX_Volt <= 100 && ModuleCTRLM.ipX_Amp <= 40)
				{
					DelayTime = ModuleCTRLM.ifAvTm100kV30mA;
				}
				else if (ModuleCTRLM.ipX_Volt > 80 && ModuleCTRLM.ipX_Volt <= 100 && ModuleCTRLM.ipX_Amp > 40)
				{
					DelayTime = ModuleCTRLM.ifAvTm100kV40mA;
				}
				else if (ModuleCTRLM.ipX_Volt > 100 && ModuleCTRLM.ipX_Volt <= 120 && ModuleCTRLM.ipX_Amp <= 40)
				{
					DelayTime = ModuleCTRLM.ifAvTm120kV30mA;
				}
				else if (ModuleCTRLM.ipX_Volt > 100 && ModuleCTRLM.ipX_Volt <= 120 && ModuleCTRLM.ipX_Amp > 40)
				{
					DelayTime = ModuleCTRLM.ifAvTm120kV40mA;
				}
				else if (ModuleCTRLM.ipX_Volt > 120 && ModuleCTRLM.ipX_Amp <= 40)
				{
					DelayTime = ModuleCTRLM.ifAvTm130kV30mA;
				}
				else if (ModuleCTRLM.ipX_Volt > 120 && ModuleCTRLM.ipX_Amp > 40)
				{
					DelayTime = ModuleCTRLM.ifAvTm130kV40mA;
				}
			}

			if (iData > 0)
			{
				if (dd.Flag == 0)		//停止->起動 ﾀｲﾏｰ値ｾｯﾄ
				{
					dd.Flag = 1;
					//dd.Data = lDelay
					dd.Data = DelayTime;
					if (dd.Data >= 5000)	//chaged by 山本　2002-8-31
					{
						dd.stime = DateTime.Now;
					}
					Out = 0;
				}
				else 					//起動中 ﾀｲﾏｰ値更新
				{
					if (dd.Data > 0)
					{
						if (dd.Data >= 5000)
						{
							if (dd.Data <= (DateTime.Now - dd.stime).TotalMilliseconds)
							{
								dd.Data = 0;
							}
						}
						else
						{
							dd.Data = dd.Data - BaseTime;
						}
					}
					if (dd.Data <= 0)
					{
						Out = 1;
					}
					else
					{
						Out = 0;
					}
				}
			}
			else
			{
				dd.Flag = 0;
				Out = 0;
			}
		}

		//v11.5削除ここから by 間々田 2006/08/01 未使用
		//Sub iOffDelay(iData As Integer, dd As DelayType, lDelay As Long, Out As Integer)
		//    If iData > 0 Then
		//        dd.Flag = 1
		//        dd.Data = lDelay
		//        Out = 1
		//    Else
		//        If dd.Flag > 0 Then
		//            If dd.Data > 0 Then
		//                dd.Data = dd.Data - BaseTime
		//            End If
		//            If dd.Data <= 0 Then
		//                Out = 0
		//                dd.Flag = 0
		//            Else
		//                Out = 1
		//            End If
		//        Else
		//            Out = 0
		//        End If
		//    End If
		//
		//End Sub
		//v11.5削除ここまで by 間々田 2006/08/01 未使用

		//1ｼｮｯﾄ
		public static void iPulse(int iData, ref PulseType pd, ref int Out)
		{
			if (iData > 0)
			{
				if (pd.Flag == 0)
				{
					pd.Flag = 1;
					Out = 1;
				}
				else
				{
					Out = 0;
				}
			}
			else
			{
				pd.Flag = 0;
				Out = 0;
			}
		}

		//v11.5削除ここから by 間々田 2006/08/01 未使用
		//Sub lusWait(lWait As Long)
		//    Dim t
		//    Dim l As Single
		//    l = BaseSecCount * (lWait / 1000000#)
		//    Do While l > 0
		//        t = Now * 24 * 60 * 60
		//        l = l - 1
		//    Loop
		//End Sub
		//
		//Sub lusInit()
		//    Dim t As Double
		//    BaseSecCount = 0
		//    t = Now * 24# * 60# * 60#
		//    Do While t = Now * 24# * 60# * 60#
		//    Loop
		//    t = Now * 24# * 60# * 60#
		//    Do While t = Now * 24# * 60# * 60#
		//        BaseSecCount = BaseSecCount + 1
		//    Loop
		//End Sub
		//v11.5削除ここまで by 間々田 2006/08/01 未使用
	}
}
