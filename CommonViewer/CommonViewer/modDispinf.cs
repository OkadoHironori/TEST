using System;
using System.Runtime.InteropServices;

namespace CT30K
{
	internal static class modDispinf
	{
		//画像表示情報
		[StructLayout(LayoutKind.Sequential)]
		public struct dispinfType
		{
			public int dpreq;				//画像表示要求ﾌﾗｸﾞ
			public int dpcomp;				//画像表示ﾓｰﾄﾞ
			public int imgdp;				//画像面表示ﾌﾗｸﾞ(非表示･表示)
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string d_exam;			//表示するﾃﾞｨﾚｸﾄﾘ名
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string d_id;				//表示するｽﾗｲｽ名
			public int d_exammax;			//ﾃﾞｨﾚｸﾄﾘ登録件数
			public int d_idmax;				//表示中のﾃﾞｨﾚｸﾄﾘのｽﾗｲｽ枚数
			public int imgcnv;				//画像面変換ﾌﾗｸﾞ
			public int winddp;				//WW･WL表示要求ﾌﾗｸﾞ
			public int infdp;				//ｲﾝﾌｫﾒｰｼｮﾝ表示要求ﾌﾗｸﾞ
			public int roidp;				//ﾛｲ表示要求ﾌﾗｸﾞ(非表示･表示)
			public int grpdp;				//ｸﾞﾗﾌｨｯｸﾃﾞｨｽﾌﾟﾚｲﾌﾗｸﾞ
			public int colormode;			//表示ﾓｰﾄﾞ（ｶﾗｰ･ﾓｰﾄﾞ）
			public int wwwlmode;			//表示幅 （WW）ﾓｰﾄﾞ
			public int Width;				//ｶﾗｰ表示階調幅
			public int level;				//ｶﾗｰ表示階調ﾚﾍﾞﾙ
			public int coloralpha;			//ｶﾗｰ表示
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3 * 8)]
			public int[] palette;			//ｶﾗｰ8色ﾊﾟﾚｯﾄ
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2 * 8)]
			public int[] paletl1;			//ｶﾗｰ8色範囲。絶対値
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2 * 8)]
			public int[] paletl2;			//ｶﾗｰ8色範囲。相対値
			public int Alpha;				//ｶﾗｰalpha define
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
			public int[] Rtbl;				//ｶﾗｰ変換ﾃｰﾌﾞﾙ(R)
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
			public int[] Gtbl;				//ｶﾗｰ変換ﾃｰﾌﾞﾙ(G)
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
			public int[] Btbl;				//ｶﾗｰ変換ﾃｰﾌﾞﾙ(B)
			public int color_max;			//ﾊﾟﾚｯﾄ作成時上限CT値
			public int color_min;			//ﾊﾟﾚｯﾄ作成時下限CT値
			public float GAMMA;				//v19.00 ガンマ補正値 by長野 2012/02/21

			public static dispinfType Initialize()
			{
				dispinfType dispinf = new dispinfType();

				dispinf.palette = new int[3 * 8];
				dispinf.paletl1 = new int[2 * 8];
				dispinf.paletl2 = new int[2 * 8];

				dispinf.Rtbl = new int[256];
				dispinf.Gtbl = new int[256];
				dispinf.Btbl = new int[256];

				return dispinf;
			}
		}


		public static dispinfType dispinf = dispinfType.Initialize();		//画像表示情報

		//dispinf（コモン）取得関数
		[DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int GetDispinf(ref dispinfType theDispinf);
		[DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int PutDispinf(ref dispinfType theDispinf);
	}
}

