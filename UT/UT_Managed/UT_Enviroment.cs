using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using IRayControler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UT_Managed
{
    [TestClass]
    public class UT_Enviroment
    {
        /// <summary>
        /// サービス登録
        /// </summary>
        /// <returns></returns>
        private static ServiceCollection GetService()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IIRayConfig, IRayConfig>();
            services.AddSingleton<IIRayCtrl, IRayCtrl>();

            return services;
        }
        [TestMethod]
        public void Detectorクラスに環境変数パス()
        {
            int testcnt = 0;

            var path = Environment.GetEnvironmentVariable("Path");
            int precont = path.ToString().Split(Path.PathSeparator).Length;

            ServiceCollection serviceCollection = GetService();
            using (var service = serviceCollection.BuildServiceProvider())
            {
                var addservice = service.GetService<IIRayConfig>();
                addservice.EndAddEvent += (s, e) =>
                {
                    testcnt++;
                };
                addservice.RequestConf();
            }

            path = Environment.GetEnvironmentVariable("Path");
            int aftcont = path.ToString().Split(Path.PathSeparator).Length;

            Assert.IsTrue(aftcont> precont);

            Assert.AreEqual(1, testcnt);
        }
        [TestMethod]
        public void 環境変数パスに追加する()
        {

            // 大文字小文字の違いを無視するDictionaryを作成
            Dictionary<string, string> envvars = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // 環境変数を取得してDictionaryにコピー
            foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables())
            {
                envvars.Add((string)entry.Key, (string)entry.Value);
            }

            //// すべての環境変数を取得
            //IDictionary envvars = Environment.GetEnvironmentVariables();

            //// 環境変数 'PATH' を取得・表示 (大文字小文字の違いがチェックされる点に注意)
            //Debug.WriteLine("PATH={0}", envvars["PATH"]);
            //Debug.WriteLine("Path={0}", envvars["Path"]);

            //Debug.WriteLine();

            // すべての環境変数を列挙して表示
            foreach (var entry in envvars)
            {
                Debug.WriteLine("{0}=>{1}", entry.Key, entry.Value);
            }

            // カレントディレクトリのフルパスを取得する
            var currentdir = Path.GetFullPath(Environment.CurrentDirectory);

            // 環境変数 'PATH' にcurrentdirを追加する
            var path = Environment.GetEnvironmentVariable("Path");


            var testnum = path.ToString().Split(Path.PathSeparator);

            Environment.SetEnvironmentVariable("Path", path + Path.PathSeparator + currentdir+"AA.exe");

            
            // 大文字小文字の違いを無視するDictionaryを作成
            envvars = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // 環境変数を取得してDictionaryにコピー
            foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables())
            {
                envvars.Add((string)entry.Key, (string)entry.Value);
            }

            path = Environment.GetEnvironmentVariable("Path");


            testnum = path.ToString().Split(';');



        }
    }
}
