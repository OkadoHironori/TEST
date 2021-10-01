using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace CT30K
{
    /// <summary>
    /// TabControlのTabPage表示／非表示切り替えクラス
    /// </summary>
    public class TabPageCtrl
    {
        private class TabPageInfo
        {
            public TabPage Page;
            public bool Visible;
            
            public TabPageInfo(TabPage page, bool isVisible)
            {
                this.Page = page;
                this.Visible = isVisible;
            }
        }
        private TabPageInfo[] tabPageInfos;
        private TabControl tabControl;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tabCtrl">対象のTabControlオブジェクト</param>
        public TabPageCtrl(TabControl tabCtrl)
        {
            tabControl = tabCtrl;
            tabPageInfos = new TabPageInfo[tabControl.TabPages.Count];

            for (int i = 0; i < tabControl.TabPages.Count; i++)
            {
                tabPageInfos[i] = new TabPageInfo(tabControl.TabPages[i], true);
            }
        }

        /// <summary>
        /// TabPageを表示／非表示
        /// </summary>
        /// <param name="index">対象のTabPage番号</param>
        /// <param name="isVisible">true=表示, false=非表示</param>
        public void TabVisible(int pageNo, bool isVisible)
        {
            if (tabPageInfos[pageNo].Visible == isVisible)
            {
                return;
            }
            tabPageInfos[pageNo].Visible = isVisible;

            // コントロールのレイアウト
            tabControl.SuspendLayout();

            //変更2014/07/27(検S1)hata
            //TabPages全体をClearすると、表示がちらつくのでPageごとに表示/非表示を設定する
            //tabControl.TabPages.Clear();
            //for (int i = 0; i < tabPageInfos.Length; i++)
            //{
            //    if (tabPageInfos[i].Visible)
            //    {
            //        tabControl.TabPages.Add(tabPageInfos[i].Page);
            //    }
            //}
            for (int i = 0; i < tabPageInfos.Length; i++)
            {
                if (tabPageInfos[i].Visible)
                {
                    //表示
                    if (!tabControl.TabPages.ContainsKey(tabPageInfos[i].Page.Name))
                    {
                        tabControl.TabPages.Insert(i, tabPageInfos[i].Page);
                    }
                }
                else
                {
                    //非表示
                    if (tabControl.TabPages.ContainsKey(tabPageInfos[i].Page.Name))
                    {
                        tabControl.TabPages.Remove(tabPageInfos[i].Page);
                    }
                }
            }

            tabControl.ResumeLayout();
        }

    }
}
