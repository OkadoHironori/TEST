using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace spc53004
{
	/// <summary>
	/// DlgDev の概要の説明です。
	/// </summary>
	public class DlgDev : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.Label LblDev;
		internal System.Windows.Forms.Label LblHndl;
		internal System.Windows.Forms.Button BtnDevCls;
		internal System.Windows.Forms.Button BtnDevOpn;
		internal System.Windows.Forms.ComboBox CmbID;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.Button BtnDevInf;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// デバイスオープン/クローズサンプル
		/// </summary>
		public DlgDev()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows フォーム デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.LblDev = new System.Windows.Forms.Label();
			this.LblHndl = new System.Windows.Forms.Label();
			this.BtnDevCls = new System.Windows.Forms.Button();
			this.BtnDevOpn = new System.Windows.Forms.Button();
			this.CmbID = new System.Windows.Forms.ComboBox();
			this.Label1 = new System.Windows.Forms.Label();
			this.BtnDevInf = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// LblDev
			// 
			this.LblDev.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblDev.Location = new System.Drawing.Point(280, 128);
			this.LblDev.Name = "LblDev";
			this.LblDev.Size = new System.Drawing.Size(216, 168);
			this.LblDev.TabIndex = 20;
			// 
			// LblHndl
			// 
			this.LblHndl.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblHndl.Location = new System.Drawing.Point(280, 48);
			this.LblHndl.Name = "LblHndl";
			this.LblHndl.Size = new System.Drawing.Size(216, 24);
			this.LblHndl.TabIndex = 19;
			// 
			// BtnDevCls
			// 
			this.BtnDevCls.Enabled = false;
			this.BtnDevCls.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.BtnDevCls.Location = new System.Drawing.Point(272, 88);
			this.BtnDevCls.Name = "BtnDevCls";
			this.BtnDevCls.Size = new System.Drawing.Size(224, 24);
			this.BtnDevCls.TabIndex = 18;
			this.BtnDevCls.Text = "デバイスクローズ";
			this.BtnDevCls.Click += new System.EventHandler(this.BtnDevCls_Click);
			// 
			// BtnDevOpn
			// 
			this.BtnDevOpn.Enabled = false;
			this.BtnDevOpn.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.BtnDevOpn.Location = new System.Drawing.Point(272, 16);
			this.BtnDevOpn.Name = "BtnDevOpn";
			this.BtnDevOpn.Size = new System.Drawing.Size(224, 24);
			this.BtnDevOpn.TabIndex = 17;
			this.BtnDevOpn.Text = "デバイスオープン";
			this.BtnDevOpn.Click += new System.EventHandler(this.BtnDevOpn_Click);
			// 
			// CmbID
			// 
			this.CmbID.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.CmbID.Location = new System.Drawing.Point(136, 64);
			this.CmbID.Name = "CmbID";
			this.CmbID.Size = new System.Drawing.Size(40, 23);
			this.CmbID.TabIndex = 16;
			this.CmbID.Text = "ComboBox1";
			this.CmbID.SelectedIndexChanged += new System.EventHandler(this.CmbID_SelectedIndexChanged);
			// 
			// Label1
			// 
			this.Label1.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.Label1.Location = new System.Drawing.Point(24, 64);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(104, 24);
			this.Label1.TabIndex = 15;
			this.Label1.Text = "ボードID選択";
			// 
			// BtnDevInf
			// 
			this.BtnDevInf.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.BtnDevInf.Location = new System.Drawing.Point(16, 16);
			this.BtnDevInf.Name = "BtnDevInf";
			this.BtnDevInf.Size = new System.Drawing.Size(160, 24);
			this.BtnDevInf.TabIndex = 14;
			this.BtnDevInf.Text = "ボードID取得";
			this.BtnDevInf.Click += new System.EventHandler(this.BtnDevInf_Click);
			// 
			// DlgDev
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(552, 437);
			this.Controls.Add(this.LblDev);
			this.Controls.Add(this.LblHndl);
			this.Controls.Add(this.BtnDevCls);
			this.Controls.Add(this.BtnDevOpn);
			this.Controls.Add(this.CmbID);
			this.Controls.Add(this.Label1);
			this.Controls.Add(this.BtnDevInf);
			this.Name = "DlgDev";
			this.Text = "デバイスオープン/クローズ";
			this.Load += new System.EventHandler(this.DlgDev_Load);
			this.Closed += new System.EventHandler(this.DlgDev_Closed);
			this.ResumeLayout(false);

		}
		#endregion
		
		private Hicpd530.HPCDEVICEINFO hpcDevInfo = new Hicpd530.HPCDEVICEINFO();

		// フォームロード処理
		private void DlgDev_Load(object sender, System.EventArgs e)
		{
			CmbID.Text = "";
		}

		// フォームが閉じようとする時の処理
		private void DlgDev_Closed(object sender, System.EventArgs e)
		{
			DlgInx f0 = new DlgInx();

			this.Dispose();
			f0.Show();
		}

		///-----------------------
		///  デバイス情報取得
		///-----------------------
		private void BtnDevInf_Click(object sender, System.EventArgs e)
		{
			ushort nIx = 0;
			uint unRet = 0;
			string s;			
			ComboBox cid = new ComboBox();
			
			// ボード枚数 & デバイス情報取得
            unRet = Cp530l1a.hcp530_GetDevInfo(ref spc53004Class.gCnt, spc53004Class.gHpcDevInfo);

			if (unRet != 0)
			{
				s = "デバイス情報が取得できません\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
				return;
			}

			// コンボボックスの内容をリセットする
			CmbID.Items.Clear();                                       
			for( nIx = 0; nIx<spc53004Class.gCnt; nIx++)
			{
				// コンボボックスにテキストアイテムを追加する
				CmbID.Items.Add(spc53004Class.gHpcDevInfo[nIx].dwBoardID);
			}
			CmbID.Text = CmbID.Items[0].ToString();

			// ボタン使用可
			BtnDevOpn.Enabled = true;
        
			// ボタン使用不可
			BtnDevInf.Enabled = false;
       
			// コンボボックス使用可
			CmbID.Enabled = true;            
		}

		private void BtnDevOpn_Click(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			string szBuffer;
			string s;

			// デバイスオープン
			unRet = Cp530l1a.hcp530_DevOpen(ref spc53004Class.hDeviceID, ref hpcDevInfo );
			if(unRet != 0)
			{
				s = "デバイスオープンできません\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
				return;
			}
 
			// デバイスハンドル表示
			szBuffer = spc53004Class.hDeviceID.ToString("X8");
			LblHndl.Text = "デバイスハンドル：" + szBuffer + "h";

			// デバイス情報表示
			LblDev.Text = "*** デバイス情報 ***\r\n" +
					"バス番号：        " + hpcDevInfo.nBusNumber.ToString() + "\r\n" +
					"デバイス番号：    " + hpcDevInfo.nDeviceNumber.ToString() + "\r\n" +
					"I/Oアドレス：     " + hpcDevInfo.dwIoPortAddress.ToString("X8") + "h\r\n" +
					"IRQ番号：         " + hpcDevInfo.dwIrqNo.ToString() + "\r\n" +
					"管理番号：        " + hpcDevInfo.dwNumber.ToString() + "\r\n" +
					"ボードID：        " + hpcDevInfo.dwBoardID.ToString();
			// ボタン使用不可
			BtnDevOpn.Enabled = false;
       
			// ボタン使用不可
			BtnDevInf.Enabled = false;
      
			// ボタン使用不可
			CmbID.Enabled = false;
			
			// ボタン使用可
			BtnDevCls.Enabled = true;        
		}

		private void BtnDevCls_Click(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			
			// デバイスクローズ
			unRet = Cp530l1a.hcp530_DevClose(spc53004Class.hDeviceID);     
			
			// デバイスハンドル表示
			LblHndl.Text = "";               
			
			// デバイス情報表示
			LblDev.Text = "";

			// ボタン使用可
			BtnDevOpn.Enabled = true;
       
			// ボタン使用可
			BtnDevInf.Enabled = true;
      
			// コントロール使用可
			CmbID.Enabled = true;
			
			// ボタン使用不可
			BtnDevCls.Enabled = false;
		}

		private void CmbID_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// 指定されたデバイス情報を、デバイスオープン用デバイス情報領域にコピー
			hpcDevInfo = spc53004Class.gHpcDevInfo[CmbID.SelectedIndex];
		}
	}
}
