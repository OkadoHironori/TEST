
using System;
using System.Runtime.InteropServices;

#pragma warning disable IDE1006

/// <summary>
///  PCI CPD Series Devise Driver I/F DLL for Windows	
///		file name:Hicpd530.cs			
///      date     :2010/11/11			
///      version  :1.0.0.0 新規作成	
///	Copyright(C) 2001-2009 Hivertec,inc. All Rights Reserved.	
/// </summary>
public class Hicpd530
{
	/// <summary>
	/// デバイス情報
	/// </summary>

    [StructLayout(LayoutKind.Sequential)]
    public struct HPCDEVICEINFO
    {
        /// <summary>
        /// バス番号
        /// </summary>
		public uint nBusNumber;
		
		/// <summary>
		/// デバイス番号
		/// </summary>
        public uint nDeviceNumber;
 
		/// <summary>
		/// I/Oポートアドレス
		/// </summary>
        public uint dwIoPortAddress;
	
		/// <summary>
		/// IRQ番号
		/// </summary>
        public uint dwIrqNo;
        
		/// <summary>
		/// 管理番号(Windows 9xでは無視)
		/// </summary>
		public uint dwNumber;
        
		/// <summary>
		/// ボードID
		/// </summary>
		public uint dwBoardID;
    }

	/// <summary>
	/// デバイス個数の取得
	/// </summary>
	/// <param name="pDevNum">デバイスの個数の格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_GetDeviceCount(ref uint pDevNum);
    
	/// <summary>
	/// デバイス情報の取得
	/// </summary>
	/// <param name="pDevNum">デバイスの個数の格納先</param>
	/// <param name="pHpcDevInfo">デバイス情報の格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_GetDeviceInfo(ref uint pDevNum, [In, Out, MarshalAs(UnmanagedType.LPArray)] HPCDEVICEINFO[] pHpcDevInfo);
    
	/// <summary>
	/// デバイスオープン
	/// </summary>
	/// <param name="hDevID">デバイスハンドル格納先</param>
	/// <param name="pHpcDevInfo">デバイス情報の格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_OpenDevice(ref uint hDevID, ref HPCDEVICEINFO pHpcDevInfo);
    
	/// <summary>
	/// デバイスクローズ
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <returns>0:正常，0以外:異常</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_CloseDevice(uint hDevID);
    
	/// <summary>
	/// メインステータス読込
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸</param>
	/// <param name="usData">データ格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_rMstsW(uint hDevID, ushort usAxis, ref ushort usData);
    
	/// <summary>
	/// サブステータス読込
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸</param>
	/// <param name="usData">データ格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_rSstsW(uint hDevID, ushort usAxis, ref ushort usData);
    
	/// <summary>
	/// コマンド書込
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸</param>
	/// <param name="usCmd">コマンド</param>
	/// <returns>0:正常，0以外:異常</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_wCmdW(uint hDevID, ushort usAxis, ushort usCmd);
    
	/// <summary>
	/// レジスタ読込
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸</param>
	/// <param name="byCmd">レジスタ読込コマンド</param>
	/// <param name="unData">データ格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_rReg(uint hDevID, ushort usAxis, byte byCmd, ref uint unData);
    
	/// <summary>
	/// レジスタ書込
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸</param>
	/// <param name="byCmd">レジスタ書込コマンド</param>
	/// <param name="unData">データ</param>
	/// <returns>0:正常，0以外:異常</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_wReg(uint hDevID, ushort usAxis, byte byCmd, uint unData);
    
	/// <summary>
	/// オプションポート読出(バイト)
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="byCmd">オプションポート読出コマンド</param>
	/// <param name="byData">データ格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_rPortB(uint hDevID, byte byCmd, ref byte byData);
	
	/// <summary>
	/// オプションポート読出(ワード)
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="byCmd">オプションポート読出コマンド</param>
	/// <param name="usData">データ格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_rPortW(uint hDevID, byte byCmd, ref ushort usData);
    
	/// <summary>
	/// オプションポート書込(バイト)
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="byCmd">オプションポート書込コマンド</param>
	/// <param name="byData">データ</param>
	/// <returns>0:正常，0以外:異常</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_wPortB(uint hDevID, byte byCmd, byte byData);
	
	/// <summary>
	/// オプションポート書込(ワード)
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="byCmd">オプションポート書込コマンド</param>
	/// <param name="usData">データ</param>
	/// <returns>0:正常，0以外:異常</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_wPortW(uint hDevID, byte byCmd, ushort usData);
    
	/// <summary>
	/// 入出力バッファ読出
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸</param>
	/// <param name="unData">データ格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_rBufDW(uint hDevID, ushort usAxis, ref uint unData);
    
	/// <summary>
	/// 入出力バッファ書込
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usAxis">軸</param>
	/// <param name="unData">データ</param>
	/// <returns>0:正常，0以外:異常</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_wBufDW(uint hDevID, ushort usAxis, uint unData);
    
	/// <summary>
	/// ボード固有コード取得
	/// </summary>
	/// <param name="hDevID">デバイスハンドル</param>
	/// <param name="usCode">ボード固有コードデータ格納先</param>
	/// <returns>0:正常，0以外:異常</returns>
    [DllImport("Hicpd530.dll")]
    public static extern uint cp530_GetBoardCode(uint hDevID, ref ushort usCode);
}
#pragma warning restore IDE1006


