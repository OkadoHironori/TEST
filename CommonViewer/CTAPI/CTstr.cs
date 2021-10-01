using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace CTAPI
{
    /// <summary>
    /// コモン構造体
    /// </summary>
    public class CTstr
    {
        #region ヘルパークラス
        /// <summary>
        /// ヘルパー
        /// </summary>
        public class StrHelper
        {
            static StrHelper()
            {
                // デフォルトS-JIS 
                TextEnc = Encoding.GetEncoding(932);
            }

            /// <summary>
            /// 文字エンコーディング
            /// </summary>
            public static Encoding TextEnc { get; set; }
 
            /// <summary>
            /// バイト->文字列変換取得
            /// </summary>
            /// <param name="data"></param>
            /// <param name="size"></param>
            /// <param name="index"></param>
            /// <returns></returns>
            public static string ToStr(byte[] data, int size, int index = 0)
            {
                byte[] bytes = new byte[size];
                Array.Copy(data, (index * size), bytes, 0, size);
                string text = TextEnc.GetString(bytes);
                return text.TrimEnd('\0');
            }

            /// <summary>
            /// 文字列バイト変換取得
            /// </summary>
            /// <param name="text"></param>
            /// <param name="size"></param>
            /// <returns></returns>
            public static byte[] ToByte(string text, int size)
            {
                byte[] bytes = new byte[size];
                Array.Clear(bytes, 0, bytes.Length);
                StrToByte(text, ref bytes, size);
                return bytes;
            }

            /// <summary>
            /// 文字列バイト変換セット
            /// </summary>
            /// <param name="text"></param>
            /// <param name="bytes"></param>
            /// <param name="size"></param>
            /// <param name="index"></param>
            public static void StrToByte(string text, ref byte[] bytes, int size, int index = 0)
            {
                byte[] data = TextEnc.GetBytes(text);
                if (data.Length < size)
                {
                    Array.Copy(data, 0, bytes, (index * size), data.Length);
                }
                else
                {
                    Array.Copy(data, 0, bytes, (index * size), (size - 1));
                }
            }
        }
        #endregion ヘルパークラス

        //-----------------------------------------------------------------------------
        // 構造体
        //-----------------------------------------------------------------------------
        #region SCANINH
        /// <summary>
        /// ﾃﾞｰﾀ収集初期化情報
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SCANINH
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	        public int[]    data_mode		;/* ｽｷｬﾝﾃﾞｰﾀﾓｰﾄﾞ				*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            //public int[]	scan_mode		;/* ｽｷｬﾝﾃﾞｰﾀﾓｰﾄﾞ				*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]    //v18.00変更 byやまおか 2011/01/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_変更2014/07/16hata_v19.51反映
            public int[] scan_mode;/* ｽｷｬﾝﾃﾞｰﾀﾓｰﾄﾞ				*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	multiscan_mode	;/* ｽｷｬﾝ条件ﾓｰﾄﾞ				*/
	        public int	    auto_zoom			;/* ｵｰﾄｽﾞｰﾑﾌﾗｸﾞ					*/
	        public int	    auto_print			;/* ｵｰﾄﾌﾟﾘﾝﾄﾌﾗｸﾞ				*/	
	        public int	    bhc					;/* BHC処理						*/
	        public int	    raw_save			;/* 生ﾃﾞｰﾀｾｰﾌﾞ					*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
	        public int[]	scan_matrix		;/* ｽｷｬﾝﾏﾄﾘｯｸｽｻｲｽﾞ				*/ //v16.10 変更 2010/02/02 by 長野
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[]    cone_matrix     ;/* ｺｰﾝﾏﾄﾘｯｸｽｻｲｽﾞ				*/ //v20.00 追加 2015/01/16 by 長野
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scan_speed		;/* ｽｷｬﾝ速度					*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scan_area		;/* ｽｷｬﾝ撮影ｴﾘｱ					*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scan_det_ap		;/* ｽｷｬﾝ検出器開口				*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scan_width		;/* ｽｷｬﾝｽﾗｲｽ幅					*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scan_filter		;/* ｽｷｬﾝﾌｨﾙﾀｰ					*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scan_energy		;/* ｽｷｬﾝ管電圧					*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scan_focus		;/* ｽｷｬﾝ管球種  REV1.00			*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	        public int[]	scano_matrix		;/* ｽｷｬﾉﾏﾄﾘｯｸｽｻｲｽﾞ				*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scano_speed		;/* ｽｷｬﾉ速度					*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scano_area		;/* ｽｷｬﾉ撮影ｴﾘｱ					*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scano_det_ap		;/* ｽｷｬﾉ検出器開口				*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scano_width		;/* ｽｷｬﾉｽﾗｲｽ幅					*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scano_energy		;/* ｽｷｬﾉ管電圧					*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scano_focus		;/* ｽｷｬﾉ管球種 REV1.00			*/
	        public int	    focus_change		;/* 焦点切替					*/
	        public int	    shutterfilter		;/* ｼｬｯﾀｰﾌｨﾙﾀｰ機構 REV2.00		*/
	        public int	    ud_mecha_pres		;/* 昇降機構の有無 REV2.00		*/
	        public int      multislice          ;/* 複数スライス同時スキャン  Rev2.10 */
	        public int      multi_tube          ;/* 複数ｘ線管　　　　　　　　Rev2.10 */
	        public int	    xray_remote			;/* X線外部制御	Rev3.00			*/
	        public int	    seqcomm				;/* FID/FCD通信	Rev3.00	  fidfcd_remote→seqcomm 名称変更 fidfcd_remote	REV4.00 */
	        public int	    auto_centering		;/* ｵｰﾄｾﾝﾀﾘﾝｸﾞ	Rev3.00			*/
	        public int	    cor_status			;/* 較正ｽﾃｰﾀｽ表示		REV4.00 */
	        public int	    auto_cor			;/* 自動較正			REV4.00 */
	        public int	    tilt				;/* ﾁﾙﾄ機構				REV4.00 */
	        public int	    iifield				;/* I.I.視野切替え		REV4.00 */
	        public int	    collimator			;/* ｺﾘﾒｰﾀ				REV4.00 */
	        public int	    filter				;/* ﾌｨﾙﾀ				REV4.00 */
	        public int	    fine_table			;/* 微調ﾃｰﾌﾞﾙ			REV5.00 */
	        public int	    slice_light			;/* 微調ﾃｰﾌﾞﾙ			REV5.00 */
	        public int	    table_auto_move		;/* 自動ﾃｰﾌﾞﾙ移動		REV6.00 */
	        public int	    scan_wizard			;/* ｽｷｬﾝｳｨｻﾞｰﾄﾞ			REV6.00 */
	        public int	    mechacontrol		;/* ﾊｲﾊﾞｰによるﾒｶ制御	REV6.00 */
	        public int	    helical				;/* ヘリカルスキャン	REV7.04 */
	        public int	    ext_trig			;/* 外部ﾄﾘｶﾞ取込み		REV7.04 */
	        public int	    table_down_acquire	;/* テーブル下降収集	REV7.04 */
	        public int	    binning				;/* ビニングモード		REV7.04 */
	        public int	    mecha_ref_ac		;/* メカ参照ｵｰﾄｾﾝﾀﾘﾝｸﾞ	REV7.04 */
	        public int	    table_y				;/* Y軸ﾃｰﾌﾞﾙ移動可否	REV7.04 */
	        public int	    table_x				;/* X軸ﾃｰﾌﾞﾙ移動可否	REV7.04 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	bin_char			;/* 各ビニングモード	REV7.04 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	iifield_char		;/* 各I.I.視野			REV7.04 */
	        public int	    collimator_ud		;/* 上下コリメータ		REV7.04 */
	        public int	    collimator_rl		;/* 左右コリメータ		REV7.04 */
	        public int	    fpd_frame			;/* FPD処理フレーム表示	REV7.04 */
	        public int	    fine_table_x		;/* 微調ﾃｰﾌﾞﾙX軸			REV7.04 */
	        public int	    fine_table_y		;/* 微調ﾃｰﾌﾞﾙY軸			REV7.04 */
	        public int	    scan_posi_entry_auto	;/* スキャン位置校正自動有無			REV7.04 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	cone_multiscan_mode	;/* コーンビームのマルチスキャンモード	REV8.00 */
	        public int	    cone_distribute			;/* コーンビーム分散処理				REV8.00 */
 	        public int	    rotate_select		;/*  回転選択 			0:可	1:不可		REV9.00 追加 2004-01-29 by 間々田 */
 	        public int	    round_trip			;/*  往復スキャン 		0:可 	1:不可		REV9.00 追加 2004-01-29 by 間々田 */
 	        public int	    over_scan			;/*  オーバースキャン 	0:可 	1:不可		REV9.00 追加 2004-01-29 by 間々田 */
	        public int	    xray_rotate			;/*  Ｘ線管回転 		0:可 	1:不可		REV9.00 追加 2004-01-29 by 間々田 */
	        public int	    mail_send			;/*  メール送信 		0:可 	1:不可		REV9.1 追加 2004-05-13 by 間々田 */
	        public int	    discharge_protect	;/* Ｘ線休止処理 		0:可 	1:不可		REV9.3 追加 2004-06-30 by 間々田 */
	        public int	    pc_freeze			;/*  PCフリーズ対策　	0:可 	1:不可		REV9.4 追加 2004-08-17 by 山本 */
	        public int	    table_restriction	;/*  テーブル動作ﾌﾚｰﾑ	0:可 	1:不可		REV9.6 追加 2004-10-12 by 間々田 */
	        public int	    ii_move				;/*  I.I.移動			0:可 	1:不可		REV9.6 追加 2004-10-12 by 間々田 */
	        public int	    artifact_reduction	;/* アーティファクト低減 0:表示 1:非表示	REV9.7 追加 2004-12-08 by 間々田 */
	        public int	    post_cone_reconstruction	;/* コーン後再構成							0:可 	1:不可	REV10.0 追加 2005-01-24 by 間々田 */
	        public int	    pcws2						;/* コーン分散処理用 PCワークステーション2	0:可 	1:不可	REV10.0 追加 2005-01-24 by 間々田 */
	        public int	    pcws3						;/* コーン分散処理用 PCワークステーション3	0:可 	1:不可	REV10.0 追加 2005-01-24 by 間々田 */
	        public int	    pcws4						;/* コーン分散処理用 PCワークステーション4	0:可 	1:不可	REV10.0 追加 2005-01-24 by 間々田 */
	        public int	    cone_distribute2			;/* コーン分散処理２						0:可 	1:不可	REV10.0 追加 2005-01-24 by 間々田 */
	        public int	    full_distortion		; // 幾何歪補正	0:２次元幾何歪 1:１次元幾何歪	v11.2追加 by 間々田 2005/10/04
	        public int	    door_lock			; // 扉電磁ロック								v11.4追加 by 間々田 2006/03/02
	        public int	    door_keyinput		; // 扉キー入力（扉電磁ロック有効時、0:扉開表示あり 1:扉開表示なし）		v11.43追加 by 間々田 2006-05-08
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	        public int[]	filter_process	; // フィルタリング処理 0:FFT 1:Conv								Rev13.00 追加 by ohkado 2007/02/19
	        public int	    fcend_not0			; // 終端が0でないFC関数(FFTフィルタ用)（0:終端が0でない 1:終端が0）Rev13.00 追加 by やまおか
	        public int	    double_oblique	    ; // ダブルオブリーク機能　0:可 1:不可	Rev13.00 追加 2007-03-19 by 間々田
	        public int	    rfc					; // RFC								Rev14.00 追加 2007-05-24 by Ohkado
	        public int	    rot_limit			; // テーブル回転角制限可否	0:可 1:不可 Rev14.00 追加 2007-06-01 by Ohkado
	        public int      gpgpu				; // 再構成高速化機能の有無 0:有り 1:無し Rev16.00 追加 2009-11-16 by Chouno
	        public int	    high_speed_camera	; // 高速度透視撮影可否 0:可 1:不可 Rev16.01 追加 by YAMAKAGE 2010/02/03
	        public int	    xrayon_beep			; // X線ONビープ音		0:出す	1:出さない	REV16.20追加 2010-01-19 byやまおか
	        public int      second_detector        ; // CT用2nd検出器切替可否 0:可 1:不可　Rev17.03 追加 2010-08-20 by 長野
	        public int	    fpd_allpix			; // FPDの全画素データを使う(周辺を切り落とさない) 0:使う 1:使わない	Rev17.22追加 byやまおか 2010/10/19
	        public int	    smooth_rot_cone		; // 連続回転コーンビームスキャン 0:可 1:不可 Rev17.40 追加 2010-10-20 by IWASAWA
	        public int	    ramdisk				; // RAMディスク 0:有　1:無	Rev17.40 追加 2010-10-25 by IWASAWA
	        public int	    transdisp_lr_inv	; // 透視画面の左右反転 0:反転する 1:反転しない Rev17.50追加 byやまおか 2011-02-01
	        public int	    mbhc                ; // BHC有無  0:する、1:しない
            
            //V19.51-->追加2014/07/16hata_v19.51反映
            public int avmode; // 産業用CTモード                 0:あり  1:なし  v18.00追加 byやまおか 2011/03/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public int[] xfocus; // X線焦点(0:小)(1:大)            0:可    1:不可  v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public int alw_autocentering; // マルチスキャン中、常にオートセンタリングを行う。 0:行う,1:行わない by長野 2013/12/09 v19.50
            //<--V19.51
            public int table_y_axis_move_acquire; //テーブルY軸移動収集 0:なし、1:あり Rev20.00 追加 by長野 2015/02/16  2015/02/16

            public int tilt_and_rot             ;// 回転傾斜テーブル有無 0:あり、1:なし Rev22.00 追加 by長野 2015/08/20

            public int adj_center_ch            ;// 回転中心ch調整機能 0:あり、1:なし Rev23.00 追加 by長野 2015/09/07

            public int cm_mode                  ;// 計測CTモード(0:計測CTモード,1:標準機モード) Rev23.10 追加 by長野 2015/09/18

            public int op_panel                 ;// 操作パネル(ON機能)(0:有,1:無) Rev23.10 追加 by長野 2015/10/16

            public int distancecor_phantomfree  ;// 専用ファントム無しの寸法構成(0:有,無:1) //Rev23.12 追加 by長野 2015/12/28

            public int save_purdata             ;// 純生データ保存機能(0:有,1:無) //Rev23.12 追加 by長野 2015/12/28

            public int lr_sft                   ;// 左右シフト(0:有,1:無) by長野 2015/11/19 

            public int ct_gene2and3             ;// ３世代、２世代兼用モード(0:有,1:無) by長野 2015/12/17

            public int ExObsCamera              ;// 外観カメラ機能(0:有,1:無) by長野 2016/02/05 Rev23.30

            public int inlineCT                 ;// インラインCT(0:無,1:有) by長野 2016/04/06 //Rev24.00 追加 

            public int table_x_axis_move_acquire;//テーブルX軸移動収集 0:なし、1:あり Rev24.00 追加 by長野 2016/05/09

            #region 初期化
            //v1951-->追加2014/10/07hata_v19.51反映
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // 配列の初期化
                data_mode = new int[4];             // ｽｷｬﾝﾃﾞｰﾀﾓｰﾄﾞ
                scan_mode = new int[4];             // ｽｷｬﾝﾃﾞｰﾀﾓｰﾄﾞ
                multiscan_mode = new int[3];        // ｽｷｬﾝ条件ﾓｰﾄﾞ
                scan_matrix = new int[5];           // ｽｷｬﾝﾏﾄﾘｯｸｽｻｲｽﾞ 
                scan_speed = new int[3];            // ｽｷｬﾝ速度
                scan_area = new int[3];             // ｽｷｬﾝ撮影ｴﾘｱ
                scan_det_ap = new int[3];           // ｽｷｬﾝ検出器開口
                scan_width = new int[3];            // ｽｷｬﾝｽﾗｲｽ幅
                scan_filter = new int[3];           // ｽｷｬﾝﾌｨﾙﾀｰ

                scan_energy = new int[3];           // ｽｷｬﾝ管電圧
                scan_focus = new int[3];            // ｽｷｬﾝ管球種
                scano_matrix = new int[4];          // ｽｷｬﾉﾏﾄﾘｯｸｽｻｲｽﾞ
                scano_speed = new int[3];           // ｽｷｬﾉ速度
                scano_area = new int[3];            // ｽｷｬﾉ撮影ｴﾘｱ
                scano_det_ap = new int[3];          // ｽｷｬﾉ検出器開口	
                scano_width = new int[3];           // ｽｷｬﾉｽﾗｲｽ幅
                scano_energy = new int[3];          // ｽｷｬﾉ管電圧
                scano_focus = new int[3];           // ｽｷｬﾉ管球種 

                bin_char = new int[3];              // 各ビニングモード
                iifield_char = new int[3];          // 各I.I.視野
                cone_multiscan_mode = new int[3];   // コーンビームのマルチスキャンモード
                filter_process = new int[2];        // フィルタリング処理 0:FFT 1:Conv
                xfocus = new int[4];                //  X線焦点(0:小)(1:大)
            }
            //<--v1951
            #endregion
        
        }   // SCANINH
        #endregion SCANINH

        #region INFDEF
        /// <summary>
        /// CT画像初期化情報
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct INFDEF
        {
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 8))]
            //public byte[]	focustype		;/* X線焦点  REV1.00			*/         
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 32))]
            //public byte[]	data_mode	;/* ｽｷｬﾝﾃﾞｰﾀﾓｰﾄﾞ	Rev3.00		*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 32))]
            //public byte[]	scan_mode 	;/* ｽｷｬﾝﾃﾞｰﾀﾓｰﾄﾞ				*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 32))]
            //public byte[]	multiscan_mode  ;/* ｽｷｬﾝ条件ﾓｰﾄﾞ			*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            //public byte[]	auto_zoom   	;/* ｵｰﾄｽﾞｰﾑ表示文字				*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            //public byte[]	auto_print		;/* ｵｰﾄﾌﾟﾘﾝﾄ表示文字			*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            //public byte[]	raw_save		;/* 生ﾃﾞｰﾀｾｰﾌﾞ表示文字			*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            //public byte[]	bhc				;/* BHC補正表示文字				*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (5 * 16))]
            //public byte[]	matrixsize	;/* ｽｷｬﾝﾏﾄﾘｯｸｽｻｲｽﾞ				*/ //v16.10 変更　2010/02/02 by 長野
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (4 * 16))]
            //public byte[]	scano_matrix	;/* ｽｷｬﾉﾏﾄﾘｯｸｽｻｲｽﾞ				*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 16))]
            //public byte[]	scan_speed	;/* ｽｷｬﾝ速度表示文字			*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 16))]
            //public byte[]	scano_speed	;/* ｽｷｬﾉ速度表示文字			*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 8))]
            //public byte[]	scan_area		;/* ｽｷｬﾝ撮影領域表示文字		*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 8))]
            //public byte[]	scano_area	;/* ｽｷｬﾉ撮影領域表示文字		*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 8))]
            //public byte[]	slice_wid		;/* ｽﾗｲｽ厚表示文字				*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 8))]
            //public byte[]	scano_slice_wid	;/* ｽｷｬﾉｽﾗｲｽ厚表示文字		*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 8))]
            //public byte[]	det_ap		;/* 検出器開口表示文字			*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 8))]
            //public byte[]	fc			;/* ﾌｨﾙﾀ関数					*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            //public byte[]	max_view			;/* 最大ﾋﾞｭｰ数 REV2.00			*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            //public byte[]	min_view			;/* 最小ﾋﾞｭｰ数 REV2.00			*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            //public byte[]	max_integ_number	;/* 最大画像積算枚数 REV2.00	*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            //public byte[]	min_integ_number	;/* 最小画像積算枚数 REV2.00	*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            //public byte[]    fimage_bit      ;/* 透視画像ビット数 REV M3     */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 16))]
            //public byte[]	multislice	;/* 複数同時ｽﾗｲｽ数　REV2.10		*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (2 * 16))]
            //public byte[]	multi_tube	;/* 複数ｘ線管種類	REV2.10		*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (2 * 32))]
            //public byte[]	table_y		;/* テーブルY軸表示文字	REV7.04	*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (2 * 16))]
            //public byte[]	detector		;/* 検出器名			REV7.04	*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 16))]
            //public byte[]	iifield		;/* I.I.視野			REV7.04 */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (2 * 16))]
            //public byte[]	filter_process;/*ﾌｨﾙﾀ処理の方法 0:FFT 1:ｺﾝﾎﾞﾘｭｰｼｮﾝ*/	// Rev13.00 追加 2007-01-22 やまおか
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (4 * 8))]
            //public byte[]	rfc_char		;/* RFC用文字 [0]:無 [1]:弱 [2]:中 [3]:強*/	// Rev14.2 追加 2008-04-08 YAMAKAGE
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (2 * 8))]
            //public byte[]	m_axis_name	;// m_axis_name[0]:Ｘ線光軸方向	m_axis_name[1]:Ｘ線光軸と垂直方向	//Rev14.24追加 by 間々田 2009-03-10
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3))]
            //public float[]	filter        ;//推奨フィルタ厚[X線I.I.視野][Ｘ線条件]	//Rev15.0追加 by 間々田 2009/04/17
            //public int	ftable_off_ang		;//微調テーブルのオフセット角	//Rev17.10追加 byやまおか 2010/08/25
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (2 * 32))]
            //public byte[]    detector_name     ;//[0]:検出器１の名称[1]：検出器２の名称 //Rev17.03 追加 by 長野 2010/08/20

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString8[] focustype;/* X線焦点  REV1.00			*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString32[] data_mode;/* ｽｷｬﾝﾃﾞｰﾀﾓｰﾄﾞ	Rev3.00		*/

            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] //'v18.00変更 byやまおか 2011/01/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_2014/07/16hata_v19.51反映
            public FixedString32[] scan_mode;/* ｽｷｬﾝﾃﾞｰﾀﾓｰﾄﾞ				*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString32[] multiscan_mode;/* ｽｷｬﾝ条件ﾓｰﾄﾞ			*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public FixedString32 auto_zoom;/* ｵｰﾄｽﾞｰﾑ表示文字				*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public FixedString32 auto_print;/* ｵｰﾄﾌﾟﾘﾝﾄ表示文字			*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public FixedString32 raw_save;/* 生ﾃﾞｰﾀｾｰﾌﾞ表示文字			*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public FixedString32 bhc;/* BHC補正表示文字				*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public FixedString16[] matrixsize;/* ｽｷｬﾝﾏﾄﾘｯｸｽｻｲｽﾞ				*/ //v16.10 変更　2010/02/02 by 長野
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public FixedString16[] scano_matrix;/* ｽｷｬﾉﾏﾄﾘｯｸｽｻｲｽﾞ				*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString16[] scan_speed;/* ｽｷｬﾝ速度表示文字			*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString16[] scano_speed;/* ｽｷｬﾉ速度表示文字			*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString8[] scan_area;/* ｽｷｬﾝ撮影領域表示文字		*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString8[] scano_area;/* ｽｷｬﾉ撮影領域表示文字		*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString8[] slice_wid;/* ｽﾗｲｽ厚表示文字				*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString8[] scano_slice_wid;/* ｽｷｬﾉｽﾗｲｽ厚表示文字		*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString8[] det_ap;/* 検出器開口表示文字			*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString8[] fc;/* ﾌｨﾙﾀ関数					*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public FixedString8 max_view;/* 最大ﾋﾞｭｰ数 REV2.00			*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public FixedString8 min_view;/* 最小ﾋﾞｭｰ数 REV2.00			*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public FixedString8 max_integ_number;/* 最大画像積算枚数 REV2.00	*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public FixedString8 min_integ_number;/* 最小画像積算枚数 REV2.00	*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public FixedString32 fimage_bit;/* 透視画像ビット数 REV M3     */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString16[] multislice;/* 複数同時ｽﾗｲｽ数　REV2.10		*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public FixedString16[] multi_tube;/* 複数ｘ線管種類	REV2.10		*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public FixedString32[] table_y;/* テーブルY軸表示文字	REV7.04	*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public FixedString16[] detector;/* 検出器名			REV7.04	*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString16[] iifield;/* I.I.視野			REV7.04 */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public FixedString16[] filter_process;/*ﾌｨﾙﾀ処理の方法 0:FFT 1:ｺﾝﾎﾞﾘｭｰｼｮﾝ*/	// Rev13.00 追加 2007-01-22 やまおか
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public FixedString8[] rfc_char;/* RFC用文字 [0]:無 [1]:弱 [2]:中 [3]:強*/	// Rev14.2 追加 2008-04-08 YAMAKAGE
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public FixedString8[] m_axis_name;// m_axis_name[0]:Ｘ線光軸方向	m_axis_name[1]:Ｘ線光軸と垂直方向	//Rev14.24追加 by 間々田 2009-03-10
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3))]
            public float[] filter;//推奨フィルタ厚[X線I.I.視野][Ｘ線条件]	//Rev15.0追加 by 間々田 2009/04/17
            
            public int ftable_off_ang;//微調テーブルのオフセット角	//Rev17.10追加 byやまおか 2010/08/25
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public FixedString32[] detector_name;//[0]:検出器１の名称[1]：検出器２の名称 //Rev17.03 追加 by 長野 2010/08/20

            //-->   追加2014/07/16hata_v19.51反映
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public FixedString8[] xfilter_c;  //X線フィルタ(0)～(5) 'v18.00追加 byやまおか 2011/02/11

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] //Rev23.10 SizeConstを2→4に変更 by長野 2015/11/04
            public FixedString8[] xfocus_c;   //X線焦点(0)小(1)大      'v18.00追加 byやまおか 2011/03/15
            //<--

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public FixedString16[] scanSamePositionName;//サンプルの配置位置名(正極・負極等) Rev24.00 by長野 2016/04/06

            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // 配列の初期化
                focustype = FixedString.CreateArray<FixedString8>(3);       // X線焦点  REV1.00
                data_mode = FixedString.CreateArray<FixedString32>(3);      // ﾃﾞｰﾀﾓｰﾄﾞ	Rev3.00
                //scan_mode = FixedString.CreateArray<FixedString32>(3);      // ｽｷｬﾝﾃﾞｰﾀﾓｰﾄﾞ
                scan_mode = FixedString.CreateArray<FixedString32>(4);      // ｽｷｬﾝﾃﾞｰﾀﾓｰﾄﾞ _変更2014/07/16hata_v19.51反映
                multiscan_mode = FixedString.CreateArray<FixedString32>(3); // ｽｷｬﾝ条件ﾓｰﾄﾞ
                auto_zoom.Initialize();                                     // ｵｰﾄｽﾞｰﾑ表示文字
                auto_print.Initialize();                                    // ｵｰﾄﾌﾟﾘﾝﾄ表示文字
                raw_save.Initialize();                                      // 生ﾃﾞｰﾀｾｰﾌﾞ表示文字
                bhc.Initialize();                                           // BHC補正表示文字
                matrixsize = FixedString.CreateArray<FixedString16>(5);     // ｽｷｬﾝﾏﾄﾘｯｸｽｻｲｽﾞ
                scano_matrix = FixedString.CreateArray<FixedString16>(4);   // ｽｷｬﾉﾏﾄﾘｯｸｽｻｲｽﾞ
                scan_speed = FixedString.CreateArray<FixedString16>(3);     // ｽｷｬﾝ速度表示文字
                scano_speed = FixedString.CreateArray<FixedString16>(3);    // ｽｷｬﾉ速度表示文字
                scan_area = FixedString.CreateArray<FixedString8>(3);       // ｽｷｬﾝ撮影領域表示文字
                scano_area = FixedString.CreateArray<FixedString8>(3);      // ｽｷｬﾉ撮影領域表示文字
                slice_wid = FixedString.CreateArray<FixedString8>(3);       // ｽﾗｲｽ厚表示文字
                scano_slice_wid = FixedString.CreateArray<FixedString8>(3); // ｽｷｬﾉｽﾗｲｽ厚表示文字
                det_ap = FixedString.CreateArray<FixedString8>(3);          // 検出器開口表示文字
                fc = FixedString.CreateArray<FixedString8>(3);              // ﾌｨﾙﾀ関数
                max_view.Initialize();                                      // 最大ﾋﾞｭｰ数 REV2.00
                min_view.Initialize();                                      // 最小ﾋﾞｭｰ数 REV2.00
                max_integ_number.Initialize();                              // 最大画像積算枚数 REV2.00
                min_integ_number.Initialize();                              // 最小画像積算枚数 REV2.00
                fimage_bit.Initialize();                                    // 透視画像ビット数 REV M3
                multislice = FixedString.CreateArray<FixedString16>(3);     // 複数同時ｽﾗｲｽ数　REV2.10
                multi_tube = FixedString.CreateArray<FixedString16>(2);     // 複数ｘ線管種類	REV2.10
                table_y = FixedString.CreateArray<FixedString32>(2);        // テーブルY軸表示文字	REV7.04
                detector = FixedString.CreateArray<FixedString16>(2);       // 検出器名			REV7.04
                iifield = FixedString.CreateArray<FixedString16>(3);        // I.I.視野			REV7.04
                filter_process = FixedString.CreateArray<FixedString16>(2); // ﾌｨﾙﾀ処理の方法 0:FFT 1:ｺﾝﾎﾞﾘｭｰｼｮﾝ       Rev13.00 追加 2007-01-22 やまおか
                rfc_char = FixedString.CreateArray<FixedString8>(4);        // RFC用文字 [0]:無 [1]:弱 [2]:中 [3]:強   Rev14.2 追加 2008-04-08 YAMAKAGE
                m_axis_name = FixedString.CreateArray<FixedString8>(2);     // m_axis_name[0]:Ｘ線光軸方向	m_axis_name[1]:Ｘ線光軸と垂直方向  Rev14.24追加 by 間々田 2009-03-10 
                filter = new float[3 * 3];                                   // 推奨フィルタ厚[X線I.I.視野][Ｘ線条件]   Rev15.0追加 by 間々田 2009/04/17
                detector_name = FixedString.CreateArray<FixedString32>(2);  // [0]:検出器１の名称[1]：検出器２の名称 //Rev17.03 追加 by 長野 2010/08/20

                xfilter_c = FixedString.CreateArray<FixedString8>(6);       // 'X線フィルタ(0)～(5)    'v18.00追加 byやまおか 2011/02/11_変更2014/07/16hata_v19.51反映
                xfocus_c = FixedString.CreateArray<FixedString8>(4);        // 'X線焦点(0)小(1)大      'v18.00追加 byやまおか 2011/03/15_変更2014/07/16hata_v19.51反映 //Rev23.10 (2)→(4)に変更 by長野 2015/11/05

                scanSamePositionName = FixedString.CreateArray<FixedString16>(4);//サンプルの配置位置名(正極・負極等) Rev24.00 by長野 2016/04/06
            }
            #endregion
        
        }   // INFDEF
        #endregion INFDEF

        #region CTINFDEF
        /// <summary>
        /// CT付帯情報初期化情報
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct CTINFDEF
        {
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (9 * 2))]
            //public byte[]	d_rawsts		;/* 生ﾃﾞｰﾀ･ｽﾃｰﾀｽ                */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 6))]
            //public byte[]	d_recokind	;/* 付帯情報ﾃﾞｰﾀ種類（再構成種）*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 6))]
            //public byte[]	full_mode		;/* ｽｷｬﾝﾓｰﾄﾞ（FULLﾓｰﾄﾞ）		*/ 
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (6 * 4))]
            //public byte[]	matsiz		;/* 画像ﾏﾄﾘｯｸｽ                  */ //v16.10 変更 by 長野　2010/02/02
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (4 * 6))]
            //public byte[]	scan_mode		;/* ｽｷｬﾝ･ﾓｰﾄﾞ                   */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 6))]
            //public byte[]	scan_speed	;/* ｽｷｬﾝ速度                    */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 8))]
            //public byte[]	scan_time		;/* ｽｷｬﾝ時間                    */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 2))]
            //public byte[]	scan_area		;/* 撮影領域                    */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 2))]
            //public byte[]	slice_wid		;/* ｽﾗｲｽ幅番号                  */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 2))]
            //public byte[]	det_ap		;/* 開口番号                    */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 4))]
            //public byte[]	focus		;/* 焦点番号  REV1.00			*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (6 * 16))]
            //public byte[]	focustype	;/* Ｘ線焦点（形式）  REV1.00	*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 2))]
            //public byte[]	energy		;/* Ｘ線条件                    */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 6))]
            //public byte[]	tilt_angle	;/* ﾃﾞｰﾀ収集傾斜角度            */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 8))]
            //public byte[]	fc			;/* ﾌｨﾙﾀ関数                    */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (5 * 2))]
            //public byte[]	sift_pos		;/* ｼﾌﾄ位置                     */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 6))]
            //public byte[]	scano_dir		;/* ｽｷｬﾉ収集時の管球方向角度	*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 2))]
            //public byte[]	pro_dir		;/* 試料挿入方向                */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 4))]
            //public byte[]	view_dir		;/* 試料観察（表示）方向        */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (5 * 2))]
            //public byte[]	pro_posdir	;/* 試料位置方向                */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            //public byte[]	scano_dispdir	;/* ｽｷｬﾉ表示方向VSN             */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (4 * 8))]
            //public byte[]	rotation		;/* ﾃﾞｰﾀ収集開始位置情報        */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            //public byte[]	w_lamp_size		;/* warningﾗﾝﾌﾟ表示ｻｲｽﾞ         */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (4 * 6))]
            //public byte[]	imgsize_2		;/* 1画像ﾃﾞｰﾀｻｲｽﾞ               */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (4 * 4))]
            //public byte[]	scano_matsiz	;/* ｽｷｬﾉ画像ﾏﾄﾘｯｸｽ              */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public FixedString2[] d_rawsts;/* 生ﾃﾞｰﾀ･ｽﾃｰﾀｽ                */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString6[] d_recokind;/* 付帯情報ﾃﾞｰﾀ種類（再構成種）*/

            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]    //v18.00変更 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_変更2014/07/16hata_v19.51反映
            public FixedString6[] full_mode;/* ｽｷｬﾝﾓｰﾄﾞ（FULLﾓｰﾄﾞ）		*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public FixedString4[] matsiz;/* 画像ﾏﾄﾘｯｸｽ                  */ //v16.10 変更 by 長野　2010/02/02

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public FixedString6[] scan_mode;/* ｽｷｬﾝ･ﾓｰﾄﾞ                   */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString6[] scan_speed;/* ｽｷｬﾝ速度                    */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString8[] scan_time;/* ｽｷｬﾝ時間                    */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString2[] scan_area;/* 撮影領域                    */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString2[] slice_wid;/* ｽﾗｲｽ幅番号                  */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString2[] det_ap;/* 開口番号                    */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString4[] focus;/* 焦点番号  REV1.00			*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public FixedString16[] focustype;/* Ｘ線焦点（形式）  REV1.00	*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString2[] energy;/* Ｘ線条件                    */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString6[] tilt_angle;/* ﾃﾞｰﾀ収集傾斜角度            */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString8[] fc;/* ﾌｨﾙﾀ関数                    */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public FixedString2[] sift_pos;/* ｼﾌﾄ位置                     */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString6[] scano_dir;/* ｽｷｬﾉ収集時の管球方向角度	*/
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString2[] pro_dir;/* 試料挿入方向                */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public FixedString4[] view_dir;/* 試料観察（表示）方向        */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public FixedString2[] pro_posdir;/* 試料位置方向                */
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public FixedString4 scano_dispdir;/* ｽｷｬﾉ表示方向VSN             */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public FixedString8[] rotation;/* ﾃﾞｰﾀ収集開始位置情報        */
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public FixedString6 w_lamp_size;/* warningﾗﾝﾌﾟ表示ｻｲｽﾞ         */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public FixedString6[] imgsize_2;/* 1画像ﾃﾞｰﾀｻｲｽﾞ               */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public FixedString4[] scano_matsiz;/* ｽｷｬﾉ画像ﾏﾄﾘｯｸｽ              */

            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // 配列の初期化
                d_rawsts = FixedString.CreateArray<FixedString2>(9);    // 生データ・ステータス
                d_recokind = FixedString.CreateArray<FixedString6>(3);  // 付帯情報データ種類（再構成種）
                //full_mode = FixedString.CreateArray<FixedString6>(3);   // スキャンモード（FULLモード）
                full_mode = FixedString.CreateArray<FixedString6>(4);   // スキャンモード（FULLモード）//v18.00変更 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_変更2014/07/16hata_v19.51反映

                matsiz = FixedString.CreateArray<FixedString4>(6);      // 画像マトリクス
                scan_mode = FixedString.CreateArray<FixedString6>(4);   // スキャン・モード
                scan_speed = FixedString.CreateArray<FixedString6>(3);  // スキャン速度
                scan_time = FixedString.CreateArray<FixedString8>(3);   // スキャン時間
                scan_area = FixedString.CreateArray<FixedString2>(3);   // 撮影領域
                slice_wid = FixedString.CreateArray<FixedString2>(3);   // スライス幅番号
                det_ap = FixedString.CreateArray<FixedString2>(3);      // 開口番号
                focus = FixedString.CreateArray<FixedString4>(3);       // 焦点番号  REV1.00
                focustype = FixedString.CreateArray<FixedString16>(6);  // Ｘ線焦点（形式）  REV1.00
                energy = FixedString.CreateArray<FixedString2>(3);      // Ｘ線条件
                tilt_angle = FixedString.CreateArray<FixedString6>(3);  // データ収集傾斜角度
                fc = FixedString.CreateArray<FixedString8>(3);          // フィルタ関数
                sift_pos = FixedString.CreateArray<FixedString2>(5);    // シフト位置
                scano_dir = FixedString.CreateArray<FixedString6>(3);   // スキャノ収集時の管球方向角度
                pro_dir = FixedString.CreateArray<FixedString2>(3);     // 試料挿入方向
                view_dir = FixedString.CreateArray<FixedString4>(3);    // 試料観察（表示）方向
                pro_posdir = FixedString.CreateArray<FixedString2>(5);  // 試料位置方向
                scano_dispdir.Initialize();                             // スキャノ表示方向VSN
                rotation = FixedString.CreateArray<FixedString8>(4);    // データ収集開始位置情報
                w_lamp_size.Initialize();                               // warningランプ表示サイズ
                imgsize_2 = FixedString.CreateArray<FixedString6>(4);   // １画像データサイズ
                scano_matsiz = FixedString.CreateArray<FixedString4>(4);// スキャノ画像マトリクス
            }
            #endregion
        
        }   // CTINFDEF
        #endregion CTINFDEF

        #region T20KINF
        /// <summary>
        /// TOSCANER-20000ﾏｽﾀｰ情報（ｼｽﾃﾑ制御情報）
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct T20KINF
        {
            //public int	cpuready			;/* cpu ready信号ﾓｰﾄﾞ			*/	//未使用
            //public int	scan_flag			;/* ｽｷｬﾝ/ｽｷｬﾉ/再構成実行ﾓｰﾄﾞ	*/	//未使用
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            //public byte[]	system_name		;/* ｼｽﾃﾑ名						*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            //public byte[]	system_type		;/* ｼｽﾃﾑﾀｲﾌﾟ					*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            //public byte[]	version			;/* ｿﾌﾄﾊﾞｰｼﾞｮﾝ情報				*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            //public byte[]	ct_gentype		;/* CTﾃﾞｰﾀ収集方式（世代）		*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            //public byte[]	now_hdcopy		;/* 現在使用可能画像ﾊｰﾄﾞｺﾋﾟｰ装置*/	//未使用
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            //public byte[]	now_3ddisplay	;/* 現在使用可能三次元表示		*/	//未使用
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            //public int[]	type_info		;/* ﾀｽｸ管理情報					*/	//未使用
            //public int	upper_limit			;/* 昇降上限値（mm） REV1.00 FLOAT->LONG			*/
            //public int	lower_limit			;/* 昇降下限値（mm） REV1.00 FLOAT->LONG			*/
            //public int	ups_power			;/* 100V電源異常(0:正常/1:UPS運転)  REV1.00 Append	*/
            //public int	ud_type				;/* ﾃｰﾌﾞﾙ昇降/検出器昇降 REV2.00*/
            //public int	v_capture_type		;/* フレームグラバー種類 REV17.10用途変更	*/
            //public int	ct30k_running		;									//未使用
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            //public byte[]	instance_uid	;/* ｲﾝｽﾀﾝｽUID			REV6.00 */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            //public byte[]	macaddress		;/* MACｱﾄﾞﾚｽを10進数にしたもの  REV6.00 */
            // [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            //public byte[]	dummy		    ;/* 調整用(これがないと後のデータがずれます) */
            //public float	fx_lower_limit		;/* 微調テーブルＸ軸の下限値（mm）REV8.00 	*/
            //public float	fx_upper_limit		;/* 微調テーブルＸ軸の上限値（mm）REV8.00 	*/
            //public float	fy_lower_limit		;/* 微調テーブルＹ軸の下限値（mm）REV8.00 	*/
            //public float	fy_upper_limit		;/* 微調テーブルＹ軸の上限値（mm）REV8.00 	*/

            public int cpuready;/* cpu ready信号ﾓｰﾄﾞ			*/	//未使用
            
            public int scan_flag;/* ｽｷｬﾝ/ｽｷｬﾉ/再構成実行ﾓｰﾄﾞ	*/	//未使用
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public FixedString16 system_name;/* ｼｽﾃﾑ名						*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public FixedString16 system_type;/* ｼｽﾃﾑﾀｲﾌﾟ					*/

            public FixedString16 system_type2;/* ｼｽﾃﾑﾀｲﾌﾟ2					*/ //Rev23.10 X線切替用に追加 by長野 2015/09/18

            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public FixedString6 version;/* ｿﾌﾄﾊﾞｰｼﾞｮﾝ情報				*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public FixedString2 ct_gentype;/* CTﾃﾞｰﾀ収集方式（世代）		*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public FixedString16 now_hdcopy;/* 現在使用可能画像ﾊｰﾄﾞｺﾋﾟｰ装置*/	//未使用
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public FixedString16 now_3ddisplay;/* 現在使用可能三次元表示		*/	//未使用
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public int[] type_info;/* ﾀｽｸ管理情報					*/	//未使用
            
            public int upper_limit;/* 昇降上限値（mm） REV1.00 FLOAT->LONG			*/
            
            public int lower_limit;/* 昇降下限値（mm） REV1.00 FLOAT->LONG			*/
            
            public int ups_power;/* 100V電源異常(0:正常/1:UPS運転)  REV1.00 Append	*/
            
            public int ud_type;/* ﾃｰﾌﾞﾙ昇降/検出器昇降 REV2.00*/
            
            public int v_capture_type;/* フレームグラバー種類 REV17.10用途変更	*/
            
            public int ct30k_running;									//未使用
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public FixedString64 instance_uid;/* ｲﾝｽﾀﾝｽUID			REV6.00 */
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            public FixedString18 macaddress;/* MACｱﾄﾞﾚｽを10進数にしたもの  REV6.00 */
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public FixedString2 dummy;/* 調整用(これがないと後のデータがずれます) */
            
            public float fx_lower_limit;/* 微調テーブルＸ軸の下限値（mm）REV8.00 	*/
            
            public float fx_upper_limit;/* 微調テーブルＸ軸の上限値（mm）REV8.00 	*/
            
            public float fy_lower_limit;/* 微調テーブルＹ軸の下限値（mm）REV8.00 	*/
            
            public float fy_upper_limit;/* 微調テーブルＹ軸の上限値（mm）REV8.00 	*/

            public float y_axis_upper_limit;/* Y軸の上限値（mm）REV20.00 by長野 2015/02/16 	*/

            public float y_axis_lower_limit;/* Y軸の下限値（mm）REV20.00 by長野 2015/02/16 	*/

            public int scanCTNo;	        //スキャンしたCT装置の識別用番号(1,2,3･･･) Rev24.00 追加 by長野 2016/04/06

            public FixedString16 scanCTName;  //スキャンした装置の識別名 Rev24.00 by長野 2016/04/06

            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // 配列の初期化
                system_name.Initialize();               // システム名
                system_type.Initialize();               // システムタイプ
                version.Initialize();                   // ｿﾌﾄﾊﾞｰｼﾞｮﾝ情報
                ct_gentype.Initialize();                // CTﾃﾞｰﾀ収集方式（世代）
                now_hdcopy.Initialize();                // 現在使用可能画像ﾊｰﾄﾞｺﾋﾟｰ装置    未使用
                now_3ddisplay.Initialize();             // 現在使用可能三次元表示          未使用
                type_info = new int[16];                // ﾀｽｸ管理情報                     未使用
                instance_uid.Initialize();              // ｲﾝｽﾀﾝｽUID			REV6.00
                macaddress.Initialize();                // MACｱﾄﾞﾚｽを10進数にしたもの  REV6.00
                scanCTName.Initialize();                // スキャンした装置の識別名 Rev24.00 by長野 2016//04/06
                dummy.Initialize();                     //調整用
            }
            #endregion       
        
        }   //T20KINF
        #endregion T20KINF

        #region SCANCONDPAR
        /// <summary>
        /// ﾃﾞｰﾀ収集条件ﾊﾟﾗﾒｰﾀ
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SCANCONDPAR
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3))]
	        public float[]	tbldia		;/* ﾃｰﾌﾞﾙ直径[焦点][ｴﾘｱ]		*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3))]
	        public float[]	slicewd		;/* ｽﾗｲｽ幅[焦点][ｴﾘｱ]			*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public float[]	realfdd			;/* 焦点X線検出器距離[焦点]		*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3))]
	        public float[]	fcd			;/* 焦点中心点[焦点][ｴﾘｱ]		*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public float[]	fanangle		;/* X線ﾌｧﾝ角[焦点]				*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	mainch			;/* ﾒｲﾝﾁｬﾝﾈﾙ[焦点]				*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	refch			;/* ﾚﾌｧﾚﾝｽﾁｬﾝﾈﾙ[焦点]			*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	dtunitno			;/* ﾒｲﾝﾁｬﾝﾈﾙﾕﾆｯﾄ[焦点]			*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	dtunitch			;/* ﾒｲﾝﾁｬﾝﾈﾙﾕﾆｯﾄ数[焦点]		*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public float[]	dtpitch			;/* 検出器ﾋﾟｯﾁ[焦点]			*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	trvsno			;/* ﾄﾗﾊﾞｰｽ数[焦点]				*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3))]
	        public float[]	trpitch		;/* ﾄﾗﾊﾞｰｽﾃﾞｰﾀﾋﾟｯﾁ[焦点][ｴﾘｱ]	*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3))]
	        public int[]	trvspt		;/* ﾄﾗﾊﾞｰｽﾃﾞｰﾀﾎﾟｲﾝﾄ[焦点][ｴﾘｱ]	*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	dmych			;/* ﾀﾞﾐｰﾁｬﾝﾈﾙ数[焦点]			*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	offsetapr		;/* ｵﾌｾｯﾄﾃﾞｰﾀ捨てﾎﾟｲﾝﾄ[焦点]	*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	offsetpt			;/* ｵﾌｾｯﾄﾃﾞｰﾀ収集ﾎﾟｲﾝﾄ[焦点]	*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	airdtpt			;/* 空気補正ﾃﾞｰﾀ収集ﾎﾟｲﾝﾄ[焦点] */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3))]
	        public int[]	aprtpt		;/* ﾃﾞｰﾀ収集捨てﾎﾟｲﾝﾄ[焦点][ｴﾘｱ]*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3))]
	        public int[]	sclsiz		;/* 再構成画素数[焦点][ｴﾘｱ]		*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3 * 3 * 3 * 3 * 3))]
	        public int[]	scanbias 	;/* ｽｷｬﾝﾊﾞｲｱｽ値 		*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3 * 3 * 3 * 3 * 3))]
	        public float[]	scanslop 	;/* ｽｷｬﾝｽﾛｰﾌﾟ値			*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scanotvs			;/* ｽｷｬﾉ昇降ﾃﾞｰﾀ収集回数[焦点]	*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scanoapr			;/* ｽｷｬﾉ昇降ﾃﾞｰﾀ収集捨て数[焦点]*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scanopt			;/* ｽｷｬﾉ昇降ﾃﾞｰﾀ収集数[焦点]	*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scanoscl			;/* ｽｷｬﾉ画素数[焦点]			*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scanoair			;/* ｽｷｬﾉ空気補正ﾃﾞｰﾀﾎﾟｲﾝﾄ[焦点]	*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3 * 3 * 3 * 3 * 3))]
	        public int[]	scanobias	;/* ｽｷｬﾉﾊﾞｲｱｽ値			*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3 * 3 * 3 * 3 * 3))]
	        public float[]	scanoslop	;/* ｽｷｬﾉｽﾛｰﾌﾟ値			*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3))]
	        public float[]	scano_tblbias	;/* ｽｷｬﾉﾃｰﾌﾞﾙﾊﾞｲｱｽ値[焦点]		*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3))]
	        public float[]	scano_udpitch	;/* ｽｷｬﾉ昇降ﾋﾟｯﾁ[焦点]			*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3 * 3 * 3 * 4))]
	        public int[]	scaninit	;/* ｽｷｬﾝN1･N2･N3係数		*/	//マイクロＣＴでは未使用
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3 * 3 * 4 * 4))]
	        public int[]	scanoinit;/* ｽｷｬﾉN1･N2･N3係数		*/	//マイクロＣＴでは未使用
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public int[]	scan_stroke		;/* ｽｷｬﾝ可能昇降ｽﾄﾛｰｸ			*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public float[]	scano_dp_ratio	;/* ｽｷｬﾉ表示時の縦横比			*/
	        public int	    fimage_hsize		;/* 透視画像横ｻｲｽﾞ REV2.00		*/
	        public int	    fimage_vsize		;/* 透視画像縦ｻｲｽﾞ REV2.00		*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (5 * 6))]
	        public float[]	a				;/* 幾何学補正係数[A0/A1/A2/A3/A4/A5] REV2.00	-> REV2.10 配列変更	*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
	        public int[]	xls				;/* 有効ﾃﾞｰﾀ開始画素	REV2.00					-> REV2.10 配列変更	*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
	        public int[]	xle				;/* 有効ﾃﾞｰﾀ画素		REV2.00					-> REV2.10 配列変更	*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
	        public float[]	xlc				;/* ｾﾝﾀｰﾁｬﾝﾈﾙ画素		REV2.00					-> REV2.10 配列変更 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
	        public float[]	scan_posi_a		;/* ｽｷｬﾝ位置を示す一次直線の傾き REV2.00		 -> REV2.10 配列変更 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
	        public float[]	scan_posi_b		;/* ｽｷｬﾝ位置を示す一次直線の切片 REV2.00		 -> REV2.10 配列変更 */
	        public int	    fimage_bit			;/* 透視画像ﾋﾞｯﾄ数		REV2.00	*/
           
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (5 * 3))]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (5 * 4))]  //v18.00変更 2->3 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野_変更2014/07/16hata_v19.51反映
            public float[] mfanangle;/* MCT用ﾌｧﾝ角		REV2.00					    -> REV2.10 配列変更	*/

            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (5 * 3))]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (5 * 4))]  //v18.00変更 2->3 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野_変更2014/07/16hata_v19.51反映
            public float[] mcenter_angle;/* 回転中心角度 REV2.00 -> REV2.10 配列変更		*/

            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (5 * 3))]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (5 * 4))]  //v18.00変更 2->3 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_変更2014/07/16hata_v19.51反映
            public float[] mcenter_channel;/* 再構成用ｾﾝﾀｰﾁｬﾝﾈﾙ REV2.00				-> REV2.10 配列変更	*/

            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = (5 * 3))]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (5 * 4))]  //v18.00変更 2->3 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_変更2014/07/16hata_v19.51反映
            public float[] mchannel_pitch;/* 1ﾁｬﾝﾈﾙ当たりの角度	  REV2.00				-> REV2.10 配列変更	*/

            public float	recon_start_angle	;/* 画像再構成開始角度    REV2.00 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public float[]   mdtpitch         ;/* 検出器ピッチ（ｍｍ）						 -> REV2.10 配列変更 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public float[]   fid_offset       ;/* ＦＩＤへの加算値(mm)REV M3					 -> REV2.10 配列変更 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public float[]   fcd_offset       ;/* ＦＣＤへの加算値(mm)REV M3					 -> REV2.10 配列変更 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (2 * 3))]
            public float[] frame_rate;/* 外部トリガのフレームレート						REV7.04 変更     */
            //public float[] frame_rate;/* 外部トリガのフレームレート						REV7.04 変更     */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public float[]  	v_mag      		;/* カメラ縦ビニング倍率 		REV7.04 2003-09-01 変更 by 間々田 */
	        public float    ver_wire_pitch      ;/* 垂直線ワイヤピッチ(cm)							REV2.10 追加     */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
	        public float[]	b				;/* ｺｰﾝﾋﾞｰﾑ用幾何歪補正係数		Rev3.00	*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]    //v18.00変更 1->2 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public int[] n1;/* 有効ﾃﾞｰﾀ開始ﾁｬﾝﾈﾙ			Rev3.00	*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]    //v18.00変更 1->2 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public int[] n2;/* 有効ﾃﾞｰﾀ終了ﾁｬﾝﾈﾙ			Rev3.00	*/
	        
            public float	delta_theta			;/* n方向ﾃﾞｰﾀﾋﾟｯﾁ(radian)		Rev3.00	*/
	        public float	dpm					;/* m方向ﾃﾞｰﾀﾋﾟｯﾁ(mm)			Rev3.00	*/
	        public float	n0					;/* n方向画像中心対応ﾁｬﾝﾈﾙ		Rev3.00	*/
	        public float	m0					;/* m方向画像中心対応ﾁｬﾝﾈﾙ		Rev3.00	*/

            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]    //v18.00変更 1->2 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public float[] theta0;/* 有効ﾌｧﾝ角(radian)			Rev3.00	*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]    //v18.00変更 1->2 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public float[] theta01;/* 有効ﾃﾞｰﾀ包含ﾌｧﾝ角1(radian)	Rev3.00	*/
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]    //v18.00変更 1->2 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public float[] theta02;/* 有効ﾃﾞｰﾀ包含ﾌｧﾝ角2(radian)	Rev3.00	*/
	        
            public float	thetaoff			;/* ｵﾌｾｯﾄ角(radian)				Rev3.00	*/
	        public int	    ioff				;/* ｵﾌｾｯﾄ識別値					Rev3.00	*/
	        public float	alpha				;/* ｵｰﾊﾞｰﾗｯﾌﾟ角度(radian)		Rev3.00	*/	/*  オーバースキャン角度（radian）に変更	REV9.00 修正 2004-01-29 by 間々田 */
	        public float	nc					;/* 回転中心ﾁｬﾝﾈﾙ				Rev3.00	*/
	        public int	    klimit				;/* ｽﾗｲｽ枚数制限値				Rev3.00	*/
	        public float	fcd_limit			;/* ﾃｰﾌﾞﾙとX線管の干渉する限界 (mm)	REV4.00 */
	        public float	fud_limit			;/* ﾃｰﾌﾞﾙとX線管の干渉する限界高さ(mm) REV6.00 */
	        public float	fud_step			;/* 精細昇降移動量 (mm)				REV4.00 */
	        public float	rud_step			;/* 粗昇降移動量 (mm)				REV4.00 */
	        public float	fud_start			;/* 精細昇降開始位置 (mm)			REV4.00 */
	        public float	fud_end				;/* 精細昇降終了位置 (mm)			REV4.00 */
	        public float	rud_start			;/* 粗昇降開始位置 (mm)				REV4.00 */
	        public float	rud_end				;/* 粗昇降終了位置 (mm)				REV4.00 */
	        public float	y_incli				;/* Y軸の傾き (rad)					REV6.00 */
	        public float	xposition			;/* X座標 (mm)						REV6.00 */
	        public int	    study_id			;/* 検査ID							REV6.00 */
	        public int	    acq_num				;/* 収集番号						REV6.00 */
	        public int	    series_num			;/* ｼﾘｰｽﾞ番号						REV6.00 */
	        public int	    scan_comp			;/* ｽｷｬﾝ済ﾌﾗｸﾞ 0:未ｽｷｬﾝ 1:ｽｷｬﾝ済	REV6.00 */
	        public float	fs					;/* 焦点ｻｲｽﾞ(mm)					REV6.00 */
	        public float	x0					;/* 骨塩等価物質密度(mg/cm3)		REV6.00 */
	        public float	x1					;/* 骨塩等価物質密度(mg/cm3)		REV6.00 */
	        public float	x2					;/* 骨塩等価物質密度(mg/cm3)		REV6.00 */
	        public float	x3					;/* 骨塩等価物質密度(mg/cm3)		REV6.00 */
	        public float	x4					;/* 骨塩等価物質密度(mg/cm3)		REV6.00 */
	        public float	x_offset			;/* SinoCenter用Xオフセット(mm)		REV7.00 */
	        public float	ref_fid				;/*	y_incliを求めたときのFID(mm)	REV7.02 */
	        public int  	detector			;/* カメラ／フラットパネルの切替え 	REV7.03 2003-03-06 追加 by 間々田 */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (2 * 3))]
            public FixedString256[] dcf;/* カメラ情報ファイル名 		REV7.04 2003-03-06 変更 by 間々田 */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public float[]  	h_mag      			;/* カメラ横ビニング倍率 		REV7.04 2003-09-01 変更 by 間々田 */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public float[]  	pulsar_v_mag      	;/* frmPulsarの縦倍率 			REV7.04 2003-09-01 変更 by 間々田 */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public float[]  	pulsar_h_mag      	;/* frmPulsarの横倍率 			REV7.04 2003-09-01 変更 by 間々田 */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public float[]  	fpulsar_v_mag      	;/* frmFPulsarの縦倍率 			REV7.04 2003-09-01 変更 by 間々田 */
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public float[]  	fpulsar_h_mag      	;/* frmFPulsarの横倍率 			REV7.04 2003-09-01 変更 by 間々田 */
	        
            public float  	fpd_pitch      			;/* フラットパネルのチャンネルピッチ(mm)	REV7.04 2003-09-01 変更 by 間々田 */
	        public float  	rot_max_speed      		;/* テーブルの最大回転速度(rpm) 			REV7.04 2003-09-01 変更 by 間々田 */
	        public float  	scan_start_angle		;/* スキャン開始角度(degree) 				REV7.04 2003-09-01 変更 by 間々田 */
	        public float  	rc_slope_ft				;/* コーンビーム回転中心傾き微調整(degree) 	REV7.04 2003-09-01 変更 by 間々田 */
	        public float  	dc_center_ft_x			;/* コーンビーム幾何歪画像中心x微調整 		REV7.04 2003-09-01 変更 by 間々田 */
	        public float  	dc_center_ft_y			;/* コーンビーム幾何歪画像中心y微調整 		REV7.04 2003-09-01 変更 by 間々田 */
	        public int	    h_size					;/* 透視画像 横サイズ						REV7.04 2003-09-27 追加 by 間々田*/
	        public int	    v_size					;/* 透視画像 縦サイズ						REV7.04 2003-09-27 追加 by 間々田*/
	        public float  	ftable_max_speed		;/* 微調テーブルの最大速度(mm/s)			REV7.04 2003-11-06 追加 by 間々田 */
	        public float  	ud_max_speed      		;/* 昇降速度の最大値（mm/s）				REV8.00 */

            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public FixedString4 distribute_drive;/* 分散処理時の画像保存先ドライブ			REV8.00 */
	                    
            public int  	distribute_pc_no		;/* 分散処理時のＰＣ番号					REV8.00 */
 	        public int	    xrot_start_pos			;/* Ｘ線管回転データ収集開始角度（degree）		REV9.00 追加 2004-01-29 by 間々田 */
 	        public int	    xrot_end_pos			;/* Ｘ線管回転データ収集終了角度（degree）		REV9.00 追加 2004-01-29 by 間々田 */
	        //public float	magnify_para			;/* モニタ上の拡大倍率係数						REV9.00 追加 2004-01-29 by 間々田 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[]    magnify_para;         /* モニタ上の拡大倍率係数						REV9.00 追加 2004-01-29 by 間々田 */ //Rev23.10 配列無し→配列[3]に変更 by長野 2015/11/04

            public float	recon_offset_angle		;/* Ｘ線管回転軸の原点補正角度（degree）		REV9.00 追加 2004-01-29 by 間々田 */
	        public float	rnw1					;/* オフセットウェイト掛け始点（0≦rnw1≦1）	REV9.00 追加 2004-01-29 by 間々田 */
	        public float	rnw2					;/* オフセットウェイト掛け終点（rnw1＜rnw2≦2）	REV9.00 追加 2004-01-29 by 間々田 */
	        public float	alpha_h					;/* ハーフスキャンオーバー角度（radian）		REV9.00 追加 2004-01-29 by 間々田 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
	        public float[]	max_mfanangle		;/* 最大ファン角								REV9.00 追加 2004-04-19 by 間々田 */
	        public float	cone_max_mfanangle		;/* 最大ファン角(コーンビーム用)				REV9.00 追加 2004-04-19 by 間々田 */
	        public int	    dev_klimit				;/* 分割再構成用パラメータ(コーンビーム用)		REV9.2 追加 2004-06-14 by 間々田 */
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3))] //Rev23.10 配列を[3*3]に変更 by長野 2015/11/04
            public float[]	detector_pitch		;/* ii視野別の検出器ピッチ						REV9.6 追加 2004-10-20 by 間々田 */
	        public float	noise_cpc				;/* ノイズ圧縮係数								REV9.7 追加 2004-12-08 by 間々田 */

            public int dummy;   //'Rev18.00 イキ  'v19.50 v19.41とv18.02の統合 by長野 2013/11/05 '調整用（次の項目がdoubleなので，ここまでで8バイトの倍数の大きさにしておきます。そうしないと後のデータがずれます）//追加2014/07/16hata_v19.51反映

            public int	    thinning_frame_num		;//スキャン中透視画像間引きフレーム数			v15.00追加 by 間々田 2008-12-24 末尾に追加するとVB側で落ちるのでここに追加
	        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
            public double[]	alk					;//２次元幾何歪補正パラメータ（Ｘ方向）			v11.20追加 by 間々田 2005-10-06
	        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
            public double[]	blk					;//２次元幾何歪補正パラメータ（Ｙ方向）			v11.20追加 by 間々田 2005-10-06
	        public int	    ist						;//幾何歪補正範囲のＸ方向の始点					v11.20追加 by 間々田 2005-10-06
	        public int	    ied						;//幾何歪補正範囲のＹ方向の始点					v11.20追加 by 間々田 2005-10-06
	        public int	    jst						;//幾何歪補正範囲のＸ方向の終点					v11.20追加 by 間々田 2005-10-06
	        public int	    jed						;//幾何歪補正範囲のＹ方向の終点					v11.20追加 by 間々田 2005-10-06
	        public float	cone_scan_posi_a		;//フル２次元歪補正コーン用スキャン位置の傾き	v11.20追加 by 間々田 2005-10-06
	        public float	cone_scan_posi_b		;//フル２次元歪補正コーン用スキャン位置の切片	v11.20追加 by 間々田 2005-10-06
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 2))]
            public float[]	rcy        		;//回転中心Ｙ座標[I.I.視野][測定位置]						v15.0追加 by 間々田 2009/05/11
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 2))]
            public float[]	rcfcd         	;//回転中心ＦＣＤ[I.I.視野][測定位置]      					v15.0追加 by 間々田 2009/05/11
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 2))]
            public float[]	rc_full_area   	;//回転中心FullScan最大スキャンエリア[I.I.視野][測定位置]   v15.0追加 by 間々田 2009/05/11
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[]	rcfdd   				;//回転中心テーブル測定時のＦＤＤ[I.I.視野]					v15.0追加 by 間々田 2009/05/14
	        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3))]
            public float[]	autotbl_para		;//断面上でROI指定することにより、テーブル自動移動するためのパラメータ[I,I視野サイズ][a,b,c] [0]:a [1]:b [2]:c v15.0追加 by 長野　2009-07-02  配列の並びを変更 09-07-24 by IWASAWA
	        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[]	autotbl_v_xray_pos	;//断面上でROI指定することにより、テーブル自動移動するための基準Y(光軸と垂直)座標		v15.0追加 by 長野　2009-07-02
	        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[]	autotbl_kof   		;//断面上でROI指定することによりテーブル自動移動するためのオフセット率					v15.0追加 by 長野　2009-07-02
	        public int	    smoothcone_min_integ	;//連続回転コーン最小積算枚数				Rev15.99 追加 by YAMAKAGE 10-01-19
	        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public float[]	mbhc_a				;//BHCパラメータ
	        public float	mbhc_p0					;//BHCパラメータP0
	        public float	mbhc_bi_increment		;//BHC biインクリメント係数
	        public float	mbhc_p0_const			;//BHC不変パラメータP0
	        public float    mbhc_airLogValue		;//エアデータから計算したP値 追加 by長野 Rev19.00 2012/5/12

            //v1951-->  //追加2014/07/16hata_v19.51反映
            public float det_sft_length;            //検出器シフトスキャンのシフト量(mm)                 'v18.00追加 byやまおか 2011/01/31 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public int   det_sft_pix;               //検出器シフトスキャンのシフト量(画素)               'v18.00追加 byやまおか 2011/01/31 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public float det_sft_length_r;          //検出器シフトスキャンの右シフト量(mm)				//v23.20追加 by長野 2015/11/19
            public int   det_sft_pix_r;             //検出器シフトスキャンの右シフト量(画素)            //v23.20追加 by長野 2015/11/19
            public float det_sft_length_l;          //検出器シフトスキャンの左シフト量(mm)				//v23.20追加 by長野 2015/11/19
            public int   det_sft_pix_l;             //検出器シフトスキャンの左シフト量(画素)            //v23.20追加 by長野 2015/11/19
            public int   inv_pix;                   //検出器両サイド(片側あたり)の無効画素(画素)         'v18.00追加 byやまおか 2011/01/31 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public int   sft_wt_flt_pix;            //検出器シフト生データ合成ウェイト1:1の長さ(画素)    'v18.00追加 byやまおか 2011/01/31 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public float alw_autocentering_jdgCh;   //マルチスキャン中、前回のオートセンタリングのchを採用するとき場合のズレch数(小数点有り) 'v19.50 by長野 2013/12/09
            //<--v1951

            //Rev22.00 --> 追加 by長野 2015/08/21
            public float tilt_and_rot_r_max_speed; //回転傾斜テーブルの回転最大速度
            public float tilt_and_rot_t_max_speed; //回転傾斜テーブルの傾斜最大速度
            //<--Re22.00

            //Rev23.30 --> 追加 by長野 2016/02/5
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] ExObsCamPixSize;            //外観カメラ画像上の1画素サイズ([0]:非ズーム時,[1]:ズーム時,[2]未使用)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] ExObsCamPixSizeOnFTable;    //外観カメラ画像上の1画素サイズ(微調テーブル上)([0]:非ズーム時,[1]:ズーム時,[2]未使用)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public int[] ExObsCamMaxArea;            //外観カメラ画像上で描画可能な円形ROIの最大直径(mm)([0]:非ズーム時,[1]:ズーム時,[2]未使用)
            //<---

            //public float dummy                      ;//ダミー(VB側で落ちるのでここに追加)						v15.0追加 by 間々田 2009/05/14
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            //public float[] dummy;//ダミー(VB側で落ちるのでここに追加)						v15.0追加 by 間々田 2009/05/14 'v19.50 'v19.41とv18.02の統合 名前が重複するためdummy→dummy2に変更 by長野 2013/11/05
            public float[] dummy2;//ダミー(VB側で落ちるのでここに追加)						v15.0追加 by 間々田 2009/05/14 'v19.50 'v19.41とv18.02の統合 名前が重複するためdummy→dummy2に変更 by長野 2013/11/05

            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // 配列の初期化
                tbldia = new float[3 * 3];                   // ﾃｰﾌﾞﾙ直径[焦点][ｴﾘｱ]
                slicewd = new float[3 * 3];                  // ｽﾗｲｽ幅[焦点][ｴﾘｱ]			
                realfdd = new float[3];                     // 焦点X線検出器距離[焦点]		
                fcd = new float[3 * 3];                      // 焦点中心点[焦点][ｴﾘｱ]		
                fanangle = new float[3];                    // X線ﾌｧﾝ角[焦点]				
                mainch = new int[3];                        // ﾒｲﾝﾁｬﾝﾈﾙ[焦点]				
                refch = new int[3];                         // ﾚﾌｧﾚﾝｽﾁｬﾝﾈﾙ[焦点]			
                dtunitno = new int[3];                      // ﾒｲﾝﾁｬﾝﾈﾙﾕﾆｯﾄ[焦点]			
                dtunitch = new int[3];                      // ﾒｲﾝﾁｬﾝﾈﾙﾕﾆｯﾄ数[焦点]		
                dtpitch = new float[3];                     // 検出器ﾋﾟｯﾁ[焦点]			
                trvsno = new int[3];                        // ﾄﾗﾊﾞｰｽ数[焦点]				
                trpitch = new float[3 * 3];                  // ﾄﾗﾊﾞｰｽﾃﾞｰﾀﾋﾟｯﾁ[焦点][ｴﾘｱ]	
                trvspt = new int[3 * 3];                     // ﾄﾗﾊﾞｰｽﾃﾞｰﾀﾎﾟｲﾝﾄ[焦点][ｴﾘｱ]	
                dmych = new int[3];                         // ﾀﾞﾐｰﾁｬﾝﾈﾙ数[焦点]			
                offsetapr = new int[3];                     // ｵﾌｾｯﾄﾃﾞｰﾀ捨てﾎﾟｲﾝﾄ[焦点]	
                offsetpt = new int[3];                      // ｵﾌｾｯﾄﾃﾞｰﾀ収集ﾎﾟｲﾝﾄ[焦点]	
                airdtpt = new int[3];                       // 空気補正ﾃﾞｰﾀ収集ﾎﾟｲﾝﾄ[焦点] 
                aprtpt = new int[3 * 3];                     // ﾃﾞｰﾀ収集捨てﾎﾟｲﾝﾄ[焦点][ｴﾘｱ]
                sclsiz = new int[3 * 3];                     // 再構成画素数[焦点][ｴﾘｱ]		
                scanbias = new int[3 * 3 * 3 * 3 * 3 * 3];       // ｽｷｬﾝﾊﾞｲｱｽ値 		
                scanslop = new float[3 * 3 * 3 * 3 * 3 * 3];     // ｽｷｬﾝｽﾛｰﾌﾟ値			
                scanotvs = new int[3];                      // ｽｷｬﾉ昇降ﾃﾞｰﾀ収集回数[焦点]	
                scanoapr = new int[3];                      // ｽｷｬﾉ昇降ﾃﾞｰﾀ収集捨て数[焦点]
                scanopt = new int[3];                       // ｽｷｬﾉ昇降ﾃﾞｰﾀ収集数[焦点]	
                scanoscl = new int[3];                      // ｽｷｬﾉ画素数[焦点]			
                scanoair = new int[3];                      // ｽｷｬﾉ空気補正ﾃﾞｰﾀﾎﾟｲﾝﾄ[焦点]	
                scanobias = new int[3 * 3 * 3 * 3 * 3 * 3];      // ｽｷｬﾉﾊﾞｲｱｽ値			
                scanoslop = new float[3 * 3 * 3 * 3 * 3 * 3];    // ｽｷｬﾉｽﾛｰﾌﾟ値			
                scano_tblbias = new float[3 * 3];            // ｽｷｬﾉﾃｰﾌﾞﾙﾊﾞｲｱｽ値[焦点]		
                scano_udpitch = new float[3 * 3];            // ｽｷｬﾉ昇降ﾋﾟｯﾁ[焦点]			
                scaninit = new int[3 * 3 * 3 * 3 * 4];          // ｽｷｬﾝN1･N2･N3係数			//マイクロＣＴでは未使用
                scanoinit = new int[3 * 3 * 3 * 4 * 4];         // ｽｷｬﾉN1･N2･N3係数			//マイクロＣＴでは未使用
                scan_stroke = new int[3];                   // ｽｷｬﾝ可能昇降ｽﾄﾛｰｸ			
                scano_dp_ratio = new float[3];              // ｽｷｬﾉ表示時の縦横比			
                a = new float[5 * 6];                        // 幾何学補正係数[A0/A1/A2/A3/A4/A5] REV2.00	-> REV2.10 配列変更	
                xls = new int[5];                           // 有効ﾃﾞｰﾀ開始画素	REV2.00					-> REV2.10 配列変更	
                xle = new int[5];                           // 有効ﾃﾞｰﾀ画素		REV2.00					-> REV2.10 配列変更	
                xlc = new float[5];                         // ｾﾝﾀｰﾁｬﾝﾈﾙ画素		REV2.00					-> REV2.10 配列変更 
                scan_posi_a = new float[5];                 // ｽｷｬﾝ位置を示す一次直線の傾き REV2.00		 -> REV2.10 配列変更 
                scan_posi_b = new float[5];                 // ｽｷｬﾝ位置を示す一次直線の切片 REV2.00		 -> REV2.10 配列変更 

                //mfanangle = new float[5 * 3];                // MCT用ﾌｧﾝ角		REV2.00					    -> REV2.10 配列変更	
                mfanangle = new float[5 * 4];                // MCT用ﾌｧﾝ角    //v18.00変更 2->3 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_変更2014/07/16hata_v19.51反映
                //mcenter_angle = new float[5 * 3];            // 回転中心角度 REV2.00 -> REV2.10 配列変更		
                mcenter_angle = new float[5 * 4];            // 回転中心角度  //v18.00変更 2->3 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_変更2014/07/16hata_v19.51反映
                //mcenter_channel = new float[5 * 3];          // 再構成用ｾﾝﾀｰﾁｬﾝﾈﾙ REV2.00				-> REV2.10 配列変更	
                mcenter_channel = new float[5 * 4];          // 再構成用ｾﾝﾀｰﾁｬﾝﾈﾙ   //v18.00変更 2->3 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_変更2014/07/16hata_v19.51反映
                //mchannel_pitch = new float[5 * 3];           // 1ﾁｬﾝﾈﾙ当たりの角度	  REV2.00				-> REV2.10 配列変更	
                mchannel_pitch = new float[5 * 4];           // 1ﾁｬﾝﾈﾙ当たりの角度    //v18.00変更 2->3 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_変更2014/07/16hata_v19.51反映
                
                mdtpitch = new float[5];                    // 検出器ピッチ（ｍｍ）						 -> REV2.10 配列変更 
                fid_offset = new float[2];                  // ＦＩＤへの加算値(mm)REV M3					 -> REV2.10 配列変更 
                fcd_offset = new float[2];                  // ＦＣＤへの加算値(mm)REV M3					 -> REV2.10 配列変更 
                frame_rate = new float[2 * 3];               // 外部トリガのフレームレート						REV7.04 変更     
                v_mag = new float[3];                       // カメラ縦ビニング倍率 		REV7.04 2003-09-01 変更 by 間々田 
                b = new float[6];                           // ｺｰﾝﾋﾞｰﾑ用幾何歪補正係数		Rev3.00	

                //n1 = new int[2];                            // 有効ﾃﾞｰﾀ開始ﾁｬﾝﾈﾙ			Rev3.00	
                n1 = new int[3];                            // 有効ﾃﾞｰﾀ開始ﾁｬﾝﾈﾙ			Rev3.00 //v18.00変更 1->2 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_変更2014/07/16hata_v19.51反映
                //n2 = new int[2];                            // 有効ﾃﾞｰﾀ終了ﾁｬﾝﾈﾙ			Rev3.00	
                n2 = new int[3];                            // 有効ﾃﾞｰﾀ終了ﾁｬﾝﾈﾙ			Rev3.00 //v18.00変更 1->2 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_変更2014/07/16hata_v19.51反映
                //theta0 = new float[2];                      // 有効ﾌｧﾝ角(radian)			Rev3.00	
                theta0 = new float[3];                      // 有効ﾌｧﾝ角(radian)			Rev3.00 //v18.00変更 1->2 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_変更2014/07/16hata_v19.51反映
                //theta01 = new float[2];                     // 有効ﾃﾞｰﾀ包含ﾌｧﾝ角1(radian)	Rev3.00	
                theta01 = new float[3];                     // 有効ﾃﾞｰﾀ包含ﾌｧﾝ角1(radian)	Rev3.00 //v18.00変更 1->2 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_変更2014/07/16hata_v19.51反映	
                //theta02 = new float[2];                     // 有効ﾃﾞｰﾀ包含ﾌｧﾝ角2(radian)	Rev3.00	
                theta02 = new float[3];                     // 有効ﾃﾞｰﾀ包含ﾌｧﾝ角2(radian)	Rev3.00 //v18.00変更 1->2 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_変更2014/07/16hata_v19.51反映	

                dcf = new FixedString256[2 * 3];             // カメラ情報ファイル名 		REV7.04 2003-03-06 変更 by 間々田 
                for (int i = 0; i < 2; i++)
                    for (int j = 0; j < 3; j++)
                        dcf[i * j].Initialize();

                h_mag = new float[3];                       // カメラ横ビニング倍率 		REV7.04 2003-09-01 変更 by 間々田 
                pulsar_v_mag = new float[3];                // frmPulsarの縦倍率 			REV7.04 2003-09-01 変更 by 間々田 
                pulsar_h_mag = new float[3];                // frmPulsarの横倍率 			REV7.04 2003-09-01 変更 by 間々田 
                fpulsar_v_mag = new float[3];               // frmFPulsarの縦倍率 			REV7.04 2003-09-01 変更 by 間々田 
                fpulsar_h_mag = new float[3];               // frmFPulsarの横倍率 			REV7.04 2003-09-01 変更 by 間々田     
                distribute_drive.Initialize();              // 分散処理時の画像保存先ドライブ			REV8.00 
                max_mfanangle = new float[5];               // 最大ファン角								REV9.00 追加 2004-04-19 by 間々田  
                detector_pitch = new float[3 * 3];          // ii視野別の検出器ピッチ						REV9.6 追加 2004-10-20 by 間々田 //Rev23.10 配列を[3]→[3*3]に変更 by長野 2015/11/04
                magnify_para = new float[3];                // Rev23.10 追加 by長野 2015/11/04
                alk = new double[36];                       // ２次元幾何歪補正パラメータ（Ｘ方向）			v11.20追加 by 間々田 2005-10-06
                blk = new double[36];                       // ２次元幾何歪補正パラメータ（Ｙ方向）			v11.20追加 by 間々田 2005-10-06
                rcy = new float[3 * 2];                      // 回転中心Ｙ座標[I.I.視野][測定位置]						v15.0追加 by 間々田 2009/05/11
                rcfcd = new float[3 * 2];                    // 回転中心ＦＣＤ[I.I.視野][測定位置]      					v15.0追加 by 間々田 2009/05/11
                rc_full_area = new float[3 * 2];             // 回転中心FullScan最大スキャンエリア[I.I.視野][測定位置]   v15.0追加 by 間々田 2009/05/11
                rcfdd = new float[3];                       // 回転中心テーブル測定時のＦＤＤ[I.I.視野]					v15.0追加 by 間々田 2009/05/14
                autotbl_para = new float[3 * 3];             // 断面上でROI指定することにより、テーブル自動移動するためのパラメータ[I,I視野サイズ][a,b,c] [0]:a [1]:b [2]:c v15.0追加 by 長野　2009-07-02  配列の並びを変更 09-07-24 by IWASAWA
                autotbl_v_xray_pos = new float[3];          // 断面上でROI指定することにより、テーブル自動移動するための基準Y(光軸と垂直)座標		v15.0追加 by 長野　2009-07-02
                autotbl_kof = new float[3];                 // 断面上でROI指定することによりテーブル自動移動するためのオフセット率					v15.0追加 by 長野　2009-07-02

                ExObsCamPixSize = new float[3];            //外観カメラ画像上の1画素サイズ([0]:非ズーム時,[1]:ズーム時,[2]未使用)//Rev23.30  追加 by長野 2016/02/5
                ExObsCamPixSizeOnFTable = new float[3];    //外観カメラ画像上の1画素サイズ(微調テーブル時)([0]:非ズーム時,[1]:ズーム時,[2]未使用)//Rev23.30  追加 by長野 2016/02/5
                ExObsCamMaxArea = new int[3];              //外観カメラ画像上で描画可能な円形ROIの最大直径(mm)([0]:非ズーム時,[1]:ズーム時,[2]未使用)//Rev23.30  追加 by長野 2016/02/5
                //dummy = new float[2];                       // ダミー 
                dummy2 = new float[2];                       // ダミー 

            }
            #endregion

        }   // SCANCONDPAR
        #endregion SCANCONDPAR

        #region WORKSHOPINF
        /// <summary>
        /// 事業所名入力機能情報
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct WORKSHOPINF
        {
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public FixedString16 workshop;/* 事業所名					*/

            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // 配列の初期化
                workshop.Initialize();
            }
            #endregion       
        
        }   // WORKSHOPINF
        #endregion WORKSHOPINF

        #region MECHAPARA
        /// <summary>
        /// メカ関連パラメータ
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MECHAPARA
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	        public float[]	rot_speed		;//回転速度[Slow/Middle/Fast/Manual]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	        public float[]	ud_speed			;//テーブル昇降速度[Slow/Middle/Fast/Manual]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	        public float[]	fcd_speed		;//FCD移動速度[Slow/Middle/Fast/Manual]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	        public float[]	fdd_speed		;//FDD移動速度[Slow/Middle/Fast/Manual]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	        public float[]	tbl_y_speed		;//テーブルY軸速度[Slow/Middle/Fast/Manual]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	        public float[]	fine_tbl_speed	;//微調テーブル移動速度[Slow/Middle/Fast/Manual]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	        public float[]	collimator_speed	;//スライスコリメータ移動速度[Slow/Middle/Fast/Manual]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	        public float[]	xray_rot_speed	;//Ｘ線管回転速度[Slow/Middle/Fast/Manual]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	        public float[]	xray_x_speed		;//Ｘ線管Ｘ軸移動速度[Slow/Middle/Fast/Manual]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	        public float[]	xray_y_speed		;//Ｘ線管Ｙ軸移動速度[Slow/Middle/Fast/Manual]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] tilt_and_rot_rot_speed;//回転速度[Slow/Middle/Fast/Manual] //Rev22.00 回転傾斜テーブル by長野 2015/08/21
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] tilt_and_rot_tilt_speed;//チルト速度[Slow/Middle/Fast/Manual] //Rev22.00 回転傾斜テーブル by長野 2015/08/21

            public float max_fcd                  ;//最大FCD(オフセット無,仕様書上の値) Rev23.30 追加 by長野 2016/02/23
            public float max_fdd                  ;//最大FDD(オフセット無,仕様書上の値) Rec23.30 追加 by長野 2016/02/23
            public float min_fcd                  ;//最小FCD(オフセット無,仕様書上の値) Rev24.00 追加 by長野 2016/05/09
            public float min_fdd                  ;//最小FDD(オフセット無,仕様書上の値) Rec24.00 追加 by長野 2016/05/09

            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // 配列の初期化
                rot_speed = new float[4];           // 回転速度[Slow/Middle/Fast/Manual]
                ud_speed = new float[4];            // テーブル昇降速度[Slow/Middle/Fast/Manual]
                fcd_speed = new float[4];           // FCD移動速度[Slow/Middle/Fast/Manual]
                fdd_speed = new float[4];           // FDD移動速度[Slow/Middle/Fast/Manual]
                tbl_y_speed = new float[4];         // テーブルY軸速度[Slow/Middle/Fast/Manual]
                fine_tbl_speed = new float[4];      // 微調テーブル移動速度[Slow/Middle/Fast/Manual]
                collimator_speed = new float[4];    // スライスコリメータ移動速度[Slow/Middle/Fast/Manual]
                xray_rot_speed = new float[4];      // Ｘ線管回転速度[Slow/Middle/Fast/Manual]
                xray_x_speed = new float[4];        // Ｘ線管Ｘ軸移動速度[Slow/Middle/Fast/Manual]
                xray_y_speed = new float[4];        // Ｘ線管Ｙ軸移動速度[Slow/Middle/Fast/Manual]
            }
            #endregion
        
        
        }   //MECHAPARA
        #endregion MECHAPARA

        #region X_TABLE
        /// <summary>
        /// Ｘ線条件
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct X_TABLE
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (3 * 3 * 2))]
	        public float[]	xtable		;//Ｘ線条件[X線I.I.視野][X線条件][管電圧／管電流]
            public float base_fdd           ;// Ｘ線条件基準FDD


            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // 配列の初期化
                xtable = new float[3 * 3 * 2];
            }
            #endregion
        
        }   //X_TABLE
        #endregion X_TABLEA

        #region HSC_PARA
        /// <summary>
        /// 高速度透視撮影関係パラメータ
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct HSC_PARA
        {
	        public int	x_warn_time			;//X線警告時間(秒)
	        public int	x_warn_res_limit	;//X線警告返答制限時間(秒)
        }   //HSC_PARA
        #endregion HSC_PARA

        #region SCANSEL
        /// <summary>
        /// ﾃﾞｰﾀ収集条件（最新情報）
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SCANSEL
        {
	        public int	data_mode			;/* ﾃﾞｰﾀ収集					*/
	        public int	scan_focus			;/* ｽｷｬﾝ管球種  REV1.00			*/
	        public int	scan_energy			;/* ｽｷｬﾝ管電圧					*/
	        public int	scan_width			;/* ｽｷｬﾝｽﾗｲｽ幅					*/
	        public int	scan_mode			;/* ｽｷｬﾝﾓｰﾄﾞ					*/
	        public int	multiscan_mode		;/* ﾏﾙﾁｽｷｬﾝ種別ﾓｰﾄﾞ				*/
	        public int	scan_speed			;/* ｽｷｬﾝ速度ﾓｰﾄﾞ				*/
	        public int	scan_area			;/* ｽｷｬﾝｴﾘｱ						*/
	        public int	filter				;/* ﾌｨﾙﾀｰﾌｧﾝｸｼｮﾝ情報			*/
	        public int	bhc_flag			;/* BHC補正有無					*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	        //public byte[]	bhc_dir		;/* BHC補正ﾃｰﾌﾞﾙのﾃﾞｨﾚｸﾄﾘ		*/
            public FixedString256 bhc_dir   ;/* BHC補正ﾃｰﾌﾞﾙのﾃﾞｨﾚｸﾄﾘ				*/
           
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	        //public byte[]	bhc_name		;
            public FixedString256 bhc_name  ;/* BHC補正ﾃｰﾌﾞﾙ名				*/
	        
            public float	pitch				;/* AUTO MULTIのｽﾗｲｽﾋﾟｯﾁ		*/
	        public int	multinum			;/* AUTO MULTIのﾏﾙﾁｽﾗｲｽ枚数		*/
	        public int	matrix_size			;/* 画像ｻｲｽﾞ					*/
	        public int	det_ap				;/* 検出器開口					*/
	        public float	tilt_angle			;/* ﾃﾞｰﾀ収集傾斜角度			*/
	        public int	scano_focus			;/* ｽｷｬﾉ管球種  REV1.00			*/
	        public int	scano_energy		;/* ｽｷｬﾉ管電圧					*/
	        public int	scano_width			;/* ｽｷｬﾉｽﾗｲｽ幅					*/
	        public int	scano_speed			;/* ｽｷｬﾉ速度					*/
	        public int	scano_area			;/* ｽｷｬﾉｴﾘｱ						*/
	        public int	scano_matrix_size	;/* ｽｷｬﾉ画像ｻｲｽﾞ				*/
	        public int	scano_det_ap		;/* ｽｷｬﾉ検出器開口				*/
	        public float	scano_tilt			;/* ｽｷｬﾉ収集時の管球方向角度	*/
	        public int	auto_print			;/* 自動ﾌﾟﾘﾝﾄ					*/
	        public int	rawdata_save		;/* 生ﾃﾞｰﾀｾｰﾌﾞﾓｰﾄﾞ				*/
	        public int	pro_mode			;/* 新試料ｺｰﾄﾞ登録ﾓｰﾄﾞ			*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	        //public byte[]	pro_code		;/* ﾃﾞｨﾚｸﾄﾘ名（試料ｺｰﾄﾞ）		*/
            public FixedString256 pro_code  ;/* ﾃﾞｨﾚｸﾄﾘ名（試料ｺｰﾄﾞ）		*/

            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	        //public byte[]	pro_name		;/* ｽﾗｲｽ名（試料名）			*/
            public FixedString256 pro_name  ;/* ｽﾗｲｽ名（試料名）			*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	        //public byte[]	sliceplan_dir	;/* ｽﾗｲｽﾌﾟﾗﾝﾌｧｲﾙのﾃﾞｨﾚｸﾄﾘ名		*/
            public FixedString256 sliceplan_dir;/* ｽﾗｲｽﾌﾟﾗﾝﾌｧｲﾙのﾃﾞｨﾚｸﾄﾘ名	*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	        //public byte[]	slice_plan		;/* ｽﾗｲｽﾌﾟﾗﾝﾌｧｲﾙ名				*/
            public FixedString256 slice_plan;/* ｽﾗｲｽﾌﾟﾗﾝﾌｧｲﾙ名			*/
	        
            public int	auto_zoomflag		;/* ｵｰﾄｽﾞｰﾑ処理の有無			*/

            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	        //public byte[]	autozoom_dir	;/* ｵｰﾄｽﾞｰﾑﾌｧｲﾙのﾃﾞｨﾚｸﾄﾘ		*/
            public FixedString256 autozoom_dir;/* ｵｰﾄｽﾞｰﾑﾌｧｲﾙのﾃﾞｨﾚｸﾄﾘ		*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	        //public byte[]	auto_zoom		;/* ｵｰﾄｽﾞｰﾑﾌｧｲﾙ名				*/
            public FixedString256 auto_zoom;/* ｵｰﾄｽﾞｰﾑﾌｧｲﾙ名				*/
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	        //public byte[]	comment		;/* ｺﾒﾝﾄ						*/
            public FixedString256 comment   ;/* ｺﾒﾝﾄ						*/
	        
            public int	area_bk				;/* 前回のｽｷｬﾝｴﾘｱ情報			*/	//マイクロＣＴでは未使用
	        public int	speed_bk			;/* 前回のｽｷｬﾝ速度情報			*/	//マイクロＣＴでは未使用
	        public int	c_emergency			;/* 非常停止要求				*/	//マイクロＣＴでは未使用
	        public int	c_scan				;/* SCAN中ｽﾃｰﾀｽ					*/
	        public int	cpu_ready			;/* SCAN START可能ｽﾃｰﾀｽ			*/	//マイクロＣＴでは未使用
	        public int	buzzer				;/* ﾌﾞｻﾞｰ要求					*/	//マイクロＣＴでは未使用
	        public int	print_req			;/* ﾌﾟﾘﾝﾄ要求(画像ﾊｰﾄﾞｺﾋﾟｰ)		*/	//マイクロＣＴでは未使用
	        public int	panel_lock			;/* ﾀｯﾁﾊﾟﾈﾙのﾛｯｸ				*/	//マイクロＣＴでは未使用
	        public int	c_cw_ccw			;/* CW回転、CCW回転ﾓｰﾄﾞ			*/
	        public int	x_ray_auto			;/* X線ﾘﾓｰﾄﾓｰﾄﾞ					*/	//マイクロＣＴでは未使用
	        public int	c_x_on				;/* X線ON要求ﾓｰﾄﾞ				*/	//マイクロＣＴでは未使用
	        public int	c_x_off				;/* X線OFF要求ﾓｰﾄﾞ				*/	//マイクロＣＴでは未使用
	        public int	c_shut_open			;/* ｼｬｯﾀ開要求ﾓｰﾄﾞ				*/	//マイクロＣＴでは未使用
	        public int	c_shut_close		;/* ｼｬｯﾀ閉要求ﾓｰﾄﾞ				*/	//マイクロＣＴでは未使用
	        public int	c_tra_reset			;/* ﾄﾗﾊﾞｰｽRESET位置要求ﾓｰﾄﾞ		*/	//マイクロＣＴでは未使用
	        public int	c_rot_reset			;/* 回転RESET位置要求ﾓｰﾄﾞ		*/	//マイクロＣＴでは未使用
	        public int	c_mecha_reset		;/* 昇降RESET位置要求ﾓｰﾄﾞ		*/	//マイクロＣＴでは未使用
	        public int	c_count_reset		;/* 昇降ｶｳﾝﾀRESET要求ﾓｰﾄﾞ		*/	//マイクロＣＴでは未使用
	        public int	err_reset			;/* ﾀｯﾁﾊﾟﾈﾙｴﾗｰﾘｾｯﾄ				*/	//マイクロＣＴでは未使用
	        public int	fapc_reset			;/* FAPCのRESET要求ﾓｰﾄﾞ			*/	//マイクロＣＴでは未使用
	        public int	para_req			;/* 撮影条件ﾘｸｴｽﾄﾌﾗｸﾞ			*/	//マイクロＣＴでは未使用
	        public int	ud_req				;/* 昇降相対位置要求ﾓｰﾄﾞ		*/	//マイクロＣＴでは未使用
	        public float	ud					;/* 昇降移動位置（相対座標）	*/	//マイクロＣＴでは未使用
	        public int	udab_req			;/* 昇降絶対位置要求ﾓｰﾄﾞ		*/	//マイクロＣＴでは未使用
	        public float	udab				;/* 昇降移動位置（絶対座標）	*/
	        public int	tr_req				;/* ﾄﾗﾊﾞｰｽ移動位置（絶対）要求	*/	//マイクロＣＴでは未使用
	        public float	tr					;/* ﾄﾗﾊﾞｰｽ移動位置（絶対）		*/	//マイクロＣＴでは未使用
	        public int	rot_req				;/* 回転移動位置要求			*/	//マイクロＣＴでは未使用
	        public float	rot					;/* 回転移動位置				*/	//マイクロＣＴでは未使用
	        public int	cnt_req				;/* 芯位置（ｾﾝﾀﾘﾝｸﾞ）要求ﾓｰﾄﾞ	*/	//マイクロＣＴでは未使用
	        public float	cnt					;/* 芯位置（ｾﾝﾀﾘﾝｸﾞ）			*/
	        public int	c_focus_reset		;/* 焦点RESET位置要求ﾓｰﾄﾞ		*/	//マイクロＣＴでは未使用
	        public float	scan_kv				;/* ｽｷｬﾝ管電圧 REV2.00			*/
	        public float	scan_ma				;/* ｽｷｬﾝ管電流 REV2.00			*/
	        public int	scan_view			;/* ｽｷｬﾝﾋﾞｭｰ数 REV2.00			*/
	        public float	mscan_area			;/* ｽｷｬﾝｴﾘｱ(mm) REV2.00			*/
	        public float	mscan_width			;/* ｽｷｬﾝｽﾗｲｽ厚(mm) REV2.00		*/
	        public float	scano_kv			;/* ｽｷｬﾉ管電圧 REV2.00			*/
	        public float	scano_ma			;/* ｽｷｬﾉ管電流 REV2.00			*/
	        public float	mscano_area			;/* ｽｷｬﾉｴﾘｱ(mm) REV2.00			*/
	        public float	mscano_width		;/* ｽｷｬﾉｽﾗｲｽ厚(mm) REV2.00		*/
	        public int	scan_integ_number	;/* ｽｷｬﾝ画像積分枚数 REV2.00	*/
	        public float	mscan_bias			;/* ｽｷｬﾝﾊﾞｲｱｽ REV2.00			*/
	        public float	mscan_slope			;/* ｽｷｬﾝｽﾛｰﾌﾟ REV2.00			*/
	        public int	scano_integ_number	;/* ｽｷｬﾉ画像積分枚数 REV2.00	*/
	        public float	mscano_bias			;/* ｽｷｬﾉﾊﾞｲｱｽ REV2.00			*/
	        public float	mscano_slope		;/* ｽｷｬﾉｽﾛｰﾌﾟ REV2.00			*/
	        public int	scan_and_view		;/* ｽｷｬﾝ中再構成 REV2.00		*/
	        public int	image_direction		;/* 画像方向 REV2.00			*/
	        public int	ii_correction		;/* II幾何歪補正有無 REV2.00	*/
	        public float	fid					;/* FID REV2.00					*/
	        public float	fcd					;/* FCD REV2.00					*/
	        public float	max_slice_wid		;/* ｽｷｬﾝ最大ｽﾗｲｽ厚(mm) REV2.00	*/

            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]  //'v18.00変更 byやまおか 2011/02/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_変更2014/07/16hata_v19.51反映
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] max_scan_area;/* 最大ｽｷｬﾝｴﾘｱ(mm) REV2.00		*/
	        
            public float	max_scano_slice_wid	;/* ｽｷｬﾉ最大ｽﾗｲｽ厚(mm) REV2.00	*/
	        public float	max_scano_area		;/* 最大ｽｷｬﾉｴﾘｱ REV2.00			*/
            public float   min_slice_wid       ;/* 最小スライス厚(mm) REV M3   */
            public int    fimage_bit          ;/* 透視画像ビット数   REV M3   */
	        public int    operation_mode      ;/* 1:SCAN 2:SCANO 3:RECON 4:ZOOM 5:GAIN 6:CONEBEAM 7:CONERECON 8:POSTCONE*/
	        public float   image_rotate_angle  ;/* 画像回転角度（度）                 Rev2.10    */	//v11.5 LONG→FLOAT by 間々田 2006/06/27
	        public int    recon_mask          ;/* 再構成形状   　　                  Rev2.10    */
	        public int    contrast_fitting    ;/* 画像階調再適度                     Rev2.10    */
	        public float   max_multislice_pitch;/* 複数ｽﾗｲｽ同時ｽｷｬﾝ最大ｽﾗｲｽﾋﾟｯﾁ（mm） Rev2.10    */
	        public int    multislice          ;/* 複数ｽﾗｲｽ同時ｽｷｬﾝ数				   Rev2.10    */
	        public float   multislice_pitch    ;/* 複数ｽﾗｲｽ同時ｽｷｬﾝﾋﾟｯﾁ（mm）         Rev2.10    */
	        public int    max_multislice      ;/* 複数ｽﾗｲｽ同時ｽｷｬﾝ最大枚数           Rev2.10    */
	        public int    fluoro_image_save   ;/* 透視画像保存                       Rev2.10    */
	        public int    table_rotation      ;/* テーブル回転モード					Rev2.10    */
	        public int    multi_tube          ;/* 複数ｘ線管						   Rev2.10    */
	        public int	mc					;/* 縦中心ﾁｬﾝﾈﾙ(ﾗｲﾝ数半幅)	Rev3.00	*/

            //public int mc_max;/* 縦中心ﾁｬﾝﾈﾙの最大値(ﾗｲﾝ数半幅)	Rev12.1	2007-2-12 by 山本 */　_変更2014/07/16hata_v19.51反映
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]//縦中心ﾁｬﾝﾈﾙの最大値(ﾗｲﾝ数半幅) 0:非シフト 1:シフト 'v18.00変更 byやまおか 2011/07/29 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public int[] mc_max;/* 縦中心ﾁｬﾝﾈﾙの最大値(ﾗｲﾝ数半幅)	Rev12.1	2007-2-12 by 山本 */
	        
            public float	zp					;/* ﾍﾘｶﾙﾋﾟｯﾁ(mm)		Rev3.00	*/
	        public int	iud					;/* 昇降識別値			Rev3.00	*/
	        public int	k					;/* ｺｰﾝﾋﾞｰﾑｽﾗｲｽ枚数		Rev3.00	*/
	        public float	delta_z				;/* 軸方向Boxelｻｲｽﾞ(mm)	Rev3.00	*/
	        public float	zs					;/* 1枚目ｽﾗｲｽのZ位置(mm)Rev3.00	*/
	        public float	ze					;/* K枚目ｽﾗｲｽのZ位置(mm)Rev3.00	*/
	        public float	delta_msw			;/* 画面上のｽﾗｲｽ幅(画素)Rev3.00	*/
	        public int	inh					;/* ﾍﾘｶﾙﾓｰﾄﾞ			Rev3.00	*/
	        public int	auto_centering		;/* ｵｰﾄｾﾝﾀﾘﾝｸﾞ			Rev3.00	*/
	        public float	cone_scan_area		;/* ｺｰﾝﾋﾞｰﾑｽｷｬﾝｴﾘｱ(mm)	Rev3.00	*/
	        public float	cone_scan_width		;/* ｺｰﾝﾋﾞｰﾑｽﾗｲｽ幅(mm)	Rev3.00	*/
	        public int	cone_image_mode		;/*	画質ﾓｰﾄﾞ(0:標準,1:精細,2:標準&高速,3:精細&高速)		*/	//REV3.00 Append by 鈴山 (2000/09/20)
	        public float	cone_max_scan_area	;/* ｺｰﾝﾋﾞｰﾑ最大ｽｷｬﾝｴﾘｱ(mm)		*/	//REV3.00 Append by 鈴山 (2000/09/26)
	        public int	fluoro_image_disp	;/* 透視画像表示	0:なし 1:あり	REV6.00 */
	        public int	binning				;/* ビニングモード　0:1x1 1:2x2 2:4x4	REV7.04 */
	        public int	cone_raw_size		;/* コーンビーム生データサイズ(KB)		REV7.04 */
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	        //public byte[]	cone_sliceplan_dir		;/* コーンビーム用スライスプランテーブルのディレクトリ名		REV8.00 */
            public FixedString256 cone_sliceplan_dir;/* コーンビーム用スライスプランテーブルのディレクトリ名		REV8.00 */
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	        //public byte[]	cone_slice_plan		;/* コーンビーム用スライスプランテーブルのファイル名			REV8.00 */
            public FixedString256 cone_slice_plan;/* コーンビーム用スライスプランテーブルのファイル名			REV8.00 */
	        
            public int	max_cone_view				;/* コーンビーム時の最大ビュー数								REV8.00 */
	        public int	cone_distribute				;/* コーンビームの分散処理（0:無効 1:有効）						REV8.00 */
	        public float	max_cone_slice_width			;/* コーンビームの最大スライス厚（mm）							REV8.00	*/
	        public float	min_cone_slice_width			;/* コーンビームの最小スライス厚（mm）							REV8.00	*/
 	        public int	rotate_select		;/*  回転選択 			0:テーブル 	1:Ｘ線管	REV9.00 追加 2004-01-29 by 間々田 */
 	        public int	round_trip			;/*  往復スキャン 		0:しない 	1:する		REV9.00 追加 2004-01-29 by 間々田 */
 	        public int	over_scan			;/*  オーバースキャン 	0:しない 	1:する		REV9.00 追加 2004-01-29 by 間々田 */
	        public int	disp_size			;/*  画像表示サイズ 	0:1024画素 	1:2048画素	REV9.00 追加 2004-01-29 by 間々田 */
	        public int	mail_send			;/*  メール送信 	0:送信しない 	1:送信する	REV9.1 追加 2004-05-13 by 間々田 */
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
	        //public byte[]	smtp_server		;/* SMTPサーバ名							REV9.1 追加 2004-05-13 by 間々田 */
            public FixedString128 smtp_server;/* SMTPサーバ名							REV9.1 追加 2004-05-13 by 間々田 */
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
	        //public byte[]	transmitting_person	;/* 送信者名							REV9.1 追加 2004-05-13 by 間々田 */
            public FixedString128 transmitting_person;/* 送信者名							REV9.1 追加 2004-05-13 by 間々田 */
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	        //public byte[]	address				;/* 宛先								REV9.1 追加 2004-05-13 by 間々田 */
            public FixedString256 address       ;/* 宛先								REV9.1 追加 2004-05-13 by 間々田 */
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	        //public byte[]	carbon_copy			;/* ＣＣ								REV9.1 追加 2004-05-13 by 間々田 */
            public FixedString256 carbon_copy   ;/* ＣＣ								REV9.1 追加 2004-05-13 by 間々田 */
	        
            public int	discharge_protect			;/* Ｘ線休止処理 	0:しない 	1:する	REV9.3 追加 2004-06-30 by 間々田 */
	        public int	artifact_reduction			;/* アーティファクト低減 0:なし 1:あり	REV9.7 追加 2004-12-08 by 間々田 */
	        public int	filter_process		;/*フィルタリング処理 0:FFT 1:Conv		Rev13.00 追加 2007-01-22 by やまおか */
	        public int	rfc					;/* RFC処理の可否						Rev14.00 追加 2007-05-24 by Ohkado */
	        public int	x_condition			;//X線条件	0:手動設定 1:L 2:M 3:H Rev15.00追加 2008-12-27 by 間々田
	        public int	multi_dbq_disp		;//マルチコーンで、全体のｽﾗｲｽﾋﾟｯﾁが一定になるとき：1　それ以外：0 Rev16.0 追加 10-01-21 by IWASAWA
	        public int	fpd_gain			;//パーキンエルマーFPDゲイン　REV16.2/17.0 山本
	        public int	fpd_integ			;//パーキンエルマーFPD積分時間　REV16.2/17.0 山本
	        public int  com_detector_no     ;//CT用2nd検出器切替可の時，CTシステムがどちらのコモンで動作しているかを示す。Rev17.03 追加 by 長野
	        public int	mbhc_flag			;// 0:BHCなし 1:BHCあり
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	        //public byte[]	mbhc_dir		;// BHCテーブルディレクトリ名?
            public FixedString256 mbhc_dir  ;// BHCテーブルディレクトリ名?
            
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
	        //public byte[]	mbhc_name		;// BHCテーブルファイル名?
            public FixedString256 mbhc_name ;// BHCテーブルファイル名?

            public float fpd_gain_f;    //FPDゲイン(表示用)   v18.00追加 byやまおか 2011/07/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_追加2014/07/16hata_v19.51反映
            public float fpd_integ_f;   //FPD積分時間(表示用) v18.00追加 byやまおか 2011/07/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05_追加2014/07/16hata_v19.51反映

            public float mscano_mdtpitch;       //Rev21.00 スキャノデータピッチ(mm)     追加 by長野 2015/2/19
            public int mscano_real_mscanopt;    //マイクロ用 スキャノポイント(最小スキャノ厚の場合) Rev21.00 追加 by長野
            public int mscanopt;                //Rev21.00 スキャノポイント             追加 by長野 2015/2/19
            public float mscano_udpitch;        //Rev21.00 スキャノ昇降ピッチ(mm)       追加 by長野 2015/2/19
            public int mscano_integ_number;     //Rev21.00 スキャノ積算枚数             追加 by長野 2015/2/19
            public float mscano_data_endpos;    //Rev21.00 スキャノ終了昇降位置                  追加 by長野 2015/2/23

            public float numOfAdjCenterCh;      //Rev23.00 回転中心ch調整量(ch) 追加 by長野 2015/09/07

            public int pur_rawdata_save;        //Rev23.12 純生データ保存 追加 by長野 2015/12/11

            public int scanCTNo;                            //スキャンしたCT装置の識別用番号(1,2,3･･･) Rev24.00 追加 by長野 2016/04/06
            public int inlineCTScanMode;                    //インラインCT時のスキャンモード(0:手動,1:自動,2;ティーチング) Rev24.00 by長野 2016/04/06
            public FixedString32 scanSampleCode;            //スキャンした対象物を識別するコード Rev24.00 追加 by長野 2016/04/06
            public int scanSamplePosition;                  //サンプルの配置位置(0,1,2,3･･･) Rev24.00 by長野 2016/04/06
            public FixedString16 scanSamplePositionName;    //サンプルの配置位置名(正極,負極等) Rev24.00 by長野 2016/04/06
            public FixedString32 scanStartTimeByPLC;        //PLCから送られてくるスキャン開始時刻(ﾃｰﾌﾞﾙにサンプルをおいた時刻) YYYY/MM/DD/HH/SS Rev24.00 by長野 2016/04/16
            public int auto_analysis_assy;                  //自動組付判定有(0:無,1:有) Rev24.00 追加 by長野 2016/04/06
            public FixedString256 assy_table_dir;//組付判定用テーブル ディレクトリ名 Rev24.00 by長野 2016/04/06
            public FixedString256 assy_table_name;//組付判定用テーブル テーブル名 Rev24.00 by長野 2016/04/06
            public int auto_analysis_inclusion;             //自動異物判定有(0:無,1:有) Rev24.00 追加 by長野 2016/04/06
            public FixedString256 inclusion_table_dir;//異物判定用テーブル ディレクトリ名 Rev24.00 by長野 2016/04/06
            public FixedString256 inclusion_table_name;//異物判定用テーブル テーブル名 Rev24.00 by長野 2016/04/06
            public float nc                         ;//回転中心ch Rev24.00 by長野 2016/04/25

            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // 配列の初期化
                bhc_dir.Initialize();               // BHC補正ﾃｰﾌﾞﾙのﾃﾞｨﾚｸﾄﾘ
                bhc_name.Initialize();              // BHC補正ﾃｰﾌﾞﾙ名
                pro_code.Initialize();              // ﾃﾞｨﾚｸﾄﾘ名（試料ｺｰﾄﾞ）
                pro_name.Initialize();              // ｽﾗｲｽ名（試料名）
                sliceplan_dir.Initialize();         // ｽﾗｲｽﾌﾟﾗﾝﾌｧｲﾙのﾃﾞｨﾚｸﾄﾘ名
                slice_plan.Initialize();            // ｽﾗｲｽﾌﾟﾗﾝﾌｧｲﾙ名
                autozoom_dir.Initialize();          // ｵｰﾄｽﾞｰﾑﾌｧｲﾙのﾃﾞｨﾚｸﾄﾘ
                auto_zoom.Initialize();             // ｵｰﾄｽﾞｰﾑﾌｧｲﾙ名
                comment.Initialize();               // ｺﾒﾝﾄ
                //max_scan_area = new float[3];       // 最大ｽｷｬﾝｴﾘｱ(mm) REV2.00  
                max_scan_area = new float[4];       // 最大ｽｷｬﾝｴﾘｱ(mm) REV2.00    //v19.50 v19.41とv18.02の統合_2014/07/16hata_v19.51反映

                cone_sliceplan_dir.Initialize();    //コーンビーム用スライスプランテーブルのディレクトリ名 REV8.00
                cone_slice_plan.Initialize();       //コーンビーム用スライスプランテーブルのファイル名 REV8.00
                smtp_server.Initialize();           //SMTPサーバ名						REV9.1 追加 2004-05-13 by 間々田
                transmitting_person.Initialize();   //送信者名							REV9.1 追加 2004-05-13 by 間々田
                address.Initialize();               //宛先								REV9.1 追加 2004-05-13 by 間々田
                carbon_copy.Initialize();           //ＣＣ								REV9.1 追加 2004-05-13 by 間々田        
                mbhc_dir.Initialize();
                mbhc_name.Initialize();

                mc_max = new int[2];                // 縦中心ﾁｬﾝﾈﾙの最大値(ﾗｲﾝ数半幅) v19.50 v19.41とv18.02の統合_追加2014/07/16hata_v19.51反映

                //Rev24.00 追加 by長野 2016/04/06 --->
                scanSampleCode.Initialize();
                scanSamplePositionName.Initialize();
                assy_table_dir.Initialize();
                assy_table_name.Initialize();
                inclusion_table_dir.Initialize();
                inclusion_table_name.Initialize();
                scanStartTimeByPLC.Initialize();
                //<---

            }
            #endregion        
        
        
        }   //SCANSEL
        #endregion SCANSEL

        #region MECAINF
        // 追加2013/04/11<dNet化>_hata
        /// <summary>
        /// メカ情報パラメータ
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MECAINF
        {
            public int emergency            ;   // EMERGNCY信号状態
            public int mecha_ready          ;   // ﾒｶ準備完了信号状態
            public int mecha_busy           ;   // ﾒｶ動作中信号状態
            public int cpu_ready            ;   // CPU処理準備完了信号状態
            public int scan                 ;   // ｽｷｬﾝ･ｽｷｬﾉ動作中信号状態
            public int ups_power            ;   // UPS電源ON/OFF状態
            public int x_ready              ;   // X線制御準備完了信号状態
            public int test_scan            ;   // ﾃｽﾄｽｷｬﾝ情報
            public int m_error              ;   // 機構系の状態。ﾒｶｴﾗｰ情報
            public int rot_mode             ;   // 回転ﾓｰﾄﾞ
            public int ud_ready             ;   // 昇降準備完了情報
            public int ud_busy              ;   // 昇降動作中情報
            public int ud_error             ;   // 昇降ｴﾗｰ情報
            public int ud_limit             ;   // 昇降ﾘﾐｯﾀ限情報
            public float ud_pos             ;   // 昇降位置。相対座標
            public float udab_pos           ;   // 昇降位置。絶対座標
            public float ud_linear_pos      ;   // 昇降位置(リニアスケール) Rev23.10 by長野 2015/09/18
            public int tr_reset             ;   // ﾄﾗﾊﾞｰｽ準備完了情報
            public int tr_busy              ;   // ﾄﾗﾊﾞｰｽ動作情報
            public int tr_error             ;   // ﾄﾗﾊﾞｰｽｴﾗｰ情報
            public int area_size            ;   // ｴﾘｱｻｲｽﾞ情報
            public int tr_resets            ;   // ﾄﾗﾊﾞｰｽ軸のSﾘｾｯﾄ位置
            public int tr_resetm            ;   // ﾄﾗﾊﾞｰｽ軸のMﾘｾｯﾄ位置
            public int tr_resetl            ;   // ﾄﾗﾊﾞｰｽ軸のLﾘｾｯﾄ位置
            public int tr_limit             ;   // ﾄﾗﾊﾞｰｽﾘﾐｯﾀ限情報
            public int tr_pos               ;   // ﾄﾗﾊﾞｰｽ位置情報
            public int rot_ready            ;   // 回転準備完了情報
            public int rot_busy             ;   // 回転動作中情報
            public int rot_error            ;   // 回転ｴﾗｰ情報
            public int rot_reset            ;   // 回転軸の0ﾘｾｯﾄ位置
            public int rot_180              ;   // 回転軸の180ﾘｾｯﾄ位置
            public int rot_pos              ;   // 回転位置
            public int cnt_ready            ; //   芯位置準備完了情報
            public int cnt_busy             ;   // 芯位置動作中情報
            public int cnt_abnormal         ;   // 芯位置異常
            public int cnt_error            ;   // 芯位置ｻｰﾎﾞ異常
            public int cnt_limit            ;   // 芯位置ﾘﾐｯﾀ限異常
            public int cnt_pos              ;   // 芯位置ﾎﾟｼﾞｼｮﾝ
            public int filter_ready         ;   // ｼｬｯﾀ･ﾌｨﾙﾀ準備完了情報
            public int filter_busy          ;   // ｼｬｯﾀ･ﾌｨﾙﾀ起動情報
            public int filter_error         ;   // ｼｬｯﾀ･ﾌｨﾙﾀｴﾗｰ情報
            public int filter_energy        ;   // ﾌｨﾙﾀ-ｴﾈﾙｷﾞ-
            public int filter_limit         ;   // ｼｬｯﾀ･ﾌｨﾙﾀﾘﾐｯﾀ限情報
            public int shutter              ;   // X線ｼｬｯﾀ-情報
            public int colimeter_ready      ;   // ｺﾘﾒｰﾀ準備完了情報
            public int colimeter_busy       ;   // ｺﾘﾒｰﾀ起動情報
            public int colimeter_error      ;   // ｺﾘﾒｰﾀｴﾗｰ情報
            public int colimeter_size       ;   // ｺﾘﾒｰﾀｻｲｽﾞ情報
            public int colimeter_limit      ;   // ｺﾘﾒｰﾀﾘﾐｯﾀ制御情報
            public int tube_ready           ;   // 管球種準備完了情報
            public int tube_busy            ;   // 管球種起動情報
            public int tube_error           ;   // 管球種ｴﾗｰ情報
            public int tube_energy          ;   // 管球種
            public int tube_limit           ;   // 管球種ﾘﾐｯﾀ制御情報
            public int shift_ready          ;   // ｼﾌﾄ準備完了
            public int shift_busy           ;   // ｼﾌﾄ動作中
            public int shift_error          ;   // ｼﾌﾄｴﾗｰ
            public int shift_size           ;   // ｼﾌﾄｻｲｽﾞ
            public int shift_limit          ;   // ｼﾌﾄﾘﾐｯﾀ限
            public int xray_ready           ;   // X線準備完了
            public int xray_on              ;   // X線 ON/OFF状態
            public int xray_avl             ;   // X線ｱﾍﾞｲﾗﾌﾞﾙ状態
            public int xray_agng            ;   // X線ｳｫｰﾑｱｯﾌﾟ状態
            public int warmup               ;   // ｳｫｰﾑｱｯﾌﾟ状態
            public int warmup_days          ;   // ｳｫｰﾑｱｯﾌﾟｽｹｼﾞｭｰﾙ
            public int xray_ovset           ;   // X線過設定
            public int xray_slferr          ;   // X線制御機ｴﾗｰ
            public int xray_tmp             ;   // X線発生装置温度異常
            public int xray_error           ;   // X線系の状態。X線ｴﾗｰ情報
            public int interlock_ready      ;   // ｲﾝﾀｰﾛｯｸ準備完了
            public int interlock_open1      ;   // 保守扉+試料扉
            public int interlock_open2      ;   // X線ｼｰﾙﾄﾞ
            public int interlock_das        ;   // DAS異常
            public int interlock_dwc        ;   // 冷却水供給装置異常
            public int power_err            ;   // DAS電源ｴﾗｰ
            public int therm_err            ;   // DAS温度異常
            public int ipr                  ;   // ﾌﾟﾘﾝﾀ動作中
            public int duc                  ;   // ﾌﾟﾘﾝﾀ接続断
            public int pdo                  ;   // ﾌﾟﾘﾝﾀ用紙切れ
            public int fapc_ready           ;   // FAPC準備完了情報
            public int fapc_error           ;   // その他のFAPC異常
            public int slow_position        ;   // ｽｷｬﾝ･ｽｷｬﾉ中に減速域に入った
            public int data_error           ;   // ﾃﾞｰﾀ収集異常
            public int x_ray_auto           ;   // X線ﾘﾓｰﾄ
            public int ram_data_error       ;   // RAMﾃﾞｰﾀ異常
            public int line_mode            ;   // 回線ﾓｰﾄﾞ（切断か接続か）
            public int op_panel             ;   // ｵﾍﾟﾊﾟﾈの状態
            public int focus_x_ready        ;   // Ｘ線焦点切替Ｘ方向準備完了
            public int focus_x_busy         ;   // Ｘ線焦点切替Ｘ方向起動情報
            public int focus_x_error        ;   // Ｘ線焦点切替Ｘ方向ｴﾗｰ情報
            public int focus_x_energy       ;   // Ｘ線焦点切替Ｘ方向焦点位置
            public int focus_x_limit        ;   // Ｘ線焦点切替Ｘ方向ﾘﾐｯﾀ限情報
            public int focus_y_ready        ;   // Ｘ線焦点切替Ｙ方向準備完了
            public int focus_y_busy         ;   // Ｘ線焦点切替Ｙ方向起動情報
            public int focus_y_error        ;   // Ｘ線焦点切替Ｙ方向ｴﾗｰ情報
            public int focus_y_energy       ;   // Ｘ線焦点切替Ｙ方向焦点位置
            public int focus_y_limit        ;   // Ｘ線焦点切替Ｙ方向ﾘﾐｯﾀ限情報
            public int focus_z_ready        ;   // Ｘ線焦点切替Ｚ方向準備完了
            public int focus_z_busy         ;   // Ｘ線焦点切替Ｚ方向起動情報
            public int focus_z_error        ;   // Ｘ線焦点切替Ｚ方向ｴﾗｰ情報
            public int focus_z_energy       ;   // Ｘ線焦点切替Ｚ方向焦点位置
            public int focus_z_limit        ;   // Ｘ線焦点切替Ｚ方向ﾘﾐｯﾀ限
            public int filter               ;   // ｼｬｯﾀﾌｨﾙﾀ選択情報
            public int rot_speed_const      ;   // 回転速度情報 REV2.00
            public int phm_ready            ;   // 較正用ファントム機構準備完了情報
            public int phm_busy             ;   // 較正用ファントム機構動作中情報
            public int phm_error            ;   // 較正用ファントム機構エラー情報
            public int phm_limit            ;   // 較正用ファントム機構リミット限情報
            public int phm_onoff            ;   // 較正用ファントム機構位置情報
            public int vertical_cor         ;   // 幾何歪較正ステータス
            public int normal_rc_cor        ;   // ﾉｰﾏﾙｽｷｬﾝ用回転中心較正ｽﾃｰﾀｽ
            public int cone_rc_cor          ;   // ｺｰﾝﾋﾞｰﾑｽｷｬﾝ用回転中心較正ｽﾃｰﾀｽ
            public int offset_cor           ;   // ｵﾌｾｯﾄ較正ｽﾃｰﾀｽ
            public int gain_cor             ;   // ｹﾞｲﾝ較正ｽﾃｰﾀｽ
            public int distance0_cor        ;   // X線管1用寸法較正ｽﾃｰﾀｽ
            public int distance1_cor        ;   // X線管2用寸法較正ｽﾃｰﾀｽ
            public int scanpos_cor          ;   // ｽｷｬﾝ位置登録ｽﾃｰﾀｽ
            public int distance_cor_inh     ;   // 寸法較正ｽﾃｰﾀｽｲﾝﾋﾋﾞｯﾄ
            public int scanpos_cor_inh      ;   // ｽｷｬﾝ位置登録ｲﾝﾋﾋﾞｯﾄ
            public int ver_iifield          ;   // 幾何歪較正I.I.視野
            public int ver_mt               ;   // 幾何歪較正X線管番号
            public float rc_kv              ;   // 回転中心較正管電圧 (kV)
            public float rc_udab_pos        ;   // 回転中心較正昇降位置 (mm)
            public int rc_iifield           ;   // 回転中心較正I.I.視野
            public int rc_mt                ;   // 回転中心較正X線管番号
            public int gain_iifield         ;   // ｹﾞｲﾝ較正I.I.視野
            public float gain_kv            ;   // ｹﾞｲﾝ較正管電圧 (kV)
            public float gain_ma            ;   // ｹﾞｲﾝ較正管電流 (mA)
            public int gain_mt              ;   // ｹﾞｲﾝ較正X線管番号
            public int gain_filter          ;   // ｹﾞｲﾝ較正ﾌｨﾙﾀ
            public int off_date             ;   // ｵﾌｾｯﾄ較正年月日
            public int dc_iifield           ;   // 寸法較正I.I.視野
            public int dc_mt                ;   // 寸法較正X線管番号
            public int sp_iifield           ;   // ｽｷｬﾝ位置登録I.I.視野
            public int sp_mt                ;   // ｽｷｬﾝ位置登録X線管番号
            public int xstg_ready           ;   // 微調X軸準備完了
            public int xstg_busy            ;   // 微調X軸動作中
            public int xstg_error           ;   // 微調X軸エラー
            public int xstg_limit           ;   // 微調X軸リミット
            public float xstg_pos           ;   // 微調X軸現在位置[mm]
            public int ystg_ready           ;   // 微調Y軸準備完了
            public int ystg_busy            ;   // 微調Y軸動作中
            public int ystg_error           ;   // 微調Y軸エラー
            public int ystg_limit           ;   // 微調Y軸リミット
            public float ystg_pos           ;   // 微調Y軸現在位置[mm]
            public int table_auto_move      ;   // 自動ﾃｰﾌﾞﾙ移動ｽﾃｰﾀｽ  0:ﾊｰﾌ/ﾌﾙ移動完了  1:ｵﾌｾｯﾄ移動完了  2:移動未完了  3:移動不可  4:移動中
            public float auto_move_xf       ;   // ﾊｰﾌ/ﾌﾙ用X座標
            public float auto_move_yf       ;   // ﾊｰﾌ/ﾌﾙ用Y座標
            public float auto_move_xo       ;   // ｵﾌｾｯﾄ用X座標
            public float auto_move_yo       ;   // ｵﾌｾｯﾄ用Y座標
            public int iifield              ;   // I.I 視野
            public int rc_bin               ;   // 回転中心校正実行時のビニングモード
            public int ver_bin              ;   // 幾何歪校正実行時のビニングモード
            public int sp_bin               ;   // スキャン位置校正実行時のビニングモード
            public int gain_bin             ;   // ゲイン校正実行時のビニングモード
            public int off_bin              ;   // オフセット校正実行時のビニングモード
            public int dc_bin               ;   // 寸法校正実行時のビニングモード
            public int dc_rs                ;   // 寸法校正回転選択ステータス     0:テーブル 1:Ｘ線管
            public int rc_rs                ;   // 回転中心校正回転選択ステータス 0:テーブル 1:Ｘ線管
            public int gain_date            ;   // ゲイン較正年月日   'v12.01追加 by 間々田 2006/12/04
            public float table_x_pos        ;   // 試料テーブル（光軸と垂直方向)座標[mm]　v15.0追加　by　長野　2009/07/16
            public int gain_fpd_gain        ;   // ゲイン校正時のFPDゲイン　      'v16.20/v17.00追加 byやまおか 2010/02/17
            public int gain_fpd_integ       ;   // ゲイン校正時のFPD積分時間　    'v16.20/v17.00追加 byやまおか 2010/02/17
            public int off_fpd_gain         ;   // オフセット校正時のFPDゲイン　  'v16.20/v17.00追加 byやまおか 2010/02/17
            public int off_fpd_integ        ;   // オフセット校正時のFPD積分時間  'v16.20/v17.00追加 byやまおか 2010/02/17
            public int dc_date              ;   // 寸法較正年月日                 'v16.20/v17.00追加 byやまおか 2010/03/02
            public int dc_time              ;   // 寸法較正時間                   'v16.20/v17.00追加 byやまおか 2010/03/02
            public int off_time             ;   // オフセット校正時間             'v16.20/v17.00追加 byやまおか 2010/03/04
            public int gain_time            ;   // ゲイン校正時間                 'v16.20/v17.00追加 byやまおか 2010/03/04
            public int sp_date              ;   // スキャン位置校正年月日         'v17.02追加 byやまおか 2010/07/08
            public int sp_time              ;   // スキャン位置校正時間           'v17.02追加 byやまおか 2010/07/08
            public int coolant_err          ;   // 水流量ｲﾝﾀｰﾛｯｸ 1:異常 0:正常    'v17.10追加 byやまおか 2010/08/25
            public int ud_u_limit           ;   // 昇降上限リミット情報           'v17.20追加 by長野     2010/09/20
            public int ud_l_limit           ;   // 昇降下限リミット情報           'v17.20追加 by長野     2010/09/20
            public int xstg_u_limit         ;   // Ｘ軸上限動作限                 'v17.47/v17.53 追加 by 間々田 2011-03-08
            public int xstg_l_limit         ;   // Ｘ軸下限動作限                 'v17.47/v17.53 追加 by 間々田 2011-03-08
            public int ystg_u_limit         ;   // Ｙ軸上限動作限                 'v17.47/v17.53 追加 by 間々田 2011-03-08
            public int ystg_l_limit         ;   // Ｙ軸下限動作限                 'v17.47/v17.53 追加 by 間々田 2011-03-08
            public int oc_trip              ;   // オイルクーラー電源トリップ     'v18.00/v17.53 追加 by 間々田 2011-03-09
            public int ups_power200         ;   // UPS200Vバックアップ商用電源異常'v18.00/v17.53 追加 by 間々田 2011-03-09

            //-->//追加2014/07/16hata_v19.51反映
            public int gain_iifield_sft;   // (検出器シフト用)ゲイン校正実行時のI.I.視野         'v18.00追加 byやまおか 2011/02/10
            public int gain_kv_sft;   // (検出器シフト用)ゲイン校正実行時の管電圧           'v18.00追加 byやまおか 2011/02/10
            public int gain_ma_sft;   // (検出器シフト用)ゲイン校正実行時の管電流           'v18.00追加 byやまおか 2011/02/10
            public int gain_mt_sft;   // (検出器シフト用)ゲイン校正実行時のX線管番号        'v18.00追加 byやまおか 2011/02/10
            public int gain_filter_sft;   // (検出器シフト用)ゲイン校正実行時のX線フィルタ      'v18.00追加 byやまおか 2011/02/10
            public int gain_bin_sft;   // (検出器シフト用)ゲイン校正実行時のビニングモード   'v18.00追加 byやまおか 2011/02/10
            public int gain_date_sft;   // (検出器シフト用)ゲイン校正実行時の年月日           'v18.00追加 byやまおか 2011/02/10
            public int gain_fpd_gain_sft;   // (検出器シフト用)ゲイン校正実行時のFPDゲイン        'v18.00追加 byやまおか 2011/02/10
            public int gain_fpd_integ_sft;   // (検出器シフト用)ゲイン校正実行時のFPD積分時間      'v18.00追加 byやまおか 2011/02/10
            public int gain_time_sft;   // (検出器シフト用)ゲイン校正実行時の時間             'v18.00追加 byやまおか 2011/02/10
            public int detsftpos_org;   // 検出器シフト 基準位置にいる？ 1:いる 0:いない      'v18.00追加 byやまおか 2011/02/12
            public int detsftpos_sft;   // 検出器シフト シフト位置にいる？ 1:いる 0:いない    'v18.00追加 byやまおか 2011/02/12
            public int normal_rc_cor_sft;   //  ｼﾌﾄｽｷｬﾝ用回転中心較正ｽﾃｰﾀｽ                         'v18.00追加 byやまおか 2011/03/07
            public int cone_rc_cor_sft;   // ｼﾌﾄｺｰﾝﾋﾞｰﾑｽｷｬﾝ用回転中心較正ｽﾃｰﾀｽ                  'v18.00追加 byやまおか 2011/03/07
            public int gain_focus;   // ゲイン校正実行時の焦点                             'v18.00追加 byやまおか 2011/03/11
            public int gain_focus_sft;   // (検出器シフト用)ゲイン校正実行時の焦点             'v18.00追加 byやまおか 2011/03/11
            public int rc_focus;   // 回転中心校正実行時の焦点                           'v18.00追加 byやまおか 2011/03/11
            public int xfilter;   // X線フィルタIndex 0:なし 1:2mm 2:4mm 3:6mm 4:8mm 5:ｼｬｯﾀ(ctinfinit.csvによる) 'v18.00追加 byやまおか 2011/06/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05　ここまで
            public int xfocus;   // X線焦点Index 0:小 1:大 'v18.00追加 byやまおか 2011/06/03
            //->TETS  14/02/22   inaba
            public int ud_xray_ready;   // X線昇降準備完了情報
            public int ud_xray_busy;   // X線昇降動作中情報
            public int ud_xray_error;   // X線昇降ｴﾗｰ情報
            public int ud_xray_limit;   // X線昇降ﾘﾐｯﾀ限情報
            public int ud_xray_u_limit;   // X線昇降上限リミット情報
            public int ud_xray_l_limit;   // X線昇降下限リミット情報
            public float ud_xray_pos;   // X線昇降位置。相対座標
            public float udab_xray_pos;   // X線昇降位置。絶対座標
            public int ud_det_ready;   // 検出器昇降準備完了情報
            public int ud_det_busy;   // 検出器昇降動作中情報
            public int ud_det_error;   // 検出器昇降ｴﾗｰ情報
            public int ud_det_limit;   // 検出器昇降ﾘﾐｯﾀ限情報
            public int ud_det_u_limit;   // 検出器昇降上限リミット情報
            public int ud_det_l_limit;   // 検出器昇降下限リミット情報
            public float ud_det_pos;   // 検出器昇降位置。相対座標
            public float udab_det_pos;   // X線昇降位置。絶対座標
            //<-TETS
            public int largeRotTable;   // v19.51 追加 テーブル装着状態かどうか by長野 2014/03/03
            //<--v19.51反映

            //Rev22.00 回転傾斜テーブル用に追加 by長野 2015/08/20
            public int tiltrot_table;   //回転傾斜テーブル(傾斜と回転ができる)の装着有無 0:無、1:有
            public int tilt_ready;      //回転傾斜テーブル 傾斜準備完了 0:準備未完、1:準備完了
            public int tilt_busy;       //回転傾斜テーブル 傾斜動作中情報 0:停止中、1:動作中
            public int tilt_error;      //回転傾斜テーブル 傾斜エラー情報 0:正常、1:異常
            public int tilt_limit;      //回転傾斜テーブル 傾斜リミット情報 0:限以外、1:限位置
            public int tilt_u_limit;    //回転傾斜テーブル 傾斜リミット情報 0:上限以外、1:上限位置
            public int tilt_l_limit;    //回転傾斜テーブル 傾斜リミット情報 0:加減以外、1:下限位置
            public float tilt_pos;      //回転傾斜テーブル 傾斜現在位置(度)
            public int tiltrot_ready;   //回転傾斜テーブル 回転準備完了 0:準備未完、1:準備完了
            public int tiltrot_busy;    //回転傾斜テーブル 回転動作中情報 0:停止中、1:動作中
            public int tiltrot_error;   //回転傾斜テーブル 回転エラー情報 0:正常、1:異常
            public int tiltrot_limit;   //回転傾斜テーブル 回転リミット情報 0:限以外、1:限位置
            public int tiltrot_u_limit; //回転傾斜テーブル 回転リミット情報 0:上限以外、1:上限位置
            public int tiltrot_l_limit; //回転傾斜テーブル 回転リミット情報 0:加減以外、1:下限位置
            public float tiltrot_pos;   //回転傾斜テーブル 回転現在位置(度)
            public float scan_fcdMecha;  // FCD値(従来値)             //Rev23.10 追加 by長野 2015/10/28
            public float scan_fcdLinear; // FCD値(リニアスケール値)   //Rev23.10 追加 by長野 2015/10/28
            public float scan_fddMecha;  // FDD値(従来値)             //Rev23.10 追加 by長野 2015/10/28
            public float scan_fddLinear; // FDD値(リニアスケール値)   //Rev23.10 追加 by長野 2015/10/28
            public float scan_table_x_posMecha;      // テーブルY軸(光軸と垂直方向)(従来値) //Rev23.10 追加 by長野 2015/10/28
            public float scan_table_x_posLinear;     // テーブルY軸(光軸と垂直方向)(リニアスケール値) //Rev23.10 追加 by長野 2015/10/28
            public float scan_ud_pos;	// スキャン実行時の昇降位置  //Rev23.10 追加 by長野 2015/10/28
            public float scan_ud_posLinear; //スキャン実行時の昇降位置(リニアスケール値)  //Rev23.10 追加 by長野 2015/10/28

            public float colpos_upper;    //上コリメータ位置(mm) Rev24.00 by長野 2016/04/08
            public float colpos_lower;    //下コリメータ位置(mm) Rev24.00 by長野 2016/04/08
            public int sampleSetPos1;     //サンプルセット位置1(0:1の位置にいない,1:1の位置にいる) Rev24.00 by長野 2016/04/28
            public int sampleSetPos2;     //サンプルセット位置2(0:2の位置にいない,1:2の位置にいる) Rev24.00 by長野 2016/04/28
            public int sampleSetPos3;     //サンプルセット位置3(0:3の位置にいない,1:3の位置にいる) Rev24.00 by長野 2016/04/28

            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // 配列の初期化
            }
            #endregion

        }   //MECAINF
        #endregion MECAINF

        #region DISPINF
        /// <summary>
        /// Dispinf構造体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct DISPINF
        {
            #region フィールド

            public int dpreq;//画像表示要求フラグ          未使用   
            public int dpcomp               ;//画像表示モード              未使用          
            public int imgdp                ;// 画像面表示フラグ(非表示･表示) 未使用

            public FixedString256 d_exam    ;// 表示するﾃﾞｨﾚｸﾄﾘ名
            public FixedString256 d_id      ;// 表示するｽﾗｲｽ名
            
            public int d_exammax            ;// ﾃﾞｨﾚｸﾄﾘ登録件数             未使用
            public int d_idmax              ;// 表示中のﾃﾞｨﾚｸﾄﾘのｽﾗｲｽ枚数   未使用
            public int imgcnv               ;// 画像面変換フラグ            未使用
            public int winddp               ;// WW･WL表示要求フラグ         未使用
            public int infdp                ;// ｲﾝﾌｫﾒｰｼｮﾝ表示要求フラグ     未使用
            public int roidp                ;// ﾛｲ表示要求フラグ(非表示･表示) 未使用
            public int grpdp                ;// ｸﾞﾗﾌｨｯｸﾃﾞｨｽﾌﾟﾚｲフラグ
            public int colormode            ;// 表示ﾓｰﾄﾞ（カラー･ﾓｰﾄﾞ）
            public int wwwlmode             ;// 表示幅 （WW）ﾓｰﾄﾞ   未使用
            public int width                ;// カラー表示階調幅
            public int level                ;// カラー表示階調ﾚﾍﾞﾙ
            public int coloralpha           ;// カラー表示             未使用
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8 * 3)]
            public int[] palette           ;// カラー8色パレット
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8 * 2)]
            public int[] paletl1           ;// カラー8色範囲。絶対値
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8 * 2)]
            public int[] paletl2           ;// カラー8色範囲。相対値
            public int alpha                ;// カラーalpha define
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public int[] Rtbl               ;// カラー変換テーブル(R)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public int[] Gtbl               ;// カラー変換テーブル(G)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public int[] Btbl               ;// カラー変換テーブル(B)
            public int color_max            ;// パレット作成時上限CT値
            public int color_min            ;// パレット作成時下限CT値
            public float Gamma              ;//ガンマ補正値　by長野　2012/02/21 

            #endregion


            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // 配列の初期化
                d_exam.Initialize();                // 表示するﾃﾞｨﾚｸﾄﾘ名
                d_id.Initialize();                  // 表示するｽﾗｲｽ名
                palette = new int[3 * 8];            // カラー8色パレット
                paletl1 = new int[2 * 8];            // カラー8色範囲。絶対値    
                paletl2 = new int[2 * 8];            // カラー8色範囲。相対値
                Rtbl = new int[256];                // カラー変換テーブル(R)
                Gtbl = new int[256];                // カラー変換テーブル(G)
                Btbl = new int[256];                // カラー変換テーブル(B)
            }
            #endregion
       
        }   //DISPINF
        #endregion DISPINF

        #region IMAGEINFO
        /// <summary>
        /// 画像付帯情報関係パラメータ
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct IMAGEINFO
        {
            #region フィールド
            public FixedString16 system_name    ;// システム名
            public FixedString6 version         ;// ソフトバージョン情報
            public FixedString2 ct_gentype      ;// CTデータ収集方式（世代）
            public FixedString256 sliceplan_der ;// スライスプランディレクトリ名
            public FixedString256 slice_plan    ;// スライスプラン名
            public FixedString4 d_sileno        ;// マルチスキャン時のスライス番号
            public FixedString10 d_date         ;// 撮影年月日
            public FixedString10 start_time     ;// スキャン時刻
            public FixedString16 workshop       ;// 事業所名
            public FixedString256 comment       ;// コメント
            public FixedString2 d_rawsts        ;// 生データ･ｽﾃｰﾀｽ
            public FixedString6 d_recokind      ;// 付帯情報データ種類
            public FixedString6 full_mode       ;// スキャンモード
            public FixedString4 matsiz          ;// 画像マトリクスサイズ
            public FixedString6 scan_mode       ;// スキャン・モード
            public FixedString6 speed_mode      ;// スキャン速度モード
            public FixedString8 scan_speed      ;// スキャン速度値
            public FixedString4 scan_time       ;// スキャン時間
            public FixedString2 scan_area       ;// 撮影領域名
            public FixedString8 area            ;// スライスエリアの番号
            public FixedString2 slice_wid       ;// スライス幅番号
            public FixedString8 width           ;// スライス幅値
            public FixedString2 det_ap_num      ;// 開口番号
            public FixedString4 focus           ;// Ｘ線焦点
            public FixedString16 tube           ;// X線管球種（形式）
            public FixedString2 energy          ;// X線条件番号
            public FixedString8 volt            ;// 管電圧
            public FixedString8 anpere          ;// 管電流
            public FixedString8 table_pos       ;// テーブル位置（相対座標）
            public FixedString8 d_tablepos      ;// テーブル位置（絶対座標）
            public FixedString6 tilt_angle      ;// データ収集傾斜角度
            public FixedString8 fc              ;// フィルタ関数
            public int bhc                      ;// コーンビーム識別値
            public FixedString256 bhc_dir       ;// コーンビームCT生データディレクトリ名
            public FixedString256 bhc_name      ;// コーンビームCT生データファイル名
            public FixedString256 zoom_dir      ;// ズームファイルディレクトリ名
            public FixedString256 zoom_name     ;// ズームファイル名
            public int zooming                  ;// 拡大画像の有無
            public FixedString4 zoomx           ;// 拡大画像のX座標
            public FixedString4 zoomy           ;// 拡大画像のY座標
            public FixedString4 zoomsize        ;// 拡大画像のサイズ
            public FixedString2 sift_pos        ;// シフト位置
            public FixedString8 scale           ;// 再構成エリア
            public FixedString256 roicaltable_dir;// ROI測定テーブルディレクトリ
            public FixedString256 roical_table  ;// ROI測定テーブル名
            public FixedString256 pdtable_dir   ;// プロフィール寸法測定テーブルディレクトリ
            public FixedString256 pd_table      ;// プロフィール寸法測定テーブル名
            public int roical                   ;// ROI CAL有無
            public int pd                       ;// プロフィール寸法測定有無
            public FixedString6 scano_dir       ;// スキャノ収集時の管球方向角度
            public FixedString2 pro_dir         ;// 試料挿入方向
            public FixedString4 view_dir        ;// 試料観察方向
            public FixedString2 pro_posdir      ;// 試料位置方向
            public FixedString4 scano_dispdir   ;// スキャノ表示方向
            public FixedString6 pix_minval      ;// 画素の最小値(CT値)
            public FixedString6 pix_maxval      ;// 画素の最大値(CT値)
            public FixedString6 ww              ;// ウィンドウ幅
            public FixedString6 wl              ;// ウィンドウレベル

            public FixedString2 dummy2          ;// 調整用（これがないと後のデータがずれます）

            public int graphic                  ;// ｸﾞﾗﾌｨｯｸ画像有無
            public FixedString8 rotation        ;// データ収集開始位置
            public int trvsno                   ;// スキャントラバース数
            public int trvspt                   ;// スキャントラバースポイント
            public float realfdd                ;// X線焦点の長さ
            public float dtpitch                ;// ﾒｲﾝチャンネルピッチ
            public float fanangle               ;// FAN角度
            public float fcd                    ;// 検査テーブルの中心点までの焦点
            public float trpitch                ;// トラバースピッチ
            public float tbldia                 ;// テーブル直径
            public int aprtpt                   ;// ｱﾌﾟﾛｰﾁポイント数
            public int bias                     ;// 画素値の調整値
            public float slope                  ;// 画素値の調整値
            public int mainch                   ;// 主検出器チャンネル数
            public int refch                    ;// 比較検出器チャンネル数
            public int dumych                   ;// データ収集ﾀﾞﾐｰチャンネル数
            public int datach                   ;// データ収集ﾚﾌｧﾚﾝｽ+ﾒｲﾝチャンネル数
            public int allch                    ;// データ収集全チャンネル数
            public int offsetapr                ;// オフセットデータ助走ポイントデータ数
            public int offsetpt                 ;// オフセットデータ収集ポイント数
            public int airdtpt                  ;// 空気補正データ長
            public int clct_inf                 ;// スキャンの撮影領域画素数
            public int scanotvs                 ;// スキャノデータ収集トラバース数
            public int scanoapr                 ;// スキャノアプローチポイントデータ数
            public int scanopt                  ;// スキャノデータ収集ポイント数
            public int scanoscl                 ;// ｽｷｬﾉﾃﾞｰﾀ収集ｽｹｰﾙｻｲｽﾞ                  '84
            public int scanoair                 ;// スキャノデータ収集空気データポイント数
            public int scanobias                ;// スキャノデータ収集バイアス値
            public float scanoslop              ;// スキャノデータ収集DCVｽﾛｰﾌﾟ値
            public float scanotblbias           ;// スキャノデータ収集テーブルバイアス値
            public float scanoudpitch           ;// スキャノデータ収集ｱｯﾌﾟﾀﾞｳﾝピッチ数
            public float scanodpratio           ;// スキャノデータ収集表示横方向圧縮率
            public FixedString4 scano_matsiz    ;// スキャノ画像のマトリクスサイズ
            public FixedString8 scan_view       ;// ビュー数
            public FixedString8 integ_number    ;// 積算枚数
            public float recon_start_angle      ;// 再構成開始角度(ズーミング時使用)
            public int image_bias               ;// バイアス
            public float image_slope            ;// スロープ
            public int image_direction          ;// 画像方向
            public float mcenter_channel        ;// 再構成用センターチャンネル
            public float mchannel_pitch         ;// 1チャンネル当たりの角度
            public float mzoomx                 ;// ズーミング時のROI中心点X座標(pixel)
            public float mzoomy                 ;// ズーミング時のROI中心点Y座標(pixel)
            public float mzoomsize              ;// ズーミング時のROIサイズ(pixel)
            public float mscan_area             ;// スキャンエリア(mm)
            public float max_mscan_area         ;// 最大スキャンエリア(mm)
            public int fimage_bit               ;// 画像取り込みﾋﾞｯﾄ数
            public float fid                    ;// X線焦点からIIまでの距離
            public int recon_mask               ;// 再構成形状
            public int n1                       ;// 有効データ開始チャンネル
            public int n2                       ;// 有効データ終了チャンネル
            public int mc                       ;// 縦中心チャンネル
            public float theta0                 ;// 有効ファン角
            public float theta01                ;// 有効データ包含ファン角1(radian)
            public float theta02                ;// 有効データ包含ファン角2(radian)
            public float thetaoff               ;// 有効データ包含ファン角
            public float nc                     ;// 回転中心チャンネル
            public float delta_theta            ;// n方向データピッチ(radian)
            public float dpm                    ;// m方向データピッチ(mm)
            public float n0                     ;// n方向画像中心対応チャンネル
            public float m0                     ;// m方向画像中心対応チャンネル
            public float alpha                  ;// ｵｰﾊﾞｰﾗｯﾌﾟ角度(radian)
            public float zp                     ;// ヘリカルピッチ(mm)
            public float zs0                    ;// 初期1枚目のスライス位置
            public int iud                      ;// 上昇下降識別値
            public int ioff                     ;// オフセット識別値
            public int k                        ;// スライス枚数
            public float delta_z                ;// 軸方向Boxelサイズ(mm)
            public float ze0                    ;// 初期K枚目スライスのZ位置(mm)
            public int inh                      ;// ヘリカルモード
            public int md                       ;// 縦チャンネル数
            public float b1                     ;// コーンビーム用幾何歪ﾊﾟﾗﾒｰﾀB1
            public float scan_posi_a            ;// スキャン位置の傾き
            public int multi_tube               ;// 複数X線管
            public float z0                     ;// コーンビームスキャン時のテーブル位置
            public int acq_view                 ;// データ収集ﾋﾞｭｰ数
            public float fid_offset             ;// FIDオフセット
            public float fcd_offset             ;// FCDオフセット
            public float a1                     ;// 幾何歪み補正ﾊﾟﾗﾒｰﾀA1 (1/mm)
            public float fs                     ;// 焦点サイズ(mm)
            public float x0                     ;// 骨塩等価物質密度（mg/cm3）
            public float x1                     ;// 骨塩等価物質密度（mg/cm3）
            public float x2                     ;// 骨塩等価物質密度（mg/cm3）
            public float x3                     ;// 骨塩等価物質密度（mg/cm3）
            public float x4                     ;// 骨塩等価物質密度（mg/cm3）
            public int instance_num             ;// ｲﾝｽﾀﾝｽ番号
            public FixedString10 iifield        ;// I.I.視野
            public FixedString10 filter         ;// フィルタ
            public int study_id                 ;// 検査ID
            public int series_num               ;// ｼﾘｰｽﾞ番号
            public int acq_num                  ;// 収集番号
            public FixedString64 instance_uid   ;// ｲﾝｽﾀﾝｽUID
            public float frame_rate             ;// ﾌﾚｰﾑﾚｰﾄ
            public float scan_start_angle       ;
            public int detector                 ;// 検出器 0:X線II　1:FPD
            public int data_mode                ;// データモード
            public int cone_image_mode          ;// コーンビーム再構成時の画質
            public int fine_table_x             ;// 微調テーブルＸ軸の有無
            public float ftable_x_pos           ;// 微調テーブルＸ軸の座標（mm）
            public int fine_table_y             ;// 微調テーブルＹ軸の有無
            public float ftable_y_pos           ;// 微調テーブルＹ軸の座標（mm）
            public int rotate_select            ;// 回転選択結果       0:テーブル回転  1:Ｘ線管回転   REV9.00 追加 2004-01-29 by 間々田
            public int c_cw_ccw                 ;// 回転方向   追加 2004/06/03 by 間々田
            public int kv                       ;// ビニング係数(v_mag/h_mag)                          REV10.0 追加 2005-01-31 by 間々田
            public FixedString16 filter_process ;// ﾌｨﾙﾀ処理の方法     Rev13.00 追加 2006-01-24 by やまおか
            public int filter_proc              ;// ﾌｨﾙﾀ処理の方法 0:FFT 1:ｺﾝﾎﾞﾘｭｰｼｮﾝ                  REV13.00 追加 by Ohkado 2007/04/16
            public float data_acq_time          ;// データ収集時間(秒)                                   REV14.00 追加 by Ohkado 2007/06/25
            public float recon_time             ;// 再構成時間(秒)                                     REV14.00 追加 by Ohkado 2007/06/25
            public int rfc                      ;// RFC処理     0:なし 1:弱 2:中 3:強                  REV14.00 追加 by Ohkado 2007/06/25
            public FixedString8 rfc_char        ;// RFC用文字　 0:なし 1:弱 2:中 3:強                  REV14.00 追加 by Ohkado 2007/06/25
            public float table_x_pos            ;// 試料テーブル(光軸と垂直方向)座標[mm]               v15追加 by 長野 2009/7/16
            public int cob_view                 ;// ｺｰﾝﾋﾞｰﾑ生ﾃﾞｰﾀ分割保存時の基準ﾋﾞｭｰ数（最後の分割分はこれより小さい値になる)  Rev16.30 10-05-17 by IWASAWA
            public int fpd_gain                 ;// FPDゲイン                                           Rev17.00追加 2010/02/17 byやまおか
            public int fpd_integ                ;// FPD積分時間                                         Rev17.00追加 2010/02/17 byやまおか
            public FixedString8 gamma;// ガンマ補正値 'v19.00 追加 by長野 2012/02/21

            //v19.00->(電S2)永井
            public int mbhc_flag;

            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            //public byte[] mbhc_dir;/* BHC				*/
            public FixedString256 mbhc_dir      ;// BHC

            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            //public byte[] mbhc_name;/* BHC			*/
            public FixedString256 mbhc_name     ;// BHC
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public float[] mbhc_a               ;// BHC
            
            public float mbhc_p0                ;// BHC
            public float mbhc_AirLogValue       ;// BHC
            //<-v19.00

            //追加2014/07/16hata_v19.51反映
            public float fpd_gain_f;// FPDゲイン 表示用(pF)   'v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public float fpd_integ_f;// FPD積分時間 表示用(ms) 'v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public int xfilter;// X線フィルタ Index  'v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public FixedString8 xfilter_c;// X線フィルタ 文字   'v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public int xfocus;// X線焦点 Index      'v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public FixedString8 xfocus_c;// X線焦点 文字       'v18.00追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            public int largetRotTable;// v19.51 回転大テーブルを装着した状態でスキャンしたかどうか by長野 2014/03/03
            //<-v19.51

            public int table_rotation; //Rev20.00 追加 テーブル回転 0:ステップ、1:連続 by長野 2015/01/29
            public int auto_centering; //Rev20.00 追加 スキャンスタート時のオートセンタリング 0:無し、1:有り by長野 2015/01/29

            public float mscano_area; //Rev21.00 スキャノエリア          追加 by長野 2015/02/19
            public float mscano_mdtpitch; //Rev21.00 スキャノデータピッチ    追加 by長野 2015/02/19
            public FixedString8 mscano_width; //Rev21.00 スキャノ厚              追加 by長野 2015/02/19
            public int mscanopt; //Rev21.00 スキャノポイント        追加 by長野 2015/02/19
            public float mscanoscl; //Rev21.00 スキャノスケール        追加 by長野 2015/02/19
            public float mscano_udpitch; //Rev21.00 スキャノ昇降ピッチ      追加 by長野 2015/02/19
            public FixedString8 mscano_integ_number; //Rev21.00 スキャノ積算枚数        追加 by長野 2015/02/19
            public int mscano_bias; //Rev21.00 スキャノバイアス        追加 by長野 2015/02/19
            public float mscano_slope; //Rev21.00 スキャノスロープ        追加 by長野 2015/02/19
            public float mscano_dp_ratio;/* スキャノ圧縮比			   Rev21.00 追加 by長野 2015/02/19 */

            //public int xls; //Rev23.00 シングル用 有効データ開始画素 転中心ch調整のため追加 追加 by長野 2015/09/07
            //public int xle; //Rev23.00 コーン用 有効データ終了画素 転中心ch調整のため追加 追加 by長野 2015/09/07

            public float numOfAdjCenterCh; //Rev23.00 回転中心ch調整量(ch) 追加 by長野 2015/09/07

            public float scan_fcdMecha; // FCD値(従来値)             Rev23.10 追加 by長野 2015/10/28
            public float scan_fcdLinear; // FCD値(リニアスケール値)   Rev23.10 追加 by長野 2015/10/28
            public float scan_fddMecha; // FDD値(従来値)             Rev23.10 追加 by長野 2015/10/28
            public float scan_fddLinear; // FDD値(リニアスケール値)						Rev23.10 追加 by長野 2015/10/28
            public float scan_table_x_posMecha; // テーブルY軸(光軸と垂直方向)(従来値)            Rev23.10 追加 by長野 2015/10/28
            public float scan_table_x_posLinear; // テーブルY軸(光軸と垂直方向)(リニアスケール値)  Rev23.10 追加 by長野 2015/10/28
            public float scan_udab_pos; // 昇降(従来値)  Rev23.10 追加 by長野 2015/10/28
            public float scan_ud_linear_pos; // 昇降(リニアスケール値)  Rev23.10 追加 by長野 2015/10/28

            public int w_scan; //Wスキャン(0:なし,1:あり) Rev25.00 追加 by長野 2016/07/05

            public int mbhc_phantomless;         // ファントムレスBHC実行有無(0:なし、1～あり(材質を示すインデックス)) Rev26.00 add by chouno 2016/12/27
            public int mbhc_phantomless_colli_on;// ファントムレスBHC実行時コリメータ有無(0:なし、1：あり) Rev26.00 add by chouno 2016/12/27
            public FixedString32 mbhc_phantomless_c;// ファントムレスBHC材質名 Rev26.00 add by chouno 2016/12/27
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] mbhc_phantomless_para;// ファントムレスBHC用係数 Rev26.00 add by chouno 2016/12/27

            #endregion

            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // 配列の初期化
                system_name.Initialize();     // システム名
                version.Initialize();          // ソフトバージョン情報
                ct_gentype.Initialize();       // CTデータ収集方式（世代）
                sliceplan_der.Initialize();  // スライスプランディレクトリ名
                slice_plan.Initialize();     // スライスプラン名
                d_sileno.Initialize();         // マルチスキャン時のスライス番号
                d_date.Initialize();          // 撮影年月日
                start_time.Initialize();      // スキャン時刻
                workshop.Initialize();        // 事業所名
                comment.Initialize();        // コメント
                d_rawsts.Initialize();        // 生データ･ｽﾃｰﾀｽ
                d_recokind.Initialize();       // 付帯情報データ種類
                full_mode.Initialize();        // スキャンモード
                matsiz.Initialize();           // 画像マトリクスサイズ
                scan_mode.Initialize();        // スキャン・モード
                speed_mode.Initialize();       // スキャン速度モード
                scan_speed.Initialize();      // スキャン速度値
                scan_time.Initialize();        // スキャン時間
                scan_area.Initialize();        // 撮影領域名
                area.Initialize();             // スライスエリアの番号
                slice_wid.Initialize();        // スライス幅番号
                width.Initialize();            // スライス幅値
                det_ap_num.Initialize();       // 開口番号
                focus.Initialize();            // Ｘ線焦点
                tube.Initialize();            // X線管球種（形式）
                energy.Initialize();           // X線条件番号
                volt.Initialize();             // 管電圧
                anpere.Initialize();           // 管電流
                table_pos.Initialize();        // テーブル位置（相対座標）
                d_tablepos.Initialize();       // テーブル位置（絶対座標）
                tilt_angle.Initialize();       // データ収集傾斜角度
                fc.Initialize();               // フィルタ関数
                bhc_dir.Initialize();        // コーンビームCT生データディレクトリ名
                bhc_name.Initialize();       // コーンビームCT生データファイル名
                zoom_dir.Initialize();       // ズームファイルディレクトリ名
                zoom_name.Initialize();      // ズームファイル名
                zoomx.Initialize();            // 拡大画像のX座標
                zoomy.Initialize();            // 拡大画像のY座標
                zoomsize.Initialize();         // 拡大画像のサイズ
                sift_pos.Initialize();         // シフト位置
                scale.Initialize();            // 再構成エリア
                roicaltable_dir.Initialize();// ROI測定テーブルディレクトリ
                roical_table.Initialize();   // ROI測定テーブル名
                pdtable_dir.Initialize();    // プロフィール寸法測定テーブルディレクトリ
                pd_table.Initialize();       // プロフィール寸法測定テーブル名
                scano_dir.Initialize();        // スキャノ収集時の管球方向角度
                pro_dir.Initialize();          // 試料挿入方向
                view_dir.Initialize();         // 試料観察方向
                pro_posdir.Initialize();       // 試料位置方向
                scano_dispdir.Initialize();    // スキャノ表示方向
                pix_minval.Initialize();      // 画素の最小値(CT値)
                pix_maxval.Initialize();       // 画素の最大値(CT値)
                ww.Initialize();               // ウィンドウ幅
                wl.Initialize();               // ウィンドウレベル
                rotation.Initialize();         // データ収集開始位置
                scano_matsiz.Initialize();     // スキャノ画像のマトリクスサイズ
                scan_view.Initialize();        // ビュー数
                integ_number.Initialize();      // 積算枚数
                iifield.Initialize();           // I.I.視野
                filter.Initialize();            // フィルタ
                instance_uid.Initialize();      // ｲﾝｽﾀﾝｽUID
                filter_process.Initialize();    // ﾌｨﾙﾀ処理の方法
                rfc_char.Initialize();          // RFC用文字　 0:なし 1:弱 2:中 3:強 
                gamma.Initialize();             //ガンマ補正値
                mbhc_dir.Initialize();          //BHC
                mbhc_name.Initialize();         //BHC

                mbhc_a = new float[7];          //BHC

                xfilter_c.Initialize();         // X線フィルタ 文字　//追加2014/07/16hata_v19.51反映
                xfocus_c.Initialize();          // X線焦点 文字　//追加2014/07/16hata_v19.51反映    

                mbhc_phantomless_c.Initialize();      //Rev26.00 add by chouno 2016/12/28
                mbhc_phantomless_para = new float[3]; //Rev26.00 add by chouno 2016/12/28
            }
            #endregion


        }
        #endregion IMAGEINFO

        #region RECONINF
        /// <summary>
        /// 再々構成・ズーミング画像生成情報
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECONINF
        {
            #region フィールド
            public float table_abpos;               // ﾃｰﾌﾞﾙ絶対位置
            public float table_repos;               // ﾃｰﾌﾞﾙ相対位置
            public FixedString256 raw_dir;          // 生ﾃﾞｰﾀのﾃﾞｨﾚｸﾄﾘ名
            public FixedString256 raw_name;         // 生ﾃﾞｰﾀのﾌｧｲﾙ名
            public int scan_mode;                   // ｽｷｬﾝﾓｰﾄﾞ
            public int zoomflag;                    // ｽﾞｰﾐﾝｸﾞ処理有無
            public FixedString256 zooming_dir;      // ｽﾞｰﾐﾝｸﾞﾃｰﾌﾞﾙﾃﾞｨﾚｸﾄﾘ名
            public FixedString256 zooming;          // ｽﾞｰﾐﾝｸﾞ収集ﾃｰﾌﾞﾙ名
            public float x;                         // ｽﾞｰﾑ中心点のX	REV M3
            public float y;                         // ｽﾞｰﾑ中心点のY	REV M3
            public float size;                      // ｽﾞｰﾑのｻｲｽﾞ		REV M3
            public FixedString6 speed_mode;         // ｽｷｬﾝ速度
            public FixedString8 fcno;               // ﾌｨﾙﾀ関数
            public FixedString2 area;               // ｴﾘｱ
            public FixedString2 energy;             // 管電圧
            public FixedString2 slice_wid;          // ｽﾗｲｽ幅
            public FixedString2 det_ap;             // 検出器開口
            public FixedString4 matrix_size;        // ﾏﾄﾘｯｸｽｻｲｽﾞ
            public FixedString2 dummy;              // 調整用(これがないと後のデータがずれます)
            public int auto_print;                  // ｵｰﾄﾌﾟﾘﾝﾄ有無情報
            public int bhcflag;                    // BHC処理有無	
            public FixedString256 bhc_dir;          // BHC補正ﾃｰﾌﾞﾙのﾃﾞｨﾚｸﾄﾘ
            public FixedString256 bhc_name;         // BHC処理のﾃｰﾌﾞﾙﾌｧｲﾙ名	
            public int zooming_num;                 // zooming 枚数

        　　//19.00->(電S2)永井
            public int mbhc_flag;                    // BHC処理有無	
            public FixedString256 mbhc_dir;          // BHC補正ﾃｰﾌﾞﾙのﾃﾞｨﾚｸﾄﾘ
            public FixedString256 mbhc_name;         // BHC処理のﾃｰﾌﾞﾙﾌｧｲﾙ名	
            public float mbhc_airLogValue;           // ｽﾞｰﾑ中心点のX	REV M3
            //<-v19.00

            #endregion

            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // 配列の初期化
                raw_dir.Initialize();           // 生ﾃﾞｰﾀのﾃﾞｨﾚｸﾄﾘ名
                raw_name.Initialize();          // 生ﾃﾞｰﾀのﾌｧｲﾙ名	
                zooming_dir.Initialize();       // ｽﾞｰﾐﾝｸﾞﾃｰﾌﾞﾙﾃﾞｨﾚｸﾄﾘ名	
                zooming.Initialize();           // ｽﾞｰﾐﾝｸﾞ収集ﾃｰﾌﾞﾙ名
                speed_mode.Initialize();        // ｽｷｬﾝ速度
                fcno.Initialize();              // ﾌｨﾙﾀ関数
                area.Initialize();              // ｴﾘｱ
                energy.Initialize();            // 管電圧
                slice_wid.Initialize();         // ｽﾗｲｽ幅	
                det_ap.Initialize();            // 検出器開口
                matrix_size.Initialize();       // ﾏﾄﾘｯｸｽｻｲｽﾞ
                bhc_dir.Initialize();           // BHC補正ﾃｰﾌﾞﾙのﾃﾞｨﾚｸﾄﾘ
                bhc_name.Initialize();          // BHC処理のﾃｰﾌﾞﾙﾌｧｲﾙ名        
                mbhc_dir.Initialize();          // BHC補正ﾃｰﾌﾞﾙのﾃﾞｨﾚｸﾄﾘ
                mbhc_name.Initialize();         // BHC処理のﾃｰﾌﾞﾙﾌｧｲﾙ名        
            }
            #endregion        
        
        }   //RECONINF
        #endregion RECONINF

        #region ROIKEY
        /// <summary>
        /// Roikey構造体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ROIKEY
        {
            #region フィールド
            /// <summary>
            /// ﾛｲ種別ﾓｰﾄﾞ
            /// </summary>
	        public int imgroi;
	    
            /// <summary>
            /// ﾛｲ動作ﾓｰﾄﾞ      未使用
            /// </summary>
            public int roi_mode;
	    
            /// <summary>
            /// ﾛｲ X座標
            /// </summary>
            public int roi_x;
	    
            /// <summary>
            /// ﾛｲ Y座標
            /// </summary>
            public int roi_y;
	    
            /// <summary>
            /// ﾛｲ Xｻｲｽﾞ
            /// </summary>
            public int roi_xsize;
	    
            /// <summary>
            /// ﾛｲ Yｻｲｽﾞ
            /// </summary>
            public int roi_ysize;
	    
            /// <summary>
            /// 不規則ROI座標
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256 * 2)]
            public int[] trace_pos;
            #endregion

            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // 配列の初期化
                trace_pos = new int[2 * 256]; // 不規則ROI座標
            }
            #endregion
        }   //ROIKEY
        #endregion ROIKEY

        #region ZOOMTBL
        /// <summary>
        /// ズーミングテーブル
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ZOOMTBL
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (32 * 3))]
            public float[] zoomtable;
            public FixedString256 commnet;/* コメント					*/

            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                //zoomtable.Initialize();
                //// 配列の初期化
                //commnet.Initialize();
            }
            #endregion

        }   // ZOOMTBL
        #endregion ZOOMTBL

        #region SP2INF
        /// <summary>
        /// SP2INF
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SP2INF
        {

            public int selnum;
            public int pd_start;
            public int rc_start;

            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
            }
            #endregion

        }   // SP2INF
        #endregion SP2INF

        #region PDPLAN
        /// <summary>
        /// PDPLAN
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct PDPLAN
        {
            #region フィールド
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public FixedString2 area;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public FixedString4 zx;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public FixedString4 zy;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public FixedString4 size;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public FixedString256 pd_dir;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public FixedString256 pd_table;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public int[] xs;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public int[] ys;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public int[] xe;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public int[] ye;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public int[] low;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public int[] high;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public int[] pt1low;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public int[] pt1high;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public int[] width;

            public int slnum;
            #endregion

            //int size = 0;

            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                //area = FixedString.CreateArray<FixedString2>(1); 
                //zx = FixedString.CreateArray<FixedString4>(1);  
                //zy = FixedString.CreateArray<FixedString4>(1);       
                //size = FixedString.CreateArray<FixedString4>(1);       
                //pd_dir = FixedString.CreateArray<FixedString256>(1);    
                //pd_table = FixedString.CreateArray<FixedString256>(1);

                area.Initialize();
                zx.Initialize();
                zy.Initialize();
                size.Initialize();
                pd_dir.Initialize();
                pd_table.Initialize();

                xs = new int[20];
                ys = new int[20];
                xe = new int[20];
                ye = new int[20];
                low = new int[20];
                high = new int[20];
                pt1low = new int[20];
                pt1high = new int[20];
            }
            #endregion

        }   // PDPLAN
        #endregion PDPLAN

        #region DISCHARGE_PROTECT
        /// <summary>
        /// DISCHARGE_PROTECT
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct DISCHARGE_PROTECT
        {

            public int xon_init_kv;
            public float xon_rise_time;
            public float ct_para1;
            public float ct_para2;
            public float ct_para3;
            public int ct_max_time;
            public float it_para1;
            public float it_para2;
            public float it_para3;
            public float it_para4;
            public float it_para5;
            public float it_para6;
            public int it_min_time;

            #region 初期化
            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
            }
            #endregion

        }   // DISCHARGE_PROTECT
        #endregion DISCHARGE_PROTECT


        #region AUTOPOS関係
       //********************************************************************************
        //  AutoPosにあった関数
        //********************************************************************************
        //SIZE構造体
        [StructLayout(LayoutKind.Sequential)]
        //public struct SIZE    //ありふれているので名前を変えておく
        public struct PosSIZE
        {
            public int CX;
            public int CY;
        }

        //追加2014/09/20(検S1)hata
        //SIZE構造体
        [StructLayout(LayoutKind.Sequential)]
         public struct PosPoint
        {
            public short X;
            public short Y;
        }

        //矩形ROI構造体
        [StructLayout(LayoutKind.Sequential)]
        public struct RECTROI
        {
            //左上座標
            //public Points pt;         //変更2014/09/20(検S1)hata
            public PosPoint pt;
            //サイズ
            public PosSIZE sz;
        }

        //試料テーブル位置構造体
        [StructLayout(LayoutKind.Sequential)]
        public struct SampleTable
        {
            //FCD方向
            public float FCD;
            //光軸直交方向
            public float lr;
            //昇降方向
            public float ud;
        }

        //微調テーブル位置構造体
        [StructLayout(LayoutKind.Sequential)]
        public struct FineTable
        {
            //x軸 (光軸方向)
            public float x;
            //y軸 (光軸直交方向)
            public float y;
        }

        //検出器情報構造体
        [StructLayout(LayoutKind.Sequential)]
        public struct Detector_Info
        {
            //検出器位置
            public float FDD;
            //検出器横方向ピッチ
            public float pitchH;
            //検出器縦方向ピッチ
            public float pitchV;
            //X線検出器の種類    v16.20/v17.00 追加 by 山影 10-03-04
            public short DetType;
        }
       #endregion AUTOPOS


    }
}
