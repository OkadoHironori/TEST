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
    public partial class frmFInteg : Form
    {
        public frmFInteg()
        {
            InitializeComponent();
        }


        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmFInteg myForm = null;


        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmFInteg Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmFInteg();
                }

                return myForm;
            }
        }
        #endregion    

    }
}
