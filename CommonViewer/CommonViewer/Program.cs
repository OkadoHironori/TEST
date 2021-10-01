using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CommonViewer
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //Rev20.00 変更 by長野 2015/01/30
            Application.Run(new frmMain());
        }
    }
}
