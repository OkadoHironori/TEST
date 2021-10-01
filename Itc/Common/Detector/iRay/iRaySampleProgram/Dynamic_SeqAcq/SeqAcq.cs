using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDetector;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

namespace Dynamic_SeqAcq
{
    class SeqAcq
    {
        public int Cntlist { get; private set; } = 1;

        private int CorrectionOpt = 0;

        Detector DetInstance;
        static void Main(string[] args)
        {
            new SeqAcq().Run();
            Console.WriteLine("Press any key to exit!");
            Console.ReadKey();
        }
        public void Run()
        {
            if (SdkInterface.Err_OK == Initializte())
            {
                Console.WriteLine("Notice: Press [Enter] to stop after started");
                Console.WriteLine("Press [Enter] to start acquire");
                Console.ReadKey();
                DetInstance.Invoke(SdkInterface.Cmd_StartAcq);
                Console.ReadKey();
                DetInstance.SyncInvoke(SdkInterface.Cmd_StopAcq, 2000);
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
                ret = DetInstance.SyncInvoke(SdkInterface.Cmd_Connect, 20000);
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
                int correction = (int)(Enm_CorrectOption.Enm_CorrectOp_SW_Gain| Enm_CorrectOption.Enm_CorrectOp_SW_PreOffset  | Enm_CorrectOption.Enm_CorrectOp_SW_Defect);
                ret = DetInstance.SyncInvoke(SdkInterface.Cmd_SetCorrectOption, correction, 5000);
                if (SdkInterface.Err_OK != ret)
                {
                    Console.Write("\t\t[No ]\n");
                    return ret;
                }
                else
                    Console.Write("\t\t[Yes]\n");

                AttrResult attr = new AttrResult();
                DetInstance.GetAttr(SdkInterface.Attr_CurrentCorrectOption, ref attr);

                //string dd = Convert.ToString((int)Enm_CorrectOption.Enm_CorrectOp_SW_Defect,2);
                //int total = dd.Count();
                //foreach()

                //dd.Where(p=>int.Parse(p)==0).Count();




                var pp = Convert.ToString(attr.nVal, 2);
                //AttrResult attr = new AttrResult();
                DetInstance.GetAttr(SdkInterface.Attr_UROM_TriggerMode, ref attr);
                int index = attr.nVal;
                string[] syncMode = { "FreeRun", "SyncIn", "SyncOut", "Unknown" };
                Debug.WriteLine(syncMode[index]);
                DetInstance.GetAttr(SdkInterface.Attr_DefectValidityState, ref attr);
                index = attr.nVal;

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
                case SdkInterface.Evt_Image:
                    Console.Write("Got image\n");
                    {
                        //must make deep copies of pParam
                        IRayImage image = (IRayImage)Marshal.PtrToStructure(pParam, typeof(IRayImage));
                        int img_width = image.nWidth;
                        int img_height = image.nHeight;
                        //short[] img_data = new short[img_width * img_height];
                        //Marshal.Copy(image.pData, img_data, 0, img_data.Length);
                        SaveImage(image.pData, img_width * img_height);


                        var vars = new IRayVariantMapItem[image.propList.nItemCount];
                        SdkParamConvertor<IRayVariantMapItem>.IntPtrToStructArray(image.propList.pItems, ref vars);
                        foreach (IRayVariantMapItem item in vars)
                        {
                            //parse the image frame no
                            if ((int)Enm_ImageTag.Enm_ImageTag_FrameNo == item.nMapKey)
                            {
                                Console.WriteLine("Frame no:{0}", item.varMapVal.val.nVal);
                                break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        //public const int DEFECTMASK = (int)(Enm_CorrectOption.Enm_CorrectOp_SW_Defect | Enm_CorrectOption.Enm_CorrectOp_HW_Defect);

        private void SaveImage(IntPtr imgPtr, int length)
        {
            ushort[] tmp = new ushort[length];
            unsafe
            {
                ushort* p = (ushort*)imgPtr;
                foreach (int idx in Enumerable.Range(0, length)) tmp[idx] = p[idx];
            }

            string workdir = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "work_dir", "NDT0505J"));

            //Debug.WriteLine(DateTime.Today.ToString("hh:mm:ss"));
            string header = Cntlist.ToString("00");

            using (var writer = new BinaryWriter(new FileStream(Path.Combine(workdir, $"{header}_Data.raw"), FileMode.Create)))
            {
                var bytes = BinaryConverter.ConvertUStoB(tmp);
                writer.Write(bytes);
            }

            Cntlist++;
        }
    }
}
