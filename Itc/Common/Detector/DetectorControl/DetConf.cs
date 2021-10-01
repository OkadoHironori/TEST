using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DetectorControl
{
    /// <summary>
    /// 検出器設定
    /// </summary>
    public class DetConf: IDetConf
    {
        /// <summary>
        /// データ読込完了イベント
        /// </summary>
        public event EventHandler EndLoadParam;
        /// <summary>
        /// パス名1
        /// </summary>
        private readonly string PathDet1 = "Detector";
        /// <summary>
        /// パス名2
        /// </summary>
        private readonly string PathDet2 = "DetectorParam";
        /// <summary>
        /// CSV名
        /// </summary>
        private readonly string FileCSV = "DetectorParam.csv";
        /// <summary>
        /// CW90回転
        /// </summary>
        public bool CW90 { get; private set; }
        /// <summary>
        /// CCW90回転
        /// </summary>
        public bool CCW90 { get; private set; }
        /// <summary>
        /// 左右反転
        /// </summary>
        public bool FlipHorizontal { get; private set; }
        /// <summary>
        /// 上下反転
        /// </summary>
        public bool FlipVertical { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DetConf()
        {
            var fullcsvpath = Path.Combine(Environment.CurrentDirectory, PathDet1, PathDet2, FileCSV);
            if (!File.Exists(fullcsvpath))
            {
                throw new Exception($"{fullcsvpath} is not exist!");
            }

            //CSVファイル読込 Tuple
            List<Tuple<string, string>> fobj = File.ReadAllLines(fullcsvpath)
                    .Where(l => !string.IsNullOrEmpty(l))
                    .Select(v => v.Split(','))
                    .Where(j => !string.IsNullOrEmpty(j[0]) && Regex.IsMatch(j[1], "^(?i)(true|false)$"))
                    .Select(c => new Tuple<string, string>(c[0], c[1])).ToList();

            foreach (var add in fobj)
            {
                switch (add.Item1)
                {
                    case (nameof(CW90)):
                        CW90 = bool.Parse(add.Item2);
                        break;
                    case (nameof(CCW90)):
                        CCW90 = bool.Parse(add.Item2);
                        break;
                    case (nameof(FlipHorizontal)):
                        FlipHorizontal = bool.Parse(add.Item2);
                        break;
                    case (nameof(FlipVertical)):
                        FlipVertical = bool.Parse(add.Item2);
                        break;
                }
            }

            if(CW90&& CCW90)
            {
                throw new Exception($"{fullcsvpath}s setting is uncorrect!{Environment.NewLine}Check {fullcsvpath}s setting");
            }
        }
        /// <summary>
        /// データ要求
        /// </summary>
        public void RequestParam()
            => EndLoadParam?.Invoke(this, new EventArgs());
    }
    /// <summary>
    /// 検出器設定
    /// </summary>
    public interface IDetConf
    {
        /// <summary>
        /// データ要求
        /// </summary>
        void RequestParam();
        /// <summary>
        /// データ読込完了イベント
        /// </summary>
        event EventHandler EndLoadParam;
    }
}
