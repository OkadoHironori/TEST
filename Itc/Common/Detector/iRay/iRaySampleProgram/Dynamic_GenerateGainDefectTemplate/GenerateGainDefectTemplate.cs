using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDetector;
using System.Threading;
using System.Runtime.InteropServices;

namespace Dynamic_GenerateGainDefectTemplate
{
    class GenerateGainDefectTemplate
    {
        //static Detector gDetInstance;
        Detector DetInstance;
        int mTotalGroup = 3;
        int mEachGroupFrames;
        int mCurGroupIndex;
        int mStartFrames;
        int[] mExpectedGrays = new int[3] { 2000, 12000, 4000 };
        System.Threading.Timer mMonitorTimer;

        AutoResetEvent mNextStepEvent = new AutoResetEvent(false);
        AutoResetEvent mErrorEvent = new AutoResetEvent(false);
         
        DisplayProgressbar mProgressBar = new DisplayProgressbar();
        static void Main(string[] args)
        {
            new GenerateGainDefectTemplate().Run();
            Console.WriteLine("Press any key to exit!");
            Console.ReadKey();
        }
        public void Run()
        {
            do{
                if (SdkInterface.Err_OK != Initializte())
                {
                    break;
                }
            
                mCurGroupIndex = 0;
                mStartFrames = 0;
                WaitHandle[] waitevents = new WaitHandle[2] { mNextStepEvent, mErrorEvent };

                AttrResult attr = new AttrResult();
                DetInstance.GetAttr(SdkInterface.Attr_DefectTotalFrames, ref attr);
                mEachGroupFrames = attr.nVal / mTotalGroup;


                int result;
                if (SdkInterface.Err_OK != (result = GainInit()))
                {
                    Console.Write("GainInit failed! err={0}\n", DetInstance.GetErrorInfo(result));
                    break;
                }
                Console.Write("Make sure the difference bewteen real-gray-value and expected-gray-value is less than 10%\n");
                while (mCurGroupIndex < mTotalGroup)
                {
                    Console.Write("--------------------[group:{0}, expect gray:{1}]------------------\n", mCurGroupIndex + 1, mExpectedGrays[mCurGroupIndex]);
                    Console.WriteLine("Press [Enter] to start collect light images");
                    Console.ReadKey();
                    StartLightAcqusition();
                    Console.Write("    Acquiring light...\n");
                    Console.WriteLine("Press [Enter] to select valid images");
                    Console.ReadKey();
                    mStartFrames = GetValidFrames();
                    Select4ValidImages(mCurGroupIndex);
                    mMonitorTimer = new Timer(OnTimer, null, 10, 100);
                    int wait = WaitHandle.WaitAny(waitevents);
                    if (1 == wait)
                    {
                        mMonitorTimer.Dispose();
                        Console.Write("Press [Enter] to restart");
                        Console.ReadKey();
                        continue;
                    }
                    Console.WriteLine("Stop expose and press [Enter] to start collect dark images");
                    Console.ReadKey();
                    mStartFrames = GetValidFrames();
                    StartDarkAcqusition();
                    Console.WriteLine("    Acquiring dark...");
                    mMonitorTimer = new Timer(OnTimer, null, 10, 100);
                    mNextStepEvent.WaitOne();
                    mCurGroupIndex += 1;
                };

                Console.WriteLine("Press [Enter] to generte template files");
                Console.ReadKey();
                Console.WriteLine("    Generating ...");
                if (SdkInterface.Err_OK == GenerateTemplateFiles())
                    Console.WriteLine("Generate gain+defect succeed");
                else
                    Console.WriteLine("Generate gain+defect failed");

                FinishGeneration();

            }while(false);

            if (null != DetInstance)
            {
                DetInstance.Dispose();
                DetInstance = null;
            }
            mNextStepEvent.Close();
            mErrorEvent.Close();
        }

        #region transaction
        int GainInit()
        {
            return DetInstance.SyncInvoke(SdkInterface.Cmd_GainInit, 5000);
        }

        // loop------step1
        int StartLightAcqusition()
        {
            return DetInstance.Invoke(SdkInterface.Cmd_StartAcq);
        }

        // loop------step2
        int Select4ValidImages(int groupindex)
        {
            return DetInstance.Invoke(SdkInterface.Cmd_GainSelectAll, groupindex, mEachGroupFrames);
        }
        // loop------step3
        int StartDarkAcqusition()
        {
            return DetInstance.Invoke(SdkInterface.Cmd_ForceDarkContinuousAcq, 0);
        }

        int GenerateTemplateFiles()
        {
            return DetInstance.Invoke(SdkInterface.Cmd_GainGeneration);
        }

        int Abort()
        {
            return DetInstance.Abort();
        }
        int FinishGeneration()
        {
            return DetInstance.SyncInvoke(SdkInterface.Cmd_FinishGenerationProcess, 5000);
        }
        #endregion

        int GetValidFrames()
        {
            AttrResult attr = new AttrResult();
            DetInstance.GetAttr(SdkInterface.Attr_DefectValidFrames, ref attr);
            return attr.nVal;
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
            catch (Exception e)
            {
                Console.Write("\t\t\t[No] - error:{0}\n", e.Message);
                return SdkInterface.Err_Unknown;
            }
        }

        public void OnTimer(object state)
        {
            int nValid = GetValidFrames() - mStartFrames;
            mProgressBar.SetProgress(nValid, mEachGroupFrames / 2);
            if (nValid == mEachGroupFrames / 2)
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
                    if (nParam1 == SdkInterface.Cmd_GainSelectAll)
                    {
                        Console.Write("\nA error happened! error={0}\n", DetInstance.GetErrorInfo(nParam2));

                        mErrorEvent.Set();
                    }
                    break;
                case SdkInterface.Evt_Image:
                    var image = (IRayImage)Marshal.PtrToStructure(pParam, typeof(IRayImage));
                    var vars = new IRayVariantMapItem[image.propList.nItemCount];
			        int nCurGrayValue = 0;
                    SdkParamConvertor<IRayVariantMapItem>.IntPtrToStructArray(image.propList.pItems, ref vars);
                    foreach(IRayVariantMapItem item in vars)
                    {
                        if ((int)Enm_ImageTag.Enm_ImageTag_CenterValue == item.nMapKey)
                        {
                            nCurGrayValue = item.varMapVal.val.nVal;
                            break;
                        }
                    }
                    int diff = mExpectedGrays[mCurGroupIndex] - nCurGrayValue;
			        if (Math.Abs(diff) * 10 > mExpectedGrays[mCurGroupIndex])
			        {
                        //Console.Write("Please ajust dose: [%d] gray-value\n", diff);
			        }
                    break;
                default:
                    break;
            }
        }
    }
}
