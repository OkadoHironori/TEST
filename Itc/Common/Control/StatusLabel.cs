using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace WindowsFormsControlLibrary1
{
    public partial class StatusLabel : Label
    {
        #region - BackingField -

        Color _StatusColor;
        int _ColorWidth = 10;
        int _Direction;

        #endregion

        /// <summary>
        /// ステータスカラー
        /// </summary>
        [Browsable(true)]
        public Color StatusColor
        {
            get => _StatusColor;
            set
            {
                if (value != _StatusColor)
                {
                    _StatusColor = value;

                    Refresh();
                }
            }
        }

        /// <summary>
        /// 塗りつぶしの幅
        /// </summary>
        [Browsable(true)]
        public int ColorWidth
        {
            get => _ColorWidth;
            set
            {
                if (value != _ColorWidth)
                {
                    _ColorWidth = value;

                    Refresh();
                }
            }
        }

        /// <summary>
        /// 表示位置選択（仮）
        /// </summary>
        [Browsable(true)]
        public int Direction
        {
            get => _Direction;
            set
            {
                if(value != _Direction)
                {
                    _Direction = value;

                    Refresh();
                }
            }
        }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public StatusLabel()
        {
            InitializeComponent();

            StatusColor = Color.Red;
        }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="container"></param>
        public StatusLabel(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// 描画イベント
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //一部色を変更して描画する。
            Rectangle rect = new Rectangle(0, 0, ColorWidth, this.Height);

            if(Direction != 0)
            {
                rect.Offset(this.Width - ColorWidth, 0);
            }

            e.Graphics.FillRectangle(new SolidBrush(StatusColor), rect);
        }
    }
}
