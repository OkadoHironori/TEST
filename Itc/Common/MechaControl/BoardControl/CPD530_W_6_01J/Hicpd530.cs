
using System;
using System.Runtime.InteropServices;

#pragma warning disable IDE1006

/// <summary>
///  PCI CPD Series Devise Driver I/F DLL for Windows	
///		file name:Hicpd530.cs			
///      date     :2010/11/11			
///      version  :1.0.0.0 �V�K�쐬	
///	Copyright(C) 2001-2009 Hivertec,inc. All Rights Reserved.	
/// </summary>
public class Hicpd530
{
	/// <summary>
	/// �f�o�C�X���
	/// </summary>

    [StructLayout(LayoutKind.Sequential)]
    public struct HPCDEVICEINFO
    {
        /// <summary>
        /// �o�X�ԍ�
        /// </summary>
		public uint nBusNumber;
		
		/// <summary>
		/// �f�o�C�X�ԍ�
		/// </summary>
        public uint nDeviceNumber;
 
		/// <summary>
		/// I/O�|�[�g�A�h���X
		/// </summary>
        public uint dwIoPortAddress;
	
		/// <summary>
		/// IRQ�ԍ�
		/// </summary>
        public uint dwIrqNo;
        
		/// <summary>
		/// �Ǘ��ԍ�(Windows 9x�ł͖���)
		/// </summary>
		public uint dwNumber;
        
		/// <summary>
		/// �{�[�hID
		/// </summary>
		public uint dwBoardID;
    }

	/// <summary>
	/// �f�o�C�X���̎擾
	/// </summary>
	/// <param name="pDevNum">�f�o�C�X�̌��̊i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_GetDeviceCount(ref uint pDevNum);
    
	/// <summary>
	/// �f�o�C�X���̎擾
	/// </summary>
	/// <param name="pDevNum">�f�o�C�X�̌��̊i�[��</param>
	/// <param name="pHpcDevInfo">�f�o�C�X���̊i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_GetDeviceInfo(ref uint pDevNum, [In, Out, MarshalAs(UnmanagedType.LPArray)] HPCDEVICEINFO[] pHpcDevInfo);
    
	/// <summary>
	/// �f�o�C�X�I�[�v��
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h���i�[��</param>
	/// <param name="pHpcDevInfo">�f�o�C�X���̊i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_OpenDevice(ref uint hDevID, ref HPCDEVICEINFO pHpcDevInfo);
    
	/// <summary>
	/// �f�o�C�X�N���[�Y
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_CloseDevice(uint hDevID);
    
	/// <summary>
	/// ���C���X�e�[�^�X�Ǎ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">��</param>
	/// <param name="usData">�f�[�^�i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_rMstsW(uint hDevID, ushort usAxis, ref ushort usData);
    
	/// <summary>
	/// �T�u�X�e�[�^�X�Ǎ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">��</param>
	/// <param name="usData">�f�[�^�i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_rSstsW(uint hDevID, ushort usAxis, ref ushort usData);
    
	/// <summary>
	/// �R�}���h����
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">��</param>
	/// <param name="usCmd">�R�}���h</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_wCmdW(uint hDevID, ushort usAxis, ushort usCmd);
    
	/// <summary>
	/// ���W�X�^�Ǎ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">��</param>
	/// <param name="byCmd">���W�X�^�Ǎ��R�}���h</param>
	/// <param name="unData">�f�[�^�i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_rReg(uint hDevID, ushort usAxis, byte byCmd, ref uint unData);
    
	/// <summary>
	/// ���W�X�^����
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">��</param>
	/// <param name="byCmd">���W�X�^�����R�}���h</param>
	/// <param name="unData">�f�[�^</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_wReg(uint hDevID, ushort usAxis, byte byCmd, uint unData);
    
	/// <summary>
	/// �I�v�V�����|�[�g�Ǐo(�o�C�g)
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="byCmd">�I�v�V�����|�[�g�Ǐo�R�}���h</param>
	/// <param name="byData">�f�[�^�i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_rPortB(uint hDevID, byte byCmd, ref byte byData);
	
	/// <summary>
	/// �I�v�V�����|�[�g�Ǐo(���[�h)
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="byCmd">�I�v�V�����|�[�g�Ǐo�R�}���h</param>
	/// <param name="usData">�f�[�^�i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_rPortW(uint hDevID, byte byCmd, ref ushort usData);
    
	/// <summary>
	/// �I�v�V�����|�[�g����(�o�C�g)
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="byCmd">�I�v�V�����|�[�g�����R�}���h</param>
	/// <param name="byData">�f�[�^</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_wPortB(uint hDevID, byte byCmd, byte byData);
	
	/// <summary>
	/// �I�v�V�����|�[�g����(���[�h)
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="byCmd">�I�v�V�����|�[�g�����R�}���h</param>
	/// <param name="usData">�f�[�^</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_wPortW(uint hDevID, byte byCmd, ushort usData);
    
	/// <summary>
	/// ���o�̓o�b�t�@�Ǐo
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">��</param>
	/// <param name="unData">�f�[�^�i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_rBufDW(uint hDevID, ushort usAxis, ref uint unData);
    
	/// <summary>
	/// ���o�̓o�b�t�@����
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">��</param>
	/// <param name="unData">�f�[�^</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_wBufDW(uint hDevID, ushort usAxis, uint unData);
    
	/// <summary>
	/// �{�[�h�ŗL�R�[�h�擾
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usCode">�{�[�h�ŗL�R�[�h�f�[�^�i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_GetBoardCode(uint hDevID, ref ushort usCode);
}
#pragma warning restore IDE1006


