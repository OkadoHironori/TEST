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
    public partial class frmPostConeReconstruction : Form
    {
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmPostConeReconstruction myForm = null;

        public frmPostConeReconstruction()
        {
            InitializeComponent();
        }

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmPostConeReconstruction Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmPostConeReconstruction();
                }

                return myForm;
            }
        }
        #endregion
    }
}
