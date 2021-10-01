using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{
    ///* *************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver9.7                */
    ///* 客先　　　　： ?????? 殿                                                    */
    ///* プログラム名： frmGainCorResult.frm                                         */
    ///* 処理概要　　： ゲイン校正結果                                               */
    ///* 注意事項　　：                                                              */
    ///* --------------------------------------------------------------------------- */
    ///* ＯＳ　　　　： Windows XP Professional (SP1)                                */
    ///* コンパイラ　： VB 6.0 (SP5)                                                 */
    ///* --------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                  */
    ///*                                                                             */
    ///* V1.00       99/XX/XX    (TOSFEC) ????????                                   */
    ///* V2.0        00/02/08    (TOSFEC) 鈴山　修   V1.00を改造                     */
    ///* V3.0        00/08/01    (TOSFEC) 鈴山　修   ｺｰﾝﾋﾞｰﾑCT対応                   */
    ///* V9.7        04/11/01    (SI4)間々田         階調変換オブジェクトを使用      */
    ///*                                                                             */
    ///* --------------------------------------------------------------------------- */
    ///* ご注意：                                                                    */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。    */
    ///*                                                                             */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2004                  */
    ///* *************************************************************************** */
    public partial class frmGainCorResult : Form
    {
        private RadioButton[] optScale = new RadioButton[3];
        private BitmapImageControl BmpICtrl; 


        //public event TransImageChangedEventHandler TransImageChanged;
        //public delegate void TransImageChangedEventHandler();

        //透視画像が変更された
        //public event CaptureOnOffChangedEventHandler CaptureOnOffChanged;
        //キャプチャオンオフ変更時イベント
        //public delegate void CaptureOnOffChangedEventHandler(bool IsOn);

        #region インスタンスを返すプロパティ

        // frmGainCorResultのインスタンス
        private static frmGainCorResult _Instance = null;

        /// <summary>
        /// frmGainCorResultのインスタンスを返す
        /// </summary>
        public static frmGainCorResult Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmGainCorResult();
                }

                return _Instance;
            }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmGainCorResult()
        {
            InitializeComponent();

            #region コントロールの貼り付け

            this.SuspendLayout();

            for (int i = 0; i < optScale.Length; i++)
            {
                this.optScale[i] = new RadioButton();
                this.optScale[i].AutoSize = true;
                this.optScale[i].Font = new Font("ＭＳ ゴシック", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(128)));
                this.optScale[i].Location = new Point(16, 20 + i * 20);
                this.optScale[i].Name = "optScale" + i.ToString();
                this.optScale[i].Size = new Size(60, 17);
                this.optScale[i].TabIndex = i + 1;
                this.optScale[i].TabStop = true;
                switch (i)
                {
                    case 0:
                        this.optScale[i].Text = " 1 倍";
                        break;
                    case 1:
                        this.optScale[i].Text = " 4 倍";
                        break;
                    case 2:
                        this.optScale[i].Text = " 16 倍";
                        break;
                    default:
                        break;
                }
                this.optScale[i].UseVisualStyleBackColor = true;
                this.optScale[i].CheckedChanged += new EventHandler(this.optScale_CheckedChanged);
                this.fraScale.Controls.Add(this.optScale[i]);
            }

            BmpICtrl= new BitmapImageControl();
            BmpICtrl.MirrorOn = false;
            BmpICtrl.SetLTSize(LookupTableSize.LT12Bit);
            BmpICtrl.WindowLevel = 2048;
            BmpICtrl.WindowWidth = 4096;

            ctlTransImage.MirrorOn = BmpICtrl.MirrorOn;

            this.ResumeLayout(false);

            #endregion
        }

        #endregion

        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************
        //入力結果
        bool OK;

        //*******************************************************************************
        //機　　能： ウィンドウレベルスライダー変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cwsldLevel_PointerValueChanged(object sender, EventArgs e)
		{
			//値をラベルに表示
            lblLevel.Text = Convert.ToString(cwsldLevel.Value);

			//階調変換を実行
            //ctlTransImage.WindowLevel = cwsldLevel.Value;
            BmpICtrl.WindowLevel = cwsldLevel.Value;

            ctlTransImage.Picture = BmpICtrl.Picture;
            //ctlTransImage.Invalidate();
            ctlTransImage.Refresh();
        }

        //*******************************************************************************
        //機　　能： ウィンドウ幅スライダー変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cwsldWidth_PointerValueChanged(object sender, EventArgs e)
		{
			//値をラベルに表示
            lblWidth.Text = Convert.ToString(cwsldWidth.Value);

			//階調変換を実行
            //ctlTransImage.WindowWidth = cwsldWidth.Value;
            BmpICtrl.WindowWidth = cwsldWidth.Value;

            //描画
            ctlTransImage.Picture = BmpICtrl.Picture;
            //ctlTransImage.Invalidate();
            ctlTransImage.Refresh();
        }

        //*******************************************************************************
        //機　　能： 倍率オプションボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Index           [I/ ] 型        1:１倍 2:４倍 3:16倍
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void optScale_CheckedChanged(object sender, EventArgs e)
		{
            int Index = -1; // 選択したラジオボタンのインデックス番号
            for (int i = 0; i < optScale.Length; i++)
            {
                if (sender.Equals(optScale[i]))
                {
                    Index = i;
                    break;
                }
            }

			//スクロールバーの最大値の調整
			//cwsldWidth.Axis.Maximum = Choose(Index + 1, 256, 1024, 4096)   'v17.00削除 byやまおか 2010/01/20
            switch (CTSettings.detectorParam.DetType)
            {
				//v17.00追加(ここから) byやまおか 2010/01/20
				case DetectorConstants.DetTypeII:
				case DetectorConstants.DetTypeHama:
                    if (Index + 1 == 1)
                    {
                        cwsldWidth.Maximum = 256;
                    }
                    else if (Index + 1 == 2)
                    {
                        cwsldWidth.Maximum = 1024;
                    }
                    else if (Index + 1 == 3)
                    {
                        cwsldWidth.Maximum = 4096;
                    }
                    break;
				case DetectorConstants.DetTypePke:
					//cwsldWidth.Axis.Maximum = Choose(Index + 1, 2048, 8192, 16384)  '変更　山本　2009-10-09
                    if (Index + 1 == 1)
                    {
                        cwsldWidth.Maximum = 4096;
                    }
                    else if (Index + 1 == 2)
                    {
                        cwsldWidth.Maximum = 16384;
                    }
                    else if (Index + 1 == 3)
                    {
                        cwsldWidth.Maximum = 65536;//v17.02変更 byやまおか 2010-06-14
                    }
                    break;
                default:
                    break;
			}
			//v17.00追加(ここまで) byやまおか 2010/01/20
			cwsldLevel.Maximum = cwsldWidth.Maximum - 1;

            //削除2014/12/15hata
            //WLMin.Text = Convert.ToString(cwsldLevel.Minimum);
            //WLMax.Text = Convert.ToString(cwsldLevel.Maximum);
            //WDMin.Text = Convert.ToString(cwsldWidth.Minimum);
            //WDMax.Text = Convert.ToString(cwsldWidth.Maximum);

            ////追加2014/11/28hata_v19.51_dnet
            //WDMax.Left = cwsldWidth.Right - WDMax.Width;
            //WLMax.Left = cwsldLevel.Right - WLMax.Width;

			//Index値を記憶
			modCT30K.FimageBitIndex = Index;

        }

        //*******************************************************************************
        //機　　能： 「はい」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void cmdOK_Click(object sender, EventArgs e)
		{
            int ret = 0;
            
            //マウスポインタを砂時計にする   'v17.00追加　山本 2010-02-03
			this.Cursor = Cursors.WaitCursor;

			//押したボタンの種類を通知
			OK = true;

			//ゲイン校正画像の保存
			//ImageSave Gain_Image(0), GAIN_CORRECT, h_size, v_size  'v17.00削除 byやまおか 2010/01/20
            switch (CTSettings.detectorParam.DetType)
            {
				case DetectorConstants.DetTypeII:
				case DetectorConstants.DetTypeHama:
                    ScanCorrect.ImageSave(ref ScanCorrect.GAIN_IMAGE[0], ScanCorrect.GAIN_CORRECT, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
					break;
				case DetectorConstants.DetTypePke:
					//ScanCorrect.ImageSave_long(ref ScanCorrect.Gain_Image_L[0], ScanCorrect.GAIN_CORRECT_L, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);    //変更　山本　2009-10-19
                    //変更2014/10/07hata_v19.51反映
                    //IICorrect.ImageSave_long(ref ScanCorrect.Gain_Image_L[0], ScanCorrect.GAIN_CORRECT_L, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                    //v18.00変更 byやまおか 2011/02/12 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    //if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift))
                    //Rev23.20 左右シフト対応 by長野 2015/11/19
                    if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_R))
                    {
                        IICorrect.ImageSave_long(ref ScanCorrect.Gain_Image_L_SFT_R[0], ScanCorrect.GAIN_CORRECT_L_SFT_R, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                    }
                    else if((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_L))
                    {
                        IICorrect.ImageSave_long(ref ScanCorrect.Gain_Image_L_SFT_L[0], ScanCorrect.GAIN_CORRECT_L_SFT_L, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                    }
                    else
                    {
                        IICorrect.ImageSave_long(ref ScanCorrect.Gain_Image_L[0], ScanCorrect.GAIN_CORRECT_L, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
                    }

                    //Rev25.00 左右ゲインの平均値調整 by長野 2016/09/24
                    if (modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_L)
                    {
                        //Rev25.00 関数化 by長野 2016/12/16
                        ScanCorrect.calShiftImageMagVal();
                    }

                    //変更2014/10/07hata_v19.51反映
#if (!NoCamera) //'v17.02変更 byやまおか 2010/07/15
                    //ret = Pulsar.PkeSetGainData(Pulsar.hPke, 1, ScanCorrect.Gain_Image_L, 1); // 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    //if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift))
                    //Rev23.20 左右シフト対応 by長野 2015/11/19
                    if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_R) || (modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_L))
                    {
                        //収集完了したら基準位置に戻るため
                        //シフトスキャン収集のときは検出器にセットしない
                        //ret = PkeSetGainData(hPke, 1, Gain_Image_L_SFT(0), 1)
                    }
                    else
                    {
                        ret = Pulsar.PkeSetGainData(Pulsar.hPke, ScanCorrect.Gain_Image_L, 1, "");
                    }

                    //ストリングテーブル化　'v17.60 by 長野　2011/05/22
                    ////If ret = 1 Then MsgBox "ゲイン校正データをセットできませんでした。", vbCritical
                    //if (Ipc32v5.ret == 1)
                    if (ret == 1)
                    {
                        MessageBox.Show(CTResources.LoadResString(20004), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
#endif
					
                    break;
                default:
                    break;
			}

			//メカの移動有無プロパティをリセットする
			modSeqComm.SeqBitWrite("GainIIChangeReset", true);

			//mecainf（コモン）の更新
			UpdateMecainf();

			//フォームを消去
			//変更2015/1/17hata_非表示のときにちらつくため
            //Hide();
            modCT30K.FormHide(this);
        }

        //*******************************************************************************
        //機　　能： 「いいえ」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void cmdEnd_Click(object sender, EventArgs e)
		{
            int ret = 0;

			//フォームを消去
			//変更2015/1/17hata_非表示のときにちらつくため
            //Hide();
            modCT30K.FormHide(this);

			//パーキンエルマーFPDかつ自動校正中の場合はゲイン校正画像をファイルからプリロードする（自動校正中にゲイン校正画像をプリロードしたためキャンセルした場合は元に戻す） v17.00追加 by　山本 2010-03-06
			//If AutoCorFlag = 1 And DetType = DetTypePke Then    '
			//v17.02修正 自動じゃない場合もプリロードする byやまおか 2010/07/06
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
				//元のゲイン校正画像をプリロードする
                //変更2014/10/07hata_v19.51反映
                //ret = Pulsar.PkeSetGainData(Pulsar.hPke, 0, ScanCorrect.Gain_Image_L, 1);                
                //if (Convert.ToBoolean(modDetShift.IsDetShiftPos == modDetShift.DetShiftConstants.DetShift_forward) || 
                //    Convert.ToBoolean(modDetShift.IsDetShiftPos == modDetShift.DetShiftConstants.DetShift_backward))    //v18.00変更 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                //{
                //    ret = Pulsar.PkeSetGainData(Pulsar.hPke, ScanCorrect.Gain_Image_L, 1, ScanCorrect.GAIN_CORRECT_L_SFT);
                //}
                //Rev23.20 左右シフト対応 by長野 2015/11/19
                if (Convert.ToBoolean(modDetShift.IsDetShiftPos == modDetShift.DetShiftConstants.DetShift_forward))
                {
                    ret = Pulsar.PkeSetGainData(Pulsar.hPke, ScanCorrect.Gain_Image_L, 1, ScanCorrect.GAIN_CORRECT_L_SFT_R);
                }
                else if(Convert.ToBoolean(modDetShift.IsDetShiftPos == modDetShift.DetShiftConstants.DetShift_backward))    //v18.00変更 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                {
                    ret = Pulsar.PkeSetGainData(Pulsar.hPke, ScanCorrect.Gain_Image_L, 1, ScanCorrect.GAIN_CORRECT_L_SFT_L);
                }
                else
                {
                    ret = Pulsar.PkeSetGainData(Pulsar.hPke, ScanCorrect.Gain_Image_L, 1, ScanCorrect.GAIN_CORRECT_L);
                }
                
                //ストリングテーブル化　'v17.60 by 長野　2011/05/22
				////If ret = 1 Then MsgBox "ゲイン校正データをセットできませんでした。", vbCritical
                //if (Ipc32v5.ret == 1)
                if (ret == 1)
                {
                    MessageBox.Show(CTResources.LoadResString(20004), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
			}
		}

        //*******************************************************************************
        //機　　能： フォームロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void frmGainCorResult_Load(object sender, EventArgs e)
		{
            try
            {
                //キャプションのセット
                SetCaption();

                //コントロールの初期化
                InitControls();

                //倍率の設定
                //SetOption optScale, FimageBitIndex
                modLibrary.SetOption(optScale, (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke ? 2 : modCT30K.FimageBitIndex));    //v17.10変更 byやまおか 2010/08/31

                //ウィンドウレベル・ウィンドウ幅のセット
                //cwsldLevel.Value = frmScanControl.WindowLevel
                //cwsldWidth.Value = frmScanControl.WindowWidth
                //v17.02変更 byやまおか 2010/07/06
                if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
                {
                    //Pkeは条件によらずゲイン値がほぼ一定(70000程度の値を÷1.5して表示)なのでWL/WWを固定にする   'v17.10変更 byやまおか 2010/08/31
                    cwsldLevel.Value = 32767;
                    cwsldWidth.Value = 65536;
                }
                else
                {
                    //Pke以外は透視画像のWL/WWを反映する
                    cwsldLevel.Value = frmScanControl.Instance.WindowLevel;
                    cwsldWidth.Value = frmScanControl.Instance.WindowWidth;
                }

                //上記の処理ではPointerValueChangedイベントは発生しないので以下の処理を行なう
                //cwsldLevel_PointerValueChanged(cwsldLevel, new AxCWUIControlsLib._DCWSlideEvents_PointerValueChangedEvent(0, (cwsldLevel.Value)));
                //cwsldWidth_PointerValueChanged(cwsldWidth, new AxCWUIControlsLib._DCWSlideEvents_PointerValueChangedEvent(0, (cwsldWidth.Value)));
                cwsldLevel_PointerValueChanged(cwsldLevel, new EventArgs());
                cwsldWidth_PointerValueChanged(cwsldWidth, new EventArgs());

                //ゲイン結果表示用配列にコピー
                ushort[] DispGAIN_IMAGE = null;                //ゲイン結果表示用画像データ配列
                DispGAIN_IMAGE = new ushort[ScanCorrect.GAIN_IMAGE.GetUpperBound(0) + 1];
                //v17.00削除 byやまおか 2010/01/20　　復活　山本 2010-03-03
                //DispGAIN_IMAGE = IIf(DetType = DetTypePke, TransImage, GAIN_IMAGE)  'v17.00追加 byやまおか 2010/02/16

                //変更2014/10/07hata_v19.51反映
                //ScanCorrect.GAIN_IMAGE.CopyTo(DispGAIN_IMAGE, 0);
                //v18.00変更 byやまおか 2011/02/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                //Rev23.20 左右シフト対応 by長野 2015/11/19
                //if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift))
                if((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_R))
                {
                    //ScanCorrect.GAIN_IMAGE_SFT.CopyTo(DispGAIN_IMAGE, 0);
                    //Rev23.20 左右シフト対応 by長野 2015/11/19
                    ScanCorrect.GAIN_IMAGE_SFT_R.CopyTo(DispGAIN_IMAGE, 0);
                }
                else if((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_L))
                {
                    ScanCorrect.GAIN_IMAGE_SFT_L.CopyTo(DispGAIN_IMAGE, 0);
                }
                else
                {
                    ScanCorrect.GAIN_IMAGE.CopyTo(DispGAIN_IMAGE, 0);
                }

                //表示するゲイン校正画像に対して欠陥補正を行う
                //If Use_FlatPanel Then Call FpdDefCorrect_short(DispGAIN_IMAGE(0), Def_IMAGE(0), h_size, v_size, 0, v_size - 1)             'v17.00deleted by 山本 2009-09-18
                if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypeHama))
                {
                    ScanCorrect.FpdDefCorrect_short(ref DispGAIN_IMAGE[0], ref ScanCorrect.Def_IMAGE[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, 0, CTSettings.detectorParam.v_size - 1);
                }   //v17.00追加 byやまおか 2010/01/20

                //変換対象となる画像の配列を登録
                //変更2014/10/07hata_v19.51反映
                //BmpICtrl.SetImage(ScanCorrect.GAIN_IMAGE);
                //if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift)) //v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                 //Rev23.20 左右シフト対応 by長野 2015/11/19
                if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_R)) 
                {
                    BmpICtrl.SetImage(ScanCorrect.GAIN_IMAGE_SFT_R);
                }
                else if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_L))
                {
                    BmpICtrl.SetImage(ScanCorrect.GAIN_IMAGE_SFT_L);
                }
                else
                {
                    BmpICtrl.SetImage(ScanCorrect.GAIN_IMAGE);
                }
                
                ctlTransImage.Picture = BmpICtrl.Picture;
                ctlTransImage.Refresh();

            }
            catch (Exception exp)
            {
                //エラー時の扱い
                MessageBox.Show(exp.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
			//return; //v17.00追加 byやまおか 2010/01/20
		}

        //*******************************************************************************
        //機　　能： コントロールの初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void InitControls()
		{
            int mod_SizeX = 0;  //v17.00追加 byやまおか 2010/02/24
            int mod_SizeY = 0;  //v17.00追加 byやまおか 2010/02/24
            
            //透視画像表示コントロール
            ctlTransImage.SizeX = CTSettings.detectorParam.h_size;
            ctlTransImage.SizeY = CTSettings.detectorParam.v_size;
            //2014/11/06hata キャストの修正
            ctlTransImage.Width = Convert.ToInt32(CTSettings.detectorParam.h_size / CTSettings.detectorParam.phm);
            ctlTransImage.Height = Convert.ToInt32(CTSettings.detectorParam.v_size / CTSettings.detectorParam.pvm);
            BmpICtrl.ImageSize = new Size(CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
            
            //PkeFPDの場合は左上にずらす(額縁をはみ出させる) 'v17.00追加 byやまおか 2010/02/24
            //If DetType = DetTypePke Then
            //v17.22変更 byやまおか 2010/10/19
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) && (!CTSettings.detectorParam.Use_FpdAllpix))
            {

                //Rev20.00 追加 by長野 2014/12/04
                BmpICtrl.MirrorOn = true;
                BmpICtrl.SetLTSize(LookupTableSize.LT16Bit);
                BmpICtrl.WindowLevel = 32768;
                BmpICtrl.WindowWidth = 65536;

                ctlTransImage.MirrorOn = BmpICtrl.MirrorOn;
                
                //画像サイズの端数(2048なら48)   'v17.10移動 byやまおか 2010/08/31
                //2014/11/06hata キャストの修正
                mod_SizeX = Convert.ToInt32((ctlTransImage.SizeX % 100) / CTSettings.detectorParam.phm);    //v17.00追加 byやまおか 2010/02/24
                mod_SizeY = Convert.ToInt32((ctlTransImage.SizeY % 100) / CTSettings.detectorParam.pvm);    //v17.00追加 byやまおか 2010/02/24

                //.Left = .Left - mod_SizeY / 2
                //.Top = .Top - mod_SizeY / 2
                //2014/11/06hata キャストの修正
                ctlTransImage.Left = Convert.ToInt32(-mod_SizeX / 2F);    //v17.10変更 byやまおか 2010/08/19
                ctlTransImage.Top = Convert.ToInt32(-mod_SizeY / 2F);     //v17.10変更 byやまおか 2010/08/19
            }
            else	//v17.10 Else追加 byやまおか 2010/08/31
            {
                //Rev20.00 追加 by長野 2014/12/04
                BmpICtrl.MirrorOn = false;
                BmpICtrl.SetLTSize(LookupTableSize.LT12Bit);
                BmpICtrl.WindowLevel = 2048;
                BmpICtrl.WindowWidth = 4096;
                ctlTransImage.MirrorOn = BmpICtrl.MirrorOn;

                //画像サイズの端数は気にしない
                mod_SizeX = 0;
                mod_SizeY = 0;

                //位置はずらさない   'v17.10変更 byやまおか 2010/08/31
                ctlTransImage.Left = 0;
                ctlTransImage.Top = 0;
            }

            //フォーム
            //Me.width = Screen.TwipsPerPixelX * (MaxVal(.width, fraControl.width) + 4)
            //Me.Height = Screen.TwipsPerPixelY * (.Height + fraControl.Height + 25)
            //PkeFPDの場合は一回り小さくなる 'v17.10追加 byやまおか 2010/08/31
            this.Width = modLibrary.MaxVal(ctlTransImage.Width, fraControl.Width) + 4 - mod_SizeX;
            this.Height = ctlTransImage.Height + fraControl.Height + 25 - mod_SizeY;

            //追加2014/11/28hata_v19.51_dnet
            //topの位置を設定する
            if ((this.Top + this.Height) > Screen.PrimaryScreen.Bounds.Height)
            {
                int mytop = Screen.PrimaryScreen.Bounds.Height - frmCTMenu.Instance.Height;     
                int top = (Screen.PrimaryScreen.Bounds.Height - this.Height)/2;
                if (top -mytop <= 0) top = 0;
                this.Top = top;
            }

            //コントロールフレーム
            //fraControl.Move ScaleWidth / 2 - fraControl.width / 2, .Height
            //PkeFPDの場合はコントロールフレームでゲイン画像の下端を隠す     'v17.00変更 byやまおか 2010/03/02
            if (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke)
            {
                //2014/11/06hata キャストの修正
                fraControl.SetBounds(0, (ctlTransImage.Top) + (ctlTransImage.Height) - Convert.ToInt32(mod_SizeY / 2F), (ctlTransImage.Width), 0, BoundsSpecified.X | BoundsSpecified.Y | BoundsSpecified.Width);
            }
            else
            {
                //2014/11/06hata キャストの修正
                fraControl.SetBounds(Convert.ToInt32(ClientRectangle.Width / 2F - fraControl.Width / 2F), ctlTransImage.Height, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            }

            //倍率のフレーム
            fraScale.Visible = (CTSettings.scancondpar.Data.fimage_bit == 2) & CTSettings.detectorParam.Use_FlatPanel;
		}

        //*******************************************************************************
        //機　　能： 各コントロールのキャプションに文字列をセットする
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void SetCaption()
		{
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

            //変更2014/10/07hata_v19.51反映
            //this.Text = StringTable.BuildResStr(StringTable.IDS_Result, StringTable.IDS_CorGain);   //ゲイン校正結果
            //v18.00変更 byやまおか 2011/07/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            string mycaption = null;
            //Rev23.20 左右シフト対応 by長野 2015/11/19
            //if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift))
            if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_R))
            {
                mycaption = StringTable.BuildResStr(StringTable.IDS_Result, StringTable.IDS_CorGain) + " - " + CTResources.LoadResString(12408) + (CTSettings.W_ScanOn == true ? "W" : CTResources.LoadResString(StringTable.IDS_Shift)) + CTResources.LoadResString((StringTable.IDS_ScanCollection));
                //ゲイン校正結果 - 右シフトスキャン収集
            }
            //Rev23.20 左右シフト対応 by長野 2015/11/19
            else if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_L))
            {
                mycaption = StringTable.BuildResStr(StringTable.IDS_Result, StringTable.IDS_CorGain) + " - " + CTResources.LoadResString(12407) + (CTSettings.W_ScanOn == true ? "W" : CTResources.LoadResString(StringTable.IDS_Shift)) + CTResources.LoadResString((StringTable.IDS_ScanCollection));
                //ゲイン校正結果 - 左シフトスキャン収集
            }
            else
            {
                mycaption = StringTable.BuildResStr(StringTable.IDS_Result, StringTable.IDS_CorGain);
                //ゲイン校正結果
            }
          
            this.Text = mycaption;

			//optScale(0).Caption = GetResString(IDS_Times, " 1") ' 4 倍 → 1 倍に変更 by 間々田 2005/01/07
			//optScale(1).Caption = GetResString(IDS_Times, " 4") ' 8 倍 → 4 倍に変更 by 間々田 2005/01/07
			//optScale(2).Caption = GetResString(IDS_Times, "16") '16 倍
			optScale[0].Text = " 1 / 16";   //v17.10変更 byやまおか 2010/08/26
			optScale[1].Text = " 1 / 4";    //v17.10変更 byやまおか 2010/08/26
			optScale[2].Text = " 1 / 1";    //v17.10変更 byやまおか 2010/08/26
		}

        //*******************************************************************************
        //機　　能： ダイアログ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [ /O] Boolean   True:「はい」・False:「いいえ」
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        //public bool Dialog()   //変更2014/10/07hata_v19.51反映
        public bool Dialog(modScanCorrect.ModeCorConstants Mode = modScanCorrect.ModeCorConstants.ModeCor_origin)
        {
			bool functionReturnValue = false;

			//戻り値用変数初期化
			OK = false;

             //追加2014/10/07hata_v19.51反映
            //モードをセット 'v18.00追加 byやまおか 2011/02/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            modScanCorrect.Mode_GainCor = Mode;

			//モーダル表示
            //変更2014/12/22hata_dNet_オーナーフォームを指定する
            //ShowDialog();
            ShowDialog(frmCTMenu.Instance);

			//戻り値セット
			functionReturnValue = OK;

			//アンロード
			this.Close();

			return functionReturnValue;
		}

        //*******************************************************************************
        //機　　能： mecainf（コモン）の更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： ゲイン校正関連のパラメータのみ更新
        //
        //履　　歴： v11.2  06/01/10  (SI3)間々田    新規作成
        //*******************************************************************************
		private void UpdateMecainf()
		{
			//mecainfType theMecainf = default(mecainfType);
            MecaInf theMecainf = new MecaInf();
            theMecainf.Data.Initialize();

			//mecainf（コモン）取得
			//modMecainf.GetMecainf(ref theMecainf);
            theMecainf.Load();


			//ゲイン校正を行ったときの管電圧
			//.gain_kv = scansel.scan_kv
			//v15.10条件追加 byやまおか 2009/10/29
            if (CTSettings.scaninh.Data.xray_remote == 0)
            {
                //変更2014/10/07hata_v19.51反映
                //theMecainf.Data.gain_kv = (float)frmXrayControl.Instance.ntbSetVolt.Value;  //v15.0変更 by 間々田 2009/04/10
                //検出器シフトに対応     'v18.00変更 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                //if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift))
                //Rev23.20 左右シフト対応 by長野 2015/11/19
                if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_R) || (modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_L))
                {
                    //2014/11/06hata キャストの修正
                    theMecainf.Data.gain_kv_sft = Convert.ToInt32(frmXrayControl.Instance.ntbSetVolt.Value);                    //シフト用
                }
                else
                {
                    theMecainf.Data.gain_kv = (float)frmXrayControl.Instance.ntbSetVolt.Value;
                }
            }

			//'ゲイン校正を行ったときの管電流
			//.gain_ma = scansel.scan_ma                 'v15.0削除 by 間々田 2009/03/18 未使用のため

            theMecainf.Data.gain_ma = (float)modScanCorrectNew.GainCurrent;//v19.00復活（開放管はターゲット電流、密封管はフィードバック値とする） by長野 2012/05/10

			//ゲイン校正を行ったときのフィルタ
            //変更2014/10/07hata_v19.51反映
            //theMecainf.Data.gain_filter = modSeqComm.GetFilterIndex();
            //検出器シフトに対応     'v18.00変更 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev23.20 左右シフト対応 by長野 2015/11/19 
            //if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift))
            if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_R) || (modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_L))
            {
                theMecainf.Data.gain_filter_sft = modSeqComm.GetFilterIndex();
                //シフト用
            }
            else
            {
                theMecainf.Data.gain_filter = modSeqComm.GetFilterIndex();
            }

			//ゲイン校正を行ったときのI.I.視野
            //変更2014/10/07hata_v19.51反映
            //theMecainf.Data.gain_iifield = modSeqComm.GetIINo();
            //検出器シフトに対応     'v18.00変更 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift))
            //Rev23.20 左右シフト対応 by長野 2015/11/19
            if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_R) || (modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_L))
            {
                theMecainf.Data.gain_iifield_sft = modSeqComm.GetIINo();                //シフト用
            }
            else
            {
                theMecainf.Data.gain_iifield = modSeqComm.GetIINo();
            }

			//ゲイン校正を行ったときのＸ線管
            //変更2014/10/07hata_v19.51反映
            //theMecainf.Data.gain_mt = CTSettings.scansel.Data.multi_tube;
            //検出器シフトに対応     'v18.00変更 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift))
            //Rev23.20 左右シフト対応 by長野 2015/11/19
            if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_R) || (modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_L))
            {
                theMecainf.Data.gain_mt_sft = CTSettings.scansel.Data.multi_tube;                //シフト用
            }
            else
            {
                theMecainf.Data.gain_mt = CTSettings.scansel.Data.multi_tube;
            }

			//ゲイン校正を行ったときのビニングモード：0(1×1)，1(2×2)，2(4×4)
            //変更2014/10/07hata_v19.51反映
            //theMecainf.Data.gain_bin = CTSettings.scansel.Data.binning;
            //検出器シフトに対応     'v18.00変更 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev23.20 左右シフト対応 by長野 2015/11/19
            //if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift))
            if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_R) || (modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_L))
            {
                theMecainf.Data.gain_bin_sft = CTSettings.scansel.Data.binning;
                //シフト用
            }
            else
            {
                theMecainf.Data.gain_bin = CTSettings.scansel.Data.binning;
            }

            //ゲイン校正を行った時の年月日               'v12.01追加 by 間々田 2006/12/04
            //変更2014/10/07hata_v19.51反映
            //theMecainf.Data.gain_date = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));  //YYYYMMDD形式
            //検出器シフトに対応     'v18.00変更 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift))
            //Rev23.20 左右シフト対応 by長野 2015/11/19
            if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_R) || (modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_L))
            {
                //theMecainf.Data.gain_date_sft = Convert.ToInt32(DateTime.Now.ToString("YYYYMMDD"));         //YYYYMMDD形式   'シフト用
                //Rev20.00 yyyyMMddに変更 by長野 2014/11/04
                theMecainf.Data.gain_date_sft = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));         //YYYYMMDD形式   'シフト用
            }
            else
            {
                //theMecainf.Data.gain_date = Convert.ToInt32(DateTime.Now.ToString("YYYYMMDD"));             //YYYYMMDD形式
                //Rev20.00 yyyyMMddに変更 by長野 2014/11/04
                theMecainf.Data.gain_date = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));             //YYYYMMDD形式
            }

			//ゲイン校正を行ったときのFPDゲイン      'v17.00追加 byやまおか 2010/02/22
            //変更2014/10/07hata_v19.51反映
            //theMecainf.Data.gain_fpd_gain = CTSettings.scansel.Data.fpd_gain;
            //検出器シフトに対応     'v18.00変更 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev23.20 左右シフト対応 by長野 2015/11/19
            //if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift))
            if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_R) || (modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_L))
            {
                theMecainf.Data.gain_fpd_gain_sft = CTSettings.scansel.Data.fpd_gain;                //シフト用
            }
            else
            {
                theMecainf.Data.gain_fpd_gain = CTSettings.scansel.Data.fpd_gain;
            }


			//ゲイン校正を行ったときのFPD積分時間    'v17.00追加 byやまおか 2010/02/22
            //変更2014/10/07hata_v19.51反映
            //theMecainf.Data.gain_fpd_integ = CTSettings.scansel.Data.fpd_integ;
            //検出器シフトに対応     'v18.00変更 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev23.20 左右シフト対応 by長野 2015/11/19
            //if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift))
            if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_R) || (modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_L))
            {
                theMecainf.Data.gain_fpd_integ_sft = CTSettings.scansel.Data.fpd_integ;
                //シフト用
            }
            else
            {
                theMecainf.Data.gain_fpd_integ = CTSettings.scansel.Data.fpd_integ;
            }

			//ゲイン校正を行ったときの年月日時       'v17.00追加 byやまおか 2010/03/04
            //変更2014/10/07hata_v19.51反映
            //theMecainf.Data.gain_time = Convert.ToInt32(DateTime.Now.ToString("HHmmss")); //HHMMSS形式
            //検出器シフトに対応     'v18.00変更 byやまおか 2011/02/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev23.20 左右シフト対応 by長野 2015/11/19
            //if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift))
            if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_R) || (modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_L))
            {
                //theMecainf.Data.gain_time_sft = Convert.ToInt32(DateTime.Now.ToString("HHMMSS"));
                //Rev20.00 HHmmssに変更 by長野 2014/11/04
                theMecainf.Data.gain_time_sft = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
                //HHMMSS形式 'シフト用
            }
            else
            {
                //theMecainf.Data.gain_time = Convert.ToInt32(DateTime.Now.ToString("HHMMSS"));
                //Rev20.00 HHmmssに変更 by長野 2014/11/04
                theMecainf.Data.gain_time = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
                //HHMMSS形式
            }

            //追加2014/10/07hata_v19.51反映
            //ゲイン校正を行ったときの焦点           'v18.00追加 byやまおか 2011/06/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev23.20 左右シフト対応 by長野 2015/11/19
            //if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift))
            if ((modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_R) || (modScanCorrect.Mode_GainCor == modScanCorrect.ModeCorConstants.ModeCor_shift_L))
            {
                theMecainf.Data.gain_focus_sft = CTSettings.mecainf.Data.xfocus;
                //シフト用
            }
            else
            {
                theMecainf.Data.gain_focus = CTSettings.mecainf.Data.xfocus;
            }

			//mecainf（コモン）更新
			//modMecainf.PutMecainf(ref theMecainf);
            theMecainf.Write();
        
        }
    }
}
