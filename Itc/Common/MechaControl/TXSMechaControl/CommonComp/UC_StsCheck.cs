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
    public partial class UC_StsCheck : UserControl
    {
        public UC_StsCheck()
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

            lblDisp.Text = chk ? "完了" : "未完了";

            //lblDisp.Text = txt;
            LED.SetEnableChek(enable, chk);
        }
    }
}
