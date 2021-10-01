using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Runtime.InteropServices;

using CT30K;
using CT30K.Common;
using CTAPI;

namespace CommonViewer
{
    public partial class frmInformation : Form
    {
        private static frmInformation _Instance = null;

        private System.Windows.Forms.Label[] lbls;
        private System.Windows.Forms.TextBox[] txts;

        private string TargetFileName;

        ImageInfo theImageInfo = new ImageInfo();

        public frmInformation()
        {
            InitializeComponent();
        }

        public static frmInformation Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmInformation();
                }

                return _Instance;
            }
        }

        private void frmInformation_Load(object sender, EventArgs e)
        {
            theImageInfo.Data.Initialize();

            Functions.ReadStructureDummy("dummy",ref theImageInfo.Data);
   
            int lblLocateBaseX = 12;
            int lblLocateBaseY = 35;
            int txtLocateBsaeX = 124;
            int txtLocateBsaeY = 31;

            int lblLocateX = lblLocateBaseX;
            int lblLocateY = lblLocateBaseY;
            int txtLocateX = txtLocateBsaeX;
            int txtLocateY = txtLocateBsaeY;

            int i = 0;
            int j = 0;

            this.lbls = new System.Windows.Forms.Label[221];
            this.txts = new System.Windows.Forms.TextBox[221];

            for (i = 1; i < lbls.Length; i++)
            {
                this.lbls[i] = new System.Windows.Forms.Label();
                this.lbls[i].Name = "lblInfo" + i.ToString();
                this.lbls[i].Text = i.ToString();
                this.lbls[i].Size = new Size(90, 12);
                //this.lbls[i].Location = new Point(12, 35 + 19 * (i - 1));
                this.lbls[i].Visible = true;

                this.txts[i] = new System.Windows.Forms.TextBox();
                this.txts[i].Name = "txtInfo" + i.ToString();
                this.txts[i].Text = i.ToString();
                this.txts[i].Size = new Size(105, 19);
                //this.txts[i].Location = new Point(124, 31 + 19 * (i - 1));
                this.txts[i].Visible = true;
                this.txts[i].TextChanged += new EventHandler(txts_TextChagend);

                if (i % 40 == 0 && i != 0)
                {
                    lblLocateX = lblLocateX + 250;
                    txtLocateX = txtLocateX + 250;
                    lblLocateY = lblLocateBaseY;
                    txtLocateY = txtLocateBsaeY;
                    this.lbls[i].Location = new Point(lblLocateX, lblLocateY);
                    this.txts[i].Location = new Point(txtLocateX, txtLocateY);
                    j = 1;
                }
                else
                {
                    this.lbls[i].Location = new Point(lblLocateX, lblLocateY + 19 * j);
                    this.txts[i].Location = new Point(txtLocateX, txtLocateY + 19 * j);
                    j++;
                }
                lbls[i].Text = Properties.Resources.ResourceManager.GetString("str" + (1000 + i).ToString());
            }
            this.Controls.AddRange(this.lbls);
            this.Controls.AddRange(this.txts);
            TargetFileName = "";

            //ImageInfo.ReadImageInfo(ref theImageInfo.Data, @"c:\ctuser\画像\test-0001");

       }
        //*******************************************************************************
        //機　　能： テキストボックス内で変更があった場合の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void txts_TextChagend(object sender, EventArgs e)
        {
            int Index = Array.IndexOf(txts,sender);

            mnuFileSave.Enabled = true;

        }

        //*******************************************************************************
        //機　　能： 「ファイル」メニューの「開く...」を選択した
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void mnuFileOpen_Click(object sender, EventArgs e)
        {

            if (!NextOperationIsOK())
                return;

            CommonDialog1.FileName = "";
            CommonDialog1.Title = "付帯情報を開く";
            CommonDialog1.Filter = "付帯情報ファイル(*.inf)|*.inf";
            CommonDialog1.ShowReadOnly = false;
            CommonDialog1.Multiselect = false;

            DialogResult result = CommonDialog1.ShowDialog();

            if (result == DialogResult.Cancel)
            {
                return;
            }

            if (LoadImageInfo(CommonDialog1.FileName))
            {
                TargetFileName = CommonDialog1.FileName;
                mnuFileSave.Enabled = false;
            }

            return;

        }
        //*******************************************************************************
        //機　　能： 指定されたファイルの画像情報をテキストボックスにロードする
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private bool LoadImageInfo(string FileName)
        {
            bool functionReturnValue = false;

            string FileNameRemoveInf;

            FileNameRemoveInf = modLibrary.RemoveExtension(FileName, ".inf");

            //付帯情報ファイルの読み込み
            if (!ImageInfo.ReadImageInfo(ref theImageInfo.Data, FileNameRemoveInf))
            {
                return functionReturnValue;
            }

            //フォームのキャプションにファイル名を表示
            this.Text = this.Tag + " " + FileName;

            var _with3 = theImageInfo.Data;
            txts[1].Text = _with3.system_name.GetString();
            //String * 16  'システム名                               '1
            txts[2].Text = _with3.version.GetString();
            //String * 6   'ソフトバージョン情報                     '2
            txts[3].Text = _with3.ct_gentype.GetString();
            //String * 2   'CTデータ収集方式（世代）                 '3
            txts[4].Text = _with3.sliceplan_der.GetString();
            //String * 256 'スライスプランディレクトリ名             '4
            txts[5].Text = _with3.slice_plan.GetString();
            //String * 256 'スライスプラン名                         '5
            txts[6].Text = _with3.d_sileno.GetString();
            //String * 4   'マルチスキャン時のスライス番号           '6
            txts[7].Text = _with3.d_date.GetString();
            //String * 10  '撮影年月日                               '7
            txts[8].Text = _with3.start_time.GetString();
            //String * 10  'スキャン時刻                             '8
            txts[9].Text = _with3.workshop.GetString();
            //String * 16  '事業所名                                 '9
            txts[10].Text = _with3.comment.GetString();
            //String * 256 'コメント・エリア                         '10
            txts[11].Text = _with3.d_rawsts.GetString();
            //String * 2   '生データ・ステータス                     '11
            txts[12].Text = _with3.d_recokind.GetString();
            //String * 6   '付帯情報データ種類                       '12
            txts[13].Text = _with3.full_mode.GetString();
            //String * 6   'スキャンモード                           '13
            txts[14].Text = _with3.matsiz.GetString();
            //String * 4   '画像マトリックサイズ                     '14
            txts[15].Text = _with3.scan_mode.GetString();
            //String * 6   'スキャン・モード                         '15
            txts[16].Text = _with3.speed_mode.GetString();
            //String * 6   'スキャン速度モード                       '16
            txts[17].Text = _with3.scan_speed.GetString();
            //String * 8   'スキャン速度値                           '17
            txts[18].Text = _with3.scan_time.GetString();
            //String * 4   'スキャン時間                             '18
            txts[19].Text = _with3.scan_area.GetString();
            //String * 2   '撮影領域名                               '19
            txts[20].Text = _with3.area.GetString();
            //String * 8   'スライスエリアの番号                     '20
            txts[21].Text = _with3.slice_wid.GetString();
            //String * 2   'スライス幅番号                           '21
            txts[22].Text = _with3.width.GetString();
            //String * 8   'スライス幅値                             '22
            txts[23].Text = _with3.det_ap_num.GetString();
            //String * 2   '開口番号                                 '23
            txts[24].Text = _with3.focus.GetString();
            //String * 4   'Ｘ線焦点                                 '24
            txts[25].Text = _with3.tube.GetString();
            //String * 16  'Ｘ線管球種（形式）                       '25
            txts[26].Text = _with3.energy.GetString();
            //String * 2   'Ｘ線条件番号                             '26
            txts[27].Text = _with3.volt.GetString();
            //String * 8   '管電圧                                   '27
            txts[28].Text = _with3.anpere.GetString();
            //String * 8   '管電流                                   '28
            txts[29].Text = _with3.table_pos.GetString();
            //String * 8   'テーブル位置（相対座標）                 '29
            txts[30].Text = _with3.d_tablepos.GetString();
            //String * 8   'テーブル位置（絶対座標）                 '30
            txts[31].Text = _with3.tilt_angle.GetString();
            //String * 6   'データ収集傾斜角度                       '31
            txts[32].Text = _with3.fc.GetString();
            //String * 8   'フィルター関数                           '32
            txts[33].Text = Convert.ToString(_with3.bhc);
            //Long         'コーンビーム識別値                       '33
            txts[34].Text = _with3.bhc_dir.GetString();
            //String * 256 'コーンビームCT生データディレクトリ名     '34
            txts[35].Text = _with3.bhc_name.GetString();
            //String * 256 'コーンビームCT生データファイル名         '35
            txts[36].Text = _with3.zoom_dir.GetString();
            //String * 256 'ズームファイルディレクトリ名             '36
            txts[37].Text = _with3.zoom_name.GetString();
            //String * 256 'ズームファイル名                         '37
            txts[38].Text = Convert.ToString(_with3.zooming);
            //Long         '拡大画像の有無                           '38
            txts[39].Text = _with3.zoomx.GetString();
            //String * 4   '拡大画像のX座標                          '39
            txts[40].Text = _with3.zoomy.GetString();
            //String * 4   '拡大画像のY座標                          '40
            txts[41].Text = _with3.zoomsize.GetString();
            //String * 4   '拡大画像のサイズ                         '41
            txts[42].Text = _with3.sift_pos.GetString();
            //String * 2   'シフト位置                               '42
            txts[43].Text = _with3.scale.GetString();
            //String * 8   '再構成エリア                             '43
            txts[44].Text = _with3.roicaltable_dir.GetString();
            //String * 256 'ROI測定テーブルディレクトリ              '44
            txts[45].Text = _with3.roical_table.GetString();
            //String * 256 'ROI測定テーブル名                        '45
            txts[46].Text = _with3.pdtable_dir.GetString();
            //String * 256 'プロフィール寸法測定テーブルディレクトリ '46
            txts[47].Text = _with3.pd_table.GetString();
            //String * 256 'プロフィール寸法測定テーブル名           '47
            txts[48].Text = Convert.ToString(_with3.roical);
            //Long         'ROI CAL有無                              '48
            txts[49].Text = Convert.ToString(_with3.pd);
            //Long         'プロフィール寸法測定有無                 '49
            txts[50].Text = _with3.scano_dir.GetString();
            //String * 6   'スキャノ収集時の管球方向角度             '50
            txts[51].Text = _with3.pro_dir.GetString();
            //String * 2   '試料挿入方向                             '51
            txts[52].Text = _with3.view_dir.GetString();
            //String * 4   '試料観察方向                             '52
            txts[53].Text = _with3.pro_posdir.GetString();
            //String * 2   '試料位置方向                             '53
            txts[54].Text = _with3.scano_dispdir.GetString();
            //String * 4   'スキャノ表示方向                         '54
            txts[55].Text = _with3.pix_minval.GetString();
            //String * 6   '画素の最小値(CT値)                       '55
            txts[56].Text = _with3.pix_maxval.GetString();
            //String * 6   '画素の最大値(CT値)                       '56
            txts[57].Text = _with3.ww.GetString();
            //String * 6   'ウィンドウ幅                             '57
            txts[58].Text = _with3.wl.GetString();
            //String * 6   'ウィンドウレベル                         '58
            txts[59].Text = Convert.ToString(_with3.graphic);
            //Long         'グラフィック画像有無                     '59
            txts[60].Text = _with3.rotation.GetString();
            //String * 8   'データ収集開始位置                       '60
            txts[61].Text = Convert.ToString(_with3.trvsno);
            //Long         'スキャントラバース数                     '61
            txts[62].Text = Convert.ToString(_with3.trvspt);
            //Long         'スキャントラバースポイント               '62
            txts[63].Text = Convert.ToString(_with3.realfdd);
            //Single       'Ｘ線焦点の長さ                           '63
            txts[64].Text = Convert.ToString(_with3.dtpitch);
            //Single       'メインチャンネルピッチ                   '64
            txts[65].Text = Convert.ToString(_with3.fanangle);
            //Single       'FAN角度                                  '65
            txts[66].Text = Convert.ToString(_with3.fcd);
            //Single       '検査テーブルの中心点までの焦点           '66
            txts[67].Text = Convert.ToString(_with3.trpitch);
            //Single       'トラバースピッチ                         '67
            txts[68].Text = Convert.ToString(_with3.tbldia);
            //Single       'テーブル直径                             '68
            txts[69].Text = Convert.ToString(_with3.aprtpt);
            //Long         'アプローチポイント数                     '69
            txts[70].Text = Convert.ToString(_with3.bias);
            //Long         '画素値の調整値                           '70
            txts[71].Text = Convert.ToString(_with3.slope);
            //Single       '画素値の調整値                           '71
            txts[72].Text = Convert.ToString(_with3.mainch);
            //Long         '主検出器チャンネル数                     '72
            txts[73].Text = Convert.ToString(_with3.refch);
            //Long         '比較検出器チャンネル数                   '73
            txts[74].Text = Convert.ToString(_with3.dumych);
            //Long         'データ収集ダミーチャンネル数             '74
            txts[75].Text = Convert.ToString(_with3.datach);
            //Long         'データ収集レファレンス+メインチャンネル数'75
            txts[76].Text = Convert.ToString(_with3.allch);
            //Long         'データ収集全チャンネル数                 '76
            txts[77].Text = Convert.ToString(_with3.offsetapr);
            //Long         'オフセットデータ助走ポイントデータ数     '77
            txts[78].Text = Convert.ToString(_with3.offsetpt);
            //Long         'オフセットデータ収集ポイント数           '78
            txts[79].Text = Convert.ToString(_with3.airdtpt);
            //Long         '空気補正データ長                         '79
            txts[80].Text = Convert.ToString(_with3.clct_inf);
            //Long         'スキャンの撮影領域画素数                 '80
            txts[81].Text = Convert.ToString(_with3.scanotvs);
            //Long         'スキャノデータ収集トラバース数           '81
            txts[82].Text = Convert.ToString(_with3.scanoapr);
            //Long         'スキャノアプローチポイントデータ数       '82
            txts[83].Text = Convert.ToString(_with3.scanopt);
            //Long         'スキャノデータ収集ポイント数             '83
            txts[84].Text = Convert.ToString(_with3.scanoscl);
            //Long         'スキャノデータ収集スケールサイズ         '84
            txts[85].Text = Convert.ToString(_with3.scanoair);
            //Long         'スキャノデータ収集空気データポイント数   '85
            txts[86].Text = Convert.ToString(_with3.scanobias);
            //Long         'スキャノデータ収集バイアス値             '86
            txts[87].Text = Convert.ToString(_with3.scanoslop);
            //Single       'スキャノデータ収集DCVスロープ値          '87
            txts[88].Text = Convert.ToString(_with3.scanotblbias);
            //txts       'スキャノデータ収集テーブルバイアス値     '88
            txts[89].Text = Convert.ToString(_with3.scanoudpitch);
            //Single       'スキャノデータ収集アップダウンピッチ数   '89
            txts[90].Text = Convert.ToString(_with3.scanodpratio);
            //Single       'スキャノデータ収集表示横方向圧縮率       '90
            txts[91].Text = _with3.scano_matsiz.GetString();
            //String * 4   'スキャノ画像のマトリックスサイズ         '91
            txts[92].Text = _with3.scan_view.GetString();
            //String * 8   'ビュー数                                 '92
            txts[93].Text = _with3.integ_number.GetString();
            //String * 8   '積算枚数                                 '93
            txts[94].Text = Convert.ToString(_with3.recon_start_angle);
            //Single       '再構成開始角度(ｽﾞｰﾐﾝｸﾞ時使用)            '94
            txts[95].Text = Convert.ToString(_with3.image_bias);
            //Long         'バイアス                                 '95
            txts[96].Text = Convert.ToString(_with3.image_slope);
            //Single       'スロープ                                 '96
            txts[97].Text = Convert.ToString(_with3.image_direction);
            //Long         '画像方向                                 '97
            txts[98].Text = Convert.ToString(_with3.mcenter_channel);
            //Single       '再構成用センターチャンネル               '98
            txts[99].Text = Convert.ToString(_with3.mchannel_pitch);
            //Single       '１チャンネル当たりの角度                 '99
            txts[100].Text = Convert.ToString(_with3.mzoomx);
            //Single       'ズーミング時のROI中心点X座標 (pixel)     '100
            txts[101].Text = Convert.ToString(_with3.mzoomy);
            //Single       'ズーミング時のROI中心点Y座標 (pixel)     '101
            txts[102].Text = Convert.ToString(_with3.mzoomsize);
            //Single       'ズーミング時のROIサイズ (pixel)          '102
            txts[103].Text = Convert.ToString(_with3.mscan_area);
            //Single       'スキャンエリア (mm)                      '103
            txts[104].Text = Convert.ToString(_with3.max_mscan_area);
            //Single       '最大スキャンエリア (mm)                  '104
            txts[105].Text = Convert.ToString(_with3.fimage_bit);
            //Long         '画像取り込みビット数                     '105
            txts[106].Text = Convert.ToString(_with3.fid);
            //Single       'X線焦点からIIまでの距離                  '106
            txts[107].Text = Convert.ToString(_with3.recon_mask);
            //Long         '再構成形状                               '107
            txts[108].Text = Convert.ToString(_with3.n1);
            //Long         '有効データ開始チャンネル                 '108
            txts[109].Text = Convert.ToString(_with3.n2);
            //Long         '有効データ終了チャンネル                 '109
            txts[110].Text = Convert.ToString(_with3.mc);
            //Long         '縦中心チャンネル                         '110
            txts[111].Text = Convert.ToString(_with3.theta0);
            //Single       '有効ファン角                             '111
            txts[112].Text = Convert.ToString(_with3.theta01);
            //Single       '有効データ包含ファン角1 (radian)         '112
            txts[113].Text = Convert.ToString(_with3.theta02);
            //Single       '有効データ包含ファン角2 (radian)         '113
            txts[114].Text = Convert.ToString(_with3.thetaoff);
            //Single       '有効データ包含ファン角                   '114
            txts[115].Text = Convert.ToString(_with3.nc);
            //Single       '回転中心チャンネル                       '115
            txts[116].Text = Convert.ToString(_with3.delta_theta);
            //Single       'n方向データピッチ (radian)               '116
            txts[117].Text = Convert.ToString(_with3.dpm);
            //Single       'm方向データピッチ (mm)                   '117
            txts[118].Text = Convert.ToString(_with3.n0);
            //Single       'n方向画像中心対応チャンネル              '118
            txts[119].Text = Convert.ToString(_with3.m0);
            //Single       'm方向画像中心対応チャンネル              '119
            txts[120].Text = Convert.ToString(_with3.alpha);
            //Single       'オーバーラップ角度 (radian)              '120
            txts[121].Text = Convert.ToString(_with3.zp);
            //Single       'ヘリカルピッチ (mm)                      '121
            txts[122].Text = Convert.ToString(_with3.zs0);
            //Single       '初期1枚目のスライス位置                  '122
            txts[123].Text = Convert.ToString(_with3.iud);
            //Long         '上昇下降識別値                           '123
            txts[124].Text = Convert.ToString(_with3.ioff);
            //Long         'オフセット識別値                         '124
            txts[125].Text = Convert.ToString(_with3.k);
            //Long         'スライス枚数                             '125
            txts[126].Text = Convert.ToString(_with3.delta_z);
            //Single       '軸方向Boxelサイズ (mm)                   '126
            txts[127].Text = Convert.ToString(_with3.ze0);
            //Single       '初期K枚目スライスのZ位置 (mm)            '127
            txts[128].Text = Convert.ToString(_with3.inh);
            //Long         'ヘリカルモード                           '128
            txts[129].Text = Convert.ToString(_with3.md);
            //Long         '縦チャンネル数                           '129
            txts[130].Text = Convert.ToString(_with3.b1);
            //Single       'コーンビーム用幾何歪パラメータB1         '130
            txts[131].Text = Convert.ToString(_with3.scan_posi_a);
            //Single       'スキャン位置の傾き                       '131
            txts[132].Text = Convert.ToString(_with3.multi_tube);
            //Long         '複数Ｘ線管                               '132
            txts[133].Text = Convert.ToString(_with3.z0);
            //Single       'コーンビームスキャン時のテーブル位置     '133
            txts[134].Text = Convert.ToString(_with3.acq_view);
            //Long         'データ収集ビュー数                       '134
            txts[135].Text = Convert.ToString(_with3.fid_offset);
            //Single       'FIDオフセット                            '135
            txts[136].Text = Convert.ToString(_with3.fcd_offset);
            //Single       'FCDオフセット                            '136
            txts[137].Text = Convert.ToString(_with3.a1);
            //Single       '幾何歪み補正パラメータA1（1/mm）         '137
            txts[138].Text = Convert.ToString(_with3.fs);
            //Single       '焦点サイズ（mm）                         '138
            txts[139].Text = Convert.ToString(_with3.x0);
            //Single       '骨塩等価物質密度（mg/cm3）               '139
            txts[140].Text = Convert.ToString(_with3.x1);
            //Single       '骨塩等価物質密度（mg/cm3）               '140
            txts[141].Text = Convert.ToString(_with3.x2);
            //Single       '骨塩等価物質密度（mg/cm3）               '141
            txts[142].Text = Convert.ToString(_with3.x3);
            //Single       '骨塩等価物質密度（mg/cm3）               '142
            txts[143].Text = Convert.ToString(_with3.x4);
            //Single       '骨塩等価物質密度（mg/cm3）               '143
            txts[144].Text = Convert.ToString(_with3.instance_num);
            //Long         'インスタンス番号                         '144
            txts[145].Text = _with3.iifield.GetString();
            //String * 10  'I.I.視野                                 '145
            txts[146].Text = _with3.filter.GetString();
            //String * 10  'フィルター                               '146
            txts[147].Text = Convert.ToString(_with3.study_id);
            //Long         '検査ID                                   '147
            txts[148].Text = Convert.ToString(_with3.series_num);
            //Long         'シリーズ番号                             '148
            txts[149].Text = Convert.ToString(_with3.acq_num);
            //Long         '収集番号                                 '149
            txts[150].Text = _with3.instance_uid.GetString();
            //String * 64  'インスタンスUID                          '150
            txts[151].Text = Convert.ToString(_with3.frame_rate);
            //Single       'フレームレート                           '151
            txts[152].Text = Convert.ToString(_with3.scan_start_angle);
            //Single       '                                         '152
            txts[153].Text = Convert.ToString(_with3.detector);
            //Long         '検出器 0:X線II　1:FPD                    '153
            txts[154].Text = Convert.ToString(_with3.data_mode);
            //Long         'データモード                             '154
            txts[155].Text = Convert.ToString(_with3.cone_image_mode);
            //Long         'コーンビーム再構成時の画質               '155
            txts[156].Text = Convert.ToString(_with3.fine_table_x);
            //Long         '微調テーブルＸ軸の有無                   '156
            txts[157].Text = Convert.ToString(_with3.ftable_x_pos);
            //Single       '微調テーブルＸ軸の座標（mm）             '157
            txts[158].Text = Convert.ToString(_with3.fine_table_y);
            //Long         '微調テーブルＹ軸の有無                   '158
            txts[159].Text = Convert.ToString(_with3.ftable_y_pos);
            //Single       '微調テーブルＹ軸の座標（mm）             '159
            txts[160].Text = Convert.ToString(_with3.rotate_select);
            //Long         '回転選択結果 0:テーブル回転 1:Ｘ線管回転 '160
            txts[161].Text = Convert.ToString(_with3.c_cw_ccw);
            //Long         '回転方向                                 '161
            txts[162].Text = Convert.ToString(_with3.kv);
            //Long         'ビニング係数(v_mag/h_mag)                '162
            txts[163].Text = Convert.ToString(_with3.mbhc_AirLogValue);
            //Single       'ログ変換後のエアーの値                   '163
            txts[164].Text = Convert.ToString(_with3.fpd_gain_f);
            //Single       'FPDゲイン 表示用(pF)                     '164
            txts[165].Text = Convert.ToString(_with3.fpd_integ_f);
            //Single       'FPD積分時間 表示用(ms)                   '165
            txts[166].Text = Convert.ToString(_with3.xfilter);
            //Long         'X線フィルタ インデックス                 '166
            txts[167].Text = _with3.xfilter_c.GetString();
            //String       'X線フィルタ                              '167
            txts[168].Text = Convert.ToString(_with3.xfocus);
            //Long         'X線焦点 インデックス                     '168
            txts[169].Text = _with3.xfocus_c.GetString();
            //String       'X線焦点                                  '169
            txts[170].Text = Convert.ToString(_with3.largetRotTable);
            //Long         '回転大テーブルを装着してスキャンしたか   '170
            txts[171].Text = Convert.ToString(_with3.table_rotation);
            //Long         '試料テーブル回転 0:ステップ,1:連続       '171
            txts[172].Text = Convert.ToString(_with3.auto_centering);
            //Long         'ｽｷｬﾝｽﾀｰﾄ時のｵｰﾄｾﾝﾀﾘﾝｸﾞ(0:無,1:有)        '172
            txts[173].Text = Convert.ToString(_with3.mscano_area);
            //Single        'スキャノエリア                          '173
            txts[174].Text = Convert.ToString(_with3.mscano_mdtpitch);
            //Single        'スキャノデータピッチ                    '174
            txts[175].Text = _with3.mscano_width.GetString();
            //String*8      'スキャノ厚                              '175
            txts[176].Text = Convert.ToString(_with3.mscanopt);
            //Long          'スキャノポイント                        '176
            txts[177].Text = Convert.ToString(_with3.mscanoscl);
            //Single        'スキャノスケール                        '177
            txts[178].Text = Convert.ToString(_with3.mscano_udpitch);
            //Single        'スキャノ昇降ピッチ                      '178
            txts[179].Text = Convert.ToString(_with3.mscano_bias);
            //Long          'スキャノ積算枚数                        '179
            txts[180].Text = Convert.ToString(_with3.mscano_slope);
            //Single        'スキャノスロープ                        '180
            txts[181].Text = Convert.ToString(_with3.mscano_dp_ratio);
            //Single        'スキャノ圧縮比                          '181
            txts[182].Text = Convert.ToString(_with3.numOfAdjCenterCh);
            //Single        '回転中心調整ch(ch)                      '182

            //Rev24.00 by長野 //Rev24.00までで追加していなかったパラメータも全部追加 --->
            txts[183].Text = Convert.ToString(_with3.scan_fcdMecha); 
            // Single       'FCD値(従来値)                                      '183 
            txts[184].Text = Convert.ToString(_with3.scan_fcdLinear);
            // Single       'FCD値(リニアスケール値)                            '184 
            txts[185].Text = Convert.ToString(_with3.scan_fddMecha);
            // Single       'FDD(従来値)                                        '185                                         
            txts[186].Text = Convert.ToString(_with3.scan_fddLinear);
            // Single       'FDD(リニアスケール値)                              '186
            txts[187].Text = Convert.ToString(_with3.scan_table_x_posMecha);
            // Single       'テーブルY軸(光軸と垂直方向)(従来値)                '187
            txts[188].Text = Convert.ToString(_with3.scan_table_x_posLinear);
            // Single       'テーブルY軸(光軸と巣直方向)(リニアスケール値)      '188
            txts[189].Text = Convert.ToString(_with3.scan_udab_pos);
            // Single       'テーブル昇降(従来値)                               '189
            txts[190].Text = Convert.ToString(_with3.scan_ud_linear_pos);
            // Single       'テーブル昇降(リニアスケール値)                     '190
            txts[191].Text = Convert.ToString(_with3.w_scan);
            // Long         'Wスキャン有無                                      '191
            txts[192].Text = Convert.ToString(_with3.mbhc_phantomless);
            // Long         'ファントムレスBHC有無                              '192
            txts[193].Text = Convert.ToString(_with3.mbhc_phantomless_colli_on);
            // Long         'ファントムレスBHC実行時コリメータ有無              '193
            txts[194].Text = _with3.mbhc_phantomless_c.GetString();
            // String       'ファントムレスBHC材質                              '194
            txts[195].Text = Convert.ToString(_with3.mbhc_phantomless_para[0]) + "," + Convert.ToString(_with3.mbhc_phantomless_para[1]) + "," + Convert.ToString(_with3.mbhc_phantomless_para[2]);
            // String       'ファントムレスBHCパラメータ                        '195
            //戻り値セット
            functionReturnValue = true;
            return functionReturnValue;

        }

        //*******************************************************************************
        //機　　能： 次の操作がOKかどうか
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private bool NextOperationIsOK()
        {
            bool functionReturnValue = false;

            //戻り値初期化
            functionReturnValue = true;

            //変更されている？
            if (mnuFileSave.Enabled)
            {
                DialogResult result = MessageBox.Show("編集された内容を保存しますか？",Application.ProductName,MessageBoxButtons.YesNo,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1);


                if(result == DialogResult.No)
                {
                    functionReturnValue = false;
                }
                else
                {
                    functionReturnValue = SaveOperation();
                }

            }
            return functionReturnValue;

        }
        //*******************************************************************************
        //機　　能： 保存オペレーション
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private bool SaveOperation()
        {
            bool functionReturnValue = false;

            //戻り値初期化
            functionReturnValue = false;

            if (string.IsNullOrEmpty(TargetFileName))
            {
                functionReturnValue = SaveAsOperation();
            }
            else if (SaveImageInfo(TargetFileName))
            {
                mnuFileSave.Enabled = false;
                functionReturnValue = true;
            }
            return functionReturnValue;

        }
        //*******************************************************************************
        //機　　能： テキストボックスの内容を画像情報ファイルに保存する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： FileName        [I/ ] String    保存ファイル名
        //戻 り 値：                 [ /O] Boolean   True: 保存成功  False: 保存失敗
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private bool SaveImageInfo(string FileName)
        {
            bool functionReturnValue = false;

            var _with4 = theImageInfo.Data;
            _with4.system_name.SetString(txts[1].Text.ToString());
			//String * 16  'システム名                               '1
			_with4.version.SetString(txts[2].Text);
			//String * 6   'ソフトバージョン情報                     '2
			_with4.ct_gentype.SetString(txts[3].Text);
			//String * 2   'CTデータ収集方式（世代）                 '3
			_with4.sliceplan_der.SetString(txts[4].Text);
			//String * 256 'スライスプランディレクトリ名             '4
			_with4.slice_plan.SetString(txts[5].Text);
			//String * 256 'スライスプラン名                         '5
			_with4.d_sileno.SetString(txts[6].Text);
			//String * 4   'マルチスキャン時のスライス番号           '6
			_with4.d_date.SetString(txts[7].Text);
			//String * 10  '撮影年月日                               '7
			_with4.start_time.SetString(txts[8].Text);
			//String * 10  'スキャン時刻                             '8
			_with4.workshop.SetString(txts[9].Text);
			//String * 16  '事業所名                                 '9
			_with4.comment.SetString(txts[10].Text);
			//String * 256 'コメント・エリア                         '10
			_with4.d_rawsts.SetString(txts[11].Text);
			//String * 2   '生データ・ステータス                     '11
			_with4.d_recokind.SetString(txts[12].Text);
			//String * 6   '付帯情報データ種類                       '12
			_with4.full_mode.SetString(txts[13].Text);
			//String * 6   'スキャンモード                           '13
			_with4.matsiz.SetString(txts[14].Text);
			//String * 4   '画像マトリックサイズ                     '14
			_with4.scan_mode.SetString(txts[15].Text);
			//String * 6   'スキャン・モード                         '15
			_with4.speed_mode.SetString(txts[16].Text);
			//String * 6   'スキャン速度モード                       '16
			_with4.scan_speed.SetString(txts[17].Text);
			//String * 8   'スキャン速度値                           '17
			_with4.scan_time.SetString(txts[18].Text);
			//String * 4   'スキャン時間                             '18
			_with4.scan_area.SetString(txts[19].Text);
			//String * 2   '撮影領域名                               '19
			_with4.area.SetString(txts[20].Text);
			//String * 8   'スライスエリアの番号                     '20
			_with4.slice_wid.SetString(txts[21].Text);
			//String * 2   'スライス幅番号                           '21
			_with4.width.SetString(txts[22].Text);
			//String * 8   'スライス幅値                             '22
			_with4.det_ap_num.SetString(txts[23].Text);
			//String * 2   '開口番号                                 '23
			_with4.focus.SetString(txts[24].Text);
			//String * 4   'Ｘ線焦点                                 '24
			_with4.tube.SetString(txts[25].Text);
			//String * 16  'Ｘ線管球種（形式）                       '25
			_with4.energy.SetString(txts[26].Text);
			//String * 2   'Ｘ線条件番号                             '26
			_with4.volt.SetString(txts[27].Text);
			//String * 8   '管電圧                                   '27
			_with4.anpere.SetString(txts[28].Text);
			//String * 8   '管電流                                   '28
			_with4.table_pos.SetString(txts[29].Text);
			//String * 8   'テーブル位置（相対座標）                 '29
			_with4.d_tablepos.SetString(txts[30].Text);
			//String * 8   'テーブル位置（絶対座標）                 '30
			_with4.tilt_angle.SetString(txts[31].Text);
			//String * 6   'データ収集傾斜角度                       '31
			_with4.fc.SetString(txts[32].Text);
			//String * 8   'フィルター関数                           '32
			int bhc = 0;
            int.TryParse(txts[33].Text, out bhc);
            _with4.bhc = bhc;
			//Long         'コーンビーム識別値                       '33
			_with4.bhc_dir.SetString(txts[34].Text);
			//String * 256 'コーンビームCT生データディレクトリ名     '34
			_with4.bhc_name.SetString(txts[35].Text);
			//String * 256 'コーンビームCT生データファイル名         '35
			_with4.zoom_dir.SetString(txts[36].Text);
			//String * 256 'ズームファイルディレクトリ名             '36
			_with4.zoom_name.SetString(txts[37].Text);
			//String * 256 'ズームファイル名                         '37
			int zooming;
            int.TryParse(txts[38].Text,out zooming);
            _with4.zooming = zooming;
			//Long         '拡大画像の有無                           '38
			_with4.zoomx.SetString(txts[39].Text);
			//String * 4   '拡大画像のX座標                          '39
			_with4.zoomy.SetString(txts[40].Text);
			//String * 4   '拡大画像のY座標                          '40
			_with4.zoomsize.SetString(txts[41].Text);
			//String * 4   '拡大画像のサイズ                         '41
			_with4.sift_pos.SetString(txts[42].Text);
			//String * 2   'シフト位置                               '42
			_with4.scale.SetString(txts[43].Text);
			//String * 8   '再構成エリア                             '43
			_with4.roicaltable_dir.SetString(txts[44].Text);
			//String * 256 'ROI測定テーブルディレクトリ              '44
			_with4.roical_table.SetString(txts[45].Text);
			//String * 256 'ROI測定テーブル名                        '45
			_with4.pdtable_dir.SetString(txts[46].Text);
			//String * 256 'プロフィール寸法測定テーブルディレクトリ '46
			_with4.pd_table.SetString(txts[47].Text);
			//String * 256 'プロフィール寸法測定テーブル名           '47
			int roical = 0;
            int.TryParse(txts[48].Text, out roical);
            _with4.roical = roical;
			//Long         'ROI CAL有無                              '48
			int pd = 0;
            int.TryParse(txts[49].Text, out pd);
            _with4.pd = pd;
			//Long         'プロフィール寸法測定有無                 '49
			_with4.scano_dir.SetString(txts[50].Text);
			//String * 6   'スキャノ収集時の管球方向角度             '50
			_with4.pro_dir.SetString(txts[51].Text);
			//String * 2   '試料挿入方向                             '51
			_with4.view_dir.SetString(txts[52].Text);
			//String * 4   '試料観察方向                             '52
			//SetField(txtInfo[53].Text, ref ref _with4.pro_posdir);
			_with4.pro_posdir.SetString(txts[53].Text);
			//String * 2   '試料位置方向                             '53
			//SetField(txtInfo[54].Text, ref ref _with4.scano_dispdir);
			_with4.scano_dispdir.SetString(txts[54].Text);
            //String * 4   'スキャノ表示方向                         '54
			//SetField(txtInfo[55].Text, ref ref _with4.pix_minval);
			_with4.pix_minval.SetString(txts[55].Text);
			//String * 6   '画素の最小値(CT値)                       '55
			//SetField(txtInfo[56].Text, ref ref _with4.pix_maxval);
			_with4.pix_maxval.SetString(txts[56].Text);
			//String * 6   '画素の最大値(CT値)                       '56
			//SetField(txtInfo[57].Text, ref ref _with4.ww);
			_with4.ww.SetString(txts[57].Text);
			//String * 6   'ウィンドウ幅                             '57
			//SetField(txtInfo[58].Text, ref ref _with4.WL);
			_with4.wl.SetString(txts[58].Text);
			//String * 6   'ウィンドウレベル                         '58
            int graphic = 0;
            int.TryParse(txts[59].Text, out graphic);
            _with4.graphic = graphic;
			//Long         'グラフィック画像有無                     '59
			//SetField(txtInfo[60].Text, ref ref _with4.rotation);
			_with4.rotation.SetString(txts[60].Text);
			//String * 8   'データ収集開始位置                       '60
            int trvsno = 0;
            int.TryParse(txts[61].Text, out trvsno);
			_with4.trvsno = trvsno;
			//Long         'スキャントラバース数                     '61
            int trvspt = 0;
            int.TryParse(txts[62].Text, out trvspt);
			_with4.trvspt = trvspt;
			//Long         'スキャントラバースポイント               '62
            double realfdd = 0;
            double.TryParse(txts[63].Text, out realfdd);
			_with4.realfdd = (float)realfdd;
			//Single       'Ｘ線焦点の長さ                           '63
            double dtpitch = 0;
            double.TryParse(txts[64].Text, out dtpitch);
			_with4.dtpitch = (float)dtpitch;
			//Single       'メインチャンネルピッチ                   '64
            double fanangle = 0;
            double.TryParse(txts[65].Text, out fanangle);
			_with4.fanangle = (float)fanangle;
			//Single       'FAN角度                                  '65
            double fcd = 0;
            double.TryParse(txts[66].Text, out fcd);
            _with4.fcd = (float)fcd;
			//Single       '検査テーブルの中心点までの焦点           '66
            double trpitch = 0;
            double.TryParse(txts[67].Text, out trpitch);
			_with4.trpitch = (float)trpitch;
			//Single       'トラバースピッチ                         '67
            double tbldia = 0;
            double.TryParse(txts[68].Text, out tbldia);
			_with4.tbldia = (float)tbldia;
			//Single       'テーブル直径                             '68
            int aprtpt = 0;
            int.TryParse(txts[69].Text, out aprtpt);
			_with4.aprtpt = aprtpt;
			//Long         'アプローチポイント数                     '69
            int bias = 0;
            int.TryParse(txts[70].Text, out bias);
			_with4.bias = bias;
			//Long         '画素値の調整値                           '70
			int slope = 0;
            int.TryParse(txts[71].Text, out slope);
            _with4.slope = slope;
			//Single       '画素値の調整値                           '71
			int mainch = 0;
            int.TryParse(txts[72].Text, out mainch);
            _with4.mainch = mainch;
			//Long         '主検出器チャンネル数                     '72
			int refch = 0;
            int.TryParse(txts[73].Text, out refch);
			_with4.refch = refch;
			//Long         '比較検出器チャンネル数                   '73
			int dummych = 0;
            int.TryParse(txts[74].Text, out dummych);
			_with4.dumych = dummych;
			//Long         'データ収集ダミーチャンネル数             '74
			int datach = 0;
            int.TryParse(txts[75].Text, out datach);
			_with4.datach = datach;
			//Long         'データ収集レファレンス+メインチャンネル数'75
			int allch = 0;
            int.TryParse(txts[76].Text, out allch);
			_with4.allch = allch;
			//Long         'データ収集全チャンネル数                 '76
			int offsetpar = 0;
            int.TryParse(txts[77].Text, out offsetpar);
			_with4.offsetapr = offsetpar;
			//Long         'オフセットデータ助走ポイントデータ数     '77
			int offsetpt = 0;
            int.TryParse(txts[78].Text, out offsetpt);
			_with4.offsetpt = offsetpt;
			//Long         'オフセットデータ収集ポイント数           '78
			int airdtpt = 0;
            int.TryParse(txts[79].Text, out airdtpt);
			_with4.airdtpt = airdtpt;
			//Long         '空気補正データ長                         '79
			int clct_inf = 0;
            int.TryParse(txts[80].Text, out clct_inf);
			_with4.clct_inf = clct_inf;
			//Long         'スキャンの撮影領域画素数                 '80
			int scanotvs = 0;
            int.TryParse(txts[81].Text, out scanotvs);
			_with4.scanotvs = scanotvs;
			//Long         'スキャノデータ収集トラバース数           '81
			int scanoapr = 0;
            int.TryParse(txts[82].Text, out scanoapr);
			_with4.scanoapr = scanoapr;
			//Long         'スキャノアプローチポイントデータ数       '82
			int scanopt = 0;
            int.TryParse(txts[83].Text, out scanopt);
			_with4.scanopt = scanopt;
			//Long         'スキャノデータ収集ポイント数             '83
			int scanoscl = 0;
            int.TryParse(txts[84].Text, out scanoscl);
			_with4.scanoscl = scanoscl;
			//Long         'スキャノデータ収集スケールサイズ         '84
			int scanoair = 0;
            int.TryParse(txts[85].Text, out scanoair);
			_with4.scanoair = scanoair;
			//Long         'スキャノデータ収集空気データポイント数   '85
			int scanobias = 0;
            int.TryParse(txts[86].Text, out scanobias);
			_with4.scanobias = scanobias;
			//Long         'スキャノデータ収集バイアス値             '86
			double scanoslope = 0;
            double.TryParse(txts[87].Text, out scanoslope);
			_with4.scanoslop = (float)scanoslope;
			//Single       'スキャノデータ収集DCVスロープ値          '87
			double scanotblbias = 0;
            double.TryParse(txts[88].Text, out scanotblbias);
			_with4.scanotblbias = (float)scanotblbias;
			//Single       'スキャノデータ収集テーブルバイアス値     '88
			double scanoudpitch = 0;
            double.TryParse(txts[89].Text, out scanoudpitch);
			_with4.scanoudpitch = (float)scanoudpitch;
			//Single       'スキャノデータ収集アップダウンピッチ数   '89
			double scanodpratio = 0;
            double.TryParse(txts[90].Text, out scanodpratio);
			_with4.scanodpratio = (float)scanodpratio;
			//Single       'スキャノデータ収集表示横方向圧縮率       '90
			//SetField(txtInfo[91].Text, ref ref _with4.scano_matsiz);
			_with4.scano_matsiz.SetString(txts[91].Text);
			//String * 4   'スキャノ画像のマトリックスサイズ         '91
			//SetField(txtInfo[92].Text, ref ref _with4.scan_view);
			_with4.scan_view.SetString(txts[92].Text);
			//String * 8   'ビュー数                                 '92
			//SetField(txtInfo[93].Text, ref ref _with4.integ_number);
			_with4.integ_number.SetString(txts[93].Text);
			//String * 8   '積算枚数                                 '93
			double recon_start_angle = 0;
            double.TryParse(txts[94].Text, out recon_start_angle);
			_with4.recon_start_angle = (float)recon_start_angle;
			//Single       '再構成開始角度(ｽﾞｰﾐﾝｸﾞ時使用)            '94
			int image_bias = 0;
            int.TryParse(txts[95].Text, out image_bias);
			_with4.image_bias = image_bias;
			//Long         'バイアス                                 '95
			double image_slope = 0;
            double.TryParse(txts[96].Text, out image_slope);
            _with4.image_slope = (float)image_slope;
			//Single       'スロープ                                 '96
			int image_direction = 0;
            int.TryParse(txts[97].Text, out image_direction);
            _with4.image_direction = image_direction;
			//Long         '画像方向                                 '97
			double mcenter_channel = 0;
            double.TryParse(txts[98].Text, out mcenter_channel);
            _with4.mcenter_channel = (float)mcenter_channel;
			//Single       '再構成用センターチャンネル               '98
			double mchannel_pitch = 0;
            double.TryParse(txts[99].Text, out mchannel_pitch);
            _with4.mchannel_pitch = (float)mchannel_pitch;
			//Single       '１チャンネル当たりの角度                 '99
			double mzoomx = 0;
            double.TryParse(txts[100].Text, out mzoomx);
			_with4.mzoomx = (float)mzoomx;
			//Single       'ズーミング時のROI中心点X座標 (pixel)     '100
			double mzoomy = 0;
            double.TryParse(txts[101].Text, out mzoomy);
			_with4.mzoomy = (float)mzoomy;
			//Single       'ズーミング時のROI中心点Y座標 (pixel)     '101
			double mzoomsize = 0;
            double.TryParse(txts[102].Text, out mzoomsize);
			_with4.mzoomsize = (float)mzoomsize;
			//Single       'ズーミング時のROIサイズ (pixel)          '102
			double mscan_area = 0;
            double.TryParse(txts[103].Text, out mscan_area);
			_with4.mscan_area = (float)mscan_area;
			//Single       'スキャンエリア (mm)                      '103
			double max_mscan_area = 0;
            double.TryParse(txts[104].Text, out max_mscan_area);
			_with4.max_mscan_area = (float)max_mscan_area;
			//Single       '最大スキャンエリア (mm)                  '104
			int fimage_bit = 0;
            int.TryParse(txts[105].Text, out fimage_bit);
            _with4.fimage_bit = fimage_bit;;
			//Long         '画像取り込みビット数                     '105
            double fid = 0;
            double.TryParse(txts[106].Text, out fid);
            _with4.fid = (float)fid;
			//Single       'X線焦点からIIまでの距離                  '106
			int recon_mask = 0;
            int.TryParse(txts[107].Text, out recon_mask);
            _with4.recon_mask = recon_mask;
			//Long         '再構成形状                               '107
			int n1 = 0;
            int.TryParse(txts[108].Text, out n1);
            _with4.n1 = n1;
			//Long         '有効データ開始チャンネル                 '108
			int n2 = 0;
            int.TryParse(txts[109].Text, out n2);
            _with4.n2 = n2;
			//Long         '有効データ終了チャンネル                 '109
			int mc = 0;
            int.TryParse(txts[110].Text, out mc);
            _with4.mc = mc;
			//Long         '縦中心チャンネル                         '110
			double theta0 = 0;
            double.TryParse(txts[111].Text, out theta0);
            _with4.theta0 = (float)theta0;
			//Single       '有効ファン角                             '111
			double theta01 = 0;
            double.TryParse(txts[112].Text, out theta01);
            _with4.theta01 = (float)theta01;
			//Single       '有効データ包含ファン角1 (radian)         '112
			double theta02 = 0;
            double.TryParse(txts[113].Text, out theta02);
            _with4.theta02 = (float)theta02;
			//Single       '有効データ包含ファン角2 (radian)         '113
			double thetaoff = 0;
            double.TryParse(txts[114].Text, out thetaoff);
            _with4.thetaoff = (float)thetaoff;
			//Single       '有効データ包含ファン角                   '114
			double nc = 0;
            double.TryParse(txts[115].Text, out nc);
            _with4.nc = (float)nc;
			//Single       '回転中心チャンネル                       '115
			double deleta_theta = 0;
            double.TryParse(txts[116].Text, out deleta_theta);
            _with4.delta_theta = (float)deleta_theta;
			//Single       'n方向データピッチ (radian)               '116
			double dpm = 0;
            double.TryParse(txts[117].Text, out dpm);
  			_with4.dpm = (float)dpm;
			//Single       'm方向データピッチ (mm)                   '117
			double n0 = 0;
            double.TryParse(txts[118].Text, out n0);
  			_with4.n0 = (float)n0;
			//Single       'n方向画像中心対応チャンネル              '118
			double m0 = 0;
            double.TryParse(txts[119].Text, out m0);
  			_with4.m0 = (float)m0;
			//Single       'm方向画像中心対応チャンネル              '119
			double alpha = 0;
            double.TryParse(txts[120].Text, out alpha);
  			_with4.alpha = (float)alpha;
			//Single       'オーバーラップ角度 (radian)              '120
			double zp = 0;
            double.TryParse(txts[121].Text, out zp);
  			_with4.zp = (float)zp;
			//Single       'ヘリカルピッチ (mm)                      '121
			double zs0 = 0;
            double.TryParse(txts[122].Text, out zs0);
  			_with4.zs0 = (float)zs0;
			//Single       '初期1枚目のスライス位置                  '122
			int iud = 0;
            int.TryParse(txts[123].Text, out iud);
            _with4.iud = iud;
			//Long         '上昇下降識別値                           '123
			int ioff = 0;
            int.TryParse(txts[124].Text, out ioff);
            _with4.ioff = ioff;
			//Long         'オフセット識別値                         '124
			int k = 0;
            int.TryParse(txts[125].Text, out k);
            _with4.k = k;
			//Long         'スライス枚数                             '125
			double delta_z = 0;
            double.TryParse(txts[126].Text, out delta_z);
            _with4.delta_z = (float)delta_z;
			//Single       '軸方向Boxelサイズ (mm)                   '126
			double ze0 = 0;
            double.TryParse(txts[127].Text, out ze0);
 			_with4.ze0 = (float)ze0;
			//Single       '初期K枚目スライスのZ位置 (mm)            '127
 			int inh = 0;
            int.TryParse(txts[128].Text, out inh);
 			_with4.inh = inh;
			//Long         'ヘリカルモード                           '128
			int md = 0;
            int.TryParse(txts[129].Text, out md);
 			_with4.md = md;
			//Long         '縦チャンネル数                           '129
 			double b1 = 0;
            double.TryParse(txts[130].Text, out b1);
 			_with4.b1 = (float)b1;
			//Single       'コーンビーム用幾何歪パラメータB1         '130
			double scan_posi_a = 0;
            double.TryParse(txts[131].Text, out scan_posi_a);
 			_with4.scan_posi_a = (float)scan_posi_a;
			//Single       'スキャン位置の傾き                       '131
 			int mutlti_tube = 0;
            int.TryParse(txts[132].Text, out mutlti_tube);
 			_with4.multi_tube = mutlti_tube;
			//Long         '複数Ｘ線管                               '132
			double z0 = 0;
            double.TryParse(txts[133].Text, out z0);
			_with4.z0 = (float)z0;
			//Single       'コーンビームスキャン時のテーブル位置     '133
			int acq_view = 0;
            int.TryParse(txts[134].Text, out acq_view);
			_with4.acq_view = acq_view;
			//Long         'データ収集ビュー数                       '134
			double fid_offset = 0;
            double.TryParse(txts[135].Text, out fid_offset);
			_with4.fid_offset = (float)fid_offset;
			//Single       'FIDオフセット                            '135
			double fcd_offset = 0;
            double.TryParse(txts[136].Text, out fcd_offset);
			_with4.fcd_offset = (float)fcd_offset;
			//Single       'FCDオフセット                            '136
			double a1 = 0;
            double.TryParse(txts[137].Text, out a1);
			_with4.a1 = (float)a1;
			//Single       '幾何歪み補正パラメータA1（1/mm）         '137
			double fs = 0;
            double.TryParse(txts[138].Text, out fs);
			_with4.fs = (float)fs;
			//Single       '焦点サイズ（mm）                         '138
			double x0 = 0;
            double.TryParse(txts[139].Text, out x0);
			_with4.x0 = (float)x0;
			//Single       '骨塩等価物質密度（mg/cm3）               '139
			double x1 = 0;
            double.TryParse(txts[140].Text, out x1);
			_with4.x1 = (float)x1;
			//Single       '骨塩等価物質密度（mg/cm3）               '140
			double x2 = 0;
            double.TryParse(txts[141].Text, out x2);
			_with4.x2 = (float)x2;
			//Single       '骨塩等価物質密度（mg/cm3）               '141
			double x3 = 0;
            double.TryParse(txts[142].Text, out x3);
			_with4.x3 = (float)x3;
			//Single       '骨塩等価物質密度（mg/cm3）               '142
			double x4 = 0;
            double.TryParse(txts[143].Text, out x4);
			_with4.x4 = (float)x4;
			//Single       '骨塩等価物質密度（mg/cm3）               '143
			int instance_num = 0;
            int.TryParse(txts[144].Text, out instance_num);
			_with4.instance_num = instance_num;
			//Long         'インスタンス番号                         '144
			//SetField(txtInfo[145].Text, ref ref _with4.iifield);
			_with4.iifield.SetString(txts[145].Text);
			//String * 10  'I.I.視野                                 '145
			//SetField(txtInfo[146].Text, ref ref _with4.filter_Renamed);
			_with4.filter.SetString(txts[146].Text);
			//String * 10  'フィルター                               '146
			int study_id = 0;
            int.TryParse(txts[147].Text, out study_id);
			_with4.study_id = study_id;
			//Long         '検査ID                                   '147
			int series_num = 0;
            int.TryParse(txts[148].Text, out series_num);
			_with4.series_num = series_num;
			//Long         'シリーズ番号                             '148
			int acq_num = 0;
            int.TryParse(txts[149].Text, out acq_num);
			_with4.acq_num = acq_num;
			//Long         '収集番号                                 '149
			//SetField(txtInfo[150].Text, ref ref _with4.instance_uid);
			_with4.instance_uid.SetString(txts[150].Text);
			//String * 64  'インスタンスUID                          '150
			double frame_rate = 0;
            double.TryParse(txts[151].Text, out frame_rate);
			_with4.frame_rate = (float)frame_rate;
			//Single       'フレームレート                           '151
			double scan_start_angle = 0;
            double.TryParse(txts[152].Text, out scan_start_angle);
			_with4.scan_start_angle = (float)scan_start_angle;
			//Single       '                                         '152
			int detector = 0;
            int.TryParse(txts[153].Text, out detector);
            _with4.detector = detector;
			//Long         '検出器 0:X線II　1:FPD                    '153
			int data_mode = 0;
            int.TryParse(txts[154].Text, out data_mode);
            _with4.data_mode = data_mode;
			//Long         'データモード                             '154
			int cone_image_mode = 0;
            int.TryParse(txts[155].Text, out cone_image_mode);
            _with4.cone_image_mode = cone_image_mode;
			//Long         'コーンビーム再構成時の画質               '155
			int fine_table_x = 0;
            int.TryParse(txts[156].Text, out fine_table_x);
            _with4.fine_table_x = fine_table_x;
			//Long         '微調テーブルＸ軸の有無                   '156
			double ftable_x_pos = 0;
            double.TryParse(txts[157].Text, out ftable_x_pos);
			_with4.ftable_x_pos = (float)ftable_x_pos;
			//Single       '微調テーブルＸ軸の座標（mm）             '157
			int fine_table_y = 0;
            int.TryParse(txts[158].Text, out fine_table_y);
			_with4.fine_table_y = fine_table_y;
			//Long         '微調テーブルＹ軸の有無                   '158
			double ftable_y_pos = 0;
            double.TryParse(txts[159].Text, out ftable_y_pos);
			_with4.ftable_y_pos = (float)ftable_y_pos;
			//Single       '微調テーブルＹ軸の座標（mm）             '159
			int rotate_select = 0;
            int.TryParse(txts[160].Text, out rotate_select);
			_with4.rotate_select = rotate_select;
			//Long         '回転選択結果 0:テーブル回転 1:Ｘ線管回転 '160
			int c_cw_ccw = 0;
            int.TryParse(txts[161].Text, out c_cw_ccw);
			_with4.c_cw_ccw = c_cw_ccw;
			//Long         '回転方向                                 '161
			int kv = 0;
            int.TryParse(txts[162].Text, out kv);
			_with4.kv = kv;
			//Long         'ビニング係数(v_mag/h_mag)                '162
			double mbhc_AirLogValue = 0;
            double.TryParse(txts[163].Text, out mbhc_AirLogValue);
			_with4.mbhc_AirLogValue = (float)mbhc_AirLogValue;
			//Single       'ログ変換後のエアーの値                   '163

            double fpd_gain_f = 0;
            double.TryParse(txts[164].Text, out fpd_gain_f);
            _with4.fpd_gain_f = (float)fpd_gain_f;
            //Single       'FPDゲイン 表示用(pF)                     '164

            double fpd_integ_f = 0;
            double.TryParse(txts[165].Text, out fpd_integ_f);
            _with4.fpd_integ_f = (float)fpd_integ_f;
            //Single       'FPD積分時間 表示用(ms)                   '165

            int xfilter = 0;
            int.TryParse(txts[166].Text, out xfilter);
            _with4.xfilter = xfilter;
            //Long　X線フィルタ インデックス                         '166

            _with4.xfilter_c.SetString(txts[167].Text);
            //String X線フィルタ                                     '167

            int xfocus = 0;
            int.TryParse(txts[168].Text, out xfocus);
            _with4.xfocus = xfocus;
            //Long X線焦点  インデックス                             '168

            _with4.xfocus_c.SetString(txts[169].Text);
            //String X線焦点                                         '169

			int largetRotTable = 0;
            int.TryParse(txts[170].Text, out largetRotTable);
			_with4.largetRotTable = largetRotTable;
			//Long         '回転大テーブルを装着してスキャンしたか   '170
			int table_rotation = 0;
            int.TryParse(txts[171].Text, out table_rotation);
			_with4.table_rotation = table_rotation;
			//Long         '試料テーブル回転(0:ステップ,1:回転)      '171
            
            int auto_centering = 0;
            int.TryParse(txts[172].Text, out auto_centering);
            _with4.auto_centering = auto_centering;
			//Long         'ｵｰﾄｾﾝﾀﾘﾝｸﾞ有無(0:無,1:有)                '172

            double mscano_area = 0;
            double.TryParse(txts[173].Text, out mscano_area);
            _with4.mscano_area = (float)mscano_area;
            //Single        'スキャノエリア                          '173

            double mscano_mdtpitch = 0;
            double.TryParse(txts[174].Text, out mscano_mdtpitch);
            _with4.mscano_mdtpitch = (float)mscano_mdtpitch;
            //Single        'スキャノデータピッチ                    '174

            _with4.mscano_width.SetString(txts[175].Text);
            //Single        'スキャノ厚                              '175

            int mscanopt = 0;
            int.TryParse(txts[176].Text, out mscanopt);
            _with4.mscanopt = mscanopt;
            //Long         'スキャノポイント                         '176

            double mscanoscl = 0;
            double.TryParse(txts[177].Text, out mscanoscl);
            _with4.mscanoscl = (float)mscanoscl;
            //Single       'スキャノスケール                         '177

            double mscano_udpitch = 0;
            double.TryParse(txts[178].Text, out mscano_udpitch);
            _with4.mscano_udpitch = (float)mscano_udpitch;
            //Single       'スキャノ昇降ピッチ                       '178

            int mscano_bias = 0;
            int.TryParse(txts[179].Text, out mscano_bias);
            _with4.mscano_bias = mscano_bias;
            //Long         'スキャノポイント                         '179

            double mscano_slope = 0;
            double.TryParse(txts[180].Text, out mscano_slope);
            _with4.mscano_slope = (float)mscano_slope;
            //Single       'スキャノスロープ                         '180

            double mscano_dp_ratio = 0;
            double.TryParse(txts[181].Text, out mscano_dp_ratio);
            _with4.mscano_dp_ratio = (float)mscano_dp_ratio;
            //Single       'スキャノ縦横比                           '181

            double numOfAdjCenterCh = 0;
            double.TryParse(txts[182].Text, out numOfAdjCenterCh);
            _with4.numOfAdjCenterCh = (float)numOfAdjCenterCh;
            //Single       '回転中心スキャノ昇降ピッチ                          '182

            double scan_fcdMecha = 0.0;
            double.TryParse(txts[183].Text, out scan_fcdMecha);
            _with4.scan_fcdMecha = (float)scan_fcdMecha;
            // Single       'FCD値(従来値)                                      '183 

            double scan_fcdLinear = 0.0;
            double.TryParse(txts[184].Text, out scan_fcdLinear);
            _with4.scan_fcdLinear = (float)scan_fcdLinear;
            // Single       'FCD値(リニアスケール値)                            '184 

            double scan_fddMecha = 0.0;
            double.TryParse(txts[185].Text, out scan_fddMecha);
            _with4.scan_fddMecha = (float)scan_fddMecha;
            // Single       'FDD(従来値)                                        '185  

            double scan_fddLinear = 0.0;
            double.TryParse(txts[186].Text, out scan_fddLinear);
            _with4.scan_fddLinear = (float)scan_fddLinear;
            // Single       'FDD(リニアスケール値)                              '186

            double scan_table_x_posMecha = 0.0;
            double.TryParse(txts[187].Text, out scan_table_x_posMecha);
            _with4.scan_table_x_posMecha = (float)scan_table_x_posMecha;
            // Single       'テーブルY軸(光軸と垂直方向)(従来値)                '187

            double scan_table_x_posLinear = 0.0;
            double.TryParse(txts[188].Text, out scan_table_x_posLinear);
            _with4.scan_table_x_posLinear = (float)scan_table_x_posLinear;
            // Single       'テーブルY軸(光軸と巣直方向)(リニアスケール値)      '188

            double scan_udab_pos = 0.0;
            double.TryParse(txts[189].Text, out scan_udab_pos);
            _with4.scan_udab_pos = (float)scan_udab_pos;
            // Single       'テーブル昇降(従来値)                               '189

            double scan_ud_linear_pos = 0.0;
            double.TryParse(txts[190].Text, out scan_ud_linear_pos);
            _with4.scan_ud_linear_pos = (float)scan_ud_linear_pos;
            // Single       'テーブル昇降(リニアスケール値)                     '190

            int w_scan = 0;
            int.TryParse(txts[191].Text, out w_scan);
            _with4.w_scan = w_scan;
            // Long         'Wスキャン                                          '191

            int pl_mbhc = 0;
            int.TryParse(txts[192].Text, out pl_mbhc);
            _with4.mbhc_phantomless = pl_mbhc;
            // Long         'ファントムレスBHC                                  '192

            int pl_collion = 0;
            int.TryParse(txts[193].Text, out pl_collion);
            _with4.mbhc_phantomless_colli_on = pl_collion;
            // Long         'ファントムレスBHCコリメータON                      '193

            _with4.mbhc_phantomless_c.SetString(txts[194].Text);
            // String       'ファントムレスBHC材質                              '194

            string[] cell;
            cell = txts[195].Text.Split(',');

            double pl_mbhc_para0 = 0.0;
            double.TryParse(cell[0], out pl_mbhc_para0);
            _with4.mbhc_phantomless_para[0] = (float)pl_mbhc_para0;
            // Single      'ファントムレスBHCパラメータ1                        '195

            double pl_mbhc_para1 = 0.0;
            double.TryParse(cell[1], out pl_mbhc_para1);
            _with4.mbhc_phantomless_para[0] = (float)pl_mbhc_para1;
            // Single      'ファントムレスBHCパラメータ1                        '195

            double pl_mbhc_para2 = 0.0;
            double.TryParse(cell[2], out pl_mbhc_para2);
            _with4.mbhc_phantomless_para[2] = (float)pl_mbhc_para2;
            // Single      'ファントムレスBHCパラメータ1                        '195

            //戻り値セット
            functionReturnValue = ImageInfo.WriteImageInfo(ref _with4, FileName, "");
			return functionReturnValue;
        }

        //*******************************************************************************
        //機　　能： 新規保存オペレーション
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private bool SaveAsOperation()
        {
            bool functionReturnValue = false;

            CommonDialog2.FileName = "";
            CommonDialog2.Title = "付帯情報を保存";
            CommonDialog2.Filter = "付帯情報ファイル(*.inf)|*.inf";
            //CommonDialog2. = false;
            //CommonDialog2.Multiselect = false;

            DialogResult result = CommonDialog2.ShowDialog();

            if (result == DialogResult.Cancel)
            {
                return functionReturnValue;
            }

            if (SaveImageInfo(CommonDialog2.FileName))
            {
                TargetFileName = CommonDialog2.FileName;
                mnuFileSave.Enabled = false;
                functionReturnValue = true;
            }

            return functionReturnValue;

        }

        private void mnuFileSaveAs_Click(object sender, EventArgs e)
        {
            SaveAsOperation();
        }

        private void mnuFile_Click(object sender, EventArgs e)
        {
            //何もしない。
        }

        private void mnuFileSave_Click(object sender, EventArgs e)
        {
            SaveOperation();
        }

        private void mnuFileQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
