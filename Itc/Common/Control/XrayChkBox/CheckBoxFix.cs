using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Itc.Common.Controls.XrayChkBox
{
    public partial class CheckBoxFix : CheckBox
    {
        /// <summary>
        /// Checkボタン
        /// </summary>
        public CheckBoxFix()
        {
            this.TextAlign = ContentAlignment.MiddleRight;
            this.Appearance = Appearance.Button;

        }
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = false; }
        }
        /// <summary>
        /// 画像表示
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int h = this.ClientSize.Height - 2;
            Rectangle rc = new Rectangle(new Point(0, 1), new Size(h, h));
            //ControlPaint.DrawCheckBox(e.Graphics, rc,
            //    this.Checked ? ButtonState.Checked : ButtonState.Normal);

            ControlPaint.DrawButton(e.Graphics, rc,
                     this.Checked ? ButtonState.Checked : ButtonState.Normal);
        }
    }
}
