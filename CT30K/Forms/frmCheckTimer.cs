using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace CT30K
{
	public partial class frmCheckTimer : Form
	{

		private List<TimerInfo> TimerInfos = null;
		private class TimerInfo
		{
			private Timer _timer;
			private string _parentName;
			private string _timerName;

			public Timer Timer
			{
				get { return _timer; }
			}
			public string ParentName
			{
				get { return _parentName; }
			}
			public string TimerName
			{
				get { return _timerName; }
			}

			public TimerInfo(Timer Timer, string ParentName, string TimerName)
			{
				_timer = Timer;
				_parentName = ParentName;
				_timerName = TimerName;
			}
		}

		private CTButton[] stsTimer = null;				// 【C#コントロールで代用】
		private Label[] stsTimerCaption = null;			// 【C#コントロールで代用】

		private static frmCheckTimer _Instance = null;

		public frmCheckTimer()
		{
			InitializeComponent();

			#region コントロール配列

			stsTimer = new CTButton[] { stsTimer0,
										stsTimer1,
										stsTimer2,
										stsTimer3,
										stsTimer4,
										stsTimer5,
										stsTimer6,
										stsTimer7,
										stsTimer8,
										stsTimer9,
										stsTimer10,
										stsTimer11,
										stsTimer12,
										stsTimer13,
										stsTimer14,
										stsTimer15,
										stsTimer16,
										stsTimer17 };

			stsTimerCaption = new Label[] { stsTimerCaption0,
											stsTimerCaption1,
											stsTimerCaption2,
											stsTimerCaption3,
											stsTimerCaption4,
											stsTimerCaption5,
											stsTimerCaption6,
											stsTimerCaption7,
											stsTimerCaption8,
											stsTimerCaption9,
											stsTimerCaption10,
											stsTimerCaption11,
											stsTimerCaption12,
											stsTimerCaption13,
											stsTimerCaption14,
											stsTimerCaption15,
											stsTimerCaption16,
											stsTimerCaption17 };

			#endregion
		}

		public static frmCheckTimer Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmCheckTimer();
				}

				return _Instance;
			}
		}


		private void cmdUpdate_Click(object sender, EventArgs e)
		{
			this.Update();
		}


		private void frmCheckTimer_Load(object sender, EventArgs e)
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Dim theForm As Form
			Dim theControl As Control

			'エラーは無視する
			On Error Resume Next
			ReDim Timers(0)

			'見つかったら直ちに抜ける
			For Each theForm In Forms
				For Each theControl In theForm.Controls
					If TypeName(theControl) = "Timer" Then
						ReDim Preserve Timers(UBound(Timers) + 1)
						Set Timers(UBound(Timers)) = theControl
					End If
				Next
			Next
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			// パブリック・非パブリックを問わず検索する
			BindingFlags bindFlag = BindingFlags.Public | 
									BindingFlags.NonPublic | 
									BindingFlags.Static |
									BindingFlags.Instance;
			
			//エラーは無視する
			TimerInfos = new List<TimerInfo>();
			TimerInfos.Add(null);

			foreach (Form form in Application.OpenForms)
			{
				try
				{
					FieldInfo[] FieldInfos = form.GetType().GetFields(bindFlag);

					foreach (FieldInfo info in FieldInfos)
					{
						if (info.FieldType == typeof(Timer))
						{
							TimerInfos.Add(new TimerInfo((Timer)info.GetValue(form), form.Name, info.Name));
						}
					}
				}
				catch
				{  }
			}
		}


		private void MyUpdate()
		{
#region 【C#コントロールで代用】
/*
			Dim i As Integer
			For i = 1 To UBound(Timers)
				With Timers(i)
					stsTimer(i).Value = .Enabled
					stsTimer(i).Caption = .Parent.Name & "." & .Name & "：" & CStr(.Interval)
				End With
			Next
*/
#endregion 【C#コントロールで代用】

			for (int i = 1; i < TimerInfos.Count; i++)
			{
				stsTimer[i].Value = TimerInfos[i].Timer.Enabled;
				stsTimerCaption[i].Text = TimerInfos[i].ParentName + "." 
										+ TimerInfos[i].TimerName + "：" 
										+ TimerInfos[i].Timer.Interval.ToString();
			}
		}

	}
}
