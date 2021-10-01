using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDetector;
using System.Xml;

namespace DownloadCorrectionFile2Device
{
    struct HWCorrectionFileInfo
    {
	    public string filetype;
	    public string index;
	    public string pga;
	    public string binning;
	    public string zoom;
	    public string activity;
	    public string validity;
	    public string despinfo;
    }
    class DownloadCorrectionFile2Device
    {
        Detector DetInstance;
        System.Timers.Timer mMonitorTimer;
        DisplayProgressbar mProgressBar = new DisplayProgressbar();
        List<HWCorrectionFileInfo> mHWFileInfoTable;
        static void Main(string[] args)
        {
            new DownloadCorrectionFile2Device().Run();
            Console.WriteLine("Press any key to exit!");
            Console.ReadKey();
        }
        public void Run()
        {
            mHWFileInfoTable = new List<HWCorrectionFileInfo>();
            if (SdkInterface.Err_OK == Initializte())
            {
                Console.Write("Download gain/defect file\n");
                do
                {

                    Console.Write("Input gain/defect file path:");
                    string path = Console.ReadLine();
                    Enm_FileTypes fileType = ParseFile(path);
                    if (Enm_FileTypes.Enm_File_Gain != fileType && Enm_FileTypes.Enm_File_Defect != fileType)
                    {
                        Console.Write("    The file path is invalid!\n");
                        continue;
                    }
                    Console.Write("Input a location(1~12):");
                    int fileIndex = 0;
                    int.TryParse(Console.ReadLine(), out fileIndex);
                    int ret = DetInstance.Invoke(SdkInterface.Cmd_DownloadCaliFile, (int)fileType, fileIndex, path, "");
                    if (SdkInterface.Err_TaskPending == ret)
                    {
                        mMonitorTimer = new System.Timers.Timer(100);
                        mMonitorTimer.Elapsed += OnTimer;
                        mMonitorTimer.Start();
                        ret = DetInstance.WaitEvent(200 * 1000);
                        if (SdkInterface.Err_OK != ret)
                        {
                            Console.Write("\nDownload failed! error:%s\n", DetInstance.GetErrorInfo(ret));
                        }
                        mProgressBar.SetProgress(DetInstance.GetAttrInt(SdkInterface.Attr_ImageTransProgress), 100);
                        mMonitorTimer.Stop();
                    }
                    else if (SdkInterface.Err_OK != ret)
                    {
                        Console.Write("Download failed! error:%s\n", DetInstance.GetErrorInfo(ret));
                    }
                    Console.Write("\nPress [Enter] to query HW-correction-template list info");
                    Console.Read();
                    ret = DetInstance.Invoke(SdkInterface.Cmd_QueryHwCaliTemplateList, (int)fileType);
                    if (SdkInterface.Err_TaskPending == ret)
                    {
                        DetInstance.WaitEvent(5000);
                    }
                    //DisplayHWFileInfo(mHWFileInfoTable);
                } while (false);
                
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
            catch (Exception e)
            {
                Console.Write("\t\t\t[No] - error:{0}\n", e.Message);
                return SdkInterface.Err_Unknown;
            }
        }
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            int nValid =  DetInstance.GetAttrInt(SdkInterface.Attr_ImageTransProgress);
            mProgressBar.SetProgress(nValid, 100);
            if (nValid == 100)
            {
                mMonitorTimer.Stop();
            }
        }
        Enm_FileTypes ParseFile(string pFilePath)
        {
	        if (string.IsNullOrEmpty(pFilePath))
		        return (Enm_FileTypes)0;

	        string str = pFilePath;
	        if (-1 != pFilePath.IndexOf(".gn"))
	        {
		        return Enm_FileTypes.Enm_File_Gain;
	        }
            else if (-1 != pFilePath.IndexOf(".dft"))
	        {
                return Enm_FileTypes.Enm_File_Defect;
	        }
	        else
		        return (Enm_FileTypes)0;
        }
        List<string> GetNodeList(XmlDocument doc, string path)
        {
            List<string> nodes = new List<string>();
            XmlNodeList nodelist = doc.SelectNodes(path);
            foreach (XmlNode item in nodelist)
            {
                nodes.Add(item.InnerText);
            }
            return nodes;
        }
        void ParseTemplateList(string xmInfo)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmInfo);
            List<string> indexList = GetNodeList(xmlDoc, @"HWCalibrationFileList/FileItem/Index");
            if (null == indexList || 0 == indexList.Count)
            {
                return;
            }
            mHWFileInfoTable.Clear();
            string fileType = xmlDoc.SelectSingleNode(@"HWCalibrationFileList/FileItem/Type").InnerText;
            List<string> pgaList =      GetNodeList(xmlDoc, @"HWCalibrationFileList/FileItem/PGA");
            List<string> binningList =  GetNodeList(xmlDoc, @"HWCalibrationFileList/FileItem/Binning");
            List<string> zoomList =     GetNodeList(xmlDoc, @"HWCalibrationFileList/FileItem/Zoom");
            List<string> activityList = GetNodeList(xmlDoc, @"HWCalibrationFileList/FileItem/Activity");
            List<string> ValidityList = GetNodeList(xmlDoc, @"HWCalibrationFileList/FileItem/Validity");
            List<string> DespInfoList = GetNodeList(xmlDoc, @"HWCalibrationFileList/FileItem/DespInfo");
            for (int index = 0; index < indexList.Count; ++index)
            {
                HWCorrectionFileInfo iteminfo = new HWCorrectionFileInfo();
                if (index < indexList.Count)
                    iteminfo.index = indexList[index];
                if (index < pgaList.Count)
                    iteminfo.pga = pgaList[index];
                if (index < binningList.Count)
                    iteminfo.binning = binningList[index];
                if (index < zoomList.Count)
                    iteminfo.zoom = zoomList[index];
                if (index < activityList.Count)
                    iteminfo.activity = activityList[index];
                if (index < ValidityList.Count)
                    iteminfo.validity = ValidityList[index];
                if (index < DespInfoList.Count)
                    iteminfo.despinfo = DespInfoList[index];

                mHWFileInfoTable.Add(iteminfo);
            }
        }

        void DisplayHWFileInfo(List<HWCorrectionFileInfo> Infotable)
        {
	        if (null == Infotable || 0 == Infotable.Count)
		        return;
	        string []filetype = new string[5] { "", "Offset", "Gain","", "Defect" };
	        Console.Write("---------------HW-correction-[%s] file info------------\n", Infotable[0].filetype);
            Console.Write("index\tPGA\tBining\tZoom\tActivity\tValidity\tDespinfo\n");
            Infotable.ForEach(item => {
                Console.Write("{0}\t{1}\t{2}\t{3}\t{4}\t\t{5}\t\t{6}\n", item.index, item.pga, item.binning,
                            item.zoom, item.activity, item.validity, item.despinfo);
            });
        }

        public void OnSdkCallback(int nEventID, int nEventLevel,
          string strMsg, int nParam1, int nParam2, int nPtrParamLen, IntPtr pParam)
        {
            switch (nEventID)
            {
                case SdkInterface.Evt_HwCaliTemplateList:
                    ParseTemplateList(System.Runtime.InteropServices.Marshal.PtrToStringAuto(pParam));
                    DisplayHWFileInfo(mHWFileInfoTable);
                    break;
                case SdkInterface.Evt_TaskResult_Succeed:
                    if (SdkInterface.Cmd_DownloadCaliFile == nParam1)
                    {
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
