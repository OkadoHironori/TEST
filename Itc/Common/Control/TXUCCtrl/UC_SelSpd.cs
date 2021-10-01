using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Itc.Common.TXEnum;
using Itc.Common.Event;

namespace Itc.Common.Controls.TXUCCtrl
{
    /// <summary>_イベントデリゲート宣言</summary>
    using CSelectChangeEventHandler = Action<object, SelectChangeEventArgs>;

    public partial class UC_SelSpd : UserControl
    {
        /// <summary>_ValueChangedイベント抑制フラグ</summary>
        private bool _ChangedIgnore = false;
        /// <summary>
        /// 選択イベント
        /// </summary>
        public event CSelectChangeEventHandler SelectChange;
        /// <summary>
        /// 
        /// </summary>
        public UC_SelSpd()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 値を入力
        /// </summary>
        public void SetValue(SelectSPD current, IEnumerable<SelectSPD> listnames)
        {
            _ChangedIgnore = true;
            var querlist = listnames.Select(p => p.DispName);
            SPD_CB.SetValue(current.DispName, querlist);
            SpdTXT.SetValue(current.SPD.ToString()+"mm/sec");
            _ChangedIgnore = false;
        }
        /// <summary>
        /// 速度変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SPD_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore) { return; }
            SelectChange?.Invoke(this, new SelectChangeEventArgs(SPD_CB.SelectedItem.ToString()));
        }
    }
}
