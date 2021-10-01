using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTAddress
{
    /// <summary>
    /// ターゲットファイルを読込みサービス
    /// </summary>
    public class LoadTargetFileService : ILoadTargetFileService
    {
        /// <summary>
        /// ターゲットファイルセット時のイベント
        /// </summary>
        public event EventHandler EndLoadInf;
        /// <summary>
        /// エラーイベント
        /// </summary>
        public event EventHandler ErrorArg;
        /// <summary>
        /// ターゲットファイル
        /// </summary>
        public string TargetFile { get; private set; }
        /// <summary>
        /// システム名
        /// </summary>
        public string SystemName { get; private set; }
        /// <summary>
        /// 1画素サイズ
        /// </summary>
        public string Scale { get; private set; }
        /// <summary>
        /// マトリックス
        /// </summary>
        public string Matrix { get; private set; }
        /// <summary>
        /// 画像フォーマット 1:16bit -32768-32767 0:14bit -8192-8191
        /// </summary>
        public string ImageFormat { get; private set; }
        /// <summary>
        /// テーブル位置 mm
        /// </summary>
        public string TblPosi { get; private set; }
        /// <summary>
        /// ウィンドウレベル
        /// </summary>
        public string WL { get; private set; }
        /// <summary>
        /// ウィンドウ幅
        /// </summary>
        public string WW { get; private set; }
        /// <summary>
        /// ガンマ値
        /// </summary>
        public string Gamma { get; private set; }
        /// <summary>
        /// マイクロCTの付帯情報サービスI/F
        /// </summary>
        private readonly IMicroCTInfService _MicroCTInf;
        /// <summary>
        /// 産業用CTの付帯情報サービスI/F
        /// </summary>
        private readonly IIndustrialCTInfService _IndustrialCTInf;
        /// <summary>
        /// ターゲットファイルを読込みサービス
        /// </summary>
        public LoadTargetFileService(IMicroCTInfService mct, IIndustrialCTInfService inct)
        {
            //マイクロCT
            _MicroCTInf = mct;
            _MicroCTInf.EndLoadInf += (s, e) =>
            {
                if (s is MicroCTInfService mctis)
                {
                    SystemName = mctis.SystemName;
                    Matrix = mctis.Matrix;
                    Scale = mctis.Scale;
                    ImageFormat = mctis.ImageFormat;
                    TblPosi = mctis.ScanTablePosi;

                    WL = mctis.WindowLevel;
                    WW = mctis.WindowWidth;
                    Gamma = mctis.Gamma;
                }
            };

            //産業用CT
            _IndustrialCTInf = inct;
            _IndustrialCTInf.EndLoadInf += (s, e) =>
            {
                if (s is IndustrialCTInfService ictis)
                {
                    SystemName = ictis.SystemName;
                    Matrix = ictis.Matrix;
                    Scale = ictis.Scale;
                    ImageFormat = ictis.ImageFormat;

                    WL = ictis.WindowLevel;
                    WW = ictis.WindowWidth;
                }
            };
        }
        /// <summary>
        /// 付帯情報読込
        /// </summary>
        /// <param name="path"></param>
        public void DoInfLoad(string path)
        {
            var infpath = path;
            var imgpath = path;
            switch (Path.GetExtension(path).ToLower())
            {
                case (".inf"):
                    infpath = Path.ChangeExtension(path, ".inf");
                    imgpath = Path.ChangeExtension(path, ".img");
                    TargetFile = imgpath;
                    break;
                case (".img"):
                    infpath = Path.ChangeExtension(path, ".inf");
                    imgpath = Path.ChangeExtension(path, ".img");
                    TargetFile = imgpath;
                    break;
                default:
                    throw new Exception("Undeveloped Extension File was loaded!");

            }

            if (File.Exists(infpath) && File.Exists(imgpath))
            {
                byte[] bytedata = File.ReadAllBytes(infpath);
                switch (Encoding.Default.GetString(bytedata, 0, 14).TrimEnd('\0'))
                {
                    case ("TOSCANER-30000"):
                        _MicroCTInf.DoInfLoad(bytedata);
                        break;
                    case ("TOSCANER-20000"):
                        _IndustrialCTInf.DoInfLoad(bytedata);
                        break;
                    default:
                        throw new Exception("Unregist File Name");
                }

                EndLoadInf?.Invoke(this, new EventArgs());
            }
            else
            {
                ErrorArg?.Invoke(this, new EventArgs());
            }
        }
    }
    /// <summary>
    /// ターゲットファイルを読込みサービス I/F
    /// </summary>
    public interface ILoadTargetFileService
    {
        /// <summary>
        /// ターゲットファイルセット時のイベント
        /// </summary>
        event EventHandler EndLoadInf;
        /// <summary>
        /// エラーイベント
        /// </summary>
        event EventHandler ErrorArg;
        /// <summary>
        /// 付帯情報読み込み
        /// </summary>
        /// <param name="path"></param>
        void DoInfLoad(string path);
    }
}
