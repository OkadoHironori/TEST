using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Itc.Common.Controls.LabelEx
{
    public partial class LabelLED : Label
    {
        public LabelLED():base()
        {
            this.Text = string.Empty;

            //フォント
            this.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));

            this.Margin = new Padding(4,0,4,0);

            this.Size = new Size(24, 32);


            this.BackColor = Color.Gray;

            this.BorderStyle = BorderStyle.Fixed3D;

            this.FlatStyle = FlatStyle.Standard;
            
        }
        /// <summary>
        /// Enableの変更
        /// </summary>
        /// <param name="enable"></param>
        public void SetEnableChek(bool enable, bool chk)
        {
            this.Enabled = enable;
            this.BackColor = enable ? Color.Gray : Color.LimeGreen;

            if(enable)
            {
                this.BackColor = chk ? Color.LimeGreen : Color.Red;
            }
        }
    }
}
