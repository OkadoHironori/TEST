using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itc.Common.NumericalAnalysis
{
    public class Label
    {
        /// <summary>
        /// ラベリング処理
        /// </summary>
        /// <param name="thresholddata"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static ushort[] Labeling(byte[] thresholddata, int width, int height)
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
            //ImageProHelper.CallDrawImagePro(labelimg, width, height);

            ///ラベリング　逆方向
            foreach (int idx_img in Enumerable.Range(0, width * height).Reverse())
            {
                var xx = idx_img % (int)(width);
                var yy = idx_img / (int)(width);

                if (xx == height - 1 || xx == 0 || yy == height - 1 || yy == 0) { continue; }

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
            //ImageProHelper.CallDrawImagePro(labelimg, width, height);

            ///ラベリング　正方向
            foreach (int idx_img in Enumerable.Range(0, width * height))
            {
                var xx = idx_img % (int)(width);
                var yy = idx_img / (int)(width);

                if (xx == width - 1 || xx == 0 || yy == height - 1 || yy == 0) { continue; }

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
            //ImageProHelper.CallDrawImagePro(labelimg, width, height);

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

            foreach (int idx_img in Enumerable.Range(0, width * height))
            {
                if (labelimg[idx_img] > 0)
                {
                    labelimg[idx_img] = resultLut[labelimg[idx_img] - 1];
                }
            }
            //ラベルがあって実際の画像にはないラベルを詰める
            var relabelmax = labelimg.Max();
            ushort[] relut = new ushort[relabelmax + 1];
            int recnt = 1;
            foreach (int labelidx in Enumerable.Range(1, relabelmax))
            {
                foreach (int idx_img in Enumerable.Range(0, width * height))
                {
                    if (labelimg[idx_img] == labelidx)
                    {
                        relut[labelidx] = (ushort)recnt;
                        recnt++;
                        break;
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
