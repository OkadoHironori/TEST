using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CTAPI
{
    /// <summary>
    /// Pulsar.dll
    /// </summary>
    public class Pulsar
    {
        /// <summary>
        /// MILハンドル
        /// </summary>
        public static IntPtr hMil = IntPtr.Zero;

        /// <summary>
        /// PKEハンドル
        /// </summary>
        public static IntPtr hPke = IntPtr.Zero;

        //-----------------------------------------------------------------------------
        // コールバック
        //-----------------------------------------------------------------------------
        /// <summary>
        /// コールバック関数
        /// </summary>
        /// <param name="handle"></param>
        public delegate void CallbackFunc(IntPtr handle);

        //-----------------------------------------------------------------------------
        // 定数
        //-----------------------------------------------------------------------------
        public const int IMAGE_BIT8 = 0;
        public const int IMAGE_BIT16 = 1;
        public const int M_SYNCHRONOUS = 1;
        public const int M_ASYNCHRONOUS = 2;


        //-----------------------------------------------------------------------------
        // API関数
        //-----------------------------------------------------------------------------
        #region API
        /// <summary>
        /// キャプチャ準備処理
        /// </summary>
        /// <param name="hMil"></param>
        /// <param name="hPke"></param>
        /// <returns></returns>
        [DllImport("Pulsar.dll")]
        public static extern int CaptureSetup(IntPtr hMil, IntPtr hPke);
        
        /// <summary>
        /// キャプチャ開始時処理
        /// </summary>
        /// <param name="hMil"></param>
        /// <param name="hPke"></param>
        [DllImport("Pulsar.dll")]
        public static extern void CaptureSeqStart(IntPtr hMil, IntPtr hPke);

        /// <summary>
        /// キャプチャ停止時処理
        /// </summary>
        /// <param name="hMil"></param>
        /// <param name="hPke"></param>
        [DllImport("Pulsar.dll")]
        public static extern void CaptureSeqStop(IntPtr hMil, IntPtr hPke);

        /// <summary>
        /// 透視画像を取得する
        /// </summary>
        /// <param name="hMil"></param>
        /// <param name="hPke"></param>
        /// <param name="TRANS_IMAGE"></param>
        [DllImport("Pulsar.dll")]
        public static extern void GetCaptureImage(
	        IntPtr	hMil,			//MILパラメータ
	        IntPtr	hPke,			//PKEパラメータ
	        ushort[] TRANS_IMAGE	//透視画像配列
        );

        /// <summary>
        /// 積算画像を取得する
        /// </summary>
        /// <param name="hMil"></param>
        /// <param name="hPke"></param>
        /// <param name="viewnum"></param>
        /// <param name="Itgnum"></param>
        /// <param name="TRANS_IMAGE"></param>
        /// <param name="SUM_IMAGE"></param>
        /// <param name="hCT30K"></param>
        /// <returns></returns>
        [DllImport("Pulsar.dll")]
        public static extern int GetCaptureSumImage(
            IntPtr hMil,			//MILパラメータ
            IntPtr hPke,			//PKEパラメータ
            int viewnum,		    //ビュー数 
            int Itgnum,			    //積算枚数
            ushort[] TRANS_IMAGE,	//透視画像配列
            int[] SUM_IMAGE,		//結果画像配列
            CallbackFunc hCT30K		//CT30Kへのコールバック関数のアドレス
        );

        /// <summary>
        /// 画像の除算(WORD用）
        /// </summary>
        /// <param name="sum_img"></param>
        /// <param name="ave_img"></param>
        /// <param name="N"></param>
        /// <param name="H_SIZE"></param>
        /// <param name="V_SIZE"></param>
        [DllImport("Pulsar.dll")]
        public static extern void DivImage_short(int[] sum_img, ushort[] ave_img, int N, int H_SIZE, int V_SIZE);

        /// <summary>
        /// 画像の除算(WORD用）
        /// </summary>
        /// <param name="sum_img"></param>
        /// <param name="ave_img"></param>
        /// <param name="N"></param>
        /// <param name="H_SIZE"></param>
        /// <param name="V_SIZE"></param>
        [DllImport("Pulsar.dll")]
        public static extern void DivImage_short_Scan(int[] sum_img, ushort[] ave_img, int N, int SIZE);

        /// <summary>
        /// 画像の除算(double用）
        /// </summary>
        /// <param name="sum_img"></param>
        /// <param name="ave_img"></param>
        /// <param name="N"></param>
        /// <param name="H_SIZE"></param>
        /// <param name="V_SIZE"></param>
        [DllImport("Pulsar.dll")]
        public static extern void DivImage_double(int[] sum_img, double[] ave_img, int N, int H_SIZE, int V_SIZE);

        // 回転中心校正データ収集関数
        // ゲイン校正データ収集関数
        // 自動スキャン位置登録関数
        // フィッティング計算を行って、スキャン位置を求める関数

        /// <summary>
        /// LUT変換関数
        /// </summary>
        /// <param name="TRANS_IMAGE"></param>
        /// <param name="DisplayBuffe"></param>
        /// <param name="LUT"></param>
        /// <param name="MirrorFlag"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        [DllImport("Pulsar.dll")]
        public static extern void LutApply(
            UInt16[] TransImgArray,
            byte[] diplayBuffer,
            byte[] LookUpTable,
            int MirrorFlg,
            int Width,
            int Height);


        /// <summary>
        /// ビットマップ変換
        /// </summary>
        /// <param name="displayBuffer"></param>
        /// <param name="bitmap"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="stride"></param>
        [DllImport("Pulsar.dll")]
        public static extern void DrawBitmap(
            byte[] displayBuffer,
            IntPtr bitmap,
            int width,
            int height,
            int stride);

         /// <summary>
        /// ビットマップ変換(反転付き)
        /// </summary>
        /// <param name="displayBuffer"></param>
        /// <param name="bitmap"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="stride"></param>
        [DllImport("Pulsar.dll")]
        public static extern void DrawBitmap2(
            byte[] displayBuffer,
            IntPtr bitmap,
            int width,
            int height,
            int stride,
            int Inverse);

        /// <summary>
        /// ビットマップからByteに変換
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="displayBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="stride"></param>      
        [DllImport("Pulsar.dll")]
        public static extern void BitmapToByte(
           IntPtr bitmap, 
           byte[] DisplayBuffer, 
           int Width, 
           int Height, 
           int stride);

        
        #endregion API

        //-----------------------------------------------------------------------------
        // MIL関数
        //-----------------------------------------------------------------------------
        #region MIL
        /// <summary>
        /// キャプチャボードの初期化
        /// </summary>
        /// <param name="dcf"></param>
        /// <param name="Image_bit"></param>
        /// <param name="s_mode"></param>
        /// <returns></returns>
        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr MilOpen(
            //[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 1)]
            //[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 1)]
            string dcf,   //カメラ情報ファイル
            int Image_bit,	//画像ビット数 0:8ビット 1:16ビット
            int s_mode	    //同期モード
        );
        //public static extern IntPtr MilOpen(
        //    [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 1)]
        //    string[] dcf  //カメラ情報ファイル
        //);

        /// <summary>
        /// ビデオキャプチャボードの開放
        /// </summary>
        /// <param name="hMil"></param>
        [DllImport("Pulsar.dll")]
        public static extern void MilClose(IntPtr hMil);

        /// <summary>
        /// 同期・非同期モードを設定する
        /// </summary>
        /// <param name="hMil"></param>
        /// <param name="mode"></param>
        [DllImport("Pulsar.dll")]
        public static extern void MilSetGrabMode(IntPtr hMil, int mode);
        
        /// <summary>
        /// 収集終了待ち処理
        /// </summary>
        /// <param name="hMil"></param>
        [DllImport("Pulsar.dll")]
        public static extern void MilGrabWait(IntPtr hMil);

        /// <summary>
        /// 動画保存用MILバッファを確保する関数
        /// </summary>
        /// <param name="StartIndex"></param>
        /// <param name="hMil"></param>
        /// <param name="orgSizeX"></param>
        /// <param name="fphm"></param>
        /// <param name="orgSizeY"></param>
        /// <param name="fpvm"></param>
        /// <param name="ZoomScale"></param>
        /// <returns></returns>
        [DllImport("Pulsar.dll")]
        public static extern int Mil9AllocUserArry(
            int StartIndex, IntPtr hMil, int orgSizeX, float fphm, int orgSizeY, float fpvm, int ZoomScale);

        /// <summary>
        /// 動画保存用MILバッファをクリアする関数
        /// </summary>
        /// <returns></returns>
        [DllImport("Pulsar.dll")]
        public static extern int Mil9ClearUserArry();

        /// <summary>
        /// 8bit画像を動画保存用MILバッファへコピーする関数
        /// </summary>
        /// <param name="MovieCount"></param>
        /// <param name="ByteImageBuff"></param>
        /// <param name="hMil"></param>
        /// <param name="orgSizeX"></param>
        /// <param name="fphm"></param>
        /// <param name="orgSizeY"></param>
        /// <param name="fpvm"></param>
        /// <param name="ZoomScale"></param>
        /// <returns></returns>
        [DllImport("Pulsar.dll")]
        public static extern int Mil9CopyUserArry(
            int MovieCount, byte[] ByteImageBuff, IntPtr hMil, int orgSizeX, float fphm, int orgSizeY, float fpvm, int ZoomScale);

        /// <summary>
        /// MILバッファから動画を作成する関数
        /// </summary>
        /// <param name="Counter"></param>
        /// <param name="FileName"></param>
        /// <param name="FrameRate"></param>
        /// <returns></returns>
        [DllImport("Pulsar.dll", CharSet = CharSet.Auto)]
        public static extern int Mil9SaveMovie(int Counter, string FileName, double FrameRate);

        /// <summary>
        /// ホストメモリを確保する関数(MILキャプチャボードを使わない場合)
        /// </summary>
        /// <param name="xSize"></param>
        /// <param name="ySize"></param>
        /// <returns></returns>
        [DllImport("Pulsar.dll")]
        public static extern IntPtr MilHostOpen(int xSize, int ySize); 


        [DllImport("Pulsar.dll")]
        public static extern void MilGrabAndGet(IntPtr hMil, ushort[] image);



        #endregion MIL

        //-----------------------------------------------------------------------------
        // PKE関数
        //-----------------------------------------------------------------------------
        #region PKE
        /// <summary>
        /// パーキンエルマー用キャプチャボードの初期化
        /// </summary>
        /// <param name="fpd_gain"></param>
        /// <param name="fpd_integ"></param>
        /// <param name="FG_type"></param>
        /// <returns></returns>
        [DllImport("Pulsar.dll")]
        public static extern IntPtr PkeOpen(
            int fpd_gain,		//ゲイン設定
            int fpd_integ,		//積分時間
            int FG_type			//フレームグラバーの種類
        );

        /// <summary>
        /// ビデオキャプチャボードの開放
        /// </summary>
        /// <param name="hPke"></param>
        [DllImport("Pulsar.dll")]
        public static extern void PkeClose(IntPtr hPke);

        /// <summary>
        /// パーキンエルマー用センサゲインとフレームレート(積分時間)のセット
        /// </summary>
        /// <param name="hPke"></param>
        /// <param name="fpd_gain"></param>
        /// <param name="fpd_integ"></param>
        /// <returns></returns>
        [DllImport("Pulsar.dll")]
        public static extern int PkeSetGainFrameRate(
            IntPtr hPke,			//初期化情報
            int fpd_gain,			//ゲイン設定
            int fpd_integ			//積分時間
        );

        /// <summary>
        /// パーキンエルマー用ゲインデータのセット
        /// </summary>
        /// <param name="hPke"></param>
        /// <param name="file_read_flag"></param>
        /// <param name="GainData"></param>
        /// <param name="preload_flag"></param>
        /// <returns></returns>
        [DllImport("Pulsar.dll")]
        //'v18.00変更 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        //public static extern int PkeSetGainData(
        //    IntPtr hPke,			//初期化情報
        //    int file_read_flag,		//ゲイン校正データをファイルから読むか引数で受け取るかのフラッグ　0:ファイル　1：引数
        //    uint[] GainData,		//ゲイン画像
        //    int preload_flag		//校正データをプリロードするかのフラッグ 0:しない 1：する
        //);
        public static extern int PkeSetGainData(
            IntPtr hPke,			//初期化情報 
            uint[] GainData,        //ゲイン画像
            int preload_flag,       //校正データをプリロードするかのフラッグ 0:しない 1：する
            string FileName         //ゲイン校正データのファイル名	//Rev18.00変更 byやまおか //v19.50 v19.41とv18.02の統合 by長野 2013/11/01
        );    

        /// <summary>
        /// パーキンエルマー用オフセットデータのセット
        /// </summary>
        /// <param name="hPke"></param>
        /// <param name="file_read_flag"></param>
        /// <param name="OffsetData"></param>
        /// <param name="preload_flag"></param>
        /// <returns></returns>
        [DllImport("Pulsar.dll")]
        public static extern int PkeSetOffsetData(
            IntPtr hPke,			//初期化情報
            int file_read_flag,		//オフセット校正データをファイルから読むか引数で受け取るかのフラッグ　0:ファイル 1：引数
            double[] OffsetData,	//オフセット画像
            int preload_flag		//校正データをプリロードするかのフラッグ 0:しない 1：する
        );

        /// <summary>
        /// パーキンエルマー用欠陥マップのセット
        /// </summary>
        /// <param name="hPke"></param>
        /// <param name="preload_flag"></param>
        /// <returns></returns>
        [DllImport("Pulsar.dll")]
        public static extern int PkeSetPixelMap(
            IntPtr hPke,			//初期化情報
            int preload_flag		//校正データをプリロードするかのフラッグ 0:しない 1：する
        );

        /// <summary>
        /// 透視画像表示用キャプチャ関数（中でループしない）
        /// </summary>
        /// <param name="hPke"></param>
        /// <param name="TRANS_IMAGE"></param>
        [DllImport("Pulsar.dll")]
        public static extern void PkeCapture(
            IntPtr hPke,
            ushort[] TRANS_IMAGE		//画像を出力する配列
        );

        /// <summary>
        /// オフセット校正データ収集関数
        /// </summary>
        /// <param name="hPke"></param>
        /// <param name="hCT30K"></param>
        /// <param name="TRANS_IMAGE"></param>
        /// <param name="OFFSET_IMAGE"></param>
        /// <param name="Itgnum"></param>
        /// <returns></returns>
        [DllImport("Pulsar.dll")]
        public static extern int PkeCaptureOffset(
            IntPtr hPke,
            CallbackFunc hCT30K,	//コールバック関数のアドレス
            ushort[] TRANS_IMAGE,	//画像を出力する配列
            double[] OFFSET_IMAGE,	//オフセット画像	
            int Itgnum			    //積算枚数
        );

        /// <summary>
        /// ゲインデータ収集関数
        /// </summary>
        /// <param name="hPke"></param>
        /// <param name="hCT30K"></param>
        /// <param name="TRANS_IMAGE"></param>
        /// <param name="GAIN_IMAGE"></param>
        /// <param name="viewnum"></param>
        /// <param name="Itgnum"></param>
        /// <returns></returns>
        [DllImport("Pulsar.dll")]
        public static extern int PkeCaptureGain(
            IntPtr hPke,
            CallbackFunc hCT30K,	//コールバック関数のアドレス
            ushort[] TRANS_IMAGE,	//CT30kに返す画像
            int[] GAIN_IMAGE,	    //ゲイン画像
            int viewnum,		    //ビュー数						
            int Itgnum			    //積算枚数
        );


        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern double PkeGetIntegTime(int fpd_integ);     //v19.10 追加 by長野 2102/07/30


        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void PkeCaptureOnly(IntPtr hPke, ushort[] TRANS_IMAGE);	


        //回転中心校正画像取込み関数
        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int RotationCenter_Data_Acquire(int hMil, int hPke, ref short TransImage, int h_size, int v_size, int Itgnum, ref short RC_IMAGE0, ref short RC_IMAGE1, ref short RC_IMAGE2, ref short RC_IMAGE3,
        ref short RC_IMAGE4, int hDevID, ref int mrc, int View, int multislice, int table_rotation, float SW, int connect, ref short RC_CONE, ref short Def_IMAGE,
        ref short GAIN_IMAGE, int detector, float FrameRate, int RotateSelect, int c_cw_ccw, int xrot_stop_pos, int CallBackAddress);        //v17.50引数変更 by 間々田 2011/01/13 DestImage, dcf削除

        //ゲイン校正画像取込み関数
        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int Gain_Data_Acquire(int hMil, int hPke, ref short TransImage, int Itgnum, int CallBackAddress, ref short Image, ref int ima2, int hDevID, ref int mrc, int View,
        int table_rotation, int detector, float FrameRate);        //v17.50変更 DestImage削除 by 間々田 2011/01/14

        //自動スキャン位置校正
        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int AutoScanpositionEntry(int hMil, int hPke, ref short TransImage, int hDevID, ref int mrc, float fud_step, float rud_step, ref float fud_start, ref float fud_end, float rud_start,
        float rud_end, int detector, ref short GAIN_IMAGE, ref short Def_IMAGE, int CallBackAddress);        //v17.50変更 DestImage削除 by 間々田 2011/01/14

        [DllImport("Pulsar.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int AcqScanPosition(int h_size, int v_size, ref float scan_posi_a, ref float scan_posi_b, ref short Image);        //追加 by 間々田 2006/01/11





        #endregion PKE
    }
}
