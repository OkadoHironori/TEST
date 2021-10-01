using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace spc53004
{
	/// <summary>
	/// DlgCnt の概要の説明です。
	/// </summary>
	public class DlgCnt : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.Button BtnMCst;
		internal System.Windows.Forms.Panel PnlSvon;
		internal System.Windows.Forms.RadioButton OptXsvon1;
		internal System.Windows.Forms.RadioButton OptXsvon0;
		internal System.Windows.Forms.Panel PnlAlm;
		internal System.Windows.Forms.RadioButton OptXalm1;
		internal System.Windows.Forms.RadioButton OptXalm0;
		internal System.Windows.Forms.Panel PnlEls;
		internal System.Windows.Forms.RadioButton OptXels1;
		internal System.Windows.Forms.RadioButton OptXels0;
		internal System.Windows.Forms.Label LblXerr;
		internal System.Windows.Forms.Label LblXint;
		internal System.Windows.Forms.Label LblXspd;
		internal System.Windows.Forms.Label LblXpos;
		internal System.Windows.Forms.Label Label13;
		internal System.Windows.Forms.Label Label12;
		internal System.Windows.Forms.TextBox TxtFH;
		internal System.Windows.Forms.Label Label11;
		internal System.Windows.Forms.Button BtnStp;
		internal System.Windows.Forms.Label LblSvon;
		internal System.Windows.Forms.Label LblAlm;
		internal System.Windows.Forms.Label LblMel;
		internal System.Windows.Forms.Label LblPel;
		internal System.Windows.Forms.Label LblXsvon;
		internal System.Windows.Forms.Label LblXalm;
		internal System.Windows.Forms.Label LblXmel;
		internal System.Windows.Forms.Label LblXpel;
		internal System.Windows.Forms.Button BtnMAcc;
		internal System.Windows.Forms.Button BtnPCst;
		internal System.Windows.Forms.Button BtnPAcc;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// 連続送りサンプル
		/// </summary>
		public DlgCnt()
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
			this.components = new System.ComponentModel.Container();
			this.BtnMCst = new System.Windows.Forms.Button();
			this.PnlSvon = new System.Windows.Forms.Panel();
			this.OptXsvon1 = new System.Windows.Forms.RadioButton();
			this.OptXsvon0 = new System.Windows.Forms.RadioButton();
			this.PnlAlm = new System.Windows.Forms.Panel();
			this.OptXalm1 = new System.Windows.Forms.RadioButton();
			this.OptXalm0 = new System.Windows.Forms.RadioButton();
			this.PnlEls = new System.Windows.Forms.Panel();
			this.OptXels1 = new System.Windows.Forms.RadioButton();
			this.OptXels0 = new System.Windows.Forms.RadioButton();
			this.LblXerr = new System.Windows.Forms.Label();
			this.LblXint = new System.Windows.Forms.Label();
			this.LblXspd = new System.Windows.Forms.Label();
			this.LblXpos = new System.Windows.Forms.Label();
			this.Label13 = new System.Windows.Forms.Label();
			this.Label12 = new System.Windows.Forms.Label();
			this.TxtFH = new System.Windows.Forms.TextBox();
			this.Label11 = new System.Windows.Forms.Label();
			this.BtnStp = new System.Windows.Forms.Button();
			this.LblSvon = new System.Windows.Forms.Label();
			this.LblAlm = new System.Windows.Forms.Label();
			this.LblMel = new System.Windows.Forms.Label();
			this.LblPel = new System.Windows.Forms.Label();
			this.LblXsvon = new System.Windows.Forms.Label();
			this.LblXalm = new System.Windows.Forms.Label();
			this.LblXmel = new System.Windows.Forms.Label();
			this.LblXpel = new System.Windows.Forms.Label();
			this.BtnMAcc = new System.Windows.Forms.Button();
			this.BtnPCst = new System.Windows.Forms.Button();
			this.BtnPAcc = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.PnlSvon.SuspendLayout();
			this.PnlAlm.SuspendLayout();
			this.PnlEls.SuspendLayout();
			this.SuspendLayout();
			// 
			// BtnMCst
			// 
			this.BtnMCst.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.BtnMCst.Location = new System.Drawing.Point(8, 112);
			this.BtnMCst.Name = "BtnMCst";
			this.BtnMCst.Size = new System.Drawing.Size(160, 24);
			this.BtnMCst.TabIndex = 100;
			this.BtnMCst.Text = "－定速連続送り";
			this.BtnMCst.Click += new System.EventHandler(this.BtnMCst_Click);
			// 
			// PnlSvon
			// 
			this.PnlSvon.Controls.Add(this.OptXsvon1);
			this.PnlSvon.Controls.Add(this.OptXsvon0);
			this.PnlSvon.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.PnlSvon.Location = new System.Drawing.Point(280, 168);
			this.PnlSvon.Name = "PnlSvon";
			this.PnlSvon.Size = new System.Drawing.Size(105, 25);
			this.PnlSvon.TabIndex = 120;
			// 
			// OptXsvon1
			// 
			this.OptXsvon1.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXsvon1.Location = new System.Drawing.Point(8, 0);
			this.OptXsvon1.Name = "OptXsvon1";
			this.OptXsvon1.Size = new System.Drawing.Size(41, 25);
			this.OptXsvon1.TabIndex = 1;
			this.OptXsvon1.Text = "ON";
			this.OptXsvon1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OptXsvon0
			// 
			this.OptXsvon0.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXsvon0.Location = new System.Drawing.Point(56, 0);
			this.OptXsvon0.Name = "OptXsvon0";
			this.OptXsvon0.Size = new System.Drawing.Size(41, 25);
			this.OptXsvon0.TabIndex = 0;
			this.OptXsvon0.Text = "OFF";
			this.OptXsvon0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// PnlAlm
			// 
			this.PnlAlm.Controls.Add(this.OptXalm1);
			this.PnlAlm.Controls.Add(this.OptXalm0);
			this.PnlAlm.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.PnlAlm.Location = new System.Drawing.Point(280, 72);
			this.PnlAlm.Name = "PnlAlm";
			this.PnlAlm.Size = new System.Drawing.Size(105, 25);
			this.PnlAlm.TabIndex = 119;
			// 
			// OptXalm1
			// 
			this.OptXalm1.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXalm1.Location = new System.Drawing.Point(8, 0);
			this.OptXalm1.Name = "OptXalm1";
			this.OptXalm1.Size = new System.Drawing.Size(41, 25);
			this.OptXalm1.TabIndex = 1;
			this.OptXalm1.Text = "A接";
			this.OptXalm1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OptXalm0
			// 
			this.OptXalm0.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXalm0.Location = new System.Drawing.Point(56, 0);
			this.OptXalm0.Name = "OptXalm0";
			this.OptXalm0.Size = new System.Drawing.Size(41, 25);
			this.OptXalm0.TabIndex = 0;
			this.OptXalm0.Text = "B接";
			this.OptXalm0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// PnlEls
			// 
			this.PnlEls.Controls.Add(this.OptXels1);
			this.PnlEls.Controls.Add(this.OptXels0);
			this.PnlEls.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.PnlEls.Location = new System.Drawing.Point(280, 8);
			this.PnlEls.Name = "PnlEls";
			this.PnlEls.Size = new System.Drawing.Size(105, 25);
			this.PnlEls.TabIndex = 118;
			// 
			// OptXels1
			// 
			this.OptXels1.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXels1.Location = new System.Drawing.Point(8, 0);
			this.OptXels1.Name = "OptXels1";
			this.OptXels1.Size = new System.Drawing.Size(41, 25);
			this.OptXels1.TabIndex = 1;
			this.OptXels1.Text = "A接";
			this.OptXels1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OptXels0
			// 
			this.OptXels0.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXels0.Location = new System.Drawing.Point(56, 0);
			this.OptXels0.Name = "OptXels0";
			this.OptXels0.Size = new System.Drawing.Size(41, 25);
			this.OptXels0.TabIndex = 0;
			this.OptXels0.Text = "B接";
			this.OptXels0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LblXerr
			// 
			this.LblXerr.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblXerr.Location = new System.Drawing.Point(8, 312);
			this.LblXerr.Name = "LblXerr";
			this.LblXerr.Size = new System.Drawing.Size(185, 25);
			this.LblXerr.TabIndex = 117;
			// 
			// LblXint
			// 
			this.LblXint.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblXint.Location = new System.Drawing.Point(8, 280);
			this.LblXint.Name = "LblXint";
			this.LblXint.Size = new System.Drawing.Size(185, 25);
			this.LblXint.TabIndex = 116;
			// 
			// LblXspd
			// 
			this.LblXspd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXspd.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblXspd.Location = new System.Drawing.Point(264, 240);
			this.LblXspd.Name = "LblXspd";
			this.LblXspd.Size = new System.Drawing.Size(113, 17);
			this.LblXspd.TabIndex = 115;
			this.LblXspd.Text = "0";
			this.LblXspd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// LblXpos
			// 
			this.LblXpos.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXpos.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblXpos.Location = new System.Drawing.Point(264, 216);
			this.LblXpos.Name = "LblXpos";
			this.LblXpos.Size = new System.Drawing.Size(113, 17);
			this.LblXpos.TabIndex = 114;
			this.LblXpos.Text = "0";
			this.LblXpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// Label13
			// 
			this.Label13.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.Label13.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.Label13.Location = new System.Drawing.Point(192, 240);
			this.Label13.Name = "Label13";
			this.Label13.Size = new System.Drawing.Size(67, 18);
			this.Label13.TabIndex = 113;
			this.Label13.Text = "現在速度";
			// 
			// Label12
			// 
			this.Label12.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.Label12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.Label12.Location = new System.Drawing.Point(192, 216);
			this.Label12.Name = "Label12";
			this.Label12.Size = new System.Drawing.Size(67, 18);
			this.Label12.TabIndex = 112;
			this.Label12.Text = "現在位置";
			// 
			// TxtFH
			// 
			this.TxtFH.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.TxtFH.Location = new System.Drawing.Point(8, 232);
			this.TxtFH.Name = "TxtFH";
			this.TxtFH.Size = new System.Drawing.Size(161, 22);
			this.TxtFH.TabIndex = 102;
			this.TxtFH.Text = "";
			this.TxtFH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// Label11
			// 
			this.Label11.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.Label11.Location = new System.Drawing.Point(40, 208);
			this.Label11.Name = "Label11";
			this.Label11.Size = new System.Drawing.Size(106, 18);
			this.Label11.TabIndex = 111;
			this.Label11.Text = "動作速度(PPS)";
			this.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// BtnStp
			// 
			this.BtnStp.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.BtnStp.Location = new System.Drawing.Point(8, 152);
			this.BtnStp.Name = "BtnStp";
			this.BtnStp.Size = new System.Drawing.Size(161, 25);
			this.BtnStp.TabIndex = 101;
			this.BtnStp.Text = "停止";
			this.BtnStp.Click += new System.EventHandler(this.BtnStp_Click);
			// 
			// LblSvon
			// 
			this.LblSvon.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblSvon.Location = new System.Drawing.Point(232, 168);
			this.LblSvon.Name = "LblSvon";
			this.LblSvon.Size = new System.Drawing.Size(41, 17);
			this.LblSvon.TabIndex = 110;
			this.LblSvon.Text = "SVON";
			this.LblSvon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LblAlm
			// 
			this.LblAlm.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblAlm.Location = new System.Drawing.Point(232, 72);
			this.LblAlm.Name = "LblAlm";
			this.LblAlm.Size = new System.Drawing.Size(49, 17);
			this.LblAlm.TabIndex = 109;
			this.LblAlm.Text = "SVALM";
			this.LblAlm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LblMel
			// 
			this.LblMel.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblMel.Location = new System.Drawing.Point(232, 40);
			this.LblMel.Name = "LblMel";
			this.LblMel.Size = new System.Drawing.Size(41, 17);
			this.LblMel.TabIndex = 108;
			this.LblMel.Text = "-ELS";
			this.LblMel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LblPel
			// 
			this.LblPel.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblPel.Location = new System.Drawing.Point(232, 8);
			this.LblPel.Name = "LblPel";
			this.LblPel.Size = new System.Drawing.Size(41, 17);
			this.LblPel.TabIndex = 107;
			this.LblPel.Text = "+ELS";
			this.LblPel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LblXsvon
			// 
			this.LblXsvon.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.LblXsvon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXsvon.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblXsvon.Location = new System.Drawing.Point(200, 168);
			this.LblXsvon.Name = "LblXsvon";
			this.LblXsvon.Size = new System.Drawing.Size(17, 17);
			this.LblXsvon.TabIndex = 106;
			// 
			// LblXalm
			// 
			this.LblXalm.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.LblXalm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXalm.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblXalm.Location = new System.Drawing.Point(200, 72);
			this.LblXalm.Name = "LblXalm";
			this.LblXalm.Size = new System.Drawing.Size(17, 17);
			this.LblXalm.TabIndex = 105;
			// 
			// LblXmel
			// 
			this.LblXmel.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.LblXmel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXmel.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblXmel.Location = new System.Drawing.Point(200, 40);
			this.LblXmel.Name = "LblXmel";
			this.LblXmel.Size = new System.Drawing.Size(17, 17);
			this.LblXmel.TabIndex = 104;
			// 
			// LblXpel
			// 
			this.LblXpel.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.LblXpel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXpel.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblXpel.Location = new System.Drawing.Point(200, 8);
			this.LblXpel.Name = "LblXpel";
			this.LblXpel.Size = new System.Drawing.Size(17, 17);
			this.LblXpel.TabIndex = 103;
			// 
			// BtnMAcc
			// 
			this.BtnMAcc.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.BtnMAcc.Location = new System.Drawing.Point(8, 80);
			this.BtnMAcc.Name = "BtnMAcc";
			this.BtnMAcc.Size = new System.Drawing.Size(161, 25);
			this.BtnMAcc.TabIndex = 99;
			this.BtnMAcc.Text = "－高速連続送り";
			this.BtnMAcc.Click += new System.EventHandler(this.BtnMAcc_Click);
			// 
			// BtnPCst
			// 
			this.BtnPCst.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.BtnPCst.Location = new System.Drawing.Point(8, 40);
			this.BtnPCst.Name = "BtnPCst";
			this.BtnPCst.Size = new System.Drawing.Size(161, 25);
			this.BtnPCst.TabIndex = 98;
			this.BtnPCst.Text = "＋定速連続送り";
			this.BtnPCst.Click += new System.EventHandler(this.BtnPCst_Click);
			// 
			// BtnPAcc
			// 
			this.BtnPAcc.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.BtnPAcc.Location = new System.Drawing.Point(8, 8);
			this.BtnPAcc.Name = "BtnPAcc";
			this.BtnPAcc.Size = new System.Drawing.Size(161, 25);
			this.BtnPAcc.TabIndex = 97;
			this.BtnPAcc.Text = "＋高速連続送り";
			this.BtnPAcc.Click += new System.EventHandler(this.BtnPAcc_Click);
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// DlgCnt
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(552, 437);
			this.Controls.Add(this.BtnMCst);
			this.Controls.Add(this.PnlSvon);
			this.Controls.Add(this.PnlAlm);
			this.Controls.Add(this.PnlEls);
			this.Controls.Add(this.LblXerr);
			this.Controls.Add(this.LblXint);
			this.Controls.Add(this.LblXspd);
			this.Controls.Add(this.LblXpos);
			this.Controls.Add(this.Label13);
			this.Controls.Add(this.Label12);
			this.Controls.Add(this.TxtFH);
			this.Controls.Add(this.Label11);
			this.Controls.Add(this.BtnStp);
			this.Controls.Add(this.LblSvon);
			this.Controls.Add(this.LblAlm);
			this.Controls.Add(this.LblMel);
			this.Controls.Add(this.LblPel);
			this.Controls.Add(this.LblXsvon);
			this.Controls.Add(this.LblXalm);
			this.Controls.Add(this.LblXmel);
			this.Controls.Add(this.LblXpel);
			this.Controls.Add(this.BtnMAcc);
			this.Controls.Add(this.BtnPCst);
			this.Controls.Add(this.BtnPAcc);
			this.Name = "DlgCnt";
			this.Text = "Ｘ軸連続送り動作";
			this.Load += new System.EventHandler(this.DlgCnt_Load);
			this.Closed += new System.EventHandler(this.DlgCnt_Closed);
			this.PnlSvon.ResumeLayout(false);
			this.PnlAlm.ResumeLayout(false);
			this.PnlEls.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		
		// ラジオボタンの配列を準備
		private System.Windows.Forms.RadioButton[] OptXels;
		private System.Windows.Forms.RadioButton[] OptXalm;
		private System.Windows.Forms.RadioButton[] OptXsvon;

		// 動作判断フラグ(0:高速，1:定速)
		private ushort gStart = 0;

		// フォームが閉じようとする時の処理
		private void DlgCnt_Closed(object sender, System.EventArgs e)
		{
			DlgInx f0 = new DlgInx();
			uint unRet = 0;

			// タイマー停止
			timer1.Enabled = false;

			// 停止コマンド
			unRet = Cp530l1a.hcp530_QuickStop(spc53004Class.hDeviceID, 1);

			// デバイスクローズ
			unRet = Cp530l1a.hcp530_DevClose(spc53004Class.hDeviceID);
			this.Dispose();
			f0.Show();
		}

		// フォームロード処理
		private void DlgCnt_Load(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;

			// ボタン設定
			OptXels0.Checked = true;
			OptXalm0.Checked = true;
			OptXsvon0.Checked = true;

			// 動作速度エディタ設定
			TxtFH.Text = "4000";
			TxtFH.MaxLength = 5;

			// ベース速度設定(400pps)
			unRet = Cp530l1a.hcp530_SetFLSpd(hDev, 0, 400);
			if (0 != unRet)
			{
				s = "エラーが発生しました!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
				return;
			}

			// ステータス表示用タイマー開始
			timer1.Enabled = true;

			//ボタンコントロール配列の作成
			this.OptXels = new System.Windows.Forms.RadioButton[]
				{this.OptXels0, this.OptXels1};

			this.OptXalm = new System.Windows.Forms.RadioButton[]
				{this.OptXalm0, this.OptXalm1};
						
			this.OptXsvon = new System.Windows.Forms.RadioButton[]
				{this.OptXsvon0, this.OptXsvon1};

			//イベントハンドラに関連付け
			for (int i = 0; i < this.OptXels.Length; i++)
				this.OptXels[i].Click +=
					new EventHandler(this.OptXels_Click);
			for (int i = 0; i < this.OptXalm.Length; i++)
				this.OptXalm[i].Click +=
					new EventHandler(this.OptXalm_Click);
			for (int i = 0; i < this.OptXsvon.Length; i++)
				this.OptXsvon[i].Click +=
					new EventHandler(this.OptXsvon_Click);
		}

		// Ｘ軸ELS入力極性
		private void OptXels_Click(object sender, EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;
			ushort Index = 0;

			for (int i = 0; i < OptXels.Length; i++)
			{
				if (sender.Equals(OptXels[i])==true)
				{
					Index = (ushort)i;
					break;
				}
			}
			
			unRet = Cp530l1a.hcp530_SetEls(hDev, 0, Index, 0);
			if (0 != unRet)
			{
				s = "エラーが発生しました!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}
		}

		// Ｘ軸SVALM入力極性
		private void OptXalm_Click(object sender, EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;
			ushort Index = 0;

			for (int i = 0; i < OptXalm.Length; i++)
			{
				if (sender.Equals(OptXalm[i])==true)
				{
					Index = (ushort)i;
					break;
				}
			}
			unRet = Cp530l1a.hcp530_SetSvAlm(hDev, 0, Index, 0);
			if (0 != unRet)
			{
				s = "エラーが発生しました!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}
		}

		// Ｘ軸サーボオン
		private void OptXsvon_Click(object sender, EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;
			ushort Index = 0;

			for (int i = 0; i < OptXsvon.Length; i++)
			{
				if (sender.Equals(OptXsvon[i])==true)
				{
					Index = (ushort)i;
					break;
				}
			}
			if (0==Index)
			{
				unRet = Cp530l1a.hcp530_SvOff(hDev, 1);
			}
			else if(1==Index)
			{
				unRet = Cp530l1a.hcp530_SvOn(hDev, 1);
			}
			if (0 != unRet)
			{
				s = "エラーが発生しました!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}		
		}

		// ステータス等読込と表示
		private void timer1_Tick(object sender, System.EventArgs e)
		{
			ushort msts = 0;
			ushort ssts = 0;
			uint unRet = 0;
			uint unErr = 0;
			uint unInt = 0;
			int nPos = 0;
			ushort usSpd = 0;
			uint hDev = spc53004Class.hDeviceID;

			// Ｘ軸メインステータスの読込
			unRet = Cp530l1a.hcp530_ReadMainSts(hDev, 0, ref msts);
			if (0==unRet)
			{
				// スタートコマンド終了
				if ((msts & 0x30) != 0)
				{
					// エラー報告
					if ((msts & 0x10) == 0x10)
					{
						LblXint.Text = "エラー終了";
						// X軸ｴﾗｰｽﾃｰﾀｽ読込
						unRet = Cp530l1a.hcp530_ReadErrorSts(hDev, 0, ref unErr);
						if (0 == unRet)
						{
							if((unErr & 0x20) == 0x20)
							{
								LblXerr.Text = "+XELS入力ONによる停止";
							}
							if((unErr & 0x40) == 0x40)
							{
								LblXerr.Text = "-XELS入力ONによる停止";
							}
							if((unErr & 0x80) == 0x80)
							{
								LblXerr.Text = "XSVALM入力ONによる停止";
							}
						}
						else
						{
							return;
						}
					}
					// イベント報告(正常停止)
					if((msts & 0x20) == 0x20)
					{
						// X軸ｲﾍﾞﾝﾄｽﾃｰﾀｽ読込
						unRet = Cp530l1a.hcp530_ReadEventSts(hDev, 0, ref unInt);
						if (0 == unRet)
						{
							if((unInt & 0x01)==0x01)
							{
								LblXint.Text = "正常停止";
							}
						}
						else
						{
							return;
						}
					}
					// ボタン使用可
					BtnPAcc.Enabled = true;
					BtnPCst.Enabled = true;
					BtnMAcc.Enabled = true;
					BtnMCst.Enabled = true;
				}
			}
			else
			{
				return;
			}

			// Ｘ軸サブステータスの読込
			unRet = Cp530l1a.hcp530_ReadSubSts(hDev, 0, ref ssts);
			if(0==unRet)
			{
				// SVON 結果表示(SVON ON：緑色表示，SVON OFF：白色表示)
				if(0x1==(ssts & 0x1))
				{
					LblXsvon.BackColor = Color.LimeGreen;
				}
				else
				{
					LblXsvon.BackColor = Color.White;
				}
				// SVALM 結果表示(SVALM ON：赤色表示，SVALM OFF：白色表示)
				if(0x800==(ssts & 0x800))
				{
					LblXalm.BackColor = Color.Red;
				}
				else
				{
					LblXalm.BackColor = Color.White;
				}
				// +ELS 結果表示(ELS ON：赤色表示，ELS OFF：白色表示)
				if(0x1000==(ssts & 0x1000))
				{
					LblXpel.BackColor = Color.Red;
				}
				else
				{
					LblXpel.BackColor = Color.White;
				}
				// -ELS 結果表示(ELS ON：赤色表示，ELS OFF：白色表示)
				if(0x2000==(ssts & 0x2000))
				{
					LblXmel.BackColor = Color.Red;
				}
				else
				{
					LblXmel.BackColor = Color.White;
				}
			}
			else
			{
				return;
			}

			// Ｘ軸現在指令位置の読込
			unRet = Cp530l1a.hcp530_ReadCtr(hDev, 0, 1, ref nPos);
			if (0==unRet)
			{
				// 現在指令位置の表示
				LblXpos.Text = nPos.ToString();   
			}
			else
			{
				return;
			}
			
			// Ｘ軸現在速度の読込
			unRet = Cp530l1a.hcp530_ReadSpd(hDev, 0, ref usSpd);
			if (0==unRet)
			{
				//' 現在速度の表示
				LblXspd.Text = usSpd.ToString();
			}
			else
			{
				return;
			}
		}

		// Ｘ軸：+連続送り加減速スタート
		private void BtnPAcc_Click(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			uint fh = 1;
			string s;

			// テキストをinteger型に変換して、変数に代入します．
			try
			{
				fh = uint.Parse(TxtFH.Text);
			}
			catch
			{
				// テキストが数字以外だった場合、メッセージを出します．
				MessageBox.Show("動作速度が設定範囲外です．");
				return;
			}

			// 動作速度設定
			unRet = Cp530l1a.hcp530_WritFHSpd(hDev, 0, fh);    
			if (0 != unRet)
			{
				s = "動作速度設定範囲外です!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}		

			// 動作モード設定(+連続送り)
			unRet = Cp530l1a.hcp530_WritOpeMode(hDev, 0, 0);   
			if (0 == unRet)
			{
				// 加速スタート
				unRet = Cp530l1a.hcp530_AccStart(hDev, 1);    
					if (0 == unRet)
					{
						// コントロール表示等
						BtnPAcc.Enabled = false;
						BtnPCst.Enabled = false;
						BtnMAcc.Enabled = false;
						BtnMCst.Enabled = false;
						LblXerr.Text = "";
						LblXint.Text = "";
					
						// 加減速動作
						gStart = 0; 
					}
			}
			if (0 != unRet)
			{
				s = "エラーが発生しました!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}		
		}

		// Ｘ軸：+連続送り定速スタート
		private void BtnPCst_Click(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			uint fh = 1;
			string s;

			// テキストをinteger型に変換して、変数に代入します．
			try
			{
				fh = uint.Parse(TxtFH.Text);
			}
			catch
			{
				// テキストが数字以外だった場合、メッセージを出します．
				MessageBox.Show("動作速度が設定範囲外です．");
				return;
			}

			// 動作速度設定
			unRet = Cp530l1a.hcp530_WritFHSpd(hDev, 0, fh);    
			if (0 != unRet)
			{
				s = "動作速度設定範囲外です!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}		

			// 動作モード設定(+連続送り)
			unRet = Cp530l1a.hcp530_WritOpeMode(hDev, 0, 0);   
			if (0 == unRet)
			{
				// FH定速スタート
				unRet = Cp530l1a.hcp530_CnstStartFH(hDev, 1);    
				if (0 == unRet)
				{
					// コントロール表示等
					BtnPAcc.Enabled = false;
					BtnPCst.Enabled = false;
					BtnMAcc.Enabled = false;
					BtnMCst.Enabled = false;
					LblXerr.Text = "";
					LblXint.Text = "";
					
					// 定速動作
					gStart = 1; 
				}
			}
			if (0 != unRet)
			{
				s = "エラーが発生しました!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}				
		}

		// Ｘ軸：-連続送り加減速スタート
		private void BtnMAcc_Click(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			uint fh = 1;
			string s;

			// テキストをinteger型に変換して、変数に代入します．
			try
			{
				fh = uint.Parse(TxtFH.Text);
			}
			catch
			{
				// テキストが数字以外だった場合、メッセージを出します．
				MessageBox.Show("動作速度が設定範囲外です．");
				return;
			}

			// 動作速度設定
			unRet = Cp530l1a.hcp530_WritFHSpd(hDev, 0, fh);    
			if (0 != unRet)
			{
				s = "動作速度設定範囲外です!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}		

			// 動作モード設定(-連続送り)
			unRet = Cp530l1a.hcp530_WritOpeMode(hDev, 0, 8);   
			if (0 == unRet)
			{
				// 加速スタート
				unRet = Cp530l1a.hcp530_AccStart(hDev, 1);    
				if (0 == unRet)
				{
					// コントロール表示等
					BtnPAcc.Enabled = false;
					BtnPCst.Enabled = false;
					BtnMAcc.Enabled = false;
					BtnMCst.Enabled = false;
					LblXerr.Text = "";
					LblXint.Text = "";
					
					// 加減速動作
					gStart = 0; 
				}
			}
			if (0 != unRet)
			{
				s = "エラーが発生しました!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}				
		}

		// Ｘ軸：-連続送り定速スタート
		private void BtnMCst_Click(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			uint fh = 1;
			string s;

			// テキストをinteger型に変換して、変数に代入します．
			try
			{
				fh = uint.Parse(TxtFH.Text);
			}
			catch
			{
				// テキストが数字以外だった場合、メッセージを出します．
				MessageBox.Show("動作速度が設定範囲外です．");
				return;
			}

			// 動作速度設定
			unRet = Cp530l1a.hcp530_WritFHSpd(hDev, 0, fh);    
			if (0 != unRet)
			{
				s = "動作速度設定範囲外です!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}		

			// 動作モード設定(-連続送り)
			unRet = Cp530l1a.hcp530_WritOpeMode(hDev, 0, 8);   
			if (0 == unRet)
			{
				// FH定速スタート
				unRet = Cp530l1a.hcp530_CnstStartFH(hDev, 1);    
				if (0 == unRet)
				{
					// コントロール表示等
					BtnPAcc.Enabled = false;
					BtnPCst.Enabled = false;
					BtnMAcc.Enabled = false;
					BtnMCst.Enabled = false;
					LblXerr.Text = "";
					LblXint.Text = "";
					
					// 定速動作
					gStart = 1; 
				}
			}
			if (0 != unRet)
			{
				s = "エラーが発生しました!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}				
		}

		private void BtnStp_Click(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;

			if (0 == gStart)
			{
				// 加減速動作ならば減速停止
				unRet = Cp530l1a.hcp530_DecStop(hDev, 1);
			}
			else
			{
				// 定速動作ならば即停止
				unRet = Cp530l1a.hcp530_QuickStop(hDev, 1);
			}
			if (0 != unRet)
			{
				s = "エラーが発生しました!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}		
		}
	}
}
