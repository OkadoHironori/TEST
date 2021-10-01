using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Itc.Common.Controls
{
    /// <summary>
    /// ドロップダウンボタン
    /// </summary>
    /// <remarks>クリックするとメニューを表示する</remarks>
    [ToolboxItem(typeof(System.Drawing.Design.ToolboxItem))]
    public partial class DropdownButton : Button
    {
        public DropdownButton()
        {
            InitializeComponent();
        }

        public DropdownButton(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            this.ContextMenuStrip?.Show(this, new Point(0, this.Height), ToolStripDropDownDirection.BelowRight);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            var g = pevent.Graphics;
            var font = new Font("Marlett", this.Font.Size);
            string s = "6"; //Marlettの6が下向きの三角マークになる
            Rectangle rect = this.ClientRectangle;
            SizeF size = g.MeasureString(s, font);

            using(var brush = new SolidBrush(this.ForeColor))
            {
                g.DrawString(s, font, brush, rect.Width - size.Width - 3, (rect.Height - size.Height) / 2.0f);
            }
        }
    }
}
