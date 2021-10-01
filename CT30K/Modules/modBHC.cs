using System;
using System.Runtime.InteropServices;
//
using CTAPI;
using CT30K.Common;

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロCT　　　　　　　　　　　　　　　　                  */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： modBHC.bas                                                  */
    ///* 処理概要　　： BHC処理関連                                                 */
    ///* 注意事項　　： なし                                                        */
    ///* -------------------------------------------------------------------------- */
    ///* 適用計算機　： DOS/V PC                                                    */
    ///* ＯＳ　　　　： WindowsXP (SP2)                                             */
    ///* コンパイラ　： VB 6.0 (SP5)                                                */
    ///* -------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
    ///*                                                                            */
    ///* V19.00      12/02/20    H.Nagai             新規作成
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2012                 */
    ///* ************************************************************************** */
	internal static class modBHC
	{

        //CTAPI.IICorrectを使う
        //
        // IICorrect.dll
        //
        //[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int GetMaxRawDataValue(string FileName, string RawFileName, ref float dia, ref float max_raw); //v8.0追加 by Murata 2007/1/09 v8.1変更 by Ohkado 2007/04/11
        //[DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]		
        //public static extern int calculate_bhc_fitting(int file_num, ref float p, ref float dia, ref float a);              //v8.0追加 by Ohkado 2007/1/12

        //Rev26.00 ファントムレスBHC材質用配列
        public static string[] BHCMaterial;//材質名
        public static int BHCmatnum = 0;//材質数
        public static int[] BHCmethod;//BHC手法[0:ファントムレス,1:従来BHC]

        public static void InitBHCMaterialList(string FileName)
        {            
            string s;
            string[] fields = new string [3];
            string[] matnum = new string[64];
            int[] method = new int[64];
            int counter = 0;
            int ii = 0;

            System.IO.StreamReader file = new System.IO.StreamReader(FileName, System.Text.Encoding.GetEncoding("Unicode"));

            s = file.ReadLine(); //1行目飛ばし

            while ((s = file.ReadLine()) != null)
            {
                fields = s.Split(',');
                matnum[counter] = fields[1];
                method[counter] = int.Parse(fields[2]);
                counter++;
            }

            BHCMaterial = new string[counter];
            BHCmethod = new int[counter];
            BHCmatnum = counter;

            while ( ii < BHCmatnum)
            {
                BHCMaterial[ii] = matnum[ii];
                BHCmethod[ii] = method[ii];
                ii++;
            }
            
//            Array.Sort(BHCMaterial);

            file.Close();
        }        
    }
}
