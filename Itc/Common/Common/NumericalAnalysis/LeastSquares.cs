using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itc.Common.NumericalAnalysis
{
    public class LeastSquares
    {
        /// <summary>
        /// 距離を求める
        /// </summary>
        /// <param name="Pointfs"></param>
        /// <param name="Bias"></param>
        /// <param name="Slope"></param>
        public static float CalMaxDistance(IEnumerable<PointF> Pointfs)
        {
           
            float max_x = Pointfs.Max(p => p.X);
            float min_x = Pointfs.Min(p => p.X);


            float max_y = Pointfs.Max(p => p.Y);
            float min_y = Pointfs.Min(p => p.Y);


            return (float)Math.Sqrt(Math.Pow(max_x - min_x, 2) + Math.Pow(max_y - min_y, 2));
        }
        /// <summary>
        /// 二次方程式　y=Ax+Bを求める
        /// </summary>
        /// <param name="Pointfs"></param>
        /// <param name="Bias"></param>
        /// <param name="Slope"></param>
        public static void QuadraticEquation(IEnumerable<PointF> Pointfs, out float Bias, out float Slope, out float Angle, out bool IsSuccess)
        {
            Bias = 0F;
            Angle = 0F;
            Slope = 0F;
            IsSuccess = false;

            var querXerror = Pointfs.ToList().FindAll(p => p.X == Pointfs.FirstOrDefault().X);
            var querYerror = Pointfs.ToList().FindAll(p => p.Y == Pointfs.FirstOrDefault().Y);

            if (querXerror.Count() == Pointfs.Count())
            {
                Angle = 90;
                Bias = 0F;
                Slope = 0F;
                IsSuccess = true;
                return;
            }

            if (querYerror.Count() == Pointfs.Count())
            {
                Angle = 0;
                Bias = 0F;
                Slope = 0F;
                IsSuccess = true;
                return;
            }

            const int Di = 2;//2次方程式なので

            double[,] rc = new double[Di, Di];

            double[] Xr = new double[Di];

            foreach (int i in Enumerable.Range(0, Di))
            {
                foreach (int j in Enumerable.Range(0, Di))
                {
                    double tmp = 0F;
                    foreach (var pair in Pointfs)
                    {
                        tmp = tmp + Math.Pow(pair.X, i + j);
                    }
                    rc[i, j] = tmp;
                }
            }

            foreach (int i in Enumerable.Range(0, Di))
            {
                double tmp = 0F;
                foreach (var pair in Pointfs)
                {
                    tmp = tmp + pair.Y * Math.Pow(pair.X, i);
                }
                Xr[i] = tmp;
            }
            double[,] inverse = InvertMatrix(rc);
            double[] Xout = new double[Di];
            //ガウスの消去法
            if (inverse != null)
            {
                Xout[0] = inverse[0, 0] * Xr[0] + inverse[1, 0] * Xr[1];
                Xout[1] = inverse[0, 1] * Xr[0] + inverse[1, 1] * Xr[1];

                //エラー処理
                Bias = (float)Xout[0];
                Slope = (float)Xout[1];
                Angle = (float)(Math.Atan(Xout[1]) * 180F / Math.PI);
                IsSuccess = true;
                return;
            }
            else
            {
                IsSuccess = false;

            }
            return;
        }
        /// <summary>
        /// 逆行列を求めます
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>Return the matrix's inverse or null if it has none</returns>
        public static double[,] InvertMatrix(double[,] matrix)
        {
            //const double tiny = 0.00001;
            const double tiny = 0.001;
            // Build the augmented matrix.
            int num_rows = matrix.GetUpperBound(0) + 1;
            double[,] augmented = new double[num_rows, 2 * num_rows];
            for (int row = 0; row < num_rows; row++)
            {
                for (int col = 0; col < num_rows; col++)
                    augmented[row, col] = matrix[row, col];
                augmented[row, row + num_rows] = 1;
            }

            // num_cols is the number of the augmented matrix.
            int num_cols = 2 * num_rows;

            // Solve.
            for (int row = 0; row < num_rows; row++)
            {
                // Zero out all entries in column r after this row.
                // See if this row has a non-zero entry in column r.
                if (Math.Abs(augmented[row, row]) < tiny)
                {
                    // Too close to zero. Try to swap with a later row.
                    for (int r2 = row + 1; r2 < num_rows; r2++)
                    {
                        if (Math.Abs(augmented[r2, row]) > tiny)
                        {
                            // This row will work. Swap them.
                            for (int c = 0; c < num_cols; c++)
                            {
                                double tmp = augmented[row, c];
                                augmented[row, c] = augmented[r2, c];
                                augmented[r2, c] = tmp;
                            }
                            break;
                        }
                    }
                }

                // If this row has a non-zero entry in column r, use it.
                if (Math.Abs(augmented[row, row]) > tiny)
                {
                    // Divide the row by augmented[row, row] to make this entry 1.
                    for (int col = 0; col < num_cols; col++)
                        if (col != row)
                            augmented[row, col] /= augmented[row, row];
                    augmented[row, row] = 1;

                    // Subtract this row from the other rows.
                    for (int row2 = 0; row2 < num_rows; row2++)
                    {
                        if (row2 != row)
                        {
                            double factor = augmented[row2, row] / augmented[row, row];
                            for (int col = 0; col < num_cols; col++)
                                augmented[row2, col] -= factor * augmented[row, col];
                        }
                    }
                }
            }

            // See if we have a solution.
            if (augmented[num_rows - 1, num_rows - 1] == 0) return null;

            // Extract the inverse array.
            double[,] inverse = new double[num_rows, num_rows];
            for (int row = 0; row < num_rows; row++)
            {
                for (int col = 0; col < num_rows; col++)
                {
                    inverse[row, col] = augmented[row, col + num_rows];
                }
            }

            return inverse;
        }
    }
}
