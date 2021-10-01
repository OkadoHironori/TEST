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
	public partial class frmXrayInformation : Form
	{
		private Label[] lblHeader = null;
		private Label[] lblInfo = null;

		private static frmXrayInformation _Instance = null;

		public frmXrayInformation()
		{
			InitializeComponent();
		}

		public static frmXrayInformation Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmXrayInformation();
				}

				return _Instance;
			}
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
		private void frmXrayInformation_Load(object sender, EventArgs e)
		{
			//コントロールの初期化
			InitControls();

			//v17.60 英語用レイアウト調整 by 長野 2011/05/25
			if (modCT30K.IsEnglish == true)
			{
				EnglishAdjustLayout();
			}
			//情報の表示（「最新の情報に更新」ボタンクリック時と同じ処理）
			cmdUpdate_Click(this, EventArgs.Empty);
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
            //Rev23.10 追加 by長野 2015/10/05
            this.Dispose();
		}


		//*******************************************************************************
		//機　　能： 「最新の情報に更新」ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdUpdate_Click(object sender, EventArgs e)
		{
			lblInfo[1].Text = GetSTIStr(modXrayControl.XrayControl.Up_XR_Status);					//状態確認（STI）
			lblInfo[2].Text = GetSTXStr(modXrayControl.XrayControl.Up_X_On);						//X線照射確認（STX）
			lblInfo[3].Text = GetSOVStr(modXrayControl.XrayControl.Up_XrayStatusSOV);				//過負荷保護機能（SOV）
			lblInfo[4].Text = Convert.ToString(modXrayControl.XrayControl.Up_XR_VoltFeedback);		//出力管電圧(SHV)
			lblInfo[5].Text = Convert.ToString(modXrayControl.XrayControl.Up_XR_CurrentFeedback);	//出力管電流(SCU)
			lblInfo[6].Text = Convert.ToString(modXrayControl.XrayControl.Up_XR_VoltSet);			//管電圧プリセット(SPV)
			lblInfo[7].Text = Convert.ToString(modXrayControl.XrayControl.Up_XR_CurrentSet);		//管電流プリセット(SPC)
			lblInfo[8].Text = GetSWRStr(modXrayControl.XrayControl.Up_Warmup);						//ウォーミングアップ実施（SWR）
			lblInfo[9].Text = GetSWEStr(modXrayControl.XrayControl.Up_Warmup);						//ウォーミングアップ完了（SWE）
			lblInfo[10].Text = modXrayControl.XrayControl.Up_XrayWarmupSWS.ToString("00");			//ウォーミングアップステップ（SWS）
			lblInfo[11].Text = GetSFCStr(modXrayControl.XrayControl.Up_Focussize);					//フォーカスモード（SFC）
			lblInfo[12].Text = GetSINStr(modXrayControl.XrayControl.Up_InterLock);					//インターロック（SIN）
			lblInfo[13].Text = GetSERStr(modXrayControl.XrayControl.Up_XrayStatusSER);				//制御基板異常確認（SER）

			//v15.10変更 byやまおか 2009/10/16
			//If XrayType = XrayTypeHamaL9191 Then
			//    lblInfo(14).Caption = .Up_XrayVacuumSVC             '真空度（SVC）
			//    lblInfo(15).Caption = CStr(.Up_XrayStatusSVV)       '真空計値（SVV）
			//End If
			if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL9191) || 
				//(modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801))
                (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10711) ||//Rev23.10 追加 by長野 2015/10/01
                (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721) ||//Rev23.10 追加 by長野 2015/10/01
                (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801))
            {
				lblInfo[14].Text = modXrayControl.XrayControl.Up_XrayVacuumSVC;						//真空度（SVC）
				lblInfo[15].Text = Convert.ToString(modXrayControl.XrayControl.Up_XrayStatusSVV);	//真空計値（SVV）

				if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801) ||
                   (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10711) ||
                   (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721)) //Rev23.10 追加 by長野 2015/10/01
                {
					lblInfo[16].Text = GetFLMStr(modXrayControl.XrayControl.Up_XrayStatusFLM);			//フィラメント状態確認（FLM）
                    lblInfo[17].Text = modXrayControl.XrayControl.Up_XrayStatusZT1.ToString();			//ターゲット温度（ZT1）
					lblInfo[18].Text = Convert.ToString(modXrayControl.XrayControl.Up_XrayStatusTYP);	//型名（TYP）
					//'テストモードなら使用最大管電圧を表示       'v15.10追加 byやまおか 2009/11/13
					//If IsTestMode Then lblInfo(19).Text = .Up_XrayStatusSMV  '使用最大間電圧（SMV）
                    //Rev23.10 条件追加 by長野 2015/10/05
                    if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801) ||
                           (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL12721)) //Rev23.10 追加 by長野 2015/10/01
                    {
                        lblInfo[19].Text = modXrayControl.XrayControl.Up_XrayStatusSMV.ToString();			//使用最大間電圧（SMV）  'v15.11変更 byやまおか 2010/02/09
                    }
                    else
                    {
                        lblInfo[19].Text = "-";
                    }
                }
			}
            //Rev23.10 フィラメントモード by長野 2015/10/05
            if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10711)
            {
                if (modXrayControl.XrayControl.Up_XrayStatusSMD == 0)
                {
                    lblInfo[19].Text = CTResources.LoadResString(14122);
                }
                else
                {
                    lblInfo[19].Text = CTResources.LoadResString(14123);
                }
            }
    	}


		//*******************************************************************************
		//機　　能： コントロールの初期化
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void InitControls()
		{
			int i = 0;
			int XrayInfoItemNum = 0;

			//サイズの調整(INTERLOCKの表示が切れていた)  'v15.10追加 byやまおか 2009/10/06
			this.Width = this.Width + 34;
			lblInfo1.Width = lblInfo1.Width + 20;
			lblInfo1.Left = lblInfo1.Left + 14;

			//XrayInfoItemNum = IIf(XrayType = XrayTypeHamaL9191, 15, 13)    'v15.10削除 byやまおか 2009/10/16
			//表示項目数を設定       'v15.10追加 byやまおか 2009/10/06
			switch (modXrayControl.XrayType)
			{
				case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
					XrayInfoItemNum = 15;
					break;
				//Case XrayTypeHamaL10801:    XrayInfoItemNum = IIf(IsTestMode, 19, 18)   'テストモードなら+1
				case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL12721://Rev23.10 追加 by長野 2015/10/01
					XrayInfoItemNum = 19;		//v15.11変更 byやまおか 2010/02/09
					break;
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711://Rev23.10 追加 by長野 2015/10/01
                    XrayInfoItemNum = 19;
                    break;
				default:
					XrayInfoItemNum = 13;
					break;
			}

			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);


			lblHeader = new Label[XrayInfoItemNum + 1];
			lblHeader[1] = lblHeader1;

			lblInfo = new Label[XrayInfoItemNum + 1];
			lblInfo[1] = lblInfo1;

			//コントロールのロード
			this.SuspendLayout();
			for (i = 2; i <= XrayInfoItemNum; i++)
			{
				lblHeader[i] = new Label();
				lblHeader[i].Name = string.Format("lblHeader{0}", i);
				lblHeader[i].Font = lblHeader1.Font;
				lblHeader[i].Size = lblHeader1.Size;
				lblHeader[i].Location = lblHeader1.Location;
				lblHeader[i].AutoSize = lblHeader1.AutoSize;

				lblHeader[i].Top = lblHeader[i - 1].Top + 20;
				lblHeader[i].Visible = true;
				lblHeader1.Parent.Controls.Add(lblHeader[i]);


				lblInfo[i] = new Label();
				lblInfo[i].Name = string.Format("lblInfo{0}", i);
				lblInfo[i].Font = lblInfo1.Font;
				lblInfo[i].Size = lblInfo1.Size;
				lblInfo[i].Location = lblInfo1.Location;
				lblInfo[i].BorderStyle = lblInfo1.BorderStyle;
				lblInfo[i].TextAlign = lblInfo1.TextAlign;

				lblInfo[i].Top = lblInfo[i - 1].Top + 20;
				lblInfo[i].Visible = true;
				lblInfo1.Parent.Controls.Add(lblInfo[i]);
			}
			this.ResumeLayout(false);

			//Ｘ線情報の項目名をリソースから取得
			for (i = 1; i <= XrayInfoItemNum; i++)
			{
				lblHeader[i].Text = CTResources.LoadResString(StringTable.IDS_XrayInfo + i);
			}

            //Rev23.10 X線毎に変更した項目を処理
            switch (modXrayControl.XrayType)
            {
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711://Rev23.10 追加 by長野 2015/10/01
                    lblHeader[19].Text = CTResources.LoadResString(14120);
                    break;
            }

			//位置の調整
			cmdUpdate.Top = lblHeader[XrayInfoItemNum].Top + lblHeader[XrayInfoItemNum].Height + 16;
			cmdClose.Top = cmdUpdate.Top;
			this.Height = cmdUpdate.Top + cmdUpdate.Height + 35;
		}


		//*******************************************************************************
		//機　　能： X線発生器状態確認（STI）の文字列を返す
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private string GetSTIStr(int theValue)
		{
			string functionReturnValue = null;

			switch (theValue)
			{
				case 0:
					functionReturnValue = "CONNECTING";			//change by 間々田 2004/09/22
					break;
				case 1:
					functionReturnValue = "PREHEAT";
					break;
				case 2:
					functionReturnValue = "NOT READY";
					break;
				case 3:
					functionReturnValue = "WARMUP";
					break;
				case 4:
					functionReturnValue = "XON";
					break;
				case 5:
					functionReturnValue = "WARMUP YET";
					break;
				case 6:
					functionReturnValue = "STAND BY";
					break;
				case 7:
					functionReturnValue = "OVER";				//change by 間々田 2004/09/22
					break;
				default:
					functionReturnValue = "";
					break;
			}
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： X線照射確認（STX）の文字列を返す
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private string GetSTXStr(int theValue)
		{
			string functionReturnValue = null;

			switch (theValue)
			{
				case 0:
					functionReturnValue = "OFF";
					break;
				case 1:
					functionReturnValue = "ON";
					break;
				default:
					functionReturnValue = "";
					break;
			}
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： 過負荷保護機能（SOV）の文字列を返す
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private string GetSOVStr(int theValue)
		{
			string functionReturnValue = null;

			switch (theValue)
			{
				case 0:
					functionReturnValue = "NORMAL";
					break;
				case 1:
					functionReturnValue = "OVER";
					break;
				default:
					functionReturnValue = "";
					break;
			}
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： ウォーミングアップ実施（SWR）の文字列を返す
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private string GetSWRStr(int theValue)
		{
			string functionReturnValue = null;

			switch (theValue)
			{
			//''        Case 0, 2:  GetSWRStr = "WARMUP ON"
			//''        Case 1:     GetSWRStr = "WARMUP OFF"
				case 0:
				case 2:
					functionReturnValue = "WARMUP OFF";			//changed by 山本 2007-5-21
					break;
				case 1:
					functionReturnValue = "WARMUP ON";			//changed by 山本 2007-5-21
					break;
				default:
					functionReturnValue = "";
					break;
			}
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： ウォーミングアップ完了（SWE）の文字列を返す
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private string GetSWEStr(int theValue)
		{
			string functionReturnValue = null;

			switch (theValue)
			{
				case 0:
				case 1:
					functionReturnValue = "WARMUP YET";
					break;
				case 2:
					functionReturnValue = "WARMUP END";
					break;
				default:
					functionReturnValue = "";
					break;
			}
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： フォーカスモード（SFC）の文字列を返す
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private string GetSFCStr(int theValue)
		{
			string functionReturnValue = null;

			switch (theValue)
			{
				case 1:
					functionReturnValue = "FCS1";
					break;
				case 2:
					functionReturnValue = "FCS2";
					break;
				case 3:
					functionReturnValue = "FCS3";
					break;
				case 4:
					functionReturnValue = "FCS4";
					break;
				default:
					functionReturnValue = "";
					break;
			}
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： インターロック（SIN）の文字列を返す
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private string GetSINStr(int theValue)
		{
			string functionReturnValue = null;

			switch (theValue)
			{
				case 0:
					functionReturnValue = "INTERLOCK OFF";
					break;
				case 1:
					functionReturnValue = "INTERLOCK ON";
					break;
				default:
					functionReturnValue = "";
					break;
			}
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： 制御基板異常確認（SER）の文字列を返す
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private string GetSERStr(int theValue)
		{
			string functionReturnValue = null;

			switch (theValue)
			{
				case 0:
					functionReturnValue = "ERR 0";
					break;
				case 1:
					functionReturnValue = "ERR 1";
					break;
				case 2:
					functionReturnValue = "ERR 2";
					break;
				case 3:
					functionReturnValue = "ERR 3";
					break;
				case 4:
					functionReturnValue = "ERR 4";
					break;
				case 5:
					functionReturnValue = "ERR 5";
					break;
				default:
					functionReturnValue = "";
					break;
			}
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： フィラメント状態確認（FLM）の文字列を返す
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V15.10  09/10/06   やまおか      新規作成
		//*******************************************************************************
		private string GetFLMStr(int theValue)
		{
			string functionReturnValue = null;

			switch (theValue)
			{
				case 0:
					functionReturnValue = "FLM 0";
					break;
				case 1:
					functionReturnValue = "FLM 1";
					break;
				case 2:
					functionReturnValue = "FLM 2";
					break;
				default:
					functionReturnValue = "";
					break;
			}
			return functionReturnValue;
		}


		//*******************************************************************************
		//機　　能： 英語用レイアウト調整
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V17.60  11/05/25   (検S１)長野      新規作成
		//*******************************************************************************
		private void EnglishAdjustLayout()
		{
			int infoMargin = 0;
			int buttonMargin = 0;
			int i = 0;
			int N = 0;
			this.Width = 407;

			//各情報の設定
			//一番長い単語をマージンとしておく。
			//infoMargin = (this.Width - lblHeader[3].Width - lblInfo[3].Width) / 3;
            infoMargin = Convert.ToInt32((this.Width - lblHeader[3].Width - lblInfo[3].Width) / 3F);
            N = lblHeader.Length - 1;
			for (i = 1; i <= N; i++)
			{
				lblHeader[i].Left = infoMargin;

				if (i <= 2)
				{
					lblInfo[i].Left = lblHeader[3].Left + lblHeader[3].Width + infoMargin + 3;
				}
				else
				{
					lblInfo[i].Left = lblHeader[3].Left + lblHeader[3].Width + infoMargin;
				}
			}

			//各ボタンの設定
            //2014/11/07hata キャストの修正
            //cmdUpdate.Width = (int)(cmdUpdate.Width * 1.2);
            //cmdUpdate.Height = (int)(cmdUpdate.Height * 1.2);
            //cmdClose.Height = (int)(cmdClose.Height * 1.2);
            cmdUpdate.Width = Convert.ToInt32(cmdUpdate.Width * 1.2);
            cmdUpdate.Height = Convert.ToInt32(cmdUpdate.Height * 1.2);
            cmdClose.Height = Convert.ToInt32(cmdClose.Height * 1.2);

            //2014/11/07hata キャストの修正
            //buttonMargin = (this.Width - cmdUpdate.Width - cmdClose.Width) / 3;
            buttonMargin = Convert.ToInt32((this.Width - cmdUpdate.Width - cmdClose.Width) / 3F);

			cmdUpdate.Left = buttonMargin;
			cmdClose.Left = cmdUpdate.Left + cmdUpdate.Width + buttonMargin;
		}
	}
}
