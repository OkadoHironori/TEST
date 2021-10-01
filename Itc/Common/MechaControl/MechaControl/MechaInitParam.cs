using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechaControl
{
    /// <summary>
    /// Mecha初期化パラメータクラス
    /// </summary>
    public class MechaInitParam : IMechaInitParam
    {
        /// <summary>
        /// ScanInhibitのパス
        /// </summary>
        private readonly string InhibitPath = @"C:\CT\SETFILE\scaninhibit.csv";
        /// <summary>
        /// ScanInhibitのパス
        /// </summary>
        private readonly string CondparaPath = @"C:\CT\SETFILE\scancondpara.csv";
        /// <summary>
        /// ScanInhibitのパス
        /// </summary>
        private readonly string MechaparaPath = @"C:\CT\SETFILE\mechapara.csv";
        /// <summary>
        ///  X線外部制御	Rev3.00	
        /// </summary>
        public int Xray_remote { get;private set; }
        /// <summary>
        /// 微調テーブル_X軸有無
        /// </summary>
        public int Fine_table_x { get; private set; }
        /// <summary>
        /// 微調テーブル_Y軸有無
        /// </summary>
        public int Fine_table_y { get; private set; }
        /// <summary>
        /// 微調テーブル_Y軸有無
        /// </summary>
        public bool Use_FlatPanel { get; private set; }
        /// <summary>
        /// テーブルとＸ線管のどちらがＸ軸として移動するか
        /// </summary>
        public int Table_x { get; set; }
        ///// <summary>
        ///// 昇降上限値
        ///// </summary>
        //public int Upper_limit { get; set; }
        ///// <summary>
        ///// 昇降下限値
        ///// </summary>
        //public int Lower_limit { get; set; }
        /// <summary>
        /// 回転スピード
        /// </summary>
        public float[] Rot_Speed { get; private set; }
        /// <summary>
        /// 昇降スピード
        /// </summary>
        public float[] Ud_Speed { get; private set; }
        /// <summary>
        /// FDD軸スピード
        /// </summary>
        public float[] FDD_Speed { get; private set; }
        /// <summary>
        /// FCD軸スピード
        /// </summary>
        //public float[] FCD_Speed { get; set; }
        /// <summary>
        /// テーブルY軸移動速度
        /// </summary>
        public float[] Tbl_y_Speed { get; private set; }
        /// <summary>
        /// 微調テーブル移動速度
        /// </summary>
        public float[] Fine_Tbl_Speed { get; private set; }
        /// <summary>
        /// スライスコリメータ移動速度
        /// </summary>
        public float[] Collimator_Speed { get; private set; }
        /// <summary>
        /// Ｘ線管回転速度
        /// </summary>
        public float[] Xray_rot_Speed { get; private set; }
        /// <summary>
        /// Ｘ線管Ｘ軸移動速度
        /// </summary>
        public float[] Xray_x_Speed { get; private set; }
        /// <summary>
        /// Ｘ線管Ｙ軸移動速度
        /// </summary>
        public float[] Xray_y_Speed { get; private set; }
        /// <summary>
        /// チルト機構　回転傾斜テーブル(傾斜)
        /// </summary>
        public float[] Tilt_and_rot_tilt_Speed { get; private set; }
        /// <summary>
        /// チルト機構　回転傾斜テーブル(回転)
        /// </summary>
        public float[] Tilt_and_rot_rot_Speed { get; private set; }
        /// <summary>
        /// Mecha初期化パラメータクラス
        /// </summary>
        public MechaInitParam()
        {
            if (!File.Exists(InhibitPath))
            {
                throw new Exception($"{InhibitPath} dosn't exists");
            }
            else
            {
                //CSVファイル読込 Tuple
                List<Tuple<string, string>> FObj = File.ReadAllLines(InhibitPath)
                        .Where(l => !string.IsNullOrEmpty(l))
                        .Select(v => v.Split(','))
                        .Where(j => !string.IsNullOrEmpty(j[1]) && !string.IsNullOrEmpty(j[2]))
                        .Select(c => new Tuple<string, string>(c[1], c[2])).ToList();

                foreach (var Obj in FObj)
                {
                    switch (Obj.Item1)
                    {
                        case ("xray_remote"):
                            Xray_remote = int.Parse(Obj.Item2);
                            break;
                        case ("fine_table_x"):
                            Fine_table_x = int.Parse(Obj.Item2);
                            break;
                        case ("fine_table_y"):
                            Fine_table_y = int.Parse(Obj.Item2);
                            break;
                        case ("table_x"):
                            Table_x = int.Parse(Obj.Item2);
                            break;
                    }
                }
            }

            if (!File.Exists(CondparaPath))
            {
                throw new Exception($"{CondparaPath} dosn't exists");
            }
            else
            {
                //CSVファイル読込 Tuple
                List<Tuple<string, string>> FObj = File.ReadAllLines(CondparaPath)
                        .Where(l => !string.IsNullOrEmpty(l))
                        .Select(v => v.Split(','))
                        .Where(j => !string.IsNullOrEmpty(j[1]) && !string.IsNullOrEmpty(j[2]))
                        .Select(c => new Tuple<string, string>(c[1], c[2])).ToList();

                foreach (var Obj in FObj)
                {
                    switch (Obj.Item1)
                    {
                        case ("detector"):
                            Use_FlatPanel = 0 == int.Parse(Obj.Item2) ? false : true;
                            break;
                        case ("fcd_limit"):
                            //FCD_Limit = float.Parse(Obj.Item2);
                            break;
                    }
                }
            }

            if (!File.Exists(MechaparaPath))
            {
                throw new Exception($"{MechaparaPath} dosn't exists");
            }
            else
            {
                List<Tuple<string, string, string, string>> FObj = File.ReadAllLines(MechaparaPath)
                                        .Where(l => !string.IsNullOrEmpty(l))
                                        .Select(v => v.Split(','))
                                        .Where(j => !string.IsNullOrEmpty(j[1]) && !string.IsNullOrEmpty(j[2]) && !string.IsNullOrEmpty(j[3]) && !string.IsNullOrEmpty(j[4]))
                                        .Select(c => new Tuple<string, string, string, string>(c[1], c[2], c[3], c[4])).ToList();

                foreach (var Obj in FObj)
                {
                    switch (Obj.Item1)
                    {
                        //case ("upper_limit"):
                        //    Upper_limit = int.Parse(Obj.Item2);
                        //    break;
                        //case ("lower_limit"):
                        //    Lower_limit = int.Parse(Obj.Item2);
                        //    break;
                        case ("rot_speed"):
                            Rot_Speed = new float[4];
                            Rot_Speed[0] = float.Parse(Obj.Item2);
                            Rot_Speed[1] = float.Parse(Obj.Item3);
                            Rot_Speed[2] = float.Parse(Obj.Item4);
                            //Rot_Speed[3] = float.Parse(Obj.Item5);
                            break;
                        case ("ud_speed"):
                            Ud_Speed = new float[4];
                            Ud_Speed[0] = float.Parse(Obj.Item2);
                            Ud_Speed[1] = float.Parse(Obj.Item3);
                            Ud_Speed[2] = float.Parse(Obj.Item4);
                            //Ud_Speed[3] = float.Parse(Obj.Item5);
                            break;
                        //case ("fcd_speed"):
                        //    FCD_Speed = new float[4];
                        //    FCD_Speed[0] = float.Parse(Obj.Item2);
                        //    FCD_Speed[1] = float.Parse(Obj.Item3);
                        //    FCD_Speed[2] = float.Parse(Obj.Item4);
                        //    FCD_Speed[3] = float.Parse(Obj.Item5);
                        //    break;
                        case ("fdd_speed"):
                            FDD_Speed = new float[4];
                            FDD_Speed[0] = float.Parse(Obj.Item2);
                            FDD_Speed[1] = float.Parse(Obj.Item3);
                            FDD_Speed[2] = float.Parse(Obj.Item4);
                            //FDD_Speed[3] = float.Parse(Obj.Item5);
                            break;
                        case ("tbl_y_speed"):
                            Tbl_y_Speed = new float[4];
                            Tbl_y_Speed[0] = float.Parse(Obj.Item2);
                            Tbl_y_Speed[1] = float.Parse(Obj.Item3);
                            Tbl_y_Speed[2] = float.Parse(Obj.Item4);
                            //Tbl_y_Speed[3] = float.Parse(Obj.Item5);
                            break;
                        case ("fine_tbl_speed"):
                            Fine_Tbl_Speed = new float[4];
                            Fine_Tbl_Speed[0] = float.Parse(Obj.Item2);
                            Fine_Tbl_Speed[1] = float.Parse(Obj.Item3);
                            Fine_Tbl_Speed[2] = float.Parse(Obj.Item4);
                            //Fine_Tbl_Speed[3] = float.Parse(Obj.Item5);
                            break;
                        case ("collimator_speed"):
                            Collimator_Speed = new float[4];
                            Collimator_Speed[0] = float.Parse(Obj.Item2);
                            Collimator_Speed[1] = float.Parse(Obj.Item3);
                            Collimator_Speed[2] = float.Parse(Obj.Item4);
                            //Collimator_Speed[3] = float.Parse(Obj.Item5);
                            break;
                        case ("xray_rot_speed"):
                            Xray_rot_Speed = new float[4];
                            Xray_rot_Speed[0] = float.Parse(Obj.Item2);
                            Xray_rot_Speed[1] = float.Parse(Obj.Item3);
                            Xray_rot_Speed[2] = float.Parse(Obj.Item4);
                            //Xray_rot_Speed[3] = float.Parse(Obj.Item5);
                            break;
                        case ("xray_x_speed"):
                            Xray_x_Speed = new float[4];
                            Xray_x_Speed[0] = float.Parse(Obj.Item2);
                            Xray_x_Speed[1] = float.Parse(Obj.Item3);
                            Xray_x_Speed[2] = float.Parse(Obj.Item4);
                            //Xray_x_Speed[3] = float.Parse(Obj.Item5);
                            break;
                        case ("xray_y_speed"):
                            Xray_y_Speed = new float[4];
                            Xray_y_Speed[0] = float.Parse(Obj.Item2);
                            Xray_y_Speed[1] = float.Parse(Obj.Item3);
                            Xray_y_Speed[2] = float.Parse(Obj.Item4);
                            //Xray_y_Speed[3] = float.Parse(Obj.Item5);
                            break;
                        case ("tilt_and_rot_tilt_speed"):
                            Tilt_and_rot_tilt_Speed = new float[4];
                            Tilt_and_rot_tilt_Speed[0] = float.Parse(Obj.Item2);
                            Tilt_and_rot_tilt_Speed[1] = float.Parse(Obj.Item3);
                            Tilt_and_rot_tilt_Speed[2] = float.Parse(Obj.Item4);
                            // Tilt_and_rot_tilt_Speed[3] = float.Parse(Obj.Item5);
                            break;
                        case ("tilt_and_rot_rot_speed"):
                            Tilt_and_rot_rot_Speed = new float[4];
                            Tilt_and_rot_rot_Speed[0] = float.Parse(Obj.Item2);
                            Tilt_and_rot_rot_Speed[1] = float.Parse(Obj.Item3);
                            Tilt_and_rot_rot_Speed[2] = float.Parse(Obj.Item4);
                            //Tilt_and_rot_rot_Speed[3] = float.Parse(Obj.Item5);
                            break;
                            //case ("min_fcd"):
                            //    Min_fcd = float.Parse(Obj.Item2);
                            //    break;
                            //case ("max_fcd"):
                            //    Max_fcd = float.Parse(Obj.Item2);
                            //    break;
                            //case ("min_fdd"):
                            //    Min_fdd = float.Parse(Obj.Item2);
                            //    break;
                            //case ("max_fdd"):
                            //    Max_fdd = float.Parse(Obj.Item2);
                            //    break;
                            //case ("largeTableRingWidth"):
                            //    LargeTableRingWidth = float.Parse(Obj.Item2);
                            //    break;
                    }
                }
            }
        }
    }
    public interface IMechaInitParam
    {
    }

}
