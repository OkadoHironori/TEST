using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
//
using CT30K.Common.Library;

namespace CT30K.Common
{
    public class IniValue
    {
        //ダブルオブリーク実行ファイル名
        public string DoubleObliquePath { get; private set; }

        //CT30Kメッセージ受信用ポート番号：デフォルトは7010 
        public int CT30KPort { get; private set; }

        //イメージプロ実行ファイル名
        public string ImageProExe { get; private set; }

        //イメージプロ起動待ち時間
        public float ImageProPauseTime { get; private set; }

        //CT30K起動時にイメージプロを起動するかどうかのフラグ
        public bool ImageProStartup { get; private set; }

        //スキャン・リトライの使用するデータ用のメモリ 追加 by長野 2014/09/11
        public int SharedMemSize { get; private set; }
        
        //段階ウォームアップ設定ステップ数
        public int STEPWU_NUM { get; private set; }

        //段階ウォームアップ設定パラメータ
        public int[] STEPWU_KV
        {
            get
            { 
                return  steppwuKV; 
            }
            set
            {
                 steppwuKV = value;
            }
        }
 
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //Ｘ線ドライバ（Viscom）のバージョン
        //public string DLLVERSION { get; private set; }
        //X線警告音WAVファイルパス
        //public string WavWarningPath { get; private set; }
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
       

        #region 静的コンストラクタ
        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static IniValue()
        {
            //int[] steppwuKV = new int[5];
        }
        #endregion




        /// <summary>
        /// 読込み
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Load(string fileName)
        {
            IniFile ini = new IniFile(fileName);
            if (!ini.Exists)
            {
                return false;
            }

            
            //DoubleObliquePath = ini.GetIniString("DoubleOblique", "ExeFileName", "");
            //WavWarningPath = ini.GetIniString("WavWarning", "WavFileName", "");
            //CT30KPort = ini.GetIniInt("SocketPort", "CT30KPort", 7010);
            GetCtIni(ini);

            return true;
        }

        private void GetCtIni(IniFile inifile)
        {

            //イメージプロ実行ファイル名取得
            ImageProExe = inifile.GetIniString("Image-Pro Plus", "ExeFileName", "");

            //イメージプロ起動待ち時間取得
            ImageProPauseTime = Convert.ToSingle(inifile.GetIniInt("Image-Pro Plus", "PauseTime", 1));

            //CT30K起動時にイメージプロを起動するか
            ImageProStartup =Convert.ToBoolean(inifile.GetIniInt("Image-Pro Plus", "Startup", 1));

            //ダブルオブリーク実行ファイル名取得
            DoubleObliquePath = inifile.GetIniString("DoubleOblique", "ExeFileName", "");

            //CT30Kメッセージ受信用ポート番号：デフォルトは7010 
            CT30KPort = inifile.GetIniInt("SocketPort", "CT30KPort", 7010);

            //使用しない
            //ramMemSize = inifile.GetIniInt("RamDisk", "RamMem", 16000);
            //追加 by長野 2014/9/11
            SharedMemSize = inifile.GetIniInt("Memory", "Memory", 16000);

            //段階ウォームアップ設定パラメータを取得     'v17.72/v19.02追加 byやまおか 2012/05/16
            //ステップ数
            //変更2014/11/28hata_v19.51_dnet
            //STEPWU_NUM = inifile.GetIniInt("StepWarmUP", "StepNUM", 1);
            STEPWU_NUM = Convert.ToInt32(inifile.GetFileIniString("StepWarmUP", "StepNUM", "1", AppValue.STEPWUP_INI));
            //値の調整
            if ((STEPWU_NUM < 1))
            {
                STEPWU_NUM = 1;
            }
            if ((STEPWU_NUM > 5))
            {
                STEPWU_NUM = 5;
            }

            //第1段階           
            //変更2014/11/28hata_v19.51_dnet
            //steppwuKV[1] = inifile.GetIniInt("StepWarmUP", "StepKV1", 1);
            steppwuKV[1] = Convert.ToInt32(inifile.GetFileIniString("StepWarmUP", "StepKV1", "1", AppValue.STEPWUP_INI));
            //値の調整
            if ((steppwuKV[1] < 1))
            {
                steppwuKV[1] = 1;
            }
            if ((steppwuKV[1] > 1000))
            {
                steppwuKV[1] = 1000;
            }

            //第2段階
            //変更2014/11/28hata_v19.51_dnet
            //steppwuKV[1] = inifile.GetIniInt("StepWarmUP", "StepKV2", 1);
            steppwuKV[2] = Convert.ToInt32(inifile.GetFileIniString("StepWarmUP", "StepKV2", "1", AppValue.STEPWUP_INI));
            //値の調整
            if ((steppwuKV[2] < 1))
            {
                steppwuKV[2] = 1;
            }
            if ((steppwuKV[2] > 1000))
            {
                steppwuKV[2] = 1000;
            }

            //第3段階
            //変更2014/11/28hata_v19.51_dnet
            //steppwuKV[3] = inifile.GetIniInt("StepWarmUP", "StepKV3", 1);
            steppwuKV[3] = Convert.ToInt32(inifile.GetFileIniString("StepWarmUP", "StepKV3", "1", AppValue.STEPWUP_INI));
            //値の調整
            if ((steppwuKV[3] < 1))
            {
                steppwuKV[3] = 1;
            }
            if ((steppwuKV[3] > 1000))
            {
                steppwuKV[3] = 1000;
            }

            //第4段階
            //変更2014/11/28hata_v19.51_dnet
            //steppwuKV[4] = inifile.GetIniInt("StepWarmUP", "StepKV4", 1);
            steppwuKV[4] = Convert.ToInt32(inifile.GetFileIniString("StepWarmUP", "StepKV4", "1", AppValue.STEPWUP_INI));
            //値の調整
            if ((steppwuKV[4] < 1))
            {
                steppwuKV[4] = 1;
            }
            if ((steppwuKV[4] > 1000))
            {
                steppwuKV[4] = 1000;
            }

            //第5段階
            //変更2014/11/28hata_v19.51_dnet
            //steppwuKV[5] = inifile.GetIniInt("StepWarmUP", "StepKV5", 1);
            steppwuKV[5] = Convert.ToInt32(inifile.GetFileIniString("StepWarmUP", "StepKV5", "1", AppValue.STEPWUP_INI));
            //値の調整
            if ((steppwuKV[5] < 1))
            {
                steppwuKV[5] = 1;
            }
            if ((steppwuKV[5] > 1000))
            {
                steppwuKV[5] = 1000;
            }

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //Ｘ線ドライバ（Viscom）のバージョン
            //DLLVERSION = inifile.GetIniString("Viscom Driver", "DllVersion", "");
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
           
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //X線警告音WAVファイルパスを取得
            //WavWarningPath = inifile.GetIniString("WavWarning", "WavFileName", "");
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

        }

        private int[] steppwuKV = new int[6];

    }
}
