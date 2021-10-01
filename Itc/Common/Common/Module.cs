using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Itc.Common
{
    public class Module
    {
        /// <summary>
        ///  文字列とComboBoxのItemsコレクションを比較して該当するインデックスに設定する
        ///  古のCTのソースを流用
        /// </summary>
        /// <param name="obj">ComboBoxコンロトロール</param>
        /// <param name="conmpstr">比較文字列</param>
        /// <returns>インデックスを返す</returns>
        public static int SetComboBoxSelectedIndex(ComboBox obj, string conmpstr)
        {
            int ret = 0;

            for (int i = 0; i < obj.Items.Count; i++)
            {
                string st = obj.Items[i].ToString();
                if (st.Contains(conmpstr))
                {
                    obj.SelectedIndex = i;
                    ret = i;
                    break;
                }
            }
            //該当ない場合は先頭を設定する
            if (ret == -1)
            {
                obj.SelectedIndex = 0;
                ret = 0;
            }


            return ret;
        }
    }
}
