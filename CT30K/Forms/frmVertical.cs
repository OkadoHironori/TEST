using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： frmVertical.frm                                             */
    ///* 処理概要　　： 幾何歪校正                                                  */
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
    ///* V4.0        01/01/30    (ITC)    鈴山　修   ﾓｰﾀﾞﾙﾌｫｰﾑ→MDI子ﾌｫｰﾑに変更     */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
    public partial class frmVertical : Form
    {
        #region インスタンスを返すプロパティ

        // frmVerticalのインスタンス
        private static frmVertical _Instance = null;

        /// <summary>
        /// frmVerticalのインスタンスを返す
        /// </summary>
        public static frmVertical Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmVertical();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmVertical()
        {
            InitializeComponent();
        }
   
        float Val_udab_pos;     //現在の昇降絶対位置
        //bool IsOkDownTable;     //テーブル下降が実行されたかどうかのフラグ
        //bool IsOkSetPhantom;

        bool myBusy;

        //Rev20.02 追加 by長野 2015/06/20
        private char presskye = (char)0;  //Keypressの値          
        private string PreUpDownValText = "";   //cwneDownTableDistanceを変更した前回の値
        private string PreUpDownEnterValText = "";   //cwneDownTableDistanceを確定した前回の値

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

                //各コントロールのEnabledプロパティを制御
                cmdEnd.Enabled = !myBusy;
                cwneSum.Enabled = !myBusy;
                cwneMA.Enabled = !myBusy;
                chkShading.Enabled = !myBusy;
                chkDownTable.Enabled = !myBusy;
                cwneDownTableDistance.Enabled = (!myBusy) & (chkDownTable.CheckState == CheckState.Checked);

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
            int rc = 0;
            //    Dim Val_udab_pos    As Single   'テーブル下降収集用：現在の昇降絶対位置  append by 間々田 2003-03-03

            //メカが動ける(パネルがOFF)かチェック     'v18.00追加 byやまおか 2011/07/02 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            if ((!modMechaControl.IsOkMechaMove()))
                return;


            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //    'RAMディスクが構築されているかどうか  'v17.40変更 byやまおか 2010/10/26
            //    'If UseRamDisk And (Not RamDiskIsReady) Then Exit Sub
            //    If UseRamDisk Then      'v17.42修正 byやまおか 2010/11/04
            //        If (Not RamDiskIsReady) Then Exit Sub
            //    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //ビジーならば、校正実行中にクリックされたとみなし、dllに対して停止要求を行なう
            if (IsBusy)
            {
                //        'シーケンサ通信確認ファイルの書き込み（停止要求）
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
                //        'MilCaptureStop         'v17.00削除 byやまおか 2010/01/20
                //        Select Case DetType
                //            Case DetTypeII, DetTypeHama
                //                MilCaptureStop
                //            Case DetTypePke
                //                PkeCaptureStop (hPke)     'changed by 山本 2009-09-16
                //        End Select
                //#End If

                //実行中の処理に対して停止要求をする 'v17.50上記の処理を関数化 by 間々田 2011/02/17
                modCT30K.CallUserStopSet();

                return;
            }

            //フル２次元幾何歪補正時：ゲイン校正が準備完了になっているかチェックする     'v11.2追加 by 間々田 2006/01/13
            if (CTSettings.scaninh.Data.full_distortion == 0)
            {
                if (!frmScanControl.Instance.IsOkGain)
                {
                    //メッセージ表示：
                    //   ゲイン校正が準備完了でないため、処理を中止します。
                    //   事前にゲイン校正を実施してください。
                    MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_CorNotReady, StringTable.IDS_CorGain), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //'シーケンサ通信確認ファイルの値を0クリアする
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

            //I.I.（またはFPD）電源のチェック  条件追加 by 間々田 2004/12/28
            if (!modSeqComm.PowerSupplyOK())
            {
                return;
            }

            //テーブル下降収集ありの場合 added by 間々田 2003-03-03
            if (chkDownTable.CheckState == CheckState.Checked)
            {
                //現在の昇降絶対位置の取得
                Val_udab_pos = CTSettings.mecainf.Data.udab_pos;
                
                //もう一度ストローク数チェックを行なう
                //        If Val_udab_pos - cwneDownTableDistance.Value < theLimit / 100 Then
                //修正 by 間々田 2003-04-01
                float cwneDownTableDistanceValue = 0.0f;
                float.TryParse(cwneDownTableDistance.Text, out cwneDownTableDistanceValue);
                if (Val_udab_pos + cwneDownTableDistanceValue > CTSettings.GValUpperLimit)
                {
                    //エラーメッセージ：
                    //   テーブル下限を超えて下降しようとしています！
                    //   現在の昇降位置(mm): xxx
                    //   テーブル下限値(mm): xxx
                    //MessageBox.Show(StringTable.GetResString(9583, "\t" + Convert.ToString(Val_udab_pos), "\t" + Convert.ToString(CTSettings.GValUpperLimit)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Rev23.10 計測CT対応 by長野 2015/10/16
                    MessageBox.Show(StringTable.GetResString(9583, "\t" + Convert.ToString(frmMechaControl.Instance.Udab_Pos), "\t" + Convert.ToString(CTSettings.GValUpperLimit)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //テーブル下降収集用のストローク数を調整してこのルーチンから抜ける
                    //            cwneDownTableDistance.Value = Val_udab_pos - theLimit / 100
                    // 【C#コントロールで代用】
                    //2014/11/07hata キャストの修正
                    //cwneDownTableDistance.Text = modLibrary.MaxVal(Convert.ToInt32(CTSettings.GValUpperLimit - Val_udab_pos), 1).ToString();//修正 by 間々田 2003-04-01
                    cwneDownTableDistance.Text = modLibrary.MaxVal(Convert.ToInt32(Math.Floor(CTSettings.GValUpperLimit - Val_udab_pos)), 1).ToString();//修正 by 間々田 2003-04-01
                    return;
                }

                //テーブル下降収集有りとして変数に保存
                modScanCorrect.DownTable = CheckState.Checked;

                //テーブル下降収集用のストローク数（移動距離）（ｍｍ）も変数に保存
                // 【C#コントロールで代用】
                float downTableDistance = 0.0f;
                float.TryParse(cwneDownTableDistance.Text, out downTableDistance);
                modScanCorrect.DownTableDistance = downTableDistance;
            }
            else
            {
                //テーブル下降収集無として変数に保存
                modScanCorrect.DownTable = System.Windows.Forms.CheckState.Unchecked;
            }

            //ビジーフラグセット
            IsBusy = true;

            //幾何歪校正画像を配列に読み込む
            //v10.0変更 by 間々田 2005/01/31 pgbVertical追加
            //2014/11/07hata キャストの修正
            //if (modScanCorrectNew.GetImageForVerticalCorrect(stsVertical, 
            //                                                 pgbVertical, 
            //                                                 (int)cwneSum.Value, 
            //                                                 (chkShading.CheckState == CheckState.Checked ? 1 : 0),
            //                                                 (int)cwneMA.Value, 
            //                                                 (modScanCorrect.DownTable == CheckState.Checked ? modScanCorrect.DownTableDistance : 0)))
            if (modScanCorrectNew.GetImageForVerticalCorrect(stsVertical,
                                                             pgbVertical,
                                                             Convert.ToInt32(cwneSum.Value),
                                                             (chkShading.CheckState == CheckState.Checked ? 1 : 0),
                                                             Convert.ToInt32(cwneMA.Value),
                                                             (modScanCorrect.DownTable == CheckState.Checked ? modScanCorrect.DownTableDistance : 0)))
            {
                //幾何歪校正パラメータ計算
                rc = ScanCorrect.Get_Vertical_Parameter_Ex(0, true);

                if (rc != -1)
                {
                    if (rc == 0)
                    {
                        //フォームを消去
                        //変更2015/1/17hata_非表示のときにちらつくため
                        //Hide();
                        modCT30K.FormHide(this);

                        //幾何歪校正結果フォームを表示する
                        frmVerticalResult.Instance.Dialog();

                    }

                    //フォームをアンロードする
                    this.Close();

                    //Rev20.02 dispose追加 by長野 2015/06/18
                    this.Dispose(); ;

                    return;
                }
            }
            
            //Rev26.00 失敗の場合は[ガイド]タブのスキャンエリア・条件の設定完了フラグを落とす add by chouno 2017/01/16
            frmScanControl.Instance.setScanAreaAndCmpFlg(false);
            
            //「データ収集異常終了」と表示
            stsVertical.Status = StringTable.GC_STS_CAPT_NG;

            //ビジーオフ
            IsBusy = false;
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
        private void cmdEnd_Click(object sender, EventArgs e)
        {
            //フォームをアンロードする
            this.Close();

            //Rev20.02 dispose追加 by長野 2015/06/18
            this.Dispose(); ;
        }

        //*******************************************************************************
        //機　　能： テーブル下降収集チェックボックスクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void chkDownTable_CheckStateChanged(object sender, EventArgs e)
        {
            //テーブル下降収集チェックボックスにチェックが入っている時のみ，テーブル下降収集用のストローク数を編集可にする
            cwneDownTableDistance.Enabled = (chkDownTable.CheckState == CheckState.Checked);
        }

        //*******************************************************************************
        //機　　能： テーブル下降収集指定ストローク変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 指定ストロークがテーブル下限を超える場合は入力をキャンセルする
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cwneDownTableDistance_ValueChanged(object sender, EventArgs e)
        {
            //Rev20.02 ゲイン校正画面の処理に合わせる by長野 2015/06/18
            ////mecainf.udab_pos：現在の昇降絶対位置の取得
            //float cwneDownTableDistanceValue = 0.0f;
            //float.TryParse(cwneDownTableDistance.Text, out cwneDownTableDistanceValue);

            //if (CTSettings.mecainf.Data.udab_pos + cwneDownTableDistanceValue > CTSettings.GValUpperLimit)
            //{
            //    //メッセージ表示：
            //    //   テーブル下限を超えて下降しようとしています！
            //    //   現在の昇降位置(mm): xxx
            //    //   テーブル下限値(mm): xxx
            //    MessageBox.Show(StringTable.GetResString(9583, "\t" + Convert.ToString(CTSettings.mecainf.Data.udab_pos), "\t" + Convert.ToString(CTSettings.GValUpperLimit)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //【C#コントロールで代用】
            bool bnoval = false;    //追加2015/01/28hata
            float downTableDistance = 0.0f;

            //変更2015/01/28hata
            //float.TryParse(cwneDownTableDistance.Text, out downTableDistance);
            if (!float.TryParse(cwneDownTableDistance.Text, out downTableDistance))
                bnoval = true;

            //追加2015/01/28hata
            if (string.IsNullOrEmpty(PreUpDownValText)) PreUpDownValText = "0";
            if (string.IsNullOrEmpty(PreUpDownEnterValText)) PreUpDownEnterValText = "0";

            //変更2015/01/28hata_if文追加
            if (presskye == (char)Keys.Return)
            {
                //mecainf.udab_pos：現在の昇降絶対位置の取得
                //if (modMecainf.mecainf.udab_pos + eventArgs.value > modGlobal.GValUpperLimit)
                if (CTSettings.mecainf.Data.udab_pos + downTableDistance > CTSettings.GValUpperLimit)
                {
                    //メッセージ表示：
                    //   テーブル下限を超えて下降しようとしています！
                    //   現在の昇降位置(mm): xxx
                    //   テーブル下限値(mm): xxx
                    //MessageBox.Show(StringTable.GetResString(9583, "\t" + Convert.ToString(CTSettings.mecainf.Data.udab_pos), "\t" + Convert.ToString(CTSettings.GValUpperLimit)),
                    //Rev23.10 計測CT対応 by長野 2015/10/16
                    MessageBox.Show(StringTable.GetResString(9583, "\t" + Convert.ToString(frmMechaControl.Instance.Udab_Pos), "\t" + Convert.ToString(CTSettings.GValUpperLimit)),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //eventArgs.value = eventArgs.previousValue;

                    //追加2015/01/28hata
                    cwneDownTableDistance.Text = PreUpDownEnterValText;
                    presskye = (char)0;
                    return;
                }
                PreUpDownEnterValText = downTableDistance.ToString();
            }
            else
            {
                if (bnoval)
                {
                    //(.)のチェック
                    //(.)が2つ以上ある場合
                    int pos0 = cwneDownTableDistance.Text.IndexOf(".");
                    int pos1 = cwneDownTableDistance.Text.LastIndexOf(".");
                    if (pos0 != pos1)
                    {
                        //前回値に戻す
                        cwneDownTableDistance.Text = PreUpDownValText;
                    }

                    //(-)のチェック
                    //(-)が間にある場合は(.)を消す
                    //string text = cwneDownTableDistance.Text;
                    //int pos = cwneDownTableDistance.Text.LastIndexOf("-");
                    //if (pos > 0)
                    //{
                    //    //(-)を消す
                    //    string seltext = text.Remove(pos, 1);
                    //    text = seltext;
                    //}
                    //if (!float.TryParse(text, out downTableDistance))
                    //{
                    //    if (text != "") text = PreUpDownValText;
                    //}
                    //cwneDownTableDistance.Text = text;

                }
                PreUpDownValText = cwneDownTableDistance.Text;
            }
            presskye = (char)0;
        }

        //追加2015/01/28hata //Rev20.02 追加 by長野 2015/06/20
        private void cwneDownTableDistance_KeyPress(object sender, KeyPressEventArgs e)
        {
            presskye = e.KeyChar;
            switch (e.KeyChar)
            {
                //数字キーと削除キー
                case (char)Keys.D0:
                case (char)Keys.D1:
                case (char)Keys.D2:
                case (char)Keys.D3:
                case (char)Keys.D4:
                case (char)Keys.D5:
                case (char)Keys.D6:
                case (char)Keys.D7:
                case (char)Keys.D8:
                case (char)Keys.D9:
                case (char)Keys.Back:
                //マイナスキーは含めない
                //case (char)45:  //(-)マイナスキー　

                case (char)46:  //(.)dotキー          
                    break;
                case (char)Keys.Return:
                    cwneDownTableDistance_ValueChanged(sender, EventArgs.Empty);
                    break;
                default:
                    e.KeyChar = (char)0;
                    e.Handled = true;
                    break;
            }
        }

        //追加2015/01/28hata //Rev20.02 追加 by長野 2015/06/20
        private void cwneDownTableDistance_Leave(object sender, EventArgs e)
        {
            float downTableDistance = 0f;
            if (string.IsNullOrEmpty(PreUpDownEnterValText)) PreUpDownEnterValText = "0";
            if (!float.TryParse(cwneDownTableDistance.Text, out downTableDistance))
            {
                if (cwneDownTableDistance.Text == "") cwneDownTableDistance.Text = PreUpDownEnterValText;
            }
            else
            {
                cwneDownTableDistance.Text = downTableDistance.ToString();
            }
            presskye = (char)0;
            cwneDownTableDistance_ValueChanged(sender, EventArgs.Empty);
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
        private void frmVertical_Load(object sender, EventArgs e)
        {
            //実行時はフラグをセット
            modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTScanCorrect;

            //キャプションのセット
            SetCaption();

            //現在のコモン内容を取り出す
            ScanCorrect.OptValueGet_Cor();

            //各コントロールの位置・サイズ等の初期化
            InitControls();

            //表示
            SetControls();

            //ビジーオフ
            IsBusy = false;

            //停止中
            stsVertical.Status = StringTable.GC_STS_STOP;
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
        private void frmVertical_FormClosed(object sender, FormClosedEventArgs e)
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

            //Label5.Caption = LoadResString(12059)                      '垂直線ﾜｲﾔﾋﾟｯﾁ
            Label5.Text = (CTSettings.scaninh.Data.full_distortion == 0 ? CTResources.LoadResString(12053) : CTResources.LoadResString(12059)); //v11.2変更 by 間々田 2005/11/29 穴の間隔/垂直線ﾜｲﾔﾋﾟｯﾁ
   
            //lblMAUni.Caption = LoadResString(10815)                    'μA    'v8.0 管電流追加 by 間々田 2003/12/04
            lblMAUni.Text = modXrayControl.CurrentUni;                  //v11.4変更 by 間々田 2006/03/02

            //Ｘ線外部制御でない場合：「Ｘ線をオンしてください。」を付加
            if (CTSettings.scaninh.Data.xray_remote != 0)
            {
                lblMessage.Text = StringTable.BuildResStr(StringTable.IDS_TurnOn, StringTable.IDS_Xray) + "\r" + lblMessage.Text;
            }

            //v17.60 フォントがバラつき不自然なため削除 by 長野 2011/06/11
            //英語環境の場合、ラベルコントロールに使用するフォントをArialにする
            //    If IsEnglish Then SetLabelFont Me
        }

        //*******************************************************************************
        //機　　能： 各コントロールの位置・サイズ等の初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v11.2  2006/01/17   ????????      新規作成
        //*******************************************************************************
        private void InitControls()
        {
            //Ｘ線外部制御不可の場合、管電流を非表示にする
            fraTubeCurrent.Visible = (CTSettings.scaninh.Data.xray_remote == 0);

            //テーブル下降収集の表示
            fraTableDownAcquire.Visible = (CTSettings.scaninh.Data.table_down_acquire == 0);
        }

        //********************************************************************************
        //機    能  ：  幾何学歪校正の設定値を表示する
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  初期値は下記のように指定する。
        //
        //                積算枚数      = Public変数
        //                垂直線ﾜｲﾔﾋﾟｯﾁ = コモン
        //
        //履    歴  ：  V2.0   00/02/08  (SI1)鈴山       新規作成
        //********************************************************************************
        private void SetControls()
        {
            //積算枚数
            //変更2014/11/28hata_v19.51_dnet
            //数字の初期値は10単位にする
            //cwneSum.Value = modScanCorrect.IntegNumAtVer;
            cwneSum.Value = ScanCorrect.RoundControlVale(modScanCorrect.IntegNumAtVer, cwneSum.Maximum, cwneSum.Minimum, 10F);

            //垂直線ワイヤピッチ
            // 【C#コントロールで代用】
            cwneWireDistance.Text = CTSettings.scancondpar.Data.ver_wire_pitch.ToString();

            //シェーディング補正
            chkShading.CheckState =(CheckState)modScanCorrect.GFlg_Shading_Ver;

            //管電流：Ｘ線制御画面のコントロールをコピー
            //v15.10条件追加 byやまおか 2009/10/29
            if (CTSettings.scaninh.Data.xray_remote == 0)
            {
                modLibrary.CopyCWNumEdit(frmXrayControl.Instance.cwneMA, cwneMA);
            }

            //テーブル下降収集用変数の値を反映させる         added by 間々田 2003-03-03
            chkDownTable.CheckState = modScanCorrect.DownTable;                         //テーブル下降収集の有無
            cwneDownTableDistance.Text = modScanCorrect.DownTableDistance.ToString();   //テーブル下降収集用のストローク数（移動距離）（ｍｍ）
        }
    }
}
