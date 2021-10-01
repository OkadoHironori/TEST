using Itc.Common.Basic;
using Itc.Common.FormObject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MechaMaintCnt
{
    public partial class FrmConfirm : Form
    {
        /// <summary>
        /// 実行可否
        /// </summary>
        public Action<ResBase> DoExcute { private get; set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FrmConfirm(NumBase nb,float cur, string mes)
        {

            InitializeComponent();

            this.Text = mes;

            UC_Conf.SetIniValue(nb.DeciPosi, nb.Max, nb.Min, nb.Increment, cur);

            UC_Conf.SetValue(nb.Cur,cur);

            UC_DoStop.DoExecute = () =>
            {
                DoExcute?.Invoke(new ResBase(true, UC_Conf.GetRes()));
                this.Close();
            };

            UC_DoStop.DoClose = () =>
            {
                DoExcute?.Invoke(new ResBase(false, UC_Conf.GetBeforeNum()));
                this.Close();
            };

        }
    }
}
