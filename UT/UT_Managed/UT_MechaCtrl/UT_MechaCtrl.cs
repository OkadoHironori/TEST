using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MechaControl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLCController;

namespace UT_Managed.UT_MechaCtrl
{
    [TestClass]
    public class UT_MechaCtrl
    {
        /// <summary>
        /// サービス登録
        /// </summary>
        /// <returns></returns>
        private static ServiceCollection GetService()
        {
            ServiceCollection servicescollection = new ServiceCollection();

            servicescollection.AddSingleton<IPLCAddress, PLCAddress>(p =>
            {
                return new PLCAddress("CT30K");
            });
            servicescollection.AddSingleton<IPLCChecker, PLCChecker>();
            servicescollection.AddSingleton<IPLCComChecker, PLCComChecker>();
            servicescollection.AddSingleton<IPLCServer, PLCServer>();
            servicescollection.AddSingleton<IPLCtoPC, PLCtoPC>();
            servicescollection.AddSingleton<IPLCProvider, PLCProvider>();
            servicescollection.AddSingleton<IPCtoPLC, PCtoPLC>();
            servicescollection.AddSingleton<IPLCControl, PLCControl>();
            return servicescollection;
        }

        [TestMethod]
        public void FCDとPLCの連携()
        {
            int evcnt = 0;
            ServiceCollection servicescollection = GetService();

            using (var service = servicescollection.BuildServiceProvider())
            {
                var mcm = service.GetService<IFCD_Ctrl>();
                //mcm.EndLoadFile += (s, e) =>
                //{
                //    evcnt++;
                //    var mcmes = s as MechaCtrlMes;
                //    Assert.AreEqual(16, mcmes.MechaMess.Count());
                //};
                mcm.Origin();
            }

            Assert.AreEqual(1, evcnt);
        }
        [TestMethod]
        public void メカコン確認()
        {
            int evcnt = 0;
            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IPLCAddress, PLCAddress>(p =>
            {
                return new PLCAddress("CT30K");
            });
            servicescollection.AddSingleton<IPLCChecker, PLCChecker>();
            servicescollection.AddSingleton<IPLCComChecker, PLCComChecker>();
            servicescollection.AddSingleton<IPLCServer, PLCServer>();
            servicescollection.AddSingleton<IPLCtoPC, PLCtoPC>();
            servicescollection.AddSingleton<IPLCProvider, PLCProvider>();
            servicescollection.AddSingleton<IPCtoPLC, PCtoPLC>();
            servicescollection.AddSingleton<IPLCControl, PLCControl>();

            
            servicescollection.AddSingleton<IMechaCtrlMes, MechaCtrlMes>();
            servicescollection.AddSingleton<IFCD_Ctrl, FCD_Ctrl>();
            servicescollection.AddSingleton<IFCD_Fix, FCD_Fix>();


            using (var service = servicescollection.BuildServiceProvider())
            {
                var serv = service.GetService<IPLCServer>();
                var fcds = service.GetService<IFCD_Ctrl>();
                fcds.PropertyChanged += (s, e) =>
                {
                    var pls = s as FCD_Ctrl;

                    evcnt++;
                };

                serv.Start();
                //fcds.Init();

                Task.WaitAll(Task.Delay(10000));

                //serv.Stop();
            }
            Assert.IsTrue(1 < evcnt);
        }

        [TestMethod]
        public void メカコントロールの指示()
        {
            int evcnt = 0;
            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IPLCAddress, PLCAddress>(p =>
            {
                return new PLCAddress("CT30K");
            });
            servicescollection.AddSingleton<IPLCChecker, PLCChecker>();
            servicescollection.AddSingleton<IPLCComChecker, PLCComChecker>();
            servicescollection.AddSingleton<IPLCServer, PLCServer>();
            servicescollection.AddSingleton<IPLCtoPC, PLCtoPC>();
            servicescollection.AddSingleton<IPCtoPLC, PCtoPLC>();
            servicescollection.AddSingleton<IPLCProvider, PLCProvider>();
            servicescollection.AddSingleton<IPLCControl, PLCControl>();

            servicescollection.AddSingleton<IMechaCtrlMes, MechaCtrlMes>();

            using (var service = servicescollection.BuildServiceProvider())
            {
                var mcm = service.GetService<IMechaCtrlMes>();
                mcm.EndLoadFile += (s, e) =>
                {
                    evcnt++;
                    var mcmes = s as MechaCtrlMes;
                    Assert.AreEqual(16, mcmes.MechaMess.Count());
                };
                mcm.RequestMes();
            }

            Assert.AreEqual(1, evcnt);
        }
        [TestMethod]
        public void PLCコントローラからの指示()
        {
            int evcnt = 0;
            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IPLCAddress, PLCAddress>(p =>
            {
                return new PLCAddress("CT30K");
            });
            servicescollection.AddSingleton<IPLCChecker, PLCChecker>();
            servicescollection.AddSingleton<IPLCComChecker, PLCComChecker>();
            servicescollection.AddSingleton<IPLCServer, PLCServer>();
            servicescollection.AddSingleton<IPLCtoPC, PLCtoPC>();
            servicescollection.AddSingleton<IPCtoPLC, PCtoPLC>();
            servicescollection.AddSingleton<IPLCProvider, PLCProvider>();
            servicescollection.AddSingleton<IPLCControl, PLCControl>();

            using (var service = servicescollection.BuildServiceProvider())
            {
                var prop = service.GetService<IPLCControl>();
                prop.PLCChanged += (s, e) =>
                {
                    var pls = s as PLCControl;

                    if (Enum.TryParse(pls.ElementType, out PLCVType type))
                    {
                        switch (type)
                        {
                            case PLCVType.BOOL:
                                Debug.WriteLine($"変化したパラメータ{pls.ElementName} {pls.BoolStatus}");
                                break;
                            case PLCVType.FLOAT:
                                Debug.WriteLine($"変化したパラメータ{pls.ElementName} {pls.FloatStatus}");
                                break;
                            case PLCVType.INT:
                                Debug.WriteLine($"変化したパラメータ{pls.ElementName} {pls.IntStatus}");
                                break;
                        }
                    }
                    evcnt++;
                };

                prop.SendMessage("FcdIndexPosition", 150.5F);//実行コマンド

                Task.WaitAll(Task.Delay(400));

                prop.SendMessage("FcdIndex", true);

                Task.WaitAll(Task.Delay(15000));

                prop.SendMessage("PanelInhibit", true);

                Task.WaitAll(Task.Delay(2000));

                Debug.WriteLine("動きます!");

                prop.SendMessage("FcdOrigin", true);//実行コマンド

                Task.WaitAll(Task.Delay(10000));

                prop.SendMessage("PanelInhibit", false);

            }
            Assert.IsTrue(1 < evcnt);
        }
        [TestMethod]
        public void PLCコントローラの確認()
        {
            int evcnt = 0;
            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IPLCAddress, PLCAddress>(p =>
            {
                return new PLCAddress("CT30K");
            });
            servicescollection.AddSingleton<IPLCChecker, PLCChecker>();
            servicescollection.AddSingleton<IPLCComChecker, PLCComChecker>();
            servicescollection.AddSingleton<IPLCServer, PLCServer>();
            servicescollection.AddSingleton<IPLCtoPC, PLCtoPC>();
            servicescollection.AddSingleton<IPCtoPLC, PCtoPLC>();
            servicescollection.AddSingleton<IPLCProvider, PLCProvider>();
            servicescollection.AddSingleton<IPLCControl, PLCControl>();


            using (var service = servicescollection.BuildServiceProvider())
            {
                var serv = service.GetService<IPLCServer>();
                var prop = service.GetService<IPLCControl>();
                prop.PLCChanged += (s, e) =>
                {
                    var pls = s as PLCControl;

                    if (Enum.TryParse(pls.ElementType, out PLCVType type))
                    {
                        switch (type)
                        {
                            case PLCVType.BOOL:
                                Debug.WriteLine($"変化したパラメータ{pls.ElementName} {pls.BoolStatus}");
                                break;
                            case PLCVType.FLOAT:
                                Debug.WriteLine($"変化したパラメータ{pls.ElementName} {pls.FloatStatus}");
                                break;
                            case PLCVType.INT:
                                Debug.WriteLine($"変化したパラメータ{pls.ElementName} {pls.IntStatus}");
                                break;
                        }
                    }
                    evcnt++;
                };

                serv.Start();

                Task.WaitAll(Task.Delay(2000));

                prop.SendMessage("PanelInhibit", true);

                prop.SendMessage("TblYOrigin", true);//実行コマンド


                Task.WaitAll(Task.Delay(60000));

                prop.SendMessage("PanelInhibit", false);

                serv.Stop();
            }
            Assert.IsTrue(1 < evcnt);
        }
        [TestMethod]
        public void プロパティ変更通知の確認()
        {
            int evcnt = 0;
            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IPLCAddress, PLCAddress>(p =>
            {
                return new PLCAddress("CT30K");
            });
            servicescollection.AddSingleton<IPLCChecker, PLCChecker>();
            servicescollection.AddSingleton<IPLCComChecker, PLCComChecker>();
            servicescollection.AddSingleton<IPLCServer, PLCServer>();
            servicescollection.AddSingleton<IPLCtoPC, PLCtoPC>();
            servicescollection.AddSingleton<IPLCProvider, PLCProvider>();

            using (var service = servicescollection.BuildServiceProvider())
            {
                var serv = service.GetService<IPLCServer>();
                var prop = service.GetService<IPLCProvider>();
                prop.PLCChanged += (s, e) =>
                {
                    var pls = s as PLCProvider;

                    if(Enum.TryParse(pls.ElementType, out PLCVType type))
                    {
                        switch (type)
                        {
                            case PLCVType.BOOL:
                                Debug.WriteLine($"変化したパラメータ{pls.ElementName} {pls.BoolStatus}");
                                break;
                            case PLCVType.FLOAT:
                                Debug.WriteLine($"変化したパラメータ{pls.ElementName} {pls.FloatStatus}");
                                break;
                            case PLCVType.INT:
                                Debug.WriteLine($"変化したパラメータ{pls.ElementName} {pls.IntStatus}");
                                break;
                        }
                    }
                    evcnt++;
                };

                serv.Start();

                Task.WaitAll(Task.Delay(1000));

                serv.Stop();
            }
            Assert.IsTrue(1 < evcnt);
        }

        [TestMethod]
        public void シリアル通信の確認()
        {
            int evcnt = 0;
            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IPLCAddress, PLCAddress>(p =>
            {
                return new PLCAddress("CT30K");
            });
            servicescollection.AddSingleton<IPLCChecker, PLCChecker>();
            servicescollection.AddSingleton<IPLCComChecker, PLCComChecker>();
            servicescollection.AddSingleton<IPLCServer, PLCServer>();

            using (var service = servicescollection.BuildServiceProvider())
            {
                var serv = service.GetService<IPLCServer>();
                serv.PLCBitMessage += (s, e) =>
                {
                    var pls = s as PLCServer;
                    string tmpmes = Encoding.GetEncoding(Encoding.ASCII.EncodingName).GetString(pls.Respons).ToString();
                    var startproto = tmpmes.Substring(0, 5);
                    var endproto = tmpmes.Substring(tmpmes.Length - 3, 3);
                    Debug.WriteLine($"{tmpmes}");
                    evcnt++;
                };
                serv.PLCWordMessage += (s, e) =>
                {
                    var pls = s as PLCServer;
                    string tmpmes = Encoding.GetEncoding(Encoding.ASCII.EncodingName).GetString(pls.Respons).ToString();
                    Debug.WriteLine($"{tmpmes}");
                    evcnt++;
                };

                serv.Start();

                Task.WaitAll(Task.Delay(2000));

                serv.Stop();
            }
            Assert.IsTrue(1 < evcnt);
        }

        [TestMethod]
        public void PLCComチェッカーの確認()
        {
            int evcnt = 0;
            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IPLCAddress, PLCAddress>(p =>
            {
                return new PLCAddress("CT30K");
            });
            servicescollection.AddSingleton<IPLCChecker, PLCChecker>();
            servicescollection.AddSingleton<IPLCComChecker, PLCComChecker>();

            using (var service = servicescollection.BuildServiceProvider())
            {
                var chk = service.GetService<IPLCComChecker>();
                chk.DoComCheck += (s, e) =>
                {
                    var plad = s as PLCComChecker;
                    evcnt++;
                };
                chk.DoComChecker();
                Task.WaitAll(Task.Delay(2000));                
            }
            Assert.IsTrue(1 < evcnt);
        }

        [TestMethod]
        public void PLCチェッカーの確認()
        {
            int evcnt = 0;
            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IPLCAddress, PLCAddress>(p =>
            {
                return new PLCAddress("CT30K");
            });
            servicescollection.AddSingleton<IPLCChecker, PLCChecker>();

            using (var service = servicescollection.BuildServiceProvider())
            {
                var chk = service.GetService<IPLCChecker>();
                chk.DoCheck += (s, e) =>
                {
                    var plad = s as PLCChecker;
                    evcnt++;
                };
                chk.Start();

                Task.WaitAll(Task.Delay(1000));

                chk.Stop();
            }
            Assert.IsTrue(5<evcnt);
        }
        [TestMethod]
        public void PLCパラメータの読込()
        {
            int evcnt = 0;
            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IPLCAddress, PLCAddress>(p => 
            {
                return new PLCAddress("CT30K");
            });

            using (var service = servicescollection.BuildServiceProvider())
            {
                var add = service.GetService<IPLCAddress>();
                add.EndLoadCSV += (s, e) =>
                {
                    var plad = s as PLCAddress;
                    evcnt++;
                    Assert.AreEqual(2, plad.ReadPLCMap.Count());
                };
                add.RequestParam();
            }
            Assert.AreEqual(1, evcnt);
        }


    }
}
