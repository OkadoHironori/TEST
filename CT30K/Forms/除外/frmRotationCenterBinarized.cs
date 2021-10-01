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
    public partial class frmRotationCenterBinarized : Form
    {
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmRotationCenterBinarized myForm = null;
        

        public frmRotationCenterBinarized()
        {
            InitializeComponent();
        }

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmRotationCenterBinarized Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmRotationCenterBinarized();
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
