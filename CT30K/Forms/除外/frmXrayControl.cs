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
    public partial class frmXrayControl : Form
    {
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmXrayControl myForm = null;

        
        public frmXrayControl()
        {
            InitializeComponent();
        }

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmXrayControl Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmXrayControl();
                }

                return myForm;
            }
        }
        #endregion

        /// <summary>
        /// MecaXrayOn状態
        /// </summary>
        public modCT30K.OnOffStatusConstants MecaXrayOn { get; set; }

        public string XrayStatus { get; set; }

        public modCT30K.OnOffStatusConstants MecaXrayAvailable { get; set; }


        internal void cmdWarmupStart_Click(object sender, EventArgs e)
        {

        }







    }
}
