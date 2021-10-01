using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Board.BoardControl
{
    /// <summary>
    /// ボード制御で利用する軸の設定をみるクラス
    /// </summary>
    public class BoardConfig: IBoardConfig
    {
        /// <summary>
        /// TXSフォルダ
        /// </summary>
        private const string TXSFolder = "TXS";
        /// <summary>
        /// Mechaフォルダ
        /// </summary>
        private const string MechaFolder = "Mecha";
        /// <summary>
        /// BoardTableフォルダ
        /// </summary>
        private const string BoardTableFolder = "BoardTable";
        /// <summary>
        /// ファイル名
        /// </summary>
        private const string ConfFile = "BoardConf.csv";
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<BoardChk> Boards { get; private set; }
        /// <summary>
        /// デバイス情報
        /// </summary>
        public IEnumerable<Devinf> DevInfs { get; private set; }
        /// <summary>
        /// ボード設定の読込完了イベント
        /// </summary>
        public event EventHandler EndLoadBoardConf;
        /// <summary>
        /// ボード初期化クラスI/F
        /// </summary>
        private readonly IBoardInit _BoardInit;
        /// <summary>
        /// ボード制御で利用する軸の設定をみるクラス
        /// </summary>
        public BoardConfig(IBoardInit boardInit)
        {
            _BoardInit = boardInit;
            _BoardInit.EndLoadBoard += (s, e) => 
            {
                BoardInit bini = s as BoardInit;
                DevInfs = bini.DevInfs;
            };
            _BoardInit.RequestParam();

            string csvpath = Path.Combine(Directory.GetCurrentDirectory(), TXSFolder, MechaFolder, BoardTableFolder, TXSFolder, ConfFile);
            if (!File.Exists(csvpath)) throw new Exception($"{csvpath}{Environment.NewLine}isn't exist.");

            //CSVファイル読込 Tuple
            List<Tuple<string, string, string>> Fobj = File.ReadAllLines(csvpath)
                    .Where(l => !string.IsNullOrEmpty(l))
                    .Select(v => v.Split(','))
                    .Where(j => !string.IsNullOrEmpty(j[0]) && Regex.IsMatch(j[1], "^(?i)(true|false)$") && Regex.IsMatch(j[2], "^[0-9]+$"))
                    .Select(c => new Tuple<string, string, string>(c[0], c[1], c[2])).ToList();

            List<BoardChk> tmplist = new List<BoardChk>();
            foreach (var Obj in Fobj)
            {
                if (bool.Parse(Obj.Item2))
                {
                    if (Enum.TryParse(Obj.Item1, out BoardAxis res))
                    {
                        var tmp = new BoardChk()
                        {
                            BAxis = res,
                            ID = DevInfs.FirstOrDefault().DevNum,
                            Idx = tmplist.Count(),
                            Axis = (ushort)res,
                        };
                        tmplist.Add(tmp);
                    }
                }
            }
            Boards = tmplist;
        }
        /// <summary>
        /// パラメータ要求
        /// </summary>
        public void RequestParam() 
            => EndLoadBoardConf?.Invoke(this, new EventArgs());

    }
    /// <summary>
    /// ボード制御で利用する軸の設定をみるクラス　I/F
    /// </summary>
    public interface IBoardConfig
    {
        /// <summary>
        /// 設定読込完了
        /// </summary>
        event EventHandler EndLoadBoardConf;
        /// <summary>
        /// パラメータ要求
        /// </summary>
        void RequestParam();
    }
    public class BoardChk
    {
        /// <summary>
        /// 
        /// </summary>
        public BoardAxis BAxis { get; set; }
        /// <summary>
        /// Index
        /// </summary>
        public int Idx { get; set; }
        /// <summary>
        /// ボードID
        /// </summary>
        public uint ID { get; set; }
        /// <summary>
        /// ボード軸
        /// </summary>
        public ushort Axis { get; set; }
    }
    /// <summary>
    /// 命令対象の軸
    /// </summary>
    public enum BoardAxis : int
    {
        /// <summary>
        /// 未割り当て
        /// </summary>
        Non = -1,
        /// <summary>
        /// 昇降
        /// </summary>
        UD_JIKU = 0,
        /// <summary>
        /// 回転
        /// </summary>
        ROT_JIKU = 1,
        /// <summary>
        /// 微調 X軸
        /// </summary>
        YSTG_JIKU = 2,
        /// <summary>
        /// 微調 Y軸
        /// </summary>
        XSTG_JIKU = 3,
        /// <summary>
        ///  傾斜（Ｚ軸）
        /// </summary>
        TILT_JIKU = 4,
        /// <summary>
        ///  傾斜回転（Ｕ軸）
        /// </summary>
        TILTROT_JIKU = 5,
        /// <summary>
        /// ファントム
        /// </summary>
        PHM_JIKU = 10,
        /// <summary>
        ///   X線昇降（Ｘ軸）
        /// </summary>
        XRAYUD_JIKU = 4,
        /// <summary>
        ///  検出器昇降（Ｙ軸）
        /// </summary>
        DETUD_JIKU = 5,
        /// <summary>
        ///  X線、検出器同期昇降（XＹ軸）
        /// </summary>
        SYNCUD_JIKU = 99
    }
}
