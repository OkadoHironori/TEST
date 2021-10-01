using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
//
using CTAPI;
using CT30K.Common;

namespace CT30K
{

	///* ************************************************************************** */
	///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： 画像処理共通.bas                                            */
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
	public static class modImgProc
	{
		//********************************************************************************
		//  定数データ宣言（WindowsAPI用）
		//********************************************************************************

		private const int RASTERCAPS = 38;
		private const int RC_PALETTE = 0x100;
		private const int SIZEPALETTE = 104;

		private const int BLACKONWHITE = 1;
		private const int WHITEONBLACK = 2;
		private const int COLORONCOLOR = 3;
		private const int HALFTONE = 4;
		private const int MAXSTRETCHBLTMODE = 4;

		//********************************************************************************
		//  定数データ宣言（CT30K独自）
		//********************************************************************************

		//ボタン定数
		public enum CTButtonConstants
		{
			btnCTOK = 1,			//ＯＫ
			btnCTCancel,			//キャンセル
			btnCTDisp				//表示
		}

		//フォーマット変換定数
		public enum FormatType
		{
			FormatUnkown = -1,
			FormatRAW,
			FormatJPG,
			FormatBMP,
			FormatPCT,
			FormatTIF8,
			FormatTIF16
		//v29.99 今のところX線管移動は不要のため変更 by長野 2013/04/08'''''ここから'''''
		//    FormatDICOM
		//v29.99 今のところX線管移動は不要のため変更 by長野 2013/04/08'''''ここまで'''''
		}


        /*
		//********************************************************************************
		//  構造体宣言（WindowsAPI用）
		//********************************************************************************

		[StructLayout(LayoutKind.Sequential)]
		public struct BITMAPINFOHEADER
		{
			public int biSize;
			public int biWidth;
			public int biHeight;
			public short biPlanes;
			public short biBitCount;
			public int biCompression;
			public int biSizeImage;
			public int biXPelsPerMeter;
			public int biYPelsPerMeter;
			public int biClrUsed;
			public int biClrImportant;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RGBQUAD
		{
			public byte rgbBlue;
			public byte rgbGreen;
			public byte rgbRed;
			public byte rgbReserved;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct BITMAPINFO
		{
			public BITMAPINFOHEADER bmiHeader;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
			public RGBQUAD[] bmiColors;

			public static BITMAPINFO Initialize()
			{
				BITMAPINFO bitmapinfo = new BITMAPINFO();
				bitmapinfo.bmiColors = new RGBQUAD[256];
				return bitmapinfo;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct BITMAP
		{
			public int bmType;
			public int bmWidth;
			public int bmHeight;
			public int bmWidthBytes;
			public short bmPlanes;
			public short bmBitsPixel;
			public int bmBits;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct PALETTEENTRY
		{
			public byte peRed;
			public byte peGreen;
			public byte peBlue;
			public byte peFlags;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct LOGPALETTE
		{
			public short palVersion;
			public short palNumEntries;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
			public PALETTEENTRY[] palPalEntry;	// Enough for 256 colors.

			public static LOGPALETTE Initialize()
			{
				LOGPALETTE logpalette = new LOGPALETTE();
				logpalette.palPalEntry = new PALETTEENTRY[256];
				return logpalette;
			}
		}

		//OleCreatePictureIndirectに渡すための構造体（本来は共用体）
		[StructLayout(LayoutKind.Sequential)]
		public struct PicBmp
		{
			public int SIZE;		//この構造体のサイズ
			public int Type;		//ピクチャーのタイプ
			public int hBmp;		//ビットマップのハンドル
			public int hPal;		//パレットのハンドル
			public int Reserved;
		}

		//GUIDを格納するための構造体
		[StructLayout(LayoutKind.Sequential)]
		public struct GUID
		{
			public int Data1;
			public short Data2;
			public short Data3;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public byte[] Data4;

			public static GUID Initialize()
			{
				GUID guid = new GUID();
				guid.Data4 = new byte[8];
				return guid;
			}
		}
        */

		//********************************************************************************
		//  構造体宣言（CT30K独自）
		//********************************************************************************

		//フィルタ処理用構造体               'v10.2追加 by 間々田 2005/06/28
		[StructLayout(LayoutKind.Sequential)]
		public struct FilterStruct
		{
			public int FilterType;		//フィルタの種類
			public int SizeGauss;		//マトリクスサイズ（ガウス）
			public int SizeHiGauss;		//マトリクスサイズ（ハイガウス）
			public int SIZE;			//マトリクスサイズ（その他）
			public int Passes;			//回数
			public int Strength;		//強さ
		}

		//フィルタ処理用構造体変数の宣言     'v10.2追加 by 間々田 2005/06/28
		public static FilterStruct CTFilter = new FilterStruct();

		[StructLayout(LayoutKind.Sequential)]
		public struct LineStruct
		{
			public short x1;
			public short y1;
			public short x2;
			public short y2;
			public bool Visible;
			public int Color;
		}


		//********************************************************************************
		//  共通データ宣言
		//********************************************************************************

		public static short CT_Bias;			//CT値Bias値
		public static short CT_Int;				//CT値Interval値
		public static short CT_Low;				//プロフィールディスタンス：CT値下限閾値
		public static short CT_High;			//プロフィールディスタンス：CT値上限閾値
		public static short CT_Unit;			//プロフィールディスタンス：CT値連結幅
		public static short CT_Low1Point;		//プロフィールディスタンス：１点ROI指定時のCT値下限閾値
		public static short CT_High1Point;		//プロフィールディスタンス：１点ROI指定時のCT値上限閾値
		public static Winapi.POINTAPI P1;		//プロフィールディスタンス：ROI指定時の座標
        public static Winapi.POINTAPI P2;		//プロフィールディスタンス：ROI指定時の座標（２点指定時）
		public static short PRDPoint;			//プロフィールディスタンス：指定点数

		public static float EnlargeRatio;		//単純拡大の倍率 added by 山本 97－12－17

        //これは使用しない 2014/01/27(検S1)hata
        //画像処理オブジェクト               'v10.2追加 by 間々田 2005/07/04
		//public static ProcOCX MyProcOCX;

		//画像フォーマット変換用変数	  
		public static FormatType SaveType;		//ファイルの種類
		public static CheckState SaveInfo;		//付帯情報付き	
		public static CheckState SaveScale;		//スケール付加（0:無し,1:有り）  'V14.0 append by 山影 2007/07/17
		public static CheckState SaveChkCont;	//階調変換 (0:無し,1:有り)       'V14.0 append by 山影 2007/07/17

		//ROI処理測定結果
		public static double[] area = new double[21];	//面積値
		public static double[] Ave = new double[21];	//平均値
		public static double[] Sd = new double[21];		//標準偏差

		//寸法測定結果
		[StructLayout(LayoutKind.Sequential)]
		public struct DistanceInfoType
		{
			public double Dist;
			public double AngleX;
			public double AngleY;
            //public short x1;
            //public short y1;
            //public short x2;
            //public short y2;
            public int x1;
            public int y1;
            public int x2;
            public int y2;           
        }

		public static DistanceInfoType[] DistanceInfo;		//v10.2追加 by 間々田 2005/07/07

		//画面印刷時のフラグ                         'v15.0追加 by 間々田 2009/08/20
		public static bool IsPrint1stMonitor;
		public static bool IsPrint2ndMonitor;

        /*
		//********************************************************************************
		//  外部関数宣言（WindowsAPI用）
		//********************************************************************************
		[DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int CreateCompatibleDC(int hdc);
		[DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int CreateCompatibleBitmap(int hdc, int nWidth, int nHeight);
		[DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int CreateDIBSection(int hdc, ref BITMAPINFO pBitmapInfo, int un, ref int lplpVoid, int Handle, int dw);
		[DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int GetDeviceCaps(int hdc, int iCapabilitiy);
		[DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int GetSystemPaletteEntries(int hdc, int wStartIndex, int wNumEntries, ref PALETTEENTRY lpPaletteEntries);
		[DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int CreatePalette(ref LOGPALETTE lpLogPalette);
		[DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int SelectPalette(int hdc, int hPalette, int bForceBackground);
		[DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int RealizePalette(int hdc);
		[DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int DeleteDC(int hdc);
		[DllImport("gdi32", EntryPoint = "GetObjectA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int GetObject(int hObject, int nCount, ref Any lpObject);					//TODO As Any
		[DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int SelectObject(int hdc, int hObject);
		[DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int StretchBlt(int hdc, int x, int y, int nWidth, int nHeight, int hSrcDC, int xSrc, int ySrc, int nSrcWidth, int nSrcHeight, int dwRop);
		[DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int BitBlt(int hDestDC, int x, int y, int nWidth, int nHeight, int hSrcDC, int xSrc, int ySrc, int dwRop);
		[DllImport("gdi32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int SetStretchBltMode(int hdc, int nStretchMode);

		[DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int GetWindowDC(int hWnd);
		[DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int ReleaseDC(int hWnd, int hdc);
		[DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int GetDesktopWindow();

		[DllImport("olepro32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int OleCreatePictureIndirect(ref PicBmp PicDesc, ref GUID RefIID, int fPictureOwnsHandle, ref Image IPic);

		[DllImport("kernel32", EntryPoint = "RtlMoveMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void CopyMemory(ref Any Destination, ref Any Source, int length);		//TODO As Any


		//********************************************************************************
		//  外部関数宣言（ImgProc.dll）
		//********************************************************************************
		//バイトデータをビットマップにセットする
		[DllImport("ImgProc.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void SetByteToBitmap(int Handle, ref byte Image);
		
		//バイト画像データ反転
		[DllImport("ImgProc.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void ReverseByteImage(ref byte Image, int imageSize);

		//ワード画像データ反転
		[DllImport("ImgProc.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void ReverseWordImage(ref short Image, int imageSize);

		//ワード画像データ左右反転       'v17.50追加 2011/02/04 by 間々田
		[DllImport("ImgProc.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void ConvertMirror(ref short Image, int Width, int Height);
        */


        /*
        //削除2015/01/21hata_Apiが64bitで使えないため
        //この関数はBitmapの生成するもの。Bitmap bmp = new Bitmap()を使用すること
		//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
		//
		// CreateBitmapPicture
		//    - Creates a bitmap type Picture object from a bitmap and
		//      palette.
		//
		// hBmp
		//    - Handle to a bitmap.
		//
		// hPal
		//    - Handle to a Palette.
		//    - Can be null if the bitmap doesn't use a palette.
		//
		// Returns
		//    - Returns a Picture object containing the bitmap.
		//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
		public static Image CreateBitmapPicture(int hBmp, int hPal = 0)
		{
            Winapi.PicBmp Pic = default(Winapi.PicBmp);

			// IPicture requires a reference to "Standard OLE Types."
			Image IPic = null;
            Winapi.GUID IID_IDispatch = Winapi.GUID.Initialize();

			// Fill in with IDispatch Interface ID.
			IID_IDispatch.Data1 = 0x20400;
			IID_IDispatch.Data4[0] = 0xc0;
			IID_IDispatch.Data4[7] = 0x46;

			// Fill Pic with necessary parts.
			Pic.SIZE = Marshal.SizeOf(Pic);					// Length of structure.
			//TODO Const vbPicTypeBitmap = 1
			Pic.Type = 1;									// Type of Picture (bitmap).
			Pic.hBmp = hBmp;								// Handle to bitmap.
			Pic.hPal = hPal;								// Handle to palette (may be null).

			// Create Picture object.
            Winapi.OleCreatePictureIndirect(ref Pic, ref IID_IDispatch, 1, ref IPic);

			// Return the new Picture object.
			return IPic;
		}
        */

        /*
        //削除2015/01/21hata_Apiが64bitで使えないため
        //この関数はGraphicsのCopyFromScreenを使用すること
		///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
		//
		// CaptureWindow
		//    - Captures any portion of a window.
		//
		// hWndSrc
		//    - Handle to the window to be captured.
		//
		// LeftSrc, TopSrc, WidthSrc, HeightSrc
		//    - Specify the portion of the window to capture.
		//    - Dimensions need to be specified in pixels.
		//
		// Returns
		//    - Returns a Picture object containing a bitmap of the specified
		//      portion of the window that was captured.
		///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
		public static Image CaptureWindow(int hWndSrc, int LeftSrc, int TopSrc, int WidthSrc, int HeightSrc)
		{
			int hDCMemory = 0;
			int hBmp = 0;
			int hBmpPrev = 0;
			int r = 0;
			int hDCSrc = 0;
			int hPal = 0;
			int hPalPrev = 0;
			int RasterCapsScrn = 0;
			int HasPaletteScrn = 0;
			int PaletteSizeScrn = 0;
            Winapi.LOGPALETTE LogPal = Winapi.LOGPALETTE.Initialize();

			// Get device context for entire window.
            hDCSrc = Winapi.GetWindowDC(hWndSrc);

			// Create a memory device context for the copy process.
            hDCMemory = Winapi.CreateCompatibleDC(hDCSrc);

			// Create a bitmap and place it in the memory DC.
            hBmp = Winapi.CreateCompatibleBitmap(hDCSrc, WidthSrc, HeightSrc);
            hBmpPrev = Winapi.SelectObject(hDCMemory, hBmp);

			// Get screen properties.
            RasterCapsScrn = Winapi.GetDeviceCaps(hDCSrc, RASTERCAPS);		// Raster
																	// capabilities.
            HasPaletteScrn = RasterCapsScrn & RC_PALETTE;			// Palette
																	// support.
            PaletteSizeScrn = Winapi.GetDeviceCaps(hDCSrc, SIZEPALETTE);	// Size of
																	// palette.
			// If the screen has a palette make a copy and realize it.
			if ((HasPaletteScrn != 0) && (PaletteSizeScrn == 256))
			{
				// Create a copy of the system palette.
				LogPal.palVersion = 0x300;
				LogPal.palNumEntries = 256;
                r = Winapi.GetSystemPaletteEntries(hDCSrc, 0, 256, ref LogPal.palPalEntry[0]);
                hPal = Winapi.CreatePalette(ref LogPal);
				// Select the new palette into the memory DC and realize it.
                hPalPrev = Winapi.SelectPalette(hDCMemory, hPal, 0);
                r = Winapi.RealizePalette(hDCMemory);
			}

			// Copy the on-screen image into the memory DC.
            r = Winapi.BitBlt(hDCMemory, 0, 0, WidthSrc, HeightSrc, hDCSrc, LeftSrc, TopSrc, 0xCC0020);	//TODO Const vbSrcCopy = 13369376 (&HCC0020)

			// Remove the new copy of the  on-screen image.
            hBmp = Winapi.SelectObject(hDCMemory, hBmpPrev);

			// If the screen has a palette get back the palette that was
			// selected in previously.
			if ((HasPaletteScrn != 0) && (PaletteSizeScrn == 256))
			{
                hPal = Winapi.SelectPalette(hDCMemory, hPalPrev, 0);
			}

			// Release the device context resources back to the system.
            r = Winapi.DeleteDC(hDCMemory);
            r = Winapi.ReleaseDC(hWndSrc, hDCSrc);

			// Call CreateBitmapPicture to create a picture object from the
			// bitmap and palette handles. Then return the resulting picture
			// object.
			return CreateBitmapPicture(hBmp, hPal);
		}
        */

        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        //
        // CaptureScreen
        //    - Captures the entire screen.
        //
        // Returns
        //    - Returns a Picture object containing a bitmap of the screen.
        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        //全面変更2015/01/21hata_Apiが使えないため
        ////Public Function CaptureScreen() As Picture
        //public static Image CaptureScreen(int? LeftSrc = null, int? TopSrc = null, int? WidthSrc = null, int? HeightSrc = null)		//v15.0変更 by 間々田 2009/08/20
        //{
        //    int hWndScreen = 0;

        //    // Get a handle to the desktop window.
        //    hWndScreen = Winapi.GetDesktopWindow();

        //    //' Call CaptureWindow to capture the entire desktop give the handle
        //    //' and return the resulting Picture object.
        //    //Set CaptureScreen = CaptureWindow(hWndScreen, 0, 0, _
        //    //'                                  Screen.width \ Screen.TwipsPerPixelX, _
        //    //'                                  Screen.Height \ Screen.TwipsPerPixelY)

        //    //v15.0以下に変更 by 間々田 2009/08/20
        //    if (LeftSrc == null) LeftSrc = 0;
        //    if (TopSrc == null) TopSrc = 0;
        //    if (WidthSrc == null) WidthSrc = Screen.PrimaryScreen.Bounds.Width;
        //    if (HeightSrc == null) HeightSrc = Screen.PrimaryScreen.Bounds.Height;

        //    // Call CaptureWindow to capture the entire desktop give the handle
        //    // and return the resulting Picture object.
        //    return CaptureWindow((int)hWndScreen, (int)LeftSrc, (int)TopSrc, (int)WidthSrc, (int)HeightSrc);
        //}
        public static Image CaptureScreen(int? LeftSrc = null, int? TopSrc = null, int? WidthSrc = null, int? HeightSrc = null)
        {
            if (LeftSrc == null) LeftSrc = 0;
            if (TopSrc == null) TopSrc = 0;
            if (WidthSrc == null) WidthSrc = Screen.PrimaryScreen.Bounds.Width;
            if (HeightSrc == null) HeightSrc = Screen.PrimaryScreen.Bounds.Height;

            //Bitmapの作成
            Bitmap bmp = new Bitmap((int)WidthSrc, (int)HeightSrc, PixelFormat.Format32bppArgb);

            //Graphicsの作成
            Graphics g = Graphics.FromImage(bmp);
            //デスクトップ画面をコピーする
            g.CopyFromScreen(new Point((int)LeftSrc, (int)TopSrc), new Point(0, 0), bmp.Size);

            //test保存用
            //bmp.Save(@"C:\CTUSER\ScreenCapture.bmp", ImageFormat.Bmp);

            //解放
            g.Dispose();

            //表示
            return bmp;

        }


		//   BitmapのBit情報の設定。
		//
		//public static void SetBitmapBits(ref int hBitmap, ref byte[] theImage)
        public static void SetBitmapBits(IntPtr hBitmap, ref byte[] theImage)
        {

            Winapi.BITMAP myBitmap = default(Winapi.BITMAP);
            Winapi.GetObject(hBitmap, Marshal.SizeOf(myBitmap), ref myBitmap);

            //変更2014/01/27(検S1)hata
            //Marshal.Copy(theImage, 0, hBitmap, myBitmap.bmWidth * myBitmap.bmHeight * sizeof(byte));
            int i = 0;
            for (i = theImage.GetLowerBound(0); i <= theImage.GetUpperBound(0); i += myBitmap.bmWidth)
            {
                Winapi.CopyMemory((IntPtr)(myBitmap.bmBits + ((myBitmap.bmHeight - 1 - i / myBitmap.bmWidth) * myBitmap.bmWidthBytes)), (IntPtr)theImage[i], myBitmap.bmWidth);
            }
		}

		//   BitmapのBit情報の設定。
		//
		//public static void GetBitmapBits(ref int hBitmap, ref byte[] theImage)
        public static void GetBitmapBits(IntPtr hBitmap, ref byte[] theImage)
        {

            Winapi.BITMAP myBitmap = default(Winapi.BITMAP);
            Winapi.GetObject(hBitmap, Marshal.SizeOf(myBitmap), ref myBitmap);

            //変更2014/01/27(検S1)hata
            //Marshal.Copy(hBitmap, theImage, 0, myBitmap.bmWidth * myBitmap.bmHeight * sizeof(byte));
            int i = 0;
            for (i = theImage.GetLowerBound(0); i <= theImage.GetUpperBound(0); i += myBitmap.bmWidth)
            {
                IntPtr Bmpptr = hBitmap + ((myBitmap.bmHeight - 1 - i / myBitmap.bmWidth) * myBitmap.bmWidthBytes) ;

                Winapi.CopyMemory((IntPtr)theImage[i], Bmpptr, myBitmap.bmWidth);
            }
		}

        //   BitmapのBit情報の設定。
        //
        //public static void GetBitmapBits(ref int hBitmap, ref byte[] theImage)
        public static void GetBitmapBits(Bitmap bmp, ref byte[] theImage)
        {

            //Winapi.BITMAP myBitmap = default(Winapi.BITMAP);
            //Winapi.GetObject(bmp, Marshal.SizeOf(myBitmap), ref myBitmap);

            ////変更2014/01/27(検S1)hata
            ////Marshal.Copy(hBitmap, theImage, 0, myBitmap.bmWidth * myBitmap.bmHeight * sizeof(byte));
            //int i = 0;
            //for (i = theImage.GetLowerBound(0); i <= theImage.GetUpperBound(0); i += myBitmap.bmWidth)
            //{
            //    IntPtr Bmpptr = hBitmap + ((myBitmap.bmHeight - 1 - i / myBitmap.bmWidth) * myBitmap.bmWidthBytes);
            //
            //    Winapi.CopyMemory((IntPtr)theImage[i], Bmpptr, myBitmap.bmWidth);
            //}


            //LocBitsを使用してデータを書き換える
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            //ビットマップデータをロックする
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            //ビットマップをByteデータに変換する
            Pulsar.BitmapToByte(bmpData.Scan0, theImage, bmpData.Width, bmpData.Height, bmpData.Stride);
           
            //ビットマップデータに対するロックを解除
            bmp.UnlockBits(bmpData);

        }


        /*
        //削除2015/01/21hata_使用していない
        //この関数はBitmapの生成するもの。Bitmap bmp = new Bitmap()を使用すること
		//*******************************************************************************
		//機　　能： ピクチャの生成
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 幅、高さが異なる場合は、拡大または縮小してコピーする
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		//   名前    CreatePicture
		//   機能    指定された大きさのBitmapを格納した
		//           Pictureオブジェクトを作成します。
		//   戻り値  作成されたBitmapを含むPictureオブジェクト。
		//   引数
		//   [in]    Width   画像の幅(Pixel単位)。
		//   [in]    Height  画像の高さ(Pixel単位)。
		//
		public static Image CreatePicture(int hdc, int Width, int Height)
		{
			//ビットマップ生成
			int hBitmap = 0;	//OLE_HANDLE
			int pDIBits = 0;
            Winapi.BITMAPINFO myBitmapInfo = Winapi.BITMAPINFO.Initialize();

			myBitmapInfo.bmiHeader.biSize = Marshal.SizeOf(myBitmapInfo.bmiHeader);
			myBitmapInfo.bmiHeader.biWidth = Width;
			myBitmapInfo.bmiHeader.biHeight = Height;
			myBitmapInfo.bmiHeader.biPlanes = 1;
			myBitmapInfo.bmiHeader.biBitCount = 8;
			myBitmapInfo.bmiHeader.biClrUsed = 256;

			int i = 0;
			for (i = 0; i <= 255; i++)
			{
				myBitmapInfo.bmiColors[i].rgbRed = (byte)i;
				myBitmapInfo.bmiColors[i].rgbGreen = (byte)i;
				myBitmapInfo.bmiColors[i].rgbBlue = (byte)i;
			}

			if (hdc != 0)
			{
				//アプリケーションから直接書き込むことのできるDIBを作成する
                hBitmap = Winapi.CreateDIBSection(hdc, ref myBitmapInfo, 0, ref pDIBits, 0, 0);
			}

			return CreateBitmapPicture(hBitmap);
		}
        */

        //*******************************************************************************
		//機　　能： ピクチャのコピー
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： 幅、高さが異なる場合は、拡大または縮小してコピーする
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
		//Public Sub CopyPicture(ByVal PicSrc As Picture, ByVal PicDest As Picture)
		//
		//    Dim r               As Long
		//    Dim hDCDest         As Long
		//    Dim hDCSrc          As Long
		//    Dim hBmpSrcPrev     As Long
		//    Dim hBmpDestPrev    As Long
		//
		//    '転送元のピクチャのビットマップ情報
		//    Dim SrcBitmap As BITMAP
		//    Call GetObject(PicSrc.Handle, Len(SrcBitmap), SrcBitmap)
		//
		//    '転送先のピクチャのビットマップ情報
		//    Dim DestBitmap As BITMAP
		//    Call GetObject(PicDest.Handle, Len(DestBitmap), DestBitmap)
		//
		//    'メモリデバイスコンテキストを作成し、転送元のピクチャのビットマップを選択
		//    hDCSrc = CreateCompatibleDC(0)
		//    hBmpSrcPrev = SelectObject(hDCSrc, PicSrc.Handle)
		//
		//    'メモリデバイスコンテキストを作成し、転送先のピクチャのビットマップを選択
		//    hDCDest = CreateCompatibleDC(0)
		//    hBmpDestPrev = SelectObject(hDCDest, PicDest.Handle)
		//
		//    'ビットマップ伸縮モード：縮小の場合ピクセル削除
		//    SetStretchBltMode hDCDest, COLORONCOLOR
		//
		//    'ビットマップを拡大または縮小してをコピー
		//    r = StretchBlt(hDCDest, 0, 0, _
		//                   DestBitmap.bmWidth, _
		//                   DestBitmap.bmHeight, _
		//                   hDCSrc, 0, 0, _
		//                   SrcBitmap.bmWidth, _
		//                   SrcBitmap.bmHeight, _
		//                   vbSrcCopy)
		//
		//    '後処理
		//    SelectObject hDCSrc, hBmpSrcPrev
		//    SelectObject hDCDest, hBmpDestPrev
		//
		//    'メモリデバイスコンテキストを破棄
		//    r = DeleteDC(hDCSrc)
		//    r = DeleteDC(hDCDest)
		//
		//End Sub
		//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''


		//*******************************************************************************
		//機　　能： 画像処理関連の初期化処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public static void InitImgProc()
		{
			//画像フォーマット変換画面（frmFormatTransfer）の初期値
			SaveType = FormatType.FormatJPG;
			SaveInfo = CheckState.Unchecked;

			//1点指定プロフィールディスタンスの初期値設定
			CT_Low1Point = 0;			//１点ROI指定時のCT値最小初期値
			CT_High1Point = 3000;		//１点ROI指定時のCT値最大初期値
			PRDPoint = 0;

			//フィルタ処理の初期値 'v10.2追加 by 間々田 2005/08/10
			CTFilter.FilterType = 0;		//フィルタの種類
			CTFilter.SIZE = 0;				//マトリクスサイズ（ガウス以外）
			CTFilter.SizeGauss = 7;			//マトリクスサイズ（ガウス）
			CTFilter.Passes = 2;			//回数
			CTFilter.Strength = 10;			//強さ

			//画面印刷時のフラグの初期値             'v15.0追加 by 間々田 2009/08/20
			IsPrint1stMonitor = true;
			IsPrint2ndMonitor = false;
		}

    }
}
