using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;

namespace CT30K
{
    /// <summary>
    /// ＣＴ画像コントロールクラス
    /// </summary>
    public partial class CTImageControl : UserControl
    {
        #region 定数
        /// <summary>
        /// ＣＴ値最小値
        /// </summary>
        private const short CT_MIN = -8192;

        /// <summary>
        /// ＣＴ値最大値
        /// </summary>
        private const short CT_MAX = 8191;
        #endregion

        #region メンバ変数
        /// <summary>
        /// ビットマップ変換用ルックアップテーブル
        /// </summary>
        private byte[] myLookupTbl = new byte[CT_MAX - CT_MIN + 1];

        /// <summary>
        /// ビットマップ
        /// </summary>
        private Bitmap myBitmap;

        /// <summary>
        /// 画像最小値
        /// </summary>
        private short myMin = CT_MIN;

        /// <summary>
        /// 画像最大値
        /// </summary>
        private short myMax = CT_MAX;

        /// <summary>
        /// ウィンドウレベル
        /// </summary>
        private int myWindowLevel = (CT_MAX + CT_MIN) / 2;
        
        /// <summary>
        /// ウィンドウ幅
        /// </summary>
        private int myWindowWidth = CT_MAX - CT_MIN + 1;

        /// <summary>
        /// 画像バッファ
        /// </summary>
        private short[] myImage;

        /// <summary>
        /// グレースケールパレット
        /// </summary>
        private static ColorPalette GrayScale256 = GetGrayScalePalette();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CTImageControl()
        {
            InitializeComponent();

            // 描画スタイルの設定
            this.SetStyle(
                ControlStyles.DoubleBuffer |         // 描画をバッファで実行する
                ControlStyles.UserPaint |            // 描画は（ＯＳでなく）独自に行う
                ControlStyles.AllPaintingInWmPaint,  // WM_ERASEBKGND を無視する( UserPaint ビットが true に設定されている場合に適用） 
                 true                                // 指定したスタイルを適用「する」
                );

        }
        #endregion

        #region プロパティ
        /// <summary>
        /// 画像値最小
        /// </summary>
        public short Min
        {
            get { return myMin; }
            set
            {
                if (value > myMax) return;

                myMin = value;

                // ビットマップ変換用ルックアップテーブルのリサイズ
                ResizeLookupTbl();
            }
        }

        /// <summary>
        /// 画像値最大
        /// </summary>
        public short Max
        {
            get { return myMax; }
            set
            {
                if (value < myMin) return;

                myMax = value;

                // ビットマップ変換用ルックアップテーブルのリサイズ
                ResizeLookupTbl();
            }
        }

        /// <summary>
        /// ウィンドウレベル
        /// </summary>
        public int WindowLevel
        {
            get { return myWindowLevel; }
            set { myWindowLevel = value; UpdateLookupTbl(); }
        }

        /// <summary>
        /// ウィンドウ幅
        /// </summary>
        public int WindowWidth
        {
            get { return myWindowWidth; }
            set { myWindowWidth = value; UpdateLookupTbl(); }
        }

        /// <summary>
        /// 画像の幅
        /// </summary>
        public int ImageWidth
        {
            get { return (myBitmap == null) ? 0 : myBitmap.Width; }
        }

        /// <summary>
        /// 画像の高さ
        /// </summary>
        public int ImageHeight
        {
            get { return (myBitmap == null) ? 0 : myBitmap.Height; }
        }

        /// <summary>
        /// 画像ビットマップ取得
        /// </summary>
        public Bitmap Picture
        {
            get { return myBitmap; }
 
        }

        #endregion

        #region パブリックメソッド

        //テスト用
        /// <summary>
        /// ビットマップをセットする
        /// </summary>
        /// <param name="sBmp"></param>
        public void SetPicture(Bitmap sBmp)
        {
            // ビットマップのサイズが異なっている場合
            if (myBitmap == null || myBitmap.Width != sBmp.Width || myBitmap.Height != sBmp.Height)
            {
                // ビットマップの生成（1バイト/ピクセル）
                myBitmap = new Bitmap(sBmp.Width, sBmp.Height, PixelFormat.Format8bppIndexed);
                myBitmap = sBmp;
            }
            else
            {
                myBitmap = sBmp;
            }
        }

        //テスト用
        /// <summary>
        /// 画像をセットする
        /// </summary>
        /// <param name="sBmp"></param>
        public void SetImage(byte[] image)
        {

            try
            {
                // 読み込み用バッファの準備
                //byte[] buf = new byte[image.Length];


                // ＣＴ値画像バッファの作成
                UpdateImage(image);

                // マトリクスを求める
                int matrix = (int)Math.Sqrt(myImage.Length);

                // ビットマップ更新
                UpdateBitmap(matrix, matrix);

            }
            catch (Exception ex)
            {
                // エラーメッセージ表示
                MessageBox.Show(ex.Message,
                                Application.ProductName,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
            }

        }

        #region ファイルから画像をロードする
        /// <summary>
        /// ファイルから画像をロードする
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="windowLevel"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public bool LoadImage(string fileName, int windowLevel, int windowWidth)
        {
            myImage = null;
            myWindowLevel = windowLevel;
            myWindowWidth = windowWidth;
            UpdateLookupTbl();

            return LoadImage(fileName);
        }

        /// <summary>
        /// ファイルから画像をロードする
        /// </summary>
        /// <param name="fileName"></param>
        public bool LoadImage(string fileName)
        {
            // 戻り値用変数
            bool result = false;

            try
            {
                // ファイルを開く
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    // 読み込み用バッファの準備
                    byte[] buf = new byte[fs.Length];

                    // ファイルの内容をすべて読み込む
                    int size = fs.Read(buf, 0, (int)fs.Length);

                    // ＣＴ値画像バッファの作成
                    UpdateImage(buf);

                    // 閉じる
                    fs.Close();

                    // 戻り値セット
                    result = (buf.Length == size);
                }

                if (result)
                {
                    // マトリクスを求める
                    int matrix = (int)Math.Sqrt(myImage.Length);

                    // ビットマップ更新
                    UpdateBitmap(matrix, matrix);
                }
            }
            catch (Exception ex)
            {
                // エラーメッセージ表示
                MessageBox.Show(ex.Message,
                                Application.ProductName,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
            }

            return result;
        }
        #endregion

        #region 指定した領域のＣＴ値の配列を取得する
        /// <summary>
        /// 指定した領域のＣＴ値の配列を取得する
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public short[,] GetCTValues(Rectangle rect) 
        {
            // 戻り値用変数
            short[,] result = new short[rect.Height, rect.Width];

            for (int i = result.GetLowerBound(0); i <= result.GetUpperBound(0); i++)
            {
                for (int j = result.GetLowerBound(1); j <= result.GetUpperBound(1); j++)
                {
                    int x = rect.Left + j;
                    int y = rect.Top + i;
                    result[i, j] = myImage[y * this.ImageWidth + x]; 
                }
            }

            return result;
        }
        #endregion

        #endregion

        #region プライベートメソッド

        #region ビットマップ変換用ルックアップテーブルのリサイズ
        /// <summary>
        /// ビットマップ変換用ルックアップテーブルのリサイズ
        /// </summary>
        private void ResizeLookupTbl()
        {
            myLookupTbl = new byte[myMax - myMin + 1];
        }
        #endregion

        #region ビットマップ変換用ルックアップテーブルの更新
        /// <summary>
        /// ビットマップ変換用ルックアップテーブルの更新
        /// </summary>
        private void UpdateLookupTbl()
        {
            float rate = (float)byte.MaxValue / myWindowWidth;

            // テーブル範囲設定
            int low = myWindowLevel - (myWindowWidth / 2) - myMin;
            int high = myWindowLevel + (myWindowWidth / 2) - myMin;

            lock (myLookupTbl)  //同期を取る（BMP表示の時にエラーするため）

            //テーブルを作成
            for (int i = 0; i < myLookupTbl.Length; i++)
            {
                if (i < low)
                    myLookupTbl[i] = byte.MinValue;
                else if (i > high)
                    myLookupTbl[i] = byte.MaxValue;
                else
                    myLookupTbl[i] = (byte)(rate * (i - low));
            }
 
            // ビットマップの更新
            UpdateBitmap();
        }
        #endregion

        #region ルックアップテーブルに基づいてビットマップデータを作成する
        /// <summary>
        /// ルックアップテーブルに基づいてビットマップデータを作成する
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void UpdateBitmap(int width, int height)
        {
            // ビットマップのサイズが異なっている場合
            if (myBitmap == null || myBitmap.Width != width || myBitmap.Height != height)
            {
                // ビットマップの生成（1バイト/ピクセル）
                myBitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

                // パレットの設定
                myBitmap.Palette = GrayScale256;
            }

            // ルックアップテーブルに基づいてビットマップデータを作成する
            UpdateBitmap();
        }

        /// <summary>
        /// ルックアップテーブルに基づいてビットマップデータを作成する
        /// </summary>
        private void UpdateBitmap()
        {
            if (myBitmap == null) return;
            if (myImage == null) return;

            // ビットマップデータをロック
            Rectangle rect = new Rectangle(0, 0, myBitmap.Width, myBitmap.Height);
            BitmapData bmpData = myBitmap.LockBits(rect, ImageLockMode.ReadWrite, myBitmap.PixelFormat);

            unsafe
            {
                // ＣＴ画像データをビットマップデータに変換
                byte* p = (byte*)bmpData.Scan0;

                for (int i = 0; i < myImage.Length; i++)
                {
                    *p++ = myLookupTbl[myImage[i] - myMin];
                }
            }

            //// ビットマップデータに対するロックを解除
            myBitmap.UnlockBits(bmpData);

            // リフレッシュ
            this.Refresh();
        }
        #endregion

        #region 画像バッファの作成
        /// <summary>
        /// 画像バッファの作成
        /// </summary>
        /// <param name="buf"></param>
        private void UpdateImage(byte[] buf)
        {
            // 画像配列確保
            myImage = new short[buf.Length / 2];
            
            // 読み込んだbyteデータを画像バッファに読み込む
            for (int i = 0, j = 0; i < myImage.Length; i++)
            {
                short ct = buf[j++];
                ct <<= 8;
                ct += buf[j++];

                // 範囲内におさめて格納
                myImage[i] = Math.Max(Math.Min(ct, myMax), myMin);
            }
        }
        #endregion

        #region グレースケール256階調用パレットを取得する（スタティック）
        /// <summary>
        /// グレースケール256階調用パレットを取得する
        /// </summary>
        /// <returns></returns>
        private static ColorPalette GetGrayScalePalette()
        {
            // パレット取得用ビットマップの生成
            Bitmap bitmap = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);

            // パレット取得
            ColorPalette palette = bitmap.Palette;

            // ビットマップの破棄
            bitmap.Dispose();

            // パレットをグレースケール256階調用にする
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }

            return palette;
        }
        #endregion

        #endregion

        #region オーバーライドされたメソッド
        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
            
            if (myBitmap == null) return;
            //ビットマップ描画
            e.Graphics.DrawImage(myBitmap, this.ClientRectangle);


            //if (myBitmap != null)
            //{
            //    RectangleF pic_rect = new RectangleF(0, 0, myBitmap.Width, myBitmap.Height);
            //    RectangleF rect = new RectangleF(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            //    if ((pic_rect.Height > ClientSize.Height) || (pic_rect.Width > ClientSize.Width))
            //    {
            //        // 縮小
            //        if (pic_rect.Height > pic_rect.Width)
            //        {
            //            float rate = (float)ClientSize.Height / pic_rect.Height;
            //            rect.Width = rate * pic_rect.Width;
            //        }
            //        else
            //        {
            //            float rate = (float)ClientSize.Width / pic_rect.Width;
            //            rect.Height = rate * pic_rect.Height;
            //        }
            //    }
            //    else
            //    {
            //        rect.Height = pic_rect.Height;
            //        rect.Width = pic_rect.Width;
            //    }
            //    rect.X = (ClientSize.Width - rect.Width) / 2;
            //    rect.Y = (ClientSize.Height - rect.Height) / 2;

            //    e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //    e.Graphics.DrawImage(myBitmap, rect, pic_rect, GraphicsUnit.Pixel);
            //}

        }

       
        #endregion

    }
}