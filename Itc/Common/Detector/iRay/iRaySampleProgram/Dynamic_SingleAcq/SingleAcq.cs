using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDetector;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

namespace Dynamic_SingleAcq
{
    class SingleAcq
    {
        Detector DetInstance;
        System.Timers.Timer ExpWindowTimer;
        int ExpWindowSeconds;
        static void Main(string[] args)
        {
            new SingleAcq().Run();
            Console.WriteLine("Press any key to exit!");
            Console.ReadKey();
        }
        public void Run()
        {
            if (SdkInterface.Err_OK == Initializte())
            {
                Console.Write("Press [Enter] to start single-acquire\n");
                Console.ReadKey();
                Console.Write("Set to SyncOut mode\n");
                DetInstance.SetAttr(SdkInterface.Attr_UROM_FluroSync_W, (int)Enm_FluroSync.Enm_FluroSync_SyncOut);

                //Console.Write("Set to SyncOut mode\n");
                //DetInstance.SetAttr(SdkInterface.Attr_UROM_FluroSync_W, (int)Enm_FluroSync.Enm_FluroSync_FreeRun);

                DetInstance.SyncInvoke(SdkInterface.Cmd_WriteUserROM, 4000);
                DetInstance.SyncInvoke(SdkInterface.Cmd_ClearAcq, 10000);
            }
            System.Threading.Thread.Sleep(100);//Wait image..
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

                return SdkInterface.Err_OK;
            }
            catch(Exception e)
            {
                Console.Write("\t\t\t[No] - error:{0}\n", e.Message);
                return SdkInterface.Err_Unknown;
            }
        }
        public void OnTimer (object sender, System.Timers.ElapsedEventArgs e)
        {
            if (1 == ExpWindowTimer.Interval)
                ExpWindowTimer.Interval = 1000;
            Console.Write(string.Format("Please expose in: {0}s\r", ExpWindowSeconds--));
        }
        public void OnSdkCallback(int nEventID, int nEventLevel,
          string strMsg, int nParam1, int nParam2, int nPtrParamLen, IntPtr pParam)
        {
            switch (nEventID)
	        {
	        case SdkInterface.Evt_Exp_Enable:
		        Console.Write("Prepare to expose\n");
                ExpWindowSeconds = nParam1/1000;
                ExpWindowTimer = new System.Timers.Timer(1);
                ExpWindowTimer.Elapsed += OnTimer;
                ExpWindowTimer.Start();
		        break;
	        case SdkInterface.Evt_Image:
                if (null != ExpWindowTimer)
                    ExpWindowTimer.Close();
		        Console.Write("\nGot image\n");
		        {
			        //must make deep copies of pParam
			        IRayImage image = (IRayImage)Marshal.PtrToStructure(pParam, typeof(IRayImage));
                        int img_width = image.nWidth;
                        int img_height = image.nHeight;

                        SaveImage(image.pData, img_width * img_height);

                        short[] img_data = new short[img_width * img_height];
                        Marshal.Copy(image.pData, img_data, 0, img_data.Length);
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
            //string timename = DateTime.Today.ToString("hh:mm:ss");

            using (var writer = new BinaryWriter(new FileStream(Path.Combine(workdir, $"Data.raw"), FileMode.Create)))
            {
                var bytes = BinaryConverter.ConvertUStoB(tmp);
                writer.Write(bytes);
            }
        }
    }
}
