using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Itc.Common.Controls.XrayChkBox
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public partial class ToolStripCheckBoxImage : ToolStripControlHost
    {
        /// <summary>
        /// ToolStripに配置されるCheckButtonコントロール
        /// </summary>
        public ToolStripCheckBoxImage()
            : base(new CheckBoxXrayImage())
        {

        }
        /// <summary>
        /// ホストしているNumUpDownコントロール
        /// </summary>
        public CheckBoxXrayImage CheckedStatus
        {
            get { return Control as CheckBoxXrayImage; }
        }
    }
}
