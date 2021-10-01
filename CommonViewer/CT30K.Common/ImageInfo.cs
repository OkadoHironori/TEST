using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CTAPI;

namespace CT30K.Common
{

    public class ImageInfo
    {
        /// <summary>
        /// 構造体データ
        /// </summary>
        public CTstr.IMAGEINFO Data;
 

        //
        //   画像情報の読み込み
        //
		public static bool ReadImageInfo(ref CTstr.IMAGEINFO  ImageInfoRec, string FileName, string Extension = ".inf")
		{
            //戻り値
			bool functionReturnValue = false;

			//short fileNo = 0;

			//戻り値初期化
			//functionReturnValue = false;

            ////エラー時の扱い
            // // ERROR: Not supported in C#: OnErrorStatement


            ////画像情報ファイルオープン
            ////fileNo = FreeFile();
            //FileSystem.FileOpen(fileNo, FileName + Extension, OpenMode.Binary, OpenAccess.Read);
            //FileStream(fileName, FileMode.Open, FileAccess.Read);

            //FileSystem.FileGet(fileNo, ImageInfoRec);

            ////画像情報ファイルクローズ
            //FileSystem.FileClose(fileNo);

            ////戻り値セット
            //functionReturnValue = true;
            //ExitHandler:
            //return functionReturnValue;


            // 画像情報ファイルの読み込み
            if (!Functions.ReadStructure(FileName + Extension, ref ImageInfoRec)) 
            {
                return functionReturnValue;
            }

            //戻り値セット
            functionReturnValue = true;
            return functionReturnValue;

		}

        //
        //   画像情報の書き込み
        //
		public static bool WriteImageInfo(ref CTstr.IMAGEINFO ImageInfoRec, string FileName, string Extension = ".inf")
		{
            //戻り値
            bool functionReturnValue = false;

            //short fileNo = 0;

            ////戻り値初期化
            //functionReturnValue = false;

            ////エラー時の扱い
            // // ERROR: Not supported in C#: OnErrorStatement


            ////画像情報ファイルオープン
            //fileNo = FreeFile();
            //FileSystem.FileOpen(fileNo, FileName + Extension, OpenMode.Binary, OpenAccess.Write);

            //FileSystem.FilePut(fileNo, ImageInfoRec);

            ////画像情報ファイルクローズ
            //FileSystem.FileClose(fileNo);

            ////戻り値セット
            //functionReturnValue = true;
            //ExitHandler:
            //return functionReturnValue;

            if (! Functions.WriteStructure( FileName + Extension, ImageInfoRec)) 
            {
                return functionReturnValue;
            }

			//戻り値セット
			functionReturnValue = true;
			return functionReturnValue;

		}


        //CT30KのmodImageInfoへ移動_2014/10/07hata
        ////
        ////   画像情報のバージョン文字列からバージョン番号を取得する  'v16.30/v17.00追加 byやまおか 2010/03/03
        ////   [例] V17.15(文字列) → (Vを除く) → (小数2位は無視する) → 17.1(数字)
        ////
        //public static float GetImageInfoVerNumber(string VersionString)
        //{

        //    float VerSng = 0;			//整数＋小数
        //    float VerInt = 0;			//整数部
        //    float VerDec = 0;			//小数部
        //    float VerDec1 = 0;			//小数１位
        //    //float VerDec2 = 0;			//小数２位(使用しない)
        //    string strVer = "";

        //    //VerSng = (object)Strings.Mid(VersionString, 2);			                            //(例)17.15
        //    //VerInt = Convert.ToSingle(VerSng % 100);			                                //(例)17.00
        //    //VerDec = (object)"0." + modFileIO.FSO.GetExtensionName(Convert.ToString(VerSng));	//(例)0.15
        //    //VerDec1 = Convert.ToSingle(Strings.Mid(Convert.ToString(VerDec), 1, 3));			//(例)0.10
        //    //VerDec2 = Convert.ToSingle(VerDec - VerDec1);			                            //(例)0.05

        //    strVer = Functions.Mid(VersionString, 2);                                       //(例)17.15
        //    VerSng = Convert.ToSingle(strVer);                                              //(例)17.15
        //    VerInt = Convert.ToSingle(VerSng % 100);			                            //(例)17.00

        //    //'v19.50 VerIntが繰り上がってたら-1する by長野 2013/11/21_2014/10/07hata_v19.51反映
        //    if ((VerInt - VerSng) >= 0.5)
        //    {
        //        VerInt = VerInt - 1;
        //    }
            
        //    //VerDec = Convert.ToSingle("0." + Path.GetExtension(strVer));	                //(例)0.15
        //    VerDec = Convert.ToSingle("0" + Path.GetExtension(strVer));	                    //(例)0.15
        //    VerDec1 = Convert.ToSingle(Functions.Mid(Convert.ToString(VerDec), 1, 3));	    //(例)0.10
        //    //VerDec2 = Convert.ToSingle(VerDec - VerDec1);			                        //(例)0.05
            
        //    return (VerInt + VerDec1);			                                            //(例)17.1

        //}
    



    }
}
