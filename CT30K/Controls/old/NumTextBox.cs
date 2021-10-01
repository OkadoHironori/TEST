using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace CT30K
{
    public partial class NumTextBox : UserControl
    {
		private float myDiscreteInterval = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		Dim myFormatString      As String
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		private bool myIncDecButton = false;
		private decimal LastValue = 0;

		private const int UpDownButtonWidth = 14;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		Const MAXLONG As Long = &H7FFFFFFF
//		Const MINLONG As Long = &H80000000
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//イベント宣言
		public new event EventHandler TextChanged;
		public event ValueChangedEventHandler ValueChanged;
		public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

		public class ValueChangedEventArgs : EventArgs
		{
			public decimal PreviousValue;
			public ValueChangedEventArgs(decimal PreviousValue) : base()
			{
				this.PreviousValue = PreviousValue;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public NumTextBox()
		{
			InitializeComponent();

			UserControl_Initialize();
		}

		//*******************************************************************************
		//機　　能： BackColor プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public override Color BackColor
		{
			get { return nudValue.BackColor; }
			set { nudValue.BackColor = value; }
		}

		//*******************************************************************************
		//機　　能： BorderStyle プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public new BorderStyle BorderStyle
		{
			get { return nudValue.BorderStyle; }
			set { nudValue.BorderStyle = value;	}
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
				NumTextBox_Resize(this, EventArgs.Empty);
			}
		}

		//*******************************************************************************
		//機　　能： CaptionFont プロパティ
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
			get { return lblCaption.Font; }
			set
			{
				lblCaption.Font = value;
				lblColon.Font = value;			//コロンにも適用
				lblUnit.Font = value;			//単位にも適用

				//高さを調整するため以下のようにする
				int temp = lblCaption.Width;
				lblCaption.AutoSize = true;
				lblCaption.AutoSize = false;
				lblCaption.Width = temp;

				NumTextBox_Resize(this, EventArgs.Empty);
			}
		}

		//*******************************************************************************
		//機　　能： DiscreteInterval プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public float DiscreteInterval
		{
			get { return myDiscreteInterval; }
			set
			{
				myDiscreteInterval = value;

                string s = value.ToString();
                int pos = s.IndexOf('.');

                // 小数点桁数の制御
                nudValue.DecimalPlaces = (pos == -1) ? 0 : s.Substring(pos + 1).Length;
			}
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
			get { return nudValue.Font; }
			set { nudValue.Font = value; }
		}

		//*******************************************************************************
		//機　　能： Max プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public decimal Max
		{
			get { return nudValue.Maximum * (decimal)myDiscreteInterval; }
			set { nudValue.Maximum = value / (decimal)myDiscreteInterval; }
		}

		//*******************************************************************************
		//機　　能： Min プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public decimal Min
		{
			get { return nudValue.Minimum * (decimal)myDiscreteInterval; }
			set { nudValue.Minimum = value / (decimal)myDiscreteInterval; }
		}

		//*******************************************************************************
		//機　　能： 最小値・最大値の設定
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public void SetMinMax(decimal theMin, decimal theMax)
		{
			nudValue.Minimum = theMin / (decimal)myDiscreteInterval;
			nudValue.Maximum = theMax / (decimal)myDiscreteInterval;
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
		public decimal Value
		{
			get { return nudValue.Value; }
			set
			{
				decimal PreviousValue = 0;
				if (this.Enabled)
				{
					PreviousValue = LastValue;
				}
				else
				{
					PreviousValue = nudValue.Value;
				}

				nudValue.Value = value;
				if (nudValue.Value != PreviousValue)
				{
					if (ValueChanged != null)
					{
						ValueChanged(this, new ValueChangedEventArgs(Value));
					}
				}

			}
		}

		//*******************************************************************************
		//機　　能： IncDecButton プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public bool IncDecButton
		{
			get { return myIncDecButton; }
			set
			{
				myIncDecButton = value;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				updValue.Visible = myIncDecButton
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				NumTextBox_Resize(this, EventArgs.Empty);
			}
		}

		//*******************************************************************************
		//機　　能： Locked プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public bool Locked
		{
			get { return nudValue.ReadOnly; }
			set
			{
				nudValue.ReadOnly = value;
//				txtValue.BackColor = IIf(newValue, vbButtonFace, vbWindowBackground)
//				txtValue.MousePointer = IIf(newValue, vbArrow, vbIbeam)
//				txtValue.TabStop = Not newValue
				this.Enabled = !value;
			}
		}

		//*******************************************************************************
		//機　　能： Unit プロパティ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		public string Unit
		{
			get { return lblUnit.Text; }
			set
			{
				lblUnit.Text = value;
				NumTextBox_Resize(this, EventArgs.Empty);
			}
		}

		//*******************************************************************************
		//機　　能： テキストボックスをクリア
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2009/08/19 (SS1)間々田   新規作成
		//*******************************************************************************
		public void Clear()
		{
			//テキストボックスをクリア
			nudValue.Text = "";
		}

		//*******************************************************************************
		//機　　能： リフレッシュ
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2009/08/19 (SS1)間々田   新規作成
		//*******************************************************************************
		public override void Refresh()
		{
			//リフレッシュ
			lblCaption.Refresh();
			nudValue.Refresh();
		}

		//*******************************************************************************
		//機　　能： テキストボックス変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		private void nudValue_TextChanged(object sender, EventArgs e)
		{
			//テキストボックス変更イベント生成
			if (TextChanged != null)
			{
				TextChanged(this, EventArgs.Empty);
			}
		}

		//*******************************************************************************
		//機　　能： 入力開始時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		private void nudValue_Enter(object sender, EventArgs e)
		{
			LastValue = nudValue.Value;
		}

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		'*******************************************************************************
		'機　　能： キー入力処理
		'
		'           変数名          [I/O] 型        内容
		'引　　数： なし
		'戻 り 値： なし
		'
		'補　　足： なし
		'
		'履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		'*******************************************************************************
		Private Sub txtValue_KeyPress(KeyAscii As Integer)

			Select Case KeyAscii
				Case vbKey0 To vbKey9, vbKeyBack, Asc("-")
				Case Else
					KeyAscii = 0
			End Select

		End Sub
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//*******************************************************************************
		//機　　能： 数値入力チェック処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//*******************************************************************************
		private void nudValue_Validating(object sender, CancelEventArgs e)
		{
			decimal d = 0;

			if (!decimal.TryParse(nudValue.Text, out d))
			{
				nudValue.Value = LastValue;
				e.Cancel = true;
				return;
			}

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//			Dim temp As Long
//			temp = Val(txtValue.Text)
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			int temp = (int)(d / (decimal)myDiscreteInterval * (decimal)myDiscreteInterval);

			this.Value = modLibrary.CorrectInRangeLong(temp, Min, Max);
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
		private void NumTextBox_Resize(object sender, EventArgs e)
		{
			//ヘッダ部分調整
			lblCaption.Location = new Point(0, (ClientRectangle.Height - lblCaption.Height) / 2);

			//コロン部分調整
			lblColon.Location = new Point(lblCaption.Width, (ClientRectangle.Height - lblColon.Height) / 2);

			//単位部分調整
			lblUnit.Location = new Point(ClientRectangle.Width - lblUnit.Width, (ClientRectangle.Height - lblUnit.Height) / 2);

			//単位の幅
            // 単位の幅
            int UnitWidth = string.IsNullOrEmpty(lblUnit.Text) ? 0 : lblUnit.Width + 30;
	
			//テキスト部分調整
            panel1.SetBounds(lblColon.Right, 0, ClientRectangle.Width - (lblCaption.Width + lblColon.Width + UnitWidth), ClientRectangle.Height);

			Size size = panel1.ClientSize;

			size.Width = myIncDecButton? panel1.ClientSize.Width : panel1.ClientSize.Width + UpDownButtonWidth;
			size.Height = nudValue.Height;

			nudValue.Size = size;
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
    
			txtValue.BorderStyle = PropBag.ReadProperty("BorderStyle", ccFixedSingle)
			txtValue.BackColor = PropBag.ReadProperty("BackColor", vbWindowBackground)
			lblUnit.Caption = PropBag.ReadProperty("Unit", "")
			lblCaption.Caption = PropBag.ReadProperty("Caption", "Caption")
			lblCaption.Alignment = PropBag.ReadProperty("CaptionAlignment", vbLeftJustify)
			CaptionWidth = PropBag.ReadProperty("CaptionWidth", 420)
			Set CaptionFont = PropBag.ReadProperty("CaptionFont", Ambient.Font)
			Set Font = PropBag.ReadProperty("Font", Ambient.Font)
			DiscreteInterval = PropBag.ReadProperty("DiscreteInterval", 1)
			Max = PropBag.ReadProperty("Max", MAXLONG)
			Min = PropBag.ReadProperty("Min", MINLONG)
			Value = PropBag.ReadProperty("Value", 0)
			IncDecButton = PropBag.ReadProperty("IncDecButton", False)
			Locked = PropBag.ReadProperty("Locked", False)

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
    
			Call PropBag.WriteProperty("BorderStyle", txtValue.BorderStyle, ccFixedSingle)
			Call PropBag.WriteProperty("BackColor", txtValue.BackColor, vbWindowBackground)
			Call PropBag.WriteProperty("Unit", lblUnit.Caption, "")
			Call PropBag.WriteProperty("Caption", lblCaption.Caption, "Caption")
			Call PropBag.WriteProperty("CaptionAlignment", lblCaption.Alignment, vbLeftJustify)
			Call PropBag.WriteProperty("CaptionWidth", CaptionWidth, 420)
			Call PropBag.WriteProperty("CaptionFont", CaptionFont, Ambient.Font)
			Call PropBag.WriteProperty("Font", Font, Ambient.Font)
			Call PropBag.WriteProperty("DiscreteInterval", DiscreteInterval, 1)
			Call PropBag.WriteProperty("Max", Max, MAXLONG)
			Call PropBag.WriteProperty("Min", Min, MINLONG)
			Call PropBag.WriteProperty("Value", Value, 0)
			Call PropBag.WriteProperty("IncDecButton", IncDecButton, False)
			Call PropBag.WriteProperty("Locked", Locked, False)

		End Sub
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

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
			myDiscreteInterval = 1;
			myIncDecButton = false;
		}
	}
}