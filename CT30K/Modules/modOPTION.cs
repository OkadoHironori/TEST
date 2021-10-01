using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using CT30K.Common;
using CTAPI;
using TransImage;
using CT30K.Properties;
//using CT30K.Controls;
//using CT30K.Modules;

namespace CT30K
{
	static class modOPTION
	{
///* ************************************************************************** */
///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
///* 客先　　　　： ?????? 殿                                                   */
///* プログラム名： modOPTION.bas                                               */
///* 処理概要　　： オプションデータ関連                                        */
///* 注意事項　　： なし                                                        */
///* -------------------------------------------------------------------------- */
///* 適用計算機　： DOS/V PC                                                    */
///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
///* コンパイラ　： VB 6.0                                                      */
///* -------------------------------------------------------------------------- */
///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
///*                                                                            */
///* V4.0        01/03/26    (ITC)    鈴山　修   新規作成                       */
///* v11.2     2006/01/17    (SI3)    間々田     不要コメントを削除             */
///* -------------------------------------------------------------------------- */
///* ご注意：                                                                   */
///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
///*                                                                            */
///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
///* ************************************************************************** */


        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************

        //オプション設定ファイルチェック用フラグ(True:有,False:無)

        //プロフィールディスタンス
		public static bool OptPRDFlag;
		//疑似カラー
		public static bool OptColorFlag;
		//スライスプラン
		//public static bool OptSlicePlanFlag;
		//和画像
		public static bool OptAdditionFlag;
		//差画像
		public static bool OptSubtractionFlag;
		//単純拡大
		public static bool OptEnlargeFlag;
		//ROI処理
		public static bool OptROIFlag;
		//寸法測定
		public static bool OptDistanceFlag;
		//プロフィール
		public static bool OptProfileFlag;
		//ヒストグラム
		public static bool OptHistgramFlag;
		//CT値表示
		public static bool OptCTDumpFlag;
		//マルチフレーム
		public static bool OptMultiFrameFlag;
		//付帯情報修正
		public static bool OptPictureRetouchFlag;
		//骨塩定量解析           'v6.0追加 by 間々田
		public static bool OptBoneDensityFlag;
		//DICOM変換              'added by 山本 2002-10-5
		public static bool OptDICOMFlag;
		//フィルタ処理           'v10.2追加 by 間々田 2005/06/21
		public static bool OptFilterFlag;
		//画像フォーマット変換   'v11.5追加 by 間々田 2005/09/01
		public static bool OptFormatTransferFlag;



        //********************************************************************************
        //機    能  ：  オプション設定ファイルをチェックする。
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  オプション設定ツール（ctoption.exe）で設定したバイナリデータを
        //              Byte型の配列に読込む。
        //
        //履    歴  ：  V1.00  97/10/07  (SI3)鈴山       新規作成
        //              V4.0   01/03/26  (SI1)鈴山       frmCTMenu.frmから移した
        //********************************************************************************
		public static void GetCTOption()
		{

			byte[] buf = new byte[8];
            
            //バイナリファイルを読み込む
            string FileName = AppValue.CTOPTION;
            buf = File.ReadAllBytes(FileName);
            
            try
            {
 			    //グローバル変数に取り込む
                //プロフィールディスタンス
			    OptPRDFlag = Convert.ToBoolean(buf[0] & 0x80);
			    //画像フォーマット変換       'v11.5追加 by 間々田 2005/09/01
                OptFormatTransferFlag = Convert.ToBoolean(buf[0] & 0x40);
			    //疑似カラー
                OptColorFlag = Convert.ToBoolean(buf[0] & 0x20);
			    //和画像
                OptAdditionFlag = Convert.ToBoolean(buf[1] & 0x80);
			    //差画像
                OptSubtractionFlag = Convert.ToBoolean(buf[1] & 0x40);
			    //単純拡大
                OptEnlargeFlag = Convert.ToBoolean(buf[1] & 0x20);
			    //ROI処理
                OptROIFlag = Convert.ToBoolean(buf[1] & 0x10);
			    //寸法測定
                OptDistanceFlag = Convert.ToBoolean(buf[1] & 0x8);
			    //プロフィール
                OptProfileFlag = Convert.ToBoolean(buf[1] & 0x4);
			    //ヒストグラム
                OptHistgramFlag = Convert.ToBoolean(buf[1] & 0x2);
			    //CT値表示
                OptCTDumpFlag = Convert.ToBoolean(buf[1] & 0x1);
			    //マルチフレーム
                OptMultiFrameFlag = Convert.ToBoolean(buf[2] & 0x40);
			    //付帯情報修正
                OptPictureRetouchFlag = Convert.ToBoolean(buf[2] & 0x20);
			    //骨塩定量解析   'V6.0 append by 間々田 2002/07/16
                OptBoneDensityFlag = Convert.ToBoolean(buf[2] & 0x8);
			    //DICOM変換      added by 山本 2002-10-5
			    OptDICOMFlag = Convert.ToBoolean(buf[2] & 0x4);
			    //フィルタ処理   'v10.2追加 by 間々田 2005/06/21
                OptFilterFlag = Convert.ToBoolean(buf[2] & 0x2);
 
            }
            catch (Exception ex)
            {
                // エラーメッセージ表示
                //変更2014/11/18hata_MessageBox確認
                //MessageBox.Show(ex.Message + "\r\n" + "\r\n" + FileName,
                //                Application.ProductName,
                //                MessageBoxButtons.OK,
                //                MessageBoxIcon.Hand);
                MessageBox.Show(ex.Message + "\r\n" + "\r\n" + FileName,
                                Application.ProductName,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

            }

		}
	}
}
