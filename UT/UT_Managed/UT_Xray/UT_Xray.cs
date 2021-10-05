using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XrayControl;

namespace UT_Managed.UT_Xray
{
    [TestClass]
    public class UT_Xray
    {
        /// <summary>
        /// デバック用シリアルに繋がっているかどうか？
        /// </summary>
        public bool NoComPort = true;
        [TestMethod]
        public void X線管電流_管電圧の設定()
        {
            List<int> cmd = new List<int>();
            int evcnt = 0;

            int setvalue = 30;

            int setvaluecur = 60;

            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IXrayAddres_L9181_02LT, XrayAddres_L9181_02LT>();
            servicescollection.AddSingleton<IXrayCheckerFrequency, XrayCheckerFrequency>();
            servicescollection.AddSingleton<IXrayCheckerIrregularity, XrayCheckerIrregularity>();
            servicescollection.AddSingleton<IXraySerialService, XraySerialService>(p =>
            {
                return new XraySerialService(new XrayCheckerFrequency(new XrayAddres_L9181_02LT()),
                                            new XrayCheckerIrregularity(new XrayAddres_L9181_02LT()),
                                            new XrayAddres_L9181_02LT())
                {
                    IsDebugMode = NoComPort,
                };
            });
            servicescollection.AddSingleton<IXrayToPC, XrayToPC>();
            servicescollection.AddSingleton<IPCToXray, PCToXray>();

            using (var service = servicescollection.BuildServiceProvider())
            {
                var cttox = service.GetService<IPCToXray>();
                cttox.EndChangedSts += (s, e) =>
                {
                    PCToXray xss = s as PCToXray;
                    Assert.AreEqual(setvalue, xss.SetTubeVoltage);
                    evcnt++;
                };

                if (NoComPort)
                {
                    var seri = service.GetService<IXraySerialService>();
                    Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));
                    var tstcmd = $"SAR 2 50 30 0 0 0 0{Environment.NewLine}";
                    seri.DoCommand(tstcmd);
                    Task.Run(() => { cttox.DoSetVolate(setvalue); });
                    var dd = $"SVI {setvalue} 30{Environment.NewLine}";
                    seri.DoCommand(dd);
                    var tstcmd2 = $"SAR 2 {setvalue} 30 0 0 0 0{Environment.NewLine}";
                    seri.DoCommand(tstcmd2);
                }
                else
                {
                    cttox.DoSetVolate(setvalue);
                }


                Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));


                if (NoComPort)
                {
                    var seri = service.GetService<IXraySerialService>();
                    Task.Run(() => { cttox.DoSetCurrent(setvalue); });
                    var dd = $"SVI {setvalue} {setvaluecur}{Environment.NewLine}";
                    seri.DoCommand(dd);
                    Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));
                    var tstcmd = $"SAR 2 {setvalue} {setvaluecur} 0 0 0 0{Environment.NewLine}";
                    seri.DoCommand(tstcmd);
                }
                else
                {
                    cttox.DoSetCurrent(setvalue);
                }
                Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(2)));
            };

            Assert.AreEqual(2, evcnt);
        }

        [TestMethod]
        public void X線焦点切替()
        {
            List<string> cmd = new List<string>();
            int evcnt = 0;

            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IXrayAddres_L9181_02LT, XrayAddres_L9181_02LT>();
            servicescollection.AddSingleton<IXrayCheckerFrequency, XrayCheckerFrequency>();
            servicescollection.AddSingleton<IXrayCheckerIrregularity, XrayCheckerIrregularity>();
            servicescollection.AddSingleton<IXraySerialService, XraySerialService>(p =>
            {
                return new XraySerialService(new XrayCheckerFrequency(new XrayAddres_L9181_02LT()),
                                            new XrayCheckerIrregularity(new XrayAddres_L9181_02LT()),
                                            new XrayAddres_L9181_02LT())
                {
                    IsDebugMode = NoComPort,
                };
            });
            servicescollection.AddSingleton<IXrayToPC, XrayToPC>();
            servicescollection.AddSingleton<IPCToXray, PCToXray>();

            using (var service = servicescollection.BuildServiceProvider())
            {
                var cttox = service.GetService<IPCToXray>();
                cttox.EndChangedSts += (s, e) =>
                {
                    PCToXray xss = s as PCToXray;
                    cmd.Add(xss.SFC);
                    evcnt++;
                };

                if (NoComPort)
                {
                    var seri = service.GetService<IXraySerialService>();
                    Task.Run(() => { cttox.DoSetFocus("SMALL"); });
                    var dd1 = $"SFC 0{Environment.NewLine}";
                    seri.DoCommand(dd1);
                    Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));
                    Task.Run(() => { cttox.DoSetFocus("MIDDLE"); });
                    var dd2 = $"SFC 1{Environment.NewLine}";
                    seri.DoCommand(dd2);
                    Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));

                }
                else
                {
                    cttox.DoSetFocus("SMALL");
                    Task.WaitAll(Task.Delay(2000));
                    cttox.DoSetFocus("MIDDLE");
                }
            };

            Assert.AreEqual(cmd.LastOrDefault(), "MIDDLE");
            Assert.AreEqual(2, evcnt);
        }

        [TestMethod]
        public void X線ONとOFF()
        {
            List<string> cmd = new List<string>();
            int evcnt = 0;
            bool DebugMode = NoComPort;

            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IXrayAddres_L9181_02LT, XrayAddres_L9181_02LT>();
            servicescollection.AddSingleton<IXrayCheckerFrequency, XrayCheckerFrequency>();
            servicescollection.AddSingleton<IXrayCheckerIrregularity, XrayCheckerIrregularity>();
            servicescollection.AddSingleton<IXraySerialService, XraySerialService>(p =>
            {
                return new XraySerialService(new XrayCheckerFrequency(new XrayAddres_L9181_02LT()),
                                            new XrayCheckerIrregularity(new XrayAddres_L9181_02LT()),
                                            new XrayAddres_L9181_02LT())
                {
                    IsDebugMode = DebugMode,
                };
            });
            servicescollection.AddSingleton<IXrayToPC, XrayToPC>();
            servicescollection.AddSingleton<IPCToXray, PCToXray>();

            using (var service = servicescollection.BuildServiceProvider())
            {
                var cttox = service.GetService<IPCToXray>();
                cttox.EndXrayOFF += (s, e) =>
                {
                    PCToXray xss = s as PCToXray;
                    cmd.Add(xss.STSProperty.Status);
                    evcnt++;
                };
                cttox.EndXrayON += (s, e) =>
                {
                    PCToXray xss = s as PCToXray;
                    cmd.Add(xss.STSProperty.Status);
                    evcnt++;
                };

                if (NoComPort)
                {
                    var seri = service.GetService<IXraySerialService>();
                    var dd0 = $"SAR 2 50 30 0 0 0 0{Environment.NewLine}";
                    seri.DoCommand(dd0);
                    Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));

                    Task.Run(() => { cttox.DoXrayOn(); });
                    var dd1 = $"SAR 3 50 30 0 0 0 0{Environment.NewLine}";
                    seri.DoCommand(dd1);
                    Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));
                    var dd2 = $"SVI 50 30{Environment.NewLine}";
                    seri.DoCommand(dd2);
                    Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(2)));

                    Task.Run(() => { cttox.DoXrayOFF(); });
                    var dd3 = $"SAR 2 0 0 0 0 0 0{Environment.NewLine}";
                    seri.DoCommand(dd3);
                    var dd4 = $"SVI 0 0{Environment.NewLine}";//管電圧0 管電流0
                    seri.DoCommand(dd4);
                    Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(2)));
                }
                else
                {
                    cttox.DoXrayOn();
                    Task.WaitAll(Task.Delay(2000));
                    cttox.DoXrayOFF();
                }
            };
            Assert.AreEqual("XrayOnReady", cmd.LastOrDefault());
            Assert.AreEqual(2, evcnt);
        }
        [TestMethod]
        public void ウォーミングアップ()
        {
            List<string> cmd = new List<string>();
            int evcnt = 0;
            bool DebugMode = NoComPort;

            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IXrayAddres_L9181_02LT, XrayAddres_L9181_02LT>();
            servicescollection.AddSingleton<IXrayCheckerFrequency, XrayCheckerFrequency>();
            servicescollection.AddSingleton<IXrayCheckerIrregularity, XrayCheckerIrregularity>();
            servicescollection.AddSingleton<IXraySerialService, XraySerialService>(p =>
            {
                return new XraySerialService(new XrayCheckerFrequency(new XrayAddres_L9181_02LT()),
                                            new XrayCheckerIrregularity(new XrayAddres_L9181_02LT()),
                                            new XrayAddres_L9181_02LT())
                {
                    IsDebugMode = NoComPort,
                };
            });
            servicescollection.AddSingleton<IXrayToPC, XrayToPC>();
            servicescollection.AddSingleton<IPCToXray, PCToXray>();

            using (var service = servicescollection.BuildServiceProvider())
            {
                var cttox = service.GetService<IPCToXray>();
                cttox.EndWUP += (s, e) =>
                {
                    PCToXray xss = s as PCToXray;
                    cmd.Add(xss.STSProperty.Status);
                    evcnt++;
                };

                if (NoComPort)
                {
                    var seri = service.GetService<IXraySerialService>();
                    var dd0 = $"SAR 0 130 30 0 0 0 0{Environment.NewLine}";
                    seri.DoCommand(dd0);
                    Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));
                    Task.Run(() => { cttox.DoWUP(); });
                    var dd1 = $"SAR 3 130 30 0 0 0 0{Environment.NewLine}";
                    seri.DoCommand(dd1);
                    Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));

                }
                else
                {
                    cttox.DoWUP();
                }
            };

            Assert.AreNotEqual("NeedWup", cmd.LastOrDefault());
            Assert.AreEqual(1, evcnt);
        }

        [TestMethod]
        public void X線源から状態及び設定値確認_デバック用()
        {
            List<string> cmd = new List<string>();
            int evcnt = 0;
            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IXrayAddres_L9181_02LT, XrayAddres_L9181_02LT>();
            servicescollection.AddSingleton<IXrayCheckerFrequency, XrayCheckerFrequency>();
            servicescollection.AddSingleton<IXrayCheckerIrregularity, XrayCheckerIrregularity>();
            servicescollection.AddSingleton<IXraySerialService, XraySerialService>(p =>
            {
                return new XraySerialService(new XrayCheckerFrequency(new XrayAddres_L9181_02LT()),
                                            new XrayCheckerIrregularity(new XrayAddres_L9181_02LT()),
                                            new XrayAddres_L9181_02LT())
                {
                    IsDebugMode = true,
                };
            });
            servicescollection.AddSingleton<IXrayToPC, XrayToPC>();


            using (var service = servicescollection.BuildServiceProvider())
            {
                var xtopc = service.GetService<IXrayToPC>();
                xtopc.PropertyChanged += (s, e) =>
                {
                    XrayToPC xss = s as XrayToPC;
                    if (!cmd.Contains(e.PropertyName))
                    {
                        cmd.Add(e.PropertyName);
                    }
                    evcnt++;
                };

                xtopc.DoAnalysisSerialCmd($"SAR 3 50 30 0 0 0 0{Environment.NewLine}");
                xtopc.DoAnalysisSerialCmd($"SVI 30 20{Environment.NewLine}");
                xtopc.DoAnalysisSerialCmd($"TYP L9421-02{Environment.NewLine}");
                xtopc.DoAnalysisSerialCmd($"SIN 0{Environment.NewLine}");
                xtopc.DoAnalysisSerialCmd($"SFC 1{Environment.NewLine}");
            };
            Assert.AreEqual(8, cmd.Count);
            Assert.IsTrue(3 < evcnt);
        }
        [TestMethod]
        public void L918102LTパラメータ読込()
        {
            int evcnt = 0;
            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IXrayAddres_L9181_02LT, XrayAddres_L9181_02LT>();

            using (var service = servicescollection.BuildServiceProvider())
            {
                var mcm = service.GetService<IXrayAddres_L9181_02LT>();
                evcnt++;
            }

            Assert.AreEqual(1, evcnt);
        }
        [TestMethod]
        public void L918102LTチェッカー確認()
        {
            List<string> cmd = new List<string>();

            int evcnt = 0;
            ServiceCollection servicescollection = new ServiceCollection();
            servicescollection.AddSingleton<IXrayAddres_L9181_02LT, XrayAddres_L9181_02LT>();
            servicescollection.AddSingleton<IXrayCheckerFrequency, XrayCheckerFrequency>();
            servicescollection.AddSingleton<IXrayCheckerIrregularity, XrayCheckerIrregularity>();

            using (var service = servicescollection.BuildServiceProvider())
            {
                var chk = service.GetService<IXrayCheckerFrequency>();
                chk.DoCheck += (s, e) =>
                {
                    XrayCheckerFrequency xc = s as XrayCheckerFrequency;
                    if (!cmd.Contains(xc.Cmd_XrayToPC))
                    {
                        cmd.Add(xc.Cmd_XrayToPC);
                    }

                    Debug.WriteLine($"時間{DateTime.Now:HHmmss.f},送信CMD {xc.Cmd_XrayToPC}");

                    evcnt++;
                };

                chk.Start();

                Task.WaitAll(Task.Delay(1100));
            }
            Assert.IsTrue(0 < evcnt);
        }
    }
}
