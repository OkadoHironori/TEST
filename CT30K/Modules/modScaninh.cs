using System;
using System.Runtime.InteropServices;
//
using CTAPI;
using CT30K.Common;

namespace CT30K
{
	///* ************************************************************************** */
	///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver11.2              */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： modScaninh.bas                                              */
	///* 処理概要　　： scaninh（コモン）の定義                                     */
	///* 注意事項　　： なし                                                        */
	///* -------------------------------------------------------------------------- */
	///* 適用計算機　： DOS/V PC                                                    */
	///* ＯＳ　　　　： WindowsXP(SP2)                                              */
	///* コンパイラ　： VB 6.0                                                      */
	///* -------------------------------------------------------------------------- */
	///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
	///*                                                                            */
	///* v11.2       05/10/19    (SI3)間々田         新規作成                       */
	///* v19.00      12/02/16    H.Nagai             BHC対応                        */
	///*                                                                            */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2005                 */
	///* ************************************************************************** */
	internal static class modScaninh
	{
		/// <summary>
		/// scaninh構造体の宣言
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct scaninhType
		{
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public int[] data_mode;					//スキャンデータモード

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] scan_mode;					//スキャンデータモード

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] multiscan_mode;			//スキャン条件モード

			public int auto_zoom;					//オートズームフラグ

			public int auto_print;					//オートプリントフラグ

			public int bhc;							//BHC処理

			public int raw_save;					//生データセーブ

			//scan_matrix(3)              As Long	'スキャンマトリックスサイズ
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
			public int[] scan_matrix;				//スキャンマトリックスサイズ v16.10 4096を追加したので要素数を変更 by 長野 10/01/29

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] scan_speed;				//スキャン速度

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] scan_area;					//スキャン撮影エリア

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] scan_det_ap;				//スキャン検出器開口

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] scan_width;				//スキャンスライス幅

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] scan_filter;				//スキャンフィルター

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] scan_energy;				//スキャン管電圧

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] scan_focus;				//スキャン管球種

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public int[] scano_matrix;				//スキャノマトリックスサイズ

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] scano_speed;				//スキャノ速度

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] scano_area;				//スキャノ撮影エリア

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] scano_det_ap;				//スキャノ検出器開口

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] scano_width;				//スキャノスライス幅

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] scano_energy;				//スキャノ管電圧

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] scano_focus;				//スキャノ管球種

			public int focus_change;				//焦点切替

			public int shutterfilter;				//シャッターフィルター機構

			public int ud_mecha_pres;				//昇降機構の有無

			public int multislice;					//複数スライス同時スキャン

			public int multi_tube;					//複数ｘ線管

			public int xray_remote;					//X線外部制御

			public int SeqComm;						//FID/FCD通信

			public int auto_centering;				//オートセンタリング

			public int cor_status;					//較正ステータス表示

			public int auto_cor;					//自動較正

			public int tilt;						//チルト機構

			public int iifield;						//I.I.視野切替え

			public int collimator;					//コリメータ

			public int filter;						//フィルタ

			public int fine_table;					//微調テーブル

			public int slice_light;					//スライスライト

			public int table_auto_move;				//自動テーブル移動

			public int scan_wizard;					//スキャンウィザード

			public int mechacontrol;				//ハイバーによるメカ制御

			public int helical;						//ヘリカルスキャン

			public int ext_trig;					//外部ﾄﾘｶﾞ取込み

			public int table_down_acquire;			//テーブル下降収集

			public int binning;						//ビニングモード

			public int mecha_ref_ac;				//メカ参照オートセンタリング

			public int table_y;						//Y軸テーブル移動可否

			public int table_x;						//X軸テーブル移動可否

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] bin_char;					//各ビニングモード

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] iifield_char;				//各I.I.視野

			public int collimator_ud;				//上下コリメータ

			public int collimator_rl;				//左右コリメータ

			public int fpd_frame;					//FPD処理フレーム表示

			public int fine_table_x;				//微調ﾃｰﾌﾞﾙX軸

			public int fine_table_y;				//微調ﾃｰﾌﾞﾙY軸

			public int scan_posi_entry_auto;		//スキャン位置校正自動有無

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] cone_multiscan_mode;		//コーンビームのマルチスキャンモード

			public int cone_distribute;				//コーンビーム分散処理

			public int rotate_select;				//回転選択                       0:可    1:不可

			public int round_trip;					//往復スキャン                   0:可    1:不可

			public int over_scan;					//オーバースキャン               0:可    1:不可

			public int xray_rotate;					//Ｘ線管回転                     0:可    1:不可

			public int mail_send;					//メール送信                     0:可    1:不可

			public int discharge_protect;			//Ｘ線休止処理                   0:可    1:不可

			public int pc_freeze;					//PCフリーズ対策　               0:可    1:不可

			public int table_restriction;			//テーブル動作ﾌﾚｰﾑ               0:可    1:不可

			public int ii_move;						//I.I.移動                       0:可    1:不可

			public int artifact_reduction;			//アーティファクト低減           0:表示  1:非表示

			public int post_cone_reconstruction;	//コーン後再構成                 0:可    1:不可

			public int pcws2;						//コーン分散処理用 PCﾜｰｸｽﾃｰｼｮﾝ2  0:可    1:不可

			public int pcws3;						//コーン分散処理用 PCﾜｰｸｽﾃｰｼｮﾝ3  0:可    1:不可

			public int pcws4;						//コーン分散処理用 PCﾜｰｸｽﾃｰｼｮﾝ4  0:可    1:不可

			public int cone_distribute2;			//コーン分散処理２               0:可    1:不可

			public int full_distortion;				//幾何歪補正                     0:２次元幾何歪 1:１次元幾何歪

			public int door_lock;					//扉電磁ロック                   0:表示  1:非表示

			public int door_keyinput;				//扉キー入力（扉電磁ロック有効時、0:扉開表示あり 1:扉開表示なし）        v11.43追加 by 間々田 2006-05-08

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public int[] filter_process;			//フィルタリング処理             (0):FFT   (1):Conv              v13.00 追加 2007-01-22 by やまおか */

			public int fcend_not0;					//終端が0でないFC関数(FFTﾌｨﾙﾀ用)（0:終端が0でない 1:終端が0）v13.00 追加 07-01-24 by やまおか

			public int double_oblique;				//ダブルオブリーク　0:可 1:不可　v13.00追加 by 間々田 2007/03/19

			public int rfc;							//RFC(ﾘﾝｸﾞｱｰﾁﾌｧｸﾄ除去補正)　　　 0:可     1:不可 v14.00追加 by Ohkado 2007/05/24

			public int rot_limit;					//テーブル回転角制限可否　　　   0:可     1:不可 v14.00追加 by Ohkado 2007/06/01

			public int gpgpu;						//再構成高速化機能の有無　　　　 0:有り   2:無し v15.03追加 by Chouno 2009/11/16

			public int high_speed_camera;			//高速度透視撮影可否　　　       0:可     1:不可 v16.01 追加 by 山影 2010/02/03

			public int xrayon_beep;					//X線ONビープ音                  0:出す  1:出さない  'v17.00追加 byやまおか 2010/01/19

			public int second_detector;				//CT用2nd検出器切替可否　　　　　0:可　　 1:不可 v17.20 追加 by Chouno 2010/08/31

			public int fpd_allpix;					//FPDの全画素データを使う        0:使う  1:使わない  'v17.22追加 byやまおか 2010/10/19

			public int smooth_rot_cone;				//連続回転コーンビームの有無　   0:有り　 1:無し v17.40追加 by 長野 2010/10/21

			public int ramdisk;						//RAMディスク                    0:あり  1:なし  v17.40追加 byやまおか 2010/10/26

			public int transdisp_lr_inv;			//透視画面の左右反転             0:する  1:しない    v17.46/v17.50追加 byやまおか 2011/02/01

			//v19.00->(電S2)永井
			public int mbhc;						//BHCの有無                      0:する　1:しない
			//<-v19.00

			/// <summary>
			/// 
			/// </summary>
			public static scaninhType Initialize()
			{
				scaninhType scaninh = new scaninhType();

				scaninh.data_mode = new int[4];
				scaninh.scan_mode = new int[3];
				scaninh.multiscan_mode = new int[3];
				scaninh.scan_matrix = new int[5];
				scaninh.scan_speed = new int[3];
				scaninh.scan_area = new int[3];
				scaninh.scan_det_ap = new int[3];
				scaninh.scan_width = new int[3];
				scaninh.scan_filter = new int[3];
				scaninh.scan_energy = new int[3];
				scaninh.scan_focus = new int[3];
				scaninh.scano_matrix = new int[4];
				scaninh.scano_speed = new int[3];
				scaninh.scano_area = new int[3];
				scaninh.scano_det_ap = new int[3];
				scaninh.scano_width = new int[3];
				scaninh.scano_energy = new int[3];
				scaninh.scano_focus = new int[3];
				scaninh.bin_char = new int[3];
				scaninh.iifield_char = new int[3];
				scaninh.cone_multiscan_mode = new int[3];
				scaninh.filter_process = new int[2];

				return scaninh;
			}
		}

		/// <summary>
		/// グローバル参照用の scaninh の宣言
		/// </summary>
		public static scaninhType scaninh = scaninhType.Initialize();

		/// <summary>
		/// scaninh の取得関数
		/// </summary>
		[DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int GetScaninh(ref scaninhType theScaninh);
	}
}
