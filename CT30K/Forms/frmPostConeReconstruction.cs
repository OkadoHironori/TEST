using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
//
using CT30K.Common;
using CTAPI;
using TransImage;
//using CT30K.Properties;

namespace CT30K
{
    ///* ************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver10.0              */
    ///* 客先　　　　： ?????? 殿                                                   */
    ///* プログラム名： frmPostConeReconstruction.frm                               */
    ///* 処理概要　　： コーン後構成                                                */
    ///* 注意事項　　： なし                                                        */
    ///* -------------------------------------------------------------------------- */
    ///* 適用計算機　： DOS/V PC                                                    */
    ///* ＯＳ　　　　： Windows XP  (SP2)                                           */
    ///* コンパイラ　： VB 6.0                                                      */
    ///* -------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
    ///*                                                                            */
    ///* V10.00      05/01/24    (ITC) 間々田        新規作成                       */
    ///*                                                                            */
    ///* -------------------------------------------------------------------------- */
    ///* ご注意：                                                                   */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
    ///*                                                                            */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2005                 */
    ///* ************************************************************************** */
    public partial class frmPostConeReconstruction : Form
    {
        #region インスタンスを返すプロパティ

        // frmPostConeReconstructionのインスタンス
        private static frmPostConeReconstruction _Instance = null;

        /// <summary>
        /// frmPostConeReconstructionのインスタンスを返す
        /// </summary>
        public static frmPostConeReconstruction Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmPostConeReconstruction();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmPostConeReconstruction()
        {
            InitializeComponent();
        }

        //処理実行中？
        private bool myBusy = false;

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

                //「実行」ボタンと「停止」ボタンの切り替え
                cmdExe.Text = (myBusy ? CTResources.LoadResString(StringTable.IDS_btnStop) : CTResources.LoadResString(StringTable.IDS_btnExe));

                //各コントロールのEnabledプロパティを制御
                cmdSelect.Enabled = !myBusy;
                cmdDelete.Enabled = (!myBusy) & (lstFile.ListCount > 0);
                cmdCancel.Enabled = !myBusy;

                //マウスポインタの制御
                this.Cursor = (myBusy ? Cursors.AppStarting : Cursors.Default);
            }
        }

        //*******************************************************************************
        //機　　能： コーンビーム生データリストファイルの保存
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v10.0  05/01/24   (SI4)間々田   新規作成
        //*******************************************************************************
        private bool SaveList()
        {
            bool functionReturnValue = false;

            //int fileNo = 0;
            int i = 0;
            string FileName = null;
            int YenPos = 0; //\の位置

            //戻り値初期化
            functionReturnValue = false;

            //ファイルオープン
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(Path.Combine(AppValue.CTTEMP, "post_cone.csv"), false, System.Text.Encoding.GetEncoding("shift-jis"));

                //ヘッダの書き込み：スライス番号,パラメータ名,生データディレクトリ名,生データファイル名
                sw.WriteLine(modLibrary.GetCsvRec(CTResources.LoadResString(StringTable.IDS_Number),
                                                  CTResources.LoadResString(StringTable.IDS_ParameterName),
                                                  CTResources.LoadResString(StringTable.IDS_RawData),
                                                  CTResources.LoadResString(StringTable.IDS_RawData)));
                for (i = 0; i <= lstFile.ListCount - 1; i++)
                {
                    //ファイル名の抽出（フルパス）
                    FileName = lstFile.List(i);

                    //\の位置を取得
                    YenPos = FileName.LastIndexOf("\\") + 1;
                    sw.WriteLine(modLibrary.GetCsvRec((i + 1).ToString(),
                                                      StringTable.FormatStr("post_cone[%1]", i),
                                                      FileName.Substring(0, YenPos),
                                                      FileName.Substring(YenPos)));
                }

                //フッタの書き込み
                sw.WriteLine(modLibrary.GetCsvRec("Slice_num", lstFile.ListCount.ToString()));

                //戻り値セット
                functionReturnValue = true;
            }
            catch (Exception e)
            {
                //エラーメッセージの表示
                MessageBox.Show(e.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                //ファイルクローズ
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
            
            return functionReturnValue;
        }

        //*******************************************************************************
        //機　　能： キャンセルボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v10.0  05/01/24   (SI4)間々田   新規作成
        //*******************************************************************************
        private void cmdCancel_Click(object eventSender, EventArgs e)
        {
            //フォームをアンロード
            this.Close();
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
        //履　　歴： v10.0  05/01/24   (SI4)間々田   新規作成
        //*******************************************************************************
        private void cmdExe_Click(object sender, EventArgs e)
        {
            //処理実行中の場合
            if (IsBusy)
            {
                //UserStopSet
                //
                //'連続回転コーンビーム＋高速再構成の時は、RAMディスクのscanstopを使う v17.40 追加 by 長野
                //If smooth_rot_cone_flg = True Then
                //
                //    UserStopSet_rmdsk
                //
                //End If

                //実行中の処理に対して停止要求をする     'v17.50上記の処理を関数化 by 間々田 2011/02/17
                modCT30K.CallUserStopSet();
                return;
            }

            //リストに１つもコーンビームの生データが入力されていない場合
            if (!(lstFile.ListCount > 0))
            {
                //メッセージを表示：
                //   コーンビームの生データが１つも指定されていません。
                //   コーンビームの生データを指定してから再度実行してください。
                MessageBox.Show(CTResources.LoadResString(9332), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //実行ファイルのチェック
            if (!File.Exists(AppValue.CONERECON))
            {
                //メッセージ表示：～が見つかりません。
                MessageBox.Show(StringTable.GetResString(StringTable.IDS_NotFound, AppValue.CONERECON), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //リストの内容をファイルに保存する
            if (!SaveList())
            {
                return;
            }

            //ビジーフラグセット
            IsBusy = true;

            //スキャン条件の一時保存
            modCommon.BackupScansel();

            //動作モードの設定
            //    構造体名：scansel
            //    コモン名：operation_mode
            //Call putcommon_long("scansel", "operation_mode", 8)
            CTSettings.scansel.Data.operation_mode = (int)ScanSel.OperationModeConstants.OP_POST_CONE;

            //スキャン条件画面のオートズームフラッグをオフする   'v14.0追加 by 間々田 2007/08/08
            CTSettings.scansel.Data.auto_zoomflag = 0;

            //modScansel.PutScansel(ref modScansel.scansel);
            CTSettings.scansel.Write();

            //フォームが乱れることがあるのでここでリフレッシュ   'v17.50追加 by 間々田 2011/02/15
            this.Refresh();

            //子プロセスの起動   'v11.5追加 by 間々田 2006/06/22
            modCT30K.StartProcess(AppValue.CONERECON);

            //フォームを非表示にする
            //Me.hide                    'v15.0非表示にしない by 間々田 2009/06/16
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
        //履　　歴： v10.0  05/01/24   (SI4)間々田   新規作成
        //*******************************************************************************
        private void frmPostConeReconstruction_Load(object sender, EventArgs e)
        {
            //フラグをセット
            modCTBusy.CTBusy = modCTBusy.CTBusy | modCTBusy.CTReconstruct;

            //キャプションのセット
            SetCaption();

            //各コントロールの初期化
            InitControls();

            //ビジーフラグ初期化
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
        //履　　歴： v10.0  05/01/24   (SI4)間々田   新規作成
        //*******************************************************************************
        private void frmPostConeReconstruction_FormClosed(object sender, FormClosedEventArgs e)
        {
            //終了時はフラグをリセット
            modCTBusy.CTBusy = modCTBusy.CTBusy & (~modCTBusy.CTReconstruct);
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
        //履　　歴： v10.0  05/01/24   (SI4)間々田   新規作成
        //*******************************************************************************
        private void SetCaption()
        {
            //Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
            StringTable.LoadResStrings(this);

            lblHeader.Text = StringTable.LoadResStringWithColon(StringTable.IDS_RawDataName);   //生データ名：
        }

        //*******************************************************************************
        //機　　能： 各コントロールの初期化
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v15.00  2008/12/01 (SS1)間々田  リニューアル
        //*******************************************************************************
        private void InitControls()
        {
            //リストコントロールボックス

            //項目削除用のボタンの設定
            lstFile.DeleteButton = cmdDelete;

            //リンクする「参照」ボタンの設定
            lstFile.ReferenceButton = cmdSelect;

            //このリストの内容
            lstFile.Description = CTResources.LoadResString(StringTable.IDS_ConeRawFile); //コーンビームＣＴ用生データファイル
        }
    }
}