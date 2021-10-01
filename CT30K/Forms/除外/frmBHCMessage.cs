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
    public partial class frmBHCMessage : Form
    {
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmBHCMessage myForm = null;
        public List<Label> lblInfBHC = null;
        public List<Label> lblPriod = null;

        public frmBHCMessage()
        {
            InitializeComponent();
        }

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmBHCMessage Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmBHCMessage();
                }

                return myForm;
            }
        }
        #endregion    

        private void frmBHCMessage_Load(object sender, EventArgs e)
        {
            lblInfBHC.Add(_lblInfBHC_0);
            lblInfBHC.Add(_lblInfBHC_1);
            lblInfBHC.Add(_lblInfBHC_2);
            lblInfBHC.Add(_lblInfBHC_3);
            lblInfBHC.Add(_lblInfBHC_4);
            lblInfBHC.Add(_lblInfBHC_5);

            lblPriod.Add(_lblPriod_0);
            lblPriod.Add(_lblPriod_1);
            lblPriod.Add(_lblPriod_2);
            lblPriod.Add(_lblPriod_3);
            lblPriod.Add(_lblPriod_4);
            lblPriod.Add(_lblPriod_5);


        }
    }
}
