using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace CT30K
{
	public partial class CTProcess: UserControl
	{
		//内部変数
		private bool m_IsSync;
		private string m_Process;
		private int hProcess;
		private Process _Process = null;

		//イベント宣言
		public event EventHandler Terminated;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		'使用するAPIの宣言
		Private Declare Function OpenProcess Lib "kernel32" (ByVal dwDesiredAccess As Long, ByVal bInheritHandle As Long, ByVal dwProcessId As Long) As Long
		Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Long) As Long
		Private Declare Function WaitForSingleObject Lib "kernel32" (ByVal hHandle As Long, ByVal dwMilliseconds As Long) As Long
		Private Declare Function GetExitCodeProcess Lib "kernel32" (ByVal hProcess As Long, lpExitCode As Long) As Long
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

// 定数の定義
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		Private Const PROCESS_QUERY_INFORMATION = &H400&
		Private Const SYNCHRONIZE As Long = &H100000
		Private Const INFINITE    As Long = &HFFFF
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private const int STILL_ACTIVE = 0x103;

		/// <summary>
		/// 
		/// </summary>
		public CTProcess()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 
		/// </summary>
		public string Process
		{
			get { return m_Process; }
			set { m_Process = value; }
		}


		public bool IsSync
		{
			get { return m_IsSync; }
			set { m_IsSync = value; }
		}

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
	'記憶領域からプロパティ値を読み込みます｡
	Private Sub UserControl_ReadProperties(PropBag As PropertyBag)
		m_Process = PropBag.ReadProperty("Process", "")
		m_IsSync = PropBag.ReadProperty("IsSync", False)
	End Sub

	'プロパティ値を記憶領域に書き込みます｡
	Private Sub UserControl_WriteProperties(PropBag As PropertyBag)
		Call PropBag.WriteProperty("Process", m_Process, "")
		Call PropBag.WriteProperty("IsSync", m_IsSync, False)
	End Sub
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		public bool Start()
		{
			//戻り値セット
			bool result = false;

			ProcessStartInfo info = new ProcessStartInfo(m_Process);
			info.WindowStyle = ProcessWindowStyle.Minimized;

			//指定したプロセス開始
			_Process = System.Diagnostics.Process.Start(info);

			if (_Process.Id != 0)
			{
				//同期で実行する場合
				if (IsSync)
				{
					//終了するまで待機する
					_Process.WaitForExit();
					_Process.Dispose();
					_Process = null;
				}

				//非同期で実行する場合
				else
				{
					//'タイマ起動
					Timer1.Enabled = true;
				}

				result = true;
			}

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Timer1_Tick(object sender, EventArgs e)
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Dim ExitCode As Long
    
			'プロセスの終了状態を取得
			If GetExitCodeProcess(hProcess, ExitCode) = 0 Then Exit Sub
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//プロセスが終了していないときはここで抜ける
			if (_Process.ExitCode == STILL_ACTIVE)
			{
				return;
			}

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			'プロセスが終了した時：プロセスのハンドルを閉じる
//			Call CloseHandle(hProcess)
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			_Process.Dispose();
			_Process = null;

			//タイマ終了
			Timer1.Enabled = false;

			//プロセス終了通知イベント生成
			if (Terminated != null)
			{
				Terminated(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventSender"></param>
		/// <param name="eventArgs"></param>
		private void CTProcess_Resize(System.Object eventSender, System.EventArgs eventArgs)
		{
			this.Width = 32;
			this.Height = 32;
		}
	}
}
