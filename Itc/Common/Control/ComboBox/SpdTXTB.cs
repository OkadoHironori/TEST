

namespace Itc.Common.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class SpdTXTB : TextBox
    {
        public SpdTXTB()
        {
            //バックをコントロールカラーにする
            this.BackColor = SystemColors.Control;
            //フォント
            this.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            //タブによるフォーカス移動を禁止
            this.TabStop = false;
        }
        /// <summary>
        /// 値をセット 色変更あり
        /// </summary>
        /// <param name="current"></param>
        /// <param name="allvalue"></param>
        public void SetValue(string current, bool isWarning = false)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string, bool>(SetValue), current, isWarning);
                return;
            }

            this.Text = current;

            this.BackColor = isWarning? Color.FromArgb(0xff, 0xff, 0x7f) : SystemColors.Control;
        }
        /// <summary>
        /// 値のセット
        /// </summary>
        /// <param name="current"></param>
        public void SetValue(string current)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(SetValue), current);
                return;
            }

            this.Text = current;
        }
    }
}
