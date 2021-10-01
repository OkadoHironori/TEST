using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Board.BoardControl
{
    public class XStgParamFix : IXStgParamFix
    {
        /// <summary>
        /// 回転軸ピッチ(mm/P)
        /// </summary>
        public float XStgStep { get; private set; }
        /// <summary>                                       
        /// 倍率
        /// </summary>
        public float XStgMag { get; private set; }
        /// <summary>
        /// アドレス増加方向
        /// </summary>
        public int XStgDir { get; private set; }
        /// <summary>
        /// パルス出力方式
        /// </summary>
        public int XStgPlsMode { get; private set; }
        /// <summary>
        /// パルス逓倍
        /// </summary>
        public int XStgPlsMult { get; private set; }
        /// <summary>
        /// 加減速レート(秒)
        /// </summary>
        public float XStgRateTime { get; private set; }
        /// <summary>
        /// ＦＬ速mm(mm/s)
        /// </summary>
        public float XStgSpdFL { get; private set; }
        /// <summary>
        /// 手動回転スピード
        /// </summary>
        public float XStgManualSpd { get; private set; }
        /// <summary>
        /// 手動動作 最高速度
        /// </summary>
        public float XStgManualLimitSpd { get; private set; }
        /// <summary>
        /// 手動動作 動作タイムアウト（msec)
        /// </summary>
        public int XStgManualTimeOut { get; set; }
        /// <summary>
        /// 位置決め 加減速レート(秒)
        /// </summary>
        public float XStgIndexRateTime { get; set; }
        /// <summary>
        /// 位置決め ＦＬ速度(mm/s)
        /// </summary>
        public float XStgIndexSpdFL { get; set; }
        /// <summary>
        /// 位置決め 動作速度(rpm)
        /// </summary>
        public float XStgIndexSpdFH { get; set; }
        /// <summary>
        /// 位置決め 動作タイムアウト(msec)
        /// </summary>
        public int XStgIndexTimeOut { get; set; }
        /// <summary>
        /// 原点復帰 動作速度(mm/s)
        /// </summary>
        public float XStgOriginSpdFL { get; set; }
        /// <summary>
        /// 原点復帰 FL速度(mm/s)
        /// </summary>
        public float XStgOriginSpdFH { get; set; }
        /// <summary>
        /// 原点復帰 動作タイムアウト時間(秒)
        /// </summary>
        public int XStgOriginTimeOut { get; set; }
        /// <summary>
        /// 原点から抜け出す送り量(mm)
        /// </summary>
        public float XStgOriginShift { get; set; }
        /// <summary>
        /// 原点位置(mm)
        /// </summary>
        public float XStgOriginPos { get; set; }
        /// <summary>
        /// 原点オフセット位置(mm)
        /// </summary>
        public float XStgOriginOffset { get; set; }
        /// <summary>
        /// 原点復帰ﾓｰﾄﾞ0:原点ｾﾝｻで停止、2:Z相で停止
        /// </summary>
        public int XStgOriginMode { get; set; }
        /// <summary>
        /// 原点復帰方向
        /// </summary>
        public int XStgOriginDir { get; set; }
        /// <summary>
        /// 原点復帰方向
        /// </summary>
        public int XStgOriginNoChk { get; set; }
        /// <summary>
        /// 停止動作 動作タイムアウト(秒)
        /// </summary>
        public int XStgStopTimeOut { get; set; }
        /// <summary>
        /// XStgLmtNoChk=1で回転制限無効､0で有効
        /// </summary>
        public int XStgLmtNoChk { get; set; } = 1;
        /// <summary>
        /// 正転ｿﾌﾄﾘﾐｯﾄ位置(mm)	
        /// </summary>
        public float XStgCwLmtPos { get; set; } = (float)50.0;
        /// <summary>
        /// 逆転ｿﾌﾄﾘﾐｯﾄ位置(mm)	
        /// </summary>
        public float XStgCcwLmtPos { get; set; } = (float)-50.0;
        /// <summary>
        /// モーター種類(0:パルス,1:サーボ)
        /// </summary>
        public int XStgMotorType { get; set; }
        /// <summary>
        /// 原点センサ極性(0:a接、1:b接)
        /// </summary>
        public int XStgOrgPolarity { get; set; }
        /// <summary>
        /// リミットセンサ極性(0:a接、1:b接)
        /// </summary>
        public int XStgLmtPolarity { get; set; }
        /// <summary>
        /// 減速センサ極性(0:a接、1:b接)
        /// </summary>
        public int XStgDlsPolarity { get; set; }
        /// <summary>
        /// サーボアラーム極性(0:a接、1:b接)
        /// </summary>
        public int XStgAlmPolarity { get; set; }
        /// <summary>
        /// リミットセンサ検出時停止方法(0:即停止、1:減速停止)
        /// </summary>
        public int XStgElsMode { get; set; }
        /// <summary>
        /// XStgLmtNoChk=1で回転制限無効､0で有効
        /// </summary>
        public int XStgNoSlowMode { get; set; }
        /// <summary>
        /// CW減速位置(mm)	
        /// </summary>
        public float XStgCrLowPos { get; set; }
        /// <summary>
        /// CCW減速位置(mm)	
        /// </summary>
        public float XStgCrHiPos { get; set; }
        /// <summary>
        /// 減速時速mm(mm/sec)	
        /// </summary>
        public float XStgSlowSpd { get; set; }
        /// <summary>
        /// 制御可能な表示値(string)
        /// </summary>
        public string XStgCtrIncrement { get; private set; }
        /// <summary>
        /// 制御可能な表示値
        /// </summary>
        public int XStgScale { get; private set; }
        /// <summary>
        /// 最大値
        /// </summary>
        public float XStgMaxValue { get; private set; }
        /// <summary>
        /// 最小値
        /// </summary>
        public float XStgMinValue { get; private set; }
        /// <summary>
        /// Inpos範囲(度)	
        /// </summary>
        public float XStgInpos { get; private set; } = (float)0.10;
        /// <summary>
        /// ボート定数 = 0x00000000
        /// 00000000 00000000 00000000 00000000	//ｺﾝﾊﾟﾚｰﾄ無し
        /// </summary>
        public uint XStgEnvLmOf { get; set; } = 0x00000000;    // 
        /// <summary>
        /// ボード定数 =  0x00005054
        /// 00000000 00000000 01010000 01010100	//ｺﾝﾊﾟﾚｰﾄ一致で減速停止	
        /// </summary>
        public uint XStgEnvLmOn { get; set; } = 0x00005054;
        /// <summary>
        /// ボード定数 = 0x00000054
        /// 00000000 00000000 00000000 01010100	//ｺﾝﾊﾟﾚｰﾄ一致でCWﾘﾐｯﾄ減速停止無し
        /// </summary>
        public uint XStgEnvLmCwOf { get; set; } = 0x00000054;
        /// <summary>
        /// ボード定数  = 0x00005000
        /// 00000000 00000000 01010000 00000000	//ｺﾝﾊﾟﾚｰﾄ一致でCCWﾘﾐｯﾄ減速停止無し
        /// </summary>
        public uint XStgEnvLmCcwOf { get;  set; } = 0x00005000;
        /// <summary>
        /// 環境変数 1 指令出力方式、信号極性設定
        /// </summary>
        public uint XStgEnv1Data { get;  set; } = 0x20434004;
        /// <summary>
        /// 環境変数 2 /ｴﾝｺｰﾀﾞ1逓倍
        /// </summary>
        public uint XStgEnv2Data { get;  set; } = 0x0020F555;
        /// <summary>
        /// 環境変数 3 Z層を用いた原点復帰？
        /// </summary>
        public uint XStgEnv3Data { get;  set; } = 0x00F00002;
        /// <summary>
        /// 環境変数 4 コンパレータ一致時の動作
        /// </summary>
        public uint XStgEnv4Data { get;  set; } = 0x00000000;
        /// <summary>
        /// 環境変数 5
        /// </summary>
        public uint XStgEnv5Data { get;  set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// 環境変数 6
        /// </summary>
        public uint XStgEnv6Data { get;  set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// 環境変数 7
        /// </summary>
        public uint XStgEnv7Data { get;  set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// 読込完了イベント
        /// </summary>             
        public event EventHandler EndLoadXStgParam;
        /// <summary>
        /// パラメータ要求
        /// </summary>
        public void Requeset()
            => EndLoadXStgParam?.Invoke(this, new EventArgs());
        /// <summary>
        /// ボードパラメータファイルへのパスを取得する。
        /// </summary>
        private static string BoardPath
        {
            get { return Path.Combine(Directory.GetCurrentDirectory(), "TXSParam", "Mecha", "BoardTable", "XStage.csv"); }
        }
        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public XStgParamFix()
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
                    case ("XStgStep"):
                        XStgStep = float.Parse(Obj.Item2);
                        break;
                    case ("XStgMag"):
                        XStgMag = float.Parse(Obj.Item2);
                        break;
                    case ("XStgDir"):
                        XStgDir = int.Parse(Obj.Item2);
                        break;
                    case ("XStgPlsMode"):
                        XStgPlsMode = int.Parse(Obj.Item2);
                        break;
                    case ("XStgPlsMult"):
                        XStgPlsMult = int.Parse(Obj.Item2);
                        break;
                    case ("XStgRateTime"):
                        XStgRateTime = float.Parse(Obj.Item2);
                        break;
                    case ("XStgSpdFL"):
                        XStgSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("XStgManualSpd"):
                        XStgManualSpd = float.Parse(Obj.Item2);
                        break;
                    case ("XStgManualLimitSpd"):
                        XStgManualLimitSpd = float.Parse(Obj.Item2);
                        break;
                    case ("XStgManualTimeOut"):
                        XStgManualTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("XStgIndexRateTime"):
                        XStgIndexRateTime = float.Parse(Obj.Item2);
                        break;
                    case ("XStgIndexSpdFL"):
                        XStgIndexSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("XStgIndexSpdFH"):
                        XStgIndexSpdFH = float.Parse(Obj.Item2);
                        break;
                    case ("XStgIndexTimeOut"):
                        XStgIndexTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginSpdFL"):
                        XStgOriginSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginSpdFH"):
                        XStgOriginSpdFH = float.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginTimeOut"):
                        XStgOriginTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginShift"):
                        XStgOriginShift = float.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginPos"):
                        XStgOriginPos = float.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginOffset"):
                        XStgOriginOffset = float.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginMode"):
                        XStgOriginMode = int.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginDir"):
                        XStgOriginDir = int.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginNoChk"):
                        XStgOriginNoChk = int.Parse(Obj.Item2);
                        break;
                    case ("XStgStopTimeOut"):
                        XStgStopTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("XStgLmtNoChk"):
                        XStgLmtNoChk = int.Parse(Obj.Item2);
                        break;
                    case ("XStgCwLmtPos"):
                        XStgCwLmtPos = float.Parse(Obj.Item2);
                        break;
                    case ("XStgCcwLmtPos"):
                        XStgCcwLmtPos = float.Parse(Obj.Item2);
                        break;
                    case ("XStgMotorType"):
                        XStgMotorType = int.Parse(Obj.Item2);
                        break;
                    case ("XStgOrgPolarity"):
                        XStgOrgPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("XStgLmtPolarity"):
                        XStgLmtPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("XStgDlsPolarity"):
                        XStgDlsPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("XStgAlmPolarity"):
                        XStgAlmPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("XStgElsMode"):
                        XStgElsMode = int.Parse(Obj.Item2);
                        break;
                    case ("XStgNoSlowMode"):
                        XStgNoSlowMode = int.Parse(Obj.Item2);
                        break;
                    case ("XStgCrLowPos"):
                        XStgCrLowPos = float.Parse(Obj.Item2);
                        break;
                    case ("XStgCrHiPos"):
                        XStgCrHiPos = float.Parse(Obj.Item2);
                        break;
                    case ("XStgSlowSpd"):
                        XStgSlowSpd = float.Parse(Obj.Item2);
                        break;

                    case ("XStgCtrIncrement"):

                        XStgCtrIncrement = Obj.Item2;

                        double input = Convert.ToDouble(XStgCtrIncrement);

                        double decimal_part = input % 1;

                        string output = Convert.ToString(decimal_part);

                        XStgScale = (output.Length - 2);

                        break;

                    case ("XStgMaxValue"):
                        XStgMaxValue = float.Parse(Obj.Item2);
                        break;

                    case ("XStgMinValue"):
                        XStgMinValue = float.Parse(Obj.Item2);
                        break;
                    case ("XStgInpos"):
                        XStgInpos = float.Parse(Obj.Item2);
                        break;
                }
            }
        }
    }
    /// <summary>
    /// 微調X軸の固定パラメータクラス
    /// </summary>
    public interface IXStgParamFix
    {
        /// <summary>
        /// 読込完了イベント
        /// </summary>
        event EventHandler EndLoadXStgParam;
        /// <summary>
        /// パラメータ要求
        /// </summary>
        void Requeset();
    }
}
