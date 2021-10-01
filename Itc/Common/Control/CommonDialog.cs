using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Itc.Common.Controls
{
    public partial class CommonDialog : Form
    {
        UserControl MyControl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CommonDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userControl"></param>
        public void SetUserControl(UserControl userControl)
        {
            if (userControl == null)
            {
                throw new ArgumentNullException(nameof(userControl));
            }

            panel2.Controls.Add(userControl);

            userControl.Dock = DockStyle.Fill;

            MyControl = userControl;

            Text = userControl.Name;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!this.Modal)
            {
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (!this.Modal)
            {
                Close();
            }
        }
    }
}
