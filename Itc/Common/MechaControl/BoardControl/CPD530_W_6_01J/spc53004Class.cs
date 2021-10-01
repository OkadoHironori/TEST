using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

namespace spc53004
{  
/// <summary>
/// spc53004Class �̊T�v�̐����ł��B
/// </summary>
	class spc53004Class
	{
		//------------------------------------------------------------------------------
		//  �O���[�o���萔
		//------------------------------------------------------------------------------
		/// �f�o�C�X�֌W�萔
		/// ���̃T���v���ł�16���܂�
		public const uint CNTMAX = 16;

		//------------------------------------------------------------------------------
		//  �O���[�o���ϐ�
		//------------------------------------------------------------------------------
		// �f�o�C�X�֌W
		/// <summary>
		/// �f�o�C�X�n���h��
		/// </summary>
		public static uint hDeviceID;

		/// <summary>
		/// �f�o�C�X��
		/// </summary>
		public static uint gCnt;                          

		/// <summary>
		/// �f�o�C�X���
		/// </summary>
		static public Hicpd530.HPCDEVICEINFO[] gHpcDevInfo = new Hicpd530.HPCDEVICEINFO[CNTMAX];

		/// <summary>
		/// ���̃v���O�����ŋ��ʂɎg���N���X
		/// </summary>
		///------------------------------------------------------------------------------
		/// �f�o�C�X�̏�����
		///------------------------------------------------------------------------------
		public static uint IniDev()
		{
			ushort nIx = 0;
			uint unRet = 0;
			Hicpd530.HPCDEVICEINFO[] hpcDevInfo = new Hicpd530.HPCDEVICEINFO[CNTMAX];
			string s;			

			// �{�[�h���� & �f�o�C�X���擾
            unRet = Cp530l1a.hcp530_GetDevInfo(ref gCnt, gHpcDevInfo);
			if (unRet != 0)
			{
				s = "�f�o�C�X��񂪎擾�ł��܂���\r\n�߂�l�F" + unRet.ToString("X8");
				MessageBox.Show(s);
				return (unRet);
			}

			hpcDevInfo[0].dwBoardID = 1;

			// �{�[�h�w��(�{�[�hID = 0�̃{�[�h�g�p)
			for(nIx=0; nIx<gCnt; nIx++)
			{
				if(0 == gHpcDevInfo[nIx].dwBoardID)
				{
					hpcDevInfo[0] = gHpcDevInfo[nIx];
					break;
				}
			}
			if (hpcDevInfo[0].dwBoardID !=0 )
			{
				MessageBox.Show("�{�[�hID = 0 �̃{�[�h����������Ă��܂���.");
				return (0x1000);
			}
			// �f�o�C�X�I�[�v��
			unRet = Cp530l1a.hcp530_DevOpen( ref hDeviceID, ref hpcDevInfo[0] );
			if (unRet != 0)
			{
				s = "�f�o�C�X�I�[�v���ł��܂���\r\n�߂�l�F" + unRet.ToString("X8"); 
				MessageBox.Show(s);
			}
			return (unRet);
		}
	}
}
