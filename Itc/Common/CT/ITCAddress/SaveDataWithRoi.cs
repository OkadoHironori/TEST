
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTAddress
{
    /// <summary>
    /// 画像保存クラス
    /// </summary>
    public class SaveDataWithRoi : ISaveDataWithRoi
    {
        /// <summary>
        /// LUTTable本体
        /// </summary>
        public byte[] Lookuptbl { get; private set; }
        /// <summary>
        /// 最大CT値 -8192 or 
        /// </summary>
        public int MaxCTValue { get; private set; }
        /// <summary>
        /// 最小CT値 8191
        /// </summary>
        public int MinCTValue { get; private set; }
        /// <summary>
        /// マトリックスサイズ
        /// </summary>
        public int Matrix { get; private set; }
        /// <summary>
        /// ウィンドウレベル
        /// </summary>
        public int WL { get; private set; }
        /// <summary>
        /// ウィンドウ幅
        /// </summary>
        public int WW { get; private set; }
        /// <summary>
        /// ガンマ値
        /// </summary>
        public float Gamma { get; private set; }
        /// <summary>
        /// 画像データ情報s
        /// </summary>
        public IEnumerable<CTdata> CTdatas { get; private set; }
        /// <summary>
        /// LUTテーブル
        /// </summary>
        private readonly ILUTable _LUTable;
        /// <summary>
        /// CTデータ
        /// </summary>
        private readonly ICTDattum _CTDattum;

        /// <summary>
        /// 画像保存クラス
        /// </summary>
        public SaveDataWithRoi(ILUTable luts, ICTDattum ctdata)
        {
            _LUTable = luts;
            _LUTable.UpdateLUT += (s, e) =>
            {
                if (s is LUTable lut)
                {
                    Lookuptbl = lut.Lookuptbl;
                    MaxCTValue = lut.MaxCTValue;
                    MinCTValue = lut.MinCTValue;
                    WL = lut.WL;
                    WW = lut.WW;
                    Gamma = lut.Gamma;
                }
            };
            _CTDattum = ctdata;
            _CTDattum.EndLoadData += (s, e) =>
            {
                if (s is CTDattum ctds)
                {
                    CTdatas = ctds.CTdatas;
                    Matrix = ctds.Matrix;
                };
            };

        }
        /// <summary>
        /// 画像を保存する
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ext"></param>
        public void DoSaveImage(string path, bool withroi = false)
        {
            var SaveFile = CTdatas.Select(p => p.FilePath.ToString() + "TEST.png").ToList();

            foreach (var ctdata in CTdatas.Select((v, i) => new { v, i }))
            {
                var savebitmap = GetBitmap(Matrix, Matrix, ctdata.v.CTData);
                using (Bitmap drawbmp = new Bitmap(Matrix, Matrix, PixelFormat.Format24bppRgb))
                {
                    using (Graphics g = Graphics.FromImage(drawbmp))
                    {
                        g.DrawImage(savebitmap, 0, 0);//元画像を描画

                        if (withroi)
                        {

                            //_PreFilter.DrawRoiBitmap(g, drawbmp, ctdata.v.Deg);

                            //_Filter.DrawRoiBitmap(g, drawbmp, ctdata.v.Deg);

                            // _Labeling.DrawRoiLabelBitmap(g, drawbmp, ctdata.v.Deg);
                        }

                        //_Analysis.DrawResRoi(g, drawbmp, ctdata.v.Deg);

                    }
                    drawbmp.Save(Path.Combine(path, SaveFile[ctdata.i]), ImageFormat.Png);
                }
            }
        }

        /// <summary>
        /// ビットマップ更新
        /// </summary>
        /// <param name="ctbuf"></param>
        /// <param name="bmp"></param>
        private Bitmap GetBitmap(int width, int height, short[] data)
        {
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            bmp.Palette = GetGrayScalePalette(PixelFormat.Format8bppIndexed);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            unsafe
            {
                //ＣＴ画像データをビットマップデータに変換
                byte* p = (byte*)bmpData.Scan0;
                Parallel.For(0, bmpData.Height, y =>
                {
                    for (int x = 0; x < bmpData.Width; x++)
                    {
                        if (data[y * bmpData.Width + x] > MaxCTValue)
                        {
                            data[y * bmpData.Width + x] = (short)MaxCTValue;
                        }
                        else if (data[y * bmpData.Width + x] < MinCTValue)
                        {
                            data[y * bmpData.Width + x] = (short)MinCTValue;
                        }

                        p[y * bmpData.Stride + x] = Lookuptbl[data[y * bmpData.Width + x] - MinCTValue];

                    }
                    for (int x = bmpData.Width; x < bmpData.Stride; x++)
                    {
                        p[y * bmpData.Stride + x] = 0;
                    }
                });
            }

            //ビットマップデータに対するロックを解除
            bmp.UnlockBits(bmpData);

            return bmp;

        }
        /// <summary>
        /// グレースケール256階調用パレットを取得する
        /// </summary>
        /// <returns></returns>
        private ColorPalette GetGrayScalePalette(System.Drawing.Imaging.PixelFormat fmt)
        {
            //パレット取得用ビットマップの生成
            Bitmap bitmap = new Bitmap(1, 1, fmt);

            //パレット取得
            ColorPalette palette = bitmap.Palette;

            //ビットマップの破棄
            bitmap.Dispose();

            //パレットをグレースケール256階調用にする
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = System.Drawing.Color.FromArgb(i, i, i);
            }

            return palette;
        }
    }
    /// <summary>
    /// 画像保存I/F
    /// </summary>
    public interface ISaveDataWithRoi
    {
        /// <summary>
        /// 画像を保存する
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ext"></param>
        void DoSaveImage(string path, bool withroi = false);
    }
}
