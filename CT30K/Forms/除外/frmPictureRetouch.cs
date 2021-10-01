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
    public partial class frmPictureRetouch : Form
    {
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmPictureRetouch myForm = null;
        
        public frmPictureRetouch()
        {
            InitializeComponent();
        }

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmPictureRetouch Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmPictureRetouch();
                }

                return myForm;
            }
        }
        #endregion    

        /// <summary>
        /// Targetプロパティ（現在表示中の画像ファイル）
        /// </summary>
        public string Target { get; set; }

    }
}
