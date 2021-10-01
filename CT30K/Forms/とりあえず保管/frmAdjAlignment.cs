using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XrayCtrl;
//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{
	public partial class frmAdjAlignment : Form
	{

#if DebugOn											//デバッグ時は仮想Ｘ線制御とする by 間々田 2004/11/29
		private frmVirtualXrayControl MyXrayObj;
#else
		private clsTActiveX MyXrayObj;
#endif

		// VB6 でのコントロール配列の代替
		private Button[] cmdWUP = null;

		//
		// cwneFocusSetValue_ValueChanged イベントで使用する static フィールド
		//
		private static decimal cwneFocusSetValue_PreviousValue = 0;		// 【C#コントロールで代用】


		private static frmAdjAlignment _Instance = null;

		public frmAdjAlignment()
		{
			InitializeComponent();

			// VB6 でのコントロール配列の代替
			cmdWUP = new Button[] { cmdWUP0, cmdWUP1, cmdWUP2 };
		}

		public static frmAdjAlignment Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmAdjAlignment();
				}

				return _Instance;
			}
		}


		//*******************************************************************************
		//機　　能： フォーカス設定値変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		// Private Sub cwneFocusSetValue_ValueChanged(Value As Variant, PreviousValue As Variant, ByVal OutOfRange As Boolean)		// 【C#コントロールで代用】
		private void cwneFocusSetValue_ValueChanged(object sender, EventArgs e)
		{
			//フォーカスがF1以外の時
			if (MyXrayObj.Up_Focussize != 1)
			{
				//メッセージ表示：フォーカスがF1時以外は無効です。
				MessageBox.Show(CTResources.LoadResString(14299), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				cwneFocusSetValue.Value = cwneFocusSetValue_PreviousValue;
				return;
			}

			cwneFocusSetValue_PreviousValue = cwneFocusSetValue.Value;

			//Dim Val1 As Integer
			float Val1 = 0;								//変更 by 間々田 2004/09/27

			//F1モードのフォーカス値を自動的に決定する
			Val1 = (float)cwneFocusSetValue.Value;								//0～15000
			MyXrayObj.XrayOBJ_Set(Val1);
		}


		//*******************************************************************************
		//機　　能： ステップコンボボックスクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cboStep_Click(object sender, EventArgs e)
		{
			//選択したステップ値の設定をフォーカス設定値の増減ボタンのステップ値に反映させる
			cwneFocusSetValue.Increment = Convert.ToDecimal(cboStep.Items[cboStep.SelectedIndex]);
		}


		//*******************************************************************************
		//機　　能： 「自動取得」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdAutoGet_Click(object sender, EventArgs e)
		{
			//フォーカスがF1以外の時
			if (MyXrayObj.Up_Focussize != 1)
			{
				//メッセージ表示：フォーカスがF1時以外は無効です。
				MessageBox.Show(CTResources.LoadResString(14299), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			int Val1 = 0;
			//F1モードのフォーカス値を自動的に決定する
			Val1 = 1;			//実行
			MyXrayObj.XrayOST_Set(Val1);
		}


		//*******************************************************************************
		//機　　能： 「保存」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdSave_Click(object sender, EventArgs e)
		{
			//フォーカスがF1以外の時
			if (MyXrayObj.Up_Focussize != 1)
			{
				//メッセージ表示：フォーカスがF1時以外は無効です。
				MessageBox.Show(CTResources.LoadResString(14299), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			int Val1 = 0;

			//調整したフォーカス値を保存する
			Val1 = 1;				//実行
			MyXrayObj.XraySAV_Set(Val1);
			//Sleep (5000)    'v16.01追加 byやまおか 2010/02/25  'v16.20削除 byやまおか 2010/04/21
			//ファームウェアが修正されたので削除しても良いが、念のため2秒待つ
			//Sleep (2000)    'v16.20変更 byやまおか 2010/04/21
			modCT30K.PauseForDoEvents(1);	//1秒待つ  'v17.10変更 byやまおか 2010/09/01
		}


		//削除ここから by 間々田 2004/09/24 アライメント値は表示のみ。設定するものではない。
		//'
		//'   Ｘ方向のビームアライメント値変更時処理
		//'
		//Private Sub cwneAlignmentX_ValueChanged(Value As Variant, PreviousValue As Variant, ByVal OutOfRange As Boolean)
		//
		//    Dim Val1 As Integer
		//
		//    'Ｘ方向のビームアライメント値を変更する
		//    Val1 = Value              '0～±1000
		//    Call MyXrayObj.XrayOBX_Set(Val1)
		//
		//End Sub
		//
		//'
		//'   Ｙ方向のビームアライメント値変更時処理
		//'
		//Private Sub cwneAlignmentY_ValueChanged(Value As Variant, PreviousValue As Variant, ByVal OutOfRange As Boolean)
		//
		//    Dim Val1 As Integer
		//
		//    'Ｙ方向のビームアライメント値を変更する
		//    Val1 = Value              '0～±1000
		//    Call MyXrayObj.XrayOBY_Set(Val1)
		//
		//End Sub
		//削除ここまで by 間々田 2004/09/24 アライメント値は表示のみ。設定するものではない。


		//*******************************************************************************
		//機　　能： 「アライメント」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdAlignment_Click(object sender, EventArgs e)
		{
			int Val1 = 0;

			//Ｘ線OFF時にクリックした場合    追加 by 間々田 2004/09/24
			if (MyXrayObj.Up_X_On == 0)
			{
				//メッセージ表示：Ｘ線OFF時にはアライメントを実行できません。
				MessageBox.Show(CTResources.LoadResString(14298), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//ビームアライメントを実行する
			Val1 = 2;						//ADJ2を実行
			MyXrayObj.XrayADJ_Set(Val1);
		}


		//*******************************************************************************
		//機　　能： 「一括アライメント」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdAlignmentAll_Click(object sender, EventArgs e)
		{
			int Val1 = 0;

			//Ｘ線OFF時にクリックした場合    追加 by 間々田 2004/09/24
			if (MyXrayObj.Up_X_On == 0)
			{
				//メッセージ表示：Ｘ線OFF時にはアライメントを実行できません。
				MessageBox.Show(CTResources.LoadResString(14298), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//一括ビームアライメントを実行する
			Val1 = 1;						//実行
			MyXrayObj.XrayADA_Set(Val1);
		}


		//*******************************************************************************
		//機　　能： 「中断」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdStop_Click(object sender, EventArgs e)
		{
			int Val1 = 0;

			//アライメント・一括ビームアライメントを中断する
			Val1 = 1;						//実行
			MyXrayObj.XraySTP_Set(Val1);
		}


		//*******************************************************************************
		//機　　能： 「ウォームアップ」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdWarmup_Click(object sender, EventArgs e)
		{
			int Val1 = 0;

			//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
			//ビープ音を鳴らす(500ms間隔で5回)   'v17.00追加 byやまおか 2010/01/19
			//If scaninh.xrayon_beep = 0 Then SoundBeep 5, 500
			//    If scaninh.xrayon_beep = 0 Then PlayXrayOnWarningSound  'v17.00変更 byやまおか 2010/03/12
			//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

			//ウォームアップを実行する
			Val1 = 1;						//実行
			MyXrayObj.XrayWarmUp_Set(Val1);
		}


		//*******************************************************************************
		//機　　能： 「ウォームアップ」フレーム内ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V15.10 09/11/16   やまおか      新規作成
		//*******************************************************************************
		private void cmdWUP_Click(object sender, EventArgs e)
		{
			if (sender as Button == null) return;

			int Index = Array.IndexOf(cmdWUP, sender);

			//'ウォームアップを実行する
			//WUP_No = Index + 1     '1:WUP  2:WUP1  3:WUP2
			//Val1 = IIf(XrayType = XrayTypeHamaL10801, WUP_No, 1)
			//Call MyXrayObj.XrayWarmUp_Set(Val1)

			//v16.01/v17.00追加(ここから) byやまおか 2010/02/22
			int i = 0;

			//ボタンの色をいったん元に戻す
			for (i = 0; i <= 2; i++)
			{
				cmdWUP[i].BackColor = SystemColors.Control;
			}

			//ウォームアップの種類を選ぶ
			modXrayControl.WUP_No = Index + 1;  //1:WUP  2:WUP1  3:WUP2

			//選ばれたボタンの色を緑にする
			cmdWUP[modXrayControl.WUP_No - 1].BackColor = Color.Lime;
			//v16.01/v17.00追加(ここまで) byやまおか 2010/02/22
		}


		//*******************************************************************************
		//機　　能： 「リセット」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdReset_Click(object sender, EventArgs e)
		{
			int Val1 = 0;

			//過負荷保護機能をリセットする
			Val1 = 1;					//実行
			MyXrayObj.XrayRST_Set(Val1);
		}


		//*******************************************************************************
		//機　　能： 「閉じる」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdClose_Click(object sender, EventArgs e)
		{
			//フォームのアンロード
			this.Close();
		}


		//*******************************************************************************
		//機　　能： イベントルーチン
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//           V16.20 10/04/21   やまおか      FeinFocus.exe→XrayCtrl.exe
		//*******************************************************************************
		//Private Sub MyXrayObj_XrayStatus3ValueDisp(Val1 As FeinFocus.udtXrayStatus3ValueDisp)
		private void MyXrayObj_XrayStatus3ValueDisp(object sender, clsTActiveX.UdtXrayStatus3EventArgs e)
		{
			XrayCtrl.clsTActiveX.udtXrayStatus3ValueDisp Val1 = e.udtXrayStatus3ValueDisp;

			//txtAlignment.BackColor = IIf((.m_XrayStatusSAD = 2) Or (.m_XrayStatusSAD = 3), vbGreen, vbButtonFace)
			txtAlignment.BackColor = (Val1.m_XrayStatusSAD == 1 || Val1.m_XrayStatusSAD == 2 || Val1.m_XrayStatusSAD == 3) ? Color.Lime : SystemColors.Control;		//v17.02変更 byやまおか 2010/07/22
			txtAlignmentAll.BackColor = (Val1.m_XrayStatusSAD == 4) ? Color.Lime : SystemColors.Control;

			//中断ボタン：アライメントや一括アライメント実行中のみ使用可とする
			cmdStop.Enabled = (txtAlignment.BackColor == Color.Lime) || (txtAlignmentAll.BackColor == Color.Lime);
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
		private void frmAdjAlignment_Load(object sender, EventArgs e)
		{
			MyXrayObj = modXrayControl.XrayControl;
			MyXrayObj.XrayStatus3ValueDisp += new clsTActiveX.XrayStatus3ValueDispEventHandler(MyXrayObj_XrayStatus3ValueDisp);

			//フォームの位置         'v15.0削除 by 間々田 2009/04/01 中央に配置するようにデザイン時に設定
			//With frmXrayControl
			//    Me.Move .Left + .width, .Top
			//End With

			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			//LoadResStrings Me  'v15.10削除 byやまおか 2009/11/16

			//キャプションのセット
			SetCaption();			//v15.10追加 byやまおか 2009/11/16

			//各コントロールの初期化
			InitControls();			//v15.10追加 byやまおか 2009/11/16

			//ステップ値の初期化
			cboStep.SelectedIndex = 0;

			//タイマーによる表示をあらかじめ行なう
			tmrAdjAlignment_Timer(this, EventArgs.Empty);

			//フォーカス値の設定値の初期値はフィードバック値とする '追加 by 間々田 2004/09/24
			cwneFocusSetValue.Value = (decimal)MyXrayObj.Up_XrayStatusSOB;
		}


		//*******************************************************************************
		//機　　能： 各コントロールのキャプションに文字列をセットする
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.10  09/11/16    やまおか    新規作成
		//*******************************************************************************
		private void SetCaption()
		{
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

			//キャプションをセット
			this.Text = CTResources.LoadResString(14214);		//X線詳細
			//fraWarmup.Caption = "ウォームアップの種類"  'ウォームアップの種類   'v16.01変更 byやまおか 2010/02/25
			//ストリングテーブル化　'v17.60 by長野 2011/5/22
			fraWarmup.Text = CTResources.LoadResString(20000);

			//ウォームアップボタンのキャプションをセット
			//cmdWUP(0).Caption = "簡易"
			//ストリングテーブル化　'v17.60 by長野 2011/5/22
			cmdWUP[0].Text = CTResources.LoadResString(20001);
			cmdWUP[1].Text = "WUP1";	//v16.01変更 byやまおか 2010/02/25
			//cmdWUP(2).Caption = "通常"
			//ストリングテーブル化　'v17.60 by長野 2011/5/22
			cmdWUP[2].Text = CTResources.LoadResString(20002);

			//ウォームアップボタンのツールチップテキストをセット
			ToolTipText.SetToolTip(cmdWUP[0], "WUP");
			ToolTipText.SetToolTip(cmdWUP[1], "WUP1");
			ToolTipText.SetToolTip(cmdWUP[2], "WUP2");
		}


		//*******************************************************************************
		//機　　能： 各コントロールの初期化
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.10  09/11/16    やまおか    新規作成
		//*******************************************************************************
		private void InitControls()
		{
			//ウォームアップボタンの表示
			switch (modXrayControl.XrayType)
			{
				case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
					cmdWarmup.Visible = false;			//ウォームアップボタンを非表示
					cmdWUP[modXrayControl.WUP_No - 1].BackColor = Color.Lime;	//ウォームアップの種類を表示 'v16.01/v17.00追加 byやまおか 2010/02/22
					cmdAutoGet.Visible = false;			//焦点切り替えがないので自動取得ボタンを非表示 'v16.20追加 byやまおか 2010/04/16
					cwneFocusSetValue.Maximum = 23000;	//15000→23000 浜ホト230kVでは15000を超えるため  'v17.42変更 byやまおか 2010/11/30
					break;

				//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
				//'        Case XrayTypeHamaL9191
				//'            fraWarmup.Visible = False       'ウォームアップフレームを非表示
				//'            'ボタン位置調整
				//'            cmdWarmup.Top = fraWarmup.Top   'ウォームアップ
				//'            cmdReset.Top = fraWarmup.Top    'リセット
				//'            cmdClose.Top = fraWarmup.Top    '閉じる
				//'            Me.Height = fraWarmup.Top + 1000    'フォーム高さを縮める
				//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

				default:
					break;

			}
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
		private void frmAdjAlignment_FormClosed(object sender, FormClosedEventArgs e)
		{
			MyXrayObj.XrayStatus3ValueDisp -= MyXrayObj_XrayStatus3ValueDisp;
			MyXrayObj = null;

			//呼び出し元の詳細ボタンを使用可とする
			if (modLibrary.IsExistForm(frmXrayControl.Instance))
			{
				frmXrayControl.Instance.cmdDetail.Enabled = true;
			}
		}


		//*******************************************************************************
		//機　　能： １秒周期のタイマー処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void tmrAdjAlignment_Timer(object sender, EventArgs e)
		{
			//フォーカス値のフィードバック値を表示する
			lblFocusFeedBack.Text = Convert.ToString(MyXrayObj.Up_XrayStatusSOB);

			//アライメント値（Ｘ方向）                           '追加 by 間々田 2004/09/24
			lblAlignmentX.Text = Convert.ToString(MyXrayObj.Up_XrayStatusSBX);

			//アライメント値（Ｙ方向）                           '追加 by 間々田 2004/09/24
			lblAlignmentY.Text = Convert.ToString(MyXrayObj.Up_XrayStatusSBY);
		}

	}
}
