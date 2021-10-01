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
	public partial class frmXrayTool : Form
	{
		private RadioButton[] optForcedWarmup = null;


		private static frmXrayTool _Instance = null;

		public frmXrayTool()
		{
			InitializeComponent();

			optForcedWarmup = new RadioButton[] { optForcedWarmup0, optForcedWarmup1 };
		}

		public static frmXrayTool Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmXrayTool();
				}

				return _Instance;
			}
		}


		//*******************************************************************************
		//機　　能：「閉じる」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.3  2006/02/20  (SI3)間々田  新規作成
		//*******************************************************************************
		private void cmdClose_Click(object sender, EventArgs e)
		{
			//フォームのアンロード
			this.Close();
		}


		//*******************************************************************************
		//機　　能：強制ウォームアップ「開始」（または「停止」）ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.3  2006/02/20  (SI3)間々田  新規作成
		//*******************************************************************************
		private void cmdStartStop_Click(object sender, EventArgs e)
		{
			DialogResult Answer;

			//ボタンを使用不可にする
			cmdStartStop.Enabled = false;

			//開始
			if (cmdStartStop.Text == CTResources.LoadResString(StringTable.IDS_btnStart))
			{

				//電磁ロックが開の場合、自動的に電磁ロックを閉とする 'v15.0追加 by 間々田 2009/07/21
				if (frmCTMenu.Instance.DoorStatus == frmCTMenu.DoorStatusConstants.DoorClosed)
				{
					modSeqComm.SeqBitWrite("DoorLockOn", true);
					modCT30K.PauseForDoEvents(1);
				}

				//ダイアログ表示：
				//   最大管電圧までウォームアップしますか？
				//   「はい」をクリックすると最大管電圧まで、「いいえ」をクリックすると設定管電圧までウォームアップします。
				Answer = MessageBox.Show(CTResources.LoadResString(16106), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

				//キャンセルの場合、何もしない
				if (Answer == DialogResult.Cancel)
				{ }
				//電磁ロックが開の場合
				else if (!modXrayControl.IsXrayInterLock())
				{
					//MsgBox "電磁ロックが開なので、処理を続行できません。", vbCritical
					//v15.03変更 リソース化&メッセージ変更　by やまおか 2009/11/17
					if (!modSeqComm.MySeq.stsDoorLock && CTSettings.scaninh.Data.door_lock == 0)
					{
						//電磁ロックが開のため、ウォームアップ開始できません。
						MessageBox.Show(StringTable.GetResString(StringTable.IDS_CannotDo, 
																 CTResources.LoadResString(StringTable.IDS_MagLock), 
																 CTResources.LoadResString(StringTable.IDS_OpenOnly), 
																 CTResources.LoadResString(StringTable.IDS_WarmupStart)), 
										Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						//扉が開のため、ウォームアップ開始できません。
						MessageBox.Show(StringTable.GetResString(StringTable.IDS_CannotDo,
																 CTResources.LoadResString(StringTable.IDS_Door),
																 CTResources.LoadResString(StringTable.IDS_OpenOnly),
																 CTResources.LoadResString(StringTable.IDS_WarmupStart)), 
										Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				else
				{
					//最大管電圧までウォームアップする場合
					if (Answer == DialogResult.Yes)
					{
						//BackXrayVoltSet = XrayVoltSet
						modXrayControl.BackXrayVoltSet = (float)frmXrayControl.Instance.ntbSetVolt.Value;		//v11.5変更 by 間々田 2006/07/10
						//SetKVMA XrayMaxVolt
						//SetKVMA frmXrayControl.cwneKV.Maximum               'v11.5変更 by 間々田 2006/07/10
						modXrayControl.SetVolt((float)frmXrayControl.Instance.cwneKV.Maximum);					//v15.0変更 by 間々田 2009/04/07
					}

					//長期・短期の設定
					modSeqComm.SeqBitWrite((optForcedWarmup[0].Checked ? "EXMWUShort" : "EXMWULong"), true);

					//ウォームアップ未完了になるまで待つ
					//PauseForDoEvents 0.5
					modCT30K.PauseForDoEvents(2);

					//Ｘ線をオン
					modXrayControl.XrayOn();			//v11.4追加 by 間々田 2006/03/03
				}
			}
			//停止
			else
			{
				//SeqBitWrite "EXMWUNone", True
				modXrayControl.XrayOff();				//v11.4変更 by 間々田 2006/03/03
			}

			//最新の状態に画面を更新する
			//PauseForDoEvents 0.5
			modCT30K.PauseForDoEvents(2);
			MyUpdate();

			//ボタンを使用可にする
			cmdStartStop.Enabled = true;
		}


		//*******************************************************************************
		//機　　能： タイマーフレーム内「開始」（または「停止」）ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.4  2006/03/06  (SI3)間々田  新規作成
		//*******************************************************************************
		private void cmdTimerStartStop_Click(object sender, EventArgs e)
		{
			//ボタンを使用不可にする
			cmdTimerStartStop.Enabled = false;

			//開始
			if (cmdTimerStartStop.Text == CTResources.LoadResString(StringTable.IDS_btnStart))
			{
				//電磁ロックが開の場合、自動的に電磁ロックを閉とする 'v15.0追加 by 間々田 2009/07/21
				if (frmCTMenu.Instance.DoorStatus == frmCTMenu.DoorStatusConstants.DoorClosed)
				{
					modSeqComm.SeqBitWrite("DoorLockOn", true);
					modCT30K.PauseForDoEvents(1);
				}

				//電磁ロックが開の場合
				if (!modXrayControl.IsXrayInterLock())
				{
					//MsgBox "電磁ロックが開なので、処理を続行できません。", vbCritical
					//v15.03変更 リソース化&メッセージ変更　by やまおか 2009/11/17
					if (!modSeqComm.MySeq.stsDoorLock & CTSettings.scaninh.Data.door_lock == 0)
					{
						//電磁ロックが開のため、Ｘ線オンできません。
						MessageBox.Show(StringTable.GetResString(StringTable.IDS_CannotDo,
																 CTResources.LoadResString(StringTable.IDS_MagLock),
																 CTResources.LoadResString(StringTable.IDS_OpenOnly),
																 CTResources.LoadResString(StringTable.IDS_XrayON)),
										Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						//扉が開のため、Ｘ線オンできません。
						MessageBox.Show(StringTable.GetResString(StringTable.IDS_CannotDo,
																 CTResources.LoadResString(StringTable.IDS_Door),
																 CTResources.LoadResString(StringTable.IDS_OpenOnly),
																 CTResources.LoadResString(StringTable.IDS_XrayON)),
										Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				else
				{
                    //2014/11/07hata キャストの修正
                    //modXrayControl.XrayToolTimerCount = (int)(cwneTimerMM.Value * 60 + cwneTimerSS.Value);
                    modXrayControl.XrayToolTimerCount = Convert.ToInt32(cwneTimerMM.Value * 60 + cwneTimerSS.Value);
					if (modXrayControl.XrayToolTimerCount > 0)
					{
						modXrayControl.XrayOn();
						frmXrayControl.Instance.tmrXrayTool.Enabled = true;
					}
				}
			}
			//停止
			else
			{
				modXrayControl.XrayOff();
				frmXrayControl.Instance.tmrXrayTool.Enabled = false;
			}

			//最新の状態に画面を更新する
			MyUpdate();

			//ボタンを使用可にする
			cmdTimerStartStop.Enabled = true;
		}


		//*******************************************************************************
		//機　　能：「解除」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.4  2006/03/03  (SI3)間々田  新規作成
		//*******************************************************************************
		private void cmdWarmupNone_Click(object sender, EventArgs e)
		{
			modSeqComm.SeqBitWrite("EXMWUNone", true);
		}


		//*******************************************************************************
		//機　　能：フォームロード時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.3  2006/02/20  (SI3)間々田  新規作成
		//*******************************************************************************
		private void frmXrayTool_Load(object sender, EventArgs e)
		{
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

			//v17.60 英語用レイアウト調整 by長野 2011/06/03
			if (modCT30K.IsEnglish)
			{
				EnglishAdjustLayout();
			}

			//デフォルト値のコントロールへのセット
			SetControls();
		}


		//*******************************************************************************
		//機　　能：フォームアンロード時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.3  2006/02/20  (SI3)間々田  新規作成
		//*******************************************************************************
		private void frmXrayTool_FormClosed(object sender, FormClosedEventArgs e)
		{
			//設定値を記憶
			modCT30K.gForcedWarmup = modLibrary.GetOption(optForcedWarmup);
            //2014/11/07hata キャストの修正
            //modXrayControl.XrayToolTimerCount = (int)(cwneTimerMM.Value * 60 + cwneTimerSS.Value);	//追加 by 間々田 2006/03/10
            modXrayControl.XrayToolTimerCount = Convert.ToInt32(cwneTimerMM.Value * 60 + cwneTimerSS.Value);	//追加 by 間々田 2006/03/10
        }


		//*******************************************************************************
		//機　　能：コントロールへの値のセット
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.3  2006/02/20  (SI3)間々田  新規作成
		//*******************************************************************************
		private void SetControls()
		{
			//オプションボタンのセット（短期か長期か）
			modLibrary.SetOption(optForcedWarmup, modCT30K.gForcedWarmup);

			//最新の状態に画面を更新する
			MyUpdate();
		}


		//*******************************************************************************
		//機　　能： 最新の状態に画面を更新する
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.3  2006/02/20  (SI3)間々田  新規作成
		//*******************************************************************************
		public void MyUpdate()
		{
			frmXrayControl frmXrayControl = frmXrayControl.Instance;

			//強制ウォームアップオプションボタン
			//optForcedWarmup(0).Enabled = Not IsXrayOn
			//optForcedWarmup(1).Enabled = Not IsXrayOn

			//v11.5変更 by 間々田 2006/06/21
			optForcedWarmup[0].Enabled = (frmXrayControl.MecaXrayOn == modCT30K.OnOffStatusConstants.OffStatus);
			optForcedWarmup[1].Enabled = (frmXrayControl.MecaXrayOn == modCT30K.OnOffStatusConstants.OffStatus);

			//強制ウォームアップ開始・停止ボタン
			//cmdStartStop.Caption = LoadResString(IIf(IsXrayOn, IDS_btnStop, IDS_btnStart)) '停止/開始
			cmdStartStop.Text = CTResources.LoadResString(frmXrayControl.MecaXrayOn == modCT30K.OnOffStatusConstants.OnStatus ? StringTable.IDS_btnStop : StringTable.IDS_btnStart);		//停止/開始  'v11.5変更 by 間々田 2006/06/21
			cmdStartStop.Refresh();
			cmdStartStop.Enabled = !frmXrayControl.tmrXrayTool.Enabled;

			//強制ウォームアップ解除ボタン
			cmdWarmupNone.Enabled = (!frmXrayControl.tmrXrayTool.Enabled) && 
									(modXrayControl.XrayWarmUp() != modXrayControl.XrayWarmUpConstants.XrayWarmUpNow);

			//タイマー開始・停止ボタン
			cmdTimerStartStop.Text = CTResources.LoadResString(frmXrayControl.tmrXrayTool.Enabled ? StringTable.IDS_btnStop : StringTable.IDS_btnStart);		//停止/開始
			cmdTimerStartStop.Enabled = frmXrayControl.tmrXrayTool.Enabled || 
										(modXrayControl.XrayWarmUp() != modXrayControl.XrayWarmUpConstants.XrayWarmUpNow);

			//タイマー起動中の場合
			if (frmXrayControl.tmrXrayTool.Enabled)
			{
				//SetEnabledInFrame fraTimer, False, "CommandButton"
#region 【C#コントロールで代用】
/*
				cwneTimerMM.Mode = cwEdModeIndicator                    'v11.4変更 by 間々田 2006/03/10
				cwneTimerSS.Mode = cwEdModeIndicator                    'v11.4変更 by 間々田 2006/03/10
*/
#endregion
				cwneTimerMM.ReadOnly = true;			//v11.4変更 by 間々田 2006/03/10
				cwneTimerSS.ReadOnly = true;			//v11.4変更 by 間々田 2006/03/10
			}
			//タイマー停止中の場合
			else
			{
				//SetEnabledInFrame fraTimer, True, "CommandButton"
#region 【C#コントロールで代用】
/*
				cwneTimerMM.Mode = cwEdModeControl                      'v11.4変更 by 間々田 2006/03/10
				cwneTimerSS.Mode = cwEdModeControl                      'v11.4変更 by 間々田 2006/03/10
*/
#endregion
				cwneTimerMM.ReadOnly = false;			//v11.4変更 by 間々田 2006/03/10
				cwneTimerSS.ReadOnly = false;			//v11.4変更 by 間々田 2006/03/10
			}

            //変更2015/02/02hata_Max/Min範囲のチェック
            //cwneTimerMM.Value = modXrayControl.XrayToolTimerCount / 60;
			//cwneTimerSS.Value = modXrayControl.XrayToolTimerCount % 60;
            cwneTimerMM.Value = modLibrary.CorrectInRange((modXrayControl.XrayToolTimerCount / 60), cwneTimerMM.Minimum, cwneTimerMM.Maximum);
            cwneTimerSS.Value = modLibrary.CorrectInRange((modXrayControl.XrayToolTimerCount % 60), cwneTimerSS.Minimum, cwneTimerSS.Maximum);
        
        }


		//*******************************************************************************
		//機　　能：フォームロード時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： v11.3  2006/02/20  (SI3)間々田  新規作成
		//*******************************************************************************
		private void EnglishAdjustLayout()
		{
			cwneTimerMM.Left = cwneTimerMM.Left - 34;
			lblMM.TextAlign = ContentAlignment.TopRight;
			lblMM.Left = cwneTimerMM.Left + cwneTimerMM.Width;
		}
	}
}
