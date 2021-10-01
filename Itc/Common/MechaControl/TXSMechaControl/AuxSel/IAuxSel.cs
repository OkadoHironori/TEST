using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXSMechaControl.AuxSel
{
    public interface IAuxSel
    {
        /// <summary>
        /// オプション設定が変更された
        /// </summary>
        event EventHandler AUXChanged;
        /// <summary>
        /// ドアインターロック変更
        /// </summary>
        /// <param name="chk"></param>
        void ChangeDoorInterlock(bool chk, Action<string> mes);
        /// <summary>
        /// X線接触センサ取付チェック入力
        /// </summary>
        /// <param name="chk"></param>
        void ChangeXrayColiChecker(bool chk, Action<string> mes);
        /// <summary>
        /// 浜ホト干渉防止設定
        /// </summary>
        /// <param name="chk"></param>
        void ChangeXrayColiPrevent(bool chk, Action<string> mes);
    }
}
