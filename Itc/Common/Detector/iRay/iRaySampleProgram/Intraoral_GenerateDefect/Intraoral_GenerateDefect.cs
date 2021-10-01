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
        int mFrameIndex;
        int[] mExpectedGrays = new int[4] { 300, 500, 1000, 2000 };

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
            
                mFrameIndex = 0;
                WaitHandle[] waitevents = new WaitHandle[2] { mNextStepEvent, mErrorEvent };

                AttrResult attr = new AttrResult();
                DetInstance.GetAttr(SdkInterface.Attr_DefectTotalFrames, ref attr);
                int TotalFrames = attr.nVal;
                DetInstance.GetAttr(SdkInterface.Attr_UROM_TriggerMode, ref attr);
                Enm_TriggerMode triggermode = (Enm_TriggerMode)attr.nVal;
                int result;
                if (SdkInterface.Err_OK != (result = DefectInit()))
                {
                    Console.Write("DefectInit failed! err={0}\n", DetInstance.GetErrorInfo(result));
                    break;
                }
                Console.Write("Make sure the difference bewteen real-gray-value and expected-gray-value is less than 10%\n");
                while (mFrameIndex < TotalFrames)
                {
                    Console.Write("--------------------[group:{0}, expect gray:{1}]------------------\n", mFrameIndex + 1, mExpectedGrays[Math.Min(3, mFrameIndex)]);
                    if (triggermode == Enm_TriggerMode.Enm_TriggerMode_FreeSync)
                    {
                        AcquireImageInOutMode();
                    }
                    else
                    {
                        AcquireImageInSoftwareMode();
                    }

                    int wait = WaitHandle.WaitAny(waitevents);
                    if (1 == wait)
                    {
                        Console.Write("Press [Enter] to restart");
                        Console.ReadKey();
                        continue;
                    }
                    result = Select4ValidImages(mFrameIndex);
                    if (SdkInterface.Err_OK != result)
                    {
                        Console.Write("Select failed! error={0}\n", DetInstance.GetErrorInfo(result));
                        continue;
                    }
                    mFrameIndex += 1;
                    Console.Write("Progress:{0}/{1}\n", mFrameIndex, TotalFrames);
                };

                Console.WriteLine("Press [Enter] to generte template files");
                Console.ReadKey();
                Console.WriteLine("    Generating ...");
                if (SdkInterface.Err_OK == GenerateTemplateFiles())
                    Console.WriteLine("Generate defect succeed");
                else
                    Console.WriteLine("Generate defect failed");

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
        bool AcquireImageInSoftwareMode()
        {
            Console.WriteLine("Press [Enter] to acquiring image");
            Console.ReadKey();
            StartLightAcqusition();
            return true;
        }

        bool AcquireImageInOutMode()
        {
            Console.WriteLine("Please expose to acquiring image\n");
            return true;
        }
        int DefectInit()
        {
            return DetInstance.SyncInvoke(SdkInterface.Cmd_DefectInit, 5000);
        }

        // loop------step1
        int StartLightAcqusition()
        {
            return DetInstance.Invoke(SdkInterface.Cmd_ClearAcq);
        }

        // loop------step2
        int Select4ValidImages(int frameindex)
        {
            return DetInstance.SyncInvoke(SdkInterface.Cmd_DefectSelectCurrent, frameindex, 5000);
        }

        int GenerateTemplateFiles()
        {
            return DetInstance.SyncInvoke(SdkInterface.Cmd_DefectGeneration, 10000);
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

                return SdkInterface.Err_OK;
            }
            catch (Exception e)
            {
                Console.Write("\t\t\t[No] - error:{0}\n", e.Message);
                return SdkInterface.Err_Unknown;
            }
        }

        public void OnSdkCallback(int nEventID, int nEventLevel,
            string strMsg, int nParam1, int nParam2, int nPtrParamLen, IntPtr pParam)
        {
            switch (nEventID)
            {
                case SdkInterface.Evt_TaskResult_Failed:
                    if (nParam1 == SdkInterface.Cmd_StartAcq || nParam1 == SdkInterface.Cmd_ClearAcq)
                    {
                        Console.Write("\nA error happened! error={0}\n", DetInstance.GetErrorInfo(nParam2));
                        mErrorEvent.Set();
                    }
                    break;
                case SdkInterface.Evt_Image:
                    var image = (IRayImage)Marshal.PtrToStructure(pParam, typeof(IRayImage));
                    int img_width = image.nWidth;
                    int img_height = image.nHeight;
                    short[] img_data = new short[img_width * img_height ];
                    Marshal.Copy(image.pData, img_data, 0, img_data.Length);
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
                    int diff = mExpectedGrays[mFrameIndex] - nCurGrayValue;
                    if (Math.Abs(diff) * 10 > mExpectedGrays[Math.Min(3, mFrameIndex)])
			        {
                        //Console.Write("Please ajust dose: [%d] gray-value\n", diff);
			        }
                    Console.Write("Got image\n");
                    mNextStepEvent.Set();
                    break;
                case SdkInterface.Evt_AutoTask_Started:
                    if (SdkInterface.Cmd_StartAcq == nParam1)
                    {
                        Console.Write("Acquiring image...\n");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
