

namespace Itc.Common.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class SpdSelCB : ComboBox
    {
        public SpdSelCB()
        {
            //バックをコントロールカラーにする
            this.BackColor = SystemColors.Control;
            //フォント
            this.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            //タブによるフォーカス移動を禁止
            this.TabStop = false;
        }
        /// <summary>
        /// 値をセットする
        /// </summary>
        /// <param name="current"></param>
        /// <param name="allvalue"></param>
        public void SetValue(string current, IEnumerable<string> allvalue)
        {
            if(string.IsNullOrEmpty(current))
            {
                this.Items.Clear();
                foreach (var user in allvalue)
                {
                    Items.Add(user);
                }
                Items.Add("-");

                this.SelectedIndex = Items.Count - 1;
            }
            else
            {
                this.Items.Clear();
                foreach (var user in allvalue)
                {
                    Items.Add(user);
                }
                SetComboBoxSelectedIndex(current);
            }
        }

        /// <summary>
        /// 値をセットする(int)
        /// </summary>
        /// <param name="current"></param>
        /// <param name="allvalue"></param>
        public void SetValue(int idx, IEnumerable<int> allvalue)
        {
            this.Items.Clear();
            foreach (var user in allvalue)
            {
                Items.Add(user);
            }
            SetComboBoxSelectedIndex(idx.ToString());
        }
        /// <summary>
        /// 値をセットする(当該値がない場合)
        /// </summary>
        /// <param name="current"></param>
        /// <param name="allvalue"></param>
        public void SetValue(string current, IEnumerable<string> allvalue, bool addNotAve)
        {
            this.Items.Clear();
            foreach (var user in allvalue)
            {
                Items.Add(user);
            }
            Items.Add("-");

            this.SelectedIndex = Items.Count - 1;

        }
        /// <summary>
        ///  文字列とComboBoxのItemsコレクションを比較して該当するインデックスに設定する
        /// </summary>
        /// <param name="conmpstr">比較文字列</param>
        /// <returns>インデックスを返す</returns>
        private void SetComboBoxSelectedIndex(string conmpstr)
        {
            int ret = 0;

            for (int i = 0; i < Items.Count; i++)
            {
                string st = Items[i].ToString();
                if (st.Contains(conmpstr))
                {
                    this.SelectedIndex = i;
                    ret = i;
                    break;
                }
            }
            //該当ない場合は先頭を設定する
            if (ret == -1)
            {
                this.SelectedIndex = 0;
                ret = 0;
            }
        }
    }
}
