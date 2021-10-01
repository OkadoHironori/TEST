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
	public partial class frmComment : Form
	{

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		'クリックしたボタン
		Private Result As VbMsgBoxResult
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//クリックしたボタン
		private DialogResult Result;


		private static frmComment _Instance = null;

		public frmComment()
		{
			InitializeComponent();
		}

		public static frmComment Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmComment();
				}

				return _Instance;
			}
		}


		//*******************************************************************************
		//機　　能： キャンセルボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			//キャンセルボタンをクリックした
			Result = DialogResult.Cancel;

			//このフォームを消去
			this.Hide();
		}


		//*******************************************************************************
		//機　　能： ＯＫボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		private void cmdOk_Click(object sender, EventArgs e)
		{
			//ＯＫボタンをクリックした
			Result = DialogResult.OK;

			//このフォームを消去
			this.Hide();
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
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		private void frmComment_Load(object sender, EventArgs e)
		{
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);
		}


		//*******************************************************************************
		//機　　能： ダイアログ処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： Prompt          [I/ ] String    プロンプト文字列
		//           Title           [I/ ] String    タイトルバーに表示する文字列
		//           Default         [I/ ] String    デフォルトのコメント文字列
		//           MaxLength       [I/ ] Long      入力できる最大文字数
		//戻 り 値：                 [ /O] String    入力されたコメント文字列
		//
		//補　　足： キャンセル時は vbNullChar を返す
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		public string Dialog(string Prompt, string Title = "", string Default = "", int MaxLength = 256)
		{
			string functionReturnValue = null;

			//プロンプト文字列
			lblPrompt.Text = Prompt;

			//タイトルバーに表示する文字列
			this.Text = (string.IsNullOrEmpty(Title) ? Application.ProductName : Title);

			//デフォルトのコメント文字列
			txtComment.Text = Default;

			//入力できる最大文字数
			txtComment.MaxLength = MaxLength;

			//このフォームをモーダル表示
			this.ShowDialog();

			//戻り値セット
			functionReturnValue = (Result == DialogResult.Cancel ? "\0" : txtComment.Text.Trim());

			//このフォームのアンロード
			this.Close();

			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： 入力テキストボックス欄変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00 2008/11/01 (SS1)間々田   リニューアル
		//*******************************************************************************
		private void txtComment_TextChanged(object sender, EventArgs e)
		{
			//全角文字を２文字とみなした文字数チェック
			modCT30K.ChangeTextBox(txtComment);
		}
	}
}
