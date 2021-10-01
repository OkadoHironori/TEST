using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

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
			MOVE_TABLE_Z,
            MOVE_FILTER //Rev23.40 変更 by長野 2016/06/19
		}

		//メカ制御画面への参照
		private frmMechaControl myMechaControl;

        //Rev23.40 追加 by長野 2016/06/19
        //X線制御画面への参照
        private frmXrayControl myXrayControl;

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
        //外観カメラ指定時の自動スキャン位置移動モードか?  Rev26.40 by chouno 2019/03/08
        private bool IsAutoScanPosCamera = false;

		//メカ移動モード定数
		public enum MechaMoveModeConstants
		{
			MechaMoveMode_Ready,
			MechaMoveMode_AutoMove,
			MechaMoveMode_ManualMove,
            MechaMoveMode_MoveFDD,
			MechaMoveMode_Done
		}

		//メカ移動モード変数
		private MechaMoveModeConstants myMechaMoveMode;

        //指定FCDと指定FDDが限界FCD内か？
        bool IsInLimitFCDandFDD = false; //Rev26.40 add by chouno 2019/02/18

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

#endregion

		private NumTextBox[] ntbMove = null;

		private static frmMechaMove _Instance = null;

		public frmMechaMove()
		{
			InitializeComponent();

			ntbMove = new NumTextBox[] { ntbMove0, ntbMove1, ntbMove2, ntbMove3, ntbMove4, ntbMove5 };
            //ntbMove = new NumTextBox[] { ntbMove0, ntbMove1, ntbMove2, ntbMove3, ntbMove4, ntbMove5, ntbMove6 }; //Rev23.40 変更 by長野 2016/06/19
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
            get//Rev26.40 add by chouno 2019/02/18
            {
                return myMechaMoveMode;
            }
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
						//cwbtnMoveValue = false;
                        cwbtnMove.Value = false;
						cwbtnMove.Enabled = true;

                        //Rev26.40 add by chouno 2019/02/18
                        IsInLimitFCDandFDD = false;

                        cwbtnMove.Caption = CTResources.LoadResString(12317); 

						//初期プロンプト
						//lblPrompt.Caption = "よろしければ「移動」ボタンをクリックしてください。"
						
                        //Rev23.30 初期プロンプトは無しでもOK by長野 2016/02/23
                        //lblPrompt.Text = CTResources.LoadResString(20042);	//ストリングテーブル化 'v17.60 by 長野 2011/05/22

                        //Rev25.01 戻す by長野 2016/1205 
                        //Rev26.40 del by chouno 2019/02/17
                        //lblPrompt.Text = CTResources.LoadResString(20042);	//ストリングテーブル化 'v17.60 by 長野 2011/05/22

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
                        //cwbtnMove.Text = CTResources.LoadResString(20043);
                        cwbtnMove.Caption = CTResources.LoadResString(20043);

 						cwbtnMoveOnColor = Color.Lime;
						cwbtnMove.Visible = true;
						cwbtnMove.Enabled = true;
						//cwbtnMoveValue = false;
                        cwbtnMove.Value = false;
                        cwbtnMoveMode = cwModeSwitch.UntilReleased;

                        //手動移動プロンプト
						//lblPrompt.Caption = "指定されたFCD位置に移動するには試料テーブルとＸ線管が接触しないよう確認しつつ、「手動移動」ボタンを押し続けてください。"
						lblPrompt.Text = CTResources.LoadResString(20044);		//ストリングテーブル化 'v17.60 by長野 2011/05/22

						//Ｘ線管干渉が制限されている場合、制限を解除する旨通知
						if ((CTSettings.scaninh.Data.table_restriction == 0) & (!modSeqComm.MySeq.stsTableMovePermit))
						{
							//lblPrompt.Caption = lblPrompt.Caption & vbCr & _
							//'                   "（「手動移動」ボタンを押すと「Ｘ線管干渉制限」は解除されます。）"
							//ストリングテーブル化 'v17.60 by長野 2011/05/22
							lblPrompt.Text = lblPrompt.Text + "\r" + CTResources.LoadResString(20045);
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
						lblPrompt.Text = CTResources.LoadResString(20046) + "\r" + "\r" + CTResources.LoadResString(20047);		//ストリングテーブル化 'v17.60 by長野 2011/05/22

						//cwbtnMoveValue = false;
						cwbtnMove.Value = false;
						cwbtnMove.Enabled = false;
						cwbtnMove.Visible = false;

						//戻り値用変数をセット
						IsOK = true;
						break;

                    //Rev26.40 add by chouno 2019/02/18
                    case MechaMoveModeConstants.MechaMoveMode_MoveFDD:

                        //マウスポインタを元に戻す
                        Cursor.Current = Cursors.Default;

                        //「閉じる」ボタンを使用可にする
                        cmdClose.Enabled = true;

                        cwbtnMove.Caption = CTResources.LoadResString(12317); 

                        //移動ボタンの調整
                        //cwbtnMoveValue = false;
                        cwbtnMove.Value = false;
                        cwbtnMove.Enabled = true;
                        cwbtnMoveMode = cwModeSwitch.WhenPressed;

                        lblPrompt.Text = "FDDを移動します。よろしければ[移動]ボタンをクリックしてください。";

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
			//this.Hide();
            this.Close();

            //Rev20.00 dispose追加 by長野 2015/04/16
            this.Dispose();
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
			//IsAutoScanPosMode = false;
            //Rev20.00 初めにMechaMoveForAutoScanPosが呼ばれた時点で、初期化とセットは終わっているので不要 by長野 2015/04/16

			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

            //追加2014/09/19(検S1)hata
            cwbtnMove.Caption = CTResources.LoadResString(12317); 
            
            ntbMove0.Caption = StringTable.GetResString(12131, CTSettings.AxisName[0]);
            ntbMove1.Caption = StringTable.GetResString(12131, CTSettings.AxisName[1]);
            //Rev21.00 表示・非表示をscaninhで制御する by長野 2015/03/16
            //Rev22.00 条件追加 微調のリミットがすべてONの場合は外れているとみなす by長野 2015/09/05
            //if ((CTSettings.scaninh.Data.fine_table_x == 0) && !((CTSettings.mecainf.Data.ystg_l_limit == 1) && (CTSettings.mecainf.Data.ystg_u_limit == 1) && (CTSettings.mecainf.Data.xstg_l_limit == 1) && (CTSettings.mecainf.Data.xstg_u_limit == 1)))
            if ((CTSettings.scaninh.Data.fine_table_x == 0) && ((CTSettings.mecainf.Data.ystg_l_limit == 1) && (CTSettings.mecainf.Data.ystg_u_limit == 1) && (CTSettings.mecainf.Data.xstg_l_limit == 1) && (CTSettings.mecainf.Data.xstg_u_limit == 1)))
            {
                //Rev99.99
                //ntbMove0.Visible = true;
            }
            else
            {
                //Rev99.99
                //bMove0.Visible = false;
            }
            //if ((CTSettings.scaninh.Data.fine_table_y == 0) && !((CTSettings.mecainf.Data.ystg_l_limit == 1) && (CTSettings.mecainf.Data.ystg_u_limit == 1) && (CTSettings.mecainf.Data.xstg_l_limit == 1) && (CTSettings.mecainf.Data.xstg_u_limit == 1)))
            //Rev99.99
            if ((CTSettings.scaninh.Data.fine_table_y == 0) && ((CTSettings.mecainf.Data.ystg_l_limit == 1) && (CTSettings.mecainf.Data.ystg_u_limit == 1) && (CTSettings.mecainf.Data.xstg_l_limit == 1) && (CTSettings.mecainf.Data.xstg_u_limit == 1)))
            {
                //ntbMove1.Visible = true;
                ntbMove1.Visible = false;
            }
            else
            {
                //Rev99.99
                //ntbMove1.Visible = false;
            }

            ntbMove4.Caption = CTSettings.AxisName[1];
            //Rev20.01 追加 by長野 2015/05/19
            ntbMove5.Caption = CTResources.LoadResString(12170);

            //表示位置
            //this.SetBounds(frmCTMenu.Instance.Left + (frmCTMenu.Instance.Width - this.Width) / 2, 
            //               frmCTMenu.Instance.Top + (frmCTMenu.Instance.Height - this.Height) / 2, 
            //               this.Width, 
            //               this.Height);

            //v17.60 英語用レイアウト調整 by長野 2011/05/25
            if (modCT30K.IsEnglish == true)
            {
                EnglishAdjustLayout();
            }

            //Rev23.10 計測CTの場合に表示桁数を変更する by長野 2015/11/04
            if (CTSettings.scaninh.Data.cm_mode == 0)
            {
                //ntbMove[(int)MECHA_MOVE.MOVE_FCD].DiscreteInterval = (float)0.001;
                //ntbMove[(int)MECHA_MOVE.MOVE_FID].DiscreteInterval = (float)0.001;
                //ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].DiscreteInterval = (float)0.001;
                //ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].DiscreteInterval = (float)0.001;
                //Rev26.14 change by chouno 2018/09/05
                ntbMove[(int)MECHA_MOVE.MOVE_FCD].DiscreteInterval = (float)0.01;
                ntbMove[(int)MECHA_MOVE.MOVE_FID].DiscreteInterval = (float)0.01;
                ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].DiscreteInterval = (float)0.01;
                ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].DiscreteInterval = (float)0.001;
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
            cwbtnMoveMode = cwModeSwitch.WhenPressed;

            Application.DoEvents();

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
        private void frmMechaMove_FormClosed(object sender, FormClosedEventArgs e)
		{
			//ＦＣＤ速度を元に戻す
			if (!modSeqComm.SeqWordWrite("YSpeed", Convert.ToString(YSpeedBack), false)) return;
			Application.DoEvents();

			//タッチパネル操作のアンロックを追加 'v17.61 by 長野 2011/09/12
			modSeqComm.SeqBitWrite("PanelInhibit", false);		//(True → False)

			//メカ制御画面への参照破棄
            if (myMechaControl != null) //追加201501/26hata_if文追加
            {
                myMechaControl.FXChanged -= myMechaControl_FXChanged;
                myMechaControl.FYChanged -= myMechaControl_FYChanged;
                myMechaControl.FCDChanged -= myMechaControl_FCDChanged;
                myMechaControl.FIDChanged -= myMechaControl_FIDChanged;
                myMechaControl.YChanged -= myMechaControl_YChanged;
                myMechaControl.UDPosChanged -= myMechaControl_UDPosChanged;
                myMechaControl = null;
            }
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
		//public bool MechaMove(decimal? FX = null, decimal? fy = null, float? FCD = null, float? Fid = null, decimal? y = null, decimal? z = null)
		public bool MechaMove(decimal? FX = null, decimal? fy = null, float? FCD = null, float? Fid = null, decimal? y = null, decimal? z = null)
        {
			bool functionReturnValue = false;

			//このフォームを表示する必要があるか
			bool IsVisible = false;

            //FDD移動有?
            bool IsFddMove = (Fid != null);

			frmMechaControl frmMechaControl = frmMechaControl.Instance;

			//値をセット

            //Rev23.10 計測CTの場合に表示桁数を変更する by長野 2015/11/04
            if (CTSettings.scaninh.Data.cm_mode == 0)
            {
                //ntbMove[(int)MECHA_MOVE.MOVE_FCD].DiscreteInterval = (float)0.001;
                //ntbMove[(int)MECHA_MOVE.MOVE_FID].DiscreteInterval = (float)0.001;
                //ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].DiscreteInterval = (float)0.001;
                //ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].DiscreteInterval = (float)0.001;
                //Rev26.14 change by chouno 2018/09/05
                ntbMove[(int)MECHA_MOVE.MOVE_FCD].DiscreteInterval = (float)0.01;
                ntbMove[(int)MECHA_MOVE.MOVE_FID].DiscreteInterval = (float)0.01;
                ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].DiscreteInterval = (float)0.01;
                ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].DiscreteInterval = (float)0.001;
            }

            //v18.00条件追加 byやまおか 2011/06/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //追加2014/10/07hata_v19.51反映
            if ((CTSettings.scaninh.Data.fine_table == 0))
            {
                ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value = (decimal)((FX == null) ? frmMechaControl.ntbFTablePosX.Value : FX);
                ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value = (decimal)((fy == null) ? frmMechaControl.ntbFTablePosY.Value : fy);
            }

			ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value = FX ?? frmMechaControl.ntbFTablePosX.Value;
			ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value = fy ?? frmMechaControl.ntbFTablePosY.Value;
			ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value = (decimal)(FCD ?? ScanCorrect.GVal_Fcd);
			ntbMove[(int)MECHA_MOVE.MOVE_FID].Value = (decimal)(Fid ?? ScanCorrect.GVal_Fid);
			ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value = y ?? frmMechaControl.ntbTableXPos.Value;
			ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Value = z ?? frmMechaControl.ntbUpDown.Value;

            //追加2014/10/07hata_v19.51反映
            //v18.00条件追加 byやまおか 2011/06/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if ((CTSettings.scaninh.Data.fine_table == 0))
            {
                myMechaControl_FXChanged(this, EventArgs.Empty);
                myMechaControl_FYChanged(this, EventArgs.Empty);
            }

			myMechaControl_FXChanged(this, EventArgs.Empty);
			myMechaControl_FYChanged(this, EventArgs.Empty);
			myMechaControl_FCDChanged(this, EventArgs.Empty);
			myMechaControl_FIDChanged(this, EventArgs.Empty);
			myMechaControl_YChanged(this, EventArgs.Empty);
			myMechaControl_UDPosChanged(this, EventArgs.Empty);

			//コントロールの位置調整・移動不要の分は非表示
			int theTop = 0;
			theTop = ntbMove[ntbMove.GetLowerBound(0)].Top;

            //追加2014/10/07hata_v19.51反映
            //微調がないときは表示させないために緑色にする   'v18.00追加 byやまおか 2011/07/04 'v19.41とv18.02の統合 by長野 2014/01/07
            //Rev22.00 条件変更 装着の有無まで見るようにした　by長野 2015/09/05
            //if ((CTSettings.scaninh.Data.fine_table != 0))
            //Rev26.00 回転大テーブル装着を条件に追加 add by chouno 2017/04/17
            if(((CTSettings.scaninh.Data.fine_table != 0) || ((CTSettings.mecainf.Data.ystg_l_limit == 1) && (CTSettings.mecainf.Data.ystg_u_limit == 1) && (CTSettings.mecainf.Data.xstg_l_limit == 1) && (CTSettings.mecainf.Data.xstg_u_limit == 1)))||
                //(modSeqComm.GetLargeRotTableSts() == 1))
                //Rev26.20 大テーブルand微調テーブルタイプ(内臓タイプ)に変更 by chouno 2019/02/11 
                ((modSeqComm.GetLargeRotTableSts() == 1) && CTSettings.t20kinf.Data.ftable_type == 0))
            {
                //ntbMove[0].BackColor = System.Drawing.Color.Lime;                //微調X
                //ntbMove[1].BackColor = System.Drawing.Color.Lime;                //微調Y
                //Rev22.00 修正 by長野 2015/09/05
                ntbMove[0].TextBackColor = System.Drawing.Color.Lime;                //微調X
                ntbMove[1].TextBackColor = System.Drawing.Color.Lime;                //微調Y
            }

			int i = 0;
			for (i = ntbMove.GetLowerBound(0); i <= ntbMove.GetUpperBound(0); i++)
			{
				//if (ntbMove[i].BackColor == Color.Lime)
                if (ntbMove[i].TextBackColor == Color.Lime)
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
            if (AppValue.IsTestMode)
			{
				ntbFCD1st.Visible = (ntbMove[(int)MECHA_MOVE.MOVE_FCD].TextBackColor != Color.Lime);
                //変更2014/11/10hata_記述ミスと思われる
                //ntbY1st.Visible = (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Top * 15 != ColorTranslator.ToOle(Color.Lime));
                ntbY1st.Visible = (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].TextBackColor !=Color.Lime);
                
                ntbFCD1st.Top = ntbMove[(int)MECHA_MOVE.MOVE_FCD].Top;
				ntbY1st.Top = ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Top;
			}

			//試料テーブルをＸ線管近くに移動する場合
            //変更2014/10/07hata_v19.51反映
            //if (IsTryOverFCDLimit())

            //Rev23.21 ここでFCD移動量がFDD側の限界を超えて移動しないかチェック
            //移動する場合は、ntbFCD1stとntbMoveを書き換える。 by長野 2016/02/23
            //if (IsTryOverFCD1_FDDLimit()
            //Rev26.00 add by chouno 2017/10/20
            if (ntbFCD1st.Visible == false)
            {
                if (IsTryOverFCD1_FDDLimit(IsFddMove ? (float)Fid : (float)ScanCorrect.GVal_Fid))//Rev26.00 change by chouno 2017/10/16
                {
                    //Rev26.14 FDD移動を考慮した値にかえる by chouno 2018/09/10
                    if (IsFddMove)
                    {
                        ntbFCD1st.Value = (decimal)(Fid - CTSettings.Gval_BetMaxFcdAndFdd);
                    }
                    else
                    {
                        ntbFCD1st.Value = (decimal)(ScanCorrect.GVal_Fid - CTSettings.Gval_BetMaxFcdAndFdd);
                    }
                }
            }

            //if (IsTryOverFCD2_FDDLimit())
            if (IsTryOverFCD2_FDDLimit(IsFddMove ? (float)Fid : (float)ScanCorrect.GVal_Fid))
            {
                //Rev26.14 FDD移動を考慮した値にかえる by chouno 2018/09/10
                if(IsFddMove)
                {
                    if ((float)Fid > CTSettings.mechapara.Data.max_fdd)
                    {
                        ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value = (decimal)(CTSettings.mechapara.Data.max_fdd - CTSettings.Gval_BetMaxFcdAndFdd);
                    }
                    else
                    {
                        ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value = (decimal)(Fid - CTSettings.Gval_BetMaxFcdAndFdd);
                    }
                }
                else
                {
                    if ((float)ScanCorrect.GVal_Fid > CTSettings.mechapara.Data.max_fdd)
                    {
                        ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value = (decimal)(CTSettings.mechapara.Data.max_fdd - CTSettings.Gval_BetMaxFcdAndFdd);
                    }
                    else
                    {
                        ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value = (decimal)(ScanCorrect.GVal_Fid - CTSettings.Gval_BetMaxFcdAndFdd);
                    }
                }

                //試料テーブルと検出器が近接するため、FCD=%1(mm)で停止します。
                lblPrompt.Text = lblPrompt.Text + "\r" + "\r" + StringTable.GetResString(24014, Convert.ToString(ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value));
            }

			//v18.00条件変更 byやまおか 2011/06/29
			if ((IsTryOverFCDLimit()) & (CTSettings.scaninh.Data.avmode != 0))
            {
                //現在のＦＣＤ位置が限界ＦＣＤ内にない場合
                //if (ScanCorrect.GVal_Fcd > CTSettings.GVal_FcdLimit)
                if (ScanCorrect.GVal_Fcd > modSeqComm.GetFCDLimit()) //Rev25.10 add by chouno 2017/09/11
				{
					//            lblPrompt.Caption = lblPrompt.Caption & vbCr & vbCr & _
					//                               "注：試料テーブルとＸ線管が近接するため、FCD=" & CStr(GVal_FcdLimit) & _
					//                                "(mm)の位置でいったん停止します。"
                    //lblPrompt.Text = lblPrompt.Text + "\r" + "\r" + StringTable.GetResString(20049, Convert.ToString(CTSettings.GVal_FcdLimit));
                    //Rev25.10 change by chuono 2017/09/11 
                    lblPrompt.Text = lblPrompt.Text + "\r" + "\r" + StringTable.GetResString(20049, Convert.ToString(modSeqComm.GetFCDLimit()));
				}
				//現在のＦＣＤ位置が限界ＦＣＤ内にある場合：
				else
				{
					//           lblPrompt.Caption = lblPrompt.Caption & vbCr & vbCr & _
					//                               "注：試料テーブルとＸ線管が近接するため、指定されたFCD位置への移動は" & _
					//                                "手動で行ないます。"
					//ストリングテーブル化 'v17.60 by長野 2011/05/22
					lblPrompt.Text = lblPrompt.Text + "\r" + "\r" + CTResources.LoadResString(20050);

					//            '手動移動モードに切り替え
					//            MechaMoveMode = MechaMoveMode_ManualMove
				}

            //追加2014/10/07hata_v19.51反映
            //産業用CTモードの場合   'v18.00変更 byやまおか 2011/06/29
            }
            //else
            else if(CTSettings.scaninh.Data.avmode == 0)
            {
                //lblPrompt.Text = lblPrompt.Text + "\n" + "\n" + "注：試料テーブルとＸ線管が近接するため、指定されたFCD位置への移動は" + "手動で行ないます。";
                //rev20.01 リソース化 by長野 2015/05/19
                lblPrompt.Text = lblPrompt.Text + "\n" + "\n" + CTResources.LoadResString(20050);
            }

			//このフォームを表示する必要がある？
			if (IsVisible)
			{
				//すでに表示している場合抜ける       '追加 by 間々田 2009/07/09
				if (this.Visible) return functionReturnValue;

                Application.DoEvents();
				//モーダル表示
                //変更2015/02/27hata
                //this.ShowDialog();
                this.ShowDialog(frmCTMenu.Instance);
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
            try
            {
                this.Close();
            }
            catch
            { 
            }
            

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
        //private void cwbtnMoveChanged()		// 【C#コントロールで代用】
        private void cwbtnMove_ValueChanged(object sender, EventArgs e)		// 【C#コントロールで代用】
		{
			//ノーマル移動の時は無視する
			if (cwbtnMoveMode == cwModeSwitch.WhenPressed) return;

			//if (cwbtnMoveValue)
            if (cwbtnMove.Visible && cwbtnMove.Value)
            {
                //点滅をさせる
                cwbtnMove.FlatStyle = FlatStyle.Standard;
                //cwbtnMove.OnColor = Color.Lime;
                cwbtnMove.BlinkInterval = CWSpeeds.cwSpeedFastest;
                //cwbtnMove.Value = true;

				//Ｘ線管干渉制限を解除
				if ((CTSettings.scaninh.Data.table_restriction == 0) && (!modSeqComm.MySeq.stsTableMovePermit))
				{
					modSeqComm.SeqBitWrite("TableMovePermit", true);
                    Application.DoEvents();
                }

				//ＦＣＤ速度を一時的に変更する
                if (!modSeqComm.SeqWordWrite("YSpeed", (CTSettings.mechapara.Data.fcd_speed[(int)frmMechaControl.SpeedConstants.SpeedSlow] * 10).ToString("0"), false)) return;
                Application.DoEvents();

				if (chkXYMove.CheckState == CheckState.Checked)
				{
					//移動X座標送信
                    //2014/11/07hata キャストの修正
                    //if (!modSeqComm.SeqWordWrite("XIndexPosition", Convert.ToString((int)(ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value * 100)), false))return;
                    //if (!modSeqComm.SeqWordWrite("XIndexPosition", Convert.ToString(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value * 100)), false)) return;
                    if (!modSeqComm.SeqWordWrite("XIndexPosition", Convert.ToString(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value * modMechaControl.GVal_TableX_SeqMagnify)), false)) return;
                    Application.DoEvents();
				}

				//移動FCD座標送信
                //2014/11/07hata キャストの修正
                //if (!modSeqComm.SeqWordWrite("YIndexPosition", Convert.ToString((int)(ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value * 10)), false)) return;
                //if (!modSeqComm.SeqWordWrite("YIndexPosition", Convert.ToString(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value * 10)), false)) return;
                if (!modSeqComm.SeqWordWrite("YIndexPosition", Convert.ToString(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value * modMechaControl.GVal_FCD_SeqMagnify)), false)) return;
                Application.DoEvents();

				if (chkXYMove.CheckState == CheckState.Checked)
				{
					//移動実行命令送信
                    if (!modSeqComm.SeqBitWrite("XYIndex", true, false)) return;
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

                //ＦＣＤ速度を元に戻す
                if (!modSeqComm.SeqWordWrite("YSpeed", Convert.ToString(YSpeedBack), false)) return;
                Application.DoEvents();


                //点滅を止める
                //cwbtnMove.BlinkInterval = CWSpeeds.cwSpeedOff;
                cwbtnMove.FlatStyle = FlatStyle.Standard;
                //cwbtnMove.OnColor = SystemColors.ButtonFace;
                //cwbtnMove.Value = false;
            
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

            ////if (!((cwbtnMoveMode == cwModeSwitch.WhenPressed && cwbtnMoveValue == true) ||
            ////    (cwbtnMoveMode == cwModeSwitch.UntilReleased && cwbtnMoveValue == false)))
            //if (!((cwbtnMoveMode == cwModeSwitch.WhenPressed && cwbtnMove.Value == true) ||
            //    (cwbtnMoveMode == cwModeSwitch.UntilReleased && cwbtnMove.Value == false)))
            //{
            //    // cwModeSwitch.WhenPressedモードで、Buttonを離す前(cwbtnMoveValue = true) または
            //    // cwModeSwitch.UntilReleasedモードで、Buttonを離した後(cwbtnMoveValue = false) の時以外は
            //    // Clickイベントは発生しない
            //    return;
            //}

			#endregion

			int err_sts = 0;
			float FCD1 = 0;
			float FCD2 = 0;
			float PosY1 = 0;
			float PosY2 = 0;

            bool IsFddMove = (ntbMove[(int)MECHA_MOVE.MOVE_FID].TextBackColor != Color.Lime);//Rev26.00 add by chouno 2017/10/16

            //追加2014/10/07hata_v19.51反映
            //メカが動ける(パネルがOFF)かチェック     'v18.00追加 byやまおか 2011/07/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if ((!modMechaControl.IsOkMechaMove()))
                return;

			//「手動移動」ボタンの時は無視する
			if (cwbtnMoveMode != cwModeSwitch.WhenPressed) return;		// 【C#コントロールで代用】

			//移動前動作チェック   'v16.10追加 byやまおか 2010/03/17
            if (!IsOkMove()) return;

            frmMechaControl frmMechaControl = frmMechaControl.Instance;

			//ＦＣＤ速度を一時的に変更する   'v16.10追加 byやまおか 2010/03/17
			if (!modSeqComm.SeqWordWrite("YSpeed", (CTSettings.mechapara.Data.fcd_speed[(int)frmMechaControl.SpeedConstants.SpeedFast] * 10).ToString("0"), false)) return;
			Application.DoEvents();

			//自動移動モードにする
			MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_AutoMove;

            //点滅をさせる
            cwbtnMove.FlatStyle = FlatStyle.Flat;
            //cwbtnMove.OnColor = Color.Lime;
            cwbtnMove.BlinkInterval = CWSpeeds.cwSpeedFastest;
            cwbtnMove.Value = true;

            string buf = null;
			buf = "";

            //Rev23.30 外観カメラ画像からのスキャン位置指定の場合は、ここでROI描画ストップ(パテントに関係するため削除禁止) & 昇降を元の位置に戻す by長野 2016/02/23
            if (frmExObsCam.Instance.ImageProc == frmExObsCam.ImageProcType.RoiAutoPos)
            {
                frmExObsCam.Instance.ROIDraw = false;
                frmExObsCam.Instance.Refresh();
            }

            if (MechaMoveMode == MechaMoveModeConstants.MechaMoveMode_AutoMove && IsInLimitFCDandFDD)
            {
                //FID
                if (ntbMove[(int)MECHA_MOVE.MOVE_FID].TextBackColor != Color.Lime)
                {
                    //指定FID位置までI.I.を移動させる
                    //2014/11/07hata キャストの修正
                    //if (!modSeqComm.MoveFID((int)ntbMove[(int)MECHA_MOVE.MOVE_FID].Value * 10))
                    //if (!modSeqComm.MoveFID(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_FID].Value * 10)))
                    if (!modSeqComm.MoveFID(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_FID].Value * modMechaControl.GVal_FDD_SeqMagnify)))//Rev23.10 変更 by長野 2015/09/18
                    {
                        //エラーの場合：指定されたFID位置までI.I.を移動させることができませんでした。
                        buf = buf + "* " + StringTable.BuildResStr(StringTable.IDS_MoveErr, StringTable.IDS_FID, StringTable.IDS_II) + "\r";
                    }
                }
            }
            else
            {
                //追加2014/10/07hata_v19.51反映
                //v18.00条件追加 byやまおか 2011/06/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                //if ((CTSettings.scaninh.Data.fine_table == 0)) {
                //Rev22.00 条件追加 微調のすべてのリミットがONの場合は装着されていないとし、実行しない by長野 2015/09/05
                if ((CTSettings.scaninh.Data.fine_table == 0) && !((CTSettings.mecainf.Data.ystg_l_limit == 1) && (CTSettings.mecainf.Data.ystg_u_limit == 1) && (CTSettings.mecainf.Data.xstg_l_limit == 1) && (CTSettings.mecainf.Data.xstg_u_limit == 1)))
                {
                    //微調Ｘ軸
                    if (ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].TextBackColor != Color.Lime)
                    {
                        if (modMechaControl.MecaYStgIndex((float)ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value) != 0)
                        {
                            //エラーの場合：指定された微調X軸位置まで微調テーブルを移動させることができませんでした。
                            buf = buf + "* " + StringTable.GetResString(StringTable.IDS_MoveErr, ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Caption, CTResources.LoadResString(StringTable.IDS_FTable)) + "\r";
                        }
                    }

                    //微調Ｙ軸
                    if (ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].TextBackColor != Color.Lime)
                    {
                        if (modMechaControl.MecaXStgIndex((float)ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value) != 0)
                        {
                            //エラーの場合：指定された微調Y軸位置まで微調テーブルを移動させることができませんでした。
                            buf = buf + "* " + StringTable.GetResString(StringTable.IDS_MoveErr, ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Caption, CTResources.LoadResString(StringTable.IDS_FTable)) + "\r";
                        }
                    }
                }

                //v19.12 windows7対策 タイマーによる更新が遅くなるため、更新処理をここで明示的に呼ぶ by長野 2013/03/08
                frmMechaControl.MyUpdate();
                frmMechaControl.UpdateMecha();

                //自動スキャン位置移動にて、微調テーブル移動でエラーが発生している場合
                //if (IsAutoScanPosMode && (!string.IsNullOrEmpty(buf)))
                if ((IsAutoScanPosMode || IsAutoScanPosCamera) && (!string.IsNullOrEmpty(buf))) //条件変更 Rev26.40 by chouno 2019/03/08
                {
                    //モードを元に戻す
                    MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Ready;

                    //専用のダイアログを表示
                    if (frmMechaMoveWarning.Instance.Dialog())
                    {
                        //自動移動モードにする
                        MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_AutoMove;

                        //追加2014/10/07hata_v19.51反映
                        //v18.00条件追加 byやまおか 2011/06/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                        if ((CTSettings.scaninh.Data.fine_table == 0))
                        {
                            //Rev23.30 外観カメラとの分岐を追加 by長野 2016/02/06
                            //if (frmExObsCam.Instance.AutoScaleMode == 0)
                            if (frmExObsCam.Instance.ImageProc == frmExObsCam.ImageProcType.RoiAutoPos)
                            {
                                //現在の微調テーブルの位置で最適な試料テーブルのXY座標を求める
                                err_sts = ScanCorrect.auto_tbl_set_byExObsCam(ref RoiCircle[0],
                                                                    frmExObsCam.Instance.matrix,
                                                                    frmExObsCam.Instance.pixsize,
                                                                    PosFy, PosFx,
                                                                    (float)frmMechaControl.ntbFTablePosY.Value, (float)frmMechaControl.ntbFTablePosX.Value,
                                                                    CTSettings.scansel.Data.fcd, CTSettings.scansel.Data.fid,//Rev26.00 add by chouno 2017/01/17
                                                                    ref FCD1, ref PosY1,
                                                                    ref FCD2, ref PosY2);
                            }
                            //else //Rev26.00 add by chouno 2017/01/17
                            else if (frmScanImage.Instance.ImageProc == frmScanImage.ImageProcType.RoiAutoPos || frmTransImage.Instance.TransImageProc == frmTransImage.TransImageProcType.TransRoiAutoPos)
                            {
                                //現在の微調テーブルの位置で最適な試料テーブルのXY座標を求める
                                err_sts = ScanCorrect.auto_tbl_set(ref RoiCircle[0],
                                                                    PosFy, PosFx,
                                                                    (float)frmMechaControl.ntbFTablePosY.Value, (float)frmMechaControl.ntbFTablePosX.Value,
                                                                    ref FCD1, ref PosY1,
                                                                    ref FCD2, ref PosY2);
                            }
                            else
                            {
                                //現在の微調テーブルの位置で最適な試料テーブルのXY座標を求める
                                err_sts = ScanCorrect.auto_tbl_set_byExObsCam(ref RoiCircle[0],
                                                                    frmExObsCam.Instance.matrix,
                                                                    frmExObsCam.Instance.pixsize,
                                                                    PosFy, PosFx,
                                                                    (float)ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value, (float)ntbMove[(int)MECHA_MOVE.MOVE_FID].Value,
                                                                    CTSettings.scansel.Data.fcd, CTSettings.scansel.Data.fid,//Rev26.00 add by chouno 2017/01/17
                                                                    ref FCD1, ref PosY1,
                                                                    ref FCD2, ref PosY2);

                            }

                            if (err_sts != 0) goto ErrorHandler;

                            //微調テーブルの移動目標値を現在の値にしておく
                            ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value = frmMechaControl.ntbFTablePosX.Value;
                            ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value = frmMechaControl.ntbFTablePosY.Value;

                            //変更2014/09/20(検S1)hata
                            //ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].BackColor = Color.Lime;
                            //ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].BackColor = Color.Lime;
                            ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].TextBackColor = Color.Lime;
                            ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].TextBackColor = Color.Lime;

                        }

                        //自動スキャン位置移動用移動第１段階ＦＣＤ・Ｙ軸座標を記憶
                        ntbFCD1st.Value = (decimal)(FCD1 - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()]);
                        ntbY1st.Value = (decimal)PosY1;

                        //自動スキャン位置移動用移動最終目標ＦＣＤ・Ｙ軸座標を記憶
                        ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value = (decimal)(FCD2 - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()]);
                        ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value = (decimal)PosY2;

                        //bufクリア
                        buf = "";

                        //Rev23.21 ここでFCD移動量がFDD側の限界を超えて移動しないかチェック
                        //移動する場合は、ntbFCD1stとntbMoveを書き換える。 by長野 2016/02/23
                        //if (IsTryOverFCD1_FDDLimit())
                        //Rev26.00 change by chouno 2017/10/16
                        if (IsTryOverFCD1_FDDLimit(IsFddMove ? (float)ntbMove[(int)MECHA_MOVE.MOVE_FID].Value : ScanCorrect.GVal_Fid))
                        {
                            ntbFCD1st.Value = (decimal)(ScanCorrect.GVal_Fid - CTSettings.Gval_BetMaxFcdAndFdd);
                        }
                        //if (IsTryOverFCD2_FDDLimit())
                        //Rev26.00 change by chouno 2017/10/16 
                        if (IsTryOverFCD2_FDDLimit(IsFddMove ? (float)ntbMove[(int)MECHA_MOVE.MOVE_FID].Value : ScanCorrect.GVal_Fid))
                        {
                            if ((float)ScanCorrect.GVal_Fid > CTSettings.mechapara.Data.max_fdd)
                            {
                                ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value = (decimal)(CTSettings.mechapara.Data.max_fdd - CTSettings.Gval_BetMaxFcdAndFdd);
                            }
                            else
                            {
                                ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value = (decimal)(ScanCorrect.GVal_Fid - CTSettings.Gval_BetMaxFcdAndFdd);
                            }
                            //試料テーブルと検出器が近接するため、FCD=%1(mm)で停止します。
                            //buf = buf + "*" + StringTable.GetResString(24014, Convert.ToString(ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value));
                            lblPrompt.Text = lblPrompt.Text + "*" + StringTable.GetResString(24014, Convert.ToString(ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value));

                        }

                    }
                    else
                    {
                        //'キャンセルの場合、メカ移動ダイアログを非表示にして抜ける
                        //Me.hide    'v17.10削除 hideしない byやまおか 2010/08/09
                        //Rev23.30 by長野 2016/03/14
                        cwbtnMove.Enabled = false;

                        return;
                    }
                }

                //Rev26.40 指定FCDと指定FDDが限界FCD内になるかチェック
                IsInLimitFCDandFDD = ((float)ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value <= CTSettings.GVal_FcdLimit) && ((CTSettings.GVal_FcdLimit + CTSettings.Gval_BetMaxFcdAndFdd) >= (float)ntbMove[(int)MECHA_MOVE.MOVE_FID].Value)
                    && (ntbMove[(int)MECHA_MOVE.MOVE_FID].TextBackColor != Color.Lime);


                //Rev25.03 change by chouno 2017/02/16
                //移動 ここから by山影			//昇降
                //if (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].TextBackColor != Color.Lime)
                //{
                //    if (modMechaControl.MechaUdIndex((float)ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Value) != 0)
                //    {
                //        //エラーの場合：指定された昇降位置まで試料テーブルを移動させることができませんでした。
                //        buf = buf + "* " + StringTable.GetResString(StringTable.IDS_MoveErr, ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Caption, CTResources.LoadResString(StringTable.IDS_SampleTable)) + "\r";
                //    }
                //}

                //Rev20.00 昇降位置と昇降位置指定位置をそろえる by長野 2015/03/06
                //frmMechaControl.cwnePos_ValueChanged(null, EventArgs.Empty);

                //Rev25.03 昇降から動作させるのは、透視位置指定のみとする。by chouno 2017/02/16
                //Rev26.40 外観カメラも昇降から動作させる by chouno 2019/03/08
                //if (IsAutoScanPosTransMode)
                if (IsAutoScanPosTransMode || IsAutoScanPosCamera)
                {
                    //移動 ここから by山影
                    //昇降
                    if (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].TextBackColor != Color.Lime)
                    {
                        if (modMechaControl.MechaUdIndex((float)ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Value) != 0)
                        {
                            //エラーの場合：指定された昇降位置まで試料テーブルを移動させることができませんでした。
                            buf = buf + "* " + StringTable.GetResString(StringTable.IDS_MoveErr, ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Caption, CTResources.LoadResString(StringTable.IDS_SampleTable)) + "\r";
                        }
                    }

                    //Rev20.00 昇降位置と昇降位置指定位置をそろえる by長野 2015/03/06
                    frmMechaControl.cwnePos_ValueChanged(null, EventArgs.Empty);

                    //透視自動スキャン位置移動にて、微調テーブル移動でエラーが発生している場合 ここからby 山影
                    //if (IsAutoScanPosTransMode && (!string.IsNullOrEmpty(buf)))
                    if (!string.IsNullOrEmpty(buf))
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
                            //Rev23.30 by長野 2016/03/14
                            cwbtnMove.Enabled = false;

                            return;
                        }
                    }
                    //ここまで by 山影
                }

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

                    //Rev26.00 add by chouno 2017/01/17
                    //Rev26.14 修正 by chouno 2018/09/10
                    bool MoveFDDBeforeMoveXY = (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value > (decimal)ScanCorrect.GVal_Fcd);
                    //試料テーブルを後退させる場合、I.I.を移動させてから試料テーブルを移動させる
                    if (MoveFDDBeforeMoveXY)
                    {
                        //FID
                        if (ntbMove[(int)MECHA_MOVE.MOVE_FID].TextBackColor != Color.Lime)
                        {
                            //指定FID位置までI.I.を移動させる
                            //2014/11/07hata キャストの修正
                            //if (!modSeqComm.MoveFID((int)ntbMove[(int)MECHA_MOVE.MOVE_FID].Value * 10))
                            //if (!modSeqComm.MoveFID(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_FID].Value * 10)))
                            if (!modSeqComm.MoveFID(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_FID].Value * modMechaControl.GVal_FDD_SeqMagnify)))//Rev23.10 変更 by長野 2015/09/18
                            {
                                //エラーの場合：指定されたFID位置までI.I.を移動させることができませんでした。
                                buf = buf + "* " + StringTable.BuildResStr(StringTable.IDS_MoveErr, StringTable.IDS_FID, StringTable.IDS_II) + "\r";
                            }
                        }
                    }

                    //Rev20.00 一度目の移動先がY軸ストローク最大、かつ、限界FCDの内側の場合は、直接2回目の移動を行う by長野 2015/03/23
                    //if (!((float)ntbFCD1st.Value < CTSettings.GVal_FcdLimit && (float)ntbY1st.Value == CTSettings.t20kinf.Data.y_axis_upper_limit))
                    //Rev25.10 change by chouno 2017/09/11
                    if (!((float)ntbFCD1st.Value < modSeqComm.GetFCDLimit() && (float)ntbY1st.Value == CTSettings.t20kinf.Data.y_axis_upper_limit))
                    {
                        if (!MoveFcdAndY((float)ntbFCD1st.Value, (float)ntbY1st.Value))
                        {
                            //エラーの場合：指定されたFCD/Y軸位置まで試料テーブルを移動させることができませんでした。
                            //               buf = "* " & "指定されたFCD/Y軸位置まで試料テーブルを移動させることができませんでした。" & vbCr
                            buf = "* " + CTResources.LoadResString(20051) + "\r";		//ストリングテーブル化 'v17.60 by 長野 2011/05/22
                        }
                    }

                    //次のMoveFcdAndY内の処理でメカ位置を正しく取得したいため メカステータス更新 by chouno 2018/09/10
                    frmMechaControl.Instance.tmrMecainfSeqCommEx();

                    if (!MoveFcdAndY((float)ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value, (float)ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value))
                    {
                        //エラーの場合：指定されたFCD/Y軸位置まで試料テーブルを移動させることができませんでした。
                        //buf = "* " & "指定されたFCD/Y軸位置まで試料テーブルを移動させることができませんでした。" & vbCr
                        buf = "* " + CTResources.LoadResString(20051) + "\r";		//ストリングテーブル化 'v17.60 by 長野 2011/05/22
                    }

                    //Rev26.14 修正 by chouno 2018/09/10
                    //試料テーブルを後退させる場合、I.I.を移動させてから試料テーブルを移動させる
                    //change Rev26.40 by chouno 2019/02/17
                    if (!MoveFDDBeforeMoveXY && !IsInLimitFCDandFDD)
                    {
                        //FID
                        if (ntbMove[(int)MECHA_MOVE.MOVE_FID].TextBackColor != Color.Lime)
                        {
                            //指定FID位置までI.I.を移動させる
                            //2014/11/07hata キャストの修正
                            //if (!modSeqComm.MoveFID((int)ntbMove[(int)MECHA_MOVE.MOVE_FID].Value * 10))
                            //if (!modSeqComm.MoveFID(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_FID].Value * 10)))
                            if (!modSeqComm.MoveFID(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_FID].Value * modMechaControl.GVal_FDD_SeqMagnify)))//Rev23.10 変更 by長野 2015/09/18
                            {
                                //エラーの場合：指定されたFID位置までI.I.を移動させることができませんでした。
                                buf = buf + "* " + StringTable.BuildResStr(StringTable.IDS_MoveErr, StringTable.IDS_FID, StringTable.IDS_II) + "\r";
                            }
                        }
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
                    if (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].TextBackColor != Color.Lime)
                    {
                        //2014/11/07hata キャストの修正
                        //if (!modSeqComm.MoveXpos((int)ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value * 100))
                        //if (!modSeqComm.MoveXpos(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value * 100)))
                        if (!modSeqComm.MoveXpos(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value * modMechaControl.GVal_TableX_SeqMagnify)))//Rev23.10 変更 by長野 2015/09/18
                        {
                            //エラーの場合：指定されたY軸位置まで試料テーブルを移動させることができませんでした。
                            buf = buf + "* " + StringTable.GetResString(StringTable.IDS_MoveErr, ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Caption, CTResources.LoadResString(StringTable.IDS_SampleTable)) + "\r";
                        }
                    }

                    //試料テーブルを後退させる場合、I.I.を移動させてから試料テーブルを移動させる
                    //Rev26.14 修正 by chouno 2018/09/10
                    //if ((ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value > (decimal)ScanCorrect.GVal_Fcd))
                    bool MoveFDDBeforeMoveXY = (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value > (decimal)ScanCorrect.GVal_Fcd);
                    //if (MoveFDDBeforeMoveXY)
                    //change Rev26.40 by chouno 2019/02/17
                    if (MoveFDDBeforeMoveXY && !IsInLimitFCDandFDD)
                    {
                        //FID
                        if (ntbMove[(int)MECHA_MOVE.MOVE_FID].TextBackColor != Color.Lime)
                        {
                            //指定FID位置までI.I.を移動させる
                            //2014/11/07hata キャストの修正
                            //if (!modSeqComm.MoveFID((int)ntbMove[(int)MECHA_MOVE.MOVE_FID].Value * 10))
                            //if (!modSeqComm.MoveFID(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_FID].Value * 10)))
                            if (!modSeqComm.MoveFID(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_FID].Value * modMechaControl.GVal_FDD_SeqMagnify)))//Rev23.10 変更 by長野 2015/09/18
                            {
                                //エラーの場合：指定されたFID位置までI.I.を移動させることができませんでした。
                                buf = buf + "* " + StringTable.BuildResStr(StringTable.IDS_MoveErr, StringTable.IDS_FID, StringTable.IDS_II) + "\r";
                            }
                        }

                        //FCD
                        if (ntbMove[(int)MECHA_MOVE.MOVE_FCD].TextBackColor != Color.Lime)
                        {
                            //指定FCD位置まで試料テーブルを移動させる
                            //2014/11/07hata キャストの修正
                            //if (!modSeqComm.MoveFCD((int)ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value * 10))
                            //if (!modSeqComm.MoveFCD(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value * 10)))
                            if (!modSeqComm.MoveFCD(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value * modMechaControl.GVal_FCD_SeqMagnify)))//Rev23.10 変更 by長野 2015/09/18
                            {
                                //エラーの場合：指定されたFCD位置まで試料テーブルを移動させることができませんでした。
                                buf = buf + "* " + StringTable.BuildResStr(StringTable.IDS_MoveErr, StringTable.IDS_FCD, StringTable.IDS_SampleTable) + "\r";
                            }
                        }
                    }
                    else
                    {
                        //FCD
                        if (ntbMove[(int)MECHA_MOVE.MOVE_FCD].TextBackColor != Color.Lime)
                        {
                            //指定FCD位置まで試料テーブルを移動させる
                            //2014/11/07hata キャストの修正
                            //if (!modSeqComm.MoveFCD((int)modLibrary.MaxVal(modLibrary.MinVal(CTSettings.GVal_FcdLimit, ScanCorrect.GVal_Fcd), (float)ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value) * 10))
                            //if (!modSeqComm.MoveFCD(Convert.ToInt32(modLibrary.MaxVal(modLibrary.MinVal(CTSettings.GVal_FcdLimit, ScanCorrect.GVal_Fcd), (float)ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value) * 10)))
                            //if (!modSeqComm.MoveFCD(Convert.ToInt32(modLibrary.MaxVal(modLibrary.MinVal(CTSettings.GVal_FcdLimit, ScanCorrect.GVal_Fcd), (float)ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value) * modMechaControl.GVal_FCD_SeqMagnify))) //Rev23.10 変更 by長野 2015/09/18
                            //Rev25.10 change by chouno 2017/09/11
                            if (!modSeqComm.MoveFCD(Convert.ToInt32(modLibrary.MaxVal(modLibrary.MinVal(modSeqComm.GetFCDLimit(), ScanCorrect.GVal_Fcd), (float)ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value) * modMechaControl.GVal_FCD_SeqMagnify))) //Rev23.10 変更 by長野 2015/09/18
                            {
                                //エラーの場合：指定されたFCD位置まで試料テーブルを移動させることができませんでした。
                                buf = buf + "* " + StringTable.BuildResStr(StringTable.IDS_MoveErr, StringTable.IDS_FCD, StringTable.IDS_SampleTable) + "\r";
                            }
                        }

                        //FID
                        if (ntbMove[(int)MECHA_MOVE.MOVE_FID].TextBackColor != Color.Lime)
                        {
                            //指定FID位置までI.I.を移動させる
                            //2014/11/07hata キャストの修正
                            //if (!modSeqComm.MoveFID((int)(ntbMove[(int)MECHA_MOVE.MOVE_FID].Value * 10)))
                            //if (!modSeqComm.MoveFID(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_FID].Value * 10)))
                            if (!modSeqComm.MoveFID(Convert.ToInt32(ntbMove[(int)MECHA_MOVE.MOVE_FID].Value * modMechaControl.GVal_FDD_SeqMagnify)))//Rev23.10 変更 by長野 2015/09/18
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

                //Rev25.03 by chouno 2017/02/23
                //if (!IsAutoScanPosTransMode)
                //条件変更 Rev26.40 by chouno 2019/03/08
                if (!IsAutoScanPosTransMode && !IsAutoScanPosCamera)
                {
                    //移動 ここから by山影
                    //昇降
                    if (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].TextBackColor != Color.Lime)
                    {
                        if (modMechaControl.MechaUdIndex((float)ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Value) != 0)
                        {
                            //エラーの場合：指定された昇降位置まで試料テーブルを移動させることができませんでした。
                            buf = buf + "* " + StringTable.GetResString(StringTable.IDS_MoveErr, ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Caption, CTResources.LoadResString(StringTable.IDS_SampleTable)) + "\r";
                        }
                    }
                }
            }

			//v19.12 windows7対策 タイマーによる更新が遅くなるため、更新処理をここで明示的に呼ぶ by長野 2013/03/08
			frmMechaControl.MyUpdate();
			frmMechaControl.UpdateMecha();

            //点滅を止める
            cwbtnMove.Value = false;
            cwbtnMove.FlatStyle = FlatStyle.Standard;
            //cwbtnMove.OnColor = SystemColors.ButtonFace;
            //cwbtnMove.BlinkInterval = CWSpeeds.cwSpeedOff;

            //変更2014/10/07hata_v19.51反映
            //産業用CTモードの場合       'v18.00追加 byやまおか 2011/06/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
			if ((CTSettings.scaninh.Data.avmode == 0)) {

				//エラーメッセージ表示
                if ((!string.IsNullOrEmpty(buf)))
                {
                    //エラーメッセージ表示
                    //Interaction.MsgBox(buf, MsgBoxStyle.Exclamation);
                    MessageBox.Show(buf, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

				//完了にする
				MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Done;

				//マイクロCTの場合
			} else {

                //エラーメッセージが存在する場合
			    if (!string.IsNullOrEmpty(buf))
			    {
                    //Rev20.01 変更 エラーが出ている場合は、主導モードではなく完了に切り替える by長野 2015/06/03
                    ////手動移動モードに切り替え
                    //MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Ready;
                
				    //エラーメッセージ表示
				    MessageBox.Show(buf, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    //完了に切り替え
                    MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Done;

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
            }

            return;

//エラー時の扱い
ErrorHandler:
            
            //点滅を止める
            cwbtnMove.Value = false;
            cwbtnMove.FlatStyle = FlatStyle.Standard;
            //cwbtnMove.OnColor = SystemColors.ButtonFace;
            //cwbtnMove.BlinkInterval = CWSpeeds.cwSpeedOff;
            
			//モードを元に戻す
			MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Ready;

			//メッセージの表示
			modCT30K.ErrMessage(err_sts);
		}

        ///指定FCDと指定FDDが限界FCD内にいるときに、手動移動後にFDDを動かす
        private void MoveOnlyFDD()
        {
 
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
            //dX = Convert.ToSingle(targetFCD.ToString("0.00")) - ScanCorrect.GVal_Fcd;
            //dY = targetY - (float)modSeqComm.MySeq.stsXPosition / 100f;
            
            //dX = (float)Math.Round((Convert.ToSingle(targetFCD.ToString("0.0")) - ScanCorrect.GVal_Fcd), 1, MidpointRounding.AwayFromZero);
            //dY = (float)Math.Round(targetY - (float)modSeqComm.MySeq.stsXPosition / 100f, 2, MidpointRounding.AwayFromZero);
            dX = targetFCD - ScanCorrect.GVal_Fcd;
            //dY = targetY - (float)modSeqComm.MySeq.stsXPosition / 100f;
            //dY = targetY - (float)modSeqComm.MySeq.stsXPosition / (float)modMechaControl.GVal_TableX_SeqMagnify;//Rev23.10 変更 by長野 2015/09/18
            if (CTSettings.scaninh.Data.cm_mode == 1)
            {
                dY = targetY - (float)modSeqComm.MySeq.stsXPosition / (float)modMechaControl.GVal_TableX_SeqMagnify;//Rev23.10 変更 by長野 2015/09/18
            }
            else
            {
                dY = targetY - (float)modSeqComm.MySeq.stsLinearTableY / (float)modMechaControl.GVal_TableX_SeqMagnify;//Rev23.10 変更 by長野 2015/09/18
            }

            Debug.WriteLine("dx = " + dX.ToString());
            Debug.WriteLine("dY = " + dY.ToString());

			//限界FCD値を考慮した移動位置
			float FCD1st = 0;
            //FCD1st = modLibrary.MaxVal(modLibrary.MinVal(CTSettings.GVal_FcdLimit, ScanCorrect.GVal_Fcd), targetFCD);
            //Rev25.10 change by chouno 2017/09/11
            FCD1st = modLibrary.MaxVal(modLibrary.MinVal(modSeqComm.GetFCDLimit(), ScanCorrect.GVal_Fcd), targetFCD);

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

            //Debug.WriteLine("setFCD = " + ((int)(FCD1st * 10)).ToString());
            //Debug.WriteLine("setYpos = " + (modSeqComm.MySeq.stsXPosition + (int)(dy1st * 100)).ToString());

            //int setFCD = (int)(Math.Round(FCD1st, 1, MidpointRounding.AwayFromZero) *10);
            //int setY =  modSeqComm.MySeq.stsXPosition + (int)(Math.Round(dy1st, 2, MidpointRounding.AwayFromZero) *100);

            //Rev26.14 修正 by chouno 2018/09/04
            int setFCD = 0;
            int setY = 0;
            if (CTSettings.scaninh.Data.cm_mode == 1)
            {
                setFCD = (int)(Math.Round(FCD1st, 1, MidpointRounding.AwayFromZero) * modMechaControl.GVal_FCD_SeqMagnify);//Rev23.10 変更 by長野 2015/09/18
                setY = modSeqComm.MySeq.stsXPosition + (int)(Math.Round(dy1st, 2, MidpointRounding.AwayFromZero) * modMechaControl.GVal_TableX_SeqMagnify);//Rev23.10 変更 by長野 2015/09/18
            }
            else
            {
                setFCD = (int)(Math.Round(FCD1st, 2, MidpointRounding.AwayFromZero) * modMechaControl.GVal_FCD_SeqMagnify);//Rev23.10 変更 by長野 2015/09/18
                setY = modSeqComm.MySeq.stsLinearTableY + (int)(Math.Round(dy1st, 2, MidpointRounding.AwayFromZero) * modMechaControl.GVal_TableX_SeqMagnify);//Rev23.10 変更 by長野 2015/09/18
            }

            Debug.WriteLine("setFCD = " + setFCD.ToString());
            Debug.WriteLine("setYpos = " + setY.ToString());



            //指定FCD位置、Y軸位置まで試料テーブルを移動させる
			//functionReturnValue = modSeqComm.MoveXY((int)(FCD1st * 10), modSeqComm.MySeq.stsXPosition + (int)(dy1st * 100));
			functionReturnValue = modSeqComm.MoveXY(setFCD, setY);

			//目標値に達したら該当するコントロールの背景を緑にする   'v17.10追加 byやまおか 2010/08/20
            //変更2014/09/20(検S1)hata
            //ntbMove[(int)MECHA_MOVE.MOVE_FCD].BackColor = (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value == frmMechaControl.Instance.ntbFCD.Value ? Color.Lime : SystemColors.Control);
            //ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].BackColor = (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value == frmMechaControl.Instance.ntbTableXPos.Value ? System.Drawing.Color.Lime : SystemColors.Control);
            ntbMove[(int)MECHA_MOVE.MOVE_FCD].TextBackColor = (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value == frmMechaControl.Instance.ntbFCD.Value ? Color.Lime : SystemColors.Control);
			ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].TextBackColor = (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value == frmMechaControl.Instance.ntbTableXPos.Value ? System.Drawing.Color.Lime : SystemColors.Control);
			
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
            //変更2014/09/20(検S1)hata
            //return (ntbMove[(int)MECHA_MOVE.MOVE_FCD].BackColor != Color.Lime) &&
            //      (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value < (decimal)CTSettings.GVal_FcdLimit) &&
            //      (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value < (decimal)ScanCorrect.GVal_Fcd);
            return (ntbMove[(int)MECHA_MOVE.MOVE_FCD].TextBackColor != Color.Lime) &&
                //(ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value < (decimal)CTSettings.GVal_FcdLimit) &&
                //Rev25.10 change by chouno 2017/09/11
                   (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value < (decimal)(modSeqComm.GetFCDLimit())) &&
                   (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value < (decimal)ScanCorrect.GVal_Fcd);
		}

        //*************************************************************************************************
        //機　　能： １回目のテーブル移動位置がFDD側限界を越えて近づけようとするつもりか？
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v23.30  2016/02/23 (検S1)長野  新規作成
        //*************************************************************************************************
        //private bool IsTryOverFCD1_FDDLimit()
        private bool IsTryOverFCD1_FDDLimit(float TargetFid)//Rev26.00 change by chouno 2017/10/16
        {
            //変更2014/09/20(検S1)hata
            //return (ntbMove[(int)MECHA_MOVE.MOVE_FCD].BackColor != Color.Lime) &&
            //      (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value < (decimal)CTSettings.GVal_FcdLimit) &&
            //      (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value < (decimal)ScanCorrect.GVal_Fcd);

            float fidlimit = 0;
            //if ((float)ScanCorrect.GVal_Fid > (float)CTSettings.mechapara.Data.max_fdd)
            if ((float)TargetFid > (float)CTSettings.mechapara.Data.max_fdd)//Rev26.00 change by chouno 2017/10/16
            {
                fidlimit = (float)CTSettings.mechapara.Data.max_fdd;
            }
            else
            {
                //fidlimit = ScanCorrect.GVal_Fid;
                fidlimit = TargetFid;//Rev26.00 change by chouno 2017/10/16
            }

            return ((ntbMove[(int)MECHA_MOVE.MOVE_FCD].TextBackColor != Color.Lime) &&
                   (((decimal)ntbFCD1st.Value + (decimal)CTSettings.Gval_BetMaxFcdAndFdd) > (decimal)fidlimit));
        }

        //*************************************************************************************************
        //機　　能： 2回目のテーブル移動位置がFDD側限界を越えて近づけようとするつもりか？
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v23.30  2016/02/23 (検S1)長野  新規作成
        //*************************************************************************************************
        //private bool IsTryOverFCD2_FDDLimit()
        private bool IsTryOverFCD2_FDDLimit(float TargetFid)//Rev26.00 change by chouno 2017/10/16
        {
            //変更2014/09/20(検S1)hata
            //return (ntbMove[(int)MECHA_MOVE.MOVE_FCD].BackColor != Color.Lime) &&
            //      (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value < (decimal)CTSettings.GVal_FcdLimit) &&
            //      (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value < (decimal)ScanCorrect.GVal_Fcd);
            float fidlimit = 0;
            //if ((float)ScanCorrect.GVal_Fid > (float)CTSettings.mechapara.Data.max_fdd)
            if ((float)TargetFid > (float)CTSettings.mechapara.Data.max_fdd) //Rev26.00 change by chouno 2017/10/16
            {
                fidlimit = (float)CTSettings.mechapara.Data.max_fdd;
            }
            else
            {
                //fidlimit = ScanCorrect.GVal_Fid;
                fidlimit = TargetFid; //Rev26.00 change by chouno 2017/10/16
            }
            return ((ntbMove[(int)MECHA_MOVE.MOVE_FCD].TextBackColor != Color.Lime) &&
                   (((decimal)ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value + (decimal)CTSettings.Gval_BetMaxFcdAndFdd) > (decimal)fidlimit));
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
            //変更2014/09/20(検S1)hata
            //ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].BackColor = (ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value == frmMechaControl.Instance.ntbFTablePosX.Value ? Color.Lime : SystemColors.Control);
            ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].TextBackColor = (ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value == frmMechaControl.Instance.ntbFTablePosX.Value ? Color.Lime : SystemColors.Control);

			//移動がすべて完了した終了処理へ
			//if (IsAllDone()) MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Done;
            if (IsAllDone())
            {
                MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Done;

                //点滅を止める
                cwbtnMove.Value = false;
                cwbtnMove.FlatStyle = FlatStyle.Standard;
                //cwbtnMove.OnColor = SystemColors.ButtonFace;
                //cwbtnMove.BlinkInterval = CWSpeeds.cwSpeedOff;
           
            }
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
            //変更2014/09/20(検S1)hata
            //ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].BackColor = (ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value == frmMechaControl.Instance.ntbFTablePosY.Value ? Color.Lime : SystemColors.Control);
            ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].TextBackColor = (ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value == frmMechaControl.Instance.ntbFTablePosY.Value ? Color.Lime : SystemColors.Control);

			//移動がすべて完了した終了処理へ
			//if (IsAllDone()) MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Done;
            if (IsAllDone())
            {
                MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Done;

                //点滅を止める
                cwbtnMove.Value = false;
                cwbtnMove.FlatStyle = FlatStyle.Standard;
                //cwbtnMove.OnColor = SystemColors.ButtonFace;
                //cwbtnMove.BlinkInterval = CWSpeeds.cwSpeedOff;
            
            }
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
            //変更2014/09/20(検S1)hata
            //ntbMove[(int)MECHA_MOVE.MOVE_FCD].BackColor = (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value == (decimal)ScanCorrect.GVal_Fcd ? Color.Lime : SystemColors.Control);
            ntbMove[(int)MECHA_MOVE.MOVE_FCD].TextBackColor = (ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value == (decimal)ScanCorrect.GVal_Fcd ? Color.Lime : SystemColors.Control);

			//移動がすべて完了した終了処理へ
			if (IsAllDone())
            {
                MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Done;

                //点滅を止める
                cwbtnMove.Value = false;
                cwbtnMove.FlatStyle = FlatStyle.Standard;
                //cwbtnMove.OnColor = SystemColors.ButtonFace;
                //cwbtnMove.BlinkInterval = CWSpeeds.cwSpeedOff;
            }

            //Rev26.40 指定FCDと指定FDDが限界FCD内に入っているとき、手動移動を先に行い、最後にFDD移動を行う。
            if (MechaMoveMode == MechaMoveModeConstants.MechaMoveMode_ManualMove && ntbMove[(int)MECHA_MOVE.MOVE_FCD].TextBackColor == Color.Lime)
            {
                MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_MoveFDD;
            }
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
            //変更2014/09/20(検S1)hata
            //ntbMove[(int)MECHA_MOVE.MOVE_FID].BackColor = (ntbMove[(int)MECHA_MOVE.MOVE_FID].Value == (decimal)ScanCorrect.GVal_Fid ? Color.Lime : SystemColors.Control);
            ntbMove[(int)MECHA_MOVE.MOVE_FID].TextBackColor = (ntbMove[(int)MECHA_MOVE.MOVE_FID].Value == (decimal)ScanCorrect.GVal_Fid ? Color.Lime : SystemColors.Control);

			//移動がすべて完了した終了処理へ
			if (IsAllDone())
            {
                MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Done;

                //点滅を止める
                cwbtnMove.Value = false;
                cwbtnMove.FlatStyle = FlatStyle.Standard;
                //cwbtnMove.OnColor = SystemColors.ButtonFace;
                //cwbtnMove.BlinkInterval = CWSpeeds.cwSpeedOff;
            
            }
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
            //変更2014/09/20(検S1)hata
            //ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].BackColor = (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value == frmMechaControl.Instance.ntbTableXPos.Value ? Color.Lime : SystemColors.Control);
            ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].TextBackColor = (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Y].Value == frmMechaControl.Instance.ntbTableXPos.Value ? Color.Lime : SystemColors.Control);

			//移動がすべて完了した終了処理へ
            if (IsAllDone()) 
            {
                MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Done;
            
                //点滅を止める
                cwbtnMove.Value = false;
                cwbtnMove.FlatStyle = FlatStyle.Standard;
                //cwbtnMove.OnColor = SystemColors.ButtonFace;
                //cwbtnMove.BlinkInterval = CWSpeeds.cwSpeedOff;
            
            }
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
            //変更2014/09/20(検S1)hata
            //ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].BackColor = (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Value == frmMechaControl.Instance.ntbUpDown.Value ? Color.Lime : SystemColors.Control);
            ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].TextBackColor = (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Value == frmMechaControl.Instance.ntbUpDown.Value ? Color.Lime : SystemColors.Control);

			//移動がすべて完了した終了処理へ
            if (IsAllDone())
            {
                MechaMoveMode = MechaMoveModeConstants.MechaMoveMode_Done;

                //点滅を止める
                cwbtnMove.Value = false;
                cwbtnMove.FlatStyle = FlatStyle.Standard;
                //cwbtnMove.OnColor = SystemColors.ButtonFace;
                //cwbtnMove.BlinkInterval = CWSpeeds.cwSpeedOff;
            
            }
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
                //変更2014/09/20(検S1)hata
                //if (ntbMove[i].BackColor == SystemColors.Control) return functionReturnValue;
                //Rev20.00 条件式変更 各軸のテキストボックスの初期値(SystemColors.Window)にも対応させる。
                //if (ntbMove[i].TextBackColor == SystemColors.Control) return functionReturnValue;
                if ((ntbMove[i].TextBackColor == SystemColors.Control) || (ntbMove[i].TextBackColor == SystemColors.Window)) return functionReturnValue;

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

            //Rev26.00 add by chouno
            //微調機能OFF、もしくは、回転大テーブルがついている場合は、微調の移動が不可なため、
            //現在値で計算を進める
            //if ((CTSettings.scaninh.Data.fine_table != 0) ||
            //    ((CTSettings.mecainf.Data.ystg_l_limit == 1) && (CTSettings.mecainf.Data.ystg_u_limit == 1) && (CTSettings.mecainf.Data.xstg_l_limit == 1) && (CTSettings.mecainf.Data.xstg_u_limit == 1)) ||
            //    (modSeqComm.MySeq.stsRotLargeTable == true))
            //Rev26.20 add by chouno 2019/02/06
            if ((CTSettings.scaninh.Data.fine_table != 0) ||
                ((CTSettings.mecainf.Data.ystg_l_limit == 1) && (CTSettings.mecainf.Data.ystg_u_limit == 1) && (CTSettings.mecainf.Data.xstg_l_limit == 1) && (CTSettings.mecainf.Data.xstg_u_limit == 1)) ||
                ((modSeqComm.MySeq.stsRotLargeTable == true) && CTSettings.t20kinf.Data.ftable_type == 0))

            {
                PosFy = CTSettings.mecainf.Data.xstg_pos;
                PosFx = CTSettings.mecainf.Data.ystg_pos;
            }

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
			ntbFCD1st.Value =Convert.ToDecimal(FCD1 - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()]);
			ntbY1st.Value = (decimal)PosY1;

            //Rev23.40 by長野 2016/04/05 Rev23.21 by長野 2016/03/10
            float TargetUdPos = 0.0f;
            float TargetFCD = 0.0f;
            TargetUdPos = (float)frmMechaControl.Instance.ntbUpDown.Value; //断面位置指定のため、移動先の昇降位置は現在と同じ
            TargetFCD = FCD2 - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()];
            if (!modMechaControl.chkTablePosByAutoPos(TargetUdPos, TargetFCD))
            {
                //メッセージの表示：移動後のテーブル昇降位置が、干渉エリア内での制限位置を越えるため、処理を中止します。
                //MsgBox LoadResString(IDS_CorReadyAlready), vbCritical
                MessageBox.Show(CTResources.LoadResString(24100), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();

                return functionReturnValue;
            }

			//移動処理（ここでモーダル表示）
            //MechaMove(PosFx, PosFy, FCD2 - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()], y: PosY2);
            MechaMove(Convert.ToDecimal(PosFx), Convert.ToDecimal(PosFy), FCD2 - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()], y: Convert.ToDecimal(PosY2));

			//戻り値設定
			functionReturnValue = IsOK;
			return functionReturnValue;

//エラー時の扱い
        ErrorHandler:
            //点滅を止める
            cwbtnMove.Value = false;
            cwbtnMove.FlatStyle = FlatStyle.Standard;
            //cwbtnMove.OnColor = SystemColors.ButtonFace;
            //cwbtnMove.BlinkInterval = CWSpeeds.cwSpeedOff;
            
			//このフォームのアンロード
			this.Close();

			//メッセージの表示
			modCT30K.ErrMessage(err_sts);

			return functionReturnValue;
		}

        //*************************************************************************************************
        //機　　能： メカ移動処理用ダイアログを生成・表示（自動スキャン位置移動用(外観カメラ)）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        public bool MechaMoveForAutoScanPos_ExObsCam(int xc, int yc, int r,int matrix,float pixsize)
        {
            bool functionReturnValue = false;

            int err_sts = 0;
            float FCD1 = 0;
            float FCD2 = 0;
            float PosY1 = 0;
            float PosY2 = 0;

            //Rev26.20 外観カメラ固定方法によって、計算に使用するFCD,FDDを変える by chouno 2019/02/12
            float targetFDD = modMechaControl.FixedCamType == 1 ? modMechaControl.BackUpFDD + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube] : CTSettings.scansel.Data.fid;
            float targetFCD = modMechaControl.FixedCamType == 1 ? modMechaControl.BackUpFCD + CTSettings.scancondpar.Data.fcd_offset[ScanCorrect.GFlg_MultiTube] : CTSettings.scansel.Data.fcd;

            #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
            /*
			'フォームをロード
			Load Me
*/
            #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            //画像断面からの自動スキャン位置移動モードにする
            //IsAutoScanPosMode = true;
            IsAutoScanPosCamera = true;//外観カメラ用の変数に変更 by chouno 2019/03/08

            //Ｘ軸・Ｙ軸を同時に移動させる
            chkXYMove.CheckState = CheckState.Checked;

            //ROI設定
            RoiCircle[0] = xc;
            RoiCircle[1] = yc;
            RoiCircle[2] = r;

            //自動スキャン位置モード設定

            //最適な微調テーブル座標を求める
            err_sts = ScanCorrect.auto_ftbl_set_byExObsCam(ref RoiCircle[0],matrix,pixsize, ref PosFy, ref PosFx);
            if (err_sts != 0) goto ErrorHandler;

            ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value = (decimal)PosFx;
            ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value = (decimal)PosFy;

            //最適な微調テーブル座標に移動したと仮定して、最適な試料テーブルのXY座標を求める
            //err_sts = ScanCorrect.auto_tbl_set_byExObsCam(ref RoiCircle[0],
            //                                    matrix,
            //                                    pixsize,
            //                                    PosFy, PosFx,
            //                                    (float)ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value,
            //                                    (float)ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value,
            //                                    CTSettings.scansel.Data.fcd, CTSettings.scansel.Data.fid,//Rev26.00 add by chouno 2017/01/17
            //                                    ref FCD1, ref PosY1,
            //                                    ref FCD2, ref PosY2);

            err_sts = ScanCorrect.auto_tbl_set_byExObsCam(ref RoiCircle[0],
                                    matrix,
                                    pixsize,
                                    PosFy, PosFx,
                                    (float)ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value,
                                    (float)ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value,
                                    targetFCD, targetFDD,//Rev26.20 計算に使用するFCD,FDDを外観カメラ固定方式に従い変更 by chouno 2019/02/12
                                    ref FCD1, ref PosY1,
                                    ref FCD2, ref PosY2);

            if (err_sts != 0) goto ErrorHandler;

            //自動スキャン位置移動用移動第１段階ＦＣＤ・Ｙ軸座標を記憶
            ntbFCD1st.Value = Convert.ToDecimal(FCD1 - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()]);
            ntbY1st.Value = (decimal)PosY1;

            //Rev23.40 by長野 2016/04/05 Rev23.21 by長野 2016/03/10
            float TargetUdPos = 0.0f;
            float TargetFCD = 0.0f;

            //TargetUdPos = (float)frmExObsCam.Instance.beforeAutoPosTableUD; //外観カメラは処理開始前の昇降位置
            //change by chouno 2019/02/12
            TargetUdPos = (float)modMechaControl.BackUpTableZ; //外観カメラは処理開始前の昇降位置

            TargetFCD = FCD2 - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()];
            if (!modMechaControl.chkTablePosByAutoPos(TargetUdPos, TargetFCD))
            {
                //メッセージの表示：移動後のテーブル昇降位置が、干渉エリア内での制限位置を越えるため、処理を中止します。
                //MsgBox LoadResString(IDS_CorReadyAlready), vbCritical
                MessageBox.Show(CTResources.LoadResString(24100), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();

                return functionReturnValue;
            }

            //移動処理（ここでモーダル表示）
            //MechaMove(PosFx, PosFy, FCD2 - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()], y: PosY2);
            //MechaMove(Convert.ToDecimal(PosFx), Convert.ToDecimal(PosFy), FCD2 - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()], y: Convert.ToDecimal(PosY2),z:(decimal)frmExObsCam.Instance.beforeAutoPosTableUD);

            //移動処理（ここでモーダル表示）
            //MechaMove(PosFx, PosFy, FCD2 - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()], y: PosY2);
            //change by chouno 2019/02/12
            MechaMove(Convert.ToDecimal(PosFx), Convert.ToDecimal(PosFy), FCD2 - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()], y: Convert.ToDecimal(PosY2), z: (decimal)modMechaControl.BackUpTableZ,Fid:modMechaControl.BackUpFDD);

            //戻り値設定
            functionReturnValue = IsOK;
            return functionReturnValue;

//エラー時の扱い
        ErrorHandler:
            //点滅を止める
            cwbtnMove.Value = false;
            cwbtnMove.FlatStyle = FlatStyle.Standard;
            //cwbtnMove.OnColor = SystemColors.ButtonFace;
            //cwbtnMove.BlinkInterval = CWSpeeds.cwSpeedOff;

            //このフォームのアンロード
            this.Close();

            //メッセージの表示
            modCT30K.ErrMessage(err_sts);

            return functionReturnValue;
        }

        //*************************************************************************************************
        //機　　能： メカ移動処理用ダイアログを生成・表示（[ガイド]タブ スキャンエリア位置移動用(外観カメラ)）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V26.00  16/12/27  (検S1)長野    新規作成
        //*************************************************************************************************
        public bool MechaMoveForAutoScanPos_ScanAreaSetForGuide(int xc, int yc, int r, int matrix, float pixsize, float fdd, float tableZ)
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
            err_sts = ScanCorrect.auto_ftbl_set_byExObsCam(ref RoiCircle[0], matrix, pixsize, ref PosFy, ref PosFx);
            if (err_sts != 0) goto ErrorHandler;

            //ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value = (decimal)PosFx;
            //ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value = (decimal)PosFy;

            PosFy = 0.00f;
            PosFx = 0.00f;

            ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value = (decimal)0.00;
            ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value = (decimal)0.00;

            //スキャンエリア設定では、FDDをiniの値でする。そのため、現在位置ではなく、
            //FDD→iniの値、FCDが試料テーブル込でFDD以上とならない位置で、スキャンエリアを満たす最適計算を計算
            float targetFDD = fdd;
            float targetFCD = ScanCorrect.GVal_Fcd;
            float fidlimit = 0.0f;

            if ((float)targetFDD > (float)CTSettings.mechapara.Data.max_fdd)
            {
                fidlimit = (float)CTSettings.mechapara.Data.max_fdd;
            }
            else
            {
                fidlimit = targetFDD;
            }
            if (fidlimit < targetFCD + CTSettings.Gval_BetMaxFcdAndFdd)
            {
                //targetFCD = ScanCorrect.GVal_Fid - CTSettings.Gval_BetMaxFcdAndFdd;
                //Rev26.14 修正 by chouno 2018/09/10
                targetFCD = fidlimit - CTSettings.Gval_BetMaxFcdAndFdd;
            }

            //最適な微調テーブル座標に移動したと仮定して、最適な試料テーブルのXY座標を求める
            err_sts = ScanCorrect.auto_tbl_set_byExObsCam(ref RoiCircle[0],
                                                matrix,
                                                pixsize,
                                                PosFy, PosFx,
                                                (float)ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value,
                                                (float)ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value,
                                                targetFCD + CTSettings.scancondpar.Data.fcd_offset[ScanCorrect.GFlg_MultiTube], targetFDD + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube],//Rev26.00 add by chouno 2017/01/17
                                                ref FCD1, ref PosY1,
                                                ref FCD2, ref PosY2);
            if (err_sts != 0) goto ErrorHandler;

            //自動スキャン位置移動用移動第１段階ＦＣＤ・Ｙ軸座標を記憶
            ntbFCD1st.Value = Convert.ToDecimal(FCD1 - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()]);
            ntbY1st.Value = (decimal)PosY1;

            //移動処理（ここでモーダル表示）
            //MechaMove(PosFx, PosFy, FCD2 - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()], y: PosY2);
            MechaMove(ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].Value,
                      ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value,
                      FCD2 - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()],
                      y: Convert.ToDecimal(PosY2),
                      Fid: targetFDD,
                      z: (decimal)tableZ);

            //戻り値設定
            functionReturnValue = IsOK;
            return functionReturnValue;

//エラー時の扱い
        ErrorHandler:
            //点滅を止める
            cwbtnMove.Value = false;
            cwbtnMove.FlatStyle = FlatStyle.Standard;
            //cwbtnMove.OnColor = SystemColors.ButtonFace;
            //cwbtnMove.BlinkInterval = CWSpeeds.cwSpeedOff;

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
		//public bool MechaMoveForAutoScanPos_Trans(int roiXC, int roiYC, int roiXL, int roiYL, short dPixel, float dRefPix, ref short[] beforeImage, ref short[] afterImage)
		public bool MechaMoveForAutoScanPos_Trans(int roiXC, int roiYC, int roiXL, int roiYL, short dPixel, float dRefPix, ref ushort[] beforeImage, ref ushort[] afterImage)
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
			//Load(this);
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//透視画面からの自動スキャン位置移動モードにする
			IsAutoScanPosTransMode = true;

			//Ｘ軸・Ｙ軸を同時に移動させる
			chkXYMove.CheckState = CheckState.Checked;

			//ビニング計算
            //2014/11/07hata キャストの修正
            //kv = (int)(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);
            kv = Convert.ToInt32(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);

			//ROI設定
			modAutoPos.RoiCoordinateTransform(roiXC, roiYC, roiXL, roiYL, ref myRoiRect);

			//画像サイズ
			imageSize.CX = CTSettings.detectorParam.h_size;
			imageSize.CY = CTSettings.detectorParam.v_size;
			//試料テーブル位置
			curTablePos.FCD = CTSettings.scansel.Data.fcd;
			curTablePos.lr =CTSettings.mecainf.Data.table_x_pos;
			curTablePos.ud =CTSettings.mecainf.Data.ud_pos;
			//微調テーブル位置
			curFTpos.x =CTSettings.mecainf.Data.ystg_pos;
			curFTpos.y =CTSettings.mecainf.Data.xstg_pos;
			//検出器
			detectorInfo.FDD = CTSettings.scansel.Data.fid;
			detectorInfo.pitchH = 10 / CTSettings.scancondpar.Data.b[1];
			detectorInfo.pitchV = detectorInfo.pitchH * kv;
			detectorInfo.DetType = Convert.ToInt16(CTSettings.detectorParam.DetType);	//v16.20/v17.00 追加 by 山影 10-03-04

			//テーブル回転角
			rotationAngle =CTSettings.mecainf.Data.rot_pos / 100.0;
			//回転中心位置
			string lblStatus3 = frmScanControl.Instance.lblStatus[3].Text;
			if (lblStatus3 == StringTable.GC_STS_STANDBY_OK || 
				lblStatus3 == StringTable.GC_STS_NORMAL_OK || 
				lblStatus3 == StringTable.GC_STS_CONE_OK)
			{
				rotationPos = CTSettings.scancondpar.Data.xlc[2];
				//If IsLRInverse Then rotationPos = h_size - rotationPos - 1
                if (CTSettings.detectorParam.Use_FlatPanel) rotationPos = CTSettings.detectorParam.h_size - rotationPos - 1;	//v17.50変更 byやまおか 2011/02/27
			}
			else	//回転中心が求まっていない場合、VC内で計算して求める
			{
				rotationPos = -1;
			}

			//スキャン位置
            //2014/11/07hata キャストの修正
            //scanpos = CTSettings.scancondpar.Data.cone_scan_posi_a * imageSize.CX / 2 + CTSettings.scancondpar.Data.cone_scan_posi_b + imageSize.CY / 2;
            scanpos = CTSettings.scancondpar.Data.cone_scan_posi_a * (float)imageSize.CX / 2F + CTSettings.scancondpar.Data.cone_scan_posi_b + imageSize.CY / 2F;
          
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
			ntbFCD1st.Value = (decimal)(optTablePos1st.FCD - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()]);
			ntbY1st.Value = (decimal)optTablePos1st.lr;

			//進捗表示をクリアする   'v17.10追加 byやまおか 2010/08/20
			frmRoiTool.Instance.lblProcess.Text = "";

            Debug.WriteLine("1st = " + optTablePos1st.lr.ToString());
            Debug.WriteLine("2nd = " + optTablePos2nd.lr.ToString());

            //Rev23.21 by長野 2016/03/10
            float TargetUdPos = 0.0f;
            float TargetFCD = 0.0f;
            TargetUdPos = (float)Convert.ToDecimal(myOptUD);
            TargetFCD = optTablePos2nd.FCD - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()];
            if (!modMechaControl.chkTablePosByAutoPos(TargetUdPos, TargetFCD))
            {
                //メッセージの表示：移動後のテーブル昇降位置が、干渉エリア内での制限位置を越えるため、処理を中止します。
                //MsgBox LoadResString(IDS_CorReadyAlready), vbCritical
                MessageBox.Show(CTResources.LoadResString(24100), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();

                return functionReturnValue;
            }

            //移動処理（ここでモーダル表示）
			MechaMove(Convert.ToDecimal(myOptFTPos.x), Convert.ToDecimal(myOptFTPos.y), optTablePos2nd.FCD - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()], y: Convert.ToDecimal(optTablePos2nd.lr), z: Convert.ToDecimal(myOptUD));

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
			//double rotationAngle = 0;													//テーブル回転角
			float scanpos = 0;															//スキャン位置
			int kv = 0;																//ビニング

			//返り値用
			modAutoPos.SampleTable optTablePos1st = default(modAutoPos.SampleTable);
			modAutoPos.SampleTable optTablePos2nd = default(modAutoPos.SampleTable);

			//ビニング計算
            //2014/11/07hata キャストの修正
            //kv = (int)(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);
            kv = Convert.ToInt32(CTSettings.detectorParam.vm / CTSettings.detectorParam.hm);

			//画像サイズ
			imageSize.CX = CTSettings.detectorParam.h_size;
			imageSize.CY = CTSettings.detectorParam.v_size;
			//試料テーブル位置
			curTablePos.FCD = CTSettings.scansel.Data.fcd;
			curTablePos.lr =CTSettings.mecainf.Data.table_x_pos;
			curTablePos.ud =CTSettings.mecainf.Data.ud_pos;
			//検出器
			detectorInfo.FDD = CTSettings.scansel.Data.fid;
			detectorInfo.pitchH = 10 / CTSettings.scancondpar.Data.b[1];
			detectorInfo.pitchV = detectorInfo.pitchH * kv;
			detectorInfo.DetType = Convert.ToInt16(CTSettings.detectorParam.DetType);		//v16.20/v17.00 追加 by 山影 10-03-04

			//スキャン位置
            //2014/11/07hata キャストの修正
            //scanpos = CTSettings.scancondpar.Data.cone_scan_posi_a * imageSize.CX / 2 + CTSettings.scancondpar.Data.cone_scan_posi_b + imageSize.CY / 2;
            scanpos = CTSettings.scancondpar.Data.cone_scan_posi_a * (float)imageSize.CX / 2F + CTSettings.scancondpar.Data.cone_scan_posi_b + imageSize.CY / 2F;
        
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
            //変更2014/09/20(検S1)hata
            //ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].BackColor = Color.Lime;
            ntbMove[(int)MECHA_MOVE.MOVE_FINE_X].TextBackColor = Color.Lime;

			ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].Value = frmMechaControl.ntbFTablePosY.Value;
            //変更2014/09/20(検S1)hata
            //ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].BackColor = Color.Lime;
            ntbMove[(int)MECHA_MOVE.MOVE_FINE_Y].TextBackColor = Color.Lime;

			ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Value = frmMechaControl.ntbUpDown.Value;
            //変更2014/09/20(検S1)hata
            //ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].BackColor = Color.Lime;
            ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].TextBackColor = Color.Lime;

			//自動スキャン位置移動用移動第１段階ＦＣＤ・Ｙ軸座標を記憶
			ntbFCD1st.Value = (decimal)(optTablePos1st.FCD - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()]);
			ntbY1st.Value = (decimal)optTablePos1st.lr;

			//自動スキャン位置移動用移動最終目標ＦＣＤ・Ｙ軸座標を記憶
			ntbMove[(int)MECHA_MOVE.MOVE_FCD].Value = (decimal)(optTablePos2nd.FCD - CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()]);
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
            //if (frmMechaControl.ntbFCD.Value < (decimal)CTSettings.GVal_FcdLimit)
            //Rev25.10 change by chouno 2017/09/11
            if (frmMechaControl.ntbFCD.Value < (decimal)(modSeqComm.GetFCDLimit()))
            {
				//現在よりも高い位置へ上昇する場合
				if (ntbMove[(int)MECHA_MOVE.MOVE_TABLE_Z].Value + 0.0001M < frmMechaControl.ntbUpDown.Value)
				{
                    //Rev21.00 昇降タイプによって変更する by長野 2015/03/17
                    if (CTSettings.t20kinf.Data.ud_type == 0)
                    {

                        //確認メッセージ表示：（コモンによってメッセージを切り替える）
                        //   リソース9510:試料テーブルが上昇しても、X線管にぶつからない位置にいることを確認して下さい。
                        //   リソース9511:試料テーブルが上昇しますので、コリメータ／フィルタ等に衝突しないか確認して下さい。
                        //   リソース9513:衝突しそうな場合は、安全な位置へ移動してから実行してください。
                        //   リソース9905:よろしければＯＫをクリックしてください。
                        DialogResult result = MessageBox.Show(CTResources.LoadResString(9510) + "\r" +
                                                                CTResources.LoadResString(9511) + "\r" +
                                                                CTResources.LoadResString(9513) + "\r" + "\r" +
                                                                CTResources.LoadResString(StringTable.IDS_ClickOK),
                                                            Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                        if (result == DialogResult.Cancel)
                        {
                            return functionReturnValue;
                        }
                    }
                    else
                    {
                        //確認メッセージ表示：（コモンによってメッセージを切り替える）
                        //   リソース9473:X線管・検出器が下降します。サンプル・コリメータ／フィルタ等に衝突しないか確認して下さい。
                        //   リソース9513:衝突しそうな場合は、安全な位置へ移動してから実行してください。
                        //   リソース9905:よろしければＯＫをクリックしてください。
                        DialogResult result = MessageBox.Show(CTResources.LoadResString(9473) + "\r" +
                                                                CTResources.LoadResString(9513) + "\r" + "\r" +
                                                                CTResources.LoadResString(StringTable.IDS_ClickOK),
                                                            Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                        if (result == DialogResult.Cancel)
                        {
                            return functionReturnValue;
                        }
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
					DialogResult result = MessageBox.Show(CTResources.LoadResString(9509) + "\r" +
															CTResources.LoadResString(9513) + "\r" + "\r" +
															CTResources.LoadResString(StringTable.IDS_ClickOK),
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

			//cwbtnMove.Text = CTResources.LoadResString(12317);
            cwbtnMove.Caption = CTResources.LoadResString(12317);
        }

    }
}
