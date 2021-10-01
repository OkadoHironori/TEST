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
	public partial class frmQueryWarmup : Form
	{
		//戻り値用変数
		private bool Result;

		private static frmQueryWarmup _Instance = null;

		public frmQueryWarmup()
		{
			InitializeComponent();
		}

		public static frmQueryWarmup Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmQueryWarmup();
				}

				return _Instance;
			}
		}


		//*************************************************************************************************
		//機　　能： 「はい」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2009/07/22 (SS1)間々田   新規作成
		//*************************************************************************************************
		private void cmdYes_Click(object sender, EventArgs e)
		{
			//戻り値にTrueをセットして、非表示にする
			Result = true;
			//変更2015/1/17hata_非表示のときにちらつくため
            //this.Hide();
            modCT30K.FormHide(this);

		}


		//*************************************************************************************************
		//機　　能： 「いいえ」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2009/07/22 (SS1)間々田   新規作成
		//*************************************************************************************************
		private void cmdNo_Click(object sender, EventArgs e)
		{
			//戻り値にFalseをセットして、非表示にする
			Result = false;
			//変更2015/1/17hata_非表示のときにちらつくため
            //this.Hide();
            modCT30K.FormHide(this);

		}


		//*************************************************************************************************
		//機　　能： ダイアログ処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： VoltAtWarmup    [I/O] Single    設定管電圧
		//戻 り 値：                 [ /O] Boolean   True:「はい」がクリックされた
		//                                           False:「いいえ」がクリックされた
		//補　　足： なし
		//
		//履　　歴： v15.00 2009/07/22 (SS1)間々田   新規作成
		//*************************************************************************************************
		public bool Dialog(ref float VoltAtWarmup)
		{
			bool functionReturnValue = false;

            //v17.60 ストリングテーブル化 by長野 2011/05/25
            StringTable.LoadResStrings(this);

            //v17.60 英語用レイアウト調整 by 長野 2011/05/25
            if (modCT30K.IsEnglish == true)
            {
                EnglishAdjustLayout();
            }

            frmXrayControl myfrmXrayControl = frmXrayControl.Instance;

            //デフォルト値のセット
            //cwneWarmupSetVolt.Value = VoltAtWarmup
            modLibrary.CopyCWNumEdit(myfrmXrayControl.cwneWarmupSetVolt, this.cwneWarmupSetVolt);		// 【C#コントロールで代用】
            //変更2015/02/02hata_Max/Min範囲のチェック
            //cwneWarmupSetVolt.Value = (decimal)VoltAtWarmup;		//v15.11追加 byやまおか 2010/02/12
            cwneWarmupSetVolt.Value = modLibrary.CorrectInRange((decimal)VoltAtWarmup, cwneWarmupSetVolt.Minimum, cwneWarmupSetVolt.Maximum);
            
            //追加2014/11/26hata
            //サイズが変わってしまうので、ここで明示的に設定
            this.Size = new Size(343, 153);
            
            //真空ステータスに重ならないように表示位置を調整する 'v17.30追加 byやまおか 2010/09/24
            this.Top = myfrmXrayControl.Top + myfrmXrayControl.Height;
            //2014/11/07hata キャストの修正
            //this.Left = frmCTMenu.Instance.Width / 2 - this.Width / 2;
            this.Left = Convert.ToInt32(frmCTMenu.Instance.Width / 2F - this.Width / 2F);
            
			//モーダル表示
            //変更2014/12/22hata_dNet_オーナーフォームを指定する
            //this.ShowDialog();
            this.ShowDialog(frmCTMenu.Instance);

			//戻り値セット
			if (Result)
			{
				VoltAtWarmup = (float)cwneWarmupSetVolt.Value;
				functionReturnValue = Result;
			}

			//フォームアンロード
			this.Close();

			return functionReturnValue;
		}


		//*************************************************************************************************
		//機　　能： 英語用レイアウト調整
		//
		//           変数名          [I/O] 型        内容
		//引　　数： 　　　　　　    [I/O]
		//戻 り 値：                 [ /O]
		//
		//補　　足： なし
		//
		//履　　歴： v17.60 2011/05/25 (検S1)長野   新規作成
		//*************************************************************************************************
		private void EnglishAdjustLayout()
		{
			Label2.Visible = false;
			Label4.Visible = true;
			Label5.Visible = true;

			Label4.Left = Label1.Left + Label1.Width + 1;
			Label4.Top = 18;

            //Rev20.01 変更 by長野 2015/05/19
			//Label5.Left = cwneWarmupSetVolt.Left + cwneWarmupSetVolt.Width + 10;
            Label5.Left = cwneWarmupSetVolt.Left + cwneWarmupSetVolt.Width + 25;
            Label5.Top = 18;

			cwneWarmupSetVolt.Left = Label4.Left + Label4.Width + 1;
		}

	}
}
