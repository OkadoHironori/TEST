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

namespace MechaMaintCnt.Selector
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
        public string SelOpt { get; private set; } = SelCont.Default.SelContent;
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
                case ("AUX"):
                    AUX.Checked = true;
                    break;
                case ("TBLY"):
                    TblY.Checked = true;
                    break;
                default:
                    AUX.Checked = true;
                    break;
                    //throw new Exception($"No develop {SelOpt}");
            }

            //_ChangedIgnore = false;
        }
        /// <summary>
        /// FDDクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FDD_CheckedChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore) { return; }
            
            SelOpt = FDD.Name;
            SelCont.Default.SelContent = SelOpt;
            SelCont.Default.Save();

            SelAxisArg?.Invoke(sender, new SelectChangeEventArgs(SelOpt));
        }
        /// <summary>
        /// FCDクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FCD_CheckedChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore) { return; }

            SelOpt = FCD.Name;
            SelCont.Default.SelContent = SelOpt;
            SelCont.Default.Save();

            SelAxisArg?.Invoke(sender, new SelectChangeEventArgs(SelOpt));
        }
        /// <summary>
        /// その他クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AUX_CheckedChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore) { return; }

            SelOpt = AUX.Name;
            SelCont.Default.SelContent = SelOpt;
            SelCont.Default.Save();

            SelAxisArg?.Invoke(sender, new SelectChangeEventArgs(SelOpt));
        }
        /// <summary>
        /// テーブルY軸クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TblY_CheckedChanged(object sender, EventArgs e)
        {
            if (_ChangedIgnore) { return; }

            SelOpt = TblY.Name;
            SelCont.Default.SelContent = SelOpt;
            SelCont.Default.Save();

            SelAxisArg?.Invoke(sender, new SelectChangeEventArgs(SelOpt));
        }
    }
}
