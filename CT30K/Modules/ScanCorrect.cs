using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
//
using CTAPI;
using CT30K.Common;
using TransImage;

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： ScanCorrect.bas                                             */
    ///* 処理概要　　： スキャン校正共通モジュール                                  */
    ///* 注意事項　　： なし                                                        */
    ///* -------------------------------------------------------------------------- */
    ///* 適用計算機　： DOS/V PC                                                    */
    ///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
    ///* コンパイラ　： VB 6.0                                                      */
    ///* -------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
    ///*                                                                            */
    ///* V1.00       99/XX/XX    (TOSFEC) ????????                                  */
    ///* V2.0        00/02/08    (TOSFEC) 鈴山　修   V1.00を改造                    */
    ///* V3.0        00/08/01    (TOSFEC) 鈴山　修   ｺｰﾝﾋﾞｰﾑCT対応                  */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
    
	internal static class ScanCorrect
	{
        //********************************************************************************
        //  定数データ宣言
        //********************************************************************************

        //各種ファイル名			
		public const string CORRECT_PATH = "C:\\CT\\ScanCorrect\\";                 //校正ﾌｧｲﾙのﾃﾞｨﾚｸﾄﾘ名			
		public const string VERT_CORRECT = CORRECT_PATH + "vertical.cor";           //幾何歪校正画像ﾌｧｲﾙ名			
		public const string RC01_CORRECT = CORRECT_PATH + "RotationCenter01.cor";   //回転中心校正画像ﾌｧｲﾙ名(No.1)		
		public const string RC02_CORRECT = CORRECT_PATH + "RotationCenter02.cor";	//回転中心校正画像ﾌｧｲﾙ名(No.2)		
		public const string RC03_CORRECT = CORRECT_PATH + "RotationCenter03.cor";	//回転中心校正画像ﾌｧｲﾙ名(No.3)		
		public const string RC04_CORRECT = CORRECT_PATH + "RotationCenter04.cor";	//回転中心校正画像ﾌｧｲﾙ名(No.4)		
		public const string RC05_CORRECT = CORRECT_PATH + "RotationCenter05.cor";	//回転中心校正画像ﾌｧｲﾙ名(No.5)		
		public const string SCPOSI_CORRECT = CORRECT_PATH + "ScanPosition.cor";	    //スキャン位置校正画像ﾌｧｲﾙ名　　added by 山本　2005-12-7			
		public const string SCPOSI_CORRECT_2ND_DET = CORRECT_PATH + "ScanPosition2ndDet.cor";   //追加　2nd検出器用 スキャン位置校正画像ﾌｧｲﾙ名 by 長野　2010-09-06		
		public const string HIZUMI_CSV = CORRECT_PATH + "hizumi.csv";	            //幾何歪ﾃｰﾌﾞﾙ

        //生データの左右の端を使わないためにファン角を計算値より小さくするための係数
        //Public Const LimitFanAngle As Single = 0.9
        //Public Const LimitFanAngle As Single = 0.99        'changed by 山本　2002-5-2
        //Public Const LimitFanAngle  As Single = 0.97        'changed by 山本　2002-6-27
        //Public Const LimitFanAngle  As Single = 0.99        'changed by 山本　2005-12-13   'v17.00変更 byやまおか 2010/02/16
		public static float LimitFanAngle;	//v17.00変更 byやまおか 2010/02/16

        //Public Const FRMWIDTH       As Long = 3             '入力画像の額縁         added by 山本 99-7-31
        //Public Const FRMHEIGHT      As Long = 2             '入力画像の額縁         'v11.2追加 by 間々田 2005/11/11		
		public static int FRMWIDTH;	    //入力画像の額縁 'v17.22変更 byやまおか 2010/10/19		
		public static int FRMHEIGHT;	//入力画像の額縁 'v17.22変更 byやまおか 2010/10/19

        //Public Const VlinePicPitch  As Long = 10           '垂直線検出ピッチ       'v9.7削除 by 間々田 2004/12/02 未使用		
		public const int Matrix = 6;	        //ﾌｨｯﾃｨﾝｸﾞ用行列のｻｲｽﾞ		
		public const double Pai = 3.141592654;	//円周率
        		
		public const int IMGPRODBG = 1;	//Image-Proのﾃﾞﾊﾞｯｸﾞ用？
        		
		//public const int DMODE_SZ = 3;	//ｽｷｬﾝﾓｰﾄﾞ(ﾊｰﾌ,ﾌﾙ,ｵﾌｾｯﾄ)     'added V2.0 by 鈴山    
        public const int DMODE_SZ = 4;      //ｽｷｬﾝﾓｰﾄﾞ(ﾊｰﾌ,ﾌﾙ,ｵﾌｾｯﾄ,ｼﾌﾄｵﾌｾｯﾄ)    'v18.00追加 byやまおか 2011/02/28 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07



        //コモン配列要素数		
		private const int MULTI_SZ = 5;	//ﾏﾙﾁｽﾗｲｽ                    'added V2.0 by 鈴山

        //*********************************************************************************************************************
        //  共通データ宣言
        //*********************************************************************************************************************

        //複数スライス対応		
		public static bool MultiSliceMode;	//ﾏﾙﾁｽﾗｲｽ校正（True:実行中,False:停止中）

        //各処理共通の変数
        //...幾何歪校正用		
		public static int VRT_Count;	                                //垂直線ﾜｲﾔの本数		
		public static double[] Mdt_Pitch = new double[MULTI_SZ + 1];	//検出器ﾋﾟｯﾁ(mm/画素)		
		public static double[] a0 = new double[MULTI_SZ + 1];	        //幾何歪校正用ﾌｨｯﾃｨﾝｸﾞ係数		
		public static double[] A1 = new double[MULTI_SZ + 1];	        //幾何歪校正用ﾌｨｯﾃｨﾝｸﾞ係数		
		public static double[] a2 = new double[MULTI_SZ + 1];	        //幾何歪校正用ﾌｨｯﾃｨﾝｸﾞ係数		
		public static double[] a3 = new double[MULTI_SZ + 1];	        //幾何歪校正用ﾌｨｯﾃｨﾝｸﾞ係数		
		public static double[] a4 = new double[MULTI_SZ + 1];	        //幾何歪校正用ﾌｨｯﾃｨﾝｸﾞ係数		
		public static double[] a5 = new double[MULTI_SZ + 1];	        //幾何歪校正用ﾌｨｯﾃｨﾝｸﾞ係数		
		public static int[] xls = new int[MULTI_SZ + 1];	            //有効画像開始画素		
		public static int[] xle = new int[MULTI_SZ + 1];	            //有効画像終了画素		
		public static double[] a0_bar = new double[MULTI_SZ + 1];	    //v11.2追加 by 間々田 2005/10/13		
		public static double[] b0_bar = new double[MULTI_SZ + 1];	    //v11.2追加 by 間々田 2005/10/13		
		public static int kmax;	//穴の個数                   'v11.2追加 by 山本　2005-12-21
        		
		public static double b0;	    //ｺｰﾝﾋﾞｰﾑ用幾何歪補正係数         'V3.0 append by 鈴山		
		public static double B1;	    //ｺｰﾝﾋﾞｰﾑ用幾何歪補正係数         'V3.0 append by 鈴山		
		public static double B2;	    //ｺｰﾝﾋﾞｰﾑ用幾何歪補正係数         'V3.0 append by 鈴山		
		public static double B3;	    //ｺｰﾝﾋﾞｰﾑ用幾何歪補正係数         'V3.0 append by 鈴山		
		public static double B4;	    //ｺｰﾝﾋﾞｰﾑ用幾何歪補正係数         'V3.0 append by 鈴山		
		public static double B5;	    //ｺｰﾝﾋﾞｰﾑ用幾何歪補正係数         'V3.0 append by 鈴山		
		public static double dpm;	    //m方向ﾃﾞｰﾀﾋﾟｯﾁ(mm)               'V3.0 append by 鈴山		
		public static float[] hizumi;	//幾何歪ﾃｰﾌﾞﾙ                     'V3.0 append by 鈴山

        //回転中心校正用		
		public static double[] xlc = new double[MULTI_SZ + 1];	                        //回転中心画素                    'Single → Double
        public static double[] xlc_sft = new double[MULTI_SZ + 1];                      //回転中心画素(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07 //追加2014/10/07hata_v19.51反映
		public static float[,] mfanangle = new float[MULTI_SZ + 1, DMODE_SZ + 1];	    //正味のﾌｧﾝ角
        
        //Public theta_s(MULTI_SZ, DMODE_SZ)         As Single   'ﾌｧﾝ角開始角度  'v11.2削除 by 間々田 2005/10/14 未使用
        //Public Theta_e(MULTI_SZ, DMODE_SZ)         As Single   'ﾌｧﾝ角終了角度  'v11.2削除 by 間々田 2005/10/14 未使用		
		public static float[,] Theta_c = new float[MULTI_SZ + 1, DMODE_SZ + 1];	        //回転中心角度(rad)		
		public static float[,] mcenter_channel = new float[MULTI_SZ + 1, DMODE_SZ + 1];	//等角度補正後のｾﾝﾀｰﾁｬﾝﾈﾙ		
		public static float[,] mchannel_pitch = new float[MULTI_SZ + 1, DMODE_SZ + 1];	//1ﾁｬﾝﾈﾙあたりのﾌｧﾝ角		
		public static float[] MaxSArea = new float[DMODE_SZ + 1];	                    //最大スキャンエリア(mm)  '←配列の必要はないが、一応残しておく V2.0 by 鈴山
        // V3.0 append by 鈴山 START		
		public static double delta_theta;	            //有効ﾌｧﾝ角(radian)		
        public static double delta_theta_sft;           //有効ﾌｧﾝ角(radian)(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07 //追加2014/10/07hata_v19.51反映

        public static double n0;	                    //画像中心対応ﾁｬﾝﾈﾙ		
        public static double n0_sft;                    //画像中心対応ﾁｬﾝﾈﾙ(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07    //追加2014/10/07hata_v19.51反映
        
        //public static int[] n1 = new int[2];	        //nの有効開始ﾁｬﾝﾈﾙ(0:非ｵﾌｾｯﾄ用,2:ｵﾌｾｯﾄｽｷｬﾝ用)		
		//public static int[] n2 = new int[2];	        //nの有効終了ﾁｬﾝﾈﾙ(0:非ｵﾌｾｯﾄ用,2:ｵﾌｾｯﾄｽｷｬﾝ用)		
		//public static double[] theta0 = new double[2];	//有効ﾌｧﾝ角(radian)(0:非ｵﾌｾｯﾄ用,2:ｵﾌｾｯﾄｽｷｬﾝ用)		
		//public static double[] theta01 = new double[2];	//有効ﾃﾞｰﾀ包含半角1(radian)(0:非ｵﾌｾｯﾄ用,2:ｵﾌｾｯﾄｽｷｬﾝ用)		
		//public static double[] theta02 = new double[2];	//有効ﾃﾞｰﾀ包含半角2(radian)(0:非ｵﾌｾｯﾄ用,2:ｵﾌｾｯﾄｽｷｬﾝ用)		
        public static int[] n1 = new int[3];	        //nの有効開始ﾁｬﾝﾈﾙ(0:非ｵﾌｾｯﾄ用,2:ｵﾌｾｯﾄｽｷｬﾝ用)   //変更2014/10/07hata_v19.51反映		
        public static int[] n2 = new int[3];	        //nの有効終了ﾁｬﾝﾈﾙ(0:非ｵﾌｾｯﾄ用,2:ｵﾌｾｯﾄｽｷｬﾝ用)   //変更2014/10/07hata_v19.51反映		
        public static double[] theta0 = new double[3];	//有効ﾌｧﾝ角(radian)(0:非ｵﾌｾｯﾄ用,2:ｵﾌｾｯﾄｽｷｬﾝ用)    //変更2014/10/07hata_v19.51反映		
        public static double[] theta01 = new double[3];	//有効ﾃﾞｰﾀ包含半角1(radian)(0:非ｵﾌｾｯﾄ用,2:ｵﾌｾｯﾄｽｷｬﾝ用)   //変更2014/10/07hata_v19.51反映		
        public static double[] theta02 = new double[3];	//有効ﾃﾞｰﾀ包含半角2(radian)(0:非ｵﾌｾｯﾄ用,2:ｵﾌｾｯﾄｽｷｬﾝ用)   //変更2014/10/07hata_v19.51反映		
        
        public static float thetaoff;	                //ｵﾌｾｯﾄ角(radian)    v11.4変更 Double→Single by 間々田 2006/03/16		
        public static float thetaoff_sft;               //ｵﾌｾｯﾄ角(radian)(シフト用)  'v18.00追加 byやまおか 2011/07/16 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07    //追加2014/10/07hata_v19.51反映

        public static int ioff;	                        //ｵﾌｾｯﾄ識別値(1:右ｵﾌｾｯﾄ,-1:左ｵﾌｾｯﾄ)		
		public static double nc;	                    //回転中心ﾁｬﾝﾈﾙ
        public static double nc_sft;                    //回転中心ﾁｬﾝﾈﾙ(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07  //追加2014/10/07hata_v19.51反映
        
        
        // V3.0 append by 鈴山 END
        //...汎用		
		public static int VIEW_N;	//ﾋﾞｭｰ数		
		//public static int v_size;	//画像縦ｻｲｽﾞ		
		//public static int h_size;	//画像横ｻｲｽﾞ

        //コモン
        //スキャン条件の設定値		
		public static int GFlg_ScanMode_D;	//ｽｷｬﾝﾓｰﾄﾞ(1:ﾊｰﾌ,2:ﾌﾙ,3:ｵﾌｾｯﾄ) ※寸法校正用  'V4.0 append by 鈴山 2001/02/05		
		public static float GVal_2R_D;	    //円柱直径画素数               ※寸法校正用  'V4.0 append by 鈴山 2001/02/05		
		public static float GVal_2R_D1;	    //1個目のワーク円柱直径画像数                      'v16.2/v17.0 追加 2010/02/23 by Isshiki
        // Rev 0-4 by 中島 START		
		public static float GVal_MScnArea;	    //ｽｷｬﾝ用スキャンエリア		
		public static float[] GVal_ScnAreaMax = new float[DMODE_SZ + 1];	//ｽｷｬﾝ用スキャンエリア最大		
		public static float[] GVal_ScnAreaMax1 = new float[DMODE_SZ + 1];	//1個目の画像のｽｷｬﾝ用スキャンエリア最大            'v16.2/v17.0 追加 2010/02/23 by Isshiki		
		public static float GVal_MaxConeArea;	//最大スキャンエリア(ｺｰﾝﾋﾞｰﾑCT用)                   'V3.0 append by 鈴山 2000/10/05		
		public static float GVal_Fid;	        //FID(ｵﾌｾｯﾄを含まない)		
		public static float GVal_Fcd;	        //FCD(ｵﾌｾｯﾄを含まない)		
		public static float GVal_Fcd_D;	        //寸法構成で求める値(ｵﾌｾｯﾄを含む)		
		public static float GVal_Fid_D;         //寸法校正後の求めるFID値(ｵﾌｾｯﾄを含む)              'v16.2/v17.0 追加　2009/10/23 by Isshiki		
		public static float GVal_Fcd1;	        //1個目の画像のFCD値                                'v16.2/v17.0 追加　2009/10/23 by Isshiki		
		public static float GVal_Fcd2;	        //2個目の画像のFCD値                                'v17.60追加 byやまおか 2011/06/10		
		public static float GVal_Fcd_O;	        //真のFCDｵﾌｾｯﾄ値                                    'v16.2/v17.0 追加　2009/10/23 by Isshiki		
		public static float GVal_Fid_O;	        //真のFIDｵﾌｾｯﾄ値                                    'v16.2/v17.0 追加　2009/10/23 by Isshiki
  
		//Public GVal_DistVal                             As Single   '寸法校正で求める値(ｵﾌｾｯﾄを含む)
		public static float[] GVal_DistVal = new float[3];	//寸法校正で求める値(ｵﾌｾｯﾄを含む)                   'v16.2/v17.0 配列に変更　2010/02/25 by Iwasawa		
        public static float[] GVal_DistValMeasure = new float[3];	//寸法校正で求める値(CT画像以外を使って計測した値) 'v23.12 配列に変更　2015/12/11 by 長野
        public static int GVal_ImgHsize;	//透視画像ｻｲｽﾞ(横)		
		public static int GVal_ImgVsize;	//透視画像ｻｲｽﾞ(縦)		
		public static float[] GVal_mdtpitch = new float[MULTI_SZ + 1];	//検出器ﾋﾟｯﾁ(mm/画素)
        // Rev 0-4 by 中島 END		
		public static int GFlg_MultiTube;		//X線管(0:130kV,1:225kV)                      'added V2.0 by 鈴山	
		public static int GFlg_MultiTube_R;	    //X線管(0:130kV,1:225kV) ※回転中心校正用    'v9.0 回転選択が可の場合は 0:テーブル回転 1:Ｘ線管回転 とする		
		public static int GFlg_MultiTube_D;	    //X線管(0:130kV,1:225kV) ※寸法校正用        'v9.0 回転選択が可の場合は 0:テーブル回転 1:Ｘ線管回転 とする

        //Public GVal_Mainch                              As Integer  'ﾁｬﾝﾈﾙ数           'v11.2削除 by 間々田 2005/10/14 未使用		
		public static int[] GVal_Xls = new int[MULTI_SZ + 1];	        //有効開始画素		
        public static int[] GVal_Xls_sft = new int[MULTI_SZ + 1];  //有効開始画素(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07  //追加2014/10/07hata_v19.51反映
        
        public static int[] GVal_Xle = new int[MULTI_SZ + 1];       //有効終了画素		
        public static int[] GVal_Xle_sft = new int[MULTI_SZ + 1];   //有効終了画素(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07 //追加2014/10/07hata_v19.51反映
		
        public static float[] GVal_ScanPosiA = new float[MULTI_SZ + 1];	//傾き		
		public static float[] GVal_ScanPosiB = new float[MULTI_SZ + 1];	//切片
		
		public static float[] theta0Max = new float[MULTI_SZ + 1];	    //最大ファン角		
		public static float theta0MaxCone;	                            //最大ファン角（コーンビーム用）

        //手入力結果の保持用		
		public static float GVal_SlWid_Rot;	    //校正処理時ｽﾗｲｽ厚(回転中心)                         'added V2.0 by 鈴山		
		public static float GVal_SlPix_Rot;	    //校正処理時ｽﾗｲｽ厚(回転中心)                         'added V2.0 by 鈴山		
		public static bool GFlg_Shading_Rot;	//回転中心校正のｼｪｰﾃﾞｨﾝｸﾞ補正(false:しない,true:する)'V4.0追加 by 鈴山 2001/01/24		
		//public static string DisFileName;	    //寸法画像ファイル名                                 'V4.0追加 by 鈴山 2001/02/01		
		public static int Xsize_D;	            //画像読み込み関数パラメータ                         'V4.0追加 by 鈴山 2001/02/01		
		public static int Ysize_D;	            //画像読み込み関数パラメータ                         'V4.0追加 by 鈴山 2001/02/01		
		public static bool IsAutoAtSpCorrect;	//スキャン位置校正時のモード（True:自動, False:手動）'v11.2追加 by 間々田 2006/01/10
		
		public static int IntegNumAtPos;	//スキャン位置校正時の積算枚数		
		public static int ViewNumAtRot;	    //回転中心校正時のビュー数		
		public static int IntegNumAtRot;	//回転中心校正時の積算枚数		
		public static int IntegNumAtOff;	//オフセット校正時の積算枚数

        //２値化閾値		
		public static int Threshold255_V;	//幾何歪校正用      by 中島 '99-8-17		
		public static int Threshold255_R;	//回転中心校正用    by 中島 '99-8-17		
		public static int Threshold255_D;	//寸法校正用        by 中島 '99-8-26
		
		public static float ConeRawSize;	//１ビューあたりの生データサイズ 'V7.0 append by 間々田 2003/09/20

        //画像バッファ		
		public static ushort[] VRT_IMAGE;	    //幾何歪校正画像		
        public static ushort[] CVRT_IMAGE;	    //幾何歪校正画像(ｼｰﾃﾞｨﾝｸﾞ補正後)		
        public static ushort[] BVRT_IMAGE;	    //幾何歪校正画像(2値化)          'added V2.0 by 鈴山		
        public static ushort[] BIN_IMAGE;	    //2値化表示画像		
        public static ushort[] VRT_THIN_IMAGE;	//幾何歪校正画像(細線化)		
        public static ushort[] CRT_IMAGE;	    //幾何歪校正画像(補正後)		
		public static byte[] FITTING_IMAGE;	//ﾌｨｯﾃｨﾝｸﾞ曲線画像		
		public static ushort[] RC_IMAGE;	    //回転中心校正画像               'added by 中島 '99-8-18		
        public static ushort[] RC_IMAGE_ORG;	//回転中心校正画像(ﾌｧｲﾙ保存用)   'added by 中島 99-5-19		
		public static ushort[] RC_IMAGE0;	    //回転中心校正画像(No.1読込用)   'added V2.0 by 鈴山		
		public static ushort[] RC_IMAGE1;	    //回転中心校正画像(No.2読込用)   'added V2.0 by 鈴山		
		public static ushort[] RC_IMAGE2;	    //回転中心校正画像(No.3読込用)   'added V2.0 by 鈴山		
		public static ushort[] RC_IMAGE3;	    //回転中心校正画像(No.4読込用)   'added V2.0 by 鈴山		
		public static ushort[] RC_IMAGE4;	    //回転中心校正画像(No.5読込用)   'added V2.0 by 鈴山		
		public static ushort[] RC_CONE;	    //回転中心校正画像(ｺｰﾝﾋﾞｰﾑCT用)  'V3.0 append by 鈴山		
        public static ushort[] RC_BILE;	    //回転中心校正画像(2値化)        'added by 稲葉 99-4-27
        //Public RC_THIN()            As Integer  '回転中心校正画像(細線化)      'v9.7 削除 by 間々田 2004/12/03
        //Public OFFSET_IMAGE()       As Integer 'ｵﾌｾｯﾄ校正画像		
		public static double[] OFFSET_IMAGE;	//ｵﾌｾｯﾄ校正画像                 'changed by 山本 2000-11-18		
		public static ushort[] GAIN_IMAGE;	        //ｹﾞｲﾝ校正画像
        //public static ushort[] GAIN_IMAGE_SFT;  //ｹﾞｲﾝ校正画像(シフト用)         'v18.00追加 byやまおか 2011/02/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07   //追加2014/10/07hata_v19.51反映
        public static ushort[] GAIN_IMAGE_SFT_R;  //ｹﾞｲﾝ校正画像(右シフト用)   //Rev23.20 左右シフト対応 by長野 2015/11/19
        public static ushort[] GAIN_IMAGE_SFT_L;  //ｹﾞｲﾝ校正画像(左シフト用)   //Rev23.20 左右シフト対応 by長野 2015/11/19
        public static uint[] Gain_Image_L;	      //ｹﾞｲﾝ校正画像(LONG型)           'v17.00added by 山本 2009-10-19		
        //public static uint[] Gain_Image_L_SFT;   //ｹﾞｲﾝ校正画像(LONG型)(シフト用) 'v18.00追加 byやまおか 2011/02/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07    //追加2014/10/07hata_v19.51反映
        public static uint[] Gain_Image_L_SFT_L;    //ｹﾞｲﾝ校正画像(LONG型)(左シフト用) 'v23.20 追加 by長野
        public static uint[] Gain_Image_L_SFT_R;    //ｹﾞｲﾝ校正画像(LONG型)(右シフト用) 'v23.20 追加 by長野
        public static ushort[] DISTANCE_IMAGE;	    //寸法校正登録用画像             'added by 中島'99-8-26		
		public static ushort[] POSITION_IMAGE;	    //ｽｷｬﾝ位置校正用画像             'added by 中島'99-8-18

        public static ushort[] GAIN_AIR_IMAGE_SFT_R;  //ゲイン校正後のエアデータ(右シフト) //Rev26.00 add by chouno 2017/01/06
        public static ushort[] GAIN_AIR_IMAGE_SFT_L;  //ゲイン校正後のエアデータ(左シフト) //Rev26.00 add by chouno 2017/01/06
        public static ushort[] GAIN_AIR_IMAGE;        //ゲイン校正後のエアデータ //Rev26.00 add by chouno 2017/01/06

        public static ushort[] W_IMAGE;	        //透視画像                       'added V6.0 by 間々田 2002-07-19
        //Public B_IMAGE()            As Integer  '透視画像(2値化)                'v15.0削除 by 間々田 2009-06-03		
        public static ushort[] Def_IMAGE;	        //フラットパネル欠陥画像         added V7.0 by 間々田 2003-09-24
        //Public Image_I() As Integer            'プロフィール表示用             'v9.7 削除 by 間々田 2004/11/04		
		public static int[] SUM_IMAGE;	        //v15.0 by 間々田 2009/01/27
        		
		public static float StartAngle;	        //データ収集開始角度 'V7.0 append by 間々田 2003-9-25

        //校正画像ファイル名（定数→ビニングによって中身が変わる変数に変更） 'V7.0 added by 間々田 2003/10/06		
		public static string OFF_CORRECT;	    //オフセット校正画像ファイル名		
		public static string GAIN_CORRECT;	    //ゲイン校正画像ファイル名		
        //public static string GAIN_CORRECT_SFT;  //ゲイン校正画像ファイル名(シフト用) 'v18.00追加 byやまおか 2011/02/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        public static string GAIN_CORRECT_SFT_R;  //ゲイン校正画像ファイル名(右シフト用) 'v23.20追加 by長野 2015/11/19
        public static string GAIN_CORRECT_SFT_L;  //ゲイン校正画像ファイル名(左シフト用) 'v23.20追加 by長野 2015/11/19
        public static string GAIN_CORRECT_L;	//ゲイン校正画像ファイル名（LONG型用）  'v17.00追加 2009-10-19　山本		
        //public static string GAIN_CORRECT_L_SFT;//ゲイン校正画像ファイル名（LONG型用）(シフト用) 'v18.00追加 byやまおか 2011/02/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        public static string GAIN_CORRECT_L_SFT_R;//ゲイン校正画像ファイル名（LONG型用）(右シフト用) 'v23.20追加 by長野 2015/11/19
        public static string GAIN_CORRECT_L_SFT_L;//ゲイン校正画像ファイル名（LONG型用）(左シフト用) 'v23.20追加 by長野 2015/11/19
        public static string GAIN_CORRECT_AIR;	//ゲイン校正画像ファイル名(一時ファイル)　'v19.00 2012/05/10 長野		
		public static string DEF_CORRECT;	    //欠陥画像ファイル名

        public static string GAIN_CORRECT_AIR_SFT_R;  //ゲイン校正後のエアデータ(右シフト)ファイル名 //Rev26.00 add by chouno 2017/01/16
        public static string GAIN_CORRECT_AIR_SFT_L;  //ゲイン校正後のエアデータ(左シフト)ファイル名 //Rev26.00 add by chouno 2017/01/16

        //Public OffsetMean  As Single     'v9.7削除 by 間々田 2004/12/08 未使用
		public static float GainMean;
        public static ushort GainMax;	    //added by 山本　2005-12-2		
		public static int GainCorFlag;	//added by 山本　2003-11-7　自動校正でゲイン校正を実行したかどうかのフラッグ		
		public static int AutoCorFlag;	//added by 山本　2010-03-06　自動校正中かどうかのフラッグ
        //Public CommCheckFlag As Long     'added by 山本　2004-8-4　校正中フラッグ  'v10.0削除 by 間々田 2005/02/14

        
        public static bool AutoJdgCorResultFlag = false;//Rev26.00 全自動構成時に校正結果を自動で判定するかどうか add by chouno 2017/01/06
        public static float myA = 0.0f;                 //Rev26.00 スキャン位置校正傾き add by chouno 2017/01/06
        public static float myB = 0.0f;                 //Rev26.00 スキャン位置構成切片 add by chouno 2017/01/06

        //２次元幾何歪補正関連配列		
		public static double[] gx;	//v11.2追加 by 間々田 2005/10/13		
		public static double[] gy;	//v11.2追加 by 間々田 2005/10/13		
		public static double[] gg;	//v11.2追加 by 間々田 2005/10/13		
		public static double[] gh;	//v11.2追加 by 間々田 2005/10/13

        //Rev25.00 シフトスキャン用画像輝度調整用係数 by長野 2016/09/24
        public static float ShiftFImageMagVal = 1.0f;

        public static float ShiftFImageMagValL = 1.0f;
        public static float ShiftFImageMagValR = 1.0f;

        //********************************************************************************
        //  外部関数宣言
        //********************************************************************************
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern double Determinant2C(ref double BUFF, short Mx_a);
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern double Determinant3C(ref double BUFF, short Mx_a);
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern double Determinant4C(ref double BUFF, short Mx_a);
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void DrawFittingCurve(ref byte Image, short h_mag, short v_mag, int h_size, int v_size, 
                                                    double a0, double A1, double a2, double a3, double a4, double a5, 
                                                    double W_DISTANCE, int VRT_Count, ref double xi, ref double ti);

        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int ImageOpen(ref short Image, string FileName, int h_size, int v_size);
        //仮設定(ushortを追加)　//2014/03/10hata
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ImageOpen(ref ushort Image, string FileName, int h_size, int v_size);
		
        
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int ImageSave(ref short Image, string FileName, int h_size, int v_size);
        //仮設定(ushortを追加)　//2014/03/10hata
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ImageSave(ref ushort Image, string FileName, int h_size, int v_size);

        
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int ImageOpen_long(ref int Image, string FileName, int h_size, int v_size);    //v17.00added by 山本　2009-10-19
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ImageOpen_long(ref uint Image, string FileName, int h_size, int v_size);    //v17.00added by 山本　2009-10-19


        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]		
		public static extern int ImageSave_long(ref int Image, string FileName, int h_size, int v_size);    //v17.00added by 山本　2009-10-19
		

        //追加 by 山本 2000-11-18
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int DoubleImageOpen(ref double Image, string FileName, int h_size, int v_size);
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int DoubleImageSave(ref double Image, string FileName, int h_size, int v_size);
		
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//public static extern int ImageCopyDtoUS(ref double D_IMAGE, ref short Image, int h_size, int v_size);
        public static extern int ImageCopyDtoUS(ref double D_IMAGE, ref ushort Image, int h_size, int v_size);

        //画像の最大値・最小値算出
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void GetMaxMin(ref short IN_BUFF, int h_size, int v_size, ref int Min, ref int Max);

        //仮設定(ushortを追加)
        //画像の最大値・最小値算出
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void GetMaxMin(ref ushort IN_BUFF, int h_size, int v_size, ref int Min, ref int Max);

        //画像の最大値・最小値算出(探索範囲有)//Rev23.11 追加 by長野 2015/12/12
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void GetMaxMinWithSearchRange(ref ushort IN_BUFF, int h_size, int v_size, int StartXIndex, int EndXIndex, int StartYIndex, int EndYIndex, ref int Min, ref int Max);

        //画像の最大値・最小値・平均・標準偏差算出(探索範囲有)//Rev23.12 追加 by長野 2015/12/12
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void GetStatisticalInfo(ref ushort IN_BUFF, int h_size, int v_size, int StartXIndex, int EndXIndex, int StartYIndex, int EndYIndex, ref int Min, ref int Max, ref float div, ref float mean);

        //画像の最大値・最小値・平均・標準偏差算出(探索範囲有) double型 //Rev26.00 追加 by長野 2016/01/07
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void GetStatisticalInfo_double(ref double IN_BUFF, int h_size, int v_size, int StartXIndex, int EndXIndex, int StartYIndex, int EndYIndex, ref int Min, ref int Max, ref float div, ref float mean);

////////added by 山本 2000-2-5
        //画像サイズ変更
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void ChangeImageSize(ref ushort IN_IMAGE, ref short OUT_IMAGE, int h_size, int v_size);		//第1引数の型変更 by 間々田 2003/09/26
////////added by 山本 2002-3-7

        //回転中心画素算出
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int GetRotationCenterPixelValue_C(ref ushort Image, ref ushort bile_image, ref double CenterPixel, int h_size, int View);

        //自動スキャン位置設定関数（断面画像上ROI指定）
        //Public Declare Function autotbl_set Lib "IICorrect.dll" (ByRef RoiCircle As Long, ByRef PosFx As Single, ByRef PosFy As Single, ByRef PosFCD As Single, ByRef PosTableY As Single) As Long '削除 by 間々田 2009/07/09

        //追加 by 間々田 2009/07/09
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int auto_tbl_set(ref int RoiCircle,
                                            float ideal_ftable_h_pos, 
                                            float ideal_ftable_v_pos, 
                                            float real_ftable_h_pos, 
                                            float real_ftable_v_pos, 
                                            ref float FCD1, 
                                            ref float table_h_xray1, 
                                            ref float FCD2, 
                                            ref float table_h_xray2);

        //追加 by 間々田 2009/07/09
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int auto_ftbl_set(ref int RoiCircle, 
                                            ref float ftable_h_pos, 
                                            ref float ftable_v_pos);

        //Rev23.30 外観カメラからのスキャン位置指定用に追加 by長野 2016/02/06 --->
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int auto_tbl_set_byExObsCam(ref int RoiCircle,
                                            int   matrix,
                                            float pixsize,
                                            float ideal_ftable_h_pos,
                                            float ideal_ftable_v_pos,
                                            float real_ftable_h_pos,
                                            float real_ftable_v_pos,
                                            float fcd,//Rev26.00 add by chouno 2017/01/17
                                            float fdd,//Rev26.00 add by chouno 2017/01/17
                                            ref float FCD1,
                                            ref float table_h_xray1,
                                            ref float FCD2,
                                            ref float table_h_xray2);

        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int auto_ftbl_set_byExObsCam(ref int RoiCircle,
                                            int matrix,
                                            float pixsize,
                                            ref float ftable_h_pos,
                                            ref float ftable_v_pos);
        //<---

        //２値化画像表示
        //Public Declare Sub BinarizeImage Lib "IICorrect.dll" (ByRef IN_IMAGE As Integer, ByRef OUT_IMAGE As Integer, ByVal h_size As Long, ByVal v_size As Long, ByVal Threshold As Long, ByVal VMAG_N As Long, ByVal HMAG_N As Long)
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void BinarizeImage(ref short IN_IMAGE, ref short OUT_IMAGE, int h_size, int v_size, int Threshold, float HMAG_N, float VMAG_N);		    //changed by 山本　2003-10-16　VMAG_NとHMAG_NをLongからSingleに変更
		
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern void BinarizeImage_signed(ref short IN_IMAGE, ref short OUT_IMAGE, int h_size, int v_size, int Threshold, float HMAG_N, float VMAG_N);		//v9.7追加 by 間々田 2004-12-09 符号付Short型配列対応
        public static extern void BinarizeImage_signed(ref ushort IN_IMAGE, ref ushort OUT_IMAGE, int h_size, int v_size, int Threshold, float HMAG_N, float VMAG_N);		//v9.7追加 by 間々田 2004-12-09 符号付Short型配列対応
        
        //校正データ収集関数
        //Public Declare Function II_Data_Acquire Lib "Pulsar.dll" (ByVal Xsize As Long, ByVal Ysize As Long, ByVal Itgnum As Long, ByVal Image_bit As Long, ByRef ima1 As Byte, ByRef ima2 As Integer, ByVal FlgPulOpen As Long, ByVal DCF As String, ByVal detector As Long) As Long    '第８引数をv_mag→ByVal DCF As Stringに変更、第９引数にdetectorを追加
        //Public Declare Function II_Data_Acquire Lib "Pulsar.dll" (ByVal xSize As Long, ByVal ySize As Long, ByVal Itgnum As Long, ByVal Image_bit As Long, ByRef ima1 As Byte, ByRef ima2 As Integer, ByVal FlgPulOpen As Long, ByVal DCF As String, ByVal detector As Long, ByVal FrameRate As Single) As Long   '引数にFrameRateを追加 changed by 山本　2004-8-3
        //Public Declare Function II_Data_Acquire Lib "Pulsar.dll" (ByVal xSize As Long, ByVal ySize As Long, ByVal Itgnum As Long, ByVal CallBackAddress As Long, ByRef ima2 As Integer, ByVal FlgPulOpen As Long, ByVal dcf As String, ByVal detector As Long, ByVal FrameRate As Single) As Long   'v10.0変更 引数Image_bitをCallBackAddressに変更、ima1削除 by 間々田 2005/01/31　'v17.50削除 by 間々田 2010/12/28

        //v15.0追加 by 間々田 2009/01/27
        //Public Declare Function MilCaptureStart Lib "Pulsar.dll" (ByVal Itgnum As Long, ByVal CallBackAddress As Long, ByRef ima2 As Integer, ByVal dcf As String) As Long
        //Public Declare Function MilCaptureStart Lib "Pulsar.dll" (ByVal hMil As Long, ByVal CallBackAddress As Long, ByRef SumImage As Long, ByVal IntegNum As Long) As Long   'v17.50削除 by 間々田 2010/12/28
        //Public Declare Function MilCaptureStart2 Lib "Pulsar.dll" (ByVal hMil As Long, ByVal CallBackAddress As Long, ByRef Image As Integer, ByRef SumImage As Long, ByVal IntegNum As Long) As Long                  'v17.50削除 by 間々田 2011/01/05
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int CaptureSetup(int hMil, int hPke);		                            //v17.50追加 by 間々田 2011/01/05
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]		
        public static extern void CaptureSeqStart(int hMil, int hPke);		                        //v17.50追加 by 間々田 2011/01/05
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void CaptureSeqStop(int hMil, int hPke);		                        //v17.50追加 by 間々田 2011/01/05
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void GetCaptureImage(int hMil, int hPke, ref short TransImage);		//v17.50追加 by 間々田 2011/01/05
        //Public Declare Function GetCaptureSumImage Lib "Pulsar.dll" (ByVal hMil As Long, ByVal hPke As Long, ByVal IntegNum As Long, ByRef TransImage As Integer, ByRef SumImage As Long, ByVal hCT30K As Long) As Long 'v17.50追加 by 間々田 2011/01/05
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int GetCaptureSumImage(int hMil, int hPke, int ViewNum, int IntegNum, ref short TransImage, ref int SumImage, int hCT30K);		//v17.66 引数追加 by 長野 2011/12/28
       
        //Public Declare Sub MilCaptureStop Lib "Pulsar.dll" ()  'v17.50削除 by 間々田 2011/01/05
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int MilOpen(string dcf, int Image_bit, int sync_mode);
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void MilClose(int hMil);
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void MilGrabWait(int hMil);		        //追加 by 間々田 2009/07/30

        //v17.00追加 by 山本 2009-09-12
        //Public Declare Function PkeOpen Lib "Pulsar.dll" (ByRef theImage As Integer, ByVal fpd_gain As Long, ByVal fpd_integ As Long) As Long
        //Public Declare Function PkeOpen Lib "Pulsar.dll" (ByRef theImage As Integer, ByVal fpd_gain As Long, ByVal fpd_integ As Long, ByVal v_capture_type As Long) As Long 'v17.10変更 byやまおか 2010/08/25
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int PkeOpen(int fpd_gain, int fpd_integ, int v_capture_type);		    //v17.50変更 by 間々田 2010/12/28
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int PkeSetOffsetData(int hPke, int file_read_flag, ref double OffsetData, int preload_flag);
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//public static extern int PkeSetGainData(int hPke, int file_read_flag, ref uint GainData, int preload_flag);
        public static extern int PkeSetGainData(int hPke, ref uint GainData, int preload_flag, string FileName);    //'v18.00変更 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int PkeSetPixelMap(int hPke, int preload_flag);
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void PkeClose(int hPke);
        //Public Declare Sub PkeCapture Lib "Pulsar.dll" (ByVal hPke As Long, ByRef DestImage As Integer, ByRef TransImage As Integer)
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void PkeCapture(int hPke, ref ushort TransImage);		                //v17.50変更 by 間々田 2011/01/05
        //Public Declare Sub PkeCaptureOnly Lib "Pulsar.dll" (ByVal hPke As Long, ByRef DestImage As Integer, ByRef TransImage As Integer)    'v17.02追加 byやまおか 2010/07/15 'v17.50削除 by 間々田 2010/12/28
        //Public Declare Sub PkeCaptureSetup Lib "Pulsar.dll" (ByVal hPke As Long, ByRef DestImage As Integer, ByRef TransImage As Integer)   'v17.02追加 byやまおか 2010/07/15  'v17.50削除 by 間々田 2011/01/05
        //Public Declare Sub PkeCaptureTransImage Lib "Pulsar.dll" (ByVal hPke As Long, ByVal CallBackAddress As Long, ByRef DestImage As Integer, ByRef TransImage As Integer)  'v17.50削除 by 間々田 2010/12/28
        //Public Declare Function PkeCaptureStart2 Lib "Pulsar.dll" (ByVal hPke As Long, ByVal CallBackAddress As Long, ByRef DestImage As Integer, ByRef TransImage As Integer, ByRef SumImage As Long, ByVal IntegNum As Long, ByVal RotFlag As Long) As Long 'v17.50削除 by 間々田 2010/12/28
        //Public Declare Function PkeCaptureOffset Lib "Pulsar.dll" (ByVal hPke As Long, ByVal CallBackAddress As Long, ByRef DestImage As Integer, ByRef TransImage As Integer, ByRef OffsetImage As Double, ByVal IntegNum As Long) As Long
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int PkeCaptureOffset(int hPke, int CallBackAddress, ref short TransImage, ref double OffsetImage, int IntegNum);
		//v17.50変更 by 間々田 2010/12/28
        //Public Declare Function PkeCaptureGain Lib "Pulsar.dll" (ByVal hPke As Long, ByVal CallBackAddress As Long, ByRef DestImage As Integer, ByRef TransImage As Integer, ByRef Gain_Image_L As Long, ByVal IntegNum As Long) As Long
        //Public Declare Function PkeCaptureGain Lib "Pulsar.dll" (ByVal hPke As Long, ByVal CallBackAddress As Long, ByRef TransImage As Integer, ByRef Gain_Image_L As Long, ByVal IntegNum As Long) As Long 'v17.50変更 by 間々田 2010/12/28
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int PkeCaptureGain(int hPke, int CallBackAddress, ref short TransImage, ref int Gain_Image_L, int ViewNum, int IntegNum);		//v17.66 引数変更 by 長野 2011/12/28
        //Public Declare Sub PkeCaptureStop Lib "Pulsar.dll" (ByVal hPke As Long)    'v17.50削除 廃止 by 間々田 2010/12/28
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int PkeSetGainFrameRate(int hPke, int fpd_gain, int fpd_integ);
        //Public Declare Function PkeGetIntegListFromINI Lib "Pulsar.dll" (ByVal hPke As Long, ByRef fpd_integlist As Double) As Long   'v17.10追加 byやまおか 2010/08/24 'v17.50削除 廃止 by 間々田 2010/12/28
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern double PkeGetIntegTime(int fpd_integ);		            //v19.10 追加 by長野 2102/07/30

        //Public Declare Function CreateTransImageMap Lib "Pulsar.dll" () As Long
		[DllImport("PulsarHelper.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]		
        public static extern int CreateTransImageMap(int hSize, int vSize);		//v17.50変更 by 間々田 2011/01/05
        //Public Declare Sub DestroyTransImageMap Lib "Pulsar.dll" (ByVal hMap As Long)
		[DllImport("PulsarHelper.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]		
        public static extern void DestroyTransImageMap(int hMap);		        //v17.50変更 by 間々田 2010/12/22
        //Public Declare Sub GetTransImage Lib "Pulsar.dll" (ByRef theImage As Integer, ByVal SIZE As Long)
		[DllImport("PulsarHelper.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]		
        public static extern void GetTransImage(ref ushort theImage);		    //v17.50変更 by 間々田 2010/12/22
        //Public Declare Sub MilCapture Lib "Pulsar.dll" (ByVal hMil As Long, ByRef theImage As Integer) 'v17.50削除 by 間々田 2010/12/22
		[DllImport("PulsarHelper.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]		
        public static extern void SetCancel(int flag);		                    //v17.50追加 by 間々田 2011/02/15

		[DllImport("PulsarHelper.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int CreateUserStopMap();		                    //v17.50追加 by 間々田 2011/01/27
		[DllImport("PulsarHelper.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void DestroyUserStopMap(int hMap);		            //v17.50追加 by 間々田 2011/01/27

        //Public Declare Sub MilSetGrabMode Lib "Pulsar.dll" (ByVal hMil As Long, ByVal Mode As Double)
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void MilSetGrabMode(int hMil, int Mode);		    //v17.50変更 by 間々田 2010/12/22

        //動画保存用の関数       'v17.30追加 MIL9対応 byやまおか 2010/09/22
        //Public Declare Function Mil9AllocUserArry Lib "Pulsar.dll" (ByVal StartIndex As Long, ByVal hMil As Long, ByVal Width As Long, ByVal fphm As Single, ByVal Height As Long, ByVal fpvm As Single) As Long
        //Public Declare Function Mil9CopyUserArry Lib "Pulsar.dll" (ByVal MovieCount As Long, ByRef ImageBuff As Byte, ByVal hMil As Long, ByVal Width As Long, ByVal fphm As Single, ByVal Height As Long, ByVal fpvm As Single) As Long
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]		
        public static extern int Mil9AllocUserArry(int StartIndex, int hMil, int Width, float fphm, int Height, float fpvm, int ZoomScale);		//v17.65 引数追加　by 長野 2011.11.30
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Mil9CopyUserArry(int MovieCount, ref byte ImageBuff, int hMil, int Width, float fphm, int Height, float fpvm, int ZoomScale);		//v17.65 引数追加 by 長野 2011.11.30
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//public static extern int Mil9SaveMovie(int MovieCount, string FileName, double FrameRate);
        public static extern int Mil9SaveMovie(int MovieCount, string FileName,int DlgFilterIndex ,double FrameRate); //Rev22.00 引数追加 by長野 2015/07/02
        //Public Declare Function MilHostOpen Lib "Pulsar.dll" (ByVal Image_bit As Long) As Long
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int MilHostOpen(int xSize, int ySize);		        //v17.50変更 by 間々田 2011/01/20
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Mil9ClearUserArry();
      
        //v15.0追加 by 間々田 2009/01/27
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void DivImage_short(ref int sum_img, ref ushort ave_img, int N, int h_size, int v_size);
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void DivImage_double(ref int sum_img, ref double ave_img, int N, int h_size, int v_size);

        //Public Declare Function Offset_Data_Acquire Lib "Pulsar.dll" (ByVal Xsize As Long, ByVal Ysize As Long, ByVal Itgnum As Long, ByVal Image_bit As Long, ByRef ima2 As Double, ByVal DCF As String, ByVal detector As Long) As Long                                               '第６引数をv_mag→ByVal DCF As Stringに変更、第７引数にdetectorを追加
        //Public Declare Function Offset_Data_Acquire Lib "Pulsar.dll" (ByVal xSize As Long, ByVal ySize As Long, ByVal Itgnum As Long, ByVal Image_bit As Long, ByRef ima2 As Double, ByVal dcf As String, ByVal detector As Long, ByVal FrameRate As Single) As Long                     '第8引数にFrameRateを追加　'v17.50削除 by 間々田 2010/12/28

        //回転中心校正画像取込み関数
        //Public Declare Function RotationCenter_Data_Acquire Lib "Pulsar.dll" (ByVal h_size As Long, ByVal v_size As Long, ByVal Itgnum As Long, ByRef RC_IMAGE0 As Integer, ByRef RC_IMAGE1 As Integer, ByRef RC_IMAGE2 As Integer, ByRef RC_IMAGE3 As Integer, ByRef RC_IMAGE4 As Integer, ByVal hDevID As Long, ByRef mrc As Long, ByVal View As Long, ByVal multislice As Long, ByVal table_rotation As Long, ByVal sw As Single, ByVal connect As Long, ByRef RC_CONE As Integer, ByRef Def_IMAGE As Integer, ByVal DCF As String, ByRef GAIN_IMAGE As Integer, ByVal detector As Long, ByVal FrameRate As Single) As Long
        //Public Declare Function RotationCenter_Data_Acquire Lib "Pulsar.dll" (ByVal h_size As Long, ByVal v_size As Long, ByVal Itgnum As Long, ByRef RC_IMAGE0 As Integer, ByRef RC_IMAGE1 As Integer, ByRef RC_IMAGE2 As Integer, ByRef RC_IMAGE3 As Integer, ByRef RC_IMAGE4 As Integer, ByVal hDevID As Long, ByRef mrc As Long, ByVal View As Long, ByVal multislice As Long, ByVal table_rotation As Long, ByVal sw As Single, ByVal connect As Long, ByRef RC_CONE As Integer, ByRef Def_IMAGE As Integer, ByVal DCF As String, ByRef GAIN_IMAGE As Integer, ByVal detector As Long, ByVal FrameRate As Single, ByVal RotateSelect As Long, ByVal c_cw_ccw As Long) As Long 'v9.0 第22～23引数を追加 by 間々田 2004/02/05
        //Public Declare Function RotationCenter_Data_Acquire Lib "Pulsar.dll" (ByVal h_size As Long, ByVal v_size As Long, ByVal Itgnum As Long, ByRef RC_IMAGE0 As Integer, ByRef RC_IMAGE1 As Integer, ByRef RC_IMAGE2 As Integer, ByRef RC_IMAGE3 As Integer, ByRef RC_IMAGE4 As Integer, ByVal hDevID As Long, ByRef mrc As Long, ByVal View As Long, ByVal multislice As Long, ByVal table_rotation As Long, ByVal sw As Single, ByVal connect As Long, ByRef RC_CONE As Integer, ByRef Def_IMAGE As Integer, ByVal DCF As String, ByRef GAIN_IMAGE As Integer, ByVal detector As Long, ByVal FrameRate As Single, ByVal RotateSelect As Long, ByVal c_cw_ccw As Long, ByVal xrot_stop_pos As Long) As Long 'v9.1 xrot_stop_posを追加 by 間々田 2004/05/20
        //Public Declare Function RotationCenter_Data_Acquire Lib "Pulsar.dll" (ByVal h_size As Long, ByVal v_size As Long, ByVal Itgnum As Long, ByRef RC_IMAGE0 As Integer, ByRef RC_IMAGE1 As Integer, ByRef RC_IMAGE2 As Integer, ByRef RC_IMAGE3 As Integer, ByRef RC_IMAGE4 As Integer, ByVal hDevID As Long, ByRef mrc As Long, ByVal View As Long, ByVal multislice As Long, ByVal table_rotation As Long, ByVal sw As Single, ByVal connect As Long, ByRef RC_CONE As Integer, ByRef Def_IMAGE As Integer, ByVal dcf As String, ByRef GAIN_IMAGE As Integer, ByVal detector As Long, ByVal FrameRate As Single, ByVal RotateSelect As Long, ByVal c_cw_ccw As Long, ByVal xrot_stop_pos As Long, ByVal CallBackAddress As Long) As Long 'v10.0 CallBackAddressを追加 by 間々田 2005/02/02
        //Public Declare Function RotationCenter_Data_Acquire Lib "Pulsar.dll" (ByVal hMil As Long, ByVal h_size As Long, ByVal v_size As Long, ByVal Itgnum As Long, ByRef RC_IMAGE0 As Integer, ByRef RC_IMAGE1 As Integer, ByRef RC_IMAGE2 As Integer, ByRef RC_IMAGE3 As Integer, ByRef RC_IMAGE4 As Integer, ByVal hDevID As Long, ByRef mrc As Long, ByVal View As Long, ByVal multislice As Long, ByVal table_rotation As Long, ByVal SW As Single, ByVal connect As Long, ByRef RC_CONE As Integer, ByRef Def_IMAGE As Integer, ByVal dcf As String, ByRef GAIN_IMAGE As Integer, ByVal detector As Long, ByVal FrameRate As Single, ByVal RotateSelect As Long, ByVal c_cw_ccw As Long, ByVal xrot_stop_pos As Long, ByVal CallBackAddress As Long) As Long 'v10.0 CallBackAddressを追加 by 間々田 2005/02/02
        //Public Declare Function RotationCenter_Data_Acquire Lib "Pulsar.dll" (ByVal hMil As Long, ByVal hPke As Long, ByRef DestImage As Integer, ByRef TransImage As Integer, ByVal h_size As Long, ByVal v_size As Long, ByVal Itgnum As Long, ByRef RC_IMAGE0 As Integer, ByRef RC_IMAGE1 As Integer, ByRef RC_IMAGE2 As Integer, ByRef RC_IMAGE3 As Integer, ByRef RC_IMAGE4 As Integer, ByVal hDevID As Long, ByRef mrc As Long, ByVal View As Long, ByVal multislice As Long, ByVal table_rotation As Long, ByVal SW As Single, ByVal connect As Long, ByRef RC_CONE As Integer, ByRef Def_IMAGE As Integer, ByVal dcf As String, ByRef GAIN_IMAGE As Integer, ByVal detector As Long, ByVal FrameRate As Single, ByVal RotateSelect As Long, ByVal c_cw_ccw As Long, ByVal xrot_stop_pos As Long, ByVal CallBackAddress As Long) As Long 'v17.00changed by 山本　2009-09-16
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]		
        public static extern int RotationCenter_Data_Acquire(int hMil, int hPke, ref short TransImage, int h_size, int v_size, int Itgnum, ref short RC_IMAGE0, ref short RC_IMAGE1, ref short RC_IMAGE2, ref short RC_IMAGE3,
		ref short RC_IMAGE4, int hDevID, ref int mrc, int View, int multislice, int table_rotation, float SW, int connect, ref short RC_CONE, ref short Def_IMAGE,
		ref short GAIN_IMAGE, int detector, float FrameRate, int RotateSelect, int c_cw_ccw, int xrot_stop_pos, int CallBackAddress);		//v17.50引数変更 by 間々田 2011/01/13 DestImage, dcf削除

        //ゲイン校正画像取込み関数
        //Public Declare Function Gain_Data_Acquire Lib "Pulsar.dll" (ByVal h_size As Long, ByVal v_size As Long, ByVal Itgnum As Long, ByVal Image_bit As Long, ByRef ima1 As Byte, ByRef ima2 As Integer, ByVal hDevID As Long, ByRef mrc As Long, ByVal View As Long, ByVal table_rotation As Long, ByVal DCF As String, ByVal detector As Long, ByVal FrameRate As Single) As Long    '第11引数変更、第12～13引数を追加
        //Public Declare Function Gain_Data_Acquire Lib "Pulsar.dll" (ByVal h_size As Long, ByVal v_size As Long, ByVal Itgnum As Long, ByVal CallBackAddress As Long, ByRef ima2 As Integer, ByVal hDevID As Long, ByRef mrc As Long, ByVal View As Long, ByVal table_rotation As Long, ByVal dcf As String, ByVal detector As Long, ByVal FrameRate As Single) As Long    'v10.0変更 引数Image_bitをCallBackAddressに変更、ima1削除 by 間々田 2005/01/31
        //Public Declare Function Gain_Data_Acquire Lib "Pulsar.dll" (ByVal hMil As Long, ByVal Itgnum As Long, ByVal CallBackAddress As Long, ByRef ima2 As Integer, ByVal hDevID As Long, ByRef mrc As Long, ByVal View As Long, ByVal table_rotation As Long, ByVal detector As Long, ByVal FrameRate As Single) As Long    'v10.0変更 引数Image_bitをCallBackAddressに変更、ima1削除 by 間々田 2005/01/31
        //Public Declare Function Gain_Data_Acquire Lib "Pulsar.dll" (ByVal hMil As Long, ByVal Itgnum As Long, ByVal CallBackAddress As Long, ByRef ima2 As Long, ByVal hDevID As Long, ByRef mrc As Long, ByVal View As Long, ByVal table_rotation As Long, ByVal detector As Long, ByVal FrameRate As Single) As Long    'v10.0変更 引数Image_bitをCallBackAddressに変更、ima1削除 by 間々田 2005/01/31
        //Public Declare Function Gain_Data_Acquire Lib "Pulsar.dll" (ByVal hMil As Long, ByVal Itgnum As Long, ByVal CallBackAddress As Long, ByRef Image As Integer, ByRef ima2 As Long, ByVal hDevID As Long, ByRef mrc As Long, ByVal View As Long, ByVal table_rotation As Long, ByVal detector As Long, ByVal FrameRate As Single) As Long    'v10.0変更 引数Image_bitをCallBackAddressに変更、ima1削除 by 間々田 2005/01/31
        //Public Declare Function Gain_Data_Acquire Lib "Pulsar.dll" (ByVal hMil As Long, ByVal hPke As Long, ByRef DestImage As Integer, ByRef TransImage As Integer, ByVal Itgnum As Long, ByVal CallBackAddress As Long, ByRef Image As Integer, ByRef ima2 As Long, ByVal hDevID As Long, ByRef mrc As Long, ByVal View As Long, ByVal table_rotation As Long, ByVal detector As Long, ByVal FrameRate As Single) As Long 'v17.00changed by 山本　2009-09-16
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int Gain_Data_Acquire(int hMil, int hPke, ref short TransImage, int Itgnum, int CallBackAddress, ref short Image, ref int ima2, int hDevID, ref int mrc, int View,
		int table_rotation, int detector, float FrameRate);		//v17.50変更 DestImage削除 by 間々田 2011/01/14

        //自動スキャン位置校正
        //Public Declare Function AutoScanpositionEntry Lib "Pulsar.dll" (ByVal H_SIZE As Long, ByVal V_SIZE As Long, ByVal itg_num As Long, ByRef scan_posi_a As Single, ByRef scan_posi_b As Single, ByVal hDevID As Long, ByRef mrc As Long, ByVal fud_step As Single, ByVal rud_step As Single, ByRef fud_start As Single, ByRef fud_end As Single, ByVal rud_start As Single, ByVal rud_end As Single, ByRef IMAGE As Byte, ByVal DCF As String) As Long   '第15引数変更
        //Public Declare Function AutoScanpositionEntry Lib "Pulsar.dll" (ByVal h_size As Long, ByVal v_size As Long, ByVal itg_num As Long, ByRef scan_posi_a As Single, ByRef scan_posi_b As Single, ByVal hDevID As Long, ByRef mrc As Long, ByVal fud_step As Single, ByVal rud_step As Single, ByRef fud_start As Single, ByRef fud_end As Single, ByVal rud_start As Single, ByVal rud_end As Single, ByRef IMAGE As Integer, ByVal DCF As String, ByVal Image_bit As Long, ByVal detector As Long, ByRef GAIN_IMAGE As Integer, ByRef Def_IMAGE As Integer) As Long   '第16,17,18,19引数追加,第15引数の型を変更 by 山本　2003-10-4
        //Public Declare Function AutoScanpositionEntry Lib "Pulsar.dll" (ByVal h_size As Long, ByVal v_size As Long, ByVal itg_num As Long, ByRef scan_posi_a As Single, ByRef scan_posi_b As Single, ByVal hDevID As Long, ByRef mrc As Long, ByVal fud_step As Single, ByVal rud_step As Single, ByRef fud_start As Single, ByRef fud_end As Single, ByVal rud_start As Single, ByVal rud_end As Single, ByRef IMAGE As Integer, ByVal DCF As String, ByVal Image_bit As Long, ByVal detector As Long, ByRef GAIN_IMAGE As Integer, ByRef Def_IMAGE As Integer, ByVal FrameRate As Single) As Long  '引数にFrameRateを追加　2004-8-3
        //Public Declare Function AutoScanpositionEntry Lib "Pulsar.dll" (ByVal h_size As Long, ByVal v_size As Long, ByVal itg_num As Long, ByRef scan_posi_a As Single, ByRef scan_posi_b As Single, ByVal hDevID As Long, ByRef mrc As Long, ByVal fud_step As Single, ByVal rud_step As Single, ByRef fud_start As Single, ByRef fud_end As Single, ByVal rud_start As Single, ByVal rud_end As Single, ByRef IMAGE As Integer, ByVal dcf As String, ByVal Image_bit As Long, ByVal detector As Long, ByRef GAIN_IMAGE As Integer, ByRef Def_IMAGE As Integer, ByVal FrameRate As Single, ByVal CallBackAddress As Long) As Long  'CallBackAddressを追加　2004-8-3
        //Public Declare Function AutoScanpositionEntry Lib "Pulsar.dll" (ByVal h_size As Long, ByVal v_size As Long, ByVal itg_num As Long, ByRef scan_posi_a As Single, ByRef scan_posi_b As Single, ByVal hDevID As Long, ByRef mrc As Long, ByVal fud_step As Single, ByVal rud_step As Single, ByRef fud_start As Single, ByRef fud_end As Single, ByVal rud_start As Single, ByVal rud_end As Single, ByRef IMAGE As Integer, ByVal dcf As String, ByVal Image_bit As Long, ByVal detector As Long, ByRef GAIN_IMAGE As Integer, ByRef Def_IMAGE As Integer, ByVal FrameRate As Single, ByVal CallBackAddress As Long, _
        //'                                            ByVal ist As Long, ByVal ied As Long, ByVal jst As Long, ByVal jed As Long, gi As Single, git As Single, gj As Single, gjt As Single, Qidjd As Long, Qidp1jd As Long, Qidjdp1 As Long, Qidp1jdp1 As Long, ByVal full_distortion As Long) As Long        '引数を追加 by 間々田 2005/10/19
        //Public Declare Function AutoScanpositionEntry Lib "Pulsar.dll" (ByVal h_size As Long, ByVal v_size As Long, ByVal itg_num As Long, ByVal hDevID As Long, ByRef mrc As Long, ByVal fud_step As Single, ByVal rud_step As Single, ByRef fud_start As Single, ByRef fud_end As Single, ByVal rud_start As Single, ByVal rud_end As Single, ByRef IMAGE As Integer, ByVal dcf As String, ByVal Image_bit As Long, ByVal detector As Long, ByRef GAIN_IMAGE As Integer, ByRef Def_IMAGE As Integer, ByVal FrameRate As Single, ByVal CallBackAddress As Long, ByVal full_distortion As Long) As Long '変更 by 間々田 2006/01/11
        //Public Declare Function AutoScanpositionEntry Lib "Pulsar.dll" (ByVal hMil As Long, ByVal hDevID As Long, ByRef mrc As Long, ByVal fud_step As Single, ByVal rud_step As Single, ByRef fud_start As Single, ByRef fud_end As Single, ByVal rud_start As Single, ByVal rud_end As Single, ByVal detector As Long, ByRef GAIN_IMAGE As Integer, ByRef Def_IMAGE As Integer, ByVal CallBackAddress As Long) As Long '変更 by 間々田 2006/01/11
        //Public Declare Function AutoScanpositionEntry Lib "Pulsar.dll" (ByVal hMil As Long, ByVal hPke As Long, ByRef DestImage As Integer, ByRef TransImage As Integer, ByVal hDevID As Long, ByRef mrc As Long, ByVal fud_step As Single, ByVal rud_step As Single, ByRef fud_start As Single, ByRef fud_end As Single, ByVal rud_start As Single, ByVal rud_end As Single, ByVal detector As Long, ByRef GAIN_IMAGE As Integer, ByRef Def_IMAGE As Integer, ByVal CallBackAddress As Long) As Long 'v17.00changed by 山本 2009-09-16
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]	
        public static extern int AutoScanpositionEntry(int hMil, int hPke, ref short TransImage, int hDevID, ref int mrc, float fud_step, float rud_step, ref float fud_start, ref float fud_end, float rud_start,
		float rud_end, int detector, ref short GAIN_IMAGE, ref short Def_IMAGE, int CallBackAddress);		//v17.50変更 DestImage削除 by 間々田 2011/01/14
		[DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		//public static extern int AcqScanPosition(int h_size, int v_size, ref float scan_posi_a, ref float scan_posi_b, ref short Image);        //追加 by 間々田 2006/01/11
        public static extern int AcqScanPosition(int h_size, int v_size, ref float scan_posi_a, ref float scan_posi_b, ref ushort Image);        //追加 by 間々田 2006/01/11
        
        //追加
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]				
        public static extern void FpdDefDetect(ref ushort GAIN_IMAGE, ref double OFF_IMAGE, ref ushort Def_IMAGE, int h_size, int v_size, float thre, float thre_n, float thre_line, int BlockSize);
		
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void FpdDefCorrect_short(ref ushort FPD_IMAGE, ref ushort Def_IMAGE, int h_size, int v_size, int vs, int ve);

        //Public Declare Sub FpdGainCorrect Lib "IICorrect.dll" (ByRef RAW_IMAGE As Integer, ByRef GAIN_IMAGE As Integer, ByVal H_SIZE As Long, ByVal V_SIZE As Long)
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void FpdGainCorrect(ref ushort RAW_IMAGE, ref ushort GAIN_IMAGE, int h_size, int v_size, int adv);		//第５引数 adv を追加 2003/10/09 by 間々田

		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void Cal_Mean_short(ref ushort RAW_IMAGE, int h_size, int v_size, ref float mean);
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void Cal_Mean_short2(ref ushort RAW_IMAGE, int h_size, int v_size, int h_trim, int v_trim, ref float mean);
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void Cal_Max_short(ref ushort RAW_IMAGE, int h_size, int v_size, ref ushort Max);		//added by 山本　2005-12-2
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void Cal_Mean_double(ref double RAW_IMAGE, int h_size, int v_size, ref float mean);		//v17.00added by 山本　2009-10-09
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int ImageCopy_short(ref ushort IN_IMAGE, ref ushort OUT_IMAGE, int h_size, int v_size);      //v17.00added by 山本　2010-03-03    'v17.02宣言修正 byやまおか 2010-07-12

        //コントラスト向上（画像データを符号なしで受け取る）
        //'Public Declare Sub ChangeFullRange Lib "Videocap.dll"    '変更 by 中島 '99-8-17
		[DllImport("ProfocxTool.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void ChangeFullRange(ref short IN_BUFF, int hSize, int vSize, int Min, int Max);
        //仮設定(ushortを追加)　//2014/03/10hata
        [DllImport("ProfocxTool.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void ChangeFullRange(ref ushort IN_BUFF, int hSize, int vSize, int Min, int Max);

        //コントラスト向上(画像データを符号付きで受け取る）
        //Public Declare Sub ChangeFullRange_Short Lib "IICorrect.dll" (ByRef IN_BUFF As Integer, ByVal hSize As Long, ByVal vSize As Long, ByVal Min As Long, ByVal Max As Long)
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]		
        public static extern void ChangeFullRange_Short(ref short IN_BUFF, int hSize, int vSize, int Min, int Max, float GAMMA);
		
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void ChangeFullRange_UShort(ref short IN_BUFF, int hSize, int vSize, int Min, int Max);		//v17.20追加 透視画像16bit対応 byやまおか 2010/09/16
        //仮設定(ushortを追加)　//2014/03/10hata
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void ChangeFullRange_UShort(ref ushort IN_BUFF, int hSize, int vSize, int Min, int Max);		//v17.20追加 透視画像16bit対応 byやまおか 2010/09/16

        //v10.2追加 by 間々田 2005/06/23 ChangeFullRange_Shortによる変換の逆の関数
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void ChangeToCTImage(ref short IN_BUFF, int hSize, int vSize, int Min, int Max);

        //v10.2追加 by 間々田 2005/06/23 ChangeFullRange_Shortによる変換の逆の関数
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void ChangeToCTImage(ref ushort IN_BUFF, int hSize, int vSize, int Min, int Max);		

        //v11.2追加ここから by 間々田 2005/10/12
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int calculate_fulldistortion_fitting(ref int kmax, 
                                                                    ref double x, ref double y, ref double area, 
                                                                    int hSize, int vSize, 
                                                                    double dPitch, int kv, 
                                                                    ref double alk, ref double blk,
	                                                                ref int ist, ref int ied, 
                                                                    ref int jst, ref int jed, 
                                                                    ref double G, ref double h);
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int Cal_1d_Distortion_Parameter(int kv, 
                                                                int h_size, 
                                                                int v_size, 
                                                                double a_0, double b_0, 
                                                                ref double alk, ref double blk, 
                                                                int kmax, 
                                                                ref double x, ref double y,
		                                                        ref double G, ref double h, 
                                                                ref double a0_bar, ref double b0_bar, 
                                                                ref int xls, ref int xle, 
                                                                ref double a0, 
                                                                ref double A1, 
                                                                ref double a2, 
                                                                ref double a3,
		                                                        ref double a4, 
                                                                ref double a5);
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void make_2d_fullindiv_table(int hSize, int vSize, 
                                                            int ist, int ied, 
                                                            int jst, int jed, 
                                                            int kv, 
                                                            ref double alk, ref double blk, 
                                                            ref float gi, ref float git, ref float gj, ref float gjt, 
                                                            ref int Qidjd, ref int Qidp1jd, ref int Qidjdp1, ref int Qidp1jdp1);
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void cone_fullindiv_crct(int hSize, 
                                                        int ist, int ied, 
                                                        int jst, int jed, 
                                                        ref float gi, ref float git, ref float gj, ref float gjt, 
                                                        ref int Qidjd, ref int Qidp1jd, ref int Qidjdp1, ref int Qidp1jdp1, 
                                                        ref ushort gDat, ref ushort pDat);
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void Zero_ImageMargin(ref ushort BUFF, int h_size, int v_size, int h_margin, int v_margin);             //v11.2追加 by 間々田 2005/11/11
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void Print_CenterPoint(ref ushort Image, int h_size, int v_size, int kmax, ref double x, ref double y); //v11.2追加 by 山本　2005-11-29
        //v11.2追加ここまで by 間々田 2005/10/12

        //追加2014/10/07hata_v19.51反映
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern double arctan(double x);        //v18.00追加 byやまおか 2011/03/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

        //左右切り落とし画像部分取り出し
        //'Public Declare Sub Pic_Imgside Lib "Videocap.dll"    '変更 by 中島 '99-8-17
        [DllImport("ProfocxTool.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void Pic_Imgside(ref ushort IN_BUFF, int FrmSiz, int hSize, int vSize);


        //********************************************************************************
        //機    能  ：  ｺｰﾝﾋﾞｰﾑ用幾何歪ﾊﾟﾗﾒｰﾀ計算
        //              変数名           [I/O] 型        内容
        //引    数  ：  b1               [I/ ] Double    幾何歪ﾌｨｯﾃｨﾝｸﾞ係数 B1
        //              v_mag            [I/ ] Long      縦方向倍率
        //              a                [I/ ] Double    ｽｷｬﾝ位置を示す傾き
        //              dpm              [ /O] Double    m方向ﾃﾞｰﾀﾋﾟｯﾁ(mm)
        //戻 り 値  ：  なし
        //補    足  ：  計算に必要なﾊﾟﾗﾒｰﾀと計算結果はすべて引数でやりとりする。
        //
        //履    歴  ：  V3.0   00/08/11  (SI1)鈴山       新規作成
        //              v9.7   04/11/25  (SI4)間々田     未使用になった引数v_magを削除
        //********************************************************************************
        //Private Sub Cal_ConeBeamParameter(ByVal B1 As Double, ByVal v_mag As Long, ByVal a As Double, ByRef dpm As Double)
        //Private Sub Cal_ConeBeamParameter(ByVal B1 As Double, ByVal a As Double, ByRef dpm As Double)
		//v11.2 変更 by 間々田 2005/10/20
		public static void Cal_ConeBeamParameter(double B1, double a, ref double dpm)
		{
			//パラメータ
			double Dpi = 0;
			double Dpj = 0;
			double b0_dash = 0;		//added by 山本　2002-6-18
			double e_dash = 0;		//added by 山本　2002-6-18
			int kv = 0;			    //added by 山本　2002-6-18

            //2014/11/13hata キャストの修正
            //kv = (int)(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);      //v7.0 FPD対応 by 間々田 2003/09/25
            kv = Convert.ToInt32(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);      //v7.0 FPD対応 by 間々田 2003/09/25

			//計算
			Dpi = 10 / B1;
			Dpj = Dpi * kv;			                //added by 山本　2002-6-18
			b0_dash = kv * a;			            //added by 山本　2002-6-18
			e_dash = System.Math.Atan(b0_dash);		//added by 山本　2002-6-18
			dpm = Dpj * System.Math.Cos(e_dash);	//added by 山本　2002-6-18
		}


        //'*******************************************************************************
        //'機　　能： 最大スライス厚再計算
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： iVSZ            [I/ ] Integer   透視画像ｻｲｽﾞ(縦)
        //'           iHSZ            [I/ ] Integer   透視画像ｻｲｽﾞ(横)
        //'           fAAA            [I/ ] Single    傾き
        //'           fBBB            [I/ ] Single    切片
        //'           fFID            [I/ ] Single    FID(FID+FIDｵﾌｾｯﾄ)
        //'           fFCD            [I/ ] Single    FCD(FCD+FCDｵﾌｾｯﾄ)
        //'           fMDT            [I/ ] Single    検出器ﾋﾟｯﾁ(mm/画素)
        //'           iVMG            [I/ ] Long      縦倍率
        //'           fSLP            [I/ ] Single    ｽﾗｲｽﾋﾟｯﾁ(mm)
        //'           iMAI            [I/ ] Integer   同時ｽｷｬﾝ枚数（1:1ｽﾗｲｽ,3:3ｽﾗｲｽ,5:5ｽﾗｲｽ）
        //'           msgOn           [I/ ] Boolean   異常メッセージ出力 True:する
        //'戻 り 値：                 [ /O] Single    計算結果   0:異常
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V2.0   00/02/08  (SI1)鈴山      新規作成（計算式を１個所に集中）
        //'           V3.0   00/08/18  (SI1)鈴山      小数点以下３桁までを有効とする
        //'           v10.0  05/02/04  (SI4)間々田    パラメータを引数で取り込むようにした
        //'                                           関数名も変更
        //'                                               Cal_MaxSliceWid_Ex -> GetSliceWidMax
        //'*******************************************************************************
        //Public Function GetSliceWidMax(ByVal iVSZ As Integer, _
        //'                               ByVal iHSZ As Integer, _
        //'                               ByVal fAAA As Single, _
        //'                               ByVal fBBB As Single, _
        //'                               ByVal fFID As Single, _
        //'                               ByVal fFCD As Single, _
        //'                               ByVal fMDT As Single, _
        //'                               ByVal iVMG As Long, _
        //'                               ByVal fSLP As Single, _
        //'                               ByVal iMAI As Integer, _
        //'                               Optional ByVal msgOn As Boolean = False) As Single
        //
        //    Dim fRET As Single  '計算結果
        //
        //    '戻り値初期化
        //    GetSliceWidMax = 0
        //
        //    'パラメータチェック
        //    If (fFID <= fFCD) Or (fFID <= 0) Or (fFCD <= 0) Then
        //        If msgOn Then
        //            'メッセージ表示：
        //            '   最大スライス厚計算に失敗しました。
        //            '   'FID' または 'FCD' の値が不正です。
        //            MsgBox LoadResString(9503), vbExclamation
        //        End If
        //        Exit Function
        //    End If
        //
        //    '計算
        //    fRET = (iVSZ - (2 * Abs(fBBB)) - (iHSZ * Abs(fAAA)) - 10) * (fFCD / fFID) * fMDT * iVMG - (2 * iMAI * fSLP)
        //
        //    If fRET < 0 Then fRET = (fFCD / fFID) * fMDT * iVMG
        //
        //    GetSliceWidMax = Val(Format$(fRET, "0.000"))    'V3.0 append by 鈴山
        //
        //End Function
		public static float GetSWMax(short iVSZ, short iHSZ, float fAAA, float fBBB, float SWMin, float fSLP, short iMAI)
		{
			float Result = 0;			//計算結果

			//計算
			Result = (iVSZ - 2 * System.Math.Abs(fBBB) - iHSZ * System.Math.Abs(fAAA) - 10) * SWMin - (2 * iMAI * fSLP);

			return (float)modLibrary.MaxVal(Result, SWMin);
		}


        //*******************************************************************************
        //'機　　能： 最小スライス厚再計算
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： fFID            [I/ ] Single    FID(FID+FIDｵﾌｾｯﾄ)
        //'           fFCD            [I/ ] Single    FCD(FCD+FCDｵﾌｾｯﾄ)
        //'           fMDT            [I/ ] Single    検出器ﾋﾟｯﾁ(mm/画素)
        //'           iVMG            [I/ ] Long      縦倍率
        //'           msgOn           [I/ ] Boolean   異常メッセージ出力 True:する
        //'戻 り 値：                 [ /O] Single    計算結果   0:異常
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V2.0   00/02/08  (SI1)鈴山      新規作成（計算式を１個所に集中）
        //'           V3.0   00/08/18  (SI1)鈴山      小数点以下３桁までを有効とする
        //'           v10.0  05/02/04  (SI4)間々田    パラメータを引数で取り込むようにした
        //'                                           関数名も変更
        //'                                               Cal_MinSliceWid_Ex -> GetSliceWidMin
        //'*******************************************************************************
        //Public Function GetSliceWidMin(ByVal fFID As Single, _
        //'                               ByVal fFCD As Single, _
        //'                               ByVal fMDT As Single, _
        //'                               ByVal iVMG As Long, _
        //'                               Optional ByVal msgOn As Boolean = False) As Single
        //
        //    '戻り値初期化
        //    GetSliceWidMin = 0
        //
        //    'パラメータチェック
        //    If (fFID <= fFCD) Or (fFID <= 0) Or (fFCD <= 0) Then
        //        If msgOn Then
        //            'メッセージ表示：
        //            '   最小スライス厚計算に失敗しました。
        //            '   'FID' または 'FCD' の値が不正です。
        //            MsgBox LoadResString(9499), vbExclamation
        //        End If
        //        Exit Function
        //    End If
        //
        //    '計算
        //    GetSliceWidMin = Val(Format$(MaxVal((fFCD / fFID) * fMDT * iVMG, 0), "0.000"))
        //
        //End Function

        //********************************************************************************
        //機    能  ：  IIの幾何歪みを求めるためにフィッティング計算を行う関数
        //              変数名           [I/O] 型        内容
        //引    数  ：  X()              [I/ ] Double    水平線と垂直線の交点Ｘ座標
        //              Y()              [I/ ] Double    水平線と垂直線の交点Ｙ座標
        //              ti()             [ /O] Double    横軸－実ワイヤ距離 ti(cm)
        //              xi()             [ /O] Double    縦軸－画像から求めたワイヤ距離 xi(画素)
        //              A0               [ /O] Double    フィッティング係数 A0                   added V2.0
        //              A1               [ /O] Double    フィッティング係数 A1                   added V2.0
        //              A2               [ /O] Double    フィッティング係数 A2                   added V2.0
        //              A3               [ /O] Double    フィッティング係数 A3                   added V2.0
        //              A4               [ /O] Double    フィッティング係数 A4                   added V2.0
        //              A5               [ /O] Double    フィッティング係数 A5                   added V2.0
        //              posi_a           [I/ ] Double    水平線の傾き                            added V2.0
        //              posi_b           [I/ ] Double    水平線の切片                            added V2.0
        //              wp               [I/ ] Double    垂直線ワイヤピッチ (cm)                 added V2.0
        //              Cross_Count      [I/ ] Long      垂直線ワイヤの本数
        //              Matrix           [I/ ] Integer   フィッティング用行列のサイズ            added V2.0
        //戻 り 値  ：                   [ /O] Boolean   True:正常   false:異常
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  99/XX/XX  ??????????????  新規作成
        //              V2.0   00/02/08  (SI1)鈴山       frmVerticalから移し、Public関数化
        //                                               計算に直接関わる値をすべて引数化
        //              V9.7   04/11/25  (SI4)間々田     Private化
        //********************************************************************************
        private static bool Caliculate_Fitting(ref double[] x, ref double[] y, ref double[] ti, ref double[] xi,
                                                ref double a0,
                                                ref double A1,
                                                ref double a2,
                                                ref double a3,
                                                ref double a4,
                                                ref double a5,
                                                double posi_a,
                                                double posi_b,
                                                double wp,
                                                int Cross_Count,
                                                int Matrix)
        {
            bool functionReturnValue = false;

            double Sigma_ti = 0;            //ﾌｨｯﾃｨﾝｸﾞ計算用 ∑ti
            double Sigma_ti2 = 0;           //ﾌｨｯﾃｨﾝｸﾞ計算用 ∑ti2
            double Sigma_ti3 = 0;           //ﾌｨｯﾃｨﾝｸﾞ計算用 ∑ti3
            double Sigma_ti4 = 0;           //ﾌｨｯﾃｨﾝｸﾞ計算用 ∑ti4
            double Sigma_ti5 = 0;           //ﾌｨｯﾃｨﾝｸﾞ計算用 ∑ti5
            double Sigma_ti6 = 0;           //ﾌｨｯﾃｨﾝｸﾞ計算用 ∑ti6
            double Sigma_ti7 = 0;           //ﾌｨｯﾃｨﾝｸﾞ計算用 ∑ti7
            double Sigma_ti8 = 0;           //ﾌｨｯﾃｨﾝｸﾞ計算用 ∑ti8
            double Sigma_ti9 = 0;           //ﾌｨｯﾃｨﾝｸﾞ計算用 ∑ti9
            double Sigma_ti10 = 0;          //ﾌｨｯﾃｨﾝｸﾞ計算用 ∑ti10
            double Sigma_xi = 0;            //ﾌｨｯﾃｨﾝｸﾞ計算用 ∑xi
            double Sigma_xiti = 0;          //ﾌｨｯﾃｨﾝｸﾞ計算用 ∑xi*ti
            double Sigma_xiti2 = 0;         //ﾌｨｯﾃｨﾝｸﾞ計算用 ∑xi*ti2
            double Sigma_xiti3 = 0;         //ﾌｨｯﾃｨﾝｸﾞ計算用 ∑xi*ti3
            double Sigma_xiti4 = 0;         //ﾌｨｯﾃｨﾝｸﾞ計算用 ∑xi*ti4
            double Sigma_xiti5 = 0;         //ﾌｨｯﾃｨﾝｸﾞ計算用 ∑xi*ti5

            int i = 0;
            double Half_Cross_Count = 0;    //垂直線の本数の１／２本-0.5(実数）
            double xc = 0;                  //スキャン位置線上での中心座標
            double yc = 0;                  //スキャン位置線上での中心座標

            double[] t = new double[36];        //行列用配列
            double[] T0 = new double[36];       //行列用配列
            double[] T1 = new double[36];       //行列用配列
            double[] T2 = new double[36];       //行列用配列
            double[] T3 = new double[36];       //行列用配列
            double[] T4 = new double[36];       //行列用配列
            double[] T5 = new double[36];       //行列用配列

            //戻り値初期化
            functionReturnValue = false;

            //エラー時の扱い
            try
            {
                //∑xi、∑ti2、∑ti3、∑xitiなどを求める
                //交点座標を横軸-実ワイヤ距離 ti(mm)、縦軸-画像から求めたワイヤ距離 xi(pixel)に変換
                //          xi(pixel)
                //          I
                //          I
                //          I
                //          I0
                //─────┼─────   ti(mm)
                //          I
                //          I
                //          I
                //          I

                //2014/11/13hata キャストの修正
                //Half_Cross_Count = Cross_Count / 2 - 0.5;   //垂直線の本数の１／２本 - 0.5（実数）
                Half_Cross_Count = Cross_Count / 2D - 0.5;   //垂直線の本数の１／２本 - 0.5（実数）

                //2014/11/13hata キャストの修正
                //xc = (int)(CTSettings.detectorParam.h_size / 2);
                //yc = posi_a * xc + posi_b + (int)(CTSettings.detectorParam.v_size / 2) - posi_a * (int)(CTSettings.detectorParam.h_size / 2);
                xc = Convert.ToInt32(Math.Floor(CTSettings.detectorParam.h_size / 2D));
                yc = posi_a * xc + posi_b +  Convert.ToInt32(Math.Floor(CTSettings.detectorParam.v_size / 2D)) - posi_a *  Convert.ToInt32(Math.Floor(CTSettings.detectorParam.h_size / 2D));

                for (i = 0; i <= Cross_Count - 1; i++)
                {
                    //垂直線の本数の(1/2番目-0.5)をti軸の原点とする。∑ti3などの数値が大きくなりすぎて精度が落ちるため
                    ti[i] = (i - Half_Cross_Count) * wp;
                    //xi(i) = Sqr((X(i) - Int(H_SIZE / 2)) ^ 2 + (Y(i) - Int(V_SIZE / 2)) ^ 2)  commented by 山本 99-5-12
                    xi[i] = Math.Sqrt(Math.Pow((x[i] - xc), 2) + Math.Pow((y[i] - yc), 2));     //added by 山本 99-5-12

                    xi[i] = xi[i] * Math.Cos(System.Math.Atan(posi_a));     //水平線の傾きによる距離を補正する

                    //2014/11/13hata キャストの修正
                    //if ((i < Half_Cross_Count) || (x[i] < (int)(CTSettings.detectorParam.h_size / 2)))
                    if ((i < Half_Cross_Count) || (x[i] < Convert.ToInt32(Math.Floor(CTSettings.detectorParam.h_size / 2D))))
                    {
                        xi[i] = -xi[i];
                    }
                }

                for (i = 0; i <= Cross_Count - 1; i++)
                {
                    Sigma_ti = Sigma_ti + ti[i];
                    Sigma_ti2 = Sigma_ti2 + Math.Pow((ti[i]), 2);
                    Sigma_ti3 = Sigma_ti3 + Math.Pow((ti[i]), 3);
                    Sigma_ti4 = Sigma_ti4 + Math.Pow((ti[i]), 4);
                    Sigma_ti5 = Sigma_ti5 + Math.Pow((ti[i]), 5);
                    Sigma_ti6 = Sigma_ti6 + Math.Pow((ti[i]), 6);
                    Sigma_ti7 = Sigma_ti7 + Math.Pow((ti[i]), 7);
                    Sigma_ti8 = Sigma_ti8 + Math.Pow((ti[i]), 8);
                    Sigma_ti9 = Sigma_ti9 + Math.Pow((ti[i]), 9);
                    Sigma_ti10 = Sigma_ti10 + Math.Pow((ti[i]), 10);
                    Sigma_xi = Sigma_xi + xi[i];
                    Sigma_xiti = Sigma_xiti + xi[i] * ti[i];
                    Sigma_xiti2 = Sigma_xiti2 + xi[i] * (Math.Pow((ti[i]), 2));
                    Sigma_xiti3 = Sigma_xiti3 + xi[i] * (Math.Pow((ti[i]), 3));
                    Sigma_xiti4 = Sigma_xiti4 + xi[i] * (Math.Pow((ti[i]), 4));
                    Sigma_xiti5 = Sigma_xiti5 + xi[i] * (Math.Pow((ti[i]), 5));
                }

                t[0] = Cross_Count;     // 行列 T =
                t[1] = Sigma_ti;        // ┌ T(0)  T(1)  T(2)  T(3)  T(4)  T(5)  ┐
                t[2] = Sigma_ti2;       // │ T(6)  T(7)  T(8)  T(9)  T(10) T(11) │
                t[3] = Sigma_ti3;       // │ T(12) T(13) T(14) T(15) T(16) T(17) │
                t[4] = Sigma_ti4;       // │ T(18) T(19) T(20) T(21) T(22) T(23) │
                t[5] = Sigma_ti5;       // │ T(24) T(25) T(26) T(27) T(28) T(29) │
                t[6] = Sigma_ti;        // └ T(30) T(31) T(32) T(33) T(34) T(35) ┘
                t[7] = Sigma_ti2;
                t[8] = Sigma_ti3;
                t[9] = Sigma_ti4;
                t[10] = Sigma_ti5;
                t[11] = Sigma_ti6;
                t[12] = Sigma_ti2;
                t[13] = Sigma_ti3;
                t[14] = Sigma_ti4;
                t[15] = Sigma_ti5;
                t[16] = Sigma_ti6;
                t[17] = Sigma_ti7;
                t[18] = Sigma_ti3;
                t[19] = Sigma_ti4;
                t[20] = Sigma_ti5;
                t[21] = Sigma_ti6;
                t[22] = Sigma_ti7;
                t[23] = Sigma_ti8;
                t[24] = Sigma_ti4;
                t[25] = Sigma_ti5;
                t[26] = Sigma_ti6;
                t[27] = Sigma_ti7;
                t[28] = Sigma_ti8;
                t[29] = Sigma_ti9;
                t[30] = Sigma_ti5;
                t[31] = Sigma_ti6;
                t[32] = Sigma_ti7;
                t[33] = Sigma_ti8;
                t[34] = Sigma_ti9;
                t[35] = Sigma_ti10;

                T0[0] = Sigma_xi;
                T0[1] = Sigma_ti;
                T0[2] = Sigma_ti2;
                T0[3] = Sigma_ti3;
                T0[4] = Sigma_ti4;
                T0[5] = Sigma_ti5;
                T0[6] = Sigma_xiti;
                T0[7] = Sigma_ti2;
                T0[8] = Sigma_ti3;
                T0[9] = Sigma_ti4;
                T0[10] = Sigma_ti5;
                T0[11] = Sigma_ti6;
                T0[12] = Sigma_xiti2;
                T0[13] = Sigma_ti3;
                T0[14] = Sigma_ti4;
                T0[15] = Sigma_ti5;
                T0[16] = Sigma_ti6;
                T0[17] = Sigma_ti7;
                T0[18] = Sigma_xiti3;
                T0[19] = Sigma_ti4;
                T0[20] = Sigma_ti5;
                T0[21] = Sigma_ti6;
                T0[22] = Sigma_ti7;
                T0[23] = Sigma_ti8;
                T0[24] = Sigma_xiti4;
                T0[25] = Sigma_ti5;
                T0[26] = Sigma_ti6;
                T0[27] = Sigma_ti7;
                T0[28] = Sigma_ti8;
                T0[29] = Sigma_ti9;
                T0[30] = Sigma_xiti5;
                T0[31] = Sigma_ti6;
                T0[32] = Sigma_ti7;
                T0[33] = Sigma_ti8;
                T0[34] = Sigma_ti9;
                T0[35] = Sigma_ti10;

                T1[0] = Cross_Count;
                T1[1] = Sigma_xi;
                T1[2] = Sigma_ti2;
                T1[3] = Sigma_ti3;
                T1[4] = Sigma_ti4;
                T1[5] = Sigma_ti5;
                T1[6] = Sigma_ti;
                T1[7] = Sigma_xiti;
                T1[8] = Sigma_ti3;
                T1[9] = Sigma_ti4;
                T1[10] = Sigma_ti5;
                T1[11] = Sigma_ti6;
                T1[12] = Sigma_ti2;
                T1[13] = Sigma_xiti2;
                T1[14] = Sigma_ti4;
                T1[15] = Sigma_ti5;
                T1[16] = Sigma_ti6;
                T1[17] = Sigma_ti7;
                T1[18] = Sigma_ti3;
                T1[19] = Sigma_xiti3;
                T1[20] = Sigma_ti5;
                T1[21] = Sigma_ti6;
                T1[22] = Sigma_ti7;
                T1[23] = Sigma_ti8;
                T1[24] = Sigma_ti4;
                T1[25] = Sigma_xiti4;
                T1[26] = Sigma_ti6;
                T1[27] = Sigma_ti7;
                T1[28] = Sigma_ti8;
                T1[29] = Sigma_ti9;
                T1[30] = Sigma_ti5;
                T1[31] = Sigma_xiti5;
                T1[32] = Sigma_ti7;
                T1[33] = Sigma_ti8;
                T1[34] = Sigma_ti9;
                T1[35] = Sigma_ti10;

                T2[0] = Cross_Count;
                T2[1] = Sigma_ti;
                T2[2] = Sigma_xi;
                T2[3] = Sigma_ti3;
                T2[4] = Sigma_ti4;
                T2[5] = Sigma_ti5;
                T2[6] = Sigma_ti;
                T2[7] = Sigma_ti2;
                T2[8] = Sigma_xiti;
                T2[9] = Sigma_ti4;
                T2[10] = Sigma_ti5;
                T2[11] = Sigma_ti6;
                T2[12] = Sigma_ti2;
                T2[13] = Sigma_ti3;
                T2[14] = Sigma_xiti2;
                T2[15] = Sigma_ti5;
                T2[16] = Sigma_ti6;
                T2[17] = Sigma_ti7;
                T2[18] = Sigma_ti3;
                T2[19] = Sigma_ti4;
                T2[20] = Sigma_xiti3;
                T2[21] = Sigma_ti6;
                T2[22] = Sigma_ti7;
                T2[23] = Sigma_ti8;
                T2[24] = Sigma_ti4;
                T2[25] = Sigma_ti5;
                T2[26] = Sigma_xiti4;
                T2[27] = Sigma_ti7;
                T2[28] = Sigma_ti8;
                T2[29] = Sigma_ti9;
                T2[30] = Sigma_ti5;
                T2[31] = Sigma_ti6;
                T2[32] = Sigma_xiti5;
                T2[33] = Sigma_ti8;
                T2[34] = Sigma_ti9;
                T2[35] = Sigma_ti10;

                T3[0] = Cross_Count;
                T3[1] = Sigma_ti;
                T3[2] = Sigma_ti2;
                T3[3] = Sigma_xi;
                T3[4] = Sigma_ti4;
                T3[5] = Sigma_ti5;
                T3[6] = Sigma_ti;
                T3[7] = Sigma_ti2;
                T3[8] = Sigma_ti3;
                T3[9] = Sigma_xiti;
                T3[10] = Sigma_ti5;
                T3[11] = Sigma_ti6;
                T3[12] = Sigma_ti2;
                T3[13] = Sigma_ti3;
                T3[14] = Sigma_ti4;
                T3[15] = Sigma_xiti2;
                T3[16] = Sigma_ti6;
                T3[17] = Sigma_ti7;
                T3[18] = Sigma_ti3;
                T3[19] = Sigma_ti4;
                T3[20] = Sigma_ti5;
                T3[21] = Sigma_xiti3;
                T3[22] = Sigma_ti7;
                T3[23] = Sigma_ti8;
                T3[24] = Sigma_ti4;
                T3[25] = Sigma_ti5;
                T3[26] = Sigma_ti6;
                T3[27] = Sigma_xiti4;
                T3[28] = Sigma_ti8;
                T3[29] = Sigma_ti9;
                T3[30] = Sigma_ti5;
                T3[31] = Sigma_ti6;
                T3[32] = Sigma_ti7;
                T3[33] = Sigma_xiti5;
                T3[34] = Sigma_ti9;
                T3[35] = Sigma_ti10;

                T4[0] = Cross_Count;
                T4[1] = Sigma_ti;
                T4[2] = Sigma_ti2;
                T4[3] = Sigma_ti3;
                T4[4] = Sigma_xi;
                T4[5] = Sigma_ti5;
                T4[6] = Sigma_ti;
                T4[7] = Sigma_ti2;
                T4[8] = Sigma_ti3;
                T4[9] = Sigma_ti4;
                T4[10] = Sigma_xiti;
                T4[11] = Sigma_ti6;
                T4[12] = Sigma_ti2;
                T4[13] = Sigma_ti3;
                T4[14] = Sigma_ti4;
                T4[15] = Sigma_ti5;
                T4[16] = Sigma_xiti2;
                T4[17] = Sigma_ti7;
                T4[18] = Sigma_ti3;
                T4[19] = Sigma_ti4;
                T4[20] = Sigma_ti5;
                T4[21] = Sigma_ti6;
                T4[22] = Sigma_xiti3;
                T4[23] = Sigma_ti8;
                T4[24] = Sigma_ti4;
                T4[25] = Sigma_ti5;
                T4[26] = Sigma_ti6;
                T4[27] = Sigma_ti7;
                T4[28] = Sigma_xiti4;
                T4[29] = Sigma_ti9;
                T4[30] = Sigma_ti5;
                T4[31] = Sigma_ti6;
                T4[32] = Sigma_ti7;
                T4[33] = Sigma_ti8;
                T4[34] = Sigma_xiti5;
                T4[35] = Sigma_ti10;

                T5[0] = Cross_Count;
                T5[1] = Sigma_ti;
                T5[2] = Sigma_ti2;
                T5[3] = Sigma_ti3;
                T5[4] = Sigma_ti4;
                T5[5] = Sigma_xi;
                T5[6] = Sigma_ti;
                T5[7] = Sigma_ti2;
                T5[8] = Sigma_ti3;
                T5[9] = Sigma_ti4;
                T5[10] = Sigma_ti5;
                T5[11] = Sigma_xiti;
                T5[12] = Sigma_ti2;
                T5[13] = Sigma_ti3;
                T5[14] = Sigma_ti4;
                T5[15] = Sigma_ti5;
                T5[16] = Sigma_ti6;
                T5[17] = Sigma_xiti2;
                T5[18] = Sigma_ti3;
                T5[19] = Sigma_ti4;
                T5[20] = Sigma_ti5;
                T5[21] = Sigma_ti6;
                T5[22] = Sigma_ti7;
                T5[23] = Sigma_xiti3;
                T5[24] = Sigma_ti4;
                T5[25] = Sigma_ti5;
                T5[26] = Sigma_ti6;
                T5[27] = Sigma_ti7;
                T5[28] = Sigma_ti8;
                T5[29] = Sigma_xiti4;
                T5[30] = Sigma_ti5;
                T5[31] = Sigma_ti6;
                T5[32] = Sigma_ti7;
                T5[33] = Sigma_ti8;
                T5[34] = Sigma_ti9;
                T5[35] = Sigma_xiti5;

                //テスト用
                //T(0, 0) = 0       ' 行列 T =
                //T(0, 1) = 1       ' ┌ T(0,0) T(0,1) T(0,2) T(0,3) T(0,4) T(0,5) ┐
                //T(0, 2) = 2       ' │ T(1,0) T(1,1) T(1,2) T(1,3) T(1,4) T(1,5) │
                //T(0, 3) = 3       ' │ T(2,0) T(2,1) T(2,2) T(2,3) T(2,4) T(2,5) │
                //T(0, 4) = 4       ' │ T(3,0) T(3,1) T(3,2) T(3,3) T(3,4) T(3,5) │
                //T(0, 5) = 5       ' │ T(4,0) T(4,1) T(4,2) T(4,3) T(4,4) T(4,5) │
                //T(1, 0) = 10      ' └ T(5,0) T(5,1) T(5,2) T(5,3) T(5,4) T(5,5) ┘
                //T(1, 1) = 11
                //T(1, 2) = 12
                //T(1, 3) = 13
                //T(1, 4) = 14
                //T(1, 5) = 15
                //T(2, 0) = 20
                //T(2, 1) = 21
                //T(2, 2) = 22
                //T(2, 3) = 23
                //T(2, 4) = 24
                //T(2, 5) = 25
                //T(3, 0) = 30
                //T(3, 1) = 31
                //T(3, 2) = 32
                //T(3, 3) = 33
                //T(3, 4) = 34
                //T(3, 5) = 35
                //T(4, 0) = 40
                //T(4, 1) = 41
                //T(4, 2) = 42
                //T(4, 3) = 43
                //T(4, 4) = 44
                //T(4, 5) = 45
                //T(5, 0) = 50
                //T(5, 1) = 51
                //T(5, 2) = 52
                //T(5, 3) = 53
                //T(5, 4) = 54
                //T(5, 5) = 55

                switch (Cross_Count)    //垂直線の本数でフィッティングの次数を変える
                {
                    case 0:
                    case 1:
                        return functionReturnValue;

                    case 2:
                        //A0 = Determinant2(T0, Matrix) / Determinant2(T, Matrix)   'A0=（行列式T0)/(行列式T) ///// VB用関数
                        //A1 = Determinant2(T1, Matrix) / Determinant2(T, Matrix)   'A1=（行列式T1)/(行列式T)
                        a0 = Determinant2C(ref T0[0], (short)Matrix) / Determinant2C(ref t[0], (short)Matrix);    //A0=（行列式T0)/(行列式T)///// VC用関数                    
                        A1 = Determinant2C(ref T1[0], (short)Matrix) / Determinant2C(ref t[0], (short)Matrix);    //A1=（行列式T1)/(行列式T)                    
                        a2 = 0;         //A2=（行列式T2)/(行列式T)                    
                        a3 = 0;         //A3=（行列式T3)/(行列式T)                    
                        a4 = 0;         //A4=（行列式T4)/(行列式T)                    
                        a5 = 0;         //A5=（行列式T5)/(行列式T)                    
                        break;
                    case 3:
                        //A0 = Determinant3(T0, Matrix) / Determinant3(T, Matrix)   'A0=（行列式T0)/(行列式T)
                        //A1 = Determinant3(T1, Matrix) / Determinant3(T, Matrix)   'A1=（行列式T1)/(行列式T)
                        //A2 = Determinant3(T2, Matrix) / Determinant3(T, Matrix)   'A2=（行列式T2)/(行列式T)
                        a0 = Determinant3C(ref T0[0], (short)Matrix) / Determinant3C(ref t[0], (short)Matrix);      //A0=（行列式T0)/(行列式T)
                        A1 = Determinant3C(ref T1[0], (short)Matrix) / Determinant3C(ref t[0], (short)Matrix);      //A1=（行列式T1)/(行列式T)
                        a2 = Determinant3C(ref T2[0], (short)Matrix) / Determinant3C(ref t[0], (short)Matrix);      //A2=（行列式T2)/(行列式T)
                        a3 = 0;         //A3=（行列式T3)/(行列式T)
                        a4 = 0;         //A4=（行列式T4)/(行列式T)
                        a5 = 0;         //A5=（行列式T5)/(行列式T)
                        break;
                    case 4:
                        //水平線と垂直線の交点が４点の場合は３次までのフィッティングが限界
                        //A0 = Determinant4(T0, Matrix) / Determinant4(T, Matrix) 'A0=（行列式T0)/(行列式T)
                        //A1 = Determinant4(T1, Matrix) / Determinant4(T, Matrix) 'A1=（行列式T1)/(行列式T)
                        //A2 = Determinant4(T2, Matrix) / Determinant4(T, Matrix) 'A2=（行列式T2)/(行列式T)
                        //A3 = Determinant4(T3, Matrix) / Determinant4(T, Matrix) 'A3=（行列式T3)/(行列式T)

                        //4本の場合も２次のフィッティングとする 99-9-27 by 山本
                        //A0 = Determinant4C(T0(0), Matrix) / Determinant4C(T(0), Matrix) 'A0=（行列式T0)/(行列式T)
                        //A1 = Determinant4C(T1(0), Matrix) / Determinant4C(T(0), Matrix) 'A1=（行列式T1)/(行列式T)
                        //A2 = Determinant4C(T2(0), Matrix) / Determinant4C(T(0), Matrix) 'A2=（行列式T2)/(行列式T)
                        //A3 = Determinant4C(T3(0), Matrix) / Determinant4C(T(0), Matrix) 'A3=（行列式T3)/(行列式T)
                        //A4 = 0     'A4=（行列式T4)/(行列式T)
                        //A5 = 0     'A5=（行列式T5)/(行列式T)
                        a0 = Determinant3C(ref T0[0], (short)Matrix) / Determinant3C(ref t[0], (short)Matrix);      //A0=（行列式T0)/(行列式T)
                        A1 = Determinant3C(ref T1[0], (short)Matrix) / Determinant3C(ref t[0], (short)Matrix);      //A1=（行列式T1)/(行列式T)
                        a2 = Determinant3C(ref T2[0], (short)Matrix) / Determinant3C(ref t[0], (short)Matrix);      //A2=（行列式T2)/(行列式T)
                        a3 = 0;         //A3=（行列式T3)/(行列式T)
                        a4 = 0;         //A4=（行列式T4)/(行列式T)
                        a5 = 0;         //A5=（行列式T5)/(行列式T)
                        break;
                    case 5:
                        //A0 = Determinant5(T0, Matrix) / Determinant5(T, Matrix)     'A0=（行列式T0)/(行列式T)
                        //A1 = Determinant5(T1, Matrix) / Determinant5(T, Matrix)     'A1=（行列式T1)/(行列式T)
                        //A2 = Determinant5(T2, Matrix) / Determinant5(T, Matrix)     'A2=（行列式T2)/(行列式T)
                        //A3 = Determinant5(T3, Matrix) / Determinant5(T, Matrix)     'A3=（行列式T3)/(行列式T)
                        //A4 = Determinant5(T4, Matrix) / Determinant5(T, Matrix)     'A4=（行列式T4)/(行列式T)

                        //5本の場合も２次のフィッティングとする 99-9-27 by 山本
                        //A0 = Determinant5C(T0(0), Matrix) / Determinant5C(T(0), Matrix)     'A0=（行列式T0)/(行列式T)
                        //A1 = Determinant5C(T1(0), Matrix) / Determinant5C(T(0), Matrix)     'A1=（行列式T1)/(行列式T)
                        //A2 = Determinant5C(T2(0), Matrix) / Determinant5C(T(0), Matrix)     'A2=（行列式T2)/(行列式T)
                        //A3 = Determinant5C(T3(0), Matrix) / Determinant5C(T(0), Matrix)     'A3=（行列式T3)/(行列式T)
                        //A4 = Determinant5C(T4(0), Matrix) / Determinant5C(T(0), Matrix)     'A4=（行列式T4)/(行列式T)
                        //A5 = 0     'A5=（行列式T5)/(行列式T)
                        a0 = Determinant3C(ref T0[0], (short)Matrix) / Determinant3C(ref t[0], (short)Matrix);      //A0=（行列式T0)/(行列式T)
                        A1 = Determinant3C(ref T1[0], (short)Matrix) / Determinant3C(ref t[0], (short)Matrix);      //A1=（行列式T1)/(行列式T)
                        a2 = Determinant3C(ref T2[0], (short)Matrix) / Determinant3C(ref t[0], (short)Matrix);      //A2=（行列式T2)/(行列式T)
                        a3 = 0;             //A3=（行列式T3)/(行列式T)
                        a4 = 0;             //A4=（行列式T4)/(行列式T)
                        a5 = 0;             //A5=（行列式T5)/(行列式T)
                        break;

                    default:

                        //8本以下の場合は3次のフィッティングとする 99-9-27 by 山本
                        if (Cross_Count < 9)
                        {

                            a0 = Determinant4C(ref T0[0], (short)Matrix) / Determinant4C(ref t[0], (short)Matrix);      //A0=（行列式T0)/(行列式T)
                            A1 = Determinant4C(ref T1[0], (short)Matrix) / Determinant4C(ref t[0], (short)Matrix);      //A1=（行列式T1)/(行列式T)
                            a2 = Determinant4C(ref T2[0], (short)Matrix) / Determinant4C(ref t[0], (short)Matrix);      //A2=（行列式T2)/(行列式T)
                            a3 = Determinant4C(ref T3[0], (short)Matrix) / Determinant4C(ref t[0], (short)Matrix);      //A3=（行列式T3)/(行列式T)
                            a4 = 0;         //A4=（行列式T4)/(行列式T)
                            a5 = 0;         //A5=（行列式T5)/(行列式T)

                        }
                        else
                        {
                            //５次のフィッティングは９本以上とする 99-9-27 by 山本
                            a0 = Determinant6(T0, Matrix) / Determinant6(t, Matrix);          //A0=（行列式T0)/(行列式T)
                            A1 = Determinant6(T1, Matrix) / Determinant6(t, Matrix);          //A1=（行列式T1)/(行列式T)
                            a2 = Determinant6(T2, Matrix) / Determinant6(t, Matrix);          //A2=（行列式T2)/(行列式T)
                            a3 = Determinant6(T3, Matrix) / Determinant6(t, Matrix);          //A3=（行列式T3)/(行列式T)
                            a4 = Determinant6(T4, Matrix) / Determinant6(t, Matrix);          //A4=（行列式T4)/(行列式T)
                            a5 = Determinant6(T5, Matrix) / Determinant6(t, Matrix);          //A5=（行列式T5)/(行列式T)

                            //A0 = Determinant6C(T0(0), Matrix) / Determinant6C(T(0), Matrix)     'A0=（行列式T0)/(行列式T)
                            //A1 = Determinant6C(T1(0), Matrix) / Determinant6C(T(0), Matrix)     'A1=（行列式T1)/(行列式T)
                            //A2 = Determinant6C(T2(0), Matrix) / Determinant6C(T(0), Matrix)     'A2=（行列式T2)/(行列式T)
                            //A3 = Determinant6C(T3(0), Matrix) / Determinant6C(T(0), Matrix)     'A3=（行列式T3)/(行列式T)
                            //A4 = Determinant6C(T4(0), Matrix) / Determinant6C(T(0), Matrix)     'A4=（行列式T4)/(行列式T)
                            //A5 = Determinant6C(T5(0), Matrix) / Determinant6C(T(0), Matrix)     'A5=（行列式T5)/(行列式T)
                        }
                        break;
                }

                //戻り値セット
                functionReturnValue = true;
            }
            catch
            {
                // Nothing
            }
            return functionReturnValue;
        }


        //********************************************************************************
        //機    能  ：  ２次の行列式を求める関数
        //              変数名           [I/O] 型        内容
        //引    数  ：  aa1()            [I/ ] Double    ２行２列の行列
        //              Mx_a             [I/ ] Integer   行列の最大行列数（＝６）
        //戻 り 値  ：                   [ /O] Double    行列式の値
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  99/XX/XX  ??????????????  新規作成
        //              V2.0   00/02/08  (SI1)鈴山       frmVerticalから移し、Public関数化
        //********************************************************************************
		private static double Determinant2(double[] aa1, int Mx_a)
		{
			// 行列 a = ┌ a(0)      a(1)    ┐
			//          └ a(Mx_a) a(Mx_a+1) ┘
            return aa1[0] * aa1[Mx_a + 1] - aa1[1] * aa1[Mx_a];
		}


        //********************************************************************************
        //機    能  ：  ３次の行列式を求める関数
        //              変数名           [I/O] 型        内容
        //引    数  ：  aa2()            [I/ ] Double    ３行３列の行列
        //              Mx_a             [I/ ] Integer   行列の最大行列数（＝６）
        //戻 り 値  ：                   [ /O] Double    行列式の値
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  99/XX/XX  ??????????????  新規作成
        //              V2.0   00/02/08  (SI1)鈴山       frmVerticalから移し、Public関数化
        //********************************************************************************
		private static double Determinant3(double[] aa2, int Mx_a)
		{
			const int Mx_b = 2;     //行列 b10, b11,b12のサイズ

			// 行列 a = ┌ a(0)      a(1)        a(2)        ┐
			//          │ a(Mx_a)   a(Mx_a  +1) a(Mx_a  +2) │
			//          └ a(Mx_a*2) a(Mx_a*2+1) a(Mx_a*2+2) ┘
			//
			double[] b10 = new double[5];		//行列用配列
			double[] b11 = new double[5];		//行列用配列
			double[] b12 = new double[5];		//行列用配列
			int i = 0;
			int j = 0;
			int K = 0;
			int N = 0;	//行カウンタ

            for (i = 0; i <= 2; i++)
            {
                for (j = 0; j <= 1; j++)
                {
                    for (K = 0; K <= 1; K++)
                    {
                        N = (j > i - 1 ? j + 1 : j);
                        switch (i)
                        {
                            case 0:
                                b10[j * Mx_b + K] = aa2[N * Mx_a + K + 1];
                                break;
                            case 1:
                                b11[j * Mx_b + K] = aa2[N * Mx_a + K + 1];
                                break;
                            case 2:
                                b12[j * Mx_b + K] = aa2[N * Mx_a + K + 1];
                                break;
                        }
                    }
                }
            }
			return aa2[0] * Determinant2(b10, Mx_b) 
                 - aa2[Mx_a] * Determinant2(b11, Mx_b) 
                 + aa2[Mx_a * 2] * Determinant2(b12, Mx_b);
		}


        //********************************************************************************
        //機    能  ：  ４次の行列式を求める関数
        //              変数名           [I/O] 型        内容
        //引    数  ：  aa3()            [I/ ] Double    ４行４列の行列
        //              Mx_a             [I/ ] Integer   行列の最大行列数（＝６）
        //戻 り 値  ：                   [ /O] Double    行列式の値
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  99/XX/XX  ??????????????  新規作成
        //              V2.0   00/02/08  (SI1)鈴山       frmVerticalから移し、Public関数化
        //********************************************************************************
		private static double Determinant4(double[] aa3, int Mx_a)
		{
			const int Mx_b = 3;	    //行列 b20, b21,b22,b23 のサイズ

			// 行列 a = ┌ a(0)      a(1)        a(2)        a(3)         ┐
			//          │ a(Mx_a)   a(Mx_a+1)   a(Mx_a+2)   a(Mx_a+3)    │
			//          │ a(Mx_a*2) a(Mx_a*2+1) a(Mx_a*2+2) a(Mx_a*2+3)  │
			//          └ a(Mx_a*3) a(Mx_a*3+1) a(Mx_a*3+2) a(Mx_a*3+3)  ┘
			//
			double[] b20 = new double[10];		//行列用配列
			double[] b21 = new double[10];		//行列用配列
			double[] b22 = new double[10];		//行列用配列
			double[] b23 = new double[10];		//行列用配列
			int i = 0;
            int j = 0;
            int K = 0;
            int N = 0;	//行カウンタ

            for (i = 0; i <= 3; i++)
            {
                for (j = 0; j <= 2; j++)
                {
                    for (K = 0; K <= 2; K++)
                    {
                        N = (j > i - 1 ? j + 1 : j);
                        switch (i)
                        {
                            case 0:
                                b20[j * Mx_b + K] = aa3[N * Mx_a + K + 1];
                                break;
                            case 1:
                                b21[j * Mx_b + K] = aa3[N * Mx_a + K + 1];
                                break;
                            case 2:
                                b22[j * Mx_b + K] = aa3[N * Mx_a + K + 1];
                                break;
                            case 3:
                                b23[j * Mx_b + K] = aa3[N * Mx_a + K + 1];
                                break;
                        }
                    }
                }
            }
			return aa3[0] * Determinant3(b20, Mx_b) 
                 - aa3[Mx_a] * Determinant3(b21, Mx_b) 
                 + aa3[Mx_a * 2] * Determinant3(b22, Mx_b) 
                 - aa3[Mx_a * 3] * Determinant3(b23, Mx_b);
		}


        //********************************************************************************
        //機    能  ：  ５次の行列式を求める関数
        //              変数名           [I/O] 型        内容
        //引    数  ：  aa4()            [I/ ] Double    ５行５列の行列
        //              Mx_a             [I/ ] Integer   行列の最大行列数（＝６）
        //戻 り 値  ：                   [ /O] Double    行列式の値
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  99/XX/XX  ??????????????  新規作成
        //              V2.0   00/02/08  (SI1)鈴山       frmVerticalから移し、Public関数化
        //********************************************************************************
        private static double Determinant5(ref double[] aa4, int Mx_a)
		{
            const int Mx_b = 4;	    //行列 b30, b31,b32,b33,b34 のサイズ

			// 行列 a = ┌ a(0)      a(1)        a(2)        a(3)        a(4)        ┐
			//          │ a(Mx_a)   a(Mx_a  +1) a(Mx_a  +2) a(Mx_a  +3) a(Mx_a  +4) │
			//          │ a(Mx_a*2) a(Mx_a*2+1) a(Mx_a*2+2) a(Mx_a*2+3) a(Mx_a*2+4) │
			//          │ a(Mx_a*3) a(Mx_a*3+1) a(Mx_a*3+2) a(Mx_a*3+3) a(Mx_a*3+4) │
			//          └ a(Mx_a*4) a(Mx_a*4+1) a(Mx_a*4+2) a(Mx_a*4+3) a(Mx_a*4+4) ┘
			//
			double[] b30 = new double[17];		//行列用配列
			double[] b31 = new double[17];		//行列用配列
			double[] b32 = new double[17];		//行列用配列
			double[] b33 = new double[17];		//行列用配列
			double[] b34 = new double[17];		//行列用配列
            int i = 0;
            int j = 0;
            int K = 0;
            int N = 0;	//行カウンタ

            for (i = 0; i <= 4; i++)
            {
                for (j = 0; j <= 3; j++)
                {
                    for (K = 0; K <= 3; K++)
                    {
                        N = (j > i - 1 ? j + 1 : j);
                        switch (i)
                        {
                            case 0:
                                b30[j * Mx_b + K] = aa4[N * Mx_a + K + 1];
                                break;
                            case 1:
                                b31[j * Mx_b + K] = aa4[N * Mx_a + K + 1];
                                break;
                            case 2:
                                b32[j * Mx_b + K] = aa4[N * Mx_a + K + 1];
                                break;
                            case 3:
                                b33[j * Mx_b + K] = aa4[N * Mx_a + K + 1];
                                break;
                            case 4:
                                b34[j * Mx_b + K] = aa4[N * Mx_a + K + 1];
                                break;
                        }
                    }
                }
            }
			return aa4[0] * Determinant4(b30, Mx_b)
                 - aa4[Mx_a] * Determinant4(b31, Mx_b)
                 + aa4[Mx_a * 2] * Determinant4(b32, Mx_b)
                 - aa4[Mx_a * 3] * Determinant4(b33, Mx_b)
                 + aa4[Mx_a * 4] * Determinant4(b34, Mx_b);
        }


        //********************************************************************************
        //機    能  ：  ６次の行列式を求める関数
        //              変数名           [I/O] 型        内容
        //引    数  ：  aa5()            [I/ ] Double    ６行６列の行列
        //              Mx_a             [I/ ] Integer   行列の最大行列数（＝６）
        //戻 り 値  ：                   [ /O] Double    行列式の値
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  99/XX/XX  ??????????????  新規作成
        //              V2.0   00/02/08  (SI1)鈴山       frmVerticalから移し、Public関数化
        //********************************************************************************
		private static double Determinant6(double[] aa5, int Mx_a)
		{
            const int Mx_b = 5;		//行列 b40, b41,b42,b43,b44,b45 のサイズ

			// 行列 a = ┌ a(0)      a(1)        a(2)        a(3)        a(4)        a(5)       ┐
			//          │ a(Mx_a)   a(Mx_a  +1) a(Mx_a  +2) a(Mx_a  +3) a(Mx_a  +4) a(Mx_a* +5)│
			//          │ a(Mx_a*2) a(Mx_a*2+1) a(Mx_a*2+2) a(Mx_a*2+3) a(Mx_a*2+4) a(Mx_a*2+5)│
			//          │ a(Mx_a*3) a(Mx_a*3+1) a(Mx_a*3+2) a(Mx_a*3+3) a(Mx_a*3+4) a(Mx_a*3+5)│
			//          │ a(Mx_a*4) a(Mx_a*4+1) a(Mx_a*4+2) a(Mx_a*4+3) a(Mx_a*4+4) a(Mx_a*4+5)│
			//          └ a(Mx_a*5) a(Mx_a*5+1) a(Mx_a*5+2) a(Mx_a*5+3) a(Mx_a*5+4) a(Mx_a*5+5)┘
			double[] b40 = new double[26];		//行列用配列
			double[] b41 = new double[26];		//行列用配列
			double[] b42 = new double[26];		//行列用配列
			double[] b43 = new double[26];		//行列用配列
			double[] b44 = new double[26];		//行列用配列
			double[] b45 = new double[26];		//行列用配列
            int i = 0;
            int j = 0;
            int K = 0;
            int N = 0;	//行カウンタ

            for (i = 0; i <= 5; i++)
            {
                for (j = 0; j <= 4; j++)
                {
                    for (K = 0; K <= 4; K++)
                    {
                        N = (j > i - 1 ? j + 1 : j);
                        switch (i)
                        {
                            case 0:
                                b40[j * Mx_b + K] = aa5[N * Mx_a + K + 1];
                                break;
                            case 1:
                                b41[j * Mx_b + K] = aa5[N * Mx_a + K + 1];
                                break;
                            case 2:
                                b42[j * Mx_b + K] = aa5[N * Mx_a + K + 1];
                                break;
                            case 3:
                                b43[j * Mx_b + K] = aa5[N * Mx_a + K + 1];
                                break;
                            case 4:
                                b44[j * Mx_b + K] = aa5[N * Mx_a + K + 1];
                                break;
                            case 5:
                                b45[j * Mx_b + K] = aa5[N * Mx_a + K + 1];
                                break;
                        }
                    }
                }
            }
			return aa5[0] * Determinant5(ref b40, Mx_b) 
                 - aa5[Mx_a] * Determinant5(ref b41, Mx_b) 
                 + aa5[Mx_a * 2] * Determinant5(ref b42, Mx_b) 
                 - aa5[Mx_a * 3] * Determinant5(ref b43, Mx_b) 
                 + aa5[Mx_a * 4] * Determinant5(ref b44, Mx_b) 
                 - aa5[Mx_a * 5] * Determinant5(ref b45, Mx_b);
		}


        //********************************************************************************
        //機    能  ：  ゲイン校正パラメータ計算
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：                   [ /O] Boolean   結果(True:正常,False:異常)
        //補    足  ：  なし
        //
        //履    歴  ：  V4.0   01/01/31  (SI1)鈴山       新規作成
        //              v7.0   03/07/29  (SI4)間々田     戻り値の変更(Long→Boolean,0→True,-1→False)
        //********************************************************************************
		public static bool Get_GainCor_Parameter_Ex()
		{
            //戻り値初期化
			bool functionReturnValue = false;

			//エラー時の扱い
            try
            {
			    //以下は、入力画像をImageProで表示させるための処理
			    if (IMGPRODBG == 1)
                {
                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    //イメージプロの新規ウィンドウ作成し，画像データ（配列）を書込む                
                    //Ipc32v5.IPOBJ.DrawWordImage(theImage: ref GAIN_IMAGE, theWidth: h_size, theHeight: v_size, IsWsCreate: true);
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    CTSettings.IPOBJ.DrawWordImage(theImage: GAIN_IMAGE, theWidth: CTSettings.detectorParam.h_size, theHeight: CTSettings.detectorParam.v_size, IsWsCreate: true);
                    
 				    if (modScanCorrect.Flg_GainShiftScan == CheckState.Checked)
                    {
					    //Ipc32v5.IPOBJ.DrawWordImage(ref GAIN_IMAGE_SFT, , , h_size, v_size, true);				//v18.00追加 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        //CTSettings.IPOBJ.DrawWordImage(theImage: GAIN_IMAGE_SFT, theWidth: CTSettings.detectorParam.h_size, theHeight: CTSettings.detectorParam.v_size, IsWsCreate: true);
                        //Rev23.20 左右シフト対応 by長野 2015/11/19
                        CTSettings.IPOBJ.DrawWordImage(theImage: GAIN_IMAGE_SFT_R, theWidth: CTSettings.detectorParam.h_size, theHeight: CTSettings.detectorParam.v_size, IsWsCreate: true);
                        CTSettings.IPOBJ.DrawWordImage(theImage: GAIN_IMAGE_SFT_L, theWidth: CTSettings.detectorParam.h_size, theHeight: CTSettings.detectorParam.v_size, IsWsCreate: true);
                    }   
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//
                }

			    //戻り値セット
			    functionReturnValue = true;
            }
            catch
            {
                // Nothing
            }
			return functionReturnValue;
		}


        //********************************************************************************
        //機    能  ：  オフセット校正パラメータ計算
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：                   [ /O] Boolean      結果(True:正常,False:異常)
        //補    足  ：  なし
        //
        //履    歴  ：  V4.0   01/01/31  (SI1)鈴山       新規作成
        //********************************************************************************
		public static bool Get_Offset_Parameter_Ex()
		{
            ////Ipc32v5.RECT tmpReg = default(Ipc32v5.RECT);
            //Winapi.RECT tmpReg = default(Winapi.RECT);
            
            int rc = 0;			//関数の戻り値(汎用)

			//戻り値初期化
			bool functionReturnValue = false;

			//エラー時の扱い
            try
            {
                if (IMGPRODBG == 1)
                {
                    
                    //以下は、入力画像をImageProで表示させるための処理
                    //Image-Pro 画像データの取得

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*
                    rc = Ipc32v5.IpAppCloseAll();                                       //☆☆開いている全ての画像ｳｨﾝﾄﾞを閉じる
                    //rc = IpWsCreate(H_SIZE, V_SIZE, 300, IMC_GRAY16)                   '☆☆新規の画像ｳｨﾝﾄﾞを開く
                    rc = Ipc32v5.IpWsCreate(h_size, v_size, 300, Ipc32v5.IMC_FLOAT);    //☆☆新規の画像ｳｨﾝﾄﾞを開く  'changed by 山本 2000-11-18
                    tmpReg.left = 0;
                    tmpReg.top = 0;
                    tmpReg.right = h_size - 1;
                    tmpReg.bottom = v_size - 1;
                    rc = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref OFFSET_IMAGE[0], Ipc32v5.CPROG);   //☆☆ユーザが作成した画像ﾃﾞｰﾀをのImage-Proの画像に書込む
                    rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);                                                 //☆☆画像ウィンドの再描画
                    rc = Ipc32v5.IpHstEqualize(Ipc32v5.EQ_BESTFIT);
                    */
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    rc = CallImageProFunction.CallPutOffsetImage(OFFSET_IMAGE, CTSettings.detectorParam.v_size, CTSettings.detectorParam.h_size);
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//
           
                }

                //戻り値セット
                functionReturnValue = true;
            }
            catch
            {
                // Nothing
            }
			return functionReturnValue;
		}


        //********************************************************************************
        //機    能  ：  回転中心校正パラメータ計算
        //              変数名           [I/O] 型        内容
        //引    数  ：  iMSL             [I/ ] Long      同時スキャン枚数(0:1ｽﾗｲｽ,1:3ｽﾗｲｽ,2:5ｽﾗｲｽ)
        //戻 り 値  ：                   [ /O] Long      結果(0:正常,1:異常)
        //補    足  ：  "■"で始まるコメント部は、なるべく設計書のフローチャートと一致する
        //              ようにしてください。
        //
        //履    歴  ：  V2.0   00/02/08  (SI1)鈴山       新規作成
        //              V4.0   01/01/29  (SI1)鈴山       画像を読み込む処理を別関数にした
        //********************************************************************************
        //Public Function Get_RotationCenter_Parameter_Ex(ByVal iMSL As Long) As Long
		public static int Get_RotationCenter_Parameter_Ex(int iMSL, int theView)        //v10.0変更 by 間々田 2005/02/04
		{	
			int rc = 0;			//関数の戻り値(汎用)

            //Ipc32v5.RECT tmpReg = default(Ipc32v5.RECT);
            //Ipc32v5.RECT roiRect = default(Ipc32v5.RECT);		//ヒストグラム用矩形
            //Winapi.RECT tmpReg = default(Winapi.RECT);
            Winapi.RECT roiRect = default(Winapi.RECT);		//ヒストグラム用矩形


            int tmpL = 0;
            int tmpL_sft = 0;       //(シフト用) 'v18.00追加 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07_追加2014/10/07hata_v19.51反映

            float[] HistData = new float[11];

			int i = 0;
			float xL0 = 0;		    //IIセンターの画素番号
            float xL0_sft = 0;      //IIセンターの画素番号(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07_追加2014/10/07hata_v19.51反映
			double Th_s = 0;	    //ファン角開始角度
            double Th_s_sft = 0;    //ファン角開始角度(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07_追加2014/10/07hata_v19.51反映
            double Th_e = 0;	    //ファン角終了角度
            double Th_e_sft = 0;    //ファン角終了角度(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07_追加2014/10/07hata_v19.51反映
            double Th_c = 0;	    //回転中心角度
            double Th_c_sft = 0;    //回転中心角度(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07_追加2014/10/07hata_v19.51反映
            int nd = 0;			    //等角度補正後の全チャンネル数(整数）
			float rNd = 0;		    //等角度補正後の全チャンネル数(実数）
            int nd_sft = 0;         //等角度補正後の全チャンネル数(整数）(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07_追加2014/10/07hata_v19.51反映
            float rNd_sft = 0;      //等角度補正後の全チャンネル数(実数）(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07_追加2014/10/07hata_v19.51反映
            int sft = 0;            //シフト量(画素)                                     'v18.00追加 byやまおか 2011/02/28 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07_追加2014/10/07hata_v19.51反映
            int sft_val = 0;        //シフト量(画素)(シフト:sft、非シフト:0)             'v18.00追加 byやまおか 2011/07/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07_追加2014/10/07hata_v19.51反映
            //int inv_pix = 0;        //無効画素                                           'v18.00追加 byやまおか 2011/02/28 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07_追加2014/10/07hata_v19.51反映

			double CPixel = 0;	    //回転中心画素

			//マルチスライス対応   added V2.0 by 鈴山
			int[] iPOS = new int[6];		        //スキャン位置の配列
			int iSCN = 0;			                //スキャン位置
			int iNNN = 0;			                //ループカウンタ
			int iSMX = 0;			                //同時スキャン枚数（1,3,5ｽﾗｲｽ）
			//float[] Min_MaxSArea = new float[4];	//複数同時スライス時に各位置で求めた最大スキャンエリアの最小値を求めるためのバッファ
            float[] Min_MaxSArea = new float[5];    //複数同時スライス時に各位置で求めた最大スキャンエリアの最小値を求めるためのバッファ 'v18.00変更 byやまおか 2011/02/28 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07_変更2014/10/07hata_v19.51反映

			//コーンビームＣＴ用     'V3.0 append by 鈴山
			float h = 0;			//透視画像横ｻｲｽﾞ
            float h_sft = 0;        //シフトを含めた透視画像横ｻｲｽﾞ 'v18.00追加 byやまおか 2011/02/28 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07_追加2014/10/07hata_v19.51反映

            float v = 0;			//透視画像縦ｻｲｽﾞ
			float bb0 = 0;			//ｽｷｬﾝ位置の傾き  'changed by 山本　2002-6-18　a->bb0
			float aa0 = 0;			//ｽｷｬﾝ位置の切片  'changed by 山本　2002-6-18　b->aa0

            float aa0_sft = 0;      //ｽｷｬﾝ位置の切片(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07_追加2014/10/07hata_v19.51反映

            float Fid = 0;			//FID+FIDｵﾌｾｯﾄ
			float FCD = 0;			//FCD+FCDｵﾌｾｯﾄ
			float ic = 0;			//

            float ic_sft = 0;       //(シフト用) 'v18.00追加 byやまおか 2011/07/09

			float jc = 0;			//
			float Dpi = 0;			//
			//    Dim e               As Single   '
			
            float r = 0;			//
            float r_sft = 0;        //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            float mm = 0;			//
            float mm_sft = 0;       //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            int Delta_Ip = 0;		//
            int Delta_Ip_sft = 0;   //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            int Ils = 0;			//
            int Ils_sft = 0;        //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            int Ile = 0;			//
            int Ile_sft = 0;        //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            float S1 = 0;			//
            float S1_sft = 0;       //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            float S2 = 0;			//
            float S2_sft = 0;       //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            float Theta1 = 0;		//
            float Theta1_sft = 0;   //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            float Theta2 = 0;		//
            float Theta2_sft = 0;   //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
			
            int scan_mode = 0;	    //ｽｷｬﾝﾓｰﾄﾞ(0:ﾌﾙ,1:ｵﾌｾｯﾄ,2:ｼﾌﾄ)(scanselは1:HALF,2:FULL,3:OFFSET,4:SHIFT)   'v18.00コメント追加 byやまおか 2011/07/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

            int kv = 0;			    //added by 山本　2002-6-18
			float b0_dash = 0;		//added by 山本　2002-6-18
			float e_dash = 0;		//added by 山本　2002-6-18
			float Dpj = 0;			//added by 山本　2002-6-18

			//戻り値初期化
            int functionReturnValue = -1;

			//エラー時の扱い
            try
            {
                //初期設定
                //VIEW_N = GVal_View
                VIEW_N = theView;			//v10.0変更 by 間々田 2005/02/04
                tmpL = VIEW_N * CTSettings.detectorParam.h_size;
			    RC_IMAGE = new ushort[tmpL];		//回転中心校正画像               'v9.7追加 by 間々田 2004/12/03  'v15.0変更 -1した by 間々田 2009/06/03
                RC_IMAGE_ORG = new ushort[tmpL];	//回転中心校正画像(ﾌｧｲﾙ保存用)   'v9.7追加 by 間々田 2004/12/03  'v15.0変更 -1した by 間々田 2009/06/03
                RC_BILE = new ushort[tmpL];		//回転中心校正画像(2値化)        'v9.7追加 by 間々田 2004/12/03  'v15.0変更 -1した by 間々田 2009/06/03

			    //マルチスライス対応
			    switch (iMSL)       //同時スキャン枚数（0:1ｽﾗｲｽ,1:3ｽﾗｲｽ,2:5ｽﾗｲｽ）
                {				    
				    case 0:
					    iPOS[0] = 2;
					    iPOS[1] = 0;
					    iPOS[2] = 0;
					    iPOS[3] = 0;
					    iPOS[4] = 0;
					    iSMX = 1;
					    break;
				    case 1:
					    iPOS[0] = 1;
					    iPOS[1] = 2;
					    iPOS[2] = 3;
					    iPOS[3] = 0;
					    iPOS[4] = 0;
					    iSMX = 3;
					    break;
				    case 2:
					    iPOS[0] = 0;
					    iPOS[1] = 1;
					    iPOS[2] = 2;
					    iPOS[3] = 3;
					    iPOS[4] = 4;
					    iSMX = 5;
					    break;
				    default:
                        throw new Exception();
					    //break;
			    }

			    //最大スキャンエリアの初期設定
			    Min_MaxSArea[0] = 100000;
			    Min_MaxSArea[1] = 100000;
			    Min_MaxSArea[2] = 100000;
                Min_MaxSArea[3] = 100000;       //v18.00追加 byやまおか 2011/02/28    //追加2014/10/07hata_v19.51反映 

                //2014/11/13hata キャストの修正
                //Threshold255_R = 65535 / 2;
                Threshold255_R = Convert.ToInt32(65535 / 2F);

			    //同時スキャン枚数分のループ
			    for (iNNN = 0; iNNN <= iSMX - 1; iNNN++) 
                {
				    //複数スライス時のスキャン位置（0～4）
				    iSCN = iPOS[iNNN];

				    //スキャン位置の画像をコピー
                    switch (iNNN)
                    {
                        case 0:
                            //                Call CopyBuff(RC_IMAGE, RC_IMAGE0, tmpL)
                            RC_IMAGE0.CopyTo(RC_IMAGE, 0);      //v9.7変更 by 間々田 2004/12/03
                            break;
                        case 1:
                            //                Call CopyBuff(RC_IMAGE, RC_IMAGE1, tmpL)
                            RC_IMAGE1.CopyTo(RC_IMAGE, 0);      //v9.7変更 by 間々田 2004/12/03
                            break;
                        case 2:
                            //                Call CopyBuff(RC_IMAGE, RC_IMAGE2, tmpL)
                            RC_IMAGE2.CopyTo(RC_IMAGE, 0);      //v9.7変更 by 間々田 2004/12/03
                            break;
                        case 3:
                            //                Call CopyBuff(RC_IMAGE, RC_IMAGE3, tmpL)
                            RC_IMAGE3.CopyTo(RC_IMAGE, 0);      //v9.7変更 by 間々田 2004/12/03
                            break;
                        case 4:
                            //                Call CopyBuff(RC_IMAGE, RC_IMAGE4, tmpL)
                            RC_IMAGE4.CopyTo(RC_IMAGE, 0);      //v9.7変更 by 間々田 2004/12/03
                            break;
                    }

				    //ファイル保存用画像としてコピー   add by 中島 99-5-19 → 関数化 V2.0 by 鈴山
				    //        Call CopyBuff(RC_IMAGE_ORG, RC_IMAGE, tmpL)
                    RC_IMAGE.CopyTo(RC_IMAGE_ORG, 0);           //v9.7変更 by 間々田 2004/12/03

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*
				    //■イメージプロで新しい画像を開く
				    rc = Ipc32v5.IpAppCloseAll();				                        //開いている全ての画像ウィンドウを閉じる
                    rc = Ipc32v5.IpWsCreate((short)h_size, (short)VIEW_N, 300, Ipc32v5.IMC_GRAY16);   //空の画像ウィンドウを生成（Gray Scale 16形式）
				    tmpReg.left = 0;
				    tmpReg.top = 0;
				    tmpReg.right = h_size - 1;
				    tmpReg.bottom = VIEW_N - 1;

				    //■イメージプロの新規画像に回転中心校正画像を書込む
				    rc = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref RC_IMAGE[0], Ipc32v5.CPROG);   //ユーザが作成した画像データをImage-Proに書込む
                    if (rc != 0) throw new Exception(); ;
				    rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);				    //画像ウィンドの再描画
                    if (rc != 0) throw new Exception(); ;

				    //■ヒストグラム処理を行なうＲＯＩを設定する
				    FRMWIDTH = (CTSettings.detectorParam.Use_FpdAllpix ? 0 : 3);			    //v17.22追加 byやまおか 2010/10/19

				    //ヒストグラム処理
				    //roiRect.Left = FRMWIDTH
				    //roiRect.Left = FRMWIDTH + IIf(DetType = DetTypePke, 40, 0)   'v17.00 パーキンエルマーFPDは余裕を大きく取る 2010-02-09　山本
				    //roiRect.Left = IIf(DetType = DetTypePke, (h_size Mod 100) / 2, FRMWIDTH)    'v17.20変更 byやまおか 2010/09/17
				    roiRect.left = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) 
                                && (!CTSettings.detectorParam.Use_FpdAllpix) ? (h_size % 100) / 2 : FRMWIDTH);    //v17.22変更 byやまおか 2010/10/19
				    //roiRect.Top = FRMWIDTH
				    roiRect.top = 0;		    //VIEW方向を狭めるのはおかしい   'v17.50変更 byやまおか 2011/03/02
				    //roiRect.Right = h_size - FRMWIDTH
				    //roiRect.Right = h_size - FRMWIDTH - IIf(DetType = DetTypePke, 40, 0)   'v17.00 パーキンエルマーFPDは余裕を大きく取る 2010-02-09　山本
				    //roiRect.Right = h_size - IIf(DetType = DetTypePke, (h_size Mod 100) / 2, FRMWIDTH)   'v17.20変更 byやまおか 2010/09/17
				    roiRect.right = h_size - ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) 
                                && (!CTSettings.detectorParam.Use_FpdAllpix) ? (h_size % 100) / 2 : FRMWIDTH);    //v17.22変更 byやまおか 2010/10/19
				    //roiRect.Bottom = VIEW_N - FRMWIDTH
				    //roiRect.bottom = VIEW_N;
                    roiRect.bottom = VIEW_N - 1;    //VIEW方向を狭めるのはおかしい   'v18.00バグ修正 byやまおか 2011/03/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

				    //■ヒストグラム処理をし、画素値の最大・最小値を求める

				    rc = Ipc32v5.IpAoiCreateBox(ref roiRect);
				    rc = Ipc32v5.IpHstCreate();
                    if (rc != 0) throw new Exception();

				    //輝度統計を取得　～Max：HistData(4)、Min：HistData(3)
                    if (Ipc32v5.IpHstGet(Ipc32v5.GETSTATS, 0, ref HistData[0]) != 0) throw new Exception();

				    //ヒストグラムウィンドウを閉じる
                    if (Ipc32v5.IpHstDestroy() != 0) throw new Exception();

                    //■画像の濃度を１６ビットフルレンジに変換する
                    //画像のゴミ除去
                    CTLib.ChangeFullRange(ref RC_IMAGE[0], CTSettings.detectorParam.h_size, VIEW_N, Convert.ToInt32(HistData[3]), Convert.ToInt32(HistData[4]));
                    */
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    //■ヒストグラム処理を行なうＲＯＩを設定する
                    FRMWIDTH = (CTSettings.detectorParam.Use_FpdAllpix ? 0 : 3);
                    //2014/11/13hata キャストの修正
                    //roiRect.left = ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
                    //            && (!CTSettings.detectorParam.Use_FpdAllpix) ? (CTSettings.detectorParam.h_size % 100) / 2 : FRMWIDTH);
                    roiRect.left = ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
                                && (!CTSettings.detectorParam.Use_FpdAllpix) ? Convert.ToInt32((CTSettings.detectorParam.h_size % 100) / 2F) : FRMWIDTH);
    
				    roiRect.top = 0;

                    //2014/11/13hata キャストの修正
                    //roiRect.right = CTSettings.detectorParam.h_size - ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
                    //            && (!CTSettings.detectorParam.Use_FpdAllpix) ? (CTSettings.detectorParam.h_size % 100) / 2 : FRMWIDTH);    
                    roiRect.right = CTSettings.detectorParam.h_size - ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
                                && (!CTSettings.detectorParam.Use_FpdAllpix) ? Convert.ToInt32((CTSettings.detectorParam.h_size % 100) / 2F) : FRMWIDTH);    
                    
                    //roiRect.bottom = VIEW_N;
                    roiRect.bottom = VIEW_N - 1;    //VIEW方向を狭めるのはおかしい   'v18.00バグ修正 byやまおか 2011/03/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

                    rc = CallImageProFunction.CallRotationCenterParameterStep1(RC_IMAGE, RC_IMAGE, VIEW_N, CTSettings.detectorParam.h_size, roiRect.left, roiRect.top, roiRect.right, roiRect.bottom);
                    if (rc != 0) throw new Exception();
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*
				    //ゴミ除去後のImagePro再表示
				    rc = Ipc32v5.IpAppCloseAll();				                            //開いている全ての画像ウィンドウを閉じる
				    rc = Ipc32v5.IpWsCreate(h_size, VIEW_N, 300, Ipc32v5.IMC_GRAY16);		//空の画像ウィンドウを生成（Gray Scale 16形式）
				    //        tmpReg.Left = 0                                                'v9.7削除 by 間々田 2004/12/02 不要
				    //        tmpReg.Top = 0                                                 'v9.7削除 by 間々田 2004/12/02 不要
				    //        tmpReg.Right = h_size - 1                                      'v9.7削除 by 間々田 2004/12/02 不要
				    //        tmpReg.Bottom = VIEW_N - 1                                     'v9.7削除 by 間々田 2004/12/02 不要
				    rc = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref RC_IMAGE[0], Ipc32v5.CPROG);   //ユーザが作成した画像データをImage-Proに書込む
				    rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);     			    //画像ウィンドウの再描画
                    if (rc != 0) throw new Exception();

				    //■画像を白黒反転する（Ｘ線検出器がフラットパネルの場合、白黒反転処理は行わない）'v7.0 条件追加 by 間々田 2003-09-25
                    if (!CTSettings.detectorParam.Use_FlatPanel) rc = Ipc32v5.IpLutSetAttr(Ipc32v5.LUT_CONTRAST, -2);

				    //■シェーディング補正する       'V4.0 append by 鈴山 2001/01/24

				    if (GFlg_Shading_Rot) 
                    {
					    //画像のシェーディング補正   added by 山本 99-7-31
					    rc = Ipc32v5.IpFltFlatten(1, 20);
				    }

				    rc = Ipc32v5.IpDocGetArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref RC_IMAGE[0], Ipc32v5.CPROG);    //画像データの取得
				    //        rc = IpAppUpdateDoc(DOCSEL_ACTIVE)                             'v9.7削除 by 間々田 2004/12/02 不要
                    if (rc != 0) throw new Exception();
                    */
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    rc = CallImageProFunction.CallRotationCenterParameterStep2(RC_IMAGE, RC_IMAGE, VIEW_N, CTSettings.detectorParam.h_size, Convert.ToInt32(CTSettings.detectorParam.Use_FlatPanel), Convert.ToInt32(GFlg_Shading_Rot));
                    if (rc != 0) throw new Exception();
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

                    //■画像の左右端を０にする

				    //左右の耳きり落とし画像にする
				    //Call Pic_Imgside(RC_IMAGE(0), FRMWIDTH, h_size, VIEW_N)
				    //v17.00パーキンエルマーFPDは余裕を大きく取る 2010-02-09　山本
				    //Call Pic_Imgside(RC_IMAGE(0), FRMWIDTH + IIf(DetType = DetTypePke, 40, 0), h_size, VIEW_N)
				    //Call Pic_Imgside(RC_IMAGE(0), IIf(DetType = DetTypePke, (h_size Mod 100) / 2, FRMWIDTH), h_size, VIEW_N)     'v17.20変更 byやまおか 2010/09/17
                    //2014/11/13hata キャストの修正
                    //Pic_Imgside(ref RC_IMAGE[0], ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
                    //                            && (!CTSettings.detectorParam.Use_FpdAllpix) ? (CTSettings.detectorParam.h_size % 100) / 2 : FRMWIDTH), CTSettings.detectorParam.h_size, VIEW_N);    //v17.20変更 byやまおか 2010/09/17
                    Pic_Imgside(ref RC_IMAGE[0], ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
                                                && (!CTSettings.detectorParam.Use_FpdAllpix) ? Convert.ToInt32((CTSettings.detectorParam.h_size % 100) / 2F) : FRMWIDTH), CTSettings.detectorParam.h_size, VIEW_N);    //v17.20変更 byやまおか 2010/09/17

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*
				    //再表示
				    rc = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref RC_IMAGE[0], Ipc32v5.CPROG);   //ユーザが作成した画像データをImage-Proに書込む
				    rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);				    //画像ウィンドウの再描画
                    */
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    rc = CallImageProFunction.CallRotationCenterParameterStep3(RC_IMAGE, VIEW_N, CTSettings.detectorParam.h_size);
                    if (rc != 0) throw new Exception();
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

                    
                    //■画像を２値化し、フォーム表示する

				    //マウスカーソルを元に戻す
				    Cursor.Current = Cursors.Default;

				    //ＶＢ画面にフォーカスを戻す
				    //frmCTMenu.SetFocus     'v16.20/v17.00削除 byやまおか 2010/03/02

				    //２値化画像を表示する（マルチスライス校正時：フォームのキャプションに連番を表示）
                    if (!frmRotationCenterBinarized.Instance.Dialog((iMSL != 0 ? Convert.ToString(iNNN + 1) + "/" + Convert.ToString(iSMX) : ""))) throw new Exception();

				    //マウスカーソルを砂時計にする
				    Cursor.Current = Cursors.WaitCursor;

				    //■Image-Proの画像を２値化する
                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*
				    rc = Ipc32v5.IpLutBinarize(0, Threshold255_R / 256, 0);			    //２値化画像に変換(CT用)
                    */
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    //2014/11/13hata キャストの修正
                    //rc = CallImageProFunction.CallRotationCenterParameterStep4(0, Threshold255_R / 256, 0);
                    rc = CallImageProFunction.CallRotationCenterParameterStep4(0, Convert.ToInt32(Threshold255_R / 256F), 0);
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//
                    if (rc != 0) throw new Exception();
                    
				    //回転中心ワイヤの個数を取得する     'V4.0 append by 鈴山 2001/01/25
				    rc = Get_RotationCenter_Wire();
				    if (rc != 1) 
                    {
					    //メッセージ表示：
					    //   回転中心ワイヤを正しく抽出できません。
					    //   回転中心校正を再度行ってください。					    
                        MessageBox.Show(CTResources.LoadResString(9462) + "\r"
                                        + StringTable.BuildResStr(StringTable.IDS_Retry, StringTable.IDS_CorRot),
                                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return functionReturnValue;
				    }

				    //Image-Pro ２値化画像データの取得   changed by 稲葉 99-4-27
				    //        tmpReg.Left = 0                                                'v9.7削除 by 間々田 2004/12/02 不要
				    //        tmpReg.Top = 0                                                 'v9.7削除 by 間々田 2004/12/02 不要
				    //        tmpReg.Right = h_size - 1                                      'v9.7削除 by 間々田 2004/12/02 不要
				    //        tmpReg.Bottom = VIEW_N - 1                                     'v9.7削除 by 間々田 2004/12/02 不要

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*
                    rc = Ipc32v5.IpDocGetArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref RC_BILE[0], Ipc32v5.CPROG);    //画像データの取得
                    */
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    rc = CallImageProFunction.CallRotationCenterParameterStep5(RC_BILE, VIEW_N, CTSettings.detectorParam.h_size);
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//
                    if (rc != 0) throw new Exception();

				    //Image-Pro 画像データの取得
				    ///'        tmpReg.Left = 0
				    ///'        tmpReg.Top = 0
				    ///'        tmpReg.Right = H_SIZE - 1
				    ///'        tmpReg.Bottom = VIEW_N - 1
				    ///'        rc = IpDocGetArea(DOCSEL_ACTIVE, tmpReg, RC_Thin(0), CPROG)
				    ///'        If rc <> 0 Then GoTo ErrorHandler

				    //rc = IpAppCloseAll()                                         '目視確認用再表示
				    //rc = IpWsCreate(H_SIZE, VIEW_N, 300, IMC_GRAY16)  '☆☆新規の画像ｳｨﾝﾄﾞを開く
				    //tmpReg.Left = 0
				    //tmpReg.top = 0
				    //tmpReg.Right = H_SIZE - 1
				    //tmpReg.Bottom = VIEW_N - 1
				    //rc = IpDocPutArea(DOCSEL_ACTIVE, tmpReg, RC_Thin(0), CPROG)
				    //rc = IpAppUpdateDoc(DOCSEL_ACTIVE)
				    //If rc <> 0 Then GoTo ErrorHandler

                    //追加2014/10/07hata_v19.51反映 
                    //シフト量                               'v18.00追加 byやまおか 2011/07/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    sft = CTSettings.scancondpar.Data.det_sft_pix;
                    //シフト量(シフトスキャン以外のときは0)  'v18.00追加 byやまおか 2011/07/04
                    //sft_val = (modScanCorrect.Flg_RCShiftScan ? sft : 0);
                    //Rev25.00 Wスキャンを条件に追加 by長野 2016/08/08
                    sft_val = ((modScanCorrect.Flg_RCShiftScan || ScanCorrect.IsW_Scan()) ? sft : 0);

                    if (GVal_Xle[iSCN] == 0) GVal_Xle[iSCN] = CTSettings.detectorParam.h_size;
                    //v18.00追加 byやまおか 2011/07/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07   //追加2014/10/07hata_v19.51反映 
                    if (GVal_Xle_sft[iSCN] == 0)
                    {
                        GVal_Xle_sft[iSCN] = CTSettings.detectorParam.h_size + sft;
                    }
				    //ワイヤの横方向の最大値、最小値を求める     changed by 稲葉 99-4-27
				    //W_Min = H_SIZE - 1
				    //W_Max = 0
				    //For j = 0 To VIEW_N - 1
				    //    c = j * H_SIZE
				    //    For i = GVal_Xls To GVal_Xle - 1
				    //        cc = c + i
				    //        If RC_Thin(cc) <> 0 Then
				    //            If W_Min > i Then
				    //                W_Min = i
				    //            End If
				    //            If W_Max < i Then
				    //                W_Max = i
				    //            End If
				    //        End If
				    //    Next i
				    //Next j

				    //■各ビューにおいて、２値化画像範囲内の重心を求め、ビューで平均化し、回転中心画素ＸLCとする

				    //回転中心画素を求める
				    //Xlc(iSCN) = (W_Min + W_Max) / 2
				    //Xlc(iSCN) = GetRotationCenterPixelValue(RC_IMAGE, RC_Bile)   '校正画像から重心により求める deleted by 山本 2000-3-14  'C++の関数に変更
				    //Xlc(iSCN) = GetRotationCenterPixelValue_C(RC_IMAGE(0), RC_Bile(0), H_SIZE, VIEW_N)     'added by 山本 2000-3-14
                    rc = GetRotationCenterPixelValue_C(ref RC_IMAGE[0], ref RC_BILE[0], ref CPixel, CTSettings.detectorParam.h_size, VIEW_N);
				    //added by 山本 2000-3-14
				    xlc[iSCN] = CPixel;
                    xlc_sft[iSCN] = CPixel;     //v18.00追加 byやまおか 2011/07/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07   //追加2014/10/07hata_v19.51反映

				    //If (Xlc(iSCN) < 0) Then GoTo ErrorHandler         'added V2.0 by 鈴山

                    if ((rc < 0)) throw new Exception();                //added V2.0 by 山本

				    //求めた回転中心に幾何歪み補正係数をかける
				    ///'        Dim T As Double
				    ///'        Dim cp As Double
				    ///'
				    ///'        cp = Xlc(iSCN)
				    ///'
				    ///'        'cp(pixel)に対応するt(cm)を求める
				    ///'        T = (cp - Int(H_SIZE / 2) - A0(iSCN)) / A1(iSCN)
				    ///'
				    ///'        'Tに対応するxe(pixel)を求める
				    ///'        Xlc(iSCN) = ((((A5(iSCN) * T + A4(iSCN)) * T + A3(iSCN)) * T + A2(iSCN)) * T + A1(iSCN)) * T + A0(iSCN) + H_SIZE / 2

				    //チャンネル数をコモンより代入する
				    //        nd = GVal_Mainch
                    
                    //nd = CTSettings.detectorParam.h_size;    		                //v9.0 change by 間々田 2004/02/18
				    //rNd = (float)Convert.ToDouble(nd);     //全チャンネル数の実数化

                    //Rev25.00 WスキャンONの場合は左右シフト同じパラメータに変える by長野 2016/08/08
                    nd = CTSettings.scansel.Data.w_scan == 1? CTSettings.detectorParam.h_size + sft : CTSettings.detectorParam.h_size;    		                //v9.0 change by 間々田 2004/02/18
                    rNd = CTSettings.scansel.Data.w_scan == 1 ? (float)Convert.ToSingle(nd_sft) : (float)Convert.ToSingle(nd);     //全チャンネル数の実数化

                    //全チャンネル数     'v18.00追加 byやまおか 2011/07/05 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07                  
                    nd_sft = CTSettings.detectorParam.h_size + sft;  
                    rNd_sft = Convert.ToSingle(nd_sft);

				    //■ファン角を求める

				    //ファン角を求める
                    //2014/11/13hata キャストの修正
                    //xL0 = (int)(CTSettings.detectorParam.h_size / 2);   //IIの中心画素番号
                    //xL0 = Convert.ToInt32(Math.Floor(CTSettings.detectorParam.h_size / 2F));   //IIの中心画素番号
                    //Rev25.00 WスキャンONの場合は左右シフトと同じパラメータに変える by長野 2016/08/08
                    xL0 =  CTSettings.scansel.Data.w_scan == 1? Convert.ToInt32(Math.Floor((CTSettings.detectorParam.h_size / 2F) + CTSettings.scancondpar.Data.det_sft_pix_r)):Convert.ToInt32(Math.Floor(CTSettings.detectorParam.h_size / 2F));   //IIの中心画素番号

                    //Th_s = Math.Atan(10 * (GVal_Xls[iSCN] - xL0) / (GVal_Fid + CTSettings.scancondpar.Data.fid_offset[GFlg_MultiTube_R]) / A1[iSCN]);
                    //Th_e = Math.Atan(10 * (GVal_Xle[iSCN] - xL0) / (GVal_Fid + CTSettings.scancondpar.Data.fid_offset[GFlg_MultiTube_R]) / A1[iSCN]);
                    //Th_c = Math.Atan(10 * (xlc[iSCN] - xL0) / (GVal_Fid + CTSettings.scancondpar.Data.fid_offset[GFlg_MultiTube_R]) / A1[iSCN]);

                    //Rev25.00 WスキャンONの場合は左右シフトと同じパラメータに変える by長野 2016/08/08
                    Th_s = Math.Atan(10 * (CTSettings.scansel.Data.w_scan == 1 ? GVal_Xls_sft[iSCN] : GVal_Xls[iSCN] - xL0) / (GVal_Fid + CTSettings.scancondpar.Data.fid_offset[GFlg_MultiTube_R]) / A1[iSCN]);
                    Th_e = Math.Atan(10 * (CTSettings.scansel.Data.w_scan == 1 ? GVal_Xle_sft[iSCN] : GVal_Xle[iSCN] - xL0) / (GVal_Fid + CTSettings.scancondpar.Data.fid_offset[GFlg_MultiTube_R]) / A1[iSCN]);
                    Th_c = Math.Atan(10 * (CTSettings.scansel.Data.w_scan == 1 ? xlc_sft[iSCN] : xlc[iSCN] - xL0) / (GVal_Fid + CTSettings.scancondpar.Data.fid_offset[GFlg_MultiTube_R]) / A1[iSCN]);

                    //■ファン角を求める(シフト用)   'v18.00追加 byやまおか 2011/07/14 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07    //追加2014/10/07hata_v19.51反映
                    //2014/11/13hata キャストの修正
                    //xL0_sft = Convert.ToInt32((CTSettings.detectorParam.h_size / 2f) + sft);
                    //Rev23.20 左右シフトの分岐を追加 by長野 2016/01/21
                    //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
                    //if (CTSettings.scaninh.Data.lr_sft == 0)
                    if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
                    {
                        xL0_sft = Convert.ToInt32(Math.Floor((CTSettings.detectorParam.h_size / 2F) + CTSettings.scancondpar.Data.det_sft_pix_r));
                    }
                    else
                    {
                        xL0_sft = Convert.ToInt32(Math.Floor((CTSettings.detectorParam.h_size / 2F) + sft));
                    }
                    Th_s_sft = Math.Atan(10 * (GVal_Xls_sft[iSCN] - xL0_sft) / (GVal_Fid + CTSettings.scancondpar.Data.fid_offset[GFlg_MultiTube_R]) / A1[iSCN]);
                    Th_e_sft = Math.Atan(10 * (GVal_Xle_sft[iSCN] - xL0_sft) / (GVal_Fid + CTSettings.scancondpar.Data.fid_offset[GFlg_MultiTube_R]) / A1[iSCN]);
                    Th_c_sft = Math.Atan(10 * (xlc_sft[iSCN] - xL0_sft) / (GVal_Fid + CTSettings.scancondpar.Data.fid_offset[GFlg_MultiTube_R]) / A1[iSCN]);

				    for (i = 0; i <= 2; i++) 
                    {
					    //theta_s(iSCN, i) = Th_s    'v11.2削除 by 間々田 2005/10/14 未使用
					    //Theta_e(iSCN, i) = Th_e    'v11.2削除 by 間々田 2005/10/14 未使用
					    Theta_c[iSCN, i] = (float)Th_c;
                    }

                    //Rev23.20 修正 シフトスキャンのTheta_cを追加 by長野 2016/01/24
                    Theta_c[iSCN, 3] = (float)Th_c_sft;

				    //■最大スキャンエリアを求める

				    //回転中心の角度を考慮して正確なmax_scan_areaを求める Conebeamでは処理済み   'v17.61/v18.00追加 byやまおか 2011/07/30
				    thetaoff = (float)Th_c;
                    thetaoff_sft = (float)Th_c_sft;    //v19.50 v19.41とv18.02の統合 by長野 2013/11/07   //追加2014/10/07hata_v19.51反映

                    //シフトスキャンの場合 'v18.00追加 byやまおか 2011/07/24 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
				    if ((IsShiftScan())) {

					    if ((Th_c_sft - Th_s_sft) < (Th_e_sft - Th_c_sft)) {
                            mfanangle[iSCN, 3] = (float)(2 * (Th_e_sft - Th_c_sft));
					    } else {
						    mfanangle[iSCN, 3] = (float)(2 * (Th_c_sft - Th_s_sft));
					    }
					    mfanangle[iSCN, 3] = mfanangle[iSCN, 3] * LimitFanAngle;
					    //生データの端を使わないため
                        mchannel_pitch[iSCN, 3] = (float)((Th_e_sft - Th_s_sft) / rNd_sft);
					    //１チャンネルあたりのファン角
                        //2014/11/13hata キャストの修正
                        //mcenter_channel[iSCN, 3] = Convert.ToInt32((Th_c_sft - Th_s_sft) / mchannel_pitch[iSCN, 3] - 0.5) + 0.5f;
                        mcenter_channel[iSCN, 3] = Convert.ToInt32(Math.Floor((Th_c_sft - Th_s_sft) / mchannel_pitch[iSCN, 3] - 0.5)) + 0.5f;

                        //等角度補正後のセンターチャンネル
					    mchannel_pitch[iSCN, 3] = mchannel_pitch[iSCN, 3] * LimitFanAngle;
					    //MaxSArea(3) = 2 * (GVal_Fcd + scancondpar.fcd_offset(GFlg_MultiTube_R)) * Sin(mfanangle(iSCN, 3) / 2) '最大スキャンエリア(mm)
					    //最大スキャンエリア(mm)     'v18.00変更 byやまおか 2011/07/30
                        MaxSArea[3] = (float)( 2 * (GVal_Fcd + CTSettings.scancondpar.Data.fcd_offset[GFlg_MultiTube_R]) / System.Math.Cos(thetaoff_sft) * System.Math.Sin(mfanangle[iSCN, 3] / 2F));

					    //複数同時スライスの中で最小の最大スキャンエリアを共通の最大スキャンエリアとする
					    if (MaxSArea[3] < Min_MaxSArea[3])
						    Min_MaxSArea[3] = MaxSArea[3];

					    //最大スキャンエリアの最終決定
					    MaxSArea[3] = Min_MaxSArea[3];

					//ハーフ、フル、オフセットスキャンの場合 'v18.00追加 byやまおか 2011/07/24
				    }
                    //Rev25.00 コメント追加 by長野 2016/08/08
                    //WスキャンONの場合は、以降の計算に使用するパラメータは全てシフトの値で算出してある。
                    else {

				        //ハーフスキャン
                        if ((Th_c - Th_s) < (Th_e - Th_c))
                        {
                            mfanangle[iSCN, 0] = (float)(2 * (Th_c - Th_s));
                        }
                        else
                        {
                            mfanangle[iSCN, 0] = (float)(2 * (Th_e - Th_c));
                        }
				        mfanangle[iSCN, 0] = mfanangle[iSCN, 0] * LimitFanAngle;		    //added by yamamoto 99-4-9  生データの端を使わないため
				        mchannel_pitch[iSCN, 0] = mfanangle[iSCN, 0] / nd;				    //１チャンネルあたりのファン角
				        mcenter_channel[iSCN, 0] = (float)(rNd / 2 - 0.5);				    //等角度補正後のセンターチャンネル
				        //MaxSArea(0) = 2 * (GVal_Fcd + scancondpar.fcd_offset(GFlg_MultiTube_R)) * Sin(mfanangle(iSCN, 0) / 2) '最大スキャンエリア(mm)
				        //最大スキャンエリア(mm)     'v17.61/v18.00変更 byやまおか 2011/07/30
                        MaxSArea[0] = (float)(2 * (GVal_Fcd + CTSettings.scancondpar.Data.fcd_offset[GFlg_MultiTube_R]) / Math.Cos(thetaoff) * Math.Sin(mfanangle[iSCN, 0] / 2));

				        //フルスキャン
                        if ((Th_c - Th_s) < (Th_e - Th_c))
                        {
                            mfanangle[iSCN, 1] = (float)(2 * (Th_c - Th_s));
                        }
                        else
                        {
                            mfanangle[iSCN, 1] = (float)(2 * (Th_e - Th_c));
                        }
				        mfanangle[iSCN, 1] = mfanangle[iSCN, 1] * LimitFanAngle;		    //added by yamamoto 99-4-9  生データの端を使わないため
				        mchannel_pitch[iSCN, 1] = mfanangle[iSCN, 1] / nd;  			    //１チャンネルあたりのファン角
				        mcenter_channel[iSCN, 1] = (float)(rNd / 2 - 0.75);				    //等角度補正後のセンターチャンネル
				        //MaxSArea(1) = 2 * (GVal_Fcd + scancondpar.fcd_offset(GFlg_MultiTube_R)) * Sin(mfanangle(iSCN, 1) / 2) '最大スキャンエリア(mm)
				        //最大スキャンエリア(mm)     'v17.61/v18.00変更 byやまおか 2011/07/30
                        MaxSArea[1] = (float)(2 * (GVal_Fcd + CTSettings.scancondpar.Data.fcd_offset[GFlg_MultiTube_R]) / Math.Cos(thetaoff) * Math.Sin(mfanangle[iSCN, 1] / 2));

				        //オフセットスキャン
                        if ((Th_c - Th_s) < (Th_e - Th_c))
                        {
                            mfanangle[iSCN, 2] = (float)(2 * (Th_e - Th_c));
                        }
                        else
                        {
                            mfanangle[iSCN, 2] = (float)(2 * (Th_c - Th_s));
                        }
				        mfanangle[iSCN, 2] = mfanangle[iSCN, 2] * LimitFanAngle;		    //added by yamamoto 99-4-9  生データの端を使わないため
				        mchannel_pitch[iSCN, 2] = (float)((Th_e - Th_s) / nd);			    //１チャンネルあたりのファン角
                        //2014/11/13hata キャストの修正
                        //mcenter_channel[iSCN, 2] = (float)((int)((Th_c - Th_s) / mchannel_pitch[iSCN, 2] - 0.5) + 0.5);	    //等角度補正後のセンターチャンネル
                        mcenter_channel[iSCN, 2] = Convert.ToInt32(Math.Floor((Th_c - Th_s) / mchannel_pitch[iSCN, 2] - 0.5F)) + 0.5F;	    //等角度補正後のセンターチャンネル
                        
                        mchannel_pitch[iSCN, 2] = mchannel_pitch[iSCN, 2] * LimitFanAngle;
				        //MaxSArea(2) = 2 * (GVal_Fcd + scancondpar.fcd_offset(GFlg_MultiTube_R)) * Sin(mfanangle(iSCN, 2) / 2) '最大スキャンエリア(mm)
				        //最大スキャンエリア(mm)     'v17.61/v18.00変更 byやまおか 2011/07/30
                        MaxSArea[2] = (float)(2 * (GVal_Fcd + CTSettings.scancondpar.Data.fcd_offset[GFlg_MultiTube_R]) / Math.Cos(thetaoff) * Math.Sin(mfanangle[iSCN, 2] / 2));

				        //複数同時スライスの中で最小の最大スキャンエリアを共通の最大スキャンエリアとする
                        if (MaxSArea[0] < Min_MaxSArea[0]) Min_MaxSArea[0] = MaxSArea[0];
                        if (MaxSArea[1] < Min_MaxSArea[1]) Min_MaxSArea[1] = MaxSArea[1];
                        if (MaxSArea[2] < Min_MaxSArea[2]) Min_MaxSArea[2] = MaxSArea[2];

                    }
                }

			    //最大スキャンエリアの最終決定
			    MaxSArea[0] = Min_MaxSArea[0];
			    MaxSArea[1] = Min_MaxSArea[1];
			    MaxSArea[2] = Min_MaxSArea[2];

                // V3.0 append by 鈴山 START
			    //コーンビームスキャンが可能な場合
			    //If Use_DataMode3 Then
                if (CTSettings.scaninh.Data.data_mode[2] == 0)       //v11.2変更 by 間々田 2005/10/19
                {
                    //シフトスキャンの場合は横長の配列にし直す 'v18.00追加 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    //if ((IsShiftScan()))
                    //Rev25.00 Wスキャンを条件に追加 by長野 2016/08/08
                    if (IsShiftScan()||IsW_Scan())
                    {
                        tmpL_sft = VIEW_N * (CTSettings.detectorParam.h_size + sft);
                        RC_IMAGE = new ushort[tmpL_sft];
                        RC_BILE = new ushort[tmpL_sft];
                    }

                    //スキャン位置の画像をコピー
				    //        Call CopyBuff(RC_IMAGE, RC_CONE, tmpL)
                    RC_CONE.CopyTo(RC_IMAGE, 0);			    //v9.7変更 by 間々田 2004/12/03


                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*                   
                    //■イメージプロで新しい画像を開く

				    //Image-Pro 画像データの取得
				    rc = Ipc32v5.IpAppCloseAll();				                        //☆☆開いている全ての画像ｳｨﾝﾄﾞを閉じる
				    //rc = Ipc32v5.IpWsCreate(h_size, VIEW_N, 300, Ipc32v5.IMC_GRAY16);   //空の画像ウィンドウを生成（Gray Scale 16形式）
				    rc = Ipc32v5.IpWsCreate(h_size + sft_val, VIEW_N, 300, Ipc32v5.IMC_GRAY16); //空の画像ウィンドウを生成（Gray Scale 16形式）'v18.00シフト対応 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
				    tmpReg.Left = 0;
				    tmpReg.Top = 0;
				    //tmpReg.Right = h_size - 1;
				    tmpReg.Right = h_size + sft_val - 1;    //v18.00シフト対応 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
				    tmpReg.Bottom = VIEW_N - 1;

				    //■イメージプロの新規画像に回転中心校正画像を書込む

				    rc = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref RC_IMAGE[0], Ipc32v5.CPROG);   //☆☆ユーザが作成した画像ﾃﾞｰﾀをのImage-Proの画像に書込む
                    if (rc != 0) throw new Exception();
				    rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);				    //☆☆画像ウィンドの再描画
                    if (rc != 0) throw new Exception();

				    //■ヒストグラム処理を行なうＲＯＩを設定する

				    //ヒストグラム処理
				    //roiRect.Left = FRMWIDTH
				    //roiRect.Left = FRMWIDTH + IIf(DetType = DetTypePke, 40, 0)  'v17.00 パーキンエルマーFPDは余裕を大きく取る 2010-02-09　山本
				    //roiRect.Left = IIf(DetType = DetTypePke, (h_size Mod 100) / 2, FRMWIDTH)     'v17.20変更 byやまおか 2010/09/17
				    roiRect.Left = ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) && (!CTSettings.detectorParam.Use_FpdAllpix) ? (h_size % 100) / 2 : FRMWIDTH);    //v17.22変更 byやまおか 2010/10/19
				    //roiRect.Top = FRMWIDTH
				    roiRect.Top = 0;				    //VIEW方向を狭めるのはおかしい   'v17.50変更 byやまおか 2011/03/02
				    //roiRect.Right = h_size - FRMWIDTH
				    //roiRect.Right = h_size - FRMWIDTH - IIf(DetType = DetTypePke, 40, 0)    'パーキンエルマーFPDは余裕を大きく取る 2010-02-09　山本
				    //roiRect.Right = h_size - IIf(DetType = DetTypePke, (h_size Mod 100) / 2, FRMWIDTH)   'v17.20変更 byやまおか 2010/09/17
				    //roiRect.Right = CTSettings.detectorParam.h_size - ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) && (!CTSettings.detectorParam.Use_FpdAllpix) ? (CTSettings.detectorParam.h_size % 100) / 2 : FRMWIDTH);     //v17.22変更 byやまおか 2010/10/19
    				roiRect.Right = CTSettings.detectorParam.h_size + sft_val - ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) & (!CTSettings.detectorParam.Use_FpdAllpix) ? (CTSettings.detectorParam.h_size % 100) / 2 : FRMWIDTH);   //v18.00シフト対応 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

                    //roiRect.Bottom = VIEW_N - FRMWIDTH
				    //roiRect.Bottom = VIEW_N     'VIEW方向を狭めるのはおかしい   'v17.50変更 byやまおか 2011/03/02
				    roiRect.Bottom = VIEW_N - 1;	    //VIEW方向を狭めるのはおかしい   'v17.61/v18.0バグ修正 byやまおか 2011/03/02

				    //■ヒストグラム処理をし、画素値の最大・最小値を求める

				    rc = Ipc32v5.IpAoiCreateBox(ref roiRect);
				    rc = Ipc32v5.IpHstCreate();
                    if (rc != 0) throw new Exception();

				    rc = Ipc32v5.IpHstGet(Ipc32v5.GETSTATS, 0, ref HistData[0]);    //☆☆輝度統計を取得　～Max：HistData(4)、Min：HistData(3)
                    if (rc != 0) throw new Exception();
				    rc = Ipc32v5.IpHstDestroy();				                    //☆☆ヒストグラムｳｨﾝﾄﾞを閉る
                    if (rc != 0) throw new Exception();
                    
				    //■画像の濃度を１６ビットフルレンジに変換する

				    //画像のゴミ除去
                    //CTLib.ChangeFullRange(ref RC_IMAGE[0], CTSettings.detectorParam.h_size, VIEW_N, Convert.ToInt32(HistData[3]), Convert.ToInt32(HistData[4]));
				    CTLib.ChangeFullRange(ref RC_IMAGE[0], CTSettings.detectorParam.h_size + sft_val, VIEW_N, Convert.ToInt32(HistData[3]), Convert.ToInt32(HistData[4]));  //v18.00シフト対応 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    */
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    //■ヒストグラム処理を行なうＲＯＩを設定する
                    FRMWIDTH = (CTSettings.detectorParam.Use_FpdAllpix ? 0 : 3);
                    //2014/11/13hata キャストの修正
                    //roiRect.left = ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
                    //            && (!CTSettings.detectorParam.Use_FpdAllpix) ? (CTSettings.detectorParam.h_size % 100) / 2 : FRMWIDTH);    //v17.22変更 byやまおか 2010/10/19
                    roiRect.left = ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
                               && (!CTSettings.detectorParam.Use_FpdAllpix) ? Convert.ToInt32((CTSettings.detectorParam.h_size % 100) / 2F) : FRMWIDTH);    //v17.22変更 byやまおか 2010/10/19
                   
                    
                    roiRect.top = 0;		    //VIEW方向を狭めるのはおかしい   'v17.50変更 byやまおか 2011/03/02
                    //roiRect.right = CTSettings.detectorParam.h_size - ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
                    //            && (!CTSettings.detectorParam.Use_FpdAllpix) ? (CTSettings.detectorParam.h_size % 100) / 2 : FRMWIDTH);    //v17.22変更 byやまおか 2010/10/19
                    //2014/11/13hata キャストの修正
                    //roiRect.right = CTSettings.detectorParam.h_size + sft_val - ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) & (!CTSettings.detectorParam.Use_FpdAllpix) ? (CTSettings.detectorParam.h_size % 100) / 2 : FRMWIDTH);   //v18.00シフト対応 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    //roiRect.right = CTSettings.detectorParam.h_size + sft_val - ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) & (!CTSettings.detectorParam.Use_FpdAllpix) ? Convert.ToInt32((CTSettings.detectorParam.h_size % 100) / 2F) : FRMWIDTH);   //v18.00シフト対応 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    //Rev23.10 ヒストグラムの設定はシフト分は考慮しない(0埋めした領域まで含まない) by長野 2015/10/19
                    roiRect.right = CTSettings.detectorParam.h_size - ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) & (!CTSettings.detectorParam.Use_FpdAllpix) ? Convert.ToInt32((CTSettings.detectorParam.h_size % 100) / 2F) : FRMWIDTH);   //v18.00シフト対応 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    
                    //roiRect.bottom = VIEW_N;    //VIEW方向を狭めるのはおかしい   'v17.50変更 byやまおか 2011/03/02
                    roiRect.bottom = VIEW_N - 1;//VIEW方向を狭めるのはおかしい   'v17.61/v18.0バグ修正 byやまおか 2011/03/02

                    //変更2014/10/07hata_v19.51反映
                    //rc = CallImageProFunction.CallRotationCenterParameterStep1(RC_IMAGE, RC_IMAGE, VIEW_N, CTSettings.detectorParam.h_size, roiRect.left, roiRect.top, roiRect.right, roiRect.bottom);
                    rc = CallImageProFunction.CallRotationCenterParameterStep1(RC_IMAGE, RC_IMAGE, VIEW_N, CTSettings.detectorParam.h_size + sft_val, roiRect.left, roiRect.top, roiRect.right, roiRect.bottom);
                    if (rc != 0) throw new Exception();
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*
                    //ゴミ除去後のImagePro再表示
				    rc = Ipc32v5.IpAppCloseAll();				                                                    //☆☆開いている全ての画像ｳｨﾝﾄﾞを閉じる
				    //rc = Ipc32v5.IpWsCreate(h_size, VIEW_N, 300, Ipc32v5.IMC_GRAY16);				                //空の画像ウィンドウを生成（Gray Scale 16形式）
                    rc = Ipc32v5.IpWsCreate(h_size + sft_val, VIEW_N, 300, Ipc32v5.IMC_GRAY16);                     //空の画像ウィンドウを生成（Gray Scale 16形式）'v18.00シフト対応 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    rc = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref RC_IMAGE[0], Ipc32v5.CPROG);   //☆☆ユーザが作成した画像ﾃﾞｰﾀをのImage-Proの画像に書込む
				    rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);				                                //画像ウィンドの再描画
                    if (rc != 0) throw new Exception();

				    //■画像を白黒反転する

				    //画像の白黒反転     added by 山本 99-7-31
                    if (!CTSettings.detectorParam.Use_FlatPanel) rc = Ipc32v5.IpLutSetAttr(Ipc32v5.LUT_CONTRAST, -2);  //v7.0 条件追加 by 間々田 2003-09-25 Ｘ線検出器がフラットパネルの場合、白黒反転処理は行わない

				    //■シェーディング補正する       'V4.0 append by 鈴山 2001/01/24

				    if (GFlg_Shading_Rot) 
                    {
					    //画像のシェーディング補正   added by 山本 99-7-31
					    rc = Ipc32v5.IpFltFlatten(1, 20);
				    }

				    rc = Ipc32v5.IpDocGetArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref RC_IMAGE[0], Ipc32v5.CPROG);   //☆☆ユーザが作成した画像ﾃﾞｰﾀをのImage-Proの画像に書込む
				    rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);                         				    //☆☆画像ウィンドの再描画
                    if (rc != 0) throw new Exception();
                    */
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    //変更2014/10/07hata_v19.51反映
                    //rc = CallImageProFunction.CallRotationCenterParameterStep2(RC_IMAGE, RC_IMAGE, VIEW_N, CTSettings.detectorParam.h_size, Convert.ToInt32(CTSettings.detectorParam.Use_FlatPanel), Convert.ToInt32(GFlg_Shading_Rot));
                    rc = CallImageProFunction.CallRotationCenterParameterStep2(RC_IMAGE, RC_IMAGE, VIEW_N, CTSettings.detectorParam.h_size + sft_val, Convert.ToInt32(CTSettings.detectorParam.Use_FlatPanel), Convert.ToInt32(GFlg_Shading_Rot));
                    if (rc != 0) throw new Exception();
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

				    //■画像の左右端を０にする

				    //左右の耳きり落とし画像にする
				    //Call Pic_Imgside(RC_IMAGE(0), FRMWIDTH, h_size, VIEW_N)
				    //v17.00 パーキンエルマーFPDは余裕を大きく取る 2010-02-09　山本
				    //Call Pic_Imgside(RC_IMAGE(0), FRMWIDTH + IIf(DetType = DetTypePke, 40, 0), h_size, VIEW_N)
				    //Call Pic_Imgside(RC_IMAGE(0), IIf(DetType = DetTypePke, (h_size Mod 100) / 2, FRMWIDTH), h_size, VIEW_N)     'v17.20変更 byやまおか 2010/09/17
                    //Pic_Imgside(ref RC_IMAGE[0], ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) && (!CTSettings.detectorParam.Use_FpdAllpix) ? (CTSettings.detectorParam.h_size % 100) / 2 : FRMWIDTH), CTSettings.detectorParam.h_size, VIEW_N);   //v17.22変更 byやまおか 2010/10/19
                    //2014/11/13hata キャストの修正
                    //Pic_Imgside(ref RC_IMAGE[0], ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) & (!CTSettings.detectorParam.Use_FpdAllpix) ? (CTSettings.detectorParam.h_size % 100) / 2 : FRMWIDTH), CTSettings.detectorParam.h_size + sft_val, VIEW_N); //v18.00シフト対応 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    Pic_Imgside(ref RC_IMAGE[0], ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) & (!CTSettings.detectorParam.Use_FpdAllpix) ? Convert.ToInt32((CTSettings.detectorParam.h_size % 100) / 2F) : FRMWIDTH), CTSettings.detectorParam.h_size + sft_val, VIEW_N); //v18.00シフト対応 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*
				    //再表示
				    rc = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref RC_IMAGE[0], Ipc32v5.CPROG);	//☆☆ユーザが作成した画像ﾃﾞｰﾀをのImage-Proの画像に書込む
				    rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);				                                //☆☆画像ウィンドの再描画
                    */
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    //変更2014/10/07hata_v19.51反映
                    //rc = CallImageProFunction.CallRotationCenterParameterStep3(RC_IMAGE, VIEW_N, CTSettings.detectorParam.h_size);
                    rc = CallImageProFunction.CallRotationCenterParameterStep3(RC_IMAGE, VIEW_N, CTSettings.detectorParam.h_size + sft_val);
                    if (rc != 0) throw new Exception();
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


				    //マウスカーソルを元に戻す
				    Cursor.Current = Cursors.Default;

				    //■Image-Proの画像を２値化する

				    //コーンビームでも2値化画像を表示する added by 山本　2003-2-19
					if (!frmRotationCenterBinarized.Instance.Dialog(CTResources.LoadResString(12390)))
                    {
                        //戻り値をセットして抜ける（途中でキャンセル）
                        functionReturnValue = 1;
                        return functionReturnValue;
                    }

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*
				    rc = Ipc32v5.IpLutBinarize(0, Threshold255_R / 256, 0);				    //☆☆２値化画像に変換(CT用)
                    if (rc != 0) throw new Exception();

				    //Image-Pro ２値化画像データの取得
				    rc = Ipc32v5.IpDocGetArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref RC_BILE[0], Ipc32v5.CPROG);
                    if (rc != 0) throw new Exception();
                    */
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    //2014/11/13hata キャストの修正
                    //rc = CallImageProFunction.CallRotationCenterParameterStep4(0, Threshold255_R / 256, 0);
                    rc = CallImageProFunction.CallRotationCenterParameterStep4(0, Convert.ToInt32(Threshold255_R / 256F), 0);
                    if (rc != 0) throw new Exception();

                    //変更2014/10/07hata_v19.51反映
                    //rc = CallImageProFunction.CallRotationCenterParameterStep5(RC_BILE, VIEW_N, CTSettings.detectorParam.h_size);
                    rc = CallImageProFunction.CallRotationCenterParameterStep5(RC_BILE, VIEW_N, CTSettings.detectorParam.h_size + sft_val);
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//
                    if (rc != 0) throw new Exception();
               

				    //■各ビューにおいて、２値化画像範囲内の重心を求め、ビューで平均化し、回転中心画素NCとする

				    //回転中心画素を求める
                    //変更2014/10/07hata_v19.51反映
                    //rc = GetRotationCenterPixelValue_C(ref RC_IMAGE[0], ref RC_BILE[0], ref CPixel, CTSettings.detectorParam.h_size, VIEW_N);    //added by 山本 2000-3-14
                    rc = GetRotationCenterPixelValue_C(ref RC_IMAGE[0], ref RC_BILE[0], ref CPixel, CTSettings.detectorParam.h_size + sft_val, VIEW_N); //v18.00シフト対応 byやまおか 2011/07/23 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    
                    nc = CPixel;
                    nc_sft = CPixel;    //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07   //追加2014/10/07hata_v19.51反映

                    if ((rc < 0)) throw new Exception();		    //added V2.0 by 山本

				    //■コーンビームＣＴ用回転中心校正パラメータ計算

                    //Rev20.00 1次元幾何歪校正は行わないため不要 by長野 2014/11/10
                    //２次元幾何歪の場合，幾何歪テーブルを読み込まない   'v11.2 if文追加 by 間々田 2005/10/06
                    //if (CTSettings.scaninh.Data.full_distortion == 1)
                    //{
                    //    //幾何歪テーブル読み込み
                    //    if (!Read_HizumiTable(ref hizumi)) throw new Exception();
                    //}

				    //(無題)
                    h = CTSettings.detectorParam.h_size;
                    v = CTSettings.detectorParam.v_size;
                    h_sft = CTSettings.detectorParam.h_size + sft;	    //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07   //追加2014/10/07hata_v19.51反映

				    //        If H = 640 Then
				    //            kv = 1
				    //        Else
				    //            kv = 2
				    //        End If

                    //2014/11/13hata キャストの修正
                    //kv = (int)(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);	    //v7.0 FPD対応 by 間々田 2003/09/25
                    kv = Convert.ToInt32(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);	    //v7.0 FPD対応 by 間々田 2003/09/25

                    bb0 = CTSettings.scancondpar.Data.cone_scan_posi_a;  //v11.4変更 by 間々田 2006/03/16 GVal_ScanPosiA(2) → scancondpar.cone_scan_posi_a
                    //aa0 = CTSettings.scancondpar.Data.cone_scan_posi_b;  //v11.4変更 by 間々田 2006/03/16 GVal_ScanPosiB(2) → scancondpar.cone_scan_posi_b
                    aa0 = modScanCondition.real_cone_scan_posi_b;//Rev23.20 左右シフト対応 by長野 2015/11/19

				    //aa0を画像左上を原点としたときの座標に変換する  'added by 山本　2002-6-18
				    aa0 = aa0 + (v - 1) / 2 - bb0 * (h - 1) / 2;
                    //Rev23.20 左右シフト対応 by長野 2016/01/23
                    //Rev25.00 Wスキャンを追加 by長野 2016/07/07
                    //if (CTSettings.scaninh.Data.lr_sft == 0)
                    if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
                    {
                        aa0_sft = aa0 + (v - 1) / 2 - bb0 * ((h - 1) / 2 + CTSettings.scancondpar.Data.det_sft_pix_r);
                    }
                    else
                    {
                        aa0_sft = aa0 + (v - 1) / 2 - bb0 * ((h - 1) / 2 + sft);    //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07   //追加2014/10/07hata_v19.51反映
                    }

                    Fid = GVal_Fid + CTSettings.scancondpar.Data.fid_offset[GFlg_MultiTube];
                    FCD = GVal_Fcd + CTSettings.scancondpar.Data.fcd_offset[GFlg_MultiTube_R];    //v9.0 change by 間々田 2004/02/18 ???

				    //ΔΘ,n0の計算
				    ic = (h - 1) / 2;
                    //Rev23.20 左右シフトの分岐を追加 by長野 2016/01/21
                    //if (CTSettings.scaninh.Data.lr_sft == 0)
                    //Rev26.10 add Wスキャンの場合 by chouno 2018/01/13
                    if (CTSettings.scaninh.Data.lr_sft == 0 && CTSettings.W_ScanOn)
                    {
                        ic_sft = (h - 1) / 2 + CTSettings.scancondpar.Data.det_sft_pix_r;     //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07   //追加2014/10/07hata_v19.51反映
                    }
                    else
                    {
                        ic_sft = (h - 1) / 2 + sft;     //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07   //追加2014/10/07hata_v19.51反映
                    }

                    jc = (v - 1) / 2;
				    Dpi = (float)(10 / B1);
				    Dpj = kv * Dpi;                     //added by 山本　2002-6-18
				    b0_dash = kv * bb0;				    //added by 山本　2002-6-18
				    //e = Atn(a)                     'deleted by 山本　2002-6-18
				    e_dash = (float)Math.Atan(b0_dash); //added by 山本　2002-6-18

				    //２次元幾何歪の場合     'v11.2追加ここから by 間々田 2005/10/06
                    if (CTSettings.scaninh.Data.full_distortion == 0)
                    {
                        //Ils = scancondpar.ist + 2
                        //Ile = scancondpar.ied - 2
                        //v17.00変更　山本 2010-01-26 パーキンエルマー用FPD(額縁の鉛の部分を除去するため）
                        //Ils = scancondpar.ist + 2 + IIf(DetType = DetTypePke, 40, 0)
                        //Ile = scancondpar.ied - 2 - IIf(DetType = DetTypePke, 40, 0)
                        //Ils = scancondpar.ist + IIf(DetType = DetTypePke, (h_size Mod 100) / 2, 2)     'v17.20変更 byやまおか 2010/09/17
                        //Ile = scancondpar.ied - IIf(DetType = DetTypePke, (h_size Mod 100) / 2, 2)     'v17.20変更 byやまおか 2010/09/17
                        //Ils = scancondpar.ist + IIf((DetType = DetTypePke) And (Not Use_FpdAllpix), (h_size Mod 100) / 2, 2)   'v17.22変更 byやまおか 2010/10/19
                        //Ile = scancondpar.ied - IIf((DetType = DetTypePke) And (Not Use_FpdAllpix), (h_size Mod 100) / 2, 2)   'v17.22変更 byやまおか 2010/10/19
                        //'Ils = scancondpar.ist + inv_pix         '開始画素   'v18.00変更 byやまおか 2011/02/28
                        //'Ils_sft = scancondpar.ist + inv_pix     '開始画素(シフト用) 'v18.00追加 byやまおか 2011/07/09
                        //'Ile = scancondpar.ied - inv_pix            '終了画素   'v18.00変更 byやまおか 2011/02/28
                        //'Ile_sft = scancondpar.ied - inv_pix '終了画素(シフト用) 'v18.00追加 byやまおか 2011/07/09
                        //'VC側と合わせるため40画素に戻す     'v17.61/v18.00変更 byやまおか 2011/07/24
                        //Ils = scancondpar.ist + IIf((DetType = DetTypePke) And (Not Use_FpdAllpix), 40, 2)   'v17.22変更 byやまおか 2010/10/19
                        //Ile = scancondpar.ied - IIf((DetType = DetTypePke) And (Not Use_FpdAllpix), 40, 2)   'v17.22変更 byやまおか 2010/10/19
                        //'VC側と合わせるため4画素にする     'v17.64変更 byやまおか 2011/10/21
                        //Ils = scancondpar.ist + IIf((DetType = DetTypePke) And (Not Use_FpdAllpix), 4, 2)   'v17.22変更 byやまおか 2010/10/19
                        //Ile = scancondpar.ied - IIf((DetType = DetTypePke) And (Not Use_FpdAllpix), 4, 2)   'v17.22変更 byやまおか 2010/10/19
                        //±4画素だと額縁が入るので±8に変更 by長野 2013/03/06
                        //Ils = CTSettings.scancondpar.Data.ist + ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) && (!CTSettings.detectorParam.Use_FpdAllpix) ? 8 : 2);    //v19.12変更 by長野 2013/03/06
                        //Ile = CTSettings.scancondpar.Data.ied - ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) && (!CTSettings.detectorParam.Use_FpdAllpix) ? 8 : 2);    //v19.12変更 by長野 2013/03/06
  					    //v19.17 検出器毎に切り替える by長野 2013/09/12
					    //If (DetType = DetTypePke) Then
                        //v19.50 条件式変更 by長野 2013/11/07  //変更2014/10/07hata_v19.51反映
                        if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke & !CTSettings.detectorParam.Use_FpdAllpix))
                        {
                            if (CTSettings.detectorParam.h_size == 1024)
                            {
                                Ils = CTSettings.scancondpar.Data.ist + 12;
                                Ile = CTSettings.scancondpar.Data.ied - 12;
                                Ils_sft = CTSettings.scancondpar.Data.ist + 12;     //v19.50 v18.02とv19.41との統合 by長野 2013/11/21
                                Ile_sft = CTSettings.scancondpar.Data.ied - 12;     //v19.50 v18.02とv19.41との統合 by長野 2013/11/21
                            }
                            else if (CTSettings.detectorParam.h_size == 2048)
                            {
                                Ils = CTSettings.scancondpar.Data.ist + 24;
                                Ile = CTSettings.scancondpar.Data.ied - 24;
                                Ils_sft = CTSettings.scancondpar.Data.ist + 24;     //v19.50 v18.02とv19.41との統合 by長野 2013/11/21
                                Ile_sft = CTSettings.scancondpar.Data.ied - 24;     //v19.50 v18.02とv19.41との統合 by長野 2013/11/21
                            }
                        }
                        else
                        {
                            Ils = CTSettings.scancondpar.Data.ist + 2;
                            Ile = CTSettings.scancondpar.Data.ied - 2;
                            Ils_sft = CTSettings.scancondpar.Data.ist + 2;          //v19.50 v18.02とv19.41との統合 by長野 2013/11/21
                            Ile_sft = CTSettings.scancondpar.Data.ied - 2;          //v19.50 v18.02とv19.41との統合 by長野 2013/11/21
                        }
                    
                    }
                    else    //v11.2追加ここまで by 間々田 2005/10/06
                    {

                        //r = Sqr(ic ^ 2 + jc ^ 2)       'deleted by 山本　2002-6-18
                        r = (float)Math.Sqrt(Math.Pow(ic, 2) + Math.Pow((kv * jc), 2));         //added by 山本　2002-6-18
                        r_sft = (float)Math.Sqrt(Math.Pow(ic_sft, 2) + Math.Pow((kv * jc), 2)); //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        //2014/11/13hata キャストの修正
                        //mm = Convert.ToInt32(2 * r);
                        //mm_sft = Convert.ToInt32(2 * r_sft);    //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        mm = Convert.ToInt32(Math.Floor(2 * r));
                        mm_sft = Convert.ToInt32(Math.Floor(2 * r_sft));    //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        
                        //Delta_Ip = Int(ic * hizumi(mm) + 2)                            'deleted by 山本　2002-6-18
                        //2014/11/13hata キャストの修正
                        //Delta_Ip = Convert.ToInt32(ic * hizumi[(int)mm] + 2 + jc * kv * kv * Math.Abs(bb0));                        //added by 山本　2002-6-18
                        //Delta_Ip_sft = Convert.ToInt32(ic_sft * hizumi[(int)mm_sft] + 2 + jc * kv * kv * Math.Abs(bb0));                        //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        Delta_Ip = Convert.ToInt32(Math.Floor(ic * hizumi[(int)mm] + 2 + jc * kv * kv * Math.Abs(bb0)));                        //added by 山本　2002-6-18
                        Delta_Ip_sft = Convert.ToInt32(Math.Floor(ic_sft * hizumi[(int)mm_sft] + 2 + jc * kv * kv * Math.Abs(bb0)));                        //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        
                        //ILs = Delta_Ip + 1         'deleted by 山本　2002-6-18
                        Ils = Delta_Ip;                        //added by 山本　2002-6-18
                        Ils_sft = Delta_Ip_sft;                 //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        //ILe = H - Delta_Ip - 2     'deleted by 山本　2002-6-18

                        //2014/11/13hata キャストの修正
                        //Ile = (int)(h - Delta_Ip - 1);                     //added by 山本　2002-6-18
                        //Ile_sft = (int)(h_sft - Delta_Ip_sft - 1);         //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        Ile = Convert.ToInt32(h - Delta_Ip - 1);                     //added by 山本　2002-6-18
                        Ile_sft = Convert.ToInt32(h_sft - Delta_Ip_sft - 1);         //(シフト用) 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

                    }	    //v11.2追加 by 間々田 2005/10/06

				    //S1 = (ic - ILs - a * (B - a * ic + a * ILs)) * Dpi * Cos(e)                    'deleted by 山本　2002-6-18
				    //S2 = (ILe - ic + a * (B - a * ic + a * ILe)) * Dpi * Cos(e)                    'deleted by 山本　2002-6-18
                    S1 =(float)(((ic - Ils) * Dpi - (aa0 + bb0 * Ils - jc) * Dpj * b0_dash) * Math.Cos(e_dash));    //added by 山本　2002-6-18
                    S1_sft =(float)(((ic_sft - Ils_sft) * Dpi - (aa0_sft + bb0 * Ils_sft - jc) * Dpj * b0_dash) * System.Math.Cos(e_dash));				//(シフト用) 'v18.00追加 byやまおか 2011/07/09

                    S2 =(float)(((Ile - ic) * Dpi + (aa0 + bb0 * Ile - jc) * Dpj * b0_dash) * Math.Cos(e_dash));    //added by 山本　2002-6-18
                    S2_sft =(float)(((Ile_sft - ic_sft) * Dpi + (aa0_sft + bb0 * (Ile_sft) - jc) * Dpj * b0_dash) * Math.Cos(e_dash));				//(シフト用) 'v18.00追加 byやまおか 2011/07/09

                    Theta1 = (float)-Math.Atan(S1 / Fid);
                    Theta1_sft = (float)-System.Math.Atan(S1_sft / Fid);		//(シフト用) 'v18.00追加 byやまおか 2011/07/09
                    Theta2 = (float)Math.Atan(S2 / Fid);
                    Theta2_sft = (float)Math.Atan(S2_sft / Fid);				//(シフト用) 'v18.00追加 byやまおか 2011/07/09

                    //delta_theta = (Theta2 - Theta1) / (h - 1)       'V3.0 change by 鈴山 2000/09/14 ("*"→"/")
				    delta_theta = (Theta2 - Theta1) / (h - 1) * LimitFanAngle;		    //v17.61/v18.00変更 byやまおか 2011/07/18
                    delta_theta_sft = (Theta2_sft - Theta1_sft) / (h_sft - 1) * LimitFanAngle;      //(シフト用) 'v18.00追加 byやまおか 2011/07/09
                  
                    n0 = -Theta1 * (h - 1) / (Theta2 - Theta1);
                    n0_sft = -Theta1_sft * (h_sft - 1) / (Theta2_sft - Theta1_sft);				//(シフト用) 'v18.00追加 byやまおか 2011/07/09

                    //Rev25.00 WスキャンONなら、ここでパラメータを全部シフトに置き換える by長野 2016/08/08
                    if (IsW_Scan())
                    {
                        S1 = S1_sft;
                        S2 = S2_sft;
                        Theta1 = Theta1_sft;
                        Theta2 = Theta2_sft;
                        delta_theta = delta_theta_sft;
                        h = h_sft;
                    }

				    //(無題)
                    if ((nc <= h - 1 - nc))
                    {
                        //非オフセット：左オフセットの場合
                        n1[0] = 0;
                        //2014/11/13hata キャストの修正
                        //n2[0] = (int)(2 * nc) + 1;
                        n2[0] = Convert.ToInt32(Math.Floor(2 * nc)) + 1;

                        //2014/11/13hata キャストの修正
                        //if ((n2[0] > h - 1)) n2[0] = (int)h - 1;
                        if ((n2[0] > h - 1)) n2[0] = Convert.ToInt32(h) - 1;
                        theta0[0] = 2 * (nc + 0.5) * delta_theta;
                    }
                    else
                    {
                        //非オフセット：右オフセットの場合
                        //2014/11/13hata キャストの修正
                        //n1[0] = (int)(2 * nc + 1 - h);
                        n1[0] = Convert.ToInt32(Math.Floor(2 * nc + 1 - h));

                        if ((n1[0] < 0)) n1[0] = 0;
                        //2014/11/13hata キャストの修正
                        //n2[0] = (int)h - 1;
                        n2[0] = Convert.ToInt32(h) - 1;
                        
                        theta0[0] = 2 * (n2[0] - nc + 0.5) * delta_theta;
                    }

				    //非オフセット：共通パラメータ計算
				    theta01[0] = (nc - n1[0] + 0.5) * delta_theta;
				    theta02[0] = (n2[0] - nc + 0.5) * delta_theta;
				    //thetaoff = (nc - n0) * delta_theta
				    thetaoff = (Convert.ToSingle(nc) - Convert.ToSingle(n0)) * Convert.ToSingle(delta_theta);    //vc側と合わせるために Single型にする 'v11.4変更 by 間々田 2006/03/16
                    thetaoff_sft = (Convert.ToSingle(nc_sft) - Convert.ToSingle(n0_sft)) * Convert.ToSingle(delta_theta_sft);   //v18.00追加 byやまおか 2011/07/16 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

				    //オフセットスキャン：共通パラメータ計算
				    n1[1] = 0;
                    //2014/11/13hata キャストの修正
                    //n2[1] = (int)h - 1;
                    n2[1] = Convert.ToInt32(h - 1);

				    theta01[1] = (nc + 0.5) * delta_theta;
				    theta02[1] = (n2[1] - nc + 0.5) * delta_theta;

                    //シフトスキャン：共通パラメータ計算     'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07 //追加2014/10/07hata_v19.51反映
                    n1[2] = 0;
                    //2014/11/13hata キャストの修正
                    //n2[2] = (int)h_sft - 1;
                    n2[2] = Convert.ToInt32(h_sft - 1);
                    
                    theta01[2] = (nc_sft + 0.5) * delta_theta_sft;
                    theta02[2] = (n2[2] - nc_sft + 0.5) * delta_theta_sft;

				    //(無題)
                    if ((nc >= n0))
                    {
                        //オフセットスキャン：右オフセットの場合
                        ioff = 1;
                        theta0[1] = 2 * theta01[1];
                    }
                    else
                    {
                        //オフセットスキャン：左オフセットの場合
                        ioff = -1;
                        theta0[1] = 2 * theta02[1];
                    }

                    //シフトスキャン     'v18.00追加 byやまおか 2011/07/03 'v19.50 追加 by長野 2014/01/17
                    if ((nc >= n0))
                    {
                        //右オフセットの場合
                        ioff = 1;
                        theta0[2] = 2 * theta01[2];
                    }
                    else
                    {
                        //左オフセットの場合
                        ioff = -1;
                        theta0[2] = 2 * theta02[2];
                    }


				    //最大スキャンエリア(ｺｰﾝﾋﾞｰﾑCT用)を求める   'V3.0 append by 鈴山 2000/10/05
                    //scan_mode = (int)modLibrary.MaxVal(CTSettings.scansel.Data.scan_mode, 2);
				    //scan_mode = scan_mode - 2;
				    //GVal_MaxConeArea = (float)(2 * FCD * Math.Sin(theta0[scan_mode] / 2) / Math.Cos(thetaoff));
                    //シフト対応 'v18.00変更 byやまおか 2011/07/16 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    switch (CTSettings.scansel.Data.scan_mode)
                    {
                        case 1:
                        case 2:
                            scan_mode = 0;
                            GVal_MaxConeArea = (float)(2 * FCD * Math.Sin(theta0[scan_mode] / 2) / Math.Cos(thetaoff));
                            break;
                        case 3:
                            scan_mode = 1;
                            GVal_MaxConeArea = (float)(2 * FCD * Math.Sin(theta0[scan_mode] / 2) / Math.Cos(thetaoff));
                            break;
                        case 4:
                            scan_mode = 2;
                            GVal_MaxConeArea = (float)(2 * FCD * Math.Sin(theta0[scan_mode] / 2) / Math.Cos(thetaoff_sft));
                            break;
                    }

                }

			    //マウスカーソルを元に戻す
			    Cursor.Current = Cursors.Default;

			    //戻り値をセットして抜ける（正常終了）
			    functionReturnValue = 0;
            }
            catch
            {
                //マウスカーソルを元に戻す
                Cursor.Current = Cursors.Default;

                if (xlc[iSCN] < 0)
                {
                    //メッセージ表示：
                    //   回転中心ワイヤが欠けている可能性があります。
                    //   回転中心校正を再度行ってください。
                    MessageBox.Show(CTResources.LoadResString(9461) + "\r"
                                    + StringTable.BuildResStr(StringTable.IDS_Retry, StringTable.IDS_CorRot),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    //メッセージ表示：
                    //   回転中心校正パラメータ計算に失敗しました。
                    //   回転中心校正を再度行ってください。
                    MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_ErrCalPara, StringTable.IDS_CorRot) + "\r" 
                                    + StringTable.BuildResStr(StringTable.IDS_Retry, StringTable.IDS_CorRot),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }         
			return functionReturnValue;
		}


        //********************************************************************************
        //機    能  ：  回転中心ワイヤの個数を取得する
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：                   [ /O] Long      回転中心ワイヤの個数(1:正常,1以外:異常)
        //補    足  ：  なし
        //
        //履    歴  ：  V4.0   01/01/25  (SI1)鈴山       新規作成
        //********************************************************************************
		private static int Get_RotationCenter_Wire()
		{
			int rc = 0;
			int cnt = 0;
			string clptxt = null;			//ImageProのクリップボード経由の測定データ
			string swork = null;			//カウント値をclptxtから取り出したもの
			string ch = null;

			//戻り値初期化
            int functionReturnValue = 0;

			//エラー時の扱い
            try
            {
                //回転中心ワイヤの個数を測定

                #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                /*
                rc = Ipc32v5.IpBlbSetAttr(Ipc32v5.BLOB_AUTORANGE, 1);		//自動輝度範囲選択
			    rc = Ipc32v5.IpBlbSetAttr(Ipc32v5.BLOB_BRIGHTOBJ, 1);		//明るいオブジェクトを選択する
			    rc = Ipc32v5.IpBlbSetAttr(Ipc32v5.BLOB_FILTEROBJECTS, 0);   //選別レンジを非適用とする  'addedb by 山本 2001-3-27
			    //rc = IpBlbEnableMeas(BLBM_MEANFERRET, 1)                   '測定項目Diameter(ave)平均直径を選択します  'deleted by 山本 2001-3-27
			    //rc = IpBlbSetFilterRange(BLBM_MEANFERRET, 15#, 1000000#)   'オブジェクトの選別を行うためのレンジ設定   'deleted by 山本 2001-3-27
			    rc = Ipc32v5.IpBlbMeasure();			                    //選択された計測を実行
			    rc = Ipc32v5.IpBlbCount();			                        //カウント／サイズの実行
			    rc = Ipc32v5.IpBlbUpdate(0);		                        //アクティブなウィンドウを更新
			    rc = Ipc32v5.IpBlbShowData(1);			                    //測定データウィンドウをオープン
			    rc = Ipc32v5.IpBlbSaveData("", Ipc32v5.S_CLIPBOARD + Ipc32v5.S_HEADER + Ipc32v5.S_Y_AXIS);	//測定データをクリップボードにコピー
			    rc = Ipc32v5.IpBlbShowData(0);			                    //測定データウィンドウをクローズ
			    rc = Ipc32v5.IpBlbShow(0);			                        //カウント／サイズウィンドウをクローズ
			    */
                //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                rc = CallImageProFunction.CallRotationCenterWire();
                #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

                clptxt = Clipboard.GetText();

			    //クリップボードから得た測定値の中の、データ部を取り出す
			    swork = "";
                for (cnt = 1; cnt <= clptxt.Length; cnt++)
                {
                    ch = clptxt.Substring(cnt - 1, 1);
                    if((int)Convert.ToChar(ch) < 0x20)
                    {
                        //CRを検出したら、その直前のデータがデータ部
                        if ((int)Convert.ToChar(ch) == 13) break;

                        swork = "";
                    }
                    else
                    {
                        //制御記号が出てくるまでは文字を加算する
                        swork = swork + ch;
                    }
                }

                //戻り値
			    //functionReturnValue = Conversion.Val(Strings.Trim(swork));
                int d;
                if (int.TryParse(swork.Trim(),out d))
                {
                    functionReturnValue = d;
                }
 
             }
            catch
            {
                // Nothing
            }
			return functionReturnValue;
		}


        //********************************************************************************
        //機    能  ：  幾何歪校正パラメータ計算
        //              変数名           [I/O] 型        内容
        //引    数  ：  iMSL             [I/ ] Long      同時スキャン枚数(0:1ｽﾗｲｽ,1:3ｽﾗｲｽ,2:5ｽﾗｲｽ)
        //戻 り 値  ：                   [ /O] Long      結果(0:正常,1:異常)
        //補    足  ：  "■"で始まるコメント部は、なるべく設計書のフローチャートと一致する
        //              ようにしてください。
        //
        //履    歴  ：   V2.0   00/02/08  (SI1)鈴山       新規作成
        //               v9.7  04/11/24  (SI4)間々田     引数 MessageOn を追加
        //               v11.2   05/10/12  (SI3)間々田   ２次元幾何歪対応
        //********************************************************************************
        public static int Get_Vertical_Parameter_Ex(int iMSL, bool messageOn = false)
        {
            int rc = 0;
            //Ipc32v5.RECT tmpReg = default(Ipc32v5.RECT);
            //Winapi.RECT tmpReg = default(Winapi.RECT);

            int tmpL = 0;

            float[] HistData = new float[11];
            int i = 0;
            //int j = 0;
            //int c = 0;			                //画像配列用カウンタ
            double[] Cross_X = new double[31];	//水平線と垂直線の交点X座標
            double[] Cross_Y = new double[31];	//水平線と垂直線の交点Y座標

            //int xl = 0;			                //入力画素座標(補正後の画素位置）
            //double XE = 0;	                    //補正前の画素座標

            //double TT = 0;
            //double XE_B = 0;	                //直前のXE
            //bool DrawFlag = false;			    //前回画像を描いた場合：true 画像を描かなかった場合：false

            //double ver_a = 0;			        //垂直線の傾き   'V3.0 change by 鈴山 (a → ver_a)
            //double ver_b = 0;			        //垂直線の切片   'V3.0 change by 鈴山 (b → ver_b)
            float pos_a = 0;			        //水平線の傾き   'V3.0 append by 鈴山
            //float pos_b = 0;			        //水平線の切片   'V3.0 append by 鈴山

            //画像取込み用。以下、交点算出前までの画像処理操作追加 by 中島　'99-8-17
            //Ipc32v5.RECT roiRect = default(Ipc32v5.RECT);			//ヒストグラム用矩形
            Winapi.RECT roiRect = default(Winapi.RECT);

            double[] ti = new double[51];		//横軸-実ワイヤ距離 ti(cm)  　　　　　　　 'changed by 山本　2002-1-31　30->50
            double[] xi = new double[51];		//縦軸-画像から求めたワイヤ距離 xi(pixel)　'changed by 山本　2002-1-31　30->50

            //マルチスライス対応   added V2.0 by 鈴山
            int iSCN = 0;			            //スキャン位置

            //傾きと切片を求める為の変数   added V2.0 by 鈴山
            float fFID = 0;			            //FID(FID+FIDｵﾌｾｯﾄ)
            float fFCD = 0;			            //FCD(FCD+FCDｵﾌｾｯﾄ)
            float fMDT = 0;			            //検出器ﾋﾟｯﾁ(mm/画素)
            int iVMG = 0;			            //縦倍率
            float fSLP = 0;			            //ｽﾗｲｽﾋﾟｯﾁ(mm)

            //垂直線の重心を求める為の変数   added by 稲葉 2001/01/23
            //double Sigma_F = 0;			        //画像ﾃﾞｰﾀの和
            //double Sigma_FX = 0;			    //画像ﾃﾞｰﾀ＊X座標の和
            //bool VRT_Cnt_Flag = false;			//垂直線ワイヤのカウンタフラグ

            //最大ファン角を求める為の変数   added by 間々田 2004/04/26
            int h = 0;
            int v = 0;
            float kv = 0;
            float bb0 = 0;
            float FGD = 0;
            float ic = 0;
            float jc = 0;
            float Dpi = 0;
            float e_dash = 0;
            float r = 0;
            float mm = 0;
            float Delta_Ip = 0;
            float Ils = 0;
            float Ile = 0;
            int sft_val = 0;            //v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

            //Dim fpd_pitch As Single                                                 'v9.7追加 下記For～Next内から移動 by 間々田 2004/11/25
            //const int H_grphrng = 30;		//数字を大きくするとグラフが横に広がる   'v9.7追加 下記For～Next内から移動 by 間々田 2004/11/25
            //const int V_grphrng = 4;		//数字を小さくするとグラフが縦に広がる   'v9.7追加 下記For～Next内から移動 by 間々田 2004/11/25

            //戻り値初期化
            int functionReturnValue = -1;

            try
            {
                //現在のコモン内容を取り出す     'added by 山本　2003-10-2　FPDの場合、H_SIZEに値をロードするため
                if (CTSettings.detectorParam.Use_FlatPanel) OptValueGet_Cor();

                //sft_val = (IsShiftScan() ? CTSettings.scancondpar.Data.det_sft_pix : 0);     //v18.00追加 byやまおか 2011/07/14 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07  //追加2014/10/07hata_v19.51反映
                //Rev25.00 Wスキャンを条件に追加 by長野 2016/08/08
                sft_val = ((IsShiftScan() || IsW_Scan()) ? CTSettings.scancondpar.Data.det_sft_pix : 0);     //v18.00追加 byやまおか 2011/07/14 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07  //追加2014/10/07hata_v19.51反映
                //配列の初期化
                tmpL = CTSettings.detectorParam.v_size * CTSettings.detectorParam.h_size;
                CVRT_IMAGE = new ushort[tmpL];		    //減算結果画像データ配列             'v15.0変更 -1した by 間々田 2009/06/03
                VRT_THIN_IMAGE = new ushort[tmpL];	    //垂直線細線化画像データの配列       'v15.0変更 -1した by 間々田 2009/06/03
                CRT_IMAGE = new ushort[tmpL];		    //幾何歪み補正後の画像データの配列   'v15.0変更 -1した by 間々田 2009/06/03
                FITTING_IMAGE = new byte[tmpL];		//フィッティング曲線画像データの配列 'v15.0変更 -1した by 間々田 2009/06/03

                //v7.0 FPD対応 by 間々田 2003/09/25
                int adv = 0;
                if (!CTSettings.detectorParam.Use_FlatPanel)
                {

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*
                    //■イメージプロで新しい画像を開く

                    //Image-Pro 画像データ表示
                    rc = Ipc32v5.IpAppCloseAll();				                            //☆☆開いている全ての画像ｳｨﾝﾄﾞを閉じる
                    rc = Ipc32v5.IpWsCreate(h_size, v_size, 300, Ipc32v5.IMC_GRAY16);		//空の画像ウィンドウを生成（Gray Scale 16形式）

                    //■イメージプロの新規画像に幾何歪校正画像を書込む
                    tmpReg.Left = 0;
                    tmpReg.Top = 0;
                    tmpReg.Right = h_size - 1;
                    tmpReg.Bottom = v_size - 1;
                    rc = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref VRT_IMAGE[0], Ipc32v5.CPROG);  //☆☆ユーザが作成した画像ﾃﾞｰﾀをImage-Proの画像に書込む
                    rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);				                                //☆☆画像ウィンドの再描画

                    //Image-Pro 画像データの配列への読み込み
                    rc = Ipc32v5.IpDocGetArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref CVRT_IMAGE[0], Ipc32v5.CPROG); //added by 山本 2005-12-17
                    */
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    rc = CallImageProFunction.CallVerticalParameterStep1(VRT_IMAGE, CVRT_IMAGE, CTSettings.detectorParam.v_size, CTSettings.detectorParam.h_size);
                    if (rc != 0) throw new Exception();
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


                    /////ゲイン校正を行い、焼付きやごみの影響を少なくする///added by 山本　2005-11-24　 start //////////////////				    
                    if (CTSettings.scaninh.Data.full_distortion == 0)    //v11.2 if文追加 by 間々田 2005/10/13
                    {
                        if (!GetGainImage()) throw new Exception();
                        //ゲイン画像データの最大値を求める
                        ScanCorrect.Cal_Max_short(ref GAIN_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, ref GainMax);
                        adv = (int)GainMax;
                        ScanCorrect.FpdGainCorrect(ref CVRT_IMAGE[0], ref GAIN_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, adv);

                        #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                        /*
                        rc = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref CVRT_IMAGE[0], Ipc32v5.CPROG); //☆☆ユーザが作成した画像ﾃﾞｰﾀをImage-Proの画像に書込む
                        rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);                     					    //☆☆画像ウィンドの再描画
                        */
                        //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                        rc = CallImageProFunction.CallVerticalParameterStep2(CVRT_IMAGE, CTSettings.detectorParam.v_size, CTSettings.detectorParam.h_size);
                        if (rc != 0) throw new Exception();
                        #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//
                    }

                    /////ゲイン校正を行い、ごみの影響を少なくする///added by 山本　2005-11-24　 END  //////////////////

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*
                    //■画像を白黒反転させる（２次元幾何歪補正時は反転させない）
                    if (CTSettings.scaninh.Data.full_distortion == 1)        //v11.2 if文追加 by 間々田 2005/10/13
                    {
                        rc = Ipc32v5.IpLutSetAttr(Ipc32v5.LUT_CONTRAST, -2);
                    }

                    //■シェーディング補正する       '下から移動してきた　by 山本　2005-12-2
                    if (modScanCorrect.GFlg_Shading_Ver == 1)
                    {
                        rc = Ipc32v5.IpFltFlatten(1, 20);
                    }

                    rc = Ipc32v5.IpDocGetArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref CVRT_IMAGE[0], Ipc32v5.CPROG);	    //イメージプロの画像を配列に取り込む 'v11.2追加 by 間々田 2005/10/12

                    //■ヒストグラム処理を行なうＲＯＩを設定する
                    FRMWIDTH = (CTSettings.detectorParam.Use_FpdAllpix ? 0 : 3);			//v17.22追加 byやまおか 2010/10/19
                    FRMHEIGHT = (CTSettings.detectorParam.Use_FpdAllpix ? 0 : 2);			//v17.22追加 byやまおか 2010/10/19

                    //ヒストグラム処理
                    roiRect.left = FRMWIDTH;
                    roiRect.top = FRMWIDTH;
                    roiRect.right = h_size - FRMWIDTH;
                    roiRect.bottom = v_size - FRMWIDTH;

                    //■ヒストグラム処理をし、画素値の最大・最小値を求める

                    rc = Ipc32v5.IpAoiCreateBox(ref roiRect);
                    rc = Ipc32v5.IpHstCreate();				                        //☆☆ヒストグラムｳｨﾝﾄﾞを開く
                    rc = Ipc32v5.IpHstGet(Ipc32v5.GETSTATS, 0, ref HistData[0]);    //☆☆輝度統計を取得　～Max：HistData(4)、Min：HistData(3)
                    rc = Ipc32v5.IpHstDestroy();				                    //☆☆ヒストグラムｳｨﾝﾄﾞを閉る

                    //■画像の濃度を１６ビットフルレンジに変更する

                    //画像のゴミ除去
                    ChangeFullRange(ref CVRT_IMAGE[0], h_size, v_size, Convert.ToInt32(HistData[3]), Convert.ToInt32(HistData[4]));

                    //ゴミ除去後のImagePro再表示
                    rc = Ipc32v5.IpAppCloseAll();				                                                        //☆☆開いている全ての画像ｳｨﾝﾄﾞを閉じる
                    rc = Ipc32v5.IpWsCreate(h_size, v_size, 300, Ipc32v5.IMC_GRAY16);				                    //空の画像ウィンドウを生成（Gray Scale 16形式）
                    rc = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref CVRT_IMAGE[0], Ipc32v5.CPROG);     //commented by 山本 99-7-31
                    rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);				                                    //☆☆画像ウィンドの再描画
                    */
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    //■ヒストグラム処理を行なうＲＯＩを設定する
                    FRMWIDTH = (CTSettings.detectorParam.Use_FpdAllpix ? 0 : 3);
                    FRMHEIGHT = (CTSettings.detectorParam.Use_FpdAllpix ? 0 : 2);
                    //ヒストグラム処理
                    roiRect.left = FRMWIDTH;
                    roiRect.top = FRMWIDTH;
                    roiRect.right = CTSettings.detectorParam.h_size - FRMWIDTH;
                    roiRect.bottom = CTSettings.detectorParam.v_size - FRMWIDTH;
                    rc = CallImageProFunction.CallVerticalParameterStep3(CVRT_IMAGE, CTSettings.detectorParam.v_size, CTSettings.detectorParam.h_size, roiRect.left, roiRect.top, roiRect.right, roiRect.bottom, CTSettings.scaninh.Data.full_distortion, modScanCorrect.GFlg_Shading_Ver);
                    if (rc != 0) throw new Exception();
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


                    //    '■シェーディング補正する     '上に移動のため削除 by 山本　2005-12-2
                    //    If GFlg_Shading_Ver = 1 Then
                    //        rc = IpFltFlatten(1, 20)
                    //        rc = IpDocGetArea(DOCSEL_ACTIVE, tmpReg, CVRT_IMAGE(0), CPROG)  'イメージプロの画像を配列に取り込む 'v11.2追加 by 間々田 2005/10/12
                    //    End If
                    //
                    //■画像の周辺部を０にする

                    //周囲の耳きり落とし画像にする
                    //Call Pic_Imgside(CVRT_IMAGE(0), FRMWIDTH, h_size, v_size)
                    Zero_ImageMargin(ref CVRT_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, FRMWIDTH, FRMHEIGHT);	    //v11.2変更 by 間々田 2005/11/11

                    //再表示
                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*
                    rc = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref CVRT_IMAGE[0], Ipc32v5.CPROG);	    //☆☆ユーザが作成した画像ﾃﾞｰﾀをImage-Proの画像に書込む
                    rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);				                                    //☆☆画像ウィンドの再描画
                    */
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    rc = CallImageProFunction.CallVerticalParameterStep2(CVRT_IMAGE, CTSettings.detectorParam.v_size, CTSettings.detectorParam.h_size);
                    if (rc != 0) throw new Exception();
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


                    //■画像を２値化し、フォームに表示する

                    //マウスカーソルを元に戻す
                    Cursor.Current = Cursors.Default;

                    //ＶＢ画面にフォーカスを戻す             V4.0 append by 鈴山 2001/02/01
                    //frmCTMenu.SetFocus                     'v9.7削除 by 間々田 2004/11/17

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
                    'CT30Kをアクティブにする
                    AppActivate App.Title                  'v9.7追加 by 間々田 2004/11/17
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                    //CT30Kをアクティブにする
                    frmCTMenu.Instance.Activate();                        //v9.7追加 by 間々田 2004/11/17

                    //２値化画像を表示する by 中島 '99-8-17
                    if (!frmVerticalBinarized.Instance.Dialog())
                    {
                        functionReturnValue = 1;        //戻り値をセット
                        return functionReturnValue;
                    }

                    //マウスカーソルを砂時計にする
                    Cursor.Current = Cursors.WaitCursor;

                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    /*
                    //２値化
                    rc = Ipc32v5.IpLutBinarize(0, Threshold255_V / 256, 0);		    //☆☆２値化画像に変換

                    //■２値化画像を配列 BVRT_IMAGE に取込む

                    //配列の初期化    added V2.0 by 鈴山
                    tmpL = Convert.ToInt32(v_size * h_size);
                    BVRT_IMAGE = new short[tmpL];				    //幾何歪校正画像２値化データの配列    'v15.0変更 -1した by 間々田 2009/06/03

                    //Image-Pro 画像データの取得    added V2.0 by 鈴山
                    rc = Ipc32v5.IpDocGetArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref BVRT_IMAGE[0], Ipc32v5.CPROG);				    //現在のImage-Pro画像を配列に読み込む
                    if (rc != 0) throw new Exception();
                    */
                    //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                    //配列の初期化    added V2.0 by 鈴山
                    tmpL = Convert.ToInt32(CTSettings.detectorParam.v_size * CTSettings.detectorParam.h_size);
                    BVRT_IMAGE = new ushort[tmpL];
                    //2014/11/13hata キャストの修正
                    //rc = CallImageProFunction.CallVerticalParameterStep4(BVRT_IMAGE, CTSettings.detectorParam.v_size, CTSettings.detectorParam.h_size, 0, Threshold255_V / 256, 0);
                    rc = CallImageProFunction.CallVerticalParameterStep4(BVRT_IMAGE, CTSettings.detectorParam.v_size, CTSettings.detectorParam.h_size, 0, Convert.ToInt32(Threshold255_V / 256F), 0);
                    if (rc != 0) throw new Exception();
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//


                    #region			    //v11.2削除ここから by 間々田 2005/10/12
                    //'■２値化画像 BVRT_IMAGE の再表示
                    //
                    //'２値化画像の再表示    added V2.0 by 鈴山
                    //'@    rc = IpAppCloseAll()                                              '☆☆開いている全ての画像ｳｨﾝﾄﾞを閉じる   deleted V2.0 by 鈴山
                    //rc = IpWsCreate(h_size, v_size, 300, IMC_GRAY16)                    '空の画像ウィンドウを生成（Gray Scale 16形式）
                    //rc = IpDocPutArea(DOCSEL_ACTIVE, tmpReg, BVRT_IMAGE(0), CPROG)      '☆☆ユーザが作成した画像ﾃﾞｰﾀをのImage-Proの画像に書込む
                    //rc = IpAppUpdateDoc(DOCSEL_ACTIVE)                                  '☆☆画像ウィンドの再描画
                    //If rc <> 0 Then GoTo Err_Process
                    //
                    //'/////////細線化法→重心法に変更(1/5) deleted by 稲葉 2002/01/23//////////////
                    ///'    '■垂直線ワイヤを細線化する
                    ///'    '画像の細線化
                    ///'    rc = IpFltThin(BinarizeConst)
                    //
                    //'Image-Pro 画像データの取得
                    //rc = IpDocGetArea(DOCSEL_ACTIVE, tmpReg, VRT_THIN_IMAGE(0), CPROG)  '☆☆ユーザが作成した画像ﾃﾞｰﾀをのImage-Proの画像に書込む
                    //v11.2削除ここまで by 間々田 2005/10/12
                    #endregion

                }

                //■各スキャン位置を示す１次方程式の傾きと切片を求める

                //複数スライスのとき、各スキャン位置を示す１次方程式の傾きと切片を求める    added V2.0 by 鈴山			    
                if (MultiSliceMode)         //←If文を追加 by 鈴山 2000/02/23
                {
                    fFID = GVal_Fid + CTSettings.scancondpar.Data.fid_offset[GFlg_MultiTube];
                    fFCD = GVal_Fcd + CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()];		    //v9.0 change by 間々田 2004/02/18
                    fMDT = GVal_mdtpitch[2];

                    //2014/11/13hata キャストの修正
                    //iVMG = (int)(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);
                    iVMG = Convert.ToInt32(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);

                    //fSLP = GVal_MltPitch
                    fSLP = CTSettings.scansel.Data.multislice_pitch;		    //v10.0 change by 間々田 2005/02/14
                    GVal_ScanPosiA[0] = GVal_ScanPosiA[2];
                    GVal_ScanPosiA[1] = GVal_ScanPosiA[2];
                    GVal_ScanPosiA[2] = GVal_ScanPosiA[2];
                    GVal_ScanPosiA[3] = GVal_ScanPosiA[2];
                    GVal_ScanPosiA[4] = GVal_ScanPosiA[2];
                    GVal_ScanPosiB[0] = GVal_ScanPosiB[2] - 2 * (fFID / fFCD) * (fSLP / (fMDT * iVMG));
                    GVal_ScanPosiB[1] = GVal_ScanPosiB[2] - 1 * (fFID / fFCD) * (fSLP / (fMDT * iVMG));
                    GVal_ScanPosiB[2] = GVal_ScanPosiB[2];
                    GVal_ScanPosiB[3] = GVal_ScanPosiB[2] + 1 * (fFID / fFCD) * (fSLP / (fMDT * iVMG));
                    GVal_ScanPosiB[4] = GVal_ScanPosiB[2] + 2 * (fFID / fFCD) * (fSLP / (fMDT * iVMG));
                }

                //v11.2追加ここから by 間々田 2005/10/03
                //int DocId1 = 0;
                //int DocId2 = 0;
                //int DocId3 = 0;
                float[] x = null;
                float[] y = null;
                float[] area = null;

                double a_0 = 0;
                double b_0 = 0;

                double[] gArea = null;
                //Dim kmax    As Long    'deleted by 山本　2005-12-21 Public宣言に変更
                //v11.2追加ここまで by 間々田 2005/10/03

                //fpd_pitch = scancondpar.fpd_pitch              'v11.2下の方から移動 by 間々田 2005/10/21

                //同時スキャン枚数分のループ
                //For iNNN = 0 To iSMX - 1
                float[] ipICal = new float[3];
                float ConeScanPosiB = 0;                //added by 山本　2005-12-17
                
                //仮の数値を入れておく
                //kmax = 2000;
                
                for (iSCN = 2 - iMSL; iSCN <= 2 + iMSL; iSCN++)     //v11.2変更 by 間々田 2005/10/21
                {
                    //FPDの場合                              'v11.2追加ここから by 間々田 2005/10/21　下から移動
                    if (CTSettings.detectorParam.Use_FlatPanel)
                    {
                        a0[iSCN] = 0;
                        A1[iSCN] = 10 / CTSettings.scancondpar.Data.fpd_pitch / CTSettings.detectorParam.hm;
                        a2[iSCN] = 0;
                        a3[iSCN] = 0;
                        a4[iSCN] = 0;
                        a5[iSCN] = 0;
                    }
                    //v11.2追加ここまで by 間々田 2005/10/21
                    else
                    {
                        //v7.0 FPD対応 by 間々田 2003/09/25
                        //If Not Use_FlatPanel Then              'v11.2削除 by 間々田 2005/10/21


                        //２次元幾何歪補正ではない場合
                        //v11.2 if文追加 by 間々田 2005/10/13
                        if (CTSettings.scaninh.Data.full_distortion == 1)
                        {
                            #region					    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                            //
                            //                '配列の初期化
                            //                ReDim FITTING_IMAGE(tmpL - 1)         'フィッティング曲線画像データの配列     'added by 鈴山 2000-1-31    'v15.0変更 -1した by 間々田 2009/06/03
                            //
                            //                '■垂直線とスキャン位置水平線の交点座標と交点数を求める
                            //
                            //                '垂直線本数の初期化    added V2.0 by 鈴山
                            //                VRT_Count = 0
                            //                VRT_Cnt_Flag = False    'added by 山本 2002-4-11
                            //                Sigma_F = 0             'added by 山本 2002-4-11
                            //                Sigma_FX = 0            'added by 山本 2002-4-11
                            //
                            //                '水平線の傾きと切片を決める     'V3.0 append by 鈴山
                            //                pos_a = GVal_ScanPosiA(iSCN)
                            //                pos_b = GVal_ScanPosiB(iSCN)
                            //
                            //                '垂直線と水平線の交点を求める
                            //                'For j = 0 To H_SIZE - 1        'commented by 山本 99－7－31 画像の端に近い部分の垂直線は無視する
                            //                'For j = 10 To H_SIZE - 1 - 10   'added by 山本 99－7－31
                            //                For j = 20 To h_size - 1 - 20   'added by 山本 2002-1-29  左右両端の余裕を10画素から20画素に変更
                            //
                            //                    'いったん画素の縦方向の座標をcに代入する（整数化するため）
                            //                    c = Int(pos_a * j + pos_b + Int(v_size / 2) - pos_a * Int(h_size / 2)) 'xi,ti座標系からi,j座標系に戻す
                            //                    c = c * h_size + j
                            //
                            //        '/////////細線化法→重心法に変更(2/5) deleted by 稲葉 2002/01/23 START //////////////
                            //        '''            If VRT_THIN_IMAGE(C) <> 0 Then
                            //        '''                Cross_X(VRT_Count) = j
                            //        '''                Cross_Y(VRT_Count) = pos_a * j + pos_b + Int(V_SIZE / 2) - pos_a * Int(H_SIZE / 2)
                            //        '''                VRT_Count = VRT_Count + 1
                            //        '''
                            //        '''            End If
                            //        '/////////細線化法→重心法に変更(2/5) deleted by 稲葉 2002/01/23 END //////////////
                            //
                            //        '/////////細線化法→重心法に変更(3/5) added by 稲葉 2002/01/23 START //////////////
                            //                    '垂直線の重心を交点座標とする change by 稲葉 2002/01/23
                            //                    If BVRT_IMAGE(c) <> 0 Then
                            //                        Sigma_F = Sigma_F + VRT_IMAGE(c)
                            //                        Sigma_FX = Sigma_FX + VRT_IMAGE(c) * j
                            //                        VRT_Cnt_Flag = True
                            //                    ElseIf VRT_Cnt_Flag Then
                            //                        Cross_X(VRT_Count) = Sigma_FX / Sigma_F
                            //                        Cross_Y(VRT_Count) = pos_a * Cross_X(VRT_Count) + pos_b + Int(v_size / 2) - pos_a * Int(h_size / 2)
                            //                        VRT_Count = VRT_Count + 1
                            //                        Sigma_F = 0
                            //                        Sigma_FX = 0
                            //                        VRT_Cnt_Flag = False
                            //                    End If
                            //        '/////////細線化法→重心法に変更(3/5) added by 稲葉 2002/01/23 END //////////////
                            //
                            //                Next
                            //
                            //    '削除ここから by 間々田 2004/12/02 実行されない
                            //    ''If (Not DBG_Hide_Ver_Slope) Then
                            //    ''
                            //    ''            '■垂直線ワイヤとＴＶ走査線のなす角度を求める
                            //    ''
                            //    ''            '垂直線ワイヤの座標を配列に代入する
                            //    ''            cnt = 0 'added V2.0 by 鈴山
                            //    ''            For j = VlinePicPitch To v_size - 1 Step VlinePicPitch
                            //    ''                c = j * h_size
                            //    ''                W_Cnt = 0
                            //    ''                For i = 0 To h_size - 1
                            //    ''                    cc = c + i
                            //    ''                    If VRT_THIN_IMAGE(cc) <> 0 Then
                            //    ''                        W_Cnt = W_Cnt + 1       'ワイヤ本数のカウント
                            //    ''                        If W_Cnt = Int(VRT_Count / 2) Then   '（ワイヤ本数／２）番目のワイヤの座標を配列に代入する
                            //    ''                            V_X(cnt) = i
                            //    ''                            V_Y(cnt) = j
                            //    ''                            cnt = cnt + 1
                            //    ''                        End If
                            //    ''                    End If
                            //    ''                Next
                            //    ''            Next
                            //    ''
                            //    ''            '■実際のワイヤ間隔に対し入力した画像から求めたワイヤ間隔のフィッティング補正カーブ係数Ａ０～Ａ５をフィッティング計算により求める。ワイヤの本数に応じ、最大５次までのフィッティング計算を行なう
                            //    ''
                            //    ''            '垂直線ワイヤの傾きを求めるためにフィッティング計算を行う
                            //    ''            Caliculate_Wire_Slope V_X, V_Y, cnt, ver_a, ver_b           'ａが垂直線の傾きを表す（ただし９０度回転している値になっている）
                            //    ''
                            //    ''            '垂直線ワイヤの傾きを度単位で求める
                            //    ''            V_Slope = 180 * Atn(1 / ver_a) / Pai
                            //    ''
                            //    ''            If V_Slope < 0 Then         'V_Slope<0 の場合は180度加算して正の数にする。
                            //    ''                V_Slope = V_Slope + 180
                            //    ''            End If
                            //    ''
                            //    ''End If
                            //    '削除ここまで by 間々田 2004/12/02 実行されない
                            //
                            //                'フィッティング計算で求めた垂直線ワイヤの近似直線を画像に描く
                            //                'xi = a * ti + b  ---(1)   フィッティング計算で求めたａ、ｂは(xi,ti)軸でのもの
                            //                '元の(x,y)軸と(ti,xi)軸の関係は ti = y - V / 2  xi = x - H / 2 であるから、これを(1)式に代入すると
                            //                'x = a * y - a * V / 2 + b + H / 2
                            //
                            //                'これまでの処理結果の画像データを Image-Pro に渡す
                            //                If IMGPRODBG = 1 Then
                            //                    rc = IpAppCloseAll()                                'added by 山本 2002-4-11
                            //                    rc = IpWsCreate(h_size, v_size, 300, IMC_GRAY16)    '空の画像ウィンドウを生成（Gray Scale 16形式）
                            //                    rc = IpDocPutArea(DOCSEL_ACTIVE, tmpReg, VRT_THIN_IMAGE(0), CPROG)  '☆☆ユーザが作成した画像ﾃﾞｰﾀをのImage-Proの画像に書込む
                            //                    rc = IpAppUpdateDoc(DOCSEL_ACTIVE)
                            //                End If
                            //
                            //                '幾何歪校正用フィッティング計算：フィッティング係数の A0,A1,A2,A3,A4,A5 を求める
                            //    #If DebugOn Then
                            //                '幾何歪み補正ファントムのワイヤが一定ピッチでないため、パラメータを固定とした by 山本 99-7-31
                            //                a0(iSCN) = 27.19048
                            //                A1(iSCN) = 86.65
                            //                a2(iSCN) = 0.458333
                            //                a3(iSCN) = 0.895833
                            //                a4(iSCN) = -0.041667
                            //                a5(iSCN) = -0.045833
                            //
                            //                'A0(iSCN) = 0
                            //                'A1(iSCN) = 41.25318
                            //                'A2(iSCN) = 0
                            //                'A3(iSCN) = 0
                            //                'A4(iSCN) = 0
                            //                'A5(iSCN) = 0
                            //    #Else
                            //                'rc = Caliculate_FittingC(Cross_X(0), Cross_Y(0), ti(0), xi(0), A0, A1, A2, A3, A4, A5, pos_a, GVal_WirePitch, VRT_Count, MATRIX)   '←C関数は未完成
                            //                If Not Caliculate_Fitting(Cross_X, Cross_Y, ti, xi, _
                            //'                                          a0(iSCN), A1(iSCN), a2(iSCN), a3(iSCN), a4(iSCN), a5(iSCN), _
                            //'                                          pos_a, pos_b, scancondpar.ver_wire_pitch, VRT_Count, Matrix) Then 'added V2.0 by 鈴山
                            //                    GoTo Err_Process
                            //                End If
                            //    #End If
                            //
                            //                'フィッティングカーブを画像に描く（デバッグ用）
                            //    '            Dim H_grphrng As Single
                            //    '            Dim V_grphrng As Single
                            //    '            H_grphrng = 30      '数字を大きくするとグラフが横に広がる
                            //    '            'V_grphrng = 2       '数字を小さくするとグラフが縦に広がる
                            //    '            V_grphrng = 4       '数字を小さくするとグラフが縦に広がる
                            //
                            //                Call DrawFittingCurve(FITTING_IMAGE(0), H_grphrng, V_grphrng, h_size, v_size, a0(iSCN), A1(iSCN), a2(iSCN), a3(iSCN), a4(iSCN), a5(iSCN), scancondpar.ver_wire_pitch, VRT_Count, xi(0), ti(0))    '★★ IICorrect.DLL
                            //
                            //                'フィッティングカーブ画像の表示 （ほんとのデバッグ用）
                            //                rc = IpAppCloseAll()                                                    '☆☆開いている全ての画像ｳｨﾝﾄﾞを閉じる
                            //                rc = IpWsCreate(h_size, v_size, 300, IMC_GRAY)                          '☆☆新規の画像ｳｨﾝﾄﾞを開く（8ビット用）
                            //                rc = IpDocPutArea(DOCSEL_ACTIVE, tmpReg, FITTING_IMAGE(0), CPROG)       '☆☆ユーザが作成した画像ﾃﾞｰﾀをのImage-Proの画像に書込む
                            //                rc = IpAppUpdateDoc(DOCSEL_ACTIVE)                                      '☆☆画像ウィンドの再描画
                            //
                            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
                            #endregion
                        }
                        //v11.2追加ここから by 間々田 2005/10/13
                        else
                        {
                            //マルチスライス校正中の場合
                            if (MultiSliceMode)
                            {
                                //hole_dataの読み込み
                                kmax = ReadHole();
                            }
                            //マルチスライス校正中ではない場合
                            else
                            {
                                pos_a = GVal_ScanPosiA[2];		    //added by 山本　2005-12-7

                                #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                                /*
                                //現在イメージプロでアクティブになっている画像（BVRT_IMAGE）の DocId を求める
                                rc = Ipc32v5.IpDocGet(Ipc32v5.GETACTDOC, 0, ref DocId1);

                                //追加 START by 山本　2005-11-28  ///////////////////////////////////
                                //カウントサイズの条件設定
                                rc = Ipc32v5.IpBlbShow(1);
                                rc = Ipc32v5.IpBlbSetFilterRange(Ipc32v5.BLBM_AREA, 10.0, 10000000.0);
                                rc = Ipc32v5.IpBlbSetAttr(Ipc32v5.BLOB_AUTORANGE, 1);		//自動抽出モード
                                rc = Ipc32v5.IpBlbSetAttr(Ipc32v5.BLOB_BRIGHTOBJ, 1);		//明るいオブジェクトを抽出

                                //カウント
                                rc = Ipc32v5.IpBlbCount();

                                //個数取得
                                rc = Ipc32v5.IpBlbGet(Ipc32v5.GETNUMOBJ, 0, 1, ref kmax);

                                //追加 END by 山本　2005-11-28  ///////////////////////////////////

                                if (kmax < 650)
                                {
                                    if (modCT30K.vm / modCT30K.hm > 1)
                                    {
                                        rc = Ipc32v5.IpFltDilate(Ipc32v5.MORPHO_3x1ROW, 3);
                                    }
                                    rc = Ipc32v5.IpFltDilate(Ipc32v5.MORPHO_5x5OCTAGON, 3);
                                }
                                else
                                {
                                    if (modCT30K.vm / modCT30K.hm > 1)
                                    {
                                        rc = Ipc32v5.IpFltDilate(Ipc32v5.MORPHO_3x1ROW, 3);
                                    }
                                    rc = Ipc32v5.IpFltDilate(Ipc32v5.MORPHO_5x5OCTAGON, 2);
                                }

                                //CVRT_IMAGEをイメージプロに書き込む
                                DocId2 = Ipc32v5.IpWsCreate(h_size, v_size, 300, Ipc32v5.IMC_GRAY16);				//空の画像ウィンドウを生成（Gray Scale 16形式）
                                rc = Ipc32v5.IpDocPutArea(DocId2, ref tmpReg, ref CVRT_IMAGE[0], Ipc32v5.CPROG);	//CVRT_IMAGEをイメージプロに書込む
                                rc = Ipc32v5.IpAppUpdateDoc(DocId2);							                    //画像ウィンドウの再描画

                                //幾何歪校正画像CVRT_IMAGEと２値化画像BVRT_IMAGEのAndをとる
                                rc = Ipc32v5.IpAppSelectDoc(DocId1);
                                rc = Ipc32v5.IpOpImageLogic(DocId2, Ipc32v5.OPL_AND, 0);

                                //イメージプロのカウントサイズ処理により幾何歪ファントムの穴を抽出し，
                                //   重心Ｘ座標（濃度）gx(i)
                                //   重心Ｙ座標（濃度）gy(i)
                                //   面積 area(i)
                                //   個数 kmax
                                //を求める
                                //追加 START by 山本　2005-10-31  ///////////////////////////////////
                                rc = Ipc32v5.IpBlbSetAttr(Ipc32v5.BLOB_AUTORANGE, 0);
                                rc = Ipc32v5.IpSegShow(1);
                                rc = Ipc32v5.IpSegSetAttr(Ipc32v5.SETCURSEL, 0);
                                rc = Ipc32v5.IpSegSetAttr(Ipc32v5.Channel, 0);
                                rc = Ipc32v5.IpSegPreview(Ipc32v5.CURRENT_C_T);
                                ipICal[0] = 1;
                                ipICal[1] = 65535;
                                rc = Ipc32v5.IpBlbMultiRanges(ref ipICal[0], 1);
                                rc = Ipc32v5.IpBlbSetFilterRange(Ipc32v5.BLBM_AREA, 0.0, 10000000.0);
                                rc = Ipc32v5.IpSegShow(0);
                                //  '追加 END by 山本　2005-10-31  ///////////////////////////////////

                                rc = Ipc32v5.IpBlbEnableMeas(Ipc32v5.BLBM_CMASSX, 1);
                                rc = Ipc32v5.IpBlbEnableMeas(Ipc32v5.BLBM_CMASSY, 1);
                                rc = Ipc32v5.IpBlbEnableMeas(Ipc32v5.BLBM_AREA, 1);

                                //カウント
                                rc = Ipc32v5.IpBlbCount();

                                //個数取得
                                rc = Ipc32v5.IpBlbGet(Ipc32v5.GETNUMOBJ, 0, 1, ref kmax);

                                //測定結果を受け取る配列を宣言
                                area = new float[kmax];
                                x = new float[kmax];
                                y = new float[kmax];
                                gArea = new double[kmax];
                                gx = new double[kmax];
                                gy = new double[kmax];
                                gg = new double[kmax];
                                gh = new double[kmax];

                                rc = Ipc32v5.IpBlbData(Ipc32v5.BLBM_AREA, 0, kmax - 1, ref area[0]);		//測定結果を取得
                                rc = Ipc32v5.IpBlbData(Ipc32v5.BLBM_CMASSX, 0, kmax - 1, ref x[0]);			//測定結果を取得
                                rc = Ipc32v5.IpBlbData(Ipc32v5.BLBM_CMASSY, 0, kmax - 1, ref y[0]);			//測定結果を取得

                                rc = Ipc32v5.IpBlbShow(0);			    //added by 山本　2005-10-31
                                */
                                //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                                int tmpKmax = 0;
                                //2014/11/13hata キャストの修正
                                //rc = CallImageProFunction.CallVerticalParameterGetHoles(CVRT_IMAGE, CTSettings.detectorParam.v_size, CTSettings.detectorParam.h_size, ref tmpKmax, (int)CTSettings.detectorParam.vm, (int)CTSettings.detectorParam.hm);
                                rc = CallImageProFunction.CallVerticalParameterGetHoles(CVRT_IMAGE, CTSettings.detectorParam.v_size, CTSettings.detectorParam.h_size, ref tmpKmax, CTSettings.detectorParam.vm, CTSettings.detectorParam.hm);
                                if (rc != 0) throw new Exception();
                                kmax = tmpKmax;
                                
                                //測定結果を受け取る配列を宣言
                                area = new float[kmax];
                                x = new float[kmax];
                                y = new float[kmax];
                                gArea = new double[kmax];
                                gx = new double[kmax];
                                gy = new double[kmax];
                                gg = new double[kmax];
                                gh = new double[kmax];                                
                                rc = CallImageProFunction.CallVerticalParameterStep5(x, y, area, kmax);
                                if (rc != 0) throw new Exception();                             
                                #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

                                //型を変換
                                for (i = 0; i < kmax; i++)
                                {
                                    gx[i] = (float)x[i];
                                    gy[i] = (float)y[i];
                                    gArea[i] = (float)area[i];
                                }

                                //２次元幾何歪パラメータ計算を行なう
                                if (calculate_fulldistortion_fitting(ref kmax, ref gx[0], ref gy[0], ref gArea[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size,
                                    Convert.ToDouble(CTSettings.scancondpar.Data.ver_wire_pitch), Convert.ToInt32(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm),
                                    ref CTSettings.scancondpar.Data.alk[0], ref CTSettings.scancondpar.Data.blk[0],
                                    ref CTSettings.scancondpar.Data.ist, ref CTSettings.scancondpar.Data.ied,
                                    ref CTSettings.scancondpar.Data.jst, ref CTSettings.scancondpar.Data.jed, ref gg[0], ref gh[0]) != 0)
                                {
                                    //マウスカーソルを元に戻す               '2006/01/30 追加 by 間々田
                                    Cursor.Current = Cursors.Default;

                                    //MsgBox "２次元幾何歪パラメータ計算に失敗しました。", vbCritical
                                    //v17.60 ストリングテーブル化 by長野 2011/05/25
                                    MessageBox.Show(CTResources.LoadResString(20116), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    return functionReturnValue;
                                }

                                //'                    For i = 0 To 35
                                //'                        Debug.Print "alk("; i; ") ="; scancondpar.alk(i)
                                //'                    Next

                                Array.Resize(ref gx, kmax);
                                Array.Resize(ref gy, kmax);
                                Array.Resize(ref gg, kmax);
                                Array.Resize(ref gh, kmax);

                                //端の点などの除去の確認       'Ver11.2追加 by 山本 2005-11-29
                                Print_CenterPoint(ref CVRT_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, kmax, ref gx[0], ref gy[0]);
                                
                                #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                                /*
                                DocId3 = Ipc32v5.IpWsCreate(h_size, v_size, 300, Ipc32v5.IMC_GRAY16);				//空の画像ウィンドウを生成（Gray Scale 16形式）
                                rc = Ipc32v5.IpDocPutArea(DocId3, ref tmpReg, ref CVRT_IMAGE[0], Ipc32v5.CPROG);    //CVRT_IMAGEをイメージプロに書込む
                                rc = Ipc32v5.IpAppUpdateDoc(DocId3);							                    //画像ウィンドウの再描画
                                */
                                //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
                                rc = CallImageProFunction.CallVerticalParameterStep6(CVRT_IMAGE, CTSettings.detectorParam.v_size, CTSettings.detectorParam.h_size);
                                if (rc != 0) throw new Exception();
                                #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//
                            }

                            //１次元幾何歪パラメータA0～A5とa0_bar, b0_barの算出

                            if (MultiSliceMode)
                            {
                                //ConeScanPosiB = CTSettings.scancondpar.Data.cone_scan_posi_b + (iSCN - 2) * (fFID / fFCD) * (fSLP / (fMDT * iVMG));
                                //Rev23.20 左右シフト対応 by長野 2015/11/19
                                ConeScanPosiB = modScanCondition.real_cone_scan_posi_b + (iSCN - 2) * (fFID / fFCD) * (fSLP / (fMDT * iVMG));
                            }
                            else
                            {
                                //ConeScanPosiB = CTSettings.scancondpar.Data.cone_scan_posi_b;
                                //Rev23.20 左右シフト対応 by長野 2015/11/19
                                ConeScanPosiB = modScanCondition.real_cone_scan_posi_b;
                            }

                            //2014/11/13hata キャストの修正
                            //a_0 = ConeScanPosiB - CTSettings.scancondpar.Data.cone_scan_posi_a * (CTSettings.detectorParam.h_size - 1) / 2 + (CTSettings.detectorParam.v_size - 1) / 2;
                            a_0 = ConeScanPosiB - CTSettings.scancondpar.Data.cone_scan_posi_a * (CTSettings.detectorParam.h_size - 1) / 2D + (CTSettings.detectorParam.v_size - 1) / 2D;

                            b_0 = CTSettings.scancondpar.Data.cone_scan_posi_a;
                            Cal_1d_Distortion_Parameter(Convert.ToInt32(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm), CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, a_0, b_0,
                                ref CTSettings.scancondpar.Data.alk[0], ref CTSettings.scancondpar.Data.blk[0], kmax,
                                ref gx[0], ref gy[0], ref gg[0], ref gh[0], ref a0_bar[iSCN], ref b0_bar[iSCN],
                                ref xls[iSCN], ref xle[iSCN], ref a0[iSCN], ref A1[iSCN], ref a2[iSCN], ref a3[iSCN], ref  a4[iSCN], ref a5[iSCN]);     //2005-10-28 yamaoka

                            GVal_ScanPosiA[iSCN] = Convert.ToSingle(b0_bar[iSCN]);
                            //2014/11/13hata キャストの修正
                            //GVal_ScanPosiB[iSCN] = (float)(a0_bar[iSCN] + b0_bar[iSCN] * (CTSettings.detectorParam.h_size - 1) / 2 - (CTSettings.detectorParam.v_size - 1) / 2);
                            GVal_ScanPosiB[iSCN] = (float)(a0_bar[iSCN] + b0_bar[iSCN] * (CTSettings.detectorParam.h_size - 1) / 2F - (CTSettings.detectorParam.v_size - 1) / 2F);
                        
                        }
                        //v11.2追加ここまで by 間々田 2005/10/13
                    }

                    #region			    //'■検出器ピッチを求める                                                'v11.2削除ここから by 間々田 2005/10/21
                    //
                    //'フラットパネルの場合   v7.0 added by 間々田 2003/09/25
                    //If Use_FlatPanel Then
                    //
                    //    'fpd_pitch = GetCommonFloat("scancondpar", "fpd_pitch")            'v11.2上の方へ移動 by 間々田 2005/10/21
                    //    'Mdt_Pitch(iSCN) = fpd_pitch
                    //    Mdt_Pitch(iSCN) = fpd_pitch * hm 'v7.0 change by 間々田 2003/10/21
                    //
                    //    '幾何歪パラメータ
                    //    ''A1(iSCN) = 10 / Fpd_Pitch
                    //    'A1(iSCN) = 10 / fpd_pitch / hm 'v7.0 change by 間々田 2003/10/21   'v11.2上の方へ移動 by 間々田 2005/10/21
                    //    '
                    //    'a0(iSCN) = 0                                                       'v11.2上の方へ移動 by 間々田 2005/10/21
                    //    'a2(iSCN) = 0                                                       'v11.2上の方へ移動 by 間々田 2005/10/21
                    //    'a3(iSCN) = 0                                                       'v11.2上の方へ移動 by 間々田 2005/10/21
                    //    'a4(iSCN) = 0                                                       'v11.2上の方へ移動 by 間々田 2005/10/21
                    //    'a5(iSCN) = 0                                                       'v11.2上の方へ移動 by 間々田 2005/10/21
                    //
                    //Else
                    //    Mdt_Pitch(iSCN) = 10 / A1(iSCN)
                    //End If
                    //
                    //GVal_mdtpitch(iSCN) = Mdt_Pitch(iSCN) 'added V2.0 by 鈴山              'v11.2削除ここまで by 間々田 2005/10/21
                    #endregion

                    //■検出器ピッチを求める（２次元幾何歪補正時は求めない）                   'v11.2追加ここから by 間々田 2005/10/21
                    Mdt_Pitch[iSCN] = 10 / A1[iSCN];
                    GVal_mdtpitch[iSCN] = (float)Mdt_Pitch[iSCN];   //added V2.0 by 鈴山          'v11.2追加ここまで by 間々田 2005/10/21

                    //入力された画像を幾何歪み補正する
                    //   テストとして垂直線画像を補正する。
                    //Call CorrectImage(VRT_IMAGE(0), CRT_IMAGE(0), H_SIZE, V_SIZE, A0, A1, A2, A3, A4, A5)   '★★ IICorrect.DLL

                    //■有効データ開始画素、終了画素を求める

                    //フラットパネルの場合   v7.0 added by 間々田 2003/09/25
                    if (CTSettings.detectorParam.Use_FlatPanel)
                    {
                        xls[iSCN] = 0;
                        //変更2014/10/07hata_v19.51反映
                        //xle[iSCN] = CTSettings.detectorParam.h_size - 1;
                        xle[iSCN] = CTSettings.detectorParam.h_size + sft_val - 1;       //v18.00変更 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

                    //Else
#region			    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''                        
                    //}
                    //else if (CTSettings.scaninh.Data.full_distortion != 0)    'v11.2変更 by 間々田 2005/10/13
                    //{
                    //    //有効画素範囲を求める
                    //    for (i = 0; i <= CTSettings.detectorParam.h_size - 1; i++)
                    //    {
                    //        xl = i - Conversion.Int(CTSettings.detectorParam.h_size / 2);
                    //        TT = (xl - a0[iSCN]) / A1[iSCN];
                    //        XE = a0[iSCN] + A1[iSCN] * TT + a2[iSCN] * Math.Pow(TT, 2) + a3[iSCN] * Math.Pow(TT, 3) + a4[iSCN] * Math.Pow(TT, 4) + a5[iSCN] * Math.Pow(TT, 5) + Conversion.Int(h_size / 2);

                    //        if (XE > 0 & XE < h_size & XE_B < XE)
                    //        {
                    //            if (DrawFlag == false)
                    //            {
                    //                xls[iSCN] = i + 1;
                    //            }
                    //            xle[iSCN] = i - 1;
                    //            DrawFlag = true;
                    //        }
                    //        else
                    //        {
                    //            if (DrawFlag == true)
                    //            {
                    //                xle[iSCN] = i - 1;
                    //            }
                    //            DrawFlag = false;
                    //        }
                    //        XE_B = XE;
                    //    }
                    //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion
                  
                    }

                    //端のデータを使用しないための処理　added by 山本　2005-12-13
                    if (xls[iSCN] < 4) xls[iSCN] = 4;
                    //変更2014/10/07hata_v19.51反映
                    //if (xle[iSCN] > CTSettings.detectorParam.h_size - 5) xle[iSCN] = CTSettings.detectorParam.h_size - 5;
                    if (xle[iSCN] > CTSettings.detectorParam.h_size + sft_val - 5) xle[iSCN] = CTSettings.detectorParam.h_size + sft_val - 5;   //v18.00変更 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

                    //追加ここから by 間々田 2004/04/26

                    //幾何歪テーブル読み込み
                    //If Read_HizumiTable(hizumi()) < 0 Then GoTo Err_Process        '削除 by 間々田 2004/06/03 ここで歪テーブルは不要

                    h = CTSettings.detectorParam.h_size;
                    v = CTSettings.detectorParam.v_size;
                    kv = CTSettings.detectorParam.vm / CTSettings.detectorParam.hm;
                    bb0 = GVal_ScanPosiA[2];
                    FGD = GVal_Fid + CTSettings.scancondpar.Data.fid_offset[modCT30K.GetFcdOffsetIndex()];

                    //最大ファン角の計算
                    //ic = (h - 1) / 2; //変更2014/10/07hata_v19.51反映
                    //2014/11/13hata キャストの修正
                    //ic = (h - 1) / 2 + sft_val; //v18.00変更 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    //jc = (v - 1) / 2;
                    ic = (float)(h - 1) / 2F + sft_val; //v18.00変更 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    jc = (float)(v - 1) / 2F;
                    
                    Dpi = (float)(10 / A1[iSCN]);
                    e_dash = (float)Math.Atan(kv * bb0);
                    r = (float)Math.Sqrt(Math.Pow(ic, 2) + Math.Pow((kv * jc), 2));

                    //2014/11/13hata キャストの修正
                    //mm = (int)(2 * r);
                    mm = Convert.ToInt32(Math.Floor(2 * r));

                    //Delta_Ip = Int(ic * hizumi(mm) + 2 + jc * kv * kv * Abs(bb0))
                    //2014/11/13hata キャストの修正
                    //Delta_Ip = (int)(2 + jc * kv * kv * Math.Abs(bb0));	    //変更 by 間々田 2004/06/03 ここで歪テーブルは不要
                    Delta_Ip = Convert.ToInt32(Math.Floor(2 + jc * kv * kv * Math.Abs(bb0)));	    //変更 by 間々田 2004/06/03 ここで歪テーブルは不要

                    Ils = Delta_Ip;
                    //Ile = h - Delta_Ip - 1;   //変更2014/10/07hata_v19.51反映
                    Ile = h + sft_val - Delta_Ip - 1;   //v18.00変更 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
				
                    theta0Max[iSCN] = (float)(2 * Math.Atan((Ile - Ils) * Dpi / Math.Cos(e_dash) * 1.02 * 0.5 / FGD));

                    //追加ここまで by 間々田 2004/04/26
                }		    //added V2.0 by 鈴山

                //コーンビームスキャンが可能な場合
                if (CTSettings.scaninh.Data.data_mode[2] == 0)
                {
                    //■垂直線ワイヤ細線化画像と画像の中心線との交点座標(Cross_X,Cross_Y)と交点数(VRT_Count)を求める

                    //フラットパネルの場合は「垂直線本数の初期化」～「幾何歪校正用フィッティング計算」を実行しない   v7.0 added by 間々田 2003/09/25
                    if (CTSettings.detectorParam.Use_FlatPanel)
                    {
                        //            B1 = 10 / Fpd_Pitch
                        B1 = 10 / CTSettings.scancondpar.Data.fpd_pitch / CTSettings.detectorParam.hm;	    //v7.0 change by 間々田 2003/10/21
                        b0 = 0;
                        B2 = 0;
                        B3 = 0;
                        B4 = 0;
                        B5 = 0;

                        //v17.00追加　山本 2009-10-06
                        for (i = 0; i <= 35; i++)
                        {
                            if (i == 1)
                            {
                                CTSettings.scancondpar.Data.alk[i] = B1;
                            }
                            else
                            {
                                CTSettings.scancondpar.Data.alk[i] = 0;
                            }

                            if (i == 6)
                            {
                                CTSettings.scancondpar.Data.blk[i] = B1;
                            }
                            else
                            {
                                CTSettings.scancondpar.Data.blk[i] = 0;
                            }
                        }
                        CTSettings.scancondpar.Data.ist = 0;
                        //CTSettings.scancondpar.Data.ied = CTSettings.detectorParam.h_size - 1;    //変更2014/10/07hata_v19.51反映
                        CTSettings.scancondpar.Data.ied = CTSettings.detectorParam.h_size + sft_val - 1;    //v18.00変更 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                        CTSettings.scancondpar.Data.jst = 0;
                        CTSettings.scancondpar.Data.jed = CTSettings.detectorParam.v_size - 1;
                    }
                    else
                    {
                        //ElseIf scaninh.full_distortion <> 0 Then            'v11.2変更 by 間々田 2005/10/06
                        #region				    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                        //            If scaninh.full_distortion <> 0 Then    'v17.10 if変更 byやまおか 2010/07/28
                        //
                        //                '垂直線本数の初期化
                        //                VRT_Count = 0
                        //                VRT_Cnt_Flag = False    'added by 山本 2002-4-11
                        //                Sigma_F = 0             'added by 山本 2002-4-11
                        //                Sigma_FX = 0            'added by 山本 2002-4-11
                        //
                        //                '水平線の傾きと切片を決める
                        //                pos_a = 0
                        //                pos_b = 0
                        //
                        //                '垂直線と水平線の交点を求める
                        //                'For j = 10 To H_SIZE - 1 - 10
                        //                For j = 20 To h_size - 1 - 20   'added by 山本 2002-1-29  左右両端の余裕を10画素から20画素に変更
                        //
                        //                    'いったん画素の縦方向の座標をcに代入する（整数化するため）
                        //                    c = Int(pos_a * j + pos_b + Int(v_size / 2) - pos_a * Int(h_size / 2)) 'xi,ti座標系からi,j座標系に戻す
                        //                    c = c * h_size + j
                        //
                        //        '/////////細線化法→重心法に変更(4/5) deleted by 稲葉 2002/01/23 START //////////////
                        //        ''''            If VRT_THIN_IMAGE(C) <> 0 Then
                        //        ''''                Cross_X(VRT_Count) = j
                        //        ''''                Cross_Y(VRT_Count) = pos_a * j + pos_b + Int(V_SIZE / 2) - pos_a * Int(H_SIZE / 2)
                        //        ''''                VRT_Count = VRT_Count + 1
                        //        ''''
                        //        ''''            End If
                        //        '/////////細線化法→重心法に変更(4/5) deleted by 稲葉 2002/01/23 END //////////////
                        //
                        //        '/////////細線化法→重心法に変更(5/5) added by 稲葉 2002/01/23 START //////////////
                        //                    '垂直線の重心を交点座標とする change by 稲葉 2002/01/23
                        //                    If BVRT_IMAGE(c) <> 0 Then
                        //                        Sigma_F = Sigma_F + VRT_IMAGE(c)
                        //                        Sigma_FX = Sigma_FX + VRT_IMAGE(c) * j
                        //                        VRT_Cnt_Flag = True
                        //                    ElseIf VRT_Cnt_Flag Then
                        //                        Cross_X(VRT_Count) = Sigma_FX / Sigma_F
                        //                        Cross_Y(VRT_Count) = pos_a * Cross_X(VRT_Count) + pos_b + Int(v_size / 2) - pos_a * Int(h_size / 2)
                        //                        VRT_Count = VRT_Count + 1
                        //                        Sigma_F = 0
                        //                        Sigma_FX = 0
                        //                        VRT_Cnt_Flag = False
                        //                    End If
                        //        '/////////細線化法→重心法に変更(5/5) added by 稲葉 2002/01/23 END //////////////
                        //
                        //                Next
                        //
                        //#If DebugOn Then
                        //#Else
                        //                '■フィッティング補正カーブ係数(B0～B5)をフィッティング計算により求める
                        //                If Not Caliculate_Fitting(Cross_X, Cross_Y, ti, xi, b0, B1, B2, B3, B4, B5, pos_a, pos_b, scancondpar.ver_wire_pitch, VRT_Count, Matrix) Then GoTo Err_Process
                        //#End If
                        //
                        //            End If  'v17.10 if追加 byやまおか 2010/07/28
                        //
                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
                        #endregion

                    }

                    #region			    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                    //        '■コーンビーム用の幾何歪テーブルの作成（２次元幾何歪の場合は作成しない）
                    //        If scaninh.full_distortion <> 0 Then                                                        'v11.2 if文追加 by 間々田 2005/10/06
                    //            Make_HizumiTable b0, B1, B2, B3, B4, B5, h_size, v_size, hizumi()
                    //        End If
                    //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
                    #endregion

                    //■コーンビーム用の幾何歪パラメータ計算
                    //Cal_ConeBeamParameter B1, pos_a, dpm
                    Cal_ConeBeamParameter((CTSettings.scaninh.Data.full_distortion == 0 ? CTSettings.scancondpar.Data.alk[1] : B1), pos_a, ref dpm);	    //v11.2変更 by 間々田 2005/10/06

                    //追加ここから by 間々田 2004/04/26

                    //幾何歪テーブル読み込み
                    //If Read_HizumiTable(hizumi()) < 0 Then GoTo Err_Process    'ここで歪テーブルをファイルから読み込むとＮＧ。削除 by 間々田 2004/06/03

                    h = CTSettings.detectorParam.h_size;
                    v = CTSettings.detectorParam.v_size;
                    kv = CTSettings.detectorParam.vm / CTSettings.detectorParam.hm;
                    bb0 = GVal_ScanPosiA[2];
                    FGD = GVal_Fid + CTSettings.scancondpar.Data.fid_offset[modCT30K.GetFcdOffsetIndex()];

                    //最大ファン角の計算
                    //ic = (h - 1) / 2; //変更2014/10/07hata_v19.51反映
                    //2014/11/13hata キャストの修正
                    //ic = (h - 1) / 2 + sft_val; //v18.00変更 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    //jc = (v - 1) / 2;
                    ic = (h - 1) / 2F + sft_val; //v18.00変更 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    jc = (v - 1) / 2F;

                    Dpi = (float)(10 / B1);
                    //ノーマルスキャン時は B1 の代わりに A1(iSCN)を使用する
                    e_dash = (float)Math.Atan(kv * bb0);

                    //２次元幾何歪の場合                                     'v11.2追加ここから by 間々田 2005/10/06
                    if (CTSettings.scaninh.Data.full_distortion == 0)
                    {
                        //Ils = scancondpar.ist + 2
                        //Ile = scancondpar.ied - 2
                        //v19.12 FPDの場合は±8になる by長野 2013/03/12
                        if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))
                        {
                            Ils = CTSettings.scancondpar.Data.ist + 8;
                            Ile = CTSettings.scancondpar.Data.ied - 8;
                        }
                        else
                        {
                            Ils = CTSettings.scancondpar.Data.ist + 2;
                            Ile = CTSettings.scancondpar.Data.ied - 2;
                        }

                        #region					//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                        //        Else                                                    'v11.2追加ここまで by 間々田 2005/10/06


                        //            r = Sqr(ic ^ 2 + (kv * jc) ^ 2)
                        //            mm = Int(2 * r)
                        //
                        //            Delta_Ip = Int(ic * hizumi(mm) + 2 + jc * kv * kv * Abs(bb0))
                        //            Ils = Delta_Ip
                        //            Ile = h - Delta_Ip - 1
                        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
                        #endregion

                    }
                    //v11.2追加 by 間々田 2005/10/06

                    theta0MaxCone = (float)(2 * Math.Atan((Ile - Ils) * Dpi / Math.Cos(e_dash) * 1.02 * 0.5 / FGD));
                    //追加ここまで by 間々田 2004/04/26
                }

                functionReturnValue = 0;

                //マウスカーソルを元に戻す   'added by 山本　2002-1-7
                Cursor.Current = Cursors.Default;

                return functionReturnValue;           
            }
            catch (Exception)
            {
                //v9.7追加 メッセージ表示ありの場合 by 間々田 2004/11/24
                if (messageOn)
                {
                    //メッセージ表示：
                    //   幾何歪校正パラメータ計算に失敗しました。
                    //   幾何歪校正を再度行ってください。
                    MessageBox.Show((StringTable.BuildResStr(StringTable.IDS_ErrCalPara, StringTable.IDS_CorDistortion) + "\r" 
                                                            + StringTable.BuildResStr(StringTable.IDS_Retry, StringTable.IDS_CorDistortion)), 
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            //マウスカーソルを元に戻す
            Cursor.Current = Cursors.Default;
            return functionReturnValue; 
        }


        //********************************************************************************
        //機    能  ：  ｺｰﾝﾋﾞｰﾑ用幾何歪ﾃｰﾌﾞﾙの作成
        //              変数名           [I/O] 型        内容
        //引    数  ：  b0               [I/ ] Double    幾何歪ﾌｨｯﾃｨﾝｸﾞ係数 B0
        //              b1               [I/ ] Double    幾何歪ﾌｨｯﾃｨﾝｸﾞ係数 B1
        //              b2               [I/ ] Double    幾何歪ﾌｨｯﾃｨﾝｸﾞ係数 B2
        //              b3               [I/ ] Double    幾何歪ﾌｨｯﾃｨﾝｸﾞ係数 B3
        //              b4               [I/ ] Double    幾何歪ﾌｨｯﾃｨﾝｸﾞ係数 B4
        //              b5               [I/ ] Double    幾何歪ﾌｨｯﾃｨﾝｸﾞ係数 B5
        //              h_siz            [I/ ] Long      透視画像横ｻｲｽﾞ
        //              v_siz            [I/ ] Long      透視画像縦ｻｲｽﾞ
        //              hizumi()         [ /O] Single    幾何歪ﾃｰﾌﾞﾙ
        //戻 り 値  ：                   [ /O] Integer   結果(0:成功,-1:失敗)
        //補    足  ：  計算に必要なﾊﾟﾗﾒｰﾀと計算結果はすべて引数でやりとりする。
        //
        //履    歴  ：  V3.0   00/08/11  (SI1)鈴山       新規作成
        //              v9.7   04/11/25  (SI4)間々田     引数 v_mag 削除
        //********************************************************************************
        //Private Sub Make_HizumiTable(ByVal B0 As Double, _
        //'                             ByVal B1 As Double, _
        //'                             ByVal B2 As Double, _
        //'                             ByVal B3 As Double, _
        //'                             ByVal B4 As Double, _
        //'                             ByVal B5 As Double, _
        //'                             ByVal v_mag As Long, _
        //'                             ByVal h_siz As Long, _
        //'                             ByVal v_siz As Long, _
        //'                             ByRef hizumi() As Single)
		private static void Make_HizumiTable(double b0, double B1, double B2, double B3, double B4, double B5, int h_siz, int v_siz, ref float[] hizumi)
		{

			//パラメータ
			double bm = 0;			//
			double dm = 0;			//
			double fm = 0;			//
			int ic = 0;			    //
			int jc = 0;			    //
			int rmax = 0;			//
			int Nr = 0;			    //幾何歪ﾃｰﾌﾞﾙのﾚｺｰﾄﾞ数?
			double rL = 0;			//
			double delta_t = 0;		//
			double delta_tsq = 0;	//
			double re = 0;			//
			int N = 0;			    //ﾙｰﾌﾟｶｳﾝﾀ
			int kv = 0;			    //added by 山本 2002-6-18

			//    If h_size = 640 Then    'added by 山本 2002-6-18
			//        kv = 1
			//    Else
			//        kv = 2
			//    End If
            //2014/11/13hata キャストの修正
            //kv = (int)(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);			//v7.0 change by 間々田 2003/09/25
            kv = Convert.ToInt32(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);			//v7.0 change by 間々田 2003/09/25

			//計算
			//    ReDim hizumi(2000)
			hizumi = new float[3501];		//changed by 山本　2003-10-17　2400画素のFPD対応

			//v7.0 FPD対応 by 間々田 2003/09/25
            if (CTSettings.detectorParam.Use_FlatPanel) 
            {
				//        For N = 0 To 2000
				//changed by 山本　2003-10-17　2400画素のFPD対応
				for (N = 0; N <= 3500; N++) 
                {
					hizumi[N] = 0;
				}
			} 
            else 
            {
				bm = B1 + 2 * B2 + 3 * B3 + 4 * B4 + 5 * B5;
				dm = B3 + 4 * B4 + 10 * B5;
				fm = B5;
                //2014/11/13hata キャストの修正
                //ic = (h_siz - 1) / 2;
                //jc = (v_siz - 1) / 2;
                ic = Convert.ToInt32((h_siz - 1) / 2F);
                jc = Convert.ToInt32((v_siz - 1) / 2F);

				//rmax = Sqr(ic ^ 2 +  jc ^ 2)
                //2014/11/13hata キャストの修正
                //rmax = (int)Math.Sqrt(Math.Pow(ic, 2) + Math.Pow((kv * jc), 2));		//changed by 山本　2002-6-18
                rmax = Convert.ToInt32(Math.Sqrt(Math.Pow(ic, 2) + Math.Pow((kv * jc), 2)));		//changed by 山本　2002-6-18
                Nr = Convert.ToInt32(2 * rmax) + 10;
				rL = -0.25;

				//ループ
				for (N = 0; N <= Nr - 1; N++) 
                {
					rL = rL + 0.5;
					delta_t = rL / B1;
					delta_tsq = Math.Pow(delta_t, 2);
					re = ((fm * delta_tsq + dm) * delta_tsq + bm) * delta_t;
					hizumi[N] = (float)((re - rL) / rL);
					//            If (N = 2000) Then Exit For
                    if ((N == 3500)) break;		//changed by 山本　2003-10-17　2400画素のFPD対応
				}
			}
		}


        //********************************************************************************
        //機    能  ：  コモンファイルの SCANSEL から各値を読みとる（校正処理に必要なものだけ）
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  frmScanConditionのローカルモジュールから流用
        //
        //履    歴  ：  V1.00    99/XX/XX    ??????????????  新規作成
        //              V2.0     00/02/08    (SI1)鈴山       コモンを追加・削除
        //              v9.0     04/02/12    (SI4)間々田     未使用変数は削除
        //********************************************************************************
		public static void OptValueGet_Cor()
		{
			//ﾋﾞｭｰ数設定値
            //VIEW_N = modLibrary.CorrectInRangeLong(CTSettings.scansel.Data.scan_view, modGlobal.GVal_ViewMin, modGlobal.GVal_ViewMax);
            VIEW_N = modLibrary.CorrectInRange(CTSettings.scansel.Data.scan_view, CTSettings.GVal_ViewMin, CTSettings.GVal_ViewMax);

			//スキャンエリア最大
            GVal_ScnAreaMax[0] = CTSettings.scansel.Data.max_scan_area[0];
            GVal_ScnAreaMax[1] = CTSettings.scansel.Data.max_scan_area[1];
            GVal_ScnAreaMax[2] = CTSettings.scansel.Data.max_scan_area[2];
            GVal_ScnAreaMax[3] = CTSettings.scansel.Data.max_scan_area[3];   //v18.00追加 byやまおか 2011/02/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07 //追加2014/10/07hata_v19.51反映

			//スキャンエリア設定値
            GVal_MScnArea = modLibrary.MinVal(CTSettings.scansel.Data.mscan_area, GVal_ScnAreaMax[2]);

			//scancondpar（コモン）取得
			//modScancondpar.CallGetScancondpar();
            CTSettings.scancondpar.Load(CTSettings.scaninh.Data.rotate_select);

            //'シフトスキャンのとき   'v18.00追加 byやまおか 2011/07/14 'v19.50 v19.41とv18.02の統合のため下に移動 by長野 2013/11/07
            //int sft_val = (IsShiftScan() ? CTSettings.scancondpar.Data.det_sft_pix : 0);
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/08/08
            int sft_val = ((IsShiftScan() || IsW_Scan()) ? CTSettings.scancondpar.Data.det_sft_pix : 0);

            //X線管

//#region		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
//            //        '回転選択が可能な場合 v9.0 added by 間々田 2004/02/13
//            //        If scaninh.rotate_select = 0 Then
//            //            GFlg_MultiTube = scansel.rotate_select
//            //        'X線管選択可の場合
//            //        ElseIf scaninh.multi_tube = 0 Then
//            //            GFlg_MultiTube = scansel.multi_tube
//            //        Else
//            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
//#endregion
            //Rev23.10 X線切替復活のためコメントアウト by長野 2015/10/18
            //GFlg_MultiTube = 0;
            //        '回転選択が可能な場合 v9.0 added by 間々田 2004/02/13
            //        If scaninh.rotate_select = 0 Then
            //            GFlg_MultiTube = scansel.rotate_select
            //        'X線管選択可の場合
            //        ElseIf scaninh.multi_tube = 0 Then
            //            GFlg_MultiTube = scansel.multi_tube
            //        Else
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            //Rev23.10 X線切替復活 by長野 2015/10/18
            if (CTSettings.scaninh.Data.multi_tube == 0)
            {
                GFlg_MultiTube = CTSettings.scansel.Data.multi_tube;
            }
            else
            {
                GFlg_MultiTube = 0;
            }

            //Rev23.10 コメントアウト by長野 2015/10/18
//#region     //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
//            //        End If
//            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
//#endregion
            
            //FID/FCDの取得 by 間々田 2005/02/14
			//Call GetFIDFCD(GVal_Fid, GVal_Fcd) 'v15.0削除 by 間々田 2009/04/10

			//検出器ピッチ             'changed V2.0 by 鈴山 (複数ｽﾗｲｽ用に配列化)
            GVal_mdtpitch[0] = CTSettings.scancondpar.Data.mdtpitch[0];
            GVal_mdtpitch[1] = CTSettings.scancondpar.Data.mdtpitch[1];
            GVal_mdtpitch[2] = CTSettings.scancondpar.Data.mdtpitch[2];
            GVal_mdtpitch[3] = CTSettings.scancondpar.Data.mdtpitch[3];
            GVal_mdtpitch[4] = CTSettings.scancondpar.Data.mdtpitch[4];

			//透視画像サイズ(横)
            GVal_ImgHsize = CTSettings.scancondpar.Data.fimage_hsize;
            if (GVal_ImgHsize <= 1) GVal_ImgHsize = 640;
            CTSettings.detectorParam.h_size = GVal_ImgHsize;

			//透視画像サイズ(縦)
            GVal_ImgVsize = CTSettings.scancondpar.Data.fimage_vsize;
            if (GVal_ImgVsize <= 1) GVal_ImgVsize = 480;
            CTSettings.detectorParam.v_size = GVal_ImgVsize;

			//幾何歪校正係数(A0～A5)
            //a0[0] = CTSettings.scancondpar.Data.a(0, 0);			//a00のこと
            //a0[1] = CTSettings.scancondpar.Data.a(0, 1);			//a10のこと
            //a0[2] = CTSettings.scancondpar.Data.a(0, 2);			//a20のこと
            //a0[3] = CTSettings.scancondpar.Data.a(0, 3);			//a30のこと
            //a0[4] = CTSettings.scancondpar.Data.a(0, 4);			//a40のこと
            a0[0] = CTSettings.scancondpar.Data.a[0 * 6 + 0];		//a[0][0]のこと
            a0[1] = CTSettings.scancondpar.Data.a[1 * 6 + 0];		//a[1][0]のこと
            a0[2] = CTSettings.scancondpar.Data.a[2 * 6 + 0];		//a[2][0]のこと
            a0[3] = CTSettings.scancondpar.Data.a[3 * 6 + 0];		//a[3][0]のこと
            a0[4] = CTSettings.scancondpar.Data.a[4 * 6 + 0];		//a[4][0]のこと

            //A1[0] = CTSettings.scancondpar.Data.a(1, 0);			//a01のこと
            //A1[1] = CTSettings.scancondpar.Data.a(1, 1);			//a11のこと
            //A1[2] = CTSettings.scancondpar.Data.a(1, 2);			//a21のこと
            //A1[3] = CTSettings.scancondpar.Data.a(1, 3);			//a31のこと
            //A1[4] = CTSettings.scancondpar.Data.a(1, 4);			//a41のこと
            A1[0] = CTSettings.scancondpar.Data.a[0 * 6 + 1];			//a[0][1]のこと
            A1[1] = CTSettings.scancondpar.Data.a[1 * 6 + 1];			//a[1][1]のこと
            A1[2] = CTSettings.scancondpar.Data.a[2 * 6 + 1];			//a[2][1]のこと
            A1[3] = CTSettings.scancondpar.Data.a[3 * 6 + 1];			//a[3][1]のこと
            A1[4] = CTSettings.scancondpar.Data.a[4 * 6 + 1];			//a[4][1]のこと
            
            if (A1[0] == 0) A1[0] = 1;
            if (A1[1] == 0) A1[1] = 1;
            if (A1[2] == 0) A1[2] = 1;
            if (A1[3] == 0) A1[3] = 1;
            if (A1[4] == 0) A1[4] = 1;

            //a2[0] = CTSettings.scancondpar.Data.a(2, 0);			//a02のこと
            //a2[1] = CTSettings.scancondpar.Data.a(2, 1);			//a12のこと
            //a2[2] = CTSettings.scancondpar.Data.a(2, 2);			//a22のこと
            //a2[3] = CTSettings.scancondpar.Data.a(2, 3);			//a32のこと
            //a2[4] = CTSettings.scancondpar.Data.a(2, 4);			//a42のこと
            a2[0] = CTSettings.scancondpar.Data.a[0 * 6 + 2];		//a[0][2]のこと
            a2[1] = CTSettings.scancondpar.Data.a[1 * 6 + 2];		//a[1][2]のこと
            a2[2] = CTSettings.scancondpar.Data.a[2 * 6 + 2];		//a[2][2]のこと
            a2[3] = CTSettings.scancondpar.Data.a[3 * 6 + 2];		//a[3][2]のこと
            a2[4] = CTSettings.scancondpar.Data.a[4 * 6 + 2];		//a[4][2]のこと

            //a3[0] = CTSettings.scancondpar.Data.a(3, 0);			//a03のこと
            //a3[1] = CTSettings.scancondpar.Data.a(3, 1);			//a13のこと
            //a3[2] = CTSettings.scancondpar.Data.a(3, 2);			//a23のこと
            //a3[3] = CTSettings.scancondpar.Data.a(3, 3);			//a33のこと
            //a3[4] = CTSettings.scancondpar.Data.a(3, 4);			//a43のこと
            a3[0] = CTSettings.scancondpar.Data.a[0 * 6 + 3];		//a[0][3]のこと
            a3[1] = CTSettings.scancondpar.Data.a[1 * 6 + 3];		//a[1][3]のこと
            a3[2] = CTSettings.scancondpar.Data.a[2 * 6 + 3];		//a[2][3]のこと
            a3[3] = CTSettings.scancondpar.Data.a[3 * 6 + 3];		//a[3][3]のこと
            a3[4] = CTSettings.scancondpar.Data.a[4 * 6 + 3];		//a[4][3]のこと
          
            //a4[0] = CTSettings.scancondpar.Data.a(4, 0);			//a04のこと
            //a4[1] = CTSettings.scancondpar.Data.a(4, 1);			//a14のこと
            //a4[2] = CTSettings.scancondpar.Data.a(4, 2);			//a24のこと
            //a4[3] = CTSettings.scancondpar.Data.a(4, 3);			//a34のこと
            //a4[4] = CTSettings.scancondpar.Data.a(4, 4);			//a44のこと
            a4[0] = CTSettings.scancondpar.Data.a[0 * 6 + 4];		//a[0][4]のこと
            a4[1] = CTSettings.scancondpar.Data.a[1 * 6 + 4];		//a[1][4]のこと
            a4[2] = CTSettings.scancondpar.Data.a[2 * 6 + 4];		//a[2][4]のこと
            a4[3] = CTSettings.scancondpar.Data.a[3 * 6 + 4];		//a[3][4]のこと
            a4[4] = CTSettings.scancondpar.Data.a[4 * 6 + 4];		//a[4][4]のこと
           
            //a5[0] = CTSettings.scancondpar.Data.a(5, 0);			//a05のこと
            //a5[1] = CTSettings.scancondpar.Data.a(5, 1);			//a15のこと
            //a5[2] = CTSettings.scancondpar.Data.a(5, 2);			//a25のこと
            //a5[3] = CTSettings.scancondpar.Data.a(5, 3);			//a35のこと
            //a5[4] = CTSettings.scancondpar.Data.a(5, 4);			//a45のこと
            a5[0] = CTSettings.scancondpar.Data.a[0 * 6 + 5];		//a[0][5]のこと
            a5[1] = CTSettings.scancondpar.Data.a[1 * 6 + 5];		//a[1][5]のこと
            a5[2] = CTSettings.scancondpar.Data.a[2 * 6 + 5];		//a[2][5]のこと
            a5[3] = CTSettings.scancondpar.Data.a[3 * 6 + 5];		//a[3][5]のこと
            a5[4] = CTSettings.scancondpar.Data.a[4 * 6 + 5];		//a[4][5]のこと

			//有効開始画素
            GVal_Xls[0] = CTSettings.scancondpar.Data.xls[0];
            GVal_Xls[1] = CTSettings.scancondpar.Data.xls[1];
            GVal_Xls[2] = CTSettings.scancondpar.Data.xls[2];
            GVal_Xls[3] = CTSettings.scancondpar.Data.xls[3];
            GVal_Xls[4] = CTSettings.scancondpar.Data.xls[4];

            //有効開始画素   'v18.00追加 byやまおか 2011/07/14 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07     //追加2014/10/07hata_v19.51反映
            GVal_Xls_sft[0] = CTSettings.scancondpar.Data.xls[0];
            GVal_Xls_sft[1] = CTSettings.scancondpar.Data.xls[1];
            GVal_Xls_sft[2] = CTSettings.scancondpar.Data.xls[2];
            GVal_Xls_sft[3] = CTSettings.scancondpar.Data.xls[3];
            GVal_Xls_sft[4] = CTSettings.scancondpar.Data.xls[4];

            //有効終了画素    'v18.00変更 byやまおか 2011/07/14'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
			//    構造体名：scancondpar
			//    コモン名：xle[5]
            //GVal_Xle[0] = CTSettings.scancondpar.Data.xle[0];
            //GVal_Xle[1] = CTSettings.scancondpar.Data.xle[1];
            //GVal_Xle[2] = CTSettings.scancondpar.Data.xle[2];
            //GVal_Xle[3] = CTSettings.scancondpar.Data.xle[3];
            //GVal_Xle[4] = CTSettings.scancondpar.Data.xle[4];
            GVal_Xle[0] = CTSettings.scancondpar.Data.xle[0] - sft_val; //変更2014/10/07hata_v19.51反映
            GVal_Xle[1] = CTSettings.scancondpar.Data.xle[1] - sft_val; //変更2014/10/07hata_v19.51反映
            GVal_Xle[2] = CTSettings.scancondpar.Data.xle[2] - sft_val; //変更2014/10/07hata_v19.51反映
            GVal_Xle[3] = CTSettings.scancondpar.Data.xle[3] - sft_val; //変更2014/10/07hata_v19.51反映
            GVal_Xle[4] = CTSettings.scancondpar.Data.xle[4] - sft_val; //変更2014/10/07hata_v19.51反映

            //有効終了画素   'v18.00追加 byやまおか 2011/07/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            GVal_Xle_sft[0] = CTSettings.scancondpar.Data.xle[0];
            GVal_Xle_sft[1] = CTSettings.scancondpar.Data.xle[1];
            GVal_Xle_sft[2] = CTSettings.scancondpar.Data.xle[2];
            GVal_Xle_sft[3] = CTSettings.scancondpar.Data.xle[3];
            GVal_Xle_sft[4] = CTSettings.scancondpar.Data.xle[4];

			//傾き
            GVal_ScanPosiA[0] = CTSettings.scancondpar.Data.scan_posi_a[0];
            GVal_ScanPosiA[1] = CTSettings.scancondpar.Data.scan_posi_a[1];
            GVal_ScanPosiA[2] = CTSettings.scancondpar.Data.scan_posi_a[2];
            GVal_ScanPosiA[3] = CTSettings.scancondpar.Data.scan_posi_a[3];
            GVal_ScanPosiA[4] = CTSettings.scancondpar.Data.scan_posi_a[4];

			//切片
			GVal_ScanPosiB[0] = CTSettings.scancondpar.Data.scan_posi_b[0];
			GVal_ScanPosiB[1] = CTSettings.scancondpar.Data.scan_posi_b[1];
			GVal_ScanPosiB[2] = CTSettings.scancondpar.Data.scan_posi_b[2];
			GVal_ScanPosiB[3] = CTSettings.scancondpar.Data.scan_posi_b[3];
			GVal_ScanPosiB[4] = CTSettings.scancondpar.Data.scan_posi_b[4];

			//コーンビーム用幾何歪補正係数(B0～B5)
			b0 = CTSettings.scancondpar.Data.b[0];
			B1 = CTSettings.scancondpar.Data.b[1];
			B2 = CTSettings.scancondpar.Data.b[2];
			B3 = CTSettings.scancondpar.Data.b[3];
			B4 = CTSettings.scancondpar.Data.b[4];
			B5 = CTSettings.scancondpar.Data.b[5];
		}


        //********************************************************************************
        //機    能  ：  ｺｰﾝﾋﾞｰﾑ用幾何歪ﾃｰﾌﾞﾙの読み込み
        //              変数名           [I/O] 型        内容
        //引    数  ：  hizumi()         [ /O] Single    幾何歪ﾃｰﾌﾞﾙ
        //戻 り 値  ：                   [ /O] Boolean   結果(True:成功, False:失敗)
        //補    足  ：  計算に必要なﾊﾟﾗﾒｰﾀと計算結果はすべて引数でやりとりする。
        //
        //履    歴  ：  V3.0   00/08/22  (SI1)鈴山       新規作成
        //********************************************************************************
        public static bool Read_HizumiTable(ref float[] hizumi)
		{
			int i = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
            short fileNo = 0;
*/
#endregion
            
            string strWork = null;

			//戻り値初期化
            bool functionReturnValue = false;

			//配列サイズ
			//    ReDim hizumi(2000)
			hizumi = new float[3501];			//changed by 山本　2003-10-17　2400画素のFPD対応
            //TODO 配列サイズ変更
			//ファイルの有無をチェック：ファイルがなければ中止
            //if (!modFileIO.FSO.FileExists(HIZUMI_CSV)) return functionReturnValue;
            if (!File.Exists(HIZUMI_CSV)) return functionReturnValue;

           StreamReader sr = null;

            try
            {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
                //幾何歪テーブルをオープン
                fileNo = FreeFile();
                FileSystem.FileOpen(fileNo, HIZUMI_CSV, OpenMode.Input);
*/
#endregion
                
                //幾何歪テーブルをオープン
                sr = new StreamReader(HIZUMI_CSV);
                
                i = 0;
			    //while (sr.Read(strWork, i, 1) != null) 
                while ((strWork = sr.ReadLine()) != null) 
                {
				    //        If (i > 2000) Then Exit Do
                    if ((i > 3500)) break;				//changed by 山本　2003-10-17　2400画素のFPD対応
				    hizumi[i] = Convert.ToSingle(strWork);
				    i = i + 1;
			    }

                //戻り値セット
                functionReturnValue = true;
            }
            catch
            {
                // Nothing
            }
            finally
            {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
                //幾何歪テーブルをクローズ
                FileSystem.FileClose(fileNo);
*/
#endregion
                
                //幾何歪テーブルをクローズ
                if (sr != null)
                {
                    sr.Close();
                    sr = null;
                }
            }
			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： 幾何歪テーブルの保存
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [ /O] Boolean   True: 保存成功、False: 保存失敗
        //
        //補　　足： なし
        //
        //履　　歴： v11.2 2005/10/13  (SI3)間々田   新規作成
        //           v11.5 2006/04/21  (WEB)間々田   戻り値を設定
        //*******************************************************************************
		public static bool WriteHizumi()
		{
			int i = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
            /*
            int fileNo = 0;
            */
#endregion
            
            //戻り値初期化
            bool functionReturnValue = false;

            StreamWriter sw = null;

			//エラー時の扱い
            try
            {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			    //ファイルオープン
			    fileNo = FreeFile();
			    FileSystem.FileOpen(fileNo, HIZUMI_CSV, OpenMode.Output);
*/
#endregion

                //ファイルオープン
                sw = new StreamWriter(HIZUMI_CSV);

                for (i = hizumi.GetLowerBound(0); i <= hizumi.GetUpperBound(0); i++) 
                {
                    sw.WriteLine(hizumi[i].ToString("0.00000000"));
			    }

                //戻り値セット
                functionReturnValue = true;
            }
            catch
            {
                // Nothing
            }
            finally
            {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			    //ファイルクローズ
			    FileSystem.FileClose(fileNo);
*/
#endregion

                //ファイルクローズ
                if (sw != null)
                {
                    sw.Close();
                    sw = null;
                }
            }
			return functionReturnValue;
		}


        //********************************************************************************
        //機    能  ：  幾何歪校正パラメータをコモンに書き込む
        //              変数名           [I/O] 型        内容
        //引    数  ：  iSCN             [I/ ] Long      スキャン位置(0～4)
        //戻 り 値  ：  なし
        //補    足  ：  この処理はＦＰＤ専用処理
        //
        //履    歴  ：  V2.0   00/02/08  (SI1)鈴山       新規作成
        //               v11.5 06/04/21  (WEB)間々田     コモンへの書き込み方法を変更。（putcommon_floatを使用するのをやめた）
        //********************************************************************************
        //Public Sub Set_Vertical_Parameter(ByVal iSCN As Long)
		public static void Set_Vertical_Parameter(int iSCN = 2)     		//v11.5変更 by 間々田 2006/04/21
		{
			//■メカの移動有無プロパティをリセットする   'V4.0 append by 鈴山 2001/02/20
			modSeqComm.SeqBitWrite("VerIIChangeReset", true);			//（シーケンサ通信が可能な場合）

			//■校正ステータスを準備完了にする   'V4.0 append by 鈴山 2001/01/25

			//'幾何歪校正ステータス
			//'    構造体名：mecainf
			//'    コモン名：vertical_cor
			//'    コモン値：0(準備未完了),1(準備完了)
			//Call putcommon_long("mecainf", "vertical_cor", 1)

			//■幾何歪校正パラメータをコモンに書き込む

			//最新scancondparの読み込み                                      'v11.5追加 by 間々田 2006/04/21
			//CallGetScancondpar     'v17.00削除 byやまおか 2010/01/20
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypeII)
             || (CTSettings.detectorParam.DetType == DetectorConstants.DetTypeHama))
            {
                //modScancondpar.CallGetScancondpar();        //v17.00修正 byやまおか 2010/03/04
                CTSettings.scancondpar.Load(CTSettings.scaninh.Data.rotate_select);

            }
		        
			//検出器ピッチ(mm/画素)
            CTSettings.scancondpar.Data.mdtpitch[iSCN] = GVal_mdtpitch[iSCN];

			//幾何歪校正係数(A0～A5)
			//    構造体名：scancondpar
			//    コモン名：a[5][6]
            //CTSettings.scancondpar.Data.a(0, iSCN) = a0[iSCN];			//a[iSCN][0]のこと
            //CTSettings.scancondpar.Data.a(1, iSCN) = A1[iSCN];			//a[iSCN][1]のこと
            //CTSettings.scancondpar.Data.a(2, iSCN) = a2[iSCN];			//a[iSCN][2]のこと
            //CTSettings.scancondpar.Data.a(3, iSCN) = a3[iSCN];			//a[iSCN][3]のこと
            //CTSettings.scancondpar.Data.a(4, iSCN) = a4[iSCN];			//a[iSCN][4]のこと
            //CTSettings.scancondpar.Data.a(5, iSCN) = a5[iSCN];			//a[iSCN][5]のこと
            CTSettings.scancondpar.Data.a[iSCN * 6 + 0] = (float)a0[iSCN];	//a[iSCN][0]のこと
            CTSettings.scancondpar.Data.a[iSCN * 6 + 1] = (float)A1[iSCN];	//a[iSCN][1]のこと
            CTSettings.scancondpar.Data.a[iSCN * 6 + 2] = (float)a2[iSCN];	//a[iSCN][2]のこと
            CTSettings.scancondpar.Data.a[iSCN * 6 + 3] = (float)a3[iSCN];	//a[iSCN][3]のこと
            CTSettings.scancondpar.Data.a[iSCN * 6 + 4] = (float)a4[iSCN];	//a[iSCN][4]のこと
            CTSettings.scancondpar.Data.a[iSCN * 6 + 5] = (float)a5[iSCN];	//a[iSCN][5]のこと

			//有効データ開始画素
            CTSettings.scancondpar.Data.xls[iSCN] = xls[iSCN];

			//有効データ終了画素
            CTSettings.scancondpar.Data.xle[iSCN] = xle[iSCN];

			//最大ファン角
            CTSettings.scancondpar.Data.max_mfanangle[iSCN] = theta0Max[iSCN];

			//複数スライスのとき、それぞれの傾きと切片を書込む
			if (MultiSliceMode) 
            {
				//傾き
				CTSettings.scancondpar.Data.scan_posi_a[0] = GVal_ScanPosiA[0];
				CTSettings.scancondpar.Data.scan_posi_a[1] = GVal_ScanPosiA[1];
				CTSettings.scancondpar.Data.scan_posi_a[2] = GVal_ScanPosiA[2];
				CTSettings.scancondpar.Data.scan_posi_a[3] = GVal_ScanPosiA[3];
				CTSettings.scancondpar.Data.scan_posi_a[4] = GVal_ScanPosiA[4];

				//切片
				CTSettings.scancondpar.Data.scan_posi_b[0] = GVal_ScanPosiB[0];
				CTSettings.scancondpar.Data.scan_posi_b[1] = GVal_ScanPosiB[1];
				CTSettings.scancondpar.Data.scan_posi_b[2] = GVal_ScanPosiB[2];
				CTSettings.scancondpar.Data.scan_posi_b[3] = GVal_ScanPosiB[3];
				CTSettings.scancondpar.Data.scan_posi_b[4] = GVal_ScanPosiB[4];
			}

			//コーンビームスキャンが可能な場合
            if (CTSettings.scaninh.Data.data_mode[2] == 0) 
            {
				//コーンビーム用幾何歪校正係数(B0～B5)
                CTSettings.scancondpar.Data.b[0] = (float)b0;
                CTSettings.scancondpar.Data.b[1] = (float)B1;
                CTSettings.scancondpar.Data.b[2] = (float)B2;
                CTSettings.scancondpar.Data.b[3] = (float)B3;
                CTSettings.scancondpar.Data.b[4] = (float)B4;
                CTSettings.scancondpar.Data.b[5] = (float)B5;

				//m方向ﾃﾞｰﾀﾋﾟｯﾁ(mm)
                CTSettings.scancondpar.Data.dpm = (float)dpm;

				//最大ファン角（コーンビーム用）
				CTSettings.scancondpar.Data.cone_max_mfanangle = theta0MaxCone;
				//added by 間々田　2004/04/26

                //幾何歪テーブルをアスキー形式でファイルに保存する
                //Rev20.00 1次元幾何歪補正はもうやらないため不要 by長野 2014/11/10
                //WriteHizumi();
            }
            
			//scancondparの書き込み                                      'v11.5追加 by 間々田 2006/04/21
			//modScancondpar.CallPutScancondpar();
            CTSettings.scancondpar.Write();


#region		//v11.5削除ここから by 間々田 2006/04/26
            //'■校正を行ったときの周辺情報をコモンに書き込む   'V4.0 append by 鈴山 2001/01/25
			//Call putcommon_long("mecainf", "ver_iifield", GetIINo())
			//Call putcommon_long("mecainf", "ver_mt", scansel.multi_tube)
			//
			//'幾何歪校正を行った時のビニングモードをコモンに書込む           v7.0 FPD対応 by 間々田 2003/09/26
			//'コモン構造体名: mecainf
			//'コモン名:       ver_bin
			//'コモン値　　　：０（１×１），１（２×２），２（４×４）
			//Call putcommon_long("mecainf", "ver_bin", scansel.binning)
            //v11.5削除ここまで by 間々田 2006/04/26
#endregion

        }


        //********************************************************************************
        //機    能  ：  校正用ファントムを出し入れする
        //              変数名           [I/O] 型        内容
        //引    数  ：  iMode            [I/ ] Integer   動作ﾓｰﾄﾞ(0:入れる,1:出す)
        //戻 り 値  ：                   [ /O] Long      関数の戻り値  -1:エラー
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //              V4.0   01/01/31  (SI1)鈴山       frmVerticalから移し、Public関数化
        //              V4.0   01/01/31  (SI1)鈴山       iModeで処理を分岐するように変更
        //********************************************************************************
		public static int Set_Vertical_Phantom(int iMode)
		{
			//Dim phm_busy    As Long
			//Dim phm_ready   As Long

			//戻り値初期化
			int functionReturnValue = -1;

            try
            {
			    //ステータスのコモンを読み込む
			    //phm_busy = GetCommonLong("mecainf", "phm_busy")
			    //phm_ready = GetCommonLong("mecainf", "phm_ready")

			    //スイッチ動作禁止
                //modMecainf.mecainfType theMecainf = default(modMecainf.mecainfType);
                MecaInf theMecainf = new MecaInf();
                theMecainf.Data.Initialize();


			    if (modMechaControl.SwOpeEnd() == 0) 
                {
				    //mecainf取得
				    //modMecainf.GetMecainf(ref theMecainf);
                    theMecainf.Load();
                    
				    //分岐
				    switch (iMode) 
                    {
					    case 0:
						    //準備ＯＫ？
						    //If phm_busy = 0 Then
						    if (theMecainf.Data.phm_busy == 0)
                            {
							    //校正用ファントムを入れる
							    functionReturnValue = modMechaControl.MecaPhmOff();     //V5.0 changed by 山本 2001/08/03  PhmOrigin->PhmOff							    
						    }
						    break;
					    case 1:
						    //準備ＯＫ？
						    //If phm_ready = 1 Then
                            if (theMecainf.Data.phm_ready == 1) 
                            {
							    //校正用ファントムを出す
							    functionReturnValue = modMechaControl.MecaPhmOn();      //V5.0 changed by 山本 2001/08/03  PhmIndex->PhmOn							    
						    }
						    break;
				    }
			    }

			    //スイッチ動作禁止解除
			    modMechaControl.SwOpeStart();
            }
            catch
            {
                // Nothing
            }
            
			return functionReturnValue;
		}


        //********************************************************************************
        //機    能  ：  スライス厚変換（画素→ｍｍ）
        //              変数名           [I/O] 型        内容
        //引    数  ：  fPIX             [I/ ] Single    スライス厚(画素)
        //              fFID             [I/ ] Single    FID(FID+FIDｵﾌｾｯﾄ)
        //              fFCD             [I/ ] Single    FCD(FCD+FCDｵﾌｾｯﾄ)
        //              fMDT             [I/ ] Single    検出器ピッチ(mm/画素)
        //              iVMG             [I/ ] Integer   縦倍率
        //戻 り 値  ：                   [ /O] Double    スライス厚(mm) 0:異常
        //補    足  ：  なし
        //
        //履    歴  ：  V2.0   00/02/08  (SI1)鈴山       新規作成
        //********************************************************************************
		public static float Trans_PixToWid(float fPIX, float fFID, float fFCD, float fMDT, int iVMG)
		{
			float functionReturnValue = 0;

			//パラメータチェック
			if ((fFID <= fFCD) || (fFID <= 0) || (fFCD <= 0) || (fMDT <= 0)) 
            {
				functionReturnValue = 0.0F;
				return functionReturnValue;
			}

			//計算
            float.TryParse(((fPIX * (fMDT * iVMG)) / (fFID / fFCD) + 1E-05).ToString("0.000"), out functionReturnValue);

			return functionReturnValue;
		}

        //********************************************************************************
        //機    能  ：  スライス厚変換（ｍｍ→画素）
        //              変数名           [I/O] 型        内容
        //引    数  ：  iSLW             [I/ ] Single    スライス厚(mm)
        //              fFID             [I/ ] Single    FID(FID+FIDｵﾌｾｯﾄ)
        //              fFCD             [I/ ] Single    FCD(FCD+FCDｵﾌｾｯﾄ)
        //              fMDT             [I/ ] Single    検出器ピッチ(mm/画素)
        //              iVMG             [I/ ] Integer   縦倍率
        //戻 り 値  ：                   [ /O] Double    スライス厚(画素) 0:異常
        //補    足  ：  なし
        //
        //履    歴  ：  V2.0   00/02/08  (SI1)鈴山       新規作成
        //********************************************************************************
		public static float Trans_WidToPix(float iSLW, float fFID, float fFCD, float fMDT, short iVMG)
		{
			float functionReturnValue = 0;

			//パラメータチェック
			if ((fFID <= fFCD) || (fFID <= 0) || (fFCD <= 0) | (fMDT <= 0)) 
            {
				functionReturnValue = 0.0F;
				return functionReturnValue;
			}

			//計算
			functionReturnValue = (iSLW / (fMDT * iVMG)) * (fFID / fFCD);
			return functionReturnValue;
		}


#region //v11.5削除ここから by 間々田 2006/06/12 modSeqCommに移動
        //'********************************************************************************
        //'機    能  ：  指定FCDに移動する
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  FCD              [I/ ] Long      FCDの指定位置(×10mm)
        //'戻 り 値  ：                   [ /O] Boolean   結果(True:正常,False:ﾀｲﾑｱｳﾄ)
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V6.0   02/09/18  (SI4)間々田     新規作成
        //'********************************************************************************
        //Public Function MoveFCD(ByVal FCD As Long) As Boolean
        //
        //    Dim nowFCD      As Long
        //    Dim nowSpeed    As Integer
        //    Dim moving      As Boolean
        //
        //    Dim StartTime   As Single   '開始時間(秒)
        //    Dim PauseTime   As Single   '待ち時間(秒)
        //    Dim count       As Integer
        //
        //    '初期値設定
        //    MoveFCD = False
        //
        //    '現在のFCD座標および速度（いずれも×10mm）を取得
        //    If Not ReadFCD(nowFCD, nowSpeed, moving) Then Exit Function
        //
        //    '現在位置と指定位置が同じ場合は関数を抜ける  'added by 山本 2002-9-24
        //    If nowFCD = FCD Then
        //        MoveFCD = True
        //        Exit Function
        //    End If
        //
        //    '現在のFCD座標から移動後のFCD座標までの移動時間を算出し、待ち時間を設定する（５秒余計にとる）
        //    If nowSpeed = 0 Then
        //        PauseTime = 5
        //    Else
        //        PauseTime = Abs(nowFCD - FCD) / nowSpeed + 5
        //    End If
        //
        //'    Debug.Print "目標FCD:" & CStr(FCD) & "  現FCD:"; CStr(nowFCD) & "  待ち時間（秒）:" & PauseTime
        //
        //    '移動FCD座標送信
        //    If Not SeqWordWrite("YIndexPosition", CStr(FCD), False) Then Exit Function
        //    DoEvents         'added by 山本　2003-10-23
        //
        //    '移動実行命令送信
        //    If Not SeqBitWrite("YIndex", True, False) Then Exit Function
        //    DoEvents         'added by 山本　2003-10-23
        //
        //    '開始時間を設定
        //    StartTime = Timer
        //    count = 1
        //
        //    '一定時間待つ・・・
        //    Do While (Timer - StartTime < PauseTime)
        //
        //        '1秒おきにチェック
        //        If Timer - StartTime > count Then
        //
        //            '現在のFCD座標および速度（いずれも×10mm）を取得
        //            If ReadFCD(nowFCD, nowSpeed, moving) Then
        //
        //                '動作が停止し、FCDが指定位置に達したら、終わり
        //                If Not moving And nowFCD = FCD Then
        //                    MoveFCD = True
        //                    Exit Do
        //                End If
        //
        //            End If
        //
        //            count = count + 1
        //
        //        End If
        //
        //        DoEvents
        //
        //    Loop
        //
        //End Function
        //
        //'********************************************************************************
        //'機    能  ：  指定FIDに移動する
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  FID              [I/ ] Long      FIDの指定位置(×10mm)
        //'戻 り 値  ：                   [ /O] Boolean   結果(True:正常,False:ﾀｲﾑｱｳﾄ)
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V6.0   02/09/27  (CATS1)山本     新規作成
        //'********************************************************************************
        //Public Function MoveFID(ByVal Fid As Long) As Boolean
        //
        //    Dim nowFID      As Long
        //    Dim nowSpeed    As Integer
        //    Dim moving      As Boolean
        //
        //    Dim StartTime   As Single   '開始時間(秒)
        //    Dim PauseTime   As Single   '待ち時間(秒)
        //    Dim count       As Integer
        //
        //    '初期値設定
        //    MoveFID = False
        //
        //    '現在のFID座標および速度（いずれも×10mm）を取得
        //    If Not ReadFID(nowFID, nowSpeed, moving) Then Exit Function
        //
        //    '現在位置と指定位置が同じ場合は関数を抜ける  'added by 山本 2002-9-24
        //    If (nowFID = Fid) Or (nowFID = Fid + 1) Or (nowFID = Fid - 1) Then
        //        MoveFID = True
        //        Exit Function
        //    End If
        //
        //    '現在のFID座標から移動後のFID座標までの移動時間を算出し、待ち時間を設定する（５秒余計にとる）
        //    If nowSpeed = 0 Then
        //        PauseTime = 5
        //    Else
        //        PauseTime = Abs(nowFID - Fid) / nowSpeed + 5
        //    End If
        //
        //'    Debug.Print "目標FID:" & CStr(FID) & "  現FID:"; CStr(nowFID) & "  待ち時間（秒）:" & PauseTime
        //
        //    '移動FID座標送信
        //    If Not SeqWordWrite("IIIndexPosition", CStr(Fid), False) Then Exit Function
        //    DoEvents         'added by 山本　2003-10-23
        //
        //    '移動実行命令送信
        //    If Not SeqBitWrite("IIIndex", True, False) Then Exit Function
        //    DoEvents         'added by 山本　2003-10-23
        //
        //    '開始時間を設定
        //    StartTime = Timer
        //    count = 1
        //
        //    '一定時間待つ・・・
        //    Do While (Timer - StartTime < PauseTime)
        //
        //        '1秒おきにチェック
        //        If Timer - StartTime > count Then
        //
        //            '現在のFID座標および速度（いずれも×10mm）を取得
        //            If ReadFID(nowFID, nowSpeed, moving) Then
        //
        //                '動作が停止し、FIDが指定位置に達したら、終わり
        //                If Not moving And ((nowFID = Fid) Or (nowFID = Fid + 1) Or (nowFID = Fid - 1)) Then
        //                    MoveFID = True
        //                    Exit Do
        //                End If
        //
        //            End If
        //
        //            count = count + 1
        //
        //        End If
        //
        //        DoEvents
        //
        //    Loop
        //
        //End Function
        //
        //'********************************************************************************
        //'機    能  ：  指定X座標に移動する
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  FCD              [I/ ] Long      FCDの指定位置(×10mm)
        //'戻 り 値  ：                   [ /O] Boolean   結果(True:正常,False:ﾀｲﾑｱｳﾄ)
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V6.0   02/09/18  (SI4)間々田     新規作成
        //'********************************************************************************
        //Public Function MoveXpos(ByVal xPos As Integer) As Boolean
        //
        //    Dim nowXpos     As Integer
        //    Dim nowSpeed    As Integer
        //    Dim moving      As Boolean
        //
        //    Dim StartTime   As Single   '開始時間(秒)
        //    Dim PauseTime   As Single   '待ち時間(秒)
        //    Dim count       As Integer
        //
        //    '初期値設定
        //    MoveXpos = False
        //
        //    '現在のX座標および速度（いずれも×10mm）を取得
        //    If Not ReadXpos(nowXpos, nowSpeed, moving) Then Exit Function
        //
        //    '現在位置と指定位置が同じ場合は関数を抜ける  'added by 山本 2002-9-24
        //    If nowXpos = xPos Then
        //        MoveXpos = True
        //        Exit Function
        //    End If
        //
        //    '現在のX座標から移動後のX座標までの移動時間を算出し、待ち時間を設定する（５秒余計にとる）
        //    If nowSpeed = 0 Then
        //        PauseTime = 5
        //    Else
		        ///        PauseTime = Abs(nowXpos - Xpos) / nowSpeed + 5      ''旧バージョン6.3用
        //        PauseTime = Abs(nowXpos - xPos) / 10 / nowSpeed + 5       '変更 巻渕 2003-03-03  小数点以下2桁まで対応
        //    End If
        //
        //    '移動X座標送信
        //    If Not SeqWordWrite("XIndexPosition", CStr(xPos), False) Then Exit Function
        //
        //    '移動実行命令送信
        //    If Not SeqBitWrite("XIndex", True, False) Then Exit Function
        //
        //    '開始時間を設定
        //    StartTime = Timer
        //    count = 1
        //
        //    '一定時間待つ・・・
        //    Do While (Timer - StartTime < PauseTime)
        //
        //        '1秒おきにチェック
        //        If Timer - StartTime > count Then
        //
		        ///            '現在のX座標および速度（いずれも×10mm）を取得      ''旧バージョン6.3用
        //            '現在のX座標および速度（nowSpeed×10mm、nowXposは×100mm）を取得      '変更 巻渕 2003-03-03  小数点以下2桁まで対応
        //            If ReadXpos(nowXpos, nowSpeed, moving) Then
        //
        //                '動作が停止し、Xが指定位置に達したら、終わり
        //                If (Not moving) And (nowXpos = xPos) Then
        //                    MoveXpos = True
        //                    Exit Do
        //                End If
        //
        //            End If
        //
        //            count = count + 1
        //
        //        End If
        //
        //        DoEvents
        //
        //    Loop
        //
        //End Function
        //v11.5削除ここから by 間々田 2006/06/12
#endregion

#region //v11.5削除ここから by 間々田 2006/06/09 現在未使用
        //'********************************************************************************
        //'機    能  ：  回転中心ファントムワイヤ位置の求出関数
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  wc               [ /O] Long
        //'戻 り 値  ：                   [ /O] Long      結果(0:正常,1:異常)
        //'補    足  ：
        //'
        //'履    歴  ：  V6.0   02/07/18  (SI4)間々田       新規作成
        //'********************************************************************************
        //Public Function Get_WirePosition(wc As Single) As Long
        //
        //    Dim rc              As Long
        //    Dim tmpReg          As RECT
        //    Dim HistData(10)    As Single
        //    Dim j               As Long
        //    Dim c               As Long     '画像配列用カウンタ
        //    Dim roiRect         As RECT     'ヒストグラム用矩形
        //    Dim Sigma_F         As Double   '画像ﾃﾞｰﾀの和
        //    Dim Sigma_FX        As Double   '画像ﾃﾞｰﾀ＊X座標の和
        //    Dim W_Cnt_Flag     As Boolean   'ワイヤのカウンタフラグ
        //
        //    On Error GoTo Err_Process
        //
        //    '■イメージプロで新しい画像を開く
        //
        //    'Image-Pro 画像データ表示
        //    rc = IpAppCloseAll()                                                '開いている全ての画像ｳｨﾝﾄﾞを閉じる
        //    rc = IpWsCreate(h_size, v_size, 300, IMC_GRAY16)                    '空の画像ウィンドウを生成（Gray Scale 16形式）
        //
        //    '■イメージプロの新規画像に透視画像を書込む
        //
        //    tmpReg.Left = 0
        //    tmpReg.Top = 0
        //    tmpReg.Right = h_size - 1
        //    tmpReg.Bottom = v_size - 1
        //    rc = IpDocPutArea(DOCSEL_ACTIVE, tmpReg, W_IMAGE(0), CPROG)         'ユーザが作成した画像ﾃﾞｰﾀをのImage-Proの画像に書込む
        //    rc = IpAppUpdateDoc(DOCSEL_ACTIVE)                                  '画像ウィンドの再描画
        //
        //    '■画像を白黒反転させる
        //
        //    rc = IpLutSetAttr(LUT_CONTRAST, -2)
        //
        //    'Image-Pro 画像データ表示
        //    tmpReg.Left = 0
        //    tmpReg.Top = 0
        //    tmpReg.Right = h_size - 1
        //    tmpReg.Bottom = v_size - 1
        //    rc = IpDocGetArea(DOCSEL_ACTIVE, tmpReg, W_IMAGE(0), CPROG)
        //    rc = IpAppUpdateDoc(DOCSEL_ACTIVE)                                  '画像ウィンドの再描画
        //
        //    '■ヒストグラム処理を行なうＲＯＩを設定する
        //
        //    'ヒストグラム処理
        //    roiRect.Left = FRMWIDTH
        //    roiRect.Top = FRMWIDTH
        //    roiRect.Right = h_size - FRMWIDTH
        //    roiRect.Bottom = v_size - FRMWIDTH
        //
        //    '■ヒストグラム処理をし、画素値の最大・最小値を求める
        //
        //    ret = IpAoiCreateBox(roiRect)
        //    rc = IpHstCreate()                                                  'ヒストグラムｳｨﾝﾄﾞを開く
        //    rc = IpHstGet(GETSTATS, 0, HistData(0))                             '輝度統計を取得　～Max：HistData(4)、Min：HistData(3)
        //    rc = IpHstDestroy()                                                 'ヒストグラムｳｨﾝﾄﾞを閉る
        //
        //    '■画像の濃度を１６ビットフルレンジに変更する
        //
        //    '画像の２値化
        //    '画像のゴミ除去
        //    ChangeFullRange W_IMAGE(0), h_size, v_size, CLng(HistData(3)), CLng(HistData(4))
        //
        //    tmpReg.Left = 0
        //    tmpReg.Top = 0
        //    tmpReg.Right = h_size - 1
        //    tmpReg.Bottom = v_size - 1
        //
        //    'ゴミ除去後のImagePro再表示
        //    rc = IpAppCloseAll()                                                '開いている全ての画像ｳｨﾝﾄﾞを閉じる
        //    rc = IpWsCreate(h_size, v_size, 300, IMC_GRAY16)                    '空の画像ウィンドウを生成（Gray Scale 16形式）
        //    rc = IpDocPutArea(DOCSEL_ACTIVE, tmpReg, W_IMAGE(0), CPROG)
        //    rc = IpAppUpdateDoc(DOCSEL_ACTIVE)                                  '画像ウィンドの再描画
        //
        //    '■画像の周辺部を０にする
        //
        //    '周囲の耳きり落とし画像にする
        //    Call Pic_Imgside(W_IMAGE(0), FRMWIDTH, h_size, v_size)
        //
        //    '再表示
        //    rc = IpDocPutArea(DOCSEL_ACTIVE, tmpReg, W_IMAGE(0), CPROG)      'ユーザが作成した画像ﾃﾞｰﾀをのImage-Proの画像に書込む
        //    rc = IpAppUpdateDoc(DOCSEL_ACTIVE)                                  '画像ウィンドの再描画
        //
        //    '■画像を２値化する
        //
        //    '２値化画像に変換
        //    rc = IpLutBinarize(0, 128, 0)
        //
        //    '■２値化画像を配列 B_IMAGE に取込む
        //
        //    '透視画像２値化データの配列の再定義
        //    ReDim B_IMAGE(v_size * h_size)
        //
        //    'Image-Pro 画像データの取得
        //    tmpReg.Left = 0
        //    tmpReg.Top = 0
        //    tmpReg.Right = h_size - 1
        //    tmpReg.Bottom = v_size - 1
        //    rc = IpDocGetArea(DOCSEL_ACTIVE, tmpReg, B_IMAGE(0), CPROG)
        //    If rc <> 0 Then GoTo Err_Process
        //
        //    '■２値化画像 B_IMAGE の再表示
        //
        //    '２値化画像の再表示
        //    rc = IpWsCreate(h_size, v_size, 300, IMC_GRAY16)                 '空の画像ウィンドウを生成（Gray Scale 16形式）
        //    tmpReg.Left = 0
        //    tmpReg.Top = 0
        //    tmpReg.Right = h_size - 1
        //    tmpReg.Bottom = v_size - 1
        //    rc = IpDocPutArea(DOCSEL_ACTIVE, tmpReg, B_IMAGE(0), CPROG)      'ユーザが作成した画像ﾃﾞｰﾀをのImage-Proの画像に書込む
        //    rc = IpAppUpdateDoc(DOCSEL_ACTIVE)                               '画像ウィンドの再描画
        //    If rc <> 0 Then GoTo Err_Process
        //
        //    'カウンタの初期化
        //    W_Cnt_Flag = False
        //    Sigma_F = 0
        //    Sigma_FX = 0
        //
        //    '垂直線と水平線の交点を求める
        //    For j = 10 To h_size - 1 - 10   '左右両端の余裕：10画素
        //
        //        'xi,ti座標系からi,j座標系に戻す
        //        c = Int(GVal_ScanPosiA(2) * j + GVal_ScanPosiB(2) + Int(v_size / 2) - GVal_ScanPosiA(2) * Int(h_size / 2))
        //        c = c * h_size + j
        //
        //        '垂直線の重心を交点座標とする
        //        If B_IMAGE(c) <> 0 Then
        //            Sigma_F = Sigma_F + W_IMAGE(c)
        //            Sigma_FX = Sigma_FX + W_IMAGE(c) * j
        //            W_Cnt_Flag = True
        //
        //        ElseIf W_Cnt_Flag Then
        //            wc = Sigma_FX / Sigma_F
        //            Exit For
        //
        //        End If
        //
        //    Next
        //
        //    Get_WirePosition = True
        //
        //    Exit Function
        //
        //Err_Process:
        //
        //    Get_WirePosition = False
        //
        //End Function
        //v11.5削除ここまで by 間々田 2006/06/09 現在未使用
#endregion

#region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //'********************************************************************************
        //'機    能  ：
        //'              変数名           [I/O] 型        内容
        //'引    数  ：
        //'戻 り 値  ：
        //'補    足  ：
        //'
        //'履    歴  ：  V7.0   03/09/25  (SI4)間々田       新規作成
        //'********************************************************************************
        //Public Sub ExtTrigOn(Optional ByVal theConeBeam As Integer = 0, _
        //'                    Optional ByVal theView As Variant, _
        //'                    Optional ByVal theIntegNum As Variant) 'theView と theIntegNum追加 2005/02/05 by 間々田
        //
        //    Dim rc          As Long
        //    Dim RAT         As Single
        //    Dim TT          As Single
        //    Dim char_data   As String
        //    Dim TrgCycle    As Single       'added by 山本　2004-6-11
        //    Dim View        As Long
        //    Dim IntegNum    As Long     'theView と theIntegNum追加 2005/02/05 by 間々田
        //
        //
        //    If IsMissing(theView) Then
        //        View = VIEW_N
        //    Else
        //        View = theView
        //    End If
        //
        //    If IsMissing(theIntegNum) Then
        //        IntegNum = scansel.scan_integ_number
        //    Else
        //        IntegNum = theIntegNum
        //    End If
        //
        //    '回転加速時間の読込み
        //
        //    'Ｘ線管回転を選択した場合シーケンサ通信でＸ線管回転軸の加速度を取得して加速時間を計算する
        //    If (scansel.rotate_select = 1) And (scaninh.rotate_select = 0) Then
        //
        //        Dim xray_accel As Single    'Ｘ線管回転軸の加速時間
        //#If v9Seq Then
        //        xray_accel = MySeq.stsXrayRotAccel / 1000
        //'        RAT = 60 * FR(theConeBeam) / xray_accel / VIEW_N / GVal_ScnInteg
        //        RAT = xray_accel        'changed by 山本　2004-6-1  シーケンサ通信の仕様変更のため
        //#Else
        //        rc = GetRotAccel(RAT)
        //#End If
        //
        //    'その他の場合
        //    Else
        //        rc = GetRotAccel(RAT)
        //
        //     End If
        //
        //    'データ収集開始角度設定
		        ///    StartAngle = Int(180 * FR(0) * RAT / VIEW_N / GVal_ScnInteg) + 1
        //'    StartAngle = Int(180 * FR(theConeBeam) * RAT / VIEW_N / GVal_ScnInteg) + 1  'change by 間々田 2003/10/22
        //'    StartAngle = 360 - StartAngle
        //    'StartAngle = Int(180 * FR(theConeBeam) * RAT / VIEW_N / GVal_ScnInteg)
        //    StartAngle = Int(180 * FR(theConeBeam) * RAT / View / IntegNum)         '引数で指定できるように変更 by 間々田 2005/02/04
        //
        //    'Ｘ線管回転を選択した場合
        //    If (scansel.rotate_select = 1) And (scaninh.rotate_select = 0) Then
        //
        //        If Use_FlatPanel Then
        //
        //#If v9Seq Then
        //            Select Case MySeq.stsXrayRotPos / 10000
        //                Case Is < 360
        //                    StartAngle = scancondpar.xrot_start_pos + StartAngle + 1 'CW
        //                Case Is > 360
        //                    StartAngle = scancondpar.xrot_end_pos - StartAngle - 1   'CCW
        //            End Select
        //#End If
        //
        //        End If
        //
        //    'テーブル回転の場合
        //    'テーブル回転・FPDの場合
        //    ElseIf Use_FlatPanel Then
        //        StartAngle = 360 - StartAngle - 1
        //
        //    'テーブル回転・I.I.の場合
        //    Else
        //        StartAngle = StartAngle + 1
        //
        //    End If
        //
        //    SeqWordWrite "ScanStartPos", CStr(Fix(StartAngle)), False
        //    PauseForDoEvents 0.2                                    'トリガパルス時間が切り替わらない対策
        //
        //    '外部トリガパルス幅の設定
        //'    TT = 1000000 / FR(0) / 2
        //    TT = 1000000 / FR(theConeBeam) / 2                      'change by 間々田 2003/10/22
        //    TrgCycle = 1000000 / FR(theConeBeam)                    'added by 山本　2004-6-11
        //
        //    SeqWordWrite "TrgTime", CStr(Fix(TT)), False
        //    PauseForDoEvents 0.2                                    'added by 山本　2004-6-11
        //
        //    SeqWordWrite "TrgCycleTime", CStr(Fix(TrgCycle)), False  'added by 山本　2004-6-11
        //    PauseForDoEvents 0.2                                    'トリガパルス時間が切り替わらない対策
        //
        //    '外部トリガＯＮ
        //    SeqBitWrite "TrgReq", True
        //    DoEvents
        //    PauseForDoEvents 0.2                                    'Ｘ線が元々ＯＮしていると外部トリガが出ない対策
        //
        //End Sub
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion
        
        
        //********************************************************************************
        //機    能  ：
        //              変数名           [I/O] 型        内容
        //引    数  ：
        //戻 り 値  ：
        //補    足  ：
        //
        //履    歴  ：  V7.0   03/09/25  (SI4)間々田       新規作成
        //********************************************************************************
		public static bool GetDefImage()
		{
			int rc = 0;

			//戻り値初期化
			bool functionReturnValue = false;

			//フラットパネル欠陥画像のファイルが存在しない場合
            //if (!modFileIO.FSO.FileExists(DEF_CORRECT))
            if (!File.Exists(DEF_CORRECT))
            {
                //メッセージ表示：欠陥画像ファイルがありません。
                MessageBox.Show(StringTable.GetResString(9912, CTResources.LoadResString(12734)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return functionReturnValue;
            }

            
			//フラットパネル欠陥画像のファイルサイズチェック
            //if (FileSystem.FileLen(DEF_CORRECT) != h_size * v_size * 2)
            FileInfo finfo = new FileInfo(DEF_CORRECT);
            if (finfo.Length != CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size * 2)
            {
                //メッセージ表示：欠陥画像のファイルサイズが正しくありません。
                MessageBox.Show(StringTable.GetResString(StringTable.IDS_FileSizeError, CTResources.LoadResString(12734)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return functionReturnValue;
            }

            Def_IMAGE = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];			//v15.0変更 -1した by 間々田 2009/06/03

			//フラットパネル欠陥画像を配列に読込む｡
            rc = IICorrect.ImageOpen(ref Def_IMAGE[0], DEF_CORRECT, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);

			functionReturnValue = true;
			return functionReturnValue;
		}


        //********************************************************************************
        //機    能  ：
        //              変数名           [I/O] 型        内容
        //引    数  ：
        //戻 り 値  ：
        //補    足  ：
        //
        //履    歴  ：  V7.0   03/09/25  (SI4)間々田       新規作成
        //********************************************************************************
		public static bool GetGainImage()
		{
			int rc = 0;

			//戻り値初期化
			bool functionReturnValue = false;

			//v17.00修正 byやまおか 2010/03/04
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypeII) || (CTSettings.detectorParam.DetType == DetectorConstants.DetTypeHama))
            {
                //ゲイン校正画像のファイルが存在しない場合
                //if (!modFileIO.FSO.FileExists(GAIN_CORRECT))
                if (!File.Exists(GAIN_CORRECT))
                {
                    //メッセージ表示：ゲイン校正画像ファイルがありません。
                    MessageBox.Show(StringTable.GetResString(9912, CTResources.LoadResString(12740)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return functionReturnValue;
                }

                //ゲイン校正画像のファイルサイズチェック
                //if (FileSystem.FileLen(GAIN_CORRECT) != h_size * v_size * 2)
                FileInfo finfo = new FileInfo(GAIN_CORRECT);
                if (finfo.Length != CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size * 2)
                {
                    //メッセージ表示：ゲイン校正画像のファイルサイズが正しくありません。
                    MessageBox.Show(StringTable.GetResString(StringTable.IDS_FileSizeError, CTResources.LoadResString(12740)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return functionReturnValue;
                }

            }	//v17.00 if追加 byやまおか 2010/02/16


            if (GainCorFlag == 0)	//v17.00 山本 2010-03-03 自動校正中にスキャン位置校正前に新たにRedimしない対策（ゲイン校正画像が消えてしまうため）
            {
                GAIN_IMAGE = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];          //v15.0変更 -1した by 間々田 2009/06/03
                Gain_Image_L = new uint[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];        //v17.00 山本 2009-10-19    'v17.02下から移動 byやまおか 2010/07/15
                
                //追加2014/10/07hata_v19.51反映
                //GAIN_IMAGE_SFT = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];                //v18.00追加 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                //Gain_Image_L_SFT = new uint[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];                //v18.00追加 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                //Rev23.20 左右シフト対応 by長野 2015/11/19
                GAIN_IMAGE_SFT_R = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
                Gain_Image_L_SFT_R = new uint[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
                GAIN_IMAGE_SFT_L = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
                Gain_Image_L_SFT_L = new uint[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
            }
			//ReDim Gain_Image_L(h_size * v_size - 1) 'v17.00 山本 2009-10-19    'v17.02上へ移動 byやまおか 2010/07/15

			//ゲイン校正画像を配列に読込む｡
			//rc = ImageOpen(Gain_Image(0), GAIN_CORRECT, h_size, v_size)    'v17.00削除 byやまおか 2010/01/20
			//v17.00追加(ここから) byやまおか 2010/01/20
            switch (CTSettings.detectorParam.DetType)
            {
                case DetectorConstants.DetTypeII:
                case DetectorConstants.DetTypeHama:
                    rc = ImageOpen(ref GAIN_IMAGE[0], GAIN_CORRECT, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                    break;
                case DetectorConstants.DetTypePke:
                    //rc = ImageOpen(Gain_Image(0), GAIN_CORRECT, h_size, v_size)    'PkeFPDの場合はここで読まない 2010-02-02 山本
                    break;
            }
			//v17.00追加(ここまで) byやまおか 2010/01/20

			functionReturnValue = true;
			return functionReturnValue;
		}


        //   オフセット校正画像・Def校正画像・ゲイン校正画像の取得
        //   副産物としてFpdGainCorrectの第５引数 ADV（ゲイン校正時のかさ上げ量）用の値取得  append by 間々田 2003/10/14
        //
		public static bool GetDefGainOffset(ref int adv)
		{
			ushort[] WorkImage = null;		//ワーク用の画像配列

			//戻り値初期化
			bool functionReturnValue = false;

			//欠陥画像データ取得
			//If Not GetDefImage() Then Exit Function
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypeII) || (CTSettings.detectorParam.DetType == DetectorConstants.DetTypeHama))	    //v17.00修正 byやまおか 2010/03/04
            {
                if (!GetDefImage()) return functionReturnValue;
            }

			//ゲイン校正画像データ取得
            if (!GetGainImage()) return functionReturnValue;

            WorkImage = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size + 1];

			//ゲイン画像データをコピー
            GAIN_IMAGE.CopyTo(WorkImage, 0);

			//ゲイン画像データに対して画素欠陥補正を行う
			//Call FpdDefCorrect_short(WorkImage(0), Def_IMAGE(0), h_size, v_size, 0, v_size - 1)
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypeII) || (CTSettings.detectorParam.DetType == DetectorConstants.DetTypeHama))	    //v17.00修正 byやまおか 2010/03/04
            {
                FpdDefCorrect_short(ref WorkImage[0], ref Def_IMAGE[0],0,0,0,0);
            }

			//ゲイン画像データの平均値を求める
            Cal_Mean_short(ref WorkImage[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, ref GainMean);

			//adv = CLng(GainMean - OffsetMean)
			//adv = CLng(GainMean)                        '修正　by　山本　2003-10-16
            adv = (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke ? 8000 : Convert.ToInt32(GainMean));		//v17.10変更 byやまおか 2010/07/28

			//戻り値のセット
			functionReturnValue = true;
			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： hole_coo.dat読み込み
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.2 2005/10/13  (SI3)間々田   新規作成
        //*******************************************************************************
		public static int ReadHole()
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
            short fileNo = 0;
*/
#endregion

            string FileName = null;
			int i = 0;
			//Dim kmax        As Integer   'deleted by 山本　2005-12-21 Public宣言に変更

			//戻り値初期化
			int functionReturnValue = 0;

			//読み込むファイルのファイル名
			FileName = CORRECT_PATH + "hole_coo.dat";
            
			//ファイルサイズから予測される穴数
            kmax = Convert.ToInt32((double)(new FileInfo(FileName).Length) / (8 * 4));
            
			//領域確保
			gx = new double[kmax];
			gy = new double[kmax];
			gg = new double[kmax];
			gh = new double[kmax];

            FileStream fs = null;
            BinaryReader br = null;

			//エラー時の扱い
            try
            {                
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
                //ファイルオープン
                fileNo = FreeFile();
			    FileSystem.FileOpen(fileNo, FileName, OpenMode.Binary, OpenAccess.Read, OpenShare.LockWrite);
*/
#endregion
                
                //ファイルオープン
                fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                br = new BinaryReader(fs);

                for (i = 0; i <= kmax - 1; i++)
                {
                    gx[i] = br.ReadDouble();
                    gy[i] = br.ReadDouble();
                    gg[i] = br.ReadDouble();
                    gh[i] = br.ReadDouble();
                }

                functionReturnValue = kmax;
            }
            catch
            {
                // Nothing
            }
            finally
            { 		
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*	
                //ファイルクローズ
                FileSystem.FileClose(fileNo);
*/
#endregion
                
                //ファイルクローズ
                if (br != null)
                {
                    br.Close();
                    br = null;
                }

                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
            }
            return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： hole_coo.dat書き込み
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.2 2005/10/13  (SI3)間々田   新規作成
        //*******************************************************************************
		public static bool WriteHole()
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			short fileNo = 0;
*/
#endregion
            
            string FileName = null;
			int i = 0;

			//戻り値初期化
            bool functionReturnValue = false;

			//読み込むファイルのファイル名
			FileName = CORRECT_PATH + "hole_coo.dat";

            FileStream fs = null;
            BinaryWriter bw = null;

			//エラー時の扱い
            try
            {
			    //ファイルサイズが小さくならない対策　by 山本　2005-11-12
                //if (!string.IsNullOrEmpty(FileSystem.Dir(FileName, FileAttribute.Normal))) FileSystem.Kill(FileName);
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			    //ファイルオープン
			    fileNo = FreeFile();
			    FileSystem.FileOpen(fileNo, FileName, OpenMode.Binary, OpenAccess.Write);
*/
#endregion

                //ファイルオープン
                fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write);
                bw = new BinaryWriter(fs);

                for (i = 0; i <= gx.Length - 1; i++)
                {
                    bw.Write(gx[i]);
                    bw.Write(gy[i]);
                    bw.Write(gg[i]);
                    bw.Write(gh[i]);
                }

                functionReturnValue = true;
            }
            catch
            {
                // Nothing
            }
            finally
            {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
                //ファイルクローズ
			    FileSystem.FileClose(fileNo);
*/
#endregion

                //ファイルクローズ
                if (bw != null)
                {
                    bw.Close();
                    bw = null;
                }

                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
            }
            return functionReturnValue;
		}

        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから
        //*************************************************************************************************
        //機　　能： FPDの場合，フォームロード時にパラメータ計算のため２次元幾何歪補正を行う
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.20  2010/09/08 (検S1)長野  リニューアル
        //*************************************************************************************************
        public static void FPD_DistorsionCorrect()
        {
            if (CTSettings.detectorParam.Use_FlatPanel)
            {
                Get_Vertical_Parameter_Ex(0);
                Set_Vertical_Parameter();

                if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
                {
                    //２次元幾何歪補正   '追加 2009-10-01 山本
                    POSITION_IMAGE = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];
                    modScanCorrectNew.DistortionCorrect(ref POSITION_IMAGE);
                }
            }//v17.00追加(ここまで) byやまおか 2010/02/08
        }

        //*************************************************************************************************
        //機　　能： シフトスキャンが選ばれているかチェックする関数
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v18.00  2011/07/14 やまおか     新規作成
        //*************************************************************************************************
        public static bool IsShiftScan()
        {
            bool functionReturnValue = false;

            //初期化
            functionReturnValue = false;

            //v19.50 frmScanControlロード前はscanselから取得する by長野 2013/11/08
            //v19.51 産業用CT以外ではfrmScanConditionを参照するように変更 by長野 2014/03/04
            if ((CTSettings.scaninh.Data.avmode == 0 & modLibrary.IsExistForm("frmScanControl")))
            {
                //スキャンコントロールで選ばれている
                if ((frmScanControl.Instance.optScanMode[4].Checked == true))
                {
                    functionReturnValue = true;
                }
            }
            else if ((CTSettings.scaninh.Data.avmode != 0 & modLibrary.IsExistForm("frmScanCondition")))
            {
                //if ((frmScanCondition.Instance.optScanMode[4].Checked == true))
                //Rev25.00 Wスキャン追加 by長野 2016/08/08
                if ((frmScanCondition.Instance.optScanMode[4].Checked == true))
                {
                    functionReturnValue = true;
                }
            }
            else
            {
                if ((CTSettings.scansel.Data.scan_mode == 4))
                {
                    functionReturnValue = true;
                }
            }
            return functionReturnValue;

        }
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここまで

        //*************************************************************************************************
        //機　　能： Wスキャン選ばれているかチェックする関数
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v25.00  2016/08/08 (検S1)長野     新規作成
        //*************************************************************************************************
        public static bool IsW_Scan()
        {
            bool functionReturnValue = false;

            //初期化
            functionReturnValue = false;

            if (CTSettings.W_ScanOn & modLibrary.IsExistForm("frmScanControl"))
            {
                //スキャンコントロールで選ばれている
                if ((frmScanControl.Instance.chkW_Scan.Checked == true))
                {
                    functionReturnValue = true;
                }
            }
            else
            {
                if ((CTSettings.scaninh.Data.avmode != 0 & modLibrary.IsExistForm("frmScanCondition")))
                {
                    if (frmScanCondition.Instance.chkW_Scan.Checked == true)
                    {
                        functionReturnValue = true;
                    }
                }
                else
                {
                    if (CTSettings.scansel.Data.w_scan == 1)
                    {
                        functionReturnValue = true;
                    }
                }
            }
            return functionReturnValue;

        }

        //追加2014/11/28hata_v19.51_dnet
        //*************************************************************************************************
        //機　　能： 指定単位で四捨五入する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Val             decimal
        //           max             decimal
        //           min             decimal
        //           Inc             int            四捨五入する単位（1,10,100,1000)

        //戻 り 値： decimal
        //
        //補　　足： なし
        //
        //履　　歴： V19.51dnet  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        public static decimal RoundControlVale(decimal Val, decimal Max, decimal Min, float Inc)
        {
            double val1 = 0;

            try
            {
                val1 = (double)Val / (double)Inc;
                val1 = Math.Round(val1, 0, MidpointRounding.AwayFromZero) * (int)Inc;

                if (val1 < (double)Min)
                {
                    //val1 = (double)Min / (double)Inc;
                    //val1 = Math.Round(val1, 0, MidpointRounding.AwayFromZero) * (int)Inc;
                    val1 = (double)Min;
                }
                else if (val1 > (double)Max)
                {
                    //
                    //val1 = (int)((double)Max / (double)Inc);
                    //val1 = Math.Round(val1, 0, MidpointRounding.AwayFromZero) * (int)Inc;
                    val1 = (double)Max;
                }
            }
            catch
            {
                val1 = (double)Inc;
            }
            return (decimal)val1;
        }
        //*******************************************************************************
        //機　　能： ゲイン校正後のエアーデータ(左右シフト位置)の平均値取得
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 
        //
        //履　　歴： v25.1  12/12/16  (検S1)長野    新規作成
        //*******************************************************************************
        public static void calShiftImageMagVal()
        {
            int invPix = 0;
            //invPix = CTSettings.detectorParam.h_size == 2048 ? 24 : 12;
            invPix = 256;
            int h_size = CTSettings.detectorParam.h_size;
            int v_size = CTSettings.detectorParam.v_size;

            int minValR = 0;
            int maxValR = 0;
            float divR = 0.0f;
            float meanR = 0.0f;
            int minValL = 0;
            int maxValL = 0;
            float divL = 0.0f;
            float meanL = 0.0f;

            ushort[] tempR = new ushort[h_size * v_size];
            ushort[] tempL = new ushort[h_size * v_size];

            ScanCorrect.ImageOpen(ref tempR[0], ScanCorrect.GAIN_CORRECT_AIR_SFT_R, h_size, v_size);
            ScanCorrect.ImageOpen(ref tempL[0], ScanCorrect.GAIN_CORRECT_AIR_SFT_L, h_size, v_size);

            ScanCorrect.GetStatisticalInfo(ref tempR[0], h_size, v_size, invPix, v_size - invPix, invPix, h_size - invPix, ref minValR, ref maxValR, ref divR, ref meanR);
            ScanCorrect.GetStatisticalInfo(ref tempL[0], h_size, v_size, invPix, v_size - invPix, invPix, h_size - invPix, ref minValL, ref maxValL, ref divL, ref meanL);

            //ScanCorrect.GetStatisticalInfo(ref tempR[0], h_size, v_size, 12, 150, 12, 800, ref minValR, ref maxValR, ref divR, ref meanR);
            //ScanCorrect.GetStatisticalInfo(ref tempL[0], h_size, v_size, 900, 1000, 12, 800, ref minValL, ref maxValL, ref divL, ref meanL);

            ScanCorrect.ShiftFImageMagVal = meanR / meanL;

            float baseMean = (meanL + meanR) / 2.0f;
            ScanCorrect.ShiftFImageMagValL = baseMean / meanL;
            ScanCorrect.ShiftFImageMagValR = baseMean / meanR;

        }
        //*************************************************************************************************
        //機　　能： ゲイン校正判定用パラメータ取得
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： 偏差最小値、偏差最大値
        //補　　足： 
        //             
        //
        //履　　歴： v26.00  2017/01/06 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static int getJdgGainCorCond(ref float devMin, ref float devMax)
        {
            int ret = 0;

            try
            {
                //システムによって返す値を変更
                if (!CTSettings.detectorParam.Use_FlatPanel)//I.I.
                {
                    int IIno = modSeqComm.GetIINo();

                    // 9/6/4.5
                    if (CTSettings.infdef.Data.iifield[0].GetString().Substring(0) == "9")//1文字目が9なら9inchI.I.
                    {
                        switch (IIno)
                        {
                            case 0:
                                devMin = CTSettings.iniValue.gainCorDevRangeMin9inchII;
                                devMax = CTSettings.iniValue.gainCorDevRangeMax9inchII;
                                break;
                            case 1:
                                devMin = CTSettings.iniValue.gainCorDevRangeMin6inchII;
                                devMax = CTSettings.iniValue.gainCorDevRangeMax6inchII;
                                break;
                            case 2:
                                devMin = CTSettings.iniValue.gainCorDevRangeMin4_5inchII;
                                devMax = CTSettings.iniValue.gainCorDevRangeMax4_5inchII;
                                break;
                            default://ありえない
                                throw new Exception();
                        }
                    }
                    // 4/2
                    else if (CTSettings.infdef.Data.iifield[0].GetString().Substring(0) == "4")//1文字目が4なら4inchI.I.
                    {
                        switch (IIno)
                        {
                            case 0:
                                devMin = CTSettings.iniValue.gainCorDevRangeMin4inchII;
                                devMax = CTSettings.iniValue.gainCorDevRangeMax4inchII;
                                break;
                            case 2:
                                devMin = CTSettings.iniValue.gainCorDevRangeMin2inchII;
                                devMax = CTSettings.iniValue.gainCorDevRangeMax2inchII;
                                break;
                            default://ありえない
                                throw new Exception();
                        }
                    }
                }
                else//FPD
                {
                    if (CTSettings.detectorParam.h_size == 2048)//16inch
                    {
                        devMin = CTSettings.iniValue.gainCorDevRangeMin16inchFPD;
                        devMax = CTSettings.iniValue.gainCorDevRangeMax16inchFPD;
                    }
                    else if (CTSettings.detectorParam.h_size == 1024)//8inch
                    {
                        devMin = CTSettings.iniValue.gainCorDevRangeMin8inchFPD;
                        devMax = CTSettings.iniValue.gainCorDevRangeMax8inchFPD;
                    }
                    else
                    {
                        //ありえない
                        throw new Exception();
                    }
                }
            }
            catch
            {
                ret = 26006;
                return ret;
            }
            return ret;
        }
        //*************************************************************************************************
        //機　　能： オフセット校正判定用パラメータ取得
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： 偏差最小値、偏差最大値、平均最小値、平均最大値
        //補　　足： 
        //             
        //
        //履　　歴： v26.00  2017/01/06 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static int getJdgOffsetCorCond(ref float devMin, ref float devMax,ref float meanMin,ref float meanMax)
        {
            int ret = 0;

            try
            {
                //システムによって返す値を変更
                if (!CTSettings.detectorParam.Use_FlatPanel)//I.I.
                {
                    int IIno = modSeqComm.GetIINo();

                    // 9/6/4.5
                    if (CTSettings.infdef.Data.iifield[0].GetString().Substring(0) == "9")//1文字目が9なら9inchI.I.
                    {
                        switch (IIno)
                        {
                            case 0:
                                devMin = CTSettings.iniValue.offsetCorDevRangeMin9inchII;
                                devMax = CTSettings.iniValue.offsetCorDevRangeMax9inchII;
                                meanMin = CTSettings.iniValue.offsetCorMeanRangeMin9inchII;
                                meanMax = CTSettings.iniValue.offsetCorMeanRangeMax9inchII;
                                break;
                            case 1:
                                devMin = CTSettings.iniValue.offsetCorDevRangeMin6inchII;
                                devMax = CTSettings.iniValue.offsetCorDevRangeMax6inchII;
                                meanMin = CTSettings.iniValue.offsetCorMeanRangeMin6inchII;
                                meanMax = CTSettings.iniValue.offsetCorMeanRangeMax6inchII;
                                break;
                            case 2:
                                devMin = CTSettings.iniValue.offsetCorDevRangeMin4_5inchII;
                                devMax = CTSettings.iniValue.offsetCorDevRangeMax4_5inchII;
                                meanMin = CTSettings.iniValue.offsetCorMeanRangeMin4_5inchII;
                                meanMax = CTSettings.iniValue.offsetCorMeanRangeMax4_5inchII;
                                break;
                            default://ありえない
                                throw new Exception();
                        }
                    }
                    // 4/2
                    else if (CTSettings.infdef.Data.iifield[0].GetString().Substring(0) == "4")//1文字目が4なら4inchI.I.
                    {
                        switch (IIno)
                        {
                            case 0:
                                devMin = CTSettings.iniValue.offsetCorDevRangeMin4inchII;
                                devMax = CTSettings.iniValue.offsetCorDevRangeMax4inchII;
                                meanMin = CTSettings.iniValue.offsetCorMeanRangeMin4inchII;
                                meanMax = CTSettings.iniValue.offsetCorMeanRangeMax4inchII;
                                break;
                            case 2:
                                devMin = CTSettings.iniValue.offsetCorDevRangeMin2inchII;
                                devMax = CTSettings.iniValue.offsetCorDevRangeMax2inchII;
                                meanMin = CTSettings.iniValue.offsetCorMeanRangeMin2inchII;
                                meanMax = CTSettings.iniValue.offsetCorMeanRangeMax2inchII;
                                break;
                            default://ありえない
                                throw new Exception();
                        }
                    }
                }
                else//FPD
                {
                    if (CTSettings.detectorParam.h_size == 2048)//16inch
                    {
                        devMin = CTSettings.iniValue.offsetCorDevRangeMin16inchFPD;
                        devMax = CTSettings.iniValue.offsetCorDevRangeMax16inchFPD;
                        meanMin = CTSettings.iniValue.offsetCorMeanRangeMin16inchFPD;
                        meanMax = CTSettings.iniValue.offsetCorMeanRangeMax16inchFPD;
                    }
                    else if (CTSettings.detectorParam.h_size == 1024)//8inch
                    {
                        devMin = CTSettings.iniValue.offsetCorDevRangeMin8inchFPD;
                        devMax = CTSettings.iniValue.offsetCorDevRangeMax8inchFPD;
                        meanMin = CTSettings.iniValue.offsetCorMeanRangeMin8inchFPD;
                        meanMax = CTSettings.iniValue.offsetCorMeanRangeMax8inchFPD;
                    }
                    else
                    {
                        //ありえない
                        throw new Exception();
                    }
                }
            }
            catch
            {
                ret = 26005;
                return ret;
            }
            return ret;
        }
        //*************************************************************************************************
        //機　　能： スキャン位置校正判定用パラメータ取得
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： 傾き最小値、傾き最大値、切片最小値、切片最大値
        //補　　足： 
        //             
        //
        //履　　歴： v26.00  2017/01/06 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static int getJdgScanPosCorCond(ref float interceptMin, ref float interceptMax, ref float slopeMin, ref float slopeMax)
        {
            int ret = 0;

            try
            {
                //システムによって返す値を変更
                if (!CTSettings.detectorParam.Use_FlatPanel)//I.I.
                {
                    int IIno = modSeqComm.GetIINo();

                    // 9/6/4.5
                    if (CTSettings.infdef.Data.iifield[0].GetString().Substring(0) == "9")//1文字目が9なら9inchI.I.
                    {
                        switch (IIno)
                        {
                            case 0:
                                interceptMin = CTSettings.iniValue.interceptRangeMax9inchII;
                                interceptMax = CTSettings.iniValue.interceptRangeMin9inchII;
                                slopeMin = CTSettings.iniValue.slopeRangeMin9inchII;
                                slopeMax = CTSettings.iniValue.slopeRangeMax9inchII;
                                break;
                            case 1:
                                interceptMin = CTSettings.iniValue.interceptRangeMin6inchII;
                                interceptMax = CTSettings.iniValue.interceptRangeMax6inchII;
                                slopeMin = CTSettings.iniValue.slopeRangeMin6inchII;
                                slopeMax = CTSettings.iniValue.slopeRangeMax6inchII;
                                break;
                            case 2:
                                interceptMin = CTSettings.iniValue.interceptRangeMin4_5inchII;
                                interceptMax = CTSettings.iniValue.interceptRangeMax4_5inchII;
                                slopeMin = CTSettings.iniValue.slopeRangeMin4_5inchII;
                                slopeMax = CTSettings.iniValue.slopeRangeMax4_5inchII;
                                break;
                            default://ありえない
                                throw new Exception();
                        }
                    }
                    // 4/2
                    else if (CTSettings.infdef.Data.iifield[0].GetString().Substring(0) == "4")//1文字目が4なら4inchI.I.
                    {
                        switch (IIno)
                        {
                            case 0:
                                interceptMin = CTSettings.iniValue.interceptRangeMin4inchII;
                                interceptMax = CTSettings.iniValue.interceptRangeMax4inchII;
                                slopeMin = CTSettings.iniValue.slopeRangeMin4inchII;
                                slopeMax = CTSettings.iniValue.slopeRangeMax4inchII;
                                break;
                            case 2:
                                interceptMin = CTSettings.iniValue.interceptRangeMin2inchII;
                                interceptMax = CTSettings.iniValue.interceptRangeMax2inchII;
                                slopeMin = CTSettings.iniValue.slopeRangeMin2inchII;
                                slopeMax = CTSettings.iniValue.slopeRangeMax2inchII;
                                break;
                            default://ありえない
                                throw new Exception();
                        }
                    }
                }
                else//FPD
                {
                    if (CTSettings.detectorParam.h_size == 2048)//16inch
                    {
                        interceptMin = CTSettings.iniValue.interceptRangeMin16inchFPD;
                        interceptMax = CTSettings.iniValue.interceptRangeMax16inchFPD;
                        slopeMin = CTSettings.iniValue.slopeRangeMin16inchFPD;
                        slopeMax = CTSettings.iniValue.slopeRangeMax16inchFPD;
                    }
                    else if (CTSettings.detectorParam.h_size == 1024)//8inch
                    {
                        interceptMin = CTSettings.iniValue.interceptRangeMin8inchFPD;
                        interceptMax = CTSettings.iniValue.interceptRangeMax8inchFPD;
                        slopeMin = CTSettings.iniValue.slopeRangeMin8inchFPD;
                        slopeMax = CTSettings.iniValue.slopeRangeMax8inchFPD;
                    }
                    else
                    {
                        //ありえない
                        throw new Exception();
                    }
                }
            }
            catch
            {
                ret = 26010;
                return ret;
            }
            return ret;
        }
        //*************************************************************************************************
        //機　　能： ゲイン校正データ保存用
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： 
        //補　　足： ゲイン校正のデータ収集を事前に行っておくこと。
        //             
        //
        //履　　歴： v26.00  2017/01/06 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static void gainCorSave()
        {
            int iret = 0;

        	switch (CTSettings.detectorParam.DetType)
            {
				case DetectorConstants.DetTypeII:
				case DetectorConstants.DetTypeHama:
                    ScanCorrect.ImageSave(ref ScanCorrect.GAIN_IMAGE[0], ScanCorrect.GAIN_CORRECT, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
					break;
				case DetectorConstants.DetTypePke:
                    if (modScanCorrect.Flg_GainShiftScan == CheckState.Checked)
                    {
                        IICorrect.ImageSave_long(ref ScanCorrect.Gain_Image_L_SFT_R[0], ScanCorrect.GAIN_CORRECT_L_SFT_R, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                        if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
                        {
                            IICorrect.ImageSave_long(ref ScanCorrect.Gain_Image_L_SFT_L[0], ScanCorrect.GAIN_CORRECT_L_SFT_L, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                        }
                    }
                    IICorrect.ImageSave_long(ref ScanCorrect.Gain_Image_L[0], ScanCorrect.GAIN_CORRECT_L, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                    
                    //Rev25.00 関数化 by長野 2016/12/16
                    ScanCorrect.calShiftImageMagVal();
                   
#if (!NoCamera) //'v17.02変更 byやまおか 2010/07/15
                    iret = Pulsar.PkeSetGainData(Pulsar.hPke, ScanCorrect.Gain_Image_L, 1, "");
                    

                    //ストリングテーブル化　'v17.60 by 長野　2011/05/22
                    ////If ret = 1 Then MsgBox "ゲイン校正データをセットできませんでした。", vbCritical
                    //if (Ipc32v5.ret == 1)
                    if (iret == 1)
                    {
                        MessageBox.Show(CTResources.LoadResString(20004), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
#endif
					
                    break;
                default:
                    break;
			}

			//メカの移動有無プロパティをリセットする
			modSeqComm.SeqBitWrite("GainIIChangeReset", true);

			//mecainf（コモン）の更新
            gainCorUpdateMecainf();
        }
        //*******************************************************************************
        //機　　能： ゲイン校正を自動で判定した場合のmecainf（コモン）の更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： ゲイン校正関連のパラメータのみ更新
        //
        //履　　歴： v26.00  17/01/06  (検S1)長野    新規作成
        //*******************************************************************************
        public static void gainCorUpdateMecainf()
        {
            MecaInf theMecainf = new MecaInf();
            theMecainf.Data.Initialize();

            //mecainf（コモン）取得
            theMecainf.Load();

            //ゲイン校正を行ったときの管電圧
            if (CTSettings.scaninh.Data.xray_remote == 0)
            {
                if (modScanCorrect.Flg_GainShiftScan == CheckState.Checked)
                {
                    theMecainf.Data.gain_kv_sft = Convert.ToInt32(frmXrayControl.Instance.ntbSetVolt.Value);                    //シフト用
                }
                else
                {
                    theMecainf.Data.gain_kv = (float)frmXrayControl.Instance.ntbSetVolt.Value;
                }
            }

            theMecainf.Data.gain_ma = (float)modScanCorrectNew.GainCurrent;//v19.00復活（開放管はターゲット電流、密封管はフィードバック値とする） by長野 2012/05/10

            if (modScanCorrect.Flg_GainShiftScan == CheckState.Checked)
            {
                theMecainf.Data.gain_filter_sft = modSeqComm.GetFilterIndex();
                //シフト用
            }
            else
            {
                theMecainf.Data.gain_filter = modSeqComm.GetFilterIndex();
            }

            //ゲイン校正を行ったときのI.I.視野
            if (modScanCorrect.Flg_GainShiftScan == CheckState.Checked)
            {
                theMecainf.Data.gain_iifield_sft = modSeqComm.GetIINo();                //シフト用
            }
            else
            {
                theMecainf.Data.gain_iifield = modSeqComm.GetIINo();
            }

            //ゲイン校正を行ったときのＸ線管
            if (modScanCorrect.Flg_GainShiftScan == CheckState.Checked)
            {
                theMecainf.Data.gain_mt_sft = CTSettings.scansel.Data.multi_tube;                //シフト用
            }
            else
            {
                theMecainf.Data.gain_mt = CTSettings.scansel.Data.multi_tube;
            }

            //ゲイン校正を行ったときのビニングモード：0(1×1)，1(2×2)，2(4×4)
            if (modScanCorrect.Flg_GainShiftScan == CheckState.Checked)
            {
                theMecainf.Data.gain_bin_sft = CTSettings.scansel.Data.binning;
                //シフト用
            }
            else
            {
                theMecainf.Data.gain_bin = CTSettings.scansel.Data.binning;
            }

            //ゲイン校正を行った時の年月日               'v12.01追加 by 間々田 2006/12/04
            if (modScanCorrect.Flg_GainShiftScan == CheckState.Checked)
            {
                theMecainf.Data.gain_date_sft = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));         //YYYYMMDD形式   'シフト用
            }
            else
            {
                theMecainf.Data.gain_date = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));             //YYYYMMDD形式
            }

            //ゲイン校正を行ったときのFPDゲイン      'v17.00追加 byやまおか 2010/02/22
            if (modScanCorrect.Flg_GainShiftScan == CheckState.Checked)
            {
                theMecainf.Data.gain_fpd_gain_sft = CTSettings.scansel.Data.fpd_gain;                //シフト用
            }
            else
            {
                theMecainf.Data.gain_fpd_gain = CTSettings.scansel.Data.fpd_gain;
            }

            //ゲイン校正を行ったときのFPD積分時間    'v17.00追加 byやまおか 2010/02/22
            if (modScanCorrect.Flg_GainShiftScan == CheckState.Checked)
            {
                theMecainf.Data.gain_fpd_integ_sft = CTSettings.scansel.Data.fpd_integ;
                //シフト用
            }
            else
            {
                theMecainf.Data.gain_fpd_integ = CTSettings.scansel.Data.fpd_integ;
            }

            //ゲイン校正を行ったときの年月日時       'v17.00追加 byやまおか 2010/03/04
            if (modScanCorrect.Flg_GainShiftScan == CheckState.Checked)
            {
                theMecainf.Data.gain_time_sft = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
                //HHMMSS形式 'シフト用
            }
            else
            {
                theMecainf.Data.gain_time = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
                //HHMMSS形式
            }

            //ゲイン校正を行ったときの焦点           'v18.00追加 byやまおか 2011/06/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (modScanCorrect.Flg_GainShiftScan == CheckState.Checked)
            {
                theMecainf.Data.gain_focus_sft = CTSettings.mecainf.Data.xfocus;
                //シフト用
            }
            else
            {
                theMecainf.Data.gain_focus = CTSettings.mecainf.Data.xfocus;
            }

            //mecainf（コモン）更新
            theMecainf.Write();

        }
        //*************************************************************************************************
        //機　　能： オフセット校正データ保存用
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： 
        //補　　足： オフセット校正のデータ収集を先に行っておくこと。
        //             
        //
        //履　　歴： v26.00  2017/01/06 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static void offsetCorSave()
        {

            int iret = 0;

            //オフセット校正画像の保存
            ScanCorrect.DoubleImageSave(ref ScanCorrect.OFFSET_IMAGE[0], ScanCorrect.OFF_CORRECT, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
#if !(NoCamera)
            //v17.00追加 byやまおか 2010/02/15
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))
            {
                //新しい校正画像をプリロードする 2010-01-06　山本
                if (ScanCorrect.AutoCorFlag == 0)   //手動校正中はプリロードする
                {
                    iret = ScanCorrect.PkeSetOffsetData((int)Pulsar.hPke, 1, ref ScanCorrect.OFFSET_IMAGE[0], 1);
                }
                else	//自動校正中はプリロードしない
                {
                    iret = ScanCorrect.PkeSetOffsetData((int)Pulsar.hPke, 1, ref ScanCorrect.OFFSET_IMAGE[0], 0);
                }
                if (iret == 1)
                {
                    //MsgBox "オフセット校正データをセットできませんでした。", vbCritical
                    MessageBox.Show(CTResources.LoadResString(20004), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);   //ストリングテーブル化 '17.60 by長野 2011/05/22
                }
            }
#endif
            //オフセット校正パラメータをコモンに書き込む
            //Call Set_Offset_Parameter      'v11.2削除 by 間々田 2005/10/17

            //mecainf（コモン）の更新
            offsetCorUpdateMecainf();    //v11.2追加 by 間々田 2005/10/17　上記 Set_Offset_Parameter と同じ処理

        }
        //*******************************************************************************
        //機　　能： mecainf（コモン）の更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： オフセット校正関連のパラメータのみ更新する
        //
        //履　　歴： v11.2  05/10/17  (SI3)間々田    新規作成
        //*******************************************************************************
        private static void offsetCorUpdateMecainf()
        {
            //mecainfType theMecainf = default(mecainfType);
            MecaInf theMecainf = new MecaInf();
            theMecainf.Data.Initialize();

            //mecainf（コモン）取得
            //modMecainf.GetMecainf(ref theMecainf);
            theMecainf.Load();

            //オフセット校正を行った時の年月日
            theMecainf.Data.off_date = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));   //YYYYMMDD形式

            //オフセット校正を行った時のビニングモード
            theMecainf.Data.off_bin = CTSettings.scansel.Data.binning;                            //0:１×１，1:２×２，2:４×４

            //オフセット校正を行ったときのFPDゲイン      'v17.00追加 byやまおか 2010/02/22
            theMecainf.Data.off_fpd_gain = CTSettings.scansel.Data.fpd_gain;

            //オフセット校正を行ったときのFPD積分時間    'v17.00追加 byやまおか 2010/02/22
            theMecainf.Data.off_fpd_integ = CTSettings.scansel.Data.fpd_integ;

            //オフセット校正を行ったときの時間           'v17.00追加 byやまおか 2010/03/04
            theMecainf.Data.off_time = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));     //HHMMSS形式

            //mecainf（コモン）更新
            //modMecainf.PutMecainf(ref theMecainf);
            theMecainf.Write();
        }
        //*************************************************************************************************
        //機　　能： スキャン位置校正データ保存用
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： 
        //補　　足： スキャン位置の傾き、切片を事前に求めておくこと。
        //             
        //
        //履　　歴： v26.00  2017/01/06 M.Chouno      新規作成
        //          
        //*************************************************************************************************
        public static void scanposCorSave(float myA,float myB)
        {
            //メカの移動有無プロパティをリセットする
            modSeqComm.SeqBitWrite("SPIIChangeReset", true);

            //scancondpar（コモン）の更新
            scanPosUpdateScancondpar(myA,myB);

            //mecainf（コモン）の更新
            scanPosUpdateMecainf();

            //scan_posi.csv の更新
            modCT30K.UpdateCsv(AppValue.SCANPOSI_CSV, "scan_posi_a[2]", CTSettings.scancondpar.Data.scan_posi_a[2].ToString("0.0000"));
            modCT30K.UpdateCsv(AppValue.SCANPOSI_CSV, "scan_posi_b[2]", CTSettings.scancondpar.Data.scan_posi_b[2].ToString("0.0000"));//ｽｷｬﾝ位置の切片
        }
        //*******************************************************************************
        //機　　能： scancondpar（コモン）の更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： スキャン位置校正関連のパラメータのみ更新
        //
        //履　　歴： v11.2  05/10/17  (SI3)間々田    新規作成・２次元幾何歪校正対応
        //*******************************************************************************
		public static void scanPosUpdateScancondpar(float myA,float myB)
		{
			double FlatPanel_dpm = 0;
			float FlatPanel_e_dash = 0;
			float FlatPanel_Ils = 0;
			float FlatPanel_Ile = 0;
			float FlatPanel_kv = 0;
			float FlatPanel_bb0 = 0;
			float FlatPanel_Dpi = 0;
			float FlatPanel_FGD = 0;
			double a_0 = 0;
			double b_0 = 0;
			double a0_bar = 0;
			double b0_bar = 0;
			int xls = 0;
			int xle = 0;
			double a0 = 0;
			double A1 = 0;
			double a2 = 0;
			double a3 = 0;
			double a4 = 0;
			double a5 = 0;
			float kv = 0;
			float bb0 = 0;
			float FGD = 0;
			float jc = 0;
			float Dpi = 0;
			float e_dash = 0;
			float Delta_Ip = 0;
			float Ils = 0;
			float Ile = 0;
			double dpm = 0;

			//v17.00追加(ここから)　山本 2010-02-06
            if (CTSettings.detectorParam.Use_FlatPanel)
            {
                CTSettings.scancondpar.Data.scan_posi_a[2] = myA + CTSettings.scancondpar.Data.rc_slope_ft;   //傾き
                CTSettings.scancondpar.Data.scan_posi_b[2] = myB;	                                        //切片
                CTSettings.scancondpar.Data.cone_scan_posi_a = myA + CTSettings.scancondpar.Data.rc_slope_ft;
                CTSettings.scancondpar.Data.cone_scan_posi_b = myB;

				//v19.10　v19.20の反映 フラットパネルも傾きを使用してdpmを計算しているので、dpmの再計算が必要 2012/11/28 by長野
                ScanCorrect.Cal_ConeBeamParameter((CTSettings.scaninh.Data.full_distortion == 0 ? CTSettings.scancondpar.Data.alk[1] : ScanCorrect.B1),
                                                    CTSettings.scancondpar.Data.cone_scan_posi_a, 
                                                    ref FlatPanel_dpm);
				//v11.2変更 by 間々田 2005/10/06
                CTSettings.scancondpar.Data.dpm = Convert.ToSingle(FlatPanel_dpm);
				//v19.10 v19.20の反映 cone_max_mfanangleが更新されないとmcが正しく求まらないため、ここで更新 2012/11/28 by長野
                FlatPanel_FGD = ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[modCT30K.GetFcdOffsetIndex()];
				FlatPanel_bb0 = ScanCorrect.GVal_ScanPosiA[2];
                FlatPanel_kv = CTSettings.detectorParam.vm / CTSettings.detectorParam.hm;
				FlatPanel_Dpi = (float)(10 / ScanCorrect.B1);
				FlatPanel_e_dash = (float)(Math.Atan(FlatPanel_kv * FlatPanel_bb0));

                //変更2014/10/07hata_v19.51反映
                ////v19.12 ±8に変更 by長野 2013/03/06
                //FlatPanel_Ils = CTSettings.scancondpar.Data.ist + 8;
                //FlatPanel_Ile = CTSettings.scancondpar.Data.ied - 8;
                ////FlatPanel_Ils = .ist + 2
                ////FlatPanel_Ile = .ied - 2
                if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
                {
                    if (CTSettings.detectorParam.h_size == 1024)
                    {
                        FlatPanel_Ils = CTSettings.scancondpar.Data.ist + 12;
                        FlatPanel_Ile = CTSettings.scancondpar.Data.ied - 12;
                    }
                    else if (CTSettings.detectorParam.h_size == 2048)
                    {
                        FlatPanel_Ils = CTSettings.scancondpar.Data.ist + 24;
                        FlatPanel_Ile = CTSettings.scancondpar.Data.ied - 24;
                    }
                }
                else
                {
                    FlatPanel_Ils = CTSettings.scancondpar.Data.ist + 2;
                    FlatPanel_Ile = CTSettings.scancondpar.Data.ied - 2;
                }

                CTSettings.scancondpar.Data.cone_max_mfanangle =Convert.ToSingle(2 * Math.Atan((FlatPanel_Ile - FlatPanel_Ils) * FlatPanel_Dpi / Math.Cos(FlatPanel_e_dash) * 1.02 * 0.5 / FlatPanel_FGD));
            
            }
            else //v17.00追加(ここまで)　山本 2010-02-06
            {
				//２次元幾何歪補正の場合
                if (CTSettings.scaninh.Data.full_distortion == 0)
                {
					//Dim kmax    As Long    'deleted by 山本　2005-12-21 Public宣言に変更

					//hole_dataの読み込み
					ScanCorrect.kmax = ScanCorrect.ReadHole();

					//a_0 = GVal_ScanPosiB(2) - GVal_ScanPosiA(2) * (h_size - 1) / 2 + (v_size - 1) / 2   'changed by 山本　2005-12-6
					//b_0 = GVal_ScanPosiA(2)
                    //2014/11/07hata キャストの修正
                    //a_0 = myB - myA * (CTSettings.detectorParam.h_size - 1) / 2 + (CTSettings.detectorParam.v_size - 1) / 2;
                    a_0 = myB - myA * (CTSettings.detectorParam.h_size - 1) / 2D + (CTSettings.detectorParam.v_size - 1) / 2D;
					//changed by 山本　2005-12-6
					b_0 = myA;
                    ScanCorrect.Cal_1d_Distortion_Parameter(Convert.ToInt32(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm), CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, a_0, b_0, ref CTSettings.scancondpar.Data.alk[0], ref CTSettings.scancondpar.Data.blk[0], ScanCorrect.kmax, ref ScanCorrect.gx[0], ref ScanCorrect.gy[0],
					                                        ref ScanCorrect.gg[0], ref ScanCorrect.gh[0], ref a0_bar, ref b0_bar, ref xls, ref xle, ref a0, ref A1, ref a2, ref a3, ref a4, ref a5);

                    CTSettings.scancondpar.Data.scan_posi_a[2] = Convert.ToSingle(b0_bar);                                                       //changed by 山本　2005-12-7
                    //2014/11/07hata キャストの修正
                    //CTSettings.scancondpar.Data.scan_posi_b[2] = Convert.ToSingle(a0_bar + b0_bar * (CTSettings.detectorParam.h_size - 1) / 2 - (CTSettings.detectorParam.v_size - 1) / 2);  //changed by 山本　2005-12-6
                    CTSettings.scancondpar.Data.scan_posi_b[2] = Convert.ToSingle(a0_bar + b0_bar * (CTSettings.detectorParam.h_size - 1) / 2D - (CTSettings.detectorParam.v_size - 1) / 2D);  //changed by 山本　2005-12-6
					
					//端のデータを使用しないための処理　added by 山本　2005-12-13
					if (xls < 4)
                    {
						xls = 4;
                    }
                    if (xle > CTSettings.detectorParam.h_size - 5)
                    {
                        xle = CTSettings.detectorParam.h_size - 5;
                    }

                    //CTSettings.scancondpar.Data.a[0, 2] = a0;    //scancondpar.a2,0のこと
                    //CTSettings.scancondpar.Data.a[1, 2] = A1;    //scancondpar.a2,1のこと
                    //CTSettings.scancondpar.Data.a[2, 2] = a2;    //scancondpar.a2,2のこと
                    //CTSettings.scancondpar.Data.a[3, 2] = a3;    //scancondpar.a2,3のこと
                    //CTSettings.scancondpar.Data.a[4, 2] = a4;    //scancondpar.a2,4のこと
                    //CTSettings.scancondpar.Data.a[5, 2] = a5;    //scancondpar.a2,5のこと
                    CTSettings.scancondpar.Data.a[2 * 6 + 0] = Convert.ToSingle(a0);    //scancondpar.a2,0のこと
                    CTSettings.scancondpar.Data.a[2 * 6 + 1] = Convert.ToSingle(A1);    //scancondpar.a2,1のこと
                    CTSettings.scancondpar.Data.a[2 * 6 + 2] = Convert.ToSingle(a2);    //scancondpar.a2,2のこと
                    CTSettings.scancondpar.Data.a[2 * 6 + 3] = Convert.ToSingle(a3);    //scancondpar.a2,3のこと
                    CTSettings.scancondpar.Data.a[2 * 6 + 4] = Convert.ToSingle(a4);    //scancondpar.a2,4のこと
                    CTSettings.scancondpar.Data.a[2 * 6 + 5] = Convert.ToSingle(a5);    //scancondpar.a2,5のこと
                    
                    CTSettings.scancondpar.Data.xls[2] = xls;
                    CTSettings.scancondpar.Data.xle[2] = xle;
                    CTSettings.scancondpar.Data.mdtpitch[2] = Convert.ToSingle(10 / A1);

					//最大ファン角を求める為の変数

					//最大ファン角の計算
                    kv = CTSettings.detectorParam.vm / CTSettings.detectorParam.hm;
					//bb0 = GVal_ScanPosiA(2)
					bb0 = myA;

                    FGD = ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[modCT30K.GetFcdOffsetIndex()];

                    //2014/11/07hata キャストの修正
                    //jc = (CTSettings.detectorParam.v_size - 1) / 2;
                    jc = (float)(CTSettings.detectorParam.v_size - 1) / 2F;
					Dpi = (float)(10 / A1);
                    e_dash = (float)(Math.Atan(kv * bb0));
                    Delta_Ip = (float)(Math.Floor(2 + jc * kv * kv * Math.Abs(bb0)));

					Ils = Delta_Ip;
                    Ile = CTSettings.detectorParam.h_size - Delta_Ip - 1;
                    CTSettings.scancondpar.Data.max_mfanangle[2] = Convert.ToSingle(2 * Math.Atan((Ile - Ils) * Dpi / Math.Cos(e_dash) * 1.02 * 0.5 / FGD));

					//コーンビームスキャンが可能な場合
                    if (CTSettings.scaninh.Data.data_mode[2] == 0)
                    {
						//最大ファン角の計算
						Dpi = (float)(10 / ScanCorrect.B1);
                        Ils = CTSettings.scancondpar.Data.ist + 2;
                        Ile = CTSettings.scancondpar.Data.ied - 2;
                        CTSettings.scancondpar.Data.cone_max_mfanangle = Convert.ToSingle(2 * System.Math.Atan((Ile - Ils) * Dpi / System.Math.Cos(e_dash) * 1.02 * 0.5 / FGD));

						//Cal_ConeBeamParameter B1, GVal_ScanPosiA(2), dpm
						ScanCorrect.Cal_ConeBeamParameter(ScanCorrect.B1, myA, ref dpm);
                        CTSettings.scancondpar.Data.dpm =Convert.ToSingle(dpm);

						//.cone_scan_posi_a = GVal_ScanPosiA(2) + .rc_slope_ft
						//.cone_scan_posi_b = GVal_ScanPosiB(2)
                        CTSettings.scancondpar.Data.cone_scan_posi_a = myA + CTSettings.scancondpar.Data.rc_slope_ft;
                        CTSettings.scancondpar.Data.cone_scan_posi_b = myB;
					}
				}
                //１次元幾何歪補正の場合
                else
                {
					//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
					//.scan_posi_a(2) = GVal_ScanPosiA(2) + .rc_slope_ft  '傾き   rc_slope_ft:コーンの傾き調整パラメータ
					//.scan_posi_b(2) = GVal_ScanPosiB(2)                 '切片
					//                    .scan_posi_a(2) = myA + .rc_slope_ft  '傾き   rc_slope_ft:コーンの傾き調整パラメータ
					//                    .scan_posi_b(2) = myB                 '切片
					//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
				}
			}
			//v17.02 if追加 byやまおか 2010/07/28

			//Scancondpar（コモン）の書き込み
			//modScancondpar.CallPutScancondpar();
            CTSettings.scancondpar.Write();

			//透視画像上のスライスラインを更新する   'v15.0変更 by 間々田 2009/06/17
			//if (modLibrary.IsExistForm(frmTransImage.Instance))
            if (modLibrary.IsExistForm("frmTransImage"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
            {
				frmTransImage.Instance.SetLine();
			}
		}

        //*******************************************************************************
        //機　　能： mecainf（コモン）の更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： スキャン位置校正関連のパラメータのみ更新
        //
        //履　　歴： v11.2  05/10/17  (SI3)間々田    新規作成
        //*******************************************************************************
		public static void scanPosUpdateMecainf()
		{
			//mecainfType theMecainf = default(mecainfType);
            MecaInf theMecainf = new MecaInf();
            theMecainf.Data.Initialize();

			//mecainf（コモン）取得
			//modMecainf.GetMecainf(ref theMecainf);
            theMecainf.Load();

			//I.I.視野
			theMecainf.Data.sp_iifield = modSeqComm.GetIINo();

			//Ｘ線管
            theMecainf.Data.sp_mt = CTSettings.scansel.Data.multi_tube;

			//ビニングモード
            theMecainf.Data.sp_bin = CTSettings.scansel.Data.binning;//0:１×１，1:２×２，2:４×４

			//スキャン位置校正を行った時の年月日   'v17.00追加 byやまおか 2010/03/02
            theMecainf.Data.sp_date = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));    //YYYYMMDD形式

			//スキャン位置校正を行った時の時間     'v17.00追加 byやまおか 2010/03/04
            theMecainf.Data.sp_time = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));      //HHMMSS形式

			//mecainf（コモン）更新
			//modMecainf.PutMecainf(ref theMecainf);
            theMecainf.Write();

        }
        //*******************************************************************************
        //機　　能： 未完な校正を行う
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： true:成功、false;失敗
        //
        //補　　足　 回転中心校正未完→オートセンタリング、寸法校正未完→対象外
        //
        //履　　歴： v26.00  17/01/06  (検S1)長野    新規作成
        //*******************************************************************************
        public static bool autoCorBeforeScanStart()
        {
            bool ret = false;
            string lblsts = "";

            modCT30K.ShowMessage(CTResources.LoadResString(26012));

            //■回転中心校正
            //未完ならオートセンタリングにする
            lblsts = frmScanControl.Instance.lblStatus[3].Text;
            if (lblsts != StringTable.GC_STS_STANDBY_OK &&
                lblsts != StringTable.GC_STS_NORMAL_OK &&
                lblsts != StringTable.GC_STS_CONE_OK)
            {
                frmScanControl.Instance.chkInhibit[3].CheckState = CheckState.Unchecked;
            }
            
            //■寸法校正
            //未完なら対象外にする
            lblsts = frmScanControl.Instance.lblStatus[5].Text;
            if (lblsts != StringTable.GC_STS_STANDBY_OK)
            {
                frmScanControl.Instance.chkInhibit[5].CheckState = CheckState.Unchecked;
            }

            //Rev99.99
            frmScanControl.Instance.setScanAreaAndCondIgnoreFlg(true);
            //frmAutoCorrectionのDoAutoCaptureだけを実行
            frmAutoCorrection.Instance.initialize();
            if (frmAutoCorrection.Instance.DoAutoCapture(true))
            {
                ret = true;
            }
            //Rev99.99
            frmScanControl.Instance.setScanAreaAndCondIgnoreFlg(false);

            frmCorrectionStatus.Instance.MyUpdate();

            frmAutoCorrection.Instance.Terminated();
            frmAutoCorrection.Instance.Dispose();

            modCT30K.HideMessage();

            return ret;
        }
	}
}
