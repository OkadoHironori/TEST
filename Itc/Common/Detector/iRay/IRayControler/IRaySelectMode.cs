using iDetector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IRayControler
{
    /// <summary>
    /// iRayのモード選択クラス
    /// </summary>
    public class IRaySelectMode: IIRaySelectMode
    {
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        [DllImport("kernel32")]
        public static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);
        /// <summary>
        /// モード変更
        /// </summary>
        public event EventHandler ChangeMode;
        /// <summary>
        /// "NDT0505J"のパス
        /// </summary>
        public string NDT0505JPath { get; private set; }
        /// <summary>
        /// 選択しているモード
        /// </summary>
        public string SelectMode { get; private set; }
        /// <summary>
        /// 検出器のWidth
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// 検出器のWidth
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// データ収集モード
        /// </summary>
        public string DataAcqMode { get; private set; }
        /// <summary>
        /// データ収集モード
        /// </summary>
        public string CurrentCorrectMode { get; private set; }
        /// <summary>
        /// オフセット有効状態
        /// </summary>
        public int OffsetValidityState { get; private set; }
        /// <summary>
        /// ゲイン有効状態
        /// </summary>
        public int GainValidityState { get; private set; }
        /// <summary>
        /// 欠陥校正有効状態
        /// </summary>
        public int DefectValidityState { get; private set; }
        /// <summary>
        /// IRay設定I/F
        /// </summary>
        private readonly IIRayConfig _IRayConf;
        /// <summary>
        /// IRayメインクラスI/F
        /// </summary>
        private readonly IIRayDetector _IRayDet;
        /// <summary>
        /// IRayメインクラスI/F
        /// </summary>
        private readonly IIRayCtrl _IRayCtrl;      
        /// <summary>
        /// IRayMode
        /// </summary>
        public IEnumerable<string> IRayMode { get; private set; }
        /// <summary>
        /// アプリケーションモード
        /// </summary>
        public IEnumerable<ApplicatioMode> AppMode { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="irayconf"></param>
        public IRaySelectMode(IIRayConfig irayconf, IIRayDetector iraydet, IIRayCtrl irayctrl)
        {
            _IRayDet = iraydet;
            _IRayConf = irayconf;
            _IRayConf.EndAddEvent += (s, e) => 
            {
                var irc = s as IRayConfig;
                NDT0505JPath = irc.NDT0505JPath;
                ParseApplicationModeInfo(NDT0505JPath);
            };
            _IRayConf.RequestConf();

            _IRayCtrl = irayctrl;
            _IRayCtrl.EndChageMode += (s, e) => 
            {
                GetModeInf();
                ChangeMode?.Invoke(this, new EventArgs());
            };
            _IRayCtrl.ChangeMode("Mode1");


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workdir"></param>
        private void ParseApplicationModeInfo(string workdir)
        {
            string filePath = string.Format("{0}\\DynamicApplicationMode.ini", workdir);

            if(!File.Exists(filePath))throw new Exception($"{filePath} is not exist");


            List<ApplicatioMode> tmpAppModes = new List<ApplicatioMode>();
     
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
                float.TryParse(strTmp.ToString(),out float freq);

                mode.Freq = freq;

                GetPrivateProfileString(section, "subset", null, strTmp, strTmp.Capacity, filePath);
                mode.Subset = strTmp.ToString();

                tmpAppModes.Add(mode);

                ++index;
            } while (true);

            AppMode = tmpAppModes;

            List<string> tmpModelist = new List<string>();
            foreach (var mode in AppMode)
            {
                tmpModelist.Add(mode.Subset);
            }
            IRayMode = tmpModelist;
        }
        /// <summary>
        /// モード設定
        /// </summary>
        /// <param name="mode"></param>
        public void SetMode(string mode)
        {
            _IRayCtrl.ChangeMode(mode);
        }
        /// <summary>
        /// モード要求
        /// </summary>
        public void RequestMode() 
            => ChangeMode?.Invoke(this, new EventArgs());
        /// <summary>
        /// モード情報取得
        /// </summary>
        public void GetModeInf()
        {
            ApplicatioMode mode = new ApplicatioMode();
            AttrResult attr = new AttrResult();
            _IRayDet.GetAttr(SdkInterface.Attr_UROM_PGA, ref attr);
            mode.PGA = attr.nVal;
            _IRayDet.GetAttr(SdkInterface.Attr_UROM_BinningMode, ref attr);
            mode.Binning = attr.nVal;
            _IRayDet.GetAttr(SdkInterface.Attr_UROM_ZoomMode, ref attr);
            mode.Zoom = attr.nVal;
            _IRayDet.GetAttr(SdkInterface.Attr_UROM_SequenceIntervalTime, ref attr);
            mode.Freq = (float)((attr.nVal == 0) ? 0 : 1000.0 / attr.nVal);
            _IRayDet.GetAttr(SdkInterface.Attr_CurrentCorrectOption, ref attr);
            var ddd = attr.nVal;

            CurrentCorrectMode = Convert.ToString(ddd,2);

            string[] dd = ddd.ToString().ToCharArray().Select(c => c.ToString()).ToArray();

            _IRayDet.GetAttr(SdkInterface.Attr_OffsetValidityState, ref attr);
            OffsetValidityState = attr.nVal;

            _IRayDet.GetAttr(SdkInterface.Attr_GainValidityState, ref attr);
            GainValidityState = attr.nVal;

            _IRayDet.GetAttr(SdkInterface.Attr_DefectValidityState, ref attr);
            DefectValidityState = attr.nVal;


            _IRayDet.GetAttr(SdkInterface.Attr_Width, ref attr);
            Width = attr.nVal;

            _IRayDet.GetAttr(SdkInterface.Attr_Height, ref attr);
            Height = attr.nVal;

            _IRayDet.GetAttr(SdkInterface.Attr_UROM_TriggerMode, ref attr);
            int index = attr.nVal;
            string[] syncMode = { "FreeRun", "SyncIn", "SyncOut", "Unknown" };
            DataAcqMode = syncMode[index];
            Debug.WriteLine(syncMode[index]);

            _IRayDet.GetAttr(SdkInterface.Attr_CurrentSubset, ref attr);
            SelectMode = attr.strVal;


            //_IRayDet.GetAttr(SdkInterface.Attr_OffsetValidityState, ref attr);
            //var ddddd = attr.nVal;
        }
    }
    /// <summary>
    /// iRayのモード選択クラス　I/F
    /// </summary>
    public interface IIRaySelectMode
    {
        /// <summary>
        /// モード変更
        /// </summary>
        event EventHandler ChangeMode;
        /// <summary>
        /// モード要求
        /// </summary>
        void RequestMode();
        /// <summary>
        /// モード設定
        /// </summary>
        /// <param name="mode"></param>
        void SetMode(string mode);
        /// <summary>
        /// モード情報取得
        /// </summary>
        void GetModeInf();
    }
    /// <summary>
    /// IRayAppMode
    /// </summary>
    public class ApplicatioMode
    {
        /// <summary>
        /// 番号
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 感度的な 1だと大きい（サリりやすい）
        /// </summary>
        public int PGA { get; set; }
        /// <summary>
        /// ビニング係数
        /// </summary>
        public int Binning { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Zoom { get; set; }
        /// <summary>
        /// FPS 
        /// </summary>
        public float Freq { get; set; }
        /// <summary>
        /// モード名
        /// </summary>
        public string Subset { get; set; }
        /// <summary>
        /// オフセット
        /// </summary>
        public bool OffsetCorrectEnable { get; set; }
        /// <summary>
        /// ゲイン
        /// </summary>
        public bool GainCorrectEnable { get; set; }
        /// <summary>
        /// ゲイン
        /// </summary>
        public bool DefectCorrectEnable { get; set; }
    };
}
