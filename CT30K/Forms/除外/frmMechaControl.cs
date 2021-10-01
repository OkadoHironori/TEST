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
    public partial class frmMechaControl : Form
    {
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmMechaControl myForm = null;

        //スピード設定定数
        public enum SpeedConstants
        {
            SpeedSlow,
            SpeedMiddle,
            SpeedFast,
            speedmanual
        }

        public frmMechaControl()
        {
            InitializeComponent();
        }

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmMechaControl Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmMechaControl();
                }

                return myForm;
            }
        }
        #endregion

        private void frmMechaControl_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// キャプチャON状態
        /// </summary>
        //public bool MecaXrayOn { get; set; }

        //FCD（含オフセット）
        public float FCDWithOffset
        {
            get { return 0; }
        }        
        //FID（含オフセット）
        public float FIDWithOffset
        {
            get { return 0; }
        }
        //FCD/FID比
        public float FCDFIDRate
        {
            get { return 0; }
        }
        //Y軸座標
        public float TableYPos
        {
            get { return 0; }
        }


    }
}
