using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDetector;
using System.Runtime.InteropServices;

namespace Dynamic_ChangeAppMode
{
    struct ApplicatioMode
    {
	    public int Index;
	    public int PGA;
	    public int Binning;
	    public int Zoom;
	    public float Freq;
	    public string subset;
    };
    class ChangeAppMode
    {
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        [DllImport("kernel32")]
        public static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

        Detector DetInstance;
        string mWorkDir;
        List<ApplicatioMode> mAppModeList;
        static void Main(string[] args)
        {
            new ChangeAppMode().Run();
            Console.WriteLine("Press any key to exit!");
            Console.ReadKey();
        }
        public void Run()
        {
            if (SdkInterface.Err_OK == Initializte())
            {
                Console.Write("Set application-mode");
                DisplayModeInfo();
                do
		        {
                    Console.Write("\nInput mode(subset) name:");
                    string subset = Console.ReadLine();

			        int ret = DetInstance.SyncInvoke(SdkInterface.Cmd_SetCaliSubset, subset, 5000);
			        if (SdkInterface.Err_OK == ret)
			        {
				        Console.Write("    SetCaliSubset Succeed\n");
				        ApplicatioMode mode = GetAppModeAttr();
				        Console.Write("    [ PGA:{0}, binning:{1}, zoom:{2}, Freq:{3:F1} ]\n", mode.PGA, mode.Binning, mode.Zoom, mode.Freq);
			        }
			        else
				        Console.Write("    SetCaliSubset Failed\n");

			        Console.Write("Input [q] to exit or other key to continue:");
			        if (ConsoleKey.Q == Console.ReadKey().Key)
				        break;
		        } while (true);
            }
            if (null != DetInstance)
            {
                DetInstance.Dispose();
                DetInstance = null;
            }
        }
        public int Initializte()
        {
            mAppModeList = new List<ApplicatioMode>();
            mWorkDir = Detector.GetWorkDirPath();
            try
            {
                Console.Write("Create instance");
                DetInstance = new Detector();
                int ret = DetInstance.Create(mWorkDir, OnSdkCallback);
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
            catch (Exception e)
            {
                Console.Write("\t\t\t[No] - error:{0}\n", e.Message);
                return SdkInterface.Err_Unknown;
            }
        }
        void ParseApplicationModeInfo()
        {
            string filePath = string.Format("{0}\\DynamicApplicationMode.ini", mWorkDir);
            string section;
            StringBuilder strTmp = new StringBuilder(128);
            int index = 1;
            do
            {
                ApplicatioMode mode = new ApplicatioMode();
                section = string.Format("ApplicationMode{0}", index);
                mode.Index = index;
                mode.PGA = (int)GetPrivateProfileInt(section, "PGA", -1, filePath);
                if (-1 == mode.PGA)
                    break;
                mode.Binning = (int)GetPrivateProfileInt(section, "Binning", 0, filePath);
                mode.Zoom = (int)GetPrivateProfileInt(section, "Zoom", 0, filePath);
                GetPrivateProfileString(section, "Frequency", null, strTmp, strTmp.Capacity, filePath);
                float.TryParse(strTmp.ToString(), out mode.Freq);
                GetPrivateProfileString(section, "subset", null, strTmp, strTmp.Capacity, filePath);
                mode.subset = strTmp.ToString();
                mAppModeList.Add(mode);
                ++index;
            } while (true);
        }
        void DisplayModeInfo()
        {
            ParseApplicationModeInfo();
	        Console.Write("Index\tPGA\tBinning\t  Zoom\tFrequency\tSubset\n");
            mAppModeList.ForEach(item=>
            {
                Console.Write("{0}\t{1}\t{2}\t  {3}\t{4:F1}\t\t{5}\n", item.Index, item.PGA, item.Binning, item.Zoom, item.Freq, item.subset);
            });
        }
        ApplicatioMode GetAppModeAttr()
        {
            ApplicatioMode mode = new ApplicatioMode();
            AttrResult attr = new AttrResult();
            DetInstance.GetAttr(SdkInterface.Attr_UROM_PGA, ref attr);
            mode.PGA = attr.nVal;
            DetInstance.GetAttr(SdkInterface.Attr_UROM_BinningMode, ref attr);
            mode.Binning = attr.nVal;
            DetInstance.GetAttr(SdkInterface.Attr_UROM_ZoomMode, ref attr);
            mode.Zoom = attr.nVal;
            DetInstance.GetAttr(SdkInterface.Attr_UROM_SequenceIntervalTime, ref attr);
            mode.Freq = (float)((attr.nVal == 0) ? 0 : 1000.0 / attr.nVal);
            return mode;
        }
        public void OnSdkCallback(int nEventID, int nEventLevel,
          string strMsg, int nParam1, int nParam2, int nPtrParamLen, IntPtr pParam)
        {
            switch (nEventID)
            {
                case SdkInterface.Evt_Exp_Enable:
                    break;
                case SdkInterface.Evt_Image:
                    break;
                default:
                    break;
            }
        }


        //----------------------------------------------------
        void SwitchApplicationMode_HWGainDefectCorrection()
        {
            int ret = DetInstance.SyncInvoke(SdkInterface.Cmd_SetCaliSubset, "Mode1", 5000);
            if (SdkInterface.Err_OK != ret)
                return;
            int file_index = 1;//defined by downloadCorrectionFile2Device
            ret = DetInstance.SyncInvoke(SdkInterface.Cmd_SelectCaliFile, (int)Enm_FileTypes.Enm_File_Gain, file_index);
            if (SdkInterface.Err_OK != ret)
                return;
            ret = DetInstance.SyncInvoke(SdkInterface.Cmd_SelectCaliFile, (int)Enm_FileTypes.Enm_File_Defect, file_index);
            if (SdkInterface.Err_OK != ret)
                return;
        }
        void SwitchApplicationMode_SWGainDefectCorrection()
        {
            int ret = DetInstance.SyncInvoke(SdkInterface.Cmd_SetCaliSubset, "Mode1", 5000);
            if (SdkInterface.Err_OK != ret)
                return;
        }
    }
}
