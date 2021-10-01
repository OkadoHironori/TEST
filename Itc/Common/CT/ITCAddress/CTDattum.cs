using CTAddress;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTAddress
{
    public class CTDattum : ICTDattum
    {
        /// <summary>
        /// データロード完了通知
        /// </summary>
        public event EventHandler EndLoadData;
        /// <summary>
        /// マトリックスサイズ
        /// </summary>
        public int Matrix { get; private set; }
        /// <summary>
        /// 最大値
        /// </summary>
        public short MaxVale { get; private set; } = 8191;
        /// <summary>
        /// 最小値
        /// </summary>
        public short MinVale { get; private set; } = -8192;
        /// <summary>
        /// ウィンドウレベル
        /// </summary>
        public int WL { get; private set; }
        /// <summary>
        /// ウィンドウ幅
        /// </summary>
        public int WW { get; private set; }
        /// <summary>
        /// ガンマ値
        /// </summary>
        public float Gamma { get; private set; }
        /// <summary>
        /// 1画素サイズ mm
        /// </summary>
        public float DotSize { get; private set; }
        /// <summary>
        /// CTデータ
        /// </summary>
        public IEnumerable<CTdata> CTdatas { get; private set; }
        /// <summary>
        /// ターゲットファイル指定サービス
        /// </summary>
        private readonly ILoadTargetFileService _LoadFileService;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="loadimg"></param>
        public CTDattum(ILoadTargetFileService loadimg)
        {
            _LoadFileService = loadimg;
            _LoadFileService.EndLoadInf += (s, e) =>
            {
                if (s is LoadTargetFileService ltfs)
                {
                    Matrix = int.Parse(ltfs.Matrix);
                    DotSize = float.Parse(ltfs.Scale);
                    WL = int.Parse(ltfs.WL);
                    WW = int.Parse(ltfs.WW);
                    Gamma = float.Parse(ltfs.Gamma);
                };
            };
        }
        /// <summary>
        /// データ生成
        /// </summary>
        /// <param name="filepaths"></param>
        public void CreateDattum(IEnumerable<string> filepaths)
        {
            List<CTdata> tmpdatas = new List<CTdata>();
            //int Idx = 1;//画像は1オリジン
            foreach (var d in filepaths)
            {
                string imgfilename = Path.GetFileNameWithoutExtension(d);
                int imgIdx = int.Parse(imgfilename.Substring(imgfilename.Length - 4, 4));



                CTdata tmpdata = new CTdata();
                tmpdata.DoLoadFile(d, Matrix, MaxVale, MinVale, imgIdx);
                tmpdatas.Add(tmpdata);
                //Idx++;
            }
            CTdatas = tmpdatas;

            EndLoadData?.Invoke(this, new EventArgs());
        }

    }
    public interface ICTDattum
    {
        /// <summary>
        /// データ生成
        /// </summary>
        /// <param name="filepaths"></param>
        void CreateDattum(IEnumerable<string> filepaths);
        /// <summary>
        /// データロード完了通知
        /// </summary>
        event EventHandler EndLoadData;
    }
}
