using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CT30K
{
    public partial class frmCTMenu : Form
    {
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmCTMenu myForm = null;

        //扉電磁ロックプロパティ用定数
        public enum DoorStatusConstants
        {
            DoorOpened = 1,
            DoorClosed,
            DoorLocked
        }

        //扉電磁ロックプロパティ用変数
        private DoorStatusConstants myDoorStatus;



        public frmCTMenu()
        {
            InitializeComponent();
        }

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmCTMenu Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmCTMenu();
                }

                return myForm;
            }
        }
        #endregion



        public object ScanImageSize { get; set; }


        public DoorStatusConstants DoorStatus
        {
            get { return myDoorStatus; }
            set
            {
                //変更時のみ設定する
                if (value == myDoorStatus)
                {
                    return;
                }

                //内部変数に記憶
                myDoorStatus = value;

                //ツールバー上のボタンに反映
                #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
                //				Toolbar1.Buttons("DoorLock").Image = Choose(myDoorStatus, "CannotLock", "Unlocked", "Locked")
                #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                string[] names = new string[] { "CannotLock", "Unlocked", "Locked" };

                tsbtnDoorLock.Image = ImageList1.Images[names[(int)myDoorStatus - 1]];
            }
        }


    }
}
