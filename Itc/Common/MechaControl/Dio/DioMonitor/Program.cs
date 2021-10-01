using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xs.DioMonitor
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try            
            {
                bool debugMode = false;
                //bool controlable = false;
                bool controlable = true;

                foreach (string s in args)
                {
                    switch (s)
                    {
                        case "-d":
                            debugMode = false;
                            break;
                        case "-c":
                            controlable = true;
                            break;
                        default:
                            break;
                    }
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1(debugMode, controlable));

            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
