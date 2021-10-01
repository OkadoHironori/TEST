using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace CT30K
{
    public partial class NumTextBox : UserControl
    {
		private float myDiscreteInterval = 0;

		private bool myIncDecButton = false;
		//private decimal LastValue = 0;
        private bool startflg = false;

        private decimal updwValue = 0;
        private string LastText = "0";

        //private const int UpDownButtonWidth = 15;
        private const int UpDownButtonWidth = 19;

		//イベント宣言
		public new event EventHandler TextChanged;
		public event ValueChangedEventHandler ValueChanged;
		public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);
        public event TextChangeEventHandler TextChange;
        public delegate void TextChangeEventHandler();

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
        public new Color BackColor
        {
            //get { return nudValue.BackColor; }
            //set { nudValue.BackColor = value; }
            get { return lblCaption.BackColor; }
            set
            {
                lblCaption.BackColor = value;
                lblColon.BackColor = value;
                lblUnit.BackColor = value;
                base.BackColor = value;
            }
        }

        public Color TextBackColor
        {
            get { return panel1.BackColor; }
            set
            {
                panel1.BackColor = value;
                txtValue.BackColor = panel1.BackColor;

            }
        }

        public Color TextForeColor
        {
            get { return txtValue.ForeColor; }
            set { txtValue.ForeColor = value;}
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
            get { return panel1.BorderStyle; }
            set
            {
                panel1.BorderStyle = value;

                NumTextBox_Resize(this, EventArgs.Empty);
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
			get { return lblCaption.Text; }
			set { lblCaption.Text = value; }
		}

        //v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここから
        //*******************************************************************************
        //機　　能： Text プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v18.00 2011/08/09   やまおか    新規作成
        //*******************************************************************************

        public override string Text
        {
            get { return txtValue.Text; }
            set
            {
                txtValue.Text = value;
                if (TextChange != null)
                {
                    TextChange();
                }
            }
        }
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここまで



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
		//*******************************************************************************//
		public new Font Font
		{
			get
			{	
                //Font f = base.Font;
                //return nudValue.Font;
                return txtValue.Font;
			}

			set
			{
				Font f = base.Font;
				nudValue.Font = value;
                txtValue.Font = value;
                NumTextBox_Resize(this, EventArgs.Empty);

                base.Font = f;
			}
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
			get 
            {   return nudValue.Maximum * (decimal)myDiscreteInterval; }
			set 
            { 
                nudValue.Maximum = value / (decimal)myDiscreteInterval; 
                //nudValue.Maximum = Math.Round( value / (decimal)myDiscreteInterval,MidpointRounding.AwayFromZero); 
            }
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
			set 
            { 
                nudValue.Minimum = value / (decimal)myDiscreteInterval;
                //nudValue.Minimum = Math.Round(value / (decimal)myDiscreteInterval, MidpointRounding.AwayFromZero); 
            }
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
            //nudValue.Maximum = Math.Round(theMax / (decimal)myDiscreteInterval, MidpointRounding.AwayFromZero);
            //nudValue.Minimum = Math.Round(theMin / (decimal)myDiscreteInterval, MidpointRounding.AwayFromZero); 

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
            ////get { return nudValue.Value; }
            ////set
            ////{
            ////    decimal PreviousValue = 0;
 
            ////    if (this.Enabled)
            ////    {
            ////        PreviousValue = LastValue;
            ////    }
            ////    else
            ////    {
            ////        PreviousValue = nudValue.Value;
            ////    }

            ////    nudValue.Value = value;
            ////    if (nudValue.Value != PreviousValue)
            ////    {
            ////        if (ValueChanged != null)
            ////        {
            ////            ValueChanged(this, new ValueChangedEventArgs(Value));
            ////        }
            ////    }

            ////}

            get {
                    decimal d = 0;
                    if (!decimal.TryParse(txtValue.Text, out d))
                    {
                        d = 0;
                    }
                    return d;
                }
            set
            {
                //decimal PreviousValue = 0;
                string  PreviousText = "0";
                if (this.Enabled)
                {
                    //PreviousValue = LastValue;
                    PreviousText = LastText;
                }
                else
                {
                    //PreviousValue = nudValue.Value;
                    PreviousText = txtValue.Text;


                }
                //txtValue.Text = value.ToString(myDiscreteInterval.ToString());
                txtValue.Text = value.ToString(modLibrary.GetFormatString(myDiscreteInterval));

                //string s = myDiscreteInterval.ToString();
                //int pos = s.IndexOf('.');
                //// 小数点桁数の制御
                //pos = (pos == -1) ? 0 : (pos + 1);
                //decimal val = Math.Round(value,pos, MidpointRounding.AwayFromZero);
                //txtValue.Text = val.ToString();

                //nudValue.Value = value;
                //if (nudValue.Value != PreviousValue)
                if (txtValue.Text != PreviousText)
                {
                    if (ValueChanged != null)
                    {
                        LastText = txtValue.Text;
                        //Rev24.00 追加 変換可能かチェック by長野 2016/06/16
                        decimal ret = 0;
                        if (decimal.TryParse(PreviousText, out ret) == true)
                        {
                            //Rev20.00 LastTextは上書きする by長野 2015/01/28
                            //変更2015/1/24hata
                            //ValueChanged(this, new ValueChangedEventArgs(Value));
                            ValueChanged(this, new ValueChangedEventArgs(Convert.ToDecimal(PreviousText)));
                        }
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
        public bool ReadOnly
        {
            //            get { return txtValue.ReadOnly; }
            //            set
            //            {
            //                //nudValue.ReadOnly = value;
            //                txtValue.ReadOnly = value;

            ////				txtValue.BackColor = IIf(newValue, vbButtonFace, vbWindowBackground)
            ////				txtValue.MousePointer = IIf(newValue, vbArrow, vbIbeam)
            ////				txtValue.TabStop = Not newValue
            //                this.Enabled = !value;
            //			}

            get { return txtValue.ReadOnly; }
            set
            {
                nudValue.ReadOnly = value;
                txtValue.ReadOnly = value;
                //panel1.Enabled = !value;

                //				txtValue.BackColor = IIf(newValue, vbButtonFace, vbWindowBackground)
                //				txtValue.MousePointer = IIf(newValue, vbArrow, vbIbeam)
                //				txtValue.TabStop = Not newValue
                
                //this.Enabled = !value;
                txtValue.BackColor = panel1.BackColor;
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
                //int temp = lblUnit.Width;
                //lblUnit.AutoSize = true;
                lblUnit.Text = value;
                //lblUnit.AutoSize = false;
                //lblUnit.Width = temp;
                                
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
			//nudValue.Text = "";
            txtValue.Text = "";
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
			//nudValue.Refresh();
		    txtValue.Refresh();
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
        //private void nudValue_TextChanged(object sender, EventArgs e)
        //{
        //    ////テキストボックス変更イベント生成
        //    //if (TextChanged != null)
        //    //{
        //    //    TextChanged(this, EventArgs.Empty);
        //    //}
        //}
        private void nudValue_ValueChanged(object sender, EventArgs e)
        {
            decimal d1 = 0;
            decimal d2 = 0;

            if (!decimal.TryParse(txtValue.Text, out d1))
            {
                d1 = 0;
            }

            if (nudValue.Value == d1) return;

            d2 = d1 + (nudValue.Value - updwValue) * nudValue.Increment;

            //2014/11/06hata キャストの修正
            //if (d2 > nudValue.Maximum)
            //    d2 = nudValue.Maximum;
            //else if (d1 < nudValue.Minimum)
            //    d2 = nudValue.Minimum;
            //int temp = (int)(d2 / (decimal)myDiscreteInterval * (decimal)myDiscreteInterval);
            int temp = Convert.ToInt32(Convert.ToInt32(d2 / (decimal)myDiscreteInterval) * myDiscreteInterval);
            temp = CorrectInRange(temp, (int)nudValue.Minimum, (int)nudValue.Maximum);
            
            txtValue.Text = temp.ToString();
            updwValue = temp;
            nudValue.Value = temp;
            
            //txtValue.Text = d2.ToString();
            //updwValue = d2;
            //nudValue.Value = d2;

        }


        private void txtValue_TextChanged(object sender, EventArgs e)
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
            //LastValue = nudValue.Value;
		}

        private void txtValue_Enter(object sender, EventArgs e)
        {
            //decimal d = 0;
            //if (!decimal.TryParse(txtValue.Text, out d))
            //{
            //    LastValue = d;
            //}
            LastText = txtValue.Text;
 
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
//            decimal d = 0;

//            if (!decimal.TryParse(nudValue.Text, out d))
//            {
//                nudValue.Value = LastValue;
//                e.Cancel = true;
//                return;
//            }

//            int temp = (int)(d / (decimal)myDiscreteInterval * (decimal)myDiscreteInterval);

//            this.Value = CorrectInRange(temp, (int)this.Min, (int)this.Max);
        }

       private void txtValue_Validating(object sender, CancelEventArgs e)
       {
            decimal d = 0;

            if (!decimal.TryParse(txtValue.Text, out d))
            {
                //nudValue.Value = LastValue;
                txtValue.Text = LastText;
                //Rev23.20 Enterイベントとループしてしまうため条件を追加 by長野 2015/12/21
                //e.Cancel = true;
                return;
            }
            //2014/11/06hata キャストの修正
            //int temp = (int)(d / (decimal)myDiscreteInterval * (decimal)myDiscreteInterval);
            int temp = Convert.ToInt32(Convert.ToInt32(d / (decimal)myDiscreteInterval) * myDiscreteInterval);
            this.Value = CorrectInRange(temp, (int)this.Min, (int)this.Max);

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
            lblCaption.Height = ClientRectangle.Height;
            lblCaption.Location = new Point(-2, (ClientRectangle.Height - lblCaption.Height) / 2);
            
            //コロン部分調整
            lblColon.Location = new Point(lblCaption.Right - 4, (ClientRectangle.Height - lblColon.Height) / 2);
            
            //単位部分調整
            lblUnit.Location = new Point(ClientRectangle.Width - lblUnit.Width, (ClientRectangle.Height - lblUnit.Height) / 2);
            
            //単位の幅
            //int UnitWidth = string.IsNullOrEmpty(lblUnit.Text) ? 0 : lblUnit.Width;// + 2;

            //テキスト部分調整           
            panel1.SetBounds(lblColon.Right - 2, 0, lblUnit.Left - (lblColon.Right - 2), ClientRectangle.Height);
            int w = nudValue.Width;
            if (!myIncDecButton) w = 0;
            txtValue.SetBounds(0, (panel1.ClientSize.Height - txtValue.Height) / 2, panel1.ClientSize.Width - w, txtValue.Height);
            nudValue.Location = new Point(txtValue.Right, txtValue.Top + (txtValue.Height - nudValue.Height) / 2);

          
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
			myDiscreteInterval = 1;
			myIncDecButton = false;

		}

        //Loadを追加
        //初期化
        private void NumTextBox_Load(object sender, EventArgs e)
        {
            if (startflg) return;

            startflg = true;
            //LastValue = this.Value;
            LastText = this.Value.ToString();

        }

        #region 値を範囲内に収まるように補正します
        /// <summary>
        /// 値を範囲内にする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static T CorrectInRange<T>(T target, T min, T max) where T : IComparable
        {
            if (target.CompareTo(min) < 0)
                return min;
            else if (target.CompareTo(max) > 0)
                return max;
            else
                return target;
        }
        #endregion


		//'*******************************************************************************
		//'機　　能： キー入力処理
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//'*******************************************************************************
/*		
        Private Sub txtValue_KeyPress(KeyAscii As Integer)

			Select Case KeyAscii
				Case vbKey0 To vbKey9, vbKeyBack, Asc("-")
				Case Else
					KeyAscii = 0
			End Select

		End Sub
*/
        private void txtValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.D0:
                case (char)Keys.D1:
                case (char)Keys.D2:
                case (char)Keys.D3:
                case (char)Keys.D4:
                case (char)Keys.D5:
                case (char)Keys.D6:
                case (char)Keys.D7:
                case (char)Keys.D8:
                case (char)Keys.D9:
                case (char)Keys.Back:
                case (char)Keys.Subtract:
                //case (char)Keys.Decimal:
                //case (char)45: 
                //case (char)46:
                    break;

                //case (char)Keys.Return:
                default:
                    e.KeyChar = (char)0;
                    e.Handled = true;
                    break;
            }
        }


 
    }
}