using Itc.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Itc.Common.Controls.NumTextBoxEx
{
    public partial class NumTextBoxEx : TextBox
    {

        /// <summary>
        /// 小数点以下の桁数
        /// </summary>
        public int DecimalPlaces { get; private set; } = 1;
        /// <summary>
        /// 設定最小値
        /// </summary>
        public string Increment { get; private set; } = "0.1";
        /// <summary>
        /// 設定最小値
        /// </summary>
        public string StringEx { get; private set; } = "0.0";
        /// <summary>
        /// 最小値
        /// </summary>
        public float Minimum { get; private set; } = short.MinValue;
        /// <summary>
        /// 最小値
        /// </summary>
        public float Maxmum { get; private set; } = short.MaxValue;
        /// <summary>
        /// 数値
        /// </summary>
        public float Value
        {
            get
            {
                return float.Parse(this.Text);
            }
            set
            {
                float val = value.CorrectRange(Minimum, Maxmum);
                this.Text = Math.Round((val / float.Parse(Increment)) * float.Parse(Increment), DecimalPlaces).ToString(StringEx);

                ValueChanged?.Invoke(this, new EventArgs());
            }
        }
        /// <summary>
        /// 数値変更時のイベント
        /// </summary>
        public event EventHandler ValueChanged;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NumTextBoxEx():base()
        {
            //フォント
            this.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            //タブによるフォーカス移動を禁止
            this.TabStop = false;
        }
        /// <summary>
        /// 値入力
        /// </summary>
        /// <param name="val"></param>
        public void SetValue(float val, float max, float min, string increment)
        {
            Value = val;
            Minimum = min;
            Maxmum = max;
            Increment = increment;

            double input = Convert.ToDouble(Increment);

            double decimal_part = input % 1;

            string output = Convert.ToString(decimal_part);

            DecimalPlaces = (output.Length - 2);

            switch(DecimalPlaces)
            {
                case (1):
                    StringEx = "0.0";
                    break;
                case (2):
                    StringEx = "0.00";
                    break;
                case (3):
                    StringEx = "0.000";
                    break;
                default:
                    StringEx = "0.0";
                    break;
            }
        }
        /// <summary>
        /// 妥当性チェック　入力時
        /// </summary>
        protected override void OnValidating(CancelEventArgs e)
        {
            if(!float.TryParse(this.Text, out float val))
            {
                //元に戻す
                this.Undo();
                e.Cancel = true;
            }

            base.OnValidating(e);
        }
        /// <summary>
        /// 妥当性チェック 入力後
        /// </summary>
        /// <param name="e"></param>
        protected override void OnValidated(EventArgs e)
        {
            this.Value = float.Parse(this.Text);
            base.OnValidated(e);
        }

    }
}
