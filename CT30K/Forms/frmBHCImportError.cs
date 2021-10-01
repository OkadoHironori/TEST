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
	public partial class frmBHCImportError : Form
	{

		private static frmBHCImportError _Instance = null;

		public frmBHCImportError()
		{
			InitializeComponent();
		}

		public static frmBHCImportError Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmBHCImportError();
				}

				return _Instance;
			}
		}


		//*******************************************************************************
		//機　　能： リストに追加する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： AddString()     [I/ ] String    読み込みできなかったファイルの名前
		//           AddCount        [I/ ] Integer   読み込みできなかったファイル数
		//           byThresholdAll  [I/ ] Boolean   真円率が閾値以上の値のﾌｧﾝﾄﾑ画像を強制的に取り込んだか?
		//戻 り 値： なし
		//
		//補　　足：

		//履　　歴： V8.0 2007/03/01  (CATS)村田            新規作成
		//           V8.1 2007/04/22  (CATS)Ohkado          byThresholdAllを削除
		//*******************************************************************************
		//Public Sub AddList(ByRef AddString() As String, ByVal AddCount As Integer, ByRef byThresholdAll As Boolean) v8.1削除
		public void AddList(string[] AddString, int AddCount)
		{
			int i = 0;

			//リストを空にする
			txtFileList.Text = "";

			for (i = 0; i <= AddCount; i++)
			{
				txtFileList.Text = txtFileList.Text + AddString[i] + "\r\n";
			}

			//If byThresholdAll Then                     'v8.1削除 by Ohkado 2007/04/22
			//    lblCheckBHCphantom.Visible = False
			//    lblPriod(4).Visible = False
			//End If
		}


		//*******************************************************************************
		//機　　能： OKボタンを押したときの処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数：
		//戻 り 値： なし
		//
		//補　　足：

		//履　　歴： V8.0 2007/03/01  (CATS)村田            新規作成
		//**************************************************************************
		private void cmdOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}


		//*******************************************************************************
		//機　　能： フォームロード時の処理（イベント処理）
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V8.00  2006/12/28   (CATS)Ohkado      新規作成
		//*******************************************************************************
		private void frmBHCImportError_Load(object sender, EventArgs e)
		{
			SetCaption();
		}


		//*******************************************************************************
		//機　　能： 各コントロールのキャプションをリソースからセットします
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.0  2006/12/28 (CATS)Ohkado      未完成
		//          v19.00 2012/03/14 H.Nagai           非シングルスキャン、管電圧不一致追加
		//*******************************************************************************
		private void SetCaption()
		{
			this.Text = CTResources.LoadResString(17543);														//

            lblattention.Text = CTResources.LoadResString(17213);												//以下の画像ファイルはBHCテーブルに追加できませんでした。
            lblConfirmTheFollowing.Text = CTResources.LoadResString(17214);									//以下の原因が考えられます。
			lblCheckRawdata.Text = StringTable.GetResString(StringTable.IDS_NotFound, StringTable.LoadResStringWithBeginLineLetter(10303));							//・生データファイルが見つかりません。
			lblCheckInffile.Text = StringTable.GetResString(StringTable.IDS_NotFound, StringTable.LoadResStringWithBeginLineLetter(StringTable.IDS_InfoFile));		//・付帯情報が見つかりません。
			lblCheckFileName.Text = StringTable.LoadResStringWithBeginLineLetter(17224);					//・同じ名前のファイルがあります。
			//v19.00 マイクロCTにはスキャノはなし
			//lblCheckScanoName.Caption = LoadResStringWithBeginLineLetter(17225)                     '・スキャノ画像の可能性があります。
			//lblCheckBHCphantom.Caption = LoadResString(17209) '円柱ファントムとして認識されませんでした. 'v8.1削除 by Ohkado 2007/04/22

			//v19.00 追加 ->
			lblCheckSingleScan.Text = StringTable.LoadResStringWithBeginLineLetter(21305);					//・シングルスキャン画像ではありません。
			lblCheckVolt.Text = StringTable.LoadResStringWithBeginLineLetter(21306);						//・管電圧が一致していません。
			lblCheckPdata.Text = StringTable.LoadResStringWithBeginLineLetter(21308);						//<- v19.00

            cmdOK.Text = CTResources.LoadResString(StringTable.IDS_btnOK);									//ＯＫ   ''ver8.30本間追加 2008/3/21


			//英語環境の場合，各コントロールに使用するフォントを Arial, サイズ10 にする
			if (modCT30K.IsEnglish) modCT30K.SetFontOnForm(this);
		}

	}
}
