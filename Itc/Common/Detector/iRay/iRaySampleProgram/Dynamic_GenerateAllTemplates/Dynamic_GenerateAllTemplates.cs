using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using iDetector;

namespace Dynamic_GenerateAllTemplates
{
    class GenerateAllTemplates
    {
        Detector DetInstance;
        AutoResetEvent mNextStepEvent = new AutoResetEvent(false);
        AutoResetEvent mErrorEvent = new AutoResetEvent(false);
        WaitHandle[] waitevents;
        Timer mMonitorTimer;
        int TotalDarkFrames;
        int TotalLightFrames;
        DisplayProgressbar mProgressBar = new DisplayProgressbar();
        static void Main(string[] args)
        {
            new GenerateAllTemplates().Run();
            Console.WriteLine("Press any key to exit!");
            Console.ReadKey();
        }
        ConsoleKey DoSelection(Enm_FileTypes type)
        {
            if (type != Enm_FileTypes.Enm_File_Gain && type != Enm_FileTypes.Enm_File_Defect)
                return ConsoleKey.Q;
            Console.Write("1. Press [Enter] to generate {0} map or\n", type == Enm_FileTypes.Enm_File_Gain ? "gain" : "defect");
            Console.Write("2. Press 'q' to exit\n");
            return Console.ReadKey().Key;
        }
        public void Run()
        {
            do
            {
                if (SdkInterface.Err_OK != Initializte())
                {
                    break;
                }
                DetInstance.SetAttr(SdkInterface.Cfg_CalibrationFlow, 1);//none zero
                waitevents = new WaitHandle[2] { mNextStepEvent, mErrorEvent };
                int ret = InitCalibration();
                if (SdkInterface.Err_OK != ret)
                {
                    Console.Write("InitCalibration failed!err={0}", DetInstance.GetErrorInfo(ret));
                    break;
                }
                Console.Write("Please make sure X-ray had been turned off and Press [Enter] to start collecting dark images\n");
                Console.ReadKey();
                AcquireDarkImages();
                Console.Write("    Generating offset map...");
                ret = GenerateOffsetTemplate();
                if (SdkInterface.Err_OK != ret)
                {
                    Console.Write("Generate offset map failed! err={0}", DetInstance.GetErrorInfo(ret));
                    break;
                }
                else
                    Console.Write("\t[Yes]\n");
                if (ConsoleKey.Q == DoSelection(Enm_FileTypes.Enm_File_Gain))
                    break;
                Console.Write("Please make sure X-ray had been turned on and start collecting light images\n");
                AcquireLightImages();
                Console.Write("    Generating gain map...");
                ret = GenerateGainTemplate();
                if (SdkInterface.Err_OK != ret)
                {
                    Console.Write("Generate gain map failed! err={0}", DetInstance.GetErrorInfo(ret));
                    break;
                }
                else
                    Console.Write("\t[Yes]\n");
                if (ConsoleKey.Q == DoSelection(Enm_FileTypes.Enm_File_Defect))
                    break;
                Console.Write("    Generating defect map...");
                ret = GenerateDefectTemplate();
                if (SdkInterface.Err_OK != ret)
                {
                    Console.Write("Generate defect map failed! err={0}", DetInstance.GetErrorInfo(ret));
                }
                else
                    Console.Write("\t[Yes]\n");
            } while (false);
            FinishCalibration();

            Deinit();
        }
        int Initializte()
        {
            try
            {
                Console.Write("Create instance");
                DetInstance = new Detector();
                int ret = DetInstance.Create(Detector.GetWorkDirPath(), OnSdkCallback);
                if (SdkInterface.Err_OK != ret)
                {
                    Console.Write("\t\t\t[No ] - error:%{0}\n", DetInstance.GetErrorInfo(ret));
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

                Console.Write("Set application-mode");
                ret = DetInstance.SyncInvoke(SdkInterface.Cmd_SetCaliSubset, "Mode1", 5000);
                if (SdkInterface.Err_OK != ret)
                    Console.Write("\t\t[No ]\n");
                else
                    Console.Write("\t\t[Yes]\n");
                return ret;
            }
            catch (Exception e)
            {
                Console.Write("\t\t\t[No] - error:{0}\n", e.Message);
                return SdkInterface.Err_Unknown;
            }
        }

        void Deinit()
        {
            if (null != DetInstance)
            {
                DetInstance.Dispose();
                DetInstance = null;
            }
            mNextStepEvent.Close();
            mErrorEvent.Close();
        }
        public void OnAcquiringDarkTimer(object state)
        {
            int nValid = GetValidDarkFrames();
            mProgressBar.SetProgress(nValid, TotalDarkFrames);
            if (nValid == TotalDarkFrames)
            {
                mMonitorTimer.Dispose();
                Console.Write("\n");
                mNextStepEvent.Set();
            }
        }
        public void OnAcquiringLightTimer(object state)
        {
            int nValid = GetValidLightFrames();
            mProgressBar.SetProgress(nValid, TotalLightFrames);
            if (nValid == TotalLightFrames)
            {
                mMonitorTimer.Dispose();
                Console.Write("\n");
                mNextStepEvent.Set();
            }
        }
        public void OnSdkCallback(int nEventID, int nEventLevel,
            string strMsg, int nParam1, int nParam2, int nPtrParamLen, IntPtr pParam)
        {
            switch (nEventID)
            {
                case SdkInterface.Evt_TaskResult_Failed:
                    if (nParam1 == SdkInterface.Cmd_ForceDarkContinuousAcq)
                    {
                        Console.Write("\nA error happened! error=%s\n", DetInstance.GetErrorInfo(nParam2));
                        mErrorEvent.Set();
                    }
                    break;
                default:
                    break;
            }
        }
        int InitCalibration()
        {
            int ret = DetInstance.SyncInvoke(SdkInterface.Cmd_CalibrationInit, 5000);
            if (SdkInterface.Err_OK == ret)
            {
                TotalDarkFrames = DetInstance.GetAttrInt(SdkInterface.Attr_OffsetTotalFrames);
                TotalLightFrames = DetInstance.GetAttrInt(SdkInterface.Attr_GainTotalFrames);
            }
            return ret;
        }

        int AcquireDarkImages()
        {
            DetInstance.Invoke(SdkInterface.Cmd_ForceDarkContinuousAcq, 0);
            mMonitorTimer = new Timer(OnAcquiringDarkTimer, null, 10, 100);
            int wait = WaitHandle.WaitAny(waitevents);
            if (1 == wait)
            {
                mMonitorTimer.Dispose();
                return SdkInterface.Err_Unknown;
            }
            return SdkInterface.Err_OK;
        }

        int AcquireLightImages()
        {
            DetInstance.Invoke(SdkInterface.Cmd_StartAcq);
            mMonitorTimer = new Timer(OnAcquiringLightTimer, null, 10, 100);
            int wait = WaitHandle.WaitAny(waitevents);
            if (1 == wait)
            {
                mMonitorTimer.Dispose();
                return SdkInterface.Err_Unknown;
            }
            return SdkInterface.Err_OK;
        }

        //Acquire dark images firstly
        int GenerateOffsetTemplate()
        {
            return DetInstance.Invoke(SdkInterface.Cmd_OffsetGeneration);
        }

        //Acquire dark and light images firstly
        int GenerateGainTemplate()
        {
            return DetInstance.Invoke(SdkInterface.Cmd_GainGeneration);
        }

        //Acquire dark and light images firstly
        int GenerateDefectTemplate()
        {
            return DetInstance.Invoke(SdkInterface.Cmd_DefectGeneration);
        }

        int AbortCalibration()
        {
            return DetInstance.Abort();
        }

        void FinishCalibration()
        {
            DetInstance.Invoke(SdkInterface.Cmd_FinishGenerationProcess);
        }

        int GetValidDarkFrames()
        {
            return DetInstance.GetAttrInt(SdkInterface.Attr_OffsetValidFrames);
        }

        int GetValidLightFrames()
        {
            return DetInstance.GetAttrInt(SdkInterface.Attr_GainValidFrames);
        }
    }
}
