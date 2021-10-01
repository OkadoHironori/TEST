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

	///* ************************************************************************** */
	///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver15.0              */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： frmImagePrint.frm                                           */
	///* 処理概要　　： 画面印刷フォーム                                            */
	///* 注意事項　　： マルチモニタの場合，印刷画面を指定できるようにした          */
	///* -------------------------------------------------------------------------- */
	///* 適用計算機　： DOS/V PC                                                    */
	///* ＯＳ　　　　： WindowsXP(SP2)                                              */
	///* コンパイラ　： VB 6.0                                                      */
	///* -------------------------------------------------------------------------- */
	///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
	///*                                                                            */
	///* V1.00       99/XX/XX    (TOSFEC) ????????   新規作成                       */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2009                 */
	///* ************************************************************************** */
	public partial class frmImagePrint : Form
	{
		//ダイアログ戻り値用変数：「印刷」ボタンがクリックされた場合Trueとする
		private bool IsOK = false;


		private static frmImagePrint _Instance = null;

		public frmImagePrint()
		{
			InitializeComponent();
		}

		public static frmImagePrint Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmImagePrint();
				}

				return _Instance;
			}
		}


		//*******************************************************************************
		//機　　能： 「メイン画面」チェックボックスクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2009/08/20 (SI1)間々田  新規作成
		//*******************************************************************************
		private void chk1stMonitor_CheckStateChanged(object sender, EventArgs e)
		{
			//チェックボックスがチェックされている場合に限り，「印刷」ボタンを使用可とする
			cmdPrint.Enabled = (chk1stMonitor.CheckState == CheckState.Checked) || (chk2ndMonitor.CheckState == CheckState.Checked);
		}


		//*******************************************************************************
		//機　　能： 「透視画面」チェックボックスクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2009/08/20 (SI1)間々田  新規作成
		//*******************************************************************************
		private void chk2ndMonitor_CheckStateChanged(object sender, EventArgs e)
		{
			//チェックボックスがチェックされている場合に限り，「印刷」ボタンを使用可とする
			cmdPrint.Enabled = (chk1stMonitor.CheckState == CheckState.Checked) || (chk2ndMonitor.CheckState == CheckState.Checked);
		}


		//*******************************************************************************
		//機　　能： 「キャンセル」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2009/08/20 (SI1)間々田  新規作成
		//*******************************************************************************
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			//非表示にする
			//変更2015/1/17hata_非表示のときにちらつくため
            //this.Hide();
            modCT30K.FormHide(this);

		}


		//*******************************************************************************
		//機　　能： 「印刷」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2009/08/20 (SI1)間々田  新規作成
		//*******************************************************************************
		private void cmdPrint_Click(object sender, EventArgs e)
		{
			//フラグをセット
			IsOK = true;

			//非表示にする
			//変更2015/1/17hata_非表示のときにちらつくため
            //this.Hide();
            modCT30K.FormHide(this);

		}


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

			//戻り値変数初期化
			IsOK = false;

			//v17.60 ストリングテーブル化　by長野 2011/05/25
			StringTable.LoadResStrings(this);

			//コントロール値の設定
			chk1stMonitor.CheckState = (modImgProc.IsPrint1stMonitor ? CheckState.Checked : CheckState.Unchecked);
			chk2ndMonitor.CheckState = (modImgProc.IsPrint2ndMonitor ? CheckState.Checked : CheckState.Unchecked);

			//モーダル表示
            //変更2014/12/22hata_dNet_オーナーフォームを指定する
            //this.ShowDialog();
            this.ShowDialog(frmCTMenu.Instance);

			//コントロール値の取得
			if (IsOK)
			{
				modImgProc.IsPrint1stMonitor = (chk1stMonitor.CheckState == CheckState.Checked);
				modImgProc.IsPrint2ndMonitor = (chk2ndMonitor.CheckState == CheckState.Checked);
			}

			//戻り値セット
			functionReturnValue = IsOK;

			//アンロード
			this.Close();
	
			return functionReturnValue;
		}

	}
}
