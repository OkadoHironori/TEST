using Itc.Common.TXEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCController
{
    /// <summary>
    /// PCからPLCへの書込クラス
    /// </summary>
    public class PCtoPLC : IPCtoPLC, IDisposable
    {
        /// <summary>
        /// PLCサーバーインタフェイス
        /// </summary>
        private readonly IPLCServer _PLCServer;
        /// <summary>
        /// PLCサーバーインタフェイス
        /// </summary>
        private readonly IPLCAddress _PLCAddress;
        /// <summary>
        /// PCからPLCへの書き込みマップ
        /// </summary>
        public PCtoPLCMap PCtoPLCMap { get; private set; }
        /// <summary>
        /// PLCの読込マップ
        /// </summary>
        //public IEnumerable<ReadPLCMap> ReadPLCMap { get; private set; }
        ///// <summary>
        ///// PC->PLC用Wordテーブル
        ///// </summary>
        //public IEnumerable<PCtoPLCWordTbl> PCtoPLCWordTbl { get; private set; }
        ///// <summary>
        ///// PLC->PC用Bitテーブル
        ///// </summary>
        //public IEnumerable<PCtoPLCBitTbl> PCtoPLCBitTbl { get; private set; }
        /// <summary>
        /// コンスト
        /// </summary>
        /// <param name="server"></param>
        public PCtoPLC(IPLCServer server, IPLCAddress address)
        {
            _PLCServer = server;
            _PLCAddress = address;
            _PLCAddress.EndLoadCSV += (s, e) => 
            {
                var pa = s as PLCAddress;
                PCtoPLCMap = pa.PCtoPLCMap;
            };
            if(PCtoPLCMap==null)
            {
                _PLCAddress.RequestParam();
            }
        }
        /// <summary>
        /// Bitコマンドの書込
        /// </summary>
        /// <param name="command"></param>
        public bool WriteCommandBit(string commandname, bool param)
        {

            PCtoPLCBitTbl dev = PCtoPLCMap.PCtoPLCBitTbl.ToList().Find(p => string.Compare(p.DevName, commandname, true) == 0);
            if (dev == null)
            {
                throw new Exception(string.Format("can't find {0}", commandname));
            }

            var bitdata = param ? "1" : "0";

            commandname = dev.Command + bitdata + "\r\n";

            _PLCServer.WriteWordPLCData(commandname);

            return true;

        }
        /// <summary>
        /// Wordコマンドの書込
        /// </summary>
        /// <param name="command"></param>
        public bool WriteCommandWord(string commandname, string param)
        {

            PCtoPLCWordTbl dev = PCtoPLCMap.PCtoPLCWordTbl.ToList().Find(p => p.DevName == commandname);
            if (dev == null)
            {
                throw new Exception(string.Format("can't find {0}", commandname));
            }

            if (dev.Endian == 1)
            {

                float data = float.Parse(param);

                int value = (int)Math.Round(data * dev.Scale, 0);

                var senddata = Convert.ToString(value, 16).PadLeft(4, '0');

                commandname = dev.Command + senddata + "\r\n";
            }
            else
            {
                float data = float.Parse(param);

                int value = (int)Math.Round(data * dev.Scale, 0);

                var tmp2 = Convert.ToString(Convert.ToInt32(value), 16).PadLeft(8, '0');

                var senddata = tmp2.Substring(4, 4) + tmp2.Substring(0, 4);   //2ﾜｰﾄﾞﾃﾞｰﾀの上位下位の入替

                commandname = dev.Command + senddata + "\r\n";
            }

            _PLCServer.WriteWordPLCData(commandname);

            return true;
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {

        }
    }
    /// <summary>
    /// インターフェイス
    /// </summary>
    public interface IPCtoPLC
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        //PCtoPLCBitTbl GetBitTbl(string cmd);
        /// <summary>
        /// Wordコマンドの書込
        /// </summary>
        /// <param name="command"></param>
        bool WriteCommandWord(string commandname, string param);
        /// <summary>
        /// Bitコマンドの書込
        /// </summary>
        /// <param name="command"></param>
        bool WriteCommandBit(string commandname, bool param);

    }
}
