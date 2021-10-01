using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechaControl
{
    public class MechaCtrlMes : IMechaCtrlMes
    {
        /// <summary>
        /// ファイルロード完了
        /// </summary>
        public event EventHandler EndLoadFile;
        /// <summary>
        /// フォルダ
        /// </summary>
        private readonly string FolderPath = "MechaCtrlMess";
        /// <summary>
        /// ファイル
        /// </summary>
        private readonly string FileName = "MecaControlMeg.csv";
        /// <summary>
        /// メッセージ
        /// </summary>
        public IEnumerable<MechaMes> MechaMess { get; private set; }
        /// <summary>
        /// メカコントロールメッセージクラス
        /// </summary>
        public MechaCtrlMes()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(),"TXS","Mecha", FolderPath, FileName);


            if (!File.Exists(filePath)) throw new Exception($"{filePath}{Environment.NewLine}is not exists");


            //CSVファイル読込 Tuple
            List<Tuple<string, string>> fileObj = File.ReadAllLines(filePath, Encoding.GetEncoding("Shift_JIS"))
                    .Where(l => !string.IsNullOrEmpty(l))
                    .Select(v => v.Split(','))
                    .Where(j => !string.IsNullOrEmpty(j[0]) && !string.IsNullOrEmpty(j[1]))
                    .Select(c => new Tuple<string, string>(c[0], c[1])).ToList();

            List<MechaMes> tmpMess = new List<MechaMes>();

            foreach (var Obj in fileObj)
            {
                MechaMes mes = new MechaMes()
                {
                    ParamName = Obj.Item1,
                    Message = Obj.Item2,
                };
                tmpMess.Add(mes);
            }

            MechaMess = tmpMess;

        }
        /// <summary>
        /// メッセージ要求
        /// </summary>
        public void RequestMes() 
            => EndLoadFile?.Invoke(this, new EventArgs());

    }

    public class MechaMes
    {
        /// <summary>
        /// パラメータ名
        /// </summary>
        public string ParamName { get; set; }
        /// <summary>
        /// メッセージ
        /// </summary>
        public string Message { get; set; }
    }
    /// <summary>
    /// メッセージの読込I/F
    /// </summary>
    public interface IMechaCtrlMes
    {
        /// <summary>
        /// ファイルロード完了
        /// </summary>
        event EventHandler EndLoadFile;
        /// <summary>
        /// メッセージ要求
        /// </summary>
        void RequestMes();
    }
}
