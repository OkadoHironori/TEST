using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Drawing2D;

//拡張メソッドを纏める
namespace Itc.Common.Extensions
{
    /// <summary>
    /// Matrixの拡張メソッド
    /// </summary>
    public static class MatrixExtensions
    {
        /// <summary>
        /// 座標変換処理
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static PointF CoordinateTransform(this Matrix mat, PointF pt)
        {
            if (null == mat)
            {
                return pt;
            }

            //座標変換処理
            var path = new GraphicsPath();

            path.AddLines(new PointF[] { pt });

            //path.AddRectangle(new RectangleF(pt, new SizeF(1, 1)));

            path.Transform(mat);

            return path.PathPoints.First();
        }

        /// <summary>
        /// 座標変換処理
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="sz"></param>
        /// <returns></returns>
        public static SizeF CoordinateTransform(this Matrix mat, SizeF sz)
        {
            var pt = new PointF(sz.Width, sz.Height);

            PointF pt2 = mat.CoordinateTransform(pt);

            return new SizeF(pt2);
        }

        /// <summary>
        /// 逆変換処理
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static PointF InverseTransform(this Matrix mat, PointF pt)
        {
            var invMat = mat.Clone() as Matrix;

            invMat.Invert();

            return invMat.CoordinateTransform(pt);
        }

        /// <summary>
        /// 逆変換処理
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="sz"></param>
        /// <returns></returns>
        public static SizeF InverseTransform(this Matrix mat, SizeF sz)
        {
            var pt = new PointF(sz.Width, sz.Height);

            PointF pt2 = mat.InverseTransform(pt);

            return new SizeF(pt2);
        }
    }
}
