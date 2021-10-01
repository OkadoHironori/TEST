using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CTAddress
{
    /// <summary>
    /// 
    /// </summary>
    public class IndustrialCTInfService: IIndustrialCTInfService
    {
        /// <summary>
        /// 付帯情報ロード完了
        /// </summary>
        public event EventHandler EndLoadInf;
        /// <summary>
        /// システム名
        /// </summary>
        public string SystemName { get; private set; }
        /// <summary>
        /// 当該付帯情報のソフトバージョン
        /// </summary>
        public string SoftVersion { get; private set; }
        /// <summary>
        /// スキャン年月日
        /// </summary>
        public string ScanDate { get; private set; }
        /// <summary>
        /// スキャン時刻
        /// </summary>
        public string ScanTime { get; private set; }
        /// <summary>
        /// 事業所名
        /// </summary>
        public string WorkshopName { get; private set; }
        /// <summary>
        /// コメント
        /// </summary>
        public string Comment { get; private set; }
        /// <summary>
        /// 画像フォーマット 1_16bit -32768-32767 0 or Null 14bit -8192-8191
        /// </summary>
        public string ImageFormat { get; private set; }
        /// <summary>
        /// スキャンモード(ReconMode)
        /// offset full half
        /// </summary>
        public string ScanMode { get; private set; }
        /// <summary>
        /// スキャン位置 //使わない　絶対位置を利用すること
        /// </summary>
        public string ScanPosi { get; private set; }
        /// <summary>
        /// スキャン位置_絶対位置 
        /// </summary>
        public string ScanPosiAbs { get; private set; }
        /// <summary>
        /// マトリクスサイズ
        /// </summary>
        public string Matrix { get; private set; }
        /// <summary>
        /// スライス厚
        /// </summary>
        public string SliceWidth { get; private set; }
        /// <summary>
        /// 管電圧
        /// </summary>
        public string Volt { get; private set; }
        /// <summary>
        /// 管電流
        /// </summary>
        public string Anpere { get; private set; }
        /// <summary>
        /// スキャンスピード(Rapid Normal Fine)
        /// </summary>
        public string SpeedMode { get; private set; }
        /// <summary>
        /// スケール確認
        /// </summary>
        public string Scale { get; private set; }
        /// <summary>
        /// ウィンドウ幅
        /// </summary>
        public string WindowWidth { get; private set; }
        /// <summary>
        /// ウィンドウレベル
        /// </summary>
        public string WindowLevel { get; private set; }
        /// <summary>
        /// CT用アドレスマップ
        /// </summary>
        public IEnumerable<MCTAddress> MCTAddresses { get; private set; }
        /// <summary>
        /// CTアドレスをロードするメソッド　I/F
        /// </summary>
        private readonly ICTAddressLoad _CTAddress;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IndustrialCTInfService(ICTAddressLoad service)
        {
            _CTAddress = service;

            var microCTfilename = Path.Combine(Directory.GetCurrentDirectory(), "TXS","CTAddressTable", "IndustrialCT.csv");
            if (File.Exists(microCTfilename))
            {
                MCTAddresses = _CTAddress.LoadCsvFile(microCTfilename);
            }
            else
            {
                throw new Exception($"{nameof(MicroCTInfService)} is invalid");
            }
        }
        /// <summary>
        /// 付帯情報読込
        /// </summary>
        /// <param name="path"></param>
        public void DoInfLoad(byte[] data)
        {
            foreach (var add in MCTAddresses)
            {
                switch(add.Name)
                {
                    case (nameof(SystemName)):
                        SystemName = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(SoftVersion)):
                        SoftVersion = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(Matrix)):
                        Matrix = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(SpeedMode)):
                        SpeedMode = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(Anpere)):
                        Anpere = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(Volt)):
                        Volt = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(SliceWidth)):
                        SliceWidth = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(Scale)):
                        Scale = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(ImageFormat)):
                        ImageFormat = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(ScanPosi)):
                        ScanPosi = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(ScanPosiAbs)):
                        ScanPosiAbs = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(Comment)):
                        Comment = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(WindowWidth)):
                        WindowWidth = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(WindowLevel)):
                        WindowLevel = _CTAddress.GetDataToString(data, add);
                        break;
                }
            }
            EndLoadInf?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// スライス位置の取得
        /// </summary>
        /// <param name="data"></param>
        public float GetSlicePosi(byte[] data)
                    => float.Parse(
                        _CTAddress.GetDataToString(data, MCTAddresses.ToList().Find(
                                            p => p.Name == nameof(ScanPosiAbs))));
    }
    /// <summary>
    /// 産業用CT付帯情報サービス I/F
    /// </summary>
    public interface IIndustrialCTInfService
    {
        /// <summary>
        /// 付帯情報ロード完了
        /// </summary>
        event EventHandler EndLoadInf;
        /// <summary>
        /// 付帯情報読み込み
        /// </summary>
        /// <param name="path"></param>
        void DoInfLoad(byte[] data);
        /// <summary>
        /// スライス位置の取得
        /// </summary>
        /// <param name="data"></param>
        float GetSlicePosi(byte[] data);
    }
}
