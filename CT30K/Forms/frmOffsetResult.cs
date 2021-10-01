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
    ///* プログラム名： frmOffsetResult.frm                                          */
    ///* 処理概要　　： オフセット校正結果                                           */
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
    ///* V7.0        03/08/01    (SI4)間々田   　    リソース対応                    */
    ///* V9.7        04/11/01    (SI4)間々田         収集停止対応                    */
    ///*                                                                             */
    ///* --------------------------------------------------------------------------- */
    ///* ご注意：                                                                    */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。    */
    ///*                                                                             */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2004                  */
    ///* *************************************************************************** */
    public partial class frmOffsetResult : Form
    {
        #region インスタンスを返すプロパティ

        // frmOffsetResultのインスタンス
        private static frmOffsetResult _Instance = null;

        /// <summary>
        /// frmOffsetResultのインスタンスを返す
        /// </summary>
        public static frmOffsetResult Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmOffsetResult();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmOffsetResult()
        {
            InitializeComponent();
        }

		private bool OK;

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
            int ret = 0;

            //フォームを非表示にする
            //変更2015/1/17hata_非表示のときにちらつくため
            //Hide();
            modCT30K.FormHide(this);

			//パーキンエルマーFPDかつ自動校正中の場合はオフセット校正画像とゲイン校正画像をファイルからプリロードする（自動校正中にオフセット校正画像、ゲイン校正画像をプリロードしたためキャンセルした場合は元に戻す） v17.00追加 by　山本 2010-03-06
			//If AutoCorFlag = 1 And DetType = DetTypePke Then    '
			//v17.02修正 自動じゃない場合もプリロードする byやまおか 2010/07/06
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))
            {
				//元のオフセット校正画像をプリロードする
                ret = ScanCorrect.PkeSetOffsetData((int)Pulsar.hPke, 0, ref ScanCorrect.OFFSET_IMAGE[0], 0);
				//If ret = 1 Then MsgBox "オフセット校正データをセットできませんでした。", vbCritical
				if (ret == 1)
                {
                    //v17.60 ストリングテーブル化 by長野 2011/05/25
                    MessageBox.Show(CTResources.LoadResString(20004), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }		
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
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		private void cmdOk_Click(object sender, EventArgs e)
		{
            int ret = 0;
            //マウスポインタを砂時計にする
			this.Cursor = Cursors.WaitCursor;

			//オフセット校正画像の保存
			ScanCorrect.DoubleImageSave(ref ScanCorrect.OFFSET_IMAGE[0], ScanCorrect.OFF_CORRECT, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
#if !(NoCamera)
			//v17.00追加 byやまおか 2010/02/15
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke))
            {
				//新しい校正画像をプリロードする 2010-01-06　山本
				if (ScanCorrect.AutoCorFlag == 0)   //手動校正中はプリロードする
                {
                    ret = ScanCorrect.PkeSetOffsetData((int)Pulsar.hPke, 1, ref ScanCorrect.OFFSET_IMAGE[0], 1);
				}
                else	//自動校正中はプリロードしない
                {
                    ret = ScanCorrect.PkeSetOffsetData((int)Pulsar.hPke, 1, ref ScanCorrect.OFFSET_IMAGE[0], 0);
				}
				if (ret == 1)
                {
					//MsgBox "オフセット校正データをセットできませんでした。", vbCritical
                    MessageBox.Show(CTResources.LoadResString(20004), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);   //ストリングテーブル化 '17.60 by長野 2011/05/22
				}
			}
#endif
			//オフセット校正パラメータをコモンに書き込む
			//Call Set_Offset_Parameter      'v11.2削除 by 間々田 2005/10/17

			//mecainf（コモン）の更新
			UpdateMecainf();    //v11.2追加 by 間々田 2005/10/17　上記 Set_Offset_Parameter と同じ処理

			//戻り値
			OK = true;

			//フォームを非表示にする
            //変更2015/1/17hata_非表示のときにちらつくため
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
		private void frmOffsetResult_Load(object sender, EventArgs e)
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
			StringTable.LoadResStrings( this);

			this.Text = StringTable.BuildResStr(StringTable.IDS_Result, StringTable.IDS_CorOffset); //オフセット校正結果
                                                                                                    //オフセット校正画像結果を保存しますか？
		}

        //*******************************************************************************
        //機　　能： ダイアログ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値：                 [ /O] Boolean   True:「はい」・False:「いいえ」
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		public bool Dialog()
		{
			bool functionReturnValue = false;

			//戻り値の初期化
			OK = false;
			
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

        //*******************************************************************************
        //機　　能： mecainf（コモン）の更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： オフセット校正関連のパラメータのみ更新する
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

			//オフセット校正を行った時の年月日
            theMecainf.Data.off_date = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));   //YYYYMMDD形式

			//オフセット校正を行った時のビニングモード
            theMecainf.Data.off_bin = CTSettings.scansel.Data.binning;                            //0:１×１，1:２×２，2:４×４

			//オフセット校正を行ったときのFPDゲイン      'v17.00追加 byやまおか 2010/02/22
            theMecainf.Data.off_fpd_gain = CTSettings.scansel.Data.fpd_gain;

			//オフセット校正を行ったときのFPD積分時間    'v17.00追加 byやまおか 2010/02/22
            theMecainf.Data.off_fpd_integ = CTSettings.scansel.Data.fpd_integ;

			//オフセット校正を行ったときの時間           'v17.00追加 byやまおか 2010/03/04
            theMecainf.Data.off_time = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));     //HHMMSS形式

			//mecainf（コモン）更新
			//modMecainf.PutMecainf(ref theMecainf);
            theMecainf.Write();

		}
    }
}
