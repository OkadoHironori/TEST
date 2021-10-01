using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itc.Common.NumericalAnalysis
{
    public class ItcStatistics
    {
        /// <summary>
        /// 標準偏差
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="StdDev"></param>
        public static void CalStandardDeviation(IEnumerable<float> datas,out float Average, out float StdDev)
        {
            StdDev = 0F;
            Average = 0F;
            float avg = 0F;
            float std = 0F;
            int n = 0;
            float sum = 0F;
            float sum2 = 0F;

            foreach(float v in datas)
            {
                sum += v;
                sum2 += v * v;
                ++n;
            }

            if (n > 0)
            {
                // n == 0は基本的にありえない
                // (トレースROIでありえる。その場合、平均と標準偏差は0.0)
                avg = sum / (float)n;
                std = (float)Math.Sqrt((sum2 / (float)n) - (avg * avg));
                StdDev = std;
                Average = avg;
            }
        }
        /// <summary>
        /// 大津二値化 
        /// </summary>
        /// <param name="anarect">ROI</param>
        /// <param name="filterdata">データ</param>
        /// <param name="width">データ幅</param>
        /// <returns></returns>
        public static int DoOtuThreshold(Rectangle anarect, short[] filterdata, int width,int height, bool ZeroOrMore = false)
        {
            double w0 = 0, w1 = 0;
            double m0 = 0, m1 = 0;
            double max_sb = 0, sb = 0;
            int th = 0;

            List<int> vdata = new List<int>();

            for (int y = anarect.Top; y < anarect.Bottom; y++)
            {
                for (int x = anarect.Left; x < anarect.Right; x++)
                {
                    if (filterdata[y * width + x] != -8192|| filterdata[y * width + x] != -8191)
                    {
                        if(ZeroOrMore)
                        {
                            if(filterdata[y * width + x] > 0)
                            {
                                vdata.Add(filterdata[y * width + x]);
                            }
                        }
                        else
                        {
                            vdata.Add(filterdata[y * width + x]);
                        }
                    }
                }
            }

            int mindata = 0;
            int maxdata = 0;

            if (vdata.Count() != 0)
            {
                mindata = vdata.Min();
                maxdata = vdata.Max();
            }

            foreach (var t in Enumerable.Range(mindata, maxdata - mindata))
            {
                w0 = 0;
                w1 = 0;
                m0 = 0;
                m1 = 0;

                foreach (var data in vdata)
                {
                    int val = (int)data;

                    if (val < t)
                    {
                        w0++;
                        m0 += val;
                    }
                    else
                    {
                        w1++;
                        m1 += val;
                    }
                }

                m0 /= w0;
                m1 /= w1;
                w0 /= (vdata.Count());
                w1 /= (vdata.Count());

                sb = w0 * w1 * Math.Pow((m0 - m1), 2);

                if (sb > max_sb)
                {
                    max_sb = sb;
                    th = t;
                }
            }

            return th;
        }
        /// <summary>
        /// 大津二値化 
        /// </summary>
        /// <param name="anarect">ROI</param>
        /// <param name="filterdata">データ</param>
        /// <param name="width">データ幅</param>
        /// <returns></returns>
        public static int DoOtuThreshold(IEnumerable<double> vdata)
        {


            //List<int> vdata = new List<int>();

            //foreach(var ddd in tdata)
            //{
            //    vdata.Add(filterdata[y * width + x]);
            //}

            //for (int y = anarect.Top; y < anarect.Bottom; y++)
            //{
            //    for (int x = anarect.Left; x < anarect.Right; x++)
            //    {
            //        if (filterdata[y * width + x] > 1)
            //        {
            //            vdata.Add(filterdata[y * width + x]);
            //        }
            //    }
            //}
            int mindata = (int)0;
            int maxdata = (int)vdata.Count();
            int mindata_ = (int)vdata.Min();
            //int maxdata = (int)vdata.Max();
            int th = 0;
            double max_sb = 0;
            foreach (var t in Enumerable.Range(mindata, maxdata - mindata))
            {
                double w2 = 0, w1 = 0;
                double m2 = 0, m1 = 0;
                double sum2 = 0, sum1 = 0;
                double ave2 = 0, ave1 = 0;
                double sig2 = 0, sig1 = 0;
                double sb = 0;
               

                foreach (var data in vdata.Select((v, i) => new { v, i }))
                {
                    //var val =Math.Exp(data.v);
                    var val = data.v;
                    if (data.i < t)
                    {
                        w1++;
                        m1 += val;
                        sum1 += val * val;

                    }
                    else
                    {
                        w2++;
                        m2 += val;
                        sum2 += val * val;
                    }
                }

                ave1 = m1 / w1;
                sig1 = (float)Math.Sqrt((sum1 / w1) - (ave1 * ave1));

                ave2 = m2 / w2;
                sig2 = (float)Math.Sqrt((sum2 / w2) - (ave2 * ave2));
                
                //var tmp0 = m0 / w0;

                //m1 /= w1;
                //w0 /= (vdata.Count());
                //w1 /= (vdata.Count());

                sb = w1 * w2 * Math.Pow((ave1 - ave2), 2);

                Debug.WriteLine($"{sb}");

                if (sb > max_sb)
                {
                    max_sb = sb;
                    th = t;
                }
            }

            return th;
        }
        /// <summary>
        /// 標準偏差を求める
        /// </summary>
        /// <param name="anarect">ROI</param>
        /// <param name="filterdata">データ</param>
        /// <param name="width">データ幅</param>
        /// <returns></returns>
        public static float CalStd(Rectangle anarect, short[] filterdata, int width, int height, bool ZeroOrMore = false)
        {
            List<int> vdata = new List<int>();

            for (int y = anarect.Top; y < anarect.Bottom; y++)
            {
                for (int x = anarect.Left; x < anarect.Right; x++)
                {
                    if (filterdata[y * width + x] != -8191|| filterdata[y * width + x] != -8192)
                    {
                        if (ZeroOrMore)
                        {
                            if (filterdata[y * width + x] > 0)
                            {
                                vdata.Add(filterdata[y * width + x]);
                            }
                        }
                        else
                        {
                            vdata.Add(filterdata[y * width + x]);
                        }
                    }
                }
            }
            double sum = 0.0;
            double sum2 = 0.0;

            foreach(var data in vdata)
            {
                sum += data;
                sum2 += data * data;
            }

            double CTavg = sum / vdata.Count();
            double CTstd = Math.Sqrt((sum2 / vdata.Count()) - (CTavg * CTavg));

            return (float)CTstd;
        }

        /// <summary>
        /// 標準偏差を求める
        /// </summary>
        /// <param name="anarect">ROI</param>
        /// <param name="filterdata">データ</param>
        /// <param name="width">データ幅</param>
        /// <returns></returns>
        public static float CalAve(Rectangle anarect, short[] filterdata, int width, int height, bool ZeroOrMore = false)
        {
            List<int> vdata = new List<int>();

            for (int y = anarect.Top; y < anarect.Bottom; y++)
            {
                for (int x = anarect.Left; x < anarect.Right; x++)
                {
                    if (filterdata[y * width + x] != -8191 || filterdata[y * width + x] != -8192)
                    {
                        if (ZeroOrMore)
                        {
                            if (filterdata[y * width + x] > 0)
                            {
                                vdata.Add(filterdata[y * width + x]);
                            }
                        }
                        else
                        {
                            vdata.Add(filterdata[y * width + x]);
                        }
                    }
                }
            }
            double sum = 0.0;
            double sum2 = 0.0;

            foreach (var data in vdata)
            {
                sum += data;
                sum2 += data * data;
            }

            double CTavg = sum / vdata.Count();
            double CTstd = Math.Sqrt((sum2 / vdata.Count()) - (CTavg * CTavg));

            return (float)CTavg;
        }

        /// <summary>
        /// 標準偏差を求める
        /// </summary>
        /// <param name="anarect">ROI</param>
        /// <param name="filterdata">データ</param>
        /// <param name="width">データ幅</param>
        /// <returns></returns>
        public static float CalStd(Rectangle anarect, ushort[] filterdata, int width, int height, bool ZeroOrMore = false)
        {
            List<int> vdata = new List<int>();

            for (int y = anarect.Top; y < anarect.Bottom; y++)
            {
                for (int x = anarect.Left; x < anarect.Right; x++)
                {
                    if (filterdata[y * width + x] > 1)
                    {
                        vdata.Add(filterdata[y * width + x]);
                    }
                }
            }
            double sum = 0.0;
            double sum2 = 0.0;

            foreach (var data in vdata)
            {
                sum += data;
                sum2 += data * data;
            }

            double CTavg = sum / vdata.Count();
            double CTstd = Math.Sqrt((sum2 / vdata.Count()) - (CTavg * CTavg));

            return (float)CTstd;
        }
        /// <summary>
        /// しきい値より大きい部分を1(byte)とする
        /// </summary>
        /// <param name="thre"></param>
        /// <param name="area"></param>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static byte[] MakeDateDevidByThreshold(double thre, Rectangle area, short[] data, int width, int height)
        {

            byte[] thredata = new byte[width * height];

            if (data == null) { return thredata; }

            for (int y = area.Top; y < area.Bottom; y++)
            {
                for (int x = area.Left; x < area.Right; x++)
                {
                    if (data[y * width + x] > thre)
                    {
                        thredata[y * width + x] = (byte)1;
                    }
                }
            }

            return thredata;
        }
        /// <summary>
        /// ラベリングデータの番号をリスト化
        /// </summary>
        /// <param name="data">ラベリングデータ</param>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        /// <returns></returns>
        public static IEnumerable<int> GetLabelList(ushort[] data, int width, int height)
        {
            List<int> labellist = new List<int>();

            foreach (int xx in Enumerable.Range(0, width))
            {
                foreach (var yy in Enumerable.Range(0, height))
                {
                    var tdata = (int)data[xx + yy * width];

                    if (tdata > 0 && !labellist.Contains(tdata))
                    {
                        labellist.Add(tdata);
                    }
                }
            }

            return labellist;
        }
        /// <summary>
        /// ラベリング処理(2D)
        /// </summary>
        /// <param name="thresholddata">1 or 0  byte配列</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static ushort[] Labeling(byte[] thresholddata, int width, int height, Rectangle rect)
        {
            ushort[] labelimg = new ushort[width * height];
            ushort[] Lutlable = new ushort[ushort.MaxValue];

            foreach (int idx in Enumerable.Range(1, ushort.MaxValue - 1))
            {
                Lutlable[idx - 1] = (ushort)idx;
            }

            int lastlabel = 0;
            ///ラベリング　正方向
            foreach (int idx_img in Enumerable.Range(0, width * height))
            {
                var xx = idx_img % (int)(width);
                var yy = idx_img / (int)(width);

                if (xx == width - 1 || xx == 0 || yy == height - 1 || yy == 0) { continue; }


                //if(xx < rect.Left || xx > rect.Right || yy < rect.Top || yy > rect.Bottom)
                //{
                //    continue;
                //}


                if (thresholddata[idx_img] > 0)
                {
                    List<int> sssss = new List<int>()
                    {
                                labelimg[idx_img],
                                labelimg[idx_img - 1],
                                labelimg[idx_img - width + 1],
                                labelimg[idx_img - width],
                                labelimg[idx_img - width - 1],
                    };
                    ushort tmpsss = (ushort)sssss.Min();

                    if (sssss.All(p => p == 0))
                    {
                        labelimg[idx_img] = (ushort)(lastlabel + 1);
                        lastlabel = (ushort)(lastlabel + 1);
                    }
                    else
                    {
                        labelimg[idx_img] = (ushort)sssss.Where(p => p != 0).Min();

                        IEnumerable<int> quer = sssss.FindAll(p => p != labelimg[idx_img]).Where(q => q != 0);

                        if (quer != null)
                        {
                            foreach (var idx in quer)
                            {
                                Lutlable[idx - 1] = labelimg[idx_img];
                            }
                        }
                    }
                }
            }

            ///ラベリング　逆方向
            foreach (int idx_img in Enumerable.Range(0, width * height).Reverse())
            {
                var xx = idx_img % (int)(width);
                var yy = idx_img / (int)(width);

                if (xx == width - 1 || xx == 0 || yy == height - 1 || yy == 0) { continue; }

                //if (xx < rect.Left || xx > rect.Right || yy < rect.Top || yy > rect.Bottom)
                //{
                //    continue;
                //}


                if (thresholddata[idx_img] > 0)
                {
                    List<int> sssss = new List<int>()
                    {
                                labelimg[idx_img],
                                labelimg[idx_img + 1],
                                labelimg[idx_img + width - 1],
                                labelimg[idx_img + width],
                                labelimg[idx_img + width + 1],
                    };
                    ushort tmpsss = (ushort)sssss.Min();

                    if (sssss.All(p => p == 0))
                    {
                        labelimg[idx_img] = (ushort)(lastlabel + 1);
                        lastlabel = (ushort)(lastlabel + 1);
                    }
                    else
                    {
                        labelimg[idx_img] = (ushort)sssss.Where(p => p != 0).Min();

                        IEnumerable<int> quer = sssss.FindAll(p => p != labelimg[idx_img]).Where(q => q != 0);

                        if (quer != null)
                        {
                            foreach (var idx in quer)
                            {
                                Lutlable[idx - 1] = labelimg[idx_img];
                            }
                        }
                    }
                }
            }



            ushort[] resultLut = new ushort[lastlabel + 1];
            resultLut[0] = 1;
            foreach (int idx in Enumerable.Range(1, lastlabel))
            {
                if (Lutlable[idx] == Lutlable[idx - 1])
                {
                    resultLut[idx] = resultLut[idx - 1];
                }
                else
                {
                    resultLut[idx] = (ushort)(resultLut[idx - 1] + 1);
                }
            }

            for (int y = rect.Top; y < rect.Bottom; y++)
            {
                for (int x = rect.Left; x < rect.Right; x++)
                {
                    if (labelimg[y * width + x] > 0)
                    {
                        labelimg[y * width + x] = resultLut[labelimg[y * width + x] - 1];
                    }
                }
            }


            //foreach (int idx_img in Enumerable.Range(0, width * height))
            //{
            //    if (labelimg[idx_img] > 0)
            //    {
            //        labelimg[idx_img] = resultLut[labelimg[idx_img] - 1];
            //    }
            //}
            //ラベルがあって実際の画像にはないラベルを詰める
            ushort relabelmax = labelimg.Max();
            ushort[] relut = new ushort[relabelmax + 1];
            int recnt = 1;
            foreach (int labelidx in Enumerable.Range(1, relabelmax))
            {
                //foreach (int idx_img in Enumerable.Range(0, width * height))
                //{
                //    if (labelimg[idx_img] == labelidx)
                //    {
                //        relut[labelidx] = (ushort)recnt;
                //        recnt++;
                //        break;
                //    }
                //}
                for (int y = rect.Top; y < rect.Bottom; y++)
                {
                    for (int x = rect.Left; x < rect.Right; x++)
                    {
                        if (labelimg[y * width + x] == labelidx)
                        {
                            relut[labelidx] = (ushort)recnt;
                            recnt++;
                            break;
                        }
                    }
                }
            }

            foreach (int idx_img in Enumerable.Range(0, width * height))
            {
                labelimg[idx_img] = relut[labelimg[idx_img]];
            }

            return labelimg;
        }
    }
}
