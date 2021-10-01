using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CT30K
{
    public partial class frmRetryCondition : Form
    {
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmRetryCondition myForm = null;
        public frmRetryCondition()
        {
            InitializeComponent();
        }

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmRetryCondition Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmRetryCondition();
                }

                return myForm;
            }
        }
        #endregion


        public void Dialog(string Rawname, bool Zooming = false)
        { 
        }

    }
}
