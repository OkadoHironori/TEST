using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： frmFilter.frm                                               */
    ///* 処理概要　　： ??????????????????????????????                              */
    ///* 注意事項　　： なし                                                        */
    ///* -------------------------------------------------------------------------- */
    ///* 適用計算機　： DOS/V PC                                                    */
    ///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
    ///* コンパイラ　： VB 6.0                                                      */
    ///* -------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
    ///*                                                                            */
    ///* v10.2      2005/06/23 (SI3)間々田          新規作成                        */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
    public partial class frmFilter : Form
    {
        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************
        private string LastTarget;

        private RadioButton[] optFilter = new RadioButton[6];   //#フィルタのラジオボタン
        private RadioButton[] optMatrix = new RadioButton[3];   //#マトリックスのラジオボタン

        #region インスタンスを返すプロパティ

        // frmFilterのインスタンス
        private static frmFilter _Instance = null;

        /// <summary>
        /// frmFilterのインスタンスを返す
        /// </summary>
        public static frmFilter Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmFilter();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmFilter()
        {
            InitializeComponent();

            #region コントロールのインスタンス作成し、プロパティを設定する

            this.SuspendLayout();

            #region #フィルタのラジオボタン

            for (int i = 0; i < optFilter.Length; i++)
            {
                optFilter[i] = new RadioButton();
                optFilter[i].AutoSize = true;
                optFilter[i].Name = "optFilter" + i.ToString();
                optFilter[i].Size = new Size(93, 20);
                optFilter[i].TabIndex = 0;
                optFilter[i].TabStop = true;
                optFilter[i].UseVisualStyleBackColor = true;

                switch (i)
                {
                    case 0:
                        optFilter[i].Text = "#ローパス";
                        optFilter[i].Location = new Point(16, 28);
                        optFilter[i].Tag = "16001";
                        break;

                    case 1:
                        optFilter[i].Text = "#ハイパス";
                        optFilter[i].Location = new Point(16, 56);
                        optFilter[i].Tag = "16002";
                        break;

                    case 2:
                        optFilter[i].Text = "#ガウス";
                        optFilter[i].Location = new Point(136, 28);
                        optFilter[i].Tag = "16003";
                        break;

                    case 3:
                        optFilter[i].Text = "#ハイガウス";
                        optFilter[i].Location = new Point(136, 56);
                        optFilter[i].Tag = "16004";
                        break;

                    case 4:
                        optFilter[i].Text = "#鮮明化";
                        optFilter[i].Location = new Point(256, 28);
                        optFilter[i].Tag = "16005";
                        break;

                    case 5:
                        optFilter[i].Text = "#メディアン";
                        optFilter[i].Location = new Point(256, 56);
                        optFilter[i].Tag = "16006";
                        break;

                    default:
                        break;
                }

                optFilter[i].CheckedChanged += new EventHandler(optFilter_CheckedChanged);

                this.fraFilter.Controls.Add(this.optFilter[i]);
            }

            #endregion

            #region #マトリックスのラジオボタン

            for (int i = 0; i < optMatrix.Length; i++)
            {
                optMatrix[i] = new RadioButton();

                optMatrix[i].AutoSize = true;
                optMatrix[i].Location = new Point(26, 32 + i * 21);
                optMatrix[i].Name = "optMatrix" + i.ToString();
                optMatrix[i].Size = new Size(62, 20);
                optMatrix[i].TabIndex = 1;
                optMatrix[i].TabStop = true;
                optMatrix[i].Text = (3 + i * 2).ToString() + " X " + (3 + i * 2).ToString();
                optMatrix[i].UseVisualStyleBackColor = true;
                optMatrix[i].CheckedChanged += new EventHandler(optMatrix_CheckedChanged);

                fraMatrix.Controls.Add(optMatrix[i]);
            }

            #endregion

            this.ResumeLayout(false);

            #endregion
        }

        //*******************************************************************************
        //機　　能： フィルタリング実行処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v10.2  2005/06/23 (SI3)間々田  新規作成
        //*******************************************************************************
        private bool DoFilter()
        {
            //戻り値初期化
            bool functionReturnValue = false;

            int rc = 0;                                     //イメージプロ関数の戻り値
            //Ipc32v5.RECT tmpReg = default(Ipc32v5.RECT);    //画像エリア指定の構造体
            int theMatrix = 0;                              //マトリクスサイズ
            short[] theImage = null;                        //画像読込みバッファ
            //int i = 0;
            int theSize = 0;
            string KernelName = null;
            string FileName = null;
            int theMin = 0;
            int theMax = 0;
            float theGamma = 0;                             //v19.00 ガンマを追加 by長野 2012/06/18

            //処理対象のファイル名
            FileName = (frmScanImage.Instance.Changed ? AppValue.OTEMPIMG : frmScanImage.Instance.Target);

            FileInfo fileInfo = new FileInfo(FileName); //ファイルのサイズを取得
            //2014/11/06hata キャストの修正
            theMatrix = Convert.ToInt32(Math.Sqrt(fileInfo.Length / 2D));
            if (!(theMatrix > 0))
            {
                return functionReturnValue;
            }

            //画像を元に戻せるようにバックアップをとっておく
            LastTarget = frmScanImage.Instance.Target;
            if (FileName.ToUpper() == AppValue.OTEMPIMG)
            {
                File.Copy(AppValue.OTEMPIMG, AppValue.LASTIMG);
            }

            //画像を取り込むための配列を確保
            theImage = new short[theMatrix * theMatrix];
            
            if (ScanCorrect.ImageOpen(ref theImage[0], FileName, theMatrix, theMatrix) != 0)
            {
                return functionReturnValue;
            }

            //modDispinf.GetDispinf(ref modDispinf.dispinf);
            CTSettings.dispinf.Load();


            //イメージプロで読み込めるように画像データを変換
            //2014/11/06hata キャストの修正
            theMin = Convert.ToInt32(CTSettings.dispinf.Data.level - CTSettings.dispinf.Data.width / 2F);
            theMax = Convert.ToInt32(CTSettings.dispinf.Data.level + CTSettings.dispinf.Data.width / 2F);
            theGamma = CTSettings.dispinf.Data.Gamma;

            //ChangeFullRange_Short theImage(0), theMatrix, theMatrix, theMin, theMax
            ScanCorrect.ChangeFullRange_Short(ref theImage[0], theMatrix, theMatrix, theMin, theMax, theGamma);

            #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            /*
            rc = Ipc32v5.IpAppCloseAll();//開いている全てのウィンドゥを閉じる
            rc = Ipc32v5.IpWsCreate((short)theMatrix, (short)theMatrix, 300, Ipc32v5.IMC_GRAY16);//空の画像ウィンドウを生成（Gray Scale 16形式）
            
            tmpReg.left = 0;
            tmpReg.top = 0;
            tmpReg.right = theMatrix - 1;
            tmpReg.bottom = theMatrix - 1;

            rc = Ipc32v5.IpDocPutArea((short)Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref theImage[0], (short)Ipc32v5.CPROG);//イメージプロに書込む
            rc = Ipc32v5.IpAppUpdateDoc(Ipc32v5.DOCSEL_ACTIVE);//再描画
            
            switch (modLibrary.GetOption(optFilter))
            {
                //ローパス
                case 0:
                    theSize = Choose(modLibrary.GetOption(optMatrix) + 1, new[] { 3, 5, 7 });
                    rc = Ipc32v5.IpFltLoPass((short)theSize, (short)cwneStrength.Value, (short)cwnePasses.Value);
                    break;

                //ハイパス
                case 1:
                    theSize = Choose(modLibrary.GetOption(optMatrix) + 1, new[] { 3, 5, 7 });
                    rc = Ipc32v5.IpFltHiPass((short)theSize, (short)cwneStrength.Value, (short)cwnePasses.Value);
                    break;

                //ガウス
                case 2:
                    theSize = (int)cwneSize.Value;
                    rc = Ipc32v5.IpFltGauss((short)theSize, (short)cwneStrength.Value, (short)cwnePasses.Value);
                    break;

                //ハイガウス
                case 3:
                    KernelName = Choose(modLibrary.GetOption(optMatrix) + 1, new[] { "HIGAUSS.7x7", "HIGAUSS.9x9" });
                    rc = Ipc32v5.IpFltConvolveKernel(KernelName, (short)cwneStrength.Value, (short)cwnePasses.Value);
                    break;

                //鮮明化
                case 4:
                    theSize = Choose(modLibrary.GetOption(optMatrix) + 1, new[] { 3, 5, 7 });
                    rc = Ipc32v5.IpFltSharpen((short)theSize, (short)cwneStrength.Value, (short)cwnePasses.Value);
                    break;

                //メディアン
                case 5:
                    theSize = Choose(modLibrary.GetOption(optMatrix) + 1, new[] { 3, 5, 7 });
                    rc = Ipc32v5.IpFltMedian((short)theSize, (short)cwnePasses.Value);
                    break;

                default:
                    break;
            }
            
            //イメージプロから読み込む
            rc = Ipc32v5.IpDocGetArea((short)Ipc32v5.DOCSEL_ACTIVE, ref tmpReg, ref theImage[0], (short)Ipc32v5.CPROG);
            */
            //64ﾋﾞｯﾄ対応（ImageProHelperを使用）
            switch (modLibrary.GetOption(optFilter))
            {
                //ローパス
                case 0:
                    theSize = Choose(modLibrary.GetOption(optMatrix) + 1, new[] { 3, 5, 7 });
                    break;

                //ハイパス
                case 1:
                    theSize = Choose(modLibrary.GetOption(optMatrix) + 1, new[] { 3, 5, 7 });
                    break;

                //ガウス
                case 2:
                    theSize = (int)cwneSize.Value;
                    break;

                //ハイガウス
                case 3:
                    KernelName = Choose(modLibrary.GetOption(optMatrix) + 1, new[] { "HIGAUSS.7x7", "HIGAUSS.9x9" });
                    break;

                //鮮明化
                case 4:
                    theSize = Choose(modLibrary.GetOption(optMatrix) + 1, new[] { 3, 5, 7 });
                    break;

                //メディアン
                case 5:
                    theSize = Choose(modLibrary.GetOption(optMatrix) + 1, new[] { 3, 5, 7 });
                    break;

                default:
                    break;
            }
            rc = CallImageProFunction.CallFilter(theImage, theImage, theMatrix, theMatrix, modLibrary.GetOption(optFilter), theSize, (int)cwneStrength.Value, (int)cwnePasses.Value, KernelName);
            #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//

            //ＣＴ値変換
            ScanCorrect.ChangeToCTImage(ref theImage[0], theMatrix, theMatrix, theMin, theMax);

            //保存
            if (ScanCorrect.ImageSave(ref theImage[0], AppValue.OTEMPIMG, theMatrix, theMatrix) != 1)
            {
                return functionReturnValue;
            }

            //戻り値初期化
            functionReturnValue = true;
            return functionReturnValue;
        }

        //*******************************************************************************
        //機　　能： 実行ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v10.2  2005/06/23 (SI3)間々田  新規作成
        //*******************************************************************************
        private void cmdExe_Click(object sender, EventArgs e)
        {
            //マウスポインタを砂時計にする
            Cursor.Current = Cursors.WaitCursor;

            //フォームを使用不可にする
            this.Enabled = false;

            if (DoFilter())
            {
                //マウスポインタを元に戻す
                Cursor.Current = Cursors.Default;

                //表示
                frmScanImage.Instance.DispTempImage(true);

                //「元に戻す」ボタンを使用可にする
                cmdUndo.Enabled = true;
            }
            else
            {
                //マウスポインタを元に戻す
                Cursor.Current = Cursors.Default;

                //メッセージ表示：フィルタ処理に失敗しました。
                MessageBox.Show(StringTable.GetResString(StringTable.IDS_WentWrong, this.Text), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //フォームを使用可にする
            this.Enabled = true;
        }

        //*******************************************************************************
        //機　　能： 元に戻すボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v10.2  2005/06/23 (SI3)間々田  新規作成
        //*******************************************************************************
        private void cmdUndo_Click(object sender, EventArgs e)
        {
            try
            {
                //前回表示画像をRESULTに復活させる
                if (LastTarget.ToUpper() == AppValue.OTEMPIMG)
                {
                    File.Copy(AppValue.LASTIMG, AppValue.OTEMPIMG, true);
                }

                //変換前の画像に戻す
                //frmScanImage.DoDispImage LastTarget
                frmScanImage.Instance.DispOrgImage();

                //「元に戻す」ボタンを使用不可にする
                cmdUndo.Enabled = false;
            }
            catch
            {
            }
        }

        //*******************************************************************************
        //機　　能： 終了ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v10.2  2005/06/23 (SI3)間々田  新規作成
        //*******************************************************************************
        private void cmdEnd_Click(object sender, EventArgs e)
        {
            //フォームのアンロード処理
            this.Close();
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
        //履　　歴： v10.2  2005/06/23 (SI3)間々田  新規作成
        //*******************************************************************************
        private void frmFilter_Load(object sender, EventArgs e)
        {
            //実行時はフラグをセット
			modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTImageProcessing;

			//キャプションのセット
			SetCaption();

			//v17.60 英語用レイアウト調整 by長野 2011/05/25
			if (modCT30K.IsEnglish)
            {
				EnglishAdjustLayout();
			}

			//フォームを標準位置に移動
			modCT30K.SetPosNormalForm(this);

			//フィルタの初期状態を設定
            modLibrary.SetOption(optFilter, modImgProc.CTFilter.FilterType, 0); //フィルタの種類
            //変更2015/02/02hata_Max/Min範囲のチェック
            //cwneSize.Value = modImgProc.CTFilter.SizeGauss;                     //マトリクスサイズ（ガウス）
            //cwnePasses.Value = modImgProc.CTFilter.Passes;                      //回数
            //cwneStrength.Value = modImgProc.CTFilter.Strength;                  //強さ
            cwneSize.Value = modLibrary.CorrectInRange(modImgProc.CTFilter.SizeGauss, cwneSize.Minimum, cwneSize.Maximum);              //マトリクスサイズ（ガウス）
            cwnePasses.Value = modLibrary.CorrectInRange(modImgProc.CTFilter.Passes, cwnePasses.Minimum, cwnePasses.Maximum);           //回数
            cwneStrength.Value = modLibrary.CorrectInRange(modImgProc.CTFilter.Strength, cwneStrength.Minimum, cwneStrength.Maximum);   //強さ

        }

        //*******************************************************************************
        //機　　能： QueryUnload イベント処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void frmFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseReason UnloadMode = e.CloseReason;

            //画像表示フォームが変更されている場合
            //if (frmScanImage.Instance.Changed &&
            //    (UnloadMode == CloseReason.ApplicationExitCall || UnloadMode == CloseReason.UserClosing))
            if (frmScanImage.Instance.Changed && (UnloadMode == CloseReason.UserClosing))
            {
                //結果保存ダイアログ
                //フィルタ処理
                if (!frmImageSave.Instance.Dialog(frmScanImage.Instance.Target, CTResources.LoadResString(10708)))
                {
                    e.Cancel = true;
                }
            }
        }

        //*******************************************************************************
        //機　　能： フォームアンロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v10.2  2005/06/23 (SI3)間々田  新規作成
        //*******************************************************************************
        private void frmFilter_FormClosed(object sender, FormClosedEventArgs e)
        {
			//フィルタの種類を記憶
			modImgProc.CTFilter.FilterType = modLibrary.GetOption(optFilter);   //フィルタの種類
			modImgProc.CTFilter.SizeGauss = (int)cwneSize.Value;                //マトリクスサイズ（ガウス）
            modImgProc.CTFilter.Passes = (int)cwnePasses.Value;                 //回数
            modImgProc.CTFilter.Strength = (int)cwneStrength.Value;             //強さ
			
			//終了時はフラグをリセット
			modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTImageProcessing);
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
        //履　　歴： v10.2  2005/06/23 (SI3)間々田  新規作成
        //*******************************************************************************
        private void SetCaption()
        {
            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            lblSizeRange.Text = StringTable.GetResString(StringTable.IDS_Range, cwneSize.Minimum.ToString(), cwneSize.Maximum.ToString());//3～31
            lblPassesRange.Text = StringTable.GetResString(StringTable.IDS_Range, cwnePasses.Minimum.ToString(), cwnePasses.Maximum.ToString());//1～10
            lblStrengthRange.Text = StringTable.GetResString(StringTable.IDS_Range, cwneStrength.Minimum.ToString(), cwneStrength.Maximum.ToString());//1～10
        }

        //*******************************************************************************
        //機　　能： フィルタ（種類）オプションボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v10.2  2005/06/23 (SI3)間々田  新規作成
        //*******************************************************************************
        void optFilter_CheckedChanged(object sender, EventArgs e)
        {
            int Index = -1;
            for (int i = 0; i < optFilter.Length; i++)
            {
                if (sender.Equals(optFilter[i]))
                {
                    Index = i;
                    break;
                }
            }

            //ガウス以外の場合、マトリクスフレームを表示する
            fraMatrix.Visible = !optFilter[2].Checked;

            //v17.60 ガウス以外の場合、サイズ指定を表示する by長野 2011/05/25
            lblSize.Visible = optFilter[2].Checked;
            cwneSize.Visible = optFilter[2].Checked;
            lblSizeRange.Visible = optFilter[2].Checked;

            //メディアン以外の場合、強さフレームを表示する
            fraStrength.Visible = !optFilter[5].Checked;

            switch (Index)
            {
                //ローパス・ハイパス・鮮明化・メディアンの場合、3x3, 5x5, 7x7のマトリクス・オプションボタンを表示
                case 0:
                case 1:
                case 4:
                case 5:
                    optMatrix[0].Text = "3 x 3";
                    optMatrix[1].Text = "5 x 5";
                    optMatrix[2].Text = "7 x 7";
                    optMatrix[2].Visible = true;
                    modLibrary.SetOption(optMatrix, modImgProc.CTFilter.SIZE, 0);
                    break;

                //ハイガウスの場合、7x7, 9x9 のマトリクス・オプションボタンを表示
                case 3:
                    optMatrix[0].Text = "7 x 7";
                    optMatrix[1].Text = "9 x 9";
                    optMatrix[2].Visible = false;
                    modLibrary.SetOption(optMatrix, modImgProc.CTFilter.SizeHiGauss, 0);
                    break;

                default:
                    break;
            }
        }

        //*******************************************************************************
        //機　　能： マトリクスオプションボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v10.2  2005/06/23 (SI3)間々田  新規作成
        //*******************************************************************************
        private void optMatrix_CheckedChanged(object sender, EventArgs e)
        {
            int Index = -1;
            for (int i = 0; i < optMatrix.Length; i++)
            {
                if (sender.Equals(optMatrix[i]))
                {
                    Index = i;
                    break;
                }
            }

            //ハイガウスの時
            if (optFilter[3].Checked)
            {
                modImgProc.CTFilter.SizeHiGauss = Index;
            }
            else
            {
                modImgProc.CTFilter.SIZE = Index;
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
        //履　　歴： v17.60  2011/05/25 (検S1)長野　 新規作成
        //*******************************************************************************
        private void EnglishAdjustLayout()
        {
            //マトリクスフレーム調整
            //fraMatrix.Left = 25;
            fraMatrix.Left = 2;

            //サイズボタン調整
            //lblSize.Left = 30;
            //cwneSize.Left = lblSize.Left + lblSize.Width + 5;
            //lblSizeRange.Left = cwneSize.Left + cwneSize.Width + 5;
            
            //Rev20.01 変更 by長野 2015/05/19
            //lblSize.Left = 2;
            //cwneSize.Left = lblSize.Left + lblSize.Width;
            //lblSizeRange.Left = cwneSize.Left + cwneSize.Width;
            //lblSize.Left = lblSize.Left - 30;
            //lblPasses.Left = lblPasses.Left - 30;
            //lblSize.Left = lblSize.Left - 30;
            lblPassesRange.Left = lblPassesRange.Left + 30;
            lblSizeRange.Left = lblSizeRange.Left + 30;
            lblStrengthRange.Left = lblStrengthRange.Left + 30;
            lblPasses.Left = lblPasses.Left - 30;
            lblSize.Left = lblSize.Left - 30;
            lblStrength.Left = lblStrength.Left - 30;

            cwnePasses.Left = cwnePasses.Left + 30;
            cwneSize.Left = cwneSize.Left + 30;
            cwneStrength.Left = cwneStrength.Left + 30;
            

            //強さフレーム調整
            lblStrength.TextAlign = ContentAlignment.TopLeft;

            //回数フレーム調整
            lblPasses.TextAlign = ContentAlignment.TopLeft;
        }

        #region VB6のChoose()相当するメソッド

        /// <summary>
        /// 引数のリストから指定の値を返す関数
        /// </summary>
        /// <param name="judge">1～n の選択可能な数値</param>
        /// <param name="values">選択リストの項目(配列)</param>
        private int Choose(int judge, int[] values)
        {
            int result = 0;

            for (int i = 0; i < values.Length; i++)
            {
                if (i + 1 == judge)
                {
                    result = values[i];
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 引数のリストから指定の値を返す関数
        /// </summary>
        /// <param name="judege">1～n の選択可能な数値</param>
        /// <param name="values">選択リストの項目(配列)</param>
        private string Choose(int judge, string[] values)
        {
            string result = "";

            for (int i = 0; i < values.Length; i++)
            {
                if (i + 1 == judge)
                {
                    result = values[i];
                    break;
                }
            }

            return result;
        }

        #endregion
    }
}
