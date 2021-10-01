using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

//
//using TransImage;
//using CTAPI;

namespace CT30K
{

    /// <summary>
    /// 線タイプ
    /// </summary>
    public enum LineConstants
    {
        ScanLine,       // スキャンライン
        UpperLine,      // コーンビーム時の上端ライン
        LowerLine,      // コーンビーム時の下端ライン
        CenterLine,     // 中心線(縦)
        CenterLineH,    // 中心線(横)
        ProfilePosV,    // ラインプロファイル(垂直位置) //Rev25.00 追加 by長野 2016/08/08
        ProfilePosH,    // ラインプロファイル(水平位置) //Rev25.00 追加 by長野 2016/08/08
        ProfileV,       // ラインプロファイル(垂直) //Rev25.00 追加 by長野 2016/08/08
        ProfileH,       // ラインプロファイル(水平) //Rev25.00 追加 by長野 2016/08/08
        Other
    }

    /// <summary>
    /// 文字列タイプ //Rev25.00 追加 by長野 2016/08/08
    /// </summary>
    public enum StringConstants
    {
        Profile0PosV,       // ラインプロファイル(垂直0%位置) //Rev25.00 追加 by長野 2016/08/08
        Profile100PosV,     // ラインプロファイル(垂直100%位置) //Rev25.00 追加 by長野 2016/08/08
        Profile0PosH,       // ラインプロファイル(水平0%位置) //Rev25.00 追加 by長野 2016/08/08
        Profile100PosH,     // ラインプロファイル(水平100%位置) //Rev25.00 追加 by長野 2016/08/08
        Other
    }

    public partial class CTImageCanvas : UserControl
    {
        #region サポートしているイベント
        //サポートしているイベント
        //public event MouseEventHandler UCMouseDown;
        //public event MouseEventHandler UCMouseUp;
        //public event MouseEventHandler UCMouseMove;
        //public event PaintEventHandler UCPaint;
        #endregion

        //別スレッドからの表示用　//追加2014/09/20hata
        delegate void PictureUpDateDelegate();

        // 線
        private Dictionary<LineConstants, TransLine> lines = new Dictionary<LineConstants, TransLine>();

        // 文字列 //Rev25.00 追加 by長野 2016/08/08
        private Dictionary<StringConstants, TransString> strings = new Dictionary<StringConstants, TransString>();


        //ルックアップテーブルが変更されたら自動で画像を更新するか
        //bool myAutoUpdate;
        //int myMin;
        //int myMax;

        /// <summary>
        /// <summary> 白黒反転（０:なし、1:あり）
        /// </summary>
        //private int myRasterOp = 0;

        /// <summary>
        /// <summary> 左右反転（０:なし、1:あり）
        /// </summary>
        private bool myMirrorOn = false;

        /// <summary>
        /// <summary> ビットマップ画像横サイズ
        /// </summary>
        private int mySizeX = 1024;

        /// <summary>
        /// <summary> ビットマップ画像縦サイズ
        /// </summary>
        private int mySizeY = 1024;

        /// <summary>
        /// <summary> FCD/FID比
        /// </summary>
        private double myFCDFIDRate = 0;


        //// 画像サイズ
        //protected const int IMAGE_X = 1024;
        //protected const int IMAGE_Y = 1024;

        //// 画像サイズ
        //private Size imageSize = new Size(IMAGE_X, IMAGE_Y);

        //// 透視画像保存データ
        //private ushort[] orgImage;

        //// 透視画像バイトデータ
        //private byte[] byteImage;

        // 表示画像
        //private Bitmap picture = null;
        private Bitmap dispPicture;

        //// ウィンドウレベル
        //private int windowLevel = 1;

        //// ウィンドウ幅
        //private int windowWidth = 1;

        //// 階調最小値(0固定)
        //private int ltMin = 0;

        //// 階調最大値
        //private int ltMax = (int)LTSize.LT12Bit;

        //// 階調用ルックアップテーブル
        //private byte[] lt;

        /// <summary>
        /// <summary> グレースケールパレット
        /// </summary>
        private static ColorPalette GrayScale256 = GetGrayScalePalette();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CTImageCanvas()
        {
            InitializeComponent();

            Init();
        }


        #region コントロールの初期化
        private void Init()
        {
            //***　ControlStyles.Opaque　****************
            //ControlStyles.Opaque　//無効領域の背景を消去しない
            //ControlStyles.Opaqueはちらつき防止に使用するが
            //DoubleBuffer=trueの場合は、これでちらつき防止ができるため不要
            //Opaque=trueの場合はOnPaintBackgroundが出ない。

            ////リサイズ時に自動的に描画する
            //this.SetStyle(ControlStyles.ResizeRedraw, true);
            ////無効領域の背景を消去しない
            ////this.SetStyle(ControlStyles.Opaque, true);
            ////ダブルバッファを有効にする
            //this.DoubleBuffered = true;

            // 描画スタイルの設定
            this.SetStyle(
                ControlStyles.DoubleBuffer |         // 描画をバッファで実行する
                ControlStyles.UserPaint |            // 描画は（ＯＳでなく）独自に行う
                ControlStyles.AllPaintingInWmPaint,  // WM_ERASEBKGND を無視する( UserPaint ビットが true に設定されている場合に適用） 
                 true                                // 指定したスタイルを適用「する」
                );

            // ビットマップの生成（1バイト/ピクセル）
            dispPicture = new Bitmap(mySizeX, mySizeY, PixelFormat.Format8bppIndexed);

            // パレットの設定
            dispPicture.Palette = GrayScale256;

            //transImageControlに処理移動
            //--------------------------------------------------------------
            //myAutoUpdate = true;            //追加 2009/08/18
            //myMin = 0;
            //myMax = (modGlobal.DetType == modGlobal.DetectorConstants.DetTypePke ? 65535 : 4095);
            //v17.10変更 byやまおか 2010/07/28
            //myWindowLevel = (myMin + myMax) / 2;
            //myWindowWidth = myMax - myMin + 1;
            //ResizeLT();
            //--------------------------------------------------------------

            //mySizeX = 320;
            //mySizeY = 240;

            myMirrorOn = false;            //v17.50追加 by 間々田 2010/12/28 画像左右反転（Pke時）対応
            
            //ResizePicture();  //transImageControlに処理移動

            InitControls();

        }


        /// <summary>
        /// コントロールの初期化
        /// </summary>
        //*******************************************************************************
        //機　　能： コントロールの初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        private void InitControls()
        {
            //ターゲットプロパティリセット
            //Target = "";

            //var _with4 = ctlTransImage;
            //_with4.SizeX = h_size;
            //_with4.SizeY = v_size;

            // 線
            lines.Add(LineConstants.ScanLine, new TransLine(LineConstants.ScanLine));
            lines.Add(LineConstants.UpperLine, new TransLine(LineConstants.UpperLine));
            lines.Add(LineConstants.LowerLine, new TransLine(LineConstants.LowerLine));
            lines.Add(LineConstants.CenterLine, new TransLine(LineConstants.CenterLine));
            lines.Add(LineConstants.CenterLineH, new TransLine(LineConstants.CenterLineH));
            lines.Add(LineConstants.ProfilePosV, new TransLine(LineConstants.ProfilePosV)); //Rev25.00 垂直プロファイル追加 by長野 2016/08/08
            lines.Add(LineConstants.ProfilePosH, new TransLine(LineConstants.ProfilePosH)); //Rev25.00 水平プロファイル追加 by長野 2016/08/08
            lines.Add(LineConstants.ProfileV, new TransLine(LineConstants.ProfileV)); //Rev25.00 垂直プロファイル追加 by長野 2016/08/08
            lines.Add(LineConstants.ProfileH, new TransLine(LineConstants.ProfileH)); //Rev25.00 水平プロファイル追加 by長野 2016/08/08

            //スキャン位置の線
            var _with5 = lines[LineConstants.ScanLine];
            //_with5.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Lime);
            _with5.Visible = true;

            //スライス厚の上側の線
            var _with6 = lines[LineConstants.UpperLine];
            //_with6.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
            _with6.Visible = true;

            //スライス厚の下側の線
            var _with7 = lines[LineConstants.LowerLine];
            //_with7.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
            _with7.Visible = true;

            //中心線（縦）
            var _with8 = lines[LineConstants.CenterLine];
            //_with8.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Cyan);
            _with8.Visible = true;

            //中心線（横）   'v15.10追加 byやまおか 2009/10/22
            var _with9 = lines[LineConstants.CenterLineH];
            //_with9.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Cyan);
            _with9.Visible = false;

            //ラインプロファイル 垂直方向 //Rev25.00 追加 by長野 2016/08/08
            var _with10 = lines[LineConstants.ProfilePosV];
            _with10.Visible = false;

            //ラインプロファイル 水平方向 //Rev25.00 追加 by長野 2016/08/08
            var _with11 = lines[LineConstants.ProfilePosH];
            _with11.Visible = false;

            //Rev25.00 文字列 by長野 2016/08/08
            //水平0%
            strings.Add(StringConstants.Profile0PosH, new TransString(StringConstants.Profile0PosH));
            var _with12 = strings[StringConstants.Profile0PosH];
            _with12.Visible = false;

            //水平100%
            strings.Add(StringConstants.Profile100PosH, new TransString(StringConstants.Profile100PosH));
            var _with13 = strings[StringConstants.Profile100PosH];
            _with13.Visible = false;

            //垂直0%
            strings.Add(StringConstants.Profile0PosV, new TransString(StringConstants.Profile0PosV));
            var _with14 = strings[StringConstants.Profile0PosV];
            _with14.Visible = false;

            //垂直100%
            strings.Add(StringConstants.Profile100PosV, new TransString(StringConstants.Profile100PosV));
            var _with15 = strings[StringConstants.Profile100PosV];
            _with15.Visible = false;

            //ラインプロファイル 垂直位置
            var _with16 = lines[LineConstants.ProfileV];
            _with14.Visible = false;

            //ラインプロファイル 水平位置
            var _with17 = lines[LineConstants.ProfileH];
            _with15.Visible = false;

        }
        #endregion




        /// <summary>
        /// <summary>画像ビットマップ取得/設定
        /// </summary>
        public Bitmap Picture
        {
            get { return (Bitmap)dispPicture.Clone(); }
            set
            {
                lock (dispPicture)
                {
                    dispPicture = null;
                    dispPicture = value;
                }
                //this.Refresh();
                try
                {
                    PictureUpDate();
                }
                catch
                {
                }
            }
        }


        //別スレッドからの表示処理　//追加2014/09/20hata
        private void PictureUpDate()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new PictureUpDateDelegate(PictureUpDate));
                return;
            }

            try
            {
                this.Refresh();
            }
            catch
            {
            }

        }

        ////*******************************************************************************
        ////機　　能： Enabled プロパティ
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        ////*******************************************************************************
        //public new bool Enabled
        //{
        //    get { return base.Enabled; }
        //    set
        //    {
        //        base.Enabled = value;
        //    }
        //}

        //TransImageControlに移行
        ////*******************************************************************************
        ////機　　能： AutoUpdate プロパティ
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        ////*******************************************************************************
        //public bool AutoUpdate
        //{
        //    get { return myAutoUpdate; }
        //    set
        //    {
        //        myAutoUpdate = value;
        //    }
        //}

        //TransImageControlに移行
        ///// </summary>
        ///// 白黒反転
        ///// </summary>
        //public int RasterOp
        //{
        //    get { return myRasterOp; }
        //    set 
        //    {
        //       if (value == myRasterOp)
        //            return;
        //        myRasterOp = value; 
        //    }
        //}

        /// 左右反転
        /// </summary>
        public bool MirrorOn
        {
            get { return myMirrorOn; }
            set 
            {
                if (value == myMirrorOn)
                    return; 
                myMirrorOn = value;
            }
        }

        //TransImageControlに移行
        //*******************************************************************************
        //機　　能： Min プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
        //public int Min
        //{
        //    get { return myMin; }
        //    set
        //    {
        //        if (value == myMin)
        //            return;
        //        if (value > myMax)
        //            return;
        //        myMin = value;

        //        ResizeLT();
        //    }
        //}
        //public int LTMin { get { return ltMin; } }

        //TransImageControlに移行
        //*******************************************************************************
        //機　　能： Max プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
        //public int Max
        //{
        //    get { return myMax; }
        //    set
        //    {
        //        if (value == myMax)
        //            return;
        //        if (value < myMin)
        //            return;
        //        myMax = value;

        //        ResizeLT();
        //    }
        //}
        //public int LTMax { get { return ltMax; } }

        ///// <summary>
        ///// 階調サイズ設定
        ///// </summary>
        ///// <param name="ltSize"></param>
        //public void SetLTSize(LTSize ltSize)
        //{
        //    ltMax = (int)ltSize;
        //    windowLevel = (ltMin + ltMax) / 2;
        //    windowWidth = ltMax - ltMin + 1;
        //    ResizeLT();
        //}

        /// <summary>
        /// FCD/FID比
        /// </summary>
        public double FCDFIDRate
        {
            get { return myFCDFIDRate; }
            set { myFCDFIDRate = value; }
        }

        /// <summary>
        /// 画像の幅
        /// </summary>
        //*******************************************************************************
        //機　　能： SizeX プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
        public int SizeX 
        {
            get { return mySizeX; }
            set 
            {
                if (value == mySizeX) return; 
                mySizeX = value;

                ////TransImageControlに移行
                ////ResizePicture();
                //ImageSize = new Size(mySizeX, mySizeY);
            }
        }

        /// <summary>
        /// 画像の高さ
        /// </summary>
        //*******************************************************************************
        //機　　能： SizeY プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
        public int SizeY
        {
            get { return mySizeY; }
            set 
            {
                if (value == mySizeY)
                    return; 
                mySizeY = value;

                //TransImageControlに移行
                //ResizePicture();
                //ImageSize = new Size(mySizeX, mySizeY);
            }
        }

        //×××
        ///// <summary>
        ///// 画像サイズ
        ///// </summary>
        //public Size ImageSize
        //{
        //    get { return imageSize; }
        //    set
        //    {
        //        if ((!value.IsEmpty) && (value.Width > 0) && (value.Height > 0))
        //        {
        //            mySizeX = value.Width;
        //            mySizeY = value.Height;
        //            imageSize = value;
        //            ResizePicture();
        //        }
        //    }
        //}

        //TransImageControlに移行
        ////*******************************************************************************
        ////機　　能： WindowLevel プロパティ
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        ////*******************************************************************************
        //public int WindowLevel
        //{
        //    get { return windowLevel; }
        //    set
        //    {
        //        windowLevel = value;
        //        if (windowLevel > ltMax) windowLevel = ltMax;
        //        if (windowLevel < ltMin) windowLevel = ltMin;
        //        ChangeLT();
        //    }
        //}

        //TransImageControlに移行
        ////*******************************************************************************
        ////機　　能： WindowWidth プロパティ
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        ////*******************************************************************************
        //public int WindowWidth
        //{
        //    get { return windowWidth; }
        //    set
        //    {
        //        windowWidth = value;
        //        if (windowWidth > ltMax) windowWidth = ltMax;
        //        if (windowWidth < ltMin) windowWidth = ltMin;
        //        ChangeLT();
        //    }
        //}

        ////TransImageControlに移行
        ////*******************************************************************************
        ////機　　能： 階調変換用ルックアップテーブル初期化
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        ////*******************************************************************************
        ////private void ResizeLT()
        ////{
        ////    LT = new byte[myMax - myMin + 1];
        ////}
        //private void ResizeLT()
        //{
        //    lt = new byte[ltMax - ltMin];
        //    GC.Collect();
        //}


        //TransImageControlに移行
        //*******************************************************************************
        //機　　能： ピクチャオブジェクトリサイズ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
        ////private void ResizePicture()
        ////{

        ////    //すでに存在するピクチャオブジェクトを破棄
        ////    //UPGRADE_NOTE: オブジェクト myPicture をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 
        ////    if ((myPicture != null))
        ////        myPicture = null;

        ////    //ピクチャオブジェクトを生成
        ////    //UPGRADE_ISSUE: UserControl プロパティ UserControl.hdc はアップグレードされませんでした。 
        ////    myPicture = CreatePicture(base.hdc, mySizeX, mySizeY);

        ////    //画像配列も更新
        ////    OrgImage = new short[mySizeX * mySizeY];
        ////    ByteImage = new byte[mySizeX * mySizeY];
        ////    imageSize = Information.UBound(OrgImage) + 1;

        ////}
        //private void ResizePicture()
        //{
        //    int length = imageSize.Width * imageSize.Height;
        //    orgImage = new ushort[length];
        //    byteImage = new byte[length];

        //    // ビットマップリサイズ
        //    ResizeBitmap(imageSize.Width, imageSize.Height);
        //    GC.Collect();
        //}

        ///// <summary>
        ///// ビットマップリサイズ処理
        ///// </summary>
        ///// <param name="width"></param>
        ///// <param name="height"></param>
        ///// <returns></returns>
        //private bool ResizeBitmap(int width, int height)
        //{
        //    //前回のビットマップのサイズと異なる場合，前回分を破棄
        //    if (picture != null)
        //    {
        //        if (width != picture.Width || height != picture.Height)
        //        {
        //            picture.Dispose();
        //            picture = null;
        //        }
        //    }

        //    //リサイズされなかった場合
        //    if (picture != null) return false;

        //    //ビットマップの生成（1バイト/ピクセル）
        //    picture = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

        //    //パレットの設定
        //    picture.Palette = GetGrayScalePalette();

        //    //リサイズされた
        //    return true;
        //}


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
        
        //TransImageControlに移行
        ////*******************************************************************************
        ////機　　能： 線の描画
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        ////*******************************************************************************
        //public void DrawLine(int x1, int y1, int x2, int y2, int Color)
        //{
        //    //'Me.Line (x1, y1) - (x2, y2), Color
        //}

        //TransImageControlに移行
        //*******************************************************************************
        //機　　能： 表示しているピクチャを返す（動画保存用）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
        //public System.Drawing.Image GetPicture()
        //{

        //    //return myPicture;
        //    return (Bitmap)picture.Clone();

        //}


        //override void OnPaintで行う
        //TransImageControlに移行
        ////*******************************************************************************
        ////機　　能： ピクチャを表示する
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        ////*******************************************************************************
        //public void DispPicture()
        //{
        //
        //    //表示
        //    AutoRedraw = true;
        //    PaintPicture(myPicture, 0, 0, ClientRectangle.Width, VB6.PixelsToTwipsY(ClientRectangle.Height), , , , , myRasterOp);
        //    //v17.50追加（参考用） by 間々田 2010/12/28
        //    //   下の方法はmyPictureを貼り付ける際に左右反転させる方法
        //    //   ただし、myPictureはすでに左右反転している（PkeFPDの場合）ので、今までの方法でよい
        //    //PaintPicture myPicture, ScaleWidth, 0, -ScaleWidth, ScaleHeight, , , , , myRasterOp
        //    AutoRedraw = false;
        //
        //}

        //×××コントロールのRefresh()を使用
        ////*******************************************************************************
        ////機　　能： リフレッシュ処理
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： なし
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        ////*******************************************************************************
        //public override void Refresh()
        //{
        //    base.Refresh();
        //}

        //TransImageControlに移行
        ////*******************************************************************************
        ////機　　能： 表示しているバイト画像データ（配列）を取得する
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： Image()         [I/ ] Byte      画像配列
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        ////*******************************************************************************
        //public byte[] GetByteImage()
        //{
        //    return (byte[])byteImage.Clone();
        //}


        //×××ユーザコントロールへ戻す　//2014/0515hata
        //TransImageControlに移行
        ////*******************************************************************************
        ////機　　能： 表示しているバイト画像データ（配列）を取得する
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： Image()         [I/ ] Integer   画像配列
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        ////*******************************************************************************
        //public ushort[] GetImage()
        //{
        //    return (ushort[])orgImage.Clone();
        //}

        ////TransImageControlに移行
        ////*******************************************************************************
        ////機　　能： 画像データ（配列）のセット
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： Image()         [I/ ] Integer   オリジナル画像配列
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        ////*******************************************************************************
        //public void SetImage(ushort[] image)
        //{
        //    if (image.Length == orgImage.Length)
        //    {
        //        //配列のコピー
        //        Array.Copy(image, orgImage, orgImage.Length);
        //        //Pictureの生成
        //        MakePicture(orgImage);
        //    }
        //}

        //TransImageControlに移行
        ////*******************************************************************************
        ////機　　能： 階調変換用ルックアップテーブルの作成
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： theLevel        [I/ ] Long      指定されたウィンドウレベル
        ////           theWidth        [I/ ] Long      指定されたウィンドウ幅
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        ////*******************************************************************************
        //private void ChangeLT()
        //{

        //    lock (lt)
        //    {
        //        // 階調変換用ルックアップテーブルの作成
        //        //MakeLT(lt, lt.Length, windowLevel, windowWidth);
        //        CTSettings.CallMakeLT(ref lt, windowLevel, windowWidth);
        //    }

        //    // 画像更新
        //    //変更 by 間々田 2009/08/18
        //    if (AutoUpdate)
        //    {
        //        MakePicture(orgImage);
        //        //OnTransImageChanged();
        //    }
        //}

        //TransImageControlに移行
        ////*******************************************************************************
        ////機　　能： Pictureの生成
        ////
        ////           変数名          [I/O] 型        内容
        ////引　　数： Image()         [I/ ] Integer   画像配列
        ////戻 り 値： なし
        ////
        ////補　　足： なし
        ////
        ////履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        ////*******************************************************************************
        ////public void MakePictute(ref short[] Image)
        ////{

        ////    // ERROR: Not supported in C#: OnErrorStatement

        ////    //v17.00追加 byやまおか 2010/02/08

        ////    //階調変換用ルックアップテーブルに基づきバイト配列に変換
        ////    if (myMin == 0)
        ////    {
        ////        //v17.50追加 by 間々田 2010/12/28 画像左右反転（Pke時）対応
        ////        if (MirrorOn)
        ////        {
        ////            ConvertWordToByteMirror(ref Image[0], ref ByteImage[0], mySizeX, mySizeY, ref LT[0]);
        ////            //v17.50追加 by 間々田 2010/12/28 画像左右反転（Pke時）対応
        ////            //v17.50追加 by 間々田 2010/12/28 画像左右反転（Pke時）対応
        ////        }
        ////        else
        ////        {
        ////            ConvertWordToByte(ref Image[0], ref ByteImage[0], imageSize, ref LT[0]);
        ////        }
        ////        //v17.50追加 by 間々田 2010/12/28 画像左右反転（Pke時）対応
        ////    }
        ////    else
        ////    {
        ////        ConvertShortToByte(ref Image[0], ref ByteImage[0], imageSize, ref LT[0], Information.UBound(LT) + 1, -myMin);
        ////    }

        ////    if (myPicture == null)
        ////        return;

        ////    //ビットマップ更新
        ////    //SetBitmapBits myPicture.Handle, ByteImage()
        ////    SetByteToBitmap(myPicture.Handle, ByteImage[0]);
        ////    //vcの関数に変更 2009-06-11

        ////    //表示
        ////    DispPicture();

        ////    return;
        ////ErrorHandler:
        ////    //v17.00追加 byやまおか 2010/02/08

        ////    //v17.00追加 byやまおか 2010/02/08
        ////    Interaction.MsgBox(Err.Description, MsgBoxStyle.Exclamation);
        ////    //v17.00追加 byやまおか 2010/02/08

        ////}
        //public void MakePicture(ushort[] image)
        //{
        //    // 階調変換用ルックアップテーブルに基づきバイト配列に変換
        //    if (MirrorOn)
        //    {
        //        //ConvertWordToByteMirror(image, byteImage, imageSize.Width, imageSize.Height, lt);
        //    }
        //    else
        //    {
        //        //ConvertWordToByte(image, byteImage, image.Length, lt);
        //    }

        //    // ビットマップ更新
        //    if (picture != null)
        //    {
        //        try
        //        {

        //            lock (picture)
        //            {
        //                ////LocBitsを使用してデータを書き換える
        //                //Rectangle rect = new Rectangle(0, 0, imageSize.Width, imageSize.Height);
        //                ////ビットマップデータをロックする
        //                //BitmapData bmpData = picture.LockBits(rect, ImageLockMode.ReadWrite, picture.PixelFormat);
        //                ////ビットマップデータに変換する
        //                ////DrawBitmap(byteImage, bmpData.Scan0, bmpData.Width, bmpData.Height, bmpData.Stride);
        //                //DrawBitmap2(byteImage, bmpData.Scan0, bmpData.Width, bmpData.Height, bmpData.Stride, myRasterOp);
        //                ////ビットマップデータに対するロックを解除
        //                //picture.UnlockBits(bmpData);
        //            }
        //        }
        //        catch
        //        {
        //        }
        //        finally
        //        {
        //        }

        //    }
        //}

        //public Dictionary<LineConstants, TransLine> myTLines
        //{
        //    get { return lines; }
        //    set
        //    {
        //        if (value == myTLines) 
        //            return;
        //        lines = value;
        //    }
        //}

        /// <summary>
        /// ラインを参照を設定する
        /// </summary>
        public void GetLines(ref Dictionary<LineConstants, TransLine> mlines)
        {
            mlines = lines;
        }

        //2014/11/11hata Point型を変更？？
        /// <summary>
        /// 線Point設定
        /// </summary>
        /// <param name="sliceWidth"></param>
        /// <param name="sliceNum"></param>
        /// <param name="slicePitch"></param>
        public void SetLinePoint(LineConstants linetype, ref PointF P1, ref PointF P2, bool lineEnabled = true)
        //public void SetLinePoint(LineConstants linetype, ref Point P1, ref Point P2, bool lineEnabled = true)
        {
            lines[linetype].P1 = P1;
            lines[linetype].P2 = P2;
            lines[linetype].Visible = lineEnabled;
        }

        //2014/11/11hata Point型を変更
        /// <summary>
        /// 線Point取得
        /// </summary>
        /// <param name="sliceWidth"></param>
        /// <param name="sliceNum"></param>
        /// <param name="slicePitch"></param>
        public void GetLinePoint(LineConstants linetype, ref PointF P1, ref PointF P2, ref bool lineEnabled)
        //public void GetLinePoint(LineConstants linetype, ref Point P1, ref Point P2, ref bool lineEnabled)
        {
            P1= lines[linetype].P1;
            P2= lines[linetype].P2;
            lineEnabled = lines[linetype].Visible;
        }

        //追加2014/10/07hata_v19.51反映
        // ユーザカラーの設定
        /// <summary>
        /// ユーザカラーの設定
        /// </summary>
        /// <param name="linetype"></param>
        /// <param name="color"></param>
        public void SetUserLineColor(LineConstants linetype, Color color)
        {
            lines[linetype].UserCoror = color;
        }

        //追加2014/10/07hata_v19.51反映
        /// <summary>
        /// ユーザカラーを使用するかどうかの設定
        /// </summary>
        /// <param name="linetype"></param>
        /// <param name="used"></param>
        public void SetUserLineColorUsed(LineConstants linetype, bool used)
        {
            lines[linetype].UserColorUsed = used;
        }

        //Rev25.00 文字列の描画追加 by長野 2016/08/08 --->
        /// <summary>
        /// 描画開始Point設定
        /// </summary>
        public void SetStringPoint(StringConstants stringtype, ref PointF P1,bool StringEnabled = true)
        {
            strings[stringtype].P1 = P1;
            strings[stringtype].Visible = StringEnabled;
        }

        //Rev25.00 文字列追加 by長野 2016/08/08 --->
        /// <summary>
        /// 文字列描画開始Point取得
        /// </summary>
        public void GetLinePoint(StringConstants stringtype, ref PointF P1, ref bool StringEnabled)
        {
            P1 = strings[stringtype].P1;
            StringEnabled = strings[stringtype].Visible;
        }

        /// <summary>
        /// 文字列設定
        /// </summary>
        public void SetStringCaption(StringConstants stringtype, string str)
        {
            strings[stringtype].str = str;
        }

        /// <summary>
        /// 文字列取得
        /// </summary>
        public void GetStringCaption(StringConstants stringtype, ref string str)
        {
            str = strings[stringtype].str;
        }

        /// <summary>
        /// 文字列の参照を設定する
        /// </summary>
        public void GetStrings(ref Dictionary<StringConstants, TransString>mStrings)
        {
            mStrings = strings;
        }
        //<---

        //Rev25.00 ラインプロファイル追加 by長野 2016/08/08 --->
        /// <summary>
        /// 描画Point(配列)設定
        /// </summary>
        public void SetLinePointSize(LineConstants lineType, int size)
        {
            switch(lineType)
            {
                case LineConstants.ProfileH:
                    lines[lineType].P_ProfH = new PointF[size];
                    break;
                case LineConstants.ProfileV:
                    lines[lineType].P_ProfV = new PointF[size];
                    break;
            }
        }

        /// <summary>
        /// 描画Point(配列)設定
        /// </summary>
        //public void SetLineProfH(ushort[] prof,int mabiki,int posH,int maxValue,int maxLinePix,int minLinePix,int h_size,int v_size)
        //Rev25.13 修正 by chouno 2017/11/18
        public void SetLineProfH(ushort[] prof, float mabiki, int posH, int maxValue, int maxLinePix, int minLinePix, int h_size, int v_size)
        {
            int cnti = 0;
            int magnify = (int)(1.0f / (float)mabiki);
            if (magnify <= 1)
            {
                for (int cnt = 0; cnt < prof.Length; cnt++)
                {
                    if (cnt % mabiki == 0)
                    {
                        lines[LineConstants.ProfileH].P_ProfH[cnti] = new PointF(cnti, h_size - (prof[cnt] / (maxValue / (maxLinePix - minLinePix)) + minLinePix));
                        cnti++;
                    }
                }
            }
            else
            {
                for (int cnt = 0; cnt < prof.Length; cnt++)
                {
                    for (int cntmag = 0; cntmag < magnify; cntmag++)
                    {
                        lines[LineConstants.ProfileH].P_ProfH[cnti] = new PointF(cnti, h_size - (prof[cnt] / (maxValue / (maxLinePix - minLinePix)) + minLinePix));
                        cnti++;
                    }
                }
            }
        }

        /// <summary>
        /// 描画Point(配列)設定
        /// </summary>
        //public void SetLineProfV(ushort[] prof, int mabiki, int posV, int maxValue, int maxLinePix, int minLinePix, int h_size, int v_size)
        //Rev25.13 修正 by chouno 2017/11/18
        public void SetLineProfV(ushort[] prof, float mabiki, int posV, int maxValue, int maxLinePix, int minLinePix, int h_size, int v_size)
        {
            int cnti = 0;
            int magnify = (int)(1.0f / (float)mabiki);
            if (magnify <= 1)
            {
                for (int cnt = 0; cnt < prof.Length; cnt++)
                {
                    if (cnt % mabiki == 0)
                    {
                        lines[LineConstants.ProfileV].P_ProfV[cnti] = new PointF(prof[cnt] / (maxValue / (maxLinePix - minLinePix)) + minLinePix, cnti);
                        cnti++;
                    }
                }
            }
            else
            {
                for (int cnt = 0; cnt < prof.Length; cnt++)
                {
                    for (int cntmag = 0; cntmag < magnify; cntmag++)
                    {
                        lines[LineConstants.ProfileV].P_ProfV[cnti] = new PointF(prof[cnt] / (maxValue / (maxLinePix - minLinePix)) + minLinePix, cnti);
                        cnti++;
                    }
                }
            }
        }
        //<---

        //*******************************************************************************
        //機　　能： 線の表示・非表示の設定
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        public void SetLineVisible(LineConstants theLine, bool IsVisible)
        {

            //線の表示・非表示の設定
            lines[theLine].Visible = IsVisible;

            //透視画像コントロールをリフレッシュ
            this.Refresh();

        }

        //*******************************************************************************
        //機　　能： 文字列の表示・非表示の設定
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        public void SetStringVisible(StringConstants theString, bool IsVisible)
        {

            //線の表示・非表示の設定
            strings[theString].Visible = IsVisible;

            //透視画像コントロールをリフレッシュ
            this.Refresh();

        }

        //*******************************************************************************
        //機　　能： 線の表示・非表示の設定
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
        //*******************************************************************************
        public bool GetLineVisible(LineConstants theLine)
        {
            //線の表示・非表示の設定
            return lines[theLine].Visible;
        }


        #region オーバーライドされたメソッド
        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="e"></param>
        //protected override void OnPaint(PaintEventArgs e)
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaint(e);
            base.OnPaintBackground(e);

            Bitmap myBitmap = dispPicture;

            if (myBitmap != null)
            {
                ////ここで表示サイズを設定する
                //int offSize = 12;
                ////RectangleF pic_rect = new RectangleF(0, 0, picture.Width, picture.Height);
                //RectangleF pic_rect = new RectangleF(offSize + 1, offSize + 1, myBitmap.Width - offSize * 2, myBitmap.Height - offSize * 2);
 
                //if ((pic_rect.Height > ClientSize.Height) || (pic_rect.Width > ClientSize.Width))
                //{
                //    // 縮小
                //    if (pic_rect.Height > pic_rect.Width)
                //    {
                //        float rate = (float)ClientSize.Height / pic_rect.Height;
                //        rect.Width = rate * pic_rect.Width;
                //    }
                //    else
                //    {
                //        float rate = (float)ClientSize.Width / pic_rect.Width;
                //        rect.Height = rate * pic_rect.Height;
                //    }
                //}
                //else
                //{
                //    rect.Height = pic_rect.Height;
                //    rect.Width = pic_rect.Width;
                //}
                //rect.X = (ClientSize.Width - rect.Width) / 2;
                //rect.Y = (ClientSize.Height - rect.Height) / 2;

                //e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                //if (myRasterOp == 1)
                //{
                //    //左右反転させる
                //    rect.X = ClientRectangle.Right;
                //    rect.Width = -rect.Width;
                //}
 
                //ビットマップ描画
                //e.Graphics.DrawImage(myBitmap, rect, pic_rect, GraphicsUnit.Pixel);
                //e.Graphics.DrawImage(myBitmap, rect);

                //RectangleF rect = new RectangleF(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
                //e.Graphics.DrawImage(myBitmap, this.ClientRectangle);

                //補間モード
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

                e.Graphics.DrawImage(myBitmap, this.ClientRectangle);

            }

            foreach (TransLine line in lines.Values)
            {
                //if (line.Visible)
                //Rev25.00 条件変更 by長野 2016/08/08
                if (line.Visible)
                {
                    if (line.LineType != LineConstants.ProfileV && line.LineType != LineConstants.ProfileH)
                    {
                        line.Draw(e.Graphics);
                    }
                    else //Rev25.00 プロファイル追加 by長野 2016/08/08
                    {
                        line.DrawGraph(e.Graphics);
                    }
                }
            }

            //Rev25.00 文字列描画 by長野　2016/08/08
            foreach (TransString str in strings.Values)
            {
                if (str.Visible)
                {
                    str.DrawStr(e.Graphics);
                }
            }

            ////イベント生成
            //if (UCPaint != null)
            //{
            //    UCPaint(this, e);
            //}

        }
        #endregion


        ////private void CTImageCanvas_MouseDown(object sender, MouseEventArgs e)
        //protected override void OnMouseDown(MouseEventArgs e)
        //{
            //base.OnMouseDown(e);

            //イベント生成
            //if (UCMouseDown != null)
            //{
            //    UCMouseDown(this, e);
            //}
        //}

        ////private void CTImageCanvas_MouseMove(object sender, MouseEventArgs e)
        //protected override void OnMouseMove(MouseEventArgs e)
        //{
        //    base.OnMouseMove(e);
        //    //イベント生成
        //    if (UCMouseMove != null)
        //    {
        //        UCMouseMove(this, e);
        //    }
        //}

        ////private void CTImageCanvas_MouseUp(object sender, MouseEventArgs e)
        //protected override void OnMouseUp(MouseEventArgs e)
        //{
        //    base.OnMouseUp(e);
        //    //イベント生成
        //    if (UCMouseUp != null)
        //    {
        //        UCMouseUp(this, e);
        //    }
        //}

    }

#region 線の描画クラス
    /// <summary>
    /// 線クラス
    /// </summary>
    public class TransLine
    {
        // タイプ
        private LineConstants lineType = LineConstants.Other;

        //追加2014/10/07hata_v19.51反映
        //ユーザ設定用のペン色
        private Color myUserPenColor = Color.Transparent;
        //ユーザ設定用ペン色有無のフラグ
        private bool myUserPenColorFlg = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type"></param>
        public TransLine(LineConstants type)
        {
            lineType = type;
        }

        /// <summary>
        /// 表示
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// タイプ
        /// </summary>
        public LineConstants LineType { get { return lineType; } }

        //2014/11/11hata Point型を変更？？
        /// <summary>
        /// 始点
        /// </summary>
        public PointF P1 { get; set; }
        //public Point P1 { get; set; }

        //2014/11/11hata Point型を変更？？
        /// <summary>
        /// 終点
        /// </summary>
        public PointF P2 { get; set; }
        //public Point P2 { get; set; }

        /// <summary>
        /// 垂直プロファイル用 //Rev25.00 追加 by長野 2016/08/08
        /// </summary>
        public PointF[] P_ProfV { get; set; }

        /// <summary>
        /// 水平プロファイル用 //Rev25.00 追加 by長野 2016/08/08
        /// </summary>
        public PointF[] P_ProfH { get; set; }

        //Rev25.00 ラインプロファイル用Pen 追加 by長野 2016/08/08
        public Pen LPpen = new Pen(Color.Red);
        public PointF P1_draw = new PointF();
        public PointF P2_draw = new PointF();
        public  GraphicsPath LPpath = new GraphicsPath();

        /// <summary>
        /// ユーザカラーの設定
        /// </summary>
        public Color UserCoror
        {
            get
            {
                return myUserPenColor;
            }
            set
            {
                myUserPenColor = value;
            }

        }

        /// <summary>
        /// ユーザカラーを使用するかどうかの設定
        /// </summary>
        public bool UserColorUsed
        {
            get
            {
                return myUserPenColorFlg;
            }
            set
            {
                myUserPenColorFlg = value;
            }

        }
  
        /// <summary>
        /// 線を描画する
        /// </summary>
        public void Draw(Graphics g)
        {
            if (Visible)
            {
                if(float.IsNaN(P1.X) || float.IsNaN(P1.Y) || float.IsNaN(P2.X) || float.IsNaN(P2.Y) ||
                   float.IsInfinity(P1.X) || float.IsInfinity(P1.Y) || float.IsInfinity(P2.X) || float.IsInfinity(P2.Y))
                {
                    //不定な値は描画しない
                    return;
                }

                GraphicsPath path = new GraphicsPath();
                path.AddLine(P1, P2);
                
                g.DrawPath(GetPen(), path);
 
                path.Dispose();
                
            }

            //Draw(g, 0);
        }

        /// <summary>
        /// 線を描画する
        /// </summary>
        public void Draw(Graphics g, int MirrorOn)
        {
            if (Visible)
            {
                if (float.IsNaN(P1.X) || float.IsNaN(P1.Y) || float.IsNaN(P2.X) || float.IsNaN(P2.Y) ||
                   float.IsInfinity(P1.X) || float.IsInfinity(P1.Y) || float.IsInfinity(P2.X) || float.IsInfinity(P2.Y))
                {
                    //不定な値は描画しない
                    return;
                }

                GraphicsPath path = new GraphicsPath();

                if (MirrorOn == 1)
                {
                    path.AddLine(P1.X, P1.Y, P2.X, P2.Y);
                }
                else
                {
                    path.AddLine(P1, P2);
                }

                g.DrawPath(GetPen(), path);
     
                path.Dispose();
                
            }
        }

        /// <summary>
        /// 線を描画する //Rev25.00 追加 by長野 2016/08/08
        /// </summary>
        public void DrawGraph(Graphics g)
        {
            if (Visible)
            {
                //Pen pen = GetPen();

                if (LineType == LineConstants.ProfileH)
                {
                    for (int cnt = 0; cnt < P_ProfH.Length - 1; cnt++)
                    {
                        P1_draw = P_ProfH[cnt];
                        P2_draw = P_ProfH[cnt + 1];
                        //LPpath.AddLine(P1_draw, P2_draw);
                        //g.DrawPath(LPpen, LPpath);
                        g.DrawLine(LPpen, P1_draw,P2_draw);
                    }
                }
                else
                {
                    for (int cnt = 0; cnt < P_ProfV.Length - 1; cnt++)
                    {
                        P1_draw = P_ProfV[cnt];
                        P2_draw = P_ProfV[cnt + 1];
                        //LPpath.AddLine(P1_draw, P2_draw);
                        //g.DrawPath(LPpen, LPpath);
                        g.DrawLine(LPpen, P1_draw, P2_draw);
                    }
                }
            }
            //Draw(g, 0);
        }

        /// <summary>
        /// ペン選択
        /// </summary>
        /// <returns></returns>
        private Pen GetPen()
        {
            //Pen pen = Pens.Red;
            Pen pen = new Pen(Color.Red);

            switch (lineType)
            {
                case LineConstants.ScanLine:
                    //pen = Pens.Green;
                    pen = Pens.LimeGreen;
                    break;
                case LineConstants.UpperLine:
                case LineConstants.LowerLine:
                    pen = Pens.Yellow;
                    break;
                case LineConstants.CenterLine:
                case LineConstants.CenterLineH:
                    pen = Pens.Cyan;
                    break;
                case LineConstants.ProfilePosV: //Rev25.00 透視プロファイル 垂直 追加  by長野 2016/08/08
                    pen.Color = Color.Red;
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    break;
                case LineConstants.ProfilePosH: //Rev25.00 透視プロファイル 水平 追加 by長野 2016/08/08
                    pen.Color = Color.Red;
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    break;
                case LineConstants.ProfileV:
                case LineConstants.ProfileH:
                    pen = Pens.Red;
                    break;
                default:
                    break;
            }
            if (myUserPenColorFlg)
                if (myUserPenColor != Color.Transparent)
                    pen.Color = myUserPenColor;
 
            return pen;
        }
    }
    #endregion 線の描画クラス

    #region 文字列の描画クラス
    /// <summary>
    /// 線クラス
    /// </summary>
    public class TransString
    {
        // タイプ
        private StringConstants StringType = StringConstants.Other;

        //追加2014/10/07hata_v19.51反映
        //ユーザ設定用のペン色
        private Color myUserPenColor = Color.Transparent;
        //ユーザ設定用ペン色有無のフラグ
        private bool myUserPenColorFlg = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type"></param>
        public TransString(StringConstants strType)
        {
            StringType = strType;
            font = new Font("MS UI Gothic", 10.5F);
            color = Color.Red;
        }

        /// <summary>
        /// 表示
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 始点
        /// </summary>
        public PointF P1 { get; set; }

        /// <summary>
        /// 文字列
        /// </summary>
        public string str { get; set; }

        /// <summary>
        /// 色
        /// </summary>
        public Color color { get; set; }

        /// <summary>
        /// フォント
        /// </summary>
        public Font font { get; set; }

        /// <summary>
        /// ユーザカラーの設定
        /// </summary>
        public Color UserCoror
        {
            get
            {
                return myUserPenColor;
            }
            set
            {
                myUserPenColor = value;
            }

        }

        /// <summary>
        /// ユーザカラーを使用するかどうかの設定
        /// </summary>
        public bool UserColorUsed
        {
            get
            {
                return myUserPenColorFlg;
            }
            set
            {
                myUserPenColorFlg = value;
            }

        }
        /// <summary>
        /// 文字列を描画する
        /// </summary>
        public void DrawStr(Graphics g)
        {
            if (Visible)
            {
                if (float.IsNaN(P1.X) || float.IsNaN(P1.Y) || float.IsInfinity(P1.X) || float.IsInfinity(P1.Y))
                {
                    //不定な値は描画しない
                    return;
                }

                g.DrawString(str, font, new SolidBrush(color), P1.X, P1.Y);
            }
        }
    }
    #endregion 文字列の描画クラス
}

