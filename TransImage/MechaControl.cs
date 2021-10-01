using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

using CTAPI;

namespace TransImage
{
    public static class MechaControl
    {
        private static int _hDevID = -1;
        //private static int hPioID1 = 0;

        //
		// MyCallbackCaptureメソッドで使用する static フィールド
		//
        public static int MyCallbackCapture_CBcount;

		//
		// MyCallback コールバック関数のデリゲート
		//
		public delegate void MyCallbackDelegate(int parm);
 		
        //
		// MyCallbackCapture コールバック関数のデリゲート
		//
        public delegate void MyCallbackCaptureDelegate(int parm);


        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int RotateInit(int hDevID);

        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int RotateManual(int hDevID, int RotDir, float RotManuSpeed, int RotSpeedDet);

        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int RotateIndex(int hDevID, int RotCoordi, float RotAngle, MyCallbackDelegate CallBackAddress);

        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int RotateOrigin(int hDevID, MyCallbackDelegate CallBackAddress);
        
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int RotateSlowStop(int hDevID, MyCallbackDelegate CallBackAddress);

        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int UdIndexBrakeLess(int hDevID, long UdCoordi, float UdIndexPos);

        //Rev21.00 追加 by長野 2015/03/08
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int UdManual(int hDevID, int UdDir, float UdManuSpeed);
        
        //Rev21.00 追加 by長野 2015/03/08
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int UdSlowStop(int hDevID, MyCallbackDelegate CallBackAddress);

        /// <summary>
        /// 積算中フラグ
        /// </summary>
        public static int hDevID
        {
            get { return _hDevID; }
            set { _hDevID = value; }
        }


        //試料テーブル回転関数
        public static void RotateTable(
            int m,				//ビュー番号(0オリジン）
            int View,			//全ビュー数
            int DevID,			//メカ制御ボードデバイスハンドル
            //int mrc,			//メカ制御ボードエラーステータス
            ref int mrc,		//メカ制御ボードエラーステータス //Rev23.40 変更 by長野 2016/06/19
            int table_rotation,	//テーブル回転モード 0:ステップ 1:連続
            int RotDirect)      //回転方向 0:CW 1:CCW
        {

            float RotAng;			//テーブル回転角度(degree）
            int rotation;			//回転回数
            float tmpRotAng;		//(m-1)時のテーブル回転角度(degree）


            if (DevID < 0) return;
            _hDevID = DevID;

            if (table_rotation == 0)
            {
                if (mrc == 0)
                {
                    RotAng = (float)((360.0 / (float)(View)) * (float)(m + 1));
                    tmpRotAng = (float)((360.0 / (float)(View)) * (float)m);
                    rotation = (int)(tmpRotAng / (float)360.0);
                    RotAng = RotAng - 360 * rotation;

                    if (RotDirect != 0) RotAng = (float)0 - RotAng;

                    if ((RotAng >= -360) && (RotAng <= 360))
                    {
                        mrc = RotateIndex(DevID, 0, RotAng, null);
                    }
                }
            }
        }

        //ヘリカルスキャン用試料テーブル昇降関数
        public static void UpDownTableHelical(
            long m,				//ビュー番号(0オリジン）
            long View,			//ビュー数(360°あたり）
            int hDevID,			//メカ制御ボードデバイスハンドル
            long mrc,			//メカ制御ボードエラーステータス
            long Inh,			//ヘリカルモード 0:非ヘリカル 1:ヘリカル
            float Zp,			//ヘリカルピッチ(mm）
            float udab_pos)		//ヘリカルスキャン開始時の昇降絶対位置(mm)
        {
            float UdIndexPos;		//昇降位置(mm)
            long UdCoordi = 0;		//座標 0:絶対座標(ソフト原点0度) 1:相対座標

            if (Inh == 1)
            {
                if (mrc == 0)
                {
                    UdIndexPos = udab_pos + ((float)(m + 1) / (float)(View) * Zp);
                    mrc = UdIndexBrakeLess(hDevID, UdCoordi, UdIndexPos);	//ブレーキ制御なし
                }
            }
        }
    
    
    
    }
}
