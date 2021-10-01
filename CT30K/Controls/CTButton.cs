using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CT30K
{
	public partial class CTButton: UserControl
	{
		private bool myValue = false;
		private Color myOnColor = Color.Lime;
		private Color myOffColor = modCT30K.DarkGreen;

		//２重呼び出し防止
		private bool myButton_Click_BUSYNOW;

		//イベント宣言
		public new event EventHandler Click;
		public event EventHandler ValueChanged;

		/// <summary>
		/// 
		/// </summary>
		public CTButton()
		{
			InitializeComponent();
		}

		//*******************************************************************************
		//機　　能： Value プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public bool Value
		{
			get { return myValue; }
			set
			{
				myButton.Enabled = true;

				if (value == myValue)
				{
					return;
				}

				myValue = value;

				//myButton.BackColor = IIf(myValue, vbGreen, DarkGreen)

				myButton.BackColor = myValue ? myOnColor : myOffColor;

				if (ValueChanged != null)
				{
					ValueChanged(this, EventArgs.Empty);
				}
			}
		}

		//*******************************************************************************
		//機　　能： Enabled プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public new bool Enabled
		{
			get { return myButton.Enabled; }
			set
			{
				myButton.Enabled = value;
				base.Enabled = value;
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
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public string Caption
		{
			get { return myButton.Text; }
			set { myButton.Text = value; }
		}
 
		//*******************************************************************************
		//機　　能： OnColor プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public Color OnColor
		{
			get { return myOnColor; }
			set
			{
				myOnColor = value;
				myButton.BackColor = myValue ? myOnColor : myOffColor;
			}
		}

		//*******************************************************************************
		//機　　能： OffColor プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public Color OffColor
		{
			get { return myOffColor; }
			set
			{
				myOffColor = value;
				myButton.BackColor = myValue ? myOnColor : myOffColor;
			}
		}

		//*******************************************************************************
		//機　　能： Picture プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public Image Picture
		{
			get { return myButton.Image; }
			set { myButton.Image = value; }
		}

		//*******************************************************************************
		//機　　能： Header プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************

		public string Header
		{
			get { return lblHeader.Text; }
			set { lblHeader.Text = value; }
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
		public Font CaptionFont
		{
			get { return myButton.Font; }
			set { myButton.Font = value; }
		}

		//*******************************************************************************
		//機　　能： ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		private void myButton_Click(object sender, EventArgs e)
		{
			//状態(True:実行中,False:停止中)
			if (myButton_Click_BUSYNOW)
			{
				return;
			}

			myButton_Click_BUSYNOW = true;

			myButton.Enabled = false;

			if (Click != null)
			{
				Click(this, EventArgs.Empty);
			}

			//元の状態に戻す
			myButton_Click_BUSYNOW = false;
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
		private void CTButton_Resize(object sender, EventArgs e)
		{
			//ヘッダ調整
            //2014/11/06hata キャストの修正
            //lblHeader.SetBounds(0, (ClientRectangle.Height - lblHeader.Height), lblHeader.Width, lblHeader.Height);
            lblHeader.SetBounds(0, Convert.ToInt32((ClientRectangle.Height - lblHeader.Height) / 2F), lblHeader.Width, lblHeader.Height);

			//ボタン調整
			if (!string.IsNullOrEmpty(lblHeader.Text))
			{
				myButton.SetBounds(lblHeader.Width, 0, ClientRectangle.Width - lblHeader.Width, ClientRectangle.Height);
			}
			else
			{
				myButton.SetBounds(0, 0, ClientRectangle.Width, ClientRectangle.Height);
			}
		}

	}
}
