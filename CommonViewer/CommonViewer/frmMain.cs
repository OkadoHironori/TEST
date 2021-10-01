using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Diagnostics;
using System.Net;



namespace CommonViewer
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        //コモン
        private void mnuFileSub_1_Click(object sender, EventArgs e)
        {
            frmCommonViewer.Instance.Show(this);
            //ShowChild(frmCommonViewer.Instance,0,0);
        }
        //付帯情報
        private void mnuFileSub_2_Click(object sender, EventArgs e)
        {
            frmInformation.Instance.Show(this);
        }
        //終了
        private void mnuFileSub_4_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void mnuFile_Click(object sender, EventArgs e)
        {
            //何もせず。
        }

    }
}
