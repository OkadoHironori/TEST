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

namespace TXSMechaControl.MechaIntegrate
{
    /// <summary>_イベントデリゲート宣言</summary>
    using CSelectChangeEventHandler = Action<object, SelectChangeEventArgs>;
    /// <summary>
    /// 
    /// </summary>
    public partial class UC_SelAxis : UserControl
    {
        /// <summary>_ValueChangedイベント抑制フラグ</summary>
        private bool _ChangedIgnore = false;
        /// <summary>
        /// 選択した軸(前回値を記憶しておく)
        /// </summary>
        public Itc.Common.TXEnum.OptMechaAxis SelOpt { get; private set; } = TXSMechaControl.MechaIntegrate.MechaParam.Default.Opt;
        /// <summary>
        /// イベント
        /// </summary>
        public event CSelectChangeEventHandler SelAxisArg;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UC_SelAxis()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            _ChangedIgnore = true;
        }
        /// <summary>
        /// 初期化完了
        /// </summary>
        public void EndInit()
        {  
            _ChangedIgnore = false;

            switch (SelOpt.ToString().ToUpper())
            {
                case ("FCD"):
                    FCD.Checked = true;
                    break;
                case ("FDD"):
                    FDD.Checked = true;
                    break;
                case ("ROT"):
                    Rot.Checked = true;
                    break;
                case ("UPDOWN"):
                    UpDown.Checked = true;
                    break;
                case ("FSTAGE"):
                    FStage.Checked = true;
                    break;
                case ("AUX"):
                    AUXSel.Checked = true;
                    break;
                case ("TBLY"):        //Add2020/12/17hata
                    TblY.Checked = true;
                    break;
                default:
                    throw new Exception($"No develop {SelOpt.ToString()}");
            }
        }
        /// <summary>
        /// FDDクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FDD_CheckedChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore) { return; }

            SelOpt = Itc.Common.TXEnum.OptMechaAxis.FDD;
            MechaParam.Default.Opt = SelOpt;
            MechaParam.Default.Save();

            SelAxisArg?.Invoke(sender, new SelectChangeEventArgs(SelOpt.ToString()));
        }
        /// <summary>
        /// FCDクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FCD_CheckedChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore) { return; }

            SelOpt = Itc.Common.TXEnum.OptMechaAxis.FCD;
            MechaParam.Default.Opt = SelOpt;
            MechaParam.Default.Save();

            SelAxisArg?.Invoke(sender, new SelectChangeEventArgs(SelOpt.ToString()));
        }
        /// <summary>
        /// その他クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AUX_CheckedChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore) { return; }

            SelOpt = Itc.Common.TXEnum.OptMechaAxis.AUX;
            MechaParam.Default.Opt = SelOpt;
            MechaParam.Default.Save();

            SelAxisArg?.Invoke(sender, new SelectChangeEventArgs(SelOpt.ToString()));
        }
        /// <summary>
        /// 回転クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rot_CheckedChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore) { return; }

            SelOpt = Itc.Common.TXEnum.OptMechaAxis.Rot;
            MechaParam.Default.Opt = SelOpt;
            MechaParam.Default.Save();

            SelAxisArg?.Invoke(sender, new SelectChangeEventArgs(SelOpt.ToString()));
        }
        /// <summary>
        /// 昇降クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpDown_CheckedChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore) { return; }

            SelOpt = Itc.Common.TXEnum.OptMechaAxis.UPDOWN;
            MechaParam.Default.Opt = SelOpt;
            MechaParam.Default.Save();

            SelAxisArg?.Invoke(sender, new SelectChangeEventArgs(SelOpt.ToString()));
        }
        /// <summary>
        /// 微調クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FStage_CheckedChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore) { return; }

            SelOpt = Itc.Common.TXEnum.OptMechaAxis.FSTAGE;
            MechaParam.Default.Opt = SelOpt;
            MechaParam.Default.Save();
            SelAxisArg?.Invoke(sender, new SelectChangeEventArgs(SelOpt.ToString()));
        }

        //Add2020/12/17hata
        private void TblY_CheckedChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore) { return; }

            ////Add2020/12/17hata
            //var ctl = (sender as Itc.Common.Controls.CRadioButton);
            //if (ctl != null && !ctl.Checked) { return; }

            SelOpt = Itc.Common.TXEnum.OptMechaAxis.TBLY;
            MechaParam.Default.Opt = SelOpt;
            MechaParam.Default.Save();

            SelAxisArg?.Invoke(sender, new SelectChangeEventArgs(SelOpt.ToString()));
        }
    }
}
