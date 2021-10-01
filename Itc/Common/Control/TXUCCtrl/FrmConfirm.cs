using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Itc.Common.Controls.TXUCCtrl
{
    public partial class FrmConfirm : Form
    {
        /// <summary>
        /// 実行可否
        /// </summary>
        public Action<bool> DoExcute { private get; set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FrmConfirm(string mes)
        {
            InitializeComponent();

            this.Text = mes;


            UC_DoStop.DoExecute = () =>
            {
                DoExcute.Invoke(true);
                this.Close();
            };

            UC_DoStop.DoClose = () =>
            {
                DoExcute.Invoke(false);
                this.Close();
            };

        }
    }
}
