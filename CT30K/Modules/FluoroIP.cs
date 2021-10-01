using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
//
using CTAPI;
using CT30K.Common;

namespace CT30K
{
    ///* *************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0                */
    ///* 客先　　　　： ?????? 殿                                                    */
    ///* プログラム名： FluoroIP.bas                                                 */
    ///* 処理概要　　： 透視画像処理共通モジュール                                   */
    ///* 注意事項　　： なし                                                         */
    ///* --------------------------------------------------------------------------  */
    ///* 適用計算機　： DOS/V PC                                                     */
    ///* ＯＳ　　　　： Windows 2000  (SP4)                                          */
    ///* コンパイラ　： VB 6.0                                                       */
    ///* --------------------------------------------------------------------------  */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                  */
    ///*                                                                             */
    ///* V1.00       00/XX/XX    (TOSFEC) ????????   新規作成                        */
    ///* V3.0        00/08/01    (TOSFEC) 鈴山　修   ｺｰﾝﾋﾞｰﾑCT対応                   */
    ///* V3.0        00/09/08    (TOSFEC) 鈴山　修   "Option Explicit"を指定         */
    ///* V11.3       06/02/24    (SI3) 間々田        動画保存対応                    */
    ///*                                                                             */
    ///* --------------------------------------------------------------------------  */
    ///* ご注意：                                                                    */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。    */
    ///*                                                                             */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                  */
    ///* *************************************************************************** */
	internal static class FluoroIP
	{
        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************

        #region　//CTSettingsへ移動
        //CTSettingsへ移動
        /*        
        //画像処理フラグ		
		public static bool IntegOn;	            //積算中フラグ		
		public static bool AverageOn;	        //アベレージングフラグ		
		public static bool SharpOn;	            //シャープフラグ		
		public static int MovieSaveTime;	    //動画保存時間                   'v11.3追加 by 間々田 2006/01/27		
        public static int gFrameRateIndex;	    //フレームレートのインデックス値 'v11.3追加 by 間々田 2006/01/27

        //透視画像処理用設定値		
		public static int AverageNum;	        //アベレージ枚数		
		public static int FIntegNum;	        //積算枚数（積分枚数）		
		public static int EdgeFilterNo;	        //エッジフィルタ番号保存用		
		public static int DiffFilterNo;	        //微分フィルタ番号保存用

		public const int IMAGE_BIT8 = 0;
		public const int IMAGE_BIT16 = 1;

		public const int M_SYNCHRONOUS = 1;
		public const int M_ASYNCHRONOUS = 2;


        //********************************************************************************
        //  外部関数宣言
        //********************************************************************************
        //アベレージング関数
		[DllImport("ImgProc.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void Averaging(ref short Image_L, 
                                            ref short Image_Ave, 
                                            int SIZE, 
                                            int AverageNum);

        //空間フィルタをかける関数                   'v15変更 IICorrect.dll → ImgProc.dll by 間々田 2009/06/16
		[DllImport("ImgProc.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void SpatialFilter(ref short IN_IMAGE, 
                                                ref short OUT_IMAGE, 
                                                ref float filter, 
                                                int h_size, 
                                                int v_size, 
                                                int K_Size);

        //シャープフィルタ専用関数（速度アップのため）'v15追加 by 間々田 2009/06/16
        //Public Declare Sub Sharpen Lib "ImgProc.dll" _
        //'    (ByRef IN_IMAGE As Integer, _
        //'    ByRef OUT_IMAGE As Integer, _
        //'    ByVal h_size As Long, _
        //'    ByVal v_size As Long _
        //')
		[DllImport("ImgProc.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void Sharpen(ref short IN_IMAGE, 
                                          ref short OUT_IMAGE, 
                                          int h_size, 
                                          int v_size, 
                                          int maxValue);		//v17.10変更 maxValue追加 byやまおか 2010/08/10

        //画像積算関数（積分用）
		[DllImport("ImgProc.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void AddImageWord(ref short IN_IMAGE, 
                                               ref int SUM_IMAGE, 
                                               int SIZE);

        //画像を除算する（積分用）
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void DivImage(ref int M_IMAGE, 
                                           ref short L_IMAGE, 
                                           int h_size, 
                                           int v_size, 
                                           int N);
        
        //画像白黒反転関数   'changed by 山本　2003-10-28　引数にadvを追加
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void InverseImage(ref short Image, 
                                               int h_size, 
                                               int v_size, 
                                               int adv);

        //DICOM変換関数                                                  'V6.0 append by 間々田 2002/08/07
		[DllImport("DICOMTransfer.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int DICOM_Transfer(string LoadFileName, 
                                                string SaveFileDir, 
                                                string PatientName, 
                                                string InstitutionName, 
                                                string PatientComments);

        //v7.0 追加 by 間々田 2003/09/25
		[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void ChangeImageSize_HV(ref short IN_IMAGE, 
                                                     ref short OUT_IMAGE, 
                                                     int h_size, 
                                                     int v_size, 
                                                     float mag_h, 
                                                     float mag_v);


		public static void InitFluoroIP()
		{
            CTSettings.scanParam.EdgeFilterNo = 3;		//エッジフィルタ番号デフォルト値
            CTSettings.scanParam.DiffFilterNo = 3;		//微分フィルタ番号デフォルト値
            CTSettings.scanParam.FIntegNum = 50;			//積分枚数のデフォルト値

			//動画保存時間：初期値は10秒
            CTSettings.scanParam.MovieSaveTime = 10;

			//ライブ画像処理：アベレージング枚数のデフォルト値
            CTSettings.scanParam.AverageNum = 5;

			//ライブ画像処理：透視画像動画保存時のフレームレートのインデックス値
            CTSettings.scanParam.gFrameRateIndex = 2;
		}

        */
        #endregion　//CTSettingsへ移動

    }
}
