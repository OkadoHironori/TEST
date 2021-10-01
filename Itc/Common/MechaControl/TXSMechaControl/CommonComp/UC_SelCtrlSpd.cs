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

namespace TXSMechaControl.CommonComp
{
    /// <summary>_イベントデリゲート宣言</summary>
    using CSelectChangeEventHandler = Action<object, SelectChangeEventArgs>;
    using NumUpdateEventHandler = Action<object, NumUpdateEventArgs>;

    public partial class UC_SelCtrlSpd : UserControl
    {
        /// <summary>_ValueChangedイベント抑制フラグ</summary>
        private bool _ChangedIgnore = false;
        /// <summary>
        /// 選択イベント
        /// </summary>
        public event CSelectChangeEventHandler SelectChange;
        /// <summary>
        /// 数値変更イベント
        /// </summary>
        public event NumUpdateEventHandler NumChanged;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UC_SelCtrlSpd()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 値を入力
        /// </summary>
        public void SetValue(SelectSPD current, IEnumerable<SelectSPD> listnames, float max, float min, string unit)
        {
            _ChangedIgnore = true;

            var querlist = listnames.Select(p => p.DispName);

            Unit.Text = unit;

            NTB_SPD.SetValue(current.SPD, max, min, "0.01");

            var querspd = listnames.ToList().Find(p => p.SPD == current.SPD);

            if (querspd != null)
            {
                current = querspd;
                SPD_CB.SetValue(current.DispName, querlist);
            }
            else
            {
                SPD_CB.SetValue(null, querlist);
            }
            _ChangedIgnore = false;
        }

        /// <summary>
        /// 数値変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NTB_SPD_ValueChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore) { return; }
            NumChanged?.Invoke(this, new NumUpdateEventArgs(NTB_SPD.Value));
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
