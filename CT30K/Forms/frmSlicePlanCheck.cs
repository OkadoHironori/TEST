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
    public partial class frmSlicePlanCheck : Form
    {
        public frmSlicePlanCheck()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmSlicePlanCheck myForm = null;


        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmSlicePlanCheck Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmSlicePlanCheck();
                }

                return myForm;
            }
        }
        #endregion

        public bool Dialog(string strInfo = "")
        {
            return true;
        }

    }
}
