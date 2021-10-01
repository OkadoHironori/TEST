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
	///* システム　　： 産業用ＣＴスキャナ TOSCANER-20000AV Ver8.0                  */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： frmBHCMessage.frm                                           */
	///* 処理概要　　： BHCファントム以外の画像を読み込んだ時に表示されるフォーム   */
	///* 注意事項　　： なし                                                        */
	///* -------------------------------------------------------------------------- */
	///* 適用計算機　： DOS/V PC                                                    */
	///* ＯＳ　　　　： WindowsXP (SP2)                                             */
	///* コンパイラ　： VB 6.0 (SP5)                                                */
	///* -------------------------------------------------------------------------- */
	///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
	///* V8.00     2007/03/07   (CATS)村田         新規作成                         */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2007                 */
	///* ************************************************************************** */
	public partial class frmBHCMessage : Form
	{

		internal Label[] lblInfBHC = null;
		internal Label[] lblPriod = null;

		private static frmBHCMessage _Instance = null;

		public frmBHCMessage()
		{
			InitializeComponent();

			lblInfBHC = new Label[] { null, lblInfBHC1, lblInfBHC2, lblInfBHC3, lblInfBHC4, lblInfBHC5, lblInfBHC6 };
			lblPriod = new Label[] { null, lblPriod1, lblPriod2, lblPriod3, lblPriod4, lblPriod5, lblPriod6 };
		}

		public static frmBHCMessage Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmBHCMessage();
				}

				return _Instance;
			}
		}


		//*******************************************************************************
		//機　　能： OKをクリックした時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.1 2007/04/16 (CATS)Ohkado    新規作成
		//*******************************************************************************
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
		//履　　歴： v8.1 2007/04/16 (CATS)Ohkado    新規作成
		//*******************************************************************************
		private void frmBHCMessage_Load(object sender, EventArgs e)
		{
			//ラベルの設定
			SetCaption();
		}


		//*******************************************************************************
		//機　　能： 表示ラベルの設定
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v8.1 2007/04/16 (CATS)Ohkado    新規作成
		//*******************************************************************************
		private void SetCaption()
		{
			int i = 0;

			cmdOK.Text = CTResources.LoadResString(StringTable.IDS_btnOK);
			this.Text = CTResources.LoadResString(17227);

			for (i = 1; i <= 6; i++)
			{
                //変更2015/01/20hata
                //lblInfBHC[i].Left = 127;
                lblInfBHC[i].Left = 95;
				lblInfBHC[i].TextAlign = ContentAlignment.TopRight;
			}

			//英語環境の場合，各コントロールに使用するフォントを Arial, サイズ10 にする
			//Rev20.01 変更 by長野 2015/05/19
            //if (modCT30K.IsEnglish) modCT30K.SetFontOnForm(this);
            if (modCT30K.IsEnglish) modCT30K.SetFontOnForm(this, "Arial", 8);

        }

	}
}
