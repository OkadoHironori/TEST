using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Itc.Common.FormObject;
using TXSMechaControl.FCD;
using Itc.Common.Basic;

namespace TXSMechaControl.MechaIntegrate
{
    public partial class UC_MainPanel : UserControl
    {
        /// <summary>
        /// メッセージ送信
        /// </summary>
        public Action<string> SendMes { private get; set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UC_MainPanel()
        {
            InitializeComponent();

            FCD.SendMes = (mes) => SendMes(mes);
            FCD.IsDoExecut = (val, cur, mes) => IsDoExecutConfig(val, cur, mes);

            FDD.SendMes = (mes) => SendMes(mes);
            FDD.IsDoExecut = (val, cur, mes) =>IsDoExecutConfig(val, cur, mes);
            

            ROT.SendMes = (mes) => SendMes(mes);
            ROT.IsDoExecut = (val, cur, mes) => IsDoExecutConfig(val, cur, mes);

            UPDOWN.SendMes = (mes) => SendMes(mes);
            UPDOWN.IsDoExecut = (val, cur, mes) => IsDoExecutConfig(val, cur, mes);

            FSTAGE.SendMes = (mes) => SendMes(mes);
            FSTAGE.IsDoExecut = (val, cur, mes) => IsDoExecutConfig(val, cur, mes);

            //Add2020/12/17hata
            TBLY.SendMes = (mes) => SendMes(mes);
            TBLY.IsDoExecut = (val, cur, mes) => IsDoExecutConfig(val, cur, mes);

            FCD.Dock = DockStyle.Fill;
            FDD.Dock = DockStyle.Fill;
            ROT.Dock = DockStyle.Fill;
            UPDOWN.Dock = DockStyle.Fill;
            FSTAGE.Dock = DockStyle.Fill;
            TBLY.Dock = DockStyle.Fill;

        }
        /// <summary>
        /// 確認メソッド
        /// </summary>
        /// <param name="nb"></param>
        /// <param name="cur"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        private ResBase IsDoExecutConfig(NumBase val, float cur, string mes)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    foreach (Control c in this.Controls) c.Enabled = false;
                });
                foreach (Control c in this.Controls) c.Enabled = false;
            }

            var resBase = new ResBase();
            using (FrmConfirm frm = new FrmConfirm(val, cur, mes))
            {
                frm.DoExcute = (b) =>
                {
                    resBase = b;
                };
                frm.ShowDialog();
            }

            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    foreach (Control c in this.Controls) c.Enabled = true;
                });
                foreach (Control c in this.Controls) c.Enabled = true;
            }

            return resBase;
        }

        /// <summary>
        /// メカ制御注入
        /// </summary>
        public void Create(IMechaIntegrate mecha)
        {
            if (mecha != null)
            {
                ROT.Inject(mecha);
                UPDOWN.Inject(mecha);
                FSTAGE.Inject(mecha);
                FDD.Inject(mecha);
                FCD.Inject(mecha);
                AUX.Inject(mecha);
                TBLY.Inject(mecha);//Add2020/12/16hata
            }
        }
        /// <summary>
        /// パネル変更
        /// </summary>
        public void ChangePanel(string dispanel)
        {
            if(InvokeRequired)
            {
                this.Invoke(new Action<string>(ChangePanel), dispanel);
                return;
            }

            //パネルの名前(Control.Name)とEnumの名前を同じにする必要がある
            foreach (Control c in this.Controls)
            { // thisはフォームを参照
                c.Visible = String.Compare(c.Name, dispanel, true) == 0 ? true : false;
            }
        }
    }
}
