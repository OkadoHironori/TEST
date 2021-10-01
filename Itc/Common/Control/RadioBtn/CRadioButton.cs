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
    public partial class CRadioButton : RadioButton
    {
        //オン時（チェック時）のボタンの色：規定値は「黄色」
        private Color CheckedColor = Color.FromArgb(0xff, 0xff, 0x7f);

        //オフ時（アンチェック時）のボタンの色：規定値は「灰色」
        private Color UnCheckedColor = Color.FromKnownColor(KnownColor.Control);


        public CRadioButton()
        {
            InitializeComponent();

            //アピアランスを「ボタン」にする
            this.Appearance = Appearance.Button;

            //タブによるフォーカス移動を禁止
            this.TabStop = false;
        }
        /// <summary>
        /// 値変更時処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCheckedChanged(EventArgs e)
        {
            //背景色の設定
            this.BackColor = this.Checked ? CheckedColor : UnCheckedColor;

            //継承元の処理を実行
            base.OnCheckedChanged(e);
        }

        //public RadioButton(IContainer container)
        //{
        //    container.Add(this);

        //    InitializeComponent();
        //}
    }
}
