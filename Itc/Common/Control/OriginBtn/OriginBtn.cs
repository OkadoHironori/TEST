using Itc.Common.Event;
using Itc.Common.TXEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Itc.Common.Controls.MoveBtn
{
    using ChkChangeEventHandler = Action<object, ChkChangeEventArgs>;

    public partial class OriginBtn : Button
    {
        /// <summary>
        /// 制限によって色を変更する
        /// </summary>
        private LimitMode _limit;
        public LimitMode Limit
        {
            get { return _limit; }
            set
            {
                if (Equals(_limit, value)) return;

                _limit = value;

                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        ChangeBackcoloer(_limit);
                    });
                }
                ChangeBackcoloer(_limit);
            }
        }
        /// <summary>
        /// バックカラーを変更
        /// </summary>
        /// <param name="mode"></param>
        private void ChangeBackcoloer(LimitMode mode)
        {
            if (_limit == LimitMode.limitless)
            {
                //バックをコントロールカラーにする
                this.BackColor = SystemColors.Control;
            }
            else
            {
                //バックをコントロールカラーにする
                this.BackColor = Color.FromArgb(0xff, 0xff, 0x7f);
            }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OriginBtn()
        {
            //バックをコントロールカラーにする
            this.BackColor = SystemColors.Control;
            //フォント
            this.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            //タブによるフォーカス移動を禁止
            this.TabStop = false;
        }
    }
}
