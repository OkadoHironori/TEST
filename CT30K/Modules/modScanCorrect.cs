using System;
using System.Windows.Forms;
//
using CTAPI;
using CT30K.Common;
using TransImage;

namespace CT30K
{
	public static class modScanCorrect
	{		
		public static int ViewNumAtGain;	            //ゲイン校正時のビュー数		
		public static int IntegNumAtGain;	            //ゲイン校正時の積算枚数		
		public static int IntegNumAtVer;	            //幾何歪校正時の積算枚数		
		public static CheckState GFlg_GainTableRot;	    //ゲイン校正時のテーブル回転                         'added by 山本 2002-10-5

        //追加2014/10/07hata_v19.51反映
        public static CheckState Flg_GainShiftScan;  //ゲイン校正でシフトスキャン収集するかのフラグ   'v18.00追加 byやまおか 2011/02/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        public static CheckState Flg_GainHaFuOfScan; //ゲイン校正で通常位置スキャン収集するかのフラグ 'v23.20追加 by長野     2015/11/19 'v23.20 

        public static ModeCorConstants Mode_GainCor;//ゲイン校正のモード                             'v18.00追加 byやまおか 2011/02/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        public static bool Flg_RCShiftScan;//回転中心校正でシフトスキャン収集するかのフラグ   'v18.00追加 byやまおか 2011/02/27 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        public static ModeCorConstants Mode_RCCor;//回転中心校正のモード                             'v18.00追加 byやまおか 2011/02/27 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        
		public static int GFlg_Shading_Ver;             //幾何歪校正のシェーディング補正(0:しない,1:する)         'V4.0追加 by 鈴山 2001/01/24

        //テーブル下降収集用の変数		
		public static CheckState DownTable;	//テーブル下降収集の有無		
		public static float DownTableDistance;	//テーブル下降収集用のストローク数（移動距離）（ｍｍ）

        //テーブルY軸移動収集用の変数		
        public static CheckState yAxisMoveTable;	//テーブルY軸移動収集の有無		
        public static float yAxisMoveTableDistance;	//テーブルY軸移動収集用のストローク数（移動距離）（ｍｍ）

        //テーブルFCD軸移動収集用の変数	//Rev23.40 by長野 2016/06/19	
        public static CheckState xAxisMoveTable;	//テーブルFCD軸移動収集の有無		
        public static float xAxisMoveTableDistance;	//テーブルFCD軸移動収集用のストローク数（移動距離）（ｍｍ）

        //校正の状態定数   'v18.00追加 byやまおか 2011/02/09
        public enum ModeCorConstants
        {
            ModeCor_origin = 0,            //基準位置のゲイン校正
            //ModeCor_shift = 1            //シフト位置のゲイン校正
            ModeCor_shift_R = 1,           　//右シフト位置のゲイン校正 //Rev23.20 左右シフト対応 by長野 2015/11/19
            ModeCor_shift_L = 2           　 //左シフト位置のゲイン校正   //Rev23.20 左右シフト対応 by長野 2015/11/19
        }

		public static void InitScanCorrect()
		{
			//幾何歪校正のシェーディング補正
			GFlg_Shading_Ver = 0;

			//ゲイン校正時のテーブル回転有無
			GFlg_GainTableRot = CheckState.Unchecked;

			//テーブル下降収集用変数の初期設定       v7.0 added by 間々田 2003-03-03
			DownTable = CheckState.Unchecked;			//テーブル下降収集の有無
			DownTableDistance = 10F;			        //テーブル下降収集用のストローク数（移動距離）（mm）

			//パーキンエルマー16インチFPDは端のデータを使わないようにするため
			//LimitFanAngle = IIf(DetType = DetTypePke, 0.96, 0.99)    'v17.00変更 byやまおか 2010/03/02
			//スキャン校正で使うファン角を調整する   'v17.22変更 byやまおか 2010/10/19
			//PkeFPDの場合
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))
            {
                //変更2014/10/07hata_v19.51反映
                ////LimitFanAngle = IIf(Use_FpdAllpix, 0.999999, 0.96)   '額縁ありの場合は端を使わない。額縁なしの場合は限界まで使う。
                //ScanCorrect.LimitFanAngle = (CTSettings.detectorParam.Use_FpdAllpix ? 0.999999F : 0.965F);     //v19.12 透視画像切り出しchを±4ch→8chにする代わりに、0.96から0.965へ広げる by長野 2013/03/06
                if (CTSettings.detectorParam.Use_FpdAllpix == true)
                {
                    ScanCorrect.LimitFanAngle = 0.999999f;
                }
                else if ((CTSettings.detectorParam.Use_FpdAllpix == false & CTSettings.detectorParam.h_size == 1024))
                {
                    //LimitFanAngle = 0.99
                    //ScanCorrect.LimitFanAngle = 0.96f;      //v19.50 0.96へ戻す by長野 2014/01/06
                    //ScanCorrect.LimitFanAngle = 0.99f;        //v20.00 0.99へ戻す by長野 2015/01/24
                    ScanCorrect.LimitFanAngle = 1.00f;        //v26.40 1.00にする byやまおか 2019/03/07
                }
                else if ((CTSettings.detectorParam.Use_FpdAllpix == false & CTSettings.detectorParam.h_size == 2048))
                {
                    //LimitFanAngle = 0.99
                    //ScanCorrect.LimitFanAngle = 0.96f;      //v19.50 0.96へ戻す by長野 2014/01/06
                    //ScanCorrect.LimitFanAngle = 0.99f;        //v20.00 0.99へ戻す by長野 2015/01/24
                    ScanCorrect.LimitFanAngle = 1.00f;        //v26.40 1.00にする byやまおか 2019/03/07
                }
            }
            //v19.17 8inchと16inchの区別をつける by長野　2013/09/13
            //LimitFanAngle = IIf(Use_FpdAllpix, 0.999999, 0.99)    'v19.17 透視画像切り出しchを±8ch→24chにする代わりに、0.965から0.975へ広げる by長野 2013/09/12
            //I.I.の場合
            else
            {
                ScanCorrect.LimitFanAngle = 0.99F;
            }
		}
	}
}
