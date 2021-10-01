using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDetector;

namespace Dynamic_Initialization
{
    class Initialization
    {
        //static Detector gDetInstance;
        Detector DetInstance;
        static void Main(string[] args)
        {
            new Initialization().Run();
            Console.WriteLine("Press any key to exit!");
            Console.ReadKey();
        }
        public void Run()
        {
            try
            {
                Console.Write("Create instance");
                using (DetInstance = new Detector())
                {
                    int ret = DetInstance.Create(Detector.GetWorkDirPath(), OnSdkCallback);
                    if (SdkInterface.Err_OK != ret)
                    {
                        Console.Write("\t\t\t[No ] - error:{0}\n", DetInstance.GetErrorInfo(ret));
                        return;
                    }
                    else
                        Console.Write("\t\t\t[Yes]\n");
                    Initializte();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed! Err:{0}", e.Message);
            }
        }

        public int Initializte()
        {
            Console.Write("Connect device");
            int ret = DetInstance.SyncInvoke(SdkInterface.Cmd_Connect, 30000);
            if (SdkInterface.Err_OK != ret)
            {
                Console.Write("\t\t\t[No ] - error:{0}\n", DetInstance.GetErrorInfo(ret));
                return ret;
            }
            else
                Console.Write("\t\t\t[Yes]\n");

            //Notice:It's necessary for Mercu-series products, or skip this step
            Console.Write("Set application-mode");
            //"Mode1" defined in DynamicApplicationMode.ini subset=Mode1
            ret = DetInstance.SyncInvoke(SdkInterface.Cmd_SetCaliSubset, "Mode1", 5000);
            if (SdkInterface.Err_OK != ret)
            {
                Console.Write("\t\t[No ]\n");
                return ret;
            }
            else
                Console.Write("\t\t[Yes]\n");

            Console.Write("Set correction option");
            int correction = (int)(Enm_CorrectOption.Enm_CorrectOp_SW_PreOffset | Enm_CorrectOption.Enm_CorrectOp_SW_Gain | Enm_CorrectOption.Enm_CorrectOp_SW_Defect);
            ret = DetInstance.SyncInvoke(SdkInterface.Cmd_SetCorrectOption, correction, 5000);
            if (SdkInterface.Err_OK != ret)
            {
                Console.Write("\t\t[No ]\n");
                return ret;
            }
            else
                Console.Write("\t\t[Yes]\n");

            return SdkInterface.Err_OK;
        }
        public void OnSdkCallback(int nEventID, int nEventLevel,
            string strMsg, int nParam1, int nParam2, int nPtrParamLen, IntPtr pParam)
        {
            
        }
    }
}
