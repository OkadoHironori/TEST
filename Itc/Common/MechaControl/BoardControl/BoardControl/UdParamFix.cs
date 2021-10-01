using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Board.BoardControl
{
    public class UdParamFix : IUdParamFix
    {
        /// <summary>
        /// 回転軸ピッチ(mm/P)
        /// </summary>
        public float UdStep { get; set; }
        /// <summary>                                       
        /// 倍率
        /// </summary>
        public float UdMag { get; set; }
        /// <summary>
        /// アドレス増加方向
        /// </summary>
        public int UdDir { get; set; }
        /// <summary>
        /// パルス出力方式
        /// </summary>
        public int UdPlsMode { get; set; }
        /// <summary>
        /// パルス逓倍
        /// </summary>
        public int UdPlsMult { get; set; }
        /// <summary>
        /// 加減速レート(秒)
        /// </summary>
        public float UdRateTime { get; set; }
        /// <summary>
        /// ＦＬ速mm(deg/s)
        /// </summary>
        public float UdSpdFL { get; set; }
        /// <summary>
        /// 手動回転スピード
        /// </summary>
        public float UdManualSpd { get; set; }
        /// <summary>
        /// 手動動作 最高速mm
        /// </summary>
        public float UdManualLimitSpd { get; set; }
        /// <summary>
        /// 手動動作 動作タイムアウト（msec)
        /// </summary>
        public int UdManualTimeOut { get; set; }
        /// <summary>
        /// 位置決め 加減速レート(秒)
        /// </summary>
        public float UdIndexRateTime { get; set; }
        /// <summary>
        /// 位置決め ＦＬ速度(deg/s)
        /// </summary>
        public float UdIndexSpdFL { get; set; }
        /// <summary>
        /// 位置決め 動作速度(rpm)
        /// </summary>
        public float UdIndexSpdFH { get; set; }
        /// <summary>
        /// 位置決め 動作タイムアウト(msec)
        /// </summary>
        public int UdIndexTimeOut { get; set; }
        /// <summary>
        /// 原点復帰 動作速度(mm/s)
        /// </summary>
        public float UdOriginSpdFL { get; set; }
        /// <summary>
        /// 原点復帰 FL速度(mm/s)
        /// </summary>
        public float UdOriginSpdFH { get; set; }
        /// <summary>
        /// 原点復帰 動作タイムアウト時間(秒)
        /// </summary>
        public int UdOriginTimeOut { get; set; }
        /// <summary>
        /// 原点から抜け出す送り量(mm)
        /// </summary>
        public float UdOriginShift { get; set; }
        /// <summary>
        /// 原点位置(mm)
        /// </summary>
        public float UdOriginPos { get; set; }
        /// <summary>
        /// 原点オフセット位置(mm)
        /// </summary>
        public float UdOriginOffset { get; set; }
        /// <summary>
        /// 原点復帰ﾓｰﾄﾞ0:原点ｾﾝｻで停止、2:Z相で停止
        /// </summary>
        public int UdOriginMode { get; set; }
        /// <summary>
        /// 原点復帰方向
        /// </summary>
        public int UdOriginDir { get; set; }
        /// <summary>
        /// 原点復帰方向
        /// </summary>
        public int UdOriginNoChk { get; set; }
        /// <summary>
        /// 停止動作 動作タイムアウト(秒)
        /// </summary>
        public int UdStopTimeOut { get; set; }
        /// <summary>
        /// UdLmtNoChk=1で回転制限無効､0で有効
        /// </summary>
        public int UdLmtNoChk { get; set; } = 1;
        /// <summary>
        /// 正転ｿﾌﾄﾘﾐｯﾄ位置(mm)	
        /// </summary>
        public float UdCwLmtPos { get; set; } = (float)700.0;
        /// <summary>
        /// 逆転ｿﾌﾄﾘﾐｯﾄ位置(mm)	
        /// </summary>
        public float UdCcwLmtPos { get; set; } = (float)-5.0;
        /// <summary>
        /// モーター種類(0:パルス,1:サーボ)
        /// </summary>
        public int UdMotorType { get; set; }
        /// <summary>
        /// 原点センサ極性(0:a接、1:b接)
        /// </summary>
        public int UdOrgPolarity { get; set; }
        /// <summary>
        /// リミットセンサ極性(0:a接、1:b接)
        /// </summary>
        public int UdLmtPolarity { get; set; }
        /// <summary>
        /// 減速センサ極性(0:a接、1:b接)
        /// </summary>
        public int UdDlsPolarity { get; set; }
        /// <summary>
        /// サーボアラーム極性(0:a接、1:b接)
        /// </summary>
        public int UdAlmPolarity { get; set; }
        /// <summary>
        /// リミットセンサ検出時停止方法(0:即停止、1:減速停止)
        /// </summary>
        public int UdElsMode { get; set; }
        /// <summary>
        /// UdLmtNoChk=1で回転制限無効､0で有効
        /// </summary>
        public int UdNoSlowMode { get; set; }
        /// <summary>
        /// CW減速位置(mm)	
        /// </summary>
        public float UdCrLowPos { get; set; }
        /// <summary>
        /// CCW減速位置(mm)	
        /// </summary>
        public float UdCrHiPos { get; set; }
        /// <summary>
        /// 減速時速mm(mm/sec)	
        /// </summary>
        public float UdSlowSpd { get; set; }
        /// <summary>
        /// 制御可能な表示値(string)
        /// </summary>
        public string UdCtrIncrement { get; private set; }
        /// <summary>
        /// 制御可能な表示値
        /// </summary>
        public int UdScale { get; private set; }
        /// <summary>
        /// 最大値
        /// </summary>
        public float UdMaxValue { get; private set; }
        /// <summary>
        /// 最小値
        /// </summary>
        public float UdMinValue { get; private set; }
        /// <summary>
        /// Inpos範囲(度)	
        /// </summary>
        public float UdInpos { get; private set; } = (float)0.10;
        /// <summary>
        /// ボート定数 = 0x00000000
        /// 00000000 00000000 00000000 00000000	//ｺﾝﾊﾟﾚｰﾄ無し
        /// </summary>
        public uint UdEnvLmOf { get; set; } = 0x00000000;    // 
        /// <summary>
        /// ボード定数 =  0x00005054
        /// 00000000 00000000 01010000 01010100	//ｺﾝﾊﾟﾚｰﾄ一致で減速停止	
        /// </summary>
        public uint UdEnvLmOn { get; set; } = 0x00005054;
        /// <summary>
        /// ボード定数 = 0x00000054
        /// 00000000 00000000 00000000 01010100	//ｺﾝﾊﾟﾚｰﾄ一致でCWﾘﾐｯﾄ減速停止無し
        /// </summary>
        public uint UdEnvLmCwOf { get; set; } = 0x00000054;
        /// <summary>
        /// ボード定数  = 0x00005000
        /// 00000000 00000000 01010000 00000000	//ｺﾝﾊﾟﾚｰﾄ一致でCCWﾘﾐｯﾄ減速停止無し
        /// </summary>
        public uint UdEnvLmCcwOf { get; set; } = 0x00005000;
        /// <summary>
        /// 環境変数 1 指令出力方式、信号極性設定
        /// </summary>
        //public uint Env1Data { get; set; } = 0x204348C4;
        public uint UdEnv1Data { get; set; } = 0x20434004;
        /// <summary>
        /// 環境変数 2 /ｴﾝｺｰﾀﾞ1逓倍
        /// </summary>
        public uint UdEnv2Data { get; set; } = 0x0020F555;
        /// <summary>
        /// 環境変数 3 Z層を用いた原点復帰？
        /// </summary>
        public uint UdEnv3Data { get; set; } = 0x00F00002;
        /// <summary>
        /// 環境変数 4 コンパレータ一致時の動作
        /// </summary>
        public uint UdEnv4Data { get; set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// 環境変数 5
        /// </summary>
        public uint UdEnv5Data { get; set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// 環境変数 6
        /// </summary>
        public uint UdEnv6Data { get; set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// 環境変数 7
        /// </summary>
        public uint UdEnv7Data { get; set; } = 0x00000000;  // 00000000 00000000 00000000 00000000

        /// <summary>
        /// ボードパラメータファイルへのパスを取得する。
        /// </summary>
        private static string BoardPath
        {
            get { return Path.Combine(Directory.GetCurrentDirectory(), "TXS", "Mecha", "BoardTable", "TXS", "UpDown.csv"); }
        }
        /// <summary>
        /// 読込完了イベント
        /// </summary>
        public event EventHandler EndLoadUdParam;
        /// <summary>
        /// パラメータ要求
        /// </summary>
        public void Requeset() 
            => EndLoadUdParam?.Invoke(this, new EventArgs());
        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public UdParamFix()
        {

            if (!File.Exists(BoardPath)) throw new Exception($"{BoardPath} dosn't exists");


            //CSVファイル読込 Tuple
            List<Tuple<string, string>> FObj = File.ReadAllLines(BoardPath)
                    .Select(v => v.Split(','))
                    .Where(j => !string.IsNullOrEmpty(j[0]) && !string.IsNullOrEmpty(j[1]))
                    .Select(c => new Tuple<string, string>(c[0], c[1])).ToList();


            if (FObj.Count() == 0) throw new Exception($"Can't Load {BoardPath} File");

            foreach (var Obj in FObj)
            {
                switch (Obj.Item1)
                {
                    case ("UdStep"):
                        UdStep = float.Parse(Obj.Item2);
                        break;
                    case ("UdMag"):
                        UdMag = float.Parse(Obj.Item2);
                        break;
                    case ("UdDir"):
                        UdDir = int.Parse(Obj.Item2);
                        break;
                    case ("UdPlsMode"):
                        UdPlsMode = int.Parse(Obj.Item2);
                        break;
                    case ("UdPlsMult"):
                        UdPlsMult = int.Parse(Obj.Item2);
                        break;
                    case ("UdRateTime"):
                        UdRateTime = float.Parse(Obj.Item2);
                        break;
                    case ("UdSpdFL"):
                        UdSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("UdManualSpd"):
                        UdManualSpd = float.Parse(Obj.Item2);
                        break;
                    case ("UdManualLimitSpd"):
                        UdManualLimitSpd = float.Parse(Obj.Item2);
                        break;
                    case ("UdManualTimeOut"):
                        UdManualTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("UdIndexRateTime"):
                        UdIndexRateTime = float.Parse(Obj.Item2);
                        break;
                    case ("UdIndexSpdFL"):
                        UdIndexSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("UdIndexSpdFH"):
                        UdIndexSpdFH = float.Parse(Obj.Item2);
                        break;
                    case ("UdIndexTimeOut"):
                        UdIndexTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("UdOriginSpdFL"):
                        UdOriginSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("UdOriginSpdFH"):
                        UdOriginSpdFH = float.Parse(Obj.Item2);
                        break;
                    case ("UdOriginTimeOut"):
                        UdOriginTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("UdOriginShift"):
                        UdOriginShift = float.Parse(Obj.Item2);
                        break;
                    case ("UdOriginPos"):
                        UdOriginPos = float.Parse(Obj.Item2);
                        break;
                    case ("UdOriginOffset"):
                        UdOriginOffset = float.Parse(Obj.Item2);
                        break;
                    case ("UdOriginMode"):
                        UdOriginMode = int.Parse(Obj.Item2);
                        break;
                    case ("UdOriginDir"):
                        UdOriginDir = int.Parse(Obj.Item2);
                        break;
                    case ("UdOriginNoChk"):
                        UdOriginNoChk = int.Parse(Obj.Item2);
                        break;
                    case ("UdStopTimeOut"):
                        UdStopTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("UdLmtNoChk"):
                        UdLmtNoChk = int.Parse(Obj.Item2);
                        break;
                    case ("UdCwLmtPos"):
                        UdCwLmtPos = float.Parse(Obj.Item2);
                        break;
                    case ("UdCcwLmtPos"):
                        UdCcwLmtPos = float.Parse(Obj.Item2);
                        break;
                    case ("UdMotorType"):
                        UdMotorType = int.Parse(Obj.Item2);
                        break;
                    case ("UdOrgPolarity"):
                        UdOrgPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("UdLmtPolarity"):
                        UdLmtPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("UdDlsPolarity"):
                        UdDlsPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("UdAlmPolarity"):
                        UdAlmPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("UdElsMode"):
                        UdElsMode = int.Parse(Obj.Item2);
                        break;
                    case ("UdNoSlowMode"):
                        UdNoSlowMode = int.Parse(Obj.Item2);
                        break;
                    case ("UdCrLowPos"):
                        UdCrLowPos = float.Parse(Obj.Item2);
                        break;
                    case ("UdCrHiPos"):
                        UdCrHiPos = float.Parse(Obj.Item2);
                        break;
                    case ("UdSlowSpd"):
                        UdSlowSpd = float.Parse(Obj.Item2);
                        break;

                    case ("UdCtrIncrement"):

                        UdCtrIncrement = Obj.Item2;

                        double input = Convert.ToDouble(UdCtrIncrement);

                        double decimal_part = input % 1;

                        string output = Convert.ToString(decimal_part);

                        UdScale = (output.Length - 2);

                        break;

                    case ("UdMaxValue"):
                        UdMaxValue = float.Parse(Obj.Item2);
                        break;

                    case ("UdMinValue"):
                        UdMinValue = float.Parse(Obj.Item2);
                        break;
                    case ("UdInpos"):
                        UdInpos = float.Parse(Obj.Item2);
                        break;
                }
            }
        }
    }
    /// <summary>
    ///　昇降パラメータ読込 I/F
    /// </summary>
    public interface IUdParamFix
    {
        /// <summary>
        /// 読込完了イベント
        /// </summary>
        event EventHandler EndLoadUdParam;
        /// <summary>
        /// パラメータ要求
        /// </summary>
        void Requeset();

    }
}
