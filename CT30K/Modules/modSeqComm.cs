using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

//
using CTAPI;
using CT30K.Common;
using TransImage;

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： modSeqComm.bas                                              */
    ///* 処理概要　　： SeqComm.dll関連                                             */
    ///* 注意事項　　： なし                                                        */
    ///* -------------------------------------------------------------------------- */
    ///* 適用計算機　： DOS/V PC                                                    */
    ///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
    ///* コンパイラ　： VB 6.0                                                      */
    ///* -------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
    ///*                                                                            */
    ///* V3.0        00/08/03    (TOSFEC) 鈴山　修   ｺｰﾝﾋﾞｰﾑCT対応(新規作成)        */
    ///* V4.0        01/01/23    (ITC)    鈴山　修   SeqCom.dllの改造対応           */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */

	internal static class modSeqComm
	{

//v11.2デバッグ時は仮想シーケンサとする by 間々田 2005/10/19
#if DebugOn
		public static frmVirtualSeqComm MySeq;			//ｼｰｹﾝｻ通信      'V9.7 名称変更 clsSeqComm→MySeq by 間々田 2004/11/11
#else
        public static SeqComm.Seq MySeq;
#endif

        //シーケンサ動作停止要求フラグ           'v15.0追加 by 間々田 2009/05/14
		static bool myRequestForStop;


        //********************************************************************************
        //機    能  ：  BitWriteメソッドを呼び出す
        //              変数名           [I/O] 型        内容
        //引    数  ：  iDeviceName      [I/ ] String    ﾃﾞﾊﾞｲｽ名
        //              iData            [I/ ] Boolean   ﾃﾞｰﾀ
        //              ErrMsgOn         [I/ ] Boolean   ｴﾗｰ表示(True:する,False:しない)
        //戻 り 値  ：                   [ /O] Boolean   結果(True:成功,False:失敗)
        //補    足  ：
        //
        //履    歴  ：  V4.0   01/01/23  (SI1)鈴山       新規作成
        //********************************************************************************
		public static bool SeqBitWrite(string iDeviceName, bool iData, bool ErrMsgOn = true)
		{
			int ReturnValue = 0;

			//戻り値初期化
			bool functionReturnValue = false;

			//シーケンサ通信オブジェクトが存在しない場合、メソッドを呼び出さない     v9.7追加 by 間々田 2004/11/10
            if (MySeq == null) return functionReturnValue;

            //    On Error Resume Next        'added by 山本　2004-8-17                  v9.7削除 by 間々田 2004/11/10

            Debug.Print("BitWrite__" + iDeviceName);
            //BitWriteメソッドを呼び出す
			ReturnValue = MySeq.BitWrite(iDeviceName, iData);

            if (ReturnValue == 0)
            {
                functionReturnValue = true;
            }
            else if (ErrMsgOn)
            {
                SeqcommErrors(ReturnValue);
            }

			//タッチパネル操作の禁止・許可状態に連動してPIOチェックを行なう    'v11.5 追加 by 間々田 2006/06/23
            if (iDeviceName == "PanelInhibit")
            {
				frmMechaControl.Instance.tmrPIOCheck.Enabled = !iData;
            }
			return functionReturnValue;
		}


        //********************************************************************************
        //機    能  ：  WordWriteメソッドを呼び出す
        //              変数名           [I/O] 型        内容
        //引    数  ：  iDeviceName      [I/ ] String    ﾃﾞﾊﾞｲｽ名
        //              iData            [I/ ] String    ﾃﾞｰﾀ
        //              ErrMsgOn         [I/ ] Boolean   ｴﾗｰ表示(True:する,False:しない)
        //戻 り 値  ：                   [ /O] Boolean   結果(True:成功,False:失敗)
        //補    足  ：
        //
        //履    歴  ：  V4.0   01/01/23  (SI1)鈴山       新規作成
        //********************************************************************************
		public static bool SeqWordWrite(string iDeviceName, string iData, bool ErrMsgOn = true)
		{
			int ReturnValue = 0;

			//戻り値初期化
			bool functionReturnValue = false;

			//シーケンサ通信オブジェクトが存在しない場合、メソッドを呼び出さない     v9.7追加 by 間々田 2004/11/10
            if (MySeq == null) return functionReturnValue;

            //    On Error Resume Next        'added by 山本　2004-8-17                  v9.7削除 by 間々田 2004/11/10
            
            //Debug.Print( "WordWrite__" +iDeviceName);
            //WordWriteメソッドを呼び出す
			ReturnValue = MySeq.WordWrite(iDeviceName,ref iData);

            if (ReturnValue == 0)
            {
                functionReturnValue = true;
            }
            else if (ErrMsgOn)
            {
                SeqcommErrors(ReturnValue);
            }
			return functionReturnValue;
		}


        //'********************************************************************************
        //'機    能  ：  StatusReadメソッドを呼び出す
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  ErrMsgOn         [I/ ] Boolean   ｴﾗｰ表示(True:する,False:しない)
        //'戻 り 値  ：                   [ /O] Boolean   結果(True:成功, False:失敗)
        //'補    足  ：
        //'
        //'履    歴  ：  V4.0   01/01/23  (SI1)鈴山       新規作成
        //'********************************************************************************
        //Public Function SeqStatusRead(Optional ByVal ErrMsgOn As Boolean = False) As Boolean
        //
        //    Dim ReturnValue As Long
        //
        //    '戻り値初期化
        //    SeqStatusRead = False
        //
        //    'シーケンサ通信オブジェクトが存在しない場合、メソッドを呼び出さない     v9.7追加 by 間々田 2004/11/10
        //    If MySeq Is Nothing Then Exit Function
        //
        //    'StatusReadメソッドを呼び出す
        //    ReturnValue = MySeq.StatusRead
        //
        //    If ReturnValue = 0 Then
        //        SeqStatusRead = True
        //    ElseIf ErrMsgOn Then
        //        Call SeqcommErrors(ReturnValue)
        //    End If
        //
        //End Function

#region //v15.0削除ここから by 間々田 2009/04/10
        //'********************************************************************************
        //'機    能  ：  FID/FCDの取得
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  oFID             [ /O] Single    FID
        //'              oFCD             [ /O] Single    FCD
        //'戻 り 値  ：                   [ /O] Boolean   結果(True:成功,False:失敗)
        //'補    足  ：
        //'
        //'履    歴  ：  v10.0　05/02/14  (SI4)間々田     新規作成
        //'********************************************************************************
        //Public Sub GetFIDFCD(oFID As Single, oFCD As Single, Optional ByVal WithOffset As Boolean = False)
        //
        //    'StatusReadメソッドを呼び出す（FID/FCD通信が可能な場合） by 間々田 2004/04/26
        //    If SeqStatusRead() Then
        //
        //        oFID = CSng(Format$(MySeq.stsFID / 10, "0.0"))
        //
        //        'Ｘ線管回転選択時
        //        If (scansel.rotate_select = 1) And (scaninh.rotate_select = 0) Then                          'v10.0変更 by 間々田 2005/02/08
        //#If v9Seq Then
        //            oFCD = CSng(Format$(MySeq.stsXrayFCD / 100, "0.0"))     'change 10→100 by 山本 2004-6-2
        //#Else
        //            oFCD = CSng(Format$(MySeq.stsFCD / 10, "0.0"))
        //#End If
        //        Else
        //            oFCD = CSng(Format$(MySeq.stsFCD / 10, "0.0"))
        //        End If
        //    Else
        //        'FID
        //        '    構造体名：scansel
        //        '    コモン名：fid
        //        oFCD = scansel.Fid - MyScansel.FidOffset
        //
        //        'FCD
        //        '    構造体名：scansel
        //        '    コモン名：fcd
        //        oFCD = scansel.FCD - MyScansel.FcdOffset
        //    End If
        //
        //    'オフセット込み？
        //    If WithOffset Then
        //        oFID = oFID + MyScansel.FidOffset
        //        oFCD = oFCD + MyScansel.FcdOffset
        //    End If
        //
        //End Sub
        //v15.0削除ここまで by 間々田 2009/04/10
#endregion

#region //v11.5削除ここから by 間々田 2006/06/12
        //'********************************************************************************
        //'機    能  ：  FCDの読み込み
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  FCD              [ /O] LONG      FCD×10(mm)
        //'戻 り 値  ：                   [ /O] Boolean   結果(TRUE:成功,FALSE:失敗)
        //'補    足  ：
        //'
        //'履    歴  ：  V6.0   02/09/17  (SI4)間々田       新規作成
        //'              v9.7   04/11/09  (SI4)間々田       関数名変更 Read_FCD→ReadFCD
        //'********************************************************************************
        //Public Function ReadFCD(FCD As Long, Speed As Integer, moving As Boolean) As Boolean
        //
        //    '戻り値初期化
        //    ReadFCD = False
        //
        //    'StatusReadメソッドを呼び出す
        //    If Not SeqStatusRead() Then Exit Function
        //
        //    'シーケンサからメカステータスの読み出し
        //    With MySeq
        //        FCD = .stsFCD
        //        Speed = .stsYSpeed
        //        moving = .stsYForward Or .stsYBackward
        //    End With
        //
        //    '戻り値
        //    ReadFCD = True
        //
        //End Function
        //
        //'********************************************************************************
        //'機    能  ：  X座標の読み込み
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  Xpos             [ /O] Integer   X座標×10(mm)
        //'戻 り 値  ：                   [ /O] Boolean   結果(TRUE:成功,FALSE:失敗)
        //'補    足  ：
        //'
        //'履    歴  ：  V6.0   02/09/18  (SI4)間々田       新規作成
        //'              v9.7   04/11/09  (SI4)間々田     関数名変更 Read_Xpos→ReadXpos
        //'********************************************************************************
        //Public Function ReadXpos(xPos As Integer, Speed As Integer, moving As Boolean) As Boolean
        //
        //    '戻り値初期化
        //    ReadXpos = False
        //
        //    'StatusReadメソッドを呼び出す
        //    If Not SeqStatusRead() Then Exit Function
        //
        //    'シーケンサからメカステータスの読み出し
        //    With MySeq
        //        xPos = .stsXPosition
        //        Speed = .stsXSpeed
        //        moving = .stsXLeft Or .stsXRight
        //    End With
        //
        //    '戻り値
        //    ReadXpos = True
        //
        //End Function
        //
        //'********************************************************************************
        //'機    能  ：  FIDの読み込み
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  FID              [ /O] LONG      FID×10(mm)
        //'戻 り 値  ：                   [ /O] Boolean   結果(TRUE:成功,FALSE:失敗)
        //'補    足  ：
        //'
        //'履    歴  ：  V6.0   02/09/27  (CATS1)山本     新規作成
        //'              v9.7   04/11/09  (SI4)間々田     関数名変更 Read_FID→ReadFID
        //'********************************************************************************
        //Public Function ReadFID(Fid As Long, Speed As Integer, moving As Boolean) As Boolean
        //
        //    '戻り値初期化
        //    ReadFID = False
        //
        //    'StatusReadメソッドを呼び出す
        //    If Not SeqStatusRead() Then Exit Function
        //
        //    'シーケンサからメカステータスの読み出し
        //    With MySeq
        //        Fid = .stsFID
        //        Speed = .stsIISpeed
        //        moving = .stsIIForward Or .stsIIBackward
        //    End With
        //
        //    '戻り値
        //    ReadFID = True
        //
        //End Function
        //v11.5削除ここまで by 間々田 2006/06/12
#endregion


        //********************************************************************************
        //機    能  ：  通信エラー処理
        //              変数名           [I/O] 型        内容
        //引    数  ：  ErrNum           [I/ ] Long      エラー番号
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V4.0   01/01/23  (SI1)鈴山       新規作成
        //              V4.0   01/02/15  (SI1)鈴山       表示方法を変更
        //********************************************************************************
		private static void SeqcommErrors(int ErrNum)
		{
			//メッセージ表示：
			//エラー番号:xxx
			//
			//通信ｴﾗｰが発生しました。
			//強制終了しますか？
			//
            DialogResult _dialogResult = MessageBox.Show(CTResources.LoadResString(StringTable.IDS_ErrorNum)
                                                       + ErrNum.ToString() + "\r\n" + "\r\n" 
                                                       + CTResources.LoadResString(9335) + "\r\n" 
                                                       + CTResources.LoadResString(9902), 
                                                       Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if(_dialogResult == DialogResult.Yes)
            {
				//End

				//下記に変更 by 間々田 2005/01/18 メインフォームをアンロードする（CT30kが起動させたSeqComm.exeなども終了させるため）
				frmCTMenu.Instance.Close();
			}
		}


#region //v11.5削除ここから by 間々田 2006/06/12
        //'********************************************************************************
        //'機    能  ：  通信エラー処理
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  ErrNum           [I/ ] Long      エラー番号
        //'戻 り 値  ：                   [ /O] Integer   MsgBox関数の戻り値
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V4.0   01/03/30  (SI1)鈴山       新規作成
        //'********************************************************************************
        //Public Function SeqcommEventErrors(ByVal ErrNum As Long) As Integer
        //
        //    'メッセージ出力用
        //    Dim strMsg As String    '表示ﾒｯｾｰｼﾞ
        //
        //    On Error Resume Next
        //
        //    'ｴﾗｰﾒｯｾｰｼﾞ
        //    '    Select Case ErrNum
        //    '   700:コミュニケーションエラーが発生しました。
        //    '   701:通信準備接続エラーが発生しました。
        //    '   702:VB通信エラーが発生しました。
        //    '   703:通信タイムアウトエラーが発生しました。
        //    '   704:シーケンサコマンド処理エラーが発生しました。
        //    '   710:読出しエラーが発生しました。
        //    '   715:ビット書込みエラーが発生しました。
        //    '   716:ビット書込みディバイスネームエラーが発生しました。
        //    '   720:ワード書込みエラーが発生しました。
        //    '   721:ワード書込みディバイスネームエラーが発生しました。
        //    '   Other:予想外のエラーが発生しました。
        //
        //    'change by 間々田 2003/07/25 リソース対応
        //    Dim MarkPos As Integer
        //    Select Case ErrNum
        //
        //        Case 700 To 704, 710, 715, 716, 720, 721
        //            strMsg = LoadResString(ErrNum)
        //            '取得したリソース文字列中の@マーク以降は無視する
        //            MarkPos = InStr(strMsg, "@")
        //            If MarkPos > 0 Then strMsg = Left$(strMsg, MarkPos - 1)
        //
        //        Case Else
        //            strMsg = LoadResString(IDS_UnkownError)
        //
        //    End Select
        //
        //    'ｴﾗｰ表示
        //'    SeqcommEventErrors = MsgBox("ｴﾗｰ番号=" & CStr(ErrNum) & vbCrLf & vbCrLf & _
		        ///                                strMsg & vbCrLf & _
		        ///                                "強制終了しますか？", _
		        ///                                vbExclamation + vbYesNo + vbDefaultButton2, _
		        ///                                App.Title)
        //
        //    SeqcommEventErrors = MsgBox(LoadResString(IDS_ErrorNum) & CStr(ErrNum) & vbCrLf & vbCrLf & _
        //'                           strMsg & vbCrLf & _
        //'                           LoadResString(9902), _
        //'                           vbExclamation + vbYesNo + vbDefaultButton2)  'リソース対応 2003/07/31 by 間々田
        //
        //End Function
        //v11.5削除ここまで by 間々田 2006/06/12
#endregion

#region //v11.4削除ここから by 間々田 2006/03/06
        ///'
		///'   シーケンサ通信を止める  by added 間々田 2004/06/08
		///'
		///Public Sub StopSeqComm()
		///
		///    On Error Resume Next        'added by 山本　2004-8-17
		///
		///    If Not (Myseq Is Nothing) Then                                                  'v9.7追加 by 間々田 2004/11/09
		///        'Set frmStatus.UC_SeqComm = Nothing
		///        Set frmMechaReset2.UC_SeqComm = Nothing 'v11.3変更 by 間々田 2006/02/20
		///        Set Myseq = Nothing
		///    End If
		///
		///End Sub
		///
		///'
		///'   シーケンサ通信を開始する  by added 間々田 2004/06/08
		///'
		///Public Sub StartSeqComm()
		///
		///    On Error Resume Next        'added by 山本　2004-8-17
		///
		///    If (scaninh.SeqComm = 0) And (Myseq Is Nothing) Then
		///        Set Myseq = New Seq
		///        'Set frmStatus.UC_SeqComm = Myseq
		///        Set frmMechaReset2.UC_SeqComm = Myseq   'v11.3変更 by 間々田 2006/02/20
		///    End If
		///
		///'表示が遅くなるため削除　by 山本　2004-6-16     'added by 山本　2004-8-19　シーケンサ通信再開後最初のプロパティが正確に取得できないため
		///    'シーケンサステータスの読み込み     added 間々田 2004/06/16
		///    If (scaninh.SeqComm = 0) And (scaninh.pc_freeze = 0) Then
		///        PauseForDoEvents 0.3  '0.3秒待つ
		///        SeqStatusRead
		///    End If
		///
		///End Sub
        //v11.4削除ここまで by 間々田 2006/03/06
#endregion


        //
        //   回転方向を取得      'v9.7関数化 by added 間々田 2004/12/03
        //
        //     戻り値    0:CW
        //               1:CCW
        //
		public static int GetRotateDirection()
		{
			//戻り値
			int functionReturnValue = 0;		//デフォルトは 0（CW）

			//シーケンサオブジェクトが存在しない場合
            if (MySeq == null) return functionReturnValue;

#if !v9Seq
            //シーケンサ通信で取得したX線管回転軸の座標から回転方向を判定
            if ((float)(MySeq.stsXrayRotPos / 10000f) > 360) functionReturnValue = 1;     //CCW
#endif

            return functionReturnValue;	
		}


        //
        //   FCD のチェック  'v9.7追加 by added 間々田 2004/12/10
        //
		public static bool CheckFCD(float theFCD)
		{
            //戻り値初期化
            bool functionReturnValue = true;

            //シーケンサオブジェクトが存在しない場合は何もしない
            if (MySeq == null) return functionReturnValue;

            //Rev25.10 PLCステータスを明示的に更新 //2017/09/11
            //干渉にかかわる処理なので、念のためseqcommのステータス更新
            modSeqComm.MySeq.StatusRead();
            modCT30K.PauseForDoEvents(0.5f);

            //Ｘ線干渉が制限されていて、FCD（FCDオフセット含まず）がfcd_limitより小さい場合
            if ((CTSettings.scaninh.Data.table_restriction == 0) && !(MySeq.stsTableMovePermit))
            {
                //if (theFCD < CTSettings.GVal_FcdLimit)
                //Rev25.10 change by chouno 2017/09/11
                if (theFCD < modSeqComm.GetFCDLimit())
                {
                    //メッセージ表示：
                    //   試料テーブルがＸ線管に近いため、干渉しないことを確認して解除ボタンを押してから再度実行して下さい。
                    MessageBox.Show(CTResources.LoadResString(9491), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    functionReturnValue = false;
                }
            }
            return functionReturnValue;
		}

        //
        //   FCD のチェック2(制限解除関係なく中止する)  //Rev23.10 追加 by長野 2015/11/04
        //
        public static bool CheckFCD2(float theFCD)
        {
            //戻り値初期化
            bool functionReturnValue = true;

            //if (theFCD < CTSettings.GVal_FcdLimit)
            if (theFCD < modSeqComm.GetFCDLimit())
            {
                //メッセージ表示：
                //   試料テーブルがＸ線管に近いため、処理を中止します。
                MessageBox.Show(CTResources.LoadResString(9492), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                functionReturnValue = false;
            }
            return functionReturnValue;
        }


        //
        //   現在のI.I.視野を返します    2005/01/12 by 間々田
        //
		public static int GetIINo()
		{
			//戻り値初期化
			int functionReturnValue = -1;

			//シーケンサオブジェクトが存在しない場合抜ける
            if (MySeq == null) return functionReturnValue;

#region     //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//高速撮影とで場合分け v16.01 追加 by 山影 10-02-17
			//        If IsHSCmode Then
			//
			//            If .stsTVII9 Then
			//                GetIINo = 0
			//            ElseIf .stsTVII6 Then
			//                GetIINo = 1
			//            ElseIf .stsTVII4 Then
			//                GetIINo = 2
			//            End If
			//
			//        Else
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

            //Rev23.40 by長野 2016/04/05 //Rev23.21 高速度カメラの記述を復活 by長野 2016/02/22
            //if (modHighSpeedCamera.IsHSCmode)
            //Rev26.40 change by chouno 2019/02/17
            if (modHighSpeedCamera.IsHSCmode || modHighSpeedCamera.IsDropTestmode)
            {
                if (MySeq.stsTVII9)
                {
                    functionReturnValue = 0;
                }
                else if (MySeq.stsTVII6)
                {
                    functionReturnValue = 1;
                }
                else if (MySeq.stsTVII4)
                {
                    functionReturnValue = 2;
                }
            }
            else
            {
                if (MySeq.stsII9)
                {
                    functionReturnValue = 0;
                }
                else if (MySeq.stsII6)
                {
                    functionReturnValue = 1;
                }
                else if (MySeq.stsII4)
                {
                    functionReturnValue = 2;
                }
            }

#region	    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //        End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion

            //フラットパネルの場合は最大視野とする
            //Rev23.40 by長野 2016/04/05 //Rev23.21 条件変更 by長野 2016/02/23
            if (CTSettings.detectorParam.Use_FlatPanel && modHighSpeedCamera.IsCTmode) functionReturnValue = 0;	//v17.00追加 byやまおか 2010/03/02
			
            return functionReturnValue;
        }


        //
        //   現在の回転大テーブル装着状態を返します   Rev19.51 2014/03/03 by 長野
        //
        public static int GetLargeRotTableSts()
        {
            int functionReturnValue = 0;

            //戻り値初期化
            functionReturnValue = -1;

            //シーケンサオブジェクトが存在しない場合抜ける
            if (MySeq == null)
                return functionReturnValue;

            if (MySeq.stsRotLargeTable == true)
            {
                functionReturnValue = 1;
            }
            else
            {
                functionReturnValue = 0;
            }
            return functionReturnValue;
        }


        //
        //   現在のフィルタ（番号）を返します    2005/11/08 by 間々田
        //
		public static int GetFilterIndex()
		{
			//戻り値初期化
			int functionReturnValue = -1;

			//シーケンサオブジェクトが存在しない場合抜ける
            if (MySeq == null) return functionReturnValue;

            if (MySeq.stsFilter0)
            {
                functionReturnValue = 0;
            }
            else if (MySeq.stsFilter1)
            {
                functionReturnValue = 1;
            }
            else if (MySeq.stsFilter2)
            {
                functionReturnValue = 2;
            }
            else if (MySeq.stsFilter3)
            {
                functionReturnValue = 3;
            }
            else if (MySeq.stsFilter4)
            {
                functionReturnValue = 4;
            }
            else if (MySeq.stsFilter5)
            {
                functionReturnValue = 5;
            }
            else if (MySeq.stsShutter)      //v18.00追加 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            {
				functionReturnValue = 5;    //シャッターは5番目
			}
			return functionReturnValue;
		}

        //
        //   セットしたいフィルタ厚のインデックスをを返します   Rev23.40 2016/06/19 by 長野
        //
        public static int SetFilterIndex(float TargetThickness)
        {
            int Index = -1;

            float[] filterThickness = new float[6];

            filterThickness[0] = -9999;
            float.TryParse(CTSettings.infdef.Data.xfilter_c[1].GetString().Replace("mm", ""), out filterThickness[1]);
            float.TryParse(CTSettings.infdef.Data.xfilter_c[2].GetString().Replace("mm", ""), out filterThickness[2]);
            float.TryParse(CTSettings.infdef.Data.xfilter_c[3].GetString().Replace("mm", ""), out filterThickness[3]);
            float.TryParse(CTSettings.infdef.Data.xfilter_c[4].GetString().Replace("mm", ""), out filterThickness[4]);
            filterThickness[5] = 9999;

            if (TargetThickness == filterThickness[0])
            {
                Index = 0;
            }
            else if (TargetThickness == filterThickness[1])
            {
                Index = 1;
            }
            else if (TargetThickness == filterThickness[2])
            {
                Index = 2;
            }
            else if (TargetThickness == filterThickness[3])
            {
                Index = 3;
            }
            else if (TargetThickness == filterThickness[4])
            {
                Index = 4;
            }
            else if (TargetThickness == filterThickness[5])
            {
                Index = 5;
            }

            return Index;
        }



        //
        //   I.I.（またはFPD）電源がＯＮかどうかチェックし、
        //   オフの場合は電源をオンにするかどうか尋ねる
        //                                                       2003/11/06 by 間々田
        //
		public static bool PowerSupplyOK()
		{
			//戻り値初期化
			bool functionReturnValue = false;

			//シーケンサオブジェクトが存在しない場合
            if (MySeq == null)
            {
                functionReturnValue = true;
            }
            //変更2014/10/07hata_v19.51反映
            //電源がすでにオンの場合
            //ElseIf MySeq.stsIIPower Then
            //v17.20 検出器切替のため条件式を変更 by 長野 2010-09-03
            else if (CTSettings.SecondDetOn & (mod2ndDetctor.IsDet1mode & MySeq.stsIIPower) | (mod2ndDetctor.IsDet2mode & MySeq.stsTVIIPower))
            {
                functionReturnValue = true;
            }
            else if (MySeq.stsIIPower)
            {
                functionReturnValue = true;

                //電源がオフの場合：I.I.電源（またはFPD電源）をＯＮしますか？
                //ElseIf MsgBox(GetResString(12289, GetResString(IDS_PowerSupply, GStrIIOrFPD)), vbExclamation + vbOKCancel) = vbOK Then
                //
                //    'I.I.電源をＯＮにする
                //     PowerSupplyOK = SeqBitWrite("IIPowerOn", True)
                //
                //     'FPDの場合、電源をONしてからしばらくたってからでないと画像取込みがエラーするため、0.2秒待つ
                //     If Use_FlatPanel Then PauseForDoEvents 0.2
                //v16.03/v16.20変更(ここから) byやまおか 2010/03/03
            }
            else
            {
                //「ウォームアップ未完了」で「Ｘ線オンボタンでウォームアップ開始するタイプ」の場合
                //If (XrayControl.Up_Wrest_Mode > 0) And ((XrayType = XrayTypeHamaL8601) Or (XrayType = XrayTypeHamaL9421) Or (XrayType = XrayTypeHamaL9191)) Then
                //If (XrayWarmUp = XrayWarmUpNotComplete) And ((XrayType = XrayTypeHamaL8601) Or (XrayType = XrayTypeHamaL9421) Or (XrayType = XrayTypeHamaL9191) Or (XrayType = XrayTypeToshibaEXM2_150)) Then     'v16.20修正 byやまおか 2010/04/06
                //If (XrayWarmUp = XrayWarmUpNotComplete) And ((XrayType = XrayTypeHamaL8601) Or (XrayType = XrayTypeHamaL9421) Or (XrayType = XrayTypeHamaL9191) Or (XrayType = XrayTypeToshibaEXM2_150) Or (XrayType = XrayTypeHamaL9421_02T)) Then 'v16.30 02T追加 byやまおか 2010/05/21
                if ((modXrayControl.XrayWarmUp() == modXrayControl.XrayWarmUpConstants.XrayWarmUpNotComplete)
                    && ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL8601) ||
                        (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL9421) ||
                        (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL9191) ||
                        (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150) ||
                        (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T) ||
                        (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02)))    //v17.71 追加 by長野 2012/03/14
                {
                    //I.I.(またはFPD)電源の確認を表示しない
                    return functionReturnValue;
                }
                else
                {
                    //I.I.電源（またはFPD電源）をＯＮしますか？
                    DialogResult _dialogResult = MessageBox.Show(StringTable.GetResString(12289, StringTable.GetResString(StringTable.IDS_PowerSupply, CTSettings.GStrIIOrFPD)),
                                                                 Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    if (_dialogResult == DialogResult.OK)
                    {
                        //変更2014/10/07hata_v19.51反映
                        //v17.20 検出器切替用に条件式を追加 by 長野 10/09/06
                        if (CTSettings.SecondDetOn)
                        {
                            if (mod2ndDetctor.IsDet1mode)
                            {
                                functionReturnValue = SeqBitWrite("IIPowerOn", true);
                            }
                            else if (mod2ndDetctor.IsDet2mode)
                            {
                                functionReturnValue = SeqBitWrite("TVIIPowerOn", true);
                            }
                        }

                        //I.I.電源をＯＮにする
                        functionReturnValue = SeqBitWrite("IIPowerOn", true);

                        //FPDの場合、電源をONしてからしばらくたってからでないと画像取込みがエラーするため、0.2秒待つ
                        if (CTSettings.detectorParam.Use_FlatPanel) modCT30K.PauseForDoEvents(0.2F);
                    }
                }
            }	    //v16.03/v16.20変更(ここまで) byやまおか 2010/03/03
            
            return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： 指定FCDに移動する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： FCD             [I/ ] Long      FCDの指定位置(×10mm)
        //                           [I/ ] Long      移動速度(×10mm)
        //戻 り 値：                 [ /O] Boolean   結果(True:正常,False:ﾀｲﾑｱｳﾄ)
        //
        //補　　足： なし
        //
        //履　　歴： v6.0   02/09/18  (SI4)間々田    新規作成
        //           v11.5　06/06/12  (WEB)間々田    ScanCorrect.bas から移動
        //*******************************************************************************
		//public static bool MoveFCD(int FCD)
		public static bool MoveFCD(int FCD,bool inLimitFCD = false)//Rev26.10 移動先が限界FCD内true,外false
        {
            DateTime StartTime;			//開始時間(秒)   'v14.14変更 Single→Variant by 間々田 2008/02/20
            double PauseTime = 0;		//待ち時間(秒)   'v14.14変更 Single→Variant by 間々田 2008/02/20

			//初期値設定
			bool functionReturnValue = false;

			//シーケンサ動作停止要求フラグリセット           'v15.0追加 by 間々田 2009/05/14
			myRequestForStop = false;

			//現在位置と指定位置が同じ場合は関数を抜ける  'added by 山本 2002-9-24
            if (MySeq.stsFCD == FCD)
            {
				functionReturnValue = true;
				return functionReturnValue;
			}

			//タイムアウトの設定（10秒余計にとる）：指定位置移動命令では最高速度で移動する
			//PauseTime = Abs(.stsFCD - FCD) / .stsYMaxSpeed + 10
            //Rev26.10 条件追加 by chouno 2018/01/18
            if (inLimitFCD == false)
            {
                PauseTime = Math.Abs(MySeq.stsFCD - FCD) / (double)MySeq.stsYSpeed + 10;		//指定速度に変更された by 間々田 2009/06/12
            }
            else
            {
                PauseTime = Math.Abs(MySeq.stsFCD - FCD) / (double)MySeq.stsFcdDecelerationSpeed + 10;		//指定速度に変更された by 間々田 2009/06/12
            }
			//移動FCD座標送信
            if (!SeqWordWrite("YIndexPosition", FCD.ToString(), false)) return functionReturnValue;
			Application.DoEvents(); 			//added by 山本　2003-10-23

			//移動実行命令送信
            if (!SeqBitWrite("YIndex", true, false)) return functionReturnValue;
			Application.DoEvents(); 			//added by 山本　2003-10-23

			//開始時間を設定
			//StartTime = Timer
			StartTime = DateTime.Now;   		//v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20

            //Rev23.10 条件追加 by長野 2015/11/03
            if (CTSettings.scaninh.Data.cm_mode == 1)
            {
                //一定時間待つ・・・
                while (!((!(MySeq.stsYForward || MySeq.stsYBackward)) && (MySeq.stsFCD == FCD)))
                {
                    //タイムアウト
                    //If (Timer - StartTime) > PauseTime Then Exit Function
                    if ((DateTime.Now - StartTime).TotalSeconds > PauseTime) return functionReturnValue;	//v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20

                    //1秒おきにチェック
                    modCT30K.PauseForDoEvents(1);

                    //ストップ要求があった場合               'v15.0追加 by 間々田 2009/05/14
                    if (myRequestForStop)
                    {
                        SeqBitWrite("YIndexStop", true, false);
                        break;
                    }
                }
            }
            else if(CTSettings.scaninh.Data.cm_mode == 0)
            {
                //一定時間待つ・・・
                //while (!((!(MySeq.stsYForward || MySeq.stsYBackward)) && (MySeq.stsLinearFCD == FCD)))
                while (!((!(MySeq.stsYForward || MySeq.stsYBackward)) && (Math.Abs(MySeq.stsLinearFCD - FCD) <= 1)))
                {
                    //タイムアウト
                    //If (Timer - StartTime) > PauseTime Then Exit Function
                    if ((DateTime.Now - StartTime).TotalSeconds > PauseTime) return functionReturnValue;	//v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20

                    //1秒おきにチェック
                    modCT30K.PauseForDoEvents(1);

                    //ストップ要求があった場合               'v15.0追加 by 間々田 2009/05/14
                    if (myRequestForStop)
                    {
                        SeqBitWrite("YIndexStop", true, false);
                        break;
                    }
                }
            }
			//移動成功
			functionReturnValue = true;
            
			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： 指定X座標に移動する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： FCD             [I/ ] Long      Ｘ軸座標の指定位置(×100mm)
        //戻 り 値：                 [ /O] Boolean   結果(True:正常,False:ﾀｲﾑｱｳﾄ)
        //
        //補　　足： なし
        //
        //履　　歴： v6.0   02/09/18  (SI4)間々田    新規作成
        //           v11.5　06/06/12  (WEB)間々田    ScanCorrect.bas から移動
        //*******************************************************************************
		public static bool MoveXpos(int xPos)
		{
			DateTime StartTime;			//開始時間(秒)   'v14.14変更 Single→Variant by 間々田 2008/02/20
            double PauseTime = 0;		//待ち時間(秒)   'v14.14変更 Single→Variant by 間々田 2008/02/20
			int IntervalTime = 0;		//余裕(秒)   'v17.02追加 byやまおか 2010/07/23

			//初期値設定
			bool functionReturnValue = false;

			//シーケンサ動作停止要求フラグリセット           'v15.0追加 by 間々田 2009/05/14
			myRequestForStop = false;

			//現在位置と指定位置が同じ場合は関数を抜ける  'added by 山本 2002-9-24
            if (MySeq.stsXPosition == xPos)
            {
				functionReturnValue = true;
				return functionReturnValue;
			}

			//タイムアウトの設定（10秒余計にとる）：指定位置移動命令では最高速度で移動する
			//PauseTime = Abs(.stsXPosition - xPos) / (.stsXMaxSpeed * 10) + 10
			//IntervalTime = (frmTransImage.Instance.GetCurrentFR() < 2 ? 15 : 10);			    //v17.02変更 byやまおか 2010/07/23
            IntervalTime = (frmTransImage.Instance.GetCurrentFR() < 2 ? 25 : 20);			    //v17.02変更 byやまおか 2010/07/23
            PauseTime = Math.Abs(MySeq.stsXPosition - xPos) / (double)(MySeq.stsXMaxSpeed * 10) + IntervalTime;	        //v17.02変更 byやまおか 2010/07/23
         
			//移動X座標送信
            if (!SeqWordWrite("XIndexPosition", Convert.ToString(xPos), false)) return functionReturnValue;

			//移動実行命令送信
            if (!SeqBitWrite("XIndex", true, false)) return functionReturnValue;

			//開始時間を設定
			//StartTime = Timer
			StartTime = DateTime.Now;		//v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20

            //Rev23.10 条件追加 by長野 2015/11/03 by長野 2015/11/03
            if (CTSettings.scaninh.Data.cm_mode == 1)
            {
                //一定時間待つ・・・
                while (!((!(MySeq.stsXLeft || MySeq.stsXRight)) && (MySeq.stsXPosition == xPos)))
                {
                    //タイムアウト
                    //If (Timer - StartTime) > PauseTime Then Exit Function
                    if ((DateTime.Now - StartTime).TotalSeconds > PauseTime) return functionReturnValue;	//v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20

                    //1秒おきにチェック
                    modCT30K.PauseForDoEvents(1);

                    //ストップ要求があった場合               'v15.0追加 by 間々田 2009/05/14
                    if (myRequestForStop)
                    {
                        SeqBitWrite("XIndexStop", true, false);
                        break;
                    }
                }
            }
            else if (CTSettings.scaninh.Data.cm_mode == 0)
            {
                //一定時間待つ・・・
                //while (!((!(MySeq.stsXLeft || MySeq.stsXRight)) && (MySeq.stsLinearTableY == xPos)))
                while (!((!(MySeq.stsXLeft || MySeq.stsXRight)) && (Math.Abs(MySeq.stsLinearTableY - xPos) <= 1)))
                {
                    //タイムアウト
                    //If (Timer - StartTime) > PauseTime Then Exit Function
                    if ((DateTime.Now - StartTime).TotalSeconds > PauseTime) return functionReturnValue;	//v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20

                    //1秒おきにチェック
                    modCT30K.PauseForDoEvents(1);

                    //ストップ要求があった場合               'v15.0追加 by 間々田 2009/05/14
                    if (myRequestForStop)
                    {
                        SeqBitWrite("XIndexStop", true, false);
                        break;
                    }
                }
            }
			//移動成功
			functionReturnValue = true;

			return functionReturnValue;
		}

        //*******************************************************************************
        //機　　能： 指定座標にX線・高速度カメラを移動する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： targetPos       [I/ ] Int       座標の指定位置(mm)
        //戻 り 値：                 [ /O] Boolean   結果(True:正常,False:ﾀｲﾑｱｳﾄor異常)
        //
        //補　　足： なし
        //
        //履　　歴： v26.40 19/02/24  (検S1)長野     新規作成
        //*******************************************************************************
        public static bool MoveXrayAndHSC(int targetPos)
        {

            //初期値設定
            bool functionReturnValue = false;

            //現在位置と指定位置が同じ場合は関数を抜ける  'added by 山本 2002-9-24
            if (MySeq.stsXrayCameraUDPosition == targetPos)
            {
                functionReturnValue = true;
                return functionReturnValue;
            }

            //移動X座標送信
            if (!SeqWordWrite("XrayCameraUDIndexPosition", Convert.ToString(targetPos), false)) return functionReturnValue;

            //移動実行命令送信
            if (!SeqBitWrite("XrayCameraUDIndex", true, false)) return functionReturnValue;

            DateTime StartTime = DateTime.Now;
            int PauseTime = CTSettings.iniValue.XrayAndHSCTimeOut;

            //Rev23.10 条件追加 by長野 2015/11/03 by長野 2015/11/03
            if (CTSettings.scaninh.Data.cm_mode == 1)
            {
                //一定時間待つ・・・
                while (!((!(MySeq.stsXrayCameraUDBusy)) && (MySeq.stsXrayCameraUDPosition == targetPos)))
                {
                    //1秒おきにチェック
                    modCT30K.PauseForDoEvents(1);

                    //異常が発生した場合 'Rev26.40 by chouno 2019/02/23
                    if (MySeq.stsXrayCameraUDError == true || ((DateTime.Now - StartTime).TotalSeconds > PauseTime))
                    {
                        SeqBitWrite("XrayCameraUDIndex", false, false);
                        return functionReturnValue;
                    }
                }
            }

            //移動成功
            functionReturnValue = true;

            return functionReturnValue;
        }
        //*******************************************************************************
        //機　　能： 指定X, Y座標に移動する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： xPos            [I/ ] Long      FCD座標の指定位置(×10mm)
        //引　　数： yPos            [I/ ] Long      Y座標（従来のX座標）の指定位置(×100mm)
        //戻 り 値：                 [ /O] Boolean   結果(True:正常,False:ﾀｲﾑｱｳﾄ)
        //
        //補　　足： なし
        //
        //履　　歴： v15.0　06/06/12  (WEB)間々田    新規作成
        //*******************************************************************************
		public static bool MoveXY(int FCD, int yPos)
		{
            DateTime StartTime;			//開始時間(秒)
            double PauseTime = 0;	    //待ち時間(秒)

			//初期値設定
			bool functionReturnValue = false;

			//シーケンサ動作停止要求フラグリセット
			myRequestForStop = false;

			//現在位置と指定位置が同じ場合は関数を抜ける
            if ((MySeq.stsFCD == FCD) & (MySeq.stsXPosition == yPos))
            {
				functionReturnValue = true;
				return functionReturnValue;
			}

            //Rev20.01 移動前にFCD軸,Y軸のセンサを踏んでいた状態の対策 by長野 2015/06/03
            bool oldstsXLLimit = false;
            bool oldstsXRLimit = false;
            bool oldstsYBLimit = false;
            bool oldstsYFLimit = false;

            oldstsXLLimit = MySeq.stsXLLimit;
            oldstsXRLimit = MySeq.stsXRLimit;
            oldstsYBLimit = MySeq.stsYBLimit;
            oldstsYFLimit = MySeq.stsYFLimit;

			//タイムアウトの設定（10秒余計にとる）：FCD指定速度で移動する
            //PauseTime = Math.Abs(MySeq.stsFCD - FCD) / (double)MySeq.stsYSpeed + 10;
            PauseTime = Math.Abs(MySeq.stsFCD - FCD) / (double)MySeq.stsYSpeed + 30; //Rev26.00 change by chouno 2017/11/08

			//移動X座標送信
            if (!SeqWordWrite("XIndexPosition", Convert.ToString(yPos), false)) return functionReturnValue;

			//移動FCD座標送信
            if (!SeqWordWrite("YIndexPosition", Convert.ToString(FCD), false)) return functionReturnValue;
			Application.DoEvents();

			//移動実行命令送信
            if (!SeqBitWrite("XYIndex", true, false)) return functionReturnValue;

			//開始時間を設定
			StartTime = DateTime.Now;

            //Rev23.10 条件追加 by長野 2015/11/03
            if (CTSettings.scaninh.Data.cm_mode == 1)
            {
                //一定時間待つ・・・
                while (!((!(MySeq.stsXLeft || MySeq.stsXRight)) && (!(MySeq.stsYForward || MySeq.stsYBackward)) &&
                         (MySeq.stsXPosition == yPos) && (MySeq.stsFCD == FCD)))
                {
                    //タイムアウト
                    if ((DateTime.Now - StartTime).TotalSeconds > PauseTime) return functionReturnValue;

                    //1秒おきにチェック
                    modCT30K.PauseForDoEvents(1);

                    //ストップ要求があった場合               'v15.0追加 by 間々田 2009/05/14
                    if (myRequestForStop)
                    {
                        SeqBitWrite("XIndexStop", true, false);
                        break;
                    }

                    //追加2014/10/07hata_v19.51反映 
                    //リミットに達した場合       'v18.00追加 byやまおか 2011/06/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    if (MySeq.stsXLLimit | MySeq.stsXRLimit | MySeq.stsYBLimit | MySeq.stsYFLimit)
                    {
                        //Rev20.01 追加 by長野 2015/06/03
                        //Rev20.01 移動前と異なるリミットがついていたらエラーとする。
                        //移動前からついていたリミットは前進方向以外は不問とする。
                        if (oldstsXLLimit != MySeq.stsXLLimit || oldstsXRLimit != MySeq.stsXRLimit || oldstsYBLimit != MySeq.stsYBLimit || oldstsYFLimit != MySeq.stsYFLimit)
                        {
                            return functionReturnValue;
                        }
                    }

                }
            }
            else if (CTSettings.scaninh.Data.cm_mode == 0)
            {
                //一定時間待つ・・・
                while (!((!(MySeq.stsXLeft || MySeq.stsXRight)) && (!(MySeq.stsYForward || MySeq.stsYBackward)) &&
                         (MySeq.stsLinearTableY == yPos) && (MySeq.stsLinearFCD == FCD)))
                {
                    //タイムアウト
                    if ((DateTime.Now - StartTime).TotalSeconds > PauseTime) return functionReturnValue;

                    //1秒おきにチェック
                    modCT30K.PauseForDoEvents(1);

                    //ストップ要求があった場合               'v15.0追加 by 間々田 2009/05/14
                    if (myRequestForStop)
                    {
                        SeqBitWrite("XIndexStop", true, false);
                        break;
                    }

                    //追加2014/10/07hata_v19.51反映 
                    //リミットに達した場合       'v18.00追加 byやまおか 2011/06/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
                    if (MySeq.stsXLLimit | MySeq.stsXRLimit | MySeq.stsYBLimit | MySeq.stsYFLimit)
                    {
                        //Rev20.01 追加 by長野 2015/06/03
                        //Rev20.01 移動前と異なるリミットがついていたらエラーとする。
                        //移動前からついていたリミットは前進方向以外は不問とする。
                        if (oldstsXLLimit != MySeq.stsXLLimit || oldstsXRLimit != MySeq.stsXRLimit || oldstsYBLimit != MySeq.stsYBLimit || oldstsYFLimit != MySeq.stsYFLimit)
                        {
                            return functionReturnValue;
                        }
                    }

                }
            }

			//移動成功
			functionReturnValue = true;

			return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： 指定FIDに移動する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： FID             [I/ ] Long      FIDの指定位置(×10mm)
        //戻 り 値：                 [ /O] Boolean   結果(True:正常,False:ﾀｲﾑｱｳﾄ)
        //
        //補　　足： なし
        //
        //履　　歴： v6.0   02/09/27  (CATS1)山本    新規作成
        //           v11.5　06/06/12  (WEB)間々田    ScanCorrect.bas から移動
        //*******************************************************************************
		public static bool MoveFID(int Fid)
		{
			DateTime StartTime;			        //開始時間(秒)   'v14.14変更 Single→Variant by 間々田 2008/02/20
			double PauseTime = 0;			    //待ち時間(秒)   'v14.14変更 Single→Variant by 間々田 2008/02/20

			bool IsTryForward = false;			//前方に移動しようとしているか 'v15.01追加 by 間々田 2009/09/02
			bool IsTryBackward = false;			//後方に移動しようとしているか 'v15.01追加 by 間々田 2009/09/02

			//初期値設定
			bool functionReturnValue = false;

			//シーケンサ動作停止要求フラグリセット           'v15.0追加 by 間々田 2009/05/14
			myRequestForStop = false;

            //IsTryForward = Fid < MySeq.stsFID;			//前方に移動しようとしているか 'v15.01追加 by 間々田 2009/09/02
            //IsTryBackward = Fid > MySeq.stsFID;		    //後方に移動しようとしているか 'v15.01追加 by 間々田 2009/09/02

            //Rev23.10 条件追加 by長野 2015/11/14
            if (CTSettings.scaninh.Data.cm_mode == 0)
            {
                IsTryForward = Fid < MySeq.stsLinearFDD;			//前方に移動しようとしているか 'v15.01追加 by 間々田 2009/09/02
                IsTryBackward = Fid > MySeq.stsLinearFDD;		    //後方に移動しようとしているか 'v15.01追加 by 間々田 2009/09/02

                //現在位置と指定位置が同じ場合は関数を抜ける  'added by 山本 2002-9-24
                //Rev26.14 計測の場合は±1は使用しない by chouno 2018/09/4 
                if (modLibrary.InRange(MySeq.stsLinearFDD, Fid, Fid))
                //if (modLibrary.InRange(MySeq.stsLinearFDD, Fid - 1, Fid + 1))
                {
                    functionReturnValue = true;
                    return functionReturnValue;
                }

                //タイムアウトの設定（10秒余計にとる）：指定位置移動命令では最高速度で移動する
                PauseTime = Math.Abs(MySeq.stsLinearFDD - Fid) / (double)MySeq.stsIIMaxSpeed + 10;
            }
            else
            {
                IsTryForward = Fid < MySeq.stsFID;			//前方に移動しようとしているか 'v15.01追加 by 間々田 2009/09/02
                IsTryBackward = Fid > MySeq.stsFID;		    //後方に移動しようとしているか 'v15.01追加 by 間々田 2009/09/02

                //現在位置と指定位置が同じ場合は関数を抜ける  'added by 山本 2002-9-24
                if (modLibrary.InRange(MySeq.stsFID, Fid - 1, Fid + 1))
                {
                    functionReturnValue = true;
                    return functionReturnValue;
                }

                //タイムアウトの設定（10秒余計にとる）：指定位置移動命令では最高速度で移動する
                PauseTime = Math.Abs(MySeq.stsFID - Fid) / (double)MySeq.stsIIMaxSpeed + 10;
            }

            ////タイムアウトの設定（10秒余計にとる）：指定位置移動命令では最高速度で移動する
            //PauseTime = Math.Abs(MySeq.stsFID - Fid) / (double)MySeq.stsIIMaxSpeed + 10;

			//移動X座標送信
            if (!SeqWordWrite("IIindexPosition", Convert.ToString(Fid), false)) return functionReturnValue;
			Application.DoEvents();			//added by 山本　2003-10-23

			//移動実行命令送信
            if (!SeqBitWrite("IIindex", true, false)) return functionReturnValue;
			Application.DoEvents();			//added by 山本　2003-10-23

			//開始時間を設定
			//StartTime = Timer
			StartTime = DateTime.Now;			//v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20

            //Rev23.10 条件追加 by長野 2015/11/03
            if (CTSettings.scaninh.Data.cm_mode == 1)
            {
                //一定時間待つ・・・
                while (!((!(MySeq.stsIIForward || MySeq.stsIIBackward)) && modLibrary.InRange(MySeq.stsFID, Fid - 1, Fid + 1)))
                {
                    //タイムアウト
                    //If (Timer - StartTime) > PauseTime Then Exit Function
                    if ((DateTime.Now - StartTime).TotalSeconds > PauseTime) return functionReturnValue;	//v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20

                    //リミットに達している                   'v15.01追加 by 間々田 2009/09/02
                    if (IsTryForward && MySeq.stsIIFLimit) return functionReturnValue;
                    if (IsTryBackward && MySeq.stsIIBLimit) return functionReturnValue;

                    //1秒おきにチェック
                    modCT30K.PauseForDoEvents(1);

                    //ストップ要求があった場合               'v15.0追加 by 間々田 2009/05/14
                    if (myRequestForStop)
                    {
                        SeqBitWrite("IIindexStop", true, false);
                        break;
                    }
                }
            }
            else if(CTSettings.scaninh.Data.cm_mode == 0)
            {
                //一定時間待つ・・・
                while (!((!(MySeq.stsIIForward || MySeq.stsIIBackward)) && modLibrary.InRange(MySeq.stsLinearFDD, Fid - 1, Fid + 1)))
                {
                    //タイムアウト
                    //If (Timer - StartTime) > PauseTime Then Exit Function
                    if ((DateTime.Now - StartTime).TotalSeconds > PauseTime) return functionReturnValue;	//v14.14変更 Timer関数を時間計測に使用しない by 間々田 2008/02/20

                    //リミットに達している                   'v15.01追加 by 間々田 2009/09/02
                    if (IsTryForward && MySeq.stsIIFLimit) return functionReturnValue;
                    if (IsTryBackward && MySeq.stsIIBLimit) return functionReturnValue;

                    //1秒おきにチェック
                    modCT30K.PauseForDoEvents(1);

                    //ストップ要求があった場合               'v15.0追加 by 間々田 2009/05/14
                    if (myRequestForStop)
                    {
                        SeqBitWrite("IIindexStop", true, false);
                        break;
                    }
                }
            }
			//移動成功
			functionReturnValue = true;

			return functionReturnValue;
		}


		public static float GetRotMax(bool? IsRotateSelect = null)
		{
			float RotMax = 0;
            RotMax = CTSettings.scancondpar.Data.rot_max_speed;		//テーブルの最大回転速度(rpm)

#if v9Seq
            if (IsRotateSelect == null)
            {
                IsRotateSelect = Convert.ToBoolean(CTSettings.scansel.Data.rotate_select == 1);
            }

			//Ｘ線管回転が選択されている場合、シーケンサ通信にて取得  v9.0 added by 間々田 2004/02/13
            if ((bool)IsRotateSelect && (CTSettings.scaninh.Data.rotate_select == 0) && (CTSettings.scaninh.Data.seqcomm == 0))
            {
                float seqRotMax = 0;
                seqRotMax = MySeq.stsXrayRotMaxSp / 10000F;
                if (seqRotMax > 0) RotMax = seqRotMax;
            }
#endif

            return RotMax;
		}


		public static int GetViewMin(bool IsTableRotate = true)
		{
			int functionReturnValue = 0;

            if (modCT30K.TableRotOn && IsTableRotate)
            {
                //scancondpar.rot_max_speed:テーブルの最大回転速度(rpm)
                //2014/11/13hata キャストの修正
                //functionReturnValue = (int)(((CTSettings.detectorParam.FR[0] * 60 / GetRotMax() - 1) / 100 + 1) * 100);
                functionReturnValue = Convert.ToInt32(Math.Floor((CTSettings.detectorParam.FR[0] * 60 / GetRotMax() - 1) / 100F + 1) * 100);
                
                
                //v19.10 最低ビュー数は600ビューで固定とする by長野 2012/09/08
                if (functionReturnValue <= 600)
                {
                    functionReturnValue = 600;
                }
            }
            else
            {
                functionReturnValue = CTSettings.GVal_ViewMin;
            }
			return functionReturnValue;
		}


		public static bool IsSeqMoving()
		{
			//戻り値初期化
			bool functionReturnValue = false;

            if (MySeq == null) return functionReturnValue;

            functionReturnValue = MySeq.stsXRight ||
                                  MySeq.stsXLeft ||
                                  MySeq.stsYForward ||
                                  MySeq.stsYBackward ||
                                  MySeq.stsIIForward ||
                                  MySeq.stsIIBackward ||
                                  //MySeq.stsCTIIDrive ||
                                  //MySeq.stsTVIIDrive ||       //v16.01 stsCTIIDrive,stsTVIIDriveを追加 by 山影 10-02-05
                                  (MySeq.stsCTIIDrive && (CTSettings.scaninh.Data.cm_mode == 1)) ||       //v23.10 変更 by長野 2015/10/21
                                  (MySeq.stsTVIIDrive && (CTSettings.scaninh.Data.cm_mode == 1)) ||       //v23.10 変更 by長野 2015/10/21
                                  ((CTSettings.scaninh.Data.cm_mode == 0) && (MySeq.stsMicroFPDBusy || MySeq.stsMicroFPDShiftBusy || MySeq.stsNanoFPDBusy || MySeq.stsNanoFPDShiftBusy)) || //Rev23.10 追加 by長野 2015/10/20  
                                  (MySeq.stsFPDLShiftBusy || MySeq.stsFDSystemBusy); //Rev23.20 追加 by長野 2015/12/22
            return functionReturnValue;
		}


        //*******************************************************************************
        //機　　能： 指定位置移動時のシーケンサ動作停止要求
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.0 2008/11/25 (SI1)間々田    新規作成
        //*******************************************************************************
		public static void SeqRequestForStop()
		{
			//シーケンサ動作停止要求フラグセット           'v15.0追加 by 間々田 2009/05/14
			myRequestForStop = true;
		}


        //*******************************************************************************
        //機　　能： 指定FCDに移動する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： FCD             [I/ ] Long      FCDの指定位置(×10mm)
        //戻 り 値：                 [ /O] Boolean   結果(True:正常,False:ﾀｲﾑｱｳﾄ)
        //
        //補　　足： なし
        //
        //履　　歴： v15.0 2008/11/25 (SI1)間々田    新規作成
        //        ： v15.0 2009/08/18 (SI1)間々田    廃止
        //*******************************************************************************
        //Public Function MoveFCDSlow(ByVal FCD As Long) As Boolean
        //
        //    Dim OrgYSpeed   As Integer
        //
        //    '初期値設定
        //    MoveFCDSlow = False
        //
        //    'シーケンサ動作停止要求フラグリセット
        //    myRequestForStop = False
        //
        //    With MySeq
        //
        //        '現在位置と指定位置が同じ場合は関数を抜ける
        //        If .stsFCD = FCD Then
        //            MoveFCDSlow = True
        //            Exit Function
        //        End If
        //
        //        '現在のスピードのバックアップ
        //        OrgYSpeed = .stsYSpeed
        //
        //        '最小スピードの設定
        //        'SeqWordWrite "YSpeed", Format$(.stsYMinSpeed, "0")
        //        SeqWordWrite "YSpeed", Format$(10, "0")
        //
        //        MoveFCDSlow = MoveFCD(FCD)
        //
        //'        'シーケンサに動作オン指令を送る
        //'        SeqBitWrite "YForward", True
        //'
        //'        Do While (FCD < .stsFCD) And (Not myRequestForStop)
        //'            'SeqBitWrite "CommCheck", True, False
        //'            PauseForDoEvents 1
        //'            .StatusRead
        //'        Loop
        //'
        //'        'シーケンサに動作オン指令を送る
        //'        SeqBitWrite "YForward", False
        //
        //        'スピードを元に戻す
        //        SeqWordWrite "YSpeed", Format$(OrgYSpeed, "0")
        //
        //    End With
        //
        //    '移動成功
        //    MoveFCDSlow = True
        //
        //End Function
        
#region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
#endregion
        //*******************************************************************************
        //機　　能： X線検出器の切替
        //
        //           変数名          [I/O] 型        内容
        //引　　数： theCommand      [I/ ] String    コマンド
        //戻 り 値：                 [ /O] Boolean   True:移動成功   False:失敗
        //
        //補　　足： なし
        //
        //履　　歴： v16.01 2010/02/12 (検SS)山影   新規作成
        //*******************************************************************************
        public static bool ChangeII(string theCommand)
        {
            bool functionReturnValue = false;

            const int thePauseTime = 30;        //I.I.切替移動タイムアウト時間(秒)
            DateTime StartTime;                 //開始時間(秒)
            int PauseTime = 0;                  //待ち時間(秒)

            //初期値設定
            functionReturnValue = false;

            switch (theCommand)
            {
                case "CTIISet":
                case "TVIISet":
                case "IIChangeReset":
                    break;
                default:
                    return functionReturnValue;
            }

            //シーケンサ動作停止要求フラグリセット
            myRequestForStop = false;

            //タイムアウトの設定。時間は実機で調整。
            PauseTime = thePauseTime;

            //I.I.切替命令送信
            if (!SeqBitWrite(theCommand, true, false))
                return functionReturnValue;

            //開始時間を設定
            StartTime = DateTime.Now;

            //X線検出器切替動作中
            while (modHighSpeedCamera.IsIIChanging)
            {

                //タイムアウト
                //if (DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Second, StartTime, DateAndTime.Now) > PauseTime)
                if ((DateTime.Now - StartTime).TotalSeconds > (double)PauseTime)
                    return functionReturnValue;

                //指定位置に達している
                if (modHighSpeedCamera.IsOKIIPos)
                    break; // TODO: might not be correct. Was : Exit Do

                //0.5秒おきにチェック 仮
                modCT30K.PauseForDoEvents(0.5F);

                //ストップ要求があった場合
                if (myRequestForStop)
                {
                    //I.I.切替強制停止
                    SeqBitWrite("IIChangeStop", true, false);
                    return functionReturnValue;
                }
            }

            //移動成功
            functionReturnValue = true;
            return functionReturnValue;

        }


        //*******************************************************************************
        //機　　能： X線検出器の切替（2nd検出器との切替）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： theCommand      [I/ ] String    コマンド
        //
        //戻 り 値：                 [ /O] Boolean   True:移動成功   False:失敗
        //
        //補　　足： なし
        //
        //履　　歴： v17.20 2010/08/31 (検S1)長野   新規作成
        //*******************************************************************************
        //Public Function ChangeDet(ByVal theCommand As String) As Boolean
        //
        //    '関係の無い文字列が入ってきたら処理を終了する by 長野 2010/09/06
        //    Select Case theCommand
        //        Case "CTIISet", "TVIISet"
        //        Case Else:  Exit Function
        //    End Select
        //
        //    'I.I.切替命令送信
        //    If Not SeqBitWrite(theCommand, True, False) Then Exit Function
        //
        //    Const thePauseTime As Long = 30 'I.I.切替移動タイムアウト時間(秒)
        //
        //    Dim StartTime As Variant    '開始時間(秒)
        //    Dim PauseTime As Variant    '待ち時間(秒)
        //    Dim ChangeComFlg As Boolean 'コモン変更フラグ
        //    Dim ChangingDet As Boolean
        //    'タイムアウトの設定。時間は実機で調整。
        //    PauseTime = thePauseTime
        //
        //    '初期値設定
        //    ChangeDet = False
        //
        //    '開始時間を設定
        //    StartTime = Now
        //
        //    'シーケンサ動作停止要求フラグリセット
        //    myRequestForStop = False
        //
        //    'シーケンサのステータスが変更待ちで１秒待つ
        //    PauseForDoEvents 1
        //
        //    'X線検出器切替動作中
        //    'Do While IsSwtichingDet
        //    Do While (DateDiff("s", StartTime, Now) < PauseTime)
        //
        //        '1秒おきにチェック
        //        PauseForDoEvents 1
        //
        //        'タイムアウト
        //        If DateDiff("s", StartTime, Now) > PauseTime Then Exit Function
        //
        //        '指定位置に達している
        //        If IsOKDetPos Then Exit Do
        //
        //        'ストップ要求があった場合
        //        If myRequestForStop Then
        //
        //            'I.I.切替強制停止
        //            Call SeqBitWrite("IIChangeStop", True, False)
        //            Exit Function
        //
        //        End If
        //
        //    Loop
        //
        //    '移動成功
        //    ChangeDet = True
        //
        //End Function

        //v23.10 追加 X線の切替 2015/09/20
        public static bool ChangeXray(mod2ndXray.XrayModeConstants theNewMode)
        {
            bool functionReturnValue = false;

            //シーケンサに送信するコマンド
            string theCommand = null;
            string theCommand2 = null;

            //戻り値初期化
            functionReturnValue = false;

            //X線切替移動可否判定
            if (!mod2ndXray.IsChangeXray())
                return functionReturnValue;

            //X線命令送信
            //theCommand = (theMode == mod2ndXray.XrayModeConstants.XrayMode_Xray1 ? "CTIISet" : "TVIISet");
            if (theNewMode == mod2ndXray.XrayModeConstants.XrayMode_Xray2)
            {
                theCommand = "NanoFPDSet";
                theCommand2 = "NanoTableYSet";
                //frmMechaControl.Instance.cwbtnChangeXray[0].BackColor = Color.Green;
                //frmMechaControl.Instance.cwbtnChangeXray[1].BlinkInterval = CWSpeeds.cwSpeedFast;
                //frmMechaControl.Instance.cwbtnChangeXray[1].Value = true;

            }
            else if (theNewMode == mod2ndXray.XrayModeConstants.XrayMode_Xray1)
            {
                theCommand = "MicroFPDSet";
                theCommand2 = "MicroTableYSet";
                //frmMechaControl.Instance.cwbtnChangeXray[1].BackColor = Color.Green;
                //frmMechaControl.Instance.cwbtnChangeXray[0].BlinkInterval = CWSpeeds.cwSpeedFast;
                //frmMechaControl.Instance.cwbtnChangeXray[0].Value = true;
            }
            else
            {
                return functionReturnValue;
            }

         
            if (!SeqBitWrite(theCommand, true, false))
                return functionReturnValue;
            Application.DoEvents();

            if (!SeqBitWrite(theCommand2, true, false))
                return functionReturnValue;
            Application.DoEvents();


            //検出器切替移動タイムアウト時間(秒)
            const short PauseTime = 30;

            //開始時間
            //StartTime = DateAndTime.Now;
            DateTime StartTime = DateTime.Now;

            //シーケンサ動作停止要求フラグリセット
            myRequestForStop = false;

            modCT30K.PauseForDoEvents(1);

            //指定位置に達しているまでループ
            while ((!mod2ndXray.IsOKXrayPos))
            {
                //1秒おきにチェック
                modCT30K.PauseForDoEvents(1);

                //タイムアウト
                //if (DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Second, StartTime, DateAndTime.Now) > PauseTime)
                if ((DateTime.Now - StartTime).TotalSeconds > (double)PauseTime)
                    return functionReturnValue;

                //ストップ要求があった場合
                if (myRequestForStop)
                {
                    //I.I.切替強制停止
                    SeqBitWrite("FPDMoveStop", true, false);
                    return functionReturnValue;
                }
            }

            //X線切替成功
            functionReturnValue = true;
            return functionReturnValue;

        }

        //追加2014/10/07hata_v19.51反映 
        //v17.50変更 mod2ndDetctorから移動。上記ChangeDetと統合した by 間々田 2011/03/17
        public static bool SwitchDet(mod2ndDetctor.DetModeConstants theMode)
        {
            bool functionReturnValue = false;
            //v19.50 元に戻す by長野 2013/12/17
            //Public Function SwitchDet(ByVal theMode As String, Optional ByVal theSetGainFlg As Boolean = UNSET_GAIN) As Boolean 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

            //シーケンサに送信するコマンド
            string theCommand = null;

            //戻り値初期化
            functionReturnValue = false;

            //ゲインデータのファイル名 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //v19.50 コメントアウト by長野 2013/12/17
            //    Dim gaincor_fn  As String   'v18.00追加 byやまおか 2011/02/12
            //    Dim ret         As Long     'v18.00追加 byやまおか 2011/02/12

            //v19.50 コメントアウト by長野 2013/12/17
            //シーケンサ動作停止要求フラグリセット   'v18.00追加 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //myRequestForStop = False

            //検出器移動可否判定
            if (!mod2ndDetctor.IsSwitchDet())
                return functionReturnValue;
            
            //撮影位置から外れる検出器電源をOFF(フラットパネルの場合、電源をOFFにしない)
            if (!CTSettings.detectorParam.Use_FlatPanel)
            {
                theCommand = (theMode == mod2ndDetctor.DetModeConstants.DetMode_Det1 ? "TVIIPowerOff" : "IIPowerOff");
                SeqBitWrite(theCommand, true);
                System.Windows.Forms.Application.DoEvents();
            }

            //検出器切替命令送信
            theCommand = (theMode == mod2ndDetctor.DetModeConstants.DetMode_Det1 ? "CTIISet" : "TVIISet");
            if (!SeqBitWrite(theCommand, true, false))
                return functionReturnValue;
            Application.DoEvents();

            //検出器切替移動タイムアウト時間(秒)
            const short PauseTime = 30;

            //開始時間
            //StartTime = DateAndTime.Now;
            DateTime StartTime = DateTime.Now;
           
            //シーケンサ動作停止要求フラグリセット
            myRequestForStop = false;

            //指定位置に達しているまでループ
            while ((!mod2ndDetctor.IsOKDetPos))
            {
                //1秒おきにチェック
                modCT30K.PauseForDoEvents(1);

                //タイムアウト
                //if (DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Second, StartTime, DateAndTime.Now) > PauseTime)
                if ((DateTime.Now - StartTime).TotalSeconds > (double)PauseTime)
                    return functionReturnValue;

                //ストップ要求があった場合
                if (myRequestForStop)
                {
                    //I.I.切替強制停止
                    SeqBitWrite("IIChangeStop", true, false);
                    return functionReturnValue;
                }
            }
 
            //v19.50 コメントアウト by長野 2013/12/17
            //    'PkeFPDで検出器シフトの場合ゲインデータをセットする 'v18.00追加 byやまおか 2011/02/27
            //    ''ゲイン校正中とスキャン中はセットしない
            //    'If ((DetType = DetTypePke) And DetShiftOn And (Not Flg_GainCorrecting)) And (Not CBool(CTBusy And CTScanStart)) Then
            //    If (IsSetGainData And theSetGainFlg) Then   '関数化&条件変更    'v18.00変更 byやまおか 2011/07/04
            //
            //#If Not NoCamera Then
            //
            //        'ゲインデータファイル名を取得
            //        Select Case theCommand
            //
            //            '基準位置に移動した場合
            //            Case "CTIISet", "IIChangeReset"
            //                ret = PkeSetGainData(hPke, Gain_Image_L(0), 1, GAIN_CORRECT_L)
            //            'シフト位置に移動した場合
            //            Case "TVIISet"
            //                ret = PkeSetGainData(hPke, Gain_Image_L_SFT(0), 1, GAIN_CORRECT_L_SFT)
            //
            //        End Select
            //
            //        'セット失敗した場合
            //        If ret = 1 Then MsgBox "ゲイン校正データをセットできませんでした。", vbCritical
            //
            //#End If
            //
            //    End If

            //検出器切替成功
            functionReturnValue = true;
            return functionReturnValue;

        }
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここから
        //********************************************************************************
        //機    能  ：  ゲインデータをセットするか
        //              変数名           [I/O] 型        内容
        //引    数  ：
        //
        //戻 り 値  ：                   [ /O] Boolean   結果(True:正常,False:異常)
        //補    足  ：  なし
        //
        //履    歴  ：  V18.00   11/07/03    やまおか    新規作成
        //********************************************************************************
        public static bool IsSetGainData()
        {
            bool functionReturnValue = false;

            //初期化
            functionReturnValue = false;

            //PkiFPDでシフトスキャンあり
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            //if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) & CTSettings.DetShiftOn)
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) & (CTSettings.DetShiftOn || CTSettings.W_ScanOn))
            {
                functionReturnValue = true;
            }
            return functionReturnValue;

        }
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここまで

        //********************************************************************************
        //機    能  ：  2世代・3世代兼用システムの場合、検出器システムが所定の位置にいない場合は、機能を制限
        //              変数名           [I/O] 型        内容
        //引    数  ：
        //
        //戻 り 値  ：                   [ /O] Boolean   結果(True:正常,False:異常)
        //補    足  ：  なし
        //
        //履    歴  ：  V23.20   15/12/21    長野    新規作成
        //********************************************************************************
        public static void ct_gene2and3LimitControl(bool Value)
        {
            frmCTMenu.Instance.Toolbar1.Items[1].Enabled = Value;			//X線
            frmCTMenu.Instance.Toolbar1.Items[5].Enabled = Value;			//I.I.視野切替
            frmCTMenu.Instance.Toolbar1.Items[5].Visible = Value;			//I.I.視野切替
            frmCTMenu.Instance.Toolbar1.Items[9].Enabled = Value;			//カメラ電源
            frmCTMenu.Instance.Toolbar1.Items[9].Visible = Value;			//カメラ電源

            //frmXrayControl.Instance.Enabled = Value;
            //frmXrayControl.Instance.cmdDetail.Enabled = Value;

            frmScanControl.Instance.SSTab1.Enabled = Value;
      
            frmScanControl.Instance.ctbtnScanStart.Enabled = Value;
            frmScanControl.Instance.ctbtnScanStop.Enabled = Value;

            frmCTMenu.Instance.tsbtnLiveImage.Enabled = Value;
            frmCTMenu.Instance.tsbtnXray.Enabled = Value;

            frmXrayControl.Instance.cmdWarmupStart.Enabled = Value;
        }
        //*******************************************************************************
        //機　　能： 電動フィルタの切替
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [I/ ] int       フィルタのインデックス
        //戻 り 値：                 [ /O] Boolean   True:移動成功   False:失敗
        //
        //補　　足： なし
        //
        //履　　歴： v23.40 2016/06/19 (検S1)長野   新規作成
        //*******************************************************************************
        public static bool ChangeFilter(int Index)
        {
            bool functionReturnValue = false;

            const int thePauseTime = 60;        //切替移動タイムアウト時間(秒)
            DateTime StartTime;                 //開始時間(秒)
            int PauseTime = 0;                  //待ち時間(秒)

            //初期値設定
            functionReturnValue = false;

            //シーケンサ動作停止要求フラグリセット
            myRequestForStop = false;

            //タイムアウトの設定。時間は実機で調整。
            PauseTime = thePauseTime;

            //フィルタを設定
            //SeqBitWrite cmdFilter(Index).tag, True
            //tagを使わないようにした    'v18.00変更 byやまおか 2011/02/12
            string command_Renamed = null;
            command_Renamed = "Filter" + Convert.ToString(Index);
            //Titanの場合はフィルタの5番目をシャッターにする 'v18.00変更 byやまおか 2011/02/20
            //Rev25.03/Rev25.02 change by chouno 2017/02/05
            if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeGeTitan || modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeSpellman) & (Index == 5))
                command_Renamed = "Shutter";

            modSeqComm.SeqBitWrite(command_Renamed, true);

            modCT30K.PauseForDoEvents(1);

            //開始時間を設定
            StartTime = DateTime.Now;

            //while (modSeqComm.MySeq.stsFilter0Run || modSeqComm.MySeq.stsFilter1Run || modSeqComm.MySeq.stsFilter2Run || modSeqComm.MySeq.stsFilter3Run || modSeqComm.MySeq.stsFilter4Run || modSeqComm.MySeq.stsFilter5Run)
            while (Index != modSeqComm.GetFilterIndex())
            {
                //タイムアウト
                //if (DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Second, StartTime, DateAndTime.Now) > PauseTime)
                if ((DateTime.Now - StartTime).TotalSeconds > (double)PauseTime)
                    return functionReturnValue;

                //指定位置に達している
                if (Index == modSeqComm.GetFilterIndex())
                {
                    frmXrayControl.Instance.MyUpdate();
                    break; // TODO: might not be correct. Was : Exit Do
                }

                //0.5秒おきにチェック 仮
                modCT30K.PauseForDoEvents(0.5F);
            }

            //移動成功
            UpdateMechaByFilterIndex(Index);

            functionReturnValue = true;
            return functionReturnValue;

        }
        //*******************************************************************************
        //機　　能： フィルタ位置コモン ステータス変更
        //
        //           変数名          [I/O] 型        内容
        //引　　数： FilterIndex     [I/ ] int       フィルタのインデックス
        //           
        //戻 り 値： 
        //
        //補　　足： なし
        //
        //履　　歴： v23.40 16/06/19  (検S1)長野    新規作成
        //           
        //*******************************************************************************
        public static void UpdateMechaByFilterIndex(int FilterIndex)
        {
            //CTSettings.mecainf.Load();
            //CTSettings.mecainf.Data.xfilter = FilterIndex;
            //CTSettings.mecainf.Write();
            //オートセンタリングスキャンが終了したらオートセンタリングをなしにする
            MecaInf theMecainf = new MecaInf();
            theMecainf.Data.Initialize();
            theMecainf.Load();
            theMecainf.Data.xfilter = FilterIndex;
            theMecainf.Write();
        }
              //*******************************************************************************
        //機　　能： FCD限界値取得
        //
        //           変数名          [I/O] 型        内容
        //引　　数： 
        //           
        //戻 り 値： (float)FCD限界値
        //
        //補　　足： ラージテーブルOn/Offに従い、限界FCD値を変更する
        //
        //履　　歴： v25.10 17/09/11  (CT開)長野    新規作成
        //           
        //*******************************************************************************
        public static float GetFCDLimit()
        {
            float ret = 0.0f;

            //Rev25.10 PLCステータスを明示的に更新 //2017/09/11
            //干渉にかかわる処理なので、念のためseqcommのステータス更新
            modSeqComm.MySeq.StatusRead();
            modCT30K.PauseForDoEvents(0.5f);

            //Rev26.20 回転テーブル装着状態にかかわらずFCDLimitで確認(FCDLimitは大テーブル装着状態のFCDLimitのため) by chouno 2019/02/11
            //if (MySeq.stsRotLargeTable == true)
            //{
            //   ret = CTSettings.GVal_FcdLimit + CTSettings.mechapara.Data.largeTableRingWidth;
            //}
            //else
            //{
                ret = CTSettings.GVal_FcdLimit;
            //}

            return ret;
        }
    }
}
