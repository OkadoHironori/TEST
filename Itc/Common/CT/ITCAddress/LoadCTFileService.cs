

using Microsoft.Extensions.DependencyInjection;
using Itc.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTAddress
{
    /// <summary>
    /// 画像ファイル読込クラス
    /// </summary>
    public class LoadCTFileService : ILoadCTFileService
    {
        public string LoadExtension = ".inf";
        /// <summary>
        /// 画像フォーマット 1:16bit -32768-32767 0:14bit -8192-8191
        /// </summary>
        public string ImageFormat { get; private set; }
        /// <summary>
        /// CTデータ
        /// </summary>
        public short[] CTData { get; private set; }
        /// <summary>
        /// 画像サイズ
        /// </summary>
        public Size ImageSize { get; private set; }
        /// <summary>
        /// Imgファイル
        /// </summary>
        public string ImgFileName { get; private set; }
        /// <summary>
        /// ファイルの読み込み完了
        /// </summary>
        public event EventHandler EndOpenFile;
        /// <summary>
        /// ターゲットファイル
        /// </summary>
        private readonly ILoadTargetFileService _TargetFileService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public LoadCTFileService(ILoadTargetFileService service)
        {
            _TargetFileService = service;
            _TargetFileService.EndLoadInf += (s, e) => 
            {
                if(s is LoadTargetFileService ltfs)
                {
                    ImgFileName = Path.ChangeExtension(ltfs.TargetFile,".img");
                    ImageSize = new Size(int.Parse(ltfs.Matrix), int.Parse(ltfs.Matrix));
                    ImageFormat = ltfs.ImageFormat;
                }
            };
        }
        /// <summary>
        /// ファイルオープン
        /// </summary>
        public void DoOpenFile()
        {
            if (File.Exists(ImgFileName))
            {
                byte[] buf = null;
                using (var fs = new FileStream(ImgFileName, FileMode.Open, FileAccess.Read))
                {
                    buf = new byte[fs.Length]; // データ格納用配列
                    fs.Read(buf, 0, (int)fs.Length);
                }
                CTData = BinaryConverter.ConvertBtoS_BigEndian(buf, ImageSize.Width, ImageSize.Height);
                EndOpenFile?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// ファイルオープン
        /// </summary>
        public void DoOpenFile(string path)
        {

            if(Path.GetExtension(path) == LoadExtension)
            {
                path = Path.ChangeExtension(path, ".img");
            }

            //path = Path.ChangeExtension(path, ".float");

            if (File.Exists(path))
            {
                byte[] buf = null;
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    buf = new byte[fs.Length]; // データ格納用配列
                    fs.Read(buf, 0, (int)fs.Length);
                }

                CTData = BinaryConverter.ConvertBtoS_BigEndian(buf, ImageSize.Width, ImageSize.Height);

                EndOpenFile?.Invoke(this, new EventArgs());
            }
            else
            {
                throw new Exception($"{path} is not exist.");
            }
                    
        }
    }
    /// <summary>
    /// 画像ファイル読込クラス I/F
    /// </summary>
    public interface ILoadCTFileService
    {
        /// <summary>
        /// ファイルオープン完了イベント
        /// </summary>
        event EventHandler EndOpenFile;
        /// <summary>
        /// ファイル開く(ターゲットファイル)
        /// </summary>
        void DoOpenFile();
        /// <summary>
        /// ファイル開く(ファイル指定)
        /// </summary>
        /// <param name="path"></param>
        void DoOpenFile(string path);
    }
}
