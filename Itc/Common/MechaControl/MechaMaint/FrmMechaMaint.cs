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


namespace MechaMaintCnt
{
    public partial class FrmMechaMaint : Form
    {
        /// <summary>
        /// インスタンス
        /// </summary>
        private static FrmMechaMaint instance = null;
        public static FrmMechaMaint Instance
        {
            get { return instance; }
        }
        /// <summary>
        /// コントロール
        /// </summary>
        private readonly IMechaMaint _Mecha;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FrmMechaMaint(IFormMain main, IMechaMaint mecha)
        {
            this.ControlBox = false;

            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            this.AutoScaleMode = AutoScaleMode.None;

            this.ShowInTaskbar = false;

            _Mecha = mecha;
            _Mecha.LockStsChanged += (s, e) =>
            {
                var mm = s as MechaMaint;
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        UC_MainPanel.Enabled = mm.CanMove;
                    });
                }
                else
                {
                    UC_MainPanel.Enabled = mm.CanMove;
                }
            };

            _Mecha.StsChanged += (s,e)=>
            {
                var mm = s as MechaMaint;
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        UC_MainPanel.Enabled = mm.IsBesy ? false : true;
                        CloseCmd.Enabled = mm.IsBesy ? false : true;
                        UC_SelElement.Enabled = mm.IsBesy ? false : true;
                    });
                }
                else
                {
                    UC_MainPanel.Enabled = mm.IsBesy ? false : true;
                    CloseCmd.Enabled = mm.IsBesy ? false : true;
                    UC_SelElement.Enabled = mm.IsBesy ? false : true;
                }
            };

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

            //メッセージ更新
            UC_MainPanel.ProcMes = (mes) =>
            {
                if(InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        ProgMes.Text = mes.Status;
                    });
                }
                else
                {
                    ProgMes.Text = mes.Status;
                }
            };

            //メカ画面の変更
            UC_SelElement.Init();
            UC_SelElement.SelAxisArg += (s, e) =>
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
            _Mecha._AuxSel.UpdateAll();

            _Mecha._FCD.UpdateAll();

            _Mecha._FDD.UpdateAll();

            _Mecha._TblY.UpdateAll();

            UC_SelElement.EndInit();
        }
        /// <summary>
        /// 閉じるボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseCmd_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStop_Click(object sender, EventArgs e)
        {
            _Mecha.RequestStopAll();
        }
    }
}
