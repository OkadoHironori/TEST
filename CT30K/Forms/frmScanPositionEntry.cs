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
    ///* プログラム名： frmScanPositionEntry.frm                                    */
    ///* 処理概要　　： スキャン位置校正                                            */
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
    ///* v11.2       05/10/19    (SI3)    間々田     ２次元幾何歪補正対応           */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
    ///* ************************************************************************** */
    public partial class frmScanPositionEntry : Form
    {
        #region インスタンスを返すプロパティ

        // frmScanPositionEntryのインスタンス
        private static frmScanPositionEntry _Instance = null;

        /// <summary>
        /// frmScanPositionEntryのインスタンスを返す
        /// </summary>
        public static frmScanPositionEntry Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmScanPositionEntry();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmScanPositionEntry()
        {
            InitializeComponent();
        }

        //bool IsOkMove;          //I.I.移動状態保持用変数（ゲイン校正）       v11.21追加 by 間々田 2006/02/10
        //bool IIChangedAtGain;   //I.I.移動状態保持用変数（スキャン位置校正） v11.21追加 by 間々田 2006/02/10
        //bool IIChangedAtSp;     //I.I.移動状態保持用変数（幾何歪校正）       v11.21追加 by 間々田 2006/02/10
        //bool IIChangedAtVer;    //I.I.移動状態保持用変数（回転中心校正）     v11.21追加 by 間々田 2006/02/10
        //bool IIChangedAtRot;    //I.I.移動状態保持用変数（寸法校正）         v11.21追加 by 間々田 2006/02/10
        //bool IIChangedAtDis;    //昇降位置(mm)
        
        //float iUdab;
        bool myBusy;

        //*************************************************************************************************
        //機　　能： IsBusyプロパティ
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v10.2 2005/08/22 (SI3)間々田    新規作成
        //*************************************************************************************************
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
                optAuto.Enabled = !myBusy;
                optManual.Enabled = !myBusy;

                //マウスポインタの制御
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
        private void cmdEnd_Click(object sender, EventArgs e)
        {
            //フォームをアンロードする
            this.Close();
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
            int ResNum = 0;
            //float fdat1 = 0;
            //float fdat2 = 0;

            //v17.02削除 byやまおか 2010/07/28
            //'v17.00追加　山本 2010-02-06　透視画像表示停止
            //frmTransImage.CaptureOn = False

            //RAMディスクが構築されているかどうか  'v17.40変更 byやまおか 2010/10/26
            //If UseRamDisk And (Not RamDiskIsReady) Then Exit Sub
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //    If UseRamDisk Then      'v17.42修正 byやまおか 2010/11/04
            //        If (Not RamDiskIsReady) Then Exit Sub
            //    End If
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

            //追加2014/10/07hata_v19.51反映
            //メカが動ける(パネルがOFF)かチェック     'v18.00追加 byやまおか 2011/07/24 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
            if ((!modMechaControl.IsOkMechaMove()))
                return;


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

                //実行中の処理に対して停止要求をする     'v17.50上記の処理を関数化 by 間々田 2011/02/17
                modCT30K.CallUserStopSet();

                //Rev20.00 追加 もしキャプチャON中だったらOFFにする by長野 2015/02/16
                if (frmTransImage.Instance.CaptureOn == true)
                {

                    frmTransImage.Instance.CaptureOn = false;

                }

                return;
            }

            //２次元幾何歪補正の場合                                     'v11.2追加 by 間々田 2005/10/11
            //If scaninh.full_distortion = 0 Then
            //ｖ17.00変更　山本 2010-02-06
            if ((CTSettings.scaninh.Data.full_distortion == 0) && (!CTSettings.detectorParam.Use_FlatPanel))
            {
                //幾何歪校正ステータスが準備完了でない場合
                if (!frmScanControl.Instance.IsOkVertical)
                {
                    //メッセージ表示：
                    //   幾何歪校正が準備完了でないため、処理を中止します。
                    //   事前に幾何歪校正を実施してください。
                    MessageBox.Show(StringTable.BuildResStr(StringTable.IDS_CorNotReady, StringTable.IDS_CorDistortion), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }

            //I.I.（またはFPD）電源のチェック  条件追加 by 間々田 2004/12/28
            if (!modSeqComm.PowerSupplyOK())
            {
                return;
            }

            //'シーケンサ通信確認ファイルの値を0クリアする
            //'ClearCommCheck
            //UserStopClear      'v11.5変更 by 間々田 2006/04/14
            //
            //'連続回転コーンビーム＋高速再構成の時は、RAMディスクのscanstopを使う v17.40 追加 by 長野
            //If smooth_rot_cone_flg = True Then
            //
            //    UserStopClear_rmdsk
            //
            //End If

            //停止要求フラグをクリアする             'v17.50上記の処理を関数化 by 間々田 2011/02/17
            modCT30K.CallUserStopClear();

            //自動モードの場合、上昇の警告を出す added by 山本 2003-3-28
            if (optAuto.Checked)
            {
                ScanCorrect.GainCorFlag = 0;
                //added by 山本　2003-11-7　自動校正でゲイン校正をしたかどうかのフラッグを立てる

                //変更2014/10/07hata_v19.51反映
   				//産業用CTモードの場合   'v18.00条件追加 byやまおか 2011/07/24 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
				if ((CTSettings.scaninh.Data.avmode == 0)) {

                    //   試料テーブルが動作しますので、試料の固定を確認してください。
				    //   よろしければＯＫをクリックして下さい。
				    if (MessageBox.Show(CTResources.LoadResString(9479) + "\r\n" +  "\r\n" + CTResources.LoadResString(StringTable.IDS_ClickOK), Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
    			        return;

			    //マイクロCTの場合
				} 
                else 
                {
                    //確認メッセージ表示：
                    //   試料テーブルが上昇しますので、コリメータ／フィルタ等に衝突しないか確認して下さい。  （リソース:9511）
                    //   Ｘ線管が下降しますので、試料テーブル等に衝突しないか確認して下さい。                （リソース:9473）
                    //   よろしければＯＫをクリックして下さい。
                    ResNum = (CTSettings.t20kinf.Data.ud_type == 1 ? 9473 : 9511);
                    //追加 コモンによってメッセージを切り替える by 間々田 2003/10/24
                    string msg = CTResources.LoadResString(ResNum) + "\r\n" + CTResources.LoadResString(StringTable.IDS_ClickOK);
                    if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                    {
                        return;
                    }
				
                }

                //FCD のチェック  'v9.7追加 by added 間々田 2004/12/10
                if (!modSeqComm.CheckFCD(ScanCorrect.GVal_Fcd))
                {
                    return;
                }
            }

            //ビジーフラグセット
            IsBusy = true;

            //スキャン位置校正のために画像を配列に読み込む
            //If GetImageForScanPositionCorrect(lblSts, cwneSum.Value, optAuto.Value) Then
            //v10.0変更 by 間々田 2005/01/31 PCフリーズ対策処理 改良版対応
            //2014/11/07hata キャストの修正
            //if (modScanCorrectNew.GetImageForScanPositionCorrect(stsScanPos, pgbScanPos, (int)cwneSum.Value, optAuto.Checked))
            if (modScanCorrectNew.GetImageForScanPositionCorrect(stsScanPos, pgbScanPos, Convert.ToInt32(cwneSum.Value), optAuto.Checked))
            {
                //フォームを消去
                //変更2015/1/17hata_非表示のときにちらつくため
                //Hide();
                modCT30K.FormHide(this);

                //スキャン位置校正結果フォームを表示する
                //frmScanPositionResult.Dialog
                frmScanPositionResult.Instance.Dialog((optAuto.Checked));  //v11.2変更 by 間々田 2005/12/05

                //フォームをアンロードする
                this.Close();
            }
            else
            {
                //Rev26.00 失敗の場合は[ガイド]タブのスキャンエリア・条件の設定完了フラグを落とす add by chouno 2017/01/16
                frmScanControl.Instance.setScanAreaAndCmpFlg(false);
                
                //「データ収集異常終了」と表示
                stsScanPos.Status = StringTable.GC_STS_CAPT_NG;

                //ビジーオフ
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
        private void frmScanPositionEntry_Load(object sender, EventArgs e)
        {
            //実行時はフラグをセット
            modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTScanCorrect;

            //キャプションのセット
            SetCaption();

            //現在のコモン内容を取り出す
            ScanCorrect.OptValueGet_Cor();

            //積算枚数をセットする
            //変更2014/11/28hata_v19.51_dnet
            //数字の初期値は10単位にする
            //cwneSum.Value = ScanCorrect.IntegNumAtPos;
            cwneSum.Value = ScanCorrect.RoundControlVale(ScanCorrect.IntegNumAtPos, cwneSum.Maximum, cwneSum.Minimum, 10F);

            //モード（自動／手動）フレームの表示・非表示の設定
            fraMode.Visible = (CTSettings.scaninh.Data.scan_posi_entry_auto == 0);

            //デフォルトのモード（True:自動, False:手動）'v11.2追加 by 間々田 2006/01/10
            optAuto.Checked = ScanCorrect.IsAutoAtSpCorrect;

            //ビジーオフ
            IsBusy = false;

            //停止中
            stsScanPos.Status = StringTable.GC_STS_STOP;
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
        private void frmScanPositionEntry_FormClosed(object sender, FormClosedEventArgs e)
        {
            //ビジーオフ
            IsBusy = false;

            //モード（True:自動, False:手動）を記憶  'v11.2追加 by 間々田 2006/01/10
            ScanCorrect.IsAutoAtSpCorrect = optAuto.Checked;

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

            //メッセージ
            if (CTSettings.scaninh.Data.xray_remote == 0)
            {
                //試料テーブルに何も載せないでください。
                //準備ができたらＯＫをクリックしてください。
                lblMessage.Text = CTResources.LoadResString(StringTable.IDS_DontPutOnTable) + "\r" + lblMessage.Text;
            }
            else
            {
                //試料テーブルに、スキャン位置校正用ファントムを載せて、Ｘ線をオンしてください。
                //準備ができたらＯＫをクリックしてください。
                lblMessage.Text = CTResources.LoadResString(12255) + "\r" + lblMessage.Text;
            }

            //v17.60 フォントがバラつき不自然なため削除 by 長野 2011/06/11
            //英語環境の場合、ラベルコントロールに使用するフォントをArialにする
            //If IsEnglish Then SetLabelFont Me
        }
    }
}
