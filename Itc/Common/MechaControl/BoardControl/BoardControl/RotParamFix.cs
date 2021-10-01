using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Board.BoardControl
{
    public class RotParamFix : IRotParamFix
    {
        /// <summary>
        /// パラメータ読込イベント
        /// </summary>
        public event EventHandler EndLoadRotParam;
        /// <summary>
        /// 回転軸ピッチ(度/P)
        /// </summary>
        public float RotStep { get; private set; }
        /// <summary>                                       
        /// 倍率
        /// </summary>
        public float RotMag { get; private set; }
        /// <summary>
        /// アドレス増加方向
        /// </summary>
        public int RotDir { get; private set; }
        /// <summary>
        /// パルス出力方式
        /// </summary>
        public int RotPlsMode { get; private set; }
        /// <summary>
        /// パルス逓倍
        /// </summary>
        public int RotPlsMult { get; private set; }
        /// <summary>
        /// 加減速レート(秒)
        /// </summary>
        public float RotRateTime { get; private set; }
        /// <summary>
        /// ＦＬ速度(deg/s)
        /// </summary>
        public float RotSpdFL { get; private set; }
        /// <summary>
        /// 手動回転スピード
        /// </summary>
        public float RotManualSpd { get; private set; }
        /// <summary>
        /// 手動動作 最高速度
        /// </summary>
        public float RotManualLimitSpd { get; private set; }
        /// <summary>
        /// 手動動作 動作タイムアウト（msec)
        /// </summary>
        public int RotManualTimeOut { get; private set; }
        /// <summary>
        /// 位置決め 加減速レート(秒)
        /// </summary>
        public float RotIndexRateTime { get; private set; }
        /// <summary>
        /// 位置決め ＦＬ速度(deg/s)
        /// </summary>
        public float RotIndexSpdFL { get; private set; }
        /// <summary>
        /// 位置決め 動作速度(rpm)
        /// </summary>
        public float RotIndexSpdFH { get; private set; }
        /// <summary>
        /// 位置決め 動作タイムアウト(msec)
        /// </summary>
        public int RotIndexTimeOut { get; private set; }
        /// <summary>
        /// 原点復帰 動作速度(deg/s)
        /// </summary>
        public float RotOriginSpdFL { get; private set; }
        /// <summary>
        /// 原点復帰 FL速度(deg/s)
        /// </summary>
        public float RotOriginSpdFH { get; private set; }
        /// <summary>
        /// 原点復帰 動作タイムアウト時間(秒)
        /// </summary>
        public int RotOriginTimeOut { get; private set; }
        /// <summary>
        /// 原点から抜け出す送り量(度)
        /// </summary>
        public float RotOriginShift { get; private set; }
        /// <summary>
        /// 原点位置(度)
        /// </summary>
        public float RotOriginPos { get; private set; }
        /// <summary>
        /// 原点オフセット位置(度)
        /// </summary>
        public float RotOriginOffset { get; private set; }
        /// <summary>
        /// 原点復帰ﾓｰﾄﾞ0:原点ｾﾝｻで停止、2:Z相で停止
        /// </summary>
        public int RotOriginMode { get; private set; }
        /// <summary>
        /// 原点復帰方向
        /// </summary>
        public int RotOriginDir { get; private set; }
        /// <summary>
        /// 原点復帰方向
        /// </summary>
        public int RotOriginNoChk { get; set; }
        /// <summary>
        /// 停止動作 動作タイムアウト(秒)
        /// </summary>
        public int RotStopTimeOut { get; private set; }
        /// <summary>
        /// RotLmtNoChk=1で回転制限無効､0で有効
        /// </summary>
        public int RotLmtNoChk { get; private set; } = 1;
        /// <summary>
        /// 正転ｿﾌﾄﾘﾐｯﾄ位置(度)	
        /// </summary>
        public float RotCwLmtPos { get; private set; } = (float)380.0;
        /// <summary>
        /// 逆転ｿﾌﾄﾘﾐｯﾄ位置(度)	
        /// </summary>
        public float RotCcwLmtPos { get; private set; } = (float)-20.0;
        /// <summary>
        /// モーター種類(0:パルス,1:サーボ)
        /// </summary>
        public int RotMotorType { get; private set; }
        /// <summary>
        /// 原点センサ極性(0:a接、1:b接)
        /// </summary>
        public int RotOrgPolarity { get; private set; }
        /// <summary>
        /// リミットセンサ極性(0:a接、1:b接)
        /// </summary>
        public int RotLmtPolarity { get; private set; }
        /// <summary>
        /// 減速センサ極性(0:a接、1:b接)
        /// </summary>
        public int RotDlsPolarity { get; private set; }
        /// <summary>
        /// サーボアラーム極性(0:a接、1:b接)
        /// </summary>
        public int RotAlmPolarity { get; private set; }
        /// <summary>
        /// リミットセンサ検出時停止方法(0:即停止、1:減速停止)
        /// </summary>
        public int RotElsMode { get; private set; }
        /// <summary>
        /// RotLmtNoChk=1で回転制限無効､0で有効
        /// </summary>
        public int RotNoSlowMode { get; private set; }
        /// <summary>
        /// CW減速位置(度)	
        /// </summary>
        public float RotCrLowPos { get; private set; }
        /// <summary>
        /// CCW減速位置(度)	
        /// </summary>
        public float RotCrHiPos { get; private set; }
        /// <summary>
        /// 減速時速度(度/sec)	
        /// </summary>
        public float RotSlowSpd { get; private set; }
        /// <summary>
        /// 制御可能な表示値(string)
        /// </summary>
        public string RotCtrIncrement { get; private set; }
        /// <summary>
        /// 制御可能な表示値
        /// </summary>
        public int RotScale { get; private set; }
        /// <summary>
        /// 最大値
        /// </summary>
        public float RotMaxValue { get; private set; }
        /// <summary>
        /// 最小値
        /// </summary>
        public float RotMinValue { get; private set; }
        /// <summary>
        /// Inpos範囲(度)	
        /// </summary>
        public float RotInpos { get; private set; } = (float)0.10;
        /// <summary>
        /// ボート定数 = 0x00000000
        /// 00000000 00000000 00000000 00000000	//ｺﾝﾊﾟﾚｰﾄ無し
        /// </summary>
        public uint RotEnvLmOf { get; } = 0x00000000;    // 
        /// <summary>
        /// ボード定数 =  0x00005054
        /// 00000000 00000000 01010000 01010100	//ｺﾝﾊﾟﾚｰﾄ一致で減速停止	
        /// </summary>
        public uint RotEnvLmOn { get; } = 0x00005054;
        /// <summary>
        /// ボード定数 = 0x00000054
        /// 00000000 00000000 00000000 01010100	//ｺﾝﾊﾟﾚｰﾄ一致でCWﾘﾐｯﾄ減速停止無し
        /// </summary>
        public uint RotEnvLmCwOf { get; } = 0x00000054;
        /// <summary>
        /// ボード定数  = 0x00005000
        /// 00000000 00000000 01010000 00000000	//ｺﾝﾊﾟﾚｰﾄ一致でCCWﾘﾐｯﾄ減速停止無し
        /// </summary>
        public uint RotEnvLmCcwOf { get; } = 0x00005000;
        /// <summary>
        /// 環境変数 1 指令出力方式、信号極性設定
        /// </summary>
        //public uint Env1Data { get; set; } = 0x204348C4;
        public uint RotEnv1Data { get; set; } = 0x20434004;
        /// <summary>
        /// 環境変数 2 /ｴﾝｺｰﾀﾞ1逓倍
        /// </summary>
        public uint RotEnv2Data { get; set; } = 0x0020F555;
        /// <summary>
        /// 環境変数 3 Z層を用いた原点復帰？
        /// </summary>
        public uint RotEnv3Data { get; set; } = 0x00F00002;
        /// <summary>
        /// 環境変数 4 コンパレータ一致時の動作
        /// </summary>
        public uint RotEnv4Data { get; set; } = 0x00000000;
        /// <summary>
        /// 環境変数 5
        /// </summary>
        public uint RotEnv5Data { get; set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// 環境変数 6
        /// </summary>
        public uint RotEnv6Data { get; set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// 環境変数 7
        /// </summary>
        public uint RotEnv7Data { get; set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// ボードパラメータファイルへのパスを取得する。
        /// </summary>
        private static string BoardPath
        {
            get { return Path.Combine(Directory.GetCurrentDirectory(), "TXS", "Mecha", "BoardTable","TXS", "Rotation.csv"); }
        }
        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public RotParamFix()
        {
            if (!File.Exists(BoardPath))
            {
                throw new Exception($"{BoardPath}{Environment.NewLine} dosn't exists");
            }
            //CSVファイル読込 Tuple
            List<Tuple<string, string>> FObj = File.ReadAllLines(BoardPath)
                    .Select(v => v.Split(','))
                    .Where(j => !string.IsNullOrEmpty(j[0]) && !string.IsNullOrEmpty(j[1]))
                    .Select(c => new Tuple<string, string>(c[0], c[1])).ToList();


            if (FObj.Count() == 0)
            {   //エラーメッセージ表示
                throw new Exception($"Can't Load {BoardPath} File");
            }
            foreach (var Obj in FObj)
            {
                switch (Obj.Item1)
                {
                    case ("RotStep"):
                        RotStep = float.Parse(Obj.Item2);
                        break;
                    case ("RotMag"):
                        RotMag = float.Parse(Obj.Item2);
                        break;
                    case ("RotDir"):
                        RotDir = int.Parse(Obj.Item2);
                        break;
                    case ("RotPlsMode"):
                        RotPlsMode = int.Parse(Obj.Item2);
                        break;
                    case ("RotPlsMult"):
                        RotPlsMult = int.Parse(Obj.Item2);
                        break;
                    case ("RotRateTime"):
                        RotRateTime = float.Parse(Obj.Item2);
                        break;
                    case ("RotSpdFL"):
                        RotSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("RotManualSpd"):
                        RotManualSpd = float.Parse(Obj.Item2);
                        break;
                    case ("RotManualLimitSpd"):
                        RotManualLimitSpd = float.Parse(Obj.Item2);
                        break;
                    case ("RotManualTimeOut"):
                        RotManualTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("RotIndexRateTime"):
                        RotIndexRateTime = float.Parse(Obj.Item2);
                        break;
                    case ("RotIndexSpdFL"):
                        RotIndexSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("RotIndexSpdFH"):
                        RotIndexSpdFH = float.Parse(Obj.Item2);
                        break;
                    case ("RotIndexTimeOut"):
                        RotIndexTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("RotOriginSpdFL"):
                        RotOriginSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("RotOriginSpdFH"):
                        RotOriginSpdFH = float.Parse(Obj.Item2);
                        break;
                    case ("RotOriginTimeOut"):
                        RotOriginTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("RotOriginShift"):
                        RotOriginShift = float.Parse(Obj.Item2);
                        break;
                    case ("RotOriginPos"):
                        RotOriginPos = float.Parse(Obj.Item2);
                        break;
                    case ("RotOriginOffset"):
                        RotOriginOffset = float.Parse(Obj.Item2);
                        break;
                    case ("RotOriginMode"):
                        RotOriginMode = int.Parse(Obj.Item2);
                        break;
                    case ("RotOriginDir"):
                        RotOriginDir = int.Parse(Obj.Item2);
                        break;
                    case ("RotOriginNoChk"):
                        RotOriginNoChk = int.Parse(Obj.Item2);
                        break;
                    case ("RotStopTimeOut"):
                        RotStopTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("RotLmtNoChk"):
                        RotLmtNoChk = int.Parse(Obj.Item2);
                        break;
                    case ("RotCwLmtPos"):
                        RotCwLmtPos = float.Parse(Obj.Item2);
                        break;
                    case ("RotCcwLmtPos"):
                        RotCcwLmtPos = float.Parse(Obj.Item2);
                        break;
                    case ("RotMotorType"):
                        RotMotorType = int.Parse(Obj.Item2);
                        break;
                    case ("RotOrgPolarity"):
                        RotOrgPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("RotLmtPolarity"):
                        RotLmtPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("RotDlsPolarity"):
                        RotDlsPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("RotAlmPolarity"):
                        RotAlmPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("RotElsMode"):
                        RotElsMode = int.Parse(Obj.Item2);
                        break;
                    case ("RotNoSlowMode"):
                        RotNoSlowMode = int.Parse(Obj.Item2);
                        break;
                    case ("RotCrLowPos"):
                        RotCrLowPos = float.Parse(Obj.Item2);
                        break;
                    case ("RotCrHiPos"):
                        RotCrHiPos = float.Parse(Obj.Item2);
                        break;
                    case ("RotSlowSpd"):
                        RotSlowSpd = float.Parse(Obj.Item2);
                        break;
                    case ("RotCtrIncrement"):

                        RotCtrIncrement = Obj.Item2;

                        double input = Convert.ToDouble(RotCtrIncrement);

                        double decimal_part = input % 1;

                        string output = Convert.ToString(decimal_part);

                        RotScale = (output.Length - 2);

                        break;
                    case ("RotMaxValue"):
                        RotMaxValue = float.Parse(Obj.Item2);
                        break;
                    case ("RotMinValue"):
                        RotMinValue = float.Parse(Obj.Item2);
                        break;
                    case ("RotInpos"):
                        RotInpos = float.Parse(Obj.Item2);
                        break;
                }
            }        
        }
        /// <summary>
        /// パラメータ要求
        /// </summary>
        public void RequestParam()
            => EndLoadRotParam?.Invoke(this, new EventArgs());
    }
    /// <summary>
    /// ROTパラメータ I/F
    /// </summary>
    public interface IRotParamFix
    {
        /// <summary>
        /// パラメータ読込イベント
        /// </summary>
        event EventHandler EndLoadRotParam;
        /// <summary>
        /// パラメータ要求
        /// </summary>
        void RequestParam();
    }

}
