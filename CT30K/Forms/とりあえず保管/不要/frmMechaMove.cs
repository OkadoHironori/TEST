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
	public partial class frmMechaMove : Form
	{

		//戻り値用変数
		private bool IsOK = false;

		public enum MECHA_MOVE
		{
			MOVE_FINE_X,
			MOVE_FINE_Y,
			MOVE_FCD,
			MOVE_FID,
			MOVE_TABLE_Y,
			MOVE_TABLE_Z
		}

		//メカ制御画面への参照
		private frmMechaControl myMechaControl;

		//ＦＣＤ速度のバックアップ用
		private int YSpeedBack = 0;

		//画像断面指定時の自動スキャン位置移動モードか？
		private bool IsAutoScanPosMode = false;

		//画像断面指定時の自動スキャン位置移動モード時に使用する変数
		private int[] RoiCircle = new int[3];
		private float PosFx;
		private float PosFy;

		//透視ROI指定テーブル移動用変数 by 山影 09-07-17
		private modAutoPos.RECTROI myRoiRect;		//矩形ROI
		private modAutoPos.FineTable myOptFTPos;	//理想の微調テーブル位置
		private float myOptUD;						//理想の昇降位置
		private float myFod;						//FOD(mm)

		//透視画面指定時の自動スキャン位置移動モードか？
		private bool IsAutoScanPosTransMode = false;

		//メカ移動モード定数
		public enum MechaMoveModeConstants
		{
			MechaMoveMode_Ready,
			MechaMoveMode_AutoMove,
			MechaMoveMode_ManualMove,
			MechaMoveMode_Done
		}

		//メカ移動モード変数
		private MechaMoveModeConstants myMechaMoveMode;


#region 【C#コントロールで代用】

		public enum cwModeSwitch
		{
			WhenPressed,
			UntilReleased,
			Indicator
		}

		internal cwModeSwitch cwbtnMoveMode = cwModeSwitch.WhenPressed;

		internal Color cwbtnMoveOnColor = SystemColors.Control;
		internal Color cwbtnMoveOffColor = SystemColors.Control;

		private bool _cwbtnMoveValue = false;

		internal bool cwbtnMoveValue
		{
			get { return _cwbtnMoveValue; }
			set
			{
				if (_cwbtnMoveValue == value) return;	// 値に変化がなければなにもしない

				_cwbtnMoveValue = !_cwbtnMoveValue;

				if (_cwbtnMoveValue == true)
				{
					cwbtnMove.BackColor = cwbtnMoveOnColor;
				}
				else
				{
					cwbtnMove.BackColor = cwbtnMoveOffColor;
					cwbtnMove.UseVisualStyleBackColor = true;
				}

				cwbtnMove_ValueChanged();
			}
		}

#endregion

		private NumTextBox[] ntbMove = null;

		private static frmMechaMove _Instance = null;

		public frmMechaMove()
		{
			InitializeComponent();

			ntbMove = new NumTextBox[] { ntbMove0, ntbMove1, ntbMove2, ntbMove3, ntbMove4, ntbMove5 };
		}

		public static frmMechaMove Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmMechaMove();
				}

				return _Instance;
			}
		}


		//*************************************************************************************************
		//機　　能： メカ移動モードプロパティ（設定のみ）
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private MechaMoveModeConstants MechaMoveMode
		{
			set
			{
				//値を記憶
				myMechaMoveMode = value;

				switch (myMechaMoveMode)
				{

					case MechaMoveModeConstants.MechaMoveMode_Ready:

						//マウスポインタを元に戻す
						Cursor.Current = Cursors.Default;

						//「閉じる」ボタンを使用可にする
						cmdClose.Enabled = true;

						//移動ボタンの調整
						cwbtnMoveValue = false;
						cwbtnMove.Enabled = true;

						//初期プロンプト
						//lblPrompt.Caption = "よろしければ「移動」ボタンをクリックしてください。"
						lblPrompt.Text = Resources.LoadResString(20042);	//ストリングテーブル化 'v17.60 by 長野 2011/05/22
						break;

					case MechaMoveModeConstants.MechaMoveMode_AutoMove:

						//マウスポインタを矢印付き砂時計にする
						Cursor.Current = Cursors.AppStarting;

						//「閉じる」ボタンを使用不可にする
						cmdClose.Enabled = false;

						//移動ボタンの調整
						cwbtnMoveOnColor = Color.Lime;
						cwbtnMove.Enabled = false;
						break;

					//手動移動モード
					case MechaMoveModeConstants.MechaMoveMode_ManualMove:

						//マウスポインタを元に戻す
						Cursor.Current = Cursors.Default;

						//「閉じる」ボタンを使用可にする
						cmdClose.Enabled = true;

						//移動ボタンの調整
						//移動ボタンの調整
						#region 【C#コントロールで代用】
						/*
						'cwbtnMove.OnText = "手動移動"
						cwbtnMove.OnText = LoadResString(20043) 'ストリングテーブル化 'v17.60 by長野 2011/05/22
						'cwbtnMove.OffText = "手動移動"
						cwbtnMove.OffText = LoadResString(20043) 'ストリングテーブル化 'v17.60 by長野 2011/05/22
						 */
						#endregion 【C#コントロールで代用】
						cwbtnMove.Text = Resources.LoadResString(20043);
						cwbtnMoveMode = cwModeSwitch.UntilReleased;
						cwbtnMoveOnColor = Color.Lime;
						cwbtnMove.Visible = true;
						cwbtnMove.Enabled = true;
						cwbtnMoveValue = false;

						//手動移動プロンプト
						//lblPrompt.Caption = "指定されたFCD位置に移動するには試料テーブルとＸ線管が接触しないよう確認しつつ、「手動移動」ボタンを押し続けてください。"
						lblPrompt.Text = Resources.LoadResString(20044);		//ストリングテーブル化 'v17.60 by長野 2011/05/22

						//Ｘ線管干渉が制限されている場合、制限を解除する旨通知
						if ((modScaninh.scaninh.table_restriction == 0) & (!modSeqComm.MySeq.stsTableMovePermit))
						{
							//lblPrompt.Caption = lblPrompt.Caption & vbCr & _
							//'                   "（「手動移動」ボタンを押すと「Ｘ線管干渉制限」は解除されます。）"
							//ストリングテーブル化 'v17.60 by長野 2011/05/22
							lblPrompt.Text = lblPrompt.Text + "\r" + Resources.LoadResString(20045);
						}
						break;

					//移動完了
					case MechaMoveModeConstants.MechaMoveMode_Done:

						//マウスポインタを元に戻す
						Cursor.Current = Cursors.Default;

						//「閉じる」ボタンを使用可にする
						cmdClose.Enabled = true;

						//移動完了プロンプト
						//lblPrompt.Caption = "移動完了"     'v16.10変更 byやまおか 2001/03/17
						//lblPrompt.Caption = "移動完了" & vbCr & vbCr & "テーブルが回転してもＸ線管に接触しないことを確認してからスキャンスタートしてください。"
						lblPrompt.Text = Resources.LoadResString(20046) + "\r" + "\r" + Resources.LoadResString(20047);		//ストリングテーブル化 'v17.60 by長野 2011/05/22

						cwbtnMoveValue = false;
						cwbtnMove.Enabled = false;
						cwbtnMove.Visible = false;

						//戻り値用変数をセット
						IsOK = true;
						break;
				}
			}
		}


		//*************************************************************************************************
		//機　　能： 「閉じる」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void cmdClose_Click(object sender, EventArgs e)
		{
			//ダイアログを非表示にする
			this.Hide();
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
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void frmMechaMove_Load(object sender, EventArgs e)
		{
			//変数初期化
			IsOK = false;
			IsAutoScanPosMode = false;

			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

			ntbMove0.Caption = StringTable.GetResString(12131, modInfdef.AxisName[0]);
			ntbMove1.Caption = StringTable.GetResString(12131, modInfdef.AxisName[1]);
			ntbMove4.Caption = modInfdef.AxisName[1];

			//v17.60 英語用レイアウト調整 by長野 2011/05/25
			if (modCT30K.IsEnglish == true)
			{
				EnglishAdjustLayout();
			}

			//メカ制御画面への参照
			myMechaControl = frmMechaControl.Instance;

			myMechaControl.FXChanged += new EventHandler(myMechaControl_FXChanged);
			myMechaControl.FYChanged += new EventHandler(myMechaControl_FYChanged);
			myMechaControl.FCDChanged += new EventHandler(myMechaControl_FCDChanged);
			myMechaControl.FIDChanged += new EventHandler(myMechaControl_FIDChanged);
			myMechaControl.YChanged += new EventHandler(myMechaControl_YChanged);
			myMechaControl.UDPosChanged += new EventHandler(myMechaControl_UDPosChanged);

			//ＦＣＤ速度のバックアップ
			YSpeedBack = modSeqComm.MySeq.stsYSpeed;

			//タッチパネル操作のロックを追加 'v17.61 by 長野 2011/09/12
			modSeqComm.SeqBitWrite("PanelInhibit", true);		//(False → True)

			//メカ動作モードを初期化
			MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Ready;
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
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		private void frmMechaMove_FormClosed()
		{
			//ＦＣＤ速度を元に戻す
			if (!modSeqComm.SeqWordWrite("YSpeed", Convert.ToString(YSpeedBack), false)) return;
			Application.DoEvents();

			//タッチパネル操作のアンロックを追加 'v17.61 by 長野 2011/09/12
			modSeqComm.SeqBitWrite("PanelInhibit", false);		//(True → False)

			//メカ制御画面への参照破棄
			myMechaControl.FXChanged -= myMechaControl_FXChanged;
			myMechaControl.FYChanged -= myMechaControl_FYChanged;
			myMechaControl.FCDChanged -= myMechaControl_FCDChanged;
			myMechaControl.FIDChanged -= myMechaControl_FIDChanged;
			myMechaControl.YChanged -= myMechaControl_YChanged;
			myMechaControl.UDPosChanged -= myMechaControl_UDPosChanged;
			myMechaControl = null;
		}


		//*************************************************************************************************
		//機　　能： メカ移動処理用ダイアログを生成・表示
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		public bool MechaMove(decimal? FX = null, decimal? fy = null, float? FCD = null, float? Fid = null, decimal? y = null, decimal? z = null)
		{
			bool functionReturnValue = false;

			//このフォームを表示する必要があるか
			bool IsVisible = false;

			frmMechaControl frmMechaControl = frmMechaControl.Instance;

			//値をセット
			ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value = FX ?? frmMechaControl.ntbFTablePosX.Value;
			ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value = fy ?? frmMechaControl.ntbFTablePosY.Value;
			ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value = (decimal)(FCD ?? ScanCorrect.GVal_Fcd);
			ntbMove[(int)MECHA_MOVE.MOVE_FID].Value = (decimal)(Fid ?? ScanCorrect.GVal_Fid);
			ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value = y ?? frmMechaControl.ntbTableXPos.Value;
			ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Value = z ?? frmMechaControl.ntbUpDown.Value;

			myMechaControl_FXChanged(this, EventArgs.Empty);
			myMechaControl_FYChanged(this, EventArgs.Empty);
			myMechaControl_FCDChanged(this, EventArgs.Empty);
			myMechaControl_FIDChanged(this, EventArgs.Empty);
			myMechaControl_YChanged(this, EventArgs.Empty);
			myMechaControl_UDPosChanged(this, EventArgs.Empty);

			//コントロールの位置調整・移動不要の分は非表示
			int theTop = 0;
			theTop = ntbMove[ntbMove.GetLowerBound(0)].Top;

			int i = 0;
			for (i = ntbMove.GetLowerBound(0); i <= ntbMove.GetUpperBound(0); i++)
			{
				if (ntbMove[i].BackColor == Color.Lime)
				{
					ntbMove[i].Visible = false;
				}
				else
				{
					ntbMove[i].Visible = true;
					IsVisible = true;
					ntbMove[i].Top = theTop;
					theTop = theTop + ntbMove[i].Height;
				}
			}

			//デバッグ用：自動スキャン位置移動用移動第１段階ＦＣＤ・Ｙ軸座標を表示する
			if (modCT30K.IsTestMode)
			{
				ntbFCD1st.Visible = (ntbMove[(int)MECHA_MOVE.MOVE_FCD].BackColor != Color.Lime);
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//				ntbY1st.Visible = (ntbMove(MOVE_TABLE_Y).Top <> vbGreen)
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
				ntbY1st.Visible = (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Top * 15 != ColorTranslator.ToOle(Color.Lime));
				ntbFCD1st.Top = ntbMove[(int)MECHA_MOVE.MOVE_FCD].Top;
				ntbY1st.Top = ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Top;
			}

			//試料テーブルをＸ線管近くに移動する場合
			if (IsTryOverFCDLimit())
			{
				//現在のＦＣＤ位置が限界ＦＣＤ内にない場合
				if (ScanCorrect.GVal_Fcd > modGlobal.GVal_FcdLimit)
				{
					//            lblPrompt.Caption = lblPrompt.Caption & vbCr & vbCr & _
					//                               "注：試料テーブルとＸ線管が近接するため、FCD=" & CStr(GVal_FcdLimit) & _
					//                                "(mm)の位置でいったん停止します。"
					//ストリングテーブル化 'v17.60 by長野 2011/05/22
					lblPrompt.Text = lblPrompt.Text + "\r" + "\r" + StringTable.GetResString(20049, Convert.ToString(modGlobal.GVal_FcdLimit));
				}
				//現在のＦＣＤ位置が限界ＦＣＤ内にある場合：
				else
				{
					//           lblPrompt.Caption = lblPrompt.Caption & vbCr & vbCr & _
					//                               "注：試料テーブルとＸ線管が近接するため、指定されたFCD位置への移動は" & _
					//                                "手動で行ないます。"
					//ストリングテーブル化 'v17.60 by長野 2011/05/22
					lblPrompt.Text = lblPrompt.Text + "\r" + "\r" + Resources.LoadResString(20050);

					//            '手動移動モードに切り替え
					//            MechaMoveMode = MechaMoveMode_ManualMove
				}
			}

			//このフォームを表示する必要がある？
			if (IsVisible)
			{
				//すでに表示している場合抜ける       '追加 by 間々田 2009/07/09
				if (this.Visible) return functionReturnValue;

				//モーダル表示
				this.ShowDialog();
			}
			else
			{
				//'すでに移動済みの場合、その旨を表示する場合
				//If NoMoveMessage Then
				//    MsgBox "すでに所定の位置に移動済みです。", vbInformation
				//End If

				IsOK = true;
			}

			//戻り値セット
			functionReturnValue = IsOK;

			//このフォームのアンロード
			this.Close();

			return functionReturnValue;
		}


		//*************************************************************************************************
		//機　　能： 手動移動ボタン値変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2009/08/18 (SS1)間々田  新規作成
		//*************************************************************************************************
		private void cwbtnMove_ValueChanged()		// 【C#コントロールで代用】
		{
			//ノーマル移動の時は無視する
			if (cwbtnMoveMode == cwModeSwitch.WhenPressed) return;

			if (cwbtnMoveValue)
			{
				//Ｘ線管干渉制限を解除
				if ((modScaninh.scaninh.table_restriction == 0) && (!modSeqComm.MySeq.stsTableMovePermit))
				{
					modSeqComm.SeqBitWrite("TableMovePermit", true);
					Application.DoEvents();
				}

				//ＦＣＤ速度を一時的に変更する
				if (!modSeqComm.SeqWordWrite("YSpeed", (modMechapara.mechapara.fcd_speed[(int)frmMechaControl.SpeedConstants.SpeedSlow] * 10).ToString("0"), false)) return;
				Application.DoEvents();

				if (chkXYMove.CheckState == CheckState.Checked)
				{
					//移動X座標送信
					if (!modSeqComm.SeqWordWrite("XIndexPosition", Convert.ToString(ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value * 100), false)) return;
					Application.DoEvents();
				}

				//移動FCD座標送信
				if (!modSeqComm.SeqWordWrite("YIndexPosition", Convert.ToString(ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value * 10), false)) return;
				Application.DoEvents();

				if (chkXYMove.CheckState == CheckState.Checked)
				{
					//移動実行命令送信
					if (!modSeqComm.SeqBitWrite("XYindex", true, false)) return;
					Application.DoEvents();
				}
				else
				{
					//移動実行命令送信
					if (!modSeqComm.SeqBitWrite("YIndex", true, false)) return;
					Application.DoEvents();
				}
			}
			else
			{
				if (chkXYMove.CheckState == CheckState.Checked)
				{
					//ストップ指令
					modSeqComm.SeqBitWrite("XIndexStop", true, false);
					Application.DoEvents();
				}
				else
				{
					//ストップ指令
					modSeqComm.SeqBitWrite("YIndexStop", true, false);
					Application.DoEvents();
				}
			}
		}


		//*************************************************************************************************
		//機　　能： 移動ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2009/08/18 (SS1)間々田  新規作成
		//*************************************************************************************************
		private void cwbtnMove_MouseClick(object sender, MouseEventArgs e)		//【C#コントロールで代用】
		{
			#region 【C#コントロールで代用】

			if (!((cwbtnMoveMode == cwModeSwitch.WhenPressed && cwbtnMoveValue == true) ||
				(cwbtnMoveMode == cwModeSwitch.UntilReleased && cwbtnMoveValue == false)))
			{
				// cwModeSwitch.WhenPressedモードで、Buttonを離す前(cwbtnMoveValue = true) または
				// cwModeSwitch.UntilReleasedモードで、Buttonを離した後(cwbtnMoveValue = false) の時以外は
				// Clickイベントは発生しない
				return;
			}

			#endregion

			int err_sts = 0;
			float FCD1 = 0;
			float FCD2 = 0;
			float PosY1 = 0;
			float PosY2 = 0;

			//「手動移動」ボタンの時は無視する
			if (cwbtnMoveMode != cwModeSwitch.WhenPressed) return;		// 【C#コントロールで代用】

			//移動前動作チェック   'v16.10追加 byやまおか 2010/03/17
			if (!IsOkMove()) return;

			frmMechaControl frmMechaControl = frmMechaControl.Instance;

			//ＦＣＤ速度を一時的に変更する   'v16.10追加 byやまおか 2010/03/17
			if (!modSeqComm.SeqWordWrite("YSpeed", (modMechapara.mechapara.fcd_speed[(int)frmMechaControl.SpeedConstants.SpeedFast] * 10).ToString("0"), false)) return;
			Application.DoEvents();

			//自動移動モードにする
			MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_AutoMove;

			string buf = null;
			buf = "";

			//微調Ｘ軸
			if (ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].BackColor != Color.Lime)
			{
				if (modMechaControl.MecaYStgIndex((float)ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value) != 0)
				{
					//エラーの場合：指定された微調X軸位置まで微調テーブルを移動させることができませんでした。
					buf = buf + "* " + StringTable.GetResString(StringTable.IDS_MoveErr, ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Caption, Resources.LoadResString(StringTable.IDS_FTable)) + "\r";
				}
			}

			//微調Ｙ軸
			if (ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].BackColor != Color.Lime)
			{
				if (modMechaControl.MecaXStgIndex((float)ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value) != 0)
				{
					//エラーの場合：指定された微調Y軸位置まで微調テーブルを移動させることができませんでした。
					buf = buf + "* " + StringTable.GetResString(StringTable.IDS_MoveErr, ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Caption, Resources.LoadResString(StringTable.IDS_FTable)) + "\r";
				}
			}

			//v19.12 windows7対策 タイマーによる更新が遅くなるため、更新処理をここで明示的に呼ぶ by長野 2013/03/08
			frmMechaControl.Update();
			frmMechaControl.UpdateMecha();

			//自動スキャン位置移動にて、微調テーブル移動でエラーが発生している場合
			if (IsAutoScanPosMode && (!string.IsNullOrEmpty(buf)))
			{
				//モードを元に戻す
				MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Ready;

				//専用のダイアログを表示
				if (frmMechaMoveWarning.Instance.Dialog())
				{
					//自動移動モードにする
					MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_AutoMove;

					//現在の微調テーブルの位置で最適な試料テーブルのXY座標を求める
					err_sts = ScanCorrect.auto_tbl_set(ref RoiCircle[0], 
														PosFy, PosFx,
														(float)frmMechaControl.ntbFTablePosY.Value, (float)frmMechaControl.ntbFTablePosX.Value, 
														ref FCD1, ref PosY1, 
														ref FCD2, ref PosY2);
					if (err_sts != 0) goto ErrorHandler;

					//微調テーブルの移動目標値を現在の値にしておく
					ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value = frmMechaControl.ntbFTablePosX.Value;
					ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value = frmMechaControl.ntbFTablePosY.Value;
					ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].BackColor = Color.Lime;
					ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].BackColor = Color.Lime;

					//自動スキャン位置移動用移動第１段階ＦＣＤ・Ｙ軸座標を記憶
					ntbFCD1st.Value = (decimal)(FCD1 - modScancondpar.scancondpar.fcd_offset[modCT30K.GetFcdOffsetIndex()]);
					ntbY1st.Value = (decimal)PosY1;

					//自動スキャン位置移動用移動最終目標ＦＣＤ・Ｙ軸座標を記憶
					ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value = (decimal)(FCD2 - modScancondpar.scancondpar.fcd_offset[modCT30K.GetFcdOffsetIndex()]);
					ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value = (decimal)PosY2;

					//bufクリア
					buf = "";
				}
				else
				{
					//'キャンセルの場合、メカ移動ダイアログを非表示にして抜ける
					//Me.hide    'v17.10削除 hideしない byやまおか 2010/08/09

					return;
				}
			}

			//移動 ここから by山影
			//昇降
			if (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].BackColor != Color.Lime)
			{
				if (modMechaControl.MechaUdIndex((float)ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Value) != 0)
				{
					//エラーの場合：指定された昇降位置まで試料テーブルを移動させることができませんでした。
					buf = buf + "* " + StringTable.GetResString(StringTable.IDS_MoveErr, ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Caption, Resources.LoadResString(StringTable.IDS_SampleTable)) + "\r";
				}
			}

			//透視自動スキャン位置移動にて、微調テーブル移動でエラーが発生している場合 ここからby 山影
			if (IsAutoScanPosTransMode && (!string.IsNullOrEmpty(buf)))
			{
				//'モードを元に戻す
				//MechaMoveMode = MechaMoveMode_Ready    'v17.10削除 下記Elseへ移動 byやまおか 2010/08/09

				//専用のダイアログを表示
				//続行の場合
				if (frmMechaMoveWarning.Instance.Dialog())
				{
					//スキャン位置再計算
					if (!MechaMoveForAutoScanPos_TransReCalc())
					{
						goto ErrorHandler;
					}

					//bufクリア
					buf = "";
				}
				else
				{
					//モードを元に戻す   'v17.10追加 ここへ移動 byやまおか 2010/08/09
					MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Ready;

					//'キャンセルの場合、メカ移動ダイアログを非表示にして抜ける
					//Me.hide        'v17.10削除 hideしない byやまおか 2010/08/09

					return;
				}
			}
			//ここまで by 山影

			//FCDとY軸を同時に移動させる場合
			//If (chkXYMove.value = vbChecked) And (ntbMove(MOVE_TABLE_Y).BackColor <> vbGreen) And (ntbMove(MOVE_FCD).BackColor <> vbGreen) Then
			if (chkXYMove.CheckState == CheckState.Checked)
			{
				//        '試料テーブルを後退させる場合、I.I.を移動させてから試料テーブルを移動させる
				//        If (ntbMove(MOVE_FCD).value > GVal_Fcd) Then
				//
				//            'FID
				//            With ntbMove(MOVE_FID)
				//                If .BackColor <> vbGreen Then
				//                    '指定FID位置までI.I.を移動させる
				//                    If Not MoveFID(.value * 10) Then
				//                        'エラーの場合：指定されたFID位置までI.I.を移動させることができませんでした。
				//                        buf = buf & "* " & BuildResStr(IDS_MoveErr, IDS_FID, IDS_II) & vbCr
				//                    End If
				//                End If
				//            End With
				//
				//            '指定FCD位置、Y軸位置まで試料テーブルを移動させる
				//            If Not MoveXY(ntbMove(MOVE_FCD).value * 10, ntbMove(MOVE_TABLE_Y).value * 100) Then
				//                'エラーの場合：指定されたFCD位置まで試料テーブルを移動させることができませんでした。
				//                buf = buf & "* " & BuildResStr(IDS_MoveErr, IDS_FCD, IDS_SampleTable) & vbCr
				//            End If
				//
				//        Else

				if (!MoveFcdAndY((float)ntbFCD1st.Value, (float)ntbY1st.Value))
				{
					//エラーの場合：指定されたFCD/Y軸位置まで試料テーブルを移動させることができませんでした。
					//               buf = "* " & "指定されたFCD/Y軸位置まで試料テーブルを移動させることができませんでした。" & vbCr
					buf = "* " + Resources.LoadResString(20051) + "\r";		//ストリングテーブル化 'v17.60 by 長野 2011/05/22
				}

				if (!MoveFcdAndY((float)ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value, (float)ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value))
				{
					//エラーの場合：指定されたFCD/Y軸位置まで試料テーブルを移動させることができませんでした。
					//buf = "* " & "指定されたFCD/Y軸位置まで試料テーブルを移動させることができませんでした。" & vbCr
					buf = "* " + Resources.LoadResString(20051) + "\r";		//ストリングテーブル化 'v17.60 by 長野 2011/05/22
				}

				//            'FID
				//            With ntbMove(MOVE_FID)
				//                If .BackColor <> vbGreen Then
				//                    '指定FID位置までI.I.を移動させる
				//                    If Not MoveFID(.value * 10) Then
				//                        'エラーの場合：指定されたFID位置までI.I.を移動させることができませんでした。
				//                        buf = buf & "* " & BuildResStr(IDS_MoveErr, IDS_FID, IDS_II) & vbCr
				//                    End If
				//                End If
				//            End With
				//
				//        End If
			}
			else
			{
				//テーブルＹ軸移動：（従来の）Ｘ軸方向の移動
				if (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].BackColor != Color.Lime)
				{
					if (!modSeqComm.MoveXpos((int)ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value * 100))
					{
						//エラーの場合：指定されたY軸位置まで試料テーブルを移動させることができませんでした。
						buf = buf + "* " + StringTable.GetResString(StringTable.IDS_MoveErr, ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Caption, Resources.LoadResString(StringTable.IDS_SampleTable)) + "\r";
					}
				}

				//試料テーブルを後退させる場合、I.I.を移動させてから試料テーブルを移動させる
				if ((ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value > (decimal)ScanCorrect.GVal_Fcd))
				{
					//FID
					if (ntbMove[(int)MECHA_MOVE.MOVE_FID].BackColor != Color.Lime)
					{
						//指定FID位置までI.I.を移動させる
						if (!modSeqComm.MoveFID((int)ntbMove[(int)MECHA_MOVE.MOVE_FID].Value * 10))
						{
							//エラーの場合：指定されたFID位置までI.I.を移動させることができませんでした。
							buf = buf + "* " + StringTable.BuildResStr(StringTable.IDS_MoveErr, StringTable.IDS_FID, StringTable.IDS_II) + "\r";
						}
					}

					//FCD
					if (ntbMove[(int)MECHA_MOVE.MOVE_FCD].BackColor != Color.Lime)
					{
						//指定FCD位置まで試料テーブルを移動させる
						if (!modSeqComm.MoveFCD((int)ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value * 10))
						{
							//エラーの場合：指定されたFCD位置まで試料テーブルを移動させることができませんでした。
							buf = buf + "* " + StringTable.BuildResStr(StringTable.IDS_MoveErr, StringTable.IDS_FCD, StringTable.IDS_SampleTable) + "\r";
						}
					}
				}
				else
				{
					//FCD
					if (ntbMove[(int)MECHA_MOVE.MOVE_FCD].BackColor != Color.Lime)
					{
						//指定FCD位置まで試料テーブルを移動させる
						if (!modSeqComm.MoveFCD((int)modLibrary.MaxVal(modLibrary.MinVal(modGlobal.GVal_FcdLimit, ScanCorrect.GVal_Fcd), (float)ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value) * 10))
						{
							//エラーの場合：指定されたFCD位置まで試料テーブルを移動させることができませんでした。
							buf = buf + "* " + StringTable.BuildResStr(StringTable.IDS_MoveErr, StringTable.IDS_FCD, StringTable.IDS_SampleTable) + "\r";
						}
					}

					//FID
					if (ntbMove[(int)MECHA_MOVE.MOVE_FID].BackColor != Color.Lime)
					{
						//指定FID位置までI.I.を移動させる
						if (!modSeqComm.MoveFID((int)(ntbMove[(int)MECHA_MOVE.MOVE_FID].Value * 10)))
						{
							//エラーの場合：指定されたFID位置までI.I.を移動させることができませんでした。
							buf = buf + "* " + StringTable.BuildResStr(StringTable.IDS_MoveErr, StringTable.IDS_FID, StringTable.IDS_II) + "\r";
						}
					}
				}
			}

			//   移動 by 山影
			//    '昇降
			//    With ntbMove(MOVE_TABLE_Z)
			//        If .BackColor <> vbGreen Then
			//            If MechaUdIndex(.Value) <> 0 Then
			//                'エラーの場合：指定された昇降位置まで試料テーブルを移動させることができませんでした。
			//                buf = buf & "* " & GetResString(IDS_MoveErr, .Caption, LoadResString(IDS_SampleTable)) & vbCr
			//            End If
			//        End If
			//    End With

			//v19.12 windows7対策 タイマーによる更新が遅くなるため、更新処理をここで明示的に呼ぶ by長野 2013/03/08
			frmMechaControl.Update();
			frmMechaControl.UpdateMecha();

			//エラーメッセージが存在する場合
			if (!string.IsNullOrEmpty(buf))
			{
				//手動移動モードに切り替え
				MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Ready;

				//エラーメッセージ表示
				MessageBox.Show(buf, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			//テーブルを限界FCD値を越えてＸ線管に近づけようとするつもりか？
			else if (IsTryOverFCDLimit())
			{
				//手動移動モードに切り替え
				MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_ManualMove;
			}
			//それ以外の場合
			else	//v17.10追加 byやまおか 2010/08/20
			{
				MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Done;		//v17.10追加 byやまおか 2010/08/20
			}

			return;

//エラー時の扱い
ErrorHandler:

			//モードを元に戻す
			MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Ready;

			//メッセージの表示
			modCT30K.ErrMessage(err_sts);
		}


		//*************************************************************************************************
		//機　　能： 指定ＦＣＤ、Ｙ軸位置移動処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2009/08/18 (SS1)間々田  新規作成
		//*************************************************************************************************
		private bool MoveFcdAndY(float targetFCD, float targetY)
		{
			bool functionReturnValue = false;

			float dX = 0;
			float dY = 0;
			float dx1st = 0;
			float dy1st = 0;

			//移動予定量
			//v17.73 v18.00からの修正の反映（指定位置に動かなくなる場合があるバグなので反映しておく) by長野
			//dX = targetFCD - GVal_Fcd  'v18.00変更 byやまおか 2011/07/14
			//SingleとVariantの演算が正しく行われないため
			//いちど文字列に変換することで小数二桁に切り詰めるてから計算する
			dX = Convert.ToSingle(targetFCD.ToString("0.00")) - ScanCorrect.GVal_Fcd;
			dY = targetY - modSeqComm.MySeq.stsXPosition / 100;

			//限界FCD値を考慮した移動位置
			float FCD1st = 0;
			FCD1st = modLibrary.MaxVal(modLibrary.MinVal(modGlobal.GVal_FcdLimit, ScanCorrect.GVal_Fcd), targetFCD);

			//限界FCD値を考慮した移動量
			dx1st = FCD1st - ScanCorrect.GVal_Fcd;
			if (dX != 0)
			{
				dy1st = dY * dx1st / dX;
			}
			else
			{
				dy1st = dY;
			}

			//指定FCD位置、Y軸位置まで試料テーブルを移動させる
			functionReturnValue = modSeqComm.MoveXY((int)(FCD1st * 10), modSeqComm.MySeq.stsXPosition + (int)(dy1st * 100));

			//目標値に達したら該当するコントロールの背景を緑にする   'v17.10追加 byやまおか 2010/08/20
			ntbMove[(int)MECHA_MOVE.MOVE_FCD].BackColor = (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value == frmMechaControl.Instance.ntbFCD.Value ? Color.Lime : SystemColors.Control);
			ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].BackColor = (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value == frmMechaControl.Instance.ntbTableXPos.Value ? System.Drawing.Color.Lime : SystemColors.Control);
			
			return functionReturnValue;
		}


		//*************************************************************************************************
		//機　　能： テーブルを限界FCD値を越えてＸ線管に近づけようとするつもりか？
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2009/08/18 (SS1)間々田  新規作成
		//*************************************************************************************************
		private bool IsTryOverFCDLimit()
		{
			return (ntbMove[(int)MECHA_MOVE.MOVE_FCD].BackColor != Color.Lime) &&
				   (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value < (decimal)modGlobal.GVal_FcdLimit) &&
				   (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value < (decimal)ScanCorrect.GVal_Fcd);
		}


		//*************************************************************************************************
		//機　　能： 微調Ｘ軸位置変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2009/08/18 (SS1)間々田  新規作成
		//*************************************************************************************************
		private void myMechaControl_FXChanged(object sender, EventArgs e)
		{
			//目標値に達したら該当するコントロールの背景を緑にする
			ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].BackColor = (ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value == frmMechaControl.Instance.ntbFTablePosX.Value ? Color.Lime : SystemColors.Control);

			//移動がすべて完了した終了処理へ
			if (IsAllDone()) MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Done;
		}


		//*************************************************************************************************
		//機　　能： 微調Ｙ軸位置変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2009/08/18 (SS1)間々田  新規作成
		//*************************************************************************************************
		private void myMechaControl_FYChanged(object sender, EventArgs e)
		{
			//目標値に達したら該当するコントロールの背景を緑にする
			ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].BackColor = (ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value == frmMechaControl.Instance.ntbFTablePosY.Value ? Color.Lime : SystemColors.Control);

			//移動がすべて完了した終了処理へ
			if (IsAllDone()) MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Done;
		}


		//*************************************************************************************************
		//機　　能： ＦＣＤ位置変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2009/08/18 (SS1)間々田  新規作成
		//*************************************************************************************************
		private void myMechaControl_FCDChanged(object sender, EventArgs e)
		{
			//目標値に達したら該当するコントロールの背景を緑にする
			ntbMove[(int)MECHA_MOVE.MOVE_FCD].BackColor = (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value == (decimal)ScanCorrect.GVal_Fcd ? Color.Lime : SystemColors.Control);

			//移動がすべて完了した終了処理へ
			if (IsAllDone()) MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Done;
		}


		//*************************************************************************************************
		//機　　能： ＦＩＤ位置変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2009/08/18 (SS1)間々田  新規作成
		//*************************************************************************************************
		private void myMechaControl_FIDChanged(object sender, EventArgs e)
		{
			//目標値に達したら該当するコントロールの背景を緑にする
			ntbMove[(int)MECHA_MOVE.MOVE_FID].BackColor = (ntbMove[(int)MECHA_MOVE.MOVE_FID].Value == (decimal)ScanCorrect.GVal_Fid ? Color.Lime : SystemColors.Control);

			//移動がすべて完了した終了処理へ
			if (IsAllDone()) MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Done;
		}


		//*************************************************************************************************
		//機　　能： Ｙ軸位置（従来のＸ軸位置）変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2009/08/18 (SS1)間々田  新規作成
		//*************************************************************************************************
		private void myMechaControl_YChanged(object sender, EventArgs e)
		{
			//目標値に達したら該当するコントロールの背景を緑にする
			ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].BackColor = (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value == frmMechaControl.Instance.ntbTableXPos.Value ? Color.Lime : SystemColors.Control);

			//移動がすべて完了した終了処理へ
			if (IsAllDone()) MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Done;
		}


		//*************************************************************************************************
		//機　　能： 昇降位置変更時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2009/08/18 (SS1)間々田  新規作成
		//*************************************************************************************************
		private void myMechaControl_UDPosChanged(object sender, EventArgs e)
		{
			//目標値に達したら該当するコントロールの背景を緑にする
			ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].BackColor =(ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Value == frmMechaControl.Instance.ntbUpDown.Value ? Color.Lime : SystemColors.Control);

			//移動がすべて完了した終了処理へ
			if (IsAllDone()) MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Done;
		}


		//*************************************************************************************************
		//機　　能： すべての移動が完了したか
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v15.00  2009/08/18 (SS1)間々田  新規作成
		//*************************************************************************************************
		private bool IsAllDone()
		{
			//戻り値初期化
			bool functionReturnValue = false;

			int i = 0;
			for (i = ntbMove.GetLowerBound(0); i <= ntbMove.GetUpperBound(0); i++)
			{
				if (ntbMove[i].BackColor == SystemColors.Control) return functionReturnValue;
			}

			//戻り値セット
			functionReturnValue = true;
			return functionReturnValue;
		}


		//*************************************************************************************************
		//機　　能： メカ移動処理用ダイアログを生成・表示（自動スキャン位置移動用）
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		public bool MechaMoveForAutoScanPos(int xc, int yc, int r)
		{
			bool functionReturnValue = false;

			int err_sts = 0;
			float FCD1 = 0;
			float FCD2 = 0;
			float PosY1 = 0;
			float PosY2 = 0;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			'フォームをロード
			Load Me
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//画像断面からの自動スキャン位置移動モードにする
			IsAutoScanPosMode = true;

			//Ｘ軸・Ｙ軸を同時に移動させる
			chkXYMove.CheckState = CheckState.Checked;

			//ROI設定
			RoiCircle[0] = xc;
			RoiCircle[1] = yc;
			RoiCircle[2] = r;

			//自動スキャン位置モード設定

			//最適な微調テーブル座標を求める
			err_sts = ScanCorrect.auto_ftbl_set(ref RoiCircle[0], ref PosFy, ref PosFx);
			if (err_sts != 0) goto ErrorHandler;

			ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value = (decimal)PosFx;
			ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value = (decimal)PosFy;

			//最適な微調テーブル座標に移動したと仮定して、最適な試料テーブルのXY座標を求める
			err_sts = ScanCorrect.auto_tbl_set(ref RoiCircle[0], 
												PosFy, PosFx, 
												(float)ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value, 
												(float)ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value, 
												ref FCD1, ref PosY1, 
												ref FCD2, ref PosY2);
			if (err_sts != 0) goto ErrorHandler;

			//自動スキャン位置移動用移動第１段階ＦＣＤ・Ｙ軸座標を記憶
			ntbFCD1st.Value = FCD1 - modScancondpar.scancondpar.fcd_offset[modCT30K.GetFcdOffsetIndex()];
			ntbY1st.Value = (decimal)PosY1;

			//移動処理（ここでモーダル表示）
			MechaMove(PosFx, PosFy, FCD2 - modScancondpar.scancondpar.fcd_offset[modCT30K.GetFcdOffsetIndex()], y: PosY2);

			//戻り値設定
			functionReturnValue = IsOK;
			return functionReturnValue;

//エラー時の扱い
ErrorHandler:
			//このフォームのアンロード
			this.Close();

			//メッセージの表示
			modCT30K.ErrMessage(err_sts);

			return functionReturnValue;
		}


		//'*******************************************************************************
		//'機　　能：AutoScanPosModeプロパティ（設定のみ）
		//'
		//'           変数名          [I/O] 型        内容
		//'引　　数： なし
		//'戻 り 値： なし
		//'
		//'補　　足： なし
		//'
		//'履　　歴： v15.00 2008/11/01 (SS1)間々田   新規作成
		//'*******************************************************************************
		//Private Property Let AutoScanPosMode(newValue As AutoScanPosModeConstants)
		//
		//    Dim err_sts  As Long
		//    Dim fcd1     As Single
		//    Dim fcd2     As Single
		//    Dim PosY1    As Single
		//    Dim PosY2    As Single
		//
		//    'モードを記憶
		//    myAutoScanPosMode = newValue
		//
		//    Select Case myAutoScanPosMode
		//
		//        '試料テーブル移動（その１）
		//        Case AutoScanPosMode_TableTryMove1st
		//
		//            '最適な微調テーブル座標を求める
		//            err_sts = auto_ftbl_set(RoiCircle(0), PosFy, PosFx)
		//            If err_sts <> 0 Then GoTo ErrorHandler
		//
		//            ntbMove(MOVE_FINE_X).value = PosFx
		//            ntbMove(MOVE_FINE_Y).value = PosFy
		//
		//            '最適な微調テーブル座標に移動したと仮定して、最適な試料テーブルのXY座標を求める
		//            err_sts = auto_tbl_set(RoiCircle(0), _
		//'                                   PosFy, PosFx, _
		//'                                   ntbMove(MOVE_FINE_Y).value, _
		//'                                   ntbMove(MOVE_FINE_X).value, _
		//'                                   fcd1, PosY1, _
		//'                                   fcd2, PosY2)
		//            If err_sts <> 0 Then GoTo ErrorHandler
		//
		//            '自動スキャン位置移動用移動第１段階ＦＣＤ・Ｙ軸座標を記憶
		//            ntbFCD1st.value = fcd1 - scancondpar.fcd_offset(GetFcdOffsetIndex())
		//            ntbY1st.value = PosY1
		//
		//            '移動
		//            MechaMove PosFx, PosFy, fcd2 - scancondpar.fcd_offset(GetFcdOffsetIndex()), , PosY2
		//
		//'        '試料テーブル移動（その１）
		//'        Case AutoScanPosMode_TableTryMove1st
		//'
		//'            'タイマーが実行できるようにイベントを取る
		//'            PauseForDoEvents 1#
		//'
		//'            '最適な試料テーブルのXY座標を求める
		//'            err_sts = auto_tbl_set(RoiCircle(0), _
		///                                   PosFy, PosFx, _
		///                                   frmMechaControl.ntbFTablePosY.value, _
		///                                   frmMechaControl.ntbFTablePosX.value, _
		///                                   fcd1, PosY1, _
		///                                   fcd2, PosY2)
		//'
		//'            If err_sts <> 0 Then GoTo ErrorHandler
		//'
		//'            '最適な試料テーブル移動（その１）
		//'            MechaMove , , fcd1 - scancondpar.fcd_offset(GetFcdOffsetIndex()), , PosY1
		//'            If Not cwbtnMove.Enabled Then cwbtnMove_Click
		//'
		//'        '試料テーブル移動（その２）
		//'        Case AutoScanPosMode_TableTryMove2nd
		//'
		//'            '最適な試料テーブル移動（その２）
		//'            MechaMove , , fcd2 - scancondpar.fcd_offset(GetFcdOffsetIndex()), , PosY2
		//'            If Not cwbtnMove.Enabled Then cwbtnMove_Click
		//
		//    End Select
		//
		//    Exit Property
		//
		//'エラー時の扱い
		//ErrorHandler:
		//    'メッセージの表示
		//    ErrMessage err_sts
		//
		//End Property

		//*************************************************************************************************
		//機　　能： メカ移動処理用ダイアログを生成・表示（自動スキャン位置移動用 ver透視）
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		public bool MechaMoveForAutoScanPos_Trans(int roiXC, int roiYC, int roiXL, int roiYL, short dPixel, float dRefPix, ref short[] beforeImage, ref short[] afterImage)
		{
			bool functionReturnValue = false;

			int err_sts = 0;

			modAutoPos.SIZE imageSize = default(modAutoPos.SIZE);						//透視画像サイズ
			modAutoPos.SampleTable curTablePos = default(modAutoPos.SampleTable);		//試料テーブル位置
			modAutoPos.FineTable curFTpos = default(modAutoPos.FineTable);				//微調テーブル位置
			modAutoPos.Detector_Info detectorInfo = default(modAutoPos.Detector_Info);	//検出器
			double rotationAngle = 0;													//テーブル回転角
			float rotationPos = 0;														//回転中心位置
			float scanpos = 0;															//スキャン位置
			int kv = 0;																	//ビニング

			//返り値用
			modAutoPos.SampleTable optTablePos1st = default(modAutoPos.SampleTable);
			modAutoPos.SampleTable optTablePos2nd = default(modAutoPos.SampleTable);

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			//フォームをロード
			Load(this);
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//透視画面からの自動スキャン位置移動モードにする
			IsAutoScanPosTransMode = true;

			//Ｘ軸・Ｙ軸を同時に移動させる
			chkXYMove.CheckState = CheckState.Checked;

			//ビニング計算
			kv = (int)(modCT30K.vm / modCT30K.hm);

			//ROI設定
			modAutoPos.RoiCoordinateTransform(roiXC, roiYC, roiXL, roiYL, ref myRoiRect);

			//画像サイズ
			imageSize.CX = ScanCorrect.h_size;
			imageSize.CY = ScanCorrect.v_size;
			//試料テーブル位置
			curTablePos.FCD = modScansel.scansel.FCD;
			curTablePos.lr = modMecainf.mecainf.table_x_pos;
			curTablePos.ud = modMecainf.mecainf.ud_pos;
			//微調テーブル位置
			curFTpos.x = modMecainf.mecainf.ystg_pos;
			curFTpos.y = modMecainf.mecainf.xstg_pos;
			//検出器
			detectorInfo.FDD = modScansel.scansel.Fid;
			detectorInfo.pitchH = 10 / modScancondpar.scancondpar.b[1];
			detectorInfo.pitchV = detectorInfo.pitchH * kv;
			detectorInfo.DetType = Convert.ToInt32(modGlobal.DetType);	//v16.20/v17.00 追加 by 山影 10-03-04

			//テーブル回転角
			rotationAngle = modMecainf.mecainf.rot_pos / 100.0;
			//回転中心位置
			string lblStatus3 = frmScanControl.Instance.lblStatus3.Text;
			if (lblStatus3 == StringTable.GC_STS_STANDBY_OK || 
				lblStatus3 == StringTable.GC_STS_NORMAL_OK || 
				lblStatus3 == StringTable.GC_STS_CONE_OK)
			{
				rotationPos = modScancondpar.scancondpar.xlc[2];
				//If IsLRInverse Then rotationPos = h_size - rotationPos - 1
				if (modGlobal.Use_FlatPanel) rotationPos = ScanCorrect.h_size - rotationPos - 1;	//v17.50変更 byやまおか 2011/02/27
			}
			else	//回転中心が求まっていない場合、VC内で計算して求める
			{
				rotationPos = -1;
			}

			//スキャン位置
			scanpos = modScancondpar.scancondpar.cone_scan_posi_a * imageSize.CX / 2 + modScancondpar.scancondpar.cone_scan_posi_b + imageSize.CY / 2;

			//最適な微調テーブル座標＆昇降位置を求める
			err_sts = modAutoPos.AutoFinetableSet_Trans(ref beforeImage[0], ref afterImage[0], ref imageSize, ref myRoiRect, 
														ref curFTpos, ref curTablePos, ref detectorInfo, rotationAngle, 
														rotationPos, scanpos, dPixel, dRefPix, ref myOptFTPos, ref myOptUD, ref myFod);
			if (err_sts != 0) goto ErrorHandler;

			ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value = (decimal)myOptFTPos.x;
			ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value = (decimal)myOptFTPos.y;
			ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Value = (decimal)myOptUD;


			//最適な微調テーブル座標＆昇降位置に移動したと仮定して、最適な試料テーブルのXY座標を求める
			curTablePos.ud = myOptUD;
			err_sts = modAutoPos.AutoTableSet_Trans(ref imageSize, ref myRoiRect, ref curTablePos, ref detectorInfo, 
													ref myOptFTPos, ref myOptFTPos, myOptUD, myFod, scanpos, ref optTablePos1st, ref optTablePos2nd);
			if (err_sts != 0) goto ErrorHandler;

			//自動スキャン位置移動用移動第１段階ＦＣＤ・Ｙ軸座標を記憶
			ntbFCD1st.Value = (decimal)(optTablePos1st.FCD - modScancondpar.scancondpar.fcd_offset[modCT30K.GetFcdOffsetIndex()]);
			ntbY1st.Value = (decimal)optTablePos1st.lr;

			//進捗表示をクリアする   'v17.10追加 byやまおか 2010/08/20
			frmRoiTool.Instance.lblProcess.Text = "";

			//移動処理（ここでモーダル表示）
			MechaMove(myOptFTPos.x, myOptFTPos.y, optTablePos2nd.FCD - modScancondpar.scancondpar.fcd_offset[modCT30K.GetFcdOffsetIndex()], y: optTablePos2nd.lr, z: myOptUD);

			//戻り値設定
			functionReturnValue = IsOK;
			return functionReturnValue;

//エラー時の扱い
ErrorHandler:
			//このフォームのアンロード
			this.Close();

			//メッセージの表示
			modCT30K.ErrMessage(err_sts);

			return functionReturnValue;
		}


		//*************************************************************************************************
		//機　　能： 透視自動スキャン位置 再計算
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*************************************************************************************************
		public bool MechaMoveForAutoScanPos_TransReCalc()
		{
			int err_sts = 0;

			//自動移動モードにする
			MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_AutoMove;

			//返り値初期化
			bool functionReturnValue = false;

			frmMechaControl frmMechaControl = frmMechaControl.Instance;

			//微調テーブル現在位置
			modAutoPos.FineTable curFTpos = default(modAutoPos.FineTable);
			curFTpos.x = (float)frmMechaControl.ntbFTablePosX.Value;
			curFTpos.y = (float)frmMechaControl.ntbFTablePosY.Value;

			modAutoPos.SIZE imageSize = default(modAutoPos.SIZE);						//透視画像サイズ
			modAutoPos.SampleTable curTablePos = default(modAutoPos.SampleTable);		//試料テーブル位置
			modAutoPos.Detector_Info detectorInfo = default(modAutoPos.Detector_Info);	//検出器
			double rotationAngle = 0;													//テーブル回転角
			float scanpos = 0;															//スキャン位置
			int kv = 0;																//ビニング

			//返り値用
			modAutoPos.SampleTable optTablePos1st = default(modAutoPos.SampleTable);
			modAutoPos.SampleTable optTablePos2nd = default(modAutoPos.SampleTable);

			//ビニング計算
			kv = (int)(modCT30K.vm / modCT30K.hm);

			//画像サイズ
			imageSize.CX = ScanCorrect.h_size;
			imageSize.CY = ScanCorrect.v_size;
			//試料テーブル位置
			curTablePos.FCD = modScansel.scansel.FCD;
			curTablePos.lr = modMecainf.mecainf.table_x_pos;
			curTablePos.ud = modMecainf.mecainf.ud_pos;
			//検出器
			detectorInfo.FDD = modScansel.scansel.Fid;
			detectorInfo.pitchH = 10 / modScancondpar.scancondpar.b[1];
			detectorInfo.pitchV = detectorInfo.pitchH * kv;
			detectorInfo.DetType = Convert.ToInt32(modGlobal.DetType);		//v16.20/v17.00 追加 by 山影 10-03-04

			//スキャン位置
			scanpos = modScancondpar.scancondpar.cone_scan_posi_a * imageSize.CX / 2 + modScancondpar.scancondpar.cone_scan_posi_b + imageSize.CY / 2;

			//現在の微調テーブルの位置で最適な試料テーブルのXY座標を求める
			err_sts = modAutoPos.AutoTableSet_Trans(ref imageSize, ref myRoiRect, ref curTablePos, ref detectorInfo, 
													ref curFTpos, ref myOptFTPos, myOptUD, myFod, scanpos, ref optTablePos1st, ref optTablePos2nd);
			if (err_sts != 0)
			{
				//メッセージの表示
				modCT30K.ErrMessage(err_sts);
				return functionReturnValue;
			}

			//微調テーブル＆昇降の移動目標値を現在の値にしておく
			ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value = frmMechaControl.ntbFTablePosX.Value;
			ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].BackColor = Color.Lime;

			ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value = frmMechaControl.ntbFTablePosY.Value;
			ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].BackColor = Color.Lime;

			ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Value = frmMechaControl.ntbUpDown.Value;
			ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].BackColor = Color.Lime;

			//自動スキャン位置移動用移動第１段階ＦＣＤ・Ｙ軸座標を記憶
			ntbFCD1st.Value = (decimal)(optTablePos1st.FCD - modScancondpar.scancondpar.fcd_offset[modCT30K.GetFcdOffsetIndex()]);
			ntbY1st.Value = (decimal)optTablePos1st.lr;

			//自動スキャン位置移動用移動最終目標ＦＣＤ・Ｙ軸座標を記憶
			ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value = (decimal)(optTablePos2nd.FCD - modScancondpar.scancondpar.fcd_offset[modCT30K.GetFcdOffsetIndex()]);
			ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value = (decimal)optTablePos2nd.lr;

			functionReturnValue = true;
			return functionReturnValue;
		}


		//*************************************************************************************************
		//機　　能： 位置移動時のチェック
		//
		//           変数名          [I/O] 型        内容
		//引　　数： TargetPos       [I/ ] Single    目標昇降位置
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v16.10 2010/03/17   (検SS)やまおか  新規作成
		//*************************************************************************************************
		private bool IsOkMove()
		{
			//戻り値初期化
			bool functionReturnValue = false;

			frmMechaControl frmMechaControl = frmMechaControl.Instance;

			//FCD（FCDオフセット含まず）がfcd_limitより小さい場合
			if (frmMechaControl.ntbFCD.Value < (decimal)modGlobal.GVal_FcdLimit)
			{
				//現在よりも高い位置へ上昇する場合
				if (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Value + 0.0001M < frmMechaControl.ntbUpDown.Value)
				{
					//確認メッセージ表示：（コモンによってメッセージを切り替える）
					//   リソース9510:試料テーブルが上昇しても、X線管にぶつからない位置にいることを確認して下さい。
					//   リソース9511:試料テーブルが上昇しますので、コリメータ／フィルタ等に衝突しないか確認して下さい。
					//   リソース9513:衝突しそうな場合は、安全な位置へ移動してから実行してください。
					//   リソース9905:よろしければＯＫをクリックしてください。
					DialogResult result = MessageBox.Show(Resources.LoadResString(9510) + "\r" + 
															Resources.LoadResString(9511) + "\r" + 
															Resources.LoadResString(9513) + "\r" + "\r" + 
															Resources.LoadResString(StringTable.IDS_ClickOK), 
														Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
					if (result == DialogResult.Cancel)
					{
						return functionReturnValue;
					}
				}
				//その他（下降もしくは動かない場合）
				else
				{
					//メッセージ表示：
					//   試料テーブルが自動的に最適位置に移動します。
					//   リソース9509:ワークがＸ線管などに衝突する恐れがありますので、移動先X,Y座標と試料の大きさを良く確認してください。
					//   リソース9513:衝突しそうな場合は、安全な位置へ移動してから実行してください。
					//   よろしければＯＫをクリックしてください。
					DialogResult result = MessageBox.Show(Resources.LoadResString(9509) + "\r" +
															Resources.LoadResString(9513) + "\r" + "\r" +
															Resources.LoadResString(StringTable.IDS_ClickOK),
														Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
					if (result == DialogResult.Cancel)
					{
						return functionReturnValue;
					}
				}
			}

			//戻り値セット
			functionReturnValue = true;
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
		//履　　歴： V17.60  11/05/25 （検S1）長野   新規作成
		//*************************************************************************************************
		private void EnglishAdjustLayout()
		{
#region 【C#コントロールで代用】
/*
			 cwbtnMove.OffText = LoadResString(12317)
			 cwbtnMove.OnText = LoadResString(12317)
*/
#endregion

			cwbtnMove.Text = Resources.LoadResString(12317);
		}


		#region 【C#コントロールで代用】

		private void cwbtnMove_MouseDown(object sender, MouseEventArgs e)
		{
			cwbtnMoveValue = true;
		}
		private void cwbtnMove_MouseUp(object sender, MouseEventArgs e)
		{
			cwbtnMoveValue = false;
			cwbtnMove_MouseClick(this, e);
		}

		private void cwbtnMove_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space) cwbtnMoveValue = true;
		}
		private void cwbtnMove_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space) cwbtnMoveValue = false;
		}

		#endregion

	}
}
