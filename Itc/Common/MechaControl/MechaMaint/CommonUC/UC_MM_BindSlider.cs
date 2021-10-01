using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Itc.Common.Basic;
using Itc.Common.Controls.TXUCCtrl;
using Itc.Common.Event;
using Itc.Common.Extensions;
using Itc.Common.FormObject;

namespace MechaMaintCnt.CommonUC
{
    using ChkChangeEventHandler = Action<object, ChkChangeEventArgs>;
    using NumUpdateEventHandler = Action<object, NumUpdateEventArgs>;

    public partial class UC_MM_BindSlider : UserControl
    {
        /// <summary>アップデート要求のイベント抑制フラグ</summary>
        private bool _RequestUpdateFlg = false;
        /// <summary>_ValueChangedイベント抑制フラグ</summary>
        private bool _ChangedIgnore = false;
        /// <summary>
        /// Forward移動のためのマウスクリックON/OFF
        /// </summary>
        public event ChkChangeEventHandler ForwordMoveStsChanged;
        /// <summary>
        /// Forward移動のためのマウスクリックON/OFF
        /// </summary>
        public event ChkChangeEventHandler BackwordMoveStsChanged;
        /// <summary>
        /// インデックス移動
        /// </summary>
        public event NumUpdateEventHandler MoveIdexChanged;
        /// <summary>
        /// アップデート要求
        /// </summary>
        public event EventHandler RequestUpdate;
        /// <summary>
        /// 実行可否
        /// </summary>
        public Func<NumBase, string, ResBase> IsDoExecut { private get; set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UC_MM_BindSlider()
        {
            InitializeComponent();

            NumUDDecimalSlider.RequestUpdate += (s, e) => 
            {
                this._RequestUpdateFlg = true;
                RequestUpdate?.Invoke(s, e);
                this._RequestUpdateFlg = false;
            };

            NumUDDecimalSlider.IsDoExecut = (f, mes, incre) =>
            {
                NumBase numb = new NumBase(SliderBind.Maximum, SliderBind.Minimum, f, NumUDDecimalSlider.DecimalPlaces, NumUDDecimalSlider.Increment);
                return IsDoExecut.Invoke(numb, mes);
            };
        }
        /// <summary>
        /// 初期値の入力
        /// </summary>
        /// <param name="max"></param>
        /// <param name="min"></param>
        public void SetInitValue(float sldmax, float sldmin, int decip, float nummax, float nummin,string increment, string unit = "mm")
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<float, float, int, float, float,string, string>(SetInitValue), sldmax, sldmin, decip, nummax, nummin, increment, unit);
                return;
            }

            this.NumUDDecimalSlider.SetIniDeci(decip, nummax, nummin, increment);

            switch (decip)
            {
                case (3):
                    this.Maxlbl.Text = sldmax.ToString("0.000");
                    this.Minlbl.Text = sldmin.ToString("0.000");
                    break;
                case (2):
                    this.Maxlbl.Text = sldmax.ToString("0.00");
                    this.Minlbl.Text = sldmin.ToString("0.00");
                    break;
                case (1):
                    this.Maxlbl.Text = sldmax.ToString("0.0");
                    this.Minlbl.Text = sldmin.ToString("0.0");
                    break;
                case (0):
                    this.Maxlbl.Text = sldmax.ToString("0");
                    this.Minlbl.Text = sldmin.ToString("0");
                    break;

                default:
                    throw new Exception("Undeveloped DecimalPlaces");
            }

            this.SliderBind.Maximum = sldmax;
            this.SliderBind.Minimum = sldmin;

            this.Unit.Text = unit;

        }
        /// <summary>
        /// 数値入力
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(float value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<float>(SetValue), value);
                return;
            }
            if (!this.SliderBind.IsSliderChanged)
            {
                _ChangedIgnore = true;
                this.NumUDDecimalSlider.DirectChanged = true;
                this.NumUDDecimalSlider.Value = value;
                this.NumUDDecimalSlider.DirectChanged = false;
                _ChangedIgnore = false;
            }

            if(_RequestUpdateFlg)
            {
                _ChangedIgnore = true;
                this.NumUDDecimalSlider.DirectChanged = true;
                this.NumUDDecimalSlider.Value = value;
                this.SliderBind.Value = value;
                this.NumUDDecimalSlider.DirectChanged = false;
                _ChangedIgnore = false;
            }
        }
        /// <summary>
        /// 前進限検知
        /// </summary>
        /// <param name="value"></param>
        public void SetValueFLimit(bool value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<bool>(SetValueFLimit), value);
                return;
            }
            this.MoveForword.BackColor = value ? Color.FromArgb(0xff, 0xff, 0x7f) : SystemColors.Control;
        }
        /// <summary>
        /// 後進限検知
        /// </summary>
        /// <param name="value"></param>
        public void SetValueBLimit(bool value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<bool>(SetValueBLimit), value);
                return;
            }
            this.MoveBackword.BackColor = value ? Color.FromArgb(0xff, 0xff, 0x7f): SystemColors.Control;
        }
        /// <summary>
        /// Forward移動のためにクリックON/OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveForword_MoveStateCtrl(object arg1, ChkChangeEventArgs arg2)
        {
            ForwordMoveStsChanged?.Invoke(arg1, arg2);
        }
        /// <summary>
        /// Forward移動のためにクリックON/OFF
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void MoveBackword_MoveStateCtrl(object arg1, ChkChangeEventArgs arg2)
        {
            BackwordMoveStsChanged?.Invoke(arg1, arg2);
        }
        /// <summary>
        /// 数値変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumUDDecimalSlider_ValueChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore){ return;}

            //スライダーで変更
            if (NumUDDecimalSlider.IsSliderDecition)
            {
                MoveIdexChanged?.Invoke(sender, new NumUpdateEventArgs(NumUDDecimalSlider.ChangedValue));
            }

            //値を直入力して変更
            if(NumUDDecimalSlider.IsDirectDecition)
            {
                MoveIdexChanged?.Invoke(sender, new NumUpdateEventArgs(NumUDDecimalSlider.ChangedValue));
            }
        }
    }
}
