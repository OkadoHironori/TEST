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
    public partial class frmCorrectionStatusModal : Form
    {
        //frmCorrectionStatusのモーダル用Form
        //
        //ShowDialgで開くと最小化のときに前面に表示されるため　
        //frmCorrectionStatusのコントロールをここに表示する

        #region インスタンスを返すプロパティ

        // frmAdditionのインスタンス
        private static frmCorrectionStatusModal _Instance = null;

        /// <summary>
        /// frmAdditionのインスタンスを返す
        /// </summary>
        public static frmCorrectionStatusModal Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmCorrectionStatusModal();
                }

                return _Instance;
            }
        }

        #endregion
        
        public frmCorrectionStatusModal()
        {
            InitializeComponent();
        }

    }
}
