using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace spc53004
{
	/// <summary>
	/// DlgOrg �̊T�v�̐����ł��B
	/// </summary>
	public class DlgOrg : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.Panel PnlSvon;
		internal System.Windows.Forms.RadioButton OptXsvon1;
		internal System.Windows.Forms.RadioButton OptXsvon0;
		internal System.Windows.Forms.Panel PnlOls;
		internal System.Windows.Forms.RadioButton OptXols1;
		internal System.Windows.Forms.RadioButton OptXols0;
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
		internal System.Windows.Forms.Label LblOls;
		internal System.Windows.Forms.Label LblAlm;
		internal System.Windows.Forms.Label LblMel;
		internal System.Windows.Forms.Label LblPel;
		internal System.Windows.Forms.Label LblXsvon;
		internal System.Windows.Forms.Label LblXols;
		internal System.Windows.Forms.Label LblXalm;
		internal System.Windows.Forms.Label LblXmel;
		internal System.Windows.Forms.Label LblXpel;
		internal System.Windows.Forms.Button BtnOrg2;
		internal System.Windows.Forms.Button BtnOrg1;
		internal System.Windows.Forms.Button BtnOrg0;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// ���_���A�T���v��
		/// </summary>
		public DlgOrg()
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
			this.PnlSvon = new System.Windows.Forms.Panel();
			this.OptXsvon1 = new System.Windows.Forms.RadioButton();
			this.OptXsvon0 = new System.Windows.Forms.RadioButton();
			this.PnlOls = new System.Windows.Forms.Panel();
			this.OptXols1 = new System.Windows.Forms.RadioButton();
			this.OptXols0 = new System.Windows.Forms.RadioButton();
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
			this.LblOls = new System.Windows.Forms.Label();
			this.LblAlm = new System.Windows.Forms.Label();
			this.LblMel = new System.Windows.Forms.Label();
			this.LblPel = new System.Windows.Forms.Label();
			this.LblXsvon = new System.Windows.Forms.Label();
			this.LblXols = new System.Windows.Forms.Label();
			this.LblXalm = new System.Windows.Forms.Label();
			this.LblXmel = new System.Windows.Forms.Label();
			this.LblXpel = new System.Windows.Forms.Label();
			this.BtnOrg2 = new System.Windows.Forms.Button();
			this.BtnOrg1 = new System.Windows.Forms.Button();
			this.BtnOrg0 = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.PnlSvon.SuspendLayout();
			this.PnlOls.SuspendLayout();
			this.PnlAlm.SuspendLayout();
			this.PnlEls.SuspendLayout();
			this.SuspendLayout();
			// 
			// PnlSvon
			// 
			this.PnlSvon.Controls.Add(this.OptXsvon1);
			this.PnlSvon.Controls.Add(this.OptXsvon0);
			this.PnlSvon.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.PnlSvon.Location = new System.Drawing.Point(288, 176);
			this.PnlSvon.Name = "PnlSvon";
			this.PnlSvon.Size = new System.Drawing.Size(105, 25);
			this.PnlSvon.TabIndex = 98;
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
			// PnlOls
			// 
			this.PnlOls.Controls.Add(this.OptXols1);
			this.PnlOls.Controls.Add(this.OptXols0);
			this.PnlOls.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.PnlOls.Location = new System.Drawing.Point(288, 112);
			this.PnlOls.Name = "PnlOls";
			this.PnlOls.Size = new System.Drawing.Size(105, 25);
			this.PnlOls.TabIndex = 97;
			// 
			// OptXols1
			// 
			this.OptXols1.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXols1.Location = new System.Drawing.Point(8, 0);
			this.OptXols1.Name = "OptXols1";
			this.OptXols1.Size = new System.Drawing.Size(41, 25);
			this.OptXols1.TabIndex = 1;
			this.OptXols1.Text = "A��";
			this.OptXols1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OptXols0
			// 
			this.OptXols0.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXols0.Location = new System.Drawing.Point(56, 0);
			this.OptXols0.Name = "OptXols0";
			this.OptXols0.Size = new System.Drawing.Size(41, 25);
			this.OptXols0.TabIndex = 0;
			this.OptXols0.Text = "B��";
			this.OptXols0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// PnlAlm
			// 
			this.PnlAlm.Controls.Add(this.OptXalm1);
			this.PnlAlm.Controls.Add(this.OptXalm0);
			this.PnlAlm.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.PnlAlm.Location = new System.Drawing.Point(288, 80);
			this.PnlAlm.Name = "PnlAlm";
			this.PnlAlm.Size = new System.Drawing.Size(105, 25);
			this.PnlAlm.TabIndex = 96;
			// 
			// OptXalm1
			// 
			this.OptXalm1.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXalm1.Location = new System.Drawing.Point(8, 0);
			this.OptXalm1.Name = "OptXalm1";
			this.OptXalm1.Size = new System.Drawing.Size(41, 25);
			this.OptXalm1.TabIndex = 1;
			this.OptXalm1.Text = "A��";
			this.OptXalm1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OptXalm0
			// 
			this.OptXalm0.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXalm0.Location = new System.Drawing.Point(56, 0);
			this.OptXalm0.Name = "OptXalm0";
			this.OptXalm0.Size = new System.Drawing.Size(41, 25);
			this.OptXalm0.TabIndex = 0;
			this.OptXalm0.Text = "B��";
			this.OptXalm0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// PnlEls
			// 
			this.PnlEls.Controls.Add(this.OptXels1);
			this.PnlEls.Controls.Add(this.OptXels0);
			this.PnlEls.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.PnlEls.Location = new System.Drawing.Point(288, 16);
			this.PnlEls.Name = "PnlEls";
			this.PnlEls.Size = new System.Drawing.Size(105, 25);
			this.PnlEls.TabIndex = 95;
			// 
			// OptXels1
			// 
			this.OptXels1.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXels1.Location = new System.Drawing.Point(8, 0);
			this.OptXels1.Name = "OptXels1";
			this.OptXels1.Size = new System.Drawing.Size(41, 25);
			this.OptXels1.TabIndex = 1;
			this.OptXels1.Text = "A��";
			this.OptXels1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OptXels0
			// 
			this.OptXels0.Appearance = System.Windows.Forms.Appearance.Button;
			this.OptXels0.Location = new System.Drawing.Point(56, 0);
			this.OptXels0.Name = "OptXels0";
			this.OptXels0.Size = new System.Drawing.Size(41, 25);
			this.OptXels0.TabIndex = 0;
			this.OptXels0.Text = "B��";
			this.OptXels0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LblXerr
			// 
			this.LblXerr.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblXerr.Location = new System.Drawing.Point(16, 320);
			this.LblXerr.Name = "LblXerr";
			this.LblXerr.Size = new System.Drawing.Size(185, 25);
			this.LblXerr.TabIndex = 94;
			// 
			// LblXint
			// 
			this.LblXint.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblXint.Location = new System.Drawing.Point(16, 288);
			this.LblXint.Name = "LblXint";
			this.LblXint.Size = new System.Drawing.Size(185, 25);
			this.LblXint.TabIndex = 93;
			// 
			// LblXspd
			// 
			this.LblXspd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXspd.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblXspd.Location = new System.Drawing.Point(272, 248);
			this.LblXspd.Name = "LblXspd";
			this.LblXspd.Size = new System.Drawing.Size(113, 17);
			this.LblXspd.TabIndex = 92;
			this.LblXspd.Text = "0";
			this.LblXspd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// LblXpos
			// 
			this.LblXpos.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXpos.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblXpos.Location = new System.Drawing.Point(272, 224);
			this.LblXpos.Name = "LblXpos";
			this.LblXpos.Size = new System.Drawing.Size(113, 17);
			this.LblXpos.TabIndex = 91;
			this.LblXpos.Text = "0";
			this.LblXpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// Label13
			// 
			this.Label13.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.Label13.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.Label13.Location = new System.Drawing.Point(200, 248);
			this.Label13.Name = "Label13";
			this.Label13.Size = new System.Drawing.Size(67, 18);
			this.Label13.TabIndex = 90;
			this.Label13.Text = "���ݑ��x";
			// 
			// Label12
			// 
			this.Label12.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.Label12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.Label12.Location = new System.Drawing.Point(200, 224);
			this.Label12.Name = "Label12";
			this.Label12.Size = new System.Drawing.Size(67, 18);
			this.Label12.TabIndex = 89;
			this.Label12.Text = "���݈ʒu";
			// 
			// TxtFH
			// 
			this.TxtFH.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.TxtFH.Location = new System.Drawing.Point(16, 240);
			this.TxtFH.Name = "TxtFH";
			this.TxtFH.Size = new System.Drawing.Size(161, 22);
			this.TxtFH.TabIndex = 88;
			this.TxtFH.Text = "";
			this.TxtFH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// Label11
			// 
			this.Label11.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.Label11.Location = new System.Drawing.Point(48, 216);
			this.Label11.Name = "Label11";
			this.Label11.Size = new System.Drawing.Size(106, 18);
			this.Label11.TabIndex = 87;
			this.Label11.Text = "���쑬�x(PPS)";
			this.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// BtnStp
			// 
			this.BtnStp.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.BtnStp.Location = new System.Drawing.Point(16, 176);
			this.BtnStp.Name = "BtnStp";
			this.BtnStp.Size = new System.Drawing.Size(161, 25);
			this.BtnStp.TabIndex = 76;
			this.BtnStp.Text = "��~";
			this.BtnStp.Click += new System.EventHandler(this.BtnStp_Click);
			// 
			// LblSvon
			// 
			this.LblSvon.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblSvon.Location = new System.Drawing.Point(240, 176);
			this.LblSvon.Name = "LblSvon";
			this.LblSvon.Size = new System.Drawing.Size(41, 17);
			this.LblSvon.TabIndex = 86;
			this.LblSvon.Text = "SVON";
			this.LblSvon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LblOls
			// 
			this.LblOls.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblOls.Location = new System.Drawing.Point(240, 112);
			this.LblOls.Name = "LblOls";
			this.LblOls.Size = new System.Drawing.Size(41, 17);
			this.LblOls.TabIndex = 85;
			this.LblOls.Text = "OLS";
			this.LblOls.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LblAlm
			// 
			this.LblAlm.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblAlm.Location = new System.Drawing.Point(240, 80);
			this.LblAlm.Name = "LblAlm";
			this.LblAlm.Size = new System.Drawing.Size(49, 17);
			this.LblAlm.TabIndex = 84;
			this.LblAlm.Text = "SVALM";
			this.LblAlm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LblMel
			// 
			this.LblMel.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblMel.Location = new System.Drawing.Point(240, 48);
			this.LblMel.Name = "LblMel";
			this.LblMel.Size = new System.Drawing.Size(41, 17);
			this.LblMel.TabIndex = 83;
			this.LblMel.Text = "-ELS";
			this.LblMel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LblPel
			// 
			this.LblPel.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblPel.Location = new System.Drawing.Point(240, 16);
			this.LblPel.Name = "LblPel";
			this.LblPel.Size = new System.Drawing.Size(41, 17);
			this.LblPel.TabIndex = 82;
			this.LblPel.Text = "+ELS";
			this.LblPel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LblXsvon
			// 
			this.LblXsvon.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.LblXsvon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXsvon.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblXsvon.Location = new System.Drawing.Point(208, 176);
			this.LblXsvon.Name = "LblXsvon";
			this.LblXsvon.Size = new System.Drawing.Size(17, 17);
			this.LblXsvon.TabIndex = 81;
			// 
			// LblXols
			// 
			this.LblXols.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.LblXols.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXols.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblXols.Location = new System.Drawing.Point(208, 112);
			this.LblXols.Name = "LblXols";
			this.LblXols.Size = new System.Drawing.Size(17, 17);
			this.LblXols.TabIndex = 80;
			// 
			// LblXalm
			// 
			this.LblXalm.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.LblXalm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXalm.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblXalm.Location = new System.Drawing.Point(208, 80);
			this.LblXalm.Name = "LblXalm";
			this.LblXalm.Size = new System.Drawing.Size(17, 17);
			this.LblXalm.TabIndex = 79;
			// 
			// LblXmel
			// 
			this.LblXmel.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.LblXmel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXmel.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblXmel.Location = new System.Drawing.Point(208, 48);
			this.LblXmel.Name = "LblXmel";
			this.LblXmel.Size = new System.Drawing.Size(17, 17);
			this.LblXmel.TabIndex = 78;
			// 
			// LblXpel
			// 
			this.LblXpel.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.LblXpel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.LblXpel.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblXpel.Location = new System.Drawing.Point(208, 16);
			this.LblXpel.Name = "LblXpel";
			this.LblXpel.Size = new System.Drawing.Size(17, 17);
			this.LblXpel.TabIndex = 77;
			// 
			// BtnOrg2
			// 
			this.BtnOrg2.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.BtnOrg2.Location = new System.Drawing.Point(16, 80);
			this.BtnOrg2.Name = "BtnOrg2";
			this.BtnOrg2.Size = new System.Drawing.Size(161, 25);
			this.BtnOrg2.TabIndex = 75;
			this.BtnOrg2.Text = "ELS���p���_���A";
			this.BtnOrg2.Click += new System.EventHandler(this.BtnOrg2_Click);
			// 
			// BtnOrg1
			// 
			this.BtnOrg1.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.BtnOrg1.Location = new System.Drawing.Point(16, 48);
			this.BtnOrg1.Name = "BtnOrg1";
			this.BtnOrg1.Size = new System.Drawing.Size(161, 25);
			this.BtnOrg1.TabIndex = 74;
			this.BtnOrg1.Text = "OLS+Z�����_���A";
			this.BtnOrg1.Click += new System.EventHandler(this.BtnOrg1_Click);
			// 
			// BtnOrg0
			// 
			this.BtnOrg0.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.BtnOrg0.Location = new System.Drawing.Point(16, 16);
			this.BtnOrg0.Name = "BtnOrg0";
			this.BtnOrg0.Size = new System.Drawing.Size(161, 25);
			this.BtnOrg0.TabIndex = 73;
			this.BtnOrg0.Text = "OLS���o���_���A";
			this.BtnOrg0.Click += new System.EventHandler(this.BtnOrg0_Click);
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// DlgOrg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(552, 437);
			this.Controls.Add(this.PnlSvon);
			this.Controls.Add(this.PnlOls);
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
			this.Controls.Add(this.LblOls);
			this.Controls.Add(this.LblAlm);
			this.Controls.Add(this.LblMel);
			this.Controls.Add(this.LblPel);
			this.Controls.Add(this.LblXsvon);
			this.Controls.Add(this.LblXols);
			this.Controls.Add(this.LblXalm);
			this.Controls.Add(this.LblXmel);
			this.Controls.Add(this.LblXpel);
			this.Controls.Add(this.BtnOrg2);
			this.Controls.Add(this.BtnOrg1);
			this.Controls.Add(this.BtnOrg0);
			this.Name = "DlgOrg";
			this.Text = "�w�����_���A����";
			this.Load += new System.EventHandler(this.DlgOrg_Load);
			this.Closed += new System.EventHandler(this.DlgOrg_Closed);
			this.PnlSvon.ResumeLayout(false);
			this.PnlOls.ResumeLayout(false);
			this.PnlAlm.ResumeLayout(false);
			this.PnlEls.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		// ���W�I�{�^���̔z�������
		private System.Windows.Forms.RadioButton[] OptXels;
		private System.Windows.Forms.RadioButton[] OptXalm;
		private System.Windows.Forms.RadioButton[] OptXols;
		private System.Windows.Forms.RadioButton[] OptXsvon;
		
		// ���씻�f�t���O(0:�����C1:�葬)
		private ushort gStart = 0;

		// �t�H�[�������悤�Ƃ��鎞�̏���
		private void DlgOrg_Closed(object sender, System.EventArgs e)
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
		private void DlgOrg_Load(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;
			
			// �{�^���ݒ�
			OptXels0.Checked = true;
			OptXalm0.Checked = true;
			OptXols0.Checked = true;
			OptXsvon0.Checked = true;

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

			//�{�^���R���g���[���z��̍쐬
			this.OptXels = new System.Windows.Forms.RadioButton[]
				{this.OptXels0, this.OptXels1};

			this.OptXalm = new System.Windows.Forms.RadioButton[]
				{this.OptXalm0, this.OptXalm1};
			
			this.OptXols = new System.Windows.Forms.RadioButton[]
				{this.OptXols0, this.OptXols1};
			
			this.OptXsvon = new System.Windows.Forms.RadioButton[]
				{this.OptXsvon0, this.OptXsvon1};

			//�C�x���g�n���h���Ɋ֘A�t��
			for (int i = 0; i < this.OptXels.Length; i++)
				this.OptXels[i].Click +=
					new EventHandler(this.OptXels_Click);
			for (int i = 0; i < this.OptXalm.Length; i++)
				this.OptXalm[i].Click +=
					new EventHandler(this.OptXalm_Click);
			for (int i = 0; i < this.OptXols.Length; i++)
				this.OptXols[i].Click +=
					new EventHandler(this.OptXols_Click);
			for (int i = 0; i < this.OptXsvon.Length; i++)
				this.OptXsvon[i].Click +=
					new EventHandler(this.OptXsvon_Click);

			// �X�e�[�^�X�\���p�^�C�}�[�J�n
			timer1.Enabled = true;
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

		//�w��OLS���͋ɐ�
		private void OptXols_Click(object sender, EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;
			ushort Index = 0;

			for (int i = 0; i < OptXols.Length; i++)
			{
				if (sender.Equals(OptXols[i])==true)
				{
					Index = (ushort)i;
					break;
				}
			}
			unRet = Cp530l1a.hcp530_SetOls(hDev, 0, Index);
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
					BtnOrg0.Enabled = true;
					BtnOrg1.Enabled = true;
					BtnOrg2.Enabled = true;
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
				// OLS ���ʕ\��(OLS ON�F�ΐF�\���COLS OFF�F���F�\��)
				if(0x4000==(ssts & 0x4000))
				{
					LblXols.BackColor = Color.LimeGreen;
				}
				else
				{
					LblXols.BackColor = Color.White;
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
				// ���ݑ��x�̕\��
				LblXspd.Text = usSpd.ToString();
			}
			else
			{
				return;
			}
		}

		//---------------------------------------
		//  �w���FOLS���o���_���A�X�^�[�g
		//---------------------------------------
		private void BtnOrg0_Click(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;
			uint fh = 1;
			
			// ���_���A���[�h�ݒ�(OLS���o)
			unRet = Cp530l1a.hcp530_SetOrgMode(hDev, 0, 1); 
			if (0 == unRet)
			{
				if(this.OptXels0.Checked==true)	unRet = Cp530l1a.hcp530_SetEls(hDev, 0, 0, 0);
				else							unRet = Cp530l1a.hcp530_SetEls(hDev, 0, 1, 0);
			}
			if (0 != unRet)
			{
				s = "�G���[���������܂���!\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
				return;
			}		

			// �e�L�X�g��integer�^�ɕϊ����āA�ϐ��ɑ�����܂��D
			try
			{
				fh = uint.Parse(TxtFH.Text);
			}
			catch
			{
				// �e�L�X�g�������ȊO�������ꍇ�A���b�Z�[�W���o���܂��D
				s = "���쑬�x���ݒ�͈͊O�ł��D";
				MessageBox.Show(s);
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

			// ���샂�[�h�ݒ�(���_���A)
			unRet = Cp530l1a.hcp530_WritOpeMode(hDev, 0, 0x18);
			if (0 == unRet)
			{
				// �����X�^�[�g
				unRet = Cp530l1a.hcp530_AccStart(hDev, 1);     
				if (0 == unRet)
				{    
					BtnOrg0.Enabled = false;
				    BtnOrg1.Enabled = false;
				    BtnOrg2.Enabled = false;
				    LblXerr.Text = "";
				    LblXint.Text = "";
				}
				else
				{
					s = "�G���[���������܂���!\r\n�߂�l�F" + unRet.ToString("X8");
					MessageBox.Show(s);
				}
			}
			else
			{
				s = "�G���[���������܂���!\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
			}
		}

		//---------------------------------------
		//  �w���FOLS+Z�����_���A�X�^�[�g
		//---------------------------------------
		private void BtnOrg1_Click(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;
			uint fh = 1;

			// ���_���A���[�h�ݒ�(OLS+Z��)
			unRet = Cp530l1a.hcp530_SetOrgMode(hDev, 0, 2); 
			if (0 == unRet)
			{
				if(this.OptXels0.Checked==true)	unRet = Cp530l1a.hcp530_SetEls(hDev, 0, 0, 0);
				else							unRet = Cp530l1a.hcp530_SetEls(hDev, 0, 1, 0);
			}
			if (0 != unRet)
			{
				s = "�G���[���������܂���!\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
				return;
			}		

			// �e�L�X�g��integer�^�ɕϊ����āA�ϐ��ɑ�����܂��D
			try
			{
				fh = uint.Parse(TxtFH.Text);
			}
			catch
			{
				// �e�L�X�g�������ȊO�������ꍇ�A���b�Z�[�W���o���܂��D
				s = "���쑬�x���ݒ�͈͊O�ł��D";
				MessageBox.Show(s);
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

			// ���샂�[�h�ݒ�(���_���A)
			unRet = Cp530l1a.hcp530_WritOpeMode(hDev, 0, 0x18);
			if (0 == unRet)
			{
				// �����X�^�[�g
				unRet = Cp530l1a.hcp530_AccStart(hDev, 1);     
				if (0 == unRet)
				{    
					BtnOrg0.Enabled = false;
					BtnOrg1.Enabled = false;
					BtnOrg2.Enabled = false;
					LblXerr.Text = "";
					LblXint.Text = "";
				}
				else
				{
					s = "�G���[���������܂���!\r\n�߂�l�F" + unRet.ToString("X8");
					MessageBox.Show(s);
				}
			}
			else
			{
				s = "�G���[���������܂���!\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
			}		
		}

		//---------------------------------------
		//  �w���FELS���p���_���A�X�^�[�g
		//---------------------------------------
		private void BtnOrg2_Click(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			uint hDev = spc53004Class.hDeviceID;
			string s;
			uint fh = 1;

			// ���_���A���[�h�ݒ�(ELS���p)
			unRet = Cp530l1a.hcp530_SetOrgMode(hDev, 0, 6); 
			if (0 == unRet)
			{
				if(this.OptXels0.Checked==true)	unRet = Cp530l1a.hcp530_SetEls(hDev, 0, 0, 1);
				else							unRet = Cp530l1a.hcp530_SetEls(hDev, 0, 1, 1);
			}
			if (0 != unRet)
			{
				s = "�G���[���������܂���!\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
				return;
			}		

			// �e�L�X�g��integer�^�ɕϊ����āA�ϐ��ɑ�����܂��D
			try
			{
				fh = uint.Parse(TxtFH.Text);
			}
			catch
			{
				// �e�L�X�g�������ȊO�������ꍇ�A���b�Z�[�W���o���܂��D
				s = "���쑬�x���ݒ�͈͊O�ł��D";
				MessageBox.Show(s);
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

			// ���샂�[�h�ݒ�(���_���A)
			unRet = Cp530l1a.hcp530_WritOpeMode(hDev, 0, 0x18);
			if (0 == unRet)
			{
				// �����X�^�[�g
				unRet = Cp530l1a.hcp530_AccStart(hDev, 1);     
				if (0 == unRet)
				{    
					BtnOrg0.Enabled = false;
					BtnOrg1.Enabled = false;
					BtnOrg2.Enabled = false;
					LblXerr.Text = "";
					LblXint.Text = "";
				}
				else
				{
					s = "�G���[���������܂���!\r\n�߂�l�F" + unRet.ToString("X8");
					MessageBox.Show(s);
				}
			}
			else
			{
				s = "�G���[���������܂���!\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
			}				
		}

		//---------------------------------------
		//	�w���F��~
		//---------------------------------------
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
