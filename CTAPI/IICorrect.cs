using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CTAPI
{
    public class IICorrect
    {
        #region API
        //-----------------------------------------------------------------------------
        // API関数
        //-----------------------------------------------------------------------------
        /// <summary>
        /// フラットパネルの透視画像をゲイン補正する
        /// </summary>
        /// <param name="RAW_IMAGE"></param>
        /// <param name="GAIN_IMAGE"></param>
        /// <param name="H_SIZE"></param>
        /// <param name="V_SIZE"></param>
        /// <param name="adv"></param>
        [DllImport("IICorrect.dll")]
        public static extern void FpdGainCorrect(
            ushort[] RAW_IMAGE,		//フラットパネル原画像
            ushort[] GAIN_IMAGE,	//フラットパネルゲイン画像
            int H_SIZE,				//画像横サイズ
            int V_SIZE,				//画像縦サイズ
            int adv					//ゲイン校正時のかさ上げ量
        );

        /// <summary>
        /// フラットパネルの欠陥を補間する
        /// </summary>
        /// <param name="FPD_IMAGE"></param>
        /// <param name="Def_IMAGE"></param>
        /// <param name="H_SIZE"></param>
        /// <param name="V_SIZE"></param>
        /// <param name="vs"></param>
        /// <param name="ve"></param>
        [DllImport("IICorrect.dll")]
        public static extern void FpdDefCorrect_short(
            ushort[] FPD_IMAGE,		//フラットパネル原画像
            ushort[] Def_IMAGE,		//フラットパネル欠陥画像　0:正常画素　65535:欠陥画素
            int H_SIZE,				//画像横サイズ
            int V_SIZE,				//画像縦サイズ
            int vs,					//画像取込み開始y座標
            int ve 					//画像取込み終了y座標
        );



        //////////////////////////////////////////////////
        //追加 2013/05/15
        //
        // ref と string と　Image　に注意。チェックする。
        // 配列、参照型　等
        //
        //////////////////////////////////////////////////

        //画像を除算する（積分用）
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void DivImage(
            ref int M_IMAGE, 
            ref short L_IMAGE, 
            int h_size, 
            int v_size, 
            int N
        );

        //画像を除算する（積分用）
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void DivImage(
            ref int M_IMAGE,
            ref ushort L_IMAGE,
            int h_size,
            int v_size,
            int N
        );


        //画像白黒反転関数   'changed by 山本　2003-10-28　引数にadvを追加
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void InverseImage(
            ref short Image, 
            int h_size, 
            int v_size, 
            int adv
        );

        //？
        //v7.0 追加 by 間々田 2003/09/25
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void ChangeImageSize_HV(
            ref short IN_IMAGE, 
            ref short OUT_IMAGE, 
            int h_size, 
            int v_size, 
            float mag_h, 
            float mag_v
        );


        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetMaxRawDataValue(
            string FileName, 
            string RawFileName, 
            ref float dia, 
            ref float max_raw
        );

        //？
        //v8.0追加 by Murata 2007/1/09 v8.1変更 by Ohkado 2007/04/11
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int calculate_bhc_fitting(
            int file_num, 
            ref float p, 
            ref float dia, 
            ref float a
        );
        //v8.0追加 by Ohkado 2007/1/12

        
        //********************************************************************************
        //  AutoPosにあった関数
        //********************************************************************************
        //微調テーブル＆昇降位置の最適位置取得
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int AutoFinetableSet_Trans(
            ref short beforeImage, 
            ref short afterImage,
            ref CTstr.PosSIZE imageSize,
            ref CTstr.RECTROI roiRect,
            ref CTstr.FineTable curFTpos,
            ref CTstr.SampleTable curTablePos,
            ref CTstr.Detector_Info detectorInfo, 
            double rotationAngle, 
            float rcPos, 
            float scanpos,
            int movePixel, 
            float moveRefPix,
            ref CTstr.FineTable optFTpos, 
            ref float optUD, 
            ref float fod
        );

        //微調＆昇降の位置修正とFCD＆光軸直交方向の最適位置取得
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int AutoTableSet_Trans(
            ref CTstr.PosSIZE imageSize,
            ref CTstr.RECTROI roiRect,
            ref CTstr.SampleTable curTablePos,
            ref CTstr.Detector_Info detectorInfo,
            ref CTstr.FineTable curFTpos,
            ref CTstr.FineTable optFTpos, 
            float optUD, 
            float fod, 
            float scanpos,
            ref CTstr.SampleTable optTablePos1st,
            ref CTstr.SampleTable optTablePos2nd
        );

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern double Determinant2C(
            ref double BUFF, 
            short Mx_a
        );

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern double Determinant3C(
            ref double 
            BUFF, 
            short Mx_a
        );

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern double Determinant4C(
            ref double BUFF, 
            short Mx_a
        );

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void DrawFittingCurve(
            ref byte Image, 
            short h_mag, 
            short v_mag, 
            int h_size, 
            int v_size, 
            double a0, 
            double A1, 
            double a2, 
            double a3, 
            double a4,
            double a5, 
            double W_DISTANCE, 
            int VRT_Count, 
            ref double xi, 
            ref double ti
        );

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ImageOpen(
            ref short Image, 
            string FileName, 
            int h_size, 
            int v_size
        );

        //仮設定(ushortを追加)　//2014/03/10hata
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ImageOpen(
            ref ushort Image,
            string FileName,
            int h_size,
            int v_size
        );
        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ImageSave(
            ref short Image, 
            string FileName, 
            int h_size, 
            int v_size
        );

        //仮設定(ushortを追加)　//2014/03/10hata
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ImageSave(
            ref ushort Image,
            string FileName,
            int h_size,
            int v_size
        );

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ImageOpen_long(
            ref int Image, 
            string FileName, 
            int h_size, 
            int v_size
        );         //v17.00added by 山本　2009-10-19

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int ImageSave_long(
        //    ref int Image, 
        //    string FileName, 
        //    int h_size, 
        //    int v_size
        //);        //v17.00added by 山本　2009-10-19
        public static extern int ImageSave_long(
            ref uint Image,
            string FileName,
            int h_size,
            int v_size
        );        //v17.00added by 山本　2009-10-19
        //追加 by 山本 2000-11-18

        //Rev25.00 追加 by長野 long型の画像を内部でushort型に変えて保存 2016/08/17
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern int ImageSave_long(
        //    ref int Image, 
        //    string FileName, 
        //    int h_size, 
        //    int v_size
        //);        //v17.00added by 山本　2009-10-19
        public static extern int ImageSave_longToUshort(
            ref uint Image,
            string FileName,
            int h_size,
            int v_size
        );        //v17.00added by 山本　2009-10-19
        //追加 by 山本 2000-11-18

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int DoubleImageOpen(
            ref double Image, 
            string FileName, 
            int h_size, 
            int v_size
        );

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int DoubleImageSave(
            ref double Image, 
            string FileName, 
            int h_size, 
            int v_size
        );

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ImageCopyDtoUS(
            ref double D_IMAGE, 
            ref short Image, 
            int h_size, 
            int v_size
        );

        //画像の最大値・最小値算出
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void GetMaxMin(
            ref short IN_BUFF,
            int h_size,
            int v_size,
            ref int Min,
            ref int Max
        );
        
        //仮設定(ushortを追加)　//2014/03/10hata
        //画像の最大値・最小値算出
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void GetMaxMin(
            ref ushort IN_BUFF, 
            int h_size, 
            int v_size, 
            ref int Min, 
            ref int Max
        );

        ////////added by 山本 2000-2-5
        //画像サイズ変更
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void ChangeImageSize(
            ref short IN_IMAGE, 
            ref short OUT_IMAGE, 
            int h_size, 
            int v_size
        );        //第1引数の型変更 by 間々田 2003/09/26
        ////////added by 山本 2002-3-7

        //回転中心画素算出
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetRotationCenterPixelValue_C(
            ref ushort Image, 
            ref short bile_image, 
            ref double CenterPixel, 
            int h_size, 
            int View
        );

        //自動スキャン位置設定関数（断面画像上ROI指定）
        //追加 by 間々田 2009/07/09
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int auto_tbl_set(
            ref int RoiCircle, 
            float ideal_ftable_h_pos, 
            float ideal_ftable_v_pos, 
            float real_ftable_h_pos, 
            float real_ftable_v_pos, 
            ref float FCD1, 
            ref float table_h_xray1, 
            ref float FCD2, 
            ref float table_h_xray2
        );

        //追加 by 間々田 2009/07/09
        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int auto_ftbl_set(
            ref int RoiCircle, 
            ref float ftable_h_pos, 
            ref float ftable_v_pos
        );

        //２値化画像表示
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void BinarizeImage(
            ref ushort IN_IMAGE, 
            ref short OUT_IMAGE, 
            int h_size, 
            int v_size, 
            int Threshold, 
            float HMAG_N, 
            float VMAG_N
        );                //changed by 山本　2003-10-16　VMAG_NとHMAG_NをLongからSingleに変更

        //２値化画像表示
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void BinarizeImage_signed(
            ref short IN_IMAGE, 
            ref short OUT_IMAGE, 
            int h_size, 
            int v_size, 
            int Threshold, 
            float HMAG_N, 
            float VMAG_N
        );         //v9.7追加 by 間々田 2004-12-09 符号付Short型配列対応
        
        //追加
        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void FpdDefDetect(
            ref short GAIN_IMAGE, 
            ref double OFF_IMAGE, 
            ref short Def_IMAGE, 
            int h_size, 
            int v_size, 
            float thre, 
            float thre_n, 
            float thre_line, 
            int BlockSize
        );

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void Cal_Mean_short(
            //画像は配列とする→  short[] RAW_IMAGE 
            ushort[] RAW_IMAGE, 
            int h_size, 
            int v_size, 
            ref float mean
        );

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void Cal_Mean_short2(
            ref ushort RAW_IMAGE, 
            int h_size, 
            int v_size, 
            int h_trim, 
            int v_trim, 
            ref float mean
        );

        //Rev25.00 追加 by長野 2016/08/17
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void Cal_Mean_short2_Long(ref uint RAW_IMAGE, int h_size, int v_size, int h_trim, int v_trim, ref float mean);

        //Rev25.00 追加 by長野 2016/08/17
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void Cal_Median_Long(ref uint RAW_IMAGE, int h_size, int v_size, int h_trim, int v_trim, ref float mean);

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void Cal_Max_short(
            ref short RAW_IMAGE, 
            int h_size, 
            int v_size, 
            ref short Max
        );        //added by 山本　2005-12-2

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void Cal_Mean_double(
            ref double RAW_IMAGE, 
            int h_size, 
            int v_size, 
            ref float mean);        //v17.00added by 山本　2009-10-09

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ImageCopy_short(ref short IN_IMAGE, 
            ref short OUT_IMAGE, 
            int h_size, 
            int v_size
        );        //v17.00added by 山本　2010-03-03    'v17.02宣言修正 byやまおか 2010-07-12

        //Rev20.00 追加 by長野 2015/02/06
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ImageCopy_short_cut(ref ushort IN_IMAGE, 
            ref ushort OUT_IMAGE,
            int h_size,
            int v_size,
            int cut_h,
            int cut_v
        );//v17.00added by 山本　2010-03-03    'v17.02宣言修正 byやまおか 2010-07-12

        //コントラスト向上(画像データを符号付きで受け取る）
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void ChangeFullRange_Short(
            ref short IN_BUFF, 
            int hSize, 
            int vSize, 
            int Min, 
            int Max, 
            float GAMMA
        );

        //コントラスト向上(画像データを符号付きで受け取る）
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void ChangeFullRange_UShort(
            ref short IN_BUFF, 
            int hSize, 
            int vSize, 
            int Min, 
            int Max
        );        //v17.20追加 透視画像16bit対応 byやまおか 2010/09/16


        //仮設定(ushortを追加)　//2014/03/10hata
        //コントラスト向上(画像データを符号付きで受け取る）
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void ChangeFullRange_UShort(
            ref ushort IN_BUFF,
            int hSize,
            int vSize,
            int Min,
            int Max
        );

        //Rev25.00 ラインプロファイルデータ取得 追加 by長野 2016/08/08
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void GetLProfileVH(
            ushort[] RAW_IMAGE,
            ushort[] v_prof,
            ushort[] h_prof,
            int h_size,
            int v_size,
            int v_pos,
            int h_pos
        );

        //v10.2追加 by 間々田 2005/06/23 
        //ChangeFullRange_Shortによる変換の逆の関数
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void ChangeToCTImage(
            ref short IN_BUFF, 
            int hSize, 
            int vSize, 
            int Min, 
            int Max
        );        //v11.2追加ここから by 間々田 2005/10/12

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int calculate_fulldistortion_fitting(
            ref int kmax, 
            ref double x, 
            ref double y, 
            ref double area, 
            int hSize, 
            int vSize, 
            double dPitch, 
            int kv, 
            ref double alk, 
            ref double blk,
            ref int ist, 
            ref int ied, 
            ref int jst, 
            ref int jed, 
            ref double G, 
            ref double h
        );

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int Cal_1d_Distortion_Parameter(
            int kv, 
            int h_size, 
            int v_size, 
            double a_0, 
            double b_0, 
            ref double alk, 
            ref double blk, 
            int kmax, 
            ref double x, 
            ref double y,
            ref double G, 
            ref double h, 
            ref double a0_bar, 
            ref double b0_bar, 
            ref int xls, 
            ref int xle, 
            ref double a0, 
            ref double A1, 
            ref double a2, 
            ref double a3,
            ref double a4, 
            ref double a5
        );

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void make_2d_fullindiv_table(
            int hSize, 
            int vSize, 
            int ist, 
            int ied, 
            int jst, 
            int jed, 
            int kv, 
            ref double alk, 
            ref double blk, 
            ref float gi,
            ref float git, 
            ref float gj, 
            ref float gjt, 
            ref int Qidjd, 
            ref int Qidp1jd, 
            ref int Qidjdp1, 
            ref int Qidp1jdp1
        );

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void cone_fullindiv_crct(
            int hSize, 
            int ist, 
            int ied, 
            int jst, 
            int jed, 
            ref float gi, 
            ref float git, 
            ref float gj, 
            ref float gjt, 
            ref int Qidjd,
            ref int Qidp1jd, 
            ref int Qidjdp1, 
            ref int Qidp1jdp1, 
            ref short gDat, 
            ref short pDat
        );

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void Zero_ImageMargin(
            ref short BUFF, 
            int h_size, 
            int v_size, 
            int h_margin, 
            int v_margin
        );        //v11.2追加 by 間々田 2005/11/11

        //？
        [DllImport("IICorrect.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void Print_CenterPoint(
            ref short Image, 
            int h_size, 
            int v_size, 
            int kmax, 
            ref double x, 
            ref double y
        );        //v11.2追加 by 山本　2005-11-29
        //v11.2追加ここまで by 間々田 2005/10/12


        #endregion
    }
}
