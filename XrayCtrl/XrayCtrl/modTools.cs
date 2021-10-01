using System;
using System.Runtime.InteropServices;
using System.Text;

namespace XrayCtrl
{
	internal static class modTools
	{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//		Option Explicit
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//*****************************************************************************
		//
		//   その他
		//
		//   Date        Version     Designed/Changed
		//   2003/05/28  1.00        (NSD)Shibui
		//
		//*****************************************************************************

		////-----------------------------------------------------------------------------
		////Windows起動時からの経過時間を[msec]単位で取得するAPIの宣言
		////-----------------------------------------------------------------------------
		//Public Declare Function GetTickCount Lib "kernel32" () As Long

		//-----------------------------------------------------------------------------
		//INIファイル処理を行うAPIの宣言
		//-----------------------------------------------------------------------------
		[DllImport("kernel32.dll", SetLastError=true, CharSet=CharSet.Auto)]
		private extern static uint GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

		[DllImport("kernel32.dll", SetLastError=true, CharSet=CharSet.Auto)]
		private extern static uint WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);

		////------------------  ウィンドウポジション設定関数と定数の宣言  -----------------------------
		//Declare Function SetWindowPos _
		//            Lib "user32" (ByVal hwnd As Long, _
		//                          ByVal hwndInsertAfter As Long, _
		//                          ByVal X As Long, ByVal Y As Long, _
		//                          ByVal CX As Long, ByVal CY As Long, _
		//                          ByVal wFlags As Long) As Long
		//
		//Public Const HWND_BOTTOM = 1     ////ウインドウを最後方に
		//Public Const HWND_TOPMOST = -1   ////常に最全面のウィンドウに設定
		//Public Const HWND_NOTOPMOST = -2 ////最上位ウィンドウの背面に
		//Public Const HWND_TOP = 0        ////ウィンドウを一番手前に
		//Public Const SWP_NOMOVE = &H2    ////現在の位置を保つフラグ
		//Public Const SWP_NOSIZE = &H1    ////現在のサイズを保つフラグ
		////------------------------------------------------------------------------------------------


		//-------------------------------------------------------
		//
		//   文字列を数値（Integer型）に変換する
		//   数値評価、範囲付き
		//
		//   パラメータ
		//       intData as integer  //数値変換したデータ
		//       strData as string   //数値変換する文字列
		//       intMin  as integer  //許容範囲 最小値
		//       intMax  as integer  //許容範囲 最大値
		//
		//   戻り値
		//       0:正常終了
		//       1:数値評価不可
		//       2:範囲外
		//
		//-------------------------------------------------------
		public static int CIntChk(out int intData, string strData, int intMin, int intMax)
		{
			int intAns;

			intData = 0;

			if (int.TryParse(strData, out intData) == true)
			{
				if (intData >= intMin && intData <= intMax)
				{
					intAns = 0;
				}
				else
				{
					intAns = 2;
				}
			}
			else
			{
				intAns = 1;
			}

			return intAns;
		}

		//v11.5削除ここから by 間々田 2006/08/01 未使用
		////-------------------------------------------------------
		////
		////   文字列を数値（Long型）に変換する
		////   数値評価、範囲付き
		////
		////   パラメータ
		////       lngData as Long     //数値変換したデータ
		////       strData as string   //数値変換する文字列
		////       lngMin  as Long     //許容範囲 最小値
		////       lngMax  as Long     //許容範囲 最大値
		////
		////   戻り値
		////       0:正常終了
		////       1:数値評価不可
		////       2:範囲外
		////
		////-------------------------------------------------------
		//Public Function CLngChk(lngData As Long, strData As String, lngMin As Long, lngMax As Long) As Integer
		//    Dim intAns  As Integer
		//
		//    If IsNumeric(strData) = True Then
		//        lngData = CLng(strData)
		//        If lngData >= lngMin And lngData <= lngMax Then
		//            intAns = 0
		//        Else
		//            intAns = 2
		//        End If
		//    Else
		//        intAns = 1
		//    End If
		//
		//    CLngChk = intAns
		//End Function
		//v11.5削除ここまで by 間々田 2006/08/01 未使用

		//-------------------------------------------------------
		//
		//   文字列を数値（Single型）に変換する
		//   数値評価、範囲付き
		//
		//   パラメータ
		//       sngData as Single   //数値変換したデータ
		//       strData as string   //数値変換する文字列
		//       sngMin  as Single   //許容範囲 最小値
		//       sngMax  as Single   //許容範囲 最大値
		//
		//   戻り値
		//       0:正常終了
		//       1:数値評価不可
		//       2:範囲外
		//
		//-------------------------------------------------------
		public static int CSngChk(out float sngData, string strData, float sngMin, float sngMax)
		{
			int intAns;

			sngData = 0;

			if (float.TryParse(strData, out sngData) == true)
			{
				if (sngData >= sngMin && sngData <= sngMax)
				{
					intAns = 0;
				}
				else
				{
					intAns = 2;
				}
			}
			else
			{
				intAns = 1;
			}

			return intAns;
		}

		////--------------------------------------------------------------------------------
		////読み込み
		////--------------------------------------------------------------------------------
		public static string INIRead(string sSect, string sKey, string sDefault, string sFileName)
		{
			StringBuilder sBuf = new StringBuilder(512);
			uint nRet;
		    
			string sAPIpath;
			sAPIpath = sFileName;
			// APIのバグ対策
//			If LenB(sAPIpath) Mod 2 <> 0 Then sAPIpath = sAPIpath & Chr$(&H0)
		        
			nRet = GetPrivateProfileString(sSect, sKey, sDefault, sBuf, (uint)sBuf.Capacity, sAPIpath);
			return sBuf.ToString();
		}

		//v11.5削除ここから by 間々田 2006/08/01 未使用
		//////--------------------------------------------------------------------------------
		//////書き込み
		//////--------------------------------------------------------------------------------
		//Public Function INIWrite(sSect As String, sKey As String, _
		//                sPString As String, sFileName As String)
		//    Dim nRet        As Integer
		//    Dim mMsgVal     As Integer
		//
		//    Dim sAPIpath As String
		//    sAPIpath = sFileName
		//    // APIのバグ対策
		//    If LenB(sAPIpath) Mod 2 <> 0 Then sAPIpath = sAPIpath & Chr$(&H0)
		//
		//    nRet = WritePrivateProfileString(sSect, sKey, sPString, sAPIpath)
		//
		//    If nRet = 0 Then
		//    // エラー処理
		//        // 書き込みエラー
		// //       MsgBox "Write ERROR"
		//    End If
		//End Function
		//v11.5削除ここまで by 間々田 2006/08/01 未使用

		////==================================================================================
		////   グリッドに文字列をセットする
		////
		////   grd     As MSFlexGrid   //対象グリッド
		////   intCol  As Integer      //列
		////   intRow  As Integer      //行
		////   strText As String       //文字列
		////==================================================================================
		//Public Sub SetMSFGrid(grd As MSFlexGrid, intCol As Integer, intRow As Integer, strText As String)
		//    grd.Col = intCol
		//    grd.Row = intRow
		//    grd.Text = strText
		//End Sub
	}
}
