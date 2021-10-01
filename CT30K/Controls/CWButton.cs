using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace CT30K
{
	public partial class CWButton : UserControl
	{
		private Image _OnImage = null;
		private Image _OffImage = null;
		private bool _Value = false;
		private CWSpeeds _BlinkInterval = CWSpeeds.cwSpeedOff;

        private bool thBlink = false;
        private Bitmap backbmp = null;
        private Color _OnColor;
        //private Color _OffColor;
        //private bool _BlinkValueCancel = false;   //削除2015/02/27hata

        //private bool _click = false;
        private bool ValueFalse_event = false;

        //２重呼び出し防止
        private bool myButton_MouseDown_BUSYNOW;
        private bool myButton_MouseUp_BUSYNOW;
        private bool myButton_MouseClick_BUSYNOW;
        //private bool myButton_Leave_BUSYNOW;      //削除2015/02/27hata
        private bool myButton_MouseCaptured_BUSYNOW;
        private bool isIgnorePreformClick = false; //Rev26.00 add by chouno 2017/10/31
        public bool IsIgnorePerformClick
        {
            get
            {
                return isIgnorePreformClick;
            }
            set
            {
                isIgnorePreformClick = value;
            }
        }

        //イベント
		public event EventHandler ValueChanged = null;
        public new event MouseEventHandler MouseUp;
        public new event MouseEventHandler MouseDown;
        public new event MouseEventHandler MouseClick;
        public new event EventHandler MouseCaptureChanged;
      
        /// <summary>
        /// 排他用オブジェクト
        /// </summary>
        private static object gcwLock = new object();

		public CWButton()
		{
			InitializeComponent();
            backbmp = new Bitmap(32, 32);
            //if (_OnImage == null) _OnImage = backbmp;
        
        }

		public Image OnImage
		{
			get { return _OnImage; }
            set
            {
                lock (gcwLock)
                {
                    _OnImage = value;
                    if (!backgroundWorker1.IsBusy)
                        myButton.BackgroundImage = value;
                    //追加2015/01/19hata
                    if(_BlinkInterval == CWSpeeds.cwSpeedOff)
                        myButton.BackgroundImage = value;
                }
            }
		}

		public Image OffImage
		{
			get { return _OffImage; }
            set { _OffImage = value;}
        }

		public bool Value
		{
			get { return _Value; }

			set
			{
				if (_Value != value)
				{
					_Value = value;

                    //変更2014/10/02hata
                    OnValueChanged();
                    //if (_click) OnValueChanged();


                    //追加2015/02/26hata
                    //OnValueChanged間で変わっていることがあるため、ここでvalueを設定する。
                    if (!_Value)
                    {
                        value = false;
                    }

                    //削除2015/02/27hata
                    ////if (_BlinkValueCancel)
                    //if (_BlinkValueCancel & _Value)
                    //{
                    //    _BlinkValueCancel = false;
                    //    value = false;
                    //    _Value = value;
                    //}

                    //変更2015/02/26hata
                    //if ((!backgroundWorker1.IsBusy) && value)
                    if ((!backgroundWorker1.IsBusy) && _Value)
                    {
                        if (_BlinkInterval != CWSpeeds.cwSpeedOff)
                        {
                            //スレッドスタート
                            thBlink = true;
                            backgroundWorker1.RunWorkerAsync();
                            Application.DoEvents();//23.40 by長野 2016/04/05 / s//Rev23.21 DoEventをかけないとIsBusy変更がかからない。by長野 2016/02/22
                        }
                        else
                        {
                            thBlink = false;

                            //lock (gLock)
                            //{
                            //    myButton.BackgroundImage = _OnImage;
                            //}
                        }
                    }
                    else
                    {
                        thBlink = false;
                        //23.40 by長野 2016/04/05 //Rev23.10 backgrounWorkerを停止させる by長野 2015/10/16
                        if (backgroundWorker1.IsBusy)
                        {
                            backgroundWorker1.CancelAsync();
                            Application.DoEvents();//23.40 by長野 2016/04/05 / s//Rev23.21 DoEventをかけないとIsBusy変更がかからない。by長野 2016/02/22
                        }
                    }
                    
                    //追加2014/10/02hata
                    if (_Value == false) 
                    {
                        //23.40 by長野 2016/04/05 //Rev23.10 backgrounWorkerを停止させる by長野 2015/10/16
                        if (backgroundWorker1.IsBusy)
                        {
                            backgroundWorker1.CancelAsync();
                            Application.DoEvents();//23.40 by長野 2016/04/05 / s//Rev23.21 DoEventをかけないとIsBusy変更がかからない。by長野 2016/02/22
                        }
                        //クリック状態を戻す
                        ValueFalse_event = true;
                        if (isIgnorePreformClick == false) //Rev26.00 add by chouno 2017/10/31
                        {
                            myButton.PerformClick();
                        }
                        ValueFalse_event = false;   //追加2015/03/18hata
                    }

                }
			}
		}



        //ValueChanged イベント
        protected virtual void OnValueChanged()
		{
			if (ValueChanged != null)
			{
				ValueChanged(this, EventArgs.Empty);
            }
		}

        //MouseUp イベント
        protected new void OnMouseUp(MouseEventArgs e)
        {
            if (MouseUp != null)
            {
                MouseUp(this, e);
            }
        }
        //MouseDown イベント
        protected new void OnMouseDown(MouseEventArgs e)
        {
            if (MouseDown != null)
            {
                MouseDown(this, e);
            }
        }

        //MouseClick イベント
        protected new void OnMouseClick(MouseEventArgs e)
        {
            if (MouseClick != null)
            {
                MouseClick(this, e);
            }
        }

        //追加2015/02/10
        //MouseCaptureChanged イベント
        protected new void OnMouseCaptureChanged(EventArgs e)
        {
            if (MouseCaptureChanged != null)
            {
                MouseCaptureChanged(this, e);
            }
        }



        //BackgroundImageLayout プロパティ
        public new ImageLayout BackgroundImageLayout
		{
			get { return myButton.BackgroundImageLayout; }
			set { myButton.BackgroundImageLayout = value; }
		}        
        
        //BlinkInterval プロパティ
        public CWSpeeds BlinkInterval
		{
			get { return _BlinkInterval; }
			set 
            {
                _BlinkInterval = value;
                if (value == CWSpeeds.cwSpeedOff) thBlink = false;                   

            }
		}

        //OnColor プロパティ
        public Color OnColor
        {
            get { return _OnColor; }
            set { _OnColor = value; }
        }

        //public Color OffColor
        //{
        //    get { return _OffColor; }
        //    set
        //    {
        //        _OffColor = value;
        //        myButton.BackColor = _Value ? _OnColor : _OffColor;
        //    }
        //}

        //BackColor プロパティ
        public new Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                Application.DoEvents();//23.40 by長野 2016/04/05 / s//Rev23.21 DoEventをかけないとIsBusy変更がかからない。by長野 2016/02/22

                if (!backgroundWorker1.IsBusy)
                {
                    base.BackColor = value;
                    myButton.BackColor = value;
                }
            }
        }

        //Caption プロパティ
        public string Caption
        {
            get { return myButton.Text; }
            set { myButton.Text = value; }
        }


        //Caption プロパティ
        public FlatStyle FlatStyle
        {
            get
            {
                return myButton.FlatStyle; 
            }
            set
            {
                myButton.FlatStyle = value; 
            
            }
        }

        //FlatAppearanceBorderColor プロパティ
        public Color FlatAppearanceBorderColor
        {
            get
            {
                return myButton.FlatAppearance.BorderColor;
            }
            set
            {
                myButton.FlatAppearance.BorderColor = value;
            }
        }

        //FlatAppearanceBorderSize プロパティ
        public int FlatAppearanceBorderSize
        {
            get
            {
                return myButton.FlatAppearance.BorderSize;
            }
            set
            {
                myButton.FlatAppearance.BorderSize = value;
            }
        }

        //FlatAppearanceMouseDownBackColor プロパティ
        public Color FlatAppearanceMouseDownBackColor
        {
            get
            {
                return myButton.FlatAppearance.MouseDownBackColor;
            }
            set
            {
                myButton.FlatAppearance.MouseDownBackColor = value;
            }
        }

        //FlatAppearanceMouseDownBackColor プロパティ
        public Color FlatAppearanceMouseOverBackColor
        {
            get
            {
                return myButton.FlatAppearance.MouseOverBackColor;
            }
            set
            {
                myButton.FlatAppearance.MouseOverBackColor = value;
            }
        }



        private void myButton_MouseDown(object sender, MouseEventArgs e)
        {
            //状態(True:実行中,False:停止中)
            if (myButton_MouseDown_BUSYNOW)
            {
                return;
            }

            myButton_MouseDown_BUSYNOW = true;

            //変更2014/10/02hata
            //OnMouseDown(e);
            //_click = true;
            OnMouseDown(e);
            if (!Value) Value = true;
            //_click = false;

            //元の状態に戻す
            myButton_MouseDown_BUSYNOW = false;
        }


        private void myButton_MouseUp(object sender, MouseEventArgs e)
        {
            //状態(True:実行中,False:停止中)
            if (myButton_MouseUp_BUSYNOW)
            {
                return;
            }

            myButton_MouseUp_BUSYNOW = true;

            //変更2014/10/02hata
            //OnMouseUp(e);
            //_click = true;
            if (Value) Value = false;
            OnMouseUp(e);
            //_click = false;

            //元の状態に戻す
            myButton_MouseUp_BUSYNOW = false;

        }

        private void myButton_MouseClick(object sender, MouseEventArgs e)
        {

            //追加2014/10/02hata
            //Valueがfalseになったときは無視する
            if (ValueFalse_event)
            {
                ValueFalse_event = false;
                return;
            }

            //状態(True:実行中,False:停止中)
            if (myButton_MouseClick_BUSYNOW)
            {
                return;
            }

            myButton_MouseClick_BUSYNOW = true;

            //_click = true;
            OnMouseClick(e);
            //_click = false;

            //元の状態に戻す
            myButton_MouseClick_BUSYNOW = false;
            //myButton.Enabled = true;
        }

        //削除2015/02/27hata
        //private void myButton_MouseLeave(object sender, EventArgs e)
        //{
        //    //状態(True:実行中,False:停止中)
        //    if (myButton_Leave_BUSYNOW)
        //    {
        //        return;
        //    }

        //    myButton_Leave_BUSYNOW = true;

        //    if (Value) _BlinkValueCancel = true;

        //    //元の状態に戻す
        //    myButton_Leave_BUSYNOW = false;
        //}

        //追加2015/02/10
        private void myButton_MouseCaptureChanged(object sender, EventArgs e)
        {
            //状態(True:実行中,False:停止中)
            if (myButton_MouseCaptured_BUSYNOW)
            {
                return;
            }

            myButton_MouseCaptured_BUSYNOW = true;

            OnMouseCaptureChanged(e);
 
            //元の状態に戻す
            myButton_MouseCaptured_BUSYNOW = false;
            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime tim1;
            Color tmpColor = this.BackColor;
            bool bChange = false;
            bool OffImg = false;

            if (myButton.BackgroundImage == null) OffImg = true;
            if (_OnImage == null) OffImg = true;
            
            while (thBlink)
            {
                if (OffImg)
                {
                    if (!bChange)
                    {
                        myButton.BackColor = tmpColor;
                        bChange = true;
                    }
                    else
                    {
                        myButton.BackColor = OnColor;
                        bChange = false;
                    }
                }
                else
                {

                    lock (gcwLock)
                    {
                        if (bChange == false)
                        {
                            bChange = true;
                            myButton.BackgroundImage = _OnImage;
                        }
                        else
                        {
                            bChange = false;
                            myButton.BackgroundImage = backbmp;
                        }
                    }
                }


                //一定時間待つ
                tim1 = DateTime.Now;
                while (tim1.AddMilliseconds((int)_BlinkInterval) > DateTime.Now)
                {
                    if (!thBlink) break;
                    //System.Threading.Thread.Sleep(1);
                    Application.DoEvents();
                }
             }
            lock (gcwLock)
            {
                myButton.BackgroundImage = _OnImage;
                myButton.BackColor = tmpColor;
            }
        }




	}

	/// <summary>
	/// 
	/// </summary>
	public enum CWSpeeds
	{
		/// <summary>
		/// 600 ms
		/// </summary>
		cwSpeedFast = 600,

		/// <summary>
		/// 150 ms
		/// </summary>
		cwSpeedFastest = 150,

		/// <summary>
		/// 900 ms
		/// </summary>
		cwSpeedMedium = 900,

		/// <summary>
		/// OFF(0 ms)
		/// </summary>
		cwSpeedOff = 0,

		/// <summary>
		/// 1200 ms
		/// </summary>
		cwSpeedSlow = 1200,

		/// <summary>
		/// 1800 ms
		/// </summary>
		cwSpeedSlowest = 1800,

		/// <summary>
		/// 300 ms
		/// </summary>
		cwSpeedVeryFast = 300,

		/// <summary>
		/// 1500 ms
		/// </summary>
		cwSpeedVerySlow = 1500,
	}
}

