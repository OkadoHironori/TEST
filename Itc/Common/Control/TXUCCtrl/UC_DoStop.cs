
namespace Itc.Common.Controls.TXUCCtrl
{
    using System;
    using System.Windows.Forms;
    public partial class UC_DoStop : UserControl
    {
        /// <summary> 
        /// 実行
        /// </summary>
        public Action DoExecute { private get; set; }

        /// <summary> 
        /// 閉じる
        /// </summary>
        public Action DoClose { private get; set; }

        public UC_DoStop()
        {
            InitializeComponent();
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            DoExecute?.Invoke();
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            DoClose?.Invoke();
        }
    }
}
