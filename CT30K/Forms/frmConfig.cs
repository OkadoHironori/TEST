using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using CT30K.Common;
using CTAPI;

namespace CT30K
{
	internal partial class frmConfig: Form
	{
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmConfig myForm = null;

#if DebugOn
        private RadioButton[] optXrayTypes;
        private RadioButton[] optDetTypes; 
#endif

        public frmConfig()
        {
            InitializeComponent();
        }

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmConfig Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmConfig();
                }

                return myForm;
            }
        }
        #endregion    


#if DebugOn

		//*******************************************************************************
		//機　　能： キャンセルボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		//*******************************************************************************
		//機　　能： 適用ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdOK_Click(object sender, EventArgs e)
		{

			var scaninh = CTSettings.scaninh.Data;

			scaninh.mechacontrol = GetButton(cmdStatus1);
			scaninh.xray_remote = GetButton(cmdStatus2);
			scaninh.seqcomm = GetButton(cmdStatus3);
			scaninh.ext_trig = GetButton(cmdStatus4);
			scaninh.pc_freeze = GetButton(cmdStatus5);
			scaninh.full_distortion = GetButton(cmdStatus6);
			scaninh.multi_tube = GetButton(cmdStatus7);
			scaninh.cor_status = GetButton(cmdStatus11);
			scaninh.auto_cor = GetButton(cmdStatus12);
			scaninh.table_auto_move = GetButton(cmdStatus13);
			scaninh.scan_wizard = GetButton(cmdStatus14);
			scaninh.table_down_acquire = GetButton(cmdStatus15);
			scaninh.fpd_frame = GetButton(cmdStatus16);
			scaninh.binning = GetButton(cmdStatus17);
			scaninh.bin_char[0] = GetButton(cmdStatus18);
			scaninh.bin_char[1] = GetButton(cmdStatus19);
			scaninh.bin_char[2] = GetButton(cmdStatus20);
			scaninh.door_keyinput = GetButton(cmdStatus21);
			scaninh.ud_mecha_pres = GetButton(cmdStatus22);
			scaninh.tilt = GetButton(cmdStatus23);
			scaninh.iifield = GetButton(cmdStatus24);
			scaninh.iifield_char[0] = GetButton(cmdStatus25);
			scaninh.iifield_char[1] = GetButton(cmdStatus26);
			scaninh.iifield_char[2] = GetButton(cmdStatus27);
			scaninh.collimator = GetButton(cmdStatus28);
			scaninh.collimator_ud = GetButton(cmdStatus29);
			scaninh.collimator_rl = GetButton(cmdStatus30);
			scaninh.filter = GetButton(cmdStatus31);
			scaninh.slice_light = GetButton(cmdStatus32);
			scaninh.table_restriction = GetButton(cmdStatus33);
			scaninh.ii_move = GetButton(cmdStatus34);
			scaninh.door_lock = GetButton(cmdStatus35);
			scaninh.table_x = GetButton(cmdStatus36);
			scaninh.table_y = GetButton(cmdStatus37);
			scaninh.fine_table = GetButton(cmdStatus38);
			scaninh.fine_table_x = GetButton(cmdStatus39);
			scaninh.fine_table_y = GetButton(cmdStatus40);
			scaninh.data_mode[0] = GetButton(cmdStatus41);

            //追加2014/10/07hata_v19.51反映
            scaninh.scan_mode[2] = GetButton(cmdStatus42);  //シフトスキャン 'v18.00追加 byやまおか 2011/02/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            scaninh.data_mode[2] = GetButton(cmdStatus43);
			scaninh.scan_mode[0] = GetButton(cmdStatus44);
			scaninh.scan_mode[1] = GetButton(cmdStatus45);
			scaninh.scan_mode[2] = GetButton(cmdStatus46);
			scaninh.multiscan_mode[0] = GetButton(cmdStatus47);
			scaninh.multiscan_mode[1] = GetButton(cmdStatus48);
			scaninh.multiscan_mode[2] = GetButton(cmdStatus49);
			scaninh.multislice = GetButton(cmdStatus50);
			scaninh.auto_zoom = GetButton(cmdStatus51);
			scaninh.raw_save = GetButton(cmdStatus52);
			scaninh.auto_print = GetButton(cmdStatus53);
			scaninh.auto_centering = GetButton(cmdStatus54);
			scaninh.cone_distribute = GetButton(cmdStatus55);
			scaninh.helical = GetButton(cmdStatus56);
			scaninh.rotate_select = GetButton(cmdStatus57);
			scaninh.round_trip = GetButton(cmdStatus58);
			scaninh.over_scan = GetButton(cmdStatus59);
			scaninh.xray_rotate = GetButton(cmdStatus60);
			scaninh.mail_send = GetButton(cmdStatus61);
			scaninh.discharge_protect = GetButton(cmdStatus62);
			scaninh.artifact_reduction = GetButton(cmdStatus63);			//アーティファクト低減
        
			scaninh.post_cone_reconstruction = GetButton(cmdStatus64);		//コーン後再構成
			scaninh.pcws2 = GetButton(cmdStatus65);							//コーン分散処理用ＰＣ２
			scaninh.pcws3 = GetButton(cmdStatus66);							//コーン分散処理用ＰＣ３
			scaninh.pcws4 = GetButton(cmdStatus67);							//コーン分散処理用ＰＣ４
			scaninh.cone_distribute2 = GetButton(cmdStatus68);				//コーン分散処理２
        
			scaninh.filter_process[0] = GetButton(cmdStatus69);
			scaninh.filter_process[1] = GetButton(cmdStatus70);
        
			scaninh.high_speed_camera = GetButton(cmdStatus10);
        
			scaninh.second_detector = GetButton(cmdStatus9);
        
			//v19.00 BHC(電S2)永井
			scaninh.mbhc = GetButton(cmdStatus71);

        
            //Control[] optXrayTypes = new Control[Frame1.Controls.Count];
            Frame1.Controls.CopyTo(optXrayTypes, 0);

            //Control[] optDetTypes = new Control[Frame2.Controls.Count];
            Frame2.Controls.CopyTo(optDetTypes, 0);

			//Ｘ線のタイプ       'v11.3追加 by 間々田 2006/02/03
			CTSettings.t20kinf.Data.system_type.SetString( Convert.ToString(modLibrary.GetOption(optXrayTypes)));
        
			//検出器の種類       'v17.00追加 byやまおか 2010/01/19
			//scancondpar.detector = Convert.ToString(modLibrary.GetOption(optDetTypes));
            CTSettings.scancondpar.Data.detector = modLibrary.GetOption(optDetTypes);
        
			//Exitボタンにフォーカス 'v17.00追加 byやまおか 2010/01/20
			cmdCancel.Focus();

			MessageBox.Show("変更された設定は今回起動時のみ適用されます。", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		//*******************************************************************************
		//機　　能： 各設定ボタンクリック時処理
		//
		//           変数名          [I/O] 型        内容
		//引　　数： なし
		//戻 り 値： なし
		//
		//補　　足： なし
		//
		//履　　歴： V1.00  99/XX/XX   ????????      新規作成
		//*******************************************************************************
		private void cmdStatus_Click(object sender, EventArgs e)
		{
			Button btn = sender as Button;

			if (btn == null)
			{
				return;
			}

			if (Convert.ToString(btn.Tag) == "DISABLE")
			{
				return;
			}

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			//すでにプログラムが起動しているならば変更不可
			if IsExistForm(frmCTMenu) Then
				MsgBox "CT30K起動時のみ設定を変更できます。", vbExclamation
				Exit Sub
			End If
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
    
			//色を反転させる
			SetButton(btn, 1 - GetButton(btn));
    
			//OKボタンを使用可にする
			cmdOK.Enabled = true;
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
        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);
        private void frmConfig_Load(object sender, EventArgs e)
        {

			var scaninh = CTSettings.scaninh.Data;

			SetButton(cmdStatus1, scaninh.mechacontrol);
			SetButton(cmdStatus2, scaninh.xray_remote);
			SetButton(cmdStatus3, scaninh.seqcomm);
			SetButton(cmdStatus4, scaninh.ext_trig);
			SetButton(cmdStatus5, scaninh.pc_freeze);
			SetButton(cmdStatus6, scaninh.full_distortion);
			SetButton(cmdStatus7, scaninh.multi_tube);
			SetButton(cmdStatus11, scaninh.cor_status);
			SetButton(cmdStatus12, scaninh.auto_cor);
			SetButton(cmdStatus13, scaninh.table_auto_move);
			SetButton(cmdStatus14, scaninh.scan_wizard);
			SetButton(cmdStatus15, scaninh.table_down_acquire);
			SetButton(cmdStatus16, scaninh.fpd_frame);
			SetButton(cmdStatus17, scaninh.binning);
			SetButton(cmdStatus18, scaninh.bin_char[0]);
			SetButton(cmdStatus19, scaninh.bin_char[1]);
			SetButton(cmdStatus20, scaninh.bin_char[2]);
			SetButton(cmdStatus21, scaninh.door_keyinput);
			SetButton(cmdStatus22, scaninh.ud_mecha_pres);
			SetButton(cmdStatus23, scaninh.tilt);
			SetButton(cmdStatus24, scaninh.iifield);
			SetButton(cmdStatus25, scaninh.iifield_char[0]);
			SetButton(cmdStatus26, scaninh.iifield_char[1]);
			SetButton(cmdStatus27, scaninh.iifield_char[2]);
			SetButton(cmdStatus28, scaninh.collimator);
			SetButton(cmdStatus29, scaninh.collimator_ud);
			SetButton(cmdStatus30, scaninh.collimator_rl);
			SetButton(cmdStatus31, scaninh.filter);
			SetButton(cmdStatus32, scaninh.slice_light);
			SetButton(cmdStatus33, scaninh.table_restriction);
			SetButton(cmdStatus34, scaninh.ii_move);
			SetButton(cmdStatus35, scaninh.door_lock);
			SetButton(cmdStatus36, scaninh.table_x);
			SetButton(cmdStatus37, scaninh.table_y);
			SetButton(cmdStatus38, scaninh.fine_table);
			SetButton(cmdStatus39, scaninh.fine_table_x);
			SetButton(cmdStatus40, scaninh.fine_table_y);
			SetButton(cmdStatus41, scaninh.data_mode[0]);

            //追加2014/10/07hata_v19.51反映
            SetButton(cmdStatus42, scaninh.data_mode[3]);   //シフトスキャン 'v18.00追加 byやまおか 2011/02/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

			SetButton(cmdStatus43, scaninh.data_mode[2]);
			SetButton(cmdStatus44, scaninh.scan_mode[0]);
			SetButton(cmdStatus45, scaninh.scan_mode[1]);
			SetButton(cmdStatus46, scaninh.scan_mode[2]);
			SetButton(cmdStatus47, scaninh.multiscan_mode[0]);
			SetButton(cmdStatus48, scaninh.multiscan_mode[1]);
			SetButton(cmdStatus49, scaninh.multiscan_mode[2]);
			SetButton(cmdStatus50, scaninh.multislice);
			SetButton(cmdStatus51, scaninh.auto_zoom);
			SetButton(cmdStatus52, scaninh.raw_save);
			SetButton(cmdStatus53, scaninh.auto_print);
			SetButton(cmdStatus54, scaninh.auto_centering);
			SetButton(cmdStatus55, scaninh.cone_distribute);
			SetButton(cmdStatus56, scaninh.helical);
			SetButton(cmdStatus57, scaninh.rotate_select);
			SetButton(cmdStatus58, scaninh.round_trip);
			SetButton(cmdStatus59, scaninh.over_scan);
			SetButton(cmdStatus60, scaninh.xray_rotate);
			SetButton(cmdStatus61, scaninh.mail_send);
			SetButton(cmdStatus62, scaninh.discharge_protect);
			SetButton(cmdStatus63, scaninh.artifact_reduction);			//アーティファクト低減
    
			SetButton(cmdStatus64, scaninh.post_cone_reconstruction);	//コーン後再構成
			SetButton(cmdStatus65, scaninh.pcws2);						//コーン分散処理用ＰＣ２
			SetButton(cmdStatus66, scaninh.pcws3);						//コーン分散処理用ＰＣ３
			SetButton(cmdStatus67, scaninh.pcws4);						//コーン分散処理用ＰＣ４
			SetButton(cmdStatus68, scaninh.cone_distribute2);			//コーン分散処理２
    
			SetButton(cmdStatus69, scaninh.filter_process[0]);
			SetButton(cmdStatus70, scaninh.filter_process[1]);
        
			SetButton(cmdStatus10, scaninh.high_speed_camera);
        
			SetButton(cmdStatus9, scaninh.second_detector);
        
			//v19.00 BHC(電S2)永井
			SetButton(cmdStatus71, scaninh.mbhc);

            optXrayTypes = new RadioButton[Frame1.Controls.Count];
            Frame1.Controls.CopyTo(optXrayTypes, 0);

            optDetTypes = new RadioButton[Frame2.Controls.Count];
            Frame2.Controls.CopyTo(optDetTypes, 0);

            //Ｘ線のタイプ       'v11.3追加 by 間々田 2006/02/03
            modLibrary.SetOption(optXrayTypes, Convert.ToInt32(CTSettings.t20kinf.Data.system_type.GetString()));
        
			//検出器の種類       'v17.00追加 byやまおか 2010/01/19
            modLibrary.SetOption(optDetTypes, CTSettings.scancondpar.Data.detector);

			//最初に表示するタブ
			tabConfig.SelectedIndex = 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theButton"></param>
		/// <param name="theValue"></param>
		private void SetButton(Button theButton, int theValue)
		{

			theButton.BackColor = theValue == 0? Color.Lime : Color.Green;
			theButton.Text = theValue == 0? "有効" : "無効";

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theButton"></param>
		/// <returns></returns>
		private int GetButton(Button theButton)
		{
			return theButton.BackColor == Color.Lime? 0 : 1;
    	}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="As"></param>
		private void optXrayType_Click(object sender, EventArgs e)
		{
			//OKボタンを使用可にする
			if (this.Visible)
			{
				cmdOK.Enabled = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Index"></param>
		private void optDetType_Click(object sender, EventArgs e)  //v17.00追加 byやまおか 2010/01/19
		{
			//OKボタンを使用可にする
			if (this.Visible)
			{
				cmdOK.Enabled = true;
			}
		}

#else
        //キャンセルボタンクリック時処理
        private void cmdCancel_Click(object sender, EventArgs e)
        {
        }

        //適用ボタンクリック時処理
        private void cmdOK_Click(object sender, EventArgs e)
        {
        }

        //各設定ボタンクリック時処理
        private void cmdStatus_Click(object sender, EventArgs e)
        {
        }

        //フォームロード時の処理
        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);
        //}
        private void frmConfig_Load(object sender, EventArgs e)
        {
        }

        private void optXrayType_Click(object sender, EventArgs e)
        {
        }

        private void optDetType_Click(object sender, EventArgs e)  //v17.00追加 byやまおか 2010/01/19
        {
        }


#endif

    }
}
