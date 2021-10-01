using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

using CT30K.Properties;
using CT30K.Common;
using CTAPI;
using TransImage;

namespace CT30K
{
    public partial class frmDistanceCorrectNew : Form
    {

        // frmDistanceCorrectByCmModeのインスタンス
        private static frmDistanceCorrectNew _Instance = null;

        private NumericUpDown[] cwne2R;
        private NumericUpDown[] cwne2R_ByCT;
        private NumericUpDown[] cwne2R_ByMeasure;
        private TextBox[] txtImgFileName;
        private TextBox[] txtImgFileNamePhantomFree;
        private Button[] cmdDialog;
        private Button[] btnRef;

        private TabPageCtrl tabPageControl;

        /// <summary>
        /// frmDistanceCorrectNewのインスタンスを返す
        /// </summary>
        public static frmDistanceCorrectNew Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmDistanceCorrectNew();
                }

                return _Instance;
            }
        }

        public frmDistanceCorrectNew()
        {
            InitializeComponent();

            //コントロールの配列化
            btnRef = new Button[] { null, btnRef0, btnRef1 };
            cmdDialog = new Button[] { null, cmdDialog_0, cmdDialog_1 };

            txtImgFileName = new TextBox[] { null, txtImgFileName0, txtImgFileName1 };
            txtImgFileNamePhantomFree = new TextBox[] { null, txtImgFileNamePhantomFree0, txtImgFileNamePhantomFree1 };

            cwne2R_ByCT = new NumericUpDown[] { null, cwne2R_ByCT0, cwne2R_ByCT1 };
            cwne2R = new NumericUpDown[] { null, cwne2R_0, cwne2R_1 };
            cwne2R_ByMeasure = new NumericUpDown[] { null, cwne2R_ByMeasure0, cwne2R_ByMeasure1 };


            //TaPageの表示／非表示切り替えのため、TabPageCtrlオブジェクトを作成
            tabPageControl = new TabPageCtrl(this.tabControl1);
        }

        //*******************************************************************************
        //機　　能： ＯＫボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.12  16/01/09   (検S1)長野   新規作成
        //*******************************************************************************
        private void cmdOK_Click(object sender, EventArgs e)
        {
            int Index = 0;      //added by Isshiki

            int SelectedIndex = 0;
            SelectedIndex = tabControl1.SelectedIndex;

            //ボタンを使用不可にする
            cmdOK.Enabled = false;
            cmdEnd.Enabled = false;

            //マウスポインタを砂時計にする
            this.Cursor = Cursors.WaitCursor;

            if (CTSettings.scaninh.Data.ct_gene2and3 == 0)
            {
                //1枚目も2枚目も同じデータが選択されたものとする。
                SelectedIndex = tabControl1.SelectedIndex;
                switch (SelectedIndex)
                {
                    case 0:
                        txtImgFileName[2].Text = txtImgFileName[1].Text;
                        cwne2R[2].Value = cwne2R[1].Value;
                        break;

                    case 1:
                        txtImgFileNamePhantomFree[2].Text = txtImgFileNamePhantomFree[1].Text;
                        cwne2R_ByCT[2].Value = cwne2R_ByCT[1].Value;
                        cwne2R_ByMeasure[2].Value = cwne2R_ByMeasure[1].Value;
                        break;

                    default:
                        break;
                }
            }

            switch (SelectedIndex)
            {
                case 0:
                    //寸法校正ファントム     V4.0 append by 鈴山 2001/02/01

                    //added by 一色
                    for (Index = 1; Index <= 2; Index++)
                    {
                        //GVal_DistVal = cwne2R(Index).Value
                        ScanCorrect.GVal_DistVal[Index] = (float)cwne2R[Index].Value;   //Rev16.2/17.0 修正 2010/02/23 by Iwasawa
                    }

                    if (DoDistanceCorrect(txtImgFileName[2].Text.Trim()))
                    {
                        //寸法校正フォームをアンロードする
                        this.Close();　   //復活2015/01/26hata　Dispose前にCloseする
                        //Rev20.00 showDialogから呼ばれているので、リソース破棄のためdispose使用 by長野 2014/12/04
                        this.Dispose();
                        return;
                    }
                    break;

                case 1:
                    //added by 一色
                    for (Index = 1; Index <= 2; Index++)
                    {
                        ScanCorrect.GVal_DistValMeasure[Index] = (float)cwne2R_ByCT[Index].Value;   //Rev16.2/17.0 修正 2010/02/23 by Iwasawa
                        ScanCorrect.GVal_DistVal[Index] = (float)cwne2R_ByMeasure[Index].Value;   //Rev16.2/17.0 修正 2010/02/23 by Iwasawa
                    }

                    if (DoDistanceCorrectByPhantomFree())
                    {
                        //寸法校正フォームをアンロードする
                        this.Close();　   //復活2015/01/26hata　Dispose前にCloseする
                        //Rev20.00 showDialogから呼ばれているので、リソース破棄のためdispose使用 by長野 2014/12/04
                        this.Dispose();
                        return;
                    }
                    break;

                default:

                    //added by 一色
                    for (Index = 1; Index <= 2; Index++)
                    {
                        //GVal_DistVal = cwne2R(Index).Value
                        ScanCorrect.GVal_DistVal[Index] = (float)cwne2R[Index].Value;   //Rev16.2/17.0 修正 2010/02/23 by Iwasawa
                    }

                    if (DoDistanceCorrect(txtImgFileName[2].Text.Trim()))
                    {
                        //寸法校正フォームをアンロードする
                        this.Close();　   //復活2015/01/26hata　Dispose前にCloseする
                        //Rev20.00 showDialogから呼ばれているので、リソース破棄のためdispose使用 by長野 2014/12/04
                        this.Dispose();
                        return;
                    }
                    break;
            }


            //マウスポインタを元に戻す
            this.Cursor = Cursors.Default;

            //ボタンの状態を元に戻す
            cmdOK.Enabled = true;
            cmdEnd.Enabled = true;
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
        //履　　歴： V23.12  16/01/09   (検S1)長野   新規作成
        //*******************************************************************************
        private void cmdEnd_Click(object sender, EventArgs e)
        {
            //寸法校正フォームをアンロードする
            this.Close();　   //復活2015/01/26hata　Dispose前にCloseする

        }

        //*******************************************************************************
        //機　　能： キャプションのセット
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.12 16/01/09  (検S1)長野      新規作成
        //*******************************************************************************
        private void SetCaption()
        {
            this.Text = CTResources.LoadResString(StringTable.IDS_CorSize);   //寸法校正

            //変更2014/10/07hata_v19.51反映
            //fraFidFcdOffset.Text = StringTable.GetResString(StringTable.IDS_Offset, CTSettings.gStrFidOrFdd + "/" + CTResources.LoadResString(StringTable.IDS_FCD));  //FIDまたはFDD/FCDオフセット 'Rev16.20/v17.00 FPD対応 2010/03/12 byやまおか
            fraFidFcdOffset.Text = StringTable.GetResString(StringTable.IDS_AxisOffset, CTSettings.gStrFidOrFdd + "/" + CTResources.LoadResString(StringTable.IDS_FCD)); //v18.00変更 IDS_Offset→IDS_AxisOffset byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            // Add Start 2018/08/28 M.Oyama 中国語対応
            tabControl1.Controls[0].Text = CTResources.LoadResString(12104);
            // Add End 2018/08/28

            /////////////////////////標準校正ファントム有タブ/////////////////////////////////

            cmdEnd.Text = CTResources.LoadResString(StringTable.IDS_btnEnd);  //終　了
            cmdOK.Text = CTResources.LoadResString(StringTable.IDS_btnOK);    //ＯＫ

            lblTitle2_0.Text = CTResources.LoadResString(12104);             //寸法校正ファントム 円柱直径：  'Rev16.2/17.0 ファントム2つに対応 2010/2/23 by Isshiki/Iwasawa
            lblTitle2_1.Text = CTResources.LoadResString(12104);             //寸法校正ファントム 円柱直径：  'Rev16.2/17.0 ファントム2つに対応 2010/2/23by Isshiki/Iwasawa
            lblTitle1_0.Text = CTResources.LoadResString(12105) + "１：";      //寸法校正用画像1：  'Rev16.2/17.0 ファントム2つに対応 2010/3/11 byやまおか
            lblTitle1_1.Text = CTResources.LoadResString(12105) + "２：";      //寸法校正用画像2：  'Rev16.2/17.0 ファントム2つに対応 2010/3/11 byやまおか

            /////////////////////////標準校正ファントム以外タブ///////////////////////////////

            lblTitle3_0.Text = CTResources.LoadResString(12105) + "１：";      //寸法校正用画像1：  'Rev16.2/17.0 ファントム2つに対応 2010/3/11 byやまおか
            lblTitle3_1.Text = CTResources.LoadResString(12105) + "２：";      //寸法校正用画像2：  'Rev16.2/17.0 ファントム2つに対応 2010/3/11 byやまおか

            frDist1.Text = CTResources.LoadResString(24000);
            frDist2.Text = CTResources.LoadResString(24000);

            lblTitle5_0.Text = CTResources.LoadResString(24001);
            lblTitle5_1.Text = CTResources.LoadResString(24001);
         
            lblTitle6_0.Text = CTResources.LoadResString(24002);
            lblTitle6_1.Text = CTResources.LoadResString(24002);

            lblTitle7_0.Text = CTResources.LoadResString(24005);
            lblTitle7_1.Text = CTResources.LoadResString(24005);

        }
        //*******************************************************************************
        //機　　能： 初期値セット
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.12 16/01/09  (検S1)長野      新規作成
        //*******************************************************************************
        private void InitControl()
        {
            int Index = 0;  //v16.2/17.0 added by 一色

            //現在のFDD・FCDオフセットを表示する     'v15.0追加 by 間々田 従来のスキャン条件で表示していたものをここに移動
            //FDD/FCDオフセットを表示
            //Rev23.10 計測CT対応 by長野 2015/10/18
            if (CTSettings.scaninh.Data.cm_mode == 0)//FCD,FDDの表示桁が増えた分、寸法校正の表示桁も増やす
            {
                lblFidOffset.Text = CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube].ToString("0.0000");
                lblFcdOffset.Text = CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()].ToString("0.000000");		//FDDオフセットは小数点以下４桁とする by 間々田 2005/11/29
            }
            else
            {
                lblFidOffset.Text = CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube].ToString("0.00");
                lblFcdOffset.Text = CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()].ToString("0.0000");		//FDDオフセットは小数点以下４桁とする by 間々田 2005/11/29
            }

            //v16.2/17.0 added by Isshiki
            for (Index = 1; Index <= 2; Index++)
            {
                //変更2015/02/02hata_Max/Min範囲のチェック
                //cwne2R[Index].Value = (decimal)ScanCorrect.GVal_DistVal[Index];
                cwne2R[Index].Value = modLibrary.CorrectInRange((decimal)ScanCorrect.GVal_DistVal[Index], cwne2R[Index].Minimum, cwne2R[Index].Maximum);
                cwne2R_ByCT[Index].Value = modLibrary.CorrectInRange((decimal)ScanCorrect.GVal_DistValMeasure[Index], cwne2R_ByCT[Index].Minimum, cwne2R_ByCT[Index].Maximum);
                cwne2R_ByMeasure[Index].Value = modLibrary.CorrectInRange((decimal)ScanCorrect.GVal_DistVal[Index], cwne2R_ByMeasure[Index].Minimum, cwne2R_ByMeasure[Index].Maximum);
            }

            //Rev23.20 X軸(旧Y軸)が動かない場合は1点モード(FDDオフセット校正無)
            if (CTSettings.scaninh.Data.table_y == 1)
            {
                //標準寸法校正ファントムタブ
                cwne2R[2].Visible = false;
                cmdDialog[2].Visible = false;
                lblTitle1_1.Visible = false;
                lblTitle2_1.Visible = false;
                txtImgFileName1.Visible = false;

                //標準寸法校正ファントム以外タブ
                lblTitle3_1.Visible = false;
                frDist2.Visible = false;

                int newTabHeight = 0;
                int newTabWidth = 0;
                int newfrmHeight = 0;
                int newfrmWidth = 0;

                int distancePixByfrm = 0;

                newTabHeight = tabControl1.Height * 2 / 3;
                newTabWidth = tabControl1.Width;

                newfrmHeight = this.Height  * 3 / 4;
                newfrmWidth = this.Width;
                distancePixByfrm = this.Height - newfrmHeight;

                fraFidFcdOffset.Top = fraFidFcdOffset.Top - distancePixByfrm;
                cmdOK.Top = cmdOK.Top - distancePixByfrm;
                cmdEnd.Top = cmdEnd.Top - distancePixByfrm;

                tabControl1.Size = new Size(newTabWidth, newTabHeight);
                this.Size = new Size(newfrmWidth, newfrmHeight);

            }

            tabPageControl.TabVisible(1, CTSettings.scaninh.Data.distancecor_phantomfree == 0); //「標準寸法校正ファントム以外」タブ
    
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
        //履　　歴： V23.12 16/01/09  (検S1)長野      新規作成
        //*******************************************************************************
        private void frmDistanceCorrectNew_Load(object sender, EventArgs e)
        {
            int Index = 0;  //v16.2/17.0 added by 一色

            //実行時はフラグをセット
            modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTScanCorrect;

            //フォームの設定
            //Me.Move FmStdLeft, FmStdTop

            //キャプションのセット
            SetCaption();

            //現在のコモン内容を取り出す
            ScanCorrect.OptValueGet_Cor();

            InitControl();

            //寸法校正ファントム

            //v16.2/17.0 added by Isshiki
            for (Index = 1; Index <= 2; Index++)
            {
                //変更2015/02/02hata_Max/Min範囲のチェック
                //cwne2R[Index].Value = (decimal)ScanCorrect.GVal_DistVal[Index];
                cwne2R[Index].Value = modLibrary.CorrectInRange((decimal)ScanCorrect.GVal_DistVal[Index], cwne2R[Index].Minimum, cwne2R[Index].Maximum);
            }
            // v16.2/17.0

            //現在のFDD・FCDオフセットを表示する     'v15.0追加 by 間々田 従来のスキャン条件で表示していたものをここに移動
            //FDD/FCDオフセットを表示
            //Rev23.10 計測CT対応 by長野 2015/10/18
            if (CTSettings.scaninh.Data.cm_mode == 0)//FCD,FDDの表示桁が増えた分、寸法校正の表示桁も増やす
            {
                lblFidOffset.Text = CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube].ToString("0.0000");
                lblFcdOffset.Text = CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()].ToString("0.000000");		//FDDオフセットは小数点以下４桁とする by 間々田 2005/11/29
            }
            else
            {
                lblFidOffset.Text = CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube].ToString("0.00");
                lblFcdOffset.Text = CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()].ToString("0.0000");		//FDDオフセットは小数点以下４桁とする by 間々田 2005/11/29
            }
        }
        //*******************************************************************************
        //機　　能： 参照ボタンクリック時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.12 16/01/09  (検S1)長野      新規作成
        //*******************************************************************************
        private void cmdDialog_Click(object sender, EventArgs e)
        {
            int Index = -1;
            for (int i = 0; i < cmdDialog.Length; i++)
            {
                if (sender.Equals(cmdDialog[i]))
                {
                    Index = i;
                    break;
                }
            }

            //Dim FileName As String
            //FileName = GetFileName(IDS_Select, LoadResString(12105), ".img")
            //If FileName <> "" Then txtImgFileName(Index).Text = FileName

            //v16.20/v17.00変更(ここから) byやまおか 2010/03/04
            string FileName = null;
            //modImageInfo.ImageInfoStruct theInfoRec = default(modImageInfo.ImageInfoStruct);
            ImageInfo theInfoRec = new ImageInfo();
            theInfoRec.Data.Initialize();

            //FileName = modFileIO.GetFileName(StringTable.IDS_Select, CTResources.LoadResString(StringTable.IDS_Select), ".img");
            FileName = modFileIO.GetFileName(StringTable.IDS_Select, CTResources.LoadResString(12105), ".img");

            if (!string.IsNullOrEmpty(FileName))
            {
                //if (!modImageInfo.ReadImageInfo(ref theInfoRec, modLibrary.RemoveExtension(FileName, ".img")))
                //if (ImageInfo.ReadImageInfo(ref theInfoRec.Data, modLibrary.RemoveExtension(FileName, ".img")))
                //Rev20.00 判定を逆に変更 by長野 2014/12/04
                if (!ImageInfo.ReadImageInfo(ref theInfoRec.Data, modLibrary.RemoveExtension(FileName, ".img")))
                {
                    //メッセージ表示：付帯情報のある画像ファイルを選択してください。
                    MessageBox.Show(CTResources.LoadResString(8127), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //If StrComp(RemoveNull(theInfoRec.full_mode), "FULL") Then
                //v16.20/v17.00修正 byやまおか 2010/03/08
                if (string.Compare(modLibrary.RemoveNull(theInfoRec.Data.full_mode.GetString()), "FULL", true) != 0)
                {
                    //メッセージ表示：スキャンモードがフルスキャンの画像を選択してください。
                    MessageBox.Show(CTResources.LoadResString(9336), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                txtImgFileName[Index].Text = FileName;
            }
            //v16.20/v17.00変更(ここまで) byやまおか 2010/03/04
        }
        //*******************************************************************************
        //機　　能： 参照ボタンクリック時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.12 16/01/09  (検S1)長野      新規作成
        //*******************************************************************************
        private void btnRef_Click(object sender, EventArgs e)
        {
            int Index = -1;
            for (int i = 0; i < btnRef.Length; i++)
            {
                if (sender.Equals(btnRef[i]))
                {
                    Index = i;
                    break;
                }
            }

            //v16.20/v17.00変更(ここから) byやまおか 2010/03/04
            string FileName = null;
            ImageInfo theInfoRec = new ImageInfo();
            theInfoRec.Data.Initialize();

            FileName = modFileIO.GetFileName(StringTable.IDS_Select, CTResources.LoadResString(12105), ".img");

            if (!string.IsNullOrEmpty(FileName))
            {
                //Rev20.00 判定を逆に変更 by長野 2014/12/04
                if (!ImageInfo.ReadImageInfo(ref theInfoRec.Data, modLibrary.RemoveExtension(FileName, ".img")))
                {
                    //メッセージ表示：付帯情報のある画像ファイルを選択してください。
                    MessageBox.Show(CTResources.LoadResString(8127), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //v16.20/v17.00修正 byやまおか 2010/03/08
                if (string.Compare(modLibrary.RemoveNull(theInfoRec.Data.full_mode.GetString()), "FULL", true) != 0)
                {
                    //メッセージ表示：スキャンモードがフルスキャンの画像を選択してください。
                    MessageBox.Show(CTResources.LoadResString(9336), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                txtImgFileNamePhantomFree[Index].Text = FileName;
            }
        }
        //*******************************************************************************
        //機　　能： 終了処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V23.12 16/01/09  (検S1)長野      新規作成
        //*******************************************************************************
        private void frmDistanceCorrectNew_FormClosed(object sender, FormClosedEventArgs e)
        {
            //終了時はフラグをリセット
            modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTScanCorrect);

            //Rev20.00 showdialogで呼ばれているのでdisposeを使ってリソース破棄 by長野 2014/12/04
            this.Dispose();
        }
        //********************************************************************************
        //機    能  ：  寸法校正実行
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：                   [ /O] Boolean   結果(True:正常,False:異常)
        //補    足  ：  ScanCorrect.bas の Get_DistanceCorrect_Parameter_Exをリニューアルした
        //
        //履    歴  ：  V9.7   04/11/11  (SI4)間々田     　　新規作成
        //             V16.2/17.0   10/02/23  Isshiki/Iwasawa     2つの位置の画像による寸法校正でFCD,FDDｵﾌｾｯﾄ値を求めるように変更
        //********************************************************************************
        private bool DoDistanceCorrect(string FileName)
        {
            int rc = 0;
            //Ipc32v5.RECT tmpReg = default(Ipc32v5.RECT);
            int isize = 0;
            float icount = 0;
            int cnt = 0;
            string clptxt = null;               //ImageProのクリップボード経由の測定データ
            string swork = null;                //カウント値をclptxtから取り出したもの
            int iMode = 0;                      //データモード（0:ﾊｰﾌ 1:ﾌﾙ 2:ｵﾌｾｯﾄ）
            float max_scan_area = 0;            //寸法校正画像の最大スキャンエリア
            float scan_area = 0;                //寸法校正画像のスキャンエリア
            int Max1 = 0;                       //画像の最大値（1～16384）
            int Min1 = 0;                       //画像の最小値（-8192～8191）
            //   Dim theInfoRec      As ImageInfoStruct  'Rev16.20/v17.00 配列にする 2010/02/14 by Iwasawa
            int DotPos = 0;
            //int ConeiMode = 0;                  //データモード（0:ﾊｰﾌ,ﾌﾙ 1:ｵﾌｾｯﾄ）   'added by 山本 2007-2-12

            //Rev16.2/17.0 added by Isshiki
            float[] fcd_offset = new float[3];  //FCDのｵﾌｾｯﾄ値
            float[] fdd_offset = new float[3];  //FDDのｵﾌｾｯﾄ値
            float[] mecaFCD = new float[3];     //FCD値(ｵﾌｾｯﾄ値を含まない)
            float[] mecaFDD = new float[3];     //FDD値(ｵﾌｾｯﾄ値を含まない)
            float[] FCD = new float[3];         //FCD値(ｵﾌｾｯﾄ値を含む)
            float[] FDD = new float[3];         //FDD値(ｵﾌｾｯﾄ値を含む)
            float[] d = new float[3];           //2値化した画像から求めた寸法校正用ファントムの半径
            float[] PixelSize = new float[3];   //1画素サイズ
            string[] iifield = new string[3];   //I.I.視野
            int i = 0;                          //カウンタ

            //modImageInfo.ImageInfoStruct[] theInfoRec = new modImageInfo.ImageInfoStruct[3];  //付帯情報 2010/02/24 by added by Iwasawa
            CTstr.IMAGEINFO[] theInfoRec = new CTstr.IMAGEINFO[3]; ;
            for (i = 0; i <= 2; i++)
            {
                theInfoRec[i].Initialize();
            }


            //戻り値初期化
            bool functionReturnValue = false;

            //Rev16.2/17.0 入力画像チェック 2つの画像で、FCDが異なる ＆ 同一の寸法校正状態で撮影されている(fcd_offsetとfid_offsetがそれぞれ等しい)　2010/02/24 by Iwasawa
            for (i = 1; i <= 2; i++)
            {
                //Rev20.00 判定を逆に変更 by長野 2014/12/04
                if (!ImageInfo.ReadImageInfo(ref theInfoRec[i], modLibrary.RemoveExtension(txtImgFileName[i].Text, ".img")))
                {
                    //メッセージ表示：
                    //付帯情報のある画像ファイルを選択してください。
                    MessageBox.Show(CTResources.LoadResString(8127), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return functionReturnValue;
                }
            }

            if (!(theInfoRec[1].fcd_offset == theInfoRec[2].fcd_offset) && theInfoRec[1].fid_offset == theInfoRec[2].fid_offset)
            {
                //メッセージ表示：
                //2つの画像を撮影した間に、寸法校正が1回以上行われています。同一の寸法校正状態で撮影した画像を選択してください。
                MessageBox.Show(CTResources.LoadResString(9327), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //9327はRev16.2/17.0で追加
                return functionReturnValue;
            }

            //Rev23.20 2世代・3世代兼用の場合、FCD不動のシステムのためチェックしない by長野 2016/01/12
            if (CTSettings.scaninh.Data.ct_gene2and3 != 0)
            {
                if (theInfoRec[1].fcd == theInfoRec[2].fcd)
                {
                    //メッセージ表示：
                    //2つの画像はFCDが同じです。FCDを変えて撮影した画像を選択してください。
                    MessageBox.Show(CTResources.LoadResString(9334), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //9334はRev16.2/17.0で追加
                    return functionReturnValue;
                }
                //チェック完
            }

            //v17.30 追加　by 長野　2010-09-27
            if (theInfoRec[1].detector != theInfoRec[2].detector)
            {
                //メッセージ表示
                //異なる検出器で撮影した画像が選択されました。同一の検出器で撮影した画像を選択してください。
                MessageBox.Show(CTResources.LoadResString(9337), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return functionReturnValue;
            }

            //変更2014/10/07hata_v19.51反映
            if (CTSettings.SecondDetOn)
            {
                if (((int)CTSettings.detectorParam.DetType != theInfoRec[1].detector))
                {
                    if (mod2ndDetctor.IsDet1mode)
                    {
                        //検出器１で撮影した画像を選択してください。
                        //Interaction.MsgBox(CT30K.My.Resources.str9338, MsgBoxStyle.Exclamation);
                        MessageBox.Show(CTResources.LoadResString(9338), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return functionReturnValue;
                    }
                    if (mod2ndDetctor.IsDet2mode)
                    {
                        //メッセージ表示
                        //検出器２で撮影した画像を選択してください。
                        //Interaction.MsgBox(CT30K.My.Resources.str9339, MsgBoxStyle.Exclamation);
                        MessageBox.Show(CTResources.LoadResString(9339), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return functionReturnValue;
                    }
                }
            }

            ////added by 一色
            //値を保存する配列を作成する。　　'//V16.2/17.0追加　2009/10/27 by Isshiki/Iwasawa
            //Dim i As Integer
            //i = 1
            for (i = 1; i <= 2; i++)
            {
                //
                //寸法校正画像を開く
                if (!File.Exists(txtImgFileName[i].Text))
                {
                    //メッセージ表示：
                    //   付帯情報のある画像ファイルを選択してください。
                    //MsgBox LoadResString(8127), vbExclamation

                    //   画像ファイル読み込みができません。
                    //   ファイルの有無を確認後、寸法校正を再度行ってください。
                    MessageBox.Show(CTResources.LoadResString(9455), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return functionReturnValue;
                }

                //入力画像のサイズからレイアウトを判定する
                FileInfo fileInfo = new FileInfo(txtImgFileName[i].Text);
                //2014/11/06hata キャストの修正
                ScanCorrect.Xsize_D = Convert.ToInt32(Math.Sqrt(fileInfo.Length / 2F));
                ScanCorrect.Ysize_D = ScanCorrect.Xsize_D;

                //領域確保
                ScanCorrect.DISTANCE_IMAGE = new ushort[ScanCorrect.Xsize_D * ScanCorrect.Ysize_D];

                if (ScanCorrect.ImageOpen(ref ScanCorrect.DISTANCE_IMAGE[0],
                                          txtImgFileName[i].Text, ScanCorrect.Xsize_D, ScanCorrect.Ysize_D) != 0)
                {
                    //メッセージ表示：
                    //   画像ファイル読み込みができません。
                    //   ファイルの有無を確認後、寸法校正を再度行ってください。
                    MessageBox.Show(CTResources.LoadResString(9455), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return functionReturnValue;
                }

                //マウスカーソルを元に戻す
                this.Cursor = Cursors.Default;

                //画像の最大値、最小値を求める
                ScanCorrect.GetMaxMin(ref ScanCorrect.DISTANCE_IMAGE[0],
                                      ScanCorrect.Xsize_D, ScanCorrect.Ysize_D, ref Min1, ref Max1);

                //２値化画像を表示する
                //If Not frmDistanceBinarized.Dialog(Min1, Max1, Threshold255_D) Then
                //v17.10変更 Captionに○/2を追加 byやまおか 2010/08/09
                if (!frmDistanceBinarized.Instance.Dialog(Min1, Max1, ref ScanCorrect.Threshold255_D, Convert.ToString(i) + "/2"))
                {
                    functionReturnValue = true;
                    return functionReturnValue;
                }

                //マウスカーソルを砂時計にする
                this.Cursor = Cursors.WaitCursor;

                //画像の２値化      'commented by 山本 99-6-5
                //☆☆２値化画像に変換(CT用)

                ScanCorrect.BIN_IMAGE = new ushort[ScanCorrect.Xsize_D * ScanCorrect.Ysize_D];
                //v15.0変更 -1した by 間々田 2009/06/03
                //       Call BinarizeImage(DISTANCE_IMAGE(0), BIN_IMAGE(0), Xsize_D, Ysize_D, Threshold255_D, 1, 1)
                ScanCorrect.BinarizeImage_signed(ref ScanCorrect.DISTANCE_IMAGE[0], ref ScanCorrect.BIN_IMAGE[0],
                                                 ScanCorrect.Xsize_D, ScanCorrect.Ysize_D, ScanCorrect.Threshold255_D, 1, 1);   //v9.7変更 by 間々田 2004-12-09 符号付Short型配列対応


                rc = CallImageProFunction.CallDoDistanceCorrectStep1(ScanCorrect.BIN_IMAGE, ScanCorrect.Ysize_D, ScanCorrect.Xsize_D);
 
                //クリップボード・データの取得
                IDataObject datObj = Clipboard.GetDataObject();
                if (datObj.GetDataPresent(DataFormats.Text))
                {
                    clptxt = (string)datObj.GetData(DataFormats.Text);
                }

                //クリップボードから得た測定値の中の、データ部を取り出す
                swork = "";
                for (cnt = 1; cnt <= clptxt.Length; cnt++)
                {
                    char ch = clptxt.Substring(cnt - 1, 1)[0];  // String → char
                    if ((int)ch < 0x20)
                    {
                        //CRを検出したら、その直前のデータがデータ部
                        if ((int)ch == 13)
                        {
                            break;
                        }
                        swork = "";
                    }
                    else
                    {
                        //制御記号が出てくるまでは文字を加算する
                        swork = swork + clptxt.Substring(cnt - 1, 1);
                    }
                }
                float.TryParse(swork.Trim(), out icount);
                //icount = icount * 1.0048999   'イメージプロによる直径測定に誤差のための調整 99-9-28 by 山本    'deleted by 山本　2005-10-22
                icount = (float)(2 * Math.Sqrt(icount / ScanCorrect.Pai));
                //面積から直径を求める　added by 山本　2005-10-22
                if (icount <= 0)
                {
                    goto Err_Process;
                }

                ScanCorrect.GVal_2R_D = icount;//V4.0 append by 鈴山 2001/02/05
                DotPos = txtImgFileName[i].Text.LastIndexOf('.') + 1;
                iMode = modCommon.MyCtinfdef.full_mode.GetIndexByStr(modLibrary.RemoveNull(theInfoRec[i].full_mode.GetString()), 0);

                ScanCorrect.GFlg_ScanMode_D = (short)(iMode + 1);
                //V4.0 append by 鈴山 2001/02/05

                //最大スキャンエリア
                max_scan_area = theInfoRec[i].max_mscan_area;           //Rev16.2/17.0 変更 2010/02/25 by Iwasawa

                //スキャンエリア
                float scale = 0.0F;
                float.TryParse(theInfoRec[i].scale.GetString(), out scale);
                //2014/11/06hata キャストの修正
                scan_area = scale / 1000F;    //Rev16.2/17.0 変更 2010/02/25 by Iwasawa

                //付帯情報の回転選択可否が可の場合、付帯情報の回転選択の値（0:テーブル回転 1:Ｘ線管回転）を代入  'added by 間々田 2004/02/18
                //        If theInfoRec.rotate_select_inh = 0 Then GFlg_MultiTube_D = theInfoRec.rotate_select
                if (CTSettings.scaninh.Data.rotate_select == 0)
                {
                    //GFlg_MultiTube_D = theInfoRec.rotate_select 'change by 間々田 2004/06/03 コモンscaninh.rotate_selectを参照する
                    ScanCorrect.GFlg_MultiTube_D = (short)theInfoRec[i].rotate_select; //Rev16.2/17.0 変更 2010/02/25 by Iwasawa
                }
                else
                {
                    //X線管(0:160kV,1:225kV)                     'added by 山本 2000-8-19 三菱対応
                    //GFlg_MultiTube_D = Val(theInfoRec.Focus)
                    int.TryParse(theInfoRec[i].focus.GetString(), out ScanCorrect.GFlg_MultiTube_D);    //Rev16.2/17.0 変更 2010/02/25 by Iwasawa
                }

                //必ず０か１にするための措置 by 間々田 2006/01/18
                if (ScanCorrect.GFlg_MultiTube_D != 1)
                {
                    ScanCorrect.GFlg_MultiTube_D = 0;
                }

                //End If 'Rev16.2/17.0 付帯情報は先に読み込むので削除したif文のカッコ 2010/02/24 by Iwasawsa

                isize = ScanCorrect.Xsize_D;

                //ＦＣＤ=(最大スキャンエリア / (2*sin(fanangle[0]/2))
                //最大スキャンエリア = (isize / icount)　* 実円柱直径 * (元の最大スキャンエリア / 元のスキャンエリア)
                switch (iMode)
                {
                    //校正画像のデータモード
                    case 0:
                    case 1:
                        //GVal_ScnAreaMax(0) = (isize / icount) * GVal_DistVal * max_scan_area / scan_area    'V4.0 change by 鈴山 2001/02/01 (cwne2R.Value → GVal_DistVal)
                        //GVal_ScnAreaMax(1) = (isize / icount) * GVal_DistVal * max_scan_area / scan_area    'V4.0 change by 鈴山 2001/02/01 (cwne2R.Value → GVal_DistVal)
                        //2014/11/06hata キャストの修正
                        ScanCorrect.GVal_ScnAreaMax[0] = ((float)isize / icount) * ScanCorrect.GVal_DistVal[i] * max_scan_area / scan_area;//v16.2/17.0 GVal_DistValを配列化 2010/02/25 by Iwasawa
                        ScanCorrect.GVal_ScnAreaMax[1] = ((float)isize / icount) * ScanCorrect.GVal_DistVal[i] * max_scan_area / scan_area;//v16.2/17.0 GVal_DistValを配列化 2010/02/25 by Iwasawa

                        //未使用のため削除　2014/06/17hata
                        //ConeiMode = 0;    //added by 山本 2007-2-12
                        break;
                    case 2:
                        //GVal_ScnAreaMax(2) = (isize / icount) * GVal_DistVal * max_scan_area / scan_area    'V4.0 change by 鈴山 2001/02/01 (cwne2R.Value → GVal_DistVal)
                        //2014/11/06hata キャストの修正
                        ScanCorrect.GVal_ScnAreaMax[2] = ((float)isize / icount) * ScanCorrect.GVal_DistVal[i] * max_scan_area / scan_area;//v16.2/17.0 GVal_DistValを配列化 2010/02/25 by Iwasawa

                        //未使用のため削除　2014/06/17hata
                        //ConeiMode = 1;    //added by 山本 2007-2-12
                        break;
                    default:
                        break;
                }

                //スキャンエリアの設定（現在のスキャン条件のデータモードでの最大スキャンエリアを書込む）
                ScanCorrect.GVal_MScnArea = ScanCorrect.GVal_ScnAreaMax[CTSettings.scansel.Data.scan_mode - 1];

                fcd_offset[i] = theInfoRec[i].fcd_offset;                  //FCDのオフセット値
                fdd_offset[i] = theInfoRec[i].fid_offset;                  //FDDのオフセット値
                mecaFCD[i] = theInfoRec[i].fcd - theInfoRec[i].fcd_offset;        //FCDメカの読値 修正 by Iwasawa
                mecaFDD[i] = theInfoRec[i].fid - theInfoRec[i].fid_offset;        //FCDメカの読値 修正 by Iwasawa
                FCD[i] = theInfoRec[i].fcd;                                //FCD値(ｵﾌｾｯﾄを含む) 修正 by Iwasawa
                FDD[i] = theInfoRec[i].fid;                                //FDD値(ｵﾌｾｯﾄを含む) 修正 by Iwasawa
                //2014/11/06hata キャストの修正
                //PixelSize[i] = (float)(Convert.ToDouble(theInfoRec[i].scale) / 1000D / Convert.ToDouble(theInfoRec[i].matsiz));	//1画素サイズ(mm)
                //Rev20.00 GetString追加 by長野 2014/12/04
                PixelSize[i] = (float)(Convert.ToDouble(theInfoRec[i].scale.GetString()) / 1000D / Convert.ToDouble(theInfoRec[i].matsiz.GetString()));	//1画素サイズ(mm)

                iifield[i] = theInfoRec[i].iifield.GetString();                         //I.I.視野
       
                //Rev16.2/17.0 added by Isshiki
                //GVal_Fcd = FCD(i)
                ScanCorrect.GVal_Fcd2 = FCD[i];
                //v17.60変更 byやまおか 2011/06/10
                //以下は他でも使用のためあえて配列にしない 2010/02/25 コメント by Iwasawa
                if (i == 1)     //1個目の各値を保持する
                {
                    ScanCorrect.GVal_ScnAreaMax1[iMode] = ScanCorrect.GVal_ScnAreaMax[iMode];
                    ScanCorrect.GVal_2R_D1 = ScanCorrect.GVal_2R_D;
                    ScanCorrect.GVal_Fcd1 = FCD[1];
                }
                //

                //Image-Pro 画像データの取得
                if (ScanCorrect.IMGPRODBG == 1)
                {
                    #region //ImageProの処理はImageProHelperを使用(ここから)-------------//
                    rc = CallImageProFunction.CallDoDistanceCorrectStep2(ScanCorrect.DISTANCE_IMAGE, ScanCorrect.Ysize_D, ScanCorrect.Xsize_D);
                    #endregion //ImageProの処理はImageProHelperを使用（ここまで）-------------//
                }

                functionReturnValue = true;

                //寸法校正フォームを隠す
                //Me.hide
                //1枚目の画像のときは隠さない
                if (i == 2) //v16.20/v17.00変更 byやまおか 2010/03/01
                {
                    //変更2015/1/17hata_非表示のときにちらつくため
                    //this.Hide();
                    modCT30K.FormHide(this);
                }

                //Rev23.20
                //寸法校正1点の場合は、配列2番目に1番目の内容をコピーしてループを抜ける by長野 2016/01/16
                if (CTSettings.scaninh.Data.ct_gene2and3 == 0 && i == 1)
                {
                    fcd_offset[2] = fcd_offset[1];                  //FCDのオフセット値
                    fdd_offset[2] = fdd_offset[1];                  //FDDのオフセット値
                    mecaFCD[2] = mecaFCD[1];                        //FCDメカの読値 修正 by Iwasawa
                    mecaFDD[2] = mecaFDD[1];                        //FCDメカの読値 修正 by Iwasawa
                    FCD[2] = FCD[1];                                //FCD値(ｵﾌｾｯﾄを含む) 修正 by Iwasawa
                    FDD[2] = FDD[1];                                //FDD値(ｵﾌｾｯﾄを含む) 修正 by Iwasawa
                    PixelSize[2] = PixelSize[1];                	//1画素サイズ(mm)
                    iifield[2] = iifield[1];                        //I.I.視野

                    //変更2015/1/17hata_非表示のときにちらつくため
                    //this.Hide();
                    modCT30K.FormHide(this);

                    break;
                }

            }   //added by 一色

            //画像から求めた円柱ファントムの直径を計算する
            d[1] = ScanCorrect.GVal_2R_D1 * PixelSize[1];
            d[2] = ScanCorrect.GVal_2R_D * PixelSize[2];

            //Rev16.2/17.0 オフセット値の誤差を計算する。 2つの画像でFCD,FDD,ファントム直径がそれぞれ異なっていても求まる計算式 by Isshiki
            //        2つの画像のｵﾌｾｯﾄを含むFCD,FDDをメカ読み値として計算し、求まったｵﾌｾｯﾄから元画像のｵﾌｾｯﾄ値を引く
            //Rev23.20 X軸(旧Y軸)が動かない場合は、旧寸法校正に変える by長野 2016/01/20
            if (CTSettings.scaninh.Data.table_y == 0)
            {
                ScanCorrect.GVal_Fcd_O = FCD[1] * FCD[2] * (FDD[1] * (float)cwne2R[2].Value * (d[1] - (float)cwne2R[1].Value) - FDD[2] * (float)cwne2R[1].Value * (d[2] - (float)cwne2R[2].Value)) / (FCD[1] * FDD[2] * (float)cwne2R[1].Value * d[2] - FCD[2] * FDD[1] * d[1] * (float)cwne2R[2].Value);
                ScanCorrect.GVal_Fid_O = FDD[1] * FDD[2] * (FCD[1] * d[2] * (d[1] - (float)cwne2R[1].Value) - FCD[2] * d[1] * (d[2] - (float)cwne2R[2].Value)) / (FCD[1] * FDD[2] * (float)cwne2R[1].Value * d[2] - FCD[2] * FDD[1] * d[1] * (float)cwne2R[2].Value);

                //真のFCDオフセット、FDDオフセット値 1、2どちらの値を使っても良い
                //GVal_Fcd_O = fcd_offset(2) + GVal_Fcd_O
                //GVal_Fid_O = fid_offset(2) + GVal_Fid_O

                ScanCorrect.GVal_Fcd_O = fcd_offset[2] + ScanCorrect.GVal_Fcd_O;
                ScanCorrect.GVal_Fid_O = fdd_offset[2] + ScanCorrect.GVal_Fid_O;


                //オフセット値を含んだFCD,FDDの値を計算　求まった値がFDD>FCDであることのチェック用なので1、2どちらの値を使っても良い
                ScanCorrect.GVal_Fcd_D = mecaFCD[2] + ScanCorrect.GVal_Fcd_O;
                ScanCorrect.GVal_Fid_D = mecaFDD[2] + ScanCorrect.GVal_Fid_O;
            }
            else
            {
                //旧寸法校正は、FDDは校正しない。
                ScanCorrect.GVal_Fcd_D = (float)((double)ScanCorrect.GVal_ScnAreaMax[iMode] / ((double)2 * Math.Sin((double)CTSettings.scancondpar.Data.mfanangle[iMode + 2 * 4] * ScanCorrect.Pai / (double)180 / (double)2)));
                ScanCorrect.GVal_Fcd_O = ScanCorrect.GVal_Fcd_D - mecaFCD[1]; 
            }
            //Rev16.2/17.0 2つの画像でFCD,FDD,ファントム直径がそれぞれ異なっていても求まる計算式にしておく 2010/02/25 by Iwasawa
            //        2つの画像のｵﾌｾｯﾄを含むFCD,FDDをメカ読み値として計算し、求まったｵﾌｾｯﾄから元画像のｵﾌｾｯﾄ値を引く



            //寸法校正結果フォームを表示する
            frmDistanceCorrectResult.Instance.ShowDialog();
            return functionReturnValue;

        Err_Process:
            //メッセージ表示：
            //   寸法校正ファントムの２値化抽出に失敗しました。
            //   事前に必要な校正を正しく行っていない可能性があります。
            //   幾何歪校正・回転中心校正を実行後、寸法校正を再度行ってください。
            //変更2014/11/18hata_MessageBox確認
            //MessageBox.Show(CTResources.LoadResString(9523), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            MessageBox.Show(CTResources.LoadResString(9523), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            return functionReturnValue;
        }
        //********************************************************************************
        //機    能  ：  寸法校正実行（寸法校正ファントムなし）
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：                   [ /O] Boolean   結果(True:正常,False:異常)
        //補    足  ：  ScanCorrect.bas の Get_DistanceCorrect_Parameter_Exをリニューアルした
        //
        //履    歴  ： V23.12  15/12/12   (検S1)長野   新規作成
        //            
        //********************************************************************************
        private bool DoDistanceCorrectByPhantomFree()
        {
            float[] fcd_offset = new float[3];  //FCDのｵﾌｾｯﾄ値
            float[] fdd_offset = new float[3];  //FDDのｵﾌｾｯﾄ値
            float[] mecaFCD = new float[3];     //FCD値(ｵﾌｾｯﾄ値を含まない)
            float[] mecaFDD = new float[3];     //FDD値(ｵﾌｾｯﾄ値を含まない)
            float[] FCD = new float[3];         //FCD値(ｵﾌｾｯﾄ値を含む)
            float[] FDD = new float[3];         //FDD値(ｵﾌｾｯﾄ値を含む)
            float[] d = new float[3];           //2値化した画像から求めた寸法校正用ファントムの半径
            float[] PixelSize = new float[3];   //1画素サイズ
            string[] iifield = new string[3];   //I.I.視野
            int i = 0;                          //カウンタ

            CTstr.IMAGEINFO[] theInfoRec = new CTstr.IMAGEINFO[3]; ;
            for (i = 0; i <= 2; i++)
            {
                theInfoRec[i].Initialize();
            }

            //戻り値初期化
            bool functionReturnValue = false;

            //Rev16.2/17.0 入力画像チェック 2つの画像で、FCDが異なる ＆ 同一の寸法校正状態で撮影されている(fcd_offsetとfid_offsetがそれぞれ等しい)　2010/02/24 by Iwasawa
            for (i = 1; i <= 2; i++)
            {
                //Rev20.00 判定を逆に変更 by長野 2014/12/04
                if (!ImageInfo.ReadImageInfo(ref theInfoRec[i], modLibrary.RemoveExtension(txtImgFileNamePhantomFree[i].Text, ".img")))
                {
                    //メッセージ表示：
                    //付帯情報のある画像ファイルを選択してください。
                    MessageBox.Show(CTResources.LoadResString(8127), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return functionReturnValue;
                }
            }

            if (!(theInfoRec[1].fcd_offset == theInfoRec[2].fcd_offset) && theInfoRec[1].fid_offset == theInfoRec[2].fid_offset)
            {
                //メッセージ表示：
                //2つの画像を撮影した間に、寸法校正が1回以上行われています。同一の寸法校正状態で撮影した画像を選択してください。
                MessageBox.Show(CTResources.LoadResString(9327), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //9327はRev16.2/17.0で追加
                return functionReturnValue;
            }

            if (theInfoRec[1].fcd == theInfoRec[2].fcd)
            {
                //メッセージ表示：
                //2つの画像はFCDが同じです。FCDを変えて撮影した画像を選択してください。
                MessageBox.Show(CTResources.LoadResString(9334), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //9334はRev16.2/17.0で追加
                return functionReturnValue;
            }
            //チェック完

            //v17.30 追加　by 長野　2010-09-27
            if (theInfoRec[1].detector != theInfoRec[2].detector)
            {
                //メッセージ表示
                //異なる検出器で撮影した画像が選択されました。同一の検出器で撮影した画像を選択してください。
                MessageBox.Show(CTResources.LoadResString(9337), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return functionReturnValue;
            }

            //変更2014/10/07hata_v19.51反映
            if (CTSettings.SecondDetOn)
            {
                if (((int)CTSettings.detectorParam.DetType != theInfoRec[1].detector))
                {
                    if (mod2ndDetctor.IsDet1mode)
                    {
                        //検出器１で撮影した画像を選択してください。
                        //Interaction.MsgBox(CT30K.My.Resources.str9338, MsgBoxStyle.Exclamation);
                        MessageBox.Show(CTResources.LoadResString(9338), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return functionReturnValue;
                    }
                    if (mod2ndDetctor.IsDet2mode)
                    {
                        //メッセージ表示
                        //検出器２で撮影した画像を選択してください。
                        //Interaction.MsgBox(CT30K.My.Resources.str9339, MsgBoxStyle.Exclamation);
                        MessageBox.Show(CTResources.LoadResString(9339), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return functionReturnValue;
                    }
                }
            }

            //値を保存する配列を作成する。　　'//V16.2/17.0追加　2009/10/27 by Isshiki/Iwasawa
            for (i = 1; i <= 2; i++)
            {
                //付帯情報の回転選択可否が可の場合、付帯情報の回転選択の値（0:テーブル回転 1:Ｘ線管回転）を代入  'added by 間々田 2004/02/18
                if (CTSettings.scaninh.Data.rotate_select == 0)
                {
                    ScanCorrect.GFlg_MultiTube_D = (short)theInfoRec[i].rotate_select; //Rev16.2/17.0 変更 2010/02/25 by Iwasawa
                }
                else
                {
                    //X線管(0:160kV,1:225kV)                     'added by 山本 2000-8-19 三菱対応
                    int.TryParse(theInfoRec[i].focus.GetString(), out ScanCorrect.GFlg_MultiTube_D);    //Rev16.2/17.0 変更 2010/02/25 by Iwasawa
                }

                //必ず０か１にするための措置 by 間々田 2006/01/18
                if (ScanCorrect.GFlg_MultiTube_D != 1)
                {
                    ScanCorrect.GFlg_MultiTube_D = 0;
                }

                fcd_offset[i] = theInfoRec[i].fcd_offset;                  //FCDのオフセット値
                fdd_offset[i] = theInfoRec[i].fid_offset;                  //FDDのオフセット値
                mecaFCD[i] = theInfoRec[i].fcd - theInfoRec[i].fcd_offset;        //FCDメカの読値 修正 by Iwasawa
                mecaFDD[i] = theInfoRec[i].fid - theInfoRec[i].fid_offset;        //FCDメカの読値 修正 by Iwasawa
                FCD[i] = theInfoRec[i].fcd;                                //FCD値(ｵﾌｾｯﾄを含む) 修正 by Iwasawa
                FDD[i] = theInfoRec[i].fid;                                //FDD値(ｵﾌｾｯﾄを含む) 修正 by Iwasawa

                if (i == 1)
                {
                    ScanCorrect.GVal_Fcd1 = FCD[i];
                }
                else if (i == 2)
                {
                    ScanCorrect.GVal_Fcd2 = FCD[i];
                }

                functionReturnValue = true;

                //Rev23.20
                //寸法校正1点の場合は、配列2番目に1番目の内容をコピーしてループを抜ける by長野 2016/01/16
                if (CTSettings.scaninh.Data.ct_gene2and3 == 0 && i == 1)
                {
                    fcd_offset[2] = fcd_offset[1];                  //FCDのオフセット値
                    fdd_offset[2] = fdd_offset[1];                  //FDDのオフセット値
                    mecaFCD[2] = mecaFCD[1];                        //FCDメカの読値 修正 by Iwasawa
                    mecaFDD[2] = mecaFDD[1];                        //FCDメカの読値 修正 by Iwasawa
                    FCD[2] = FCD[1];                                //FCD値(ｵﾌｾｯﾄを含む) 修正 by Iwasawa
                    FDD[2] = FDD[1];                                //FDD値(ｵﾌｾｯﾄを含む) 修正 by Iwasawa

                    //変更2015/1/17hata_非表示のときにちらつくため
                    //this.Hide();
                    modCT30K.FormHide(this);

                    break;
                }
            }

            //CT画像を使って計測した値を代入
            d[1] = (float)cwne2R_ByCT[1].Value;
            d[2] = (float)cwne2R_ByCT[2].Value;

            //Rev16.2/17.0 オフセット値の誤差を計算する。 2つの画像でFCD,FDD,ファントム直径がそれぞれ異なっていても求まる計算式 by Isshiki
            //        2つの画像のｵﾌｾｯﾄを含むFCD,FDDをメカ読み値として計算し、求まったｵﾌｾｯﾄから元画像のｵﾌｾｯﾄ値を引く
            ScanCorrect.GVal_Fcd_O = FCD[1] * FCD[2] * (FDD[1] * (float)cwne2R_ByMeasure[2].Value * (d[1] - (float)cwne2R_ByMeasure[1].Value) - FDD[2] * (float)cwne2R_ByMeasure[1].Value * (d[2] - (float)cwne2R_ByMeasure[2].Value)) / (FCD[1] * FDD[2] * (float)cwne2R_ByMeasure[1].Value * d[2] - FCD[2] * FDD[1] * d[1] * (float)cwne2R_ByMeasure[2].Value);
            ScanCorrect.GVal_Fid_O = FDD[1] * FDD[2] * (FCD[1] * d[2] * (d[1] - (float)cwne2R_ByMeasure[1].Value) - FCD[2] * d[1] * (d[2] - (float)cwne2R_ByMeasure[2].Value)) / (FCD[1] * FDD[2] * (float)cwne2R_ByMeasure[1].Value * d[2] - FCD[2] * FDD[1] * d[1] * (float)cwne2R_ByMeasure[2].Value);

            ScanCorrect.GVal_Fcd_O = fcd_offset[2] + ScanCorrect.GVal_Fcd_O;
            ScanCorrect.GVal_Fid_O = fdd_offset[2] + ScanCorrect.GVal_Fid_O;

            //オフセット値を含んだFCD,FDDの値を計算　求まった値がFDD>FCDであることのチェック用なので1、2どちらの値を使っても良い
            ScanCorrect.GVal_Fcd_D = mecaFCD[2] + ScanCorrect.GVal_Fcd_O;
            ScanCorrect.GVal_Fid_D = mecaFDD[2] + ScanCorrect.GVal_Fid_O;

            //寸法校正結果フォームを表示する
            frmDistanceCorrectResultPhantomFree.Instance.ShowDialog();

            return functionReturnValue;

        }
    }
}
