//---------------------------------------------------------------------------------------------------- 
// プログラム名： CT.Controls Ver1.0   
// ファイル名　： NumUpDown.cs    
// 処理概要　　： 
// 注意事項　　：   
//  
// ＯＳ　　　　： Windows XP Professional (SP2) 
// コンパイラ　： Microsoft Visual C# 2005  
//  
// VERSION  DATE        BY              CHANGE/COMMENT     
// v2.00    2008/04/16  (SS1)間々田     新規作成
//
// (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2008
//----------------------------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Itc.Common.Controls
{
    /// <summary>
    /// NumericUpDownの機能追加版
    /// </summary>
    public class NumUpDown : NumericUpDown
    {
        //内部変数
        private Slider mySlider;        //連動するSliderコントロール
        private bool IgnoreValueChange = false;
        private int myPreviousValue;    //前回値

        //ValueChangedを定義しなおす
        new public event EventHandler<ValueEventArgs> ValueChanged;

        /// <summary>
        /// 連動するSliderコントロール
        /// </summary>
        public Slider BindingSlider
        {
            get { return mySlider;  }
            set 
            {
                if (mySlider != null) mySlider.Leave -= new EventHandler(mySlider_Leave);
                mySlider = value;
                if (mySlider != null) mySlider.Leave += new EventHandler(mySlider_Leave);
            }
        }

        /// <summary>
        /// 連動するSliderコントロールのLeaveイベントをこのコントロールのLeaveイベントに結びつける
        /// </summary>
        private void mySlider_Leave(object sender, EventArgs e)
        {
            this.OnLeave(e);
        }

        /// <summary>
        /// 最大値
        /// </summary>
        new public int Maximum
        {
            get { return (int)base.Maximum; }
            set
            {
                if (value < Minimum) return;
                IgnoreValueChange = true;
                base.Maximum = (decimal)value;
                if (mySlider != null) mySlider.Maximum = value;
                IgnoreValueChange = false;
            }
        }

        /// <summary>
        /// 最小値
        /// </summary>
        new public int Minimum
        {
            get { return (int)base.Minimum; }
            set
            {
                if (value > Maximum) return;
                IgnoreValueChange = true;
                base.Minimum = (decimal)value;
                if (mySlider != null) mySlider.Minimum = value;
                IgnoreValueChange = false;
            }
        }

        /// <summary>
        /// このコントロールに割り当てる値
        /// </summary>
        new public int Value
        {
            get{ return (int)base.Value;}
            set
            {
                base.Value = (decimal)Math.Max(Minimum, Math.Min(value, Maximum));
                base.Refresh();
            }
        }

        /// <summary>
        /// ValueChanged イベントを発生させるための処理
        /// </summary>
        protected override void OnValueChanged(EventArgs e)
        {
            //スライダーコントロールにも値を反映させる
            if (mySlider != null)
            {
                if (!mySlider.Focused)
                {
                    mySlider.Value = Value;
                }
            }

            //イベントを発生させる：最大値・最小値変更時に伴うValueの変更は無視
            if (ValueChanged != null && (!IgnoreValueChange))
            {
                ValueChanged(this, new ValueEventArgs(myPreviousValue));
            }

            //今回の値を次回のPreviousValueとして保存
            myPreviousValue = (int)base.Value;
        }

        /// <summary>
        /// Enabled プロパティ変更時処理
        /// </summary>
        protected override void OnEnabledChanged(EventArgs e)
        {
            //連動するSliderコントロールにも反映させる
            if (mySlider != null) mySlider.Enabled = this.Enabled;

            //基本クラスの OnEnabledChanged メソッドを呼び出す
            base.OnEnabledChanged(e);
        }

        /// <summary>
        /// 妥当性チェック          //1.09追加 by 間々田 2007/12/14
        /// </summary>
        protected override void OnValidating(CancelEventArgs e)
        {
            UpDownBase upDown = this as UpDownBase;

            try
            {
                int val = int.Parse(upDown.Text);
            }
            catch
            {
                //元に戻す
                upDown.Text = this.Value.ToString();
                e.Cancel = true;
            }

            //基本クラスの OnValidating メソッドを呼び出す
            base.OnValidating(e);
        }

    }

    //前回値をサポートする独自のイベント変数
    public class ValueEventArgs : EventArgs
    {
        /// <summary>
        /// 前回値
        /// </summary>
        private int myPreviousValue;

        public ValueEventArgs(int value)
        {
            myPreviousValue = value;
        }
        public int PreviousValue
        {
            get { return myPreviousValue; }
            set { myPreviousValue = value; }
        }
    }
}
