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
    public partial class frmScanControl : Form
    {
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmScanControl myForm = null;

        internal List<Button> cmdImageProc = null;

        public frmScanControl()
        {
            InitializeComponent();

            cmdImageProc = new List<Button>();
            cmdImageProc.Add(cmdImageProc1);

        }

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmScanControl Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmScanControl();
                }

                return myForm;
            }
        }
        #endregion    
  
        //フラットパネル設定「ゲイン」「積分時間」更新処理
        public int SetFpdGainInteg(int FpdGainIndex, int FpdIntegIndex)		//v17.10変更 byやまおか 2010/08/26
        {
            int functionReturnValue = 0;
            return functionReturnValue;
        }
  
    }
}
