using System.Windows.Forms;
using System.Drawing;
using System;
using System.Diagnostics;

namespace CT30K
{
    /// <summary>
    /// 固定フォーム
    /// </summary>
    public class FixedForm : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FixedForm()
        {
            // 固定ダイアログ
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            // コントロールボックスは非表示
            this.ControlBox = false;

            // 表示位置はマニュアル
            this.StartPosition = FormStartPosition.Manual;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="f">親フォーム</param>
        /// <param name="x">表示位置のＸ座標</param>
        /// <param name="y">表示位置のＹ座標</param>
        public FixedForm(Form f, int x, int y)
            : this()
        {
            //親フォーム
            this.MdiParent = f;

            //表示位置
            this.Location = new Point(x, y);
        }

        /// <summary>
        /// WndProc メソッドをオーバーライドする：フォームを移動させないための措置
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;
            const int SC_MASK = 0xFFF0;

            if (m.Msg == WM_SYSCOMMAND)
            {
                int status = m.WParam.ToInt32() & SC_MASK;
                
                //フォームの移動を捕捉したら以降の制御をカットする
                if (status == SC_MOVE) return;
            }

            //基本クラスのメソッドを実行する
            base.WndProc(ref m);
        }

    }
}
