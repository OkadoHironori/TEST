using System;
using System.Collections.Generic;
using System.Linq;

namespace Itc.Common.Utility
{
    public static class Gamma
    {
        static double Convert(double invGamma, int value, int start, int end, int min, int max)
            => _CalcGamma(invGamma, value, start, end, min, max);

        private static double _CalcGamma(double invGamma, int value, int start, int end, int min, int max)
        {
            return Math.Pow((double)(value - start) / (end - start), invGamma) * (max - min) + min;
        }

        /// <summary>
        /// ガンマ変換
        /// </summary>
        /// <param name="gamma"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static IEnumerable<double> Convert(double gamma, int start, int end, int min, int max)
        {
            double g = 1 / gamma;

            return Enumerable.Range(start, end - start)
                .Select(v => _CalcGamma(g, v, start, end, min, max));
        }
    }
}
