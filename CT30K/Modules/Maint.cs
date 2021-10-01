using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

//
using CT30K.Properties;
using CTAPI;
using CT30K.Common;

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： Maint.bas                                                   */
    ///* 処理概要　　： ??????????????????????????????                              */
    ///* 注意事項　　： なし                                                        */
    ///* -------------------------------------------------------------------------- */
    ///* 適用計算機　： DOS/V PC                                                    */
    ///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
    ///* コンパイラ　： VB 6.0                                                      */
    ///* -------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
    ///*                                                                            */
    ///* V1.00       99/XX/XX    (TOSFEC) ????????   新規作成                       */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
	//internal static class Maint
	public static class Maint
	{
        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************

        // delete by 間々田 2003/07/17 Start
        //'ﾌｨﾙﾀ関数ﾊﾟﾗﾒｰﾀ                                  '**********************************************
        //Public FC1pr(1) As Long          'FC1ﾊﾟﾗﾒｰﾀ      *  FC*pr(0) ﾌｨﾙﾀ種類   0:lacks  1: shepp  2: Y = HighReso *
        //Public FC2pr(1) As Long          'FC2ﾊﾟﾗﾒｰﾀ      *  FC*pr(1) FFTｻｲｽﾞ   0:1024  1:2048  3:4096  *
        //Public FC3pr(1) As Long          'FC3ﾊﾟﾗﾒｰﾀ      '**********************************************
        //'ｵﾌｾｯﾄｽｷｬﾝ、高解像度ｶﾒﾗ用追加
        //Public FC4pr(1) As Long          'FC4ﾊﾟﾗﾒｰﾀ
        //Public FC5pr(1) As Long          'FC5ﾊﾟﾗﾒｰﾀ
        //Public FC6pr(1) As Long          'FC6ﾊﾟﾗﾒｰﾀ
        //'ﾌｨﾙﾀ種類追加
        //Public FC7pr(1) As Long          'FC7ﾊﾟﾗﾒｰﾀ
        //Public FC8pr(1) As Long          'FC8ﾊﾟﾗﾒｰﾀ
        //Public FC9pr(1) As Long          'FC9ﾊﾟﾗﾒｰﾀ
        //
        //'ﾌｨﾙﾀ関数ﾊﾟﾗﾒｰﾀ a
        //Public FC1a As Single
        //Public FC2a As Single
        //Public FC3a As Single
        //Public FC4a As Single
        //Public FC5a As Single
        //Public FC6a As Single
        //Public FC7a As Single
        //Public FC8a As Single
        //Public FC9a As Single
        // delete by 間々田 End

        //上記の変数を構造体にしてかつ配列扱いにする by 間々田 2003/07/11
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
        Private Type FCType
            Kind As Long    'ﾌｨﾙﾀ種類   0:lacks  1: shepp  2: Y = HighReso
            SIZE As Long    'FFTｻｲｽﾞ   1024, 2048, 4096
            a    As Single  'ﾌｨﾙﾀ関数ﾊﾟﾗﾒｰﾀ
        End Type
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
        public struct FCType
		{			
			public int Kind;	//ﾌｨﾙﾀ種類   0:lacks  1: shepp  2: Y = HighReso			
			public int SIZE;	//FFTｻｲｽﾞ   1024, 2048, 4096			
			public float a;	    //ﾌｨﾙﾀ関数ﾊﾟﾗﾒｰﾀ
		}

        //変更2014/10/07hata_v19.51反映
		//public static FCType[] FCRec = new FCType[10];
        public static FCType[] FCRec = new FCType[13];  //v18.00 変更 9→12 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

		public static int ConeAdjustFlg;    //コーンビーム調整用フラッグ　added by 山本　2002-8-31

#region //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //Public ViscomMaintFlg   As Long 'Viscomメンテナンスフラッグ　added by 山本　2006-12-27
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
#endregion


        //追加2014/10/07hata_v19.51反映
        //IPDOCINFO 構造体
        public struct IPDOCINFO
        {
            public int Width;
            public int Height;
            public int iClass;
        }
 

        //v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここから
        //*******************************************************************************
        //機　　能： FCデータを作成する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v18.00  2011/03/10 (電S1)間々田 新規作成
        //*******************************************************************************
        public static void MakeFCData(ref FCType theFCType, ref float[] fc, ref float[] FCn)
        {

            //パラメータ定数
            const float slope = 0.5f;
            //sheppフィルタの傾きに掛ける係数
            const double iw = 10.0;
            const double omegaA = 125.0 / 1024.0;
            const double omegaD = 250.0 / 1024.0;
            const double omegaR = 50.0 / 1024.0;

            //HighResoをより強調の強いﾌｨﾙﾀにしたい場合は"RR"を6,7等にする。HighResoをより強調の弱いﾌｨﾙﾀにしたい場合は"RR"を3,4等にする。
            const double RR = 5.0;

            int i = 0;

            int we = 0;
            we = theFCType.SIZE - 1;

            int wn = 0;
            //2014/11/13hata キャストの修正
            //wn = we / 2;
            wn = Convert.ToInt32(we / 2F);

            //FCデータ作成
            switch (theFCType.Kind)
            {

                case 0:
                    //Laks

                    for (i = 0; i <= wn; i++)
                    {
                        //2014/11/13hata キャストの修正
                        //fc[i] = theFCType.a * i / wn;
                        fc[i] = theFCType.a * i / (float)wn;
                        fc[we - i] = fc[i];
                    }


                    //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
                    for (i = 0; i <= wn; i++)
                    {
                        //2014/11/13hata キャストの修正
                        //FCn[i] = theFCType.a * i / wn;
                        FCn[i] = theFCType.a * i / (float)wn;
                    }

                    for (i = 0; i <= wn - 1; i++)
                    {
                        FCn[we - i] = FCn[i + 1];
                    }

                    break;
                //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

                case 1:
                    //Shepp

                    for (i = 0; i <= wn; i++)
                    {

                        if (i == 0)
                        {
                            fc[i] = 0;
                        }
                        else
                        {
                            //2014/11/13hata キャストの修正
                            //fc[i] =(float)(theFCType.a * slope * (1.0 / wn) * Math.Abs((2 * wn / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * i) / (2 * wn))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * i) / (2 * wn))) / ((ScanCorrect.Pai * i) / (2 * wn))), 2));
                            fc[i] = (float)(theFCType.a * slope * (1.0 / wn) * Math.Abs((2 * wn / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * i) / (double)(2 * wn))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * i) / (double)(2 * wn))) / ((ScanCorrect.Pai * i) / (double)(2 * wn))), 2));
                        }

                        fc[we - i] = fc[i];

                    }


                    //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
                    for (i = 0; i <= wn; i++)
                    {
                        if (i == 0)
                        {
                            FCn[i] = 0;
                        }
                        else
                        {
                            //2014/11/13hata キャストの修正
                            //FCn[i] = (float)(theFCType.a * slope * (1.0 / wn) * Math.Abs((2 * wn / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * i) / (2 * wn))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * i) / (2 * wn))) / ((ScanCorrect.Pai * i) / (2 * wn))), 2));
                            FCn[i] = (float)(theFCType.a * slope * (1.0 / wn) * Math.Abs((2 * wn / ScanCorrect.Pai) * Math.Sin((ScanCorrect.Pai * i) / (double)(2 * wn))) * Math.Pow(((Math.Sin((ScanCorrect.Pai * i) / (double)(2 * wn))) / ((ScanCorrect.Pai * i) / (double)(2 * wn))), 2));
                        }
                    }

                    for (i = 0; i <= wn - 1; i++)
                    {
                        FCn[we - i] = FCn[i + 1];
                    }

                    break;
                //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

                case 2:
                    //HighReso

                    for (i = 0; i <= wn; i++)
                    {

                        if (i < iw)
                        {
                            //fc[i] = theFCType.a * i / wn;
                            fc[i] = theFCType.a * i / (float)wn;
                        }
                        else if (i < (omegaD * we))
                        {
                            //2014/11/13hata キャストの修正
                            //fc[i] = (float)((theFCType.a * i / wn) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we), 2) / (Math.Pow((i - iw), 2) + Math.Pow((omegaA * we), 2)))));
                            fc[i] = (float)((theFCType.a * i / (double)wn) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we), 2) / (Math.Pow((i - iw), 2) + Math.Pow((omegaA * we), 2)))));
                        }
                        else
                        {
                            //2014/11/13hata キャストの修正
                            //fc[i] = (float)((theFCType.a * i / wn) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we), 2) / (Math.Pow((i - iw), 2) + Math.Pow((omegaA * we), 2)))) * Math.Exp(-System.Math.Log(2) * (Math.Pow((i - (omegaD * we)), 2) / Math.Pow((omegaR * we), 2))));
                            fc[i] = (float)((theFCType.a * i / (double)wn) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we), 2) / (Math.Pow((i - iw), 2) + Math.Pow((omegaA * we), 2)))) * Math.Exp(-System.Math.Log(2) * (Math.Pow((i - (omegaD * we)), 2) / Math.Pow((omegaR * we), 2))));
                        }

                        fc[we - i] = fc[i];

                    }


                    //v13.00追加 終端が0でないFC関数も作る(↓↓↓ここから↓↓↓) 2007/02/21 やまおか
                    for (i = 0; i <= wn; i++)
                    {
                        if (i < iw)
                        {
                            //FCn[i] = theFCType.a * i / wn;
                            FCn[i] = theFCType.a * i / (float)wn;
                        }
                        else if (i < (omegaD * we))
                        {
                            //2014/11/13hata キャストの修正
                            //FCn[i] = (float)((theFCType.a * i / wn) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we), 2) / (Math.Pow((i - iw), 2) + Math.Pow((omegaA * we), 2)))));
                            FCn[i] = (float)((theFCType.a * i / (double)wn) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we), 2) / (Math.Pow((i - iw), 2) + Math.Pow((omegaA * we), 2)))));
                        }
                        else
                        {
                            //2014/11/13hata キャストの修正
                            //FCn[i] = (float)((theFCType.a * i / wn) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we), 2) / (Math.Pow((i - iw), 2) + Math.Pow((omegaA * we), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((i - (omegaD * we)), 2) / Math.Pow((omegaR * we), 2))));
                            FCn[i] = (float)((theFCType.a * i / (double)wn) * (RR - (RR - 1.0) * (Math.Pow((omegaA * we), 2) / (Math.Pow((i - iw), 2) + Math.Pow((omegaA * we), 2)))) * Math.Exp(-Math.Log(2) * (Math.Pow((i - (omegaD * we)), 2) / Math.Pow((omegaR * we), 2))));
                        }
                    }

                    for (i = 0; i <= wn - 1; i++)
                    {
                        FCn[we - i] = FCn[i + 1];
                    }

                    break;
                //v13.00追加 終端が0でないFC関数も作る(↑↑↑ここまで↑↑↑) 2007/02/21 やまおか

                default:

                    //メッセージ表示：フィルタ種類エラー
                    //Interaction.MsgBox(CT30K.My.Resources.ResourceManager.GetString("str" + Convert.ToString(StringTable.IDS_FilterTypeError)));
                    //変更2014/11/18hata_MessageBox確認
                    //MessageBox.Show(CTResources.LoadResString(StringTable.IDS_FilterTypeError), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(CTResources.LoadResString(StringTable.IDS_FilterTypeError));
                    return;
            }

            //データがない場合の値
            for (i = we + 1; i <= fc.GetUpperBound(0); i++)
            {
                fc[i] = 0;
                FCn[i] = 0;
                //v13.00追加 2007/02/21 やまおか
            }

        }
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここまで


    }
}
