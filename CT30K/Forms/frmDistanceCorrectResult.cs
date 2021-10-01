using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
//
using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver9.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： frmDistanceCorrectResult.frm                                */
    ///* 処理概要　　： 寸法校正結果                                                */
    ///* 注意事項　　： なし                                                        */
    ///* -------------------------------------------------------------------------- */
    ///* 適用計算機　： DOS/V PC                                                    */
    ///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
    ///* コンパイラ　： VB 6.0                                                      */
    ///* -------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
    ///*                                                                            */
    ///* V1.00       99/XX/XX    (TOSFEC) ????????                                  */
    ///* V2.0        00/02/08    (TOSFEC) 鈴山　修   V1.00を改造                    */
    ///* V3.0        00/08/01    (TOSFEC) 鈴山　修   ｺｰﾝﾋﾞｰﾑCT対応                  */
    ///* V9.0        04/02/18    (SI4)    間々田　   リソース対応                   */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
    public partial class frmDistanceCorrectResult : Form
    {
        #region インスタンスを返すプロパティ

        // frmDistanceCorrectResultのインスタンス
        private static frmDistanceCorrectResult _Instance = null;

        /// <summary>
        /// frmDistanceCorrectResultのインスタンスを返す
        /// </summary>
        public static frmDistanceCorrectResult Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmDistanceCorrectResult();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmDistanceCorrectResult()
        {
            InitializeComponent();
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
            //寸法校正結果フォームをアンロード
            this.Close();
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
            //added by 山本　2004-2-28   FCDオフセットの絶対値が大きい時は警告を出す
            //If Abs(GVal_Fcd_D - GVal_Fcd) > 10 Then
            //FCDオフセットとFDDオフセットをチェックする 'v17.60変更 byやまおか 2011/06/10
            //FCDは±20mm、FDDは±50mmまで許す
            if (Math.Abs(ScanCorrect.GVal_Fcd_O) > 20 || Math.Abs(ScanCorrect.GVal_Fid_O) > 50)
            {
                //メッセージ表示：
                //   寸法校正結果が正しくない可能性がありますので、結果を保存しません。
                //   寸法校正を再度行ってください。
                //Interaction.MsgBox(CT30K.My.Resources.str9326 + Constants.vbCr + StringTable.BuildResStr(StringTable.IDS_Retry, StringTable.IDS_CorSize), MsgBoxStyle.Exclamation);
                MessageBox.Show(CTResources.LoadResString(9326) + "\r" + StringTable.BuildResStr(StringTable.IDS_Retry, StringTable.IDS_CorSize), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                //寸法校正用パラメータのコモンへの書き込み
                //Call Set_Distance_Parameter

                //FDDの値を更新する  'v18.00追加 byやまおか 2011/03/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                frmMechaControl.Instance.FidValueChange();
                
                //v11.2以下に変更ここから by 間々田 2006/01/13 Set_Distance_Parameterの同等処理

                //メカの移動有無プロパティをリセットする
                modSeqComm.SeqBitWrite("DisXChangeReset", true);
                modSeqComm.SeqBitWrite("DisYChangeReset", true);
                modSeqComm.SeqBitWrite("DisIIChangeReset", true);
                
                //scancondpar（コモン）の更新
                UpdateScancondpar();

                //scansel（コモン）の更新
                UpdateScansel();

                //mecainf（コモン）の更新
                UpdateMecainf();
                //v11.2以下に変更ここまで by 間々田 2005/10/18

                //FCD/FDDが変わったときの処理    'v17.60追加 byやまおか 2011/06/08
                //frmMechaControl.Instance.ntbFCD_ValueChanged((frmMechaControl.Instance.ntbFCD.Value));
                //frmMechaControl.Instance.ntbFID_ValueChanged((frmMechaControl.Instance.ntbFID.Value));
                frmMechaControl.Instance.ntbFCD_ValueChanged(sender, new NumTextBox.ValueChangedEventArgs(frmMechaControl.Instance.ntbFCD.Value));
                frmMechaControl.Instance.ntbFID_ValueChanged(sender, new NumTextBox.ValueChangedEventArgs(frmMechaControl.Instance.ntbFID.Value));

                //FCD/FDDが変わったためオートセンタリングをありにする    'v17.60追加 byやまおか 2011/06/13
                frmScanControl.Instance.chkInhibit[3].CheckState = CheckState.Unchecked;
            }

            //寸法校正結果フォームをアンロード
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
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void frmDistanceCorrectResult_Load(object sender,EventArgs e)
        {
            //Rev23.20 各CaptionはSetCaptionとしてまとめた by長野 2016/01/12
            SetCaption();

            //Rev23.20 コントロール初期化を追加
            InitControl();

            //CT30Kをアクティブにする
            frmCTMenu.Instance.Activate();  //v16.20追加 byやまおか 2010/04/21
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
        private void InitControl()
        {
            //Rev23.20 2世代・3世代兼用の場合は、寸法校正2枚目の結果とFCD補正値は表示しない。 by長野 2016/01/12
            if (CTSettings.scaninh.Data.ct_gene2and3 == 0)
            {
                //寸法校正用画像2とFDDオフセット値は非表示
                lblCaption1.Visible = false;                    //寸法校正用画像2    'Rev17.10追加 byやまおか 2010/08/09
                
                lblTitle4.Visible = false;                      //円柱直径　　　　　 'Rev16.2/17.0 added by Isshiki
                lblColon4.Visible = false;
                lbl2RVal2.Visible = false;                      //lblFcdVal(2).Caption = Format$(GVal_Fcd, "0.000")
                Label20.Visible = false;
                
                lblTitle5.Visible = false;                      //最大ｽｷｬﾝｴﾘｱ        'Rev17.10追加 byやまおか 2010/08/09
                lblColon5.Visible = false;
                lblScnAreaVal2.Visible = false;
                Label9.Visible = false;

                lblTitle6.Visible = false;                      //ＦＣＤ             'Rev17.10追加 byやまおか 2010/08/09
                lblColon8.Visible = false;
                lblFcdVal2.Visible = false;                     //2枚目のFCD     'v17.60変更 byやまおか 2011/06/10
                Label10.Visible = false;                
                
                lblTitle8.Visible = false;
                lblColon7.Visible = false;
                lblFidOffset.Visible = false;                   //小数点以下４桁表示とする v11.2変更 by 間々田 2005/11/29
                Label17.Visible = false;
           
                //寸法校正用画像2の位置に校正結果を移動
                int distancePix = 0;//各ボタンの移動量
                distancePix = lblCaption2.Top - lblCaption1.Top;

                lblCaption2.SetBounds(lblCaption1.Left, lblCaption1.Top, lblCaption2.Width, lblCaption2.Height); //寸法校正結果
                lblTitle7.SetBounds(lblTitle4.Left,lblTitle4.Top,lblTitle7.Width,lblTitle7.Height);              //ＦＣＤオフセット
                lblFcdOffset.SetBounds(lbl2RVal2.Left, lbl2RVal2.Top, lblFcdOffset.Width, lblFcdOffset.Height);  //FCDオフセット値
                Label4.SetBounds(Label20.Left, Label20.Top, Label4.Width, Label4.Height);                        //単位(mm)
                lblColon3.SetBounds(lblColon4.Left, lblColon4.Top, lblColon4.Width, lblColon4.Height);

                lblMessage.Top = lblMessage.Top - distancePix;
                cmdOK.Top = cmdOK.Top - distancePix;
                cmdEnd.Top = cmdEnd.Top - distancePix;

                int newfrmWidth = this.Width;
                int newfrmHeight = this.Height - distancePix;

                this.Size = new Size(newfrmWidth, newfrmHeight);
            }
        }

        //*******************************************************************************
        //機　　能： キャプションの設定
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
            //各コントロールのキャプション用の文字列をリソースから取得 by 間々田 2003/08/22
            this.Text = StringTable.BuildResStr(StringTable.IDS_Result, StringTable.IDS_CorSize);

            //寸法校正結果
            cmdEnd.Text = CTResources.LoadResString(StringTable.IDS_btnNo);              //いいえ
            cmdOK.Text = CTResources.LoadResString(StringTable.IDS_btnYes);               //はい
            Label20.Text = CTResources.LoadResString(StringTable.IDS_Pixels);             //画素
            Labe1.Text = CTResources.LoadResString(StringTable.IDS_Pixels);               //v17.60 寸法校正用画像１の「画素」もストリングテーブル化 by 長野 2011/05/28
            lblMessage.Text = CTResources.LoadResString(StringTable.IDS_QueryResultSave); //結果を保存しますか？

            lblCaption0.Text = CTResources.LoadResString(12105) + "１";                   //寸法校正用画像1    'Rev17.10追加 byやまおか 2010/08/09
            lblTitle1.Text = CTResources.LoadResString(12106);                            //円柱直径
            lblTitle2.Text = CTResources.LoadResString(StringTable.IDS_MaxScanArea);      //最大ｽｷｬﾝｴﾘｱ
            lblTitle3.Text = CTResources.LoadResString(StringTable.IDS_FCD);              //ＦＣＤ
            lblCaption1.Text = CTResources.LoadResString(12105) + "２";                   //寸法校正用画像2    'Rev17.10追加 byやまおか 2010/08/09
            lblTitle4.Text = CTResources.LoadResString(12106);                            //円柱直径　　　　　　　　'//Rev16.2/17.0 added by Isshiki
            lblTitle5.Text = CTResources.LoadResString(StringTable.IDS_MaxScanArea);      //最大ｽｷｬﾝｴﾘｱ        'Rev17.10追加 byやまおか 2010/08/09
            lblTitle6.Text = CTResources.LoadResString(StringTable.IDS_FCD);              //ＦＣＤ             'Rev17.10追加 byやまおか 2010/08/09

            lblCaption2.Text = StringTable.BuildResStr(StringTable.IDS_Result, StringTable.IDS_CorSize);//寸法校正結果   'Rev17.10追加 byやまおか 2010/08/09
            lblTitle7.Text = CTResources.LoadResString(12397);                               //ＦＣＤオフセット       'v9.7追加 by 間々田 2004/12/27
            //変更2014/10/07hata_v19.51反映
            //lblTitle8.Text = StringTable.GetResString(StringTable.IDS_Offset, CTSettings.gStrFidOrFdd);//FIDまたはFDDオフセット  'v16.20/v17.00追加 byやまおか 2010/03/12
            lblTitle8.Text = StringTable.GetResString(StringTable.IDS_AxisOffset, CTSettings.gStrFidOrFdd); //v18.00変更 IDS_Offset→IDS_AxisOffset byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //寸法校正パラメータ表示
            lblScnAreaVal1.Text = ScanCorrect.GVal_ScnAreaMax1[ScanCorrect.GFlg_ScanMode_D - 1].ToString("0.000");
            lbl2RVal1.Text = ScanCorrect.GVal_2R_D1.ToString("0.000");                  //Rev16.2/17.0 added by Isshiki
            lblFcdVal1.Text = ScanCorrect.GVal_Fcd1.ToString("0.000");                  //Rev16.2/17.0 added by Isshiki
            lblScnAreaVal2.Text = ScanCorrect.GVal_ScnAreaMax[ScanCorrect.GFlg_ScanMode_D - 1].ToString("0.000");
            lbl2RVal2.Text = ScanCorrect.GVal_2R_D.ToString("0.000");                   //lblFcdVal(2).Caption = Format$(GVal_Fcd, "0.000")
            lblFcdVal2.Text = ScanCorrect.GVal_Fcd2.ToString("0.000");                  //2枚目のFCD     'v17.60変更 byやまおか 2011/06/10
            //lblFcdOffset.Caption = Format$(GVal_Fcd_D - GVal_Fcd, "0.000")        'FCDオフセット値も表示する v9.7追加 by 間々田 2004/12/27
            lblFcdOffset.Text = ScanCorrect.GVal_Fcd_O.ToString("0.0000");
            lblFidOffset.Text = ScanCorrect.GVal_Fid_O.ToString("0.0000");              //小数点以下４桁表示とする v11.2変更 by 間々田 2005/11/29
        }


        //*******************************************************************************
        //機　　能： scancondpar（コモン）の更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 寸法校正パラメータをコモンに書き込む
        //
        //履　　歴： v11.2  2006/01/13  (SI3)間々田    新規作成
        //*******************************************************************************
        private void UpdateScancondpar()
        {
            //新ＦＣＤ、ＦＣＤオフセット代入
            //.fcd_offset(GFlg_MultiTube_D) = GVal_Fcd_D - GVal_Fcd
            CTSettings.scancondpar.Data.fcd_offset[ScanCorrect.GFlg_MultiTube_D] = ScanCorrect.GVal_Fcd_O;   //v16.2/17.0変更　2009/10/26　By Isshiki
            
            //Rev23.20 2世代・3世代兼用の場合はFDDオフセットは保存しない by長野 2016/01/12
            if (CTSettings.scaninh.Data.ct_gene2and3 == 1)
            {
                CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube_D] = ScanCorrect.GVal_Fid_O;   //V16.2/17.0追加　2009/10/26　By Isshiki
            }

            //■ＣＳＶファイルの保存

            //ＦＩＤオフセットは現在値をそのまま、ＦＣＤオフセットは現在のX線管についてのみ書き込む

            //ファイルオープン
            StreamWriter sw = null;
            try
            {
                //変更2014/10/07hata_v19.51反映
                ////FileSystem.FileOpen(fileNo, modFileIO.OFFSET_CSV, OpenMode.Output);
                //sw = new StreamWriter(AppValue.OFFSET_CSV, false); //ファイルオープン (これまでの内容は消去され上書きするモード)
                //v17.20 条件式を追加 by 長野 2010-08-31
                if (CTSettings.SecondDetOn & mod2ndDetctor.IsDet2mode)
                {
                    //変更2015/01/22hata
                    ////FileSystem.FileOpen(fileNo, modFileIO.OFFSET_2_CSV, OpenMode.Output);
                    //sw = new StreamWriter(AppValue.OFFSET_2_CSV, false); //ファイルオープン (これまでの内容は消去され上書きするモード)
                    sw = new StreamWriter(AppValue.OFFSET_2_CSV, false, Encoding.GetEncoding("shift-jis")); //ファイルオープン (これまでの内容は消去され上書きするモード)
                }
                else
                {
                    //変更2015/01/22hata
                    ////FileSystem.FileOpen(fileNo, modFileIO.OFFSET_CSV, OpenMode.Output);
                    //sw = new StreamWriter(AppValue.OFFSET_CSV, false); //ファイルオープン (これまでの内容は消去され上書きするモード)
                    sw = new StreamWriter(AppValue.OFFSET_CSV, false, Encoding.GetEncoding("shift-jis")); //ファイルオープン (これまでの内容は消去され上書きするモード)
                }

                //LoadResString(IDS_FIDAddValue):FID加算値(mm)
                //FileSystem.PrintLine(fileNo, CTResources.LoadResString(StringTable.IDS_FIDAddValue) + ",fid_offset," + 
                //                             modScancondpar.scancondpar.fid_offset[0].ToString("0.0000") + "," + 
                //                             modScancondpar.scancondpar.fid_offset[1].ToString("0.0000") + "," +  "0.0000");
                sw.WriteLine(CTResources.LoadResString(StringTable.IDS_FIDAddValue) + ",fid_offset," +
                             CTSettings.scancondpar.Data.fid_offset[0].ToString("0.0000") + "," +
                             CTSettings.scancondpar.Data.fid_offset[1].ToString("0.0000") + "," + "0.0000");

                //LoadResString(IDS_FCDAddValue):FCD加算値(mm)
                //FileSystem.PrintLine(fileNo, CTResources.LoadResString(StringTable.IDS_FCDAddValue) + ",fcd_offset," + 
                //                             modScancondpar.scancondpar.fcd_offset[0].ToString("0.0000") + "," + 
                //                             modScancondpar.scancondpar.fcd_offset[1].ToString("0.0000") + "," + "0.0000");
                sw.WriteLine(CTResources.LoadResString(StringTable.IDS_FCDAddValue) + ",fcd_offset," +
                             CTSettings.scancondpar.Data.fcd_offset[0].ToString("0.0000") + "," +
                             CTSettings.scancondpar.Data.fcd_offset[1].ToString("0.0000") + "," + "0.0000");
            }
            catch (Exception)
            {
            }
            finally
            {
                //ファイルクローズ
                if (sw != null)
                {
                    sw.Close();
                    sw = null;
                }
            }

            //Scancondpar（コモン）の書き込み
            //modScancondpar.CallPutScancondpar();
            CTSettings.scancondpar.Write();

        }

        //*******************************************************************************
        //機　　能： scansel（コモン）の更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.2  2006/01/13  (SI3)間々田    新規作成
        //*******************************************************************************
        private void UpdateScansel()
        {
            CTSettings.scansel.Data.max_scan_area[0] = ScanCorrect.GVal_ScnAreaMax[0];
            CTSettings.scansel.Data.max_scan_area[1] = ScanCorrect.GVal_ScnAreaMax[1];
            CTSettings.scansel.Data.max_scan_area[2] = ScanCorrect.GVal_ScnAreaMax[2];
            //追加2014/10/07hata_v19.51反映
            CTSettings.scansel.Data.max_scan_area[3] = ScanCorrect.GVal_ScnAreaMax[3];  //v18.00追加 byやまおか 2011/02/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            CTSettings.scansel.Data.mscan_area = ScanCorrect.GVal_MScnArea;
            CTSettings.scansel.Data.fcd = ScanCorrect.GVal_Fcd + CTSettings.scancondpar.Data.fcd_offset[ScanCorrect.GFlg_MultiTube_D];
            CTSettings.scansel.Data.fid = ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube_D];        //V16.2/17.0追加　2009/10/26　By Isshiki

            //scansel書き込み
            //modScansel.PutScansel(ref modScansel.scansel);
            CTSettings.scansel.Write();
        }

        //*******************************************************************************
        //機　　能： mecainf（コモン）の更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 寸法校正関連のパラメータのみ更新
        //
        //履　　歴： v11.2  2006/01/13  (SI3)間々田    新規作成
        //*******************************************************************************
        private void UpdateMecainf()
        {
			//modMecainf.mecainfType theMecainf = default(modMecainf.mecainfType);
            MecaInf theMecainf = new MecaInf();
            theMecainf.Data.Initialize();
            
            //mecainf（コモン）取得
            //modMecainf.GetMecainf(ref theMecainf);
            theMecainf.Load();

            //I.I.視野
            theMecainf.Data.dc_iifield = modSeqComm.GetIINo();

            //Ｘ線管
            theMecainf.Data.dc_mt = CTSettings.scansel.Data.multi_tube;

            //ビニングモード
            theMecainf.Data.dc_bin = CTSettings.scansel.Data.binning;     //0:１×１，1:２×２，2:４×４

            //回転選択が可能な場合
            if (CTSettings.scaninh.Data.rotate_select == 0)
            {
                theMecainf.Data.dc_rs = CTSettings.scansel.Data.rotate_select;
            }

            //寸法校正を行った時の年月日   'v16.20/v17.00追加 byやまおか 2010/03/02
            theMecainf.Data.dc_date = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));    //YYYYMMDD形式

            //寸法校正を行った時の時間     'v16.20/v17.00追加 byやまおか 2010/03/04
            theMecainf.Data.dc_time = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));      //HHMMSS形式

            //mecainf（コモン）更新
            //modMecainf.PutMecainf(ref theMecainf);
            theMecainf.Write();
        
        }
    }
}
