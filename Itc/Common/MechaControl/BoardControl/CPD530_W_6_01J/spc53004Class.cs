using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

namespace spc53004
{  
/// <summary>
/// spc53004Class の概要の説明です。
/// </summary>
	class spc53004Class
	{
		//------------------------------------------------------------------------------
		//  グローバル定数
		//------------------------------------------------------------------------------
		/// デバイス関係定数
		/// このサンプルでは16枚まで
		public const uint CNTMAX = 16;

		//------------------------------------------------------------------------------
		//  グローバル変数
		//------------------------------------------------------------------------------
		// デバイス関係
		/// <summary>
		/// デバイスハンドル
		/// </summary>
		public static uint hDeviceID;

		/// <summary>
		/// デバイス個数
		/// </summary>
		public static uint gCnt;                          

		/// <summary>
		/// デバイス情報
		/// </summary>
		static public Hicpd530.HPCDEVICEINFO[] gHpcDevInfo = new Hicpd530.HPCDEVICEINFO[CNTMAX];

		/// <summary>
		/// このプログラムで共通に使うクラス
		/// </summary>
		///------------------------------------------------------------------------------
		/// デバイスの初期化
		///------------------------------------------------------------------------------
		public static uint IniDev()
		{
			ushort nIx = 0;
			uint unRet = 0;
			Hicpd530.HPCDEVICEINFO[] hpcDevInfo = new Hicpd530.HPCDEVICEINFO[CNTMAX];
			string s;			

			// ボード枚数 & デバイス情報取得
            unRet = Cp530l1a.hcp530_GetDevInfo(ref gCnt, gHpcDevInfo);
			if (unRet != 0)
			{
				s = "デバイス情報が取得できません\r\n戻り値：" + unRet.ToString("X8");
				MessageBox.Show(s);
				return (unRet);
			}

			hpcDevInfo[0].dwBoardID = 1;

			// ボード指定(ボードID = 0のボード使用)
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
				MessageBox.Show("ボードID = 0 のボードが装着されていません.");
				return (0x1000);
			}
			// デバイスオープン
			unRet = Cp530l1a.hcp530_DevOpen( ref hDeviceID, ref hpcDevInfo[0] );
			if (unRet != 0)
			{
				s = "デバイスオープンできません\r\n戻り値：" + unRet.ToString("X8"); 
				MessageBox.Show(s);
			}
			return (unRet);
		}
	}
}
