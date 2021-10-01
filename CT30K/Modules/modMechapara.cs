using System;
using System.Runtime.InteropServices;

namespace CT30K
{
	internal static class modMechapara
	{
		//メカパラ構造体     Manual追加 2009/07/24
		[StructLayout(LayoutKind.Sequential)]
		public struct MechaparaType
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] rot_speed;			//回転速度[Slow/Middle/Fast/Manual]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] ud_speed;			//テーブル昇降速度[Slow/Middle/Fast/Manual]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] fcd_speed;			//FCD移動速度[Slow/Middle/Fast/Manual]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] fdd_speed;			//FDD移動速度[Slow/Middle/Fast/Manual]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] tbl_y_speed;			//テーブルY軸速度[Slow/Middle/Fast/Manual]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] fine_tbl_speed;		//微調テーブル移動速度[Slow/Middle/Fast/Manual]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] collimator_speed;	//スライスコリメータ移動速度[Slow/Middle/Fast/Manual]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] xray_rot_speed;		//Ｘ線管回転速度[Slow/Middle/Fast/Manual]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] xray_x_speed;		//Ｘ線管Ｘ軸移動速度[[Slow/Middle/Fast/Manual]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public float[] xray_y_speed;		//Ｘ線管Ｙ軸移動速度[[Slow/Middle/Fast/Manual]

			public static MechaparaType Initialize()
			{
				MechaparaType mechapara = new MechaparaType();

				mechapara.rot_speed = new float[4];
				mechapara.ud_speed = new float[4];
				mechapara.fcd_speed = new float[4];
				mechapara.fdd_speed = new float[4];
				mechapara.tbl_y_speed = new float[4];
				mechapara.fine_tbl_speed = new float[4];
				mechapara.collimator_speed = new float[4];
				mechapara.xray_rot_speed = new float[4];
				mechapara.xray_x_speed = new float[4];
				mechapara.xray_y_speed = new float[4];

				return mechapara;
			}
		}

		//メカパラ構造体変数
		public static MechaparaType mechapara = MechaparaType.Initialize();

		//メカパラ（コモン）取得関数
		[DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int GetMechapara(ref MechaparaType theMechapara);
		
		//メカパラ（コモン）設定関数
		[DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int PutMechapara(ref MechaparaType theMechapara);
	}
}
