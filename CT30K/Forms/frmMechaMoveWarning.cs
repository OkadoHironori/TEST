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
	public partial class frmMechaMoveWarning : Form
	{
		private bool IsOK = false;


		private static frmMechaMoveWarning _Instance = null;

		public frmMechaMoveWarning()
		{
			InitializeComponent();
		}

		public static frmMechaMoveWarning Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmMechaMoveWarning();
				}

				return _Instance;
			}
		}


		//*************************************************************************************************
		//機　　能：
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.0  2009/07/09 (SS1)間々田   新規作成
		//*************************************************************************************************
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			//ダイアログを消去
			//変更2015/1/17hata_非表示のときにちらつくため
            //this.Hide();
            modCT30K.FormHide(this);

		}


		//*************************************************************************************************
		//機　　能：
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.0  2009/07/09 (SS1)間々田   新規作成
		//*************************************************************************************************
		private void cmdContinue_Click(object sender, EventArgs e)
		{
			IsOK = true;

			//ダイアログを消去
			//変更2015/1/17hata_非表示のときにちらつくため
            //this.Hide();
            modCT30K.FormHide(this);

		}


		//*************************************************************************************************
		//機　　能：
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.0  2009/07/09 (SS1)間々田   新規作成
		//*************************************************************************************************
		private void frmMechaMoveWarning_Load(object sender, EventArgs e)
		{
			//内部変数初期化
			IsOK = false;

			//v17.60 ストリングテーブル化 by長野 2011/05/30
			StringTable.LoadResStrings(this);

			//v17.60 英語用レイアウト調整 by長野 2011/5/25
			if (modCT30K.IsEnglish)
			{
				EnglishAdjustLayout();
			}

			//プロンプト文字列の設定
			//    lblMessage.Caption = "指定したエリアを回転中心位置に移動できませんでした。" & vbCr & _
			//'                         "このまま続行することも可能です。" & vbCr & _
			//'                         "最適なスキャン位置でスキャンするためには、微調テーブルのＸ軸、Ｙ軸をリセットした後、スキャンしたいエリアがテーブルの中心付近になるようにワークをセットし直してください。"

			//    'ストリングテーブル化 'v17.60 by長野 2011/05/22
			//    lblMessage.Caption = LoadResString(20052) & vbCr & vbCr & _
			//'                         LoadResString(20053) & vbCr & vbCr & _
			//'                         LoadResString(20054)
			//ストリングテーブル修正 v17.61 by長野 2011/09/12
			lblMessage.Text = CTResources.LoadResString(20052) + "\r" 
							+ CTResources.LoadResString(20053) + "\r" 
							+ CTResources.LoadResString(20054);
		}


		//*************************************************************************************************
		//機　　能： モーダルダイアログ処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値：                 [ /O] Boolean   True:「続行」ボタンをクリックした
		//                                           True:「キャンセル」ボタンをクリックした
		//補　　足： なし
		//
		//履　　歴： v15.0  2009/07/09 (SS1)間々田   新規作成
		//*************************************************************************************************
		public bool Dialog()
		{
			bool functionReturnValue = false;

			//モーダル表示
            //変更2014/12/22hata_dNet_オーナーフォームを指定する
            //this.ShowDialog();
            this.ShowDialog(frmCTMenu.Instance);

			//戻り値セット
			functionReturnValue = IsOK;

			//フォームのアンロード
			this.Close();
			return functionReturnValue;
		}


		//*************************************************************************************************
		//機　　能： 英語用レイアウト調整
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v17.60  2011/05/25(検S１)長野   新規作成
		//*************************************************************************************************
		private void EnglishAdjustLayout()
		{
            //2014/11/07hata キャストの修正
            //lblMessage.Height = (int)(lblMessage.Height * 1.1);
            lblMessage.Height = Convert.ToInt32(lblMessage.Height * 1.1F);
        }
	}
}
