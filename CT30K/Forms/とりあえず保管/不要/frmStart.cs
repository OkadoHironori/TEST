using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace CT30K
{
	internal partial class frmStart : System.Windows.Forms.Form
	{
		/// <summary>
		/// 
		/// </summary>
		public frmStart()
		{
			InitializeComponent();
		}

		//*******************************************************************************
		//機　　能： メッセージの表示
		//
		//           変数名          [I/O] 型        内容
		//引　　数： msgStr          [I/ ] String    表示するメッセージ
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public void Display(string msgStr = "")
		{
			if (!string.IsNullOrEmpty(msgStr))
			{
				lblMessage.Text = msgStr;
			}

			if (pgbProcessing.Value < 90)
			{
				pgbProcessing.Value = pgbProcessing.Value + 10;
			}

			this.Refresh();		//v9.7追加 by 間々田 2004-12-24

			//スキャン用じゃない場合はここで立ち上がり完了とする 'v17.53条件追加 byやまおか 2011/05/13
			if (modScaninh.scaninh.mechacontrol != 0)
			{
				modCT30K.CT30kNowStartingFlg = false;		//v17.20追加 byやまおか 2010/09/16
			}
		}

		//*******************************************************************************
		//機　　能： フォームロード時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void frmStart_Load(object sender, EventArgs e)
		{
//			Dim sts As Long
//
//			'ウィンドウを常に最前面表示にする
//			sts = SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE Or SWP_NOSIZE)

			//製品名とバージョンの表示：バージョン表示はコモンを使用
			AssemblyTitleAttribute attri = (AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute));

			lblProductName.Text = attri.Title + "\r" + modLibrary.RemoveNull(modT20kinf.t20kinf.version);

			//初期メッセージ
			lblMessage.Text = "Now Starting " + attri.Title;

			//プログレスバーの初期値は０
			pgbProcessing.Value = 0;

			//表示位置（中央よりやや上方に配置）
			Size screen = Screen.PrimaryScreen.Bounds.Size;
			this.Location = new Point((screen.Width - this.Width) / 2, (screen.Height - this.Height) / 2 - this.Height * 2 / 3);
		}

		//*******************************************************************************
		//機　　能： フォームアンロード時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void frmStart_FormClosed(object sender, FormClosedEventArgs e)
		{
			pgbProcessing.Value = 100;
		}
	}
}
