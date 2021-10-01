using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;
namespace CT30K
{
    public partial class frmVerticalBinarized : Form
    {
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmVerticalBinarized myForm = null;
        
        public frmVerticalBinarized()
        {
            InitializeComponent();
        }

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmVerticalBinarized Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmVerticalBinarized();
                }

                return myForm;
            }
        }
        #endregion


        public bool Dialog()
        {
            return true;
        }
    }
}
