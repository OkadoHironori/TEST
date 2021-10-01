using Itc.Common.TXEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXSMechaControl.UpDown
{
    public class Ud_Param
    {
        /// <summary>
        /// スピードリスト
        /// </summary>
        public IEnumerable<SelectSPD> Speedlist { get; private set; }
        /// <summary>
        /// 選択しているスピード
        /// </summary>
        public SelectSPD CurrentSpd { get; set; } = new SelectSPD();
        /// <summary>
        /// スピード
        /// </summary>
        public float SPD { get; set; } = TXSMecha.Default.UdSPD;
        /// <summary>
        /// パラメータ読込
        /// </summary>
        /// <returns></returns>
        public bool SetParam()
        {
            if (!string.IsNullOrEmpty(GetInitMechaparaFileName1()))
            {
                //CSVファイル読込 Tuple
                List<Tuple<string, string, string, string, string>> FObj = File.ReadAllLines(GetInitMechaparaFileName1())
                        .Where(l => !string.IsNullOrEmpty(l))
                        .Select(v => v.Split(','))
                        .Where(j => !string.IsNullOrEmpty(j[1]) && !string.IsNullOrEmpty(j[2]) && !string.IsNullOrEmpty(j[3]) && !string.IsNullOrEmpty(j[4]) && !string.IsNullOrEmpty(j[5]))
                        .Select(c => new Tuple<string, string, string, string, string>(c[1], c[2], c[3], c[4], c[5])).ToList();

                foreach (var Obj in FObj)
                {
                    switch (Obj.Item1)
                    {
                        case ("ud_speed"):

                            List<SelectSPD> Spd = new List<SelectSPD>()
                            {
                                new SelectSPD()
                                {
                                    SPD = float.Parse(Obj.Item2),
                                    DispName = MechaRes.SPD_SL,
                                },
                                new SelectSPD()
                                {
                                    SPD =  float.Parse(Obj.Item3),
                                    DispName = MechaRes.SPD_L,
                                },
                                new SelectSPD()
                                {
                                    SPD = float.Parse(Obj.Item4),
                                    DispName = MechaRes.SPD_M,
                                },
                                new SelectSPD()
                                {
                                    SPD = float.Parse(Obj.Item5),
                                    DispName = MechaRes.SPD_H,
                                },
                             };
                            Speedlist = Spd;
                            CurrentSpd.SPD = SPD;
                            break;
                    }
                }
                return true;
            }

            return false;
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="spd"></param>
        public void ChangedSaveSpd(float spd)
        {
            TXSMecha.Default.UdSPD = spd;
            TXSMecha.Default.Save();
            CurrentSpd.SPD = spd;
            SPD = spd;
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
    }
}
