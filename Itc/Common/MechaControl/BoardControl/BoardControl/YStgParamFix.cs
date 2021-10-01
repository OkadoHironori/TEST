using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Board.BoardControl
{
    public class YStgParamFix: IYStgParamFix
    {
        /// <summary>
        /// 回転軸ピッチ(mm/P)
        /// </summary>
        public float YStgStep { get; set; }
        /// <summary>                                       
        /// 倍率
        /// </summary>
        public float YStgMag { get; set; }
        /// <summary>
        /// アドレス増加方向
        /// </summary>
        public int YStgDir { get; set; }
        /// <summary>
        /// パルス出力方式
        /// </summary>
        public int YStgPlsMode { get; set; }
        /// <summary>
        /// パルス逓倍
        /// </summary>
        public int YStgPlsMult { get; set; }
        /// <summary>
        /// 加減速レート(秒)
        /// </summary>
        public float YStgRateTime { get; set; }
        /// <summary>
        /// ＦＬ速mm(mm/s)
        /// </summary>
        public float YStgSpdFL { get; set; }
        /// <summary>
        /// 手動回転スピード
        /// </summary>
        public float YStgManualSpd { get; set; }
        /// <summary>
        /// 手動動作 最高速度
        /// </summary>
        public float YStgManualLimitSpd { get; set; }
        /// <summary>
        /// 手動動作 動作タイムアウト（msec)
        /// </summary>
        public int YStgManualTimeOut { get; set; }
        /// <summary>
        /// 位置決め 加減速レート(秒)
        /// </summary>
        public float YStgIndexRateTime { get; set; }
        /// <summary>
        /// 位置決め ＦＬ速度(mm/s)
        /// </summary>
        public float YStgIndexSpdFL { get; set; }
        /// <summary>
        /// 位置決め 動作速度(rpm)
        /// </summary>
        public float YStgIndexSpdFH { get; set; }
        /// <summary>
        /// 位置決め 動作タイムアウト(msec)
        /// </summary>
        public int YStgIndexTimeOut { get; set; }
        /// <summary>
        /// 原点復帰 動作速度(mm/s)
        /// </summary>
        public float YStgOriginSpdFL { get; set; }
        /// <summary>
        /// 原点復帰 FL速度(mm/s)
        /// </summary>
        public float YStgOriginSpdFH { get; set; }
        /// <summary>
        /// 原点復帰 動作タイムアウト時間(秒)
        /// </summary>
        public int YStgOriginTimeOut { get; set; }
        /// <summary>
        /// 原点から抜け出す送り量(mm)
        /// </summary>
        public float YStgOriginShift { get; set; }
        /// <summary>
        /// 原点位置(mm)
        /// </summary>
        public float YStgOriginPos { get; set; }
        /// <summary>
        /// 原点オフセット位置(mm)
        /// </summary>
        public float YStgOriginOffset { get; set; }
        /// <summary>
        /// 原点復帰ﾓｰﾄﾞ0:原点ｾﾝｻで停止、2:Z相で停止
        /// </summary>
        public int YStgOriginMode { get; set; }
        /// <summary>
        /// 原点復帰方向
        /// </summary>
        public int YStgOriginDir { get; set; }
        /// <summary>
        /// 原点復帰方向
        /// </summary>
        public int YStgOriginNoChk { get; set; }
        /// <summary>
        /// 停止動作 動作タイムアウト(秒)
        /// </summary>
        public int YStgStopTimeOut { get; set; }
        /// <summary>
        /// YStgLmtNoChk=1で回転制限無効､0で有効
        /// </summary>
        public int YStgLmtNoChk { get; set; } = 1;
        /// <summary>
        /// 正転ｿﾌﾄﾘﾐｯﾄ位置(mm)	
        /// </summary>
        public float YStgCwLmtPos { get; set; } = (float)50.0;
        /// <summary>
        /// 逆転ｿﾌﾄﾘﾐｯﾄ位置(mm)	
        /// </summary>
        public float YStgCcwLmtPos { get; set; } = (float)-50.0;
        /// <summary>
        /// モーター種類(0:パルス,1:サーボ)
        /// </summary>
        public int YStgMotorType { get; set; }
        /// <summary>
        /// 原点センサ極性(0:a接、1:b接)
        /// </summary>
        public int YStgOrgPolarity { get; set; }
        /// <summary>
        /// リミットセンサ極性(0:a接、1:b接)
        /// </summary>
        public int YStgLmtPolarity { get; set; }
        /// <summary>
        /// 減速センサ極性(0:a接、1:b接)
        /// </summary>
        public int YStgDlsPolarity { get; set; }
        /// <summary>
        /// サーボアラーム極性(0:a接、1:b接)
        /// </summary>
        public int YStgAlmPolarity { get; set; }
        /// <summary>
        /// リミットセンサ検出時停止方法(0:即停止、1:減速停止)
        /// </summary>
        public int YStgElsMode { get; set; }
        /// <summary>
        /// YStgLmtNoChk=1で回転制限無効､0で有効
        /// </summary>
        public int YStgNoSlowMode { get; set; }
        /// <summary>
        /// CW減速位置(mm)	
        /// </summary>
        public float YStgCrLowPos { get; set; }
        /// <summary>
        /// CCW減速位置(mm)	
        /// </summary>
        public float YStgCrHiPos { get; set; }
        /// <summary>
        /// 減速時速度(mm/sec)	
        /// </summary>
        public float YStgSlowSpd { get; set; }
        /// <summary>
        /// 制御可能な表示値(string)
        /// </summary>                                                            
        public string YStgCtrIncrement { get; private set; }
        /// <summary>
        /// 制御可能な表示値
        /// </summary>
        public int YStgScale { get; private set; }
        /// <summary>
        /// 最大値
        /// </summary>
        public float YStgMaxValue { get; private set; }
        /// <summary>
        /// 最小値
        /// </summary>
        public float YStgMinValue { get; private set; }
        /// <summary>
        /// Inpos範囲(度)	
        /// </summary>
        public float YStgInpos { get; private set; } = (float)0.10;
        /// <summary>
        /// ボート定数 = 0x00000000
        /// 00000000 00000000 00000000 00000000	//ｺﾝﾊﾟﾚｰﾄ無し
        /// </summary>
        public uint YStgEnvLmOf { get; set; } = 0x00000000;    // 
        /// <summary>
        /// ボード定数 =  0x00005054
        /// 00000000 00000000 01010000 01010100	//ｺﾝﾊﾟﾚｰﾄ一致で減速停止	
        /// </summary>
        public uint YStgEnvLmOn { get; set; } = 0x00005054;
        /// <summary>
        /// ボード定数 = 0x00000054
        /// 00000000 00000000 00000000 01010100	//ｺﾝﾊﾟﾚｰﾄ一致でCWﾘﾐｯﾄ減速停止無し
        /// </summary>
        public uint YStgEnvLmCwOf { get; set; } = 0x00000054;
        /// <summary>
        /// ボード定数  = 0x00005000
        /// 00000000 00000000 01010000 00000000	//ｺﾝﾊﾟﾚｰﾄ一致でCCWﾘﾐｯﾄ減速停止無し
        /// </summary>
        public uint YStgEnvLmCcwOf { get; set; } = 0x00005000;
        /// <summary>
        /// 環境変数 1 指令出力方式、信号極性設定
        /// </summary>
        //public uint Env1Data { get; set; } = 0x204348C4;
        public uint YStgEnv1Data { get; set; } = 0x20434004;
        /// <summary>
        /// 環境変数 2 /ｴﾝｺｰﾀﾞ1逓倍
        /// </summary>
        public uint YStgEnv2Data { get; set; } = 0x0020F555;
        /// <summary>
        /// 環境変数 3 Z層を用いた原点復帰？
        /// </summary>
        public uint YStgEnv3Data { get; set; } = 0x00F00002;
        /// <summary>
        /// 環境変数 4 コンパレータ一致時の動作
        /// </summary>
        public uint YStgEnv4Data { get; set; } = 0x00000000;
        /// <summary>
        /// 環境変数 5
        /// </summary>
        public uint YStgEnv5Data { get; set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// 環境変数 6
        /// </summary>
        public uint YStgEnv6Data { get; set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// 環境変数 7
        /// </summary>
        public uint YStgEnv7Data { get; set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// 読込完了イベント
        /// </summary>             
        public event EventHandler EndLoadYStgParam;
        /// <summary>
        /// パラメータ要求
        /// </summary>
        public void Requeset()
            => EndLoadYStgParam?.Invoke(this, new EventArgs());

        /// <summary>
        /// ボードパラメータファイルへのパスを取得する。
        /// </summary>
        private static string BoardPath
        {
            get { return Path.Combine(Directory.GetCurrentDirectory(), "TXSParam", "Mecha", "BoardTable", "YStage.csv"); }
        }
        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public YStgParamFix()
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
                    case ("YStgStep"):
                        YStgStep = float.Parse(Obj.Item2);
                        break;
                    case ("YStgMag"):
                        YStgMag = float.Parse(Obj.Item2);
                        break;
                    case ("YStgDir"):
                        YStgDir = int.Parse(Obj.Item2);
                        break;
                    case ("YStgPlsMode"):
                        YStgPlsMode = int.Parse(Obj.Item2);
                        break;
                    case ("YStgPlsMult"):
                        YStgPlsMult = int.Parse(Obj.Item2);
                        break;
                    case ("YStgRateTime"):
                        YStgRateTime = float.Parse(Obj.Item2);
                        break;
                    case ("YStgSpdFL"):
                        YStgSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("YStgManualSpd"):
                        YStgManualSpd = float.Parse(Obj.Item2);
                        break;
                    case ("YStgManualLimitSpd"):
                        YStgManualLimitSpd = float.Parse(Obj.Item2);
                        break;
                    case ("YStgManualTimeOut"):
                        YStgManualTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("YStgIndexRateTime"):
                        YStgIndexRateTime = float.Parse(Obj.Item2);
                        break;
                    case ("YStgIndexSpdFL"):
                        YStgIndexSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("YStgIndexSpdFH"):
                        YStgIndexSpdFH = float.Parse(Obj.Item2);
                        break;
                    case ("YStgIndexTimeOut"):
                        YStgIndexTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginSpdFL"):
                        YStgOriginSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginSpdFH"):
                        YStgOriginSpdFH = float.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginTimeOut"):
                        YStgOriginTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginShift"):
                        YStgOriginShift = float.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginPos"):
                        YStgOriginPos = float.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginOffset"):
                        YStgOriginOffset = float.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginMode"):
                        YStgOriginMode = int.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginDir"):
                        YStgOriginDir = int.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginNoChk"):
                        YStgOriginNoChk = int.Parse(Obj.Item2);
                        break;
                    case ("YStgStopTimeOut"):
                        YStgStopTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("YStgLmtNoChk"):
                        YStgLmtNoChk = int.Parse(Obj.Item2);
                        break;
                    case ("YStgCwLmtPos"):
                        YStgCwLmtPos = float.Parse(Obj.Item2);
                        break;
                    case ("YStgCcwLmtPos"):
                        YStgCcwLmtPos = float.Parse(Obj.Item2);
                        break;
                    case ("YStgMotorType"):
                        YStgMotorType = int.Parse(Obj.Item2);
                        break;
                    case ("YStgOrgPolarity"):
                        YStgOrgPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("YStgLmtPolarity"):
                        YStgLmtPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("YStgDlsPolarity"):
                        YStgDlsPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("YStgAlmPolarity"):
                        YStgAlmPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("YStgElsMode"):
                        YStgElsMode = int.Parse(Obj.Item2);
                        break;
                    case ("YStgNoSlowMode"):
                        YStgNoSlowMode = int.Parse(Obj.Item2);
                        break;
                    case ("YStgCrLowPos"):
                        YStgCrLowPos = float.Parse(Obj.Item2);
                        break;
                    case ("YStgCrHiPos"):
                        YStgCrHiPos = float.Parse(Obj.Item2);
                        break;
                    case ("YStgSlowSpd"):
                        YStgSlowSpd = float.Parse(Obj.Item2);
                        break;

                    case ("YStgCtrIncrement"):

                        YStgCtrIncrement = Obj.Item2;

                        double input = Convert.ToDouble(YStgCtrIncrement);

                        double decimal_part = input % 1;

                        string output = Convert.ToString(decimal_part);

                        YStgScale = (output.Length - 2);

                        break;

                    case ("YStgMaxValue"):
                        YStgMaxValue = float.Parse(Obj.Item2);
                        break;

                    case ("YStgMinValue"):
                        YStgMinValue = float.Parse(Obj.Item2);
                        break;
                    case ("YStgInpos"):
                        YStgInpos = float.Parse(Obj.Item2);
                        break;
                }
            }
            return;
        }
    }

    public interface IYStgParamFix
    {
        /// <summary>
        /// 読込完了イベント
        /// </summary>             
        event EventHandler EndLoadYStgParam;
        /// <summary>
        /// パラメータ要求
        /// </summary>
        void Requeset();
    }
}
