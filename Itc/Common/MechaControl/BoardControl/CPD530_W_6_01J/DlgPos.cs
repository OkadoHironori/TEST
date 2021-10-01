using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace spc53004
{
	/// <summary>
	/// DlgPos �̊T�v�̐����ł��B
	/// </summary>
	public class DlgPos : System.Windows.Forms.Form
	{
		public System.Windows.Forms.TextBox TxtXdstnc;
		public System.Windows.Forms.Button BtnCtrRst;
		public System.Windows.Forms.Button BtnCPos;
		public System.Windows.Forms.Button BtnAPos;
		public System.Windows.Forms.Button BtnStp;
		public System.Windows.Forms.Panel PnlEls;
		public System.Windows.Forms.RadioButton OptXels0;
		public System.Windows.Forms.RadioButton OptXels1;
		public System.Windows.Forms.Panel PnlAlm;
		public System.Windows.Forms.RadioButton OptXalm0;
		public System.Windows.Forms.RadioButton OptXalm1;
		public System.Windows.Forms.Panel PnlSvon;
		public System.Windows.Forms.RadioButton OptXsvon0;
		public System.Windows.Forms.RadioButton OptXsvon1;
		public System.Windows.Forms.TextBox TxtFH;
		public System.Windows.Forms.Label Label4;
		public System.Windows.Forms.Label Label1;
		public System.Windows.Forms.Label Label2;
		public System.Windows.Forms.Label Label3;
		public System.Windows.Forms.Label LblPel;
		public System.Windows.Forms.Label LblMel;
		public System.Windows.Forms.Label LblAlm;
		public System.Windows.Forms.Label LblXpel;
		public System.Windows.Forms.Label LblXmel;
		public System.Windows.Forms.Label LblXalm;
		public System.Windows.Forms.Label LblXsvon;
		public System.Windows.Forms.Label LblSvon;
		public System.Windows.Forms.Label LblXpos;
		public System.Windows.Forms.Label LblXspd;
		public System.Windows.Forms.Label LblXerr;
		public System.Windows.Forms.Label LblXint;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// �ʒu���߃T���v��
		/// </summary>
		public DlgPos()
		{
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
		}

		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
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

		#region Windows �t�H�[�� �f�U�C�i�Ő������ꂽ�R�[�h 
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.TxtXdstnc = new System.Windows.Forms.TextBox();
			this.BtnCtrRst = new System.Windows.Forms.Button();
			this.BtnCPos = new System.Windows.Forms.Button();
			this.BtnAPos = new System.Windows.Forms.Button();
			this.BtnStp = new System.Windows.Forms.Button();
			this.PnlEls = new System.Windows.Forms.Panel();
			this.OptXels0 = new System.Windows.Forms.RadioButton();
			this.OptXels1 = new System.Windows.Forms.RadioButton();
			this.PnlAlm = new System.Windows.Forms.Panel();
			this.OptXalm0 = new System.Windows.Forms.RadioButton();
			this.OptXalm1 = new System.Windows.Forms.RadioButton();
			this.PnlSvon = new System.Windows.Forms.Panel();
			this.OptXsvon0 = new System.Windows.Forms.RadioButton();
			this.OptXsvon1 = new System.Windows.Forms.RadioButton();
			this.TxtFH = new System.Windows.Forms.TextBox();
			this.Label4 = new System.Windows.Forms.Label();
			this.Label1 = new System.Windows.Forms.Label();
			this.Label2 = new System.Windows.Forms.Label();
			this.Label3 = new System.Windows.Forms.Label();
			this.LblPel = new System.Windows.Forms.Label();
			this.LblMel = new System.Windows.Forms.Label();
			this.LblAlm = new System.Windows.Forms.Label();
			this.LblXpel = new System.Windows.Forms.Label();
			this.LblXmel = new System.Windows.Forms.Label();
			this.LblXalm = new System.Windows.Forms.Label();
			this.LblXsvon = new System.Windows.Forms.Label();
			this.LblSvon = new System.Windows.Forms.Label();
			this.LblXpos = new System.Windows.Forms.Label();
			this.LblXspd = new System.Windows.Forms.Label();
			this.LblXerr = new System.Windows.Forms.Label();
			this.LblXint = new System.Windows.Forms.Label();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.PnlEls.SuspendLayout();
			this.PnlAlm.SuspendLayout();
			this.PnlSvon.SuspendLayout();
			this.SuspendLayout();
			// 
			// TxtXdstnc
			// 
			this.TxtXdstnc.AcceptsReturn = true;
			this.TxtXdstnc.AutoSize = false;
			this.TxtXdstnc.BackColor = System.Drawing.SystemColors.Window;
			this.TxtXdstnc.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.TxtXdstnc.ForeColor = System.Drawing.SystemColors.WindowText;
			this.TxtXdstnc.Location = new System.Drawing.Point(16, 184);
			this.TxtXdstnc.MaxLength = 10;
			this.TxtXdstnc.Multiline = true;
			this.TxtXdstnc.Name = "TxtXdstnc";
			this.TxtXdstnc.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.TxtXdstnc.Size = new System.Drawing.Size(161, 22);
			this.TxtXdstnc.TabIndex = 104;
			this.TxtXdstnc.Text = "";
			this.TxtXdstnc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// BtnCtrRst
			// 
			this.BtnCtrRst.BackColor = System.Drawing.SystemColors.Control;
			this.BtnCtrRst.Cursor = System.Windows.Forms.Cursors.Default;
			this.BtnCtrRst.ForeColor = System.Drawing.SystemColors.ControlText;
			this.BtnCtrRst.Location = new System.Drawing.Point(16, 80);
			this.BtnCtrRst.Name = "BtnCtrRst";
			this.BtnCtrRst.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.BtnCtrRst.Size = new System.Drawing.Size(161, 25);
			this.BtnCtrRst.TabIndex = 83;
			this.BtnCtrRst.Text = "�J�E���^���Z�b�g";
			this.BtnCtrRst.Click += new System.EventHandler(this.BtnCtrRst_Click);
			// 
			// BtnCPos
			// 
			this.BtnCPos.BackColor = System.Drawing.SystemColors.Control;
			this.BtnCPos.Cursor = System.Windows.Forms.Cursors.Default;
			this.BtnCPos.ForeColor = System.Drawing.SystemColors.ControlText;
			this.BtnCPos.Location = new System.Drawing.Point(16, 48);
			this.BtnCPos.Name = "BtnCPos";
			this.BtnCPos.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.BtnCPos.Size = new System.Drawing.Size(161, 25);
			this.BtnCPos.TabIndex = 82;
			this.BtnCPos.Text = "�葬�ʒu����";
			this.BtnCPos.Click += new System.EventHandler(this.BtnCPos_Click);
			// 
			// BtnAPos
			// 
			this.BtnAPos.BackColor = System.Drawing.SystemColors.Control;
			this.BtnAPos.Cursor = System.Windows.Forms.Cursors.Default;
			this.BtnAPos.ForeColor = System.Drawing.SystemColors.ControlText;
			this.BtnAPos.Location = new System.Drawing.Point(16, 16);
			this.BtnAPos.Name = "BtnAPos";
			this.BtnAPos.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.BtnAPos.Size = new System.Drawing.Size(161, 25);
			this.BtnAPos.TabIndex = 81;
			this.BtnAPos.Text = "�����ʒu����";
			this.BtnAPos.Click += new System.EventHandler(this.BtnAPos_Click);
			// 
			// BtnStp
			// 
			this.BtnStp.BackColor = System.Drawing.SystemColors.Control;
			this.BtnStp.Cursor = System.Windows.Forms.Cursors.Default;
			this.BtnStp.ForeColor = System.Drawing.SystemColors.ControlText;
			this.BtnStp.Location = new System.Drawing.Point(16, 120);
			this.BtnStp.Name = "BtnStp";
			this.BtnStp.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.BtnStp.Size = new System.Drawing.Size(161, 25);
			this.BtnStp.TabIndex = 84;
			this.BtnStp.Text = "��~";
			this.BtnStp.Click += new System.EventHandler(this.BtnStp_Click);
			// 
			// PnlEls
			// 
			this.PnlEls.BackColor = System.Drawing.SystemColors.Control;
			this.PnlEls.Controls.Add(this.OptXels0);
			this.PnlEls.Controls.Add(this.OptXels1);
			this.PnlEls.Cursor = System.Windows.Forms.Cursors.Default;
			this.PnlEls.ForeColor = System.Drawing.SystemColors.ControlText;
			this.PnlEls.Location = new System.Drawing.Point(288, 16);
			this.PnlEls.Name = "PnlEls";
			this.PnlEls.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.PnlEls.Size = new System.Drawing.Size(105, 25);
			this.PnlEls.TabIndex = 87;
			this.PnlEls.Text = "Frame2";
			// 
			// OptXels0
			// 
			this.OptXels0.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXels0.BackColor = System.Drawing.SystemColors.Control;
			this.OptXels0.Cursor = System.Windows.Forms.Cursors.Default;
			this.OptXels0.ForeColor = System.Drawing.SystemColors.ControlText;
			this.OptXels0.Location = new System.Drawing.Point(56, 0);
			this.OptXels0.Name = "OptXels0";
			this.OptXels0.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.OptXels0.Size = new System.Drawing.Size(41, 25);
			this.OptXels0.TabIndex = 0;
			this.OptXels0.TabStop = true;
			this.OptXels0.Text = "B��";
			this.OptXels0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OptXels1
			// 
			this.OptXels1.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXels1.BackColor = System.Drawing.SystemColors.Control;
			this.OptXels1.Cursor = System.Windows.Forms.Cursors.Default;
			this.OptXels1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.OptXels1.Location = new System.Drawing.Point(8, 0);
			this.OptXels1.Name = "OptXels1";
			this.OptXels1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.OptXels1.Size = new System.Drawing.Size(41, 25);
			this.OptXels1.TabIndex = 1;
			this.OptXels1.TabStop = true;
			this.OptXels1.Text = "A��";
			this.OptXels1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// PnlAlm
			// 
			this.PnlAlm.BackColor = System.Drawing.SystemColors.Control;
			this.PnlAlm.Controls.Add(this.OptXalm0);
			this.PnlAlm.Controls.Add(this.OptXalm1);
			this.PnlAlm.Cursor = System.Windows.Forms.Cursors.Default;
			this.PnlAlm.ForeColor = System.Drawing.SystemColors.ControlText;
			this.PnlAlm.Location = new System.Drawing.Point(288, 80);
			this.PnlAlm.Name = "PnlAlm";
			this.PnlAlm.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.PnlAlm.Size = new System.Drawing.Size(105, 25);
			this.PnlAlm.TabIndex = 86;
			this.PnlAlm.Text = "Frame2";
			// 
			// OptXalm0
			// 
			this.OptXalm0.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXalm0.BackColor = System.Drawing.SystemColors.Control;
			this.OptXalm0.Cursor = System.Windows.Forms.Cursors.Default;
			this.OptXalm0.ForeColor = System.Drawing.SystemColors.ControlText;
			this.OptXalm0.Location = new System.Drawing.Point(56, 0);
			this.OptXalm0.Name = "OptXalm0";
			this.OptXalm0.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.OptXalm0.Size = new System.Drawing.Size(41, 25);
			this.OptXalm0.TabIndex = 0;
			this.OptXalm0.TabStop = true;
			this.OptXalm0.Text = "B��";
			this.OptXalm0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OptXalm1
			// 
			this.OptXalm1.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXalm1.BackColor = System.Drawing.SystemColors.Control;
			this.OptXalm1.Cursor = System.Windows.Forms.Cursors.Default;
			this.OptXalm1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.OptXalm1.Location = new System.Drawing.Point(8, 0);
			this.OptXalm1.Name = "OptXalm1";
			this.OptXalm1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.OptXalm1.Size = new System.Drawing.Size(41, 25);
			this.OptXalm1.TabIndex = 1;
			this.OptXalm1.TabStop = true;
			this.OptXalm1.Text = "A��";
			this.OptXalm1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// PnlSvon
			// 
			this.PnlSvon.BackColor = System.Drawing.SystemColors.Control;
			this.PnlSvon.Controls.Add(this.OptXsvon0);
			this.PnlSvon.Controls.Add(this.OptXsvon1);
			this.PnlSvon.Cursor = System.Windows.Forms.Cursors.Default;
			this.PnlSvon.ForeColor = System.Drawing.SystemColors.ControlText;
			this.PnlSvon.Location = new System.Drawing.Point(288, 176);
			this.PnlSvon.Name = "PnlSvon";
			this.PnlSvon.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.PnlSvon.Size = new System.Drawing.Size(105, 25);
			this.PnlSvon.TabIndex = 85;
			this.PnlSvon.Text = "Frame2";
			// 
			// OptXsvon0
			// 
			this.OptXsvon0.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXsvon0.BackColor = System.Drawing.SystemColors.Control;
			this.OptXsvon0.Cursor = System.Windows.Forms.Cursors.Default;
			this.OptXsvon0.ForeColor = System.Drawing.SystemColors.ControlText;
			this.OptXsvon0.Location = new System.Drawing.Point(56, 0);
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
			this.OptXsvon1.Location = new System.Drawing.Point(8, 0);
			this.OptXsvon1.Name = "OptXsvon1";
			this.OptXsvon1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.OptXsvon1.Size = new System.Drawing.Size(41, 25);
			this.OptXsvon1.TabIndex = 1;
			this.OptXsvon1.TabStop = true;
			this.OptXsvon1.Text = "ON";
			this.OptXsvon1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TxtFH
			// 
			this.TxtFH.AcceptsReturn = true;
			this.TxtFH.AutoSize = false;
			this.TxtFH.BackColor = System.Drawing.SystemColors.Window;
			this.TxtFH.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.TxtFH.ForeColor = System.Drawing.SystemColors.WindowText;
			this.TxtFH.Location = new System.Drawing.Point(16, 240);
			this.TxtFH.MaxLength = 5;
			this.TxtFH.Multiline = true;
			this.TxtFH.Name = "TxtFH";
			this.TxtFH.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.TxtFH.Size = new System.Drawing.Size(161, 22);
			this.TxtFH.TabIndex = 103;
			this.TxtFH.Text = "";
			this.TxtFH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// Label4
			// 
			this.Label4.AutoSize = true;
			this.Label4.BackColor = System.Drawing.SystemColors.Control;
			this.Label4.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label4.Location = new System.Drawing.Point(48, 160);
			this.Label4.Name = "Label4";
			this.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label4.Size = new System.Drawing.Size(76, 15);
			this.Label4.TabIndex = 105;
			this.Label4.Text = "�ړ���(pulse)";
			// 
			// Label1
			// 
			this.Label1.AutoSize = true;
			this.Label1.BackColor = System.Drawing.SystemColors.Control;
			this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label1.Location = new System.Drawing.Point(48, 216);
			this.Label1.Name = "Label1";
			this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label1.Size = new System.Drawing.Size(84, 15);
			this.Label1.TabIndex = 94;
			this.Label1.Text = "���쑬�x(PPS)";
			// 
			// Label2
			// 
			this.Label2.AutoSize = true;
			this.Label2.BackColor = System.Drawing.SystemColors.Control;
			this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label2.Location = new System.Drawing.Point(200, 224);
			this.Label2.Name = "Label2";
			this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label2.Size = new System.Drawing.Size(54, 15);
			this.Label2.TabIndex = 93;
			this.Label2.Text = "���݈ʒu";
			// 
			// Label3
			// 
			this.Label3.AutoSize = true;
			this.Label3.BackColor = System.Drawing.SystemColors.Control;
			this.Label3.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label3.Location = new System.Drawing.Point(200, 248);
			this.Label3.Name = "Label3";
			this.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label3.Size = new System.Drawing.Size(54, 15);
			this.Label3.TabIndex = 92;
			this.Label3.Text = "���ݑ��x";
			// 
			// LblPel
			// 
			this.LblPel.BackColor = System.Drawing.SystemColors.Control;
			this.LblPel.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblPel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblPel.Location = new System.Drawing.Point(240, 16);
			this.LblPel.Name = "LblPel";
			this.LblPel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblPel.Size = new System.Drawing.Size(41, 17);
			this.LblPel.TabIndex = 102;
			this.LblPel.Text = "+ELS";
			// 
			// LblMel
			// 
			this.LblMel.BackColor = System.Drawing.SystemColors.Control;
			this.LblMel.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblMel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblMel.Location = new System.Drawing.Point(240, 48);
			this.LblMel.Name = "LblMel";
			this.LblMel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblMel.Size = new System.Drawing.Size(41, 17);
			this.LblMel.TabIndex = 101;
			this.LblMel.Text = "-ELS";
			// 
			// LblAlm
			// 
			this.LblAlm.BackColor = System.Drawing.SystemColors.Control;
			this.LblAlm.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblAlm.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblAlm.Location = new System.Drawing.Point(240, 80);
			this.LblAlm.Name = "LblAlm";
			this.LblAlm.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblAlm.Size = new System.Drawing.Size(49, 17);
			this.LblAlm.TabIndex = 100;
			this.LblAlm.Text = "SVALM";
			// 
			// LblXpel
			// 
			this.LblXpel.BackColor = System.Drawing.SystemColors.Window;
			this.LblXpel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXpel.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblXpel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.LblXpel.Location = new System.Drawing.Point(208, 16);
			this.LblXpel.Name = "LblXpel";
			this.LblXpel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblXpel.Size = new System.Drawing.Size(17, 17);
			this.LblXpel.TabIndex = 99;
			// 
			// LblXmel
			// 
			this.LblXmel.BackColor = System.Drawing.SystemColors.Window;
			this.LblXmel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXmel.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblXmel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.LblXmel.Location = new System.Drawing.Point(208, 48);
			this.LblXmel.Name = "LblXmel";
			this.LblXmel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblXmel.Size = new System.Drawing.Size(17, 17);
			this.LblXmel.TabIndex = 98;
			// 
			// LblXalm
			// 
			this.LblXalm.BackColor = System.Drawing.SystemColors.Window;
			this.LblXalm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXalm.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblXalm.ForeColor = System.Drawing.SystemColors.WindowText;
			this.LblXalm.Location = new System.Drawing.Point(208, 80);
			this.LblXalm.Name = "LblXalm";
			this.LblXalm.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblXalm.Size = new System.Drawing.Size(17, 17);
			this.LblXalm.TabIndex = 97;
			// 
			// LblXsvon
			// 
			this.LblXsvon.BackColor = System.Drawing.SystemColors.Window;
			this.LblXsvon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXsvon.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblXsvon.ForeColor = System.Drawing.SystemColors.WindowText;
			this.LblXsvon.Location = new System.Drawing.Point(208, 176);
			this.LblXsvon.Name = "LblXsvon";
			this.LblXsvon.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblXsvon.Size = new System.Drawing.Size(17, 17);
			this.LblXsvon.TabIndex = 96;
			// 
			// LblSvon
			// 
			this.LblSvon.BackColor = System.Drawing.SystemColors.Control;
			this.LblSvon.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblSvon.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblSvon.Location = new System.Drawing.Point(232, 176);
			this.LblSvon.Name = "LblSvon";
			this.LblSvon.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblSvon.Size = new System.Drawing.Size(41, 17);
			this.LblSvon.TabIndex = 95;
			this.LblSvon.Text = "SVON";
			// 
			// LblXpos
			// 
			this.LblXpos.BackColor = System.Drawing.SystemColors.Control;
			this.LblXpos.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXpos.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblXpos.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblXpos.Location = new System.Drawing.Point(272, 224);
			this.LblXpos.Name = "LblXpos";
			this.LblXpos.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblXpos.Size = new System.Drawing.Size(113, 17);
			this.LblXpos.TabIndex = 91;
			this.LblXpos.Text = "0";
			this.LblXpos.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// LblXspd
			// 
			this.LblXspd.BackColor = System.Drawing.SystemColors.Control;
			this.LblXspd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXspd.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblXspd.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblXspd.Location = new System.Drawing.Point(272, 248);
			this.LblXspd.Name = "LblXspd";
			this.LblXspd.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblXspd.Size = new System.Drawing.Size(113, 17);
			this.LblXspd.TabIndex = 90;
			this.LblXspd.Text = "0";
			this.LblXspd.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// LblXerr
			// 
			this.LblXerr.BackColor = System.Drawing.SystemColors.Control;
			this.LblXerr.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblXerr.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblXerr.Location = new System.Drawing.Point(16, 320);
			this.LblXerr.Name = "LblXerr";
			this.LblXerr.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblXerr.Size = new System.Drawing.Size(185, 25);
			this.LblXerr.TabIndex = 89;
			// 
			// LblXint
			// 
			this.LblXint.BackColor = System.Drawing.SystemColors.Control;
			this.LblXint.Cursor = System.Windows.Forms.Cursors.Default;
			this.LblXint.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LblXint.Location = new System.Drawing.Point(16, 288);
			this.LblXint.Name = "LblXint";
			this.LblXint.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblXint.Size = new System.Drawing.Size(185, 25);
			this.LblXint.TabIndex = 88;
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// DlgPos
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(552, 437);
			this.Controls.Add(this.TxtXdstnc);
			this.Controls.Add(this.BtnCtrRst);
			this.Controls.Add(this.BtnCPos);
			this.Controls.Add(this.BtnAPos);
			this.Controls.Add(this.BtnStp);
			this.Controls.Add(this.PnlEls);
			this.Controls.Add(this.PnlAlm);
			this.Controls.Add(this.PnlSvon);
			this.Controls.Add(this.TxtFH);
			this.Controls.Add(this.Label4);
			this.Controls.Add(this.Label1);
			this.Controls.Add(this.Label2);
			this.Controls.Add(this.Label3);
			this.Controls.Add(this.LblPel);
			this.Controls.Add(this.LblMel);
			this.Controls.Add(this.LblAlm);
			this.Controls.Add(this.LblXpel);
			this.Controls.Add(this.LblXmel);
			this.Controls.Add(this.LblXalm);
			this.Controls.Add(this.LblXsvon);
			this.Controls.Add(this.LblSvon);
			this.Controls.Add(this.LblXpos);
			this.Controls.Add(this.LblXspd);
			this.Controls.Add(this.LblXerr);
			this.Controls.Add(this.LblXint);
			this.Name = "DlgPos";
			this.Text = "�w���ʒu���ߓ���";
			this.Load += new System.EventHandler(this.DlgPos_Load);
			this.Closed += new System.EventHandler(this.DlgPos_Closed);
			this.PnlEls.ResumeLayout(false);
			this.PnlAlm.ResumeLayout(false);
			this.PnlSvon.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		
		// ���W�I�{�^���̔z�������
		private System.Windows.Forms.RadioButton[] OptXels;
		private System.Windows.Forms.RadioButton[] OptXalm;
		private System.Windows.Forms.RadioButton[] OptXsvon;

		// ���씻�f�t���O(0:�����C1:�葬)
		private ushort gStart = 0;
		
		// �t�H�[�������悤�Ƃ��鎞�̏���
		private void DlgPos_Closed(object sender, System.EventArgs e)
		{
			DlgInx f0 = new DlgInx();
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;

			// �^�C�}�[��~
			timer1.Enabled = false;

			// ��~�R�}���h
			unRet = Cp530l1a.hcp530_QuickStop(hDev, 1);

			// �f�o�C�X�N���[�Y
			unRet = Cp530l1a.hcp530_DevClose(hDev);

			this.Dispose();
			f0.Show();
		}

		// �t�H�[�����[�h����
		private void DlgPos_Load(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;

			// �{�^���ݒ�
			OptXels0.Checked = true;
			OptXalm0.Checked = true;
			OptXsvon0.Checked = true;

			// �ړ��ʃG�f�B�^�ݒ�
			TxtXdstnc.Text = "10000";
			TxtXdstnc.MaxLength = 8;

			// ���쑬�x�G�f�B�^�ݒ�
			TxtFH.Text = "4000";
			TxtFH.MaxLength = 5;

			// �x�[�X���x�ݒ�(400pps)
			unRet = Cp530l1a.hcp530_SetFLSpd(hDev, 0, 400);
			if (0 != unRet)
			{
				s = "�G���[���������܂���!\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
				return;
			}

			// �X�e�[�^�X�\���p�^�C�}�[�J�n
			timer1.Enabled = true;
			
			//�{�^���R���g���[���z��̍쐬
			this.OptXels = new System.Windows.Forms.RadioButton[]
				{this.OptXels0, this.OptXels1};

			this.OptXalm = new System.Windows.Forms.RadioButton[]
				{this.OptXalm0, this.OptXalm1};
						
			this.OptXsvon = new System.Windows.Forms.RadioButton[]
				{this.OptXsvon0, this.OptXsvon1};

			//�C�x���g�n���h���Ɋ֘A�t��
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

		//�w��ELS���͋ɐ�
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
				s = "�G���[���������܂���!\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
			}
		}

		//�w��SVALM���͋ɐ�
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
				s = "�G���[���������܂���!\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
			}
		}

		//�w���T�[�{�I��
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
				s = "�G���[���������܂���!\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
			}		
		}

		//  �X�e�[�^�X���Ǎ��ƕ\��
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

			// �w�����C���X�e�[�^�X�̓Ǎ�
			unRet = Cp530l1a.hcp530_ReadMainSts(hDev, 0, ref msts);
			if (0==unRet)
			{
				// �X�^�[�g�R�}���h�I��
				if ((msts & 0x30) != 0)
				{
					// �G���[��
					if ((msts & 0x10) == 0x10)
					{
						LblXint.Text = "�G���[�I��";
						// X���װ�ð���Ǎ�
						unRet = Cp530l1a.hcp530_ReadErrorSts(hDev, 0, ref unErr);
						if (0 == unRet)
						{
							if((unErr & 0x20) == 0x20)
							{
								LblXerr.Text = "+XELS����ON�ɂ���~";
							}
							if((unErr & 0x40) == 0x40)
							{
								LblXerr.Text = "-XELS����ON�ɂ���~";
							}
							if((unErr & 0x80) == 0x80)
							{
								LblXerr.Text = "XSVALM����ON�ɂ���~";
							}
						}
						else
						{
							return;
						}
					}
					// �C�x���g��(�����~)
					if((msts & 0x20) == 0x20)
					{
						// X������Ľð���Ǎ�
						unRet = Cp530l1a.hcp530_ReadEventSts(hDev, 0, ref unInt);
						if (0 == unRet)
						{
							if((unInt & 0x01)==0x01)
							{
								LblXint.Text = "�����~";
							}
						}
						else
						{
							return;
						}
					}
					// �{�^���g�p��
					BtnAPos.Enabled = true;
					BtnCPos.Enabled = true;
					BtnCtrRst.Enabled = true;
				}
			}
			else
			{
				return;
			}

			// �w���T�u�X�e�[�^�X�̓Ǎ�
			unRet = Cp530l1a.hcp530_ReadSubSts(hDev, 0, ref ssts);
			if(0==unRet)
			{
				// SVON ���ʕ\��(SVON ON�F�ΐF�\���CSVON OFF�F���F�\��)
				if(0x1==(ssts & 0x1))
				{
					LblXsvon.BackColor = Color.LimeGreen;
				}
				else
				{
					LblXsvon.BackColor = Color.White;
				}
				// SVALM ���ʕ\��(SVALM ON�F�ԐF�\���CSVALM OFF�F���F�\��)
				if(0x800==(ssts & 0x800))
				{
					LblXalm.BackColor = Color.Red;
				}
				else
				{
					LblXalm.BackColor = Color.White;
				}
				// +ELS ���ʕ\��(ELS ON�F�ԐF�\���CELS OFF�F���F�\��)
				if(0x1000==(ssts & 0x1000))
				{
					LblXpel.BackColor = Color.Red;
				}
				else
				{
					LblXpel.BackColor = Color.White;
				}
				// -ELS ���ʕ\��(ELS ON�F�ԐF�\���CELS OFF�F���F�\��)
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

			// �w�����ݎw�߈ʒu�̓Ǎ�
			unRet = Cp530l1a.hcp530_ReadCtr(hDev, 0, 1, ref nPos);
			if (0==unRet)
			{
				// ���ݎw�߈ʒu�̕\��
				LblXpos.Text = nPos.ToString();   
			}
			else
			{
				return;
			}
			
			// �w�����ݑ��x�̓Ǎ�
			unRet = Cp530l1a.hcp530_ReadSpd(hDev, 0, ref usSpd);
			if (0==unRet)
			{
				//' ���ݑ��x�̕\��
				LblXspd.Text = usSpd.ToString();
			}
			else
			{
				return;
			}
		}

		// �w���F�������ʒu���߃X�^�[�g
		private void BtnAPos_Click(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			uint fh = 1;
			int dst = 0;
			string s;

			// ���쑬�x�e�L�X�g��integer�^�ɕϊ����āA�ϐ��ɑ�����܂��D
			try
			{
				fh = uint.Parse(TxtFH.Text);
			}
			catch
			{
				// �e�L�X�g�������ȊO�������ꍇ�A���b�Z�[�W���o���܂��D
				MessageBox.Show("���쑬�x���ݒ�͈͊O�ł��D");
				return;
			}

			// �ړ��ʃe�L�X�g��integer�^�ɕϊ����āA�ϐ��ɑ�����܂��D
			try
			{
				dst = int.Parse(TxtXdstnc.Text);
			}
			catch
			{
				// �e�L�X�g�������ȊO�������ꍇ�A���b�Z�[�W���o���܂��D
				MessageBox.Show("�ړ��ʂ��ݒ�͈͊O�ł��D");
				return;
			}

			// ���쑬�x�ݒ�
			unRet = Cp530l1a.hcp530_WritFHSpd(hDev, 0, fh);    
			if (0 != unRet)
			{
				s = "���쑬�x�ݒ�͈͊O�ł�!\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
				return;
			}		

			// �ړ��ʐݒ�
			unRet = Cp530l1a.hcp530_WritPos(hDev, 0, dst);    
			if (0 == unRet)
			{
				// ���샂�[�h�ݒ�(�ʒu���ߓ���)
				unRet = Cp530l1a.hcp530_WritOpeMode(hDev, 0, 0x41);   
				if (0 == unRet)
				{
					// �����X�^�[�g
					unRet = Cp530l1a.hcp530_AccStart(hDev, 1);    
					if (0 == unRet)
					{
						// �R���g���[���\����
						BtnAPos.Enabled = false;
						BtnCPos.Enabled = false;
						BtnCtrRst.Enabled = false;
						LblXerr.Text = "";
						LblXint.Text = "";
					
						// ����������
						gStart = 0; 
					}
				}
			}		
			if (0 != unRet)
			{
				s = "�G���[���������܂���!\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
			}						
		}

		// �w���F�葬�ʒu���߃X�^�[�g
		private void BtnCPos_Click(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			uint fh = 1;
			int dst = 0;
			string s;

			// ���쑬�x�e�L�X�g��integer�^�ɕϊ����āA�ϐ��ɑ�����܂��D
			try
			{
				fh = uint.Parse(TxtFH.Text);
			}
			catch
			{
				// �e�L�X�g�������ȊO�������ꍇ�A���b�Z�[�W���o���܂��D
				MessageBox.Show("���쑬�x���ݒ�͈͊O�ł��D");
				return;
			}

			// �ړ��ʃe�L�X�g��integer�^�ɕϊ����āA�ϐ��ɑ�����܂��D
			try
			{
				dst = int.Parse(TxtXdstnc.Text);
			}
			catch
			{
				// �e�L�X�g�������ȊO�������ꍇ�A���b�Z�[�W���o���܂��D
				MessageBox.Show("�ړ��ʂ��ݒ�͈͊O�ł��D");
				return;
			}

			// ���쑬�x�ݒ�
			unRet = Cp530l1a.hcp530_WritFHSpd(hDev, 0, fh);    
			if (0 != unRet)
			{
				s = "���쑬�x�ݒ�͈͊O�ł�!\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
				return;
			}		

			// �ړ��ʐݒ�
			unRet = Cp530l1a.hcp530_WritPos(hDev, 0, dst);    
			if (0 == unRet)
			{
				// ���샂�[�h�ݒ�(�ʒu���ߓ���)
				unRet = Cp530l1a.hcp530_WritOpeMode(hDev, 0, 0x41);   
				if (0 == unRet)
				{
					// FH�葬�X�^�[�g
					unRet = Cp530l1a.hcp530_CnstStartFH(hDev, 1);    
					if (0 == unRet)
					{
						// �R���g���[���\����
						BtnAPos.Enabled = false;
						BtnCPos.Enabled = false;
						BtnCtrRst.Enabled = false;
						LblXerr.Text = "";
						LblXint.Text = "";
					
						// �葬����
						gStart = 1; 
					}
				}
			}		
			if (0 != unRet)
			{
				s = "�G���[���������܂���!\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
			}				
		}

		// �J�E���^���Z�b�g
		private void BtnCtrRst_Click(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;

			// �w�߈ʒu���Z�b�g
			unRet = Cp530l1a.hcp530_WritCtr(hDev, 0, 0, 1);  
			if (0 != unRet)
			{
				s = "�G���[���������܂���!\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
			}				
		}	

		// �w���F��~
		private void BtnStp_Click(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;

			if (0 == gStart)
			{
				// ����������Ȃ�Ό�����~
				unRet = Cp530l1a.hcp530_DecStop(hDev, 1);
			}
			else
			{
				// �葬����Ȃ�Α���~
				unRet = Cp530l1a.hcp530_QuickStop(hDev, 1);
			}
			if (0 != unRet)
			{
				s = "�G���[���������܂���!\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
			}		
		}
	}
}
