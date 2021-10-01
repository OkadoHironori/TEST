using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
//
using CT30K.Common;
using CTAPI;
using TransImage;
//using CT30K.Controls;
//using CT30K.Modules;
using CT30K.Properties;

namespace CT30K
{
    public partial class frmStart : Form
	{
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmStart myForm = null;

        
        /// <summary>
		/// スタート画面
		/// </summary>
		public frmStart()
		{
			InitializeComponent();
		}

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmStart Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmStart();
                }

                return myForm;
            }
        }
        #endregion


		//*******************************************************************************
		//機　　能： メッセージの表示
		//
		//           変数名          [I/O] 型        内容
		//引　　数： msgStr          [I/ ] String    表示するメッセージ
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		public void Display(string msgStr = "")
		{
			if (!string.IsNullOrEmpty(msgStr))
			{
				lblMessage.Text = msgStr;
			}

			if (pgbProcessing.Value < 90)
			{
				pgbProcessing.Value = pgbProcessing.Value + 10;
			}

			this.Refresh();		//v9.7追加 by 間々田 2004-12-24

			//スキャン用じゃない場合はここで立ち上がり完了とする 'v17.53条件追加 byやまおか 2011/05/13
			if (CTSettings.scaninh.Data.mechacontrol != 0)
			{
				modCT30K.CT30kNowStartingFlg = false;		//v17.20追加 byやまおか 2010/09/16
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
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void frmStart_Load(object sender, EventArgs e)
		{
//			Dim sts As Long
//
//			'ウィンドウを常に最前面表示にする
//			sts = SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE Or SWP_NOSIZE)
            
			//製品名とバージョンの表示：バージョン表示はコモンを使用
			AssemblyTitleAttribute attri = (AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute));

			lblProductName.Text = attri.Title + "\r" + modLibrary.RemoveNull(CTSettings.t20kinf.Data.version.GetString());

            //BMPをリソースから取得  'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //int pict_id = 0;
            if ((CTSettings.scaninh.Data.avmode == 0))
            {
                //pict_id = 102;  //東芝 産業用ＣＴ
                this.BackgroundImage = Resources.TosIndustrialCT;
            }
            else
            {
                //pict_id = 101;  //東芝 マイクロＣＴ
                this.BackgroundImage = Resources.TosMicroCT;
            }
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;

            //装置名称を表示         'v18.00追加 byやまおか 2011/07/13
            int str_id = 0;
            if ((CTSettings.scaninh.Data.avmode == 0))
            {
                str_id = StringTable.IDS_TosIndustrialCT;   //東芝 産業用ＣＴ
            }
            else
            {
                str_id = StringTable.IDS_TosMicroCT;        //東芝 マイクロＣＴ
            }
            var _with1 = lblToscaner;
            _with1.Text = CTResources.LoadResString(str_id);
            //2014/11/07hata キャストの修正
            //_with1.SetBounds(((this.Width - _with1.Width) / 2), 0, 0, 0, System.Windows.Forms.BoundsSpecified.X);
            _with1.SetBounds(Convert.ToInt32((this.Width - _with1.Width) / 2F), 0, 0, 0, System.Windows.Forms.BoundsSpecified.X);


			//初期メッセージ
			lblMessage.Text = "Now Starting " + attri.Title;

			//プログレスバーの初期値は０
			pgbProcessing.Value = 0;

			//表示位置（中央よりやや上方に配置）
			Size screen = Screen.PrimaryScreen.Bounds.Size;
            //2014/11/07hata キャストの修正
            //this.Location = new Point((screen.Width - this.Width) / 2, (screen.Height - this.Height) / 2 - this.Height * 2 / 3);
            this.Location = new Point(Convert.ToInt32((screen.Width - this.Width) / 2F), Convert.ToInt32((screen.Height - this.Height) / 2F) - Convert.ToInt32(this.Height * 2F / 3F));
        }

		//*******************************************************************************
		//機　　能： フォームアンロード時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void frmStart_FormClosed(object sender, FormClosedEventArgs e)
		{
			pgbProcessing.Value = 100;
		}
	}
}
