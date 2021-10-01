//using Itc.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using System.Xml.Serialization;

namespace Itc.Common.Controls
{
    /// <summary>
    /// つまみの形状
    /// </summary>
    public enum ToggleStyle
    {
        /// <summary>
        /// 四角形
        /// </summary>
        Normal,

        /// <summary>
        /// 三角形
        /// </summary>
        Arrow,
    }

    /// <summary>
    /// つまみの向き
    /// </summary>
    public enum Direction
    {
        LeftToRight,

        RightToLeft,

        BottomUp,

        TopDown
    }

    /// <summary>
    /// つまみの情報
    /// </summary>
    [Serializable()]
    public class Toggle : INotifyPropertyChanged
    {
        #region - BackingField - 

        decimal _Value;

        bool _Visible;

        bool _Enabled;

        Color _Color;

        ToggleStyle _Style;

        Direction _Direction;

        Size _Size;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        private void SetProperty<T>(ref T value, T newvalue, [System.Runtime.CompilerServices.CallerMemberName]string name = "")
        {
            value = newvalue;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;


        #endregion

        public int No { get; }

        /// <summary>
        /// スライダーの位置
        /// </summary>
        public decimal Value { get => _Value; set => SetProperty(ref _Value, value); }

        /// <summary>
        /// 表示・非表示設定
        /// </summary>
        public bool Visible { get => _Visible; set => SetProperty(ref _Visible, value); }

        /// <summary>
        /// 有効・無効設定
        /// </summary>
        public bool Enabled { get => _Enabled; set => SetProperty(ref _Enabled, value); }

        /// <summary>
        /// 色を設定または取得します
        /// </summary>
        public Color Color { get => _Color; set => SetProperty(ref _Color, value); }

        /// <summary>
        /// 形状
        /// </summary>
        public ToggleStyle Style { get => _Style; set => SetProperty(ref _Style, value); }

        /// <summary>
        /// 向き
        /// </summary>
        public Direction Direction { get => _Direction; set => SetProperty(ref _Direction, value); }

        /// <summary>
        /// 位置  Value -> Point変換
        /// </summary>
        //public Point Point => Converter?.Invoke(Value) ?? Point.Empty;
        public Point Point => Converter?.Invoke(Value) ?? Point.Empty;
        //public Point Point => Point.Empty;

        //public Func<decimal, Point> Converter { get; set; }
        [XmlIgnore]
        [field: NonSerialized()]
        public Converter<decimal, Point> Converter { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        public Size Size { get => _Size; set => SetProperty(ref _Size, value); }

        /// <summary>
        /// 領域
        /// </summary>
        //public Rectangle Rectangle => new Rectangle(Point, Size);
        public Rectangle Rectangle => new Rectangle(Point, Size.Empty).InflateWith(Size.Width / 2, Size.Height / 2); //中心基準

        //Mode 操作制御？

        const int defaultSize = 12;

        //internal ButtonState ButtonState { get; set; }

        public Toggle() : this(0)
        {

        }

        /// <summary>
        /// 新しいインスタンスを生成します
        /// </summary>
        public Toggle(int no)
        {
            No = no;

            Value = 0;

            Visible = true;

            Enabled = true;

            Color = SystemColors.Control;

            Style = ToggleStyle.Normal;

            Direction = Direction.LeftToRight;

            Size = new Size(defaultSize, defaultSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        internal void Draw(Graphics g, Rectangle rectangle)
        {
            if (!Visible) return;

            using (var brush = new SolidBrush(Color))
            {
                if (Style == ToggleStyle.Arrow)
                {
                    var points = new Point[3];

                    g.FillPolygon(brush, points);

                    //縁取り
                    g.DrawPolygon(new Pen(Color.Gray), points);
                }
                else if (Style == ToggleStyle.Normal)
                {
                    DrawToggleImage(g, rectangle, brush, Direction);
                }
            }
        }

        private void DrawToggleImage(Graphics g, Rectangle rectangle, SolidBrush brush, Direction direction)
        {
            //全体描画：
            ControlPaint.DrawButton(g, rectangle, ButtonState.Normal);

            var r = new Rectangle(rectangle.Location, new Size(rectangle.Width - 1, rectangle.Height - 1));

            g.FillRectangle(brush, r);

            const int gap = 3;

            var rect = _GetRect(rectangle, direction, gap);

            ControlPaint.DrawButton(g, rect, ButtonState.Normal);
        }

        private static Rectangle _GetRect(Rectangle rect, Direction direction, int gap)
        {
            if (direction == Direction.BottomUp || direction == Direction.TopDown)
            {
                rect.Y += rect.Height / 2 - gap;

                rect.Height = gap;
            }
            else
            {
                rect.X += rect.Width / 2 - gap;

                rect.Width = gap;
            }

            return rect;
        }

        internal void Draw(Graphics g) => Draw(g, Rectangle);

        internal void Draw(Graphics g, Point point) => Draw(g, new Rectangle(point, Size));

        //ポインター情報
        private IEnumerable<Point> CreateArrow(Point point, int size)
        {
            yield return new Point(0, 0);
            yield return new Point(0, 0);
            yield return new Point(0, 0);
        }
    }



    //未使用
    public class LimitMark
    {
        #region - BackingField - 

        protected Color _Color;

        #endregion

        public decimal Value { get; set; }

        public Color Color
        {
            get => _Color;
            set
            {
                _Color = value;

                Brush = new SolidBrush(value);
            }
        }

        private Brush Brush { get; set; }

        public bool Visible { get; set; }

        public Rectangle Rectangle { get; set; }

        public void Draw(Graphics g)
        {
            if (!Visible) return;

            //Value -> pos -> Rectangle

            g.FillRectangle(Brush, Rectangle);
        }
    }

}
