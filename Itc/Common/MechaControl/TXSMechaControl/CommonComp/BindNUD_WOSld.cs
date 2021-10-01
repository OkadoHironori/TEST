using Itc.Common.Basic;
using Itc.Common.Event;
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
    using NumUpdateEventHandler = Action<object, NumUpdateEventArgs>;

    public partial class BindNUD_WOSld : NumericUpDown
    {
        /// <summary>
        /// 実行可否
        /// </summary>
        public Func<float, string, ResBase> IsDoExecut { private get; set; }
        /// <summary>
        /// アップデートリクエスト
        /// </summary>
        public event EventHandler RequestUpdate;
        /// <summary>
        /// 数値変更イベント
        /// </summary>
        public event NumUpdateEventHandler ChangeDataUpdate;
        /// <summary>
        /// INotifyPropertyChagedでデータを変化させたか？
        /// </summary>
        public bool DirectChanged { get; set; } = false;
        /// <summary>
        /// 前回値の初期値入力イベント抑制フラグ
        /// </summary>
        private bool _PreviousValue = false;
        /// <summary>
        /// 前回値
        /// </summary>
        private float PreviousValue=999;    //前回値 初期値が0だとありそうなので、初期値にふさわしくない値をいれておくこと
        /// <summary>
        /// スライダーなしの確認画面表示付き確認画面（ホーム位置）
        /// </summary>
        public BindNUD_WOSld()
        {
            //フォント
            this.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            //タブによるフォーカス移動を禁止
            this.TabStop = false;
        }
        /// <summary>
        /// 初期値入力
        /// </summary>
        /// <param name="v"></param>
        public void SetPreviousValue(float v)
        {
            if (_PreviousValue) { return; }

            PreviousValue = v;

            _PreviousValue = true;
        }
        /// <summary>
        /// 初期値の設定
        /// </summary>
        /// <param name="dec"></param>
        public void SetIniDeci(int dec, float max, float min)
        {
            this.DecimalPlaces = dec;

            switch (dec)
            {
                case (3):
                    this.Increment = (decimal)0.001;
                    break;
                case (2):
                    this.Increment = (decimal)0.01;
                    break;
                case (1):
                    this.Increment = (decimal)0.1;
                    break;
                case (0):
                    this.Increment = 1;
                    break;

                default:
                    throw new Exception("Undeveloped DecimalPlaces");
            }

            this.Maximum = (decimal)max;

            this.Minimum = (decimal)min;
        }
        /// <summary>
        /// 値変更時のイベントハンドラ
        /// </summary>
        /// <param name="e"></param>
        protected override void OnValueChanged(EventArgs e)
        {

            UpDownBase upDown = this as UpDownBase;

            if (!float.TryParse(upDown.Text, out float val))
            {//変な値が入ったのでキャンセル
                upDown.Text = this.Value.ToString();

                base.OnValidating(new CancelEventArgs(true));

                return;
            }
            else
            {
                if (!DirectChanged)
                {
                    if (val != PreviousValue)
                    {
                        var res = IsDoExecut.Invoke((float)this.Value, "テキスト");

                        if (res.Res)
                        {
                            ChangeDataUpdate(this, new NumUpdateEventArgs(res.ResValue));

                            PreviousValue = res.ResValue;
                        }
                        else
                        {
                            RequestUpdate(this, new EventArgs());

                            PreviousValue = res.ResValue;
                        }
                    }
                }
            }

            base.OnValueChanged(e);

        }
    }
}
