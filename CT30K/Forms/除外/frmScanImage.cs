using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace CT30K
{
    public partial class frmScanImage : Form
    {
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmScanImage myForm = null;

        public frmScanImage()
        {
            InitializeComponent();
        }

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmScanImage Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmScanImage();
                }

                return myForm;
            }
        }
        #endregion    

        /// <summary>
        /// Targetプロパティ（現在表示中の画像ファイル）
        /// </summary>
        public string Target { get; set; }


        public int PicWidth
        {
            //戻り値セット
            get { return 0; }
        }

        public int PicHeight
        {
            //戻り値セット
            get { return 0; }
        }
        public int Magnify
        {
            //戻り値セット
            get { return 0; }
        }

        public void UpdateMagnifyCaption()
        {
  
        }


        public void ClickToolBarButton(ToolStripButton button)
        {
            ToolStripItemClickedEventArgs te = null;
            
            button.Checked = true;
            Toolbar1_ItemClicked(button, te);
          
        }

        private void Toolbar1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


    }
}
