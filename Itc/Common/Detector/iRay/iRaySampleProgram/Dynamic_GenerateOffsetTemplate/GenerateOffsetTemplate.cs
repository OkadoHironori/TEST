//#define SHOW_PROGRESS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDetector;
using System.Threading;

namespace Dynamic_GenerateOffsetTemplate
{
    class GenerateOffsetTemplate
    {
        //static Detector gDetInstance;
        Detector DetInstance;
        int mTotalNumber;
#if SHOW_PROGRESS
        System.Timers.Timer mMonitorTimer;
#endif
        DisplayProgressbar mProgressBar = new DisplayProgressbar();
        static void Main(string[] args)
        {
            new GenerateOffsetTemplate().Run();
            Console.WriteLine("Press any key to exit!");
            Console.ReadKey();
        }
        public void Run()
        {
            if (SdkInterface.Err_OK == Initializte())
            {
                AttrResult attr = new AttrResult();
                DetInstance.GetAttr(SdkInterface.Attr_OffsetTotalFrames, ref attr);
                mTotalNumber = attr.nVal;
                DetInstance.GetAttr(SdkInterface.Attr_UROM_SequenceIntervalTime, ref attr);
                int intervalTimeOfEachFrame = attr.nVal;
                int timeout = mTotalNumber * intervalTimeOfEachFrame + 5000;
                Console.Write("Generate offset...\n");
#if SHOW_PROGRESS
                int ret = DetInstance.Invoke(SdkInterface.Cmd_OffsetGeneration);
                if (ret == SdkInterface.Err_TaskPending)
                {
                    mMonitorTimer = new System.Timers.Timer(100);
                    mMonitorTimer.Elapsed += OnTimer;
                    mMonitorTimer.Start();
                    DetInstance.WaitEvent(timeout);
                }
#else
                int ret = DetInstance.SyncInvoke(SdkInterface.Cmd_OffsetGeneration, timeout);
#endif
            }
            if (null != DetInstance)
            {
                DetInstance.Dispose();
                DetInstance = null;
            }
        }

        public int Initializte()
        {
            try
            {
                Console.Write("Create instance");
                DetInstance = new Detector();
                int ret = DetInstance.Create(Detector.GetWorkDirPath(), OnSdkCallback);
                if (SdkInterface.Err_OK != ret)
                {
                    Console.Write("\t\t\t[No ] - error:{0}\n", DetInstance.GetErrorInfo(ret));
                    return ret;
                }
                else
                    Console.Write("\t\t\t[Yes]\n");
                Console.Write("Connect device");
                ret = DetInstance.SyncInvoke(SdkInterface.Cmd_Connect, 30000);
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

                return SdkInterface.Err_OK;
            }
            catch(Exception e)
            {
                Console.Write("\t\t\t[No] - error:{0}\n", e.Message);
                return SdkInterface.Err_Unknown;
            }
        }
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            AttrResult attr = new AttrResult();
            DetInstance.GetAttr(SdkInterface.Attr_OffsetValidFrames, ref attr);
            int nValid = attr.nVal;
            mProgressBar.SetProgress(nValid, mTotalNumber);
            if (nValid == mTotalNumber)
            {
#if SHOW_PROGRESS
                mMonitorTimer.Close();
#endif
                Console.Write("\n");
            }
        }
        public void OnSdkCallback(int nEventID, int nEventLevel,
            string strMsg, int nParam1, int nParam2, int nPtrParamLen, IntPtr pParam)
        {
            switch (nEventID)
            {
                case SdkInterface.Evt_TaskResult_Succeed:
                case SdkInterface.Evt_TaskResult_Failed:
                    if (nParam1 == SdkInterface.Cmd_OffsetGeneration)
                    {
                        Console.WriteLine("Offset template generated - {0}", DetInstance.GetErrorInfo(nParam2));
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
