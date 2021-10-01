using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Diagnostics;


namespace CT30K {
	static class Program {
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main() 
        {
            // Mod Start 2018/07/26 M.Oyama
            //// 実行環境が日本語でも英語でもない場合は、言語を英語にする
            ////変更2015/01/16hata
            ////if ((!Thread.CurrentThread.CurrentUICulture.Name.StartsWith("ja")) &&
            ////    (!Thread.CurrentThread.CurrentUICulture.Name.StartsWith("en")))
            ////{
            ////    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en", false);
            ////}
            //if ((!Thread.CurrentThread.CurrentUICulture.Name.StartsWith("ja-JP")) &&
            //    (!Thread.CurrentThread.CurrentUICulture.Name.StartsWith("en-US")))
            //{
            //    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US", false);
            //    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
            //}
            // 実行環境が日本語、英語、中国語でもない場合は、言語を英語にする
            if ((!Thread.CurrentThread.CurrentCulture.Name.StartsWith("ja-JP")) &&
                (!Thread.CurrentThread.CurrentCulture.Name.StartsWith("en-US")) &&
                (!Thread.CurrentThread.CurrentCulture.Name.StartsWith("zh-CN")))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US", false);
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
            }
            // Mod End 2018/07/26

            //追加2015/01/16hata
            //カルチャの設定
            if (Thread.CurrentThread.CurrentUICulture.Name != Thread.CurrentThread.CurrentCulture.Name)
            {
                // Mod Start 2018/08/03 M.Oyama
                //Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentCulture;
                //Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
                if (Thread.CurrentThread.CurrentCulture.Name == "zh-CN")
                {
                    // 中国語の場合はニュートラルカルチャ"zh-Hans"(簡体字中国語)を設定
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-Hans");
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
                }
                else
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentCulture;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
                }
            }

            // 二重起動防止処理
            // Mutex の新しいインスタンスを生成 (Mutex の名前にアセンブリ名を付ける)
            System.Threading.Mutex hMutex = new System.Threading.Mutex(false, Application.ProductName);

            // Mutex のシグナルを受信できるかどうか判断
            if (hMutex.WaitOne(0, false))
            {
                GC.KeepAlive(hMutex);         // hMutex をガベージ コレクション対象から除外する

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // 初期設定
                modCT30K.CT30KMain();

                //Application.Run(new frmCTMenu());
                Application.Run(frmCTMenu.Instance);

                hMutex.ReleaseMutex();        // Mutex を開放する

            }
            else
            {
                //CTプログラムが即に実行されていますので、先に立ち上がっている方を使用してください。
                //MessageBox.Show(ResMessage.Get(IDSNum.IDS_DoubleStart), Application.ExecutablePath,
                //    MessageBoxButtons.OK, MessageBoxIcon.Error);

                MessageBox.Show(CTResources.LoadResString(StringTable.IDS_DoubleStart), Application.ExecutablePath,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            // Mutexを破棄する
            hMutex.Close();

        }
	}
}
