using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//
using CT30K.Common;
using CTAPI;
using TransImage;
//using CT30K.Controls;
//using CT30K.Modules;
using CT30K.Properties;

namespace CT30K
{
    public partial class TestReloaForm : Form
    {
        private static TestReloaForm _Instance = null;

        bool bPIOCheck = false;
        bool bMecainf = false;
        bool bSeqComm = false;
        bool bMecainfSeqComm = false;
        bool bXrayUpdate = false;

        public TestReloaForm()
        {
            InitializeComponent();
        }


        public static TestReloaForm Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new TestReloaForm();
                }

                return _Instance;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mod2ndDetctor.ReloadForms();

            bPIOCheck = frmMechaControl.Instance.tmrPIOCheck.Enabled;
            bMecainf = frmMechaControl.Instance.tmrMecainf.Enabled;
            bSeqComm = frmMechaControl.Instance.tmrSeqComm.Enabled;
            bMecainfSeqComm = frmMechaControl.Instance.tmrMecainfSeqComm.Enabled;
            bXrayUpdate = frmXrayControl.Instance.tmrUpdate.Enabled;

            frmXrayControl.Instance.tmrUpdate.Enabled =true;
        }

        private void ReloaTestForm_Load(object sender, EventArgs e)
        {
            bPIOCheck = frmMechaControl.Instance.tmrPIOCheck.Enabled;
            bMecainf =  frmMechaControl.Instance.tmrMecainf.Enabled;
            bSeqComm = frmMechaControl.Instance.tmrSeqComm.Enabled;
            bMecainfSeqComm = frmMechaControl.Instance.tmrMecainfSeqComm.Enabled;
            bXrayUpdate = frmXrayControl.Instance.tmrUpdate.Enabled;

        }
    }
}
