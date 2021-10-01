using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;

using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{
    public partial class frmDistanceCorrectResultPhantomFree : Form
    {
        // frmDistanceCorrectResultPhantomFreeのインスタンス
        private static frmDistanceCorrectResultPhantomFree _Instance = null;

        /// <summary>
        /// frmDistanceCorrectPhantomFreeのインスタンスを返す
        /// </summary>
        public static frmDistanceCorrectResultPhantomFree Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmDistanceCorrectResultPhantomFree();
                }

                return _Instance;
            }
        }

        public frmDistanceCorrectResultPhantomFree()
        {
            InitializeComponent();
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
        //履　　歴： V23.12  15/12/12   (検S1)長野   新規作成
        //*******************************************************************************
        private void frmDistanceCorrectResultPhantomFree_Load(object sender, EventArgs e)
        {
           
            //CT30Kをアクティブにする
            frmCTMenu.Instance.Activate();  //v16.20追加 byやまおか 2010/04/21
        }

        //*******************************************************************************
        //機　　能： Captionのセット
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.12  15/12/12   (検S1)長野   新規作成
        //*******************************************************************************
        private void SetCaption()
        {
            //各コントロールのキャプション用の文字列をリソースから取得 by 間々田 2003/08/22
            this.Text = StringTable.BuildResStr(StringTable.IDS_Result, StringTable.IDS_CorSize);

            //寸法校正結果
            cmdEnd.Text = CTResources.LoadResString(StringTable.IDS_btnNo);               //いいえ
            cmdOK.Text = CTResources.LoadResString(StringTable.IDS_btnYes);               //はい
            lblMessage.Text = CTResources.LoadResString(StringTable.IDS_QueryResultSave); //結果を保存しますか？

            lblCaption0.Text = CTResources.LoadResString(12105) + "１";                   //寸法校正用画像1
            lblTitle1.Text = CTResources.LoadResString(24001);                            //実測値
            lblTitle2.Text = CTResources.LoadResString(24002);                            //CT計測値
            lblTitle3.Text = CTResources.LoadResString(StringTable.IDS_FCD);              //ＦＣＤ
            lblCaption1.Text = CTResources.LoadResString(12105) + "２";                   //寸法校正用画像2
            lblTitle4.Text = CTResources.LoadResString(24001);                            //実測値
            lblTitle5.Text = CTResources.LoadResString(24002);                      　    //CT計測値
            lblTitle6.Text = CTResources.LoadResString(StringTable.IDS_FCD);              //ＦＣＤ  

            lblCaption2.Text = StringTable.BuildResStr(StringTable.IDS_Result, StringTable.IDS_CorSize);//寸法校正結果   'Rev17.10追加 byやまおか 2010/08/09

            lblTitle7.Text = CTResources.LoadResString(12397);                               //ＦＣＤオフセット       'v9.7追加 by 間々田 2004/12/27
            //変更2014/10/07hata_v19.51反映
            lblTitle8.Text = StringTable.GetResString(StringTable.IDS_AxisOffset, CTSettings.gStrFidOrFdd); //v18.00変更 IDS_Offset→IDS_AxisOffset byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //寸法校正パラメータ表示
            lbl2RVal1.Text = frmDistanceCorrectNew.Instance.cwne2R_ByMeasure0.Value.ToString("0.0000");
            lbl2RMeasureVal1.Text = frmDistanceCorrectNew.Instance.cwne2R_ByCT0.Value.ToString("0.0000");
            lblFcdVal1.Text = ScanCorrect.GVal_Fcd1.ToString("0.000");
            lbl2RVal2.Text = frmDistanceCorrectNew.Instance.cwne2R_ByCT1.Value.ToString("0.0000");
            lbl2RMeasureVal2.Text = frmDistanceCorrectNew.Instance.cwne2R_ByMeasure1.Value.ToString("0.0000");
            lblFcdVal2.Text = ScanCorrect.GVal_Fcd2.ToString("0.000");                  //2枚目のFCD     'v17.60変更 byやまおか 2011/06/10
            lblFcdOffset.Text = ScanCorrect.GVal_Fcd_O.ToString("0.0000");
            lblFidOffset.Text = ScanCorrect.GVal_Fid_O.ToString("0.0000");              //小数点以下４桁表示とする v11.2変更 by 間々田 2005/11/29
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
        //履　　歴： V23.12  15/12/12   (検S1)長野   新規作成
        //*******************************************************************************
        private void InitControl()
        {
            //2世代・3世代兼用の場合は、寸法校正画像2とFDD校正結果は出さない
            if (CTSettings.scaninh.Data.ct_gene2and3 == 0)
            {
                lblCaption1.Visible = false; //寸法校正用画像2

                lblTitle4.Visible = false;   //実測値
                lblColon4.Visible = false;
                lbl2RMeasureVal2.Visible = false;
                Label20.Visible = false;

                lblTitle5.Visible = false;   //CT計測値
                lblColon5.Visible = false;
                lbl2RVal2.Visible = false;
                Label9.Visible = false;
                
                lblTitle6.Visible = false;   //ＦＣＤ  
                lblColon8.Visible = false;
                lblFcdVal2.Visible = false;
                Label10.Visible = false;

                lblTitle8.Visible = false;
                lblColon7.Visible = false;
                lblFidOffset.Visible = false;
                Label17.Visible = false;

                //寸法校正画像2の位置に寸法校正結果を移動させる
                int distancePix = 0;
                distancePix = lblCaption1.Top - lblCaption2.Top;
                //寸法校正結果
                lblCaption2.SetBounds(lblCaption1.Left, lblCaption1.Top, lblCaption2.Width, lblCaption2.Height);
                lblTitle7.SetBounds(lblTitle4.Left, lblTitle4.Top, lblTitle7.Width, lblTitle7.Height);
                lblColon3.SetBounds(lblColon4.Left, lblTitle4.Top, lblColon3.Width, lblColon3.Height);
                Label4.SetBounds(Label10.Left, Label10.Top, Label4.Width, Label4.Height);

                cmdOK.SetBounds(cmdOK.Left, cmdOK.Top - distancePix, cmdOK.Width, cmdOK.Height);
                cmdEnd.SetBounds(cmdEnd.Left, cmdEnd.Top - distancePix, cmdEnd.Width, cmdEnd.Height);
                lblMessage.SetBounds(lblMessage.Left, lblMessage.Top - distancePix, lblMessage.Width, lblMessage.Height);

                int newfrmWidth = this.Width;
                int newfrmHeight = this.Height - distancePix;

                this.Size = new Size(newfrmWidth, newfrmHeight);

            }
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
        //履　　歴： V23.12  15/12/12   (検S1)長野   新規作成
        //*******************************************************************************
        private void cmdOK_Click(object sender, EventArgs e)
        {
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
        //機　　能： scancondpar（コモン）の更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 寸法校正パラメータをコモンに書き込む
        //
        //履　　歴： V23.12  15/12/12   (検S1)長野   新規作成
        //*******************************************************************************
        private void UpdateScancondpar()
        {
            //新ＦＣＤ、ＦＣＤオフセット代入
            //.fcd_offset(GFlg_MultiTube_D) = GVal_Fcd_D - GVal_Fcd
            CTSettings.scancondpar.Data.fcd_offset[ScanCorrect.GFlg_MultiTube_D] = ScanCorrect.GVal_Fcd_O;   //v16.2/17.0変更　2009/10/26　By Isshiki
            CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube_D] = ScanCorrect.GVal_Fid_O;   //V16.2/17.0追加　2009/10/26　By Isshiki

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
        //履　　歴： V23.12  15/12/12   (検S1)長野   新規作成
        //*******************************************************************************
        private void UpdateScansel()
        {
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
        //履　　歴： V23.12  15/12/12   (検S1)長野   新規作成
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

        //*******************************************************************************
        //機　　能： 「いいえ」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.12  15/12/12   (検S1)長野   新規作成
        //*******************************************************************************
        private void cmdEnd_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
