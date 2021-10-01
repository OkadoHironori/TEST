using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Board.BoardControl;
using Itc.Common.TXEnum;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Hicpd530;

namespace UT_Managed
{
    [TestClass]
    public class UT_Board
    {
        /// <summary>
        /// サービス登録
        /// </summary>
        /// <returns></returns>
        private static ServiceCollection GetService()
        {
            bool noboad = false;

            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IBoardInit, BoardInit>(p =>
            {
                if (noboad)
                {
                    List<Devinf> list = new List<Devinf>()
                {
                    new Devinf(){DevNum=1,Hpcdeviceinfo=new HPCDEVICEINFO()}
                };
                    return new BoardInit(list);
                }
                else
                {
                    return new BoardInit();
                };
            });

            services.AddSingleton<IBoardConfig, BoardConfig>();
            
            services.AddSingleton<IBoardSender, BoardSender>();

            services.AddSingleton<IBoardControl, BoardControl>();
            services.AddSingleton<IBoardProvider, BoardProvider>();
            
            services.AddSingleton<IRotParamFix, RotParamFix>();
            services.AddSingleton<IRotationCtrl, RotationCtrl>();

            services.AddSingleton<IUdParamFix, UdParamFix>();
            services.AddSingleton<IUpDownCtrl, UpDownCtrl>();

            services.AddSingleton<IYStageCtrl, YStageCtrl>();
            services.AddSingleton<IYStgParamFix, YStgParamFix>();

            services.AddSingleton<IXStageCtrl, XStageCtrl>();
            services.AddSingleton<IXStgParamFix, XStgParamFix>();

            return services;
        }
        [TestMethod]
        public void ボードパラメータ読()
        {
            int testcnt = 0;

            ServiceCollection serviceCollection = GetService();
            using (var service = serviceCollection.BuildServiceProvider())
            {
                var boardconfService = service.GetService<IBoardConfig>();
                boardconfService.EndLoadBoardConf += (s, e) =>
                {
                    var bf = s as BoardConfig;

                    testcnt++;
                };
                boardconfService.RequestParam();
            }
            Assert.AreEqual(1, testcnt);
        }
        [TestMethod]
        public void ボード_回転_手動()
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
                boardRotService.Manual(RevMode.CW,10);

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

            float rotPosi = 0;
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

                    Debug.WriteLine($"PLC送信 {br.RotPos}");
                    testcnt++;
                };
                boardRotService.RequestParamEvent += (s, e) =>
                {
                    var br = s as RotationCtrl;
                    rotPosi = br.RotPos;
                };

                boardRotService.Origin(Cts);

                boardRotService.RequestParam();
            }
            Assert.IsTrue(testcnt>0);
            Assert.AreEqual(0F, rotPosi);
        }
        [TestMethod]
        public void ボード_昇降_原点復帰()
        {
            int testcnt = 0;

            ServiceCollection serviceCollection = GetService();

            using (var service = serviceCollection.BuildServiceProvider())
            {

                CancellationTokenSource Cts = new CancellationTokenSource();

                var bproservice = service.GetService<IBoardProvider>();
                bproservice.CheckerStart();
                var boardUpDownService = service.GetService<IUpDownCtrl>();
                boardUpDownService.NotifyCount += (s, e) =>
                {
                    var br = s as UpDownCtrl;
                    testcnt++;
                };
                boardUpDownService.Respons += (s, e) =>
                {
                    var br = s as UpDownCtrl;
                    Assert.AreEqual(300, br.UdPos);
                };

                Task.WaitAll(Task.Delay(1000));
                boardUpDownService.Index(PosiMode.Abso, 3, Cts);
                Task.WaitAll(Task.Delay(1000));
                boardUpDownService.Origin(Cts);

                boardUpDownService.Request();

                //Task.WaitAll(Task.Delay(5000));
            }
        }
        [TestMethod]
        public void ボード_原点復帰後に下降限移動()
        {
            int testcnt = 0;

            ServiceCollection serviceCollection = GetService();

            using (var service = serviceCollection.BuildServiceProvider())
            {

                CancellationTokenSource Cts = new CancellationTokenSource();

                var bproservice = service.GetService<IBoardProvider>();
                bproservice.CheckerStart();
                var boardUpDownService = service.GetService<IUpDownCtrl>();
                boardUpDownService.NotifyCount += (s, e) =>
                {
                    var br = s as UpDownCtrl;
                    testcnt++;
                };
                boardUpDownService.Respons += (s, e) =>
                {
                    var br = s as UpDownCtrl;

                    Assert.IsTrue(br.UdCwEls);
                    //Assert.AreEqual(630, br.UdPos);
                };

                Task.WaitAll(Task.Delay(1000));
                boardUpDownService.Index(PosiMode.Abso, 3, Cts);
                Task.WaitAll(Task.Delay(1000));
                boardUpDownService.Origin(Cts);

                //boardUpDownService.Request();


                boardUpDownService.Index(PosiMode.Rela, 330, Cts);

                //boardUpDownService.Request();
                boardUpDownService.Request();



                //Task.WaitAll(Task.Delay(5000));
            }
        }
        [TestMethod]
        public void ボード_昇降_インデックス()
        {
            int testcnt = 0;

            ServiceCollection serviceCollection = GetService();

            using (var service = serviceCollection.BuildServiceProvider())
            {

                var CurrentPosi = 0.0F;


                CancellationTokenSource Cts = new CancellationTokenSource();

                var bproservice = service.GetService<IBoardProvider>();

                var boardUpDownService = service.GetService<IUpDownCtrl>();
                boardUpDownService.Respons += (s, e) =>
                {
                    var br = s as UpDownCtrl;

                    CurrentPosi = br.UdPos;

                    testcnt++;
                };
                boardUpDownService.Request();


                bproservice.CheckerStart();

               // Task.WaitAll(Task.Delay(5000));

                boardUpDownService.Index(PosiMode.Abso,100,Cts);

                //Task.WaitAll(Task.Delay(5000));
            }
        }
    }
}
