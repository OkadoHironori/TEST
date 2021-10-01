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
    public partial class frmRoiTool : Form
    {
        public frmRoiTool()
        {
            InitializeComponent();
        }
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmRoiTool myForm = null;


        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmRoiTool Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmRoiTool();
                }

                return myForm;
            }
        }
        #endregion    

    }
}
