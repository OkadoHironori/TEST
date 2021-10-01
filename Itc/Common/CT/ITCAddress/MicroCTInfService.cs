using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CTAddress
{
    /// <summary>
    /// マイクロCTの付帯情報サービスクラス
    /// </summary>
    public class MicroCTInfService : IMicroCTInfService
    {
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
        /// 画像フォーマット 1:16bit -32768-32767 0:14bit -8192-8191
        /// </summary>
        public string ImageFormat { get; private set; }
        /// <summary>
        /// スキャンモード(ReconMode)
        /// offset full half
        /// </summary>
        public string ScanMode { get; private set; }
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
        /// スキャン位置
        /// </summary>
        public string ScanPosi { get; private set; }
        /// <summary>
        /// スキャン位置
        /// </summary>
        public string ScanPosiAbs { get; private set; }
        /// <summary>
        /// FOVを変更したか?
        /// </summary>
        public bool IsFovConvImage { get; private set; }
        /// <summary>
        /// フィルター関数
        /// </summary>
        public string Filter { get; private set; }
        /// <summary>
        /// オリジナルのスキャンエリア(ズーミング用)
        /// </summary>
        public float ScanAreaTblDia { get; private set; }
        /// <summary>
        /// スキャンエリア(mm)
        /// </summary>
        public string ScanArea { get; private set; }
        /// <summary>
        /// ウィンドウ幅
        /// 産業用CTにも対応
        /// </summary>
        public string WindowWidth { get; private set; }
        /// <summary>
        /// ウィンドウレベル
        /// 産業用CTにも対応
        /// </summary>
        public string WindowLevel { get; private set; }
        /// <summary>
        /// 焦点（産業用ＣＴ）
        /// </summary>
        public string Focus { get; private set; }
        /// <summary>
        /// X線源タイプ
        /// </summary>
        public string XrayGenType { get; private set; }
        /// <summary>
        /// スケール
        /// </summary>
        public string Scale { get; private set; }
        /// <summary>
        /// ビュー数（マイクロＣＴ）表示するビュー数
        /// </summary>
        public string ScanView { get; private set; }
        /// <summary>
        /// 積算枚数（マイクロＣＴ）
        /// </summary>
        public string IntegNumber { get; private set; }
        /// <summary>
        /// I.I.視野（マイクロＣＴ）
        /// </summary>
        public string IIField { get; private set; }
        /// <summary>
        /// コーンビーム（マイクロＣＴ）
        /// </summary>
        public string Conebeam { get; private set; }
        /// <summary>
        /// 断面像方向（マイクロＣＴ）
        /// </summary>
        public string ImageDirection { get; private set; }
        /// <summary>
        /// 画像バイアス（マイクロＣＴ）
        /// </summary>
        public string ImageBias { get; private set; }
        /// <summary>
        /// 画像スロープ（マイクロＣＴ）
        /// </summary>
        public string ImageSlope { get; private set; }
        /// <summary>
        /// FDD（マイクロＣＴ）
        /// </summary>
        public string Fdd { get; private set; }
        /// <summary>
        /// ＦＣＤ（マイクロＣＴ）
        /// </summary>
        public string Fcd { get; private set; }
        /// <summary>
        /// フィルタ処理の方法 
        /// </summary>
        public string FilterProcess { get; private set; }
        /// <summary>
        /// 検出器種類（マイクロＣＴ）
        /// I.I. = 0  FPD = 2 (1と3)は不明 
        /// </summary>
        public string Detector { get; private set; }
        /// <summary>
        /// データ収集時間(秒)（マイクロＣＴ）
        /// </summary>
        public string DataAcqTime { get; private set; }
        /// <summary>
        /// 再構成時間(秒)（マイクロＣＴ）
        /// </summary>
        public string ReconTime { get; private set; }
        /// <summary>
        /// RFC用文字(リングノイズ低減)（マイクロＣＴ） 
        /// </summary>
        public string RfcChar { get; private set; }
        /// <summary>
        /// テーブル Y軸　
        /// </summary>
        public string TableYCood { get; private set; }
        /// <summary>
        /// FPDゲイン (マイクロＣＴ) 値(pF) //v2.1.3追加 by長野 2011/06/24
        /// </summary>
        public string FpdGain_f { get; private set; }
        /// <summary>
        /// FPD積分時間 (マイクロＣＴ) 値(msec) //v2.1.3追加 by長野 2011/06/24
        /// </summary>
        public string FpdIntegNum_f { get; private set; }
        /// <summary>
        /// ガンマ補正値 (マイクロＣＴ) //v2.1.7追加 by長野 2011/04/04
        /// </summary>
        public string Gamma { get; private set; }
        /// <summary>
        /// BHC (マイクロＣＴ) //v2.9.0追加 by長野 2017/03/13
        /// </summary>
        public string Mbhc_flag { get; private set; }
        /// <summary>
        /// BHCテーブル ディレクトリ名 (マイクロＣＴ) //v2.1.7追加 by長野 2012/05/02
        /// </summary>
        public string Mbhc_dir { get; private set; }
        /// <summary>
        /// BHCテーブル ファイル名 (マイクロＣＴ) //v2.1.7追加 by長野 2012/05/02
        /// </summary>
        public string Mbhc_name { get; private set; }
        /// <summary>
        /// ファントムレスBHCフラグ(マイクロＣＴ) //v2.9.0追加 by長野 2017/03/13
        /// </summary>
        public string Phantomelss_mbhc { get; private set; }
        /// <summary>
        /// ファントムレスBHC材質名(マイクロＣＴ) //v2.9.0追加 by長野 2017/03/13
        /// </summary>
        public string Phantomelss_mbhc_name { get; private set; }
        /// <summary>
        /// アーチファクト低減（マイクロＣＴ）
        /// </summary>
        public string ArtifactReduction { get; private set; }
        /// <summary>
        /// 回転方法　連続 or ステップ　（マイクロＣＴ）
        /// 0:ステップ、1:連続
        /// </summary>
        public string TableRotationMode { get; private set; }
        /// <summary>
        /// 回転方向　CW or CCW　（マイクロＣＴ）
        /// </summary>
        public string TableCCWCWMode { get; private set; }
        /// <summary>
        /// 画像回転角度　（三世代）
        /// </summary>
        public string ReconStartAngle { get; private set; }
        /// <summary>
        /// 微調テーブルX軸
        /// </summary>
        public string ADTableY { get; private set; }
        /// <summary>
        /// 微調テーブルX軸
        /// </summary>
        public string ADTableX { get; private set; }
        /// <summary>
        /// ズーミング
        /// </summary>
        public string Zooming { get; private set; }
        /// <summary>
        /// 微調テーブルあり・なし
        /// </summary>
        public string ADTable { get; private set; }
        /// <summary>
        /// スキャン時のテーブルの高さ
        /// </summary>
        public string ScanTablePosi { get; private set; }
        /// <summary>
        /// 昇降識別値
        /// 1：上昇　-1:下降
        /// </summary>
        public string IUD { get; private set; }
        /// <summary>
        /// 断面変換有効ソフトで生成したファイルか?
        /// </summary>
        public string MPRConvCreaOption { get; private set; }
        /// <summary>
        /// マスク 正方形 Square = 0: 円:Circle = 1
        /// </summary>
        public string ReconMask { get; private set; }
        /// <summary>
        /// Dicom変換用 UUID
        /// </summary>
        public string Instance_UID { get; private set; }
        /// <summary>
        /// Dicom変換用 Study ID
        /// </summary>
        public string Study_Id { get; private set; }
        /// <summary>
        /// 検出器横方向画素数（生データ）
        /// </summary>
        public string Mainch { get; private set; }
        /// <summary>
        /// CT用アドレスマップ
        /// </summary>
        public IEnumerable<MCTAddress> MCTAddresses { get; private set; }
        /// <summary>
        /// CTアドレスのI/F
        /// </summary>
        private readonly ICTAddressLoad _CTAddress;
        /// <summary>
        /// 付帯情報の読込完了
        /// </summary>
        public event EventHandler EndLoadInf;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="address"></param>
        public MicroCTInfService(ICTAddressLoad service)
        {
            _CTAddress = service;

            var microCTfilename = Path.Combine(Directory.GetCurrentDirectory(),"TXS", "CTAddressTable", "MicroCT.csv");
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
                switch (add.Name)
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
                    case (nameof(ScanDate)):
                        ScanDate = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(ScanTime)):
                        ScanTime = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(WorkshopName)):
                        WorkshopName = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(Comment)):
                        Comment = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(ScanMode)):
                        ScanMode = _CTAddress.GetDataToString(data, add);
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
                    case (nameof(ReconMask)):
                        ReconMask = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(Study_Id)):
                        Study_Id = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(Instance_UID)):
                        Instance_UID = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(Gamma)):
                        Gamma = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(WindowLevel)):
                        WindowLevel = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(WindowWidth)):
                        WindowWidth = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(XrayGenType)):
                        XrayGenType = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(Focus)):
                        Focus = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(Volt)):
                        Volt = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(Anpere)):
                        Anpere = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(SliceWidth)):
                        SliceWidth = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(Mainch)):
                        Mainch = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(ImageBias)):
                        ImageBias = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(ImageSlope)):
                        ImageSlope = _CTAddress.GetDataToString(data, add);
                        break;
                    case (nameof(ScanTablePosi)):
                        ScanTablePosi = _CTAddress.GetDataToString(data, add);
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
                                            p => p.Name == nameof(ScanPosiAbs)
                                            )));
        /// <summary>
        /// 付帯情報保存
        /// </summary>
        /// <param name="path"></param>
        public void DoSaveInf(string path)
        {
            var addresslist = MCTAddresses.ToList();
            var maxbytesize = addresslist.Max(p => p.AddNum);
            var extsize = addresslist.Find(p => p.AddNum == maxbytesize).AddLength;
            byte[] savebytedata = new byte[maxbytesize + extsize];
            PropertyInfo[] propinfs = this.GetType().GetProperties();
            _CTAddress.SaveInf(path, this, propinfs, addresslist, savebytedata);
        }
    }
    /// <summary>
    /// マイクロCTの付帯情報サービスI/F
    /// </summary>
    public interface IMicroCTInfService
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
        /// <summary>
        /// 付帯情報保存
        /// </summary>
        /// <param name="path"></param>
        void DoSaveInf(string path);
    }
}
