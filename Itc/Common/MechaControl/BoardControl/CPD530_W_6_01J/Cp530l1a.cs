
/* ���L�U��̊֐����x���ł̃}���`�X���b�h�̓���������ꍇ */
/* public static uint hcp530_rReg(uint hDevID, ushort usAxis, byte byCmd, ref uint unReg) */
/* public static uint hcp530_wReg(uint hDevID, ushort usAxis, byte byCmd, uint unReg) */
/* public static uint hcp530_rBufDW(uint hDevID, ushort usAxis, ref uint unReg) */
/* public static uint hcp530_wBufDW(uint hDevID, ushort usAxis, uint unReg) */
/* public static uint hcp530_rPortB(uint hDevID, byte byCmd, ref byte byData) */
/* public static uint hcp530_rPortW(uint hDevID, byte byCmd, ref ushort usData) */
/* public static uint hcp530_wPortB(uint hDevID, byte byCmd, byte byData) */
/* public static uint hcp530_wPortW(uint hDevID, byte byCmd, ushort usData) */
/* ���̃R�����g�̍s��L���ɂ��܂� */
//#define APP_SYNC

using System;
using System.Runtime.InteropServices;
using System.Threading;
  
/// <summary>
/// HPCI-CPD530/CPD570(N) series Library lebel1a function for Visual C#
/// file name	:Cp530l1a.cs
/// class name	:Cp530l1a
/// Copyright(C) 2001-2012  Hivertec,inc. All Rights Reserved.
/// </summary>
public class Cp530l1a
{
	/// X���w��
	public const ushort X_AX = 0;
    /// Y���w��
	public const ushort Y_AX = 1;
	/// Z���w��
	public const ushort Z_AX = 2;
	/// U���w��
	public const ushort U_AX = 3;
	/// V���w��
	public const ushort V_AX = 4;    
	/// W���w��	
	public const ushort W_AX = 5;
	/// A���w��
	public const ushort A_AX = 6;
	/// B���w��
	public const ushort B_AX = 7;    

	// ---------------------------------
	// for CPD5016 (2010.07.29)
	// ---------------------------------
	/// X1���w��
	public const ushort X1_AX = 0;
	/// Y1���w��
	public const ushort Y1_AX = 1;
	/// Z1���w��
	public const ushort Z1_AX = 2;
	/// U1���w��
	public const ushort U1_AX = 3;
	/// X2���w��
	public const ushort X2_AX = 4;
	/// Y2���w��	
	public const ushort Y2_AX = 5;
	/// Z2���w��
	public const ushort Z2_AX = 6;
	/// U2���w��
	public const ushort U2_AX = 7;
	/// X3���w��
	public const ushort X3_AX = 8;
	/// Y3���w��	
	public const ushort Y3_AX = 9;
	/// Z3���w��
	public const ushort Z3_AX =10;
	/// U3���w��
	public const ushort U3_AX =11;
	/// X4���w��
	public const ushort X4_AX =12;
	/// Y4���w��	
	public const ushort Y4_AX =13;
	/// Z4���w��
	public const ushort Z4_AX =14;
	/// U4���w��
	public const ushort U4_AX =15;

	/// �����R�}���h
	public const ushort NOP = 0x0000;		    
    
	/// �\�t�g�E�F�A���Z�b�g
	public const ushort SRST = 0x0004;
	    
	/// CTR1(�w�߈ʒu)���Z�b�g
	public const ushort CTR1R = 0x0020;
	/// CTR2(�@�B�ʒu)���Z�b�g
	public const ushort CTR2R = 0x0021;
	/// CTR3(�΍�)���Z�b�g    
	public const ushort CTR3R = 0x0022;
	/// CTR4(�ėp)���Z�b�g
	public const ushort CTR4R = 0x0023;
	
	/// SVON ON
	public const ushort SVON = 0x0018;
	/// SVON OFF    
	public const ushort SVOFF = 0x0010;		    

	/// SVRST ON
	public const ushort SVRSTON = 0x0019;

	/// SVRST OFF
	public const ushort SVRSTOFF = 0x0011;

	/// SVCTRCL�M���o��
	public const ushort CLROUT = 0x0024;
    /// SVCTRCL�M�����Z�b�g
	public const ushort CLRRST = 0x0025;	    

	/// ����p�v�����W�X�^�̃L�����Z��
	public const ushort PRECAN = 0x0026;        
	/// RCMP5�p�v�����W�X�^(PRCP5)�̃L�����Z��
	public const ushort PCPCAN = 0x0027;        
	/// ����p�v�����W�X�^�f�[�^�̃V�t�g
	public const ushort PRESHF = 0x002b;        
	/// RCMP5�p�v�����W�X�^(PRCP5)�f�[�^�̃V�t�g
	public const ushort PCPSHF = 0x002c;        
	/** �R���p���[�^�ɂ�鑬�x�p�^�[���ύX�f�[�^
	�Ƃ��ē���p�v�����W�X�^���m�肵�܂��B
	���͑�s */
	public const ushort PRESET = 0x004f;
    
	/// PCS���͑�s
	public const ushort PCSON = 0x0028;
	/// LTC���͑�s	    
	public const ushort LTCH = 0x0029;		    
	
	/// FL�葬�X�^�[�g
	public const ushort STAFL = 0x0050;
	/// FH�葬�X�^�[�g    
	public const ushort STAFH = 0x0051;
	/// FH�葬�X�^�[�g(FH�葬->����)    
	public const ushort STAFHD = 0x0052;
	/// �����X�^�[�g(����->FH�葬->����)    
	public const ushort STAUD = 0x0053;
	/// �c��FL�葬�X�^�[�g    
	public const ushort CNTFL = 0x0054;
	/// �c��FH�葬�X�^�[�g	    
	public const ushort CNTFH = 0x0055;
	/// �c��FH�葬�X�^�[�g(FH�葬->����)		    
	public const ushort CNTD = 0x0057;		    
	/// �c�ʉ����X�^�[�g(����->FH�葬->����)
	public const ushort CNTUD = 0x0057;
	/// STA�o��(�����X�^�[�g)
	public const ushort CMSTA = 0x0006;
	/// STA���͑�s
	public const ushort SPSTA = 0x002A;		    

	/// FL�葬�֏u�����x�ύX
	public const ushort FCHGL = 0x0040;		    
	/// FH�葬�֏u�����x�ύX	
	public const ushort FCHGH = 0x0041;		    
	/// FL���x�܂Ō���	
	public const ushort FSCHL = 0x0042;		    
	/// FH���x�܂ŉ���	
	public const ushort FSCHH = 0x0043;		    
    
	/// ����~
	public const ushort CMEMG = 0x0005;		    
	/// STP�o��(������~)
	public const ushort CMSTP = 0x0007;
	/// ����~
	public const ushort STOP = 0x0049;
	/// ������~
	public const ushort SDSTP = 0x004A;		    

	/// PRMV���W�X�^��������
	public const byte WPRMV = 0x80;	            
	/// PRFL���W�X�^��������	
	public const byte WPRFL = 0x81;             
	/// PRFH���W�X�^��������	
	public const byte WPRFH = 0x82;			    
	/// PRUR���W�X�^��������	
	public const byte WPRUR = 0x83;			    
	/// PRDR���W�X�^��������	
	public const byte WPRDR = 0x84;			    
	/// PRMG���W�X�^��������	
	public const byte WPRMG = 0x85;			    
	/// PRDP���W�X�^��������	
	public const byte WPRDP = 0x86;			    
	/// PRMD���W�X�^��������	
	public const byte WPRMD = 0x87;			    
	/// PRIP���W�X�^��������	
	public const byte WPRIP = 0x88;			    
	/// PRUS���W�X�^��������	
	public const byte WPRUS = 0x89;			    
	/// PRDS���W�X�^��������	
	public const byte WPRDS = 0x8a;			    
	/// PRCP5���W�X�^��������	
	public const byte WPRCP5 = 0x8b;		    
	/// PRCI���W�X�^��������	
	public const byte WPRCI = 0x8c;			    
	/// RMV���W�X�^��������	
	public const byte WRMV = 0x90;	            
	/// RFL���W�X�^��������
	public const byte WRFL = 0x91;              
	/// RFH���W�X�^��������
	public const byte WRFH = 0x92;			    
	/// RUR���W�X�^��������
	public const byte WRUR = 0x93;			    
	/// RDR���W�X�^��������
	public const byte WRDR = 0x94;			    
	/// RMG���W�X�^��������
	public const byte WRMG = 0x95;			    
	/// RDP���W�X�^��������
	public const byte WRDP = 0x96;			    
	/// RMD���W�X�^��������
	public const byte WRMD = 0x97;			    
	/// RIP���W�X�^��������
	public const byte WRIP = 0x98;			    
	/// RUS���W�X�^��������
	public const byte WRUS = 0x99;			    
	/// RDS���W�X�^��������
	public const byte WRDS = 0x9a;			    
	/// RFA���W�X�^��������
	public const byte WRFA = 0x9b;			    
	/// RENV1���W�X�^��������
	public const byte WRENV1 = 0x9c;		    
	/// RENV2���W�X�^��������
	public const byte WRENV2 = 0x9d;		    
	/// RENV3���W�X�^��������
	public const byte WRENV3 = 0x9e;		    
	/// RENV4���W�X�^��������
	public const byte WRENV4 = 0x9f;		    
	/// RENV5���W�X�^��������
	public const byte WRENV5 = 0xa0;		    
	/// RENV6���W�X�^��������
	public const byte WRENV6 = 0xa1;		    
	/// RENV7���W�X�^��������
	public const byte WRENV7 = 0xa2;		    
	/// RCTR1���W�X�^��������
	public const byte WRCTR1 = 0xa3;		    
	/// RCTR2���W�X�^��������
	public const byte WRCTR2 = 0xa4;		    
	/// RCTR3���W�X�^��������
	public const byte WRCTR3 = 0xa5;		    
	/// RCTR4���W�X�^��������
	public const byte WRCTR4 = 0xa6;		    
	/// RCMP1���W�X�^��������
	public const byte WRCMP1 = 0xa7;		    
	/// RCMP2���W�X�^��������
	public const byte WRCMP2 = 0xa8;		    
	/// RCMP3���W�X�^��������
	public const byte WRCMP3 = 0xa9;		    
	/// RCMP4���W�X�^��������
	public const byte WRCMP4 = 0xaa;		    
	/// RCMP5���W�X�^��������
	public const byte WRCMP5 = 0xab;		    
	/// RIRQ���W�X�^��������
	public const byte WRIRQ = 0xac;			    
	/// RCI���W�X�^��������
	public const byte WRCI = 0xbc;
		            
	/// PRMV���W�X�^�ǂݏo��
	public const byte RPRMV = 0xc0;
	/// PRFL���W�X�^�ǂݏo��		    
	public const byte RPRFL = 0xc1;
	/// PRFH���W�X�^�ǂݏo��		    
	public const byte RPRFH = 0xc2;
	/// PRUR���W�X�^�ǂݏo��		    
	public const byte RPRUR = 0xc3;			    
	/// PRDR���W�X�^�ǂݏo��
	public const byte RPRDR = 0xc4;			    
	/// PRMG���W�X�^�ǂݏo��
	public const byte RPRMG = 0xc5;			    
	/// PRDP���W�X�^�ǂݏo��
	public const byte RPRDP = 0xc6;			    
	/// PRMD���W�X�^�ǂݏo��
	public const byte RPRMD = 0xc7;			    
	/// PRMD���W�X�^�ǂݏo��
	public const byte RPRIP = 0xc8;			    
	/// PRUS���W�X�^�ǂݏo��
	public const byte RPRUS = 0xc9;			    
	/// PRDS���W�X�^�ǂݏo��
	public const byte RPRDS = 0xca;			    
	/// PRCP5���W�X�^�ǂݏo��
	public const byte RPRCP5 = 0xcb;		    
	/// PRCI���W�X�^�ǂݏo��
	public const byte RPRCI = 0xcc;		        
	/// RMV���W�X�^�ǂݏo��
	public const byte RRMV = 0xd0;			    
	/// RFL���W�X�^�ǂݏo��
	public const byte RRFL = 0xd1;			    
	/// RFH���W�X�^�ǂݏo��
	public const byte RRFH = 0xd2;			    
	/// RUR���W�X�^�ǂݏo��
	public const byte RRUR = 0xd3;			    
	/// RDR���W�X�^�ǂݏo��
	public const byte RRDR = 0xd4;			    
	/// RMG���W�X�^�ǂݏo��
	public const byte RRMG = 0xd5;			    
	/// RDP���W�X�^�ǂݏo��
	public const byte RRDP = 0xd6;			    
	/// RMD���W�X�^�ǂݏo��
	public const byte RRMD = 0xd7;			    
	/// RMD���W�X�^�ǂݏo��
	public const byte RRIP = 0xd8;			    
	/// RUS���W�X�^�ǂݏo��
	public const byte RRUS = 0xd9;			    
	/// RDS���W�X�^�ǂݏo��
	public const byte RRDS = 0xda;			    
	/// RFA���W�X�^�ǂݏo��
	public const byte RRFA = 0xdb;			    
	/// RENV1���W�X�^�ǂݏo��
	public const byte RRENV1 = 0xdc;		    
	/// RENV2���W�X�^�ǂݏo��
	public const byte RRENV2 = 0xdd;		    
	/// RENV3���W�X�^�ǂݏo��
	public const byte RRENV3 = 0xde;		    
	/// RENV4���W�X�^�ǂݏo��
	public const byte RRENV4 = 0xdf;		    
	/// RENV5���W�X�^�ǂݏo��
	public const byte RRENV5 = 0xe0;		    
	/// RENV6���W�X�^�ǂݏo��
	public const byte RRENV6 = 0xe1;		    
	/// RENV7���W�X�^�ǂݏo��
	public const byte RRENV7 = 0xe2;		    
	/// RCTR1���W�X�^�ǂݏo��
	public const byte RRCTR1 = 0xe3;		    
	/// RCTR2���W�X�^�ǂݏo��
	public const byte RRCTR2 = 0xe4;		    
	/// RCTR3���W�X�^�ǂݏo��
	public const byte RRCTR3 = 0xe5;		    
	/// RCTR4���W�X�^�ǂݏo��
	public const byte RRCTR4 = 0xe6;		    
	/// RCMP1���W�X�^�ǂݏo��
	public const byte RRCMP1 = 0xe7;		    
	/// RCMP2���W�X�^�ǂݏo��
	public const byte RRCMP2 = 0xe8;		    
	/// RCMP3���W�X�^�ǂݏo��
	public const byte RRCMP3 = 0xe9;		    
	/// RCMP4���W�X�^�ǂݏo��
	public const byte RRCMP4 = 0xea;		    
	/// RCMP5���W�X�^�ǂݏo��
	public const byte RRCMP5 = 0xeb;		    
	/// RIRQ���W�X�^�ǂݏo��
	public const byte RRIRQ = 0xec;			    
	/// RLTC1���W�X�^�ǂݏo��
	public const byte RRLTC1 = 0xed;		    
	/// RLTC2���W�X�^�ǂݏo��
	public const byte RRLTC2 = 0xee;		    
	/// RLTC3���W�X�^�ǂݏo��
	public const byte RRLTC3 = 0xef;		    
	/// RLTC4���W�X�^�ǂݏo��
	public const byte RRLTC4 = 0xf0;		    
	/// RSTS���W�X�^�ǂݏo��
	public const byte RRSTS = 0xf1;
    /// REST���W�X�^�ǂݏo��//�ǋL�@��]�G���[�̏ڍ׃X�e�[�^�X
    public const byte RREST = 0xf2;			    
	/// RIST���W�X�^�ǂݏo��
	public const byte RRIST = 0xf3;			    
	/// RPLS���W�X�^�ǂݏo��
	public const byte RRPLS = 0xf4;			  
	/// RSPD���W�X�^�ǂݏo��
	public const byte RRSPD = 0xf5;			    
	/// RSDC���W�X�^�ǂݏo��
	public const byte RRSDC = 0xf6;			    
	/// RCI���W�X�^�ǂݏo��
	public const byte RRCI = 0xfc;		        
	/// RCIC���W�X�^�ǂݏo��
	public const byte RRCIC = 0xfd;		        
	/// RIPS���W�X�^�ǂݏo��
	public const byte RRIPS = 0xff;			    

	/// MSTS �X�^�[�g�R�}���h�������ݍς݃r�b�g
	public const ushort M_SCM = 0x0001;         
	/// MSTS ���쒆�r�b�g
	public const ushort M_RUN = 0x0002;         
	/// MSTS ��~���r�b�g
	public const ushort M_END = 0x0008;         
	/// MSTS �G���[�񍐃r�b�g
	public const ushort M_ERR = 0x0010;         
	/// MSTS �C�x���g�񍐃r�b�g
	public const ushort M_INT = 0x0020;         
	/// MSTS �V�[�P���X�ԍ��r�b�g
	public const ushort M_SCx = 0x00C0;         
	/// MSTS CMP1�����������r�b�g
	public const ushort M_CMP1 = 0x0100;        
	/// MSTS CMP2�����������r�b�g
	public const ushort M_CMP2 = 0x0200;        
	/// MSTS CMP3�����������r�b�g
	public const ushort M_CMP3 = 0x0400;        
	/// MSTS CMP4����������M�r�b�g
	public const ushort M_CMP4 = 0x0800;        
	/// MSTS CMP5�����������r�b�g
	public const ushort M_CMP5 = 0x1000;        
	/// MSTS ������p�v�����W�X�^�t���r�b�g
	public const ushort M_PRF = 0x4000;         
	/// MSTS CMP5�p�v�����W�X�^�t���r�b�g
	public const ushort M_PDF = 0x8000;         

	/// SSTS SVON�r�b�g
	public const ushort S_SVON = 0x0001;        
	/// SSTS SVRST�r�b�g
	public const ushort S_SVRS = 0x0002;        
	/// SSTS �������r�b�g
	public const ushort S_FU = 0x0100;          
	/// SSTS �������r�b�g
	public const ushort S_FD = 0x0200;          
	/// SSTS �葬���쒆�r�b�g
	public const ushort S_FC = 0x0400;          
	/// SSTS SVALM�r�b�g
	public const ushort S_ALM = 0x0800;         
	/// SSTS +ELS�r�b�g
	public const ushort S_PEL = 0x1000;         
	/// SSTS -ELS�r�b�g
	public const ushort S_MEL = 0x2000;         
	/// SSTS OLS�r�b�g
	public const ushort S_OLS = 0x4000;         
	/// SSTS DLS�r�b�g
	public const ushort S_DLS = 0x8000;         

	/// RSTS ��������r�b�g 0:+�C1:-
	public const uint R_DIR = 0x00000010;       
	/// RSTS STA�r�b�g
	public const uint R_STA = 0x00000020;       
	/// RSTS STP�r�b�g
	public const uint R_STP = 0x00000040;       
	/// RSTS EMG�r�b�g
	public const uint R_EMG = 0x00000080;       
	/// RSTS PCS�r�b�g
	public const uint R_PCS = 0x00000100;       
	/// RSTS SVCTRCL�r�b�g
	public const uint R_ERC = 0x00000200;       
	/// RSTS Z���r�b�g
	public const uint R_EZ = 0x00000400;        
	/// RSTS +DR�r�b�g
	public const uint R_DRP = 0x00000800;       
	/// RSTS -DR�r�b�g
	public const uint R_DRM = 0x00001000;       
	/// RSTS INPOS�r�b�g
	public const uint R_INPOS = 0x0010000;      

	/// EST CMP1(+SLS)�ɂ���~
	public const uint ES_C1 = 0x00000001;
    /// EST CMP2(-SLS)�ɂ���~   
	public const uint ES_C2 = 0x00000002;       
	/// EST CMP3�ɂ���~
	public const uint ES_C3 = 0x00000004;       
	/// EST CMP4�ɂ���~
	public const uint ES_C4 = 0x00000008;       
	/// EST CMP5�ɂ���~
	public const uint ES_C5 = 0x00000010;       
	/// EST +ELS�ɂ���~
	public const uint ES_PL = 0x00000020;       
	/// EST -ELS�ɂ���~
	public const uint ES_ML = 0x00000040;       
	/// EST SVALM�ɂ���~
	public const uint ES_AL = 0x00000080;       
	/// EST STP�ɂ���~
	public const uint ES_SP = 0x00000100;       
	/// EST EMG�ɂ���~
	public const uint ES_EM = 0x00000200;       
	/// EST DLS�ɂ���~
	public const uint ES_SD = 0x00000400;       
	/// EST ��ԃf�[�^�ُ�ɂ���~
	public const uint ES_DT = 0x00001000;       
	/// EST ��ԑ����ɂ���~
	public const uint ES_IP = 0x00002000;       
	/// EST �p���T���̓o�b�t�@�I�[�o�[�t���[
	public const uint ES_PO = 0x00004000;       
	/// EST ��ԃf�[�^�����W�I�[�o�[
	public const uint ES_AO = 0x00008000;       
	/// EST �G���R�[�_�M���G���[
	public const uint ES_EE = 0x00010000;       
	/// EST �p���T���͐M���G���[
	public const uint ES_PE = 0x00020000;       

	/// IST �����~
	public const uint IS_EN = 0x00000001;       
	/// IST ������X�^�[�g
	public const uint IS_NN = 0x00000002;       
	/// IST ����p�v�����W�X�^�t������
	public const uint IS_NM = 0x00000004;       
	/// IST PRCP5�t������
	public const uint IS_ND = 0x00000008;       
	/// IST �����J�n
	public const uint IS_US = 0x00000010;       
	/// IST �����I��
	public const uint IS_UE = 0x00000020;       
	/// IST �����J�n
	public const uint IS_DS = 0x00000040;       
	/// IST �����I��
	public const uint IS_DE = 0x00000080;       
	/// IST CMP1��������
	public const uint IS_C1 = 0x00000100;       
	/// IST CMP2��������
	public const uint IS_C2 = 0x00000200;       
	/// IST CMP3��������
	public const uint IS_C3 = 0x00000400;       
	/// IST CMP4��������
	public const uint IS_C4 = 0x00000800;       
	/// IST CMP5��������
	public const uint IS_C5 = 0x00001000;       
	/// IST OLSon�ɂ�郉�b�`
	public const uint IS_OL = 0x00008000;       
	/// IST DLS����OFF��ON
	public const uint IS_SD = 0x00010000;       
	/// IST +DR����OFF��ON
	public const uint IS_PD = 0x00020000;       
	/// IST -DR����OFF��ON
	public const uint IS_MD = 0x00040000;       
	/// IST STA����OFF��ON
	public const uint IS_SA = 0x00080000;       

	/// DLS�L���ݒ�r�b�g(PRMD)
	public const uint MD_DLSE_BIT = 0x00000100;
	/// INPOS����L���ݒ�r�b�g(PRMD)	
	public const uint MD_INPE_BIT = 0x00000200;
	/// �����������ݒ�r�b�g(PRMD) 		
	public const uint MD_ACC_BIT = 0x00000400;
	/// �����J�n�_�v�Z���@�ݒ�r�b�g(PRMD) 		
	public const uint MD_SDP_BIT = 0x00002000;
	/// PCS�L���ݒ�r�b�g(PRMD) 		
	public const uint MD_PCSE_BIT = 0x00004000;
	/// �X�^�[�g����ݒ�r�b�g0(PRMD) 		
	public const uint MD_MSY0_BIT = 0x00040000;
	/// �X�^�[�g����ݒ�r�b�g1(PRMD) 		
	public const uint MD_MSY1_BIT = 0x00080000;
	/// STP���͗L���ݒ�r�b�g(PRMD) 		
	public const uint MD_STPE_BIT = 0x01000000;	 		

	/// ELS���o������ݒ�r�b�g(RENV1)
	public const uint R1_ELSM_BIT = 0x00000008;	 		
	/// DLS���o������ݒ�r�b�g(RENV1)
	public const uint R1_DLSM_BIT = 0x00000010;	 		
	/// DLS���b�`����ݒ�r�b�g(RENV1) 
	public const uint R1_DLLT_BIT = 0x00000020;			
	/// DLS���͘_���ݒ�r�b�g(RENV1)
	public const uint R1_DLSL_BIT = 0x00000040;	 		
	/// OLS���͘_���ݒ�r�b�g(RENV1)
	public const uint R1_OLSL_BIT = 0x00000080;	 		
	/// SVALM���o������ݒ�r�b�g(RENV1)
	public const uint R1_ALMM_BIT = 0x00000100;	 		
	/// SVALM���͘_���ݒ�r�b�g(RENV1) 
	public const uint R1_ALML_BIT = 0x00000200;			
	/// �ُ��~��SVCTRCL�����o�͐ݒ�r�b�g(RENV1) 	
	public const uint R1_CLRE_BIT = 0x00000400;		
	/// ���_���A������SVCTRCL�����o�͐ݒ�r�b�g(RENV1)
	public const uint R1_CLRO_BIT = 0x00000800;	 		
	/// STP���͎���~���@�ݒ�r�b�g(RENV1)
	public const uint R1_CSTP_BIT = 0x00080000; 
	/// INPOS���͘_���ݒ�r�b�g(RENV1)
	public const uint R1_INPL_BIT = 0x00400000;	 		
	/// PCS���͘_���ݒ�r�b�g(RENV1)
	public const uint R1_PCSL_BIT = 0x01000000;	 		
	/// EZ���͘_���ݒ�r�b�g(RENV2)
	public const uint R2_EZL_BIT = 0x00800000;

    /// HPCI-CPD553 �f�W�^���t�B���^���Ԑݒ�|�[�g
    public const byte DIGIFIL = 0x40;
    /// HPCI-CPD553 �f�W�^���t�B���^�L���I���|�[�g
    public const byte DIGISEL = 0x42;
    /// HPCI-CPD553 ���b�`�r�b�g�I���|�[�g
    public const byte LATENA = 0x44;
    /// HPCI-CPD553 ���b�`���͐M���G�b�W�I��L�o�C�g�|�[�g
    public const byte LATEDGL = 0x48;
    /// HPCI-CPD553 ���b�`���͐M���G�b�W�I��H�o�C�g�|�[�g
    public const byte LATEDGH = 0x4a;
    /// HPCI-CPD553 ���b�`�M���N���A�|�[�g
    public const byte LATSTS = 0x4c;
    /// HPCI-CPD553 �C�x���g�^�C�}�|�[�g
    public const byte EVENTTM = 0x50;
    /// HPCI-CPD553 ����o�͑I���|�[�g
    public const byte DO_SEL = 0x52;
    /// HPCI-CPD553 ������͑I���|�[�g
    public const byte DI_U = 0x54;
    /// HPCI-CPD553 �o�̓|�[�g1
    public const byte DO1_PRT = 0x62;
    /// HPCI-CPD553 �o�̓|�[�g2
    public const byte DO2_PRT = 0x63;
    /// HPCI-CPD553 DIO�r�b�g�I���|�[�g
    public const byte DIOBIT = 0x7F;
	/// ELS�ɐ��I��
	public const byte ELPOL = 0x80;     
    /// DLS/PCS�ؑ�
	public const byte DLS_PCS = 0x82;	
    /// CMP4��STA
	public const byte C4STA = 0x84;	    
    /// CMP5��STP
	public const byte C5STP = 0x86;	    
    /// HPCI-CPD508 BOLS/PCS�ؑ�
	public const byte BOL_PCS = 0x88;
    /// HPCI-CPD553 CMP�o�͑I���|�[�g
    public const byte COUT = 0x88;
    /// HPCI-CPD508 SVALM/DI/EMG�ؑ�
	public const byte INP_SEL = 0x8a;
    /// HPCI-CPD578 J3�o�̓}�X�N
	public const byte J3_OUT = 0x8a;
    /// HPCI-CPD553 �G���R�[�_�t�B���^OFF�I��ݒ�|�[�g
    public const byte ENCFIL = 0x8a;
    /// HPCI-CPD578 X-U��CMP�o�͐ؑ�
	public const byte COTSEL1 = 0x8c;	
    /// HPCI-CPD578 V-B��CMP�o�͐ؑ�
	public const byte COTSEL2 = 0x8e;
	/// HPCI-CPD578N �����o�̓}�X�N�|�[�g
	public const byte BINTM = 0x90;
    /// HPCI-CPD578N X,V�������pCMP����
	public const byte SYNC_C_EN = 0x94;
	/// HPCI-CPD553 �����v���r�b�g�I���|�[�g
	public const byte INTENA = 0x94;
    /// HPCI-CPD578N X�����pCMP�I��
	public const byte XSYNC_C = 0x96;
	/// HPCI-CPD553 ���͐M���G�b�W�I��L�o�C�g�|�[�g
	public const byte INTEDGL = 0x96;
    /// HPCI-CPD578N V�����pCMP�I��
	public const byte VSYNC_C = 0x98;
    /// HPCI-CPD553 ���͐M���G�b�W�I��H�o�C�g�|�[�g
	public const byte INTEDGH = 0x98;
    /// HPCI-CPD553 �����N���A�|�[�g
    public const byte INTCLR = 0x9a;	
    /// HPCI-CPD578N �G���R�[�_�t�B���^�ݒ�
	public const byte ENFIL = 0xa2;	    
    /// HPCI-CPD578N �O���p���T���o�͐ݒ�
	public const byte J3_SEL = 0xa4;	
    /// HPCI-CPD578N ��������
	public const byte AXIS_SEL = 0xa8;	
    /// HPCI-CPD578N X-U�������@�\�ݒ�
	public const byte SYNC_SET1 = 0xf0;	
    /// HPCI-CPD578N V-B�������@�\�ݒ�
	public const byte SYNC_SET2 = 0xf2;	

    /// �s���ȃp�����[�^
    public const uint ILLEGAL_PRM = 0x0100;
	/// �{�[�h�R�[�h���ُ�
    public const uint OTHER_BOARD = 0x0200;
	/// �s���ȃn���h��
    public const uint ILLEGAL_HANDLE = uint.MaxValue;

#if APP_SYNC
	static System.Object[] thisLock =new System.Object[16];
#endif

	/// <summary>
	/// ���W�X�^�Ǐo
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="byCmd">���W�X�^�Ǐo���R�}���h</param>
	/// <param name="unReg">���W�X�^����ǂݏo���ꂽ�f�[�^�i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
	public static uint hcp530_rReg(uint hDevID, ushort usAxis, byte byCmd, ref uint unReg)
	{
		uint unRet = 0;
#if APP_SYNC
		lock (thisLock[(hDevID & 0xff)-1])
		{
#endif
			unRet = Hicpd530.cp530_rReg(hDevID, usAxis, byCmd, ref unReg);

#if APP_SYNC
		}
#endif
		return (unRet);
	}

	/// <summary>
	/// ���W�X�^�����֐�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="byCmd">���W�X�^�������݃R�}���h</param>
	/// <param name="unReg">���W�X�^�֏������ރf�[�^</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
	public static uint hcp530_wReg(uint hDevID, ushort usAxis, byte byCmd, uint unReg)
	{
		uint unRet = 0;

#if APP_SYNC
		lock (thisLock[(hDevID & 0xff)-1])
		{
#endif
			unRet = Hicpd530.cp530_wReg(hDevID, usAxis, byCmd, unReg);

#if APP_SYNC
		}
#endif
		return (unRet);
	}

	/// <summary>
	/// ���o�̓o�b�t�@�Ǐo��
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="unReg">���o�̓o�b�t�@����ǂݏo���ꂽ�f�[�^�i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
	public static uint hcp530_rBufDW(uint hDevID, ushort usAxis, ref uint unReg)
	{
		uint unRet = 0;

#if APP_SYNC
		lock (thisLock[(hDevID & 0xff)-1])
		{
#endif
			unRet = Hicpd530.cp530_rBufDW(hDevID, usAxis, ref unReg);

#if APP_SYNC
		}
#endif
		return (unRet);
	}

	/// <summary>
	/// ���o�̓o�b�t�@������
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="unReg">���o�̓o�b�t�@�֏����ރf�[�^</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
	public static uint hcp530_wBufDW(uint hDevID, ushort usAxis, uint unReg)
	{
		uint unRet = 0;

#if APP_SYNC
		lock (thisLock[(hDevID & 0xff)-1])
		{
#endif
			unRet = Hicpd530.cp530_wBufDW(hDevID, usAxis, unReg);

#if APP_SYNC
		}
#endif
		return (unRet);
	}

	/// <summary>
	/// �I�v�V�����|�[�g�Ǐo��(�o�C�g)
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="byCmd">�ǂݏo���R�}���h</param>
	/// <param name="byData">�I�v�V�����|�[�g����ǂݏo���ꂽ�f�[�^�i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
	public static uint hcp530_rPortB(uint hDevID, byte byCmd, ref byte byData)
	{
		uint unRet = 0;

#if APP_SYNC
		lock (thisLock[(hDevID & 0xff)-1])
		{
#endif
			unRet = Hicpd530.cp530_rPortB(hDevID, byCmd, ref byData);

#if APP_SYNC
		}
#endif
		return (unRet);
	}

	/// <summary>
	/// �I�v�V�����|�[�g�Ǐo��(���[�h)
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="byCmd">�ǂݏo���R�}���h</param>
	/// <param name="usData">�I�v�V�����|�[�g����ǂݏo���ꂽ�f�[�^�i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
	public static uint hcp530_rPortW(uint hDevID, byte byCmd, ref ushort usData)
	{
		uint unRet = 0;

#if APP_SYNC
		lock (thisLock[(hDevID & 0xff)-1])
		{
#endif
			unRet = Hicpd530.cp530_rPortW(hDevID, byCmd, ref usData);

#if APP_SYNC
		}
#endif
		return (unRet);
	}
    
	/// <summary>
	/// �I�v�V�����|�[�g������(�o�C�g)
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="byCmd">�������݃R�}���h</param>
	/// <param name="byData">�I�v�V�����|�[�g�֏����ރf�[�^</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
	public static uint hcp530_wPortB(uint hDevID, byte byCmd, byte byData)
	{
		uint unRet = 0;

#if APP_SYNC
		lock (thisLock[(hDevID & 0xff)-1])
		{
#endif
			unRet = Hicpd530.cp530_wPortB(hDevID, byCmd, byData);

#if APP_SYNC
		}
#endif
		return (unRet);
	}

	/// <summary>
	/// �I�v�V�����|�[�g������(���[�h)
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="byCmd">�������݃R�}���h</param>
	/// <param name="usData">�I�v�V�����|�[�g�֏����ރf�[�^</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
	public static uint hcp530_wPortW(uint hDevID, byte byCmd, ushort usData)
	{
		uint unRet = 0;

#if APP_SYNC
		lock (thisLock[(hDevID & 0xff)-1])
		{
#endif
			unRet = Hicpd530.cp530_wPortW(hDevID, byCmd, usData);

#if APP_SYNC
		}
#endif
		return (unRet);
	}

	/// <summary>
	/// �{�[�h�����C�f�o�C�X���̎擾
	/// </summary>
	/// <param name="unCnt">�{�[�h�����̊i�[��</param>
	/// <param name="hpcInfo">�f�o�C�X���̊i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_GetDevInfo(ref uint unCnt, [In, Out, MarshalAs(UnmanagedType.LPArray)] Hicpd530.HPCDEVICEINFO[] hpcInfo)
    {
        uint cnt = 0,
            i,
            unRet = 0;

        Hicpd530.HPCDEVICEINFO[] h = new Hicpd530.HPCDEVICEINFO[16];

        unRet = Hicpd530.cp530_GetDeviceCount(ref cnt);				// �{�[�h�����擾
		if (unRet != 0)            return (unRet);

        // �{�[�h������0���܂���16���ȏ�̎��̓G���[
        if ((0 == cnt) || (16 <= cnt))
        {
            return (ILLEGAL_PRM);
        }

        unCnt = cnt;
        unRet = Hicpd530.cp530_GetDeviceInfo(ref cnt, h);	// �f�o�C�X���擾
		if (unRet != 0)            return (unRet);

        for (i = 0; i < cnt; i++)
        {
            hpcInfo[i] = h[i];
        }
        return (unRet);
    }

	/// <summary>
	/// �f�o�C�X�̃I�[�v���C�f�o�C�X�̏�����
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h���̊i�[��</param>
	/// <param name="hInfo">�f�o�C�X���̊i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_DevOpen(ref uint hDevID, ref Hicpd530.HPCDEVICEINFO hInfo)
    {
        uint h = ILLEGAL_HANDLE,
                unRet = 0;
        ushort usAx,
                usCode = 0,
                usSts = 0,
				// ---------------------------------
				// for CPD5016 (2010.07.29)
				// ---------------------------------
				// usMax = 8;
				usMax = 16;
        byte byAxis = 0;

        unRet = Hicpd530.cp530_OpenDevice(ref h, ref hInfo);
        if (unRet != 0)
        {
            return (unRet);
        }
        else
        {
            hDevID = h;
        }

#if APP_SYNC
		thisLock[(hDevID & 0xff)-1] = new System.Object();
#endif

        // �{�[�h����
        unRet = Hicpd530.cp530_GetBoardCode(h, ref usCode);

        // �I�v�V�����|�[�g�ݒ�
        switch (usCode)
        {
            case 0x578a:
                unRet |= hcp530_rPortB(h, AXIS_SEL, ref byAxis);        // �{�[�h�����ǂݏo��
                if (byAxis == 4)
                {
                    unRet |= hcp530_wPortB(h, ELPOL, 0x00);		        // ELS�ɐ��I���|�[�g����(B��)
                    unRet |= hcp530_wPortB(h, DLS_PCS, 0x0f);		    // DLS/PCS�ؑփ|�[�g����(PCS)
                    unRet |= hcp530_wPortB(h, C4STA, 0x00);		        // �R���p���[�^4�o�͐ؑփ|�[�g����(�o�͕s��)
                    unRet |= hcp530_wPortB(h, C5STP, 0x00);		        // �R���p���[�^5�o�͐ؑփ|�[�g����(�o�͕s��)  
                    unRet |= hcp530_wPortB(h, DLS_PCS, 0x0f);	        // DLS/PCS�ؑփ|�[�g����(PCS)
                    unRet |= hcp530_wPortB(h, COTSEL1, 0xff);	        // X-U��CMP�o�͐ؑփ|�[�g(�o�͕s��)
                    unRet |= Hicpd530.cp530_wCmdW(h, X_AX, SRST);	    // X-U���\�t�g�E�F�A���Z�b�g
                    usMax = 4;
                }
                else if (byAxis == 8)
                {
                    unRet |= hcp530_wPortB(h, ELPOL, 0x00);		        // ELS�ɐ��I���|�[�g����(B��)
                    unRet |= hcp530_wPortB(h, DLS_PCS, 0xff);		    // DLS/PCS�ؑփ|�[�g����(PCS)
                    unRet |= hcp530_wPortB(h, C4STA, 0x00);		        // �R���p���[�^4�o�͐ؑփ|�[�g����(�o�͕s��)
                    unRet |= hcp530_wPortB(h, C5STP, 0x00);		        // �R���p���[�^5�o�͐ؑփ|�[�g����(�o�͕s��)
                    unRet |= hcp530_wPortB(h, DLS_PCS, 0xff);		    // DLS/PCS�ؑփ|�[�g����(PCS)
                    unRet |= hcp530_wPortB(h, COTSEL1, 0xff);		    // X-U��CMP�o�͐ؑփ|�[�g(�o�͕s��)
                    unRet |= hcp530_wPortB(h, COTSEL2, 0xff);		    // V-B��CMP�o�͐ؑփ|�[�g(�o�͕s��)
                    unRet |= Hicpd530.cp530_wCmdW(h, X_AX, SRST);	    // X-U���\�t�g�E�F�A���Z�b�g
                    unRet |= Hicpd530.cp530_wCmdW(h, V_AX, SRST);	    // V-B���\�t�g�E�F�A���Z�b�g
                    usMax = 8;
                }
                else
                {
                    unRet |= hcp530_wPortB(h, ELPOL, 0x00);		        // ELS�ɐ��I���|�[�g����(B��)
                    unRet |= hcp530_wPortB(h, DLS_PCS, 0xff);		    // DLS/PCS�ؑփ|�[�g����(PCS)
                    unRet |= hcp530_wPortB(h, C4STA, 0x00);		        // �R���p���[�^4�o�͐ؑփ|�[�g����(�o�͕s��)
                    unRet |= hcp530_wPortB(h, C5STP, 0x00);		        // �R���p���[�^5�o�͐ؑփ|�[�g����(�o�͕s��)
                    unRet |= hcp530_wPortB(h, J3_OUT, 0x00);		    // J3�o�̓}�X�N�|�[�g(�o�̓}�X�N)
                    unRet |= hcp530_wPortB(h, COTSEL1, 0xff);		    // X-U��CMP�o�͐ؑփ|�[�g(�o�͕s��)
                    unRet |= hcp530_wPortB(h, COTSEL2, 0xff);		    // V-B��CMP�o�͐ؑփ|�[�g(�o�͕s��)
                    unRet |= Hicpd530.cp530_wCmdW(h, X_AX, SRST);	    // X-U���\�t�g�E�F�A���Z�b�g
                    unRet |= Hicpd530.cp530_wCmdW(h, V_AX, SRST);	    // V-B���\�t�g�E�F�A���Z�b�g
                    usMax = 8;
                }
                break;
            case 0x5254:
                for (usAx = X_AX; usAx < (B_AX + 1); usAx++)
                {
                    unRet |= Hicpd530.cp530_rMstsW(h, usAx, ref usSts);
                    if (usSts == 0xffff)
                    {
                        usMax = usAx;
                        break;
                    }
                }
                if (usMax == 2)
                {							// HPCI-CPD532
                    unRet = hcp530_wPortB(h, ELPOL, 0x00);		        // ELS�ɐ��I���|�[�g����(B��)
                    unRet |= hcp530_wPortB(h, DLS_PCS, 0x03);		    // DLS/PCS�ؑփ|�[�g����(PCS)
                    unRet |= hcp530_wPortB(h, C4STA, 0x00);		        // �R���p���[�^4�o�͐ؑփ|�[�g����(�o�͕s��)
                    unRet |= hcp530_wPortB(h, C5STP, 0x00);		        // �R���p���[�^5�o�͐ؑփ|�[�g����(�o�͕s��)
                    unRet |= Hicpd530.cp530_wCmdW(h, X_AX, SRST);	    // X-Y���\�t�g�E�F�A���Z�b�g
                }
                else if (usMax == 4)
                {							// HPCI-CPD534
                    unRet = hcp530_wPortB(h, ELPOL, 0x00);		        // ELS�ɐ��I���|�[�g����(B��)
                    unRet |= hcp530_wPortB(h, DLS_PCS, 0x0f);		    // DLS/PCS�ؑփ|�[�g����(PCS)
                    unRet |= hcp530_wPortB(h, C4STA, 0x00);		        // �R���p���[�^4�o�͐ؑփ|�[�g����(�o�͕s��)
                    unRet |= hcp530_wPortB(h, C5STP, 0x00);		        // �R���p���[�^5�o�͐ؑփ|�[�g����(�o�͕s��)
                    unRet |= Hicpd530.cp530_wCmdW(h, X_AX, SRST);	    // X-U���\�t�g�E�F�A���Z�b�g
                }
                else if (usMax == 8)
                {							// HPCI-CPD508
                    unRet = hcp530_wPortB(h, ELPOL, 0x00);		        // ELS�ɐ��I���|�[�g����(B��)
                    unRet |= hcp530_wPortB(h, C4STA, 0x00);		        // �R���p���[�^4�o�͐ؑփ|�[�g����(�o�͕s��)
                    unRet |= hcp530_wPortB(h, BOL_PCS, 0x00);		    // BOLS/PCS�ؑփ|�[�g����(BOLS)
                    unRet |= hcp530_wPortB(h, INP_SEL, 0x00);		    // SVALM/DI/EMG�ؑփ|�[�g����(SVALM)
                    unRet |= Hicpd530.cp530_wCmdW(h, X_AX, SRST);	    // X-U���\�t�g�E�F�A���Z�b�g
                    unRet |= Hicpd530.cp530_wCmdW(h, V_AX, SRST);	    // V-B���\�t�g�E�F�A���Z�b�g
                }
                break;
            case 0x5016:					// HPCI-CPD5016
			    unRet = hcp530_wPortW(h, ELPOL, 0x0000);			    // ELS�ɐ��I���|�[�g����(B��)
			    unRet |= hcp530_wPortW(h, C4STA, 0x0000);			    // �R���p���[�^4�o�͐ؑփ|�[�g����(�o�͕s��)
			    unRet |= hcp530_wPortW(h, BOL_PCS, 0x0000);			    // BOLS/PCS�ؑփ|�[�g����(BOLS)
			    unRet |= hcp530_wPortW(h, INP_SEL, 0x0000);		        // SVALM/DI/EMG�ؑփ|�[�g����(SVALM)
                unRet |= Hicpd530.cp530_wCmdW(h, X1_AX, SRST);	        // X1-U1���\�t�g�E�F�A���Z�b�g
                unRet |= Hicpd530.cp530_wCmdW(h, X2_AX, SRST);	        // X2-U2���\�t�g�E�F�A���Z�b�g
                unRet |= Hicpd530.cp530_wCmdW(h, X3_AX, SRST);	        // X3-U3���\�t�g�E�F�A���Z�b�g
                unRet |= Hicpd530.cp530_wCmdW(h, X4_AX, SRST);	        // X4-U4���\�t�g�E�F�A���Z�b�g
                usMax = 16;
                break;
            case 0x5530:					// HPCI-CPD553
                unRet = hcp530_wPortB(h, DIGIFIL, 0x00);		// �f�W�^���t�B���^���Ԑݒ�|�[�g(����)
                unRet |= hcp530_wPortB(h, DIGISEL, 0x00);		// �f�W�^���t�B���^�L���I���|�[�g(����)
                unRet |= hcp530_wPortW(h, LATENA, 0x0000);		// ���b�`�r�b�g�I���|�[�g(����)
                unRet |= hcp530_wPortW(h, LATEDGL, 0x0000);		// ���b�`���͐M���G�b�W�I��L�o�C�g�|�[�g(�����b�W)
                unRet |= hcp530_wPortW(h, LATEDGH, 0x0000);		// ���b�`���͐M���G�b�W�I��H�o�C�g�|�[�g(�����b�W)
                unRet |= hcp530_wPortW(h, LATSTS, 0x0000);		// ���b�`�M���N���A�|�[�g
                unRet |= hcp530_wPortB(h, EVENTTM, 0x00);		// �C�x���g�^�C�}�|�[�g(����)
                unRet |= hcp530_wPortW(h, DO_SEL, 0x0000);		// ����o�͑I���|�[�g(�ʏ�DO)
                unRet |= hcp530_wPortW(h, DI_U, 0x0000);		// ������͑I���|�[�g(IN��U���G���R�[�_A���Ƃ��Ďg�p)
                unRet |= hcp530_wPortB(h, DO1_PRT, 0x00);		// �o�̓|�[�g1
                unRet |= hcp530_wPortB(h, DO2_PRT, 0x00);		// �o�̓|�[�g2
                unRet |= hcp530_wPortB(h, DIOBIT, 0x00);		// DIO�r�b�g�I���|�[�g(16IN/8OUT)
                unRet |= hcp530_wPortB(h, ELPOL, 0x00);			// ELS�ɐ��I���|�[�g����(B��)
                unRet |= hcp530_wPortB(h, DLS_PCS, 0x0f);		// DLS/PCS�ؑփ|�[�g����(PCS)
                unRet |= hcp530_wPortB(h, C4STA, 0x00);			// �R���p���[�^4�o�͐ؑփ|�[�g����(�o�͕s��)
                unRet |= hcp530_wPortB(h, C5STP, 0x00);			// �R���p���[�^5�o�͐ؑփ|�[�g����(�o�͕s��)
                unRet |= hcp530_wPortW(h, COUT, 0xffff);		// CMP�o�͑I���|�[�g����(�o�͕s��)
                unRet |= hcp530_wPortW(h, ENCFIL, 0x0000);		// �G���R�[�_�t�B���^OFF�I��ݒ�|�[�g(FILTER ON)
                unRet |= hcp530_wPortW(h, BINTM, 0x00);			// �����C�l�[�u���|�[�g����(�o�͕s��)
                unRet |= hcp530_wPortW(h, INTENA, 0x0000);		// �����v���r�b�g�I���|�[�g(����)
                unRet |= hcp530_wPortW(h, INTEDGL, 0x0000);		// ���͐M���G�b�W�I��L�o�C�g�|�[�g(�����b�W)
                unRet |= hcp530_wPortW(h, INTEDGH, 0x0000);		// ���͐M���G�b�W�I��H�o�C�g�|�[�g(�����b�W)
                unRet |= hcp530_wPortW(h, INTCLR, 0x0000);		// �����N���A�|�[�g
                unRet |= Hicpd530.cp530_wCmdW(h, X_AX, SRST);	// X-Z���\�t�g�E�F�A���Z�b�g
                usMax = 3;
				break;
            default:
                return OTHER_BOARD;
        }

        // ���W�X�^������
        for (usAx = X1_AX; usAx < usMax; usAx++)	// for CPD5016 (2010.07.29)
        {	
			// ���W�X�^����(�x�[�X���x���W�X�^:PRFL = 200)
            unRet |= hcp530_wReg(h, usAx, WPRFL, 200);

            // ���W�X�^����(���쑬�x���W�X�^:PRFH = 2000)
            unRet |= hcp530_wReg(h, usAx, WPRFH, 2000);

            // ���W�X�^����(�����������������[�g���W�X�^)
            // 		��N���b�N���g�� = 19,660,800Hz, �������� = �������� = 0.5�b
            // 		PRFH = 2000, PRFL = 200, PRUR:�������[�g
            // 		��������(�b) = (PRFH - PRFL) * (PRUR + 1) * 4 / 19660800
            // 		PRUR = 1364
            unRet |= hcp530_wReg(h, usAx, WPRUR, 1364);

            // ���W�X�^����(�{�� = 1�{, PRMG = 299)
            unRet |= hcp530_wReg(h, usAx, WPRMG, 299);

            // ���W�X�^����(���샂�[�h:PRMD = 0x8008000)
            unRet |= hcp530_wReg(h, usAx, WPRMD, 0x8008000);

            // ���W�X�^����(�ړ��ʕ␳���x:RFA = 200)
            unRet |= hcp530_wReg(h, usAx, WRFA, 200);

            // ���W�X�^����(���ݒ�1:RENV1 = 0x20434004)
            // 		�w�ߏo��:�ʃp���X�w�߂̐��_��
            // 		OLS & DLS & SVALM = B��, INPOS = A��, 
            // 		ELS & SVALM���͎�����~,DLS���o������ = �����̂�,���b�`���Ȃ�,
            // 		���޴װ����_���A���������ر�o�͂��Ȃ�
            unRet |= hcp530_wReg(h, usAx, WRENV1, 0x20434004);

            // 	�ėp���o�͐ݒ蓙
            if (usCode == 0x578a)
            {	// HPCI-CPD578
                // ���W�X�^����(���ݒ�2:RENV2 = 0x0020fd55)
                unRet |= hcp530_wReg(h, usAx, WRENV2, 0x0020fd55);
            }
            else if (usCode == 0x5254)
            {	// HPCI-CPD534,532,508
                // ���W�X�^����(���ݒ�2:RENV2 = 0x0020f555)
                unRet |= hcp530_wReg(h, usAx, WRENV2, 0x0020f555);
            }
            else if (usCode == 0x5016)
            {	// HPCI-CPD5016
                // ���W�X�^����(���ݒ�2:RENV2 = 0x0020f555)
                unRet |= hcp530_wReg(h, usAx, WRENV2, 0x0020f555);
            }
            else if (usCode == 0x5530)
            {	// HPCI-CPD553
                // ���W�X�^����(���ݒ�2:RENV2 = 0x0020f555)
                unRet |= hcp530_wReg(h, usAx, WRENV2, 0x0020f555);
            }

            // ���W�X�^����(���ݒ�3:RENV3 = 0x00f00002)
            // 		���_���A�F���_�Z���T+Z�����_���A, 
            // 		CLR����OFF��ON & ���_���A�������J�E���^�N���A
            unRet |= hcp530_wReg(h, usAx, WRENV3, 0x00f00002);

            // ���W�X�^����(�C�x���g�ݒ�:RIRQ = 1(�����~)
            unRet |= Cp530l1a.hcp530_wReg(h, usAx, WRIRQ, 1);

            // SVON OFF
            unRet |= Hicpd530.cp530_wCmdW(h, usAx, 0x10);

            // SVRST OFF
            unRet |= Hicpd530.cp530_wCmdW(h, usAx, 0x11);

            // ����~
            unRet |= Hicpd530.cp530_wCmdW(h, usAx, 0x49);
        }
        return (unRet);
    }

	/// <summary>
	/// �f�o�C�X�̃N���[�Y
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_DevClose(uint hDevID)
    {
        uint unRet = 0;

        unRet = Hicpd530.cp530_CloseDevice(hDevID);
        return (unRet);
    }

	/// <summary>
	/// ���_���A�����̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="usMode">���_���A���[�h</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SetOrgMode(uint hDevID, ushort usAxis, ushort usMode)
    {
        uint unRet = 0;
        uint unRenv3 = 0;
        const uint CTR2_SRC = 0x00000300;
        const uint ORG_MODE = 0x0000000F;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`15�ȊO�܂��� 
        // ���_���A���[�h��0�`12�ȊO�̎��͈����G���[ 
//      if ((usAxis < 0) || (7 < usAxis) || 
		if ((usAxis < 0) || (15 < usAxis ) ||
            (usMode < 0) || (12 < usMode))
        {
            return (ILLEGAL_PRM);
        }

        // �����W�X�^3(RENV3)�Ǎ��� 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV3, ref unRenv3);
		if (unRet != 0)            return (unRet);

        // Orgmode9�̎��CCTR2���͂��w�߈ʒu�ɂ��� 
        // (Orgmode9�ȊO�̎��CCTR2���͂͋@�B�ʒu����) 
        if (usMode == 9)
        {
            unRenv3 &= (~CTR2_SRC);
            unRenv3 |= 0x00000100;
        }
        unRenv3 &= (~ORG_MODE);
        unRenv3 |= usMode;

        // �����W�X�^3(RENV3)������ 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV3, unRenv3);
        return (unRet);
    }

	/// <summary>
	/// �\�t�g���~�b�g�̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="nPsl">+SLS</param>
	/// <param name="nMsl">-SLS</param>
	/// <param name="usEnable">SLS�g�p/�s�g�p</param>
	/// <param name="usStp">��~���@</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SetSls(uint hDevID, ushort usAxis, int nPsl, int nMsl, ushort usEnable, ushort usStp)
    {
        uint unRet = 0;
        uint unRenv4 = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`7�ȊO 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// ���ԍ���0�`15�ȊO 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

		// �\�t�g���~�b�g�g�p����+SLS < -SLS�̎��͈����G���[ 
		if ((usEnable != 0) && (nPsl <= nMsl))	return (ILLEGAL_PRM);

        // �����W�X�^4(RENV4)�Ǎ��� 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV4, ref unRenv4);
        if (unRet != 0)            return (unRet);

		switch (usEnable)
		{
			case 0:
				// SLS�s�g�p 
				unRenv4 &= 0xffffe3e3;
				break;
			case 1:
				// SLS�g�p 
				unRenv4 &= 0xffff0000;
				if (usStp == 0)			unRenv4 |= 0x00003838;		// ����~
				else if (usStp == 1)	unRenv4 |= 0x00005858;		// ������~
				else					return (ILLEGAL_PRM);
				break;
			default:
				return (ILLEGAL_PRM);
		}

        // +SLS�f�[�^�ݒ�(RCMP1)  
        unRet = hcp530_wReg(hDevID, usAxis, WRCMP1, (uint)nPsl);
		if (unRet != 0)            return (unRet);

        // -SLS�f�[�^�ݒ�(RCMP2) 
        unRet = hcp530_wReg(hDevID, usAxis, WRCMP2, (uint)nMsl);
		if (unRet != 0)            return (unRet);

		// �����W�X�^4(RENV4)������ 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV4, unRenv4);
        return (unRet);
    }

	/// <summary>
	/// ELS�̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="usPol">�ɐ�</param>
	/// <param name="usStp">��~���@</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SetEls(uint hDevID, ushort usAxis, ushort usPol, ushort usStp)
    {
		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
//		byte   byData = 0;
		ushort byData = 0;
		uint unRet = 0;
        uint unRenv1 = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`7�ȊO�̎��͈����G���[ 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// ���ԍ���0�`15�ȊO�̎��͈����G���[ 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ELS�ɐ��|�[�g�Ǎ��� 
//		unRet = hcp530_rPortB(hDevID, ELPOL, ref byData);
		unRet = hcp530_rPortW(hDevID, ELPOL, ref byData);
		if (unRet != 0)            return (unRet);

        // �����W�X�^�P(RENV1)�Ǎ��� 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV1, ref unRenv1);
		if (unRet != 0)            return (unRet);

		switch(usPol)
		{
			// ---------------------------------
			// for CPD5016 (2010.07.29)
			// ---------------------------------
//			case 0:	byData &= (byte)~(0x01 << usAxis);	break;	// B��
//			case 1:	byData |= (byte) (0x01 << usAxis);	break;	// A��
			case 0:	byData &= (ushort)~(0x01 << usAxis);	break;	// B��
			case 1:	byData |= (ushort) (0x01 << usAxis);	break;	// A��
			default:return (ILLEGAL_PRM);
		}

		switch(usStp)
		{
			case 0: unRenv1 &= (uint)(~R1_ELSM_BIT);	break;	// ����~
			case 1: unRenv1 |= (uint)R1_ELSM_BIT;		break;  // ������~
			default:return (ILLEGAL_PRM);
		}		

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ELS�ɐ��|�[�g������
//		unRet = hcp530_wPortB(hDevID, ELPOL, byData);
		unRet = hcp530_wPortW(hDevID, ELPOL, byData);
		if (unRet != 0)				return (unRet);

        // �����W�X�^�P(RENV1)������ 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV1, unRenv1);
        return (unRet);
    }

	/// <summary>
	/// OLS�̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="usPol">�ɐ�</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>			
    public static uint hcp530_SetOls(uint hDevID, ushort usAxis, ushort usPol)
    {
        uint unRet = 0;
        uint unRenv1 = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`7�ȊO�̎��͈����G���[ 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// ���ԍ���0�`15�ȊO�̎��͈����G���[ 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // �����W�X�^�P(RENV1)�Ǎ��� 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV1, ref unRenv1);
		if (unRet != 0)				return (unRet);
		switch(usPol)
		{
			case 0:unRenv1 &= (~R1_OLSL_BIT);	break;	// B��
			case 1:unRenv1 |= R1_OLSL_BIT;		break;	// A��
            default:							return (ILLEGAL_PRM);
        }

        // �����W�X�^�P(RENV1)������ 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV1, unRenv1);
        return (unRet);
    }

	/// <summary>
	/// SVALM�̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="usPol">�ɐ�</param>
	/// <param name="usStp">��~���@</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>	
    public static uint hcp530_SetSvAlm(uint hDevID, ushort usAxis, ushort usPol, ushort usStp)
    {
        uint unRet = 0;
        uint unRenv1 = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`7�ȊO�̎��͈����G���[ 
//      if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// ���ԍ���0�`15�ȊO�̎��͈����G���[ 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // �����W�X�^�P(RENV1)�Ǎ��� 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV1, ref unRenv1);
		if (unRet != 0)				return (unRet);
		switch(usPol)
		{
			case 0:	unRenv1 &= (~R1_ALML_BIT);	break;	// B��
			case 1:	unRenv1 |= R1_ALML_BIT;		break;	// A��
			default:							return (ILLEGAL_PRM);
		}
		switch(usStp)
		{
			case 0: unRenv1 &= (~R1_ALMM_BIT);	break;	// ����~
			case 1: unRenv1 |= R1_ALMM_BIT;		break;  // ������~
			default:							return (ILLEGAL_PRM);
		}		
        // �����W�X�^�P(RENV1)������ 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV1, unRenv1);
        return (unRet);
    }

 	/// <summary>
 	/// INPOS�̐ݒ�
 	/// </summary>
 	/// <param name="hDevID">�f�o�C�X�n���h��</param>
 	/// <param name="usAxis">���ԍ�</param>
 	/// <param name="usEnable">�g�p/�s�g�p</param>
 	/// <param name="usPol">�ɐ�</param>
 	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SetInpos(uint hDevID, ushort usAxis, ushort usEnable, ushort usPol)
    {
        uint unRet = 0;
        uint unRmd = 0;
        uint unRenv1 = 0;

		// ---------------------------------
		// for CPD5016 (2011.02.16)
		// ---------------------------------
		// ���ԍ���0�`7�ȊO�̎��͈����G���[ 
		//      if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// ���ԍ���0�`15�ȊO�̎��͈����G���[ 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // ���샂�[�h(PRMD)�Ǎ��� 
        unRet = hcp530_rReg(hDevID, usAxis, RPRMD, ref unRmd);
		if (unRet != 0)            return (unRet);

        // �����W�X�^�P(RENV1)�Ǎ��� 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV1, ref unRenv1);
		if (unRet != 0)            return (unRet);

        // INPOS����L��/����
		switch (usEnable)
		{
			case 0:unRmd &= (~MD_INPE_BIT);	break;		// �s�g�p
			case 1:unRmd |= MD_INPE_BIT;	break;		// �g�p
			default:						return (ILLEGAL_PRM);
		}

        // INPOS���͋ɐ�
		switch (usPol)
		{
			case 0:unRenv1 &= (~R1_INPL_BIT);	break;	// B��
			case 1:unRenv1 |= R1_INPL_BIT;		break;	// A��
			default:							return (ILLEGAL_PRM);
		}

        // ���샂�[�h(PRMD)������ 
        unRet = hcp530_wReg(hDevID, usAxis, WPRMD, unRmd);
		if (unRet != 0)            return (unRet);
		// �����W�X�^�P(RENV1)������ 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV1, unRenv1);
        return (unRet);
    }

	/// <summary>
	/// �G���R�[�_Z���̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="usCnt">���_���A�Ŏg�p����Z����-1</param>
	/// <param name="usPol">�ɐ�</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SetEz(uint hDevID, ushort usAxis, ushort usCnt, ushort usPol)
    {
        uint unRet = 0;
        uint unRenv2 = 0;
        uint unRenv3 = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`7�ȊO�̎��͈����G���[ 
        // ���_���A�Ŏg�p����y���񐔂�16��܂łȂ̂�usCnt��15�܂�
//      if ((usAxis < 0) || (7 < usAxis) || (15 < usCnt))
//      {
//          return (ILLEGAL_PRM);
//      }
		// ���ԍ���0�`7�ȊO�̎��͈����G���[ 
		// ���_���A�Ŏg�p����y���񐔂�16��܂łȂ̂�usCnt��15�܂�
		if ((usAxis < 0) || (15 < usAxis) || (15 < usCnt))
		{
			return (ILLEGAL_PRM);
		}

        // �����W�X�^2(RENV2)�Ǎ��� 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV2, ref unRenv2);
		if (unRet != 0)            return (unRet);

        // �����W�X�^3(RENV3)�Ǎ��� 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV3, ref unRenv3);
		if (unRet != 0)            return (unRet);

        // �ɐ��I�� 
 		switch (usPol)
		{
			// ---------------------------------
			// for CPD5016 (2010.08.31)
			// ---------------------------------
//			case 0:unRenv2 |= R2_EZL_BIT;		break;	// B��
//			case 1:unRenv2 &= (~R2_EZL_BIT);	break;	// A��
			case 1:unRenv2 |= R2_EZL_BIT;		break;	// B��
			case 0:unRenv2 &= (~R2_EZL_BIT);	break;	// A��
			default:							return (ILLEGAL_PRM);
            
        }

        // ���_���A�Ŏg�p����y����
        unRenv3 = (unRenv3 & (uint)0xffffff0f) | (uint)(usCnt << 4);

        // �����W�X�^2(RENV2)������ 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV2, unRenv2);
		if (unRet != 0)            return (unRet);

        // �����W�X�^3(RENV3)������ 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV3, unRenv3);
        return (unRet);
    }

	/// <summary>
	/// �T�[�{�G���[�J�E���^�N���A�o�͂̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="usUse">�g�p���@</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SetSvCtrCl(uint hDevID, ushort usAxis, ushort usUse)
    {
        uint unRet = 0;
        uint unRenv1 = 0;

		// ���ԍ���0�`7�ȊO�̎��͈����G���[ 
		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);

        // �����W�X�^�P(RENV1)�Ǎ��� 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV1, ref unRenv1);
        if (unRet != 0) return (unRet);

        unRenv1 &= (~(R1_CLRE_BIT | R1_CLRO_BIT));
        switch (usUse)
        {
            case 0:												break;	// �s�g�p
            case 1:  unRenv1 |= R1_CLRO_BIT;					break;	// ���_���A������
            case 2:  unRenv1 |= R1_CLRE_BIT;					break;	// �ُ��~��
            case 3:  unRenv1 |= (R1_CLRE_BIT | R1_CLRO_BIT);	break;  // ���_���A�����y�шُ��~�� 
			default: return (ILLEGAL_PRM);    
        }
        // �����W�X�^�P(RENV1)������ 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV1, unRenv1);
        return (unRet);
    }

	/// <summary>
	/// �w�߃p���X�̏o�͌`���̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="usCmdPls">�w�߃p���X�̏o�͌`��(�ʃp���X/���ʃp���X)</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SetCmdPulse(uint hDevID, ushort usAxis, ushort usCmdPls)
    {
        uint unRet = 0;
        uint unRenv1 = 0;
        const uint PMD = 0x00000007;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`7�ȊO �܂��� 
// 		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// ���ԍ���0�`15�ȊO 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // �����W�X�^�P(RENV1)�Ǎ��� 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV1, ref unRenv1);
		if (unRet != 0) return (unRet);

        unRenv1 &= (~PMD);
        switch (usCmdPls)
        {
            case 0: unRenv1 |= (uint)0x04;	break;	// �� 
            case 1: unRenv1 |= (uint)0x02;	break;	// ���� 
            case 2: unRenv1 |= (uint)0x07;	break;
            case 3: unRenv1 |= (uint)0x00;	break;
			default:						return (ILLEGAL_PRM);
        }

        // �����W�X�^�P(RENV1)������ 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV1, unRenv1);
        return (unRet);
    }

	/// <summary>
	/// �������`���ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="usAcc">�������`��(����/S��)</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SetAccProfile(uint hDevID, ushort usAxis, ushort usAcc)
    {
        uint unRet = 0;
        uint unRmd = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`7�ȊO�̎��͈����G���[ 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// ���ԍ���0�`15�ȊO�̎��͈����G���[ 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);
		
		// ���샂�[�h(PRMD)�Ǎ��� 
        unRet = hcp530_rReg(hDevID, usAxis, RPRMD, ref unRmd);
        if (unRet != 0)			return (unRet);
        
		switch (usAcc)
		{
			case 0:unRmd &= (~MD_ACC_BIT);	break;		// ����������
			case 1:unRmd |= MD_ACC_BIT;		break;		// S��������
			default:						return (ILLEGAL_PRM);
		}

        // ���샂�[�h(PRMD)������
        unRet = hcp530_wReg(hDevID, usAxis, WPRMD, unRmd);
        return (unRet);
    }

	/// <summary>
	/// �����J�n�_�̐ݒ����
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="usSdp">�����J�n�_�̐ݒ����(����/�}�j���A��)</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SetAutoDec(uint hDevID, ushort usAxis, ushort usSdp)
    {
        uint unRet = 0;
        uint unRmd = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`7�ȊO�̎��͈����G���[ 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// ���ԍ���0�`15�ȊO�̎��͈����G���[ 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);
		
		// ���샂�[�h(PRMD)�Ǎ��� 
        unRet = hcp530_rReg(hDevID, usAxis, RPRMD, ref unRmd);
		if (unRet != 0)			return (unRet);

		switch(usSdp)
		{
			case 0:unRmd &= (~MD_SDP_BIT);	break;	// ����
			case 1:unRmd |= MD_SDP_BIT;		break;	// �}�j���A��
			default:						return (ILLEGAL_PRM);
		}

        // ���샂�[�h(PRMD)������
        unRet = hcp530_wReg(hDevID, usAxis, WPRMD, unRmd);
        return (unRet);
    }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="usEnable">DLS/PCS�g�p�؂�ւ�</param>
	/// <param name="usPol">�ɐ�</param>
	/// <param name="usMot">DLS���͎��̓���</param>
	/// <param name="usLtc">���b�`����/���Ȃ�</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SetDlsSel(uint hDevID, ushort usAxis, ushort usEnable, ushort usPol, ushort usMot, ushort usLtc)
    {
        byte byData = 0;
        uint unRet = 0;
        uint unRmd = 0;
        uint unRenv1 = 0;

		// ���ԍ���0�`7�ȊO�̎��͈����G���[ 
		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);

        // DLS/PCS�ؑփ|�[�g�Ǎ�
        unRet = hcp530_rPortB(hDevID, DLS_PCS, ref byData);
        if (unRet != 0)            return (unRet);

        // ���샂�[�h(PRMD)�Ǎ�
        unRet = hcp530_rReg(hDevID, usAxis, RPRMD, ref unRmd);
		if (unRet != 0)            return (unRet);

        // �����W�X�^1(RENV1)�Ǎ���
        unRet = hcp530_rReg(hDevID, usAxis, RRENV1, ref unRenv1);
		if (unRet != 0)            return (unRet);

        // DLS/PCS�g�p�I��
        switch (usEnable)
        {
            case 0:         // DLS����
				byData &= (byte)(~(0x01 << usAxis));
				unRmd |= MD_DLSE_BIT;		// DLS�g�p
                unRmd &= (~MD_PCSE_BIT);	// PCS�s�g�p
                unRenv1 &= (~R1_PCSL_BIT);  // PCS B��
				// DLS���͋ɐ�
                if (usPol == 0)			unRenv1 &= (~R1_DLSL_BIT);	// DLS B��
                else if (usPol == 1)	unRenv1 |= R1_DLSL_BIT;		// DLS A��
                else					return (ILLEGAL_PRM);
                break;
            case 1:         // PCS����
				byData |= (byte)(0x01 << usAxis);	// PCS�g�p
				unRmd &= (~MD_DLSE_BIT);		// DLS�s�g�p
                unRenv1 &= (~R1_DLSL_BIT);		// DLS B��
				// PCS���͋ɐ�
				if (usPol == 0)			unRenv1 &= (~R1_PCSL_BIT);	// PCS B��
                else if (usPol == 1)	unRenv1 |= R1_PCSL_BIT;		// PCS A��
                else					return (ILLEGAL_PRM);
                break;
            case 2:         // DLS, PCS�Ƃ��s�g�p
				byData &= (byte)(~(0x01 << usAxis));
				unRmd &= (~MD_DLSE_BIT);		// DLS�s�g�p
                unRmd &= (~MD_PCSE_BIT);		// PCS�s�g�p
                unRenv1 |= R1_DLSL_BIT;			// DLS A��
                unRenv1 &= (~R1_PCSL_BIT);		// PCS B��
                break;
			default:
										return (ILLEGAL_PRM);
        }

        // DLS���͎��̓���I��
        if (usMot == 0)			unRenv1 &= (~R1_DLSM_BIT);	// �����̂�
        else if (usMot == 1)	unRenv1 |= R1_DLSM_BIT;		// ������~
        else					return (ILLEGAL_PRM);

        // DLS���̓��b�`�I��
        if (usLtc == 0)			unRenv1 &= (~R1_DLLT_BIT);	// ���b�`���Ȃ�
        else if (usLtc == 1)	unRenv1 |= R1_DLLT_BIT;		// ���b�`����
        else					return (ILLEGAL_PRM);

        // �����W�X�^�P(RENV1)������
        unRet = hcp530_wReg(hDevID, usAxis, WRENV1, unRenv1);
		if (unRet != 0)            return (unRet);

        // ���샂�[�h(PRMD)������
        unRet = hcp530_wReg(hDevID, usAxis, WPRMD, unRmd);
		if (unRet != 0)            return (unRet);
		
		// DLS/PCS�ؑփ|�[�g������
        unRet = hcp530_wPortB(hDevID, DLS_PCS, byData);
        return (unRet);
    }

	/// <summary>
	/// ���C���X�e�[�^�X�Ǎ���
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="usSts">���C���X�e�[�^�X�i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_ReadMainSts(uint hDevID, ushort usAxis, ref ushort usSts)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`7�ȊO�̎��͈����G���[ 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// ���ԍ���0�`15�ȊO�̎��͈����G���[ 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // ���C���X�e�[�^�X�Ǎ���
        unRet = Hicpd530.cp530_rMstsW(hDevID, usAxis, ref usSts);
        return (unRet);
    }

	/// <summary>
	/// �G���[�X�e�[�^�X�Ǎ���
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="unSts">�G���[�X�e�[�^�X�i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_ReadErrorSts(uint hDevID, ushort usAxis, ref uint unSts)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`7�ȊO�̎��͈����G���[ 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// ���ԍ���0�`15�ȊO�̎��͈����G���[ 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);
		
		// �G���[�X�e�[�^�X�Ǎ���
        unRet = hcp530_rReg(hDevID, usAxis, 0xf2, ref unSts);
        return (unRet);
    }

	/// <summary>
	/// �C�x���g�X�e�[�^�X�Ǎ���
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="unSts">�C�x���g�X�e�[�^�X�i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_ReadEventSts(uint hDevID, ushort usAxis, ref uint unSts)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`7�ȊO�̎��͈����G���[ 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// ���ԍ���0�`15�ȊO�̎��͈����G���[ 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // �C�x���g�X�e�[�^�X�Ǎ���
        unRet = hcp530_rReg(hDevID, usAxis, 0xf3, ref unSts);
        return (unRet);
    }

	/// <summary>
	/// �T�u�X�e�[�^�X�Ǎ���
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="usSts">�T�u�X�e�[�^�X�i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_ReadSubSts(uint hDevID, ushort usAxis, ref ushort usSts)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`7�ȊO�̎��͈����G���[ 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// ���ԍ���0�`15�ȊO�̎��͈����G���[ 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // �T�u�X�e�[�^�X�Ǎ���
        unRet = Hicpd530.cp530_rSstsW(hDevID, usAxis, ref usSts);
        return (unRet);
    }

	/// <summary>
	/// �g���X�e�[�^�X�Ǎ���
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="unSts">�g���X�e�[�^�X�i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_ReadExSts(uint hDevID, ushort usAxis, ref uint unSts)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`7�ȊO�̎��͈����G���[ 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// ���ԍ���0�`15�ȊO�̎��͈����G���[ 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);
		
		// �g���X�e�[�^�X�Ǎ���
        unRet = hcp530_rReg(hDevID, usAxis, 0xf1, ref unSts);
        return (unRet);
    }

	/// <summary>
	/// ���x�Ǎ���
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="usSpd">���x�i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_ReadSpd(uint hDevID, ushort usAxis, ref ushort usSpd)
    {
        uint unRet = 0;
        uint unSpd = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`7�ȊO�̎��͈����G���[ 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// ���ԍ���0�`15�ȊO�̎��͈����G���[ 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // ���x�Ǎ�
        unRet = hcp530_rReg(hDevID, usAxis, RRSPD, ref unSpd);
        unSpd &= (uint)0x0000ffff;
        usSpd = (ushort)unSpd;
        return (unRet);
    }

 	/// <summary>
	/// �J�E���^�Ǎ���
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="usSelCtr">�ǂݍ��ރJ�E���^�̎w��</param>
	/// <param name="nCtrValue">�J�E���^�l�̊i�[��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_ReadCtr(uint hDevID, ushort usAxis, ushort usSelCtr, ref int nCtrValue)
    {
        uint unRet = 0;
        uint unData = 0;
        byte byCmd = RRCTR1;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`7�ȊO�̎��͈����G���[ 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// ���ԍ���0�`15�ȊO�̎��͈����G���[ 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // �J�E���^�Ǎ�
        switch (usSelCtr)
        {
            case 1: byCmd = RRCTR1; break;
            case 2: byCmd = RRCTR2; break;
            case 3: byCmd = RRCTR3; break;
            case 4: byCmd = RRCTR4; break;
			default:				return (ILLEGAL_PRM);
        }

        unRet = hcp530_rReg(hDevID, usAxis, byCmd, ref unData);
        nCtrValue = (int)unData;
        return (unRet);
    }

 	/// <summary>
	/// �C�x���g�}�X�N�̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="unMask">�C�x���g�}�X�N�f�[�^</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SetEventMask(uint hDevID, ushort usAxis, uint unMask)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`15�ȊO�܂���
        // �C�x���g�}�X�N�f�[�^�� 0�`0x0003ffff�ȊO�̎��͈����G���[
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);
		if ((unMask & 0xfffc0000) != 0)         return (ILLEGAL_PRM);

        // �C�x���g�}�X�N(RIRQ)���W�X�^�̏�����
        unRet = hcp530_wReg(hDevID, usAxis, WRIRQ, unMask);
        return (unRet);
    }

	/// <summary>
	/// �x�[�X���x�̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="unRfl">�x�[�X���x���W�X�^�l</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SetFLSpd(uint hDevID, ushort usAxis, uint unRfl)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`15�ȊO�܂���
        // ���x���W�X�^��1�`0xffff�ȊO�̎��͈����G���[
//      if ((usAxis < 0) || (7 < usAxis)||
		if ((usAxis < 0) || (15 < usAxis)||
            (unRfl == 0) || (65535 < unRfl))
        {
            return (ILLEGAL_PRM);
        }

        // PRFL�̏�����
        unRet = hcp530_wReg(hDevID, usAxis, WPRFL, unRfl);
        return (unRet);
    }

	/// <summary>
	/// �⏕���x�̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="unRfa">�⏕���x���W�X�^�l</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SetAuxSpd(uint hDevID, ushort usAxis, uint unRfa)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`15�ȊO�܂���
        // ���x���W�X�^��1�`0xffff�ȊO�̎��͈����G���[
//      if ((usAxis < 0) || (7 < usAxis)||
		if ((usAxis < 0) || (15 < usAxis)||
			(unRfa == 0) || (65535 < unRfa))
        {
            return (ILLEGAL_PRM);
        }

        // RFA�̏�����
        unRet = hcp530_wReg(hDevID, usAxis, WRFA, unRfa);
        return (unRet);
    }

	/// <summary>
	/// �������[�g�̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/** <param name="unRur">�������[�g���W�X�^�l
	��N���b�N���g�� = 19660800Hz
	������������������(�b) = (PRFH - PRFL) * (PRUR + 1) * 4 / 19660800
	�r����������������(�b) = (PRFH - PRFL) * (PRUR + 1) * 8 / 19660800
	</param> */
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SetAccRate(uint hDevID, ushort usAxis, uint unRur)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`15�ȊO�܂���
        // �������[�g��1�`0xffff�ȊO�̎��͈����G���[
//      if ((usAxis < 0) || (7 < usAxis)||
		if ((usAxis < 0) || (15 < usAxis)||
		(unRur == 0) || (65535 < unRur))
        {
            return (ILLEGAL_PRM);
        }

        // PRUR�̏�����
        unRet = hcp530_wReg(hDevID, usAxis, WPRUR, unRur);
        return (unRet);
    }

	/// <summary>
	/// �������[�g�̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="unRdr">�������[�g���W�X�^�l</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SetDecRate(uint hDevID, ushort usAxis, uint unRdr)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`15�ȊO�܂���
        // �������[�g��1�`0xffff�ȊO�̎��͈����G���[
//      if ((usAxis < 0) || (7 < usAxis)|| (65535 < unRdr))
		if ((usAxis < 0) || (15 < usAxis)|| (65535 < unRdr))
		{
            return (ILLEGAL_PRM);
        }

        // PRDR�̏�����
        unRet = hcp530_wReg(hDevID, usAxis, WPRDR, unRdr);
        return (unRet);
    }

	/// <summary>
	/// ���x�{�����W�X�^�̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="unRmg">���x�{�����W�X�^�l(RMG) RMG=300/���x�{��-1 </param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SetMult(uint hDevID, ushort usAxis, uint unRmg)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`15�ȊO�܂���
        // �{���ݒ�l��2�`4095�ȊO�͈����G���[ 
//      if ((usAxis < 0) || (7 < usAxis) || (unRmg < 2) || (4095 < unRmg)) 
		if ((usAxis < 0) || (15 < usAxis) || (unRmg < 2) || (4095 < unRmg)) 
		{
            return (ILLEGAL_PRM);
        }

        // PRMG�̏�����
        unRet = hcp530_wReg(hDevID, usAxis, WPRMG, unRmg);
        return (unRet);
    }

	/// <summary>
	/// �����J�n�_�̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="nRdp">�����J�n�_</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SetDecPoint(uint hDevID, ushort usAxis, int nRdp)
    {
        uint unRet = 0;
        uint unData = (uint)nRdp;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`7�ȊO�̓G���[
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// ���ԍ���0�`15�ȊO�̓G���[
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);
		
        // PRDP�̏�����
        unRet = hcp530_wReg(hDevID, usAxis, WPRDP, unData);
        return (unRet);
    }

	/// <summary>
	/// ���샂�[�h������
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="usMode">���샂�[�h</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_WritOpeMode(uint hDevID, ushort usAxis, ushort usMode)
    {
        uint unRet = 0;
        uint unRmd = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`15�ȊO�܂���
        // ���샂�[�h���ُ�̎��͈����G���[
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);
		if ((usMode & 0xff80) != 0)             return (ILLEGAL_PRM);

        // ���샂�[�h(PRMD)�Ǎ���
        unRet = hcp530_rReg(hDevID, usAxis, RPRMD, ref unRmd);
		if (unRet != 0)            return (unRet);

        unRmd &= 0xef3a700;
        if (usMode == 0x42)	unRmd |= 0x4041;
		else				unRmd |= usMode;

        // ���샂�[�h(PRMD)������
        unRet = hcp530_wReg(hDevID, usAxis, WPRMD, unRmd);
        return (unRet);
    }

	/// <summary>
	/// ���쑬�x���W�X�^�̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="unRfh">���쑬�x���W�X�^�l</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_WritFHSpd(uint hDevID, ushort usAxis, uint unRfh)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`15�ȊO�܂���
        // ���x���W�X�^��1�`0xffff�ȊO�̎��͈����G���[
//      if ((usAxis < 0) || (7 < usAxis) ||
		if ((usAxis < 0) || (15 < usAxis) ||
		(unRfh == 0) || (65535 < unRfh))
        {
            return (ILLEGAL_PRM);
        }

        // PRFH�̏�����
        unRet = hcp530_wReg(hDevID, usAxis, WPRFH, unRfh);
        return (unRet);
    }

	/// <summary>
	/// �ʒu���߈ړ��ʂ̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="nDstnc">�ړ���</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_WritPos(uint hDevID, ushort usAxis, int nDstnc)
    {
        uint unRet;				// �߂�l
        uint unData=(uint)nDstnc;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`15�ȊO�̓G���[
        // �ړ��ʂ̃`�F�b�N
//      if ((usAxis < 0) || (7 < usAxis) ||
		if ((usAxis < 0) || (15 < usAxis) ||
		(nDstnc < -134217728) || (134217727 < nDstnc)) 
        {
            return (ILLEGAL_PRM);
        }

        // PRMV�̏�����
        unRet = hcp530_wReg(hDevID, usAxis, WPRMV, unData);
        return (unRet);
    }

	/// <summary>
	/// ������Ԃ̈ړ��ʂ̐ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="nDstnc">�ړ���</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_WritLine(uint hDevID, ushort usAxis, int nDstnc)
    {
        uint unRet = 0;
        uint unData = (uint)nDstnc;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ���0�`15�ȊO�̓G���[
        // �ړ��ʂ̃`�F�b�N
//      if ((usAxis < 0) || (7 < usAxis) ||
		if ((usAxis < 0) || (15 < usAxis) ||
		    (nDstnc < -134217728) || (134217727 < nDstnc))
        {
            return (ILLEGAL_PRM);
        }

        // PRMV�̏�����
        unRet = hcp530_wReg(hDevID, usAxis, WPRMV, unData);
        return (unRet);
    }

 	/// <summary>
	/// �~�ʕ�Ԃ̃f�[�^�ݒ�
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���g�ݍ��킹</param>
	/// <param name="nEnd1">�I�_�ʒu1</param>
	/// <param name="nEnd2">�I�_�ʒu2</param>
	/// <param name="nCen1">���S�ʒu1</param>
	/// <param name="nCen2">���S�ʒu2</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_WritCircl(uint hDevID, ushort usAxis,
                            int nEnd1, int nEnd2, int nCen1, int nCen2)
    {
        uint unRet = 0;
        ushort usAx1 = 0;
        ushort usAx2 = 0;
        uint unEnd1 = (uint)nEnd1;
        uint unEnd2 = (uint)nEnd2;
        uint unCen1 = (uint)nCen1;
        uint unCen2 = (uint)nCen2;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ��Ԏ��̑g�ݍ��킹�̈�����0�`23�ȊO�̎��̓G���[
        // ���S�ʒu�ƏI�_�ʒu�̃`�F�b�N
//      if ((usAxis < 0) || (usAxis > 11) ||  
		if ((usAxis < 0) || (usAxis > 23) ||  
		(nEnd1 < -134217728) || (134217727 < nEnd1)|| 
        (nEnd2 < -134217728) || (134217727 < nEnd2)||  
        (nCen1 < -134217728) || (134217727 < nCen1)||
        (nCen2 < -134217728) || (134217727 < nCen2))
        {
            return (ILLEGAL_PRM);
        }
        if ((nCen1 == 0) && (nCen2 == 0))
        {
            return (ILLEGAL_PRM);
        }

        switch (usAxis)
        {
			// ---------------------------------
			// for CPD5016 (2010.07.29)
			// ---------------------------------
//			case 0: usAx1 = 0; usAx2 = 1; break;	// X(0), Y(1)
//			case 1: usAx1 = 0; usAx2 = 2; break;	// X(0), Z(2)
//			case 2: usAx1 = 0; usAx2 = 3; break;	// X(0), U(3)
//			case 3: usAx1 = 1; usAx2 = 2; break;	// Y(1), Z(2)
//			case 4: usAx1 = 1; usAx2 = 3; break;	// Y(1), U(3)
//			case 5: usAx1 = 2; usAx2 = 3; break;	// Z(2), U(3)
//			case 6: usAx1 = 4; usAx2 = 5; break;	// V(4), W(5)
//			case 7: usAx1 = 4; usAx2 = 6; break;	// V(4), A(6)
//			case 8: usAx1 = 4; usAx2 = 7; break;	// V(4). B(7)
//			case 9: usAx1 = 5; usAx2 = 6; break;	// W(5), A(6)
//			case 10: usAx1 = 5; usAx2 = 7; break;	// W(5), B(7)
//			case 11: usAx1 = 6; usAx2 = 7; break;	// A(6), B(7)

			// X1�`U1
			case  0: usAx1 =  0; usAx2 =  1;	break;	// X1(0)-Y1(1)
			case  1: usAx1 =  0; usAx2 =  2;	break;	// X1(0)-Z1(2)
			case  2: usAx1 =  0; usAx2 =  3;	break;	// X1(0)-U1(3)
			case  3: usAx1 =  1; usAx2 =  2;	break;	// Y1(1)-Z1(2)
			case  4: usAx1 =  1; usAx2 =  3;	break;	// Y1(1)-U1(3)
			case  5: usAx1 =  2; usAx2 =  3;	break;	// Z1(2)-U1(3)

			// X2�`U2
			case  6: usAx1 =  4; usAx2 =  5;	break;	// X2(4)-Y2(5)
			case  7: usAx1 =  4; usAx2 =  6;	break;	// X2(4)-Z2(6)
			case  8: usAx1 =  4; usAx2 =  7;	break;	// X2(4)-U2(7)
			case  9: usAx1 =  5; usAx2 =  6;	break;	// Y2(5)-Z2(6)
			case 10: usAx1 =  5; usAx2 =  7;	break;	// Y2(5)-U2(7)
			case 11: usAx1 =  6; usAx2 =  7;	break;	// Z2(6)-U2(7)

			// X3�`U3
			case 12: usAx1 =  8; usAx2 =  9;	break;	// X3(8)-Y3(9)
			case 13: usAx1 =  8; usAx2 = 10;	break;	// X3(8)-Z3(10)
			case 14: usAx1 =  8; usAx2 = 11;	break;	// X3(8)-U3(11)
			case 15: usAx1 =  9; usAx2 = 10;	break;	// Y3(9)-Z3(10)
			case 16: usAx1 =  9; usAx2 = 11;	break;	// Y3(9)-U3(11)
			case 17: usAx1 = 10; usAx2 = 11;	break;	// Z3(10)-U3(11)

			// X3�`U3
			case 18: usAx1 = 12; usAx2 = 13;	break;	// X4(12)-Y4(13)
			case 19: usAx1 = 12; usAx2 = 14;	break;	// X4(12)-Z4(14)
			case 20: usAx1 = 12; usAx2 = 15;	break;	// X4(12)-U4(15)
			case 21: usAx1 = 13; usAx2 = 14;	break;	// Y4(13)-Z4(14)
			case 22: usAx1 = 13; usAx2 = 15;	break;	// Y4(13)-U4(15)
			case 23: usAx1 = 14; usAx2 = 15;	break;	// Z4(14)-U4(15)
		}
        // PRMV(�I�_�ʒu)�̏�����
        unRet = hcp530_wReg(hDevID, usAx1, WPRMV, unEnd1);
        if (unRet != 0)            return (unRet);
        unRet = hcp530_wReg(hDevID, usAx2, WPRMV, unEnd2);
		if (unRet != 0)            return (unRet);

        // PRIP(���S�ʒu)�̏�����
        unRet = hcp530_wReg(hDevID, usAx1, WPRIP, unCen1);
		if (unRet != 0)            return (unRet);
		unRet = hcp530_wReg(hDevID, usAx2, WPRIP, unCen2);
        return (unRet);
    }

	/// <summary>
	/// �J�E���^�v���Z�b�g
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <param name="nData">�v���Z�b�g�l</param>
	/// <param name="usCtr">�J�E���^�I��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_WritCtr(uint hDevID, ushort usAxis, int nData, ushort usCtr)
    {
        byte byCmd = WRCTR1;
        uint unRet = 0;
        uint unData = (uint)nData;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ�0�`7�ȊO�̎��̓G���[
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// ���ԍ�0�`15�ȊO�̎��̓G���[
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // �v���Z�b�g�l�̃`�F�b�N
        if (usCtr == 3)
        {	
            // �J�E���^�R�͂P�U�r�b�g
            if ((nData < -32768) || (32767 < nData))
            {
                return (ILLEGAL_PRM);
            }
        }
        else
        {	
            // ���̑��̃J�E���^�͂R�Q�r�b�g
            if ((nData < -134217728) || (134217727 < nData))
            {
                return (ILLEGAL_PRM);
            }
        }

        // �v���Z�b�g�l������
		switch(usCtr)
		{
			case 1:	byCmd = WRCTR1;	break;
			case 2:	byCmd = WRCTR2;	break;
			case 3:	byCmd = WRCTR3;	break;
			case 4:	byCmd = WRCTR4;	break;
			default:				return (ILLEGAL_PRM);
		}			

		unRet = hcp530_wReg(hDevID, usAxis, byCmd, unData);
        return (unRet);
    }

	/// <summary>
	/// ������~
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���g�ݍ��킹(HEX)</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_DecStop(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = SDSTP;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ�0x01�`0xff�ȊO�̎��̓G���[
//		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		// ���ԍ�0x0001�`0xffff�ȊO�̎��̓G���[
		if ((usAxis <= 0) || (0xffff < usAxis))	return (ILLEGAL_PRM);

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// X - U : �w���Ɍ�����~
//		if((usAxis & 0x0f) != 0)
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));
//		}
        // V - B : �u���Ɍ�����~
//		if (((usAxis & 0xf0) != 0) && (unRet == 0))
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
//		}

		// X1 - U1 : �w�P���Ɍ�����~
		if( (usAxis & 0x000f) != 0 )
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, (ushort)( ((usAxis & 0x000f) << 8) | usCmd) );
		}
		// X2 - U2 : �w�Q���Ɍ�����~
		if( ((usAxis & 0x00f0) != 0) && (unRet == 0) )
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X2_AX, (ushort)( ((usAxis & 0x00f0) << 4) | usCmd) );
		}
		// X3 - U3 : �w�R���Ɍ�����~
		if( ((usAxis & 0x0f00) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X3_AX, (ushort)( (usAxis & 0x0f00 ) | usCmd));
		}
		// X4 - U4 : �w�S���Ɍ�����~
		if( ((usAxis & 0xf000) != 0) && (unRet == 0))
		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, (ushort)( (usAxis & 0xf000 >> 4) | usCmd));
			ushort ax = (ushort)(((usAxis & 0xf000) >> 4) | usCmd);	// C#
			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, ax);
		}

        return (unRet);
    }

    // ����~
    // 		�����F�f�o�C�X�n���h���C���g�ݍ��킹(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���g�ݍ��킹(HEX)</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_QuickStop(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = STOP;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ�0x01�`0xff�ȊO�̎��̓G���[
//		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		// ���ԍ�0x0001�`0xffff�ȊO�̎��̓G���[
		if ((usAxis <= 0) || (0xffff < usAxis))	return (ILLEGAL_PRM);

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// X - U : �w���ɑ���~
//		if ((usAxis & 0x0f) != 0)
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));
//		}
        // V - B : �u���ɑ���~
//		if (((usAxis & 0xf0) != 0) && (unRet == 0))
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
//		}

		// X1 - U1 : �w�P���ɑ���~
		if( (usAxis & 0x000f) != 0 )
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, (ushort)( ((usAxis & 0x000f) << 8) | usCmd) );
		}
		// X2 - U2 : �w�Q���ɑ���~
		if( ((usAxis & 0x00f0) != 0) && (unRet == 0) )
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X2_AX, (ushort)( ((usAxis & 0x00f0) << 4) | usCmd) );
		}
		// X3 - U3 : �w�R���ɑ���~
		if( ((usAxis & 0x0f00) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X3_AX, (ushort)( (usAxis & 0x0f00 ) | usCmd));
		}
		// X4 - U4 : �w�S���ɑ���~
		if( ((usAxis & 0xf000) != 0) && (unRet == 0))
		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, (ushort)( (usAxis & 0xf000 >> 4) | usCmd));
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, 0x0f49);
			ushort ax = (ushort)(((usAxis & 0xf000) >> 4) | usCmd);	// C#
			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, ax);
		}

		return (unRet);
    }

    // ����~ 
    // 		�����F�f�o�C�X�n���h���C���g�ݍ��킹(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���g�ݍ��킹(HEX)</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_EmgStop(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = CMEMG;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ�0x01�`0xff�ȊO�̎��̓G���[
//		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		// ���ԍ�0x0001�`0xffff�ȊO�̎��̓G���[
		if ((usAxis <= 0) || (0xffff < usAxis))	return (ILLEGAL_PRM);

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// X - U : �w���ɔ���~
//		if ((usAxis & 0x0f) != 0)
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));
//		}
        // V - B : �u���ɔ���~
//		if (((usAxis & 0xf0) != 0) && (unRet == 0))
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
//		}

		// X1 - U1 : �w�P���ɔ���~
		if( (usAxis & 0x000f) != 0 )
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, (ushort)( ((usAxis & 0x000f) << 8) | usCmd) );
		}
		// X2 - U2 : �w�Q���ɔ���~
		if( ((usAxis & 0x00f0) != 0) && (unRet == 0) )
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X2_AX, (ushort)( ((usAxis & 0x00f0) << 4) | usCmd) );
		}
		// X3 - U3 : �w�R���ɔ���~
		if( ((usAxis & 0x0f00) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X3_AX, (ushort)( (usAxis & 0x0f00 ) | usCmd));
		}
		// X4 - U4 : �w�S���ɔ���~
		if( ((usAxis & 0xf000) != 0) && (unRet == 0))
		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, (ushort)( (usAxis & 0xf000 >> 4) | usCmd));
			ushort ax = (ushort)(((usAxis & 0xf000) >> 4) | usCmd);	// for C#
			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, ax);
		}

        return (unRet);
    }

    // ����������~
    // 		�����F�f�o�C�X�n���h���C���g�ݍ��킹(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���g�ݍ��킹(HEX)</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SyDecStop(uint hDevID, ushort usAxis)
    {
        ushort usAx = 0;
		uint[] unRmd = {0,0,0,0,0,0,0,0};
		uint unRenv1 = 0;
		uint unData = 0;
		uint unRet = 0;
        ushort[] usAxBit = { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };

        // ���ԍ�0x01�`0xff�ȊO�̎��̓G���[
        if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
        
		for(usAx=X_AX; usAx < usAxBit.Length; usAx++) {
            if ((usAxis & usAxBit[usAx])==usAxBit[usAx])
            {
				// �w�ߎ�
		        // �����W�X�^�P(RENV1): ������~
                unRet = hcp530_rReg(hDevID, usAx, RRENV1, ref unRenv1);
				if (unRet != 0)            return (unRet);
				// STP���͎�����~�ݒ�̏ꍇ
				if ((unRmd[usAx] & R1_CSTP_BIT) == 0)
                {
                    unRenv1 |= (uint)R1_CSTP_BIT;	// STP���͂Ō�����~
                    unRet = hcp530_wReg(hDevID, usAx, WRENV1, unRenv1);
					if (unRet != 0)            return (unRet);
				}
                
                // ���샂�[�h(RMD): STP���͗L��
                unRet = hcp530_rReg(hDevID, usAx, RRMD, ref unRmd[usAx]);
				if (unRet != 0)            return (unRet);
				// STP���͖����Ȃ��STP���͗L��
				if ((unRmd[usAx] & MD_STPE_BIT) == 0)
                {
                    unData = unRmd[usAx] | MD_STPE_BIT;
                    unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
					if (unRet != 0)            return (unRet);
				}
            }
            else
            {
				// ��w�ߎ�
				// ���샂�[�h(RMD) 
				unRet = hcp530_rReg(hDevID, usAx, RRMD, ref unRmd[usAx]);
				if (unRet != 0)            return (unRet);
				// STP���͗L���Ȃ��STP���͗L������
				if ((unRmd[usAx] & MD_STPE_BIT) == MD_STPE_BIT)
				{
					unData = unRmd[usAx] & (~MD_STPE_BIT);
					unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
					if (unRet != 0)            return (unRet);
				}
			}
        }
        unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, CMSTP);	// X���FSTP�o��
		if (unRet != 0)            return (unRet);

		// ���샂�[�h��߂�
		for (usAx = X_AX; usAx < usAxBit.Length; usAx++)
		{
			if ((usAxis & usAxBit[usAx]) == usAxBit[usAx])
			{
				unData = unRmd[usAx] & (~MD_STPE_BIT);
				unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
				if (unRet != 0)            return (unRet);
			}
			else
			{
				unRet = hcp530_wReg(hDevID, usAx, WRMD, unRmd[usAx]);
				if (unRet != 0)            return (unRet);
			}
		}
        return (unRet);
    }

    // ��������~ 
    // 		�����F�f�o�C�X�n���h���C���g�ݍ��킹(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���g�ݍ��킹(HEX)</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SyQuickStop(uint hDevID, ushort usAxis)
    {
		ushort usAx = 0;
		uint[] unRmd = {0,0,0,0,0,0,0,0};
		uint unRenv1 = 0;
		uint unData = 0;
		uint unRet = 0;
		ushort[] usAxBit = { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };

		// ���ԍ�0x01�`0xff�ȊO�̎��̓G���[
		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);

		for(usAx=X_AX; usAx < usAxBit.Length; usAx++) 
		{
			if ((usAxis & usAxBit[usAx])==usAxBit[usAx])
			{
				// �w�ߎ�
				// �����W�X�^�P(RENV1): ����~
				unRet = hcp530_rReg(hDevID, usAx, RRENV1, ref unRenv1);
				if (unRet != 0)            return (unRet);
				// STP���͎�������~�ݒ�̏ꍇ
				if ((unRmd[usAx] & R1_CSTP_BIT) == R1_CSTP_BIT)
				{
					unRenv1 &= (uint)(~R1_CSTP_BIT);	// STP���͂ő���~
					unRet = hcp530_wReg(hDevID, usAx, WRENV1, unRenv1);
					if (unRet != 0)            return (unRet);
				}
                
				// ���샂�[�h(RMD): STP���͗L��
				unRet = hcp530_rReg(hDevID, usAx, RRMD, ref unRmd[usAx]);
				if (unRet != 0)            return (unRet);
				// STP���͖����Ȃ��STP���͗L��
				if ((unRmd[usAx] & MD_STPE_BIT) == 0)
				{
					unData = unRmd[usAx] | MD_STPE_BIT;
					unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
					if (unRet != 0)            return (unRet);
				}
			}
			else
			{
				// ��w�ߎ�
				// ���샂�[�h(RMD) 
				unRet = hcp530_rReg(hDevID, usAx, RRMD, ref unRmd[usAx]);
				if (unRet != 0)            return (unRet);
				// STP���͗L���Ȃ��STP���͗L������
				if ((unRmd[usAx] & MD_STPE_BIT) == MD_STPE_BIT)
				{
					unData = unRmd[usAx] & (~MD_STPE_BIT);
					unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
					if (unRet != 0)            return (unRet);
				}
			}
		}
		unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, CMSTP);	// X���FSTP�o��
		if (unRet != 0)            return (unRet);

		// ���샂�[�h��߂�
		for (usAx = X_AX; usAx < usAxBit.Length; usAx++)
		{
			if ((usAxis & usAxBit[usAx]) == usAxBit[usAx])
			{
				unData = unRmd[usAx] & (~MD_STPE_BIT);
				unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
				if (unRet != 0)            return (unRet);
			}
			else
			{
				unRet = hcp530_wReg(hDevID, usAx, WRMD, unRmd[usAx]);
				if (unRet != 0)            return (unRet);
			}
		}
		return (unRet);
    }

	/// <summary>
	/// �X�^�[�g(�����֐�)
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���g�ݍ��킹(HEX)</param>
	/// <param name="usCmd">�X�^�[�g�R�}���h</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
	private static uint cpd_Start(uint hDevID, ushort usAxis, ushort usCmd)
	{
		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		ushort usAx = 0;
		uint unData = 0;
//		uint[] unRmd = {0,0,0,0,0,0,0,0};
		uint[] unRmd = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
		uint unRet = 0;
//		ushort[] usAxBit = { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };
		ushort[] usAxBit =
		{
			0x0001,	// X1	 0
			0x0002,	// Y1	 1
			0x0004,	// Z1	 2
			0x0008,	// U1	 3
			0x0010,	// X2	 4
			0x0020,	// Y2	 5
			0x0040,	// Z2	 6
			0x0080,	// U2	 7
			0x0100,	// X3	 8
			0x0200,	// Y3	 9
			0x0400,	// Z3	10
			0x0800,	// U3	11
			0x1000,	// X4	12
			0x2000,	// Y4	13
			0x4000,	// Z4	14
			0x8000	// U4	15
		};

        // ���ԍ�0x01�`0xff�ȊO�̎��̓G���[
//		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		// ���ԍ�0x01�`0xffff�ȊO�̎��̓G���[
		if ((usAxis <= 0) || (0xffff < usAxis))	return (ILLEGAL_PRM);

		// ---------------------------------
		// �O���[�v�P�̂̏ꍇ
		// ---------------------------------
		short gCnt = 0;
		if( (usAxis & 0x000f) != 0 )	gCnt++;
		if( (usAxis & 0x00f0) != 0 )	gCnt++;
		if( (usAxis & 0x0f00) != 0 )	gCnt++;
		if( (usAxis & 0xf000) != 0 )	gCnt++;
		if( gCnt == 1 )
		{
			// X1�`U1���O���[�v�̂�
			if( (usAxis & 0x000f ) != 0 )
			{
				usCmd = (ushort)((usAxis << 8) | usCmd);
				unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, usCmd);
				return (unRet);
			}
			// X2�`U2���O���[�v�̂�
			else if( (usAxis & 0x00f0 ) != 0 )
			{
				usCmd = (ushort)((usAxis << 4) | usCmd);
				unRet = Hicpd530.cp530_wCmdW(hDevID, X2_AX, usCmd);
				return (unRet);
			}
			// X3�`U3���O���[�v�̂�
			else if( (usAxis & 0x0f00 ) != 0 )
			{
				usCmd = (ushort)(usAxis | usCmd);
				unRet = Hicpd530.cp530_wCmdW(hDevID, X3_AX, usCmd);
				return (unRet);
			}
			// X4�`U4���O���[�v�̂�
			else if( (usAxis & 0xf000 ) != 0 )
			{
				usCmd = (ushort)((usAxis >> 4) | usCmd);
				unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, usCmd);
				return (unRet);
			}
		}

		// ---------------------------------
		// �O���[�v���݂̏ꍇ
		// ---------------------------------
		for (usAx = 0; usAx < usAxBit.Length; usAx++)
		{
			if((usAxis & usAxBit[usAx]) == usAxBit[usAx])
			{
				// �����X�^�[�g:�w�ߎ�
				// ���샂�[�h(RMD): �����X�^�[�g
				unRet = hcp530_rReg(hDevID, usAx, RPRMD, ref unRmd[usAx]);
				if (unRet != 0)            return (unRet);
				unData = unRmd[usAx] & (~(MD_MSY0_BIT | MD_MSY1_BIT));
				unData |= MD_MSY0_BIT;
				unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
				if (unRet != 0)            return (unRet);
				unRet = Hicpd530.cp530_wCmdW(hDevID, usAx, usCmd);	// �R�}���h������
				if (unRet != 0)            return (unRet);
			} 
			else 
			{
				// �����X�^�[�g:��w�ߎ�
				// ���샂�[�h(RMD): STA���͂ŃX�^�[�g�����Ȃ�
				unRet = hcp530_rReg(hDevID, usAx, RPRMD, ref unRmd[usAx]);
				if (unRet != 0)            return (unRet);
				unData = unRmd[usAx] & (~(MD_MSY0_BIT | MD_MSY1_BIT));
				unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
				if (unRet != 0)            return (unRet);
			}
		}
		unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, CMSTA);	// X�������X�^�[�g�o��
		for (usAx = 0; usAx < usAxBit.Length; usAx++)
		{
			if(((usAxis & usAxBit[usAx]) == usAxBit[usAx])) 
			{
				// ���샂�[�h(RMD)������
				unRet = hcp530_wReg(hDevID, usAx, WRMD, unRmd[usAx]);
				if (unRet != 0)            return (unRet);
			}
		}

/**
        if((usAxis & 0xf0) == 0) 
		{
        // X�`U��(4���܂�)
            usCmd = (ushort)((usAxis << 8) | usCmd);
	        unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, usCmd);		// �R�}���h������
		} 
		else 
		{
        // 5���ȏ�
	        if((usAxis & 0x0f) == 0) {
	        // 5���ȏ㤒A��V�`B��
                usCmd = (ushort)((usAxis << 4) | STAUD);
		        unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, usCmd);	// �R�}���h������
	        } else {


	        // 5���ȏ�FX�`U����V�`B��������
                for (usAx = 0; usAx < usAxBit.Length; usAx++)
                {
			        if((usAxis & usAxBit[usAx]) == usAxBit[usAx]){
			        // �����X�^�[�g:�w�ߎ�
				        // ���샂�[�h(RMD): �����X�^�[�g
                        unRet = hcp530_rReg(hDevID, usAx, RPRMD, ref unRmd[usAx]);
						if (unRet != 0)            return (unRet);
						unData = unRmd[usAx] & (~(MD_MSY0_BIT | MD_MSY1_BIT));
                        unData |= MD_MSY0_BIT;
                        unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
						if (unRet != 0)            return (unRet);
						unRet = Hicpd530.cp530_wCmdW(hDevID, usAx, usCmd);	// �R�}���h������
						if (unRet != 0)            return (unRet);
					} 
					else 
					{
			        // �����X�^�[�g:��w�ߎ�
				        // ���샂�[�h(RMD): STA���͂ŃX�^�[�g�����Ȃ�
                        unRet = hcp530_rReg(hDevID, usAx, RPRMD, ref unRmd[usAx]);
						if (unRet != 0)            return (unRet);
						unData = unRmd[usAx] & (~(MD_MSY0_BIT | MD_MSY1_BIT));
                        unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
						if (unRet != 0)            return (unRet);
					}
		        }
		        unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, CMSTA);	// X�������X�^�[�g�o��
                for (usAx = 0; usAx < usAxBit.Length; usAx++)
                {
			        if(((usAxis & usAxBit[usAx]) == usAxBit[usAx])) {
				        // ���샂�[�h(RMD)������
                        unRet = hcp530_wReg(hDevID, usAx, WRMD, unRmd[usAx]);
						if (unRet != 0)            return (unRet);
					}
		        }
	        }
        }
**/
	return (unRet);
	}

	/// <summary>
	/// �����X�^�[�g
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���g�ݍ��킹(HEX)</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_AccStart(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;

		unRet = cpd_Start(hDevID, usAxis, STAUD);

		return (unRet);
    }

	/// <summary>
	/// FH�葬�X�^�[�g
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���g�ݍ��킹(HEX)</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_CnstStartFH(uint hDevID, ushort usAxis)
    {
		uint unRet = 0;

		unRet = cpd_Start(hDevID, usAxis, STAFH);

		return (unRet);
    }

	/// <summary>
	/// FL�葬�X�^�[�g 
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���g�ݍ��킹(HEX)</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_CnstStartFL(uint hDevID, ushort usAxis)
    {
		uint unRet = 0;

		unRet = cpd_Start(hDevID, usAxis, STAFL);

		return (unRet);
    }

	/// <summary>
	/// FH�葬�X�^�[�g�㌸����~
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���g�ݍ��킹(HEX)</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_CnstStartByDec(uint hDevID, ushort usAxis)
    {
		uint unRet = 0;

		unRet = cpd_Start(hDevID, usAxis, STAFHD);

		return (unRet);
    }

    // �T�[�{�I��
    // 		�����F�f�o�C�X�n���h���C���g�ݍ��킹(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SvOn(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = SVON;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ�0x01�`0xff�ȊO�̎��̓G���[
//		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		// ���ԍ�0x0001�`0xffff�ȊO�̎��̓G���[
		if ((usAxis <= 0) || (0xffff < usAxis))	return (ILLEGAL_PRM);
		
		// X1 - U1 : �w�P���ɃT�[�{�n�m
		if ((usAxis & 0x000f) != 0)
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, (ushort)(((usAxis & 0x000f) << 8) | usCmd));
		}
		// X2 - U2 : �w�Q���ɃT�[�{�n�m
		if (((usAxis & 0x00f0)!=0) && (unRet==0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X2_AX, (ushort)(((usAxis & 0x00f0) << 4) | usCmd));
		}
		// X3 - U3 : �w�R���ɃT�[�{�n�m
		if (((usAxis & 0x0f00)!=0) && (unRet==0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X3_AX, (ushort)(((usAxis & 0x0f00)) | usCmd));
		}
		// X4 - U4 : �w�S���ɃT�[�{�n�m
		if (((usAxis & 0xf000)!=0) && (unRet==0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, (ushort)(((usAxis & 0xf000) >> 4) | usCmd));
		}

		// X - U : �w���ɃT�[�{�n�m
//		if ((usAxis & 0x0f) != 0)
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));
//		}
		// V - B : �u���ɃT�[�{�n�m
//		if (((usAxis & 0xf0)!=0) && (unRet==0))
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
//		}
        return (unRet);
    }

    // �T�[�{�I�t
    // 		�����F�f�o�C�X�n���h���C���g�ݍ��킹(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SvOff(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = SVOFF;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ�0x01�`0xff�ȊO�̎��̓G���[
//		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		// ���ԍ�0x0001�`0xffff�ȊO�̎��̓G���[
		if ((usAxis <= 0) || (0xffff < usAxis))	return (ILLEGAL_PRM);
		
		// X1 - U1 : �w�P���ɃT�[�{�n�e�e
		if ((usAxis & 0x000f) != 0)
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, (ushort)(((usAxis & 0x000f) << 8) | usCmd));
		}
		// X2 - U2 : �w�Q���ɃT�[�{�n�e�e
		if (((usAxis & 0x00f0) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X2_AX, (ushort)(((usAxis & 0x00f0) << 4) | usCmd));
		}
		// X3 - U3 : �w�R���ɃT�[�{�n�e�e
		if (((usAxis & 0x0f00) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X3_AX, (ushort)(((usAxis & 0x0f00) ) | usCmd));
		}
		// X4 - U4 : �w�S���ɃT�[�{�n�e�e
		if (((usAxis & 0xf000) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, (ushort)(((usAxis & 0xf000) >> 4) | usCmd));
		}

		// X - U : �w���ɃT�[�{�n�e�e
//		if ((usAxis & 0x0f) != 0)
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));
//		}
		// V - B : �u���ɃT�[�{�n�e�e
//		if (((usAxis & 0xf0) != 0) && (unRet == 0))
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
//		}
        return (unRet);
    }

    // �T�[�{���Z�b�g�I��
    // 		�����F�f�o�C�X�n���h���C���g�ݍ��킹(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SvResetOn(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = SVRSTON;

        // ���ԍ�0x01�`0xff�ȊO�̎��̓G���[
		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		
		// X - U : �w���ɃT�[�{���Z�b�g�n�m
        if ((usAxis & 0x0f) != 0)
        {
            unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));

        }
        // V - B : �u���ɃT�[�{���Z�b�g�n�m
        if (((usAxis & 0xf0) != 0) && (unRet == 0))
        {
            unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
        }
        return (unRet);
    }

    // �T�[�{���Z�b�g�I�t 
    // 		�����F�f�o�C�X�n���h���C���g�ݍ��킹(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_SvResetOff(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = SVRSTOFF;

        // ���ԍ�0x01�`0xff�ȊO�̎��̓G���[
		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		
		// X - U : �w���ɃT�[�{���Z�b�g�n�e�e
        if ((usAxis & 0x0f) != 0)
        {
            unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));

        }
        // V - B : �u���ɃT�[�{���Z�b�g�n�e�e
        if (((usAxis & 0xf0) != 0) && (unRet == 0))
        {
            unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
        }
        return (unRet);
    }

    // �p���X���[�^�I��
    // 		�����F�f�o�C�X�n���h���C���g�ݍ��킹(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���ԍ�</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_PMOn(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = SVOFF;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ�0x01�`0xff�ȊO�̎��̓G���[
//		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		// ���ԍ�0x0001�`0xffff�ȊO�̎��̓G���[
		if ((usAxis <= 0) || (0xffff < usAxis))	return (ILLEGAL_PRM);
		
		// X1 - U1 : �w�P���Ƀp���X���[�^�n�m
		if ((usAxis & 0x000f) != 0)
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, (ushort)(((usAxis & 0x000f) << 8) | usCmd));
		}
		// X2 - U2 : �w�Q���Ƀp���X���[�^�n�m
		if (((usAxis & 0x00f0) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X2_AX, (ushort)(((usAxis & 0x00f0) << 4) | usCmd));
		}
		// X3 - U3 : �w�R���Ƀp���X���[�^�n�m
		if (((usAxis & 0x0f00) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X3_AX, (ushort)(((usAxis & 0x0f00) ) | usCmd));
		}
		// X4 - U4 : �w�S���Ƀp���X���[�^�n�m
		if (((usAxis & 0xf000) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, (ushort)(((usAxis & 0xf000) >> 4) | usCmd));
		}

		// X - U : �w���Ƀp���X���[�^�n�m
//		if ((usAxis & 0x0f) != 0)
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));
//		}
		// V - B : �u���Ƀp���X���[�^�n�m
//		if (((usAxis & 0xf0) != 0) && (unRet == 0))
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
//		}
        return (unRet);
    }

	/// <summary>
	/// �p���X���[�^�I�t
	/// </summary>
	/// <param name="hDevID">�f�o�C�X�n���h��</param>
	/// <param name="usAxis">���g�ݍ��킹(HEX)</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
    public static uint hcp530_PMOff(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = SVON;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ���ԍ�0x01�`0xff�ȊO�̎��̓G���[
//		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		// ���ԍ�0x0001�`0xffff�ȊO�̎��̓G���[
		if ((usAxis <= 0) || (0xffff < usAxis))	return (ILLEGAL_PRM);
		
		// X1 - U1 : �w�P���Ƀp���X���[�^�n�e�e
		if ((usAxis & 0x000f) != 0)
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, (ushort)(((usAxis & 0x000f) << 8) | usCmd));
		}
		// X2 - U2 : �w�Q���Ƀp���X���[�^�n�e�e
		if (((usAxis & 0x00f0) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X2_AX, (ushort)(((usAxis & 0x00f0) << 4) | usCmd));
		}
		// X3 - U3 : �w�R���Ƀp���X���[�^�n�e�e
		if (((usAxis & 0x0f00) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X3_AX, (ushort)(((usAxis & 0x0f00) ) | usCmd));
		}
		// X4 - U4 : �w�S���Ƀp���X���[�^�n�e�e
		if (((usAxis & 0xf000) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, (ushort)(((usAxis & 0xf000) >> 4) | usCmd));
		}

		// X - U : �w���Ƀp���X���[�^�n�e�e
//		if ((usAxis & 0x0f) != 0)
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));
//		}
        // V - B : �u���Ƀp���X���[�^�n�e�e
//		if (((usAxis & 0xf0) != 0) && (unRet == 0))
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
//		}
        return (unRet);
    }

	/// <summary>
	/// ���������[�g�̌v�Z
	/// </summary>
	/// <param name="unRate">����(����)���[�g���W�X�^�l�v�Z����</param>
	/// <param name="unTime">����(����)����(msec)</param>
	/// <param name="unRfh">���쑬�x���W�X�^�l</param>
	/// <param name="unRfl">�x�[�X���x���W�X�^�l</param>
	/// <param name="usAcc">�������`��</param>
	/// <param name="usS">�\��</param>
	/// <returns>0:����C0�ȊO:�ُ�</returns>
		
    public static uint hcp530_CalAccRate(ref uint unRate, uint unTime, uint unRfh, uint unRfl, ushort usAcc, ushort usS)
    {
        double dblData = 0;
        double dblTime = 0;
        uint unData = 0;

        if ((unRfl == 0) || (unRfh == 0) || (unRfh <= unRfl) || (unTime == 0)) 
        {
            return (ILLEGAL_PRM);
        }

        dblTime = (double)unTime / 1000;    // ��������(�b)

		if (usS == 0)
		{
			if (usAcc==0)
			{
				// �������������[�g:PRUR = 4915200 * (���������� / (FH - FL)) - 1)
				dblData = dblTime / (unRfh - unRfl);
				dblData = 4915200 * dblData - 1;
			}
			else if (usAcc == 1)
			{
				// ���������̂Ȃ��r�����������[�g:PRUR = 2457600 * (���������� / (FH - FL)) - 1)
				dblData = dblTime / (unRfh - unRfl);
				dblData = 2457600 * dblData - 1;
			}
			else
			{
				return (ILLEGAL_PRM);
			}
		}
		else if(usS<32767)
		{
			dblData = dblTime / (unRfh - unRfl + 2 * usS);
			dblData = 4915200 * dblData - 1;
		}
		else
		{
			return (ILLEGAL_PRM);	
		}

		unData = (uint)dblData;
		if ((unData == 0) || (65535 < unData)) 
		{
			return (ILLEGAL_PRM);
		} 
		else 
		{
			unRate = unData;
			return (0);
		}
    }
}