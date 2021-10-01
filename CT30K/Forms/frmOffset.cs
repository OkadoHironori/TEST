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
    ///* プログラム名： frmOffset.frm                                                */
    ///* 処理概要　　： オフセット校正                                               */
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
    ///* V4.0        01/01/30    (ITC)    鈴山　修   ﾓｰﾀﾞﾙﾌｫｰﾑ→MDI子ﾌｫｰﾑに変更      */
    ///* V9.7        04/11/01    (SI4)間々田         収集停止対応                    */
    ///*                                                                             */
    ///* --------------------------------------------------------------------------- */
    ///* ご注意：                                                                    */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。    */
    ///*                                                                             */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2004                  */
    ///* *************************************************************************** */
    public partial class frmOffset : Form
    {
        #region インスタンスを返すプロパティ

        // frmOffsetのインスタンス
        private static frmOffset _Instance = null;

        /// <summary>
        /// frmOffsetのインスタンスを返す
        /// </summary>
        public static frmOffset Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmOffset();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmOffset()
        {
            InitializeComponent();
        }

        bool myBusy;

        //*******************************************************************************
        //機　　能： IsBusyプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v10.2 2005/08/22 (SI3)間々田    新規作成
        //*******************************************************************************
        public bool IsBusy
        {
            get { return myBusy; }
            set
            {
                //設定値を保存
                myBusy = value;

                //「ＯＫ」ボタンと「停止」ボタンの切り替え
                cmdOK.Text = (myBusy ? CTResources.LoadResString(StringTable.IDS_btnStop) : CTResources.LoadResString(StringTable.IDS_btnOK));

                //終了ボタンの使用可・不可の設定
                cmdEnd.Enabled = !myBusy;

                //積算枚数欄の使用可・不可の設定
                cwneSum.Enabled = !myBusy;

                //マウスポインタを元に戻す
                this.Cursor = (myBusy ? Cursors.AppStarting : Cursors.Default);

                //CTBusyフラグを更新
                if (myBusy)
                {
                    modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTMechaBusy;
                }
                else
                {
                    modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTMechaBusy);
                }
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
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdEnd_Click(object eventSender, EventArgs e)
        {
            //フォームをアンロードする
            this.Close();

            //Rev20.01 dispose 追加 by長野 2015/05/19
            this.Dispose();
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
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdOK_Click(object sender, EventArgs e)
        {
            int ret = 0;
            
            //RAMディスクが構築されているかどうか  'v17.40変更 byやまおか 2010/10/26
            //If UseRamDisk And (Not RamDiskIsReady) Then Exit Sub
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //    If UseRamDisk Then      'v17.42修正 byやまおか 2010/11/04
            //        If (Not RamDiskIsReady) Then Exit Sub
            //    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //メカが動ける(パネルがOFF)かチェック     'v18.00追加 byやまおか 2011/07/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if ((!modMechaControl.IsOkMechaMove()))
                return;

            //ビジーならば校正実行中にクリックされたとみなし、dllに対して停止要求を行なう
            if (IsBusy)
            {
                //#If Not NoCamera Then   'v15.10条件追加 byやまおか 2009/10/29
                //        ''キャプチャストップ     'v15.0追加 by 間々田 2009/02/09
                //        ''MilCaptureStop         'v17.00削除 byやまおか 2010/01/20
                //        'Select Case DetType
                //        '    Case DetTypeII, DetTypeHama
                //        '        MilCaptureStop
                //        '    Case DetTypePke
                //        '        PkeCaptureStop (hPke)     'changed by 山本 2009-09-16
                //        'End Select
                //#End If

                //実行中の処理に対して停止要求をする 'v17.50上記の処理を関数化 by 間々田 2011/02/17
                modCT30K.CallUserStopSet();

                return;
            }

            //停止要求フラグをクリアする             'v17.50追加 by 間々田 2011/02/17
            modCT30K.CallUserStopClear();

            //I.I.（またはFPD）電源のチェック    追加 by 間々田 2004/12/28
            if (!modSeqComm.PowerSupplyOK())
            {
                return;
            }

            //ビジーフラグセット
            IsBusy = true;

            //オフセット校正画像を配列に読み込む
            //2014/11/07hata キャストの修正
            //if (modScanCorrectNew.GetImageForOffsetCorrect(stsOffset, pgbOffset, (int)cwneSum.Value))
            if (modScanCorrectNew.GetImageForOffsetCorrect(stsOffset, pgbOffset, Convert.ToInt32(cwneSum.Value)))
            {

                //フォームを非表示にする
                //変更2015/1/17hata_非表示のときにちらつくため
                //Hide();
                modCT30K.FormHide(this);

                //オフセット校正結果フォームを表示する
                frmOffsetResult.Instance.Dialog();

                //フォームをアンロードする
                this.Close();

                //Rev20.01 dispose 追加 by長野 2015/05/19
                this.Dispose();
            }
            else
            {
                //Rev26.00 失敗の場合は[ガイド]タブのスキャンエリア・条件の設定完了フラグを落とす add by chouno 2017/01/16
                frmScanControl.Instance.setScanAreaAndCmpFlg(false);

                //「データ収集異常終了」と表示
                stsOffset.Status = StringTable.GC_STS_CAPT_NG;

                //PkeFPDの場合   'v17.02追加 byやまおか 2010/07/15
                if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))
                {

#if (!NoCamera) //'v19.50 v19.41とv18.02の統合 by長野 2013/11/05      //追加2014/10/07hata_v19.51反映

                    //元のオフセット校正画像をプリロードする
                    ret = ScanCorrect.PkeSetOffsetData((int)Pulsar.hPke, 0, ref ScanCorrect.OFFSET_IMAGE[0], 0);
                    //If ret = 1 Then MsgBox "オフセット校正データをセットできませんでした。", vbCritical
                    if (ret == 1)
                    {
                        //v17.60 ストリングテーブル化 by長野 2011/05/25
                        MessageBox.Show(CTResources.LoadResString(20004), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

#endif
                }

                //ビジーフラグリセット
                IsBusy = false;
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
        private void frmOffset_Load(object sender, EventArgs e)
        {
            //v17.10削除 byやまおか 2010/07/28
            //'画像取込を停止させる 'v17.00追加　山本 2010-01-25 フリーズ対策
            //frmTransImage.CaptureOn = False
            //DoEvents

            //実行時はフラグをセット
            modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTScanCorrect;

            //キャプションのセット
            SetCaption();

            //v17.60 英語用レイアウト調整 by長野 201/05/25
            if (modCT30K.IsEnglish == true)
            {
                EnglishAdjustLayout();
            }

            //積算枚数をセットする
            //変更2014/11/28hata_v19.51_dnet
            //数字の初期値は10単位にする
            //cwneSum.Value = ScanCorrect.IntegNumAtOff;
            cwneSum.Value = ScanCorrect.RoundControlVale(ScanCorrect.IntegNumAtOff, cwneSum.Maximum, cwneSum.Minimum, 10F);

            //停止中
            stsOffset.Status = StringTable.GC_STS_STOP;

            //ビジーオフ
            IsBusy = false;
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
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void frmOffset_FormClosed(object sender, FormClosedEventArgs e)
        {
            //ビジーオフ
            IsBusy = false;

            //終了時はフラグをリセット
            modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTScanCorrect);

            //Rev20.01 追加 by長野 2015/06/03
            frmXrayControl.Instance.UpdateWarmUp();
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

            //Ｘ線外部制御でない場合：「Ｘ線をオフしてください。」を付加
            if (CTSettings.scaninh.Data.xray_remote != 0)
            {
                lblMessage.Text = StringTable.BuildResStr(StringTable.IDS_TurnOff, StringTable.IDS_Xray) + "\r" + lblMessage.Text;
            }

            //v17.60 フォントがバラつき不自然なため削除 by 長野 2011/06/11
            //英語環境の場合、ラベルコントロールに使用するフォントをArialにする
            //If IsEnglish Then SetLabelFont Me
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
        //履　　歴： V17.60  11/05/25  (検S１)長野        新規作成
        //*******************************************************************************
        private void EnglishAdjustLayout()
        {
            short margin = 0;

            //2014/11/07hata キャストの修正
            //margin = 900 / 15;
            margin = 60;
            //Rev20.01 変更 by長野 2015/05/19
            //lblIntegNum.Left = lblIntegNum.Left + margin;
            //lblColon1.Left = lblColon1.Left + margin;
            lblIntegNum.Left = lblIntegNum.Left + margin - 25;
            lblColon1.Left = lblColon1.Left + margin - 25;

            cwneSum.Left = cwneSum.Left + margin;
            Label7.Left = Label7.Left + margin;
        }
    }
}
