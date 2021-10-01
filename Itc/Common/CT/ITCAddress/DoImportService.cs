using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CTAddress
{
    /// <summary>
    ///　ファイルインポートサービスクラス
    /// </summary>
    public class DoImportService : IDoImportService, IDisposable
    {
        /// <summary>
        /// インポート用パラメタセット
        /// </summary>
        public event EventHandler SetDoImportParam;
        /// <summary>
        /// CTファイル名
        /// </summary>
        public IEnumerable<ImportFile> ImportFiles { get; private set; }
        /// <summary>
        /// スライスピッチ
        /// </summary>
        public float SlicePitch { get; private set; }
        /// <summary>
        /// インポート時のスキャンエリア
        /// </summary>
        public float ImportScanArea { get; private set; }
        /// <summary>
        /// 1画素サイズ
        /// </summary>
        public float ImportDotSize { get; private set; }
        /// <summary>
        /// マトリックス
        /// </summary>
        public int Matrix { get; private set; }
        /// <summary>
        /// MPR縦方向サイズ
        /// </summary>
        public int DispStackSize { get; private set; }
        /// <summary>
        /// 表示マトリックスサイズ
        /// </summary>
        public int DispMatrix { get; private set; }
        /// <summary>
        /// ファイル選択サービス I/F
        /// </summary>
        private readonly IFileSelectService _FileSelectService;
        /// <summary>
        /// FOV変換 I/F
        /// </summary>
        private readonly IFOVConverter _FOVConv;
        /// <summary>
        /// 縦断面の画素数
        /// </summary>
        private readonly ICalStackSize _StackSize;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileselect"></param>
        public DoImportService(IFileSelectService filesel, IFOVConverter conv, ICalStackSize stack)
        {
            _FileSelectService = filesel;
            _FileSelectService.CmpSerchFile += (s, e) =>
            {
                if(s is FileSelectService fss)
                {
                    ImportFiles = fss.ImportFiles;

                    SlicePitch = fss.SlicePitch;

                    Matrix = int.Parse(fss.Matrix);

                    DispMatrix = Matrix;

                    ImportScanArea = float.Parse(fss.Scale);

                    ImportDotSize = fss.DotSize;

                    if(DispStackSize==0)
                    {
                        _FOVConv.InputFov(ImportScanArea);
                    }

                    SetDoImportParam?.Invoke(this, new EventArgs());
                }
            };

            _FOVConv = conv;
            _FOVConv.EndInputFov += (s, e) => 
            {
                if(s is FOVConverter fovc)
                {
                    ImportScanArea = fovc.ImportScanArea;
                    ImportDotSize = fovc.ImportDotSize;
                    SetDoImportParam?.Invoke(this, new EventArgs());
                }
            };

            _StackSize = stack;
            _StackSize.EndCalStack += (s, e) => 
            {
                if (s is CalStackSize css)
                {
                    DispStackSize = css.DispStackSize;
                    SetDoImportParam?.Invoke(this, new EventArgs());
                }
            };
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {

        }
    }
    /// <summary>
    /// DOインポートサービス I/F
    /// </summary>
    public interface IDoImportService
    {
        /// <summary>
        /// インポート用パラメタセット
        /// </summary>
        event EventHandler SetDoImportParam;
    }
}
