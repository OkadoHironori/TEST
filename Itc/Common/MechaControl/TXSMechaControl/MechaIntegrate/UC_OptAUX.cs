using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TXSMechaControl.MechaIntegrate
{
    public partial class UC_OptAUX : UserControl
    {
        /// <summary>_ValueChangedイベント抑制フラグ</summary>
        private bool _ChangedIgnore = false;
        /// <summary>
        /// メカインターフェイス
        /// </summary>
        public IMechaIntegrate Mecha { get; set; }
        /// <summary>
        /// DI注入
        /// </summary>
        public void Inject(IMechaIntegrate mecha)
        {
            Mecha = mecha;
            Mecha._Aux.AUXChanged += (s, e) =>
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        UpdateAuxSel(s, e);
                    });
                }
                else
                {
                    UpdateAuxSel(s, e);
                }
            };
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void UpdateAuxSel(object s, EventArgs e)
        {
            try
            {
                _ChangedIgnore = true;
                CB_DoorInterlock.Checked = (s as AuxSel.AuxSel).AuxProvider.DoorInterlock;
                CB_XrayColiSensor.Checked = (s as AuxSel.AuxSel).AuxProvider.XraySourceCollisionSensor;
                CB_XraySourceColi.Checked = (s as AuxSel.AuxSel).AuxProvider.XraySourceCollisionPrevent;
            }
            finally
            {
                _ChangedIgnore = false;
            }

        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UC_OptAUX()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 扉インターロック変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_DoorInterlock_CheckedChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore) { return; }            
            Mecha._Aux.ChangeDoorInterlock(CB_DoorInterlock.Checked, null);
        }
        /// <summary>
        /// センサ取付チェック入力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_XrayColiSensor_CheckedChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore) { return; }
            Mecha._Aux.ChangeXrayColiChecker(CB_XrayColiSensor.Checked, null);
        }
        /// <summary>
        /// 浜松ホト干渉防止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_XraySourceColi_CheckedChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore) { return; }
            Mecha._Aux.ChangeXrayColiPrevent(CB_XraySourceColi.Checked, null);
        }
    }
}
