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
    public partial class frmTransImage : Form
    {

        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmTransImage myForm = null;

        public int adv = 0;
        public int hMil =0 ;

        public frmTransImage()
        {
            InitializeComponent();
        }

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmTransImage Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmTransImage();
                }

                return myForm;
            }
        }
        #endregion

        /// <summary>
        /// キャプチャON状態
        /// </summary>
        public bool CaptureOn { get; set; }

        /// <summary>
        /// ZoomScale
        /// </summary>
        public long ZoomScale { get; set; }

        
        //フレームレートの計算
        public float GetCurrentFR()
        {
            return 0f;
        }

        //フレームレートの計算
        public void Update(bool Captured, int RasterOP)
        {
        }
    }
}
