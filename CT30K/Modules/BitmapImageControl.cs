using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
//
using CTAPI;

namespace CT30K
{

    #region  階調サイズ
    /// <summary>
    /// 階調サイズ
    /// </summary>
    public enum LookupTableSize : int
    {
        LT16Bit = 65535,
        LT12Bit = 4095,
        LT10Bit = 1023,
        LT8Bit = 255,
        LTScanImg = 8191
    }
    #endregion

    public class BitmapImageControl
    {
        #region サポートしているイベント
        //画像更新イベント
        public event EventHandler ImageChanged;
        //Integral独自のデリゲート宣言
        //public delegate void IntegralCountUpHandler(int Count, int CountEnd);
        //Integral独自のイベント宣言
        //public event IntegralCountUpHandler IntegralCountUp;
        #endregion


        #region メンバー変数
        protected const int IMAGE_X = 1024;
        protected const int IMAGE_Y = 1024;

        // 画像サイズ
        private Size imageSize = new Size(IMAGE_X, IMAGE_Y);

        // 透視画像保存データ
        private ushort[] orgImage;

        // 透視画像バイトデータ
        private byte[] byteImage;

        //Rev20.00 追加 by長野 2014/12/04
        private short[] orgSImage;

        // 表示画像
        private Bitmap picture = null;

        // ウィンドウレベル
        private int windowLevel = 1;

        // ウィンドウ幅
        private int windowWidth = 1;

        // 階調最小値(デフォルト:0)
        private int ltMin = 0;

        // 階調最小値(透視像の最小値)
        private const int ltTransImgMin = 0;

        // 階調最小値(断面像の最小値)
        private const int ltScanImgMin = -8192;

        // 階調最大値
        private int ltMax = (int)LookupTableSize.LT12Bit;

        // 階調用ルックアップテーブル
        private byte[] lt;

        //（反転　０:なし、1:あり）
        private int raster = 0;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BitmapImageControl()
        {
            ResizePicture();

        }
        #endregion
        
        #region プロパティ
        /// <summary>
        /// 階調最大値
        /// </summary>
        public int LTMax { get { return ltMax; } }

        /// <summary>
        /// 階調最小値
        /// </summary>
        public int LTMin { get { return ltMin; } }

        /// <summary>
        /// 画像自動更新
        /// </summary>
        //public bool AutoUpdate { get; set; }

        /// <summary>
        /// 画像ミラー
        /// </summary>
        public bool MirrorOn { get; set; }

        /// <summary>
        /// 表示サイズ
        /// </summary>
        public Size ViewSize { get; set; }

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
        /// ウィンドウ幅
        /// </summary>
        public int WindowWidth
        {
            get { return windowWidth; }
            set
            {
                windowWidth = value;
                if (windowWidth > ltMax) windowWidth = ltMax;
                if (windowWidth < ltMin) windowWidth = ltMin;
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
                OnImageChanged();
            }
        }


        /// <summary>
        /// 画像ビットマップ取得
        /// </summary>
        public Bitmap Picture
        {
            get
            {
                //描画でエラーするためコピーを渡す
                lock (picture)
                {
                    return (Bitmap)picture.Clone();
                }

            }
        }
        #endregion


        #region パブリックメソッド

        /// <summary>
        /// 階調サイズ設定
        /// </summary>
        /// <param name="ltSize"></param>
        public void SetLTSize(LookupTableSize ltSize)
        {
            if (ltSize == LookupTableSize.LTScanImg)
            {
                ltMin = ltScanImgMin;
            }
            else
            {
                ltMin = ltTransImgMin;
            }

            ltMax = (int)ltSize;
            //2014/11/13hata キャストの修正
            //windowLevel = (ltMin + ltMax) / 2;
            windowLevel = Convert.ToInt32((ltMin + ltMax) / 2F);
            windowWidth = ltMax - ltMin + 1;
            ResizeLT();
        }
        #endregion


        #region プライベートメソッド
        /// <summary>
        /// 階調用ルックアップテーブル初期化
        /// </summary>
        private void ResizeLT()
        {
            lt = new byte[ltMax - ltMin];
            //GC.Collect();
        }

        /// <summary>
        /// 画像リサイズ
        /// </summary>
        private void ResizePicture()
        {
            int length = imageSize.Width * imageSize.Height;
            orgImage = new ushort[length];
            //Rev20.00 追加 by長野 2014/12/04
            orgSImage = new short[length];

            byteImage = new byte[length];

            // ビットマップリサイズ
            ResizeBitmap(imageSize.Width, imageSize.Height);
            //GC.Collect();
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
            //if (AutoUpdate)
            //{
            //    MakePicture(orgImage);
            //}
            MakePicture(orgImage);
            
        }
       #endregion



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
            return (ushort[])orgImage.Clone();
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
        /// 画像データセット
        /// </summary>
        /// <param name="image"></param>
        public void SetImageMirror(ushort[] image)
        {
            if (image.Length == orgImage.Length)
            {
                Array.Copy(image, orgImage, orgImage.Length);
                MakePictureMirror(orgImage);
            }

        }

        public void SetImage(ushort[] image, int Width, int Height )
        {
            if (image.Length == orgImage.Length)
            {
                //サイズが同じ場合
                SetImage(image);
            }
            else
            {   
                //サイズが違う場合
                ImageSize = new Size(Width, Height);
                MakePicture(orgImage);            
            }    
        }


         /// <summary>
        /// 画像作成
        /// </summary>
        /// <param name="image"></param>
        public void MakePicture(ushort[] image)
        {
            //Rev20.00 追加 by長野 2014/12/04
            //ltMinが-8192の時は、CT画像(short)と判断
            if (this.ltMin == -8192)
            {
                for (int cnt = 0; cnt < image.Length; cnt++)
                {
                    orgSImage[cnt] = (short)image[cnt];
                }
                //最後の引数は旧ソフトでは変数だが、常に0だったので0としてある。
                ImgProc.ConvertShortToByte(orgSImage, byteImage, orgSImage.Length, lt, lt.Length, 0);
            }
            else
            {            // 階調変換用ルックアップテーブルに基づきバイト配列に変換
                if (MirrorOn)
                {
                    ImgProc.ConvertWordToByteMirror(image, byteImage, imageSize.Width, imageSize.Height, lt);
                }
                else
                {
                    ImgProc.ConvertWordToByte(image, byteImage, image.Length, lt);
                }
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

        /// <summary>
        /// 画像作成
        /// </summary>
        /// <param name="image"></param>
        public void MakePictureMirror(ushort[] image)
        {
            //Rev20.00 追加 by長野 2014/12/04
            //ltMinが-8192の時は、CT画像(short)と判断
            if (this.ltMin == -8192)
            {
                for (int cnt = 0; cnt < image.Length; cnt++)
                {
                    orgSImage[cnt] = (short)image[cnt];
                }
                //最後の引数は旧ソフトでは変数だが、常に0だったので0としてある。
                ImgProc.ConvertShortToByte(orgSImage, byteImage, orgSImage.Length, lt, lt.Length, 0);
            }
            else
            {   // 階調変換用ルックアップテーブルに基づきバイト配列に変換
                ImgProc.ConvertWordToByteMirror(image, byteImage, imageSize.Width, imageSize.Height, lt);
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

        //画像表示用のイベント
        protected void OnImageChanged()
        {
            if (ImageChanged != null)
            {
                ImageChanged(this, EventArgs.Empty);
            }
        }

    }
}
