using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CT30K
{
    public partial class CTSliderVScroll : UserControl
    {
        #region EventHandlerを定義

        //EventHandlerを定義
        [Browsable(true)]
        [Category("アクション")]
        [Description("値が変更されたときに発生します。")]
        public event EventHandler ValueChanged;

        //[Browsable(true)]
        //[Category("アクション")]
        //[Description("スクロールしたときに発生します。")]
        //public event ScrollEventHandler Scroll;

        //スクロールをデリゲートを使って独自に宣言
        [Browsable(true)]
        [Category("アクション")]
        [Description("スクロールボタンを離したときに発生します。")]
        public event EventHandler SliderScrollComited;

        [Browsable(true)]
        [Category("アクション")]
        [Description("マウスの捕捉したときに発生します。")]
        public new event EventHandler MouseCaptureChanged;

        public new MouseEventHandler MouseUp;           
        public new MouseEventHandler MouseDown;         


        //public delegate void SliderScrollHandlerComited(object sender, EventArgs e);
        #endregion

        //内部変数
        int Max = 100;
        int Min = 0;
        int DefaultPointerHeight = 12;
        int _PointerHeight = 12;
        decimal trueValue = 0;
        int largeChange = 5;
        int decimalPlaces = 0;
        int lineOffset = 2;

        bool _Reverse = false;
        bool _ArrowVisible = false;
        decimal _ArrowValue = 0;
        int offsetPos = 0;

        bool _scroll = false;
        bool captured = false;
        bool clicked = false;


        Point MouseCap = new Point(0, 0);
        Rectangle pPointerRect= new Rectangle(0, 0, 12, 12); //Pointerの領域
        Point pPointerPos = new Point(0,0);      //Pointerの左上座標
        Point pPointerOffset = new Point(0,0);   //Pointerとマウス位置のオフセット量

        Rectangle pArrowRect = new Rectangle(0, 0, 12, 12);   //Pointerの領域
        Point pArrowPos = new Point(0, 0);        //Pointerの左上座標
        
        BorderStyle borderStyle = BorderStyle.None;
        Bitmap canvas = null;

        //PointerChangeModeの制御タイプ
        public enum PointerChangeType
        {
            PointChangeOn,      //クリックした位置に移動
            PointChangeOff,
        }
        PointerChangeType pointChange = PointerChangeType.PointChangeOn;

        
        //ArrowDirectionの制御タイプ
        public enum ArrowDirectionType
        {
            Left,       //左向きに表示
            Right       //右向きに表示
        }
        ArrowDirectionType _ArrowDirection = ArrowDirectionType.Right;


        public CTSliderVScroll()
        {
            InitializeComponent();

            this.BorderStyle = BorderStyle.None;
            borderStyle = BorderStyle.Fixed3D;
        
            canvas = new Bitmap(16, 16);

            picField.BackColor = Color.Transparent;
            pPointerRect = new Rectangle(0, 0, 12, 12);
            pArrowRect = new Rectangle(0, 0, 12, 12);
            picField.Size = this.ClientRectangle.Size;

            pPointerRect = new Rectangle(0, 0, picField.ClientRectangle.Width, PointerHeight);
            pPointerPos = new Point(0, 0);
            offsetPos = pPointerRect.Height / 2;

            pArrowRect = new Rectangle(0, 0, picField.ClientRectangle.Width, PointerHeight);
            pArrowPos = new Point(0, 0);

            //BuutonDisp();
            picField.Refresh();
        }

        #region プロパティ
        /// <summary>
        /// 値
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("スライダーの値を設定また取得します。")]
        public decimal Value
        {
            get 
            {
                return trueValue;
            }
            set
            {
                decimal val = SetDecimalFormat(value);
                if (Max < val)
                {
                    val = Max;
                }
                else if (Min > val)
                {
                    val = Min;
                }                
                if (trueValue == val) return;
                trueValue = val;
                
                if (!_scroll) SetPointeValue(trueValue);
                
                if (ValueChanged != null)
                    ValueChanged(this, EventArgs.Empty);

                picField.Refresh();
            }
        }

        /// <summary>
        /// 最大値
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("最大値を設定また取得します。")]
        public int Maximum
        {
            get { return Max; }
            set
            {
                Max = value;
            }
        }

        /// <summary>
        /// 最小値
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("最小値を設定また取得します。")]
        public int Minimum
        {
            get { return Min; }
            set
            {
                Min = value;
            }
        }

        /// <summary>
        /// スライダの変更方向を逆にする
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("スライダの変更方向を設定また取得します。")]
        public bool Reverse
        {
            get { return _Reverse; }
            set
            {
                _Reverse = value;
                SetPointeValue(trueValue);
                SetArrowValue(_ArrowValue);
                picField.Refresh();
            }
        }

        /// <summary>
        /// ポイントchange
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("クリックした時のポイントchange方法を設定また取得します。")]
        public PointerChangeType PointChangeMode
        {
            get { return pointChange; }
            set { pointChange = value; }
        }
        
        /// <summary>
        /// ラージchange
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("クリックした時の増減値を設定また取得します。PointChangeModeがFalseの場合に有効になります。")]
        public int LargeChange
        {
            get { return largeChange; }
            set { largeChange = value; }
        }
       
        ///// <summary>
        ///// 最小change
        ///// </summary>
        //public int SmallChange
        //{
        //    get { return smallChange; }
        //    set { smallChange = value; }
        //}

        
        /// <summary>
        /// ポインタサイズ
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("ポインタサイズを設定また取得します。")]
        public int PointerHeight
        {
            get { return _PointerHeight; }
            set
            {
                if (value < DefaultPointerHeight) value = DefaultPointerHeight;
                _PointerHeight = value;
                SetPointeSize(_PointerHeight);
                picField.Refresh();
            }
        }

        /// <summary>
        /// マウスキャプチャー有無
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("マウスキャプチャー有無を取得します。")]
        public new bool Capture
        {
            //get { return picPointer.Capture; }
            get { return captured; }
        }


        /// <summary>
        /// 小数点の表示
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("小数点の表示します。")]
        public int DecimalPlaces
        {
            get { return decimalPlaces; }
            set { decimalPlaces = value;}
        }


        /// <summary>
        /// 矢印ポインターラベルの値
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("矢印ポインターの値を設定また取得します。")]
        public decimal ArrowValue
        {
            get { return _ArrowValue; }
            set
            {
                decimal val = SetDecimalFormat(value);
                if (Max < val)
                {
                    val = Max;
                }
                else if (Min > val)
                {
                    val = Min;
                }
                if (_ArrowValue == val) return;
                _ArrowValue = val;

                SetArrowValue(_ArrowValue);
                //BuutonDisp();
                picField.Refresh();
            }
        }

        /// <summary>
        /// 矢印ポインターラベルの表示／非表示
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("矢印ポインターの表示／非表示を設定また取得します。")]
        public bool ArrowVisible
        {
            get { return _ArrowVisible; }
            set 
            { 
                _ArrowVisible = value;
                //BuutonDisp();
                picField.Refresh();
            }
        }

        /// <summary>
        /// 矢印ポインターの表示方向
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("矢印ポインターの表示方向を設定また取得します。")]
        public ArrowDirectionType ArrowDirection
        {
            get { return _ArrowDirection; }
            set 
            { 
                _ArrowDirection = value;
                //BuutonDisp();
                picField.Refresh();
            }
        }

         /// <summary>
        /// BorderStyleの設定
        /// </summary>
        [Browsable(true)]
        [Category("動作")]
        [Description("BorderStyleを設定します。")]
        public new BorderStyle BorderStyle
        {
            get { return borderStyle; }
            set 
            {   
               borderStyle = value;
               //BuutonDisp(); 
               picField.Refresh();
            }
        }

        #endregion


        //ポインター位置からValueを取得する
        private decimal GetPointeValue(int pos)
        {
            decimal val = 0;
            decimal pos1 = 0;

            if ((pos >= 0) & (pos <= picField.ClientRectangle.Height - pPointerRect.Height))
            {
                //if (pPointerPos.Y != pos) pPointerPos = new Point(0, pos);
            }
            else if (pos < 0)
            {
                pos = 0;
            }
            else if (pos > picField.ClientRectangle.Height - pPointerRect.Height)
            {
                pos = picField.ClientRectangle.Height - pPointerRect.Height;
            }
            if (pPointerPos.Y != pos) pPointerPos = new Point(0, pos);


            if (!_Reverse)
            {
                //反転（上がMin）
                pos1 = (picField.ClientRectangle.Height - pPointerRect.Height);
                val = (pos1 - pos) / pos1 * (Max - Min);
            }
            else
            {
                pos1 = (picField.ClientRectangle.Height - pPointerRect.Height);
                val = pos / pos1 * (Max - Min);
            }

             decimal val1 = SetDecimalFormat(val);

             return val1;
        }

        //Valueからポインター位置を設定する
        private void SetPointeValue(decimal val)
        {
            decimal pos = 0;
            decimal pos1 = 0;
            
            if (!_Reverse)
            {
                //反転（上がMin）
                pos1 = val / (Max - Min) * (picField.ClientRectangle.Height - pPointerRect.Height);
                pos = (picField.ClientRectangle.Height - pPointerRect.Height) - pos1;
            }
            else
            {
                pos = val / (Max - Min) * (picField.ClientRectangle.Height - pPointerRect.Height);
            }
            pPointerPos = new Point(0, (int)pos);
        }

 
        //ポインターサイズの設定
        private void SetPointeSize(int size)
        {
            pPointerRect.Size = new Size(pPointerRect.Width, size);
            offsetPos = pPointerRect.Height / 2;
            
            pArrowRect.Size = new Size(pPointerRect.Width, size);
            SetPointeValue(trueValue);
            SetArrowValue(_ArrowValue);
            
        }

        //decimalPlacesにあわせた型にフォーマットする
        private decimal SetDecimalFormat(decimal val)
        {
            decimal d = val;
            double dp = Math.Pow(10, -decimalPlaces);
            d = (decimal)(Convert.ToInt32((double)val / dp) * dp);            
            return d;
        }

        //Valueからポインター位置を設定する
        private void SetArrowValue(decimal val)
        {
            decimal pos = 0;
            decimal pos1 = 0;

            if (!_Reverse)
            {
                //反転（TopがMin）
                pos1 = val / (Max - Min) * (picField.ClientRectangle.Height - pPointerRect.Height);
                pos = (picField.ClientRectangle.Height - pPointerRect.Height) - pos1;
            }
            else
            {
                pos = val / (Max - Min) * (picField.ClientRectangle.Height - pPointerRect.Height);
            }
            pArrowPos = new Point(0, (int)pos);
        }

        //ボタンの絵を表示する
        private void BuutonDisp(Graphics g = null)
        {
            //描画先とするImageオブジェクトを作成する
            if ((canvas.Width != picField.ClientRectangle.Width) | (canvas.Height != picField.ClientRectangle.Height))
                canvas = new Bitmap(picField.ClientRectangle.Width, picField.ClientRectangle.Height);

            //ImageオブジェクトのGraphicsオブジェクトを作成する
            if (g == null)
                g = Graphics.FromImage(canvas);

            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            //BorderStyleの設定
            Border3DStyle ctrlBorder = Border3DStyle.Flat;
            ButtonState buttonBorder = ButtonState.Normal;
            switch(borderStyle)
            {
                case BorderStyle.None:
                    ctrlBorder = Border3DStyle.Adjust;
                    break;
                case BorderStyle.FixedSingle:
                    //ctrlBorder = Border3DStyle.Bump;
                    ctrlBorder = Border3DStyle.Flat;
                    break;
                case BorderStyle.Fixed3D:
                    ctrlBorder = Border3DStyle.Sunken;                   
                    break;
            }

            //コントロールの3Dスタイルの輪郭の描画
            Rectangle rect = new Rectangle(picField.ClientRectangle.Left,
                                           picField.ClientRectangle.Top + pPointerRect.Height / 2 - lineOffset,
                                           picField.ClientRectangle.Width,
                                           picField.ClientRectangle.Height - pPointerRect.Height + lineOffset * 2);
            //if (borderStyle == BorderStyle.None)
            //    g.FillRectangle(Brushes.WhiteSmoke, rect);
            //else
            //    ControlPaint.DrawBorder3D(g, rect, ctrlBorder, Border3DSide.All);
            ControlPaint.DrawBorder3D(g, rect, ctrlBorder, Border3DSide.All);

            //矢印の表示
            if (_ArrowVisible)
                ArrowPointerDisp(g);

            //ボタンコントロールの描画
            //int offset = 0;
            ControlPaint.DrawButton(g, pPointerPos.X, pPointerPos.Y, pPointerRect.Width, pPointerRect.Height, buttonBorder);

            //ボタンコントロール中央の描画
            ControlPaint.DrawButton(g, pPointerPos.X, pPointerPos.Y + pPointerRect.Height / 2 - 2, pPointerRect.Width, 3, buttonBorder);


            //picField.BackgroundImage = canvas;

            //////リソースを解放する
            //g.Dispose();
    
        }

        //矢印ポインターの絵を描画
        private void ArrowPointerDisp(Graphics g)
        {
            //pnlArrow.Size = picPointer.Size;

            ////描画先とするImageオブジェクトを作成する
            //Bitmap Arrowcanvas = new Bitmap(pArrowRect.Width, pArrowRect.Height);
            ////ImageオブジェクトのGraphicsオブジェクトを作成する
            //g = Graphics.FromImage(Arrowcanvas);
            
            //三角のポインターを描画
            Point[] points = new Point[3];
            int midH = Convert.ToInt32(pArrowRect.Height / 2F) - 1;
            if (_ArrowDirection == ArrowDirectionType.Right)
            {
                points[0] = new Point(pArrowPos.X, pArrowPos.Y);
                points[1] = new Point(pArrowRect.Width - 1,pArrowPos.Y + midH);
                points[2] = new Point(pArrowPos.X, pArrowPos.Y + pArrowRect.Height - 1);
            }
            else
            {
                points[0] = new Point(pArrowPos.X + pArrowRect.Width - 1, pArrowPos.Y);
                points[1] = new Point(pArrowPos.X, pArrowPos.Y + midH);
                points[2] = new Point(pArrowPos.X + pArrowRect.Width - 1, pArrowPos.Y + pArrowRect.Height - 1);
            }

            g.FillPolygon(Brushes.Yellow, points);
            g.DrawPolygon(Pens.Goldenrod, points);

            //リソースを解放する
            //g.Dispose();

            //pnlDirectionに表示するBorderStyle
            //pnlArrow.BackgroundImage = Arrowcanvas;

        }

         //ポインターのMouseDown動作
         private void picField_MouseDown(object sender, MouseEventArgs e)
        {
            decimal pos =0;
            int setpos = 0;

            //ポインター領域をクリック
            if ((pPointerPos.X <= e.X) & (pPointerPos.X + pPointerRect.Width >= e.X) &
                (pPointerPos.Y <= e.Y) & (pPointerPos.Y + pPointerRect.Height >= e.Y))
            {
                //captured = true;
                MouseCap = new Point(e.X, e.Y);
                pPointerOffset = new Point((MouseCap.X - pPointerPos.X), (MouseCap.Y - pPointerPos.Y));

                this.Focus();
                captured = true;
                clicked = false;
                //MouseCaptureChanged処理
                if (MouseCaptureChanged != null)
                {
                    MouseCaptureChanged(this, EventArgs.Empty);
                }
                
                //MouseDown処理
                if (MouseDown != null)
                {
                    MouseDown(this, e);
                }
 
            }
            
            //ポインター領域でないところをクリック
            else
            {
                if ((picField.ClientRectangle.Top + pPointerRect.Height / 2 <= e.Y) & 
                    (picField.ClientRectangle.Height - pPointerRect.Height / 2 >= e.Y))
                {
                    this.Focus();
                    captured = true;
                    clicked = true;

                    if (pointChange == PointerChangeType.PointChangeOn)
                    {
                        //PointChangeOnの場合
                        //クリック位置を設定値にする
                        setpos = e.Y- offsetPos;
                        pos = GetPointeValue(setpos);
                    }
                    else
                    {
                        //PointChangeOffの場合
                        //largeChangeで行う
                        if (pPointerPos.Y < e.Y)
                            pos = trueValue + largeChange; //Down
                        else
                            pos = trueValue - largeChange; //Up
                        
                    }
                    Value = pos;

                    //MouseCaptureChanged処理
                    if (MouseCaptureChanged != null)
                    {
                        MouseCaptureChanged(this, EventArgs.Empty);
                    }
                    //MouseDown処理
                    if (MouseDown != null)
                    {
                        MouseDown(this, e);
                    }
                     
                }
            }
         }

         //ポインターのMouseMove動作
         private void picField_MouseMove(object sender, MouseEventArgs e)
        {
            if (captured & !clicked)
            {
                int pos = e.Y - pPointerOffset.Y;
                //if ((pos >= 0) & (pos <= picField.ClientRectangle.Height - pPointerRect.Height))
                //{
                    pPointerPos = new Point(0, pos);

                    //値に換算する
                    _scroll = true;
                    Value = GetPointeValue(pPointerPos.Y);
                    _scroll = false;

                //}
            }
         }

        //ポインターのMouseUp動作
        private void picField_MouseUp(object sender, MouseEventArgs e)
        {
            clicked = false;
            if (captured)
            {
                this.Focus();

                //MouseUp処理
                if (MouseUp != null)
                {
                    MouseUp(this, e);
                }
            }
        }

         //ポインターのMouseCaptureChanged動作
         private void picField_MouseCaptureChanged(object sender, EventArgs e)
         {
             if (captured)
             {
                 this.Focus();
                 captured = false;
                 //MouseCaptureChanged処理
                 if (MouseCaptureChanged != null)
                 {
                     MouseCaptureChanged(this, EventArgs.Empty);
                 }
             }
         }
        
        //ポインターのResize動作
        private void picField_Resize(object sender, EventArgs e)
        {
            pPointerRect.Width = picField.ClientSize.Width;
            //描画先とするImageオブジェクトを作成する
            canvas = new Bitmap(picField.ClientRectangle.Width, picField.ClientRectangle.Height);
            //BuutonDisp();
            picField.Refresh();
        }


        private void picField_Paint(object sender, PaintEventArgs e)
        {
            //補間モード
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            BuutonDisp(e.Graphics);
        }


    }
}
