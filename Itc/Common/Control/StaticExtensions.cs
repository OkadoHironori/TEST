using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Itc.Common.Controls
{
    //※ Itc.Commonを参照するとデザイナーで使用できなかったためとりあえずコピー
    internal static class DrawingExtensions
    {
        /// <summary>
        /// ２点間の差分をサイズで取得
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static Size Subtract(this Point pt1, Point pt2)
        {
            return new Size(pt1.X - pt2.X, pt1.Y - pt2.Y);
        }

        /// <summary>
        /// ２点間の差分をサイズで取得
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static SizeF Subtract(this PointF pt1, PointF pt2)
        {
            return new SizeF(pt1.X - pt2.X, pt1.Y - pt2.Y);
        }

        /// <summary>
        /// ２点間の距離取得
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static double Distance(PointF pt1, PointF pt2)
        {
            var sz = pt1.Subtract(pt2);

            return Math.Sqrt(sz.Width * sz.Width + sz.Height * sz.Height);
        }

        /// <summary>
        /// ２点間の中点を取得
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static PointF Between(PointF pt1, PointF pt2)
        {
            return new PointF((pt1.X + pt2.X) / 2.0f, (pt1.Y + pt2.Y) / 2.0f);
        }

        /// <summary>
        /// Rectの中心位置取得
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static PointF Center(this RectangleF rect)
        {
            return Between(rect.Location, new PointF(rect.Right, rect.Bottom));
        }

        /// <summary>
        /// ポイント中心の矩形を取得
        /// </summary>
        /// <param name="center"></param>
        /// <param name="sz"></param>
        /// <returns></returns>
        public static RectangleF RectangleWithCenter(this PointF center, SizeF sz)
        {
            var r = new RectangleF(center, sz);
            r.Offset(-sz.Width / 2.0f, -sz.Height / 2.0f);
            return r;
        }

        /// <summary>
        /// 指定量拡大させたRectangleを取得します
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Rectangle InflateWith(this Rectangle rectangle, Size size)
         => rectangle.InflateWith(size.Width, size.Height);

        /// <summary>
        /// 指定量拡大させたRectangleを取得します
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Rectangle InflateWith(this Rectangle rectangle, int width, int height)
        {
            var r = rectangle;

            r.Inflate(width, height);

            return r;
        }

        /// <summary>
        /// 指定量拡大させたRectangleFを取得します
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static RectangleF InflateWith(this RectangleF rectangle, SizeF size)
         => rectangle.InflateWith(size.Width, size.Height);

        /// <summary>
        /// 指定量拡大させたRectangleFを取得します
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static RectangleF InflateWith(this RectangleF rectangle, float width, float height)
        {
            var r = rectangle;

            r.Inflate(width, height);

            return r;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Rectangle OffsetWith(this Rectangle rectangle, int x, int y)
        {
            var r = rectangle;

            r.Offset(x, y);

            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static RectangleF OffsetWith(this RectangleF rectangle, float x, float y)
        {
            var r = rectangle;

            r.Offset(x, y);

            return r;
        }
    }
}
