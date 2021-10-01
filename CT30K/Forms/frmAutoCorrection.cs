using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using CTAPI;
using CT30K.Common;
using TransImage;

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： frmAutoCorrection.frm                                       */
    ///* 処理概要　　： 自動校正画面                                                */
    ///* 注意事項　　： なし                                                        */
    ///* -------------------------------------------------------------------------- */
    ///* 適用計算機　： DOS/V PC                                                    */
    ///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
    ///* コンパイラ　： VB 6.0                                                      */
    ///* -------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
    ///*                                                                            */
    ///* V4.0        01/01/29    (ITC)    鈴山　修   新規作成                       */
    ///* V4.0        01/01/30    (ITC)    鈴山　修   ﾓｰﾀﾞﾙﾌｫｰﾑ→MDI子ﾌｫｰﾑに変更     */
    ///* v11.2     2006/01/17    (ITC)    間々田     全体的に見直し                 */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
    public partial class frmAutoCorrection : Form
    {
        #region インスタンスを返すプロパティ

        // frmAutoCorrectionのインスタンス
        private static frmAutoCorrection _Instance = null;

        //Rev20.00 全自動校正開始ボタンをクリックする前の各ボックスのenableバックアップ用
        private bool bakcwneViewGainEnabled = false;
        private bool bakcwneSumGainEnabled = false;
        private bool bakcwneGMAEnabled = false;
        private bool bakchkHaFuOfScanEnabled = false;
        private bool bakchkShiftScanEnabled = false;
        private bool bakchkGainTableRotEnabled = false;
        private bool bakchkSPCorrectEnabled = false;
        private bool bakcwneSumSpEnabled = false;
        private bool bakcwneSumVerEnabled = false;
        private bool bakcwneVMAEnabled = false;
        private bool bakchkShadingEnabled = false;
        private bool bakcwneSumOffEnabled = false;
        private bool bakchkYAxisMoveEnabled = false; //Rev20.00 追加 by長野 2015/02/16
        private bool bakchkXAxisMoveEnabled = false; //Rev23.40 追加 by長野 2016/06/19

        /// <summary>
        /// frmAutoCorrectionのインスタンスを返す
        /// </summary>
        public static frmAutoCorrection Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmAutoCorrection();
                }

                return _Instance;
            }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmAutoCorrection()
        {
            InitializeComponent();
        }

        #endregion

        //IsBusyプロパティ用変数
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
        private bool IsBusy
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

                //入力可能なコントロールのEnabledプロパティを制御
                Control theControl = null;
                foreach (Control theControl_loopVariable in this.Controls)
                {
                    theControl = theControl_loopVariable;
                    switch (theControl.GetType().Name)
                    {
                        case "CWNumEdit":
                        case "CheckBox":
                            theControl.Enabled = !myBusy;
                            break;
                        default:
                            break;
                    }
                }

                //マウスポインタを制御
                this.Cursor = (myBusy ? Cursors.AppStarting : Cursors.Default);

                //メッセージ表示：
                if (myBusy)
                {
                    //処理中
                    lblMessage.Text = StringTable.GC_STS_CPU_BUSY;
                }
                else
                {
                    //試料テーブルに何も載せないでください。
                    //準備ができたらＯＫをクリックしてください。
                    lblMessage.Text = CTResources.LoadResString(StringTable.IDS_DontPutOnTable) + "\r\n" + CTResources.LoadResString(StringTable.IDS_ClickOKIfReady);
                }
                lblMessage.Refresh();

                //ビジーでなければ更新処理
                if (!myBusy)
                {
                    //Update();
                    MyUpdate();
                }

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

        //追加2014/10/07hata_v19.51反映
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここから
        //*************************************************************************************************
        //機　　能： ハーフフルオフセットスキャン収集チェックボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V18.00  2011/02/04  やまおか    新規作成
        //*************************************************************************************************
        private void chkHaFuOfScan_CheckStateChanged(System.Object eventSender, System.EventArgs eventArgs)
        {

            //常にチェックＯＮにする
            chkHaFuOfScan.CheckState = System.Windows.Forms.CheckState.Checked;

        }

        //追加2014/10/07hata_v19.51反映
        //*************************************************************************************************
        //機　　能： シフトスキャン収集チェックボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V18.00  2011/02/11  やまおか    新規作成
        //*************************************************************************************************
        private void chkShiftScan_CheckStateChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            //ゲイン校正ステータス（簡易表示）の更新
            var _with1 = frmCorrectionStatus.Instance;
            if ((chkShiftScan.CheckState == System.Windows.Forms.CheckState.Checked))
            {
                //シフトスキャンならゲイン校正ステータス(シフト用)を考慮して更新
                _with1.UpdateStatus(_with1.lblItemGain, ref frmScanControl.Instance.lblStatus[0], "", _with1.lblItemGainShift);
            }
            else
            {
                //シフトスキャン以外は通常のゲイン校正ステータスだけで更新
                _with1.UpdateStatus(_with1.lblItemGain, ref frmScanControl.Instance.lblStatus[0]);
            }

            //ゲイン校正ステータス（自動校正画面）の更新
            MyUpdate();
        }
        //v19.50 v19.41とv18.02の統合 by長野 2013/11/05 ここまで


        //*************************************************************************************************
        //機　　能： テーブル回転チェックボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v7.0  2003/10/20 (SI3)間々田    新規作成
        //*************************************************************************************************
        private void chkGainTableRot_CheckStateChanged(object sender, EventArgs e)
        {
            //テーブル回転する・しないによって最低ビュー数を変更する

            //最低ビュー数を求める（テーブル回転する・しないによって変更する）
            cwneViewGain.Minimum = modSeqComm.GetViewMin(chkGainTableRot.CheckState == CheckState.Checked);

            //ビュー数の範囲を表示
            lblViewGainMinMax.Text = StringTable.GetResString(StringTable.IDS_Range, cwneViewGain.Minimum.ToString(), cwneViewGain.Maximum.ToString());
        }

        //*************************************************************************************************
        //機　　能： スキャン位置校正チェックボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v7.0  2003/10/20 (SI3)間々田    新規作成
        //*************************************************************************************************
        private void chkSPCorrect_CheckStateChanged(object sender, EventArgs e)
        {
            //校正ステータス画面のスキャン位置校正チェックボックスをクリックした時と同じ処理
            frmScanControl.Instance.chkInhibit[1].CheckState = chkSPCorrect.CheckState;

            //更新
            MyUpdate();
        }

        //*************************************************************************************************
        //機　　能： 終了ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void cmdEnd_Click(object sender, EventArgs e)
        {
            //追加2014/10/07hata_v19.51反映
            //ゲイン校正ステータス（簡易表示）の更新 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
			var _with3 = frmCorrectionStatus.Instance;
			//if ((CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift) ) {
			//Rev25.00 Wスキャン用の条件追加 by長野 2016/07/05
            if ((CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift) ||
                (CTSettings.scansel.Data.w_scan == 1)) //シフトスキャン、または、Wスキャンが選ばれていたら。
            {
                //シフトスキャンならゲイン校正ステータス(シフト用)を考慮して更新
				_with3.UpdateStatus(_with3.lblItemGain,ref frmScanControl.Instance.lblStatus[0], "", _with3.lblItemGainShift);
			} else {
				//シフトスキャン以外は通常のゲイン校正ステータスだけで更新
				_with3.UpdateStatus(_with3.lblItemGain,ref frmScanControl.Instance.lblStatus[0]);
			}

            //自動校正フォームをアンロードする
            this.Close();
        }

        //*************************************************************************************************
        //機　　能： ＯＫボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void cmdOK_Click(object sender, EventArgs e)
        {
            //RAMディスクが構築されているかどうか  'v17.40変更 byやまおか 2010/10/26
            //If UseRamDisk And (Not RamDiskIsReady) Then Exit Sub
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //If UseRamDisk Then      'v17.42修正 byやまおか 2010/11/04
            //    If (Not RamDiskIsReady) Then Exit Sub
            //End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //追加2014/10/07hata_v19.51反映
            //メカが動ける(パネルがOFF)かチェック 'v18.00追加 byやまおか 2011/07/02
            if ((!modMechaControl.IsOkMechaMove()))
                return;
            //v19.50 v19.41とv18.02の統合 by長野 2013/11/05

            //ビジー？→校正実行中にクリックされたとみなし、dllに対して停止要求を行なう
            if (IsBusy)
            {
                //        '停止要求フラグセット
                //        UserStopSet
                //
                //        '連続回転コーンビーム＋高速再構成の時は、RAMディスクのscanstopを使う v17.40 追加 by 長野
                //        If smooth_rot_cone_flg = True Then
                //
                //            UserStopSet_rmdsk
                //
                //        End If
                //
                //#If Not NoCamera Then   'v15.10条件追加 byやまおか 2009/10/29
                //        'キャプチャストップ     'v15.0追加 by 間々田 2009/02/09
                //        'MilCaptureStop     'v17.00削除 byやまおか 2010/02/02
                //        Select Case DetType 'v17.00追加(ここから) byやまおか 2010/02/02
                //            Case DetTypeII, DetTypeHama
                //                MilCaptureStop
                //            Case DetTypePke
                //                PkeCaptureStop (hPke)     'changed by 山本 2009-09-16
                //        End Select          'v17.00追加(ここまで) byやまおか 2010/01/19
                //#End If

                //実行中の処理に対して停止要求をする     'v17.50上記の処理を関数化 by 間々田 2011/02/17
                modCT30K.CallUserStopSet();

                return;
            }

            //すべてが校正済みの場合
            if ((stsCorrect0.Status != StringTable.GC_STS_STANDBY_NG) && (stsCorrect1.Status != StringTable.GC_STS_STANDBY_NG) && 
                (stsCorrect2.Status != StringTable.GC_STS_STANDBY_NG) && (stsCorrect3.Status != StringTable.GC_STS_STANDBY_NG))
            {
                //メッセージの表示：実行しようとする校正がすべて完了しているので、処理を中止します。
                //MsgBox LoadResString(IDS_CorReadyAlready), vbCritical
                MessageBox.Show(CTResources.LoadResString(StringTable.IDS_CorReadyAlready), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);//警告→情報 'v17.60変更 byやまおか 2011/06/07
                return;
            }

            //I.I.（またはFPD）電源のチェック
            if (!modSeqComm.PowerSupplyOK())
            {
                return;
            }

            //スキャン位置校正もしくはテーブル回転ありのゲイン校正の予定がある場合、FCDのチェック
            if ((stsCorrect1.Status == StringTable.GC_STS_STANDBY_NG) || 
                (stsCorrect0.Status == StringTable.GC_STS_STANDBY_NG && (chkGainTableRot.CheckState == CheckState.Checked)))
            {
                if (!modSeqComm.CheckFCD(ScanCorrect.GVal_Fcd))
                {
                    return;
                }
            }

            //Rev20.00 Rev20.00 ゲイン校正、かつ、Y軸移動ありの場合は確認のメッセージを出す。 by長野 2015/02/16
            if ((stsCorrect0.Status == StringTable.GC_STS_STANDBY_NG) && chkYAxisMove.CheckState == CheckState.Checked)
            {
                //警告メッセージの表示（コモンによってメッセージを切り替える）：
                //   データ収集時に試料テーブルがY軸方向に移動します。
                //   移動してもサンプルまたは試料テーブルが、検査室やコリメータと衝突しないことを確認してください。
                //   よろしければＯＫをクリックして下さい。
                //if (Interaction.MsgBox(CTResources.LoadResString(CTSettings.t20kinf.Data.ud_type == 1 ? 9471 : 9405)) + Constants.vbCrLf + CTResources.LoadResString(StringTable.IDS_ClickOK), MsgBoxStyle.OkCancel + MsgBoxStyle.Exclamation) == MsgBoxResult.Cancel)
                string msg = (CTResources.LoadResString(9406)) + "\r\n" + CTResources.LoadResString(StringTable.IDS_ClickOK);
                if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                {
                    return;
                }
            }

            //Rev21.00 昇降タイプで分ける by長野 2015/03/18 
            if (CTSettings.t20kinf.Data.ud_type == 0)
            {
                //スキャン位置校正の予定がある場合
                //If stsCorrect(1).Status = GC_STS_STANDBY_NG Then
                //変更 by 間々田 2009/08/17 ゲイン校正・幾何歪校正時にテーブルを下げる際にも警告を出す
                if ((stsCorrect0.Status == StringTable.GC_STS_STANDBY_NG) ||
                    (stsCorrect1.Status == StringTable.GC_STS_STANDBY_NG) ||
                    (stsCorrect2.Status == StringTable.GC_STS_STANDBY_NG))
                {
                    //変更2014/10/07hata_v19.51反映
                    //産業用CTモードの場合   'v18.00条件追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    if ((CTSettings.scaninh.Data.avmode == 0))
                    {
                        //   試料テーブルが動作しますので、試料の固定を確認してください。
                        //   よろしければＯＫをクリックして下さい。
                        if (MessageBox.Show(CTResources.LoadResString(9479) + "\r\n" + "\r\n" + CTResources.LoadResString(StringTable.IDS_ClickOK), Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                            return;

                        //マイクロCTの場合
                    }
                    else
                    {

                        //警告メッセージの表示（コモンによってメッセージを切り替える）：
                        //   スキャン位置校正のデータ収集時に試料テーブルがX線管の近くで上昇します。
                        //   コリメータがX線管の近くにあると試料テーブルと衝突する恐れがあるので待避しておいてください。
                        //   よろしければＯＫをクリックして下さい。
                        //
                        //   ↓メッセージ変更 by 間々田 2009/08/17
                        //
                        //警告メッセージの表示（コモンによってメッセージを切り替える）：
                        //   データ収集時に試料テーブルがX線管の近くで昇降します。
                        //   コリメータがX線管の近くにあると試料テーブルと衝突する恐れがあるので待避しておいてください。
                        //   よろしければＯＫをクリックして下さい。
                        //if (Interaction.MsgBox(CTResources.LoadResString(CTSettings.t20kinf.Data.ud_type == 1 ? 9471 : 9405)) + Constants.vbCrLf + CTResources.LoadResString(StringTable.IDS_ClickOK), MsgBoxStyle.OkCancel + MsgBoxStyle.Exclamation) == MsgBoxResult.Cancel)
                        string msg = (CTSettings.t20kinf.Data.ud_type == 1 ? CTResources.LoadResString(9471) : CTResources.LoadResString(9405)) + "\r\n" + CTResources.LoadResString(StringTable.IDS_ClickOK);
                        if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                }
            }
            else
            {
                //ゲイン・幾何歪校正がある場合
                //If stsCorrect(1).Status = GC_STS_STANDBY_NG Then
                //変更 by 間々田 2009/08/17 ゲイン校正・幾何歪校正時にテーブルを下げる際にも警告を出す
                if ((stsCorrect0.Status == StringTable.GC_STS_STANDBY_NG) ||
                    (stsCorrect2.Status == StringTable.GC_STS_STANDBY_NG))
                {
                    //変更2014/10/07hata_v19.51反映
                    //産業用CTモードの場合   'v18.00条件追加 byやまおか 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    if ((CTSettings.scaninh.Data.avmode == 0))
                    {
                        //   試料テーブルが動作しますので、試料の固定を確認してください。
                        //   よろしければＯＫをクリックして下さい。
                        if (MessageBox.Show(CTResources.LoadResString(9479) + "\r\n" + "\r\n" + CTResources.LoadResString(StringTable.IDS_ClickOK), Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                            return;

                        //マイクロCTの場合
                    }
                    else
                    {

                        //警告メッセージの表示（コモンによってメッセージを切り替える）：
                        //   スキャン位置校正のデータ収集時に試料テーブルがX線管の近くで上昇します。
                        //   コリメータがX線管の近くにあると試料テーブルと衝突する恐れがあるので待避しておいてください。
                        //   よろしければＯＫをクリックして下さい。
                        //
                        //   ↓メッセージ変更 by 間々田 2009/08/17
                        //
                        //警告メッセージの表示（コモンによってメッセージを切り替える）：
                        //   データ収集時に試料テーブルがX線管の近くで昇降します。
                        //   コリメータがX線管の近くにあると試料テーブルと衝突する恐れがあるので待避しておいてください。
                        //   よろしければＯＫをクリックして下さい。
                        //if (Interaction.MsgBox(CTResources.LoadResString(CTSettings.t20kinf.Data.ud_type == 1 ? 9471 : 9405)) + Constants.vbCrLf + CTResources.LoadResString(StringTable.IDS_ClickOK), MsgBoxStyle.OkCancel + MsgBoxStyle.Exclamation) == MsgBoxResult.Cancel)
                        string msg = (CTSettings.t20kinf.Data.ud_type == 1 ? CTResources.LoadResString(9471) : CTResources.LoadResString(9405)) + "\r\n" + CTResources.LoadResString(StringTable.IDS_ClickOK);
                        if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                }
                if (stsCorrect1.Status == StringTable.GC_STS_STANDBY_NG)
                {
                    //警告メッセージの表示（コモンによってメッセージを切り替える）：
                    //   データ収集時に試料テーブルがX線管の近くで昇降します。
                    //   コリメータがX線管の近くにあると試料テーブルと衝突する恐れがあるので待避しておいてください。
                    //   よろしければＯＫをクリックして下さい。
                    //if (Interaction.MsgBox(CTResources.LoadResString(CTSettings.t20kinf.Data.ud_type == 1 ? 9471 : 9405)) + Constants.vbCrLf + CTResources.LoadResString(StringTable.IDS_ClickOK), MsgBoxStyle.OkCancel + MsgBoxStyle.Exclamation) == MsgBoxResult.Cancel)
                    string msg = (CTSettings.t20kinf.Data.ud_type == 1 ? CTResources.LoadResString(9473) : CTResources.LoadResString(9405)) + "\r\n" + CTResources.LoadResString(StringTable.IDS_ClickOK);
                    if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                    {
                        return;
                    }

                }
            }

            //追加2014/10/07hata_v19.51反映
            //シフトスキャン収集にチェックがあるときは   'v18.00追加 byやまおか 2011/02/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //Rev25.00 WスキャンON時も同じ動きになる by長野 2016/07/05
            if ((chkShiftScan.CheckState == CheckState.Checked))
            {
                modScanCorrect.Flg_GainShiftScan = CheckState.Checked;      //フラグON
                //シフトスキャン収集にチェックがないときは
            }
            else
            {
                modScanCorrect.Flg_GainShiftScan = CheckState.Unchecked;    //フラグOFF
            }

            if ((chkHaFuOfScan.CheckState == CheckState.Checked))
            {
                modScanCorrect.Flg_GainHaFuOfScan = CheckState.Checked;      //フラグON
                //シフトスキャン収集にチェックがないときは
            }
            else
            {
                modScanCorrect.Flg_GainHaFuOfScan = CheckState.Unchecked;    //フラグOFF
            }

            //ビジーフラグをセット
            IsBusy = true;

            //'停止要求フラグクリア
            //UserStopClear
            //
            //'連続回転コーンビーム＋高速再構成の時は、RAMディスクのscanstopを使う v17.40 追加 by 長野
            //If smooth_rot_cone_flg = True Then
            //
            //    UserStopClear_rmdsk
            //
            //End If

            //停止要求フラグをクリアする             'v17.50上記の処理を関数化 by 間々田 2011/02/17
            modCT30K.CallUserStopClear();

            ScanCorrect.AutoCorFlag = 1;        //自動校正中フラグを1にする 山本 2010-03-06

            //Rev20.00 追加 自動校正開始前に各コントロールをロック by長野 2015/02/06
            LockControl();

            //自動校正の開始
            //if (DoAutoCapture())
            if (DoAutoCapture(ScanCorrect.AutoJdgCorResultFlag)) //Rev26.00 change by chouno 2017/01/06
            {
                //自動校正フォームを非表示にする
                //Me.Visible = False     'v15.0削除 by 間々田 2009/05/11 非表示にしない

                //Rev26.00 add 校正結果を自動で判定した場合は結果保存済み by chouno 2017/01/07
                if (ScanCorrect.AutoJdgCorResultFlag == false)
                {
                    //校正結果保存
                    if (SaveResult())
                    {
                        //Rev20.01 追加 by長野 2015/06/03
                        frmXrayControl.Instance.UpdateWarmUp();

                        //Rev20.00 ロックを解除 by長野 2015/02/06
                        UnLockControl();

                        //Rev20.01 追加 by長野 2015/06/03
                        frmXrayControl.Instance.UpdateWarmUp();

                        //自動校正フォームをアンロードする
                        this.Close();

                        return;
                    }
                }
            }
            else //Rev26.00 失敗した場合は、[ガイド]タブのスキャンエリア・条件の設定完了フラグを落とす add by chouno 2017/01/16
            {
                frmScanControl.Instance.setScanAreaAndCmpFlg(false);
            }

            //Rev20.00 ロックを解除 by長野 2015/02/06
            UnLockControl();

            //Rev26.00 add by chouno 2017/04/20
            frmCorrectionStatus.Instance.MyUpdate();
            MyUpdate();

            //Rev20.01 追加 by長野 2015/06/03
            frmXrayControl.Instance.UpdateWarmUp();

            //ビジーフラグをリセット
            IsBusy = false;

            ScanCorrect.AutoCorFlag = 0;    //自動校正中フラグを0にする 山本 2010-03-06

            //自動校正フォームを表示する
            //Me.Visible = True         'v15.0削除 by 間々田 2009/05/11
        }
        //*************************************************************************************************
        //機　　能： フォームロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v20.00  15/02/05   (検S1)長野      新規作成
        //*************************************************************************************************
        private void LockControl()
        {
            bakcwneViewGainEnabled = cwneViewGain.Enabled;
            bakcwneSumGainEnabled = cwneSumGain.Enabled;
            bakcwneGMAEnabled = cwneGMA.Enabled;
            bakchkHaFuOfScanEnabled = chkHaFuOfScan.Enabled;
            bakchkShiftScanEnabled = chkShiftScan.Enabled;
            bakchkGainTableRotEnabled = chkGainTableRot.Enabled;
            bakchkSPCorrectEnabled = chkSPCorrect.Enabled;
            bakcwneSumSpEnabled = cwneSumSp.Enabled;
            bakcwneSumVerEnabled = cwneSumVer.Enabled;
            bakcwneVMAEnabled = cwneVMA.Enabled;
            bakchkShadingEnabled = chkShading.Enabled;
            bakcwneSumOffEnabled = cwneSumOff.Enabled;
            bakchkYAxisMoveEnabled = chkYAxisMove.Enabled; //Rev20.00 追加 by長野 2015/02/16
            bakchkXAxisMoveEnabled = chkXAxisMove.Enabled; //Rev23.40 追加 by長野 2016/06/19

            cwneViewGain.Enabled = false;
            cwneSumGain.Enabled = false;
            cwneGMA.Enabled = false;
            chkHaFuOfScan.Enabled = false;
            chkShiftScan.Enabled = false;
            chkGainTableRot.Enabled = false;
            chkSPCorrect.Enabled = false;
            cwneSumSp.Enabled = false;
            cwneSumVer.Enabled = false;
            cwneVMA.Enabled = false;
            chkShading.Enabled = false;
            cwneSumOff.Enabled = false;
            chkYAxisMove.Enabled = false; //Rev20.00 追加 by長野 2015/02/16
            chkXAxisMove.Enabled = false; //Rev23.40 追加 by長野 2016/06/19
 
        }
        //*************************************************************************************************
        //機　　能： フォームロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v20.00  15/02/05   (検S1)長野      新規作成
        //*************************************************************************************************
        private void UnLockControl()
        {
            cwneViewGain.Enabled = bakcwneViewGainEnabled;
            cwneSumGain.Enabled = bakcwneSumGainEnabled;
            cwneGMA.Enabled = bakcwneGMAEnabled;
            chkHaFuOfScan.Enabled = bakchkHaFuOfScanEnabled;
            chkShiftScan.Enabled = bakchkShiftScanEnabled;
            chkGainTableRot.Enabled = bakchkGainTableRotEnabled;
            chkSPCorrect.Enabled = bakchkSPCorrectEnabled;
            cwneSumSp.Enabled = bakcwneSumSpEnabled;
            cwneSumVer.Enabled = bakcwneSumVerEnabled;
            cwneVMA.Enabled = bakcwneVMAEnabled;
            chkShading.Enabled = bakchkShadingEnabled;
            cwneSumOff.Enabled = bakcwneSumOffEnabled;
            chkYAxisMove.Enabled = bakchkYAxisMoveEnabled; //rev20.00 追加 by長野 2015/02/16
            chkXAxisMove.Enabled = bakchkXAxisMoveEnabled; //Rev23.40 追加 by長野 2016/06/19
        }
        //*************************************************************************************************
        //機　　能： フォームロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void frmAutoCorrection_Load(object sender, EventArgs e)
        {
            //実行時はフラグをセット
            modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTScanCorrect;

            //各コントロールのキャプションに文字をセット
            SetCaption();

            //v17.60 英語用レイアウト調整 by長野 2011/05/25
            if (modCT30K.IsEnglish)
            {
                EnglishAdjustLayout();
            }

            //現在のコモン内容を取り出す
            ScanCorrect.OptValueGet_Cor();

            //各コントロールの初期化
            InitControls();

            //各コントロールに値をセット         'v11.2追加 by 間々田 2006/01/12
            SetControls();

            //ビジープロパティの初期化
            IsBusy = false;
        }

        //*************************************************************************************************
        //機　　能： 初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 他クラスからの呼び出し用
        //
        //履　　歴： V26.00  17/01/06   (検S1)長野   新規作成
        //*************************************************************************************************
        public void initialize()
        {
            //実行時はフラグをセット
            modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTScanCorrect;

            //各コントロールのキャプションに文字をセット
            SetCaption();

            //v17.60 英語用レイアウト調整 by長野 2011/05/25
            if (modCT30K.IsEnglish)
            {
                EnglishAdjustLayout();
            }

            //現在のコモン内容を取り出す
            ScanCorrect.OptValueGet_Cor();

            //各コントロールの初期化
            InitControls();

            //各コントロールに値をセット         'v11.2追加 by 間々田 2006/01/12
            SetControls();

            //ビジープロパティの初期化
            IsBusy = false;
        }

        //*************************************************************************************************
        //機　　能： フォームアンロード時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void frmAutoCorrection_FormClosed(object sender, FormClosedEventArgs e)
        {
            //ビジーオフ
            IsBusy = false;

            //Rev23.11 /Rev20.111 追加 by長野 2015/12/28
            frmXrayControl.Instance.UpdateWarmUp();

            //Rev26.14 終了時は校正自動判定実行フラグはfalse
            ScanCorrect.AutoJdgCorResultFlag = false;

            //終了時はフラグをリセット
            modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTScanCorrect);

            Application.DoEvents();    //v17.60追加 byやまおか 2011/06/10
        }
        //*************************************************************************************************
        //機　　能： 終了処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V26.00  16/01/16   (検S1)長野   新規作成
        //*************************************************************************************************
        public void Terminated()
        {
            //ビジーオフ
            IsBusy = false;

            //Rev23.11 /Rev20.111 追加 by長野 2015/12/28
            frmXrayControl.Instance.UpdateWarmUp();

            //終了時はフラグをリセット
            modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTScanCorrect);

            Application.DoEvents();    //v17.60追加 byやまおか 2011/06/10
        }
        //*************************************************************************************************
        //機　　能： 各コントロールの初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void InitControls()
        {
            //Ｘ線外部制御不可の場合、管電流を不可視にする
            if (CTSettings.scaninh.Data.xray_remote == 1)
            {
                lblMAName0.Visible = false;
                lblColon9.Visible = false;
                cwneGMA.Visible = false;
                lblGMAUni.Visible = false;

                lblMAName2.Visible = false;
                lblColon4.Visible = false;

                cwneVMA.Visible = false;    //added by 山本　2002-1-7
                lblVMAUni.Visible = false;  //added by 山本　2002-1-7
            }

            //v7.0 added by 間々田 2003/09/26
            //X線検出器がフラットパネルの場合､幾何歪校正のフレームを見えなくし､下にあるフレームやボタンを上にずらす
            int posUp = 0;
            if (CTSettings.detectorParam.Use_FlatPanel)
            {
                fraVertical.Visible = false;
                posUp = fraVertical.Top - fraOffset.Top;
                fraOffset.Top = fraOffset.Top + posUp;
                cmdOK.Top = cmdOK.Top + posUp;
                cmdEnd.Top = cmdEnd.Top + posUp;
                this.Height = this.Height + posUp;
            }

            //２次元幾何歪の場合，スキャン位置校正と幾何歪校正の配置を入れ替える     v11.2追加 by 間々田 2005/10/07
            //If scaninh.full_distortion = 0 Then                            'v17.00削除 byやまおか 2010/01/20
            //v17.00追加 byやまおか 2010/01/20
            if ((CTSettings.scaninh.Data.full_distortion == 0) && (!CTSettings.detectorParam.Use_FlatPanel))
            {
                fraVertical.Top = fraScanPosi.Top;
                fraScanPosi.Top = fraVertical.Top + fraVertical.Height + 180/15;
                //スキャン位置校正対象・非対象チェックボックスの位置
                chkSPCorrect.Top = fraScanPosi.Top + 60;
            }

            //PkeFPDの場合はオフセット校正を先頭にする   'v17.00追加 byやまおか 2010/03/01
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))
            {
                fraOffset.Top = fraGain.Top;
                fraGain.Top = fraOffset.Top + fraOffset.Height + 180/15;
                fraScanPosi.Top = fraGain.Top + fraGain.Height + 180/15;
                chkSPCorrect.Top = fraScanPosi.Top + 60/15;
            }

            //Rev25.00 表示順に記述しなおし+表示位置調整追加 by長野 2016/08/04
            ////テーブル回転収集の表示(PkeFPDの場合は非表示)   'v17.02追加 byやまおか 2010/07/23
            //chkGainTableRot.Visible = !(CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);

            ////Rev20.00 追加 by長野 2015/02/16
            //if (CTSettings.scaninh.Data.table_y_axis_move_acquire == 0)
            //{
            //    chkYAxisMove.Visible = true;
            //    //Rev23.40 初期値をチェック有に変更 by長野 2016/06/19
            //    chkYAxisMove.CheckState = CheckState.Checked;
            //}
            //else
            //{
            //    chkYAxisMove.Visible = false;
            //}

            ////Rev23.40 追加 by長野 2016/06/19
            //if (CTSettings.scaninh.Data.table_x_axis_move_acquire == 0)
            //{
            //    chkXAxisMove.Visible = true;
            //    chkXAxisMove.CheckState = CheckState.Checked;
            //}
            //else
            //{
            //    chkXAxisMove.Visible = false;
            //}

            int NextPosX = 0;
            int NextPosY = 0;

            //Rev20.00 追加 by長野 2015/02/16
            if (CTSettings.scaninh.Data.table_y_axis_move_acquire == 0)
            {
                chkYAxisMove.Visible = true;
                //Rev23.40 初期値をチェック有に変更 by長野 2016/06/19
                chkYAxisMove.CheckState = CheckState.Checked;
                NextPosX = chkYAxisMove.Location.X;
                NextPosY = chkYAxisMove.Location.Y + chkYAxisMove.Height;
            }
            else
            {
                chkYAxisMove.Visible = false;
            }

            //Rev23.40 追加 by長野 2016/06/19
            if (CTSettings.scaninh.Data.table_x_axis_move_acquire == 0)
            {
                chkXAxisMove.Visible = true;
                chkXAxisMove.CheckState = CheckState.Checked;
                NextPosX = chkXAxisMove.Location.X;
                NextPosX = chkXAxisMove.Location.Y + chkXAxisMove.Height;
            }
            else
            {
                chkXAxisMove.Visible = false;
            }

            //追加2014/10/07hata_v19.51反映
            //シフトスキャン機能     'v18.00追加 byやまおか 2011/02/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            //fraShiftScan.Visible = CTSettings.DetShiftOn;
            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            fraShiftScan.Visible = (CTSettings.DetShiftOn || CTSettings.W_ScanOn);
            fraShiftScan.BorderStyle = BorderStyle.None;

            //テーブル回転収集の表示(PkeFPDの場合は非表示)   'v17.02追加 byやまおか 2010/07/23
            chkGainTableRot.Visible = !(CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke);

            //追加2015/01/28hata
            //Tabインデックスを設定する
            //上から順番にTabが移動するようにする
            //通常(fraGain(1)-fraScanPosi(2)-fraVertical(3)-fraOffset(4)) 
            if (fraOffset.Top < fraGain.Top)
            {
                //Pkeの場合Offsetを先頭にする
                fraOffset.TabIndex = 1;     //Offset
            }
            else if (fraVertical.Top < fraScanPosi.Top)
            {
                //２次元幾何歪の場合Verticalを2番目にする
                fraVertical.TabIndex = 2;   //Vertical
                fraScanPosi.TabIndex = 3;   //ScanPosi
            }

            //scaninhによって校正結果の自動判定チェックボックスの表示・非表示 by井上 2017/12/12
            if (CTSettings.scaninh.Data.auto_judge == 1)
            {
                chkAutoJdgResult.Visible = false;
            }

        }

        //*************************************************************************************************
        //機　　能： 各コントロールのキャプションに文字列をセットする
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V7.00  03/08/25 (SI4)間々田     新規作成
        //*************************************************************************************************
        private void SetCaption()
        {
            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            //ゲイン校正フレーム
            chkGainTableRot.Text = StringTable.BuildResStr(StringTable.IDS_Rotate, StringTable.IDS_Table);
            //テーブル回転
            //lblGMAUni.Caption = LoadResString(10815)                                       'μA
            lblGMAUni.Text = modXrayControl.CurrentUni;     //v11.4変更 by 間々田 2006/03/02

            //Rev20.00 追加 by長野 2015/02/16
            // Mod Start 2018/08/23 M.Oyama 中国語対応
            //chkYAxisMove.Text = StringTable.GetResString(StringTable.IDS_Table) + StringTable.GetResString(StringTable.IDS_Move, CTSettings.AxisName[1]);
            chkYAxisMove.Text = StringTable.GetResString(StringTable.IDS_SampleTable) + StringTable.GetResString(StringTable.IDS_Move, CTSettings.AxisName[1]);
            // Mod End 2018/08/23

            //Rev23.40 追加 by長野 2016/06/19
            // Mod Start 2018/08/23 M.Oyama 中国語対応
            //chkXAxisMove.Text = StringTable.GetResString(StringTable.IDS_Table) + StringTable.GetResString(StringTable.IDS_Move, StringTable.GetResString(StringTable.IDS_Axis, CTResources.LoadResString(StringTable.IDS_FCD)));
            chkXAxisMove.Text = StringTable.GetResString(StringTable.IDS_SampleTable) + StringTable.GetResString(StringTable.IDS_Move, StringTable.GetResString(StringTable.IDS_Axis, CTResources.LoadResString(StringTable.IDS_FCD)));
            // Mod End 2018/08/23

            //追加2014/10/07hata_v19.51反映
            //スキャン収集     'v18.00追加 byやまおか 2011/03/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            chkHaFuOfScan.Text = CTResources.LoadResString(StringTable.IDS_Half) + ", " + CTResources.LoadResString(StringTable.IDS_Full) + ", " + CTResources.LoadResString(StringTable.IDS_Offset) + CTResources.LoadResString(StringTable.IDS_ScanCollection);            //ハーフフルオフセット
            
            //chkShiftScan.Text = CTResources.LoadResString(StringTable.IDS_Shift) + CTResources.LoadResString(StringTable.IDS_ScanCollection);            //シフト
            //Rev25.00 通常のシフトとWスキャンで表示文字列を変更 by長野 2016/07/05
            //if (CTSettings.W_ScanOn)
            //Rev26.10 シフトスキャンの名称がWスキャンの場合は、Wスキャンで表示 by chouno 2018/01/16
            if (CTSettings.W_ScanOn || CTSettings.infdef.Data.scan_mode[3].GetString() == CTResources.LoadResString(25009))
            {
                chkShiftScan.Text = "W" + CTResources.LoadResString(StringTable.IDS_ScanCollection);            //Wスキャン
            }
            else
            {
                chkShiftScan.Text = CTResources.LoadResString(StringTable.IDS_Shift) + CTResources.LoadResString(StringTable.IDS_ScanCollection);            //シフト
            }

            //幾何歪校正フレーム
            lblVMAUni.Text = modXrayControl.CurrentUni;     //v11.4変更 by 間々田 2006/03/02

            Label11.Text = (CTSettings.scaninh.Data.full_distortion == 0 ? CTResources.LoadResString(12053) : CTResources.LoadResString(12059));//穴の間隔/垂直線ﾜｲﾔﾋﾟｯﾁ

            //v17.60 フォントがバラつき不自然なため削除 by 長野 2011/06/11
            //英語環境の場合、ラベルコントロールに使用するフォントをArialにする
            //If IsEnglish Then SetLabelFont Me
        }

        //*************************************************************************************************
        //機　　能： ゲイン校正
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 各校正を関数化
        //
        //履　　歴： v11.1 2005/10/11  (SI3)間々田   新規作成
        //*************************************************************************************************
        private bool GainCorrect()
        {
            //戻り値初期化
             bool functionReturnValue = false;
             int ret = 0;

            //確認メッセージ：
            //   必要に応じて試料テーブルにゲイン校正ファントムを取り付けたり管電流値を小さくしたりして、Ｘ線をオンしてください。
            //   準備ができたらＯＫをクリックしてください。
             if (CTSettings.scaninh.Data.xray_remote == 1)
             {
                MessageBox.Show(CTResources.LoadResString(9569) + CTResources.LoadResString(StringTable.IDS_ClickOKIfReady), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
             }

             //追加2014/10/07hata_v19.51反映
             //シフトスキャン収集にチェックがあるときは   'v18.00追加 byやまおか 2011/02/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
             //Rev25.00 WスキャンON時も同じ動きになる by長野 2016/07/05 
             if ((chkShiftScan.CheckState == CheckState.Checked))
             {
                 modScanCorrect.Flg_GainShiftScan = CheckState.Checked;                 //フラグON
             
             //シフトスキャン収集にチェックがないときは
             }
             else
             {
                 modScanCorrect.Flg_GainShiftScan = CheckState.Unchecked;                 //フラグOFF
             }

             //Rev23.20 左右シフト対応 by長野 2015/11/19
             if ((chkHaFuOfScan.CheckState == CheckState.Checked))
             {
                 modScanCorrect.Flg_GainHaFuOfScan = CheckState.Checked;                 //フラグON

                 //シフトスキャン収集にチェックがないときは
             }
             else
             {
                 modScanCorrect.Flg_GainHaFuOfScan = CheckState.Unchecked;                 //フラグOFF
             }

            //ゲイン校正：v15.0引数(7番目)追加 by 間々田 2009/05/12 テーブルを最下部まで下降させる
            //2014/11/06hata キャストの修正
            //if (!modScanCorrectNew.GetImageForGainCorrect(stsCorrect0, 
            //                                              pgbAutoCorrect, 
            //                                              Convert.ToInt32(cwneViewGain.Value), 
            //                                              Convert.ToInt32(cwneSumGain.Value), 
            //                                              chkGainTableRot.CheckState, 
            //                                              Convert.ToInt32(cwneGMA.Value),
            //                                              (CTSettings.GValUpperLimit - (float)frmMechaControl.Instance.ntbUpDown.Value)))
            //Rev20.00 引数追加 by長野 2015/02/16
            //if (!modScanCorrectNew.GetImageForGainCorrect(stsCorrect0,
            //                                                pgbAutoCorrect,
            //                                                Convert.ToInt32(cwneViewGain.Value),
            //                                                Convert.ToInt32(cwneSumGain.Value),
            //                                                chkGainTableRot.CheckState,
            //                                                Convert.ToInt32(cwneGMA.Value),
            //                                                (CTSettings.GValUpperLimit - (float)frmMechaControl.Instance.ntbUpDown.Value),
            //                                                chkYAxisMove.CheckState == CheckState.Checked?(CTSettings.t20kinf.Data.y_axis_upper_limit - (float)frmMechaControl.Instance.ntbTableXPos.Value):0))
            //Rev23.40 引数変更 by長野 2016/06/19
            //if (!modScanCorrectNew.GetImageForGainCorrect(stsCorrect0,
            //                                                    pgbAutoCorrect,
            //                                                    Convert.ToInt32(cwneViewGain.Value),
            //                                                    Convert.ToInt32(cwneSumGain.Value),
            //                                                    chkGainTableRot.CheckState,
            //                                                    Convert.ToInt32(cwneGMA.Value),
            //                                                    (CTSettings.GValUpperLimit - (float)frmMechaControl.Instance.ntbUpDown.Value),
            //                                                    chkYAxisMove.CheckState == CheckState.Checked ? (CTSettings.t20kinf.Data.y_axis_upper_limit - (float)frmMechaControl.Instance.ntbTableXPos.Value) : 0,
            //                                                    chkXAxisMove.CheckState == CheckState.Checked ? (CTSettings.mechapara.Data.min_fcd - (float)frmMechaControl.Instance.ntbFCD.Value) : 0))
            //Rev26.00 change by chouno 2017/01/17
            if (!modScanCorrectNew.GetImageForGainCorrect(stsCorrect0,
                                                                pgbAutoCorrect,
                                                                Convert.ToInt32(cwneViewGain.Value),
                                                                Convert.ToInt32(cwneSumGain.Value),
                                                                chkGainTableRot.CheckState,
                                                                Convert.ToInt32(cwneGMA.Value),
                                                                (CTSettings.GValUpperLimit - (float)frmMechaControl.Instance.ntbUpDown.Value),
                                                                chkYAxisMove.CheckState == CheckState.Checked ? (CTSettings.iniValue.GainCorTableY - (float)frmMechaControl.Instance.ntbTableXPos.Value) : 0,
                                                                chkXAxisMove.CheckState == CheckState.Checked ? (CTSettings.iniValue.GainCorFCD - (float)frmMechaControl.Instance.ntbFCD.Value) : 0,
                                                                stsCorrect1))
            {
                //Ｘ線ＯＦＦ処理
                //Call_XrayOFF
                //v11.3変更 by 間々田 2006/02/20
                if (CTSettings.scaninh.Data.xray_remote == 0)
                {
                    modXrayControl.XrayOff();
                    Application.DoEvents();
                }

                //メッセージ表示：
                //   ゲイン校正に失敗しましたので、全自動校正を中止します。
                //   ゲイン校正は手動で行ってください。
                MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_ErrCorAuto, StringTable.IDS_CorGain), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                //PkeFPDの場合は元のゲイン校正画像をプリロードする     'v17.02追加 byやまおか 2010/07/15
                if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))
                {
                    //変更2014/10/07hata_v19.51反映
                    //ret = Pulsar.PkeSetGainData(Pulsar.hPke, 0, ScanCorrect.Gain_Image_L, 1);
                    ret = Pulsar.PkeSetGainData(Pulsar.hPke, ScanCorrect.Gain_Image_L, 1, ScanCorrect.GAIN_CORRECT_L);  //v18.00変更 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
					                    
                    //v17.60 ストリングテーブル化 by長野 2011/05/25
                    //If ret = 1 Then MsgBox "ゲイン校正データをセットできませんでした。", vbCritical
                    if (ret == 1)
                    {
                        MessageBox.Show(CTResources.LoadResString(20003), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                return functionReturnValue;
            }

            //データ収集完了
            stsCorrect0.Status = StringTable.GC_STS_CAPT_OK;

            //戻り値セット
            functionReturnValue = true;
            return functionReturnValue;
        }
        //*************************************************************************************************
        //機　　能： ゲイン校正判定
        //
        //           変数名          [I/O] 型        内容
        //引　　数： ゲイン校正データファイル名
        //戻 り 値： true:成功、false:失敗
        //
        //補　　足： 偏差の範囲でゲイン校正の合否を判定
        //
        //履　　歴： v11.1 2005/10/11  (SI3)間々田   新規作成
        //*************************************************************************************************
        private bool jdgGainCorrect(ushort[] gain_data)
        {
            bool ret = false;
            int iret = 0;
            float devRangeMin = 0.0f;
            float devRangeMax = 0.0f;
            
            //条件取得
            iret = ScanCorrect.getJdgGainCorCond(ref devRangeMin, ref devRangeMax);
            if (iret != 0)
            {
                MessageBox.Show(CTResources.LoadResString(iret), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ret;
            }

            //偏差取得
            //PkeFPDの場合の額縁幅を計算する 'v17.53追加 byやまおか 2011/05/13
            int modX = 0;
            int modY = 0;
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) && (!CTSettings.detectorParam.Use_FpdAllpix))
            {
                modX = Convert.ToInt32((frmTransImage.Instance.ctlTransImage.SizeX % 100) / 2F);
                modY = Convert.ToInt32((frmTransImage.Instance.ctlTransImage.SizeY % 100) / 2F);
            }
            else
            {
                modX = 0;
                modY = 0;
            }

            int StartXIndex = 0;
            int EndXIndex = 0;
            int StartYIndex = 0;
            int EndYIndex = 0;
            StartXIndex = modX;
            EndXIndex = CTSettings.detectorParam.h_size - modX;
            StartYIndex = modY;
            EndYIndex = CTSettings.detectorParam.v_size - modY;

            //統計情報を求める。
            int minVal = 0;
            int maxVal = 0;
            float div = 0.0f;
            float mean = 0.0f;
            ScanCorrect.GetStatisticalInfo(ref gain_data[0], CTSettings.detectorParam.h_size,CTSettings.detectorParam.v_size, StartXIndex, EndXIndex, StartYIndex, EndYIndex, ref minVal, ref maxVal, ref div, ref mean);

            //判定
            if (div < devRangeMin || div > devRangeMax)
            {
                ret = false;
            }
            else
            {
                ret = true;
            }

            return ret;
        }
        //*************************************************************************************************
        //機　　能： スキャン位置校正
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 各校正を関数化
        //
        //履　　歴： v11.2 2005/10/11  (SI3)間々田   新規作成
        //*************************************************************************************************
        private bool ScanPositionCorrect()
        {
            //戻り値初期化
            bool functionReturnValue = false;

            //確認メッセージ：
            //   Ｘ線をオンしてください。
            //   準備ができたらＯＫをクリックしてください。
            if (CTSettings.scaninh.Data.xray_remote == 1)
            {
                MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_TurnOn, StringTable.IDS_Xray), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            //スキャン位置校正画像を配列に読み込む
            //2014/11/06hata キャストの修正
            if (!modScanCorrectNew.GetImageForScanPositionCorrect(stsCorrect1, pgbAutoCorrect, Convert.ToInt32(cwneSumSp.Value)))
            {
                //Ｘ線ＯＦＦ処理
                //Call_XrayOFF
                //v11.3変更 by 間々田 2006/02/20
                if (CTSettings.scaninh.Data.xray_remote == 0)
                {
                    modXrayControl.XrayOff();
                    Application.DoEvents();
                }

                //メッセージ表示：
                //   スキャン位置校正に失敗しましたので、全自動校正を中止します。
                //   スキャン位置校正は手動で行ってください。
                MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_ErrCorAuto, StringTable.IDS_CorScanPos), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return functionReturnValue;
            }

            //データ収集完了
            stsCorrect1.Status = StringTable.GC_STS_CAPT_OK;

            //戻り値セット
            functionReturnValue = true;
            return functionReturnValue;
        }
        //*************************************************************************************************
        //機　　能： オフセット校正判定
        //
        //           変数名          [I/O] 型        内容
        //引　　数： オフセット校正データファイル名
        //戻 り 値： true:成功、false:失敗
        //
        //補　　足： 偏差の範囲でゲイン校正の合否を判定
        //
        //履　　歴： v11.1 2005/10/11  (SI3)間々田   新規作成
        //*************************************************************************************************
        private bool jdgScanPositionCorrect(ref ushort[] position_data)
        {
            bool ret = false;
            int iret = 0;
            float slope = 0.0f;
            float intercept = 0.0f;

            float interceptMin = 0.0f;
            float intercpetMax = 0.0f;
            float slopeMin = 0.0f;
            float slopeMax = 0.0f;

            //条件取得
            iret = ScanCorrect.getJdgScanPosCorCond(ref interceptMin, ref intercpetMax, ref slopeMin, ref slopeMax);
            if (iret != 0)
            {
                MessageBox.Show(CTResources.LoadResString(iret), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ret;
            }

            if (ScanCorrect.AcqScanPosition(CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, ref slope, ref intercept, ref position_data[0]) == -1)
            {
                //MsgBox "スキャン位置の抽出に失敗しました。再度実行してもエラーする場合は手動で実行してください。", vbCritical
                MessageBox.Show(CTResources.LoadResString(20076) + System.Environment.NewLine + CTResources.LoadResString(20077), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);//ストリングテーブル化 '17.60 by長野 2011/05/22
                return ret;
            }

            ScanCorrect.myA = slope;
            ScanCorrect.myB = intercept;

            //AcqScanPositionはmyA,myBが算出できれば成功となるので、ここで算出した値をチェックする。
            if(intercept < interceptMin || intercept > intercpetMax)
            {
                ret = false;
            }
            else
            {
                ret = true;
            }

            if (slope < slopeMin || slope > slopeMax)
            {
                ret = false;
            }
            else
            {
                ret = true;
            }

            return ret;
        }

        //*************************************************************************************************
        //機　　能： 幾何歪校正
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 各校正を関数化
        //
        //履　　歴： v11.2 2005/10/11  (SI3)間々田   新規作成
        //*************************************************************************************************
        private bool VerticalCorrect()
        {
            //戻り値初期化
            bool functionReturnValue = false;

            //確認メッセージ：
            //   Ｘ線をオンしてください。
            //   準備ができたらＯＫをクリックしてください。
            if (CTSettings.scaninh.Data.xray_remote == 1)
            {
                string msg = StringTable.BuildResStr(StringTable.IDS_TurnOn, StringTable.IDS_Xray) + "\r" + CTResources.LoadResString(StringTable.IDS_ClickOKIfReady);
                MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            //幾何歪校正画像を配列に読み込む：v15.0引数(7番目)追加 by 間々田 2009/05/12 テーブルを最下部まで下降させる
            //2014/11/06hata キャストの修正
            if (!modScanCorrectNew.GetImageForVerticalCorrect(stsCorrect2, 
                                                              pgbAutoCorrect, 
                                                              Convert.ToInt32(cwneSumVer.Value), 
                                                              Convert.ToInt32(chkShading.CheckState), 
                                                              Convert.ToInt32(cwneVMA.Value),
                                                              Convert.ToInt32((CTSettings.GValUpperLimit - (float)frmMechaControl.Instance.ntbUpDown.Value))))
            {
                //Ｘ線ＯＦＦ処理
                //Call_XrayOFF
                //v11.3変更 by 間々田 2006/02/20
                if (CTSettings.scaninh.Data.xray_remote == 0)
                {
                    modXrayControl.XrayOff();
                    System.Windows.Forms.Application.DoEvents();
                }

                //メッセージ表示：
                //   幾何歪校正に失敗しましたので、全自動校正を中止します。
                //   幾何歪校正は手動で行ってください。
                MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_ErrCorAuto, StringTable.IDS_CorDistortion), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return functionReturnValue;
            }

            //データ収集完了
            stsCorrect2.Status = StringTable.GC_STS_CAPT_OK;

            //戻り値セット
            functionReturnValue = true;
            return functionReturnValue;
        }

        //*************************************************************************************************
        //機　　能： オフセット校正
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 各校正を関数化
        //
        //履　　歴： v11.1 2005/10/11  (SI3)間々田   新規作成
        //*************************************************************************************************
        private bool OffsetCorrect()
        {
            //戻り値初期化
             bool functionReturnValue = false;
             int ret = 0;
 
            //確認メッセージ：
            //   Ｘ線をオフしてください。
            //   準備ができたらＯＫをクリックしてください。
            if (CTSettings.scaninh.Data.xray_remote == 1)
            {
                //Interaction.MsgBox(StringTable.BuildResStr(StringTable.IDS_TurnOff, StringTable.IDS_Xray) + Constants.vbCr + CT30K.My.Resources.ResourceManager.GetString("str" + Convert.ToString(StringTable.IDS_ClickOKIfReady)), MsgBoxStyle.Exclamation);
                string msg = StringTable.BuildResStr(StringTable.IDS_TurnOff, StringTable.IDS_Xray) + "\r" + CTResources.LoadResString(StringTable.IDS_ClickOKIfReady);
                MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            //ここでオフしない。GetImageForOffsetCorrect()の中でオフする 'v17.43削除 byやまおか 2011/01/21
            //'Ｘ線オフ   'v16.30/v17.00追加 byやまおか 2010/03/01
            //If (scaninh.xray_remote = 0) Then
            //    If IsXrayOn Then XrayOff
            //End If

            //オフセット校正画像を配列に読み込む
            //if (!modScanCorrectNew.GetImageForOffsetCorrect(stsCorrect3, pgbAutoCorrect, Convert.ToInt32(cwneSumOff.Value)))
            if (!modScanCorrectNew.GetImageForOffsetCorrect(stsCorrect3, pgbAutoCorrect, Convert.ToInt32(cwneSumOff.Value),chkAutoJdgResult.Checked)) //Rev26.00 change by chouno 2017/01/05 
            {
                //メッセージ表示：
                //   オフセット校正に失敗しましたので、全自動校正を中止します。
                //   オフセット校正は手動で行ってください。
                MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_ErrCorAuto, StringTable.IDS_CorOffset), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return functionReturnValue;
            }

            //PkeFPDの場合   'v17.02追加 byやまおか 2010/07/15
            
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))
            {
                //変更2014/10/07hata_v19.51反映
#if (!NoCamera) //'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                //元のオフセット校正画像をプリロードする
                //Ipc32v5.ret = ScanCorrect.PkeSetOffsetData(modCT30K.hPke, 0, ref ScanCorrect.OFFSET_IMAGE[0], 0);
                ret = Pulsar.PkeSetOffsetData(Pulsar.hPke, 0, ScanCorrect.OFFSET_IMAGE, 0);
                             
                //If ret = 1 Then MsgBox "オフセット校正データをセットできませんでした。", vbCritical
                //v17.60 ストリングテーブル化 by長野 2011/05/25
                if (ret == 1)
                {
                    MessageBox.Show(CTResources.LoadResString(20003), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
#endif
            }

            //データ収集完了
            stsCorrect3.Status = StringTable.GC_STS_CAPT_OK;

            //戻り値セット
            functionReturnValue = true;
            return functionReturnValue;
        }
        //*************************************************************************************************
        //機　　能： オフセット校正判定
        //
        //           変数名          [I/O] 型        内容
        //引　　数： オフセット校正データファイル名
        //戻 り 値： true:成功、false:失敗
        //
        //補　　足： 偏差の範囲でゲイン校正の合否を判定
        //
        //履　　歴： v11.1 2005/10/11  (SI3)間々田   新規作成
        //*************************************************************************************************
        private bool jdgOffsetCorrect(double[] offset_data)
        {
            bool ret = false;
            int iret = 0;
            float devRangeMin = 0.0f;
            float devRangeMax = 0.0f;
            float meanRangeMin = 0.0f;
            float meanRangeMax = 0.0f;

            //条件取得
            iret = ScanCorrect.getJdgOffsetCorCond(ref devRangeMin, ref devRangeMax,ref meanRangeMin,ref meanRangeMax);
            if (iret != 0)
            {
                MessageBox.Show(CTResources.LoadResString(iret), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ret;
            }

            //PkeFPDの場合の額縁幅を計算する 'v17.53追加 byやまおか 2011/05/13
            int modX = 0;
            int modY = 0;
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) && (!CTSettings.detectorParam.Use_FpdAllpix))
            {
                modX = Convert.ToInt32((frmTransImage.Instance.ctlTransImage.SizeX % 100) / 2F);
                modY = Convert.ToInt32((frmTransImage.Instance.ctlTransImage.SizeY % 100) / 2F);
            }
            else
            {
                modX = 0;
                modY = 0;
            }

            int StartXIndex = 0;
            int EndXIndex = 0;
            int StartYIndex = 0;
            int EndYIndex = 0;
            StartXIndex = modX;
            EndXIndex = CTSettings.detectorParam.h_size - modX;
            StartYIndex = modY;
            EndYIndex = CTSettings.detectorParam.v_size - modY;

            //統計情報を求める。
            int minVal = 0;
            int maxVal = 0;
            float div = 0.0f;
            float mean = 0.0f;
            ScanCorrect.GetStatisticalInfo_double(ref offset_data[0], CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size, StartXIndex, EndXIndex, StartYIndex, EndYIndex, ref minVal, ref maxVal, ref div, ref mean);

            //判定
            if (div < devRangeMin || div > devRangeMax)
            {
                ret = false;
            }
            else
            {
                ret = true;
            }

            if (mean < meanRangeMin || mean > meanRangeMax)
            {
                ret = false;
            }
            else
            {
                ret = true;
            }

            return ret;
        
        }
        //*************************************************************************************************
        //機　　能： 自動校正画面更新処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.1 2005/11/14  (SI3)間々田   新規作成
        //*************************************************************************************************
        //private void Update()
        private void MyUpdate()
        {
            stsCorrect0.Status = frmScanControl.Instance.lblStatus[0].Text; //ゲイン校正ステータス
            stsCorrect1.Status = frmScanControl.Instance.lblStatus[1].Text; //スキャン位置校正ステータス
            stsCorrect2.Status = frmScanControl.Instance.lblStatus[2].Text; //幾何歪校正ステータス
            stsCorrect3.Status = frmScanControl.Instance.lblStatus[4].Text; //オフセット校正ステータス

            //スキャン位置校正チェックボックス
            chkSPCorrect.CheckState = frmScanControl.Instance.chkInhibit[1].CheckState;
        }

        //*************************************************************************************************
        //機　　能： ステータスラベル変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 表示色を設定する
        //
        //履　　歴： v11.1 2005/11/14  (SI3)間々田   新規作成
        //*************************************************************************************************
        private void stsCorrect_Changed(object sender, EventArgs e)
		{
            //準備未完了の場合のみ，同一フレーム内のコントロールを使用可にする
            if (sender.Equals(stsCorrect0))
            {
                modLibrary.SetEnabledInFrame(fraGain, (stsCorrect0.Status == StringTable.GC_STS_STANDBY_NG), "Label");
            }
            else if (sender.Equals(stsCorrect1))
            {
                modLibrary.SetEnabledInFrame(fraScanPosi, (stsCorrect1.Status == StringTable.GC_STS_STANDBY_NG), "Label");
            }
            else if (sender.Equals(stsCorrect2))
            {
                modLibrary.SetEnabledInFrame(fraVertical, (stsCorrect2.Status == StringTable.GC_STS_STANDBY_NG), "Label");
            }
            else if (sender.Equals(stsCorrect3))
            {
                modLibrary.SetEnabledInFrame(fraOffset, (stsCorrect3.Status == StringTable.GC_STS_STANDBY_NG), "Label");
            }
		}

        //*************************************************************************************************
        //機　　能： 自動収集関数
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [ /O] Boolean   True: 成功　False: キャンセルもしくはエラー
        //
        //補　　足： なし
        //
        //履　　歴： v11.2 2006/01/12  (SI3)間々田   新規作成
        //*************************************************************************************************
        //private bool DoAutoCapture()
        public bool DoAutoCapture(bool autoJdgCorResult)//Rev26.00 change by chouno 2017/01/06
        {
            //戻り値の初期化
            bool functionReturnValue = false;
            int ret = 0;

            ScanCorrect.GainCorFlag = 0;

            //マウスカーソルを矢印つきの砂時計にする
            Cursor.Current = Cursors.AppStarting;

            //現在の昇降位置を記憶                       'v15.0追加 by 間々田 2009/05/12
            float OrgUDPos = 0;
            OrgUDPos = (float)frmMechaControl.Instance.ntbUpDown.Value;

            //タッチパネル操作を禁止（シーケンサ通信が可能な場合）
            modSeqComm.SeqBitWrite("PanelInhibit", true);

            //PkeFPDの場合、最初にオフセット校正を行う   'v17.00追加 byやまおか 2010/03/01
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))
            {
                if (stsCorrect3.Status == StringTable.GC_STS_STANDBY_NG)
                {
                    if (!OffsetCorrect())
                    {
                        goto ExitHandler;
                    }

                    //Rev26.00 オフセット校正の判定と保存 add by chouno 2017/01/06
                    if (autoJdgCorResult == true)
                    {
                        //判定
                        if (!jdgOffsetCorrect(ScanCorrect.OFFSET_IMAGE))
                        {
                            MessageBox.Show(CTResources.LoadResString(26009), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto ExitHandler;
                        }
                        //保存
                        ScanCorrect.offsetCorSave();
                    }

                    //変更2014/10/07hata_v19.51反映
#if (!NoCamera) //'v19.50 v19.41とv18.02の統合 by長野 2013/11/05

                    //パーキンエルマーFPDの場合はオフセット校正画像の保存をする v17.00追加 by　山本 2010-03-03
                    //新しい校正画像をプリロードする
                    //Ipc32v5.ret = ScanCorrect.PkeSetOffsetData(modCT30K.hPke, 1, ref ScanCorrect.OFFSET_IMAGE[0], 1);
                    ret = Pulsar.PkeSetOffsetData(Pulsar.hPke, 1, ScanCorrect.OFFSET_IMAGE, 1);

                    //If ret = 1 Then MsgBox "オフセット校正データをセットできませんでした。", vbCritical
                    //v17.60 ストリングテーブル化 by長野 2011/05/25
                    if (ret == 1)
                    {
                        MessageBox.Show(CTResources.LoadResString(20004), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
#endif
                }
            }

            //ゲイン校正
            if (stsCorrect0.Status == StringTable.GC_STS_STANDBY_NG)
            {
                if (!GainCorrect())
                {
                    goto ExitHandler;
                }
                ScanCorrect.GainCorFlag = 1;
                //added by 山本　2003-11-7　自動校正でゲイン校正をしたかどうかのフラッグを立てる

                //Rev26.00 ゲイン校正の判定と保存 add by chouno 2017/01/06 --->
                if (autoJdgCorResult == true)
                {
                    //判定
                    if (!jdgGainCorrect(ScanCorrect.GAIN_AIR_IMAGE))
                    {
                        MessageBox.Show(CTResources.LoadResString(26008), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        goto ExitHandler;
                    }
                    //判定 シフト
                    if (modScanCorrect.Flg_GainShiftScan == CheckState.Checked)
                    {
                        if (!jdgGainCorrect(ScanCorrect.GAIN_AIR_IMAGE_SFT_R))
                        {
                            MessageBox.Show(CTResources.LoadResString(26008), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto ExitHandler;
                        }
                        if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
                        {
                            if (!jdgGainCorrect(ScanCorrect.GAIN_AIR_IMAGE_SFT_L))
                            {
                                MessageBox.Show(CTResources.LoadResString(26008), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                goto ExitHandler;
                            }
                        }
                    }
                    //保存
                    ScanCorrect.gainCorSave();
                }
                //<--- 

                //パーキンエルマーFPDの場合はゲイン校正画像の保存をする v17.00追加 by　山本 2010-03-03
                //
                if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))
                {
                    //新しい校正画像をプリロードする
                    //変更2014/10/07hata_v19.51反映
#if (! NoCamera)    //'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    //ret = Pulsar.PkeSetGainData(Pulsar.hPke, 1, ScanCorrect.Gain_Image_L, 1);
                    ret = Pulsar.PkeSetGainData(Pulsar.hPke, ScanCorrect.Gain_Image_L, 1, "");   //v18.00変更 byやまおか 2011/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/12

                    //If ret = 1 Then MsgBox "ゲイン校正データをセットできませんでした。", vbCritical
                    //v17.60 ストリングテーブル化 by長野 2011/05/25
                    if (ret == 1)
                    {
                        MessageBox.Show(CTResources.LoadResString(20003), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //Rev26.00 add by chouno 
                        goto ExitHandler;
                    }
#endif
                }

            }

            //２次元幾何歪の場合，スキャン位置校正と幾何歪校正の順序を入れ替える
            if (CTSettings.scaninh.Data.full_distortion == 0)
            {
                //幾何歪校正     Ｘ線検出器がフラットパネルの場合、幾何歪校正処理は行わない
                if ((stsCorrect2.Status == StringTable.GC_STS_STANDBY_NG) && !CTSettings.detectorParam.Use_FlatPanel)
                {
                    if (!VerticalCorrect())
                    {
                        goto ExitHandler;
                    }
                }

                //元の昇降位置に戻す                         'v15.0追加 by 間々田 2009/05/12
                modMechaControl.MechaUdIndex(OrgUDPos);

                //mecainfの取得
                //modMecainf.GetMecainf(ref modMecainf.mecainf);  //v15.0追加 by 間々田 2009/05/12
                CTSettings.mecainf.Load();

                //スキャン位置校正
                if (stsCorrect1.Status == StringTable.GC_STS_STANDBY_NG)
                {
                    if (!ScanPositionCorrect())
                    {
                        goto ExitHandler;
                    }

                    //Rev26.00 スキャン位置校正の判定と保存
                    if (autoJdgCorResult == true)
                    {
                        //判定
                        if(!jdgScanPositionCorrect(ref ScanCorrect.POSITION_IMAGE))
                        {
                            MessageBox.Show(CTResources.LoadResString(26011),Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Error);
                            goto ExitHandler;
                        }
                        //保存
                        ScanCorrect.scanposCorSave(ScanCorrect.myA, ScanCorrect.myB);
                    }
                }
            }
            else
            {
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //        'スキャン位置校正
                //        If stsCorrect(1).Status = GC_STS_STANDBY_NG Then
                //            If Not ScanPositionCorrect() Then GoTo ExitHandler
                //        End If
                //
                //        '幾何歪校正     Ｘ線検出器がフラットパネルの場合、幾何歪校正処理は行わない
                //        If (stsCorrect(2).Status = GC_STS_STANDBY_NG) And Not Use_FlatPanel Then
                //            If Not VerticalCorrect() Then GoTo ExitHandler
                //        End If
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            }   //v11.2追加ここまで by 間々田 2005/10/11

            //オフセット校正（PkeFPDの場合は、ここで行わない）
            //v17.00 if追加 byやまおか 2010/03/01
            if (CTSettings.detectorParam.DetType != DetectorConstants.DetTypePke)
            {
                if (stsCorrect3.Status == StringTable.GC_STS_STANDBY_NG)
                {
                    if (!OffsetCorrect())
                    {
                        goto ExitHandler;
                    }
                    //Rev26.00 オフセット校正の判定と保存 add by chouno 2017/01/06
                    if (autoJdgCorResult == true)
                    {
                        //判定
                        if (!jdgOffsetCorrect(ScanCorrect.OFFSET_IMAGE))
                        {
                            MessageBox.Show(CTResources.LoadResString(26009), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto ExitHandler;
                        }
                        //保存
                        ScanCorrect.offsetCorSave();
                    }
                }
            }

            //戻り値セット
            functionReturnValue = true;

        ExitHandler:
            //Ｘ線ＯＦＦ処理
            if (CTSettings.scaninh.Data.xray_remote == 0)
            {
                modXrayControl.XrayOff();
            }

            modCT30K.PauseForDoEvents(0.3f); //added by 山本　2007-3-3  シーケンサ通信エラーが発生する対策

            //元の昇降位置に戻す                         'v15.0追加 by 間々田 2009/05/12
            modMechaControl.MechaUdIndex(OrgUDPos);

            //マウスカーソルを元に戻す
            Cursor.Current = Cursors.Default;

            //タッチパネル操作を許可
            modSeqComm.SeqBitWrite("PanelInhibit", false);
            return functionReturnValue;
        }

        //*************************************************************************************************
        //機　　能： 各校正結果を保存する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [ /O] Boolean   True: 成功　False: キャンセルもしくはエラー
        //
        //補　　足： なし
        //
        //履　　歴： v11.1 2005/11/14  (SI3)間々田   新規作成
        //*************************************************************************************************
        private bool SaveResult()
        {
            //戻り値の初期化
            bool functionReturnValue = false;

            //オフセット校正(PkeFPDの場合は最初に表示する)   'v17.00追加 byやまおか 2010/03/01
            if ((stsCorrect3.Status == StringTable.GC_STS_CAPT_OK) && (CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))
            {
                //オフセット校正結果フォームを表示する
                if (!frmOffsetResult.Instance.Dialog())
                {
                    return functionReturnValue;
                }
            }

            //ゲイン校正
            if (stsCorrect0.Status == StringTable.GC_STS_CAPT_OK)
            {
                //ゲイン校正結果フォームを表示する
                //変更2014/10/07hata_v19.51反映
                //if (!frmGainCorResult.Instance.Dialog())
                //{
                //    return functionReturnValue;
                //}
                //v19.50 'v19.41とv18.02の統合 by長野 2013/11/12
                if (!frmGainCorResult.Instance.Dialog(modScanCorrect.ModeCorConstants.ModeCor_origin))
                    return functionReturnValue;
                //基準位置のゲイン校正(常に実行)
                if (Convert.ToBoolean(modScanCorrect.Flg_GainShiftScan))
                {
                    //Rev23.20 左右シフト対応 by長野 2015/11/19
                    //if (!frmGainCorResult.Instance.Dialog(modScanCorrect.ModeCorConstants.ModeCor_shift))
                    //    return functionReturnValue;     //シフト位置のゲイン校正(チェックがあれば実行)
                    if (!frmGainCorResult.Instance.Dialog(modScanCorrect.ModeCorConstants.ModeCor_shift_R))
                        return functionReturnValue;     //シフト位置のゲイン校正(チェックがあれば実行)
                    //Rev25.03/25.02 条件追加 by chouno 2017/02/16
                    if (CTSettings.scaninh.Data.lr_sft == 0 || CTSettings.W_ScanOn)
                    {
                        if (!frmGainCorResult.Instance.Dialog(modScanCorrect.ModeCorConstants.ModeCor_shift_L))
                            return functionReturnValue;     //シフト位置のゲイン校正(チェックがあれば実行)
                    }

                }

            }

            //２次元幾何歪の場合，スキャン位置校正と幾何歪校正の順序を入れ替える
            if (CTSettings.scaninh.Data.full_distortion == 0)
            {
                //幾何歪校正
                if (stsCorrect2.Status == StringTable.GC_STS_CAPT_OK)
                {
                    //幾何歪校正パラメータ計算
                    if (ScanCorrect.Get_Vertical_Parameter_Ex(0) == -1)
                    {
                        return functionReturnValue;
                    }

                    //幾何歪校正結果フォームを表示する
                    if (!frmVerticalResult.Instance.Dialog())
                    {
                        return functionReturnValue;
                    }
                }

                //スキャン位置校正
                if (stsCorrect1.Status == StringTable.GC_STS_CAPT_OK)
                {
                    //スキャン位置校正結果フォームを表示する
                    if (!frmScanPositionResult.Instance.Dialog(true))
                    {
                        return functionReturnValue;
                    }
                }
            }
            else
            {
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
                //スキャン位置校正
                //        If stsCorrect(1).Status = GC_STS_CAPT_OK Then
                //
                //            'スキャン位置校正結果フォームを表示する
                //            If Not frmScanPositionResult.Dialog(True) Then Exit Function
                //
                //        End If
                //
                //        '幾何歪校正
                //        If stsCorrect(2).Status = GC_STS_CAPT_OK Then
                //
                //            '幾何歪校正パラメータ計算
                //            If Get_Vertical_Parameter_Ex(0) = -1 Then Exit Function
                //
                //            '幾何歪校正結果フォームを表示する
                //            If Not frmVerticalResult.Dialog() Then Exit Function
                //
                //        End If
                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            }

            //オフセット校正(PkeFPDの場合は表示しない)   'v17.00変更 byやまおか 2010/03/01
            //If stsCorrect(3).Status = GC_STS_CAPT_OK Then
            if ((stsCorrect3.Status == StringTable.GC_STS_CAPT_OK) && (CTSettings.detectorParam.DetType != DetectorConstants.DetTypePke))
            {
                //オフセット校正結果フォームを表示する
                if (!frmOffsetResult.Instance.Dialog())
                {
                    return functionReturnValue;
                }
            }

            //戻り値セット
            functionReturnValue = true;
            return functionReturnValue;
        }

        //*************************************************************************************************
        //機　　能： 各コントロールに値をセットする
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.2 2006/01/12  (SI3)間々田   新規作成
        //*************************************************************************************************
        private void SetControls()
        {
            //ゲイン校正の設定値
            //変更2014/11/28hata_v19.51_dnet
            //数字の初期値は10単位にする
            //cwneSumGain.Value = modScanCorrect.IntegNumAtGain;              //積算枚数
            cwneSumGain.Value = ScanCorrect.RoundControlVale(modScanCorrect.IntegNumAtGain, cwneSumGain.Maximum, cwneSumGain.Minimum, 10F);

            cwneViewGain.Maximum = CTSettings.GVal_ViewMax;                 //ビュー数最大値
            //変更2015/02/02hata_Max/Min範囲のチェック
            //cwneViewGain.Value = modScanCorrect.ViewNumAtGain;              //ビュー数
            cwneViewGain.Value = modLibrary.CorrectInRange(modScanCorrect.ViewNumAtGain, cwneViewGain.Minimum, cwneViewGain.Maximum);              //ビュー数
            
            chkGainTableRot.CheckState = modScanCorrect.GFlg_GainTableRot;  //テーブル回転有無
            //追加2014/11/26hata_v19.51_dnet
            //イベントを発生させる
            chkGainTableRot_CheckStateChanged(chkGainTableRot, EventArgs.Empty);        
            
            if (CTSettings.scaninh.Data.xray_remote == 0)                        //v15.10条件追加 byやまおか 2009/10/29
            {
                modLibrary.CopyCWNumEdit(frmXrayControl.Instance.cwneMA, cwneGMA);  //管電流：Ｘ線制御画面のコントロールをコピー
            }

            //追加2014/10/07hata_v19.51反映
            //シフトスキャン収集     'v18.00追加 byやまおか 2011/02/08 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if (CTSettings.DetShiftOn)
            {
                chkShiftScan.CheckState = (CTSettings.scansel.Data.scan_mode == (int)ScanSel.ScanModeConstants.ScanModeShift ? CheckState.Checked : CheckState.Unchecked);
                //追加2014/11/26hata_v19.51_dnet
                //イベントを発生させる
                chkShiftScan_CheckStateChanged(chkShiftScan, EventArgs.Empty);
            }

            //Rev25.00 Wスキャン追加 by長野 2016/06/19
            if (CTSettings.W_ScanOn)
            {
                chkShiftScan.CheckState = (CTSettings.scansel.Data.w_scan == 1? CheckState.Checked : CheckState.Unchecked);
                //イベントを発生させる
                chkShiftScan_CheckStateChanged(chkShiftScan, EventArgs.Empty);
            }
          
            //スキャン位置校正の設定値
            //変更2014/11/28hata_v19.51_dnet
            //数字の初期値は10単位にする
            //cwneSumSp.Value = ScanCorrect.IntegNumAtPos;                    //積算枚数
            cwneSumSp.Value = ScanCorrect.RoundControlVale(ScanCorrect.IntegNumAtPos, cwneSumSp.Maximum, cwneSumSp.Minimum, 10F);

            //幾何学歪校正の設定値
            //変更2014/11/28hata_v19.51_dnet
            //数字の初期値は10単位にする
            //cwneSumVer.Value = modScanCorrect.IntegNumAtVer;                //積算枚数
            cwneSumVer.Value = ScanCorrect.RoundControlVale(modScanCorrect.IntegNumAtVer, cwneSumVer.Maximum, cwneSumVer.Minimum, 10F);

            cwneWireVer.Text = CTSettings.scancondpar.Data.ver_wire_pitch.ToString();  //垂直線ワイヤピッチ
            chkShading.CheckState = (CheckState)System.Math.Sign(modScanCorrect.GFlg_Shading_Ver);//シェーディング補正
            
            //v15.10条件追加 byやまおか 2009/10/29
            if (CTSettings.scaninh.Data.xray_remote == 0)
            {
                modLibrary.CopyCWNumEdit(frmXrayControl.Instance.cwneMA, cwneVMA);  //管電流：Ｘ線制御画面のコントロールをコピー
            }

            //オフセット校正の設定値
            //変更2014/11/28hata_v19.51_dnet
            //数字の初期値は10単位にする
            //cwneSumOff.Value = ScanCorrect.IntegNumAtOff;                   //積算枚数
            cwneSumOff.Value = ScanCorrect.RoundControlVale(ScanCorrect.IntegNumAtOff, cwneSumOff.Maximum, cwneSumOff.Minimum, 10F);
        }

        //*************************************************************************************************
        //機　　能： 英語用レイアウト調整
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V17.60  11/05/25 (検S１)長野    新規作成
        //*************************************************************************************************
        private void EnglishAdjustLayout()
        {
            //2014/11/06hata キャストの修正
            stsCorrect0.Width = Convert.ToInt32(stsCorrect0.Width * 1.2F);
            stsCorrect1.Width = stsCorrect0.Width;
            stsCorrect2.Width = stsCorrect0.Width;
            stsCorrect3.Width = stsCorrect0.Width;
        }

        //追加2014/11/28hata_v19.51_dnet
        //*************************************************************************************************
        //機　　能： ビュー数変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V19.51dnet  99/XX/XX   ????????      新規作成
        //*************************************************************************************************
        private void cwneViewGain_ValueChanged(object sender, EventArgs e)
        {
            decimal val1 = 0;

            //数字のインクリメントを合わせる10,100,200･･･
            //val1 = cwneViewGain.Value / (decimal)100.0;
            //val1 = Math.Round(val1, 0, MidpointRounding.AwayFromZero) * 100;
            //if (val1 < cwneViewGain.Minimum) val1 = cwneViewGain.Minimum;
            //if (val1 > cwneViewGain.Maximum) val1 = cwneViewGain.Maximum;
            val1 = ScanCorrect.RoundControlVale(cwneViewGain.Value, cwneViewGain.Maximum, cwneViewGain.Minimum, 100F);
            if (cwneViewGain.Value != val1)
            {
                cwneViewGain.Value = val1;
            }
        }
        //*************************************************************************************************
        //機　　能： 校正判定実施切り替え用チェックボックスのチェック変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V26.00  17/01/07  (検S1)長野    新規作成
        //*************************************************************************************************
        private void chkAutoJdgResult_CheckedChanged(object sender, EventArgs e)
        {
            if(chkAutoJdgResult.Checked == true)
            {
                ScanCorrect.AutoJdgCorResultFlag = true;
            }
            else
            {
                ScanCorrect.AutoJdgCorResultFlag = false;
            }
        }

    }
}
