using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CT30K
{
    public partial class CTSliderV : UserControl
    {
        //EventHandlerを定義
        [Browsable(true)]
        [Category("アクション")]
        [Description("値が変更されたときに発生します。")]
        public event EventHandler ValueChanged;

        public new ScrollEventHandler Scroll;
        public new EventHandler MouseCaptureChanged;    //追加2014/12/17hata
        public new MouseEventHandler MouseUp;           //追加2014/12/17hata

        //内部変数
        private bool _Reverse = false;
        private TicksLabelType _TicksLabel = TicksLabelType.TicksLabelOn;
        private int setValue;
        private int trueValue;
        private bool _ControlLock = false;
        private bool _TicksLine = true;
        private Font font;    //追加2014/12/15hata        
        private const int ButonOffset = 16; //追加2014/12/15hata
        private int maximum = 0;    //追加2014/12/15hata        

        //追加2014/12/15hata
        //Ticks制御タイプ
        public enum TicksLabelType
        {
            TicksLabelOn,       // ラベルをすべて表示する
            TicksLabelOff,      // ラベルをすべて表示しない
            TicksLabelMidOff    // 中央のラベルを表示しない
        }

        //コンストラクタ
        public CTSliderV()
        {
            InitializeComponent();

            // イベント定義
            InitializeEventHandler();

            font = vScrollBar1.Font;    //追加2014/12/15hata
            base.Font = font;           //追加2014/12/15hata
            lblTop.Font = font;         //追加2014/12/15hata
            lblMid.Font = font;         //追加2014/12/15hata
            lblBottom.Font = font;      //追加2014/12/15hata
            Maximum = 100;              //追加2014/12/15hata
            Minimum = 0;                //追加2014/12/15hata
            LargeChange = 5;            //追加2014/12/15hata      
            SmallChange = 1;            //追加2014/12/15hata
            trueValue = vScrollBar1.Value;
            setValue = vScrollBar1.Value;
        }

        #region イベント定義
        /// <summary>
        /// イベント定義
        /// </summary>
        private void InitializeEventHandler()
        {
            // スクロールバー値変更時処理
            vScrollBar1.ValueChanged += (sender, e) =>
            {

                if (_ControlLock) return;

                setValue = vScrollBar1.Value;
                if (_Reverse)
                {
                    //変更2014/12/15hata
                    //trueValue = vScrollBar1.Maximum - setValue;
                    trueValue = maximum - setValue + vScrollBar1.Minimum;
                }
                else
                {
                    trueValue = setValue;
                }

                if (ValueChanged != null)
                {
                    if (this.CanFocus & vScrollBar1.CanFocus & this.ActiveControl == vScrollBar1) this.Focus();  //追加2015/01/24hata
                    ValueChanged(this, EventArgs.Empty);
                    if (pnlDummy.CanFocus) pnlDummy.Focus();    //追加2015/01/24hata
                }
            };

            // スクロールバー値変更時処理
            vScrollBar1.Scroll += (sender, e) =>
            {
                if (_ControlLock)
                {
                    e.NewValue = e.OldValue;
                    //vScrollBar1.Value = setValue;
                    return;
                }

                setValue = vScrollBar1.Value;
                
                if (_Reverse)
                {
                    //変更2014/12/15hata
                    //trueValue = vScrollBar1.Maximum - setValue + vScrollBar1.Minimum;
                    trueValue = maximum - setValue + vScrollBar1.Minimum;
                }
                else
                {
                    trueValue = setValue;
                }
                if (Scroll != null)
                {
                    if (this.CanFocus & vScrollBar1.CanFocus & this.ActiveControl == vScrollBar1) this.Focus();  //追加2015/01/24hata
                    Scroll(this, e);
                    if (pnlDummy.CanFocus) pnlDummy.Focus();    //追加2015/01/24hata
                }
            };

            //追加2014/12/17hata
            // MouseUp時の処理
            vScrollBar1.MouseUp += (sender, e) =>
            {
                if (MouseUp != null)
                {
                    if (this.CanFocus & vScrollBar1.CanFocus & this.ActiveControl == vScrollBar1) this.Focus();  //追加2015/01/24hata
                    MouseUp(this, e);
                    if (pnlDummy.CanFocus) pnlDummy.Focus();    //追加2015/01/24hata
                }

            };

            //MouseCaptureChanged時の処理
            vScrollBar1.MouseCaptureChanged += (sender, e) =>
            {
                if (MouseCaptureChanged != null)
                {
                    if (this.CanFocus & vScrollBar1.CanFocus & this.ActiveControl == vScrollBar1) this.Focus();  //追加2015/01/24hata
                    MouseCaptureChanged(this, e);
                    if (pnlDummy.CanFocus) pnlDummy.Focus();    //追加2015/01/24hata
                }
            };

        }
        #endregion   
    
        #region プロパティ
        /// <summary>
        /// 値
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("スライダーの値を設定また取得します。")]
        public int Value
        {
            get{return trueValue;} 
            set
            {
                //変更2014/12/15hata
                //if(vScrollBar1.Maximum < value) value = vScrollBar1.Maximum;
                if (maximum < value) value = maximum;

                if(vScrollBar1.Minimum > value) value = vScrollBar1.Minimum;

                trueValue = value;
                if (_Reverse)
                {
                    //変更2014/12/15hata
                    //setValue = vScrollBar1.Maximum - trueValue + vScrollBar1.Minimum;
                    setValue = maximum - trueValue + vScrollBar1.Minimum;
                }
                else
                {
                    setValue = trueValue;
                }

                vScrollBar1.Value = setValue;
            }
        }

        /// <summary>
        /// 最大値
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("最大値を設定また取得します。")]
        public int Maximum
        {
            //変更2014/12/15hata
            //get { return vScrollBar1.Maximum; }
            get { return maximum; }
            set
            {
                //変更2014/12/15hata
                //vScrollBar1.Maximum = value;
                if (value + vScrollBar1.LargeChange - 1 < vScrollBar1.Minimum)
                {
                    value =maximum;
                    return;
                }
                maximum = value;
                vScrollBar1.Maximum = value + vScrollBar1.LargeChange - 1;
                
                ////lblMax.Text = hScrollBar1.Maximum.ToString();
                //if (_Reverse)
                //{
                //    lblTop.Text = Convert.ToString(vScrollBar1.Maximum);
                //}
                //else
                //{
                //    lblBottom.Text = Convert.ToString(vScrollBar1.Maximum);
                //}

                //int iMid = (vScrollBar1.Maximum- vScrollBar1.Minimum)/ 2;
                //lblMid.Text = Convert.ToString(iMid);
                LabelChange();

            }
        }

        /// <summary>
        /// 最小値
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("最小値を設定また取得します。")]
        public int Minimum
        {
            get { return vScrollBar1.Minimum; }
            set
            {
                //追加2014/12/15hata
                if (value > maximum)
                {
                    value = vScrollBar1.Minimum;
                    return;
                }
                vScrollBar1.Minimum = value;
                //if (_Reverse)
                //{
                //    lblBottom.Text = Convert.ToString(vScrollBar1.Minimum);
                // }
                //else
                //{
                //    lblTop.Text = Convert.ToString(vScrollBar1.Minimum);
                //}
                //int iMid = (vScrollBar1.Maximum - vScrollBar1.Minimum) / 2;
                //lblMid.Text = Convert.ToString(iMid);
                LabelChange();

            }
        }

        /// <summary>
        /// スライダの変更方向を逆にする
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("スライダの動作方向を逆にします。")]
        public bool Reverse
        {
            get { return _Reverse; }
            set
            {
                _Reverse = value;
                int val = trueValue;
                Value = val;
                //if (_Reverse)
                //{
                //   lblTop.Text = Convert.ToString(vScrollBar1.Maximum);
                //   lblBottom.Text = Convert.ToString(vScrollBar1.Minimum);
                //}
                //else
                //{
                //    lblTop.Text = Convert.ToString(vScrollBar1.Minimum);
                //    lblBottom.Text = Convert.ToString(vScrollBar1.Maximum);
                //}

                LabelChange();

            }
        }

        /// <summary>
        /// ラージchange
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("スライダーの大動作量を設定します。")]
        public int LargeChange
        {
            get { return vScrollBar1.LargeChange; }
            set { vScrollBar1.LargeChange = value;}
        }

        /// <summary>
        /// 最小change
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("スライダーの小動作量を設定します。")]
        public int SmallChange
        {
            get { return vScrollBar1.SmallChange; }
            set { vScrollBar1.SmallChange = value;}
        }

        /// <summary>
        /// 目盛りラベル表示設定
        /// </summary>
        [Browsable(true)]
        [Category("表示")]
        [Description("目盛りラベルの表示モードを設定します。" + 
                     "TicksLabelOn： ラベルを表示します。" +
                     "TicksLabelOff： ラベルを非表示にします。" +
                     "TicksLabelMidOff： 中央のラベルを非表示にします。")]
        public TicksLabelType TicksLabel
        {
            get { return _TicksLabel; }
            set
            {
                _TicksLabel = value;
                FormResize();
            }
        }

        /// <summary>
        /// 目盛りの表示有無
        /// </summary>
        [Browsable(true)]
        [Category("表示")]
        [Description("目盛りの表示有無を設定します。")]
        public bool TicksLine
        {
            get { return _TicksLine; }
            set
            {
                _TicksLine = value;
                FormResize();

            }
        }


        /// <summary>
        /// 変更不可にする
        /// /// </summary>
        public bool ControlLock
        {
            get { return _ControlLock; }
            set { _ControlLock = value; }
        }

        /// <summary>
        /// フォント設定
        /// </summary>
        public new Font Font
        {
            get { return font; }
            set
            {
                font = value;
                lblTop.Font = font;
                lblBottom.Font = font;
                lblMid.Font = font;
            }
        }

        /// <summary>
        /// コントロールがマウスをキャプチャしたかどうかを示す値を取得または設定します。
        /// </summary>
        public new bool Capture
        {
            get { return vScrollBar1.Capture; }
            set
            {
                vScrollBar1.Capture = value;
                base.Capture = value;
            }
        }
        #endregion


        private void lblTop_Resize(object sender, EventArgs e)
        {
            //int _width;
            //lblTop.Anchor = AnchorStyles.None;
            //lblBottom.Anchor = AnchorStyles.None;
            //vScrollBar1.Anchor = AnchorStyles.None;

            //_width = (lblTop.Width > lblBottom.Width) ? lblTop.Width : lblBottom.Width;
            ////this.Width = _width + vScrollBar1.Width + 2;
            //Application.DoEvents();
            //this.Width = vScrollBar1.Width + _width + 2;
            //lblTop.Left = vScrollBar1.Left + vScrollBar1.Width;
            //lblBottom.Left = vScrollBar1.Left + vScrollBar1.Width;
            //Application.DoEvents();
            
            //vScrollBar1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            //lblTop.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            //lblBottom.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right); ;
            FormResize();


        }

        private void lblTop_AutoSizeChanged(object sender, EventArgs e)
        {
            //int _width;
            //lblTop.Anchor = AnchorStyles.None;
            //lblBottom.Anchor = AnchorStyles.None;
            //vScrollBar1.Anchor = AnchorStyles.None;

            //_width = (lblTop.Width > lblBottom.Width) ? lblTop.Width : lblBottom.Width;
            ////this.Width = _width + vScrollBar1.Width + 2;
            //Application.DoEvents();
            //this.Width = vScrollBar1.Width + _width + 2;
            //lblTop.Left = vScrollBar1.Left + vScrollBar1.Width;
            //lblBottom.Left = vScrollBar1.Left + vScrollBar1.Width;
            //Application.DoEvents();

            //vScrollBar1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            //lblTop.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            //lblBottom.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            FormResize();

        }

        private void FormResize()
        {
            int ioffset = 0;
            int _width = 0;
            int setwidth = 0;
            int lineLen = 5;    //ラインの長さ
            int marginLen = 4;   //ラベルとラインのマージン
             
            lblTop.Anchor = AnchorStyles.None;
            lblMid.Anchor = AnchorStyles.None;
            lblBottom.Anchor = AnchorStyles.None;
            vScrollBar1.Anchor = AnchorStyles.None;

            lineTicksTop.Anchor = AnchorStyles.None;
            lineTicksMid.Anchor = AnchorStyles.None;
            lineTicksBot.Anchor = AnchorStyles.None;


            if (_TicksLine)
            {
                ioffset = lineLen;
                //ioffset = 9;
                //setwidth = this.Width - 2 - ioffset;
                
                lineTicksTop.Visible = true;
                lineTicksMid.Visible = true;
                lineTicksBot.Visible = true;
            }
            else
            {
                ioffset = 0;
                lineTicksTop.Visible = false;
                lineTicksMid.Visible = false;
                lineTicksBot.Visible = false;
            }

            if (_TicksLabel != TicksLabelType.TicksLabelOff)
            {
                ioffset = ioffset + marginLen;
                //ioffset = ioffset + 3;

                _width = lblTop.Width;
                if (_width < lblMid.Width) _width = lblMid.Width;
                if (_width < lblBottom.Width) _width = lblBottom.Width;
                //_width = (lblTop.Width > lblBottom.Width) ? lblTop.Width : lblBottom.Width;
                //setwidth = this.Width - _width - 2 - ioffset;
                
                setwidth = this.Width - _width - ioffset;

                lblTop.Visible = true;
                lblMid.Visible = true;
                lblBottom.Visible = true;
            }
            else
            {
                setwidth = this.Width - ioffset;
                lblTop.Visible = false;
                lblMid.Visible = false;
                lblBottom.Visible = false;
            }

            //if ((_TicksLabel == TicksLabelType.TicksLabelOff) && (!_TicksLine))
            //{
            //    setwidth = this.Width;
            //}

            if (_TicksLabel == TicksLabelType.TicksLabelMidOff)
            {
                lblMid.Visible = false;
                lineTicksMid.Visible = false;
            }
            
            vScrollBar1.Left = 0;
            vScrollBar1.Width = setwidth;

            //Ticksの位置
            lineTicksTop.X1 = vScrollBar1.Width + 1;
            lineTicksTop.X2 = lineTicksTop.X1 + lineLen;
            lineTicksTop.Y1 = vScrollBar1.Top + ButonOffset;
            lineTicksTop.Y2 = lineTicksTop.Y1;

            lineTicksBot.X1 = lineTicksTop.X1;
            lineTicksBot.X2 = lineTicksTop.X2;
            lineTicksBot.Y1 = vScrollBar1.Bottom - ButonOffset;
            lineTicksBot.Y2 = lineTicksBot.Y1;
          
            lineTicksMid.X1 = lineTicksTop.X1;
            lineTicksMid.X2 = lineTicksTop.X2;
            lineTicksMid.Y1 = lineTicksTop.Y1 + Convert.ToInt32((lineTicksBot.Y1 - lineTicksTop.Y1 - ButonOffset) / 2F) + Convert.ToInt32(ButonOffset / 2F);
            lineTicksMid.Y2 = lineTicksMid.Y1;

            //ラベルの位置
            int lblToptop = lineTicksTop.Y1 - Convert.ToInt32(lblTop.Height / 2F);
            int lblBottop = lineTicksBot.Y1 - Convert.ToInt32(lblBottom.Height / 2F);

            lblTop.Top = lblToptop;
            lblBottom.Top = lblBottop;
            lblMid.Top = lineTicksMid.Y1 - Convert.ToInt32(lblMid.Height / 2F);
            lblTop.Left = vScrollBar1.Width + ioffset;
            lblBottom.Left = lblTop.Left;
            lblMid.Left = lblTop.Left;

            vScrollBar1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            lblTop.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            lblBottom.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            lblMid.Anchor = AnchorStyles.Right;

            lineTicksTop.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            lineTicksBot.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            lineTicksMid.Anchor = AnchorStyles.Right;
            
        }

        //目盛りラベルの更新
        private void LabelChange()
        {                

            if (_Reverse)
            {
                //lblTop.Text = Convert.ToString(vScrollBar1.Maximum);
                lblTop.Text = Convert.ToString(maximum);
                lblBottom.Text = Convert.ToString(vScrollBar1.Minimum);
            }
            else
            {
                lblTop.Text = Convert.ToString(vScrollBar1.Minimum);
                //lblBottom.Text = Convert.ToString(vScrollBar1.Maximum);
                lblBottom.Text = Convert.ToString(maximum);
            }

            int iMid = (maximum + vScrollBar1.Minimum) / 2;
            lblMid.Text = Convert.ToString(iMid);
            

        }

        #region オーバーライドされたメソッド
        protected override void OnResize(EventArgs e)
        {
            FormResize();
        }
        #endregion
       
    }
}
