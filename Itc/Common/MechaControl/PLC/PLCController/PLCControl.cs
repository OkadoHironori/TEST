
namespace PLCController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// PLCコントローラ
    /// </summary>
    public class PLCControl : IPLCControl, IDisposable
    {
        /// <summary>
        /// Int
        /// </summary>
        public int IntStatus { get; private set; }
        /// <summary>
        /// Bool
        /// </summary>
        public bool BoolStatus { get; private set; }
        /// <summary>
        /// 浮動小数点
        /// </summary>
        public float FloatStatus { get; private set; }
        /// <summary>
        /// タイプ
        /// </summary>
        public string ElementType { get; private set; }
        /// <summary>
        /// エレメント名
        /// </summary>
        public string ElementName { get; private set; }
        /// <summary>
        /// PLCが変化した
        /// </summary>
        public event EventHandler PLCChanged;
        /// <summary>
        /// PLCの読込マップ
        /// </summary>
        public IEnumerable<ReadPLCMap> ReadPLCMap { get; private set; }
        /// <summary>
        /// PLC生存確認
        /// </summary>
        private readonly IPLCProvider _Provider;
        /// <summary>
        /// PLC->PC
        /// </summary>
        private readonly IPLCServer _PLCService;
        /// <summary>
        /// PC->PLC
        /// </summary>
        private readonly IPCtoPLC _PCtoPLC;
        /// <summary>
        /// PC->PLC
        /// </summary>
        private readonly IPLCtoPC _PLCtoPC;
        /// <summary>
        /// PLCアドレス
        /// </summary>
        private readonly IPLCAddress _PLCAddress;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="chk"></param>
        public PLCControl(IPLCServer plsserv, IPCtoPLC pctoplc, IPLCAddress address, IPLCProvider provider, IPLCtoPC plctopc)
        {
            _Provider = provider;
            _Provider.PLCChanged += (s, e) =>
            {
                var prov = s as PLCProvider;
                ElementName = prov.ElementName;
                ElementType = prov.ElementType;
                IntStatus = prov.IntStatus;
                BoolStatus = prov.BoolStatus;
                FloatStatus = prov.FloatStatus;
                PLCChanged?.Invoke(this, new EventArgs());
            };

            _PLCAddress = address;
            _PLCAddress.EndLoadCSV += (s, e) =>
            {
                PLCAddress plad = s as PLCAddress;
                ReadPLCMap = plad.ReadPLCMap;
            };

            _PCtoPLC = pctoplc;

            _PLCtoPC = plctopc;

            _PLCService = plsserv;

            Task.WaitAll(Task.Delay(4000));//4秒待ってパラメータの計算を止める
            _PLCService.Start();
        }
        /// <summary>
        /// PLCへメッセージ送信(bool型)
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="sts"></param>
        /// <param name="mesbox"></param>
        /// <returns></returns>
        public void SendMessage(string cmd, bool sts)
        {
            if(!_PCtoPLC.WriteCommandBit(cmd, sts))
            {
                throw new Exception($"{cmd}{Environment.NewLine} couldn't send the PLC.");
            }
        }

        /// <summary>
        /// PLCへメッセージ送信(float)
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        public void SendMessage(string cmd, float param)
        {
            if(!_PCtoPLC.WriteCommandWord(cmd, param.ToString()))
            {
                throw new Exception($"{cmd}{Environment.NewLine} couldn't send the PLC.");
            }
        }
        /// <summary>
        /// スケールと有効桁数を変換するメソッド
        /// </summary>
        /// <returns></returns>
        public int ConvertScaleToDecip(int PLCscale)
        {
            switch(PLCscale)
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
            var quertbl = ReadPLCMap.ToList();
            foreach(var tble in quertbl)
            {
                var quername = tble.PLCContext.ReadTbl.ToList().Find(p => string.Compare(p.DevName, devName, true) == 0);

                if(quername!= null)
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

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            _PLCService.Stop();
            Task.WaitAll(Task.Delay(4000));//4秒待ってパラメータの計算を止める
        }
    }

    public interface IPLCControl
    {
        /// <summary>
        /// 変更通知
        /// </summary>
        //event EventHandler PLCChanged;​
        /// <summary>
        /// PLC接続完了イベント
        /// </summary>
        //event EventHandler EndPLCConnect;
        /// <summary>
        /// PLCへメッセージ送信(float)
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        void SendMessage(string cmd, float param);
        /// <summary>
        /// PLCへメッセージ送信(float)
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        void SendMessage(string cmd, bool sts);
        /// <summary>
        /// スケールと有効桁数を変換するメソッド(winformsのNumeric​UpDownクラス用)
        /// </summary>
        /// <returns></returns>
        int ConvertScaleToDecip(int PLCscale);
        /// <summary>
        /// デバイス名から有効桁数を取得
        /// </summary>
        /// <param name="devName"></param>
        /// <returns></returns>
        int ConvertScaleToDecip(string devName);
        /// <summary>
        /// PLC接続完了イベント
        /// </summary>
        event EventHandler PLCChanged;

    }
}
