using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace spc53004
{
	/// <summary>
	/// Form1 �̊T�v�̐����ł��B
	/// </summary>
	public class DlgInx : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.GroupBox GroupBox1;
		internal System.Windows.Forms.RadioButton OprMode7;
		internal System.Windows.Forms.RadioButton OprMode6;
		internal System.Windows.Forms.RadioButton OprMode5;
		internal System.Windows.Forms.RadioButton OprMode4;
		internal System.Windows.Forms.RadioButton OprMode3;
		internal System.Windows.Forms.RadioButton OprMode2;
		internal System.Windows.Forms.RadioButton OprMode1;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// �v���O�����I�����
		/// </summary>
		public DlgInx()
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
				if (components != null) 
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
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.OprMode7 = new System.Windows.Forms.RadioButton();
			this.OprMode6 = new System.Windows.Forms.RadioButton();
			this.OprMode5 = new System.Windows.Forms.RadioButton();
			this.OprMode4 = new System.Windows.Forms.RadioButton();
			this.OprMode3 = new System.Windows.Forms.RadioButton();
			this.OprMode2 = new System.Windows.Forms.RadioButton();
			this.OprMode1 = new System.Windows.Forms.RadioButton();
			this.GroupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// GroupBox1
			// 
			this.GroupBox1.Controls.Add(this.OprMode7);
			this.GroupBox1.Controls.Add(this.OprMode6);
			this.GroupBox1.Controls.Add(this.OprMode5);
			this.GroupBox1.Controls.Add(this.OprMode4);
			this.GroupBox1.Controls.Add(this.OprMode3);
			this.GroupBox1.Controls.Add(this.OprMode2);
			this.GroupBox1.Controls.Add(this.OprMode1);
			this.GroupBox1.Font = new System.Drawing.Font("�l�r �S�V�b�N", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.GroupBox1.Location = new System.Drawing.Point(20, 14);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(512, 408);
			this.GroupBox1.TabIndex = 11;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "����I��";

			// 
			// OprMode7
			// 
			this.OprMode7.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.OprMode7.Location = new System.Drawing.Point(48, 352);
			this.OprMode7.Name = "OprMode7";
			this.OprMode7.Size = new System.Drawing.Size(144, 24);
			this.OprMode7.TabIndex = 8;
			this.OprMode7.Text = "�v���O�����I��";
			// 
			// OprMode6
			// 
			this.OprMode6.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.OprMode6.Location = new System.Drawing.Point(48, 280);
			this.OprMode6.Name = "OprMode6";
			this.OprMode6.Size = new System.Drawing.Size(136, 24);
			this.OprMode6.TabIndex = 6;
			this.OprMode6.Text = "�~�ʕ�ԓ���";
			// 
			// OprMode5
			// 
			this.OprMode5.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.OprMode5.Location = new System.Drawing.Point(48, 232);
			this.OprMode5.Name = "OprMode5";
			this.OprMode5.Size = new System.Drawing.Size(136, 24);
			this.OprMode5.TabIndex = 5;
			this.OprMode5.Text = "������ԓ���";
			// 
			// OprMode4
			// 
			this.OprMode4.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.OprMode4.Location = new System.Drawing.Point(48, 184);
			this.OprMode4.Name = "OprMode4";
			this.OprMode4.Size = new System.Drawing.Size(136, 24);
			this.OprMode4.TabIndex = 4;
			this.OprMode4.Text = "�ʒu���ߓ���";
			// 
			// OprMode3
			// 
			this.OprMode3.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.OprMode3.Location = new System.Drawing.Point(48, 136);
			this.OprMode3.Name = "OprMode3";
			this.OprMode3.Size = new System.Drawing.Size(136, 24);
			this.OprMode3.TabIndex = 3;
			this.OprMode3.Text = "�A�����蓮��";
			// 
			// OprMode2
			// 
			this.OprMode2.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.OprMode2.Location = new System.Drawing.Point(48, 88);
			this.OprMode2.Name = "OprMode2";
			this.OprMode2.Size = new System.Drawing.Size(136, 24);
			this.OprMode2.TabIndex = 2;
			this.OprMode2.Text = "���_���A����";
			// 
			// OprMode1
			// 
			this.OprMode1.Font = new System.Drawing.Font("�l�r �S�V�b�N", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.OprMode1.Location = new System.Drawing.Point(48, 40);
			this.OprMode1.Name = "OprMode1";
			this.OprMode1.Size = new System.Drawing.Size(232, 24);
			this.OprMode1.TabIndex = 1;
			this.OprMode1.Text = "�f�o�C�X�I�[�v��/�N���[�Y";
			// 
			// DlgInx
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(552, 437);
			this.Controls.Add(this.GroupBox1);
			this.Name = "DlgInx";
			this.Text = "[HVT] HPCI-CPD530�V���[�Y�@�T���v���v���O����";
			this.Load += new System.EventHandler(this.DlgInx_Load);
			this.Closed += new System.EventHandler(this.DlgInx_Closed);
			this.GroupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// �A�v���P�[�V�����̃��C�� �G���g�� �|�C���g�ł��B
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new DlgInx());
		}

		private System.Windows.Forms.RadioButton[] OprModes;

		// �t�H�[�����[�h����
		private void DlgInx_Load(object sender, System.EventArgs e)
		{
			//�{�^���R���g���[���z��̍쐬
			this.OprModes = new System.Windows.Forms.RadioButton[7];

			//�{�^���R���g���[���̔z��ɂ��łɍ쐬����Ă���C���X�^���X����
			this.OprModes[0] = this.OprMode1;
			this.OprModes[1] = this.OprMode2;
			this.OprModes[2] = this.OprMode3;
			this.OprModes[3] = this.OprMode4;
			this.OprModes[4] = this.OprMode5;
			this.OprModes[5] = this.OprMode6;
			this.OprModes[6] = this.OprMode7;

			this.OprMode1.Checked = true;

			//�C�x���g�n���h���Ɋ֘A�t���i�K�v�Ȏ��̂݁j
			for (int i = 0; i < this.OprModes.Length; i++)
			{
				this.OprModes[i].MouseUp +=
					new System.Windows.Forms.MouseEventHandler(this.OprModes_MouseUp);
			}
		}

		//Button�̃N���b�N�C�x���g�n���h��
		private void OprModes_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			int i = 0;
			int nIx = 8;
			uint unRet = 0;
			DlgInx f0 = new DlgInx();
			DlgDev f1 = new DlgDev();
			DlgOrg f2 = new DlgOrg();
			DlgCnt f3 = new DlgCnt();
			DlgPos f4 = new DlgPos();
			DlgLin f5 = new DlgLin();
			DlgCir f6 = new DlgCir();

			for (i = 0; i < OprModes.Length; i++)
			{
				if (sender.Equals(OprModes[i])==true)
				{
					nIx = i;
					break;
				}
			}
			switch(nIx)
			{
				// �f�o�C�X���擾
				case 0:
					// �f�o�C�X�I�[�v���^�N���[�Y�̃_�C�A���O�\��
					f1.Show();
					// ���C���̃E�B���h�E���B��
					this.Hide();
					break;
				// ���_���A����
				case 1: 
					// ���_���A���@�̏����l��OLS+Z�����_���A
					// �f�o�C�X�̏�����
					unRet = spc53004Class.IniDev();
					if(0==unRet)
					{
						// ���_���A����̃_�C�A���O�\��
						f2.Show();
						// ���C���̃E�B���h�E���B��
						this.Hide();
					} 
					else 
					{
						break;
					}
					break;
				// �A�����蓮��
				case 2: 
					// �f�o�C�X�̏�����
					unRet = spc53004Class.IniDev();
					if(0==unRet)
					{
						// �A�����蓮��̃_�C�A���O�쐬
						f3.Show(); 
						// ���C���̃E�B���h�E���B��
						this.Hide();
					}
					break;
				// �ʒu���ߓ���
				case 3: 
					// �f�o�C�X�̏�����
					unRet = spc53004Class.IniDev();
					if(0==unRet)
					{
						// �ʒu���ߓ���̃_�C�A���O�쐬
						f4.Show(); 
						// ���C���̃E�B���h�E���B��
						this.Hide();
					} 
					break;
				// ������ԓ���
				case 4: 
					// �f�o�C�X�̏�����
					unRet = spc53004Class.IniDev();
					if(0==unRet)
					{
						// ������ԓ���̃_�C�A���O�쐬
						f5.Show(); 
						// ���C���̃E�B���h�E���B��
						this.Hide();
					} 
					break;
				// �~�ʕ�ԓ���
				case 5: 
					// �f�o�C�X�̏�����
					unRet = spc53004Class.IniDev();
					if(0==unRet)
					{
						// �~�ʕ�ԓ���̃_�C�A���O�쐬
						f6.Show(); 
						// ���C���̃E�B���h�E���B��
						this.Hide();
					} 
					break;
				// �v���O�����I��
				case 6: 
					this.Close();
					break;
				default: 
					break;
			}
		}

		private void DlgInx_Closed(object sender, System.EventArgs e)
		{
			DlgInx f0 = new DlgInx();
			DlgDev f1 = new DlgDev();
			DlgOrg f2 = new DlgOrg();
			DlgCnt f3 = new DlgCnt();
			DlgPos f4 = new DlgPos();
			DlgLin f5 = new DlgLin();
			DlgCir f6 = new DlgCir();

			f0.Dispose();
			f1.Dispose();
			f2.Dispose();
			f3.Dispose();
			f4.Dispose();
			f5.Dispose();
			f6.Dispose();
			System.Environment.Exit(0);
		}
	}
}
