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
    /// CT付帯情報のアドレスリスト読込クラス
    /// </summary>
    public class CTAddressLoad : ICTAddressLoad
    {
        /// <summary>
        /// 付帯情報変数をStringに変換するメソッド
        /// </summary>
        /// <param name="data"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public string GetDataToString(byte[] data, MCTAddress add)
        {
            string res = string.Empty;

            if (data.Length > add.AddNum)
            {

                switch (add.TypeName)
                {
                    case ("C"):

                        res = Encoding.Default.GetString(data, add.AddNum, add.AddLength).TrimEnd('\0');

                        if (add.Scale != 1)
                        {
                            string[] countpoint = res.Split('.');
                            int dd = countpoint[countpoint.Count() - 1].Count();
                            double resdouble = double.Parse(res) / add.Scale;
                            resdouble = (double)(Math.Floor(resdouble * (Math.Pow(10, (dd + 3)))) / (Math.Pow(10, (dd + 3))));//桁落ちを恐れて念のため
                            res = resdouble.ToString();
                        }

                        break;
                    case ("Int"):

                        res = BitConverter.ToInt32(data, add.AddNum).ToString();

                        if (add.IsConvertBooltoString)
                        {
                            var tmpres = int.Parse(res) == 1 ? true : false;

                            if (tmpres)
                            {
                                res = bool.TrueString;
                            }
                            else
                            {
                                res = bool.FalseString;
                            }
                        }
                        else
                        {
                            if (add.Scale != 1)
                            {
                                float resdouble = int.Parse(res) / add.Scale;
                                res = resdouble.ToString();
                            }
                        }


                        break;
                    case ("Float"):
                        res = BitConverter.ToSingle(data, add.AddNum).ToString();
                        break;

                    case ("Short"):

                        res = BitConverter.ToInt16(data, add.AddNum).ToString();

                        if (add.IsConvertBooltoString)
                        {
                            var tmpres = int.Parse(res) == 1 ? true : false;

                            if (tmpres)
                            {
                                res = bool.TrueString;
                            }
                            else
                            {
                                res = bool.FalseString;
                            }
                        }
                        else
                        {
                            if (add.Scale != 1)
                            {
                                float resdouble = int.Parse(res) / add.Scale;
                                res = resdouble.ToString();
                            }
                        }

                        break;
                }
            }

            return res;
        }
        /// <summary>
        /// 読込
        /// </summary>
        public IEnumerable<MCTAddress> LoadCsvFile(string path)
        {
            //CSVファイル読込 Tuple
            List<Tuple<string, string, string, string, string, string, string>> FObj = File.ReadAllLines(path)
                    .Where(l => !string.IsNullOrEmpty(l))
                    .Select(v => v.Split(','))
                    .Where(j => Regex.IsMatch(j[0], @"^[0-9]+$") && !string.IsNullOrEmpty(j[1]) && !string.IsNullOrEmpty(j[2]))
                    .Select(c => new Tuple<string, string, string, string, string, string, string>(c[0], c[1], c[2], c[3], c[4], c[5], c[6])).ToList();

            List<MCTAddress> mCTs = new List<MCTAddress>();

            foreach (var tmp in FObj)
            {
                MCTAddress mCT = new MCTAddress()
                {
                    Name = tmp.Item2,
                    TypeName = tmp.Item3,
                    AddNum = int.Parse(tmp.Item4),
                    AddLength = int.Parse(tmp.Item5),
                    Scale = int.Parse(tmp.Item6),
                    IsConvertBooltoString = int.Parse(tmp.Item7) == 1 ? true : false,
                };
                mCTs.Add(mCT);
            }

            return mCTs;
        }
        /// <summary>
        /// 付帯情報保存
        /// </summary>
        /// <param name="path"></param>
        public void SaveInf(string path, object classparam, PropertyInfo[] propinfs, IEnumerable<MCTAddress> addresslist, byte[] savebytedata)
        {
            foreach (var propinf in propinfs)
            {
                object data = propinf.GetValue(classparam);

                if (data != null)
                {
                    string sdata = data.ToString();

                    foreach (var address in addresslist)
                    {
                        if (address.Name == propinf.Name)
                        {
                            switch (address.TypeName)
                            {
                                case ("C"):

                                    Encoding.Default.GetBytes(sdata, 0, sdata.Length, savebytedata, address.AddNum);

                                    break;
                                case ("Int"):

                                    byte[] setByteArray_Int = null;

                                    if (address.IsConvertBooltoString)
                                    {
                                        int int_val = bool.Parse(sdata) == true ? 1 : 0;
                                        setByteArray_Int = BitConverter.GetBytes(int_val);
                                    }
                                    else
                                    {
                                        setByteArray_Int = BitConverter.GetBytes(int.Parse(sdata));
                                    }

                                    Array.Copy(setByteArray_Int, 0, savebytedata, address.AddNum, address.AddLength);

                                    break;
                                case ("Float"):

                                    byte[] setByteArray_Float = BitConverter.GetBytes(float.Parse(sdata));

                                    Array.Copy(setByteArray_Float, 0, savebytedata, address.AddNum, address.AddLength);

                                    break;

                                case ("Short"):

                                    byte[] setByteArray_Short = null;

                                    if (address.IsConvertBooltoString)
                                    {
                                        int short_val = bool.Parse(sdata) == true ? 1 : 0;
                                        setByteArray_Short = BitConverter.GetBytes(short_val);
                                    }
                                    else
                                    {
                                        setByteArray_Short = BitConverter.GetBytes(int.Parse(sdata));
                                    }

                                    Array.Copy(setByteArray_Short, 0, savebytedata, address.AddNum, address.AddLength);

                                    break;
                            }
                        }
                    }
                }
            }

            using (var writer = new BinaryWriter(new FileStream(path, FileMode.Create)))
            {
                writer.Write(savebytedata);
            }
        }
    }
    /// <summary>
    /// CTのアドレス
    /// </summary>
    public class MCTAddress
    {
        /// <summary>
        /// 変数名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 型名
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// アドレス番号
        /// </summary>
        public int AddNum { get; set; }
        /// <summary>
        /// アドレス長
        /// </summary>
        public int AddLength { get; set; }
        /// <summary>
        /// スケール
        /// </summary>
        public int Scale { get; set; }
        /// <summary>
        /// Intで読込んだBool値をStringで表現するか?
        /// </summary>
        public bool IsConvertBooltoString { get; set; }
    }
    /// <summary>
    /// CT付帯情報のアドレスリスト読込クラス I/F
    /// </summary>
    public interface ICTAddressLoad
    {
        /// <summary>
        /// 付帯情報の読込
        /// </summary>
        /// <param name="bytedata">バイトデータ</param>
        IEnumerable<MCTAddress> LoadCsvFile(string path);
        /// <summary>
        /// 付帯情報変数をStringに変換するメソッド
        /// </summary>
        /// <param name="data"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        string GetDataToString(byte[] data, MCTAddress add);
        /// <summary>
        /// 付帯情報の保存
        /// </summary>
        /// <param name="data"></param>
        /// <param name="add"></param>
        void SaveInf(string path, object classparam, PropertyInfo[] infos, IEnumerable<MCTAddress> addresseslist, byte[] savebytelist);
    }
        
}
