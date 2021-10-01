using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Itc.Common.Event;
using Itc.Common.Basic;
using Itc.Common.Extensions;

namespace TXSMechaControl.CommonComp
{
    using NumUpdateEventHandler = Action<object, NumUpdateEventArgs>;
    /// <summary>
    /// 
    /// </summary>
    public partial class UC_NUM_Update : UserControl
    {
        /// <summary>
        /// 実行イベント
        /// </summary>
        public event NumUpdateEventHandler NumUpdateArg;
        /// <summary>
        /// キャンセルなのでそのままアップデート要求
        /// </summary>
        public event EventHandler RequestUpdate;
        /// <summary>
        /// 実行可否問い合わせ
        /// </summary>
        public Func<NumBase, string, ResBase> IsDoExecut { private get; set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UC_NUM_Update()
        {
            InitializeComponent();

            CONF_DATA.IsDoExecut = (f, mes) =>
            {
                NumBase numb = new NumBase(this.CONF_DATA.Maximum, this.CONF_DATA.Minimum, f, this.CONF_DATA.DecimalPlaces, this.CONF_DATA.Increment);
                return IsDoExecut.Invoke(numb, mes);
            };

            CONF_DATA.RequestUpdate += (s, e) =>
              {
                  RequestUpdate?.Invoke(s, e);
              };

            CONF_DATA.ChangeDataUpdate += (s, e) =>
            {
                NumUpdateArg?.Invoke(s, e);
            };

        }
        /// <summary>
        /// 初期値を入力
        /// </summary>
        /// <param name="name">項目名</param>
        /// <param name="decp">有効桁数</param>
        /// <param name="max">最大値</param>
        /// <param name="min">最小値</param>
        public void SetInitValue(string name, int decp, float max, float min)
        {
            try
            {
                CONF_DATA.DirectChanged = true;
                CONF_DATA.SetPreviousValue((max + min) / 2);
                CONF_DATA.SetIniDeci(decp, max, min);
                TXT_BOX.Text = name;
            }
            finally
            {
                CONF_DATA.DirectChanged = false;
            }
        }
        /// <summary>
        /// Enable設定
        /// </summary>
        /// <param name="data"></param>
        public void SetEnabel(bool _enable)
        {
            TXT_BOX.Enabled = _enable;
            CONF_DATA.Enabled = _enable;
        }
        /// <summary>
        /// 値を入力
        /// </summary>
        /// <param name="data"></param>
        public void SetValue(float data)
        {
            try
            {
                var tmpdata = data.CorrectRange<float>((float)CONF_DATA.Minimum, (float)CONF_DATA.Maximum);


                CONF_DATA.DirectChanged = true;
                CONF_DATA.Value = (decimal)tmpdata;
            }
            finally
            {
                CONF_DATA.DirectChanged = false;
            }
        }
    }
}
