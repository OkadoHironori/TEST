

namespace Itc.Common.Controls.TextBoxReadOnly
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// ReadOnlyのテキストボックス（TextBox）
    /// </summary>
    public partial class TextBoxReadOnly : TextBox
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TextBoxReadOnly()
            : base()
        {
            //バックをコントロールカラーにする
            this.BackColor = SystemColors.Control;
            //ReadOnly属性
            this.ReadOnly = true;
            //フォント
            this.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));

            //タブによるフォーカス移動を禁止
            this.TabStop = false;
        }
    }
}
