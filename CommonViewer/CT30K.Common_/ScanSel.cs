using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CTAPI;

namespace CT30K.Common
{

    public class ScanSel
    {
        // スキャンモード定数
        //public const int DataModeScan = 1;      // スキャン
        //public const int DataModeCone = 4;      // コーンビームスキャン
 
        //public const int ScanModeHalf = 1;      // ハーフスキャン
        //public const int ScanModeFull = 2;      // フルスキャン
        //public const int ScanModeOffset = 3;    // オフセットスキャン

        //// マルチスキャンモード定数
        //public const int MultiScanModeSingle = 1;       // シングルスキャン
        //public const int MultiScanModeMulti = 3;        // マルチスキャン
        //public const int MultiScanModeSlicePlan = 5;    // スライスプラン

        //// マトリクスサイズ定数
        //public const int MatrixSize256 = 1;             // 256×256
        //public const int MatrixSize512 = 2;             // 512×512
        //public const int MatrixSize1024 = 3;            // 1024×1024
        //public const int MatrixSize2048 = 4;            // 2048×2048
        //public const int MatrixSize4096 = 5;            // 4096×4096 v16.10 4096を追加 by 長野 10/03/16

        //// オペレーションモード定数
        //public const int OP_SCAN = 1;
        //public const int OP_RECON = 3;
        //public const int OP_ZOOM = 4;
        //public const int OP_CONEBEAM = 6;
        //public const int OP_CONERECON = 7;
        //public const int OP_POST_CONE = 8;

        //スキャンモード定数
        public enum DataModeConstants : int
		{
			 DataModeScan = 1,			//スキャン
			 DataModeCone = 4			//コーンビームスキャン
		}

        //スキャンモード定数
        public enum ScanModeConstants : int
		{
			ScanModeHalf = 1,			//ハーフスキャン
			ScanModeFull,			    //フルスキャン
			ScanModeOffset,		        //オフセットスキャン
            ScanModeShift               //シフトスキャン 'v18.00追加 byやまおか 2011/02/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_追加2014/07/16hata_v19.51反映
		}

        //マルチスキャンモード定数
        public enum MultiScanModeConstants : int
		{
			MultiScanModeSingle = 1,	//シングルスキャン
			MultiScanModeMulti = 3,		//マルチスキャン
			MultiScanModeSlicePlan = 5	//スライスプラン
		}

        //マトリクスサイズ定数
        public enum MatrixSizeConstants : int
		{
			MatrixSize256 = 1,		    //256×256
			MatrixSize512,			    //512×512
			MatrixSize1024,			    //1024×1024
			MatrixSize2048,			    //2048×2048
			MatrixSize4096			    //4096×4096 v16.10 4096を追加 by 長野 10/03/16
		}

        //オペレーションモード定数
        public enum OperationModeConstants : int
		{
			OP_SCAN = 1,
			OP_RECON = 3,
			OP_ZOOM = 4,
			OP_CONEBEAM = 6,
			OP_CONERECON = 7,
			OP_POST_CONE = 8,
            OP_SHIFT_SCAN = 9,  //v18.00追加 byやまおか 2011/03/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_追加2014/07/16hata_v19.51反映
            OP_SHIFT_CONE = 10  //v18.00追加 byやまおか 2011/03/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_追加2014/07/16hata_v19.51反映
		}
	

        /// <summary>
        /// 構造体データ
        /// </summary>
        public CTstr.SCANSEL Data;

        /// <summary>
        /// バックアップ構造体データ
        /// </summary>
        private CTstr.SCANSEL backup;

        /// <summary>
        /// データ読込み
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (ComLib.GetScansel(out Data) != 0)
            {
                return false;
            }
            return true;
        }
         
        /// <summary>
        /// データ読込み　（VB6のGetMyScansel)
        /// </summary>
        /// <param name="scaninh"></param>
        /// <returns></returns>
        public bool Load(CTstr.SCANINH scaninh)
        {
            bool res = Load();

            if (res)
            {
                // マトリクスサイズ
                if ((Data.matrix_size < 1) || (Data.matrix_size > 5))
                {
                    Data.matrix_size = 2;
                }

                // スキャンモード：1(ﾊｰﾌ),2(ﾌﾙ),3(ｵﾌｾｯﾄ)                  注）frmTableAutoMove.cmdMove_Click でも変更される
                //変更2014/10/07hata_v19.51反映
                //if ((Data.scan_mode < 1) || (Data.scan_mode > 3))
                if ((Data.scan_mode < 1) || (Data.scan_mode > 4))   //'v18.00変更 4(ｼﾌﾄｵﾌｾｯﾄ)追加 byやまおか 2011/02/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                {
                    Data.scan_mode = 2;
                }

                // 積算枚数
                if (Data.scan_integ_number < 1) Data.scan_integ_number = 1;

                // X線管：0(130kV),1(225kV)                               注）Set_RotationCenter_Parameterでも変更される
                if (Data.multi_tube != 1) Data.multi_tube = 0;

                // 追加2013/04/09<dNet化>_hata
                // 回転選択:0（テーブル）、1（X線管）
                if (Data.rotate_select != 1) Data.rotate_select = 0;
                
                // マルチスライス
                // 同時スキャン枚数最大(0:1ｽﾗｲｽ,1:3ｽﾗｲｽ,2:5ｽﾗｲｽ)
                if (Data.max_multislice < 0) Data.max_multislice = 0;
                if (Data.max_multislice > 2) Data.max_multislice = 2;                               
                // 同時スキャン枚数(0:1ｽﾗｲｽ,1:3ｽﾗｲｽ,2:5ｽﾗｲｽ)
                if (Data.multislice < 0) Data.multislice = 0;
                if (Data.multislice > Data.max_multislice) Data.multislice = Data.max_multislice;
                // 同時スキャンピッチ
                if (Data.multislice_pitch > Data.max_multislice_pitch) Data.multislice_pitch = Data.max_multislice_pitch;

                // コーン分散処理：0(なし),1(あり)
                if (((scaninh.cone_distribute == 0) || (scaninh.cone_distribute2 == 0)) &&
                    (Data.cone_distribute == 1) && (Data.data_mode == (int)DataModeConstants.DataModeCone))
                {
                    Data.cone_distribute = 1;
                }
                else
                {
                    Data.cone_distribute = 0;
                }
            }

            return res;
        }

        /// <summary>
        /// バックアップ
        /// </summary>
        /// <returns></returns>
        public bool Backup()
        {
            if (ComLib.GetScansel(out backup) != 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// リストア
        /// </summary>
        /// <returns></returns>
        public bool Restore()
        {
            if (ComLib.PutScansel(ref backup) != 0)
            {
                return false;
            }

            // データ反映
            //return Load();
            return true;
        }

        /// 書き込み
        /// </summary>
        /// <returns></returns>
        public bool Write()
        {
            if (ComLib.PutScansel(ref Data) != 0)
            {
                return false;
            }

            // データ反映
            //return Load();
            return true;
        }

        /// データ置き換え
        /// </summary>
        /// <returns></returns>
        public bool Put(CTstr.SCANSEL data)
        {
            if (ComLib.PutScansel(ref data) != 0)
            {
                return false;
            }

            // データ反映
            //return Load();
            return true;
        }

    }
}
