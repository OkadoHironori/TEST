using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CT30K
{
    public partial class CTSliderTrack : UserControl
    {
        //EventHandlerを定義
        public EventHandler ValueChanged;
        public new EventHandler Scroll;
        public new EventHandler MouseCaptureChanged;
        public new MouseEventHandler MouseUp;
        public new MouseEventHandler MouseDown;        //追加2015/01/08hata_型変更

        //public new ScrollEventHandler Scroll;

        //内部変数
        private bool _Reverse = false;
        //private bool _Ticks = true;
        
        //変更2014/12/22hata_型変更
        //private int setValue;
        //private int trueValue;
        private decimal setValue;
        private decimal trueValue;
        private bool bvalueChanged = false; //追加2015/01/09

        //private bool _ControlLock = false;


        //コンストラクタ        
        public CTSliderTrack()
        {
            InitializeComponent();

            // イベント定義
            InitializeEventHandler();
            trueValue = trackBar1.Value;
            setValue = trackBar1.Value;
        }

        #region イベント定義
        /// <summary>
        /// イベント定義
        /// </summary>
        private void InitializeEventHandler()
        {
            // スクロールバー値変更時処理
            trackBar1.ValueChanged += (sender, e) =>
            {
                //変更2015/01/09
                if(!bvalueChanged) setValue = trackBar1.Value;
             
                if (_Reverse)
                {
                    trueValue = trackBar1.Maximum - setValue;
                }
                else
                {
                    trueValue = setValue;
                }

                if (ValueChanged != null)
                    ValueChanged(this, e);
            };

            // スクロールバー値変更時処理
            trackBar1.Scroll += (sender, e) =>
            {
                //変更2015/01/09
                if (!bvalueChanged) setValue = trackBar1.Value;
                
                if (_Reverse)
                {
                    trueValue = trackBar1.Maximum - setValue + trackBar1.Minimum;
                }
                else
                {
                    trueValue = setValue;
                }

                if (Scroll != null)
                    Scroll(this, e);

            };

            // MouseUp時の処理
            trackBar1.MouseUp += (sender, e) =>
            {
                if (MouseUp != null)
                    MouseUp(this, e);

            };

            //MouseCaptureChanged時の処理
            trackBar1.MouseCaptureChanged  += (sender, e) =>
            {
                if (MouseCaptureChanged != null)
                    MouseCaptureChanged(this, e);

            };

            //Resize時の処理
            trackBar1.Resize += (sender, e) =>
            {
                trackBar1.Anchor = AnchorStyles.None;
                this.Size = trackBar1.Size;
                trackBar1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);

            };

            //追加2015/01/08hata
            // MouseDown時の処理
            trackBar1.MouseDown += (sender, e) =>
            {
                if (MouseDown != null)
                    MouseDown(this, e);

            };


        }
        #endregion

        #region プロパティ
        /// <summary>
        /// 値
        /// </summary>
        //public int Value
        public decimal Value
        {
            get { return Convert.ToDecimal(trueValue); }
            set
            {
                if (trackBar1.Maximum < value) value = Convert.ToDecimal(trackBar1.Maximum);
                if (trackBar1.Minimum > value) value = Convert.ToDecimal(trackBar1.Minimum);
                
                trueValue = value;
                if (_Reverse)
                {
                    setValue = trackBar1.Maximum - trueValue + trackBar1.Minimum;
                }
                else
                {
                    setValue = trueValue;
                }
                bvalueChanged = true; //追加2015/01/09
                
                //2014/12/22hata_型変更
                //trackBar1.Value = setValue;
                trackBar1.Value = (int)Math.Round(setValue, MidpointRounding.AwayFromZero);
                
                bvalueChanged = false; //追加2015/01/09
            }
        }

        /// <summary>
        /// 最大値
        /// </summary>
        public int Maximum
        {
            get { return trackBar1.Maximum; }
            set
            {
                trackBar1.Maximum = value;
            }
        }
 
        /// <summary>
        /// 最小値
        /// </summary>
        public int Minimum
        {
            get { return trackBar1.Minimum; }
            set
            {
                trackBar1.Minimum = value;
            }
        }
    

        /// <summary>
        /// スライダの変更方向を逆にする
        /// </summary>
        public bool Reverse
        {
            get { return _Reverse; }
            set
            {
                _Reverse = value;
                Value = trueValue;
            }
        }

        /// <summary>
        /// ラージchange
        /// </summary>
        public int LargeChange
        {
            get { return trackBar1.LargeChange; }
            set { trackBar1.LargeChange = value; }
        }
        
        /// <summary>
        /// 最小change
        /// </summary>
        public int SmallChange
        {
            get { return trackBar1.SmallChange; }
            set { trackBar1.SmallChange = value; }
        }        
       
        /// <summary>
        /// コントロールの向き
        /// </summary>
        public Orientation Orientation
        {
            get { return trackBar1.Orientation; }
            set { trackBar1.Orientation = value; }
        }

        /// <summary>
        /// 目盛りの表示の方向
        /// </summary>
        public TickStyle TickStyle
        {
            get { return trackBar1.TickStyle; }
            set { trackBar1.TickStyle = value; }
        }

        /// <summary>
        /// 目盛りマーク間の数
        /// </summary>
        public int TickFrequency
        {
            get { return trackBar1.TickFrequency; }
            set { trackBar1.TickFrequency = value; }     
        }

        /// <summary>
        /// サイズを自動敵に調整する
        /// </summary>
        public new bool AutoSize
        {
            get { return trackBar1.AutoSize; }
            set { trackBar1.AutoSize = value; }
        }
        #endregion

        //private void trackBar1_Resize(object sender, EventArgs e)
        //{
        //    trackBar1.Anchor = AnchorStyles.None;
        //    this.Size = trackBar1.Size;
        //    trackBar1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);

        //}

        //private void trackBar1_MouseCaptureChanged(object sender, EventArgs e)
        //{
        //    if (MouseCaptureChanged != null)
        //        MouseCaptureChanged(this, e);

        //}
    }
}
