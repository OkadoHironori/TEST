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
using Itc.Common.TXEnum;

namespace MechaMaintCnt.MainPanel
{
    public partial class UC_MainPanel : UserControl
    {
        /// <summary>
        /// メッセージ送信
        /// </summary>
        public Action<CalProgress> ProcMes { private get; set; }
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
            FCD.ProcMes = (mes) => ProcMes(mes);
                       
            FDD.SendMes = (mes) => SendMes(mes);
            FDD.IsDoExecut = (val, cur, mes) =>IsDoExecutConfig(val, cur, mes);
            FDD.ProcMes = (mes) => ProcMes(mes);

            TBLY.SendMes = (mes) => SendMes(mes);
            TBLY.IsDoExecut = (val, cur, mes) => IsDoExecutConfig(val, cur, mes);
            TBLY.ProcMes = (mes) => ProcMes(mes);


            AUX.Dock = DockStyle.Fill;
            FCD.Dock = DockStyle.Fill;
            FDD.Dock = DockStyle.Fill;
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
        public void Create(IMechaMaint mechaMaint)
        {
            if (mechaMaint != null)
            {
                FCD.Inject(mechaMaint._FCD);
                FDD.Inject(mechaMaint._FDD);
                AUX.Inject(mechaMaint._AuxSel);
                TBLY.Inject(mechaMaint._TblY);//Add2020/12/16hata
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
