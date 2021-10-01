using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Itc.Common.Controls
{
    /// <summary>
    /// 
    /// </summary>
    [System.ComponentModel.ToolboxItem(typeof(System.Drawing.Design.ToolboxItem))]
    public partial class CheckButton : CheckBox
    {
        /// <summary>
        /// Checked状態の背景色
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        public System.Drawing.Color CheckedColor { get; set; }

        /// <summary>
        /// Unchecked状態の背景色
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        public System.Drawing.Color UncheckedColor { get; set; }

        /// <summary>
        /// Indeterminate状態の背景色
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        public System.Drawing.Color IndeterminateColor { get; set; }

        /// <summary>
        /// 新しいインスタンスを生成します
        /// </summary>
        public CheckButton()
            : base()
        {
            InitializeComponent();

            this.Appearance = Appearance.Button;

            this.TextAlign = ContentAlignment.MiddleCenter;

            this.CheckStateChanged += CheckButton_CheckStateChanged;

            this.UseVisualStyleBackColor = false;

            CheckButton_CheckStateChanged(this, EventArgs.Empty);
        }

        void CheckButton_CheckStateChanged(object sender, EventArgs e)
        {
            switch (this.CheckState)
            {
                case CheckState.Checked:
                    this.BackColor = CheckedColor;
                    break;

                case CheckState.Indeterminate:
                    this.BackColor = IndeterminateColor;
                    break;

                case CheckState.Unchecked:
                    this.BackColor = UncheckedColor;
                    break;

                default:
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
