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
    public partial class frmScanCondition : Form
    {
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmScanCondition myForm = null;
        
        public frmScanCondition()
        {
            InitializeComponent();
        }
        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmScanCondition Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmScanCondition();
                }

                return myForm;
            }
        }
        #endregion

        
        public void Setup(bool IsVisible = false, int IsConeBeam = 0, int k = 0, double swPix = 0, double delta_z_pix = 0) { }



        public bool ScanOptValueChk2ok { get; set; }


 

    }
}
