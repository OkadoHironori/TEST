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
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public partial class BtnXrayOn : ToolStripButton
    {
        /// <summary>
        /// Checkボタン
        /// </summary>
        public BtnXrayOn()
        {
            this.CheckOnClick = true;
            this.Image = new Bitmap(ITCRes.bt_x_ray_dis);
            this.ImageAlign = ContentAlignment.MiddleCenter;
            this.ImageScaling = ToolStripItemImageScaling.None;

            this.CheckedChanged += (s, e) =>
            {
                if (this.Checked)
                {
                    this.Image = new Bitmap(ITCRes.bt_x_ray_orange);
                }
                else
                {
                    this.Image = new Bitmap(ITCRes.bt_x_ray_dis);
                }
            };
        }
        /// <summary>
        /// 表示更新
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
}
