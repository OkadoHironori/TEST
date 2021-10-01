using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TXSMechaControl.CommonComp
{
    public partial class UC_StsCheckBtn : UserControl
    {
        /// <summary>
        /// リセットボタン押下
        /// </summary>
        public event EventHandler ResetEventArg;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UC_StsCheckBtn()
        {
            InitializeComponent();
        }
        /// <summary>
        /// セット
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="enable"></param>
        /// <param name="chk"></param>
        public void SetText(string txt, bool enable, bool chk)
        {
            lblDisp.Text = chk ? "OK" : "Alarm";
            LED.SetEnableChek(enable, chk);
            Reset.Enabled = !chk;
        }
        /// <summary>
        /// リセットボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset_Click(object s, EventArgs e)
        {
            ResetEventArg?.Invoke(s, e);
        }
    }
}
