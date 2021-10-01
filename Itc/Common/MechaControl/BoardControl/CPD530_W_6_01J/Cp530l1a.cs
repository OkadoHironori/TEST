
/* 下記６種の関数レベルでのマルチスレッドの同期をする場合 */
/* public static uint hcp530_rReg(uint hDevID, ushort usAxis, byte byCmd, ref uint unReg) */
/* public static uint hcp530_wReg(uint hDevID, ushort usAxis, byte byCmd, uint unReg) */
/* public static uint hcp530_rBufDW(uint hDevID, ushort usAxis, ref uint unReg) */
/* public static uint hcp530_wBufDW(uint hDevID, ushort usAxis, uint unReg) */
/* public static uint hcp530_rPortB(uint hDevID, byte byCmd, ref byte byData) */
/* public static uint hcp530_rPortW(uint hDevID, byte byCmd, ref ushort usData) */
/* public static uint hcp530_wPortB(uint hDevID, byte byCmd, byte byData) */
/* public static uint hcp530_wPortW(uint hDevID, byte byCmd, ushort usData) */
/* 下のコメントの行を有効にします */
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
	/// X軸指定
	public const ushort X_AX = 0;
    /// Y軸指定
	public const ushort Y_AX = 1;
	/// Z軸指定
	public const ushort Z_AX = 2;
	/// U軸指定
	public const ushort U_AX = 3;
	/// V軸指定
	public const ushort V_AX = 4;    
	/// W軸指定	
	public const ushort W_AX = 5;
	/// A軸指定
	public const ushort A_AX = 6;
	/// B軸指定
	public const ushort B_AX = 7;    

	// ---------------------------------
	// for CPD5016 (2010.07.29)
	// ---------------------------------
	/// X1軸指定
	public const ushort X1_AX = 0;
	/// Y1軸指定
	public const ushort Y1_AX = 1;
	/// Z1軸指定
	public const ushort Z1_AX = 2;
	/// U1軸指定
	public const ushort U1_AX = 3;
	/// X2軸指定
	public const ushort X2_AX = 4;
	/// Y2軸指定	
	public const ushort Y2_AX = 5;
	/// Z2軸指定
	public const ushort Z2_AX = 6;
	/// U2軸指定
	public const ushort U2_AX = 7;
	/// X3軸指定
	public const ushort X3_AX = 8;
	/// Y3軸指定	
	public const ushort Y3_AX = 9;
	/// Z3軸指定
	public const ushort Z3_AX =10;
	/// U3軸指定
	public const ushort U3_AX =11;
	/// X4軸指定
	public const ushort X4_AX =12;
	/// Y4軸指定	
	public const ushort Y4_AX =13;
	/// Z4軸指定
	public const ushort Z4_AX =14;
	/// U4軸指定
	public const ushort U4_AX =15;

	/// 無効コマンド
	public const ushort NOP = 0x0000;		    
    
	/// ソフトウェアリセット
	public const ushort SRST = 0x0004;
	    
	/// CTR1(指令位置)リセット
	public const ushort CTR1R = 0x0020;
	/// CTR2(機械位置)リセット
	public const ushort CTR2R = 0x0021;
	/// CTR3(偏差)リセット    
	public const ushort CTR3R = 0x0022;
	/// CTR4(汎用)リセット
	public const ushort CTR4R = 0x0023;
	
	/// SVON ON
	public const ushort SVON = 0x0018;
	/// SVON OFF    
	public const ushort SVOFF = 0x0010;		    

	/// SVRST ON
	public const ushort SVRSTON = 0x0019;

	/// SVRST OFF
	public const ushort SVRSTOFF = 0x0011;

	/// SVCTRCL信号出力
	public const ushort CLROUT = 0x0024;
    /// SVCTRCL信号リセット
	public const ushort CLRRST = 0x0025;	    

	/// 動作用プリレジスタのキャンセル
	public const ushort PRECAN = 0x0026;        
	/// RCMP5用プリレジスタ(PRCP5)のキャンセル
	public const ushort PCPCAN = 0x0027;        
	/// 動作用プリレジスタデータのシフト
	public const ushort PRESHF = 0x002b;        
	/// RCMP5用プリレジスタ(PRCP5)データのシフト
	public const ushort PCPSHF = 0x002c;        
	/** コンパレータによる速度パターン変更データ
	として動作用プリレジスタを確定します。
	入力代行 */
	public const ushort PRESET = 0x004f;
    
	/// PCS入力代行
	public const ushort PCSON = 0x0028;
	/// LTC入力代行	    
	public const ushort LTCH = 0x0029;		    
	
	/// FL定速スタート
	public const ushort STAFL = 0x0050;
	/// FH定速スタート    
	public const ushort STAFH = 0x0051;
	/// FH定速スタート(FH定速->減速)    
	public const ushort STAFHD = 0x0052;
	/// 加速スタート(加速->FH定速->減速)    
	public const ushort STAUD = 0x0053;
	/// 残量FL定速スタート    
	public const ushort CNTFL = 0x0054;
	/// 残量FH定速スタート	    
	public const ushort CNTFH = 0x0055;
	/// 残量FH定速スタート(FH定速->減速)		    
	public const ushort CNTD = 0x0057;		    
	/// 残量加速スタート(加速->FH定速->減速)
	public const ushort CNTUD = 0x0057;
	/// STA出力(同時スタート)
	public const ushort CMSTA = 0x0006;
	/// STA入力代行
	public const ushort SPSTA = 0x002A;		    

	/// FL定速へ瞬時速度変更
	public const ushort FCHGL = 0x0040;		    
	/// FH定速へ瞬時速度変更	
	public const ushort FCHGH = 0x0041;		    
	/// FL速度まで減速	
	public const ushort FSCHL = 0x0042;		    
	/// FH速度まで加速	
	public const ushort FSCHH = 0x0043;		    
    
	/// 非常停止
	public const ushort CMEMG = 0x0005;		    
	/// STP出力(同時停止)
	public const ushort CMSTP = 0x0007;
	/// 即停止
	public const ushort STOP = 0x0049;
	/// 減速停止
	public const ushort SDSTP = 0x004A;		    

	/// PRMVレジスタ書き込み
	public const byte WPRMV = 0x80;	            
	/// PRFLレジスタ書き込み	
	public const byte WPRFL = 0x81;             
	/// PRFHレジスタ書き込み	
	public const byte WPRFH = 0x82;			    
	/// PRURレジスタ書き込み	
	public const byte WPRUR = 0x83;			    
	/// PRDRレジスタ書き込み	
	public const byte WPRDR = 0x84;			    
	/// PRMGレジスタ書き込み	
	public const byte WPRMG = 0x85;			    
	/// PRDPレジスタ書き込み	
	public const byte WPRDP = 0x86;			    
	/// PRMDレジスタ書き込み	
	public const byte WPRMD = 0x87;			    
	/// PRIPレジスタ書き込み	
	public const byte WPRIP = 0x88;			    
	/// PRUSレジスタ書き込み	
	public const byte WPRUS = 0x89;			    
	/// PRDSレジスタ書き込み	
	public const byte WPRDS = 0x8a;			    
	/// PRCP5レジスタ書き込み	
	public const byte WPRCP5 = 0x8b;		    
	/// PRCIレジスタ書き込み	
	public const byte WPRCI = 0x8c;			    
	/// RMVレジスタ書き込み	
	public const byte WRMV = 0x90;	            
	/// RFLレジスタ書き込み
	public const byte WRFL = 0x91;              
	/// RFHレジスタ書き込み
	public const byte WRFH = 0x92;			    
	/// RURレジスタ書き込み
	public const byte WRUR = 0x93;			    
	/// RDRレジスタ書き込み
	public const byte WRDR = 0x94;			    
	/// RMGレジスタ書き込み
	public const byte WRMG = 0x95;			    
	/// RDPレジスタ書き込み
	public const byte WRDP = 0x96;			    
	/// RMDレジスタ書き込み
	public const byte WRMD = 0x97;			    
	/// RIPレジスタ書き込み
	public const byte WRIP = 0x98;			    
	/// RUSレジスタ書き込み
	public const byte WRUS = 0x99;			    
	/// RDSレジスタ書き込み
	public const byte WRDS = 0x9a;			    
	/// RFAレジスタ書き込み
	public const byte WRFA = 0x9b;			    
	/// RENV1レジスタ書き込み
	public const byte WRENV1 = 0x9c;		    
	/// RENV2レジスタ書き込み
	public const byte WRENV2 = 0x9d;		    
	/// RENV3レジスタ書き込み
	public const byte WRENV3 = 0x9e;		    
	/// RENV4レジスタ書き込み
	public const byte WRENV4 = 0x9f;		    
	/// RENV5レジスタ書き込み
	public const byte WRENV5 = 0xa0;		    
	/// RENV6レジスタ書き込み
	public const byte WRENV6 = 0xa1;		    
	/// RENV7レジスタ書き込み
	public const byte WRENV7 = 0xa2;		    
	/// RCTR1レジスタ書き込み
	public const byte WRCTR1 = 0xa3;		    
	/// RCTR2レジスタ書き込み
	public const byte WRCTR2 = 0xa4;		    
	/// RCTR3レジスタ書き込み
	public const byte WRCTR3 = 0xa5;		    
	/// RCTR4レジスタ書き込み
	public const byte WRCTR4 = 0xa6;		    
	/// RCMP1レジスタ書き込み
	public const byte WRCMP1 = 0xa7;		    
	/// RCMP2レジスタ書き込み
	public const byte WRCMP2 = 0xa8;		    
	/// RCMP3レジスタ書き込み
	public const byte WRCMP3 = 0xa9;		    
	/// RCMP4レジスタ書き込み
	public const byte WRCMP4 = 0xaa;		    
	/// RCMP5レジスタ書き込み
	public const byte WRCMP5 = 0xab;		    
	/// RIRQレジスタ書き込み
	public const byte WRIRQ = 0xac;			    
	/// RCIレジスタ書き込み
	public const byte WRCI = 0xbc;
		            
	/// PRMVレジスタ読み出し
	public const byte RPRMV = 0xc0;
	/// PRFLレジスタ読み出し		    
	public const byte RPRFL = 0xc1;
	/// PRFHレジスタ読み出し		    
	public const byte RPRFH = 0xc2;
	/// PRURレジスタ読み出し		    
	public const byte RPRUR = 0xc3;			    
	/// PRDRレジスタ読み出し
	public const byte RPRDR = 0xc4;			    
	/// PRMGレジスタ読み出し
	public const byte RPRMG = 0xc5;			    
	/// PRDPレジスタ読み出し
	public const byte RPRDP = 0xc6;			    
	/// PRMDレジスタ読み出し
	public const byte RPRMD = 0xc7;			    
	/// PRMDレジスタ読み出し
	public const byte RPRIP = 0xc8;			    
	/// PRUSレジスタ読み出し
	public const byte RPRUS = 0xc9;			    
	/// PRDSレジスタ読み出し
	public const byte RPRDS = 0xca;			    
	/// PRCP5レジスタ読み出し
	public const byte RPRCP5 = 0xcb;		    
	/// PRCIレジスタ読み出し
	public const byte RPRCI = 0xcc;		        
	/// RMVレジスタ読み出し
	public const byte RRMV = 0xd0;			    
	/// RFLレジスタ読み出し
	public const byte RRFL = 0xd1;			    
	/// RFHレジスタ読み出し
	public const byte RRFH = 0xd2;			    
	/// RURレジスタ読み出し
	public const byte RRUR = 0xd3;			    
	/// RDRレジスタ読み出し
	public const byte RRDR = 0xd4;			    
	/// RMGレジスタ読み出し
	public const byte RRMG = 0xd5;			    
	/// RDPレジスタ読み出し
	public const byte RRDP = 0xd6;			    
	/// RMDレジスタ読み出し
	public const byte RRMD = 0xd7;			    
	/// RMDレジスタ読み出し
	public const byte RRIP = 0xd8;			    
	/// RUSレジスタ読み出し
	public const byte RRUS = 0xd9;			    
	/// RDSレジスタ読み出し
	public const byte RRDS = 0xda;			    
	/// RFAレジスタ読み出し
	public const byte RRFA = 0xdb;			    
	/// RENV1レジスタ読み出し
	public const byte RRENV1 = 0xdc;		    
	/// RENV2レジスタ読み出し
	public const byte RRENV2 = 0xdd;		    
	/// RENV3レジスタ読み出し
	public const byte RRENV3 = 0xde;		    
	/// RENV4レジスタ読み出し
	public const byte RRENV4 = 0xdf;		    
	/// RENV5レジスタ読み出し
	public const byte RRENV5 = 0xe0;		    
	/// RENV6レジスタ読み出し
	public const byte RRENV6 = 0xe1;		    
	/// RENV7レジスタ読み出し
	public const byte RRENV7 = 0xe2;		    
	/// RCTR1レジスタ読み出し
	public const byte RRCTR1 = 0xe3;		    
	/// RCTR2レジスタ読み出し
	public const byte RRCTR2 = 0xe4;		    
	/// RCTR3レジスタ読み出し
	public const byte RRCTR3 = 0xe5;		    
	/// RCTR4レジスタ読み出し
	public const byte RRCTR4 = 0xe6;		    
	/// RCMP1レジスタ読み出し
	public const byte RRCMP1 = 0xe7;		    
	/// RCMP2レジスタ読み出し
	public const byte RRCMP2 = 0xe8;		    
	/// RCMP3レジスタ読み出し
	public const byte RRCMP3 = 0xe9;		    
	/// RCMP4レジスタ読み出し
	public const byte RRCMP4 = 0xea;		    
	/// RCMP5レジスタ読み出し
	public const byte RRCMP5 = 0xeb;		    
	/// RIRQレジスタ読み出し
	public const byte RRIRQ = 0xec;			    
	/// RLTC1レジスタ読み出し
	public const byte RRLTC1 = 0xed;		    
	/// RLTC2レジスタ読み出し
	public const byte RRLTC2 = 0xee;		    
	/// RLTC3レジスタ読み出し
	public const byte RRLTC3 = 0xef;		    
	/// RLTC4レジスタ読み出し
	public const byte RRLTC4 = 0xf0;		    
	/// RSTSレジスタ読み出し
	public const byte RRSTS = 0xf1;
    /// RESTレジスタ読み出し//追記　回転エラーの詳細ステータス
    public const byte RREST = 0xf2;			    
	/// RISTレジスタ読み出し
	public const byte RRIST = 0xf3;			    
	/// RPLSレジスタ読み出し
	public const byte RRPLS = 0xf4;			  
	/// RSPDレジスタ読み出し
	public const byte RRSPD = 0xf5;			    
	/// RSDCレジスタ読み出し
	public const byte RRSDC = 0xf6;			    
	/// RCIレジスタ読み出し
	public const byte RRCI = 0xfc;		        
	/// RCICレジスタ読み出し
	public const byte RRCIC = 0xfd;		        
	/// RIPSレジスタ読み出し
	public const byte RRIPS = 0xff;			    

	/// MSTS スタートコマンド書き込み済みビット
	public const ushort M_SCM = 0x0001;         
	/// MSTS 動作中ビット
	public const ushort M_RUN = 0x0002;         
	/// MSTS 停止中ビット
	public const ushort M_END = 0x0008;         
	/// MSTS エラー報告ビット
	public const ushort M_ERR = 0x0010;         
	/// MSTS イベント報告ビット
	public const ushort M_INT = 0x0020;         
	/// MSTS シーケンス番号ビット
	public const ushort M_SCx = 0x00C0;         
	/// MSTS CMP1条件成立中ビット
	public const ushort M_CMP1 = 0x0100;        
	/// MSTS CMP2条件成立中ビット
	public const ushort M_CMP2 = 0x0200;        
	/// MSTS CMP3条件成立中ビット
	public const ushort M_CMP3 = 0x0400;        
	/// MSTS CMP4条件成立中Mビット
	public const ushort M_CMP4 = 0x0800;        
	/// MSTS CMP5条件成立中ビット
	public const ushort M_CMP5 = 0x1000;        
	/// MSTS 次動作用プリレジスタフルビット
	public const ushort M_PRF = 0x4000;         
	/// MSTS CMP5用プリレジスタフルビット
	public const ushort M_PDF = 0x8000;         

	/// SSTS SVONビット
	public const ushort S_SVON = 0x0001;        
	/// SSTS SVRSTビット
	public const ushort S_SVRS = 0x0002;        
	/// SSTS 加速中ビット
	public const ushort S_FU = 0x0100;          
	/// SSTS 減速中ビット
	public const ushort S_FD = 0x0200;          
	/// SSTS 定速動作中ビット
	public const ushort S_FC = 0x0400;          
	/// SSTS SVALMビット
	public const ushort S_ALM = 0x0800;         
	/// SSTS +ELSビット
	public const ushort S_PEL = 0x1000;         
	/// SSTS -ELSビット
	public const ushort S_MEL = 0x2000;         
	/// SSTS OLSビット
	public const ushort S_OLS = 0x4000;         
	/// SSTS DLSビット
	public const ushort S_DLS = 0x8000;         

	/// RSTS 動作方向ビット 0:+，1:-
	public const uint R_DIR = 0x00000010;       
	/// RSTS STAビット
	public const uint R_STA = 0x00000020;       
	/// RSTS STPビット
	public const uint R_STP = 0x00000040;       
	/// RSTS EMGビット
	public const uint R_EMG = 0x00000080;       
	/// RSTS PCSビット
	public const uint R_PCS = 0x00000100;       
	/// RSTS SVCTRCLビット
	public const uint R_ERC = 0x00000200;       
	/// RSTS Z相ビット
	public const uint R_EZ = 0x00000400;        
	/// RSTS +DRビット
	public const uint R_DRP = 0x00000800;       
	/// RSTS -DRビット
	public const uint R_DRM = 0x00001000;       
	/// RSTS INPOSビット
	public const uint R_INPOS = 0x0010000;      

	/// EST CMP1(+SLS)による停止
	public const uint ES_C1 = 0x00000001;
    /// EST CMP2(-SLS)による停止   
	public const uint ES_C2 = 0x00000002;       
	/// EST CMP3による停止
	public const uint ES_C3 = 0x00000004;       
	/// EST CMP4による停止
	public const uint ES_C4 = 0x00000008;       
	/// EST CMP5による停止
	public const uint ES_C5 = 0x00000010;       
	/// EST +ELSによる停止
	public const uint ES_PL = 0x00000020;       
	/// EST -ELSによる停止
	public const uint ES_ML = 0x00000040;       
	/// EST SVALMによる停止
	public const uint ES_AL = 0x00000080;       
	/// EST STPによる停止
	public const uint ES_SP = 0x00000100;       
	/// EST EMGによる停止
	public const uint ES_EM = 0x00000200;       
	/// EST DLSによる停止
	public const uint ES_SD = 0x00000400;       
	/// EST 補間データ異常による停止
	public const uint ES_DT = 0x00001000;       
	/// EST 補間他軸による停止
	public const uint ES_IP = 0x00002000;       
	/// EST パルサ入力バッファオーバーフロー
	public const uint ES_PO = 0x00004000;       
	/// EST 補間データレンジオーバー
	public const uint ES_AO = 0x00008000;       
	/// EST エンコーダ信号エラー
	public const uint ES_EE = 0x00010000;       
	/// EST パルサ入力信号エラー
	public const uint ES_PE = 0x00020000;       

	/// IST 正常停止
	public const uint IS_EN = 0x00000001;       
	/// IST 次動作スタート
	public const uint IS_NN = 0x00000002;       
	/// IST 動作用プリレジスタフル→空き
	public const uint IS_NM = 0x00000004;       
	/// IST PRCP5フル→空き
	public const uint IS_ND = 0x00000008;       
	/// IST 加速開始
	public const uint IS_US = 0x00000010;       
	/// IST 加速終了
	public const uint IS_UE = 0x00000020;       
	/// IST 減速開始
	public const uint IS_DS = 0x00000040;       
	/// IST 減速終了
	public const uint IS_DE = 0x00000080;       
	/// IST CMP1条件成立
	public const uint IS_C1 = 0x00000100;       
	/// IST CMP2条件成立
	public const uint IS_C2 = 0x00000200;       
	/// IST CMP3条件成立
	public const uint IS_C3 = 0x00000400;       
	/// IST CMP4条件成立
	public const uint IS_C4 = 0x00000800;       
	/// IST CMP5条件成立
	public const uint IS_C5 = 0x00001000;       
	/// IST OLSonによるラッチ
	public const uint IS_OL = 0x00008000;       
	/// IST DLS入力OFF→ON
	public const uint IS_SD = 0x00010000;       
	/// IST +DR入力OFF→ON
	public const uint IS_PD = 0x00020000;       
	/// IST -DR入力OFF→ON
	public const uint IS_MD = 0x00040000;       
	/// IST STA入力OFF→ON
	public const uint IS_SA = 0x00080000;       

	/// DLS有効設定ビット(PRMD)
	public const uint MD_DLSE_BIT = 0x00000100;
	/// INPOS制御有効設定ビット(PRMD)	
	public const uint MD_INPE_BIT = 0x00000200;
	/// 加減速方式設定ビット(PRMD) 		
	public const uint MD_ACC_BIT = 0x00000400;
	/// 減速開始点計算方法設定ビット(PRMD) 		
	public const uint MD_SDP_BIT = 0x00002000;
	/// PCS有効設定ビット(PRMD) 		
	public const uint MD_PCSE_BIT = 0x00004000;
	/// スタート制御設定ビット0(PRMD) 		
	public const uint MD_MSY0_BIT = 0x00040000;
	/// スタート制御設定ビット1(PRMD) 		
	public const uint MD_MSY1_BIT = 0x00080000;
	/// STP入力有効設定ビット(PRMD) 		
	public const uint MD_STPE_BIT = 0x01000000;	 		

	/// ELS検出時動作設定ビット(RENV1)
	public const uint R1_ELSM_BIT = 0x00000008;	 		
	/// DLS検出時動作設定ビット(RENV1)
	public const uint R1_DLSM_BIT = 0x00000010;	 		
	/// DLSラッチ動作設定ビット(RENV1) 
	public const uint R1_DLLT_BIT = 0x00000020;			
	/// DLS入力論理設定ビット(RENV1)
	public const uint R1_DLSL_BIT = 0x00000040;	 		
	/// OLS入力論理設定ビット(RENV1)
	public const uint R1_OLSL_BIT = 0x00000080;	 		
	/// SVALM検出時動作設定ビット(RENV1)
	public const uint R1_ALMM_BIT = 0x00000100;	 		
	/// SVALM入力論理設定ビット(RENV1) 
	public const uint R1_ALML_BIT = 0x00000200;			
	/// 異常停止時SVCTRCL自動出力設定ビット(RENV1) 	
	public const uint R1_CLRE_BIT = 0x00000400;		
	/// 原点復帰完了時SVCTRCL自動出力設定ビット(RENV1)
	public const uint R1_CLRO_BIT = 0x00000800;	 		
	/// STP入力時停止方法設定ビット(RENV1)
	public const uint R1_CSTP_BIT = 0x00080000; 
	/// INPOS入力論理設定ビット(RENV1)
	public const uint R1_INPL_BIT = 0x00400000;	 		
	/// PCS入力論理設定ビット(RENV1)
	public const uint R1_PCSL_BIT = 0x01000000;	 		
	/// EZ入力論理設定ビット(RENV2)
	public const uint R2_EZL_BIT = 0x00800000;

    /// HPCI-CPD553 デジタルフィルタ時間設定ポート
    public const byte DIGIFIL = 0x40;
    /// HPCI-CPD553 デジタルフィルタ有効選択ポート
    public const byte DIGISEL = 0x42;
    /// HPCI-CPD553 ラッチビット選択ポート
    public const byte LATENA = 0x44;
    /// HPCI-CPD553 ラッチ入力信号エッジ選択Lバイトポート
    public const byte LATEDGL = 0x48;
    /// HPCI-CPD553 ラッチ入力信号エッジ選択Hバイトポート
    public const byte LATEDGH = 0x4a;
    /// HPCI-CPD553 ラッチ信号クリアポート
    public const byte LATSTS = 0x4c;
    /// HPCI-CPD553 イベントタイマポート
    public const byte EVENTTM = 0x50;
    /// HPCI-CPD553 特殊出力選択ポート
    public const byte DO_SEL = 0x52;
    /// HPCI-CPD553 特殊入力選択ポート
    public const byte DI_U = 0x54;
    /// HPCI-CPD553 出力ポート1
    public const byte DO1_PRT = 0x62;
    /// HPCI-CPD553 出力ポート2
    public const byte DO2_PRT = 0x63;
    /// HPCI-CPD553 DIOビット選択ポート
    public const byte DIOBIT = 0x7F;
	/// ELS極性選択
	public const byte ELPOL = 0x80;     
    /// DLS/PCS切替
	public const byte DLS_PCS = 0x82;	
    /// CMP4→STA
	public const byte C4STA = 0x84;	    
    /// CMP5→STP
	public const byte C5STP = 0x86;	    
    /// HPCI-CPD508 BOLS/PCS切替
	public const byte BOL_PCS = 0x88;
    /// HPCI-CPD553 CMP出力選択ポート
    public const byte COUT = 0x88;
    /// HPCI-CPD508 SVALM/DI/EMG切替
	public const byte INP_SEL = 0x8a;
    /// HPCI-CPD578 J3出力マスク
	public const byte J3_OUT = 0x8a;
    /// HPCI-CPD553 エンコーダフィルタOFF選択設定ポート
    public const byte ENCFIL = 0x8a;
    /// HPCI-CPD578 X-U軸CMP出力切替
	public const byte COTSEL1 = 0x8c;	
    /// HPCI-CPD578 V-B軸CMP出力切替
	public const byte COTSEL2 = 0x8e;
	/// HPCI-CPD578N 割込出力マスクポート
	public const byte BINTM = 0x90;
    /// HPCI-CPD578N X,V軸同期用CMP許可
	public const byte SYNC_C_EN = 0x94;
	/// HPCI-CPD553 割込要因ビット選択ポート
	public const byte INTENA = 0x94;
    /// HPCI-CPD578N X同期用CMP選択
	public const byte XSYNC_C = 0x96;
	/// HPCI-CPD553 入力信号エッジ選択Lバイトポート
	public const byte INTEDGL = 0x96;
    /// HPCI-CPD578N V同期用CMP選択
	public const byte VSYNC_C = 0x98;
    /// HPCI-CPD553 入力信号エッジ選択Hバイトポート
	public const byte INTEDGH = 0x98;
    /// HPCI-CPD553 割込クリアポート
    public const byte INTCLR = 0x9a;	
    /// HPCI-CPD578N エンコーダフィルタ設定
	public const byte ENFIL = 0xa2;	    
    /// HPCI-CPD578N 外部パルサ入出力設定
	public const byte J3_SEL = 0xa4;	
    /// HPCI-CPD578N 軸数判別
	public const byte AXIS_SEL = 0xa8;	
    /// HPCI-CPD578N X-U軸同期機能設定
	public const byte SYNC_SET1 = 0xf0;	
    /// HPCI-CPD578N V-B軸同期機能設定
	public const byte SYNC_SET2 = 0xf2;	

    /// 不正なパラメータ
    public const uint ILLEGAL_PRM = 0x0100;
	/// ボードコードが異常
    public const uint OTHER_BOARD = 0x0200;
	/// 不正なハンドル
    public const uint ILLEGAL_HANDLE = uint.MaxValue;

#if APP_SYNC
	static System.Object[] thisLock =new System.Object[16];
#endif

	/// <summary>
	/// レジスタ読出
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="byCmd">レジスタ読出しコマンド</param>
	/// <param name="unReg">レジスタから読み出されたデータ格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
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
	/// レジスタ書込関数
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="byCmd">レジスタ書き込みコマンド</param>
	/// <param name="unReg">レジスタへ書き込むデータ</param>
	/// <returns>0:正常，0以外:異常</returns>
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
	/// 入出力バッファ読出し
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="unReg">入出力バッファから読み出されたデータ格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
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
	/// 入出力バッファ書込み
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="unReg">入出力バッファへ書込むデータ</param>
	/// <returns>0:正常，0以外:異常</returns>
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
	/// オプションポート読出し(バイト)
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="byCmd">読み出しコマンド</param>
	/// <param name="byData">オプションポートから読み出されたデータ格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
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
	/// オプションポート読出し(ワード)
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="byCmd">読み出しコマンド</param>
	/// <param name="usData">オプションポートから読み出されたデータ格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
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
	/// オプションポート書込み(バイト)
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="byCmd">書き込みコマンド</param>
	/// <param name="byData">オプションポートへ書込むデータ</param>
	/// <returns>0:正常，0以外:異常</returns>
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
	/// オプションポート書込み(ワード)
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="byCmd">書き込みコマンド</param>
	/// <param name="usData">オプションポートへ書込むデータ</param>
	/// <returns>0:正常，0以外:異常</returns>
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
	/// ボード枚数，デバイス情報の取得
	/// </summary>
	/// <param name="unCnt">ボード枚数の格納先</param>
	/// <param name="hpcInfo">デバイス情報の格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_GetDevInfo(ref uint unCnt, [In, Out, MarshalAs(UnmanagedType.LPArray)] Hicpd530.HPCDEVICEINFO[] hpcInfo)
    {
        uint cnt = 0,
            i,
            unRet = 0;

        Hicpd530.HPCDEVICEINFO[] h = new Hicpd530.HPCDEVICEINFO[16];

        unRet = Hicpd530.cp530_GetDeviceCount(ref cnt);				// ボード枚数取得
		if (unRet != 0)            return (unRet);

        // ボード枚数が0枚または16枚以上の時はエラー
        if ((0 == cnt) || (16 <= cnt))
        {
            return (ILLEGAL_PRM);
        }

        unCnt = cnt;
        unRet = Hicpd530.cp530_GetDeviceInfo(ref cnt, h);	// デバイス情報取得
		if (unRet != 0)            return (unRet);

        for (i = 0; i < cnt; i++)
        {
            hpcInfo[i] = h[i];
        }
        return (unRet);
    }

	/// <summary>
	/// デバイスのオープン，デバイスの初期化
	/// </summary>
	/// <param name="hDevID">デバイスハンドルの格納先</param>
	/// <param name="hInfo">デバイス情報の格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
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

        // ボード判別
        unRet = Hicpd530.cp530_GetBoardCode(h, ref usCode);

        // オプションポート設定
        switch (usCode)
        {
            case 0x578a:
                unRet |= hcp530_rPortB(h, AXIS_SEL, ref byAxis);        // ボード軸数読み出し
                if (byAxis == 4)
                {
                    unRet |= hcp530_wPortB(h, ELPOL, 0x00);		        // ELS極性選択ポート書込(B接)
                    unRet |= hcp530_wPortB(h, DLS_PCS, 0x0f);		    // DLS/PCS切替ポート書込(PCS)
                    unRet |= hcp530_wPortB(h, C4STA, 0x00);		        // コンパレータ4出力切替ポート書込(出力不可)
                    unRet |= hcp530_wPortB(h, C5STP, 0x00);		        // コンパレータ5出力切替ポート書込(出力不可)  
                    unRet |= hcp530_wPortB(h, DLS_PCS, 0x0f);	        // DLS/PCS切替ポート書込(PCS)
                    unRet |= hcp530_wPortB(h, COTSEL1, 0xff);	        // X-U軸CMP出力切替ポート(出力不可)
                    unRet |= Hicpd530.cp530_wCmdW(h, X_AX, SRST);	    // X-U軸ソフトウェアリセット
                    usMax = 4;
                }
                else if (byAxis == 8)
                {
                    unRet |= hcp530_wPortB(h, ELPOL, 0x00);		        // ELS極性選択ポート書込(B接)
                    unRet |= hcp530_wPortB(h, DLS_PCS, 0xff);		    // DLS/PCS切替ポート書込(PCS)
                    unRet |= hcp530_wPortB(h, C4STA, 0x00);		        // コンパレータ4出力切替ポート書込(出力不可)
                    unRet |= hcp530_wPortB(h, C5STP, 0x00);		        // コンパレータ5出力切替ポート書込(出力不可)
                    unRet |= hcp530_wPortB(h, DLS_PCS, 0xff);		    // DLS/PCS切替ポート書込(PCS)
                    unRet |= hcp530_wPortB(h, COTSEL1, 0xff);		    // X-U軸CMP出力切替ポート(出力不可)
                    unRet |= hcp530_wPortB(h, COTSEL2, 0xff);		    // V-B軸CMP出力切替ポート(出力不可)
                    unRet |= Hicpd530.cp530_wCmdW(h, X_AX, SRST);	    // X-U軸ソフトウェアリセット
                    unRet |= Hicpd530.cp530_wCmdW(h, V_AX, SRST);	    // V-B軸ソフトウェアリセット
                    usMax = 8;
                }
                else
                {
                    unRet |= hcp530_wPortB(h, ELPOL, 0x00);		        // ELS極性選択ポート書込(B接)
                    unRet |= hcp530_wPortB(h, DLS_PCS, 0xff);		    // DLS/PCS切替ポート書込(PCS)
                    unRet |= hcp530_wPortB(h, C4STA, 0x00);		        // コンパレータ4出力切替ポート書込(出力不可)
                    unRet |= hcp530_wPortB(h, C5STP, 0x00);		        // コンパレータ5出力切替ポート書込(出力不可)
                    unRet |= hcp530_wPortB(h, J3_OUT, 0x00);		    // J3出力マスクポート(出力マスク)
                    unRet |= hcp530_wPortB(h, COTSEL1, 0xff);		    // X-U軸CMP出力切替ポート(出力不可)
                    unRet |= hcp530_wPortB(h, COTSEL2, 0xff);		    // V-B軸CMP出力切替ポート(出力不可)
                    unRet |= Hicpd530.cp530_wCmdW(h, X_AX, SRST);	    // X-U軸ソフトウェアリセット
                    unRet |= Hicpd530.cp530_wCmdW(h, V_AX, SRST);	    // V-B軸ソフトウェアリセット
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
                    unRet = hcp530_wPortB(h, ELPOL, 0x00);		        // ELS極性選択ポート書込(B接)
                    unRet |= hcp530_wPortB(h, DLS_PCS, 0x03);		    // DLS/PCS切替ポート書込(PCS)
                    unRet |= hcp530_wPortB(h, C4STA, 0x00);		        // コンパレータ4出力切替ポート書込(出力不可)
                    unRet |= hcp530_wPortB(h, C5STP, 0x00);		        // コンパレータ5出力切替ポート書込(出力不可)
                    unRet |= Hicpd530.cp530_wCmdW(h, X_AX, SRST);	    // X-Y軸ソフトウェアリセット
                }
                else if (usMax == 4)
                {							// HPCI-CPD534
                    unRet = hcp530_wPortB(h, ELPOL, 0x00);		        // ELS極性選択ポート書込(B接)
                    unRet |= hcp530_wPortB(h, DLS_PCS, 0x0f);		    // DLS/PCS切替ポート書込(PCS)
                    unRet |= hcp530_wPortB(h, C4STA, 0x00);		        // コンパレータ4出力切替ポート書込(出力不可)
                    unRet |= hcp530_wPortB(h, C5STP, 0x00);		        // コンパレータ5出力切替ポート書込(出力不可)
                    unRet |= Hicpd530.cp530_wCmdW(h, X_AX, SRST);	    // X-U軸ソフトウェアリセット
                }
                else if (usMax == 8)
                {							// HPCI-CPD508
                    unRet = hcp530_wPortB(h, ELPOL, 0x00);		        // ELS極性選択ポート書込(B接)
                    unRet |= hcp530_wPortB(h, C4STA, 0x00);		        // コンパレータ4出力切替ポート書込(出力不可)
                    unRet |= hcp530_wPortB(h, BOL_PCS, 0x00);		    // BOLS/PCS切替ポート書込(BOLS)
                    unRet |= hcp530_wPortB(h, INP_SEL, 0x00);		    // SVALM/DI/EMG切替ポート書込(SVALM)
                    unRet |= Hicpd530.cp530_wCmdW(h, X_AX, SRST);	    // X-U軸ソフトウェアリセット
                    unRet |= Hicpd530.cp530_wCmdW(h, V_AX, SRST);	    // V-B軸ソフトウェアリセット
                }
                break;
            case 0x5016:					// HPCI-CPD5016
			    unRet = hcp530_wPortW(h, ELPOL, 0x0000);			    // ELS極性選択ポート書込(B接)
			    unRet |= hcp530_wPortW(h, C4STA, 0x0000);			    // コンパレータ4出力切替ポート書込(出力不可)
			    unRet |= hcp530_wPortW(h, BOL_PCS, 0x0000);			    // BOLS/PCS切替ポート書込(BOLS)
			    unRet |= hcp530_wPortW(h, INP_SEL, 0x0000);		        // SVALM/DI/EMG切替ポート書込(SVALM)
                unRet |= Hicpd530.cp530_wCmdW(h, X1_AX, SRST);	        // X1-U1軸ソフトウェアリセット
                unRet |= Hicpd530.cp530_wCmdW(h, X2_AX, SRST);	        // X2-U2軸ソフトウェアリセット
                unRet |= Hicpd530.cp530_wCmdW(h, X3_AX, SRST);	        // X3-U3軸ソフトウェアリセット
                unRet |= Hicpd530.cp530_wCmdW(h, X4_AX, SRST);	        // X4-U4軸ソフトウェアリセット
                usMax = 16;
                break;
            case 0x5530:					// HPCI-CPD553
                unRet = hcp530_wPortB(h, DIGIFIL, 0x00);		// デジタルフィルタ時間設定ポート(無効)
                unRet |= hcp530_wPortB(h, DIGISEL, 0x00);		// デジタルフィルタ有効選択ポート(無効)
                unRet |= hcp530_wPortW(h, LATENA, 0x0000);		// ラッチビット選択ポート(無効)
                unRet |= hcp530_wPortW(h, LATEDGL, 0x0000);		// ラッチ入力信号エッジ選択Lバイトポート(立上りッジ)
                unRet |= hcp530_wPortW(h, LATEDGH, 0x0000);		// ラッチ入力信号エッジ選択Hバイトポート(立上りッジ)
                unRet |= hcp530_wPortW(h, LATSTS, 0x0000);		// ラッチ信号クリアポート
                unRet |= hcp530_wPortB(h, EVENTTM, 0x00);		// イベントタイマポート(無効)
                unRet |= hcp530_wPortW(h, DO_SEL, 0x0000);		// 特殊出力選択ポート(通常DO)
                unRet |= hcp530_wPortW(h, DI_U, 0x0000);		// 特殊入力選択ポート(INをU軸エンコーダA相として使用)
                unRet |= hcp530_wPortB(h, DO1_PRT, 0x00);		// 出力ポート1
                unRet |= hcp530_wPortB(h, DO2_PRT, 0x00);		// 出力ポート2
                unRet |= hcp530_wPortB(h, DIOBIT, 0x00);		// DIOビット選択ポート(16IN/8OUT)
                unRet |= hcp530_wPortB(h, ELPOL, 0x00);			// ELS極性選択ポート書込(B接)
                unRet |= hcp530_wPortB(h, DLS_PCS, 0x0f);		// DLS/PCS切替ポート書込(PCS)
                unRet |= hcp530_wPortB(h, C4STA, 0x00);			// コンパレータ4出力切替ポート書込(出力不可)
                unRet |= hcp530_wPortB(h, C5STP, 0x00);			// コンパレータ5出力切替ポート書込(出力不可)
                unRet |= hcp530_wPortW(h, COUT, 0xffff);		// CMP出力選択ポート書込(出力不可)
                unRet |= hcp530_wPortW(h, ENCFIL, 0x0000);		// エンコーダフィルタOFF選択設定ポート(FILTER ON)
                unRet |= hcp530_wPortW(h, BINTM, 0x00);			// 割込イネーブルポート書込(出力不可)
                unRet |= hcp530_wPortW(h, INTENA, 0x0000);		// 割込要因ビット選択ポート(無効)
                unRet |= hcp530_wPortW(h, INTEDGL, 0x0000);		// 入力信号エッジ選択Lバイトポート(立上りッジ)
                unRet |= hcp530_wPortW(h, INTEDGH, 0x0000);		// 入力信号エッジ選択Hバイトポート(立上りッジ)
                unRet |= hcp530_wPortW(h, INTCLR, 0x0000);		// 割込クリアポート
                unRet |= Hicpd530.cp530_wCmdW(h, X_AX, SRST);	// X-Z軸ソフトウェアリセット
                usMax = 3;
				break;
            default:
                return OTHER_BOARD;
        }

        // レジスタ初期化
        for (usAx = X1_AX; usAx < usMax; usAx++)	// for CPD5016 (2010.07.29)
        {	
			// レジスタ書込(ベース速度レジスタ:PRFL = 200)
            unRet |= hcp530_wReg(h, usAx, WPRFL, 200);

            // レジスタ書込(動作速度レジスタ:PRFH = 2000)
            unRet |= hcp530_wReg(h, usAx, WPRFH, 2000);

            // レジスタ書込(直線加速時加速レートレジスタ)
            // 		基準クロック周波数 = 19,660,800Hz, 加速時間 = 減速時間 = 0.5秒
            // 		PRFH = 2000, PRFL = 200, PRUR:加速レート
            // 		加速時間(秒) = (PRFH - PRFL) * (PRUR + 1) * 4 / 19660800
            // 		PRUR = 1364
            unRet |= hcp530_wReg(h, usAx, WPRUR, 1364);

            // レジスタ書込(倍率 = 1倍, PRMG = 299)
            unRet |= hcp530_wReg(h, usAx, WPRMG, 299);

            // レジスタ書込(動作モード:PRMD = 0x8008000)
            unRet |= hcp530_wReg(h, usAx, WPRMD, 0x8008000);

            // レジスタ書込(移動量補正速度:RFA = 200)
            unRet |= hcp530_wReg(h, usAx, WRFA, 200);

            // レジスタ書込(環境設定1:RENV1 = 0x20434004)
            // 		指令出力:個別パルス指令の正論理
            // 		OLS & DLS & SVALM = B接, INPOS = A接, 
            // 		ELS & SVALM入力時即停止,DLS検出時動作 = 減速のみ,ラッチしない,
            // 		ｻｰﾎﾞｴﾗｰ､原点復帰完了ｶｳﾝﾀｸﾘｱ出力しない
            unRet |= hcp530_wReg(h, usAx, WRENV1, 0x20434004);

            // 	汎用入出力設定等
            if (usCode == 0x578a)
            {	// HPCI-CPD578
                // レジスタ書込(環境設定2:RENV2 = 0x0020fd55)
                unRet |= hcp530_wReg(h, usAx, WRENV2, 0x0020fd55);
            }
            else if (usCode == 0x5254)
            {	// HPCI-CPD534,532,508
                // レジスタ書込(環境設定2:RENV2 = 0x0020f555)
                unRet |= hcp530_wReg(h, usAx, WRENV2, 0x0020f555);
            }
            else if (usCode == 0x5016)
            {	// HPCI-CPD5016
                // レジスタ書込(環境設定2:RENV2 = 0x0020f555)
                unRet |= hcp530_wReg(h, usAx, WRENV2, 0x0020f555);
            }
            else if (usCode == 0x5530)
            {	// HPCI-CPD553
                // レジスタ書込(環境設定2:RENV2 = 0x0020f555)
                unRet |= hcp530_wReg(h, usAx, WRENV2, 0x0020f555);
            }

            // レジスタ書込(環境設定3:RENV3 = 0x00f00002)
            // 		原点復帰：原点センサ+Z相原点復帰, 
            // 		CLR入力OFF→ON & 原点復帰完了時カウンタクリア
            unRet |= hcp530_wReg(h, usAx, WRENV3, 0x00f00002);

            // レジスタ書込(イベント設定:RIRQ = 1(正常停止)
            unRet |= Cp530l1a.hcp530_wReg(h, usAx, WRIRQ, 1);

            // SVON OFF
            unRet |= Hicpd530.cp530_wCmdW(h, usAx, 0x10);

            // SVRST OFF
            unRet |= Hicpd530.cp530_wCmdW(h, usAx, 0x11);

            // 即停止
            unRet |= Hicpd530.cp530_wCmdW(h, usAx, 0x49);
        }
        return (unRet);
    }

	/// <summary>
	/// デバイスのクローズ
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_DevClose(uint hDevID)
    {
        uint unRet = 0;

        unRet = Hicpd530.cp530_CloseDevice(hDevID);
        return (unRet);
    }

	/// <summary>
	/// 原点復帰方式の設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="usMode">原点復帰モード</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SetOrgMode(uint hDevID, ushort usAxis, ushort usMode)
    {
        uint unRet = 0;
        uint unRenv3 = 0;
        const uint CTR2_SRC = 0x00000300;
        const uint ORG_MODE = 0x0000000F;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～15以外または 
        // 原点復帰モードが0～12以外の時は引数エラー 
//      if ((usAxis < 0) || (7 < usAxis) || 
		if ((usAxis < 0) || (15 < usAxis ) ||
            (usMode < 0) || (12 < usMode))
        {
            return (ILLEGAL_PRM);
        }

        // 環境レジスタ3(RENV3)読込み 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV3, ref unRenv3);
		if (unRet != 0)            return (unRet);

        // Orgmode9の時，CTR2入力を指令位置にする 
        // (Orgmode9以外の時，CTR2入力は機械位置推奨) 
        if (usMode == 9)
        {
            unRenv3 &= (~CTR2_SRC);
            unRenv3 |= 0x00000100;
        }
        unRenv3 &= (~ORG_MODE);
        unRenv3 |= usMode;

        // 環境レジスタ3(RENV3)書込み 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV3, unRenv3);
        return (unRet);
    }

	/// <summary>
	/// ソフトリミットの設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="nPsl">+SLS</param>
	/// <param name="nMsl">-SLS</param>
	/// <param name="usEnable">SLS使用/不使用</param>
	/// <param name="usStp">停止方法</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SetSls(uint hDevID, ushort usAxis, int nPsl, int nMsl, ushort usEnable, ushort usStp)
    {
        uint unRet = 0;
        uint unRenv4 = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～7以外 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// 軸番号が0～15以外 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

		// ソフトリミット使用時に+SLS < -SLSの時は引数エラー 
		if ((usEnable != 0) && (nPsl <= nMsl))	return (ILLEGAL_PRM);

        // 環境レジスタ4(RENV4)読込み 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV4, ref unRenv4);
        if (unRet != 0)            return (unRet);

		switch (usEnable)
		{
			case 0:
				// SLS不使用 
				unRenv4 &= 0xffffe3e3;
				break;
			case 1:
				// SLS使用 
				unRenv4 &= 0xffff0000;
				if (usStp == 0)			unRenv4 |= 0x00003838;		// 即停止
				else if (usStp == 1)	unRenv4 |= 0x00005858;		// 減速停止
				else					return (ILLEGAL_PRM);
				break;
			default:
				return (ILLEGAL_PRM);
		}

        // +SLSデータ設定(RCMP1)  
        unRet = hcp530_wReg(hDevID, usAxis, WRCMP1, (uint)nPsl);
		if (unRet != 0)            return (unRet);

        // -SLSデータ設定(RCMP2) 
        unRet = hcp530_wReg(hDevID, usAxis, WRCMP2, (uint)nMsl);
		if (unRet != 0)            return (unRet);

		// 環境レジスタ4(RENV4)書込み 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV4, unRenv4);
        return (unRet);
    }

	/// <summary>
	/// ELSの設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="usPol">極性</param>
	/// <param name="usStp">停止方法</param>
	/// <returns>0:正常，0以外:異常</returns>
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
		// 軸番号が0～7以外の時は引数エラー 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// 軸番号が0～15以外の時は引数エラー 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ELS極性ポート読込み 
//		unRet = hcp530_rPortB(hDevID, ELPOL, ref byData);
		unRet = hcp530_rPortW(hDevID, ELPOL, ref byData);
		if (unRet != 0)            return (unRet);

        // 環境レジスタ１(RENV1)読込み 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV1, ref unRenv1);
		if (unRet != 0)            return (unRet);

		switch(usPol)
		{
			// ---------------------------------
			// for CPD5016 (2010.07.29)
			// ---------------------------------
//			case 0:	byData &= (byte)~(0x01 << usAxis);	break;	// B接
//			case 1:	byData |= (byte) (0x01 << usAxis);	break;	// A接
			case 0:	byData &= (ushort)~(0x01 << usAxis);	break;	// B接
			case 1:	byData |= (ushort) (0x01 << usAxis);	break;	// A接
			default:return (ILLEGAL_PRM);
		}

		switch(usStp)
		{
			case 0: unRenv1 &= (uint)(~R1_ELSM_BIT);	break;	// 即停止
			case 1: unRenv1 |= (uint)R1_ELSM_BIT;		break;  // 減速停止
			default:return (ILLEGAL_PRM);
		}		

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// ELS極性ポート書込み
//		unRet = hcp530_wPortB(hDevID, ELPOL, byData);
		unRet = hcp530_wPortW(hDevID, ELPOL, byData);
		if (unRet != 0)				return (unRet);

        // 環境レジスタ１(RENV1)書込み 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV1, unRenv1);
        return (unRet);
    }

	/// <summary>
	/// OLSの設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="usPol">極性</param>
	/// <returns>0:正常，0以外:異常</returns>			
    public static uint hcp530_SetOls(uint hDevID, ushort usAxis, ushort usPol)
    {
        uint unRet = 0;
        uint unRenv1 = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～7以外の時は引数エラー 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// 軸番号が0～15以外の時は引数エラー 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // 環境レジスタ１(RENV1)読込み 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV1, ref unRenv1);
		if (unRet != 0)				return (unRet);
		switch(usPol)
		{
			case 0:unRenv1 &= (~R1_OLSL_BIT);	break;	// B接
			case 1:unRenv1 |= R1_OLSL_BIT;		break;	// A接
            default:							return (ILLEGAL_PRM);
        }

        // 環境レジスタ１(RENV1)書込み 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV1, unRenv1);
        return (unRet);
    }

	/// <summary>
	/// SVALMの設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="usPol">極性</param>
	/// <param name="usStp">停止方法</param>
	/// <returns>0:正常，0以外:異常</returns>	
    public static uint hcp530_SetSvAlm(uint hDevID, ushort usAxis, ushort usPol, ushort usStp)
    {
        uint unRet = 0;
        uint unRenv1 = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～7以外の時は引数エラー 
//      if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// 軸番号が0～15以外の時は引数エラー 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // 環境レジスタ１(RENV1)読込み 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV1, ref unRenv1);
		if (unRet != 0)				return (unRet);
		switch(usPol)
		{
			case 0:	unRenv1 &= (~R1_ALML_BIT);	break;	// B接
			case 1:	unRenv1 |= R1_ALML_BIT;		break;	// A接
			default:							return (ILLEGAL_PRM);
		}
		switch(usStp)
		{
			case 0: unRenv1 &= (~R1_ALMM_BIT);	break;	// 即停止
			case 1: unRenv1 |= R1_ALMM_BIT;		break;  // 減速停止
			default:							return (ILLEGAL_PRM);
		}		
        // 環境レジスタ１(RENV1)書込み 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV1, unRenv1);
        return (unRet);
    }

 	/// <summary>
 	/// INPOSの設定
 	/// </summary>
 	/// <param name="hDevID">デバイスハンドル</param>
 	/// <param name="usAxis">軸番号</param>
 	/// <param name="usEnable">使用/不使用</param>
 	/// <param name="usPol">極性</param>
 	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SetInpos(uint hDevID, ushort usAxis, ushort usEnable, ushort usPol)
    {
        uint unRet = 0;
        uint unRmd = 0;
        uint unRenv1 = 0;

		// ---------------------------------
		// for CPD5016 (2011.02.16)
		// ---------------------------------
		// 軸番号が0～7以外の時は引数エラー 
		//      if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// 軸番号が0～15以外の時は引数エラー 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // 動作モード(PRMD)読込み 
        unRet = hcp530_rReg(hDevID, usAxis, RPRMD, ref unRmd);
		if (unRet != 0)            return (unRet);

        // 環境レジスタ１(RENV1)読込み 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV1, ref unRenv1);
		if (unRet != 0)            return (unRet);

        // INPOS制御有効/無効
		switch (usEnable)
		{
			case 0:unRmd &= (~MD_INPE_BIT);	break;		// 不使用
			case 1:unRmd |= MD_INPE_BIT;	break;		// 使用
			default:						return (ILLEGAL_PRM);
		}

        // INPOS入力極性
		switch (usPol)
		{
			case 0:unRenv1 &= (~R1_INPL_BIT);	break;	// B接
			case 1:unRenv1 |= R1_INPL_BIT;		break;	// A接
			default:							return (ILLEGAL_PRM);
		}

        // 動作モード(PRMD)書込み 
        unRet = hcp530_wReg(hDevID, usAxis, WPRMD, unRmd);
		if (unRet != 0)            return (unRet);
		// 環境レジスタ１(RENV1)書込み 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV1, unRenv1);
        return (unRet);
    }

	/// <summary>
	/// エンコーダZ相の設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="usCnt">原点復帰で使用するZ相回数-1</param>
	/// <param name="usPol">極性</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SetEz(uint hDevID, ushort usAxis, ushort usCnt, ushort usPol)
    {
        uint unRet = 0;
        uint unRenv2 = 0;
        uint unRenv3 = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～7以外の時は引数エラー 
        // 原点復帰で使用するＺ相回数は16回までなのでusCntは15まで
//      if ((usAxis < 0) || (7 < usAxis) || (15 < usCnt))
//      {
//          return (ILLEGAL_PRM);
//      }
		// 軸番号が0～7以外の時は引数エラー 
		// 原点復帰で使用するＺ相回数は16回までなのでusCntは15まで
		if ((usAxis < 0) || (15 < usAxis) || (15 < usCnt))
		{
			return (ILLEGAL_PRM);
		}

        // 環境レジスタ2(RENV2)読込み 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV2, ref unRenv2);
		if (unRet != 0)            return (unRet);

        // 環境レジスタ3(RENV3)読込み 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV3, ref unRenv3);
		if (unRet != 0)            return (unRet);

        // 極性選択 
 		switch (usPol)
		{
			// ---------------------------------
			// for CPD5016 (2010.08.31)
			// ---------------------------------
//			case 0:unRenv2 |= R2_EZL_BIT;		break;	// B接
//			case 1:unRenv2 &= (~R2_EZL_BIT);	break;	// A接
			case 1:unRenv2 |= R2_EZL_BIT;		break;	// B接
			case 0:unRenv2 &= (~R2_EZL_BIT);	break;	// A接
			default:							return (ILLEGAL_PRM);
            
        }

        // 原点復帰で使用するＺ相回数
        unRenv3 = (unRenv3 & (uint)0xffffff0f) | (uint)(usCnt << 4);

        // 環境レジスタ2(RENV2)書込み 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV2, unRenv2);
		if (unRet != 0)            return (unRet);

        // 環境レジスタ3(RENV3)書込み 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV3, unRenv3);
        return (unRet);
    }

	/// <summary>
	/// サーボエラーカウンタクリア出力の設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="usUse">使用方法</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SetSvCtrCl(uint hDevID, ushort usAxis, ushort usUse)
    {
        uint unRet = 0;
        uint unRenv1 = 0;

		// 軸番号が0～7以外の時は引数エラー 
		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);

        // 環境レジスタ１(RENV1)読込み 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV1, ref unRenv1);
        if (unRet != 0) return (unRet);

        unRenv1 &= (~(R1_CLRE_BIT | R1_CLRO_BIT));
        switch (usUse)
        {
            case 0:												break;	// 不使用
            case 1:  unRenv1 |= R1_CLRO_BIT;					break;	// 原点復帰完了時
            case 2:  unRenv1 |= R1_CLRE_BIT;					break;	// 異常停止時
            case 3:  unRenv1 |= (R1_CLRE_BIT | R1_CLRO_BIT);	break;  // 原点復帰完了及び異常停止時 
			default: return (ILLEGAL_PRM);    
        }
        // 環境レジスタ１(RENV1)書込み 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV1, unRenv1);
        return (unRet);
    }

	/// <summary>
	/// 指令パルスの出力形式の設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="usCmdPls">指令パルスの出力形式(個別パルス/共通パルス)</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SetCmdPulse(uint hDevID, ushort usAxis, ushort usCmdPls)
    {
        uint unRet = 0;
        uint unRenv1 = 0;
        const uint PMD = 0x00000007;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～7以外 または 
// 		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// 軸番号が0～15以外 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // 環境レジスタ１(RENV1)読込み 
        unRet = hcp530_rReg(hDevID, usAxis, RRENV1, ref unRenv1);
		if (unRet != 0) return (unRet);

        unRenv1 &= (~PMD);
        switch (usCmdPls)
        {
            case 0: unRenv1 |= (uint)0x04;	break;	// 個別 
            case 1: unRenv1 |= (uint)0x02;	break;	// 共通 
            case 2: unRenv1 |= (uint)0x07;	break;
            case 3: unRenv1 |= (uint)0x00;	break;
			default:						return (ILLEGAL_PRM);
        }

        // 環境レジスタ１(RENV1)書込み 
        unRet = hcp530_wReg(hDevID, usAxis, WRENV1, unRenv1);
        return (unRet);
    }

	/// <summary>
	/// 加減速形式設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="usAcc">加減速形式(直線/S字)</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SetAccProfile(uint hDevID, ushort usAxis, ushort usAcc)
    {
        uint unRet = 0;
        uint unRmd = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～7以外の時は引数エラー 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// 軸番号が0～15以外の時は引数エラー 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);
		
		// 動作モード(PRMD)読込み 
        unRet = hcp530_rReg(hDevID, usAxis, RPRMD, ref unRmd);
        if (unRet != 0)			return (unRet);
        
		switch (usAcc)
		{
			case 0:unRmd &= (~MD_ACC_BIT);	break;		// 直線加減速
			case 1:unRmd |= MD_ACC_BIT;		break;		// S字加減速
			default:						return (ILLEGAL_PRM);
		}

        // 動作モード(PRMD)書込み
        unRet = hcp530_wReg(hDevID, usAxis, WPRMD, unRmd);
        return (unRet);
    }

	/// <summary>
	/// 減速開始点の設定方式
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="usSdp">減速開始点の設定方式(自動/マニュアル)</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SetAutoDec(uint hDevID, ushort usAxis, ushort usSdp)
    {
        uint unRet = 0;
        uint unRmd = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～7以外の時は引数エラー 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// 軸番号が0～15以外の時は引数エラー 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);
		
		// 動作モード(PRMD)読込み 
        unRet = hcp530_rReg(hDevID, usAxis, RPRMD, ref unRmd);
		if (unRet != 0)			return (unRet);

		switch(usSdp)
		{
			case 0:unRmd &= (~MD_SDP_BIT);	break;	// 自動
			case 1:unRmd |= MD_SDP_BIT;		break;	// マニュアル
			default:						return (ILLEGAL_PRM);
		}

        // 動作モード(PRMD)書込み
        unRet = hcp530_wReg(hDevID, usAxis, WPRMD, unRmd);
        return (unRet);
    }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="usEnable">DLS/PCS使用切り替え</param>
	/// <param name="usPol">極性</param>
	/// <param name="usMot">DLS入力時の動作</param>
	/// <param name="usLtc">ラッチする/しない</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SetDlsSel(uint hDevID, ushort usAxis, ushort usEnable, ushort usPol, ushort usMot, ushort usLtc)
    {
        byte byData = 0;
        uint unRet = 0;
        uint unRmd = 0;
        uint unRenv1 = 0;

		// 軸番号が0～7以外の時は引数エラー 
		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);

        // DLS/PCS切替ポート読込
        unRet = hcp530_rPortB(hDevID, DLS_PCS, ref byData);
        if (unRet != 0)            return (unRet);

        // 動作モード(PRMD)読込
        unRet = hcp530_rReg(hDevID, usAxis, RPRMD, ref unRmd);
		if (unRet != 0)            return (unRet);

        // 環境レジスタ1(RENV1)読込み
        unRet = hcp530_rReg(hDevID, usAxis, RRENV1, ref unRenv1);
		if (unRet != 0)            return (unRet);

        // DLS/PCS使用選択
        switch (usEnable)
        {
            case 0:         // DLS入力
				byData &= (byte)(~(0x01 << usAxis));
				unRmd |= MD_DLSE_BIT;		// DLS使用
                unRmd &= (~MD_PCSE_BIT);	// PCS不使用
                unRenv1 &= (~R1_PCSL_BIT);  // PCS B接
				// DLS入力極性
                if (usPol == 0)			unRenv1 &= (~R1_DLSL_BIT);	// DLS B接
                else if (usPol == 1)	unRenv1 |= R1_DLSL_BIT;		// DLS A接
                else					return (ILLEGAL_PRM);
                break;
            case 1:         // PCS入力
				byData |= (byte)(0x01 << usAxis);	// PCS使用
				unRmd &= (~MD_DLSE_BIT);		// DLS不使用
                unRenv1 &= (~R1_DLSL_BIT);		// DLS B接
				// PCS入力極性
				if (usPol == 0)			unRenv1 &= (~R1_PCSL_BIT);	// PCS B接
                else if (usPol == 1)	unRenv1 |= R1_PCSL_BIT;		// PCS A接
                else					return (ILLEGAL_PRM);
                break;
            case 2:         // DLS, PCSとも不使用
				byData &= (byte)(~(0x01 << usAxis));
				unRmd &= (~MD_DLSE_BIT);		// DLS不使用
                unRmd &= (~MD_PCSE_BIT);		// PCS不使用
                unRenv1 |= R1_DLSL_BIT;			// DLS A接
                unRenv1 &= (~R1_PCSL_BIT);		// PCS B接
                break;
			default:
										return (ILLEGAL_PRM);
        }

        // DLS入力時の動作選択
        if (usMot == 0)			unRenv1 &= (~R1_DLSM_BIT);	// 減速のみ
        else if (usMot == 1)	unRenv1 |= R1_DLSM_BIT;		// 減速停止
        else					return (ILLEGAL_PRM);

        // DLS入力ラッチ選択
        if (usLtc == 0)			unRenv1 &= (~R1_DLLT_BIT);	// ラッチしない
        else if (usLtc == 1)	unRenv1 |= R1_DLLT_BIT;		// ラッチする
        else					return (ILLEGAL_PRM);

        // 環境レジスタ１(RENV1)書込み
        unRet = hcp530_wReg(hDevID, usAxis, WRENV1, unRenv1);
		if (unRet != 0)            return (unRet);

        // 動作モード(PRMD)書込み
        unRet = hcp530_wReg(hDevID, usAxis, WPRMD, unRmd);
		if (unRet != 0)            return (unRet);
		
		// DLS/PCS切替ポート書込み
        unRet = hcp530_wPortB(hDevID, DLS_PCS, byData);
        return (unRet);
    }

	/// <summary>
	/// メインステータス読込み
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="usSts">メインステータス格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_ReadMainSts(uint hDevID, ushort usAxis, ref ushort usSts)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～7以外の時は引数エラー 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// 軸番号が0～15以外の時は引数エラー 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // メインステータス読込み
        unRet = Hicpd530.cp530_rMstsW(hDevID, usAxis, ref usSts);
        return (unRet);
    }

	/// <summary>
	/// エラーステータス読込み
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="unSts">エラーステータス格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_ReadErrorSts(uint hDevID, ushort usAxis, ref uint unSts)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～7以外の時は引数エラー 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// 軸番号が0～15以外の時は引数エラー 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);
		
		// エラーステータス読込み
        unRet = hcp530_rReg(hDevID, usAxis, 0xf2, ref unSts);
        return (unRet);
    }

	/// <summary>
	/// イベントステータス読込み
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="unSts">イベントステータス格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_ReadEventSts(uint hDevID, ushort usAxis, ref uint unSts)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～7以外の時は引数エラー 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// 軸番号が0～15以外の時は引数エラー 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // イベントステータス読込み
        unRet = hcp530_rReg(hDevID, usAxis, 0xf3, ref unSts);
        return (unRet);
    }

	/// <summary>
	/// サブステータス読込み
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="usSts">サブステータス格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_ReadSubSts(uint hDevID, ushort usAxis, ref ushort usSts)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～7以外の時は引数エラー 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// 軸番号が0～15以外の時は引数エラー 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // サブステータス読込み
        unRet = Hicpd530.cp530_rSstsW(hDevID, usAxis, ref usSts);
        return (unRet);
    }

	/// <summary>
	/// 拡張ステータス読込み
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="unSts">拡張ステータス格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_ReadExSts(uint hDevID, ushort usAxis, ref uint unSts)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～7以外の時は引数エラー 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// 軸番号が0～15以外の時は引数エラー 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);
		
		// 拡張ステータス読込み
        unRet = hcp530_rReg(hDevID, usAxis, 0xf1, ref unSts);
        return (unRet);
    }

	/// <summary>
	/// 速度読込み
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="usSpd">速度格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_ReadSpd(uint hDevID, ushort usAxis, ref ushort usSpd)
    {
        uint unRet = 0;
        uint unSpd = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～7以外の時は引数エラー 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// 軸番号が0～15以外の時は引数エラー 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // 速度読込
        unRet = hcp530_rReg(hDevID, usAxis, RRSPD, ref unSpd);
        unSpd &= (uint)0x0000ffff;
        usSpd = (ushort)unSpd;
        return (unRet);
    }

 	/// <summary>
	/// カウンタ読込み
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="usSelCtr">読み込むカウンタの指定</param>
	/// <param name="nCtrValue">カウンタ値の格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_ReadCtr(uint hDevID, ushort usAxis, ushort usSelCtr, ref int nCtrValue)
    {
        uint unRet = 0;
        uint unData = 0;
        byte byCmd = RRCTR1;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～7以外の時は引数エラー 
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// 軸番号が0～15以外の時は引数エラー 
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // カウンタ読込
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
	/// イベントマスクの設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="unMask">イベントマスクデータ</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SetEventMask(uint hDevID, ushort usAxis, uint unMask)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～15以外または
        // イベントマスクデータが 0～0x0003ffff以外の時は引数エラー
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);
		if ((unMask & 0xfffc0000) != 0)         return (ILLEGAL_PRM);

        // イベントマスク(RIRQ)レジスタの書込み
        unRet = hcp530_wReg(hDevID, usAxis, WRIRQ, unMask);
        return (unRet);
    }

	/// <summary>
	/// ベース速度の設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="unRfl">ベース速度レジスタ値</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SetFLSpd(uint hDevID, ushort usAxis, uint unRfl)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～15以外または
        // 速度レジスタが1～0xffff以外の時は引数エラー
//      if ((usAxis < 0) || (7 < usAxis)||
		if ((usAxis < 0) || (15 < usAxis)||
            (unRfl == 0) || (65535 < unRfl))
        {
            return (ILLEGAL_PRM);
        }

        // PRFLの書込み
        unRet = hcp530_wReg(hDevID, usAxis, WPRFL, unRfl);
        return (unRet);
    }

	/// <summary>
	/// 補助速度の設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="unRfa">補助速度レジスタ値</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SetAuxSpd(uint hDevID, ushort usAxis, uint unRfa)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～15以外または
        // 速度レジスタが1～0xffff以外の時は引数エラー
//      if ((usAxis < 0) || (7 < usAxis)||
		if ((usAxis < 0) || (15 < usAxis)||
			(unRfa == 0) || (65535 < unRfa))
        {
            return (ILLEGAL_PRM);
        }

        // RFAの書込み
        unRet = hcp530_wReg(hDevID, usAxis, WRFA, unRfa);
        return (unRet);
    }

	/// <summary>
	/// 加速レートの設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/** <param name="unRur">加速レートレジスタ値
	基準クロック周波数 = 19660800Hz
	直線加速時加速時間(秒) = (PRFH - PRFL) * (PRUR + 1) * 4 / 19660800
	Ｓ字加速時加速時間(秒) = (PRFH - PRFL) * (PRUR + 1) * 8 / 19660800
	</param> */
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SetAccRate(uint hDevID, ushort usAxis, uint unRur)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～15以外または
        // 加速レートが1～0xffff以外の時は引数エラー
//      if ((usAxis < 0) || (7 < usAxis)||
		if ((usAxis < 0) || (15 < usAxis)||
		(unRur == 0) || (65535 < unRur))
        {
            return (ILLEGAL_PRM);
        }

        // PRURの書込み
        unRet = hcp530_wReg(hDevID, usAxis, WPRUR, unRur);
        return (unRet);
    }

	/// <summary>
	/// 減速レートの設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="unRdr">減速レートレジスタ値</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SetDecRate(uint hDevID, ushort usAxis, uint unRdr)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～15以外または
        // 加速レートが1～0xffff以外の時は引数エラー
//      if ((usAxis < 0) || (7 < usAxis)|| (65535 < unRdr))
		if ((usAxis < 0) || (15 < usAxis)|| (65535 < unRdr))
		{
            return (ILLEGAL_PRM);
        }

        // PRDRの書込み
        unRet = hcp530_wReg(hDevID, usAxis, WPRDR, unRdr);
        return (unRet);
    }

	/// <summary>
	/// 速度倍率レジスタの設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="unRmg">速度倍率レジスタ値(RMG) RMG=300/速度倍率-1 </param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SetMult(uint hDevID, ushort usAxis, uint unRmg)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～15以外または
        // 倍率設定値が2～4095以外は引数エラー 
//      if ((usAxis < 0) || (7 < usAxis) || (unRmg < 2) || (4095 < unRmg)) 
		if ((usAxis < 0) || (15 < usAxis) || (unRmg < 2) || (4095 < unRmg)) 
		{
            return (ILLEGAL_PRM);
        }

        // PRMGの書込み
        unRet = hcp530_wReg(hDevID, usAxis, WPRMG, unRmg);
        return (unRet);
    }

	/// <summary>
	/// 減速開始点の設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="nRdp">減速開始点</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SetDecPoint(uint hDevID, ushort usAxis, int nRdp)
    {
        uint unRet = 0;
        uint unData = (uint)nRdp;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～7以外はエラー
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// 軸番号が0～15以外はエラー
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);
		
        // PRDPの書込み
        unRet = hcp530_wReg(hDevID, usAxis, WPRDP, unData);
        return (unRet);
    }

	/// <summary>
	/// 動作モード書込み
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="usMode">動作モード</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_WritOpeMode(uint hDevID, ushort usAxis, ushort usMode)
    {
        uint unRet = 0;
        uint unRmd = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～15以外または
        // 動作モードが異常の時は引数エラー
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);
		if ((usMode & 0xff80) != 0)             return (ILLEGAL_PRM);

        // 動作モード(PRMD)読込み
        unRet = hcp530_rReg(hDevID, usAxis, RPRMD, ref unRmd);
		if (unRet != 0)            return (unRet);

        unRmd &= 0xef3a700;
        if (usMode == 0x42)	unRmd |= 0x4041;
		else				unRmd |= usMode;

        // 動作モード(PRMD)書込み
        unRet = hcp530_wReg(hDevID, usAxis, WPRMD, unRmd);
        return (unRet);
    }

	/// <summary>
	/// 動作速度レジスタの設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="unRfh">動作速度レジスタ値</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_WritFHSpd(uint hDevID, ushort usAxis, uint unRfh)
    {
        uint unRet = 0;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～15以外または
        // 速度レジスタが1～0xffff以外の時は引数エラー
//      if ((usAxis < 0) || (7 < usAxis) ||
		if ((usAxis < 0) || (15 < usAxis) ||
		(unRfh == 0) || (65535 < unRfh))
        {
            return (ILLEGAL_PRM);
        }

        // PRFHの書込み
        unRet = hcp530_wReg(hDevID, usAxis, WPRFH, unRfh);
        return (unRet);
    }

	/// <summary>
	/// 位置決め移動量の設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="nDstnc">移動量</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_WritPos(uint hDevID, ushort usAxis, int nDstnc)
    {
        uint unRet;				// 戻り値
        uint unData=(uint)nDstnc;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～15以外はエラー
        // 移動量のチェック
//      if ((usAxis < 0) || (7 < usAxis) ||
		if ((usAxis < 0) || (15 < usAxis) ||
		(nDstnc < -134217728) || (134217727 < nDstnc)) 
        {
            return (ILLEGAL_PRM);
        }

        // PRMVの書込み
        unRet = hcp530_wReg(hDevID, usAxis, WPRMV, unData);
        return (unRet);
    }

	/// <summary>
	/// 直線補間の移動量の設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="nDstnc">移動量</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_WritLine(uint hDevID, ushort usAxis, int nDstnc)
    {
        uint unRet = 0;
        uint unData = (uint)nDstnc;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号が0～15以外はエラー
        // 移動量のチェック
//      if ((usAxis < 0) || (7 < usAxis) ||
		if ((usAxis < 0) || (15 < usAxis) ||
		    (nDstnc < -134217728) || (134217727 < nDstnc))
        {
            return (ILLEGAL_PRM);
        }

        // PRMVの書込み
        unRet = hcp530_wReg(hDevID, usAxis, WPRMV, unData);
        return (unRet);
    }

 	/// <summary>
	/// 円弧補間のデータ設定
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸組み合わせ</param>
	/// <param name="nEnd1">終点位置1</param>
	/// <param name="nEnd2">終点位置2</param>
	/// <param name="nCen1">中心位置1</param>
	/// <param name="nCen2">中心位置2</param>
	/// <returns>0:正常，0以外:異常</returns>
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
		// 補間軸の組み合わせの引数が0～23以外の時はエラー
        // 中心位置と終点位置のチェック
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

			// X1～U1
			case  0: usAx1 =  0; usAx2 =  1;	break;	// X1(0)-Y1(1)
			case  1: usAx1 =  0; usAx2 =  2;	break;	// X1(0)-Z1(2)
			case  2: usAx1 =  0; usAx2 =  3;	break;	// X1(0)-U1(3)
			case  3: usAx1 =  1; usAx2 =  2;	break;	// Y1(1)-Z1(2)
			case  4: usAx1 =  1; usAx2 =  3;	break;	// Y1(1)-U1(3)
			case  5: usAx1 =  2; usAx2 =  3;	break;	// Z1(2)-U1(3)

			// X2～U2
			case  6: usAx1 =  4; usAx2 =  5;	break;	// X2(4)-Y2(5)
			case  7: usAx1 =  4; usAx2 =  6;	break;	// X2(4)-Z2(6)
			case  8: usAx1 =  4; usAx2 =  7;	break;	// X2(4)-U2(7)
			case  9: usAx1 =  5; usAx2 =  6;	break;	// Y2(5)-Z2(6)
			case 10: usAx1 =  5; usAx2 =  7;	break;	// Y2(5)-U2(7)
			case 11: usAx1 =  6; usAx2 =  7;	break;	// Z2(6)-U2(7)

			// X3～U3
			case 12: usAx1 =  8; usAx2 =  9;	break;	// X3(8)-Y3(9)
			case 13: usAx1 =  8; usAx2 = 10;	break;	// X3(8)-Z3(10)
			case 14: usAx1 =  8; usAx2 = 11;	break;	// X3(8)-U3(11)
			case 15: usAx1 =  9; usAx2 = 10;	break;	// Y3(9)-Z3(10)
			case 16: usAx1 =  9; usAx2 = 11;	break;	// Y3(9)-U3(11)
			case 17: usAx1 = 10; usAx2 = 11;	break;	// Z3(10)-U3(11)

			// X3～U3
			case 18: usAx1 = 12; usAx2 = 13;	break;	// X4(12)-Y4(13)
			case 19: usAx1 = 12; usAx2 = 14;	break;	// X4(12)-Z4(14)
			case 20: usAx1 = 12; usAx2 = 15;	break;	// X4(12)-U4(15)
			case 21: usAx1 = 13; usAx2 = 14;	break;	// Y4(13)-Z4(14)
			case 22: usAx1 = 13; usAx2 = 15;	break;	// Y4(13)-U4(15)
			case 23: usAx1 = 14; usAx2 = 15;	break;	// Z4(14)-U4(15)
		}
        // PRMV(終点位置)の書込み
        unRet = hcp530_wReg(hDevID, usAx1, WPRMV, unEnd1);
        if (unRet != 0)            return (unRet);
        unRet = hcp530_wReg(hDevID, usAx2, WPRMV, unEnd2);
		if (unRet != 0)            return (unRet);

        // PRIP(中心位置)の書込み
        unRet = hcp530_wReg(hDevID, usAx1, WPRIP, unCen1);
		if (unRet != 0)            return (unRet);
		unRet = hcp530_wReg(hDevID, usAx2, WPRIP, unCen2);
        return (unRet);
    }

	/// <summary>
	/// カウンタプリセット
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <param name="nData">プリセット値</param>
	/// <param name="usCtr">カウンタ選択</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_WritCtr(uint hDevID, ushort usAxis, int nData, ushort usCtr)
    {
        byte byCmd = WRCTR1;
        uint unRet = 0;
        uint unData = (uint)nData;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号0～7以外の時はエラー
//		if ((usAxis < 0) || (7 < usAxis))		return (ILLEGAL_PRM);
		// 軸番号0～15以外の時はエラー
		if ((usAxis < 0) || (15 < usAxis))		return (ILLEGAL_PRM);

        // プリセット値のチェック
        if (usCtr == 3)
        {	
            // カウンタ３は１６ビット
            if ((nData < -32768) || (32767 < nData))
            {
                return (ILLEGAL_PRM);
            }
        }
        else
        {	
            // その他のカウンタは３２ビット
            if ((nData < -134217728) || (134217727 < nData))
            {
                return (ILLEGAL_PRM);
            }
        }

        // プリセット値書込み
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
	/// 減速停止
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸組み合わせ(HEX)</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_DecStop(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = SDSTP;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号0x01～0xff以外の時はエラー
//		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		// 軸番号0x0001～0xffff以外の時はエラー
		if ((usAxis <= 0) || (0xffff < usAxis))	return (ILLEGAL_PRM);

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// X - U : Ｘ軸に減速停止
//		if((usAxis & 0x0f) != 0)
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));
//		}
        // V - B : Ｖ軸に減速停止
//		if (((usAxis & 0xf0) != 0) && (unRet == 0))
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
//		}

		// X1 - U1 : Ｘ１軸に減速停止
		if( (usAxis & 0x000f) != 0 )
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, (ushort)( ((usAxis & 0x000f) << 8) | usCmd) );
		}
		// X2 - U2 : Ｘ２軸に減速停止
		if( ((usAxis & 0x00f0) != 0) && (unRet == 0) )
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X2_AX, (ushort)( ((usAxis & 0x00f0) << 4) | usCmd) );
		}
		// X3 - U3 : Ｘ３軸に減速停止
		if( ((usAxis & 0x0f00) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X3_AX, (ushort)( (usAxis & 0x0f00 ) | usCmd));
		}
		// X4 - U4 : Ｘ４軸に減速停止
		if( ((usAxis & 0xf000) != 0) && (unRet == 0))
		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, (ushort)( (usAxis & 0xf000 >> 4) | usCmd));
			ushort ax = (ushort)(((usAxis & 0xf000) >> 4) | usCmd);	// C#
			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, ax);
		}

        return (unRet);
    }

    // 即停止
    // 		引数：デバイスハンドル，軸組み合わせ(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸組み合わせ(HEX)</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_QuickStop(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = STOP;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号0x01～0xff以外の時はエラー
//		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		// 軸番号0x0001～0xffff以外の時はエラー
		if ((usAxis <= 0) || (0xffff < usAxis))	return (ILLEGAL_PRM);

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// X - U : Ｘ軸に即停止
//		if ((usAxis & 0x0f) != 0)
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));
//		}
        // V - B : Ｖ軸に即停止
//		if (((usAxis & 0xf0) != 0) && (unRet == 0))
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
//		}

		// X1 - U1 : Ｘ１軸に即停止
		if( (usAxis & 0x000f) != 0 )
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, (ushort)( ((usAxis & 0x000f) << 8) | usCmd) );
		}
		// X2 - U2 : Ｘ２軸に即停止
		if( ((usAxis & 0x00f0) != 0) && (unRet == 0) )
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X2_AX, (ushort)( ((usAxis & 0x00f0) << 4) | usCmd) );
		}
		// X3 - U3 : Ｘ３軸に即停止
		if( ((usAxis & 0x0f00) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X3_AX, (ushort)( (usAxis & 0x0f00 ) | usCmd));
		}
		// X4 - U4 : Ｘ４軸に即停止
		if( ((usAxis & 0xf000) != 0) && (unRet == 0))
		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, (ushort)( (usAxis & 0xf000 >> 4) | usCmd));
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, 0x0f49);
			ushort ax = (ushort)(((usAxis & 0xf000) >> 4) | usCmd);	// C#
			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, ax);
		}

		return (unRet);
    }

    // 非常停止 
    // 		引数：デバイスハンドル，軸組み合わせ(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸組み合わせ(HEX)</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_EmgStop(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = CMEMG;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号0x01～0xff以外の時はエラー
//		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		// 軸番号0x0001～0xffff以外の時はエラー
		if ((usAxis <= 0) || (0xffff < usAxis))	return (ILLEGAL_PRM);

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// X - U : Ｘ軸に非常停止
//		if ((usAxis & 0x0f) != 0)
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));
//		}
        // V - B : Ｖ軸に非常停止
//		if (((usAxis & 0xf0) != 0) && (unRet == 0))
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
//		}

		// X1 - U1 : Ｘ１軸に非常停止
		if( (usAxis & 0x000f) != 0 )
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, (ushort)( ((usAxis & 0x000f) << 8) | usCmd) );
		}
		// X2 - U2 : Ｘ２軸に非常停止
		if( ((usAxis & 0x00f0) != 0) && (unRet == 0) )
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X2_AX, (ushort)( ((usAxis & 0x00f0) << 4) | usCmd) );
		}
		// X3 - U3 : Ｘ３軸に非常停止
		if( ((usAxis & 0x0f00) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X3_AX, (ushort)( (usAxis & 0x0f00 ) | usCmd));
		}
		// X4 - U4 : Ｘ４軸に非常停止
		if( ((usAxis & 0xf000) != 0) && (unRet == 0))
		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, (ushort)( (usAxis & 0xf000 >> 4) | usCmd));
			ushort ax = (ushort)(((usAxis & 0xf000) >> 4) | usCmd);	// for C#
			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, ax);
		}

        return (unRet);
    }

    // 同時減速停止
    // 		引数：デバイスハンドル，軸組み合わせ(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸組み合わせ(HEX)</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SyDecStop(uint hDevID, ushort usAxis)
    {
        ushort usAx = 0;
		uint[] unRmd = {0,0,0,0,0,0,0,0};
		uint unRenv1 = 0;
		uint unData = 0;
		uint unRet = 0;
        ushort[] usAxBit = { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };

        // 軸番号0x01～0xff以外の時はエラー
        if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
        
		for(usAx=X_AX; usAx < usAxBit.Length; usAx++) {
            if ((usAxis & usAxBit[usAx])==usAxBit[usAx])
            {
				// 指令軸
		        // 環境レジスタ１(RENV1): 減速停止
                unRet = hcp530_rReg(hDevID, usAx, RRENV1, ref unRenv1);
				if (unRet != 0)            return (unRet);
				// STP入力時即停止設定の場合
				if ((unRmd[usAx] & R1_CSTP_BIT) == 0)
                {
                    unRenv1 |= (uint)R1_CSTP_BIT;	// STP入力で減速停止
                    unRet = hcp530_wReg(hDevID, usAx, WRENV1, unRenv1);
					if (unRet != 0)            return (unRet);
				}
                
                // 動作モード(RMD): STP入力有効
                unRet = hcp530_rReg(hDevID, usAx, RRMD, ref unRmd[usAx]);
				if (unRet != 0)            return (unRet);
				// STP入力無効ならばSTP入力有効
				if ((unRmd[usAx] & MD_STPE_BIT) == 0)
                {
                    unData = unRmd[usAx] | MD_STPE_BIT;
                    unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
					if (unRet != 0)            return (unRet);
				}
            }
            else
            {
				// 非指令軸
				// 動作モード(RMD) 
				unRet = hcp530_rReg(hDevID, usAx, RRMD, ref unRmd[usAx]);
				if (unRet != 0)            return (unRet);
				// STP入力有効ならばSTP入力有効無効
				if ((unRmd[usAx] & MD_STPE_BIT) == MD_STPE_BIT)
				{
					unData = unRmd[usAx] & (~MD_STPE_BIT);
					unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
					if (unRet != 0)            return (unRet);
				}
			}
        }
        unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, CMSTP);	// X軸：STP出力
		if (unRet != 0)            return (unRet);

		// 動作モードを戻す
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

    // 同時即停止 
    // 		引数：デバイスハンドル，軸組み合わせ(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸組み合わせ(HEX)</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SyQuickStop(uint hDevID, ushort usAxis)
    {
		ushort usAx = 0;
		uint[] unRmd = {0,0,0,0,0,0,0,0};
		uint unRenv1 = 0;
		uint unData = 0;
		uint unRet = 0;
		ushort[] usAxBit = { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };

		// 軸番号0x01～0xff以外の時はエラー
		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);

		for(usAx=X_AX; usAx < usAxBit.Length; usAx++) 
		{
			if ((usAxis & usAxBit[usAx])==usAxBit[usAx])
			{
				// 指令軸
				// 環境レジスタ１(RENV1): 即停止
				unRet = hcp530_rReg(hDevID, usAx, RRENV1, ref unRenv1);
				if (unRet != 0)            return (unRet);
				// STP入力時減速停止設定の場合
				if ((unRmd[usAx] & R1_CSTP_BIT) == R1_CSTP_BIT)
				{
					unRenv1 &= (uint)(~R1_CSTP_BIT);	// STP入力で即停止
					unRet = hcp530_wReg(hDevID, usAx, WRENV1, unRenv1);
					if (unRet != 0)            return (unRet);
				}
                
				// 動作モード(RMD): STP入力有効
				unRet = hcp530_rReg(hDevID, usAx, RRMD, ref unRmd[usAx]);
				if (unRet != 0)            return (unRet);
				// STP入力無効ならばSTP入力有効
				if ((unRmd[usAx] & MD_STPE_BIT) == 0)
				{
					unData = unRmd[usAx] | MD_STPE_BIT;
					unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
					if (unRet != 0)            return (unRet);
				}
			}
			else
			{
				// 非指令軸
				// 動作モード(RMD) 
				unRet = hcp530_rReg(hDevID, usAx, RRMD, ref unRmd[usAx]);
				if (unRet != 0)            return (unRet);
				// STP入力有効ならばSTP入力有効無効
				if ((unRmd[usAx] & MD_STPE_BIT) == MD_STPE_BIT)
				{
					unData = unRmd[usAx] & (~MD_STPE_BIT);
					unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
					if (unRet != 0)            return (unRet);
				}
			}
		}
		unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, CMSTP);	// X軸：STP出力
		if (unRet != 0)            return (unRet);

		// 動作モードを戻す
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
	/// スタート(内部関数)
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸組み合わせ(HEX)</param>
	/// <param name="usCmd">スタートコマンド</param>
	/// <returns>0:正常，0以外:異常</returns>
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

        // 軸番号0x01～0xff以外の時はエラー
//		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		// 軸番号0x01～0xffff以外の時はエラー
		if ((usAxis <= 0) || (0xffff < usAxis))	return (ILLEGAL_PRM);

		// ---------------------------------
		// グループ単体の場合
		// ---------------------------------
		short gCnt = 0;
		if( (usAxis & 0x000f) != 0 )	gCnt++;
		if( (usAxis & 0x00f0) != 0 )	gCnt++;
		if( (usAxis & 0x0f00) != 0 )	gCnt++;
		if( (usAxis & 0xf000) != 0 )	gCnt++;
		if( gCnt == 1 )
		{
			// X1～U1軸グループのみ
			if( (usAxis & 0x000f ) != 0 )
			{
				usCmd = (ushort)((usAxis << 8) | usCmd);
				unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, usCmd);
				return (unRet);
			}
			// X2～U2軸グループのみ
			else if( (usAxis & 0x00f0 ) != 0 )
			{
				usCmd = (ushort)((usAxis << 4) | usCmd);
				unRet = Hicpd530.cp530_wCmdW(hDevID, X2_AX, usCmd);
				return (unRet);
			}
			// X3～U3軸グループのみ
			else if( (usAxis & 0x0f00 ) != 0 )
			{
				usCmd = (ushort)(usAxis | usCmd);
				unRet = Hicpd530.cp530_wCmdW(hDevID, X3_AX, usCmd);
				return (unRet);
			}
			// X4～U4軸グループのみ
			else if( (usAxis & 0xf000 ) != 0 )
			{
				usCmd = (ushort)((usAxis >> 4) | usCmd);
				unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, usCmd);
				return (unRet);
			}
		}

		// ---------------------------------
		// グループ混在の場合
		// ---------------------------------
		for (usAx = 0; usAx < usAxBit.Length; usAx++)
		{
			if((usAxis & usAxBit[usAx]) == usAxBit[usAx])
			{
				// 加速スタート:指令軸
				// 動作モード(RMD): 同時スタート
				unRet = hcp530_rReg(hDevID, usAx, RPRMD, ref unRmd[usAx]);
				if (unRet != 0)            return (unRet);
				unData = unRmd[usAx] & (~(MD_MSY0_BIT | MD_MSY1_BIT));
				unData |= MD_MSY0_BIT;
				unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
				if (unRet != 0)            return (unRet);
				unRet = Hicpd530.cp530_wCmdW(hDevID, usAx, usCmd);	// コマンド書込み
				if (unRet != 0)            return (unRet);
			} 
			else 
			{
				// 加速スタート:非指令軸
				// 動作モード(RMD): STA入力でスタートさせない
				unRet = hcp530_rReg(hDevID, usAx, RPRMD, ref unRmd[usAx]);
				if (unRet != 0)            return (unRet);
				unData = unRmd[usAx] & (~(MD_MSY0_BIT | MD_MSY1_BIT));
				unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
				if (unRet != 0)            return (unRet);
			}
		}
		unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, CMSTA);	// X軸同時スタート出力
		for (usAx = 0; usAx < usAxBit.Length; usAx++)
		{
			if(((usAxis & usAxBit[usAx]) == usAxBit[usAx])) 
			{
				// 動作モード(RMD)書込み
				unRet = hcp530_wReg(hDevID, usAx, WRMD, unRmd[usAx]);
				if (unRet != 0)            return (unRet);
			}
		}

/**
        if((usAxis & 0xf0) == 0) 
		{
        // X～U軸(4軸まで)
            usCmd = (ushort)((usAxis << 8) | usCmd);
	        unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, usCmd);		// コマンド書込み
		} 
		else 
		{
        // 5軸以上
	        if((usAxis & 0x0f) == 0) {
	        // 5軸以上､但しV～B軸
                usCmd = (ushort)((usAxis << 4) | STAUD);
		        unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, usCmd);	// コマンド書込み
	        } else {


	        // 5軸以上：X～U軸とV～B軸が混在
                for (usAx = 0; usAx < usAxBit.Length; usAx++)
                {
			        if((usAxis & usAxBit[usAx]) == usAxBit[usAx]){
			        // 加速スタート:指令軸
				        // 動作モード(RMD): 同時スタート
                        unRet = hcp530_rReg(hDevID, usAx, RPRMD, ref unRmd[usAx]);
						if (unRet != 0)            return (unRet);
						unData = unRmd[usAx] & (~(MD_MSY0_BIT | MD_MSY1_BIT));
                        unData |= MD_MSY0_BIT;
                        unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
						if (unRet != 0)            return (unRet);
						unRet = Hicpd530.cp530_wCmdW(hDevID, usAx, usCmd);	// コマンド書込み
						if (unRet != 0)            return (unRet);
					} 
					else 
					{
			        // 加速スタート:非指令軸
				        // 動作モード(RMD): STA入力でスタートさせない
                        unRet = hcp530_rReg(hDevID, usAx, RPRMD, ref unRmd[usAx]);
						if (unRet != 0)            return (unRet);
						unData = unRmd[usAx] & (~(MD_MSY0_BIT | MD_MSY1_BIT));
                        unRet = hcp530_wReg(hDevID, usAx, WRMD, unData);
						if (unRet != 0)            return (unRet);
					}
		        }
		        unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, CMSTA);	// X軸同時スタート出力
                for (usAx = 0; usAx < usAxBit.Length; usAx++)
                {
			        if(((usAxis & usAxBit[usAx]) == usAxBit[usAx])) {
				        // 動作モード(RMD)書込み
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
	/// 加速スタート
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸組み合わせ(HEX)</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_AccStart(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;

		unRet = cpd_Start(hDevID, usAxis, STAUD);

		return (unRet);
    }

	/// <summary>
	/// FH定速スタート
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸組み合わせ(HEX)</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_CnstStartFH(uint hDevID, ushort usAxis)
    {
		uint unRet = 0;

		unRet = cpd_Start(hDevID, usAxis, STAFH);

		return (unRet);
    }

	/// <summary>
	/// FL定速スタート 
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸組み合わせ(HEX)</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_CnstStartFL(uint hDevID, ushort usAxis)
    {
		uint unRet = 0;

		unRet = cpd_Start(hDevID, usAxis, STAFL);

		return (unRet);
    }

	/// <summary>
	/// FH定速スタート後減速停止
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸組み合わせ(HEX)</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_CnstStartByDec(uint hDevID, ushort usAxis)
    {
		uint unRet = 0;

		unRet = cpd_Start(hDevID, usAxis, STAFHD);

		return (unRet);
    }

    // サーボオン
    // 		引数：デバイスハンドル，軸組み合わせ(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SvOn(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = SVON;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号0x01～0xff以外の時はエラー
//		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		// 軸番号0x0001～0xffff以外の時はエラー
		if ((usAxis <= 0) || (0xffff < usAxis))	return (ILLEGAL_PRM);
		
		// X1 - U1 : Ｘ１軸にサーボＯＮ
		if ((usAxis & 0x000f) != 0)
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, (ushort)(((usAxis & 0x000f) << 8) | usCmd));
		}
		// X2 - U2 : Ｘ２軸にサーボＯＮ
		if (((usAxis & 0x00f0)!=0) && (unRet==0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X2_AX, (ushort)(((usAxis & 0x00f0) << 4) | usCmd));
		}
		// X3 - U3 : Ｘ３軸にサーボＯＮ
		if (((usAxis & 0x0f00)!=0) && (unRet==0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X3_AX, (ushort)(((usAxis & 0x0f00)) | usCmd));
		}
		// X4 - U4 : Ｘ４軸にサーボＯＮ
		if (((usAxis & 0xf000)!=0) && (unRet==0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, (ushort)(((usAxis & 0xf000) >> 4) | usCmd));
		}

		// X - U : Ｘ軸にサーボＯＮ
//		if ((usAxis & 0x0f) != 0)
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));
//		}
		// V - B : Ｖ軸にサーボＯＮ
//		if (((usAxis & 0xf0)!=0) && (unRet==0))
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
//		}
        return (unRet);
    }

    // サーボオフ
    // 		引数：デバイスハンドル，軸組み合わせ(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SvOff(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = SVOFF;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号0x01～0xff以外の時はエラー
//		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		// 軸番号0x0001～0xffff以外の時はエラー
		if ((usAxis <= 0) || (0xffff < usAxis))	return (ILLEGAL_PRM);
		
		// X1 - U1 : Ｘ１軸にサーボＯＦＦ
		if ((usAxis & 0x000f) != 0)
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, (ushort)(((usAxis & 0x000f) << 8) | usCmd));
		}
		// X2 - U2 : Ｘ２軸にサーボＯＦＦ
		if (((usAxis & 0x00f0) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X2_AX, (ushort)(((usAxis & 0x00f0) << 4) | usCmd));
		}
		// X3 - U3 : Ｘ３軸にサーボＯＦＦ
		if (((usAxis & 0x0f00) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X3_AX, (ushort)(((usAxis & 0x0f00) ) | usCmd));
		}
		// X4 - U4 : Ｘ４軸にサーボＯＦＦ
		if (((usAxis & 0xf000) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, (ushort)(((usAxis & 0xf000) >> 4) | usCmd));
		}

		// X - U : Ｘ軸にサーボＯＦＦ
//		if ((usAxis & 0x0f) != 0)
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));
//		}
		// V - B : Ｖ軸にサーボＯＦＦ
//		if (((usAxis & 0xf0) != 0) && (unRet == 0))
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
//		}
        return (unRet);
    }

    // サーボリセットオン
    // 		引数：デバイスハンドル，軸組み合わせ(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SvResetOn(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = SVRSTON;

        // 軸番号0x01～0xff以外の時はエラー
		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		
		// X - U : Ｘ軸にサーボリセットＯＮ
        if ((usAxis & 0x0f) != 0)
        {
            unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));

        }
        // V - B : Ｖ軸にサーボリセットＯＮ
        if (((usAxis & 0xf0) != 0) && (unRet == 0))
        {
            unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
        }
        return (unRet);
    }

    // サーボリセットオフ 
    // 		引数：デバイスハンドル，軸組み合わせ(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_SvResetOff(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = SVRSTOFF;

        // 軸番号0x01～0xff以外の時はエラー
		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		
		// X - U : Ｘ軸にサーボリセットＯＦＦ
        if ((usAxis & 0x0f) != 0)
        {
            unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));

        }
        // V - B : Ｖ軸にサーボリセットＯＦＦ
        if (((usAxis & 0xf0) != 0) && (unRet == 0))
        {
            unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
        }
        return (unRet);
    }

    // パルスモータオン
    // 		引数：デバイスハンドル，軸組み合わせ(HEX)
	/// <summary>
	/// 
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸番号</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_PMOn(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = SVOFF;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号0x01～0xff以外の時はエラー
//		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		// 軸番号0x0001～0xffff以外の時はエラー
		if ((usAxis <= 0) || (0xffff < usAxis))	return (ILLEGAL_PRM);
		
		// X1 - U1 : Ｘ１軸にパルスモータＯＮ
		if ((usAxis & 0x000f) != 0)
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, (ushort)(((usAxis & 0x000f) << 8) | usCmd));
		}
		// X2 - U2 : Ｘ２軸にパルスモータＯＮ
		if (((usAxis & 0x00f0) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X2_AX, (ushort)(((usAxis & 0x00f0) << 4) | usCmd));
		}
		// X3 - U3 : Ｘ３軸にパルスモータＯＮ
		if (((usAxis & 0x0f00) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X3_AX, (ushort)(((usAxis & 0x0f00) ) | usCmd));
		}
		// X4 - U4 : Ｘ４軸にパルスモータＯＮ
		if (((usAxis & 0xf000) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, (ushort)(((usAxis & 0xf000) >> 4) | usCmd));
		}

		// X - U : Ｘ軸にパルスモータＯＮ
//		if ((usAxis & 0x0f) != 0)
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));
//		}
		// V - B : Ｖ軸にパルスモータＯＮ
//		if (((usAxis & 0xf0) != 0) && (unRet == 0))
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
//		}
        return (unRet);
    }

	/// <summary>
	/// パルスモータオフ
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸組み合わせ(HEX)</param>
	/// <returns>0:正常，0以外:異常</returns>
    public static uint hcp530_PMOff(uint hDevID, ushort usAxis)
    {
        uint unRet = 0;
        ushort usCmd = SVON;

		// ---------------------------------
		// for CPD5016 (2010.07.29)
		// ---------------------------------
		// 軸番号0x01～0xff以外の時はエラー
//		if ((usAxis <= 0) || (0xff < usAxis))	return (ILLEGAL_PRM);
		// 軸番号0x0001～0xffff以外の時はエラー
		if ((usAxis <= 0) || (0xffff < usAxis))	return (ILLEGAL_PRM);
		
		// X1 - U1 : Ｘ１軸にパルスモータＯＦＦ
		if ((usAxis & 0x000f) != 0)
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X1_AX, (ushort)(((usAxis & 0x000f) << 8) | usCmd));
		}
		// X2 - U2 : Ｘ２軸にパルスモータＯＦＦ
		if (((usAxis & 0x00f0) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X2_AX, (ushort)(((usAxis & 0x00f0) << 4) | usCmd));
		}
		// X3 - U3 : Ｘ３軸にパルスモータＯＦＦ
		if (((usAxis & 0x0f00) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X3_AX, (ushort)(((usAxis & 0x0f00) ) | usCmd));
		}
		// X4 - U4 : Ｘ４軸にパルスモータＯＦＦ
		if (((usAxis & 0xf000) != 0) && (unRet == 0))
		{
			unRet = Hicpd530.cp530_wCmdW(hDevID, X4_AX, (ushort)(((usAxis & 0xf000) >> 4) | usCmd));
		}

		// X - U : Ｘ軸にパルスモータＯＦＦ
//		if ((usAxis & 0x0f) != 0)
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, X_AX, (ushort)(((usAxis & 0x0f) << 8) | usCmd));
//		}
        // V - B : Ｖ軸にパルスモータＯＦＦ
//		if (((usAxis & 0xf0) != 0) && (unRet == 0))
//		{
//			unRet = Hicpd530.cp530_wCmdW(hDevID, V_AX, (ushort)(((usAxis & 0xf0) << 4) | usCmd));
//		}
        return (unRet);
    }

	/// <summary>
	/// 加減速レートの計算
	/// </summary>
	/// <param name="unRate">加速(減速)レートレジスタ値計算結果</param>
	/// <param name="unTime">加速(減速)時間(msec)</param>
	/// <param name="unRfh">動作速度レジスタ値</param>
	/// <param name="unRfl">ベース速度レジスタ値</param>
	/// <param name="usAcc">加減速形式</param>
	/// <param name="usS">予約</param>
	/// <returns>0:正常，0以外:異常</returns>
		
    public static uint hcp530_CalAccRate(ref uint unRate, uint unTime, uint unRfh, uint unRfl, ushort usAcc, ushort usS)
    {
        double dblData = 0;
        double dblTime = 0;
        uint unData = 0;

        if ((unRfl == 0) || (unRfh == 0) || (unRfh <= unRfl) || (unTime == 0)) 
        {
            return (ILLEGAL_PRM);
        }

        dblTime = (double)unTime / 1000;    // 加速時間(秒)

		if (usS == 0)
		{
			if (usAcc==0)
			{
				// 直線加減速レート:PRUR = 4915200 * (加減速時間 / (FH - FL)) - 1)
				dblData = dblTime / (unRfh - unRfl);
				dblData = 4915200 * dblData - 1;
			}
			else if (usAcc == 1)
			{
				// 直線部分のないＳ字加減速レート:PRUR = 2457600 * (加減速時間 / (FH - FL)) - 1)
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