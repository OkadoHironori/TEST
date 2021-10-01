using Itc.Common.Event;

namespace Dio.DioController
{
    using DioLib;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using DioEventHandler = System.Action<object, DioEventArgs>;

    /// <summary>
    /// DioからPCへの情報提供インターフェイス
    /// </summary>
    public class DioToPC : IDioToPC, IDisposable
    {
        /// <summary>
        /// DIテーブル読込完了イベント
        /// </summary>
        public event EventHandler EndLoadDITbl;
        /// <summary>
        /// DIテーブル
        /// </summary>
        public IEnumerable<DIOTable> DItable { get; private set; }
        /// <summary>
        /// ロック用
        /// </summary>
        readonly object lockobjdi = new object();
        /// <summary>
        /// Dioの定期監視用
        /// </summary>
        private System.Timers.Timer _Timer = null;
        /// <summary>
        /// DIOポーリング時間
        /// </summary>
        private int PollingTime = 100;
        /// <summary>
        /// DIOデータ読込イベント
        /// </summary>
        public event DioEventHandler DioMessage;
        /// <summary>
        /// DIOインターフェイス
        /// </summary>
        private IDio _Dio;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dio"></param>
        public DioToPC()
        {

        }
        /// <summary>
        /// DIOインターフェイス注入
        /// </summary>
        /// <param name="dio"></param>
        public void Create(IDio dio)
        {
            _Dio = dio;
        }
        /// <summary>
        /// Dio開始
        /// </summary>
        public void DioStart()
        {
            if (_Timer == null)
            {

                _Timer = new System.Timers.Timer()
                {
                    Interval = PollingTime,
                    AutoReset = false
                };

                //できれば非同期
                _Timer.Elapsed += delegate
                {
                    bool[] dibits = new bool[_Dio.DoBits];

                    //念のためロックしているが効果は薄いと思う
                    lock (lockobjdi)
                    {
                        _Dio.GetDiAll(dibits);
                    }

                    DioMessage?.Invoke(this, new DioEventArgs(dibits));

                    _Timer.Start();
                };

                _Timer.Start();

                //_Timer.Enabled = true;//タイマー開始
            }
        }
        /// <summary>
        /// DIテーブル
        /// </summary>
        public bool Load()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "TXSParam", "Mecha", "DIO", "DITable.csv");

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

            DItable = SetTableForEnum(FObj);

            EndLoadDITbl?.Invoke(this, new EventArgs());

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
        /// DIO監視タイマー作動中かどうかでtrueとfalseを判定
        /// </summary>
        /// <returns></returns>
        public bool IsDioStart()
        {
            return _Timer != null ? true : false;
        }
        /// <summary>
        /// 破棄
        /// 監視タイマー停止
        /// </summary>
        public void Dispose()
        {
            if (_Timer != null)
            {
                _Timer.Stop();
                _Timer.Enabled = false;//停止
                Task.WaitAll(Task.Delay((PollingTime * 2)));//処理中かもしれないので念のため待つ
                _Timer.Dispose();
                _Timer = null;
            }
        }
    }

    public interface IDioToPC
    {
        /// <summary>
        /// Dioインターフェイス注入
        /// </summary>
        /// <param name="dio"></param>
        void Create(IDio dio);
        /// <summary>
        /// Dioデータ読込
        /// </summary>
        event DioEventHandler DioMessage;
        /// <summary>
        /// DIテーブル読込完了イベント
        /// </summary>
        event EventHandler EndLoadDITbl;
        /// <summary>
        /// Dio監視開始
        /// </summary>
        void DioStart();
        /// <summary>
        /// Dio監視中か?
        /// </summary>
        /// <returns></returns>
        bool IsDioStart();
        /// <summary>
        /// パラメータ読込
        /// </summary>
        /// <param name="mesbox"></param>
        /// <returns></returns>
        bool Load();
    }
}
