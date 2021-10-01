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
    public partial class BtnEncLockOn : ToolStripButton
    {
        /// <summary>
        /// ロックボタン
        /// </summary>
        public BtnEncLockOn()
        {
            //this.TextAlign = ContentAlignment.MiddleRight;

            this.CheckOnClick = true;
            this.Image = new Bitmap(ITCRes.bt_lock_open);
            this.ImageAlign = ContentAlignment.MiddleCenter;
            this.ImageScaling = ToolStripItemImageScaling.None;

            this.CheckedChanged += (s, e) =>
            {
                if (this.Checked)
                {
                    this.Image = new Bitmap(ITCRes.bt_lock_dis);
                }
                else
                {
                    this.Image = new Bitmap(ITCRes.bt_lock_open);
                }
            };

            this.EnabledChanged += (s, e) =>
            {
                if (this.Enabled)
                {
                    if (this.Checked)
                    {
                        this.Image = new Bitmap(ITCRes.bt_lock_dis);
                    }
                    else
                    {
                        this.Image = new Bitmap(ITCRes.bt_lock_open);
                    }
                }
                else
                {
                    if (this.Checked)
                    {
                        this.Image = new Bitmap(ITCRes.bt_lock_dis);
                    }
                    else
                    {
                        this.Image = new Bitmap(ITCRes.bt_lock_open);
                    }
                }
            };

     
        }

        /// <summary>
        /// 画像表示
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaint(PaintEventArgs e)
        {

            base.OnPaint(e);

            //if (this.Checked)
            //{
            //    // this.Image = new Bitmap(ITCRes.status_EMG_on);
            //    this.Image = new Bitmap(ITCRes.bt_x_ray_dis);

            //}
            //else
            //{
            //    this.Image = new Bitmap(ITCRes.bt_x_ray_orange);
            //    //this.Image = new Bitmap(ITCRes.status_EMG_off);
            //}
        }
    }
}
