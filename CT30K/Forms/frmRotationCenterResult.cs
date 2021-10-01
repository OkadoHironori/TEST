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
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： frmRotationCenterResult.frm                                 */
    ///* 処理概要　　： 回転中心校正結果                                            */
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
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
    public partial class frmRotationCenterResult : Form
    {
        #region インスタンスを返すプロパティ

        // frmRotationCenterResultのインスタンス
        private static frmRotationCenterResult _Instance = null;

        /// <summary>
        /// frmRotationCenterResultのインスタンスを返す
        /// </summary>
        public static frmRotationCenterResult Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmRotationCenterResult();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmRotationCenterResult()
        {
            InitializeComponent();
        }


        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************

        //メンバ変数
        bool OK;        //入力結果

        float xf;       //自動テーブル移動座標：ハーフ／フル用ｘ座標（mm）
        float yf;       //自動テーブル移動座標：ハーフ／フル用ｙ座標（mm）
        float xo;       //自動テーブル移動座標：オフセット用ｘ座標（mm）
        float yo;       //自動テーブル移動座標：オフセット用ｙ座標（mm）

        int SliceNo;  //マルチスライスの番号

        //********************************************************************************
        //機    能  ：  回転中心校正画像の保存
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //              V2.0   00/02/14  (SI1)鈴山       関数の型宣言を追加
        //                                               マルチスライス対応
        //              V4.0   01/01/29  (SI1)鈴山       frmRotationCenterから移し、Private関数化
        //********************************************************************************
        private void ImgSave()
        {
            string[] aNAM = new string[6];
            string FileName = null;

            //保存先の一覧作成
            //Select Case GFlg_MltSlice
            switch (CTSettings.scansel.Data.multislice)
            {
                //v10.0変更 by 間々田 2005/02/14
                case 0:
                    aNAM[0] = "";
                    aNAM[1] = "";
                    aNAM[2] = ScanCorrect.RC01_CORRECT;
                    aNAM[3] = "";
                    aNAM[4] = "";
                    break;
                case 1:
                    aNAM[0] = "";
                    aNAM[1] = ScanCorrect.RC01_CORRECT;
                    aNAM[2] = ScanCorrect.RC02_CORRECT;
                    aNAM[3] = ScanCorrect.RC03_CORRECT;
                    aNAM[4] = "";
                    break;
                case 2:
                    aNAM[0] = ScanCorrect.RC01_CORRECT;
                    aNAM[1] = ScanCorrect.RC02_CORRECT;
                    aNAM[2] = ScanCorrect.RC03_CORRECT;
                    aNAM[3] = ScanCorrect.RC04_CORRECT;
                    aNAM[4] = ScanCorrect.RC05_CORRECT;
                    break;
                default:
                    break;
            }

            //保存先を決定
            FileName = aNAM[SliceNo];
            if (!string.IsNullOrEmpty(FileName))
            {
                //回転中心校正画像の保存
                ScanCorrect.ImageSave(ref ScanCorrect.RC_IMAGE_ORG[0], FileName, CTSettings.detectorParam.h_size, ScanCorrect.VIEW_N);
            }
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
            //フォームを消去
            //変更2015/1/17hata_非表示のときにちらつくため
            //Hide();
            modCT30K.FormHide(this);
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
            if (SliceNo == 2)
            {
                //変更2014/10/07hata_v19.51反映
                //if (ScanCorrect.mfanangle[2, 1] == 0 || ScanCorrect.mcenter_channel[2, 1] == 0 ||
                //    (CTSettings.scaninh.Data.data_mode[2] == 0 && ScanCorrect.nc == 0))
                //{
                //    //MsgBox "保存しようとするパラメータ値が不適切なため、処理を中止します。", vbCritical
                //    MessageBox.Show(CTResources.LoadResString(20061), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);   //ストリングテーブル化 'v17.60 by長野 2011/05/22
                //    return;
                //}
                //シフトスキャン以外の場合   'v18.00変更 byやまおか 2011/02/28 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                if (!modScanCorrect.Flg_RCShiftScan)
                {
                    if ((ScanCorrect.mfanangle[2, 1] == 0) | (ScanCorrect.mcenter_channel[2, 1] == 0) | (CTSettings.scaninh.Data.data_mode[2] == 0 & ScanCorrect.nc == 0))
                    {
                        goto ErrorHandler;
                    }
                
                //シフトスキャンの場合
                }
                else
                {
                    if ((ScanCorrect.mfanangle[2, 3] == 0) | (ScanCorrect.mcenter_channel[2, 3] == 0) | (CTSettings.scaninh.Data.data_mode[3] == 0 & ScanCorrect.nc == 0))
                    {
                        goto ErrorHandler;
                    }
                }
           
            }

            //押したボタンの種類を通知
            OK = true;

            //回転中心校正画像の保存
            ImgSave();

            //回転中心校正パラメータのコモンへの書き込み
            Set_RotationCenter_Parameter(SliceNo);

            //フォームを消去
            //変更2015/1/17hata_非表示のときにちらつくため
            //Hide();
            modCT30K.FormHide(this);

            //追加2014/10/07hata_v19.51反映
            return;

        ErrorHandler:
            //v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //v18.00追加 byやまおか 2011/02/28 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //MsgBox "保存しようとするパラメータ値が不適切なため、処理を中止します。", vbCritical
            //Interaction.MsgBox(CT30K.My.Resources.str20061, MsgBoxStyle.Critical);            //ストリングテーブル化 'v17.60 by長野 2011/05/22
            MessageBox.Show(CTResources.LoadResString(20061), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

            //フォームを消去
            //変更2015/1/20hata_非表示のときにちらつくため
            //Hide();
            modCT30K.FormHide(this);
            
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
        private void frmRotationCenterResult_Load(object sender, EventArgs e)
        {
            //キャプションのセット
            SetCaption();

            //CT30Kをアクティブにする
            frmCTMenu.Instance.Activate();  //v16.20追加 byやまおか 2010/04/21
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

            this.Text = StringTable.BuildResStr(StringTable.IDS_Result, StringTable.IDS_CorRot);    //回転中心校正結果
        }

        //*******************************************************************************
        //機　　能： モーダルダイアログ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： strInfo         [I/ ] String    フォームのキャプションに加える文字列
        //戻 り 値：                 [ /O] Boolean  「はい」がクリックされた場合  : True
        //                                          「いいえ」がクリックされた場合: False
        //補　　足：
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        //Public Function Dialog(Optional ByVal strInfo As String = "") As Boolean
        public bool Dialog(string strInfo = "", short pos = 2)  //v15.0変更 by 間々田 2009/04/02
        {
            bool functionReturnValue = false;

            int TRight = 296;

            //戻り値用変数の初期化
            OK = false;

            //複数スライス時のスキャン位置（0～4）
            SliceNo = pos;

            //フォームのキャプションに情報を加える
            if (!string.IsNullOrEmpty(strInfo))
            {
                this.Text = this.Text + " - " + strInfo;
            }

             //フォームにパラメータをセットする
            //変更2014/10/07hata_v19.51反映
            //v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //lblCenterPixel.Text = ScanCorrect.xlc[SliceNo].ToString("0.000000");
            //lblCenterPixel.Text = (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift ? ScanCorrect.xlc_sft[SliceNo] : ScanCorrect.xlc[SliceNo]).ToString("0.000000");
            //Rev25.00 Wスキャンを条件に追加 by長野 2016/07/07
            if ((CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift) || (CTSettings.scansel.Data.w_scan == 1))
            {
                lblCenterPixel.Text = ScanCorrect.xlc_sft[SliceNo].ToString("0.000000");
            }
            else
            {
                lblCenterPixel.Text = ScanCorrect.xlc[SliceNo].ToString("0.000000");
            }
            
            lblFanAngle.Text = (ScanCorrect.mfanangle[SliceNo, CTSettings.scansel.Data.scan_mode - 1] * 180 / ScanCorrect.Pai).ToString("0.00000000");
            lblMaxScanArea.Text = (ScanCorrect.MaxSArea[CTSettings.scansel.Data.scan_mode - 1]).ToString("0.000000");

            lblCenterPixel.Left = TRight - lblCenterPixel.Width;
            lblFanAngle.Left = TRight - lblFanAngle.Width;
            lblMaxScanArea.Left = TRight - lblMaxScanArea.Width;

            //モーダルでフォームを表示
            //変更2014/12/22hata_dNet_オーナーフォームを指定する
            //this.ShowDialog();
            this.ShowDialog(frmCTMenu.Instance);

            //戻り値のセット
            functionReturnValue = OK;

            //フォームをアンロード
            this.Close();
            return functionReturnValue;
        }

        //********************************************************************************
        //機    能  ：  回転中心校正パラメータをコモンに書き込む
        //              変数名           [I/O] 型        内容
        //引    数  ：  iSCN             [I/ ] Long      スキャン位置(0～4)
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V2.0   00/02/08  (SI1)鈴山       新規作成
        //          ：  V6.0   02/08/21  (SI4)間々田     自動テーブル移動座標の計算・コモン書込み処理追加
        //********************************************************************************
        private void Set_RotationCenter_Parameter(int iSCN)
        {
            float Fid = 0;      //GVal_Fid + GVal_FidOffset(GFlg_MultiTube)
            float FCD1 = 0;     //GVal_Fcd + GVal_FcdOffset(GFlg_MultiTube)
            float FCD2 = 0;
            float FCD3 = 0;
            float xr = 0;       //現在のｘ座標（mm）
            float dc1 = 0;      //（mm）
            float r = 0;        //（mm）
            double THo = 0;     //θo（rad）
            double THw = 0;     //θw（rad）

            float xL0 = 0;      //IIセンターの画素番号   'added by 山本 2002-9-23
            double Th_s = 0;    //ファン角開始角度       'added by 山本 2002-9-23
            double Th_e = 0;    //ファン角終了角度       'added by 山本 2002-9-23
            double Th_c = 0;    //回転中心角度           'added by 山本 2002-9-23
            double Alpha = 0;   //added by 山本 2002-9-23
            float gFCD2 = 0;    //added by 山本 2002-9-23

            //frmStatus.tmrCorStatus.Enabled = False      'added by 山本　2003-3-15 回転中心校正ステータスを書き換えるのでその間はCheck_SeqCor関数は停止させておく v11.5削除 by 間々田 2006/06/05

            //scansel（コモン）の更新
            UpdateScansel();

            //scancondpar（コモン）の更新
            //変更2014/10/07hata_v19.51反映
            //UpdateScancondpar(iSCN);
            //Rev25.00 Wスキャン追加 by長野 2016/07/07
            //UpdateScancondpar(iSCN, Convert.ToBoolean(CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift));
            UpdateScancondpar(iSCN, ((CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift) || CTSettings.scansel.Data.w_scan == 1)? true:false);

            //v18.00変更 byやまおか 2011/07/13 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05


            //メカの移動有無プロパティをリセットする   '上から移動 by 山本　2004-8-5
            modSeqComm.SeqBitWrite("RotXChangeReset", true);
            modSeqComm.SeqBitWrite("RotYChangeReset", true);
            modSeqComm.SeqBitWrite("RotIIChangeReset", true);

            // V6.0 append by 間々田 2002/07/10 START

            //ファン角を求める added by 山本 2002-9-23 START
            //xL0 = Conversion.Int(CTSettings.detectorParam.h_size / 2);
            xL0 = (float)Math.Floor(CTSettings.detectorParam.h_size / 2d);
            //2014/11/07hata キャストの修正
            //Th_s = Math.Atan(10 * (ScanCorrect.GVal_Xls[2] - xL0) / (ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube_R]) / ScanCorrect.A1[2]);
            //Th_e = Math.Atan(10 * (ScanCorrect.GVal_Xle[2] - xL0) / (ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube_R]) / ScanCorrect.A1[2]);
            //Th_c = Math.Atan(10 * (ScanCorrect.xlc[2] - xL0) / (ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube_R]) / ScanCorrect.A1[2]);
            Th_s = Math.Atan(10 * ((float)ScanCorrect.GVal_Xls[2] - xL0) / (ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube_R]) / ScanCorrect.A1[2]);
            Th_e = Math.Atan(10 * ((float)ScanCorrect.GVal_Xle[2] - xL0) / (ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube_R]) / ScanCorrect.A1[2]);
            Th_c = Math.Atan(10 * ((float)ScanCorrect.xlc[2] - xL0) / (ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube_R]) / ScanCorrect.A1[2]);
            Alpha = Th_e + Th_s;
            //ファン角を求める added by 山本 2002-9-23 END

            //回転選択が可の場合、自動テーブル移動は行なわれない 'v9.0 条件追加 by 間々田 2004/02/18
            if (CTSettings.scaninh.Data.rotate_select == 1)
            {
                //Xrの取得
                //xr = iSeqCor.stsXPosition / 10        '削除 巻渕 2003-03-03 ''旧バージョン6.3用
                //2014/11/07hata キャストの修正
                //xr = modSeqComm.MySeq.stsXPosition / 100;   //追加 巻渕 2003-03-03   小数点以下2桁まで対応
                xr = modSeqComm.MySeq.stsXPosition / 100F;   //追加 巻渕 2003-03-03   小数点以下2桁まで対応

                //FID,FCD1の取得
                Fid = ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube];
                FCD1 = ScanCorrect.GVal_Fcd + CTSettings.scancondpar.Data.fcd_offset[ScanCorrect.GFlg_MultiTube];

                //（xf,yf）、（xo,yo）の計算
                //dc1 = (ScanCorrect.xlc[2] - Conversion.Int(CTSettings.detectorParam.h_size / 2)) * 10 / ScanCorrect.A1[2];
                dc1 = (float)((ScanCorrect.xlc[2] - Math.Floor(CTSettings.detectorParam.h_size / 2d)) * 10 / ScanCorrect.A1[2]);
                //THw = Atn((H_SIZE / 2) * (10 / A1(2)) / CDbl(FID))
                THw = (Th_e - Th_s) * ScanCorrect.LimitFanAngle / 2;    //changed by 山本 2002-9-23
                r = ScanCorrect.MaxSArea[1] / 2;
                gFCD2 = (float)(r / Math.Sin(THw));                              //added by 山本 2002-9-23
                //FCD2 = r / Sin(THw)
                FCD2 = (float)(gFCD2 * Math.Cos(Alpha));                         //changed by 山本 2002-9-23
                THo = 0.9 * THw;
                FCD3 = (float)(r * Math.Cos(THo) / Math.Sin(THw + Math.Abs(THo)));

                //xf = xr - dc1 * (FCD1 / FID) + (FCD1 - FCD2) * Tan(CDbl(y_incli))
                //xf = xr + gFCD2 * Math.Sin(Alpha) - dc1 * (FCD1 / Fid) + (FCD1 - FCD2) * Math.Tan(Convert.ToDouble(modScancondpar.scancondpar.y_incli));
                xf = xr + gFCD2 * (float)Math.Sin(Alpha) - dc1 * (FCD1 / Fid) + (FCD1 - FCD2) * (float)Math.Tan(CTSettings.scancondpar.Data.y_incli);
                //changed by 山本 2002-9-23
                //If Abs(xf - xr) < 0.1 Then          'added by 山本 2002-9-23 X軸は0.1mm以下の移動をしないためのif文追加     ''旧バージョン6.3用
                //変更 巻渕 2003-03-03   小数点以下2桁まで対応
                if (Math.Abs(xf - xr) < 0.01)
                {
                    xf = xr;
                }
                //If Abs(xf - xr) < 0.1 Then          'added by 山本 2002-9-23 X軸が移動しない場合はY軸も移動しない   ''旧バージョン6.3用
                //変更 巻渕 2003-03-03   小数点以下2桁まで対応
                if (Math.Abs(xf - xr) < 0.01)
                {
                    yf = FCD1 - CTSettings.scancondpar.Data.fcd_offset[ScanCorrect.GFlg_MultiTube];
                }
                else
                {
                    yf = FCD2 - CTSettings.scancondpar.Data.fcd_offset[ScanCorrect.GFlg_MultiTube];
                }
                //xo = xf + FCD3 * Tan(THo)
                xo = xr - dc1 * (FCD1 / Fid) + (FCD1 - FCD3) * (float)Math.Tan(Convert.ToDouble(CTSettings.scancondpar.Data.y_incli)) + FCD3 * (float)Math.Tan(THo);   //changed by 山本 2002-9-23
                yo = FCD3 - CTSettings.scancondpar.Data.fcd_offset[ScanCorrect.GFlg_MultiTube];

                //v11.5削除ここから by 間々田 2006/06/05 UpdateMecainfに移動
                //'コモンへの書き込み：自動テーブル移動座標（xf,yf）、（xo,yo）
                //Call putcommon_float("mecainf", "auto_move_xf", xf)
                //Call putcommon_float("mecainf", "auto_move_yf", yf)
                //Call putcommon_float("mecainf", "auto_move_xo", xo)
                //Call putcommon_float("mecainf", "auto_move_yo", yo)
                //
                //'コモンへの書き込み：自動テーブル移動ステータスコモンに値２（移動未完了）を書き込む
                //putcommon_long "mecainf", "table_auto_move", 2
                //v11.5削除ここまで by 間々田 2006/06/05 UpdateMecainfに移動
            }

            // V6.0 append by 間々田 2002/07/10 END

            //オートセンタリングをなしにする                                     v7.0 added by 間々田 2003/09/26
            //Call putcommon_long("scansel", "auto_centering", 0)
            //MyScansel.IsAutoCentering = False                               'v10.0追加 by 間々田 2005/02/03
            frmScanControl.Instance.chkInhibit[3].CheckState = CheckState.Checked;  //v11.2変更 by 間々田 2006/01/13

            //added by 山本 2003-3-15 回転中心校正ステータスを書き換えるのでその間はCheck_SeqCor関数は停止させておいたので再開させる
            //frmStatus.tmrCorStatus.Enabled = True                  v11.5削除 by 間々田 2006/06/05

            //mecainf（コモン）の更新
            UpdateMecainf();
        }

        //*******************************************************************************
        //機　　能： scansel（コモン）の更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 回転中心校正関連のパラメータのみ更新
        //
        //履　　歴： v11.2  05/10/17  (SI3)間々田    新規作成
        //*******************************************************************************
        private void UpdateScansel()
        {
            //X線管：0(130kV),1(225kV)
            ScanCorrect.GFlg_MultiTube = ScanCorrect.GFlg_MultiTube_R;

            CTSettings.scansel.Data.multi_tube = ScanCorrect.GFlg_MultiTube;

            //FID
            CTSettings.scansel.Data.fid = ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[ScanCorrect.GFlg_MultiTube];

            //FCD
            CTSettings.scansel.Data.fcd = ScanCorrect.GVal_Fcd + CTSettings.scancondpar.Data.fcd_offset[modCT30K.GetFcdOffsetIndex()];

            //最大スキャンエリア
            CTSettings.scansel.Data.max_scan_area[0] = ScanCorrect.MaxSArea[0];
            CTSettings.scansel.Data.max_scan_area[1] = ScanCorrect.MaxSArea[1];
            CTSettings.scansel.Data.max_scan_area[2] = ScanCorrect.MaxSArea[2];
            //追加2014/10/07hata_v19.51反映
            CTSettings.scansel.Data.max_scan_area[3] = ScanCorrect.MaxSArea[3];     //v18.00追加 byやまおか 2011/02/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //スキャン条件のスキャンエリアは最大
            CTSettings.scansel.Data.mscan_area = ScanCorrect.MaxSArea[CTSettings.scansel.Data.scan_mode - 1];

            //コーンビームスキャンが可能な場合、コーンビーム用のパラメータをコモンに書き込む
            if (CTSettings.scaninh.Data.data_mode[2] == 0)
            {
                //最大スキャンエリア(ｺｰﾝﾋﾞｰﾑCT用)
                CTSettings.scansel.Data.cone_max_scan_area = ScanCorrect.GVal_MaxConeArea;

                //スキャンエリア(ｺｰﾝﾋﾞｰﾑCT用)
                CTSettings.scansel.Data.cone_scan_area = ScanCorrect.GVal_MaxConeArea;
            }

            //scansel書き込み
            //modScansel.PutScansel(ref CTSettings.scansel.Data);
            CTSettings.scansel.Write();
        }

        //*******************************************************************************
        //機　　能： scancondpar（コモン）の更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 回転中心校正関連のパラメータのみ更新
        //
        //履　　歴： v11.2  05/10/17  (SI3)間々田    新規作成
        //*******************************************************************************
        //private void UpdateScancondpar(int iSCN)
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        private void UpdateScancondpar(int iSCN, bool IsSftScan)
        {
            //最新Scancondpar取得
            //modScancondpar.CallGetScancondpar();
            CTSettings.scancondpar.Load(CTSettings.scaninh.Data.rotate_select);

            //回転中心画素
            //変更2014/10/07hata_v19.51反映
            //CTSettings.scancondpar.Data.xlc[iSCN] = (float)ScanCorrect.xlc[iSCN];
            CTSettings.scancondpar.Data.xlc[iSCN] = (float)(IsSftScan ? ScanCorrect.xlc_sft[iSCN] : ScanCorrect.xlc[iSCN]);     //v18.00変更 byやまおか 2011/07/13 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //ファン角：mfanangle[5][3]
            //CTSettings.scancondpar.Data.mfanangle[0, iSCN] = ScanCorrect.mfanangle[iSCN, 0] * 180 / ScanCorrect.Pai;
            //CTSettings.scancondpar.Data.mfanangle[1, iSCN] = ScanCorrect.mfanangle[iSCN, 1] * 180 / ScanCorrect.Pai;
            //CTSettings.scancondpar.Data.mfanangle[2, iSCN] = ScanCorrect.mfanangle[iSCN, 2] * 180 / ScanCorrect.Pai;
            //CTSettings.scancondpar.Data.mfanangle[0 + iSCN * 5] = (float)(ScanCorrect.mfanangle[iSCN, 0] * 180 / ScanCorrect.Pai);
            //CTSettings.scancondpar.Data.mfanangle[1 + iSCN * 5] = (float)(ScanCorrect.mfanangle[iSCN, 1] * 180 / ScanCorrect.Pai);
            //CTSettings.scancondpar.Data.mfanangle[2 + iSCN * 5] = (float)(ScanCorrect.mfanangle[iSCN, 2] * 180 / ScanCorrect.Pai);
            ////追加2014/10/07hata_v19.51反映
            //CTSettings.scancondpar.Data.mfanangle[3 + iSCN * 5] = (float)(ScanCorrect.mfanangle[iSCN, 3] * 180 / ScanCorrect.Pai);  //v18.00追加 byやまおか 2011/07/13 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev23.20 修正 by長野 2016/01/08--->
            CTSettings.scancondpar.Data.mfanangle[0 + iSCN * 4] = (float)(ScanCorrect.mfanangle[iSCN, 0] * 180 / ScanCorrect.Pai);
            CTSettings.scancondpar.Data.mfanangle[1 + iSCN * 4] = (float)(ScanCorrect.mfanangle[iSCN, 1] * 180 / ScanCorrect.Pai);
            CTSettings.scancondpar.Data.mfanangle[2 + iSCN * 4] = (float)(ScanCorrect.mfanangle[iSCN, 2] * 180 / ScanCorrect.Pai);
            //追加2014/10/07hata_v19.51反映
            CTSettings.scancondpar.Data.mfanangle[3 + iSCN * 4] = (float)(ScanCorrect.mfanangle[iSCN, 3] * 180 / ScanCorrect.Pai);  //v18.00追加 byやまおか 2011/07/13 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //<---

            //回転中心角度：Mcenter_angle[5][3]
            //CTSettings.scancondpar.Data.mcenter_angle[0, iSCN] = ScanCorrect.Theta_c[iSCN, 0];
            //CTSettings.scancondpar.Data.mcenter_angle[1, iSCN] = ScanCorrect.Theta_c[iSCN, 1];
            //CTSettings.scancondpar.Data.mcenter_angle[2, iSCN] = ScanCorrect.Theta_c[iSCN, 2];
            //CTSettings.scancondpar.Data.mcenter_angle[0 + iSCN * 5] = ScanCorrect.Theta_c[iSCN, 0];
            //CTSettings.scancondpar.Data.mcenter_angle[1 + iSCN * 5] = ScanCorrect.Theta_c[iSCN, 1];
            //CTSettings.scancondpar.Data.mcenter_angle[2 + iSCN * 5] = ScanCorrect.Theta_c[iSCN, 2];
            ////追加2014/10/07hata_v19.51反映
            //CTSettings.scancondpar.Data.mcenter_angle[3 + iSCN * 5] = ScanCorrect.Theta_c[iSCN, 3];     //v18.00追加 byやまおか 2011/07/13 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev23.20 修正 by長野 2016/01/08
            CTSettings.scancondpar.Data.mcenter_angle[0 + iSCN * 4] = ScanCorrect.Theta_c[iSCN, 0];
            CTSettings.scancondpar.Data.mcenter_angle[1 + iSCN * 4] = ScanCorrect.Theta_c[iSCN, 1];
            CTSettings.scancondpar.Data.mcenter_angle[2 + iSCN * 4] = ScanCorrect.Theta_c[iSCN, 2];
            //追加2014/10/07hata_v19.51反映
            CTSettings.scancondpar.Data.mcenter_angle[3 + iSCN * 4] = ScanCorrect.Theta_c[iSCN, 3];     //v18.00追加 byやまおか 2011/07/13 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //センターチャンネル：mcenter_channel[5][3]
            //CTSettings.scancondpar.Data.mcenter_channel[0, iSCN] = ScanCorrect.mcenter_channel[iSCN, 0];
            //CTSettings.scancondpar.Data.mcenter_channel[1, iSCN] = ScanCorrect.mcenter_channel[iSCN, 1];
            //CTSettings.scancondpar.Data.mcenter_channel[2, iSCN] = ScanCorrect.mcenter_channel[iSCN, 2];
            //CTSettings.scancondpar.Data.mcenter_channel[0 + iSCN * 5] = ScanCorrect.mcenter_channel[iSCN, 0];
            //CTSettings.scancondpar.Data.mcenter_channel[1 + iSCN * 5] = ScanCorrect.mcenter_channel[iSCN, 1];
            //CTSettings.scancondpar.Data.mcenter_channel[2 + iSCN * 5] = ScanCorrect.mcenter_channel[iSCN, 2];
            ////追加2014/10/07hata_v19.51反映
            //CTSettings.scancondpar.Data.mcenter_channel[3 + iSCN * 5] = ScanCorrect.mcenter_channel[iSCN, 3];   //v18.00追加 byやまおか 2011/07/13 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev23.20 修正 by長野 2015/01/08
            CTSettings.scancondpar.Data.mcenter_channel[0 + iSCN * 4] = ScanCorrect.mcenter_channel[iSCN, 0];
            CTSettings.scancondpar.Data.mcenter_channel[1 + iSCN * 4] = ScanCorrect.mcenter_channel[iSCN, 1];
            CTSettings.scancondpar.Data.mcenter_channel[2 + iSCN * 4] = ScanCorrect.mcenter_channel[iSCN, 2];
            //追加2014/10/07hata_v19.51反映
            CTSettings.scancondpar.Data.mcenter_channel[3 + iSCN * 4] = ScanCorrect.mcenter_channel[iSCN, 3];   //v18.00追加 byやまおか 2011/07/13 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //チャンネルピッチ角：mchannel_pitch[5][3]
            //CTSettings.scancondpar.Data.mchannel_pitch[0, iSCN] = ScanCorrect.mchannel_pitch[iSCN, 0];
            //CTSettings.scancondpar.Data.mchannel_pitch[1, iSCN] = ScanCorrect.mchannel_pitch[iSCN, 1];
            //CTSettings.scancondpar.Data.mchannel_pitch[2, iSCN] = ScanCorrect.mchannel_pitch[iSCN, 2];
            //CTSettings.scancondpar.Data.mchannel_pitch[0 + iSCN * 5] = ScanCorrect.mchannel_pitch[iSCN, 0];
            //CTSettings.scancondpar.Data.mchannel_pitch[1 + iSCN * 5] = ScanCorrect.mchannel_pitch[iSCN, 1];
            //CTSettings.scancondpar.Data.mchannel_pitch[2 + iSCN * 5] = ScanCorrect.mchannel_pitch[iSCN, 2];
            ////追加2014/10/07hata_v19.51反映
            //CTSettings.scancondpar.Data.mchannel_pitch[3 + iSCN * 5] = ScanCorrect.mchannel_pitch[iSCN, 3];     //v18.00追加 byやまおか 2011/07/13 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev23.20 修正 by長野 2016/01/08
            CTSettings.scancondpar.Data.mchannel_pitch[0 + iSCN * 4] = ScanCorrect.mchannel_pitch[iSCN, 0];
            CTSettings.scancondpar.Data.mchannel_pitch[1 + iSCN * 4] = ScanCorrect.mchannel_pitch[iSCN, 1];
            CTSettings.scancondpar.Data.mchannel_pitch[2 + iSCN * 4] = ScanCorrect.mchannel_pitch[iSCN, 2];
            //追加2014/10/07hata_v19.51反映
            CTSettings.scancondpar.Data.mchannel_pitch[3 + iSCN * 4] = ScanCorrect.mchannel_pitch[iSCN, 3];     //v18.00追加 byやまおか 2011/07/13 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //コーンビームスキャンが可能な場合、コーンビーム用のパラメータをコモンに書き込む
            if (CTSettings.scaninh.Data.data_mode[2] == 0)
            {
                //有効ファン角(radian)
                //変更2014/10/07hata_v19.51反映
                //CTSettings.scancondpar.Data.delta_theta = (float)ScanCorrect.delta_theta;
                CTSettings.scancondpar.Data.delta_theta = (float)(IsSftScan ? ScanCorrect.delta_theta_sft : ScanCorrect.delta_theta);   //v18.00変更 byやまおか 2011/07/13 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05


                //画像中心対応チャンネル
                //変更2014/10/07hata_v19.51反映
                //CTSettings.scancondpar.Data.n0 = (float)ScanCorrect.n0;
                CTSettings.scancondpar.Data.n0 = (float)(IsSftScan ? ScanCorrect.n0_sft : ScanCorrect.n0);  //v18.00変更 byやまおか 2011/07/13 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

                //nの有効開始チャンネル
                CTSettings.scancondpar.Data.n1[0] = ScanCorrect.n1[0];           //非ｵﾌｾｯﾄ用
                CTSettings.scancondpar.Data.n1[1] = ScanCorrect.n1[1];           //ｵﾌｾｯﾄｽｷｬﾝ用
                //追加2014/10/07hata_v19.51反映
                CTSettings.scancondpar.Data.n1[2] = ScanCorrect.n1[2];           //ｼﾌﾄｽｷｬﾝ用  'v18.00追加 byやまおか 2011/07/13 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

                //nの有効終了チャンネル
                CTSettings.scancondpar.Data.n2[0] = ScanCorrect.n2[0];           //非ｵﾌｾｯﾄ用
                CTSettings.scancondpar.Data.n2[1] = ScanCorrect.n2[1];           //ｵﾌｾｯﾄｽｷｬﾝ用
                //追加2014/10/07hata_v19.51反映
                CTSettings.scancondpar.Data.n2[2] = ScanCorrect.n2[2];                //ｼﾌﾄｽｷｬﾝ用  'v18.00追加 byやまおか 2011/07/13 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
 
                //有効ファン角(radian)
                CTSettings.scancondpar.Data.theta0[0] = (float)ScanCorrect.theta0[0];   //非ｵﾌｾｯﾄ用
                CTSettings.scancondpar.Data.theta0[1] = (float)ScanCorrect.theta0[1];   //ｵﾌｾｯﾄｽｷｬﾝ用
                //追加2014/10/07hata_v19.51反映
                CTSettings.scancondpar.Data.theta0[2] = (float)ScanCorrect.theta0[2];   //ｼﾌﾄｽｷｬﾝ用  'v18.00追加 byやまおか 2011/07/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

                //有効データ包含半角1(radian)
                CTSettings.scancondpar.Data.theta01[0] = (float)ScanCorrect.theta01[0]; //非ｵﾌｾｯﾄ用
                CTSettings.scancondpar.Data.theta01[1] = (float)ScanCorrect.theta01[1]; //ｵﾌｾｯﾄｽｷｬﾝ用
                //追加2014/10/07hata_v19.51反映
                CTSettings.scancondpar.Data.theta01[2] = (float)ScanCorrect.theta01[2]; //ｼﾌﾄｽｷｬﾝ用  'v18.00追加 byやまおか 2011/07/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

                //有効データ包含半角2(radian)
                CTSettings.scancondpar.Data.theta02[0] = (float)ScanCorrect.theta02[0]; //非ｵﾌｾｯﾄ用
                CTSettings.scancondpar.Data.theta02[1] = (float)ScanCorrect.theta02[1]; //ｵﾌｾｯﾄｽｷｬﾝ用
                //追加2014/10/07hata_v19.51反映
                CTSettings.scancondpar.Data.theta02[2] = (float)ScanCorrect.theta02[2]; //ｼﾌﾄｽｷｬﾝ用  'v18.00追加 byやまおか 2011/07/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

                //オフセット角(radian)
                //変更2014/10/07hata_v19.51反映
                //CTSettings.scancondpar.Data.thetaoff = ScanCorrect.thetaoff;
                CTSettings.scancondpar.Data.thetaoff = (IsSftScan ? ScanCorrect.thetaoff_sft : ScanCorrect.thetaoff);   //v18.00変更 byやまおか 2011/07/24 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

                //オフセット識別値：1(右ｵﾌｾｯﾄ),-1(左ｵﾌｾｯﾄ)
                CTSettings.scancondpar.Data.ioff = ScanCorrect.ioff;

                //回転中心チャンネル
                //変更2014/10/07hata_v19.51反映
                //CTSettings.scancondpar.Data.nc = (float)ScanCorrect.nc;
                CTSettings.scancondpar.Data.nc = (float)(IsSftScan ? ScanCorrect.nc_sft : ScanCorrect.nc);      //v18.00変更 byやまおか 2011/07/13 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            }

            //Scancondpar書き込み
            //modScancondpar.CallPutScancondpar();
            CTSettings.scancondpar.Write();

        }

        //*******************************************************************************
        //機　　能： mecainf（コモン）の更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 回転中心校正関連のパラメータのみ更新
        //
        //履　　歴： v11.2  05/10/17  (SI3)間々田    新規作成
        //*******************************************************************************
        private void UpdateMecainf()
        {
            //mecainfType theMecainf = default(mecainfType);
            MecaInf theMecainf = new MecaInf();
            theMecainf.Data.Initialize();

            //mecainf（コモン）取得
            //modMecainf.GetMecainf(ref theMecainf);
            theMecainf.Load();

            //校正ステータスを準備完了にする
            theMecainf.Data.normal_rc_cor = 1;   //ノーマルスキャン
            theMecainf.Data.cone_rc_cor = 1;     //コーン

            //管電圧(KV)
            //v16.30条件追加 byやまおか 2010/05/21
            if ((CTSettings.scaninh.Data.xray_remote == 0))
            {
                //.rc_kv = scansel.scan_kv
                theMecainf.Data.rc_kv = (float)frmXrayControl.Instance.ntbSetVolt.Value;    //v15.0変更 by 間々田 2009/04/10
            }

            //昇降位置(mm)
            //theMecainf.Data.rc_udab_pos = theMecainf.Data.udab_pos;
            //Rev23.10 計測CT対応 by長野 2015/10/16
            if (CTSettings.scaninh.Data.cm_mode == 0)
            {
                theMecainf.Data.rc_udab_pos = theMecainf.Data.ud_linear_pos;
            }
            else
            {
                theMecainf.Data.rc_udab_pos = theMecainf.Data.udab_pos;
            }

            //I.I.視野
            theMecainf.Data.rc_iifield = modSeqComm.GetIINo();

            //Ｘ線管
            theMecainf.Data.rc_mt = CTSettings.scansel.Data.multi_tube;

            //回転選択が可能な場合
            if (CTSettings.scaninh.Data.rotate_select == 0)
            {
                theMecainf.Data.rc_rs = ScanCorrect.GFlg_MultiTube_R;    //GFlg_MultiTube_R に回転中心校正画面で選択した回転方式の値が入っています
            }

            //ビニングモード
            theMecainf.Data.rc_bin = CTSettings.scansel.Data.binning;     //0:１×１，1:２×２，2:４×４

            //回転選択が可の場合、自動テーブル移動は行なわれない 'v9.0 条件追加 by 間々田 2004/02/18
            if (CTSettings.scaninh.Data.rotate_select == 1)
            {
                //自動テーブル移動座標（xf,yf）（xo,yo）
                theMecainf.Data.auto_move_xf = xf;
                theMecainf.Data.auto_move_yf = yf;
                theMecainf.Data.auto_move_xo = xo;
                theMecainf.Data.auto_move_yo = yo;

                //自動テーブル移動ステータスコモンに値２（移動未完了）を書き込む
                theMecainf.Data.table_auto_move = 2;
            }

            //追加2014/10/07hata_v19.51反映
            //焦点           'v18.00追加 byやまおか 2011/06/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            theMecainf.Data.rc_focus = CTSettings.mecainf.Data.xfocus;

            //mecainf（コモン）更新
            //modMecainf.PutMecainf(ref theMecainf);
            theMecainf.Write();

            //回転中心校正ステータスの更新
            frmCorrectionStatus.Instance.UpdateRCStatus();
        }
        
    }
        
}
