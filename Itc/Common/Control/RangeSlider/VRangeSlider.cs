//using Itc.Common.Extensions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Itc.Common.Controls
{
    //垂直方向
    [Serializable]
    public class VRangeSlider : RangeSlider
    {
        public VRangeSlider() : base()
        {
            Size = new Size(20, 200);

            ValueArea = this.ClientRectangle.InflateWith(0, -ToggleWidth);

            Toggles[0].Direction = Direction.TopDown;
            Toggles[1].Direction = Direction.TopDown;
        }

        protected override Point DecimalToPoint(decimal value)
        {
            float rate = ValueArea.Height / (float)Math.Abs(Maximum - Minimum);

            var p = (float)value * rate + ValueArea.Top;

            return new Point(center, (int)p);
        }

        protected override decimal PointToDecimal(Point point)
        {
            var value = (point.Y - (int)ValueArea.Top) * (float)Math.Abs(Maximum - Minimum) / ValueArea.Height;

            return Math.Min(Maximum, Math.Max(Minimum, (decimal)value));
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var c = (Right - Left) / 2;
            int offset = 5;
                        
                //Low～High間を塗りつぶし
                e.Graphics.FillRectangle(_BarBrush, 
                    RectangleF.FromLTRB(
                        c - offset,
                        Toggles[0].Point.Y,
                        c + offset,
                        Toggles[1].Point.Y));

            //つまみ描画
            Toggles?.ForEach(t => t?.Draw(e.Graphics));
        }

        int center => (ValueArea.Left + ValueArea.Right) / 2;

        /// <summary>
        /// リサイズ処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            //つまみのサイズ調整
            Toggles?.ForEach(t => t.Size = new Size(Width, ToggleWidth));

            ValueArea = this.ClientRectangle.InflateWith(0, -ToggleWidth);

            //（リミットマークの調整）

            Invalidate();
        }
    }
}
