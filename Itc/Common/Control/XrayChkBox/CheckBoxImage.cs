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
    public partial class CheckBoxXrayImage : CheckBox
    {
        /// <summary>
        /// Checkボタン
        /// </summary>
        public CheckBoxXrayImage()
        {
            InitializeComponent();
            this.Appearance = Appearance.Button;
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }
        /// <summary>
        /// 画像表示
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            if (this.Checked)
            {
                //pevent.Graphics.DrawImage(new Bitmap(ITCRes.status_EMG_on), new Point(0, 0));
                //pevent.Graphics.DrawString("EMG", new Font("MS UI Gothic", 20), Brushes.Blue, 0, 0);
            }
            else
            {
                //pevent.Graphics.DrawImage(new Bitmap(ITCRes.status_EMG_off), new Point(0, 0));
                //pevent.Graphics.DrawString("EMG", new Font("MS UI Gothic", 20), Brushes.Blue, 0, 0);
            }
        }
    }
}
