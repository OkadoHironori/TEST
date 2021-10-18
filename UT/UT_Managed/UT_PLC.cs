using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Board.BoardControl;
using Itc.Common.TXEnum;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLCController;

namespace UT_Managed
{
    [TestClass]
    public class UT_PLC
    {
        /// <summary>
        /// サービス登録
        /// </summary>
        /// <returns></returns>
        private static ServiceCollection GetService()
        {
            ServiceCollection services = new ServiceCollection();

            
            services.AddSingleton<IPLCProvider, PLCProvider>();
            services.AddSingleton<IPLCAddress, PLCAddress>();
            services.AddSingleton<IPLCServer, PLCServer>();
            services.AddSingleton<IPCtoPLC, PCtoPLC>();
            services.AddSingleton<IPLCChecker, PLCChecker>();            
            services.AddSingleton<IPLCControl, PLCControl>();
            services.AddSingleton<IPLCtoPC, PLCtoPC>();

            return services;
        }
        [TestMethod]
        public void PLCパラメータ読()
        {
            //int testcnt = 0;

            //ServiceCollection serviceCollection = GetService();
            //using (var service = serviceCollection.BuildServiceProvider())
            //{
            //    var plcCntService = service.GetService<IPLCControl>();
            //    plcCntService.StatusChanged += (s, e) =>
            //    {
            //        DateTime dt = DateTime.Now;
            //        var bf = s as PLCControl;
            //        var stsname = (s as PLCmodel).Element;
            //        Debug.WriteLine($"{dt} Pro:{e.PropertyName} ElementName{stsname}");
            //        testcnt++;
            //    };
            //    plcCntService.ConnectPLC();
            //    Task.WaitAll(Task.Delay(10000));
            //}
            //Assert.IsTrue(testcnt>0);
        }
        [TestMethod]
        public void PLC_FCD_原点復帰()
        {
            int testcnt = 0;

            ServiceCollection serviceCollection = GetService();


            using (var service = serviceCollection.BuildServiceProvider())
            {

                CancellationTokenSource Cts = new CancellationTokenSource();

                var bproservice = service.GetService<IBoardProvider>();
                bproservice.CheckerStart();
                var boardRotService = service.GetService<IRotationCtrl>();
                boardRotService.NotifyCount += (s, e) =>
                {
                    var br = s as RotationCtrl;

                    testcnt++;
                };

                boardRotService.Manual(RevMode.CW,1);

                Task.WaitAll(Task.Delay(5000));

                boardRotService.Stop(StopMode.Fast);

            }

        }
        [TestMethod]
        public void ボード_回転_インデックス()
        {
            int testcnt = 0;

            ServiceCollection serviceCollection = GetService();


            using (var service = serviceCollection.BuildServiceProvider())
            {

                CancellationTokenSource Cts = new CancellationTokenSource();

                var bproservice = service.GetService<IBoardProvider>();
                bproservice.CheckerStart();
                var boardRotService = service.GetService<IRotationCtrl>();
                boardRotService.NotifyCount += (s, e) =>
                {
                    var br = s as RotationCtrl;

                    testcnt++;
                };
                boardRotService.Index(PosiMode.Abso, -100, Cts);
            }
        }
        [TestMethod]
        public void ボード_回転_原点復帰()
        {
            int testcnt = 0;

            ServiceCollection serviceCollection = GetService();
            using (var service = serviceCollection.BuildServiceProvider())
            {

                CancellationTokenSource Cts =  new CancellationTokenSource();

                var bproservice = service.GetService<IBoardProvider>();
                bproservice.CheckerStart();
                var boardRotService = service.GetService<IRotationCtrl>();
                boardRotService.NotifyCount += (s, e) =>
                {
                    var br = s as RotationCtrl;

                    testcnt++;
                };

                boardRotService.Origin(Cts);
            }
        }
    }
}
