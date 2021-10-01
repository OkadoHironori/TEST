using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DetectorControl
{
    /// <summary>
    /// ビットマップ変換クラス
    /// </summary>
    public class BitmapConverter: IBitmapConverter
    {
        /// <summary>
        /// ビットマップ保存完了
        /// </summary>
        public event EventHandler EndSaveBitmap;

        public string SaveedPath { get; private set; }
        /// <summary>
        /// 保存パス
        /// </summary>
        public string SavePath { get; private set; }
        /// <summary>
        /// 保存名
        /// </summary>
        public string SaveName { get; private set; } = "Live";
        /// <summary>
        /// 保存名
        /// </summary>
        public string FixSaveName { get; private set; } = "FixLive";
        /// <summary>
        /// ファイル番号
        /// </summary>
        public string SaveNo { get; private set; }
        /// <summary>
        /// 拡張子
        /// </summary>
        public string ExtName { get; private set; } = ".png";
        /// <summary>
        /// LUTTable本体
        /// </summary>
        public byte[] Lookuptbl { get; private set; }
        /// <summary>
        /// 最大値 
        /// </summary>
        public int MaxValue { get; private set; }
        /// <summary>
        /// 最小値
        /// </summary>
        public int MinValue { get; private set; }
        /// <summary>
        /// 透視データ
        /// </summary>
        public ushort[] TansImage { get; private set; }
        /// <summary>
        /// 表示画像幅
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// 表示画像高さ
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// LUTのI/F
        /// </summary>
        private readonly ILUTableUS _LUTable;
        /// <summary>
        /// 検出器データ
        /// </summary>
        private readonly IDetectorData _DetData;
        /// <summary>
        /// ビットマップ変換クラス
        /// </summary>
        /// <param name="_lut"></param>
        public BitmapConverter(ILUTableUS _lut, IDetectorData _detdata)
        {
            _LUTable = _lut;
            _LUTable.UpdateLUT += (s, e) =>
            {
                var lut = s as LUTableUS;
                Lookuptbl = lut.Lookuptbl;
                MaxValue = lut.MaxValue;
                MinValue = lut.MinValue;
            };

            _DetData = _detdata;
            _DetData.EndConvertArrayData += (s, e) => 
            {
                var dd = s as DetectorData;
                Width = dd.DispWidth;
                Height = dd.DispHeight;
                TansImage = dd.ArrayData;

                DoSaveFixImage();
            };

            SavePath = Path.Combine(Directory.GetCurrentDirectory(), "UT_連続画像保存");
            if(!Directory.Exists(SavePath))Directory.CreateDirectory(Path.Combine(SavePath));
        }
        /// <summary>
        /// 階調変換後の画像を保存
        /// </summary>
        public void DoSaveImage()
        {
            var fils = Directory.EnumerateFiles(SavePath, $"*{ExtName}", SearchOption.TopDirectoryOnly);
            SaveNo = (fils.Count() + 1).ToString("0000");

            using (Bitmap savebitmap = GetBitmap(Width, Height, TansImage))
            {
                using (Bitmap drawbmp = new Bitmap(Width, Height, PixelFormat.Format24bppRgb))
                {
                    using (Graphics g = Graphics.FromImage(drawbmp))
                    {
                        g.DrawImage(savebitmap, 0, 0);//元画像を描画

                        drawbmp.Save(Path.Combine(SavePath, $"{SaveName}-{SaveNo}{ExtName}"), ImageFormat.Png);

                        SaveedPath = Path.Combine(SavePath, $"{SaveName}-{SaveNo}{ExtName}");
                    }
                }
            }

            EndSaveBitmap?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 階調変換後の画像を保存
        /// </summary>
        public void DoSaveFixImage()
        {
            using (Bitmap savebitmap = GetBitmap(Width, Height, TansImage))
            {
                using (Bitmap drawbmp = new Bitmap(Width, Height, PixelFormat.Format24bppRgb))
                {
                    using (Graphics g = Graphics.FromImage(drawbmp))
                    {
                        g.DrawImage(savebitmap, 0, 0);//元画像を描画

                        drawbmp.Save(Path.Combine(SavePath, $"{FixSaveName}{ExtName}"), ImageFormat.Png);

                        SaveedPath = Path.Combine(SavePath, $"{FixSaveName}{ExtName}");
                    }
                }
            }

            EndSaveBitmap?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// ビットマップ更新
        /// </summary>
        /// <param name="ctbuf"></param>
        /// <param name="bmp"></param>
        private Bitmap GetBitmap(int width, int height, ushort[] data)
        {
            Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            Bitmap bmp = bitmap;
            bmp.Palette = GetGrayScalePalette(System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
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
                        p[y * bmpData.Stride + x] = Lookuptbl[data[y * bmpData.Width + x] - MinValue];

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
    /// ビットマップ変換　I/F
    /// </summary>
    public interface IBitmapConverter
    {
        /// <summary>
        /// 階調変換後の画像を保存
        /// </summary>
        void DoSaveImage();
        /// <summary>
        /// ビットマップ保存完了
        /// </summary>
        event EventHandler EndSaveBitmap;

    }
}
