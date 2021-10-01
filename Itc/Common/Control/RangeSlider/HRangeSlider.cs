//using Itc.Common.Extensions;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Itc.Common.Controls
{
    //水平方向
    [Serializable]
    public class HRangeSlider : RangeSlider
    {
        public HRangeSlider() : base()
        {
            Size = new Size(200, 20);

            ValueArea = this.ClientRectangle.InflateWith(-ToggleWidth, 0);

            Toggles[0].Direction = Direction.LeftToRight;
            Toggles[1].Direction = Direction.LeftToRight;
        }

        protected override Point DecimalToPoint(decimal value)
        {
            float rate = ValueArea.Width / (float)Math.Abs(Maximum - Minimum);

            var p = (float)value * rate + ValueArea.Left;

            return new Point((int)p, center);
        }

        protected override decimal PointToDecimal(Point point)
        {
            var value = (point.X - (int)ValueArea.Left) * (float)Math.Abs(Maximum - Minimum) / ValueArea.Width;

            return Math.Min(Maximum, Math.Max(Minimum, (decimal)value));
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var c = (Bottom - Top) / 2;
            int offset = 5;

            //Low～High間を塗りつぶし
            e.Graphics.FillRectangle(_BarBrush,
                RectangleF.FromLTRB(
                    Toggles[0].Point.X, 
                    c - offset, 
                    Toggles[1].Point.X, 
                    c + offset));

            //つまみ描画
            Toggles?.ForEach(t => t?.Draw(e.Graphics));
        }

        int center => (ValueArea.Top + ValueArea.Bottom) / 2;

        /// <summary>
        /// リサイズ処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            //つまみのサイズ調整
            Toggles?.ForEach(t => t.Size = new Size(ToggleWidth, Height));

            ValueArea = this.ClientRectangle.InflateWith(-ToggleWidth, 0);

            //（リミットマークの調整）

            Invalidate();
        }
    }
}
