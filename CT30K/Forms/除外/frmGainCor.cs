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
    public partial class frmGainCor : Form
    {
        public frmGainCor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmGainCor myForm = null;


        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmGainCor Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmGainCor();
                }

                return myForm;
            }
        }
        #endregion    

    }
}
