using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;
namespace CT30K
{
    public partial class frmImagePrint : Form
    {
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmImagePrint myForm = null;

        public frmImagePrint()
        {
            InitializeComponent();
        }

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmImagePrint Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmImagePrint();
                }

                return myForm;
            }
        }
        #endregion   
    
        //*******************************************************************************
        //機　　能： ダイアログ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [ /O] Boolean   True:「印刷」ボタンがクリックされた
        //                                           False:「キャンセル」ボタンがクリックされた
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2009/08/20 (SI1)間々田  新規作成
        //*******************************************************************************
        public bool Dialog()
        {
            bool functionReturnValue = false;
            return functionReturnValue;
        }

    }
}
