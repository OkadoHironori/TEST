using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class frmMeca : Form
    {
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmMeca myForm = null;

        public frmMeca()
        {
            InitializeComponent();
        }

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmMeca Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmMeca();
                }

                return myForm;
            }
        }
        #endregion    

    }
}
