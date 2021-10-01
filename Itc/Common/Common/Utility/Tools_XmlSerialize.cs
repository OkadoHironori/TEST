using System.IO;

namespace Itc.Common.Utility
{
    /// <summary>
    /// XMLシリアライズ関係
    /// </summary>
    public static partial class Tools
    {
        //日本語の読込用にencodingを指定する
        readonly static System.Text.Encoding Encoding = System.Text.Encoding.UTF8;

        /// <summary>
        /// XmlSerializerによる保存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="filepath">ファイルパス</param>
        public static void SaveByXmlSerializer<T>(T src, string filepath)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            
            //※「OpenOrCreate」ではゴミが残る
            using (var fs = new FileStream(filepath, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, Encoding))
                {
                    serializer.Serialize(sw, src);
                }
            }
        }

        /// <summary>
        /// XmlSerializerによる読み込み
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filepath">ファイルパス</param>
        /// <returns></returns>
        public static T LoadByXmlSerializer<T>(string filepath)
        {
            //xml読込
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            
            //@18-06-13 v1.2.0.4 日本語の読込が出来なかったのでencodingを指定する
            using (var fs = new FileStream(filepath, FileMode.Open))
            {
                using (var sr = new StreamReader(fs, Encoding))
                {
                    return (T)serializer.Deserialize(sr);
                }
            }
        }
    }
}
