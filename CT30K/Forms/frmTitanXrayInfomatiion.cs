using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CT30K.Properties;


namespace CT30K
{
    public partial class frmTitanXrayInfomation : Form
    {
        #region インスタンスを返すプロパティ

        // frmTitanXrayInfomatiionのインスタンス
        private static frmTitanXrayInfomation _Instance = null;

        /// <summary>
        ///frmTitanXrayInfomatiionのインスタンスを返す
        /// </summary>
        public static frmTitanXrayInfomation Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmTitanXrayInfomation();
                }

                return _Instance;
            }
        }

        #endregion


        #region コンストラクタ
        public frmTitanXrayInfomation()
        {
            InitializeComponent();
        }
        #endregion

        //*******************************************************************************
        //機　　能： フォームアンロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V19.50  14/02/03   (検S1)長野      新規作成
        //*******************************************************************************

        private void cmdClose_Click(System.Object eventSender, System.EventArgs eventArgs)
        {

            this.Close();

        }
        //*******************************************************************************
        //機　　能： エラーリセット時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V19.50  14/02/03   (検S1)長野      新規作成
        //*******************************************************************************

        private void cmdErrReset_Click(System.Object eventSender, System.EventArgs eventArgs)
        {

            modTitan.Ti_SetTitanReset();

            //少し待つ
            System.Threading.Thread.Sleep(100);       //v17.72/v19.02追加 byやまおか 2012/05/16

            //更新する
            cmdUpdate_Click(cmdUpdate, new System.EventArgs());

        }
        //*******************************************************************************
        //機　　能： 更新ボタンクリック時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V19.50  14/02/03   (検S1)長野      新規作成
        //*******************************************************************************

        private void cmdUpdate_Click(System.Object eventSender, System.EventArgs eventArgs)
        {

            int ret = 0;
            ret = modTitan.Ti_GetErrorCode();

            //Rev23.20 通信関係のエラーを追加 by長野 2016/01/11
            int comm_ret = 0;
            comm_ret = modTitan.Ti_GetCommErrorCode();

            //commのエラーを表示
            if (comm_ret == 0)
            {
                lblCommErrNoTxt.Text = CTResources.LoadResString(21358);
            }
            else
            {
                lblCommErrNoTxt.Text = comm_ret.ToString();
            }
            //番号と内容を表示
            //番号
            lblErrNoTxt.Text = Convert.ToString(ret);

            //内容
            switch (ret)
            {

                case 0:
                    //CTResources.LoadResString(20095)
                    lblErrContentsTxt.Text = CTResources.LoadResString(21358);
                    break;

                case 33:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21317);
                    break;

                case 35:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21318);
                    break;

                case 37:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21319);
                    break;

                case 38:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21320);
                    break;

                case 39:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21321);
                    break;

                case 41:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21322);
                    break;

                case 43:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21323);
                    break;

                case 44:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21324);
                    break;

                case 46:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21325);
                    break;

                case 47:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21326);
                    break;

                case 48:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21327);
                    break;

                case 49:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21328);
                    break;

                case 50:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21329);
                    break;

                case 51:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21330);
                    break;

                case 53:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21331);
                    break;

                case 57:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21332);
                    break;

                case 58:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21333);
                    break;

                case 61:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21334);
                    break;

                case 62:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21335);
                    break;

                case 63:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21336);
                    break;

                case 64:
                    
                    lblErrContentsTxt.Text = CTResources.LoadResString(21337);
                    break;

                case 65:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21338);
                    break;

                case 72:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21339);
                    break;

                case 80:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21340);
                    break;

                case 86:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21341);
                    break;

                case 87:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21342);
                    break;

                case 88:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21343);
                    break;

                case 92:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21344);
                    break;

                case 93:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21345);
                    break;

                case 94:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21346);
                    break;

                case 95:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21347);
                    break;

                case 103:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21348);
                    break;

                case 104:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21349);
                    break;

                case 105:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21350);
                    break;

                case 108:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21351);
                    break;

                case 110:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21352);
                    break;

                case 111:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21353);
                    break;

                case 113:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21354);
                    break;

                case 123:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21355);
                    break;

                case 126:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21356);
                    break;

                case 127:

                    lblErrContentsTxt.Text = CTResources.LoadResString(21357);
                    break;

            }

        }

        //*******************************************************************************
        //機　　能： フォームロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V19.50  14/02/03   (検S1)長野      新規作成
        //*******************************************************************************
        private void frmTitanXrayInfomation_Load(System.Object eventSender, System.EventArgs eventArgs)
        {

            //キャプションのセット
            StringTable.LoadResStrings(this);

            //情報の表示（「最新の情報に更新」ボタンクリック時と同じ処理）
            cmdUpdate_Click(cmdUpdate, new System.EventArgs());

        }


    }
}
