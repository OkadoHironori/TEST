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
    public partial class BaseDialog : Form
    {
        public BaseDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ロード時の処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            //
            if (this.DesignMode) return;

            // メインフォームをオーナーとする
            this.Owner = frmCTMenu.Instance;

            // フォームを標準位置に移動
            SetPosNormalForm(this);

            // 継承元の処理を実行
            base.OnLoad(e);
        }

        #region フォームの位置を設定する
        /// <summary>
        /// フォームの位置を設定する
        /// 画面左側の中央に位置設定する
        /// </summary>
        /// <param name="form">位置設定するフォーム</param>
        private static void SetPosNormalForm(Form form)
        {
            // 画面をはみ出す場合は、ツールバー位置にそろえる(CT画像にかぶる)     'v15.0追加 byやまおか 2009/07/30
            int pos_left = frmScanControl.Instance.Width + frmCTMenu.Instance.Toolbar1.Width + 2 - form.Width;
            int toolbar_right = frmCTMenu.Instance.Toolbar1.Right + 2;

            form.Location = new Point(Math.Max(pos_left, toolbar_right), Screen.PrimaryScreen.Bounds.Height / 2 - form.Height / 2);
        }
        #endregion


    }
}
