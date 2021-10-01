using Itc.Common.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PLCController
{
    /// <summary>
    /// PLCからのデータ取扱クラス
    /// TODO もっときれいになると思う
    /// </summary>
    public class PLCtoPC : IPLCtoPC, IDisposable
    {
        /// <summary>
        /// PLCサーバーインタフェイス
        /// </summary>
        private readonly IPLCServer _PLCServer;
        /// <summary>
        /// コマンドタイプ
        /// </summary>
        public string CmdType { get; private set; }
        /// <summary>
        /// PLCから送信されてくるboollist
        /// </summary>
        public IEnumerable<bool> PLCBools { get; private set; }
        /// <summary>
        /// PLCから送信されてくるメッセージ
        /// </summary>
        public string[] PLCMes { get; private set; }
        /// <summary>
        /// コマンド受信
        /// </summary>
        public event EventHandler PLCRecived;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="server"></param>
        public PLCtoPC(IPLCServer server)
        {
            _PLCServer = server;

            _PLCServer.PLCBitMessage += (s, e) =>
            {
                PLCServer plcservice = s as PLCServer;
                PLCBools = null;
                PLCMes = null;
                List<bool> boollist = new List<bool>();//boolリスト

                //var filename = Path.Combine(Path.GetDirectoryName(Directory.GetCurrentDirectory()), "bittest.dat");
                //using (var writer = new BinaryWriter(new FileStream(filename, FileMode.Create)))
                //{
                //    writer.Write(e.CmdByte);
                //}

                string tmpmes = Encoding.GetEncoding(Encoding.ASCII.EncodingName).GetString(plcservice.Respons).ToString();
                var startproto = tmpmes.Substring(0, 5);
                var endproto = tmpmes.Substring(tmpmes.Length - 3, 3);
                if (startproto == "\u000200FF" && endproto == "\u0003\r\n")
                {
                    if (plcservice.Respons != null && Enum.TryParse(plcservice.CmdType, out PLCDataType type))
                    {
                        if (type == PLCDataType.ParamBit)
                        {

                            string allmes = Encoding.GetEncoding(Encoding.ASCII.EncodingName).GetString(plcservice.Respons).ToString().Substring(5);//最初の(1文字("F") = 5文字)を除去
                            allmes = allmes.ToString().Substring(0, allmes.Length - 3);//終わりの4文字を除去
                            if (Regex.IsMatch(allmes, @"^[0-9A-F]{4}"))
                            {
                                string[] strm = allmes.StrSplit(1);//文字分割
                                foreach (var mes in strm)
                                {
                                    if (int.TryParse(mes, out int res))
                                    {
                                        bool flg = res == 1 ? true : false;
                                        boollist.Add(flg);
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                return;//途中で途切れた
                            }

                            PLCBools = boollist;
                            CmdType = type.ToString();
                            PLCRecived?.Invoke(this, new EventArgs());

                        }
                        else
                        {
                            string allmes = Encoding.GetEncoding(Encoding.ASCII.EncodingName).GetString(plcservice.Respons).ToString().Substring(8);//最初の(4文字＋4文字("F000") = 8文字)を除去
                            allmes = allmes.ToString().Substring(0, allmes.Length - 4);//終わりの4文字を除去
                            if (Regex.IsMatch(allmes, @"^[0-9A-F]{4}"))
                            {
                                string[] strm = allmes.StrSplit(1);//文字分割

                                IEnumerable<int> dataIdx = Enumerable.Range(0, allmes.Length);
                                foreach (var ddd in dataIdx)
                                {
                                    try
                                    {
                                        var pp = Convert.ToString(Convert.ToInt32(strm[ddd], 16), 2);//16進数→2進数の変換
                                        pp = string.Format("{0:D16}", int.Parse(pp));//0埋め
                                        string[] pptrm = pp.StrSplit(1);//分割
                                        IEnumerable<int> Idx16 = Enumerable.Range(0, pptrm.Count()).Reverse();//逆カウント
                                        foreach (var d in Idx16)
                                        {
                                            bool flg = int.Parse(pptrm[d]) == 1 ? true : false;
                                            boollist.Add(flg);
                                        }
                                    }
                                    catch
                                    {
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                return;//途中で途切れた
                            }

                            PLCBools = boollist;
                            CmdType = type.ToString();

                            PLCRecived?.Invoke(this, new EventArgs());

                            //Task.Run(() => PLCRecived?.Invoke(this, new EventArgs()));
                        }
                    }
                }
                else
                {
                    return;//不正データ
                }
            };

            _PLCServer.PLCWordMessage += (s, e) =>
            {
                //var filename = Path.Combine(Path.GetDirectoryName(Directory.GetCurrentDirectory()), "wordtest.dat");
                //using (var writer = new BinaryWriter(new FileStream(filename, FileMode.Create)))
                //{
                //    writer.Write(e.CmdByte);
                //}
                PLCServer plcservice = s as PLCServer;
                PLCBools = null;
                PLCMes = null;
                List<bool> boollist = new List<bool>();//boolリスト

                string tmpmes = Encoding.GetEncoding(Encoding.ASCII.EncodingName).GetString(plcservice.Respons).ToString();
                var startproto = tmpmes.Substring(0, 5);
                if ((startproto == "\u000200FF"|| startproto == "\u000600FF"))
                {
                    if (plcservice.CmdType != null)
                    {
                        string allmes = tmpmes.ToString().Substring(5);//最初の5文字を除去
                        string[] text = allmes.StrSplit(4);//拡張メソッドで4文字毎に分割

                        PLCMes = text;
                        CmdType = plcservice.CmdType;

                        PLCRecived.Invoke(this, new EventArgs());
                        //Task.Run(() => PLCRecived.Invoke(this, new EventArgs()));
                    }
                }
                else
                {
                    return;
                }
            };
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            PLCBools = null;
            PLCMes = null;
        }
    }
    /// <summary>
    /// 破棄
    /// </summary>
    public interface IPLCtoPC
    {
        event EventHandler PLCRecived;
    }
}
