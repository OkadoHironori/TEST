using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Itc.Common.Event;

namespace Itc.Common.Controls
{

    using NumUpdateEventHandler = Action<object, NumUpdateEventArgs>;

    public partial class UC_NumUpdate : UserControl
    {
        /// <summary>_ValueChangedイベント抑制フラグ</summary>
        private bool _ChangedIgnore = false;
        /// <summary>
        /// 値を入力したときのイベント
        /// </summary>
        public event NumUpdateEventHandler NumUpdateArg;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UC_NumUpdate()
        {
            InitializeComponent();
        }
        /// <summary>
        /// データ注入
        /// </summary>
        /// <param name="data"></param>
        public void SetValue(float data)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<float>(SetValue), data);
                return;
            }
            _ChangedIgnore = true;
            this.numData.Value = data;
            _ChangedIgnore = false;
        }
        /// <summary>
        /// 更新クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("変更する?","確認",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                NumUpdateArg?.Invoke(sender, new NumUpdateEventArgs(this.numData.Value));
            }
        }
        /// <summary>
        /// 値変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numData_ValueChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore)
            {
                return;
            }            
        }
    }
}
