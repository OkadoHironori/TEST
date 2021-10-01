using Itc.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Itc.Common.Controls.NumCtrl
{
    /// <summary>
    /// NumericUpDownの機能追加版
    /// </summary>
    public class NumUpDownEx : NumericUpDown
    {
        public float PrevValue { get; private set; } = 0;

        /// <summary>
        /// 表示用
        /// </summary>
        public string ValueEx { get; private set; } = "0.01";

        public NumUpDownEx()
        {
            //フォント
            this.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            //タブによるフォーカス移動を禁止
            this.TabStop = false;
        }
        /// <summary>
        /// 初期値の設定
        /// </summary>
        /// <param name="dec"></param>
        public void SetIniDeci(int dec, float max, float min, decimal incre, float prev)
        {
            PrevValue = prev;

            this.DecimalPlaces = dec;
            this.Increment = incre;

            switch (dec)
            {
                case (3):
                    ValueEx = "0.000";
                    break;
                case (2):
                    ValueEx = "0.00";
                    break;
                case (1):
                    ValueEx = "0.0";
                    break;
                case (0):
                    ValueEx = "0";
                    break;

                default:
                    throw new Exception("Undeveloped DecimalPlaces");
            }

            //switch(dec)
            //{
            //    case (3):
            //        this.Increment = (decimal)0.001;
            //        break;
            //    case (2):
            //        this.Increment = (decimal)0.01;
            //        break;
            //    case (1):
            //        this.Increment = (decimal)0.1;
            //        break;
            //    case (0):
            //        this.Increment = 1;
            //        break;

            //    default:
            //        throw new Exception("Undeveloped DecimalPlaces");
            //}

            this.Maximum = (decimal)max;

            this.Minimum = (decimal)min;
        }

        /// <summary>
        /// 数値以外の値を入れたらキャンセル
        /// </summary>
        protected override void OnValidating(CancelEventArgs e)
        {
            UpDownBase upDown = this as UpDownBase;

            if(!float.TryParse(upDown.Text, out float val))
            {
                upDown.Text = this.PrevValue.ToString();
                e.Cancel = true;
                base.OnValidating(e);
                return;
            }

            if(!val.InRange((float)this.Minimum, (float)this.Maximum))
            {
                upDown.Text = this.PrevValue.ToString();
                e.Cancel = true;
                base.OnValidating(e);
                return;
            }

            var bunbo = (float)Increment * (float)Math.Pow(10, DecimalPlaces);
            var bunshi = val * (float)Math.Pow(10, DecimalPlaces);

            if (bunshi % bunbo == 0)
            {
                upDown.Text = this.Value.ToString();
                base.OnValidating(e);
                return;
            }
            else
            {
                upDown.Text = this.PrevValue.ToString();
                e.Cancel = true;
                base.OnValidating(e);
                return;
            }
            //{

            //}
                

        }
    }
}
