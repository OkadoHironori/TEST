using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//
using CTAPI;
using CT30K.Common;


namespace CT30K
{

    //追加2014/10/07hata_v19.51反映
    public class modImageInfo
    {

#if !CommView   //'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

        //
        //   画像情報のバージョン文字列からバージョン番号を取得する  'v16.30/v17.00追加 byやまおか 2010/03/03
        //   [例] V17.15(文字列) → (Vを除く) → (小数2位は無視する) → 17.1(数字)
        //
        public static float GetImageInfoVerNumber(string VersionString)
        {

            float VerSng = 0;			//整数＋小数
            float VerInt = 0;			//整数部
            float VerDec = 0;			//小数部
            float VerDec1 = 0;			//小数１位
            //float VerDec2 = 0;			//小数２位(使用しない)
            string strVer = "";

            //VerSng = (object)Strings.Mid(VersionString, 2);			                            //(例)17.15
            //VerInt = Convert.ToSingle(VerSng % 100);			                                //(例)17.00
            //VerDec = (object)"0." + modFileIO.FSO.GetExtensionName(Convert.ToString(VerSng));	//(例)0.15
            //VerDec1 = Convert.ToSingle(Strings.Mid(Convert.ToString(VerDec), 1, 3));			//(例)0.10
            //VerDec2 = Convert.ToSingle(VerDec - VerDec1);			                            //(例)0.05
            strVer = Functions.Mid(VersionString, 2);                                       //(例)17.15
            VerSng = Convert.ToSingle(strVer);                                              //(例)17.15                       
            VerInt = Convert.ToSingle(VerSng % 100);			                            //(例)17.00

            //'v19.50 VerIntが繰り上がってたら-1する by長野 2013/11/21_2014/07/16hata_v19.51反映
            if ((VerInt - VerSng) >= 0.5)
            {
                VerInt = VerInt - 1;
            }            
           
            //VerDec = Convert.ToSingle("0." + Path.GetExtension(strVer));	                //(例)0.15
            VerDec = Convert.ToSingle("0" + Path.GetExtension(strVer));	                    //(例)0.15
            VerDec1 = Convert.ToSingle(Functions.Mid(Convert.ToString(VerDec), 1, 3));	    //(例)0.10
            //VerDec2 = Convert.ToSingle(VerDec - VerDec1);			                        //(例)0.05

            return (VerInt + VerDec1);			                                            //(例)17.1

        }

        //'
        //'   FPDゲインの文字列を取得する     'v18.00追加 byやまおか 2011/07/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        //'
        public static string GetImageInfoFpdGain(ref CTstr.IMAGEINFO Info)
        {
            string caption_gain = "";
            float info_version = 0;

            info_version = GetImageInfoVerNumber(Info.version.GetString());

            //'Ver17のときは付帯情報がゲインのindexしか持っていない。
            //'indexから、システムのリストを使ってゲインの文字列を作る。
            //'※注意※ 違うシステムで表示させると、違う文字列が表示されてしまう。
            if (info_version == 17)
            {
                //'Ver17のときは付帯情報がゲインのindexしか持っていない。
                caption_gain = modCT30K.GetFpdGainStr(Info.fpd_gain);
            }
            else if (info_version >= 18)
            {
                //'Ver18以降は付帯情報がゲインの文字列を持いるのでそのまま表示する。
                caption_gain = Convert.ToString(Info.fpd_gain_f);
            }
            else
            {
                //'それ以外のときは、表示しない。
                caption_gain = "";
            }

            //GetImageInfoFpdGain = caption_gain
            return caption_gain;
        }

        //'
        //'   FPD積分時間の文字列を取得する     'v18.00追加 byやまおか 2011/07/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        //'
        public static string GetImageInfoFpdInteg(ref CTstr.IMAGEINFO Info)
        {
            string caption_integ = "";
            float info_version = 0;

            info_version = GetImageInfoVerNumber(Info.version.GetString());

            //'Ver17のときは付帯情報がゲインのindexしか持っていない。
            //'indexから、システムのリストを使ってゲインの文字列を作る。
            //'※注意※ 違うシステムで表示させると、違う文字列が表示されてしまう。
            if (info_version == 17)
            {
                //'Ver17のときは付帯情報がゲインのindexしか持っていない。
                caption_integ = modCT30K.GetFpdIntegStr(Info.fpd_integ);
            }
            else if (info_version >= 18)
            {
                //'Ver18以降は付帯情報がゲインの文字列を持いるのでそのまま表示する。
                caption_integ = Convert.ToString(Info.fpd_integ_f);
            }
            else
            {
                //'それ以外のときは、表示しない。
                caption_integ = "";
            }

            return caption_integ;

        }

#else
#endif

    }

}
