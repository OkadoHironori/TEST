using Itc.Common.Basic;
using Itc.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TXSMechaControl.CommonComp
{
    public partial class BindNUD : NumericUpDown
    {
        /// <summary>
        /// 実行可否
        /// </summary>
        public Func<float, string, string, ResBase> IsDoExecut { private get; set; }
        /// <summary>
        /// アップデートリクエスト
        /// </summary>
        public event EventHandler RequestUpdate;
        /// <summary>
        /// スライダーで変化させた？
        /// </summary>
        public bool IsSliderDecition { get; set; } = false;
        /// <summary>
        /// 値入力で変化させたか？
        /// </summary>
        public bool IsDirectDecition { get; set; } = false;
        /// <summary>
        /// INotifyPropertyChagedでデータを変化させたか？
        /// </summary>
        public bool DirectChanged { get; set; } = false;
        /// <summary>
        /// 変更後の値
        /// </summary>
        public float ChangedValue { get; private set; }
        /// <summary>
        /// 表示用
        /// </summary>
        public string ValueEx { get; private set; } = "0.01";
        /// <summary>
        /// 連動するFCD_スライダーコントロール
        /// </summary>
        private BindSlider mySlider;
        /// <summary>
        /// 連動するSliderコントロール
        /// </summary>
        public BindSlider BindSlider
        {
            get
            {
                return mySlider;
            }
            set
            {
                if (mySlider != null)
                {
                    mySlider.Leave -= new EventHandler(Slider_Leave);
                }
                mySlider = value;
                if (mySlider != null)
                {
                    mySlider.Leave += new EventHandler(Slider_Leave);
                }
            }
        }
        /// <summary>
        /// スライダー外す（ラムダ式にしたい）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_Leave(object sender, EventArgs e)
        {
            this.OnLeave(e);
        }
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
        public BindNUD()
        {

        }

        /// <summary>
        /// 初期値の設定
        /// </summary>
        /// <param name="dec"></param>
        public void SetIniDeci(int dec, float max, float min, string increment)
        {
            try
            {
                DirectChanged = true;

                this.DecimalPlaces = dec;

                switch (dec)
                {
                    case (3):
                        this.Increment = (decimal)0.001;
                        ValueEx = "0.000";
                        break;
                    case (2):
                        this.Increment = (decimal)0.01;
                        ValueEx = "0.00";
                        break;
                    case (1):
                        this.Increment = (decimal)0.1;
                        ValueEx = "0.0";
                        break;
                    case (0):
                        this.Increment = 1;
                        ValueEx = "0";
                        break;

                    default:
                        throw new Exception("Undeveloped DecimalPlaces");
                }

                this.Maximum = (decimal)max;

                this.Minimum = (decimal)min;

                this.Increment = (decimal)decimal.Parse(increment);

            }
            finally
            {
                DirectChanged = false;
            }
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
                if (!mySlider.Focused && !DirectChanged)
                {
                    var res = IsDoExecut.Invoke(Value, "テキスト", Increment.ToString());

                    if (res.Res)
                    {
                        IsDirectDecition = true;
                        mySlider.Value = res.ResValue;
                        ChangedValue = res.ResValue;
                        mySlider.Data.Clear();
                        mySlider.Data.Add(Value);

                        base.OnValueChanged(e);
                    }
                    else
                    {
                        RequestUpdate(this, new EventArgs());
                        return;
                    }
                }
                else if (!mySlider.Focused && DirectChanged)//INotifyPropertyChangedで変更
                {
                    mySlider.Value = Value;
                    mySlider.Data.Clear();
                    mySlider.Data.Add(Value);
                }
            }

            IsDirectDecition = false;
        }

        /// <summary>
        /// 設定値が増分の倍数でない場合は、設定値を超えない増分の倍数値にする
        /// </summary>
        /// <param name="e"></param>
        protected override void OnValidating(CancelEventArgs e)
        {
            UpDownBase upDown = this as UpDownBase;

            if (!float.TryParse(upDown.Text, out float data))
            {
                upDown.Text = this.Value.ToString();//元に戻す
                e.Cancel = true;
            }

            base.OnValidating(e);
        }
        /// <summary>
        /// 設定値が増分の倍数でない場合は、設定値を超えない増分の倍数値にする
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            BindSlider.ChangeBindSlider += (s, d) =>
            {
                try
                {
                    if (Value != d.NumValue && mySlider.Data.Count() != 1)
                    {
                        var res = IsDoExecut.Invoke(Value, "スライダ", Increment.ToString());

                        if (res.Res)
                        {
                            mySlider.Data.Clear();
                            mySlider.Data.Add(res.ResValue);
                            ChangedValue = res.ResValue;
                            IsSliderDecition = true;
                            base.OnValueChanged(new EventArgs());
                            IsSliderDecition = false;
                        }
                        else
                        {
                            mySlider.Data.Clear();
                            BindSlider.IsSliderChanged = false;
                            RequestUpdate(this, new EventArgs());
                        }
                    }
                }
                finally
                {
                    BindSlider.IsSliderChanged = false;
                }
            };
            base.OnPaint(e);
        }
    }
}
