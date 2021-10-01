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
    ///* プログラム名： frmVerticalResult.frm                                       */
    ///* 処理概要　　： 幾何歪校正結果                                              */
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
    public partial class frmVerticalResult : Form
    {
        #region インスタンスを返すプロパティ

        // frmVerticalResultのインスタンス
        private static frmVerticalResult _Instance = null;

        /// <summary>
        /// frmVerticalResultのインスタンスを返す
        /// </summary>
        public static frmVerticalResult Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmVerticalResult();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmVerticalResult()
        {
            InitializeComponent();
        }

        //********************************************************************************
        //  共通データ宣言
        //********************************************************************************
		bool OK;            //入力結果
		short SliceNo;      //マルチスライスの番号

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
		private void cmdOK_Click(object eventSender, EventArgs e)
		{
			//Scancondpar（コモン）の更新
			if (!UpdateScancondpar())
            {
				//MsgBox "保存しようとするパラメータ値が不適切なため、処理を中止します。", vbCritical
                MessageBox.Show(CTResources.LoadResString(20061), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);//ストリングテーブル化 'v17.60 by長野 2011/05/22
				return;
			}

			//押したボタンの種類を通知
			OK = true;

			//幾何歪校正パラメータのコモンへの書き込み
			//Call Set_Vertical_Parameter(TargetSliceNo)

			//v11.2以下に変更ここから by 間々田 2005/10/18 Set_Vertical_Parameterの同等処理

			//メカの移動有無プロパティをリセットする
			modSeqComm.SeqBitWrite("VerIIChangeReset", true);

			//回転中心校正関連のメカ移動プロパティを移動ありとする
			modSeqComm.SeqBitWrite("RotXChangeSet", true);
			modSeqComm.SeqBitWrite("RotYChangeSet", true);
			modSeqComm.SeqBitWrite("RotIIChangeSet", true);

			//フル2次元幾何歪補正時はスキャン位置校正関連のメカ移動プロパティを移動ありとする
			//If scaninh.full_distortion = 0 Then
			//変更 by 間々田 2006/02/07 マルチスライス校正時は移動ありとしない
            if ((CTSettings.scaninh.Data.full_distortion == 0) && (!ScanCorrect.MultiSliceMode))
            {
				modSeqComm.SeqBitWrite("SPIIChangeSet", true);
			}

			//mecainf（コモン）の更新
			UpdateMecainf();

			//コーンビームスキャンが可能な場合
            if (CTSettings.scaninh.Data.data_mode[2] == 0)
            {
                //幾何歪テーブルをアスキー形式でファイルに保存する
                //Rev20.00 1次元歪補正は、もうやらないため不要 by長野 2014/11/10 
                //ScanCorrect.WriteHizumi();

				//２次元幾何歪補正の場合，穴の座標 gx, gy, gg, gh をファイルに保存
                if (CTSettings.scaninh.Data.full_distortion == 0)
                {
                    ScanCorrect.WriteHole();
                }
			}

			//v11.2以下に変更ここまで by 間々田 2005/10/18

			//幾何歪校正画像の保存
			//added by 山本　2005-12-17
			if (ScanCorrect.MultiSliceMode == false)
            {
                ScanCorrect.ImageSave(ref ScanCorrect.VRT_IMAGE[0], ScanCorrect.VERT_CORRECT, CTSettings.detectorParam.h_size, CTSettings.detectorParam.v_size);
			}

			//スキャンエリアを更新するためにスキャン条件設定
			frmScanCondition.Instance.Setup();    //v17.61追加 byやまおか 2011/06/29

			//フォームを消去
			//変更2015/1/17hata_非表示のときにちらつくため
            //this.Hide();
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
		private void frmVerticalResult_Load(object sender, EventArgs e)
		{
			//キャプションのセット
			SetCaption();

            int TextRight = lblVerticalWire.Right;

			//フォームにパラメータをセットする
			//lblVerticalWire.Caption = CStr(VRT_Count)
            lblVerticalWire.Text = (CTSettings.scaninh.Data.full_distortion == 0 ? Convert.ToString(ScanCorrect.kmax) : Convert.ToString(ScanCorrect.VRT_Count));    //v11.2変更 by 山本 2005-12-21
            lblVerticalWire.Left = lblVerticalWire.Left - (lblVerticalWire.Right - TextRight);
                      
            lblDetectorPitch.Text = ScanCorrect.Mdt_Pitch[SliceNo].ToString("0.000000");
            lblDetectorPitch.Left = lblDetectorPitch.Left - (lblDetectorPitch.Right - TextRight);
            
            lblA0.Text = ScanCorrect.a0[SliceNo].ToString("0.000000");
            lblA0.Left = lblA0.Left - (lblA0.Right - TextRight);
            
            lblA1.Text = ScanCorrect.A1[SliceNo].ToString("0.000000");
            lblA1.Left = lblA1.Left - (lblA1.Right - TextRight);
			            
            lblA2.Text = ScanCorrect.a2[SliceNo].ToString("0.000000");
            lblA2.Left = lblA2.Left - (lblA2.Right - TextRight);

            lblA3.Text = ScanCorrect.a3[SliceNo].ToString("0.000000");
            lblA3.Left = lblA3.Left - (lblA3.Right - TextRight);
            
            lblA4.Text = ScanCorrect.a4[SliceNo].ToString("0.000000");
            lblA4.Left = lblA4.Left - (lblA4.Right - TextRight);
            
            lblA5.Text = ScanCorrect.a5[SliceNo].ToString("0.000000");
            lblA5.Left = lblA5.Left - (lblA5.Right - TextRight);
            
            lblXls.Text = Convert.ToString(ScanCorrect.xls[SliceNo]);
            lblXls.Left = lblXls.Left - (lblXls.Right - TextRight);
            
            lblXle.Text = Convert.ToString(ScanCorrect.xle[SliceNo]);
            lblXle.Left = lblXle.Left - (lblXle.Right - TextRight);

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

			this.Text = StringTable.BuildResStr(StringTable.IDS_Result, StringTable.IDS_CorDistortion); //幾何歪校正結果

			//Label2.Caption = LoadResString(10821)                  '本
            Label2.Text = (CTSettings.scaninh.Data.full_distortion == 0 ? CTResources.LoadResString(10801) : CTResources.LoadResString(10821));  //v11.2変更 by 山本 2005-12-21 本/個
			//Label3.Caption = LoadResString(12337)                                  '検出ワイヤ
            Label3.Text = (CTSettings.scaninh.Data.full_distortion == 0 ? CTResources.LoadResString(12339) : CTResources.LoadResString(12337));  //v11.2変更 by 山本 2005-12-21 検出ワイヤ/検出穴
		}

        //*******************************************************************************
        //機　　能： モーダルダイアログ処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： strInfo         [I/ ] String    フォームのキャプションに付加する文字列
        //           Pos             [I/ ] Integer   スライス位置
        //戻 り 値：                 [ /O] Boolean  「はい」がクリックされた場合  : True
        //                                          「いいえ」がクリックされた場合: False
        //補　　足：
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        //Public Function Dialog(Optional ByVal strInfo As String = "") As Boolean
		public bool Dialog(string strInfo = "", int pos = 2)  //v11.2変更 by 間々田 2005/10/18
		{
			bool functionReturnValue = false;

			//戻り値用変数の初期化
			OK = false;

			//スライス位置
			SliceNo = (short)pos;  //v11.2追加 by 間々田 2005/10/18

			//フォームのキャプションに情報を加える
            if (!string.IsNullOrEmpty(strInfo))
            {
                this.Text = this.Text + " - " + strInfo;
            }

			//モーダルでフォームを表示
            //変更2014/12/22hata_dNet_オーナーフォームを指定する
            //ShowDialog();
            this.ShowDialog(frmCTMenu.Instance);

			//戻り値のセット
			functionReturnValue = OK;

			//フォームをアンロード
			this.Close();
			return functionReturnValue;
		}

        //*******************************************************************************
        //機　　能： scancondpar（コモン）の更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 幾何歪校正関連のパラメータのみ更新
        //
        //履　　歴： v11.2  05/10/17  (SI3)間々田    新規作成
        //*******************************************************************************
		private bool UpdateScancondpar()
		{
			bool functionReturnValue = false;

			//最大ファン角を求める為の変数       'v15.03追加 byやまおか 2009/11/19
			int h = 0;
			int v = 0;
			float kv = 0;
			float bb0 = 0;
			float FGD = 0;
			float ic = 0;
			float jc = 0;
			float Dpi = 0;
			float e_dash = 0;
			//float r = 0;
			//float mm = 0;
			//float Delta_Ip = 0;
			float Ils = 0;
			float Ile = 0;

			//チェック
			if (SliceNo == 2)
            {
                if (ScanCorrect.GVal_mdtpitch[SliceNo] == 0)
                {
                    return functionReturnValue;
                }
                if (ScanCorrect.A1[SliceNo] == 0)
                {
                    return functionReturnValue;
                }
			}

            CTSettings.scancondpar.Data.mdtpitch[SliceNo] = ScanCorrect.GVal_mdtpitch[SliceNo];      //検出器ピッチ (mm/画素)

			//幾何歪校正係数(A0～A5)
            //CTSettings.scancondpar.Data.a[0, SliceNo] = Convert.ToSingle(ScanCorrect.a0[SliceNo]);   //scancondpar.a[SliceNo, 0]のこと
            //CTSettings.scancondpar.Data.a[1, SliceNo] = Convert.ToSingle(ScanCorrect.A1[SliceNo]);   //scancondpar.a[SliceNo, 1]のこと
            //CTSettings.scancondpar.Data.a[2, SliceNo] = Convert.ToSingle(ScanCorrect.a2[SliceNo]);   //scancondpar.a[SliceNo, 2]のこと
            //CTSettings.scancondpar.Data.a[3, SliceNo] = Convert.ToSingle(ScanCorrect.a3[SliceNo]);   //scancondpar.a[SliceNo, 3]のこと
            //CTSettings.scancondpar.Data.a[4, SliceNo] = Convert.ToSingle(ScanCorrect.a4[SliceNo]);   //scancondpar.a[SliceNo, 4]のこと
            //CTSettings.scancondpar.Data.a[5, SliceNo] = Convert.ToSingle(ScanCorrect.a5[SliceNo]);   //scancondpar.a[SliceNo, 5]のこと
            CTSettings.scancondpar.Data.a[SliceNo * 6 + 0] = Convert.ToSingle(ScanCorrect.a0[SliceNo]);   //scancondpar.a[SliceNo, 0]のこと
            CTSettings.scancondpar.Data.a[SliceNo * 6 + 1] = Convert.ToSingle(ScanCorrect.A1[SliceNo]);   //scancondpar.a[SliceNo, 1]のこと
            CTSettings.scancondpar.Data.a[SliceNo * 6 + 2] = Convert.ToSingle(ScanCorrect.a2[SliceNo]);   //scancondpar.a[SliceNo, 2]のこと
            CTSettings.scancondpar.Data.a[SliceNo * 6 + 3] = Convert.ToSingle(ScanCorrect.a3[SliceNo]);   //scancondpar.a[SliceNo, 3]のこと
            CTSettings.scancondpar.Data.a[SliceNo * 6 + 4] = Convert.ToSingle(ScanCorrect.a4[SliceNo]);   //scancondpar.a[SliceNo, 4]のこと
            CTSettings.scancondpar.Data.a[SliceNo * 6 + 5] = Convert.ToSingle(ScanCorrect.a5[SliceNo]);   //scancondpar.a[SliceNo, 5]のこと
            
            CTSettings.scancondpar.Data.xls[SliceNo] = ScanCorrect.xls[SliceNo];                     //有効データ開始画素
            CTSettings.scancondpar.Data.xle[SliceNo] = ScanCorrect.xle[SliceNo];                     //有効データ終了画素
            CTSettings.scancondpar.Data.max_mfanangle[SliceNo] = ScanCorrect.theta0Max[SliceNo];     //最大ファン角

			//複数スライスのとき、それぞれの傾きと切片を書込む
			if (ScanCorrect.MultiSliceMode)
            {
				//傾き
                CTSettings.scancondpar.Data.scan_posi_a[0] = ScanCorrect.GVal_ScanPosiA[0];
                CTSettings.scancondpar.Data.scan_posi_a[1] = ScanCorrect.GVal_ScanPosiA[1];
                CTSettings.scancondpar.Data.scan_posi_a[2] = ScanCorrect.GVal_ScanPosiA[2];
                CTSettings.scancondpar.Data.scan_posi_a[3] = ScanCorrect.GVal_ScanPosiA[3];
                CTSettings.scancondpar.Data.scan_posi_a[4] = ScanCorrect.GVal_ScanPosiA[4];

				//切片
                CTSettings.scancondpar.Data.scan_posi_b[0] = ScanCorrect.GVal_ScanPosiB[0];
                CTSettings.scancondpar.Data.scan_posi_b[1] = ScanCorrect.GVal_ScanPosiB[1];
                CTSettings.scancondpar.Data.scan_posi_b[2] = ScanCorrect.GVal_ScanPosiB[2];
                CTSettings.scancondpar.Data.scan_posi_b[3] = ScanCorrect.GVal_ScanPosiB[3];
                CTSettings.scancondpar.Data.scan_posi_b[4] = ScanCorrect.GVal_ScanPosiB[4];
			}

			//コーンビームスキャンが可能な場合，コーンビーム用のパラメータをコモンに書き込む
            if (CTSettings.scaninh.Data.data_mode[2] == 0)
            {
				//２次元幾何歪補正の場合
                if (CTSettings.scaninh.Data.full_distortion == 0)
                {
                    CTSettings.scancondpar.Data.scan_posi_a[2] = (float)ScanCorrect.b0_bar[2];
                    //2014/11/07hata キャストの修正
                    //CTSettings.scancondpar.Data.scan_posi_b[2] = (float)(ScanCorrect.a0_bar[2] + ScanCorrect.b0_bar[2] * (CTSettings.detectorParam.h_size - 1) / 2 - (CTSettings.detectorParam.v_size - 1) / 2);    //changed by 山本　2005-12-6
                    CTSettings.scancondpar.Data.scan_posi_b[2] = (float)(ScanCorrect.a0_bar[2] + ScanCorrect.b0_bar[2] * (CTSettings.detectorParam.h_size - 1) / 2F - (CTSettings.detectorParam.v_size - 1) / 2F);    //changed by 山本　2005-12-6

                    CTSettings.scancondpar.Data.b[1] = (float)CTSettings.scancondpar.Data.alk[1];
                    ScanCorrect.B1 = CTSettings.scancondpar.Data.alk[1];                 //グローバル変数にも書き込む
				}
                else
                {
					//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
					//                'コーンビーム用幾何歪校正係数(B0～B5)
					//                .b(0) = CSng(b0)
					//                .b(1) = CSng(B1)
					//                .b(2) = CSng(B2)
					//                .b(3) = CSng(B3)
					//                .b(4) = CSng(B4)
					//                .b(5) = CSng(B5)
					//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
				}

                CTSettings.scancondpar.Data.dpm = Convert.ToSingle(ScanCorrect.dpm); //m方向ﾃﾞｰﾀﾋﾟｯﾁ(mm)

				//v15.03追加（ここから） byやまおか 2009/11/19
				//コモン初期化後、正しいコーン再構成エリアが設定できていなかった

				//最大ファン角の計算
                h = CTSettings.detectorParam.h_size;
                v = CTSettings.detectorParam.v_size;
                kv = CTSettings.detectorParam.vm / CTSettings.detectorParam.hm;
				bb0 = ScanCorrect.GVal_ScanPosiA[2];
				FGD = ScanCorrect.GVal_Fid + CTSettings.scancondpar.Data.fid_offset[modCT30K.GetFcdOffsetIndex()];

                //2014/11/07hata キャストの修正
                //ic = (CTSettings.detectorParam.h_size - 1) / 2;
                //jc = (CTSettings.detectorParam.v_size - 1) / 2;
                ic = (CTSettings.detectorParam.h_size - 1) / 2F;
                jc = (CTSettings.detectorParam.v_size - 1) / 2F;

                Dpi = (float)(10 / ScanCorrect.B1);
				//ノーマルスキャン時は B1 の代わりに A1(iSCN)を使用する
				e_dash = (float)Math.Atan(kv * bb0);

				//２次元幾何歪の場合
                if (CTSettings.scaninh.Data.full_distortion == 0)
                {
					Ils = CTSettings.scancondpar.Data.ist + 2;
					Ile = CTSettings.scancondpar.Data.ied - 2;
				}
                else
                {
					//v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
					//                r = Sqr(ic ^ 2 + (kv * jc) ^ 2)
					//                mm = Int(2 * r)
					//                Delta_Ip = Int(ic * hizumi(mm) + 2 + jc * kv * kv * Abs(bb0))
					//                Ils = Delta_Ip
					//                Ile = h - Delta_Ip - 1
					//v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
				}

                ScanCorrect.theta0MaxCone = (float)(2 * Math.Atan((Ile - Ils) * Dpi / Math.Cos(e_dash) * 1.02 * 0.5 / FGD));
				//v15.03追加（ここまで） byやまおか 2009/11/19

                CTSettings.scancondpar.Data.cone_max_mfanangle = ScanCorrect.theta0MaxCone;  //最大ファン角（コーンビーム用）
			}

			//Scancondpar（コモン）の書き込み
			//modScancondpar.CallPutScancondpar();
            CTSettings.scancondpar.Write();

			functionReturnValue = true;
			return functionReturnValue;
		}

        //*******************************************************************************
        //機　　能： mecainf（コモン）の更新
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： 幾何歪校正関連のパラメータのみ更新
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

			//I.I.視野
            theMecainf.Data.ver_iifield = modSeqComm.GetIINo();

			//Ｘ線管
            theMecainf.Data.ver_mt = CTSettings.scansel.Data.multi_tube;

			//ビニングモード
            theMecainf.Data.ver_bin = CTSettings.scansel.Data.binning;    //0:１×１，1:２×２，2:４×４

			//回転中心校正ステータスコモンを０クリアする
            theMecainf.Data.normal_rc_cor = 0;
            theMecainf.Data.cone_rc_cor = 0;
            theMecainf.Data.rc_kv = 0;
            theMecainf.Data.rc_udab_pos = 0;
            theMecainf.Data.rc_iifield = -1;
            theMecainf.Data.rc_mt = -1;
            theMecainf.Data.rc_bin = -1;

			//フル2次元幾何歪補正時はスキャン位置校正ステータスコモンを０クリアする
			//If scaninh.full_distortion = 0 Then
			//変更 by 間々田 2006/02/07 マルチスライス校正時は０クリアしない
            if ((CTSettings.scaninh.Data.full_distortion == 0) && (!ScanCorrect.MultiSliceMode))
            {
                theMecainf.Data.sp_iifield = -1;
                theMecainf.Data.sp_mt = -1;
                theMecainf.Data.sp_bin = -1;
			}

			//mecainf（コモン）更新
			//modMecainf.PutMecainf(ref theMecainf);
            theMecainf.Write();


			//回転中心校正ステータスの更新
			frmCorrectionStatus.Instance.UpdateRCStatus();
		}
    }
}
