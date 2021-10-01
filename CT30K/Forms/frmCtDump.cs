using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{
	///* ************************************************************************** */
	///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： Ct値表示                                                    */
	///* 処理概要　　： ??????????????????????????????                              */
	///* 注意事項　　： なし                                                        */
	///* -------------------------------------------------------------------------- */
	///* 適用計算機　： DOS/V PC                                                    */
	///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
	///* コンパイラ　： VB 6.0                                                      */
	///* -------------------------------------------------------------------------- */
	///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
	///*                                                                            */
	///* V1.00       99/XX/XX    (TOSFEC) ????????   新規作成                       */
	///*                                                                            */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
	///* ************************************************************************** */
	public partial class frmCtDump : Form
	{
		private Button[] cmdMove = null;

		private static frmCtDump _Instance = null;

		//********************************************************************************
		//  共通データ宣言
		//********************************************************************************
		//アンロードモード
		private int myUnloadMode = 0;

		//コントロールのインデクス値
		public enum MoveIndexType
		{
			IndexMoveUp,
			IndexMoveDown,
			IndexMoveLeft,
			IndexMoveRight
		}

		//ＣＴ画像フォーム
		private frmScanImage myScanImage;


		public frmCtDump()
		{
			InitializeComponent();

			cmdMove = new Button[] { cmdMove0, cmdMove1, cmdMove2, cmdMove3 };

			MSFlexGrid1.Rows.Add(24);
		}

		public static frmCtDump Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmCtDump();
				}

				return _Instance;
			}
		}


		//*************************************************************************************************
		//機　　能： フォームロード時の処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void frmCtDump_Load(object sender, EventArgs e)
		{
			//フォームの位置の設定
			modCT30K.SetPosNormalForm(this);

			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

			//v17.60 英語用レイアウト調整 by 長野 2011/05/25
			if (modCT30K.IsEnglish == true)
			{
				EnglishAdjustLayout();
			}

			//各コントロールの初期化
			InitControls();

			//ＣＴ画像フォームの参照設定
			myScanImage = frmScanImage.Instance;
			myScanImage.RoiChanged += new EventHandler(myScanImage_RoiChanged);

			//ROI制御スタート
			frmScanImage.Instance.ImageProc = frmScanImage.ImageProcType.RoiCTDump;
		}


		//*************************************************************************************************
		//機　　能： QueryUnload イベント処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)

			'アンロードモードの保持
			myUnloadMode = UnloadMode

		End Sub
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
		private void frmCtDump_FormClosing(object sender, FormClosingEventArgs e)		// 【C#コントロールで代用】
		{
			CloseReason UnloadMode = e.CloseReason;

			//アンロードモードの保持
			myUnloadMode = (int)UnloadMode;
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
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		Private Sub Form_Unload(Cancel As Integer)

			'ＣＴ画像フォームの参照破棄
			Set myScanImage = Nothing

			'プログラム終了要求によるアンロードの場合は以後の処理は行わない
			If Not (myUnloadMode = vbFormCode Or myUnloadMode = vbFormControlMenu) Then Exit Sub

			'ROI制御終了
			frmScanImage.ImageProc = RoiNone

		End Sub
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
		private void frmCtDump_FormClosed(object sender, FormClosedEventArgs e)			// 【C#コントロールで代用】
		{
			//ＣＴ画像フォームの参照破棄
            if (myScanImage != null) //追加201501/26hata_if文追加
            {
                myScanImage.RoiChanged -= myScanImage_RoiChanged;
                myScanImage = null;
            }

			//プログラム終了要求によるアンロードの場合は以後の処理は行わない
            //if (!((myUnloadMode == (int)CloseReason.ApplicationExitCall) || (myUnloadMode == (int)CloseReason.UserClosing))) return;
            if (myUnloadMode != (int)CloseReason.UserClosing) return;

			//ROI制御終了
			frmScanImage.Instance.ImageProc = frmScanImage.ImageProcType.RoiNone;
		}


		//*************************************************************************************************
		//機　　能： 各コントロールの初期化
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void InitControls()
		{
#region 【C#コントロールで代用】
/*
			Dim i As Integer

			'MSFlexGridの設定
			With MSFlexGrid1

				For i = 0 To .Cols - 1
					.ColWidth(i) = .Width / .Cols
				Next
				For i = 0 To .Rows - 1
					.RowHeight(i) = .Height / .Rows
				Next

				'英語環境の場合
				If IsEnglish Then
					.Font.Name = "Arial"
					.Font.SIZE = 8
				End If

			End With
*/
#endregion 【C#コントロールで代用】

			this.SuspendLayout();

			//MSFlexGridの設定
			MSFlexGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			foreach (DataGridViewRow row in MSFlexGrid1.Rows)
			{
                //2014/11/06hata キャストの修正
                row.Height = Convert.ToInt32((float)MSFlexGrid1.Height / (float)MSFlexGrid1.RowCount);
			}

			//英語環境の場合
			if (modCT30K.IsEnglish)
			{
				MSFlexGrid1.Font = new Font("Arial", 8F); ;
			}

			this.ResumeLayout(false);
		}


		//*************************************************************************************************
		//機　　能： 上下左右ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void cmdMove_Click(object sender, EventArgs e)
		{
			if (sender as Button == null) return;
			int Index = Array.IndexOf(cmdMove, sender);
			if (Index < 0) return;

			int xc = 0;
			int yc = 0;
			int xl = 0;
			int yl = 0;

			//ROI座標を取得
			DrawRoi.roi.GetRectangleShape2(1, ref xc, ref yc, ref xl, ref yl);

			//移動後の座標を計算
			switch (Index)
			{
                //2014/11/06hata キャストの修正
				case (int)MoveIndexType.IndexMoveLeft:
					xc = xc - Convert.ToInt32(cwneMove.Value);
					break;
				case (int)MoveIndexType.IndexMoveRight:
					xc = xc + Convert.ToInt32(cwneMove.Value);
					break;
				case (int)MoveIndexType.IndexMoveUp:
					yc = yc - Convert.ToInt32(cwneMove.Value);
					break;
				case (int)MoveIndexType.IndexMoveDown:
					yc = yc + Convert.ToInt32(cwneMove.Value);
					break;
				default:
					break;
			}

			//移動後のROI座標が適切であれば
			if (!DrawRoi.roi.SetRectangleShape2(1, xc, yc, xl, yl))
			{
				//メッセージ表示：表示する領域座標が範囲を超えています。
				MessageBox.Show(CTResources.LoadResString(9572), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}


		//*************************************************************************************************
		//機　　能： 「終了」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void cmdEnd_Click(object eventSender, EventArgs e)
		{
			//アンロード
			this.Close();
		}


		//*************************************************************************************************
		//機　　能： 更新処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		//*************************************************************************************************
		private void myScanImage_RoiChanged(object sender, EventArgs e)
		{
			int x1 = 0;
			int x2 = 0;
			int y1 = 0;
			int y2 = 0;
			int[] OutBuff = null;
			int X = 0;
			int Y = 0;
			int i = 0;

			//ROI座標を取得
			DrawRoi.roi.GetRectangleShape(1, ref x1, ref y1, ref x2, ref y2);

			OutBuff = new int[(x2 - x1) * (y2 - y1) + 1];

			//ＣＴ値取得処理
			if (!ImgProc.CtDump(x1, y1, x2, y2, OutBuff))
			{
				//メッセージ表示：ＣＴ値表示処理に失敗しました。
				MessageBox.Show(StringTable.GetResString(StringTable.IDS_WentWrong, StringTable.GetResString(StringTable.IDS_Processing, this.Text)), 
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}


#region 【C#コントロールで代用】
/*
			'取得したCT値をGridにセット
			With MSFlexGrid1

				.Redraw = False

				i = 0
				For Y = 0 To 24 - 1
					.Row = Y
					For X = 0 To 16 - 1
						.col = X
						.Text = CStr(OutBuff(i))
						i = i + 1
					Next
				Next

				.Redraw = True

			End With
*/
#endregion 【C#コントロールで代用】

			//取得したCT値をGridにセット
			MSFlexGrid1.SuspendLayout();

			i = 0;
			for (Y = 0; Y <= 24 - 1; Y++)
			{
				for (X = 0; X <= 16 - 1; X++)
				{
					MSFlexGrid1[X, Y].Value = OutBuff[i];
					i = i + 1;
				}
			}

			MSFlexGrid1.ResumeLayout(false);

			//表示ROI座標を表示
			lblCtDump.Text = StringTable.LoadResStringWithColon(StringTable.IDS_RoiCoordinate) + StringTable.FormatStr("(%1,%2)-(%3,%4)", x1, y1, x2, y2);
		}


// 【C#コントロールで代用】
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		'*************************************************************************************************
		'機　　能： 移動数入力時のエラー処理
		'
		'           変数名          [I/O] 型        内容
		'引　　数： なし
		'戻 り 値： なし
		'
		'補　　足： なし
		'
		'履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
		'*************************************************************************************************
		Private Sub cwneMove_Error(Number As Integer, Description As String, Scode As Long, Source As String, HelpFile As String, HelpContext As Long, CancelDisplay As Boolean)
    
			'コンポーネントワークス側のメッセージを表示しないようにする
			CancelDisplay = True

		End Sub
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版


		//*************************************************************************************************
		//機　　能： 英語用レイアウト調整
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v17.60  2011/05/25 (検S1)長野  新規作成
		//*************************************************************************************************
		private void EnglishAdjustLayout()
		{
			cwneMove.Left = cwneMove.Left + 60;
			//lblMove.Left = lblMove.Left + 60;
		    //Rev20.01 変更 by長野 2015/05/19
            lblMove.Left = lblMove.Left - 35;
        }


		// 【C#コントロールで代用】
		private void Shape1_Paint(object sender, PaintEventArgs e)
		{
			Shape1.BorderStyle = BorderStyle.None;
			e.Graphics.DrawRectangle(new Pen(Color.Lime, 1), 0, 0, 13, 17);
		}

	}
}
