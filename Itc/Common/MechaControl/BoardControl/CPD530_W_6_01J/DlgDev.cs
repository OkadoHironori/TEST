using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace spc53004
{
	/// <summary>
	/// DlgDev �̊T�v�̐����ł��B
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
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// �f�o�C�X�I�[�v��/�N���[�Y�T���v��
		/// </summary>
		public DlgDev()
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
			this.LblDev.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblDev.Location = new System.Drawing.Point(280, 128);
			this.LblDev.Name = "LblDev";
			this.LblDev.Size = new System.Drawing.Size(216, 168);
			this.LblDev.TabIndex = 20;
			// 
			// LblHndl
			// 
			this.LblHndl.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.LblHndl.Location = new System.Drawing.Point(280, 48);
			this.LblHndl.Name = "LblHndl";
			this.LblHndl.Size = new System.Drawing.Size(216, 24);
			this.LblHndl.TabIndex = 19;
			// 
			// BtnDevCls
			// 
			this.BtnDevCls.Enabled = false;
			this.BtnDevCls.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.BtnDevCls.Location = new System.Drawing.Point(272, 88);
			this.BtnDevCls.Name = "BtnDevCls";
			this.BtnDevCls.Size = new System.Drawing.Size(224, 24);
			this.BtnDevCls.TabIndex = 18;
			this.BtnDevCls.Text = "�f�o�C�X�N���[�Y";
			this.BtnDevCls.Click += new System.EventHandler(this.BtnDevCls_Click);
			// 
			// BtnDevOpn
			// 
			this.BtnDevOpn.Enabled = false;
			this.BtnDevOpn.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.BtnDevOpn.Location = new System.Drawing.Point(272, 16);
			this.BtnDevOpn.Name = "BtnDevOpn";
			this.BtnDevOpn.Size = new System.Drawing.Size(224, 24);
			this.BtnDevOpn.TabIndex = 17;
			this.BtnDevOpn.Text = "�f�o�C�X�I�[�v��";
			this.BtnDevOpn.Click += new System.EventHandler(this.BtnDevOpn_Click);
			// 
			// CmbID
			// 
			this.CmbID.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.CmbID.Location = new System.Drawing.Point(136, 64);
			this.CmbID.Name = "CmbID";
			this.CmbID.Size = new System.Drawing.Size(40, 23);
			this.CmbID.TabIndex = 16;
			this.CmbID.Text = "ComboBox1";
			this.CmbID.SelectedIndexChanged += new System.EventHandler(this.CmbID_SelectedIndexChanged);
			// 
			// Label1
			// 
			this.Label1.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.Label1.Location = new System.Drawing.Point(24, 64);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(104, 24);
			this.Label1.TabIndex = 15;
			this.Label1.Text = "�{�[�hID�I��";
			// 
			// BtnDevInf
			// 
			this.BtnDevInf.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.BtnDevInf.Location = new System.Drawing.Point(16, 16);
			this.BtnDevInf.Name = "BtnDevInf";
			this.BtnDevInf.Size = new System.Drawing.Size(160, 24);
			this.BtnDevInf.TabIndex = 14;
			this.BtnDevInf.Text = "�{�[�hID�擾";
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
			this.Text = "�f�o�C�X�I�[�v��/�N���[�Y";
			this.Load += new System.EventHandler(this.DlgDev_Load);
			this.Closed += new System.EventHandler(this.DlgDev_Closed);
			this.ResumeLayout(false);

		}
		#endregion
		
		private Hicpd530.HPCDEVICEINFO hpcDevInfo = new Hicpd530.HPCDEVICEINFO();

		// �t�H�[�����[�h����
		private void DlgDev_Load(object sender, System.EventArgs e)
		{
			CmbID.Text = "";
		}

		// �t�H�[�������悤�Ƃ��鎞�̏���
		private void DlgDev_Closed(object sender, System.EventArgs e)
		{
			DlgInx f0 = new DlgInx();

			this.Dispose();
			f0.Show();
		}

		///-----------------------
		///  �f�o�C�X���擾
		///-----------------------
		private void BtnDevInf_Click(object sender, System.EventArgs e)
		{
			ushort nIx = 0;
			uint unRet = 0;
			string s;			
			ComboBox cid = new ComboBox();
			
			// �{�[�h���� & �f�o�C�X���擾
            unRet = Cp530l1a.hcp530_GetDevInfo(ref spc53004Class.gCnt, spc53004Class.gHpcDevInfo);

			if (unRet != 0)
			{
				s = "�f�o�C�X��񂪎擾�ł��܂���\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
				return;
			}

			// �R���{�{�b�N�X�̓��e�����Z�b�g����
			CmbID.Items.Clear();                                       
			for( nIx = 0; nIx<spc53004Class.gCnt; nIx++)
			{
				// �R���{�{�b�N�X�Ƀe�L�X�g�A�C�e����ǉ�����
				CmbID.Items.Add(spc53004Class.gHpcDevInfo[nIx].dwBoardID);
			}
			CmbID.Text = CmbID.Items[0].ToString();

			// �{�^���g�p��
			BtnDevOpn.Enabled = true;
        
			// �{�^���g�p�s��
			BtnDevInf.Enabled = false;
       
			// �R���{�{�b�N�X�g�p��
			CmbID.Enabled = true;            
		}

		private void BtnDevOpn_Click(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			string szBuffer;
			string s;

			// �f�o�C�X�I�[�v��
			unRet = Cp530l1a.hcp530_DevOpen(ref spc53004Class.hDeviceID, ref hpcDevInfo );
			if(unRet != 0)
			{
				s = "�f�o�C�X�I�[�v���ł��܂���\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
				return;
			}
 
			// �f�o�C�X�n���h���\��
			szBuffer = spc53004Class.hDeviceID.ToString("X8");
			LblHndl.Text = "�f�o�C�X�n���h���F" + szBuffer + "h";

			// �f�o�C�X���\��
			LblDev.Text = "*** �f�o�C�X��� ***\r\n" +
					"�o�X�ԍ��F        " + hpcDevInfo.nBusNumber.ToString() + "\r\n" +
					"�f�o�C�X�ԍ��F    " + hpcDevInfo.nDeviceNumber.ToString() + "\r\n" +
					"I/O�A�h���X�F     " + hpcDevInfo.dwIoPortAddress.ToString("X8") + "h\r\n" +
					"IRQ�ԍ��F         " + hpcDevInfo.dwIrqNo.ToString() + "\r\n" +
					"�Ǘ��ԍ��F        " + hpcDevInfo.dwNumber.ToString() + "\r\n" +
					"�{�[�hID�F        " + hpcDevInfo.dwBoardID.ToString();
			// �{�^���g�p�s��
			BtnDevOpn.Enabled = false;
       
			// �{�^���g�p�s��
			BtnDevInf.Enabled = false;
      
			// �{�^���g�p�s��
			CmbID.Enabled = false;
			
			// �{�^���g�p��
			BtnDevCls.Enabled = true;        
		}

		private void BtnDevCls_Click(object sender, System.EventArgs e)
		{
			uint unRet = 0;
			
			// �f�o�C�X�N���[�Y
			unRet = Cp530l1a.hcp530_DevClose(spc53004Class.hDeviceID);     
			
			// �f�o�C�X�n���h���\��
			LblHndl.Text = "";               
			
			// �f�o�C�X���\��
			LblDev.Text = "";

			// �{�^���g�p��
			BtnDevOpn.Enabled = true;
       
			// �{�^���g�p��
			BtnDevInf.Enabled = true;
      
			// �R���g���[���g�p��
			CmbID.Enabled = true;
			
			// �{�^���g�p�s��
			BtnDevCls.Enabled = false;
		}

		private void CmbID_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// �w�肳�ꂽ�f�o�C�X�����A�f�o�C�X�I�[�v���p�f�o�C�X���̈�ɃR�s�[
			hpcDevInfo = spc53004Class.gHpcDevInfo[CmbID.SelectedIndex];
		}
	}
}
