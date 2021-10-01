using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
//


namespace CTAPI
{
    /// <summary>
    /// ImgProc.dll
    /// </summary>
    public class ImgProc
    {
        #region API
         //-----------------------------------------------------------------------------
        // API関数
        //-----------------------------------------------------------------------------
        private const string DLL_PATH = @"ImageProcNew.dll";
       /// <summary>
        /// ルックアップテーブル生成
        /// </summary>
        /// <param name="LT"></param>
        /// <param name="LT_SIZE"></param>
        /// <param name="wLevel"></param>
        /// <param name="wWidth"></param>
        //[DllImport("ImgProc.dll")]
        [DllImport(DLL_PATH)]
        public static extern void MakeLT(
	        byte[] LT,				//階調変換用ルックアップテーブル
	        int LT_SIZE,			//ルックアップテーブルサイズ		
	        int wLevel,				//階調変換ウィンドウレベル
	        int wWidth				//階調変換ウィンドウ幅
        );

        /// <summary>
        /// ルックアップテーブルによるバイト変換（符号付配列→バイト）
        /// </summary>
        /// <param name="IN_IMAGE"></param>
        /// <param name="OUT_IMAGE"></param>
        /// <param name="IMAGE_SIZE"></param>
        /// <param name="LT"></param>
        /// <param name="LT_SIZE"></param>
        /// <param name="offset"></param>
        //[DllImport("ImgProc.dll")]
        [DllImport(DLL_PATH)]
        public static extern void ConvertShortToByte(
            short[] IN_IMAGE,       //入力画像
            byte[] OUT_IMAGE,       //出力画像
            int IMAGE_SIZE,         //画像サイズ
            byte[] LT,              //階調変換用ルックアップテーブル
            int LT_SIZE,            //ルックアップテーブルサイズ
            int offset              //オフセット値
        );
         
        /// <summary>
        /// ルックアップテーブルによるバイト変換（符号なし配列→バイト）
        /// </summary>
        /// <param name="IN_IMAGE"></param>
        /// <param name="OUT_IMAGE"></param>
        /// <param name="IMAGE_SIZE"></param>
        /// <param name="LT"></param>
        //[DllImport("ImgProc.dll")]
        [DllImport(DLL_PATH)]
        public static extern void ConvertWordToByte(
            ushort[] IN_IMAGE,		//入力画像			
            byte[] OUT_IMAGE,		//出力画像			
            int IMAGE_SIZE,		    //画像サイズ 
            byte[] LT				//階調変換用ルックアップテーブル
        );

        /// <summary>
        /// ルックアップテーブルによるバイト変換（符号なし配列→バイト、ミラーリング）
        /// </summary>
        /// <param name="IN_IMAGE"></param>
        /// <param name="OUT_IMAGE"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="LT"></param>
        //[DllImport("ImgProc.dll")]
        [DllImport(DLL_PATH)]
        public static extern void ConvertWordToByteMirror(
            ushort[] IN_IMAGE,	    //入力画像			
            byte[] OUT_IMAGE,	    //出力画像			
            int width,		        //画像サイズ 
            int height,		        //画像サイズ 
            byte[] LT			    //階調変換用ルックアップテーブル
        );

        /// <summary>
        /// バイトデータをビットマップにセットする
        /// </summary>
        /// <param name="hBitmap"></param>
        /// <param name="IMAGE"></param>
        //[DllImport("ImgProc.dll")]
        [DllImport(DLL_PATH)]
        public static extern void SetByteToBitmap(
            IntPtr hBitmap,	    //ビットマップハンドル
            byte[] IMAGE		//バイト配列
        );

        /// <summary>
        /// バイト画像データ反転
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="imageSize"></param>
        //[DllImport("ImgProc.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void ReverseByteImage(ref byte Image, int imageSize);

        /// <summary>
        /// ワード画像データ反転
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="imageSize"></param>
        //[DllImport("ImgProc.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //[DllImport(DLL_PATH, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern void ReverseWordImage(ref short Image, int imageSize);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void ReverseWordImage(ref ushort Image, int imageSize);


        //ワード画像データ左右反転       'v17.50追加 2011/02/04 by 間々田
        //[DllImport("ImgProc.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //[DllImport(DLL_PATH, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //public static extern void ConvertMirror(ref short Image, int Width, int Height);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void ConvertMirror(ref ushort Image, int Width, int Height);

        /// <summary>
        /// アベレージングを行う
        /// </summary>
        /// <param name="LIVE_IMAGE"></param>
        /// <param name="M_IMAGE"></param>
        /// <param name="SIZE"></param>
        /// <param name="AveNum"></param>
        //[DllImport("ImgProc.dll")]
        [DllImport(DLL_PATH)]
        public static extern void Averaging(
            ushort[] LIVE_IMAGE,	//ライブ画像			
            ushort[] M_IMAGE,		//アベレージング画像	
            int SIZE,				//画像サイズ
            int AveNum              //アベレージ枚数
        );

        /// <summary>
        /// シャープ
        /// </summary>
        /// <param name="IN_IMAGE"></param>
        /// <param name="OUT_IMAGE"></param>
        /// <param name="H_SIZE"></param>
        /// <param name="V_SIZE"></param>
        /// <param name="maxValue"></param>
        //[DllImport("ImgProc.dll")]
        [DllImport(DLL_PATH)]
        public static extern void Sharpen(
            ushort[] IN_IMAGE,		//入力画像
            ushort[] OUT_IMAGE,		//出力画像
            int H_SIZE,			    //画像横サイズ
            int V_SIZE,				//画像縦サイズ
            int maxValue			//画像の最大輝度値
        );

        /// <summary>
        /// 画像に空間フィルタをかける関数
        /// </summary>
        /// <param name="IN_IMAGE"></param>
        /// <param name="OUT_IMAGE"></param>
        /// <param name="Filter"></param>
        /// <param name="H_SIZE"></param>
        /// <param name="V_SIZE"></param>
        /// <param name="K_Size"></param>
        //[DllImport("ImgProc.dll")]
        [DllImport(DLL_PATH)]
        public static extern void SpatialFilter(
            ushort[] IN_IMAGE,		//入力画像		
            ushort[] OUT_IMAGE,		//出力画像		
            float[] Filter,			//空間フィルタ
            int H_SIZE,				//画像横サイズ
            int V_SIZE,				//画像縦サイズ
            int K_Size				//カーネルサイズ（空間フィルタサイズ）
        );

        /// <summary>
        /// 画像を積算する（積分処理用）
        /// </summary>
        /// <param name="IMAGE"></param>
        /// <param name="SUM_IMAGE"></param>
        /// <param name="SIZE"></param>
        //[DllImport("ImgProc.dll")]
        [DllImport(DLL_PATH)]
        public static extern void AddImageWord(
            ushort[] IMAGE,					//画像		
            int[] SUM_IMAGE,				//積算画像
            int SIZE						//画像サイズ
        );

        /// <summary>
        /// 画像を積算する（積分処理用）
        /// </summary>
        /// <param name="IMAGE"></param>
        /// <param name="SUM_IMAGE"></param>
        /// <param name="SIZE"></param>
        //[DllImport("ImgProc.dll")]
        [DllImport(DLL_PATH)]
        public static extern void AddImageWord(
            short[] IMAGE,					//画像		
            int[] SUM_IMAGE,				//積算画像
            int SIZE						//画像サイズ
        );


        //現在使っていない
        /*　
        /// <summary>
        /// 和画像
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        //[DllImport("ImgProc.dll")]
        [DllImport("ImageProcNew.dll")]
        public static extern bool AddCTImage(string files);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        //[DllImport("ImgProc.dll")]
        [DllImport("ImageProcNew.dll")]
        public static extern bool SubCTImage(string file1, string file2);
        

        /// <summary>
        /// マルチフレーム処理
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        //[DllImport("ImgProc.dll")]
        [DllImport(DLL_PATH)]
        public static extern bool MultiFrame(string files);

        /// <summary>
        /// 拡大処理
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        //[DllImport("ImgProc.dll")]
        [DllImport(DLL_PATH)]
        public static extern bool Enlarge(string file, Winapi.RECT rect);
         */

        //--------------------------------------
        //  ProcOCXにあったものをDll化
        //--------------------------------------
        /// <summary>
        /// ＲＯＩ処理（Roi）メソッド
        /// 指定範囲の画素平均，標準偏差，面積(mm)を求めます。
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "CtRoi")]
        public static extern bool CtRoi(ref double OutArea, ref double OutAve, ref double OutSd);

        ///// <summary>
        ///// test処理
        ///// </summary>
        //[DllImport(DLL_PATH, EntryPoint = "testSum")]
        //public static extern int testSum(int A, int B);

        ///// <summary>
        ///// test処理
        ///// </summary>
        //[DllImport(DLL_PATH, EntryPoint = "testKakezan")]
        //public static extern bool testKakezan(ref double A, ref double B);

        /// <summary>
        /// 画像表示
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "DispImage")]
        //public static extern bool DispImage([MarshalAs(UnmanagedType.LPStr)] string FileName);
        public static extern bool DispImage(string FileName);

        /// <summary>
        /// 画像表示
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "DispBitImage")]
        //public static extern bool DispBitImage(int Low, int High, [MarshalAs(UnmanagedType.LPStr)] string FileName);	//v10.2変更 by 間々田 2005-06-14													
        public static extern bool DispBitImage(int Low, int High, string FileName);	//v10.2変更 by 間々田 2005-06-14													


        /// <summary>
        /// マルチフレーム処理メソッド
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "CtMultiFrame")]
        public static extern bool CtMultiFrame([MarshalAs(UnmanagedType.LPStr)] string FileNameList);
        
        /// <summary>
        /// 画像の加算演算メソッド
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "CtAdd")]
        public static extern bool CtAdd([MarshalAs(UnmanagedType.LPStr)] string FileNameList);

        /// <summary>
        /// 画像の減算演算メソッド
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "CtSub")]
        public static extern bool CtSub([MarshalAs(UnmanagedType.LPStr)] string FileName1, [MarshalAs(UnmanagedType.LPStr)] string FileName2);

        /// <summary>
        /// 単純拡大（補間有り）処理メソッド（CClipTransformメソッド使用）
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "CtEnlarge")]
        public static extern bool CtEnlarge([MarshalAs(UnmanagedType.LPStr)] string FileName1, int SX, int SY, int EX, int EY);

        /// <summary>
        /// ヒストグラム収集処理メソッド
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "CtHist")]
        public static extern bool CtHist(int Center, int Interval);

        /// <summary>
        /// 指定した２点間のから角度計算を行い，プロフィール測定を行います。
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "GetProfile")]
        public static extern bool GetProfile(int XS, int YS, int XE, int YE, int Bias, int Interval, ref int DispMode, ref int PS, ref int PNum, int[] PTbl);

        /// <summary>
        /// 入力指定された閾値内のﾌﾟﾛﾌｨｰﾙ値部分のROIを強調表示、ASCII番号表示します。
        /// その後、２点間距離測定を行います。
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "K3Pro")]
        public static extern bool K3Pro(int XS, int YS, int XE, int YE, int Bias, int Interval, int DispMode, int PS, int PNum, int[] PTbl, int High, int Low, int Unit);

        /// <summary>
        /// カラーパレット設定処理メソッド
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "CtColor")]
        public static extern bool CtColor(int rdMode, int ColorMode, int ColorType, ref int ColorVal, int[] palett, int[] paletl1, int[] paletl2);

        /// <summary>
        /// 指定領域のＣＴ値を取得
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "CtDump")]
        public static extern bool CtDump(int x1, int y1, int x2, int y2, int[] DumpBuff);

        /// <summary>
        //プロフィール＆ディスタンスの測定結果を、固定パスから読み出します。
		//固定パス  (C:\\CT\\TEMP\\PRDTEST.CSV")
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "PRDTempLoad")]
        public static extern int PRDTempLoad(ref double XS, ref double YS, ref double XE, ref double YE, ref int LowLmt, ref int UpLmt, ref int UnitLmt, ref double Dist, ref double AngleX, ref double AngleY);//PRDメソッド

        /// <summary>
        //１点指定選択処理
        /// </summary>
        [DllImport(DLL_PATH, EntryPoint = "GetRoiPoint")]
        public static extern bool GetRoiPoint(int PX, int PY, int LowLmt, int HighLmt, ref int OutXS, ref int OutYS, ref int OutXE, ref int OutYE);


        #endregion

    }
}
