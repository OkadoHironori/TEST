using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Test
{
	static class modMecainf
	{

#if AllTest

        //メカ情報
        public struct mecainfType
        {
            ////EMERGNCY信号状態
            //public int emergency;
            ////ﾒｶ準備完了信号状態
            //public int mecha_ready;
            ////ﾒｶ動作中信号状態
            //public int mecha_busy;
            ////CPU処理準備完了信号状態
            //public int cpu_ready;
            ////ｽｷｬﾝ･ｽｷｬﾉ動作中信号状態
            //public int scan;
            ////UPS電源ON/OFF状態
            //public int ups_power;
            ////X線制御準備完了信号状態
            //public int x_ready;
            ////ﾃｽﾄｽｷｬﾝ情報
            //public int test_scan;
            ////機構系の状態。ﾒｶｴﾗｰ情報
            //public int m_error;
            ////回転ﾓｰﾄﾞ
            //public int rot_mode;
            ////昇降準備完了情報
            //public int ud_ready;
            ////昇降動作中情報
            //public int ud_busy;
            ////昇降ｴﾗｰ情報
            //public int ud_error;
            ////昇降ﾘﾐｯﾀ限情報
            //public int ud_limit;
            ////昇降位置。相対座標
            //public float ud_pos;
            ////昇降位置。絶対座標
            //public float udab_pos;
            ////ﾄﾗﾊﾞｰｽ準備完了情報
            //public int tr_reset;
            ////ﾄﾗﾊﾞｰｽ動作情報
            //public int tr_busy;
            ////ﾄﾗﾊﾞｰｽｴﾗｰ情報
            //public int tr_error;
            ////ｴﾘｱｻｲｽﾞ情報
            //public int area_size;
            ////ﾄﾗﾊﾞｰｽ軸のSﾘｾｯﾄ位置
            //public int tr_resets;
            ////ﾄﾗﾊﾞｰｽ軸のMﾘｾｯﾄ位置
            //public int tr_resetm;
            ////ﾄﾗﾊﾞｰｽ軸のLﾘｾｯﾄ位置
            //public int tr_resetl;
            ////ﾄﾗﾊﾞｰｽﾘﾐｯﾀ限情報
            //public int tr_limit;
            ////ﾄﾗﾊﾞｰｽ位置情報
            //public int tr_pos;
            ////回転準備完了情報
            //public int rot_ready;
            ////回転動作中情報
            //public int rot_busy;
            ////回転ｴﾗｰ情報
            //public int rot_error;
            ////回転軸の0ﾘｾｯﾄ位置
            //public int rot_reset;
            ////回転軸の180ﾘｾｯﾄ位置
            //public int rot_180;
            ////回転位置
            //public int rot_pos;
            ////芯位置準備完了情報
            //public int cnt_ready;
            ////芯位置動作中情報
            //public int cnt_busy;
            ////芯位置異常
            //public int cnt_abnormal;
            ////芯位置ｻｰﾎﾞ異常
            //public int cnt_error;
            ////芯位置ﾘﾐｯﾀ限異常
            //public int cnt_limit;
            ////芯位置ﾎﾟｼﾞｼｮﾝ
            //public int cnt_pos;
            ////ｼｬｯﾀ･ﾌｨﾙﾀ準備完了情報
            //public int filter_ready;
            ////ｼｬｯﾀ･ﾌｨﾙﾀ起動情報
            //public int filter_busy;
            ////ｼｬｯﾀ･ﾌｨﾙﾀｴﾗｰ情報
            //public int filter_error;
            ////ﾌｨﾙﾀ-ｴﾈﾙｷﾞ-
            //public int filter_energy;
            ////ｼｬｯﾀ･ﾌｨﾙﾀﾘﾐｯﾀ限情報
            //public int filter_limit;
            ////X線ｼｬｯﾀ-情報
            //public int shutter;
            ////ｺﾘﾒｰﾀ準備完了情報
            //public int colimeter_ready;
            ////ｺﾘﾒｰﾀ起動情報
            //public int colimeter_busy;
            ////ｺﾘﾒｰﾀｴﾗｰ情報
            //public int colimeter_error;
            ////ｺﾘﾒｰﾀｻｲｽﾞ情報
            //public int colimeter_size;
            ////ｺﾘﾒｰﾀﾘﾐｯﾀ制御情報
            //public int colimeter_limit;
            ////管球種準備完了情報
            //public int tube_ready;
            ////管球種起動情報
            //public int tube_busy;
            ////管球種ｴﾗｰ情報
            //public int tube_error;
            ////管球種
            //public int tube_energy;
            ////管球種ﾘﾐｯﾀ制御情報
            //public int tube_limit;
            ////ｼﾌﾄ準備完了
            //public int shift_ready;
            ////ｼﾌﾄ動作中
            //public int shift_busy;
            ////ｼﾌﾄｴﾗｰ
            //public int shift_error;
            ////ｼﾌﾄｻｲｽﾞ
            //public int shift_size;
            ////ｼﾌﾄﾘﾐｯﾀ限
            //public int shift_limit;
            ////X線準備完了
            //public int xray_ready;
            ////X線 ON/OFF状態
            //public int xray_on;
            ////X線ｱﾍﾞｲﾗﾌﾞﾙ状態
            //public int xray_avl;
            ////X線ｳｫｰﾑｱｯﾌﾟ状態
            //public int xray_agng;
            ////ｳｫｰﾑｱｯﾌﾟ状態
            //public int warmup;
            ////ｳｫｰﾑｱｯﾌﾟｽｹｼﾞｭｰﾙ
            //public int warmup_days;
            ////X線過設定
            //public int xray_ovset;
            ////X線制御機ｴﾗｰ
            //public int xray_slferr;
            ////X線発生装置温度異常
            //public int xray_tmp;
            ////X線系の状態。X線ｴﾗｰ情報
            //public int xray_error;
            ////ｲﾝﾀｰﾛｯｸ準備完了
            //public int interlock_ready;
            ////保守扉+試料扉
            //public int interlock_open1;
            ////X線ｼｰﾙﾄﾞ
            //public int interlock_open2;
            ////DAS異常
            //public int interlock_das;
            ////冷却水供給装置異常
            //public int interlock_dwc;
            ////DAS電源ｴﾗｰ
            //public int power_err;
            ////DAS温度異常
            //public int therm_err;
            ////ﾌﾟﾘﾝﾀ動作中
            //public int ipr;
            ////ﾌﾟﾘﾝﾀ接続断
            //public int duc;
            ////ﾌﾟﾘﾝﾀ用紙切れ
            //public int pdo;
            ////FAPC準備完了情報
            //public int fapc_ready;
            ////その他のFAPC異常
            //public int fapc_error;
            ////ｽｷｬﾝ･ｽｷｬﾉ中に減速域に入った
            //public int slow_position;
            ////ﾃﾞｰﾀ収集異常
            //public int data_error;
            ////X線ﾘﾓｰﾄ
            //public int x_ray_auto;
            ////RAMﾃﾞｰﾀ異常
            //public int ram_data_error;
            ////回線ﾓｰﾄﾞ（切断か接続か）
            //public int line_mode;
            ////ｵﾍﾟﾊﾟﾈの状態
            //public int op_panel;
            ////Ｘ線焦点切替Ｘ方向準備完了
            //public int focus_x_ready;
            ////Ｘ線焦点切替Ｘ方向起動情報
            //public int focus_x_busy;
            ////Ｘ線焦点切替Ｘ方向ｴﾗｰ情報
            //public int focus_x_error;
            ////Ｘ線焦点切替Ｘ方向焦点位置
            //public int focus_x_energy;
            ////Ｘ線焦点切替Ｘ方向ﾘﾐｯﾀ限情報
            //public int focus_x_limit;
            ////Ｘ線焦点切替Ｙ方向準備完了
            //public int focus_y_ready;
            ////Ｘ線焦点切替Ｙ方向起動情報
            //public int focus_y_busy;
            ////Ｘ線焦点切替Ｙ方向ｴﾗｰ情報
            //public int focus_y_error;
            ////Ｘ線焦点切替Ｙ方向焦点位置
            //public int focus_y_energy;
            ////Ｘ線焦点切替Ｙ方向ﾘﾐｯﾀ限情報
            //public int focus_y_limit;
            ////Ｘ線焦点切替Ｚ方向準備完了
            //public int focus_z_ready;
            ////Ｘ線焦点切替Ｚ方向起動情報
            //public int focus_z_busy;
            ////Ｘ線焦点切替Ｚ方向ｴﾗｰ情報
            //public int focus_z_error;
            ////Ｘ線焦点切替Ｚ方向焦点位置
            //public int focus_z_energy;
            ////Ｘ線焦点切替Ｚ方向ﾘﾐｯﾀ限
            //public int focus_z_limit;
            
            ////ｼｬｯﾀﾌｨﾙﾀ選択情報
            //public int filter_Renamed;
            
            ////回転速度情報 REV2.00
            //public int rot_speed_const;
            ////較正用ファントム機構準備完了情報
            //public int phm_ready;
            ////較正用ファントム機構動作中情報
            //public int phm_busy;
            ////較正用ファントム機構エラー情報
            //public int phm_error;
            ////較正用ファントム機構リミット限情報
            //public int phm_limit;
            ////較正用ファントム機構位置情報
            //public int phm_onoff;
            ////幾何歪較正ステータス
            //public int vertical_cor;
            ////ﾉｰﾏﾙｽｷｬﾝ用回転中心較正ｽﾃｰﾀｽ
            //public int normal_rc_cor;
            ////ｺｰﾝﾋﾞｰﾑｽｷｬﾝ用回転中心較正ｽﾃｰﾀｽ
            //public int cone_rc_cor;
            ////ｵﾌｾｯﾄ較正ｽﾃｰﾀｽ
            //public int offset_cor;
            ////ｹﾞｲﾝ較正ｽﾃｰﾀｽ
            //public int gain_cor;
            ////X線管1用寸法較正ｽﾃｰﾀｽ
            //public int distance0_cor;
            ////X線管2用寸法較正ｽﾃｰﾀｽ
            //public int distance1_cor;
            ////ｽｷｬﾝ位置登録ｽﾃｰﾀｽ
            //public int scanpos_cor;
            ////寸法較正ｽﾃｰﾀｽｲﾝﾋﾋﾞｯﾄ
            //public int distance_cor_inh;
            ////ｽｷｬﾝ位置登録ｲﾝﾋﾋﾞｯﾄ
            //public int scanpos_cor_inh;
            ////幾何歪較正I.I.視野
            //public int ver_iifield;
            ////幾何歪較正X線管番号
            //public int ver_mt;
            ////回転中心較正管電圧 (kV)
            //public float rc_kv;
            ////回転中心較正昇降位置 (mm)
            //public float rc_udab_pos;
            ////回転中心較正I.I.視野
            //public int rc_iifield;
            ////回転中心較正X線管番号
            //public int rc_mt;
            ////ｹﾞｲﾝ較正I.I.視野
            //public int gain_iifield;
            ////ｹﾞｲﾝ較正管電圧 (kV)
            //public float gain_kv;
            ////ｹﾞｲﾝ較正管電流 (mA)
            //public float gain_ma;
            ////ｹﾞｲﾝ較正X線管番号
            //public int gain_mt;
            ////ｹﾞｲﾝ較正ﾌｨﾙﾀ
            //public int gain_filter;
            ////ｵﾌｾｯﾄ較正年月日
            //public int off_date;
            ////寸法較正I.I.視野
            //public int dc_iifield;
            ////寸法較正X線管番号
            //public int dc_mt;
            ////ｽｷｬﾝ位置登録I.I.視野
            //public int sp_iifield;
            ////ｽｷｬﾝ位置登録X線管番号
            //public int sp_mt;
            ////微調X軸準備完了
            //public int xstg_ready;
            ////微調X軸動作中
            //public int xstg_busy;
            ////微調X軸エラー
            //public int xstg_error;
            ////微調X軸リミット
            //public int xstg_limit;
            ////微調X軸現在位置[mm]
            //public float xstg_pos;
            ////微調Y軸準備完了
            //public int ystg_ready;
            ////微調Y軸動作中
            //public int ystg_busy;
            ////微調Y軸エラー
            //public int ystg_error;
            ////微調Y軸リミット
            //public int ystg_limit;
            ////微調Y軸現在位置[mm]
            //public float ystg_pos;
            ////自動ﾃｰﾌﾞﾙ移動ｽﾃｰﾀｽ  0:ﾊｰﾌ/ﾌﾙ移動完了  1:ｵﾌｾｯﾄ移動完了  2:移動未完了  3:移動不可  4:移動中
            //public int table_auto_move;
            ////ﾊｰﾌ/ﾌﾙ用X座標
            //public float auto_move_xf;
            ////ﾊｰﾌ/ﾌﾙ用Y座標
            //public float auto_move_yf;
            ////ｵﾌｾｯﾄ用X座標
            //public float auto_move_xo;
            ////ｵﾌｾｯﾄ用Y座標
            //public float auto_move_yo;
            ////I.I 視野
            //public int iifield;
            ////回転中心校正実行時のビニングモード
            //public int rc_bin;
            ////幾何歪校正実行時のビニングモード
            //public int ver_bin;
            ////スキャン位置校正実行時のビニングモード
            //public int sp_bin;
            ////ゲイン校正実行時のビニングモード
            //public int gain_bin;
            ////オフセット校正実行時のビニングモード
            //public int off_bin;
            ////寸法校正実行時のビニングモード
            //public int dc_bin;
            ////寸法校正回転選択ステータス     0:テーブル 1:Ｘ線管
            //public int dc_rs;
            ////回転中心校正回転選択ステータス 0:テーブル 1:Ｘ線管
            //public int rc_rs;

            public int emergency;   // EMERGNCY信号状態
            public int mecha_ready;   // ﾒｶ準備完了信号状態
            public int mecha_busy;   // ﾒｶ動作中信号状態
            public int cpu_ready;   // CPU処理準備完了信号状態
            public int scan;   // ｽｷｬﾝ･ｽｷｬﾉ動作中信号状態
            public int ups_power;   // UPS電源ON/OFF状態
            public int x_ready;   // X線制御準備完了信号状態
            public int test_scan;   // ﾃｽﾄｽｷｬﾝ情報
            public int m_error;   // 機構系の状態。ﾒｶｴﾗｰ情報
            public int rot_mode;   // 回転ﾓｰﾄﾞ
            public int ud_ready;   // 昇降準備完了情報
            public int ud_busy;   // 昇降動作中情報
            public int ud_error;   // 昇降ｴﾗｰ情報
            public int ud_limit;   // 昇降ﾘﾐｯﾀ限情報
            public float ud_pos;   // 昇降位置。相対座標
            public float udab_pos;   // 昇降位置。絶対座標
            public int tr_reset;   // ﾄﾗﾊﾞｰｽ準備完了情報
            public int tr_busy;   // ﾄﾗﾊﾞｰｽ動作情報
            public int tr_error;   // ﾄﾗﾊﾞｰｽｴﾗｰ情報
            public int area_size;   // ｴﾘｱｻｲｽﾞ情報
            public int tr_resets;   // ﾄﾗﾊﾞｰｽ軸のSﾘｾｯﾄ位置
            public int tr_resetm;   // ﾄﾗﾊﾞｰｽ軸のMﾘｾｯﾄ位置
            public int tr_resetl;   // ﾄﾗﾊﾞｰｽ軸のLﾘｾｯﾄ位置
            public int tr_limit;   // ﾄﾗﾊﾞｰｽﾘﾐｯﾀ限情報
            public int tr_pos;   // ﾄﾗﾊﾞｰｽ位置情報
            public int rot_ready;   // 回転準備完了情報
            public int rot_busy;   // 回転動作中情報
            public int rot_error;   // 回転ｴﾗｰ情報
            public int rot_reset;   // 回転軸の0ﾘｾｯﾄ位置
            public int rot_180;   // 回転軸の180ﾘｾｯﾄ位置
            public int rot_pos;   // 回転位置
            public int cnt_ready; //   芯位置準備完了情報
            public int cnt_busy;   // 芯位置動作中情報
            public int cnt_abnormal;   // 芯位置異常
            public int cnt_error;   // 芯位置ｻｰﾎﾞ異常
            public int cnt_limit;   // 芯位置ﾘﾐｯﾀ限異常
            public int cnt_pos;   // 芯位置ﾎﾟｼﾞｼｮﾝ
            public int filter_ready;   // ｼｬｯﾀ･ﾌｨﾙﾀ準備完了情報
            public int filter_busy;   // ｼｬｯﾀ･ﾌｨﾙﾀ起動情報
            public int filter_error;   // ｼｬｯﾀ･ﾌｨﾙﾀｴﾗｰ情報
            public int filter_energy;   // ﾌｨﾙﾀ-ｴﾈﾙｷﾞ-
            public int filter_limit;   // ｼｬｯﾀ･ﾌｨﾙﾀﾘﾐｯﾀ限情報
            public int shutter;   // X線ｼｬｯﾀ-情報
            public int colimeter_ready;   // ｺﾘﾒｰﾀ準備完了情報
            public int colimeter_busy;   // ｺﾘﾒｰﾀ起動情報
            public int colimeter_error;   // ｺﾘﾒｰﾀｴﾗｰ情報
            public int colimeter_size;   // ｺﾘﾒｰﾀｻｲｽﾞ情報
            public int colimeter_limit;   // ｺﾘﾒｰﾀﾘﾐｯﾀ制御情報
            public int tube_ready;   // 管球種準備完了情報
            public int tube_busy;   // 管球種起動情報
            public int tube_error;   // 管球種ｴﾗｰ情報
            public int tube_energy;   // 管球種
            public int tube_limit;   // 管球種ﾘﾐｯﾀ制御情報
            public int shift_ready;   // ｼﾌﾄ準備完了
            public int shift_busy;   // ｼﾌﾄ動作中
            public int shift_error;   // ｼﾌﾄｴﾗｰ
            public int shift_size;   // ｼﾌﾄｻｲｽﾞ
            public int shift_limit;   // ｼﾌﾄﾘﾐｯﾀ限
            public int xray_ready;   // X線準備完了
            public int xray_on;   // X線 ON/OFF状態
            public int xray_avl;   // X線ｱﾍﾞｲﾗﾌﾞﾙ状態
            public int xray_agng;   // X線ｳｫｰﾑｱｯﾌﾟ状態
            public int warmup;   // ｳｫｰﾑｱｯﾌﾟ状態
            public int warmup_days;   // ｳｫｰﾑｱｯﾌﾟｽｹｼﾞｭｰﾙ
            public int xray_ovset;   // X線過設定
            public int xray_slferr;   // X線制御機ｴﾗｰ
            public int xray_tmp;   // X線発生装置温度異常
            public int xray_error;   // X線系の状態。X線ｴﾗｰ情報
            public int interlock_ready;   // ｲﾝﾀｰﾛｯｸ準備完了
            public int interlock_open1;   // 保守扉+試料扉
            public int interlock_open2;   // X線ｼｰﾙﾄﾞ
            public int interlock_das;   // DAS異常
            public int interlock_dwc;   // 冷却水供給装置異常
            public int power_err;   // DAS電源ｴﾗｰ
            public int therm_err;   // DAS温度異常
            public int ipr;   // ﾌﾟﾘﾝﾀ動作中
            public int duc;   // ﾌﾟﾘﾝﾀ接続断
            public int pdo;   // ﾌﾟﾘﾝﾀ用紙切れ
            public int fapc_ready;   // FAPC準備完了情報
            public int fapc_error;   // その他のFAPC異常
            public int slow_position;   // ｽｷｬﾝ･ｽｷｬﾉ中に減速域に入った
            public int data_error;   // ﾃﾞｰﾀ収集異常
            public int x_ray_auto;   // X線ﾘﾓｰﾄ
            public int ram_data_error;   // RAMﾃﾞｰﾀ異常
            public int line_mode;   // 回線ﾓｰﾄﾞ（切断か接続か）
            public int op_panel;   // ｵﾍﾟﾊﾟﾈの状態
            public int focus_x_ready;   // Ｘ線焦点切替Ｘ方向準備完了
            public int focus_x_busy;   // Ｘ線焦点切替Ｘ方向起動情報
            public int focus_x_error;   // Ｘ線焦点切替Ｘ方向ｴﾗｰ情報
            public int focus_x_energy;   // Ｘ線焦点切替Ｘ方向焦点位置
            public int focus_x_limit;   // Ｘ線焦点切替Ｘ方向ﾘﾐｯﾀ限情報
            public int focus_y_ready;   // Ｘ線焦点切替Ｙ方向準備完了
            public int focus_y_busy;   // Ｘ線焦点切替Ｙ方向起動情報
            public int focus_y_error;   // Ｘ線焦点切替Ｙ方向ｴﾗｰ情報
            public int focus_y_energy;   // Ｘ線焦点切替Ｙ方向焦点位置
            public int focus_y_limit;   // Ｘ線焦点切替Ｙ方向ﾘﾐｯﾀ限情報
            public int focus_z_ready;   // Ｘ線焦点切替Ｚ方向準備完了
            public int focus_z_busy;   // Ｘ線焦点切替Ｚ方向起動情報
            public int focus_z_error;   // Ｘ線焦点切替Ｚ方向ｴﾗｰ情報
            public int focus_z_energy;   // Ｘ線焦点切替Ｚ方向焦点位置
            public int focus_z_limit;   // Ｘ線焦点切替Ｚ方向ﾘﾐｯﾀ限
            public int filter;   // ｼｬｯﾀﾌｨﾙﾀ選択情報
            public int rot_speed_const;   // 回転速度情報 REV2.00
            public int phm_ready;   // 較正用ファントム機構準備完了情報
            public int phm_busy;   // 較正用ファントム機構動作中情報
            public int phm_error;   // 較正用ファントム機構エラー情報
            public int phm_limit;   // 較正用ファントム機構リミット限情報
            public int phm_onoff;   // 較正用ファントム機構位置情報
            public int vertical_cor;   // 幾何歪較正ステータス
            public int normal_rc_cor;   // ﾉｰﾏﾙｽｷｬﾝ用回転中心較正ｽﾃｰﾀｽ
            public int cone_rc_cor;   // ｺｰﾝﾋﾞｰﾑｽｷｬﾝ用回転中心較正ｽﾃｰﾀｽ
            public int offset_cor;   // ｵﾌｾｯﾄ較正ｽﾃｰﾀｽ
            public int gain_cor;   // ｹﾞｲﾝ較正ｽﾃｰﾀｽ
            public int distance0_cor;   // X線管1用寸法較正ｽﾃｰﾀｽ
            public int distance1_cor;   // X線管2用寸法較正ｽﾃｰﾀｽ
            public int scanpos_cor;   // ｽｷｬﾝ位置登録ｽﾃｰﾀｽ
            public int distance_cor_inh;   // 寸法較正ｽﾃｰﾀｽｲﾝﾋﾋﾞｯﾄ
            public int scanpos_cor_inh;   // ｽｷｬﾝ位置登録ｲﾝﾋﾋﾞｯﾄ
            public int ver_iifield;   // 幾何歪較正I.I.視野
            public int ver_mt;   // 幾何歪較正X線管番号
            public float rc_kv;   // 回転中心較正管電圧 (kV)
            public float rc_udab_pos;   // 回転中心較正昇降位置 (mm)
            public int rc_iifield;   // 回転中心較正I.I.視野
            public int rc_mt;   // 回転中心較正X線管番号
            public int gain_iifield;   // ｹﾞｲﾝ較正I.I.視野
            public float gain_kv;   // ｹﾞｲﾝ較正管電圧 (kV)
            public float gain_ma;   // ｹﾞｲﾝ較正管電流 (mA)
            public int gain_mt;   // ｹﾞｲﾝ較正X線管番号
            public int gain_filter;   // ｹﾞｲﾝ較正ﾌｨﾙﾀ
            public int off_date;   // ｵﾌｾｯﾄ較正年月日
            public int dc_iifield;   // 寸法較正I.I.視野
            public int dc_mt;   // 寸法較正X線管番号
            public int sp_iifield;   // ｽｷｬﾝ位置登録I.I.視野
            public int sp_mt;   // ｽｷｬﾝ位置登録X線管番号
            public int xstg_ready;   // 微調X軸準備完了
            public int xstg_busy;   // 微調X軸動作中
            public int xstg_error;   // 微調X軸エラー
            public int xstg_limit;   // 微調X軸リミット
            public float xstg_pos;   // 微調X軸現在位置[mm]
            public int ystg_ready;   // 微調Y軸準備完了
            public int ystg_busy;   // 微調Y軸動作中
            public int ystg_error;   // 微調Y軸エラー
            public int ystg_limit;   // 微調Y軸リミット
            public float ystg_pos;   // 微調Y軸現在位置[mm]
            public int table_auto_move;   // 自動ﾃｰﾌﾞﾙ移動ｽﾃｰﾀｽ  0:ﾊｰﾌ/ﾌﾙ移動完了  1:ｵﾌｾｯﾄ移動完了  2:移動未完了  3:移動不可  4:移動中
            public float auto_move_xf;   // ﾊｰﾌ/ﾌﾙ用X座標
            public float auto_move_yf;   // ﾊｰﾌ/ﾌﾙ用Y座標
            public float auto_move_xo;   // ｵﾌｾｯﾄ用X座標
            public float auto_move_yo;   // ｵﾌｾｯﾄ用Y座標
            public int iifield;   // I.I 視野
            public int rc_bin;   // 回転中心校正実行時のビニングモード
            public int ver_bin;   // 幾何歪校正実行時のビニングモード
            public int sp_bin;   // スキャン位置校正実行時のビニングモード
            public int gain_bin;   // ゲイン校正実行時のビニングモード
            public int off_bin;   // オフセット校正実行時のビニングモード
            public int dc_bin;   // 寸法校正実行時のビニングモード
            public int dc_rs;   // 寸法校正回転選択ステータス     0:テーブル 1:Ｘ線管
            public int rc_rs;   // 回転中心校正回転選択ステータス 0:テーブル 1:Ｘ線管
            public int gain_date;   // ゲイン較正年月日   'v12.01追加 by 間々田 2006/12/04
            public float table_x_pos;   // 試料テーブル（光軸と垂直方向)座標[mm]　v15.0追加　by　長野　2009/07/16
            public int gain_fpd_gain;   // ゲイン校正時のFPDゲイン　      'v16.20/v17.00追加 byやまおか 2010/02/17
            public int gain_fpd_integ;   // ゲイン校正時のFPD積分時間　    'v16.20/v17.00追加 byやまおか 2010/02/17
            public int off_fpd_gain;   // オフセット校正時のFPDゲイン　  'v16.20/v17.00追加 byやまおか 2010/02/17
            public int off_fpd_integ;   // オフセット校正時のFPD積分時間  'v16.20/v17.00追加 byやまおか 2010/02/17
            public int dc_date;   // 寸法較正年月日                 'v16.20/v17.00追加 byやまおか 2010/03/02
            public int dc_time;   // 寸法較正時間                   'v16.20/v17.00追加 byやまおか 2010/03/02
            public int off_time;   // オフセット校正時間             'v16.20/v17.00追加 byやまおか 2010/03/04
            public int gain_time;   // ゲイン校正時間                 'v16.20/v17.00追加 byやまおか 2010/03/04
            public int sp_date;   // スキャン位置校正年月日         'v17.02追加 byやまおか 2010/07/08
            public int sp_time;   // スキャン位置校正時間           'v17.02追加 byやまおか 2010/07/08
            public int coolant_err;   // 水流量ｲﾝﾀｰﾛｯｸ 1:異常 0:正常    'v17.10追加 byやまおか 2010/08/25
            public int ud_u_limit;   // 昇降上限リミット情報           'v17.20追加 by長野     2010/09/20
            public int ud_l_limit;   // 昇降下限リミット情報           'v17.20追加 by長野     2010/09/20
            public int xstg_u_limit;   // Ｘ軸上限動作限                 'v17.47/v17.53 追加 by 間々田 2011-03-08
            public int xstg_l_limit;   // Ｘ軸下限動作限                 'v17.47/v17.53 追加 by 間々田 2011-03-08
            public int ystg_u_limit;   // Ｙ軸上限動作限                 'v17.47/v17.53 追加 by 間々田 2011-03-08
            public int ystg_l_limit;   // Ｙ軸下限動作限                 'v17.47/v17.53 追加 by 間々田 2011-03-08
            public int oc_trip;   // オイルクーラー電源トリップ     'v18.00/v17.53 追加 by 間々田 2011-03-09
            public int ups_power200;   // UPS200Vバックアップ商用電源異常'v18.00/v17.53 追加 by 間々田 2011-03-09

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
        }

        //mecainf（コモン）構造体変数
        public static mecainfType mecainf;
        
        //mecainf（コモン）取得関数
        [DllImport("comlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetMecainf(ref mecainfType theMecainf);

#endif

    }
}
