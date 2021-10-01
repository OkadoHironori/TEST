using System;
using System.Diagnostics;
using System.IO;
using Itc.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UT_Managed.UT_DataAnalysis
{
    [TestClass]
    public class UT_DataAnalysis
    {
        [TestMethod]
        public void A_フォルダ作成()
        {
            //string PathAna = "Analysis";
            //string fullpath = Path.Combine(Environment.CurrentDirectory, PathAna);
            //Directory.CreateDirectory(Path.Combine(fullpath));
        }
        [TestMethod]
        public void B_フォルダ作成()
        {
            //string PathAna = "Analysis";
            //string fullpath = Path.Combine(Environment.CurrentDirectory, PathAna);


            //var files =Directory.GetFiles(fullpath, "*.raw", SearchOption.TopDirectoryOnly);


            //foreach(var file in files)
            //{
            //    byte[] buf = null;
            //    using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            //    {
            //        buf = new byte[fs.Length]; // データ格納用配列
            //        fs.Read(buf, 0, (int)fs.Length);
            //    }
            //    var CTData = BinaryConverter.ConvertBtoF_BigEndian(buf,1536, 864);


            //    var ttt = file.Split(',');

            //    int ddd = 1536 * 432 + 432;

            //    Debug.WriteLine($"{}{CTData[ddd]}");

            //}
        }
    }
}
