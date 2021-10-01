using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DetectorControl;
using IRayControler;
using Itc.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UT_Managed.UT_Detector.UT_IRAY
{
    [TestClass]
    public class UT_iRayDetector
    {
        
        /// <summary>
        /// サービス登録
        /// </summary>
        /// <returns></returns>
        private static ServiceCollection GetService()
        {
            ServiceCollection services = new ServiceCollection();

            
            services.AddSingleton<IIRayCtrl, IRayCtrl>();
            services.AddSingleton<IIRayConfig, IRayConfig>();
            services.AddSingleton<IIRayDetector, IRayDetector>();
            services.AddSingleton<IIRaySelectMode, IRaySelectMode>();
            services.AddSingleton<IIRayCorrection, IRayCorrection>();
            services.AddSingleton<IIRaySeqAcquisition, IRaySeqAcquisition>();

            services.AddSingleton<IDetectorCtrl, DetectorCtrl>();

            services.AddSingleton<IDetectorData, DetectorData>();
            services.AddSingleton<IDetConf, DetConf>();
            services.AddSingleton<IFlipHorizontal, FlipHorizontal>();
            services.AddSingleton<IFlipVertical, FlipVertical>();
            services.AddSingleton<IRotCCW90, RotCCW90>();
            services.AddSingleton<IRotCW90, RotCW90>();
            services.AddSingleton<ILUTableUS, LUTableUS>();
            services.AddSingleton<IBitmapConverter, BitmapConverter>();
            

            return services;
        }
        /// <summary>
        /// dllのパス登録
        /// </summary>
        [TestInitialize]
        public void Regist()
        {
            string PathDet = "Detector";
            string PathIDet = "iDetector";
            string fullpath = Path.Combine(Environment.CurrentDirectory, PathDet, PathIDet);
            string path = Environment.GetEnvironmentVariable("Path");
            Environment.SetEnvironmentVariable("Path", path + Path.PathSeparator + fullpath);
        }
        [TestMethod]
        public void 画像回転CCW90()
        {
            string testpath = Path.Combine(Directory.GetCurrentDirectory(), "UT_CCW90");
            string savepath_bef = Path.Combine(testpath, "前CCW90.tif");
            string savepath_aft = Path.Combine(testpath, "後CCW90.tif");
            DirectoryProcessor.Delete(Path.Combine(testpath));
            Directory.CreateDirectory(Path.Combine(testpath));
            //データ作成
            int width = 256;
            int height = 128;
            ushort[] data = MakeGray16Data(width, height);

            SaveImage(savepath_bef, data, width, height, PixelFormats.Gray16);

            ushort exres = data.LastOrDefault();
            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IRotCCW90, RotCCW90>();
            servicescollection.AddSingleton<IDetConf, DetConf>();
            
            using (var service = servicescollection.BuildServiceProvider())
            {
                IRotCCW90 ccw90 = service.GetService<IRotCCW90>();
                ccw90.EndRotCCW90 += (s, e) =>
                {
                    RotCCW90 rccw90 = s as RotCCW90;

                    Assert.AreEqual(width, rccw90.Height);

                    Assert.AreEqual(height, rccw90.Width);

                    ushort[] tmpdata = rccw90.Targets;

                    Assert.AreEqual(exres, tmpdata[height-1]);

                    SaveImage(savepath_aft, tmpdata, rccw90.Width, rccw90.Height, PixelFormats.Gray16);
                };
                ccw90.DoRotCCW90(data, width, height);
            }
        }
        [TestMethod]
        public void 画像回転CW90()
        {
            string testpath = Path.Combine(Directory.GetCurrentDirectory(), "UT_CW90");
            string savepath_bef = Path.Combine(testpath, "前CW90.tif");
            string savepath_aft = Path.Combine(testpath, "後CW90.tif");
            DirectoryProcessor.Delete(Path.Combine(testpath));
            Directory.CreateDirectory(Path.Combine(testpath));
            //データ作成
            int width = 256;
            int height = 128;
            ushort[] data = MakeGray16Data(width, height);

            SaveImage(savepath_bef, data, width, height, PixelFormats.Gray16);

            ushort exres = data.LastOrDefault();
            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IRotCW90, RotCW90>();
            servicescollection.AddSingleton<IDetConf, DetConf>();

            using (var service = servicescollection.BuildServiceProvider())
            {
                IRotCW90 cw90 = service.GetService<IRotCW90>();
                cw90.EndRotCW90 += (s, e) =>
                {
                    RotCW90 rcw90 = s as RotCW90;

                    Assert.AreEqual(width, rcw90.Height);

                    Assert.AreEqual(height, rcw90.Width);

                    ushort[] tmpdata = rcw90.Targets;

                    Assert.AreEqual(exres, tmpdata[0 +(rcw90.Height - 1) * rcw90.Width]);

                    SaveImage(savepath_aft, tmpdata, rcw90.Width, rcw90.Height, PixelFormats.Gray16);
                };
                cw90.DoRotCW90(data, width, height);
            }
        }
        [TestMethod]
        public void 左右反転Fliphorizontal()
        {
            string testpath = Path.Combine(Directory.GetCurrentDirectory(), "UT_FlipHorizontal");
            string savepath_bef = Path.Combine(testpath, "前左右反転.tif");
            string savepath_aft = Path.Combine(testpath, "後左右反転.tif");
            DirectoryProcessor.Delete(Path.Combine(testpath));
            Directory.CreateDirectory(Path.Combine(testpath));
            //データ作成
            int width = 256;
            int height = 128;
            ushort[] data = MakeGray16Data(width, height);

            SaveImage(savepath_bef, data, width, height, PixelFormats.Gray16);

            ushort exres = data.LastOrDefault();
            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IFlipHorizontal, FlipHorizontal>();
            servicescollection.AddSingleton<IDetConf, DetConf>();

            using (var service = servicescollection.BuildServiceProvider())
            {
                IFlipHorizontal fliphori = service.GetService<IFlipHorizontal>();
                fliphori.EndFlipHorizontal += (s, e) =>
                {
                    FlipHorizontal fh = s as FlipHorizontal;

                    Assert.AreEqual(width, fh.Width);

                    Assert.AreEqual(height, fh.Height);

                    ushort[] tmpdata = fh.Targets;

                    Assert.AreEqual(exres, tmpdata[0 + (fh.Height - 1) * fh.Width]);

                    SaveImage(savepath_aft, tmpdata, fh.Width, fh.Height, PixelFormats.Gray16);
                };
                fliphori.DoFlipHorizontal(data, width, height);
            }
        }
        [TestMethod]
        public void 上下反転FlipVertical()
        {
            string testpath = Path.Combine(Directory.GetCurrentDirectory(), "UT_FlipVertical");
            string savepath_bef = Path.Combine(testpath, "前_上下反転.tif");
            string savepath_aft = Path.Combine(testpath, "後_上下反転.tif");
            DirectoryProcessor.Delete(Path.Combine(testpath));
            Directory.CreateDirectory(Path.Combine(testpath));
            //データ作成
            int width = 256;
            int height = 128;
            ushort[] data = MakeGray16Data(width, height);

            SaveImage(savepath_bef, data, width, height, PixelFormats.Gray16);

            ushort exres = data.LastOrDefault();
            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IFlipVertical, FlipVertical>();
            servicescollection.AddSingleton<IDetConf, DetConf>();

            using (var service = servicescollection.BuildServiceProvider())
            {
                IFlipVertical fliphori = service.GetService<IFlipVertical>();
                fliphori.EndFlipVertical += (s, e) =>
                {
                    FlipVertical fh = s as FlipVertical;

                    Assert.AreEqual(width, fh.Width);

                    Assert.AreEqual(height, fh.Height);

                    ushort[] tmpdata = fh.Targets;

                    Assert.AreEqual(exres, tmpdata[0 +  fh.Width-1]);

                    SaveImage(savepath_aft, tmpdata, fh.Width, fh.Height, PixelFormats.Gray16);
                };
                fliphori.DoFlipVertical(data, width, height);
            }
        }
        /// <summary>
        /// グラデーションデータ
        /// </summary>
        /// <returns></returns>
        private ushort[] MakeGray16Data(int width, int height)
        {
            ushort[] data = new ushort[width * height];

            double dw = (double)((double)ushort.MaxValue / (height*width));

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    data[i + j * width] = (ushort)(i * j * dw);
                }
            }

            return data;
        }
        /// <summary>
        /// 画像生保存
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="bmpData"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="format"></param>
        /// <param name="dpi"></param>
        private void SaveImage(string filename, Array bmpData, int width, int height, System.Windows.Media.PixelFormat format, double dpi = 96)
        {
            int stride = ((width * format.BitsPerPixel + 31) / 32) * 4;

            System.Windows.Media.Imaging.BitmapSource bs = System.Windows.Media.Imaging.BitmapSource.Create(
                width,
                height,
                dpi,
                dpi,
                format,
                null,
                bmpData,
                stride);

            // ファイル名の拡張子
            var ext = Path.GetExtension(filename).ToLower();

            BitmapEncoder encoder;

            switch (ext)
            {
                case ".bmp":
                    encoder = new BmpBitmapEncoder();
                    break;

                case ".tif":
                    encoder = new TiffBitmapEncoder { Compression = TiffCompressOption.None };
                    break;

                case ".png":
                    encoder = new PngBitmapEncoder();
                    break;

                default:
                    encoder = null;
                    break;
            }

            using (Stream stream = new FileStream(filename, FileMode.Create))
            {
                var bf = BitmapFrame.Create(bs);
                encoder.Frames.Add(bf);
                encoder.Save(stream);
            }
        }
    }
}
