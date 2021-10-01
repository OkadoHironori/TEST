using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XrayCtrl;

namespace CT30K
{
	public partial class frmVirtualXrayControl : Form
	{
		private static frmVirtualXrayControl _Instance = null;

		public frmVirtualXrayControl()
		{
			InitializeComponent();

//デバッグ用
//#if DebugOn											//デバッグ時は仮想Ｘ線制御とする by 間々田 2004/11/29
//Rev23.10 変更 by長野 2015/10/02
#if XrayDebugOn

			this.nudUp_Focussize.ValueChanged += new System.EventHandler(this.Up_Focussize_ValueChanged);
            this.nudUp_Warmup.ValueChanged += new System.EventHandler(this.Up_Warmup_ValueChanged);
            this.nudUp_X_Avail.ValueChanged += new System.EventHandler(this.Up_X_Avail_ValueChanged);
            this.nudUp_X_On.ValueChanged += new System.EventHandler(this.Up_X_On_ValueChanged);
            this.nudUp_XR_CurrentFeedback.ValueChanged += new System.EventHandler(this.Up_XR_CurrentFeedback_ValueChanged);
            this.nudUp_XR_CurrentSet.ValueChanged += new System.EventHandler(this.Up_XR_CurrentSet_ValueChanged);
            this.nudUp_XR_VoltFeedback.ValueChanged += new System.EventHandler(this.Up_XR_VoltFeedback_ValueChanged);
            this.nudUp_XR_VoltSet.ValueChanged += new System.EventHandler(this.Up_XR_VoltSet_ValueChanged);
            this.cobUp_XrayStatusTYP.Click += new System.EventHandler(this.Up_XrayStatusTYP_Click);
            this.nudUp_XrayTargetInfSTG.ValueChanged += new System.EventHandler(this.Up_XrayTargetInfSTG_ValueChanged);
            this.cobUp_XrayVacuumSVC.Click += new System.EventHandler(this.Up_XrayVacuumSVC_Click);
			this.Load += new System.EventHandler(this.frmVirtualXrayControl_Load);
#endif

		}

		public static frmVirtualXrayControl Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmVirtualXrayControl();
				}

				return _Instance;
			}
		}
		
//デバッグ用　ここから
//#if DebugOn											//デバッグ時は仮想Ｘ線制御とする by 間々田 2004/11/29
//Rev23.10 変更 by長野 2015/10/02
#if XrayDebugOn
		//'' --------------------------------------------------------
		//       イベント宣言
		//' --------------------------------------------------------
		 // 管電圧・管電流更新
		public class XrayValueEventArgs : EventArgs
		{
			public clsTActiveX.XrayValue XrayValue;
		}
		public delegate void XrayValueDispEventHandler(object sender, XrayValueEventArgs e);
		public event XrayValueDispEventHandler XrayValueDisp;

		// 出力管電圧・出力管電流・メカ位置・拡大率更新
		public class MechDataEventArgs : EventArgs
		{
			public clsTActiveX.MechData MechData;
		}
		public delegate void MechDataDispEventHandler(object sender, MechDataEventArgs e);
		public event MechDataDispEventHandler MechDataDisp;

		// 装置状態
		public class StatusValueEventArgs : EventArgs
		{
			public clsTActiveX.StatusValue StatusValue;
		}
		public delegate void StatusValueDispEventHandler(object sender, StatusValueEventArgs e);
		public event StatusValueDispEventHandler StatusValueDisp;
		
		// ユーザ設定
		public class UserValueEventArgs : EventArgs
		{
			public clsTActiveX.UserValue UserValue;
		}
		public delegate void UserValueDispEventHandler(object sender, UserValueEventArgs e);
		public event UserValueDispEventHandler UserValueDisp;

		// ウォームアップ残時間
		public class WarmUpTimeEventArgs : EventArgs
		{
			public clsTActiveX.WarmUpTime WarmUpTime;
		}
		public delegate void WarmUpTimeDispEventHandler(object sender, WarmUpTimeEventArgs e);
		public event WarmUpTimeDispEventHandler WarmUpTimeDisp;
		
		// ActiveX 制御エラー発生
		public class ErrSetEventArgs : EventArgs
		{
			public clsTActiveX.ErrSet ErrSet;
		}
		public delegate void ErrSetDispEventHandler(object sender, ErrSetEventArgs e);
		public event ErrSetDispEventHandler ErrSetDisp;

		//L9191/L10801用
		public class UdtXrayStatus3EventArgs : EventArgs
		{
			public clsTActiveX.udtXrayStatus3ValueDisp udtXrayStatus3ValueDisp;
		}
		public delegate void XrayStatus3ValueDispEventHandler(object sender, UdtXrayStatus3EventArgs e);
		public event XrayStatus3ValueDispEventHandler XrayStatus3ValueDisp;


		private string LastText;
		private string LastText2;		//v15.10追加 byやまおか 2009/10/13


        //==================================================
        //プロパティ(Get)
        //==================================================
        //プリヒート
        public int Up_PreHeat
        {
            get
            {
                return 0;
            }
        }
      

        //Z軸タイプ
        public int Up_XR_type
        {
            get
            {
                return 0;
            }
        }


        //設定管電圧
        public float Up_XR_VoltFeedback
        {
            get
            {
                return (float)nudUp_XR_VoltFeedback.Value;
            }
        }


        //設定管電流
        public float Up_XR_CurrentFeedback
        {
            get
            {
                return (float)nudUp_XR_VoltFeedback.Value;
            }
        }


        //ISOWAT制御時の電圧・電流
        public float Up_X_Watt
        {
            get
            {
                return 0;
            }
        }


        //ファインセット
        public int Up_XR_Fine
        {
            get
            {
                return 0;
            }
        }


        //X線ON中
        public int Up_X_On
        {
            get
            {
                return (int)nudUp_X_On.Value;
            }
        }


        //スタンバイ
        public int Up_Standby
        {
            get
            {
                return (int)nudUp_Standby.Value;
            }
        }


        //インターロック
        public int Up_InterLock
        {
            get
            {
                return (int)nudUp_InterLock.Value;
            }
        }


        //焦点
        public int Up_Focussize
        {
            get
            {
                return (int)nudUp_Focussize.Value;
            }
        }


        //ウォームアップ中
        public int Up_Warmup
        {
            get
            {
                return (int)nudUp_Warmup.Value;
            }
        }


        //X線正常
        public int Up_XR_Status
        {
            get
            {
                return (int)nudUp_XR_Status.Value;
            }
        }


        //
        public int Up_Permit_Warmup
        {
            get
            {
                return 0;
            }
        }


        //ウォームアップモード
        public int Up_Wrest_Mode
        {
            get
            {
                return (int)nudUp_Wrest_Mode.Value;
            }
        }


        //ウォームアップ残時間
        public int Up_Wrest_TimeM
        {
            get
            {
                return (int)nudUp_Wrest_TimeM.Value;
            }
        }


        //ウォームアップ残時間
        public int Up_Wrest_TimeS
        {
            get
            {
                return (int)nudUp_Wrest_TimeS.Value;
            }
        }


        //X線OFFモード
        public int Up_Xcont_Mode
        {
            get
            {
                return (int)nudUp_Xcont_Mode.Value;
            }
        }


        //X線OFF時間
        public int Up_Xtimer
        {
            get
            {
                return (int)nudUp_Xtimer.Value;
            }
        }


        //最新X線OFF日時
        public string Up_X_LastOff
        {
            get
            {
                return "";
            }
        }


        //異常発生コード
        public int Up_Err
        {
            get
            {
                return (int)cwneError.Value;
            }
        }


        //設定管電圧プロパティ
        public float Up_XR_VoltSet
        {
            get
            {
                return (int)nudUp_XR_VoltSet.Value;
            }
        }


        //設定管電流プロパティ
        public float Up_XR_CurrentSet
        {
            get
            {
                return (int)nudUp_XR_CurrentSet.Value;
            }
        }


        //設定可能電圧
        public float Up_XR_Max_kV
        {
            get
            {
                return (int)nudUp_XR_Max_kV.Value;
            }
        }


        //設定可能電流
        public float Up_XR_Max_mA
        {
            get
            {
                return (int)nudUp_XR_Max_mA.Value;
            }
        }


        //X線アベイラブル
        public int Up_X_Avail
        {
            get
            {
                return (int)nudUp_X_Avail.Value;
            }
        }

        //ｳｫｰﾑｱｯﾌﾟｽﾃｯﾌﾟ
        public int Up_XrayWarmupSWS
        {
            get
            {
                return (int)nudUp_XrayWarmupSWS.Value;
            }
        }

        //最小管電圧
        public float Up_XR_Min_kV
        {
            get
            {
                return (int)nudUp_XR_Min_kV.Value;
            }
        }

        //最小管電流
        public float Up_XR_Min_mA
        {
            get
            {
                return (int)nudUp_XR_Min_mA.Value;
            }
        }

        //   設定可能最大管電圧(kV)
        public int Up_XrayScaleMaxkV
        {
            get
            {
                return 0;
            }
        }


        //   設定可能最大管電流(μA)
        public int Up_XrayScaleMaxuA
        {
            get
            {
                return 0;
            }
        }        


        //   設定可能最小管電圧(kV)
        public int Up_XrayScaleMinkV
        {
            get
            {
                return 0;
            }
        } 
        

        //   設定可能最小管電流(μA)
        public int Up_XrayScaleMinuA
        {
            get
            {
                return 0;
            }
        }


        //   最大POWER制限が最大（W)かタゲット電流の把握
        public int Up_XrayMaxPower
        {
            get
            {
                return 0;
            }
        }        


        //   フォーカスF1時の最大(W)又はターゲット電流
        public float Up_XrayF1MaxPower
        {
            get 
            {
                return 0;
            }
        }


        //   フォーカスF2時の最大(W)又はターゲット電流
        public float Up_XrayF2MaxPower
        {
            get 
            {
                return 0;
            }
        }


        //   フォーカスF3時の最大(W)又はターゲット電流
        public float Up_XrayF3MaxPower
        {
            get 
            {
                return 0;
            }
        }


        //   フォーカスF4時の最大(W)又はターゲット電流
        public float Up_XrayF4MaxPower
        {
            get 
            {
                return 0;
            }
        }


        //   ウォーミングアップパターンの時間表示有無
        public int Up_XrayWarmupTime
        {
            get 
            {
                return 0;
            }
        }        


        //   ウォーミングアップパターン１のウォームアップ時間（分）
        public int Up_XrayWarmup1Time
        {
            get 
            {
                return 0;
            }
        }         


        //   ウォーミングアップパターン２のウォームアップ時間（分）
        public int Up_XrayWarmup2Time
        {
            get 
            {
                return 0;
            }
        }


        //   ウォーミングアップパターン３のウォームアップ時間（分）
        public int Up_XrayWarmup3Time
        {
            get 
            {
                return 0;
            }
        }
        
        //   最大Power制限値が最大（W)かターゲット電流の把握
        public int Up_XrayFocusNumber
        {
            get 
            {
                return 0;
            }
        }

        //   アベイラブル管電圧範囲
        public int Up_XrayAvailkV
        {
            get 
            {
                return 0;
            }
        }
        
        //   アベイラブル管電流範囲
        public int Up_XrayAvailuA
        {
            get 
            {
                return 0;
            }
        }        

        //   X線ON中に設定値を変更した場合のアベイラブル時間
        public int Up_XrayAvailTimeInside
        {
            get 
            {
                return 0;
            }
        }        

        //   X線OFFからX線ON時のアベイラブル時間
        public int Up_XrayAvailTimeOn
        {
            get 
            {
                return 0;
            }
        }

        //   ターゲット電流ステータスの有無
        public int Up_XrayTargetInf
        {
            get 
            {
                return 0;
            }
        }        

         //   ターゲット電流
        public float Up_XrayTargetInfSTG
        {
            get
            {
                return (float)nudUp_XrayTargetInfSTG.Value;
            }
        }        

        //   真空度情報の有無
        public int Up_XrayVacuumInf
        {
            get
            {
                return 0;
            }
        }        

        //   真空度
        public string Up_XrayVacuumSVC
        {
            get
            {
                return cobUp_XrayVacuumSVC.Text;
            }
        }        

        //   X軸方向アライメント確認
        public int Up_XrayStatusSBX
        {
            get
            {
                return (int)nudUp_XrayStatusSBX.Value;
            }
        }        

        //   Y軸方向アライメント確認
        public int Up_XrayStatusSBY
        {
            get
            {
                return (int)nudUp_XrayStatusSBY.Value;
            }
        }        
        
        //   アライメントモニタ
        public int Up_XrayStatusSAD
        {
            get
            {
                return 0;
            }
        }        
        

        //   フォーカス値
        public float Up_XrayStatusSOB
        {
            get
            {
                return (float)nudUp_XrayStatusSOB.Value;
            }
        }        


        //   真空計値
        public float Up_XrayStatusSVV
        {
            get
            {
                return (float)nudUp_XrayStatusSVV.Value;
            }
        }        


        //   電源ON通電時間
        public long Up_XrayStatusSTM
        {
            get
            {
                return 0;
            }
        }        
        

        //   X線照射時間
        public long Up_XrayStatusSXT
        {
            get
            {
                return 0;
            }
        }        
        

        //   フィラメント入力確認
        public int Up_XrayStatusSHM
        {
            get
            {
                return 0;
            }
        }        


        //   フィラメント設定電圧確認
        public float Up_XrayStatusSHS
        {
            get
            {
                return 0;
            }
        }


        //   フィラメント通電時間
        public long Up_XrayStatusSHT
        {
            get
            {
                return 0;
            }
        }


        //   自動X線停止時間
        public int Up_XrayStatusSAT
        {
            get
            {
                return 0;
            }
        }


        //   過負荷保護機構
        public int Up_XrayStatusSOV
        {
            get
            {
                return (int)nudUp_XrayStatusSOV.Value;
            }
        }


        //   制御基板異常
        public int Up_XrayStatusSER
        {
            get
            {
                return (int)nudUp_XrayStatusSER.Value; ;
            }
        }


        //   ウォームアップモードステップ確認
        public int Up_XrayStatusSWS
        {
            //    Up_XrayStatusSWS
            get { return 0; }
        }

        //   フィラメント状態確認
        public int Up_XrayStatusFLM
        {
            get
            {
                return (int)nudUp_XrayStatusFLM.Value; ;
            }
        }


        //   ターゲット温度
        public int Up_XrayStatusZT1
        {
            get
            {
                return (int)nudUp_XrayStatusZT1.Value; ;
            }
        }


        //   X線装置型名
        public string Up_XrayStatusTYP
        {
            get
            {
                return cobUp_XrayStatusTYP.Text;
            }
        }


        //   ウォームアップ状態確認
        public string Up_XrayStatusSWE
        {
            get
            {
                return "";
            }
        }


        //   ウォームアップ管電圧上昇下降パラメータ確認
        public string Up_XrayStatusSWU
        {
            get
            {
                return "";
            }
        }


        //   使用上限管電圧読み出し      //v15.10追加 byやまおか 2009/11/12
        public int Up_XrayStatusSMV
        {
            get
            {
                return 0;
            }
        }

        //   ステータス自動送信確認
        public string Up_XrayStatusSSA
        {
            get
            {
                return "";
            }
        }


        //   ｵﾍﾟﾚｰﾄｽｲｯﾁ状態確認(0：OFF、1:REMOTE、2:LOCAL)
        public int Up_XrayOperateSRL
        {
            get
            {
                return 0;
            }
        }

        //   ﾘﾓｰﾄ動作状態確認(0:BUSY、1:READY)
        public int Up_XrayRemoteSRB
        {
            get
            {
                return 0;
            }
        }

        //バッテリー状態確認(0:正常、1:Low)
        //L9421-02/L9181-02用
        public int Up_XrayBatterySBT
        {
            get
            {
                return 0;
            }
        }

        //   X線電力制限
        public int Up_XrayWattageLimit
        {
            get
            {
                return 0;
            }
        }

        public int Up_XrayStatusSMD
        {
            get
            {
                return 0;
            }
        }


		//==================================================
		//メソッド
		//==================================================
		//X線ON/OFFメソッド
		public int Xrayonoff_Set(int minf)
		{
			nudUp_X_On.Value = (minf == 1 ? 1 : 0);
			return 0;
		}

		//管電圧・管電流設定メソッド
		public int XrayValue_Set(clsTActiveX.XrayValueSet Val1)
		{
			nudUp_XR_VoltSet.Value = (decimal)Val1.m_kVSet;
			nudUp_XR_CurrentSet.Value = (decimal)Val1.m_mASet;
			return 0;
		}

		//フォーカスOBJを設定する
		public int XrayOBJ_Set(float Val1)
		{
			return 0;
		}

		//フォーカス値を保存する
		public int XraySAV_Set(int Val1)
		{
			return 0;
		}

		//フォーカス値を自動的に決定する
		public int XrayOST_Set(int Val1)
		{
			return 0;
		}

		//電子ビームのビームアライメントを調整する
		public int XrayADJ_Set(int Val1)
		{
			return 0;
		}

		//電子ビームのビームアライメント調整を一括で実施する
		public int XrayADA_Set(int Val1)
		{
			return 0;
		}

		//電子ビームのビームアライメント調整を中止する
		public int XraySTP_Set(int Val1)
		{
			return 0;
		}

		//過負荷保護機能を解除する
		public int XrayRST_Set(int Val1)
		{
			return 0;
		}

		//ウォームアップ完了状態時にウォームアップを開始する
		public int XrayWarmUp_Set(int Val1)
		{
			return 0;
		}

		//上限管電圧を制限する       'v15.10追加 byやまおか 2009/11/12
		public int XrayCMV_Set(int Val1)
		{
        #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			XrayControl.Up_XrayStatusSMV = Val1    'v17.00追加 byやまおか 2010/02/10
*/
        #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

		//TODO 読み取り専用
		//	modXrayControl.XrayControl.Up_XrayStatusSMV = Val1;		//v17.00追加 byやまおか 2010/02/10

			return 0;
		}

		//ユーザ設定メソッド
		public int UserValue_Set(clsTActiveX.UserValueSet Val1)
		{
			nudUp_Focussize.Value = Val1.m_XrayFocusSet;
			return 0;
		}

		//コマンドボタン（ウォーミングアップ強制終了）メソッド
		public int WarmUpQuit_Set(int minf)
		{
			return 0;
		}

		//状態要求メソッド
		public int X_AllEventRaise_Set(int minf)
		{
			return 0;
		}

		//メッセージ確認メソッド
		public int MessageOk_Set(int minf)
		{
			return 0;
		}

        //フィラメントモード設定
        public int XrayMDE_Set(int minf)
        {
            return 0;
        }

		//イベント処理動作メソッド
		public int EventValue_Set(int minf)
		{
			return 0;
		}


		//フォームロード時
		private void frmVirtualXrayControl_Load(object sender, EventArgs e)
		{
			LastText = Convert.ToString(cobUp_XrayVacuumSVC.Items[0]);
			cobUp_XrayVacuumSVC.SelectedIndex = 0;
			//v15.10追加(ここから) byやまおか 2009/10/13
			LastText2 = Convert.ToString(cobUp_XrayStatusTYP.Items[0]);
			cobUp_XrayStatusTYP.SelectedIndex = 0;
			nudUp_X_Avail.Value = 1;		//v17.00追加 byやまおか 2010/02/18
			this.Top = 33;

			switch (modXrayControl.XrayType)
			{
				case modXrayControl.XrayTypeConstants.XrayTypeFeinFocus:
					nudUp_XR_Max_kV.Value = 225;
					break;
				case modXrayControl.XrayTypeConstants.XrayTypeKevex:
					nudUp_XR_Max_kV.Value = 130;
					break;
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181:
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL9181_02: //追加2014/11/05hata L9181-02に対応
					nudUp_XR_Max_kV.Value = 130;
					break;
				case modXrayControl.XrayTypeConstants.XrayTypeHamaL9191:
					nudUp_XR_Max_kV.Value = 160;
					break;
				case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421:
					nudUp_XR_Max_kV.Value = 90;
					break;
				case modXrayControl.XrayTypeConstants.XrayTypeViscom:
					nudUp_XR_Max_kV.Value = 225;
					break;
				case modXrayControl.XrayTypeConstants.XrayTypeHamaL10801:
					nudUp_XR_Max_kV.Value = 230;
					break;
				case modXrayControl.XrayTypeConstants.XrayTypeHamaL9421_02T:	//v16.30追加 byやまおか 2010/05/21
					nudUp_XR_Max_kV.Value = 90;
					break;
				case modXrayControl.XrayTypeConstants.XrayTypeHamaL8121_02:		//v17.71追加 by長野 2012/03/14
					nudUp_XR_Max_kV.Value = 150;
                    break;
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL12721:       //Rev23.10 追加 by長野 2015/10/01
                    nudUp_XR_Max_kV.Value = 300;
                    break;
                case modXrayControl.XrayTypeConstants.XrayTypeHamaL10711:       //Rev23.10 追加 by長野 2015/10/01
                    nudUp_XR_Max_kV.Value = 160;
                    break;
			}
			//v15.10追加(ここまで) byやまおか 2009/10/13
		}


		private void Up_Focussize_ValueChanged(object sender, EventArgs e)
		{
			if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom) return;

			clsTActiveX.UserValue Val1 = default(clsTActiveX.UserValue);
			Val1.m_XrayFocusSize = (int)nudUp_Focussize.Value;		//焦点
			
			if(UserValueDisp != null)
			{
				UserValueEventArgs uve = new UserValueEventArgs();
				uve.UserValue = Val1;

				UserValueDisp(this, uve);
			}
		}


		private void Up_XR_VoltFeedback_ValueChanged(object sender, EventArgs e)
		{
			ChangeMechDataDisp();
		}

		private void Up_XR_CurrentFeedback_ValueChanged(object sender, EventArgs e)
		{
			ChangeMechDataDisp();
		}

		private void Up_X_On_ValueChanged(object sender, EventArgs e)
		{
			ChangeMechDataDisp();
		}

		private void Up_X_Avail_ValueChanged(object sender, EventArgs e)
		{
			ChangeMechDataDisp();
		}

		private void Up_XrayStatusTYP_Click(object sender, EventArgs e)				//v15.10追加 byやまおか 2009/10/07
		{
			if (LastText2 != cobUp_XrayStatusTYP.Text)
			{
				ChangeMechDataDisp();
				LastText2 = cobUp_XrayStatusTYP.Text;
			}
		}

		private void Up_XrayTargetInfSTG_ValueChanged(object sender, EventArgs e)
		{
			ChangeMechDataDisp();
		}

		private void Up_XrayVacuumSVC_Click(object sender, EventArgs e)
		{
			if (LastText != cobUp_XrayVacuumSVC.Text)
			{
				ChangeMechDataDisp();
				LastText = cobUp_XrayVacuumSVC.Text;
			}
		}

		private void ChangeMechDataDisp()
		{
			if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom) return;

			clsTActiveX.MechData Val1 = default(clsTActiveX.MechData);
			Val1.m_XrayOnSet = (int)nudUp_X_On.Value;
			//Val1.m_Curent = Up_XR_VoltFeedback.Value       '逆だった   'v15.10変更 byやまおか 2009/10/23
			//Val1.m_Voltage = Up_XR_CurrentFeedback.Value   '逆だった   'v15.10変更 byやまおか 2009/10/23
			Val1.m_Curent = (int)nudUp_XR_CurrentFeedback.Value;
			Val1.m_Voltage = (int)nudUp_XR_VoltFeedback.Value;
			Val1.m_XAvail = (int)nudUp_X_Avail.Value;						//X線アベイラブル
			Val1.m_XrayTargetInfSTG = (int)nudUp_XrayTargetInfSTG.Value;	//ターゲット電流
			//Val1.m_XrayTargetLimit = 0
			//Val1.m_XrayVacuumInf = 0
			Val1.m_XrayVacuumSVC = cobUp_XrayVacuumSVC.Text;				//真空度

			if (MechDataDisp != null)
			{
				MechDataEventArgs e = new MechDataEventArgs();
				e.MechData = Val1;

				MechDataDisp(this, e);
			}
		}


		private void Up_XR_VoltSet_ValueChanged(object sender, EventArgs e)
		{
			ChangeXrayValueDisp();
		}

		private void Up_XR_CurrentSet_ValueChanged(object sender, EventArgs e)
		{
			ChangeXrayValueDisp();
		}

		private void ChangeXrayValueDisp()
		{
			if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom) return;

			clsTActiveX.XrayValue Val1 = default(clsTActiveX.XrayValue);
			Val1.m_kVSet = (float)nudUp_XR_VoltSet.Value;			//設定管電圧プロパティ
			Val1.m_mASet = (float)nudUp_XR_CurrentSet.Value;		//設定管電流プロパティ
			
			if (XrayValueDisp != null)
			{
				XrayValueEventArgs e = new XrayValueEventArgs();
				e.XrayValue = Val1;

				XrayValueDisp(this, e);
			}
		}


		//ウォームアップステータス更新   'v17.72/v19.02追加 byやまおか 2012/05/11
		private void Up_Warmup_ValueChanged(object sender, EventArgs e)
		{
			ChangeStatusValueDisp();
		}

		//ウォームアップステータス更新   'v17.72/v19.02追加 byやまおか 2012/05/11
        //メソッド名が StatusValueDisp イベントと重複するため ChangeStatusValueDisp() に変更
        private void ChangeStatusValueDisp()
		{
			if (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeViscom) return;

			clsTActiveX.StatusValue Val1 = default(clsTActiveX.StatusValue);
			Val1.m_WarmUp = (int)nudUp_Warmup.Value;

			if (StatusValueDisp != null)
			{
				StatusValueEventArgs e = new StatusValueEventArgs();
				e.StatusValue = Val1;

				StatusValueDisp(this, e);
			}
		}


#endif
        //デバッグ用　ここまで

	}
}
