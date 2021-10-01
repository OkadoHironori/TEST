using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
//
//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

using System.Threading; //Rev20.00 追加 by長野 2014/12/15
namespace CT30K
{

	///* ************************************************************************** */
	///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： frmStatus.frm                                               */
	///* 処理概要　　： ステータス                                                  */
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
	///* V5.0        01/07/31    (CATS)   山本輝夫   メカ制御変更                   */
	///* v11.2       05/10/19    (SI3)    間々田     フル２次元対応                 */
	///*                                                                            */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
	///* ************************************************************************** */
	
    //public partial class frmStatus : Form
	public partial class frmStatus : FixedForm
	{

		//********************************************************************************
		//  共通データ宣言
		//********************************************************************************

		private int scanProcessID = 0;		//scanav.exe の プロセスID格納用 'V6.0 append by 間々田 2002-9-9
		//Dim temp_rc_iifield         As Long     '回転中心校正（オートセンタリング）時のII視野 added by 山本　2004-6-16

		//スライスプランテーブル・パラメータチェック用
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		Const SlicePlanNGOpenRead   As Long = 2 ^ 0
		Const SlicePlanNGUdAbsPos   As Long = 2 ^ 1
		Const SlicePlanNGScanArea   As Long = 2 ^ 2
		Const SlicePlanNGSliceWid   As Long = 2 ^ 3
		Const SlicePlanNGScanView   As Long = 2 ^ 4
		Const SlicePlanNGIntegNum   As Long = 2 ^ 5
		Const SlicePlanNGMatrixSize As Long = 2 ^ 6
		Const SlicePlanNGScanView2  As Long = 2 ^ 7
		Const SlicePlanNGFTableXpos As Long = 2 ^ 8
		Const SlicePlanNGFTableYpos As Long = 2 ^ 9
		Const SlicePlanNGConeK      As Long = 2 ^ 10
		Const SlicePlanNGConeWPK    As Long = 2 ^ 11
		Const SlicePlanNGRawDatSize As Long = 2 ^ 12
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
		private const int SlicePlanNGOpenRead = 1;
		private const int SlicePlanNGUdAbsPos = 2;
		private const int SlicePlanNGScanArea = 4;
		private const int SlicePlanNGSliceWid = 8;
		private const int SlicePlanNGScanView = 16;
		private const int SlicePlanNGIntegNum = 32;
		private const int SlicePlanNGMatrixSize = 64;
		private const int SlicePlanNGScanView2 = 128;
		private const int SlicePlanNGFTableXpos = 256;
		private const int SlicePlanNGFTableYpos = 512;
		private const int SlicePlanNGConeK  = 1028;
		private const int SlicePlanNGConeWPK = 2048;
		private const int SlicePlanNGRawDatSize = 4096;

		private float UdPosAtScanStart = 0;		//マルチスキャンでオートセンタリングありの場合、スキャン開始時の昇降位置を書き込むための変数 v11.5追加 by 間々田 2006/09/21

        //テーブル連続回転時、scancondpar.dev_klimitのバックアップをとっておく by長野 2014/03/04     //追加2014/10/07hata_v19.51反映
        //Rev20.00 64bit化により廃止 by長野 2014/11/04
        //int bak_dev_klimit;

		//メカコントロールフォーム参照用
		frmMechaControl myMechaControl = null;

        //Stopwatch処理（画質マトリックスの撮影時間確認用） ReV26.00 by井上
        //private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

		private static frmStatus _Instance = null;

		public frmStatus()
		{
			InitializeComponent();
		}

		public static frmStatus Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmStatus();
				}

				return _Instance;
			}
		}



		//********************************************************************************
		//機    能  ：  校正ステータス監視（スキャンスタート時のチェック）
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：                   [ /O] Boolean   結果 True  : ｽｷｬﾝを続行
		//                                                    False : ｽｷｬﾝを中止
		//補    足  ：  校正ステータスが準備未完了なら、原因となる校正名を表示する。
		//
		//履    歴  ：  V4.0   01/01/22  (SI1)鈴山       新規作成
		//********************************************************************************
		private bool Check_ScanStart()
		{
			bool functionReturnValue = false;

			frmScanControl frmScanControl = frmScanControl.Instance;

			//全体の判定
			if (frmScanControl.IsOkAll)
			{
				//準備完了
				functionReturnValue = true;
			}
			else
			{
				string strMsg = "";		//表示ﾒｯｾｰｼﾞ

				//リソース 9324:
				//   下記校正が準備完了でないため、
				//   正しい画像が表示されない可能性がありますが、
				//   スキャンを実行しますか？
				strMsg = CTResources.LoadResString(9324) + "\r\n" + "\r\n";

				//原因を探る
				if (!frmScanControl.IsOkGain ) strMsg = strMsg + CTResources.LoadResString(StringTable.IDS_CorGain) + "\r\n";					//ゲイン校正
				if (!frmScanControl.IsOkScanPosition ) strMsg = strMsg + CTResources.LoadResString(StringTable.IDS_CorScanPos) + "\r\n";		//スキャン位置校正
				//If Not .IsOkVertical() Then strMsg = strMsg & LoadResString(IDS_CorDistortion) & vbCrLf     '幾何歪校正
                if (!frmScanControl.IsOkVertical && !CTSettings.detectorParam.Use_FlatPanel) strMsg = strMsg + CTResources.LoadResString(StringTable.IDS_CorDistortion) + "\r\n";	//幾何歪校正 'v17.00変更 byやまおか 2010/03/03
				if (!frmScanControl.IsOkRotationCenter ) strMsg = strMsg + CTResources.LoadResString(StringTable.IDS_CorRot) + "\r\n";		//回転中心校正
				if (!frmScanControl.IsOkOffset ) strMsg = strMsg + CTResources.LoadResString(StringTable.IDS_CorOffset) + "\r\n";				//オフセット校正
				if (!frmScanControl.IsOkDistance ) strMsg = strMsg + CTResources.LoadResString(StringTable.IDS_CorSize) + "\r\n";				//寸法校正

				//確認メッセージ
				DialogResult result = MessageBox.Show(strMsg, Application.ProductName, 
													  MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
				functionReturnValue = (result == DialogResult.Yes);
			}

			return functionReturnValue;
		}
        //********************************************************************************
        //機    能  ：  校正ステータス監視（スキャノスタート時のチェック）
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：                   [ /O] Boolean   結果 True  : ｽｷｬﾝを続行
        //                                                    False : ｽｷｬﾝを中止
        //補    足  ：  校正ステータスが準備未完了なら、原因となる校正名を表示する。
        //
        //履    歴  ：  V21.00   15/03/05  (検S1)長野       新規作成
        //********************************************************************************
        private bool Check_ScanoStart() //Rev21.00 追加 by長野 2015/03/06
        {
            bool functionReturnValue = false;

            frmScanControl frmScanControl = frmScanControl.Instance;

            //判定
            //if (frmScanControl.IsOkAll)
            if (frmScanControl.IsOkGain && frmScanControl.IsOkOffset && frmScanControl.IsOkDistance)
            {
                //準備完了
                functionReturnValue = true;
            }
            else
            {
                string strMsg = "";		//表示ﾒｯｾｰｼﾞ

                //リソース 9324:
                //   下記校正が準備完了でないため、
                //   正しい画像が表示されない可能性がありますが、
                //   スキャンを実行しますか？
                strMsg = CTResources.LoadResString(9324) + "\r\n" + "\r\n";

                //原因を探る
                if (!frmScanControl.IsOkGain) strMsg = strMsg + CTResources.LoadResString(StringTable.IDS_CorGain) + "\r\n";					//ゲイン校正
                if (!frmScanControl.IsOkOffset) strMsg = strMsg + CTResources.LoadResString(StringTable.IDS_CorOffset) + "\r\n";				//オフセット校正
                if (!frmScanControl.IsOkDistance) strMsg = strMsg + CTResources.LoadResString(StringTable.IDS_CorSize) + "\r\n";				//寸法校正

                //確認メッセージ
                DialogResult result = MessageBox.Show(strMsg, Application.ProductName,
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                functionReturnValue = (result == DialogResult.Yes);
            }

            return functionReturnValue;
        }
        //********************************************************************************
        //機    能  ：  校正ステータス監視（スキャノスタート時の昇降速度チェック）
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：                   [ /O] Boolean   結果 True  : ｽｷｬﾝを続行
        //                                                    False : ｽｷｬﾝを中止
        //補    足  ：  校正ステータスが準備未完了なら、原因となる校正名を表示する。
        //
        //履    歴  ：  V21.00   15/03/05  (検S1)長野       新規作成
        //********************************************************************************
        private bool Check_ScanoUdpSpeed() //Rev21.00 追加 by長野 2015/03/06
        {
            bool functionReturnValue = false;

            frmScanControl frmScanControl = frmScanControl.Instance;

            //速度のチェック
            //Rev21.00 空振り用のテーブルを作る。
            //
            string buf = null;
            string[] strCell = null;
            double UdStep = (double)0.0;
            double UdMaxSpeed = (double)0.0;
            double UdMinSpeed = (double)0.0;

            //ファイルオープン
            StreamReader file = null;
            try
            {
                //変更2015/01/22hata
                //file = new StreamReader(FileName);
                file = new StreamReader(@"c:\ct\mechadata\boardpara.csv", Encoding.GetEncoding("shift-jis"));

                //while (!FileSystem.EOF(fileNo))
                while ((buf = file.ReadLine()) != null)
                {
                    //１行読み込む
                    if (!string.IsNullOrEmpty(buf))
                    {
                        //カンマで区切って配列に格納
                        strCell = buf.Split(',');

                        //コメントか？
                        if (strCell[0].Trim() == "UdStep")
                        {
                            //先頭列の文字が数字なら情報を取り出す
                            double IsNumeric = 0;
                            if (double.TryParse(strCell[1], out IsNumeric))
                            {
                                UdStep = IsNumeric;
                            }
                        }
                        //コメントか？
                        if (strCell[0].Trim() == "UdMmsFL")
                        {
                            //先頭列の文字が数字なら情報を取り出す
                            double IsNumeric = 0;
                            if (double.TryParse(strCell[1], out IsNumeric))
                            {
                                UdMinSpeed = IsNumeric;
                            }
                        }
                        //コメントか？
                        if (strCell[0].Trim() == "UdManualLimitMms")
                        {
                            //先頭列の文字が数字なら情報を取り出す
                            double IsNumeric = 0;
                            if (double.TryParse(strCell[1], out IsNumeric))
                            {
                                UdMaxSpeed = IsNumeric;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                //ファイルクローズ
                if (file != null)
                {
                    file.Close();
                    file = null;
                }
            }
            double FrameRate = 0.0;
            //v19.10 PKEとその他の検出器でframerate取得方法を変える 2012/09/06 by長野
            if (CTSettings.scancondpar.Data.detector == 2)
            {
                //Rev20.00 変更 by長野 2015/02/06
                FrameRate = (double)1.0 / (modCT30K.fpd_integlist[frmScanControl.cmbInteg.SelectedIndex] / (double)1000.0);
                //if (modDeclare.GetPrivateProfileString("Timings", "Timing_" + Convert.ToString(frmScanControl.Instance.cmbInteg.SelectedIndex), "", dummy, (uint)dummy.Capacity, AppValue.XisIniFileName) > 0)
                //{
                //    double dummyValue = 0;
                //    double.TryParse(dummy.ToString(), out dummyValue);
                //    FrameRate = 1000 / (dummyValue / 1000);
                //}
            }
            else
            {
                FrameRate = frmTransImage.Instance.GetCurrentFR();
            }
            float tmpL = 0;
            float tmpSec = 0;
            tmpSec = CTSettings.scansel.Data.mscanopt * CTSettings.scansel.Data.mscano_integ_number / (float)FrameRate;
            //tmpL = mscanopt * mscano_width;
            tmpL = (CTSettings.scansel.Data.mscano_real_mscanopt - 1) * CTSettings.scansel.Data.min_slice_wid;
            float UdSpeed = (float)(tmpL / tmpSec);
            //Rev26.25(特) 変更 by chouno
            //if (UdSpeed > UdMaxSpeed || UdSpeed < UdMinSpeed)
            //{
            //    MessageBox.Show(CTResources.LoadResString(22012), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return functionReturnValue;
            //}
            if (UdSpeed > UdMaxSpeed)
            {
                MessageBox.Show("スキャノ撮影時のテーブル昇降速度が上限を超えています。スキャノピッチを小さくするか、積分枚数、積分時間(FPD)を大きくしてください。" +
                                "\r\n" + "スキャノテーブル昇降速度:" + UdSpeed.ToString("0.000") + "\r\n" + "テーブル昇降最高速度:" + UdMaxSpeed.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return functionReturnValue;
            }
            else if (UdSpeed < UdMinSpeed)
            {
                MessageBox.Show("スキャノ撮影時のテーブル昇降速度が下限を下回っています。スキャノピッチを大きくするか、積分枚数、積分時間(FPD)を小さくしてください。" +
                                "\r\n" + "スキャノテーブル昇降速度:" + UdSpeed.ToString("0.000") + "\r\n" + "テーブル昇降最低速度:" + UdMinSpeed.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return functionReturnValue;
            }
            functionReturnValue = true;
            return functionReturnValue;
        }
		//********************************************************************************
		//機    能  ：  コーンビームスキャン時のスキャン条件チェック
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：                   [ /O] Boolean   実行結果  True:正常  False:エラー
		//補    足  ：
		//
		//履    歴  ：  V4.0   01/04/14  (CATS)山本       新規作成
		//********************************************************************************
		private bool ConeBeamScanConditionCheck()
		{
			//戻り値初期化
			bool functionReturnValue = false;

			//'幾何歪ﾊﾟﾗﾒｰﾀB1(ｺｰﾝﾋﾞｰﾑCT用)
			//'    構造体名：scancondpar
			//'    コモン名：b[1]             ※[0]～[5]の[1]だけ
			//'If GetCommonFloat("scancondpar", "b[1]") = 0 Then Exit Function
			if (CTSettings.scancondpar.Data.b[1] == 0 ) return functionReturnValue;                          //v11.5変更 by 間々田 2006/04/24

			//スキャンエリア(ｺｰﾝﾋﾞｰﾑCT用)：スキャンエリア = 0 でもオートセンタリングありならエラーとしない
			if ((CTSettings.scansel.Data.cone_scan_area == 0) && (CTSettings.scansel.Data.auto_centering == 0)) return functionReturnValue;

			//マトリクスサイズ
			if (CTSettings.scansel.Data.matrix_size == 0 ) return functionReturnValue;

			//スライス厚(mm)
			if (CTSettings.scansel.Data.cone_scan_width == 0 ) return functionReturnValue;

			//スライス枚数
			if (CTSettings.scansel.Data.k == 0 ) return functionReturnValue;

			functionReturnValue = true;
			return functionReturnValue;
		}


		//********************************************************************************
		//機    能  ：  ヘリカルスキャン時の昇降範囲チェック
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：                   [ /O] Boolean   実行結果  True:正常  False:エラー
		//補    足  ：
		//
		//履    歴  ：  V3.0   00/10/24  (CATS)山本       新規作成
		//********************************************************************************
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//Private Function HelicalUpDownCheck() As Boolean
		//
		//    Dim Zdae    As Single
		//    Dim kzp     As Single
		//    Dim zs      As Single
		//    Dim ze      As Single
		//
		//    'データ収集終了高さ Zdae の計算
		//    'scancondpar.Alpha : オーバーラップ角度(radian)
		//    'mecainf.udab_pos  : ヘリカル開始位置(mm)
		//    'scansel.Zp        : ヘリカルピッチ(mm)
		//    'scansel.DeltaZ    : スライスピッチ(mm)=軸方向Boxelｻｲｽﾞ(mm)
		//    'scansel.K         : スライス枚数
		//    kzp = 1 + scancondpar.Alpha / Pai
		//    zs = mecainf.udab_pos + scansel.Zp * kzp / 2
		//    ze = zs + scansel.delta_z * (scansel.K - 1)
		//    Zdae = ze + scansel.Zp * kzp / 2
		//
		//    HelicalUpDownCheck = (Zdae <= GValUpperLimit) 'GValUpperLimit:昇降上限値(mm)
		//
		//End Function
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

		//*************************************************************************************************
		//機　　能： スキャン終了後に透視画像へ水平線を付加する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 個別の処理は、別関数（DrawHorizon_Execute）で行なう
		//
		//履　　歴： v2.0    00/02/08  (SI1)鈴山     新規作成
		//                   00/03/16  (SI1)鈴山     ｲﾒｰｼﾞﾌﾟﾛの起動・終了を追加
		//           v15.00 2008/11/01 (SI1)間々田   リニューアル
		//*************************************************************************************************
		private void DrawHorizon_AfterScan()
		{
			int i = 0;
			int pos = 0;
			//v17.68 追加 by長野 2012-02-23 ここから
            string FileName = null;
            string FolderName = null;
            string BaseName = null;
			string Result1 = null;
            string Result2 = null;
			//v17.68 追加 by長野 2012-02-23 ここまで
            
            string tmpstr = null;//Rev25.15 add by chouno 2018/01/11

    		//イメージプロ起動
			if (!modCT30K.StartImagePro()) return;

            //Rev25.15 スキャン中、再構成を行わない場合を追加 by chouno 2018/01/11
            if (modCT30K.ImportedCTImages != null)
            {
                //画像枚数分のループ
                for (i = 1; i <= modCT30K.ImportedCTImages.Count; i++)
                {
                    pos = 2 + Math.Sign(CTSettings.scansel.Data.pitch) * (CTSettings.scansel.Data.multislice - (i - 1) % (CTSettings.scansel.Data.multislice * 2 + 1));

                    //Rev20.00 コーンビームスキャン時も保存できるようにする by長野 2015/02/06
                    if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScan)
                    {
                        //v17.68 オートズーミング時に保存した透視画像のファイル名には-Zxxxは付かないので、削除するよう処理を追加 by長野 2012-02-23
                        if (CTSettings.scansel.Data.auto_zoomflag == 1)
                        {
                            FileName = modCT30K.ImportedCTImages[i - 1];

                            FolderName = Path.GetDirectoryName(FileName);

                            //ベース名取得
                            BaseName = Path.GetFileNameWithoutExtension(FileName);

                            //ズーミング連番・スキャノ連番を取り除く
                            if (Regex.IsMatch(BaseName.ToLower(), @".+-s\d{3}$") || (Regex.IsMatch(BaseName.ToLower(), @".+-z\d{3}$")))
                            {
                                Result1 = BaseName.Substring(0, BaseName.Length - 5);
                            }

                            Result2 = Path.Combine(FolderName, Result1);
                        }
                        else
                        {
                            Result2 = modCT30K.ImportedCTImages[i - 1];
                        }
                    }
                    else
                    {
                        Result2 = modCT30K.ImportedCTImages[i - 1];
                    }

                    //お目当てのファイルを処理する
                    //DrawHorizon_Execute ChangeExtension(ImportedCTImages(i), ".flu"), pos
                    //v17.68 Result2に変更 by長野 2012-02-23

                    tmpstr = modFileIO.ChangeExtension(Result2, ".flu");
                    if (File.Exists(tmpstr))
                    {
                        DrawHorizon_Execute(tmpstr, pos);
                    }
                }
            }
            else
            {
                pos = 2 + Math.Sign(CTSettings.scansel.Data.pitch) * (CTSettings.scansel.Data.multislice - (i - 1) % (CTSettings.scansel.Data.multislice * 2 + 1));

                if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScan)
                {
                    //シングル最大枚数ループ
                    for (int cnt = 1; cnt <= 9999; cnt++)
                    {
                        Result2 = Path.Combine(CTSettings.scansel.Data.pro_code.GetString() + CTSettings.scansel.Data.pro_name.GetString()) + "-" + cnt.ToString("0000") + ".img";
                        tmpstr = modFileIO.ChangeExtension(Result2, ".flu");

                        if (File.Exists(tmpstr))
                        {
                            //お目当てのファイルを処理する
                            //DrawHorizon_Execute ChangeExtension(ImportedCTImages(i), ".flu"), pos
                            //v17.68 Result2に変更 by長野 2012-02-23
                            DrawHorizon_Execute(tmpstr, pos);
                        }
                    }
                }
                else
                {
                    //コーン回数最大分のループ
                    for (int cnt = 1; cnt <= 999; cnt++)
                    {
                        Result2 = Path.Combine(CTSettings.scansel.Data.pro_code.GetString() + CTSettings.scansel.Data.pro_name.GetString()) + "-" + cnt.ToString("000") + "-0001.img";
                        tmpstr = modFileIO.ChangeExtension(Result2, ".flu");

                        if (File.Exists(tmpstr))
                        {
                            //お目当てのファイルを処理する
                            //DrawHorizon_Execute ChangeExtension(ImportedCTImages(i), ".flu"), pos
                            //v17.68 Result2に変更 by長野 2012-02-23
                            DrawHorizon_Execute(tmpstr, pos);
                        }
                    }
                }
            }
		}
        //*************************************************************************************************
        //機　　能： スキャン終了後にスキャノデータをtiffに変換する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 個別の処理は、別関数（DrawHorizon_Execute）で行なう
        //
        //履　　歴： v21.00   15/02/24  (検S1)長野    新規作成
        //*************************************************************************************************
        private void ConvertToTiffForScano()
        {
            int i = 0;
            int pos = 0;
            //v17.68 追加 by長野 2012-02-23 ここから
            string FileName = null;
            string FolderName = null;
            string BaseName = null;
            string Result1 = null;
            string Result2 = null;
            //v17.68 追加 by長野 2012-02-23 ここまで

            //イメージプロ起動
            if (!modCT30K.StartImagePro()) return;

            //画像枚数分のループ
            for (i = 1; i <= modCT30K.ImportedCTImages.Count; i++)
            {
                pos = 2 + Math.Sign(CTSettings.scansel.Data.pitch) * (CTSettings.scansel.Data.multislice - (i - 1) % (CTSettings.scansel.Data.multislice * 2 + 1));

                //Rev20.00 コーンビームスキャン時も保存できるようにする by長野 2015/02/06
                if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScan)
                {
                    //v17.68 オートズーミング時に保存した透視画像のファイル名には-Zxxxは付かないので、削除するよう処理を追加 by長野 2012-02-23
                    if (CTSettings.scansel.Data.auto_zoomflag == 1)
                    {
                        FileName = modCT30K.ImportedCTImages[i - 1];

                        FolderName = Path.GetDirectoryName(FileName);

                        //ベース名取得
                        BaseName = Path.GetFileNameWithoutExtension(FileName);

                        //ズーミング連番・スキャノ連番を取り除く
                        if (Regex.IsMatch(BaseName.ToLower(), @".+-s\d{3}$") || (Regex.IsMatch(BaseName.ToLower(), @".+-z\d{3}$")))
                        {
                            Result1 = BaseName.Substring(0, BaseName.Length - 5);
                        }

                        Result2 = Path.Combine(FolderName, Result1);
                    }
                    else
                    {
                        Result2 = modCT30K.ImportedCTImages[i - 1];
                    }
                }
                else
                {
                    Result2 = modCT30K.ImportedCTImages[i - 1];
                }

                //お目当てのファイルを処理する
                //DrawHorizon_Execute ChangeExtension(ImportedCTImages(i), ".flu"), pos
                //v17.68 Result2に変更 by長野 2012-02-23
                ConvertToTiffForScano_Execute(@"c:\ct\temp\ScanoTiff.testtiff", Result2, "tif");
            }
        }
        //********************************************************************************
        //機    能  ：  スキャノデータをtiffに変換
        //              変数名           [I/O] 型        内容
        //引    数  ：  FileName         [I/ ] String    画像ファイル名（拡張子なし）
        //              iSCN             [I/ ] Long      スキャン位置(0～4)
        //戻 り 値  ：  なし
        //補    足  ：  イメージプロで透視画像（*.testitff）を開き、tiffに変換します。
        //              フォーマット変換がベースです
        //
        //履    歴  ：  V21.00  15/02/15  (検S1)長野      新規作成
        //                     
        //                     
        //                     
        //********************************************************************************
        //Private Sub DrawHorizon_Execute(ByVal FileName As String, ByVal iSCN As Long)
        private string ConvertToTiffForScano_Execute(string FileName, string SavePath, string SaveFormat = "TIFF")
        {
            int rc = 0;					//イメージプロ関数の戻り値

            int h_size = 0;
            int v_size = 0;

            //Ipc32v5.RECT roiRect = default(Ipc32v5.RECT);	//ヒストグラム用矩形
            //Winapi.RECT roiRect = default(Winapi.RECT);		//ヒストグラム用矩形

            float[] HistData = new float[11];	//ヒストグラム用配列
            //int adv = 0;

            //戻り値初期化
            string functionReturnValue = "";	//v11.5追加 by 間々田 2006/06/09

            //入力画像のサイズからマトリクスを判定する
            FileInfo fileInfo = new FileInfo(FileName);
            //2014/11/06hata キャストの修正
            h_size = CTSettings.scancondpar.Data.fimage_hsize;
            v_size = (int)fileInfo.Length / h_size / 2;

            //左右反転させるパラメータ
            int LRInverse = Convert.ToInt32(CTSettings.detectorParam.IsLRInverse);

            //保存ファイル名
            string SaveFileName = modLibrary.RemoveExtension(SavePath, ".img") + "f." + SaveFormat;
            rc = CallImageProFunction.CallScanoToTiff(
                    v_size,
                    h_size,
                    FileName,
                    SaveFormat,
                    SaveFileName,
                    LRInverse
                    );

            //保存に成功した場合、保存ファイル名を戻り値として返す
            if (rc == 0) functionReturnValue = SaveFileName;

            //v11.5追加ここまで by 間々田 2006/06/08

            return functionReturnValue;
        }
		//*******************************************************************************
		//機　　能： mecainf（コモン）の更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 回転中心校正関連のパラメータのみ更新
		//
		//履　　歴： v11.2  06/01/13  (SI3)間々田    新規作成
		//*******************************************************************************
		private void UpdateMecainf()
		{
			//CTSettings.mecainf.DataType theMecainf = default(CTSettings.mecainf.DataType);
            MecaInf theMecainf = new MecaInf();
            theMecainf.Data.Initialize();

			bool Changed = false;

			//mecainf（コモン）取得
			//modMecainf.GetMecainf(ref theMecainf);
            theMecainf.Load();


            if (CTSettings.scansel.Data.scan_kv != theMecainf.Data.rc_kv) Changed = true;
            
            //if (theMecainf.Data.udab_pos != theMecainf.Data.rc_udab_pos) Changed = true;
            //Rev23.10 計測CT対応 by長野 2015/10/16
            if (CTSettings.scaninh.Data.cm_mode == 0)
            {
                if(theMecainf.Data.ud_linear_pos != theMecainf.Data.rc_udab_pos) Changed = true;
            }
            else
            {
                if(theMecainf.Data.udab_pos != theMecainf.Data.rc_udab_pos) Changed = true;
            }
            
            //If temp_rc_iifield <> .rc_iifield Then Changed = True
            if (theMecainf.Data.iifield != theMecainf.Data.rc_iifield) Changed = true;			//v11.5変更 by 間々田 2006-07-10
            //if (CTSettings.scansel.Data.multi_tube != theMecainf.Data.rc_mt) Changed = true;
            //Rev23.00 変更 by長野 2015/10/18
            if (CTSettings.scansel.Data.multi_tube != theMecainf.Data.rc_mt)
            {
                Changed = true;
                theMecainf.Data.rc_mt = CTSettings.scansel.Data.multi_tube;
            }
     
			//コーン時
			if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone)
			{
                theMecainf.Data.cone_rc_cor = 1;
                if (Changed) theMecainf.Data.normal_rc_cor = 0;
			}
			//ノーマルスキャン時
			else
			{
                theMecainf.Data.normal_rc_cor = 1;
                if (Changed) theMecainf.Data.cone_rc_cor = 0;
			}

			//管電圧(KV)
            theMecainf.Data.rc_kv = CTSettings.scansel.Data.scan_kv;

			//昇降位置(mm)
			//.rc_udab_pos = .udab_pos
            theMecainf.Data.rc_udab_pos = UdPosAtScanStart;	//マルチスキャンでオートセンタリングありの場合、スキャン開始時の昇降位置を書き込む v11.5変更 by 間々田 2006/09/21

			//I.I.視野
			//.rc_iifield = temp_rc_iifield          'スキャン終了直後にシーケンサ通信を再開するため、プロプティを正しく取得できないためスキャン前に取得しておいた値を使う
            theMecainf.Data.rc_iifield = theMecainf.Data.iifield;						//v11.5変更 by 間々田 2006-07-10

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//        'Ｘ線管
			//        .rc_mt = scansel.multi_tube
			//
			//        '回転選択が可能な場合
			//        If scaninh.rotate_select = 0 Then
			//            .rc_rs = scansel.rotate_select
			//        End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//ビニングモード
            theMecainf.Data.rc_bin = CTSettings.scansel.Data.binning;			//0:１×１，1:２×２，2:４×４

			//mecainf（コモン）更新
			//modMecainf.PutMecainf(ref theMecainf);
            theMecainf.Write();

			//mecainf読み込み    '追加 by 間々田 2009/06/08
			//modMecainf.GetMecainf(ref CTSettings.mecainf.Data);
            CTSettings.mecainf.Load();

		}


		//********************************************************************************
		//機    能  ：  画像ファイルごとに透視画像へ水平線を付加する
		//              変数名           [I/O] 型        内容
		//引    数  ：  FileName         [I/ ] String    画像ファイル名（拡張子なし）
		//              iSCN             [I/ ] Long      スキャン位置(0～4)
		//戻 り 値  ：  なし
		//補    足  ：  イメージプロで透視画像（*.flu）を開き、水平線を記録します。
		//              結果を別フォーマット（*.jpg）で保存し、元画像（*.flu）は削除します。
		//
		//履    歴  ：  V2.0   00/02/08  (SI1)鈴山       新規作成
		//                     00/03/04  (CATS)山本      透視画像のコントラスト改善
		//                     00/03/13  (SI1)鈴山       引数（iSCN）を追加
		//                     00/03/16  (SI1)鈴山       ｲﾒｰｼﾞﾌﾟﾛの起動・終了を削除
		//********************************************************************************
		//Private Sub DrawHorizon_Execute(ByVal FileName As String, ByVal iSCN As Long)
		private string DrawHorizon_Execute(string FileName, int iSCN, string SaveFormat = "JPG", bool DelFLU = true)	//v11.5変更 by 間々田 2006/06/09
		{
			ushort[] IMG_FLU;			//画像読込みバッファ（透視画像）
            ushort[] IMG_FLU2;          //画像読み込みバッファ(透視画像、ただしパーキンエルマーFPD用に両端のデータを削除したもの) //Rev20.00 追加 by長野 2015/02/06
            int modX = 0;               //画像の両端削除用 by長野 Rev20.00 2015/02/06
            int modY = 0;               //画像の両端削除用 by長野 Rev20.00 2015/02/06
            
            int rc = 0;					//イメージプロ関数の戻り値

            //Rev20.00 追加 by長野 2015/02/06
            int Horizonx1 = 0;
            int Horizonx2 = 0;
            int Horizony1 = 0;
            int Horizony2 = 0;
            int Horizony1UCone = 0;
            int Horizony2UCone = 0;
            int Horizony1LCone = 0;
            int Horizony2LCone = 0;
            float Swad = 0;

            //Ipc32v5.RECT tmpReg = default(Ipc32v5.RECT);	//画像エリア指定の構造体
			//int x1 = 0;					//水平線のＸ開始点       'v17.50変更 Integer→Long by 間々田 2011/02/03
			//int y1 = 0;					//水平線のＹ開始点       'v17.50変更 Integer→Long by 間々田 2011/02/03
			//int x2 = 0;					//水平線のＸ終了点       'v17.50変更 Integer→Long by 間々田 2011/02/03
			//int y2 = 0;					//水平線のＹ終了点       'v17.50変更 Integer→Long by 間々田 2011/02/03
			
            //Ipc32v5.RECT roiRect = default(Ipc32v5.RECT);	//ヒストグラム用矩形
            Winapi.RECT roiRect = default(Winapi.RECT);		//ヒストグラム用矩形

            
            float[] HistData = new float[11];	//ヒストグラム用配列
			int adv = 0;

			//戻り値初期化
			string functionReturnValue = "";	//v11.5追加 by 間々田 2006/06/09

			//透視画像読み取り（*.flu）
			IMG_FLU = new ushort[CTSettings.scancondpar.Data.fimage_hsize * CTSettings.scancondpar.Data.fimage_vsize + 1];
            //Rev20.00 追加 by長野 2015/02/06
            IMG_FLU2 = new ushort[(CTSettings.scancondpar.Data.fimage_hsize - Convert.ToInt32((CTSettings.scancondpar.Data.fimage_hsize % 100) / 2F * 2)) * (CTSettings.scancondpar.Data.fimage_vsize - Convert.ToInt32((CTSettings.scancondpar.Data.fimage_hsize % 100) / 2F * 2))];


			//If ImageOpen(IMG_FLU(0), FileName & ".flu", scancondpar.fimage_hsize, scancondpar.fimage_vsize) <> 0 Then Exit Function
			if (IICorrect.ImageOpen(ref IMG_FLU[0], FileName, CTSettings.scancondpar.Data.fimage_hsize, CTSettings.scancondpar.Data.fimage_vsize) != 0) return functionReturnValue;	//v11.5変更 by 間々田 2006/06/09

            //Rev20.00 追加 by長野 2015/02/06
            if (CTSettings.detectorParam.Use_FlatPanel)
            {
                if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
                {
                    modX = Convert.ToInt32((CTSettings.scancondpar.Data.fimage_hsize % 100) / 2F);
                    modY = Convert.ToInt32((CTSettings.scancondpar.Data.fimage_vsize % 100) / 2F);
                    IICorrect.ImageCopy_short_cut(ref IMG_FLU[0],ref IMG_FLU2[0],CTSettings.scancondpar.Data.fimage_hsize,CTSettings.scancondpar.Data.fimage_vsize,modX,modY);
                }
            }

			//Ｘ線検出器がフラットパネルの場合 2003/10/21 by 間々田
            if (CTSettings.detectorParam.Use_FlatPanel)
			{
				if (ScanCorrect.GetDefGainOffset(ref adv))
				{
                    if (CTSettings.detectorParam.DetType ==DetectorConstants.DetTypeHama)		//v17.00 if追加 byやまおか 2010/02/16
					{
						ScanCorrect.FpdGainCorrect(ref IMG_FLU[0], ref ScanCorrect.GAIN_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, adv);								//ゲイン補正
						ScanCorrect.FpdDefCorrect_short(ref IMG_FLU[0], ref ScanCorrect.Def_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, 0, CTSettings.detectorParam.v_size - 1);	//欠陥補正
					}

                    //追加2014/12/15hata
                    //欠陥データとGainデータをtransImageCtrlへセットする
                    CTSettings.transImageControl.SetDefGain(ScanCorrect.Def_IMAGE, ScanCorrect.GAIN_IMAGE, adv);
                }
			}

            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            /*
            rc = Ipc32v5.IpAppCloseAll();													//☆☆開いている全ての画像ｳｨﾝﾄﾞを閉じる
			rc = Ipc32v5.IpWsCreate(CTSettings.scancondpar.Data.fimage_hsize, CTSettings.scancondpar.Data.fimage_vsize, 300, Ipc32v5.IMC_GRAY16);			//空の画像ウィンドウを生成（Gray Scale 16形式）
			tmpReg.Left = 0;
			tmpReg.Top = 0;
			tmpReg.Right = CTSettings.scancondpar.Data.fimage_hsize - 1;
			tmpReg.Bottom = CTSettings.scancondpar.Data.fimage_vsize - 1;
			rc = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, IMG_FLU[0], Ipc32v5.CPROG);		//☆☆ユーザが作成した画像ﾃﾞｰﾀをImage-Proの画像に書込む
			rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);												//☆☆画像ウィンドの再描画

			//ヒストグラム処理
			//roiRect.Left = FRMWIDTH
			//roiRect.Top = FRMWIDTH
			//roiRect.Right = scancondpar.fimage_hsize - FRMWIDTH
			//roiRect.Bottom = scancondpar.fimage_vsize - FRMWIDTH
			//roiRect.Left = FRMWIDTH + IIf(DetType = DetTypePke, 40, 0)  'v17.00変更　山本 2010-02-09
			//roiRect.Top = FRMWIDTH + IIf(DetType = DetTypePke, 40, 0)   'v17.00変更　山本 2010-02-09
			//roiRect.Right = scancondpar.fimage_hsize - FRMWIDTH - IIf(DetType = DetTypePke, 40, 0)  'v17.00変更　山本 2010-02-09
			//roiRect.Bottom = scancondpar.fimage_vsize - FRMWIDTH - IIf(DetType = DetTypePke, 40, 0) 'v17.00変更　山本 2010-02-09
			roiRect.Left = ((modGlobal.DetType == modGlobal.DetectorConstants.DetTypePke) ? (CTSettings.scancondpar.Data.fimage_hsize % 100) / 2 : ScanCorrect.FRMWIDTH);		//v17.20変更 byやまおか 2010/09/17
			roiRect.Top = ((modGlobal.DetType == modGlobal.DetectorConstants.DetTypePke) ? (CTSettings.scancondpar.Data.fimage_vsize % 100) / 2 : ScanCorrect.FRMWIDTH);			//v17.20変更 byやまおか 2010/09/17
			roiRect.Right = CTSettings.scancondpar.Data.fimage_hsize - ((modGlobal.DetType == modGlobal.DetectorConstants.DetTypePke) ? (CTSettings.scancondpar.Data.fimage_hsize % 100) / 2 : ScanCorrect.FRMWIDTH);		//v17.20変更 byやまおか 2010/09/17
			roiRect.Bottom = CTSettings.scancondpar.Data.fimage_vsize - ((modGlobal.DetType == modGlobal.DetectorConstants.DetTypePke) ? (CTSettings.scancondpar.Data.fimage_vsize % 100) / 2 : ScanCorrect.FRMWIDTH);	//v17.20変更 byやまおか 2010/09/17
			rc = Ipc32v5.IpAoiCreateBox(ref roiRect);
			rc = Ipc32v5.IpHstCreate();														//☆☆ヒストグラムｳｨﾝﾄﾞを開く
			rc = Ipc32v5.IpHstGet(Ipc32v5.GETSTATS, 0, ref HistData[0]);					//☆☆輝度統計を取得　～Max：HistData(4)、Min：HistData(3)
			rc = Ipc32v5.IpHstDestroy();													//☆☆ヒストグラムｳｨﾝﾄﾞを閉る

            //画像を１６ビットフルレンジにして画像コントラストを改善する
			if ((HistData[3] != 0) || (HistData[4] != 0))			//v17.00 if追加　山本 2010-02-09
			{
				ScanCorrect.ChangeFullRange(ref IMG_FLU[0], CTSettings.scancondpar.Data.fimage_hsize, CTSettings.scancondpar.Data.fimage_vsize, HistData[3], HistData[4]);
			}

			rc = Ipc32v5.IpAppCloseAll();																		//☆☆開いている全ての画像ｳｨﾝﾄﾞを閉じる
			rc = Ipc32v5.IpWsCreate(CTSettings.scancondpar.Data.fimage_hsize, CTSettings.scancondpar.Data.fimage_vsize, 300, Ipc32v5.IMC_GRAY16);		//☆☆新規の画像ｳｨﾝﾄﾞを開く
			rc = Ipc32v5.IpDocPutArea(Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref IMG_FLU[0], Ipc32v5.CPROG);		//☆☆ユーザが作成した画像ﾃﾞｰﾀをImage-Proの画像に書込む
			rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);													//☆☆画像ウィンドの再描画

			//縦倍率に応じて、縦方向に拡大する
			//    rc = IpWsScale(Val_ImgHsize, Val_ImgVsize * Val_v_mag, 1)               '☆☆
			rc = Ipc32v5.IpWsScale(CTSettings.scancondpar.Data.fimage_hsize / modCT30K.phm, CTSettings.scancondpar.Data.fimage_vsize / modCT30K.pvm, 1);		//FPD対応 by 間々田 2003/09/26

            //フルカラーに変換
			rc = Ipc32v5.IpWsConvertImage(Ipc32v5.IMC_RGB, Ipc32v5.CONV_SCALE, 0, 0, 0, 0);	//ImagePro Ver5の場合
			if (SaveFormat != "bmp")		//v11.5追加 by 間々田 2006/10/20 ビットマップ形式（つまり透視画像表示の場合）はここでラインを描画しないことにした
			{
				//'水平線座標指定
				//x1 = 0
				//y1 = (-Val_ScanPosiA(iSCN) * Val_ImgHsize + 2 * Val_ScanPosiB(iSCN) + Val_ImgVsize) / pvm / 2           'FPD対応 by 間々田 2003/09/26
				//x2 = Val_ImgHsize / phm - 1                                                                             'FPD対応 by 間々田 2003/09/26
				//y2 = (Val_ScanPosiA(iSCN) * Val_ImgHsize + 2 * Val_ScanPosiB(iSCN) + Val_ImgVsize) / pvm / 2            'FPD対応 by 間々田 2003/09/26

				//水平線座標指定     'v11.5変更 by 間々田 2006/04/21
				x1 = 0;
                y1 = (int)((-CTSettings.scancondpar.Data.scan_posi_a[iSCN] * CTSettings.scancondpar.Data.fimage_hsize + 2 * CTSettings.scancondpar.Data.scan_posi_b[iSCN] + CTSettings.scancondpar.Data.fimage_vsize) / modCT30K.pvm / 2);		//FPD対応 by 間々田 2003/09/26
				x2 = (int)(CTSettings.scancondpar.Data.fimage_hsize / modCT30K.phm) - 1;																																					//FPD対応 by 間々田 2003/09/26
                y2 = (int)((CTSettings.scancondpar.Data.scan_posi_a[iSCN] * CTSettings.scancondpar.Data.fimage_hsize + 2 * CTSettings.scancondpar.Data.scan_posi_b[iSCN] + CTSettings.scancondpar.Data.fimage_vsize) / modCT30K.pvm / 2);		//FPD対応 by 間々田 2003/09/26

				//左右反転させる場合はy1とy2をスワップ   'v17.50追加 透視画像左右反転表示対応 by 間々田 2011/02/03
				if (modGlobal.IsLRInverse) modLibrary.Swap(ref y1, ref y2);

				rc = Ipc32v5.IpListPts(ref Ipc32v5.Pts[0], Convert.ToString(x1) + " " + Convert.ToString(y1) + " " + Convert.ToString(x2) + " " + Convert.ToString(y2));

			//v9.5 コンパイルオプションによる分岐を追加 by 間々田 2004/09/17
#if ImageProV3
				rc = Ipc32v5.IpPalSetRGBBrush(1, 0, 255, 0);					//線色を緑に設定
				rc = Ipc32v5.IpAnotLine(ref Ipc32v5.Pts[0], 2, 0, 0);			//線を描く
#else
				//v9.5 追加ここから Image-Pro Ver.5の場合  by 間々田 2004/09/17
				rc = Ipc32v5.IpAnCreateObj(Ipc32v5.GO_OBJ_POLY);									//線分オブジェクトの作成
				rc = Ipc32v5.IpAnPolyAddPtArray(ref Ipc32v5.Pts[0], 2);								//描画
				rc = Ipc32v5.IpAnSet(Ipc32v5.GO_ATTR_PENCOLOR, ColorTranslator.ToOle(Color.Lime));	//緑
				rc = Ipc32v5.IpAnBurn();															//書込む
				//v9.5 追加ここまで Image-Pro Ver.5の場合  by 間々田 2004/09/17
#endif
			}								//v11.5追加 by 間々田 2006/10/20

			//透視画像保存（*.jpg）
			//rc = IpWsSaveAs(FileName & "f.jpg", "JPG")  'v11.5削除 by 間々田 2006/06/08

			//v11.5追加ここから by 間々田 2006/06/08

			//保存ファイル名
			string SaveFileName = modLibrary.RemoveExtension(FileName, ".flu") + "f." + SaveFormat;

			//透視画像保存
			rc = Ipc32v5.IpWsSaveAs(SaveFileName, SaveFormat);
            */
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            //Roiパラメータ
            //2014/11/07hata キャストの修正
            //roiRect.left = ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) ? (CTSettings.scancondpar.Data.fimage_hsize % 100) / 2 : ScanCorrect.FRMWIDTH);		//v17.20変更 byやまおか 2010/09/17
            //roiRect.top = ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) ? (CTSettings.scancondpar.Data.fimage_vsize % 100) / 2 : ScanCorrect.FRMWIDTH);			//v17.20変更 byやまおか 2010/09/17
            //roiRect.right = CTSettings.scancondpar.Data.fimage_hsize - ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) ? (CTSettings.scancondpar.Data.fimage_hsize % 100) / 2 : ScanCorrect.FRMWIDTH);		//v17.20変更 byやまおか 2010/09/17
            //roiRect.bottom = CTSettings.scancondpar.Data.fimage_vsize - ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) ? (CTSettings.scancondpar.Data.fimage_vsize % 100) / 2 : ScanCorrect.FRMWIDTH);	//v17.20変更 byやまおか 2010/09/17
            //Rev20.00 変更 by長野 2015/02/06
            //roiRect.left = ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) ? Convert.ToInt32((CTSettings.scancondpar.Data.fimage_hsize % 100) / 2F) : ScanCorrect.FRMWIDTH);		//v17.20変更 byやまおか 2010/09/17
            //roiRect.top = ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) ? Convert.ToInt32((CTSettings.scancondpar.Data.fimage_vsize % 100) / 2F) : ScanCorrect.FRMWIDTH);			//v17.20変更 byやまおか 2010/09/17
            //roiRect.right = CTSettings.scancondpar.Data.fimage_hsize - ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) ? Convert.ToInt32((CTSettings.scancondpar.Data.fimage_hsize % 100) / 2F) : ScanCorrect.FRMWIDTH);		//v17.20変更 byやまおか 2010/09/17
            //roiRect.bottom = CTSettings.scancondpar.Data.fimage_vsize - ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) ? Convert.ToInt32((CTSettings.scancondpar.Data.fimage_vsize % 100) / 2F) : ScanCorrect.FRMWIDTH);	//v17.20変更 byやまおか 2010/09/17
            
            roiRect.left = ScanCorrect.FRMWIDTH;
            roiRect.top = ScanCorrect.FRMWIDTH;
            roiRect.right = CTSettings.scancondpar.Data.fimage_hsize - ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) ? 2 * modX: ScanCorrect.FRMWIDTH);		//v17.20変更 byやまおか 2010/09/17
            roiRect.bottom = CTSettings.scancondpar.Data.fimage_vsize - ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) ? 2 * modY: ScanCorrect.FRMWIDTH);	//v17.20変更 byやまおか 2010/09/17

            //左右反転させるパラメータ
            int LRInverse = Convert.ToInt32(CTSettings.detectorParam.IsLRInverse);
            //水平線座標指定     'v11.5変更 by 間々田 2006/04/21
            Horizonx1 = 0;
            //2014/11/07hata キャストの修正
            //int Horizony1 = (int)((-CTSettings.scancondpar.Data.scan_posi_a[iSCN] * CTSettings.scancondpar.Data.fimage_hsize + 2 * CTSettings.scancondpar.Data.scan_posi_b[iSCN] + CTSettings.scancondpar.Data.fimage_vsize) / CTSettings.detectorParam.pvm / 2);		//FPD対応 by 間々田 2003/09/26
            //int Horizonx2 = (int)(CTSettings.scancondpar.Data.fimage_hsize / CTSettings.detectorParam.phm) - 1;																																					//FPD対応 by 間々田 2003/09/26
            //int Horizony2 = (int)((CTSettings.scancondpar.Data.scan_posi_a[iSCN] * CTSettings.scancondpar.Data.fimage_hsize + 2 * CTSettings.scancondpar.Data.scan_posi_b[iSCN] + CTSettings.scancondpar.Data.fimage_vsize) / CTSettings.detectorParam.pvm / 2);		//FPD対応 by 間々田 2003/09/26
            Horizonx2 = Convert.ToInt32(CTSettings.scancondpar.Data.fimage_hsize / CTSettings.detectorParam.phm - 1);																																					//FPD対応 by 間々田 2003/09/26

            //Rev20.00 条件を追加 by長野 2015/02/05
            if(CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
                Horizony1 = Convert.ToInt32((CTSettings.scancondpar.Data.scan_posi_a[iSCN] * CTSettings.scancondpar.Data.fimage_hsize + 2 * CTSettings.scancondpar.Data.scan_posi_b[iSCN] + CTSettings.scancondpar.Data.fimage_vsize) / CTSettings.detectorParam.pvm / 2F);		//FPD対応 by 間々田 2003/09/26
                Horizony2 = Convert.ToInt32((-CTSettings.scancondpar.Data.scan_posi_a[iSCN] * CTSettings.scancondpar.Data.fimage_hsize + 2 * CTSettings.scancondpar.Data.scan_posi_b[iSCN] + CTSettings.scancondpar.Data.fimage_vsize) / CTSettings.detectorParam.pvm / 2F);		//FPD対応 by 間々田 2003/09/26
            }
            else
            {
                Horizony1 = Convert.ToInt32((-CTSettings.scancondpar.Data.scan_posi_a[iSCN] * CTSettings.scancondpar.Data.fimage_hsize + 2 * CTSettings.scancondpar.Data.scan_posi_b[iSCN] + CTSettings.scancondpar.Data.fimage_vsize) / CTSettings.detectorParam.pvm / 2F);		//FPD対応 by 間々田 2003/09/26
                Horizony2 = Convert.ToInt32((CTSettings.scancondpar.Data.scan_posi_a[iSCN] * CTSettings.scancondpar.Data.fimage_hsize + 2 * CTSettings.scancondpar.Data.scan_posi_b[iSCN] + CTSettings.scancondpar.Data.fimage_vsize) / CTSettings.detectorParam.pvm / 2F);		//FPD対応 by 間々田 2003/09/26
            }

           
            //Rev20.00 コーン撮影範囲を計算 by長野 2015/02/05
            if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone)
            {
                ImageInfo theInfo = new ImageInfo();
            
                if (ImageInfo.ReadImageInfo(ref theInfo.Data, modFileIO.RemoveExtensionEx(FileName)))
                {
                    float theSlicePitch = theInfo.Data.delta_z;
                    float theSliceWidth = 0;
                    float.TryParse(theInfo.Data.width.GetString(), out theSliceWidth);
                    int theSliceNum = theInfo.Data.k;
                    float FCD = theInfo.Data.fcd;
                    float FDD = theInfo.Data.fid;
                    float r = FCD / FDD;
                    Swad = ((float)theSlicePitch * ((float)theSliceNum - 1) + (float)theSliceWidth) / 2F / r / CTSettings.scancondpar.Data.dpm;
                }

                Horizony1UCone = (int)(Horizony1 - Swad / modCT30K.vScale);
                Horizony2UCone = (int)(Horizony2 - Swad / modCT30K.vScale);
                Horizony1LCone = (int)(Horizony1 + Swad / modCT30K.vScale);
                Horizony2LCone = (int)(Horizony2 + Swad / modCT30K.vScale);
            }
            else
            {
                Horizony1UCone = 0;
                Horizony2UCone = 0;
                Horizony1LCone = 0;
                Horizony2LCone = 0;
            }
              


            //保存ファイル名
            string SaveFileName = modLibrary.RemoveExtension(FileName, ".flu") + "f." + SaveFormat;
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
                rc = CallImageProFunction.CallDrawHorizon(IMG_FLU2,
                     CTSettings.scancondpar.Data.fimage_vsize - modY * 2,
                     CTSettings.scancondpar.Data.fimage_hsize - modX * 2,
                     CTSettings.detectorParam.phm,
                     CTSettings.detectorParam.pvm,
                     roiRect.left,
                     roiRect.top,
                     roiRect.right,
                     roiRect.bottom,
                     Horizonx1,
                     Horizony1,
                     Horizonx2,
                     Horizony2,
                     LRInverse,
                     SaveFormat,
                     SaveFileName,
                     Horizony1UCone,//Rev20.00 引数追加 by長野 2015/02/06
                     Horizony2UCone,//Rev20.00 引数追加 by長野 2015/02/06
                     Horizony1LCone,//Rev20.00 引数追加 by長野 2015/02/06
                     Horizony2LCone //Rev20.00 引数追加 by長野 2015/02/06
                     );
            }
            else
            {
                rc = CallImageProFunction.CallDrawHorizon(IMG_FLU,
                    CTSettings.scancondpar.Data.fimage_vsize,
                    CTSettings.scancondpar.Data.fimage_hsize,
                    CTSettings.detectorParam.phm,
                    CTSettings.detectorParam.pvm,
                    roiRect.left,
                    roiRect.top,
                    roiRect.right,
                    roiRect.bottom,
                    Horizonx1,
                    Horizony1,
                    Horizonx2,
                    Horizony2,
                    LRInverse,
                    SaveFormat,
                    SaveFileName,
                    Horizony1UCone,//Rev20.00 引数追加 by長野 2015/02/06
                    Horizony2UCone,//Rev20.00 引数追加 by長野 2015/02/06
                    Horizony1LCone,//Rev20.00 引数追加 by長野 2015/02/06
                    Horizony2LCone //Rev20.00 引数追加 by長野 2015/02/06                    
                    );
            }
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//
  
			//保存に成功した場合、保存ファイル名を戻り値として返す
			if (rc == 0) functionReturnValue = SaveFileName;

			//v11.5追加ここまで by 間々田 2006/06/08

			//元画像削除（*.flu）
			//Call Kill(FileName & ".flu")
			if (DelFLU) File.Delete(FileName);					//v11.5変更 by 間々田 2006/06/08

			return functionReturnValue;
		}


		//********************************************************************************
		//機    能  ：  プロセス終了待ち処理の改良
		//              変数名           [I/O] 型        内容
		//引    数  ：  obj              [I/ ] Object    ???????
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V1.00  99/07/31  (CATE)山本      新規作成
		//********************************************************************************
		//Public Sub dwAppTerminated(obj As Object)
		public void EndProcessScan(int ReturnCode = 0)		//v11.5変更 by 間々田 2006/07/03
		{
			//int long_data = 0;			//整数型の読み書き用
			//int error_sts = 0;			//戻り値
			bool IsScanOK = false;		//スキャンが正常かどうかのフラグ

            //Stopwatch処理//// Rev26.00 by井上 ///////////////////////////////////////////////////////////////////////////
            //sw.Stop();

            //double ScanTimes = sw.Elapsed.TotalSeconds;

            //System.IO.StreamReader sr = new System.IO.StreamReader(@"C:\CT30Kv26.00_20170310\TimeMeasure\condition.txt",
            //    System.Text.Encoding.GetEncoding("shift_jis"));
            //string CondName = sr.ReadToEnd();

            //sr.Close();

            //string basePath = "C:\\CT30Kv26.00_20170310\\TimeMeasure";
            //string absolutePath = System.IO.Path.Combine(basePath, CondName);

            //System.IO.StreamWriter rs = new System.IO.StreamWriter(absolutePath,
            //    true, System.Text.Encoding.GetEncoding("shift_jis"));
            //rs.WriteLine(ScanTimes);

            //rs.Close();

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////


			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//進捗ステータス非表示
			//    Unload frmScanStatus
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            ////Rev20.00 ここで、確実に破棄 by長野 2014/08/27
            //for (int cnt = 0; cnt < 30; cnt++)
            //{
            //    if (CTSettings.transImageControl.hSharedCTDataMap[cnt] != IntPtr.Zero)
            //    {

            //        TransImageControl.DestroySharedCTConeDataMap(0, CTSettings.transImageControl.hSharedCTDataMap[cnt]);
            //        CTSettings.transImageControl.hSharedCTDataMap[cnt] = IntPtr.Zero;
            //    }
            //}

            ////Rev20.00 ここで、確実に破棄 by長野 2014/08/27
            //for (int cnt = 0; cnt < 30; cnt++)
            //{
            //    if (CTSettings.transImageControl.hSharedCTDataMap[cnt] != IntPtr.Zero)
            //    {
            //        TransImageControl.DestroySharedCTSingleDataMap(CTSettings.transImageControl.hSharedCTDataMap[cnt]);
            //        CTSettings.transImageControl.hSharedCTDataMap[cnt] = IntPtr.Zero;
            //    }
            //}

            //Rev20.00 共有メモリ破棄 by長野 2014/09/11
            CTSettings.transImageControl.DestroyCTScanObj();

            //Rev20.00 正常終了なら、ここで原点復帰 by長野 2015/02/21
            if (ReturnCode == 0)
            {
                MechaControl.RotateIndex(modDeclare.hDevID1, 0, 0, null); ;
            }

            //エラー処理  added V2.0 by 鈴山
			try
			{
                if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)		//v17.00追加 byやまおか 2010/02/04
				{
                    if (modCT30K.tmp_on == 1) CTSettings.scanParam.FPGainOn = true;		//ゲイン補正フラッグを元に戻す 2009-09-30
				}

				//scanav.exeやconebeam.exeからの送信を拒否 'v11.5追加 by 間々田 2006/06/21
				//    frmCTMenu.socCT30K.Close

				//Ｘ線外部制御可能の場合、Ｘ線をＯＦＦする       'V3.0 append by 鈴山
				if (CTSettings.scaninh.Data.xray_remote == 0)
				{
					//Ｘ線オンの場合のみＸ線をオフする   'v11.5条件追加 by 間々田 2006/08/01 この処理のあと、直ちにスキャンにいくとＸ線がオンしない場合があるので、その対策。
					//                                                                       なお通常は、scanav.exeやconebeam.exe側でＸ線オフしているはず。
					if (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus)
					{
						modXrayControl.XrayOff();
					}
				}

				//v16.00 MPR画像自動表示対応のためコメントアウト by 長野 2010/01/07
				//CT30Kをアクティブにする                        'v10.0追加 by 間々田 2005/02/14
				//    AppActivate App.Title
				//    DoEvents
				//高速再構成じゃないときはCT30Kをアクティブにする    'v17.10変更 byやまおか 2010/07/28
				if (CTSettings.scaninh.Data.gpgpu == 1)
				{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//					AppActivate App.Title
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
					frmCTMenu.Instance.Activate();
					Application.DoEvents();
				}

				//frmInformation.Refresh                  'added by 山本　2004-6-17　マルチスキャンをストップした時にスキャン中の付帯情報が残ってしまう対策

				//'シーケンサ通信チェックプログラムを破棄             'v9.7 追加 by 間々田 2004/11/11
				//If Not (MyCommCheck Is Nothing) Then Set MyCommCheck = Nothing                         'v11.4削除 by 間々田 2006/03/06
                //変更2014/10/07hata_v19.51反映
                //frmMechaControl.Instance.tmrSeqComm.Interval = 500;											//v11.4追加 by 間々田 2006/03/06
                frmMechaControl.Instance.tmrMecainfSeqComm.Interval = 1000; //v19.50 タイマーの統合 by長野 2013/12/17

				//エラーコードをチェック
				//'IsScanOK = Not IsScanError()
				//IsScanOK = Not IsScanError(True)        'v9.1 メール送信がらみで引数追加 by 間々田 2004/05/13

				//v11.5以下に変更 by 間々田 2006/07/03
				//メール送信ありの場合 追加 by 間々田 2004/05/13
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
				//    If (scaninh.mail_send = 0) And (scansel.mail_send = 1) Then
				//        CallSendMail ReturnCode '1902（オペレータによる中止）の時もメール送信するように変更 by 間々田 2004/05/20
				//        PauseForDoEvents 1#         'added by 山本　2004-7-21
				//    End If
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

				//v17.40 ここから追加 by 長野
				//非常停止ボタンが押された場合、VB側からuserstopでスキャンを止めるように変更したので、ReturnCode1902と
				//mecainf.emergency or emergencyButton_Flg で非常停止による終了かを判別する by 長野 2010/10/21
				//    'タイマーによるmecainfの更新がとまっているので、ここで直接呼ぶ
				//    GetMecainf mecainf

				if ((ReturnCode == 1902) && ((CTSettings.mecainf.Data.emergency == 1) || (modCT30K.emergencyButton_Flg == true)) || (modSeqComm.MySeq.stsEmergency))
				{
					ReturnCode = 1903;
				}

				if (modCT30K.emergencyButton_Flg == true)
				{
					//emergencyButton_Flgを必ずfalseで初期化する
					modCT30K.emergencyButton_Flg = false;
				}

				//v17.40 ここまで追加 by 長野

				if (ReturnCode == 1901 || ReturnCode == 0)		//正常終了時またはスキャンエラーコードが０の時
				{
					IsScanOK = true;
				}
                //else //Rev23.20 非常停止orスキャンストップ(放電含む)以外でスキャンを停止した場合は、シーケンサにエラーしたことを通知 by長野 2016/01/19
                else if (ReturnCode == 1902 || ReturnCode == 1903)
                {
                    //Rev23.13 exeをストップするタイミングによっては、ExScanStart内で待機状態になるため、ここでフラグをONにして確実に終了させる by長野 2016/03/08
                    modScanCondition.ExScanStartAbortFlg = true;

                    modCT30K.ErrMessage(ReturnCode, Icon: MessageBoxIcon.Error);	//異常終了時はエラーメッセージを表示
                    frmScanControl.Instance.ctbtnScanStart.Enabled = true;			//異常時はここでスタートボタンを有効にする   'v17.43/v17.53追加 byやまおか 2011/02/01
                    IsScanOK = false;
                }
                else
                {
                    //Rev23.13 exeをストップするタイミングによっては、ExScanStart内で待機状態になるため、ここでフラグをONにして確実に終了させる by長野 2016/03/08
                    modScanCondition.ExScanStartAbortFlg = true;

                    modSeqComm.MySeq.BitWrite("ScanPCErr", true);
                    modCT30K.ErrMessage(ReturnCode, Icon: MessageBoxIcon.Error);	//異常終了時はエラーメッセージを表示
                    modSeqComm.MySeq.BitWrite("ScanPCErr", false);
                    frmScanControl.Instance.ctbtnScanStart.Enabled = true;			//異常時はここでスタートボタンを有効にする   'v17.43/v17.53追加 byやまおか 2011/02/01
                    IsScanOK = false;
                }

				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
				//コーン分散処理の場合、分散処理ステータスフォームを元に戻す v8.0 added by 間々田 2003/12/19
				//    If IsExistForm(frmDistributeStatus) Then
				//        frmDistributeStatus.Top = 5910
				//        frmDistributeStatus.Refresh
				//        DoEvents
				//    End If
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

				//メカ再オープン
				//Rev20.00 不要のためコメントアウト by長野 2015/03/04
                //modMechaControl.MechaOpen();
                modMechaControl.Mechastatus_check(modDeclare.hDevID1);

				//scancondparの取得
				//modScancondpar.CallGetScancondpar();		//v11.5下から移動 by 間々田 2006/04/26
                CTSettings.scancondpar.Load(CTSettings.scaninh.Data.rotate_select);

                //Rev20.00 64bit化により廃止 by長野 2014/11/04
                ////追加2014/10/07hata_v19.51反映
                ////v19.51 テーブル連続回転の場合、dev_klimitの値を元に戻す by長野 2014/03/04
                //if ((modCT30K.TableRotOn))
                //{
                //    CTSettings.scancondpar.Data.dev_klimit = bak_dev_klimit;
                //}

				//Scanselの内容を更新    'v10.0追加 by 間々田 2005/02/14(回転中心校正自動スキャンの場合Scansel.mscan_areaが変更されるため)
				//GetScansel
				modCommon.GetMyScansel();													//v11.2追加 by 間々田 2006/01/13

				if (IsScanOK)
				{
                    
					//透視画像保存が選択されていたら、透視画像へ水平線を付加する
					//if ((CTSettings.scansel.Data.fluoro_image_save == 1) && (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScan))
                    //Rev20.00 コーンでも保存できるようにする by長野 2015/02/06
                    if ((CTSettings.scansel.Data.fluoro_image_save == 1))
                    {
						DrawHorizon_AfterScan();
					}

					//オートセンタリングスキャン時   added by 山本 2001-4-10
					if (CTSettings.scansel.Data.auto_centering == 1)
					{
						//Set_No_Movement_for_RotationCenter

						//回転中心用X、Y、II軸移動をなしに設定
						modSeqComm.SeqBitWrite("RotXChangeReset", true);
						modSeqComm.SeqBitWrite("RotYChangeReset", true);
						modSeqComm.SeqBitWrite("RotIIChangeReset", true);

						//mecainf（コモン）の更新
						UpdateMecainf();
					}

					//オートプリントかつスライスプランでない場合は画面を印刷する
					if ((CTSettings.scansel.Data.auto_print == 1) && (!(CTSettings.scansel.Data.multiscan_mode == 5)))
					{
						frmCTMenu.Instance.Focus();
						Application.DoEvents();
						//ImagePrint.DoPrint();
                        //Rev20.00 変更 by長野 2015/02/06
                        ImagePrint.DoPrint(1);
                    }

                    //Rev23.10 検出器がシフト位置の場合は、元に戻す
                    //検出器が基準位置にいない場合は基準位置に移動する   'v18.00追加 byやまおか 2011/02/27
                    //if (ScanCorrect.IsShiftScan())
                    //Rev25.00 Wスキャンを条件に追加 by長野 2016/08/08
                    if (ScanCorrect.IsShiftScan() || ScanCorrect.IsW_Scan())
                    {
                        //If (Not ShiftDet(DetShift_origin)) Then
                        //ゲインをセットしない   'v18.00変更 byやまおか 2011/07/04
                        //if ((!(modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_origin, modDetShift.UNSET_GAIN))))
                        //Rev25.01 修正 by長野 2016/12/09
                        if ((!(modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_origin, modDetShift.SET_GAIN))))
                        {
                            //                MsgBox "検出器シフトに失敗しました。", vbExclamation
                            //Interaction.MsgBox(CT30K.My.Resources.str21314, MsgBoxStyle.Exclamation);
                            MessageBox.Show(CTResources.LoadResString(21314), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }

                    //収集番号を１カウントアップし、スキャン済みフラッグをスキャン済みにする V6.0 append by 間々田 2002/08/20
					//CallGetScancondpar 'v11.5上に移動 by 間々田 2006/04/26
					CTSettings.scancondpar.Data.acq_num = CTSettings.scancondpar.Data.acq_num + 1;	//収集番号
					CTSettings.scancondpar.Data.scan_comp = 1;										//スキャン済みフラッグ：0(未スキャン),1(スキャン済み)
					//modScancondpar.CallPutScancondpar();
                    CTSettings.scancondpar.Write();

					//今回の画像保存先を求めておく           'v15.0追加 by 間々田 2009/06/16
                    modCT30K.DestOld = Path.Combine(CTSettings.scansel.Data.pro_code.GetString(), CTSettings.scansel.Data.pro_name.GetString());
                    
				}
				else
				{
                    //Rev23.12 純生データONの場合は、保存済みのデータを削除 by長野 2015/12/28
                    if (CTSettings.scansel.Data.pur_rawdata_save == 1)
                    {
                        modScanCondition.DeletePurData();
                    }
					//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
					//分散スキャンキャンセルフラグを立てる　added by 山本　2004-3-2
					//       GFlg_Distribute_Cancel = 1
					//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
				}
			}
			finally
			{
                modCT30K.PauseForDoEvents(1);   //v18.00追加 byやまおか 2011/07/04

                //v17.60/v18.00 ここへ移動 byやまおか 2011/03/14
				//CT30K側のMILを再度オープンする
                CTSettings.scanParam.fpd_gain = CTSettings.scansel.Data.fpd_gain;
                CTSettings.scanParam.fpd_integ = CTSettings.scansel.Data.fpd_integ;
                
                frmTransImage.Instance.CaptureOpen();

				//終了時はフラグをリセット
				modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTScanStart);
				modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTMechaBusy);		//追加 by 間々田 2009/06/16

                //Rev20.01 追加 by長野 2015/06/03
                frmXrayControl.Instance.UpdateWarmUp();

				//mecainf更新のためのタイマーを再開
                //v19.50 タイマーの統合 by長野 2013/12/17    //変更2014/10/07hata_v19.51反映
                //frmMechaControl.Instance.tmrMecainf.Enabled = true;
                modMechaControl.Flg_MechaControlUpdate = true;

				//タッチパネル操作を許可（シーケンサ通信が可能な場合）
				modSeqComm.SeqBitWrite("PanelInhibit", false);

				//外部トリガ取込みかつテーブル回転が連続回転の時             'V7.0 append by 間々田 2003/09/10
				if (CTSettings.scaninh.Data.ext_trig == 0 && modCT30K.TableRotOn) modSeqComm.SeqBitWrite("TrgReq", false);

				//画面情報を表示する。(※メニューバー更新の為)
				//    tmrStatus_Timer

                //オートセンタリングスキャンが終了したらオートセンタリングをなしにする
                MecaInf theMecainf = new MecaInf();
                theMecainf.Data.Initialize();
                theMecainf.Load();
                //Rev21.00/Rev20.00 スキャン成否に関わらず、Mecainfは取得する by長野 2015/03/19
                if (IsScanOK && (CTSettings.scansel.Data.auto_centering == 1))
                {
                    frmScanControl.Instance.chkInhibit[3].CheckState = CheckState.Checked;

                    //Rev21.00/Rev20.00 スキャン成否に関わらず、Mecainfは取得する by長野 2015/03/19
                    ////追加2014/10/07hata_v19.51反映
                    ////mecainf（コモン）取得
                    ////GetMecainf(theMecainf);
                    //theMecainf.Load();
                    
                    //焦点を記憶
                    theMecainf.Data.rc_focus = CTSettings.mecainf.Data.xfocus;  //v18.00変更 byやまおか 2011/06/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    
                    //mecainf（コモン）更新
                    //PutMecainf(theMecainf);
                    theMecainf.Write();
				}

                //Rev21.00/20.00 追加 by長野 2015/03/12
                frmMechaControl.Instance.UpdateMecha();
                //Rev20.01 修正 by長野 2015/05/20
                //frmMechaControl.Instance.cwnePos.Value = Convert.ToDecimal(Math.Round(theMecainf.Data.udab_pos, 3, MidpointRounding.AwayFromZero));
                //if(((Convert.ToDecimal(Math.Round(theMecainf.Data.udab_pos, 3, MidpointRounding.AwayFromZero))) <= frmMechaControl.Instance.cwnePos.Maximum) && 
                //(frmMechaControl.Instance.cwnePos.Minimum <= (Convert.ToDecimal(Math.Round(theMecainf.Data.udab_pos, 3, MidpointRounding.AwayFromZero)))))
                //{
                //   frmMechaControl.Instance.cwnePos.Value = Convert.ToDecimal(Math.Round(theMecainf.Data.udab_pos, 3, MidpointRounding.AwayFromZero));
                //}
                //Rev23.10 計測CT対応 by長野 2015/10/16
                if(((Convert.ToDecimal(Math.Round(frmMechaControl.Instance.Udab_Pos, 3, MidpointRounding.AwayFromZero))) <= frmMechaControl.Instance.cwnePos.Maximum) &&
                    (frmMechaControl.Instance.cwnePos.Minimum <= (Convert.ToDecimal(Math.Round(frmMechaControl.Instance.Udab_Pos, 3, MidpointRounding.AwayFromZero)))))
                {
                    frmMechaControl.Instance.cwnePos.Value = Convert.ToDecimal(Math.Round(frmMechaControl.Instance.Udab_Pos, 3, MidpointRounding.AwayFromZero));
                }
                
                //Rev26.10/25.15 昇降現在値と昇降位置指定の値を確実にそろえる by chouno 2018/01/13
                frmMechaControl.Instance.txtUpDownPos.Text = frmMechaControl.Instance.cwnePos.Value.ToString("000.000");

                //Rev21.00 削除 by長野 2015/03/12
                //Rev20.00 昇降位置と、昇降位置指定の値をそろえる by長野 2015/03/06
                //frmMechaControl.Instance.cwnePos_ValueChanged(null, EventArgs.Empty);

				//v11.5削除ここから by 間々田 2006/06/12
				//'プロパティ読み込み         'V3.0 append by 鈴山 2000/10/02 (ｲﾍﾞﾝﾄ取りこぼし対策)
				//'Get_AllProperty
				//If scaninh.xray_remote = 0 Then SetCommonLong "mecainf", "xray_avl", IIf(IsXrayAvailable, 1, 0)
				//
				//'Ｘ線ステータスの送信       'V4.0 append by 鈴山 2001/02/08
				//'Check_SeqXray
				//If scaninh.xray_remote = 0 Then SeqBitWrite "stsXrayOn", IsXrayOn, False 'v11.3変更 by 間々田 2006/02/20
				//v11.5削除ここまで by 間々田 2006/06/12

				//v17.40 追加 by 長野
				//X線停止要求クリア

				//UserStopClear
				//
				// '連続回転コーンビーム＋高速再構成の時は、RAMディスクのscanstopを使う v17.40 追加 by 長野
				//If smooth_rot_cone_flg = True Then
				//
				//    UserStopClear_rmdsk
				//
				//End If
                
                //Rev20.00 追加 by長野 2015/02/09
                //正常終了時にも最終処理でストップ処理を入れて、正常終了後の透視画像の階調変換ができるようにする措置
                CTSettings.transImageControl.CaptureScanStop();

				//停止要求フラグをクリアする             'v17.50上記の処理を関数化 by 間々田 2011/02/17
				modCT30K.CallUserStopClear();

				//マウスカーソルを元に戻す
				Cursor.Current = Cursors.Default;

				//v17.60/v18.00 上へ移動 byやまおか 2011/03/14
				//'CT30K側のMILを再度オープンする
				//frmTransImage.CaptureOpen
			}
		}
        //********************************************************************************
        //機    能  ：  プロセス終了待ち処理の改良(スキャノ用)
        //              変数名           [I/O] 型        内容
        //引    数  ：  obj              [I/ ] Object    ???????
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V21.00  15/03/08  (検S1)長野      新規作成
        //********************************************************************************
        //Public Sub dwAppTerminated(obj As Object)
        public void EndProcessScano(int ReturnCode = 0)		//Rev21.00 スキャノ用追加 by長野 2015/03/06
        {
            //int long_data = 0;			//整数型の読み書き用
            //int error_sts = 0;			//戻り値
            bool IsScanOK = false;		//スキャンが正常かどうかのフラグ

            //Rev20.00 共有メモリ破棄 by長野 2014/09/11
            CTSettings.transImageControl.DestroyCTScanObj();

            ////Rev20.00 正常終了なら、ここで原点復帰 by長野 2015/02/21
            //if (ReturnCode == 0)
            //{
            //    MechaControl.RotateIndex(modDeclare.hDevID1, 0, 0, null); ;
            //}

            //エラー処理  added V2.0 by 鈴山
            try
            {
                if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)		//v17.00追加 byやまおか 2010/02/04
                {
                    if (modCT30K.tmp_on == 1) CTSettings.scanParam.FPGainOn = true;		//ゲイン補正フラッグを元に戻す 2009-09-30
                }

                //scanav.exeやconebeam.exeからの送信を拒否 'v11.5追加 by 間々田 2006/06/21
                //    frmCTMenu.socCT30K.Close

                //Ｘ線外部制御可能の場合、Ｘ線をＯＦＦする       'V3.0 append by 鈴山
                if (CTSettings.scaninh.Data.xray_remote == 0)
                {
                    //Ｘ線オンの場合のみＸ線をオフする   'v11.5条件追加 by 間々田 2006/08/01 この処理のあと、直ちにスキャンにいくとＸ線がオンしない場合があるので、その対策。
                    //                                                                       なお通常は、scanav.exeやconebeam.exe側でＸ線オフしているはず。
                    if (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus)
                    {
                        modXrayControl.XrayOff();
                    }
                }

                //v16.00 MPR画像自動表示対応のためコメントアウト by 長野 2010/01/07
                //CT30Kをアクティブにする                        'v10.0追加 by 間々田 2005/02/14
                //    AppActivate App.Title
                //    DoEvents
                //高速再構成じゃないときはCT30Kをアクティブにする    'v17.10変更 byやまおか 2010/07/28
                if (CTSettings.scaninh.Data.gpgpu == 1)
                {
                    #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                    //					AppActivate App.Title
                    #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
                    frmCTMenu.Instance.Activate();
                    Application.DoEvents();
                }

                //frmInformation.Refresh                  'added by 山本　2004-6-17　マルチスキャンをストップした時にスキャン中の付帯情報が残ってしまう対策

                //'シーケンサ通信チェックプログラムを破棄             'v9.7 追加 by 間々田 2004/11/11
                //If Not (MyCommCheck Is Nothing) Then Set MyCommCheck = Nothing                         'v11.4削除 by 間々田 2006/03/06
                //変更2014/10/07hata_v19.51反映
                //frmMechaControl.Instance.tmrSeqComm.Interval = 500;											//v11.4追加 by 間々田 2006/03/06
                frmMechaControl.Instance.tmrMecainfSeqComm.Interval = 1000; //v19.50 タイマーの統合 by長野 2013/12/17

                if ((ReturnCode == 1902) && ((CTSettings.mecainf.Data.emergency == 1) || (modCT30K.emergencyButton_Flg == true)) || (modSeqComm.MySeq.stsEmergency))
                {
                    ReturnCode = 1903;
                }

                if (modCT30K.emergencyButton_Flg == true)
                {
                    //emergencyButton_Flgを必ずfalseで初期化する
                    modCT30K.emergencyButton_Flg = false;
                }

                if (ReturnCode == 1901 || ReturnCode == 0)		//正常終了時またはスキャンエラーコードが０の時
                {
                    IsScanOK = true;
                }
                //else //Rev23.20 非常停止orスキャンストップ(放電含む)を介さずにスキャンを停止した場合は、シーケンサにエラーしたことを通知 by長野 2016/01/19
                else if (ReturnCode == 1902 || ReturnCode == 1903)
                {
                    modCT30K.ErrMessage(ReturnCode, Icon: MessageBoxIcon.Error);	//異常終了時はエラーメッセージを表示
                    frmScanControl.Instance.ctbtnScanStart.Enabled = true;			//異常時はここでスタートボタンを有効にする   'v17.43/v17.53追加 byやまおか 2011/02/01
                    IsScanOK = false;
                }
                else
                {
                    modSeqComm.MySeq.BitWrite("ScanPCErr", true);
                    modCT30K.ErrMessage(ReturnCode, Icon: MessageBoxIcon.Error);	//異常終了時はエラーメッセージを表示
                    modSeqComm.MySeq.BitWrite("ScanPCErr", false);
                    frmScanControl.Instance.ctbtnScanStart.Enabled = true;			//異常時はここでスタートボタンを有効にする   'v17.43/v17.53追加 byやまおか 2011/02/01
                    IsScanOK = false;
                }

                //scancondparの取得
                //modScancondpar.CallGetScancondpar();		//v11.5下から移動 by 間々田 2006/04/26
                CTSettings.scancondpar.Load(CTSettings.scaninh.Data.rotate_select);

                //Scanselの内容を更新    'v10.0追加 by 間々田 2005/02/14(回転中心校正自動スキャンの場合Scansel.mscan_areaが変更されるため)
                //GetScansel
                modCommon.GetMyScansel();													//v11.2追加 by 間々田 2006/01/13

                if (IsScanOK)
                {
                    //Rev21.00 スキャノの場合は、tiff画像を作成する
                    if ((CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScano))
                    {
                        ConvertToTiffForScano();
                    }

                    //収集番号を１カウントアップし、スキャン済みフラッグをスキャン済みにする V6.0 append by 間々田 2002/08/20
                    //CallGetScancondpar 'v11.5上に移動 by 間々田 2006/04/26
                    CTSettings.scancondpar.Data.acq_num = CTSettings.scancondpar.Data.acq_num + 1;	//収集番号
                    CTSettings.scancondpar.Data.scan_comp = 1;										//スキャン済みフラッグ：0(未スキャン),1(スキャン済み)
                    //modScancondpar.CallPutScancondpar();
                    CTSettings.scancondpar.Write();

                    //今回の画像保存先を求めておく           'v15.0追加 by 間々田 2009/06/16
                    modCT30K.DestOld = Path.Combine(CTSettings.scansel.Data.pro_code.GetString(), CTSettings.scansel.Data.pro_name.GetString());

                }
                else
                {
                    //Rev23.12 純生データONの場合は、保存済みのデータを削除 by長野 2015/12/28
                    if (CTSettings.scansel.Data.pur_rawdata_save == 1)
                    {
                        modScanCondition.DeletePurData();
                    }
                    //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
                    //分散スキャンキャンセルフラグを立てる　added by 山本　2004-3-2
                    //       GFlg_Distribute_Cancel = 1
                    //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
                }
            }
            finally
            {
                modCT30K.PauseForDoEvents(1);   //v18.00追加 byやまおか 2011/07/04

                //v17.60/v18.00 ここへ移動 byやまおか 2011/03/14
                //CT30K側のMILを再度オープンする
                CTSettings.scanParam.fpd_gain = CTSettings.scansel.Data.fpd_gain;
                CTSettings.scanParam.fpd_integ = CTSettings.scansel.Data.fpd_integ;

                frmTransImage.Instance.CaptureOpen();

                //終了時はフラグをリセット
                //modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTScanStart);
                modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTScanStart);
                modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTMechaBusy);		//追加 by 間々田 2009/06/16

                ////mecainf更新のためのタイマーを再開
                ////v19.50 タイマーの統合 by長野 2013/12/17    //変更2014/10/07hata_v19.51反映
                ////frmMechaControl.Instance.tmrMecainf.Enabled = true;
                //modMechaControl.Flg_MechaControlUpdate = true;

                ////タッチパネル操作を許可（シーケンサ通信が可能な場合）
                //modSeqComm.SeqBitWrite("PanelInhibit", false);

                //外部トリガ取込みかつテーブル回転が連続回転の時             'V7.0 append by 間々田 2003/09/10
                if (CTSettings.scaninh.Data.ext_trig == 0 && modCT30K.TableRotOn) modSeqComm.SeqBitWrite("TrgReq", false);

                //画面情報を表示する。(※メニューバー更新の為)
                //    tmrStatus_Timer

                //オートセンタリングスキャンが終了したらオートセンタリングをなしにする
                MecaInf theMecainf = new MecaInf();
                theMecainf.Data.Initialize();
                //Rev21.00 成否に関わらずMecainfをロード by長野 2015/03/19
                theMecainf.Load();
                if (IsScanOK && (CTSettings.scansel.Data.auto_centering == 1))
                {
                    //Rev21.00 ｽｷｬﾝじゃないので不要 by長野 2015/3/22
                    //frmScanControl.Instance.chkInhibit[3].CheckState = CheckState.Checked;

                    //Rev21.00 スキャン成否に関わらず、Mecainfは取得する by長野 2015/03/19
                    ////追加2014/10/07hata_v19.51反映
                    ////mecainf（コモン）取得
                    ////GetMecainf(theMecainf);
                    //theMecainf.Load();

                    //焦点を記憶
                    theMecainf.Data.rc_focus = CTSettings.mecainf.Data.xfocus;  //v18.00変更 byやまおか 2011/06/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

                    //mecainf（コモン）更新
                    //PutMecainf(theMecainf);
                    theMecainf.Write();

                }

                //Rev21.00 
                //mecainf更新のためのタイマーを再開
                //v19.50 タイマーの統合 by長野 2013/12/17    //変更2014/10/07hata_v19.51反映
                //frmMechaControl.Instance.tmrMecainf.Enabled = true;
                modMechaControl.Flg_MechaControlUpdate = true;

                //Rev21.00 追加 by長野 2015/03/09
                if (IsScanOK)
                {
                    //成功した場合のみテーブルを元にいちに戻す by長野 2015/03/09
                    modMechaControl.MechaUdIndex(modScanCondition.bakScanoUdPos);
                }
                modCT30K.PauseForDoEvents(0.5f);

                //Rev21.00 成否に関わらずMecainfをロード by長野 2015/03/19
                theMecainf.Load();

                //Rev21.00 追加 by長野 2015/03/12
                frmMechaControl.Instance.UpdateMecha();
                //Rev21.00 変更 by長野 2015/03/19

                //frmMechaControl.Instance.cwnePos.Value = Convert.ToDecimal(Math.Round(theMecainf.Data.udab_pos, 3, MidpointRounding.AwayFromZero));
                //Rev23.10 計測CT対応 by長野 2015/10/16
                if (((Convert.ToDecimal(Math.Round(frmMechaControl.Instance.Udab_Pos, 3, MidpointRounding.AwayFromZero))) <= frmMechaControl.Instance.cwnePos.Maximum) &&
                    (frmMechaControl.Instance.cwnePos.Minimum <= (Convert.ToDecimal(Math.Round(frmMechaControl.Instance.Udab_Pos, 3, MidpointRounding.AwayFromZero)))))
                {
                    frmMechaControl.Instance.cwnePos.Value = Convert.ToDecimal(Math.Round(frmMechaControl.Instance.Udab_Pos, 3, MidpointRounding.AwayFromZero));
                }
                
                //Rev20.00 昇降位置と、昇降位置指定の値をそろえる by長野 2015/03/06
                //frmMechaControl.Instance.cwnePos_ValueChanged(null, EventArgs.Empty);

                //タッチパネル操作を許可（シーケンサ通信が可能な場合）
                modSeqComm.SeqBitWrite("PanelInhibit", false);


                //Rev20.00 追加 by長野 2015/02/09
                //正常終了時にも最終処理でストップ処理を入れて、正常終了後の透視画像の階調変換ができるようにする措置
                CTSettings.transImageControl.CaptureScanStop();

                //停止要求フラグをクリアする             'v17.50上記の処理を関数化 by 間々田 2011/02/17
                modCT30K.CallUserStopClear();

                //マウスカーソルを元に戻す
                Cursor.Current = Cursors.Default;

                //frmMechaControl.Instance.cwne txtUpDownPos.Text = cwnePos.Value.ToString("0.000");

                //v17.60/v18.00 上へ移動 byやまおか 2011/03/14
                //'CT30K側のMILを再度オープンする
                //frmTransImage.CaptureOpen
            }
        }
		//********************************************************************************
		//機    能  ：  スキャン動作前のチェック
		//              変数名           [I/O] 型        内容
		//引    数  ：
		//戻 り 値  ：                   [ /O] Boolean   結果(True:成功,False:失敗)
		//補    足  ：  スキャンスタートボタンの処理をサブルーチン化
		//
		//履    歴  ：  V3.0   00/08/02  (SI1)鈴山       新規作成
		//              V7.0   03/03/31  (SI4)間々田     不要なコード、コメントを整理
		//********************************************************************************
		private bool ExOperationChk()
		{
			//スキャンスタートできない場合、その理由を表示するように変更 'by 鈴山 '97-10-20
			//条件を満たさない場合、スキャンスタートを実行しない。

			//返り値を初期化
			bool functionReturnValue = false;

			//'連続コーン＋高速再構成のスキャン条件の時、RAMディスクが構築されているかどうか  'v17.40　追加 by 長野 2010/10/21
			//If (scansel.data_mode = 4) And scansel.table_rotation = 1 And (scaninh.gpgpu = 0) Then
			//
			//        If Not (smooth_rot_cone_flg = True) Then
			//
			//        'メッセージ表示
			//        '連続回転コーンビーム環境を構築中です。この処理には数分を要します。
			//        'しばらくしてから、処理を実行してください。連続回転コーンビームスキャン以外は実行可能です。
			//        MsgBox LoadResString(17506) & vbCr & LoadResString(17507), vbCritical
			//
			//        Exit Function
			//
			//        End If
			//
			//End If
			//RAMディスクが構築されているかどうか  'v17.40変更 byやまおか 2010/10/26
			//If UseRamDisk And (Not RamDiskIsReady) Then Exit Function
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//    If UseRamDisk Then      'v17.42修正 byやまおか 2010/11/04
			//        If (Not RamDiskIsReady) Then Exit Function
			//    End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//ハードディスク空き容量不足の場合
			double value = 0;
			double.TryParse(Convert.ToString(frmCTMenu.Instance.ScanAvairableNum), out value);
			if (!(value > 0))
			{
				modCT30K.ErrMessage(1301, Icon: MessageBoxIcon.Exclamation);
				return functionReturnValue;
			}

			//マルチスキャンモードごとのチェック
			switch (CTSettings.scansel.Data.multiscan_mode)
			{
				//マルチスキャン時のチェック
				case (int)ScanSel.MultiScanModeConstants.MultiScanModeMulti:

					if (!Multi_Check())
					{
						modCT30K.ErrMessage(1201, Icon: MessageBoxIcon.Exclamation);
						return functionReturnValue;
					}
					break;

				//スライスプラン時のチェック
				case (int)ScanSel.MultiScanModeConstants.MultiScanModeSlicePlan:

					//スライスプランテーブルが存在しない場合
					if (!File.Exists(modCT30K.GetSlicePlanTable(CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone)))
					{
						modCT30K.ErrMessage(1208, Icon: MessageBoxIcon.Exclamation);
						return functionReturnValue;
					}
					//スライスプランテーブルの許容範囲チェック
					else if (!SlicePlan_Check(CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone))
					{
						return functionReturnValue;
					}
					break;
    		}

			//v19.00 BHCテーブルが選択されている(BHC有)の場合は、テーブルが存在するかどうかをチェックする by長野 2012/05/02 '後で追加すること
			//    If scaninh.mbhc = 0 And scansel.mbhc_flag = 1 Then
			//
			//        'BHCテーブルが存在しない場合
			//        'If Not FSO.FileExists(scansel.mbhc_dir + scansel.mbhc_name) Then
			//        If Not FSO.FileExists(scansel.mbhc_dir + scansel.mbhc_name + ".csv") Then
			//            Call ErrMessage(1217, vbExclamation)
			//            Exit Function
			//        End If
			//
			//    End If

			//校正ステータスのチェック
			if (!Check_ScanStart()) return functionReturnValue;

			//校正ファイルの有無チェック
			if (!IsOkCorrectFile())
			{
				//エラーメッセージ表示：スキャン校正ファイルがすべて揃っていません。
				MessageBox.Show(CTResources.LoadResString(9408), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return functionReturnValue;
			}

			//v15.10下へ移動 byやまおか 2009/10/29
			//'ウォームアップが未完了の場合           'V6.0 append by 間々田 2002-08-30
			//If XrayControl.Up_Wrest_Mode > 0 Then
			//    Call ErrMessage(1134, vbExclamation)
			//    Exit Function
			//End If

			//Ｘ線が準備完了でない場合または異常の時はエラーとする   'v11.3追加 by 間々田 2006/02/17
			if (CTSettings.scaninh.Data.xray_remote == 0)
			{
				//ウォームアップが未完了の場合           'v15.10ここへ移動 byやまおか 2009/10/29
				if (modXrayControl.XrayControl.Up_Wrest_Mode > 0)
				{
					modCT30K.ErrMessage(1134, Icon: MessageBoxIcon.Exclamation);
					return functionReturnValue;
				}

				//Ｘ線がオフの場合
				if (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OffStatus)
				{
					//電磁ロックが開の場合、自動的に電磁ロックを閉とする '追加 by 間々田 2009/08/24
					if (frmCTMenu.Instance.DoorStatus == frmCTMenu.DoorStatusConstants.DoorClosed)
					{
						modSeqComm.SeqBitWrite("DoorLockOn", true);
						modCT30K.PauseForDoEvents(2);
					}

					//インターロックチェック                             '追加 by 間々田 2009/08/24
					if (!modXrayControl.IsXrayInterLock())
					{
						//メッセージ表示：
						//MsgBox "電磁ロックが開なので、スキャンスタートできません。", vbCritical
						//v15.03変更 リソース化&メッセージ変更　by やまおか 2009/11/19
						if (!modSeqComm.MySeq.stsDoorLock && CTSettings.scaninh.Data.door_lock == 0)
						{
							//電磁ロックが開のため、スキャンスタートできません。
							MessageBox.Show(StringTable.GetResString(StringTable.IDS_CannotDo, 
																	CTResources.LoadResString(StringTable.IDS_MagLock), 
																	CTResources.LoadResString(StringTable.IDS_OpenOnly), 
																	CTResources.LoadResString(StringTable.IDS_btnScanStart)), 
											Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						else
						{
							//扉が開のため、スキャンスタートできません。
							MessageBox.Show(StringTable.GetResString(StringTable.IDS_CannotDo, 
																	CTResources.LoadResString(StringTable.IDS_Door), 
																	CTResources.LoadResString(StringTable.IDS_OpenOnly), 
																	CTResources.LoadResString(StringTable.IDS_btnScanStart)), 
											Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						return functionReturnValue;
					}

					//If (Not (IsXrayReady And IsXrayInterLock And (XrayWarmUp = XrayWarmUpComplete))) Or IsXrayError Then
					//If frmXrayControl.lblXray.Caption <> GC_STS_STANDBY_OK Then 'v11.5変更 by 間々田 2006/07/10XrayStatus
					if (frmXrayControl.Instance.XrayStatus != StringTable.GC_STS_STANDBY_OK)	//v11.5変更 by 間々田 2006/07/10
					{
						modCT30K.ErrMessage(1127, Icon: MessageBoxIcon.Exclamation);
						return functionReturnValue;
					}
				}
			}

			//v17.65 スキャンスタート直前でメカを動かすとmeca_readyが立つ前にmecainfをチェックしてしまうため、ここでpioチェックとGetMecainfをする by(検S1)長野 2011/11/25
			modMechaControl.PioChkStart(20);
			//modMecainf.GetMecainf(ref CTSettings.mecainf.Data);
            CTSettings.mecainf.Load();

			//mecainfのチェック

			//メカが準備完了でない場合
			if (CTSettings.mecainf.Data.mecha_ready != 1)
			{
				modCT30K.ErrMessage(1126, Icon: MessageBoxIcon.Exclamation);
				return functionReturnValue;
			}

			//'v11.3削除ここから by 間々田 2006/02/17
			//''Ｘ線が準備完了でない場合または異常の時はエラーとする
			//'If (scaninh.xray_remote = 0) And (.xray_on = 0) And ((.x_ready = 0) Or (.xray_error = 1)) Then
			//'    Call ErrMessage(1127, vbExclamation)
			//'    Exit Function
			//'End If
			//'v11.3削除ここまで by 間々田 2006/02/17
            
            //Rev23.10 シフトスキャンのチェック by長野 2015/11/03
            //FDD可変の場合は、FDD1000のときのみシフトスキャン可とする。
            //Rev25.00 従来シフトは今までとおりFDD1000mmとする by長野 2016/07/05
            if (CTSettings.scaninh.Data.avmode != 0 && CTSettings.scansel.Data.scan_mode == 4)
            {
                //Rev25.00 追加 by長野 2016/08/21
                float ScanMechaFDD = 0.0f;
                ScanMechaFDD = CTSettings.scaninh.Data.cm_mode == 1 ? (float)modSeqComm.MySeq.stsFID : (float)modSeqComm.MySeq.stsLinearFDD; 

                //エラーメッセージ表示：シフトスキャンを行う場合、FDDを1000mmに設定してください。
                if (Math.Round((float)modSeqComm.MySeq.stsLinearFDD / (float)modMechaControl.GVal_FDD_SeqMagnify) != 1000)
                {
                    MessageBox.Show(CTResources.LoadResString(23014), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return functionReturnValue;
                }
            }
            //Rev25.00 SｽｷｬﾝON時、FDD700mm以下はｽｷｬﾝ不可とする(X線が蹴られるから)
            if (CTSettings.scansel.Data.w_scan == 1)
            {
                //Rev25.00 追加 by長野 2016/08/21
                float ScanMechaFDD = 0.0f;
                ScanMechaFDD = CTSettings.scaninh.Data.cm_mode == 1 ? (float)modSeqComm.MySeq.stsFID : (float)modSeqComm.MySeq.stsLinearFDD; 

                //エラーメッセージ表示：Wスキャンを行う場合、FDDを700mm以上に設定してください。
                if (Math.Round((float)ScanMechaFDD / (float)modMechaControl.GVal_FDD_SeqMagnify) < 750.0f)
                {
                    MessageBox.Show(CTResources.LoadResString(25007), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return functionReturnValue;
                }
            }

			//コーンビームスキャン時のチェック
			if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone)
			{
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
				//ヘリカルモード時
				//        If scansel.inh = 1 Then
				//
				//            'ヘリカルスキャンの昇降範囲チェック append by 山本  2000-10-24
				//            If Not HelicalUpDownCheck() Then
				//
				//                'エラーメッセージ表示：
				//                '   試料テーブルがヘリカルスキャン中に上限に達するため、スキャンを中止します。
				//                '   スキャン条件を再設定してください。
				//                MsgBox LoadResString(9508), vbExclamation
				//                Exit Function
				//
				//            End If
				//
				//        End If
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

				//コーンビームのスキャン条件チェック     'V4.0 append by 山本 START 2001-4-14
				if (!ConeBeamScanConditionCheck())
				{
					//エラーメッセージ表示：
					//   スキャン条件値が不適切です。
					//   スキャン条件を再設定してください。
					MessageBox.Show(CTResources.LoadResString(9397), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return functionReturnValue;
				}

				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
				//        'コーン分散処理ありの場合  added by 間々田　2003/12/18
				//        If scansel.cone_distribute = 1 Then
				//            If Not ConeDistributeOK() Then Exit Function
				//        End If
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
			}
			//ノーマルスキャン時のチェック
			else
			{
				//オートズームモード時のチェック
				if (CTSettings.scansel.Data.auto_zoomflag == 1)
				{
					//ズーミングテーブルが存在しない場合
					if (!File.Exists(modCT30K.GetZoomingTable()))
					{
						modCT30K.ErrMessage(1207, Icon: MessageBoxIcon.Exclamation);
						return functionReturnValue;
					}
				}
			}

			//回転中心や幾何歪校正を1度も実施していない場合のエラー対策　added by 山本　2002-12-4 START

			//回転中心画素
			//    構造体名：scancondpar
			//    コモン名：xlc[5]
			//If (GetCommonFloat("scancondpar", "xlc[2]") = 0) And (Not MyScansel.IsAutoCentering) Then

			//v10.02以下に修正 by 間々田 2005/06/01
			//オートセンタリングなしの場合
			if (CTSettings.scansel.Data.auto_centering == 0)
			{
				float rXlc = ((CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone) ? CTSettings.scancondpar.Data.nc : CTSettings.scancondpar.Data.xlc[2]);
				if (rXlc == 0)
				{
					//エラーメッセージ表示：
					//   回転中心校正が行われていないためスキャンを中止します
					//   回転中心校正を実行するかスキャン条件でオートセンタリングを「あり」に設定してからスキャンスタートしてください
					MessageBox.Show(CTResources.LoadResString(9465), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return functionReturnValue;
				}
			}
			//オートセンタリングありでフル２次元幾何歪補正時の場合
			else if (CTSettings.scaninh.Data.full_distortion == 0)
			{
				//スキャン位置校正ステータスをチェック
				if (!frmScanControl.Instance.IsOkScanPosition)
				{
					//メッセージ表示：
					//   スキャン位置校正が準備完了でないため、処理を中止します。
					//   事前にスキャン位置校正を実施してください。
					MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_CorNotReady, StringTable.IDS_CorScanPos), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return functionReturnValue;
				}
			}

			//検出器ﾋﾟｯﾁ(mm/画素)
			//    構造体名：scancondpar
			//    コモン名：mdtpitch[5]
			//If GetCommonFloat("scancondpar", "mdtpitch[2]") = 0 Then
			//If scancondpar.mdtpitch(2) = 0 Then 'v11.5変更 by 間々田 2006/04/24
            if ((CTSettings.scancondpar.Data.mdtpitch[2] == 0) && (!CTSettings.detectorParam.Use_FlatPanel))		//v17.00変更 byやまおか 2010/02/24
			{
				//エラーメッセージ表示：
				//   幾何歪校正が行われていないためスキャンを中止します
				//   幾何歪校正を実行してからスキャンスタートしてください
				MessageBox.Show(CTResources.LoadResString(9483), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return functionReturnValue;
			}

			//回転中心や幾何歪校正を1度も実施していない場合のエラー対策　added by 山本　2002-12-4 END

			//連続回転モードであれば最低ビュー数のチェックを行う     'v8.0 added by 間々田 2003/11/20
			if (modCT30K.TableRotOn)
			{
				int MinView = 0;

				//FRの取得
				//float theFR = 0;
				//Rev20.00 変更 by長野 2015/02/06
                double theFR = 0;

                //theFR = GetCommonFloat("scancondpar", "frame_rate[" & CStr(IIf(MyScansel.IsConeBeam, 1, 0)) & "][" & CStr(binning) & "]")
                //Rev20.00 条件追加 by長野 2015/02/06
                if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
                {
                    theFR = (double)1.0 / (modCT30K.fpd_integlist[frmScanControl.Instance.cmbInteg.SelectedIndex] / (double)1000.0);

                }
                else
                {
                    //theFR = CTSettings.scancondpar.Data.frame_rate[CTSettings.scansel.Data.binning][((CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone) ? 1 : 0)];		//v11.4変更 by 間々田 2006/03/13
                    int _mode = ((CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone) ? 1 : 0);
                    int _binnning = CTSettings.scansel.Data.binning;
                    theFR = CTSettings.scancondpar.Data.frame_rate[_binnning * 2 + _mode];
                }
                //2014/11/07hata キャストの修正
                //MinView = (int)Math.Truncate((theFR * 60 / modSeqComm.GetRotMax() - 1) / 100 + 1) * 100;
                MinView = Convert.ToInt32(Math.Floor((theFR * 60 / modSeqComm.GetRotMax() - 1) / 100F + 1)) * 100;

				//ﾋﾞｭｰ数設定値のチェック
				if (CTSettings.scansel.Data.scan_view < MinView)
				{
					//スキャン条件でビュー数をMinView以上に設定してからスキャンしてください。
					MessageBox.Show(StringTable.GetResString(9468, Convert.ToString(MinView)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return functionReturnValue;
				}
			}

			//I.I.（またはFPD）電源のチェック  added by 間々田 2003/11/06
			if (!modSeqComm.PowerSupplyOK()) return functionReturnValue;

			//FCD のチェック  'v9.7追加 by added 間々田 2004/12/10
			if (!modSeqComm.CheckFCD(ScanCorrect.GVal_Fcd))return functionReturnValue;

			functionReturnValue = true;
			return functionReturnValue;
		}
        //********************************************************************************
        //機    能  ：  スキャノ動作前のチェック
        //              変数名           [I/O] 型        内容
        //引    数  ：
        //戻 り 値  ：                   [ /O] Boolean   結果(True:成功,False:失敗)
        //補    足  ：  スキャンスタートボタンの処理をサブルーチン化
        //
        //履    歴  ：  V21.00 15/02/19  (検S1)長野      新規作成
        // 
        //********************************************************************************
        private bool ExScanoOperationChk()
        {
            //スキャンスタートできない場合、その理由を表示するように変更 'by 鈴山 '97-10-20
            //条件を満たさない場合、スキャンスタートを実行しない。

            //返り値を初期化
            bool functionReturnValue = false;

            ////ハードディスク空き容量不足の場合
            //double value = 0;
            //double.TryParse(Convert.ToString(frmCTMenu.Instance.ScanAvairableNum), out value);
            //if (!(value > 0))
            //{
            //    modCT30K.ErrMessage(1301, Icon: MessageBoxIcon.Exclamation);
            //    return functionReturnValue;
            //}

            //校正ステータスのチェック
            if (!Check_ScanoStart()) return functionReturnValue;

            if (!Check_ScanoUdpSpeed()) return functionReturnValue;

            //縦横比のチェック(20倍以上は警告を出す)
            double scano_ratio = 0.0;
            scano_ratio = (double)CTSettings.scansel.Data.mscanopt / 1024.0;
            if (scano_ratio > 20.0)
            {
                //確認メッセージ
                DialogResult result = MessageBox.Show(CTResources.LoadResString(22007), Application.ProductName,
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                functionReturnValue = (result == DialogResult.Yes);
                if (functionReturnValue == false)
                {
                    return (functionReturnValue);
                }
            }

            //校正ファイルの有無チェック
            if (!IsOkCorrectFile())
            {
                //エラーメッセージ表示：スキャン校正ファイルがすべて揃っていません。
                MessageBox.Show(CTResources.LoadResString(9408), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return functionReturnValue;
            }

            //Ｘ線が準備完了でない場合または異常の時はエラーとする   'v11.3追加 by 間々田 2006/02/17
            if (CTSettings.scaninh.Data.xray_remote == 0)
            {
                //ウォームアップが未完了の場合           'v15.10ここへ移動 byやまおか 2009/10/29
                if (modXrayControl.XrayControl.Up_Wrest_Mode > 0)
                {
                    modCT30K.ErrMessage(1134, Icon: MessageBoxIcon.Exclamation);
                    return functionReturnValue;
                }

                //Ｘ線がオフの場合
                if (frmXrayControl.Instance.MecaXrayOn == modCT30K.OnOffStatusConstants.OffStatus)
                {
                    //電磁ロックが開の場合、自動的に電磁ロックを閉とする '追加 by 間々田 2009/08/24
                    if (frmCTMenu.Instance.DoorStatus == frmCTMenu.DoorStatusConstants.DoorClosed)
                    {
                        modSeqComm.SeqBitWrite("DoorLockOn", true);
                        modCT30K.PauseForDoEvents(2);
                    }

                    //インターロックチェック                             '追加 by 間々田 2009/08/24
                    if (!modXrayControl.IsXrayInterLock())
                    {
                        //メッセージ表示：
                        //MsgBox "電磁ロックが開なので、スキャンスタートできません。", vbCritical
                        //v15.03変更 リソース化&メッセージ変更　by やまおか 2009/11/19
                        if (!modSeqComm.MySeq.stsDoorLock && CTSettings.scaninh.Data.door_lock == 0)
                        {
                            //電磁ロックが開のため、スキャンスタートできません。
                            MessageBox.Show(StringTable.GetResString(StringTable.IDS_CannotDo,
                                                                    CTResources.LoadResString(StringTable.IDS_MagLock),
                                                                    CTResources.LoadResString(StringTable.IDS_OpenOnly),
                                                                    CTResources.LoadResString(StringTable.IDS_btnScanStart)),
                                            Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            //扉が開のため、スキャンスタートできません。
                            MessageBox.Show(StringTable.GetResString(StringTable.IDS_CannotDo,
                                                                    CTResources.LoadResString(StringTable.IDS_Door),
                                                                    CTResources.LoadResString(StringTable.IDS_OpenOnly),
                                                                    CTResources.LoadResString(StringTable.IDS_btnScanStart)),
                                            Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return functionReturnValue;
                    }

                    //If (Not (IsXrayReady And IsXrayInterLock And (XrayWarmUp = XrayWarmUpComplete))) Or IsXrayError Then
                    //If frmXrayControl.lblXray.Caption <> GC_STS_STANDBY_OK Then 'v11.5変更 by 間々田 2006/07/10XrayStatus
                    if (frmXrayControl.Instance.XrayStatus != StringTable.GC_STS_STANDBY_OK)	//v11.5変更 by 間々田 2006/07/10
                    {
                        modCT30K.ErrMessage(1127, Icon: MessageBoxIcon.Exclamation);
                        return functionReturnValue;
                    }
                }
            }

            //v17.65 スキャンスタート直前でメカを動かすとmeca_readyが立つ前にmecainfをチェックしてしまうため、ここでpioチェックとGetMecainfをする by(検S1)長野 2011/11/25
            modMechaControl.PioChkStart(20);
            //modMecainf.GetMecainf(ref CTSettings.mecainf.Data);
            CTSettings.mecainf.Load();

            //mecainfのチェック

            ////メカが準備完了でない場合
            //if (CTSettings.mecainf.Data.mecha_ready != 1)
            //{
            //    modCT30K.ErrMessage(1126, Icon: MessageBoxIcon.Exclamation);
            //    return functionReturnValue;
            //}

            //I.I.（またはFPD）電源のチェック  added by 間々田 2003/11/06
            if (!modSeqComm.PowerSupplyOK()) return functionReturnValue;

            //FCD のチェック  'v9.7追加 by added 間々田 2004/12/10
            if (!modSeqComm.CheckFCD(ScanCorrect.GVal_Fcd)) return functionReturnValue;

            functionReturnValue = true;
            return functionReturnValue;
        }
		//********************************************************************************
		//機    能  ：  スキャンスタート
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：                   [ /O] Boolean   結果(True:成功, False:失敗)
		//補    足  ：  スキャンスタートボタンの処理をサブルーチン化
		//
		//履    歴  ：  V3.0   00/08/02  (SI1)鈴山       新規作成
		//             v13.0   07/03/19  (WEB)間々田     ダブルオブリーク対応
		//             v17.48/v17.53  11/03/22  (検S1)やまおか  強制終了の解析のためUserLogを追加
		//********************************************************************************
		public bool ExScanStart()
		{
			//int error_sts = 0;				//戻り値
			//int ProcessID = 0;				//ShellコマンドのプロセスID格納用
			string ExeFileName = null;		//実行ﾌｧｲﾙ名

            //Stopwatchスタート　Rev26.00 by井上
            //sw.Reset();
            //sw.Start();

//スキャンスタートで落ちる現象解析のため
//出荷用には不要なので消してもよい
//ログファイル行先頭記号「;」区切記号「,」行末尾記号はなし
//ログファイル中に「,;」があればスキャンスタート途中で終了したことになる
#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/24
			modCT30K.UserLogDel(524288);		//512kB(0.5MB)
			modCT30K.UserLogOut(";" + DateTime.Now + ",");
#endif
			//戻り値初期化
			bool functionReturnValue = false;

			//ダブルオブリークで画像用にメモリが読み込まれたままの場合はスキャンを中止　'v13.0追加 by 間々田 2007/03/19
			if (CTSettings.scaninh.Data.double_oblique == 0)
			{
                if (modDoubleOblique.IsDoubleObliqueBusy(CTResources.LoadResString(StringTable.IDS_Scan)))
				{
#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/24
					modCT30K.UserLogOut("\r\n");
#endif
					return functionReturnValue;
				}
			}

#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/21
			modCT30K.UserLogOut("1,");
#endif

			//運転準備ボタンが押されていなければ無効 'v17.48/v17.53追加 byやまおか 2011/03/24
			if (!modSeqComm.MySeq.stsRunReadySW)
			{
				//MsgBox "運転準備スイッチを押してください。", vbCritical
				MessageBox.Show(CTResources.LoadResString(8059), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);	//v17.60 ストリングテーブル化 by長野 2011/05/25
#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/24
				modCT30K.UserLogOut("\r\n");
#endif
				return functionReturnValue;
			}

            //Rev26.00 add by chouno 2017/03/13
            if (modMechaControl.IsOkMechaMoveWithLargeTable() == false)
            {
                return functionReturnValue;
            }

			//CTソフトとタッチパネル操作をロックをテーブル回転リセット前に追加　by  v17.61 by 長野 2011/09/12
			LockCTsoftAndPanel();

            //Rev26.00 スキャン前に未完な校正は、ここで行う。
            if (frmScanControl.Instance.autoCorBeforeScanFlg == true && !frmScanControl.Instance.IsOkAll)
            {
                if (!ScanCorrect.autoCorBeforeScanStart())
                {
                    UnLockCTsoftAndPanel();
                    return functionReturnValue;
                }
            }

			//    'テーブル回転角度が０でない場合、０にしてからスタート   'v15.0追加 by 間々田 2009/07/21
			//    If Not frmMechaControl.ctchkRotate(0).Value Then
			//        'frmMessage.lblMessage.Caption = "テーブル回転角度を０度にリセットしています..."
			//        'ShowMessage "テーブル回転角度を０度にリセットしています..."                    '変更 by 間々田 2009/08/24
			//         ShowMessage LoadResString(20092) 'v14.99ストリングテーブル化 by長野 2011/05/25
			//        'error_sts = MecaRotateIndex(0)
			//        '検出器によって回転方向を変える 'v17.02変更 byやまおか 2010/07/21
			//        'FPDの場合はCWで0に戻る
			//        If Use_FlatPanel Then
			//            error_sts = RotateInit(hDevID1)     '回転軸初期化
			//            error_sts = MecaRotateOrigin(True)  '回転軸原点復帰
			//        'I.I.の場合はCCWで0に戻る
			//        Else
			//            error_sts = MecaRotateIndex(0)
			//        End If
			//
			//        Dim WaitTime As Long
			//        'WaitTime = 20 * 1000
			//        WaitTime = 30 * 1000    'v16.20変更 微妙に足りないときがあるため20s→30s byやまおか 2010/04/21
			//        Do While Not frmMechaControl.ctchkRotate(0).Value
			//            If TimeOut(WaitTime) Then
			//                'Unload frmMessage
			//                HideMessage    '変更 by 間々田 2009/08/24
			//                'MsgBox "テーブル回転角度を原点の位置に戻すことができませんでした。", vbExclamation
			//                MsgBox LoadResString(20093), vbExclamation 'v17.60 by長野 2011/05/25
			//#If UserLog Then    'v17.48/v17.53追加 byやまおか 2011/03/24
			//    UserLogOut vbCrLf
			//#End If
			//               'CTソフトとタッチパネル操作をアンロック　by  v17.61 by 長野 2011/09/12
			//                UnLockCTsoftAndPanel
			//
			//                Exit Function
			//            End If
			//        Loop
			//        'Unload frmMessage
			//        HideMessage    '変更 by 間々田 2009/08/24
			//
			//    End If
			//上記処理を関数化   'v17.61/v18.00変更 byやまおか 2011/07/23
			//If (Not Check_RotateZero) Then Exit Function
			//テーブル原点に戻せなかった時に処理を抜ける際、CTソフトとタッチパネルをアンロックしてから抜けるように変更 by長野 'v19.02 2012/07/17
            //Rev21.00 スキャノの場合は戻さない by長野 2015/03/09
            if (CTSettings.scansel.Data.data_mode != (int)ScanSel.DataModeConstants.DataModeScano)
            {
                if (!Check_RotateZero())
                {
                    UnLockCTsoftAndPanel();
                    return functionReturnValue;
                }
            }

#if UserLog     //'v17.48/v17.53追加 byやまおか 2011/03/21 'v19.50 追加 by長野 2013/11/05
            modCT30K.UserLogOut("1-2,");
#endif

            //検出器が基準位置にいない場合は基準位置に移動する   'v18.00追加 byやまおか 2011/02/27
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //if (CTSettings.DetShiftOn)
            if (CTSettings.DetShiftOn || CTSettings.W_ScanOn)
            {
                if ((modDetShift.DetShift != modDetShift.DetShiftConstants.DetShift_origin))
                {
 				    //If (Not ShiftDet(DetShift_origin)) Then
				    //ゲインをセットしない   'v18.00変更 byやまおか 2011/07/04
                    if ((!(modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_origin, modDetShift.UNSET_GAIN))))
                    {
					    //                MsgBox "検出器シフトに失敗しました。", vbExclamation
					    //Interaction.MsgBox(CT30K.My.Resources.str21314, MsgBoxStyle.Exclamation);
                        MessageBox.Show(CTResources.LoadResString(21314), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    //if (CTSettings.scaninh.Data.lr_sft == 0)
                    //Rev25.00 Wスキャンを追加 by長野 2016/07/05
                    if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
                    {
                        if ((!(modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_forward, modDetShift.UNSET_GAIN))))
                        {
                            //MsgBox "検出器シフトに失敗しました。", vbExclamation
                            //Interaction.MsgBox(CT30K.My.Resources.str21314, MsgBoxStyle.Exclamation);
                            MessageBox.Show(CTResources.LoadResString(21314), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        //Rev23.20 左右シフトONの場合は、右→基準の順に動かしてバックラッシュによる誤差をできる限り同じにする。 by長野 2015/12/28
                        if ((!(modDetShift.ShiftDet(modDetShift.DetShiftConstants.DetShift_origin,modDetShift.UNSET_GAIN))))
                        {
                            //                MsgBox "検出器シフトに失敗しました。", vbExclamation
                            //Interaction.MsgBox(CT30K.My.Resources.str21314, MsgBoxStyle.Exclamation);
                            MessageBox.Show(CTResources.LoadResString(21314), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
			}

#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/2
			modCT30K.UserLogOut("2,");
#endif

            //スキャン動作前のチェック
            //if ((CTSettings.scansel.Data.data_mode != (int)ScanSel.DataModeConstants.DataModeCone) || (Maint.ConeAdjustFlg == 0))
            //Rev21.00 変更 by長野 2015/03/09
            if (((CTSettings.scansel.Data.data_mode != (int)ScanSel.DataModeConstants.DataModeCone) || (Maint.ConeAdjustFlg == 0)) && (CTSettings.scansel.Data.data_mode != (int)ScanSel.DataModeConstants.DataModeScano))
            {
                //If Not ExOperationChk() Then Exit Function
                if (!ExOperationChk())
                {
#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/24
					modCT30K.UserLogOut("\r\n");
#endif
                    //CTソフトとタッチパネル操作をアンロック　by  v17.61 by 長野 2011/09/12
                    UnLockCTsoftAndPanel();

                    return functionReturnValue;
                }
            }
            else if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScano)　//Rev21.00 追加 by長野 2015/03/08
            {
                //If Not ExOperationChk() Then Exit Function
                if (!ExScanoOperationChk())
                {
#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/24
					modCT30K.UserLogOut("\r\n");
#endif
                    //CTソフトとタッチパネル操作をアンロック　by  v17.61 by 長野 2011/09/12
                    UnLockCTsoftAndPanel();

                    return functionReturnValue;
                }

            }
#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/21
			modCT30K.UserLogOut("3,");
#endif

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//コーン分散処理が有効の時は画像保存ドライブをチェック  2003.12.15 松井
			//    If ((scaninh.cone_distribute = 0) Or (scaninh.cone_distribute2 = 0)) And (scansel.data_mode = DataModeCone) And (scansel.cone_distribute = 1) Then
			//
			//        Dim driveName As String
			//        driveName = RemoveNull(scancondpar.distribute_drive)
			//        If UCase$(Left$(driveName, 1)) <> UCase$(Left$(scansel.pro_code, 1)) Then
			//            'メッセージ表示：コーン分散処理で有効を選択したら、画像保存先にはドライブ %1 を指定してください。
			//            MsgBox GetResString(12911, driveName), vbCritical
			//#If UserLog Then    'v17.48/v17.53追加 byやまおか 2011/03/24
			//    UserLogOut vbCrLf
			//#End If
			//        'CTソフトとタッチパネル操作をアンロック　by  v17.61 by 長野 2011/09/12
			//         UnLockCTsoftAndPanel
			//
			//            Exit Function
			//        End If
			//
			//    End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//高速再構成が有効＆連続回転選択時は、通常のconebeam.exeに切り替える v16.2 追加 by 山影 10-04-01
			//CONEBEAM = IIf((scaninh.gpgpu = 0) And TableRotOn, CONEBEAM_NORMAL, CONEBEAM_GPGPU)    'これだと.gpgpu=1のときにGPGPUを選んでしまう
			//    If (scaninh.gpgpu = 0) Then     'v16.20修正 byやまおか 2010/05/13
			//        CONEBEAM = IIf(TableRotOn, CONEBEAM_NORMAL, CONEBEAM_GPGPU)
			//    End If
			//テーブル連続回転で、RAMディスクが構築されてない場合は通常コーンビーム 'v17.40 条件式を変更 by 長野 2010/10/21
			//If (TableRotOn = True) And (smooth_rot_cone_flg = False) Then
			//    CONEBEAM = CONEBEAM_NORMAL
			//End If

            //AppValue内でやっているのでここkでは不要　2014/03/05(検S1)hata
            //コーンビーム実行ファイルの決定  scaninh.gpgpuだけを見る   'v17.45/17.50上の処理を修正 2011/02/22 by 間々田
            //AppValue.CONEBEAM = ((CTSettings.scaninh.Data.gpgpu == 0) ? AppValue.CONEBEAM_GPGPU : AppValue.CONEBEAM_NORMAL);

			//実行ファイル名の決定
            //ExeFileName = ((CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone) ? AppValue.CONEBEAM : AppValue.SCANAV);
            //実行ファイル名の決定
            //ExeFileName = ((CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone) ? AppValue.CONEBEAM : AppValue.SCANAV);
            //Rev21.00 変更 by長野 2015/02/19
            if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone)
            {
                ExeFileName = AppValue.CONEBEAM;
            }
            else if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScan)
            {
                ExeFileName = AppValue.SCANAV;
            }
            else if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScano)
            {
                ExeFileName = AppValue.SCANOAV;
            }

			//実行ファイルがなければ中止
			if (!File.Exists(ExeFileName))
			{
				//メッセージ表示：～が見つかりません。スキャンスタートを中止します。
				MessageBox.Show(StringTable.GetResString(StringTable.IDS_NotFound, ExeFileName) + "\r\n" + CTResources.LoadResString(9586), 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/24
				modCT30K.UserLogOut("\r\n");
#endif

				//CTソフトとタッチパネル操作をアンロックを追加　by  v17.61 by 長野 2011/09/12
				UnLockCTsoftAndPanel();

				return functionReturnValue;
			}
            //Rev20.00 ここに移動 by長野 2014/12/15 by長野
            //フラグをセット
            //if (ExeFileName == AppValue.SCANAV || ExeFileName == AppValue.CONEBEAM)
            //{
            //    modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTScanStart;
            //}
            //else
            //{
            //    modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTReconStart;
            //}

#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/21
			modCT30K.UserLogOut("4,");
#endif

			//'コーンビーム時
			//'If scansel.data_mode = DataModeCone Then   '念のためスキャン時も以下の処理を行う 'v15.0削除 by 間々田 2009/06/16
			//
			//    'Load frmConeScanCondition           'added by 山本　2002-12-14　テーブルの高さを変えた場合、再構成範囲を変えないとエラーするため
			//    'frmConeScanCondition.cmdOk_Click    'added by 山本　2002-12-14　テーブルの高さを変えた場合、再構成範囲を変えないとエラーするため
			//
			//    'v15.0変更 by 間々田 2009/01/18 コーンビームスキャン条件画面とスキャン条件画面を統合したので
			//    Load frmScanCondition           'added by 山本　2002-12-14　テーブルの高さを変えた場合、再構成範囲を変えないとエラーするため
			//    frmScanCondition.cmdOk_Click    'added by 山本　2002-12-14　テーブルの高さを変えた場合、再構成範囲を変えないとエラーするため
			//
			//'End If

                        //Rev21.00 条件式追加 by長野 2015/02/19
            if (CTSettings.scansel.Data.data_mode != (int)ScanSel.DataModeConstants.DataModeScano)
            {
                //スキャン条件設定   v15.0変更 by 間々田 2009/06/17
                //変更2014/07/04(検S1)hata　
                //ScanOptValueChk2okが読めないため、formを閉じないようする。後で閉じる。
                //frmScanCondition.Instance.Setup();
                frmScanCondition.Instance.Setup(NoClose: true);

                //v17.65 回転中心が左右によっているメッセージが出た場合、ここでのスキャン中止をできるようにした。by(検S1)長野 2011/11/26
                //その場合は、スキャンを中止するように変更 by長野 2011/11/24
                if (!frmScanCondition.Instance.ScanOptValueChk2ok)
                {
                    //if (modLibrary.IsExistForm("frmScanCondition")) 
                    frmScanCondition.Instance.Close();      //追加2014/07/04(検S1)hata

                    //CTソフトとタッチパネル操作をアンロック 'v17.61 by長野 2011/9/12
                    UnLockCTsoftAndPanel();
                    return functionReturnValue;
                }
                //if (modLibrary.IsExistForm(frmScanCondition.Instance)) 
                frmScanCondition.Instance.Close();  //追加2014/07/04(検S1)hata
            }
            else
            {
                //スキャノ条件設定   v15.0変更 by 間々田 2009/06/17
                frmScanoCondition.Instance.Setup(NoClose: true);

                frmScanoCondition.Instance.Close();  //追加2014/07/04(検S1)hata

                //Rev26.40 修正 by chouno 2019/03/25
                if (!Check_ScanoUdpSpeed())
                {
                    UnLockCTsoftAndPanel();

                    return functionReturnValue;
                }

                //Rev21.00 スキャノ終了後に元に位置に戻すため、現在位置をバックアップ by長野 2015/03/09
                modScanCondition.bakScanoUdPos = (float)frmMechaControl.Instance.ntbUpDown.Value;
            }

#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/21
			modCT30K.UserLogOut("5,");
#endif

			//実行時はフラグをセット
			//Call StatusFlag_Set(ScanStartFlag, True)   'v11.5削除 by 間々田 2006/07/03

			//タイマーの中止
			//frmStatus.tmrStatus.Enabled = False

			//スキャン状態のコモンを更新する by 山本 99-7-31
			//    構造体名：scansel
			//    コモン名：c_scan
			//    コモン値：0(スキャン中でない),1(スキャン中)
			//Call putcommon_long("scansel", "c_scan", 1)    'v11.5削除 by 間々田 2006/07/04
			//    Call tmrStatus_Timer

			//タッチパネル操作を禁止（シーケンサ通信が可能な場合）  V4.0 append by 鈴山 2001/01/23
			//テーブル回転リセット中にタッチパネルが操作できるため、回転リセットの手前に移動 v17.61 by 長野 2011/09/12
			//CTソフトとタッチパネル操作のロックをまとめて関数化 v17.61 by 長野 2011/09/12
			//SeqBitWrite "PanelInhibit", True      'V4.0 change by 鈴山 2001/02/20 (False → True)

			//v11.5削除 by 間々田 2006/07/10 シーケンサ通信を停止させない
			//'PCフリーズ対策時はシーケンサ通信を停止させるため少し待つ added by 山本　2004-8-4
			//If scaninh.pc_freeze = 0 Then
			//    PauseForDoEvents 0.5
			//End If

			//v19.00 Pke製FPDでの場合、ゲイン校正後のエアデータからPを算出しておく by長野 2012/04/10
			if (CTSettings.scancondpar.Data.detector == 2)
			{
				if (GetAirDataPValue() == false)
				{
					//CTソフトとタッチパネル操作をアンロック 'v17.61 by長野 2011/9/12
					UnLockCTsoftAndPanel();
					return functionReturnValue;
				}
			}

            //オートセンタリングありの場合
            //if ((CTSettings.scaninh.Data.auto_centering == 0) && (CTSettings.scansel.Data.auto_centering == 1))
            //Rev21.00 条件式変更 by長野 2015/03/08
            if ((CTSettings.scaninh.Data.auto_centering == 0) && (CTSettings.scansel.Data.auto_centering == 1) && (CTSettings.scansel.Data.data_mode != (int)ScanSel.DataModeConstants.DataModeScano))
            {
                //FCD+FCDオフセットが0以下になっていた場合、処理を中止する   'v9.7追加 by 間々田 2004/02/16
				if (frmMechaControl.Instance.FCDWithOffset <= 0)
				{
					//メッセージ表示：
					//   FCD値が負の数になっているので、試料テーブルＹ軸のFCD値の誤差が大きい可能性があります。
					//   Ｙ軸の原点復帰とコモン初期化を行ってから再度実行してください。
					//MsgBox LoadResString(9330), vbCritical

					//v14.24変更 by 間々田 2009/03/10 メカ軸名称はコモンを使用する
					//   リソース9330の中身も以下に変更：
					//   FCD値が負の数になっているので、試料テーブル%1軸のFCD値の誤差が大きい可能性があります。
					//   %1軸の原点復帰とコモン初期化を行ってから再度実行してください。
					MessageBox.Show(StringTable.GetResString(9330, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[0].GetString())), 
									Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

					//タッチパネル操作を禁止解除
					//CTソフトとタッチパネル操作の解除を関数化した v17.61 by 長野 2011/09/12
					//SeqBitWrite "PanelInhibit", False

					//CTソフトとタッチパネル操作をアンロックを追加　by  v17.61 by 長野 2011/09/12
					UnLockCTsoftAndPanel();

#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/24
					modCT30K.UserLogOut("\r\n");
#endif

					return functionReturnValue;
				}

				//FID/FCDをコモンに書き込む
				CTSettings.scansel.Data.fid = frmMechaControl.Instance.FIDWithOffset;	//FID
				CTSettings.scansel.Data.fcd = frmMechaControl.Instance.FCDWithOffset;	//FCD
				//PutScansel scansel         'v11.5削除 by 間々田 2006/07/10

				//現在の昇降位置を記憶する（スキャン終了後に mecainf.rc_udab_pos に書き込むため） v11.5追加 by 間々田 2006/09/21
				//UdPosAtScanStart = CTSettings.mecainf.Data.udab_pos;
                //Rev23.10 計測CT対応 by長野 2015/10/16
                UdPosAtScanStart = frmMechaControl.Instance.Udab_Pos;
            }

#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/21
			modCT30K.UserLogOut("6,");
#endif

			//テーブル回転リセット中にCTソフトが操作できるため、回転リセットの手前に移動 v17.61 by 長野 2011/09/12
			//CTソフトとタッチパネル操作の禁止をまとめて関数化 v17.61 by 長野 2011/09/12
			//CTBusy = CTBusy Or CTMechaBusy '追加 by 間々田 2009/06/16

			//ライブ画像処理停止
			frmTransImage.Instance.CaptureOn = false;
			Application.DoEvents();						//v15.0追加 by 間々田 2009/04/22

#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/21
			modCT30K.UserLogOut("7,");
#endif

            #region // クローズしない。
            ////CT30K側のMILをクローズする
            //frmTransImage.Instance.TransImageCtrl.CaptureClose();
            //Application.DoEvents();						//v15.0追加 by 間々田 2009/04/22
            #endregion


#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/21
			modCT30K.UserLogOut("8,");
#endif
            #region // クローズしない。
            ////メカクローズ
            //modMechaControl.MechaClose();
            #endregion


#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/21
			modCT30K.UserLogOut("9,");
#endif

            //現在のｘ座標を読み取り、コモンに書き込む
			//    構造体名：scancondpar
			//    コモン名：xposition
			//   コモン値：UC_Seqcom.stsXPosition/10 (mm)       '旧バージョン6.3用
			//    コモン値：UC_Seqcom.stsXPosition/100 (mm)      '変更 巻渕 2003-03-03   小数点以下2桁まで対応
			//Call putcommon_float("scancondpar", "xposition", CSng(Myseq.stsXPosition / 100))
            //2014/11/07hata キャストの修正
            //CTSettings.scancondpar.Data.xposition = modSeqComm.MySeq.stsXPosition / 100;		//v11.5変更 by 間々田 2006/04/26
            //CTSettings.scancondpar.Data.xposition = modSeqComm.MySeq.stsXPosition / 100F;		//v11.5変更 by 間々田 2006/04/26
            CTSettings.scancondpar.Data.xposition = modSeqComm.MySeq.stsXPosition / (float)modMechaControl.GVal_TableX_SeqMagnify;//Rev23.10 変更 by長野 2015/09/18

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//外部トリガ取込みかつテーブル回転が連続回転の時             'V7.0 append by 間々田 2003/09/10
			//    If (scaninh.ext_trig = 0) And TableRotOn Then
			//
			//        '外部トリガＯＮ処理
			//        ExtTrigOn IIf(scansel.data_mode = DataModeCone, 1, 0)
			//
			//        'データ収集開始角度のコモンへの書込み
			//
			//        'ExtTrigOnで求めたStartAngleをコモンのscancondpar.scan_start_angleに書き込む        change by 間々田 2004/05/20
			//        'Call putcommon_float("scancondpar", "scan_start_angle", StartAngle)
			//        scancondpar.scan_start_angle = StartAngle    'v11.5変更 by 間々田 2006/04/26
			//
			//    End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //Rev20.00 64bit化により廃止 by長野 2014/11/04
            ////追加2014/10/07hata_v19.51反映
            ////v19.51 テーブル回転が連続回転の時、メモリを節約するため枚数を減らす by長野 2014/03/04 by長野
            //if ((modCT30K.TableRotOn))
            //{
            //    bak_dev_klimit = CTSettings.scancondpar.Data.dev_klimit;
            //    CTSettings.scancondpar.Data.dev_klimit = CTSettings.scancondpar.Data.dev_klimit / 2;
            //}

			//modScancondpar.CallPutScancondpar();		//v11.5追加 by 間々田 2006/04/26
            CTSettings.scancondpar.Write();

#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/21
			modCT30K.UserLogOut("10,");
#endif

			//現在のI.I.視野をコモンに書き込む
			//temp_rc_iifield = GetIINo()              'added by 山本　2004-6-16　オートセンタリングスキャン完了後にコモン書き込み用に変数に記憶しておく
			//Call putcommon_long("mecainf", "iifield", temp_rc_iifield)

			//v11.5変更 by 間々田 2006-07-10 スキャン時もシーケンサ通信を停止させないようにしたので
			//Call putcommon_long("mecainf", "iifield", GetIINo())

			//v15.0変更 by 間々田 2009/07/16
			//modMecainf.GetMecainf(ref CTSettings.mecainf.Data);
            CTSettings.mecainf.Load();

			CTSettings.mecainf.Data.iifield = modSeqComm.GetIINo();										//I.I.視野
			CTSettings.mecainf.Data.table_x_pos = (float)frmMechaControl.Instance.ntbTableXPos.Value;	//試料テーブル（光軸と垂直方向)座標[mm] （従来のＸ軸座標）も書き込む

            //Rev23.10 追加 by長野 2015/10/29
            CTSettings.mecainf.Data.scan_table_x_posMecha = modSeqComm.MySeq.stsXPosition / modMechaControl.GVal_TableX_SeqMagnify;
            CTSettings.mecainf.Data.scan_table_x_posLinear = modSeqComm.MySeq.stsLinearTableY / modMechaControl.GVal_TableX_SeqMagnify;
            CTSettings.mecainf.Data.scan_fcdMecha = frmMechaControl.Instance.FCDWithOffsetMecha;
            CTSettings.mecainf.Data.scan_fcdLinear = frmMechaControl.Instance.FCDWithOffsetLinear;
            CTSettings.mecainf.Data.scan_fddMecha = frmMechaControl.Instance.FIDWithOffsetMecha;
            CTSettings.mecainf.Data.scan_fddLinear = frmMechaControl.Instance.FIDWithOffsetLinear;
            CTSettings.mecainf.Data.scan_ud_pos = CTSettings.mecainf.Data.udab_pos;
            CTSettings.mecainf.Data.scan_ud_posLinear = CTSettings.mecainf.Data.ud_linear_pos;
            //焦点切替えフレーム
            switch (modXrayControl.XrayType)
            {
                //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421
                //Case XrayTypeKevex, XrayTypeHamaL9181, XrayTypeHamaL9191, XrayTypeHamaL9421, XrayTypeHamaL9421_02T  'v16.30 02T追加 byやまおか 2010/05/21
                case modXrayControl.XrayTypeConstants.XrayTypeKevex:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:     //v17.71 追加 by長野 2012/03/14
                case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:          //'v17.71 追加 by長野 2012/03/14 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07   //追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02:     //追加2014/10/07hata_v19.51反映
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711:       //Rev23.10 追加 by長野 2015/10/01

                    CTSettings.mecainf.Data.xfocus = modXrayControl.XrayControl.Up_Focussize - 1;
                    break;
            }

            //追加2014/10/07hata_v19.51反映
            //v19.51 回転大テーブル装着状態かどうかをmecainfに書き込む by長野 2014/03/04
            //回転テーブルが装着されているかどうか 'v19.51 追加 by長野 2014/03/03
            CTSettings.mecainf.Data.largeRotTable = modSeqComm.GetLargeRotTableSts();
            
            //modMecainf.PutMecainf(ref CTSettings.mecainf.Data);
            CTSettings.mecainf.Write();


#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/21
			modCT30K.UserLogOut("11,");
#endif

			//v11.5削除ここから by 間々田 2006/07/03
			//'コーン分散処理の場合、分散処理ステータスフォームを下げる
			//If IsExistForm(frmDistributeStatus) Then
			//    'frmDistributeStatus.Top = Me.Top + 9500
			//    frmDistributeStatus.Top = Me.Top + Me.Height + 1050
			//End If
			//v11.5削除ここまで by 間々田 2006/07/03

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//コモンの回転選択が可能でかつscansel.rotate_selectがX線管回転の場合は、
			//シーケンサ通信で取得したX線管回転軸の座標からスキャン時の回転方向を判定し、コモンに書込む  'v9.0 change by 間々田 2004/02/16
			//    If (scansel.rotate_select = 1) And (scaninh.rotate_select = 0) And (scaninh.SeqComm = 0) Then
			//        'Call putcommon_long("scansel", "c_cw_ccw", GetRotateDirection())
			//        scansel.c_cw_ccw = GetRotateDirection() 'v11.5変更 by 間々田 2006/07/10
			//    End If
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//動作モード
			//    構造体名：scansel
			//    コモン名：operation_mode
			//    コモン値：1:SCAN 2:SCANO 3:RECON 4:ZOOM 5:GAIN 6:CONEBEAM 7:CONERECON
            //CTSettings.scansel.Data.operation_mode = ((CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone) ? (int)ScanSel.OperationModeConstants.OP_CONEBEAM : (int)ScanSel.OperationModeConstants.OP_SCAN);		//v11.5変更 by 間々田 2006/07/10
            //シフトスキャンに対応   'v18.00変更 byやまおか 2011/06/29      //変更2014/10/07hata_v19.51反映
            //シフトスキャンに対応   'v18.00変更 byやまおか 2011/06/29      //変更2014/10/07hata_v19.51反映
            switch (CTSettings.scansel.Data.data_mode)
            {
                case (int)ScanSel.DataModeConstants.DataModeCone:
                    
                    //Rev25.00 Wスキャン対応 by長野 2016/07/07
                    //CTSettings.scansel.Data.operation_mode = (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift ? (int)ScanSel.OperationModeConstants.OP_SHIFT_CONE : (int)ScanSel.OperationModeConstants.OP_CONEBEAM);
                    CTSettings.scansel.Data.operation_mode = ((CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift || CTSettings.scansel.Data.w_scan == 1) ? (int)ScanSel.OperationModeConstants.OP_SHIFT_CONE : (int)ScanSel.OperationModeConstants.OP_CONEBEAM);

                    break;
                //Rev21.00 追加 by長野 2015/02/19
                case (int)ScanSel.DataModeConstants.DataModeScan:
                    
                    //Rev25.00 Wスキャン対応 by長野 2016/07/07
                    //CTSettings.scansel.Data.operation_mode = (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift ? (int)ScanSel.OperationModeConstants.OP_SHIFT_SCAN : (int)ScanSel.OperationModeConstants.OP_SCAN);
                    CTSettings.scansel.Data.operation_mode = ((CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift || CTSettings.scansel.Data.w_scan == 1) ? (int)ScanSel.OperationModeConstants.OP_SHIFT_SCAN : (int)ScanSel.OperationModeConstants.OP_SCAN);
                    break;
                //Rev21.00 追加 by長野 2015/02/19
                case (int)ScanSel.DataModeConstants.DataModeScano:
                    CTSettings.scansel.Data.operation_mode = (int)ScanSel.OperationModeConstants.OP_SCANO;
                    break;
                default:
                    
                    //Rev25.00 Wスキャン対応 by長野 2016/07/07
                    //CTSettings.scansel.Data.operation_mode = (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift ? (int)ScanSel.OperationModeConstants.OP_SHIFT_SCAN : (int)ScanSel.OperationModeConstants.OP_SCAN);
                    CTSettings.scansel.Data.operation_mode = ((CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift || CTSettings.scansel.Data.w_scan == 1)? (int)ScanSel.OperationModeConstants.OP_SHIFT_SCAN : (int)ScanSel.OperationModeConstants.OP_SCAN);
                    break;
            }

			//管電圧/管電流      '追加 by 間々田 2009/08/06  'v15.10削除 byやまおか 2009/11/26
			//scansel.scan_kv = frmXrayControl.cwneKV.Value    '管電圧
			//scansel.scan_ma = frmXrayControl.cwneMA.Value    '管電流

			//v15.10追加 byやまおか 2009/11/26
			//Ｘ線制御なし＆メカ制御ありの場合、スキャンコントロール欄の値をscanselにセット
			if ((CTSettings.scaninh.Data.xray_remote == 1) && (CTSettings.scaninh.Data.mechacontrol == 0))
			{
				CTSettings.scansel.Data.scan_kv = (float)frmScanControl.Instance.cwneKV.Value;		//管電圧
				CTSettings.scansel.Data.scan_ma = (float)frmScanControl.Instance.cwneMA.Value;		//管電流
			}
			//通常の場合、X線コントロール欄の値をscanselにセット
			else
			{
				//CTSettings.scansel.Data.scan_kv = (float)frmXrayControl.Instance.cwneKV.Value;		//管電圧
				//CTSettings.scansel.Data.scan_ma = (float)frmXrayControl.Instance.cwneMA.Value;		//管電流
                //制御器の設定値を書き込む   'v18.00変更 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05   //変更2014/10/07hata_v19.51反映
                CTSettings.scansel.Data.scan_kv = (float)frmXrayControl.Instance.ntbSetVolt.Value;      //管電圧
                CTSettings.scansel.Data.scan_ma = (float)frmXrayControl.Instance.ntbSetCurrent.Value;   //管電流
            }

#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/21
			modCT30K.UserLogOut("12,");
#endif

			//scansel書き込み    'v11.5追加 by 間々田 2006/07/10
			//modScansel.PutScansel(ref CTSettings.scansel.Data);
            CTSettings.scansel.Write();

#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/21
			modCT30K.UserLogOut("13,");
#endif

			//'PCフリーズ対策処理 added by 山本　2004-8-4                                                    ' CommCheck.exeを起動しない。シーケンサを止めないことにした（スキャン中もCommCheckの書き込みはCT30Kが行なう）
			//If scaninh.pc_freeze = 0 Then                                                                  'v11.4削除 by 間々田 2006/03/06
			//    'シーケンサ通信プログラム生成                   'v9.7 停止処理対応 by 間々田 2004/11/09    'v11.4削除 by 間々田 2006/03/06
			//    Set MyCommCheck = New clsCommCheck                                                         'v11.4削除 by 間々田 2006/03/06
			//    MyCommCheck.Execute                                                                        'v11.4削除 by 間々田 2006/03/06
			//End If                                                                                         'v11.4削除 by 間々田 2006/03/06

            //v19.50 タイマーの統合 by長野 2013/12/17    //変更2014/10/07hata_v19.51反映
            //frmMechaControl.Instance.tmrSeqComm.Interval = 2000;											//v11.4追加 by 間々田 2006/03/06
            frmMechaControl.Instance.tmrMecainfSeqComm.Interval = 2000;

			//mecainf更新のためのタイマーをオフ
            //v19.50 タイマーの統合 by長野 2013/12/17    //変更2014/10/07hata_v19.51反映
            //frmMechaControl.Instance.tmrMecainf.Enabled = false;
            modMechaControl.Flg_MechaControlUpdate = false;

            //Rev26.00
            stsMecha.Status = StringTable.GC_STS_BUSY;


			//v11.5追加 by 間々田 2006/07/28 ズーミング直後のスキャン時にオートズーミングスキャンとなってしまうことへの対策
			//modReconinf.GetReconinf(ref CTSettings.reconinf.Data);
            CTSettings.reconinf.Load();
            
            CTSettings.reconinf.Data.zoomflag = 0;
			//modReconinf.PutReconinf(ref CTSettings.reconinf.Data);
            CTSettings.reconinf.Write();

#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/21
			modCT30K.UserLogOut("14,");
#endif

            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)	//v17.00追加(ここから) byやまおか 2010/02/04
			{
				//ゲイン画像を読み込む 2009-09-29 山本
                if (CTSettings.detectorParam.Use_FlatPanel)
				{
                    if (CTSettings.scanParam.FPGainOn == true)
					{
                        CTSettings.scanParam.FPGainOn = false;		//ゲイン補正は行わない
						modCT30K.tmp_on = 1;
					}
					else
					{
						modCT30K.tmp_on = 0;
					}

					ScanCorrect.GetGainImage();

					frmTransImage.Instance.adv = 0;		//advは固定で2000とする
				}
			}																	//v17.00追加(ここまで) byやまおか 2010/02/04

#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/21
			modCT30K.UserLogOut("15,");
#endif
            //Rev20.00 ここでコモンの内容を全てファイルに吐き出すので
            //排他処理でとめられている操作がないように念のため少しだけ待つ。 //Rev20.00 by長野 2015/02/12
            modCT30K.PauseForDoEvents(1.0f);

            //Rev20.00 撮影前にこの時点でのコモンをファイルに落とす by長野 2014/09/11
            int sts = ComLib.SaveSharedCTCommon();
            if (sts != 0)
            {
                //CTソフトとタッチパネル操作をアンロック 
                UnLockCTsoftAndPanel();
                return functionReturnValue;
            }

            //子プロセスの起動    'v11.5追加 by 間々田 2006/07/03
            scanProcessID = modCT30K.StartProcess(ExeFileName);

            //Rev20.00 画面にロックがかかるので、スキャンスタートボタンだけ有効にする by長野 2014/12/15
            frmScanControl.Instance.ctbtnScanStart.Enabled = true;
            //Rev20.00 追加 by長野 2015/02/21
            frmScanControl.Instance.ctbtnScanStop.Enabled = true;
            modCT30K.PauseForDoEvents(1);

            //Rev20.00 ONしない by長野 2015/02/20
            ////Rev20.00 exe起動後に変更 by長野 2014/12/15
            //if (CTSettings.scaninh.Data.xray_remote == 0)
            //{
            //    //Ｘ線ＯＮ処理(アベイラブル待ち）
            //    sts = modXrayControl.TryXrayOn();
            //    if (sts != 0)
            //    {
            //        //CTソフトとタッチパネル操作をアンロック 
            //        UnLockCTsoftAndPanel();
            //        return functionReturnValue;
            //    }
            //}

            //
            //Scanスレッド起動
            //

            //Rev20.00 コーンの場合のみ、ConebeamGpgpu側が計算するjsとjeが確定するまで待つ by長野 2014/12/15
            //Rev20.00 データモードに関わらず待つ by長野 2015/02/20
            //if (ExeFileName == AppValue.CONEBEAM)
            //{
                int cnt = 0;
                while (modScanCondition.ScanStartFlg == false)
                {
                    //計算待ち
                    modCT30K.PauseForDoEvents(0.5f);
                    cnt++;
                    if (modScanCondition.ExScanStartAbortFlg == true)
                    {
                        sts = 1902;
                        modScanCondition.ExScanStartAbortFlg = false;
                        frmScanControl.Instance.ctbtnScanStart.Enabled = false;
                        frmScanControl.Instance.ctbtnScanStop.Enabled = false;
                        //frmScanControl.Instance.ctbtnScanStop.Visible = false;

                        break;
                    }
                    if (cnt > 360)
                    {
                        sts = 1913;
                        break;
                    }
                }
                //フラグを元に戻しておく。
                modScanCondition.ScanStartFlg = false;
            //}

            if (sts != 0)
            {
                UnLockCTsoftAndPanel();
   				//実行中の処理に対して停止要求をする     'v17.50上記の処理を関数化 by 間々田 2011/02/17
				modCT30K.CallUserStopSet();
                return (functionReturnValue);
            }


            //Rev20.00 マルチやスライスプランで再定義・再計算ができるように関数化してmodScanCondition.csに移動しました。by長野 2014/09/11
            sts = modScanCondition.ScanPreparation();

            if (sts != 0)
            {
                //CTソフトとタッチパネル操作をアンロック 
                UnLockCTsoftAndPanel();
                //実行中の処理に対して停止要求をする     'v17.50上記の処理を関数化 by 間々田 2011/02/17
                modCT30K.CallUserStopSet();
                return (functionReturnValue);

            }

            //Rev20.00 マルチ用に改造 by長野 2014/08/27
            //CTSettings.transImageControl.CaptureScanStart();
            //Rev20.00 シングルとコーンで分岐させる by長野 2014/08/27
            if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone)
            {
                if (CTSettings.scansel.Data.multiscan_mode != 1)
                {
                    CTSettings.transImageControl.CaptureScanStart(CTSettings.scansel.Data.multinum);
                }
                else
                {
                    CTSettings.transImageControl.CaptureScanStart();
                }
            }
            else if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScan)
            {
                if (CTSettings.scansel.Data.multiscan_mode != 1)
                {
                    CTSettings.transImageControl.CaptureSingleScanStart(CTSettings.scansel.Data.multinum);
                }
                else
                {
                    CTSettings.transImageControl.CaptureSingleScanStart();
                }
            }
            else if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeScano) //Rev21.00 追加 by長野 2015/02/19 
            {
                CTSettings.transImageControl.CaptureScanoStart();
            }

			functionReturnValue = true;

#if UserLog			//v17.48/v17.53追加 byやまおか 2011/03/21
			modCT30K.UserLogOut("16," + "\r\n");
#endif
			return functionReturnValue;
		}


		//********************************************************************************
		//機    能  ：  マルチモードのパラメータチェック
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：                   [ /O] Boolean   True:マルチ可  False:マルチ不可
		//補    足  ：
		//
		//履    歴  ：  V1.00  XX/XX/XX  (SI3)鈴山       新規作成
		//********************************************************************************
		private bool Multi_Check()
		{
			//Dim udab_pos    As Single
			float targetPos = 0;

			//'昇降位置、絶対座標
			//'    構造体名：mecainf
			//'    コモン名：udab_pos
			//udab_pos = GetCommonFloat("mecainf", "udab_pos")

			//mecainf.udab_pos:昇降位置（絶対座標）
			//scansel.pitch   :ｽｷｬﾝﾋﾟｯﾁ
			//scansel.multinum:ﾏﾙﾁ枚数
			//targetPos = udab_pos + scansel.pitch * (scansel.multinum - 1)
			targetPos = CTSettings.mecainf.Data.udab_pos + CTSettings.scansel.Data.pitch * (CTSettings.scansel.Data.multinum - 1);		//v11.5変更 by 間々田 2006/04/24
            
			//戻り値セット
            //return ((targetPos < CTSettings.GValUpperLimit) && (targetPos > CTSettings.GValLowerLimit));
            //Rev21.00/Rev20.00 変更 by長野 2015/03/20
            return ((targetPos <= CTSettings.GValUpperLimit) && (targetPos >= CTSettings.GValLowerLimit));

        }


		//********************************************************************************
		//機    能  ：  スライスプランモードのパラメータチェック
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：                   [ /O] Long      0:スライスプラン可  1:不可
		//補    足  ：  スライスプランモード以外の時はスライスプラン可としておく
		//
		//履    歴  ：  V2.0   00/02/08  (SI1)鈴山       新規作成
		//********************************************************************************
		//Private Function SlicePlan_Check(Optional ByVal ConeBeamOn As Boolean = False) As Long
		private bool SlicePlan_Check(bool ConeBeamOn = false)			//change by 間々田 2003/12/22
		{
			bool functionReturnValue = false;

			//Dim max_scan_area(DMODE_SZ) As Single   'スキャンエリア最大
			float max_scan_area = 0;				//スキャンエリア最大
			float theta0 = 0;
			float ksw = 0;
			float mce = 0;
			float DeltaZ = 0;						//スライスプランテーブルのコーンスライスピッチ
			int K = 0;								//スライスプランテーブルのコーンスライス枚数
			float SW = 0;							//スライスプランテーブルのスライス厚
			int View = 0;							//スライスプランテーブルのビュー数
			int theKlimit = 0;
			int scan_view = 0;						//ビュー数
			int matrix_size = 0;					//マトリクスサイズ(512 or 1024)
			string FileName = null;					//スライスプランテーブルファイル名
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Dim fileNo          As Integer  'ファイル番号
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
			string strREC = null;					//レコード
			string[] strCell = null;				//,で区切られた要素を格納する文字列型配列
			int Result = 0;							//スライスプランテーブルのエラーステータス

			//スキャンエリア最大
			if (ConeBeamOn)
			{
				max_scan_area = Convert.ToSingle(CTSettings.scansel.Data.cone_max_scan_area.ToString("0.000"));
			}
			else
			{
				max_scan_area = Convert.ToSingle(CTSettings.scansel.Data.max_scan_area[CTSettings.scansel.Data.scan_mode - 1].ToString("0.000"));
			}

			//v8.0 added by 間々田 2003/11/26 Start
			theta0 = CTSettings.scancondpar.Data.theta0[((CTSettings.scansel.Data.scan_mode == 3) ? 1 : 0)];
			ksw = (float)(1 - Math.Sin(theta0 / 2) / Math.Cos(CTSettings.scancondpar.Data.thetaoff));
			//v8.0 added by 間々田 2003/11/26 End

			//スライスプランテーブルの取得
			FileName = modCT30K.GetSlicePlanTable(ConeBeamOn);

			//エラー時の扱い
			//On Error GoTo FileError_Handler
			try
			{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				'ファイルオープン
				fileNo = FreeFile()
				Open FileName For Input As #fileNo

				Do Until EOF(fileNo)

					'１行読み込み
					Line Input #fileNo, strREC
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
				
				StreamReader sr = null;

				try
				{
					//ファイルオープン
                    //変更2015/01/22hata
                    //sr = new StreamReader(FileName);
                    sr = new StreamReader(FileName, Encoding.GetEncoding("shift-jis"));

					while ((strREC = sr.ReadLine()) != null)	//１行読み込み
					{
						if (!string.IsNullOrEmpty(strREC))
						{
							//文字型配列に分ける
							strCell = strREC.Split(',');

							//レコード先頭列が数字なら
							double isNumeric = 0;
							if (double.TryParse(strCell[0], out isNumeric))
							{
								//スライスプランテーブル・エラーステータス初期化
								Result = 0;		//ＯＫ

								int.TryParse(strCell[7], out scan_view);		//ビュー数
								int.TryParse(strCell[9], out matrix_size);		//マトリックスサイズ

								//絶対位置のチェック
								int strCell4 = 0;
								int.TryParse(strCell[4], out strCell4);
								//if (!modLibrary.InRangeFloat(strCell4, modGlobal.GValLowerLimit, modGlobal.GValUpperLimit))
                                if (!modLibrary.InRange(strCell4, CTSettings.GValLowerLimit, CTSettings.GValUpperLimit))
                                {
									Result = Result | SlicePlanNGUdAbsPos;
								}

								//スキャンエリアのチェック：オートセンタリングありの場合、チェックしない
								if (CTSettings.scansel.Data.auto_centering == 0)
								{
									//If (mscan_area > max_scan_area) Or (mscan_area <= min_scan_area) Then
									double strCell5 = 0;
									double.TryParse(strCell[5], out strCell5);
									if (!modLibrary.InRange(Convert.ToSingle(strCell5.ToString("0.000")), 0.001F, max_scan_area))
									{
										Result = Result | SlicePlanNGScanArea;
									}
								}

								//ビュー数のチェック
								//   GVal_ViewMin : ビュー数最小（コモン infdef.min_view）
								//   GVal_ViewMax : ビュー数最大（コモン infdef.max_view）
                                if (!modLibrary.InRange(scan_view, CTSettings.GVal_ViewMin, CTSettings.GVal_ViewMax))
								{
									Result = Result | SlicePlanNGScanView;
								}

								//If (scan_mode = 3 And table_rotation = 0 And multi_slice = 2 And scan_view > 1600) Or _
								//   (scan_mode = 3 And table_rotation = 0 And multi_slice = 1 And scan_view > 2000) Or _
								//   (scan_mode <> 3 And table_rotation = 0 And multi_slice = 2 And scan_view > 3000) Then
								//   Result = Result Or SlicePlanNGScanView2          'ビュー数(取り込み抜け）ＮＧ
								//End If

                                if (!(CTSettings.scansel.Data.table_rotation == 1))
								{
									if ((CTSettings.scansel.Data.scan_mode == 3 && CTSettings.scansel.Data.multislice == 2 && scan_view > 1600) || 
										(CTSettings.scansel.Data.scan_mode == 3 && CTSettings.scansel.Data.multislice == 1 && scan_view > 2000) || 
										(CTSettings.scansel.Data.scan_mode != 3 && CTSettings.scansel.Data.multislice == 2 && scan_view > 3000))
									{
										Result = Result | SlicePlanNGScanView2;				//ビュー数(取り込み抜け）ＮＧ
									}
								}

								//積算枚数のチェック
								//   GValIntegNumMin : 画像積算枚数最小（コモン infdef.min_integ_number）
								//   GValIntegNumMax : 画像積算枚数最大（コモン infdef.max_integ_number）
								int strCell8 = 0;
								int.TryParse(strCell[8], out strCell8);
                                if (!modLibrary.InRange(strCell8, CTSettings.GValIntegNumMin, CTSettings.GValIntegNumMax))
								{
									Result = Result | SlicePlanNGIntegNum;
								}

								//マトリクスサイズのチェック
								//If (matrix_size <> 512) And (matrix_size <> 1024) And (matrix_size <> 256) And (matrix_size <> 2048) Then
								//if (!(modLibrary.GetIndexByStr(modLibrary.RemoveNull(strCell[9]), modCommon.MyCtinfdef.matsiz) > -1))		//v11.4変更 by 間々田 2006/03/15
                                if (!(modCommon.MyCtinfdef.matsiz.GetIndexByStr(modLibrary.RemoveNull(strCell[9]), 0) > -1))
                                {
									Result = Result | SlicePlanNGMatrixSize;
								}

								//微調テーブルのＸ座標チェック（微調テーブルのＸ有の場合）   'v8.0追加 by 間々田 2003/11/17
								int strCell18 = 0;
								int.TryParse(strCell[18], out strCell18);
								if (strCell18 == 1)
								{
									double strCell19 = 0;
									double.TryParse(strCell[19], out strCell19);
									if (!modLibrary.InRange(Convert.ToSingle(strCell19.ToString("0.00")), 
																 Convert.ToSingle(CTSettings.t20kinf.Data.fx_lower_limit.ToString("0.00")), 
																 Convert.ToSingle(CTSettings.t20kinf.Data.fx_upper_limit.ToString("0.00"))))
									{
										Result = Result | SlicePlanNGFTableXpos;
									}
								}

								//微調テーブルのＹ座標チェック（微調テーブルのＹ有の場合）   'v8.0追加 by 間々田 2003/11/17
								int strCell20 = 0;
								int.TryParse(strCell[20], out strCell20);
								if (strCell20 == 1)
								{
									double strCell21 = 0;
									double.TryParse(strCell[21], out strCell21);
									if (!modLibrary.InRange(Convert.ToSingle(strCell21.ToString("0.00")), 
																 Convert.ToSingle(CTSettings.t20kinf.Data.fy_lower_limit.ToString("0.00")), 
																 Convert.ToSingle(CTSettings.t20kinf.Data.fy_upper_limit.ToString("0.00"))))
									{
										Result = Result | SlicePlanNGFTableYpos;
									}
								}

								double strCell6 = 0;
								double.TryParse(strCell[6], out strCell6);

								//コーンビーム時
								if (ConeBeamOn)
								{
									float.TryParse(strCell[16], out DeltaZ);		//スライスプランテーブルのコーンスライスピッチ
									int.TryParse(strCell[17], out K);				//スライスプランテーブルのコーンスライス枚数
									float.TryParse(strCell[6], out SW);				//スライスプランテーブルのスライス厚
									int.TryParse(strCell[7], out View);				//スライスプランテーブルのビュー数

									//コーンスライス枚数のチェック
									theKlimit = ((matrix_size == 1024) ? 128 : CTSettings.scancondpar.Data.klimit);
									if (!modLibrary.InRange(K, 1, theKlimit))
									{
										Result = Result | SlicePlanNGConeK;				//コーンスライス枚数ＮＧ
									}
									else
									{
										mce = (float)(0.5 * (Math.Pow(1024, 3) / (View * CTSettings.scancondpar.Data.fimage_hsize) - 1));
										if ((2 * (mce - 1) * CTSettings.scancondpar.Data.dpm * CTSettings.scansel.Data.fcd * ksw / CTSettings.scansel.Data.fid) > (DeltaZ * (K - 1) + SW * ksw))
										{ }
										else
										{
											Result = Result | SlicePlanNGConeWPK;		//スライス厚・コーンスライスピッチ・コーンスライス枚数ＮＧ
										}
									}

									//保存画像サイズを計算する
									modCT30K.Cal_SaveImageSize();
									//                    If GValRawFileSize > CLng(2) * CLng(1024) * CLng(1024) * CLng(1024) Then
									if ((modCT30K.GValRawFileSize > 2 * Math.Pow(1024, 3)))
									{
										Result = Result | SlicePlanNGRawDatSize;		//生データサイズ２ＧＢ以下の条件ＮＧ
									}
								}
								//コーンビーム時以外：スライス厚のチェック
                                //else if (!modLibrary.InRangeFloat(Convert.ToSingle(strCell6.ToString("0.000")), 
                                //                                  Convert.ToSingle(CTSettings.scansel.Data.min_slice_wid.ToString("0.000")), 
                                //                                  Convert.ToSingle(CTSettings.scansel.Data.max_slice_wid.ToString("0.000"))))
                                else if (!modLibrary.InRange(Convert.ToSingle(strCell6.ToString("0.000")),
                                                                  Convert.ToSingle(CTSettings.scansel.Data.min_slice_wid.ToString("0.000")),
                                                                  Convert.ToSingle(CTSettings.scansel.Data.max_slice_wid.ToString("0.000"))))
                                {
									Result = Result | SlicePlanNGSliceWid;
								}

								//テーブルに何らかの問題がある場合
								if (Result != 0)
								{
									frmSlicePlanCheck.Instance.lstMessage.Items.Add(CTResources.LoadResString(12918) + strCell[0] + ":");					//LoadResString(12918):スライス番号
									//If Result And SlicePlanNGUdAbsPos Then .AddItem vbTab & LoadResString(9415)  'スキャン位置が上下限値範囲外です。
									//If Result And SlicePlanNGScanArea Then .AddItem vbTab & LoadResString(9414)  'スキャンエリアが上下限値範囲外です。
									//If Result And SlicePlanNGSliceWid Then .AddItem vbTab & LoadResString(9416)   'スライス厚が上下限値範囲外です。
									//If Result And SlicePlanNGScanView Then .AddItem vbTab & LoadResString(9417)   'ビュー数が上下限値範囲外です。
									//If Result And SlicePlanNGIntegNum Then .AddItem vbTab & LoadResString(9420)   '積算枚数が上下限値範囲外です。
									//If Result And SlicePlanNGMatrixSize Then .AddItem vbTab & LoadResString(9419) 'マトリックスサイズが許容範囲外です。
									//If Result And SlicePlanNGScanView2 Then .AddItem vbTab & LoadResString(9418)  'ビュー数が大きく、画像取り込み抜けやメモリエラーを起こす可能性があります。
									//If Result And SlicePlanNGFTableXpos Then .AddItem vbTab & LoadResString(9441) '微調テーブルのＸ座標が上下限値範囲外です。
									//If Result And SlicePlanNGFTableYpos Then .AddItem vbTab & LoadResString(9442) '微調テーブルのＹ座標が上下限値範囲外です。
									//If Result And SlicePlanNGConeK Then .AddItem vbTab & LoadResString(9474)      'コーンスライス枚数が上下限値範囲外です。
									//If Result And SlicePlanNGConeWPK Then .AddItem vbTab & LoadResString(9475)    'スライス厚・スライスピッチ・コーンスライス枚数が不適切です。
									//If Result And SlicePlanNGRawDatSize Then .AddItem vbTab & LoadResString(9476) '生データサイズが２ＧＢを超えます。
									if ((Result & SlicePlanNGUdAbsPos) != 0) frmSlicePlanCheck.Instance.lstMessage.Items.Add("\t" + StringTable.BuildResStr(9928, StringTable.IDS_ScanPos));		//スキャン位置が上下限値範囲外です。
									if ((Result & SlicePlanNGScanArea) != 0) frmSlicePlanCheck.Instance.lstMessage.Items.Add("\t" + StringTable.BuildResStr(9928, StringTable.IDS_ScanArea));		//スキャンエリアが上下限値範囲外です。
									if ((Result & SlicePlanNGSliceWid) != 0) frmSlicePlanCheck.Instance.lstMessage.Items.Add("\t" + StringTable.BuildResStr(9928, StringTable.IDS_SliceWidth));		//スライス厚が上下限値範囲外です。
									if ((Result & SlicePlanNGScanView) != 0) frmSlicePlanCheck.Instance.lstMessage.Items.Add("\t" + StringTable.BuildResStr(9928, StringTable.IDS_ViewNum));		//ビュー数が上下限値範囲外です。
									if ((Result & SlicePlanNGIntegNum) != 0) frmSlicePlanCheck.Instance.lstMessage.Items.Add("\t" + StringTable.BuildResStr(9928, StringTable.IDS_IntegNum));		//積算枚数が上下限値範囲外です。
									if ((Result & SlicePlanNGMatrixSize) != 0) frmSlicePlanCheck.Instance.lstMessage.Items.Add("\t" + CTResources.LoadResString(9419));		//マトリックスサイズが許容範囲外です。
									if ((Result & SlicePlanNGScanView2) != 0) frmSlicePlanCheck.Instance.lstMessage.Items.Add("\t" + CTResources.LoadResString(9418));		//ビュー数が大きく、画像取り込み抜けやメモリエラーを起こす可能性があります。
									//'If Result And SlicePlanNGFTableXpos Then .AddItem vbTab & LoadResString(9441) '微調テーブルのＸ座標が上下限値範囲外です。
									//'If Result And SlicePlanNGFTableYpos Then .AddItem vbTab & LoadResString(9442) '微調テーブルのＹ座標が上下限値範囲外です。

									//'v14.24変更 by 間々田 2009/03/10 メカ軸名称はコモンを使用する
									//'   リソース9440新規追加：微調テーブルの%1座標が上下限値範囲外です。
									if ((Result & SlicePlanNGFTableXpos) != 0) frmSlicePlanCheck.Instance.lstMessage.Items.Add("\t" + StringTable.GetResString(9440, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[1].GetString())));
                                    if ((Result & SlicePlanNGFTableYpos) != 0) frmSlicePlanCheck.Instance.lstMessage.Items.Add("\t" + StringTable.GetResString(9440, modLibrary.RemoveNull(CTSettings.infdef.Data.m_axis_name[0].GetString())));

									if ((Result & SlicePlanNGConeK) != 0) frmSlicePlanCheck.Instance.lstMessage.Items.Add("\t" + CTResources.LoadResString(9474));		//コーンスライス枚数が上下限値範囲外です。
									if ((Result & SlicePlanNGConeWPK) != 0) frmSlicePlanCheck.Instance.lstMessage.Items.Add("\t" + CTResources.LoadResString(9475));		//スライス厚・スライスピッチ・コーンスライス枚数が不適切です。
									if ((Result & SlicePlanNGRawDatSize) != 0) frmSlicePlanCheck.Instance.lstMessage.Items.Add("\t" + CTResources.LoadResString(9476));	//生データサイズが２ＧＢを超えます。
								}

							}
						}
					}
				}
				finally
				{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				'ファイルクローズ
				Close #fileNo
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

					//ファイルクローズ
					if (sr != null)
					{
						sr.Close();
						sr = null;
					}

					//if (!modLibrary.IsExistForm(frmSlicePlanCheck.Instance))
                    if (!modLibrary.IsExistForm("frmSlicePlanCheck"))  //OpenしていないとLoadしてしまうため　2014/06/05(検S1)hata
                    {
						functionReturnValue = true;
					}
					else
					{
						functionReturnValue = frmSlicePlanCheck.Instance.Dialog(FileName);
					}
				}
			}
			catch (Exception ex)
			{
				//エラーメッセージ表示：
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				MsgBox Error(Err.Number), vbCritical, "SlicePlan Table Checking"
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
				MessageBox.Show(ex.Message, "SlicePlan Table Checking", MessageBoxButtons.OK, MessageBoxIcon.Error);
				functionReturnValue = false;
			}
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： 各コントロールの初期化
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V9.7  04/12/08 (SI4)間々田      新規作成
		//           v11.2 05/10/19 (SI3)間々田      scaninh構造体変数を使用
		//*******************************************************************************
		private void InitControls()
		{
			//メカ制御が可能な場合のみ、スキャンステータスラベルを表示する
			stsScan.Visible = (CTSettings.scaninh.Data.mechacontrol == 0);

			//メカ制御が可能な場合のみ、メカテータスラベルを表示する
			stsMecha.Visible = (CTSettings.scaninh.Data.mechacontrol == 0);

			//校正ステータスが表示可能、かつ、メカ制御が可能な場合のみ、校正ステータスラベル数を表示する
			stsCorrect.Visible = ((CTSettings.scaninh.Data.cor_status == 0) && (CTSettings.scaninh.Data.mechacontrol == 0));

			//v29.99 今のところ不要なのでfalse by長野 2013/04/08'''''ここから'''''
			//スキャン用のRAMディスクを使用する場合のみ、キャプチャステータスラベルを表示する
			//stsCapture.Visible = (.mechacontrol = 0) And UseRamDisk  'v17.40追加 byやまおか 2010/10/26
			stsCapture.Visible = false;
			//v29.99 今のところ不要なのでfalse by長野 2013/04/08'''''ここまで'''''
		}


		//*******************************************************************************
		//機　　能： フォームロード時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void frmStatus_Load(object sender, EventArgs e)
		{
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

            //プロパティがCaptionのため設定できないので直接設定する
            stsScan.Caption = CTResources.LoadResString(12028);
            stsCPU.Caption = CTResources.LoadResString(12297);
            stsMecha.Caption = CTResources.LoadResString(12302);
            stsCorrect.Caption = CTResources.LoadResString(12305);
            stsCapture.Caption = CTResources.LoadResString(12299);
            
			//v17.60 英語用レイアウト調整 by長野 2011/05/25
			if (modCT30K.IsEnglish == true)
			{
				EnglishAdjustLayout();
			}

			//コントロールの初期化
			InitControls();

			//フォームの表示位置：ＣＴ画像フォームの直下
			this.SetBounds(frmScanImage.Instance.Left, frmScanImage.Instance.Top + frmScanImage.Instance.Height, frmScanImage.Instance.Width, 0, 
							BoundsSpecified.X | BoundsSpecified.Y | BoundsSpecified.Width);


            //メカ制御画面への参照
			myMechaControl = frmMechaControl.Instance;
			myMechaControl.MecainfChanged += new EventHandler(myMechaControl_MecainfChanged);

			//v29.99 今のところ不要なのでfalse by長野 2013/04/08'''''ここから'''''
			//いちどステータスを更新する
			//    frmStatus.UpdateCaptureStatus
			//v29.99 今のところ不要なのでfalse by長野 2013/04/08'''''ここまで'''''

			//RAMディスク監視タイマーを起動する  'v17.40追加 byやまおか 2010/10/26
			//If UseRamDisk Then frmCTMenu.tmrRamDiskChk.Enabled = True
			//スキャンPCの場合だけ起動する(2ndPCの場合は起動しない)  'v17.43変更 byやまおか 2011/01/26
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//    If UseRamDisk And (scaninh.mechacontrol = 0) Then frmCTMenu.tmrRamDiskChk.Enabled = True
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

        }


		//*************************************************************************************************
		//機　　能： フォームアンロード時処理（イベント処理）
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Cancel          [ /O] Integer   True（0以外）: アンロードをキャンセル
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void frmStatus_FormClosed(object sender, FormClosedEventArgs e)
		{
			//メカ制御画面への参照破棄
            if (myMechaControl != null) //追加201501/26hata_if文追加
            {
                myMechaControl.MecainfChanged -= myMechaControl_MecainfChanged;
                myMechaControl = null;
            }

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//RAMディスク監視タイマーを停止する
			//    If UseRamDisk Then frmCTMenu.tmrRamDiskChk.Enabled = False  'v17.40追加 byやまおか 2010/10/26
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
		}


		//********************************************************************************
		//機    能  ：  校正ファイルの状態がスキャン可能かどうかを確認する
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：                   [ /O] Boolean   True:正常   False:異常
		//補    足  ：  なし
		//
		//履    歴  ：  V1.00  99/XX/XX  ??????????????  新規作成
		//              V2.0   00/02/08  (SI1)鈴山       回転中心校正ﾌｧｲﾙのﾁｪｯｸ個数を増やす(1→5)
		//              V7.0   00/10/06  (SI4)間々田     戻り値の型をBoolean型に修正
		//              v9.7   04/12/02  (SI4)間々田     ScanCorrect.bas より移動、Private化
		//********************************************************************************
		private bool IsOkCorrectFile()
		{
			//戻り値初期化
			bool functionReturnValue = false;

			//■各校正ファイルを調べる

			//幾何歪校正ファイル確認
			if (!File.Exists(ScanCorrect.VERT_CORRECT)) return functionReturnValue;

			//回転中心校正ファイル確認   MyScansel.MultiSlice 同時スキャン枚数:0(1ｽﾗｲｽ),1(3ｽﾗｲｽ),2(5ｽﾗｲｽ)
			if ((!File.Exists(ScanCorrect.RC01_CORRECT)) && (CTSettings.scansel.Data.multislice >= 0)) return functionReturnValue;
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//    If (Not FSO.FileExists(RC02_CORRECT)) And (scansel.multislice >= 1) Then Exit Function
			//    If (Not FSO.FileExists(RC03_CORRECT)) And (scansel.multislice >= 1) Then Exit Function
			//    If (Not FSO.FileExists(RC04_CORRECT)) And (scansel.multislice >= 2) Then Exit Function
			//    If (Not FSO.FileExists(RC05_CORRECT)) And (scansel.multislice >= 2) Then Exit Function
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//オフセット校正ファイル確認
			if (!File.Exists(ScanCorrect.OFF_CORRECT)) return functionReturnValue;

			//ゲイン校正ファイル確認
			//If Not FSO.FileExists(GAIN_CORRECT) Then Exit Function
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)		//v17.00 山本 2010-02-02
			{
				if (!File.Exists(ScanCorrect.GAIN_CORRECT_L)) return functionReturnValue;		//パーキンエルマーFPDの場合はLONG型の方だけチェックする
			}
			else
			{
				if (!File.Exists(ScanCorrect.GAIN_CORRECT)) return functionReturnValue;
			}

			//戻り値セット
			functionReturnValue = true;
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： 校正ステータスラベルの更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.2  2006/01/17   ????????      新規作成
		//*******************************************************************************
		public void UpdateCorrectStatus()
		{
			stsCorrect.Status = (frmScanControl.Instance.IsOkAll ? StringTable.GC_STS_STANDBY_OK : StringTable.GC_STS_STANDBY_NG);
		}


		//'*******************************************************************************
		//'機　　能： データモード表示の更新
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： タイマー処理からはずした
		//'
		//'履　　歴： v11.4  2006/03/13   ????????      新規作成
		//'*******************************************************************************
		//Public Sub UpdateDataMode()
		//
		//    'スキャン/コーンビームのラベル表示を切り替える
		//    stsScan.Header = LoadResString(IIf(scansel.data_mode = DataModeCone, IDS_ConeBeam, IDS_Scan)) 'コーンビーム/スキャン
		//
		//End Sub


		//*******************************************************************************
		//機　　能： メカステータスの更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.2  06/01/13  (SI3)間々田    新規作成
		//*******************************************************************************
		private void myMechaControl_MecainfChanged(object sender, EventArgs e)
		{
			//ステータス画面（frmStatus）にも表示                                    'v11.4追加 by 間々田 2006/03/13
			//   mecainf.mecha_ready 動作中・準備完了       (0:動作中　,1:準備完了)
			//   mecainf.mecha_busy 停止・動作中            (0:停止　　,1:動作中)
			//   mecainf.ud_ready   昇降準備完了            (0:準備未完,1:準備完了)
			//   mecainf.rot_ready  回転準備完了　　        (0:準備未完,1:準備完了)
			//   mecainf.phm_onoff  校正用ﾌｧﾝﾄﾑ機構位置　　 (0:原点    ,1:ﾌｧﾝﾄﾑ有位置)  FPDの場合ファントムは無視
			//
			//   v11.5変更 動作中と準備完了の表示優先順位を入れ替えた
			//If .mecha_busy = 1 Then
			if((CTSettings.mecainf.Data.mecha_busy == 1) || modSeqComm.IsSeqMoving())		//シーケンサに動作も含めることにした by 間々田 2009/05/12
			{
				stsMecha.Status = StringTable.GC_STS_BUSY;				//動作中
			}
			//        ElseIf (.mecha_ready = 1) And _
			//               (.rot_ready = 1) And _
			//               (.ud_ready = 1) And _
			//               ((.phm_onoff = 0) Or Use_FlatPanel) And _
			//               (IsOKIIPos() = True) And (IsOKDetPos() = True) Then  'v16.01 IsOKIIPos追加 by 山影 10-02-05 'v17.20 IsOKDetPosを追加 by 長野 10/09/06

            //else if ((CTSettings.mecainf.Data.mecha_ready == 1) && 
            //        (CTSettings.mecainf.Data.rot_ready == 1) && 
            //        (CTSettings.mecainf.Data.ud_ready == 1) &&
            //        (CTSettings.mecainf.Data.phm_onoff == 0 || CTSettings.detectorParam.Use_FlatPanel))
		    else if ((CTSettings.mecainf.Data.mecha_ready == 1) & 
                     (CTSettings.mecainf.Data.rot_ready == 1) & 
                     (CTSettings.mecainf.Data.ud_ready == 1) &
                     ((CTSettings.mecainf.Data.phm_onoff == 0) | CTSettings.detectorParam.Use_FlatPanel) & 
                     (modHighSpeedCamera.IsOKIIPos == true) & 
                     (mod2ndDetctor.IsOKDetPos == true) & 
                     (modMechaControl.Flg_StartupRotReset) &
                     (modMechaControl.Flg_StartupUpDownReset) &
                     (mod2ndXray.IsOKXrayPos == true) //Rev23.10 追加 by長野 2015/10/20
                )
            {
				stsMecha.Status = StringTable.GC_STS_STANDBY_OK;		//準備完了
			}
			else
			{
				stsMecha.Status = StringTable.GC_STS_STANDBY_NG;		//準備未完了
			}
		}


		//*******************************************************************************
		//機　　能： CTBusy関連ステータスの更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.2  06/01/13  (SI3)間々田    新規作成
		//*******************************************************************************
		public void UpdateCTBusyStatus()
		{
			//スキャンステータス（停止中・動作中）
			stsScan.Status = (((modCTBusy.CTBusy & modCTBusy.CTScanStart) != 0) ? StringTable.GC_STS_Scan : CTResources.LoadResString(12114));

			//ＣＰＵステータス（準備完了・処理中）
			stsCPU.Status = ((modCTBusy.CTBusy != 0) ? StringTable.GC_STS_CPU_BUSY : StringTable.GC_STS_STANDBY_OK);
		}


		//*******************************************************************************
		//機　　能： キャプチャステータスラベルの更新
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.2  2006/01/17   ????????      新規作成
		//*******************************************************************************
		//v29.99 今のところ不要なのでfalse by長野 2013/04/08'''''ここから'''''
		//Public Sub UpdateCaptureStatus()
		//
		//    stsCapture.Status = IIf(smooth_rot_cone_flg, GC_STS_STANDBY_OK, GC_STS_STANDBY_NG)
		//
		//End Sub
		//v29.99 今のところ不要なのでfalse by長野 2013/04/08'''''ここまで'''''


		//*******************************************************************************
		//機　　能： 英語用レイアウト調整
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V17.60  11/05/25  (検S１)長野      新規作成
		//*******************************************************************************
		private void EnglishAdjustLayout()
		{
			int margin = 10;

			stsMecha.Width = 200;
			stsMecha.CaptionWidth = 108;
			stsCorrect.Width = 200;
			stsCorrect.CaptionWidth = 108;
			stsCapture.Width = 100;
			stsCapture.CaptionWidth = 84;

			stsMecha.Left = stsScan.Left + stsScan.Width + margin;
			stsCPU.Left = stsMecha.Left + stsMecha.Width + margin;
			stsCorrect.Left = stsCPU.Left + stsCPU.Width + margin;
		}


		//*******************************************************************************
		//機　　能： CTソフトとタッチパネル操作のロック
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v17.61  2011/09/12   (検S1)長野　　　　　　　　　      新規作成
		//*******************************************************************************
		private void LockCTsoftAndPanel()
		{
			//タッチパネル操作のロック
			modSeqComm.SeqBitWrite("PanelInhibit", true);		//(False → True)

			//CTソフトのロック
			modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTMechaBusy;
		}


		//*******************************************************************************
		//機　　能： CTソフトとタッチパネル操作のアンロック
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v17.61  2011/09/12   (検S1)長野　　　　　　　　　      新規作成
		//*******************************************************************************
		private void UnLockCTsoftAndPanel()
		{
			//タッチパネル操作のアンロック
			modSeqComm.SeqBitWrite("PanelInhibit", false);		//(True → False)

			//CTソフトのアンロック
			modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTMechaBusy);
		}


		//*******************************************************************************
		//機　　能： テーブル回転位置が0度にいるか確認
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： Check_RotateZero[ /O] Boolean   True:正常   False:異常
		//
		//補　　足： なし
		//
		//履　　歴： v17.61/v18.00  2011/07/23  やまおか    新規作成
		//*******************************************************************************
		public bool Check_RotateZero()
		{
			//戻り値初期化
			bool functionReturnValue = false;

			int error_sts = 0;			//戻り値

            //追加2015/01/29hata
            //FCD のチェック  'v9.7追加 by added 間々田 2004/12/10
            if (!modSeqComm.CheckFCD(ScanCorrect.GVal_Fcd)) return functionReturnValue;


			//テーブル回転角度が０でない場合、０にしてからスタート   'v15.0追加 by 間々田 2009/07/21
			//If Not frmMechaControl.ctchkRotate(0).Value Then
			//If (Not frmMechaControl.ctchkRotate(0).Value) Or (Not Flg_StartupRotReset) Then 'v18.00追加 byやまおか 2011/07/01
			//if (! frmMechaControl.Instance.ctchkRotate[0].Value)		//v17.61戻す byやまおか 2011/09/14
            //v18.00追加 byやまおか 2011/07/01 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05       //変更2014/10/07hata_v19.51反映
            //if ((!frmMechaControl.Instance.ctchkRotate[0].Value) | (!modMechaControl.Flg_StartupRotReset)) 
            //Rev25.03 change by chouno 2017/02/19
            if ((!frmMechaControl.Instance.ctchkRotate[0].Value) | (!modMechaControl.Flg_StartupRotReset) || (CTSettings.mecainf.Data.rot_busy == 0 && CTSettings.mecainf.Data.rot_ready == 0))
            {
                //frmMessage.lblMessage.Caption = "テーブル回転角度を０度にリセットしています..."
				//ShowMessage "テーブル回転角度を０度にリセットしています..."                    '変更 by 間々田 2009/08/24
				modCT30K.ShowMessage(CTResources.LoadResString(20092));		//v14.99ストリングテーブル化 by長野 2011/05/25  'v19.02修正 byやまおか 2012/07/09
				//error_sts = MecaRotateIndex(0)
				//検出器によって回転方向を変える 'v17.02変更 byやまおか 2010/07/21
				//FPDの場合はCWで0に戻る
                if (CTSettings.detectorParam.Use_FlatPanel)
				{
					error_sts = modMechaControl.RotateInit(modDeclare.hDevID1);		//回転軸初期化
					error_sts = modMechaControl.MecaRotateOrigin(true);		        //回転軸原点復帰
				}
				//I.I.の場合はCCWで0に戻る
				else
				{
					error_sts = modMechaControl.MecaRotateIndex(0);
                }

				int WaitTime = 0;
				//WaitTime = 20 * 1000
				WaitTime = 30 * 1000;		//v16.20変更 微妙に足りないときがあるため20s→30s byやまおか 2010/04/21

				while (!frmMechaControl.Instance.ctchkRotate[0].Value)
				{
					if (modDoubleOblique.TimeOut(ref WaitTime))
					{
						//Unload frmMessage
						modCT30K.HideMessage();		//変更 by 間々田 2009/08/24
						//MsgBox "テーブル回転角度を原点の位置に戻すことができませんでした。", vbExclamation
						MessageBox.Show(CTResources.LoadResString(20093), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);		//v17.60 by長野 2011/05/25    'v19.02修正 byやまおか 20120/07/09
						return functionReturnValue;
					}
				}

				//Unload frmMessage
				modCT30K.HideMessage();		//変更 by 間々田 2009/08/24
			}

			functionReturnValue = true;
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： ゲイン校正時に取得したエアデータからPを計算する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： GetAirDataPValue[ /O] Boolean   True:正常   False:異常
		//
		//補　　足： なし
		//
		//履　　歴： v19.00              (検S1)長野              2012/04/10
		//*******************************************************************************
		public bool GetAirDataPValue()
		{
			//戻り値初期化
			bool functionReturnValue = false;

			int h_trim = 0;
			int v_trim = 0;
			float AirMean = 0;
			ushort[] TEMP_IMAGE = null;
			int Result = 0;

			//v19.00
			int AIR_ADD_VALUE = 0x7FFF;

			int bit_num = 0;
			double integ_num = 0;

			bit_num = 2 * CTSettings.scancondpar.Data.fimage_bit + 8;
			integ_num = CTSettings.scansel.Data.scan_integ_number;
			if (integ_num == 0) integ_num = 1;

            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
			{
                if (CTSettings.scaninh.Data.ct_gene2and3 == 0) // Rev23.20 2・3世代兼用の場合は上部の視野が欠けるためtrimの量を変える by長野 2016/01/23 
                {
                    if (CTSettings.detectorParam.h_size == 1024 && CTSettings.detectorParam.v_size == 1024)
                    {
                        h_trim = 212;
                        v_trim = 212;
                    }
                    else if (CTSettings.detectorParam.h_size == 2048 && CTSettings.detectorParam.v_size == 2048)
                    {
                        h_trim = 424;
                        v_trim = 424;
                    }
                }
                else
                {
                    if (CTSettings.detectorParam.h_size == 1024 && CTSettings.detectorParam.v_size == 1024)
                    {
                        h_trim = 12;
                        v_trim = 12;
                    }
                    else if (CTSettings.detectorParam.h_size == 2048 && CTSettings.detectorParam.v_size == 2048)
                    {
                        h_trim = 24;
                        v_trim = 24;
                    }
                }
			}
			else
			{
				h_trim = 0;
				v_trim = 0;
			}

            TEMP_IMAGE = new ushort[CTSettings.detectorParam.h_size * CTSettings.detectorParam.v_size];	//v15.0変更 -1した by 間々田 2009/06/03

			//ゲイン校正時に取得したエアデータを読み込む
            Result = ScanCorrect.ImageOpen(ref TEMP_IMAGE[0], ScanCorrect.GAIN_CORRECT_AIR, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);

			//ゲイン校正後のエアデータから平均値を出す
            ScanCorrect.Cal_Mean_short2(ref TEMP_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, h_trim, v_trim, ref AirMean);

			//積算枚数をかける
			AirMean = AirMean * CTSettings.scansel.Data.scan_integ_number;

			//modScancondpar.CallGetScancondpar();
            CTSettings.scancondpar.Load(CTSettings.scaninh.Data.rotate_select);

			//v19.00 シングルとコーンではlogValueの計算式が異なる。スキャン時とエアデータ取得時の管電流の違いによる補正は、スキャンソフト側で行う by長野 2012/05/10
			if (CTSettings.scansel.Data.data_mode == (int)ScanSel.DataModeConstants.DataModeCone)
			{
				//scancondpar.mbhc_airLogValue = AIR_ADD_VALUE - ((2 ^ 15 - 1) / Log(integ_num * (2 ^ bit_num - 1))) * Log(AirMean) + AIR_ADD_VALUE + (frmXrayControl.ntbSetCurrent.Value / mecainf.gain_ma)
				CTSettings.scancondpar.Data.mbhc_airLogValue = (float)(AIR_ADD_VALUE - ((Math.Pow(2, 15) - 1) / Math.Log(integ_num * (Math.Pow(2, bit_num) - 1))) * Math.Log(AirMean) + AIR_ADD_VALUE);
			}
			else
			{
				//scancondpar.mbhc_airLogValue = AIR_ADD_VALUE - ((2 ^ 15 - 1) / Log((2 ^ bit_num - 1))) * Log(AirMean) + AIR_ADD_VALUE + (frmXrayControl.ntbSetCurrent.Value / mecainf.gain_ma)
				CTSettings.scancondpar.Data.mbhc_airLogValue = (float)(AIR_ADD_VALUE - ((Math.Pow(2, 15) - 1) / Math.Log(Math.Pow(2, bit_num) - 1)) * Math.Log(AirMean) + AIR_ADD_VALUE);
			}

			//modScancondpar.CallPutScancondpar();
            CTSettings.scancondpar.Write();

			if (Result == 0)
			{
				functionReturnValue = true;
			}

			return functionReturnValue;
		}

        //追加2014/05hata
        private void frmStatus_Activated(object sender, EventArgs e)
        {
            ////描画を強制する
            //if (this.Visible && this.Enabled) this.Refresh();
        }


	}
}
