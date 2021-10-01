using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

using System.Collections.Generic;
//using Itc.Common.Extensions;

namespace Itc.Common.Controls
{
    //TODO : Descriptionを追加する
    public abstract partial class RangeSlider : UserControl
    {
        Rectangle _valueArea;

        Toggle Hold = null;

        int lastIndex = 0;

        private Size Offset;

        //bool Captured; //マウスイベントの有効無効設定？

        //bool Reverse; // +/-反転。必要なら追加する

        Color _BarColor;
        protected Brush _BarBrush;

        [field:NonSerialized]
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        protected RangeSlider()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            BarColor = Color.RoyalBlue;
            BackColor = Color.Transparent;

            Toggles = Enumerable.Range(0, 2).Select(i => new Toggle(i) { Converter = DecimalToPoint }).ToList();

            Toggles.ForEach(t => t.PropertyChanged += (s, e) => ValueChanged?.Invoke(this, new ValueChangedEventArgs(t.Value, t.No)));

            Toggles[(int)ToggleType.Low].Value = Minimum;
            Toggles[(int)ToggleType.High].Value = Maximum;
        }

        enum ToggleType { Low = 0, High = 1 }

        /// <summary>
        /// スクロール可能な範囲の最大値を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(100)]
        [Bindable(BindableSupport.Yes)]
        public decimal Maximum { get; set; } = 100;

        /// <summary>
        /// スクロール可能な範囲の最小値を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(0)]
        [Bindable(BindableSupport.Yes)]
        public decimal Minimum { get; set; } = 0;

        /// <summary>
        /// 小さい値を設定または取得します
        /// </summary>
        [Browsable(true)]
        [DefaultValue(0)]
        [Bindable(BindableSupport.Yes)]
        public decimal Low
        {
            get => Toggles[(int)ToggleType.Low].Value;
            set
            {
                if (value != Toggles[(int)ToggleType.Low].Value)
                {
                    Toggles[(int)ToggleType.Low].Value = value;

                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 大きい値を設定または取得します
        /// </summary>
        [Browsable(true)]
        [DefaultValue(100)]
        [Bindable(BindableSupport.Yes)]
        public decimal High
        {
            get => Toggles[(int)ToggleType.High].Value;
            set
            {
                if (value != Toggles[(int)ToggleType.High].Value)
                {
                    Toggles[(int)ToggleType.High].Value = value;

                    Invalidate();
                }
            }
        }

        /// <summary>
        /// High / Low から加減算する値を設定または取得します
        /// </summary>
        [Browsable(true)]
        [DefaultValue(10)]
        public decimal LargeChange { get; set; } = 10;

        /// <summary>
        /// High / Low から加減算する値を設定または取得します
        /// </summary>
        [Browsable(true)]
        [DefaultValue(1)]
        public decimal SmallChange { get; set; } = 1;

        /// <summary>
        /// 小数点の表示位置を設定または取得します
        /// </summary>
        [Browsable(true)]
        [DefaultValue(0)]
        public int DecimalPlaces { get; set; } = 0;

        /// <summary>
        /// Low～High間の色を設定または取得します
        /// </summary>
        [Browsable(true)]
        public Color BarColor
        {
            get => _BarColor;
            set
            {
                if (value != _BarColor)
                {
                    _BarColor = value;

                    _BarBrush = new SolidBrush(value);
                }
            }
        }

        /// <summary>
        /// つまみ
        /// </summary>
        //public Toggle[] Toggles { get; set; }
        public List<Toggle> Toggles { get; }

        ///// <summary>
        ///// リミットマーク（未使用）
        ///// </summary>
        //public LimitMark[] LimitMarks { get; set; }

        /// <summary>
        /// つまみのサイズ
        /// </summary>
        public int ToggleWidth { get; set; } = 12;

        //有効範囲
        protected Rectangle ValueArea { get => _valueArea; set => _valueArea = value; }
        
        private Border3DStyle GetBorder3DStyle(BorderStyle borderStyle)
        {
            switch (borderStyle)
            {
                case BorderStyle.None:
                default:
                    return Border3DStyle.Adjust;
                    
                case BorderStyle.FixedSingle:
                    return Border3DStyle.Flat;

                case BorderStyle.Fixed3D:
                    return Border3DStyle.Sunken;
            }
        }

        bool Clicked => (null != Hold);

        protected abstract Point DecimalToPoint(decimal value);

        protected abstract decimal PointToDecimal(Point point);

        /// <summary>
        /// MouseDown時処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            //つまみが重なっていた場合
            bool isOverlapped = Toggles?.All(t => t.Visible && t.Enabled && t.Rectangle.Contains(e.Location)) ?? false;

            if (isOverlapped)
            {
                if (Toggles[(int)ToggleType.Low].Value == Maximum)
                {
                    Hold = Toggles[(int)ToggleType.Low];
                }
                else if (Toggles[(int)ToggleType.High].Value == Minimum)
                {
                    Hold = Toggles[(int)ToggleType.High];
                }
            }

            if(null == Hold)
            {
                Hold = Toggles?.FirstOrDefault(t => t.Visible && t.Enabled && t.Rectangle.Contains(e.Location));
            }

            //つまみ上でクリックした場合
            if (Clicked)
            {
                //Focus();

                Offset = e.Location.Subtract(Hold.Point);

                lastIndex = Hold.No;
            }
            //それ以外でクリックした場合
            else
            {
                //最後に操作したつまみに、+/-LargeChange
                if (e.Location.X < Toggles[lastIndex].Point.X)
                {
                    Toggles[lastIndex].Value -= LargeChange;
                }
                else
                {
                    Toggles[lastIndex].Value += LargeChange;
                }
            }

            //イベント

            //イベント

            Invalidate();
        }

        /// <summary>
        /// MouseMove時処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (Clicked)
            {
                var p = e.Location;

                p.Offset(-Offset.Width, -Offset.Height);

                if (Hold.No == (int)ToggleType.Low)
                {
                    Hold.Value = Math.Min(PointToDecimal(p), Toggles[(int)ToggleType.High].Value);
                }
                else if (Hold.No == (int)ToggleType.High)
                {
                    Hold.Value = Math.Max(PointToDecimal(p), Toggles[(int)ToggleType.Low].Value);
                }

                ////値に換算
                //Hold.Value = PointToDecimal(p);

                Invalidate();
            }
        }

        /// <summary>
        /// MouseUp時処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (Capture)
            {
                Focus();
            }

            Hold = null;

            Invalidate();
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            Rectangle rect = ValueArea.InflateWith(-1, -1);

            Border3DStyle border3DStyle = GetBorder3DStyle(this.BorderStyle);

            ControlPaint.DrawBorder3D(e.Graphics, rect, border3DStyle, Border3DSide.All);
        }
    }
}
