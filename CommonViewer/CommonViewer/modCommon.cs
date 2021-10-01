//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using CTAPI;
using CT30K.Common;

namespace CT30K
{
	static class modCommon
	{
///* ************************************************************************** */
///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
///* 客先　　　　： ?????? 殿                                                   */
///* プログラム名： modCommon.bas                                               */
///* 処理概要　　： コモン関連のモジュールをまとめた                            */
///* 注意事項　　： なし                                                        */
///* -------------------------------------------------------------------------- */
///* 適用計算機　： DOS/V PC                                                    */
///* ＯＳ　　　　： Windows XP  (SP2)                                           */
///* コンパイラ　： VB 6.0                                                      */
///* -------------------------------------------------------------------------- */
///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
///*                                                                            */
///* V11.5      06/09/21    (WEB)間々田          新規作成                       */
///* V19.00     12/02/20    H.Nagai              BHC対応                        */
///*                                                                            */
///* -------------------------------------------------------------------------- */
///* ご注意：                                                                   */
///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
///*                                                                            */
///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
///* ************************************************************************** */


//********************************************************************************
//  構造体宣言
//********************************************************************************
//5（ctinfdefのサブセット）構造体（構造体各要素は可変文字列とする）
		public struct MyCtinfdefType
		{

            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]     //'ｽｷｬﾝﾓｰﾄﾞ（FULLﾓｰﾄﾞ）   'v18.00変更 byやまおか 2011/07/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public FixedString6[] full_mode;/* ｽｷｬﾝﾓｰﾄﾞ（FULLﾓｰﾄﾞ）		*/

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public FixedString4[] matsiz;/* 画像ﾏﾄﾘｯｸｽ                  */ //v16.10 変更 by 長野　2010/02/02

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString2[] scan_area;/* 撮影領域                    */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString8[] fc;/* ﾌｨﾙﾀ関数                    */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public FixedString16[] filter_process;/*ﾌｨﾙﾀ処理の方法 0:FFT 1:ｺﾝﾎﾞﾘｭｰｼｮﾝ*/	// Rev13.00 追加 2007-01-22 やまおか

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString16[] iifield;/* I.I.視野			REV7.04 */
            
            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // 配列の初期化
                //full_mode = FixedString.CreateArray<FixedString6>(3);   // スキャンモード（FULLモード）
                full_mode = FixedString.CreateArray<FixedString6>(4);   // スキャンモード（FULLモード  // 'ｽｷｬﾝﾓｰﾄﾞ（FULLﾓｰﾄﾞ）   'v18.00変更 byやまおか 2011/07/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                matsiz = FixedString.CreateArray<FixedString4>(6);      // 画像マトリクス
                scan_area = FixedString.CreateArray<FixedString2>(3);   // 撮影領域
                fc = FixedString.CreateArray<FixedString8>(3);          // フィルタ関数
                filter_process = FixedString.CreateArray<FixedString16>(2);  // Ｘ線焦点（形式）  REV1.00
                iifield = FixedString.CreateArray<FixedString16>(3);  // Ｘ線焦点（形式）  REV1.0
            }
            #endregion
        
		}

        //MyCtinfdef構造体変数
		public static MyCtinfdefType MyCtinfdef;

        //データ収集条件 'バックアップ用
		private static ScanSel scanselTmp;

        /*
        //********************************************************************************
        //  外部関数宣言
        //********************************************************************************
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int ctcominit(int Mode);
		[DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //Private Declare Function getcommon_long Lib "comlib.dll" (ByVal com_name As String, ByVal Name As String, Data As Long) As Long
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
		public static extern int putcommon_long(string com_name, string Name, int Data);
		[DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int UserStopSet();
		[DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int UserStopClear();
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //Public Declare Function get_det_com Lib "comlib.dll" (ByVal det_no As Long) As Long
        //Public Declare Function ramdiskclear Lib "comlib.dll" () As Long 'v17.40 追加 by 長野
        //Public Declare Function scanstop_set_rmdsk Lib "comlib.dll" () As Long 'v17.40 追加 by 長野
        //Public Declare Function UserStopSet_rmdsk Lib "comlib.dll" () As Long 'v17.40 追加 by 長野
        //Public Declare Function UserStopClear_rmdsk Lib "comlib.dll" () As Long 'v17.40 追加 by 長野
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''

        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //v19.00 産業用CTからコピー (電S2)永井
        //Private Declare Function getcommon_float Lib "comlib.dll" (ByVal com_name As String, ByVal Name As String, Data As Single) As Long
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
        */



        //scanselのバックアップ
		public static void BackupScansel()
		{

			//modScansel.GetScansel(ref scanselTmp);
            //backup変数に保持する
            CTSettings.scansel.Backup();

		}

        //scanselのリストア
		public static void RestoreScansel()
		{

			//modScansel.PutScansel(ref scanselTmp);
            //backup変数のデータに書き込む
            CTSettings.scansel.Restore();


			//scanselに反映させる    'v14.0追加 by 間々田 2007/08/08
			GetMyScansel();

		}

        //*******************************************************************************
        //機　　能： コモンからScansel情報を取得し、グローバル変数に記憶させる
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： この関数は、CT30K起動時、コモン初期化時、および
        //           スキャン条件（コーンスキャン条件）画面にて条件保存時に実行される
        //           なお、この関数以外では極力 MyScanselの値を書き換えないこと
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        //Public Sub GetScansel()   //v11.2変更 by 間々田 2006/01/13
		public static void GetMyScansel()
		{

			//scansel取得
			//modScansel.GetScansel(ref modScansel.scansel);
            //scansel(コモン)の取得
            if (CTSettings.scansel == null)
            {
                CTSettings.scansel = new ScanSel();
                CTSettings.scansel.Data.Initialize();
            }
            if (!CTSettings.scansel.Load(CTSettings.scaninh.Data))
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("ScanSel読み込み失敗",
                //                    Application.ProductName,
                //                    MessageBoxButtons.OK,
                //                    MessageBoxIcon.Error);
            }

            //CTSettings.scansel.Load(CTSettings.scaninh.Data)内で実施している
            /*
            var _scansel = CTSettings.scansel.Data;

            //scanselの補正

            //マトリクスサイズ：2(512x512),3(1024x1024)
            switch (_scansel.matrix_size)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    //v16.10変更 4→5 長野 2010/02/02
                    break;
                default:
                    _scansel.matrix_size = 2;
                    break;
            }

            //スキャンモード：1(ﾊｰﾌ),2(ﾌﾙ),3(ｵﾌｾｯﾄ)                  注）frmTableAutoMove.cmdMove_Click でも変更される
            switch (_scansel.scan_mode)
            {
                case 1:
                case 2:
                case 3:
                    break;
                default:
                    _scansel.scan_mode = 2;
                    break;
            }

            //積算枚数
            _scansel.scan_integ_number = modLibrary.MaxVal(_scansel.scan_integ_number, 1);

            #region v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //        'X線管：0(130kV),1(225kV)                               注）Set_RotationCenter_Parameterでも変更される
            //        .multi_tube = IIf(.multi_tube = 1, 1, 0)
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            #endregion

            //回転選択：0(テーブル), 1(Ｘ線管)
            _scansel.rotate_select = (_scansel.rotate_select == 1 ? 1 : 0);

            //マルチスライス
            _scansel.max_multislice = modLibrary.MinVal(_scansel.max_multislice, 2);//同時スキャン枚数最大(0:1ｽﾗｲｽ,1:3ｽﾗｲｽ,2:5ｽﾗｲｽ)
            _scansel.multislice = modLibrary.MinVal(_scansel.multislice, _scansel.max_multislice);//同時スキャン枚数(0:1ｽﾗｲｽ,1:3ｽﾗｲｽ,2:5ｽﾗｲｽ)
            _scansel.multislice_pitch = modLibrary.MinVal(_scansel.multislice_pitch, _scansel.max_multislice_pitch);//同時スキャンピッチ


            #region v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //コーン分散処理：0(なし),1(あり)
            //        .cone_distribute = IIf(((scaninh.cone_distribute = 0) Or (scaninh.cone_distribute2 = 0)) And _
            //'                               (.cone_distribute = 1) And (.data_mode = DataModeCone), 1, 0)
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            #endregion
            */

        }

        //*******************************************************************************
        //機　　能： MyCtinfdef(Ctinfdefのサブセット、スライスプロジェクトで使用する文字列の構造体)を作成
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： Ctinfdefのままだと扱いにくいので作成した
        //
        //履　　歴： v7.00 2005/08/25 (SI3)間々田    新規作成
        //*******************************************************************************
        public static void SetMyCtinfdef()
        {

            int i = 0;
            MyCtinfdef.Initialize();

            var _with2 = MyCtinfdef;

            //for (i = Information.LBound(_with2.full_mode); i <= Information.UBound(_with2.full_mode); i++)
            //{
            //    //ｽｷｬﾝﾓｰﾄﾞ（FULLﾓｰﾄﾞ）
            //    //_with2.full_mode[i] = modLibrary.RemoveNull(CTSettings.ctinfdef.Data.full_mode[i].GetString());
            //    _with2.full_mode[i].SetString(modLibrary.RemoveNull(CTSettings.ctinfdef.Data.full_mode[i].GetString()));
            //}

            //for (i = Information.LBound(_with2.matsiz); i <= Information.UBound(_with2.matsiz); i++) {
            //  //画像ﾏﾄﾘｯｸｽ
            //    //_with2.matsiz[i] = modLibrary.RemoveNull(CTSettings.ctinfdef.Data.matsiz[i]);
            //    _with2.matsiz[i].SetString( modLibrary.RemoveNull(CTSettings.ctinfdef.Data.matsiz[i].GetString()));
            //}

            //for (i = Information.LBound(_with2.scan_area); i <= Information.UBound(_with2.scan_area); i++) {
            //    //撮影領域
            //    //_with2.scan_area[i] = modLibrary.RemoveNull(CTSettings.ctinfdef.Data.scan_area[i]);
            //    _with2.scan_area[i].SetString(modLibrary.RemoveNull(CTSettings.ctinfdef.Data.scan_area[i].GetString()));
            //}

            //for (i = Information.LBound(_with2.fc); i <= Information.UBound(_with2.fc); i++) {
            //    //ﾌｨﾙﾀ関数
            //    //_with2.fc[i] = modLibrary.RemoveNull(modCtinfdef.ctinfdef.fc[i]);
            //    _with2.fc[i].SetString(modLibrary.RemoveNull(CTSettings.ctinfdef.Data.fc[i].GetString()));
            //}

            ////v13.00追加 by Ohkado?
            //for (i = Information.LBound(_with2.filter_process); i <= Information.UBound(_with2.filter_process); i++) {
            //    //ﾌｨﾙﾀ処理
            //    //_with2.filter_process[i] = modLibrary.RemoveNull(CTSettings.infdef.Data.filter_process[i]);
            //    _with2.filter_process[i].SetString(modLibrary.RemoveNull(CTSettings.infdef.Data.filter_process[i].GetString()));
            //}

            ////v15.0追加 by 間々田 2009/04/10
            //for (i = Information.LBound(_with2.iifield); i <= Information.UBound(_with2.iifield); i++) {
            //    //I.I.視野
            //    //_with2.iifield[i] = modLibrary.RemoveNull(CTSettings.infdef.Data.iifield[i]);
            //    _with2.iifield[i].SetString(modLibrary.RemoveNull(CTSettings.infdef.Data.iifield[i].GetString()));
            //}

            for (i = 0; i < MyCtinfdef.full_mode.Length; i++)
            {
                //ｽｷｬﾝﾓｰﾄﾞ（FULLﾓｰﾄﾞ）
                //_with2.full_mode[i].SetString(modLibrary.RemoveNull(CTSettings.ctinfdef.Data.full_mode[i].GetString()));
                _with2.full_mode[i] = CTSettings.ctinfdef.Data.full_mode[i];
            
            }

            for (i = 0; i < _with2.matsiz.Length; i++)
            {
                //画像ﾏﾄﾘｯｸｽ
                //_with2.matsiz[i].SetString(modLibrary.RemoveNull(CTSettings.ctinfdef.Data.matsiz[i].GetString()));
                _with2.matsiz[i] = CTSettings.ctinfdef.Data.matsiz[i];
            }

            for (i = 0; i < _with2.scan_area.Length; i++)
            {
                //撮影領域
                //_with2.scan_area[i].SetString(modLibrary.RemoveNull(CTSettings.ctinfdef.Data.scan_area[i].GetString()));
                _with2.scan_area[i] = CTSettings.ctinfdef.Data.scan_area[i];
            }

            for (i = 0; i < _with2.fc.Length; i++)
            {
                //ﾌｨﾙﾀ関数
                //_with2.fc[i].SetString(modLibrary.RemoveNull(CTSettings.ctinfdef.Data.fc[i].GetString()));
                _with2.fc[i] = CTSettings.ctinfdef.Data.fc[i];
            }

            //v13.00追加 by Ohkado?
            for (i = 0; i < _with2.filter_process.Length; i++)
            {
                //ﾌｨﾙﾀ処理
                //_with2.filter_process[i].SetString(modLibrary.RemoveNull(CTSettings.infdef.Data.filter_process[i].GetString()));
                _with2.filter_process[i] = CTSettings.infdef.Data.filter_process[i];
            }

            //v15.0追加 by 間々田 2009/04/10
            for (i = 0; i < _with2.iifield.Length; i++)
            {
                //I.I.視野
                //_with2.iifield[i].SetString(modLibrary.RemoveNull(CTSettings.infdef.Data.iifield[i].GetString()));
                _with2.iifield[i] = CTSettings.infdef.Data.iifield[i];
            }

        }
        
        #region v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //********************************************************************************
        //機    能  ：  getcommon_long関数を呼び出す
        //              変数名           [I/O] 型        内容
        //引    数  ：  com_name         [I/ ] String    構造体名
        //              name             [I/ ] String    コモン名
        //
        //戻 り 値  ：                   [ /O] Long      getcommon_long関数で取得した値
        //補    足  ：  なし
        //
        //履    歴  ：  v7.0   03/07/07  (SI4)間々田     新規作成
        //********************************************************************************
        //Public Function GetCommonLong(ByVal com_name As String, ByVal Name As String) As Long
        //
        //    Dim theValue    As Long
        //
        //    'コモン関数の実行
        //    If getcommon_long(com_name, Name, theValue) <> 0 Then
        //        'エラー表示：[中止]なら終了
        //        If ErrMessage(2218) = vbAbort Then End
        //    End If
        //
        //    '値を返す
        //    GetCommonLong = theValue
        //
        //End Function
        #endregion

        #region v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //'********************************************************************************
        //'機    能  ：  putcommon_long関数を呼び出す
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  strStructName    [I/ ] String    構造体名
        //'              strCommonName    [I/ ] String    コモン名
        //'              theData          [I/ ] Long      設定するデータ値
        //'戻 り 値  ：                   [ /O] Long      putcommon_long関数の戻り値
        //'補    足  ：  なし
        //'
        //'履    歴  ：  v9.5   04/09/28  (SI4)間々田     新規作成
        //'********************************************************************************
        //'Public Function SetCommonLong(ByVal strStructName As String, ByVal strCommonName As String, ByVal theData As Long) As Long
        //Public Function SetCommonLong(ByVal strStructName As String, ByVal strCommonName As String, ByVal theData As Long) As Boolean
        //
        //    Dim error_sts   As Long     'putcommon_long関数の戻り値
        //
        //    '戻り値初期化
        //    SetCommonLong = False
        //
        //    'すでに登録されているコモン値と同じなら何もしない
        //    If GetCommonLong(strStructName, strCommonName) = theData Then Exit Function
        //
        //    'コモン関数の実行
        //    error_sts = putcommon_long(strStructName, strCommonName, theData)
        //
        //    'エラーメッセージ
        //    If error_sts <> 0 Then
        //        'エラー表示：[中止]なら終了
        //        If ErrMessage(2219) = vbAbort Then End
        //    End If
        //
        //    '戻り値セット
        //    'SetCommonLong = error_sts
        //    SetCommonLong = True
        //
        //End Function
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
        #endregion

        #region v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //v19.00 追加 ->(電S2)永井
        //'********************************************************************************
        //'機    能  ：  getcommon_float関数を呼び出す
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  com_name         [I/ ] String    構造体名
        //'              name             [I/ ] String    コモン名
        //'
        //'戻 り 値  ：                   [ /O] Single      getcommon_float関数で取得した値
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V5.0   04/05/27  (SI4)間々田     新規作成
        //'********************************************************************************
        //Public Function GetCommonFloat(ByVal com_name As String, ByVal Name As String) As Single
        //
        //    Dim theValue    As Single
        //
        //    'コモン関数の実行
        //    If getcommon_float(com_name, Name, theValue) <> 0 Then
        //        Call ErrMessage(2220) 'エラー表示：[中止]なら終了
        //    End If
        //
        //    GetCommonFloat = theValue
        //
        //End Function

        //<- v19.00
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
        #endregion

 

		public static int Getmsetup_acq_view(
            int	view ,			    //(I) 再構成ﾋﾞｭｰ数
            double  delta_fai_m,	//(I) X線管の1ビュー当たりの回転角(radian)
            int js		   ,	    //(I) 純生データの縦開始位置
            int	je		   ,	    //(I) 純生データの縦終了位置
            ref int m_alpha,		//(O) ｵｰﾊﾞｰﾗｯﾌﾟα°分のﾋﾞｭｰ数
            ref uint mov,			//(O) 実際に収集するﾋﾞｭｰ数
            ref int cob_view,		//(O) つの生ﾃﾞｰﾀﾌｧｲﾙに入る基本view数(最後のﾌｧｲﾙは余りが入る)
            ref int cob_num,        //(O) 生ﾃﾞｰﾀ分割数
            ref int mainch,             
            ref int md)		   
            
        {
	        double	maxfanangle ; //ﾊｰﾌｽｷｬﾝ時のﾃﾞｰﾀ収集ﾌｧﾝ角の計算に使用する
            int j               ; //ｶｳﾝﾀ
	        //double	pai		 ; //円周率
	        int	    viewsize	; //Rev16.30 ｺｰﾝﾋﾞｰﾑ生ﾃﾞｰﾀ1view当りのサイズ 10-05-18 by IWASAWA
	        int	    pur_viewsize; //Rev16.30 ｺｰﾝﾋﾞｰﾑ純生ﾃﾞｰﾀ1view当りのサイズ 10-05-18 by IWASAWA
	        double	max_viewsize; //Rev16.30 1view当たりの生or 純生の大きい方のサイズ（単位：MB）　//単位がByteのままだとlong型の最大が2Gになってしまう。unsigned longでも4Gになってしまうため

            const int HALF     = 1;
            const int FULL     = 2;
            const int DFULL    = 3;
            const int SCAN	   = 1;
            const int GAIN	   = 2;
            const int SCANO    = 3;
            const int CONEBEAM = 4;
            const int ONE_MEGA = 1048576;	//1MByte
            const int COB_MAX_SIZE = 1800;	//ｺｰﾝﾋﾞｰﾑ生ﾃﾞｰﾀ最大容量(1ﾌｧｲﾙあたり) 単位MB　　本番は余裕を見て1999MBに設定のこと
            const int SCBNUM		= 30;	// ｺｰﾝﾋﾞｰﾑ生ﾃﾞｰﾀ分割最大分割数 10-05-17 by IWASAWA

            var _scansel = CTSettings.scansel.Data;
            var _scancondpar = CTSettings.scancondpar.Data;

            mainch =_scancondpar.mainch[_scansel.scan_focus - 1]/(int)_scancondpar.h_mag[_scansel.binning];
            md = _scansel.mc * 2 + 1; 

	        //pai = Math.PI;	//(double)3.141592654 ;
	        if(_scansel.scan_mode == HALF)
	        {
		        m_alpha = (int)((double)_scancondpar.alpha_h / delta_fai_m) + 1 ;
	        }
	        else
	        {
		        m_alpha = (int)((double)_scancondpar.alpha / delta_fai_m) + 1 ;
	        }

            if(_scansel.data_mode == SCAN)
	        {
		        //Half SCAN時のﾃﾞｰﾀ収集view数を計算
		        if((_scansel.scan_mode) == 1) {
			        //Rev.2.00 複数ｽﾗｲｽ同時ｽｷｬﾝ時は最大となるファン角から計算する
			        maxfanangle = (double)0.0 ; 	//maxfanangle 初期化
			        switch(_scansel.multislice)
			        {
				        case 0:
					        if(_scansel.auto_centering == 1)	//ﾊｰﾌｽｷｬﾝのｵｰﾄｾﾝﾀﾘﾝｸﾞ
					        {
						        maxfanangle = (double)_scancondpar.max_mfanangle[2] ;
                                mov = (uint)(view / 2 + ((maxfanangle / delta_fai_m) + 1));		//Rev11.0 maxfanangleはrad 05-07-25　by IWASAWA
					        }
					        else
					        {
                                //maxfanangle = (double)_scancondpar.mfanangle[2 + (_scansel.scan_mode - 1) * 5] ;
                                //Rev99.99 変更 by長野 2014/10/16
                                maxfanangle = (double)_scancondpar.mfanangle[2 * 4 + (_scansel.scan_mode - 1)];
                                mov = (uint)(view * ((180.0 + maxfanangle) / 360.0)) + 1;     //切り上げのための+1
					        }
					        break ;
				        case 1 :
					        if(_scansel.auto_centering == 1)	//ﾊｰﾌｽｷｬﾝのｵｰﾄｾﾝﾀﾘﾝｸﾞ
					        {
						        for(j = 1 ; j < 4 ; j++)
						        {
							        if(maxfanangle > (double)_scancondpar.max_mfanangle[j])
							        {
								        maxfanangle = (double)_scancondpar.max_mfanangle[j] ;
							        }
						        }
                                mov = (uint)(view / 2 + ((maxfanangle / delta_fai_m) + 1));		//Rev11.0 maxfanangleはrad 05-07-25　by IWASAWA
					        }
					        else
					        {
						        for(j = 1 ; j < 4 ; j++)
						        {
                                    //if (maxfanangle > (double)_scancondpar.mfanangle[j + (_scansel.scan_mode - 1) * 5])
                                    //{
                                    //    maxfanangle = (double)_scancondpar.mfanangle[j + (_scansel.scan_mode - 1) * 5];
                                    //}
                                    //Rev99.99 変更 by長野 2014/10/16
                                    if (maxfanangle > (double)_scancondpar.mfanangle[j * 4 + (_scansel.scan_mode - 1)])
                                    {
                                        maxfanangle = (double)_scancondpar.mfanangle[j * 4 + (_scansel.scan_mode - 1)];
                                    }
						        }
                                mov = (uint)(view * ((180.0 + maxfanangle) / 360.0)) + 1;     //切り上げのための+1
					        }
					        break ;

				        case 2:
					        if(_scansel.auto_centering == 1)	//ﾊｰﾌｽｷｬﾝのｵｰﾄｾﾝﾀﾘﾝｸﾞ
					        {
						        for(j = 0 ; j < 5 ; j++)
						        {
							        if(maxfanangle < (double)_scancondpar.max_mfanangle[j])
							        {
								        maxfanangle = (double)_scancondpar.max_mfanangle[j] ;
							        }
						        }
                                mov = (uint)(view / 2 + ((maxfanangle / delta_fai_m) + 1));		//Rev11.0 maxfanangleはrad 05-07-25　by IWASAWA
					        }
					        else
					        {
						        for(j = 0 ; j < 5 ; j++)
						        {
                                    //if (maxfanangle < (double)_scancondpar.mfanangle[j + (_scansel.scan_mode - 1) * 5])
                                    //{
                                    //    maxfanangle = (double)_scancondpar.mfanangle[j + (_scansel.scan_mode - 1) * 5];
                                    //}
                                    //Rev99.99 変更 by長野 2014/10/16
                                    if (maxfanangle < (double)_scancondpar.mfanangle[j * 4 + (_scansel.scan_mode - 1)])
                                    {
                                        maxfanangle = (double)_scancondpar.mfanangle[j * 4 + (_scansel.scan_mode - 1)];
                                    }
						        }
                                mov = (uint)(view * ((180.0 + maxfanangle) / 360.0)) + 1;     //切り上げのための+1
					        }
					        break ;
			        }
            
		        } else	//通常ｽｷｬﾝ, ｵﾌｾｯﾄｽｷｬﾝ
		        {
                    mov = (uint)view;
		        }
	        }
	        else if(_scansel.data_mode == CONEBEAM)
	        {
		        //Half Conebeam 時のﾃﾞｰﾀ収集view数を計算
		        if((_scansel.scan_mode) == HALF) 
		        {
			        if(_scansel.auto_centering == 1)	//ﾊｰﾌｽｷｬﾝのｵｰﾄｾﾝﾀﾘﾝｸﾞ
			        {
				        maxfanangle = (double)_scancondpar.cone_max_mfanangle ;
                        mov = (uint)(view / 2 + ((maxfanangle / delta_fai_m) + 1));		//Rev11.0 maxfanangleはrad 05-07-25　by IWASAWA
			        }
			        else
			        {
				        maxfanangle = (double)_scancondpar.theta0[0];	//Conebeamのﾊｰﾌはﾌﾙと同じ
				        //*mov = (unsigned long)(view * ((180.0 + maxfanangle) / 360.0)) + 1 ;     //切り上げのための+1
                        mov = (uint)(view / 2 + ((maxfanangle / delta_fai_m) + 1));     //切り上げのための+1  //Rev11.0 maxfanangleは(rad) 05-05-25 by IWASAWA
			        }
			
		        }
		        else	//通常Conebeam, ｵﾌｾｯﾄConebeam
		        {
                    mov = (uint)view;
		        }

	        }
	        else
	        {
		        return(1);	//ありえない
	        }

	        //ｵｰﾊﾞｰｽｷｬﾝﾊﾟﾗﾒｰﾀ設定 SCAN, Conebeam共通
	        if(_scansel.over_scan == 1)
	        {
		        if((_scansel.scan_mode) == HALF)		//ﾊｰﾌｽｷｬﾝ
		        {
			        //*mov	= (long)( (pai + (double)maxfanangle + 2.0 * (double)_scancondpar.alpha_h) / delta_fai_m  ) +1;  //宇山さんに聞く
                    mov = (uint)(mov + 2 * m_alpha);
			
		        }
		        else									//ﾉｰﾏﾙ、ｵﾌｾｯﾄｽｷｬﾝ
		        {
                    mov = (uint)(mov + 2 * m_alpha);
		        }
	        }

	        //Rev16.30 生ﾃﾞｰﾀﾌｧｲﾙ分割パラメータ設定 10-05-18 by IWASAWA
	        if(_scansel.data_mode == SCAN)
	        {	//スキャン時未使用パラメータだが設定する
		        cob_view = view ;
		        cob_num = 1;
	        }
	        else	//Conebeam時生ﾃﾞｰﾀのﾋﾞｭｰ数はmovになる
	        {
		        if(_scansel.table_rotation == 1){	//連続回転用純生サイズ計算
			        if(_scancondpar.fimage_bit == 0) //8bits
			        {
				        pur_viewsize = sizeof(char) * mainch * (je - js + 1) ;
			        }
			        else  //10bits or 12bits
			        {
				        pur_viewsize = sizeof(ushort) * mainch * (je - js + 1) ;
			        }
		        }
		        else{
			        pur_viewsize = 0;
		        }

		        //生ﾃﾞｰﾀｻｲｽﾞ計算
		        viewsize = sizeof(ushort) * mainch * md ;

		        //分割数は純生と生の大きいほうに合わせて決定する 単位はMB
		        if(viewsize > pur_viewsize){
			        max_viewsize = (double)viewsize/(double)ONE_MEGA;
		        }
		        else{
			        max_viewsize = (double)pur_viewsize/(double)ONE_MEGA;
		        }

		        if((max_viewsize * (double)mov) <= (double)COB_MAX_SIZE)	//生ﾃﾞｰﾀｻｲｽﾞが2GB以下ならば分割しない
		        {
			        //基本ﾋﾞｭｰ数
                    cob_view = (int)mov;

			        //分割数
			        cob_num = 1;
		        }
		        else{
			        //基本ﾋﾞｭｰ数
			        cob_view = (int)((double)COB_MAX_SIZE / max_viewsize);

			        //分割数
			        if((mov % cob_view) == 0){
                        cob_num = (int)(mov / cob_view);
			        }
			        else{
                        cob_num = (int)((mov / cob_view) + 1);
			        }
			        if(cob_num > SCBNUM)
				        return(1457);	//分割数オーバー
		        }
	        }
	
	        return(0) ;
        }
        //Rev99.99 追加 by長野
        public static int GetScan_Divpara_Set(
        long H_SIZE,					//透視画像横サイズ
        long V_SIZE,					//透視画像縦サイズ
        ref float[] SPA,					//スキャン位置を表す１次直線の傾き
        ref float[] SPB,					//スキャン位置を表す１次直線の切片
        ref float[] Delta_Ysw,              //画像上のスライス厚
        ref int vs,						//積算除算処理開始y座標
        ref int ve,						//積算除算処理終了y座標
        long delta_Jp,					//縦方向の歪分				 
        long ms)						//複数同時スライス数 0:1枚 1:3枚 2:5枚
        {
            if (SPA[2] >= 0)
            {
                vs = (int)(-((float)H_SIZE / (float)2.0) * ((SPA[2 - ms])) + ((SPB[2 - ms])) - ((Delta_Ysw[2 - ms]) / (float)2.0) + ((float)V_SIZE / (float)2.0) - (float)2.0 - delta_Jp);	//-2.0は補間のための余裕
                ve = (int)(((float)H_SIZE / (float)2.0 - (float)1.0) * ((SPA[2 + ms])) + ((SPB[2 + ms])) + ((Delta_Ysw[2 + ms]) / (float)2.0) + ((float)V_SIZE / (float)2.0) + (float)2.0 + delta_Jp);  //+2.0は補間のための余裕
            }
            else
            {
                vs = (int)(((float)H_SIZE / (float)2.0 - (float)1.0) * ((SPA[2 - ms])) + ((SPB[2 - ms])) - ((Delta_Ysw[2 - ms]) / (float)2.0) + ((float)V_SIZE / (float)2.0) - (float)2.0 - delta_Jp);	//-2.0は補間のための余裕
                ve = (int)(-((float)H_SIZE / (float)2.0) * ((SPA[2 + ms])) + ((SPB[2 + ms])) + ((Delta_Ysw[2 + ms]) / (float)2.0) + ((float)V_SIZE / (float)2.0) + (float)2.0 + delta_Jp);				//+2.0は補間のための余裕
            }

            if (vs < 0)
            {
                vs = 0;
            }

            if (ve > V_SIZE - 1)
            {
                ve = (int)V_SIZE - 1;
            }

            if (ve <= vs) ve = vs + 1;	//added by 山本 2002-4-11 
            return (0);
        }
    }
}
