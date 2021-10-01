using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CT30K
{
    public partial class CTCheckBox : UserControl
    {
		private static readonly Color OnColor = Color.Lime;
		private static readonly Color OffColor = Color.White;

		public event EventHandler CheckedByClick;

		/// <summary>
		/// 
		/// </summary>
		public CTCheckBox()
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
			get { return myPicture.BackColor == OnColor; }
			set
			{
				if (value == this.Value)
				{
					return;
				}

				myPicture.BackColor = value ? OnColor : OffColor;

//				myPicture.Enabled = True

				if (value)
				{
					this.Checked = false;	//チェックを外す
				}
			}
		}

		//*******************************************************************************
		//機　　能： Checked プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public bool Checked
		{
			get { return myPicture.Image != null; }
			set
			{
				if (value == this.Checked)
				{
					return;
				}

				if (value)
				{
					myPicture.Image = imageList.Images["Checked.ico"];
				}
				else
				{
					myPicture.Image = null;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void myPicture_Click(object sender, EventArgs e)
		{
			//すでに値がセットされている場合何もしない   'v15.0追加 by 間々田 2009/05/15
			if (Value)
			{
				return;
			}

			this.Checked = !this.Checked;

			if (this.Checked && (!Value))
			{
//				myPicture.Enabled = False

				if (CheckedByClick != null)
				{
					CheckedByClick(this, EventArgs.Empty);
				}
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
		private void CTCheckBox_Resize(object sender, EventArgs e)
		{
			this.Width = myPicture.Width;
			this.Height = myPicture.Height;
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
    
			Me.Value = PropBag.ReadProperty("Value", False)
			Me.Checked = PropBag.ReadProperty("Checked", False)

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
    
			Call PropBag.WriteProperty("Value", Me.Value, False)
			Call PropBag.WriteProperty("Checked", Me.Checked, False)

		End Sub
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

    }
}
