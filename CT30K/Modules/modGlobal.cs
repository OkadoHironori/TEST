using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
 // ERROR: Not supported in C#: OptionDeclaration

using CTAPI;
using CT30K.Common;
using CT30K.Properties;

namespace CT30K
{
	static class modGlobal
	{
///* ************************************************************************** */
///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver10.0              */
///* 客先　　　　： ?????? 殿                                                   */
///* プログラム名： mod.bas                                                     */
///* 処理概要　　： ＣＴ起動時に値が決定される変数（その後不変）を定義します。  */
///* 注意事項　　： なし                                                        */
///* -------------------------------------------------------------------------- */
///* 適用計算機　： DOS/V PC                                                    */
///* ＯＳ　　　　： Windows XP  (SP4)                                           */
///* コンパイラ　： VB 6.0                                                      */
///* -------------------------------------------------------------------------- */
///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
///*                                                                            */
///* V10.00                                                                     */
///*                                                                         　 */
///*                                                                            */
///*                                                                            */
///* -------------------------------------------------------------------------- */
///* ご注意：                                                                   */
///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
///*                                                                            */
///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
        ///* ************************************************************************** */



        #region <CTSettingsで実施>
        /*
        //ﾋﾞｭｰ数最小
        public static short GVal_ViewMin;
        //ﾋﾞｭｰ数最大
        public static short GVal_ViewMax;
        //画像積算枚数最小
        public static int GValIntegNumMin;
        //画像積算枚数最大
        public static int GValIntegNumMax;
        //I.I もしくは FPD
        public static string GStrIIOrFPD;
        //テーブル移動 もしくは Ｘ線管移動
        public static string GStrTableOrXray;

        //ﾃｰﾌﾞﾙがX線管と干渉する限界FCD ※自動校正用
        public static float GVal_FcdLimit;

        //昇降上限値(mm)
        public static float GValUpperLimit;
        //昇降下限値(mm)
        public static float GValLowerLimit;

        //v11.2追加ここから by 間々田 2005/10/19
        //Ｘ線検出器がフラットパネル場合:True            'v7.0追加 by 間々田 2003/09/08
        public static bool Use_FlatPanel;
        //FCD/FDD の文字列                               'V9.6 append by 間々田 2004/10/13
        public static string gStrFidOrFdd;

        //X線検出器の種類    'v16.20/v17.00追加byやまおか 2010/01/19
        public enum DetectorConstants
        {
            DetTypeII,
            //0  I.I.
            DetTypeHama,
            //1  浜ホトFPD
            DetTypePke
            //2  パーキンエルマーFPD
        }
        public static DetectorConstants DetType;

//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
//分散処理関連の変数
//Public UsePCWS2             As Boolean  'コーン分散処理用ＰＣ２（True:可, False:不可    'v10.0追加 by 間々田 2005/01/24
//Public UsePCWS3             As Boolean  'コーン分散処理用ＰＣ３（True:可, False:不可）  'v10.0追加 by 間々田 2005/01/24
//Public UsePCWS4             As Boolean  'コーン分散処理用ＰＣ４（True:可, False:不可）  'v10.0追加 by 間々田 2005/01/24
//v11.2追加ここまで by 間々田 2005/10/19
//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
//Public HscOn                As Boolean  '高速度透視撮影 (True:有，False:無)             'v16.01 追加 by 山影 10-02-18
//Public SecondDetOn          As Boolean  '検出器切替機能 (True:有, False:無)             'v17.20 追加 by 長野 10-08-31
//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//FPDの全画素データを使う (True:使う, False:使わない)    'v17.22追加 byやまおか 2010/10/19
		public static bool Use_FpdAllpix;

//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
//Public UseRamDisk           As Boolean  'RAMディスクの有無（True:有，False:無）         'v17.40追加 byやまおか 2010/10/26
//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
			//透視画像左右反転フラグ(FPDの場合だけ有効)      'v17.50 modAutoPosから移動 byやまおか 2011/20/20
		public static bool IsLRInverse;


//*******************************************************************************
//機　　能： ＣＴ起動時に値が決定される変数（その後不変）をコモンから取得
//
//           変数名          [I/O] 型        内容
//引　　数： なし
//戻 り 値： なし
//
//補　　足： なし
//
//履　　歴： V1.00  99/XX/XX   ????????      新規作成
//*******************************************************************************
		public static void SetFixedVariable()
		{
			var _with1 = CTSettings.scaninh.Data;

			//        UsePCWS2 = (scaninh.pcws2 = 0)                         'コーン分散処理用ＰＣ２
			//        UsePCWS3 = (scaninh.pcws3 = 0)                         'コーン分散処理用ＰＣ３
			//        UsePCWS4 = (scaninh.pcws4 = 0)                         'コーン分散処理用ＰＣ４
			//v16.01 変更 by 山影 10-02-18

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//        UsePCWS2 = (.pcws2 = 0)                         'コーン分散処理用ＰＣ２
			//        UsePCWS3 = (.pcws3 = 0)                         'コーン分散処理用ＰＣ３
			//        UsePCWS4 = (.pcws4 = 0)                         'コーン分散処理用ＰＣ４
			//
			//        HscOn = (.high_speed_camera = 0)                '高速度透視撮影機能     'v16.01 追加 by 山影 10-02-18
			//
			//        SecondDetOn = (.second_detector = 0) And Not HscOn   'v17.20 検出器切替機能(高速透視優先) by 長野 10-08-31
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			Use_FpdAllpix = (_with1.fpd_allpix == 0);
            var _with2 = CTSettings.scancondpar.Data;
			//FPDの全画素データを使う    'v17.22追加 byやまおか 2010/10/19

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//        UseRamDisk = (.ramdisk = 0)     'v17.40追加 byやまおか 2010/10/26
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


			//Ｘ線検出器の種類：　０：Ｘ線I.I.　１：フラットパネル
			//Use_FlatPanel = (scancondpar.detector = 1) 'v16.20/v17.00削除 byやまおか 2010/01/19
			Use_FlatPanel = (_with2.detector == 1) | (_with2.detector == 2);
			//v16.20/v17.00追加 byやまおか 2010/01/19
			//DetType = Conversion.Val(Convert.ToString(_with2.detector));
            DetType = (DetectorConstants)(_with2.detector);


			//0:I.I.  1:浜ホトFPD  2:PkeFPD  'v16.20/v17.00追加 byやまおか 2010/01/19

			//v17.20 検出器の種類にかかわらず、FDDで統一する by 長野 2010/09/20
			//gStrFidOrFdd = IIf(Use_FlatPanel, "FDD", "FID") 'FCD/FDD の文字列  V9.6 append by 間々田 2004/10/13
			gStrFidOrFdd = "FDD";

			//ﾃｰﾌﾞﾙがX線管と干渉する限界FCD
			GVal_FcdLimit = _with2.fcd_limit;


			//FPD(X線からみた画像)でscaninhibitの設定が有効な場合に透視を反転させる    'v17.50変更 byやまおか 2011/02/02
			//このフラグが有効な場合はI.I.と同じように検出器側から見た透視になる
            IsLRInverse = Use_FlatPanel & Convert.ToBoolean(_with1.transdisp_lr_inv == 0);

            
            var _with3 = CTSettings.infdef.Data;

			//ビュー数最小
			GVal_ViewMin = (short)Conversion.Val(_with3.min_view);

			//ビュー数最大
            GVal_ViewMax = (short)modLibrary.MaxVal(Conversion.Val(_with3.max_view), GVal_ViewMin);

			//画像積算枚数最小
			GValIntegNumMin = (int)modLibrary.MaxVal(Conversion.Val(_with3.min_integ_number), 1);

			//画像積算枚数最大
           GValIntegNumMax = (int)modLibrary.MaxVal(Conversion.Val(_with3.max_integ_number), GValIntegNumMin);

			//I.I もしくは FPD の文字列
			GStrIIOrFPD = modLibrary.RemoveNull(_with3.detector[(Use_FlatPanel ? 1 : 0)].GetString());

			//テーブル移動 もしくは Ｘ線管移動
			//GStrTableOrXray = RemoveNull(.table_y(IIf(scaninh.table_y = 1, 1, 0)))
			//v17.60 ストリングテーブル化 by長野 2011/05/25
			//v29.99 今のところX線管移動は不要のため変更 by長野 2013/04/08'''''ここから'''''
			//GStrTableOrXray = IIf(scaninh.table_y = 1, LoadResString(20174), LoadResString(20126))
			GStrTableOrXray = CTResources.LoadResString(20126);
			var _with4 =CTSettings.t20kinf.Data;
			//v29.99 今のところX線管移動は不要のため変更 by長野 2013/04/08'''''ここまで'''''

			//昇降上限値(mm)：×100の値を格納（実際には mechapara.csv の d8_1b の値）
			GValUpperLimit = _with4.upper_limit / 100;

			//昇降下限値(mm)：×100の値を格納（実際には mechapara.csv の d8_1c の値）
			GValLowerLimit = _with4.lower_limit / 100;

			//Ｘ線制御器のタイプ取得：0(FEINFOCUS),1(KEVEX),2(浜ホト)
            modXrayControl.XrayType = (modXrayControl.XrayTypeConstants)Conversion.Val(_with4.system_type);

		}
        */
        #endregion <CTSettingsで実施>

    }
}
