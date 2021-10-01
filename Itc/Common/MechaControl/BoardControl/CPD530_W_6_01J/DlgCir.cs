using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace spc53004
{
	/// <summary>
	/// DlgCir の概要の説明です。
	/// </summary>
	public class DlgCir : System.Windows.Forms.Form
	{
		public System.Windows.Forms.Label LblXspd;
		public System.Windows.Forms.Button BtnCcw;
		public System.Windows.Forms.Button BtnStp;
		public System.Windows.Forms.Button BtnCw;
		public System.Windows.Forms.TextBox TxtYCntr;
		public System.Windows.Forms.TextBox TxtXCntr;
		public System.Windows.Forms.TextBox TxtYEnd;
		public System.Windows.Forms.TextBox TxtXEnd;
		public System.Windows.Forms.GroupBox FraYax;
		public System.Windows.Forms.Panel FraYSvon;
		public System.Windows.Forms.RadioButton OptYsvon1;
		public System.Windows.Forms.RadioButton OptYsvon0;
		public System.Windows.Forms.Panel FraYAlm;
		public System.Windows.Forms.RadioButton OptYalm1;
		public System.Windows.Forms.RadioButton OptYalm0;
		public System.Windows.Forms.Panel FraYEls;
		public System.Windows.Forms.RadioButton OptYels1;
		public System.Windows.Forms.RadioButton OptYels0;
		public System.Windows.Forms.Label LblYpos;
		public System.Windows.Forms.Label Label6;
		public System.Windows.Forms.Label LblSvon2;
		public System.Windows.Forms.Label LblYsvon;
		public System.Windows.Forms.Label LblYalm;
		public System.Windows.Forms.Label LblYmel;
		public System.Windows.Forms.Label LblYpel;
		public System.Windows.Forms.Label LblAlm2;
		public System.Windows.Forms.Label LblMel2;
		public System.Windows.Forms.Label LblPel2;
		public System.Windows.Forms.GroupBox FraXax;
		public System.Windows.Forms.Label LblXpos;
		public System.Windows.Forms.Panel FraXEls;
		public System.Windows.Forms.RadioButton OptXels0;
		public System.Windows.Forms.RadioButton OptXels1;
		public System.Windows.Forms.Panel FraXAlm;
		public System.Windows.Forms.RadioButton OptXalm0;
		public System.Windows.Forms.RadioButton OptXalm1;
		public System.Windows.Forms.Panel FraXSvon;
		public System.Windows.Forms.RadioButton OptXsvon0;
		public System.Windows.Forms.RadioButton OptXsvon1;
		public System.Windows.Forms.Label LblPel1;
		public System.Windows.Forms.Label LblMel1;
		public System.Windows.Forms.Label LblAlm1;
		public System.Windows.Forms.Label LblXpel;
		public System.Windows.Forms.Label LblXmel;
		public System.Windows.Forms.Label LblXalm;
		public System.Windows.Forms.Label LblXsvon;
		public System.Windows.Forms.Label LblSvon1;
		public System.Windows.Forms.Label Label5;
		public System.Windows.Forms.Label Label4;
		public System.Windows.Forms.Label Label3;
		public System.Windows.Forms.Label Label2;
		public System.Windows.Forms.Label Label1;
		public System.Windows.Forms.Label Label7;
		public System.Windows.Forms.Label LblYerr;
		public System.Windows.Forms.Label LblXint;
		public System.Windows.Forms.Label LblXerr;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// 円弧補間サンプル
		/// </summary>
		public DlgCir()
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
			this.LblXspd = new System.Windows.Forms.Label();
			this.BtnCcw = new System.Windows.Forms.Button();
			this.BtnStp = new System.Windows.Forms.Button();
			this.BtnCw = new System.Windows.Forms.Button();
			this.TxtYCntr = new System.Windows.Forms.TextBox();
			this.TxtXCntr = new System.Windows.Forms.TextBox();
			this.TxtYEnd = new System.Windows.Forms.TextBox();
			this.TxtXEnd = new System.Windows.Forms.TextBox();
			this.FraYax = new System.Windows.Forms.GroupBox();
			this.FraYSvon = new System.Windows.Forms.Panel();
			this.OptYsvon1 = new System.Windows.Forms.RadioButton();
			this.OptYsvon0 = new System.Windows.Forms.RadioButton();
			this.FraYAlm = new System.Windows.Forms.Panel();
			this.OptYalm1 = new System.Windows.Forms.RadioButton();
			this.OptYalm0 = new System.Windows.Forms.RadioButton();
			this.FraYEls = new System.Windows.Forms.Panel();
			this.OptYels1 = new System.Windows.Forms.RadioButton();
			this.OptYels0 = new System.Windows.Forms.RadioButton();
			this.LblYpos = new System.Windows.Forms.Label();
			this.Label6 = new System.Windows.Forms.Label();
			this.LblSvon2 = new System.Windows.Forms.Label();
			this.LblYsvon = new System.Windows.Forms.Label();
			this.LblYalm = new System.Windows.Forms.Label();
			this.LblYmel = new System.Windows.Forms.Label();
			this.LblYpel = new System.Windows.Forms.Label();
			this.LblAlm2 = new System.Windows.Forms.Label();
			this.LblMel2 = new System.Windows.Forms.Label();
			this.LblPel2 = new System.Windows.Forms.Label();
			this.FraXax = new System.Windows.Forms.GroupBox();
			this.LblXpos = new System.Windows.Forms.Label();
			this.FraXEls = new System.Windows.Forms.Panel();
			this.OptXels0 = new System.Windows.Forms.RadioButton();
			this.OptXels1 = new System.Windows.Forms.RadioButton();
			this.FraXAlm = new System.Windows.Forms.Panel();
			this.OptXalm0 = new System.Windows.Forms.RadioButton();
			this.OptXalm1 = new System.Windows.Forms.RadioButton();
			this.FraXSvon = new System.Windows.Forms.Panel();
			this.OptXsvon0 = new System.Windows.Forms.RadioButton();
			this.OptXsvon1 = new System.Windows.Forms.RadioButton();
			this.LblPel1 = new System.Windows.Forms.Label();
			this.LblMel1 = new System.Windows.Forms.Label();
			this.LblAlm1 = new System.Windows.Forms.Label();
			this.LblXpel = new System.Windows.Forms.Label();
			this.LblXmel = new System.Windows.Forms.Label();
			this.LblXalm = new System.Windows.Forms.Label();
			this.LblXsvon = new System.Windows.Forms.Label();
			this.LblSvon1 = new System.Windows.Forms.Label();
			this.Label5 = new System.Windows.Forms.Label();
			this.Label4 = new System.Windows.Forms.Label();
			this.Label3 = new System.Windows.Forms.Label();
			this.Label2 = new System.Windows.Forms.Label();
			this.Label1 = new System.Windows.Forms.Label();
			this.Label7 = new System.Windows.Forms.Label();
			this.LblYerr = new System.Windows.Forms.Label();
			this.LblXint = new System.Windows.Forms.Label();
			this.LblXerr = new System.Windows.Forms.Label();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.FraYax.SuspendLayout();
			this.FraYSvon.SuspendLayout();
			this.FraYAlm.SuspendLayout();
			this.FraYEls.SuspendLayout();
			this.FraXax.SuspendLayout();
			this.FraXEls.SuspendLayout();
			this.FraXAlm.SuspendLayout();
			this.FraXSvon.SuspendLayout();
			this.SuspendLayout();
			// 
			// LblXspd
			// 
			this.LblXspd.BackColor = System.Drawing.SystemColors.Control;
			this.LblXspd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXspd.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblXspd.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblXspd.Location = new System.Drawing.Point(424, 288);
			this.LblXspd.Name = "LblXspd";
			this.LblXspd.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblXspd.Size = new System.Drawing.Size(105, 17);
			this.LblXspd.TabIndex = 98;
			this.LblXspd.Text = "0";
			this.LblXspd.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// BtnCcw
			// 
			this.BtnCcw.BackColor = System.Drawing.SystemColors.Control;
			this.BtnCcw.Cursor = System.Windows.Forms.Cursors.Default;
			this.BtnCcw.ForeColor = System.Drawing.SystemColors.ControlText;
			this.BtnCcw.Location = new System.Drawing.Point(8, 48);
			this.BtnCcw.Name = "BtnCcw";
			this.BtnCcw.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.BtnCcw.Size = new System.Drawing.Size(137, 25);
			this.BtnCcw.TabIndex = 90;
			this.BtnCcw.Text = "円弧補間(CCW)";
			this.BtnCcw.Click += new System.EventHandler(this.BtnCcw_Click);
			// 
			// BtnStp
			// 
			this.BtnStp.BackColor = System.Drawing.SystemColors.Control;
			this.BtnStp.Cursor = System.Windows.Forms.Cursors.Default;
			this.BtnStp.ForeColor = System.Drawing.SystemColors.ControlText;
			this.BtnStp.Location = new System.Drawing.Point(8, 80);
			this.BtnStp.Name = "BtnStp";
			this.BtnStp.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.BtnStp.Size = new System.Drawing.Size(137, 25);
			this.BtnStp.TabIndex = 91;
			this.BtnStp.Text = "停止";
			this.BtnStp.Click += new System.EventHandler(this.BtnStp_Click);
			// 
			// BtnCw
			// 
			this.BtnCw.BackColor = System.Drawing.SystemColors.Control;
			this.BtnCw.Cursor = System.Windows.Forms.Cursors.Default;
			this.BtnCw.ForeColor = System.Drawing.SystemColors.ControlText;
			this.BtnCw.Location = new System.Drawing.Point(8, 16);
			this.BtnCw.Name = "BtnCw";
			this.BtnCw.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.BtnCw.Size = new System.Drawing.Size(137, 25);
			this.BtnCw.TabIndex = 89;
			this.BtnCw.Text = "円弧補間(CW)";
			this.BtnCw.Click += new System.EventHandler(this.BtnCw_Click);
			// 
			// TxtYCntr
			// 
			this.TxtYCntr.AcceptsReturn = true;
			this.TxtYCntr.AutoSize = false;
			this.TxtYCntr.BackColor = System.Drawing.SystemColors.Window;
			this.TxtYCntr.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.TxtYCntr.ForeColor = System.Drawing.SystemColors.WindowText;
			this.TxtYCntr.Location = new System.Drawing.Point(8, 248);
			this.TxtYCntr.MaxLength = 10;
			this.TxtYCntr.Multiline = true;
			this.TxtYCntr.Name = "TxtYCntr";
			this.TxtYCntr.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.TxtYCntr.Size = new System.Drawing.Size(137, 22);
			this.TxtYCntr.TabIndex = 105;
			this.TxtYCntr.Text = "";
			this.TxtYCntr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// TxtXCntr
			// 
			this.TxtXCntr.AcceptsReturn = true;
			this.TxtXCntr.AutoSize = false;
			this.TxtXCntr.BackColor = System.Drawing.SystemColors.Window;
			this.TxtXCntr.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.TxtXCntr.ForeColor = System.Drawing.SystemColors.WindowText;
			this.TxtXCntr.Location = new System.Drawing.Point(8, 208);
			this.TxtXCntr.MaxLength = 10;
			this.TxtXCntr.Multiline = true;
			this.TxtXCntr.Name = "TxtXCntr";
			this.TxtXCntr.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.TxtXCntr.Size = new System.Drawing.Size(137, 22);
			this.TxtXCntr.TabIndex = 103;
			this.TxtXCntr.Text = "";
			this.TxtXCntr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// TxtYEnd
			// 
			this.TxtYEnd.AcceptsReturn = true;
			this.TxtYEnd.AutoSize = false;
			this.TxtYEnd.BackColor = System.Drawing.SystemColors.Window;
			this.TxtYEnd.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.TxtYEnd.ForeColor = System.Drawing.SystemColors.WindowText;
			this.TxtYEnd.Location = new System.Drawing.Point(8, 168);
			this.TxtYEnd.MaxLength = 10;
			this.TxtYEnd.Multiline = true;
			this.TxtYEnd.Name = "TxtYEnd";
			this.TxtYEnd.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.TxtYEnd.Size = new System.Drawing.Size(137, 22);
			this.TxtYEnd.TabIndex = 101;
			this.TxtYEnd.Text = "";
			this.TxtYEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// TxtXEnd
			// 
			this.TxtXEnd.AcceptsReturn = true;
			this.TxtXEnd.AutoSize = false;
			this.TxtXEnd.BackColor = System.Drawing.SystemColors.Window;
			this.TxtXEnd.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.TxtXEnd.ForeColor = System.Drawing.SystemColors.WindowText;
			this.TxtXEnd.Location = new System.Drawing.Point(8, 128);
			this.TxtXEnd.MaxLength = 10;
			this.TxtXEnd.Multiline = true;
			this.TxtXEnd.Name = "TxtXEnd";
			this.TxtXEnd.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.TxtXEnd.Size = new System.Drawing.Size(137, 22);
			this.TxtXEnd.TabIndex = 99;
			this.TxtXEnd.Text = "";
			this.TxtXEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// FraYax
			// 
			this.FraYax.BackColor = System.Drawing.SystemColors.Control;
			this.FraYax.Controls.Add(this.FraYSvon);
			this.FraYax.Controls.Add(this.FraYAlm);
			this.FraYax.Controls.Add(this.FraYEls);
			this.FraYax.Controls.Add(this.LblYpos);
			this.FraYax.Controls.Add(this.Label6);
			this.FraYax.Controls.Add(this.LblSvon2);
			this.FraYax.Controls.Add(this.LblYsvon);
			this.FraYax.Controls.Add(this.LblYalm);
			this.FraYax.Controls.Add(this.LblYmel);
			this.FraYax.Controls.Add(this.LblYpel);
			this.FraYax.Controls.Add(this.LblAlm2);
			this.FraYax.Controls.Add(this.LblMel2);
			this.FraYax.Controls.Add(this.LblPel2);
			this.FraYax.ForeColor = System.Drawing.SystemColors.ControlText;
			this.FraYax.Location = new System.Drawing.Point(352, 8);
			this.FraYax.Name = "FraYax";
			this.FraYax.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.FraYax.Size = new System.Drawing.Size(185, 265);
			this.FraYax.TabIndex = 93;
			this.FraYax.TabStop = false;
			this.FraYax.Text = "Y軸";
			// 
			// FraYSvon
			// 
			this.FraYSvon.BackColor = System.Drawing.SystemColors.Control;
			this.FraYSvon.Controls.Add(this.OptYsvon1);
			this.FraYSvon.Controls.Add(this.OptYsvon0);
			this.FraYSvon.Cursor = System.Windows.Forms.Cursors.Default;
			this.FraYSvon.ForeColor = System.Drawing.SystemColors.ControlText;
			this.FraYSvon.Location = new System.Drawing.Point(88, 184);
			this.FraYSvon.Name = "FraYSvon";
			this.FraYSvon.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.FraYSvon.Size = new System.Drawing.Size(89, 25);
			this.FraYSvon.TabIndex = 27;
			this.FraYSvon.Text = "Frame2";
			// 
			// OptYsvon1
			// 
			this.OptYsvon1.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptYsvon1.BackColor = System.Drawing.SystemColors.Control;
			this.OptYsvon1.Cursor = System.Windows.Forms.Cursors.Default;
			this.OptYsvon1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.OptYsvon1.Location = new System.Drawing.Point(0, 0);
			this.OptYsvon1.Name = "OptYsvon1";
			this.OptYsvon1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.OptYsvon1.Size = new System.Drawing.Size(41, 25);
			this.OptYsvon1.TabIndex = 1;
			this.OptYsvon1.TabStop = true;
			this.OptYsvon1.Text = "ON";
			this.OptYsvon1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OptYsvon0
			// 
			this.OptYsvon0.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptYsvon0.BackColor = System.Drawing.SystemColors.Control;
			this.OptYsvon0.Cursor = System.Windows.Forms.Cursors.Default;
			this.OptYsvon0.ForeColor = System.Drawing.SystemColors.ControlText;
			this.OptYsvon0.Location = new System.Drawing.Point(48, 0);
			this.OptYsvon0.Name = "OptYsvon0";
			this.OptYsvon0.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.OptYsvon0.Size = new System.Drawing.Size(41, 25);
			this.OptYsvon0.TabIndex = 0;
			this.OptYsvon0.TabStop = true;
			this.OptYsvon0.Text = "OFF";
			this.OptYsvon0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FraYAlm
			// 
			this.FraYAlm.BackColor = System.Drawing.SystemColors.Control;
			this.FraYAlm.Controls.Add(this.OptYalm1);
			this.FraYAlm.Controls.Add(this.OptYalm0);
			this.FraYAlm.Cursor = System.Windows.Forms.Cursors.Default;
			this.FraYAlm.ForeColor = System.Drawing.SystemColors.ControlText;
			this.FraYAlm.Location = new System.Drawing.Point(88, 88);
			this.FraYAlm.Name = "FraYAlm";
			this.FraYAlm.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.FraYAlm.Size = new System.Drawing.Size(89, 25);
			this.FraYAlm.TabIndex = 24;
			this.FraYAlm.Text = "Frame2";
			// 
			// OptYalm1
			// 
			this.OptYalm1.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptYalm1.BackColor = System.Drawing.SystemColors.Control;
			this.OptYalm1.Cursor = System.Windows.Forms.Cursors.Default;
			this.OptYalm1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.OptYalm1.Location = new System.Drawing.Point(0, 0);
			this.OptYalm1.Name = "OptYalm1";
			this.OptYalm1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.OptYalm1.Size = new System.Drawing.Size(41, 25);
			this.OptYalm1.TabIndex = 1;
			this.OptYalm1.TabStop = true;
			this.OptYalm1.Text = "A接";
			this.OptYalm1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OptYalm0
			// 
			this.OptYalm0.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptYalm0.BackColor = System.Drawing.SystemColors.Control;
			this.OptYalm0.Cursor = System.Windows.Forms.Cursors.Default;
			this.OptYalm0.ForeColor = System.Drawing.SystemColors.ControlText;
			this.OptYalm0.Location = new System.Drawing.Point(48, 0);
			this.OptYalm0.Name = "OptYalm0";
			this.OptYalm0.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.OptYalm0.Size = new System.Drawing.Size(41, 25);
			this.OptYalm0.TabIndex = 0;
			this.OptYalm0.TabStop = true;
			this.OptYalm0.Text = "B接";
			this.OptYalm0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FraYEls
			// 
			this.FraYEls.BackColor = System.Drawing.SystemColors.Control;
			this.FraYEls.Controls.Add(this.OptYels1);
			this.FraYEls.Controls.Add(this.OptYels0);
			this.FraYEls.Cursor = System.Windows.Forms.Cursors.Default;
			this.FraYEls.ForeColor = System.Drawing.SystemColors.ControlText;
			this.FraYEls.Location = new System.Drawing.Point(88, 24);
			this.FraYEls.Name = "FraYEls";
			this.FraYEls.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.FraYEls.Size = new System.Drawing.Size(89, 25);
			this.FraYEls.TabIndex = 21;
			this.FraYEls.Text = "Frame2";
			// 
			// OptYels1
			// 
			this.OptYels1.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptYels1.BackColor = System.Drawing.SystemColors.Control;
			this.OptYels1.Cursor = System.Windows.Forms.Cursors.Default;
			this.OptYels1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.OptYels1.Location = new System.Drawing.Point(0, 0);
			this.OptYels1.Name = "OptYels1";
			this.OptYels1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.OptYels1.Size = new System.Drawing.Size(41, 25);
			this.OptYels1.TabIndex = 1;
			this.OptYels1.TabStop = true;
			this.OptYels1.Text = "A接";
			this.OptYels1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OptYels0
			// 
			this.OptYels0.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptYels0.BackColor = System.Drawing.SystemColors.Control;
			this.OptYels0.Cursor = System.Windows.Forms.Cursors.Default;
			this.OptYels0.ForeColor = System.Drawing.SystemColors.ControlText;
			this.OptYels0.Location = new System.Drawing.Point(48, 0);
			this.OptYels0.Name = "OptYels0";
			this.OptYels0.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.OptYels0.Size = new System.Drawing.Size(41, 25);
			this.OptYels0.TabIndex = 0;
			this.OptYels0.TabStop = true;
			this.OptYels0.Text = "B接";
			this.OptYels0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LblYpos
			// 
			this.LblYpos.BackColor = System.Drawing.SystemColors.Control;
			this.LblYpos.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblYpos.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblYpos.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblYpos.Location = new System.Drawing.Point(72, 232);
			this.LblYpos.Name = "LblYpos";
			this.LblYpos.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblYpos.Size = new System.Drawing.Size(105, 17);
			this.LblYpos.TabIndex = 39;
			this.LblYpos.Text = "0";
			this.LblYpos.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// Label6
			// 
			this.Label6.AutoSize = true;
			this.Label6.BackColor = System.Drawing.SystemColors.Control;
			this.Label6.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label6.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label6.Location = new System.Drawing.Point(8, 232);
			this.Label6.Name = "Label6";
			this.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label6.Size = new System.Drawing.Size(54, 15);
			this.Label6.TabIndex = 38;
			this.Label6.Text = "現在位置";
			// 
			// LblSvon2
			// 
			this.LblSvon2.BackColor = System.Drawing.SystemColors.Control;
			this.LblSvon2.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblSvon2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblSvon2.Location = new System.Drawing.Point(40, 184);
			this.LblSvon2.Name = "LblSvon2";
			this.LblSvon2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblSvon2.Size = new System.Drawing.Size(41, 17);
			this.LblSvon2.TabIndex = 37;
			this.LblSvon2.Text = "SVON";
			// 
			// LblYsvon
			// 
			this.LblYsvon.BackColor = System.Drawing.SystemColors.Window;
			this.LblYsvon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblYsvon.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblYsvon.ForeColor = System.Drawing.SystemColors.WindowText;
			this.LblYsvon.Location = new System.Drawing.Point(8, 184);
			this.LblYsvon.Name = "LblYsvon";
			this.LblYsvon.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblYsvon.Size = new System.Drawing.Size(17, 17);
			this.LblYsvon.TabIndex = 36;
			// 
			// LblYalm
			// 
			this.LblYalm.BackColor = System.Drawing.SystemColors.Window;
			this.LblYalm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblYalm.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblYalm.ForeColor = System.Drawing.SystemColors.WindowText;
			this.LblYalm.Location = new System.Drawing.Point(8, 88);
			this.LblYalm.Name = "LblYalm";
			this.LblYalm.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblYalm.Size = new System.Drawing.Size(17, 17);
			this.LblYalm.TabIndex = 35;
			// 
			// LblYmel
			// 
			this.LblYmel.BackColor = System.Drawing.SystemColors.Window;
			this.LblYmel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblYmel.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblYmel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.LblYmel.Location = new System.Drawing.Point(8, 56);
			this.LblYmel.Name = "LblYmel";
			this.LblYmel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblYmel.Size = new System.Drawing.Size(17, 17);
			this.LblYmel.TabIndex = 34;
			// 
			// LblYpel
			// 
			this.LblYpel.BackColor = System.Drawing.SystemColors.Window;
			this.LblYpel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblYpel.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblYpel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.LblYpel.Location = new System.Drawing.Point(8, 24);
			this.LblYpel.Name = "LblYpel";
			this.LblYpel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblYpel.Size = new System.Drawing.Size(17, 17);
			this.LblYpel.TabIndex = 33;
			// 
			// LblAlm2
			// 
			this.LblAlm2.BackColor = System.Drawing.SystemColors.Control;
			this.LblAlm2.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblAlm2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblAlm2.Location = new System.Drawing.Point(40, 88);
			this.LblAlm2.Name = "LblAlm2";
			this.LblAlm2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblAlm2.Size = new System.Drawing.Size(49, 17);
			this.LblAlm2.TabIndex = 32;
			this.LblAlm2.Text = "SVALM";
			// 
			// LblMel2
			// 
			this.LblMel2.BackColor = System.Drawing.SystemColors.Control;
			this.LblMel2.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblMel2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblMel2.Location = new System.Drawing.Point(40, 56);
			this.LblMel2.Name = "LblMel2";
			this.LblMel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblMel2.Size = new System.Drawing.Size(41, 17);
			this.LblMel2.TabIndex = 31;
			this.LblMel2.Text = "-ELS";
			// 
			// LblPel2
			// 
			this.LblPel2.BackColor = System.Drawing.SystemColors.Control;
			this.LblPel2.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblPel2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblPel2.Location = new System.Drawing.Point(40, 24);
			this.LblPel2.Name = "LblPel2";
			this.LblPel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblPel2.Size = new System.Drawing.Size(41, 17);
			this.LblPel2.TabIndex = 30;
			this.LblPel2.Text = "+ELS";
			// 
			// FraXax
			// 
			this.FraXax.BackColor = System.Drawing.SystemColors.Control;
			this.FraXax.Controls.Add(this.LblXpos);
			this.FraXax.Controls.Add(this.FraXEls);
			this.FraXax.Controls.Add(this.FraXAlm);
			this.FraXax.Controls.Add(this.FraXSvon);
			this.FraXax.Controls.Add(this.LblPel1);
			this.FraXax.Controls.Add(this.LblMel1);
			this.FraXax.Controls.Add(this.LblAlm1);
			this.FraXax.Controls.Add(this.LblXpel);
			this.FraXax.Controls.Add(this.LblXmel);
			this.FraXax.Controls.Add(this.LblXalm);
			this.FraXax.Controls.Add(this.LblXsvon);
			this.FraXax.Controls.Add(this.LblSvon1);
			this.FraXax.Controls.Add(this.Label5);
			this.FraXax.ForeColor = System.Drawing.SystemColors.ControlText;
			this.FraXax.Location = new System.Drawing.Point(160, 8);
			this.FraXax.Name = "FraXax";
			this.FraXax.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.FraXax.Size = new System.Drawing.Size(185, 265);
			this.FraXax.TabIndex = 92;
			this.FraXax.TabStop = false;
			this.FraXax.Text = "X軸";
			// 
			// LblXpos
			// 
			this.LblXpos.BackColor = System.Drawing.SystemColors.Control;
			this.LblXpos.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXpos.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblXpos.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblXpos.Location = new System.Drawing.Point(72, 232);
			this.LblXpos.Name = "LblXpos";
			this.LblXpos.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblXpos.Size = new System.Drawing.Size(105, 17);
			this.LblXpos.TabIndex = 16;
			this.LblXpos.Text = "0";
			this.LblXpos.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// FraXEls
			// 
			this.FraXEls.BackColor = System.Drawing.SystemColors.Control;
			this.FraXEls.Controls.Add(this.OptXels0);
			this.FraXEls.Controls.Add(this.OptXels1);
			this.FraXEls.Cursor = System.Windows.Forms.Cursors.Default;
			this.FraXEls.ForeColor = System.Drawing.SystemColors.ControlText;
			this.FraXEls.Location = new System.Drawing.Point(88, 24);
			this.FraXEls.Name = "FraXEls";
			this.FraXEls.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.FraXEls.Size = new System.Drawing.Size(89, 25);
			this.FraXEls.TabIndex = 7;
			this.FraXEls.Text = "Frame2";
			// 
			// OptXels0
			// 
			this.OptXels0.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXels0.BackColor = System.Drawing.SystemColors.Control;
			this.OptXels0.Cursor = System.Windows.Forms.Cursors.Default;
			this.OptXels0.ForeColor = System.Drawing.SystemColors.ControlText;
			this.OptXels0.Location = new System.Drawing.Point(48, 0);
			this.OptXels0.Name = "OptXels0";
			this.OptXels0.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.OptXels0.Size = new System.Drawing.Size(41, 25);
			this.OptXels0.TabIndex = 0;
			this.OptXels0.TabStop = true;
			this.OptXels0.Text = "B接";
			this.OptXels0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OptXels1
			// 
			this.OptXels1.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXels1.BackColor = System.Drawing.SystemColors.Control;
			this.OptXels1.Cursor = System.Windows.Forms.Cursors.Default;
			this.OptXels1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.OptXels1.Location = new System.Drawing.Point(0, 0);
			this.OptXels1.Name = "OptXels1";
			this.OptXels1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.OptXels1.Size = new System.Drawing.Size(41, 25);
			this.OptXels1.TabIndex = 1;
			this.OptXels1.TabStop = true;
			this.OptXels1.Text = "A接";
			this.OptXels1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FraXAlm
			// 
			this.FraXAlm.BackColor = System.Drawing.SystemColors.Control;
			this.FraXAlm.Controls.Add(this.OptXalm0);
			this.FraXAlm.Controls.Add(this.OptXalm1);
			this.FraXAlm.Cursor = System.Windows.Forms.Cursors.Default;
			this.FraXAlm.ForeColor = System.Drawing.SystemColors.ControlText;
			this.FraXAlm.Location = new System.Drawing.Point(88, 88);
			this.FraXAlm.Name = "FraXAlm";
			this.FraXAlm.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.FraXAlm.Size = new System.Drawing.Size(89, 25);
			this.FraXAlm.TabIndex = 4;
			this.FraXAlm.Text = "Frame2";
			// 
			// OptXalm0
			// 
			this.OptXalm0.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXalm0.BackColor = System.Drawing.SystemColors.Control;
			this.OptXalm0.Cursor = System.Windows.Forms.Cursors.Default;
			this.OptXalm0.ForeColor = System.Drawing.SystemColors.ControlText;
			this.OptXalm0.Location = new System.Drawing.Point(48, 0);
			this.OptXalm0.Name = "OptXalm0";
			this.OptXalm0.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.OptXalm0.Size = new System.Drawing.Size(41, 25);
			this.OptXalm0.TabIndex = 0;
			this.OptXalm0.TabStop = true;
			this.OptXalm0.Text = "B接";
			this.OptXalm0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OptXalm1
			// 
			this.OptXalm1.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXalm1.BackColor = System.Drawing.SystemColors.Control;
			this.OptXalm1.Cursor = System.Windows.Forms.Cursors.Default;
			this.OptXalm1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.OptXalm1.Location = new System.Drawing.Point(0, 0);
			this.OptXalm1.Name = "OptXalm1";
			this.OptXalm1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.OptXalm1.Size = new System.Drawing.Size(41, 25);
			this.OptXalm1.TabIndex = 1;
			this.OptXalm1.TabStop = true;
			this.OptXalm1.Text = "A接";
			this.OptXalm1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FraXSvon
			// 
			this.FraXSvon.BackColor = System.Drawing.SystemColors.Control;
			this.FraXSvon.Controls.Add(this.OptXsvon0);
			this.FraXSvon.Controls.Add(this.OptXsvon1);
			this.FraXSvon.Cursor = System.Windows.Forms.Cursors.Default;
			this.FraXSvon.ForeColor = System.Drawing.SystemColors.ControlText;
			this.FraXSvon.Location = new System.Drawing.Point(88, 184);
			this.FraXSvon.Name = "FraXSvon";
			this.FraXSvon.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.FraXSvon.Size = new System.Drawing.Size(89, 25);
			this.FraXSvon.TabIndex = 1;
			this.FraXSvon.Text = "Frame2";
			// 
			// OptXsvon0
			// 
			this.OptXsvon0.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXsvon0.BackColor = System.Drawing.SystemColors.Control;
			this.OptXsvon0.Cursor = System.Windows.Forms.Cursors.Default;
			this.OptXsvon0.ForeColor = System.Drawing.SystemColors.ControlText;
			this.OptXsvon0.Location = new System.Drawing.Point(48, 0);
			this.OptXsvon0.Name = "OptXsvon0";
			this.OptXsvon0.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.OptXsvon0.Size = new System.Drawing.Size(41, 25);
			this.OptXsvon0.TabIndex = 0;
			this.OptXsvon0.TabStop = true;
			this.OptXsvon0.Text = "OFF";
			this.OptXsvon0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OptXsvon1
			// 
			this.OptXsvon1.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXsvon1.BackColor = System.Drawing.SystemColors.Control;
			this.OptXsvon1.Cursor = System.Windows.Forms.Cursors.Default;
			this.OptXsvon1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.OptXsvon1.Location = new System.Drawing.Point(0, 0);
			this.OptXsvon1.Name = "OptXsvon1";
			this.OptXsvon1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.OptXsvon1.Size = new System.Drawing.Size(41, 25);
			this.OptXsvon1.TabIndex = 1;
			this.OptXsvon1.TabStop = true;
			this.OptXsvon1.Text = "ON";
			this.OptXsvon1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LblPel1
			// 
			this.LblPel1.BackColor = System.Drawing.SystemColors.Control;
			this.LblPel1.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblPel1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblPel1.Location = new System.Drawing.Point(40, 24);
			this.LblPel1.Name = "LblPel1";
			this.LblPel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblPel1.Size = new System.Drawing.Size(41, 17);
			this.LblPel1.TabIndex = 15;
			this.LblPel1.Text = "+ELS";
			// 
			// LblMel1
			// 
			this.LblMel1.BackColor = System.Drawing.SystemColors.Control;
			this.LblMel1.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblMel1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblMel1.Location = new System.Drawing.Point(40, 56);
			this.LblMel1.Name = "LblMel1";
			this.LblMel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblMel1.Size = new System.Drawing.Size(41, 17);
			this.LblMel1.TabIndex = 17;
			this.LblMel1.Text = "-ELS";
			// 
			// LblAlm1
			// 
			this.LblAlm1.BackColor = System.Drawing.SystemColors.Control;
			this.LblAlm1.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblAlm1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblAlm1.Location = new System.Drawing.Point(40, 88);
			this.LblAlm1.Name = "LblAlm1";
			this.LblAlm1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblAlm1.Size = new System.Drawing.Size(49, 17);
			this.LblAlm1.TabIndex = 20;
			this.LblAlm1.Text = "SVALM";
			// 
			// LblXpel
			// 
			this.LblXpel.BackColor = System.Drawing.SystemColors.Window;
			this.LblXpel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXpel.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblXpel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.LblXpel.Location = new System.Drawing.Point(8, 24);
			this.LblXpel.Name = "LblXpel";
			this.LblXpel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblXpel.Size = new System.Drawing.Size(17, 17);
			this.LblXpel.TabIndex = 14;
			// 
			// LblXmel
			// 
			this.LblXmel.BackColor = System.Drawing.SystemColors.Window;
			this.LblXmel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXmel.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblXmel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.LblXmel.Location = new System.Drawing.Point(8, 56);
			this.LblXmel.Name = "LblXmel";
			this.LblXmel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblXmel.Size = new System.Drawing.Size(17, 17);
			this.LblXmel.TabIndex = 16;
			// 
			// LblXalm
			// 
			this.LblXalm.BackColor = System.Drawing.SystemColors.Window;
			this.LblXalm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXalm.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblXalm.ForeColor = System.Drawing.SystemColors.WindowText;
			this.LblXalm.Location = new System.Drawing.Point(8, 88);
			this.LblXalm.Name = "LblXalm";
			this.LblXalm.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblXalm.Size = new System.Drawing.Size(17, 17);
			this.LblXalm.TabIndex = 18;
			// 
			// LblXsvon
			// 
			this.LblXsvon.BackColor = System.Drawing.SystemColors.Window;
			this.LblXsvon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXsvon.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblXsvon.ForeColor = System.Drawing.SystemColors.WindowText;
			this.LblXsvon.Location = new System.Drawing.Point(8, 184);
			this.LblXsvon.Name = "LblXsvon";
			this.LblXsvon.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblXsvon.Size = new System.Drawing.Size(17, 17);
			this.LblXsvon.TabIndex = 21;
			// 
			// LblSvon1
			// 
			this.LblSvon1.BackColor = System.Drawing.SystemColors.Control;
			this.LblSvon1.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblSvon1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblSvon1.Location = new System.Drawing.Point(40, 184);
			this.LblSvon1.Name = "LblSvon1";
			this.LblSvon1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblSvon1.Size = new System.Drawing.Size(41, 17);
			this.LblSvon1.TabIndex = 22;
			this.LblSvon1.Text = "SVON";
			// 
			// Label5
			// 
			this.Label5.AutoSize = true;
			this.Label5.BackColor = System.Drawing.SystemColors.Control;
			this.Label5.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label5.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label5.Location = new System.Drawing.Point(8, 232);
			this.Label5.Name = "Label5";
			this.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label5.Size = new System.Drawing.Size(54, 15);
			this.Label5.TabIndex = 17;
			this.Label5.Text = "現在位置";
			// 
			// Label4
			// 
			this.Label4.AutoSize = true;
			this.Label4.BackColor = System.Drawing.SystemColors.Control;
			this.Label4.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label4.Location = new System.Drawing.Point(24, 232);
			this.Label4.Name = "Label4";
			this.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label4.Size = new System.Drawing.Size(74, 15);
			this.Label4.TabIndex = 106;
			this.Label4.Text = "Y軸中心位置";
			// 
			// Label3
			// 
			this.Label3.AutoSize = true;
			this.Label3.BackColor = System.Drawing.SystemColors.Control;
			this.Label3.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label3.Location = new System.Drawing.Point(24, 192);
			this.Label3.Name = "Label3";
			this.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label3.Size = new System.Drawing.Size(74, 15);
			this.Label3.TabIndex = 104;
			this.Label3.Text = "X軸中心位置";
			// 
			// Label2
			// 
			this.Label2.AutoSize = true;
			this.Label2.BackColor = System.Drawing.SystemColors.Control;
			this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label2.Location = new System.Drawing.Point(24, 152);
			this.Label2.Name = "Label2";
			this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label2.Size = new System.Drawing.Size(74, 15);
			this.Label2.TabIndex = 102;
			this.Label2.Text = "Y軸終点位置";
			// 
			// Label1
			// 
			this.Label1.AutoSize = true;
			this.Label1.BackColor = System.Drawing.SystemColors.Control;
			this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label1.Location = new System.Drawing.Point(24, 112);
			this.Label1.Name = "Label1";
			this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label1.Size = new System.Drawing.Size(74, 15);
			this.Label1.TabIndex = 100;
			this.Label1.Text = "X軸終点位置";
			// 
			// Label7
			// 
			this.Label7.AutoSize = true;
			this.Label7.BackColor = System.Drawing.SystemColors.Control;
			this.Label7.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label7.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label7.Location = new System.Drawing.Point(360, 288);
			this.Label7.Name = "Label7";
			this.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label7.Size = new System.Drawing.Size(54, 15);
			this.Label7.TabIndex = 97;
			this.Label7.Text = "合成速度";
			// 
			// LblYerr
			// 
			this.LblYerr.BackColor = System.Drawing.SystemColors.Control;
			this.LblYerr.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblYerr.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblYerr.Location = new System.Drawing.Point(8, 352);
			this.LblYerr.Name = "LblYerr";
			this.LblYerr.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblYerr.Size = new System.Drawing.Size(185, 25);
			this.LblYerr.TabIndex = 96;
			// 
			// LblXint
			// 
			this.LblXint.BackColor = System.Drawing.SystemColors.Control;
			this.LblXint.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblXint.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblXint.Location = new System.Drawing.Point(8, 288);
			this.LblXint.Name = "LblXint";
			this.LblXint.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblXint.Size = new System.Drawing.Size(185, 25);
			this.LblXint.TabIndex = 95;
			// 
			// LblXerr
			// 
			this.LblXerr.BackColor = System.Drawing.SystemColors.Control;
			this.LblXerr.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblXerr.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblXerr.Location = new System.Drawing.Point(8, 320);
			this.LblXerr.Name = "LblXerr";
			this.LblXerr.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblXerr.Size = new System.Drawing.Size(185, 25);
			this.LblXerr.TabIndex = 94;
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// DlgCir
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(552, 437);
			this.Controls.Add(this.LblXspd);
			this.Controls.Add(this.BtnCcw);
			this.Controls.Add(this.BtnStp);
			this.Controls.Add(this.BtnCw);
			this.Controls.Add(this.TxtYCntr);
			this.Controls.Add(this.TxtXCntr);
			this.Controls.Add(this.TxtYEnd);
			this.Controls.Add(this.TxtXEnd);
			this.Controls.Add(this.FraYax);
			this.Controls.Add(this.FraXax);
			this.Controls.Add(this.Label4);
			this.Controls.Add(this.Label3);
			this.Controls.Add(this.Label2);
			this.Controls.Add(this.Label1);
			this.Controls.Add(this.Label7);
			this.Controls.Add(this.LblYerr);
			this.Controls.Add(this.LblXint);
			this.Controls.Add(this.LblXerr);
			this.Name = "DlgCir";
			this.Text = "ＸＹ軸円弧補間動作";
			this.Load += new System.EventHandler(this.DlgCir_Load);
			this.Closed += new System.EventHandler(this.DlgCir_Closed);
			this.FraYax.ResumeLayout(false);
			this.FraYSvon.ResumeLayout(false);
			this.FraYAlm.ResumeLayout(false);
			this.FraYEls.ResumeLayout(false);
			this.FraXax.ResumeLayout(false);
			this.FraXEls.ResumeLayout(false);
			this.FraXAlm.ResumeLayout(false);
			this.FraXSvon.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		private System.Windows.Forms.RadioButton[] OptXels;
		private System.Windows.Forms.RadioButton[] OptXalm;
		private System.Windows.Forms.RadioButton[] OptXsvon;
		private System.Windows.Forms.RadioButton[] OptYels;
		private System.Windows.Forms.RadioButton[] OptYalm;
		private System.Windows.Forms.RadioButton[] OptYsvon;
		private System.Windows.Forms.Label[] LblPel;
		private System.Windows.Forms.Label[] LblMel;
		private System.Windows.Forms.Label[] LblAlm;
		private System.Windows.Forms.Label[] LblPos;
		private System.Windows.Forms.Label[] LblSvon;

		// フォームが閉じようとする時の処理
		private void DlgCir_Closed(object sender, System.EventArgs e)
		{
			DlgInx f0 = new DlgInx();
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;

			// タイマー停止
			timer1.Enabled = false;

			// 停止コマンド
			unRet = Cp530l1a.hcp530_QuickStop(hDev, 3);

			// デバイスクローズ
			unRet = Cp530l1a.hcp530_DevClose(hDev);

			this.Dispose();
			f0.Show();
		}

		// フォームロード処理
		private void DlgCir_Load(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;

			// ボタン設定
			OptXels0.Checked = true;
			OptXalm0.Checked = true;
			OptXsvon0.Checked = true;
			OptYels0.Checked = true;
			OptYalm0.Checked = true;
			OptYsvon0.Checked = true;

			// 終点位置エディタ設定
			TxtXEnd.Text = "0";
			TxtXEnd.MaxLength = 8;
			TxtYEnd.Text = "0";
			TxtYEnd.MaxLength = 8;

			// 中心位置エディタ設定
			TxtXCntr.Text = "500";
			TxtXCntr.MaxLength = 8;
			TxtYCntr.Text = "0";
			TxtYCntr.MaxLength = 8;

			// 動作速度設定(500pps)
			unRet = Cp530l1a.hcp530_WritFHSpd(hDev, 0, 500);
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
			
			this.OptYels = new System.Windows.Forms.RadioButton[]
				{this.OptYels0, this.OptYels1};

			this.OptYalm = new System.Windows.Forms.RadioButton[]
				{this.OptYalm0, this.OptYalm1};
						
			this.OptYsvon = new System.Windows.Forms.RadioButton[]
				{this.OptYsvon0, this.OptYsvon1};

			//ラベルコントロール配列の作成
			this.LblPel = new System.Windows.Forms.Label[]
				{this.LblXpel, this.LblYpel};

			this.LblMel = new System.Windows.Forms.Label[]
				{this.LblXmel, this.LblYmel};

			this.LblAlm = new System.Windows.Forms.Label[]
				{this.LblXalm, this.LblYalm};

			this.LblPos = new System.Windows.Forms.Label[]
				{this.LblXpos, this.LblYpos};

			this.LblSvon = new System.Windows.Forms.Label[]
				{this.LblXsvon, this.LblYsvon};

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
			for (int i = 0; i < this.OptYels.Length; i++)
				this.OptYels[i].Click +=
					new EventHandler(this.OptYels_Click);
			for (int i = 0; i < this.OptYalm.Length; i++)
				this.OptYalm[i].Click +=
					new EventHandler(this.OptYalm_Click);
			for (int i = 0; i < this.OptYsvon.Length; i++)
				this.OptYsvon[i].Click +=
					new EventHandler(this.OptYsvon_Click);
		}

		//Ｘ軸ELS入力極性
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

		//Ｘ軸SVALM入力極性
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

		//Ｘ軸サーボオン
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

		//Ｙ軸ELS入力極性
		private void OptYels_Click(object sender, EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;
			ushort Index = 0;

			for (int i = 0; i < OptYels.Length; i++)
			{
				if (sender.Equals(OptYels[i])==true)
				{
					Index = (ushort)i;
					break;
				}
			}
			
			unRet = Cp530l1a.hcp530_SetEls(hDev, 1, Index, 0);
			if (0 != unRet)
			{
				s = "エラーが発生しました!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}
		}

		//Ｙ軸SVALM入力極性
		private void OptYalm_Click(object sender, EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;
			ushort Index = 0;

			for (int i = 0; i < OptYalm.Length; i++)
			{
				if (sender.Equals(OptYalm[i])==true)
				{
					Index = (ushort)i;
					break;
				}
			}
			unRet = Cp530l1a.hcp530_SetSvAlm(hDev, 1, Index, 0);
			if (0 != unRet)
			{
				s = "エラーが発生しました!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}
		}

		//Ｙ軸サーボオン
		private void OptYsvon_Click(object sender, EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;
			ushort Index = 0;

			for (int i = 0; i < OptYsvon.Length; i++)
			{
				if (sender.Equals(OptYsvon[i])==true)
				{
					Index = (ushort)i;
					break;
				}
			}
			if (0==Index)
			{
				unRet = Cp530l1a.hcp530_SvOff(hDev, 2);
			}
			else if(1==Index)
			{
				unRet = Cp530l1a.hcp530_SvOn(hDev, 2);
			}
			if (0 != unRet)
			{
				s = "エラーが発生しました!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}		
		}

		//  ステータス等読込と表示
		private void timer1_Tick(object sender, System.EventArgs e)
		{
			ushort msts = 0;
			ushort ssts = 0;
			ushort usAx = 0;
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
						// Y軸ｴﾗｰｽﾃｰﾀｽ読込
						unRet = Cp530l1a.hcp530_ReadErrorSts(hDev, 1, ref unErr);
						if (0 == unRet)
						{
							if((unErr & 0x20) == 0x20)
							{
								LblYerr.Text = "+YELS入力ONによる停止";
							}
							if((unErr & 0x40) == 0x40)
							{
								LblYerr.Text = "-YELS入力ONによる停止";
							}
							if((unErr & 0x80) == 0x80)
							{
								LblYerr.Text = "YSVALM入力ONによる停止";
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
						// X軸ｲﾍﾞﾝﾄｽﾃｰﾀｽ読込
						unRet = Cp530l1a.hcp530_ReadEventSts(hDev, 0, ref unInt);
						if (0 != unRet)
							return;
					}					
					// ボタン使用可
					BtnCw.Enabled = true;
					BtnCcw.Enabled = true;
				}
			}
			else
			{
				return;
			}

			// サブステータスと現在位置の読込
			for(usAx=0;usAx<2;usAx++)
			{
				unRet = Cp530l1a.hcp530_ReadSubSts(hDev, usAx, ref ssts);
				if(0==unRet)
				{
					// SVON 結果表示(SVON ON：緑色表示，SVON OFF：白色表示)
					if(0x1==(ssts & 0x1))
					{
						LblSvon[usAx].BackColor = Color.LimeGreen;
					}
					else
					{
						LblSvon[usAx].BackColor = Color.White;
					}
					// SVALM 結果表示(SVALM ON：赤色表示，SVALM OFF：白色表示)
					if(0x800==(ssts & 0x800))
					{
						LblAlm[usAx].BackColor = Color.Red;
					}
					else
					{
						LblAlm[usAx].BackColor = Color.White;
					}
					// +ELS 結果表示(ELS ON：赤色表示，ELS OFF：白色表示)
					if(0x1000==(ssts & 0x1000))
					{
						LblPel[usAx].BackColor = Color.Red;
					}
					else
					{
						LblPel[usAx].BackColor = Color.White;
					}
					// -ELS 結果表示(ELS ON：赤色表示，ELS OFF：白色表示)
					if(0x2000==(ssts & 0x2000))
					{
						LblMel[usAx].BackColor = Color.Red;
					}
					else
					{
						LblMel[usAx].BackColor = Color.White;
					}
				}
				else
				{
					return;
				}
				unRet = Cp530l1a.hcp530_ReadCtr(hDev, usAx, 1, ref nPos);
				if(0==unRet)
				{
					// 現在指令位置の表示
					LblPos[usAx].Text = nPos.ToString();    
				}
				else
				{
					return;
				}
			}

			// ＸＹ軸現在(合成)速度の読込
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

		// 円弧補間(CW方向)スタート
		private void BtnCw_Click(object sender, System.EventArgs e)
		{
			int lEnd1 = 0;
			int lEnd2 = 0;
			int lCntr1 = 0;
			int lCntr2 = 0;
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;
			
			// 終点位置のテキストをinteger型に変換して、変数に代入します．
			try
			{
				lEnd1 = int.Parse(TxtXEnd.Text);
				lEnd2 = int.Parse(TxtYEnd.Text);
			}
			catch
			{
				// テキストが数字以外だった場合、メッセージを出します．
				MessageBox.Show("終点位置が設定範囲外です．");
				return;
			}
			
			// 中心位置のテキストをinteger型に変換して、変数に代入します．
			try
			{
				lCntr1 = int.Parse(TxtXCntr.Text);
				lCntr2 = int.Parse(TxtYCntr.Text);
			}
			catch
			{
				// テキストが数字以外だった場合、メッセージを出します．
				MessageBox.Show("中心位置が設定範囲外です．");
				return;
			}

			// 円弧補間データ設定
			unRet = Cp530l1a.hcp530_WritCircl(hDev, 0, lEnd1, lEnd2, lCntr1, lCntr2);
			if (0 == unRet)
			{
				// 動作モード設定(円弧補間動作(CW方向))
				unRet = Cp530l1a.hcp530_WritOpeMode(hDev, 0, 0x64);
				if (0 == unRet)
				{
					unRet = Cp530l1a.hcp530_WritOpeMode(hDev, 1, 0x64);
					if (0 == unRet)
					{
						// 動作速度定速スタート(X-Y軸)
						unRet = Cp530l1a.hcp530_CnstStartFH(hDev, 3);
						if (0 == unRet)
						{
							// コントロール表示等
							BtnCw.Enabled = false;
							BtnCcw.Enabled = false;
							LblXerr.Text = "";
							LblYerr.Text = "";
							LblXint.Text = "";
						}
					}
				}
			}
			if (0 != unRet)
			{
				s = "エラーが発生しました!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}
		}

		// 円弧補間(CCW方向)スタート
		private void BtnCcw_Click(object sender, System.EventArgs e)
		{
			int lEnd1 = 0;
			int lEnd2 = 0;
			int lCntr1 = 0;
			int lCntr2 = 0;
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;
			
			// 終点位置のテキストをinteger型に変換して、変数に代入します．
			try
			{
				lEnd1 = int.Parse(TxtXEnd.Text);
				lEnd2 = int.Parse(TxtYEnd.Text);
			}
			catch
			{
				// テキストが数字以外だった場合、メッセージを出します．
				MessageBox.Show("終点位置が設定範囲外です．");
				return;
			}
			
			// 中心位置のテキストをinteger型に変換して、変数に代入します．
			try
			{
				lCntr1 = int.Parse(TxtXCntr.Text);
				lCntr2 = int.Parse(TxtYCntr.Text);
			}
			catch
			{
				// テキストが数字以外だった場合、メッセージを出します．
				MessageBox.Show("中心位置が設定範囲外です．");
				return;
			}

			// 円弧補間データ設定
			unRet = Cp530l1a.hcp530_WritCircl(hDev, 0, lEnd1, lEnd2, lCntr1, lCntr2);
			if (0 == unRet)
			{
				// 動作モード設定(円弧補間動作(CCW方向))
				unRet = Cp530l1a.hcp530_WritOpeMode(hDev, 0, 0x65);
				if (0 == unRet)
				{
					unRet = Cp530l1a.hcp530_WritOpeMode(hDev, 1, 0x65);
					if (0 == unRet)
					{
						// 動作速度定速スタート(X-Y軸)
						unRet = Cp530l1a.hcp530_CnstStartFH(hDev, 3);
						if (0 == unRet)
						{
							// コントロール表示等
							BtnCw.Enabled = false;
							BtnCcw.Enabled = false;
							LblXerr.Text = "";
							LblYerr.Text = "";
							LblXint.Text = "";
						}
					}
				}
			}
			if (0 != unRet)
			{
				s = "エラーが発生しました!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}		
		}

		// Ｘ-Ｙ軸：即停止
		private void BtnStp_Click(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;

			unRet = Cp530l1a.hcp530_QuickStop(hDev, 3);
			if (0 != unRet)
			{
				s = "エラーが発生しました!\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
			}
		}
	}
}
