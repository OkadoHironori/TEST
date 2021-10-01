using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;
//
using CT30K.Common;
using CTAPI;
using TransImage;
//using CT30K.Controls;
//using CT30K.Modules;
using CT30K.Properties;

namespace CT30K
{
    /// <summary>
    /// 画像情報表示フォーム
    /// </summary>
    public partial class frmImageInfo : Form
    {
        /// <summary>
        /// 現在表示している付帯情報レコード
        /// </summary>
        private ImageInfo myInfoRec;

        /// <summary>
        /// 現在表示している付帯情報のファイル名（拡張子なし）
        /// </summary>
        private string myTarget;
        
        /// <summary>
        /// 詳細表示モード？
        /// </summary>
        private bool myDetailOn;

        private List<Label> lblContext = new List<Label>();
        private List<Label> lblItem = new List<Label>();
        private List<Label> lblColon = new List<Label>();

        #region 項目のインデクスを格納する変数
        /// <summary>
        /// スキャン位置
        /// </summary>
        private int IdxScanPos;

        /// <summary>
        /// スキャン年月日
        /// </summary>
        private int IdxScanDate;

        /// <summary>
        /// スキャン時刻
        /// </summary>
        private int IdxScanTime;

        /// <summary>
        /// データ収集時間
        /// </summary>
        private int IdxDataAcq;

        /// <summary>
        /// 再構成時間
        /// </summary>
        private int IdxReconTime;
        
        /// <summary>
        /// 管電圧
        /// </summary>
        private int IdxTubeVolt;

        /// <summary>
        /// 管電流
        /// </summary>
        private int IdxTubeCurrent;

        /// <summary>
        /// ビュー数
        /// </summary>
        private int IdxViews;

        /// <summary>
        /// 積算枚数
        /// </summary>
        private int IdxIntegNum;

        /// <summary>
        /// I.I.視野
        /// </summary>
        private int IdxIIField;

        /// <summary>
        /// FOV
        /// </summary>
        private int IdxFOV;

        /// <summary>
        /// スライス厚
        /// </summary>
        private int IdxSliceWidth;

        /// <summary>
        /// システム名
        /// </summary>
        private int IdxSystemName;

        /// <summary>
        /// 画像サイズ
        /// </summary>
        private int IdxMatrix;

        /// <summary>
        /// コメント
        /// </summary>
        private int IdxComment;

        /// <summary>
        /// スキャンモード
        /// </summary>
        private int IdxScanMode;

        /// <summary>
        /// フィルタ関数
        /// </summary>
        private int IdxFilterFunc;

        /// <summary>
        /// フィルタ処理
        /// </summary>
        private int IdxFilterProc;

        /// <summary>
        /// RFC
        /// </summary>
        private int IdxRFC;

        /// <summary>
        /// BHCテーブル
        /// </summary>
        private int IdxBHCTable;

        /// <summary>
        /// 画像バイアス
        /// </summary>
        private int IdxImageBias;

        /// <summary>
        /// 画像スロープ
        /// </summary>
        private int IdxImageSlope;

        /// <summary>
        /// データモード   'v15.10追加 byやまおか 2009/11/26
        /// </summary>
        private int IdxDataMode;

        /// <summary>
        /// 断面像方向
        /// </summary>
        private int IdxDirection;

        /// <summary>
        /// FID
        /// </summary>
        private int IdxFID;

        /// <summary>
        /// FCD
        /// </summary>
        private int IdxFCD;

        /// <summary>
        /// ウィンドウレベル
        /// </summary>
        private int IdxWindowLevel;

        /// <summary>
        /// ウィンドウ幅
        /// </summary>
        private int IdxWindowWidth;
        
        /// <summary>
        /// 1画素サイズ
        /// </summary>
        private int IdxPixelSize;
        
        /// <summary>
        /// 拡大倍率
        /// </summary>
        private int IdxMagnify;
        
        /// <summary>
        /// スケール
        /// </summary>
        private int IdxScale;
        
        /// <summary>
        /// FPDゲイン      'v17.00追加 byやまおか 2010/02/17
        /// </summary>
        private int IdxFpdGain;

        /// <summary>
        /// FPD積分時間    'v17.00追加 byやまおか 2010/02/17
        /// </summary>
        private int IdxFpdInteg;

        /// <summary>
        /// ガンマ補正値    'v19.00追加 by長野 2012/02/21
        /// </summary>
        private int IdxGamma;


        #endregion

        #region メンバ変数
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmImageInfo myForm = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmImageInfo()
        {
            InitializeComponent();

            // イベント定義
            //InitializeEventHandler();
        }
        #endregion

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmImageInfo Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmImageInfo();
                }

                return myForm;
            }
        }
        #endregion

        //正規表現に変更した。
        //#region イベント定義
        ///// <summary>
        ///// イベント定義
        ///// </summary>
        //private void InitializeEventHandler()
        //{
        //    //「詳細」（または「簡易」）ボタンクリック時処理
        //    cmdDetailMode.Click += (sender, e) => DetailOn = !DetailOn;
            
        //    // フォームロード時の処理
        //    this.Load += (sender, e) =>
        //    {
        //        //このフォームのキャプション
        //        this.Text = Resources._12800;//画像情報

        //        //項目の初期化
        //        AddItems();

        //        //コメントに関してクリックが可能であることをユーザーに知らせるための設定
        //        //なお、このフォームのロード時の Enabled プロパティは False

        //        lblContext[IdxComment].Cursor = Cursors.Help;//マウスを近づけると？付き矢印となるようにする
        //        toolTip1.SetToolTip(lblContext[IdxComment], Resources._12126);//クリックすると値を入力できます。

        //        //初期表示：簡易表示モード
        //        DetailOn = false;

        //        //表示位置： スキャン画像の左下  'v15.10追加 byやまおか 2009/11/26
        //        this.Location = new Point(frmScanImage.Instance.Left + frmCTMenu.Instance.Toolbar1.Width + 4, 
        //                                  frmScanImage.Instance.Bottom - this.Height - 16);
        //    };

        //    // フォームクロージング時処理
        //    this.FormClosing += (sender, e) => 
        //    {
        //        // フォームのコントロールメニューから「閉じる」コマンドが選択された場合
        //        if (e.CloseReason == CloseReason.UserClosing)
        //        {
        //            // 非表示にする
        //            this.Hide();

        //            // クローズさせない
        //            e.Cancel = true;
        //        }
        //    };

        //    #region クライアントサイズ変更時処理
        //    this.ClientSizeChanged += (sender, e) =>
        //    {
        //        // スケールと詳細/簡易表示切替ボタンの位置の調整
        //        fraScale.Top = cmdDetailMode.Top = ClientRectangle.Height - fraScale.Height - 2;
        //    };
        //    #endregion
        //}
        //#endregion


        // フォームロード時の処理
        private void frmImageInfo_Load(object sender, EventArgs e)
        {
            //このフォームのキャプション
            this.Text = Resources._12800;//画像情報

            //v17.60 英語用レイアウト調整 by長野 2011/05/23
            if (modCT30K.IsEnglish == true)
            {
                EnglishAdjustLayout();
            }

            //項目の初期化
            AddItems();

            //コメントに関してクリックが可能であることをユーザーに知らせるための設定
            //なお、このフォームのロード時の Enabled プロパティは False

            lblContext[IdxComment].Cursor = Cursors.Help;//マウスを近づけると？付き矢印となるようにする
            toolTip1.SetToolTip(lblContext[IdxComment], Resources._12126);//クリックすると値を入力できます。

            //初期表示：簡易表示モード
            DetailOn = false;

            //表示位置： スキャン画像の左下  'v15.10追加 byやまおか 2009/11/26
            this.Location = new Point(frmScanImage.Instance.Left + frmCTMenu.Instance.Toolbar1.Width + 4,
                                      frmScanImage.Instance.Bottom - this.Height - 16);
        
        }

        // フォームクロージング時処理
        private void frmImageInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            // フォームのコントロールメニューから「閉じる」コマンドが選択された場合
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // 非表示にする
                this.Hide();

                // クローズさせない
                e.Cancel = true;
            }
        }

        //「詳細」（または「簡易」）ボタンクリック時処理
        private void cmdDetailMode_Click(object sender, EventArgs e)
        {
            DetailOn = !DetailOn;
        }

        //クライアントサイズ変更時処理
        private void frmImageInfo_ClientSizeChanged(object sender, EventArgs e)
        {
            // スケールと詳細/簡易表示切替ボタンの位置の調整
            fraScale.Top = cmdDetailMode.Top = ClientRectangle.Height - fraScale.Height - 2;

        }

        #region プロパティ
        /// <summary>
        /// DetailOnプロパティ：詳細・簡易表示用
        /// </summary>
        private bool DetailOn
        {
            get { return myDetailOn; }
            set
            {
                myDetailOn = value;

                // ボタンのテキストを更新
                cmdDetailMode.Text = (myDetailOn ? "<<簡易表示" : "詳細表示>>");

                int top = lblItem[0].Top;

                for (int i = 0; i < lblItem.Count; i++)
                {
                    // 表示するか？
                    bool isVisible = myDetailOn | Convert.ToBoolean(lblItem[i].Tag);

                    lblItem[i].Visible = isVisible;
                    lblItem[i].Top = top;
                    lblColon[i].Visible = isVisible;
                    lblColon[i].Top = top;
                    lblContext[i].Visible = isVisible;
                    lblContext[i].Top = top;

                    // 次に表示する位置
                    if (isVisible)
                        top += lblItem[i].Height + 1;
                }

                // 調整後の高さを取得
                int height = 32 + top + fraScale.Height;

                // ボトムを固定して高さを調整する：トップが画面からはみ出さないようにもする
                this.SetBounds(this.Left, Math.Max(this.Bottom - height, 0), this.Width, height);
            }
        }

        /// <summary>
        /// Commentプロパティ
        /// </summary>
        public string Comment
        {
            get { return lblContext[IdxComment].Text.Trim(); }
            set
            {
                // 付帯情報へ書き込み
                myInfoRec.Data.comment.SetString(value);
                Functions.WriteStructure(myTarget + ".inf", myInfoRec);

                // コメントを表示するコントロールを更新
                lblContext[IdxComment].Text = value + Strings.Space(20);
            }
        }

        /// <summary>
        /// SizePerPixelプロパティ：１画素あたりの長さ(mm)
        /// </summary>
        public double SizePerPixel
        {
            get { return Conversion.Val(lblContext[IdxPixelSize].Text); }
        }

        /// <summary>
        /// SliceNameプロパティ：スライス名（.img付き）
        /// </summary>
        public string SliceName
        {
            get { return Path.GetFileName(modLibrary.AddExtension(myTarget, ".img")); }
        }

        /// <summary>
        /// FullSliceNameプロパティ
        /// </summary>
        public string FullSliceName
        {
            get { return modLibrary.AddExtension(myTarget, ".img"); }
        }

        /// <summary>
        /// ScanDateプロパティ：スキャン年月日
        /// </summary>
        public string ScanDate
        {
            get { return Strings.Trim(lblContext[IdxScanDate].Text); }
        }

        /// <summary>
        /// SlicePosプロパティ：スライス位置（mm付き文字列）
        /// </summary>
        public string SlicePos
        {
            get { return myInfoRec.Data.table_pos.GetString() + " mm"; }
        }

        /// <summary>
        /// Matrixプロパティ：マトリクスサイズ
        /// </summary>
        public int Matrix
        {
            get { return Convert.ToInt32(myInfoRec.Data.matsiz.GetString()); }
        }

        /// <summary>
        /// ScaleValueプロパティ：現在表示中の画像のスケール値
        /// </summary>
        public double ScaleValue
        {
            get { return Conversion.Val(myInfoRec.Data.scale.GetString()) / 1000; }
        }

        /// <summary>
        /// IsConeBeamプロパティ：コーンビーム画像か？
        /// </summary>
        public bool IsConeBeam
        {
            get { return (myInfoRec.Data.bhc == 1); }
        }

        /// <summary>
        /// ReconStartAngleプロパティ：表示している画像回転角
        /// </summary>
        public float ReconStartAngle
        {
            get { return myInfoRec.Data.recon_start_angle; }
        }

        /// <summary>
        /// ウィンドウレベル
        /// </summary>
        public int WindowLevel
        {
            get { return Convert.ToInt32(myInfoRec.Data.wl.GetString()); }
            set
            {

                if (string.IsNullOrEmpty(myTarget))
                    return;

                // 付帯情報へ書き込み
                myInfoRec.Data.wl.SetString(value.ToString());
                Functions.WriteStructure(myTarget + ".inf", myInfoRec);

                // 該当するコントロールを更新
                lblContext[IdxWindowLevel].Text = Convert.ToString(value);
            }
        }

        /// <summary>
        /// ウィンドウ幅
        /// </summary>
        public int WindowWidth
        {
            get { return Convert.ToInt32(myInfoRec.Data.ww.GetString()); }
            set
            {
                if (string.IsNullOrEmpty(myTarget))
                    return;

                // 付帯情報へ書き込み
                myInfoRec.Data.ww.SetString(value.ToString());
                Functions.WriteStructure(myTarget + ".inf", myInfoRec);

                // 該当するコントロールを更新
                lblContext[IdxWindowWidth].Text = Convert.ToString(value);
            }
        }

        //*******************************************************************************
        //機　　能： ガンマ補正値
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.99 2012/02/21 (検S1)長野    新規作成
        //*******************************************************************************

        public float GAMMA
        {


            get { return Convert.ToSingle(Conversion.Val(myInfoRec.Data.gamma.GetString())); }
            set
            {

                if (string.IsNullOrEmpty(myTarget))
                    return;

                //付帯情報へ書き込み
                myInfoRec.Data.gamma.SetString(value.ToString());
                Functions.WriteStructure(myTarget + ".inf", myInfoRec);

                //該当するコントロールを更新
                lblContext[IdxGamma].Text = Convert.ToString(value);

            }
        }


        /// <summary>
        /// Targetプロパティ：現在表示中のファイル名（拡張子なし）
        /// </summary>
        public string Target
        {
            get { return myTarget; }
            set
            {
                if (value != myTarget)
                {
                    if (string.IsNullOrEmpty(myTarget))
                    {
                        for (int i = 0; i < lblContext.Count; i++)
                        {
                            lblContext[i].Enabled = true;
                        }
                    }
                    else if (string.IsNullOrEmpty(value))
                    {
                        for (int i = 0; i < lblContext.Count; i++)
                        {
                            lblContext[i].Text = "";
                            lblContext[i].Enabled = false;
                        }
                    }
                }

                myTarget = value;

                // 詳細・簡易ボタンの使用可能・不可の制御
                cmdDetailMode.Enabled = !string.IsNullOrEmpty(myTarget);

                // ヌルが設定された場合，ここで抜ける
                if (string.IsNullOrEmpty(myTarget))
                {
                    return;
                }

                // 画像情報ファイルの読み込み
                if (!Functions.ReadStructure(myTarget + ".inf", ref myInfoRec)) 
                {
                    return;
                }

                //デフォルトパス名の設定
                //modFileIO.SaveDefaultFolder(Resources.STR_CTImage, Path.GetDirectoryName(myTarget));
                modFileIO.SaveDefaultFolder(Convert.ToString((int)StringTable.IDS_CTImage), Path.GetDirectoryName(myTarget));
                
                //途中エラーが発生しても処理を続行する
                // ERROR: Not supported in C#: OnErrorStatement


                //var _with2 = myInfoRec;
                lblContext[IdxScanPos].Text = myInfoRec.Data.d_tablepos.GetString();                                     //スキャン位置（絶対座標）
                lblContext[IdxScanDate].Text = myInfoRec.Data.d_date.GetString();                                        //スキャン年月日
                lblContext[IdxScanTime].Text = myInfoRec.Data.start_time.GetString();                                    //スキャン時刻
                lblContext[IdxTubeVolt].Text = Conversion.Val(myInfoRec.Data.volt.GetString()).ToString("0.0");          //管電圧(kV)
                lblContext[IdxTubeCurrent].Text = Conversion.Val(myInfoRec.Data.anpere.GetString()).ToString("0.000");   //管電流(μA)
                lblContext[IdxViews].Text = myInfoRec.Data.scan_view.GetString().Trim();                                 //ビュー数
                lblContext[IdxIntegNum].Text = myInfoRec.Data.integ_number.GetString().Trim();                           //積算枚数
                
                // I.I.視野
                if ((myInfoRec.Data.detector == 0) | (myInfoRec.Data.detector == 1))
                {
                    lblContext[IdxIIField].Text = myInfoRec.Data.iifield.GetString();
                }
                else
                {
                    lblContext[IdxIIField].Text = "";               
                }

                lblContext[IdxFOV].Text = myInfoRec.Data.mscan_area.ToString("0.000");                                   //FOV(mm)                　  
                lblContext[IdxSliceWidth].Text = Conversion.Val(myInfoRec.Data.width.GetString()).ToString("0.000");     //スライス厚(mm)
                lblContext[IdxSystemName].Text = myInfoRec.Data.system_name.GetString();                                 //システム名
                lblContext[IdxMatrix].Text = myInfoRec.Data.matsiz.GetString();                                          //画像サイズ
                lblContext[IdxComment].Text = myInfoRec.Data.comment.GetString() + Strings.Space(20);                    //コメント                   
                lblContext[IdxScanMode].Text = myInfoRec.Data.full_mode.GetString();                                     //スキャンモード
                lblContext[IdxFilterFunc].Text = myInfoRec.Data.fc.GetString();                                          //フィルタ関数
                lblContext[IdxImageBias].Text = myInfoRec.Data.image_bias.ToString("0.0");                               //画像バイアス
                lblContext[IdxImageSlope].Text = myInfoRec.Data.image_slope.ToString("0.0");                             //画像スロープ
                lblContext[IdxDataMode].Text = (myInfoRec.Data.bhc == 1 ? Resources.ResourceManager.GetString("STR_" + Convert.ToString(StringTable.IDS_ConeBeam)) : Resources.ResourceManager.GetString("STR_" + Convert.ToString(StringTable.IDS_Scan)));//データモード 1バイト　1:"コーンビーム", 1以外:"スキャン"  'v15.10変更 byやまおか 2009/11/26

                //断画像方向 8バイト 番号を文字化する  1:"下から見た画像", 1以外:"上から見た画像"
                lblContext[IdxDirection].Text = Resources.ResourceManager.GetString("STR_" + (myInfoRec.Data.image_direction == 1 ? StringTable.IDS_DirectionBottom : StringTable.IDS_DirectionTop));
                lblContext[IdxDirection].Text = lblContext[IdxDirection].Text.Replace(" view", "");                 //英語環境対策

                lblContext[IdxFID].Text = (myInfoRec.Data.fid - myInfoRec.Data.fid_offset).ToString("0.0");                   //FID
                lblContext[IdxFCD].Text = (myInfoRec.Data.fcd - myInfoRec.Data.fcd_offset).ToString("0.0");                   //FCD
                lblContext[IdxWindowLevel].Text = myInfoRec.Data.wl.GetString();                                         //ウィンドウレベル
                lblContext[IdxWindowWidth].Text = myInfoRec.Data.ww.GetString();                                         //ウィンドウ幅

                //ガンマ補正値 追加 by長野 2012/02/21
                if ((Convert.ToDouble(myInfoRec.Data.gamma) == 0))
                {
                    myInfoRec.Data.gamma.SetString( "1.00");
                    //ガンマ補正値を持たない旧バージョンの画像は初期値1とする
                }
                lblContext[IdxGamma].Text = Conversion.Val(myInfoRec.Data.gamma.GetString()).ToString("0.000");
                
                lblContext[IdxDataAcq].Text = (myInfoRec.Data.data_acq_time <= 0 ? "" : myInfoRec.Data.data_acq_time.ToString("0.0"));//ﾃﾞｰﾀ収集時間 追加 byやまおか 2007/08/09
                lblContext[IdxReconTime].Text = (myInfoRec.Data.recon_time <= 0 ? "" : myInfoRec.Data.recon_time.ToString("0.0"));//再構成時間   追加 byやまおか 2007/08/09

                if (IdxRFC > 0)
                {
                    lblContext[IdxRFC].Text = myInfoRec.Data.rfc_char.GetString().Trim();//RFC 追加 byやまおか 2007/06/26
                }

                //v19.00 BHC　(電S2)永井
                if (IdxBHCTable > 0)
                {
                    if (myInfoRec.Data.mbhc_flag == 1)
                    {
                        lblContext[IdxBHCTable].Text = modLibrary.AddExtension(modLibrary.RemoveNull(myInfoRec.Data.mbhc_name.GetString()), ".csv");
                    }
                    else
                    {
                        lblContext[IdxBHCTable].Text = "";
                    }
                }


                if (IdxFilterProc > 0)
                {
                    lblContext[IdxFilterProc].Text = myInfoRec.Data.filter_process.GetString().Trim();//フィルタ処理 追加 byやまおか 2007/06/26
                }

                //1画素サイズ(mm)
                if (Conversion.Val(myInfoRec.Data.matsiz.GetString()) == 0)
                {
                    lblContext[IdxPixelSize].Text = "";
                }
                else
                {
                    lblContext[IdxPixelSize].Text = (ScaleValue / Conversion.Val(myInfoRec.Data.matsiz.GetString())).ToString("##0.0000000000");
                }

                //If DetType = DetTypePke Then    'v17.00追加(ここから) byやまおか 2010/02/17
                //v17.30 条件式を追加　by 長野 2010-09-26]
                //v17.00追加(ここから) byやまおか 2010/02/17
                if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) & (myInfoRec.Data.detector == 2))
                {
                    lblContext[IdxFpdGain].Text = (ImageInfo.GetImageInfoVerNumber(myInfoRec.Data.version.GetString()) >= 17 ? modCT30K.GetFpdGainStr(myInfoRec.Data.fpd_gain) : "");//FPDゲイン
                    lblContext[IdxFpdInteg].Text = (ImageInfo.GetImageInfoVerNumber(myInfoRec.Data.version.GetString()) >= 17 ? modCT30K.GetFpdIntegStr(myInfoRec.Data.fpd_integ) : "");//FPD積分時間
                }
                //v17.00追加(ここまで) byやまおか 2010/02/17
                //v16.10 4096対応　scansel.disp_sizeと画像の拡大縮小を、ここで更新する　by 長野 2010/02/16
                UpDateDispSize();

                //v16.10 4096対応  "拡大・縮小"キャプションを更新する by 長野 2010/03/08
                frmScanImage.Instance.UpdateMagnifyCaption();

                //スケールの更新
                UpdateScale();

                // 付帯情報修正中の場合、付帯情報修正フォームを最新の状態にする
                if (modLibrary.IsExistForm(frmPictureRetouch.Instance))
                {
                    //frmPictureRetouch.Instance.LoadImageInfo(myTarget + ".inf");
                    frmPictureRetouch.Instance.Target= myTarget + ".inf";
                }

                #region	    //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                /*
                //骨塩定量測定キャリブROI測定済みフラグのクリア added by 山本 2002-10-19
                modCT30K.GFlg_MaesureCalibRoi = 0;
                */
                #endregion	//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''

            }
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// 「ROI処理あり」を設定する
        /// </summary>
        public void DoRoi()
        {
            // dispinf読み込み
            CTSettings.dispinf.Load();

            // 付帯情報へ書き込み
            myInfoRec.Data.roicaltable_dir = CTSettings.dispinf.Data.d_exam;
            myInfoRec.Data.roical_table.SetString(CTSettings.dispinf.Data.d_id.GetString() + "-ROI.csv");
            myInfoRec.Data.roical = 1;

            Functions.WriteStructure(myTarget + ".inf", myInfoRec);
        }

        /// <summary>
        /// 「プロフィールディスタンス処理あり」を設定する
        /// </summary>
        public void DoPRD()
        {
            // dispinf読み込み
            CTSettings.dispinf.Load();

            // 付帯情報へ書き込み
            myInfoRec.Data.pdtable_dir = CTSettings.dispinf.Data.d_exam;
            myInfoRec.Data.pd_table.SetString(CTSettings.dispinf.Data.d_id.GetString() + "-PRD.csv");
            myInfoRec.Data.pd = 1;

            Functions.WriteStructure(myTarget + ".inf", myInfoRec);
        }
        #endregion

        /// <summary>
        /// 項目を追加する
        /// </summary>
	    private void AddItems()
	    {
		    //スキャン位置
		    IdxScanPos = AddItem(CTResources.LoadResString(StringTable.IDS_ScanPos) + "(mm)", true);

		    //スキャン年月日
            IdxScanDate = AddItem(CTResources.LoadResString(StringTable.IDS_ScanDate), true);

		    //スキャン時刻
            IdxScanTime = AddItem(CTResources.LoadResString(StringTable.IDS_ScanTime), true);

		    //データ収集時間
            IdxDataAcq = AddItem(CTResources.LoadResString(StringTable.IDS_DataAcqTime));

		    //再構成時間
            IdxReconTime = AddItem(CTResources.LoadResString(StringTable.IDS_ReconTime));

		    //管電圧(kV)
            IdxTubeVolt = AddItem(CTResources.LoadResString(StringTable.IDS_TubeVoltage) + "(kV)", true);

		    //管電流(μA)：東芝 EXM2-150の場合はmA
            IdxTubeCurrent = AddItem(CTResources.LoadResString(StringTable.IDS_TubeCurrent) + "(" + modXrayControl.CurrentUni + ")", true);

		    //ビュー数
            IdxViews = AddItem(CTResources.LoadResString(StringTable.IDS_ViewNum), true);

		    //積算枚数
            IdxIntegNum = AddItem(CTResources.LoadResString(StringTable.IDS_IntegNum), true);

		    //I.I.視野：FPDの場合I.I.視野の個所にビニングモードを表示する
		    //v17.30 付帯情報にビニングモードをもっていないので、フラットパネルの場合はI.I.視野サイズ(ビニングモード)は表示しない by 長野 2010-09-26
		    //IdxIIField = AddItem(IIf(Use_FlatPanel, LoadResString(IDS_BinningMode), LoadResString(IDS_IIField)), True)
            if (!CTSettings.detectorParam.Use_FlatPanel)
            {
                IdxIIField = AddItem(CTResources.LoadResString(StringTable.IDS_IIField), true);
		    }

		    //FPDゲイン      'v17.00追加 byやまおか 2010/02/17
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
                IdxFpdGain = AddItem(Resources._20015 + "(pF)");
            }
		    //FPD積分時間      'v17.00追加 byやまおか 2010/02/17
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
                IdxFpdInteg = AddItem(Resources._20016 + "(ms)");
            }
		    //FOV(mm)               
		    IdxFOV = AddItem("FOV(mm)", true);

		    //スライス厚(mm)
            IdxSliceWidth = AddItem(CTResources.LoadResString(StringTable.IDS_SliceWidth) + "(mm)", true);

		    //システム名
            IdxSystemName = AddItem(CTResources.LoadResString(StringTable.IDS_SystemName));

		    //マトリクスサイズ
            IdxMatrix = AddItem(CTResources.LoadResString(StringTable.IDS_Matrix), true);

		    // コメント
            IdxComment = AddItem(CTResources.LoadResString(StringTable.IDS_Comment));

            // コメント項目クリック時処理
            lblContext[IdxComment].Click += (sender, e) =>
            {
                // コメント入力ダイアログ表示
                string comment = frmComment.Dialog("ＣＴ画像に対するコメントを入力してください：",
                                                    StringTable.GetResString(StringTable.IDS_InputCommentOf, this.Text), 
                                                    this.Comment);
                if (comment != null) this.Comment = comment;           
            };

		    //スキャンモード
            IdxScanMode = AddItem(CTResources.LoadResString(StringTable.IDS_ScanMode), true);

		    //フィルタ関数
            IdxFilterFunc = AddItem(CTResources.LoadResString(StringTable.IDS_FilterFunc));

		    //フィルタ処理
            if ((CTSettings.scaninh.Data.filter_process[0] == 0) && (CTSettings.scaninh.Data.filter_process[1] == 0))
            {
                IdxFilterProc = AddItem(CTResources.LoadResString(StringTable.IDS_FilterProc));
		    }

		    //RFC
            if (CTSettings.scaninh.Data.rfc == 0)
            {
                IdxRFC = AddItem(CTResources.LoadResString(StringTable.IDS_RFC));
		    }

            //v19.00 BHC (電S2)永井
            if (CTSettings.scaninh.Data.mbhc == 0)
            {
                //BHC有効
                IdxBHCTable = AddItem(CTResources.LoadResString(StringTable.IDS_BHCFile));
            }

            //画像バイアス
            IdxImageBias = AddItem(CTResources.LoadResString(StringTable.IDS_ImageBias));

		    //画像スロープ
            IdxImageSlope = AddItem(CTResources.LoadResString(StringTable.IDS_ImageSlope));

		    //データモード       'v15.10追加 byやまおか 2009/11/26
            IdxDataMode = AddItem(CTResources.LoadResString(StringTable.IDS_DataMode), true);

		    //断面像方向
            IdxDirection = AddItem(CTResources.LoadResString(StringTable.IDS_ImageDirection));

		    //FID
            IdxFID = AddItem(CTSettings.gStrFidOrFdd);

		    //FCD
            IdxFCD = AddItem(CTResources.LoadResString(StringTable.IDS_FCD));

		    //ウィンドウレベル
            IdxWindowLevel = AddItem(CTResources.LoadResString(StringTable.IDS_WindowLevel));

		    //ウィンドウ幅
            IdxWindowWidth = AddItem(CTResources.LoadResString(StringTable.IDS_WindowWidth));

            //ガンマ補正値 '19.00追加 by長野 2012/02/21
            IdxGamma = AddItem(CTResources.LoadResString(StringTable.IDS_Gamma));

		    //1画素サイズ(mm)
            IdxPixelSize = AddItem(CTResources.LoadResString(StringTable.IDS_PixelSize));

		    //拡大倍率
            IdxMagnify = AddItem(CTResources.LoadResString(StringTable.IDS_Magnification));

		    // スケール
            IdxScale = AddItem(CTResources.LoadResString(StringTable.IDS_Scale));

            // スケール項目変更時処理
            lblContext[IdxScale].TextChanged += (sender, e) =>
            {
                // スケール値
                double scale = Conversion.Val(lblContext[IdxScale].Text);

                // 拡大倍率の更新
                lblContext[IdxMagnify].Text = (scale == 0) ? "" : (CTSettings.scancondpar.Data.magnify_para / scale).ToString("##0.00000");
            };
	    }

        /// <summary>
        /// 項目を追加する
        /// </summary>
        /// <param name="itemString">項目名</param>
        /// <returns></returns>
	    private int AddItem(string itemString)
	    {
            return AddItem(itemString, false);
        }

        /// <summary>
        /// 項目を追加する
        /// </summary>
        /// <param name="itemString">項目名</param>
        /// <param name="isDetail"></param>
        /// <returns></returns>
	    private int AddItem(string itemString, bool isDetail)
        {
            // ラベルコントロール生成
            const int ITEM_LEFT = 8;
            const int COLON_LEFT = 108;
            const int TEXT_LEFT = 118;

            // 追加項目の位置
          
            int top = (lblItem.Count > 0) ? lblItem[lblItem.Count - 1].Bottom + 1: 8;

            int index = lblItem.Count;

            //項目名用ラベル
            {
                lblItem.Add(new Label());
                lblItem[index].Left = ITEM_LEFT;
                lblItem[index].Top = top;
                lblItem[index].Text = itemString;                   // 項目名用ラベルに項目名をセット
                lblItem[index].Tag = Convert.ToString(isDetail);    
                lblItem[index].AutoSize = true; 
                this.Controls.Add(lblItem[index]);
            }

            //コロン用ラベル
            {
                lblColon.Add(new Label());
                lblColon[index].Left = COLON_LEFT;
                lblColon[index].Top = top;
                lblColon[index].Text = ":";                   
                lblColon[index].AutoSize = true;
                this.Controls.Add(lblColon[index]);
            }

            //項目内容用ラベル
            {
                lblContext.Add(new Label());
                lblContext[index].Left = TEXT_LEFT;
                lblContext[index].Top = top;
                lblContext[index].AutoSize = true;
                this.Controls.Add(lblContext[index]);
            }

            // 戻り値セット
            return index;
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpDateDispSize()
        {
            // scansel 読み込み
            CTSettings.scansel.Load();

            //今まで表示していた画像が、拡大か縮小かを確認する。
            if (frmScanImage.Instance.hsbImage.Visible)
            {
                // 今まで表示していた画像が拡大の場合は、表示しようとしている画像も拡大で表示させるため、マトリクスサイズに従い、disp_sizeを決める。
                switch (this.Matrix)
                {
                    case 2048:
                        CTSettings.scansel.Data.disp_size = 1;
                        break;

                    case 4096:
                        CTSettings.scansel.Data.disp_size = 2;
                        break;

                    default:
                        CTSettings.scansel.Data.disp_size = 0;
                        break;
                }
            }
            else
            {
                // 今まで表示していた画像が縮小だった場合、表示しようとしている画像も縮小で表示する。
                CTSettings.scansel.Data.disp_size = 0;
            }

            // scansel 書き込み
            CTSettings.scansel.Restore();
        }

        /// <summary>
        /// スケールの更新
        /// </summary>
        public void UpdateScale()
        {
            if (frmScanImage.Instance.mnuEnlarge.Enabled)
            {
                //scansel.disp_sizeを見て，4096と2048の場合のスケールを変更する。
                switch (CTSettings.scansel.Data.disp_size)
                {
                    case 0:
                        lblContext[IdxScale].Text = Convert.ToString(ScaleValue / 10);
                        break;

                    case 1:
                        lblContext[IdxScale].Text = Convert.ToString(ScaleValue / 10 / 2);
                        break;

                    case 2:
                        lblContext[IdxScale].Text = Convert.ToString(ScaleValue / 10 / 4);
                        break;
                }
            }
            else
            {
                lblContext[IdxScale].Text = Convert.ToString(ScaleValue / 10);
            }
        }

        //*******************************************************************************
        //機　　能： 英語用レイアウト調整
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v17.60 2011/05/25 (検S1)長野   新規作成
        //*******************************************************************************
		private void EnglishAdjustLayout()
		{
			lblItem[1].Left = 3;
			lblColon[1].Left = lblColon[1].Left + 9;
			lblContext[1].Left = lblContext[1].Left + 9;
			cmdDetailMode.Height = 30;
		}

    }
}