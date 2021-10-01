using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace CT30K
{
	///* ************************************************************************** */
	///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： modLibrary.bas                                              */
	///* 処理概要　　： 汎用モジュール                                              */
	///* 注意事項　　： なし                                                        */
	///* -------------------------------------------------------------------------- */
	///* 適用計算機　： DOS/V PC                                                    */
	///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
	///* コンパイラ　： VB 6.0                                                      */
	///* -------------------------------------------------------------------------- */
	///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
	///*                                                                            */
	///* V3.0        00/09/20    (TOSFEC) 鈴山　修   ｺｰﾝﾋﾞｰﾑCT対応(新規作成)        */
	///* V19.00      12/02/21    H.Nagai             BHC対応
	///*                                                                            */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
	///* ************************************************************************** */
	internal static class modLibrary
	{

		//点
		[StructLayout(LayoutKind.Sequential)]
		public struct Points
		{
			public short x;
			public short y;
		}


		//********************************************************************************
		//  定数データ宣言
		//********************************************************************************

		//Public Const FILE_ATTRIBUTE_DIRECTORY = &H10        'ファイル属性定数（ﾃﾞｨﾚｸﾄﾘ）   'v17.50削除 2011/02/28 by 間々田

		//Public Const SYNCHRONIZE = &H100000                                                'v17.50削除 2011/02/28 by 間々田
		//Public Const INFINITE = &HFFFF&      '  Infinite timeout                           'v17.50削除 2011/02/28 by 間々田

		//ウィンドウポジション   'V4.0 append by 鈴山 2001/02/14
		public const int HWND_BOTTOM = 1;			//'ウインドウを最後方に
		public const int HWND_TOPMOST = -1;			//'常に最全面のウィンドウに設定
		public const int HWND_NOTOPMOST = -2;		//'最上位ウィンドウの背面に
		public const int HWND_TOP = 0;				//'ウィンドウを一番手前に
		public const int SWP_NOMOVE = 0x2;			//'現在の位置を保つフラグ
		public const int SWP_NOSIZE = 0x1;			//'現在のサイズを保つフラグ
		public const int SWP_FRAMECHANGED = 0x20;	//v15.0追加 by 間々田 2009/01/18
		public const int SWP_HIDEWINDOW = 0x80;		//v15.0追加 by 間々田 2009/01/18
		public const int SWP_SHOWWINDOW = 0x40;

		//整数（Integer）の最大値・最小値    'append by 間々田 2003/07/30
		public const short IntMax = 32767;
		public const short IntMin = -32768;


		//********************************************************************************
		//  外部関数宣言
		//********************************************************************************

		//ファイル属性取得ＡＰＩ関数の宣言
		//Declare Function GetFileAttributes Lib "kernel32" Alias "GetFileAttributesA" (ByVal lpFileName As String) As Long  'v17.50削除 2011/02/28 by 間々田

		//コマンド（実行ファイル）の終了確認用ＡＰＩ関数の宣言  added by 山本 97-10-20
		[DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int OpenProcess(int dwDesiredAccess, int bInheritHandle, int dwProcessId);
		//Declare Function WaitForSingleObject Lib "kernel32" (ByVal hHandle As Long, ByVal dwMilliseconds As Long) As Long

		//ウィンドウポジション   'V4.0 append by 鈴山 2001/02/14
		[DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int SetWindowPos(int hWnd, int hwndInsertAfter, int x, int y, int CX, int CY, int wFlags);
		[DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int GetWindowRect(int hWnd, ref Ipc32v5.RECT lpRect);


		//********************************************************************************
		//機    能  ：  ファイル名がディレクトリか判断する
		//              変数名           [I/O] 型        内容
		//引    数  ：  FN               [I/ ] String    文字列
		//戻 り 値  ：                   [ /O] Boolean   判定結果  True:一致  False:不一致
		//補    足  ：  なし
		//
		//履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
		//              V3.0   00/09/05  (SI1)鈴山       右端の"\"は許可するように変更
		//              v17.50 2011/02/28 (電S1)間々田   廃止（未使用）
		//********************************************************************************
		//Public Function DirectoryNameCheck(FN As String) As Boolean
		//
		//    'ファイル名がディレクトリか判断する
		//    If (GetFileAttributes(FN) And FILE_ATTRIBUTE_DIRECTORY) = &H10 Then
		//        DirectoryNameCheck = True
		//    Else
		//        DirectoryNameCheck = False
		//    End If
		//
		//    'getfileattributes関数が属性の取得に失敗した場合も偽とする
		//    If GetFileAttributes(FN) = -1 Then
		//        DirectoryNameCheck = False
		//    End If
		//
		//    '右端がピリオド(".")だったらディレクトリとみなさない
		//    If (Right(FN, 1) = ".") Then
		//        DirectoryNameCheck = False
		//    End If
		//
		//End Function


		//********************************************************************************
		//機    能  ：  最後に発生したｴﾗｰの番号とその内容を表示する
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V3.0   00/09/27  (SI1)鈴山       新規作成
		//              V4.0   01/02/15  (SI1)鈴山       表示方法を変更
		//              v7.0   03/06/26  (SI4)間々田     Function→Sub
		//********************************************************************************
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		Public Function ErrorDescription(Optional ByVal strMsg As String = "")

			'メッセージ表示：
			'   ｴﾗｰ番号= XXX
			'
			'   エラー内容
			'
			'   引数で指定されたメッセージ
			'
			ErrorDescription = MsgBox(LoadResString(IDS_ErrorNum) & Str(Err.Number) & vbCrLf & vbCrLf & _
										Err.Description & vbCrLf & vbCrLf & _
										strMsg, _
										vbCritical + vbAbortRetryIgnore)

		End Function
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
		public static DialogResult ErrorDescription(string strMsg = "", Exception ex = null)		//TODO Errオブジェクト
		{
			//メッセージ表示：
			//   ｴﾗｰ番号= XXX
			//
			//   エラー内容
			//
			//   引数で指定されたメッセージ
			//
			string showMessage = null;

			if (ex == null)
			{
				showMessage = strMsg;
			}
			else
			{
				showMessage = Resources.LoadResString(StringTable.IDS_ErrorNum) + ex.GetType().Name + "\r\n" + "\r\n" +
							  ex.Message + "\r\n" + "\r\n" + 
							  strMsg;
			}

			return MessageBox.Show(showMessage, Application.ProductName, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
		}


		//********************************************************************************
		//機    能  ：  ファイル名の禁止文字のチェック(\,/,:,*,?,",<,>,|)
		//              変数名           [I/O] 型        内容
		//引    数  ：  FileName         [I/O] String    文字列
		//戻 り 値  ：                   [ /O] Boolean   判定結果  True:可  False:不可
		//補    足  ：  なし
		//
		//履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
		//********************************************************************************
		public static bool FileNameProhibitionCheck(string FileName)
		{
			bool functionReturnValue = false;

			//ファイル名の禁止文字
			//Const ProhibitStr As String = "\/:*?<>| " & """"
			const string ProhibitStr = ".\\/:*?<>| " + "\"";		//v16.20/v17.00変更 byやまおか 2010/03/02


			//戻り値初期化
			int i = 0;
			functionReturnValue = false;
			for (i = 1; i <= FileName.Length;i++)
			{
				if (ProhibitStr.IndexOf(FileName.Substring(i - 1, 1)) > -1) return functionReturnValue;
			}

			functionReturnValue = true;
			return functionReturnValue;
		}


		//********************************************************************************
		//機    能  ：  ディレクトリ名の禁止文字のチェック(,/,*,?,",<,>,|)
		//              変数名           [I/O] 型        内容
		//引    数  ：  FileName         [I/O] String    文字列
		//戻 り 値  ：                   [ /O] Boolean   判定結果  True:可  False:不可
		//補    足  ：  なし
		//
		//履    歴  ：  v16.20/v17.00  XX/XX/XX  ??????????????  新規作成
		//********************************************************************************
		public static bool DirNameProhibitionCheck(string FileName)
		{
			bool functionReturnValue = false;

			//ディレクトリ名の禁止文字
			const string ProhibitStr = "./*?<>|" + "\"";

			int i = 0;

			//戻り値初期化
			functionReturnValue = false;

			for (i = 1; i <= FileName.Length; i++)
			{
				if (ProhibitStr.IndexOf(FileName.Substring(i - 1, 1)) > -1) return functionReturnValue;
			}

			functionReturnValue = true;
			return functionReturnValue;
		}


		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//'********************************************************************************
		//'機    能  ：  文字列がすべて数字か調べる関数
		//'              変数名           [I/O] 型        内容
		//'引    数  ：  chr              [I/ ] String    文字列
		//'戻 り 値  ：                   [ /O] Boolean   判定結果  True:一致  False:不一致
		//'補    足  ：  なし
		//'
		//'履    歴  ：  V1.00  97/11/20  (CATE)山本      新規作成
		//'********************************************************************************
		//Public Function IsNumberCharacter(ByVal chr As String) As Boolean
		//
		//    'Dim i As Integer
		//    '
		//    'For i = 1 To Len(chr)
		//    '
		//    '    Select Case Mid$(chr, i, 1)
		//    '        Case "0" To "9"
		//    '        Case Else
		//    '            IsNumberCharacter = False
		//    '            Exit Function
		//    '    End Select
		//    '
		//    'Next
		//    '
		//    'IsNumberCharacter = True
		//
		//    'v11.5変更 by 間々田 2006/07/06
		//    IsNumberCharacter = (chr <> "") And (chr Like String$(Len(chr), "#"))
		//
		//End Function
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


		//********************************************************************************
		//機    能  ：  最大値を返す
		//              変数名           [I/O] 型        内容
		//引    数  ：  P1()             [I/ ] Variant   配列
		//戻 り 値  ：                   [ /O] Variant   配列の最大値
		//補    足  ：
		//
		//
		//履    歴  ：  V3.0   00/09/04  (SI1)鈴山       新規作成
		//********************************************************************************
		public static short MaxVal(params short[] P1)
		{
			short y = P1[0];
			foreach (short x in P1)
			{
				if (x > y) y = x;
			}
			return y;
		}
		public static int MaxVal(params int[] P1)
		{
			int y = P1[0];
			foreach (int x in P1)
			{
				if (x > y) y = x;
			}
			return y;
		}
		public static float MaxVal(params float[] P1)
		{
			float y = P1[0];
			foreach (float x in P1)
			{
				if (x > y) y = x;
			}
			return y;
		}
		public static double MaxVal(params double[] P1)
		{
			double y = P1[0];
			foreach (double x in P1)
			{
				if (x > y) y = x;
			}
			return y;
		}


		//********************************************************************************
		//機    能  ：  最小値を返す
		//              変数名           [I/O] 型        内容
		//引    数  ：  P1()             [I/ ] Variant   配列
		//戻 り 値  ：                   [ /O] Variant   配列の最小値
		//補    足  ：
		//
		//履    歴  ：  V3.0   00/09/04  (SI1)鈴山       新規作成
		//********************************************************************************
		public static short MinVal(params short[] P1)
		{
			short y = P1[0];
			foreach (short x in P1)
			{
				if (x < y) y = x;
			}
			return y;
		}
		public static int MinVal(params int[] P1)
		{
			int y = P1[0];
			foreach (int x in P1)
			{
				if (x < y) y = x;
			}
			return y;
		}
		public static float MinVal(params float[] P1)
		{
			float y = P1[0];
			foreach (float x in P1)
			{
				if (x < y) y = x;
			}
			return y;
		}
		public static double MinVal(params double[] P1)
		{
			double y = P1[0];
			foreach (double x in P1)
			{
				if (x < y) y = x;
			}
			return y;
		}


		//********************************************************************************
		//機    能  ：  カンマ区切りのレコードから指定位置の文字列を取得
		//              変数名           [I/O] 型        内容
		//引    数  ：  strTarget        [I/ ] String    入力データ
		//              no               [I/ ] Integer   指定位置
		//戻 り 値  ：                   [ /O] String    出力データ
		//補    足  ：  なし
		//
		//履    歴  ：  V5.0 2004/06/09  (SI4)間々田     新規作成
		//                                               (以前の SelectFromRecord と同等処理）
		//********************************************************************************
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//Public Function GetSplitItem(ByVal strTarget As String, ByVal no As Integer) As String
		//
		//    Dim strCell() As String
		//
		//    '戻り値の初期値
		//    GetSplitItem = ""
		//
		//    '対象文字がヌルの場合
		//    If strTarget = "" Then Exit Function
		//
		//    'カンマで区切り、文字列型配列に格納
		//    strCell = Split(strTarget, ",")
		//
		//    '指定位置の文字列を返す
		//    If UBound(strCell) >= no - 1 Then GetSplitItem = strCell(no - 1)
		//
		//End Function
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''


		//********************************************************************************
		//機    能  ：
		//
		//
		//              変数名           [I/O] 型        内容
		//引    数  ：  targetString     [I/ ] String
		//戻 り 値  ：  GetFirstItem()   [ /O] String
		//
		//補    足  ：  なし
		//
		//履    歴  ：  v7.00  03/10/22  (SI4)間々田     新規作成
		//********************************************************************************
		public static string GetFirstItem(string TargetString, string delimiter = ",")
		{
			string functionReturnValue = null;

			int pos = 0;

			pos = TargetString.IndexOf(delimiter);
			if (pos > -1)
			{
				functionReturnValue = TargetString.Substring(0, pos);
			}
			else
			{
				functionReturnValue = TargetString;
			}
			return functionReturnValue;
		}


		//
		//   フォームが存在するか調べる      append 2003/07/31 by 間々田
		//
		public static bool IsExistForm(Form theForm)
		{
			//戻り値初期化
			bool functionReturnValue = true;

			//エラーは無視する
			try
			{
				//見つかったら直ちに抜ける
				foreach (Form workForm in Application.OpenForms) 
				{
					if (workForm.Equals(theForm)) return functionReturnValue;
				}
			}
			catch
			{
				// Nothing
			}

			//見つからなかったらFalseをセット
			functionReturnValue = false;
			return functionReturnValue;
		}


		//
		//   値が特定の範囲内か調べる      append 2003/07/31 by 間々田
		//
		public static bool InRange(int theValue, int MinValue, int MaxValue)
		{
			bool functionReturnValue = false;

			if (theValue < MinValue)
			{
				functionReturnValue = false;
			}
			else if (theValue > MaxValue)
			{
				functionReturnValue = false;
			}
			else
			{
				functionReturnValue = true;
			}

			return functionReturnValue;
		}


		//
		//   値が特定の範囲内か調べる      append 2003/07/31 by 間々田
		//
		public static bool InRangeFloat(float theValue, float MinValue, float MaxValue)
		{
			bool functionReturnValue = false;

			if (theValue < MinValue)
			{
				functionReturnValue = false;
			}
			else if (theValue > MaxValue)
			{
				functionReturnValue = false;
			}
			else
			{
				functionReturnValue = true;
			}

			return functionReturnValue;
		}


		//
		//   値を範囲内に収まるように補正します v9 by 間々田 2004/02/25
		//
		public static short CorrectInRange(short theValue, short MinValue, short MaxValue)
		{
			short functionReturnValue = 0;

			if (theValue < MinValue)
			{
				functionReturnValue = MinValue;
			}
			else if (theValue > MaxValue)
			{
				functionReturnValue = MaxValue;
			}
			else
			{
				functionReturnValue = theValue;
			}

			return functionReturnValue;
		}


		//
		//   値を指定された範囲内の値に調整する added by 間々田 2004/02/03
		//
		public static int CorrectInRangeLong(int theValue, int MinValue, int MaxValue)
		{
			int functionReturnValue = 0;

			if (theValue < MinValue)
			{
				functionReturnValue = MinValue;
			}
			else if (theValue > MaxValue)
			{
				functionReturnValue = MaxValue;
			}
			else
			{
				functionReturnValue = theValue;
			}

			return functionReturnValue;
		}


		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//'
		//'   値を範囲内に収まるように補正します（Float型用） v11.3 by 間々田 2006/02/20
		//'
		//Public Function CorrectInRangeFloat(ByVal theValue As Single, ByVal MinValue As Single, ByVal MaxValue As Single) As Single
		//
		//    Select Case theValue
		//
		//        Case Is < MinValue
		//            CorrectInRangeFloat = MinValue
		//
		//        Case Is > MaxValue
		//            CorrectInRangeFloat = MaxValue
		//
		//        Case Else
		//            CorrectInRangeFloat = theValue
		//
		//    End Select
		//
		//End Function
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


		//
		//   アークサインを求める
		//
		public static double ArcSin(double theValue)
		{
			double functionReturnValue = 0;

			if (theValue == -1 || theValue == 1)
			{
				//追加 2004/04/08 by 間々田
				functionReturnValue = theValue * ScanCorrect.Pai / 2.0;
			}
			else
			{
				functionReturnValue = Math.Atan(theValue / Math.Sqrt(-theValue * theValue + 1));
			}

			return functionReturnValue;
		}


		//V8.0 追加  松井
		public static int GetOption(RadioButton[] optArray)
		{
			int functionReturnValue = 0;

			try
			{
				foreach (RadioButton optUnit in optArray)
				{
					if (optUnit.Checked)
					{
						functionReturnValue = Array.IndexOf(optArray, optUnit);
						return functionReturnValue;
					}
				}
			}
			catch
			{
				functionReturnValue = -1;
			}

			return functionReturnValue;
		}


		// added V7.0 by 間々田 2003/11/12
		//Public Sub SetOption(optArray As Object, ByVal theValue As Long, Optional ByVal MissingValue As Long)
		public static void SetOption(RadioButton[] optArray, int theValue, int? MissingValue = null)
		{
			foreach (RadioButton optUnit in optArray)
			{
				if (Array.IndexOf(optArray, optUnit) == theValue)
				{
					optUnit.Checked = true;
					return;
				}
			}

			if (MissingValue != null)
			{
				foreach (RadioButton optUnit in optArray)
				{
					if (Array.IndexOf(optArray, optUnit) == MissingValue)
					{
						optUnit.Checked = true;
						return;
					}
				}
			}
		}


		//
		//   v9.5 追加 by 間々田 2004/09/24
		//
		//Public Sub SetCmdButton(cmdArray As Object, ByVal theValue As Long)
		public static void SetCmdButton(Button[] cmdArray, int theValue, Color? OnColor = null, Color? OffColor = null, bool ControlEnabled = false)		//v11.3変更 by 間々田 2006/02/20
		{
			//For Each cmdUnit In cmdArray
			//    cmdUnit.BackColor = IIf(cmdUnit.Index = theValue, vbGreen, vbButtonFace)
			//Next

			//v11.3以下に変更 by 間々田 2006/02/20
			foreach (Button cmdUnit in cmdArray)
			{
				cmdUnit.BackColor = (Array.IndexOf(cmdArray, cmdUnit) == theValue ? (Color)(OnColor ?? Color.Lime) : (Color)(OffColor ?? SystemColors.Control));
				//If ControlEnabled Then cmdUnit.Enabled = (cmdUnit.Index <> theValue)
				if (ControlEnabled) cmdUnit.Enabled = (Array.IndexOf(cmdArray, cmdUnit) != theValue) && cmdUnit.Parent.Enabled;			//v11.4変更 by 間々田 2006/03/02
			}
		}


		public static int GetCmdButton(Button[] cmdArray, Color? OnColor = null)
		{
			int functionReturnValue = 0;

			foreach (Button cmdUnit in cmdArray)
			{
				if (cmdUnit.BackColor == (Color)(OnColor ?? Color.Lime))
				{
					functionReturnValue = Array.IndexOf(cmdArray, cmdUnit);
					return functionReturnValue;
				}
			}

			//見つからなかった場合
			functionReturnValue = -1;
			return functionReturnValue;
		}


		// added V7.0 by 間々田 2003/11/12 'True のオプションボタンが非表示の場合、表示中の別のオプションボタンをTrueとする
		//Public Sub CorrectOption(optArray As Object)
		public static bool CorrectOption(RadioButton[] optArray)		//change by 間々田 2004/06/16
		{
			bool functionReturnValue = false;			//added by 間々田 2004/06/16

			foreach (RadioButton optUnit in optArray)
			{
				if (optUnit.Checked && optUnit.Enabled) return functionReturnValue;
			}

			foreach (RadioButton optUnit in optArray)
			{
				if (optUnit.Enabled)
				{
					optUnit.Checked = true;
					functionReturnValue = true;		//added by 間々田 2004/06/16
					return functionReturnValue;
				}
			}
			return functionReturnValue;
		}


		public static void RePosOption(RadioButton[] optArray)
		{
			int Count = 0;
			int theTop = 0;

			Count = 0;
			foreach (RadioButton optUnit in optArray)
			{
				if (optUnit.Enabled) Count = Count + 1;
			}

			theTop = 0;
			foreach (RadioButton optUnit in optArray)
			{
				if (optUnit.Enabled)
				{
					theTop = theTop + (int)((optUnit.Parent.Height) / (Count + 1));
					optUnit.Top = theTop;
				}
				else
				{
					optUnit.Visible = false;
				}
			}
		}


		//
		//   拡張子を付加します  v9.5追加 by 間々田 2004/09/21
		//
		public static string AddExtension(string strTarget, string strExtension)
		{
			string functionReturnValue = strTarget;

			if (string.IsNullOrEmpty(strTarget))
			{ }
			else if (!strTarget.ToLowerInvariant().EndsWith(strExtension.ToLowerInvariant()))
			{
				functionReturnValue = strTarget + strExtension;
			}

			return functionReturnValue;
		}


		//
		//   拡張子を除去します  v9.5追加 by 間々田 2004/09/21
		//
		public static string RemoveExtension(string strTarget, string strExtension)
		{
			string functionReturnValue = strTarget;

			if (string.IsNullOrEmpty(strTarget))
			{ }
			else if (strTarget.ToLowerInvariant().EndsWith(strExtension.ToLowerInvariant()))
			{
				functionReturnValue = strTarget.Substring(0, strTarget.Length - strExtension.Length);
			}

			return functionReturnValue;
		}


		//
		//   スワップ    v9.5追加 by 間々田 2004/09/27
		//
		public static void Swap(ref int a, ref int b)
		{
			int w = 0;

			w = a;
			a = b;
			b = w;
		}


		public static string GetCsvItem(string FileName, string KeyWord)
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*

			Dim fileNo      As Integer
			Dim buf         As String
			Dim strCell()   As String
			Dim DotPos      As Integer

			'戻り値初期化
			GetCsvItem = ""

			On Error GoTo ExitHandler

			'ファイルオープン
			fileNo = FreeFile()
			Open FileName For Input As fileNo

			Do While Not EOF(fileNo) 'ファイルの終端までループを繰り返す
    
				'１行読み込む
				Line Input #fileNo, buf
        
				If buf <> "" Then

					'カンマで区切って配列変数に格納
					strCell = Split(buf, ",")
            
					'最初の項目がキーワードに該当する？
					If Trim$(strCell(0)) = KeyWord Then
						DotPos = InStr(buf, ",")
						If DotPos > 0 Then GetCsvItem = Mid$(buf, DotPos + 1)
						Exit Do
					End If
            
				End If
        
			Loop
    
			'ファイルクローズ
			Close fileNo

		ExitHandler:

 */
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			string functionReturnValue = null;

			string buf = null;
			string[] strCell = null;
			int DotPos = -1;

			StreamReader sr = null;

			try
			{
				//ファイルオープン
				sr = new StreamReader(FileName);

				//ファイルの終端までループを繰り返す
				while ((buf = sr.ReadLine()) != null)	//１行読み込む
				{
					if (!string.IsNullOrEmpty(buf))
					{
						//カンマで区切って配列変数に格納
						strCell = buf.Split(',');

						//最初の項目がキーワードに該当する？
						if (strCell[0].Trim() == KeyWord)
						{
							DotPos = buf.IndexOf(',');
							if (DotPos > -1) functionReturnValue = buf.Substring(DotPos + 1);
							break;
						}
					}
				}
			}
			finally
			{
				//ファイルクローズ
				if (sr != null)
				{
					sr.Close();
					sr = null;
				}
			}

			return functionReturnValue;
		}


		//
		//   ヌル以降の文字列を取り除く  v9.7追加 by 間々田 2004/12/09
		//
		public static string RemoveNull(string TargetString)
		{
			//Dim nullPos As Integer
			//
			//nullPos = InStr(TargetString, chr(0))
			//
			//If nullPos > 0 Then
			//    RemoveNull = Left$(TargetString, nullPos - 1)
			//Else
			//    RemoveNull = TargetString
			//End If

			//v11.4変更 by 間々田 2006/03/16
			return GetFirstItem(TargetString, "\0").Trim();
		}


		//*******************************************************************************
		//機　　能： 文字列内のCR・CFを半角スペースに置き換える
		//
		//           変数名          [I/O] 型        内容
		//引　　数： TargetString    [I/ ] String    処理対象文字列
		//戻 り 値：                 [ /O] String    処理結果文字列
		//
		//補　　足： なし
		//
		//履　　歴： v11.3 2006/02/20 (SI3)間々田    新規作成
		//*******************************************************************************
		public static string RemoveCRLF(string TargetString)
		{
			return TargetString.Replace("\r", " ").Replace("\n", " ");
		}


		//*******************************************************************************
		//機　　能： 指定されたフレーム内のコントロールの使用可・不可を設定する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 除外するコントロールは ExceptContorls で指定する
		//
		//履　　歴： v11.2 2006/01/11 (SI3)間々田    新規作成
		//*******************************************************************************
		public static void SetEnabledInFrame(GroupBox theFrame, bool theOnOff, params string[] ExceptContorls)
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Dim theControl      As Control

			Dim ExceptControl   As Variant


			On Error Resume Next
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			bool InFrame = false;
			bool IsExceptControl = false;

			try
			{
				foreach (Control theControl in theFrame.Controls)
				{
					try
					{
						InFrame = theFrame.Equals(theControl.Parent);

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
						If Err.Number <> 0 Then
							Err.Clear
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

						if (InFrame)
						{
							IsExceptControl = false;

							if (ExceptContorls != null)
							{
								foreach (string ExceptControl in ExceptContorls)
								{
									if (theControl.GetType().Name == ExceptControl)
									{
										IsExceptControl = true;
										break;
									}
								}
							}

							if (!IsExceptControl) theControl.Enabled = theOnOff;
						}
					}
					catch
					{
						// Nothing
					}
				}
			}
			catch
			{
				// Nothing
			}
		}


		//*******************************************************************************
		//機　　能：
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.4 2006/03/02 (SI3)間々田    新規作成
		//*******************************************************************************
		public static string GetFormatString(float theValue)
		{
			string theString = null;
			int i = 0;

			theString = Convert.ToString(theValue);

			for (i = 1; i <= 9; i++)
			{
				theString = theString.Replace(Convert.ToString(i), "0");
			}

			return theString;
		}


		//*******************************************************************************
		//機　　能：
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.4 2006/03/02 (SI3)間々田    新規作成
		//*******************************************************************************
		public static int GetIndexByStr(string theString, string[] theArray, int MissingValue = 0)
		{
			int functionReturnValue = 0;

			int i = 0;

			//戻り値初期化
			functionReturnValue = MissingValue;

			for (i = theArray.GetLowerBound(0); i <= theArray.GetUpperBound(0); i++)
			{
				if (theArray[i].Trim().ToUpperInvariant() == theString.Trim().ToUpperInvariant())
				{
					functionReturnValue = i;
					return functionReturnValue;
				}
			}
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能：
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.43 2006/04/21 (WEB)間々田    新規作成
		//*******************************************************************************
		public static void SetField(string theData, ref string theFiled)
		{
			theFiled = theData.Trim() + new string('\0', theFiled.Length);
		}


		//*******************************************************************************
		//機　　能： ﾄﾞライブの空き容量（Kバイト単位）を取得
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.5 2006/06/27 (WEB)間々田    新規作成
		//*******************************************************************************
		//Public Function GetDiskFreeKByte(ByVal theDrive As String) As Long
		public static long GetDiskFreeKByte(string theDrive)
		{
			long functionReturnValue = 0;

			//Dim RootPathName            As String
			int SectorsPerCluster = 0;
			int BytesPerSector = 0;
			int NumberOfFreeClusters = 0;
			int TtoalNumberOfClusters = 0;

			//保存ドライブのパスを取得
			//RootPathName = IIf(theDrive = "", "c", Left$(theDrive, 1)) & ":\"

			//ﾄﾞライブの空き容量を取得（ＡＰＩ関数の呼出し）
			//If GetDiskFreeSpace(RootPathName, SectorsPerCluster, BytesPerSector, NumberOfFreeClusters, TtoalNumberOfClusters) = 0 Then
			if (modDeclare.GetDiskFreeSpace(theDrive, ref SectorsPerCluster, ref BytesPerSector, ref NumberOfFreeClusters, ref TtoalNumberOfClusters) == 0)
			{
				//エラー時は－１を返す
				functionReturnValue = -1;
			}
			else
			{
				//空き容量の計算(Kバイト)
				functionReturnValue = NumberOfFreeClusters / 1024 * BytesPerSector * SectorsPerCluster;
			}
			return functionReturnValue;
		}


		//v15.0追加 by 間々田 2009/01/15
		public static string GetCsvRec(params string[] p)
		{
			return string.Join(",", p);
		}


		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//v15.0追加 by 間々田 2009/01/15
		//Public Sub CenteringInFrame(theControl As Control)
		//
		//    On Error Resume Next
		//    With theControl.Container
		//        theControl.Move .Width / 2 - theControl.Width / 2, .Height / 2 - theControl.Height / 2
		//    End With
		//
		//End Sub
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


		public static void CopyCWNumEdit(NumericUpDown cwneSource, NumericUpDown cwneDestination)
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			With cwneDestination
				.DiscreteInterval = cwneSource.DiscreteInterval
				.IncDecValue = cwneSource.IncDecValue
				.FormatString = cwneSource.FormatString
				.SetMinMax cwneSource.Minimum, cwneSource.Maximum
				.Value = cwneSource.Value
			End With
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			cwneDestination.Increment = cwneSource.Increment;
			cwneDestination.DecimalPlaces = cwneSource.DecimalPlaces;
			cwneDestination.Maximum = cwneSource.Maximum;
			cwneDestination.Minimum = cwneSource.Minimum;
			cwneDestination.Value = cwneSource.Value;

		}


		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//Public Sub SetLabelFont(theForm As Form, Optional ByVal fontName As String = "Arial") ', Optional ByVal fontSize As Integer = 10)
		//
		//    Dim theControl As Control
		//
		//    For Each theControl In theForm.Controls
		//        If TypeName(theControl) = "Label" Then
		//            theControl.Font.Name = fontName
		//            'theControl.Font.size = fontSize
		//        End If
		//    Next
		//
		//End Sub
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

		//v19.00 追加 ->(電S2)永井

		//*******************************************************************************
		//機　　能： ヌルを付加する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： TargetString    [I/ ] String    処理対象文字列
		//           theLength       [I/ ] Integer   ヌル付加後の全体の文字長を指定（未指定時は256）
		//戻 り 値：                 [ /O] String    処理結果文字列
		//
		//補　　足： なし
		//
		//履　　歴： v6.00 2005/03/11 (SI3)間々田    新規作成
		//*******************************************************************************
		public static string AddNull(string TargetString, int theLength = 256)
		{
			string theString = null;

			theString = TargetString.Trim();

			return theString + new string('\0', MaxVal(theLength - modDeclare.lstrlen(theString), 0));
		}


		//
		//   コモンダイアログ用フィルタに設定する文字列を作成する
		//
		public static string MakeCommonDialogFilter(string fileType, string Extension)
		{
			//xxx(*yyy)|*yyy|すべてのファイル(*.*)|*.*
			return fileType + "(*" + Extension + ")|*" + Extension + "|" + Resources.LoadResString(10300);
		}


		//********************************************************************************
		//機    能  ：  コモンダイアログ（ファイル選択［複数指定有］）にて取得したファイル名のリストから
		//              パス名とファイル名に分け、配列に格納して返す
		//
		//              変数名           [I/O] 型        内容
		//引    数  ：  fileNameList     [I/ ] String    コモンダイアログから受け取るファイル名のリスト
		//戻 り 値  ：  FileName()       [ /O] String    FileName(0):パス名（末尾に \ がつかないタイプ。ルート直下は \ がつく）
		//                                               FileName(n):フルファイル名 （n>0）
		//補    足  ：  なし
		//
		//履    歴  ：  V4.00  02/10/22  (SI4)間々田     新規作成
		//               5.00  04/08/27  (SI4)間々田     引数追加
		//********************************************************************************
		public static void GetFileList(string fileNameList, ref string[] FileName, bool multiOn = true)
		{

			string[] strCell = null;
			int YenPos = 0;
			int i = 0;
			int fileCount = 0;

			//コモンダイアログで取得したファイル名のリストを文字列配列に分割
			//追加 v5.00 04/08/27
			if (multiOn)
			{
				//strCell = Split(fileNameList, " ")
				strCell = fileNameList.Split('\0');				//変更 2005/03/02 コモンダイアログ呼び出し時にcdlOFNExplorerを追加したため
			}
			else
			{									//追加 v5.00 04/08/27
				strCell = new string[1];		//追加 v5.00 04/08/27
			}									//追加 v5.00 04/08/27

			//取得したファイルの数およびファイル名を格納する配列の宣言
			fileCount = strCell.GetUpperBound(0) == 0 ? 1 : strCell.GetUpperBound(0);
			FileName = new string[fileCount + 1];

			//単数ファイル指定
			if (fileCount == 1)
			{
				YenPos = fileNameList.LastIndexOf("\\");
				FileName[0] = fileNameList.Substring(0, YenPos);

				//パスの末尾が : の場合、\ を付加
				if (FileName[0].EndsWith(":")) FileName[0] = FileName[0] + "\\";

				FileName[1] = fileNameList;
			}
			//複数ファイル指定
			else
			{
				//パス名が格納されているstrCell(0)をそのまま FileName(0) にコピー
				FileName[0] = strCell[0];

				//パス名の末尾が \ でない場合、\ を付加
				if (!strCell[0].EndsWith("\\")) strCell[0] = strCell[0] + "\\";

				//v6.0削除ここから by 間々田 2005/03/11
				//For i = 1 To fileCount
				//    fileName(i) = strCell(0) & strCell(i)
				//Next
				//v6.0削除ここまで by 間々田 2005/03/11

				//v6.0追加ここから by 間々田 2005/03/11

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				Dim theFileLists    As New Collection
				Dim theFile         As Variant
        
				'コレクションにソートしながら格納
				For i = 1 To fileCount
					For Each theFile In theFileLists
						If strCell(i) < theFile Then Exit For
					Next
					If IsEmpty(theFile) Then
						theFileLists.Add strCell(i), strCell(i)
					Else
						theFileLists.Add strCell(i), strCell(i), before:=theFile
					End If
				Next
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

				SortedList<string, string> theFileLists = new SortedList<string, string>();

				//コレクションにソートしながら格納
				for (i = 1; i <= fileCount; i++)
				{
					theFileLists.Add(strCell[i], strCell[i]);
				}

				//配列に格納
				i = 0;
				foreach (string theFile in theFileLists.Values)
				{
					i = i + 1;
					FileName[i] = strCell[0] + theFile;
				}
				//v6.0追加ここまで by 間々田 2005/03/11
			}

			//'念のためすべての文字列を大文字にしておく   'v7.0削除 by 間々田 2006/02/10
			//For i = 0 To fileCount
			//    fileName(i) = UCase$(fileName(i))
			//Next
		}


		//*******************************************************************************
		//機　　能： テキストボックスバイト数チェック（全角文字対応版）
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  2004/06/01 (SI4)間々田   新規作成
		//*******************************************************************************
		public static void CheckTextBox(TextBox theTextBox)
		{

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Dim ansiStr     As String

			With theTextBox

				'イベントの連鎖防止
				If Not .Enabled Then Exit Sub

				'バイト数チェックのためansiコードに変換する
				ansiStr = StrConv(.Text, vbFromUnicode)

				'バイト数がMaxLengthを超えたら
				If LenB(ansiStr) > .MaxLength Then

					'メッセージボックスの表示：%1文字を越えています。
					'v19.00 マイクロCTにあわせる
					'ShowMessage GetResString(9928, CStr(.MaxLength)), vbCritical
					ShowMessage GetResString(9318, CStr(.MaxLength))

					'強制的にMaxLengthバイトにする。unicodeに戻す
					.Enabled = False
					.Text = StrConv(LeftB(ansiStr, .MaxLength), vbUnicode)
					.Enabled = True

				End If

			End With
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//イベントの連鎖防止
			if (!theTextBox.Enabled) return;

			string tbText = theTextBox.Text;

			//バイト数がMaxLengthを超えたら
			if (Encoding.Default.GetByteCount(tbText) > theTextBox.MaxLength)
			{
				//メッセージボックスの表示：%1文字を越えています。
				//v19.00 マイクロCTにあわせる
				//ShowMessage GetResString(9928, CStr(.MaxLength)), vbCritical
				modCT30K.ShowMessage(StringTable.GetResString(9318, theTextBox.MaxLength.ToString()));

				//強制的にMaxLengthバイトにする。
				string cutText = Encoding.Default.GetString(Encoding.Default.GetBytes(tbText), 0, theTextBox.MaxLength);

				if (cutText[cutText.Length - 1] != tbText[cutText.Length - 1])
				{
					cutText = cutText.Substring(0, cutText.Length - 1);
				}

				theTextBox.Enabled = false;
				theTextBox.Text = cutText;
				theTextBox.Enabled = true;
			}
		}


		//*******************************************************************************
		//機　　能： フルファイル名をディレクトリ名とファイル名に分ける
		//
		//           変数名          [I/O] 型        内容
		//引　　数： FullFileName    [I/ ] String    フルファイル名
		//           DirName         [ /O] String    ディレクトリ名（末尾に\が付いている）
		//           FileName        [ /O] String    ファイル名
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.2  2007/05/09  (WEB)間々田  新規作成
		//*******************************************************************************
		public static void SeparateFileName(string FullFileName, ref string DirName, ref string FileName)
		{
			int YenPos = 0;

			YenPos = FullFileName.LastIndexOf("\\");
			DirName = FullFileName.Substring(0, YenPos);
			FileName = FullFileName.Substring(YenPos + 1);
		}


		//*******************************************************************************
		//機　　能： ディレクトリ名およびファイル名のチェック関数
		//
		//           変数名          [I/O] 型        内容
		//引　　数： DirName         [I/ ] String
		//
		//戻 り 値：                 [ /O] Boolean
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//Public Function IsValidFileName(ByVal DirName As String, ByVal FileName As String) As Boolean
		//
		//    Dim hFso As FileSystemObject
		//    Set hFso = New FileSystemObject
		//
		//'v8.0 削除 by 村田
		//    '戻り値初期化
		//'    IsValidFileName = False
		//'
		//'    If DirName = "" Then                                'ディレクトリ名を指定していない
		//'        Exit Function
		//'    ElseIf Dir(DirName, vbDirectory) = "" Then          'ディレクトリが存在しない
		//'        Exit Function
		//'    ElseIf fileName = "" Then                           'ファイル名を指定していない
		//'        Exit Function
		//'    ElseIf Not FileNameProhibitionCheck(fileName) Then  'ファイル名に禁止文字が含まれている
		//'        Exit Function
		//'    End If
		//'
		//'    IsValidFileName = True
		//
		//    '戻り値初期化
		//    IsValidFileName = True
		//
		//    'フォルダの存在確認
		//    If Not hFso.FolderExists(DirName) Then
		//        IsValidFileName = False
		//    'ファイルの存在確認
		//    ElseIf Not hFso.FileExists(DirName & FileName) Then
		//        IsValidFileName = False
		//    ElseIf Not FileNameProhibitionCheck(FileName) Then  'ファイル名に禁止文字が含まれている
		//        IsValidFileName = False
		//    End If
		//
		//    Set hFso = Nothing
		//
		//End Function
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


		//********************************************************************************
		//機    能  ：  ファイル名の禁止文字のチェック(\,/,:,*,?,",<,>,|)(ピリオド[.]判定なし)
		//              変数名           [I/O] 型        内容
		//引    数  ：  FileName         [I/O] String    文字列
		//戻 り 値  ：                   [ /O] Boolean   判定結果  True:可  False:不可
		//補    足  ：  なし
		//
		//履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
		//********************************************************************************
		public static bool FileNameProhibitionCheck2(string FileName)
		{
			bool functionReturnValue = false;

			//ファイル名の禁止文字
			const string ProhibitStr = "\\/:*?<>| " + "\"";

			//戻り値初期化
			int i = 0;
			functionReturnValue = false;

			for (i = 1; i <= FileName.Length; i++)
			{
				if (ProhibitStr.IndexOf(FileName.Substring(i - 1, 1)) > -1) return functionReturnValue;
			}

			functionReturnValue = true;
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： ディレクトリ名およびファイル名のチェック関数(ピリオド判定なし)
		//
		//           変数名          [I/O] 型        内容
		//引　　数： DirName         [I/ ] String
		//
		//戻 り 値：                 [ /O] Boolean
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static bool IsValidFileName2(string DirName, string FileName)
		{

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Dim hFso As FileSystemObject
			Set hFso = New FileSystemObject
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//v8.0 削除 by 村田
			//戻り値初期化
			//    IsValidFileName = False
			//
			//    If DirName = "" Then                                'ディレクトリ名を指定していない
			//        Exit Function
			//    ElseIf Dir(DirName, vbDirectory) = "" Then          'ディレクトリが存在しない
			//        Exit Function
			//    ElseIf fileName = "" Then                           'ファイル名を指定していない
			//        Exit Function
			//    ElseIf Not FileNameProhibitionCheck(fileName) Then  'ファイル名に禁止文字が含まれている
			//        Exit Function
			//    End If
			//
			//    IsValidFileName = True

			//戻り値初期化
			bool functionReturnValue = true;

			//フォルダの存在確認
			if (!Directory.Exists(DirName))
			{
				functionReturnValue = false;
			}
			//ファイルの存在確認
			else if (!File.Exists(Path.Combine(DirName, FileName)))
			{
				functionReturnValue = false;
			}
			else if (!FileNameProhibitionCheck2(FileName))		//ファイル名に禁止文字が含まれている
			{
				functionReturnValue = false;
			}

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Set hFso = Nothing
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			return functionReturnValue;
		}

//<- v19.00
	}
}
