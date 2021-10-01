using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTAddress
{
    /// <summary>
    /// FOV変換クラス
    /// </summary>
    public class FOVConverter : IFOVConverter
    {
        /// <summary>
        /// エラー発生時
        /// </summary>
        public event EventHandler ErrorMes;
        /// <summary>
        /// FOV入力完了
        /// </summary>
        public event EventHandler EndInputFov;
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string ErrorMessage { get; private set; } = "不正な値を入力しました";
        /// <summary>
        /// インポート用ファイル
        /// </summary>
        public IEnumerable<ImportFile> ImportFiles { get; private set; }
        /// <summary>
        /// スライスピッチ
        /// </summary>
        public float SlicePitch { get; private set; }
        /// <summary>
        /// マトリックス
        /// </summary>
        public int Matrix { get; private set; }
        /// <summary>
        /// インポート時のスキャンエリア
        /// </summary>
        public float ImportScanArea { get; private set; }
        /// <summary>
        /// インポート時の画素サイズ
        /// </summary>
        public float ImportDotSize { get; private set; }
        /// <summary>
        /// インポートサービス
        /// </summary>
        private readonly IFileSelectService _FileSel;
        /// <summary>
        /// FOV変換クラス
        /// </summary>
        /// <param name="service"></param>
        public FOVConverter(IFileSelectService service)
        {
            _FileSel = service;
            _FileSel.CmpSerchFile += (s, e) =>
            {
                if (s is FileSelectService dis)
                {
                    SlicePitch = dis.SlicePitch;

                    Matrix = int.Parse(dis.Matrix);

                    ImportFiles = dis.ImportFiles;

                    ImportScanArea = (float)Math.Round(float.Parse(dis.Scale), 3, MidpointRounding.AwayFromZero);

                    ImportDotSize =  ImportScanArea/ Matrix;

                    EndInputFov?.Invoke(this, new EventArgs());
                }
            };

        }
        /// <summary>
        /// FOV変換時
        /// </summary>
        /// <param name="inputfov"></param>
        public void InputFov(float inputfov)
        {
            float rifox = (float)Math.Round(inputfov, 3, MidpointRounding.AwayFromZero);

            if (rifox > (float)Math.Round(ImportScanArea, 3, MidpointRounding.AwayFromZero))
            {
                ErrorMes?.Invoke(this, new EventArgs());
                return;
            }

            if(rifox != (float)Math.Round(ImportScanArea, 3, MidpointRounding.AwayFromZero))
            {
                ImportScanArea = rifox;
                ImportDotSize = ImportScanArea / Matrix;
            }
            else
            {
                ImportScanArea = ImportScanArea;
            }

            EndInputFov?.Invoke(this, new EventArgs());
        }
    }
    /// <summary>
    /// FOV変換クラスI/F
    /// </summary>
    public interface IFOVConverter
    {
        /// <summary>
        /// エラー発生時
        /// </summary>
        event EventHandler ErrorMes;
        /// <summary>
        /// FOV入力完了
        /// </summary>
        event EventHandler EndInputFov;
        /// <summary>
        /// FOV入力
        /// </summary>
        void InputFov(float inputfov);

    }
}
