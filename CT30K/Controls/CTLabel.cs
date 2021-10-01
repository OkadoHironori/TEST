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
    public partial class CTLabel : UserControl
    {
        private Color _OnColor;
        private Color _OffColor;

        //private bool _Value = false;
        private BlinkSpeeds _BlinkInterval = BlinkSpeeds.cwSpeedOff;

        private bool thBlink = false;
        

        //２重呼び出し防止
        //private bool myButton_MouseDown_BUSYNOW;
        //private bool myButton_MouseUp_BUSYNOW;

        //イベント
        public event EventHandler ValueChanged = null;
        //public new event MouseEventHandler MouseUp;

        /// <summary>
        /// 排他用オブジェクト
        /// </summary>
        private static object gCTLblLock = new object();        
        
        public CTLabel()
        {
            InitializeComponent();
        }

        //*******************************************************************************
        //機　　能： Font プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
        public new Font Font
        //public override Font Font
        {
            get { return label1.Font; }
            set
            {
                label1.Anchor = AnchorStyles.None;
                base.Font = value;
                label1.Font = value;

                label1.AutoSize = true;
                label1.AutoSize = false;

                //Size ns = new Size(label1.Width, label1.Height);
                //int iWtmp = label1.Width;
                //int iHtmp = label1.Height;
                //label1.Width = iWtmp;
                //label1.Height = iHtmp;
                //label1.Size = ns;

                //label1.Size = this.Size;
                //abel1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;

            }
        }

        //*******************************************************************************
        //機　　能： Caption プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成Alignment
        //*******************************************************************************
        public string Caption
        {
            get { return label1.Text; }
            set 
            { 
                label1.Text = value;
                label1.Anchor = AnchorStyles.None;
                label1.AutoSize = true;
                //Application.DoEvents();
                label1.AutoSize = false;

            }
        }

        //*******************************************************************************
        //機　　能： CaptionAlignment プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
        public ContentAlignment CaptionAlignment
        {
            get { return label1.TextAlign; }
            set { label1.TextAlign = value; }
        }

        //*******************************************************************************
        //機　　能： CaptionWidth プロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
        //*******************************************************************************
        public int CaptionWidth
        {
            get { return label1.Width; }
            set { label1.Width = value;}
        }

        public BlinkSpeeds BlinkInterval
        {
            get { return _BlinkInterval; }
            set 
            { 
                _BlinkInterval = value;
                
                if ((!backgroundWorker1.IsBusy))
                {
                    if (_BlinkInterval != BlinkSpeeds.cwSpeedOff)
                    {
                        //スレッドスタート
                        thBlink = true;
                        backgroundWorker1.RunWorkerAsync();
                    }
                    else
                    {
                        thBlink = false;
                    }
                }
                else
                {
                    thBlink = false;
                }
            }
        }


        //public override Color ForeColor
        public new Color ForeColor
        {
            get
            {
                return label1.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                label1.ForeColor = value;
            }
        }

        //点滅時のOnカラー
        public Color OnColor
        {
            get { return _OnColor; }
            set
            {
                lock (gCTLblLock)
                {
                    _OnColor = value;
                    this.BackColor = value;
                    label1.BackColor = value;
                }
            }
        }

        //点滅時のOffカラー
        public Color OffColor
        {
            get { return _OffColor; }
            set 
            {
                lock (gCTLblLock)
                {
                    _OffColor = value;
                }
             }
        }

        //public bool Value
        //{
        //    get { return _Value; }

        //    set
        //    {
        //        if (_Value != value)
        //        {
        //            _Value = value;
        //            //OnValueChanged();
                    
        //            if ((!backgroundWorker1.IsBusy) && value)
        //            {
        //                if (_BlinkInterval != BlinkSpeeds.cwSpeedOff)
        //                {
        //                    //スレッドスタート
        //                    thBlink = true;
        //                    backgroundWorker1.RunWorkerAsync();
        //                }
        //                else
        //                {
        //                    thBlink = false;
        //                    //lock (gLock)
        //                    //{
        //                    //    myButton.BackgroundImage = _OnImage;
        //                    //}
        //                }
        //            }
        //            else
        //            {
        //                thBlink = false;
        //            }
        //        }
        //    }
        //}


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime tim1;
            bool bChange = false;
            //Color tmpColor = this.BackColor;
            
            while (thBlink)
            {
                lock (gCTLblLock)
                {
                    if (bChange == false)
                    {
                        bChange = true;
                        //this.BackColor = _OnColor;
                        //this.BackColor = tmpColor;
                        //label1.BackColor = this.BackColor;
                        //this.BackColor = tmpColor;
                        //label1.BackColor = tmpColor;
                        this.BackColor = _OnColor;
                        label1.BackColor = _OnColor;

                    }
                    else
                    {
                        bChange = false;
                        //this.BackColor = _OffColor;
                        //label1.BackColor = this.BackColor;
                        this.BackColor = _OffColor;
                        label1.BackColor = _OffColor;
                    }
                }
                //一定時間待つ
                tim1 = DateTime.Now;
                while (tim1.AddMilliseconds((int)_BlinkInterval) > DateTime.Now)
                {
                    if (!thBlink) break;
                    if (_BlinkInterval == BlinkSpeeds.cwSpeedOff)
                    {
                        thBlink = false;
                        break;
                    
                    }
                    System.Threading.Thread.Sleep(1);
                }
            }
            lock (gCTLblLock)
            {
                //this.BackColor = _OnColor;
                //this.BackColor = tmpColor;
                //label1.BackColor = this.BackColor;
                this.BackColor = _OnColor;
                label1.BackColor = _OnColor;
            }
        }

        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, EventArgs.Empty);
            }
        }

        private void CTLabel_Resize(object sender, EventArgs e)
        {
            label1.Anchor = AnchorStyles.None;
            label1.SetBounds(0, 0, this.Width, this.Height);
            label1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        }
   


    }

    /// <summary>
    /// 
    /// </summary>
    public enum BlinkSpeeds
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
