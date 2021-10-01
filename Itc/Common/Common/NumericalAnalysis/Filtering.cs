using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itc.Common.NumericalAnalysis
{
    public class Filtering
    {
        /// <summary>
        /// 強調フィルタ
        /// 右側を強調  -1,0,1,-1,0,1,-1,0,1,
        /// 左側を強調   1,0,-1,1,0,-1,1,0,-1,
        /// 上側を強調   1,1,1,0,0,0,-1,-1,-1,
        /// 下側を強調   -1,-1,-1,0,0,0,1,1,1,
        /// </summary>
        /// <param name="org"></param>
        /// <param name="filterData"></param>
        /// <param name="kernel"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="arc"></param>
        public static void EnhanceFiltering(short[] org, out short[] filterData, List<short> kernel, int width, int height, Rectangle? arc = null)
        {
            filterData = new short[width * height];

            Rectangle anaRect = new Rectangle(0, 0, width, height);
            if (arc != null)
            {
                anaRect = new Rectangle((int)arc?.X,
                                        (int)arc?.Y,
                                        (int)arc?.Width,
                                        (int)arc?.Height);

            }

            short[] orgmed = new short[width * height];

            for (int y = anaRect.Top - 3; y < anaRect.Bottom + 3; y++)
            {
                for (int x = anaRect.Left - 3; x < anaRect.Right + 3; x++)
                {
                    if (y < 3 || y > (height - 3 - 1))
                    {
                        continue;
                    }

                    if (x < 3 || x > (width - 3 - 1))
                    {
                        continue;
                    }

                    List<int> values = new List<int>()
                    {
                        org[(y - 3) * width + (x - 3)],
                        org[(y - 3) * width + (x - 2)],
                        org[(y - 3) * width + (x - 1)],
                        org[(y - 3) * width + x],
                        org[(y - 3) * width + (x + 1)],
                        org[(y - 3) * width + (x + 2)],
                        org[(y - 3) * width + (x + 3)],

                        org[(y - 2) * width + (x - 3)],
                        org[(y - 2) * width + (x - 2)],
                        org[(y - 2) * width + (x - 1)],
                        org[(y - 2) * width + x],
                        org[(y - 2) * width + (x + 1)],
                        org[(y - 2) * width + (x + 2)],
                        org[(y - 2) * width + (x + 3)],

                        org[(y - 1) * width + (x - 3)],
                        org[(y - 1) * width + (x - 2)],
                        org[(y - 1) * width + (x - 1)],
                        org[(y - 1) * width + x],
                        org[(y - 1) * width + (x + 1)],
                        org[(y - 1) * width + (x + 2)],
                        org[(y - 1) * width + (x + 3)],

                        org[(y - 0) * width + (x - 3)],
                        org[(y - 0) * width + (x - 2)],
                        org[(y - 0) * width + (x - 1)],
                        org[(y - 0) * width + x],
                        org[(y - 0) * width + (x + 1)],
                        org[(y - 0) * width + (x + 2)],
                        org[(y - 0) * width + (x + 3)],

                        org[(y + 1) * width + (x - 3)],
                        org[(y + 1) * width + (x - 2)],
                        org[(y + 1) * width + (x - 1)],
                        org[(y + 1) * width + x],
                        org[(y + 1) * width + (x + 1)],
                        org[(y + 1) * width + (x + 2)],
                        org[(y + 1) * width + (x + 3)],

                        org[(y + 2) * width + (x - 3)],
                        org[(y + 2) * width + (x - 2)],
                        org[(y + 2) * width + (x - 1)],
                        org[(y + 2) * width + x],
                        org[(y + 2) * width + (x + 1)],
                        org[(y + 2) * width + (x + 2)],
                        org[(y + 2) * width + (x + 3)],

                        org[(y + 3) * width + (x - 3)],
                        org[(y + 3) * width + (x - 2)],
                        org[(y + 3) * width + (x - 1)],
                        org[(y + 3) * width + x],
                        org[(y + 3) * width + (x + 1)],
                        org[(y + 3) * width + (x + 2)],
                        org[(y + 3) * width + (x + 3)],

                    };
                    orgmed[y * width + x] = (short)values.OrderByDescending(p => p).ElementAt(5);
                }
            }

            foreach (int idx_img in Enumerable.Range(0, width * height))
            {
                var xx = idx_img % (int)(width);
                var yy = idx_img / (int)(width);


                if (xx == width - 1 || xx == 0 || yy == height - 1 || yy == 0)
                {
                    filterData[idx_img] = -8191;
                }
                else if (anaRect.X <= xx && anaRect.X + anaRect.Width > xx && anaRect.Y <= yy && anaRect.Y + anaRect.Height > yy)
                {
                    List<short> tmpdata = new List<short>()
                    {
                        orgmed[idx_img - 1 - width],
                        orgmed[idx_img - width],
                        orgmed[idx_img + 1 - width],
                        orgmed[idx_img - 1],
                        orgmed[idx_img ],
                        orgmed[idx_img + 1],
                        orgmed[idx_img - 1 + width],
                        orgmed[idx_img + width],
                        orgmed[idx_img + 1 + width],
                    };

                    List<float> caldata = new List<float>();
                    foreach (var coef in kernel.Select((v, i) => new { v, i }))
                    {
                        var data = coef.v * tmpdata[coef.i];
                        caldata.Add(data);
                    }

                    filterData[idx_img] = (short)(caldata.Sum());
                }
                else
                {
                    filterData[idx_img] = -8191;
                }
            }
        }
        public static IEnumerable<double> GetHorizontalProfile(short[] org, int width, int height, Rectangle? arc = null)
        {

            Rectangle anaRect = new Rectangle(0, 0, width, height);
            if (arc != null)
            {
                anaRect = new Rectangle((int)arc?.X,
                                        (int)arc?.Y,
                                        (int)arc?.Width/2,
                                        (int)arc?.Height);

            }

            List<double> data = new List<double>();
            int chousei = 0;
            if (anaRect.Bottom == height) { chousei = 1; }

            foreach (var idx_X in Enumerable.Range(anaRect.Left, anaRect.Width))
            {
                List<int> tmp = new List<int>();                
                foreach (var idx_Y in Enumerable.Range(anaRect.Top, anaRect.Height- chousei))
                {
                    if(org[idx_X + idx_Y * width] == -8192 || org[idx_X + idx_Y * width] == -8191)
                    {
                        throw new Exception("Error Value");
                    }

                    var tmpdata = org[idx_X + idx_Y * width];
                    if (tmpdata < 0)
                    {
                        tmpdata = 0;
                    }

                    tmp.Add(tmpdata);

                    //if (idx_Y > anaRect.Top + anaRect.Height - 3)
                    //{
                    //    Debug.WriteLine($"{org[idx_X + idx_Y * width] + 8192}");
                    //}

                }

                data.Add(tmp.Average());
            }



            return data;
            
        }
            /// <summary>
            /// メディアン
            /// </summary>
            /// <param name="org"></param>
            /// <param name="filterData"></param>
            /// <param name="kernel"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <param name="arc"></param>
            public static short[] MedianFilter(short[] org, int width, int height)
        {
            short[] orgmed = new short[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y < 3 || y > (height - 3 - 1))
                    {
                        continue;
                    }

                    if (x < 3 || x > (width - 3 - 1))
                    {
                        continue;
                    }

                    List<int> values = new List<int>()
                    {
                        org[(y - 3) * width + (x - 3)],
                        org[(y - 3) * width + (x - 2)],
                        org[(y - 3) * width + (x - 1)],
                        org[(y - 3) * width + x],
                        org[(y - 3) * width + (x + 1)],
                        org[(y - 3) * width + (x + 2)],
                        org[(y - 3) * width + (x + 3)],

                        org[(y - 2) * width + (x - 3)],
                        org[(y - 2) * width + (x - 2)],
                        org[(y - 2) * width + (x - 1)],
                        org[(y - 2) * width + x],
                        org[(y - 2) * width + (x + 1)],
                        org[(y - 2) * width + (x + 2)],
                        org[(y - 2) * width + (x + 3)],

                        org[(y - 1) * width + (x - 3)],
                        org[(y - 1) * width + (x - 2)],
                        org[(y - 1) * width + (x - 1)],
                        org[(y - 1) * width + x],
                        org[(y - 1) * width + (x + 1)],
                        org[(y - 1) * width + (x + 2)],
                        org[(y - 1) * width + (x + 3)],

                        org[(y - 0) * width + (x - 3)],
                        org[(y - 0) * width + (x - 2)],
                        org[(y - 0) * width + (x - 1)],
                        org[(y - 0) * width + x],
                        org[(y - 0) * width + (x + 1)],
                        org[(y - 0) * width + (x + 2)],
                        org[(y - 0) * width + (x + 3)],

                        org[(y + 1) * width + (x - 3)],
                        org[(y + 1) * width + (x - 2)],
                        org[(y + 1) * width + (x - 1)],
                        org[(y + 1) * width + x],
                        org[(y + 1) * width + (x + 1)],
                        org[(y + 1) * width + (x + 2)],
                        org[(y + 1) * width + (x + 3)],

                        org[(y + 2) * width + (x - 3)],
                        org[(y + 2) * width + (x - 2)],
                        org[(y + 2) * width + (x - 1)],
                        org[(y + 2) * width + x],
                        org[(y + 2) * width + (x + 1)],
                        org[(y + 2) * width + (x + 2)],
                        org[(y + 2) * width + (x + 3)],

                        org[(y + 3) * width + (x - 3)],
                        org[(y + 3) * width + (x - 2)],
                        org[(y + 3) * width + (x - 1)],
                        org[(y + 3) * width + x],
                        org[(y + 3) * width + (x + 1)],
                        org[(y + 3) * width + (x + 2)],
                        org[(y + 3) * width + (x + 3)],

                    };
                    orgmed[y * width + x] = (short)values.OrderByDescending(p => p).ElementAt(5);
                }
            }

            return orgmed;
        }
    }
}
