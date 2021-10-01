using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Itc.Common.FormObject;
using Itc.Common.TXEnum;
using TXSMechaControl.FCD;

namespace TXSMechaControl.MechaIntegrate
{
    public partial class FrmMechaMaint : Form
    {
        private readonly IMechaIntegrate _Mecha;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FrmMechaMaint(IFormMain main, IMechaIntegrate mecha)
        {
            _Mecha = mecha;

            this.Owner = main.FormObject;

            InitializeComponent();

            //メインパネル
            UC_MainPanel.Create(mecha);

            //メッセージの表示(場合によってはメインフォームから表示させること)
            UC_MainPanel.SendMes = (mes) =>
            {
                MessageBox.Show(mes,
                                Application.ProductName,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
            };


            //メカ画面の変更
            selAxis.Init();
            selAxis.SelAxisArg += (s, e) =>
            {
                this.UC_MainPanel.ChangePanel(e.SelectName);
            };

        }
        /// <summary>
        /// FrmMechaMaint表示時に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMechaMaint_Shown(object sender, EventArgs e)
        {
            selAxis.EndInit();

            _Mecha._FCD.UpdateAll();

            _Mecha._FDD.UpdateAll();

            _Mecha._TableY.UpdateAll();//Add2020/12/17hata

            _Mecha._Rot.UpdateAll();

            _Mecha._Ud.UpdateAll();

            _Mecha._FStage.UpdateAll();

            Task.WaitAll(Task.Delay(1000));
        }
    }
}
