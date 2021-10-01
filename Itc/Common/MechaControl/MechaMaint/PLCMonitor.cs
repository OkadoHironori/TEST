using Itc.Common.Event;
using Itc.Common.Extensions;
using Itc.Common.TXEnum;
using PLCController;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MechaMaintCnt
{
    using MessageEventHandler = Action<object, MessageEventArgs>;
    /// <summary>
    /// TXSのPLC監視クラス
    /// </summary>
    public class PLCMonitor : IPLCMonitor
    {
        /// <summary>
        /// システムモデル
        /// </summary>
        public string SYSTEM_MODEL { get; }
        /// <summary>
        /// 
        /// </summary>
        MonitorProvider MonitorProvider { get; }
        /// <summary>
        /// PLC読取コマンド
        /// </summary>
        public event MessageEventHandler RequestPLC;
        /// <summary>
        /// 変更通知
        /// </summary>
        public event Property​Changed​Event​Handler StatusChanged;
        /// <summary>
        /// PLCアドレス
        /// </summary>
        private readonly IPLCAddress _PLCAddress; 
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="address"></param>
        public PLCMonitor(IPLCAddress address)
        {
            _PLCAddress = address;
            MonitorProvider = new MonitorProvider(address);
            MonitorProvider.Create();
            //PLCの変更通知（前処理）           
            MonitorProvider.ValueChanged += async (s, e) =>
            {
                await Task.Run(() => StatusChanged?.Invoke(s, e));
            };

            SYSTEM_MODEL = address.SYSTEM_MODEL;

        }
        /// <summary>
        /// PLCから受信したデータを解析する
        /// </summary>
        /// <param name="data"></param>
        public void SetSerialData(byte[] data, PLCDataType type)
        {
            MonitorProvider.SetSerialData(data, type);
        }
        public void SetSerialData(string data, PLCDataType type)
        {
            MonitorProvider.SetSerialData(data, type);
        }

        /// <summary>
        /// コマンドを取得しにきた
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetRequestCmd(PLCDataType type)
        {
            if(type == PLCDataType.ParamWordNew)
            {
                type = PLCDataType.ParamWord;
            }

            var quer = _PLCAddress.ReadPLCMap.ToList().Find(p => p.PLCDataType == type);
            if(quer!=null)
            {
                return quer.PLCContext.ReadCommand;
            }
            throw new Exception($"{type.ToString()}Couldn't find Command");      
        }

        /// <summary>
        /// PLCへメッセージ送信(bool型)
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="sts"></param>
        /// <param name="mesbox"></param>
        /// <returns></returns>
        public void SendMessage(string cmd, bool sts, Action<string> mesbox)
        {

            PCtoPLCBitTbl dev = _PLCAddress.PCtoPLCMap.PCtoPLCBitTbl.ToList().Find(p => string.Compare(p.DevName, cmd, true) == 0);
            if (dev == null)
            {
                throw new Exception(string.Format("can't find {0}", cmd));
            }

            var bitdata = sts ? "1" : "0";

            cmd = dev.Command + bitdata + "\r\n";

            RequestPLC?.Invoke(this, new MessageEventArgs(cmd));

        }

        /// <summary>
        /// PLCへメッセージ送信(float)
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        public void SendMessage(string cmd, float param, Action<string> mesbox)
        {
            PCtoPLCWordTbl dev = _PLCAddress.PCtoPLCMap.PCtoPLCWordTbl.ToList().Find(p => p.DevName == cmd);
            if (dev == null)
            {
                throw new Exception(string.Format("can't find {0}", cmd));
            }

            if (dev.Endian == 1)
            {

                float data = float.Parse(param.ToString());

                int value = (int)Math.Round(data * dev.Scale, 0);

                var senddata = Convert.ToString(value, 16).PadLeft(4, '0');

                cmd = dev.Command + senddata + "\r\n";
            }
            else
            {
                float data = float.Parse(param.ToString());

                int value = (int)Math.Round(data * dev.Scale, 0);

                var tmp2 = Convert.ToString(Convert.ToInt32(value), 16).PadLeft(8, '0');

                var senddata = tmp2.Substring(4, 4) + tmp2.Substring(0, 4);   //2ﾜｰﾄﾞﾃﾞｰﾀの上位下位の入替

                cmd = dev.Command + senddata + "\r\n";
            }

            RequestPLC?.Invoke(this, new MessageEventArgs(cmd));
        }
        /// <summary>
        /// スケールと有効桁数を変換するメソッド
        /// </summary>
        /// <returns></returns>
        public int ConvertScaleToDecip(int PLCscale)
        {
            switch (PLCscale)
            {
                case (1):
                    return 0;
                case (10):
                    return 1;
                case (100):
                    return 2;
                case (1000):
                    return 3;
                default:
                    throw new Exception("Undeveloped Scale Value");
            }
        }
        /// <summary>
        /// .csvのデバイス名から有効桁数を取得するメソッド
        /// </summary>
        /// <returns></returns>
        public int ConvertScaleToDecip(string devName)
        {
            string dname = string.Empty;
            int plcscale = 0;
            var quertbl = _PLCAddress.ReadPLCMap.ToList();
            foreach (var tble in quertbl)
            {
                var quername = tble.PLCContext.ReadTbl.ToList().Find(p => string.Compare(p.DevName, devName, true) == 0);

                if (quername != null)
                {
                    dname = quername.DevName;
                    plcscale = quername.Scale;
                    break;
                }
            }

            if (string.IsNullOrEmpty(dname))
            {
                var mes = $"{devName} can't find Table";
                throw new Exception(mes);
            }

            return ConvertScaleToDecip(plcscale);

        }
    }
    /// <summary>
    /// TXSのPLC監視クラス
    /// </summary>
    public interface IPLCMonitor
    {
        /// <summary>
        /// システムモデル
        /// </summary>
        string SYSTEM_MODEL { get; }
        /// <summary>
        /// 要求
        /// </summary>
        event MessageEventHandler RequestPLC;
        /// <summary>
        /// 受信
        /// </summary>
        /// <param name="data"></param>
        void SetSerialData(byte[] data, PLCDataType type);
        /// <summary>
        /// 受信
        /// </summary>
        /// <param name="data"></param>
        void SetSerialData(string data, PLCDataType type);
        /// <summary>
        /// コマンド取得
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetRequestCmd(PLCDataType type);
        /// <summary>
        /// PLCProviderのイベントハンドラ
        /// </summary>
        event Property​Changed​Event​Handler StatusChanged;
        /// <summary>
        /// PLCへメッセージ送信(float)
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        void SendMessage(string cmd, float param, Action<string> mesbox);
        /// <summary>
        /// PLCへメッセージ送信(float)
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        void SendMessage(string cmd, bool sts, Action<string> mesbox);
        /// <summary>
        /// .csvのデバイス名から有効桁数を取得するメソッド
        /// </summary>
        /// <returns></returns>
        int ConvertScaleToDecip(string devName);
        /// <summary>
        /// スケールと有効桁数を変換するメソッド
        /// </summary>
        /// <returns></returns>
        int ConvertScaleToDecip(int PLCscale);

    }
}

