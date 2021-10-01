using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Itc.Common.Controls
{
    /// <summary>
    /// 幅・高さを自動調整するFlowLayoutPanel
    /// </summary>
    public partial class FlowLayoutPanelEx : FlowLayoutPanel
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public FlowLayoutPanelEx()
        {
            InitializeComponent();

            WrapContents = false;
        }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="container"></param>
        public FlowLayoutPanelEx(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            WrapContents = false;
        }

        /// <summary>
        /// 幅または高さを自動調整する
        /// </summary>
        [Browsable(true)]
        public bool AutoAdjust { get; set; }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

            if (!AutoAdjust)
            {
                return;
            }

            bool isHorizontal = FlowDirection == FlowDirection.TopDown || FlowDirection == FlowDirection.BottomUp;

            Action<Control> adjust = isHorizontal
                ? new Action<Control>(_AdjustWidth)
                : new Action<Control>(_AdjustHeight);

            foreach (Control c in Controls)
            {
                adjust(c);
            }
        }

        private void _AdjustWidth(Control control) => control.Width = Width - control.Margin.Horizontal - Padding.Horizontal;

        private void _AdjustHeight(Control control) => control.Height = Height - control.Margin.Vertical - Padding.Vertical;
    }
}
