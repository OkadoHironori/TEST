using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
//
using CTAPI;

namespace TransImage
{

    /// <summary>
    /// 階調サイズ
    /// </summary>
    public enum LTSize : int
    {
        LT16Bit = 65535,
        LT12Bit = 4095
    }


    /// <summary>
    /// 透視画像
    /// </summary>
    public class TransPicture
    {
        //透視画像更新イベント
        public event EventHandler TransImageChanged;

        //Integral独自のデリゲート宣言
        public delegate void IntegralCountUpHandler(int Count, int CountEnd);
        //Integral独自のイベント宣言
        public event IntegralCountUpHandler IntegralCountUp;


        //protected const int IMAGE_X = 1000;
        //protected const int IMAGE_Y = 1000;
        protected const int IMAGE_X = 1024;
        protected const int IMAGE_Y = 1024;

        // 画像サイズ
        private Size imageSize = new Size(IMAGE_X, IMAGE_Y);

        // 透視画像保存データ
        private ushort[] orgImage;

        // 透視画像バイトデータ
        private byte[] byteImage;

        // 表示画像
        private Bitmap picture = null;

        // ウィンドウレベル
        private int windowLevel;

        // ウィンドウ幅
        private int windowWidth;

        // 階調最小値(0固定)
        private int ltMin = 0;

        // 階調最大値
        private int ltMax = (int)LTSize.LT12Bit;

        // 階調用ルックアップテーブル
        private byte[] lt;

        //（反転　０:なし、1:あり）
        private int raster = 0;

        ////Pkeの校正ファイル
        //private string _Gain_Correct_File = null;
        //private string _Gain_Correct_Sft_File = null;
        //private string _Gain_Correct_L_File = null;
        //private string _Gain_Correct_L_Sft_File = null;
        //private string _Gain_Correct_Aire_File = null;
        //private string _Off_Correct_File = null;
        //private string _Def_Correct_File = null;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TransPicture()
        {
            ResizePicture();

            //picture = new Bitmap("test-0001.jpg");
        }

        /// <summary>
        /// 階調最大値
        /// </summary>
        public int LTMax { get { return ltMax; } }

        /// <summary>
        /// 階調最小値
        /// </summary>
        public int LTMin { get { return ltMin; } }

        #region コメント追加用
        //Rev22.00 コメント追加用 by長野 2015/07/03
        private bool myCommentFlg = false;
        public bool CommentFlg
        {
            get
            {
                return (myCommentFlg);
            }
            set
            {
                myCommentFlg = value;
            }
        }

        private string myStr1 = " ";
        public string Str1
        {
            get
            {
                return (myStr1);
            }
            set
            {
                myStr1 = value;
            }
        }

        private string myStr2 = " ";
        public string Str2
        {
            get
            {
                return (myStr2);
            }
            set
            {
                myStr2 = value;
            }
        }

        private string myStr3 = " ";
        public string Str3
        {
            get
            {
                return (myStr3);
            }
            set
            {
                myStr3 = value;
            }
        }

        private string myStr4 = " ";
        public string Str4
        {
            get
            {
                return (myStr4);
            }
            set
            {
                myStr4 = value;
            }
        }

        private int myPosNum = 1;
        public int PosNum
        {
            get
            {
                return (myPosNum);
            }
            set
            {
                myPosNum = value;
            }
        }

        private int myColor = 128;
        public int CommentColor
        {
            get
            {
                return (myColor);
            }
            set
            {
                myColor = value;
            }
        }

        private int mySize = 20;
        public int CommentSize
        {
            get
            {
                return (mySize);
            }
            set
            {
                mySize = value;
            }
        }

        private int myPosX = 100;
        public int PosX
        {
            get
            {
                return (myPosX);
            }
            set
            {
                myPosX = value;
            }
        }

        private int myPosY = 100;
        public int PosY
        {
            get
            {
                return (myPosY);
            }
            set
            {
                myPosY = value;
            }
        }

        private int myMarginPix = 10;
        public int marginPix
        {
            get
            {
                return myMarginPix;
            }
            set
            {
                myMarginPix = value;
            }
        }
        #endregion

        /// <summary>
        /// 階調サイズ設定
        /// </summary>
        /// <param name="ltSize"></param>
        public void SetLTSize(LTSize ltSize)
        {
            ltMax = (int)ltSize;
            //windowLevel = (ltMin + ltMax) / 2;
            windowLevel = Convert.ToInt32((ltMin + ltMax) / 2F);
            windowWidth = ltMax - ltMin + 1;
            ResizeLT();
        }

        /// <summary>
        /// 階調用ルックアップテーブル初期化
        /// </summary>
        private void ResizeLT()
        {
            //変更2015/02/26hata
            //lt = new byte[ltMax - ltMin];
            lt = new byte[ltMax - ltMin　+　1];
            GC.Collect();
        }

        /// <summary>
        /// 画像リサイズ
        /// </summary>
        private void ResizePicture()
        {
            int length = imageSize.Width * imageSize.Height;
            orgImage = new ushort[length];
            byteImage = new byte[length];

            // ビットマップリサイズ
            ResizeBitmap(imageSize.Width, imageSize.Height);
            GC.Collect();
        }

        /// <summary>
        /// ビットマップリサイズ処理
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private bool ResizeBitmap(int width, int height)
        {
            //前回のビットマップのサイズと異なる場合，前回分を破棄
            if (picture != null)
            {
                if (width != picture.Width || height != picture.Height)
                {
                    picture.Dispose();
                    picture = null;
                }
            }

            //リサイズされなかった場合
            if (picture != null) return false;

            //ビットマップの生成（1バイト/ピクセル）
            picture = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            //パレットの設定
            picture.Palette = GetGrayScalePalette();

            //リサイズされた
            return true;
        }

        /// <summary>
        /// グレースケール256階調用パレットを取得する
        /// </summary>
        /// <returns></returns>
        private static ColorPalette GetGrayScalePalette()
        {
            //パレット取得用ビットマップの生成
            Bitmap bitmap = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);

            //パレット取得
            ColorPalette palette = bitmap.Palette;

            //ビットマップの破棄
            bitmap.Dispose();

            //パレットをグレースケール256階調用にする
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }

            return palette;
        }

        /// <summary>
        /// 階調変換用ルックアップテーブル更新
        /// </summary>
        private void ChangeLT()
        {
            lock (lt)
            {
                // 階調変換用ルックアップテーブルの作成
                ImgProc.MakeLT(lt, lt.Length, windowLevel, windowWidth);
            }
            
            // 画像更新
            if (AutoUpdate)
            {
                MakePicture(orgImage);
                OnTransImageChanged();
            }
            
        }

        /// <summary>
        /// ウィンドウレベル
        /// </summary>
        public int WindowLevel
        {
            get { return windowLevel; }
            set
            {
                windowLevel = value;
                if (windowLevel > ltMax) windowLevel = ltMax;
                if (windowLevel < ltMin) windowLevel = ltMin;
                ChangeLT();
            }
        }

        /// <summary>
        /// ウィンドウ幅 ltMax - ltMin + 1
        /// </summary>
        public int WindowWidth
        {
            get { return windowWidth; }
            set
            {
                windowWidth = value;
                //変更2015/02/26hata
                //if (windowWidth > ltMax) windowWidth = ltMax;
                //if (windowWidth < ltMin) windowWidth = ltMin;
                if (windowWidth > (ltMax - ltMin + 1)) windowWidth = ltMax - ltMin + 1;
                if (windowWidth < 1) windowWidth = 1;
                ChangeLT();
            }
        }

        /// </summary>
        /// 白黒反転
        /// </summary>
        public int RasterOp
        {
            get { return raster; }
            set
            {
                if (value == raster) return;
                raster = value;
                MakePicture(orgImage);
                OnTransImageChanged();
            }
        }


        /// <summary>
        /// 画像ビットマップ取得
        /// </summary>
        public Bitmap Picture
        {
            get {
                //描画でエラーするためコピーを渡す
                lock (picture)
                {
                    return (Bitmap)picture.Clone();
                }

            }
        }

        /// <summary>
        /// バイト画像データ取得(コピー)
        /// </summary>
        /// <returns></returns>
        public byte[] GetByteImage()
        {
            return (byte[])byteImage.Clone();
        }

        /// <summary>
        /// 画像データ取得(コピー)
        /// </summary>
        /// <returns></returns>
        public ushort[] GetImage()
        {
            lock (orgImage)
            {
                return (ushort[])orgImage.Clone();
            }
        }

        /// <summary>
        /// 画像データセット
        /// </summary>
        /// <param name="image"></param>
        public void SetImage(ushort[] image)
        {
            if (image.Length == orgImage.Length)
            {
                
                Array.Copy(image, orgImage, orgImage.Length);
                MakePicture(orgImage);
            }
        }

        /// <summary>
        /// 画像自動更新
        /// </summary>
        public bool AutoUpdate { get; set; }

        /// <summary>
        /// 画像ミラー
        /// </summary>
        public bool MirrorOn { get; set; }


        /// <summary>
        /// 画像サイズ
        /// </summary>
        public Size ImageSize
        {
            get { return imageSize; }
            set
            {
                if ((!value.IsEmpty) && (value.Width > 0) && (value.Height > 0))
                {
                    imageSize = value;
                    ResizePicture();
                }
            }
        }

         /// <summary>
        /// 画像作成
        /// </summary>
        /// <param name="image"></param>
        public void MakePicture(ushort[] image)
        {
            // 階調変換用ルックアップテーブルに基づきバイト配列に変換
            if (MirrorOn)
            {
                ImgProc.ConvertWordToByteMirror(image, byteImage, imageSize.Width, imageSize.Height, lt);
            }
            else
            {
                ImgProc.ConvertWordToByte(image, byteImage, image.Length, lt);
            }

            //Rev22.00 追加 by長野 2015/07/03
            if (myCommentFlg == true )
            {
                Pulsar.Mil9AddComment(Pulsar.hMil, imageSize.Width, imageSize.Height, myPosX, myPosY,mySize,myColor,myStr1,myStr2,myStr3,myStr4,ref byteImage[0],myMarginPix);
            }


            // ビットマップ更新
            if (picture != null)
            {
                try
                {
                    lock (picture)
                    {
                        //LocBitsを使用してデータを書き換える
                        Rectangle rect = new Rectangle(0, 0, imageSize.Width, imageSize.Height);
                        //ビットマップデータをロックする
                        BitmapData bmpData = picture.LockBits(rect, ImageLockMode.ReadWrite, picture.PixelFormat);
                        //ビットマップデータに変換する
                        //Pulsar.DrawBitmap(byteImage, bmpData.Scan0, bmpData.Width, bmpData.Height, bmpData.Stride);
                        Pulsar.DrawBitmap2(byteImage, bmpData.Scan0, bmpData.Width, bmpData.Height, bmpData.Stride, raster);
                        //ビットマップデータに対するロックを解除
                        picture.UnlockBits(bmpData);
                    }                  
                }
                catch
                {
                }
                finally
                {
                }

            }
        }


        //透視画像表示用のイベント
        protected void OnTransImageChanged()
        {
            if (TransImageChanged != null)
            {
                TransImageChanged(this, EventArgs.Empty);
            }
        }

        //積分用のカウントイベント
        protected void OnIntegralCountUp(int Count, int CountEnd)
        {
            if ( IntegralCountUp != null)
            {
                IntegralCountUp(Count ,CountEnd);
            }
        }
    
    }
}
