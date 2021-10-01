using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRayControler
{
    /// <summary>
    /// iRay用の環境パス追加クラス
    /// </summary>
    public class IRayConfig: IIRayConfig
    {
        /// <summary>
        /// 環境変数追加完了
        /// </summary>
        public event EventHandler EndAddEvent;
        /// <summary>
        /// メインフォルダ
        /// </summary>
        private readonly string PathDet = "Detector";
        /// <summary>
        /// iDetector用フォルダ
        /// </summary>
        private readonly string PathIDet = "iDetector";
        /// <summary>
        /// Workフォルダ
        /// </summary>
        private readonly string PathWorkDir = "work_dir";
        /// <summary>
        /// NDT0505Jフォルダ
        /// </summary>
        private readonly string PathNDT0505J = "NDT0505J";
        /// <summary>
        /// "NDT0505J"のパス
        /// </summary>
        public string NDT0505JPath { get; private set; }
        /// <summary>
        /// iRay用の環境パス追加クラス
        /// </summary>
        public IRayConfig()
        {
            var fullpath = Path.Combine(Environment.CurrentDirectory, PathDet, PathIDet);
            if (!Directory.Exists(fullpath))
            {
                throw new Exception($"{fullpath} is not exist!");
            }

            NDT0505JPath = Path.Combine(fullpath, PathWorkDir, PathNDT0505J);
            if (!Directory.Exists(NDT0505JPath))
            {
                throw new Exception($"{NDT0505JPath} is not exist!");
            }
        }
        /// <summary>
        /// Config要求
        /// </summary>
        public void RequestConf()
            => EndAddEvent?.Invoke(this, new EventArgs());
    }

    public interface IIRayConfig
    {
        /// <summary>
        /// Configイベント
        /// </summary>
        event EventHandler EndAddEvent;
        /// <summary>
        /// Config要求
        /// </summary>
        void RequestConf();

    }
}
