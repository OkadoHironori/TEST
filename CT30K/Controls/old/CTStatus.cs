using System;
using System.Windows.Forms;
using System.Drawing;

namespace CT30K
{
    /// <summary>
    /// ＣＴステータスコントロール
    /// </summary>
    public partial class CTStatus : UserControl
    {
		//イベント宣言
		public event EventHandler Changed;

		/// <summary>
		/// 
		/// </summary>
		public CTStatus()
		{
			InitializeComponent();

			UserControl_Initialize();
		}

		//*******************************************************************************
		//機　　能： Font プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public override Font Font
		{
			get { return lblCaption.Font; }
			set
			{
				lblCaption.Font = value;
				lblColon.Font = value;
				lblStatus.Font = value;

				int temp = lblCaption.Width;
				lblCaption.AutoSize = true;
				lblCaption.AutoSize = false;
				lblCaption.Width = temp;
			}
		}

		//*******************************************************************************
		//機　　能： Caption プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成Alignment
		//*******************************************************************************
		public string Caption
		{
			get { return lblCaption.Text; }
			set { lblCaption.Text = value; }
		}

		//*******************************************************************************
		//機　　能： CaptionAlignment プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public ContentAlignment CaptionAlignment
		{
			get { return lblCaption.TextAlign; }
			set { lblCaption.TextAlign = value; }
		}

		//*******************************************************************************
		//機　　能： CaptionWidth プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************

		public int CaptionWidth
		{
			get { return lblCaption.Width; }
			set
			{
				lblCaption.Width = value;

				CTStatus_Resize(this, EventArgs.Empty);
			}
		}

		//*******************************************************************************
		//機　　能： Status プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public string Status
		{
			get { return lblStatus.Text; }
			set { lblStatus.Text = value; }
		}

		//*******************************************************************************
		//機　　能： ステータス変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		private void lblStatus_Change()
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			With lblStatus
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			if (lblStatus.Text == StringTable.GC_STS_STANDBY_OK)
			{
				lblStatus.BackColor = Color.Lime;		//準備完了
			}
			else if (lblStatus.Text == StringTable.GC_STS_STANDBY_NG)
			{
				lblStatus.BackColor = Color.Yellow;		//準備未完
			}
			else if (lblStatus.Text == StringTable.GC_STS_STANDBY_NG2)
			{
				lblStatus.BackColor = Color.Yellow;		//準備未完
			}
			else if (lblStatus.Text == StringTable.GC_STS_MovementLimit)
			{
				lblStatus.BackColor = Color.Cyan;		//動作限
			}
			else if (lblStatus.Text == Resources.str12363 || lblStatus.Text == Resources.str12364)
			{
				lblStatus.BackColor = Color.Cyan;		//正転限/逆転限
			}
			else if (lblStatus.Text == StringTable.GC_STS_STOP)
			{
				lblStatus.BackColor = Color.Lime;		//停止中
			}
			else if (lblStatus.Text == Resources.str12114)
			{
				lblStatus.BackColor = Color.Lime;		//停止中(英語はReady)
			}
			else if (lblStatus.Text == StringTable.GC_STS_BUSY)
			{
				lblStatus.BackColor = Color.Red;		//動作中
			}
			else if (lblStatus.Text == StringTable.GC_STS_Scan)
			{
				lblStatus.BackColor = Color.Red;		//動作中（英語はScanning）
			}
			else if (lblStatus.Text == StringTable.GC_STS_CPU_BUSY)
			{
				lblStatus.BackColor = Color.Red;		//処理中
			}
			else if (lblStatus.Text == StringTable.GC_Xray_Error)
			{
				lblStatus.BackColor = Color.Red;		//異常
			}
			else if (lblStatus.Text == StringTable.GC_STS_CAPT_NG)
			{
				lblStatus.BackColor = Color.Yellow;		//データ収集異常終了
			}
			else if (lblStatus.Text == StringTable.GC_STS_CAPT_OK)
			{
				lblStatus.BackColor = Color.Lime;		//データ収集完了
			}
			else if (lblStatus.Text == StringTable.GC_STS_X_AVAIL)
			{
				lblStatus.BackColor = Color.Red;		//Ｘ線アベイラブル待ち
			}
			else if (lblStatus.Text == StringTable.GC_STS_CAPTURE)
			{
				lblStatus.BackColor = Color.Red;		//データ収集中
			}
			else if (lblStatus.Text == StringTable.GC_STS_TABLE_MOVING)
			{
				lblStatus.BackColor = Color.Red;		//テーブル移動中
			}
			else if (lblStatus.Text == StringTable.GC_STS_PHANTOM_MOVING)
			{
				lblStatus.BackColor = Color.Red;		//ファントム移動中
			}
			else if (lblStatus.Text == StringTable.GC_STS_IGNORE)
			{
				lblStatus.BackColor = Color.Cyan;		//対象外
			}

			//イベント生成
			if (Changed != null)
			{
				Changed(this, EventArgs.Empty);
			}
		}

		//*******************************************************************************
		//機　　能： サイズ変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		private void CTStatus_Resize(object sender, EventArgs e)
		{
			//ヘッダ部分調整
			lblCaption.SetBounds(0, (ClientRectangle.Height - lblCaption.Height) / 2, lblCaption.Width, lblCaption.Height);

			//コロン部分調整
			lblColon.SetBounds(lblCaption.Width, (ClientRectangle.Height - lblCaption.Height) / 2, lblColon.Width, lblColon.Height);

			//ステータス部分調整
			lblStatus.SetBounds(lblCaption.Width + lblColon.Width, 0, ClientRectangle.Width - (lblCaption.Width + lblColon.Width), ClientRectangle.Height);
		}

		//*******************************************************************************
		//機　　能： 初期化処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		private void UserControl_Initialize()
		{
			lblCaption.Text = Resources.ResourceManager.GetString("str" + Convert.ToString(StringTable.IDS_Status));
			lblColon.Text = Resources.ResourceManager.GetString("str" + Convert.ToString(StringTable.IDS_Colon));
		}

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		'*******************************************************************************
		'機　　能： 記憶領域からプロパティ値を読み込みます
		'
		'           変数名          [I/O] 型        内容
		'引　　数： なし
		'戻 り 値： なし
		'
		'補　　足： なし
		'
		'履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		'*******************************************************************************
		Private Sub UserControl_ReadProperties(PropBag As PropertyBag)
    
			lblStatus.Caption = PropBag.ReadProperty("Status", "")
			lblCaption.Caption = PropBag.ReadProperty("Caption", "")
			lblCaption.Alignment = PropBag.ReadProperty("CaptionAlignment", vbLeftJustify)
			Set Font = PropBag.ReadProperty("Font", Ambient.Font)
			CaptionWidth = PropBag.ReadProperty("CaptionWidth", 15)

		End Sub

		'*******************************************************************************
		'機　　能： プロパティ値を記憶領域に書き込みます
		'
		'           変数名          [I/O] 型        内容
		'引　　数： なし
		'戻 り 値： なし
		'
		'補　　足： なし
		'
		'履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		'*******************************************************************************
		Private Sub UserControl_WriteProperties(PropBag As PropertyBag)
    
			Call PropBag.WriteProperty("Status", lblStatus.Caption, "")
			Call PropBag.WriteProperty("Caption", lblCaption.Caption, "")
			Call PropBag.WriteProperty("CaptionAlignment", lblCaption.Alignment, vbLeftJustify)
			Call PropBag.WriteProperty("Font", Font, Ambient.Font)
			Call PropBag.WriteProperty("CaptionWidth", CaptionWidth, 15)

		End Sub
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

    }
}
