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
    public partial class frmMechaMove : Form
    {
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmMechaMove myForm = null;

        public frmMechaMove()
        {
            InitializeComponent();
        }

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmMechaMove Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmMechaMove();
                }

                return myForm;
            }
        }
        #endregion

        public bool MechaMove(float FX, float fy, float FCD, float FID, float y, float z)
        {
            return true;
 
        }


    }
}
