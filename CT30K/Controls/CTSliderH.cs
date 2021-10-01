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
    public partial class CTSliderH : UserControl
    {
        //EventHandlerの定義
        [Browsable(true)]
        [Category("アクション")]
        [Description("値が変更されたときに発生します。")]
        public event EventHandler ValueChanged;

        public new ScrollEventHandler Scroll;
        public new EventHandler MouseCaptureChanged;    //追加2014/12/15hata
        public new MouseEventHandler MouseUp;           //追加2014/12/15hata

        //内部変数
        private bool _Reverse = false;
        private TicksLabelType _TicksLabel = TicksLabelType.TicksLabelOn;
        private int setValue = 0;
        private int trueValue = 0;
        private int maximum = 0;    //追加2014/12/15hata        
        private Font font;    //追加2014/12/15hata        
        private const int ButonOffset = 16; //追加2014/12/15hata
        private bool _TicksLine = true;

        //追加2014/12/15hata
        //Ticks制御タイプ
        public enum TicksLabelType
        {
            TicksLabelOn,       // ラベルをすべて表示する
            TicksLabelOff,      // ラベルをすべて表示しない
            TicksLabelMidOff    // 中央のラベルを表示しない
        }

        public CTSliderH()
        {
            InitializeComponent();

            // イベント定義
            InitializeEventHandler();

            font = hScrollBar1.Font;    //追加2014/12/15hata
            base.Font = font;           //追加2014/12/15hata
            lblMin.Font = font;         //追加2014/12/15hata
            lblMid.Font = font;         //追加2014/12/15hata
            lblMax.Font = font;         //追加2014/12/15hata
            Maximum = 100;              //追加2014/12/15hata
            Minimum = 0;                //追加2014/12/15hata
            LargeChange = 5;            //追加2014/12/15hata      
            SmallChange = 1;            //追加2014/12/15hata
            trueValue = hScrollBar1.Value;
            setValue = hScrollBar1.Value;
        }

        #region イベント定義
        /// <summary>
        /// イベント定義
        /// </summary>
        private void InitializeEventHandler()
        {
            // スクロールバー値変更時処理
            hScrollBar1.ValueChanged += (sender, e) =>
            {
                setValue = hScrollBar1.Value;
                if (_Reverse)
                {
                    //変更2014/12/15hata
                    //trueValue = hScrollBar1.Maximum - setValue + hScrollBar1.Minimum;
                    trueValue = maximum - setValue + hScrollBar1.Minimum;
                }
                else
                {
                    trueValue = setValue;
                }

                if (ValueChanged != null)
                {
                    if (this.CanFocus & hScrollBar1.CanFocus & this.ActiveControl == hScrollBar1) this.Focus();  //追加2015/01/24hata
                    ValueChanged(this, EventArgs.Empty);
                    if (pnlDummy.CanFocus) pnlDummy.Focus();    //追加2015/01/24hata
                }
                    
                //lblMin.Focus();
                //this.Focus();
            };

            // スクロールバー値変更時処理
            hScrollBar1.Scroll += (sender, e) =>
            {

                setValue = hScrollBar1.Value;
                if (_Reverse)
                {
                    //変更2014/12/15hata
                    //trueValue = hScrollBar1.Maximum - setValue + hScrollBar1.Minimum;
                    trueValue = maximum - setValue + hScrollBar1.Minimum;
                }
                else
                {
                    trueValue = setValue;
                }

                if (Scroll != null)
                {
                    if (this.CanFocus & hScrollBar1.CanFocus & this.ActiveControl == hScrollBar1) this.Focus();  //追加2015/01/24hata
                    Scroll(this, e);
                    if (pnlDummy.CanFocus) pnlDummy.Focus();
                }
                //lblMin.Focus();
                //this.Focus();


            };

            //追加2014/12/15hata
            // MouseUp時の処理
            hScrollBar1.MouseUp += (sender, e) =>
            {
                if (MouseUp != null)
                {
                    if (this.CanFocus & hScrollBar1.CanFocus & this.ActiveControl == hScrollBar1) this.Focus();  //追加2015/01/24hata
                    MouseUp(this, e);
                    if (pnlDummy.CanFocus) pnlDummy.Focus();
                }
                //lblMin.Focus();
                //this.Focus();

            };

            //MouseCaptureChanged時の処理
            hScrollBar1.MouseCaptureChanged += (sender, e) =>
            {
                if (MouseCaptureChanged != null)
                {
                    if (this.CanFocus & hScrollBar1.CanFocus & this.ActiveControl == hScrollBar1) this.Focus();  //追加2015/01/24hata
                    MouseCaptureChanged(this, e);
                    if (pnlDummy.CanFocus) pnlDummy.Focus();
                }
                //lblMin.Focus();
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
            get { return trueValue; }
            set
            {
                //変更2014/12/15hata
                //if (hScrollBar1.Maximum < value) value = hScrollBar1.Maximum;
                if (maximum < value) value = maximum;

                if (hScrollBar1.Minimum > value) value = hScrollBar1.Minimum;

                trueValue = value;
                if (_Reverse)
                {
                    //変更2014/12/15hata
                    //setValue = hScrollBar1.Maximum - trueValue + hScrollBar1.Minimum;
                    setValue = maximum - trueValue + hScrollBar1.Minimum;
                }
                else
                {
                    setValue = trueValue;
                }

                hScrollBar1.Value = setValue;
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
            //get { return hScrollBar1.Maximum; }
            get { return maximum; }
            set
            {
                //変更2014/12/15hata
                //hScrollBar1.Maximum = value;
                if (value + hScrollBar1.LargeChange - 1 < hScrollBar1.Minimum)
                {
                    value = maximum;
                    return;
                }

                maximum = value;
                hScrollBar1.Maximum = value  + hScrollBar1.LargeChange - 1;
                
                //if (_Reverse)
                //{
                //    lblMin.Text = Convert.ToString(hScrollBar1.Maximum);
                //}
                //else
                //{
                //    lblMax.Text = Convert.ToString(hScrollBar1.Maximum);
                //}
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
            get { return hScrollBar1.Minimum; }
            set
            {
                //追加2014/12/15hata
                if (value > maximum)
                {
                    value = hScrollBar1.Minimum;
                    return;
                }
                hScrollBar1.Minimum = value;
                //if (_Reverse)
                //{
                //    lblMax.Text = Convert.ToString(hScrollBar1.Minimum);
                //}
                //else
                //{
                //    lblMin.Text = Convert.ToString(hScrollBar1.Minimum);
                //}
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
                //    lblMin.Text = Convert.ToString(hScrollBar1.Maximum);
                //    lblMax.Text = Convert.ToString(hScrollBar1.Minimum);
                //}
                //else
                //{
                //    lblMin.Text = Convert.ToString(hScrollBar1.Minimum);
                //    lblMax.Text = Convert.ToString(hScrollBar1.Maximum);
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
            get { return hScrollBar1.LargeChange; }
            set { hScrollBar1.LargeChange = value; }
        }

        /// <summary>
        /// 最小change
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("スライダーの小動作量を設定します。")]
        public int SmallChange
        {
            get { return hScrollBar1.SmallChange; }
            set { hScrollBar1.SmallChange = value; }
        }

        /// <summary>
        /// 目盛りラベル表示有無
        /// </summary>
        [Browsable(true)]
        [Category("表示")]
        //[Description("目盛りラベルの表示モードを設定します。TicksLabelOn： ラベルを表示します。TicksLabelOff： ラベルを非表示にします。 TicksLabelMidOff： 中央のラベルを非表示にします。")]
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
        /// 目盛表示有無
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
        /// フォント設定
        /// </summary>
        public new Font Font
        {
            get { return font; }
            set
            {
                font = value;
                lblMax.Font = font;
                lblMin.Font = font;
                lblMid.Font = font;
                FormResize();
            }
        }

        /// <summary>
        /// コントロールがマウスをキャプチャしたかどうかを示す値を取得または設定します。
        /// </summary>
        public new bool Capture
        {
            get { return hScrollBar1.Capture; }
            set { 
                    hScrollBar1.Capture =value;
                    base.Capture = value;
                }
        }

        #endregion

        private void lblTicks_AutoSizeChanged(object sender, EventArgs e)
        {
            FormResize();
        }

        //追加2014/12/15hata
        private void lblTicks_Resize(object sender, EventArgs e)
        {
            FormResize();
        }


        //フォームサイズの更新
        private void FormResize()
        {

            int ioffset = 0;
            int setwidth = 0;
            int lineLen = 5;    //ラインの長さ
            int marginLen = 4;   //ラベルとラインのマージン

            //アンカーを切る
            lblMin.Anchor = AnchorStyles.None;
            lblMid.Anchor = AnchorStyles.None;
            lblMax.Anchor = AnchorStyles.None;
            lineTicksMin.Anchor = AnchorStyles.None;
            lineTicksMid.Anchor = AnchorStyles.None;
            lineTicksMax.Anchor = AnchorStyles.None;
            hScrollBar1.Anchor = AnchorStyles.None;

            if (_TicksLine)
            {
                ioffset = lineLen;
                //hScrollBar1.Height = this.Height - lblMin.Height - ioffset;
                lineTicksMin.Visible = true;
                lineTicksMid.Visible = true;
                lineTicksMax.Visible = true;
            }
            else
            {
                ioffset = 0;
                lineTicksMin.Visible = false;
                lineTicksMid.Visible = false;
                lineTicksMax.Visible = false;
            }

            if (_TicksLabel != TicksLabelType.TicksLabelOff)
            {
                ioffset = ioffset + marginLen;
                setwidth = this.Height - lblMin.Height - ioffset;
                lblMin.Visible = true;
                lblMax.Visible = true;
                lblMid.Visible = true;
            }
            else
            {
                setwidth = this.Height - ioffset;
                lblMin.Visible = false;
                lblMax.Visible = false;
                lblMid.Visible = false;
            }
            if (_TicksLabel == TicksLabelType.TicksLabelMidOff)
            {
                lblMid.Visible = false;
                lineTicksMid.Visible = false;
            }

            hScrollBar1.Top = 0;
            hScrollBar1.Height = setwidth;

            //Ticksの位置
            lineTicksMin.X1 = hScrollBar1.Left + ButonOffset;
            lineTicksMin.X2 = lineTicksMin.X1;
            lineTicksMin.Y1 = hScrollBar1.Bottom + 1;
            lineTicksMin.Y2 = lineTicksMin.Y1 + lineLen;          

            lineTicksMax.X1 = hScrollBar1.Right - ButonOffset;
            lineTicksMax.X2 = lineTicksMax.X1;
            lineTicksMax.Y1 = lineTicksMin.Y1;
            lineTicksMax.Y2 = lineTicksMin.Y2;

            lineTicksMid.X1 = lineTicksMin.X1 + Convert.ToInt32((lineTicksMax.X1 - lineTicksMin.X1 - ButonOffset) / 2F) + Convert.ToInt32(ButonOffset / 2F);
            lineTicksMid.X2 = lineTicksMid.X1;
            lineTicksMid.Y1 = lineTicksMin.Y1;
            lineTicksMid.Y2 = lineTicksMin.Y2;

            //ラベルの位置
            int lblMinleft = hScrollBar1.Left + ButonOffset - Convert.ToInt32(lblMin.Width / 2F);
            int lblMaxleft = hScrollBar1.Right - ButonOffset - Convert.ToInt32(lblMax.Width / 2F);
            
            if (lblMinleft < 0) lblMinleft = 0;
            if ((lblMaxleft + lblMax.Width) > this.Width) lblMaxleft = this.Width - lblMax.Width;
            lblMin.Left = lblMinleft;
            lblMax.Left = lblMaxleft;
            lblMid.Left = lineTicksMid.X1 - Convert.ToInt32(lblMid.Width /2F);

            lblMin.Top = hScrollBar1.Height + ioffset;
            lblMax.Top = hScrollBar1.Height + ioffset;
            lblMid.Top = hScrollBar1.Height + ioffset;

            //アンカーを戻す
            hScrollBar1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            lblMin.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            lblMax.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            lblMid.Anchor = (AnchorStyles.Bottom);

            lineTicksMin.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            lineTicksMax.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            lineTicksMid.Anchor = (AnchorStyles.Bottom);
        }


        //目盛りラベルの更新
        private void LabelChange()
        {
            if (_Reverse)
            {
                //lblMin.Text = Convert.ToString(hScrollBar1.Maximum);
                lblMin.Text = Convert.ToString(maximum);
                lblMax.Text = Convert.ToString(hScrollBar1.Minimum);
            }
            else
            {
                lblMin.Text = Convert.ToString(hScrollBar1.Minimum);
                //lblMax.Text = Convert.ToString(hScrollBar1.Maximum);
                lblMax.Text = Convert.ToString(maximum);
            }
            lblMid.Text = Convert.ToString((maximum + hScrollBar1.Minimum) / 2);

        }

        #region オーバーライドされたメソッド
        protected override void OnResize(EventArgs e)
        {
            FormResize();
        }
        #endregion

    }
}
