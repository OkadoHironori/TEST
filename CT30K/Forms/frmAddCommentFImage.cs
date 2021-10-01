using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using CT30K.Modules;
using CTAPI;
using System.Resources;
using CT30K.Properties;
using TransImage;

namespace CT30K
{
    public partial class frmAddCommentFImage : Form
    {
        #region フィールド

        static private int myFontSize = 20;
        static private int myFontColor = 128;
        static private int myStrPos = 1;

        static private string myComment1 = "";
        static private string myComment2 = "";
        static private string myComment3 = "";
        static private string myComment4 = "";

        Pen PenYellow = new Pen(Color.Yellow,1);
        Pen PenCyan = new Pen(Color.Cyan, 1);
        Pen PenGreen = new Pen(Color.Green,1);

        #endregion 

        #region プロパティ
        static public int FontSize
        {
            get
            {
                return (myFontSize);
            }
            set
            {
                myFontSize = value;
            }
        }

        static public int FontColor
        {
            get
            {
                return (myFontColor);
            }
            set
            {
                myFontColor = value;
            }
        }

        static public int StrPos
        {
            get
            {
                return (myStrPos);
            }
            set
            {
                myStrPos = value;
            }
        }

        static public string Comment1
        {
            get
            {
                return (myComment1);
            }
            set
            {
                myComment1 = value;
            }
        }
        static public string Comment2
        {
            get
            {
                return (myComment2);
            }
            set
            {
                myComment2 = value;
            }
        }
        static public string Comment3
        {
            get
            {
                return (myComment3);
            }
            set
            {
                myComment3 = value;
            }
        }
        static public string Comment4
        {
            get
            {
                return (myComment4);
            }
            set
            {
                myComment4 = value;
            }
        }

        #endregion

        #region メンバ変数

        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmAddCommentFImage myForm = null;
        #endregion

        private TextBox[] textBox = null;

        public frmAddCommentFImage()
        {
            InitializeComponent();

            textBox = new TextBox[] {textBox1,textBox2,textBox3,textBox4};

        }

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmAddCommentFImage Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmAddCommentFImage();
                }

                return myForm;
            }
        }
        #endregion

        ////透視画像フォームへの参照
        public frmTransImage myTransImage;
     
        //*************************************************************************************************
        //機　　能： フォームロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V22.00  15/07/02   (検S1)長野      新規作成
        //*************************************************************************************************
        private void frmAddCommentFImage_Load(object sender, EventArgs e)
        {
            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            //Rev24.00 by長野 2016/04/05
            this.Text = CTResources.LoadResString(12816) + CTResources.LoadResString(22016);

            myTransImage = frmTransImage.Instance;		//透視画像
            //キャプチャーOnOffイベント
            myTransImage.CaptureOnOffChanged += new frmTransImage.CaptureOnOffChangedEventHandler(myTransImage_CaptureOnOffChanged);
            if (frmTransImage.Instance.CaptureOn == false)
            {
                nudFontSize.Enabled = true;
                nudFontColor.Enabled = true;
            }
            else
            {
                nudFontSize.Enabled = false;
                nudFontColor.Enabled = false;
            }

            // Mod Start 2018/08/24 M.Oyama 中国語対応
            ////Rev23.40 by長野 2016/04/05 / Rev23.14 英語版対応 by長野 2016/03/16
            //if (modCT30K.IsEnglish)
            //{
            //    this.Height = (int)(this.Height * 1.1f);
            //}
            this.Height = (int)(this.Height * 1.1f);
            // Mod End 2018/08/24

            //表示位置をメインの真ん中にもってくる
            int x = 0;
            int y = 0;
            this.StartPosition = FormStartPosition.Manual;
            x = Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2;
            y = Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2;
            this.Location = new Point(x, y);

            //frmTransImageに表示されている画像の縮小を表示
            pictureBox1.Image = frmTransImage.Instance.ctlTransImage.Picture;

            //Bitmap bmp = new Bitmap(frmTransImage.Instance.ctlTransImage.Width, frmTransImage.Instance.ctlTransImage.Height);

            //bmp = frmTransImage.Instance.ctlTransImage.Picture;

            //Graphics g = Graphics.FromImage(this.pictureBox1.Image);

            //Pen p = new Pen(Color.Yellow, 12);

            //g.DrawLine(p, 100, 500, 500, 700);
            
            //g.Dispose();


            //他フォームから参照できるように設定した内容をプロパティに入れておく
            textBox1.Text = myComment1;
            textBox2.Text = myComment2;
            textBox3.Text = myComment3;
            textBox4.Text = myComment4;
            nudFontColor.Value = (decimal)myFontColor;
            nudFontSize.Value = (decimal)myFontSize;
            switch (myStrPos)
            {
                case 1:

                    chkCommentPosTL.CheckState = CheckState.Checked;
                    break;

                case 2:

                    chkCommentPosTR.CheckState = CheckState.Checked;
                    break;

                case 3:

                    chkCommentPosBR.CheckState = CheckState.Checked;
                    break;

                case 4:

                    chkCommentPosBL.CheckState = CheckState.Checked;
                    break;

                default:

                    chkCommentPosBL.CheckState = CheckState.Checked;
                    break;
            }
        }
        //*************************************************************************************************
        //機　　能： フォームアンロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V22.00  15/07/02   (検S1)長野      新規作成
        //*************************************************************************************************
        private void frmAddCommentFImage_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.Close();
        }
        //*************************************************************************************************
        //機　　能： OKボタンクリック時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V22.00  15/07/02   (検S1)長野      新規作成
        //*************************************************************************************************
        private void btnOK_Click(object sender, EventArgs e)
        {
            //最初に文字列に"\\"が入っていないかチェック→UniCodeのため"\\"はバックスラッシュに変わるので禁止
            for (int cnt = 0; cnt < 4; cnt++)
            {
                if (textBox[cnt].Text.Contains("\\"))
                {
                    //メッセージ表示：
                    //   コメントに以下の禁止文字を使用しないでください。
                    //   \ / : * ? < > | " Space
                    MessageBox.Show(CTResources.LoadResString(22017) + "\r" + "\r"
                                    + "\\" , Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }
            //他フォームから参照できるように設定した内容をプロパティに入れておく
            myFontColor = (int)nudFontColor.Value;
            myFontSize = (int)nudFontSize.Value;
            myComment1 = textBox1.Text;
            myComment2 = textBox2.Text;
            myComment3 = textBox3.Text;
            myComment4 = textBox4.Text;
            if (chkCommentPosTL.CheckState == CheckState.Checked)
            {
                myStrPos = 1;
     
            }
            else if (chkCommentPosTR.CheckState == CheckState.Checked)
            {
                myStrPos = 2;
            }
            else if (chkCommentPosBR.CheckState == CheckState.Checked)
            {
                myStrPos = 3;
            }
            else if (chkCommentPosBL.CheckState == CheckState.Checked)
            {
                myStrPos = 4;
            }

            frmTransImageControl.Instance.AddCommentInit();

            if (frmTransImage.Instance.CaptureOn == false)
            {
                CTSettings.transImageControl.Update();
            }

            this.Close();
        }
        //*************************************************************************************************
        //機　　能： キャンセルボタンクリック時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V22.00  15/07/02   (検S1)長野      新規作成
        //*************************************************************************************************
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //*************************************************************************************************
        //機　　能： 表示位置用チェックボックスクリック時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V22.00  15/07/02   (検S1)長野      新規作成
        //*************************************************************************************************
        private void chkCommentPosTL_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCommentPosTL.CheckState == CheckState.Checked)
            {
                chkCommentPosTL.Enabled = false;
                chkCommentPosTR.CheckState = CheckState.Unchecked;
                chkCommentPosBR.CheckState = CheckState.Unchecked;
                chkCommentPosBL.CheckState = CheckState.Unchecked;
            }
            else
            {
                chkCommentPosTL.Enabled = true;
            }
        }

        private void chkCommentPosTR_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCommentPosTR.CheckState == CheckState.Checked)
            {
                chkCommentPosTR.Enabled = false;
                chkCommentPosTL.CheckState = CheckState.Unchecked;
                chkCommentPosBR.CheckState = CheckState.Unchecked;
                chkCommentPosBL.CheckState = CheckState.Unchecked;
            }
            else
            {
                chkCommentPosTR.Enabled = true;
            }
        }

        private void chkCommentPosBR_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCommentPosBR.CheckState == CheckState.Checked)
            {
                chkCommentPosBR.Enabled = false;
                chkCommentPosTL.CheckState = CheckState.Unchecked;
                chkCommentPosTR.CheckState = CheckState.Unchecked;
                chkCommentPosBL.CheckState = CheckState.Unchecked;
            }
            else
            {
                chkCommentPosBR.Enabled = true;
            }
        }

        private void chkCommentPosBL_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCommentPosBL.CheckState == CheckState.Checked)
            {
                chkCommentPosBL.Enabled = false;
                chkCommentPosTL.CheckState = CheckState.Unchecked;
                chkCommentPosTR.CheckState = CheckState.Unchecked;
                chkCommentPosBR.CheckState = CheckState.Unchecked;
            }
            else
            {
                chkCommentPosBL.Enabled = true;
            }
        }
        //*******************************************************************************
        //機　　能： キャプチャON・OFF変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v22.00 2015/07/06 (検S1)長野   リニューアル
        //*******************************************************************************
        private void myTransImage_CaptureOnOffChanged(bool IsOn)
        {

            if (!IsOn)
            {
                nudFontSize.Enabled = true;
                nudFontColor.Enabled = true;
            }
            else
            {
                nudFontSize.Enabled = false;
                nudFontColor.Enabled = false;
            }

        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            int Index = -1;
            for (int i = 0; i < textBox.Length; i++)
            {
                if(sender.Equals(textBox[i]))
                {
                    Index = i;
                }
            }
            if (Index != -1)
            {
                modCT30K.ChangeTextBoxNoMessageBox(textBox[Index]);
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(PenCyan, 64, 0, 64, 124);
            e.Graphics.DrawLine(PenGreen, 0, 64, 124, 64);
            e.Graphics.DrawLine(PenYellow, 0, 84, 124, 84);
            e.Graphics.DrawLine(PenYellow, 0, 44, 124, 44);
        }

  
    }
}
