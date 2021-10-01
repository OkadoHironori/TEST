using CTAddress;
using Itc.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CTAddress
{

    public class CTdata : ICTdata
    {

        /// <summary>
        /// CTデータ
        /// </summary>
        public short[] CTData { get; private set; }
        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FilePath { get; private set; }
        /// <summary>
        /// ファイル番号
        /// </summary>
        public int FileNum { get; private set; }
        public CTdata()
        {

        }

        public void DoLoadFile(string path, int mat, int max ,int min, int idx)
        {
            if (File.Exists(path))
            {
                FilePath = path;
                FileNum = idx;
                byte[] buf = null;
                using (var fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    buf = new byte[fs.Length]; // データ格納用配列
                    fs.Read(buf, 0, (int)fs.Length);
                    CTData = BinaryConverter.ConvertBtoS_BigEndian(buf, mat, mat);
                }
            }
            //EndLoadData?.Invoke(this, new EventArgs());
        }
    }
    public interface ICTdata
    {
        /// <summary>
        /// データロード
        /// </summary>
        /// <param name="path"></param>
        void DoLoadFile(string path, int mat, int max, int min, int idx);
        ///// <summary>
        ///// データロード完了通知
        ///// </summary>
        //event EventHandler EndLoadData;
    }
}
