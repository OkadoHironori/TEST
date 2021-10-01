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
    public partial class FormTransparence : Form
    {

        #region インスタンスを返すプロパティ

        //FormTransparenceのインスタンス
        //最小化した透明な化フォームを表示する
        private static FormTransparence _Instance = null;

        /// <summary>
        /// FormTransparenceのインスタンスを返す
        /// </summary>
        public static FormTransparence Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new FormTransparence();
                }

                return _Instance;
            }
        }

        #endregion
        
        public FormTransparence()
        {
            InitializeComponent();
        }

        private void FormTransparence_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Opacity = 0;
            //this.WindowState = FormWindowState.Minimized;
            this.Location = new Point(frmCTMenu.Instance.Left, frmCTMenu.Instance.Top - this.Height -10);

        }
    }
}
