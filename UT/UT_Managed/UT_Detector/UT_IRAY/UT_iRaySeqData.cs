using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using DetectorControl;
using IRayControler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UT_Managed.UT_Detector.UT_IRAY
{
    [TestClass]
    public class UT_iRaySeqData
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
        public void iRay終了()
        {
            ServiceCollection serviceCollection = GetService();
            using (var service = serviceCollection.BuildServiceProvider())
            {
                IIRayCtrl ctrl = service.GetService<IIRayCtrl>();
                ctrl.EndChageMode += (s, e) =>
                {
                    var tt = s as IRayCtrl;
                    Debug.WriteLine($"設定:{tt.CorrectionParam}");
                };
                ctrl.ChangeMode("Mode1");
            }
        }

        [TestMethod]
        public void iRay社データ収集()
        {
            ServiceCollection serviceCollection = GetService();
            using (var service = serviceCollection.BuildServiceProvider())
            {
                IIRaySeqAcquisition acq = service.GetService<IIRaySeqAcquisition>();
                var savefile = service.GetService<IBitmapConverter>();
                savefile.EndSaveBitmap += (s, e) => 
                {
                    var tt = s as BitmapConverter;    
                    Debug.WriteLine($"{tt.SaveedPath}");
                };
                acq.SetCorrection();
                acq.DoAcqData();

                Task.WaitAll(Task.Delay(10000));

                acq.DoStopAcqData();

            }
        }
    }
}
