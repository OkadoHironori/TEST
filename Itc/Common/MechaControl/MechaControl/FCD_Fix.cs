using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechaControl
{
    /// <summary>
    /// FCDの固定パラメータ
    /// </summary>
    public class FCD_Fix : IFCD_Fix
    {
        /// <summary>
        /// ファイルロード完了
        /// </summary>
        public event EventHandler EndLoadFixParam;
        /// <summary>
        /// スピードリスト
        /// </summary>
        public IEnumerable<SelectSPD> Speedlist { get; private set; }
        /// <summary>
        /// 選択しているスピード
        /// </summary>
        public SelectSPD CurrentSpd { get; private  set; }
        /// <summary>
        /// 回転大テーブル(リング幅)
        /// </summary>
        public float LargeTableRingWidth { get; private set; }
        /// <summary>
        /// テーブルがX線源と干渉する限界FCD ※自動校正用
        /// </summary>
        public float FCD_Limit { get; private set; }
        /// <summary>
        /// FCD最小値
        /// </summary>
        public float Min_fcd { get; private set; }
        /// <summary>
        /// FCD最大値
        /// </summary>
        public float Max_fcd { get; private set; }
        /// <summary>
        /// FCD最小値_Numeric​UpDownクラス用
        /// </summary>
        public float Min_fcd_forNumUD => Min_fcd - 20.0F;//余裕分 mm
        /// <summary>
        /// FCD最大値_Numeric​UpDownクラス用
        /// </summary>
        public float Max_fcd_forNumUD => Max_fcd + 20.0F;//余裕分 mm
        /// <summary>
        /// メッセージ
        /// </summary>
        public IEnumerable<MechaMes> MechaMess { get; private set; }
        /// <summary>
        /// メカコントロールメッセージI/F
        /// </summary>
        private readonly IMechaCtrlMes _MechaCtrlMes;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mcm"></param>
        public FCD_Fix(IMechaCtrlMes mcm)
        {
            _MechaCtrlMes = mcm;
            _MechaCtrlMes.EndLoadFile += (s, e) => 
            {
                var _mcm = s as MechaCtrlMes;
                MechaMess = _mcm.MechaMess;
            };
            _MechaCtrlMes.RequestMes();

            if (!string.IsNullOrEmpty(GetInitMechaparaFileName1()) && !string.IsNullOrEmpty(GetInitMechaparaFileName2()))
            {
                //CSVファイル読込 Tuple
                List<Tuple<string, string>> FObk = File.ReadAllLines(GetInitMechaparaFileName1())
                        .Where(l => !string.IsNullOrEmpty(l))
                        .Select(v => v.Split(','))
                        .Where(j => !string.IsNullOrEmpty(j[1]) && !string.IsNullOrEmpty(j[2]))
                        .Select(c => new Tuple<string, string>(c[1], c[2])).ToList();
                foreach (var Obj in FObk)
                {
                    switch (Obj.Item1)
                    {
                        case ("min_fcd"):
                            Min_fcd = float.Parse(Obj.Item2);
                            break;
                        case ("max_fcd"):
                            Max_fcd = float.Parse(Obj.Item2);
                            break;
                        case ("largeTableRingWidth"):
                            LargeTableRingWidth = float.Parse(Obj.Item2);
                            break;
                    }
                }

                //CSVファイル読込 Tuple
                List<Tuple<string, string, string, string, string>> FObj
                    = File.ReadAllLines(GetInitMechaparaFileName1(),Encoding.GetEncoding("Shift_JIS"))
                        .Where(l => !string.IsNullOrEmpty(l))
                        .Select(v => v.Split(','))
                        .Where(j => !string.IsNullOrEmpty(j[0]) && !string.IsNullOrEmpty(j[1]) && !string.IsNullOrEmpty(j[2]) && !string.IsNullOrEmpty(j[3]) && !string.IsNullOrEmpty(j[4]))
                        .Select(c => new Tuple<string, string, string, string, string>(c[0], c[1], c[2], c[3], c[4])).ToList();

                foreach (var Obj in FObj)
                {
                    switch (Obj.Item2)
                    {
                        case ("fcd_speed"):

                            var querspd_sl = MechaMess.ToList().Find(p => p.ParamName == "SPD_SL");

                            List<SelectSPD> Spd = new List<SelectSPD>()
                            {
                                new SelectSPD()
                                {
                                    SPD = float.Parse(Obj.Item3),
                                    DispName = MechaMess.ToList().Find(p=>p.ParamName=="SPD_SL").Message,
                                },
                                new SelectSPD()
                                {
                                    SPD =  float.Parse(Obj.Item4),
                                    DispName = MechaMess.ToList().Find(p=>p.ParamName=="SPD_L").Message,
                                },
                                new SelectSPD()
                                {
                                    SPD = float.Parse(Obj.Item5),
                                     DispName = MechaMess.ToList().Find(p=>p.ParamName=="SPD_M").Message,
                                },
                                new SelectSPD()
                                {
                                    SPD = float.Parse(Obj.Item5),
                                    DispName = MechaMess.ToList().Find(p=>p.ParamName=="SPD_H").Message,
                                },
                             };
                            Speedlist = Spd;
                            break;
                    }
                }


                CurrentSpd = Speedlist.ElementAt(3);

                //CSVファイル読込 Tuple
                List<Tuple<string, string>> FObc = File.ReadAllLines(GetInitMechaparaFileName2())
                        .Where(l => !string.IsNullOrEmpty(l))
                        .Select(v => v.Split(','))
                        .Where(j => !string.IsNullOrEmpty(j[1]) && !string.IsNullOrEmpty(j[2]))
                        .Select(c => new Tuple<string, string>(c[1], c[2])).ToList();

                foreach (var Obj in FObc)
                {
                    switch (Obj.Item1)
                    {
                        case ("fcd_limit"):
                            FCD_Limit = float.Parse(Obj.Item2);
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// ファイル存在確認
        /// </summary>
        /// <returns></returns>
        private string GetInitMechaparaFileName1()
        {
            string mechapara = Path.Combine("C:\\CT\\SETFILE", "mechapara.csv");
            if (File.Exists(mechapara))
            {
                return mechapara;
            }

            mechapara = Path.Combine(Directory.GetCurrentDirectory(),
                        "TXSParam", "Mecha", "MechaTbl", "mechapara.csv");

            if (File.Exists(mechapara))
            {
                return mechapara;
            }
            else
            {
                throw new Exception($"{mechapara} dosn't exist.");
            }
        }
        /// <summary>
        /// ファイル名
        /// </summary>
        /// <returns></returns>
        private string GetInitMechaparaFileName2()
        {
            string mechapara = Path.Combine("C:\\CT\\SETFILE", "scancondpara.csv");
            if (File.Exists(mechapara))
            {
                return mechapara;
            }

            mechapara = Path.Combine(Directory.GetCurrentDirectory(),
                        "TXSParam", "Mecha", "MechaTbl", "scancondpara.csv");

            if (File.Exists(mechapara))
            {
                return mechapara;
            }
            else
            {
                throw new Exception($"{mechapara} dosn't exist.");
            }
        }

        /// <summary>
        /// メッセージ要求
        /// </summary>
        public void RequestParam() 
            => EndLoadFixParam?.Invoke(this, new EventArgs());
    }
    /// <summary>
    /// FCDの固定パラメータI/F
    /// </summary>
    public interface IFCD_Fix
    {
        /// <summary>
        /// ファイルロード完了
        /// </summary>
        event EventHandler EndLoadFixParam;
        /// <summary>
        /// メッセージ要求
        /// </summary>
        void RequestParam();
    }


    public class SelectSPD
    {
        /// <summary>
        /// 速度
        /// </summary>
        public float SPD { get; set; }
        /// <summary>
        /// 名前
        /// </summary>
        public string DispName { get; set; }
    }
}
