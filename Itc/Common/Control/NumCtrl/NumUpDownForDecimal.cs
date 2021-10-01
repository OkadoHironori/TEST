using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Itc.Common.Controls
{
    /// <summary>
    /// NumericUpDownの機能追加版(Valueで小数を扱う版)
    /// </summary>
    public class NumUpDownForDecimal : NumericUpDown
    {
        /// <summary>
        /// 連動するSliderコントロール
        /// </summary>
        private Slider mySlider;

        /// <summary>
        /// 値
        /// </summary>
        public new float Value
        {
            get
            {
                return (float)base.Value;
            }
            set
            {
                var dv = (decimal)value;
                if (Increment > 0)
                {
                    base.Value = Math.Round((int)(dv / Increment) * Increment, DecimalPlaces);
                }
                else
                {
                    base.Value = 0;
                }
            }
        }

        /// <summary>
        /// 連動するSliderコントロール
        /// </summary>
        public Slider BindingSlider
        {
            get
            {
                return mySlider;
            }
            set
            {
                if (mySlider != null)
                {
                    mySlider.Leave -= new EventHandler(mySlider_Leave);
                }
                mySlider = value;
                if (mySlider != null)
                {
                    mySlider.Leave += new EventHandler(mySlider_Leave);
                }
            }
        }

        /// <summary>
        /// 最大値
        /// </summary>
        public new float Maximum
        {
            get
            {
                return (float)base.Maximum;
            }
            set
            {
                base.Maximum = (decimal)value;
                if(mySlider != null)
                {
                    mySlider.Maximum = value;
                }
            }
        }

        /// <summary>
        /// 最小値
        /// </summary>
        public new float Minimum
        {
            get
            {
                return (float)base.Minimum;
            }
            set
            {
                base.Minimum = (decimal)value;
                if(mySlider != null)
                {
                    mySlider.Minimum = value;
                }
            }
        }

        private void mySlider_Leave(object sender, EventArgs e)
        {
            this.OnLeave(e);
        }

        /// <summary>
        /// 値変更時のイベントハンドラ
        /// </summary>
        /// <param name="e"></param>
        protected override void OnValueChanged(EventArgs e)
        {
            //スライダコントロールに値を反映する
            if (mySlider != null)
            {
                if(!mySlider.Focused)
                {
                    mySlider.Value = Value;
                }
            }

            base.OnValueChanged(e);
        }

        /// <summary>
        /// 設定値が増分の倍数でない場合は、設定値を超えない増分の倍数値にする
        /// </summary>
        /// <param name="e"></param>
        protected override void OnValidating(CancelEventArgs e)
        {
            UpDownBase upDown = this as UpDownBase;
            try
            {
                int val = int.Parse(upDown.Text);
                float inc = (float)this.Increment;
                float incP = inc * (float)Math.Pow(10, this.DecimalPlaces);
                int incI = (int)incP;

                float f = Value * (float)Math.Pow(10, this.DecimalPlaces);
                int i = (int)f;
                if (incI != 0 && i % incI != 0)
                {
                    Value = (i / incI) * inc;
                }
            }
            catch
            {
                //元に戻す
                upDown.Text = this.Value.ToString();
                e.Cancel = true;
            }

            base.OnValidating(e);
        }
    }
}
