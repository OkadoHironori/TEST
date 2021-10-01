

namespace Dio.DioController
{
    using DioLib;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class PCToDio : IPCToDio, IDisposable
    {
        /// <summary>
        /// DOテーブルロード完了
        /// </summary>
        public event EventHandler EndLoadDOTbl;
        /// <summary>
        /// DOテーブル
        /// </summary>
        public IEnumerable<DIOTable> DOtable { get; private set; }
        /// <summary>
        /// DIOインターフェイス
        /// </summary>
        private IDio _Dio;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dio"></param>
        public PCToDio()
        {
            //_Dio = dio;
        }
        public void Create(IDio dio)
        {
            _Dio = dio;
        }
        /// <summary>
        /// PCからDIOへの通知（「原点位置に来たよ」とか）
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sts"></param>
        public void Notify(string cmd, bool chk)
        {
            var quer = DOtable.ToList().Find(p => p.Device_name == cmd);
            if (quer != null)
            {
                _Dio.SetDo(chk, (uint)quer.Dio_num);
            }
            else
            {
                throw new Exception("DOtable.csv is illegal");
            }
        }
        /// <summary>
        /// DIテーブル
        /// </summary>
        public bool Load()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "TXSParam", "Mecha", "DIO", "DOTable.csv");

            if (!File.Exists(path))
            {
                var mes = $"{path}{Environment.NewLine}{path}{Environment.NewLine}";
                throw new Exception($"{mes} dosn't exists");
            }

            //CSVファイル読込 Tuple
            List<Tuple<string, string>> FObj = new List<Tuple<string, string>>();
            FObj = File.ReadAllLines(path)
                    .Select(v => v.Split(','))
                    .Where(j => !string.IsNullOrEmpty(j[0]) && !string.IsNullOrEmpty(j[1]) && bool.Parse(j[2]))
                    .Select(c => new Tuple<string, string>(c[0], c[1])).ToList();

            DOtable = SetTableForEnum(FObj);

            EndLoadDOTbl?.Invoke(this, new EventArgs());

            return true;
        }
        /// <summary>
        /// テーブルをIEnumerableに変換する
        /// </summary>
        /// <param name="FObj"></param>
        /// <returns></returns>
        private IEnumerable<DIOTable> SetTableForEnum(List<Tuple<string, string>> FObj)
        {
            List<DIOTable> tmpDItable = new List<DIOTable>();
            foreach (var Obj in FObj)
            {
                DIOTable unit = new DIOTable()
                {
                    Dio_num = int.Parse(Obj.Item1),
                    Device_name = Obj.Item2,
                };
                tmpDItable.Add(unit);
            }
            return tmpDItable;
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            
        }
    }
    /// <summary>
    /// PCからDIOへの
    /// </summary>
    public interface IPCToDio
    {
        /// <summary>
        /// Dioインターフェイス注入
        /// </summary>
        /// <param name="dio"></param>
        void Create(IDio dio);
        /// <summary>
        /// パラメータ読込
        /// </summary>
        /// <param name="mesbox"></param>
        /// <returns></returns>
        bool Load();
        /// <summary>
        /// PCからDioへの通知
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sts"></param>
        void Notify(string cmd, bool sts);
        /// <summary>
        /// DOテーブルロード完了
        /// </summary>
        event EventHandler EndLoadDOTbl;
    }
}
