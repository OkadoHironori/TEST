using System;

namespace XrayCtrl
{
    internal static class Module1
    {
        public static int gTosCount = 0;
        public static cCtrlm mcCtrlm;
        public static cValue mcValue;
        //public LogSet           As Integer //削除 by 間々田 2006/04/17　未使用
        public static int EventValue;  //イベント開始/停止
    
        //public XrayValue_Flg    As Integer //削除 by 間々田 2006/04/17　未使用

        //public mcKevexCtrlm As cKevexCtrlm

        //public gz_Show As Integer  //削除 by 間々田 2006/04/17　未使用

        //2004-09-09 Shibui
        //***********************************************************
        //   定数宣言
        //***********************************************************
        public const int gcintMaxOBJ = 15000;  //最大フォーカス値
        public const int gcintMinOBJ = 0;      //最小フォーカス値
        public const int gcintMaxOBX = 1000;   //最大電子ビームのX方向位置
        public const int gcintMinOBX = -1000;  //最小電子ビームのX方向位置
        public const int gcintMaxOBY = 1000;   //最大電子ビームのX方向位置
        public const int gcintMinOBY = -1000;  //最小電子ビームのX方向位置
//追加2009/08/19(KSS)hata L10801 ---------->
        public const int gcintMaxOBJL10801 = 23000;  //最大フォーカス値
        public const int gcintMinOBJL10801 = 0;      //最小フォーカス値
        public const int gcintMaxOBXL10801 = 1200;   //最大電子ビームのX方向位置
        public const int gcintMinOBXL10801 = -1200;  //最小電子ビームのX方向位置
        public const int gcintMaxOBYL10801 = 1200;   //最大電子ビームのX方向位置
        public const int gcintMinOBYL10801 = -1200;  //最小電子ビームのX方向位置
//追加2009/08/19(KSS)hata L10801 ----------<

//追加2015/11/08(検S1)長野 L12721 ---------->
        public const int gcintMaxOBJL12721 = 30000;  //最大フォーカス値
        public const int gcintMinOBJL12721 = 0;      //最小フォーカス値
        public const int gcintMaxOBXL12721 = 1200;   //最大電子ビームのX方向位置
        public const int gcintMinOBXL12721 = -1200;  //最小電子ビームのX方向位置
        public const int gcintMaxOBYL12721 = 1200;   //最大電子ビームのX方向位置
        public const int gcintMinOBYL12721 = -1200;  //最小電子ビームのX方向位置
//追加2015/11/08(検S1)長野 L12721 ----------<

//追加2015/10/05(KS1)長野 L10711 ---------->
        public const int gcintMaxOBJL10711 = 24000;  //最大フォーカス値
        public const int gcintMinOBJL10711 = 0;      //最小フォーカス値
        public const int gcintMaxOBXL10711 = 1000;   //最大電子ビームのX方向位置
        public const int gcintMinOBXL10711 = -1000;  //最小電子ビームのX方向位置
        public const int gcintMaxOBYL10711 = 1000;   //最大電子ビームのX方向位置
        public const int gcintMinOBYL10711 = -1000;  //最小電子ビームのX方向位置
        public const int gcintMaxCAXL10711 = 1000;   //最大電子ビームのX方向位置(コンデンサ)
        public const int gcintMinCAXL10711 = -1000;  //最小電子ビームのX方向位置(コンデンサ)
        public const int gcintMaxCAYL10711 = 1000;   //最大電子ビームのY方向位置(コンデンサ)
        public const int gcintMinCAYL10711 = -1000;  //最小電子ビームのY方向位置(コンデンサ)
//追加2015/10/05(KS1)長野 L10711 ----------<

        //追加 by 間々田 2006/04/17 tool1.basから移動してきた
        //Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)

        //削除ここから by 間々田 2006/04/17　無意味
        //public Sub Logic()
        //
        //End Sub
        //
        ////=================================================
        ////小数点一桁以降切り捨て
        ////
        //public Function fixPlus01(val As Single) As Single
        //Dim buf1 As Integer
        //Dim buf2 As Single
        //Dim buf3 As Single
        //
        //    buf1 = val / 0.1
        //    fixPlus01 = buf1 * 0.1
        //End Function
        //削除ここまで by 間々田 2006/04/17


        //public Sub logOut(ByVal theString As String)
        //
        ////#If DebugOn Then
        //
        //    Dim fileNo As Integer
        //
        //    fileNo = FreeFile()
        //    Open "c:\ct\command\CT30kDebugTmp.txt" For Append As fileNo
        //
        //    Print #fileNo, Now & " FeinFocus.exe : " & theString
        //
        //    Close fileNo
        //
        ////#End If
        //
        //End Sub
    }
}
